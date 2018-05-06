using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Globalization;
using System.Xml;

public class GlobalClass
{
    private DataSet _dsGlobal;
    private DataView dvSchemaFields;

    //------------------------------
    public static string strLapbaseCnnString, strSqlCnnString;
    public Int32 ValidDays = 0;

    private int _User_SNo, _User_GourpCode;
    private string tempOrganizationCode;
    private string _AppSchemaID, _ErrAlias, _ErrDesc, _UserID, _UserFullName, _DBPath, /*_SessionID, */_OrganizationCode;
    private string _Remote_IPAddress, _logon_UserName, _Remote_UserName, _CultureInfo, _PageDirection, _LanguageCode, _UserPW, _VisitWeeksFlag;
    private byte Gender;
    private long longPatientID;

    //----- System Details variables
    private int UseRace, ComordityVisitMonths, ReferenceBMI;
    private string Field1Name, Field2Name, Field3Name, Field4Name, Field5Name, MField1Name, MField2Name, WOScale, SubmitData;
    private bool UseIWonBMI, PatCOM, FUShowVisits, FUCom, FUinv, Imperial, PatInv;
    private double TargetBMI;

    //----- Investigations normals
    private string AlbuminL, AlbuminH, AlbuminU,
                    AlkPhosL, AlkPhosH, AlkPhosU,
                    ALTL, ALTH, ALTU,
                    ASTL, ASTH, ASTU,
                    B12L, B12H, B12U,
                    BicarbonateL, BicarbonateH, BicarbonateU,
                    BilirubinL, BilirubinH, BilirubinU,
                    CalciumL, CalciumH, CalciumU,
                    ChlorideL, ChlorideH, ChlorideU,
                    CPKL, CPKH, CPKU,
                    CreatinineL, CreatinineH, CreatinineU,
                    DiastolicBPL, DiastolicBPH, DiastolicBPU,
                    FBloodGlucoseL, FBloodGlucoseH, FBloodGlucoseU,
                    FerritinL, FerritinH, FerritinU,
                    FolateL, FolateH, FolateU,
                    FSerumInsulinL, FSerumInsulinH, FSerumInsulinU,
                    GGTL, GGTH, GGTU,
                    HBA1CL, HBA1CH, HBA1CU,
                    HDLCholesterolL, HDLCholesterolH, HDLCholesterolU,
                    HemoglobinL, HemoglobinH, HemoglobinU,
                    HomocysteineL, HomocysteineH, HomocysteineU,
                    IBCL, IBCH, IBCU,
                    IronL, IronH, IronU,
                    PhosphateL, PhosphateH, PhosphateU,
                    PlateletsL, PlateletsH, PlateletsU,
                    PotassiumL, PotassiumH, PotassiumU,
                    SodiumL, SodiumH, SodiumU,
                    SystolicBPL, SystolicBPH, SystolicBPU,
                    T4L, T4H, T4U,
                    T3L, T3H, T3U,
                    TotalCholesterolL, TotalCholesterolH, TotalCholesterolU,
                    TproteinL, TproteinH, TproteinU,
                    TransferrinL, TransferrinH, TransferrinU,
                    TriglyceridesL, TriglyceridesH, TriglyceridesU,
                    TSHL, TSHH, TSHU,
                    UreaL, UreaH, UreaU,
                    VitDL, VitDH, VitDU,
                    WCCL, WCCH, WCCU;

    //===========================================================================================================
    #region public GlobalClass()
    /*
     * The Initial function of GlobalClass and set ConnectionString for both OLE CONNECTION and SQL SERVER CONNECTION
     * we use OLE CONNECTION for OLE COMMAND (because of old version of application)
     */
    public GlobalClass()
    {
        GlobalClass.strLapbaseCnnString = System.Configuration.ConfigurationManager.AppSettings["DBConnectionString"];
        GlobalClass.strSqlCnnString = System.Configuration.ConfigurationManager.ConnectionStrings["SqlDBConnectionString"].ConnectionString;
    }
    #endregion

    #region public DataSet FetchLanguages()
    /*To load all laguages from tblLanguages and returns a DATASET to fill Language dropdown list*/
    public DataSet FetchLanguages()
    {
        SqlCommand cmdSelect = new SqlCommand();

        MakeStoreProcedureName(ref cmdSelect, "sp_FetchLanguage", false);
        FillDataSet(cmdSelect, "tblLanguages");
        return (this._dsGlobal);
    }
    #endregion

    #region public void FetchError()
    /*
     * this function is to load appropiate Error/Warning message based of selected language, this function should be extented if we want to support multi-lingual version of application, 
     * so we need to show proper messages in selected language
     */
    public void FetchError()
    {
        string strSql;

        strSql = "SELECT * FROM tblErrors WHERE Alias_Name = '" + _ErrAlias + "'";
        FillDataSet(strSql, "tblError");

        if (_dsGlobal.Tables[0].DefaultView.Count > 0)
        {
            switch (this._LanguageCode.Substring(0, 2))
            {
                case "en":
                    _ErrDesc = _dsGlobal.Tables[0].DefaultView[0]["Error_Desc_English"].ToString();
                    break;
                case "ar":
                    _ErrDesc = _dsGlobal.Tables[0].DefaultView[0]["Error_Desc_Arabic"].ToString();
                    break;
                case "es":
                    _ErrDesc = _dsGlobal.Tables[0].DefaultView[0]["Error_Desc_Spanish"].ToString();
                    break;
            }
        }
        else
        {
            _ErrDesc = "No appropiate error message is found...";
        }
        return;
    }
    #endregion

    #region public byte IsValidUserNamePassword()
    /*
     * this function is to check user credential in login page, this funcion is called in 2 different places,
     * 1) Login page 2) Users Management page (users.lapbase.net) to enter Admin credential
     * if function is called on login page, the LoginFlag is TRUE, otherwise it's FALSE
     * So if the LOGINFLAG is true, we set some data of current user
     */
    public byte IsValidUserNamePassword(string strPassword, ref System.Web.UI.HtmlControls.HtmlTextArea myText, Boolean LoginFlag)
    {
        SqlCommand cmdSelect = new SqlCommand();
        byte flag = 0;

        MakeStoreProcedureName(ref cmdSelect, "sp_Logon_CheckUserCredentials", false);
        cmdSelect.Parameters.Add("@strOrgDomainName", SqlDbType.VarChar, 80).Value = _OrganizationCode;
        cmdSelect.Parameters.Add("@UserID", SqlDbType.VarChar, 25).Value = _UserID;
        cmdSelect.Parameters.Add("@LoginFlag", SqlDbType.Bit).Value = LoginFlag;
        //cmdSelect.Parameters.Add("@UserPW", OleDbType.VarBinary, 50).Value = _UserPW;

        try
        {
            FillDataSet(cmdSelect, "tblUsers");
            if ((_dsGlobal.Tables.Count == 0) || (_dsGlobal.Tables[0].Rows.Count == 0))
                flag = 0;
            else
            {
                DataView dvUser = _dsGlobal.Tables[0].DefaultView;
                for (int idx = 0; (idx < dvUser.Count) && (flag == 0); idx++)
                {
                    string strHexPassword = HexEncoding.ToString(GetSecureBinaryData(_UserPW)),
                            strUserPW = HexEncoding.ToString((byte[])dvUser[idx]["UserPW"]).Substring(0, strHexPassword.Length);

                    myText.Value = strUserPW + "  " + HexEncoding.ToString((byte[])dvUser[idx]["UserPW"]) + "  " + HexEncoding.ToString(GetSecureBinaryData(strPassword));
                    if (strHexPassword.Equals(strUserPW))
                        if (dvUser[idx]["Permission_Flag"].ToString().Equals("1"))
                        {
                            flag = 1;
                            if (LoginFlag) // login to system
                            {
                                _UserFullName = _dsGlobal.Tables[0].Rows[0]["User_Name"].ToString() + "  " + _dsGlobal.Tables[0].Rows[0]["User_SirName"].ToString();
                                _User_SNo = Convert.ToInt32(_dsGlobal.Tables[0].Rows[0]["UserPracticeCode"].ToString());
                                _OrganizationCode = _dsGlobal.Tables[0].Rows[0]["OrganizationCode"].ToString();
                                _CultureInfo = _dsGlobal.Tables[0].Rows[0]["CultureInfo"].ToString();
                                _PageDirection = _dsGlobal.Tables[0].Rows[0]["Direction"].ToString();
                                _LanguageCode = _dsGlobal.Tables[0].Rows[0]["Language_Code"].ToString();

                                ValidDays = Convert.ToInt32(_dsGlobal.Tables[0].Rows[0]["ValidDays"].ToString());

                                Imperial = _dsGlobal.Tables[0].Rows[0]["Imperial"].ToString().Equals(String.Empty) || _dsGlobal.Tables[0].Rows[0]["Imperial"].ToString().Equals("True");
                                VisitWeeksFlag = _dsGlobal.Tables[0].Rows[0]["VisitWeeksFlag"].ToString();
                                SubmitData = _dsGlobal.Tables[0].Rows[0]["SubmitData"].ToString();
                                //if (_dsGlobal.Tables[0].Rows[0]["Imperial"].ToString().Length == 0)
                                //    Imperial = true;
                                //else
                                //    Imperial = _dsGlobal.Tables[0].Rows[0]["Imperial"].ToString() == "True";


                            }
                        }
                        else
                            flag = 2;
                }
            }
        }
        catch (Exception err) { myText.Value = err.ToString(); this.AddErrorLogData("0", "", "Administrator", "Login to system", "IsValidUserNamePassword", err.ToString()); flag = 0; }
        return (flag);
    }
    #endregion

    #region public string ShowSchema(DataSet dsReport, string XsltFilePath)
    /*
     * In most of lists in the application (such as Patient list, visit list, operation list,letters of selected language,
     * we use XML document (made by DataSet) and XSL file to make these list, 
     * this funcion is to create a XML document and merge with XSL file, and generates a STRING of this merging
     * and returns the final STRING.
     * We use this STRING as INNETHTML of DIV(html component).
     * all above lists(patients, visit, operations and etc) are inside of a DIV.
     * 
     * Input Parameter :
     * 1) DataSet including data to be converted to XML Data Document
     * 2) XsltFilePath is the full path and file name of XSLT file
     * Output Parameter : 
     * 1) a STRING containing the result of merging XML data document and XSL file
     */
    public string ShowSchema(DataSet dsReport, string XsltFilePath)
    {
        System.Xml.XmlDataDocument XmlDataDoc;
        System.Xml.Xsl.XslCompiledTransform XslComTranse = new System.Xml.Xsl.XslCompiledTransform();
        System.Text.StringBuilder sbSchema = new System.Text.StringBuilder();
        System.IO.TextWriter twSchema = new System.IO.StringWriter(sbSchema);
        System.Xml.Xsl.XsltSettings xsltSetting = new System.Xml.Xsl.XsltSettings(true, true);

        XmlDataDoc = new System.Xml.XmlDataDocument(dsReport);
        XslComTranse.Load(XsltFilePath, xsltSetting, null);
        XslComTranse.Transform(XmlDataDoc, null, twSchema);

        return (sbSchema.ToString());
    }
    #endregion

    #region public bool IsUserLogoned(string strSessionID, string strUserCode, string strURLDomainName)
    /*
     * at the begining of each application item (form/report) we check whether user has logined to application (the URL of organization) or not, 
     * if user is a valid user, we save Session.ID for him/her.
     * Input Paramerters
     * 1) strSessionID : containing SessionID value for the user
     * 2) strUserCide : if user has logined to the application, the UserCode is fetched from tblUsers (UserPracticeCode)
     * 3) strURLDomainName : it's the Request.Url.Host and equal to current organization url
     * Output value
     * 1) Boolean (true/false), true : if the current user is permitted to access to the requested page
     * 
     * Also we check this situation, after login to system, the current user, changes the Organization URL in Browser to access to other
     * organization's data, so everytime on each page, we should check whether the user is permitted to access to this page of organization or not
     */
    public bool IsUserLogoned(string strSessionID, string strUserCode, string strURLDomainName)
    {
        bool flag = (strSessionID.Length > 0) && (Convert.ToInt64(strUserCode) > 0);
        flag &= IsDomainOK(strUserCode, strURLDomainName.ToLower());
        return (flag);
    }
    #endregion

