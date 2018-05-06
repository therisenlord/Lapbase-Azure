using System;
using System.Web;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.IO;
using System.Xml;
using System.Text;

/// <summary>
/// Summary description for UploadWebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class UploadWebService : System.Web.Services.WebService {

    public UploadWebService () {
        //Uncomment the following line if using designed components 
    }
    
    #region [WebMethod] public XmlDocument SaveVisitDocumentData
    [WebMethod]
    public XmlDocument SaveVisitDocumentData(string fileName, int filetype, string strDocumentName, string strDoc_Description)
    {
        string filePath = GetFilePath(fileName, filetype), strReturn;
        SqlCommand cmdSave = new SqlCommand();
        GlobalClass gClass = new GlobalClass();

        try
        {
            gClass.MakeStoreProcedureName(ref cmdSave, "sp_ConsultFU1_VisitDocument_InsertData", true);

            cmdSave.Parameters.Add("@PatientID", SqlDbType.Int);
            cmdSave.Parameters.Add("@ConsultID", SqlDbType.Int);
            cmdSave.Parameters.Add("@DocumentType", SqlDbType.SmallInt);
            cmdSave.Parameters.Add("@DocumentFileName", SqlDbType.VarChar, 100);
            cmdSave.Parameters.Add("@DocumentName", SqlDbType.VarChar, 50);
            cmdSave.Parameters.Add("@Doc_Description", SqlDbType.VarChar, 1024);
            cmdSave.Parameters.Add("@ContentFile", SqlDbType.Image);

            cmdSave.Parameters["@PatientID"].Value = Convert.ToInt64(Context.Request.Cookies["PatientID"].Value);
            cmdSave.Parameters["@ConsultID"].Value = Convert.ToInt64(Context.Request.Cookies["ConsultID"].Value);
            cmdSave.Parameters["@DocumentType"].Value = filetype;
            cmdSave.Parameters["@DocumentFileName"].Value = fileName;
            cmdSave.Parameters["@DocumentName"].Value = strDocumentName;
            cmdSave.Parameters["@Doc_Description"].Value = strDoc_Description;
            cmdSave.Parameters["@ContentFile"].Value = DBNull.Value;

            gClass.ExecuteDMLCommand(cmdSave);
            strReturn = string.Empty;
        }
        catch (Exception err)
        {
            strReturn = err.ToString();
        }

        return gClass.GetXmlDocument(Guid.NewGuid(), strReturn, 0, 0);
    }
    #endregion

    #region private string GetFilePath
    private string GetFilePath(string fileName, int intFileType)
    {
        string uploadFolder, strFolder = "" ;
        switch (intFileType)
        {
            case 1 : // Image
                strFolder = "Photos";
                break;
            case 2 :
                strFolder = "Videos";
                break;
        }
        uploadFolder = Path.Combine(this.Context.ApplicationInstance.Request.PhysicalApplicationPath, strFolder);
        return Path.Combine(uploadFolder, fileName);
    }
    #endregion

    #region private byte[] GetFileBinaryData
    private byte[] GetFileBinaryData(string filePath)
    {
        FileStream oImg;
        BinaryReader oBinaryReader;
        byte[] oImgByteArray;

        oImg = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        oBinaryReader = new BinaryReader(oImg);
        oImgByteArray = oBinaryReader.ReadBytes((int)oImg.Length);
        oBinaryReader.Close();
        oImg.Close();
        return (oImgByteArray);
    }
    #endregion

    //#region [WebMethod] public XmlDocument DeleteVisitDocumentData
    //[WebMethod]
    //public XmlDocument DeleteVisitDocumentData(string fileName, int filetype)
    //{
    //    string filePath = GetFilePath(fileName, filetype), strReturn;
    //    OleDbCommand cmdDelete = new System.Data.OleDb.OleDbCommand();
    //    GlobalClass gClass = new GlobalClass();

    //    try
    //    {
    //        gClass.MakeStoreProcedureName(ref cmdDelete, "sp_ConsultFU1_VisitDocument_DeleteData", true);
    //        cmdDelete.Parameters.Add("@ConsultID", System.Data.OleDb.OleDbType.BigInt).Value = Convert.ToInt64(Context.Request.Cookies["ConsultID"].Value); //GlobalClass.ConsultID;
    //        cmdDelete.Parameters.Add("@DocumentFileName", System.Data.OleDb.OleDbType.VarChar, 100).Value = fileName;
    //        cmdDelete.Parameters.Add("@DocumentType", System.Data.OleDb.OleDbType.SmallInt).Value = filetype;

    //        gClass.ExecuteDMLCommand(cmdDelete);
    //        strReturn = Convert.ToInt64(Context.Request.Cookies["ConsultID"].Value) + " " + fileName + " " + filetype;

    //        if (System.IO.File.Exists(filePath))    System.IO.File.Delete(filePath);
    //    }
    //    catch (Exception err)
    //    {
    //        strReturn = err.ToString();
    //    }

    //    return gClass.GetXmlDocument(Guid.NewGuid(), strReturn, 0, 0);
    //}
    //#endregion
}

