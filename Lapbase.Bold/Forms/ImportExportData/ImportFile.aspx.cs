using System;
using System.Transactions;
using System.Diagnostics;
using System.Configuration;
using System.Collections;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.Web.UI;
using Telerik.WebControls;

public partial class Forms_ImportExportData_ImportFile : System.Web.UI.Page
{

    //todo:
    //need to escape " on mdbpath
    //update records if its already exist before

    GlobalClass gClass = new GlobalClass();
    Boolean ExportFlag = false;
    private long FileSize = 0;
    private string strDocumentName = "";
    private string strLapdataPath = "";
    private byte[] oDocumentByteArray;
    private int SrcFileType = 0;
    private string strSqlConsult = "SELECT * from PatientConsult where videoLocation is not null;";
    private string strSqlConsultImage = "SELECT [Patient Id], ConsultId, DateSeen, ImageDate, ImageLocation, ImageName from PatientConsult where ImageLocation is not null and ImageLocation <> ''";
    private string strSqlConsultImage1 = "SELECT [Patient Id], ConsultId, DateSeen, ImageDate1 as ImageDate, ImageLocation1 as ImageLocation, ImageName1 as ImageName from PatientConsult where ImageLocation1 is not null and ImageLocation1 <> ''";
    private string strSqlConsultImage2 = "SELECT [Patient Id], ConsultId, DateSeen, ImageDate2 as ImageDate, ImageLocation2 as ImageLocation, ImageName2 as ImageName from PatientConsult where ImageLocation2 is not null and ImageLocation2 <> ''";

    private int visitFileSuccessNum = 0;
    private int visitFileFailNum = 0;
    private string visitFileFail = "";


