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


public partial class Forms_ImportExportData_ExportBold : System.Web.UI.Page
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

    #region protected void ExportBold(object sender, EventArgs e)
    protected void ExportBold(object sender, EventArgs e)
    {
        try
        {
            SqlCommand cmdSelect = new SqlCommand();
            DataSet dsResult;

            int patientID = 0;
            int organizationCode = 0;
            int admitID =0;

            patientID = Convert.ToInt32(txtPatientID.Value);
            organizationCode = Convert.ToInt32(gClass.OrganizationCode);
            admitID = Convert.ToInt32(txtDetailID.Value);

            string boldVendorCode = "C52CEF57-268F-4DFD-BE8D-9D66E00CF3D3";
            string boldPracticeCOEID = "2000DBB";
            string boldStartXML = "";
            string tempBoldResult = "";
            string boldResult = "";
            string boldRequest = "";
            string boldRequestDetail = "";

            string boldPatPatient = "Patient";
            string boldPatSavePatRequest = "SavePatientRequest";
            string boldPatChartNumber = "";
            string boldPatCharity = "";
            string boldPatConsentReceived = "";
            string boldPatDeceased = "";
            string boldPatDOB = "";
            string boldPatEmployer = "";
            string boldPatFirstName = "";
            string boldPatLastName = "";
            string boldPatInsuranceCoversProcedure = "";
            string boldPatMiddleInitial = "";
            string boldPatSelfPay = "";
            string boldPatSuffix = "";
            string boldPatGenderCode = "";
            string boldPatEmploymentStatusCode = "";
            string boldPatRaceCodes = "";
            string[] tempBoldPatPatientInsurance;
            string boldPatPatientInsuranceCodes = "";
            string boldPatSavePatRequestResult = "";


            string boldHosHospitalVisit = "HospitalVisit";
            string boldHosSaveHospitalVisitRequest = "SaveHospitalVisitRequest";
            string boldHosSurgeryDate = "";
            string boldHosDateOfAdmission = "";
            string boldHosDateOfLastWeight = "";
            string boldHosRevision = "";
            string boldHosFacilityCOEID = "";
            string boldHosSurgeonCOEID = "";
            string boldHosDurationOfSurgery = "";
            string boldHosDurationOfAnesthesia = "";
            string boldHosEstimatedBloodLossInCC = "";
            string boldHosBloodTransfusionInUnits = "";
            string boldHosLastWeightBeforeSurgery = "";
            string boldHosSurgicalResidentParticipated = "";
            string boldHosSurgicalFellowParticipated = "";
            string boldHosDischargeDate = "";
            string boldHosDischargeLocationCode = "";
            string boldHosASAClassificationCode = "";
            string boldHosBariatricProcedureCode = "";
            string boldHosLastWeightBeforeSurgeryUnitType = "";
            string boldHosLastWeightBeforeSurgeryMetricValue = "";
            string boldHosLastWeightBeforeSurgeryEstimated = "";
            

            if (txtType.Value == "p")
            {
                gClass.MakeStoreProcedureName(ref cmdSelect, "sp_SRCPatientDataGet", false);
                cmdSelect.Parameters.Add("@vintOrganizationCode", SqlDbType.Int).Value = organizationCode;
                cmdSelect.Parameters.Add("@vintPatientId", SqlDbType.Int).Value = patientID;
                dsResult = gClass.FetchData(cmdSelect, "tblResult");

                if ((dsResult.Tables.Count > 0) && (dsResult.Tables[0].Rows.Count > 0))
                {
                    boldRequest = boldPatPatient;
                    boldRequestDetail = boldPatSavePatRequest;

                    //split insurance code
                    tempBoldPatPatientInsurance = dsResult.Tables[0].Rows[0]["PatientInsuranceCodes"].ToString().Split(',');
                    foreach (string tempPatientInsurance in tempBoldPatPatientInsurance)
                    {
                        boldPatPatientInsuranceCodes += "<string>" + tempPatientInsurance + "</string>";
                    }

                    boldPatChartNumber = dsResult.Tables[0].Rows[0]["ChartNumber"].ToString();


                    boldPatCharity = "<Charity>" + dsResult.Tables[0].Rows[0]["Charity"].ToString() + "</Charity>";
                    boldPatConsentReceived = "<ConsentRecieved>" + dsResult.Tables[0].Rows[0]["ConsentRecieved"].ToString() + "</>";
                    //boldPatDeceased = "<Deceased>" + Result.Tables[0].Rows[0]["Deceased"].ToString() + "</Deceased>";
                    boldPatDOB = "<DOB>" + dsResult.Tables[0].Rows[0]["YearOfBirth"].ToString() + "</DOB>";
                    boldPatEmployer = "<Employer>" + dsResult.Tables[0].Rows[0]["Employer"].ToString() + "</Employer>";
                    boldPatFirstName = "<FirstName>" + dsResult.Tables[0].Rows[0]["FirstName"].ToString() + "</FirstName>";
                    boldPatLastName = "<LastName>" + dsResult.Tables[0].Rows[0]["LastName"].ToString() + "</LastName>";
                    boldPatInsuranceCoversProcedure = "<InsuranceCoversProcedure>" + dsResult.Tables[0].Rows[0]["InsuranceCoversProcedure"].ToString() + "</InsuranceCoversProcedure>";
                    boldPatMiddleInitial = "<MiddleInitial>" + dsResult.Tables[0].Rows[0]["MiddleName"].ToString() + "</MiddleInitial>";
                    boldPatSelfPay = "<SelfPay>" + dsResult.Tables[0].Rows[0]["SelfPay"].ToString() + "</SelfPay>";
                    boldPatSuffix = "<Suffix>" + dsResult.Tables[0].Rows[0]["Suffix"].ToString() + "</Suffix>";
                    boldPatGenderCode = "<GenderCode>" + dsResult.Tables[0].Rows[0]["GenderCode"].ToString() + "</GenderCode>";
                    boldPatEmploymentStatusCode = "<EmploymentStatusCode>" + dsResult.Tables[0].Rows[0]["EmploymentStatusCode"].ToString() + "</EmploymentStatusCode>";
                    boldPatRaceCodes = "<RaceCodes><string>" + dsResult.Tables[0].Rows[0]["RACE"].ToString() + "</string></RaceCodes>";
                    boldPatPatientInsuranceCodes = "<PatientInsuranceCodes>" + boldPatPatientInsuranceCodes + "</PatientInsuranceCodes>";

                    tempBoldResult =  boldPatCharity + boldPatConsentReceived + boldPatDOB + boldPatEmployer + boldPatFirstName + boldPatLastName + boldPatInsuranceCoversProcedure + boldPatMiddleInitial + boldPatSelfPay + boldPatSuffix + boldPatGenderCode + boldPatEmploymentStatusCode + boldPatRaceCodes + boldPatPatientInsuranceCodes;
                }
            }

            if (txtType.Value == "o")
            {
                gClass.MakeStoreProcedureName(ref cmdSelect, "sp_SRCHospitalVisitGetSingle", false);
                cmdSelect.Parameters.Add("@vintOrganizationCode", SqlDbType.Int).Value = organizationCode;
                cmdSelect.Parameters.Add("@vintPatientId", SqlDbType.Int).Value = patientID;
                cmdSelect.Parameters.Add("@vintAdmitID", SqlDbType.Int).Value = admitID;
                dsResult = gClass.FetchData(cmdSelect, "tblResult");

                if ((dsResult.Tables.Count > 0) && (dsResult.Tables[0].Rows.Count > 0))
                {
                    boldRequest = boldHosHospitalVisit;
                    boldRequestDetail = boldHosSaveHospitalVisitRequest;
                    
                    
                    boldHosSurgeryDate = "<SurgeryDate>"+dsResult.Tables[0].Rows[0]["SurgeryDate"].ToString()+"</SurgeryDate>";
                    boldHosDateOfAdmission = "<DateOfAdmission>"+dsResult.Tables[0].Rows[0]["AdmitDate"].ToString()+"</DateOfAdmission>";
                    boldHosDateOfLastWeight = "<DateOfLastWeight>"+dsResult.Tables[0].Rows[0]["DateOfLastWeight"].ToString()+"</DateOfLastWeight>";
                    //boldHosRevision = "<Revision>"+dsResult.Tables[0].Rows[0][""].ToString()+"</Revision>";
                    boldHosFacilityCOEID = "<FacilityCOEID>"+dsResult.Tables[0].Rows[0]["HospitalBoldCode"].ToString()+"</FacilityCOEID>";
                    boldHosSurgeonCOEID = "<SurgeonCOEID>"+dsResult.Tables[0].Rows[0]["DoctorBoldCode"].ToString()+"</SurgeonCOEID>";
                    boldHosDurationOfSurgery = "<DurationOfSurgery>"+dsResult.Tables[0].Rows[0]["Duration"].ToString()+"</DurationOfSurgery>";
                    boldHosDurationOfAnesthesia = "<DurationOfAnesthesia>"+dsResult.Tables[0].Rows[0]["DurationOfAnesthesia"].ToString()+"</DurationOfAnesthesia>";
                    //boldHosEstimatedBloodLossInCC = "<EstimatedBloodLossInCC>"+dsResult.Tables[0].Rows[0][""].ToString()+"</EstimatedBloodLossInCC>";
                    boldHosBloodTransfusionInUnits = "<BloodTransfusionInUnits>" + dsResult.Tables[0].Rows[0]["BloodTransfusionInUnits"].ToString() + "</BloodTransfusionInUnits>";
                    boldHosLastWeightBeforeSurgery = "<LastWeightBeforeSurgery>"+dsResult.Tables[0].Rows[0]["LastWeightBeforeSurgery"].ToString()+"</LastWeightBeforeSurgery>";
                    boldHosSurgicalResidentParticipated = "<SurgicalResidentParticipated>" + dsResult.Tables[0].Rows[0]["SurgicalResidentParticipated"].ToString() + "</SurgicalResidentParticipated>";
                    boldHosSurgicalFellowParticipated = "<SurgicalFellowParticipated>" + dsResult.Tables[0].Rows[0]["SurgicalFellowParticipated"].ToString() + "</SurgicalFellowParticipated>";
                    boldHosDischargeDate = "<DischargeDate>" + dsResult.Tables[0].Rows[0]["DischargeDate"].ToString() + "</DischargeDate>";
                    boldHosDischargeLocationCode = "<DischargeLocationCode>" + dsResult.Tables[0].Rows[0]["DischargeLocationCode"].ToString() + "</DischargeLocationCode>";
                    boldHosASAClassificationCode = "<ASAClassificationCode>" + dsResult.Tables[0].Rows[0]["ASAClassificationCode"].ToString() + "</ASAClassificationCode>";
                    boldHosBariatricProcedureCode = "<BariatricProcedureCode>" + dsResult.Tables[0].Rows[0]["BariatricProcedureCode"].ToString() + "</BariatricProcedureCode>";
                    //boldHosLastWeightBeforeSurgeryUnitType = "<UnitType>"+dsResult.Tables[0].Rows[0][""].ToString()+"</UnitType>";
                    //boldHosLastWeightBeforeSurgeryMetricValue = "<MetricValue>"+dsResult.Tables[0].Rows[0][""].ToString()+"</MetricValue>";
                    //boldHosLastWeightBeforeSurgeryEstimated = "<Estimated>"+"</Estimated>";

                    tempBoldResult = boldHosSurgeryDate + boldHosDateOfAdmission + boldHosDateOfLastWeight + boldHosFacilityCOEID + boldHosSurgeonCOEID + boldHosDurationOfSurgery + boldHosDurationOfAnesthesia + boldHosEstimatedBloodLossInCC + boldHosBloodTransfusionInUnits + boldHosLastWeightBeforeSurgery + boldHosSurgicalResidentParticipated + boldHosSurgicalFellowParticipated + boldHosDischargeDate + boldHosDischargeLocationCode + boldHosASAClassificationCode + boldHosBariatricProcedureCode;
          
                }
            }

            boldStartXML = "<?xml version=\"1.0\" encoding=\"utf-8\"?><" + boldRequestDetail + " xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><SecurityToken xmlns=\"http://www.surgicalreview.org/Bold\" /><Version xmlns=\"http://www.surgicalreview.org/Bold\" /><RequestId xmlns=\"http://www.surgicalreview.org/Bold\" /><VendorCode xmlns=\"http://www.surgicalreview.org/Bold\">" + boldVendorCode + "</VendorCode><PracticeCOEID xmlns=\"http://www.surgicalreview.org/Bold\">" + boldPracticeCOEID + "</PracticeCOEID><PatientChartNumber xmlns=\"http://www.surgicalreview.org/Bold\">" + boldPatChartNumber + "</PatientChartNumber>" + "<" + boldRequest + " xmlns=\"http://www.surgicalreview.org/Bold\">";
            boldResult += boldStartXML + tempBoldResult + "</" + boldRequest + ">" + "</" + boldRequestDetail + ">";

            txtNotes.Value = boldResult;
            







            /*
            string FileControl = "textFile";
            string filePath = "";

            int patientSuccessUpdate = 0;
            int patientSuccessInsert = 0;
            int patientFail = 0;
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
                        SqlCommand cmdSelect = new SqlCommand();
                        SqlCommand cmdSelectLast = new SqlCommand();
                        SqlCommand cmdSave = new SqlCommand();
                        SqlCommand cmdSaveEMR = new SqlCommand();

                        message = streamReader.ReadLine();
                        patientDetails = message.Split(',');

                        matchingPatientID = 0;
                        surname = patientDetails[0];
                        firstname = patientDetails[1];
                        dob = patientDetails[2];
                        gender = patientDetails[3];
                        family = patientDetails[4];
                        street = patientDetails[5];
                        city = patientDetails[6];
                        state = patientDetails[7];
                        postcode = patientDetails[8];
                        email = patientDetails[9];
                        homephone = patientDetails[10];
                        workphone = patientDetails[11];
                        mobilephone = patientDetails[12];

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
                        cmdSave.Parameters.Add("@RefDrId1", SqlDbType.VarChar, 10).Value = DBNull.Value;
                        cmdSave.Parameters.Add("@RefDrId2", SqlDbType.VarChar, 10).Value = DBNull.Value;
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

                        gClass.AddLogParameters(ref cmdSave, Request.Cookies["Logon_UserName"].Value, Request.Url.Host, (matchingPatientID == 0) ? "insert" : "update");

                        //add emr data (family structure)
                        try
                        {
                            gClass.ExecuteDMLCommand(cmdSave);
                            gClass.SaveUserLogFile(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Cookies["Logon_UserName"].Value,
                                                    Context.Request.Url.Host, "Import CSV", 2, "Save CSV Data", "PID:", matchingPatientID.ToString());

                            family = UppercaseFirst(family);
                            if (family.ToUpper() == "S" || family.ToUpper() == "M" || family.ToUpper() == "D" || family.ToUpper() == "SP" || family.ToUpper() == "P")
                            {
                                try
                                {
                                    if (matchingPatientID > 0)
                                    {
                                        //if it is an update
                                        patientID = matchingPatientID;
                                    }
                                    else
                                    {
                                        //if it is an insert
                                        DataSet dsPatientEMR = new DataSet();
                                        gClass.MakeStoreProcedureName(ref cmdSelectLast, "sp_PatientData_LastPatientID", true);
                                        dsPatientEMR = gClass.FetchData(cmdSelectLast, "tblPatient");
                                        if (dsPatientEMR.Tables[0].Rows.Count > 0)
                                        {
                                            patientID = Convert.ToInt32(dsPatientEMR.Tables[0].Rows[0]["Patient ID"]);
                                        }
                                    }

                                    gClass.MakeStoreProcedureName(ref cmdSaveEMR, "sp_PatientEMR_cmdInsertDetails", true);

                                    cmdSaveEMR.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                                    cmdSaveEMR.Parameters.Add("@PatientId", SqlDbType.Int).Value = Convert.ToInt64(patientID);
                                    cmdSaveEMR.Parameters.Add("@PageNo", SqlDbType.Int).Value = 1;
                                    cmdSaveEMR.Parameters.Add("@Details_FamilyStructure", SqlDbType.VarChar, 50).Value = family;

                                    gClass.ExecuteDMLCommand(cmdSaveEMR);
                                }
                                catch (Exception err)
                                {
                                    gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                                        Context.Request.Cookies["Logon_UserName"].Value, "PID : " + matchingPatientID.ToString(), "Save CSV Data EMR", err.ToString());
                                }
                            }
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
             */
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                                                    Context.Request.Cookies["Logon_UserName"].Value, "Import CSV", "Import CSV", err.ToString());
        }
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
}

