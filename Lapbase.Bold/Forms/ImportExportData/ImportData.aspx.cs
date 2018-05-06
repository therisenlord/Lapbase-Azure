using System;
using System.Transactions;
using System.Diagnostics;
using System.Configuration;
using System.Collections;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.Web.UI;
using Telerik.WebControls;
using NHapi.Base.Model;
using NHapi.Base.Parser;
using NHapi.Base;
using NHapi.Model.V23;
using NHapi.Model.V23.Message;
using NHapi.Model.V23.Segment;
using System.Text.RegularExpressions;
using iTextSharp.text;
using System.IO;
using System.Text;

public partial class Forms_ImportExportData_ImportData : System.Web.UI.Page
{

    //todo:
    //need to escape " on mdbpath
    //update records if its already exist before

    GlobalClass gClass = new GlobalClass();

    #region protected void Page_Load(object sender, EventArgs e)
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "no-cache");
        Response.Expires = -1;
        
        string strScript = String.Empty;

        try
        {
            Page.Culture = Request.Cookies["CultureInfo"].Value;
            gClass.LanguageCode = Request.Cookies["LanguageCode"].Value;
            gClass.OrganizationCode = Request.Cookies["OrganizationCode"].Value;
            if (gClass.IsUserLogoned(Session.SessionID, Request.Cookies["UserPracticeCode"].Value, Request.Url.Host))
            {
                if (!IsPostBack)
                {
                    gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value,
                                            Request.Cookies["Logon_UserName"].Value,
                                            Request.Url.Host,
                                            "Import File Form", 2, "Browse", "", "");
                }
            }
            else
            {
                gClass.ReturnToLoginPage(Request.Url.Host, Request.Cookies["LanguageCode"].Value, Response);
            }
        }
        catch (Exception err)
        {
            string strLanguageCode;
            try
            {
                strLanguageCode = Request.Cookies["LanguageCode"].Value;
                gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Import/Export data form", "Loading Patient List (Page_Load function)", err.ToString());
            }
            catch { strLanguageCode = "en-US"; }
            gClass.ReturnToLoginPage(Request.Url.Host, strLanguageCode, Response);
        }
        return;
    }
    #endregion

    #region  static UppercaseFirst(String s)
    static string UppercaseFirst(String s)
    {
        // Check for empty string.
        if (string.IsNullOrEmpty(s))
        {
            return string.Empty;
        }
        // Return char and concat substring.
        return char.ToUpper(s[0]) + s.Substring(1);
    }
    #endregion

    #region private string GetFilePath
    private string GetFilePath(string fileName)
    {
        int intLastIndex = fileName.LastIndexOf("\\");
        if (intLastIndex == 0)
            intLastIndex = fileName.LastIndexOf("/");

        String now = DateTime.Now.ToString("yyyyMMddHHmmssffff");

        fileName = fileName.Substring(intLastIndex + 1);
        return System.IO.Path.Combine(GetDocumentPath(), gClass.OrganizationCode + "_" + now + "_" + fileName);
    }
    #endregion

    #region private void GetDocumentPath()
    private string GetDocumentPath()
    {
        string uploadFolder, strFolder = "";
        System.IO.DirectoryInfo di;
        strFolder = "Documents";

        uploadFolder = System.IO.Path.Combine(this.Context.ApplicationInstance.Request.PhysicalApplicationPath, strFolder);

        di = new System.IO.DirectoryInfo(uploadFolder);
        if (di.Exists == false) // Create the directory only if it does not already exist.
            di.Create();
        return (uploadFolder);
    }
    #endregion

    #region protected string GetRefDrId(string refDrSurname, string refDrFirstname, string refDrAddress, string refDrCity, string refDrState, string refDrPostcode, string refDrPhone, string refDrFax)
    protected string GetRefDrId(string refDrSurname, string refDrFirstname, string refDrAddress, string refDrCity, string refDrState, string refDrPostcode, string refDrPhone, string refDrFax)
    {
        //check if ref dr exist by using surname, firstname and city(suburb)
        //  if exist, skip and use the first ref id found as the refering doctor for the patient
        //  if not exist, add the ref doctor
        //      get the ref id by using the same sql
        //      insert patient

        SqlCommand cmdSelectRefDr = new SqlCommand();
        SqlCommand cmdSaveRefDr = new SqlCommand();
        string refdrid = "";

        try
        {
            //checkin, inserting ref dr 1
            if (refDrSurname.Trim() != "" && refDrFirstname.Trim() != "" && refDrCity.Trim() != "")
            {
                DataSet dsRefDr = new DataSet();
                gClass.MakeStoreProcedureName(ref cmdSelectRefDr, "sp_RefDoctors_SelectRefDoctorByNameAndCity", true);
                cmdSelectRefDr.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                cmdSelectRefDr.Parameters.Add("@Surname", System.Data.SqlDbType.VarChar, 50).Value = refDrSurname;
                cmdSelectRefDr.Parameters.Add("@Firstname", System.Data.SqlDbType.VarChar, 50).Value = refDrFirstname;
                cmdSelectRefDr.Parameters.Add("@City", System.Data.SqlDbType.VarChar, 50).Value = refDrCity;

                dsRefDr = gClass.FetchData(cmdSelectRefDr, "tblRefDr");
                if (dsRefDr.Tables[0].Rows.Count > 0)
                {
                    //grab the ref id
                    refdrid = dsRefDr.Tables[0].Rows[0]["RefDrId"].ToString();
                }
                else
                {
                    //add ref dr
                    String strRefSurName = refDrSurname.Trim().Replace("'", "`");

                    gClass.MakeStoreProcedureName(ref cmdSaveRefDr, "sp_RefDoctors_SaveData", false);
                    cmdSaveRefDr.Parameters.Add("@RefDrID_Old", SqlDbType.VarChar, 10).Value = "";
                    cmdSaveRefDr.Parameters.Add("@RefDrID", SqlDbType.VarChar, 10).Value = ((strRefSurName.Length > 4) ? strRefSurName.Substring(0, 4) : strRefSurName) + refDrFirstname.Trim().Replace("'", "`").Substring(0, 1);
                    cmdSaveRefDr.Parameters.Add("@Surname", SqlDbType.VarChar, 50).Value = refDrSurname.Trim().Replace("'", "`");
                    cmdSaveRefDr.Parameters.Add("@FirstName", SqlDbType.VarChar, 50).Value = refDrFirstname.Trim().Replace("'", "`");
                    cmdSaveRefDr.Parameters.Add("@Title", SqlDbType.VarChar, 15).Value = "";
                    cmdSaveRefDr.Parameters.Add("@UseFirst", SqlDbType.Bit).Value = false;
                    cmdSaveRefDr.Parameters.Add("@Address1", SqlDbType.VarChar, 50).Value = refDrAddress.Trim().Replace("'", "`");
                    cmdSaveRefDr.Parameters.Add("@Address2", SqlDbType.VarChar, 50).Value = "";
                    cmdSaveRefDr.Parameters.Add("@Suburb", SqlDbType.VarChar, 50).Value = refDrCity.Trim().Replace("'", "`");
                    cmdSaveRefDr.Parameters.Add("@PostalCode", SqlDbType.VarChar, 10).Value = refDrPostcode.Trim().Replace("'", "`");
                    cmdSaveRefDr.Parameters.Add("@State", SqlDbType.VarChar, 10).Value = refDrState.Trim().Replace("'", "`");
                    cmdSaveRefDr.Parameters.Add("@Phone", SqlDbType.VarChar, 20).Value = refDrPhone.Trim().Replace("'", "`");
                    cmdSaveRefDr.Parameters.Add("@Fax", SqlDbType.VarChar, 20).Value = refDrFax.Trim().Replace("'", "`");
                    cmdSaveRefDr.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
                    cmdSaveRefDr.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                    cmdSaveRefDr.Parameters.Add("@vblnIsHide", SqlDbType.Bit).Value = false;

                    gClass.ExecuteDMLCommand(cmdSaveRefDr);


                    dsRefDr = new DataSet();
                    cmdSelectRefDr = new SqlCommand();
                    gClass.MakeStoreProcedureName(ref cmdSelectRefDr, "sp_RefDoctors_SelectRefDoctorByNameAndCity", true);
                    cmdSelectRefDr.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                    cmdSelectRefDr.Parameters.Add("@Surname", System.Data.SqlDbType.VarChar, 50).Value = refDrSurname;
                    cmdSelectRefDr.Parameters.Add("@Firstname", System.Data.SqlDbType.VarChar, 50).Value = refDrFirstname;
                    cmdSelectRefDr.Parameters.Add("@City", System.Data.SqlDbType.VarChar, 50).Value = refDrCity;

                    dsRefDr = gClass.FetchData(cmdSelectRefDr, "tblRefDr");
                    refdrid = dsRefDr.Tables[0].Rows[0]["RefDrId"].ToString();
                }
            }
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                                    Context.Request.Cookies["Logon_UserName"].Value, "Import CSV", "Import CSV", err.ToString());

        }

        return refdrid;
    }
    #endregion

    #region protected string GetDrId(string drSurname, string drFirstname, string drCity)
    protected string GetDrId(string drSurname, string drFirstname, string drCity)
    {
        //check if dr exist by using surname, firstname and city(suburb)
        //  if exist, skip and use the first ref id found as the doctor for the patient
        //  if not exist, add the doctor
        //      get the dr id by using the same sql
        //      insert visit

        SqlCommand cmdSelectDr = new SqlCommand();
        SqlCommand cmdSaveDr = new SqlCommand();
        string drid = "";

        try
        {
            //checkin, inserting dr 1
            if (drSurname.Trim() != "" && drFirstname.Trim() != "")
            {
                DataSet dsDr = new DataSet();
                gClass.MakeStoreProcedureName(ref cmdSelectDr, "sp_Doctors_SelectDoctorByNameAndCity", true);
                cmdSelectDr.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                cmdSelectDr.Parameters.Add("@Surname", System.Data.SqlDbType.VarChar, 50).Value = drSurname;
                cmdSelectDr.Parameters.Add("@Firstname", System.Data.SqlDbType.VarChar, 50).Value = drFirstname;
                cmdSelectDr.Parameters.Add("@City", System.Data.SqlDbType.VarChar, 50).Value = drCity;

                dsDr = gClass.FetchData(cmdSelectDr, "tblDr");
                if (dsDr.Tables[0].Rows.Count > 0)
                {
                    //grab the dr id
                    drid = dsDr.Tables[0].Rows[0]["DoctorID"].ToString();
                }
                else
                {
                    //add dr
                    gClass.MakeStoreProcedureName(ref cmdSaveDr, "sp_Doctors_SaveData", true);

                    cmdSaveDr.Parameters.Add("@DoctorID", SqlDbType.Int).Value = 0;
                    cmdSaveDr.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                    cmdSaveDr.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
                    cmdSaveDr.Parameters.Add("@Surname", SqlDbType.VarChar, 30).Value = drSurname.Replace("'", "`");
                    cmdSaveDr.Parameters.Add("@Firstname", SqlDbType.VarChar, 30).Value = drFirstname.Replace("'", "`");
                    cmdSaveDr.Parameters.Add("@Title", SqlDbType.VarChar, 10).Value = "";
                    cmdSaveDr.Parameters.Add("@DoctorBoldCode", SqlDbType.VarChar, 20).Value = "";
                    cmdSaveDr.Parameters.Add("@Address1", SqlDbType.VarChar, 50).Value = "";
                    cmdSaveDr.Parameters.Add("@Address2", SqlDbType.VarChar, 50).Value = "";
                    cmdSaveDr.Parameters.Add("@Suburb", SqlDbType.VarChar, 40).Value = drCity.Replace("'", "`");
                    cmdSaveDr.Parameters.Add("@Postcode", SqlDbType.VarChar, 10).Value = "";
                    cmdSaveDr.Parameters.Add("@State", SqlDbType.VarChar, 10).Value = "";
                    cmdSaveDr.Parameters.Add("@Degrees", SqlDbType.VarChar, 50).Value = "";
                    cmdSaveDr.Parameters.Add("@Speciality", SqlDbType.VarChar, 100).Value = "";
                    cmdSaveDr.Parameters.Add("@UseOwnLetterHead", SqlDbType.Bit).Value = false;
                    cmdSaveDr.Parameters.Add("@PrefSurgeryType", SqlDbType.VarChar, 10).Value = "";
                    cmdSaveDr.Parameters.Add("@PrefApproach", SqlDbType.VarChar, 50).Value = "";
                    cmdSaveDr.Parameters.Add("@PrefCategory", SqlDbType.VarChar, 10).Value = "";
                    cmdSaveDr.Parameters.Add("@PrefGroup", SqlDbType.VarChar, 10).Value = "";
                    cmdSaveDr.Parameters.Add("@vblnIsSurgeon", SqlDbType.Bit).Value = true;
                    cmdSaveDr.Parameters.Add("@vblnIsHide", SqlDbType.Bit).Value = false;

                    gClass.SaveDoctorData(cmdSaveDr,
                                    Convert.ToInt32(gClass.OrganizationCode),
                                    Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value),
                                    true);


                    dsDr = new DataSet();
                    cmdSelectDr = new SqlCommand();
                    gClass.MakeStoreProcedureName(ref cmdSelectDr, "sp_Doctors_SelectDoctorByNameAndCity", true);
                    cmdSelectDr.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                    cmdSelectDr.Parameters.Add("@Surname", System.Data.SqlDbType.VarChar, 50).Value = drSurname;
                    cmdSelectDr.Parameters.Add("@Firstname", System.Data.SqlDbType.VarChar, 50).Value = drFirstname;
                    cmdSelectDr.Parameters.Add("@City", System.Data.SqlDbType.VarChar, 50).Value = drCity;

                    dsDr = gClass.FetchData(cmdSelectDr, "tblDr");
                    drid = dsDr.Tables[0].Rows[0]["DoctorID"].ToString();
                }
            }
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                                    Context.Request.Cookies["Logon_UserName"].Value, "Import CSV", "Import CSV", err.ToString());

        }

        return drid;
    }
    #endregion
    
    #region protected void ImportFile(object sender, EventArgs e)
    protected void ImportFile(object sender, EventArgs e)
    {
        string importType = selectType.SelectedValue;
        if (importType == "patient")
            ImportPatientCSV();
        else if (importType == "visit")
            ImportVisitCSV();
        //else if (importType == "operation")
        //    ImportOperationCSV();
        else if (importType == "operationtype")
            ImportOperationTypeCSV();
        else if (importType == "operationduration")
            ImportOperationDurationCSV();
    }
    #endregion

    #region protected void DownloadFile(object sender, EventArgs e)
    protected void DownloadFile(object sender, EventArgs e)
    {
        string openFilePath = "";
        string importType = selectType.SelectedValue;
        string filename = "";

        openFilePath = "../../temp/template/" + importType + ".csv";
        Response.Redirect(openFilePath);
    }
    #endregion

    #region private void ImportPatientCSV()
    private void ImportPatientCSV()
    {

        //need to add refby doctor first


        try
        {
            string FileControl = "textFile";
            string filePath = "";

            int patientSuccessUpdate = 0;
            int patientSuccessInsert = 0;
            int patientFail = 0;
            int recordNo = 0;
            Boolean failImport = false;
            string patientFailList = "";
            string message = "";
            string[] patientDetails;

            string resultNotes = "";

            Int32 int32Temp = 0;
            Int64 matchingPatientID = 0;
            Int64 patientID = 0;
            String strNameId = String.Empty;
            string surname = "";
            string firstname = "";
            string dob = "";
            string gender = "";
            string family = "";
            string street = "";
            string city = "";
            string state = "";
            string postcode = "";
            string email = "";
            string homephone = "";
            string workphone = "";
            string mobilephone = "";
            string clinicalnotes = "";
            string refDrId1 = "";
            string refDrId2 = "";
            string refDrSurname = "";
            string refDrFirstname = "";
            string refDrAddress = "";
            string refDrCity = "";
            string refDrState = "";
            string refDrPostcode = "";
            string refDrPhone = "";
            string refDrFax = "";
            string refDrSurname2 = "";
            string refDrFirstname2 = "";
            string refDrAddress2 = "";
            string refDrCity2 = "";
            string refDrState2 = "";
            string refDrPostcode2 = "";
            string refDrPhone2 = "";
            string refDrFax2 = "";
            string strRefSurName = "";

            string height = "";
            string weight = "";
            string bmi = "";

            filePath = GetFilePath(textFile.PostedFile.FileName);
            if (textFile.PostedFile.FileName != "")
            {
                if (Path.GetExtension(textFile.PostedFile.FileName).ToUpper() == ".CSV")
                {
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);

                    textFile.PostedFile.SaveAs(filePath);

                    StreamReader streamReader = new StreamReader(filePath);

                    do
                    {
                        message = streamReader.ReadLine();
                        if (recordNo > 0)
                        {
                            SqlCommand cmdSelect = new SqlCommand();
                            SqlCommand cmdSelectLast = new SqlCommand();
                            SqlCommand cmdSave = new SqlCommand();
                            SqlCommand cmdSaveEMR = new SqlCommand();

                            //message.Replace(',','|');
                            //message.Replace("'",'`');

                            patientDetails = message.Split(',');

                            matchingPatientID = 0;

                            surname = patientDetails[0];
                            firstname = patientDetails[1];
                            dob = patientDetails[2];
                            gender = patientDetails[3];
                            street = patientDetails[4] + " " + patientDetails[5];
                            city = patientDetails[6];
                            postcode = patientDetails[7];
                            state = patientDetails[8];
                            homephone = patientDetails[9];
                            workphone = patientDetails[10];
                            mobilephone = patientDetails[11];
                            email = patientDetails[12];
                            refDrSurname = patientDetails[13];
                            refDrFirstname = patientDetails[14];
                            refDrAddress = patientDetails[15];
                            refDrCity = patientDetails[16];
                            refDrState = patientDetails[17];
                            refDrPostcode = patientDetails[18];
                            refDrPhone = patientDetails[19];
                            refDrFax = patientDetails[20];
                            refDrSurname2 = patientDetails[21];
                            refDrFirstname2 = patientDetails[22];
                            refDrAddress2 = patientDetails[23];
                            refDrCity2 = patientDetails[24];
                            refDrState2 = patientDetails[25];
                            refDrPostcode2 = patientDetails[26];
                            refDrPhone2 = patientDetails[27];
                            refDrFax2 = patientDetails[28];

                            refDrId1 = GetRefDrId(refDrSurname, refDrFirstname, refDrAddress, refDrCity, refDrState, refDrPostcode, refDrPhone, refDrFax);
                            refDrId2 = GetRefDrId(refDrSurname2, refDrFirstname2, refDrAddress2, refDrCity2, refDrState2, refDrPostcode2, refDrPhone2, refDrFax2);

                            strNameId = (surname.Length == 0) ? String.Empty : ((surname.Length > 3) ? surname.Substring(0, 4) : surname);
                            strNameId += (firstname.Length == 0) ? String.Empty : firstname.Substring(0, 1);

                            DataSet dsPatient = new DataSet();
                            gClass.MakeStoreProcedureName(ref cmdSelect, "sp_PatientData_LoadDataByNameDOB", true);
                            cmdSelect.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                            cmdSelect.Parameters.Add("@Surname", System.Data.SqlDbType.VarChar, 40).Value = surname;
                            cmdSelect.Parameters.Add("@Firstname", System.Data.SqlDbType.VarChar, 30).Value = firstname;
                            cmdSelect.Parameters.Add("@Birthdate", System.Data.SqlDbType.DateTime).Value = Convert.ToDateTime(dob);

                            dsPatient = gClass.FetchData(cmdSelect, "tblPatient");
                            if (dsPatient.Tables[0].Rows.Count > 0)
                            {
                                //update
                                matchingPatientID = Convert.ToInt32(dsPatient.Tables[0].Rows[0]["Patient ID"]);
                                cmdSave.Parameters.Add("@PatientId", SqlDbType.Int).Value = matchingPatientID;

                                gClass.MakeStoreProcedureName(ref cmdSave, "sp_PatientData_cmdUpdate", true);
                            }
                            else
                            {
                                gClass.MakeStoreProcedureName(ref cmdSave, "sp_PatientData_cmdInsert", true);
                            }

                            cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Int32.TryParse(gClass.OrganizationCode, out int32Temp) ? int32Temp : 0;
                            cmdSave.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Int32.TryParse(Request.Cookies["UserPracticeCode"].Value, out int32Temp) ? int32Temp : 0;

                            cmdSave.Parameters.Add("@NameId", SqlDbType.VarChar, 7).Value = strNameId;
                            cmdSave.Parameters.Add("@Surname", SqlDbType.VarChar, 40).Value = surname;
                            cmdSave.Parameters.Add("@Firstname", SqlDbType.VarChar, 30).Value = firstname;
                            cmdSave.Parameters.Add("@Title", SqlDbType.SmallInt).Value = DBNull.Value;
                            cmdSave.Parameters.Add("@Street", SqlDbType.VarChar, 40).Value = street;
                            cmdSave.Parameters.Add("@Suburb", SqlDbType.VarChar, 40).Value = city;
                            cmdSave.Parameters.Add("@State", SqlDbType.VarChar, 10).Value = state;
                            cmdSave.Parameters.Add("@Postcode", SqlDbType.VarChar, 10).Value = postcode;
                            cmdSave.Parameters.Add("@HomePhone", SqlDbType.VarChar, 30).Value = homephone;
                            cmdSave.Parameters.Add("@WorkPhone", SqlDbType.VarChar, 30).Value = workphone;
                            cmdSave.Parameters.Add("@Race", SqlDbType.VarChar, 3).Value = DBNull.Value;
                            cmdSave.Parameters.Add("@Birthdate", SqlDbType.DateTime).Value = Convert.ToDateTime(dob);
                            cmdSave.Parameters.Add("@Consultationdate", SqlDbType.DateTime).Value = DBNull.Value;
                            cmdSave.Parameters.Add("@Sex", SqlDbType.VarChar, 1).Value = gender;
                            cmdSave.Parameters.Add("@DoctorId", SqlDbType.Int).Value = DBNull.Value;
                            cmdSave.Parameters.Add("@RefDrId1", SqlDbType.VarChar, 10).Value = refDrId1;
                            cmdSave.Parameters.Add("@RefDrId2", SqlDbType.VarChar, 10).Value = refDrId2;
                            cmdSave.Parameters.Add("@RefDrId3", SqlDbType.VarChar, 10).Value = DBNull.Value;
                            cmdSave.Parameters.Add("@Patient_MDId", SqlDbType.VarChar, 10).Value = DBNull.Value;
                            cmdSave.Parameters.Add("@RefDrDuration1", SqlDbType.VarChar, 3).Value = DBNull.Value;
                            cmdSave.Parameters.Add("@RefDrDuration2", SqlDbType.VarChar, 3).Value = DBNull.Value;
                            cmdSave.Parameters.Add("@RefDrDuration3", SqlDbType.VarChar, 3).Value = DBNull.Value;
                            cmdSave.Parameters.Add("@RefDrStatus1", SqlDbType.VarChar, 3).Value = DBNull.Value;
                            cmdSave.Parameters.Add("@RefDrStatus2", SqlDbType.VarChar, 3).Value = DBNull.Value;
                            cmdSave.Parameters.Add("@RefDrStatus3", SqlDbType.VarChar, 3).Value = DBNull.Value;
                            cmdSave.Parameters.Add("@RefDrDate1", SqlDbType.DateTime).Value = DBNull.Value;
                            cmdSave.Parameters.Add("@RefDrDate2", SqlDbType.DateTime).Value = DBNull.Value;
                            cmdSave.Parameters.Add("@RefDrDate3", SqlDbType.DateTime).Value = DBNull.Value;
                            cmdSave.Parameters.Add("@MobilePhone", SqlDbType.VarChar, 30).Value = mobilephone;
                            cmdSave.Parameters.Add("@EmailAddress", SqlDbType.VarChar, 100).Value = email;
                            cmdSave.Parameters.Add("@Insurance", SqlDbType.VarChar, 50).Value = DBNull.Value;
                            cmdSave.Parameters.Add("@Patient_CustomID", SqlDbType.VarChar, 20).Value = DBNull.Value;
                            cmdSave.Parameters.Add("@SocialHistory", SqlDbType.VarChar, 2048).Value = DBNull.Value;
                            cmdSave.Parameters.Add("@MedicalSummary", SqlDbType.VarChar).Value = DBNull.Value;
                            cmdSave.Parameters.Add("@SourceImport", SqlDbType.Bit).Value = true;

                            gClass.AddLogParameters(ref cmdSave, Request.Cookies["Logon_UserName"].Value, Request.Url.Host, (matchingPatientID == 0) ? "insert" : "update");

                            //add emr data (family structure)
                            try
                            {
                                gClass.ExecuteDMLCommand(cmdSave);
                                gClass.SaveUserLogFile(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Cookies["Logon_UserName"].Value,
                                                        Context.Request.Url.Host, "Import CSV", 2, "Save CSV Data", "PID:", matchingPatientID.ToString());

                                if (matchingPatientID > 0)
                                    patientSuccessUpdate++;
                                else
                                    patientSuccessInsert++;
                            }
                            catch (Exception err)
                            {
                                patientFail++;
                                patientFailList += "\n-" + firstname + " " + surname + ", " + dob;
                                // failed to insert test for which patient in which file name
                                gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                                            Context.Request.Cookies["Logon_UserName"].Value, "PID : " + matchingPatientID.ToString(), "Save CSV Data", err.ToString());
                            }
                        }
                        recordNo++;
                    } while (streamReader.Peek() != -1);
                    resultNotes += "Success insert: " + patientSuccessInsert;
                    resultNotes += "\nSuccess update: " + patientSuccessUpdate;
                    resultNotes += "\nFail: " + patientFail;

                    if (patientFail > 0)
                    resultNotes += patientFailList;
                }
                else
                {
                    resultNotes = "Please enter a proper csv file";
                }
            }
            txtNotes.Value = "RESULT:\n\n" + resultNotes;
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                                                    Context.Request.Cookies["Logon_UserName"].Value, "Import CSV", "Import CSV", err.ToString());
        }
    }
    #endregion

    #region private void ImportOperationDurationCSV()
    private void ImportOperationDurationCSV()
    {
        try
        {
            string FileControl = "textFile";
            string filePath = "";

            int patientFound = 0;
            int patientNotFound = 0;
            int patientSuccessUpdate = 0;
            int patientSuccessInsert = 0;
            int patientFail = 0;
            int recordNo = 0;
            Boolean failImport = false;
            string message = "";
            string[] patientDetails;

            Int32 int32Temp = 0;
            string resultNotes = "";
            string updateNotes = "";
            string insertNotes = "";
            string notFoundNotes = "";
            string failNotes = "";
            string updateOrInsert = "";

            string surname = "";
            string firstname = "";
            string operationDate = "";
            string operationDuration = "";

            string oldDuration = "";

            SqlCommand cmdSelect = new SqlCommand();
            SqlCommand cmdSave = new SqlCommand();

            filePath = GetFilePath(textFile.PostedFile.FileName);
            if (textFile.PostedFile.FileName != "")
            {
                if (Path.GetExtension(textFile.PostedFile.FileName).ToUpper() == ".CSV")
                {
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);

                    textFile.PostedFile.SaveAs(filePath);

                    StreamReader streamReader = new StreamReader(filePath);

                    do
                    {
                        if (recordNo > 0)
                        {
                            cmdSelect = new SqlCommand();
                            cmdSave = new SqlCommand();

                            message = streamReader.ReadLine();

                            patientDetails = message.Split(',');

                            surname = patientDetails[0];
                            firstname = patientDetails[1];
                            operationDate = patientDetails[2];
                            operationDuration = patientDetails[3];
                            oldDuration = "0";

                            DataSet dsPatient = new DataSet();
                            gClass.MakeStoreProcedureName(ref cmdSelect, "sp_PatientOperation_CheckOperationExist", true);
                            cmdSelect.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                            cmdSelect.Parameters.Add("@Surname", System.Data.SqlDbType.VarChar, 40).Value = surname;
                            cmdSelect.Parameters.Add("@Firstname", System.Data.SqlDbType.VarChar, 30).Value = firstname;
                            cmdSelect.Parameters.Add("@OperationDate", System.Data.SqlDbType.DateTime).Value = Convert.ToDateTime(operationDate);
                            //adjust original date to match date format in database local and production might different

                            dsPatient = gClass.FetchData(cmdSelect, "tblPatient");
                            if (dsPatient.Tables[0].Rows.Count > 0)
                            {
                                patientFound++;
                                updateOrInsert = "update";
                                oldDuration = dsPatient.Tables[0].Rows[0]["duration"].ToString().Trim();
                                oldDuration = oldDuration == "" ? "0" : oldDuration;

                                gClass.MakeStoreProcedureName(ref cmdSave, "sp_PatientOperation_cmdUpdateOperationDuration", true);
                                //}
                                //else
                                //{
                                //    updateOrInsert = "insert";
                                //    gClass.MakeStoreProcedureName(ref cmdSave, "sp_PatientOperation_cmdInsertOperationDuration", true);
                                //}

                                cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Int32.TryParse(gClass.OrganizationCode, out int32Temp) ? int32Temp : 0;

                                cmdSave.Parameters.Add("@Surname", SqlDbType.VarChar, 40).Value = surname;
                                cmdSave.Parameters.Add("@Firstname", SqlDbType.VarChar, 30).Value = firstname;
                                cmdSave.Parameters.Add("@OperationDate", SqlDbType.DateTime).Value = Convert.ToDateTime(operationDate);
                                cmdSave.Parameters.Add("@OperationDuration", SqlDbType.Int).Value = Convert.ToInt32(operationDuration);

                                gClass.AddLogParameters(ref cmdSave, Request.Cookies["Logon_UserName"].Value, Request.Url.Host, updateOrInsert);

                                try
                                {
                                    gClass.ExecuteDMLCommand(cmdSave);
                                    gClass.SaveUserLogFile(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Cookies["Logon_UserName"].Value,
                                                            Context.Request.Url.Host, "Import CSV Operation", 2, "Save CSV Data", "Pname: ", firstname + " " + surname);
                                    if (updateOrInsert == "update")
                                    {
                                        patientSuccessUpdate++;
                                        updateNotes += "\n- Update: #" + recordNo + ", " + "Surname=" + surname + ", Firstname=" + firstname + ", OperationDate=" + operationDate + ", PrevOperationDuration=" + oldDuration + ", NewOperationDuration=" + operationDuration;
                                    }
                                    else
                                    {
                                        patientSuccessInsert++;
                                        insertNotes += "\n- Insert: #" + recordNo + ", " + "Surname=" + surname + ", Firstname=" + firstname + ", OperationDate=" + operationDate + ", PrevOperationDuration=" + oldDuration + ", NewOperationDuration=" + operationDuration;
                                    }
                                }
                                catch (Exception err)
                                {
                                    patientFail++;
                                    failNotes += "\n- Failed " + updateOrInsert + ": #" + recordNo + ", " + "Surname=" + surname + ", Firstname=" + firstname + ", OperationDate=" + operationDate + ", PrevOperationDuration=" + oldDuration + ", NewOperationDuration=" + operationDuration;
                                    // failed to insert test for which patient in which file name
                                    gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                                                Context.Request.Cookies["Logon_UserName"].Value, "Pname: " + firstname + " " + surname, "Save CSV Operation", err.ToString());
                                }
                            }
                            else
                            {
                                patientNotFound++;
                                notFoundNotes += "\n- Not Found: #" + recordNo + ", " + "Surname=" + surname + ", Firstname=" + firstname + ", OperationDate=" + operationDate + ", NewOperationDuration=" + operationDuration;

                            }
                        }
                        recordNo++;
                    } while (streamReader.Peek() != -1);
                    resultNotes += "Record found: " + patientFound;
                    resultNotes += "\n\nSuccess insert: " + patientSuccessInsert;
                    resultNotes += "\n" + insertNotes;
                    resultNotes += "\n\nSuccess update: " + patientSuccessUpdate;
                    resultNotes += "\n" + updateNotes;
                    resultNotes += "\n\nRecord not found: " + patientNotFound;
                    resultNotes += "\n" + notFoundNotes;
                    resultNotes += "\n\nFail: " + patientFail;
                    resultNotes += "\n" + failNotes;

                    resultNotes += "\nFail: " + patientFail;
                }
                else
                {
                    resultNotes = "Please enter a proper csv file";
                }
            }
            txtNotes.Value = "RESULT:\n\n" + resultNotes;
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                                                    Context.Request.Cookies["Logon_UserName"].Value, "Import CSV", "Import CSV", err.ToString());
        }
    }
    #endregion

    #region private void ImportOperationTypeCSV()
    private void ImportOperationTypeCSV()
    {
        try
        {
            string FileControl = "textFile";
            string filePath = "";

            int patientFound = 0;
            int patientNotFound = 0;
            int patientSuccessUpdate = 0;
            int patientSuccessInsert = 0;
            int patientFail = 0;
            int recordNo = 0;
            Boolean failImport = false;
            string message = "";
            string[] patientDetails;

            Int32 int32Temp = 0;
            string resultNotes = "";
            string updateNotes = "";
            string insertNotes = "";
            string notFoundNotes = "";
            string failNotes = "";
            string updateOrInsert = "";

            string surname = "";
            string firstname = "";
            string operationDate = "";
            string operationSurgery = "";

            string oldSurgery = "";

            SqlCommand cmdSelect = new SqlCommand();
            SqlCommand cmdSave = new SqlCommand();

            filePath = GetFilePath(textFile.PostedFile.FileName);
            if (textFile.PostedFile.FileName != "")
            {
                if (Path.GetExtension(textFile.PostedFile.FileName).ToUpper() == ".CSV")
                {
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);

                    textFile.PostedFile.SaveAs(filePath);

                    StreamReader streamReader = new StreamReader(filePath);

                    do
                    {
                        message = streamReader.ReadLine();
                        if (recordNo > 0)
                        {
                            cmdSelect = new SqlCommand();
                            cmdSave = new SqlCommand();


                            patientDetails = message.Split(',');

                            surname = patientDetails[0];
                            firstname = patientDetails[1];
                            operationDate = patientDetails[2];
                            operationSurgery = patientDetails[3];
                            oldSurgery = "";

                            DataSet dsPatient = new DataSet();
                            gClass.MakeStoreProcedureName(ref cmdSelect, "sp_PatientOperation_CheckOperationExist", true);
                            cmdSelect.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                            cmdSelect.Parameters.Add("@Surname", System.Data.SqlDbType.VarChar, 40).Value = surname;
                            cmdSelect.Parameters.Add("@Firstname", System.Data.SqlDbType.VarChar, 30).Value = firstname;
                            cmdSelect.Parameters.Add("@OperationDate", System.Data.SqlDbType.DateTime).Value = Convert.ToDateTime(operationDate);
                            //adjust original date to match date format in database local and production might different

                            dsPatient = gClass.FetchData(cmdSelect, "tblPatient");
                            if (dsPatient.Tables[0].Rows.Count > 0)
                            {
                                patientFound++;
                                updateOrInsert = "update";
                                oldSurgery = dsPatient.Tables[0].Rows[0]["SurgeryType"].ToString().Trim();

                                gClass.MakeStoreProcedureName(ref cmdSave, "sp_PatientOperation_cmdUpdateOperationType", true);
                                //}
                                //else
                                //{
                                //    updateOrInsert = "insert";
                                //    gClass.MakeStoreProcedureName(ref cmdSave, "sp_PatientOperation_cmdInsertOperationDuration", true);
                                //}

                                cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Int32.TryParse(gClass.OrganizationCode, out int32Temp) ? int32Temp : 0;

                                cmdSave.Parameters.Add("@Surname", SqlDbType.VarChar, 40).Value = surname;
                                cmdSave.Parameters.Add("@Firstname", SqlDbType.VarChar, 30).Value = firstname;
                                cmdSave.Parameters.Add("@OperationDate", SqlDbType.DateTime).Value = Convert.ToDateTime(operationDate);
                                cmdSave.Parameters.Add("@OperationType", SqlDbType.VarChar, 20).Value = operationSurgery;

                                gClass.AddLogParameters(ref cmdSave, Request.Cookies["Logon_UserName"].Value, Request.Url.Host, updateOrInsert);

                                try
                                {
                                    gClass.ExecuteDMLCommand(cmdSave);
                                    gClass.SaveUserLogFile(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Cookies["Logon_UserName"].Value,
                                                            Context.Request.Url.Host, "Import CSV Operation", 2, "Save CSV Data", "Pname: ", firstname + " " + surname);
                                    if (updateOrInsert == "update")
                                    {
                                        patientSuccessUpdate++;
                                        updateNotes += "\n- Update: #" + recordNo + ", " + "Surname=" + surname + ", Firstname=" + firstname + ", OperationDate=" + operationDate + ", PrevOperationType=" + oldSurgery + ", NewOperationType=" + operationSurgery;
                                    }
                                    else
                                    {
                                        patientSuccessInsert++;
                                        insertNotes += "\n- Insert: #" + recordNo + ", " + "Surname=" + surname + ", Firstname=" + firstname + ", OperationDate=" + operationDate + ", PrevOperationType=" + oldSurgery + ", NewOperationType=" + operationSurgery;
                                    }
                                }
                                catch (Exception err)
                                {
                                    patientFail++;
                                    failNotes += "\n- Failed " + updateOrInsert + ": #" + recordNo + ", " + "Surname=" + surname + ", Firstname=" + firstname + ", OperationDate=" + operationDate + ", PrevOperationType=" + oldSurgery + ", NewOperationType=" + operationSurgery;
                                    // failed to insert test for which patient in which file name
                                    gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                                                Context.Request.Cookies["Logon_UserName"].Value, "Pname: " + firstname + " " + surname, "Save CSV Operation", err.ToString());
                                }
                            }
                            else
                            {
                                patientNotFound++;
                                notFoundNotes += "\n- Not Found: #" + recordNo + ", " + "Surname=" + surname + ", Firstname=" + firstname + ", OperationDate=" + operationDate + ", NewOperationType=" + operationSurgery;

                            }
                        }
                        recordNo++;
                    } while (streamReader.Peek() != -1);
                    resultNotes += "Record found: " + patientFound;
                    resultNotes += "\n\nSuccess insert: " + patientSuccessInsert;
                    resultNotes += "\n" + insertNotes;
                    resultNotes += "\n\nSuccess update: " + patientSuccessUpdate;
                    resultNotes += "\n" + updateNotes;
                    resultNotes += "\n\nRecord not found: " + patientNotFound;
                    resultNotes += "\n" + notFoundNotes;
                    resultNotes += "\n\nFail: " + patientFail;
                    resultNotes += "\n" + failNotes;

                    resultNotes += "\nFail: " + patientFail;
                }
                else
                {
                    resultNotes = "Please enter a proper csv file";
                }
            }
            txtNotes.Value = "RESULT:\n\n" + resultNotes;
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                                                    Context.Request.Cookies["Logon_UserName"].Value, "Import CSV", "Import CSV", err.ToString());
        }
    }
    #endregion

    //#region private void ImportOperationCSV()
    //private void ImportOperationCSV()
    //{
    //    try
    //    {
    //        string FileControl = "textFile";
    //        string filePath = "";

    //        int patientFound = 0;
    //        int patientNotFound = 0;
    //        int patientSuccessUpdate = 0;
    //        int patientSuccessInsert = 0;
    //        int patientFail = 0;
    //        int recordNo = 0;
    //        Boolean failImport = false;
    //        string message = "";
    //        string[] patientDetails;

    //        Int32 int32Temp = 0;
    //        string resultNotes = "";
    //        string updateNotes = "";
    //        string insertNotes = "";
    //        string notFoundNotes = "";
    //        string failNotes = "";
    //        string updateOrInsert = "";

    //        string surname = "";
    //        string firstname = "";
    //        string operationDate = "";
    //        string operationSurgery = "";
    //        string operationDuration = "";
    //        string operationBySurname = "";
    //        string operationByFirstname = "";
    //        string operationByCity = "";
    //        string operationBy = "";

    //        string oldSurgery = "";

    //        SqlCommand cmdSelect = new SqlCommand();
    //        SqlCommand cmdSelectExist = new SqlCommand();
    //        SqlCommand cmdSave = new SqlCommand();
    //        DataSet dsPatient = new DataSet();
    //        DataSet dsPatientExist = new DataSet();

    //        filePath = GetFilePath(textFile.PostedFile.FileName);
    //        if (textFile.PostedFile.FileName != "")
    //        {
    //            if (Path.GetExtension(textFile.PostedFile.FileName).ToUpper() == ".CSV")
    //            {
    //                if (System.IO.File.Exists(filePath))
    //                    System.IO.File.Delete(filePath);

    //                textFile.PostedFile.SaveAs(filePath);

    //                StreamReader streamReader = new StreamReader(filePath);

    //                do
    //                {
    //                    message = streamReader.ReadLine();
    //                    if (recordNo > 0)
    //                    {
    //                        cmdSelect = new SqlCommand();
    //                        cmdSave = new SqlCommand();


    //                        patientDetails = message.Split(',');

    //                        surname = patientDetails[0];
    //                        firstname = patientDetails[1];
    //                        dob = patientDetails[2];
    //                        operationDate = patientDetails[3];
    //                        operationSurgery = patientDetails[4];
    //                        operationDuration = patientDetails[5];
    //                        operationBySurname = patientDetails[6];
    //                        operationByFirstname = patientDetails[7];
    //                        operationByCity = patientDetails[8];
    //                        oldSurgery = "";

    //                        dsPatient = new DataSet();
    //                        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_PatientOperation_CheckOperationExist", true);
    //                        cmdSelect.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
    //                        cmdSelect.Parameters.Add("@Surname", System.Data.SqlDbType.VarChar, 40).Value = surname;
    //                        cmdSelect.Parameters.Add("@Firstname", System.Data.SqlDbType.VarChar, 30).Value = firstname;
    //                        cmdSelect.Parameters.Add("@OperationDate", System.Data.SqlDbType.DateTime).Value = Convert.ToDateTime(operationDate);
    //                        //adjust original date to match date format in database local and production might different

    //                        //check if patient exist
    //                        dsPatientExist = new DataSet();
    //                        gClass.MakeStoreProcedureName(ref cmdSelectExist, "sp_PatientData_LoadDataByNameDOB", true);
    //                        cmdSelectExist.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
    //                        cmdSelectExist.Parameters.Add("@Surname", System.Data.SqlDbType.VarChar, 40).Value = surname;
    //                        cmdSelectExist.Parameters.Add("@Firstname", System.Data.SqlDbType.VarChar, 30).Value = firstname;
    //                        cmdSelectExist.Parameters.Add("@Birthdate", System.Data.SqlDbType.DateTime).Value = Convert.ToDateTime(dob);

    //                        dsPatientExist = gClass.FetchData(cmdSelectExist, "tblPatientExist");
    //                        if (dsPatientExist.Tables[0].Rows.Count > 0)
    //                        {
    //                            patientid = dsPatientExist.Tables[0].Rows[0]["PatientID"].ToString().Trim();
    //                            intPatientID = Int32.TryParse(patientid, out int32Temp) ? int32Temp : 0;
    //                        }

    //                        operationBy = GetDrId(operationBySurname, operationByFirstname, operationByCity);
                            
    //                        if(operationBy.Trim() != "")
    //                        {
    //                            decResult = 0;
    //                            if (dsPatient.Tables[0].Rows.Count > 0)
    //                            {
    //                                //update
    //                                patientFound++;
    //                                updateOrInsert = "update";
    //                                oldSurgery = dsPatient.Tables[0].Rows[0]["SurgeryType"].ToString().Trim();
    //                                gClass.MakeStoreProcedureName(ref cmdSave, "sp_PatientOperations_UpdateData", true);

    //                                cmdSave.Parameters.Add("@SourceImport", SqlDbType.Bit).Value = true;
    //                            }
    //                            else
    //                            {
    //                                //insert
    //                                updateOrInsert = "insert";
    //                                gClass.MakeStoreProcedureName(ref cmdSave, "sp_PatientOperations_InsertData", true);
                                
                                    
                                    
    //                                cmdSave.Parameters.Add("@SourceImport", SqlDbType.Bit).Value = true;
    //                            }

    //                            try
    //                            {
    //                                gClass.ExecuteDMLCommand(cmdSave);

    //                                //if success, save bold
    //                                gClass.SaveUserLogFile(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Cookies["Logon_UserName"].Value,
    //                                                        Context.Request.Url.Host, "Import CSV Operation", 2, "Save CSV Data", "Pname: ", firstname + " " + surname);
    //                                if (updateOrInsert == "update")
    //                                {
    //                                    patientSuccessUpdate++;
    //                                    updateNotes += "\n- Update: #" + recordNo + ", " + "Surname=" + surname + ", Firstname=" + firstname + ", OperationDate=" + operationDate + ", PrevOperationType=" + oldSurgery + ", NewOperationType=" + operationSurgery;
    //                                }
    //                                else
    //                                {
    //                                    patientSuccessInsert++;
    //                                    insertNotes += "\n- Insert: #" + recordNo + ", " + "Surname=" + surname + ", Firstname=" + firstname + ", OperationDate=" + operationDate + ", PrevOperationType=" + oldSurgery + ", NewOperationType=" + operationSurgery;
    //                                }
    //                            }
    //                            catch (Exception err)
    //                            {
    //                                patientFail++;
    //                                failNotes += "\n- Failed " + updateOrInsert + ": #" + recordNo + ", " + "Surname=" + surname + ", Firstname=" + firstname + ", OperationDate=" + operationDate + ", PrevOperationType=" + oldSurgery + ", NewOperationType=" + operationSurgery;
    //                                // failed to insert test for which patient in which file name
    //                                gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
    //                                            Context.Request.Cookies["Logon_UserName"].Value, "Pname: " + firstname + " " + surname, "Save CSV Operation", err.ToString());
    //                            }
    //                        }
    //                        else
    //                        {
    //                            patientNotFound++;
    //                            notFoundNotes += "\n- Not Found: #" + recordNo + ", " + "Surname=" + surname + ", Firstname=" + firstname + ", OperationDate=" + operationDate + ", NewOperationType=" + operationSurgery;

    //                        }
    //                    }
    //                    recordNo++;
    //                } while (streamReader.Peek() != -1);
    //                resultNotes += "Record found: " + patientFound;
    //                resultNotes += "\n\nSuccess insert: " + patientSuccessInsert;
    //                resultNotes += "\n" + insertNotes;
    //                resultNotes += "\n\nSuccess update: " + patientSuccessUpdate;
    //                resultNotes += "\n" + updateNotes;
    //                resultNotes += "\n\nRecord not found: " + patientNotFound;
    //                resultNotes += "\n" + notFoundNotes;
    //                resultNotes += "\n\nFail: " + patientFail;
    //                resultNotes += "\n" + failNotes;

    //                resultNotes += "\nFail: " + patientFail;
    //            }
    //            else
    //            {
    //                resultNotes = "Please enter a proper csv file";
    //            }
    //        }
    //        txtNotes.Value = "RESULT:\n\n" + resultNotes;
    //    }
    //    catch (Exception err)
    //    {
    //        gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
    //                                                Context.Request.Cookies["Logon_UserName"].Value, "Import CSV", "Import CSV", err.ToString());
    //    }
    //}
    //#endregion

    #region private void ImportVisitCSV()
    private void ImportVisitCSV()
    {
        //still neet to update visit date
        try
        {
            string FileControl = "textFile";
            string filePath = "";

            int patientNotExist = 0;
            int patientFound = 0;
            int patientNotFound = 0;
            int doctorNotFound = 0;
            int patientSuccessUpdate = 0;
            int patientSuccessInsert = 0;
            int patientFail = 0;
            int recordNo = 0;
            Boolean failImport = false;
            string message = "";
            string[] patientDetails;

            Int32 int32Temp = 0;
            string resultNotes = "";
            string updateNotes = "";
            string insertNotes = "";
            string notFoundNotes = "";
            string patientNotExistNotes = "";
            string notFoundDoctorNotes = "";
            string failNotes = "";
            string updateOrInsert = "";

            string surname = "";
            string firstname = "";
            string dob = "";
            string visitDate = "";
            string visitSeenByID = "";
            string visitWeight = "";
            string visitHeight = "";
            string visitSeenBySurname = "";
            string visitSeenByFirstname = "";
            string visitSeenByCity = "";
            string visitNotes = "";
            string patientid = "";

            string prevVisitSeenBy = "";
            string prevVisitWeight = "";
            string prevVisitHeight = "";
            string prevVisitSeenBySurname = "";
            string prevVisitSeenByFirstname = "";

            DataSet dsPatient = new DataSet();
            DataSet dsPatientExist = new DataSet();
            DataSet dsDoctor = new DataSet();
            SqlCommand cmdSelect = new SqlCommand();
            SqlCommand cmdSelectExist = new SqlCommand();
            SqlCommand cmdSelectDoctor = new SqlCommand();
            SqlCommand cmdSave = new SqlCommand();

            Decimal decResult = 0;
            Decimal decWeight = 0;
            DateTime dtDateSeen;
            Int32 intPatientID = 0;

            Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

            filePath = GetFilePath(textFile.PostedFile.FileName);
            if (textFile.PostedFile.FileName != "")
            {
                if (Path.GetExtension(textFile.PostedFile.FileName).ToUpper() == ".CSV")
                {
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);

                    textFile.PostedFile.SaveAs(filePath);

                    StreamReader streamReader = new StreamReader(filePath);

                    do
                    {
                        message = streamReader.ReadLine();
                        if (recordNo > 0)
                        {
                            cmdSelect = new SqlCommand();
                            cmdSelectExist = new SqlCommand();
                            cmdSelectDoctor = new SqlCommand();
                            cmdSave = new SqlCommand();


                            // extract the fields
                            patientDetails = CSVParser.Split(message);

                            // clean up the fields (remove " and leading spaces)
                            for (int i = 0; i < patientDetails.Length; i++)
                            {
                                patientDetails[i] = patientDetails[i].TrimStart(' ', '"');
                                patientDetails[i] = patientDetails[i].TrimEnd('"');
                            }

                            surname = patientDetails[0];
                            firstname = patientDetails[1];
                            dob = patientDetails[2];
                            visitDate = patientDetails[3];
                            visitSeenByID = patientDetails[4];
                            visitWeight = patientDetails[5];
                            visitHeight = patientDetails[6];
                            visitSeenBySurname = patientDetails[7];
                            visitSeenByFirstname = patientDetails[8];
                            visitSeenByCity = patientDetails[9];
                            visitNotes = patientDetails[10];

                            prevVisitSeenBy = "";
                            prevVisitWeight = "";
                            prevVisitHeight = "";
                            prevVisitSeenBySurname = "";
                            prevVisitSeenByFirstname = "";
                            intPatientID = 0;
                            patientid = "";

                            dsPatient = new DataSet();
                            gClass.MakeStoreProcedureName(ref cmdSelect, "sp_ConsultFU1_CheckConsultExist", true);
                            cmdSelect.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                            cmdSelect.Parameters.Add("@Surname", System.Data.SqlDbType.VarChar, 40).Value = surname;
                            cmdSelect.Parameters.Add("@Firstname", System.Data.SqlDbType.VarChar, 30).Value = firstname;
                            cmdSelect.Parameters.Add("@DateSeen", System.Data.SqlDbType.DateTime).Value = Convert.ToDateTime(visitDate);
                            //adjust original date to match date format in database local and production might different

                            dsPatient = gClass.FetchData(cmdSelect, "tblPatient");

                            //check if patient exist
                            dsPatientExist = new DataSet();
                            gClass.MakeStoreProcedureName(ref cmdSelectExist, "sp_PatientData_LoadDataByNameDOB", true);
                            cmdSelectExist.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                            cmdSelectExist.Parameters.Add("@Surname", System.Data.SqlDbType.VarChar, 40).Value = surname;
                            cmdSelectExist.Parameters.Add("@Firstname", System.Data.SqlDbType.VarChar, 30).Value = firstname;
                            cmdSelectExist.Parameters.Add("@Birthdate", System.Data.SqlDbType.DateTime).Value = Convert.ToDateTime(dob);

                            dsPatientExist = gClass.FetchData(cmdSelectExist, "tblPatientExist");
                            if (dsPatientExist.Tables[0].Rows.Count > 0)
                            {
                                patientid = dsPatientExist.Tables[0].Rows[0]["PatientID"].ToString().Trim();
                                intPatientID = Int32.TryParse(patientid, out int32Temp) ? int32Temp : 0;
                            }

                            visitSeenByID = GetDrId(visitSeenBySurname, visitSeenByFirstname, visitSeenByCity);
                            
                            if(visitSeenByID.Trim() != "")
                            {
                                decResult = 0;
                                if (dsPatient.Tables[0].Rows.Count > 0)
                                {
                                    patientFound++;
                                    updateOrInsert = "update";
                                    prevVisitSeenBy = dsPatient.Tables[0].Rows[0]["DoctorName"].ToString().Trim();
                                    prevVisitWeight = dsPatient.Tables[0].Rows[0]["Weight"].ToString().Trim();
                                    prevVisitHeight = dsPatient.Tables[0].Rows[0]["Height"].ToString().Trim();
                                }
                                else
                                {
                                    updateOrInsert = "insert";
                                    patientNotFound++;
                                    notFoundNotes += "\n- Not Found (Try to insert): #" + recordNo + ", " + "Surname=" + surname + ", Firstname=" + firstname + ", Visit Date=" + visitDate;
                                }

                                gClass.MakeStoreProcedureName(ref cmdSave, "sp_ConsultFU1_ProgressNotes_InsertDataByDateSeen", true);


                                dtDateSeen = Convert.ToDateTime(visitDate);

                                cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Int32.TryParse(gClass.OrganizationCode, out int32Temp) ? int32Temp : 0;
                                cmdSave.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
                                cmdSave.Parameters.Add("@Surname", SqlDbType.VarChar, 40).Value = surname;
                                cmdSave.Parameters.Add("@Firstname", SqlDbType.VarChar, 30).Value = firstname;
                                cmdSave.Parameters.Add("@PatientID", SqlDbType.Int).Value = intPatientID;
                                cmdSave.Parameters.Add("@DateSeen", SqlDbType.DateTime).Value = dtDateSeen;
                                cmdSave.Parameters.Add("@Seenby", SqlDbType.Int).Value = Convert.ToInt32(visitSeenByID);

                                cmdSave.Parameters.Add("@Weight", SqlDbType.Decimal);
                                cmdSave.Parameters.Add("@BMIWeight", SqlDbType.Decimal);
                                if (visitWeight.Trim().Length == 0)
                                {
                                    decWeight = 0;
                                }
                                else if (Decimal.TryParse(visitWeight.Trim(), out decResult))
                                {
                                    decWeight = decResult;
                                }
                                else
                                {
                                    decWeight = 0;
                                }

                                cmdSave.Parameters["@Weight"].Value = decWeight;
                                cmdSave.Parameters["@BMIWeight"].Value = decWeight;


                                cmdSave.Parameters.Add("@Notes", SqlDbType.VarChar, 2048).Value = visitNotes;
                                cmdSave.Parameters.Add("@Height", SqlDbType.Decimal).Value = Decimal.TryParse(visitHeight, out decResult) ? decResult : 0;
                                cmdSave.Parameters.Add("@ReservoirVolume", SqlDbType.VarChar, 5).Value = "";
                                cmdSave.Parameters.Add("@CoMorbidityVisit", SqlDbType.Bit).Value = 0;
                                cmdSave.Parameters.Add("@VisitType", SqlDbType.SmallInt).Value = 1;

                                gClass.AddLogParameters(ref cmdSave, Request.Cookies["Logon_UserName"].Value, Request.Url.Host, updateOrInsert);

                                try
                                {
                                    if (patientid != "")
                                    {
                                        gClass.ExecuteDMLCommand(cmdSave);
                                        gClass.SaveUserLogFile(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Cookies["Logon_UserName"].Value,
                                                                Context.Request.Url.Host, "Import CSV Operation", 2, "Save CSV Data", "Pname: ", firstname + " " + surname);


                                        UpdateVisitDatesInPatientTable(intPatientID, dtDateSeen, decWeight);

                                        if (updateOrInsert == "update")
                                        {
                                            patientSuccessUpdate++;
                                            updateNotes += "\n- Update: #" + recordNo + ", " + "Surname=" + surname + ", Firstname=" + firstname + ", VisitDate=" + visitDate;
                                        }
                                        else
                                        {
                                            patientSuccessInsert++;
                                            insertNotes += "\n- Insert: #" + recordNo + ", " + "Surname=" + surname + ", Firstname=" + firstname + ", VisitDate=" + visitDate;
                                        }
                                    }
                                    else
                                    {
                                        patientNotExist++;
                                        patientNotExistNotes += "\n- Patient Not Exist: #" + recordNo + ", " + "Surname=" + surname + ", Firstname=" + firstname;

                                    }
                                }
                                catch (Exception err)
                                {
                                    patientFail++;
                                    failNotes += "\n- Failed " + updateOrInsert + ": #" + recordNo + ", " + "Surname=" + surname + ", Firstname=" + firstname + ", VisitDate=" + visitDate;
                                    // failed to insert test for which patient in which file name
                                    gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                                                Context.Request.Cookies["Logon_UserName"].Value, "Pname: " + firstname + " " + surname, "Save CSV Visit", err.ToString());
                                }

                            }
                            else
                            {
                                doctorNotFound++;
                                notFoundDoctorNotes += "\n- Not Found Doctor: #" + recordNo + ", " + "Doctor Surname=" + visitSeenBySurname + ", Doctor Firstname=" + visitSeenByFirstname;
                            }
                        }
                        recordNo++;
                    } while (streamReader.Peek() != -1);
                    resultNotes += "Consult found: " + patientFound;
                    resultNotes += "\n\nSuccess insert: " + patientSuccessInsert;
                    resultNotes += "\n" + insertNotes;
                    resultNotes += "\n\nSuccess update: " + patientSuccessUpdate;
                    resultNotes += "\n" + updateNotes;
                    resultNotes += "\n\nConsult not found: " + patientNotFound;
                    resultNotes += "\n" + notFoundNotes;
                    resultNotes += "\n\nDoctor not found: " + doctorNotFound;
                    resultNotes += "\n" + notFoundDoctorNotes;
                    resultNotes += "\n\nPatient not found: " + patientNotExist;
                    resultNotes += "\n" + patientNotExistNotes;
                    resultNotes += "\n\nFail: " + patientFail;
                    resultNotes += "\n" + failNotes;

                    resultNotes += "\nFail: " + patientFail;
                }
                else
                {
                    resultNotes = "Please enter a proper csv file";
                }
            }
            txtNotes.Value = "RESULT:\n\n" + resultNotes;
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                                                    Context.Request.Cookies["Logon_UserName"].Value, "Import CSV", "Import CSV", err.ToString());
        }
    }
    #endregion

    #region private void UpdateVisitDatesInPatientTable(Int32 intPatientID, DateTime dtDateSeen, Decimal decWeight)
    /*
     * this function is to update visit
     */
    private void UpdateVisitDatesInPatientTable(Int32 intPatientID, DateTime dtDateSeen, Decimal decWeight)
    {
        SqlCommand cmdUpdate = new SqlCommand();

        //1) first we check that whether the visit date is the last visit date or not
        gClass.MakeStoreProcedureName(ref cmdUpdate, "sp_ConsultFU1_ProgressNotes_IsTheLastVisitDate", true);

        cmdUpdate.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdUpdate.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
        cmdUpdate.Parameters.Add("@PatientId", SqlDbType.Int).Value = Convert.ToInt32(intPatientID);
        cmdUpdate.Parameters.Add("@VisitDate", SqlDbType.DateTime);

        try { cmdUpdate.Parameters["@VisitDate"].Value = Convert.ToDateTime(dtDateSeen); }
        catch { cmdUpdate.Parameters["@VisitDate"].Value = DBNull.Value; }
        DataSet dsTemp = gClass.FetchData(cmdUpdate, "tblVisit");
        Boolean flag = (dsTemp.Tables.Count > 0) && (dsTemp.Tables[0].Rows.Count > 0);

        if (!flag) // false means there is no visit date after the current visit date
        {
            cmdUpdate.Parameters.Clear();
            gClass.MakeStoreProcedureName(ref cmdUpdate, "sp_ConsultFU1_ProgressNotes_UpdateVisitDate", true);
            cmdUpdate.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
            cmdUpdate.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
            cmdUpdate.Parameters.Add("@PatientId", SqlDbType.Int).Value = Convert.ToInt32(intPatientID);
            cmdUpdate.Parameters.Add("@DateLastVisit", SqlDbType.DateTime);
            try { cmdUpdate.Parameters["@DateLastVisit"].Value = Convert.ToDateTime(dtDateSeen); }
            catch { cmdUpdate.Parameters["@DateLastVisit"].Value = DBNull.Value; }
            cmdUpdate.Parameters.Add("@DateNextVisit", SqlDbType.DateTime).Value = DBNull.Value;
           
            gClass.ExecuteDMLCommand(cmdUpdate);
            gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value, Request.Cookies["Logon_UserName"].Value, Request.Url.Host,
                                "Patient Visit Form", 2, "Update visit dates", "PatientID", Convert.ToString(intPatientID));

            gClass.SaveActionLog(gClass.OrganizationCode,
                                 Request.Cookies["UserPracticeCode"].Value,
                                 Request.Url.Host,
                                 System.Configuration.ConfigurationManager.AppSettings["VisitPage"].ToString(),
                                 System.Configuration.ConfigurationManager.AppSettings["LogUpdate"].ToString(),
                                 "Save " + System.Configuration.ConfigurationManager.AppSettings["VisitDate"].ToString(),
                                 Convert.ToString(intPatientID),
                                 "");

            //update pwd weight
            Decimal decResult = 0;
            SqlCommand cmdUpdatePWD = new SqlCommand();

            //sql to update current weight on pwd
            gClass.MakeStoreProcedureName(ref cmdUpdatePWD, "sp_ConsultFU1_PatientWeightData_UpdatePatientWeight", true);

            cmdUpdatePWD.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
            cmdUpdatePWD.Parameters.Add("@PatientId", SqlDbType.Int).Value = Convert.ToInt32(intPatientID);
            cmdUpdatePWD.Parameters.Add("@Weight", SqlDbType.Decimal).Value = decWeight;

            gClass.ExecuteDMLCommand(cmdUpdatePWD);
            gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value, Request.Cookies["Logon_UserName"].Value, Request.Url.Host,
                                "Patient Visit Form", 2, "Update Current Weight", "PatientID", Convert.ToString(intPatientID));

            gClass.SaveActionLog(gClass.OrganizationCode,
                                 Request.Cookies["UserPracticeCode"].Value,
                                 Request.Url.Host,
                                 System.Configuration.ConfigurationManager.AppSettings["VisitPage"].ToString(),
                                 System.Configuration.ConfigurationManager.AppSettings["LogUpdate"].ToString(),
                                 "Save " + System.Configuration.ConfigurationManager.AppSettings["PatientWeightData"].ToString(),
                                 Convert.ToString(intPatientID),
                                 "");
        }
        return;
    }
    #endregion

    #region private string CheckNull(Object oData)
    private string CheckNull(Object oData)
    {
        string returnString = "";

        if (oData != null)
            returnString = oData.ToString();

        return returnString;
    }
    #endregion

    #region private void FindControlRecursive(Control root, string id)
    private Control FindControlRecursive(Control root, string id)
    {
        if (root.ID == id)
        {
            return root;
        }

        foreach (Control c in root.Controls)
        {
            Control t = FindControlRecursive(c, id);
            if (t != null)
            {
                return t;
            }
        }
        return null;
    }
    #endregion

    #region protected void DownloadResult(object sender, EventArgs e)
    protected void DownloadResult(object sender, EventArgs e)
    {
        string reportFormat = "";
        string resultText = "";
        reportFormat = "doc";

        String now = DateTime.Now.ToString("yyyyMMddHHmmssffff");

        string fileName = "ErrorNotes_" + gClass.OrganizationCode + "_" + now + "_";
        string saveFilePath = Server.MapPath(".\\.\\..\\temp\\") + fileName + "." + reportFormat;
        string openFilePath = "..//temp//" + fileName + "." + reportFormat;

        StringBuilder sbTop = new System.Text.StringBuilder();
        sbTop.Append(@"
                    <html 
                    xmlns:o='urn:schemas-microsoft-com:office:office' 
                    xmlns:w='urn:schemas-microsoft-com:office:word'
                    xmlns='http://www.w3.org/TR/REC-html40'>
                    <head><title></title>

                    <!--[if gte mso 9]>
                    <xml>
                    <w:WordDocument>
                    <w:View>Print</w:View>
                    <w:Zoom>90</w:Zoom>
                    <w:DoNotOptimizeForBrowser/>
                    </w:WordDocument>
                    </xml>
                    <![endif]-->


                    <style>
                    p.MsoFooter, li.MsoFooter, div.MsoFooter
                    {
                    margin:0in;
                    margin-bottom:.0001pt;
                    mso-pagination:widow-orphan;
                    tab-stops:center 3.0in right 6.0in;
                    font-size:12.0pt;
                    }
                    <style>

                    <!-- /* Style Definitions */

                    @page Section1
                    {
                    margin:1.0in 0.5in 1.0in 0.5in ;
                    mso-header-margin:.5in;
                    mso-header:h1;
                    mso-footer: f1; 
                    mso-footer-margin:.5in;
                    }


                    div.Section1
                    {
                    page:Section1;
                    }

                    table#hrdftrtbl
                    {
                    margin:0in 0in 0in 9in;
                    }
                    -->
                    </style></head>

                    <body lang=EN-US style='tab-interval:.3in'>");

        resultText = txtNotes.Value.ToString();
        resultText = resultText.Replace("\r\n", "<br>");
        sbTop.Append(resultText);

        sbTop.Append(@"
                    <div class=Section1>
                    <tr>
                    <td>
                    <div style='mso-element:footer' id=f1>
                    <p class=MsoFooter>");
        //sbTop.Append(@"<span>" + demographicPatientNameFooter + "</span><span style='mso-tab-count:1'>&nbsp;</span>" + demographicConsutlationComplete+"<span style='mso-tab-count:1'>&nbsp;</span>page: <span style='mso-field-code: PAGE'></span>of <span style='mso-field-code: NUMPAGES '></span>");
        //sbTop.Append(@"<table width='100%'><tr><td width='40%' height='20'>" + demographicPatientNameFooter + "&nbsp;</td><td width='35%' align='center'>" + demographicConsutlationComplete + "&nbsp;</td><td width='25%' align='right'>page: <span style='mso-field-code: PAGE'></span>of <span style='mso-field-code: NUMPAGES '></span>&nbsp;</td></tr></table>");
        sbTop.Append(@"</p></div></td></tr>
                    </div>
                    </body></html>
                    ");

        string strBody = sbTop.ToString();
        Response.AppendHeader("Content-Type", "application/msword");
        Response.AppendHeader("Content-disposition", "attachment; filename=" + fileName + "." + reportFormat);
        Response.Write(strBody);

    }
    #endregion
}

