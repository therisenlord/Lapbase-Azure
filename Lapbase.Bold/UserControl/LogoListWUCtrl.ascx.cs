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

public partial class UserControl_LogoListWUCtrl : System.Web.UI.UserControl
{
    GlobalClass gClass = new GlobalClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        gClass.OrganizationCode = Request.Cookies["OrganizationCode"].Value;
        if (!IsPostBack)
            FetchLogoList();
    }

    //------------------------------------------------------------------------------------------------------------
    public void FetchLogoList()
    {
        System.Data.SqlClient.SqlCommand cmdSelect = new System.Data.SqlClient.SqlCommand();

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_Logos_LoadData", true);

        cmdSelect.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        LogosList.DataSource = gClass.FetchData(cmdSelect, "tblLogo");
        LogosList.DataMember = "tblLogo";
        LogosList.DataValueField = "LogoId";
        LogosList.DataTextField = "LogoName";
        try { LogosList.DataBind(); }
        catch { }
        LogosList.Items.Insert(0, new ListItem("Select ...", ""));
    }

    //----------------------------------------------------------------------------------------------------------
    public int Width
    {
        set { LogosList.Width = Unit.Percentage(value); }
    }

    public string CssClass
    {
        set { LogosList.CssClass = value; }
    }

    public string SelectedValue
    {
        set { LogosList.SelectedValue = value; }
        get { return LogosList.SelectedValue; }
    }

    public int ItemsCount
    {
        get { return LogosList.Items.Count; }
    }

    public string SelectedText
    {
        get { return LogosList.Items[LogosList.SelectedIndex].Text; }
    }

    public int SelectedIndex
    {
        set { LogosList.SelectedIndex = value; }
        get { return LogosList.SelectedIndex; }
    }

    public bool autoPostBack
    {
        set { LogosList.AutoPostBack = value; }
    }

    public EventHandler selectedIndexChanged
    {
        set { LogosList.SelectedIndexChanged += value; }
    }

    public short SetTabIndex
    {
        set { LogosList.TabIndex = value; }
    }

    public DataSet dsDataSource
    {
        set { LogosList.DataSource = value; }
    }

    public string strDataMember
    {
        set { LogosList.DataMember = value; }
    }

    public string strDataValueField
    {
        set { LogosList.DataValueField = value; }
    }

    public string strDataTextField
    {
        set { LogosList.DataTextField = value; }
    }

    public ListItemCollection AllItems
    {
        get { return LogosList.Items; }
    }
}
