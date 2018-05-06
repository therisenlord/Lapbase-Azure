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

public partial class UserControl_DocumentTypeWUCtrl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DataSet dsTemp;
        System.Data.SqlClient.SqlCommand cmdSelect = new System.Data.SqlClient.SqlCommand();
        GlobalClass gClass = new GlobalClass();

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_FileManagement_FetchFileCategory", true) ;
        dsTemp = gClass.FetchData(cmdSelect, "tblFileCategory");

        DocumentType.DataSource = dsTemp;
        DocumentType.DataMember = "tblFileCategory";
        DocumentType.DataValueField = "Type_ID";
        DocumentType.DataTextField = "Type_Name";
        DocumentType.DataBind();
        dsTemp.Dispose();
        cmdSelect.Dispose();
    }

    public string selectedValue
    {
        get {return (DocumentType.SelectedValue);}
        set { DocumentType.SelectedValue = value; }
    }

    public string selectedText
    {
        get { return (DocumentType.Items[DocumentType.SelectedIndex].Text); }
    }
}
