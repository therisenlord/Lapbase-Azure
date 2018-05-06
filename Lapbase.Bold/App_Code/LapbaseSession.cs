using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for LapbaseSession
/// </summary>
public static class LapbaseSession
{
    #region variables
    #endregion

    #region properties
    /// <summary>
    /// Gets or sets the Patient's Id
    /// </summary>
    public static Int32 PatientId
    {
        set 
        { 
            HttpContext.Current.Session["PatientId"] = value.ToString();
        }
        get 
        {
            if (HttpContext.Current.Session["PatientId"] != null)
            {
                Int32 intTemp = 0;
                Int32.TryParse(HttpContext.Current.Session["PatientId"].ToString(), out intTemp);
                return intTemp;
            }
            else
            {
                return 0;
            }
        }
    }

    /// <summary>
    /// Gets or sets the current organization code
    /// </summary>
    public static Int32 OrganizationCode 
    {
        set
        {
            HttpContext.Current.Session["OrganizationCode"] = value.ToString();
        }
        get
        {
            if (HttpContext.Current.Session["OrganizationCode"] != null)
            {
                Int32 intTemp = 0;
                Int32.TryParse(HttpContext.Current.Session["OrganizationCode"].ToString(), out intTemp);
                return intTemp;
            }
            else
            {
                return 0;
            }
        }
    }

    /// <summary>
    /// Gets or sets the current organization code
    /// </summary>
    public static Int32 UserPracticeCode
    {
        set
        {
            HttpContext.Current.Session["UserPracticeCode"] = value.ToString();
        }
        get
        {
            if (HttpContext.Current.Session["UserPracticeCode"] != null)
            {
                Int32 intTemp = 0;
                Int32.TryParse(HttpContext.Current.Session["UserPracticeCode"].ToString(), out intTemp);
                return intTemp;
            }
            else
            {
                return 0;
            }
        }
    }
    #endregion

}
