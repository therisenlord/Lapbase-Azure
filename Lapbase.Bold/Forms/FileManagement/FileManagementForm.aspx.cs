using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Globalization;
using Microsoft.Web.UI;

public partial class Forms_FileManagement_FileManagementForm : System.Web.UI.Page
{
    GlobalClass gClass = new GlobalClass();

    #region protected void Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        bool LoadFlag = true;
        string strLanguageCode = "", strURL, strRawUrl ;

        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "no-cache");
        Response.Expires = -1;

        strURL = Request.Url.ToString().Replace(" ", "").Replace("%20", "");
        strRawUrl = Request.RawUrl.Replace(" ", "").Replace("%20", "");

        try
        {
            Page.Culture = Request.Cookies["CultureInfo"].Value;
            gClass.OrganizationCode = Request.Cookies["OrganizationCode"].Value;
            txtHApplicationURL.Value = Request.Url.Scheme + "://" + Request.Url.Host + Request.ApplicationPath + "/";
            lblPatientID.Text = Request.Cookies["PatientID"].Value;
            txtHCulture.Value = Request.Cookies["CultureInfo"].Value;
            
            if (gClass.IsUserLogoned(Session.SessionID, Request.Cookies["UserPracticeCode"].Value, Request.Url.Host))
            {
                if (!IsPostBack)
                    bodyFileManagement.Attributes.Add("onload", "javascript:InitializePage('" + Request.QueryString["QSN"].ToUpper() + "')");

                gClass.SaveActionLog(gClass.OrganizationCode,
                     Request.Cookies["UserPracticeCode"].Value,
                     Request.Url.Host,
                     System.Configuration.ConfigurationManager.AppSettings["DocumentPage"].ToString(),
                     System.Configuration.ConfigurationManager.AppSettings["LogRead"].ToString(),
                     "Load " + System.Configuration.ConfigurationManager.AppSettings["DocumentPage"].ToString() + " List",
                     "",
                     "");

                if (Request.Cookies["PermissionLevel"].Value == "1o")
                {
                    btnUpload.Style["display"] = "none";
                }

            }
            else
            {
                LoadFlag = false;
                strLanguageCode = Request.Cookies["LanguageCode"].Value;
            }
        }
        catch (Exception err)
        {
            try
            {
                strLanguageCode = Request.Cookies["LanguageCode"].Value;
                gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "File Management Form", "Loading Files Data (Page_Load function)", err.ToString());
            }
            catch { strLanguageCode = "en-US"; }

            LoadFlag = false;
        }

        if (LoadFlag && Request.QueryString["ReLoad"].Equals("0")) // Call by AJAX function from FileManagement.JS
        {
            Response.Clear();
            switch (Request.QueryString["QSN"].ToUpper())
            {
                case "TYPE":
                case "TYPEDATE":
                case "DATE":
                    FetchPatientGroupDocument();
                    break;
                case "LOADDOCUMENTITEMS":
                    FetchPatientGroupDocumentItems();
                    break;
                case "LOADDOCUMENTDETAIL":
                    FetchDocumentDetailInformation();
                    break;
                case "LOADEVENTDATA":
                    FetchEventData();
                    break;
                case "LOADDOCUMENTITEMS_EVENTDATE":
                    FetchPatientGroupDocumentItems_OnEventDate();
                    break;
                case "LOADDOCUMENTITEMS_EVENTLINK":
                    FetchPatientGroupDocumentItems_OnEventLink();
                    break;
                case "DELETEDOCUMENT":
                    DeleteDocument();
                    break;
                case "LOADINGDOCUMENT":
                    LoadDocumentContent();
                    break;
                case "LOADEVENTHISTORY":
                    FetchDocumentEventHistory();
                    break;
            }
            Response.End();
        }
        else
            if (!LoadFlag)
                gClass.ReturnToLoginPage(Request.Url.Host, strLanguageCode, Response);
    }
    #endregion

    #region private void FetchPatientGroupDocument
    private void FetchPatientGroupDocument()
    {
        SqlCommand cmdSelect = new SqlCommand();
        DataSet dsDocument = new DataSet();
        DataTable myTable = new DataTable();

        switch (Request.QueryString["QSN"].ToUpper())
        {
            case "TYPE":
            case "TYPEDATE":
                gClass.MakeStoreProcedureName(ref cmdSelect, "sp_FileManagement_FetchPatientDocuments", true);
                cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                cmdSelect.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
                cmdSelect.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
                cmdSelect.Parameters.Add("@GetType", SqlDbType.Bit).Value = 1;
                cmdSelect.Parameters.Add("@ShowDeleted", SqlDbType.Bit).Value = Convert.ToBoolean(Request.QueryString["SD"] == "1");
                myTable = gClass.FetchData(cmdSelect, "tblParent").Tables[0].Copy();
                cmdSelect.Parameters["@GetType"].Value = 0;
                dsDocument = gClass.FetchData(cmdSelect, "tblChild");
                break;
            case "DATE":
                gClass.MakeStoreProcedureName(ref cmdSelect, "sp_FileManagement_FetchPatientDocuments_ShowByDate", true);
                cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                cmdSelect.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
                cmdSelect.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
                cmdSelect.Parameters.Add("@GetAll", SqlDbType.Bit).Value = 0;
                cmdSelect.Parameters.Add("@ShowDeleted", SqlDbType.Bit).Value = Convert.ToBoolean(Request.QueryString["SD"] == "1");
                myTable = gClass.FetchData(cmdSelect, "tblParent").Tables[0].Copy();

                cmdSelect.Parameters["@GetAll"].Value = 1;
                dsDocument = gClass.FetchData(cmdSelect, "tblChild");
                break;
        }

        DataColumn tempCol = new DataColumn();
        tempCol.Caption = "ShowBy";
        tempCol.DataType = Type.GetType("System.String");
        tempCol.DefaultValue = Request.QueryString["QSN"].ToUpper();
        tempCol.ColumnName = "ShowBy";
        myTable.Columns.Add(tempCol);
        dsDocument.Tables.Add(myTable);

        switch (Request.QueryString["QSN"].ToUpper())
        {
            case "TYPE":
            case "TYPEDATE":
                AddColumns(dsDocument);
                break;
        }

        dsDocument.AcceptChanges();
        Response.Write(gClass.ShowSchema(dsDocument, Server.MapPath(".") + @"\Includes\LeftFrame_DocumentsListXSLTFile.xsl"));
        dsDocument.Dispose();
    }
    #endregion

    #region private void AddColumns()
    private void AddColumns(DataSet dsDocument)
    {
        DataTable myTable;
        DataColumn tempCol;

        if (dsDocument.Tables.Contains("tblChild"))
        {
            // VERY IMPORTANT
            // we use ISLOAD field in XSLT to load document data
            DataView viewChild = dsDocument.Tables["tblChild"].DefaultView;
            myTable = dsDocument.Tables["tblChild"];
            tempCol = new DataColumn();
            tempCol.Caption = "IsLoad";
            tempCol.DataType = Type.GetType("System.Int16");
            tempCol.DefaultValue = 0;
            tempCol.ColumnName = "IsLoad";
            myTable.Columns.Add(tempCol);

            foreach (DataRowView drv in dsDocument.Tables["tblParent"].DefaultView)
            {
                //viewChild.Sort = "Type_ID ASC, Original_EventDate DESC";
                viewChild.RowFilter = "DocumentType = " + drv["Type_ID"].ToString();
                viewChild.RowStateFilter = DataViewRowState.OriginalRows;
                if (viewChild.Count > 0)
                    viewChild[0]["IsLoad"] = 1;
            }
        }

        dsDocument.AcceptChanges();
    }
    #endregion

    #region private void FetchPatientGroupDocumentItems()
    private void FetchPatientGroupDocumentItems()
    {
        SqlCommand cmdSelect = new SqlCommand();
        DataSet dsDocument = new DataSet();

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_FileManagement_FetchPatientDocumentItems", true);
        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt64(gClass.OrganizationCode);
        cmdSelect.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt64(Request.Cookies["UserPracticeCode"].Value);
        cmdSelect.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt64(Request.Cookies["PatientID"].Value);
        cmdSelect.Parameters.Add("@DocumentType", SqlDbType.Int).Value = Convert.ToByte(Request.QueryString["QSV"]);
        cmdSelect.Parameters.Add("@ShowDeleted", SqlDbType.Bit).Value = Convert.ToByte(Request.QueryString["SD"]);

        dsDocument = gClass.FetchData(cmdSelect, "tblPatientDocumentItems");
        if (Request.QueryString["LT"].ToUpper() == "THUMBNAIL")
            Response.Write(dsDocument.GetXml());
        else if (Request.QueryString["LT"].ToUpper() == "DETAIL") 
            Response.Write(gClass.ShowSchema(dsDocument, Server.MapPath(".") + @"\Includes\RightFrame_DocumentsListXSLTFile.xsl"));
        
        dsDocument.Dispose();
    }
    #endregion

    #region private void FetchDocumentDetailInformation();
    private void FetchDocumentDetailInformation()
    {
        SqlCommand cmdSelect = new SqlCommand(), cmdCaption = new SqlCommand();
        DataSet dsDocument;

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_FileManagement_FetchDocumentDetialInformation", true);
        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSelect.Parameters.Add("@tblPatientDocumentsID", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["QSV"]);

        dsDocument = gClass.FetchData(cmdSelect, "tblDocumentDetailInformation");
        dsDocument.Tables.Add(gClass.FetchCaptions("DocumentDetailInformationXSLTFile", Request.Cookies["CultureInfo"].Value).Tables[0].Copy());
        dsDocument.AcceptChanges();


        DataColumn dcTemp = new DataColumn();
        dcTemp.DataType = Type.GetType("System.String");
        dcTemp.DefaultValue = Request.Cookies["PermissionLevel"].Value;
        dcTemp.ColumnName = "PermissionLevel";
        dsDocument.Tables[0].Columns.Add(dcTemp);
        dsDocument.AcceptChanges();


        Response.Write(gClass.ShowSchema(dsDocument, Server.MapPath(".") + @"\Includes\DocumentDetailInformationXSLTFile.xsl"));
        dsDocument.Dispose();
    }
    #endregion

    #region private void FetchEventData()
    private void FetchEventData()
    {
        SqlCommand cmdSelect = new SqlCommand(); 
        DataSet dsDocument = new DataSet();

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_FileManagement_FetchEventData", true);
        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSelect.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
        cmdSelect.Parameters.Add("@tblPatientDocumentsID", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["QSV"]);
        cmdSelect.Parameters.Add("@ImperialFlag", SqlDbType.Bit).Value = Request.Cookies["Imperial"].Value.Equals("True");
        cmdSelect.Parameters.Add("@EventLink", SqlDbType.Char).Value = Request.QueryString["EL"][0]; 

        dsDocument = gClass.FetchData(cmdSelect, "tblEventData");
        dsDocument.Tables.Add(gClass.FetchCaptions("EventDataXSLTFile", Request.Cookies["CultureInfo"].Value).Tables[0].Copy());
        dsDocument.AcceptChanges();
        Response.Write(gClass.ShowSchema(dsDocument, Server.MapPath(".") + @"\Includes\EventDataXSLTFile.xsl"));
        return;
    }
    #endregion

    #region private void FetchPatientGroupDocumentItems_OnEventDate
    private void FetchPatientGroupDocumentItems_OnEventDate()
    {
        SqlCommand cmdSelect = new SqlCommand();
        DataSet dsDocument = new DataSet();

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_FileManagement_FetchPatientDocumentItems_OnEventDate", true);
        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt64(gClass.OrganizationCode);
        cmdSelect.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt64(Request.Cookies["UserPracticeCode"].Value);
        cmdSelect.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt64(Request.Cookies["PatientID"].Value);
        cmdSelect.Parameters.Add("@DocumentType", SqlDbType.Int).Value = Convert.ToByte(Request.QueryString["QSV"]);
        cmdSelect.Parameters.Add("@EventDate", SqlDbType.DateTime).Value = Convert.ToDateTime(Request.QueryString["ED"].Replace("-", "/"));
        cmdSelect.Parameters.Add("@ShowDeleted", SqlDbType.Bit).Value = Convert.ToByte(Request.QueryString["SD"]);

        dsDocument = gClass.FetchData(cmdSelect, "tblPatientDocumentItems");
        if (Request.QueryString["LT"].ToUpper() == "THUMBNAIL")
            Response.Write(dsDocument.GetXml());
        else if (Request.QueryString["LT"].ToUpper() == "DETAIL")
            Response.Write(gClass.ShowSchema(dsDocument, Server.MapPath(".") + @"\Includes\RightFrame_DocumentsListXSLTFile.xsl"));

        dsDocument.Dispose();
        return;
    }
    #endregion

    #region private void FetchPatientGroupDocumentItems_OnEventLink
    private void FetchPatientGroupDocumentItems_OnEventLink()
    {
        SqlCommand cmdSelect = new SqlCommand();
        DataSet dsDocument = new DataSet();

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_FileManagement_FetchPatientDocumentItems_OnEventLink", true);
        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt64(gClass.OrganizationCode);
        cmdSelect.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt64(Request.Cookies["UserPracticeCode"].Value);
        cmdSelect.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt64(Request.Cookies["PatientID"].Value);
        cmdSelect.Parameters.Add("@DocumentType", SqlDbType.Int).Value = Convert.ToByte(Request.QueryString["QSV"]);
        cmdSelect.Parameters.Add("@EventDate", SqlDbType.DateTime).Value = Convert.ToDateTime(Request.QueryString["ED"].Replace("-", "/"));
        cmdSelect.Parameters.Add("@EventLink", SqlDbType.Char).Value = Request.QueryString["EL"]; 

        dsDocument = gClass.FetchData(cmdSelect, "tblPatientDocumentItems");
        if (Request.QueryString["LT"].ToUpper() == "THUMBNAIL")
            Response.Write(dsDocument.GetXml());
        else if (Request.QueryString["LT"].ToUpper() == "DETAIL")
            Response.Write(gClass.ShowSchema(dsDocument, Server.MapPath(".") + @"\Includes\RightFrame_DocumentsListXSLTFile.xsl"));

        dsDocument.Dispose();
    }
    #endregion

    #region private void DeleteDocument()
    private void DeleteDocument()
    {
        SqlCommand cmdDelete = new SqlCommand();

        gClass.MakeStoreProcedureName(ref cmdDelete, "sp_FileManagement_DeleteDocument", true) ;
        cmdDelete.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdDelete.Parameters.Add("@DocumentID", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["QSV"]);
        cmdDelete.Parameters.Add("@DeleteAction", SqlDbType.Bit).Value = (Request.QueryString["AC"] == "true");

        try
        {
            gClass.ExecuteDMLCommand(cmdDelete);
            AddDocumentEventLog(Convert.ToInt32(Request.QueryString["QSV"]), (byte)((Request.QueryString["AC"] == "true") ? 4 : 5));
            
            gClass.SaveActionLog(gClass.OrganizationCode,
                                         Request.Cookies["UserPracticeCode"].Value,
                                         Request.Url.Host,
                                         System.Configuration.ConfigurationManager.AppSettings["DocumentPage"].ToString(),
                                         System.Configuration.ConfigurationManager.AppSettings["LogDelete"].ToString(),
                                         "Delete " + System.Configuration.ConfigurationManager.AppSettings["DocumentPage"].ToString() + " Data",
                                         Request.Cookies["PatientID"].Value,
                                         Request.QueryString["QSV"]);

        }
        catch(Exception Error){
            Response.Write(Error.ToString());
        }
        return;
    }
    #endregion

    #region private void LoadDocument
    private void LoadDocumentContent()
    {
        SqlCommand      cmdSelect = new SqlCommand();
        DataSet         dsDocument;
        string          strFileName = "";
        string strScript = "";

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_FileManagement_FetchDocumentContent", true);
        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSelect.Parameters.Add("@DocumentID", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["DID"]);
        try
        {
            dsDocument = gClass.FetchData(cmdSelect, "tblDocumentContent");
            if (dsDocument.Tables.Count > 0)
                if (dsDocument.Tables["tblDocumentContent"].Rows.Count > 0)
                {
                    DataRow myRow = dsDocument.Tables["tblDocumentContent"].Rows[0];
                    byte[] oDocumentByteArray = (byte[])myRow["ContentFile"];

                    strFileName = GetDocumentPath(Server.MapPath(".") + "/Documents/") + Request.QueryString["DID"] + "_" + myRow["DocumentFileName"].ToString();
                    if (System.IO.File.Exists(strFileName)) System.IO.File.Delete(strFileName);
                    System.IO.FileStream fs = new System.IO.FileStream(strFileName, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write);
                    BinaryWriter bw = new BinaryWriter(fs);
                    bw.Write(oDocumentByteArray);
                    bw.Flush();
                    fs.Close();
                }
            dsDocument.Dispose();
            strFileName = Request.Url.Scheme + "://" + Request.Url.Host + Request.ApplicationPath + "/Forms/FileManagement" + strFileName.Replace(Server.MapPath("."), "");
                       
            Response.Write(strFileName);

            AddDocumentEventLog(Convert.ToInt32(Request.QueryString["DID"]), 2);
        }
        catch (Exception err) { Response.Write(err.ToString()); }
        return;
    }
    #endregion

    #region private void GetDocumentPath( )
    private string GetDocumentPath(string DocumentFolder)
    {
        System.IO.DirectoryInfo di;

        di = new System.IO.DirectoryInfo(DocumentFolder);
        if (di.Exists == false) di.Create();
        return (DocumentFolder);
    }
    #endregion

    #region private void AddDocumentEventLog
    private void AddDocumentEventLog(int intDocumentID, byte intEventCode)
    {
        gClass.AddDocumentEventLog(gClass.OrganizationCode, intDocumentID, intEventCode, Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value), Request.Url.Host, "");
    }
    #endregion

    #region private void FetchDocumentEventHistory
    private void FetchDocumentEventHistory()
    {
        SqlCommand cmdSelect = new SqlCommand();
        DataSet dsDocument = new DataSet();

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_FileManagement_FetchDocumentEventHistory", true);
        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSelect.Parameters.Add("@DocumentID", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["QSV"]);

        dsDocument = gClass.FetchData(cmdSelect, "tblDocumentHistory");
        Response.Write(gClass.ShowSchema(dsDocument, Server.MapPath(".") + @"\Includes\RightFrame_DocumentHistoryXSLTFile.xsl"));
        dsDocument.Dispose();
    }
    #endregion

}
