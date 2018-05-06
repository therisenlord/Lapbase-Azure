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

public partial class Forms_ImportExportData_UpdateOpWeight : System.Web.UI.Page
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
                                            "Update Weight Form", 2, "Browse", "", "");
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

    #region protected void updateOpWeight(object sender, EventArgs e)
    protected void updateOpWeight(object sender, EventArgs e)
    {
        SqlCommand cmdUpdate = new SqlCommand();

        gClass.MakeStoreProcedureName(ref cmdUpdate, "sp_PatientOperation_UpdateSingleOpweight", true);
        cmdUpdate.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        
        try{
            gClass.ExecuteDMLCommand(cmdUpdate); 
            gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value, Request.Cookies["Logon_UserName"].Value,
                                        Request.Url.Host, "ImportExport", 2, "Update Opearation Weight", "User Practice Code", Request.Cookies["UserPracticeCode"].Value);
        }
        catch (Exception err)
        {
            txtNotes.Value = txtNotes.Value + "\nError in Updating Operation Weight\n";
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Baseline", "Data saving baseline", err.ToString());
        }
        
        txtNotes.Value = txtNotes.Value + "\nUpdate Operation Weight Success\n";
        cmdUpdate.Dispose();
    }
    #endregion
}

