using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;

/// <summary>
/// Summary description for GlobalWebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class GlobalWebService : System.Web.Services.WebService {
    GlobalClass gClass = new GlobalClass();

    public GlobalWebService () {
        gClass.OrganizationCode = Context.Request.Cookies["OrganizationCode"].Value;
    }

    #region [WebMethod] private string UpdatePatientTargetWeight_PWD
    [WebMethod]
    public string UpdatePatientTargetWeight_PWD(string Group, string TargetWeight)
    {
        string strReturn = "";
        SqlCommand cmdUpdate = new SqlCommand();

        gClass.MakeStoreProcedureName(ref cmdUpdate, "sp_ConsultFU1_PatientWeightData_UpdatePatientTitle", true);

        cmdUpdate.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdUpdate.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Context.Request.Cookies["UserPracticeCode"].Value);
        cmdUpdate.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(Context.Request.Cookies["PatientID"].Value);
        cmdUpdate.Parameters.Add("@TargetWeight", SqlDbType.Decimal);
        cmdUpdate.Parameters.Add("@Group", SqlDbType.VarChar, 6).Value = Group;

        try { cmdUpdate.Parameters["@TargetWeight"].Value = Convert.ToDecimal(TargetWeight); }
        catch { cmdUpdate.Parameters["@TargetWeight"].Value = 0; }
        
        try
        {
            gClass.ExecuteDMLCommand(cmdUpdate);
            strReturn = string.Empty;
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host, Context.Request.Cookies["Logon_UserName"].Value, "Global Web Service", "UpdatePatientTargetWeight_PWD function", err.ToString());
        }
        return (strReturn);
    }
    #endregion

    #region [WebMethod] public XmlDocument CheckDate(string strDate)
    [WebMethod]
    public XmlDocument CheckDate(string strDate)
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo(Context.Request.Cookies["CultureInfo"].Value, true);
        //System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo(GlobalClass.CultureInfo, true);
        DateTime tempDate = new DateTime();
        string strResult = "";

        if (strDate.Trim() == string.Empty)
            strResult = "";
        else
            try
            {
                tempDate = Convert.ToDateTime(strDate);
                if (tempDate > DateTime.Now)
                    strResult = "E2";
                else
                    strResult = "";
            }
            catch { strResult = "E1"; }
        return gClass.GetXmlDocument(Guid.NewGuid(), strResult);

    }
    #endregion

    #region [WebMethod] public XmlDocument RefferingDoctor_SaveProc
    [WebMethod]
    public XmlDocument RefferingDoctor_SaveProc(string strSurname, string strFirstName, string strTitle, string UseFirst, 
        string strAddress1, string strAddress2, string strSuburb, string strPostalCode,
        string strState, string strPhone, string strFax, string strRefDoctorID)
    {
        SqlCommand cmdSave = new SqlCommand();
        string strReturn = "";

        gClass.MakeStoreProcedureName(ref cmdSave, "sp_RefDoctors_SaveData", false);
        cmdSave.Parameters.Add("@RefDrID_Old", SqlDbType.VarChar, 10).Value = strRefDoctorID.Trim();

        cmdSave.Parameters.Add("@RefDrID", SqlDbType.VarChar, 10).Value = 
            ((strSurname.Trim().Length > 4) ? strSurname.Trim().Replace("'", "`").Substring(0, 4) : strSurname.Trim().Replace("'", "`"))
            + strFirstName.Trim().Replace("'", "`").Substring(0, 1);
        cmdSave.Parameters.Add("@Surname", SqlDbType.VarChar, 50).Value = strSurname.Replace("'", "`");
        cmdSave.Parameters.Add("@FirstName", SqlDbType.VarChar, 50).Value = strFirstName.Replace("'", "`");
        cmdSave.Parameters.Add("@Title", SqlDbType.VarChar, 15).Value = strTitle.Replace("'", "`");
        cmdSave.Parameters.Add("@UseFirst", SqlDbType.Bit).Value = UseFirst.Replace("'", "`");
        cmdSave.Parameters.Add("@Address1", SqlDbType.VarChar, 50).Value = strAddress1.Replace("'", "`");
        cmdSave.Parameters.Add("@Address2", SqlDbType.VarChar, 50).Value = strAddress2.Replace("'", "`");
        cmdSave.Parameters.Add("@Suburb", SqlDbType.VarChar, 50).Value = strSuburb.Replace("'", "`");
        cmdSave.Parameters.Add("@PostalCode", SqlDbType.VarChar, 6).Value = strPostalCode.Replace("'", "`");
        cmdSave.Parameters.Add("@State", SqlDbType.VarChar, 3).Value = strState.Replace("'", "`");
        cmdSave.Parameters.Add("@Phone", SqlDbType.VarChar, 20).Value = strPhone.Replace("'", "`");
        cmdSave.Parameters.Add("@Fax", SqlDbType.VarChar, 20).Value = strFax.Replace("'", "`");
        cmdSave.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Context.Request.Cookies["UserPracticeCode"].Value);
        cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);

        try
        {
            gClass.ExecuteDMLCommand(cmdSave);
            strReturn = "";
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host, Context.Request.Cookies["Logon_UserName"].Value, "Global Web Service", "RefferingDoctor_SaveProc function", err.ToString());
        }
        cmdSave.Dispose();
        return gClass.GetXmlDocument(Guid.NewGuid(), strReturn);
    }
    #endregion

    #region [WebMethod] public XmlDocument CalculateAge
    [WebMethod]
    public XmlDocument CalculateAge(string strBirthDate)
    {
        string strResult;
        System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo(Context.Request.Cookies["CultureInfo"].Value, true);
        if (strBirthDate.Trim() == string.Empty)
            strResult = "";
        else
        {
            try { strResult = CalculateAge(Convert.ToDateTime(strBirthDate)); }
            catch { strResult = ""; }
        }

        return gClass.GetXmlDocument(Guid.NewGuid(), strResult);
    }
    #endregion

    #region private string CalculateAge
    private string CalculateAge(DateTime BirthDate)
    {
        int intAge = DateTime.Now.Year - BirthDate.Year;
        if (DateTime.Now.Month < BirthDate.Month)
            --intAge;
        else if (DateTime.Now.Month == BirthDate.Month)
            if (DateTime.Now.Day < BirthDate.Day)
                --intAge;

        return (intAge.ToString());
    }
    #endregion

}

