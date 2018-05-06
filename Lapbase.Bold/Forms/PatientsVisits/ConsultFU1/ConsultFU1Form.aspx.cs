using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Globalization;
using System.Xml;
using Microsoft.Web.UI;
using Lapbase.Configuration.ConfigurationApplication;



/// <summary>
/// This form is called when a paritcular patient in Patients list is selected
/// </summary>
public partial class Forms_PatientsVisits_ConsultFU1_ConsultFU1Form : Lapbase.Business.BasePage
{
    GlobalClass gClass = new GlobalClass();
    String BoldErrorMsg = "";
    String VisitType = "";
    String strNumberFormat = System.Configuration.ConfigurationManager.AppSettings["NumberFormat"].ToString();


    String[,] FollowUp = new String[,] { { "rbAppointment", "FollowupAppointment" }, { "rbPhone", "FollowupPhone" }, { "rbLetterPatient", "FollowupLetterPatient" }, { "rbLetterPhysician", "FollowupLetterPhysician" }, { "rbTransfer", "FollowupTransfer" }, { "rbRefuseFup", "FollowupRefuse" }, { "rbLostFup", "FollowupLost" } };

    String[,] comorbidityArr = new String[,] {{"cmbHypertension","CVS_Hypertension","bold"},{ "cmbCongestive","CVS_Congestive","bold"},{ "cmbIschemic","CVS_Ischemic","bold"},{ "cmbAngina","CVS_Angina","bold"},
        {"cmbPeripheral","CVS_Peripheral","bold"},{ "cmbLower","CVS_Lower","bold"},{ "cmbDVT","CVS_DVT","bold"},{ "cmbGlucose","MET_Glucose","bold"},{ "cmbLipids","MET_Lipids","bold"},
        {"cmbGout","MET_Gout","bold"},{ "cmbGred","GAS_Gerd","bold"},{ "cmbCholelithiasis","GAS_Cholelithiasis","bold"},{ "cmbLiver","GAS_Liver","bold"},{"cmbBackPain","MUS_BackPain","bold"},
        {"cmbMusculoskeletal","MUS_Musculoskeletal","bold"},{ "cmbFibro","MUS_Fibromyalgia","bold"},{ "cmbPsychosocial","PSY_Impairment","bold"},{ "cmbDepression","PSY_Depression","bold"},
        {"cmbConfirmed","PSY_MentalHealth","bold"},{ "cmbAlcohol","PSY_Alcohol","bold"},{ "cmbTobacco","PSY_Tobacoo","bold"},{ "cmbAbuse","PSY_Abuse","bold"},{ "cmbStressUrinary","GEN_Stress","bold"},
        {"cmbCerebri","GEN_Cerebri","bold"},{ "cmbHernia","GEN_Hernia","bold"},{ "cmbFunctional","GEN_Functional","bold"},{ "cmbSkin","GEN_Skin","bold"},{ "cmbObstructive","PUL_Obstructive","bold"},
        {"cmbObesity","PUL_Obesity","bold"},{ "cmbPulmonary","PUL_PulHypertension","bold"},{ "cmbAsthma","PUL_Asthma","bold"},{ "cmbPolycystic","REPRD_Polycystic","bold"},{ "cmbMenstrual","REPRD_Menstrual","bold"},
        {"cmbPrevPCISurgery","GEN_PrevPCISurgery","bold"},
        {"cmbSmokerACS","ACS_Smoke","acs"},{ "cmbOxygenACS","ACS_Oxy","acs"},{ "cmbEmbolismACS","ACS_Embo","acs"},{ "cmbCopdACS","ACS_Copd","acs"},{ "cmbCpapACS","ACS_Cpap","acs"},{ "cmbShoACS","ACS_Sho","acs"},
        {"cmbGerdACS","ACS_Gerd","acs"},{ "cmbGallstoneACS","ACS_Gal","acs"},{ "cmbMusculoDiseaseACS","ACS_Muscd","acs"},{ "cmbActivityLimitedACS","ACS_Pain","acs"},
        {"cmbDailyMedACS","ACS_Meds","acs"},{ "cmbSurgicalACS","ACS_Surg","acs"},{ "cmbMobilityACS","ACS_Mob","acs"},
        {"cmbUrinaryACS","ACS_Uri","acs"},{ "cmbMyocardinalACS","ACS_Myo","acs"},{ "cmbPrevPCIACS","ACS_Pci","acs"}, 
        {"cmbPrevCardiacACS","ACS_Csurg","acs"},{ "cmbHyperlipidemiaACS","ACS_Lipid","acs"},{ "cmbHypertensionACS","ACS_Hyper","acs"},{ "cmbDVTACS","ACS_Dvt","acs"}, 
        {"cmbVenousACS","ACS_Venous","acs"},{ "cmbHealthStatusACS","ACS_Health","acs"},{ "cmbDiabetesACS","ACS_Diab","acs"},
        {"cmbObesityACS", "ACS_Obese","acs"},
        {"cmbFatACS", "ACS_Fat","acs"},
        {"cmbRenalInsuff", "GEN_RenalInsuff", "both" }, { "cmbRenalFail", "GEN_RenalFail", "both" }, 
        {"cmbSteroid", "GEN_Steroid", "both" }, { "cmbTherapeutic", "GEN_Therapeutic", "both" }
    };

    #region protected void Page_Load(object sender, EventArgs e)
    ///<summary>
    /*
     * 
     */
    /// <summary>
    ///when a particular patient is selected and the page is being loaded, this funcion does
    ///1) setting Culture for page, in order to check some values based on Culture of Selected language such as DATE
    ///2) Setting "Patient ID" of selected patient, Height and Weight measurment unit by using Imperial flag data, 
    ///   if Imperial == true, Height and weight measurment are "inch" and "lbs"
    ///   if Imperial == false, Height and weight measurment are "cm" and "kg"
    ///   these tasks are done by "IntialiseFormSetting()" function
    ///3) Load all visits of selected patient , this's done by "LoadAllVisits(false)" function
    ///4) add log record for browing visits data , this's done by "gClass.SaveUserLogFile" function 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "no-cache");
        Response.Expires = -1;


