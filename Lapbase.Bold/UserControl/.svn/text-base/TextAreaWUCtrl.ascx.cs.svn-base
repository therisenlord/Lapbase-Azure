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

public partial class UserControl_TextAreaWUCtrl : System.Web.UI.UserControl
{
    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            txtGlobal.Attributes.Add("onfocus", "javascript:this.style.background = 'SandyBrown'");
            txtGlobal.Attributes.Add("onblur", "javascript:this.style.background = ''");
        }
    }
    #endregion

    #region Methods
    /// <summary>
    /// This function is to add a style to TextBox
    /// </summary>
    /// <param name="Key"></param>
    /// <param name="Value"></param>
    public void SetStyle(String Key, String Value)
    {
        this.txtGlobal.Style.Add(Key, Value);
    }
    #endregion

    #region Properties
    public string width
    {
        set { txtGlobal.Style.Add("Width", value); }
    }
    
    public string height
    {
        set { txtGlobal.Style.Add("height", value); }
    }

    public string Direction
    {
        set { txtGlobal.Style.Add("Direction", value); }
    }

    public int rows
    {
        set { txtGlobal.Rows = value; }
    }

    public string Text
    {
        set { txtGlobal.Value = value; }
        get { return txtGlobal.Value; }
    }
    /*
    public System.EventHandler OnTextChanged
    {
        set { txtGlobal.TextChanged +=  value; }
    }*/

    #endregion
}
