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

public partial class UserControl_RegionListWUCtrl : System.Web.UI.UserControl
{
    GlobalClass gClass = new GlobalClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        gClass.OrganizationCode = Request.Cookies["OrganizationCode"].Value;
        if (!IsPostBack)
            FetchRegionList();
    }

    //------------------------------------------------------------------------------------------------------------
    public void FetchRegionList()
    {
        System.Data.SqlClient.SqlCommand cmdSelect = new System.Data.SqlClient.SqlCommand();

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_Regions_LoadData", true);

        cmdSelect.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        RegionsList.DataSource = gClass.FetchData(cmdSelect, "tblRegion");
        RegionsList.DataMember = "tblRegion";
        RegionsList.DataValueField = "Region Id";
        RegionsList.DataTextField = "Region Name";
        try { RegionsList.DataBind(); }
        catch { }
        RegionsList.Items.Insert(0, new ListItem("Select ...", ""));
    }

    //----------------------------------------------------------------------------------------------------------
    public int Width
    {
        set { RegionsList.Width = Unit.Percentage(value); }
    }

    public string CssClass
    {
        set { RegionsList.CssClass = value; }
    }

    public string SelectedValue
    {
        set { RegionsList.SelectedValue = value; }
        get { return RegionsList.SelectedValue; }
    }

    public int ItemsCount
    {
        get { return RegionsList.Items.Count; }
    }

    public string SelectedText
    {
        get { return RegionsList.Items[RegionsList.SelectedIndex].Text; }
    }

    public int SelectedIndex
    {
        set { RegionsList.SelectedIndex = value; }
        get { return RegionsList.SelectedIndex; }
    }

    public bool autoPostBack
    {
        set { RegionsList.AutoPostBack = value; }
    }

    public EventHandler selectedIndexChanged
    {
        set { RegionsList.SelectedIndexChanged += value; }
    }

    public short SetTabIndex
    {
        set { RegionsList.TabIndex = value; }
    }

    public DataSet dsDataSource
    {
        set { RegionsList.DataSource = value; }
    }

    public string strDataMember
    {
        set { RegionsList.DataMember = value; }
    }

    public string strDataValueField
    {
        set { RegionsList.DataValueField = value; }
    }

    public string strDataTextField
    {
        set { RegionsList.DataTextField = value; }
    }

    public ListItemCollection AllItems
    {
        get { return RegionsList.Items; }
    }
}
