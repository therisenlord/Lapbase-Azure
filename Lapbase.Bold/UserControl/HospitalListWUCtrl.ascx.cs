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

public partial class UserControl_HospitalListWUCtrl : System.Web.UI.UserControl
{
    GlobalClass gClass = new GlobalClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        gClass.OrganizationCode = Request.Cookies["OrganizationCode"].Value;

        if (!IsPostBack)
            FetchHospitalList();
    }

    //------------------------------------------------------------------------------------------------------------
    public void FetchHospitalList()
    {
        System.Data.SqlClient.SqlCommand cmdSelect = new System.Data.SqlClient.SqlCommand();

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_Hospitals_LoadData", true);

        cmdSelect.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        HospitalsList.DataSource = gClass.FetchData(cmdSelect, "tblHospital");
        HospitalsList.DataMember = "tblHospital";
        HospitalsList.DataValueField = "Hospital Id";
        HospitalsList.DataTextField = "Hospital Name";
        try { HospitalsList.DataBind(); }
        catch { }
        HospitalsList.Items.Insert(0, new ListItem("Select ...", ""));
    }

    //----------------------------------------------------------------------------------------------------------
    public int Width
    {
        set { HospitalsList.Width = Unit.Percentage(value); }
    }

    public string CssClass
    {
        set { HospitalsList.CssClass = value; }
    }

    public string SelectedValue
    {
        set { HospitalsList.SelectedValue = value; }
        get { return HospitalsList.SelectedValue; }
    }

    public int ItemsCount
    {
        get { return HospitalsList.Items.Count; }
    }

    public string SelectedText
    {
        get { return HospitalsList.Items[HospitalsList.SelectedIndex].Text; }
    }

    public int SelectedIndex
    {
        set { HospitalsList.SelectedIndex = value; }
        get { return HospitalsList.SelectedIndex; }
    }

    public bool autoPostBack
    {
        set { HospitalsList.AutoPostBack = value; }
    }

    public EventHandler selectedIndexChanged
    {
        set { HospitalsList.SelectedIndexChanged += value; }
    }

    public short SetTabIndex
    {
        set { HospitalsList.TabIndex = value; }
    }

    public DataSet dsDataSource
    {
        set { HospitalsList.DataSource = value; }
    }

    public string strDataMember
    {
        set { HospitalsList.DataMember = value; }
    }

    public string strDataValueField
    {
        set { HospitalsList.DataValueField = value; }
    }

    public string strDataTextField
    {
        set { HospitalsList.DataTextField = value; }
    }

    public ListItemCollection AllItems
    {
        get { return HospitalsList.Items; }
    }
}
