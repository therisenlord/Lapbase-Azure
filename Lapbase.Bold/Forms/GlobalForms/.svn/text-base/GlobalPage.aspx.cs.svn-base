using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class Forms_GlobalForms_GlobalPage : System.Web.UI.Page
{
    GlobalClass gClass = new GlobalClass();

    #region protected void Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "no-cache");
        Response.Expires = -1;

        if (Request.QueryString.Count == 0) return;

        Response.Clear();
        switch (Request.QueryString["QSN"].ToString().ToUpper()) //QSN
        {
            case "LFC": // Load Fields Caption
                //LoadFieldNames(string strCulture, string strFormID)

                LoadFieldNames(Request.QueryString["Culture"].ToString(), Request.QueryString["FormID"].ToString(), Request.QueryString["FormID"].ToString().Equals("frmLogon"));

                //switch (Request.QueryString["FormID"].ToString())
                //{
                //    case "frmLogon":
                //        LoadFieldNames(Request.QueryString["Culture"].ToString(), Request.QueryString["FormID"].ToString(), true);
                //        break;
                //    default :
                //        break;
                //}
                break;
        }
        Response.End();
    }
    #endregion

    #region public void LoadFieldNames
    public void LoadFieldNames(string strCulture, string strFormID, bool LogonFormFlag)
    {
        try
        {
            string strLanguageCode = "", strDirection = "";

            if (LogonFormFlag)
            {
                SetLanguageInfo(strCulture);
                strLanguageCode = Response.Cookies["LanguageCode"].Value;
                strDirection = Response.Cookies["Direction"].Value;
            }
            else
            {
                strLanguageCode = Request.Cookies["LanguageCode"].Value;
                strDirection = Request.Cookies["Direction"].Value;
            }

            gClass.LanguageCode = strLanguageCode;// Response.Cookies["LanguageCode"].Value; //Application["LanguageCode"].ToString();
            gClass.FetchCaptions(strFormID, strLanguageCode);

            if (gClass.dsSchemaFields.Tables.Count > 0)
            {
                System.Data.DataColumn dtCol = new System.Data.DataColumn();

                dtCol.ColumnName = "strLanguage";
                dtCol.Caption = "strLanguage";
                dtCol.DataType = Type.GetType("System.String");
                dtCol.DefaultValue = strLanguageCode;// Response.Cookies["LanguageCode"].Value.Substring(0, 2);
                gClass.dsSchemaFields.Tables[0].Columns.Add(dtCol);

                dtCol = new System.Data.DataColumn();
                dtCol.ColumnName = "strDirection";
                dtCol.Caption = "strDirection";
                dtCol.DataType = Type.GetType("System.String");
                dtCol.DefaultValue = strDirection;// Response.Cookies["Direction"].Value;

                gClass.dsSchemaFields.Tables[0].Columns.Add(dtCol);
                gClass.dsSchemaFields.AcceptChanges();
            }

            if (LogonFormFlag)
                Response.Write(gClass.dsSchemaFields.GetXml());
            else
            {
                Response.Write(gClass.dsSchemaFields.GetXml());
            }
        }
        catch (Exception err)
        {
            Response.Write(err.ToString());
            gClass.AddErrorLogData("0", Request.Url.Host, "", "Global.aspx - Load captions and titles", "LoadFieldNames function", err.ToString());
        }
        return;
    }
    #endregion

    #region private void SetLanguageInfo(string strCulture)
    private void SetLanguageInfo(string strCulture)
    {
        string strLanguage = strCulture;
        int Idx = strLanguage.IndexOf(" ");

        Response.Cookies["LanguageCode"].Value = strLanguage.Substring(0, Idx).Trim();
        Request.Cookies["LanguageCode"].Value = strLanguage.Substring(0, Idx).Trim();
        strLanguage = strLanguage.Substring(Idx).Trim();
        Idx = strLanguage.IndexOf(" ");

        Response.Cookies.Set(new HttpCookie("CultureInfo", strLanguage.Substring(0, Idx).Trim()));
        Response.Cookies.Set(new HttpCookie("Direction", strLanguage.Substring(Idx).Trim()));
        return;
    }
    #endregion

}
