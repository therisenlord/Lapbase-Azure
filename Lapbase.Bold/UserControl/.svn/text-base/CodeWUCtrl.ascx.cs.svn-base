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

public partial class UserControl_CodeWUCtrl : System.Web.UI.UserControl
{
    private string  strCriteria = "";

    private Boolean blnFirstRow = true;
    private int     intUserPracticeCode = 0;
    private string strGroupCode = "GRO";

    #region protected void Page_Load(object sender, EventArgs e)
    protected void Page_Load(object sender, EventArgs e)
    {
        intUserPracticeCode = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
        if (!IsPostBack)
            try{FetchCodeListData();}catch{}
    }
    #endregion

    #region public void FetchCodeListData()
    public void FetchCodeListData()
    {
        GlobalClass gClass = new GlobalClass();
        System.Data.SqlClient.SqlCommand cmdSelect = new System.Data.SqlClient.SqlCommand();

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_Codes_LoadData", false);
        cmdSelect.Parameters.Add("@UserPracticeCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
        cmdSelect.Parameters.Add("@CategoryCode", System.Data.SqlDbType.VarChar, 10).Value = strCriteria;

        CodeList.DataSource = gClass.FetchData(cmdSelect, "tblCode");
        CodeList.DataMember = "tblCode";
        CodeList.DataValueField = "Code";
        CodeList.DataTextField = "Description";
        try { CodeList.DataBind(); }
        catch { }
        if (blnFirstRow == true)
        CodeList.Items.Insert(0, new ListItem("Select ...", ""));
        //CodeList.Items.Insert(0, new ListItem((strCriteria.Equals(strGroupCode) ? "Select (optional)..." : "Select ..."), ""));
        return;
    }
    #endregion

    #region Properties
    public string CriteriaString
    {
        set { strCriteria = value; }
    }

    public int Width
    {
        set { CodeList.Width = Unit.Percentage(value); }
    }

    public string CssClass
    {
        set { CodeList.CssClass = value; }
    }

    public string SelectedValue
    {
        set { CodeList.SelectedValue = value; }
        get { return CodeList.SelectedValue; }
    }

    public int ItemsCount
    {
        get { return CodeList.Items.Count; }
    }

    public string SelectedText
    {
        get { return CodeList.Items[CodeList.SelectedIndex].Text; }
    }

    public ListItemCollection Items
    {
        get { return CodeList.Items; }
    }

    public int SelectedIndex
    {
        set { CodeList.SelectedIndex = value; }
        get { return CodeList.SelectedIndex; }
    }

    public bool autoPostBack
    {
        set { CodeList.AutoPostBack = value; }
    }

    public bool IsEnabled
    {
        set { CodeList.Enabled = value; }
    }

    public short SetTabIndex
    {
        set { CodeList.TabIndex = value; }
    }
    #endregion

    public Boolean FirstRow
    {
        set { blnFirstRow = value; }
        get { return blnFirstRow; }
    }
}