    #region private Boolean IsDomainOK(string strURLDomainName)
    /*
     * this function checks that the current user is a valid member of the current organization or not.
     * this funcion is called at the begining of each application item (forms/reports) by "IsUserLogoned" funcion
     * Input parameter:
     * 1) strUserCode : the User code from tblUsers
     * 2) strURLDomainName : the organization url in Request.Url.Host
     * Return value :
     * 1) Boolean (true/false), true if user is a member of the organization, otherwise false
     */
    private Boolean IsDomainOK(string strUserCode, string strURLDomainName)
    {
        Boolean flag = true;
        SqlCommand cmdCommand = new SqlCommand();

        MakeStoreProcedureName(ref cmdCommand, "sp_UsersManagement_CheckUserDomain", false);
        cmdCommand.Parameters.Add("@strDomainName", SqlDbType.VarChar, 100).Value = strURLDomainName;
        cmdCommand.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(strUserCode);
        try { flag = (this.FetchData(cmdCommand, "tblUserDomain").Tables[0].Rows.Count > 0); }
        catch (Exception err) { flag = false; this.AddErrorLogData(strUserCode, "", "", "", "Check User Domain", err.ToString()); }
        cmdCommand.Dispose();
        return flag;
    }
    #endregion

    #region public void SetFieldsName(System.Web.UI.Control ParentControl, HttpResponse httpResp)
    /*
     * this is a recursive funtion to load all titles and captions of the page by using selected language.
     * the second parameter is a temporary just to write some messages for debugging.
     * always this function is called after gClass.FetchCaptions(formID, LanguageCode);
     * the fetchCaptions function creates a list of captions and titles of current from and selected language.
     * inside this function 'SetFieldsName' we filter the above list and load the proper title.
     */
    public void SetFieldsName(System.Web.UI.Control ParentControl, HttpResponse httpResp)
    {
        foreach (Control ctrl in ParentControl.Controls)
        {
            if (ctrl.Controls.Count > 0)
            {
                SetFieldsName(ctrl, httpResp);
            }
            else
            {
                switch (ctrl.GetType().ToString())
                {
                    case "System.Web.UI.HtmlControls.HtmlButton":
                    case "System.Web.UI.HtmlControls.HtmlAnchor":
                    case "System.Web.UI.WebControls.TextBox":
                    case "System.Web.UI.WebControls.Label":
                    case "System.Web.UI.WebControls.Button":
                    case "System.Web.UI.WebControls.LinkButton":
                    case "System.Web.UI.WebControls.TableCell":
                        dvSchemaFields.RowFilter = "Field_ID ='" + ctrl.ID.ToString() + "'";
                        dvSchemaFields.RowStateFilter = DataViewRowState.OriginalRows;
                        if (dvSchemaFields.Count > 0)
                        {
                            string strCtrlID = "", strAlign = "";
                            strCtrlID = dvSchemaFields[0]["Field_Caption"].ToString();
                            ((WebControl)ctrl).Style.Add("text-align", strAlign);
                            switch (ctrl.GetType().ToString())
                            {
                                case "System.Web.UI.HtmlControls.HtmlButton":
                                    ((System.Web.UI.HtmlControls.HtmlButton)ctrl).Attributes["value"] = strCtrlID;
                                    break;
                                case "System.Web.UI.HtmlControls.HtmlAnchor":
                                    ((System.Web.UI.HtmlControls.HtmlAnchor)ctrl).InnerText = strCtrlID;
                                    break;
                                case "System.Web.UI.WebControls.Label":
                                    ((System.Web.UI.WebControls.Label)ctrl).Text = strCtrlID;
                                    break;

                                case "System.Web.UI.WebControls.TableCell":
                                    ((System.Web.UI.WebControls.TableCell)ctrl).Text = strCtrlID;
                                    break;

                                case "System.Web.UI.WebControls.Button":
                                    ((System.Web.UI.WebControls.Button)ctrl).Text = strCtrlID;
                                    ((WebControl)ctrl).Style.Add("text-align", "Center");
                                    break;

                                case "System.Web.UI.WebControls.LinkButton":
                                    ((System.Web.UI.WebControls.LinkButton)ctrl).Text = strCtrlID;
                                    ((WebControl)ctrl).Style.Add("text-align", "Center");
                                    break;
                            }
                        }
                        break;

                    case "System.Web.UI.WebControls.DropDownList":
                        break;

                    case "System.Web.UI.WebControls.RadioButtonList":
                        break;
                }
            }
        }
        return;
    }
    #endregion

    #region private void FetchListItems(string ControlID)
    /*
     * this function is to load the data of list (controlID : dropdownlist ID, radiobuttonlist ID and etc) and fill a dataset of result set,
     * after this function, the function SetListItems, is called from the application
     */
    private void FetchListItems(string ControlID)
    {
        string strSql;

        strSql = "SELECT B.* FROM tblAppSchema A with(nolock, nowait) INNER JOIN tblAppSchemaListItems B with(nolock, nowait) ON A.tblAPPSCHEMA_CODE = B.tblAPPSCHEMA_CODE ";
        strSql += "WHERE (A.APPSCHEMA_ID = '" + this._AppSchemaID + "') and (B.Field_ID = '" + ControlID + "') ";
        strSql += "Order By B.List_Value";
        FillDataSet(strSql, "tblListItems");
        return;
    }
    #endregion

    #region public void SetListItems(string strAppSchema, System.Web.UI.Control MyControl, byte ListType)
    /*
     * this function achieves like the SetFieldsName, but just for lists (dropdown list, radiobutton list and etc)
     * for example in PatientList form, SortBy dropdownlist.
     * this function used a dataset which is created by FetchListItems
     */
    public void SetListItems(string strAppSchema, System.Web.UI.Control MyControl, byte ListType)
    {
        this._AppSchemaID = strAppSchema;
        string strTextField = "";

        FetchListItems(MyControl.ID);
        if (this._dsGlobal.Tables[0].Rows.Count > 0)
        {
            switch (this.LanguageCode.Substring(0, 2))
            {
                case "en": // English
                    strTextField = "List_Text_English";
                    break;

                case "ar": // Arabic
                    strTextField = "List_Text_Arabic";
                    break;

                case "es": // 
                    strTextField = "List_Text_Spanish";
                    break;
            }

            switch (ListType)
            {
                case 1:
                    ((System.Web.UI.WebControls.DropDownList)MyControl).DataSource = this._dsGlobal;
                    ((System.Web.UI.WebControls.DropDownList)MyControl).DataMember = "tblListItems";
                    ((System.Web.UI.WebControls.DropDownList)MyControl).DataValueField = "List_Value";
                    ((System.Web.UI.WebControls.DropDownList)MyControl).DataTextField = strTextField;
                    ((System.Web.UI.WebControls.DropDownList)MyControl).DataBind();
                    break;

                case 2:
                    ((System.Web.UI.WebControls.RadioButtonList)MyControl).DataSource = this._dsGlobal;
                    ((System.Web.UI.WebControls.RadioButtonList)MyControl).DataMember = "tblListItems";
                    ((System.Web.UI.WebControls.RadioButtonList)MyControl).DataValueField = "List_Value";
                    ((System.Web.UI.WebControls.RadioButtonList)MyControl).DataTextField = strTextField;
                    ((System.Web.UI.WebControls.RadioButtonList)MyControl).DataBind();
                    ((System.Web.UI.WebControls.RadioButtonList)MyControl).Items[0].Selected = true;
                    break;
            }
        }
        return;
    }
    #endregion

    #region public string AddErrorLogData(string strUserPracticeCode, string strClientID, string strUserName, string strSchemaName, string strProcess, string strErrorMessage)
    /*
     * this function is to add a log record for each error/exception during runtime.
     * we used these data to trace and find out any problem during running the application
     * this function is called by Exception/Catch (try{} catch{call the function})
     */
    public void AddErrorLogData(string strUserPracticeCode, string strClientID, string strUserName, string strSchemaName, string strProcessName, string strErrorMessage)
    {
        SqlCommand cmdInsert = new SqlCommand();

        MakeStoreProcedureName(ref cmdInsert, "sp_ErrorLogFile_InsertData", true);
        cmdInsert.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(strUserPracticeCode);
        cmdInsert.Parameters.Add("@ClientIP", SqlDbType.VarChar, 15).Value = strClientID;
        cmdInsert.Parameters.Add("@UserName", SqlDbType.VarChar, 50).Value = strUserName;
        cmdInsert.Parameters.Add("@SchemaName", SqlDbType.VarChar, 50).Value = strSchemaName;
        cmdInsert.Parameters.Add("@ProcessName", SqlDbType.VarChar, 50).Value = strProcessName;
        cmdInsert.Parameters.Add("@ErrorMessage", SqlDbType.VarChar, 4095).Value = strErrorMessage;

        ExecuteDMLCommand(cmdInsert);
        return;
    }
    #endregion

    #region public void AddErrorLogData(string strUserPracticeCode, string strClientID, string strUserName, string strSchemaName, string strProcessName, System.Text.StringBuilder strErrorMessage)
    /// <summary>
    /// This function is to add an error message into ErrorLog Table.
    /// </summary>
    /// <param name="strUserPracticeCode"></param>
    /// <param name="strClientID"></param>
    /// <param name="strUserName"></param>
    /// <param name="strSchemaName"></param>
    /// <param name="strProcessName"></param>
    /// <param name="strErrorMessage"></param>
    public void AddErrorLogData(string strUserPracticeCode, string strClientID, string strUserName, string strSchemaName, string strProcessName, System.Text.StringBuilder strErrorMessage)
    {
        SqlCommand cmdInsert = new SqlCommand();

        MakeStoreProcedureName(ref cmdInsert, "sp_ErrorLogFile_InsertData", true);
        cmdInsert.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(strUserPracticeCode);
        cmdInsert.Parameters.Add("@ClientIP", SqlDbType.VarChar, 15).Value = strClientID;
        cmdInsert.Parameters.Add("@UserName", SqlDbType.VarChar, 50).Value = strUserName;
        cmdInsert.Parameters.Add("@SchemaName", SqlDbType.VarChar, 50).Value = strSchemaName;
        cmdInsert.Parameters.Add("@ProcessName", SqlDbType.VarChar, 50).Value = strProcessName;
        cmdInsert.Parameters.Add("@ErrorMessage", SqlDbType.VarChar, 4095).Value = strErrorMessage.ToString();

        ExecuteDMLCommand(cmdInsert);
        return;
    }
    #endregion

    #region public DataSet FillCityList(string strOrganizationCode, string strUserPracticeCode)
    /*
     * this function is load and fill a dynamic dropdownlist for all cities in Patient data form / City field,
     * in the patient data form, as soon as user enters a value in the city fields, a dropdown is loaded and user can select the city,
     * the data of that dynamic dropdownlist is loaded by this function.
     * the function is called by Patient data form (Patient Detials).
     * Input Parameter:
     * 1) strOrganizationCode : the code of organization from tblOrganization
     * 2) strUserPracticeCode : the code of current user from tblUsers (this parameter is redundant in store procedure!!!)
     * Return value :
     * 1) DataSet including all cities name from tblPatients for the current organization
     */
    public DataSet FillCityList(string strOrganizationCode, string strUserPracticeCode)
    {
        SqlCommand cmdSelect = new SqlCommand();

        MakeStoreProcedureName(ref cmdSelect, "sp_City_LoadData", true);
        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(strOrganizationCode);
        cmdSelect.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(strUserPracticeCode);
        FillDataSet(cmdSelect, "tblCity");
        return (this._dsGlobal);
    }
    #endregion

    #region public DataSet FillInsuranceList()
    /*this function acheives like FillCityList*/
    public DataSet FillInsuranceList()
    {
        string strSql;

        strSql = "Select DISTINCT Insurance FROM tblPatients with(nowait, nolock) where (Insurance <> '') and (not Insurance is null ) ORDER BY Insurance";
        FillDataSet(strSql, "tblInsurance");
        return (this._dsGlobal);
    }
    #endregion

    #region public DataSet FillReferredDoctorList(string strOrganizationCode, string strUserPracticeCode)
    /*
     * this function is to load a list of referring doctors data of current organization,
     * the strUserPracticeCode is redundant!!!!
     * the result dataset is used in different places, for example in Patient Data form to load a dynamic dropdown list for 3 referring doctor fields.
     * if the SurName (store procedure parameter) is null, it loads all referring doctors data
     * otherwaise, it just loads only doctors that their names contain the value of this parameter
     */
    public DataSet FillReferredDoctorList(string strOrganizationCode, string strUserPracticeCode)
    {
        SqlCommand cmdSelect = new SqlCommand();

        MakeStoreProcedureName(ref cmdSelect, "sp_ReferredDoctor_LoadData", true);
        cmdSelect.Parameters.Add("@SurName", SqlDbType.VarChar, 50).Value = string.Empty;
        cmdSelect.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(strUserPracticeCode);
        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(strOrganizationCode);
        cmdSelect.Parameters.Add("@vblnIsHide", SqlDbType.Bit).Value = false;

        FillDataSet(cmdSelect, "tblReferredDoctor");
        return (this._dsGlobal);
    }
    #endregion

