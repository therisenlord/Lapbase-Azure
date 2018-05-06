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

public partial class Forms_ImportExportData_UpdateDateSeen : System.Web.UI.Page
{
    GlobalClass gClass = new GlobalClass();

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

    #region protected void checkDate(object sender, EventArgs e)
    protected void checkDate(object sender, EventArgs e)
    {
        ArrayList patientList = new ArrayList();

        System.Data.SqlClient.SqlConnection cnnSource = new System.Data.SqlClient.SqlConnection();
        System.Data.SqlClient.SqlCommand cmdSource;
        System.Data.SqlClient.SqlDataReader drSource;
        System.Data.SqlClient.SqlDataReader drSourceValidate;
        cnnSource.ConnectionString = GlobalClass.strSqlCnnString;
        cnnSource.Open();
        String strRes = "", strSql_Source = String.Empty;
        cmdSource = cnnSource.CreateCommand();
        cmdSource.CommandType = CommandType.Text;
        cmdSource.CommandText = "SELECT * FROM tblPatients WHERE OrganizationCode = '" + gClass.OrganizationCode + "' order by [patient id]";

        String validateDate = "SELECT top 1 patientPatientID, surname,firstname, patientLastVisit, visitDateSeen, ";
        validateDate += "case when patientLastVisit = visitDateSeen then '1' else '0' end as status ";
        validateDate += "FROM ";
        validateDate += "(select [Patient Id] as patientPatientID, [Date Last Visit] as patientLastVisit, organizationcode, firstname,surname from tblPatients where organizationCode = '" + gClass.OrganizationCode + "') tablePatient, ";
        validateDate += "(select [Patient Id] as visitPatientID, [DateSeen] as visitDateSeen, organizationcode from tblPatientConsult where OrganizationCode ='" + gClass.OrganizationCode + "' and datedeleted is null) tablePatientVisit ";
        validateDate += "WHERE patientPatientID = visitPatientID and tablePatientVisit.organizationcode = '" + gClass.OrganizationCode + "' and tablePatient.organizationcode='" + gClass.OrganizationCode + "'";


        strSql_Source = cmdSource.CommandText;
        drSource = cmdSource.ExecuteReader(CommandBehavior.Default);

        while (drSource.Read())
        {
            patientList.Add(drSource.GetValue(2).ToString());
        }
        drSource.Close();
        
        
        foreach(string tempPatientID in patientList)
        {
            cmdSource.CommandText = validateDate + "AND patientPatientID ='" + tempPatientID + "' ORDER BY visitdateseen desc";
            drSource = cmdSource.ExecuteReader(CommandBehavior.Default);
            
            while (drSource.Read())
            {
                if (drSource.GetValue(3).ToString() != drSource.GetValue(4).ToString())
                    strRes += drSource.GetValue(0).ToString()+" - "+ drSource.GetValue(1).ToString()+ " - "+ drSource.GetValue(2).ToString() +"\n";
            }            
            drSource.Close();
        }
        //update notes
        txtNotes.Value = txtNotes.Value + strRes;

        drSource.Close();
    }
    #endregion

    #region protected void updateDate(object sender, EventArgs e)
    protected void updateDate(object sender, EventArgs e)
    {
        SqlCommand cmdUpdate = new SqlCommand();

        gClass.MakeStoreProcedureName(ref cmdUpdate, "sp_PatientData_UpdateVisitDate", true);
        cmdUpdate.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);


        ArrayList patientList = new ArrayList();

        System.Data.SqlClient.SqlConnection cnnSource = new System.Data.SqlClient.SqlConnection();
        System.Data.SqlClient.SqlCommand cmdSource;
        System.Data.SqlClient.SqlDataReader drSource;
        System.Data.SqlClient.SqlDataReader drSourceValidate;
        cnnSource.ConnectionString = GlobalClass.strSqlCnnString;
        cnnSource.Open();
        String strRes = "", strSql_Source = String.Empty;
        cmdSource = cnnSource.CreateCommand();
        cmdSource.CommandType = CommandType.Text;
        cmdSource.CommandText = "SELECT * FROM tblPatients WHERE OrganizationCode = '" + gClass.OrganizationCode + "' order by [patient id]";

