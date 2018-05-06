using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Globalization;
using System.Transactions;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.Web.UI;
using Lapbase.Business;
using Telerik.WebControls;
using NHapi.Base.Model;
using NHapi.Base.Parser;
using NHapi.Base;
using NHapi.Model.V23;
using NHapi.Model.V23.Message;
using NHapi.Model.V23.Segment;
using Lapbase.Configuration.ConfigurationApplication;

public partial class Forms_EMR_EMRForm : BasePage
{
    Int16 IsDoneSaveFlag = 0;
    Int32 int32Temp = 0;
    Decimal decTemp = 0;
    String strNumberFormat = System.Configuration.ConfigurationManager.AppSettings["NumberFormat"].ToString();
    String BoldErrorMsg = "";

    GlobalClass gClass = new GlobalClass();

    String[,] DemographicStructure = new String[,] { { "rbDemographicFamilyStructure", "Details_FamilyStructure" } };

    String[,] RegistryData = new String[,] { { "rdRegistrySleepApnea_", "Registry_SleepApnea" },{ "rdRegistryGerd_", "Registry_Gerd" },
        { "rdRegistryHyperlipidemia_", "Registry_Hyperlipidemia" },{ "rdRegistryHypertension_", "Registry_Hypertension" },{ "rdRegistryDiabetes_", "Registry_Diabetes" }};

    String[,] WeightHistory = new String[,] { { "rdWeightHistoryHistoryIndicateBirth_", "WeightHistory_WeightBirth" },{ "rdWeightHistoryHistoryIndicateStart_", "WeightHistory_WeightStartSchool" },{ "rdWeightHistoryHistoryIndicateHighStart_", "WeightHistory_WeightStartHighSchool" },
        { "rdWeightHistoryHistoryIndicateHighEnd_", "WeightHistory_WeightEndHighSchool" },{ "rdWeightHistoryHistoryIndicateWork_", "WeightHistory_WeightWork" },{ "rdWeightHistoryHistoryIndicateMarriage_", "WeightHistory_WeightMarriage" }};
    
    String[] WeightHistoryList = new String[] { "WeightHistory_TryMethod", "WeightHistory_GroupList", "WeightHistory_PillList", "WeightHistory_AdviceList", "WeightHistory_DietList", "WeightHistory_Other", "WeightHistory_TreatmentList" };

    String[,] SpecialInvestigation = new String[,] { { "rdSpecialInvestigationSS", "SpecialInvestigation_ActionSS" }, 
    { "rdSpecialInvestigationGE", "SpecialInvestigation_ActionGE" },{"rdSpecialInvestigationSP","SpecialInvestigation_ActionSP"},
    {"rdSpecialInvestigationNA","SpecialInvestigation_ActionNA"},{"rdSpecialInvestigationPS","SpecialInvestigation_ActionPS"},
    {"rdSpecialInvestigationOA","SpecialInvestigation_ActionOA"}};
                                                
    String[,] Investigation = new String[,] { { "rdInvestigationRBS", "Investigation_ActionRBS" }, 
    { "rdInvestigationABS", "Investigation_ActionABS" },{ "rdInvestigationRFT", "Investigation_ActionRFT" },
    { "rdInvestigationABG", "Investigation_ActionABG" },{ "rdInvestigationEET", "Investigation_ActionEET" },
    { "rdInvestigationBM", "Investigation_ActionBM" }, { "rdInvestigationEMS", "Investigation_ActionEMS" },
    { "rdInvestigationP", "Investigation_ActionP" }, { "rdInvestigationEKG", "Investigation_ActionEKG" },
    { "rdInvestigationCX", "Investigation_ActionCX" }};
    
    String[] Background = new String[] { "Background_Diabetes", "Background_HeartDisease", "Background_Hypertension", "Background_Gout", "Background_Obesity", "Background_Snoring", "Background_Asthma", "Background_HighCholesterol" };

    String[] Exam = new String[] { "GAO", "HNG", "HNH", "HNM", "HNN", "CH", "CP", "AA", "LSR", "LSB", "LSA", "MSO", "MF" };

    String[,] Allergy = new String[,] { { "rdAlergyAlcoholDoYouDrink_", "Allergy_HaveDrink" }, { "rdAlergySmokeDoYouSmoke_", "Allergy_Smoke" } };
    
    String[,] AllergyRadio = new String[,] { { "rdAllergy", "Allergy_HaveAllergy" }, { "rdMedication", "Allergy_HaveMedication" },
    {"rdLatex","Allergy_Latex"}, {"rdBleedExcess","Allergy_ExcessBleed"}, {"rdAllergyDrugsDoYou","Allergy_DoDrugs"}};
    
    String[,] Template = new String[,] { { "templateDemographic", "emrdemographic" }, 
        { "templateMeasurement", "emrmeasurement" }, { "templateComplaint", "emrcomplaint" }, { "templateComorbidities", "emrcomorbidities" },
        { "templateMedications", "emrmedications" }, { "templateWeightLossHistory", "emrweightlosshistory" } ,{ "templatePastFamilyHistory", "emrpastfamilyhistory" },
        { "templateAllergies", "emrallergies" } ,{ "templatePhysicalExamination", "emrphysicalexam" } ,{ "templateLabs", "emrlabs" },
        { "templateInvestigationsRefferals", "emrinvestigations" } ,{ "templateManagementPlan", "emrmanagementplan" }};

    Boolean savePatientData = false;
    Boolean savePatientWeightData = false;
    Boolean saveBoldData = false;
    Boolean saveEMRData = false;
    Boolean saveComorbidityData = false;
    Boolean saveComorbidityCheckData = false;

    #region protected void Page_Load
    /*
     * this function is to initialize the page when it's being loaded,
     * 1) check user has logined and is permitted to access to this page
     * 2) because this page is called from Patient list by 2 ways, Add new patient that "PID" in QueryString is "0"
     *    click on a particular patient (on patinet list), the "PID" is the PatientID and should be kept as a Cookie to use during runtime
     * 3) fetches data and fills dropdownlists such as Title, Gender, Doctors, Referring Doctors, Race, City and Insurance
     * 4) based on System detail for current user and Imeprial flag, set the proper weight measurment 
     *    Imperial flag == 1, the weight measurment is "lb", height measurment is "inch"
     *    Imperial flag == 0, the weight measurment is "kg", height measurment is "cm"
     *    Settng BMI refrence value
     * 5) by using selected languge, set the page direction "Right To Left" and "Left To Right"
     *    setting DATE format such as "dd/mm/yyyy", "mm/dd/yyyy"
     */
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            gClass.OrganizationCode = Request.Cookies["OrganizationCode"].Value;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            bodyPatientData.Attributes.Add("onload", "javascript:InitializePage();");
            RegisterClientScript();
            if (txtPatientID.Value == "0" || txtPatientID.Value == "")
                txtPatientID.Value = (Request.QueryString.Count > 0) ? Request.QueryString["PID"].ToString() : Request.Cookies["PatientID"].Value;
            Response.Cookies["PatientID"].Value = txtPatientID.Value;

            
            //split feature
            string Feature = Request.Cookies["Feature"].Value;
            string[] Features = Feature.Split(new string[] { "**" }, StringSplitOptions.None);

            string ShowDownloadExcelGraph = Features[2];

            if (ShowDownloadExcelGraph == "False" || ShowDownloadExcelGraph == "")
            {
                btnDownloadExcel.Style["display"] = "none";
            }