    #region private void FillDataSet(string strSql, string strTableName)
    /*
     * this function is to run a sql command (sql string command, olecommand and sqlcommand) and fills a dataset 
     * the table name of the dataset is strTableName.
     * in the GlobalClass, there are 3 FillDataSet functions that have different input parameters, but the achievement is same.
     */
    private void FillDataSet(string strSql, string strTableName)
    {
        SqlDataAdapter daSql;

        this._dsGlobal = new DataSet("dsSchema");

        daSql = new SqlDataAdapter("", strSqlCnnString);
        daSql.SelectCommand.CommandType = CommandType.Text;
        daSql.SelectCommand.CommandText = strSql;
        daSql.Fill(this._dsGlobal, strTableName);
        daSql.Dispose();
    }
    #endregion

    #region private void FillDataSet(OleDbCommand cmdCommand, string strTableName)
    /*
     * this function is to run a sql command (sql string command, olecommand and sqlcommand) and fills a dataset 
     * the table name of the dataset is strTableName.
     * in the GlobalClass, there are 3 FillDataSet functions that have different input parameters, but the achievement is same.
     */
    private void FillDataSet(OleDbCommand cmdCommand, string strTableName)
    {
        OleDbDataAdapter daDataAdapter;
        using (OleDbConnection cnnLapbase = new OleDbConnection(strLapbaseCnnString))
        {
            this._dsGlobal = new DataSet("dsSchema");
            this._dsGlobal.Tables.Clear();
            daDataAdapter = new OleDbDataAdapter();
            daDataAdapter.SelectCommand = cmdCommand;
            daDataAdapter.SelectCommand.Connection = cnnLapbase;
            daDataAdapter.SelectCommand.CommandTimeout = 0;
            try { daDataAdapter.Fill(this._dsGlobal, strTableName); }
            catch { daDataAdapter.SelectCommand.Connection.ConnectionString = System.Configuration.ConfigurationManager.AppSettings["DBConnectionString"]; daDataAdapter.Fill(this._dsGlobal, strTableName); }
            daDataAdapter.Dispose();
            cnnLapbase.Close();
            cnnLapbase.Dispose();
        }
    }
    #endregion

