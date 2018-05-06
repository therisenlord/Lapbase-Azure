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

public partial class UserControl_MenuWUCtrl : System.Web.UI.UserControl
{
    GlobalClass gClass = new GlobalClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        gClass.OrganizationCode = Request.Cookies["OrganizationCode"].Value;
        if (Request.Cookies["PermissionLevel"].Value == "1o" || Request.Cookies["PermissionLevel"].Value == "2t" || (Request.Cookies["PermissionLevel"].Value == "3f" && Request.Cookies["SurgeonID"].Value == "0"))
            menuReports.Visible = false;
        //if (Request.Cookies["PermissionLevel"].Value == "1o" || Request.Cookies["PermissionLevel"].Value == "2t" || Request.Cookies["PermissionLevel"].Value == "3f")
        
        
        //if (Convert.ToInt32(gClass.OrganizationCode) != 2 && Convert.ToInt32(gClass.OrganizationCode) != 122 && Convert.ToInt32(gClass.OrganizationCode) != 88)
        if (Convert.ToInt32(gClass.OrganizationCode) != 122 && Convert.ToInt32(gClass.OrganizationCode) != 88)
            menuImport.Visible = false;

        //if (Convert.ToInt32(gClass.OrganizationCode) != 2 && Convert.ToInt32(gClass.OrganizationCode) != 122 && Convert.ToInt32(gClass.OrganizationCode) != 208)
        if (Convert.ToInt32(gClass.OrganizationCode) != 122)
            menuImportCSV.Visible = false;

        //split feature
        string Feature = Request.Cookies["Feature"].Value;
        string[] Features = Feature.Split(new string[] { "**" }, StringSplitOptions.None);

        string ShowExport = Features[3];

        if (ShowExport == "False")
        {
            menuExportCSV.Visible = false;
        }
        
        string ShowBSR = Features[4];

        if (ShowBSR == "False")
        {
            menuBSR.Visible = false;
        }

        
        
    }
}
