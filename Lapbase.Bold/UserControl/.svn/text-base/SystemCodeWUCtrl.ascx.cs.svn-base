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

public partial class UserControl_SystemCodeWUCtrl : System.Web.UI.UserControl
{
    private string  strCriteria = "";

    private Boolean blnFirstRow = true;
    private int intUserPracticeCode = 0;
    GlobalClass gClass = new GlobalClass();
    private Boolean _IsBold = false;
    private String strBoldCode = String.Empty, strSCode = String.Empty;

    //-------------------------------------------------------------------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        intUserPracticeCode = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
        if (!IsPostBack)
        {
            try
            {
                FetchSystemCodeListData();
            }
            catch(Exception err)
            {
                gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, 
                        "System Code Web User Control", "FetchSystemCodeListData function", err.ToString());
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------------
    public void FetchSystemCodeListData()
    {
        Int32 count=0;
        System.Data.SqlClient.SqlCommand cmdSelect = new System.Data.SqlClient.SqlCommand();

        gClass.MakeStoreProcedureName(ref cmdSelect, (_IsBold ? "sp_SystemCodes_LoadData_Bold" : "sp_SystemCodes_LoadData"), true);

        cmdSelect.Parameters.Add("@CategoryCode", System.Data.SqlDbType.VarChar, 10).Value = strCriteria;
        if (_IsBold)
        {
            cmdSelect.Parameters.Add("@GroupCode", System.Data.SqlDbType.VarChar, 10).Value = strBoldCode;
            cmdSelect.Parameters.Add("@SCode", System.Data.SqlDbType.VarChar, 10).Value = strSCode;
        }
        SystemCodeList.DataSource = gClass.FetchData(cmdSelect, "tblCode");
        SystemCodeList.DataMember = "tblCode";
        SystemCodeList.DataValueField = "Code";
        SystemCodeList.DataTextField = "Description";

        try { SystemCodeList.DataBind(); }
        catch { }
        if(blnFirstRow == true)
        SystemCodeList.Items.Insert(0, new ListItem("Select ...", ""));

        /*for (int Xh = 0; Xh < SystemCodeList.Items.Count; Xh++)
        {
            SystemCodeList.Items[Xh].Attributes.Add("title", SystemCodeList.Items[Xh].Text);
        }*/

        return;
    }

    //------------------------------------------------------------------------------------------------------------
    public string SCode
    {
        set { strSCode = value; }
    }

    public string CriteriaString
    {
        set { strCriteria = value; }
    }

    public int Width
    {
        set { SystemCodeList.Width = Unit.Percentage(value); }
    }

    public int Height
    {
        set { SystemCodeList.Height = Unit.Pixel(value); }
    }

    public Boolean Enabled
    {
        set { this.SystemCodeList.Enabled = value; }
    }

    public string CssClass
    {
        set { SystemCodeList.CssClass = value; }
    }

    public string SelectedValue
    {
        set { SystemCodeList.SelectedValue = value; }
        get { return SystemCodeList.SelectedValue; }
    }

    public int ItemsCount
    {
        get { return SystemCodeList.Items.Count; }
    }

    public string SelectedText
    {
        get { return SystemCodeList.Items[SystemCodeList.SelectedIndex].Text; }
    }

    public int SelectedIndex
    {
        set { SystemCodeList.SelectedIndex = value; }
        get { return SystemCodeList.SelectedIndex; }
    }

    public bool autoPostBack
    {
        set { SystemCodeList.AutoPostBack = value; }
    }

    public EventHandler selectedIndexChanged
    {
        set { SystemCodeList.SelectedIndexChanged += value; }
    }

    public short SetTabIndex
    {
        set { SystemCodeList.TabIndex = value; }
    }

    public ListItemCollection Items
    {
        get { return SystemCodeList.Items; }
    }

    public String BoldData
    {
        set {
            strBoldCode = value;
            _IsBold = (strBoldCode.Trim().Length > 0); 
        }
    }

    public Boolean Display
    {

        set { SystemCodeList.Style.Add("display", value ? "block" : "none"); }
    }


    public Boolean FirstRow
    {
        set { blnFirstRow = value; }
        get { return blnFirstRow; }
    }
}
