using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using Lapbase.Business;

public partial class Default2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnDisplay(object sender, EventArgs e)
    {
        GlobalClass gClass = new GlobalClass();
        gClass.OrganizationCode = Request.Cookies["OrganizationCode"].Value;

        String str = "<u> File: " + FileUpload1.FileName + "<u/><br/>";
        ArrayList fileArr = new ArrayList();
        ArrayList updateArr = new ArrayList();
        ArrayList addArr = new ArrayList();

        String patientID = "";
        String patientTitle;
        String patientFullname;
        String patientSurname;
        String patientFirstname;
        String patientAddress;
        String patientSuburb;
        String patientPostcode;
        String patientDOB;
        String patientHomePhone;
        String patientWorkPhone;
        String patientGender;
        ArrayList newPatientDetail = new ArrayList(); 

        String dbpatientID;

        Boolean successUpdate = true;


        if(FileUpload1.HasFile)
        {
            //get patient list
            System.Data.DataSet dsPatient = new System.Data.DataSet();
            System.Data.SqlClient.SqlCommand cmdSql = new System.Data.SqlClient.SqlCommand();

            cmdSql.CommandType = System.Data.CommandType.StoredProcedure;
            gClass.MakeStoreProcedureName(ref cmdSql, "sp_PatientsList_LoadAllPatient", true);
            //cmdSql.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.VarChar, 10).Value = gClass.OrganizationCode;
            cmdSql.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.VarChar, 10).Value = 34;
            gClass.FetchData(cmdSql, "tblPatients");
            dsPatient = gClass.dsGlobal;

            try
            {
                Stream stream = FileUpload1.FileContent;
                StreamReader reader = new StreamReader(stream);
                string strLine = "";
                while ((strLine = reader.ReadLine()) != null)
                {
                    patientID = strLine.Substring(0, 9).Trim();

                    if (fileArr.IndexOf(patientID) == -1)
                    {
                        patientTitle = strLine.Substring(9, 5).Trim();
                        patientSurname = strLine.Substring(14, 30).Trim();
                        patientFirstname = strLine.Substring(44, 30).Trim();
                        patientAddress = strLine.Substring(74, 40).Trim();
                        patientSuburb = strLine.Substring(114, 25).Trim();
                        patientPostcode = strLine.Substring(139, 4).Trim();
                        patientDOB = strLine.Substring(143, 10).Trim();
                        patientHomePhone = strLine.Substring(194, 14).Trim();
                        patientWorkPhone = strLine.Substring(208, 15).Trim();
                        patientGender = strLine.Substring(222, 16).Trim();
                        patientFullname = patientFirstname + patientSurname + patientDOB;

                        //add new detail to array
                        newPatientDetail.Add(patientID);
                        newPatientDetail.Add(patientTitle);
                        newPatientDetail.Add(patientSurname);
                        newPatientDetail.Add(patientFirstname);
                        newPatientDetail.Add(patientAddress);
                        newPatientDetail.Add(patientSuburb);
                        newPatientDetail.Add(patientPostcode);
                        newPatientDetail.Add(patientDOB);
                        newPatientDetail.Add(patientHomePhone);
                        newPatientDetail.Add(patientWorkPhone);
                        newPatientDetail.Add(patientGender);

                             
                        if (dsPatient.Tables[0].Select("[Patient MD Id]='" + patientID + "'").Length > 0)
                        {
                            //if there is any match in md id

                            DataRow[] drPatient = dsPatient.Tables[0].Select("[Patient MD Id]='" + patientID + "'");
                            dbpatientID = drPatient[0]["Patient Id"].ToString();

                            successUpdate = UpdatePatientData_DemoGraphics(dbpatientID, drPatient[0], newPatientDetail);

                            //if success, add to array, display in report
                            if (successUpdate == true)
                                updateArr.Add(patientID);
                        }
                        else if (dsPatient.Tables[0].Select("Fullname='" + patientFullname + "'").Length > 0)
                        {
                            //if there is any match in firstname and surname

                            DataRow[] drPatient = dsPatient.Tables[0].Select("Fullname='" + patientFullname + "'");
                            dbpatientID = drPatient[0]["Patient Id"].ToString();

                            successUpdate = UpdatePatientData_DemoGraphics(dbpatientID, drPatient[0], newPatientDetail);

                            //if success, add to array, display in report
                            if (successUpdate == true)
                                updateArr.Add(patientID);
                        }
                        else
                        {
                            DataTable dt = new DataTable();
                            //DataRow drow;
                            //drow = dt.NewRow();
                            DataRow drPatient = dt.NewRow();

                            successUpdate = UpdatePatientData_DemoGraphics("0", drPatient, newPatientDetail);

                            //if success, add to array, display in report
                            if (successUpdate == true)
                                updateArr.Add(patientID);
                        }

                        fileArr.Add(patientID);
                    }
                } 
            }

            catch( Exception ex)
            {
                str += "<br/><b>Error</b><br/>Unable to display "+ FileUpload1.FileName +"<br/>" + ex.Message;
            }
        }
        else
        {
            str = "No file uploaded.";
        }
        lblDisplay.Text = str;
        lblMessage.Text = "";
    }

    #region private void UpdatePatientData_DemoGraphics(String dbpatientID, DataRow drPatient, ArrayList newPatientDetail)
    private Boolean UpdatePatientData_DemoGraphics(String dbpatientID, DataRow drPatient, ArrayList newPatientDetail)
    {
        DateTimeFormatInfo dtInfo = new DateTimeFormatInfo();
        dtInfo.ShortDatePattern = @"dd/MM/yyyy";


        GlobalClass gClass = new GlobalClass();
        SqlCommand cmdSave = new SqlCommand();

        String newPatientID = newPatientDetail[0].ToString();
        String newPatientTitle = newPatientDetail[1].ToString();
        String newPatientSurname = newPatientDetail[2].ToString();
        String newPatientFirstname = newPatientDetail[3].ToString();
        String newPatientAddress = newPatientDetail[4].ToString();
        String newPatientSuburb = newPatientDetail[5].ToString();
        String newPatientPostcode = newPatientDetail[6].ToString();
        String newPatientDOB = newPatientDetail[7].ToString();
        String newPatientHomePhone = newPatientDetail[8].ToString();
        String newPatientWorkPhone = newPatientDetail[9].ToString();
        String newPatientGender = newPatientDetail[10].ToString();

        String strNameId = String.Empty;
        
        strNameId = (newPatientSurname.Length == 0) ? String.Empty : ((newPatientSurname.Length > 3) ? newPatientSurname.Substring(0, 4) : newPatientSurname);
        strNameId += (newPatientFirstname.Length == 0) ? String.Empty : newPatientFirstname.Substring(0, 1);

        if (strNameId.Equals(String.Empty))
        {
            //fail to insert-- need to return something
            return false;
        }

        if (dbpatientID == "0")
            gClass.MakeStoreProcedureName(ref cmdSave, "sp_PatientData_cmdInsert", true);
        else
            gClass.MakeStoreProcedureName(ref cmdSave, "sp_PatientData_cmdUpdate", true);

        cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(34);
        cmdSave.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(87);

        cmdSave.Parameters.Add("@Patient_MDId", SqlDbType.VarChar, 7).Value = newPatientID;
        cmdSave.Parameters.Add("@NameId", SqlDbType.VarChar, 7).Value = strNameId;
        cmdSave.Parameters.Add("@Surname", SqlDbType.VarChar, 40).Value = newPatientSurname.Replace("'", "`");
        cmdSave.Parameters.Add("@Firstname", SqlDbType.VarChar, 30).Value = newPatientFirstname.Replace("'", "`"); 
        cmdSave.Parameters.Add("@Title", SqlDbType.SmallInt).Value = Convert.ToInt32(1);
        cmdSave.Parameters.Add("@Street", SqlDbType.VarChar, 40).Value = newPatientAddress;
        cmdSave.Parameters.Add("@Suburb", SqlDbType.VarChar, 40).Value = newPatientSuburb;
        cmdSave.Parameters.Add("@State", SqlDbType.VarChar, 10).Value = "";
        cmdSave.Parameters.Add("@Postcode", SqlDbType.VarChar, 10).Value = newPatientPostcode;
        cmdSave.Parameters.Add("@HomePhone", SqlDbType.VarChar, 30).Value = newPatientHomePhone;
        cmdSave.Parameters.Add("@WorkPhone", SqlDbType.VarChar, 30).Value = newPatientWorkPhone;
        cmdSave.Parameters.Add("@Race", SqlDbType.VarChar, 3).Value = "";
        cmdSave.Parameters.Add("@Birthdate", SqlDbType.DateTime);

        if (newPatientDOB.Trim() == String.Empty)
            cmdSave.Parameters["@Birthdate"].Value = DBNull.Value;
        else
            try
            {
                if (Convert.ToDateTime(newPatientDOB, dtInfo) < DateTime.Now)
                    cmdSave.Parameters["@Birthdate"].Value = Convert.ToDateTime(newPatientDOB, dtInfo);
                else
                    cmdSave.Parameters["@Birthdate"].Value = DBNull.Value;
            }
            catch { cmdSave.Parameters["@Birthdate"].Value = DBNull.Value; }

        cmdSave.Parameters.Add("@Sex", SqlDbType.VarChar, 1).Value = newPatientGender;
        cmdSave.Parameters.Add("@DoctorId", SqlDbType.Int).Value = Convert.ToInt32(1);
        cmdSave.Parameters.Add("@RefDrId1", SqlDbType.VarChar, 10).Value = "";
        cmdSave.Parameters.Add("@RefDrId2", SqlDbType.VarChar, 10).Value = "";
        cmdSave.Parameters.Add("@RefDrId3", SqlDbType.VarChar, 10).Value = "";

        cmdSave.Parameters.Add("@MobilePhone", SqlDbType.VarChar, 30).Value = "";
        cmdSave.Parameters.Add("@EmailAddress", SqlDbType.VarChar, 100).Value = "";
        cmdSave.Parameters.Add("@Insurance", SqlDbType.VarChar, 50).Value = "";
        cmdSave.Parameters.Add("@Patient_CustomID", SqlDbType.VarChar, 20).Value = "";

        gClass.AddLogParameters(ref cmdSave, Request.Cookies["Logon_UserName"].Value, Request.Url.Host, (dbpatientID.Equals("0")) ? "insert" : "update");

        try
        {
            if (dbpatientID.Equals("0")) // means new Patient Data, data must be inserted
            {
                gClass.SavePatientData(1, cmdSave);
                //Context.Response.SetCookie(new HttpCookie("PatientID", dbpatientID));
                //gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value, Request.Cookies["Logon_UserName"].Value, Request.Url.Host,
                //                        "Baseline Form", 2, "Add Data", "PatientCode", Response.Cookies["PatientID"].Value);
            }
            else //data must be Updated
            {
                cmdSave.Parameters.Add("@PatientId", SqlDbType.Int).Value = Convert.ToInt64(dbpatientID);

                gClass.SavePatientData(2, cmdSave);
                //Context.Response.SetCookie(new HttpCookie("PatientID", dbpatientID));
                //gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value, Request.Cookies["Logon_UserName"].Value,
                //                        Request.Url.Host, "Baseline Form", 2, "Modify Data", "PatientCode", dbpatientID);
            }
            //success
            //IsDoneSaveFlag = 1;
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Baseline", "Data saving baseline", err.ToString());
            //IsDoneSaveFlag = 0;
        }
        cmdSave.Dispose();

        return true;
    }
    #endregion

}
