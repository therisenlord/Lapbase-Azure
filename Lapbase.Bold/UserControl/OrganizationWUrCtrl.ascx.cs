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

public partial class UserControl_OrganizationWUrCtrl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try { FetchOrganizationListData(); }catch(Exception err) { Response.Write(err.ToString()); }
        }
    }

    #region public void FetchOrganizationListData()
    public void FetchOrganizationListData()
    {
        GlobalClass gClass = new GlobalClass();
        System.Data.SqlClient.SqlCommand cmdSelect = new System.Data.SqlClient.SqlCommand();

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_Organization_LoadData", false);
        cmdSelect.Parameters.Add("@strOrganizationCode", System.Data.SqlDbType.VarChar, 10).Value = String.Empty;

        OrganizationList.DataSource = gClass.FetchData(cmdSelect, "tblOrganization");
        OrganizationList.DataMember = "tblOrganization";
        OrganizationList.DataValueField = "Code";
        OrganizationList.DataTextField = "OrganizationName";
        OrganizationList.DataBind();
        OrganizationList.Items.Insert(0, new ListItem("Select from list...", ""));
        return;
    }
    #endregion

    #region Properties

    public int Width
    {
        set { OrganizationList.Width = Unit.Percentage(value); }
    }

    public string CssClass
    {
        set { OrganizationList.CssClass = value; }
    }

    public string selectedValue
    {
        set { OrganizationList.SelectedValue = value; }
        get { return OrganizationList.SelectedValue; }
    }

    public int ItemsCount
    {
        get { return OrganizationList.Items.Count; }
    }

    public string SelectedText
    {
        get { return OrganizationList.Items[OrganizationList.SelectedIndex].Text; }
    }

    public int SelectedIndex
    {
        set { OrganizationList.SelectedIndex = value; }
        get { return OrganizationList.SelectedIndex; }
    }

    public bool autoPostBack
    {
        set { OrganizationList.AutoPostBack = value; }
    }

    public bool IsEnabled
    {
        set { OrganizationList.Enabled = value; }
    }

    public short SetTabIndex
    {
        set { OrganizationList.TabIndex = value; }
    }
    #endregion

}