            if (!IsPostBack)
            {
                FillDropdownLists();
                txtHPermissionLevel.Value = Request.Cookies["PermissionLevel"].Value;
                txtHDataClamp.Value = Request.Cookies["DataClamp"].Value;

                if (txtPatientID.Value.Equals("0"))
                {
                    gClass.SaveUserLogFile(base.UserPracticeCode.ToString(),
                                            Request.Cookies["Logon_UserName"].Value,
                                            Request.Url.Host,
                                            "Baseline Form", 2, "Open Form to add new patient", "", "");

                    gClass.SaveActionLog(gClass.OrganizationCode,
                                         Request.Cookies["UserPracticeCode"].Value,
                                         Request.Url.Host,
                                         System.Configuration.ConfigurationManager.AppSettings["MedicalRecordPage"].ToString(),
                                         System.Configuration.ConfigurationManager.AppSettings["LogRead"].ToString(),
                                         "Open form to add a new patient",
                                         "",
                                         "");

                    LoadLastPatientCustomIDFromDatabase();
                    LoadDefault();
                }
                else
                {
                    gClass.SaveUserLogFile(base.UserPracticeCode.ToString(),
                                            Request.Cookies["Logon_UserName"].Value,
                                            Request.Url.Host,
                                            "Baseline Form", 2, "Open Form to see patient data", "PatientCode", txtPatientID.Value);

                    gClass.SaveActionLog(gClass.OrganizationCode,
                                         Request.Cookies["UserPracticeCode"].Value,
                                         Request.Url.Host,
                                         System.Configuration.ConfigurationManager.AppSettings["MedicalRecordPage"].ToString(),
                                         System.Configuration.ConfigurationManager.AppSettings["LogRead"].ToString(),
                                         "Load " + System.Configuration.ConfigurationManager.AppSettings["PatientPage"].ToString() + " Data",
                                         txtPatientID.Value,
                                         "");

                    LoadPatientData();
                }

                string url = Request.Url.Host;

                //ObseitySurgeryCA config
                if (url.ToUpper().IndexOf("OBESITYSURGERYCA") >= 0)
                {
                    CommWeightLoss.Visible = true;
                    CommDurationLoss.Visible = true;
                }

                if (Convert.ToBoolean(Request.Cookies["EMR"].Value) == false)
                {
                    li_Div3.Style["display"] = "none";
                    li_Div9.Style["display"] = "none";
                    li_Div11.Style["display"] = "none";
                    menurow2.Style["display"] = "none";
                }

                if (Request.Cookies["PermissionLevel"].Value == "1o" || Request.Cookies["PermissionLevel"].Value == "2t" || Request.Cookies["PermissionLevel"].Value == "3f" || Request.Cookies["PermissionLevel"].Value == "4s" || Request.Cookies["SubmitData"].Value.IndexOf("submitbold") >= 0)
                {
                    btnDeletePatient1.Style["display"] = "none";
                    btnDeletePatient2.Style["display"] = "none";
                    btnDeletePatient3.Style["display"] = "none";
                    btnDeletePatient5.Style["display"] = "none";
                    btnDeletePatient6.Style["display"] = "none";
                    btnDeletePatient8.Style["display"] = "none";
                    btnDeletePatient9.Style["display"] = "none";
                    btnDeletePatient10.Style["display"] = "none";
                    btnDeletePatient11.Style["display"] = "none";
                    btnDeletePatient12.Style["display"] = "none";
                    btnDeletePatient13.Style["display"] = "none";
                    btnDeletePatient16.Style["display"] = "none";
                    btnDeletePatient17.Style["display"] = "none";
                }
                if (Request.Cookies["PermissionLevel"].Value == "1o")
                {
                    btnSavePatient1.Style["display"] = "none";
                    btnSavePatient2.Style["display"] = "none";
                    btnSavePatient3.Style["display"] = "none";
                    btnSavePatient5.Style["display"] = "none";
                    btnSavePatient6.Style["display"] = "none";
                    btnSavePatient8.Style["display"] = "none";
                    btnSavePatient9.Style["display"] = "none";
                    btnSavePatient10.Style["display"] = "none";
                    btnSavePatient11.Style["display"] = "none";
                    btnSavePatient12.Style["display"] = "none";
                    btnSavePatient13.Style["display"] = "none";
                    btnSavePatient16.Style["display"] = "none";
                    btnSavePatient17.Style["display"] = "none";                     
                }
                if (Request.Cookies["SubmitData"].Value == "")
                {
                    btnSavePatient5.Style["display"] = "none";
                }
                bariatricDefault.Style["display"] = "none";
                bariatricBold.Style["display"] = "none";
                nonbariatricDefault.Style["display"] = "none";
                nonbariatricBold.Style["display"] = "none";
                comorbidityDefault.Style["display"] = "none";
                comorbidityBold.Style["display"] = "none";
                comorbidityACS.Style["display"] = "none";

                if (Request.Cookies["SubmitData"].Value.IndexOf("bold") >= 0 || Request.Cookies["SubmitData"].Value == "")
                {
                    bariatricBold.Style["display"] = "block";
                    nonbariatricBold.Style["display"] = "block";

                    if (Request.Cookies["SubmitData"].Value.IndexOf("bold") >= 0)
                        comorbidityBold.Style["display"] = "block";
                    else if (Request.Cookies["SubmitData"].Value == "")
                        comorbidityDefault.Style["display"] = "block";

                    LoadBoldList();
                }
                
                if (Request.Cookies["SubmitData"].Value.IndexOf("acs") >= 0)
                {
                    bariatricDefault.Style["display"] = "block";
                    nonbariatricDefault.Style["display"] = "block";
                    comorbidityACS.Style["display"] = "block";
                }

                if (Request.Cookies["SubmitData"].Value == "")
                {
                    nonbariatricDefault.Style["display"] = "block";
                }
                //FillTemplate();

                //DEMO SETTING
                if (url.ToUpper().IndexOf("DEMO") >= 0 || url.ToUpper().IndexOf("FERRY") >= 0 || url.ToUpper().IndexOf(".0.105") >= 0)
                {
                    //AIGB Setting
                    li_Div18.Style["display"] = "block";
                }

            }
            RemoveFirstItemFromDropDownList();
        }
        catch (Exception err)
        {
            string strLanguageCode;
            try
            {
                strLanguageCode = Request.Cookies["LanguageCode"].Value;
                gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Patient Data Form", "Page_Load function", err.ToString());
            }
            catch { strLanguageCode = "en-US"; }
            gClass.ReturnToLoginPage(Request.Url.Host, strLanguageCode, Response);
        }
    }
    #endregion

    #region events
    protected override void OnLoad(EventArgs e)
    {
        txtHApplicationURL.Value = Request.Url.Scheme + "://" + Request.Url.Host + Request.ApplicationPath + "/";
        txtUseImperial.Value = (Request.Cookies["Imperial"].Value == "True") ? "1" : "0"; //base.Imperial ? "1" : "0"; 
        gClass.LanguageCode = base.LanguageCode;
        txtHCulture.Value = base.CultureInfo;
        InitialiseFormSetting();

        base.OnLoad(e);
    }
    #endregion 

    #region protected void linkBtnSave_OnClick(object sender, EventArgs e)
    protected void linkBtnSave_OnClick(object sender, EventArgs e)
    {
        String strScript = String.Empty;

        savePatientData = false;
        savePatientWeightData = false;
        saveBoldData = false;
        saveEMRData = false;
        saveComorbidityData = false;
        saveComorbidityCheckData = false;

        if (Request.Cookies["PermissionLevel"].Value != "1o")
        {
            IsDoneSaveFlag = -1;

            switch (txtHPageNo.Value)
            {
                case "3":
                case "6":
                case "7":
                case "9":
                case "10":
                case "11":
                case "13":
                case "16":
                case "18":
                    SavePatientDataEMR();
                    break;

                case "1":
                    SavePatientData();
                    break;

                case "2":
                    SavePatientDataPreviousSurgery();
                    break;

                case "5":
                    SavePatientDataComorbidity();
                    break;

                case "8":
                    SavePatientLabs();
                    SavePatientDataEMR();
                    break;

                case "12":
                    SavePatientData();
                    SavePatientDataHeightWeight();
                    break;

                case "17":
                    IsDoneSaveFlag = 1;
                    break;

                default:
                    IsDoneSaveFlag = -1;
                    break;
            }
            
            switch (IsDoneSaveFlag)
            {
                case 1: // Saving data procedure is done sucessfully                    
                    Response.Redirect("~/Forms/EMR/EMRForm.aspx?PID=" + txtPatientID.Value + "&section=" + txtSelectedPageNo.Value, false);

                    //LoadPatientData();
                    if (txtHPageNo.Value == "1")
                    {
                        LoadPatientEMR("children");
                        //calculate age
                        DateTime currentDate = Convert.ToDateTime(txtHCurrentDate.Value);
                        DateTime birthDate = Convert.ToDateTime(txtBirthDate.Text);

                        int intAge = currentDate.Year - birthDate.Year;
                        if (currentDate.Month < birthDate.Month)
                            --intAge;
                        else if (currentDate.Month == birthDate.Month)
                            if (currentDate.Day < birthDate.Day)
                                --intAge;

                        if (intAge > 0)
                        {
                            strScript += "SetInnerText($get('tblPatientTitle_lblAge_Value'), " + intAge.ToString() + " + ' yrs');";
                            strScript += "SetInnerText($get('lblAge'), " + intAge.ToString() + ");";
                        }
                        else
                        {
                            strScript += "SetInnerText($get('tblPatientTitle_lblAge_Value'), '');";
                            strScript += "SetInnerText($get('lblAge'), '');";
                        }
                    }
                    else if (txtHPageNo.Value == "9")
                    {
                        LoadPatientEMR("medication");
                    }
                    else if (txtHPageNo.Value == "2")
                    {
                        if(Request.Cookies["SubmitData"].Value.IndexOf("acs") >= 0)
                            LoadPatientEMR("previoussurgery");
                        else if (Request.Cookies["SubmitData"].Value.IndexOf("bold") >= 0 || Request.Cookies["SubmitData"].Value == "")
                            LoadPatientData_BoldDataGroup("previoussurgery");
                    }
                    else if (txtHPageNo.Value == "5")
                    {                        
                        if(Request.Cookies["SubmitData"].Value.IndexOf("bold") >= 0)
                            LoadPatientData_BoldDataGroup("comorbidity");
                    }
                    

                    decimal visitWeight = 0;
                    if (cmbVisitWeeks.SelectedValue.ToString() != "")
                    {
                        strScript += "SetInnerText($get('tblPatientTitle_lblCalculateVisitIntro'), 'Calculate visit weeks and weight loss from:');";
                        strScript += "SetInnerText($get('tblPatientTitle_lblCalculateVisit'),'" + cmbVisitWeeks.SelectedText + "');";

                        strScript += "SetInnerText($get('tblPatientTitle_lblCalculateVisitDate'),'');";

                        if (cmbVisitWeeks.SelectedValue.ToString() == "3")
                        {
                            visitWeight = Decimal.TryParse(txtHStartWeight.Value, out decTemp) ? decTemp : 0;
                            strScript += "SetInnerText($get('tblPatientTitle_lblCalculateVisitDate'),'" + txtZeroDate.Text + "');";                            
                        }
                        else if (cmbVisitWeeks.SelectedValue.ToString() == "4")
                        {
                            visitWeight = Decimal.TryParse(txtHStartWeight.Value, out decTemp) ? decTemp : 0;
                            strScript += "SetInnerText($get('tblPatientTitle_lblCalculateVisitDate'),'" + txtHLapbandDate.Value + "');";
                        }
                    }

                    Response.Cookies.Set(new HttpCookie("VisitWeeksFlag", cmbVisitWeeks.SelectedValue));                        

                    strScript += "ShowDivMessage('The information has been saved...', true); UpdatePatientTitle(); document.getElementById('tblPatientTitle_div_PatientTitle').style.display = 'block';";
                    strScript += "setDivStatus(" + txtSelectedPageNo.Value + ");";
                    strScript += "SetEvents();";

                    
                    LapbaseSession.PatientId = Convert.ToInt32(txtPatientID.Value);
                    break;

                case 0: // Saving data procedure is not done sucessfully
                    strScript = "document.getElementById('divErrorMessage').style.display = 'block';";
                    strScript += "SetInnerText(document.getElementById('pErrorMessage'), 'Error in save data ..." + BoldErrorMsg + "');";
                    strScript += "if (" + txtHPageNo.Value + "!= 0) setDivStatus(" + txtHPageNo.Value + ");";
                    strScript += "SetEvents();";
                    break;

                case -1:   // data is not ready to save because some fields are empty such as Surname, Firstname is DEMOGRAPG sub-page
                    strScript = "if (AppSchema_ButtonNo != 0) ";
                    strScript += "  document.location.assign(document.getElementById('txtHApplicationURL').value + AppSchemaMenu_href[AppSchema_ButtonNo-1]);";
                    strScript += "else if (" + txtSelectedPageNo.Value + "!= 0) setDivStatus(" + txtSelectedPageNo.Value + ");";
                    strScript += "SetEvents();";
                    break;
            }
            ScriptManager.RegisterStartupScript(linkBtnSave, linkBtnSave.GetType(), Guid.NewGuid().ToString(), strScript, true);
        }
        else
        {
            strScript = "if (AppSchema_ButtonNo != 0) ";
            strScript += "  document.location.assign(document.getElementById('txtHApplicationURL').value + AppSchemaMenu_href[AppSchema_ButtonNo-1]);";

            strScript += "if (" + txtSelectedPageNo.Value + "!= 0) setDivStatus(" + txtSelectedPageNo.Value + ");"; ;
            strScript += "SetEvents();";
            ScriptManager.RegisterStartupScript(linkBtnSave, linkBtnSave.GetType(), Guid.NewGuid().ToString(), strScript, true);
        }
    }
    #endregion 
    
    #region private void LoadPatientData()
    private void LoadPatientData()
    {
        SqlCommand cmdSelect = new SqlCommand();
        DataSet dsPatient = new DataSet();

        try
        {
            gClass.MakeStoreProcedureName(ref cmdSelect, "sp_PatientData_LoadData", true);
            cmdSelect.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
            cmdSelect.Parameters.Add("@UserPracticeCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
            cmdSelect.Parameters.Add("@PatientID", System.Data.SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
            cmdSelect.Parameters.Add("@Patient_CustomID", System.Data.SqlDbType.VarChar, 20).Value = txtPatientID.Value;
            dsPatient = gClass.FetchData(cmdSelect, "tblPatient");
            if (dsPatient.Tables.Count > 0)
            {
                LoadPatientEMR("");
                LoadPatientData_DemoGraphics(dsPatient.Tables[0].DefaultView);
                LoadPatientData_Measurement(dsPatient.Tables[0].DefaultView);
                LoadPatientData_BoldDataGroup("");
                LoadLogoData("");
                //LoadPatientPathology();
            }
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Patient Data Form", "Loading Patient Data - LoadPatientData function", err.ToString());
        }
    }
    #endregion

    #region private LoadDefault()
    private void LoadDefault()
    {
        //default

        HtmlInputRadioButton tempControlRadio = new HtmlInputRadioButton();
        String tempRadioField = "";

        //Special Investigation
        for (int Xh = 0; Xh < SpecialInvestigation.Length / 2; Xh++)
        {
            tempRadioField = SpecialInvestigation[Xh, 1];

            tempControlRadio = (HtmlInputRadioButton)FindControlRecursive(this.Page, SpecialInvestigation[Xh, 0] + "N");
            tempControlRadio.Checked = true;
        }

        //Investigation
        for (int Xh = 0; Xh < Investigation.Length / 2; Xh++)
        {
            tempRadioField = Investigation[Xh, 1];

            tempControlRadio = (HtmlInputRadioButton)FindControlRecursive(this.Page, Investigation[Xh, 0] + "N");
            tempControlRadio.Checked = true;
        }

        rbDeceasedPrimaryProcedureN.Checked = true;

        cmbComplaint.SelectedValue = "115";
    }
    #endregion

    #region private void LoadPatientEMR(string section)
    private void LoadPatientEMR(string section)
    {
        SqlCommand cmdSelect = new SqlCommand();
        DataSet dsPatient = new DataSet();

        String[] tempRegistryDataList;
        String[] tempWeightHistoryList;
        String[] tempBackgroundHistory;
        String[] children;        
        String[] child;
        String childAge;
        String childGender;
        String tempChildList = "";
        Int32 totalChild = 0;
        String maleSelected;
        String femaleSelected;
        
        String[] medications;
        String[] medication;
        String medicationName;
        String medicationDosage;
        String medicationFreq;
        String tempMedicationList = "";
        Int32 totalMedication = 0;
        
        String[] nonBariatrics;
        String nonBariatricSurgeryName;
        String tempNonBariatricList = "";
        Int32 totalNonBariatrics = 0;


        HtmlInputCheckBox tempControlCheckbox = new HtmlInputCheckBox();
        String tempCheckBox = "";

        HtmlInputRadioButton tempControlRadio = new HtmlInputRadioButton();
        String tempRadioField = "";

        HtmlInputText tempControlText = new HtmlInputText();
        String tempTextField = "";

        try
        {
            gClass.MakeStoreProcedureName(ref cmdSelect, "sp_PatientEMR_LoadData", true);
            cmdSelect.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
            cmdSelect.Parameters.Add("@PatientID", System.Data.SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
            dsPatient = gClass.FetchData(cmdSelect, "tblPatient");

            if ((dsPatient.Tables.Count > 0) && (dsPatient.Tables[0].Rows.Count > 0))
            {
                if (section == "")
                {
                    //Demographic
                    txtOccupation.Text = dsPatient.Tables[0].Rows[0]["Details_Occupation"].ToString();
                    txtLiveHome.Text = dsPatient.Tables[0].Rows[0]["Details_LiveAtHome"].ToString();
                    txtPartnerName.Text = dsPatient.Tables[0].Rows[0]["Details_SpouseName"].ToString();
                    txtNOKName.Text = dsPatient.Tables[0].Rows[0]["Details_NextOfKinName"].ToString();
                    txtNOKRelationship.Text = dsPatient.Tables[0].Rows[0]["Details_NextOfKinRelation"].ToString();
                    txtNOKAddress.Value = dsPatient.Tables[0].Rows[0]["Details_NextOfKinAddress"].ToString();
                    txtNOKHome.Text = dsPatient.Tables[0].Rows[0]["Details_NextOfKinHomePhone"].ToString();
                    txtNOKWork.Text = dsPatient.Tables[0].Rows[0]["Details_NextOfKinWorkPhone"].ToString();
                    txtNOKMobile.Text = dsPatient.Tables[0].Rows[0]["Details_NextOfKinMobile"].ToString();
                    txtAddC1Name.Text = dsPatient.Tables[0].Rows[0]["Details_AddContact1Name"].ToString();
                    txtAddC1Relationship.Text = dsPatient.Tables[0].Rows[0]["Details_AddContact1Relation"].ToString();
                    txtAddC1Address.Value = dsPatient.Tables[0].Rows[0]["Details_AddContact1Address"].ToString();
                    txtAddC1Home.Text = dsPatient.Tables[0].Rows[0]["Details_AddContact1HomePhone"].ToString();
                    txtAddC1Mob.Text = dsPatient.Tables[0].Rows[0]["Details_AddContact1Mobile"].ToString();
                    txtAddC2Name.Text = dsPatient.Tables[0].Rows[0]["Details_AddContact2Name"].ToString();
                    txtAddC2Relationship.Text = dsPatient.Tables[0].Rows[0]["Details_AddContact2Relation"].ToString();
                    txtAddC2Address.Value = dsPatient.Tables[0].Rows[0]["Details_AddContact2Address"].ToString();
                    txtAddC2Home.Text = dsPatient.Tables[0].Rows[0]["Details_AddContact2HomePhone"].ToString();
                    txtAddC2Mob.Text = dsPatient.Tables[0].Rows[0]["Details_AddContact2Mobile"].ToString();
                    txtMedicareNumber.Text = dsPatient.Tables[0].Rows[0]["Details_MedicareNumber"].ToString();
                    txtDeceasedDate.Text = gClass.TruncateDate(dsPatient.Tables[0].Rows[0]["DeceasedDate"].ToString().Trim(), Request.Cookies["CultureInfo"].Value, 1);
                    txtDeceasedNote.Text = dsPatient.Tables[0].Rows[0]["Details_DeceasedNote"].ToString();
                    if(dsPatient.Tables[0].Rows[0]["Details_DeceasedPrimaryProcedure"].ToString().Equals(Boolean.TrueString))
                    {
                        rbDeceasedPrimaryProcedureY.Checked = true;
                        rowDeceasedNote.Style["display"] = "block";
                    }
                    else
                    {
                        rbDeceasedPrimaryProcedureN.Checked = true;
                        rowDeceasedNote.Style["display"] = "none";
                    }
                    
                    detailsPhoto.ImageUrl = dsPatient.Tables[0].Rows[0]["Details_PhotoPath"].ToString();
                    setPhotoVisibility();
                    

                    for (int Xh = 0; Xh < DemographicStructure.Length / 2; Xh++)
                    {
                        tempRadioField = DemographicStructure[Xh, 1];

                        if (dsPatient.Tables[0].Rows[0][tempRadioField].ToString().Trim() != "")
                        {
                            tempControlRadio = (HtmlInputRadioButton)FindControlRecursive(this.Page, DemographicStructure[Xh, 0] + UppercaseFirst(dsPatient.Tables[0].Rows[0][tempRadioField].ToString()));
                            tempControlRadio.Checked = true;
                        }
                    }
                    
                    //Weight Loss History
                    for (int Xh = 0; Xh < WeightHistory.Length / 2; Xh++)
                    {
                        tempRadioField = WeightHistory[Xh, 1];

                        if (dsPatient.Tables[0].Rows[0][tempRadioField].ToString().Trim() != "")
                        {
                            tempControlRadio = (HtmlInputRadioButton)FindControlRecursive(this.Page, WeightHistory[Xh, 0] + UppercaseFirst(dsPatient.Tables[0].Rows[0][tempRadioField].ToString()));
                            tempControlRadio.Checked = true;
                        }
                    }

                    txtHWeightHistoryGainWeight.Value = dsPatient.Tables[0].Rows[0]["WeightHistory_GainWeight"].ToString();
                    txtWeightHistoryGainYears.Text = dsPatient.Tables[0].Rows[0]["WeightHistory_GainYears"].ToString();

                    txtWeightHistoryLossTryLose.Text = dsPatient.Tables[0].Rows[0]["WeightHistory_LoseYears"].ToString();
                    txtWeightHistoryLossGrpOther.Text = dsPatient.Tables[0].Rows[0]["WeightHistory_GroupOther"].ToString();
                    txtWeightHistoryLossDietOther.Text = dsPatient.Tables[0].Rows[0]["WeightHistory_PillOther"].ToString();
                    txtWeightHistoryLossPAOther.Text = dsPatient.Tables[0].Rows[0]["WeightHistory_AdviceOther"].ToString();
                    txtWeightHistoryLossCosmetic.Text = dsPatient.Tables[0].Rows[0]["WeightHistory_CosmeticList"].ToString();
                    txtWeightHistoryGroupDuration.Text = dsPatient.Tables[0].Rows[0]["WeightHistory_GroupDuration"].ToString();
                    txtWeightHistoryGroupLose.Text = dsPatient.Tables[0].Rows[0]["WeightHistory_GroupLose"].ToString();
                    
                    for (int Xh = 0; Xh < WeightHistoryList.Length; Xh++)
                    {
                        tempCheckBox = WeightHistoryList[Xh];
                        if (dsPatient.Tables[0].Rows[0][tempCheckBox].ToString() != "")
                        {
                            tempWeightHistoryList = dsPatient.Tables[0].Rows[0][tempCheckBox].ToString().Split('-');
                            foreach (string familyMember in tempWeightHistoryList)
                            {
                                tempControlCheckbox = (HtmlInputCheckBox)FindControlRecursive(this.Page, familyMember);
                                tempControlCheckbox.Checked = true;
                            }
                        }
                    }


                    //Registry Data
                    for (int Xh = 0; Xh < RegistryData.Length / 2; Xh++)
                    {
                        tempRadioField = RegistryData[Xh, 1];

                        if (dsPatient.Tables[0].Rows[0][tempRadioField].ToString().Trim() != "")
                        {
                            tempControlRadio = (HtmlInputRadioButton)FindControlRecursive(this.Page, RegistryData[Xh, 0] + UppercaseFirst(dsPatient.Tables[0].Rows[0][tempRadioField].ToString()));
                            tempControlRadio.Checked = true;
                        }
                    }
                    txtAntihypertensives.Value = dsPatient.Tables[0].Rows[0]["Registry_Antihypertensives"].ToString();
                    rdRegistryMedication.Value = dsPatient.Tables[0].Rows[0]["Registry_Insulin"].ToString();


                    //Background
                    for (int Xh = 0; Xh < Background.Length; Xh++)
                    {
                        tempCheckBox = Background[Xh];
                        if (dsPatient.Tables[0].Rows[0][tempCheckBox].ToString().Trim() != "")
                        {
                            tempBackgroundHistory = dsPatient.Tables[0].Rows[0][tempCheckBox].ToString().Split('-');
                            foreach (string familyMember in tempBackgroundHistory)
                            {
                                if (familyMember.Trim() != "")
                                {
                                    tempControlCheckbox = (HtmlInputCheckBox)FindControlRecursive(this.Page, familyMember);
                                    tempControlCheckbox.Checked = true;
                                }
                            }
                        }
                    }

                    txtBackgroundFamilyHistory.Text = dsPatient.Tables[0].Rows[0]["Background_FamilyHistory"].ToString();
                    txtBackgroundPastHealth.Text = dsPatient.Tables[0].Rows[0]["Background_PastHealth"].ToString();


                    //Special Investigation
                    txtSpecialInvestigationSSS.Value = dsPatient.Tables[0].Rows[0]["SpecialInvestigation_SuggestedSS"].ToString();
                    txtSpecialInvestigationGES.Value = dsPatient.Tables[0].Rows[0]["SpecialInvestigation_SuggestedGE"].ToString();
                    txtSpecialInvestigationSPS.Value = dsPatient.Tables[0].Rows[0]["SpecialInvestigation_SuggestedSP"].ToString();
                    txtSpecialInvestigationNAS.Value = dsPatient.Tables[0].Rows[0]["SpecialInvestigation_SuggestedNA"].ToString();
                    txtSpecialInvestigationPSS.Value = dsPatient.Tables[0].Rows[0]["SpecialInvestigation_SuggestedPS"].ToString();
                    txtSpecialInvestigationOAS.Value = dsPatient.Tables[0].Rows[0]["SpecialInvestigation_SuggestedOA"].ToString();

                    for (int Xh = 0; Xh < SpecialInvestigation.Length / 2; Xh++)
                    {
                        tempRadioField = SpecialInvestigation[Xh, 1];

                        if (dsPatient.Tables[0].Rows[0][tempRadioField].ToString().Trim() != "")
                        {
                            tempControlRadio = (HtmlInputRadioButton)FindControlRecursive(this.Page, SpecialInvestigation[Xh, 0] + dsPatient.Tables[0].Rows[0][tempRadioField].ToString().Trim());
                            tempControlRadio.Checked = true;
                        }
                        else
                        {
                            tempControlRadio = (HtmlInputRadioButton)FindControlRecursive(this.Page, SpecialInvestigation[Xh, 0] + "N");
                            tempControlRadio.Checked = true;
                        }
                    }

                    //Investigation
                    txtInvestigationRBS.Value = dsPatient.Tables[0].Rows[0]["Investigation_RBS"].ToString();
                    txtInvestigationABS.Value = dsPatient.Tables[0].Rows[0]["Investigation_ABS"].ToString();
                    txtInvestigationRFT.Value = dsPatient.Tables[0].Rows[0]["Investigation_RFT"].ToString();
                    txtInvestigationABG.Value = dsPatient.Tables[0].Rows[0]["Investigation_ABG"].ToString();
                    txtInvestigationEET.Value = dsPatient.Tables[0].Rows[0]["Investigation_EET"].ToString();
                    txtInvestigationBM.Value = dsPatient.Tables[0].Rows[0]["Investigation_BM"].ToString();
                    txtInvestigationEMS.Value = dsPatient.Tables[0].Rows[0]["Investigation_EMS"].ToString();
                    txtInvestigationP.Value = dsPatient.Tables[0].Rows[0]["Investigation_P"].ToString();
                    txtInvestigationEKG.Value = dsPatient.Tables[0].Rows[0]["Investigation_EKG"].ToString();
                    txtInvestigationCX.Value = dsPatient.Tables[0].Rows[0]["Investigation_CX"].ToString();

                    for (int Xh = 0; Xh < Investigation.Length / 2; Xh++)
                    {
                        tempRadioField = Investigation[Xh, 1];

                        if (dsPatient.Tables[0].Rows[0][tempRadioField].ToString().Trim() != "")
                        {
                            tempControlRadio = (HtmlInputRadioButton)FindControlRecursive(this.Page, Investigation[Xh, 0] + dsPatient.Tables[0].Rows[0][tempRadioField].ToString().Trim());
                            tempControlRadio.Checked = true;
                        }
                        else
                        {
                            tempControlRadio = (HtmlInputRadioButton)FindControlRecursive(this.Page, Investigation[Xh, 0] + "N");
                            tempControlRadio.Checked = true;
                        }
                    }

                    //Allergies
                    chkAlergyDrinkTypeBeer.Checked = dsPatient.Tables[0].Rows[0]["Allergy_DrinkBeer"].ToString().Equals(Boolean.TrueString);
                    chkAlergyDrinkTypeWine.Checked = dsPatient.Tables[0].Rows[0]["Allergy_DrinkWine"].ToString().Equals(Boolean.TrueString);
                    chkAlergyDrinkTypeSpirits.Checked = dsPatient.Tables[0].Rows[0]["Allergy_DrinkSpirits"].ToString().Equals(Boolean.TrueString);
                    rdAlergySmokeHaveYouSmoke_Yes.Checked = dsPatient.Tables[0].Rows[0]["Allergy_HaveSmoke"].ToString().Equals(Boolean.TrueString);
                    rdAlergySmokeHaveYouSmoke_No.Checked = dsPatient.Tables[0].Rows[0]["Allergy_HaveSmoke"].ToString().Equals(Boolean.FalseString);

                    txtAllergy.Text = dsPatient.Tables[0].Rows[0]["Allergy_ListAllergy"].ToString();
                    txtMedication.Text = dsPatient.Tables[0].Rows[0]["Allergy_ListMedication"].ToString();
                    txtAnesthetic.Text = dsPatient.Tables[0].Rows[0]["Allergy_Anesthetic"].ToString();
                    txtAnestheticRisk.Text = dsPatient.Tables[0].Rows[0]["Allergy_AnestheticRisk"].ToString();
                    txtAlergyDrinkPerDay.Text = dsPatient.Tables[0].Rows[0]["Allergy_DrinkPerDay"].ToString();
                    txtAlergyDrinkPerWeek.Text = dsPatient.Tables[0].Rows[0]["Allergy_DrinkPerWeek"].ToString();
                    txtAlergySmokePerDay.Text = dsPatient.Tables[0].Rows[0]["Allergy_SmokePerDay"].ToString();
                    txtAlergySmokePastPerDay.Text = dsPatient.Tables[0].Rows[0]["Allergy_HaveSmokePerDay"].ToString();
                    txtAlergySmokeHowmanyYears.Text = dsPatient.Tables[0].Rows[0]["Allergy_HaveSmokeYears"].ToString();
                    txtAlergySmokeWhenStop.Text = dsPatient.Tables[0].Rows[0]["Allergy_HaveSmokeStop"].ToString();
                    txtAlergyDrugsNotes.Text = dsPatient.Tables[0].Rows[0]["Allergy_DoDrugs_Notes"].ToString();


                    for (int Xh = 0; Xh < Allergy.Length / 2; Xh++)
                    {
                        tempRadioField = Allergy[Xh, 1];

                        if (dsPatient.Tables[0].Rows[0][tempRadioField].ToString().Trim() != "")
                        {
                            tempControlRadio = (HtmlInputRadioButton)FindControlRecursive(this.Page, Allergy[Xh, 0] + UppercaseFirst(dsPatient.Tables[0].Rows[0][tempRadioField].ToString()));
                            tempControlRadio.Checked = true;
                        }
                    }

                    for (int Xh = 0; Xh < AllergyRadio.Length / 2; Xh++)
                    {
                        tempRadioField = AllergyRadio[Xh, 1];

                        tempControlRadio = (HtmlInputRadioButton)FindControlRecursive(this.Page, AllergyRadio[Xh, 0] + "N");
                        if (dsPatient.Tables[0].Rows[0][tempRadioField].ToString().Equals(Boolean.TrueString))
                            tempControlRadio = (HtmlInputRadioButton)FindControlRecursive(this.Page, AllergyRadio[Xh, 0] + "Y");
                        tempControlRadio.Checked = true;
                    }

                    //Physical Exam
                    txtExamNotes.Text = dsPatient.Tables[0].Rows[0]["Exam_Notes"].ToString();
                    for (int Xh = 0; Xh < Exam.Length; Xh++)
                    {
                        tempTextField = Exam[Xh];
                        if (dsPatient.Tables[0].Rows[0]["Exam_" + tempTextField].ToString().Trim() != "")
                        {
                            tempControlCheckbox = (HtmlInputCheckBox)FindControlRecursive(this.Page, "chkExam" + tempTextField);
                            tempControlCheckbox.Checked = true;

                            tempControlText = (HtmlInputText)FindControlRecursive(this.Page, "txtExam" + tempTextField);
                            tempControlText.Value = dsPatient.Tables[0].Rows[0]["Exam_" + tempTextField].ToString().Trim();
                            tempControlText.Disabled = false;
                        }
                    }

                    //Labs
                    txtLabNotes.Text = dsPatient.Tables[0].Rows[0]["Lab_Notes"].ToString();

                    //Referral
                    txtReferralsPA.Value = dsPatient.Tables[0].Rows[0]["Referrals_PA"].ToString();
                    txtReferralsC.Value = dsPatient.Tables[0].Rows[0]["Referrals_C"].ToString();
                    txtReferralsRP.Value = dsPatient.Tables[0].Rows[0]["Referrals_RP"].ToString();
                    txtReferralsE.Value = dsPatient.Tables[0].Rows[0]["Referrals_E"].ToString();
                    txtReferralsAP.Value = dsPatient.Tables[0].Rows[0]["Referrals_AP"].ToString();
                    txtReferralsGM.Value = dsPatient.Tables[0].Rows[0]["Referrals_GM"].ToString();
                    txtReferralsO.Value = dsPatient.Tables[0].Rows[0]["Referrals_O"].ToString();
                    chkReferralsPA.Checked = dsPatient.Tables[0].Rows[0]["Referrals_PA"] != DBNull.Value;
                    chkReferralsC.Checked = dsPatient.Tables[0].Rows[0]["Referrals_C"] != DBNull.Value;
                    chkReferralsRP.Checked = dsPatient.Tables[0].Rows[0]["Referrals_RP"] != DBNull.Value;
                    chkReferralsE.Checked = dsPatient.Tables[0].Rows[0]["Referrals_E"] != DBNull.Value;
                    chkReferralsAP.Checked = dsPatient.Tables[0].Rows[0]["Referrals_AP"] != DBNull.Value;
                    chkReferralsGM.Checked = dsPatient.Tables[0].Rows[0]["Referrals_GM"] != DBNull.Value;
                    chkReferralsO.Checked = dsPatient.Tables[0].Rows[0]["Referrals_O"] != DBNull.Value;


                    //Complaint
                    cmbComplaint.SelectedValue = dsPatient.Tables[0].Rows[0]["Complaint"].ToString();

                    txtComplaint.Text = dsPatient.Tables[0].Rows[0]["ComplaintNotes"].ToString();


                    //Management Plan
                    txtManagement.Text = dsPatient.Tables[0].Rows[0]["Management"].ToString();
                }

                if (section == "" || section == "medication")
                {
                    //Medication
                    medicationDiv.InnerHtml = "";
                    if (dsPatient.Tables[0].Rows[0]["Medication"].ToString().Trim() != "")
                    {
                        medications = dsPatient.Tables[0].Rows[0]["Medication"].ToString().Split('+');

                        foreach (string tempMedication in medications)
                        {
                            totalMedication++;
                            medication = tempMedication.Split('*');
                            medicationName = medication[0];
                            medicationDosage = medication[1];
                            medicationFreq = medication[2];

                            tempMedicationList += "<div id='idMed" + totalMedication + "div'><table><tr><td style='width: 45%'><input type=hidden name=delMedications" + totalMedication + " value=no><input type=textbox runat='server' name='txtMedicationsName" + totalMedication + "' value='" + medicationName + "' size='50'></td><td align='center' style='width:25%'><input type=textbox runat='server' name='txtMedicationsDosage" + totalMedication + "' value='" + medicationDosage + "'></td><td align='center' style='width: 25%'><input type='textbox' runat='server' name='txtMedicationsFreq" + totalMedication + "' value='" + medicationFreq + "'></td><td width='5%' align='center'><input type=button name=remove" + totalMedication + " value=' - ' onclick='javascript:removeMedications(" + totalMedication + ",\"idMed" + totalMedication + "div\")'></td></tr></table></div>";
                        }
                        medicationDiv.InnerHtml = tempMedicationList;
                    }
                    totalMedications.Value = totalMedication.ToString();
                }
                if (section == "" || section == "children")
                {
                    //demographic - children
                    childrenDiv.InnerHtml = "";
                    if (dsPatient.Tables[0].Rows[0]["Details_Children"].ToString().Trim() != "")
                    {
                        children = dsPatient.Tables[0].Rows[0]["Details_Children"].ToString().Split('+');

                        foreach (string tempChild in children)
                        {
                            maleSelected = "";
                            femaleSelected = "";

                            totalChild++;
                            child = tempChild.Split('*');
                            childAge = child[0];
                            childGender = child[1];

                            if (childGender == "M")
                                maleSelected = "Selected = selected";
                            else
                                femaleSelected = "Selected = selected";

                            tempChildList += "<div id=id" + totalChild + "div>Age &nbsp; <input type=hidden name=del" + totalChild + " value=no><input type=textbox runat='server' name=txtChildAge" + totalChild + " value='" + childAge + "'> &nbsp; &nbsp; &nbsp; Gender <select runat='server' id=cmbChildGender" + totalChild + "><option value='M' " + maleSelected + ">Male</option><option value='F' " + femaleSelected + ">Female</option></select> &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <input type=button name=remove" + totalChild + " value='-' onclick='javascript:removeChildren(" + totalChild + ",\"id" + totalChild + "div\")'></div>";

                        }
                        childrenDiv.InnerHtml = tempChildList;
                    }
                    totalChildren.Value = totalChild.ToString();
                }
                if (section == "" || section == "previoussurgery")
                {
                    //background - previous surgeries
                    
                    String[] bariatrics;
                    String[] bariatric;
                    String bariatricSurgeryYear;
                    String bariatricOriginalWeight;
                    String bariatricOriginalWeightEstimated;
                    String bariatricOriginalWeightActual;
                    String bariatricLowestWeight;
                    String bariatricLowestWeightEstimated;
                    String bariatricLowestWeightActual;
                    String bariatricSurgeryName;
                    String bariatricSurgeryEvent;
                    String tempBariatricList = "";
                    Int32 totalBariatrics = 0;


                    //Bariatric
                    bariatricDiv.InnerHtml = "";
                    if (dsPatient.Tables[0].Rows[0]["Background_PreviousBariatric"].ToString().Trim() != "")
                    {
                        bariatrics = dsPatient.Tables[0].Rows[0]["Background_PreviousBariatric"].ToString().Split('+');

                        foreach (string tempBariatric in bariatrics)
                        {
                            totalBariatrics++;
                            bariatric = tempBariatric.Split('*');

                            bariatricSurgeryYear = bariatric[0];
                            bariatricOriginalWeight = bariatric[1];
                            bariatricOriginalWeightEstimated = bariatric[2] == "true" ? "checked" : "";
                            bariatricOriginalWeightActual = bariatric[2] == "false" ? "checked" : "";
                            bariatricLowestWeight = bariatric[3];
                            bariatricLowestWeightEstimated = bariatric[4] == "true" ? "checked" : "";
                            bariatricLowestWeightActual = bariatric[4] == "false" ? "checked" : "";
                            bariatricSurgeryName = bariatric[5];
                            bariatricSurgeryEvent = bariatric[6];

                            tempBariatricList += "<div id='idBariatric" + totalBariatrics + "div'><table><tr><td style='width: 20%'>Year:</td><td style='width: 75%'><input type='textbox' runat='server' name=txtBariatricYear" + totalBariatrics + " size='10' value='" + bariatricSurgeryYear + "'></td><td style='width : 5%'>&nbsp;</td></tr><tr><td>Original Weight:</td><td><input type='textbox' runat='server' name=txtBariatricOriginalWeight" + totalBariatrics + " size='10'value='" + bariatricOriginalWeight + "'> &nbsp;" + lblMeasurementWeightUnit.Text + " &nbsp; &nbsp; <input type='radio' name='rbOrgWeight" + totalBariatrics + "' id='rbOrgWeight_Estimated" + totalBariatrics + "' runat='server' " + bariatricOriginalWeightEstimated + " />&nbsp; Estimated &nbsp; &nbsp; <input type='radio' name='rbOrgWeight" + totalBariatrics + "' id='rbOrgWeight_Actual" + totalBariatrics + "' runat='server' " + bariatricOriginalWeightActual + "/>&nbsp; Actual</td><td>&nbsp;</td></tr><tr><td>Lowest Weight Acheived:</td><td><input type='textbox' runat='server' name=txtBariatricLowestWeight" + totalBariatrics + " size='10' value='" + bariatricLowestWeight + "'> &nbsp;" + lblMeasurementWeightUnit.Text + " &nbsp; &nbsp; <input type='radio' name='rbLowestWeight" + totalBariatrics + "' id='rbLowestWeight_Estimated" + totalBariatrics + "' runat='server' " + bariatricLowestWeightEstimated + " />&nbsp; Estimated &nbsp; &nbsp; <input type='radio' name='rbLowestWeight" + totalBariatrics + "' id='rbLowestWeight_Actual" + totalBariatrics + "' runat='server' " + bariatricLowestWeightActual + "/>&nbsp; Actual</td><td>&nbsp;</td></td><td>&nbsp;</td></tr><tr><td>Previous Bariatric Surgeries:</td><td><input type='textbox' runat='server' name=txtBariatricSurgeries" + totalBariatrics + " size='100' value='" + bariatricSurgeryName + "'></td><td>&nbsp;</td></tr><tr><td>Adverse Events associated:</td><td><input type='textbox' runat='server' name=txtBariatricEvents" + totalBariatrics + " size='100' value='" + bariatricSurgeryEvent + "'></td><td><input type=hidden name=delBariatric" + totalBariatrics + " value=no><input type=button name=removeBariatric" + totalBariatrics + " value=' - ' onclick='javascript:removeBariatric(" + totalBariatrics + ",\"idBariatric" + totalBariatrics + "div\")'></td></tr></table></div>";
                        }
                        bariatricDiv.InnerHtml = tempBariatricList;
                    }
                    totalBariatric.Value = totalBariatrics.ToString();


                    //Previous NonBariatric Surgery
                    nonBariatricDiv.InnerHtml = "";
                    if (dsPatient.Tables[0].Rows[0]["Background_PreviousNonBariatric"].ToString().Trim() != "")
                    {
                        nonBariatrics = dsPatient.Tables[0].Rows[0]["Background_PreviousNonBariatric"].ToString().Split('+');

                        foreach (string tempBariatric in nonBariatrics)
                        {
                            totalNonBariatrics++;
                            nonBariatricSurgeryName = tempBariatric;

                            tempNonBariatricList += "<div id='idNonBariatric" + totalNonBariatrics + "div'><table><tr><td style='width: 90%'><input type=hidden name=delNonBariatric" + totalNonBariatrics + " value=no><input type=textbox runat='server' name=txtNonBariatric" + totalNonBariatrics + " size='130' value='" + nonBariatricSurgeryName + "'></td><td style='width : 10%'><input type=button name=removeNonBariatric" + totalNonBariatrics + " value=' - ' onclick='javascript:removeNonBariatric(" + totalNonBariatrics + ",\"idNonBariatric" + totalNonBariatrics + "div\")'></td></tr></table></div>";
                        }
                        nonBariatricDiv.InnerHtml = tempNonBariatricList;
                    }
                    totalNonBariatric.Value = totalNonBariatrics.ToString();
                }

            }
            setPhotoVisibility();
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Patient Data Form", "Loading Patient EMR - LoadPatientData function", err.ToString());
        }
    }
    #endregion

    #region setPhotoVisibility()
    private void setPhotoVisibility()
    {
        if (detailsPhoto.ImageUrl == "")
        {
            lblPhoto.Style["display"] = "block";
            uploadPhoto.Style["display"] = "block";
            clearFileUpload.Style["display"] = "block";
            changeFileUpload.Style["display"] = "none";
            divPhoto.Style["display"] = "none";
            cancelFileUpload.Style["display"] = "none";

            tempPhotoDisplay.Value = "none";
        }
        else
        {
            lblPhoto.Style["display"] = "none";
            uploadPhoto.Style["display"] = "none";
            clearFileUpload.Style["display"] = "none";
            cancelFileUpload.Style["display"] = "none";
            changeFileUpload.Style["display"] = "block";
            divPhoto.Style["display"] = "block";

            tempPhotoDisplay.Value = "display";
        }
    }
    #endregion

    #region private void LoadPatientData_DemoGraphics(DataView dvPatient)
    private void LoadPatientData_DemoGraphics(DataView dvPatient)
    {
        txtPatient_CustomID.Text = dvPatient[0]["Patient_CustomID"].ToString().Equals(String.Empty) ? dvPatient[0]["PatientID"].ToString() : dvPatient[0]["Patient_CustomID"].ToString();
        txtPatientID.Value = dvPatient[0]["PatientID"].ToString();
        txtSurName.Text = dvPatient[0]["Surname"].ToString();
        txtFirstName.Text = dvPatient[0]["Firstname"].ToString();
        txtStreet.Text = dvPatient[0]["Street"].ToString();
        txtCity.Text = dvPatient[0]["Suburb"].ToString();
        txtState.Text = dvPatient[0]["State"].ToString();
        txtPostCode.Text = dvPatient[0]["Postcode"].ToString();
        txtInsurance.Text = dvPatient[0]["Insurance"].ToString();
        txtEmail.Text = dvPatient[0]["EmailAddress"].ToString();
        txtPhone_H.Text = dvPatient[0]["HomePhone"].ToString();
        txtPhone_W.Text = dvPatient[0]["WorkPhone"].ToString();
        txtMobile.Text = dvPatient[0]["MobilePhone"].ToString();
        txtBirthDate.Text = gClass.TruncateDate(dvPatient[0]["BirthDate"].ToString().Trim(), Request.Cookies["CultureInfo"].Value, 1);
        txtConsultationDate.Text = gClass.TruncateDate(dvPatient[0]["ConsultationDate"].ToString().Trim(), Request.Cookies["CultureInfo"].Value, 1);
        cmbSurgon.SelectedValue = dvPatient[0]["DoctorID"].ToString();
        txtHReferredBy.Value = dvPatient[0]["RefDrId1"].ToString();
        txtHOtherDoctors1.Value = dvPatient[0]["RefDrId2"].ToString();
        txtHOtherDoctors2.Value = dvPatient[0]["RefDrId3"].ToString();
        cmbRace.SelectedValue = dvPatient[0]["Race"].ToString();
        rblTitle.Value = dvPatient[0]["Title"].ToString();
        rblGender.SelectedValue = dvPatient[0]["Sex"].ToString();
        lblAge.InnerText = CalculateAge(txtBirthDate.Text);

        txtReferredBy.Text = txtHReferredBy.Value.Replace("`", "'");
        txtOtherDoctors1.Text = txtHOtherDoctors1.Value.Replace("`", "'");
        txtOtherDoctors2.Text = txtHOtherDoctors2.Value.Replace("`", "'");
        txtRefDuration1.SelectedValue = dvPatient[0]["RefDrDuration1"].ToString();
        txtRefDuration2.SelectedValue = dvPatient[0]["RefDrDuration2"].ToString();
        txtRefDuration3.SelectedValue = dvPatient[0]["RefDrDuration3"].ToString();
        txtRefStatus1.SelectedValue = dvPatient[0]["RefDrStatus1"].ToString();
        txtRefStatus2.SelectedValue = dvPatient[0]["RefDrStatus2"].ToString();
        txtRefStatus3.SelectedValue = dvPatient[0]["RefDrStatus3"].ToString();
        txtRefDate1.Text = gClass.TruncateDate(dvPatient[0]["RefDrDate1"].ToString().Trim(), Request.Cookies["CultureInfo"].Value, 1);
        txtRefDate2.Text = gClass.TruncateDate(dvPatient[0]["RefDrDate2"].ToString().Trim(), Request.Cookies["CultureInfo"].Value, 1);
        txtRefDate3.Text = gClass.TruncateDate(dvPatient[0]["RefDrDate3"].ToString().Trim(), Request.Cookies["CultureInfo"].Value, 1);

        SetReferredDoctors(txtReferredBy, txtHReferredBy.Value.Replace("`", "'"));
        SetReferredDoctors(txtOtherDoctors1, txtHOtherDoctors1.Value.Replace("`", "'"));
        SetReferredDoctors(txtOtherDoctors2, txtHOtherDoctors2.Value.Replace("`", "'"));

        txtSocialHistory.Text = dvPatient[0]["SocialHistory"].ToString();
        txtMedicalSummary.Text = dvPatient[0]["MedicalSummary"].ToString();
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
            rowLogo.Style["display"] = "none";
            txtHLogoMandatory.Value = "0";
        }
        else if (url.ToUpper().IndexOf("AIGB") >= 0 && dsLogo.Tables[0].Rows.Count > 0)
        {
            //AIGB Setting
            txtHLogoMandatory.Value = "1";
        }
    }
    #endregion

    #region private void LoadPatientData_BoldDataGroup(string section)
    private void LoadPatientData_BoldDataGroup(string section)
    {
        SqlCommand cmdSelect = new SqlCommand();
        DataSet dsPatient = new DataSet();
        DataSet dsComorbidityCheck = new DataSet();

        if (section == "" || section == "previoussurgery")
        {
            //1) loading Patient BOLD Data (Bold Baseline data)
            cmdSelect.Parameters.Clear();
            gClass.MakeStoreProcedureName(ref cmdSelect, "sp_PatientData_LoadBoldData", true);
            cmdSelect.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
            cmdSelect.Parameters.Add("@PatientID", System.Data.SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
            dsPatient = gClass.FetchData(cmdSelect, "tblPatient_BoldData");
            if ((dsPatient.Tables.Count > 0) && (dsPatient.Tables[0].Rows.Count > 0))
                LoadPatientData_BOLDData(dsPatient.Tables[0].DefaultView, section);
        }

        if (section == "" || section =="comorbidity")
        {
            //2) loading Bold Comorbidity and Medications/Vitamins data
            cmdSelect.Parameters.Clear();
            gClass.MakeStoreProcedureName(ref cmdSelect, "sp_ConsultFU1_ProgressNotes_LoadBoldComorbidityData", true);
            cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
            cmdSelect.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
            cmdSelect.Parameters.Add("@ConsultId", SqlDbType.Int).Value = 0;
            dsPatient = gClass.FetchData(cmdSelect, "tblPatient_BoldData");
            if ((dsPatient.Tables.Count > 0) && (dsPatient.Tables[0].Rows.Count > 0))
                LoadPatientData_BOLDComorbidityData(dsPatient.Tables[0].DefaultView, section);


            if (Request.Cookies["SubmitData"].Value == "")
            {
                cmdSelect.Parameters.Clear();
                gClass.MakeStoreProcedureName(ref cmdSelect, "sp_ConsultFU1_ProgressNotes_LoadComorbidityCheck", true);
                cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                cmdSelect.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
                dsComorbidityCheck = gClass.FetchData(cmdSelect, "tblPatient_ComorbidityCheck");

                if ((dsComorbidityCheck.Tables.Count > 0) && (dsComorbidityCheck.Tables[0].Rows.Count > 0))
                    LoadPatientData_ComobidityCheck(dsComorbidityCheck.Tables[0].DefaultView, section);
            }

        }
    }
    #endregion

    #region private void LoadPatientData_BOLDData(DataView dvPatient, string section)
    /// <summary>
    /// this function is to load patient bold data (used for baseline bold data)
    /// </summary>
    /// <history>
    ///     <change author = "Ali Farahani" date="01 Nov 07" version = "1.0"/>
    /// </history>
    /// <param name="dvPatient">Default Data view including BOLD data</param>
    private void LoadPatientData_BOLDData(DataView dvPatient, string section)
    {
        if (section == "")
        {
            txtBoldChartNumber.Text = dvPatient[0]["BoldChartNumber"].ToString();

            if (dvPatient.Count == 0) return;
            txtChartNumber.Text = dvPatient[0]["ChartNumber"].ToString();

            txtSSN.Text = dvPatient[0]["SocialSecurityNumber"].ToString();
            cmbEmployment.SelectedValue = dvPatient[0]["EmploymentStatus"].ToString();
            txtEmployer.Text = dvPatient[0]["EmployerName"].ToString();
            txtInsuranceNumber.Text = dvPatient[0]["InsuranceNumber"].ToString();

            if (dvPatient[0]["HasConsentedToSRC"].ToString().Equals(Boolean.TrueString))
            {
                rdSend2SRC_Yes.Checked = true;
                rdSend2SRC_No.Checked = false;
            }
            else
            {
                rdSend2SRC_Yes.Checked = false;
                rdSend2SRC_No.Checked = true;
            }


            rbHealthInsurance_Yes.Checked = false;
            rbHealthInsurance_No.Checked = false;
            rbHealthInsurance_Unknown.Checked = false;
            switch (dvPatient[0]["IsCoverProcedure"].ToString())
            {
                case "1":
                    rbHealthInsurance_Yes.Checked = true;
                    break;
                case "0":
                    rbHealthInsurance_No.Checked = true;
                    break;
                case "-1":
                    rbHealthInsurance_Unknown.Checked = true;
                    break;
            }

            txtSecondaryCoverage.Text = dvPatient[0]["SecondaryInsurance"].ToString();
            txtTertiaryCoverage.Text = dvPatient[0]["TertiaryInsurance"].ToString();

            chkSelfPay.Checked = dvPatient[0]["IsSelfPay"].ToString().Equals("True");
            chkCharity.Checked = dvPatient[0]["IsCharity"].ToString().Equals("True");
            chkMedicare.Checked = dvPatient[0]["IsMedicare"].ToString().Equals("True");
            chkMedicaid.Checked = dvPatient[0]["IsMedicaid"].ToString().Equals("True");
            chkPrivateIns.Checked = dvPatient[0]["IsPrivateInsurance"].ToString().Equals("True");
            chkGovernmentIns.Checked = dvPatient[0]["IsGovernmentInsurance"].ToString().Equals("True");

            chkPreOPWeightLoss.Checked = Decimal.TryParse(dvPatient[0]["PreOperativeWeightLoss"].ToString(), out decTemp);
            txtPreOpWeightLoss.Text = dvPatient[0]["PreOperativeWeightLoss"].ToString();

            cmbDietaryWeightLoss.SelectedValue = dvPatient[0]["DietryWeightLoss"].ToString();
            chkDietWeightLoss.Checked = !dvPatient[0]["DietryWeightLoss"].ToString().Equals("0");

            chkDurationObesity.Checked = dvPatient[0]["DurationObesity"].ToString().Equals("True");
            chkSmokingCessation.Checked = dvPatient[0]["SmokingCessation"].ToString().Equals("True");
            chkMentalHealthClearance.Checked = dvPatient[0]["MentalHealthClearance"].ToString().Equals("True");
            chkIntelligenceTesting.Checked = dvPatient[0]["IQTesting"].ToString().Equals("True");
            chkPreCert_Other.Checked = dvPatient[0]["PreCertification_Other"].ToString().Trim().Length > 0;
            txtPreCert_Other.Text = dvPatient[0]["PreCertification_Other"].ToString();
            txtPreCert_Other.Enabled = txtPreCert_Other.Text.Trim().Length > 0;
        }

        if (section == "" || section == "previoussurgery")
        {
            txtPrevBariatricSurgery_Selected.Value = dvPatient[0]["PBS_Procedure"].ToString();
            txtPrevBarSurgery_Year.Text = dvPatient[0]["PBS_Year"].ToString();
            txtOrgWeight.Text = dvPatient[0]["OriginalWeight"].ToString();
            cmbSurgeon.SelectedValue = dvPatient[0]["PBS_SurgeonID"].ToString();

            if (dvPatient[0]["OriginalWeight_Actual"].ToString().Equals("True")) rbOrgWeight_Actual.Checked = true;
            else rbOrgWeight_Estimated.Checked = true;

            txtLowestWeightAchieved.Text = dvPatient[0]["LowestWeightAchieved"].ToString();

            if (dvPatient[0]["LowestWeightAchieved_Actual"].ToString().Equals("True")) rbLowestWeightAchieved_Actual.Checked = true;
            else rbLowestWeightAchieved_Estimated.Checked = true;

            txtAdverseEvents_Selected.Value = dvPatient[0]["PBS_Event"].ToString();
            txtPrevBariatricSurgery_Selected.Value = dvPatient[0]["PBS_Procedure"].ToString();
            txtPrevNonBariatricSurgery_Selected.Value = dvPatient[0]["PNBS_Procedure"].ToString();

            FillSelectedLists(cmbPrevBariatricSurgery, listPrevBariatricSurgery_Selected, txtPrevBariatricSurgery_Selected.Value);
            FillSelectedLists(cmbPrevNonBariatricSurgery, listPrevNonBariatricSurgery_Selected, txtPrevNonBariatricSurgery_Selected.Value);
            FillSelectedLists(cmbAdverseEvents, listAdverseEvents_Selected, txtAdverseEvents_Selected.Value);
        }

    }
    #endregion

    #region private void LoadPatientData_ComobidityCheck(DataView dvComobidity, string section)
    private void LoadPatientData_ComobidityCheck(DataView dvComobidity, string section)
    {
        if (section == "")
        {
            chkDiabDef.Checked = dvComobidity[0]["AUS_EndDiab"].ToString().Equals(Boolean.TrueString);
            chkThyDef.Checked = dvComobidity[0]["AUS_EndThy"].ToString().Equals(Boolean.TrueString);
            chkEndOtherDef.Checked = dvComobidity[0]["AUS_EndOther"].ToString().Equals(Boolean.TrueString);
            chkAsthmaDef.Checked = dvComobidity[0]["AUS_PulAsthma"].ToString().Equals(Boolean.TrueString);
            chkApneaDef.Checked = dvComobidity[0]["AUS_PulApnea"].ToString().Equals(Boolean.TrueString);
            chkEmbDef.Checked = dvComobidity[0]["AUS_PulEmb"].ToString().Equals(Boolean.TrueString);
            chkPulOtherDef.Checked = dvComobidity[0]["AUS_PulOther"].ToString().Equals(Boolean.TrueString);
            chkRefDef.Checked = dvComobidity[0]["AUS_GasRef"].ToString().Equals(Boolean.TrueString);
            chkUlcDef.Checked = dvComobidity[0]["AUS_GasUlc"].ToString().Equals(Boolean.TrueString);
            chkGallDef.Checked = dvComobidity[0]["AUS_GasGall"].ToString().Equals(Boolean.TrueString);
            chkHepDef.Checked = dvComobidity[0]["AUS_GasHep"].ToString().Equals(Boolean.TrueString);
            chkGasOtherDef.Checked = dvComobidity[0]["AUS_GasOther"].ToString().Equals(Boolean.TrueString);
            chkIscDef.Checked = dvComobidity[0]["AUS_CvsIsc"].ToString().Equals(Boolean.TrueString);
            chkBloodDef.Checked = dvComobidity[0]["AUS_CvsBlood"].ToString().Equals(Boolean.TrueString);
            chkColDef.Checked = dvComobidity[0]["AUS_CvsCol"].ToString().Equals(Boolean.TrueString);
            chkDVTDef.Checked = dvComobidity[0]["AUS_CvsDVT"].ToString().Equals(Boolean.TrueString);
            chkVenDef.Checked = dvComobidity[0]["AUS_CvsVen"].ToString().Equals(Boolean.TrueString);
            chkAntiDef.Checked = dvComobidity[0]["AUS_CvsAnti"].ToString().Equals(Boolean.TrueString);
            chkCvsOtherDef.Checked = dvComobidity[0]["AUS_CvsOther"].ToString().Equals(Boolean.TrueString);
            chkDepDef.Checked = dvComobidity[0]["AUS_PsyDep"].ToString().Equals(Boolean.TrueString);
            chkAnxDef.Checked = dvComobidity[0]["AUS_PsyAnx"].ToString().Equals(Boolean.TrueString);
            chkPhobDef.Checked = dvComobidity[0]["AUS_PsyPhob"].ToString().Equals(Boolean.TrueString);
            chkEatDef.Checked = dvComobidity[0]["AUS_PsyEat"].ToString().Equals(Boolean.TrueString);
            chkHeadDef.Checked = dvComobidity[0]["AUS_PsyHead"].ToString().Equals(Boolean.TrueString);
            chkStrokeDef.Checked = dvComobidity[0]["AUS_PsyStroke"].ToString().Equals(Boolean.TrueString);
            chkPsyOtherDef.Checked = dvComobidity[0]["AUS_PsyOther"].ToString().Equals(Boolean.TrueString);
            chkBackDef.Checked = dvComobidity[0]["AUS_MuscBack"].ToString().Equals(Boolean.TrueString);
            chkHipDef.Checked = dvComobidity[0]["AUS_MuscHip"].ToString().Equals(Boolean.TrueString);
            chkKneeDef.Checked = dvComobidity[0]["AUS_MuscKnee"].ToString().Equals(Boolean.TrueString);
            chkFeetDef.Checked = dvComobidity[0]["AUS_MuscFeet"].ToString().Equals(Boolean.TrueString);
            chkFibrDef.Checked = dvComobidity[0]["AUS_MuscFibr"].ToString().Equals(Boolean.TrueString);
            chkMuscOtherDef.Checked = dvComobidity[0]["AUS_MuscOther"].ToString().Equals(Boolean.TrueString);
            chkInfDef.Checked = dvComobidity[0]["AUS_GenInf"].ToString().Equals(Boolean.TrueString);
            chkRenDef.Checked = dvComobidity[0]["AUS_GenRen"].ToString().Equals(Boolean.TrueString);
            chkUriDef.Checked = dvComobidity[0]["AUS_GenUri"].ToString().Equals(Boolean.TrueString);
            chkPsoDef.Checked = dvComobidity[0]["AUS_OtherPso"].ToString().Equals(Boolean.TrueString);
            chkSkinDef.Checked = dvComobidity[0]["AUS_OtherSkin"].ToString().Equals(Boolean.TrueString);
            chkCancerDef.Checked = dvComobidity[0]["AUS_OtherCancer"].ToString().Equals(Boolean.TrueString);
            chkAnemiaDef.Checked = dvComobidity[0]["AUS_OtherAnemia"].ToString().Equals(Boolean.TrueString);
            chkOtherOtherDef.Checked = dvComobidity[0]["AUS_OtherOther"].ToString().Equals(Boolean.TrueString);
        }
    }
    #endregion

    #region private void LoadPatientData_BOLDComorbidityData(DataView dvPatient, string section)
    private void LoadPatientData_BOLDComorbidityData(DataView dvPatient, string section)
    {
        String[] comorbidities;
        String[] comorbidity;
        String comorbidityName;
        String comorbidityNotes;
        String tempComorbidityList = "";
        Int32 totalComorbidity = 0;

        if (section == "")
        {
            cmbHypertension.SelectedValue = dvPatient[0]["CVS_Hypertension"].ToString();
            cmbCongestive.SelectedValue = dvPatient[0]["CVS_Congestive"].ToString();
            cmbIschemic.SelectedValue = dvPatient[0]["CVS_Ischemic"].ToString();
            cmbAngina.SelectedValue = dvPatient[0]["CVS_Angina"].ToString();
            cmbPeripheral.SelectedValue = dvPatient[0]["CVS_Peripheral"].ToString();
            cmbLower.SelectedValue = dvPatient[0]["CVS_Lower"].ToString();
            cmbDVT.SelectedValue = dvPatient[0]["CVS_DVT"].ToString();
            cmbGlucose.SelectedValue = dvPatient[0]["MET_Glucose"].ToString();
            cmbLipids.SelectedValue = dvPatient[0]["MET_Lipids"].ToString();
            cmbGout.SelectedValue = dvPatient[0]["MET_Gout"].ToString();
            cmbObstructive.SelectedValue = dvPatient[0]["PUL_Obstructive"].ToString();
            cmbObesity.SelectedValue = dvPatient[0]["PUL_Obesity"].ToString();
            cmbPulmonary.SelectedValue = dvPatient[0]["PUL_PulHypertension"].ToString();
            cmbAsthma.SelectedValue = dvPatient[0]["PUL_Asthma"].ToString();
            cmbGred.SelectedValue = dvPatient[0]["GAS_Gerd"].ToString();
            cmbCholelithiasis.SelectedValue = dvPatient[0]["GAS_Cholelithiasis"].ToString();
            cmbLiver.SelectedValue = dvPatient[0]["GAS_Liver"].ToString();
            cmbBackPain.SelectedValue = dvPatient[0]["MUS_BackPain"].ToString();
            cmbMusculoskeletal.SelectedValue = dvPatient[0]["MUS_Musculoskeletal"].ToString();
            cmbFibro.SelectedValue = dvPatient[0]["MUS_Fibromyalgia"].ToString();
            cmbPolycystic.SelectedValue = dvPatient[0]["REPRD_Polycystic"].ToString();
            cmbMenstrual.SelectedValue = dvPatient[0]["REPRD_Menstrual"].ToString();
            cmbPsychosocial.SelectedValue = dvPatient[0]["PSY_Impairment"].ToString();
            cmbDepression.SelectedValue = dvPatient[0]["PSY_Depression"].ToString();
            cmbConfirmed.SelectedValue = dvPatient[0]["PSY_MentalHealth"].ToString();
            cmbAlcohol.SelectedValue = dvPatient[0]["PSY_Alcohol"].ToString();
            cmbTobacco.SelectedValue = dvPatient[0]["PSY_Tobacoo"].ToString();
            cmbAbuse.SelectedValue = dvPatient[0]["PSY_Abuse"].ToString();
            cmbStressUrinary.SelectedValue = dvPatient[0]["GEN_Stress"].ToString();
            cmbCerebri.SelectedValue = dvPatient[0]["GEN_Cerebri"].ToString();
            cmbHernia.SelectedValue = dvPatient[0]["GEN_Hernia"].ToString();
            cmbFunctional.SelectedValue = dvPatient[0]["GEN_Functional"].ToString();
            cmbSkin.SelectedValue = dvPatient[0]["GEN_Skin"].ToString();

            cmbRenalInsuff.SelectedValue = dvPatient[0]["GEN_RenalInsuff"].ToString();
            cmbRenalFail.SelectedValue = dvPatient[0]["GEN_RenalFail"].ToString();
            cmbSteroid.SelectedValue = dvPatient[0]["GEN_Steroid"].ToString();
            cmbTherapeutic.SelectedValue = dvPatient[0]["GEN_Therapeutic"].ToString();
            cmbPrevPCISurgery.SelectedValue = dvPatient[0]["GEN_PrevPCISurgery"].ToString();


            cmbSmokerACS.SelectedValue = dvPatient[0]["ACS_Smoke"].ToString();
            cmbOxygenACS.SelectedValue = dvPatient[0]["ACS_Oxy"].ToString();
            cmbEmbolismACS.SelectedValue = dvPatient[0]["ACS_Embo"].ToString();
            cmbCopdACS.SelectedValue = dvPatient[0]["ACS_Copd"].ToString();
            cmbCpapACS.SelectedValue = dvPatient[0]["ACS_Cpap"].ToString();
            cmbGerdACS.SelectedValue = dvPatient[0]["ACS_Gerd"].ToString();
            cmbGallstoneACS.SelectedValue = dvPatient[0]["ACS_Gal"].ToString();
            cmbMusculoDiseaseACS.SelectedValue = dvPatient[0]["ACS_Muscd"].ToString();
            cmbActivityLimitedACS.SelectedValue = dvPatient[0]["ACS_Pain"].ToString();
            cmbDailyMedACS.SelectedValue = dvPatient[0]["ACS_Meds"].ToString();
            cmbSurgicalACS.SelectedValue = dvPatient[0]["ACS_Surg"].ToString();
            cmbMobilityACS.SelectedValue = dvPatient[0]["ACS_Mob"].ToString();
            cmbRenalInsuffACS.SelectedValue = dvPatient[0]["GEN_RenalInsuff"].ToString();
            cmbRenalFailACS.SelectedValue = dvPatient[0]["GEN_RenalFail"].ToString();
            cmbUrinaryACS.SelectedValue = dvPatient[0]["ACS_Uri"].ToString();
            cmbMyocardinalACS.SelectedValue = dvPatient[0]["ACS_Myo"].ToString();
            cmbPrevPCIACS.SelectedValue = dvPatient[0]["ACS_Pci"].ToString();
            cmbPrevCardiacACS.SelectedValue = dvPatient[0]["ACS_Csurg"].ToString();
            cmbHyperlipidemiaACS.SelectedValue = dvPatient[0]["ACS_Lipid"].ToString();
            cmbHypertensionACS.SelectedValue = dvPatient[0]["ACS_Hyper"].ToString();
            cmbDVTACS.SelectedValue = dvPatient[0]["ACS_Dvt"].ToString();
            cmbVenousACS.SelectedValue = dvPatient[0]["ACS_Venous"].ToString();
            cmbHealthStatusACS.SelectedValue = dvPatient[0]["ACS_Health"].ToString();
            cmbDiabetesACS.SelectedValue = dvPatient[0]["ACS_Diab"].ToString();
            cmbSteroidsACS.SelectedValue = dvPatient[0]["GEN_Steroid"].ToString();
            cmbAnticogulationACS.SelectedValue = dvPatient[0]["GEN_Therapeutic"].ToString();
            cmbObesityACS.SelectedValue = dvPatient[0]["ACS_Obese"].ToString();
            cmbShoACS.SelectedValue = dvPatient[0]["ACS_Sho"].ToString();
            cmbFatACS.SelectedValue = dvPatient[0]["ACS_Fat"].ToString();

            txtcmbHypertension.Text = dvPatient[0]["CVS_Hypertension_Notes"].ToString();
            txtcmbCongestive.Text = dvPatient[0]["CVS_Congestive_Notes"].ToString();
            txtcmbIschemic.Text = dvPatient[0]["CVS_Ischemic_Notes"].ToString();
            txtcmbAngina.Text = dvPatient[0]["CVS_Angina_Notes"].ToString();
            txtcmbPeripheral.Text = dvPatient[0]["CVS_Peripheral_Notes"].ToString();
            txtcmbLower.Text = dvPatient[0]["CVS_Lower_Notes"].ToString();
            txtcmbDVT.Text = dvPatient[0]["CVS_DVT_Notes"].ToString();
            txtcmbGlucose.Text = dvPatient[0]["MET_Glucose_Notes"].ToString();
            txtcmbLipids.Text = dvPatient[0]["MET_Lipids_Notes"].ToString();
            txtcmbGout.Text = dvPatient[0]["MET_Gout_Notes"].ToString();
            txtcmbObstructive.Text = dvPatient[0]["PUL_Obstructive_Notes"].ToString();
            txtcmbObesity.Text = dvPatient[0]["PUL_Obesity_Notes"].ToString();
            txtcmbPulmonary.Text = dvPatient[0]["PUL_PulHypertension_Notes"].ToString();
            txtcmbAsthma.Text = dvPatient[0]["PUL_Asthma_Notes"].ToString();
            txtcmbGred.Text = dvPatient[0]["GAS_Gerd_Notes"].ToString();
            txtcmbCholelithiasis.Text = dvPatient[0]["GAS_Cholelithiasis_Notes"].ToString();
            txtcmbLiver.Text = dvPatient[0]["GAS_Liver_Notes"].ToString();
            txtcmbBackPain.Text = dvPatient[0]["MUS_BackPain_Notes"].ToString();
            txtcmbMusculoskeletal.Text = dvPatient[0]["MUS_Musculoskeletal_Notes"].ToString();
            txtcmbFibro.Text = dvPatient[0]["MUS_Fibromyalgia_Notes"].ToString();
            txtcmbPolycystic.Text = dvPatient[0]["REPRD_Polycystic_Notes"].ToString();
            txtcmbMenstrual.Text = dvPatient[0]["REPRD_Menstrual_Notes"].ToString();
            txtcmbPsychosocial.Text = dvPatient[0]["PSY_Impairment_Notes"].ToString();
            txtcmbDepression.Text = dvPatient[0]["PSY_Depression_Notes"].ToString();
            txtcmbConfirmed.Text = dvPatient[0]["PSY_MentalHealth_Notes"].ToString();
            txtcmbAlcohol.Text = dvPatient[0]["PSY_Alcohol_Notes"].ToString();
            txtcmbTobacco.Text = dvPatient[0]["PSY_Tobacoo_Notes"].ToString();
            txtcmbAbuse.Text = dvPatient[0]["PSY_Abuse_Notes"].ToString();
            txtcmbStressUrinary.Text = dvPatient[0]["GEN_Stress_Notes"].ToString();
            txtcmbCerebri.Text = dvPatient[0]["GEN_Cerebri_Notes"].ToString();
            txtcmbHernia.Text = dvPatient[0]["GEN_Hernia_Notes"].ToString();
            txtcmbFunctional.Text = dvPatient[0]["GEN_Functional_Notes"].ToString();
            txtcmbSkin.Text = dvPatient[0]["GEN_Skin_Notes"].ToString();

            txtcmbRenalInsuff.Text = dvPatient[0]["GEN_RenalInsuff_Notes"].ToString();
            txtcmbRenalFail.Text = dvPatient[0]["GEN_RenalFail_Notes"].ToString();
            txtcmbSteroid.Text = dvPatient[0]["GEN_Steroid_Notes"].ToString();
            txtcmbTherapeutic.Text = dvPatient[0]["GEN_Therapeutic_Notes"].ToString();
            txtcmbPrevPCISurgery.Text = dvPatient[0]["GEN_PrevPCISurgery_Notes"].ToString();


            txtcmbSmokerACS.Text = dvPatient[0]["ACS_Smoke_Notes"].ToString();
            txtcmbOxygenACS.Text = dvPatient[0]["ACS_Oxy_Notes"].ToString();
            txtcmbEmbolismACS.Text = dvPatient[0]["ACS_Embo_Notes"].ToString();
            txtcmbCopdACS.Text = dvPatient[0]["ACS_Copd_Notes"].ToString();
            txtcmbCpapACS.Text = dvPatient[0]["ACS_Cpap_Notes"].ToString();
            txtcmbGerdACS.Text = dvPatient[0]["ACS_Gerd_Notes"].ToString();
            txtcmbGallstoneACS.Text = dvPatient[0]["ACS_Gal_Notes"].ToString();
            txtcmbMusculoDiseaseACS.Text = dvPatient[0]["ACS_Muscd_Notes"].ToString();
            txtcmbActivityLimitedACS.Text = dvPatient[0]["ACS_Pain_Notes"].ToString();
            txtcmbDailyMedACS.Text = dvPatient[0]["ACS_Meds_Notes"].ToString();
            txtcmbSurgicalACS.Text = dvPatient[0]["ACS_Surg_Notes"].ToString();
            txtcmbMobilityACS.Text = dvPatient[0]["ACS_Mob_Notes"].ToString();
            txtcmbRenalInsuffACS.Text = dvPatient[0]["GEN_RenalInsuff_Notes"].ToString();
            txtcmbRenalFailACS.Text = dvPatient[0]["GEN_RenalFail_Notes"].ToString();
            txtcmbUrinaryACS.Text = dvPatient[0]["ACS_Uri_Notes"].ToString();
            txtcmbMyocardinalACS.Text = dvPatient[0]["ACS_Myo_Notes"].ToString();
            txtcmbPrevPCIACS.Text = dvPatient[0]["ACS_Pci_Notes"].ToString();
            txtcmbPrevCardiacACS.Text = dvPatient[0]["ACS_Csurg_Notes"].ToString();
            txtcmbHyperlipidemiaACS.Text = dvPatient[0]["ACS_Lipid_Notes"].ToString();
            txtcmbHypertensionACS.Text = dvPatient[0]["ACS_Hyper_Notes"].ToString();
            txtcmbDVTACS.Text = dvPatient[0]["ACS_Dvt_Notes"].ToString();
            txtcmbVenousACS.Text = dvPatient[0]["ACS_Venous_Notes"].ToString();
            txtcmbHealthStatusACS.Text = dvPatient[0]["ACS_Health_Notes"].ToString();
            txtcmbDiabetesACS.Text = dvPatient[0]["ACS_Diab_Notes"].ToString();
            txtcmbSteroidsACS.Text = dvPatient[0]["GEN_Steroid_Notes"].ToString();
            txtcmbAnticogulationACS.Text = dvPatient[0]["GEN_Therapeutic_Notes"].ToString();
            txtcmbObesityACS.Text = dvPatient[0]["ACS_Obese_Notes"].ToString();
            txtcmbNotesACS.Text = dvPatient[0]["ACS_General_Notes"].ToString();
            txtcmbShoACS.Text = dvPatient[0]["ACS_Sho_Notes"].ToString();
            txtcmbFatACS.Text = dvPatient[0]["ACS_Fat_Notes"].ToString();

            txtDiabDef.Text = dvPatient[0]["AUS_EndDiab"].ToString();
            txtThyDef.Text = dvPatient[0]["AUS_EndThy"].ToString();
            txtEndOtherNameDef.Text = dvPatient[0]["AUS_EndOtherName"].ToString();
            txtEndOtherDescDef.Text = dvPatient[0]["AUS_EndOtherDesc"].ToString();
            txtAsthmaDef.Text = dvPatient[0]["AUS_PulAsthma"].ToString();
            txtApneaDef.Text = dvPatient[0]["AUS_PulApnea"].ToString();
            txtEmbDef.Text = dvPatient[0]["AUS_PulEmb"].ToString();
            txtPulOtherNameDef.Text = dvPatient[0]["AUS_PulOtherName"].ToString();
            txtPulOtherDescDef.Text = dvPatient[0]["AUS_PulOtherDesc"].ToString();
            txtRefDef.Text = dvPatient[0]["AUS_GasRef"].ToString();
            txtUlcDef.Text = dvPatient[0]["AUS_GasUlc"].ToString();
            txtGallDef.Text = dvPatient[0]["AUS_GasGall"].ToString();
            txtHepDef.Text = dvPatient[0]["AUS_GasHep"].ToString();
            txtGasOtherNameDef.Text = dvPatient[0]["AUS_GasOtherName"].ToString();
            txtGasOtherDescDef.Text = dvPatient[0]["AUS_GasOtherDesc"].ToString();
            txtIscDef.Text = dvPatient[0]["AUS_CvsIsc"].ToString();
            txtBloodDef.Text = dvPatient[0]["AUS_CvsBlood"].ToString();
            txtColDef.Text = dvPatient[0]["AUS_CvsCol"].ToString();
            txtDVTDef.Text = dvPatient[0]["AUS_CvsDVT"].ToString();
            txtVenDef.Text = dvPatient[0]["AUS_CvsVen"].ToString();
            txtAntiDef.Text = dvPatient[0]["AUS_CvsAnti"].ToString();
            txtCvsOtherNameDef.Text = dvPatient[0]["AUS_CvsOtherName"].ToString();
            txtCvsOtherDescDef.Text = dvPatient[0]["AUS_CvsOtherDesc"].ToString();
            txtDepDef.Text = dvPatient[0]["AUS_PsyDep"].ToString();
            txtAnxDef.Text = dvPatient[0]["AUS_PsyAnx"].ToString();
            txtPhobDef.Text = dvPatient[0]["AUS_PsyPhob"].ToString();
            txtEatDef.Text = dvPatient[0]["AUS_PsyEat"].ToString();
            txtHeadDef.Text = dvPatient[0]["AUS_PsyHead"].ToString();
            txtStrokeDef.Text = dvPatient[0]["AUS_PsyStroke"].ToString();
            txtPsyOtherNameDef.Text = dvPatient[0]["AUS_PsyOtherName"].ToString();
            txtPsyOtherDescDef.Text = dvPatient[0]["AUS_PsyOtherDesc"].ToString();
            txtBackDef.Text = dvPatient[0]["AUS_MuscBack"].ToString();
            txtHipDef.Text = dvPatient[0]["AUS_MuscHip"].ToString();
            txtKneeDef.Text = dvPatient[0]["AUS_MuscKnee"].ToString();
            txtFeetDef.Text = dvPatient[0]["AUS_MuscFeet"].ToString();
            txtFibrDef.Text = dvPatient[0]["AUS_MuscFibr"].ToString();
            txtMuscOtherNameDef.Text = dvPatient[0]["AUS_MuscOtherName"].ToString();
            txtMuscOtherDescDef.Text = dvPatient[0]["AUS_MuscOtherDesc"].ToString();
            txtInfDef.Text = dvPatient[0]["AUS_GenInf"].ToString();
            txtRenDef.Text = dvPatient[0]["AUS_GenRen"].ToString();
            txtUriDef.Text = dvPatient[0]["AUS_GenUri"].ToString();
            txtPsoDef.Text = dvPatient[0]["AUS_OtherPso"].ToString();
            txtSkinDef.Text = dvPatient[0]["AUS_OtherSkin"].ToString();
            txtCancerDef.Text = dvPatient[0]["AUS_OtherCancer"].ToString();
            txtAnemiaDef.Text = dvPatient[0]["AUS_OtherAnemia"].ToString();
            txtOtherOtherNameDef.Text = dvPatient[0]["AUS_OtherOtherName"].ToString();
            txtOtherOtherDescDef.Text = dvPatient[0]["AUS_OtherOtherDesc"].ToString();

            chkComorbidityReview.Checked = dvPatient[0]["Comorbidity_Review"].ToString().Equals(Boolean.TrueString);
        }
        otherComorbiditiesDiv.InnerHtml = "";
        if (section == "" || section == "comorbidity")
        {
            if (dvPatient[0]["Extra_Comorbidity"].ToString().Trim() != "")
            {
                comorbidities = dvPatient[0]["Extra_Comorbidity"].ToString().Split('+');

                foreach (string tempComorbidity in comorbidities)
                {
                    totalComorbidity++;
                    comorbidity = tempComorbidity.Split('*');
                    comorbidityName = comorbidity[0];
                    comorbidityNotes = comorbidity[1];

                    tempComorbidityList += "<div id='idCom" + totalComorbidity + "div'><table><tr><td style='width: 25%' valign='top'><input type=hidden name=delComorbidities" + totalComorbidity + " value=no>Name: <input type=textbox runat='server' name='txtComorbiditiesName" + totalComorbidity + "' value='" + comorbidityName + "'></td><td valign='top' style='width:5%'>Notes:</td><td style='width: 65%' valign='top'><textarea ID='txtComorbiditiesNotes" + totalComorbidity + "' runat='server rows='2' cols='65'>" + comorbidityNotes + "</textarea></td><td width='5%' valign='top'><input type=button name=remove" + totalComorbidity + " value=' - ' onclick='javascript:removeComorbidities(" + totalComorbidity + ",\"idCom" + totalComorbidity + "div\")'></td></tr></table></div>";
                }
                otherComorbiditiesDiv.InnerHtml = tempComorbidityList;
            }
            totalComorbidities.Value = totalComorbidity.ToString();
        }
    }
    #endregion 
    
    #region private void LoadPatientData_Measurement(DataView dvPatient)
    private void LoadPatientData_Measurement(DataView dvPatient)
    {
        Boolean HasPatientVisit = dvPatient[0]["HasVisit"].ToString().Equals("True");
        Decimal decTemp = 0m;

        txtBMIHeight.Value = dvPatient[0]["BMIHeight"].ToString().Equals("") ? "0" : dvPatient[0]["BMIHeight"].ToString();
        txtBMI.Text = dvPatient[0]["InitBMI"].ToString().Equals("") ? "0" : (Decimal.TryParse(dvPatient[0]["InitBMI"].ToString(), out decTemp) ? decTemp.ToString(strNumberFormat) : "0");
        txtHHeight.Value = dvPatient[0]["Height"].ToString();
        txtHStartWeight.Value = dvPatient[0]["StartWeight"].ToString();
        txtHTargetWeight.Value = dvPatient[0]["TargetWeight"].ToString();
        txtHCurrentWeight.Value = dvPatient[0]["LastVisitWeight"].ToString(); // CurrentWeight
        txtMeasurementEWeight.Text = dvPatient[0]["ExcessWeight"].ToString();
        txtHIdealWeight.Value = dvPatient[0]["IdealWeight"].ToString();
        txtMeasurementTWeight.Text = dvPatient[0]["TargetWeight"].ToString();

        txtMeasurementNeck.Text = dvPatient[0]["StartNeck"].ToString();
        txtMeasurementWaist.Text = dvPatient[0]["StartWaist"].ToString();
        txtMeasurementHip.Text = dvPatient[0]["StartHip"].ToString();
        txtMeasurementPR.Text = dvPatient[0]["StartPR"].ToString();
        txtMeasurementRR.Text = dvPatient[0]["StartRR"].ToString();
        txtMeasurementBPLow.Text = dvPatient[0]["StartBP1"].ToString();
        txtMeasurementBPHigh.Text = dvPatient[0]["StartBP2"].ToString();

        txtHLapbandDate.Value = gClass.TruncateDate(dvPatient[0]["LapbandDate"].ToString().Trim(), Request.Cookies["CultureInfo"].Value, 1);
        txtZeroDate.Text = gClass.TruncateDate(dvPatient[0]["ZeroDate"].ToString().Trim(), Request.Cookies["CultureInfo"].Value, 1);

        if(dvPatient[0]["VisitWeeksFlag"].ToString() != "")
        cmbVisitWeeks.SelectedValue = dvPatient[0]["VisitWeeksFlag"].ToString();

        if (Decimal.TryParse(txtHIdealWeight.Value, out decTemp))
        {
            if (decTemp == 0)
            {
                Int16 intRefBMI;

                Int16.TryParse(txtHRefBMI.Value, out intRefBMI);

                if (Decimal.TryParse(txtBMIHeight.Value, out decTemp))
                    txtHIdealWeight.Value = (intRefBMI * Math.Pow((double)decTemp, (double)2)).ToString();
                else
                    txtHIdealWeight.Value = "0";
            }
        }
        else txtHIdealWeight.Value = "0";

        //lblCurrentWeight.Visible = HasPatientVisit;
        //txtCurrentWeight.Visible = HasPatientVisit;
        //lblCurrentEWL1.Visible = HasPatientVisit;
        //lblCurrentWeight_Value.Visible = HasPatientVisit;
        //lblCurrentWeightTitle.Visible = HasPatientVisit;

        if (txtUseImperial.Value.Equals("1"))
        {
            txtMeasurementHeight.Text = Decimal.TryParse(txtHHeight.Value, out decTemp) ? (decTemp / (Decimal)0.0254).ToString(strNumberFormat) : "0";
            txtMeasurementWeight.Text = Decimal.TryParse(txtHStartWeight.Value, out decTemp) ? (decTemp / (Decimal)0.45359237).ToString(strNumberFormat) : "0";
            txtIdealWeight.Text = Decimal.TryParse(txtHIdealWeight.Value, out decTemp) ? (decTemp / (Decimal)0.45359237).ToString(strNumberFormat) : "0";
            //txtCurrentWeight.Text = Decimal.TryParse(txtHCurrentWeight.Value, out decTemp) ? (decTemp / (Decimal)0.45359237).ToString(strNumberFormat) : "0";
            txtMeasurementEWeight.Text = Decimal.TryParse(txtMeasurementEWeight.Text, out decTemp) ? (decTemp / (Decimal)0.45359237).ToString(strNumberFormat) : "0";
            txtMeasurementTWeight.Text = Decimal.TryParse(txtMeasurementTWeight.Text, out decTemp) ? (decTemp / (Decimal)0.45359237).ToString(strNumberFormat) : "0";

            txtMeasurementNeck.Text = Decimal.TryParse(txtMeasurementNeck.Text, out decTemp) ? (decTemp / (Decimal)2.54).ToString(strNumberFormat) : "0";
            txtMeasurementWaist.Text = Decimal.TryParse(txtMeasurementWaist.Text, out decTemp) ? (decTemp / (Decimal)2.54).ToString(strNumberFormat) : "0";
            txtMeasurementHip.Text = Decimal.TryParse(txtMeasurementHip.Text, out decTemp) ? (decTemp / (Decimal)2.54).ToString(strNumberFormat) : "0";

            txtWeightHistoryGainWeight.Text = Decimal.TryParse(txtHWeightHistoryGainWeight.Value, out decTemp) ? (decTemp / (Decimal)0.45359237).ToString(strNumberFormat) : "0";
        }
        else
        {
            txtMeasurementHeight.Text = Decimal.TryParse(txtHHeight.Value, out decTemp) ? (decTemp * 100).ToString(strNumberFormat) : "0";
            txtMeasurementWeight.Text = Decimal.TryParse(txtHStartWeight.Value, out decTemp) ? decTemp.ToString(strNumberFormat) : "0";
            txtIdealWeight.Text = Decimal.TryParse(txtHIdealWeight.Value, out decTemp) ? decTemp.ToString(strNumberFormat) : "0";
            //txtCurrentWeight.Text = Decimal.TryParse(txtHCurrentWeight.Value, out decTemp) ? decTemp.ToString(strNumberFormat) : "0";
            txtMeasurementEWeight.Text = Decimal.TryParse(txtMeasurementEWeight.Text, out decTemp) ? decTemp.ToString(strNumberFormat) : "0";
            txtMeasurementTWeight.Text = Decimal.TryParse(txtMeasurementTWeight.Text, out decTemp) ? decTemp.ToString(strNumberFormat) : "0";
           
            txtMeasurementNeck.Text = Decimal.TryParse(txtMeasurementNeck.Text, out decTemp) ? decTemp.ToString(strNumberFormat) : "0";
            txtMeasurementWaist.Text = Decimal.TryParse(txtMeasurementWaist.Text, out decTemp) ? decTemp.ToString(strNumberFormat) : "0";
            txtMeasurementHip.Text = Decimal.TryParse(txtMeasurementHip.Text, out decTemp) ? decTemp.ToString(strNumberFormat) : "0";

            txtWeightHistoryGainWeight.Text = Decimal.TryParse(txtHWeightHistoryGainWeight.Value, out decTemp) ? decTemp.ToString(strNumberFormat) : "0";
        }
        //lblCurrentWeight_Value.InnerText = txtCurrentWeight.Text;
        lblMeasurementIWeight.Text = txtIdealWeight.Text;
        txtMeasurementNotes.Text = dvPatient[0]["Notes"].ToString();
    }
    #endregion

    #region private void LoadPatientPathology()
    private void LoadPatientPathology()
    {        
        SqlCommand cmdSelect = new SqlCommand();
        DataSet dsPatient = new DataSet();
        string pathologyResult = "";
        string name = "";
        string unit = "";
        string range = "";
        string status = "";
        string value = "";
        string previousSection = "";

        string fontColor;
        string fontNormal = "black";
        string fontAbNormal = "red";

        try
        {
            gClass.MakeStoreProcedureName(ref cmdSelect, "sp_PatientPathology_LoadSingleData", true);
            cmdSelect.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
            cmdSelect.Parameters.Add("@PatientID", System.Data.SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
            cmdSelect.Parameters.Add("@PathologyBaseline", System.Data.SqlDbType.VarChar).Value = "baseline";
            dsPatient = gClass.FetchData(cmdSelect, "tblPatient");

            if (dsPatient.Tables.Count > 0 && dsPatient.Tables[0].Rows.Count > 0)
            {
                pathologyResult += "<table>";
                for (int Xh = 0; Xh < dsPatient.Tables[0].Rows.Count; Xh++)
                {
                    if (previousSection!= "" && previousSection != dsPatient.Tables[0].Rows[Xh]["SectionID"].ToString())
                    {
                        pathologyResult += "<tr><td><br></td></tr>";
                    }

                    name = dsPatient.Tables[0].Rows[Xh]["TestName"].ToString();
                    unit = dsPatient.Tables[0].Rows[Xh]["TestUnit"].ToString();
                    range = dsPatient.Tables[0].Rows[Xh]["TestRange"].ToString().Trim();
                    if (range != "")
                        range = "(" + range + ")";
                    status = dsPatient.Tables[0].Rows[Xh]["TestStatus"].ToString();
                    value = dsPatient.Tables[0].Rows[Xh]["TestValue"].ToString();
                    value = value.Replace("\\.br\\", "<br>");

                    if (status == "1")
                        fontColor = fontAbNormal;
                    else
                        fontColor = fontNormal;

                    pathologyResult += "<tr><td>" + name + " <font color='" + fontColor + "'/>" + value + "</font> " + unit + " " + range + "</td></tr>";
                    previousSection = dsPatient.Tables[0].Rows[Xh]["SectionID"].ToString();
                }
                pathologyResult += "</table>";
                pathologyResultTable.InnerHtml = pathologyResult;
            }
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Patient Data Form", "Loading Patient Pathology - LoadPatientData function", err.ToString());
        }
    }
    #endregion

    #region protected void linkBtnRefDoctorSave_OnClick(object sender, EventArgs e)
    protected void linkBtnRefDoctorSave_OnClick(object sender, EventArgs e)
    {
        SqlCommand cmdSave = new SqlCommand();
        String strScript = String.Empty;

        gClass.MakeStoreProcedureName(ref cmdSave, "sp_RefDoctors_SaveData", false);
        cmdSave.Parameters.Add("@RefDrID_Old", SqlDbType.VarChar, 10).Value = "";
        cmdSave.Parameters.Add("@RefDrID", SqlDbType.VarChar, 10).Value = txtPatientRefDrID.Value;
        cmdSave.Parameters.Add("@Surname", SqlDbType.VarChar, 50).Value = txtPatientRefSurname.Value;
        cmdSave.Parameters.Add("@FirstName", SqlDbType.VarChar, 50).Value = txtPatientRefFirstname.Value;
        cmdSave.Parameters.Add("@Title", SqlDbType.VarChar, 15).Value = txtPatientRefTitle.Value;
        cmdSave.Parameters.Add("@UseFirst", SqlDbType.Bit).Value = txtPatientRefUseFirst.Value;
        cmdSave.Parameters.Add("@Address1", SqlDbType.VarChar, 50).Value = txtPatientRefAddress1.Value;
        cmdSave.Parameters.Add("@Address2", SqlDbType.VarChar, 50).Value = txtPatientRefAddress2.Value;
        cmdSave.Parameters.Add("@Suburb", SqlDbType.VarChar, 50).Value = txtPatientRefSuburb.Value;
        cmdSave.Parameters.Add("@PostalCode", SqlDbType.VarChar, 10).Value = txtPatientRefPostalCode.Value;
        cmdSave.Parameters.Add("@State", SqlDbType.VarChar, 10).Value = txtPatientRefState.Value;
        cmdSave.Parameters.Add("@Phone", SqlDbType.VarChar, 20).Value = txtPatientRefPhone.Value;
        cmdSave.Parameters.Add("@Fax", SqlDbType.VarChar, 20).Value = txtPatientRefFax.Value;
        cmdSave.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
        cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);

        try
        {
            gClass.ExecuteDMLCommand(cmdSave);

            strScript = "javascript:SetEvents();addRefDrCombo();initialize();";
            ScriptManager.RegisterStartupScript(linkBtnSave, linkBtnSave.GetType(), Guid.NewGuid().ToString(), strScript, true);
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host, Context.Request.Cookies["Logon_UserName"].Value, "Global Web Service", "RefferingDoctor_SaveProc function", err.ToString());
        }

        cmdSave.Dispose();
        Response.End();
    }
    #endregion
    
    #region protected void btnDeletePatient_onserverclick(object sender, EventArgs e)
    /*
     * this function is to delete patient data
     */
    protected void btnDeletePatient_onserverclick(object sender, EventArgs e)
    {
        if (txtHDelete.Value == "1")
        {
            PatientData_DeleteProc();
        }
    }
    #endregion

    #region private void SavePatientDataEMR()
    private void SavePatientDataEMR()
    {
        string tempAnswer = "";
        DateTime deceasedDate = DateTime.MinValue;

        if (txtPatientID.Value.Equals("0"))
        {
            IsDoneSaveFlag = -1;
            return;
        }
        Int32 pageNo = Convert.ToInt32(txtHPageNo.Value);
        Boolean savePage = true;
        SqlCommand cmdSave = new SqlCommand();
        String currPage = "";

        cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSave.Parameters.Add("@PatientId", SqlDbType.Int).Value = Convert.ToInt32(txtPatientID.Value);
        cmdSave.Parameters.Add("@PageNo", SqlDbType.Int).Value = pageNo;

        if (pageNo == 1 || pageNo == 3)
        {
            cmdSave.Parameters.Add("@Complaint", SqlDbType.VarChar, 5).Value = cmbComplaint.SelectedValue;
        }

        if (pageNo == 1)
        {
            currPage = System.Configuration.ConfigurationManager.AppSettings["DemographicPage"].ToString();

            gClass.MakeStoreProcedureName(ref cmdSave, "sp_PatientEMR_cmdInsertDetails", true);

            tempAnswer = "";
            if (rbDemographicFamilyStructureM.Checked)
                tempAnswer = rbDemographicFamilyStructureM.Value;
            else if (rbDemographicFamilyStructureS.Checked)
                tempAnswer = rbDemographicFamilyStructureS.Value;
            else if (rbDemographicFamilyStructureD.Checked)
                tempAnswer = rbDemographicFamilyStructureD.Value;
            else if (rbDemographicFamilyStructureP.Checked)
                tempAnswer = rbDemographicFamilyStructureP.Value;
            else if (rbDemographicFamilyStructureSp.Checked)
                tempAnswer = rbDemographicFamilyStructureSp.Value;

            cmdSave.Parameters.Add("@Details_FamilyStructure", SqlDbType.VarChar, 50).Value = tempAnswer;
            cmdSave.Parameters.Add("@Details_Occupation", SqlDbType.VarChar, 100).Value = txtOccupation.Text.Trim();
            cmdSave.Parameters.Add("@Details_LiveAtHome", SqlDbType.VarChar, 3).Value = txtLiveHome.Text.Trim();
            cmdSave.Parameters.Add("@Details_SpouseName", SqlDbType.VarChar, 50).Value = txtPartnerName.Text.Trim();
            cmdSave.Parameters.Add("@Details_Children", SqlDbType.VarChar, 50).Value = txtDetailChildren.Value.Trim();
            cmdSave.Parameters.Add("@Details_NextOfKinName", SqlDbType.VarChar, 50).Value = txtNOKName.Text.Trim();
            cmdSave.Parameters.Add("@Details_NextOfKinRelation", SqlDbType.VarChar, 50).Value = txtNOKRelationship.Text.Trim();
            cmdSave.Parameters.Add("@Details_NextOfKinAddress", SqlDbType.VarChar, 200).Value = txtNOKAddress.Value.Trim();
            cmdSave.Parameters.Add("@Details_NextOfKinHomePhone", SqlDbType.VarChar, 30).Value = txtNOKHome.Text.Trim();
            cmdSave.Parameters.Add("@Details_NextOfKinWorkPhone", SqlDbType.VarChar, 30).Value = txtNOKWork.Text.Trim();
            cmdSave.Parameters.Add("@Details_NextOfKinMobile", SqlDbType.VarChar, 30).Value = txtNOKMobile.Text.Trim();
            cmdSave.Parameters.Add("@Details_AddContact1Name", SqlDbType.VarChar, 50).Value = txtAddC1Name.Text.Trim();
            cmdSave.Parameters.Add("@Details_AddContact1Relation", SqlDbType.VarChar, 50).Value = txtAddC1Relationship.Text.Trim();
            cmdSave.Parameters.Add("@Details_AddContact1Address", SqlDbType.VarChar, 200).Value = txtAddC1Address.Value.Trim();
            cmdSave.Parameters.Add("@Details_AddContact1HomePhone", SqlDbType.VarChar, 30).Value = txtAddC1Home.Text.Trim();
            cmdSave.Parameters.Add("@Details_AddContact1Mobile", SqlDbType.VarChar, 30).Value = txtAddC1Mob.Text.Trim();
            cmdSave.Parameters.Add("@Details_AddContact2Name", SqlDbType.VarChar, 50).Value = txtAddC2Name.Text.Trim();
            cmdSave.Parameters.Add("@Details_AddContact2Relation", SqlDbType.VarChar, 50).Value = txtAddC2Relationship.Text.Trim();
            cmdSave.Parameters.Add("@Details_AddContact2Address", SqlDbType.VarChar, 200).Value = txtAddC2Address.Value.Trim();
            cmdSave.Parameters.Add("@Details_AddContact2HomePhone", SqlDbType.VarChar, 30).Value = txtAddC2Home.Text.Trim();
            cmdSave.Parameters.Add("@Details_AddContact2Mobile", SqlDbType.VarChar, 30).Value = txtAddC2Mob.Text.Trim();
            cmdSave.Parameters.Add("@Details_MedicareNumber", SqlDbType.VarChar, 100).Value = txtMedicareNumber.Text.Trim();

            cmdSave.Parameters.Add("@Details_DeceasedPrimaryProcedure", SqlDbType.Bit).Value = rbDeceasedPrimaryProcedureY.Checked ? 1 : 0;
            cmdSave.Parameters.Add("@Details_DeceasedNote", SqlDbType.VarChar, 2048).Value = "";
            if (rbDeceasedPrimaryProcedureY.Checked)
                cmdSave.Parameters["@Details_DeceasedNote"].Value = txtDeceasedNote.Text.Trim();

            cmdSave.Parameters.Add("@DeceasedDate", SqlDbType.DateTime);
            if (txtDeceasedDate.Text.Trim() == String.Empty)
                cmdSave.Parameters["@DeceasedDate"].Value = DBNull.Value;
            else
            {
                try
                {
                    cmdSave.Parameters["@DeceasedDate"].Value = Convert.ToDateTime(txtDeceasedDate.Text);
                }
                catch { cmdSave.Parameters["@DeceasedDate"].Value = DBNull.Value; }
            }

            //---------------------upload photo--------------------------
            //change naming
            //save it along with save emr
            //display it on load
            //provide change photo href. after clicking it, hide photo, mark it in hidden var (this look will have cancel and browse)
            //if cancel, bring back to prev display, if change, do replace this

            string strFilePath = "";
            string strURIPath = "";
            string strFileName = "";

            cmdSave.Parameters.Add("@Details_PhotoPath", SqlDbType.VarChar, 2048).Value = DBNull.Value;

            try
            {
                if (uploadPhoto.PostedFile != null)
                {
                    if (uploadPhoto.PostedFile.FileName.Trim() != "")
                    {
                        if (tempPhotoStatus.Value == "1")
                        {
                            strFilePath = GetFilePath(uploadPhoto.PostedFile.FileName);

                            if (System.IO.File.Exists(strFilePath))
                                System.IO.File.Delete(strFilePath);

                            uploadPhoto.PostedFile.SaveAs(strFilePath);

                            int intLastIndex = strFilePath.LastIndexOf("\\");
                            if (intLastIndex == 0)
                                intLastIndex = strFilePath.LastIndexOf("/");

                            strFileName = strFilePath.Substring(intLastIndex + 1);

                            strURIPath = "../../Photos/Baseline/" + strFileName;

                            cmdSave.Parameters["@Details_PhotoPath"].Value = strURIPath;
                        }
                    }
                }
                if (strURIPath == "" && tempPhotoDisplay.Value == "none")
                    cmdSave.Parameters["@Details_PhotoPath"].Value = strURIPath;
                //---------------------
            }
            catch(Exception err)
            {
                gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host,
                                    Request.Cookies["Logon_UserName"].Value, "Baseline", "Save Photo", err.ToString());
            }
        }
        else if (pageNo == 2)
        {
            currPage = System.Configuration.ConfigurationManager.AppSettings["PastHistoryPage"].ToString();

            gClass.MakeStoreProcedureName(ref cmdSave, "sp_PatientEMR_cmdInsertBackground", true);
            cmdSave.Parameters.Add("@Background_Diabetes", SqlDbType.VarChar, 512).Value = ListCheckBoxAnswer(new String[] { "chkBackgroundDF", "chkBackgroundDM", "chkBackgroundDS", "chkBackgroundDN", "chkBackgroundDD" });
            cmdSave.Parameters.Add("@Background_HeartDisease", SqlDbType.VarChar, 512).Value = ListCheckBoxAnswer(new String[] { "chkBackgroundHDF", "chkBackgroundHDM", "chkBackgroundHDS", "chkBackgroundHDN", "chkBackgroundHDD" });
            cmdSave.Parameters.Add("@Background_Hypertension", SqlDbType.VarChar, 512).Value = ListCheckBoxAnswer(new String[] { "chkBackgroundHF", "chkBackgroundHM", "chkBackgroundHS", "chkBackgroundHN", "chkBackgroundHD" });
            cmdSave.Parameters.Add("@Background_Gout", SqlDbType.VarChar, 512).Value = ListCheckBoxAnswer(new String[] { "chkBackgroundGF", "chkBackgroundGM", "chkBackgroundGS", "chkBackgroundGN", "chkBackgroundGD" });
            cmdSave.Parameters.Add("@Background_Obesity", SqlDbType.VarChar, 512).Value = ListCheckBoxAnswer(new String[] { "chkBackgroundOF", "chkBackgroundOM", "chkBackgroundOS", "chkBackgroundON", "chkBackgroundOD" });
            cmdSave.Parameters.Add("@Background_Snoring", SqlDbType.VarChar, 512).Value = ListCheckBoxAnswer(new String[] { "chkBackgroundSF", "chkBackgroundSM", "chkBackgroundSS", "chkBackgroundSN", "chkBackgroundSD" });
            cmdSave.Parameters.Add("@Background_Asthma", SqlDbType.VarChar, 512).Value = ListCheckBoxAnswer(new String[] { "chkBackgroundAF", "chkBackgroundAM", "chkBackgroundAS", "chkBackgroundAN", "chkBackgroundAD" });
            cmdSave.Parameters.Add("@Background_HighCholesterol", SqlDbType.VarChar, 512).Value = ListCheckBoxAnswer(new String[] { "chkBackgroundHCF", "chkBackgroundHCM", "chkBackgroundHCS", "chkBackgroundHCN", "chkBackgroundHCD" });
            cmdSave.Parameters.Add("@Background_FamilyHistory", SqlDbType.VarChar, 2048).Value = txtBackgroundFamilyHistory.Text.Trim();
            cmdSave.Parameters.Add("@Background_PastHealth", SqlDbType.VarChar, 2048).Value = txtBackgroundPastHealth.Text.Trim();
            cmdSave.Parameters.Add("@Background_PreviousBariatric", SqlDbType.VarChar).Value = txtDetailBariatric.Value.Trim();
            cmdSave.Parameters.Add("@Background_PreviousNonBariatric", SqlDbType.VarChar).Value = txtDetailNonBariatric.Value.Trim();
        }
        else if (pageNo == 3)
        {
            currPage = System.Configuration.ConfigurationManager.AppSettings["ComplaintPage"].ToString();

            gClass.MakeStoreProcedureName(ref cmdSave, "sp_PatientEMR_cmdInsertComplaint", true);
            cmdSave.Parameters.Add("@ComplaintNotes", SqlDbType.VarChar, 2048).Value = txtComplaint.Text.Trim();
        }
        else if (pageNo == 6)
        {
            currPage = System.Configuration.ConfigurationManager.AppSettings["InvestigationPage"].ToString();

            gClass.MakeStoreProcedureName(ref cmdSave, "sp_PatientEMR_cmdInsertInvestigation", true);
            cmdSave.Parameters.Add("@SpecialInvestigation_SuggestedSS", SqlDbType.VarChar, 150).Value = txtSpecialInvestigationSSS.Value.Trim();
            cmdSave.Parameters.Add("@SpecialInvestigation_ActionSS", SqlDbType.VarChar, 5).Value = rdSpecialInvestigationSST.Checked ? rdSpecialInvestigationSST.Value : (rdSpecialInvestigationSSA.Checked ? rdSpecialInvestigationSSA.Value : (rdSpecialInvestigationSSN.Checked ? rdSpecialInvestigationSSN.Value : rdSpecialInvestigationSSN.Value));
            cmdSave.Parameters.Add("@SpecialInvestigation_SuggestedGE", SqlDbType.VarChar, 150).Value = txtSpecialInvestigationGES.Value.Trim();
            cmdSave.Parameters.Add("@SpecialInvestigation_ActionGE", SqlDbType.VarChar, 5).Value = rdSpecialInvestigationGET.Checked ? rdSpecialInvestigationGET.Value : (rdSpecialInvestigationGEA.Checked ? rdSpecialInvestigationGEA.Value : (rdSpecialInvestigationGEN.Checked ? rdSpecialInvestigationGEN.Value : rdSpecialInvestigationGEN.Value));
            cmdSave.Parameters.Add("@SpecialInvestigation_SuggestedSP", SqlDbType.VarChar, 150).Value = txtSpecialInvestigationSPS.Value.Trim();
            cmdSave.Parameters.Add("@SpecialInvestigation_ActionSP", SqlDbType.VarChar, 5).Value = rdSpecialInvestigationSPT.Checked ? rdSpecialInvestigationSPT.Value : (rdSpecialInvestigationSPA.Checked ? rdSpecialInvestigationSPA.Value : (rdSpecialInvestigationSPN.Checked ? rdSpecialInvestigationSPN.Value : rdSpecialInvestigationSPN.Value));
            cmdSave.Parameters.Add("@SpecialInvestigation_SuggestedNA", SqlDbType.VarChar, 150).Value = txtSpecialInvestigationNAS.Value.Trim();
            cmdSave.Parameters.Add("@SpecialInvestigation_ActionNA", SqlDbType.VarChar, 5).Value = rdSpecialInvestigationNAT.Checked ? rdSpecialInvestigationNAT.Value : (rdSpecialInvestigationNAA.Checked ? rdSpecialInvestigationNAA.Value : (rdSpecialInvestigationNAN.Checked ? rdSpecialInvestigationNAN.Value : rdSpecialInvestigationNAN.Value));
            cmdSave.Parameters.Add("@SpecialInvestigation_SuggestedPS", SqlDbType.VarChar, 150).Value = txtSpecialInvestigationPSS.Value.Trim();
            cmdSave.Parameters.Add("@SpecialInvestigation_ActionPS", SqlDbType.VarChar, 5).Value = rdSpecialInvestigationPST.Checked ? rdSpecialInvestigationPST.Value : (rdSpecialInvestigationPSA.Checked ? rdSpecialInvestigationPSA.Value : (rdSpecialInvestigationPSN.Checked ? rdSpecialInvestigationPSN.Value : rdSpecialInvestigationPSN.Value));
            cmdSave.Parameters.Add("@SpecialInvestigation_SuggestedOA", SqlDbType.VarChar, 150).Value = txtSpecialInvestigationOAS.Value.Trim();
            cmdSave.Parameters.Add("@SpecialInvestigation_ActionOA", SqlDbType.VarChar, 5).Value = rdSpecialInvestigationOAT.Checked ? rdSpecialInvestigationOAT.Value : (rdSpecialInvestigationOAA.Checked ? rdSpecialInvestigationOAA.Value : (rdSpecialInvestigationOAN.Checked ? rdSpecialInvestigationOAN.Value : rdSpecialInvestigationOAN.Value));

            cmdSave.Parameters.Add("@Investigation_RBS", SqlDbType.VarChar, 150).Value = txtInvestigationRBS.Value.Trim();
            cmdSave.Parameters.Add("@Investigation_ActionRBS", SqlDbType.VarChar, 5).Value = rdInvestigationRBST.Checked ? rdInvestigationRBST.Value : (rdInvestigationRBSA.Checked ? rdInvestigationRBSA.Value : (rdInvestigationRBSN.Checked ? rdInvestigationRBSN.Value : rdInvestigationRBSN.Value));
            cmdSave.Parameters.Add("@Investigation_ABS", SqlDbType.VarChar, 150).Value = txtInvestigationABS.Value.Trim();
            cmdSave.Parameters.Add("@Investigation_ActionABS", SqlDbType.VarChar, 5).Value = rdInvestigationABST.Checked ? rdInvestigationABST.Value : (rdInvestigationABSA.Checked ? rdInvestigationABSA.Value : (rdInvestigationABSN.Checked ? rdInvestigationABSN.Value : rdInvestigationABSN.Value));
            cmdSave.Parameters.Add("@Investigation_RFT", SqlDbType.VarChar, 150).Value = txtInvestigationRFT.Value.Trim();
            cmdSave.Parameters.Add("@Investigation_ActionRFT", SqlDbType.VarChar, 5).Value = rdInvestigationRFTT.Checked ? rdInvestigationRFTT.Value : (rdInvestigationRFTA.Checked ? rdInvestigationRFTA.Value : (rdInvestigationRFTN.Checked ? rdInvestigationRFTN.Value : rdInvestigationRFTN.Value));
            cmdSave.Parameters.Add("@Investigation_ABG", SqlDbType.VarChar, 150).Value = txtInvestigationABG.Value.Trim();
            cmdSave.Parameters.Add("@Investigation_ActionABG", SqlDbType.VarChar, 5).Value = rdInvestigationABGT.Checked ? rdInvestigationABGT.Value : (rdInvestigationABGA.Checked ? rdInvestigationABGA.Value : (rdInvestigationABGN.Checked ? rdInvestigationABGN.Value : rdInvestigationABGN.Value));
            cmdSave.Parameters.Add("@Investigation_EET", SqlDbType.VarChar, 150).Value = txtInvestigationEET.Value.Trim();
            cmdSave.Parameters.Add("@Investigation_ActionEET", SqlDbType.VarChar, 5).Value = rdInvestigationEETT.Checked ? rdInvestigationEETT.Value : (rdInvestigationEETA.Checked ? rdInvestigationEETA.Value : (rdInvestigationEETN.Checked ? rdInvestigationEETN.Value : rdInvestigationEETN.Value));
            cmdSave.Parameters.Add("@Investigation_BM", SqlDbType.VarChar, 150).Value = txtInvestigationBM.Value.Trim();
            cmdSave.Parameters.Add("@Investigation_ActionBM", SqlDbType.VarChar, 5).Value = rdInvestigationBMT.Checked ? rdInvestigationBMT.Value : (rdInvestigationBMA.Checked ? rdInvestigationBMA.Value : (rdInvestigationBMN.Checked ? rdInvestigationBMN.Value : rdInvestigationBMN.Value));
            cmdSave.Parameters.Add("@Investigation_EMS", SqlDbType.VarChar, 150).Value = txtInvestigationEMS.Value.Trim();
            cmdSave.Parameters.Add("@Investigation_ActionEMS", SqlDbType.VarChar, 5).Value = rdInvestigationEMST.Checked ? rdInvestigationEMST.Value : (rdInvestigationEMSA.Checked ? rdInvestigationEMSA.Value : (rdInvestigationEMSN.Checked ? rdInvestigationEMSN.Value : rdInvestigationEMSN.Value));
            cmdSave.Parameters.Add("@Investigation_P", SqlDbType.VarChar, 150).Value = txtInvestigationP.Value.Trim();
            cmdSave.Parameters.Add("@Investigation_ActionP", SqlDbType.VarChar, 5).Value = rdInvestigationPT.Checked ? rdInvestigationPT.Value : (rdInvestigationPA.Checked ? rdInvestigationPA.Value : (rdInvestigationPN.Checked ? rdInvestigationPN.Value : rdInvestigationPN.Value));
            cmdSave.Parameters.Add("@Investigation_EKG", SqlDbType.VarChar, 150).Value = txtInvestigationEKG.Value.Trim();
            cmdSave.Parameters.Add("@Investigation_ActionEKG", SqlDbType.VarChar, 5).Value = rdInvestigationEKGT.Checked ? rdInvestigationEKGT.Value : (rdInvestigationEKGA.Checked ? rdInvestigationEKGA.Value : (rdInvestigationEKGN.Checked ? rdInvestigationEKGN.Value : rdInvestigationEKGN.Value));
            cmdSave.Parameters.Add("@Investigation_CX", SqlDbType.VarChar, 150).Value = txtInvestigationCX.Value.Trim();
            cmdSave.Parameters.Add("@Investigation_ActionCX", SqlDbType.VarChar, 5).Value = rdInvestigationCXT.Checked ? rdInvestigationCXT.Value : (rdInvestigationCXA.Checked ? rdInvestigationCXA.Value : (rdInvestigationCXN.Checked ? rdInvestigationCXN.Value : rdInvestigationCXN.Value));

            cmdSave.Parameters.Add("@Referrals_PA", SqlDbType.VarChar, 512).Value = chkReferralsPA.Checked ? txtReferralsPA.Value.Trim() : null;
            cmdSave.Parameters.Add("@Referrals_C", SqlDbType.VarChar, 512).Value = chkReferralsC.Checked ? txtReferralsC.Value.Trim() : null;
            cmdSave.Parameters.Add("@Referrals_RP", SqlDbType.VarChar, 512).Value = chkReferralsRP.Checked ? txtReferralsRP.Value.Trim() : null;
            cmdSave.Parameters.Add("@Referrals_E", SqlDbType.VarChar, 512).Value = chkReferralsE.Checked ? txtReferralsE.Value.Trim() : null;
            cmdSave.Parameters.Add("@Referrals_AP", SqlDbType.VarChar, 512).Value = chkReferralsAP.Checked ? txtReferralsAP.Value.Trim() : null;
            cmdSave.Parameters.Add("@Referrals_GM", SqlDbType.VarChar, 512).Value = chkReferralsGM.Checked ? txtReferralsGM.Value.Trim() : null;
            cmdSave.Parameters.Add("@Referrals_O", SqlDbType.VarChar, 512).Value = chkReferralsO.Checked ? txtReferralsO.Value.Trim() : null;
        }
        else if (pageNo == 7)
        {
            currPage = System.Configuration.ConfigurationManager.AppSettings["ReviewPage"].ToString();

            gClass.MakeStoreProcedureName(ref cmdSave, "sp_PatientEMR_cmdInsertReview", true);
            cmdSave.Parameters.Add("@ReviewSystem_PFSH", SqlDbType.VarChar, 512).Value = "";
            cmdSave.Parameters.Add("@ReviewSystem_General", SqlDbType.VarChar, 512).Value = "";
            cmdSave.Parameters.Add("@ReviewSystem_Gastro", SqlDbType.VarChar, 512).Value = "";
            cmdSave.Parameters.Add("@ReviewSystem_Cardio", SqlDbType.VarChar, 512).Value = "";
            cmdSave.Parameters.Add("@ReviewSystem_Resp", SqlDbType.VarChar, 512).Value = "";
            cmdSave.Parameters.Add("@ReviewSystem_Musculo", SqlDbType.VarChar, 512).Value = "";
            cmdSave.Parameters.Add("@ReviewSystem_Extr", SqlDbType.VarChar, 512).Value = "";
            cmdSave.Parameters.Add("@ReviewSystem_Genito", SqlDbType.VarChar, 512).Value = "";
            cmdSave.Parameters.Add("@ReviewSystem_Skin", SqlDbType.VarChar, 512).Value = "";
            cmdSave.Parameters.Add("@ReviewSystem_Neuro", SqlDbType.VarChar, 512).Value = "";
            cmdSave.Parameters.Add("@ReviewSystem_Psych", SqlDbType.VarChar, 512).Value = "";
            cmdSave.Parameters.Add("@ReviewSystem_Endo", SqlDbType.VarChar, 512).Value = "";
            cmdSave.Parameters.Add("@ReviewSystem_Hema", SqlDbType.VarChar, 512).Value = "";
            cmdSave.Parameters.Add("@ReviewSystem_ENT", SqlDbType.VarChar, 512).Value = "";
            cmdSave.Parameters.Add("@ReviewSystem_Eyes", SqlDbType.VarChar, 512).Value = "";
            cmdSave.Parameters.Add("@ReviewSystem_Meds", SqlDbType.VarChar, 512).Value = "";
        }
        else if (pageNo == 8)
        {
            currPage = System.Configuration.ConfigurationManager.AppSettings["LabPage"].ToString();

            gClass.MakeStoreProcedureName(ref cmdSave, "sp_PatientEMR_cmdInsertLabs", true);
            cmdSave.Parameters.Add("@Lab_Notes", SqlDbType.VarChar, 2048).Value = txtLabNotes.Text.Trim();
        }
        else if (pageNo == 9)
        {
            currPage = System.Configuration.ConfigurationManager.AppSettings["MedicationPage"].ToString();

            gClass.MakeStoreProcedureName(ref cmdSave, "sp_PatientEMR_cmdInsertMedication", true);
            cmdSave.Parameters.Add("@Medication", SqlDbType.VarChar, 2048).Value = txtDetailMedications.Value.Trim();
        }
        else if (pageNo == 10)
        {
            currPage = System.Configuration.ConfigurationManager.AppSettings["AllergyPage"].ToString();

            gClass.MakeStoreProcedureName(ref cmdSave, "sp_PatientEMR_cmdInsertAllergy", true);
            cmdSave.Parameters.Add("@Allergy_HaveAllergy", SqlDbType.Bit).Value = rdAllergyY.Checked ? 1 : 0;
            cmdSave.Parameters.Add("@Allergy_ListAllergy", SqlDbType.VarChar, 512).Value = txtAllergy.Text.Trim();
            cmdSave.Parameters.Add("@Allergy_HaveMedication", SqlDbType.Bit).Value = rdMedicationY.Checked ? 1 : 0;
            cmdSave.Parameters.Add("@Allergy_ListMedication", SqlDbType.VarChar, 512).Value = txtMedication.Text.Trim();
            cmdSave.Parameters.Add("@Allergy_Latex", SqlDbType.Bit).Value = rdLatexY.Checked ? 1 : 0;
            cmdSave.Parameters.Add("@Allergy_Anesthetic", SqlDbType.VarChar, 512).Value = txtAnesthetic.Text.Trim();
            cmdSave.Parameters.Add("@Allergy_ExcessBleed", SqlDbType.Bit).Value = rdBleedExcessY.Checked ? 1 : 0;
            cmdSave.Parameters.Add("@Allergy_AnestheticRisk", SqlDbType.VarChar, 512).Value = txtAnestheticRisk.Text.Trim();
            cmdSave.Parameters.Add("@Allergy_HaveDrink", SqlDbType.VarChar, 20).Value = rdAlergyAlcoholDoYouDrink_Regularly.Checked ? rdAlergyAlcoholDoYouDrink_Regularly.Value : (rdAlergyAlcoholDoYouDrink_Rarely.Checked ? rdAlergyAlcoholDoYouDrink_Rarely.Value : (rdAlergyAlcoholDoYouDrink_Never.Checked ? rdAlergyAlcoholDoYouDrink_Never.Value : rdAlergyAlcoholDoYouDrink_Never.Value));
            cmdSave.Parameters.Add("@Allergy_DrinkPerDay", SqlDbType.VarChar, 20).Value = txtAlergyDrinkPerDay.Text.Trim();
            cmdSave.Parameters.Add("@Allergy_DrinkPerWeek", SqlDbType.VarChar, 20).Value = txtAlergyDrinkPerWeek.Text.Trim();
            cmdSave.Parameters.Add("@Allergy_DrinkBeer", SqlDbType.Bit).Value = chkAlergyDrinkTypeBeer.Checked;
            cmdSave.Parameters.Add("@Allergy_DrinkWine", SqlDbType.Bit).Value = chkAlergyDrinkTypeWine.Checked;
            cmdSave.Parameters.Add("@Allergy_DrinkSpirits", SqlDbType.Bit).Value = chkAlergyDrinkTypeSpirits.Checked;
            cmdSave.Parameters.Add("@Allergy_Smoke", SqlDbType.VarChar, 20).Value = rdAlergySmokeDoYouSmoke_Yes.Checked ? rdAlergySmokeDoYouSmoke_Yes.Value : (rdAlergySmokeDoYouSmoke_No.Checked ? rdAlergySmokeDoYouSmoke_No.Value : (rdAlergySmokeDoYouSmoke_Never.Checked ? rdAlergySmokeDoYouSmoke_Never.Value : rdAlergySmokeDoYouSmoke_Never.Value));
            cmdSave.Parameters.Add("@Allergy_SmokePerDay", SqlDbType.VarChar, 20).Value = txtAlergySmokePerDay.Text.Trim();
            cmdSave.Parameters.Add("@Allergy_HaveSmoke", SqlDbType.Bit).Value = rdAlergySmokeHaveYouSmoke_Yes.Checked ? 1 : 0;
            cmdSave.Parameters.Add("@Allergy_HaveSmokePerDay", SqlDbType.VarChar, 20).Value = txtAlergySmokePastPerDay.Text.Trim();
            cmdSave.Parameters.Add("@Allergy_HaveSmokeYears", SqlDbType.VarChar, 20).Value = txtAlergySmokeHowmanyYears.Text.Trim();
            cmdSave.Parameters.Add("@Allergy_HaveSmokeStop", SqlDbType.VarChar, 20).Value = txtAlergySmokeWhenStop.Text.Trim();
            cmdSave.Parameters.Add("@Allergy_DoDrugs", SqlDbType.Bit).Value = rdAllergyDrugsDoYouY.Checked ? 1 : 0;
            cmdSave.Parameters.Add("@Allergy_DoDrugs_Notes", SqlDbType.VarChar, 512).Value = txtAlergyDrugsNotes.Text.Trim();
        }
        else if (pageNo == 11)
        {
            currPage = System.Configuration.ConfigurationManager.AppSettings["WeightHistoryPage"].ToString();

            gClass.MakeStoreProcedureName(ref cmdSave, "sp_PatientEMR_cmdInsertWeightHistory", true); cmdSave.Parameters.Add("@WeightHistory_GainWeight", SqlDbType.Decimal);
            cmdSave.Parameters["@WeightHistory_GainWeight"].Value = Decimal.TryParse(txtHWeightHistoryGainWeight.Value, out decTemp) ? decTemp : 0;
            cmdSave.Parameters.Add("@WeightHistory_GainYears", SqlDbType.VarChar, 5).Value = txtWeightHistoryGainYears.Text.Trim();

            tempAnswer = "";
            if (rdWeightHistoryHistoryIndicateBirth_Below.Checked)
                tempAnswer = rdWeightHistoryHistoryIndicateBirth_Below.Value;
            else if (rdWeightHistoryHistoryIndicateBirth_Average.Checked)
                tempAnswer = rdWeightHistoryHistoryIndicateBirth_Average.Value;
            else if (rdWeightHistoryHistoryIndicateBirth_Above.Checked)
                tempAnswer = rdWeightHistoryHistoryIndicateBirth_Above.Value;
            else if (rdWeightHistoryHistoryIndicateBirth_Very.Checked)
                tempAnswer = rdWeightHistoryHistoryIndicateBirth_Very.Value;
            cmdSave.Parameters.Add("@WeightHistory_WeightBirth", SqlDbType.VarChar, 20).Value = tempAnswer;

            tempAnswer = "";
            if (rdWeightHistoryHistoryIndicateStart_Below.Checked)
                tempAnswer = rdWeightHistoryHistoryIndicateStart_Below.Value;
            else if (rdWeightHistoryHistoryIndicateStart_Average.Checked)
                tempAnswer = rdWeightHistoryHistoryIndicateStart_Average.Value;
            else if (rdWeightHistoryHistoryIndicateStart_Above.Checked)
                tempAnswer = rdWeightHistoryHistoryIndicateStart_Above.Value;
            else if (rdWeightHistoryHistoryIndicateStart_Very.Checked)
                tempAnswer = rdWeightHistoryHistoryIndicateStart_Very.Value;
            cmdSave.Parameters.Add("@WeightHistory_WeightStartSchool", SqlDbType.VarChar, 20).Value = tempAnswer;

            tempAnswer = "";
            if (rdWeightHistoryHistoryIndicateHighStart_Below.Checked)
                tempAnswer = rdWeightHistoryHistoryIndicateHighStart_Below.Value;
            else if (rdWeightHistoryHistoryIndicateHighStart_Average.Checked)
                tempAnswer = rdWeightHistoryHistoryIndicateHighStart_Average.Value;
            else if (rdWeightHistoryHistoryIndicateHighStart_Above.Checked)
                tempAnswer = rdWeightHistoryHistoryIndicateHighStart_Above.Value;
            else if (rdWeightHistoryHistoryIndicateHighStart_Very.Checked)
                tempAnswer = rdWeightHistoryHistoryIndicateHighStart_Very.Value;
            cmdSave.Parameters.Add("@WeightHistory_WeightStartHighSchool", SqlDbType.VarChar, 20).Value = tempAnswer;

            tempAnswer = "";
            if (rdWeightHistoryHistoryIndicateHighEnd_Below.Checked)
                tempAnswer = rdWeightHistoryHistoryIndicateHighEnd_Below.Value;
            else if (rdWeightHistoryHistoryIndicateHighEnd_Average.Checked)
                tempAnswer = rdWeightHistoryHistoryIndicateHighEnd_Average.Value;
            else if (rdWeightHistoryHistoryIndicateHighEnd_Above.Checked)
                tempAnswer = rdWeightHistoryHistoryIndicateHighEnd_Above.Value;
            else if (rdWeightHistoryHistoryIndicateHighEnd_Very.Checked)
                tempAnswer = rdWeightHistoryHistoryIndicateHighEnd_Very.Value;
            cmdSave.Parameters.Add("@WeightHistory_WeightEndHighSchool", SqlDbType.VarChar, 20).Value = tempAnswer;

            tempAnswer = "";
            if (rdWeightHistoryHistoryIndicateWork_Below.Checked)
                tempAnswer = rdWeightHistoryHistoryIndicateWork_Below.Value;
            else if (rdWeightHistoryHistoryIndicateWork_Average.Checked)
                tempAnswer = rdWeightHistoryHistoryIndicateWork_Average.Value;
            else if (rdWeightHistoryHistoryIndicateWork_Above.Checked)
                tempAnswer = rdWeightHistoryHistoryIndicateWork_Above.Value;
            else if (rdWeightHistoryHistoryIndicateWork_Very.Checked)
                tempAnswer = rdWeightHistoryHistoryIndicateWork_Very.Value;
            cmdSave.Parameters.Add("@WeightHistory_WeightWork", SqlDbType.VarChar, 20).Value = tempAnswer;

            tempAnswer = "";
            if (rdWeightHistoryHistoryIndicateMarriage_Below.Checked)
                tempAnswer = rdWeightHistoryHistoryIndicateMarriage_Below.Value;
            else if (rdWeightHistoryHistoryIndicateMarriage_Average.Checked)
                tempAnswer = rdWeightHistoryHistoryIndicateMarriage_Average.Value;
            else if (rdWeightHistoryHistoryIndicateMarriage_Above.Checked)
                tempAnswer = rdWeightHistoryHistoryIndicateMarriage_Above.Value;
            else if (rdWeightHistoryHistoryIndicateMarriage_Very.Checked)
                tempAnswer = rdWeightHistoryHistoryIndicateMarriage_Very.Value;
            cmdSave.Parameters.Add("@WeightHistory_WeightMarriage", SqlDbType.VarChar, 20).Value = tempAnswer;

            cmdSave.Parameters.Add("@WeightHistory_LoseYears", SqlDbType.VarChar, 20).Value = txtWeightHistoryLossTryLose.Text.Trim();
            cmdSave.Parameters.Add("@WeightHistory_TryMethod", SqlDbType.VarChar, 512).Value = ListCheckBoxAnswer(new String[] { "chkWeightHistoryLossDiet", "chkWeightHistoryLossExercise" });
            cmdSave.Parameters.Add("@WeightHistory_GroupList", SqlDbType.VarChar, 512).Value = ListCheckBoxAnswer(new String[] { "chkWeightHistoryLossGrpJG", "chkWeightHistoryLossGrpWW", "chkWeightHistoryLossGrpGM", "chkWeightHistoryLossGrpLE", "chkWeightHistoryLossGrpNS", "chkWeightHistoryLossGrpTC", "chkWeightHistoryLossGrpHL", "chkWeightHistoryLossGrpGB", "chkWeightHistoryLossGrpSS", "chkWeightHistoryLossGrpAD", "chkWeightHistoryLossGrpGD", "chkWeightHistoryLossGrpLW", "chkWeightHistoryLossGrpMF", "chkWeightHistoryLossGrpML", "chkWeightHistoryLossGrpOF", "chkWeightHistoryLossGrpP", "chkWeightHistoryLossGrpSF", "chkWeightHistoryLossGrpSB", "chkWeightHistoryLossGrpT" });
            cmdSave.Parameters.Add("@WeightHistory_GroupOther", SqlDbType.VarChar, 512).Value = txtWeightHistoryLossGrpOther.Text.Trim();
            cmdSave.Parameters.Add("@WeightHistory_PillList", SqlDbType.VarChar, 512).Value = ListCheckBoxAnswer(new String[] { "chkWeightHistoryLossDietD", "chkWeightHistoryLossDietT", "chkWeightHistoryLossDietA", "chkWeightHistoryLossDietX", "chkWeightHistoryLossDietR", "chkWeightHistoryLossDietP", "chkWeightHistoryLossDietPf", "chkWeightHistoryLossDietAm", "chkWeightHistoryLossDietM", "chkWeightHistoryLossDietRe" });
            cmdSave.Parameters.Add("@WeightHistory_GroupDuration", SqlDbType.VarChar, 20).Value = txtWeightHistoryGroupDuration.Text.Trim();
            cmdSave.Parameters.Add("@WeightHistory_GroupLose", SqlDbType.VarChar, 20).Value = txtWeightHistoryGroupLose.Text.Trim();
            
            cmdSave.Parameters.Add("@WeightHistory_PillOther", SqlDbType.VarChar, 512).Value = txtWeightHistoryLossDietOther.Text.Trim();
            cmdSave.Parameters.Add("@WeightHistory_AdviceList", SqlDbType.VarChar, 512).Value = ListCheckBoxAnswer(new String[] { "chkWeightHistoryLossPALD", "chkWeightHistoryLossPAD", "chkWeightHistoryLossPAN", "chkWeightHistoryLossPAH", "chkWeightHistoryLossPAP", "chkWeightHistoryLossPAA" });
            cmdSave.Parameters.Add("@WeightHistory_AdviceOther", SqlDbType.VarChar, 512).Value = txtWeightHistoryLossPAOther.Text.Trim();
            cmdSave.Parameters.Add("@WeightHistory_DietList", SqlDbType.VarChar, 512).Value = ListCheckBoxAnswer(new String[] { "chkWeightHistoryLossLCMD", "chkWeightHistoryLossLCOT" });
            cmdSave.Parameters.Add("@WeightHistory_Other", SqlDbType.VarChar, 512).Value = ListCheckBoxAnswer(new String[] { "chkWeightHistoryLossOIT", "chkWeightHistoryLossOHR", "chkWeightHistoryLossOWL" });
            cmdSave.Parameters.Add("@WeightHistory_TreatmentList", SqlDbType.VarChar, 512).Value = ListCheckBoxAnswer(new String[] { "chkWeightHistoryLossSSS", "chkWeightHistoryLossSFG", "chkWeightHistoryLossSAG", "chkWeightHistoryLossSB", "chkWeightHistoryLossSA", "chkWeightHistoryLossSL" });
            cmdSave.Parameters.Add("@WeightHistory_CosmeticList", SqlDbType.VarChar, 512).Value = txtWeightHistoryLossCosmetic.Text.Trim();
        }
        else if (pageNo == 13)
        {
            currPage = System.Configuration.ConfigurationManager.AppSettings["ExamPage"].ToString();

            gClass.MakeStoreProcedureName(ref cmdSave, "sp_PatientEMR_cmdInsertExam", true);
            cmdSave.Parameters.Add("@Exam_Notes", SqlDbType.VarChar, 2048).Value = txtExamNotes.Text.Trim();
            cmdSave.Parameters.Add("@Exam_GAO", SqlDbType.VarChar, 512).Value = chkExamGAO.Checked ? txtExamGAO.Value.Trim() : null;
            cmdSave.Parameters.Add("@Exam_HNG", SqlDbType.VarChar, 512).Value = chkExamHNG.Checked ? txtExamHNG.Value.Trim() : null;
            cmdSave.Parameters.Add("@Exam_HNH", SqlDbType.VarChar, 512).Value = chkExamHNH.Checked ? txtExamHNH.Value.Trim() : null;
            cmdSave.Parameters.Add("@Exam_HNM", SqlDbType.VarChar, 512).Value = chkExamHNM.Checked ? txtExamHNM.Value.Trim() : null;
            cmdSave.Parameters.Add("@Exam_HNN", SqlDbType.VarChar, 512).Value = chkExamHNN.Checked ? txtExamHNN.Value.Trim() : null;
            cmdSave.Parameters.Add("@Exam_CH", SqlDbType.VarChar, 512).Value = chkExamCH.Checked ? txtExamCH.Value.Trim() : null;
            cmdSave.Parameters.Add("@Exam_CP", SqlDbType.VarChar, 512).Value = chkExamCP.Checked ? txtExamCP.Value.Trim() : null;
            cmdSave.Parameters.Add("@Exam_AA", SqlDbType.VarChar, 512).Value = chkExamAA.Checked ? txtExamAA.Value.Trim() : null;
            cmdSave.Parameters.Add("@Exam_LSR", SqlDbType.VarChar, 512).Value = chkExamLSR.Checked ? txtExamLSR.Value.Trim() : null;
            cmdSave.Parameters.Add("@Exam_LSB", SqlDbType.VarChar, 512).Value = chkExamLSB.Checked ? txtExamLSB.Value.Trim() : null;
            cmdSave.Parameters.Add("@Exam_LSA", SqlDbType.VarChar, 512).Value = chkExamLSA.Checked ? txtExamLSA.Value.Trim() : null;
            cmdSave.Parameters.Add("@Exam_MSO", SqlDbType.VarChar, 512).Value = chkExamMSO.Checked ? txtExamMSO.Value.Trim() : null;
            cmdSave.Parameters.Add("@Exam_MF", SqlDbType.VarChar, 512).Value = chkExamMF.Checked ? txtExamMF.Value.Trim() : null;
        }
        else if (pageNo == 16)
        {
            currPage = System.Configuration.ConfigurationManager.AppSettings["ManagementPage"].ToString();

            gClass.MakeStoreProcedureName(ref cmdSave, "sp_PatientEMR_cmdInsertManagement", true);
            cmdSave.Parameters.Add("@Management", SqlDbType.VarChar, 2048).Value = txtManagement.Text.Trim();
        }
        else if (pageNo == 18)
        {
            currPage = System.Configuration.ConfigurationManager.AppSettings["RegistryPage"].ToString();

            gClass.MakeStoreProcedureName(ref cmdSave, "sp_PatientEMR_cmdInsertRegistry", true); 
            cmdSave.Parameters.Add("@Registry_Antihypertensives", SqlDbType.VarChar, 50).Value = txtAntihypertensives.Value.Trim();
            cmdSave.Parameters.Add("@Registry_Insulin", SqlDbType.VarChar, 50).Value = rdRegistryMedication.Value;

            tempAnswer = "";
            if (rdRegistrySleepApnea_Yes.Checked)
                tempAnswer = rdRegistrySleepApnea_Yes.Value;
            else if (rdRegistrySleepApnea_No.Checked)
                tempAnswer = rdRegistrySleepApnea_No.Value;
            else if (rdRegistrySleepApnea_Notdocumented.Checked)
                tempAnswer = rdRegistrySleepApnea_Notdocumented.Value;
            cmdSave.Parameters.Add("@Registry_SleepApnea", SqlDbType.VarChar, 50).Value = tempAnswer;

            tempAnswer = "";
            if (rdRegistryGerd_Yes.Checked)
                tempAnswer = rdRegistryGerd_Yes.Value;
            else if (rdRegistryGerd_No.Checked)
                tempAnswer = rdRegistryGerd_No.Value;
            else if (rdRegistryGerd_Notdocumented.Checked)
                tempAnswer = rdRegistryGerd_Notdocumented.Value;
            cmdSave.Parameters.Add("@Registry_Gerd", SqlDbType.VarChar, 50).Value = tempAnswer;

            tempAnswer = "";
            if (rdRegistryHyperlipidemia_Yes.Checked)
                tempAnswer = rdRegistryHyperlipidemia_Yes.Value;
            else if (rdRegistryHyperlipidemia_No.Checked)
                tempAnswer = rdRegistryHyperlipidemia_No.Value;
            else if (rdRegistryHyperlipidemia_Notdocumented.Checked)
                tempAnswer = rdRegistryHyperlipidemia_Notdocumented.Value;
            cmdSave.Parameters.Add("@Registry_Hyperlipidemia", SqlDbType.VarChar, 50).Value = tempAnswer;

            tempAnswer = "";
            if (rdRegistryHypertension_Yes.Checked)
                tempAnswer = rdRegistryHypertension_Yes.Value;
            else if (rdRegistryHypertension_No.Checked)
                tempAnswer = rdRegistryHypertension_No.Value;
            else if (rdRegistryHypertension_Notdocumented.Checked)
                tempAnswer = rdRegistryHypertension_Notdocumented.Value;
            cmdSave.Parameters.Add("@Registry_Hypertension", SqlDbType.VarChar, 50).Value = tempAnswer;

            tempAnswer = "";
            if (rdRegistryDiabetes_Yes.Checked)
                tempAnswer = rdRegistryDiabetes_Yes.Value;
            else if (rdRegistryDiabetes_No.Checked)
                tempAnswer = rdRegistryDiabetes_No.Value;
            else if (rdRegistryDiabetes_Notdocumented.Checked)
                tempAnswer = rdRegistryDiabetes_Notdocumented.Value;
            cmdSave.Parameters.Add("@Registry_Diabetes", SqlDbType.VarChar, 50).Value = tempAnswer;
        }
        else
        {
            savePage = false;
        }
        if (savePage == true)
        {
            try
            {
                //insert log
                //before updating data
                SaveActionDetailLog();

                gClass.ExecuteDMLCommand(cmdSave);
                Response.SetCookie(new HttpCookie("PatientID", txtPatientID.Value));
                gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value, Request.Cookies["Logon_UserName"].Value, Request.Url.Host,
                            "Baseline Form", 2, "Modify Height/Weight/Notes data", "PatientCode", txtPatientID.Value);

                gClass.SaveActionLog(gClass.OrganizationCode,
                                     Request.Cookies["UserPracticeCode"].Value,
                                     Request.Url.Host,
                                     System.Configuration.ConfigurationManager.AppSettings["MedicalRecordPage"].ToString(),
                                     System.Configuration.ConfigurationManager.AppSettings["LogUpdate"].ToString(),
                                     "Save " + currPage + " Data",
                                     txtPatientID.Value,
                                     "");

                IsDoneSaveFlag = 1;
            }
            catch (Exception err)
            {
                gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host,
                                        Request.Cookies["Logon_UserName"].Value, "Baseline", "Data saving height/weight/notes", err.ToString());
                IsDoneSaveFlag = 0;
            }
        }
        cmdSave.Dispose();
    }
    #endregion 

    #region private void SavePatientDataBold()
    private void SavePatientDataBold()
    {
        if (txtPatientID.Value.Equals("0"))
        {
            IsDoneSaveFlag = -1;
            return;
        }
        SqlCommand cmdSave = new SqlCommand();
        String strNameId = String.Empty;
        Decimal decTemp = 0;
        Int32 int32Temp = 0;
        System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo(Request.Cookies["CultureInfo"].Value, true);

        gClass.MakeStoreProcedureName(ref cmdSave, "sp_PatientData_SaveBoldData", true);
        cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Int32.TryParse(gClass.OrganizationCode, out int32Temp) ? int32Temp : 0;
        cmdSave.Parameters.Add("@PatientId", SqlDbType.Int).Value = Int32.TryParse(txtPatientID.Value, out int32Temp) ? int32Temp : 0;
        cmdSave.Parameters.Add("@ChartNumber", SqlDbType.VarChar, 50).Value = txtChartNumber.Text.Trim();
        cmdSave.Parameters.Add("@BoldChartNumber", SqlDbType.VarChar, 50).Value = txtBoldChartNumber.Text.Trim();
        cmdSave.Parameters.Add("@SocialSecurityNumber", SqlDbType.VarChar, 15).Value = txtSSN.Text.Trim();
        cmdSave.Parameters.Add("@EmploymentStatus", SqlDbType.VarChar, 6).Value = cmbEmployment.SelectedValue;
        cmdSave.Parameters.Add("@EmployerName", SqlDbType.VarChar, 100).Value = txtEmployer.Text.Trim();
        cmdSave.Parameters.Add("@InsuranceNumber", SqlDbType.VarChar, 100).Value = txtInsuranceNumber.Text.Trim();
        cmdSave.Parameters.Add("@HasConsentedToSRC", SqlDbType.Bit).Value = rdSend2SRC_Yes.Checked;
        cmdSave.Parameters.Add("@HasInsurance", SqlDbType.Bit).Value = (txtInsurance.Text.Trim().Length > 0);
        cmdSave.Parameters.Add("@IsCoverProcedure", SqlDbType.SmallInt).Value = rbHealthInsurance_Yes.Checked ? 1 : (rbHealthInsurance_No.Checked ? 0 : (rbHealthInsurance_Unknown.Checked ? -1 : -1));
        cmdSave.Parameters.Add("@SecondaryInsurance", SqlDbType.VarChar, 50).Value = txtSecondaryCoverage.Text.Trim();
        cmdSave.Parameters.Add("@TertiaryInsurance", SqlDbType.VarChar, 50).Value = txtTertiaryCoverage.Text.Trim();
        cmdSave.Parameters.Add("@IsSelfPay", SqlDbType.Bit).Value = chkSelfPay.Checked;
        cmdSave.Parameters.Add("@IsCharity", SqlDbType.Bit).Value = chkCharity.Checked;
        cmdSave.Parameters.Add("@IsMedicare", SqlDbType.Bit).Value = chkMedicare.Checked;
        cmdSave.Parameters.Add("@IsMedicaid", SqlDbType.Bit).Value = chkMedicaid.Checked;
        cmdSave.Parameters.Add("@IsPrivateInsurance", SqlDbType.Bit).Value = chkPrivateIns.Checked;
        cmdSave.Parameters.Add("@IsGovernmentInsurance", SqlDbType.Bit).Value = chkGovernmentIns.Checked;
        cmdSave.Parameters.Add("@PreOperativeWeightLoss", SqlDbType.Decimal).Value = Decimal.TryParse(txtPreOpWeightLoss.Text, out decTemp) ? decTemp : 0;
        cmdSave.Parameters.Add("@DietryWeightLoss", SqlDbType.VarChar, 8).Value = cmbDietaryWeightLoss.SelectedValue;
        cmdSave.Parameters.Add("@DurationObesity", SqlDbType.Bit).Value = chkDurationObesity.Checked;
        cmdSave.Parameters.Add("@SmokingCessation", SqlDbType.Bit).Value = chkSmokingCessation.Checked;
        cmdSave.Parameters.Add("@MentalHealthClearance", SqlDbType.Bit).Value = chkMentalHealthClearance.Checked;
        cmdSave.Parameters.Add("@IQTesting", SqlDbType.Bit).Value = chkIntelligenceTesting.Checked;
        cmdSave.Parameters.Add("@PreCertification_Other", SqlDbType.VarChar, 100).Value = txtPreCert_Other.Text.Trim();
        cmdSave.Parameters.Add("@PNBS_Procedure", SqlDbType.VarChar, 100).Value = txtPrevNonBariatricSurgery_Selected.Value;

        try
        {
            //insert log
            //before updating data
            SaveActionDetailLog();

            gClass.ExecuteDMLCommand(cmdSave);
            gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value, Request.Cookies["Logon_UserName"].Value, Request.Url.Host, "Baseline Form - BOLD data", 2, "Save (Add new/update) bold data",
                    "PatientCode", txtPatientID.Value);

            gClass.SaveActionLog(gClass.OrganizationCode,
                                 Request.Cookies["UserPracticeCode"].Value,
                                 Request.Url.Host,
                                 System.Configuration.ConfigurationManager.AppSettings["MedicalRecordPage"].ToString(),
                                 System.Configuration.ConfigurationManager.AppSettings["LogUpdate"].ToString(),
                                 "Save " + System.Configuration.ConfigurationManager.AppSettings["PatientPage"].ToString() + " BOLD Data",
                                 txtPatientID.Value,
                                 "");

            //only save if BOLD mode is on
            if (Request.Cookies["SubmitData"].Value.IndexOf("submitbold") >= 0)
            {
                SubmitBOLDPatientData();
            }
            
            IsDoneSaveFlag = 1;
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Baseline Form - BOLD data", "Save (Add new/update) bold data", err.ToString());
            IsDoneSaveFlag = 0;
        }
    }
    #endregion

    #region private void ListCheckBoxAnswer(string[] choiceArray)
    private string ListCheckBoxAnswer(String[] choiceArray)
    {
        string tempChoice = "";
        HtmlInputCheckBox tempControl = new HtmlInputCheckBox();
        Boolean test = false;

        for (int Xh = 0; Xh < choiceArray.Length; Xh++)
        {
            tempControl = (HtmlInputCheckBox)FindControlRecursive(this.Page, choiceArray[Xh]);
            if (tempControl.Checked)
                tempChoice += choiceArray[Xh] + "-";
        }

        tempChoice = tempChoice.TrimEnd('-');
        return tempChoice;
    }        
    #endregion 

    #region private void SavePatientDataComorbidity()
    private void SavePatientDataComorbidity()
    {
        if (txtPatientID.Value.Equals("0"))
        {
            IsDoneSaveFlag = -1;
            return;
        }

        string tempRenalInsuf = "";
        string tempRenalFail;
        string tempSteroid;
        string tempTherapeutic;
                
        string tempRenalInsufNotes;
        string tempRenalFailNotes;
        string tempSteroidNotes;
        string tempTherapeuticNotes;

        if (Request.Cookies["SubmitData"].Value.IndexOf("acs") >= 0)
        {
            tempRenalInsuf = cmbRenalInsuffACS.SelectedValue;
            tempRenalFail = cmbRenalFailACS.SelectedValue;
            tempSteroid = cmbSteroidsACS.SelectedValue;
            tempTherapeutic = cmbAnticogulationACS.SelectedValue;

            tempRenalInsufNotes = txtcmbRenalInsuffACS.Text.Trim();
            tempRenalFailNotes = txtcmbRenalFailACS.Text.Trim();
            tempSteroidNotes = txtcmbSteroidsACS.Text.Trim();
            tempTherapeuticNotes = txtcmbAnticogulationACS.Text.Trim();
        }
        else
        {
            tempRenalInsuf = cmbRenalInsuff.SelectedValue;
            tempRenalFail = cmbRenalFail.SelectedValue;
            tempSteroid = cmbSteroid.SelectedValue;
            tempTherapeutic = cmbTherapeutic.SelectedValue;

            tempRenalInsufNotes = txtcmbRenalInsuff.Text.Trim();
            tempRenalFailNotes = txtcmbRenalFail.Text.Trim();
            tempSteroidNotes = txtcmbSteroid.Text.Trim();
            tempTherapeuticNotes = txtcmbTherapeutic.Text.Trim();
        }

        //comorbidity
        SqlCommand cmdSave = new SqlCommand();
        gClass.MakeStoreProcedureName(ref cmdSave, "sp_ConsultFu1_BoldComorbidity_SaveData", true);
        cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSave.Parameters.Add("@PatientId", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
        cmdSave.Parameters.Add("@ConsultId", SqlDbType.Int).Value = 0;

        cmdSave.Parameters.Add("@CVS_Hypertension", SqlDbType.NVarChar, 10).Value = cmbHypertension.SelectedValue;
        cmdSave.Parameters.Add("@CVS_Congestive", SqlDbType.NVarChar, 10).Value = cmbCongestive.SelectedValue;
        cmdSave.Parameters.Add("@CVS_Ischemic", SqlDbType.NVarChar, 10).Value = cmbIschemic.SelectedValue;
        cmdSave.Parameters.Add("@CVS_Angina", SqlDbType.NVarChar, 10).Value = cmbAngina.SelectedValue;
        cmdSave.Parameters.Add("@CVS_Peripheral", SqlDbType.NVarChar).Value = cmbPeripheral.SelectedValue;
        cmdSave.Parameters.Add("@CVS_Lower", SqlDbType.NVarChar, 10).Value = cmbLower.SelectedValue;
        cmdSave.Parameters.Add("@CVS_DVT", SqlDbType.NVarChar, 10).Value = cmbDVT.SelectedValue;
        cmdSave.Parameters.Add("@MET_Glucose", SqlDbType.NVarChar, 10).Value = cmbGlucose.SelectedValue;
        cmdSave.Parameters.Add("@MET_Lipids", SqlDbType.NVarChar, 10).Value = cmbLipids.SelectedValue;
        cmdSave.Parameters.Add("@MET_Gout", SqlDbType.NVarChar, 10).Value = cmbGout.SelectedValue;
        cmdSave.Parameters.Add("@PUL_Obstructive", SqlDbType.NVarChar, 10).Value = cmbObstructive.SelectedValue;
        cmdSave.Parameters.Add("@PUL_Obesity", SqlDbType.NVarChar, 10).Value = cmbObesity.SelectedValue;
        cmdSave.Parameters.Add("@PUL_PulHypertension", SqlDbType.NVarChar, 10).Value = cmbPulmonary.SelectedValue;
        cmdSave.Parameters.Add("@PUL_Asthma", SqlDbType.NVarChar, 10).Value = cmbAsthma.SelectedValue;
        cmdSave.Parameters.Add("@GAS_Gerd", SqlDbType.NVarChar, 10).Value = cmbGred.SelectedValue;
        cmdSave.Parameters.Add("@GAS_Cholelithiasis", SqlDbType.NVarChar, 10).Value = cmbCholelithiasis.SelectedValue;
        cmdSave.Parameters.Add("@GAS_Liver", SqlDbType.NVarChar, 10).Value = cmbLiver.SelectedValue;
        cmdSave.Parameters.Add("@MUS_BackPain", SqlDbType.NVarChar, 10).Value = cmbBackPain.SelectedValue;
        cmdSave.Parameters.Add("@MUS_Musculoskeletal", SqlDbType.NVarChar, 10).Value = cmbMusculoskeletal.SelectedValue;
        cmdSave.Parameters.Add("@MUS_Fibromyalgia", SqlDbType.NVarChar, 10).Value = cmbFibro.SelectedValue;
        cmdSave.Parameters.Add("@REPRD_Polycystic", SqlDbType.NVarChar, 10).Value = cmbPolycystic.SelectedValue;
        cmdSave.Parameters.Add("@REPRD_Menstrual", SqlDbType.NVarChar, 10).Value = cmbMenstrual.SelectedValue;
        cmdSave.Parameters.Add("@PSY_Impairment", SqlDbType.NVarChar, 10).Value = cmbPsychosocial.SelectedValue;
        cmdSave.Parameters.Add("@PSY_Depression", SqlDbType.NVarChar, 10).Value = cmbDepression.SelectedValue;
        cmdSave.Parameters.Add("@PSY_MentalHealth", SqlDbType.NVarChar, 10).Value = cmbConfirmed.SelectedValue;
        cmdSave.Parameters.Add("@PSY_Alcohol", SqlDbType.NVarChar, 10).Value = cmbAlcohol.SelectedValue;
        cmdSave.Parameters.Add("@PSY_Tobacoo", SqlDbType.NVarChar, 10).Value = cmbTobacco.SelectedValue;
        cmdSave.Parameters.Add("@PSY_Abuse", SqlDbType.NVarChar, 10).Value = cmbAbuse.SelectedValue;
        cmdSave.Parameters.Add("@GEN_Stress", SqlDbType.NVarChar, 10).Value = cmbStressUrinary.SelectedValue;
        cmdSave.Parameters.Add("@GEN_Cerebri", SqlDbType.NVarChar, 10).Value = cmbCerebri.SelectedValue;
        cmdSave.Parameters.Add("@GEN_Hernia", SqlDbType.NVarChar, 10).Value = cmbHernia.SelectedValue;
        cmdSave.Parameters.Add("@GEN_Functional", SqlDbType.NVarChar, 10).Value = cmbFunctional.SelectedValue;
        cmdSave.Parameters.Add("@GEN_Skin", SqlDbType.NVarChar, 10).Value = cmbSkin.SelectedValue;
        cmdSave.Parameters.Add("@VitaminList", SqlDbType.NVarChar, 50).Value = "";
        cmdSave.Parameters.Add("@Vitamin_Description", SqlDbType.NVarChar, 255).Value = "";
        
        cmdSave.Parameters.Add("@GEN_PrevPCISurgery", SqlDbType.NVarChar, 10).Value = cmbPrevPCISurgery.SelectedValue;


        cmdSave.Parameters.Add("@ACS_Smoke", SqlDbType.NVarChar, 10).Value = cmbSmokerACS.SelectedValue;
        cmdSave.Parameters.Add("@ACS_Oxy", SqlDbType.NVarChar, 10).Value = cmbOxygenACS.SelectedValue;
        cmdSave.Parameters.Add("@ACS_Embo", SqlDbType.NVarChar, 10).Value = cmbEmbolismACS.SelectedValue;
        cmdSave.Parameters.Add("@ACS_Copd", SqlDbType.NVarChar, 10).Value = cmbCopdACS.SelectedValue;
        cmdSave.Parameters.Add("@ACS_Cpap", SqlDbType.NVarChar, 10).Value = cmbCpapACS.SelectedValue;
        cmdSave.Parameters.Add("@ACS_Gerd", SqlDbType.NVarChar, 10).Value = cmbGerdACS.SelectedValue;
        cmdSave.Parameters.Add("@ACS_Gal", SqlDbType.NVarChar, 10).Value = cmbGallstoneACS.SelectedValue;
        cmdSave.Parameters.Add("@ACS_Muscd", SqlDbType.NVarChar, 10).Value = cmbMusculoDiseaseACS.SelectedValue;
        cmdSave.Parameters.Add("@ACS_Pain", SqlDbType.NVarChar, 10).Value = cmbActivityLimitedACS.SelectedValue;
        cmdSave.Parameters.Add("@ACS_Meds", SqlDbType.NVarChar, 10).Value = cmbDailyMedACS.SelectedValue;
        cmdSave.Parameters.Add("@ACS_Surg", SqlDbType.NVarChar, 10).Value = cmbSurgicalACS.SelectedValue;
        cmdSave.Parameters.Add("@ACS_Mob", SqlDbType.NVarChar, 10).Value = cmbMobilityACS.SelectedValue;
        cmdSave.Parameters.Add("@ACS_Uri", SqlDbType.NVarChar, 10).Value = cmbUrinaryACS.SelectedValue;
        cmdSave.Parameters.Add("@ACS_Myo", SqlDbType.NVarChar, 10).Value = cmbMyocardinalACS.SelectedValue;
        cmdSave.Parameters.Add("@ACS_Pci", SqlDbType.NVarChar, 10).Value = cmbPrevPCIACS.SelectedValue;
        cmdSave.Parameters.Add("@ACS_Csurg", SqlDbType.NVarChar, 10).Value = cmbPrevCardiacACS.SelectedValue;
        cmdSave.Parameters.Add("@ACS_Lipid", SqlDbType.NVarChar, 10).Value = cmbHyperlipidemiaACS.SelectedValue;
        cmdSave.Parameters.Add("@ACS_Hyper", SqlDbType.NVarChar, 10).Value = cmbHypertensionACS.SelectedValue;
        cmdSave.Parameters.Add("@ACS_Dvt", SqlDbType.NVarChar, 10).Value = cmbDVTACS.SelectedValue;
        cmdSave.Parameters.Add("@ACS_Venous", SqlDbType.NVarChar, 10).Value = cmbVenousACS.SelectedValue;
        cmdSave.Parameters.Add("@ACS_Health", SqlDbType.NVarChar, 10).Value = cmbHealthStatusACS.SelectedValue;
        cmdSave.Parameters.Add("@ACS_Diab", SqlDbType.NVarChar, 10).Value = cmbDiabetesACS.SelectedValue;
        cmdSave.Parameters.Add("@ACS_Obese", SqlDbType.NVarChar, 10).Value = cmbObesityACS.SelectedValue;
        cmdSave.Parameters.Add("@ACS_Sho", SqlDbType.NVarChar, 10).Value = cmbShoACS.SelectedValue;
        cmdSave.Parameters.Add("@ACS_Fat", SqlDbType.NVarChar, 10).Value = cmbFatACS.SelectedValue;
        
        cmdSave.Parameters.Add("@GEN_RenalInsuff", SqlDbType.NVarChar, 10).Value = tempRenalInsuf;
        cmdSave.Parameters.Add("@GEN_RenalFail", SqlDbType.NVarChar, 10).Value = tempRenalFail;
        cmdSave.Parameters.Add("@GEN_Steroid", SqlDbType.NVarChar, 10).Value = tempSteroid;
        cmdSave.Parameters.Add("@GEN_Therapeutic", SqlDbType.NVarChar, 10).Value = tempTherapeutic;


        cmdSave.Parameters.Add("@AUS_EndDiab", SqlDbType.VarChar).Value = txtDiabDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_EndThy", SqlDbType.VarChar).Value = txtThyDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_EndOtherName", SqlDbType.VarChar).Value = txtEndOtherNameDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_EndOtherDesc", SqlDbType.VarChar).Value = txtEndOtherDescDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_PulAsthma", SqlDbType.VarChar).Value = txtAsthmaDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_PulApnea", SqlDbType.VarChar).Value = txtApneaDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_PulEmb", SqlDbType.VarChar).Value = txtEmbDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_PulOtherName", SqlDbType.VarChar).Value = txtPulOtherNameDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_PulOtherDesc", SqlDbType.VarChar).Value = txtPulOtherDescDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_GasRef", SqlDbType.VarChar).Value = txtRefDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_GasUlc", SqlDbType.VarChar).Value = txtUlcDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_GasGall", SqlDbType.VarChar).Value = txtGallDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_GasHep", SqlDbType.VarChar).Value = txtHepDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_GasOtherName", SqlDbType.VarChar).Value = txtGasOtherNameDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_GasOtherDesc", SqlDbType.VarChar).Value = txtGasOtherDescDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_CvsIsc", SqlDbType.VarChar).Value = txtIscDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_CvsBlood", SqlDbType.VarChar).Value = txtBloodDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_CvsCol", SqlDbType.VarChar).Value = txtColDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_CvsDVT", SqlDbType.VarChar).Value = txtDVTDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_CvsVen", SqlDbType.VarChar).Value = txtVenDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_CvsAnti", SqlDbType.VarChar).Value = txtAntiDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_CvsOtherName", SqlDbType.VarChar).Value = txtCvsOtherNameDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_CvsOtherDesc", SqlDbType.VarChar).Value = txtCvsOtherDescDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_PsyDep", SqlDbType.VarChar).Value = txtDepDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_PsyAnx", SqlDbType.VarChar).Value = txtAnxDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_PsyPhob", SqlDbType.VarChar).Value = txtPhobDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_PsyEat", SqlDbType.VarChar).Value = txtEatDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_PsyHead", SqlDbType.VarChar).Value = txtHeadDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_PsyStroke", SqlDbType.VarChar).Value = txtStrokeDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_PsyOtherName", SqlDbType.VarChar).Value = txtPsyOtherNameDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_PsyOtherDesc", SqlDbType.VarChar).Value = txtPsyOtherDescDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_MuscBack", SqlDbType.VarChar).Value = txtBackDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_MuscHip", SqlDbType.VarChar).Value = txtHipDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_MuscKnee", SqlDbType.VarChar).Value = txtKneeDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_MuscFeet", SqlDbType.VarChar).Value = txtFeetDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_MuscFibr", SqlDbType.VarChar).Value = txtFibrDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_MuscOtherName", SqlDbType.VarChar).Value = txtMuscOtherNameDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_MuscOtherDesc", SqlDbType.VarChar).Value = txtMuscOtherDescDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_GenInf", SqlDbType.VarChar).Value = txtInfDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_GenRen", SqlDbType.VarChar).Value = txtRenDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_GenUri", SqlDbType.VarChar).Value = txtUriDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_OtherPso", SqlDbType.VarChar).Value = txtPsoDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_OtherSkin", SqlDbType.VarChar).Value = txtSkinDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_OtherCancer", SqlDbType.VarChar).Value = txtCancerDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_OtherAnemia", SqlDbType.VarChar).Value = txtAnemiaDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_OtherOtherName", SqlDbType.VarChar).Value = txtOtherOtherNameDef.Text.Trim();
        cmdSave.Parameters.Add("@AUS_OtherOtherDesc", SqlDbType.VarChar).Value = txtOtherOtherDescDef.Text.Trim();

        //comorbidity notes
        SqlCommand cmdSaveNotes = new SqlCommand();
        gClass.MakeStoreProcedureName(ref cmdSaveNotes, "sp_ConsultFu1_BoldComorbidityNotes_SaveData", true);
        cmdSaveNotes.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSaveNotes.Parameters.Add("@PatientId", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
        cmdSaveNotes.Parameters.Add("@ConsultId", SqlDbType.Int).Value = 0;

        cmdSaveNotes.Parameters.Add("@CVS_Hypertension", SqlDbType.NVarChar, 1024).Value = txtcmbHypertension.Text.Trim();
        cmdSaveNotes.Parameters.Add("@CVS_Congestive", SqlDbType.NVarChar, 1024).Value = txtcmbCongestive.Text.Trim();
        cmdSaveNotes.Parameters.Add("@CVS_Ischemic", SqlDbType.NVarChar, 1024).Value = txtcmbIschemic.Text.Trim();
        cmdSaveNotes.Parameters.Add("@CVS_Angina", SqlDbType.NVarChar, 1024).Value = txtcmbAngina.Text.Trim();
        cmdSaveNotes.Parameters.Add("@CVS_Peripheral", SqlDbType.NVarChar, 1024).Value = txtcmbPeripheral.Text.Trim();
        cmdSaveNotes.Parameters.Add("@CVS_Lower", SqlDbType.NVarChar, 1024).Value = txtcmbLower.Text.Trim();
        cmdSaveNotes.Parameters.Add("@CVS_DVT", SqlDbType.NVarChar, 1024).Value = txtcmbDVT.Text.Trim();
        cmdSaveNotes.Parameters.Add("@MET_Glucose", SqlDbType.NVarChar, 1024).Value = txtcmbGlucose.Text.Trim();
        cmdSaveNotes.Parameters.Add("@MET_Lipids", SqlDbType.NVarChar, 1024).Value = txtcmbLipids.Text.Trim();
        cmdSaveNotes.Parameters.Add("@MET_Gout", SqlDbType.NVarChar, 1024).Value = txtcmbGout.Text.Trim();
        cmdSaveNotes.Parameters.Add("@PUL_Obstructive", SqlDbType.NVarChar, 1024).Value = txtcmbObstructive.Text.Trim();
        cmdSaveNotes.Parameters.Add("@PUL_Obesity", SqlDbType.NVarChar, 1024).Value = txtcmbObesity.Text.Trim();
        cmdSaveNotes.Parameters.Add("@PUL_PulHypertension", SqlDbType.NVarChar, 1024).Value = txtcmbPulmonary.Text.Trim();
        cmdSaveNotes.Parameters.Add("@PUL_Asthma", SqlDbType.NVarChar, 1024).Value = txtcmbAsthma.Text.Trim();
        cmdSaveNotes.Parameters.Add("@GAS_Gerd", SqlDbType.NVarChar, 1024).Value = txtcmbGred.Text.Trim();
        cmdSaveNotes.Parameters.Add("@GAS_Cholelithiasis", SqlDbType.NVarChar, 1024).Value = txtcmbCholelithiasis.Text.Trim();
        cmdSaveNotes.Parameters.Add("@GAS_Liver", SqlDbType.NVarChar, 1024).Value = txtcmbLiver.Text.Trim();
        cmdSaveNotes.Parameters.Add("@MUS_BackPain", SqlDbType.NVarChar, 1024).Value = txtcmbBackPain.Text.Trim();
        cmdSaveNotes.Parameters.Add("@MUS_Musculoskeletal", SqlDbType.NVarChar, 1024).Value = txtcmbMusculoskeletal.Text.Trim();
        cmdSaveNotes.Parameters.Add("@MUS_Fibromyalgia", SqlDbType.NVarChar, 1024).Value = txtcmbFibro.Text.Trim();
        cmdSaveNotes.Parameters.Add("@REPRD_Polycystic", SqlDbType.NVarChar, 1024).Value = txtcmbPolycystic.Text.Trim();
        cmdSaveNotes.Parameters.Add("@REPRD_Menstrual", SqlDbType.NVarChar, 1024).Value = txtcmbMenstrual.Text.Trim();
        cmdSaveNotes.Parameters.Add("@PSY_Impairment", SqlDbType.NVarChar, 1024).Value = txtcmbPsychosocial.Text.Trim();
        cmdSaveNotes.Parameters.Add("@PSY_Depression", SqlDbType.NVarChar, 1024).Value = txtcmbDepression.Text.Trim();
        cmdSaveNotes.Parameters.Add("@PSY_MentalHealth", SqlDbType.NVarChar, 1024).Value = txtcmbConfirmed.Text.Trim();
        cmdSaveNotes.Parameters.Add("@PSY_Alcohol", SqlDbType.NVarChar, 1024).Value = txtcmbAlcohol.Text.Trim();
        cmdSaveNotes.Parameters.Add("@PSY_Tobacoo", SqlDbType.NVarChar, 1024).Value = txtcmbTobacco.Text.Trim();
        cmdSaveNotes.Parameters.Add("@PSY_Abuse", SqlDbType.NVarChar, 1024).Value = txtcmbAbuse.Text.Trim();
        cmdSaveNotes.Parameters.Add("@GEN_Stress", SqlDbType.NVarChar, 1024).Value = txtcmbStressUrinary.Text.Trim();
        cmdSaveNotes.Parameters.Add("@GEN_Cerebri", SqlDbType.NVarChar, 1024).Value = txtcmbCerebri.Text.Trim();
        cmdSaveNotes.Parameters.Add("@GEN_Hernia", SqlDbType.NVarChar, 1024).Value = txtcmbHernia.Text.Trim();
        cmdSaveNotes.Parameters.Add("@GEN_Functional", SqlDbType.NVarChar, 10).Value = txtcmbFunctional.Text.Trim();
        cmdSaveNotes.Parameters.Add("@GEN_Skin", SqlDbType.NVarChar, 1024).Value = txtcmbSkin.Text.Trim();
        cmdSaveNotes.Parameters.Add("@Extra_Comorbidity", SqlDbType.NVarChar, 2048).Value = txtDetailComorbidities.Value.Trim();
        cmdSaveNotes.Parameters.Add("@Comorbidity_Review", SqlDbType.Bit).Value = chkComorbidityReview.Checked ? true : false;
        
        cmdSaveNotes.Parameters.Add("@GEN_PrevPCISurgery", SqlDbType.NVarChar, 1024).Value = txtcmbPrevPCISurgery.Text.Trim();


        cmdSaveNotes.Parameters.Add("@ACS_Smoke", SqlDbType.NVarChar, 1024).Value = txtcmbSmokerACS.Text.Trim();
        cmdSaveNotes.Parameters.Add("@ACS_Oxy", SqlDbType.NVarChar, 1024).Value = txtcmbOxygenACS.Text.Trim();
        cmdSaveNotes.Parameters.Add("@ACS_Embo", SqlDbType.NVarChar, 1024).Value = txtcmbEmbolismACS.Text.Trim();
        cmdSaveNotes.Parameters.Add("@ACS_Copd", SqlDbType.NVarChar, 1024).Value = txtcmbCopdACS.Text.Trim();
        cmdSaveNotes.Parameters.Add("@ACS_Cpap", SqlDbType.NVarChar, 1024).Value = txtcmbCpapACS.Text.Trim();
        cmdSaveNotes.Parameters.Add("@ACS_Gerd", SqlDbType.NVarChar, 1024).Value = txtcmbGerdACS.Text.Trim();
        cmdSaveNotes.Parameters.Add("@ACS_Gal", SqlDbType.NVarChar, 1024).Value = txtcmbGallstoneACS.Text.Trim();
        cmdSaveNotes.Parameters.Add("@ACS_Muscd", SqlDbType.NVarChar, 1024).Value = txtcmbMusculoDiseaseACS.Text.Trim();
        cmdSaveNotes.Parameters.Add("@ACS_Pain", SqlDbType.NVarChar, 1024).Value = txtcmbActivityLimitedACS.Text.Trim();
        cmdSaveNotes.Parameters.Add("@ACS_Meds", SqlDbType.NVarChar, 1024).Value = txtcmbDailyMedACS.Text.Trim();
        cmdSaveNotes.Parameters.Add("@ACS_Surg", SqlDbType.NVarChar, 1024).Value = txtcmbSurgicalACS.Text.Trim();
        cmdSaveNotes.Parameters.Add("@ACS_Mob", SqlDbType.NVarChar, 1024).Value = txtcmbMobilityACS.Text.Trim();
        cmdSaveNotes.Parameters.Add("@ACS_Uri", SqlDbType.NVarChar, 1024).Value = txtcmbUrinaryACS.Text.Trim();
        cmdSaveNotes.Parameters.Add("@ACS_Myo", SqlDbType.NVarChar, 1024).Value = txtcmbMyocardinalACS.Text.Trim();
        cmdSaveNotes.Parameters.Add("@ACS_Pci", SqlDbType.NVarChar, 1024).Value = txtcmbPrevPCIACS.Text.Trim();
        cmdSaveNotes.Parameters.Add("@ACS_Csurg", SqlDbType.NVarChar, 1024).Value = txtcmbPrevCardiacACS.Text.Trim();
        cmdSaveNotes.Parameters.Add("@ACS_Lipid", SqlDbType.NVarChar, 1024).Value = txtcmbHyperlipidemiaACS.Text.Trim();
        cmdSaveNotes.Parameters.Add("@ACS_Hyper", SqlDbType.NVarChar, 1024).Value = txtcmbHypertensionACS.Text.Trim();
        cmdSaveNotes.Parameters.Add("@ACS_Dvt", SqlDbType.NVarChar, 1024).Value = txtcmbDVTACS.Text.Trim();
        cmdSaveNotes.Parameters.Add("@ACS_Venous", SqlDbType.NVarChar, 1024).Value = txtcmbVenousACS.Text.Trim();
        cmdSaveNotes.Parameters.Add("@ACS_Health", SqlDbType.NVarChar, 1024).Value = txtcmbHealthStatusACS.Text.Trim();
        cmdSaveNotes.Parameters.Add("@ACS_Diab", SqlDbType.NVarChar, 1024).Value = txtcmbDiabetesACS.Text.Trim();
        cmdSaveNotes.Parameters.Add("@ACS_Obese", SqlDbType.NVarChar, 1024).Value = txtcmbObesityACS.Text.Trim();
        cmdSaveNotes.Parameters.Add("@ACS_General", SqlDbType.NVarChar, 1024).Value = txtcmbNotesACS.Text.Trim();
        cmdSaveNotes.Parameters.Add("@ACS_Sho", SqlDbType.NVarChar, 1024).Value = txtcmbShoACS.Text.Trim();
        cmdSaveNotes.Parameters.Add("@ACS_Fat", SqlDbType.NVarChar, 1024).Value = txtcmbFatACS.Text.Trim();
        
        cmdSaveNotes.Parameters.Add("@GEN_RenalInsuff", SqlDbType.NVarChar, 1024).Value = tempRenalInsufNotes;
        cmdSaveNotes.Parameters.Add("@GEN_RenalFail", SqlDbType.NVarChar, 1024).Value = tempRenalFailNotes;
        cmdSaveNotes.Parameters.Add("@GEN_Steroid", SqlDbType.NVarChar, 1024).Value = tempSteroidNotes;
        cmdSaveNotes.Parameters.Add("@GEN_Therapeutic", SqlDbType.NVarChar, 1024).Value = tempTherapeuticNotes;

        SqlCommand cmdSaveCheck = new SqlCommand();
        if (Request.Cookies["SubmitData"].Value == "")
        {
            //comorbidity check
            gClass.MakeStoreProcedureName(ref cmdSaveCheck, "sp_ConsultFu1_ComorbidityCheck_SaveData", true);
            cmdSaveCheck.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
            cmdSaveCheck.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
        
            cmdSaveCheck.Parameters.Add("@AUS_EndDiab", SqlDbType.Bit).Value = chkDiabDef.Checked;
            cmdSaveCheck.Parameters.Add("@AUS_EndThy", SqlDbType.Bit).Value = chkThyDef.Checked;
            cmdSaveCheck.Parameters.Add("@AUS_EndOther", SqlDbType.Bit).Value = chkEndOtherDef.Checked;

            cmdSaveCheck.Parameters.Add("@AUS_PulAsthma", SqlDbType.Bit).Value = chkAsthmaDef.Checked;
            cmdSaveCheck.Parameters.Add("@AUS_PulApnea", SqlDbType.Bit).Value = chkApneaDef.Checked;
            cmdSaveCheck.Parameters.Add("@AUS_PulEmb", SqlDbType.Bit).Value = chkEmbDef.Checked;
            cmdSaveCheck.Parameters.Add("@AUS_PulOther", SqlDbType.Bit).Value = chkPulOtherDef.Checked;

            cmdSaveCheck.Parameters.Add("@AUS_GasRef", SqlDbType.Bit).Value = chkRefDef.Checked;
            cmdSaveCheck.Parameters.Add("@AUS_GasUlc", SqlDbType.Bit).Value = chkUlcDef.Checked;
            cmdSaveCheck.Parameters.Add("@AUS_GasGall", SqlDbType.Bit).Value = chkGallDef.Checked;
            cmdSaveCheck.Parameters.Add("@AUS_GasHep", SqlDbType.Bit).Value = chkHepDef.Checked;
            cmdSaveCheck.Parameters.Add("@AUS_GasOther", SqlDbType.Bit).Value = chkGasOtherDef.Checked;

            cmdSaveCheck.Parameters.Add("@AUS_CvsIsc", SqlDbType.Bit).Value = chkIscDef.Checked;
            cmdSaveCheck.Parameters.Add("@AUS_CvsBlood", SqlDbType.Bit).Value = chkBloodDef.Checked;
            cmdSaveCheck.Parameters.Add("@AUS_CvsCol", SqlDbType.Bit).Value = chkColDef.Checked;
            cmdSaveCheck.Parameters.Add("@AUS_CvsDVT", SqlDbType.Bit).Value = chkDVTDef.Checked;
            cmdSaveCheck.Parameters.Add("@AUS_CvsVen", SqlDbType.Bit).Value = chkVenDef.Checked;
            cmdSaveCheck.Parameters.Add("@AUS_CvsAnti", SqlDbType.Bit).Value = chkAntiDef.Checked;
            cmdSaveCheck.Parameters.Add("@AUS_CvsOther", SqlDbType.Bit).Value = chkCvsOtherDef.Checked;

            cmdSaveCheck.Parameters.Add("@AUS_PsyDep", SqlDbType.Bit).Value = chkDepDef.Checked;
            cmdSaveCheck.Parameters.Add("@AUS_PsyAnx", SqlDbType.Bit).Value = chkAnxDef.Checked;
            cmdSaveCheck.Parameters.Add("@AUS_PsyPhob", SqlDbType.Bit).Value = chkPhobDef.Checked;
            cmdSaveCheck.Parameters.Add("@AUS_PsyEat", SqlDbType.Bit).Value = chkEatDef.Checked;
            cmdSaveCheck.Parameters.Add("@AUS_PsyHead", SqlDbType.Bit).Value = chkHeadDef.Checked;
            cmdSaveCheck.Parameters.Add("@AUS_PsyStroke", SqlDbType.Bit).Value = chkStrokeDef.Checked;
            cmdSaveCheck.Parameters.Add("@AUS_PsyOther", SqlDbType.Bit).Value = chkPsyOtherDef.Checked;

            cmdSaveCheck.Parameters.Add("@AUS_MuscBack", SqlDbType.Bit).Value = chkBackDef.Checked;
            cmdSaveCheck.Parameters.Add("@AUS_MuscHip", SqlDbType.Bit).Value = chkHipDef.Checked;
            cmdSaveCheck.Parameters.Add("@AUS_MuscKnee", SqlDbType.Bit).Value = chkKneeDef.Checked;
            cmdSaveCheck.Parameters.Add("@AUS_MuscFeet", SqlDbType.Bit).Value = chkFeetDef.Checked;
            cmdSaveCheck.Parameters.Add("@AUS_MuscFibr", SqlDbType.Bit).Value = chkFibrDef.Checked;
            cmdSaveCheck.Parameters.Add("@AUS_MuscOther", SqlDbType.Bit).Value = chkMuscOtherDef.Checked;

            cmdSaveCheck.Parameters.Add("@AUS_GenInf", SqlDbType.Bit).Value = chkInfDef.Checked;
            cmdSaveCheck.Parameters.Add("@AUS_GenRen", SqlDbType.Bit).Value = chkRenDef.Checked;
            cmdSaveCheck.Parameters.Add("@AUS_GenUri", SqlDbType.Bit).Value = chkUriDef.Checked;
            cmdSaveCheck.Parameters.Add("@AUS_OtherPso", SqlDbType.Bit).Value = chkPsoDef.Checked;
            cmdSaveCheck.Parameters.Add("@AUS_OtherSkin", SqlDbType.Bit).Value = chkSkinDef.Checked;
            cmdSaveCheck.Parameters.Add("@AUS_OtherCancer", SqlDbType.Bit).Value = chkCancerDef.Checked;
            cmdSaveCheck.Parameters.Add("@AUS_OtherAnemia", SqlDbType.Bit).Value = chkAnemiaDef.Checked;
            cmdSaveCheck.Parameters.Add("@AUS_OtherOther", SqlDbType.Bit).Value = chkOtherOtherDef.Checked;
        }


        try
        {
            //insert log
            //before updating data
            SaveActionDetailLog();

            gClass.ExecuteDMLCommand(cmdSave);
            gClass.ExecuteDMLCommand(cmdSaveNotes);
            if (Request.Cookies["SubmitData"].Value == "")
            {
                gClass.ExecuteDMLCommand(cmdSaveCheck);
            }

            gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value, Request.Cookies["Logon_UserName"].Value, Request.Url.Host, "Baseline Form - BOLD Comorbidity ", 2,
                    "Save (Add new/update) bold comorbidity data", "PatientCode", txtPatientID.Value);

            gClass.SaveActionLog(gClass.OrganizationCode,
                                 Request.Cookies["UserPracticeCode"].Value,
                                 Request.Url.Host,
                                 System.Configuration.ConfigurationManager.AppSettings["MedicalRecordPage"].ToString(),
                                 System.Configuration.ConfigurationManager.AppSettings["LogUpdate"].ToString(),
                                 "Save " + System.Configuration.ConfigurationManager.AppSettings["ComorbidityPage"].ToString() + " Data",
                                 txtPatientID.Value,
                                 "");

            IsDoneSaveFlag = 1;
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Baseline Form - BOLD Comorbidity ",
                    "Save (Add new/update) bold data", err.ToString());
            IsDoneSaveFlag = 0;
        }
    }
    #endregion

    #region private void SavePatientDataHeightWeight()
    private void SavePatientDataHeightWeight()
    {
        if (txtPatientID.Value.Equals("0"))
        {
            IsDoneSaveFlag = -1;
            return;
        }
        SqlCommand cmdSave = new SqlCommand();
        cmdSave_AddParameters(ref cmdSave);
        cmdSave.Parameters["@OrganizationCode"].Value = Int32.TryParse(gClass.OrganizationCode, out int32Temp) ? int32Temp : 0;
        cmdSave.Parameters["@UserPracticeCode"].Value = Int32.TryParse(Request.Cookies["UserPracticeCode"].Value, out int32Temp) ? int32Temp : 0; 
        cmdSave.Parameters["@PageNo"].Value = 2;
        cmdSave.Parameters["@PatientId"].Value = Int32.TryParse(txtPatientID.Value, out int32Temp) ? int32Temp : 0;

        cmdSave.Parameters["@Height"].Value = Decimal.TryParse(txtHHeight.Value, out decTemp) ? decTemp : 0;
        cmdSave.Parameters["@StartWeight"].Value = Decimal.TryParse(txtHStartWeight.Value, out decTemp) ? decTemp : 0;
        cmdSave.Parameters["@IdealWeight"].Value = Decimal.TryParse(txtHIdealWeight.Value, out decTemp) ? decTemp : 0;
        cmdSave.Parameters["@CurrentWeight"].Value = Decimal.TryParse(txtHCurrentWeight.Value, out decTemp) ? decTemp : 0;
        cmdSave.Parameters["@TargetWeight"].Value = Decimal.TryParse(txtHTargetWeight.Value, out decTemp) ? decTemp : 0;
        cmdSave.Parameters["@losttofollowup"].Value = 0;
        cmdSave.Parameters["@StartBMIWeight"].Value = Decimal.TryParse(txtHStartWeight.Value, out decTemp) ? decTemp : 0;
        cmdSave.Parameters["@BMIHeight"].Value = Decimal.TryParse(txtBMIHeight.Value, out decTemp) ? decTemp : 0;
        cmdSave.Parameters["@BMI"].Value = Decimal.TryParse(txtHBMI.Value, out decTemp) ? decTemp : 0;
        txtBMI.Text = txtHBMI.Value; // IMPORTANT, DO NOT DELETE OR COMMENT OUT THIS STATEMENT
        cmdSave.Parameters["@Notes"].Value = txtMeasurementNotes.Text.Replace("'", "`");


        cmdSave.Parameters["@StartNeck"].Value = Decimal.TryParse(txtHMeasurementNeck.Value, out decTemp) ? decTemp : 0;
        cmdSave.Parameters["@StartWaist"].Value = Decimal.TryParse(txtHMeasurementWaist.Value, out decTemp) ? decTemp : 0;
        cmdSave.Parameters["@StartHip"].Value = Decimal.TryParse(txtHMeasurementHip.Value, out decTemp) ? decTemp : 0;

        cmdSave.Parameters["@StartPR"].Value = Int32.TryParse(txtMeasurementPR.Text, out int32Temp) ? int32Temp : 0;
        cmdSave.Parameters["@StartRR"].Value = Int32.TryParse(txtMeasurementRR.Text, out int32Temp) ? int32Temp : 0;

        cmdSave.Parameters["@StartBP1"].Value = Int32.TryParse(txtMeasurementBPLow.Text, out int32Temp) ? int32Temp : 0;
        cmdSave.Parameters["@StartBP2"].Value = Int32.TryParse(txtMeasurementBPHigh.Text, out int32Temp) ? int32Temp : 0;

        cmdSave.Parameters["@VisitWeeksFlag"].Value = cmbVisitWeeks.SelectedValue;

        if (txtZeroDate.Text.Trim() == String.Empty)
            cmdSave.Parameters["@Zerodate"].Value = DBNull.Value;
        else
        {
            try
            {
                cmdSave.Parameters["@Zerodate"].Value = Convert.ToDateTime(txtZeroDate.Text);
            }
            catch { cmdSave.Parameters["@Zerodate"].Value = DBNull.Value; }
        }

        try
        {
            //insert log
            //before updating data
            SaveActionDetailLog();

            gClass.SavePatientWeightData(cmdSave);
            Response.SetCookie(new HttpCookie("PatientID", txtPatientID.Value));
            gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value, Request.Cookies["Logon_UserName"].Value, Request.Url.Host,
                        "Baseline Form", 2, "Modify Height/Weight/Notes data", "PatientCode", txtPatientID.Value);
            
            gClass.SaveActionLog(gClass.OrganizationCode,
                                 Request.Cookies["UserPracticeCode"].Value,
                                 Request.Url.Host,
                                 System.Configuration.ConfigurationManager.AppSettings["MedicalRecordPage"].ToString(),
                                 System.Configuration.ConfigurationManager.AppSettings["LogUpdate"].ToString(),
                                 "Save " + System.Configuration.ConfigurationManager.AppSettings["PatientWeightData"].ToString(),
                                 txtPatientID.Value,
                                 "");
            gClass.SaveActionLog(gClass.OrganizationCode,
                                 Request.Cookies["UserPracticeCode"].Value,
                                 Request.Url.Host,
                                 System.Configuration.ConfigurationManager.AppSettings["MedicalRecordPage"].ToString(),
                                 System.Configuration.ConfigurationManager.AppSettings["LogUpdate"].ToString(),
                                 "Save " + System.Configuration.ConfigurationManager.AppSettings["MeasurementPage"].ToString(),
                                 txtPatientID.Value,
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
    }
    #endregion 

    #region private void SavePatientDataPreviousSurgery()
    private void SavePatientDataPreviousSurgery()
    {
        if (txtPatientID.Value.Equals("0"))
        {
            IsDoneSaveFlag = -1;
            return;
        }
        
        //BOLD validation
        //only check if BOLD mode is on
        if (Request.Cookies["SubmitData"].Value.IndexOf("submitbold") >= 0)
        {
            if (ValidateBOLDPatientData() == false)
                return;
        }

        SqlCommand cmdSave = new SqlCommand();
        String strNameId = String.Empty;
        Decimal decTemp = 0;
        Int32 int32Temp = 0;
        System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo(Request.Cookies["CultureInfo"].Value, true);

        gClass.MakeStoreProcedureName(ref cmdSave, "sp_PatientData_SaveBoldPreviousSurgery", true);
        cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Int32.TryParse(gClass.OrganizationCode, out int32Temp) ? int32Temp : 0;
        cmdSave.Parameters.Add("@PatientId", SqlDbType.Int).Value = Int32.TryParse(txtPatientID.Value, out int32Temp) ? int32Temp : 0;
        cmdSave.Parameters.Add("@PBS_Procedure", SqlDbType.VarChar, 100).Value = txtPrevBariatricSurgery_Selected.Value;
        cmdSave.Parameters.Add("@PBS_Year", SqlDbType.Int).Value = Int32.TryParse(txtPrevBarSurgery_Year.Text, out int32Temp) ? int32Temp : 0;
        cmdSave.Parameters.Add("@OriginalWeight", SqlDbType.Decimal).Value = Decimal.TryParse(txtOrgWeight.Text, out decTemp) ? decTemp : 0;
        cmdSave.Parameters.Add("@OriginalWeight_Actual", SqlDbType.Bit).Value = rbOrgWeight_Actual.Checked ? true : (rbOrgWeight_Estimated.Checked ? false : false);
        cmdSave.Parameters.Add("@LowestWeightAchieved", SqlDbType.Decimal).Value = Decimal.TryParse(txtLowestWeightAchieved.Text, out decTemp) ? decTemp : 0;
        cmdSave.Parameters.Add("@LowestWeightAchieved_Actual", SqlDbType.Bit).Value = rbLowestWeightAchieved_Actual.Checked ? true : (rbLowestWeightAchieved_Estimated.Checked ? false : false);
        cmdSave.Parameters.Add("@PBS_Event", SqlDbType.VarChar, 100).Value = txtAdverseEvents_Selected.Value;
        cmdSave.Parameters.Add("@PBS_SurgeonID", SqlDbType.Int).Value = Int32.TryParse(cmbSurgeon.SelectedValue, out int32Temp) ? int32Temp : 0;

        try
        {
            //insert log
            //before updating data
            SaveActionDetailLog();

            gClass.ExecuteDMLCommand(cmdSave);
            gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value, Request.Cookies["Logon_UserName"].Value, Request.Url.Host, "Baseline Form - BOLD data", 2, "Save (Add new/update) bold data",
                    "PatientCode", txtPatientID.Value);

            gClass.SaveActionLog(gClass.OrganizationCode,
                                 Request.Cookies["UserPracticeCode"].Value,
                                 Request.Url.Host,
                                 System.Configuration.ConfigurationManager.AppSettings["MedicalRecordPage"].ToString(),
                                 System.Configuration.ConfigurationManager.AppSettings["LogUpdate"].ToString(),
                                 "Save " + System.Configuration.ConfigurationManager.AppSettings["PastHistoryPage"].ToString() + " BOLD Data",
                                 txtPatientID.Value,
                                 "");

            IsDoneSaveFlag = 1;
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Baseline Form - BOLD data", "Save (Add new/update) bold data", err.ToString());
            IsDoneSaveFlag = 0;
        }
        SavePatientDataBold();
        SavePatientDataEMR();
    }
    #endregion

    #region private void SavePatientData()
    private void SavePatientData()
    {
        SqlCommand cmdSave = new SqlCommand();
        SqlCommand cmdSelect = new SqlCommand();
        String strNameId = String.Empty;
        DataSet dsCode = new DataSet();
        string strScript = String.Empty;

        strNameId = (txtSurName.Text.Length == 0) ? String.Empty : ((txtSurName.Text.Length > 3) ? txtSurName.Text.Substring(0, 4) : txtSurName.Text);
        strNameId += (txtFirstName.Text.Length == 0) ? String.Empty : txtFirstName.Text.Substring(0, 1);

        if (strNameId.Equals(String.Empty))
        {
            IsDoneSaveFlag = -1;
            return;
        }

        //BOLD validation
        //only check if submit BOLD mode is on
        if (Request.Cookies["SubmitData"].Value.IndexOf("submitbold") >= 0)
        {
            if(ValidateBOLDPatientData() == false)
                return;
        }


        if (txtPatientID.Value.Equals("0") || (txtPatientID.Value.Trim().Equals(String.Empty)))
        {
            txtPatientID.Value = "0"; // if PatientID is null, we set it by 0
            gClass.MakeStoreProcedureName(ref cmdSave, "sp_PatientData_cmdInsert", true);
            cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Int32.TryParse(gClass.OrganizationCode, out int32Temp) ? int32Temp : 0;
            cmdSave.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Int32.TryParse(Request.Cookies["UserPracticeCode"].Value, out int32Temp) ? int32Temp : 0;
        }
        else
            gClass.MakeStoreProcedureName(ref cmdSave, "sp_PatientData_cmdUpdate", true);

        cmdSave.Parameters.Add("@NameId", SqlDbType.VarChar, 7).Value = strNameId;
        cmdSave.Parameters.Add("@Surname", SqlDbType.VarChar, 40).Value = txtSurName.Text.Replace("'", "`");
        cmdSave.Parameters.Add("@Firstname", SqlDbType.VarChar, 30).Value = txtFirstName.Text.Replace("'", "`");
        cmdSave.Parameters.Add("@Title", SqlDbType.SmallInt).Value = rblTitle.Value;
        cmdSave.Parameters.Add("@Street", SqlDbType.VarChar, 40).Value = txtStreet.Text;
        cmdSave.Parameters.Add("@Suburb", SqlDbType.VarChar, 40).Value = txtCity.Text;
        cmdSave.Parameters.Add("@State", SqlDbType.VarChar, 10).Value = txtState.Text;
        cmdSave.Parameters.Add("@Postcode", SqlDbType.VarChar, 10).Value = txtPostCode.Text;
        cmdSave.Parameters.Add("@HomePhone", SqlDbType.VarChar, 30).Value = txtPhone_H.Text;
        cmdSave.Parameters.Add("@WorkPhone", SqlDbType.VarChar, 30).Value = txtPhone_W.Text;
        cmdSave.Parameters.Add("@Race", SqlDbType.VarChar, 3).Value = cmbRace.SelectedValue;
        cmdSave.Parameters.Add("@Birthdate", SqlDbType.DateTime);
        cmdSave.Parameters.Add("@Consultationdate", SqlDbType.DateTime);

        if (txtBirthDate.Text.Trim() == String.Empty)
            cmdSave.Parameters["@Birthdate"].Value = DBNull.Value;
        else
            try
            {
                if (Convert.ToDateTime(txtBirthDate.Text) < DateTime.Now)
                    cmdSave.Parameters["@Birthdate"].Value = Convert.ToDateTime(txtBirthDate.Text);
                else
                    cmdSave.Parameters["@Birthdate"].Value = DBNull.Value;
            }
            catch { cmdSave.Parameters["@Birthdate"].Value = DBNull.Value; }

        if (txtConsultationDate.Text.Trim() == String.Empty)
            cmdSave.Parameters["@Consultationdate"].Value = DBNull.Value;
        else
        {
            try
            {
                cmdSave.Parameters["@Consultationdate"].Value = Convert.ToDateTime(txtConsultationDate.Text);
            }
            catch { cmdSave.Parameters["@Consultationdate"].Value = DBNull.Value; }
        }

        cmdSave.Parameters.Add("@Sex", SqlDbType.VarChar, 1).Value = rblGender.SelectedValue;
        cmdSave.Parameters.Add("@DoctorId", SqlDbType.Int).Value = (cmbSurgon.SelectedIndex > 0) ? Convert.ToInt32(cmbSurgon.SelectedValue) : 0;
        cmdSave.Parameters.Add("@RefDrId1", SqlDbType.VarChar, 10).Value = txtHReferredBy.Value;
        cmdSave.Parameters.Add("@RefDrId2", SqlDbType.VarChar, 10).Value = txtHOtherDoctors1.Value;
        cmdSave.Parameters.Add("@RefDrId3", SqlDbType.VarChar, 10).Value = txtHOtherDoctors2.Value;
        cmdSave.Parameters.Add("@Patient_MDId", SqlDbType.VarChar, 10).Value = "";
        cmdSave.Parameters.Add("@RefDrDuration1", SqlDbType.VarChar, 3).Value = txtRefDuration1.SelectedValue;
        cmdSave.Parameters.Add("@RefDrDuration2", SqlDbType.VarChar, 3).Value = txtRefDuration2.SelectedValue;
        cmdSave.Parameters.Add("@RefDrDuration3", SqlDbType.VarChar, 3).Value = txtRefDuration3.SelectedValue;
        cmdSave.Parameters.Add("@RefDrStatus1", SqlDbType.VarChar, 3).Value = txtRefStatus1.SelectedValue;
        cmdSave.Parameters.Add("@RefDrStatus2", SqlDbType.VarChar, 3).Value = txtRefStatus2.SelectedValue;
        cmdSave.Parameters.Add("@RefDrStatus3", SqlDbType.VarChar, 3).Value = txtRefStatus3.SelectedValue;
        cmdSave.Parameters.Add("@RefDrDate1", SqlDbType.DateTime);
        cmdSave.Parameters.Add("@RefDrDate2", SqlDbType.DateTime);
        cmdSave.Parameters.Add("@RefDrDate3", SqlDbType.DateTime);
        if (txtRefDate1.Text.Trim() == String.Empty)
            cmdSave.Parameters["@RefDrDate1"].Value = DBNull.Value;
        else
        {
            try
            {
                cmdSave.Parameters["@RefDrDate1"].Value = Convert.ToDateTime(txtRefDate1.Text);
            }
            catch { cmdSave.Parameters["@RefDrDate1"].Value = DBNull.Value; }
        }
        if (txtRefDate2.Text.Trim() == String.Empty)
            cmdSave.Parameters["@RefDrDate2"].Value = DBNull.Value;
        else
        {
            try
            {
                cmdSave.Parameters["@RefDrDate2"].Value = Convert.ToDateTime(txtRefDate2.Text);
            }
            catch { cmdSave.Parameters["@RefDrDate2"].Value = DBNull.Value; }
        }
        if (txtRefDate3.Text.Trim() == String.Empty)
            cmdSave.Parameters["@RefDrDate3"].Value = DBNull.Value;
        else
        {
            try
            {
                cmdSave.Parameters["@RefDrDate3"].Value = Convert.ToDateTime(txtRefDate3.Text);
            }
            catch { cmdSave.Parameters["@RefDrDate3"].Value = DBNull.Value; }
        }

        cmdSave.Parameters.Add("@MobilePhone", SqlDbType.VarChar, 30).Value = txtMobile.Text;
        cmdSave.Parameters.Add("@EmailAddress", SqlDbType.VarChar, 100).Value = txtEmail.Text;
        cmdSave.Parameters.Add("@Insurance", SqlDbType.VarChar, 50).Value = txtInsurance.Text;
        cmdSave.Parameters.Add("@Patient_CustomID", SqlDbType.VarChar, 20).Value = txtPatient_CustomID.Text;
        cmdSave.Parameters.Add("@SocialHistory", SqlDbType.VarChar, 2048).Value = txtSocialHistory.Text.Replace("'", "`");
        cmdSave.Parameters.Add("@MedicalSummary", SqlDbType.VarChar).Value = txtMedicalSummary.Text.Replace("'", "`");

        gClass.AddLogParameters(ref cmdSave, Request.Cookies["Logon_UserName"].Value, Request.Url.Host, (txtPatientID.Value.Equals("0")) ? "insert" : "update");


        //check patient customid
        //if exist for other patient id, give warning, LoadLastPatientCustomIDFromDatabase, stop
        //else, proceed

        String patientCustomID = (txtPatient_CustomID.Text.Equals(String.Empty) || txtPatient_CustomID.Text.Equals("0")) ? txtPatientID.Value : txtPatient_CustomID.Text;
        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_PatientData_CheckPatientCustomID", true);
        cmdSelect.Parameters.Add("@PatientCustomID", SqlDbType.VarChar).Value = patientCustomID;
        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.VarChar).Value = Int32.TryParse(gClass.OrganizationCode, out int32Temp) ? int32Temp : 0; ;
        dsCode = gClass.FetchData(cmdSelect, "tblPatientCustom");

        Boolean saveValid = false;

        if (dsCode.Tables.Count > 0 && (dsCode.Tables[0].Rows.Count > 0))
        {
            String tempPatientID = dsCode.Tables[0].Rows[0]["PatientID"].ToString();
            
            if (tempPatientID == txtPatientID.Value)
                saveValid = true;
            else
                saveValid = false;
        }
        else
            saveValid = true;

        if (saveValid == true)
        {
            try
            {
                if (txtPatientID.Value.Equals("0")) // means new Patient Data, data must be inserted
                {
                    gClass.SavePatientData(1, cmdSave);
                    txtPatientID.Value = gClass.PatientID.ToString();
                    txtPatient_CustomID.Text = patientCustomID;
                    Context.Response.SetCookie(new HttpCookie("PatientID", txtPatientID.Value));
                    gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value, Request.Cookies["Logon_UserName"].Value, Request.Url.Host,
                                            "Baseline Form", 2, "Add Data", "PatientCode", Response.Cookies["PatientID"].Value);


                    gClass.SaveActionLog(gClass.OrganizationCode,
                                         Request.Cookies["UserPracticeCode"].Value,
                                         Request.Url.Host,
                                         System.Configuration.ConfigurationManager.AppSettings["MedicalRecordPage"].ToString(),
                                         System.Configuration.ConfigurationManager.AppSettings["LogCreate"].ToString(),
                                         "Save " + System.Configuration.ConfigurationManager.AppSettings["DemographicPage"].ToString() + " Data",
                                         Response.Cookies["PatientID"].Value,
                                         "");

                    Response.Cookies.Set(new HttpCookie("PatientID", txtPatientID.Value));
                    Response.Redirect("~/Forms/EMR/EMRForm.aspx?PID=" + txtPatientID.Value, false);
                }
                else //data must be Updated
                {
                    //insert log
                    //before updating data
                    SaveActionDetailLog();

                    cmdSave.Parameters.Add("@PatientId", SqlDbType.Int).Value = Convert.ToInt64(txtPatientID.Value);
                    cmdSave.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Context.Request.Cookies["UserPracticeCode"].Value);
                    cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);

                    gClass.SavePatientData(2, cmdSave);

                    Context.Response.SetCookie(new HttpCookie("PatientID", txtPatientID.Value));
                    gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value, Request.Cookies["Logon_UserName"].Value,
                                            Request.Url.Host, "Baseline Form", 2, "Modify Data", "PatientCode", txtPatientID.Value);

                    gClass.SaveActionLog(gClass.OrganizationCode,
                                         Request.Cookies["UserPracticeCode"].Value,
                                         Request.Url.Host,
                                         System.Configuration.ConfigurationManager.AppSettings["MedicalRecordPage"].ToString(),
                                         System.Configuration.ConfigurationManager.AppSettings["LogUpdate"].ToString(),
                                         "Save " + System.Configuration.ConfigurationManager.AppSettings["DemographicPage"].ToString() + " Data",
                                         txtPatientID.Value,
                                         "");
                }
                IsDoneSaveFlag = 1;
            }
            catch (Exception err)
            {
                gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Baseline", "Data saving baseline", err.ToString());
                IsDoneSaveFlag = 0;
            }

            if (txtBoldChartNumber.Text == "")
                txtBoldChartNumber.Text = Convert.ToInt32(gClass.OrganizationCode) + "-" + txtPatientID.Value;

            SavePatientDataBold();
            SavePatientDataEMR();

            ScriptManager.RegisterStartupScript(linkBtnSave, linkBtnSave.GetType(), Guid.NewGuid().ToString(), strScript, true);

            cmdSave.Dispose();
        }
        else
        {
            strScript = "document.getElementById('divErrorMessage').style.display = 'block';";
            strScript += "SetInnerText(document.getElementById('pErrorMessage'), 'ID " + patientCustomID + " has been allocated to other patient. We have load the new patient ID, please try to save it again. If the error still exist, please type a unique custom ID');";
            //strScript += "SetEvents();";
            ScriptManager.RegisterStartupScript(linkBtnSave, linkBtnSave.GetType(), Guid.NewGuid().ToString(), strScript, true);

            LoadLastPatientCustomIDFromDatabase();
        }
    }
    #endregion
    
    #region private void InitialiseFormSetting()
    private void InitialiseFormSetting()
    {
        lblMeasurementWeightUnit.Text = (txtUseImperial.Value == "1") ? "lbs" : "kg";
        lblMeasurementIWeightUnit.Text = (txtUseImperial.Value == "1") ? "lbs" : "kg";
        lblMeasurementEWeightUnit.Text = (txtUseImperial.Value == "1") ? "lbs" : "kg";
        lblMeasurementTWeightUnit.Text = (txtUseImperial.Value == "1") ? "lbs" : "kg";

        lblMeasurementHeightUnit.Text = (txtUseImperial.Value == "1") ? "inches" : "cm";
        lblMeasurementNeckUnit.Text = (txtUseImperial.Value == "1") ? "inches" : "cm";
        lblMeasurementWaistUnit.Text = (txtUseImperial.Value == "1") ? "inches" : "cm";
        lblMeasurementHipUnit.Text = (txtUseImperial.Value == "1") ? "inches" : "cm";

        lblWeightHistoryGainWeightUnit.Text = (txtUseImperial.Value == "1") ? "lbs" : "kg";

        //lblCurrentEWL1.Text = (txtUseImperial.Value == "1") ? "lbs" : "kg";
        lblOrgWeight_Unit.Text = (txtUseImperial.Value == "1") ? "lbs" : "kg";
        lblPreOpWeightLoss_Unit.Text = (txtUseImperial.Value == "1") ? "lbs" : "kg";
        lblLowestWeightAchieved_Unit.Text = (txtUseImperial.Value == "1") ? "lbs" : "kg";
        

        gClass.FetchSystemDetails(base.UserPracticeCode); //intUserPracticeCode
        txtHRefBMI.Value = gClass.SD_ReferenceBMI > 0 ? gClass.SD_ReferenceBMI.ToString() : "27";
        txtHTargetBMI.Value = gClass.SD_TargetBMI > 0 ? gClass.SD_TargetBMI.ToString() : "27";
        lblMeasurementIBMI.Text = txtHRefBMI.Value;
        lblMeasurementTBMI.Text = txtHTargetBMI.Value;
        return;
    }
    #endregion

    #region protected void PatientData_DeleteProc()
    protected void PatientData_DeleteProc()
    {
        SqlCommand cmdUpdate = new SqlCommand();
        string strScript = String.Empty;

        gClass.MakeStoreProcedureName(ref cmdUpdate, "sp_PatientData_DeleteData", true);
        cmdUpdate.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdUpdate.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);

        cmdUpdate.Parameters.Add("@DeletedByUser", SqlDbType.VarChar, 50).Value = Context.Request.Cookies["Logon_UserName"].Value;
        cmdUpdate.Parameters.Add("@DateDeleted", SqlDbType.DateTime).Value = Convert.ToDateTime(txtHCurrentDate.Value);

        try
        {
            gClass.ExecuteDMLCommand(cmdUpdate);
            gClass.SaveUserLogFile(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Cookies["Logon_UserName"].Value,
                                    Context.Request.Url.Host, "Patient Detail Form", 2, "Delete Patient Data", "Patient ID", Request.Cookies["PatientID"].Value);

            gClass.SaveActionLog(gClass.OrganizationCode,
                                         Request.Cookies["UserPracticeCode"].Value,
                                         Request.Url.Host,
                                         System.Configuration.ConfigurationManager.AppSettings["MedicalRecordPage"].ToString(),
                                         System.Configuration.ConfigurationManager.AppSettings["LogDelete"].ToString(),
                                         "Delete " + System.Configuration.ConfigurationManager.AppSettings["PatientPage"].ToString() + " Data",
                                         txtPatientID.Value,
                                         "");

            //go back to first page
            strScript = "javascript:document.location.assign(document.getElementById('txtHApplicationURL').value + 'Forms/PatientsVisits/PatientsVisitsForm.aspx');";
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                        Context.Request.Cookies["Logon_UserName"].Value, "PatientID : " + Context.Request.Cookies["PatientID"].Value, "Data deleting Patient", err.ToString());
        }
        ScriptManager.RegisterStartupScript(linkBtnSave, linkBtnSave.GetType(), Guid.NewGuid().ToString(), strScript, true);
        return;
    }
    #endregion
    
    #region private void RegisterClientScript()
    private void RegisterClientScript()
    {
        Page.Culture = Request.Cookies["CultureInfo"].Value;
        txtHCurrentDate.Value = DateTime.Now.ToShortDateString();

        System.Globalization.CultureInfo myCI = new System.Globalization.CultureInfo(Request.Cookies["CultureInfo"].Value, false);
        lblDateFormat.Text = myCI.DateTimeFormat.ShortDatePattern.ToLower();
        txtBirthDate.toolTip = myCI.DateTimeFormat.ShortDatePattern;
        txtZeroDate.toolTip = myCI.DateTimeFormat.ShortDatePattern;
        txtRefDate1.toolTip = myCI.DateTimeFormat.ShortDatePattern;
        txtRefDate2.toolTip = myCI.DateTimeFormat.ShortDatePattern;
        txtRefDate3.toolTip = myCI.DateTimeFormat.ShortDatePattern;

        bodyPatientData.Style.Add("Direction", Request.Cookies["Direction"].Value);
        bodyPatientData.Attributes.Add("onload", "javascript:InitializePage();");
        cmbReferredDoctorsList.Style.Add("display", "none");
        cmbCity.Style.Add("display", "none");
        cmbInsurance.Style.Add("display", "none");
        //txtCurrentWeight.SetStyle("display", "none");
        txtIdealWeight.SetStyle("display", "none");

        return;
    }
    #endregion

    #region private void FillDropdownLists( )
    private void FillDropdownLists()
    {
        DataSet dsList = new DataSet();
        int Xt = 0;

        cmbCity.DataSource = gClass.FillCityList(gClass.OrganizationCode, Request.Cookies["UserPracticeCode"].Value); //intUserPracticeCode.ToString()
        cmbCity.DataMember = "tblCity";
        cmbCity.DataValueField = "Suburb_Value";
        cmbCity.DataTextField = "Suburb";
        cmbCity.DataBind();
        cmbCity.Items.Insert(0, new ListItem("Select...", ""));

        // Page 1 - Insurance
        cmbInsurance.DataSource = gClass.FillInsuranceList();
        cmbInsurance.DataMember = "tblInsurance";
        cmbInsurance.DataValueField = "Insurance";
        cmbInsurance.DataTextField = "Insurance";
        cmbInsurance.DataBind();
        cmbInsurance.Items.Insert(0, new ListItem("Select...", ""));

        // 3) Page 1 - Referred Doctor
        dsList = gClass.FillReferredDoctorList(gClass.OrganizationCode, Request.Cookies["UserPracticeCode"].Value); //intUserPracticeCode.ToString()
        cmbReferredDoctorsList.DataSource = dsList;
        cmbReferredDoctorsList.DataMember = "tblReferredDoctor";
        cmbReferredDoctorsList.DataValueField = "RefDrId";
        cmbReferredDoctorsList.DataTextField = "Doctor_Name";
        cmbReferredDoctorsList.DataBind();
        cmbReferredDoctorsList.Items.Insert(0, new ListItem("Select...", ""));

        for (int Xh = 0; Xh < cmbReferredDoctorsList.Items.Count; Xh++)
        {
            if (cmbReferredDoctorsList.Items[Xh].Value.IndexOf("`") != -1)
                cmbReferredDoctorsList.Items[Xh].Value = cmbReferredDoctorsList.Items[Xh].Value.Replace("`", "'");
            if (cmbReferredDoctorsList.Items[Xh].Text.IndexOf("`") != -1)
                cmbReferredDoctorsList.Items[Xh].Text = cmbReferredDoctorsList.Items[Xh].Text.Replace("`", "'");

            // cmbReferredDoctorsList.Items[Xh].Attributes.Add("suburb", dsList.Tables[0].Columns["Suburb"].ToString());
        }
        for (int Xh = 1; Xh < dsList.Tables[0].Rows.Count; Xh++)
        {
            Xt = Xh;
            Xt -= 1;
            cmbReferredDoctorsList.Items[Xh].Attributes.Add("title", dsList.Tables[0].Rows[Xt]["Suburb"].ToString());
        }
        return;
    }
    #endregion

    #region private void LoadLastPatientCustomIDFromDatabase()
    /// <summary>
    /// This function is to load the "Last Patient Custom ID" + 1, when a new patient is being adding.
    /// </summary>
    private void LoadLastPatientCustomIDFromDatabase()
    {
        SqlCommand cmdPatientCustomID = new SqlCommand();
        DataSet dsPatientCustomID;
        Int64 intPatientCustomID = 0;

        gClass.MakeStoreProcedureName(ref cmdPatientCustomID, "sp_PatientData_LastPatientCustomID", true);
        cmdPatientCustomID.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Int32.TryParse(gClass.OrganizationCode, out int32Temp) ? int32Temp : 0;

        dsPatientCustomID = gClass.FetchData(cmdPatientCustomID, "tblPatient");

        for (int Xh = 0; Xh < dsPatientCustomID.Tables[0].Rows.Count; Xh++)
        {
            if (Convert.ToInt64(dsPatientCustomID.Tables[0].Rows[Xh]["PatientCustomID"].ToString()) > intPatientCustomID)
                intPatientCustomID = Convert.ToInt64(dsPatientCustomID.Tables[0].Rows[Xh]["PatientCustomID"].ToString());
        }

        if (intPatientCustomID == 0)
            LoadLastPatientIDFromDatabase();
        else
        {
            txtPatient_CustomID.Text = (intPatientCustomID + 1).ToString();
        }
    }
    #endregion

    #region private void LoadLastPatientIDFromDatabase()
    /// <summary>
    /// This function is to load the "Last Patient ID" + 1, when a new patient is being adding.
    /// </summary>
    private void LoadLastPatientIDFromDatabase()
    {
        SqlCommand cmdPatientID = new SqlCommand();
        DataSet dsPatientID;
        Int32 intPatientID = 0;

        gClass.MakeStoreProcedureName(ref cmdPatientID, "sp_PatientData_LastPatientID", true);
        dsPatientID = gClass.FetchData(cmdPatientID, "tblPatient");
        txtPatient_CustomID.Text = (dsPatientID.Tables.Count > 0 && dsPatientID.Tables[0].Rows.Count > 0) ? dsPatientID.Tables[0].Rows[0][0].ToString() : "";
        if (dsPatientID.Tables.Count > 0 && dsPatientID.Tables[0].Rows.Count > 0)
        {
            Int32.TryParse(dsPatientID.Tables[0].Rows[0][0].ToString(), out intPatientID);
            txtPatient_CustomID.Text = (intPatientID + 1).ToString();
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

    #region private void RemoveFirstItemFromDropDownList()
    private void RemoveFirstItemFromDropDownList()
    {
        if (cmbPrevBariatricSurgery.Items.Count > 0)
            cmbPrevBariatricSurgery.Items.RemoveAt(0);
        if (cmbPrevNonBariatricSurgery.Items.Count > 0)
            cmbPrevNonBariatricSurgery.Items.RemoveAt(0);
        if (cmbAdverseEvents.Items.Count > 0)
            cmbAdverseEvents.Items.RemoveAt(0);

        FillBoldList(cmbPrevBariatricSurgery, listPrevBariatricSurgery);
        FillBoldList(cmbPrevNonBariatricSurgery, listPrevNonBariatricSurgery);
        FillBoldList(cmbAdverseEvents, listAdverseEvents);
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
        gClass.MakeStoreProcedureName(ref cmdSave, "sp_PatientWeightData_cmdInsert", true);

        cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int);
        cmdSave.Parameters.Add("@UserPracticeCode", SqlDbType.Int);
        cmdSave.Parameters.Add("@PatientId", SqlDbType.Int);
        cmdSave.Parameters.Add("@PageNo", SqlDbType.Int);
        // PAGE 2
        cmdSave.Parameters.Add("@Height", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@StartWeight", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@IdealWeight", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@CurrentWeight", SqlDbType.Decimal);
        cmdSave.Parameters.Add("@TargetWeight", SqlDbType.Decimal);
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
        cmdSave.Parameters.Add("@StartBP1", SqlDbType.Int);
        cmdSave.Parameters.Add("@StartBP2", SqlDbType.Int);
        
        cmdSave.Parameters.Add("@Zerodate", SqlDbType.DateTime);
        cmdSave.Parameters.Add("@VisitWeeksFlag", SqlDbType.Int);

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

    #region private string CalculateAge(String strBirthDate)
    private string CalculateAge(String strBirthDate)
    {
        int intAge = 0;
        DateTime dtTemp;

        if (DateTime.TryParse(strBirthDate, out dtTemp))
        {
            DateTime BirthDate = dtTemp;
            intAge = DateTime.Now.Year - BirthDate.Year;
            if (DateTime.Now.Month < BirthDate.Month)
                --intAge;
            else if (DateTime.Now.Month == BirthDate.Month)
                if (DateTime.Now.Day < BirthDate.Day)
                    --intAge;
        }
        else intAge = 0;
        return (intAge.ToString());
    }
    #endregion

    #region private void SetReferredDoctors(DropDownList cmbRefDoctor, String strValue)
    private void SetReferredDoctors(UserControl_TextBoxWUCtrl txtRefDoctor, String strValue)
    {
        string foundItem = "";

        foreach (ListItem item in cmbReferredDoctorsList.Items)
        {
            if (item.Value.ToLower().Equals(strValue.ToLower()))
                foundItem = item.Text;
        }

        if (foundItem != "")
            txtRefDoctor.Text = foundItem;
        else
            txtRefDoctor.Text = "Select...";

        return;
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

    #region private string GetFilePath
    private string GetFilePath(string fileName)
    {
        int intLastIndex = fileName.LastIndexOf("\\");
        if (intLastIndex == 0)
            intLastIndex = fileName.LastIndexOf("/");

        fileName = fileName.Substring(intLastIndex + 1);
        return System.IO.Path.Combine(GetDocumentPath(), gClass.OrganizationCode + "_" + Request.Cookies["PatientID"].Value + "_" + fileName);
    }
        #endregion

    #region private void GetDocumentPath()
    private string GetDocumentPath()
    {
        string uploadFolder, strFolder = "";
        System.IO.DirectoryInfo di;
        strFolder = "Photos/Baseline";

        uploadFolder = System.IO.Path.Combine(this.Context.ApplicationInstance.Request.PhysicalApplicationPath, strFolder);

        di = new System.IO.DirectoryInfo(uploadFolder);
        if (di.Exists == false) // Create the directory only if it does not already exist.
            di.Create();
        return (uploadFolder);
    }
    #endregion

    #region protected void SavePatientLabs()
    protected void SavePatientLabs()
    {
        if (txtFile.PostedFile.FileName.Trim() != "")
        {
            SqlCommand cmdSave = new SqlCommand();
            SqlCommand cmdLoad = new SqlCommand();
            DataSet dsPathologyID;
            int newPathologyID = 0;
            try
            {
                gClass.MakeStoreProcedureName(ref cmdSave, "sp_PatientPathology_UpdatePathology", true);

                //save pathology id
                cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                cmdSave.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
                cmdSave.Parameters.Add("@PathologyBaseline", SqlDbType.VarChar).Value = "baseline";
                cmdSave.Parameters.Add("@DeletedByUser", SqlDbType.VarChar, 50).Value = Context.Request.Cookies["Logon_UserName"].Value;
                cmdSave.Parameters.Add("@DateDeleted", SqlDbType.DateTime).Value = Convert.ToDateTime(txtHCurrentDate.Value);


                gClass.ExecuteDMLCommand(cmdSave);
                gClass.SaveUserLogFile(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Cookies["Logon_UserName"].Value,
                                        Context.Request.Url.Host, "EMR Form", 2, "Save Pathology", "PID:", Context.Request.Cookies["PatientID"].Value);

                gClass.SaveActionLog(gClass.OrganizationCode,
                                         Request.Cookies["UserPracticeCode"].Value,
                                         Request.Url.Host,
                                         System.Configuration.ConfigurationManager.AppSettings["MedicalRecordPage"].ToString(),
                                         System.Configuration.ConfigurationManager.AppSettings["LogUpdate"].ToString(),
                                         "Save " + System.Configuration.ConfigurationManager.AppSettings["PathologyPage"].ToString() + " Data",
                                         Request.Cookies["PatientID"].Value,
                                         "");

                gClass.MakeStoreProcedureName(ref cmdLoad, "sp_PatientPathology_LastPathologyID", true);
                cmdLoad.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                cmdLoad.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);

                dsPathologyID = gClass.FetchData(cmdLoad, "tblPatientPathology");

                Int32.TryParse(dsPathologyID.Tables[0].Rows[0]["PathologyID"].ToString(), out newPathologyID);
            }
            catch (Exception err)
            {
                gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                            Context.Request.Cookies["Logon_UserName"].Value, "EMR PID : " + Context.Request.Cookies["PatientID"].Value, "Save Pathology", err.ToString());
            }
            string strFilePath = GetFilePath(txtFile.PostedFile.FileName);

            if (newPathologyID > 0)
            {
                //save pathology data
                try
                {
                    string strDocumentName = "";

                    if (System.IO.File.Exists(strFilePath))
                        System.IO.File.Delete(strFilePath);
                    strDocumentName = txtFile.PostedFile.FileName;

                    txtFile.PostedFile.SaveAs(strFilePath);

                    StreamReader streamReader = new StreamReader(strFilePath);

                    string message = streamReader.ReadToEnd();
                    streamReader.Close();

                    PipeParser parser = new PipeParser();
                    IMessage m = parser.Parse(message);

                    ORU_R01 oruR01 = m as ORU_R01;
                    oruR01 = oruR01;

                    string testID = "";
                    string testName = "";
                    string testValue = "";
                    string testUnit = "";
                    string testRange = "";
                    string testStatus = "";
                    string testDate = "";
                    string pathologyDate = "";
                    string year = "";
                    string month = "";
                    string date = "";

                    gClass.MakeStoreProcedureName(ref cmdSave, "sp_PatientPathology_InsertPathologyData", true);
                    cmdSave.Parameters.Clear();

                    cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                    cmdSave.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
                    cmdSave.Parameters.Add("@PathologyID", SqlDbType.Int).Value = newPathologyID;
                    cmdSave.Parameters.Add("@PathologyDataDate", SqlDbType.DateTime);
                    cmdSave.Parameters.Add("@SectionID", SqlDbType.Int);
                    cmdSave.Parameters.Add("@TestID", SqlDbType.VarChar);
                    cmdSave.Parameters.Add("@TestName", SqlDbType.VarChar);
                    cmdSave.Parameters.Add("@TestValue", SqlDbType.VarChar);
                    cmdSave.Parameters.Add("@TestUnit", SqlDbType.VarChar);
                    cmdSave.Parameters.Add("@TestRange", SqlDbType.VarChar);
                    cmdSave.Parameters.Add("@TestStatus", SqlDbType.VarChar);

                    //gClass.TruncateDate(dvPatient[0]["BirthDate"].ToString().Trim(), Request.Cookies["CultureInfo"].Value, 1);

                    for (int i = 0; i < oruR01.RESPONSERepetitionsUsed; i++)
                    {
                        for (int j = 0; j < oruR01.GetRESPONSE(i).ORDER_OBSERVATIONRepetitionsUsed; j++)
                        {
                            testDate = oruR01.GetRESPONSE(i).GetORDER_OBSERVATION(j).OBR.RequestedDateTime.TimeOfAnEvent.Value.ToString();

                            year = testDate.Substring(0, 4);
                            month = testDate.Substring(4, 2);
                            date = testDate.Substring(6, 2);
                            pathologyDate = date + "-" + month + "-" + year;

                            cmdSave.Parameters["@PathologyDataDate"].Value = gClass.TruncateDate(pathologyDate, Request.Cookies["CultureInfo"].Value, 1);
                            for (int k = 0; k < oruR01.GetRESPONSE(i).GetORDER_OBSERVATION(j).OBSERVATIONRepetitionsUsed; k++)
                            {
                                testID = CheckNull(oruR01.GetRESPONSE(i).GetORDER_OBSERVATION(j).GetOBSERVATION(k).OBX.ObservationIdentifier.Identifier.Value);
                                testName = CheckNull(oruR01.GetRESPONSE(i).GetORDER_OBSERVATION(j).GetOBSERVATION(k).OBX.ObservationIdentifier.Text.Value);
                                testValue = CheckNull(oruR01.GetRESPONSE(i).GetORDER_OBSERVATION(j).GetOBSERVATION(k).OBX.GetObservationValue(0).Data);
                                testUnit = CheckNull(oruR01.GetRESPONSE(i).GetORDER_OBSERVATION(j).GetOBSERVATION(k).OBX.Units.Identifier.Value);
                                testRange = CheckNull(oruR01.GetRESPONSE(i).GetORDER_OBSERVATION(j).GetOBSERVATION(k).OBX.ReferencesRange.Value);
                                testStatus = CheckNull(oruR01.GetRESPONSE(i).GetORDER_OBSERVATION(j).GetOBSERVATION(k).OBX.AbnormalFlagsRepetitionsUsed);

                                cmdSave.Parameters["@SectionID"].Value = i;
                                cmdSave.Parameters["@TestID"].Value = testID;
                                cmdSave.Parameters["@TestName"].Value = testName;
                                cmdSave.Parameters["@TestValue"].Value = testValue;
                                cmdSave.Parameters["@TestUnit"].Value = testUnit;
                                cmdSave.Parameters["@TestRange"].Value = testRange;
                                cmdSave.Parameters["@TestStatus"].Value = testStatus;

                                gClass.ExecuteDMLCommand(cmdSave);
                                gClass.SaveUserLogFile(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Cookies["Logon_UserName"].Value,
                                                        Context.Request.Url.Host, "EMR Form", 2, "Save Pathology Data", "PID:", Context.Request.Cookies["PatientID"].Value);

                                gClass.SaveActionLog(gClass.OrganizationCode,
                                         Request.Cookies["UserPracticeCode"].Value,
                                         Request.Url.Host,
                                         System.Configuration.ConfigurationManager.AppSettings["MedicalRecordPage"].ToString(),
                                         System.Configuration.ConfigurationManager.AppSettings["LogUpdate"].ToString(),
                                         "Save " + System.Configuration.ConfigurationManager.AppSettings["PathologyPage"].ToString() + " Data",
                                         Context.Request.Cookies["PatientID"].Value,
                                         "");
                            }
                        }
                    }

                    if (pathologyDate != "")
                    {
                        gClass.MakeStoreProcedureName(ref cmdSave, "sp_PatientPathology_UpdatePathology", true);
                        cmdSave.Parameters.Clear();
                        //save pathology id
                        cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                        cmdSave.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
                        cmdSave.Parameters.Add("@PathologyID", SqlDbType.Int).Value = Convert.ToInt32(newPathologyID);
                        cmdSave.Parameters.Add("@PathologyDate", SqlDbType.DateTime).Value = gClass.TruncateDate(pathologyDate, Request.Cookies["CultureInfo"].Value, 1);


                        gClass.ExecuteDMLCommand(cmdSave);
                    }


                    IsDoneSaveFlag = 1;
                }
                catch (Exception err)
                {
                    IsDoneSaveFlag = 0;
                    gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                                Context.Request.Cookies["Logon_UserName"].Value, "EMR PID : " + Context.Request.Cookies["PatientID"].Value, "Save Pathology Data", err.ToString());
                }
            }
        }
    }
    #endregion

    #region private string CheckNull(Object oData)
    private string CheckNull(Object oData)
    {
        string returnString = "";

        if (oData != null)
            returnString = oData.ToString();

        return returnString;
    }
    #endregion
        
    #region private Boolean ValidateBOLDPatientData()
    private Boolean ValidateBOLDPatientData()
    {
        BoldErrorMsg += cmbRace.SelectedValue.ToString() != "" ? "" : "race, ";
        BoldErrorMsg += cmbEmployment.SelectedValue.ToString() != "" ? "" : "employment status, ";

        if (txtPrevBariatricSurgery_Selected.Value.Trim() != "")
        {
            BoldErrorMsg += txtHDoctorBoldList.Value.IndexOf(cmbSurgeon.SelectedValue + "-") < 0 ? "surgeon must have BOLD ID, " : "";
        }

        if (chkSelfPay.Checked == false && chkCharity.Checked == false && chkMedicare.Checked == false && chkMedicaid.Checked == false && chkPrivateIns.Checked == false && chkGovernmentIns.Checked == false)
        {
            BoldErrorMsg += "payment option, ";
        }

        if (BoldErrorMsg != "")
        {
            BoldErrorMsg = " Please check the " + BoldErrorMsg.Substring(0, BoldErrorMsg.Length - 2);

            IsDoneSaveFlag = 0;
            return false;
        }
        return true;
    }
    #endregion

    #region private void SubmitBOLDPatientData()
    private void SubmitBOLDPatientData()
    {
        Int32 intPatientID;
        Int32.TryParse(txtPatientID.Value, out intPatientID);
        String strAuth = "";

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
            objSRC.SavePatientData();


            if (objSRC.PatientErrors.Count > 0)
            {
                for (int Xh = 0; Xh < objSRC.PatientErrors.Count; Xh++)
                {
                    gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                                      Context.Request.Cookies["Logon_UserName"].Value, "Patient ID : " + intPatientID.ToString(), "BOLD - Data saving Patient ", objSRC.PatientErrors[Xh].ErrorMessage.ToString());
                }
            }
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                        Context.Request.Cookies["Logon_UserName"].Value, "Patient ID : " + intPatientID.ToString(), "BOLD - Data Patient ", strAuth + " " +err.ToString());
        }
        //----------------------------------------------------------------------------------------------------
    }
    #endregion

    #region private void LoadBoldList()
    private void LoadBoldList()
    {
        SqlCommand cmdSelect = new SqlCommand();
        string strReturn = String.Empty;
        string doctorList = "";
        string hospitalList = "";
        int Xh;

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_Doctors_LoadData", true);
        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSelect.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);

        DataSet dsDoctorTemp = gClass.FetchData(cmdSelect, "tblDoctor");

        for (Xh = 0; Xh < dsDoctorTemp.Tables[0].Rows.Count; Xh++)
        {
            if (dsDoctorTemp.Tables[0].Rows[Xh]["DoctorBoldCode"].ToString().Trim() != "")
                doctorList += dsDoctorTemp.Tables[0].Rows[Xh]["DoctorID"].ToString().Trim() + "-";
        }
        txtHDoctorBoldList.Value = doctorList;

        cmdSelect.Parameters.Clear();
        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_Hospitals_LoadData", true);
        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);

        DataSet dsHospitalTemp = gClass.FetchData(cmdSelect, "tblHospital");

        for (Xh = 0; Xh < dsHospitalTemp.Tables[0].Rows.Count; Xh++)
        {
            if (dsHospitalTemp.Tables[0].Rows[Xh]["HospitalBoldCode"].ToString().Trim() != "")
                hospitalList += dsHospitalTemp.Tables[0].Rows[Xh]["Hospital ID"].ToString().Trim() + "-";
        }
        txtHHospitalBoldList.Value = hospitalList;
    }
    #endregion

    #region protected FillTemplate()
    protected void FillTemplate()
    {
        SqlCommand cmdSelect = new SqlCommand();

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_Templates_LoadData", true);
        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSelect.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);

        DataView dvReportTemplate = gClass.FetchData(cmdSelect, "tblReportTemplate").Tables[0].DefaultView;
        
        HtmlSelect tempControlSelect = new HtmlSelect();
        String tempSelect = "";

        for (int Xh = 0; Xh < Template.Length / 2; Xh++)
        {
            tempSelect = Template[Xh, 1];

            tempControlSelect = (HtmlSelect)FindControlRecursive(this.Page, Template[Xh, 0]);
            tempControlSelect.DataSource = dvReportTemplate;
            tempControlSelect.DataMember = "tblReportTemplate";
            tempControlSelect.DataTextField = "TemplateName";
            tempControlSelect.DataValueField = "TemplateID";
            tempControlSelect.DataBind();
            tempControlSelect.Items.Insert(0, new ListItem("", ""));
            tempControlSelect.Items[0].Selected = true;
        }
        
        DataSet dsTemplate = new DataSet();        
        String tempDBValue = "";
        String tempTemplateID = "";
        
        try
        {
            cmdSelect.Parameters.Clear();
            gClass.MakeStoreProcedureName(ref cmdSelect, "sp_ReportTemplates_LoadDataByID", true);
            cmdSelect.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
            cmdSelect.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
            dsTemplate = gClass.FetchData(cmdSelect, "tblReportTemplate");

            if ((dsTemplate.Tables.Count > 0) && (dsTemplate.Tables[0].Rows.Count > 0))
            {
                for (int Xh = 0; Xh < Template.Length / 2; Xh++)
                {
                    tempSelect = Template[Xh, 1];
                    
                    for (int Xi = 0; Xi < dsTemplate.Tables[0].Rows.Count; Xi++)
                    {
                        tempDBValue = dsTemplate.Tables[0].Rows[Xi]["ReportTemplateName"].ToString();
                        if (tempDBValue == tempSelect)
                        {
                            tempControlSelect = (HtmlSelect)FindControlRecursive(this.Page, Template[Xh, 0]);
                            tempTemplateID = dsTemplate.Tables[0].Rows[0]["TemplateID"].ToString();
                            //tempControlSelect.Items.FindByValue(tempTemplateID).Selected = true;
                            //not yet working
                        }
                    }
                }                
            }
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Patient Data Form", "Loading Report Template", err.ToString());
        }
    }
    #endregion

    #region private void SaveReportTemplate()
    private void SaveReportTemplate()
    {
        if (txtPatientID.Value.Equals("0"))
        {
            IsDoneSaveFlag = -1;
            return;
        }
        String strTemplateType = "";
        Int32 intTemplateValue = 0;
        Boolean savePage = true;
        SqlCommand cmdSave = new SqlCommand();

        cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSave.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);

        gClass.MakeStoreProcedureName(ref cmdSave, "sp_ReportTemplates_SaveData", true);
        
        HtmlSelect tempControlSelect = new HtmlSelect();

        try
        {
            for (int Xh = 0; Xh < Template.Length / 2; Xh++)
            {
                tempControlSelect = (HtmlSelect)FindControlRecursive(this.Page, Template[Xh, 0]);
                strTemplateType = Template[Xh, 1];
                intTemplateValue = Int32.TryParse(tempControlSelect.Value, out intTemplateValue) ? intTemplateValue : 0;
                
                cmdSave.Parameters.Clear();
                cmdSave.Parameters.Add("@ReportTemplateName", SqlDbType.VarChar, 20).Value = strTemplateType.Replace("'", "`");
                cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                cmdSave.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
                cmdSave.Parameters.Add("@TemplateID", SqlDbType.Int).Value = intTemplateValue;

                gClass.ExecuteDMLCommand(cmdSave);    
            }
            gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value, Request.Cookies["Logon_UserName"].Value, Request.Url.Host,
                    "Baseline Form", 2, "Modify report template", "PatientCode", txtPatientID.Value);
            IsDoneSaveFlag = 1;
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host,
                    Request.Cookies["Logon_UserName"].Value, "Baseline Form", "Data saving report template", err.ToString());
            IsDoneSaveFlag = 0;
        }
        cmdSave.Dispose();
    }
    #endregion 

    #region private void SaveActionDetailLog()
    private void SaveActionDetailLog()
    {
        SqlCommand cmdSaveLog = new SqlCommand();
        //insert log
        //before updating data
        try
        {
            if (savePatientData == false)
            {
                gClass.MakeStoreProcedureName(ref cmdSaveLog, "sp_PatientDataLog_cmdInsert", true);
                cmdSaveLog.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                cmdSaveLog.Parameters.Add("@PatientId", SqlDbType.Int).Value = Convert.ToInt32(txtPatientID.Value);
                cmdSaveLog.Parameters.Add("@LogUserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
                cmdSaveLog.Parameters.Add("@LogDateTime", SqlDbType.DateTime).Value = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
                gClass.ExecuteDMLCommand(cmdSaveLog);
                savePatientData = true;
            }
            
            if (saveEMRData == false)
            {
                cmdSaveLog = new SqlCommand();
                gClass.MakeStoreProcedureName(ref cmdSaveLog, "sp_PatientEMRLog_cmdInsert", true);
                cmdSaveLog.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                cmdSaveLog.Parameters.Add("@PatientId", SqlDbType.Int).Value = Convert.ToInt32(txtPatientID.Value);
                cmdSaveLog.Parameters.Add("@LogUserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
                cmdSaveLog.Parameters.Add("@LogDateTime", SqlDbType.DateTime).Value = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
                gClass.ExecuteDMLCommand(cmdSaveLog);
                saveEMRData = true;
            }

            if (saveComorbidityData == false)
            {
                cmdSaveLog = new SqlCommand();
                gClass.MakeStoreProcedureName(ref cmdSaveLog, "sp_ConsultFu1_BoldComorbidityLog_SaveData", true);
                cmdSaveLog.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                cmdSaveLog.Parameters.Add("@PatientId", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
                cmdSaveLog.Parameters.Add("@LogUserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
                cmdSaveLog.Parameters.Add("@LogDateTime", SqlDbType.DateTime).Value = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
                gClass.ExecuteDMLCommand(cmdSaveLog);
                saveComorbidityData = true;
            }

            if (saveComorbidityCheckData == false)
            {
                cmdSaveLog = new SqlCommand();
                gClass.MakeStoreProcedureName(ref cmdSaveLog, "sp_ConsultFu1_ComorbidityCheckLog_SaveData", true);
                cmdSaveLog.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                cmdSaveLog.Parameters.Add("@PatientId", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
                cmdSaveLog.Parameters.Add("@LogUserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
                cmdSaveLog.Parameters.Add("@LogDateTime", SqlDbType.DateTime).Value = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
                gClass.ExecuteDMLCommand(cmdSaveLog);
                saveComorbidityCheckData = true;
            }

            if (savePatientWeightData == false)
            {
                cmdSaveLog = new SqlCommand();
                gClass.MakeStoreProcedureName(ref cmdSaveLog, "sp_PatientWeightDataLog_cmdInsert", true);
                cmdSaveLog.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                cmdSaveLog.Parameters.Add("@PatientId", SqlDbType.Int).Value = Convert.ToInt32(txtPatientID.Value);
                cmdSaveLog.Parameters.Add("@LogUserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
                cmdSaveLog.Parameters.Add("@LogDateTime", SqlDbType.DateTime).Value = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
                cmdSaveLog.Parameters.Add("@PageNo", SqlDbType.Int).Value = 2;
                gClass.ExecuteDMLCommand(cmdSaveLog);
                savePatientWeightData = true;
            }

            if (saveBoldData == false)
            {
                cmdSaveLog = new SqlCommand();
                gClass.MakeStoreProcedureName(ref cmdSaveLog, "sp_PatientDataLog_SaveBoldData", true);
                cmdSaveLog.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                cmdSaveLog.Parameters.Add("@PatientId", SqlDbType.Int).Value = Convert.ToInt32(txtPatientID.Value);
                cmdSaveLog.Parameters.Add("@LogUserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
                cmdSaveLog.Parameters.Add("@LogDateTime", SqlDbType.DateTime).Value = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
                gClass.ExecuteDMLCommand(cmdSaveLog);
                saveBoldData = true;
            }
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                        Context.Request.Cookies["Logon_UserName"].Value, "PID : " + txtPatientID.Value, "Data Saving Patient Data Log", err.ToString());
        }
    }
    #endregion
}