        try
        {
            //Page.Culture = Request.Cookies["CultureInfo"].Value;
            gClass.LanguageCode = base.LanguageCode;//Request.Cookies["LanguageCode"].Value;
            gClass.OrganizationCode = Request.Cookies["OrganizationCode"].Value;
            txtHCurrentDate.Value = DateTime.Now.ToShortDateString();

            //if (gClass.IsUserLogoned(Session.SessionID, Request.Cookies["UserPracticeCode"].Value, Request.Url.Host))
            if (gClass.IsUserLogoned(Session.SessionID, base.UserPracticeCode.ToString(), base.DomainURL))
            {

                //split feature
                string Feature = Request.Cookies["Feature"].Value;
                string[] Features = Feature.Split(new string[] { "**" }, StringSplitOptions.None);

                string ShowDownloadExcelGraph = Features[2];

                if (ShowDownloadExcelGraph == "False" || ShowDownloadExcelGraph == "")
                {
                    btnDownloadExcel.Style["display"] = "none";
                }

                if (! IsPostBack)
                {
                    txtHApplicationURL.Value = Request.Url.Scheme + "://" + Request.Url.Host + Request.ApplicationPath + "/";
                    txtHCulture.Value = base.CultureInfo; //Request.Cookies["CultureInfo"].Value;
                    IntialiseFormSetting();

                    LoadPatientData();
                    LoadOperationList();
                    LoadAllVisits("false");
                    LoadPatientEMR();
                    LoadLogoData("");

                    //gClass.SaveUserLogFile( base.UserPracticeCode// Request.Cookies["UserPracticeCode"].Value, 
                    //                        , base.UserName // Request.Cookies["Logon_UserName"].Value, 
                    //                        , base.DomainURL //Request.Url.Host, 
                    //                        , "Patient Visit Form", 2, "Browse", "PatientCode",
                    //                        txtHPatientID.Value);


                    txtHPermissionLevel.Value = Request.Cookies["PermissionLevel"].Value;
                    txtHDataClamp.Value = Request.Cookies["DataClamp"].Value;


                    if (Request.Cookies["SubmitData"].Value.IndexOf("submitbold") >= 0 || Request.Cookies["PermissionLevel"].Value == "1o" || Request.Cookies["PermissionLevel"].Value == "2t" || Request.Cookies["PermissionLevel"].Value == "3f" || Request.Cookies["PermissionLevel"].Value == "4s")
                        divBtnDelete.Style["display"] = "none";

                    if (Request.Cookies["PermissionLevel"].Value == "1o")
                    {
                        divBtnCancel.Style["display"] = "none";
                        divBtnCancelComment.Style["display"] = "none";
                        divBtnAdd.Style["display"] = "none";
                        btnAddFile.Style["display"] = "none";
                        lblPhoto_PN.Style["display"] = "none";
                        btnSaveFollowup.Style["display"] = "none";                        
                    }

                    //comorbidityDefault.Style["display"] = "none";
                    comorbidityBold.Style["display"] = "none";
                    comorbidityACS.Style["display"] = "none";

                    if (Request.Cookies["SubmitData"].Value.IndexOf("bold") >= 0)
                        comorbidityBold.Style["display"] = "block";

                    if (Request.Cookies["SubmitData"].Value.IndexOf("acs") >= 0)
                        comorbidityACS.Style["display"] = "block";
                }
                RemoveFirstItemFromDropDownList();
                
                //check for super bill function availability
                string superBillFunc = Request.Cookies["SuperBill"].Value.Equals("True") ? "1" : "0";
                txtHSuperBill.Value = superBillFunc;
                if (superBillFunc == "0")
                {                    
                    rowlblChiefComplaint.Style["display"] = "none";
                    rowcmbChiefComplaint.Style["display"] = "none";
                    rowlblMedicalProvider.Style["display"] = "none";
                    rowcmbMedicalProvider.Style["display"] = "none";
                    
                    rowlblLapbandAdjustment.Style["display"] = "none";

                    divbtnSuperBill.Style["display"] = "none";

                    rowchkSuperBill.Style["display"] = "none";
                }
                lblSignBy.Text = "Signed By: " + Request.Cookies["Logon_UserName"].Value;
            }
            else gClass.ReturnToLoginPage(Request.Url.Host, Request.Cookies["LanguageCode"].Value, Response);
        }
        catch (Exception err)
        {
            string strLanguageCode;
            try
            {
                strLanguageCode = Request.Cookies["LanguageCode"].Value;
                gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Visit Form", "Loading Patient List (Page_Load function)", err.ToString());
            }
            catch { strLanguageCode = "en-US"; }
            gClass.ReturnToLoginPage(Request.Url.Host, strLanguageCode, Response);
        }
        return;
    }
    #endregion 

    #region private void LoadPatientEMR()
    private void LoadPatientEMR()
    {
        try
        {
            SqlCommand cmdSelect = new SqlCommand();
            DataSet dsPatient = new DataSet();

            gClass.MakeStoreProcedureName(ref cmdSelect, "sp_PatientEMR_LoadData", true);
            cmdSelect.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
            cmdSelect.Parameters.Add("@PatientID", System.Data.SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
            dsPatient = gClass.FetchData(cmdSelect, "tblPatient");

            if ((dsPatient.Tables.Count > 0) && (dsPatient.Tables[0].Rows.Count > 0))
            {
                txtHInitRegistrySleepApnea.Value = dsPatient.Tables[0].Rows[0]["Registry_SleepApnea"].ToString().ToUpper();
                txtHInitRegistryGerd.Value = dsPatient.Tables[0].Rows[0]["Registry_Gerd"].ToString().ToUpper();
                txtHInitRegistryHyperlipidemia.Value = dsPatient.Tables[0].Rows[0]["Registry_Hyperlipidemia"].ToString().ToUpper();
                txtHInitRegistryDiabetes.Value = dsPatient.Tables[0].Rows[0]["Registry_Diabetes"].ToString().ToUpper();
                txtHInitRegistryHypertension.Value = dsPatient.Tables[0].Rows[0]["Registry_Hypertension"].ToString().ToUpper();
                
            }
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Visit Patient EMR", "Loading Visit Patient EMR - LoadPatientEMR function", err.ToString());
        }
    }
    #endregion

    #region private void IntialiseFormSetting()
    /*
     * this function is to initilise some fields with default values,
     */
    private void IntialiseFormSetting()
    {
        if (Request.QueryString.Count > 0)
            switch (Request.QueryString["QSN"].ToString()) //QSN
            {
                case "PID":
                    Response.Cookies.Set(new HttpCookie("PatientID", Request.QueryString["QSV"].ToString()));
                    Request.Cookies["PatientID"].Value = Request.QueryString["QSV"].ToString();
                    txtHPatientID.Value = Request.QueryString["QSV"].ToString();
                    break;
            }
        else
        {
            Response.Cookies.Set(Request.Cookies["PatientID"]);
            txtHPatientID.Value = Request.Cookies["PatientID"].Value;
        }

        txtUseImperial.Value = Request.Cookies["Imperial"].Value.Equals("True") ? "1" : "0";
        lblWeightUnit_PN.Text = Request.Cookies["Imperial"].Value.Equals("True") ? "lbs" : "kg";
        lblHeightUnit_PN.InnerText = Request.Cookies["Imperial"].Value.Equals("True") ? "inch" : "cm";
        lblWeightLoss_TH.Text = Request.Cookies["VisitWeeksFlag"].Value.Equals("0") ? "Total Loss" : "Total Loss";


        lblNeckUnit_PN.InnerText = Request.Cookies["Imperial"].Value.Equals("True") ? "inch" : "cm";
        lblWaistUnit_PN.InnerText = Request.Cookies["Imperial"].Value.Equals("True") ? "inch" : "cm";
        lblHipUnit_PN.InnerText = Request.Cookies["Imperial"].Value.Equals("True") ? "inch" : "cm";
        
        txtDateSeen_PN.Attributes["onblur"] = "javascript:DateSeen_onBlur()";
        bodyConsultFU1.Attributes.Add("onload", "javascript:InitializePage();");
        bodyConsultFU1.Style.Add("Direction", Request.Cookies["Direction"].Value);
        imgPatientNotes.Attributes.Add("onclick", "javascript:ShowPatientNotesDiv();");

        //System.Globalization.CultureInfo myCI = new System.Globalization.CultureInfo(Request.Cookies["CultureInfo"].Value, false);
        System.Globalization.CultureInfo myCI = new System.Globalization.CultureInfo(base.CultureInfo, false);

        txtHDateCreated.Value = Lapbase.Data.Common.TruncateDate(DateTime.Now.ToShortDateString(), Request.Cookies["CultureInfo"].Value);


        lblDateFormat.Text = myCI.DateTimeFormat.ShortDatePattern.ToLower();
        txtDateSeen_PN.toolTip = myCI.DateTimeFormat.ShortDatePattern;
        txtFollowupDate.toolTip = myCI.DateTimeFormat.ShortDatePattern;

        btnLoadVisitData.Style.Add("display", "none");
        return;
    }
    #endregion 

    #region private void LoadAllVisits(String CallBySave)
    /*
     * this function is to load all visits (normal and comorbidity) and called in 2 cases, 
     * if the function is called when the page is being loaded,CallBySave is false, 
     * else if CallBySave is true, it means that after saving visit data, the list of visits should be reloaded.
     * also if CallBySave is true, there are some client-side function which should be run after saving and loading visit data
     */
    private void LoadAllVisits(String CallBySave)
    {
        string strScript ="";

        if (CallBySave == "superbill")
            strScript = "javascript:btnPrintSuperBill_onclick();";
        else {
            string msg = "";

            if (CallBySave == "save")
                msg = "Saving";
            else if (CallBySave == "delete")
                msg = "Deleting";

            strScript = "javascript:CheckAllVisitDocument();" + ((CallBySave == "save" || CallBySave == "delete" || CallBySave == "superbill") ? "InitializePage();ShowDivMessage('" + msg + " data is done successfully...', true);" : "") + ((CallBySave == "superbill") ? "btnPrintSuperBill_onclick();" : "");
        }
        LoadGeneralNotes();
        LoadProgressNoteList(true);
        LoadProgressNoteList(false);
        btnLoadRefDoctorData();
        LoadFollowupCheck();

        try
        {
            ScriptManager.RegisterStartupScript(updatePanelList, btnAddVisit.GetType(), "key", strScript, true);

            gClass.SaveActionLog(gClass.OrganizationCode,
                     Request.Cookies["UserPracticeCode"].Value,
                     Request.Url.Host,
                     System.Configuration.ConfigurationManager.AppSettings["VisitPage"].ToString(),
                     System.Configuration.ConfigurationManager.AppSettings["LogRead"].ToString(),
                     "Load " + System.Configuration.ConfigurationManager.AppSettings["VisitPage"].ToString() + " List",
                     Request.Cookies["PatientID"].Value,
                     "");
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Visit Form", "LoadAllVisits function - ScriptManager.RegisterStartupScript", err.ToString());
        }
    }
    #endregion 

    #region protected void btnAddVisit_onserverclick(object sender, EventArgs e)
    /*
     * this function is to save visit data (normal and comorbidity visit) and after saving data, the visits list is reloaded
     */
    protected void btnAddVisit_onserverclick(object sender, EventArgs e)
    {
        Boolean validBold = true;

        if (txtHSaveFlag.Value == "1")
        {
            if (chkCommentOnly.Checked == false)
            {
                //BOLD validation
                //only check if BOLD mode is on
                if (Request.Cookies["SubmitData"].Value.IndexOf("submitbold") >= 0)
                {
                    validBold = ValidateBOLDPatientVisit();
                }

                if (validBold == true)
                {
                    ProgressNote_SaveProc();
                    BoldComorbidity_SaveProc();

                    if (Request.Cookies["SubmitData"].Value.IndexOf("submitbold") >= 0)
                    {
                        SubmitBOLDPatientVisit(Convert.ToInt32(txtHTempConsultID.Value));
                    }
                    LoadAllVisits("save");
                }
            }
            else
            {
                ProgressNote_SaveCommentProc();
                LoadAllVisits("save");
            }
        }
    }
    #endregion

    #region protected void btnAddFollowUp_onserverclick(object sender, EventArgs e)
    protected void btnAddFollowUp_onserverclick(object sender, EventArgs e)
    {
        Int16 IsDoneSaveFlag = 0;
        String strScript = "";
        DateTime dtResult = DateTime.Now;
        SqlCommand cmdSave = new SqlCommand();

        gClass.MakeStoreProcedureName(ref cmdSave, "sp_FollowUpCheck_SaveData", true);
        cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSave.Parameters.Add("@PatientId", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);

        cmdSave.Parameters.Add("@FollowupAppointment", SqlDbType.VarChar, 20).Value = rbAppointmentY.Checked ? rbAppointmentY.Value : rbAppointmentN.Value;
        cmdSave.Parameters.Add("@FollowupPhone", SqlDbType.VarChar, 20).Value = rbPhoneT.Checked ? rbPhoneT.Value : (rbPhoneO.Checked ? rbPhoneO.Value : (rbPhoneNVR.Checked ? rbPhoneNVR.Value : rbPhoneNVR.Value));
        cmdSave.Parameters.Add("@FollowupLetterPatient", SqlDbType.VarChar, 20).Value = rbLetterPatientT.Checked ? rbLetterPatientT.Value : (rbLetterPatientO.Checked ? rbLetterPatientO.Value : (rbLetterPatientNVR.Checked ? rbLetterPatientNVR.Value : rbLetterPatientNVR.Value));
        cmdSave.Parameters.Add("@FollowupLetterPhysician", SqlDbType.VarChar, 20).Value = rbLetterPhysicianT.Checked ? rbLetterPhysicianT.Value : (rbLetterPhysicianO.Checked ? rbLetterPhysicianO.Value : (rbLetterPhysicianNVR.Checked ? rbLetterPhysicianNVR.Value : rbLetterPhysicianNVR.Value));
        cmdSave.Parameters.Add("@FollowupTransfer", SqlDbType.VarChar, 20).Value = rbTransferY.Checked ? rbTransferY.Value : rbTransferN.Value;
        cmdSave.Parameters.Add("@FollowupRefuse", SqlDbType.VarChar, 20).Value = rbRefuseFupY.Checked ? rbRefuseFupY.Value : rbRefuseFupN.Value;
        cmdSave.Parameters.Add("@FollowupLost", SqlDbType.VarChar, 20).Value = rbLostFupY.Checked ? rbLostFupY.Value : rbLostFupN.Value;   
        cmdSave.Parameters.Add("@FollowupTransferName", SqlDbType.VarChar, 1024).Value = txtLetterPhysician.Value.Trim();
        cmdSave.Parameters.Add("@FollowupDoNotContact", SqlDbType.Bit).Value = chkDoNotContact.Checked;
        cmdSave.Parameters.Add("@FollowupDate", SqlDbType.DateTime);
        if (DateTime.TryParse(txtFollowupDate.Text.Trim(), out dtResult))
            cmdSave.Parameters["@FollowupDate"].Value = dtResult;
        else cmdSave.Parameters["@FollowupDate"].Value = DBNull.Value;

        try
        {
            gClass.ExecuteDMLCommand(cmdSave);
            gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value, Request.Cookies["Logon_UserName"].Value, Request.Url.Host,
                                            "Patient Visit Form", 2, "Save followup check", "PatientID", Request.Cookies["PatientID"].Value);

            gClass.SaveActionLog(gClass.OrganizationCode,
                                 Request.Cookies["UserPracticeCode"].Value,
                                 Request.Url.Host,
                                 System.Configuration.ConfigurationManager.AppSettings["VisitPage"].ToString(),
                                 System.Configuration.ConfigurationManager.AppSettings["LogUpdate"].ToString(),
                                 "Save " + System.Configuration.ConfigurationManager.AppSettings["VisitFollowUp"].ToString(),
                                 Request.Cookies["PatientID"].Value,
                                 "");

            IsDoneSaveFlag = 1;
        }
        catch (Exception err)
        {
            ShowDivErrorMessage("Error in data saving...");
            gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                        Context.Request.Cookies["Logon_UserName"].Value, "Patient ID : " + Context.Request.Cookies["PatientID"].Value, "Save followup check", err.ToString());
            IsDoneSaveFlag = 0;
        }

        cmdSave.Dispose();

        switch (IsDoneSaveFlag)
        {
            case 1: // Saving data procedure is done sucessfully
                strScript = "  ShowDivMessage('The information has been saved...', true);";
                strScript += "SetEvents();";
                break;
            case 0: // Saving data procedure is done sucessfully
                strScript = "  ShowDivMessage('Error in save data ...', true);";
                strScript += "SetEvents();";
                break;
        }
        ScriptManager.RegisterStartupScript(UpdateFollowup, UpdateFollowup.GetType(), Guid.NewGuid().ToString(), strScript, true);
    }
    #endregion

    #region private void LoadFollowupCheck()
    /// <summary>
    /// This function is to load Followup check
    /// </summary>
    private void LoadFollowupCheck()
    {
        HtmlInputRadioButton tempControlRadio = new HtmlInputRadioButton();
        String tempRadioField = "";

        SqlCommand cmdSelect = new SqlCommand();
        DataSet dsFollowup;

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_FollowUpCheck_LoadData", true);
        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt64(gClass.OrganizationCode);
        cmdSelect.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt64(Request.Cookies["PatientID"].Value);
        dsFollowup = gClass.FetchData(cmdSelect, "tblFollowupCheck");

        try
        {
            if (dsFollowup.Tables.Count > 0 && (dsFollowup.Tables[0].Rows.Count > 0))
            {
                txtLetterPhysician.Value = dsFollowup.Tables[0].Rows[0]["FollowupTransferName"].ToString().Trim();

                txtFollowupDate.Text = gClass.TruncateDate(dsFollowup.Tables[0].Rows[0]["FollowupDate"].ToString().Trim(), Request.Cookies["CultureInfo"].Value, 1);
                chkDoNotContact.Checked = dsFollowup.Tables[0].Rows[0]["FollowupDoNotContact"].ToString().Equals("True");

                for (int Xh = 0; Xh < FollowUp.Length / 2; Xh++)
                {
                    tempRadioField = FollowUp[Xh, 1];

                    if (dsFollowup.Tables[0].Rows[0][tempRadioField].ToString().Trim() != "")
                    {
                        tempControlRadio = (HtmlInputRadioButton)FindControlRecursive(this.Page, FollowUp[Xh, 0] + UppercaseFirst(dsFollowup.Tables[0].Rows[0][tempRadioField].ToString()));
                        tempControlRadio.Checked = true;
                    }
                }
            }
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Visit Form - FollowUp Check", "LoadFollowupCheck function", err.ToString());
        }

        dsFollowup.Dispose();
        cmdSelect.Dispose();
    }
    #endregion

    #region protected void btnSaveReportNotes_OnClick(object sender, EventArgs e)
    /*
     * this function is called when we click on preview and print on reports
     */
    protected void btnSaveReportNotes_OnClick(object sender, EventArgs e)
    {
        SqlCommand cmdSave = new SqlCommand();

        gClass.MakeStoreProcedureName(ref cmdSave, "sp_FollowUpNotes_SaveData", true);
        cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSave.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
        String followUpNotes = txtFollowUpNotes_PN.Text;
        followUpNotes = followUpNotes.Replace("\n", "<br/>");
        cmdSave.Parameters.Add("@Notes", SqlDbType.VarChar, 2048).Value = followUpNotes;

        try
        {
            gClass.ExecuteDMLCommand(cmdSave);
            gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value, Request.Cookies["Logon_UserName"].Value, Request.Url.Host,
                "Patient Visit Report", 2, "FollowUp Report", "UserPracticeCode", Request.Cookies["UserPracticeCode"].Value);

            gClass.SaveActionLog(gClass.OrganizationCode,
                                 Request.Cookies["UserPracticeCode"].Value,
                                 Request.Url.Host,
                                 System.Configuration.ConfigurationManager.AppSettings["VisitPage"].ToString(),
                                 System.Configuration.ConfigurationManager.AppSettings["LogUpdate"].ToString(),
                                 "Save " + System.Configuration.ConfigurationManager.AppSettings["VisitFollowUpNotes"].ToString(),
                                 Request.Cookies["PatientID"].Value,
                                 "");

        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Visit Form - FollowUp Report", "btnPreview_onserverclick function", err.ToString());
        }
    }
    #endregion

    #region protected void btnPrintSuperBill_onserverclick(object sender, EventArgs e)
    /*
     * this function is to save visit data (normal and comorbidity visit) and after saving data, proceed to report page
     */
    protected void btnPrintSuperBill_onserverclick(object sender, EventArgs e)
    {
        Boolean validBold = true;
        String createdDate = txtHDateCreated.Value;
        String currentDate = txtHCurrentDate.Value;

        //if dataclamp is activated, permission lvl 2 or 3
        //check for created date for this patient
        Boolean allowSave = true;

        if (txtHDataClamp.Value.ToLower() == "true" && (txtHPermissionLevel.Value == "2t" || txtHPermissionLevel.Value == "3f"))
        {
            if (createdDate != "" && createdDate != currentDate)
                allowSave = false;
        }

        //if got permission to save
        if (Request.Cookies["PermissionLevel"].Value != "1o" && allowSave == true)
        {
            if (txtHSaveFlag.Value == "1")
            {
                //BOLD validation
                //only check if BOLD mode is on
                if (Request.Cookies["SubmitData"].Value.IndexOf("submitbold") >= 0)
                {
                    validBold = ValidateBOLDPatientVisit();
                }

                if (validBold == true)
                {
                    ProgressNote_SaveProc();
                    BoldComorbidity_SaveProc();

                    if (Request.Cookies["SubmitData"].Value.IndexOf("submitbold") >= 0)
                    {
                        SubmitBOLDPatientVisit(Convert.ToInt32(txtHConsultID.Value));
                    }
                }
            }
        }
        else
            txtHTempConsultID.Value = txtHConsultID.Value;

        LoadAllVisits("superbill");
    }
    #endregion

    #region private void ProgressNote_SaveCommentProc()
    /// <summary>
    /// this function is to save visit comment data
    /// </summary>
    private void ProgressNote_SaveCommentProc()
    {
        SqlCommand cmdSave = new SqlCommand();
        DateTime dtResult = DateTime.Now;
        DateTime dtTemp;
        Boolean blnInsert = false;

        gClass.MakeStoreProcedureName(ref cmdSave, "sp_ConsultFU1_ProgressNotesComment_SaveData", true);
        cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSave.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
        cmdSave.Parameters.Add("@PatientId", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
        cmdSave.Parameters.Add("@CommentId", SqlDbType.Int).Value = Convert.ToInt32(txtHCommentID.Value);

        cmdSave.Parameters.Add("@DateSeen", SqlDbType.DateTime);
        if (DateTime.TryParse(txtDateSeen_PN.Text.Trim(), out dtResult))
            cmdSave.Parameters["@DateSeen"].Value = dtResult;
        else cmdSave.Parameters["@DateSeen"].Value = DBNull.Value;

        cmdSave.Parameters.Add("@Seenby", SqlDbType.Int).Value = Convert.ToInt32(cmbDoctorList_PN.SelectedValue);
        cmdSave.Parameters.Add("@Notes", SqlDbType.VarChar, 2048).Value = txtNotes_PN.Text;
        cmdSave.Parameters.Add("@DateCreated", SqlDbType.DateTime);
        if (DateTime.TryParse(txtHCurrentDate.Value, out dtTemp))
            cmdSave.Parameters["@DateCreated"].Value = dtTemp;
        else cmdSave.Parameters["@DateCreated"].Value = DBNull.Value;


        blnInsert = Convert.ToInt64(txtHCommentID.Value) == 0 ? true : false;

        if (blnInsert == false)
        {
            //insert log
            //before updating data
            try
            {
                SqlCommand cmdSaveLog = new SqlCommand();
                gClass.MakeStoreProcedureName(ref cmdSaveLog, "sp_ConsultFU1_ProgressNotesCommentLog_SaveData", true);
                cmdSaveLog.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                cmdSaveLog.Parameters.Add("@CommentID", SqlDbType.Int).Value = Convert.ToInt32(txtHCommentID.Value);
                cmdSaveLog.Parameters.Add("@LogUserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
                cmdSaveLog.Parameters.Add("@LogDateTime", SqlDbType.DateTime).Value = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
                gClass.ExecuteDMLCommand(cmdSaveLog);
            }
            catch (Exception err)
            {
                gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host,
                            Request.Cookies["Logon_UserName"].Value, "Comment  PID : " + Request.Cookies["PatientID"].Value, "Data Saving Comment Log", err.ToString());
            }
        }

        try
        {
            txtHCommentID.Value = gClass.SaveCommentData(1, cmdSave, Convert.ToInt32(Request.Cookies["PatientID"].Value), Convert.ToInt32(gClass.OrganizationCode)).ToString();
            gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value, Request.Cookies["Logon_UserName"].Value, Request.Url.Host,
                                            "Patient Visit Form", 2, "Save visit data", "CommentID", txtHCommentID.Value);

            gClass.SaveActionLog(gClass.OrganizationCode,
                                 Request.Cookies["UserPracticeCode"].Value,
                                 Request.Url.Host,
                                 System.Configuration.ConfigurationManager.AppSettings["VisitPage"].ToString(),
                                 ((blnInsert) ? System.Configuration.ConfigurationManager.AppSettings["LogCreate"].ToString() : System.Configuration.ConfigurationManager.AppSettings["LogUpdate"].ToString()),
                                 "Save " + System.Configuration.ConfigurationManager.AppSettings["VisitComment"].ToString(),
                                 Request.Cookies["PatientID"].Value,
                                 txtHCommentID.Value);
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Visit Form", "ProgressNote_SaveCommentProc function", err.ToString());
            ShowDivErrorMessage("Error in data saving...");
        }
        return;
    }
    #endregion

    #region private void ProgressNote_SaveProc()
    /// <summary>
    /// this function is to save visit data (all data in YELLOW section), these data are main part of visit and are common for both normal and comobodity visits.
    /// Also if the visit is the last visit for the current patient, we need to update visit date fields (Last visit date, next visit date) in tblPatients
    /// </summary>
    private void ProgressNote_SaveProc()
    {
        SqlCommand cmdSave = new SqlCommand();
        decimal dcWeight = 0;
        Decimal decResult = 0;
        Double dblResult = 0;
        DateTime dtResult = DateTime.Now;
        DateTime dtTemp;
        Boolean blnInsert = false;

        gClass.MakeStoreProcedureName(ref cmdSave, "sp_ConsultFU1_ProgressNotes_SaveData", true);
        cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSave.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
        cmdSave.Parameters.Add("@PatientId", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
        cmdSave.Parameters.Add("@ConsultId", SqlDbType.Int).Value = Convert.ToInt32(txtHConsultID.Value);

        cmdSave.Parameters.Add("@DateSeen", SqlDbType.DateTime);
        if (DateTime.TryParse(txtDateSeen_PN.Text.Trim(), out dtResult))
            cmdSave.Parameters["@DateSeen"].Value = dtResult;
        else cmdSave.Parameters["@DateSeen"].Value = DBNull.Value;

        cmdSave.Parameters.Add("@Seenby", SqlDbType.Int).Value = Convert.ToInt32(cmbDoctorList_PN.SelectedValue);
        cmdSave.Parameters.Add("@MedicalProvider", SqlDbType.Int).Value = Convert.ToInt32(cmbMedicalProvider_PN.SelectedValue); 

        cmdSave.Parameters.Add("@Weight", SqlDbType.Decimal);
        if (txtHWeight_PN.Value.Trim().Length == 0)
            cmdSave.Parameters["@Weight"].Value = 0;
        else if (Decimal.TryParse(txtHWeight_PN.Value.Trim(), out decResult))
            cmdSave.Parameters["@Weight"].Value = decResult;
        else
            cmdSave.Parameters["@Weight"].Value = 0;

        cmdSave.Parameters.Add("@ReservoirVolume", SqlDbType.VarChar, 5);
        if (Decimal.TryParse(txtHFinalVol.Value.Trim(), out decResult))
        {
            if (decResult > 0)
                txtRV_PN.Text = txtHFinalVol.Value;
        }

        if (txtRV_PN.Text.Trim().Length == 0) cmdSave.Parameters["@ReservoirVolume"].Value = String.Empty;
        else if (Decimal.TryParse(txtRV_PN.Text.Trim(), out decResult) )
            cmdSave.Parameters["@ReservoirVolume"].Value = decResult.ToString();
        else cmdSave.Parameters["@ReservoirVolume"].Value = String.Empty;

        cmdSave.Parameters.Add("@CoMorbidityVisit", SqlDbType.Bit).Value = chkComorbidity.Checked;
        cmdSave.Parameters.Add("@Notes", SqlDbType.VarChar, 2048).Value = txtNotes_PN.Text;
        cmdSave.Parameters.Add("@Adjustment", SqlDbType.VarChar, 2048).Value = txtLapbandAdjustment_PN.Text;

        cmdSave.Parameters.Add("@DateNextVisit", SqlDbType.DateTime);
        if (DateTime.TryParse(txtNextVisitDate_PN.Value.Trim(), out dtResult))
            cmdSave.Parameters["@DateNextVisit"].Value = dtResult;
        else cmdSave.Parameters["@DateNextVisit"].Value = DBNull.Value;

        cmdSave.Parameters.Add("@VideoLocation", SqlDbType.VarChar, 150).Value = DBNull.Value;
        cmdSave.Parameters.Add("@VideoDate", SqlDbType.DateTime).Value = DBNull.Value;

        if (Decimal.TryParse(txtHWeight_PN.Value.Trim(), out decResult))    dcWeight = decResult;
        else dcWeight = 0;

        if (txtUseImperial.Value == "1")
            cmdSave.Parameters.Add("@BMIWeight", SqlDbType.Decimal).Value = dcWeight / (decimal)2.2;
        else
            cmdSave.Parameters.Add("@BMIWeight", SqlDbType.Decimal).Value = dcWeight;

        cmdSave.Parameters.Add("@Height", SqlDbType.Decimal).Value = Decimal.TryParse(txtHHeight_PN.Value, out decResult) ? decResult : 0;
        cmdSave.Parameters.Add("@VisitType", SqlDbType.SmallInt).Value = 1; // default value ???????????- Convert.ToInt16(rblVisitType.SelectedValue);
        cmdSave.Parameters.Add("@BloodPressure", SqlDbType.Decimal).Value = Decimal.TryParse(txtBloodPressure_PN.Text, out decResult) ? decResult : 0; // default value ???????????- Convert.ToInt16(rblVisitType.SelectedValue);


        cmdSave.Parameters.Add("@LetterSent", SqlDbType.Bit).Value = chkLetterSent.Checked;

        cmdSave.Parameters.Add("@ProgressReview", SqlDbType.Bit).Value = chkReview.Checked;

        cmdSave.Parameters.Add("@DateCreated", SqlDbType.DateTime);
        if (DateTime.TryParse(txtHCurrentDate.Value, out dtTemp))
            cmdSave.Parameters["@DateCreated"].Value = dtTemp;
        else cmdSave.Parameters["@DateCreated"].Value = DBNull.Value;

        //parameter for lapband adjustment------------------------------------------------------------------------------------------------
        cmdSave.Parameters.Add("@AdjConsent", SqlDbType.Bit).Value = chkAdjConsent.Checked;
        cmdSave.Parameters.Add("@AdjAntiseptic", SqlDbType.Bit).Value = chkAdjAntiseptic.Checked;
        cmdSave.Parameters.Add("@AdjAnesthesia", SqlDbType.Bit).Value = chkAdjAnesthesia.Checked;
        cmdSave.Parameters.Add("@AdjAnesthesiaVol", SqlDbType.Decimal).Value = Decimal.TryParse(txtAdjAnesthesiaVol_PN.Value.Trim(), out decResult) ? decResult : 0;
        cmdSave.Parameters.Add("@AdjNeedle", SqlDbType.Bit).Value = chkAdjNeedle.Checked;
        cmdSave.Parameters.Add("@AdjVolume", SqlDbType.Bit).Value = chkAdjVolume.Checked;
        cmdSave.Parameters.Add("@AdjInitialVol", SqlDbType.Decimal).Value = Decimal.TryParse(txtInitialVol_PN.Value.Trim(), out decResult) ? decResult : 0;
        cmdSave.Parameters.Add("@AdjAddVol", SqlDbType.Decimal).Value = Decimal.TryParse(txtAddVol_PN.Value.Trim(), out decResult) ? decResult : 0;
        cmdSave.Parameters.Add("@AdjRemoveVol", SqlDbType.Decimal).Value = Decimal.TryParse(txtRemoveVol_PN.Value.Trim(), out decResult) ? decResult : 0;
        cmdSave.Parameters.Add("@AdjTolerate", SqlDbType.Bit).Value = chkAdjTolerate.Checked;
        cmdSave.Parameters.Add("@AdjBarium", SqlDbType.Bit).Value = chkAdjBarium.Checked;
        cmdSave.Parameters.Add("@AdjOmni", SqlDbType.Bit).Value = chkAdjOmni.Checked;
        cmdSave.Parameters.Add("@AdjProtocol", SqlDbType.Bit).Value = chkAdjProtocol.Checked;
        
       
        //parameter for visit review------------------------------------------------------------------------------------------------------
        cmdSave.Parameters.Add("@PR", SqlDbType.Int).Value = Double.TryParse(txtPR_PN.Text.Trim(), out dblResult) ? dblResult : 0;
        cmdSave.Parameters.Add("@RR", SqlDbType.Int).Value = Double.TryParse(txtRR_PN.Text.Trim(), out dblResult) ? dblResult : 0;
        cmdSave.Parameters.Add("@BP1", SqlDbType.Decimal).Value = Decimal.TryParse(txtBP1_PN.Text.Trim(), out decResult) ? decResult : 0;
        cmdSave.Parameters.Add("@BP2", SqlDbType.Decimal).Value = Decimal.TryParse(txtBP2_PN.Text.Trim(), out decResult) ? decResult : 0;
        cmdSave.Parameters.Add("@Neck", SqlDbType.Decimal).Value = Decimal.TryParse(txtHNeck_PN.Value.Trim(), out decResult) ? decResult : 0;
        cmdSave.Parameters.Add("@Waist", SqlDbType.Decimal).Value = Decimal.TryParse(txtHWaist_PN.Value.Trim(), out decResult) ? decResult : 0;
        cmdSave.Parameters.Add("@Hip", SqlDbType.Decimal).Value = Decimal.TryParse(txtHHip_PN.Value.Trim(), out decResult) ? decResult : 0;

        cmdSave.Parameters.Add("@GeneralReview", SqlDbType.VarChar, 512).Value = txtDetGeneral_PN.Value.Trim();
        cmdSave.Parameters.Add("@CardioReview", SqlDbType.VarChar, 512).Value = txtDetCardio_PN.Value.Trim();
        cmdSave.Parameters.Add("@RespReview", SqlDbType.VarChar, 512).Value = txtDetResp_PN.Value.Trim();
        cmdSave.Parameters.Add("@GastroReview", SqlDbType.VarChar, 512).Value = txtDetGastro_PN.Value.Trim();
        cmdSave.Parameters.Add("@GenitoReview", SqlDbType.VarChar, 512).Value = txtDetGenito_PN.Value.Trim();
        cmdSave.Parameters.Add("@ExtrReview", SqlDbType.VarChar, 512).Value = txtDetExtr_PN.Value.Trim();
        cmdSave.Parameters.Add("@NeuroReview", SqlDbType.VarChar, 512).Value = txtDetNeuro_PN.Value.Trim();
        cmdSave.Parameters.Add("@MusculoReview", SqlDbType.VarChar, 512).Value = txtDetMusculo_PN.Value.Trim();
        cmdSave.Parameters.Add("@SkinReview", SqlDbType.VarChar, 512).Value = txtDetSkin_PN.Value.Trim();
        cmdSave.Parameters.Add("@PsychReview", SqlDbType.VarChar, 512).Value = txtDetPsych_PN.Value.Trim();
        cmdSave.Parameters.Add("@EndoReview", SqlDbType.VarChar, 512).Value = txtDetEndo_PN.Value.Trim();
        cmdSave.Parameters.Add("@HemaReview", SqlDbType.VarChar, 512).Value = txtDetHema_PN.Value.Trim();
        cmdSave.Parameters.Add("@ENTReview", SqlDbType.VarChar, 512).Value = txtDetENT_PN.Value.Trim();
        cmdSave.Parameters.Add("@EyesReview", SqlDbType.VarChar, 512).Value = txtDetEyes_PN.Value.Trim();
        cmdSave.Parameters.Add("@PFSHReview", SqlDbType.VarChar, 512).Value = txtDetPFSH_PN.Value.Trim();
        cmdSave.Parameters.Add("@MedsReview", SqlDbType.VarChar, 512).Value = txtDetMeds_PN.Value.Trim();


        cmdSave.Parameters.Add("@Satiety", SqlDbType.Int).Value = Double.TryParse(txtHSatiety_PN.Value.Trim(), out dblResult) ? dblResult : 0;

        cmdSave.Parameters.Add("@ChiefComplaint", SqlDbType.VarChar, 50).Value = cmbChiefComplaint.SelectedValue.ToString();
        cmdSave.Parameters.Add("@SupportGroup", SqlDbType.VarChar, 50).Value = cmbSupportGroup.SelectedValue.ToString().Trim();

        cmdSave.Parameters.Add("@RegistryReview", SqlDbType.Bit).Value = chkRegistryReview.Checked;
        cmdSave.Parameters.Add("@RegistrySleepApnea", SqlDbType.VarChar, 50).Value = chkRegistryReview.Checked?cmbRegistrySleepApnea.SelectedValue.ToString().Trim():"";
        cmdSave.Parameters.Add("@RegistryGerd", SqlDbType.VarChar, 50).Value = chkRegistryReview.Checked?cmbRegistryGerd.SelectedValue.ToString().Trim():"";
        cmdSave.Parameters.Add("@RegistryHyperlipidemia", SqlDbType.VarChar, 50).Value = chkRegistryReview.Checked?cmbRegistryHyperlipidemia.SelectedValue.ToString().Trim():"";
        cmdSave.Parameters.Add("@RegistryDiabetes", SqlDbType.VarChar, 50).Value = chkRegistryReview.Checked ? cmbRegistryDiabetes.SelectedValue.ToString().Trim() : "";
        cmdSave.Parameters.Add("@RegistryHypertension", SqlDbType.VarChar, 50).Value = chkRegistryReview.Checked ? cmbRegistryHypertension.SelectedValue.ToString().Trim() : "";

        cmdSave.Parameters.Add("@RegistryDiabetesDetail", SqlDbType.Bit).Value = rbRegistryDiabetesY.Checked;
        cmdSave.Parameters.Add("@RegistryTreatmentDiet", SqlDbType.Bit).Value = rbRegistryDiabetesY.Checked ? chkRegistryDiet.Checked : false;
        cmdSave.Parameters.Add("@RegistryTreatmentOral", SqlDbType.Bit).Value = rbRegistryDiabetesY.Checked ? chkRegistryOral.Checked : false;
        cmdSave.Parameters.Add("@RegistryTreatmentInsulin", SqlDbType.Bit).Value = rbRegistryDiabetesY.Checked ? chkRegistryInsulin.Checked : false;
        cmdSave.Parameters.Add("@RegistryTreatmentOther", SqlDbType.Bit).Value = rbRegistryDiabetesY.Checked ? chkRegistryOther.Checked : false;
        cmdSave.Parameters.Add("@RegistryTreatmentCombination", SqlDbType.Bit).Value = rbRegistryDiabetesY.Checked ? chkRegistryCombination.Checked : false;
        cmdSave.Parameters.Add("@RegistryReoperation", SqlDbType.Bit).Value = rbRegistryReoperationY.Checked;
        cmdSave.Parameters.Add("@RegistryReoperationNote", SqlDbType.VarChar, 512).Value = txtRegistryReoperationReason.Text;
        
        cmdSave.Parameters.Add("@RegistrySEDetail", SqlDbType.Bit).Value = rbRegistrySEY.Checked;
        cmdSave.Parameters.Add("@RegistrySEList", SqlDbType.VarChar, 50).Value = cmbSEList.SelectedValue.ToString().Trim();
        cmdSave.Parameters.Add("@RegistrySENote", SqlDbType.VarChar, 512).Value = txtRegistrySEReason.Text;
        
        //parameter for visit review------------------------------------------------------------------------------------------------------

        blnInsert = Convert.ToInt64(txtHConsultID.Value) == 0 ? true : false;

        if (blnInsert == false)
        {
            //insert log
            //before updating data
            try
            {
                SqlCommand cmdSaveLog = new SqlCommand();
                gClass.MakeStoreProcedureName(ref cmdSaveLog, "sp_ConsultFU1_ProgressNotesLog_SaveData", true);
                cmdSaveLog.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                cmdSaveLog.Parameters.Add("@ConsultID", SqlDbType.Int).Value = Convert.ToInt32(txtHConsultID.Value);
                cmdSaveLog.Parameters.Add("@LogUserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
                cmdSaveLog.Parameters.Add("@LogDateTime", SqlDbType.DateTime).Value = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
                gClass.ExecuteDMLCommand(cmdSaveLog);
            }
            catch (Exception err)
            {
                gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host,
                            Request.Cookies["Logon_UserName"].Value, "Visit  PID : " + Request.Cookies["PatientID"].Value, "Data Saving Visit Log", err.ToString());
            }
        }
          
        try
        {
            if (txtHConsultID.Value == "0")
            {
                txtHConsultID.Value = gClass.SaveVisitData(1, cmdSave, Convert.ToInt32(Request.Cookies["PatientID"].Value)).ToString();
                Response.SetCookie(new HttpCookie("ConsultID", txtHConsultID.Value)); // IMPORTANT, WE USE THIS IN UPLOADING PAGE --- DO NOT REMOVE
                txtHTempConsultID.Value = txtHConsultID.Value;
                txtHConsultID.Value = "0";
            }
            else
            {
                gClass.SaveVisitData(2, cmdSave, Convert.ToInt32(Request.Cookies["PatientID"].Value));
                txtHTempConsultID.Value = txtHConsultID.Value;
            }

            gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value, Request.Cookies["Logon_UserName"].Value, Request.Url.Host,
                                            "Patient Visit Form", 2, "Save visit data", "ConsultID", txtHConsultID.Value);

            gClass.SaveActionLog(gClass.OrganizationCode,
                     Request.Cookies["UserPracticeCode"].Value,
                     Request.Url.Host,
                     System.Configuration.ConfigurationManager.AppSettings["VisitPage"].ToString(),
                     ((blnInsert) ? System.Configuration.ConfigurationManager.AppSettings["LogCreate"].ToString() : System.Configuration.ConfigurationManager.AppSettings["LogUpdate"].ToString()),
                     "Save " + System.Configuration.ConfigurationManager.AppSettings["VisitVisit"].ToString(),
                     Request.Cookies["PatientID"].Value,
                     txtHTempConsultID.Value);

            //if registry review marked, add into txtHRegistryDone
            if (chkRegistryReview.Checked)
            {
                String strScript = "AddRegistryDone(monthGroup);";
                ScriptManager.RegisterStartupScript(btnCalculateWeightOtherData, btnCalculateWeightOtherData.GetType(), Guid.NewGuid().ToString(), strScript, true);

            }
            UpdateVisitDatesInPatientTable();
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Visit Form", "ProgressNote_SaveProc function", err.ToString());
            ShowDivErrorMessage("Error in data saving...");
        }
        return;
    }
    #endregion 

    #region private void UpdateVisitDatesInPatientTable()
    /*
     * this function is to update visit
     */
    private void UpdateVisitDatesInPatientTable()
    {
        SqlCommand cmdUpdate = new SqlCommand();

        //1) first we check that whether the visit date is the last visit date or not
        gClass.MakeStoreProcedureName(ref cmdUpdate, "sp_ConsultFU1_ProgressNotes_IsTheLastVisitDate", true);

        cmdUpdate.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdUpdate.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
        cmdUpdate.Parameters.Add("@PatientId", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
        cmdUpdate.Parameters.Add("@VisitDate", SqlDbType.DateTime);

        try { cmdUpdate.Parameters["@VisitDate"].Value = Convert.ToDateTime(txtDateSeen_PN.Text.Trim()); }
        catch { cmdUpdate.Parameters["@VisitDate"].Value = DBNull.Value; }
        DataSet dsTemp = gClass.FetchData(cmdUpdate, "tblVisit");
        Boolean flag  = (dsTemp.Tables.Count > 0) && (dsTemp.Tables[0].Rows.Count > 0);

        if (!flag) // false means there is no visit date after the current visit date
        {
            cmdUpdate.Parameters.Clear();
            gClass.MakeStoreProcedureName(ref cmdUpdate, "sp_ConsultFU1_ProgressNotes_UpdateVisitDate", true);
            cmdUpdate.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
            cmdUpdate.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
            cmdUpdate.Parameters.Add("@PatientId", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
            cmdUpdate.Parameters.Add("@DateLastVisit", SqlDbType.DateTime);
            cmdUpdate.Parameters.Add("@DateNextVisit", SqlDbType.DateTime);
            try { cmdUpdate.Parameters["@DateLastVisit"].Value = Convert.ToDateTime(txtDateSeen_PN.Text.Trim()); }
            catch { cmdUpdate.Parameters["@DateLastVisit"].Value = DBNull.Value; }
            try { cmdUpdate.Parameters["@DateNextVisit"].Value = Convert.ToDateTime(txtNextVisitDate_PN.Value.Trim()); }
            catch { cmdUpdate.Parameters["@DateNextVisit"].Value = DBNull.Value; }

            gClass.ExecuteDMLCommand(cmdUpdate);
            gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value, Request.Cookies["Logon_UserName"].Value, Request.Url.Host,
                                "Patient Visit Form", 2, "Update visit dates", "PatientID", Request.Cookies["PatientID"].Value);

            gClass.SaveActionLog(gClass.OrganizationCode,
                                 Request.Cookies["UserPracticeCode"].Value,
                                 Request.Url.Host,
                                 System.Configuration.ConfigurationManager.AppSettings["VisitPage"].ToString(),
                                 System.Configuration.ConfigurationManager.AppSettings["LogUpdate"].ToString(),
                                 "Save " + System.Configuration.ConfigurationManager.AppSettings["VisitDate"].ToString(),
                                 Request.Cookies["PatientID"].Value,
                                 "");

            //update pwd weight
            Decimal decResult = 0;
            SqlCommand cmdUpdatePWD = new SqlCommand();

            //sql to update current weight on pwd
            gClass.MakeStoreProcedureName(ref cmdUpdatePWD, "sp_ConsultFU1_PatientWeightData_UpdatePatientWeight", true);

            cmdUpdatePWD.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
            cmdUpdatePWD.Parameters.Add("@PatientId", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
            cmdUpdatePWD.Parameters.Add("@Weight", SqlDbType.Decimal);
            if (txtHWeight_PN.Value.Trim().Length == 0)
                cmdUpdatePWD.Parameters["@Weight"].Value = 0;
            else if (Decimal.TryParse(txtHWeight_PN.Value.Trim(), out decResult))
                cmdUpdatePWD.Parameters["@Weight"].Value = decResult;
            else
                cmdUpdatePWD.Parameters["@Weight"].Value = 0;

            gClass.ExecuteDMLCommand(cmdUpdatePWD);
            gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value, Request.Cookies["Logon_UserName"].Value, Request.Url.Host,
                                "Patient Visit Form", 2, "Update Current Weight", "PatientID", Request.Cookies["PatientID"].Value);

            gClass.SaveActionLog(gClass.OrganizationCode,
                                 Request.Cookies["UserPracticeCode"].Value,
                                 Request.Url.Host,
                                 System.Configuration.ConfigurationManager.AppSettings["VisitPage"].ToString(),
                                 System.Configuration.ConfigurationManager.AppSettings["LogUpdate"].ToString(),
                                 "Save " + System.Configuration.ConfigurationManager.AppSettings["PatientWeightData"].ToString(),
                                 Request.Cookies["PatientID"].Value,
                                 "");
        }
        return;
    }
    #endregion 

    #region private void BoldComorbidity_SaveProc();
    private void BoldComorbidity_SaveProc()
    {
        SqlCommand cmdSave = new SqlCommand();
        String baseValue = String.Empty;
        UserControl_SystemCodeWUCtrl tempSelect = new UserControl_SystemCodeWUCtrl();
        String tempSelectField = "";
        String tempField = "";

        //get baseline comorbidity
        DataView dvVisitBaseline = LoadComorbidityData(Convert.ToInt32(0));
        Boolean flag = dvVisitBaseline.Count > 0;
        
        gClass.MakeStoreProcedureName(ref cmdSave, "sp_ConsultFu1_BoldComorbidity_SaveData", true);
        cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSave.Parameters.Add("@PatientId", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
        cmdSave.Parameters.Add("@ConsultId", SqlDbType.Int).Value = Convert.ToInt32(txtHTempConsultID.Value);
        cmdSave.Parameters.Add("@VitaminList", SqlDbType.NVarChar, 50).Value = GetMedicationVitaminValues();
        cmdSave.Parameters.Add("@Vitamin_Description", SqlDbType.NVarChar, 255).Value = txtMedicationNotes.Text;
        cmdSave.Parameters.Add("@MedicationList", SqlDbType.VarChar).Value = txtMedication_Selected.Value;

        
        //add commorbidity
        for (int Xh = 0; Xh < comorbidityArr.Length / 3; Xh++)
        {
            tempSelectField = comorbidityArr[Xh, 1];
            tempField = comorbidityArr[Xh, 0];

            if (comorbidityArr[Xh, 2] == "both")
            {
                if (Request.Cookies["SubmitData"].Value.IndexOf("acs") >= 0)
                    tempField += "ACS";
            }

            tempSelect = (UserControl_SystemCodeWUCtrl)FindControlRecursive(this.Page, tempField);

            baseValue = flag ? dvVisitBaseline[0][tempSelectField].ToString() : String.Empty;
            cmdSave.Parameters.Add("@" + tempSelectField, SqlDbType.NVarChar, 10).Value = tempSelect.SelectedValue != "" ? tempSelect.SelectedValue : baseValue;
        }

        try
        {
            gClass.ExecuteDMLCommand(cmdSave);
            BoldComorbidity_SaveNotesProc();
            gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value, Request.Cookies["Logon_UserName"].Value, Request.Url.Host,
                "Patient Visit Form", 2, "BOLD Comorbidity", "Consult ID", txtHConsultID.Value);

            gClass.SaveActionLog(gClass.OrganizationCode,
                                 Request.Cookies["UserPracticeCode"].Value,
                                 Request.Url.Host,
                                 System.Configuration.ConfigurationManager.AppSettings["VisitPage"].ToString(),
                                 System.Configuration.ConfigurationManager.AppSettings["LogUpdate"].ToString(),
                                 "Save " + System.Configuration.ConfigurationManager.AppSettings["VisitPage"].ToString() + " BOLD Data",
                                 Request.Cookies["PatientID"].Value,
                                 txtHConsultID.Value);

            String strScript = "CheckAgeToShow();"; 
            ScriptManager.RegisterStartupScript(btnAddVisit, btnAddVisit.GetType(), Guid.NewGuid().ToString(), strScript, true);
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Visit Form - Comorbidity", "BoldComorbidity_SaveProc function", err.ToString());
            ShowDivErrorMessage("Error in data saving...");
        }
    }
    #endregion

    #region private void BoldComorbidityNotes_SaveProc()
    private void BoldComorbidity_SaveNotesProc()
    {
        SqlCommand cmdSave = new SqlCommand();

        gClass.MakeStoreProcedureName(ref cmdSave, "sp_ConsultFu1_BoldComorbidity_SaveNotesData", true);
        cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSave.Parameters.Add("@PatientId", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
        cmdSave.Parameters.Add("@ConsultId", SqlDbType.Int).Value = Convert.ToInt32(txtHConsultID.Value);
        cmdSave.Parameters.Add("@CVS", SqlDbType.NVarChar, 1024).Value = txtCVSNote.Text.Trim();
        cmdSave.Parameters.Add("@MET", SqlDbType.NVarChar, 1024).Value = txtMETNote.Text.Trim();
        cmdSave.Parameters.Add("@PUL", SqlDbType.NVarChar, 1024).Value = txtPULNote.Text.Trim();
        cmdSave.Parameters.Add("@GAS", SqlDbType.NVarChar, 1024).Value = txtGASNote.Text.Trim(); 
        cmdSave.Parameters.Add("@MUS", SqlDbType.NVarChar, 1024).Value = txtMUSNote.Text.Trim();
        cmdSave.Parameters.Add("@REPRD", SqlDbType.NVarChar, 1024).Value = txtFEMNote.Text.Trim();
        cmdSave.Parameters.Add("@PSY", SqlDbType.NVarChar, 1024).Value = txtPSYNote.Text.Trim();
        cmdSave.Parameters.Add("@GEN", SqlDbType.NVarChar, 1024).Value = txtGENNote.Text.Trim();
        gClass.ExecuteDMLCommand(cmdSave);
    }
    #endregion 

    #region private string GetMedicationVitaminValues(){
    private string GetMedicationVitaminValues()
    {
        String strValues = String.Empty;
        
        strValues += (chkMutipleVitamin.Checked ? "1" : "0") + ";";
        strValues += (chkCalcium.Checked ? "1" : "0") + ";";
        strValues += (chkVitaminB12.Checked ? "1" : "0") + ";";
        strValues += (chkIron.Checked ? "1" : "0") + ";";
        strValues += (chkVitaminD.Checked ? "1" : "0") + ";";
        strValues += (chkVitaminADE.Checked ? "1" : "0") + ";";
        strValues += (chkCalciumVitaminD.Checked ? "1" : "0") + ";";

        return strValues;
    }
    #endregion 
    
    #region private void LoadGeneralNotes()
    /// <summary>
    /// This function is to load Notes as GENERAL NOTES
    /// </summary>
    private void LoadGeneralNotes()
    {
        SqlCommand cmdSelect = new SqlCommand();
        DataSet dsPatient;

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_GeneralNotesGet", false);
        cmdSelect.Parameters.Add("@vintOrganizationCode", SqlDbType.Int).Value = Convert.ToInt64(gClass.OrganizationCode);
        cmdSelect.Parameters.Add("@vintPatientID", SqlDbType.Int).Value = Convert.ToInt64(Request.Cookies["PatientID"].Value);
        dsPatient = gClass.FetchData(cmdSelect, "tblGeneralNotes");
        txtPatientNotes.Text = (dsPatient.Tables["tblGeneralNotes"].Rows.Count > 0) ? dsPatient.Tables["tblGeneralNotes"].Rows[0]["GeneralNotes"].ToString() : "";
        dsPatient.Dispose();
        cmdSelect.Dispose();
    }
    #endregion
    
    #region private void LoadProgressNoteList(bool ComorbidityFlag)
    private void LoadProgressNoteList(bool ComorbidityFlag)
    {
        SqlCommand cmdSelect = new SqlCommand(), cmdDocument = new SqlCommand();
        DataSet dsPatient;

        Int32 intSurgeonID=0;

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_ConsultFU1_ProgressNotes_LoadData", true);
        gClass.MakeStoreProcedureName(ref cmdDocument, "sp_ConsultFU1_ProgressNotes_EachVisitDocuments", true);
        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt64(gClass.OrganizationCode);
        cmdSelect.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt64(Request.Cookies["UserPracticeCode"].Value);
        cmdSelect.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt64(Request.Cookies["PatientID"].Value); 
        cmdSelect.Parameters.Add("@ImperialFlag", SqlDbType.Bit).Value = Request.Cookies["Imperial"].Value.Equals("True");
        cmdSelect.Parameters.Add("@ComorbidityFlag", SqlDbType.Bit).Value = ComorbidityFlag;
        cmdSelect.Parameters.Add("@VisitWeeksFlag", SqlDbType.VarChar).Value = Request.Cookies["VisitWeeksFlag"].Value;
        cmdSelect.Parameters.Add("@ReportFlag", SqlDbType.Bit).Value = false;
        cmdSelect.Parameters.Add("@IncludeComment", SqlDbType.Bit).Value = true;

        dsPatient = gClass.FetchData(cmdSelect, "tblConsultFU1_ProgressNotes");

        if (dsPatient.Tables.Count > 0)
        {
            DataColumn dcTemp = new DataColumn();
            dcTemp.DataType = Type.GetType("System.String");
            dcTemp.DefaultValue = "";
            dcTemp.ColumnName = "tempDateSeen";
            dcTemp.Caption = "tempDateSeen";
            dsPatient.Tables[0].Columns.Add(dcTemp);

            dcTemp = new DataColumn();
            dcTemp.DataType = Type.GetType("System.String");
            dcTemp.DefaultValue = "";
            dcTemp.ColumnName = "tempFirstVisitDate";
            dcTemp.Caption = "tempFirstVisitDate";
            dsPatient.Tables[0].Columns.Add(dcTemp);

            dcTemp = new DataColumn();
            dcTemp.DataType = Type.GetType("System.String");
            dcTemp.DefaultValue = "";
            dcTemp.ColumnName = "tempZeroDate";
            dcTemp.Caption = "tempZeroDate";
            dsPatient.Tables[0].Columns.Add(dcTemp);

            dcTemp = new DataColumn();
            dcTemp.DataType = Type.GetType("System.String");
            dcTemp.DefaultValue = "";
            dcTemp.ColumnName = "tempOperationDate";
            dcTemp.Caption = "tempOperationDate";
            dsPatient.Tables[0].Columns.Add(dcTemp);

            dcTemp = new DataColumn();
            dcTemp.DataType = Type.GetType("System.String");
            dcTemp.DefaultValue = "";
            dcTemp.ColumnName = "tempDateNextVisit";
            dcTemp.Caption = "tempDateNextVisit";
            dsPatient.Tables[0].Columns.Add(dcTemp);

            dcTemp = new DataColumn();
            dcTemp.DataType = Type.GetType("System.String");
            dcTemp.DefaultValue = "";
            dcTemp.ColumnName = "DateCreatedFormated";
            dcTemp.Caption = "DateCreatedFormated";
            dsPatient.Tables[0].Columns.Add(dcTemp);

            bool latestNextVisit = true;
            dcTemp = new DataColumn();
            dcTemp.DataType = Type.GetType("System.String");
            dcTemp.DefaultValue = "";
            dcTemp.ColumnName = "latestNextVisit";
            dcTemp.Caption = "latestNextVisit";
            dsPatient.Tables[0].Columns.Add(dcTemp);

            dcTemp = new DataColumn();
            dcTemp.DataType = Type.GetType("System.String");
            dcTemp.DefaultValue = "";
            dcTemp.ColumnName = "monthGroup";
            dcTemp.Caption = "monthGroup";
            dsPatient.Tables[0].Columns.Add(dcTemp);


            dcTemp = new DataColumn();
            dcTemp.DataType = Type.GetType("System.String");
            dcTemp.DefaultValue = "";
            dcTemp.ColumnName = "displayDiabetes";
            dcTemp.Caption = "displayDiabetes";
            dsPatient.Tables[0].Columns.Add(dcTemp);

            lblReservoirVolume_TH.Style["display"] = "none";
            String RegistryDone = "";
            Decimal Months = 0;

            Int32 MonthGroup = 0;
            String LoadPartially = "Y";

            for (int Idx = 0; Idx < dsPatient.Tables[0].Rows.Count; Idx++)
            {
                intSurgeonID = Convert.ToInt32(dsPatient.Tables[0].Rows[Idx]["SurgeonID"]);
                /*
                dsPatient.Tables[0].Rows[Idx]["tempDateSeen"] = Lapbase.Data.Common.TruncateDate(dsPatient.Tables[0].Rows[Idx]["DateSeen"].ToString(), Request.Cookies["CultureInfo"].Value);
                dsPatient.Tables[0].Rows[Idx]["tempDateNextVisit"] = Lapbase.Data.Common.TruncateDate(dsPatient.Tables[0].Rows[Idx]["DateNextVisit"].ToString(), Request.Cookies["CultureInfo"].Value);
                */
                if (Convert.ToInt64(dsPatient.Tables[0].Rows[Idx]["ReservoirVolumeExist"]) > 0)
                    lblReservoirVolume_TH.Style["display"] = "block";

                try {
                    if (dsPatient.Tables[0].Rows[Idx]["CommentOnly"].ToString() == "0")
                    {
                        txtHDateFirstVisit.Value = Convert.ToDateTime(dsPatient.Tables[0].Rows[Idx]["DateSeen"].ToString().Trim()).ToShortDateString();
                    }
                    //display diabetes question at first visit occurring up to 1 month pre-annual anniversary or up to 2 months post-annual anniversary after Primary Op 
                    if (Convert.ToDateTime(dsPatient.Tables[0].Rows[Idx]["DateSeen"].ToString().Trim()) > Convert.ToDateTime(txtHDiabetesPeriodMin.Value) && Convert.ToDateTime(dsPatient.Tables[0].Rows[Idx]["DateSeen"].ToString().Trim()) < Convert.ToDateTime(txtHDiabetesPeriodMax.Value))
                    {
                        txtHDiabetesPeriodMax.Value = Convert.ToDateTime(dsPatient.Tables[0].Rows[Idx]["DateSeen"].ToString().Trim()).ToShortDateString();
                    }


                    //display se question at first visit occurring up to 1 month or up to 2 months after latest operation 
                    if (Convert.ToDateTime(dsPatient.Tables[0].Rows[Idx]["DateSeen"].ToString().Trim()) > Convert.ToDateTime(txtHSEPeriodMin.Value) && Convert.ToDateTime(dsPatient.Tables[0].Rows[Idx]["DateSeen"].ToString().Trim()) < Convert.ToDateTime(txtHSEPeriodMax.Value))
                    {
                        txtHSEPeriodMax.Value = Convert.ToDateTime(dsPatient.Tables[0].Rows[Idx]["DateSeen"].ToString().Trim()).ToShortDateString();
                    }
                }
                catch { }

                try { dsPatient.Tables[0].Rows[Idx]["tempDateSeen"] = Convert.ToDateTime(dsPatient.Tables[0].Rows[Idx]["DateSeen"].ToString().Trim()).ToShortDateString(); }
                catch { }
                try { dsPatient.Tables[0].Rows[Idx]["tempFirstVisitDate"] = Convert.ToDateTime(dsPatient.Tables[0].Rows[Idx]["FirstVisitDate"].ToString().Trim()).ToShortDateString(); }
                catch { }
                try { dsPatient.Tables[0].Rows[Idx]["tempZeroDate"] = Convert.ToDateTime(dsPatient.Tables[0].Rows[Idx]["ZeroDate"].ToString().Trim()).ToShortDateString(); }
                catch { }
                try { dsPatient.Tables[0].Rows[Idx]["tempOperationDate"] = Convert.ToDateTime(dsPatient.Tables[0].Rows[Idx]["OperationDate"].ToString().Trim()).ToShortDateString(); }
                catch { }
                try { dsPatient.Tables[0].Rows[Idx]["DateCreatedFormated"] = gClass.TruncateDate(dsPatient.Tables[0].Rows[Idx]["DateCreated"].ToString().Trim(), Request.Cookies["CultureInfo"].Value, 1); }
                catch { }
                try { dsPatient.Tables[0].Rows[Idx]["tempDateNextVisit"] = Convert.ToDateTime(dsPatient.Tables[0].Rows[Idx]["DateNextVisit"].ToString().Trim()).ToShortDateString(); }
                catch { }
                try
                {
                    if (dsPatient.Tables[0].Rows[Idx]["CommentOnly"].ToString() == "0" && latestNextVisit == true)
                    {
                        dsPatient.Tables[0].Rows[Idx]["latestNextVisit"] = "1";
                        latestNextVisit = false;
                    }
                }
                catch { }



                Months = 0;

                try
                {
                    //get the months since
                    if (dsPatient.Tables[0].Rows[Idx]["VisitWeeksFlag"].ToString() == "1")
                    {
                        Months = Convert.ToInt64(dsPatient.Tables[0].Rows[Idx]["MonthsFromFirstVisit"].ToString());
                    }
                    else if (dsPatient.Tables[0].Rows[Idx]["VisitWeeksFlag"].ToString() == "3")
                    {
                        Months = Convert.ToInt64(dsPatient.Tables[0].Rows[Idx]["MonthsFromZeroDate"].ToString());
                    }
                    else if (dsPatient.Tables[0].Rows[Idx]["VisitWeeksFlag"].ToString() == "4")
                    {
                        Months = Convert.ToInt64(dsPatient.Tables[0].Rows[Idx]["MonthsFromOperationDate"].ToString());
                    }

                    //classified it in 6 months, 12 months, or anually
                    MonthGroup = CalculateMonthGroup(Months);
                    
                    dsPatient.Tables[0].Rows[Idx]["monthGroup"] = MonthGroup;
                }
                catch { }
                
                if (dsPatient.Tables[0].Rows[Idx]["RegistryReview"].ToString() == "1")
                {
                    RegistryDone += "*" + MonthGroup + "*";
                }
            }
            if (txtHRegistryDone.Value == "")
            {
                txtHRegistryDone.Value = RegistryDone;
            }
            cmbLetterFrom_PN.SelectedValue = intSurgeonID.ToString();

            dsPatient.Tables[0].AcceptChanges();
            gClass.CalculateWeightData(ref dsPatient, "tblConsultFU1_ProgressNotes", Request.Cookies["Imperial"].Value.Equals("True"));

            cmdDocument.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt64(gClass.OrganizationCode);
            cmdDocument.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt64(Request.Cookies["UserPracticeCode"].Value);
            cmdDocument.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt64(Request.Cookies["PatientID"].Value);

            dsPatient.Tables.Add(gClass.FetchData(cmdDocument, "tblDocuments").Tables[0].Copy());



            if (!ComorbidityFlag)
            {
                if (Request.QueryString["LP"]!=null)
                {
                    LoadPartially = Request.QueryString["LP"].ToString();
                }

                if (LoadPartially == "Y")
                {
                    if (dsPatient.Tables[0].Rows.Count > 20)
                    {
                        div_LoadAllVisit.Style["display"] = "block";
                    }
                    for (int IdxRow = 0; IdxRow < dsPatient.Tables[0].Rows.Count; IdxRow++)
                    {
                        if (IdxRow >= 20)
                        {
                            dsPatient.Tables[0].Rows[IdxRow].Delete();
                        }
                    }
                }
                div_VisitsList.InnerHtml = gClass.ShowSchema(dsPatient, Server.MapPath(".") + @"\Includes\ConsultFU1XSLTFile.xsl");
            }
            else
                div_ComorbidityVisitsList.InnerHtml = gClass.ShowSchema(dsPatient, Server.MapPath(".") + @"\Includes\ConsultFU1XSLTFile.xsl");

            dsPatient.Dispose();
        }
        return;
    }
    #endregion 

    #region private void LoadOperationList()
    private void LoadOperationList()
    {
        SqlCommand cmdSelect = new SqlCommand();
        string strReturn = String.Empty;

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_Operation_SelectPatientOperationsList", true);
        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSelect.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
        cmdSelect.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
        cmdSelect.Parameters.Add("@ImperialFlag", SqlDbType.Bit).Value = Request.Cookies["Imperial"].Value.Equals("True");
        try
        {
            DataSet dsTemp = gClass.FetchData(cmdSelect, "tblPatientOperation");
            DataColumn dcStrDate = new DataColumn();

            dcStrDate.ColumnName = "strDateOperation";
            dcStrDate.DataType = Type.GetType("System.String");
            dsTemp.Tables[0].Columns.Add(dcStrDate);
            for (int Xh = 0; Xh < dsTemp.Tables[0].Rows.Count; Xh++)
            {
                dsTemp.Tables[0].Rows[Xh]["strDateOperation"] = gClass.TruncateDate(dsTemp.Tables[0].Rows[Xh]["OperationDate"].ToString(), Request.Cookies["CultureInfo"].Value, 1);
                if(txtHLatestOperationDate.Value == "")
                    txtHLatestOperationDate.Value = gClass.TruncateDate(dsTemp.Tables[0].Rows[Xh]["OperationDate"].ToString(), Request.Cookies["CultureInfo"].Value, 1);
            }
            dsTemp.AcceptChanges();

            if (txtHLatestOperationDate.Value != "")
            {
                DateTime dtSEPeriodMin;
                DateTime dtSEPeriodMax;

                dtSEPeriodMin = Convert.ToDateTime(txtHLatestOperationDate.Value.Trim()).AddMonths(1);
                dtSEPeriodMax = Convert.ToDateTime(txtHLatestOperationDate.Value.Trim()).AddMonths(2);

                txtHSEPeriodMin.Value = dtSEPeriodMin.ToShortDateString();
                txtHSEPeriodMax.Value = dtSEPeriodMax.ToShortDateString();
            }

            strReturn = gClass.ShowSchema(dsTemp, Server.MapPath(".") + @"\Includes\OperationsListXSLTFile.xsl");
        }
        catch (Exception err)
        {
            strReturn = String.Empty;

            gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                Context.Request.Cookies["Logon_UserName"].Value, "Operation form (Ajax form)",
                "Load Patient's Operation List - LoadPatientOperations function", err.ToString());
        }
        divOperationListDetail.InnerHtml = strReturn;
        return;
    }
    #endregion 

    #region private void ShowDivErrorMessage(string strErrorMessage)
    private void ShowDivErrorMessage(string strErrorMessage)
    {
        divErrorMessage.Style["diplay"] = (strErrorMessage.Trim().Length == 0) ? "none" : "block";
        pErrorMessage.InnerHtml = (strErrorMessage.Trim().Length == 0) ? "" : strErrorMessage;
    }
    #endregion 

    #region protected void btnLoadVisitData_OnClick(object sender, EventArgs e)
    /// <summary>
    /// first we call the "CheckBoldComorbidiyInBaseline()" to check the BASELINE Comorbidity to hide/show each dropdownlist
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnLoadVisitData_OnClick(object sender, EventArgs e)
    {
        UserControl_SystemCodeWUCtrl tempSelect = new UserControl_SystemCodeWUCtrl();
        String tempSelectField = "";
        String tempField = "";

        DataView dvBaseline = CheckBoldComorbidiyInBaseline();
        DataView dvVisit = LoadComorbidityData(Convert.ToInt32(txtHConsultID.Value));

        if (dvVisit != null && dvVisit.Count > 0)
        {
            //DataView dvVisit = dsVisit.Tables["tblVisit"].DefaultView;
            Boolean flag = dvVisit.Count > 0;

            for (int Xh = 0; Xh < comorbidityArr.Length / 3; Xh++)
            {
                tempSelectField = comorbidityArr[Xh, 1];
                tempField = comorbidityArr[Xh, 0];

                if (comorbidityArr[Xh, 2] == "both")
                {
                    if (Request.Cookies["SubmitData"].Value.IndexOf("acs") >= 0)
                    {
                        tempField += "ACS";
                        tempSelect = (UserControl_SystemCodeWUCtrl)FindControlRecursive(this.Page, tempField);
                        tempSelect.SelectedValue = flag ? dvVisit[0][tempSelectField].ToString() : String.Empty;
                    }
                }

                tempSelect = (UserControl_SystemCodeWUCtrl)FindControlRecursive(this.Page, tempField);
                tempSelect.SelectedValue = flag ? dvVisit[0][tempSelectField].ToString() : String.Empty;
            }


            SetMedicationVitaminValues(flag ? dvVisit[0]["VitaminList"].ToString() : String.Empty);
            txtMedicationNotes.Text = flag ? dvVisit[0]["Vitamin_Description"].ToString() : String.Empty;
            txtMedication_Selected.Value = flag ? dvVisit[0]["MedicationList"].ToString() : String.Empty;

            FillSelectedLists(cmbMedication, listMedication_Selected, txtMedication_Selected.Value);

            // Now we load the comorbidity notes
            txtCVSNote.Text = flag ? dvVisit[0]["CVS"].ToString() : String.Empty;
            txtMETNote.Text = flag ? dvVisit[0]["MET"].ToString() : String.Empty;
            txtPULNote.Text = flag ? dvVisit[0]["PUL"].ToString() : String.Empty;
            txtGASNote.Text = flag ? dvVisit[0]["GAS"].ToString() : String.Empty;
            txtMUSNote.Text = flag ? dvVisit[0]["MUS"].ToString() : String.Empty;
            txtFEMNote.Text = flag ? dvVisit[0]["REPRD"].ToString() : String.Empty;
            txtPSYNote.Text = flag ? dvVisit[0]["PSY"].ToString() : String.Empty;
            txtGENNote.Text = flag ? dvVisit[0]["GEN"].ToString() : String.Empty;

            SqlCommand cmdLoad = new SqlCommand();
            gClass.MakeStoreProcedureName(ref cmdLoad, "sp_ConsultFU1_ProgressNotes_LoadBoldHistoryData", true);
            cmdLoad.Parameters.Add("@vintOrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
            cmdLoad.Parameters.Add("@vintPatientID", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
            cmdLoad.Parameters.Add("@vintConsultId", SqlDbType.Int).Value = Convert.ToInt32(txtHConsultID.Value);
            cmdLoad.Parameters.Add("@vstrBoldCode", SqlDbType.VarChar, 10);

            LoadBoldHistoryData(dvBaseline, ref cmdLoad, "CVS", divCVSHistoryData);
            LoadBoldHistoryData(dvBaseline, ref cmdLoad, "MET", divMETHistoryData);
            LoadBoldHistoryData(dvBaseline, ref cmdLoad, "PUL", divPULHistoryData);
            LoadBoldHistoryData(dvBaseline, ref cmdLoad, "GAS", divGASHistoryData);
            LoadBoldHistoryData(dvBaseline, ref cmdLoad, "MUS", divMUSHistoryData);
            LoadBoldHistoryData(dvBaseline, ref cmdLoad, "REPRD", divREPRDHistoryData);
            LoadBoldHistoryData(dvBaseline, ref cmdLoad, "PSY", divPSYCHHistorydata);
            LoadBoldHistoryData(dvBaseline, ref cmdLoad, "GEN", divGENHistoryData);
            
            LoadBoldHistoryData(dvBaseline, ref cmdLoad, "PULACS", divPULACSHistoryData);
            LoadBoldHistoryData(dvBaseline, ref cmdLoad, "GASACS", divGASACSHistoryData);
            LoadBoldHistoryData(dvBaseline, ref cmdLoad, "MUSCACS", divMUSCACSHistoryData);
            LoadBoldHistoryData(dvBaseline, ref cmdLoad, "RENACS", divRENACSHistoryData);
            LoadBoldHistoryData(dvBaseline, ref cmdLoad, "CARDACS", divCARDACSHistoryData);
            LoadBoldHistoryData(dvBaseline, ref cmdLoad, "VASCACS", divVASCACSHistoryData);
            LoadBoldHistoryData(dvBaseline, ref cmdLoad, "OTHERACS", divOTHERACSHistoryData);
            LoadBoldHistoryData(dvBaseline, ref cmdLoad, "GENACS", divGENACSHistoryData);
        }
        ScriptManager.RegisterStartupScript(btnLoadVisitData, btnLoadVisitData.GetType(), Guid.NewGuid().ToString(), "HideDivMessage();", true);
    }
    #endregion

    #region protected void btnLoadRefDoctorData()
    protected void btnLoadRefDoctorData()
    {
        SqlCommand cmdLoad = new SqlCommand();
        DataSet dsRefDoctor = new DataSet();
        Int32 intPatientID = 0;
        String refDrID1 = "";
        String refDrID2 = "";
        String refDrID3 = "";
        String refDrName1 = "";
        String refDrName2 = "";
        String refDrName3 = "";
    
        gClass.MakeStoreProcedureName(ref cmdLoad, "sp_PatientData_LoadRefDoctor", true);

        cmdLoad.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdLoad.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);

        dsRefDoctor = gClass.FetchData(cmdLoad, "tblPatient_RefDoctorData");

        //default
        cmbDoctor1.Style["display"] = "none";
        cmbDoctor2.Style["display"] = "none";
        cmbDoctor3.Style["display"] = "none";
        lblDoctor1.Style["display"] = "none";
        lblDoctor2.Style["display"] = "none";
        lblDoctor3.Style["display"] = "none";
        cmbDoctor1.Value = "";
        cmbDoctor2.Value = "";
        cmbDoctor3.Value = "";
        lblWarning.Style["display"] = "none";
        
        if ((dsRefDoctor.Tables.Count > 0) && (dsRefDoctor.Tables[0].Rows.Count > 0))
        {
            refDrID1 = dsRefDoctor.Tables[0].Rows[0]["RefDrId1"].ToString();
            refDrID2 = dsRefDoctor.Tables[0].Rows[0]["RefDrId2"].ToString();
            refDrID3 = dsRefDoctor.Tables[0].Rows[0]["RefDrId3"].ToString();
            refDrName1 = dsRefDoctor.Tables[0].Rows[0]["RefDrName1"].ToString();
            refDrName2 = dsRefDoctor.Tables[0].Rows[0]["RefDrName2"].ToString();
            refDrName3 = dsRefDoctor.Tables[0].Rows[0]["RefDrName3"].ToString();

            if (refDrID1 == "" && refDrID2 == "" && refDrID3 == "")
            {
                lblWarning.Style["display"] = "block";
                lblWarning.Text = "There is no Referring Doctor for this patient.<br>To add, please go to 'Patient Details' >> 'Demographic'.";
            }
            else{
                if (refDrID1 != "")
                {
                    cmbDoctor1.Style["display"] = "block";
                    lblDoctor1.Style["display"] = "block";
                    cmbDoctor1.Value = refDrID1;
                    lblDoctor1.Text = refDrName1;
                }
                if (refDrID2 != "")
                {
                    cmbDoctor2.Style["display"] = "block";
                    lblDoctor2.Style["display"] = "block";
                    cmbDoctor2.Value = refDrID2;
                    lblDoctor2.Text = refDrName2;
                }
                if (refDrID3 != "")
                {
                    cmbDoctor3.Style["display"] = "block";
                    lblDoctor3.Style["display"] = "block";
                    cmbDoctor3.Value = refDrID3;
                    lblDoctor3.Text = refDrName3;
                }
            }

            //strRefSurname2 = dsRefDoctor.Tables[0].Rows[0][5].ToString();
            //LoadPatientData_RefDoctorData(dsPatient.Tables[0].DefaultView);
        }
    }
    #endregion

    #region private void LoadBoldHistoryData(DataView dvBaseline, ref SqlCommand cmdLoad, String strCategory, HtmlGenericControl htmlDiv)
    /// <summary>
    ///     This function is to load out the Bold Comorbidity history data for each group
    /// </summary>
    /// <param name="cmdLoad">the SQL Command</param>
    /// <param name="strCategory">Comorbidity category code like 'CVS', 'MET' and etc</param>
    /// <param name="htmlDiv">the DIV section where the data is loaded</param>
    /// <history>
    ///     <change user="Ali Farahani" date = "10 Dec 07">Initial version</change>
    /// </history>
    private void LoadBoldHistoryData(DataView dvBaseline, ref SqlCommand cmdLoad, String strCategory, HtmlGenericControl htmlDiv)
    {
        DataSet dsTemp = new DataSet();
        DataTable dtTemp = new DataTable("tblVisitDate");
        String tempCode = "";

        cmdLoad.Parameters["@vstrBoldCode"].Value = strCategory;
        dsTemp = gClass.FetchData(cmdLoad, "tblHistoryData");

        gClass.AddColumn(ref dtTemp, "strVisitDate", "System.String", "");
        dtTemp.Constraints.Add(new UniqueConstraint(dtTemp.Columns[0]));
        foreach (DataRow dr in dsTemp.Tables["tblHistoryData"].Rows)
        {
            tempCode = strCategory + "_" + dr["Code"].ToString().Trim();

            if (dvBaseline[0][tempCode + "_Rank"].ToString().Trim() != "0")
            {
                DataRow drTemp = dtTemp.NewRow();
                dr["strVisitDate"] = Convert.ToDateTime(dr["VisitDate"].ToString().Trim()).ToShortDateString();
                drTemp["strVisitDate"] = Convert.ToDateTime(dr["VisitDate"].ToString().Trim()).ToShortDateString();
                try { dtTemp.Rows.Add(drTemp); }
                catch { }
            }
        }
        dsTemp.Tables.Add(dtTemp);
        htmlDiv.InnerHtml = gClass.ShowSchema(dsTemp, Server.MapPath(".") + "/Includes/BoldHistoryValueXSLTFile.xsl");

        dsTemp.Dispose();
    }
    #endregion 

    #region private void SetMedicationVitaminValues(String strVitamins)
    private void SetMedicationVitaminValues(String strVitamins)
    {
        if (strVitamins.Equals(String.Empty))
        {
            chkMutipleVitamin.Checked = chkCalcium.Checked = chkVitaminB12.Checked = chkIron.Checked = chkVitaminD.Checked = chkVitaminADE.Checked = chkCalciumVitaminD.Checked = false;
        }
        else
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(";");
            String[] strVitaminCollection = regex.Split(strVitamins);

            chkMutipleVitamin.Checked = strVitaminCollection[0].Equals("1");
            chkCalcium.Checked = strVitaminCollection[1].Equals("1");
            chkVitaminB12.Checked = strVitaminCollection[2].Equals("1");
            chkIron.Checked = strVitaminCollection[3].Equals("1");
            chkVitaminD.Checked = strVitaminCollection[4].Equals("1");
            chkVitaminADE.Checked = strVitaminCollection[5].Equals("1");
            chkCalciumVitaminD.Checked = strVitaminCollection[6].Equals("1");
        }
    }
    #endregion 

    #region private DataView CheckBoldComorbidiyInBaseline()
    /// <summary>
    /// this function is to load baseline bold comorbidity and based on these values, some dropdownlist shoud be visible or hidden
    /// <history>
    ///     <change author="Ali Farahani" VersionNo = "1.0" Date="06 Nov 07">Initial version</change>
    /// </history>
    /// </summary>
    private DataView CheckBoldComorbidiyInBaseline()
    {
        DataView dvComorbidity = LoadComorbidityData(0); // we pass 0 to load Baseline Comorbidity data
        if (dvComorbidity != null && dvComorbidity.Count > 0)
            ShowHideComorbidityFields(dvComorbidity);
        else
        {
            //if there is no baseline comorbidity data, the message about non-baseline comorbidity is loaded
            divBaselineSetup.Style["display"] = "block";
        }
        return dvComorbidity;
    }
    #endregion 

    #region private DataView LoadComorbidityData(Int32 intConsultantID)
    private DataView LoadComorbidityData(Int32 intConsultantID)
    {
        SqlCommand cmdLoad = new SqlCommand();
        DataSet dsVisit;
        DataView dvTemp;

        gClass.MakeStoreProcedureName(ref cmdLoad,
            (intConsultantID == 0) ? "sp_ConsultFU1_ProgressNotes_LoadBaselineBoldComorbidity" : "sp_ConsultFU1_ProgressNotes_LoadBoldComorbidityData"
            , true);
        cmdLoad.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdLoad.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
        cmdLoad.Parameters.Add("@ConsultId", SqlDbType.Int).Value = intConsultantID;
        dsVisit = gClass.FetchData(cmdLoad, "tblComorbidity");
        cmdLoad.Dispose();
        if (dsVisit.Tables.Count > 0)
            dvTemp = dsVisit.Tables[0].DefaultView;
        else
            dvTemp = null;
        //return ((dsVisit.Tables.Count > 0) ? dsVisit.Tables[0].DefaultView : null); 
        return dvTemp;
    }
    #endregion

    #region private void ShowHideComorbidityFields(DataView dvPatient)
    private void ShowHideComorbidityFields(DataView dvPatient)
    {
        String strScript = String.Empty;

        Boolean CVS_Flag, MET_Flag, PUL_Flag, GAS_Flag, MUS_Flag, REPRD_Flag, PSY_Flag, GEN_Flag, PULACS_Flag, GASACS_Flag, MUSCACS_Flag, RENACS_Flag, CARDACS_Flag, VASCACS_Flag, OTHERACS_Flag, GENACS_Flag;

        CVS_Flag = MET_Flag = PUL_Flag = GAS_Flag = MUS_Flag = REPRD_Flag = PSY_Flag = GEN_Flag = PULACS_Flag = GASACS_Flag = MUSCACS_Flag = RENACS_Flag = CARDACS_Flag = VASCACS_Flag = OTHERACS_Flag = GENACS_Flag = false;

        CheckComorbidityFieldFromBaseLine(dvPatient, "CVS_Hypertension", "CVS_Hypertension", tr_cmbHypertension, cmbHypertension, lblHypertension, ref CVS_Flag, ref strScript); //_Rank
        CheckComorbidityFieldFromBaseLine(dvPatient, "CVS_Congestive", "CVS_Congestive", tr_cmbCongestive, cmbCongestive, lblCongestiveHeartFailure, ref CVS_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "CVS_Ischemic", "CVS_Ischemic", tr_cmbIschemic, cmbIschemic, lblIschemicHeartDisease, ref CVS_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "CVS_Angina", "CVS_Angina", tr_cmbAngina, cmbAngina, lblAnginaAssessment, ref CVS_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "CVS_Peripheral", "CVS_Peripheral", tr_cmbPeripheral, cmbPeripheral, lblPeripheralVascularDisease, ref CVS_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "CVS_Lower", "CVS_Lower", tr_cmbLower, cmbLower, lblLowerExtremityEdema, ref CVS_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "CVS_DVT", "CVS_DVT", tr_cmbDVT, cmbDVT, lblDVT, ref CVS_Flag, ref strScript);
        divCardiovascularDisease.Style["display"] = CVS_Flag ? "block" : "none";

        CheckComorbidityFieldFromBaseLine(dvPatient, "MET_Glucose", "MET_Glucose", tr_cmbGlucose, cmbGlucose, lblGlucoseMetabolism, ref MET_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "MET_Lipids", "MET_Lipids", tr_cmbLipids, cmbLipids, lblLipids, ref MET_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "MET_Gout", "MET_Gout", tr_cmbGout, cmbGout, lblGoutHyperuricemia, ref MET_Flag, ref strScript);
        divMetabolic.Style["display"] = MET_Flag ? "block" : "none";

        CheckComorbidityFieldFromBaseLine(dvPatient, "PUL_Obstructive", "PUL_Obstructive", tr_cmbObstructive, cmbObstructive, lblObstructiveSleepApneaSyndrome, ref PUL_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "PUL_Obesity", "PUL_Obesity", tr_cmbObesity, cmbObesity, lblObesityHypoventilationSyndrome, ref PUL_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "PUL_PulHypertension", "PUL_PulHypertension", tr_cmbPulmonary, cmbPulmonary, lblPulmonaryHypertension, ref PUL_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "PUL_Asthma", "PUL_Asthma", tr_cmbAsthma, cmbAsthma, lblAsthma, ref PUL_Flag, ref strScript);
        divPulmonary.Style["display"] = PUL_Flag ? "block" : "none";

        CheckComorbidityFieldFromBaseLine(dvPatient, "GAS_Gerd", "GAS_Gerd", tr_cmbGred, cmbGred, lblGerd, ref GAS_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "GAS_Cholelithiasis", "GAS_Cholelithiasis", tr_cmbCholelithiasis, cmbCholelithiasis, lblCholelithiasis, ref GAS_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "GAS_Liver", "GAS_Liver", tr_cmbLiver, cmbLiver, lblLiverDisease, ref GAS_Flag, ref strScript);
        divGastroIntestinal.Style["display"] = GAS_Flag ? "block" : "none";

        CheckComorbidityFieldFromBaseLine(dvPatient, "MUS_BackPain", "MUS_BackPain", tr_cmbBackPain, cmbBackPain, lblBackPain, ref MUS_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "MUS_Musculoskeletal", "MUS_Musculoskeletal", tr_cmbMusculoskeletal, cmbMusculoskeletal, lblMusculoskeletalDisease, ref MUS_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "MUS_Fibromyalgia", "MUS_Fibromyalgia", tr_cmbFibro, cmbFibro, lblFibromyalgia, ref MUS_Flag, ref strScript);
        divMusculoskeletal.Style["display"] = MUS_Flag ? "block" : "none";

        CheckComorbidityFieldFromBaseLine(dvPatient, "REPRD_Polycystic", "REPRD_Polycystic", tr_cmbPolycystic, cmbPolycystic, lblPolycysticOverianSyndrome, ref REPRD_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "REPRD_Menstrual", "REPRD_Menstrual", tr_cmbMenstrual, cmbMenstrual, lblMenstrualIrregularities, ref REPRD_Flag, ref strScript);
        divReproductive.Style["display"] = REPRD_Flag ? "block" : "none";

        CheckComorbidityFieldFromBaseLine(dvPatient, "PSY_Impairment", "PSY_Impairment", tr_cmbPsychosocial, cmbPsychosocial, lblPsychosocialImpairment, ref PSY_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "PSY_Depression", "PSY_Depression", tr_cmbDepression, cmbDepression, lblDepression, ref PSY_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "PSY_MentalHealth", "PSY_MentalHealth", tr_cmbConfirmed, cmbConfirmed, lblConfirmedMentalHealthDiagnosis, ref PSY_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "PSY_Alcohol", "PSY_Alcohol", tr_cmbAlcohol, cmbAlcohol, lblAlcoholUse, ref PSY_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "PSY_Tobacoo", "PSY_Tobacoo", tr_cmbTobacco, cmbTobacco, lblTobaccoUse, ref PSY_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "PSY_Abuse", "PSY_Abuse", tr_cmbAbuse, cmbAbuse, lblSubstanceAbuse, ref PSY_Flag, ref strScript);
        divPsychosocial.Style["display"] = PSY_Flag ? "block" : "none";

        CheckComorbidityFieldFromBaseLine(dvPatient, "GEN_Stress", "GEN_Stress", tr_cmbStressUrinary, cmbStressUrinary, lblStressUrinaryIncontinence, ref GEN_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "GEN_Cerebri", "GEN_Cerebri", tr_cmbCerebri, cmbCerebri, lblCerebi, ref GEN_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "GEN_Hernia", "GEN_Hernia", tr_cmbHernia, cmbHernia, lblAbdominalHernia, ref GEN_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "GEN_Functional", "GEN_Functional", tr_cmbFunctional, cmbFunctional, lblFunctionalStatus, ref GEN_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "GEN_Skin", "GEN_Skin", tr_cmbSkin, cmbSkin, lblAbdominal, ref GEN_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "GEN_RenalInsuff", "GEN_RenalInsuff", tr_cmbRenalInsuf, cmbRenalInsuff, lblRenalInsuf, ref GEN_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "GEN_RenalFail", "GEN_RenalFail", tr_cmbRenalFail, cmbRenalFail, lblRenalFail, ref GEN_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "GEN_Steroid", "GEN_Steroid", tr_cmbSteroid, cmbSteroid, lblSteroid, ref GEN_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "GEN_Therapeutic", "GEN_Therapeutic", tr_cmbTherapeutic, cmbTherapeutic, lblTherapeutic, ref GEN_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "GEN_PrevPCISurgery", "GEN_PrevPCISurgery", tr_cmbPrevPCISurgery, cmbPrevPCISurgery, lblPrevPCISurgery, ref GEN_Flag, ref strScript);
        divGeneral.Style["display"] = GEN_Flag ? "block" : "none";

        CheckComorbidityFieldFromBaseLine(dvPatient, "ACS_Smoke", "PULACS_Smoke", tr_cmbSmokerACS, cmbSmokerACS, lblSmokerACS, ref PULACS_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "ACS_Oxy", "PULACS_Oxy", tr_cmbOxygenACS, cmbOxygenACS, lblOxygenACS, ref PULACS_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "ACS_Embo", "PULACS_Embo", tr_cmbEmbolismACS, cmbEmbolismACS, lblEmbolismACS, ref PULACS_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "ACS_Copd", "PULACS_Copd", tr_cmbCopdACS, cmbCopdACS, lblCopdACS, ref PULACS_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "ACS_Cpap", "PULACS_Cpap", tr_cmbCpapACS, cmbCpapACS, lblCpapACS, ref PULACS_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "ACS_Sho", "PULACS_Sho", tr_cmbShoACS, cmbShoACS, lblShoACS, ref PULACS_Flag, ref strScript);
        divPulmonaryACS.Style["display"] = PULACS_Flag ? "block" : "none";

        CheckComorbidityFieldFromBaseLine(dvPatient, "ACS_Gerd", "GASACS_Gerd", tr_cmbGerdACS, cmbGerdACS, lblGerdACS, ref GASACS_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "ACS_Gal", "GASACS_Gal", tr_cmbGallstoneACS, cmbGallstoneACS, lblGallstoneACS, ref GASACS_Flag, ref strScript);
        divGastroACS.Style["display"] = GASACS_Flag ? "block" : "none";

        CheckComorbidityFieldFromBaseLine(dvPatient, "ACS_Muscd", "MUSCACS_Muscd", tr_cmbMusculoDiseaseACS, cmbMusculoDiseaseACS, lblMusculoDiseaseACS, ref MUSCACS_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "ACS_Pain", "MUSCACS_Pain", tr_cmbActivityLimitedACS, cmbActivityLimitedACS, lblActivityLimitedACS, ref MUSCACS_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "ACS_Meds", "MUSCACS_Meds", tr_cmbDailyMedACS, cmbDailyMedACS, lblDailyMedACS, ref MUSCACS_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "ACS_Surg", "MUSCACS_Surg", tr_cmbSurgicalACS, cmbSurgicalACS, lblSurgicalACS, ref MUSCACS_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "ACS_Mob", "MUSCACS_Mob", tr_cmbMobilityACS, cmbMobilityACS, lblMobilityACS, ref MUSCACS_Flag, ref strScript);
        divMusculoACS.Style["display"] = MUSCACS_Flag ? "block" : "none";

        CheckComorbidityFieldFromBaseLine(dvPatient, "GEN_RenalInsuff", "GEN_RenalInsuff", tr_cmbRenalInsuffACS, cmbRenalInsuffACS, lblRenalInsuffACS, ref RENACS_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "GEN_RenalFail", "GEN_RenalFail", tr_cmbRenalFailACS, cmbRenalFailACS, lblRenalFailACS, ref RENACS_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "ACS_Uri", "RENACS_Uri", tr_cmbUrinaryACS, cmbUrinaryACS, lblUrinaryACS, ref RENACS_Flag, ref strScript);
        divRenalACS.Style["display"] = RENACS_Flag ? "block" : "none";

        CheckComorbidityFieldFromBaseLine(dvPatient, "ACS_Myo", "CARDACS_Myo", tr_cmbMyocardinalACS, cmbMyocardinalACS, lblMyocardinalACS, ref CARDACS_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "ACS_Pci", "CARDACS_Pci", tr_cmbPrevPCIACS, cmbPrevPCIACS, lblPrevPCIACS, ref CARDACS_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "ACS_Csurg", "CARDACS_Csurg", tr_cmbPrevCardiacACS, cmbPrevCardiacACS, lblPrevCardiacACS, ref CARDACS_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "ACS_Lipid", "CARDACS_Lipid", tr_cmbHyperlipidemiaACS, cmbHyperlipidemiaACS, lblHyperlipidemiaACS, ref CARDACS_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "ACS_Hyper", "CARDACS_Hyper", tr_cmbHypertensionACS, cmbHypertensionACS, lblHypertensionACS, ref CARDACS_Flag, ref strScript);
        divCardiacACS.Style["display"] = CARDACS_Flag ? "block" : "none";

        CheckComorbidityFieldFromBaseLine(dvPatient, "ACS_Dvt", "VASCACS_Dvt", tr_cmbDVTACS, cmbDVTACS, lblDVTACS, ref VASCACS_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "ACS_Venous", "VASCACS_Venous", tr_cmbVenousACS, cmbVenousACS, lblVenousACS, ref VASCACS_Flag, ref strScript);
        divVascularACS.Style["display"] = VASCACS_Flag ? "block" : "none";

        CheckComorbidityFieldFromBaseLine(dvPatient, "ACS_Health", "OTHERACS_Health", tr_cmbHealthStatusACS, cmbHealthStatusACS, lblHealthStatusACS, ref OTHERACS_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "ACS_Diab", "OTHERACS_Diab", tr_cmbDiabetesACS, cmbDiabetesACS, lblDiabetesACS, ref OTHERACS_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "GEN_Steroid", "GEN_Steroid", tr_cmbSteroidACS, cmbSteroidACS, lblSteroidACS, ref OTHERACS_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "GEN_Therapeutic", "GEN_Therapeutic", tr_cmbTherapeuticACS, cmbTherapeuticACS, lblTherapeuticACS, ref OTHERACS_Flag, ref strScript);
        CheckComorbidityFieldFromBaseLine(dvPatient, "ACS_Obese", "OTHERACS_Obese", tr_cmbObesityACS, cmbObesityACS, lblObesityACS, ref OTHERACS_Flag, ref strScript);
        divOtherACS.Style["display"] = OTHERACS_Flag ? "block" : "none";

        CheckComorbidityFieldFromBaseLine(dvPatient, "ACS_FAT", "GENACS_FAT", tr_cmbFatACS, cmbFatACS, lblFatACS, ref GENACS_Flag, ref strScript);
        divGenACS.Style["display"] = GENACS_Flag ? "block" : "none";

        divBaselineSetup.Style["display"] = (CVS_Flag || MET_Flag || PUL_Flag || GAS_Flag || MUS_Flag || REPRD_Flag || PSY_Flag || GEN_Flag || PULACS_Flag || GASACS_Flag || MUSCACS_Flag || RENACS_Flag || CARDACS_Flag || VASCACS_Flag || OTHERACS_Flag) ? "none" : "block";
        strScript += "; HideDivMessage();";
        ScriptManager.RegisterStartupScript(btnLoadVisitData, btnLoadVisitData.GetType(), Guid.NewGuid().ToString(), strScript, true);
    }
    #endregion 

    #region private void CheckComorbidityFieldFromBaseLine(DataView dvPatient, String strFieldName, String strCategoryName, UserControl_SystemCodeWUCtrl cmbHypertension, HtmlControl lblHypertension, ref Boolean CVS_Flag)
    private void CheckComorbidityFieldFromBaseLine(DataView dvPatient, String strFieldName, String strCategoryName, TableRow trRow, UserControl_SystemCodeWUCtrl cmbComorbidity, HtmlControl lblComorbidity, ref Boolean Group_Flag, ref String strScript)
    {
        String strRankValue = dvPatient[0][strCategoryName + "_Rank"].ToString().Trim();
        Boolean blnHideFlag = (strRankValue.Equals(String.Empty) || strRankValue == null || strRankValue.Equals("0"));

        Group_Flag |= !blnHideFlag;
        if (!blnHideFlag)
            cmbComorbidity.SelectedValue = dvPatient[0][strFieldName].ToString();
        cmbComorbidity.Display = !blnHideFlag;
        lblComorbidity.Visible = !blnHideFlag;
        strScript += "$get('" + trRow.ClientID + "').style.height = " + (!blnHideFlag ? "29" : "0") + ";";
    }
    #endregion

    #region protected void btnCalculateWeightOtherData_OnClick(object sender, EventArgs e)
    protected void btnCalculateWeightOtherData_OnClick(object sender, EventArgs e)
    {
        SqlCommand cmdCommand = new SqlCommand();
        DataSet dsVisit = new DataSet();
        DateTime dtDateSeen = new DateTime();
        string strScript = "javascript:HideDivMessage(); SetEvents(); ";
        String comFlag;

        gClass.MakeStoreProcedureName(ref cmdCommand, "sp_ConsultFU1_WeightCalculationForBMI", true);

        if (txtCurrList.Value == "visit")
            comFlag = Boolean.FalseString;
        else
            comFlag = Boolean.TrueString;

        
        cmdCommand.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdCommand.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
        cmdCommand.Parameters.Add("@Weight", SqlDbType.Decimal).Value = txtWeight_PN.Text == "" ? "0" : txtWeight_PN.Text;
        cmdCommand.Parameters.Add("@ImperialFlag", SqlDbType.Bit).Value = Request.Cookies["Imperial"].Value.Equals(Boolean.TrueString);
        cmdCommand.Parameters.Add("@ComorbidityFlag", SqlDbType.Bit).Value = comFlag;
        cmdCommand.Parameters.Add("@VisitWeeksFlag", SqlDbType.VarChar).Value = Request.Cookies["VisitWeeksFlag"].Value;

        if (DateTime.TryParse(txtDateSeen_PN.Text, out dtDateSeen))
            cmdCommand.Parameters.Add("@VisitDate", SqlDbType.DateTime).Value = dtDateSeen;
        else cmdCommand.Parameters.Add("@VisitDate", SqlDbType.DateTime).Value = DBNull.Value;

        dsVisit = gClass.FetchData(cmdCommand, "tblWeight");

        if ((dsVisit.Tables.Count > 0) && (dsVisit.Tables[0].Rows.Count > 0))
        {
            String startWeight = dsVisit.Tables[0].Rows[0]["DefFirstWeight"].ToString();
            String idealWeight = dsVisit.Tables[0].Rows[0]["DefIdealWeight"].ToString();
            String prevWeight = dsVisit.Tables[0].Rows[0]["DefPrevWeight"].ToString();
            String height = dsVisit.Tables[0].Rows[0]["BMIHeight"].ToString();
            String firstVisitDate;
            String firstVisitDateType;

            if (Request.Cookies["VisitWeeksFlag"].Value == "4")
            {
                firstVisitDateType = "OperationDate";                
                startWeight = dsVisit.Tables[0].Rows[0]["StartWeight"].ToString();
            }
            else if (Request.Cookies["VisitWeeksFlag"].Value == "3")
            {
                firstVisitDateType = "ZeroDate";  
                startWeight = dsVisit.Tables[0].Rows[0]["StartWeight"].ToString();
            }
            else
            {
                firstVisitDateType = "FirstVisitDate";  
            }

            if (dsVisit.Tables[0].Rows[0][firstVisitDateType].ToString().Trim() == "")
                firstVisitDate = "";
            else
                firstVisitDate = gClass.TruncateDate(dsVisit.Tables[0].Rows[0][firstVisitDateType].ToString().Trim(), Request.Cookies["CultureInfo"].Value, 1); 

            String imperialFlag = dsVisit.Tables[0].Rows[0]["ImperialFlag"].ToString();
            String weight = dsVisit.Tables[0].Rows[0]["Weight"].ToString();

            String visitDate = gClass.TruncateDate(dsVisit.Tables[0].Rows[0]["VisitDate"].ToString().Trim(), Request.Cookies["CultureInfo"].Value, 1);


            Decimal Months = Convert.ToDecimal(dsVisit.Tables[0].Rows[0]["Months"].ToString() == ""? "0": dsVisit.Tables[0].Rows[0]["Months"].ToString());
            Int32 MonthGroup = 0;
            MonthGroup = CalculateMonthGroup(Months);

            strScript += "setBaselineData('" + startWeight + "','" + idealWeight + "','" + prevWeight + "','" + height + "','" + firstVisitDate + "','" + imperialFlag + "','" + weight + "','" + visitDate + "','" + MonthGroup.ToString() + "');";
        }
        cmdCommand.Dispose();
        dsVisit.Dispose();
        ScriptManager.RegisterStartupScript(btnCalculateWeightOtherData, btnCalculateWeightOtherData.GetType(), Guid.NewGuid().ToString(), strScript, true);
    }
    #endregion 

    #region protected void btnLoadBaselineComorbidity_OnClick
    /// <summary>
    /// this function is called when user clicks the VISIT COMORBIDITY checkbox in yello section.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnLoadBaselineComorbidity_OnClick(object sender, EventArgs e)
    {
        DataView dvBaseline = CheckBoldComorbidiyInBaseline();
    }
    #endregion

    #region protected void linkBtnSaveNotes_OnClick(object sender, EventArgs e)
    protected void linkBtnSaveNotes_OnClick(object sender, EventArgs e)
    {
        Int16 IsDoneSaveFlag = 0;
        String strScript = "";

        SqlCommand cmdSave = new SqlCommand();

        cmdSave_AddParameters(ref cmdSave);
        cmdSave.Parameters["@OrganizationCode"].Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSave.Parameters["@UserPracticeCode"].Value = Convert.ToInt32(Context.Request.Cookies["UserPracticeCode"].Value);
        cmdSave.Parameters["@PageNo"].Value = 2;
        cmdSave.Parameters["@PatientId"].Value = Convert.ToInt32(txtHPatientID.Value);
        cmdSave.Parameters["@Notes"].Value = txtPatientNotes.Text.Replace("'", "`");       

        try
        {
            gClass.SavePatientWeightData(cmdSave);
            Response.SetCookie(new HttpCookie("PatientID", Context.Request.Cookies["PatientID"].Value));
            gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value, Request.Cookies["Logon_UserName"].Value, Request.Url.Host,
                        "Baseline Form", 2, "Modify Height/Weight/Notes data", "PatientCode", Context.Request.Cookies["PatientID"].Value);

            gClass.SaveActionLog(gClass.OrganizationCode,
                     Request.Cookies["UserPracticeCode"].Value,
                     Request.Url.Host,
                     System.Configuration.ConfigurationManager.AppSettings["VisitPage"].ToString(),
                     System.Configuration.ConfigurationManager.AppSettings["LogUpdate"].ToString(),
                     "Save " + System.Configuration.ConfigurationManager.AppSettings["PatientWeightData"].ToString(),
                     Request.Cookies["PatientID"].Value,
                     "");

            IsDoneSaveFlag = 1;
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host,
                                    Request.Cookies["Logon_UserName"].Value, "Baseline", "Data saving height/weight/notes", err.ToString());
            IsDoneSaveFlag = 0;
        }

        cmdSave.Dispose();

        switch (IsDoneSaveFlag)
        {
            case 1: // Saving data procedure is done sucessfully
                strScript = "  ShowDivMessage('The information has been saved...', true);";
                strScript += "SetEvents();";
                break;
            case 0: // Saving data procedure is done sucessfully
                strScript = "  ShowDivMessage('Error in save data ...', true);";
                strScript += "SetEvents();";
                break;
        }
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), Guid.NewGuid().ToString(), strScript, true);
    }
    #endregion 

    #region private void cmdSave_AddParameters(ref SqlCommand cmdSave)
    /// <summary>
    /// this function is to initialize the SqlCommand .
    /// all parameters of ths cmdSave, are used in sub-pages' fields except DEMOGRAPHICS sub-page.
    /// Data of DEMOGRAPHICS are saveed into tblPatients,
    /// other data of othere sub-pages are saved into tblPatientWeightData (PWD)
    /// </summary>
    /// <param name="cmdSave">Sql Command to initialize.</param>
    private void cmdSave_AddParameters(ref SqlCommand cmdSave)
    {
        gClass.MakeStoreProcedureName(ref cmdSave, "sp_PatientWeightData_cmdUpdate", true);

        cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int);
        cmdSave.Parameters.Add("@UserPracticeCode", SqlDbType.Int);
        cmdSave.Parameters.Add("@PatientId", SqlDbType.Int);
        cmdSave.Parameters.Add("@PageNo", SqlDbType.Int);
        // PAGE 2
        cmdSave.Parameters.Add("@Height", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@StartWeight", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@IdealWeight", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@CurrentWeight", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@losttofollowup", SqlDbType.Bit);
        cmdSave.Parameters.Add("@StartBMIWeight", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@BMIHeight", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@BaseHypertensionProblems", SqlDbType.Bit);
        cmdSave.Parameters.Add("@BaseBloodPressureRxDetails", SqlDbType.VarChar, 255);
        cmdSave.Parameters.Add("@BaseSystolicBP", SqlDbType.Int);
        cmdSave.Parameters.Add("@BaseDiastolicBP", SqlDbType.Int);
        cmdSave.Parameters.Add("@BaseLipidProblems", SqlDbType.Bit);
        cmdSave.Parameters.Add("@BaseLipidRxDetails", SqlDbType.VarChar, 255);
        cmdSave.Parameters.Add("@BaseTriglycerides", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@BaseTotalCholesterol", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@BaseHDLCholesterol", SqlDbType.Decimal);

        cmdSave.Parameters.Add("@BaseDiabetesProblems", SqlDbType.Bit);
        cmdSave.Parameters.Add("@BaseDiabetesRxDetails", SqlDbType.VarChar, 255);

        cmdSave.Parameters.Add("@LastImageDate", SqlDbType.DateTime);
        cmdSave.Parameters.Add("@LastImageLocation", SqlDbType.VarChar, 150);
        cmdSave.Parameters.Add("@BMI", SqlDbType.Decimal);

        cmdSave.Parameters.Add("@StartNeck", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@StartWaist", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@StartHip", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@StartPR", SqlDbType.Int);
        cmdSave.Parameters.Add("@StartRR", SqlDbType.Int);
        cmdSave.Parameters.Add("@StartBP1", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@StartBP2", SqlDbType.Decimal);
        // PAGE 3
        cmdSave.Parameters.Add("@BaseBMR", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@BaseImpedance", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@BaseFatPerCent", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@BaseFreeFatMass", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@BaseTotalBodyWater", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@BaseHomocysteine", SqlDbType.Int);
        cmdSave.Parameters.Add("@BaseTSH", SqlDbType.Int);
        cmdSave.Parameters.Add("@BaseT4", SqlDbType.Int);
        cmdSave.Parameters.Add("@BaseT3", SqlDbType.Int);
        cmdSave.Parameters.Add("@BaseHBA1C", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@BaseFSerumInsulin", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@BaseFBloodGlucose", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@BaseIron", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@BaseFerritin", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@BaseTransferrin", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@BaseIBC", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@BaseFolate", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@BaseB12", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@BaseHemoglobin", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@BasePlatelets", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@BaseWCC", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@BaseCalcium", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@BasePhosphate", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@BaseVitD", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@BaseBilirubin", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@BaseTProtein", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@BaseAlkPhos", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@BaseALT", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@BaseAST", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@BaseGGT", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@BaseAlbumin", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@BaseSodium", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@BasePotassium", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@BaseChloride", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@BaseBicarbonate", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@BaseUrea", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@BaseCreatinine", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@BaseFatMass", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@BaseLDLCholesterol", SqlDbType.Decimal);

        // PAGE 4
        cmdSave.Parameters.Add("@BaseUserField1", SqlDbType.VarChar, 25);
        cmdSave.Parameters.Add("@BaseUserField2", SqlDbType.VarChar, 25);
        cmdSave.Parameters.Add("@BaseUserField3", SqlDbType.VarChar, 25);
        cmdSave.Parameters.Add("@BaseUserField4", SqlDbType.VarChar, 25);
        cmdSave.Parameters.Add("@BaseUserField5", SqlDbType.VarChar, 25);
        cmdSave.Parameters.Add("@BaseUserMemoField1", SqlDbType.VarChar, 1024);
        cmdSave.Parameters.Add("@BaseUserMemoField2", SqlDbType.VarChar, 1024);

        // PAGE 5
        cmdSave.Parameters.Add("@BaseAsthmaProblems", SqlDbType.Bit);
        cmdSave.Parameters.Add("@BaseAsthmaLevel", SqlDbType.VarChar, 2);
        cmdSave.Parameters.Add("@BaseAsthmaDetails", SqlDbType.VarChar, 255);

        cmdSave.Parameters.Add("@BaseRefluxProblems", SqlDbType.Bit);
        cmdSave.Parameters.Add("@BaseRefluxLevel", SqlDbType.VarChar, 2);
        cmdSave.Parameters.Add("@BaseRefluxDetails", SqlDbType.VarChar, 255);

        cmdSave.Parameters.Add("@BaseSleepProblems", SqlDbType.Bit);
        cmdSave.Parameters.Add("@BaseSleepLevel", SqlDbType.VarChar, 2);
        cmdSave.Parameters.Add("@BaseSleepDetails", SqlDbType.VarChar, 255);

        cmdSave.Parameters.Add("@BaseFertilityProblems", SqlDbType.Bit);
        cmdSave.Parameters.Add("@BaseFertilityDetails", SqlDbType.VarChar, 255);
        cmdSave.Parameters.Add("@BaseArthritisProblems", SqlDbType.Bit);
        cmdSave.Parameters.Add("@BaseArthritisDetails", SqlDbType.VarChar, 255);
        cmdSave.Parameters.Add("@BaseIncontinenceProblems", SqlDbType.Bit);
        cmdSave.Parameters.Add("@BaseIncontinenceDetails", SqlDbType.VarChar, 255);
        cmdSave.Parameters.Add("@BaseBackProblems", SqlDbType.Bit);
        cmdSave.Parameters.Add("@BaseBackDetails", SqlDbType.VarChar, 255);
        cmdSave.Parameters.Add("@BaseCVDProblems", SqlDbType.Bit);
        cmdSave.Parameters.Add("@BaseCVDDetails", SqlDbType.VarChar, 255);
        cmdSave.Parameters.Add("@BaseOtherDetails", SqlDbType.VarChar, 255);

        cmdSave.Parameters.Add("@BaseFertilityLevel", SqlDbType.VarChar, 2);
        cmdSave.Parameters.Add("@BaseArthritisLevel", SqlDbType.VarChar, 2);
        cmdSave.Parameters.Add("@BaseIncontinenceLevel", SqlDbType.VarChar, 2);
        cmdSave.Parameters.Add("@BaseBackPainLevel", SqlDbType.VarChar, 2);
        cmdSave.Parameters.Add("@BaseCVDLevel", SqlDbType.VarChar, 2);

        // Page 6
        cmdSave.Parameters.Add("@Notes", SqlDbType.VarChar, 2048);

        // Initialize Parameters
        gClass.InitialParameters(ref cmdSave);
        return;
    }
    #endregion
    
    #region protected void btnDeleteVisit_onserverclick(object sender, EventArgs e)
    /*
     * this function is to save visit data (normal and comorbidity visit) and after saving data, the visits list is reloaded
     */
    protected void btnDeleteVisit_onserverclick(object sender, EventArgs e)
    {
        if (txtHDelete.Value == "1")
        {
            ProgressNote_DeleteProc();
            LoadAllVisits("");
        }
    }
    #endregion

    #region private void FindControlRecursive(Control root, string id)
    private Control FindControlRecursive(Control root, string id)
    {
        if (root.ID == id)
        {
            return root;
        }

        foreach (Control c in root.Controls)
        {
            Control t = FindControlRecursive(c, id);
            if (t != null)
            {
                return t;
            }
        }
        return null;
    }
    #endregion

    #region protected void ProgressNote_DeleteProc()
    protected void ProgressNote_DeleteProc()
    {
        
            SqlCommand cmdUpdate = new SqlCommand();
            string strScript = String.Empty;
            string visitCommentSP = "";
            string visitComment = "";
            string visitCommentValue = "";
            string eventType = "";
            string visitType = "";

            if (chkCommentOnly.Checked == false)
            {
                visitCommentSP = "sp_ConsultFU1_ProgressNotes_DeleteData";
                visitComment = "@ConsultId";
                visitCommentValue = txtHConsultID.Value;
                eventType = "V";
                visitType = System.Configuration.ConfigurationManager.AppSettings["VisitVisit"].ToString();
            }
            else
            {
                visitCommentSP = "sp_ConsultFU1_ProgressNotesComment_DeleteData";
                visitComment = "@CommentId";
                visitCommentValue = txtHCommentID.Value;
                eventType = "C";
                visitType = System.Configuration.ConfigurationManager.AppSettings["VisitComment"].ToString();
            }

            gClass.MakeStoreProcedureName(ref cmdUpdate, visitCommentSP, true);
            cmdUpdate.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
            cmdUpdate.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
            cmdUpdate.Parameters.Add(visitComment, SqlDbType.Int).Value = Convert.ToInt32(visitCommentValue);

            cmdUpdate.Parameters.Add("@DeletedByUser", SqlDbType.VarChar, 50).Value = Context.Request.Cookies["Logon_UserName"].Value;
            cmdUpdate.Parameters.Add("@DateDeleted", SqlDbType.DateTime).Value = Convert.ToDateTime(txtHCurrentDate.Value);

            try
            {
                gClass.ExecuteDMLCommand(cmdUpdate);
                gClass.SaveUserLogFile(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Cookies["Logon_UserName"].Value,
                                        Context.Request.Url.Host, "Visit Form", 2, "Delete Visit BOLD Data", "Visit Code " + visitCommentSP + " - " + visitComment, txtHConsultID.Value);
                
                gClass.SaveActionLog(gClass.OrganizationCode,
                                     Request.Cookies["UserPracticeCode"].Value,
                                     Request.Url.Host,
                                     System.Configuration.ConfigurationManager.AppSettings["VisitPage"].ToString(),
                                     System.Configuration.ConfigurationManager.AppSettings["LogDelete"].ToString(),
                                     "Delete " + visitType,
                                     Request.Cookies["PatientID"].Value,
                                     visitCommentValue);

                UpdatePatientVisitDate();
                DeleteVisitDocument(Convert.ToInt32(visitCommentValue),eventType);
                //strScript = "javascript:SetEvents();UpdateOtherFieldsBasedOnSelectedSurgeryType();ClearFields();controlBar_Buttons_OnClick(1);";
                // strScript += "linkBtnSave_CallBack(true);";
                strScript = "javascript:ProgressNotes_ClearFields;";
            }
            catch (Exception err)
            {
                gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                            Context.Request.Cookies["Logon_UserName"].Value, "Visit PID : " + Context.Request.Cookies["PatientID"].Value, "Data deleting Visit - VisitOperationProc function " + visitCommentSP + " - " + visitComment, err.ToString());
                //strScript = "javascript:linkBtnSave_CallBack(false);";
            }

            ScriptManager.RegisterStartupScript(btnDeleteVisit, btnDeleteVisit.GetType(), Guid.NewGuid().ToString(), strScript, true);
        return;
    }
    #endregion
        
    #region protected void updatePatientVisitDate()
    protected void UpdatePatientVisitDate()
    {
        SqlCommand cmdUpdate = new SqlCommand();

        gClass.MakeStoreProcedureName(ref cmdUpdate, "sp_PatientData_UpdateVisitDate", true);
        cmdUpdate.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdUpdate.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
        try
        {
            gClass.ExecuteDMLCommand(cmdUpdate);
            gClass.SaveUserLogFile(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Cookies["Logon_UserName"].Value,
                                    Context.Request.Url.Host, "Visit Form", 2, "Update Patient Visit Date", "Patient ID", txtHPatientID.Value);

            gClass.SaveActionLog(gClass.OrganizationCode,
                     Request.Cookies["UserPracticeCode"].Value,
                     Request.Url.Host,
                     System.Configuration.ConfigurationManager.AppSettings["VisitPage"].ToString(),
                     System.Configuration.ConfigurationManager.AppSettings["LogUpdate"].ToString(),
                     "Save " + System.Configuration.ConfigurationManager.AppSettings["VisitDate"].ToString(),
                     txtHPatientID.Value,
                     "");
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                        Context.Request.Cookies["Logon_UserName"].Value, "Patient ID : " + Context.Request.Cookies["PatientID"].Value, "Data Updating Patient VisitDate - VisitOperationProc function", err.ToString());
        }
        return;
    }
    #endregion

    #region protected void DeleteVisitDocument(Int32 visitCommentValue, string eventType)
    protected void DeleteVisitDocument(Int32 visitCommentValue, string eventType)
    {
        SqlCommand cmdDelete = new SqlCommand();

        gClass.MakeStoreProcedureName(ref cmdDelete, "sp_FileManagement_DeleteDocumentByEventID", true);
        cmdDelete.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdDelete.Parameters.Add("@EventId", SqlDbType.Int).Value = Convert.ToInt32(visitCommentValue);
        cmdDelete.Parameters.Add("@EventLink", SqlDbType.VarChar, 10).Value = eventType;
        cmdDelete.Parameters.Add("@DeleteAction", SqlDbType.Bit).Value = true;

        try
        {
            gClass.ExecuteDMLCommand(cmdDelete);
            gClass.SaveUserLogFile(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Cookies["Logon_UserName"].Value,
                                    Context.Request.Url.Host, "Visit Form", 2, "Delete Document by Event", "Consult ID", txtHConsultID.Value);
            
            gClass.SaveActionLog(gClass.OrganizationCode,
                                 Request.Cookies["UserPracticeCode"].Value,
                                 Request.Url.Host,
                                 System.Configuration.ConfigurationManager.AppSettings["VisitPage"].ToString(),
                                 System.Configuration.ConfigurationManager.AppSettings["LogDelete"].ToString(),
                                 "Delete " + System.Configuration.ConfigurationManager.AppSettings["DocumentPage"].ToString() + " Data",
                                 Request.Cookies["PatientID"].Value,
                                 txtHConsultID.Value);

        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                        Context.Request.Cookies["Logon_UserName"].Value, "Patient ID : " + Context.Request.Cookies["PatientID"].Value, "Deleting Document by Event - VisitOperationProc function", err.ToString());
        }
        return;
    }
    #endregion

    #region  static UppercaseFirst(String s)
    static string UppercaseFirst(String s)
    {
        // Check for empty string.
        if (string.IsNullOrEmpty(s))
        {
            return string.Empty;
        }
        // Return char and concat substring.
        return char.ToUpper(s[0]) + s.Substring(1);
    }
    #endregion
    
    #region private Boolean ValidateBOLDPatientVisit()
    private Boolean ValidateBOLDPatientVisit()
    {
        string strScript = "";

        SqlCommand cmdSelect = new SqlCommand();
        DataSet dsVisit = new DataSet();
        DataSet dsInitCom = new DataSet();
        DateTime dateSeen = DateTime.MinValue;
        DateTime dbdateSeen = DateTime.MinValue;
        DateTime firstOperationAdmit = DateTime.MinValue;
        DateTime firstOperationDischarge = DateTime.MinValue;
        DateTime lastVisitDate = DateTime.MinValue;
        String consultID = "";
        Boolean returnVal = true;
        Decimal decResult = 0;
        Boolean initCom = true;


        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_SRCInitComorbidityCheck", false);
        cmdSelect.Parameters.Add("@vintOrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSelect.Parameters.Add("@vintPatientId", System.Data.SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
        dsInitCom = gClass.FetchData(cmdSelect, "tblInitialComorbidity");

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_SRCVisitCheck", false);
        cmdSelect.Parameters.Add("@vintConsultId", SqlDbType.Int).Value = Convert.ToInt32(txtHConsultID.Value);

        dsVisit = gClass.FetchData(cmdSelect, "tblVisit");



        try { dateSeen = Convert.ToDateTime(gClass.TruncateDate(txtDateSeen_PN.Text.Trim(), Request.Cookies["CultureInfo"].Value, 1)); }
        catch { }
        try { firstOperationAdmit = Convert.ToDateTime(gClass.TruncateDate(dsVisit.Tables[0].Rows[0]["AdmitDate"].ToString().Trim(), Request.Cookies["CultureInfo"].Value, 1)); }
        catch { }
        try { firstOperationDischarge = Convert.ToDateTime(gClass.TruncateDate(dsVisit.Tables[0].Rows[0]["DischargeDate"].ToString().Trim(), Request.Cookies["CultureInfo"].Value, 1)); }
        catch { }
        try { lastVisitDate = Convert.ToDateTime(gClass.TruncateDate(dsVisit.Tables[0].Rows[0]["LastVisitDate"].ToString().Trim(), Request.Cookies["CultureInfo"].Value, 1)); }
        catch { }
        try { dbdateSeen = Convert.ToDateTime(gClass.TruncateDate(dsVisit.Tables[0].Rows[0]["DBVisitDate"].ToString().Trim(), Request.Cookies["CultureInfo"].Value, 1)); }
        catch { }
        try { consultID = dsVisit.Tables[0].Rows[0]["ConsultID"].ToString().Trim(); }
        catch { }


        BoldErrorMsg += !Decimal.TryParse(dsVisit.Tables[0].Rows[0]["InitHeight"].ToString().Trim(), out decResult) || decResult == 0? "<br/>- Height can not be empty. Please fill it in on Measurement, under Patient Details or Medical Record tab" : "";
        BoldErrorMsg += chkComorbidity.Checked == false? "<br/>- Comorbidity should be reviewed on every visit. Please tick on Review Comorbidity" : "";

            //preop: BoldErrorMsg += dateSeen < lastVisitDate ? "<br/>- Visits must be added sequentially. Visit date is before prior preoperative visits" : "";
            //postop: 

        if (dbdateSeen != DateTime.MinValue)
        {
            //on editting visit
            //determine preop or postop visit
            if (dbdateSeen <= firstOperationAdmit)
            {
                //preop visit
                VisitType = "preop";
                //can not occur after first operation admit
                BoldErrorMsg += dateSeen > firstOperationAdmit ? "<br/>- Preoperative visits must occur before facility stays. Visit date is after subsequent facility stay" : "";
            }
            else
            {
                //postop visit
                VisitType = "postop";
                //can not occur before first operation discharge
                BoldErrorMsg += dateSeen <= firstOperationDischarge ? "<br/>- Postoperative visit date has to be on or after facility stay discharge date" : "";
            }
        }
        else
        {
            //on adding visit
            if (firstOperationAdmit == DateTime.MinValue)
            {
                //preop visit
                VisitType = "preop";
                BoldErrorMsg += dateSeen < lastVisitDate ? "<br/>- Visits must be added sequentially. Visit date is before prior preoperative visits" : "";
            }
            else
            {
                //postop visit
                VisitType = "postop";

                BoldErrorMsg += dateSeen < lastVisitDate ? "<br/>- Visits must be added sequentially. Visit date is before prior postoperative visits" : "";
                BoldErrorMsg += dateSeen <= firstOperationDischarge ? "<br/>- Postoperative visit date has to be on or after facility stay discharge date" : "";
            }
        }


        

        if (dsInitCom.Tables.Count > 0 && (dsInitCom.Tables[0].Rows.Count > 0))
        {
            foreach (DataRow drVisit in dsInitCom.Tables[0].Rows)
                foreach (DataColumn dcVisit in dsInitCom.Tables[0].Columns)
                {
                    if (drVisit[dcVisit].ToString() == "" && initCom == true)
                    {
                        BoldErrorMsg += "<br/>- Patient must have a complete initial comorbidity review. Please fill it in on Comorbidity, under Patient Details or Medical Record tab";
                        initCom = false;
                    }
                }            
        }
        else
            BoldErrorMsg += "<br/>- Patient must have an initial comorbidity review. Please fill it in on Comorbidity, under Patient Details or Medical Record tab";
        
        if (BoldErrorMsg != "")
        {
            BoldErrorMsg = "<br/>Please check the following: " + BoldErrorMsg;

            strScript = "document.getElementById('divErrorMessage').style.display = 'block';";
            strScript += "document.getElementById('pErrorMessage').innerHTML = 'Error in save data ..." + BoldErrorMsg + "';";
            strScript += "SetEvents();HideDivMessage();CheckComorbidityVisit($get(\"chkComorbidity\"));";

            ScriptManager.RegisterStartupScript(updatePanelList, btnAddVisit.GetType(), "key", strScript, true);

            returnVal = false;
        }

        return returnVal;
    }
    #endregion

    #region private void SubmitBOLDPatientVisit(Int32 intConsultID)
    private void SubmitBOLDPatientVisit(Int32 intConsultID)
    {
        Int32 intPatientID;
        Int32.TryParse(Request.Cookies["PatientID"].Value.ToString(), out intPatientID);
        try
        {
            //save to BOLD----------------------------------------------------------------------------------------        
            Lapbase.Business.SRCObject objSRC = new Lapbase.Business.SRCObject();

            objSRC.PatientID = intPatientID;
            objSRC.OrganizationCode = base.OrganizationCode;
            objSRC.Imperial = base.Imperial;
            objSRC.VendorCode = LapbaseConfiguration.SRCVendorCode;
            objSRC.PracticeCEO = LapbaseConfiguration.PracticeCEOCode;
            objSRC.SurgeonCEO = LapbaseConfiguration.SurgeonCEOCode;
            objSRC.FacilityCEO = LapbaseConfiguration.FacilityCEOCode;
            objSRC.SRCUserName = LapbaseConfiguration.SRCUserName;
            objSRC.SRCPassword = LapbaseConfiguration.SRCPassword;

            if (VisitType == "preop")
                objSRC.SavePatientPreOpVisit(intConsultID);
            else
                objSRC.SavePatientPostOpVisit(intConsultID);

            if (objSRC.PreOperativeVisitErrors.Count > 0 || objSRC.PostOperativeVisitErrors.Count > 0)
            {
                if (objSRC.PreOperativeVisitErrors.Count > 0)
                {
                    for (int Xh = 0; Xh < objSRC.PreOperativeVisitErrors.Count; Xh++)
                    {
                        gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                                          Context.Request.Cookies["Logon_UserName"].Value, "Patient ID : " + intPatientID.ToString() + ", Consult ID:" + intConsultID.ToString(), "BOLD - Data saving PreOp Visit ", objSRC.PreOperativeVisitErrors[Xh].ErrorMessage.ToString());
                    }
                }

                if (objSRC.PostOperativeVisitErrors.Count > 0)
                {
                    for (int Xh = 0; Xh < objSRC.PostOperativeVisitErrors.Count; Xh++)
                    {
                        gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                                          Context.Request.Cookies["Logon_UserName"].Value, "Patient ID : " + intPatientID.ToString() + ", Consult ID:" + intConsultID.ToString(), "BOLD - Data saving PostOp Visit ", objSRC.PostOperativeVisitErrors[Xh].ErrorMessage.ToString());
                    }
                }
            }
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                        Context.Request.Cookies["Logon_UserName"].Value, "Patient ID : " + intPatientID.ToString() + ", Consult ID:" + intConsultID.ToString(), "BOLD - Data saving Visit ", err.ToString());
        }
        //----------------------------------------------------------------------------------------------------
    }
    #endregion
    
    #region private void RemoveFirstItemFromDropDownList()
    private void RemoveFirstItemFromDropDownList()
    {
        FillBoldList(cmbMedication, listMedication);
    }
    #endregion

    #region private void FillBoldList(UserControl_SystemCodeWUCtrl ddlSource, HtmlSelect listDest)
    private void FillBoldList(UserControl_SystemCodeWUCtrl ddlSource, HtmlSelect listDest)
    {
        if (listDest.Items.Count == 0)
        {
            foreach (ListItem sourceItem in ddlSource.Items)
            {
                ListItem destItem = new ListItem();
                destItem.Value = sourceItem.Value;
                destItem.Text = sourceItem.Text;
                listDest.Items.Add(destItem);
            }
        }
    }
    #endregion

    #region private void FillSelectedLists(HtmlSelect listOrigin, HtmlSelect listSelected, String strListValue)
    private void FillSelectedLists(UserControl_SystemCodeWUCtrl listOrigin, HtmlSelect listSelected, String strListValue)
    {
        Int32 idx = strListValue.IndexOf(";");
        listSelected.Items.Clear();
        listOrigin.FetchSystemCodeListData();
        while (idx > 0)
        {
            String strTempValue = strListValue.Substring(0, idx);
            strListValue = strListValue.Substring(idx + 1);
            idx = strListValue.IndexOf(";");

            foreach (ListItem item in listOrigin.Items)
                if (item.Value.Equals(strTempValue)) listSelected.Items.Add(item);
        }
    }
    #endregion

    #region private void LoadLogoData(string section)
    private void LoadLogoData(string section)
    {
        SqlCommand cmdSelect = new SqlCommand();
        DataSet dsLogo = new DataSet();

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_Logos_LoadData", true);
        cmdSelect.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        dsLogo = gClass.FetchData(cmdSelect, "tblLogo");

        string url = "";
        url = HttpContext.Current.Request.Url.AbsoluteUri;

        if ((dsLogo.Tables[0].Rows.Count <= 0))
        {
            rowLogoLetterDoctor.Style["display"] = "none";
            rowLogoFollowup.Style["display"] = "none";
            rowLogoSuperBill.Style["display"] = "none";
            txtHLogoMandatory.Value = "0";
        }
        else if (url.ToUpper().IndexOf("AIGB") >= 0 && dsLogo.Tables[0].Rows.Count > 0)
        {
            //AIGB Setting
            txtHLogoMandatory.Value = "1";
        }

    }
    #endregion

    #region private int CalculateMonthGroup(Decimal Months)
    private int CalculateMonthGroup(Decimal Months)
    {
        //classified it in 6 months, 12 months, or anually
        //the window will be: 
        /*  6 months can be 5 - 8 months,
            12 months: 9 – 15 months
            2 years:  18 – 29 months
            And then onwards annually with the window being 6 months before to 6 months after.*/

        Int32 MonthReminder = 0;
        Int32 MonthGroup = 0;

        if (Months >= 5 && Months <= 8)
        {
            MonthGroup = 6;
        }
        else if (Months >= 9 && Months <= 15)
        {
            MonthGroup = 12;
        }
        else if (Months >= 18)
        {
            MonthGroup = Convert.ToInt32(Math.Floor(Months / 12)) * 12;
            MonthReminder = Convert.ToInt32(Math.Floor(Months % 12));
            if (MonthReminder > 5)
            {
                MonthGroup = MonthGroup + 12;
            }
        }
        return MonthGroup;
    }
    #endregion

    #region private void LoadPatientData()
    private void LoadPatientData()
    {
        SqlCommand cmdSelect = new SqlCommand();
        DataSet dsPatient = new DataSet();
        DateTime dtDiabetesPeriodMin;
        DateTime dtDiabetesPeriodMax;
        try
        {
            gClass.MakeStoreProcedureName(ref cmdSelect, "sp_PatientData_LoadData", true);
            cmdSelect.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
            cmdSelect.Parameters.Add("@UserPracticeCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
            cmdSelect.Parameters.Add("@PatientID", System.Data.SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
            if (Request.Cookies["PatientCustomID"] != null)
                cmdSelect.Parameters.Add("@Patient_CustomID", System.Data.SqlDbType.VarChar, 20).Value = Request.Cookies["PatientCustomID"].Value;

            dsPatient = gClass.FetchData(cmdSelect, "tblPatient");
            txtHBaselineOperationDate.Value = dsPatient.Tables[0].Rows[0]["LapBandDate"].ToString();

            if (dsPatient.Tables[0].Rows[0]["LapBandDate"].ToString().Trim() != "")
            {
                dtDiabetesPeriodMin = Convert.ToDateTime(dsPatient.Tables[0].Rows[0]["LapBandDate"].ToString().Trim()).AddMonths(11);
                dtDiabetesPeriodMax = Convert.ToDateTime(dsPatient.Tables[0].Rows[0]["LapBandDate"].ToString().Trim()).AddMonths(14);

                txtHDiabetesPeriodMin.Value = dtDiabetesPeriodMin.ToShortDateString();
                txtHDiabetesPeriodMax.Value = dtDiabetesPeriodMax.ToShortDateString();
            }
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Patient Visit Form", "Loading Patient Data - LoadPatientData function", err.ToString());
        }
    }
    #endregion

    #region protected void btnLoadAllVisit_OnClick(object sender, EventArgs e)
    protected void btnLoadAllVisit_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("~/Forms/PatientsVisits/ConsultFU1/ConsultFU1Form.aspx?QSN=PID&QSV=" + Request.Cookies["PatientID"].Value + "&LP=N", false);
    }
    #endregion
}