    #region protected void Page_Load(object sender, EventArgs e)
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "no-cache");
        Response.Expires = -1;

        try
        {
            Page.Culture = Request.Cookies["CultureInfo"].Value;
            gClass.LanguageCode = Request.Cookies["LanguageCode"].Value;
            gClass.OrganizationCode = Request.Cookies["OrganizationCode"].Value;
            if (gClass.IsUserLogoned(Session.SessionID, Request.Cookies["UserPracticeCode"].Value, Request.Url.Host))
            {
                if (!IsPostBack)
                {
                    gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value,
                                            Request.Cookies["Logon_UserName"].Value,
                                            Request.Url.Host,
                                            "Import File Form", 2, "Browse", "", "");
                }
            }
            else
            {
                gClass.ReturnToLoginPage(Request.Url.Host, Request.Cookies["LanguageCode"].Value, Response);
            }
        }
        catch (Exception err)
        {
            string strLanguageCode;
            try
            {
                strLanguageCode = Request.Cookies["LanguageCode"].Value;
                gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Import/Export data form", "Loading Patient List (Page_Load function)", err.ToString());
            }
            catch { strLanguageCode = "en-US"; }
            gClass.ReturnToLoginPage(Request.Url.Host, strLanguageCode, Response);
        }
        return;
    }
    #endregion

    #region protected void ExportFromSqlServerToMDB(object sender, EventArgs e)
    protected void ExportFromSqlServerToMDB(object sender, EventArgs e)
    {
        System.Data.OleDb.OleDbConnection cnnDest = new System.Data.OleDb.OleDbConnection();
        System.Data.OleDb.OleDbCommand cmdDest;

        Char[] delimiter = { '\\' };
        String[] SrcFileArr;

        String strSql = "";
        String SrcFileID = "";
        String SrcFileName = "";
        String SrcDocumentName = "";
        String SrcUploadDate = "";
        String SrcPatientID = "";
        String SrcEventID = "0";    //0 for baseline, consultid for visit
        String SrcEventLink = "B";  //V, B or O
        String SrcFullFilePath = "";
        String SrcFolderPath = "";

        String DestFileName = "";
        strLapdataPath = "";

        strLapdataPath = txtFolder.Text;

        String strDocID = "";

        //notes
        int fileNum = 0;
        int fileSuccessNum = 0;
        int fileFailNum = 0;
        String fileFail = "";
        String baselineNotes = "";

        txtErrNotes.InnerHtml = "";

        //need to escape "
        String strMDBPath = strLapdataPath + "\\Lapdata.mdb";
        //String strMDBPath = "C:\\Lapdata\\Lapdata.mdb;";

        try
        {
            if (strLapdataPath == "")
                txtErrNotes.InnerHtml = "Warning: Please enter the Lapdata folder path";

            cnnDest.ConnectionString = "PROVIDER=Microsoft.Jet.OLEDB.4.0;Data Source=" + strMDBPath;
            cnnDest.Open();
            cmdDest = cnnDest.CreateCommand();
            cmdDest.CommandType = CommandType.Text;

            strSql = "Select * from tblDocuments where LinkName is not null and LinkName <> ''";

            cmdDest.CommandText = strSql;
            OleDbDataReader drSource = cmdDest.ExecuteReader(CommandBehavior.Default);

            while (drSource.Read())
            {
                FileSize = 0;
                strDocumentName = "";
                SrcFileType = 0;

                //for every data on tblDocuments in mdb file, upload file
                SrcFileID = drSource["LinikId"].ToString();
                SrcFullFilePath = drSource["LinkName"].ToString();
                SrcDocumentName = drSource["Description"].ToString();
                SrcUploadDate = drSource["Date"].ToString();
                SrcPatientID = drSource["PatientId"].ToString();
                SrcEventID = "0";
                SrcEventLink = "B";

                //remove # at the end of filename
                if (SrcFullFilePath.Substring((SrcFullFilePath.Length) - 1) == "#")
                    SrcFullFilePath = SrcFullFilePath.Replace("#", "");

                //split file path
                SrcFileArr = SrcFullFilePath.Split(delimiter);

                //get filename and folder path
                SrcFileName = SrcFileArr[(SrcFileArr.Length) - 1];
                SrcFolderPath = SrcFileArr[(SrcFileArr.Length) - 2];

                //call function to copy file
                DestFileName = copyFile(SrcFileName, SrcFolderPath);

                //save to db
                strDocID = SaveVisitDocumentData(SrcPatientID, SrcEventID, SrcUploadDate, SrcEventLink, SrcUploadDate, SrcFileType, DestFileName, SrcDocumentName);

                if (strDocID != "")
                    gClass.AddDocumentEventLog(gClass.OrganizationCode, Convert.ToInt32(strDocID), 1, Convert.ToInt64(Request.Cookies["UserPracticeCode"].Value), Request.Url.Host, "Upload file");

                //to be diplayed on notes
                fileNum++;

                if (strDocID != "")
                    fileSuccessNum++;
                else
                {
                    fileFailNum++;
                    fileFail += SrcFileID + " - ";
                }
            }
            baselineNotes = "Finish Uploading baseline file\n";
            baselineNotes += "-------------------------------------\n";
            baselineNotes += "Total file: " + fileNum + "\n";
            baselineNotes += "Success: " + fileSuccessNum + "\n";
            baselineNotes += "Failed: " + fileFailNum + "\n";
            baselineNotes += "Failed File (Link ID): " + fileFail + "\n\n\n";

            //update notes
            txtNotes.Value = txtNotes.Value + baselineNotes;
        }
        catch (Exception err)
        {
            ExportFlag = false;
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host,
                    Request.Cookies["Logon_UserName"].Value, "Import/Export data Form", "ExportFromSqlServerToMDB function",
                    strSql + " ... " + err.ToString());
        }
        finally { cnnDest.Close(); cnnDest.Dispose(); }
        return;
    }
    #endregion

    #region protected void CheckVisitRecord(object sender, EventArgs e)
    protected void CheckVisitRecord(object sender, EventArgs e)
    {
        System.Data.OleDb.OleDbConnection cnnDest = new System.Data.OleDb.OleDbConnection();
        System.Data.OleDb.OleDbCommand cmdDest;

        strLapdataPath = txtFolder.Text;

        txtErrNotes.InnerHtml = "";
        int numOfRec = 0;
        String strMDBPath = strLapdataPath + "\\Lapdata.mdb";
        //String strMDBPath = "C:\\Lapdata\\Lapdata.mdb;";
        try
        {
            if (strLapdataPath == "")
                txtErrNotes.InnerHtml = "Warning: Please enter the Lapdata folder path";

            cnnDest.ConnectionString = "PROVIDER=Microsoft.Jet.OLEDB.4.0;Data Source=" + strMDBPath;
            cnnDest.Open();
            cmdDest = cnnDest.CreateCommand();
            cmdDest.CommandType = CommandType.Text;

            cmdDest.CommandText = strSqlConsult;
            OleDbDataReader drSource = cmdDest.ExecuteReader(CommandBehavior.Default);


            while (drSource.Read())
            {
                numOfRec++;
            }
            lblNumofRec.Text = numOfRec.ToString();
        }
        catch (Exception err)
        {
            ExportFlag = false;
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host,
                    Request.Cookies["Logon_UserName"].Value, "Import/Export data Form", "CheckVisitRecord function",
                    strSqlConsult + " ... " + err.ToString());
        }
    }
    #endregion

    #region protected void ImportVisitDocument(object sender, EventArgs e)
    protected void ImportVisitDocument(object sender, EventArgs e)
    {
        System.Data.OleDb.OleDbConnection cnnDest = new System.Data.OleDb.OleDbConnection();
        System.Data.OleDb.OleDbCommand cmdDest;

        Char[] delimiter = { '\\' };
        String[] SrcFileArr;

        String SrcFileID = "";
        String SrcFileName = "";
        String SrcDocumentName = "";
        String SrcUploadDate = "";
        String SrcPatientID = "";
        String SrcEventID = "0";    //0 for baseline, consultid for visit
        String SrcEventLink = "V";  //V, B or O
        String SrcEventDate = "0";
        String SrcFullFilePath = "";
        String SrcFolderPath = "";

        String DestFileName = "";
        strLapdataPath = "";

        strLapdataPath = txtFolder.Text;
        String startRecord = txtStartRecord.Text;
        String endRecord = txtEndRecord.Text;
        startRecord = startRecord.Trim();
        endRecord = endRecord.Trim();

        int startNum = 0;
        int endNum = 0;

        int count = 0;
        String strDocID = "";

        //notes
        int fileNum = 0;
        int fileSuccessNum = 0;
        int fileFailNum = 0;
        String fileFail = "";
        String baselineNotes = "";

        txtErrNotes.InnerHtml = "";

        String strMDBPath = strLapdataPath + "\\Lapdata.mdb";
        //String strMDBPath = "C:\\Lapdata\\Lapdata.mdb;";


        if (startRecord == "" || startRecord == "0")
            txtErrNotes.InnerHtml = "Warning: Please enter the Starting record number on visit file";
        else if (endRecord == "" || endRecord == "0")
            txtErrNotes.InnerHtml = "Warning: Please enter the Ending record number on visit file";
        else
        {
            startNum = Convert.ToInt32(startRecord);
            endNum = Convert.ToInt32(endRecord);

            if (endNum > startNum)
            {
                try
                {
                    if (strLapdataPath == "")
                        txtErrNotes.InnerHtml = "Warning: Please enter the Lapdata folder path";

                    cnnDest.ConnectionString = "PROVIDER=Microsoft.Jet.OLEDB.4.0;Data Source=" + strMDBPath;
                    cnnDest.Open();
                    cmdDest = cnnDest.CreateCommand();
                    cmdDest.CommandType = CommandType.Text;

                    cmdDest.CommandText = strSqlConsult;
                    OleDbDataReader drSource = cmdDest.ExecuteReader(CommandBehavior.Default);

                    while (drSource.Read())
                    {
                        count++;

                        if (count >= startNum && count <= endNum)
                        {
                            FileSize = 0;
                            strDocumentName = "";
                            SrcFileType = 0;

                            SrcPatientID = drSource["Patient Id"].ToString();
                            SrcEventID = drSource["ConsultId"].ToString();
                            SrcEventDate = drSource["DateSeen"].ToString();
                            SrcUploadDate = drSource["VideoDate"].ToString();
                            SrcFileName = drSource["VideoLocation"].ToString();
                            SrcDocumentName = drSource["VideoName"].ToString();
                            SrcEventLink = "V";

                            //call function to copy file
                            DestFileName = copyFile(SrcFileName, "IMAGES");

                            //save to db                
                            strDocID = SaveVisitDocumentData(SrcPatientID, SrcEventID, SrcEventDate, SrcEventLink, SrcUploadDate, SrcFileType, DestFileName, SrcDocumentName);
                            if (strDocID != "")
                                gClass.AddDocumentEventLog(gClass.OrganizationCode, Convert.ToInt32(strDocID), 1, Convert.ToInt64(Request.Cookies["UserPracticeCode"].Value), Request.Url.Host, "Upload visit file");

                            //to be diplayed on notes
                            fileNum++;

                            if (strDocID != "")
                                fileSuccessNum++;
                            else
                            {
                                fileFailNum++;
                                fileFail += SrcEventID + " - ";
                            }
                        }
                    }
                    baselineNotes = "Finish Uploading Visit Video File\n";
                    baselineNotes += "-------------------------------------\n";
                    baselineNotes += "Record number: " + startNum.ToString() + " - " + endNum.ToString() + "\n";
                    baselineNotes += "Total file: " + fileNum + "\n";
                    baselineNotes += "Success: " + fileSuccessNum + "\n";
                    baselineNotes += "Failed: " + fileFailNum + "\n";
                    baselineNotes += "Failed File (Consult ID): " + fileFail + "\n\n\n";

                    //update notes
                    txtNotes.Value = txtNotes.Value + baselineNotes;
                }
                catch (Exception err)
                {
                    ExportFlag = false;
                    gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host,
                            Request.Cookies["Logon_UserName"].Value, "Import/Export data Form", "ExportFromSqlServerToMDB function",
                            strSqlConsult + " ... " + err.ToString());
                }
                finally { cnnDest.Close(); cnnDest.Dispose(); }
            }
            else
                txtErrNotes.InnerHtml = "Warning: Please enter the proper range";
        }
        return;
    }
    #endregion

    #region protected void ImportVisitImageDocument(object sender, EventArgs e)
    protected void ImportVisitImageDocument(object sender, EventArgs e)
    {
        Char[] delimiter = { '\\' };
        String[] SrcFileArr;

        String SrcFileID = "";
        String SrcFileName = "";
        String SrcDocumentName = "";
        String SrcUploadDate = "";
        String SrcPatientID = "";
        String SrcEventID = "0";    //0 for baseline, consultid for visit
        String SrcEventLink = "V";  //V, B or O
        String SrcEventDate = "0";
        String SrcFullFilePath = "";
        String SrcFolderPath = "";

        String DestFileName = "";
        strLapdataPath = "";

        strLapdataPath = txtFolder.Text;

        int count = 0;
        String strDocID = "";

        //notes
        int fileNum = 0;
        int fileSuccessNum = 0;
        int fileFailNum = 0;
        String fileFail = "";
        String baselineNotes = "";

        txtErrNotes.InnerHtml = "";

        String strMDBPath = strLapdataPath + "\\Lapdata.mdb";

        try
        {
            if (strLapdataPath == "")
                txtErrNotes.InnerHtml = "Warning: Please enter the Lapdata folder path";


            for (int imageCount = 1; imageCount <= 3; imageCount++)
            {
                System.Data.OleDb.OleDbConnection cnnDest = new System.Data.OleDb.OleDbConnection();
                System.Data.OleDb.OleDbCommand cmdDest;

                cnnDest.ConnectionString = "PROVIDER=Microsoft.Jet.OLEDB.4.0;Data Source=" + strMDBPath;
                cnnDest.Open();
                cmdDest = cnnDest.CreateCommand();
                cmdDest.CommandType = CommandType.Text;

                switch (imageCount)
                {
                    case 1:
                        cmdDest.CommandText = strSqlConsultImage;
                        break;
                    case 2:
                        cmdDest.CommandText = strSqlConsultImage1;
                        break;
                    case 3:
                        cmdDest.CommandText = strSqlConsultImage2;
                        break;
                }

                OleDbDataReader drSource = cmdDest.ExecuteReader(CommandBehavior.Default);

                while (drSource.Read())
                {
                    count++;
                    FileSize = 0;
                    strDocumentName = "";
                    SrcFileType = 0;

                    SrcPatientID = drSource["Patient Id"].ToString();
                    SrcEventID = drSource["ConsultId"].ToString();
                    SrcEventDate = drSource["DateSeen"].ToString();
                    SrcUploadDate = drSource["ImageDate"].ToString();
                    SrcFileName = drSource["ImageLocation"].ToString();
                    SrcDocumentName = drSource["ImageName"].ToString();
                    SrcEventLink = "V";

                    //call function to copy file
                    DestFileName = copyFile(SrcFileName, "PHOTOS");

                    //save to db                
                    strDocID = SaveVisitDocumentData(SrcPatientID, SrcEventID, SrcEventDate, SrcEventLink, SrcUploadDate, SrcFileType, DestFileName, SrcDocumentName);
                    if (strDocID != "")
                        gClass.AddDocumentEventLog(gClass.OrganizationCode, Convert.ToInt32(strDocID), 1, Convert.ToInt64(Request.Cookies["UserPracticeCode"].Value), Request.Url.Host, "Upload visit file");

                    //to be diplayed on notes
                    fileNum++;

                    if (strDocID != "")
                        fileSuccessNum++;
                    else
                    {
                        fileFailNum++;
                        fileFail += SrcEventID + " - ";
                    }
                }
                cnnDest.Close(); cnnDest.Dispose();
            }

            baselineNotes = "Finish Uploading Visit Image File\n";
            baselineNotes += "-------------------------------------\n";
            baselineNotes += "Total file: " + fileNum + "\n";
            baselineNotes += "Success: " + fileSuccessNum + "\n";
            baselineNotes += "Failed: " + fileFailNum + "\n";
            baselineNotes += "Failed File (Consult ID): " + fileFail + "\n\n\n";

            //update notes
            txtNotes.Value = txtNotes.Value + baselineNotes;
        }
        catch (Exception err)
        {
            ExportFlag = false;
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host,
                   Request.Cookies["Logon_UserName"].Value, "Import/Export data Form", "ImportVisitImageDocument function",
                   strSqlConsult + " ... " + err.ToString());
        }
        return;
    }
    #endregion

    #region private string copyFile(string SrcFileName, string SrcFolderPath)
    private string copyFile(string SrcFileName, string SrcFolderPath)
    {
        int iPlace = 0;

        String DestFileName = "";
        String DestFolderPath = "";
        String DestFullFilePath = "";
        String SrcFileExtension = "";
        String NewSrcFullFilePath = "";


        String SrcFileSecondExtension = "";
        try
        {
            NewSrcFullFilePath = System.IO.Path.Combine(strLapdataPath, SrcFolderPath);
            NewSrcFullFilePath = System.IO.Path.Combine(NewSrcFullFilePath, SrcFileName);

            //get file extension and file type
            SrcFileExtension = SrcFileName.Substring(SrcFileName.LastIndexOf(".") + 1);
            SrcFileType = GetFileType(SrcFileExtension);

            iPlace = SrcFileName.IndexOf(".");
            DestFileName = SrcFileName.Remove(iPlace, 1).Insert(iPlace, "_");

            //check if extension is repeated .mpg.mpg
            SrcFileSecondExtension = SrcFileName.Substring(0, SrcFileName.LastIndexOf("."));
            SrcFileSecondExtension = SrcFileSecondExtension.Substring(SrcFileSecondExtension.LastIndexOf(".") + 1);

            if (SrcFileExtension.ToLower() == SrcFileSecondExtension.ToLower())
            {
                NewSrcFullFilePath = NewSrcFullFilePath.Substring(0, NewSrcFullFilePath.LastIndexOf("."));
                DestFileName = DestFileName.Substring(0, SrcFileName.LastIndexOf("."));
            }
            DestFolderPath = System.IO.Path.Combine(this.Context.ApplicationInstance.Request.PhysicalApplicationPath, GetDocumentPath(SrcFileType));
            DestFullFilePath = System.IO.Path.Combine(DestFolderPath, DestFileName);

            //copy file, overwrite it when exist
            File.Copy(NewSrcFullFilePath, DestFullFilePath, true);


            if (SrcFileType == 2)
            {
                DestFileName = Convert2FLVfile(DestFileName, DestFullFilePath);
            }
            else
            {
                GetFileSize(DestFullFilePath);
                GetFileBinaryData(DestFullFilePath);
            }
        }
        catch (Exception err)
        {
        }
        return DestFileName;
    }
    #endregion

    #region private string SaveVisitDocumentData
    private string SaveVisitDocumentData(string patientID, string eventID, string eventDate, string eventLink, string uploadDate, int fileType, string strDocumentName, string strDocDescription)
    {
        /*
        //try to update if file already uploaded as baseline
        gClass.MakeStoreProcedureName(ref cmdUpdate, "sp_FileManagement_PatientDocuments_UpdateData", true);
        cmdUpdate.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdUpdate.Parameters.Add("@PatientId", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
        cmdUpdate.Parameters.Add("@DocumentFileName", System.Data.SqlDbType.VarChar, 100).Value = strDocumentName;
        cmdUpdate.Parameters.Add("@EventID", System.Data.SqlDbType.Int).Value = Convert.ToInt64(eventID);
        cmdUpdate.Parameters.Add("@EventDate", System.Data.SqlDbType.DateTime);
        cmdUpdate.Parameters.Add("@EventLink", System.Data.SqlDbType.Char).Value = eventLink;
        cmdUpdate.Parameters.Add("@DocumentName", System.Data.SqlDbType.VarChar, 50).Value = strDocDescription;

        try{cmdUpdate.Parameters["@EventDate"].Value = Convert.ToDateTime(eventDate);}
        catch { cmdUpdate.Parameters["@EventDate"].Value = DBNull.Value; }

        try
        {
            gClass.ExecuteDMLCommand(cmdUpdate);

            gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value, Request.Cookies["Logon_UserName"].Value, Request.Url.Host,
                                "Upload Document Form", "Update visit document function", err.ToString());
        }
        */

        GlobalClass gClass = new GlobalClass();
        string strReturn = "";
        System.Data.SqlClient.SqlCommand cmdSave = new System.Data.SqlClient.SqlCommand(), cmdSaveContent = new System.Data.SqlClient.SqlCommand();

        gClass.MakeStoreProcedureName(ref cmdSave, "sp_FileManagement_PatientDocuments_InsertData", true);
        gClass.MakeStoreProcedureName(ref cmdSaveContent, "sp_FileManagement_PatientDocumentsContent_InsertData", true);

        // Adding Parameters
        cmdSave.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSave.Parameters.Add("@UserPracticeCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
        cmdSave.Parameters.Add("@PatientID", System.Data.SqlDbType.Int).Value = Convert.ToInt64(patientID);
        cmdSave.Parameters.Add("@EventID", System.Data.SqlDbType.Int);
        cmdSave.Parameters.Add("@EventDate", System.Data.SqlDbType.DateTime);
        cmdSave.Parameters.Add("@EventLink", System.Data.SqlDbType.Char);
        cmdSave.Parameters.Add("@DocumentType", System.Data.SqlDbType.SmallInt);
        cmdSave.Parameters.Add("@DocumentFileName", System.Data.SqlDbType.VarChar, 100);
        cmdSave.Parameters.Add("@DocumentName", System.Data.SqlDbType.VarChar, 50);
        cmdSave.Parameters.Add("@DocumentFileSize", System.Data.SqlDbType.Int);
        cmdSave.Parameters.Add("@UploadDate", System.Data.SqlDbType.DateTime);
        cmdSave.Parameters.Add("@Doc_Description", System.Data.SqlDbType.VarChar, 1024);

        cmdSaveContent.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSaveContent.Parameters.Add("@UserPracticeCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
        cmdSaveContent.Parameters.Add("@tblPatientDocumentsID", System.Data.SqlDbType.Int);
        cmdSaveContent.Parameters.Add("@ContentFile", System.Data.SqlDbType.Image);

        // Initialising Parameters
        cmdSave.Parameters["@EventID"].Value = Convert.ToInt64(eventID);

        cmdSave.Parameters["@EventLink"].Value = eventLink;

        try
        {
            cmdSave.Parameters["@EventDate"].Value = Convert.ToDateTime(eventDate);
        }
        catch { cmdSave.Parameters["@EventDate"].Value = DBNull.Value; }


        cmdSave.Parameters["@DocumentType"].Value = fileType;
        cmdSave.Parameters["@DocumentFileName"].Value = strDocumentName;
        cmdSave.Parameters["@DocumentName"].Value = strDocDescription;
        cmdSave.Parameters["@DocumentFileSize"].Value = FileSize;
        try { cmdSave.Parameters["@UploadDate"].Value = Convert.ToDateTime(eventDate); }
        catch { cmdSave.Parameters["@UploadDate"].Value = DBNull.Value; }
        cmdSave.Parameters["@Doc_Description"].Value = strDocDescription;
        cmdSaveContent.Parameters["@ContentFile"].Value = oDocumentByteArray;

        try
        {
            if (FileSize > 0)
                strReturn = gClass.SaveDocumentAndContent(cmdSave, cmdSaveContent).ToString();
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Upload Document Form", "Import document function", err.ToString());
        }
        return strReturn;
    }
    #endregion

    #region private int GetFileType(string fileExtension)
    private int GetFileType(string fileExtension)
    {
        //get file type based on file extension
        int fileType = 0;
        switch (fileExtension.ToUpper())
        {
            case "JPG":
            case "JPEG":
            case "BMP":
            case "PNG":
            case "GIF":
            case "PSD":
            case "PSP":
            case "TIF":
                fileType = 1;
                break;

            case "3GP":
            case "ASF":
            case "ASX":
            case "MP4":
            case "QT":
            case "MOV":
            case "WMV":
            case "AVI":
            case "MPG":
            case "MPEG":
            case "RM":
            case "SWF":
                fileType = 2;
                break;

            default:
                fileType = 3;
                break;
        }
        return (fileType);
    }
    #endregion

    #region private string GetDocumentPath(int intFileType)
    private string GetDocumentPath(int intFileType)
    {
        //get document path based on file type
        string uploadFolder, strFolder = "";
        System.IO.DirectoryInfo di;

        switch (intFileType)
        {
            case 1:
                strFolder = "Photos";
                break;
            case 2:
                strFolder = "Videos";
                break;
            case 3:
                strFolder = "Documents";
                break;
        }

        uploadFolder = System.IO.Path.Combine(this.Context.ApplicationInstance.Request.PhysicalApplicationPath, strFolder);

        di = new System.IO.DirectoryInfo(uploadFolder);
        if (di.Exists == false) // Create the directory only if it does not already exist.
            di.Create();
        return (uploadFolder);
    }
    #endregion

    #region private void GetFileSize
    private void GetFileSize(string strFileName)
    {
        System.IO.FileInfo finfo = new System.IO.FileInfo(strFileName);
        FileSize = finfo.Length;
    }
    #endregion

    #region private void GetFileBinaryData
    private void GetFileBinaryData(string filePath)
    {
        System.IO.FileStream oImg;
        System.IO.BinaryReader oBinaryReader;

        oImg = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
        oBinaryReader = new System.IO.BinaryReader(oImg);
        oDocumentByteArray = oBinaryReader.ReadBytes((int)oImg.Length);
        oBinaryReader.Close();
        oImg.Close();
    }
    #endregion

    #region private string Convert2FLVfile(string fileName, string strVideoFullFileName)
    private string Convert2FLVfile(string fileName, string strVideoFullFileName)
    {
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(System.IO.Path.Combine(Request.PhysicalApplicationPath, @"ffmpeg\ffmpeg.exe"));
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Minimized;

        System.Diagnostics.Process myProcess;
        string strArguments, strFLVFileName;




        strDocumentName = System.IO.Path.GetFileNameWithoutExtension(fileName) + ".flv";
        strArguments = "-y -i \"" + strVideoFullFileName + "\" -acodec mp3 -ar 22050 -sameq -f flv ";

        strFLVFileName = System.IO.Path.Combine(GetDocumentPath(2), strDocumentName);

        startInfo.Arguments = strArguments + "\"" + strFLVFileName + "\"";

        // strFLVFileName = "C:\\Lapdata\\2.18-2-06.flv";
        // startInfo.Arguments = "-y -i C:\\Lapdata\\IMAGES\\2.18-2-06.MPG -acodec mp3 -ar 22050 -sameq -f flv "+strFLVFileName;


        myProcess = System.Diagnostics.Process.Start(startInfo);

        while (!myProcess.HasExited)
        {
            myProcess.Refresh();
            System.Threading.Thread.Sleep(5000); // wait for 5 seconds, then check it again
        }
        myProcess.Dispose();

        GetFileSize(strFLVFileName);
        GetFileBinaryData(strFLVFileName);

        return strDocumentName;
    }
    #endregion

    #region protected void DeleteDocument(object sender, EventArgs e)
    protected void DeleteDocument(object sender, EventArgs e)
    {
        SqlCommand cmdSelect = new SqlCommand(), cmdDelete = new SqlCommand();
        DataSet dsPatient;
        string currFileName, DestFolderPath, docTypeFolder = "";
        int docTypeSelected = 0;
        string patientDocumentID = "";

        int fileSkipNum = 0;
        int fileNum = 0;
        int fileSuccessNum = 0;
        int fileFailNum = 0;
        String fileFail = "";
        String baselineNotes = "";

        try
        {
            gClass.MakeStoreProcedureName(ref cmdSelect, "sp_FileManagement_FetchAllDocuments", true);
            gClass.MakeStoreProcedureName(ref cmdDelete, "sp_FileManagement_RemoveDocument", true);

            cmdDelete.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt64(gClass.OrganizationCode);
            cmdDelete.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt64(Request.Cookies["UserPracticeCode"].Value);
            cmdDelete.Parameters.Add("@DocumentID", SqlDbType.Int);

            cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt64(gClass.OrganizationCode);
            cmdSelect.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt64(Request.Cookies["UserPracticeCode"].Value);

            if (radPhoto.Checked == true)
            {
                docTypeSelected = 1;
                docTypeFolder = "Photos";
            }
            else if (radDoc.Checked == true)
            {
                docTypeSelected = 3;
                docTypeFolder = "Documents";
            }
            else if (radVideo.Checked == true)
            {
                docTypeSelected = 2;
                docTypeFolder = "Videos";
            }

            cmdSelect.Parameters.Add("@DocType", SqlDbType.Int).Value = docTypeSelected;

            dsPatient = gClass.FetchData(cmdSelect, "tblConsultFU1_ProgressNotes");

            for (int Idx = 0; Idx < dsPatient.Tables[0].Rows.Count; Idx++)
            {
                fileNum++;
                currFileName = dsPatient.Tables[0].Rows[Idx]["DocumentFileName"].ToString();
                patientDocumentID = dsPatient.Tables[0].Rows[Idx]["tblPatientDocumentsID"].ToString();
                DestFolderPath = System.IO.Path.Combine(docTypeFolder, currFileName);
                DestFolderPath = System.IO.Path.Combine(this.Context.ApplicationInstance.Request.PhysicalApplicationPath,DestFolderPath);
                if (File.Exists(DestFolderPath))
                {
                    //if its video, ignore file extension
                    if (docTypeSelected == 2)
                    {
                        File.Delete(DestFolderPath);
                        DestFolderPath = DestFolderPath.Replace(".FLV", ".MPG");
                        File.Delete(DestFolderPath);
                    }
                    else
                        File.Delete(DestFolderPath);



                    if (File.Exists(DestFolderPath))
                    {
                        fileFailNum++;
                        fileFail += DestFolderPath + " - ";
                    }
                    else
                    {
                        cmdDelete.Parameters["@DocumentID"].Value = Convert.ToInt64(patientDocumentID);

                        gClass.ExecuteDMLCommand(cmdDelete);

                        fileSuccessNum++;
                    }
                }
                else
                    fileSkipNum++;

            }

            baselineNotes = "Finish Deleting " + docTypeFolder + " File\n";
            baselineNotes += "-------------------------------------\n";
            baselineNotes += "Total record: " + fileNum + "\n";
            baselineNotes += "Success: " + fileSuccessNum + "\n";
            baselineNotes += "Failed: " + fileFailNum + "\n";
            baselineNotes += "Skipped: " + fileSkipNum + "\n";
            baselineNotes += "Failed File : " + fileFail + "\n\n\n";

            //update notes
            txtNotes.Value = txtNotes.Value + baselineNotes;
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host,
                Request.Cookies["Logon_UserName"].Value, "Delete Data Form", "DeleteDocument function",
                err.ToString());
        }
    }
    #endregion
}
