using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Telerik.WebControls;
using Microsoft.SqlServer.Management;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System.Net.Mail;
using Microsoft.Web.UI;
using System.Transactions;

public partial class Forms_ImportExportData_ImportExportForm : System.Web.UI.Page
{
    #region constants
    const String    M_LAPBASE_CODES = "tblcodes_staging",
                    M_LAPBASE_DOCTORS = "tbldoctors_staging",
                    M_LAPBASE_IDEALWEIGHTS = "tblidealweights_staging",
                    M_LAPBASE_SYSTEMCODES = "tblsystemcodes_staging",
                    M_LAPBASE_SYSTEMNORMALS = "tblsystemnormals_staging",
                    M_LAPBASE_HOSPITALS = "tblhospitals_staging",
                    M_LAPBASE_REFERRINGDOCTORS = "tblreferringdoctors_staging",
                    M_LAPBASE_PATIENTS = "tblpatients_staging",
                    M_LAPBASE_PATIENTWEIGHTDATA = "tblpatientweightdata_staging",
                    M_LAPBASE_PATIENTCONSULT = "tblpatientconsult_staging",
                    M_LAPBASE_OPEVENTS = "tblopevents_staging",
                    M_LAPBASE_COMPLICATION = "tblcomplications_staging";
    #endregion 

    #region Global Variables
    private bool    ImperialFlag = false;
    GlobalClass     gClass = new GlobalClass();
    string          strResult = "", strFileName;
    int             intPatients = 0, Org_intPatients = 0,
                    intVisits = 0, Org_intVisits = 0,
                    intOperations = 0, Org_intOperations = 0,
                    intComplications = 0, Org_intComplications = 0,
                    intSystemCodes = 0, Org_intSystemCodes = 0,
                    intCodes = 0, Org_intCodes = 0,
                    intDoctors = 0, Org_intDoctors = 0,
                    intHospitals = 0, Org_intHospitals = 0,
                    Org_intSystemNormals = 0,
                    intReferringDoctors = 0, Org_intReferringDoctors = 0, 
                    intPatientWeightData = 0, Org_intPatientWeightData = 0;

    string[,] strTablesName = { 
                                {"Codes", M_LAPBASE_CODES, "0", "0"}
                                , {"Doctors", M_LAPBASE_DOCTORS , "0", "0"}
                                , {"IdealWeights", M_LAPBASE_IDEALWEIGHTS , "0", "0"}
                                , {"SystemCodes", M_LAPBASE_SYSTEMCODES , "0", "0"}
                               //{"SystemDetails", "tblSystemDetails"}, ????????????????????????????
                                , {"SystemNormals", M_LAPBASE_SYSTEMNORMALS , "0", "0"}
                                , {"Hospitals", M_LAPBASE_HOSPITALS , "0", "0"}
                                , {"ReferringDoctors", M_LAPBASE_REFERRINGDOCTORS , "0", "0"}
                                , {"Patients", M_LAPBASE_PATIENTS , "0", "0"}
                                , {"PatientWeightData", M_LAPBASE_PATIENTWEIGHTDATA , "0", "0"}
                                , {"PatientConsult", M_LAPBASE_PATIENTCONSULT , "0", "0"}
                                , {"OpEvents", M_LAPBASE_OPEVENTS , "0", "0"}
                                , {"tblComplications", M_LAPBASE_COMPLICATION , "0", "0"}
                              };
    int intRank = 4;

