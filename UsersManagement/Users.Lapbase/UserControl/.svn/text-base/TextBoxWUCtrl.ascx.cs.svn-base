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

public partial class UserControl_TextBoxWUCtrl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            txtGlobal.Attributes.Add("onfocus", "javascript:this.style.background = 'SandyBrown'");
            txtGlobal.Attributes.Add("onblur", "javascript:this.style.background = ''");
        }
    }
    //------------------------------------------------------------------------------------------------------------
    public string width
    {
        set { txtGlobal.Style.Add("Width", value);}
    }

    public string toolTip
    {
        set { txtGlobal.ToolTip = value; }
    }

    public string height
    {
        set { txtGlobal.Style.Add("height", value); }
    }

    public string Direction
    {
        set { txtGlobal.Style.Add("Direction", value); }
    }

    public TextBoxMode textMode
    {
        set { txtGlobal.TextMode =  value; }
    }

    public int maxLength
    {
        set { txtGlobal.MaxLength = value; }
    }

    public int rows
    {
        set { txtGlobal.Rows = value; }
    }

    public string Text
    {
        set { txtGlobal.Text = value; }
        get { return txtGlobal.Text; }
    }

    public System.Drawing.Color ForeColor
    {
        set { txtGlobal.ForeColor = value; }
    }

    public bool AutoPostBack
    {
        set { txtGlobal.AutoPostBack = value; }
    }

    /*
    public System.EventHandler OnTextChanged
    {
        set { txtGlobal.TextChanged +=  value; }
    }*/

    public short TabIndex
    {
        set { txtGlobal.TabIndex = value; }
    }

    public bool Enabled
    {
        set { txtGlobal.Enabled = value; }
    }

    public bool ReadOnly
    {
        set { txtGlobal.ReadOnly = value; }
    }
}
