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

public partial class Forms_ImportExportData_UpdateBandType : System.Web.UI.Page
{
    GlobalClass gClass = new GlobalClass();
    Boolean ExportFlag = false;
    private long FileSize = 0;
    private string strDocumentName = "";
    private string strLapdataPath = "";
    private byte[] oDocumentByteArray;
    private int SrcFileType = 0;
    private string strSqlPWD = "SELECT * FROM PatientWeightData WHERE SurgeryType in ('1','11','12','13','14','15') ORDER BY [Patient ID];";
    private string strSqlOpEvents = "SELECT * FROM OpEvents WHERE SurgeryType in ('1','11','12','13','14','15') ORDER BY PatientID;";

    #region protected void Page_Load(object sender, EventArgs e)
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "no-cache");
        Response.Expires = -1;

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
                                            "Update Date Form", 2, "Browse", "", "");
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

    #region protected void checkBand(object sender, EventArgs e)
    protected void checkBand(object sender, EventArgs e)
    {
        ArrayList patientList = new ArrayList();        
        String prevPatientID = "";

        System.Data.SqlClient.SqlConnection cnnSource = new System.Data.SqlClient.SqlConnection();
        System.Data.SqlClient.SqlCommand cmdSource;
        System.Data.SqlClient.SqlDataReader drSource;
        System.Data.SqlClient.SqlDataReader drSourceValidate;
        cnnSource.ConnectionString = GlobalClass.strSqlCnnString;
        cnnSource.Open();
        String strRes = "", strSql_Source = String.Empty;
        cmdSource = cnnSource.CreateCommand();
        cmdSource.CommandType = CommandType.Text;

        //get all patient with adjustable surgery type and not empty band type
        cmdSource.CommandText = "SELECT OrganizationCode, PatientID, AdmitID, SurgeryType, BandType FROM tblOpEvents_Bold WHERE BandType is not null AND BandType != '' AND OrganizationCode = '" + Convert.ToInt32(gClass.OrganizationCode) + "'";


        strSql_Source = cmdSource.CommandText;
        drSource = cmdSource.ExecuteReader(CommandBehavior.Default);

        while (drSource.Read())
        {
            String tempOrganization = drSource.GetValue(0).ToString();
            String tempPatientID = drSource.GetValue(1).ToString();
            String tempAdmitID = drSource.GetValue(2).ToString();
            String tempSurgeryCode = drSource.GetValue(3).ToString();
            String tempBandType = drSource.GetValue(4).ToString();



            //take first found
            if (tempPatientID != prevPatientID)
            {
                /* print sql
                String updateCmd = "UPDATE tblOpEvents SET LapbandType = '" + tempBandType + "' ";
                updateCmd = updateCmd + "WHERE organizationcode = '" + tempOrganizationCode + "' AND patientid = '" + tempPatientID + "' ";
                updateCmd = updateCmd + "AND admitid = '" + tempAdmitID + "'";

                String updatePWDCmd = "UPDATE tblPatientWeightData SET LapbandType = '" + tempBandType + "' ";
                updatePWDCmd = updatePWDCmd + "WHERE organizationcode = '" + tempOrganizationCode + "' AND patientid = '" + tempPatientID + "' ";
                updatePWDCmd = updatePWDCmd + "AND surgeryType = '" + tempSurgeryCode + "'";
                */

                txtNotes.Value = txtNotes.Value + "Organization Code: " + tempOrganization + "\n";
                txtNotes.Value = txtNotes.Value + "Patient ID: " + tempPatientID + "\n";
                txtNotes.Value = txtNotes.Value + "Admit ID: " + tempAdmitID + "\n";
                txtNotes.Value = txtNotes.Value + "Surgery Type: " + tempSurgeryCode + "\n";
                txtNotes.Value = txtNotes.Value + "Band Type: " + tempBandType + "\n\n";

                /*
                txtNotes.Value = txtNotes.Value + "** " + updateCmd + "\n";
                txtNotes.Value = txtNotes.Value + "** " + updatePWDCmd + "\n\n";
                  */
            }
            prevPatientID = tempPatientID;
        }
        drSource.Close();
    }
    #endregion

    #region protected void updateBand(object sender, EventArgs e)
    protected void updateBand(object sender, EventArgs e)
    {
        ArrayList patientList = new ArrayList();
        String tempOrganization;
        String tempPatientID;
        String tempAdmitID;
        String tempSurgeryCode;
        String tempBandType;
        String prevPatientID = "";

        System.Data.SqlClient.SqlConnection cnnSource = new System.Data.SqlClient.SqlConnection();
        System.Data.SqlClient.SqlCommand cmdSource;
        System.Data.SqlClient.SqlDataReader drSource;
        System.Data.SqlClient.SqlDataReader drSourceValidate;
        cnnSource.ConnectionString = GlobalClass.strSqlCnnString;
        cnnSource.Open();
        String strRes = "", strSql_Source = String.Empty;
        cmdSource = cnnSource.CreateCommand();
        cmdSource.CommandType = CommandType.Text;

        SqlCommand cmdUpdate = new SqlCommand();

        //get all patient with adjustable surgery type and not empty band type
        cmdSource.CommandText = "SELECT OrganizationCode, PatientID, AdmitID, SurgeryType, BandType FROM tblOpEvents_Bold WHERE BandType is not null AND BandType != '' AND OrganizationCode = '" + Convert.ToInt32(gClass.OrganizationCode) + "'";
        
        strSql_Source = cmdSource.CommandText;
        drSource = cmdSource.ExecuteReader(CommandBehavior.Default);

        while (drSource.Read())
        {
            tempOrganization = drSource.GetValue(0).ToString();
            tempPatientID = drSource.GetValue(1).ToString();
            tempAdmitID = drSource.GetValue(2).ToString();
            tempSurgeryCode = drSource.GetValue(3).ToString();
            tempBandType = drSource.GetValue(4).ToString();
            

            //take first found
            if (tempPatientID != prevPatientID)
            {
                cmdUpdate = new SqlCommand();

                gClass.MakeStoreProcedureName(ref cmdUpdate, "sp_PatientData_UpdateBandType", true);
                cmdUpdate.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(tempOrganization);
                cmdUpdate.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(tempPatientID);
                cmdUpdate.Parameters.Add("@AdmitID", SqlDbType.Int).Value = Convert.ToInt32(tempAdmitID);
                cmdUpdate.Parameters.Add("@SurgeryType", SqlDbType.VarChar, 10).Value = tempSurgeryCode;
                cmdUpdate.Parameters.Add("@BandType", SqlDbType.VarChar, 10).Value = tempBandType;

                try
                {
                    gClass.ExecuteDMLCommand(cmdUpdate);
                    gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value, Request.Cookies["Logon_UserName"].Value,
                                                Request.Url.Host, "ImportExport", 2, "Update Band Type", "User Practice Code", Request.Cookies["UserPracticeCode"].Value);
                }
                catch (Exception err)
                {
                    txtNotes.Value = txtNotes.Value + "\nError in Updating Band Type\n";
                    gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "ImportExport", "Data saving bandtype", err.ToString());
                }

                txtNotes.Value = txtNotes.Value + "\nUpdate Band Type - " + Convert.ToInt32(gClass.OrganizationCode) + " - " + tempPatientID + " - " + tempAdmitID + " - " + tempSurgeryCode + " - " + tempBandType + " - Success\n";
                cmdUpdate.Dispose();
            }
            prevPatientID = tempPatientID;
        }
        drSource.Close();
    }
    #endregion



    #region protected void ImportFromMDB(object sender, EventArgs e)
    protected void ImportFromMDB(object sender, EventArgs e)
    {
        System.Data.OleDb.OleDbConnection cnnDest = new System.Data.OleDb.OleDbConnection();
        System.Data.OleDb.OleDbCommand cmdPWDDest;
        System.Data.OleDb.OleDbCommand cmdOpEventsDest;

        SqlCommand cmdUpdate = new SqlCommand();
        String tempPatientID;
        String tempAdmitID;
        String tempSurgeryCode = "BAA1061"; ;
        String tempBandType;
        String importNotes = "";
        strLapdataPath = txtFolder.Text;
        txtErrNotes.InnerHtml = "";

        int numOfRec = 0;
        String strMDBPath = strLapdataPath;

        try
        {
            if (strLapdataPath == "")
                txtErrNotes.InnerHtml = "Warning: Please enter the Lapdata folder path";

            cnnDest.ConnectionString = "PROVIDER=Microsoft.Jet.OLEDB.4.0;Data Source=" + strMDBPath;
            cnnDest.Open();
            cmdPWDDest = cnnDest.CreateCommand();
            cmdPWDDest.CommandType = CommandType.Text;
            cmdOpEventsDest = cnnDest.CreateCommand();
            cmdOpEventsDest.CommandType = CommandType.Text;

            cmdPWDDest.CommandText = strSqlPWD;
            OleDbDataReader drSource = cmdPWDDest.ExecuteReader(CommandBehavior.Default);

            //update pwd
            while (drSource.Read())
            {
                numOfRec++;
                cmdUpdate = new SqlCommand();
                tempPatientID = drSource["Patient Id"].ToString();
                tempBandType = checkBandType(drSource["SurgeryType"].ToString());
                
                gClass.MakeStoreProcedureName(ref cmdUpdate, "sp_PatientData_UpdatePWDBandTypeFromMDB", true);
                cmdUpdate.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                cmdUpdate.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(tempPatientID);
                cmdUpdate.Parameters.Add("@SurgeryType", SqlDbType.VarChar, 10).Value = tempSurgeryCode;
                cmdUpdate.Parameters.Add("@BandType", SqlDbType.VarChar, 10).Value = tempBandType;

                try
                {
                    gClass.ExecuteDMLCommand(cmdUpdate);
                    gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value, Request.Cookies["Logon_UserName"].Value,
                                                Request.Url.Host, "ImportExport", 2, "Update PWD Band Type From MDB", "User Practice Code", Request.Cookies["UserPracticeCode"].Value);
                }
                catch (Exception err)
                {
                    txtNotes.Value = txtNotes.Value + "\nError in Updating PWD Band Type From MDB\n";
                    gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "ImportExport", "Data saving PWD bandtype", err.ToString());
                }

                //importNotes = importNotes + drSource["Patient Id"].ToString() + " " + drSource["SurgeryType"].ToString() + "\n";
                //get new band code
                //update tblPWD set lapbandtype = xx where patientid = xx and organizationcode = xx and surgerytype = xx and (lapbandtype = empty or null)
            }
            lblPWDNumofRec.Text = numOfRec.ToString();
            numOfRec = 0;
            importNotes = importNotes + "\n\n";
            
            cmdOpEventsDest.CommandText = strSqlOpEvents;
            drSource = cmdOpEventsDest.ExecuteReader(CommandBehavior.Default);

            while (drSource.Read())
            {
                numOfRec++;
                cmdUpdate = new SqlCommand();
                tempPatientID = drSource["PatientId"].ToString();
                tempAdmitID = drSource["AdmitId"].ToString();
                tempBandType = checkBandType(drSource["SurgeryType"].ToString());

                gClass.MakeStoreProcedureName(ref cmdUpdate, "sp_PatientData_UpdateOpBandTypeFromMDB", true);
                cmdUpdate.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                cmdUpdate.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(tempPatientID);
                cmdUpdate.Parameters.Add("@AdmitID", SqlDbType.Int).Value = Convert.ToInt32(tempAdmitID);
                cmdUpdate.Parameters.Add("@SurgeryType", SqlDbType.VarChar, 10).Value = tempSurgeryCode;
                cmdUpdate.Parameters.Add("@BandType", SqlDbType.VarChar, 10).Value = tempBandType;

                try
                {
                    gClass.ExecuteDMLCommand(cmdUpdate);
                    gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value, Request.Cookies["Logon_UserName"].Value,
                                                Request.Url.Host, "ImportExport", 2, "Update OpEvents Band Type From MDB", "User Practice Code", Request.Cookies["UserPracticeCode"].Value);
                }
                catch (Exception err)
                {
                    txtNotes.Value = txtNotes.Value + "\nError in Updating OpEvents Band Type From MDB\n";
                    gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "ImportExport", "Data saving PWD bandtype", err.ToString());
                }


                //importNotes = importNotes + drSource["PatientId"].ToString() + " " + drSource["SurgeryType"].ToString() + "\n";
                
                //get new band code
                //update tblOpEvents set lapbandtype = xx where patientid = xx and organizationcode = xx and surgerytype = xx and admitid = xx and (lapbandtype = empty or null)
            }

            txtNotes.Value = txtNotes.Value + importNotes;
            lblOpEventsNumofRec.Text = numOfRec.ToString();
        }
        catch (Exception err)
        {
            ExportFlag = false;
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host,
                    Request.Cookies["Logon_UserName"].Value, "Import/Export data Form", "CheckVisitRecord function",
                    "SELECT SurgeryType ... " + err.ToString());
        }
    }
    #endregion

    #region private String checkBandType(str tempSurgeryCode)
    private String checkBandType(string tempSurgeryCode)
    {
        String tempBandType = "";
        switch (tempSurgeryCode)
        {
            case "1": //LAGB - Lapband
                tempBandType = "BAA1001";
                break;
            case "11": //LAGB - SAGB
                tempBandType = "BAA1002";
                break;
            case "12": //LAGB - other
                tempBandType = "BAA1008";
                break;
            case "13": //LAGB - AMI Band
                tempBandType = "BAA1003";
                break;
            case "14": //LAGB - Soft gastric band
                tempBandType = "BAA1004";
                break;
            case "15": //LAGB - Vangard Band
                tempBandType = "BAA1005";
                break;
        }
        return tempBandType;
    }
    #endregion
}