    System.Collections.Generic.List<Int32> intListExistFields;
    System.Collections.Generic.List<String[,]> strListNonExistFields = new System.Collections.Generic.List<String[,]>();
    #endregion

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
            txtHApplicationURL.Value = Request.Url.Scheme + "://" + Request.Url.Host + Request.ApplicationPath + "/";
            bodyImportExport.Style.Add("Direction", Request.Cookies["Direction"].Value);
            if (gClass.IsUserLogoned(Session.SessionID, Request.Cookies["UserPracticeCode"].Value, Request.Url.Host))
            {
                if (!IsPostBack)
                {
                    //Do not display SelectedFilesCount progress indicator.
                    RadProgressArea1.ProgressIndicators &= ~ProgressIndicators.SelectedFilesCount;

                    RadProgressArea1.Localization["UploadedFiles"] = "Processed ";
                    RadProgressArea1.Localization["TotalFiles"] = "";
                    RadProgressArea1.Localization["CurrentFileName"] = "File: ";

                    RadProgressContext progress = RadProgressContext.Current;
                    //Prevent the secondary progress from appearing when the file is uploaded (FileCount etc.)
                    progress["SecondaryTotal"] = "0";
                    progress["SecondaryValue"] = "0";
                    progress["SecondaryPercent"] = "0";

                    gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value,
                                            Request.Cookies["Logon_UserName"].Value,
                                            Request.Url.Host,
                                            "Import/Export Data Form", 2, "Browse", "", "");
                }
            }
            else 
                gClass.ReturnToLoginPage(Request.Url.Host, Request.Cookies["LanguageCode"].Value, Response);
        }
        catch (Exception err)
        {
            string strLanguageCode;
            try
            {
                strLanguageCode = Request.Cookies["LanguageCode"].Value;
                gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Import/Export Page (Second stage)", " (Page_Load function)", err.ToString());
            }
            catch { strLanguageCode = "en-US"; }
            gClass.ReturnToLoginPage(Request.Url.Host, strLanguageCode, Response);
        }
    }
    #endregion 

    #region protected void btnUploadMDB_onclick(object sender, EventArgs e)
    protected void btnUploadMDB_onclick(object sender, EventArgs e)
    {
        //UploadedFile file = RadUploadContext.Current.UploadedFiles[inputFile1.UniqueID];
        DataTable dtTable = new DataTable();
        DataColumn dcCol = new DataColumn("FileName", Type.GetType("System.String"));
        dtTable.Columns.Add(dcCol);

        dcCol = new DataColumn("ContentLength", Type.GetType("System.String"));
        dtTable.Columns.Add(dcCol);
        dtTable.AcceptChanges();

        txtHFileName.Value = "";
        for (int Xh = 1; Xh <= RadUploadContext.Current.UploadedFiles.Count; Xh++)
        {
            HtmlInputFile inputFile = (HtmlInputFile)(FindControl("inputFile" + Xh));
            UploadedFile file = RadUploadContext.Current.UploadedFiles[inputFile.UniqueID];

            if (file != null)
            {
                const int total = 100;

                RadProgressContext progress = RadProgressContext.Current;
                for (int i = 0; i < total; i++)
                {
                    progress["SecondaryTotal"] = total.ToString();
                    progress["SecondaryValue"] = i.ToString();
                    progress["SecondaryPercent"] = i.ToString();
                    progress["CurrentOperationText"] = file.GetName() + " is being processed...";

                    if (!Response.IsClientConnected)
                    {
                        //Cancel button was clicked or the browser was closed, so stop processing
                        break;
                    }

                    //Stall the current thread for 0.1 seconds
                    System.Threading.Thread.Sleep(100);
                }

                string strMDBPath = Server.MapPath(".") + "/MDB/" + Request.Cookies["UserPracticeCode"].Value + file.GetName();
                try { System.IO.File.Delete(strMDBPath); }
                catch { }
                try
                {
                    DataRow drRow = dtTable.NewRow();
                    drRow["FileName"] = file.GetName();
                    drRow["ContentLength"] = file.ContentLength.ToString();
                    dtTable.Rows.Add(drRow);
                    dtTable.AcceptChanges();

                    file.SaveAs(strMDBPath);
                    div_importStep1A.Style["display"] = "none";
                    div_importStep1B.Style["display"] = "none";
                    div_importStep2A.Style["display"] = "block";
                    txtHFileName.Value += file.GetName() + ";";
                }
                catch (Exception err) { Response.Write(err.ToString()); }
            }
        }

        repeaterUpload.DataSource = dtTable.DefaultView;
        repeaterUpload.DataBind();
        return;
    }
    #endregion

    #region protected void btnImport_onclick(object sender, EventArgs e)
    protected void btnImport_onclick(object sender, EventArgs e)
    {
        string strFileNames = txtHFileName.Value;

        while (strFileNames.IndexOf(";") != -1)
        {
            strFileName = strFileNames.Substring(0, strFileNames.IndexOf(";"));
            strFileNames = strFileNames.Substring(strFileNames.IndexOf(";") + 1);

            if (ImportFromMDB2SqlServer(Server.MapPath(".") + "/MDB/" + Request.Cookies["UserPracticeCode"].Value + strFileName))
                strResult += "Data importing from " + strFileName + " is done successfully...";
        }
        div_importStep2A.Style["display"] = "none";
        div_ImportStep2B.Style["display"] = "none";
        div_ImportStep2C.Style["display"] = "block";

        lblPatients_value.Text = intPatients.ToString() + " of " + Org_intPatients.ToString();
        lblPWD_value.Text = intPatientWeightData.ToString() + " of " + Org_intPatientWeightData.ToString();
        lblVisits_value.Text = intVisits.ToString() + " of " + Org_intVisits.ToString();
        lblOperations_value.Text = intOperations.ToString() + " of " + Org_intOperations.ToString();
        lblDoctors_value.Text = intDoctors.ToString() + " of " + Org_intDoctors.ToString();
        lblRefDoctors_value.Text = intReferringDoctors.ToString() + " of " + Org_intReferringDoctors.ToString();
        lblSystemCodes_value.Text = intSystemCodes.ToString() + " of " + Org_intSystemCodes.ToString();
        lblComplications_value.Text = intComplications.ToString() + " of " + Org_intComplications.ToString();
        lblCodes_value.Text = intCodes.ToString() + " of " + Org_intCodes.ToString();
        lblHospitals_value.Text = intHospitals.ToString() + " of " + Org_intHospitals.ToString();

        imgPatients.Src = (intPatients > 0) ? (intPatients == Org_intPatients ? "~/img/tick.gif" : "~/img/ico_error.gif") : "";
        imgPWD.Src = (intPatientWeightData > 0) ? (intPatientWeightData == Org_intPatientWeightData ? "~/img/tick.gif" : "~/img/ico_error.gif") : "";
        imgVisit.Src = (intVisits > 0) ? (intVisits == Org_intVisits ? "~/img/tick.gif" : "~/img/ico_error.gif") : "";
        imgOperations.Src = (intOperations > 0) ? (intOperations == Org_intOperations ? "~/img/tick.gif" : "~/img/ico_error.gif") : "";
        imgComplications.Src = (intComplications > 0) ? (intComplications == Org_intComplications ? "~/img/tick.gif" : "~/img/ico_error.gif") : "";
        imgDoctors.Src = (intDoctors > 0) ? (intDoctors == Org_intDoctors ? "~/img/tick.gif" : "~/img/ico_error.gif") : "";
        imgRefDoctors.Src = (intReferringDoctors > 0) ? (intReferringDoctors == Org_intReferringDoctors ? "~/img/tick.gif" : "~/img/ico_error.gif") : "";
        imgHospitals.Src = (intHospitals > 0) ? (intHospitals == Org_intHospitals ? "~/img/tick.gif" : "~/img/ico_error.gif") : "";
        imgCodes.Src = (intCodes > 0) ? (intCodes == Org_intCodes ? "~/img/tick.gif" : "~/img/ico_error.gif") : "";
        imgSystemCodes.Src = (intSystemCodes > 0) ? (intSystemCodes == Org_intSystemCodes ? "~/img/tick.gif" : "~/img/ico_error.gif") : "";
    }
    #endregion

    #region private void ImportFromMDB2SqlServer(string strMDBPath)
    /**/
    private bool ImportFromMDB2SqlServer(string strMDBPath)
    {
        String strSql = "", strSqlFields = String.Empty, strSqlValues = String.Empty;
        bool ImportFlag = true;

        OleDbConnection cnnSource = new OleDbConnection("PROVIDER=Microsoft.Jet.OLEDB.4.0;Data Source=" + strMDBPath);
        SqlConnection cnnDest = new System.Data.SqlClient.SqlConnection(GlobalClass.strSqlCnnString);
        OleDbCommand cmdSource = new OleDbCommand();
        SqlCommand cmdDest = new SqlCommand();

        cmdSource.Connection = cnnSource;
        cmdDest.Connection = cnnDest;
        cmdSource.CommandType = CommandType.Text;
        cmdDest.CommandType = CommandType.Text;

        cmdDest.CommandTimeout = 120;
        cmdSource.CommandTimeout = 120;
        try
        {
            cnnSource.Open();
            cnnDest.Open();
            CheckLocalImperialStatus(cnnSource);
            // First Delete all data in Staging Tables for current Organization and user
            DeleteDataInStagingTables(cnnDest);
            for (int Xh = 0; Xh < (strTablesName.Length / intRank); Xh++)
                if (IsTableIsValid(cnnSource, strTablesName[Xh, 0]))
                {
                    Boolean FirstRecordFlag = false;
                    
                    cmdSource.CommandText = String.Format("Select * from {0}", strTablesName[Xh, 0]);
                    OleDbDataReader drSource = cmdSource.ExecuteReader(CommandBehavior.Default);
                    cmdDest.CommandText = String.Format("SELECT TOP 1 * FROM {0}", strTablesName[Xh, 1]);
                    SqlDataReader drDest = cmdDest.ExecuteReader(CommandBehavior.Default);
                    DataTable dtDest = drDest.GetSchemaTable();
                    drDest.Close();

                    while (drSource.Read())
                    {
                        if (!IsEmptyRecord(strTablesName[Xh, 0], drSource))
                        {
                            if (!FirstRecordFlag)
                            {
                                FirstRecordFlag = true;
                                MakeFieldsList(drSource, dtDest, Xh);
                            }

                            strSql = "insert into " + strTablesName[Xh, 1] + " ( ";
                            strSqlFields = strSqlValues = String.Empty;
                            AddOrganizationCode(strTablesName[Xh, 1], true, ref strSqlFields);
                            AddUserPracticeCode(strTablesName[Xh, 1], true, ref strSqlFields);

                            AddOrganizationCode(strTablesName[Xh, 1], false, ref strSqlValues);
                            AddUserPracticeCode(strTablesName[Xh, 1], false, ref strSqlValues);

                            foreach (Int32 Idx in intListExistFields)
                            {
                                String strField = drSource.GetName(Idx);
                                strSqlFields += "[" + drSource.GetName(Idx) + "] , ";

                                if (!CheckExceptions(strTablesName[Xh, 0], drSource, Idx, ref strSqlValues))
                                    switch (drSource.GetFieldType(Idx).ToString().ToUpper())
                                    {
                                        case "SYSTEM.DATETIME":
                                            if (drSource.IsDBNull(Idx))
                                                strSqlValues += "null, ";
                                            else
                                            {
                                                Page.Culture = "en-US";
                                                string strDate = drSource.GetValue(Idx).ToString();
                                                strDate = strDate.Substring(0, strDate.IndexOf(" "));
                                                try
                                                {
                                                    if ((Convert.ToDateTime(strDate).Year < 1900) || (Convert.ToDateTime(strDate).Year > 9999))
                                                        strDate = "";
                                                    else strDate = Convert.ToDateTime(strDate).ToShortDateString();
                                                }
                                                catch { strDate = ""; }
                                                strSqlValues += "'" + strDate + "' , ";
                                            }
                                            break;
                                        case "SYSTEM.STRING":
                                            if (drSource.IsDBNull(Idx))
                                                strSqlValues += "'', ";
                                            else
                                                strSqlValues += "'" + drSource.GetValue(Idx).ToString().Trim().Replace("'", "`") + "', ";
                                            break;

                                        case "SYSTEM.BOOLEAN":
                                            strSqlValues += (drSource.GetValue(Idx).ToString().Equals("TRUE") ? "1" : "0") + ", ";
                                            break;

                                        default:
                                            if (!ImperialConvertedFields(strTablesName[Xh, 0], drSource, Idx, ref strSqlValues))
                                                if (drSource.IsDBNull(Idx))
                                                    strSqlValues += "0, ";
                                                else
                                                    strSqlValues += drSource.GetValue(Idx).ToString() + ", ";
                                            break;
                                    }
                            }

                            strSqlFields = strSqlFields.Substring(0, strSqlFields.Length - 2); // Remove the last ', ' from the strSqlFields
                            strSqlValues = strSqlValues.Substring(0, strSqlValues.Length - 2); // Remove the last ', ' from the strSqlValues
                            strSql += strSqlFields + ") values (" + strSqlValues + ")";

                            cmdDest.CommandText = strSql;
                            strTablesName[Xh, 2] = (Convert.ToInt32(strTablesName[Xh, 2]) + 1).ToString();
                            try { cmdDest.ExecuteNonQuery(); strTablesName[Xh, 3] = (Convert.ToInt32(strTablesName[Xh, 3]) + 1).ToString(); }
                            catch {}
                            //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                            //sb.Append(err.Message + System.Environment.NewLine + strSqlFields + System.Environment.NewLine + strSqlValues);
                            //txtTest.Value += sb.ToString() + System.Environment.NewLine;
                            //gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Import/Export Popup Form", strTablesName[Xh, 0], sb);
                        } // End of IsEmptyRecord
                    }   // End of while (drSource.Read())
                    GetImportedRecordNo(Xh);
                    drSource.Close();
                    drSource.Dispose();
                } // for
            ImportFlag = true;
        }
        finally
        {
            MovingFromStagingTablesIntoLiveTables(cnnDest);
            if (cnnSource.State != ConnectionState.Closed)
                cnnSource.Close();
            if (cnnDest.State != ConnectionState.Closed)
                cnnDest.Close();
            cnnDest.Dispose();
            cnnSource.Dispose();
        }

        return (ImportFlag);
    }

    /// <summary>
    /// This is to delete all records in staging tables for current organization and user.
    /// </summary>
    /// <param name="cnnDest">The Connection of Production Database</param>
    private void DeleteDataInStagingTables(SqlConnection cnnDest){
        SqlCommand cmdCommand = cnnDest.CreateCommand();

        cmdCommand.CommandType = CommandType.StoredProcedure;
        gClass.MakeStoreProcedureName(ref cmdCommand, "sp_StagingTablesDelete", false);

        cmdCommand.Parameters.Add("@intOrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdCommand.Parameters.Add("@intUserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
        try
        {
            cmdCommand.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            txtTest.Value += System.Environment.NewLine + ex.Message;
        }
        finally
        {
            cmdCommand.Dispose();
        }
    }

    /// <summary>
    /// After importing data from MDB into Staging Tables, the data is moved from 
    /// Staging tables into Live.
    /// </summary>
    /// <param name="cnnDest">The Connection of Production Database</param>
    private void MovingFromStagingTablesIntoLiveTables(SqlConnection cnnDest)
    {
        SqlCommand cmdCommand = cnnDest.CreateCommand();
        
        cmdCommand.CommandType = CommandType.StoredProcedure;
        gClass.MakeStoreProcedureName(ref cmdCommand, "sp_MovingDataFromStagingTablesIntoLive", false);
        cmdCommand.Parameters.Add("@intOrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdCommand.Parameters.Add("@intUserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
        
        try
        {
            DataSet dsCounts = gClass.FetchData(cmdCommand, "tblCounts");

            if (dsCounts.Tables.Count > 0)
            {
                DataView dvCounts = dsCounts.Tables[0].DefaultView;

                Int32.TryParse(dvCounts[0]["CodesCount"].ToString() , out intCodes);
                Int32.TryParse(dvCounts[0]["SystemCodesCount"].ToString(), out intSystemCodes);
                Int32.TryParse(dvCounts[0]["ReferringDoctorsCount"].ToString(), out intReferringDoctors);
                Int32.TryParse(dvCounts[0]["HospitalsCount"].ToString(), out intHospitals);
                Int32.TryParse(dvCounts[0]["DoctorsCount"].ToString(), out intDoctors);
                Int32.TryParse(dvCounts[0]["PatientsCount"].ToString(), out intPatients);
                Int32.TryParse(dvCounts[0]["PatientWeightCount"].ToString(), out intPatientWeightData);
                Int32.TryParse(dvCounts[0]["PatientConsultCount"].ToString(), out intVisits);
                Int32.TryParse(dvCounts[0]["OpEventsCount"].ToString(), out intOperations);
                Int32.TryParse(dvCounts[0]["ComplicationsCount"].ToString(), out intComplications);
            }
        }
        catch (Exception ex)
        {
            txtTest.Value += ex.Message;
        }
        finally
        {
            cmdCommand.Dispose();
        }
    }
    #endregion

    #region private void CheckLocalImperialStatus(OleDbConnection cnnSource)
    private void CheckLocalImperialStatus(OleDbConnection cnnSource)
    {
        OleDbCommand cmdSelect = new OleDbCommand();

        cmdSelect.Connection = cnnSource;
        cmdSelect.CommandType = CommandType.Text;
        cmdSelect.CommandText = "Select ID from MSysObjects where (type = 1) and ([Name] = 'systemdetails')";

        if (IsTableIsValid(cnnSource, "systemdetails"))
        {
            cmdSelect.CommandText = "Select Imperial from systemdetails";
            try { ImperialFlag = (bool)cmdSelect.ExecuteScalar(); }
            catch (Exception err)
            {
                ImperialFlag = false;
                gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Import/Export Page (Second stage)", "CheckLocalImperialStatus function", err.ToString());
            }
        }
        cmdSelect.Dispose();
    }
    #endregion

    #region private bool IsTableIsValid(OleDbConnection cnnSource, string strTableName)
    private bool IsTableIsValid(OleDbConnection cnnSource, string strTableName)
    {
        bool flag = false;
        DataTable dtTable;
        DataView dvView;

        try
        {
            string[] strRestrictions = new string[] { null, null, null, "TABLE" };
            dtTable = cnnSource.GetSchema("Tables", strRestrictions);
            dvView = dtTable.DefaultView;

            dvView.RowFilter = "TABLE_Name = '" + strTableName + "'";
            dvView.RowStateFilter = DataViewRowState.OriginalRows;

            flag = dvView.Count > 0;
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Import/Export Page (Second stage)", "IsTableIsValid function " + strTableName, err.ToString());
            flag = false;
        }
        return (flag);
    }
    #endregion

    #region SetIndentityInsert
    private void SetIndentityInsert(System.Data.SqlClient.SqlCommand cmdDest, string strTableName, string strIndentity)
    {
        switch (strTableName)
        {
            case M_LAPBASE_DOCTORS :
            //case "tblHospitals":
            case M_LAPBASE_PATIENTS:
            case M_LAPBASE_OPEVENTS:
            case M_LAPBASE_COMPLICATION:
            case M_LAPBASE_PATIENTCONSULT:
                cmdDest.CommandText = "Set Identity_Insert " + strTableName + strIndentity;
                cmdDest.ExecuteNonQuery();
                break;
        }
        return;
    }

    private void SetIndentityInsert(OleDbCommand cmdDest, string strTableName, string strIndentity)
    {
        switch (strTableName)
        {
            case M_LAPBASE_DOCTORS:
            //case "tblHospitals":
            case M_LAPBASE_PATIENTS:
            case M_LAPBASE_OPEVENTS:
            case M_LAPBASE_COMPLICATION:
            case M_LAPBASE_PATIENTCONSULT:
                cmdDest.CommandText = "Set Identity_Insert " + strTableName + strIndentity;
                cmdDest.ExecuteNonQuery();
                break;
        }
        return;
    }
    #endregion

    #region private void AddOrganizationCode(string strTableName, bool TitleFlag, ref string strSql)
    private void AddOrganizationCode(string strTableName, bool TitleFlag, ref string strSql)
    {
        switch (strTableName.ToLower())
        {
            case M_LAPBASE_DOCTORS:
            case M_LAPBASE_IDEALWEIGHTS:
            case M_LAPBASE_REFERRINGDOCTORS : 
            case M_LAPBASE_PATIENTWEIGHTDATA :
            case M_LAPBASE_COMPLICATION :
            case M_LAPBASE_OPEVENTS : 
            case M_LAPBASE_PATIENTCONSULT :
            case M_LAPBASE_PATIENTS :
            case M_LAPBASE_HOSPITALS :
                strSql += (TitleFlag ? "[OrganizationCode]" : gClass.OrganizationCode) + " , ";
                break;
        }
        return;
    }
    #endregion

    #region private void AddUserPracticeCode(string strTableName, bool TitleFlag)
    private void AddUserPracticeCode(string strTableName, bool TitleFlag, ref string strSql)
    {
        switch (strTableName.ToLower())
        {
            case M_LAPBASE_CODES :
            case M_LAPBASE_DOCTORS :
            case M_LAPBASE_IDEALWEIGHTS :
            case M_LAPBASE_SYSTEMCODES :
            case M_LAPBASE_SYSTEMNORMALS :
            case M_LAPBASE_HOSPITALS :
            case M_LAPBASE_REFERRINGDOCTORS :
            case M_LAPBASE_PATIENTS :
            case M_LAPBASE_PATIENTWEIGHTDATA :
            case M_LAPBASE_OPEVENTS :
            case M_LAPBASE_PATIENTCONSULT :
            case M_LAPBASE_COMPLICATION :
                strSql += (TitleFlag ? "[UserPracticeCode]" : Request.Cookies["UserPracticeCode"].Value) + " , ";
                break;
        }
        return;
    }
    #endregion

    #region private bool IsEmptyRecord()
    private bool IsEmptyRecord(string strTableName, OleDbDataReader drSource)
    {
        bool flag = false;

        switch (strTableName.ToUpper())
        {
            case "PATIENTS":
                flag = ((drSource.GetValue(drSource.GetOrdinal("Surname")).ToString().Trim().Length == 0) && (drSource.GetValue(drSource.GetOrdinal("Firstname")).ToString().Trim().Length == 0));
                break;

            case "PATIENTCONSULT":
                flag = drSource.IsDBNull(drSource.GetOrdinal("DateSeen")) || (drSource.GetValue(drSource.GetOrdinal("DateSeen")).ToString().Trim().Length == 0);
                break;

            case "SYSTEMDETAILS":
                flag = drSource.IsDBNull(drSource.GetOrdinal("SystemName")) || (drSource.GetValue(drSource.GetOrdinal("SystemName")).ToString().Trim().Length == 0);
                break;

        }
        return flag;
    }
    #endregion

    #region private void RollBackAllData(OleDbCommand cmdDest)
    private void RollBackAllData(OleDbCommand cmdDest)
    {
        String strTableName = String.Empty;
        try
        {
            for (int Xh = 0; Xh < (strTablesName.Length / intRank); Xh++)
            {
                strTableName = strTablesName[Xh, 1];
                DeleteRecordInDest(cmdDest, strTablesName[Xh, 1]);
            }
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Import/Export Page (Second stage)", "RollBackAllData function , Table Name : " + strTableName, err.ToString());
        }
    }
    #endregion

    #region DeleteRecordInDest
    private void DeleteRecordInDest(System.Data.SqlClient.SqlCommand cmdDest, string strTableName)
    {
        try
        {
            cmdDest.CommandText = "Delete from " + strTableName + " where OrganizationCode = " + gClass.OrganizationCode;
            cmdDest.ExecuteNonQuery();
        }
        catch
        {
            cmdDest.CommandText = "Delete from " + strTableName + " where UserPracticeCode = " + Request.Cookies["UserPracticeCode"].Value;
            cmdDest.ExecuteNonQuery();
        }
    }

    private void DeleteRecordInDest(OleDbCommand cmdDest, string strTableName)
    {
        try
        {
            cmdDest.CommandText = "Delete from " + strTableName + " where OrganizationCode = " + gClass.OrganizationCode;
            cmdDest.ExecuteNonQuery();
        }
        catch
        {
            cmdDest.CommandText = "Delete from " + strTableName + " where UserPracticeCode = " + Request.Cookies["UserPracticeCode"].Value;
            cmdDest.ExecuteNonQuery();
        }
    }
    #endregion

    #region private void MakeFieldsList(OleDbDataReader drSource, int idxTable)
    private void MakeFieldsList(OleDbDataReader drSource, DataTable dtDest, int idxTable)
    {
        intListExistFields = new System.Collections.Generic.List<Int32>();

        for (int Idx = 0; Idx < drSource.FieldCount; Idx++)
        {
            Boolean ExistFlag = false;
            String strFieldName = drSource.GetName(Idx);
            foreach (DataRow field in dtDest.Rows)
                if (field[dtDest.Columns[0]].ToString().ToLower().Equals(strFieldName.ToLower()))
                {
                    ExistFlag = true;
                    break;
                }
            
            if (ExistFlag)
                intListExistFields.Add(Idx);
            else
                strListNonExistFields.Add(new String[,] { { strTablesName[idxTable, 1], strFieldName } });
        }
    }
	#endregion

    #region private bool CheckExceptions(str strTableName, OleDbDataReader drSource, int Idx, ref string strSql)
    private bool CheckExceptions(string strTableName, OleDbDataReader drSource, int Idx, ref string strSql)
    {
        bool flag = false;

        switch (strTableName.ToUpper())
        {
            case "PATIENTS":
                switch (drSource.GetName(Idx).ToUpper())
                {
                    case "TITLE":
                        flag = true;
                        switch (drSource.GetValue(Idx).ToString().ToUpper())
                        {
                            case "MR":
                            case "MR.":
                                strSql += "1, ";
                                break;
                            case "MRS":
                            case "MRS.":
                                strSql += "2, ";
                                break;
                            case "MS":
                            case "MS.":
                                strSql += "5, ";
                                break;
                            case "MISS":
                            case "MISS.":
                                strSql += "3, ";
                                break;
                            case "DR":
                            case "DR.":
                                strSql += "4, ";
                                break;
                            case "MSTR":
                            case "MSTR.":
                                strSql += "6, ";
                                break;
                            default:
                                strSql += "0, ";
                                break;
                        }
                        break;
                }
                break;

            case "PATIENTWEIGHTDATA" : // PatientWeightData
            case "OPEVENTS" : // Operation table
                switch (drSource.GetName(Idx).ToUpper())
                {
                    case "SURGERYTYPE":
                        flag = true;
                        switch (drSource.GetValue(Idx).ToString().ToUpper())
                        {
                            case "1" : //LAGB - Lapband
                            case "11": //LAGB - SAGB
                            case "12": //LAGB - other
                            case "13": //LAGB - AMI Band
                            case "14": //LAGB - Soft gastric band
                            case "15": //LAGB - Vangard Band
                                strSql += "'BAA1061' , "; // BAA1061	Gastric banding, adjustable
                                break;
                            case "2": // Roux en Y gastric bypass
                                strSql += "'BAA1063' , "; // BAA1063	Gastric bypass (Roux-en-Y)
                                break;
                            case "6" : // Bilio-pancreatic diversion
                                strSql += "'BAA1058' , "; // BAA1058	Biliopancreatic diversion (BPD)
                                break;
                            case "8": // Gastric Balloon
                                strSql += "'BAA1060' , "; // BAA1060	Gastric balloon
                                break;
                            case "7": // Other
                            case "9": //  Medical 1
                            case "91": // Medical 2
                            case "92": // Medical 3
                                strSql += "'BAA1071' , "; // BAA1071	Other
                                break;
                            case "61": // Sleeve gastrectomy
                                strSql += "'BAA1067' , ";
                                break;
                            default :
                                strSql += "'' , ";
                                break;
                        }

                        break;
                }
                break;
        }
        return flag;
    }
    #endregion

    #region private bool ImperialConvertedFields(str strTableName, OleDbDataReader drSource, int Idx, ref string strSql)
    private bool ImperialConvertedFields(string strTableName, OleDbDataReader drSource, int Idx, ref string strSql)
    {
        bool flag = false;

        switch (strTableName.ToUpper())
        {
            case "PATIENTWEIGHTDATA":
                switch (drSource.GetName(Idx).ToUpper())
                {
                    case "HEIGHT":
                        //case "BMIHEIGHT":
                        double dblHeight = drSource.IsDBNull(Idx) ? 0 : Convert.ToDouble(drSource.GetValue(Idx).ToString());

                        //dblHeight = chkLocalImperial.Checked ? (dblHeight * 0.0254) : (dblHeight / 100);
                        dblHeight = ImperialFlag ? (dblHeight * 0.0254) : (dblHeight / 100);
                        strSql += ((decimal)dblHeight).ToString() + " , ";
                        flag = true;
                        break;

                    case "STARTWEIGHT":
                    case "STARTBMIWEIGHT":
                    case "CURRENTWEIGHT":
                    case "IDEALWEIGHT":
                    case "OPWEIGHT":
                    case "TARGETWEIGHT":
                        double dcWeight = drSource.IsDBNull(Idx) ? 0 : Convert.ToDouble(drSource.GetValue(Idx).ToString());

                        //dcWeight = chkLocalImperial.Checked ? (dcWeight * 0.45359237) : dcWeight;
                        dcWeight = ImperialFlag ? (dcWeight * 0.45359237) : dcWeight;
                        strSql += ((decimal)dcWeight).ToString() + " , ";
                        flag = true;
                        break;
                }
                break;

            case "PATIENTCONSULT":
                switch (drSource.GetName(Idx).ToUpper())
                {
                    case "WEIGHT":
                        double dcWeight = drSource.IsDBNull(Idx) ? 0 : Convert.ToDouble(drSource.GetValue(Idx).ToString());

                        //dcWeight = chkLocalImperial.Checked ? (dcWeight * 0.45359237) : dcWeight;
                        dcWeight = ImperialFlag ? (dcWeight * 0.45359237) : dcWeight;
                        strSql += ((decimal)dcWeight).ToString() + " , ";
                        flag = true;
                        break;
                }
                break;
        }
        return (flag);
    }
    #endregion

    #region private void GetImportedRecordNo(string strTableName){
    private void GetImportedRecordNo(int idxTable)
    {
        switch (strTablesName[idxTable,1].ToLower())
        {
            case M_LAPBASE_CODES :
                Org_intCodes = Convert.ToInt32(strTablesName[idxTable, 2]);
                break;
            case M_LAPBASE_DOCTORS :
                Org_intDoctors = Convert.ToInt32(strTablesName[idxTable, 2]);
                break;
            case M_LAPBASE_IDEALWEIGHTS :
                break;
            case M_LAPBASE_SYSTEMCODES :
                Org_intSystemCodes = Convert.ToInt32(strTablesName[idxTable, 2]);
                break;
            case M_LAPBASE_SYSTEMNORMALS :
                Org_intSystemNormals = Convert.ToInt32(strTablesName[idxTable, 2]);
                break;
            case M_LAPBASE_HOSPITALS :
                Org_intHospitals = Convert.ToInt32(strTablesName[idxTable, 2]);
                break;
            case M_LAPBASE_REFERRINGDOCTORS :
                Org_intReferringDoctors = Convert.ToInt32(strTablesName[idxTable, 2]);
                break;
            case M_LAPBASE_PATIENTS :
                Org_intPatients = Convert.ToInt32(strTablesName[idxTable, 2]);
                break;
            case M_LAPBASE_PATIENTWEIGHTDATA :
                Org_intPatientWeightData = Convert.ToInt32(strTablesName[idxTable, 2]);
                break;
            case M_LAPBASE_OPEVENTS :
                Org_intOperations = Convert.ToInt32(strTablesName[idxTable, 2]);
                break;
            case M_LAPBASE_PATIENTCONSULT :
                Org_intVisits = Convert.ToInt32(strTablesName[idxTable, 2]);
                break;
            case M_LAPBASE_COMPLICATION :
                Org_intComplications = Convert.ToInt32(strTablesName[idxTable, 2]);
                break;
        }
    }
    #endregion

    #region private void UpdateBMIInPatientWeightDataTable(OleDbCommand cmdDesc)
    /// <summary>
    /// This function is to update BMI value in tblPatientWeightData after data is imported from local MDB file
    /// </summary>
    private void UpdateBMIInPatientWeightDataTable()
    {
        using (SqlConnection connection = new SqlConnection(GlobalClass.strSqlCnnString))
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                SqlCommand command = connection.CreateCommand();
                gClass.MakeStoreProcedureName(ref command, "sp_PatientWeightData_UpdateBMI", false);
                command.ExecuteNonQuery();
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
            }
        }
    }
    #endregion 

    #region private void SendEmail
    private void SendEmail()
    {
        SmtpClient smtpClient = new SmtpClient();
        MailMessage message = new MailMessage();
        String strScript = String.Empty;

        try
        {
            smtpClient.Host = System.Configuration.ConfigurationManager.AppSettings["MailServerHost"];
            smtpClient.Port = 25;

            //message.From = new MailAddress("support@lapbase.com", "Lapbase web application support team");
            message.From = new MailAddress("farahani.av@gmail.com", "Lapbase web application support team");
            message.Subject = "User Name : " + Request.Cookies["Logon_UserName"].Value + ", Data Importing from Local MDB File";
            message.CC.Add("farahani.av@gmail.com");
            
            message.IsBodyHtml = true;

            message.Body = "<Table style='width:500px'>";
            message.Body += "   <tr>";
            message.Body += "       <td style='width:100px'>User name</td>";
            message.Body += "       <td>" + Request.Cookies["Logon_UserName"].Value + "</td>";
            message.Body += "   </tr>";
            message.Body += "   <tr>";
            message.Body += "       <td>File name</td>";
            message.Body += "       <td>" + strFileName + "</td>";
            message.Body += "   </tr>";
            message.Body += "   <tr>";
            message.Body += "       <td colspan='2'><br/></td>";
            message.Body += "   </tr>";
            if (strListNonExistFields.Count > 0)
            {
                message.Body += "   <tr>";
                message.Body += "       <td colspan='2'>Here is list of fields that are not existed in Lapbase web application database...</td>";
                message.Body += "   </tr>";
                message.Body += "   <tr>";
                message.Body += "       <td colspan='2'><br/></td>";
                message.Body += "   </tr>";
                message.Body += "   <tr style='font-weight:bold;font-size:medium;font-style:italic'>";
                message.Body += "       <td>Table</td>";
                message.Body += "       <td>Field</td>";
                message.Body += "   </tr>";
                foreach (String[,] strField in strListNonExistFields)
                {
                    message.Body += "   <tr style='font-size:small'>";
                    message.Body += "       <td>" + strField[0, 0] + "</td>";
                    message.Body += "       <td>" + strField[0, 1] + "</td>";
                    message.Body += "   </tr>";
                }

                strScript = "document.getElementById('divErrorMessage').style.display = 'block';";
                strScript += "SetInnerText(document.getElementById('divErrorMessage'), 'Data importing has been done with some defects...');";
            }
            else
            {
                message.Body += "   <tr>";
                message.Body += "       <td colspan='2'>The data importing from local MDB file is done successfully...</td>";
                message.Body += "   </tr>";

                strScript = "document.getElementById('divErrorMessage').style.display = 'none';";
            }
            message.Body += "</Table>";

            // Send SMTP mail
            smtpClient.Send(message);

            ScriptManager.RegisterStartupScript(UpdatePanel3, btnUploadMDB.GetType(), Guid.NewGuid().ToString(), strScript, true);
        }
        catch (Exception ex)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Import/Export Page", "Send Email - "  + strListNonExistFields.Count.ToString(), ex.ToString());
        }
    }
    /**/
    #endregion
}
