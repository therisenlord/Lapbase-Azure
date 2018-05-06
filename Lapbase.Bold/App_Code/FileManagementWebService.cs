using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;


/// <summary>
/// Summary description for FileManagementWebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class FileManagementWebService : System.Web.Services.WebService {

    GlobalClass gClass = new GlobalClass();

    #region public FileManagementWebService
    public FileManagementWebService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
        gClass.OrganizationCode = Context.Request.Cookies["OrganizationCode"].Value;
    }
    #endregion

    #region [WebMethod] public SaveDocumentDetail
    [WebMethod]
    public XmlDocument SaveDocumentDetail(string strDocumentID, string strDocumentName, string strDoc_Description)
    {
        SqlCommand    cmdUpdate = new SqlCommand();
        string          strResult = "";

        gClass.MakeStoreProcedureName(ref cmdUpdate, "sp_FileManagement_UpdateDocumentDetail", true);
        try
        {
            cmdUpdate.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
            cmdUpdate.Parameters.Add("@tblPatientDocumentsID", SqlDbType.Int).Value = Convert.ToInt32(strDocumentID);
            cmdUpdate.Parameters.Add("@DocumentName", SqlDbType.VarChar, 50).Value = strDocumentName;
            cmdUpdate.Parameters.Add("@Doc_Description", SqlDbType.VarChar, 1024).Value = strDoc_Description;

            gClass.ExecuteDMLCommand(cmdUpdate);
            strResult = "0";

            gClass.SaveActionLog(gClass.OrganizationCode,
                                         Context.Request.Cookies["UserPracticeCode"].Value,
                                         Context.Request.Url.Host,
                                         System.Configuration.ConfigurationManager.AppSettings["DocumentPage"].ToString(),
                                         System.Configuration.ConfigurationManager.AppSettings["LogUpdate"].ToString(),
                                         "Save " + System.Configuration.ConfigurationManager.AppSettings["DocumentPage"].ToString() + " Data Detail",
                                         Context.Request.Cookies["PatientID"].Value,
                                         strDocumentID);
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host, Context.Request.Cookies["Logon_UserName"].Value, "File Management Form", "Update File data (SaveDocumentDetail)", err.ToString());
            strResult = "-1";
        }
        return (gClass.GetXmlDocument(Guid.NewGuid(), strResult));
    }
    #endregion
}