        String validateDate = "SELECT top 1 patientPatientID, surname,firstname, patientLastVisit, visitDateSeen, ";
        validateDate += "case when patientLastVisit = visitDateSeen then '1' else '0' end as status ";
        validateDate += "FROM ";
        validateDate += "(select [Patient Id] as patientPatientID, [Date Last Visit] as patientLastVisit, organizationcode, firstname,surname from tblPatients where organizationCode = '" + gClass.OrganizationCode + "') tablePatient, ";
        validateDate += "(select [Patient Id] as visitPatientID, [DateSeen] as visitDateSeen, organizationcode from tblPatientConsult where OrganizationCode ='" + gClass.OrganizationCode + "' and datedeleted is null) tablePatientVisit ";
        validateDate += "WHERE patientPatientID = visitPatientID and tablePatientVisit.organizationcode = '" + gClass.OrganizationCode + "' and tablePatient.organizationcode='" + gClass.OrganizationCode + "'";


        strSql_Source = cmdSource.CommandText;
        drSource = cmdSource.ExecuteReader(CommandBehavior.Default);

        while (drSource.Read())
        {
            patientList.Add(drSource.GetValue(2).ToString());
        }
        drSource.Close();


        foreach (string tempPatientID in patientList)
        {
            cmdUpdate = new SqlCommand();

            gClass.MakeStoreProcedureName(ref cmdUpdate, "sp_PatientData_UpdateVisitDate", true);
            cmdUpdate.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
            cmdUpdate.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(tempPatientID);





            cmdSource.CommandText = validateDate + "AND patientPatientID ='" + tempPatientID + "' ORDER BY visitdateseen desc";
            drSource = cmdSource.ExecuteReader(CommandBehavior.Default);

            while (drSource.Read())
            {
                if (drSource.GetValue(3).ToString() != drSource.GetValue(4).ToString())
                {
                    gClass.ExecuteDMLCommand(cmdUpdate);                    
                }
            }
            drSource.Close();
        }
        drSource.Close();
    }
    #endregion


    #region protected void checkFirstVisitDate(object sender, EventArgs e)
    protected void checkFirstVisitDate(object sender, EventArgs e)
    {
        ArrayList patientList = new ArrayList();

        System.Data.SqlClient.SqlConnection cnnSource = new System.Data.SqlClient.SqlConnection();
        System.Data.SqlClient.SqlCommand cmdSource;
        System.Data.SqlClient.SqlDataReader drSource;
        System.Data.SqlClient.SqlDataReader drSourceValidate;
        cnnSource.ConnectionString = GlobalClass.strSqlCnnString;
        cnnSource.Open();
        String strRes = "", strSql_Source = String.Empty;
        cmdSource = cnnSource.CreateCommand();
        cmdSource.CommandType = CommandType.Text;
        cmdSource.CommandText = "SELECT * FROM tblPatients WHERE OrganizationCode = '" + gClass.OrganizationCode + "' order by [patient id]";

        String validateDate = "SELECT top 1 patientPatientID, surname,firstname, patientFirstVisit, visitDateSeen, ";
        validateDate += "case when patientFirstVisit = visitDateSeen then '1' else '0' end as status ";
        validateDate += "FROM ";
        validateDate += "(select [Patient Id] as patientPatientID, [Date First Visit] as patientFirstVisit, organizationcode, firstname,surname from tblPatients where organizationCode = '" + gClass.OrganizationCode + "') tablePatient, ";
        validateDate += "(select [Patient Id] as visitPatientID, [DateSeen] as visitDateSeen, organizationcode from tblPatientConsult where OrganizationCode ='" + gClass.OrganizationCode + "' and datedeleted is null) tablePatientVisit ";
        validateDate += "WHERE patientPatientID = visitPatientID and tablePatientVisit.organizationcode = '" + gClass.OrganizationCode + "' and tablePatient.organizationcode='" + gClass.OrganizationCode + "'";


        strSql_Source = cmdSource.CommandText;
        drSource = cmdSource.ExecuteReader(CommandBehavior.Default);

        while (drSource.Read())
        {
            patientList.Add(drSource.GetValue(2).ToString());
        }
        drSource.Close();


        foreach (string tempPatientID in patientList)
        {
            cmdSource.CommandText = validateDate + "AND patientPatientID ='" + tempPatientID + "' ORDER BY visitdateseen asc";
            drSource = cmdSource.ExecuteReader(CommandBehavior.Default);

            while (drSource.Read())
            {
                if (drSource.GetValue(3).ToString() != drSource.GetValue(4).ToString())
                    strRes += drSource.GetValue(0).ToString() + " - " + drSource.GetValue(1).ToString() + " - " + drSource.GetValue(2).ToString() + " - recorded:" + drSource.GetValue(3).ToString()+ " - actual: " + drSource.GetValue(4).ToString() +"\n";
            }
            drSource.Close();
        }
        //update notes
        txtNotes.Value = txtNotes.Value + strRes;

        drSource.Close();
    }
    #endregion

    #region protected void updateFirstVisitDate(object sender, EventArgs e)
    protected void updateFirstVisitDate(object sender, EventArgs e)
    {
        SqlCommand cmdUpdate = new SqlCommand();

        gClass.MakeStoreProcedureName(ref cmdUpdate, "sp_PatientData_UpdateVisitDate", true);
        cmdUpdate.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);


        ArrayList patientList = new ArrayList();

        System.Data.SqlClient.SqlConnection cnnSource = new System.Data.SqlClient.SqlConnection();
        System.Data.SqlClient.SqlCommand cmdSource;
        System.Data.SqlClient.SqlDataReader drSource;
        System.Data.SqlClient.SqlDataReader drSourceValidate;
        cnnSource.ConnectionString = GlobalClass.strSqlCnnString;
        cnnSource.Open();
        String strRes = "", strSql_Source = String.Empty;
        cmdSource = cnnSource.CreateCommand();
        cmdSource.CommandType = CommandType.Text;
        cmdSource.CommandText = "SELECT * FROM tblPatients WHERE OrganizationCode = '" + gClass.OrganizationCode + "' order by [patient id]";

        String validateDate = "SELECT top 1 patientPatientID, surname,firstname, patientFirstVisit, visitDateSeen, ";
        validateDate += "case when patientFirstVisit = visitDateSeen then '1' else '0' end as status ";
        validateDate += "FROM ";
        validateDate += "(select [Patient Id] as patientPatientID, [Date First Visit] as patientFirstVisit, organizationcode, firstname,surname from tblPatients where organizationCode = '" + gClass.OrganizationCode + "') tablePatient, ";
        validateDate += "(select [Patient Id] as visitPatientID, [DateSeen] as visitDateSeen, organizationcode from tblPatientConsult where OrganizationCode ='" + gClass.OrganizationCode + "' and datedeleted is null) tablePatientVisit ";
        validateDate += "WHERE patientPatientID = visitPatientID and tablePatientVisit.organizationcode = '" + gClass.OrganizationCode + "' and tablePatient.organizationcode='" + gClass.OrganizationCode + "'";


        strSql_Source = cmdSource.CommandText;
        drSource = cmdSource.ExecuteReader(CommandBehavior.Default);

        while (drSource.Read())
        {
            patientList.Add(drSource.GetValue(2).ToString());
        }
        drSource.Close();


        foreach (string tempPatientID in patientList)
        {
            cmdUpdate = new SqlCommand();

            gClass.MakeStoreProcedureName(ref cmdUpdate, "sp_PatientData_UpdateVisitDate", true);
            cmdUpdate.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
            cmdUpdate.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(tempPatientID);





            cmdSource.CommandText = validateDate + "AND patientPatientID ='" + tempPatientID + "' ORDER BY visitdateseen asc";
            drSource = cmdSource.ExecuteReader(CommandBehavior.Default);

            while (drSource.Read())
            {
                if (drSource.GetValue(3).ToString() != drSource.GetValue(4).ToString())
                {
                    gClass.ExecuteDMLCommand(cmdUpdate);
                }
            }
            drSource.Close();
        }
        drSource.Close();
    }
    #endregion
}