    #region private void FillDataSet(SqlCommand cmdCommand, string strTableName)
    /*
     * this function is to run a sql command (sql string command, olecommand and sqlcommand) and fills a dataset 
     * the table name of the dataset is strTableName.
     * in the GlobalClass, there are 3 FillDataSet functions that have different input parameters, but the achievement is same.
     */
    private void FillDataSet(SqlCommand cmdCommand, string strTableName)
    {
        SqlDataAdapter daDataAdapter;
        using (SqlConnection cnnLapbase = new SqlConnection(strSqlCnnString))
        {
            this._dsGlobal = new DataSet("dsSchema");
            this._dsGlobal.Tables.Clear();
            daDataAdapter = new SqlDataAdapter();
            daDataAdapter.SelectCommand = cmdCommand;
            daDataAdapter.SelectCommand.Connection = cnnLapbase;
            daDataAdapter.SelectCommand.CommandTimeout = 0;
            try { daDataAdapter.Fill(this._dsGlobal, strTableName); }
            catch { daDataAdapter.SelectCommand.Connection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SqlDBConnectionString"].ConnectionString; daDataAdapter.Fill(this._dsGlobal, strTableName); }
            daDataAdapter.Dispose();
            cnnLapbase.Close();
            cnnLapbase.Dispose();
        }
    }
    #endregion

    #region public void FetchSystemDetails(string strUserSNo)
    /*
     * this function is to load the detail setting of the current user(strUserSNo is the code of current user from tblUsers), 
     * and is called in different places, 
     * for example in Settings page(Top Menu bar in application) / User's fields sub-page, and
     * wherever we need to check the IMPERIALFLAG to calculate weight data and weight measurment 'KG' & 'LBS'.
     */
    public void FetchSystemDetails(int intUserPracticeCode)
    {
        string strSql;
        SqlCommand cmdSelect = new SqlCommand();

        this.MakeStoreProcedureName(ref cmdSelect, "sp_SystemDetails_LoadData", false);
        cmdSelect.Parameters.Add("@intUserPracticeCode", System.Data.SqlDbType.Int).Value = intUserPracticeCode;

        FillDataSet(cmdSelect, "tblSystemDetails");
        if ((this._dsGlobal.Tables.Count > 0) && (this._dsGlobal.Tables[0].Rows.Count > 0))
        {
            DataRow drSystem = this._dsGlobal.Tables[0].Rows[0];
            UseRace = Convert.ToInt16(drSystem["UseRace"].ToString());
            if (drSystem["ComordityVisitMonths"].ToString().Length > 0)
                ComordityVisitMonths = Convert.ToInt16(drSystem["ComordityVisitMonths"].ToString());
            else
                ComordityVisitMonths = 0;
            ReferenceBMI = Convert.ToInt16(drSystem["ReferenceBMI"].ToString());

            WOScale = drSystem["WOScale"].ToString();

            UseIWonBMI = Convert.ToBoolean(drSystem["IdealonBMI"].ToString());
            PatCOM = Convert.ToBoolean(drSystem["PatCOM"].ToString());
            FUShowVisits = Convert.ToBoolean(drSystem["FUPNotes"].ToString());
            FUCom = Convert.ToBoolean(drSystem["FUCom"].ToString());
            FUinv = Convert.ToBoolean(drSystem["FUinv"].ToString());
            Imperial = Convert.ToBoolean(drSystem["Imperial"].ToString());
            PatInv = Convert.ToBoolean(drSystem["PatInv"].ToString());

            TargetBMI = Convert.ToDouble(drSystem["TargetBMI"].ToString());
        }
        _dsGlobal.Dispose();
        return;
    }
    #endregion

    #region public void FetchSystemInvestigationNormals
    /*
     * this is function is to fetch Investigation data for Investigation form,
     * but as long as the investigation form is not included in the application, this funcion is not used.
     * we just keep this function when we want to implement the investigation form.
     */
    public void FetchSystemInvestigationNormals(int intUserPracticeCode)
    {
        SqlCommand cmdSelect = new SqlCommand();

        MakeStoreProcedureName(ref cmdSelect, "sp_SystemNormals_LoadData", true);
        cmdSelect.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = intUserPracticeCode;

        FillDataSet(cmdSelect, "tblSystemNormals");
        foreach (DataRowView drvRow in this._dsGlobal.Tables[0].DefaultView)
        {
            switch (drvRow["Code"].ToString())
            {
                case "Albumin":
                    AlbuminL = (this.Gender == 1) ? drvRow["MetricLow"].ToString() : drvRow["MetricLow_F"].ToString();
                    AlbuminH = (this.Gender == 1) ? drvRow["MetricHigh"].ToString() : drvRow["MetricHigh_F"].ToString();
                    AlbuminU = drvRow["MetricUnits"].ToString();
                    break;

                case "AlkPhos":
                    AlkPhosL = (this.Gender == 1) ? drvRow["MetricLow"].ToString() : drvRow["MetricLow_F"].ToString();
                    AlkPhosH = (this.Gender == 1) ? drvRow["MetricHigh"].ToString() : drvRow["MetricHigh_F"].ToString();
                    AlkPhosU = drvRow["MetricUnits"].ToString();
                    break;

                case "ALT":
                    ALTL = (this.Gender == 1) ? drvRow["MetricLow"].ToString() : drvRow["MetricLow_F"].ToString();
                    ALTH = (this.Gender == 1) ? drvRow["MetricHigh"].ToString() : drvRow["MetricHigh_F"].ToString();
                    ALTU = drvRow["MetricUnits"].ToString();
                    break;

                case "AST":
                    ASTL = (this.Gender == 1) ? drvRow["MetricLow"].ToString() : drvRow["MetricLow_F"].ToString();
                    ASTH = (this.Gender == 1) ? drvRow["MetricHigh"].ToString() : drvRow["MetricHigh_F"].ToString();
                    ASTU = drvRow["MetricUnits"].ToString();
                    break;

                case "B12":
                    B12L = (this.Gender == 1) ? drvRow["MetricLow"].ToString() : drvRow["MetricLow_F"].ToString();
                    B12H = (this.Gender == 1) ? drvRow["MetricHigh"].ToString() : drvRow["MetricHigh_F"].ToString();
                    B12U = drvRow["MetricUnits"].ToString();
                    break;

                case "Bicarbonate":
                    BicarbonateL = (this.Gender == 1) ? drvRow["MetricLow"].ToString() : drvRow["MetricLow_F"].ToString();
                    BicarbonateH = (this.Gender == 1) ? drvRow["MetricHigh"].ToString() : drvRow["MetricHigh_F"].ToString();
                    BicarbonateU = drvRow["MetricUnits"].ToString();
                    break;

                case "Bilirubin":
                    BilirubinL = (this.Gender == 1) ? drvRow["MetricLow"].ToString() : drvRow["MetricLow_F"].ToString();
                    BilirubinH = (this.Gender == 1) ? drvRow["MetricHigh"].ToString() : drvRow["MetricHigh_F"].ToString();
                    BilirubinU = drvRow["MetricUnits"].ToString();
                    break;

                case "Calcium":
                    CalciumL = (this.Gender == 1) ? drvRow["MetricLow"].ToString() : drvRow["MetricLow_F"].ToString();
                    CalciumH = (this.Gender == 1) ? drvRow["MetricHigh"].ToString() : drvRow["MetricHigh_F"].ToString();
                    CalciumU = drvRow["MetricUnits"].ToString();
                    break;

                case "Chloride":
                    ChlorideL = (this.Gender == 1) ? drvRow["MetricLow"].ToString() : drvRow["MetricLow_F"].ToString();
                    ChlorideH = (this.Gender == 1) ? drvRow["MetricHigh"].ToString() : drvRow["MetricHigh_F"].ToString();
                    ChlorideU = drvRow["MetricUnits"].ToString();
                    break;

                case "CPK":
                    CPKL = (this.Gender == 1) ? drvRow["MetricLow"].ToString() : drvRow["MetricLow_F"].ToString();
                    CPKH = (this.Gender == 1) ? drvRow["MetricHigh"].ToString() : drvRow["MetricHigh_F"].ToString();
                    CPKU = drvRow["MetricUnits"].ToString();
                    break;

                case "Creatinine":
                    CreatinineL = (this.Gender == 1) ? drvRow["MetricLow"].ToString() : drvRow["MetricLow_F"].ToString();
                    CreatinineH = (this.Gender == 1) ? drvRow["MetricHigh"].ToString() : drvRow["MetricHigh_F"].ToString();
                    CreatinineU = drvRow["MetricUnits"].ToString();
                    break;

                case "DiastolicBP":
                    DiastolicBPL = (this.Gender == 1) ? drvRow["MetricLow"].ToString() : drvRow["MetricLow_F"].ToString();
                    DiastolicBPH = (this.Gender == 1) ? drvRow["MetricHigh"].ToString() : drvRow["MetricHigh_F"].ToString();
                    DiastolicBPU = drvRow["MetricUnits"].ToString();
                    break;

                case "FBloodGlucose":
                    FBloodGlucoseL = (this.Gender == 1) ? drvRow["MetricLow"].ToString() : drvRow["MetricLow_F"].ToString();
                    FBloodGlucoseH = (this.Gender == 1) ? drvRow["MetricHigh"].ToString() : drvRow["MetricHigh_F"].ToString();
                    FBloodGlucoseU = drvRow["MetricUnits"].ToString();
                    break;

                case "Ferritin":
                    FerritinL = (this.Gender == 1) ? drvRow["MetricLow"].ToString() : drvRow["MetricLow_F"].ToString();
                    FerritinH = (this.Gender == 1) ? drvRow["MetricHigh"].ToString() : drvRow["MetricHigh_F"].ToString();
                    FerritinU = drvRow["MetricUnits"].ToString();
                    break;

                case "Folate":
                    FolateL = (this.Gender == 1) ? drvRow["MetricLow"].ToString() : drvRow["MetricLow_F"].ToString();
                    FolateH = (this.Gender == 1) ? drvRow["MetricHigh"].ToString() : drvRow["MetricHigh_F"].ToString();
                    FolateU = drvRow["MetricUnits"].ToString();
                    break;

                case "FSerumInsulin":
                    FSerumInsulinL = (this.Gender == 1) ? drvRow["MetricLow"].ToString() : drvRow["MetricLow_F"].ToString();
                    FSerumInsulinH = (this.Gender == 1) ? drvRow["MetricHigh"].ToString() : drvRow["MetricHigh_F"].ToString();
                    FSerumInsulinU = drvRow["MetricUnits"].ToString();
                    break;

                case "GGT":
                    GGTL = (this.Gender == 1) ? drvRow["MetricLow"].ToString() : drvRow["MetricLow_F"].ToString();
                    GGTH = (this.Gender == 1) ? drvRow["MetricHigh"].ToString() : drvRow["MetricHigh_F"].ToString();
                    GGTU = drvRow["MetricUnits"].ToString();
                    break;

                case "HBA1C":
                    HBA1CL = (this.Gender == 1) ? drvRow["MetricLow"].ToString() : drvRow["MetricLow_F"].ToString();
                    HBA1CH = (this.Gender == 1) ? drvRow["MetricHigh"].ToString() : drvRow["MetricHigh_F"].ToString();
                    HBA1CU = drvRow["MetricUnits"].ToString();
                    break;

                case "HDLCholesterol":
                    HDLCholesterolL = (this.Gender == 1) ? drvRow["MetricLow"].ToString() : drvRow["MetricLow_F"].ToString();
                    HDLCholesterolH = (this.Gender == 1) ? drvRow["MetricHigh"].ToString() : drvRow["MetricHigh_F"].ToString();
                    HDLCholesterolU = drvRow["MetricUnits"].ToString();
                    break;

                case "Hemoglobin":
                    HemoglobinL = (this.Gender == 1) ? drvRow["MetricLow"].ToString() : drvRow["MetricLow_F"].ToString();
                    HemoglobinH = (this.Gender == 1) ? drvRow["MetricHigh"].ToString() : drvRow["MetricHigh_F"].ToString();
                    HemoglobinU = drvRow["MetricUnits"].ToString();
                    break;

                case "Homocysteine":
                    HomocysteineL = (this.Gender == 1) ? drvRow["MetricLow"].ToString() : drvRow["MetricLow_F"].ToString();
                    HomocysteineH = (this.Gender == 1) ? drvRow["MetricHigh"].ToString() : drvRow["MetricHigh_F"].ToString();
                    HomocysteineU = drvRow["MetricUnits"].ToString();
                    break;

                case "IBC":
                    IBCL = (this.Gender == 1) ? drvRow["MetricLow"].ToString() : drvRow["MetricLow_F"].ToString();
                    IBCH = (this.Gender == 1) ? drvRow["MetricHigh"].ToString() : drvRow["MetricHigh_F"].ToString();
                    IBCU = drvRow["MetricUnits"].ToString();
                    break;

                case "Iron":
                    IronL = (this.Gender == 1) ? drvRow["MetricLow"].ToString() : drvRow["MetricLow_F"].ToString();
                    IronH = (this.Gender == 1) ? drvRow["MetricHigh"].ToString() : drvRow["MetricHigh_F"].ToString();
                    IronU = drvRow["MetricUnits"].ToString();
                    break;

                case "Phosphate":
                    PhosphateL = (this.Gender == 1) ? drvRow["MetricLow"].ToString() : drvRow["MetricLow_F"].ToString();
                    PhosphateH = (this.Gender == 1) ? drvRow["MetricHigh"].ToString() : drvRow["MetricHigh_F"].ToString();
                    PhosphateU = drvRow["MetricUnits"].ToString();
                    break;

                case "Platelets":
                    PlateletsL = (this.Gender == 1) ? drvRow["MetricLow"].ToString() : drvRow["MetricLow_F"].ToString();
                    PlateletsH = (this.Gender == 1) ? drvRow["MetricHigh"].ToString() : drvRow["MetricHigh_F"].ToString();
                    PlateletsU = drvRow["MetricUnits"].ToString();
                    break;

                case "Potassium":
                    PotassiumL = (this.Gender == 1) ? drvRow["MetricLow"].ToString() : drvRow["MetricLow_F"].ToString();
                    PotassiumH = (this.Gender == 1) ? drvRow["MetricHigh"].ToString() : drvRow["MetricHigh_F"].ToString();
                    PotassiumU = drvRow["MetricUnits"].ToString();
                    break;

                case "Sodium":
                    SodiumL = (this.Gender == 1) ? drvRow["MetricLow"].ToString() : drvRow["MetricLow_F"].ToString();
                    SodiumH = (this.Gender == 1) ? drvRow["MetricHigh"].ToString() : drvRow["MetricHigh_F"].ToString();
                    SodiumU = drvRow["MetricUnits"].ToString();
                    break;

                case "SystolicBP":
                    SystolicBPL = (this.Gender == 1) ? drvRow["MetricLow"].ToString() : drvRow["MetricLow_F"].ToString();
                    SystolicBPH = (this.Gender == 1) ? drvRow["MetricHigh"].ToString() : drvRow["MetricHigh_F"].ToString();
                    SystolicBPU = drvRow["MetricUnits"].ToString();
                    break;

                case "T4":
                    T4L = (this.Gender == 1) ? drvRow["MetricLow"].ToString() : drvRow["MetricLow_F"].ToString();
                    T4H = (this.Gender == 1) ? drvRow["MetricHigh"].ToString() : drvRow["MetricHigh_F"].ToString();
                    T4U = drvRow["MetricUnits"].ToString();
                    break;

                case "T3":
                    T3L = (this.Gender == 1) ? drvRow["MetricLow"].ToString() : drvRow["MetricLow_F"].ToString();
                    T3H = (this.Gender == 1) ? drvRow["MetricHigh"].ToString() : drvRow["MetricHigh_F"].ToString();
                    T3U = drvRow["MetricUnits"].ToString();
                    break;

                case "TotalCholesterol":
                    TotalCholesterolL = (this.Gender == 1) ? drvRow["MetricLow"].ToString() : drvRow["MetricLow"].ToString();
                    TotalCholesterolH = (this.Gender == 1) ? drvRow["MetricHigh"].ToString() : drvRow["MetricHigh_F"].ToString();
                    TotalCholesterolU = drvRow["MetricUnits"].ToString();
                    break;

                case "Tprotein":
                    TproteinL = (this.Gender == 1) ? drvRow["MetricLow"].ToString() : drvRow["MetricLow_F"].ToString();
                    TproteinH = (this.Gender == 1) ? drvRow["MetricHigh"].ToString() : drvRow["MetricHigh_F"].ToString();
                    TproteinU = drvRow["MetricUnits"].ToString();
                    break;

                case "Transferrin":
                    TransferrinL = (this.Gender == 1) ? drvRow["MetricLow"].ToString() : drvRow["MetricLow_F"].ToString();
                    TransferrinH = (this.Gender == 1) ? drvRow["MetricHigh"].ToString() : drvRow["MetricHigh_F"].ToString();
                    TransferrinU = drvRow["MetricUnits"].ToString();
                    break;

                case "Triglycerides":
                    TriglyceridesL = (this.Gender == 1) ? drvRow["MetricLow"].ToString() : drvRow["MetricLow_F"].ToString();
                    TriglyceridesH = (this.Gender == 1) ? drvRow["MetricHigh"].ToString() : drvRow["MetricHigh_F"].ToString();
                    TriglyceridesU = drvRow["MetricUnits"].ToString();
                    break;

                case "TSH":
                    TSHL = (this.Gender == 1) ? drvRow["MetricLow"].ToString() : drvRow["MetricLow_F"].ToString();
                    TSHH = (this.Gender == 1) ? drvRow["MetricHigh"].ToString() : drvRow["MetricHigh_F"].ToString();
                    TSHU = drvRow["MetricUnits"].ToString();
                    break;

                case "Urea":
                    UreaL = (this.Gender == 1) ? drvRow["MetricLow"].ToString() : drvRow["MetricLow_F"].ToString();
                    UreaH = (this.Gender == 1) ? drvRow["MetricHigh"].ToString() : drvRow["MetricHigh_F"].ToString();
                    UreaU = drvRow["MetricUnits"].ToString();
                    break;

                case "VitD":
                    VitDL = (this.Gender == 1) ? drvRow["MetricLow"].ToString() : drvRow["MetricLow_F"].ToString();
                    VitDH = (this.Gender == 1) ? drvRow["MetricHigh"].ToString() : drvRow["MetricHigh_F"].ToString();
                    VitDU = drvRow["MetricUnits"].ToString();
                    break;

                case "WCC":
                    WCCL = (this.Gender == 1) ? drvRow["MetricLow"].ToString() : drvRow["MetricLow_F"].ToString();
                    WCCH = (this.Gender == 1) ? drvRow["MetricHigh"].ToString() : drvRow["MetricHigh_F"].ToString();
                    WCCU = drvRow["MetricUnits"].ToString();
                    break;
            }
        }
        return;
    }
    #endregion

    #region public DataSet LoadLanguageCharacters
    /*
     * this function is to load all letters and characters of selected language on Patient list page, 
     * the return value is a dataset of letters.
     */
    public DataSet LoadLanguageCharacters()
    {
        SqlCommand cmdSelect = new SqlCommand();

        cmdSelect.Parameters.Add("LanguageCode", SqlDbType.VarChar, 3);
        MakeStoreProcedureName(ref cmdSelect, "sp_Characters_LoadData", true);

        cmdSelect.Parameters["LanguageCode"].Value = this._LanguageCode.Substring(0, 2) + "%";
        FillDataSet(cmdSelect, "tblCharacters");
        return (this._dsGlobal);
    }
    #endregion

    #region public void SavePatientData(byte ActionCode, SqlCommand cmdPatient)
    /*
     * this function is to save (Add new/update) the patient data (Patient's detail page) / Demographics sub-page
     * if ActionCode = 1, it means the data should be add as new , else data should be updated.
     * in case of add as new patient, we should return the PatientID (generated as AutoNumber) for the current patient.
     * the data on Demographics, are saved in tblPatients
     */
    public void SavePatientData(byte ActionCode, SqlCommand cmdPatient)
    {
        using (SqlConnection cnnLapbase = new SqlConnection(strSqlCnnString))
        {
            cnnLapbase.Open();
            SqlTransaction transPatient = cnnLapbase.BeginTransaction(IsolationLevel.ReadCommitted);
            cmdPatient.Connection = cnnLapbase;
            cmdPatient.Transaction = transPatient;
            cmdPatient.ExecuteNonQuery();

            // find the last PatientID, if the sqlcommand is a select command
            if (ActionCode == 1) // the data must be inserted
            {
                cmdPatient.Parameters.Clear();
                this.MakeStoreProcedureName(ref cmdPatient, "sp_PatientData_LastPatientID", true);
                PatientID = Convert.ToInt64(cmdPatient.ExecuteScalar());
            }
            transPatient.Commit();
            cnnLapbase.Close();
        }
        return;
    }
    #endregion

    #region public void SavePatientWeightData(SqlCommand cmdPWD)
    /*
     * this function is to save patient's data on Patient Detail page/ other sub-pages except Baseline,
     * all data on other sub-pages, are saved into tblPatientWeightData
     */
    public void SavePatientWeightData(SqlCommand cmdPWD)
    {
        using (SqlConnection cnnLapbase = new SqlConnection(strSqlCnnString))
        {
            cnnLapbase.Open();
            SqlTransaction transPatient = cnnLapbase.BeginTransaction(IsolationLevel.ReadCommitted);
            cmdPWD.Connection = cnnLapbase;
            cmdPWD.Transaction = transPatient;
            cmdPWD.ExecuteNonQuery();
            transPatient.Commit();
            cnnLapbase.Close();
        }
    }
    #endregion

    #region public long SaveVisitData(byte ActionCode, SqlCommand cmdVisit, int intPatientID)
    /*
     * this function is to save (Add new/ update) visit data of the current patient,
     * if ActionCode is 1, it means the data should be added and the last generated ConsultID should be returned
     * otherwise, the data should be updated
     */
    public long SaveVisitData(byte ActionCode, SqlCommand cmdVisit, int intPatientID)
    {
        long ConsultID = 0;
        using (SqlConnection cnnLapbase = new SqlConnection(strSqlCnnString))
        {
            cnnLapbase.Open();
            SqlTransaction transPatient = cnnLapbase.BeginTransaction(IsolationLevel.ReadCommitted);
            cmdVisit.Connection = cnnLapbase;
            cmdVisit.Transaction = transPatient;
            cmdVisit.ExecuteNonQuery();

            if (ActionCode == 1) // the data must be inserted
            {
                cmdVisit.Parameters.Clear();
                MakeStoreProcedureName(ref cmdVisit, "sp_ConsultFU1_ProgressNotes_GetLastConsultID", true);
                cmdVisit.Parameters.Add("@PatientID", SqlDbType.Int).Value = intPatientID;
                ConsultID = Convert.ToInt64(cmdVisit.ExecuteScalar());
            }
            transPatient.Commit();
            cnnLapbase.Close();
        }
        return (ConsultID);
    }
    #endregion

    #region public long SaveCommentData(byte ActionCode, SqlCommand cmdVisit, int intPatientID, int intOrganizationCode)
    /*
     * this function is to save (Add new/ update) comment data of the current patient,
     * if ActionCode is 1, it means the data should be added and the last generated CommentID should be returned
     * otherwise, the data should be updated
     */
    public long SaveCommentData(byte ActionCode, SqlCommand cmdComment, int intPatientID, int intOrganizationCode)
    {
        long CommentID = 0;
        using (SqlConnection cnnLapbase = new SqlConnection(strSqlCnnString))
        {
            cnnLapbase.Open();
            SqlTransaction transPatient = cnnLapbase.BeginTransaction(IsolationLevel.ReadCommitted);
            cmdComment.Connection = cnnLapbase;
            cmdComment.Transaction = transPatient;
            cmdComment.ExecuteNonQuery();

            if (ActionCode == 1) // the data must be inserted
            {
                cmdComment.Parameters.Clear();
                MakeStoreProcedureName(ref cmdComment, "sp_ConsultFU1_ProgressNotes_GetLastCommentID", true);
                cmdComment.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = intOrganizationCode;
                cmdComment.Parameters.Add("@PatientID", SqlDbType.Int).Value = intPatientID;
                CommentID = Convert.ToInt64(cmdComment.ExecuteScalar());
            }
            transPatient.Commit();
            cnnLapbase.Close();
        }
        return (CommentID);
    }
    #endregion

    #region public DataSet FetchData(OleDbCommand cmdSelect, string strTableName)
    /*
     * this function is a global function that runs cmdSelect (OLEDB and SQL Command) and returns a dataset containing a table (table name is strTableName),
     * the table name is important when we use the return dataset in XSL files to load XML data
     */
    public DataSet FetchData(OleDbCommand cmdSelect, string strTableName)
    {
        FillDataSet(cmdSelect, strTableName);
        return (this._dsGlobal);
    }
    #endregion

    #region public DataSet FetchData(SqlCommand cmdSelect, string strTableName)
    /*
     * this function is a global function that runs cmdSelect (OLEDB and SQL Command) and returns a dataset containing a table (table name is strTableName),
     * the table name is important when we use the return dataset in XSL files to load XML data
     */
    public DataSet FetchData(SqlCommand cmdSelect, string strTableName)
    {
        FillDataSet(cmdSelect, strTableName);
        return (this._dsGlobal);
    }
    #endregion

    #region public int ExecuteDMLCommand(OleDbCommand cmdCommand)
    /*this function is a global function and to run DML (such as Insert, Update, delete and etc) DBCommand (OLE or SQL)*/
    public int ExecuteDMLCommand(OleDbCommand cmdCommand)
    {
        int intReturn = 0;
        try
        {
            using (OleDbConnection cnnLapbase = new OleDbConnection(strLapbaseCnnString))
            {
                try { cnnLapbase.Open(); }
                catch { cnnLapbase.ConnectionString = System.Configuration.ConfigurationManager.AppSettings["DBConnectionString"]; cnnLapbase.Open(); }
                cmdCommand.Connection = cnnLapbase;
                intReturn = cmdCommand.ExecuteNonQuery();
                cnnLapbase.Close();
                cnnLapbase.Dispose();
            }
        }
        catch { intReturn = -1; }
        return (intReturn);
    }
    #endregion

    #region public int ExecuteDMLCommand(SqlCommand cmdCommand)
    /*this function is a global function and to run DML (such as Insert, Update, delete and etc) DBCommand (OLE or SQL)*/
    public int ExecuteDMLCommand(SqlCommand cmdCommand)
    {
        int intReturn = 0;
        using (SqlConnection cnnLapbase = new SqlConnection(strSqlCnnString))
        {
            try { cnnLapbase.Open(); }
            catch { cnnLapbase.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SqlDBConnectionString"].ConnectionString; cnnLapbase.Open(); }
            cmdCommand.Connection = cnnLapbase;
            intReturn = cmdCommand.ExecuteNonQuery();
            cnnLapbase.Close();
        }
        return (intReturn);
    }
    #endregion

    #region public string TruncateDate(string strDate, string strCultureInfo, byte intDateType)
    /*
     * this function is to change the format of input parameter date (strDate),
     * for example, all dates are save in Sql Server with 00:00:00 as Time part, but when we load data from Sql Server, 
     * we need to omit this time part, this is an example of usage of this function
     */
    public string TruncateDate(string strDate, string strCultureInfo, byte intDateType)
    {
        DateTimeFormatInfo myDTFI = new CultureInfo(strCultureInfo, false).DateTimeFormat;
        DateTime resultDate = DateTime.Now;

        if (strDate.Length != 0)
        {
            if (DateTime.TryParse(strDate, myDTFI, DateTimeStyles.None, out resultDate))
            {
                switch (intDateType)
                {
                    case 1:  // DD/MM/YYYY or MM/DD/YYYY
                        strDate = resultDate.ToString(/*"d"*/);
                        if (strDate.Length > 10)
                            strDate = strDate.Substring(0, strDate.IndexOf(" "));
                        break;
                    case 2: // MM/YYYY
                        strDate = GetMonthName(resultDate.Month) + " " + resultDate.Year.ToString();
                        break;
                    case 3: // DD MMM YYYY
                        strDate = resultDate.Day + " " + GetMonthName(resultDate.Month) + " " + resultDate.Year.ToString();
                        break;
                }
            }
            else
                strDate = "";
        }
        else strDate = "";
        return (strDate);
    }
    #endregion

    #region public void AddLogParameters(ref OleDbCommand cmdCommand, string strLogonUserName, string strRemoteIPAddress, string strAction)
    /*
     * In Access version of lapbase application, there 8 fields for log events in all tables, 4 fields when data are added, 4 fields when data are updated
     * So, in Lapbase web application, at the begining, we added these 8 fields in tables.
     * during runtime of application, we needed to fill these 8 fields based on transaction "insert" and "update"
     * this function is to fill these 8 fields based on transaction
     * 
     * we have 2 versions of this function using OLE or SQL DBCommand
     */
    public void AddLogParameters(ref OleDbCommand cmdCommand, string strLogonUserName, string strRemoteIPAddress, string strAction)
    {
        string[] InsertParams = { "DateCreated", "CreatedByUser", "CreatedByComputer", "CreatedByWindowsUser" };
        string[] UpdateParams = { "LastModified", "ModifiedByUser", "ModifiedByComputer", "ModifiedByWindowsUser" };
        string[] ParamsValue = { "", strLogonUserName, strRemoteIPAddress, "" };

        switch (strAction.ToLower())
        {
            case "insert":
                for (int Idx = 0; Idx < 4; Idx++)
                    if (Idx == 0)
                        cmdCommand.Parameters.Add(InsertParams[Idx], OleDbType.Date).Value = Convert.ToDateTime(DateTime.Now.ToShortDateString() + "  " + DateTime.Now.ToShortTimeString());
                    else
                        cmdCommand.Parameters.Add(InsertParams[Idx], OleDbType.VarChar, 30).Value = ParamsValue[Idx];
                break;
            case "update":
                for (int Idx = 0; Idx < 4; Idx++)
                    if (Idx == 0)
                        cmdCommand.Parameters.Add(UpdateParams[Idx], OleDbType.Date).Value = Convert.ToDateTime(DateTime.Now.ToShortDateString() + "  " + DateTime.Now.ToShortTimeString());
                    else
                        cmdCommand.Parameters.Add(UpdateParams[Idx], OleDbType.VarChar, 30).Value = ParamsValue[Idx];
                break;
        }
        return;
    }
    #endregion

    #region public void AddLogParameters(ref OleDbCommand cmdCommand, string strLogonUserName, string strRemoteIPAddress, string strAction)
    /*
     * In Access version of lapbase application, there 8 fields for log events in all tables, 4 fields when data are added, 4 fields when data are updated
     * So, in Lapbase web application, at the begining, we added these 8 fields in tables.
     * during runtime of application, we needed to fill these 8 fields based on transaction "insert" and "update"
     * this function is to fill these 8 fields based on transaction
     * 
     * we have 2 versions of this function using OLE or SQL DBCommand
     */
    public void AddLogParameters(ref SqlCommand cmdCommand, string strLogonUserName, string strRemoteIPAddress, string strAction)
    {
        string[] InsertParams = { "DateCreated", "CreatedByUser", "CreatedByComputer", "CreatedByWindowsUser" };
        string[] UpdateParams = { "LastModified", "ModifiedByUser", "ModifiedByComputer", "ModifiedByWindowsUser" };
        string[] ParamsValue = { "", strLogonUserName, strRemoteIPAddress, "" };

        switch (strAction.ToLower())
        {
            case "insert":
                for (int Idx = 0; Idx < 4; Idx++)
                    if (Idx == 0)
                        cmdCommand.Parameters.Add(InsertParams[Idx], SqlDbType.DateTime).Value = Convert.ToDateTime(DateTime.Now.ToShortDateString() + "  " + DateTime.Now.ToShortTimeString());
                    else
                        cmdCommand.Parameters.Add(InsertParams[Idx], SqlDbType.VarChar, 30).Value = ParamsValue[Idx];
                break;
            case "update":
                for (int Idx = 0; Idx < 4; Idx++)
                    if (Idx == 0)
                        cmdCommand.Parameters.Add(UpdateParams[Idx], SqlDbType.DateTime).Value = Convert.ToDateTime(DateTime.Now.ToShortDateString() + "  " + DateTime.Now.ToShortTimeString());
                    else
                        cmdCommand.Parameters.Add(UpdateParams[Idx], SqlDbType.VarChar, 30).Value = ParamsValue[Idx];
                break;
        }
        return;
    }
    #endregion

    #region public void SaveUserLogFile
    /*this function is to add a log record into tblUsysUserObjectLogs, for each event during runtime of application*/
    public void SaveUserLogFile(string strUser_SNo, string strLogonUserName, string strRemoteIPAddress, string strObjectName,
                                short ObjectType, string strActionName, string strSchemaName, string strSchemaCode)
    {
        SqlCommand cmdInsert = new SqlCommand();

        MakeStoreProcedureName(ref cmdInsert, "sp_UsysUserObjectLogs_InsertData", false);
        cmdInsert.Parameters.Add("ComputerName", SqlDbType.VarChar, 50).Value = strRemoteIPAddress; ;
        cmdInsert.Parameters.Add("UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(strUser_SNo); ;
        cmdInsert.Parameters.Add("SystemUsername", SqlDbType.VarChar, 50).Value = "";
        cmdInsert.Parameters.Add("AccessUsername", SqlDbType.VarChar, 50).Value = strLogonUserName;
        cmdInsert.Parameters.Add("ObjectName", SqlDbType.VarChar, 255).Value = strObjectName;
        cmdInsert.Parameters.Add("ObjectType", SqlDbType.SmallInt).Value = ObjectType;
        cmdInsert.Parameters.Add("OpenTime", SqlDbType.DateTime).Value = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString(); ;
        cmdInsert.Parameters.Add("ActionName", SqlDbType.VarChar, 255).Value = strActionName;
        cmdInsert.Parameters.Add("SchemaName", SqlDbType.VarChar, 255).Value = strSchemaName;
        cmdInsert.Parameters.Add("SchemaCode", SqlDbType.VarChar, 255).Value = strSchemaCode;
        ExecuteDMLCommand(cmdInsert);
        return;
    }
    #endregion

    #region public void SaveActionLog
    /*this function is to add a log record into tblActionLog, for each event during runtime of application*/
    public void SaveActionLog(string strOrganizationCode, string strUserPracticeCode, string strComputerName, string strPage,
                                string strAction, string strActionDetail, string strPatientID, string strRecordID)
    {
        SqlCommand cmdInsert = new SqlCommand();

        MakeStoreProcedureName(ref cmdInsert, "sp_ActionLog_InsertData", false);
        cmdInsert.Parameters.Add("OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(strOrganizationCode);
        cmdInsert.Parameters.Add("UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(strUserPracticeCode);
        cmdInsert.Parameters.Add("ComputerName", SqlDbType.VarChar, 255).Value = strComputerName;
        cmdInsert.Parameters.Add("Page", SqlDbType.VarChar, 255).Value = strPage;
        cmdInsert.Parameters.Add("Action", SqlDbType.VarChar, 255).Value = strAction;
        cmdInsert.Parameters.Add("ActionDetail", SqlDbType.VarChar, 255).Value = strActionDetail;
        cmdInsert.Parameters.Add("PatientID", SqlDbType.VarChar, 255).Value = strPatientID;
        cmdInsert.Parameters.Add("RecordID", SqlDbType.VarChar, 255).Value = strRecordID;
        cmdInsert.Parameters.Add("DateTime", SqlDbType.DateTime).Value = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();

        ExecuteDMLCommand(cmdInsert);
        return;
    }
    #endregion 

    #region public void CalculateWeightData(ref DataSet dsPatient,string TableName, bool ImperialFlag)
    /*
     * this function is to calculate Weight data that are used in visit page / visit list and in all reports containing visit data such as letter to doctor, follow up assessment and etc
     */
    public void CalculateWeightData(ref DataSet dsPatient, string TableName, bool ImperialFlag)
    {
        DataTable dtTemp = dsPatient.Tables[TableName];
        DataColumn dcTemp = new DataColumn();

        dcTemp.ColumnName = "Weeks";
        dcTemp.DataType = Type.GetType("System.Decimal");
        dcTemp.DefaultValue = 0;
        dtTemp.Columns.Add(dcTemp);

        dcTemp = new DataColumn();
        dcTemp.ColumnName = "WeightLoss";
        dcTemp.DataType = Type.GetType("System.Decimal");
        dcTemp.DefaultValue = 0;
        dtTemp.Columns.Add(dcTemp);

        dcTemp = new DataColumn();
        dcTemp.ColumnName = "WeightLossPerWeek";
        dcTemp.DataType = Type.GetType("System.Decimal");
        dcTemp.DefaultValue = 0;
        dtTemp.Columns.Add(dcTemp);

        for (int Xh = 0; Xh < dtTemp.Rows.Count; Xh++)
        {
            if (dtTemp.Rows[Xh]["CommentOnly"].ToString() == "0")
            {
                TimeSpan tsTemp = new TimeSpan();
                DateTime CurrentVisitDate, PrevVisitDate = new DateTime();
                DateTime TempPrevVisitDate = new DateTime();
                decimal CurrentWeight, PrevWeight = 0;
                bool PrevVisitDateFlag = true;


                if (!DateTime.TryParse(dtTemp.Rows[Xh]["DateSeen"].ToString(), out CurrentVisitDate))
                    CurrentVisitDate = new DateTime();

                if (!Decimal.TryParse(dtTemp.Rows[Xh]["Weight"].ToString(), out CurrentWeight))
                    CurrentWeight = 0;

                if (Xh + 1 <= dtTemp.Rows.Count - 1)
                {
                    for (int Xh2 = Xh; Xh2 < dtTemp.Rows.Count; Xh2++)
                    {
                        if (dtTemp.Rows[Xh2]["CommentOnly"].ToString() == "0")
                        {
                            if (Xh2 < dtTemp.Rows.Count)
                            {
                                if (!DateTime.TryParse(dtTemp.Rows[Xh2]["DateSeen"].ToString(), out TempPrevVisitDate))
                                    TempPrevVisitDate = new DateTime();

                                if (CurrentVisitDate > TempPrevVisitDate)
                                {
                                    if (!DateTime.TryParse(dtTemp.Rows[Xh2]["DateSeen"].ToString(), out PrevVisitDate))
                                    {
                                        PrevVisitDate = DateTime.MinValue; PrevVisitDateFlag = false;
                                    }

                                    if (!Decimal.TryParse(dtTemp.Rows[Xh2]["Weight"].ToString(), out PrevWeight))
                                        PrevWeight = 0;
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (!DateTime.TryParse(dtTemp.Rows[Xh]["LapbandDate"].ToString(), out PrevVisitDate))
                    {
                        PrevVisitDate = DateTime.MinValue; PrevVisitDateFlag = false;
                    }
                    if (!Decimal.TryParse(dtTemp.Rows[Xh]["StartWeight"].ToString(), out PrevWeight)) PrevWeight = 0;
                }

                // Weeks between current visit with previous visit/lapband date
                if (!PrevVisitDateFlag)
                    dtTemp.Rows[Xh]["Weeks"] = 0;
                else
                    if (CurrentVisitDate >= PrevVisitDate)
                    {
                        tsTemp = new System.TimeSpan(CurrentVisitDate.Ticks - PrevVisitDate.Ticks);
                        dtTemp.Rows[Xh]["Weeks"] = Math.Ceiling(Convert.ToDecimal(tsTemp.Days / 7));
                    }
                    else
                        dtTemp.Rows[Xh]["Weeks"] = 0;

                // Weight Loss between the current weight and previous weight
                dtTemp.Rows[Xh]["WeightLoss"] = PrevWeight - CurrentWeight;

                // Weight Loss per week
                try { dtTemp.Rows[Xh]["WeightLossPerWeek"] = (PrevWeight - CurrentWeight) / (Math.Ceiling(Convert.ToDecimal(tsTemp.Days / 7))); }
                catch { dtTemp.Rows[Xh]["WeightLossPerWeek"] = 0; }
                dsPatient.AcceptChanges();
            }
        }
        return;
    }
    #endregion

    #region public string GetMonthName(int intMonth)
    public string GetMonthName(int intMonth)
    {
        string strMonthName = "";

        switch (intMonth)
        {
            case 1:
                strMonthName = "Jan";
                break;
            case 2:
                strMonthName = "Feb";
                break;
            case 3:
                strMonthName = "Mar";
                break;
            case 4:
                strMonthName = "Apr";
                break;
            case 5:
                strMonthName = "May";
                break;
            case 6:
                strMonthName = "Jun";
                break;
            case 7:
                strMonthName = "Jul";
                break;
            case 8:
                strMonthName = "Aug";
                break;
            case 9:
                strMonthName = "Sep";
                break;
            case 10:
                strMonthName = "Oct";
                break;
            case 11:
                strMonthName = "Nov";
                break;
            case 12:
                strMonthName = "Dec";
                break;
        }
        return (strMonthName);
    }
    #endregion

    #region public void InitialParameters(ref OleDbCommand cmdOperation)
    /*this function is to initilize the OLE and SQL DBCommand, after creating the command*/
    public void InitialParameters(ref OleDbCommand cmdCommand)
    {
        for (int Xh = 0; Xh < cmdCommand.Parameters.Count; Xh++)
            switch (cmdCommand.Parameters[Xh].OleDbType)
            {
                case OleDbType.VarChar:
                    cmdCommand.Parameters[Xh].Value = "";
                    break;
                case OleDbType.Decimal:
                case OleDbType.Double:
                case OleDbType.Integer:
                    cmdCommand.Parameters[Xh].Value = 0;
                    break;
                case OleDbType.Boolean:
                    cmdCommand.Parameters[Xh].Value = false;
                    break;
                case OleDbType.Date:
                    cmdCommand.Parameters[Xh].Value = DBNull.Value;
                    break;
            }
        return;
    }
    #endregion

    #region public void InitialParameters(ref SqlCommand cmdCommand)
    /*this function is to initilize the OLE and SQL DBCommand, after creating the command*/
    public void InitialParameters(ref SqlCommand cmdCommand)
    {
        for (int Xh = 0; Xh < cmdCommand.Parameters.Count; Xh++)
            switch (cmdCommand.Parameters[Xh].SqlDbType)
            {
                case SqlDbType.VarChar:
                    cmdCommand.Parameters[Xh].Value = "";
                    break;
                case SqlDbType.Decimal:
                case SqlDbType.Int:
                    cmdCommand.Parameters[Xh].Value = 0;
                    break;
                case SqlDbType.Bit:
                    cmdCommand.Parameters[Xh].Value = false;
                    break;
                case SqlDbType.DateTime:
                    cmdCommand.Parameters[Xh].Value = DBNull.Value;
                    break;
            }
        return;
    }
    #endregion

    #region public string SaveOperationData(SqlCommand cmdSave)
    /*
     * this function is to save Operation data (Add new / Update)
     * in case of adding new operation data, the DBCommand doesn't include the @AdmitID parameter, in this case we need to fetch out the last operation ID (AdmitID) from tblOpEvents
     */
    public string SaveOperationData(SqlCommand cmdSave)
    {
        string tempAdmitNo = "0";

        using (SqlConnection cnnLapbase = new SqlConnection(strSqlCnnString))
        {
            cnnLapbase.Open();
            SqlTransaction transPatient = cnnLapbase.BeginTransaction(IsolationLevel.ReadCommitted);
            cmdSave.Connection = cnnLapbase;
            cmdSave.Transaction = transPatient;
            cmdSave.ExecuteNonQuery();

            // for new operation we should fetch the serial no for the newest operation
            if (!cmdSave.Parameters.Contains("@AdmitID"))
            {
                cmdSave.Parameters.Clear();
                MakeStoreProcedureName(ref cmdSave, "sp_PatientOperations_cmdSelectLastOperationSerialNo", true);
                try
                {
                    _dsGlobal = new DataSet();
                    SqlDataAdapter tempDA = new SqlDataAdapter();
                    tempDA.SelectCommand = cmdSave;
                    tempDA.SelectCommand.Connection = cnnLapbase;
                    tempDA.Fill(this._dsGlobal, "tblOperation");
                    tempDA.Dispose();

                    DataView tempDV = _dsGlobal.Tables[0].DefaultView;
                    tempAdmitNo = tempDV.Count > 0 ? tempDV[0]["AdmitID"].ToString() : "0";
                    transPatient.Commit();
                }
                catch (Exception err)
                {
                    transPatient.Rollback();
                    tempAdmitNo = err.ToString();
                }
            }
            else transPatient.Commit();
            cnnLapbase.Close();
        }
        return (tempAdmitNo);
    }
    #endregion

    #region private void FindMinMax(DataView dvTemp, string FieldName, ref string strMax, ref string strMin)
    /*
     * this function is to find out the min & max value of FieldName (input parameter) in dvTemp(Dataview) 
     * for Graph Reports (in Visit page/ Report WL and %EWL graphs)
     */
    public void FindMinMax(DataView dvTemp, string FieldName, ref string strMax, ref string strMin)
    {
        decimal minValue = 0, maxValue = 100;

        dvTemp.Sort = FieldName + " ASC";
        strMin = dvTemp[0][FieldName].ToString();
        dvTemp.Sort = FieldName + " DESC";
        strMax = dvTemp[0][FieldName].ToString();
        dvTemp.Dispose();

        try { minValue = Convert.ToDecimal(strMin); }
        catch { minValue = 0; }

        try { maxValue = Convert.ToDecimal(strMax); }
        catch { maxValue = 100; }

        if ((maxValue > 0) && (maxValue < 100)) maxValue = 100;
        else if ((maxValue >= 100) && (maxValue < 150)) maxValue = 150;
        else if ((maxValue >= 150) && (maxValue < 200)) maxValue = 200;
        else maxValue = 300;

        if (minValue > 0) minValue = 0;
        else if ((minValue < 0) && (minValue > -50)) minValue = -50;
        else if ((minValue <= -50) && (minValue > -100)) minValue = -100;
        else minValue = -200;

        strMin = minValue.ToString();
        strMax = maxValue.ToString();
    }
    #endregion

    #region private XmlDocument GetXmlDocument(2 Parameter)
    /*
     * this function is to generate an SOAP document for strReturnValue.
     * in some cases, we use SOAP to send XMLHTTPRequest (for ajax function using javascript component) to server to run a particular function, 
     * because of sending SOAP request, we need to generate a SOAP Response at the server-side and send it back to client, 
     * so this function generate XML Document for SOAP Response, and at the client-side, after receiving the whole response from server, 
     * we use XmlHttpResponse.responseXML instead of XmlHttpResponse.responseText
     */
    public XmlDocument GetXmlDocument(Guid id, string strReturnValue)
    {
        XmlDocument xml = new XmlDocument();
        System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
        XmlTextWriter writer = new XmlTextWriter(memoryStream, System.Text.Encoding.UTF8);

        writer.WriteStartElement("UploadResponse");
        writer.WriteElementString("ID", id.ToString());
        writer.WriteElementString("ReturnValue", strReturnValue);
        writer.WriteEndElement();
        writer.Flush();

        writer.BaseStream.Position = 0;
        xml.Load(writer.BaseStream);
        writer.Close();

        return xml;
    }
    #endregion

    #region private XmlDocument GetXmlDocument(4 Parameter)
    /*
     * this function is to generate an SOAP document for strReturnValue.
     * in some cases, we use SOAP to send XMLHTTPRequest (for ajax function using javascript component) to server to run a particular function, 
     * because of sending SOAP request, we need to generate a SOAP Response at the server-side and send it back to client, 
     * so this function generate XML Document for SOAP Response, and at the client-side, after receiving the whole response from server, 
     * we use XmlHttpResponse.responseXML instead of XmlHttpResponse.responseText
     */
    public XmlDocument GetXmlDocument(Guid id, string errorMessage, long offset, int bufferLength)
    {
        XmlDocument xml = new XmlDocument();
        System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();

        XmlTextWriter writer = new XmlTextWriter(memoryStream, System.Text.Encoding.UTF8);
        writer.WriteStartElement("UploadResponse");
        writer.WriteElementString("ID", id.ToString());
        writer.WriteElementString("ErrorMessage", errorMessage);
        writer.WriteElementString("OffSet", offset.ToString());
        writer.WriteElementString("BufferLength", bufferLength.ToString());
        writer.WriteEndElement();
        writer.Flush();

        writer.BaseStream.Position = 0;
        xml.Load(writer.BaseStream);
        writer.Close();

        return xml;
    }
    #endregion

    #region public int SaveDocumentAndContent(SqlCommand cmdSave, SqlCommand cmdSaveContent)
    /*
     * this function is to save document information and content, when user uploads his/her file
     */
    public int SaveDocumentAndContent(SqlCommand cmdSave, SqlCommand cmdSaveContent)
    {
        int longDocumentID = 0;
        using (SqlConnection cnnLapbase = new SqlConnection(strSqlCnnString))
        {
            cnnLapbase.Open();
            SqlTransaction transPatient = cnnLapbase.BeginTransaction(IsolationLevel.ReadCommitted);
            cmdSave.Connection = cnnLapbase;

            cmdSave.Transaction = transPatient;
            cmdSaveContent.Transaction = transPatient;
            cmdSave.ExecuteNonQuery();

            // find the last tblPatientDocumentsID, if the sqlcommand is a select command
            cmdSave.Parameters.Clear();
            MakeStoreProcedureName(ref cmdSave, "sp_FileManagement_PatientDocuments_GetLastPatientDocumentsID", true);
            longDocumentID = Convert.ToInt32(cmdSave.ExecuteScalar());

            transPatient.Commit();
            cmdSaveContent.Connection = cnnLapbase;
            cmdSaveContent.Parameters["@tblPatientDocumentsID"].Value = longDocumentID;
            cmdSaveContent.ExecuteNonQuery();
            cnnLapbase.Close();
        }
        return (longDocumentID);
    }
    #endregion

    #region public void AddDocumentEventLog(string strOrganizationCode, int intPatientDocumentsID, byte intEventCode, long intUserSNo, string strLocation, string strNotes)
    /*
     * this function is to insert a log record for each transaction that happened on patient's document, for example adding, viewing, updating, deleting and etc
     */
    public void AddDocumentEventLog(string strOrganizationCode, int intPatientDocumentsID, byte intEventCode, long intUserSNo, string strLocation, string strNotes)
    {
        SqlCommand cmdInsert = new SqlCommand();

        MakeStoreProcedureName(ref cmdInsert, "sp_FileManagement_EventLog_InsertData", true);
        cmdInsert.Parameters.Add("@tblPatientDocumentsID", SqlDbType.Int).Value = intPatientDocumentsID;
        cmdInsert.Parameters.Add("@EventCode", SqlDbType.SmallInt).Value = intEventCode;
        cmdInsert.Parameters.Add("@tblUSERS_SNO", SqlDbType.Int).Value = intUserSNo;
        cmdInsert.Parameters.Add("@Location", SqlDbType.VarChar, 50).Value = strLocation;
        cmdInsert.Parameters.Add("@Notes", SqlDbType.VarChar, 512).Value = strNotes;
        cmdInsert.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(strOrganizationCode);

        this.ExecuteDMLCommand(cmdInsert);
        cmdInsert.Dispose();
    }
    #endregion

    #region public string SaveDoctorData(SqlCommand cmdSave, int OrganizationCode, int UserPracticeCode, bool ActionCode)
    /*
     * this function is to save (add new/ update) doctor's data, in case of adding a new doctor, we return the DoctorID for the current doctor
     */
    public string SaveDoctorData(SqlCommand cmdSave, int OrganizationCode, int UserPracticeCode, bool ActionCode)
    {
        string strDoctorID = "0";

        using (SqlConnection cnnLapbase = new SqlConnection(strSqlCnnString))
        {
            cnnLapbase.Open();
            SqlTransaction transPatient = cnnLapbase.BeginTransaction(IsolationLevel.ReadUncommitted);
            try
            {
                cmdSave.Connection = cnnLapbase;
                cmdSave.Transaction = transPatient;
                cmdSave.ExecuteNonQuery();

                if (ActionCode) // the data must be inserted
                {
                    cmdSave.Parameters.Clear();
                    MakeStoreProcedureName(ref cmdSave, "sp_Doctors_GetLastDoctorID", true);
                    strDoctorID = cmdSave.ExecuteScalar().ToString();
                }
                transPatient.Commit();
            }
            catch (Exception err)
            {
                strDoctorID = err.ToString();
                transPatient.Rollback();
            }
            cnnLapbase.Close();
        }
        return (strDoctorID);
    }
    #endregion

    #region public string SaveSystemDetails(SqlCommand cmdSave, int UserPracticeCode, bool ActionCode)
    /*
     * this function is to save (add as new/ update) the system details (User's fields sub page in MenuSetup page), in case of adding new data, the last system id should be returned,
     */
    public string SaveSystemDetails(SqlCommand cmdSave, int UserPracticeCode, bool InsertFlag)
    {
        string strSystemID = "0";

        using (SqlConnection cnnLapbase = new SqlConnection(strSqlCnnString))
        {
            cnnLapbase.Open();
            SqlTransaction transPatient = cnnLapbase.BeginTransaction(IsolationLevel.ReadUncommitted);
            cmdSave.Connection = cnnLapbase;
            cmdSave.Transaction = transPatient;
            cmdSave.ExecuteNonQuery();

            //if (InsertFlag) // the data must be inserted
            //{
            //    cmdSave.Parameters.Clear();
            //    MakeStoreProcedureName(ref cmdSave, "sp_SystemDetails_LoadData", true);
            //    cmdSave.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = UserPracticeCode;
            //    strSystemID = cmdSave.ExecuteScalar().ToString();
            //}
            transPatient.Commit();
            cnnLapbase.Close();
        }
        return (strSystemID);
    }
    #endregion

    #region public void ReturnToLoginPage(string strDomainName , string strLanguageCode, HttpResponse Response)
    public void ReturnToLoginPage(string strDomainName, string strLanguageCode, HttpResponse Response)
    {
        this._LanguageCode = strLanguageCode;
        Response.Redirect("http://" + strDomainName,false);
    }
    #endregion

    #region public DataSet FetchCaptions(string strAppSchemaID, string strCultureInfo)
    /*
     * this function is to load all titles and caption of the current application schema(strAppSchemaID) by using CultureInfo of the selected langauge (selected language on login page)
     * abd retrun a dataset of captions
     */
    public DataSet FetchCaptions(string strAppSchemaID, string strCultureInfo)
    {
        SqlCommand cmdCaption = new SqlCommand();
        MakeStoreProcedureName(ref cmdCaption, "sp_AppSchemaFields_LoadCaptions", false);
        cmdCaption.Parameters.Add("@strAppSchemaID", SqlDbType.VarChar, 100).Value = strAppSchemaID;
        cmdCaption.Parameters.Add("@strCulture", SqlDbType.VarChar, 10).Value = strCultureInfo.Substring(0, 5);
        return (this.FetchData(cmdCaption, "tblCaptions"));
    }
    #endregion

    #region public void AddNewUser(SqlCommand cmdSave)
    public void AddNewUser(SqlCommand cmdSave)
    {
        using (SqlConnection cnnLapbase = new SqlConnection(strSqlCnnString))
        {
            cnnLapbase.Open();
            cmdSave.Connection = cnnLapbase;
            cmdSave.ExecuteNonQuery();
            _User_SNo = Convert.ToInt32(cmdSave.Parameters["@UserPracticeCode"].Value);
            cnnLapbase.Close();
        }
        return;
    }
    #endregion

    #region public Byte[] GetSecureBinaryData(String strValue)
    /*
     * this function is to convert an encrypted data for input parameter,
     * this function usually is used to encrypt passwords
     */
    public Byte[] GetSecureBinaryData(String strValue)
    {
        System.Security.Cryptography.MD5CryptoServiceProvider md5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
        Byte[] hashedDataBytes;
        System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();

        hashedDataBytes = md5Hasher.ComputeHash(encoder.GetBytes(strValue));
        return hashedDataBytes;
    }
    #endregion

    #region public void MakeStoreProcedureName(ref SqlCommand cmdCommand, string strStoreProcedureName, Boolean AddVersionFlag)
    /*
     * this function is to generate name for SqlServer Storeprocedure,
     * if they are related to particular version, it adds a prefix "Application version" to store procedure's name
     * this function is used for both OLE DBcommand and SQL DBCommand
     */
    public void MakeStoreProcedureName(ref SqlCommand cmdCommand, string strStoreProcedureName, Boolean AddVersionFlag)
    {
        cmdCommand.CommandType = CommandType.StoredProcedure;
        if (AddVersionFlag)
            cmdCommand.CommandText = "dbo." + System.Configuration.ConfigurationManager.AppSettings["ApplicationVersion"].Replace(".", "_") + "_" + strStoreProcedureName;
        else
            cmdCommand.CommandText = "dbo." + strStoreProcedureName;
    }
    #endregion

    #region public void MakeStoreProcedureName(ref OleDbCommand cmdCommand, string strStoreProcedureName, Boolean AddVersionFlag)
    /*
     * this function is to generate name for SqlServer Storeprocedure,
     * if they are related to particular version, it adds a prefix "Application version" to store procedure's name
     * this function is used for both OLE DBcommand and SQL DBCommand
     */
    public void MakeStoreProcedureName(ref OleDbCommand cmdCommand, string strStoreProcedureName, Boolean AddVersionFlag)
    {
        cmdCommand.CommandType = CommandType.StoredProcedure;
        if (AddVersionFlag)
            cmdCommand.CommandText = "dbo." + System.Configuration.ConfigurationManager.AppSettings["ApplicationVersion"].Replace(".", "_") + "_" + strStoreProcedureName;
        else
            cmdCommand.CommandText = "dbo." + strStoreProcedureName;
    }
    #endregion

    #region public Boolean DoesOrganizationHaveLanguage(string strOrganization, out String strLanguage, out String strLanguageCode, out String strCultureInfo)
    public Boolean DoesOrganizationHaveLanguage(string strOrganization, out String strLanguage, out String strLanguageCode, out String strCultureInfo, out String strDirection)
    /*
     * this is function is to check and load the defined Language for current Organizaton ,
     * if there is a predefined language, loads back 
     *  strLanguage ("English-US", "English-Australia")
     *  strLanguageCode ("en-US")
     *  strCultureInfo  ("EN-US")
     */
    {
        SqlCommand cmdSelect = new SqlCommand();

        this.MakeStoreProcedureName(ref cmdSelect, "sp_UsersManagement_LoadOrganizationLanguage", false);
        cmdSelect.Parameters.Add("@strOrganizationName", SqlDbType.VarChar, 100).Value = strOrganization;
        cmdSelect.Parameters.Add("@strLanguage", SqlDbType.VarChar, 50); cmdSelect.Parameters["@strLanguage"].Direction = ParameterDirection.Output;
        cmdSelect.Parameters.Add("@strLanguageCode", SqlDbType.VarChar, 10); cmdSelect.Parameters["@strLanguageCode"].Direction = ParameterDirection.Output;
        cmdSelect.Parameters.Add("@strCultureInfo", SqlDbType.VarChar, 10); cmdSelect.Parameters["@strCultureInfo"].Direction = ParameterDirection.Output;
        cmdSelect.Parameters.Add("@strDirection", SqlDbType.VarChar, 10); cmdSelect.Parameters["@strDirection"].Direction = ParameterDirection.Output;

        this.FetchData(cmdSelect, "tblOrganization");
        strLanguage = cmdSelect.Parameters["@strLanguage"].Value.ToString();
        strLanguageCode = cmdSelect.Parameters["@strLanguageCode"].Value.ToString();
        strCultureInfo = cmdSelect.Parameters["@strLanguageCode"].Value.ToString();
        strDirection = cmdSelect.Parameters["@strDirection"].Value.ToString();

        return (strLanguage.Length > 0);
    }
    #endregion

    #region public void AddColumn(ref DataTable dtTable, string strColumnName, string strType, string strValue)
    public void AddColumn(ref DataTable dtTable, string strColumnName, string strType, string strValue)
    {
        string strDataType = strType.Substring(strType.IndexOf(".") + 1);
        DataColumn dcTemp = new DataColumn();
        dcTemp.Caption = strColumnName;
        dcTemp.ColumnName = strColumnName;
        dcTemp.DataType = Type.GetType(strType);
        switch (strDataType.ToUpper())
        {
            case "STRING":
                dcTemp.DefaultValue = strValue;
                break;
            case "INT":
            case "INT16":
            case "INT32":
                dcTemp.DefaultValue = Convert.ToInt32(strValue);
                break;
            default:
                dcTemp.DefaultValue = strValue;
                break;
        }
        dtTable.Columns.Add(dcTemp);
    }
    #endregion

    //************************************************************************************************************
    #region Properties
    public string Remote_IPAddress
    {
        set { _Remote_IPAddress = value; }
    }

    public string Logon_UserName
    {
        set { _logon_UserName = value; }
    }
    public string Remote_UserName
    {
        set { _Remote_UserName = value; }
    }

    public string LanguageCode
    {
        get
        {
            if (_LanguageCode == "")
                _LanguageCode = "en-AU";
            return _LanguageCode;
        }
        set { _LanguageCode = value; }
    }

    public string AppSchema_ID
    {
        set { _AppSchemaID = value; }
    }

    public DataSet dsSchemaFields
    {
        get { return this._dsGlobal; }
    }

    public string ErrorAlias
    {
        set { _ErrAlias = value; }
    }

    public string ErrorDesc
    {
        get { return this._ErrDesc; }
    }

    public DataSet dsErrors
    {
        get { return _dsGlobal; }
    }

    public string UserID
    {
        set { _UserID = value; }
    }

    public string UserPassword
    {
        set { _UserPW = value; }
    }

    public string OrganizationCode
    {
        set { 
            tempOrganizationCode = value.Remove(0, 3);
            tempOrganizationCode = tempOrganizationCode.Remove(tempOrganizationCode.Length - 3);
            //_OrganizationCode = tempOrganizationCode;
        }
        get {return (tempOrganizationCode);}
    }

    public string UserFullName
    {
        get { return _UserFullName; }
    }

    public int User_SNo
    {
        get { return _User_SNo; }
        set { _User_SNo = value; }
    }

    //public int Group_Code
    //{
    //    get {return _User_GourpCode ;}
    //    set {_User_GourpCode = value;}
    //}

    public string CultureInfo
    {
        get
        {
            if (_CultureInfo == "")
                _CultureInfo = "en-au";
            return _CultureInfo;
        }
        set { _CultureInfo = value; }
    }

    public string PageDirection
    {
        get { return _PageDirection; }
    }

    public string DatabasePath
    {
        set
        {
            _DBPath = value;
            _DBPath += @"Database\";
        }
    }

    public string LapbaseConnectionString
    {
        get { return strLapbaseCnnString; }
    }

    //------ System Detail properties
    public int SD_UseRace
    {
        get { return UseRace; }
    }

    public int SD_ComordityVisitMonths
    {
        get { return ComordityVisitMonths; }
    }

    public int SD_ReferenceBMI
    {
        get { return ReferenceBMI; }
    }

    public bool SD_PatInv
    {
        get { return PatInv; }
    }

    public string SD_Field1Name
    {
        get { return Field1Name; }
    }

    public string SD_Field2Name
    {
        get { return Field2Name; }
    }

    public string SD_Field3Name
    {
        get { return Field3Name; }
    }

    public string SD_Field4Name
    {
        get { return Field4Name; }
    }

    public string SD_Field5Name
    {
        get { return Field5Name; }
    }

    public string SD_MField1Name
    {
        get { return MField1Name; }
    }

    public string SD_MField2Name
    {
        get { return MField2Name; }
    }

    public string SD_WOScale
    {
        get { return SD_WOScale; }
    }

    public bool SD_UseIWonBMI
    {
        get { return UseIWonBMI; }
    }

    public bool SD_PatCOM
    {
        get { return PatCOM; }
    }

    public bool SD_FUShowVisits
    {
        get { return FUShowVisits; }
    }

    public bool SD_FUCom
    {
        get { return FUCom; }
    }

    public bool SD_FUinv
    {
        get { return FUinv; }
    }

    public bool SD_Imperial
    {
        get { return Imperial; }
    }

    public double SD_TargetBMI
    {
        get { return TargetBMI; }
    }
    
    public string SD_SubmitData
    {
        get { return SubmitData; }
    }

    //------ Investigation Normals properties

    public string Inv_AlbuminL
    {
        get { return AlbuminL; }
    }

    public string Inv_AlbuminH
    {
        get { return AlbuminH; }
    }
    public string Inv_AlbuminU
    {
        get { return AlbuminU; }
    }

    public string Inv_AlkPhosL
    {
        get { return AlkPhosL; }
    }
    public string Inv_AlkPhosH
    {
        get { return AlkPhosH; }
    }
    public string Inv_AlkPhosU
    {
        get { return AlkPhosU; }
    }

    public string Inv_ALTL
    {
        get { return ALTL; }
    }
    public string Inv_ALTH
    {
        get { return ALTH; }
    }
    public string Inv_ALTU
    {
        get { return ALTU; }
    }

    public string Inv_Astl
    {
        get { return ASTL; }
    }
    public string Inv_ASTH
    {
        get { return ASTH; }
    }
    public string Inv_ASTU
    {
        get { return ASTU; }
    }

    public string Inv_B12L
    {
        get { return B12L; }
    }
    public string Inv_B12H
    {
        get { return B12H; }
    }
    public string Inv_B12U
    {
        get { return B12U; }
    }

    public string Inv_BicarbonateL
    {
        get { return BicarbonateL; }
    }
    public string Inv_BicarbonateH
    {
        get { return BicarbonateH; }
    }
    public string Inv_BicarbonateU
    {
        get { return BicarbonateU; }
    }

    public string Inv_BilirubinL
    {
        get { return BilirubinL; }
    }
    public string Inv_BilirubinH
    {
        get { return BilirubinH; }
    }
    public string Inv_BilirubinU
    {
        get { return BilirubinU; }
    }

    public string Inv_CalciumL
    {
        get { return CalciumL; }
    }
    public string Inv_CalciumH
    {
        get { return CalciumH; }
    }
    public string Inv_CalciumU
    {
        get { return CalciumU; }
    }

    public string Inv_ChlorideL
    {
        get { return ChlorideL; }
    }
    public string Inv_ChlorideH
    {
        get { return ChlorideH; }
    }
    public string Inv_ChlorideU
    {
        get { return ChlorideU; }
    }

    public string Inv_CPKL
    {
        get { return CPKL; }
    }
    public string Inv_CPKH
    {
        get { return CPKH; }
    }
    public string Inv_CPKU
    {
        get { return CPKU; }
    }

    public string Inv_CreatinineL
    {
        get { return CreatinineL; }
    }
    public string Inv_CreatinineH
    {
        get { return CreatinineH; }
    }
    public string Inv_CreatinineU
    {
        get { return CreatinineU; }
    }

    public string Inv_DiastolicBPL
    {
        get { return DiastolicBPL; }
    }
    public string Inv_DiastolicBPH
    {
        get { return DiastolicBPH; }
    }
    public string Inv_DiastolicBPU
    {
        get { return DiastolicBPU; }
    }

    public string Inv_FBloodGlucoseL
    {
        get { return FBloodGlucoseL; }
    }
    public string Inv_FBloodGlucoseH
    {
        get { return FBloodGlucoseH; }
    }
    public string Inv_FBloodGlucoseU
    {
        get { return FBloodGlucoseU; }
    }

    public string Inv_FerritinL
    {
        get { return FerritinL; }
    }
    public string Inv_FerritinH
    {
        get { return FerritinH; }
    }
    public string Inv_FerritinU
    {
        get { return FerritinU; }
    }

    public string Inv_FolateL
    {
        get { return FolateL; }
    }
    public string Inv_FolateH
    {
        get { return FolateH; }
    }
    public string Inv_FolateU
    {
        get { return FolateU; }
    }

    public string Inv_FSerumInsulinL
    {
        get { return FSerumInsulinL; }
    }
    public string Inv_FSerumInsulinH
    {
        get { return FSerumInsulinH; }
    }
    public string Inv_FSerumInsulinU
    {
        get { return FSerumInsulinU; }
    }

    public string Inv_GGTL
    {
        get { return GGTL; }
    }
    public string Inv_GGTH
    {
        get { return GGTH; }
    }
    public string Inv_GGTU
    {
        get { return GGTU; }
    }

    public string Inv_HBA1CL
    {
        get { return HBA1CL; }
    }
    public string Inv_HBA1CH
    {
        get { return HBA1CH; }
    }
    public string Inv_HBA1CU
    {
        get { return HBA1CU; }
    }

    public string Inv_HDLCholesterolL
    {
        get { return HDLCholesterolL; }
    }
    public string Inv_HDLCholesterolH
    {
        get { return HDLCholesterolH; }
    }
    public string Inv_HDLCholesterolU
    {
        get { return HDLCholesterolU; }
    }

    public string Inv_HemoglobinL
    {
        get { return HemoglobinL; }
    }
    public string Inv_HemoglobinH
    {
        get { return HemoglobinH; }
    }
    public string Inv_HemoglobinU
    {
        get { return HemoglobinU; }
    }

    public string Inv_HomocysteineL
    {
        get { return HomocysteineL; }
    }
    public string Inv_HomocysteineH
    {
        get { return HomocysteineH; }
    }
    public string Inv_HomocysteineU
    {
        get { return HomocysteineU; }
    }

    public string Inv_IBCL
    {
        get { return IBCL; }
    }
    public string Inv_IBCH
    {
        get { return IBCH; }
    }
    public string Inv_IBCU
    {
        get { return IBCU; }
    }

    public string Inv_IronL
    {
        get { return IronL; }
    }
    public string Inv_IronH
    {
        get { return IronH; }
    }
    public string Inv_IronU
    {
        get { return IronU; }
    }

    public string Inv_PhosphateL
    {
        get { return PhosphateL; }
    }
    public string Inv_PhosphateH
    {
        get { return PhosphateH; }
    }
    public string Inv_PhosphateU
    {
        get { return PhosphateU; }
    }

    public string Inv_PlateletsL
    {
        get { return PlateletsL; }
    }
    public string Inv_PlateletsH
    {
        get { return PlateletsH; }
    }
    public string Inv_PlateletsU
    {
        get { return PlateletsU; }
    }

    public string Inv_PotassiumL
    {
        get { return PotassiumL; }
    }
    public string Inv_PotassiumH
    {
        get { return PotassiumH; }
    }
    public string Inv_PotassiumU
    {
        get { return PotassiumU; }
    }

    public string Inv_SodiumL
    {
        get { return SodiumL; }
    }
    public string Inv_SodiumH
    {
        get { return SodiumH; }
    }
    public string Inv_SodiumU
    {
        get { return SodiumU; }
    }

    public string Inv_SystolicBPL
    {
        get { return SystolicBPL; }
    }
    public string Inv_SystolicBPH
    {
        get { return SystolicBPH; }
    }
    public string Inv_SystolicBPU
    {
        get { return SystolicBPU; }
    }

    public string Inv_T4L
    {
        get { return T4L; }
    }
    public string Inv_T4H
    {
        get { return T4H; }
    }
    public string Inv_T4U
    {
        get { return T4U; }
    }

    public string Inv_T3L
    {
        get { return T3L; }
    }
    public string Inv_T3H
    {
        get { return T3H; }
    }
    public string Inv_T3U
    {
        get { return T3U; }
    }

    public string Inv_TotalCholesterolL
    {
        get { return TotalCholesterolL; }
    }
    public string Inv_TotalCholesterolH
    {
        get { return TotalCholesterolH; }
    }
    public string Inv_TotalCholesterolU
    {
        get { return TotalCholesterolU; }
    }

    public string Inv_TproteinL
    {
        get { return TproteinL; }
    }
    public string Inv_TproteinH
    {
        get { return TproteinH; }
    }
    public string Inv_TproteinU
    {
        get { return TproteinU; }
    }

    public string Inv_TransferrinL
    {
        get { return TransferrinL; }
    }
    public string Inv_TransferrinH
    {
        get { return TransferrinH; }
    }
    public string Inv_TransferrinU
    {
        get { return TransferrinU; }
    }

    public string Inv_TriglyceridesL
    {
        get { return TriglyceridesL; }
    }
    public string Inv_TriglyceridesH
    {
        get { return TriglyceridesH; }
    }
    public string Inv_TriglyceridesU
    {
        get { return TriglyceridesU; }
    }

    public string Inv_TSHL
    {
        get { return TSHL; }
    }
    public string Inv_TSHH
    {
        get { return TSHH; }
    }
    public string Inv_TSHU
    {
        get { return TSHU; }
    }

    public string Inv_UreaL
    {
        get { return UreaL; }
    }
    public string Inv_UreaH
    {
        get { return UreaH; }
    }
    public string Inv_UreaU
    {
        get { return UreaU; }
    }

    public string Inv_VitDL
    {
        get { return VitDL; }
    }
    public string Inv_VitDH
    {
        get { return VitDH; }
    }
    public string Inv_VitDU
    {
        get { return VitDU; }
    }

    public string Inv_WCCL
    {
        get { return WCCL; }
    }
    public string Inv_WCCH
    {
        get { return WCCH; }
    }

    //------------------------
    public byte GenderType
    {
        set { Gender = value; }
    }


    public long PatientID
    {
        set { longPatientID = value; }
        get { return longPatientID; }
    }

    public DataSet dsGlobal
    {
        set { _dsGlobal = value; }
        get { return this._dsGlobal; }
    }

    public String VisitWeeksFlag
    {
        set { _VisitWeeksFlag = value; }
        get { return _VisitWeeksFlag; }
    }
    #endregion
}
