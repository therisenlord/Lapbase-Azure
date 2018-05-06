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
using Lapbase.Configuration.ConfigurationApplication;

public partial class UserControl_AppSchemaFuncWUCtrl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        a_ImportFile.Style["display"] = "block";
        a_UpdateDateSeen.Style["display"] = "block";
        a_UpdateCurrentWeight.Style["display"] = "block";
        a_UpdateBandType.Style["display"] = "block";
        a_UpdateSurgeryType.Style["display"] = "block";
        a_ImportPathology.Style["display"] = "block";

        return;
    }

    #region Properties
    public string currentItem
    {
        set {((HtmlControl)FindControl("li_" + value)).Attributes.Add("class", "current");}
    }
    #endregion
}
