using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Globalization;
using System.Xml;
using System.IO;
using System.Text;


/// <summary>
/// Summary description for ConsultFU1WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class ConsultFU1WebService : System.Web.Services.WebService {
    GlobalClass gClass = new GlobalClass();

    public ConsultFU1WebService () {
        System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo(Context.Request.Cookies["CultureInfo"].Value, true);
        gClass.OrganizationCode = Context.Request.Cookies["OrganizationCode"].Value;
    }

    /*************************************************************************************************************
     * This Section contain all functions about Progress Notes of each patient
     ************************************************************************************************************/
    #region [WebMethod] public XmlDocument UpdateNextDateVisit
    [WebMethod]
    public XmlDocument UpdateNextDateVisit(string strDateSeen, string strMonthWeek, string strMonthWeekFlag)
    {
        CultureInfo myCulture = new CultureInfo(Context.Request.Cookies["CultureInfo"].Value, true);
        DateTimeFormatInfo myDTFI = myCulture.DateTimeFormat;
        DateTime resultDate = DateTime.Now;
        int intMonthWeek = 0;
        string strValue = "";
        Boolean blnErrorFlag = false;

        try
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo(Context.Request.Cookies["CultureInfo"].Value, true);
            if (! Int32.TryParse(strMonthWeek, out intMonthWeek))
            {
                blnErrorFlag = true;
            }

            if (!DateTime.TryParse(strDateSeen, out resultDate))
            {
                blnErrorFlag = true;
            }

            if (!blnErrorFlag)
            {
                if (strMonthWeekFlag == "1") //Month
                    resultDate = resultDate.AddMonths(intMonthWeek);
                else
                    resultDate = resultDate.AddDays(intMonthWeek * 7);
                strValue = resultDate.ToString(myCulture.DateTimeFormat.ShortDatePattern);
                strValue += ";" + gClass.GetMonthName(resultDate.Month) + " " + resultDate.Year.ToString();
            }
            else { strValue = "E2"; }
        }
        catch 
        {
            strValue = "E2";
        }
        return gClass.GetXmlDocument(Guid.NewGuid(), strValue );
    }
    #endregion

    #region [WebMethod] public XmlDocument FetchCurrentDate()
    [WebMethod]
    public XmlDocument FetchCurrentDate()
    {
        string strReturn = "";

        System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo(Context.Request.Cookies["CultureInfo"].Value, true);
        strReturn = DateTime.Now.ToShortDateString();

        return gClass.GetXmlDocument(Guid.NewGuid(), strReturn);
    }
    #endregion

    #region [WebMethod] public XmlDocument UpdatePatientNotes(string strPatientNotes)
    [WebMethod]
    public XmlDocument UpdatePatientNotes(string strPatientNotes)
    {
        string strReturn = "";
        SqlCommand cmdUpdate = new SqlCommand(); 

        gClass.MakeStoreProcedureName(ref cmdUpdate, "sp_ConsultFU1_PatientWeightData_UpdatePatientNotes", true);
        cmdUpdate.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode); 
        cmdUpdate.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Context.Request.Cookies["UserPracticeCode"].Value); 
        cmdUpdate.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(Context.Request.Cookies["PatientID"].Value); 
        cmdUpdate.Parameters.Add("@PatientNotes", SqlDbType.VarChar, 1024).Value = strPatientNotes;

        try
        {
            //save into log
            SqlCommand cmdSaveLog = new SqlCommand();
            gClass.MakeStoreProcedureName(ref cmdSaveLog, "sp_PatientDataLog_cmdInsert", true);
            cmdSaveLog.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
            cmdSaveLog.Parameters.Add("@PatientId", SqlDbType.Int).Value = Convert.ToInt32(Context.Request.Cookies["PatientID"].Value);
            cmdSaveLog.Parameters.Add("@LogUserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Context.Request.Cookies["UserPracticeCode"].Value);
            cmdSaveLog.Parameters.Add("@LogDateTime", SqlDbType.DateTime).Value = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
            gClass.ExecuteDMLCommand(cmdSaveLog);

            cmdSaveLog = new SqlCommand();
            gClass.MakeStoreProcedureName(ref cmdSaveLog, "sp_PatientEMRLog_cmdInsert", true);
            cmdSaveLog.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
            cmdSaveLog.Parameters.Add("@PatientId", SqlDbType.Int).Value = Convert.ToInt32(Context.Request.Cookies["PatientID"].Value);
            cmdSaveLog.Parameters.Add("@LogUserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Context.Request.Cookies["UserPracticeCode"].Value);
            cmdSaveLog.Parameters.Add("@LogDateTime", SqlDbType.DateTime).Value = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
            gClass.ExecuteDMLCommand(cmdSaveLog);

            cmdSaveLog = new SqlCommand();
            gClass.MakeStoreProcedureName(ref cmdSaveLog, "sp_PatientWeightDataLog_cmdInsert", true);
            cmdSaveLog.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
            cmdSaveLog.Parameters.Add("@PatientId", SqlDbType.Int).Value = Convert.ToInt32(Context.Request.Cookies["PatientID"].Value);
            cmdSaveLog.Parameters.Add("@LogUserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Context.Request.Cookies["UserPracticeCode"].Value);
            cmdSaveLog.Parameters.Add("@LogDateTime", SqlDbType.DateTime).Value = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
            cmdSaveLog.Parameters.Add("@PageNo", SqlDbType.Int).Value = 2;
            gClass.ExecuteDMLCommand(cmdSaveLog);

            cmdSaveLog = new SqlCommand();
            gClass.MakeStoreProcedureName(ref cmdSaveLog, "sp_PatientDataLog_SaveBoldData", true);
            cmdSaveLog.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
            cmdSaveLog.Parameters.Add("@PatientId", SqlDbType.Int).Value = Convert.ToInt32(Context.Request.Cookies["PatientID"].Value);
            cmdSaveLog.Parameters.Add("@LogUserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Context.Request.Cookies["UserPracticeCode"].Value);
            cmdSaveLog.Parameters.Add("@LogDateTime", SqlDbType.DateTime).Value = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
            gClass.ExecuteDMLCommand(cmdSaveLog);



            gClass.SavePatientWeightData(cmdUpdate);
            strReturn = string.Empty;

            gClass.SaveUserLogFile(Context.Request.Cookies["UserPracticeCode"].Value,
                Context.Request.Cookies["Logon_UserName"].Value,
                Context.Request.Url.Host, "Visit Form", 2, "Save Patient General data", "Patient ID", Context.Request.Cookies["PatientID"].Value);

        }
        catch (Exception err)
        {
            strReturn = err.ToString();
            gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                Context.Request.Cookies["Logon_UserName"].Value, "Visit Form", "Save Patient General data - UpdatePatientNotes function", err.ToString());
        }
        return gClass.GetXmlDocument(Guid.NewGuid(), strReturn);
    }
    #endregion
}