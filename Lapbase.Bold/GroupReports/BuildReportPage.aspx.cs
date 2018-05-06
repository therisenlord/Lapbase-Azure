using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using dotnetCHARTING;
using Microsoft.Reporting.WebForms;
using System.Collections.Generic;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

public partial class Reports_BuildReportPage : System.Web.UI.Page
{
    private string  strXSLTFileName = "";
    private string strLapbaseDataSourceName = "dsLapbase";
    private int intUserPracticeCode = 0, intPatientID = 0, intOrganizationCode;

    String strNumberFormat = System.Configuration.ConfigurationManager.AppSettings["NumberFormat"].ToString();

    private string titleSurgeon = "";
    private string titleHospital = "";
    private string titleRegion = "";
    private string titleOperation = "";
    private string titleApproach = "";
    private string titleCategory = "";
    private string titleGroup = "";
    private string titleBandType = "";
    private string titleBandSize = "";
    private string titleDate = "";
    private string titleAge = "";
    private string titleBMI = "";
    private string titleSerialNo = "";

    private string titleGraphPatientName = "";
    private string titleGraphPatientAge = "";
    private string titleGraphPatientStartWeight = "";
    private string titleGraphPatientCurrentWeight = "";
    private string titleGraphPatientSurgeryDate = "";
    private string titleGraphPatientInitialBMI = "";
    private string titleGraphPatientTargetWeight = "";
    DateTime titleGraphPatientFirstVisitDate;
    
    GlobalClass gClass = new GlobalClass();

    Dictionary<string, string> codeComDesc = new Dictionary<string, string>();
    Dictionary<string, string> codeComQuestDesc = new Dictionary<string, string>();
    Dictionary<string, string> codeComRank = new Dictionary<string, string>();
    Dictionary<string, string> codeAdevDesc = new Dictionary<string, string>();
    Dictionary<string, string> codePBSDesc = new Dictionary<string, string>();
    Dictionary<string, string> codePBNSDesc = new Dictionary<string, string>();
    Dictionary<string, string> codeDemoDesc = new Dictionary<string, string>();
    Dictionary<string, string> codeEMPDesc = new Dictionary<string, string>();
    Dictionary<string, string> codeDWLDesc = new Dictionary<string, string>();
    Dictionary<string, string> codeASADesc = new Dictionary<string, string>();
    Dictionary<string, string> codeContactDesc = new Dictionary<string, string>();
    Dictionary<string, string> codeAssistantDesc = new Dictionary<string, string>();
    Dictionary<string, string> codeConcurrentDesc = new Dictionary<string, string>();
    Dictionary<string, string> codeReasonDesc = new Dictionary<string, string>();
    Dictionary<string, string> codeAdevSTDesc = new Dictionary<string, string>();
    Dictionary<string, string> codeBSTDesc = new Dictionary<string, string>();
    Dictionary<string, string> codeBackgroundDesc = new Dictionary<string, string>();


    String[,] comorbidityArr = new String[,] {{"cmbHypertension","CVS_Hypertension","bold"},{ "cmbCongestive","CVS_Congestive","bold"},{ "cmbIschemic","CVS_Ischemic","bold"},{ "cmbAngina","CVS_Angina","bold"},
        {"cmbPeripheral","CVS_Peripheral","bold"},{ "cmbLower","CVS_Lower","bold"},{ "cmbDVT","CVS_DVT","bold"},{ "cmbGlucose","MET_Glucose","bold"},{ "cmbLipids","MET_Lipids","bold"},
        {"cmbGout","MET_Gout","bold"},{ "cmbGred","GAS_Gerd","bold"},{ "cmbCholelithiasis","GAS_Cholelithiasis","bold"},{ "cmbLiver","GAS_Liver","bold"},{"cmbBackPain","MUS_BackPain","bold"},
        {"cmbMusculoskeletal","MUS_Musculoskeletal","bold"},{ "cmbFibro","MUS_Fibromyalgia","bold"},{ "cmbPsychosocial","PSY_Impairment","bold"},{ "cmbDepression","PSY_Depression","bold"},
        {"cmbConfirmed","PSY_MentalHealth","bold"},{ "cmbAlcohol","PSY_Alcohol","bold"},{ "cmbTobacco","PSY_Tobacoo","bold"},{ "cmbAbuse","PSY_Abuse","bold"},{ "cmbStressUrinary","GEN_Stress","bold"},
        {"cmbCerebri","GEN_Cerebri","bold"},{ "cmbHernia","GEN_Hernia","bold"},{ "cmbFunctional","GEN_Functional","bold"},{ "cmbSkin","GEN_Skin","bold"},{ "cmbObstructive","PUL_Obstructive","bold"},
        {"cmbObesity","PUL_Obesity","bold"},{ "cmbPulmonary","PUL_PulHypertension","bold"},{ "cmbAsthma","PUL_Asthma","bold"},{ "cmbPolycystic","REPRD_Polycystic","bold"},{ "cmbMenstrual","REPRD_Menstrual","bold"},
        {"cmbPrevPCISurgery","GEN_PrevPCISurgery","bold"},
        {"cmbRenalInsuff", "GEN_RenalInsuff", "both" }, { "cmbRenalFail", "GEN_RenalFail", "both" }, 
        {"cmbSteroid", "GEN_Steroid", "both" }, { "cmbTherapeutic", "GEN_Therapeutic", "both" }
    };

    #region protected void Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        gClass.OrganizationCode = Request.Cookies["OrganizationCode"].Value;
        intUserPracticeCode = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
        intOrganizationCode = Convert.ToInt32(gClass.OrganizationCode);
        if (Request.Cookies["PatientID"] != null)
            intPatientID = Convert.ToInt32(Request.Cookies["PatientID"].Value);

        if (!IsPostBack)
        {
            bodyReport.Attributes.Add("onload", "javascript:body_onload();");
            ChooseReports();
        }
        else { bodyReport.Attributes.Remove("onload"); }

        // Create a new dictionary of strings, with string keys.
        codeComDesc = buildCodeDictionary("com", "desc");
        codeComQuestDesc = buildCodeDictionary("comQuest", "desc");
        codeComRank = buildCodeDictionary("comRank", "rank");
        codeAdevDesc = buildCodeDictionary("adev", "desc");
        codePBSDesc = buildCodeDictionary("pbs", "desc");
        codePBNSDesc = buildCodeDictionary("nbst", "desc");
        codeDemoDesc = buildCodeDictionary("demo", "desc");
        codeEMPDesc = buildCodeDictionary("emp", "desc");
        codeDWLDesc = buildCodeDictionary("dwl", "desc");
        codeASADesc = buildCodeDictionary("asa", "desc");
        codeContactDesc = buildCodeDictionary("contact", "desc");
        codeAssistantDesc = buildCodeDictionary("assistant", "desc");
        codeConcurrentDesc = buildCodeDictionary("concurrent", "desc");
        codeReasonDesc = buildCodeDictionary("reason", "desc");
        codeAdevSTDesc = buildCodeDictionary("adevst", "desc");
        codeBSTDesc = buildCodeDictionary("bst", "desc");
        codeBackgroundDesc = buildCodeDictionary("background", "desc");

    }
    #endregion 

    #region private void ChooseReports
    private void ChooseReports()
    {
        txtReportCode.Value = Request.QueryString["RP"].ToUpper();

        switch (Request.QueryString["RP"].ToUpper())
        {
            case "FUA": //Follow Up Assessment
                txtReportName.Value = "Follow Up Assessment Report";
                break;

            case "RDL": //Ref. Doctor Letter
                txtReportName.Value = "Ref. Doctor Letter";
                break;

            case "COMPSUM" : // Complication Summary 1
                txtReportName.Value = "Adverse Event Summary Report";
                break;

            case "COMPSUMBYPATIENT": // Complication Summary 2
                txtReportName.Value = "Adverse Event Summary by Patient";
                break;

            case "OPERATIONLOS":
                txtReportName.Value = "Operation duration and LOS";
                break;

            case "PATIENTLIST":
                txtReportName.Value = "Patient List with Last Visit Date";
                break;

            case "COEREPORT" :
                txtReportName.Value = "Patient list with complications";
                break;

            case "SUMMARYBYQUARTER" :
                txtReportName.Value = "Weight Loss";
                break;

            case "SUMMARYONLY" :
                txtReportName.Value = "Summary Only";
                break;

            case "BMIEWLGRAPH" :
                txtReportName.Value = "BMI and %EWL Graph";
                break;

            case "BLC":
                txtReportName.Value = "Baseline Comorbidities";
                break;

            case "CMCP":
                txtReportName.Value = "Comorbidities and Complications";
                break;

            case "EWLG":
                gClass.SaveActionLog(gClass.OrganizationCode,
                                     Request.Cookies["UserPracticeCode"].Value,
                                     Request.Url.Host,
                                     System.Configuration.ConfigurationManager.AppSettings["VisitPage"].ToString(),
                                     System.Configuration.ConfigurationManager.AppSettings["LogPrint"].ToString(),
                                     "Load " + System.Configuration.ConfigurationManager.AppSettings["ReportEWL"].ToString(),
                                     Request.QueryString["PID"],
                                     "");
                if (Request.QueryString["preview"] == "2")
                {
                    txtReportName.Value = "% EWL Graph";
                }
                else
                {
                    Response.Redirect("EWLGraph/EWLGraphPage.aspx?PID=" + Request.QueryString["PID"]);
                }
                break;

            case "IEWLG":
                gClass.SaveActionLog(gClass.OrganizationCode,
                                     Request.Cookies["UserPracticeCode"].Value,
                                     Request.Url.Host,
                                     System.Configuration.ConfigurationManager.AppSettings["VisitPage"].ToString(),
                                     System.Configuration.ConfigurationManager.AppSettings["LogPrint"].ToString(),
                                     "Load " + System.Configuration.ConfigurationManager.AppSettings["ReportIWL"].ToString(),
                                     Request.QueryString["PID"],
                                     "");
                if (Request.QueryString["preview"] == "2")
                {
                    txtReportName.Value = "IEWL Graph";
                }
                else
                {
                    Response.Redirect("IEWLGraph/IEWLGraphPage.aspx?PID=" + Request.QueryString["PID"] + "&Param=" + Request.QueryString["Param"]);
                }
                break;

            case "WLG":
                gClass.SaveActionLog(gClass.OrganizationCode,
                                     Request.Cookies["UserPracticeCode"].Value,
                                     Request.Url.Host,
                                     System.Configuration.ConfigurationManager.AppSettings["VisitPage"].ToString(),
                                     System.Configuration.ConfigurationManager.AppSettings["LogPrint"].ToString(),
                                     "Load " + System.Configuration.ConfigurationManager.AppSettings["ReportWL"].ToString(),
                                     Request.QueryString["PID"],
                                     "");
                if (Request.QueryString["preview"] == "2")
                {
                    txtReportName.Value = "WL Graph";
                }
                else
                {
                    Response.Redirect("WLGraph/WLGraphPage.aspx?PID=" + Request.QueryString["PID"]);
                }
                break;

            case "INVG" :
                Response.Redirect("InvGraph/InvGraphFullPage.aspx?PID=" + Request.QueryString["PID"]);
                break;

            case "GCOM":
                gClass.SaveUserLogFile(intUserPracticeCode.ToString(),// Request.Cookies["UserPracticeCode"].Value, 
                                        Request.Cookies["Logon_UserName"].Value,
                                        Request.Url.Host,
                                        "Group Comorbidities Report", 3, "Browse", "PatientCode",
                                        intPatientID.ToString());// Request.Cookies["PatientID"].Value);
                txtReportName.Value = "Group Comorbidities";
                strXSLTFileName = @"GroupComorbidity/en_MenuComorbidityXSLTFile.xsl";
                GroupComorbidity_BuildReport(gClass);
                break;

            case "OD" : // Operation Details
                txtReportName.Value = "Operation Details Report";
                break;

            case "SB": // Super Bill
                txtReportName.Value = "Super Bill";
                break;

            case "EMR": //Follow Up Assessment
                txtReportName.Value = "Medical Report";
                break;

            case "OPERATIONDETAILS":
                txtReportName.Value = "Operation Details";
                break;

            case "PATIENTPROGRESS":
                txtReportName.Value = "Patient Progress";
                break;

            case "ACS": //Follow Up Assessment
                txtReportName.Value = "ACS Form";
                break;

            case "PATIENTCONTACT":
                txtReportName.Value = "Patient Contact";
                break;

            case "OREG1": //Operation Registry
                Response.SetCookie(new HttpCookie("PatientID", Request.QueryString["PatID"]));
                txtReportName.Value = "Operation Registry";
                break;

            case "OREG2": //Operation Registry Followup
                Response.SetCookie(new HttpCookie("PatientID", Request.QueryString["PatID"]));
                txtReportName.Value = "Operation Registry Followup";
                break;

            case "REPPATIENTLIST": //Patient List Report
                txtReportName.Value = "Patient List";
                break;

            case "REPOPERATIONLIST": //Operation List Report
                txtReportName.Value = "Operation List";
                break;

            case "REPVISITLIST": //Visit List Report
                txtReportName.Value = "Visit List";
                break;

            case "BSRREPORT": //BSR Report
                txtReportName.Value = "BSR Report";
                break;
        }
        return;
    }
    #endregion 

    #region protected void btnBuildReport_OnClick(object sender, EventArgs e)
    protected void btnBuildReport_OnClick(object sender, EventArgs e)
    {
        switch (txtReportCode.Value.ToUpper())
        {
            case "FUA": //Follow Up Assessment
                gClass.SaveUserLogFile(intUserPracticeCode.ToString(), Request.Cookies["Logon_UserName"].Value,
                                        Request.Url.Host, "Followup Assessment Report", 3, "Browse", "PatientCode", intPatientID.ToString());
                try { FollowUpAssessment_BuildReport(Request.QueryString["Param"].ToUpper());

                gClass.SaveActionLog(gClass.OrganizationCode,
                                     Request.Cookies["UserPracticeCode"].Value,
                                     Request.Url.Host,
                                     System.Configuration.ConfigurationManager.AppSettings["VisitPage"].ToString(),
                                     System.Configuration.ConfigurationManager.AppSettings["LogPrint"].ToString(),
                                     "Load " + System.Configuration.ConfigurationManager.AppSettings["ReportFUA"].ToString(),
                                     Request.Cookies["PatientID"].Value,
                                     "");
                }
                catch (Exception err)
                {
                    gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value,
                                        "Visit Form", "Visit Reports - Follow Up ", err.ToString());
                }
                break;

            case "RDL": //Ref. Doctor Letter
                gClass.SaveUserLogFile(intUserPracticeCode.ToString(), Request.Cookies["Logon_UserName"].Value, Request.Url.Host,
                                        "Ref. Doctor Letter Report", 3, "Browse", "PatientCode", intPatientID.ToString());
                try
                {
                    gClass.SaveActionLog(gClass.OrganizationCode,
                                         Request.Cookies["UserPracticeCode"].Value,
                                         Request.Url.Host,
                                         System.Configuration.ConfigurationManager.AppSettings["VisitPage"].ToString(),
                                         System.Configuration.ConfigurationManager.AppSettings["LogPrint"].ToString(),
                                         "Load " + System.Configuration.ConfigurationManager.AppSettings["ReportRDL"].ToString(),
                                         Request.Cookies["PatientID"].Value,
                                         "");

                    RefDoctorLetter_BuildReport(Request.QueryString["Param"].ToUpper());
                }
                catch (Exception err)
                {
                    gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value,
                                        "Visit Form", "Visit Reports - Letter to Doctors", err.ToString());
                }
                break;

            case "COMPSUM": // Complication Summary 1
                gClass.SaveUserLogFile(intUserPracticeCode.ToString(), Request.Cookies["Logon_UserName"].Value,
                                        Request.Url.Host, "Group Report - Complication Summary 1", 3, "Browse", "", "");

                gClass.SaveActionLog(gClass.OrganizationCode,
                                     Request.Cookies["UserPracticeCode"].Value,
                                     Request.Url.Host,
                                     System.Configuration.ConfigurationManager.AppSettings["ReportComplicationSummary"].ToString(),
                                     System.Configuration.ConfigurationManager.AppSettings["LogPrint"].ToString(),
                                     "Load " + System.Configuration.ConfigurationManager.AppSettings["ReportComplicationSummary"].ToString(),
                                     "",
                                     "");
                
                try { ComplicationSummary_BuildReport(); }
                catch (Exception err)
                {
                    gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value,
                                        "Group Reports", "Complication Summary - ComplicationSummary_BuildReport() function", err.ToString());
                }
                break;

            case "COMPSUMBYPATIENT": // Complication Summary 2
                gClass.SaveUserLogFile(intUserPracticeCode.ToString(), Request.Cookies["Logon_UserName"].Value, Request.Url.Host,
                                        "Group Report - Complication  by Patient", 3, "Browse", "", "");

                gClass.SaveActionLog(gClass.OrganizationCode,
                     Request.Cookies["UserPracticeCode"].Value,
                     Request.Url.Host,
                     System.Configuration.ConfigurationManager.AppSettings["ReportComplicationSummaryByPatient"].ToString(),
                     System.Configuration.ConfigurationManager.AppSettings["LogPrint"].ToString(),
                     "Load " + System.Configuration.ConfigurationManager.AppSettings["ReportComplicationSummaryByPatient"].ToString(),
                     "",
                     "");

                try { ComplicationSummaryByPatient_BuildReport(); }
                catch (Exception err)
                {
                    gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value,
                                        "Group Reports", "Complication  by Patient - ComplicationSummaryByPatient_BuildReport function", err.ToString());
                }
                break;

            case "OPERATIONLOS":
                gClass.SaveUserLogFile(intUserPracticeCode.ToString(), Request.Cookies["Logon_UserName"].Value, Request.Url.Host,
                                        "Group Report - Operation duration and LOS", 3, "Browse", "", "");
                try { OperationDurationLOS_BuildReport(); }
                catch (Exception err)
                {
                    gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value,
                                        "Group Reports", "Operation duration and LOS - OperationDurationLOS_BuildReport function", err.ToString());
                }
                break;

            case "PATIENTLIST":
                gClass.SaveUserLogFile(intUserPracticeCode.ToString(), Request.Cookies["Logon_UserName"].Value, Request.Url.Host,
                                        "Group Report - Patient List with Last Visit Date", 3, "Browse", "", "");

                gClass.SaveActionLog(gClass.OrganizationCode,
                                     Request.Cookies["UserPracticeCode"].Value,
                                     Request.Url.Host,
                                     System.Configuration.ConfigurationManager.AppSettings["ReportPatientList"].ToString(),
                                     System.Configuration.ConfigurationManager.AppSettings["LogPrint"].ToString(),
                                     "Load " + System.Configuration.ConfigurationManager.AppSettings["ReportPatientList"].ToString(),
                                     "",
                                     "");

                try { PatientList_BuildReport(); }
                catch (Exception err)
                {
                    gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value,
                                        "Group Reports", "Patient List with Last Visit Date - PatientList_BuildReport function", err.ToString());
                }
                break;

            case "COEREPORT":
                gClass.SaveUserLogFile(intUserPracticeCode.ToString(), Request.Cookies["Logon_UserName"].Value, Request.Url.Host,
                                        "Group Report - Patient list with complications", 3, "Browse", "", "");
                try { PatientListWithComplications_BuildReport(); }
                catch (Exception err)
                {
                    gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value,
                                        "Group Reports", "Patient list with complications - PatientListWithComplications_BuildReport function", err.ToString());
                }
                break;

            case "SUMMARYBYQUARTER":
                gClass.SaveUserLogFile(intUserPracticeCode.ToString(), Request.Cookies["Logon_UserName"].Value, Request.Url.Host,
                                        "Group Report - Weight Loss", 3, "Browse", "", "");

                gClass.SaveActionLog(gClass.OrganizationCode,
                                     Request.Cookies["UserPracticeCode"].Value,
                                     Request.Url.Host,
                                     System.Configuration.ConfigurationManager.AppSettings["ReportWeightLoss"].ToString(),
                                     System.Configuration.ConfigurationManager.AppSettings["LogPrint"].ToString(),
                                     "Load " + System.Configuration.ConfigurationManager.AppSettings["ReportWeightLoss"].ToString(),
                                     "",
                                     "");

                try { SummaryByQuarter_BuildReport(); }
                catch (Exception err)
                {

                    gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value,
                                        "Group Reports", "Weight Loss - SummaryByQuarter_BuildReport function", err.ToString());
                }
                break;

            case "SUMMARYONLY":
                gClass.SaveUserLogFile(intUserPracticeCode.ToString(), Request.Cookies["Logon_UserName"].Value,
                                        Request.Url.Host, "Group Report - Summary Only", 3, "Browse", "", "");
                try { SummaryByQuarter_BuildReport(); }
                catch (Exception err)
                {
                    gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value,
                                        "Group Reports", "Summary Only - SummaryByQuarter_BuildReport function", err.ToString());
                }
                break;

            case "BMIEWLGRAPH":
                gClass.SaveUserLogFile(intUserPracticeCode.ToString(), Request.Cookies["Logon_UserName"].Value, Request.Url.Host,
                                        "Group Report - BMI and %EWL Graph", 3, "Browse", "", "");

                gClass.SaveActionLog(gClass.OrganizationCode,
                                     Request.Cookies["UserPracticeCode"].Value,
                                     Request.Url.Host,
                                     System.Configuration.ConfigurationManager.AppSettings["ReportBMIEWL"].ToString(),
                                     System.Configuration.ConfigurationManager.AppSettings["LogPrint"].ToString(),
                                     "Load " + System.Configuration.ConfigurationManager.AppSettings["ReportBMIEWL"].ToString(),
                                     "",
                                     "");

                try { BMIEWLGraph_BuildReport(); }
                catch (Exception err)
                {
                    gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value,
                                        "Group Reports", "BMI and %EWL Graph - BMIEWLGraph_BuildReport function", err.ToString());
                }
                break;

            case "BLC":
                gClass.SaveUserLogFile(intUserPracticeCode.ToString(), Request.Cookies["Logon_UserName"].Value,
                                        Request.Url.Host, "Baseline Comorbidities Report", 3, "Browse", "PatientCode", intPatientID.ToString());
                strXSLTFileName = @"BaseLineComorbidities/en_BaseLineComorbiditiesXSLTFile.xsl";
                BaseLineComorbidities_BuildReport(gClass);
                break;

            case "CMCP":
                gClass.SaveUserLogFile(intUserPracticeCode.ToString(), // Request.Cookies["UserPracticeCode"].Value,
                                        Request.Cookies["Logon_UserName"].Value,
                                        Request.Url.Host,
                                        "Comorbidities and Complications Report", 3, "Browse", "PatientCode",
                                        intPatientID.ToString()); //Request.Cookies["PatientID"].Value);
                strXSLTFileName = @"ComorbidityFUAssessment/en_ComorbidityFUAssessmentXSLTFile.xsl";
                ComorbidityFUAssessment_BuildReport(gClass);
                break;

            case "EWLG":
                try { EWLG_BuildReport(); }
                catch (Exception err)
                {
                    gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value,
                                        "Group Reports", "%EWL Graph - EWLG_BuildReport function", err.ToString());
                }
                break;

            case "IEWLG":
                try { IEWLG_BuildReport(); }
                catch (Exception err)
                {
                    gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value,
                                        "Group Reports", "IEWLG Graph - IEWLG_BuildReport function", err.ToString());
                }
                break;

            case "WLG":
                try { WLG_BuildReport(); }
                catch (Exception err)
                {
                    gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value,
                                        "Group Reports", "WL Graph - WLG_BuildReport function", err.ToString());
                }
                break;

            case "INVG":
                Response.Redirect("InvGraph/InvGraphFullPage.aspx?PID=" + Request.QueryString["PID"]);
                break;

            case "GCOM":
                gClass.SaveUserLogFile(intUserPracticeCode.ToString(),// Request.Cookies["UserPracticeCode"].Value, 
                                        Request.Cookies["Logon_UserName"].Value,
                                        Request.Url.Host,
                                        "Group Comorbidities Report", 3, "Browse", "PatientCode",
                                        intPatientID.ToString());// Request.Cookies["PatientID"].Value);
                strXSLTFileName = @"GroupComorbidity/en_MenuComorbidityXSLTFile.xsl";
                GroupComorbidity_BuildReport(gClass);
                break;

            case "OD": // Operation Details
                gClass.SaveUserLogFile(intUserPracticeCode.ToString(),// Request.Cookies["UserPracticeCode"].Value,
                                        Request.Cookies["Logon_UserName"].Value,
                                        Request.Url.Host,
                                        "Operation Details Report", 3, "Browse", "PatientCode",
                                        intPatientID.ToString());// Request.Cookies["PatientID"].Value);
                OperationDetails_BuildReport(gClass, Request.QueryString["Param"].ToUpper());
                break;

            case "SB":
                gClass.SaveUserLogFile(intUserPracticeCode.ToString(), Request.Cookies["Logon_UserName"].Value, Request.Url.Host,
                                        "Visit Report - Super Bill", 3, "Browse", "", "");
                try
                {
                    SB_BuildReport(Request.QueryString["Param"].ToUpper());
                }
                catch (Exception err)
                {
                    gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value,
                                        "Visit Report","Super Bill", err.ToString());
                }
                break;

            case "EMR":
                gClass.SaveUserLogFile(intUserPracticeCode.ToString(), Request.Cookies["Logon_UserName"].Value,
                                        Request.Url.Host, "Group Report - EMR", 3, "Browse", "", "");

                gClass.SaveActionLog(gClass.OrganizationCode,
                                     Request.Cookies["UserPracticeCode"].Value,
                                     Request.Url.Host,
                                     System.Configuration.ConfigurationManager.AppSettings["PatientPage"].ToString(),
                                     System.Configuration.ConfigurationManager.AppSettings["LogPrint"].ToString(),
                                     "Load " + System.Configuration.ConfigurationManager.AppSettings["ReportEMR"].ToString(),
                                     Request.Cookies["PatientID"].Value,
                                     "");

                try { EMR_BuildReport(); }
                catch (Exception err)
                {
                    gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value,
                                        "EMR Report", "EMR - EMR_BuildReport function", err.ToString());
                }
                break;

            case "ACS":
                gClass.SaveUserLogFile(intUserPracticeCode.ToString(), Request.Cookies["Logon_UserName"].Value,
                                        Request.Url.Host, "Group Report - ACS", 3, "Browse", "", "");
                try { ACS_BuildReport(); }
                catch (Exception err)
                {
                    gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value,
                                        "ACS Form", "ACS - ACS_BuildReport function", err.ToString());
                }
                break;

            case "OPERATIONDETAILS":
                gClass.SaveUserLogFile(intUserPracticeCode.ToString(), Request.Cookies["Logon_UserName"].Value, Request.Url.Host,
                                        "Group Report - Operation Details", 3, "Browse", "", "");

                gClass.SaveActionLog(gClass.OrganizationCode,
                                     Request.Cookies["UserPracticeCode"].Value,
                                     Request.Url.Host,
                                     System.Configuration.ConfigurationManager.AppSettings["ReportOperationDetail"].ToString(),
                                     System.Configuration.ConfigurationManager.AppSettings["LogPrint"].ToString(),
                                     "Load " + System.Configuration.ConfigurationManager.AppSettings["ReportOperationDetail"].ToString(),
                                     "",
                                     "");

                try { OperationDetailList_BuildReport(); }
                catch (Exception err)
                {
                    gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value,
                                        "Group Reports", "Operation Detail List - OperationDetailList_BuildReport function", err.ToString());
                }
                break;

            case "PATIENTPROGRESS":
                gClass.SaveUserLogFile(intUserPracticeCode.ToString(), Request.Cookies["Logon_UserName"].Value, Request.Url.Host,
                                        "Group Report - Patient Progress", 3, "Browse", "", "");

                gClass.SaveActionLog(gClass.OrganizationCode,
                                     Request.Cookies["UserPracticeCode"].Value,
                                     Request.Url.Host,
                                     System.Configuration.ConfigurationManager.AppSettings["ReportPatientProgress"].ToString(),
                                     System.Configuration.ConfigurationManager.AppSettings["LogPrint"].ToString(),
                                     "Load " + System.Configuration.ConfigurationManager.AppSettings["ReportPatientProgress"].ToString(),
                                     "",
                                     "");

                try { PatientProgress_BuildReport(); }
                catch (Exception err)
                {
                    gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value,
                                        "Group Reports", "Patient Progress - PatientProgress_BuildReport function", err.ToString());
                }
                break;

            case "PATIENTCONTACT":
                gClass.SaveUserLogFile(intUserPracticeCode.ToString(), Request.Cookies["Logon_UserName"].Value, Request.Url.Host,
                                        "Group Report - Patient Contact", 3, "Browse", "", "");

                gClass.SaveActionLog(gClass.OrganizationCode,
                                     Request.Cookies["UserPracticeCode"].Value,
                                     Request.Url.Host,
                                     System.Configuration.ConfigurationManager.AppSettings["ReportPatientContact"].ToString(),
                                     System.Configuration.ConfigurationManager.AppSettings["LogPrint"].ToString(),
                                     "Load " + System.Configuration.ConfigurationManager.AppSettings["ReportPatientContact"].ToString(),
                                     "",
                                     "");

                try { PatientContact_BuildReport(); }
                catch (Exception err)
                {
                    gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value,
                                        "Group Reports", "Patient Contact - PatientContact_BuildReport function", err.ToString());
                }
                break;

            case "OREG1":
                gClass.SaveUserLogFile(intUserPracticeCode.ToString(), Request.Cookies["Logon_UserName"].Value,
                                        Request.Url.Host, "Group Report - Operation Registry", 3, "Browse", "", "");

                gClass.SaveActionLog(gClass.OrganizationCode,
                                     Request.Cookies["UserPracticeCode"].Value,
                                     Request.Url.Host,
                                     System.Configuration.ConfigurationManager.AppSettings["PatientPage"].ToString(),
                                     System.Configuration.ConfigurationManager.AppSettings["LogPrint"].ToString(),
                                     "Load " + System.Configuration.ConfigurationManager.AppSettings["ReportOREG1"].ToString(),
                                     Request.Cookies["PatientID"].Value,
                                     "");

                try { OREG1_BuildReport(); }
                catch (Exception err)
                {
                    gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value,
                                        "Operation Registry Report", "OREG1 - OREG1_BuildReport function", err.ToString());
                }
                break;

            case "OREG2":
                gClass.SaveUserLogFile(intUserPracticeCode.ToString(), Request.Cookies["Logon_UserName"].Value,
                                        Request.Url.Host, "Group Report - Operation Registry", 3, "Browse", "", "");

                gClass.SaveActionLog(gClass.OrganizationCode,
                                     Request.Cookies["UserPracticeCode"].Value,
                                     Request.Url.Host,
                                     System.Configuration.ConfigurationManager.AppSettings["PatientPage"].ToString(),
                                     System.Configuration.ConfigurationManager.AppSettings["LogPrint"].ToString(),
                                     "Load " + System.Configuration.ConfigurationManager.AppSettings["ReportOREG2"].ToString(),
                                     Request.Cookies["PatientID"].Value,
                                     "");

                try { OREG2_BuildReport(); }
                catch (Exception err)
                {
                    gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value,
                                        "Operation Registry Report", "OREG2 - OREG2_BuildReport function", err.ToString());
                }
                break;

            case "REPPATIENTLIST":
                gClass.SaveUserLogFile(intUserPracticeCode.ToString(), Request.Cookies["Logon_UserName"].Value, Request.Url.Host,
                                        "Export - Patient List", 3, "Browse", "", "");

                gClass.SaveActionLog(gClass.OrganizationCode,
                                     Request.Cookies["UserPracticeCode"].Value,
                                     Request.Url.Host,
                                     System.Configuration.ConfigurationManager.AppSettings["ReportPatientList"].ToString(),
                                     System.Configuration.ConfigurationManager.AppSettings["LogPrint"].ToString(),
                                     "Load " + System.Configuration.ConfigurationManager.AppSettings["ReportPatientList"].ToString(),
                                     "",
                                     "");

                try { RepPatientList_BuildReport(); }
                catch (Exception err)
                {
                    gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value,
                                        "Group Reports", "Patient List - RepPatientList_BuildReport function", err.ToString());
                }
                break;


            case "REPOPERATIONLIST":
                gClass.SaveUserLogFile(intUserPracticeCode.ToString(), Request.Cookies["Logon_UserName"].Value, Request.Url.Host,
                                        "Export - Operation List", 3, "Browse", "", "");

                gClass.SaveActionLog(gClass.OrganizationCode,
                                     Request.Cookies["UserPracticeCode"].Value,
                                     Request.Url.Host,
                                     System.Configuration.ConfigurationManager.AppSettings["ReportOperationList"].ToString(),
                                     System.Configuration.ConfigurationManager.AppSettings["LogPrint"].ToString(),
                                     "Load " + System.Configuration.ConfigurationManager.AppSettings["ReportOperationList"].ToString(),
                                     "",
                                     "");

                try { RepOperationList_BuildReport(); }
                catch (Exception err)
                {
                    gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value,
                                        "Group Reports", "Operation List - RepOperationList_BuildReport function", err.ToString());
                }
                break;

            case "REPVISITLIST":
                gClass.SaveUserLogFile(intUserPracticeCode.ToString(), Request.Cookies["Logon_UserName"].Value, Request.Url.Host,
                                        "Export - Visit List", 3, "Browse", "", "");

                gClass.SaveActionLog(gClass.OrganizationCode,
                                     Request.Cookies["UserPracticeCode"].Value,
                                     Request.Url.Host,
                                     System.Configuration.ConfigurationManager.AppSettings["ReportVisitList"].ToString(),
                                     System.Configuration.ConfigurationManager.AppSettings["LogPrint"].ToString(),
                                     "Load " + System.Configuration.ConfigurationManager.AppSettings["ReportVisitList"].ToString(),
                                     "",
                                     "");

                try { RepVisitList_BuildReport(); }
                catch (Exception err)
                {
                    gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value,
                                        "Group Reports", "Visit List - RepVisitList_BuildReport function", err.ToString());
                }
                break;

            case "BSRREPORT":
                gClass.SaveUserLogFile(intUserPracticeCode.ToString(), Request.Cookies["Logon_UserName"].Value, Request.Url.Host,
                                        "Group Report - BSR Report", 3, "Browse", "", "");

                gClass.SaveActionLog(gClass.OrganizationCode,
                                     Request.Cookies["UserPracticeCode"].Value,
                                     Request.Url.Host,
                                     System.Configuration.ConfigurationManager.AppSettings["ReportBSR"].ToString(),
                                     System.Configuration.ConfigurationManager.AppSettings["LogPrint"].ToString(),
                                     "Load " + System.Configuration.ConfigurationManager.AppSettings["ReportBSR"].ToString(),
                                     "",
                                     "");

                try { BSRReport_BuildReport(); }
                catch (Exception err)
                {
                    gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value,
                                        "Group Reports", "BSR Report - BSRReport_BuildReport function", err.ToString());
                }
                break;
        }
        //Response.End();
        return;
    }
    #endregion 

    #region private void FollowUpAssessment_BuildReport
    private void FollowUpAssessment_BuildReport(string strParam)
    {
        DataSet dsReport;
        DataSet dsLogo = new DataSet();
        
        DataColumn      dcTemp;
        SqlCommand      cmdSelectPatientData = new SqlCommand(),
                        cmdSelectPatientVisit = new SqlCommand();
        string          strReport = "";
        bool            includeComment = false;
        strXSLTFileName = @"FollowUpAssessment/en_FollowUpAssessmentXSLTFile.xsl";

        string fileName = Request.Cookies["PatientID"].Value + "-" + gClass.OrganizationCode;
        string saveFilePath = Server.MapPath(".\\.\\..\\temp\\") + fileName + ".doc";
        string openFilePath = "..//temp//" + fileName + ".doc";

        gClass.MakeStoreProcedureName(ref cmdSelectPatientData, "sp_Rep_FollowUpAssessment", true);
        gClass.MakeStoreProcedureName(ref cmdSelectPatientVisit, "sp_ConsultFU1_ProgressNotes_LoadData", true);

        cmdSelectPatientData.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = intOrganizationCode;
        cmdSelectPatientData.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = intUserPracticeCode;
        cmdSelectPatientData.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["PID"]);
        cmdSelectPatientData.Parameters.Add("@ImperialFlag", SqlDbType.Bit).Value = Request.Cookies["Imperial"].Value.Equals("True");

        cmdSelectPatientVisit.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = intOrganizationCode;
        cmdSelectPatientVisit.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = intUserPracticeCode;
        cmdSelectPatientVisit.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["PID"]);
        cmdSelectPatientVisit.Parameters.Add("@ImperialFlag", SqlDbType.Bit).Value = Request.Cookies["Imperial"].Value.Equals("True");
        cmdSelectPatientVisit.Parameters.Add("@ComorbidityFlag", SqlDbType.Bit).Value = false;
        cmdSelectPatientVisit.Parameters.Add("@ReportFlag", SqlDbType.Bit).Value = true;
        cmdSelectPatientVisit.Parameters.Add("@VisitWeeksFlag", SqlDbType.VarChar).Value = Request.Cookies["VisitWeeksFlag"].Value;
        
        includeComment = strParam[4].ToString()=="1"?true:false;
        cmdSelectPatientVisit.Parameters.Add("@IncludeComment", SqlDbType.Bit).Value = includeComment;

        String logoID;
        String LogoPath = "";
        try
        {
            logoID = strParam.Substring(5, 3);
            if (logoID != "000")
            {
                dsLogo = LoadLogoByID(Convert.ToInt32(logoID));
                LogoPath = dsLogo.Tables[0].Rows[0]["LogoPath"].ToString();
                LogoPath = LogoPath.Substring(IndexOfOccurence(LogoPath, "/", 2));
            }
        }
        catch (Exception err) {}





        DataSet dsVisitTemp = gClass.FetchData(cmdSelectPatientVisit, "tblPatientVisit");

        //get comorbidity value
        //loop for each comorbidity
        //if not empty,add to array
        // for each visit, check the comorbidity value that baseline is valid
        DataView dvBaseline = LoadComorbidityData(0); // we pass 0 to load Baseline Comorbidity data
        List<String> baselineComorbidityAvailable = new List<String>();
        String tempSelectField = "";
        String strRankValue = "";

        if (dvBaseline != null && dvBaseline.Count > 0)
        {
            //loop for each comorbidity
            for (int Xh = 0; Xh < comorbidityArr.Length / 3; Xh++)
            {
                tempSelectField = comorbidityArr[Xh, 1];

                //currently only for bold
                if (comorbidityArr[Xh, 2] != "acs" && (Request.Cookies["SubmitData"].Value.IndexOf("bold") >= 0 || Request.Cookies["SubmitData"].Value.IndexOf("submitbold") >= 0))
                {
                    strRankValue = dvBaseline[0][tempSelectField + "_Rank"].ToString().Trim();
                    if (strRankValue.Equals(String.Empty) || strRankValue == null || strRankValue.Equals("0"))
                    {
                        //dont add to list
                    }
                    else
                    {
                        baselineComorbidityAvailable.Add(tempSelectField);
                    }
                }
            }
        }


        //get comorbididy value
        DataView dvVisitComorbidity;
        Int32 tempConsultID = 0;
        string tempVisitComorbidity = "";
        string comorbidityValue = "";
        string comorbidityQuestionValue = "";
        string comorbidityResult = "";

        dcTemp = new DataColumn();
        dcTemp.DataType = Type.GetType("System.String");
        dcTemp.DefaultValue = "";
        dcTemp.ColumnName = "comorbidityNote";
        dcTemp.Caption = "comorbidityNote";
        dsVisitTemp.Tables[0].Columns.Add(dcTemp);

        for (int Xh = 0; Xh < dsVisitTemp.Tables[0].Rows.Count; Xh++)
        {
            comorbidityResult = GetComorbidityResult(Convert.ToInt32(dsVisitTemp.Tables[0].Rows[Xh]["ConsultID"].ToString().Trim()), dsVisitTemp.Tables[0].Rows[Xh]["ComorbidityVisit"].ToString().Trim(), baselineComorbidityAvailable, "<br>");
            if (comorbidityResult.Trim() != "")
            {
                try { dsVisitTemp.Tables[0].Rows[Xh]["comorbidityNote"] = comorbidityResult; }
                catch { }
            }
        }
        //---------------------------------------------------------------------------------

        dsReport = gClass.FetchData(cmdSelectPatientData, "tblPatientData");
        dsReport.Tables.Add(dsVisitTemp.Tables[0].Copy());
        gClass.CalculateWeightData(ref dsReport, "tblPatientVisit", Request.Cookies["Imperial"].Value.Equals("True"));
        // Add Param to Tables: 1)Patient Photo and Patient Note into tblPatientData 2)Progress Note into PatientVisit
        dcTemp = new DataColumn();
        dcTemp.Caption = "IncludePhoto";
        dcTemp.ColumnName = "IncludePhoto";
        dcTemp.DataType = Type.GetType("System.String");
        dcTemp.DefaultValue = strParam[0];
        dsReport.Tables["tblPatientData"].Columns.Add(dcTemp);

        dcTemp = new DataColumn();
        dcTemp.Caption = "IncludePatientNote";
        dcTemp.ColumnName = "IncludePatientNote";
        dcTemp.DataType = Type.GetType("System.String");
        dcTemp.DefaultValue = strParam[2];
        dsReport.Tables["tblPatientData"].Columns.Add(dcTemp);

        dcTemp = new DataColumn();
        dcTemp.Caption = "IncludeProgressNote";
        dcTemp.ColumnName = "IncludeProgressNote";
        dcTemp.DataType = Type.GetType("System.String");
        dcTemp.DefaultValue = strParam[1];
        dsReport.Tables["tblPatientVisit"].Columns.Add(dcTemp);

        dcTemp = new DataColumn();
        dcTemp.Caption = "Last10Visits";
        dcTemp.ColumnName = "Last10Visits";
        dcTemp.DataType = Type.GetType("System.String");
        dcTemp.DefaultValue = strParam[3];
        dsReport.Tables["tblPatientVisit"].Columns.Add(dcTemp);

        dcTemp = new DataColumn();
        dcTemp.Caption = "LogoPath";
        dcTemp.ColumnName = "LogoPath";
        dcTemp.DataType = Type.GetType("System.String");
        dcTemp.DefaultValue = LogoPath;
        dsReport.Tables["tblPatientData"].Columns.Add(dcTemp);

        dsReport.Tables.Add(gClass.FetchCaptions("en_FollowUpAssessmentXSLTFile", Request.Cookies["CultureInfo"].Value).Tables[0].Copy());
        dsReport.AcceptChanges();

        string url = "";
        try
        {
            if (dsReport.Tables.Count > 0)
            {
                strReport = gClass.ShowSchema(dsReport, Server.MapPath(strXSLTFileName));



                if (Request.QueryString["Preview"] == "4")
                {
                    Response.AppendHeader("Content-Type", "application/msword");
                    Response.AppendHeader("Content-disposition", "attachment; filename=FollowUpReport-" + fileName + ".doc");

                    url = HttpContext.Current.Request.Url.AbsoluteUri;
                    url = url.Substring(0, url.IndexOf("?"));
                    strReport = strReport.Replace("/Photos/Logo", url + "/../../Photos/Logo");

                    Response.Write(strReport);
                }
                else
                {
                    url = HttpContext.Current.Request.Url.AbsoluteUri;
                    url = url.Substring(0, url.IndexOf("?"));
                    strReport = strReport.Replace("/Photos/Logo", url + "/../../Photos/Logo");
                    tcXML.InnerHtml = strReport;
                }
            }
        }
        catch (Exception err) { Response.Write(err.ToString()); }
        return;
    }
    #endregion 

    #region private String GetComorbidityResult(Int32 intConsultantID, String ComorbidityCond, List<String> baselineComorbidityAvailable, String CarrReturn)
    private String GetComorbidityResult(Int32 intConsultantID, String ComorbidityCond, List<String> baselineComorbidityAvailable, String CarrReturn)
    {
        //get comorbidity value
        String comorbidityResult = "";
        Int32 tempConsultID = 0;
        DataView dvVisitComorbidity;
        String tempVisitComorbidity = "";
        String comorbidityValue = "";
        String comorbidityQuestionValue = "";

        if (ComorbidityCond == "True")
        {
            comorbidityResult = "";
            dvVisitComorbidity = LoadComorbidityData(intConsultantID);
            //for each baseline
            foreach (string tempBaselineComorbidity in baselineComorbidityAvailable)
            {
                tempVisitComorbidity = "";
                comorbidityValue = "";
                comorbidityQuestionValue = "";

                tempVisitComorbidity = dvVisitComorbidity[0][tempBaselineComorbidity].ToString().Trim();
                comorbidityValue = getDescription(tempVisitComorbidity, "com");
                comorbidityQuestionValue = getDescription(tempVisitComorbidity, "comQuest");
                comorbidityResult = comorbidityResult + "- " + comorbidityQuestionValue + " : " + comorbidityValue + CarrReturn;
            }
        }
        return comorbidityResult;
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

        return dvTemp;
    }
    #endregion

    #region private void RefDoctorLetter_BuildReport(string strParam)
    private void RefDoctorLetter_BuildReport(string strParam)
    {
        SqlCommand cmdSelect = new SqlCommand(),
                        cmdSelectPatientVisit = new SqlCommand(),
                        cmdSelectRefDoctor = new SqlCommand();
        DataSet dsReport = new DataSet("dsSchema");
        DataSet dsLogo = new DataSet();
        DataColumn dcDate = new DataColumn("CurrentDate", Type.GetType("System.String")), dcTemp;
        string strFields = "", strReport = "";
        string[] strRefDrId = { "", "", "" };
        List<DataTable> dtList = new List<DataTable>();

        if (strParam.IndexOf(";") > -1)
        {
            int Xh = -1;
            strFields = strParam.Substring(0, strParam.IndexOf(";"));
            strParam = strParam.Substring(strParam.IndexOf(";") + 1);

            while (strParam.IndexOf(";") > -1)
            {
                strRefDrId[++Xh] = strParam.Substring(0, strParam.IndexOf(";"));
                strParam = strParam.Substring(strParam.IndexOf(";") + 1);
            }
            if (strParam.Length > 0) strRefDrId[++Xh] = strParam;
        }
        else
            strFields = strParam;

        String surgeonID = strFields.Substring(5, 3);

        String logoID;
        String LogoPath = "";
        try
        {
            logoID = strFields.Substring(8, 3);
            if (logoID != "000")
            {
                dsLogo = LoadLogoByID(Convert.ToInt32(logoID));
                LogoPath = dsLogo.Tables[0].Rows[0]["LogoPath"].ToString();
                LogoPath = LogoPath.Substring(IndexOfOccurence(LogoPath, "/", 2));
            }
        }
        catch (Exception err) { }

        dcDate.DefaultValue = DateTime.Now.ToLongDateString();

        bool includeComment = false;

        //AIGB setting
        //AIGB should use different template
        //if (url.ToUpper().IndexOf("AIGB") >= 0)
        //    strXSLTFileName = @"RefDoctorLetter/en_AIGBRefDoctorLetterXSLTFile.xsl";
        //else
        strXSLTFileName = @"RefDoctorLetter/en_RefDoctorLetterXSLTFile.xsl";

        //string fileName = Request.Cookies["PatientID"].Value + "-" + gClass.OrganizationCode;
        //string saveFilePath = Server.MapPath(".\\.\\..\\temp\\") + fileName + ".doc";
        //string openFilePath = "..//temp//" + fileName + ".doc";

        //need to put delay as we are still waiting for the followupnotes to be saved
        System.Threading.Thread.Sleep(2000);
        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_Rep_RefDoctorLetter", true);
        gClass.MakeStoreProcedureName(ref cmdSelectPatientVisit, "sp_ConsultFU1_ProgressNotes_LoadData", true);
        gClass.MakeStoreProcedureName(ref cmdSelectRefDoctor, "sp_RefDoctors_SelectRefDoctorByRefDoctorID", true);

        cmdSelect.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["PID"]);
        cmdSelect.Parameters.Add("@ImperialFlag", SqlDbType.Bit).Value = Request.Cookies["Imperial"].Value.Equals("True");
        cmdSelect.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = intUserPracticeCode;
        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = intOrganizationCode;
        cmdSelect.Parameters.Add("@SurgeonID", SqlDbType.Int).Value = Convert.ToInt32(surgeonID);
        dsReport.Tables.Add(gClass.FetchData(cmdSelect, "tblReport").Tables[0].Copy());

        cmdSelectPatientVisit.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = intOrganizationCode;
        cmdSelectPatientVisit.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = intUserPracticeCode;
        cmdSelectPatientVisit.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["PID"]);
        cmdSelectPatientVisit.Parameters.Add("@ImperialFlag", SqlDbType.Bit).Value = Request.Cookies["Imperial"].Value.Equals("True");
        cmdSelectPatientVisit.Parameters.Add("@ComorbidityFlag", SqlDbType.Bit).Value = false;
        cmdSelectPatientVisit.Parameters.Add("@ReportFlag", SqlDbType.Bit).Value = true;
        cmdSelectPatientVisit.Parameters.Add("@VisitWeeksFlag", SqlDbType.VarChar).Value = Request.Cookies["VisitWeeksFlag"].Value;
        includeComment = strFields[4].ToString() == "1" ? true : false;

        cmdSelectPatientVisit.Parameters.Add("@IncludeComment", SqlDbType.Bit).Value = includeComment;

        DataSet dsVisitTemp = gClass.FetchData(cmdSelectPatientVisit, "tblPatientVisit");



        //get comorbidity value
        //loop for each comorbidity
        //if not empty,add to array
        // for each visit, check the comorbidity value that baseline is valid
        DataView dvBaseline = LoadComorbidityData(0); // we pass 0 to load Baseline Comorbidity data
        List<String> baselineComorbidityAvailable = new List<String>();
        String tempSelectField = "";
        String strRankValue = "";
        
        if (dvBaseline != null && dvBaseline.Count > 0)
        {
            //loop for each comorbidity
            for (int Xh = 0; Xh < comorbidityArr.Length / 3; Xh++)
            {
                tempSelectField = comorbidityArr[Xh, 1];

                //currently only for bold
                if (comorbidityArr[Xh, 2] != "acs" && (Request.Cookies["SubmitData"].Value.IndexOf("bold") >= 0 || Request.Cookies["SubmitData"].Value.IndexOf("submitbold") >= 0))
                {
                    strRankValue = dvBaseline[0][tempSelectField + "_Rank"].ToString().Trim();
                    if (strRankValue.Equals(String.Empty) || strRankValue == null || strRankValue.Equals("0"))
                    {
                        //dont add to list
                    }
                    else
                    {
                        baselineComorbidityAvailable.Add(tempSelectField);
                    }
                }
            }
        }

        //get comorbididy value
        DataView dvVisitComorbidity;
        Int32 tempConsultID = 0;
        string tempVisitComorbidity = "";
        string comorbidityValue = "";
        string comorbidityQuestionValue = "";
        string comorbidityResult = "";

        dcTemp = new DataColumn();
        dcTemp.DataType = Type.GetType("System.String");
        dcTemp.DefaultValue = "";
        dcTemp.ColumnName = "comorbidityNote";
        dcTemp.Caption = "comorbidityNote";
        dsVisitTemp.Tables[0].Columns.Add(dcTemp);

        for (int Xh = 0; Xh < dsVisitTemp.Tables[0].Rows.Count; Xh++)
        {
            comorbidityResult = GetComorbidityResult(Convert.ToInt32(dsVisitTemp.Tables[0].Rows[Xh]["ConsultID"].ToString().Trim()), dsVisitTemp.Tables[0].Rows[Xh]["ComorbidityVisit"].ToString().Trim(), baselineComorbidityAvailable, "<br>");
            if(comorbidityResult.Trim() != "")
            {
                try { dsVisitTemp.Tables[0].Rows[Xh]["comorbidityNote"] = comorbidityResult; }
                catch { }
            }
        }
        //---------------------------------------------------------------------------------

        dsReport.Tables.Add(dsVisitTemp.Tables[0].Copy());

        // Add current date to tblReport
        dsReport.Tables[0].Columns.Add(dcDate);

        // Add Param to Tables: 1)Patient Photo and Patient Note into tblPatientData 2)Progress Note into PatientVisit
        dcTemp = new DataColumn();
        dcTemp.Caption = "CurrentVisitOnly";
        dcTemp.ColumnName = "CurrentVisitOnly";
        dcTemp.DataType = Type.GetType("System.String");
        dcTemp.DefaultValue = strFields[0];
        dsReport.Tables["tblReport"].Columns.Add(dcTemp);

        dcTemp = new DataColumn();
        dcTemp.Caption = "IncludeProgressNote";
        dcTemp.ColumnName = "IncludeProgressNote";
        dcTemp.DataType = Type.GetType("System.String");
        dcTemp.DefaultValue = strFields[1];
        dsReport.Tables["tblPatientVisit"].Columns.Add(dcTemp);

        dcTemp = new DataColumn();
        dcTemp.Caption = "IncludePatientNote";
        dcTemp.ColumnName = "IncludePatientNote";
        dcTemp.DataType = Type.GetType("System.String");
        dcTemp.DefaultValue = strFields[2];
        dsReport.Tables["tblReport"].Columns.Add(dcTemp);

        dcTemp = new DataColumn();
        dcTemp.Caption = "Last10Visits";
        dcTemp.ColumnName = "Last10Visits";
        dcTemp.DataType = Type.GetType("System.String");
        dcTemp.DefaultValue = strFields[3];
        dsReport.Tables["tblPatientVisit"].Columns.Add(dcTemp);
        
        dcTemp = new DataColumn();
        dcTemp.Caption = "LogoPath";
        dcTemp.ColumnName = "LogoPath";
        dcTemp.DataType = Type.GetType("System.String");
        dcTemp.DefaultValue = LogoPath;
        dsReport.Tables["tblReport"].Columns.Add(dcTemp);
        
        dsReport.Tables.Add(gClass.FetchCaptions("en_RefDoctorLetterXSLTFile",

        Request.Cookies["CultureInfo"].Value).Tables[0].Copy());
        gClass.CalculateWeightData(ref dsReport, "tblPatientVisit", Request.Cookies["Imperial"].Value.Equals("True"));

        cmdSelectRefDoctor.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = intOrganizationCode;
        cmdSelectRefDoctor.Parameters.Add("@RefDrId", SqlDbType.VarChar, 10);
        cmdSelectRefDoctor.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = intUserPracticeCode;
        dsReport.AcceptChanges();

        for (int Xh = 0; Xh < dsReport.Tables.Count; Xh++)
            dtList.Add(dsReport.Tables[Xh].Copy());
        dsReport.Dispose();


        //-------------------------------
        string imperialMode = (Request.Cookies["Imperial"].Value == "True") ? "1" : "0";

        Phrase phrase = new Phrase("");

        string reportFormat = "";

        if (Request.QueryString["Preview"] == "3")//pdf
            reportFormat = "pdf";
        else if (Request.QueryString["Preview"] == "4")//word document
            reportFormat = "doc";

        //if there is record
        string patientHeightMeasurment = (imperialMode == "1" ? "inches" : "cm");
        string patientWeightMeasurment = (imperialMode == "1" ? "lbs" : "kg");

        string fileName = "LetterToDoctor-"+Request.Cookies["PatientID"].Value + "-" + gClass.OrganizationCode;
        string saveFilePath = Server.MapPath(".\\.\\..\\temp\\") + fileName + "." + reportFormat;
        string openFilePath = "..//temp//" + fileName + "." + reportFormat;


        DataSet dsSystemDetail = new DataSet();

        dsSystemDetail = LoadSystemDetail();

        Int32 margin = 0;
        try { margin = Convert.ToInt32(dsSystemDetail.Tables[0].Rows[0]["LetterheadMargin"].ToString().Trim()); }
        catch { margin = 0; }

        iTextSharp.text.Document oDoc = new iTextSharp.text.Document(PageSize.LETTER, 0, 0, 30f, 30f);
        PdfWriter.GetInstance(oDoc, new FileStream(saveFilePath, FileMode.Create));

        oDoc.Open();
        string strDoc = "";
        //-------------------------------



        for (int Xh = 0; (Xh < strRefDrId.Length) && (strRefDrId[Xh] != ""); Xh++)
        {
            DataSet dsTemp = new DataSet("dsSchema");
            DataTable dtTemp;

            foreach (DataTable dt in dtList) dsTemp.Tables.Add(dt.Copy());

            cmdSelectRefDoctor.Parameters["@RefDrId"].Value = strRefDrId[Xh];
            dtTemp = gClass.FetchData(cmdSelectRefDoctor, "RefDoctor").Tables[0].Copy();

            if (dsTemp.Tables.Contains("RefDoctor"))
                foreach (DataRow drRow in dtTemp.Rows)
                    dsTemp.Tables["RefDoctor"].ImportRow(drRow);
            else
                dsTemp.Tables.Add(dtTemp);
            try
            {
                #region declare parameter passed from frontend
                string includeCurrentVisitOnly = dsTemp.Tables["tblReport"].Rows[0]["CurrentVisitOnly"].ToString();
                string IncludeLastTenVisit = dsTemp.Tables["tblPatientVisit"].Rows[0]["Last10Visits"].ToString();
                string IncludeProgressNote = dsTemp.Tables["tblPatientVisit"].Rows[0]["IncludeProgressNote"].ToString();
                string includePatientNote = dsTemp.Tables["tblReport"].Rows[0]["IncludePatientNote"].ToString();
                #endregion

                #region var declaration Date
                //var declaration Date
                string dateNow = DateTime.Now.ToString("D");
                #endregion

                #region var declaration RefDr
                //var declaration RefDr
                string refDrName = dsTemp.Tables["RefDoctor"].Rows[0]["RefDr_Name"].ToString();
                string refDrAddress1 = dsTemp.Tables["RefDoctor"].Rows[0]["Address1"].ToString();
                string refDrAddress2 = dsTemp.Tables["RefDoctor"].Rows[0]["Address2"].ToString();
                string refDrAddress3 = dsTemp.Tables["RefDoctor"].Rows[0]["Address3"].ToString();
                #endregion

                #region var declaration Patient
                //var declaration Patient
                string patientRe = "RE: ";
                string patientName = dsTemp.Tables["tblReport"].Rows[0]["PatName"].ToString();
                string patientStreet = dsTemp.Tables["tblReport"].Rows[0]["Street"].ToString();
                string patientSuburbPostcode = dsTemp.Tables["tblReport"].Rows[0]["SuburbPostcode"].ToString();
                string patientBirthDate = dsTemp.Tables["tblReport"].Rows[0]["Birthdate"].ToString();
                #endregion

                #region var declaration Comment
                //var declaration Comment
                string commentFName = dsTemp.Tables["tblReport"].Rows[0]["FName"].ToString();
                string commentWasRecently = "";
                string commentFollowupNotes = dsTemp.Tables["tblReport"].Rows[0]["FollowUpNotes"].ToString();

                #endregion

                #region var declaration Doctor
                //var declaration Doctor
                string doctorName = dsTemp.Tables["tblReport"].Rows[0]["DoctorName"].ToString();
                string doctorSpeciality = dsTemp.Tables["tblReport"].Rows[0]["Speciality"].ToString();
                string doctorDegrees = dsTemp.Tables["tblReport"].Rows[0]["Degrees"].ToString();
                #endregion

                #region var declaration Assessment
                //var declaration Assessment
                string assessmentIntro = "";
                string assessmentHeight = "";
                string assessmentWeight = "";
                string assessmentBMI = "";
                string assessmentOperationDate = "";
                string assessmentIdealWeight = "";
                string assessmentTargetWeight = "";
                string assessmentAboveIdealWeight = "";
                Decimal decTemp = 0m;

                double assessmentHeightValue = Math.Round(Convert.ToDouble(dsTemp.Tables["tblReport"].Rows[0]["Height"].ToString()));
                string assessmentWeightValue = dsTemp.Tables["tblReport"].Rows[0]["StartWeight"].ToString();
                assessmentWeightValue = Decimal.TryParse(assessmentWeightValue, out decTemp) ? decTemp.ToString(strNumberFormat) : "0";
                string assessmentBMIValue = dsTemp.Tables["tblReport"].Rows[0]["BMI"].ToString();
                assessmentBMIValue = Decimal.TryParse(assessmentBMIValue, out decTemp) ? decTemp.ToString(strNumberFormat) : "0";
                string assessmentOperationDateValue = dsTemp.Tables["tblReport"].Rows[0]["LapBandDate"].ToString();
                string assessmentIdealWeightValue = dsTemp.Tables["tblReport"].Rows[0]["IdealWeight"].ToString();
                assessmentIdealWeightValue = Decimal.TryParse(assessmentIdealWeightValue, out decTemp) ? decTemp.ToString(strNumberFormat) : "0";
                string assessmentTargetWeightValue = dsTemp.Tables["tblReport"].Rows[0]["TargetWeight"].ToString();
                assessmentTargetWeightValue = Decimal.TryParse(assessmentTargetWeightValue, out decTemp) ? decTemp.ToString(strNumberFormat) : "0";
                //string assessmentAboveIdealWeightValue = dsTemp.Tables["tblReport"].Rows[0]["ExcessWeight"].ToString();
                #endregion

                #region var declaration Patient Note
                //var declaration Patient Note
                string patientNoteIntro = "";
                string patientNoteValue = dsTemp.Tables["tblReport"].Rows[0]["SignificantEvents"].ToString().Trim();
                #endregion

                #region var declaration visit
                //var declaration visit

                string visitWeekFlag = dsTemp.Tables["tblPatientVisit"].Rows[0]["VisitWeeksFlag"].ToString();

                string visitFollowup = "";
                string visitDate = "";
                string visitWeek = "";
                string visitCurrent = "";
                string visitLoss = "";
                string visitBMI = "";
                string visitEWL = "";
                string visitReservoir = "";

                string visitDoctorValue = "";
                string visitDateValue = "";
                string visitWeekValue = "";
                string visitCurrentValue = "";
                string visitLossValue = "";
                string visitBMIValue = "";
                string visitEWLValue = "";
                string visitReservoirValue = "";
                string visitProgressNoteValue = "";

                #endregion

                #region var declaration caption
                //var declaration caption
                foreach (DataRow dr in dsTemp.Tables["tblCaptions"].Rows)
                {
                    if (dr["Field_ID"].ToString() == "lblWasRecently")
                        commentWasRecently = dr["FIELD_CAPTION"].ToString();

                    if (dr["Field_ID"].ToString() == "lblInitialAssessment")
                        assessmentIntro = dr["FIELD_CAPTION"].ToString();

                    if (dr["Field_ID"].ToString() == "lblHeight")
                        assessmentHeight = dr["FIELD_CAPTION"].ToString();

                    if (dr["Field_ID"].ToString() == "lblWeight")
                        assessmentWeight = dr["FIELD_CAPTION"].ToString();

                    if (dr["Field_ID"].ToString() == "lblBMI")
                        assessmentBMI = dr["FIELD_CAPTION"].ToString();

                    if (dr["Field_ID"].ToString() == "lblOperationDate")
                        assessmentOperationDate = dr["FIELD_CAPTION"].ToString();

                    if (dr["Field_ID"].ToString() == "lblIdealWeight")
                        assessmentIdealWeight = dr["FIELD_CAPTION"].ToString();

                    if (dr["Field_ID"].ToString() == "lblTargetWeight")
                        assessmentTargetWeight = dr["FIELD_CAPTION"].ToString();

                    if (dr["Field_ID"].ToString() == "lblAboveIdealWeight")
                        assessmentAboveIdealWeight = dr["FIELD_CAPTION"].ToString();

                    if (dr["Field_ID"].ToString() == "lblPatientNotes")
                        patientNoteIntro = dr["FIELD_CAPTION"].ToString();

                    if (dr["Field_ID"].ToString() == "lblFollowUp")
                        visitFollowup = dr["FIELD_CAPTION"].ToString();

                    if (dr["Field_ID"].ToString() == "lblDate_TC")
                        visitDate = dr["FIELD_CAPTION"].ToString();

                    if (dr["Field_ID"].ToString() == "lblWeeks_TC")
                        visitWeek = dr["FIELD_CAPTION"].ToString();

                    if (dr["Field_ID"].ToString() == "lblCurrent_TC")
                        visitCurrent = dr["FIELD_CAPTION"].ToString() + " (" + patientWeightMeasurment + ")";

                    if (visitWeekFlag == "0")
                    {
                        if (dr["Field_ID"].ToString() == "lblLoss_TC")
                            visitLoss = dr["FIELD_CAPTION"].ToString() + " (" + patientWeightMeasurment + ")";
                    }
                    else
                    {
                        if (dr["Field_ID"].ToString() == "lblTotalLoss_TC")
                            visitLoss = dr["FIELD_CAPTION"].ToString() + " (" + patientWeightMeasurment + ")";
                    }

                    if (dr["Field_ID"].ToString() == "lblBMI_TC")
                        visitBMI = dr["FIELD_CAPTION"].ToString();

                    if (dr["Field_ID"].ToString() == "lblEWL_TC")
                        visitEWL = dr["FIELD_CAPTION"].ToString();

                    if (dr["Field_ID"].ToString() == "lblReservoirVolume_TC")
                        visitReservoir = dr["FIELD_CAPTION"].ToString();
                }
                #endregion


                if (reportFormat == "pdf")
                {
                    #region print pdf
                    #region declare table and width

                    Cell cell = new Cell("");
                    Font H2 = new Font(Font.HELVETICA, 12, Font.BOLD);
                    Font H3 = new Font(Font.HELVETICA, 10, Font.BOLD);
                    Font H4 = new Font(Font.HELVETICA, 9, Font.BOLD);
                    Font smallFont = new Font(Font.HELVETICA, 6, 0);
                    Font normalFont = new Font(Font.HELVETICA, 8, 0);
                    Font normalBoldFont = new Font(Font.HELVETICA, 8, Font.BOLD);





                    //create header
                    iTextSharp.text.Table tableLogo = new iTextSharp.text.Table(1, 1);
                    iTextSharp.text.Table tableMargin = new iTextSharp.text.Table(1, 1);
                    iTextSharp.text.Table tableDate = new iTextSharp.text.Table(1, 1);
                    iTextSharp.text.Table tableRefDr = new iTextSharp.text.Table(1, 5);
                    iTextSharp.text.Table tablePatient = new iTextSharp.text.Table(1, 4);
                    iTextSharp.text.Table tableComment = new iTextSharp.text.Table(1, 2);
                    iTextSharp.text.Table tableDoctor = new iTextSharp.text.Table(1, 3);
                    iTextSharp.text.Table tableAssessment = new iTextSharp.text.Table(4, 5);
                    iTextSharp.text.Table tablePatientNote = new iTextSharp.text.Table(1, 2);
                    iTextSharp.text.Table tableVisit = new iTextSharp.text.Table(8, 150);

                    // set *column* widths
                    float[] logoWidths = { 1f };
                    tableLogo.Spacing = 1;
                    tableLogo.Padding = margin;
                    tableLogo.Widths = logoWidths;
                    tableLogo.DefaultCell.BorderWidth = 0;
                    tableLogo.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    tableLogo.DefaultCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    tableLogo.TableFitsPage = true;
                    tableLogo.BorderColor = new Color(255, 255, 255);

                    float[] marginWidths = { 1f };
                    tableMargin.Spacing = 1;
                    tableMargin.Padding = margin;
                    tableMargin.Widths = marginWidths;
                    tableMargin.DefaultCell.BorderWidth = 0;
                    tableMargin.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    tableMargin.DefaultCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    tableMargin.TableFitsPage = true;
                    tableMargin.BorderColor = new Color(255, 255, 255);

                    float[] dateWidths = { 1f };
                    tableDate.Spacing = 1;
                    tableDate.Widths = dateWidths;
                    tableDate.DefaultCell.BorderWidth = 0;
                    tableDate.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    tableDate.TableFitsPage = true;
                    tableDate.BorderColor = new Color(255, 255, 255);

                    float[] refDrWidths = { 1f };
                    tableRefDr.Spacing = 1;
                    tableRefDr.Widths = refDrWidths;
                    tableRefDr.DefaultCell.BorderWidth = 0;
                    tableRefDr.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    tableRefDr.TableFitsPage = true;
                    tableRefDr.BorderColor = new Color(255, 255, 255);

                    float[] patientWidths = { 1f };
                    tablePatient.Spacing = 1;
                    tablePatient.Widths = patientWidths;
                    tablePatient.DefaultCell.BorderWidth = 0;
                    tablePatient.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    tablePatient.TableFitsPage = true;
                    tablePatient.BorderColor = new Color(255, 255, 255);

                    float[] commentWidths = { 1f };
                    tableComment.Spacing = 1;
                    tableComment.Widths = commentWidths;
                    tableComment.DefaultCell.BorderWidth = 0;
                    tableComment.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    tableComment.TableFitsPage = true;
                    tableComment.BorderColor = new Color(255, 255, 255);

                    float[] doctorWidths = { 1f };
                    tableDoctor.Spacing = 1;
                    tableDoctor.Widths = doctorWidths;
                    tableDoctor.DefaultCell.BorderWidth = 0;
                    tableDoctor.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    tableDoctor.TableFitsPage = true;
                    tableDoctor.BorderColor = new Color(255, 255, 255);

                    float[] assessmentWidths = { .15f, .15f, .15f, .55f };
                    tableAssessment.Spacing = 1;
                    tableAssessment.Widths = assessmentWidths;
                    tableAssessment.DefaultCell.BorderWidth = 0;
                    tableAssessment.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    tableAssessment.TableFitsPage = true;
                    tableAssessment.BorderColor = new Color(255, 255, 255);

                    float[] patientNoteWidths = { 1f };
                    tablePatientNote.Spacing = 1;
                    tablePatientNote.Widths = patientNoteWidths;
                    tablePatientNote.DefaultCell.BorderWidth = 0;
                    tablePatientNote.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    tablePatientNote.TableFitsPage = true;
                    tablePatientNote.BorderColor = new Color(255, 255, 255);

                    float[] visitWidths = { .175f, .15f, .1f, .125f, .125f, .1f, .1f, .125f };
                    tableVisit.Spacing = 1;
                    tableVisit.Widths = visitWidths;
                    tableVisit.DefaultCell.BorderWidth = 0;
                    tableVisit.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    tableVisit.TableFitsPage = true;
                    tableVisit.BorderColor = new Color(255, 255, 255);
                    #endregion

                    #region Logo
                    //Logo----------------------------------------------------------------------------------------------

                    if (LogoPath != "")
                    {
                        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~" + LogoPath));
                        logo.ScalePercent(50);
                        cell = new Cell(logo);
                        cell.BorderWidth = 0;
                        tableLogo.AddCell(cell);
                    }
                    #endregion

                    #region Margin
                    //Margin----------------------------------------------------------------------------------------------
                    phrase = new Phrase("", normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableMargin.AddCell(cell);
                    #endregion

                    #region Date
                    //Date----------------------------------------------------------------------------------------------
                    phrase = new Phrase(dateNow + "\n\n", normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDate.AddCell(cell);
                    #endregion

                    #region RefDr
                    //RefDr----------------------------------------------------------------------------------------------
                    phrase = new Phrase("             " + refDrName, H4);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableRefDr.AddCell(cell);

                    if (refDrAddress1 != "")
                    {
                        phrase = new Phrase("             " + "   " + refDrAddress1, H4);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tableRefDr.AddCell(cell);
                    }

                    if (refDrAddress2 != "")
                    {
                        phrase = new Phrase("             " + "   " + refDrAddress2, H4);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tableRefDr.AddCell(cell);
                    }

                    if (refDrAddress3 != "")
                    {
                        phrase = new Phrase("             " + "   " + refDrAddress3, H4);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tableRefDr.AddCell(cell);
                    }

                    phrase = new Phrase("\n\n", normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableRefDr.AddCell(cell);
                    #endregion

                    #region Patient
                    //Patient----------------------------------------------------------------------------------------------
                    phrase = new Phrase(patientRe + patientName, normalBoldFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tablePatient.AddCell(cell);

                    if (patientStreet != "")
                    {
                        phrase = new Phrase("       " + patientStreet, normalBoldFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tablePatient.AddCell(cell);
                    }

                    if (patientSuburbPostcode != "")
                    {
                        phrase = new Phrase("       " + patientSuburbPostcode, normalBoldFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tablePatient.AddCell(cell);
                    }
                    phrase = new Phrase("       DOB: " + patientBirthDate + "\n\n", normalBoldFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tablePatient.AddCell(cell);
                    #endregion

                    #region Comment
                    //Comment----------------------------------------------------------------------------------------------
                    /*
                    if (commentWasRecently != "")
                    {
                        phrase = new Phrase(commentFName + commentWasRecently + "\n", normalFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tableComment.AddCell(cell);
                    }
                    */
                    phrase = new Phrase(commentFollowupNotes.Replace("<br/>","\n") + "\n\n\n", normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableComment.AddCell(cell);
                    #endregion

                    #region Doctor
                    //Doctor----------------------------------------------------------------------------------------------
                    phrase = new Phrase(doctorName, H3);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDoctor.AddCell(cell);

                    if (doctorSpeciality != "")
                    {
                        phrase = new Phrase(doctorSpeciality, normalFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tableDoctor.AddCell(cell);
                    }

                    if (doctorDegrees != "")
                    {
                        phrase = new Phrase(doctorDegrees, normalFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tableDoctor.AddCell(cell);
                    }

                    phrase = new Phrase("\n\n", normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDoctor.AddCell(cell);
                    #endregion

                    #region Assessment
                    //Assessment----------------------------------------------------------------------------------------------
                    phrase = new Phrase(assessmentIntro, normalBoldFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 4;
                    tableAssessment.AddCell(cell);

                    phrase = new Phrase(assessmentHeight, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableAssessment.AddCell(cell);

                    phrase = new Phrase(assessmentHeightValue + " " + patientHeightMeasurment, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableAssessment.AddCell(cell);

                    phrase = new Phrase(assessmentIdealWeight, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableAssessment.AddCell(cell);

                    phrase = new Phrase(assessmentIdealWeightValue + " " + patientWeightMeasurment, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableAssessment.AddCell(cell);

                    phrase = new Phrase(assessmentWeight, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableAssessment.AddCell(cell);

                    phrase = new Phrase(assessmentWeightValue + " " + patientWeightMeasurment, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableAssessment.AddCell(cell);

                    phrase = new Phrase(assessmentTargetWeight, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableAssessment.AddCell(cell);

                    phrase = new Phrase(assessmentTargetWeightValue + " " + patientWeightMeasurment, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableAssessment.AddCell(cell);

                    phrase = new Phrase(assessmentBMI, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableAssessment.AddCell(cell);

                    phrase = new Phrase(assessmentBMIValue, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 3;
                    tableAssessment.AddCell(cell);
                    /*
                    phrase = new Phrase(assessmentAboveIdealWeight, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableAssessment.AddCell(cell); 
                    
                    phrase = new Phrase("A", normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableAssessment.AddCell(cell); 
                    */
                    phrase = new Phrase(assessmentOperationDate, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableAssessment.AddCell(cell);

                    phrase = new Phrase(assessmentOperationDateValue, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 3;
                    tableAssessment.AddCell(cell);

                    phrase = new Phrase("\n\n", normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 4;
                    tableAssessment.AddCell(cell);
                    #endregion

                    #region Patient Note
                    //Patient Note----------------------------------------------------------------------------------------------
                    if (includePatientNote == "1" && patientNoteValue != "")
                    {
                        phrase = new Phrase(patientNoteIntro, normalBoldFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tablePatientNote.AddCell(cell);

                        phrase = new Phrase(patientNoteValue + "\n\n\n", normalFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tablePatientNote.AddCell(cell);
                    }
                    #endregion

                    #region Visit
                    //Visit----------------------------------------------------------------------------------------------
                    phrase = new Phrase(visitFollowup, normalBoldFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 8;
                    tableVisit.AddCell(cell);

                    phrase = new Phrase("", normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableVisit.AddCell(cell);

                    phrase = new Phrase(visitDate, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableVisit.AddCell(cell);

                    phrase = new Phrase(visitWeek, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableVisit.AddCell(cell);

                    phrase = new Phrase(visitCurrent, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableVisit.AddCell(cell);

                    phrase = new Phrase(visitLoss, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableVisit.AddCell(cell);

                    phrase = new Phrase(visitBMI, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableVisit.AddCell(cell);

                    phrase = new Phrase(visitEWL, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableVisit.AddCell(cell);

                    phrase = new Phrase(visitReservoir, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableVisit.AddCell(cell);


                    int countVisit = 0;
                    foreach (DataRow drVisit in dsTemp.Tables["tblPatientVisit"].Rows)
                    {
                        if (countVisit == 1 && includeCurrentVisitOnly == "1")
                            break;
                        else if (countVisit == 10 && IncludeLastTenVisit == "1")
                            break;

                        visitDoctorValue = drVisit["DoctorName"].ToString();
                        visitDateValue = drVisit["strDateSeen"].ToString();
                        visitCurrentValue = drVisit["Weight"].ToString();
                        visitBMIValue = drVisit["BMI"].ToString();
                        visitReservoirValue = drVisit["ReservoirVolume"].ToString();
                        visitProgressNoteValue = drVisit["Notes"].ToString();

                        if (visitReservoirValue != "" && visitReservoirValue != "0")
                        {
                            visitReservoirValue = visitReservoirValue + " mls";
                        }

                        if (visitWeekFlag == "0")
                        {
                            visitWeekValue = drVisit["Weeks"].ToString();
                            visitLossValue = drVisit["WeightLoss"].ToString();
                            visitEWLValue = drVisit["EWL"].ToString();
                        }
                        else if (visitWeekFlag == "1")
                        {
                            visitWeekValue = drVisit["WeeksFromFirstVisit"].ToString();
                            visitLossValue = drVisit["WeightLossFromFirstVisit"].ToString();
                            visitEWLValue = drVisit["EWL"].ToString();
                        }
                        else if (visitWeekFlag == "3")
                        {
                            visitWeekValue = drVisit["WeeksFromZeroDate"].ToString();
                            visitLossValue = drVisit["WeightLossFromOperationDate"].ToString();
                            visitEWLValue = drVisit["EWLOperationDate"].ToString();
                        }
                        else if (visitWeekFlag == "4")
                        {
                            visitWeekValue = drVisit["WeeksFromOperationDate"].ToString();
                            visitLossValue = drVisit["WeightLossFromOperationDate"].ToString();
                            visitEWLValue = drVisit["EWLOperationDate"].ToString();
                        }
                        else
                        {
                            visitWeekValue = drVisit["Weeks"].ToString();
                            visitLossValue = drVisit["WeightLoss"].ToString();
                            visitEWLValue = drVisit["EWL"].ToString();
                        }
                        if (visitWeekValue == "0")
                            visitWeekValue = "";

                        visitBMIValue = Decimal.TryParse(visitBMIValue, out decTemp) ? decTemp.ToString(strNumberFormat) : "0";
                        visitCurrentValue = Decimal.TryParse(visitCurrentValue, out decTemp) ? decTemp.ToString(strNumberFormat) : "0";
                        visitLossValue = Decimal.TryParse(visitLossValue, out decTemp) ? decTemp.ToString(strNumberFormat) : "0";
                        visitEWLValue = Decimal.TryParse(visitEWLValue, out decTemp) ? decTemp.ToString(strNumberFormat) : "0";

                        //get comorbidity value
                        comorbidityResult = GetComorbidityResult(Convert.ToInt32(drVisit["ConsultID"].ToString().Trim()), drVisit["ComorbidityVisit"].ToString().Trim(), baselineComorbidityAvailable, "\n");
            
                        phrase = new Phrase(visitDoctorValue, normalFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tableVisit.AddCell(cell);

                        phrase = new Phrase(visitDateValue, normalFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tableVisit.AddCell(cell);

                        phrase = new Phrase(visitWeekValue, normalFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tableVisit.AddCell(cell);

                        phrase = new Phrase(visitCurrentValue, normalFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tableVisit.AddCell(cell);

                        phrase = new Phrase(visitLossValue, normalFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tableVisit.AddCell(cell);

                        phrase = new Phrase(visitBMIValue, normalFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tableVisit.AddCell(cell);

                        phrase = new Phrase(visitEWLValue, normalFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tableVisit.AddCell(cell);

                        phrase = new Phrase(visitReservoirValue, normalFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tableVisit.AddCell(cell);

                        if (IncludeProgressNote == "1" && visitProgressNoteValue != "")
                        {
                            phrase = new Phrase("", normalFont);
                            cell = new Cell(phrase);
                            cell.BorderWidth = 0;
                            tableVisit.AddCell(cell);

                            phrase = new Phrase(visitProgressNoteValue, normalFont);
                            cell = new Cell(phrase);
                            cell.BorderWidth = 0;
                            cell.Colspan = 7;
                            tableVisit.AddCell(cell);
                        }

                        if (comorbidityResult.Trim() != "")
                        {
                            phrase = new Phrase("", normalFont);
                            cell = new Cell(phrase);
                            cell.BorderWidth = 0;
                            tableVisit.AddCell(cell);

                            phrase = new Phrase(comorbidityResult, normalFont);
                            cell = new Cell(phrase);
                            cell.BorderWidth = 0;
                            cell.Colspan = 7;
                            tableVisit.AddCell(cell);
                        }

                        countVisit++;
                    }

                    #endregion


                    if (Xh > 0)
                        oDoc.NewPage();

                    oDoc.Add(tableLogo);
                    oDoc.Add(tableMargin);
                    oDoc.Add(tableDate);
                    oDoc.Add(tableRefDr);
                    oDoc.Add(tablePatient);
                    oDoc.Add(tableComment);
                    oDoc.Add(tableDoctor);
                    oDoc.Add(tableAssessment);
                    oDoc.Add(tablePatientNote);
                    oDoc.Add(tableVisit);


                    #endregion
                }
                else if (reportFormat == "doc")
                {
                    #region print word doc

                    if (Xh > 0)
                        strDoc += "<br clear=all style='mso-page-orientation: landscape;mso-special-character:line-break;page-break-before:always'>";


                    #region Logo
                    //Logo----------------------------------------------------------------------------------------------
                    if (LogoPath != "")
                    {
                        string url = "";
                        url = HttpContext.Current.Request.Url.AbsoluteUri;
                        url = url.Substring(0, url.IndexOf("?"));
                        strDoc += "<table><tr><td><img src='" + url + "/../.." + LogoPath + "'/></td></tr></table>";
                    }
                    #endregion

                    #region Margin
                    strDoc += "<table><tr><td style='margin-top:" + margin + "cm'>&nbsp;</td></tr></table>";
                    #endregion

                    #region Date
                    strDoc += "<table><tr><td>" + dateNow + "</td></tr><tr><td>&nbsp;</td></tr></table>";
                    #endregion

                    #region RefDr
                    //RefDr----------------------------------------------------------------------------------------------
                    strDoc += "<table><tr><td style='margin-left:1cm;'><h3>" + refDrName + "</h3></td></tr>";
                    if (refDrAddress1 != "")
                        strDoc += "<tr><td style='margin-left:1cm;'><h3>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + refDrAddress1 + "</h3></td></tr>";

                    if (refDrAddress2 != "")
                        strDoc += "<tr><td style='margin-left:1cm;'><h3>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + refDrAddress2 + "</h3></td></tr>";

                    if (refDrAddress3 != "")
                        strDoc += "<tr><td style='margin-left:1cm;'><h3>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + refDrAddress3 + "</h3></td></tr>";
                    strDoc += "<tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr></table>";
                    #endregion

                    #region Patient
                    //Patient----------------------------------------------------------------------------------------------
                    strDoc += "<table><tr><td style='font-weight:bold'>" + patientRe + patientName + "</td></tr>";

                    if (patientStreet != "")
                        strDoc += "<tr><td style='font-weight:bold'>       " + patientStreet + " </td></tr>";


                    if (patientSuburbPostcode != "")
                        strDoc += "<tr><td style='font-weight:bold'>       " + patientSuburbPostcode + " </td></tr>";

                    strDoc += "<tr><td style='font-weight:bold'>       DOB: " + patientBirthDate + "</td></tr><tr><td>&nbsp;</td></tr></table>";
                    #endregion

                    #region Comment
                    //Comment----------------------------------------------------------------------------------------------
                    strDoc += "<table>";
                    if (commentWasRecently != "")
                        strDoc += "<tr style='display:none'><td>"+commentFName + commentWasRecently+"</td></tr><tr><td>&nbsp;</td></tr>";

                    strDoc += "<tr><td>" + commentFollowupNotes + "</td></tr><tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr></table>";
                    #endregion

                    #region Doctor
                    //Doctor----------------------------------------------------------------------------------------------
                    strDoc += "<table><tr><td><h2>" + doctorName + "</h2><td></tr>";

                    if (doctorSpeciality != "")
                        strDoc += "<tr><td>" + doctorSpeciality + "<td></tr>";

                    if (doctorDegrees != "")
                        strDoc += "<tr><td>" + doctorDegrees + "<td></tr>";

                    strDoc += "<tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr></table>";
                    #endregion

                    #region Assessment
                    //Assessment----------------------------------------------------------------------------------------------
                    strDoc += "<table><tr><td style='font-weight:bold' colspan='4'>" + assessmentIntro + "</td></tr>";

                    strDoc += "<tr><td>" + assessmentHeight + "</td><td>" + assessmentHeightValue + " " + patientHeightMeasurment + "</td><td>" + assessmentIdealWeight + "</td><td>" + assessmentIdealWeightValue + " " + patientWeightMeasurment + "</td></tr>";
                    strDoc += "<tr><td>"+assessmentWeight+"</td><td>"+assessmentWeightValue + " " + patientWeightMeasurment+"</td><td>"+assessmentTargetWeight+"</td><td>"+assessmentTargetWeightValue + " " + patientWeightMeasurment+"</td></tr>";
                    strDoc += "<tr><td>"+assessmentBMI+"</td><td colspan='3'>"+assessmentBMIValue+"</td></tr>";
                    strDoc += "<tr><td>"+assessmentOperationDate+"</td><td>"+assessmentOperationDateValue+"</td></tr>";
                    strDoc += "<tr><td colspan='4'>&nbsp;</td></tr><tr><td colspan='4'>&nbsp;</td></tr></table>";
                    #endregion

                    #region Patient Note
                    //Patient Note----------------------------------------------------------------------------------------------
                    if (includePatientNote == "1" && patientNoteValue != "")
                    {
                        strDoc += "<table><tr><td style='font-weight:bold'>" + patientNoteIntro + "</td></tr>";
                        strDoc += "<tr><td>" + patientNoteValue + "</td></tr><tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr></table>";
                    }
                    #endregion

                    #region Visit
                    //Visit----------------------------------------------------------------------------------------------
                    strDoc += "<table width='100%'><tr><td colspan='8' style='font-weight:bold'>"+visitFollowup+"</td></tr>";
                    strDoc += "<tr><td width='17.5%'>&nbsp;</td><td width='15%'>"+visitDate+"</td><td width='10%'>"+visitWeek+"</td><td width='12.5%'>"+visitCurrent+"</td>";
                    strDoc += "<td width='12.5%'>"+visitLoss+"</td><td width='10%'>"+visitBMI+"</td><td width='10%'>"+visitEWL+"</td><td width='12.5%'>"+visitReservoir+"</td></tr>";

                    int countVisit = 0;
                    foreach (DataRow drVisit in dsTemp.Tables["tblPatientVisit"].Rows)
                    {
                        if (countVisit == 1 && includeCurrentVisitOnly == "1")
                            break;
                        else if (countVisit == 10 && IncludeLastTenVisit == "1")
                            break;

                        visitDoctorValue = drVisit["DoctorName"].ToString();
                        visitDateValue = drVisit["strDateSeen"].ToString();
                        visitCurrentValue = drVisit["Weight"].ToString();
                        visitBMIValue = drVisit["BMI"].ToString();
                        visitReservoirValue = drVisit["ReservoirVolume"].ToString();
                        visitProgressNoteValue = drVisit["Notes"].ToString();

                        if (visitReservoirValue != "" && visitReservoirValue != "0")
                        {
                            visitReservoirValue = visitReservoirValue + " mls";
                        }

                        if (visitWeekFlag == "0")
                        {
                            visitWeekValue = drVisit["Weeks"].ToString();
                            visitLossValue = drVisit["WeightLoss"].ToString();
                            visitEWLValue = drVisit["EWL"].ToString();
                        }
                        else if (visitWeekFlag == "1")
                        {
                            visitWeekValue = drVisit["WeeksFromFirstVisit"].ToString();
                            visitLossValue = drVisit["WeightLossFromFirstVisit"].ToString();
                            visitEWLValue = drVisit["EWL"].ToString();
                        }
                        else if (visitWeekFlag == "3")
                        {
                            visitWeekValue = drVisit["WeeksFromZeroDate"].ToString();
                            visitLossValue = drVisit["WeightLossFromOperationDate"].ToString();
                            visitEWLValue = drVisit["EWLOperationDate"].ToString();
                        }
                        else if (visitWeekFlag == "4")
                        {
                            visitWeekValue = drVisit["WeeksFromOperationDate"].ToString();
                            visitLossValue = drVisit["WeightLossFromOperationDate"].ToString();
                            visitEWLValue = drVisit["EWLOperationDate"].ToString();
                        }
                        else
                        {
                            visitWeekValue = drVisit["Weeks"].ToString();
                            visitLossValue = drVisit["WeightLoss"].ToString();
                            visitEWLValue = drVisit["EWL"].ToString();
                        }

                        visitBMIValue = Decimal.TryParse(visitBMIValue, out decTemp) ? decTemp.ToString(strNumberFormat) : "0";
                        visitCurrentValue = Decimal.TryParse(visitCurrentValue, out decTemp) ? decTemp.ToString(strNumberFormat) : "0";
                        visitLossValue = Decimal.TryParse(visitLossValue, out decTemp) ? decTemp.ToString(strNumberFormat) : "0";
                        visitEWLValue = Decimal.TryParse(visitEWLValue, out decTemp) ? decTemp.ToString(strNumberFormat) : "0";
                        //get comorbidity value
                        comorbidityResult = GetComorbidityResult(Convert.ToInt32(drVisit["ConsultID"].ToString().Trim()), drVisit["ComorbidityVisit"].ToString().Trim(), baselineComorbidityAvailable, "<br>");
            

                        
                        strDoc += "<tr><td>"+visitDoctorValue+"</td><td>"+visitDateValue+"</td><td>"+visitWeekValue+"</td><td>"+visitCurrentValue+"</td>";
                        strDoc += "<td>"+visitLossValue+"</td><td>"+visitBMIValue+"</td><td>"+visitEWLValue+"</td><td>"+visitReservoirValue+"</td></tr>";

                        if (IncludeProgressNote == "1" && visitProgressNoteValue != "")
                            strDoc += visitProgressNoteValue+"</td></tr>";

                        
                        if (comorbidityResult.Trim() != "")
                        {
                            strDoc += "<tr><td>&nbsp;</td><td colspan='7'>"+comorbidityResult+"</td></tr>";
                        }

                        countVisit++;
                    }
                    strDoc += "</table>";

                    #endregion

                    #endregion
                }
                else if (Request.QueryString["Preview"] == "0" || Request.QueryString["Preview"] == "1")
                {
                    #region preview and print
                    //for preview and print

                    strReport += gClass.ShowSchema(dsTemp, Server.MapPath(strXSLTFileName));
                    #endregion
                }
            }
            catch (Exception err)
            {
                string error = err.ToString();
                //gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Patient Data Form", "Loading Patient Data - LoadPatientData function", err.ToString());
            }

            dsReport.Dispose();
        }



        try
        {
            if (dsReport.Tables.Count > 0)
            {
                if (Request.QueryString["Preview"] == "4")
                {
                    StringBuilder sbTop = new System.Text.StringBuilder();
                    sbTop.Append(@"
                    <html 
                    xmlns:o='urn:schemas-microsoft-com:office:office' 
                    xmlns:w='urn:schemas-microsoft-com:office:word'
                    xmlns='http://www.w3.org/TR/REC-html40'>
                    <head><title></title>

                    <!--[if gte mso 9]>
                    <xml>
                    <w:WordDocument>
                    <w:View>Print</w:View>
                    <w:Zoom>90</w:Zoom>
                    <w:DoNotOptimizeForBrowser/>
                    </w:WordDocument>
                    </xml>
                    <![endif]-->


                    <style>
                    p.MsoFooter, li.MsoFooter, div.MsoFooter
                    {
                    margin:0in;
                    margin-bottom:.0001pt;
                    mso-pagination:widow-orphan;
                    tab-stops:center 3.0in right 6.0in;
                    font-size:12.0pt;
                    }
                    <style>

                    <!-- /* Style Definitions */

                    @page Section1
                    {
                    margin:1.0in 0.5in 1.0in 0.5in ;
                    mso-header-margin:.5in;
                    mso-header:h1;
                    mso-footer: f1; 
                    mso-footer-margin:.5in;
                    }


                    div.Section1
                    {
                    page:Section1;
                    }

                    table#hrdftrtbl
                    {
                    margin:0in 0in 0in 9in;
                    }
                    -->
                    </style></head>

                    <body lang=EN-US style='tab-interval:.3in'>");

                    sbTop.Append(strDoc);

                    sbTop.Append(@"
                    </body></html>
                    ");

                    string strBody = sbTop.ToString();
                    Response.AppendHeader("Content-Type", "application/msword");
                    Response.AppendHeader("Content-disposition", "attachment; filename=" + fileName + "." + reportFormat);
                    Response.Write(strBody);
                }
                else if (reportFormat == "pdf")
                {
                    oDoc.Close();
                    Response.Redirect(openFilePath);
                }
                else if (Request.QueryString["Preview"] == "0" || Request.QueryString["Preview"] == "1")
                {
                    string url = "";
                    url = HttpContext.Current.Request.Url.AbsoluteUri;
                    url = url.Substring(0, url.IndexOf("?"));
                    strReport = strReport.Replace("/Photos/Logo", url + "/../../Photos/Logo");
                    tcXML.InnerHtml = strReport;
                }
            }
        }
        catch (Exception err) { Response.Write(err.ToString()); }
        return;
    }
    #endregion RefDoctorLetter_BuildReport

    #region private void InitializeReportCommand(ref SqlCommand cmdSelect, string strCommandText)
    private void InitializeReportCommand(ref SqlCommand cmdSelect, string strCommandText)
    {
        System.IFormatProvider format = new System.Globalization.CultureInfo(Request.Cookies["CultureInfo"].Value.ToString(), true);

        gClass.MakeStoreProcedureName(ref cmdSelect, strCommandText, true);

        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSelect.Parameters.Add("@SurgeonId", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["Surgeon"]);
        cmdSelect.Parameters.Add("@HospitalCode", SqlDbType.VarChar, 6).Value = (Request.QueryString["Hospital"].Equals("0") || (Request.QueryString["Hospital"].Length == 0)) ? string.Empty : Request.QueryString["Hospital"];
        cmdSelect.Parameters.Add("@RegionId", SqlDbType.VarChar, 6).Value = (Request.QueryString["Region"].Equals("0") || (Request.QueryString["Region"].Length == 0)) ? string.Empty : Request.QueryString["Region"];
        
        cmdSelect.Parameters.Add("@FDate", SqlDbType.VarChar);
        cmdSelect.Parameters.Add("@TDate", SqlDbType.VarChar);

        cmdSelect.Parameters.Add("@FAge", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["FAge"]);
        cmdSelect.Parameters.Add("@TAge", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["TAge"]);
        cmdSelect.Parameters.Add("@FBMI", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["FBMI"]);
        cmdSelect.Parameters.Add("@TBMI", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["TBMI"]);

        cmdSelect.Parameters.Add("@GroupCode", SqlDbType.VarChar, 10).Value = (Request.QueryString["Grp"].Equals("0") || (Request.QueryString["Grp"].Length == 0)) ? string.Empty : Request.QueryString["Grp"];
        cmdSelect.Parameters.Add("@SurgeryType", SqlDbType.VarChar, 10).Value = (Request.QueryString["Sur"].Equals("0") || (Request.QueryString["Sur"].Length == 0)) ? string.Empty : Request.QueryString["Sur"];
        cmdSelect.Parameters.Add("@BandType", SqlDbType.VarChar, 10).Value = (Request.QueryString["Band"].Equals("0") || (Request.QueryString["Band"].Length == 0)) ? string.Empty : Request.QueryString["Band"];

        cmdSelect.Parameters.Add("@Approach", SqlDbType.VarChar, 10).Value = (Request.QueryString["App"].Equals("0") || (Request.QueryString["App"].Length == 0)) ? string.Empty : Request.QueryString["App"];
        cmdSelect.Parameters.Add("@Category", SqlDbType.VarChar, 10).Value = (Request.QueryString["Cat"].Equals("0") || (Request.QueryString["Cat"].Length == 0)) ? string.Empty : Request.QueryString["Cat"];
        cmdSelect.Parameters.Add("@BandSize", SqlDbType.VarChar, 10).Value = (Request.QueryString["Size"].Equals("0") || (Request.QueryString["Size"].Length == 0)) ? string.Empty : Request.QueryString["Size"];
        

        String FDate = Request.QueryString["FDate"];
        String TDate = Request.QueryString["TDate"];
        string parsedFDate = "";
        string parsedTDate = "";
        if (FDate != "0")
        {
            DateTime fdt = DateTime.Parse(FDate, format);
            string fSplitDate = fdt.Day.ToString();
            string fSplitMonth = fdt.Month.ToString();
            string fSplitYear = fdt.Year.ToString();
            parsedFDate = fSplitMonth + "/" + fSplitDate + "/" + fSplitYear;
        }
        if (TDate != "0")
        {
            DateTime tdt = DateTime.Parse(TDate, format);
            string tSplitDate = tdt.Day.ToString();
            string tSplitMonth = tdt.Month.ToString();
            string tSplitYear = tdt.Year.ToString();
            parsedTDate = tSplitMonth + "/" + tSplitDate + "/" + tSplitYear;
        }



        

        cmdSelect.Parameters["@FDate"].Value = parsedFDate;
        cmdSelect.Parameters["@TDate"].Value = parsedTDate;
        

        //Report Title 
        //Date
        if (parsedFDate == "" && parsedTDate == "")
            titleDate = "-";
        else if (parsedFDate != "" && parsedTDate != "")
            titleDate = Request.QueryString["FDate"] + " - " + Request.QueryString["TDate"];
        else if (parsedFDate != "")
            titleDate = "From " + Request.QueryString["FDate"];
        else if (parsedTDate != "")
            titleDate = "Until " + Request.QueryString["TDate"];

        if (Request.QueryString["FAge"].Equals("0") && Request.QueryString["TAge"].Equals("0"))
            titleAge = "-";
        else if (Request.QueryString["FAge"].Equals("0") == false && Request.QueryString["TAge"].Equals("0") == false)
            titleAge = Request.QueryString["FAge"] + " - " + Request.QueryString["TAge"];
        else if (Request.QueryString["FAge"].Equals("0") == false)
            titleAge = "Min " + Request.QueryString["FAge"];
        else if (Request.QueryString["TAge"].Equals("0") == false)
            titleAge = "Max " + Request.QueryString["TAge"];

        if (Request.QueryString["FBMI"].Equals("0") && Request.QueryString["TBMI"].Equals("0"))
            titleBMI = "-";
        else if (Request.QueryString["FBMI"].Equals("0") == false && Request.QueryString["TBMI"].Equals("0") == false)
            titleBMI = Request.QueryString["FBMI"] + " - " + Request.QueryString["TBMI"];
        else if (Request.QueryString["FBMI"].Equals("0") == false)
            titleBMI = "Min " + Request.QueryString["FBMI"];
        else if (Request.QueryString["TBMI"].Equals("0") == false)
            titleBMI = "Max " + Request.QueryString["TBMI"];
        
        DataSet dsReportTitle;
        SqlCommand cmdSelectTitle = new SqlCommand();

        gClass.MakeStoreProcedureName(ref cmdSelectTitle, "sp_Rep_ReportTitle", true);

        cmdSelectTitle.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSelectTitle.Parameters.Add("@SurgeonId", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["Surgeon"]);
        cmdSelectTitle.Parameters.Add("@HospitalCode", SqlDbType.VarChar, 6).Value = (Request.QueryString["Hospital"].Equals("0") || (Request.QueryString["Hospital"].Length == 0)) ? string.Empty : Request.QueryString["Hospital"];
        cmdSelectTitle.Parameters.Add("@RegionId", SqlDbType.VarChar, 6).Value = (Request.QueryString["Region"].Equals("0") || (Request.QueryString["Region"].Length == 0)) ? string.Empty : Request.QueryString["Region"];
        cmdSelectTitle.Parameters.Add("@GroupCode", SqlDbType.VarChar, 10).Value = (Request.QueryString["Grp"].Equals("0") || (Request.QueryString["Grp"].Length == 0)) ? string.Empty : Request.QueryString["Grp"];
        cmdSelectTitle.Parameters.Add("@SurgeryType", SqlDbType.VarChar, 10).Value = (Request.QueryString["Sur"].Equals("0") || (Request.QueryString["Sur"].Length == 0)) ? string.Empty : Request.QueryString["Sur"];
        cmdSelectTitle.Parameters.Add("@BandType", SqlDbType.VarChar, 10).Value = (Request.QueryString["Band"].Equals("0") || (Request.QueryString["Band"].Length == 0)) ? string.Empty : Request.QueryString["Band"];
        cmdSelectTitle.Parameters.Add("@Approach", SqlDbType.VarChar, 10).Value = (Request.QueryString["App"].Equals("0") || (Request.QueryString["App"].Length == 0)) ? string.Empty : Request.QueryString["App"];
        cmdSelectTitle.Parameters.Add("@Category", SqlDbType.VarChar, 10).Value = (Request.QueryString["Cat"].Equals("0") || (Request.QueryString["Cat"].Length == 0)) ? string.Empty : Request.QueryString["Cat"];
        cmdSelectTitle.Parameters.Add("@BandSize", SqlDbType.VarChar, 10).Value = (Request.QueryString["Size"].Equals("0") || (Request.QueryString["Size"].Length == 0)) ? string.Empty : Request.QueryString["Size"];
        
        dsReportTitle = gClass.FetchData(cmdSelectTitle, "tblReportTitle");

        titleSurgeon = (dsReportTitle.Tables[0].Rows[0]["SurgeonTitle"].ToString() != "" ? dsReportTitle.Tables[0].Rows[0]["SurgeonTitle"].ToString() : "All");
        titleHospital = (dsReportTitle.Tables[0].Rows[0]["HospitalTitle"].ToString() != "" ? dsReportTitle.Tables[0].Rows[0]["HospitalTitle"].ToString() : "All");
        titleRegion = (dsReportTitle.Tables[0].Rows[0]["RegionTitle"].ToString() != "" ? dsReportTitle.Tables[0].Rows[0]["RegionTitle"].ToString() : "All");
        titleOperation = (dsReportTitle.Tables[0].Rows[0]["SurgeryTitle"].ToString() != "" ? dsReportTitle.Tables[0].Rows[0]["SurgeryTitle"].ToString() : "All");
        titleApproach = (dsReportTitle.Tables[0].Rows[0]["ApproachTitle"].ToString() != "" ? dsReportTitle.Tables[0].Rows[0]["ApproachTitle"].ToString() : "All");
        titleCategory = (dsReportTitle.Tables[0].Rows[0]["CategoryTitle"].ToString() != "" ? dsReportTitle.Tables[0].Rows[0]["CategoryTitle"].ToString() : "All");
        titleGroup = (dsReportTitle.Tables[0].Rows[0]["GroupTitle"].ToString() != "" ? dsReportTitle.Tables[0].Rows[0]["GroupTitle"].ToString() : "All");
        titleBandType = (dsReportTitle.Tables[0].Rows[0]["BandTypeTitle"].ToString() != "" ? dsReportTitle.Tables[0].Rows[0]["BandTypeTitle"].ToString() : "All");
        titleBandSize = (dsReportTitle.Tables[0].Rows[0]["BandSizeTitle"].ToString() != "" ? dsReportTitle.Tables[0].Rows[0]["BandSizeTitle"].ToString() : "All");
        titleSerialNo = (Request.QueryString["SerNo"].Trim() != "" ? Request.QueryString["SerNo"] : "All");
    }
    #endregion 

    #region Complication Summary with RDL Builder
    #region private void ComplicationSummary_BuildReport(int intReportCode)
    private void ComplicationSummary_BuildReport( )
    {
        DataSet     dsReport;
        DataTable   dtTemp;
        SqlCommand  cmdSelect = new SqlCommand();
        strXSLTFileName = "GroupReport/Complications/ComplicationSummaryXSLTFile.xsl";
        Int16       xh = 0;

        InitializeReportCommand(ref cmdSelect, "sp_Rep_Complications");
        dsReport = gClass.FetchData(cmdSelect, "tblComplicationSummary");
        dtTemp = dsReport.Tables["tblComplicationSummary"];
        gClass.AddColumn(ref dtTemp, "ReportDate", "System.String", DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToShortTimeString());

        dtTemp = new DataTable("tblComplication");
        gClass.AddColumn(ref dtTemp, "CompType", "System.String", "");
        gClass.AddColumn(ref dtTemp, "Complication", "System.String", "");
        dtTemp.Constraints.Add(new UniqueConstraint(dtTemp.Columns[0]));
        foreach (DataRow dr in dsReport.Tables["tblComplicationSummary"].Rows)
        {
            DataRow drTemp = dtTemp.NewRow();
            drTemp["CompType"] = dr["CompType"].ToString();
            drTemp["Complication"] = dr["Complication"].ToString();
            try { dtTemp.Rows.Add(drTemp); }
            catch { }
        }
        dsReport.Tables.Add(dtTemp);

        dtTemp = new DataTable("tblSurgery");
        gClass.AddColumn(ref dtTemp, "CompType", "System.String", "");
        gClass.AddColumn(ref dtTemp, "ComplicationCode", "System.String", "");
        gClass.AddColumn(ref dtTemp, "SurgeryType", "System.String", "");
        gClass.AddColumn(ref dtTemp, "SurgeryType_Desc", "System.String", "");
        DataColumn[] _Columns = new DataColumn[dtTemp.Columns.Count];
        xh = 0; foreach (DataColumn dc in dtTemp.Columns) _Columns[xh++] = dc;
        dtTemp.Constraints.Add(new UniqueConstraint(_Columns));
        foreach (DataRow dr in dsReport.Tables["tblComplicationSummary"].Rows)
        {
            DataRow drTemp = dtTemp.NewRow();

            drTemp["CompType"] = dr["CompType"].ToString();
            drTemp["ComplicationCode"] = dr["ComplicationCode"].ToString();
            drTemp["SurgeryType"] = dr["SurgeryType"].ToString();
            drTemp["SurgeryType_Desc"] = dr["SurgeryType_Desc"].ToString();
            try { dtTemp.Rows.Add(drTemp); }
            catch { }
        }
        dsReport.Tables.Add(dtTemp);

        xh = 0;
        dtTemp = new DataTable("tblSurgery_Comp");
        gClass.AddColumn(ref dtTemp, "ComplicationCode", "System.String", "");
        gClass.AddColumn(ref dtTemp, "SurgeryType", "System.String", "");
        gClass.AddColumn(ref dtTemp, "Complication", "System.String", "");
        _Columns = new DataColumn[dtTemp.Columns.Count];
        xh = 0; foreach (DataColumn dc in dtTemp.Columns) _Columns[xh++] = dc;
        dtTemp.Constraints.Add(new UniqueConstraint(_Columns));
        foreach (DataRow dr in dsReport.Tables["tblComplicationSummary"].Rows)
        {
            DataRow drTemp = dtTemp.NewRow();
            drTemp["ComplicationCode"] = dr["ComplicationCode"].ToString();
            drTemp["SurgeryType"] = dr["SurgeryType"].ToString();
            drTemp["Complication"] = dr["Complication"].ToString();
            try { dtTemp.Rows.Add(drTemp); }
            catch { }
        }
        dsReport.Tables.Add(dtTemp);
        dsReport.AcceptChanges();

        //tcXML.InnerHtml += gClass.ShowSchema(dsReport, Server.MapPath(strXSLTFileName));
        CreateOutputFile("COMPSUM", dsReport);
    }
    #endregion 

    #region private void ComplicationSummary_RDL(DataSet dsReport, string strRDLFilename, string strLanguage)
    private void ComplicationSummary_RDL(DataSet dsReport, string strRDLFilename, string strLanguage)
    {
        System.IO.FileStream stream = System.IO.File.OpenWrite(strRDLFilename);
        System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(stream, System.Text.Encoding.UTF8);

        writer.Formatting = System.Xml.Formatting.Indented;
        writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"utf-8\"");
        writer.WriteStartElement("Report");     //<Report>
        AddReportConfiguration(ref writer, "Complication Summary", strLanguage, (Decimal)29, (Decimal)0.5, (Decimal)0.5, (Decimal)0.5, (Decimal)0.5, (Decimal)21, "cm");
        AddDataSource(ref writer, dsReport);
        AddDataSets(ref writer, dsReport);
        #region body
        writer.WriteStartElement("Body");   //<Body>
        writer.WriteElementString("Height", "28cm"); //Report.height - (TopMargin + bottomMargin) == 28 - (0.5+0.5)
        writer.WriteStartElement("ReportItems");//<ReportItems>
        writer.WriteStartElement("Table");          //<Table>
        writer.WriteAttributeString("Name", "tblPageHeader");
        writer.WriteElementString("DataSetName", "tblComplicationSummary");

        #region TableColumns
        writer.WriteStartElement("TableColumns");   //<TableColumns>
        AddColumn(ref writer, "2cm");
        AddColumn(ref writer, "6cm");
        AddColumn(ref writer, "1.5cm");
        AddColumn(ref writer, "2cm");
        AddColumn(ref writer, "3cm");
        AddColumn(ref writer, "1cm");
        AddColumn(ref writer, "2cm");
        writer.WriteEndElement();                   //<TableColumns>
        #endregion 

        #region Header
        writer.WriteStartElement("Header");             //<Header>
        writer.WriteElementString("RepeatOnNewPage", "true");
        #region TableRows
        writer.WriteStartElement("TableRows");  //<TableRows>
        #region Row0
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Image", "imgTitle", Request.Url.Scheme + "://" + Request.Url.Host + Request.ApplicationPath + "/img/print_header_bar.gif", DetailCellStyle("", "", "", ""), "7");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion
        #region Row1
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "lblSurgeon", "Surgeon", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVSurgeon", titleSurgeon, DetailCellStyle("", "", "", ""), "2");
        AddCell(ref writer, "Textbox", "lblReportTitle", "- COMPLICATIONS", DetailCellStyle("", "12pt", "", ""), "4");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion
        #region Row2
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "lblHospital", "Hospital", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVHospital", titleHospital, DetailCellStyle("", "", "", ""), "2");
        AddCell(ref writer, "Textbox", "lblReportSubTitle", "- READMISSIONS", DetailCellStyle("", "12pt", "", ""), "4");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion
        #region Row21
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "lblRegion", "Region", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVRegion", titleRegion, DetailCellStyle("", "", "", ""), "6");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion
        #region Row3
        writer.WriteStartElement("TableRow");       //<TableRow>
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");     //  <TableCells>
        AddCell(ref writer, "Textbox", "txtTDate", "Date", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTDate", titleDate, DetailCellStyle("", "", "", ""), "2");
        AddCell(ref writer, "Textbox", "txtReportDate", "=Fields!ReportDate.Value", DetailCellStyle("Blue", "", "", ""), "4");
        writer.WriteEndElement();                   //  </TableCells>
        writer.WriteEndElement();                   //</TableRow>
        #endregion

        #region Row31
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTAge", "Age", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTAge", titleAge, DetailCellStyle("", "", "", ""), "6");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row32
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTBMI", "BMI", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTBMI", titleBMI, DetailCellStyle("", "", "", ""), "6");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 
        
        #region Row33
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTOperation", "Operation", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTOperation", titleOperation, DetailCellStyle("", "", "", ""), "6");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row34
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTApproach", "Approach", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTApproach", titleApproach, DetailCellStyle("", "", "", ""), "6");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row35
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTCategory", "Category", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTCategory", titleCategory, DetailCellStyle("", "", "", ""), "6");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row36
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTGroup", "Group", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTGroup", titleGroup, DetailCellStyle("", "", "", ""), "6");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row37
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTBandType", "Band Type", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTBandType", titleBandType, DetailCellStyle("", "", "", ""), "6");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row38
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTBandSize", "Band Size", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTBandSize", titleBandSize, DetailCellStyle("", "", "", ""), "6");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row4
        writer.WriteStartElement("TableRow");           //<TableRow>
        writer.WriteElementString("Height", "0.001cm");
        writer.WriteStartElement("TableCells");         //  <TableCells>
        AddCell(ref writer, "Line", "Line1", "", DetailCellStyle("", "", "", ""), "7");
        writer.WriteEndElement();                       //  </TableCells>
        writer.WriteEndElement();                       //</TableRow>
        #endregion
        writer.WriteEndElement();               //</TableRows>
        #endregion
        writer.WriteEndElement();                       //</Header>
        #endregion 

        #region <TableGroups>
        writer.WriteStartElement("TableGroups");    //<TableGroups>

        writer.WriteStartElement("TableGroup");         //<TableGroup>  --  Complication Type
        writer.WriteStartElement("Grouping");               //<Grouping>
        writer.WriteAttributeString("Name", "rptComplicationTypeGroup");
        writer.WriteStartElement("GroupExpressions");           //<GroupExpressions>
        writer.WriteElementString("GroupExpression", "=Fields!CompType.Value");
        writer.WriteEndElement();                               //</GroupExpressions>
        writer.WriteEndElement();                           //</Grouping>
        writer.WriteStartElement("Header");                 //<Header>
        writer.WriteStartElement("TableRows");                  //<TableRows>
        writer.WriteStartElement("TableRow");                       //<TableRow>
        writer.WriteElementString("Height", "0.7cm");
        writer.WriteStartElement("TableCells");                         //<TableCells>
        AddCell(ref writer, "Textbox", "lblCompType", "=Fields!CompType.value", DetailCellStyle("", "11pt", "", "LightSkyBlue"), "2");
        AddCell(ref writer, "Textbox", "lblEmpty2", "", DetailCellStyle("", "", "", "LightSkyBlue"), "5");
        writer.WriteEndElement();                                       //</TableCells>
        writer.WriteEndElement();                                   //</TableRow>
        writer.WriteEndElement();                               //</TableRows>
        writer.WriteEndElement();                           //</Header>
        writer.WriteEndElement();                       //</TableGroup>

        writer.WriteStartElement("TableGroup");         //<TableGroup>  --  Surgery Type
        writer.WriteStartElement("Grouping");               //<Grouping>
        writer.WriteAttributeString("Name", "rptSurgeryTypeGroup");
        writer.WriteStartElement("GroupExpressions");           //<GroupExpressions>
        writer.WriteElementString("GroupExpression", "=Fields!SurgeryType_Desc.Value");
        writer.WriteEndElement();                               //</GroupExpressions>
        writer.WriteEndElement();                           //</Grouping>
        writer.WriteStartElement("Header");                 //<Header>
        writer.WriteStartElement("TableRows");                  //<TableRows>
        writer.WriteStartElement("TableRow");                       //<TableRow>
        writer.WriteElementString("Height", "0.7cm");
        writer.WriteStartElement("TableCells");                         //<TableCells>
        AddCell(ref writer, "Textbox", "lblSurgery", "=Fields!SurgeryType_Desc.Value", DetailCellStyle("", "10pt", "", ""), "2");
        AddCell(ref writer, "Textbox", "lblEmpty22", "", DetailCellStyle("", "", "", ""), "5");
        writer.WriteEndElement();                                       //</TableCells>
        writer.WriteEndElement();                                   //</TableRow>
        writer.WriteEndElement();                               //</TableRows>
        writer.WriteEndElement();                           //</Header>
        writer.WriteEndElement();                       //</TableGroup>

        writer.WriteStartElement("TableGroup");         //<TableGroup>  --  Complication
        writer.WriteStartElement("Grouping");               //<Grouping>
        writer.WriteAttributeString("Name", "rptComplicationGroup");
        writer.WriteStartElement("GroupExpressions");           //<GroupExpressions>
        writer.WriteElementString("GroupExpression", "=Fields!Complication.Value");
        writer.WriteEndElement();                               //</GroupExpressions>
        writer.WriteEndElement();                           //</Grouping>
        writer.WriteStartElement("Header");                 //<Header>
        writer.WriteStartElement("TableRows");                  //<TableRows>
        writer.WriteStartElement("TableRow");                       //<TableRow>
        writer.WriteElementString("Height", "0.7cm");
        writer.WriteStartElement("TableCells");                         //<TableCells>
        AddCell(ref writer, "Textbox", "lblEmpty31", "", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtComplication", "=Fields!Complication.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "lblNumber", "Number", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtNumber", "=count(Fields!Complication.Value)", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "lblAvgWeeksPostOp", "Avg weeks post Op.", DetailCellStyle("Red", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtAvgWeeks", "=round(avg(Fields!Weeks.Value))", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "lblEmpty32", "", DetailCellStyle("", "", "", ""), "");
        writer.WriteEndElement();                                       //</TableCells>
        writer.WriteEndElement();                                   //</TableRow>

        writer.WriteStartElement("TableRow");                       //<TableRow>
        writer.WriteElementString("Height", "0.7cm");
        writer.WriteStartElement("TableCells");                         //<TableCells>
        AddCell(ref writer, "Textbox", "lblEmpty41", "", DetailCellStyle("", "", "", ""), "4");
        AddCell(ref writer, "Textbox", "lblReadmissions", "Readmissions", DetailCellStyle("Red", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtSumReadMitted", "=sum(iif(Fields!Readmitted.Value, 1, 0))", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "lblEmpty42", "", DetailCellStyle("", "", "", ""), "");
        writer.WriteEndElement();                                       //</TableCells>
        writer.WriteEndElement();                                   //</TableRow>
        writer.WriteEndElement();                               //</TableRows>
        writer.WriteEndElement();                           //</Header>
        writer.WriteEndElement();                       //</TableGroup>
        writer.WriteEndElement();                   //</TableGroups>
        #endregion 

        #region <Footer>
        writer.WriteStartElement("Footer");         //<Footer>
        writer.WriteElementString("RepeatOnNewPage", "false");
        writer.WriteStartElement("TableRows");      //  <TableRows>
        writer.WriteStartElement("TableRow");       //      <TableRow>
        writer.WriteElementString("Height", "1cm");
        writer.WriteStartElement("TableCells");     //          <TableCells>
        AddCell(ref writer, "Textbox", "lblEmptyFooter", "", DetailCellStyle("", "", "", ""), "6");
        AddCell(ref writer, "Image", "imgLogo", Request.Url.Scheme + "://" + Request.Url.Host + Request.ApplicationPath + "/img/banner_sml.gif", DetailCellStyle("", "", "", ""), "");
        writer.WriteEndElement();     //                       </TableCells>
        writer.WriteEndElement();     //                    </TableRow>
        writer.WriteEndElement();     //                </TableRows>
        writer.WriteEndElement();                  //</Footer>
        #endregion 
        
        writer.WriteEndElement();                   //<Table>
        writer.WriteEndElement();           //ReportItems
        writer.WriteEndElement();       //</Body>
        #endregion
        writer.WriteEndElement();               //</Report>
        writer.Flush();
        stream.Close();
    }
    #endregion
    #endregion

    #region Complication Summary By Patient with RDL Builder
    #region private void ComplicationSummaryByPatient_BuildReport( )
    private void ComplicationSummaryByPatient_BuildReport()
    {
        DataSet     dsReport;
        DataTable   dtTemp;
        SqlCommand  cmdSelect = new SqlCommand();
        Int16       xh = 0;

        strXSLTFileName = "GroupReport/Complications/ComplicationSummaryByPatientXSLTFile.xsl";
        InitializeReportCommand(ref cmdSelect, "sp_Rep_Complications_ByPatient");
        dsReport = gClass.FetchData(cmdSelect, "tblComplicationSummary");
        dtTemp = dsReport.Tables["tblComplicationSummary"];


        DataColumn dcTemp = new DataColumn();
        dcTemp.DataType = Type.GetType("System.String");
        dcTemp.DefaultValue = 0;
        dcTemp.ColumnName = "AdverseSurgeryDesc";
        dcTemp.Caption = "AdverseSurgeryDesc";
        dsReport.Tables[0].Columns.Add(dcTemp);

        for (int Idx = 0; Idx < dsReport.Tables[0].Rows.Count; Idx++)
        {
            dsReport.Tables[0].Rows[Idx]["AdverseSurgeryDesc"] = splitEventSurgery(dsReport.Tables[0].Rows[Idx]["AdverseSurgery"].ToString());
        }        
        dsReport.Tables[0].AcceptChanges();

        gClass.AddColumn(ref dtTemp, "ReportDate", "System.String", DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToShortTimeString());


        dtTemp = new DataTable("tblComplication");
        gClass.AddColumn(ref dtTemp, "CompType", "System.String", "");
        gClass.AddColumn(ref dtTemp, "Complication", "System.String", "");
        gClass.AddColumn(ref dtTemp, "ReportDate", "System.String", "");
        dtTemp.Constraints.Add(new UniqueConstraint(dtTemp.Columns[0]));
        foreach (DataRow dr in dsReport.Tables["tblComplicationSummary"].Rows)
        {
            DataRow drTemp = dtTemp.NewRow();
            drTemp["CompType"] = dr["CompType"].ToString();
            drTemp["Complication"] = dr["Complication"].ToString();
            drTemp["ReportDate"] = dr["ReportDate"].ToString();
            try { dtTemp.Rows.Add(drTemp); }
            catch { }
        }
        dsReport.Tables.Add(dtTemp);


        dtTemp = new DataTable("tblDoctors");
        gClass.AddColumn(ref dtTemp, "CompType", "System.String", "");
        gClass.AddColumn(ref dtTemp, "SurgeonId", "System.String", "");
        gClass.AddColumn(ref dtTemp, "DoctorName", "System.String", "");
        DataColumn[] _Columns = new DataColumn[dtTemp.Columns.Count];
        xh = 0; foreach (DataColumn dc in dtTemp.Columns) _Columns[xh++] = dc;
        dtTemp.Constraints.Add(new UniqueConstraint(_Columns));
        foreach (DataRow dr in dsReport.Tables["tblComplicationSummary"].Rows)
        {
            DataRow drTemp = dtTemp.NewRow();

            drTemp["CompType"] = dr["CompType"].ToString();
            drTemp["SurgeonId"] = dr["SurgeonId"].ToString();
            drTemp["DoctorName"] = dr["DoctorName"].ToString();
            try { dtTemp.Rows.Add(drTemp); }
            catch { }
        }
        dsReport.Tables.Add(dtTemp);

        dtTemp = new DataTable("tblSurgery");
        gClass.AddColumn(ref dtTemp, "CompType", "System.String", "");
        gClass.AddColumn(ref dtTemp, "SurgeonId", "System.String", "");
        gClass.AddColumn(ref dtTemp, "SurgeryType", "System.String", "");
        gClass.AddColumn(ref dtTemp, "SurgeryType_Desc", "System.String", "");
        _Columns = new DataColumn[dtTemp.Columns.Count];
        xh = 0; foreach (DataColumn dc in dtTemp.Columns) _Columns[xh++] = dc;
        dtTemp.Constraints.Add(new UniqueConstraint(_Columns));
        foreach (DataRow dr in dsReport.Tables["tblComplicationSummary"].Rows)
        {
            DataRow drTemp = dtTemp.NewRow();

            drTemp["CompType"] = dr["CompType"].ToString();
            drTemp["SurgeonId"] = dr["SurgeonId"].ToString();
            drTemp["SurgeryType"] = dr["SurgeryType"].ToString();
            drTemp["SurgeryType_Desc"] = dr["SurgeryType_Desc"].ToString();
            try { dtTemp.Rows.Add(drTemp); }
            catch { }
        }
        dsReport.Tables.Add(dtTemp);


        dtTemp = new DataTable("tblComplication_Details");
        gClass.AddColumn(ref dtTemp, "CompType", "System.String", "");
        gClass.AddColumn(ref dtTemp, "SurgeonId", "System.String", "");
        gClass.AddColumn(ref dtTemp, "SurgeryType", "System.String", "");
        gClass.AddColumn(ref dtTemp, "ComplicationCode", "System.String", "");
        gClass.AddColumn(ref dtTemp, "Complication", "System.String", "");
        gClass.AddColumn(ref dtTemp, "AdverseSurgery", "System.String", "");
        _Columns = new DataColumn[dtTemp.Columns.Count];
        xh = 0; foreach (DataColumn dc in dtTemp.Columns) _Columns[xh++] = dc;
        dtTemp.Constraints.Add(new UniqueConstraint(_Columns));
        foreach (DataRow dr in dsReport.Tables["tblComplicationSummary"].Rows)
        {
            DataRow drTemp = dtTemp.NewRow();

            drTemp["CompType"] = dr["CompType"].ToString();
            drTemp["SurgeonId"] = dr["SurgeonId"].ToString();
            drTemp["SurgeryType"] = dr["SurgeryType"].ToString();
            drTemp["ComplicationCode"] = dr["ComplicationCode"].ToString();
            drTemp["Complication"] = dr["Complication"].ToString();
            try { dtTemp.Rows.Add(drTemp); }
            catch { }
        }
        dsReport.Tables.Add(dtTemp);

        //tcXML.InnerHtml += gClass.ShowSchema(dsReport, Server.MapPath(strXSLTFileName));
        CreateOutputFile("COMPSUMBYPATIENT", dsReport);
    }
    #endregion 

    private void ComplicationSummaryByPatient_RDL(DataSet dsReport, string strRDLFilename, string strLanguage)
    {
        System.IO.FileStream stream = System.IO.File.OpenWrite(strRDLFilename);
        System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(stream, System.Text.Encoding.UTF8);

        writer.Formatting = System.Xml.Formatting.Indented;
        writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"utf-8\"");
        writer.WriteStartElement("Report");     //<Report>
        AddReportConfiguration(ref writer, "Operation Duration with LOS", strLanguage, (Decimal)29, (Decimal)0.5, (Decimal)0.5, (Decimal)0.5, (Decimal)0.5, (Decimal)21, "cm");
        AddDataSource(ref writer, dsReport);
        AddDataSets(ref writer, dsReport);
        
        #region body
        writer.WriteStartElement("Body");   //<Body>
        writer.WriteElementString("Height", "28cm"); //Report.height - (TopMargin + bottomMargin) == 28 - (0.5+0.5)
        writer.WriteStartElement("ReportItems");//<ReportItems>
        writer.WriteStartElement("Table");          //<Table>
        writer.WriteAttributeString("Name", "tblPageHeader");
        writer.WriteElementString("DataSetName", "tblComplicationSummary");

        #region TableColumns
        writer.WriteStartElement("TableColumns");   //<TableColumns>
        AddColumn(ref writer, "1.5cm");
        AddColumn(ref writer, "4cm");
        AddColumn(ref writer, "1cm");
        AddColumn(ref writer, "1cm");
        AddColumn(ref writer, "1cm");
        AddColumn(ref writer, "1cm");
        AddColumn(ref writer, "2cm");
        AddColumn(ref writer, "2.5cm");
        AddColumn(ref writer, "2cm");
        AddColumn(ref writer, "2.5cm");
        AddColumn(ref writer, "0.8cm");
        AddColumn(ref writer, "1cm");
        AddColumn(ref writer, "2.5cm");
        AddColumn(ref writer, "5cm");
        writer.WriteEndElement();                   //<TableColumns>
        #endregion 

        #region Header
        writer.WriteStartElement("Header");             //<Header>
        writer.WriteElementString("RepeatOnNewPage", "true");
        #region TableRows
        writer.WriteStartElement("TableRows");  //<TableRows>

        #region Row0
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Image", "imgTitle", Request.Url.Scheme + "://" + Request.Url.Host + Request.ApplicationPath + "/img/print_header_bar.gif", DetailCellStyle("", "", "", ""), "14");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row1
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtSurgeon", "Surgeon", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVSurgeon", titleSurgeon, DetailCellStyle("", "", "", ""), "6");
        AddCell(ref writer, "Textbox", "txtReportTitle", "Complication", DetailCellStyle("", "12pt", "", ""), "7");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion
        #region Row2
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtHospital", "Hospital", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVHospital", titleHospital, DetailCellStyle("", "", "", ""), "13");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion
        #region Row21
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtRegion", "Region", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVRegion", titleRegion, DetailCellStyle("", "", "", ""), "13");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion
        #region Row3
        writer.WriteStartElement("TableRow");       //<TableRow>
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");     //  <TableCells>
        AddCell(ref writer, "Textbox", "txtTDate", "Date", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTDate", titleDate, DetailCellStyle("", "", "", ""), "6");
        AddCell(ref writer, "Textbox", "txtReportDate", "=Fields!ReportDate.Value", DetailCellStyle("Blue", "", "", ""), "7");
        writer.WriteEndElement();                   //  </TableCells>
        writer.WriteEndElement();                   //</TableRow>
        #endregion
        
        #region Row31
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTAge", "Age", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTAge", titleAge, DetailCellStyle("", "", "", ""), "13");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row32
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTBMI", "BMI", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTBMI", titleBMI, DetailCellStyle("", "", "", ""), "13");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 
        
        #region Row33
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTOperation", "Operation", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTOperation", titleOperation, DetailCellStyle("", "", "", ""), "13");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row34
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTApproach", "Approach", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTApproach", titleApproach, DetailCellStyle("", "", "", ""), "13");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row35
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTCategory", "Category", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTCategory", titleCategory, DetailCellStyle("", "", "", ""), "13");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row36
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTGroup", "Group", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTGroup", titleGroup, DetailCellStyle("", "", "", ""), "13");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row37
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTBandType", "Band Type", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTBandType", titleBandType, DetailCellStyle("", "", "", ""), "13");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row38
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTBandSize", "Band Size", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTBandSize", titleBandSize, DetailCellStyle("", "", "", ""), "13");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row4
        writer.WriteStartElement("TableRow");           //<TableRow>
        writer.WriteElementString("Height", "0.001cm");
        writer.WriteStartElement("TableCells");         //  <TableCells>
        AddCell(ref writer, "Line", "Line1", "", DetailCellStyle("", "", "", ""), "14");
        writer.WriteEndElement();                       //  </TableCells>
        writer.WriteEndElement();                       //</TableRow>
        #endregion    

        writer.WriteEndElement();               //</TableRows>
        #endregion
        writer.WriteEndElement();                       //</Header>
        #endregion 

        #region <TableGroups>
        writer.WriteStartElement("TableGroups");    //<TableGroups>

        writer.WriteStartElement("TableGroup");         //<TableGroup>  --  Complication Type
        writer.WriteStartElement("Grouping");               //<Grouping>
        writer.WriteAttributeString("Name", "rptComplicationTypeGroup");
        writer.WriteStartElement("GroupExpressions");           //<GroupExpressions>
        writer.WriteElementString("GroupExpression", "=Fields!CompType.Value");
        writer.WriteEndElement();                               //</GroupExpressions>
        writer.WriteEndElement();                           //</Grouping>
        //writer.WriteStartElement("Header");                 //<Header>
        //writer.WriteStartElement("TableRows");                  //<TableRows>
        //writer.WriteStartElement("TableRow");                       //<TableRow>
        //writer.WriteElementString("Height", "0.7cm");
        //writer.WriteStartElement("TableCells");                         //<TableCells>
        //AddCell(ref writer, "Textbox", "lblEmpty1", "", DetailCellStyle("", "", "", "LightSkyBlue"), "13");
        //writer.WriteEndElement();                                       //</TableCells>
        //writer.WriteEndElement();                                   //</TableRow>
        //writer.WriteEndElement();                               //</TableRows>
        //writer.WriteEndElement();                           //</Header>
        writer.WriteEndElement();                       //</TableGroup>

        writer.WriteStartElement("TableGroup");         //<TableGroup>  --  Doctor Name
        writer.WriteStartElement("Grouping");               //<Grouping>
        writer.WriteAttributeString("Name", "rptDoctorNameGroup");
        writer.WriteStartElement("GroupExpressions");           //<GroupExpressions>
        writer.WriteElementString("GroupExpression", "=Fields!DoctorName.Value");
        writer.WriteEndElement();                               //</GroupExpressions>
        writer.WriteEndElement();                           //</Grouping>
        writer.WriteStartElement("Header");                 //<Header>
        writer.WriteStartElement("TableRows");                  //<TableRows>
        writer.WriteStartElement("TableRow");                       //<TableRow>
        writer.WriteElementString("Height", "0.7cm");
        writer.WriteStartElement("TableCells");                         //<TableCells>
        AddCell(ref writer, "Textbox", "txtDoctorName", "=Fields!DoctorName.value", DetailCellStyle("", "12pt", "", "LightSkyBlue"), "14");
        writer.WriteEndElement();                                       //</TableCells>
        writer.WriteEndElement();                                   //</TableRow>
        writer.WriteEndElement();                               //</TableRows>
        writer.WriteEndElement();                           //</Header>
        writer.WriteEndElement();                       //</TableGroup>

        writer.WriteStartElement("TableGroup");         //<TableGroup>  --  Surgery Type
        writer.WriteStartElement("Grouping");               //<Grouping>
        writer.WriteAttributeString("Name", "rptSurgeryTypeGroup");
        writer.WriteStartElement("GroupExpressions");           //<GroupExpressions>
        writer.WriteElementString("GroupExpression", "=Fields!SurgeryType_Desc.Value");
        writer.WriteEndElement();                               //</GroupExpressions>
        writer.WriteEndElement();                           //</Grouping>
        writer.WriteStartElement("Header");                 //<Header>
        writer.WriteStartElement("TableRows");                  //<TableRows>
        writer.WriteStartElement("TableRow");                       //<TableRow>
        writer.WriteElementString("Height", "0.7cm");
        writer.WriteStartElement("TableCells");                         //<TableCells>
        AddCell(ref writer, "Textbox", "lblSurgery", "=Fields!SurgeryType_Desc.Value", DetailCellStyle("", "11pt", "", "LightSkyBlue"), "14");
        writer.WriteEndElement();                                       //</TableCells>
        writer.WriteEndElement();                                   //</TableRow>
        writer.WriteEndElement();                               //</TableRows>
        writer.WriteEndElement();                           //</Header>
        writer.WriteEndElement();                       //</TableGroup>

        writer.WriteStartElement("TableGroup");         //<TableGroup>  --  Complication
        writer.WriteStartElement("Grouping");               //<Grouping>
        writer.WriteAttributeString("Name", "rptComplicationGroup");
        writer.WriteStartElement("GroupExpressions");           //<GroupExpressions>
        writer.WriteElementString("GroupExpression", "=Fields!Complication.Value");
        writer.WriteEndElement();                               //</GroupExpressions>
        writer.WriteEndElement();                           //</Grouping>
        writer.WriteStartElement("Header");                 //<Header>
        writer.WriteStartElement("TableRows");                  //<TableRows>
        writer.WriteStartElement("TableRow");                       //<TableRow>
        writer.WriteElementString("Height", "0.7cm");
        writer.WriteStartElement("TableCells");                         //<TableCells>
        AddCell(ref writer, "Textbox", "txtComplication", "=Fields!Complication.Value", DetailCellStyle("", "10pt", "", "LightGrey"), "4");
        AddCell(ref writer, "Textbox", "lblNumber", "Total Events:", DetailCellStyle("", "", "", "LightGrey"), "2");
        AddCell(ref writer, "Textbox", "txtNumber", "=count(Fields!Complication.Value)", DetailCellStyle("", "", "", "LightGrey"), "8");
        writer.WriteEndElement();                                       //</TableCells>
        writer.WriteEndElement();                                   //</TableRow>
        writer.WriteEndElement();                               //</TableRows>
        writer.WriteEndElement();                           //</Header>
        writer.WriteEndElement();                       //</TableGroup>

        writer.WriteStartElement("TableGroup");         //<TableGroup>  --  PatientID
        writer.WriteStartElement("Grouping");               //<Grouping>
        writer.WriteAttributeString("Name", "rptPatientIDGroup");
        writer.WriteStartElement("GroupExpressions");           //<GroupExpressions>
        writer.WriteElementString("GroupExpression", "=Fields!PatientId.Value");
        writer.WriteEndElement();                               //</GroupExpressions>
        writer.WriteEndElement();                           //</Grouping>
        writer.WriteStartElement("Header");                 //<Header>
        writer.WriteStartElement("TableRows");                  //<TableRows>
        writer.WriteStartElement("TableRow");                       //<TableRow>
        writer.WriteElementString("Height", "0.7cm");
        writer.WriteStartElement("TableCells");                         //<TableCells>
        AddCell(ref writer, "Textbox", "txtPatientName", "=Fields!PatientName.Value", DetailCellStyle("", "", "", ""), "2");
        AddCell(ref writer, "Textbox", "lblSex", "Sex : ", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtSex", "=Fields!Sex.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "lblAge", "Age : ", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtAge", "=Fields!Age.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "lblSurgeryDate", "Surgery Date :", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtSurgeryDate", "=Fields!OperationDate.Value", DetailCellStyle("", "", "", ""), "");

        AddCell(ref writer, "Textbox", "lblDate", "Events Date : ", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtComplicationDate", "=Fields!ComplicationDate.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtWeeks", "=Fields!Weeks.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "lblWeeks", "weeks", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "lblEventSurgery", "Events Surgery : ", DetailCellStyle("", "", "Right", ""), "");
        AddCell(ref writer, "Textbox", "txtComplicationSurgery", "=Fields!AdverseSurgeryDesc.Value", DetailCellStyle("", "", "", ""), "");

        writer.WriteEndElement();                                       //</TableCells>
        writer.WriteEndElement();                                   //</TableRow>
        writer.WriteEndElement();                               //</TablrRows>
        writer.WriteEndElement();                           //</Header>
        writer.WriteEndElement();                       //</TableGroup>

        writer.WriteEndElement();                   //</TableGroups>
        #endregion 

        #region <Footer>
        writer.WriteStartElement("Footer");         //<Footer>
        writer.WriteElementString("RepeatOnNewPage", "false");
        writer.WriteStartElement("TableRows");      //  <TableRows>
        writer.WriteStartElement("TableRow");       //      <TableRow>
        writer.WriteElementString("Height", "1cm");
        writer.WriteStartElement("TableCells");     //          <TableCells>
        AddCell(ref writer, "Textbox", "lblEmptyFooter", "", DetailCellStyle("", "", "", ""), "13");
        AddCell(ref writer, "Image", "imgLogo", Request.Url.Scheme + "://" + Request.Url.Host + Request.ApplicationPath + "/img/banner_sml.gif", DetailCellStyle("", "", "Right", ""), "");
        writer.WriteEndElement();     //                       </TableCells>
        writer.WriteEndElement();     //                    </TableRow>
        writer.WriteEndElement();     //                </TableRows>
        writer.WriteEndElement();                  //</Footer>
        #endregion 

        #region Deatails
        writer.WriteStartElement("Details");        //<Details>
        writer.WriteStartElement("TableRows");      //  <TableRows>
        writer.WriteStartElement("TableRow");       //      <TableRow>
        writer.WriteElementString("Height", "0.75cm");
        writer.WriteStartElement("TableCells");     //          <TableCells>

        AddCell(ref writer, "Textbox", "txtNote", "=Fields!ComplicationNotes.Value", DetailCellStyle("", "", "", ""), "13");
        AddCell(ref writer, "Textbox", "lblEmpty5", "", DetailCellStyle("", "", "", ""), "");

        writer.WriteEndElement();                   //          </TableCells>
        writer.WriteEndElement();                   //      </TableRow>
        writer.WriteEndElement();                   //  </TableRows>
        writer.WriteEndElement();                   //</Details>
        #endregion

        writer.WriteEndElement();                   //<Table>
        writer.WriteEndElement();           //ReportItems
        writer.WriteEndElement();       //</Body>
        #endregion

        writer.WriteEndElement();   //</Report>
        writer.Flush();
        stream.Close();
    }
    #region private String splitEventSurgery(string surgery){
        private String splitEventSurgery(string surgery)
        {
            String[] surgeryEvents;
            String returnValue = "";

            if (surgery.Trim() != "")
            {
                surgeryEvents = surgery.Split(';');

                foreach (string tempEvent in surgeryEvents)
                {
                    if (tempEvent != "")
                        returnValue += getDescription(tempEvent, "adevst") + ", ";
                }
                returnValue = returnValue.Substring(0, returnValue.Length - 2);
            }
            return returnValue;
        }
    #endregion
    #endregion

    #region private DataSet GetGraphTitle()
    private DataSet GetGraphTitle()
    {
        DataSet dsReport = new DataSet();
        SqlCommand cmdSelect = new SqlCommand();
        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_Rep_EWL_WLGraphFullPage", true);

        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSelect.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
        cmdSelect.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["PID"]);
        cmdSelect.Parameters.Add("@ImperialFlag", SqlDbType.Bit).Value = Request.Cookies["Imperial"].Value.Equals("True");

        dsReport = gClass.FetchData(cmdSelect, "tblReport");

        DataView dvReport = dsReport.Tables[0].DefaultView;

        titleGraphPatientName = dvReport[0]["PatientName"].ToString();
        titleGraphPatientAge = "Age : " + dvReport[0]["AGE"].ToString();

        if (dvReport[0]["VisitWeeksFlag"].ToString() == "3")
        {
            titleGraphPatientSurgeryDate = "Zero Date : " + dvReport[0]["strZeroDate"].ToString();
        }
        else
        {
            titleGraphPatientSurgeryDate = "Surgery Date : " + dvReport[0]["strLapBandDate"].ToString();
        }

        titleGraphPatientCurrentWeight = "Current Weight : " + Math.Round(Convert.ToDecimal(dvReport[0]["CurrentWeight"].ToString())).ToString() + " " + dsReport.Tables[0].Rows[0]["WeightMeasurment"].ToString();
        titleGraphPatientTargetWeight = "Target Weight : " + Math.Round(Convert.ToDecimal(dvReport[0]["TargetWeight"].ToString())).ToString() + " " + dsReport.Tables[0].Rows[0]["WeightMeasurment"].ToString();
        titleGraphPatientInitialBMI = "Initial BMI : " + dvReport[0]["InitBMI"].ToString();
        titleGraphPatientStartWeight = "Start Weight : " + Math.Round(Convert.ToDecimal(dvReport[0]["StartWeight"].ToString())).ToString() + " " + dsReport.Tables[0].Rows[0]["WeightMeasurment"].ToString();
        
        //get first visit date
        if (dvReport[0]["VisitWeeksFlag"].ToString() == "3")
        {
            titleGraphPatientFirstVisitDate = Convert.ToDateTime(dvReport[0]["strZeroDate"].ToString());
        }
        else
        {
            titleGraphPatientFirstVisitDate = Convert.ToDateTime(dvReport[0]["strLapBandDate"].ToString());
        }
        
        return dsReport;
    }
    #endregion

    #region IEWLG Report with RDL Builder
    #region private void IEWLG_BuildReport()
    private void IEWLG_BuildReport()
    {
        DataSet dsReport = new DataSet();
        dsReport = GetGraphTitle();

        int[] month = new int[] { 0, 3, 6, 9, 12, 18, 24 };
        Double[] idealWeightLoss = new Double[] { 0, 30, 38, 45, 51, 60, 66 };
        DateTime dtDateSeen = new DateTime();
        try
        {

            DataColumn dcTemp = new DataColumn();
            dcTemp.DataType = Type.GetType("System.String");
            dcTemp.DefaultValue = "";
            dcTemp.ColumnName = "strTargetDateSeen";
            dcTemp.Caption = "strTargetDateSeen";
            dsReport.Tables[0].Columns.Add(dcTemp);

            dcTemp = new DataColumn();
            dcTemp.DataType = Type.GetType("System.Double");
            dcTemp.DefaultValue = 0;
            dcTemp.ColumnName = "TargetEWL";
            dcTemp.Caption = "TargetEWL";
            dsReport.Tables[0].Columns.Add(dcTemp);

            if (dsReport.Tables["tblReport"].Rows.Count < month.Length)
            {
                for (int b = dsReport.Tables["tblReport"].Rows.Count; b < month.Length; b++)
                {
                    DataTable dtTemp = dsReport.Tables["tblReport"];
                    DataRow drTemp = dtTemp.NewRow();
                    drTemp["strDateSeen"] = 0;
                    drTemp["EWL"] = 0;
                    drTemp["strTargetDateSeen"] = 0;
                    drTemp["TargetEWL"] = 0;
                    try { dtTemp.Rows.Add(drTemp); }
                    catch (Exception err) { }
                }
            }
        }
        catch(Exception err)
        {
        }

        for (int b = 0; b < month.Length; b++)
        {
            try
            {
                dtDateSeen = titleGraphPatientFirstVisitDate.AddMonths(month[b]);
                dsReport.Tables["tblReport"].Rows[b]["strTargetDateSeen"] = gClass.TruncateDate(dtDateSeen.ToString(), Request.Cookies["CultureInfo"].Value, 3);
                dsReport.Tables["tblReport"].Rows[b]["TargetEWL"] = idealWeightLoss[b];
                
            }
            catch { }
        }
        CreateOutputFile("IEWLG", dsReport);
    }
    #endregion

    #region private void IEWLG_RDL(DataSet dsReport, string strRDLFilename, string strLanguage)
    private void IEWLG_RDL(DataSet dsReport, string strRDLFilename, string strLanguage)
    {
        try
        {
            System.IO.FileStream stream = System.IO.File.OpenWrite(strRDLFilename);
            System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(stream, System.Text.Encoding.UTF8);

            #region Report
            writer.Formatting = System.Xml.Formatting.Indented;
            writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"utf-8\"");
            writer.WriteStartElement("Report");     //<Report>
            AddReportConfiguration(ref writer, "IEWLG Graph", strLanguage, (Decimal)29, (Decimal)0.5, (Decimal)0.5, (Decimal)0.5, (Decimal)0.5, (Decimal)21, "cm");
            AddDataSource(ref writer, dsReport);
            AddDataSets(ref writer, dsReport);

            #region Body
            writer.WriteStartElement("Body");
            writer.WriteElementString("Height", "20cm"); //Report.height - (TopMargin + bottomMargin) == 29.7 - (0.5+0.5)
            writer.WriteStartElement("ReportItems");

            #region Table
            writer.WriteStartElement("Table");
            writer.WriteAttributeString("Name", "rptIEWLG");

            #region TableColumns
            writer.WriteStartElement("TableColumns");

            AddColumn(ref writer, "4cm");
            AddColumn(ref writer, "4cm");
            AddColumn(ref writer, "4cm");
            AddColumn(ref writer, "4cm");
            writer.WriteEndElement();
            #endregion

            #region Header
            writer.WriteStartElement("Header");
            writer.WriteElementString("RepeatOnNewPage", "true");

            #region TableRows
            writer.WriteStartElement("TableRows");

            #region Row0
            writer.WriteStartElement("TableRow");
            writer.WriteElementString("Height", "0.5cm");
            writer.WriteStartElement("TableCells");
            AddCell(ref writer, "Textbox", "txtHeader", "Target % Excess Weight Loss Chart", DetailCellStyle("", "", "", ""), "4");
            writer.WriteEndElement();
            writer.WriteEndElement();
            #endregion

            #region Row1
            writer.WriteStartElement("TableRow");
            writer.WriteElementString("Height", "0.5cm");
            writer.WriteStartElement("TableCells");
            AddCell(ref writer, "Textbox", "txtPatientName", titleGraphPatientName, DetailCellStyle("", "", "", ""), "4");
            writer.WriteEndElement();
            writer.WriteEndElement();
            #endregion

            #region Row2
            writer.WriteStartElement("TableRow");
            writer.WriteElementString("Height", "0.5cm");
            writer.WriteStartElement("TableCells");
            AddCell(ref writer, "Textbox", "txtPatientAge", titleGraphPatientAge, DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "txtPatientSurgeryDate", titleGraphPatientSurgeryDate, DetailCellStyle("", "", "", ""), "3");
            writer.WriteEndElement();
            writer.WriteEndElement();
            #endregion

            #region Row3
            writer.WriteStartElement("TableRow");
            writer.WriteElementString("Height", "0.5cm");
            writer.WriteStartElement("TableCells");
            AddCell(ref writer, "Textbox", "txtPatientStartWeight", titleGraphPatientStartWeight, DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "txtPatientInitialBMI", titleGraphPatientInitialBMI, DetailCellStyle("", "", "", ""), "3");
            writer.WriteEndElement();
            writer.WriteEndElement();
            #endregion

            #region Row4
            writer.WriteStartElement("TableRow");
            writer.WriteElementString("Height", "0.5cm");
            writer.WriteStartElement("TableCells");
            AddCell(ref writer, "Textbox", "txtPatientCurrentWeight", titleGraphPatientCurrentWeight, DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "txtPatientTargetWeight", titleGraphPatientTargetWeight, DetailCellStyle("", "", "", ""), "3");
            writer.WriteEndElement();
            writer.WriteEndElement();
            #endregion

            #region Row5
            writer.WriteStartElement("TableRow");           //<TableRow>
            writer.WriteElementString("Height", "0.5cm");
            writer.WriteStartElement("TableCells");         //  <TableCells>
            AddCell(ref writer, "Textbox", "txtBlankRow", "", DetailCellStyle("", "", "", ""), "4");
            writer.WriteEndElement();                       //  </TableCells>
            writer.WriteEndElement();                       //</TableRow>
            #endregion

            #region Row6
            writer.WriteStartElement("TableRow");           //<TableRow>
            writer.WriteElementString("Height", "0.001cm");
            writer.WriteStartElement("TableCells");         //  <TableCells>
            AddCell(ref writer, "Line", "Line1", "", DetailCellStyle("", "", "", ""), "4");
            writer.WriteEndElement();                       //  </TableCells>
            writer.WriteEndElement();                       //</TableRow>
            #endregion

            #region Row7
            writer.WriteStartElement("TableRow");
            writer.WriteElementString("Height", "0.5cm");
            writer.WriteStartElement("TableCells");

            AddCell(ref writer, "Textbox", "txtVisitDate_hr", "Visit Date", DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "txtEWL_hr", "Excess Weight Loss", DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "txtTargetVisitDate_hr", "Target Visit Date", DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "txtTargetEWL_hr", "Target Excess Weight Loss", DetailCellStyle("", "", "", ""), "");

            writer.WriteEndElement();
            writer.WriteEndElement();
            #endregion

            writer.WriteEndElement();
            #endregion
            writer.WriteEndElement();
            #endregion

            #region Details
            writer.WriteStartElement("Details");
            writer.WriteStartElement("TableRows");
            writer.WriteStartElement("TableRow");
            writer.WriteElementString("Height", "0.5cm");
            writer.WriteStartElement("TableCells");

            AddCell(ref writer, "Textbox", "txtVisitDate", "=Fields!strDateSeen.Value", DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "txtEWL", "=Fields!EWL.Value", DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "txtTargetVisitDate", "=Fields!strTargetDateSeen.Value", DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "txtTargetEWL", "=Fields!TargetEWL.Value", DetailCellStyle("", "", "", ""), "");

            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
            #endregion

            writer.WriteEndElement();
            #endregion

            writer.WriteEndElement();
            writer.WriteEndElement();
            #endregion

            writer.WriteEndElement();
            #endregion
            writer.Flush();
            stream.Close();
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value,
                                "IEWLG Report", "IEWLG", err.ToString());
        }
    }
    #endregion
    #endregion

    #region WLG Report with RDL Builder
    #region private void WLG_BuildReport()
    private void WLG_BuildReport()
    {
        DataSet dsReport = new DataSet();
        dsReport = GetGraphTitle();
        CreateOutputFile("WLG", dsReport);
    }
    #endregion

    #region private void WLG_RDL(DataSet dsReport, string strRDLFilename, string strLanguage)
    private void WLG_RDL(DataSet dsReport, string strRDLFilename, string strLanguage)
    {
        try
        {
            System.IO.FileStream stream = System.IO.File.OpenWrite(strRDLFilename);
            System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(stream, System.Text.Encoding.UTF8);

            #region Report
            writer.Formatting = System.Xml.Formatting.Indented;
            writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"utf-8\"");
            writer.WriteStartElement("Report");     //<Report>
            AddReportConfiguration(ref writer, "WL Graph", strLanguage, (Decimal)29, (Decimal)0.5, (Decimal)0.5, (Decimal)0.5, (Decimal)0.5, (Decimal)21, "cm");
            AddDataSource(ref writer, dsReport);
            AddDataSets(ref writer, dsReport);

            #region Body
            writer.WriteStartElement("Body");
            writer.WriteElementString("Height", "20cm"); //Report.height - (TopMargin + bottomMargin) == 29.7 - (0.5+0.5)
            writer.WriteStartElement("ReportItems");

            #region Table
            writer.WriteStartElement("Table");
            writer.WriteAttributeString("Name", "rptWLG");

            #region TableColumns
            writer.WriteStartElement("TableColumns");

            AddColumn(ref writer, "4cm");
            AddColumn(ref writer, "4cm");
            writer.WriteEndElement();
            #endregion

            #region Header
            writer.WriteStartElement("Header");
            writer.WriteElementString("RepeatOnNewPage", "true");

            #region TableRows
            writer.WriteStartElement("TableRows");

            #region Row0
            writer.WriteStartElement("TableRow");
            writer.WriteElementString("Height", "0.5cm");
            writer.WriteStartElement("TableCells");
            AddCell(ref writer, "Textbox", "txtHeader", "Weight Loss Chart", DetailCellStyle("", "", "", ""), "2");
            writer.WriteEndElement();
            writer.WriteEndElement();
            #endregion

            #region Row1
            writer.WriteStartElement("TableRow");
            writer.WriteElementString("Height", "0.5cm");
            writer.WriteStartElement("TableCells");
            AddCell(ref writer, "Textbox", "txtPatientName", titleGraphPatientName, DetailCellStyle("", "", "", ""), "2");
            writer.WriteEndElement();
            writer.WriteEndElement();
            #endregion

            #region Row2
            writer.WriteStartElement("TableRow");
            writer.WriteElementString("Height", "0.5cm");
            writer.WriteStartElement("TableCells");
            AddCell(ref writer, "Textbox", "txtPatientAge", titleGraphPatientAge, DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "txtPatientSurgeryDate", titleGraphPatientSurgeryDate, DetailCellStyle("", "", "", ""), "");
            writer.WriteEndElement();
            writer.WriteEndElement();
            #endregion

            #region Row3
            writer.WriteStartElement("TableRow");
            writer.WriteElementString("Height", "0.5cm");
            writer.WriteStartElement("TableCells");
            AddCell(ref writer, "Textbox", "txtPatientStartWeight", titleGraphPatientStartWeight, DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "txtPatientInitialBMI", titleGraphPatientInitialBMI, DetailCellStyle("", "", "", ""), "");
            writer.WriteEndElement();
            writer.WriteEndElement();
            #endregion

            #region Row4
            writer.WriteStartElement("TableRow");
            writer.WriteElementString("Height", "0.5cm");
            writer.WriteStartElement("TableCells");
            AddCell(ref writer, "Textbox", "txtPatientCurrentWeight", titleGraphPatientCurrentWeight, DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "txtPatientTargetWeight", titleGraphPatientTargetWeight, DetailCellStyle("", "", "", ""), "");
            writer.WriteEndElement();
            writer.WriteEndElement();
            #endregion

            #region Row5
            writer.WriteStartElement("TableRow");           //<TableRow>
            writer.WriteElementString("Height", "0.5cm");
            writer.WriteStartElement("TableCells");         //  <TableCells>
            AddCell(ref writer, "Textbox", "txtBlankRow", "", DetailCellStyle("", "", "", ""), "2");
            writer.WriteEndElement();                       //  </TableCells>
            writer.WriteEndElement();                       //</TableRow>
            #endregion

            #region Row6
            writer.WriteStartElement("TableRow");           //<TableRow>
            writer.WriteElementString("Height", "0.001cm");
            writer.WriteStartElement("TableCells");         //  <TableCells>
            AddCell(ref writer, "Line", "Line1", "", DetailCellStyle("", "", "", ""), "2");
            writer.WriteEndElement();                       //  </TableCells>
            writer.WriteEndElement();                       //</TableRow>
            #endregion

            #region Row7
            writer.WriteStartElement("TableRow");
            writer.WriteElementString("Height", "0.5cm");
            writer.WriteStartElement("TableCells");

            AddCell(ref writer, "Textbox", "txtVisitDate_hr", "Visit Date", DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "txtWL_hr", "Weight", DetailCellStyle("", "", "", ""), "");

            writer.WriteEndElement();
            writer.WriteEndElement();
            #endregion

            writer.WriteEndElement();
            #endregion
            writer.WriteEndElement();
            #endregion

            #region Details
            writer.WriteStartElement("Details");
            writer.WriteStartElement("TableRows");
            writer.WriteStartElement("TableRow");
            writer.WriteElementString("Height", "0.5cm");
            writer.WriteStartElement("TableCells");

            AddCell(ref writer, "Textbox", "txtVisitDate", "=Fields!strDateSeen.Value", DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "txtWL", "=Fields!Weight.Value", DetailCellStyle("", "", "", ""), "");

            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
            #endregion

            writer.WriteEndElement();
            #endregion

            writer.WriteEndElement();
            writer.WriteEndElement();
            #endregion

            writer.WriteEndElement();
            #endregion
            writer.Flush();
            stream.Close();
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value,
                                "WLG Report", "WLG", err.ToString());
        }
    }
    #endregion
    #endregion

    #region EWLG Report with RDL Builder
    #region private void EWLG_BuildReport()
    private void EWLG_BuildReport()
    {
        DataSet dsReport = new DataSet();
        dsReport = GetGraphTitle();
        CreateOutputFile("EWLG", dsReport);
    }
    #endregion

    #region private void EWLG_RDL(DataSet dsReport, string strRDLFilename, string strLanguage)
    private void EWLG_RDL(DataSet dsReport, string strRDLFilename, string strLanguage)
    {
        try
        {
            System.IO.FileStream stream = System.IO.File.OpenWrite(strRDLFilename);
            System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(stream, System.Text.Encoding.UTF8);

            #region Report
            writer.Formatting = System.Xml.Formatting.Indented;
            writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"utf-8\"");
            writer.WriteStartElement("Report");     //<Report>
            AddReportConfiguration(ref writer, "%EWL Graph", strLanguage, (Decimal)29, (Decimal)0.5, (Decimal)0.5, (Decimal)0.5, (Decimal)0.5, (Decimal)21, "cm");
            AddDataSource(ref writer, dsReport);
            AddDataSets(ref writer, dsReport);

            #region Body
            writer.WriteStartElement("Body");
            writer.WriteElementString("Height", "20cm"); //Report.height - (TopMargin + bottomMargin) == 29.7 - (0.5+0.5)
            writer.WriteStartElement("ReportItems");

            #region Table
            writer.WriteStartElement("Table");
            writer.WriteAttributeString("Name", "rptEWLG");

            #region TableColumns
            writer.WriteStartElement("TableColumns");

            AddColumn(ref writer, "4cm");
            AddColumn(ref writer, "4cm");
            writer.WriteEndElement();
            #endregion

            #region Header
            writer.WriteStartElement("Header");
            writer.WriteElementString("RepeatOnNewPage", "true");

            #region TableRows
            writer.WriteStartElement("TableRows");

            #region Row0
            writer.WriteStartElement("TableRow");
            writer.WriteElementString("Height", "0.5cm");
            writer.WriteStartElement("TableCells");
            AddCell(ref writer, "Textbox", "txtHeader", "% Excess Weight Loss Chart", DetailCellStyle("", "", "", ""), "2");
            writer.WriteEndElement();
            writer.WriteEndElement();
            #endregion

            #region Row1
            writer.WriteStartElement("TableRow");
            writer.WriteElementString("Height", "0.5cm");
            writer.WriteStartElement("TableCells");
            AddCell(ref writer, "Textbox", "txtPatientName", titleGraphPatientName, DetailCellStyle("", "", "", ""), "2");
            writer.WriteEndElement();
            writer.WriteEndElement();
            #endregion

            #region Row2
            writer.WriteStartElement("TableRow");
            writer.WriteElementString("Height", "0.5cm");
            writer.WriteStartElement("TableCells");
            AddCell(ref writer, "Textbox", "txtPatientAge", titleGraphPatientAge, DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "txtPatientSurgeryDate", titleGraphPatientSurgeryDate, DetailCellStyle("", "", "", ""), "");
            writer.WriteEndElement();
            writer.WriteEndElement();
            #endregion

            #region Row3
            writer.WriteStartElement("TableRow");
            writer.WriteElementString("Height", "0.5cm");
            writer.WriteStartElement("TableCells");
            AddCell(ref writer, "Textbox", "txtPatientStartWeight", titleGraphPatientStartWeight, DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "txtPatientInitialBMI", titleGraphPatientInitialBMI, DetailCellStyle("", "", "", ""), "");
            writer.WriteEndElement();
            writer.WriteEndElement();
            #endregion

            #region Row4
            writer.WriteStartElement("TableRow");
            writer.WriteElementString("Height", "0.5cm");
            writer.WriteStartElement("TableCells");
            AddCell(ref writer, "Textbox", "txtPatientCurrentWeight", titleGraphPatientCurrentWeight, DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "txtPatientTargetWeight", titleGraphPatientTargetWeight, DetailCellStyle("", "", "", ""), "");
            writer.WriteEndElement();
            writer.WriteEndElement();
            #endregion

            #region Row5
            writer.WriteStartElement("TableRow");           //<TableRow>
            writer.WriteElementString("Height", "0.5cm");
            writer.WriteStartElement("TableCells");         //  <TableCells>
            AddCell(ref writer, "Textbox", "txtBlankRow", "", DetailCellStyle("", "", "", ""), "2");
            writer.WriteEndElement();                       //  </TableCells>
            writer.WriteEndElement();                       //</TableRow>
            #endregion

            #region Row6
            writer.WriteStartElement("TableRow");           //<TableRow>
            writer.WriteElementString("Height", "0.001cm");
            writer.WriteStartElement("TableCells");         //  <TableCells>
            AddCell(ref writer, "Line", "Line1", "", DetailCellStyle("", "", "", ""), "2");
            writer.WriteEndElement();                       //  </TableCells>
            writer.WriteEndElement();                       //</TableRow>
            #endregion

            #region Row7
            writer.WriteStartElement("TableRow");
            writer.WriteElementString("Height", "0.5cm");
            writer.WriteStartElement("TableCells");

            AddCell(ref writer, "Textbox", "txtVisitDate_hr", "Visit Date", DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "txtEWL_hr", "%Excess Weight Loss", DetailCellStyle("", "", "", ""), "");

            writer.WriteEndElement();
            writer.WriteEndElement();
            #endregion

            writer.WriteEndElement();
            #endregion
            writer.WriteEndElement();
            #endregion

            #region Details
            writer.WriteStartElement("Details");
            writer.WriteStartElement("TableRows");
            writer.WriteStartElement("TableRow");
            writer.WriteElementString("Height", "0.5cm");
            writer.WriteStartElement("TableCells");

            AddCell(ref writer, "Textbox", "txtVisitDate", "=Fields!strDateSeen.Value", DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "txtEWL", "=Fields!EWL.Value", DetailCellStyle("", "", "", ""), "");

            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
            #endregion

            writer.WriteEndElement();
            #endregion

            writer.WriteEndElement();
            writer.WriteEndElement();
            #endregion

            writer.WriteEndElement();
            #endregion
            writer.Flush();
            stream.Close();
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value,
                                "EWLG Report", "EWLG", err.ToString());
        }
    }
    #endregion
    #endregion

    #region Patient Progress with RDL Builder
    #region private void PatientProgress_BuildReport()
    private void PatientProgress_BuildReport()
    {
        DataSet dsReport = new DataSet();
        SqlCommand cmdSelect = new SqlCommand();

        InitializeReportCommand(ref cmdSelect, "sp_Rep_PatientProgress");
        cmdSelect.Parameters.Add("@ImperialFlag", SqlDbType.Bit).Value = Request.Cookies["Imperial"].Value.Equals("True");
        cmdSelect.Parameters.Add("@ReportType", SqlDbType.Int).Value = 1;
        dsReport = gClass.FetchData(cmdSelect, "tblPatientProgress");

        DataTable dtTemp = dsReport.Tables["tblPatientProgress"];
        gClass.AddColumn(ref dtTemp, "ReportDate", "System.String", DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToShortTimeString());

        dsReport.AcceptChanges();

        CreateOutputFile("PatientProgress", dsReport);
    }
    #endregion 
    
    #region private void PatientProgress_RDL(DataSet dsReport, string strRDLFilename, string strLanguage)
    private void PatientProgress_RDL(DataSet dsReport, string strRDLFilename, string strLanguage)
    {
        System.IO.FileStream stream = System.IO.File.OpenWrite(strRDLFilename);
        System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(stream, System.Text.Encoding.UTF8);

        #region Report
        writer.Formatting = System.Xml.Formatting.Indented;
        writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"utf-8\"");
        writer.WriteStartElement("Report");     //<Report>
        AddReportConfiguration(ref writer, "Patient Progress", strLanguage, (Decimal)29, (Decimal)0.5, (Decimal)0.5, (Decimal)0.5, (Decimal)0.5, (Decimal)21, "cm");
        AddDataSource(ref writer, dsReport);
        AddDataSets(ref writer, dsReport);

        #region Body
        writer.WriteStartElement("Body");
        writer.WriteElementString("Height", "20cm"); //Report.height - (TopMargin + bottomMargin) == 29.7 - (0.5+0.5)
        writer.WriteStartElement("ReportItems");

        #region Table
        writer.WriteStartElement("Table");
        writer.WriteAttributeString("Name", "rptPatientProgress");

        #region TableColumns
        writer.WriteStartElement("TableColumns");

        AddColumn(ref writer, "1.5cm");
        AddColumn(ref writer, "0.75cm");
        AddColumn(ref writer, "2cm");
        AddColumn(ref writer, "2.5cm");
        AddColumn(ref writer, "1cm");
        AddColumn(ref writer, "2.5cm");
        AddColumn(ref writer, "2.5cm");
        AddColumn(ref writer, "2.5cm");
        AddColumn(ref writer, "2.5cm");
        AddColumn(ref writer, "2cm");
        AddColumn(ref writer, "2cm");
        AddColumn(ref writer, "2cm");
        AddColumn(ref writer, "2cm");
        AddColumn(ref writer, "2cm");
        writer.WriteEndElement();
        #endregion

        #region Header
        writer.WriteStartElement("Header");
        writer.WriteElementString("RepeatOnNewPage", "true");

        #region TableRows
        writer.WriteStartElement("TableRows");

        #region Row0
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Image", "imgTitle", Request.Url.Scheme + "://" + Request.Url.Host + Request.ApplicationPath + "/img/print_header_bar.gif", DetailCellStyle("", "", "", ""), "14");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row1
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtSurgeon", "Surgeon", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVSurgeon", titleSurgeon, DetailCellStyle("", "", "", ""), "6");
        AddCell(ref writer, "Textbox", "txtReportTitle", "Patient Progress", DetailCellStyle("", "12pt", "", ""), "7");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row2
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtHospital", "Hospital", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVHospital", titleHospital, DetailCellStyle("", "", "", ""), "13");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row21
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtRegion", "Region", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVRegion", titleRegion, DetailCellStyle("", "", "", ""), "13");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row3
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTDate", "Date", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTDate", titleDate, DetailCellStyle("", "", "", ""), "6");
        AddCell(ref writer, "Textbox", "txtReportDate", "=Fields!ReportDate.Value", DetailCellStyle("Blue", "", "", ""), "7");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row31
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTAge", "Age", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTAge", titleAge, DetailCellStyle("", "", "", ""), "13");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row32
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTBMI", "BMI", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTBMI", titleBMI, DetailCellStyle("", "", "", ""), "13");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row33
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTOperation", "Operation", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTOperation", titleOperation, DetailCellStyle("", "", "", ""), "13");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row34
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTApproach", "Approach", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTApproach", titleApproach, DetailCellStyle("", "", "", ""), "13");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row35
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTCategory", "Category", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTCategory", titleCategory, DetailCellStyle("", "", "", ""), "13");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row36
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTGroup", "Group", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTGroup", titleGroup, DetailCellStyle("", "", "", ""), "13");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row37
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTBandType", "Band Type", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTBandType", titleBandType, DetailCellStyle("", "", "", ""), "13");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row38
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTBandSize", "Band Size", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTBandSize", titleBandSize, DetailCellStyle("", "", "", ""), "13");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row4
        writer.WriteStartElement("TableRow");           //<TableRow>
        writer.WriteElementString("Height", "0.001cm");
        writer.WriteStartElement("TableCells");         //  <TableCells>
        AddCell(ref writer, "Line", "Line1", "", DetailCellStyle("", "", "", ""), "14");
        writer.WriteEndElement();                       //  </TableCells>
        writer.WriteEndElement();                       //</TableRow>
        #endregion

        #region Row5
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "1cm");
        writer.WriteStartElement("TableCells");

        AddCell(ref writer, "Textbox", "txtPatientName_hr", "Patient Name", DetailCellStyle("Blue", "", "", "LightGrey"), "4");
        AddCell(ref writer, "Textbox", "txtAge_hr", "Age", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtSurgeryDate_hr", "Surgery Date", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtLastVisitDate_hr", "Last Visit Date", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtStartWeight_hr", "Start Weight", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtCurrentWeight_hr", "Current Weight", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtLoss_hr", "Loss", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtInitialBMI_hr", "Initial BMI", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtBMI_hr", "BMI", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtBMIChange_hr", "BMI Change", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtEWL_hr", "%EWL", DetailCellStyle("Blue", "", "", "LightGrey"), "");

        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        writer.WriteEndElement();
        #endregion
        writer.WriteEndElement();
        #endregion

        #region Details
        writer.WriteStartElement("Details");
        writer.WriteStartElement("TableRows");
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.75cm");
        writer.WriteStartElement("TableCells");

        AddCell(ref writer, "Textbox", "txtPatientID", "=Fields!PatientID.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtPatientTitle", "=Fields!PatientTitle.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtPatientFirstname", "=Fields!Firstname.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtPatientLastname", "=Fields!Surname.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtAge", "=Fields!AGE.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtSurgeryDate", "=Fields!LapbandDate.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtLastVisitDate", "=Fields!dateseen.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtStartWeight", "=Fields!StartWeight.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtCurrentWeight", "=Fields!Weight.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtLoss", "=Fields!StartWeight.Value - Fields!Weight.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtInitialBMI", "=Fields!InitBMI.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtBMI", "=Fields!BMI.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtBMIChange", "=Fields!InitBMI.Value - Fields!BMI.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtEWL", "=Fields!EWLL.Value", DetailCellStyle("", "", "", ""), "");

        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        writer.WriteEndElement();
        #endregion

        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        writer.WriteEndElement();
        #endregion
        writer.Flush();
        stream.Close();
    }
    #endregion
    #endregion
    
    #region Operaiton Detail List with RDL Builder
    #region private void OperationDetailList_BuildReport()
    private void OperationDetailList_BuildReport()
    {
        DataSet dsReport = new DataSet();
        SqlCommand cmdSelect = new SqlCommand();        

        InitializeReportCommand(ref cmdSelect, "sp_Rep_OperationDetails");

        cmdSelect.Parameters.Add("@SerialNo", SqlDbType.VarChar, 50).Value = (Request.QueryString["SerNo"].Equals("0") || (Request.QueryString["SerNo"].Length == 0)) ? string.Empty : Request.QueryString["SerNo"];
        dsReport = gClass.FetchData(cmdSelect, "tblOperationDetails");

        DataTable dtTemp = dsReport.Tables["tblOperationDetails"];
        gClass.AddColumn(ref dtTemp, "ReportDate", "System.String", DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToShortTimeString());

        dsReport.AcceptChanges();

        CreateOutputFile("OperationDetails", dsReport);
    }
    #endregion 
    
    #region private void OperationDetailList_RDL(DataSet dsReport, string strRDLFilename, string strLanguage)
    private void OperationDetailList_RDL(DataSet dsReport, string strRDLFilename, string strLanguage)
    {
        System.IO.FileStream stream = System.IO.File.OpenWrite(strRDLFilename);
        System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(stream, System.Text.Encoding.UTF8);

        #region Report
        writer.Formatting = System.Xml.Formatting.Indented;
        writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"utf-8\"");
        writer.WriteStartElement("Report");     //<Report>
        AddReportConfiguration(ref writer, "Operation Details", strLanguage, (Decimal)29, (Decimal)0.5, (Decimal)0.5, (Decimal)0.5, (Decimal)0.5, (Decimal)21, "cm");
        AddDataSource(ref writer, dsReport);
        AddDataSets(ref writer, dsReport);

        #region Body
        writer.WriteStartElement("Body");
        writer.WriteElementString("Height", "20cm"); //Report.height - (TopMargin + bottomMargin) == 29.7 - (0.5+0.5)
        writer.WriteStartElement("ReportItems");

        #region Table
        writer.WriteStartElement("Table");
        writer.WriteAttributeString("Name", "rptOperationDetails");

        #region TableColumns
        writer.WriteStartElement("TableColumns");
        AddColumn(ref writer, "1.5cm");
        AddColumn(ref writer, "3.5cm");
        AddColumn(ref writer, "2.5cm");
        AddColumn(ref writer, "3.5cm");
        AddColumn(ref writer, "3cm");
        AddColumn(ref writer, "3cm");
        AddColumn(ref writer, "2.5cm");
        AddColumn(ref writer, "2.5cm");
        AddColumn(ref writer, "2cm");
        AddColumn(ref writer, "1.5cm");
        AddColumn(ref writer, "2cm");
        writer.WriteEndElement();
        #endregion

        #region Header
        writer.WriteStartElement("Header");
        writer.WriteElementString("RepeatOnNewPage", "true");

        #region TableRows
        writer.WriteStartElement("TableRows");

        #region Row0
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Image", "imgTitle", Request.Url.Scheme + "://" + Request.Url.Host + Request.ApplicationPath + "/img/print_header_bar.gif", DetailCellStyle("", "", "", ""), "11");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion
        
        #region Row1
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtSurgeon", "Surgeon", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVSurgeon", titleSurgeon, DetailCellStyle("", "", "", ""), "5");
        AddCell(ref writer, "Textbox", "txtReportTitle", "Operation Details", DetailCellStyle("", "12pt", "", ""), "5");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row2
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtHospital", "Hospital", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVHospital", titleHospital, DetailCellStyle("", "", "", ""), "10");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row21
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtRegion", "Region", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVRegion", titleRegion, DetailCellStyle("", "", "", ""), "10");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row3
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTDate", "Date", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTDate", titleDate, DetailCellStyle("", "", "", ""), "5");
        AddCell(ref writer, "Textbox", "txtReportDate", "=Fields!ReportDate.Value", DetailCellStyle("Blue", "", "", ""), "5");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row31
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTAge", "Age", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTAge", titleAge, DetailCellStyle("", "", "", ""), "10");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row32
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTBMI", "BMI", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTBMI", titleBMI, DetailCellStyle("", "", "", ""), "10");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row33
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTOperation", "Operation", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTOperation", titleOperation, DetailCellStyle("", "", "", ""), "10");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row34
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTApproach", "Approach", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTApproach", titleApproach, DetailCellStyle("", "", "", ""), "10");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row35
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTCategory", "Category", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTCategory", titleCategory, DetailCellStyle("", "", "", ""), "10");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row36
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTGroup", "Group", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTGroup", titleGroup, DetailCellStyle("", "", "", ""), "10");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row37
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTBandType", "Band Type", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTBandType", titleBandType, DetailCellStyle("", "", "", ""), "10");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row38
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTBandSize", "Band Size", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTBandSize", titleBandSize, DetailCellStyle("", "", "", ""), "10");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row39
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTSerialNo", "Serial No", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTSerialNo", titleSerialNo, DetailCellStyle("", "", "", ""), "10");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row4
        writer.WriteStartElement("TableRow");           //<TableRow>
        writer.WriteElementString("Height", "0.001cm");
        writer.WriteStartElement("TableCells");         //  <TableCells>
        AddCell(ref writer, "Line", "Line1", "", DetailCellStyle("", "", "", ""), "11");
        writer.WriteEndElement();                       //  </TableCells>
        writer.WriteEndElement();                       //</TableRow>
        #endregion

        #region Row5
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "1cm");
        writer.WriteStartElement("TableCells");

        AddCell(ref writer, "Textbox", "txtPatientName_hr", "Patient Name", DetailCellStyle("Blue", "", "", "LightGrey"), "2");
        AddCell(ref writer, "Textbox", "txtSurgeryDate_hr", "Operation Date", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtSurgeryType_hr", "Operation", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtSurgerySurgeon_hr", "Surgeon", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtSurgeryApproach_hr", "Approach", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtSurgeryCategory_hr", "Category", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtSurgeryHospital_hr", "Hospital", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtSurgeryBandTypehr", "Band Type", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtSurgeryBandSize_hr", "Band Size", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtSurgerySerialNo_hr", "Serial No", DetailCellStyle("Blue", "", "", "LightGrey"), "");

        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        writer.WriteEndElement();
        #endregion 
        writer.WriteEndElement();
        #endregion 
        
        #region Details
        writer.WriteStartElement("Details");
        writer.WriteStartElement("TableRows");
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.75cm");
        writer.WriteStartElement("TableCells");

        AddCell(ref writer, "Textbox", "txtPatientID", "=Fields!PatientID.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtPatientName", "=Fields!PatientName.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtSurgeryDate", "=Fields!OperationDate.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtSurgery", "=Fields!SurgeryType_Desc.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtSurgeonName", "=Fields!DoctorName.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtApproach", "=Fields!Approach_Desc.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtCategory", "=Fields!Category_Desc.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtHospitalDesc", "=Fields!HospitalName.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtBandType", "=Fields!BandType_Desc.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtBandSize", "=Fields!BandSize_Desc.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtSerialNo", "=Fields!SerialNo.Value", DetailCellStyle("", "", "", ""), "");

        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 
        
        #region <Footer>
        writer.WriteStartElement("Footer");         //<Footer>
        writer.WriteElementString("RepeatOnNewPage", "false");
        writer.WriteStartElement("TableRows");      //  <TableRows>
        #region Summery Data in Footer - Row1
        writer.WriteStartElement("TableRow");       //      <TableRow>
        writer.WriteElementString("Height", "10cm");
        writer.WriteStartElement("TableCells");     //          <TableCells>
        writer.WriteStartElement("TableCell");     //              <TableCell>
        writer.WriteElementString("ColSpan", "11");
        writer.WriteStartElement("ReportItems");    //                  <ReportItems>
        writer.WriteStartElement("Rectangle");      //                      <Rectangle>
        writer.WriteAttributeString("Name", "rec1");
        writer.WriteElementString("Height", "10cm");
        writer.WriteStartElement("ReportItems");      //                        <ReportItems>

        double heightPos = 1;
        int count = 0;
        string key = "";
        string value = "";

        AddReportItem(ref writer, "Line", "Line2_RepFooter", "", ElementChilds("28cm", "0cm", "", "", (heightPos-0.1)+"cm", "0cm"), DetailCellStyle("", "", "", ""));

        AddReportItem(ref writer, "Textbox", "lblPatientNumber", "Number of Patients :", ElementChilds("3cm", "0.5cm", "", "1", heightPos+"cm", "1cm"), DetailCellStyle("", "9pt", "Left", ""));
        AddReportItem(ref writer, "Textbox", "txtPatientNumber", "=CountDistinct(Fields!PatientID.value)", ElementChilds("1cm", "0.5cm", "", "1", heightPos+"cm", "4cm"), DetailCellStyle("", "9pt", "Left", ""));
        
        heightPos += 0.6;
        AddReportItem(ref writer, "Textbox", "lblOperationNumber", "Number of Operations :", ElementChilds("3.5cm", "0.5cm", "", "1", heightPos+"cm", "1cm"), DetailCellStyle("", "9pt", "Left", ""));
        AddReportItem(ref writer, "Textbox", "txtOperationNumber", "=count(Fields!PatientID.value)", ElementChilds("1cm", "0.5cm", "", "1", heightPos+"cm", "4.5cm"), DetailCellStyle("", "9pt", "Left", ""));

        foreach (KeyValuePair<string, string> pair in codeBSTDesc)
        {
            heightPos += 0.6;
            count++;
            key = pair.Key.ToString();
            value = pair.Value.ToString();

            AddReportItem(ref writer, "Textbox", "lblOperation" + count, value+" :", ElementChilds("8cm", "0.5cm", "", "1", heightPos + "cm", "1.5cm"), DetailCellStyle("", "9pt", "Left", ""));
            AddReportItem(ref writer, "Textbox", "txtOperation" + count, "=Sum(IIf(Fields!SurgeryType.value = \"" + key + "\",1,0))", ElementChilds("1cm", "0.5cm", "", "1", heightPos + "cm", "9.5cm"), DetailCellStyle("", "9pt", "Left", ""));
        }

        writer.WriteEndElement();                   //                          </ReportItems>
        writer.WriteEndElement();                   //                      </Rectabgle>
        writer.WriteEndElement();                   //                  </ReportItems>
        writer.WriteEndElement();                   //             </TableCell>
        writer.WriteEndElement();                   //          </TableCells>
        writer.WriteEndElement();                 //        </TableRow>
        #endregion

        writer.WriteEndElement();                   //  </TableRows>
        writer.WriteEndElement();                   //</Footer>
        #endregion

        writer.WriteEndElement();
        #endregion

        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        writer.WriteEndElement();
        #endregion
        writer.Flush();
        stream.Close();
    }
    #endregion
    #endregion
        
    #region Operaiton Duration with LOS with RDL Builder
    #region private void OperationDurationLOS_BuildReport()
    private void OperationDurationLOS_BuildReport()
    {
        DataSet dsReport = new DataSet();
        strXSLTFileName = "GroupReport/OperationDurationWithLOS/OperationdurationwithLOSXSLTFile.xsl";

        FetchOperationSummary(ref dsReport);
        CreateOutputFile("OPERATIONLOS", dsReport);
        //tcXML.InnerHtml += gClass.ShowSchema(dsReport, Server.MapPath(strXSLTFileName));
    }
    #endregion 
    
    #region private void FetchOperationSummary(ref DataSet dsReport)
    private void FetchOperationSummary(ref DataSet dsReport)
    {
        DataTable dtTemp;
        SqlCommand cmdSelect = new SqlCommand();
        Int16 xh = 0;

        InitializeReportCommand(ref cmdSelect, "sp_Rep_OperationDurationWithLOS");
        dsReport = gClass.FetchData(cmdSelect, "tblOperations");
        dtTemp = dsReport.Tables["tblOperations"];
        gClass.AddColumn(ref dtTemp, "ReportDate", "System.String", DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToShortTimeString());

        if (Request.QueryString["Format"].Equals("1")) // HTML Format
        {
            dtTemp = new DataTable("tblSurgery");
            gClass.AddColumn(ref dtTemp, "SurgeryType", "System.String", "");
            gClass.AddColumn(ref dtTemp, "SurgeryType_Desc", "System.String", "");
            dtTemp.Constraints.Add(new UniqueConstraint(dtTemp.Columns[0]));

            foreach (DataRow dr in dsReport.Tables["tblOperations"].Rows)
            {
                DataRow drTemp = dtTemp.NewRow();
                drTemp["SurgeryType"] = dr["SurgeryType"].ToString();
                drTemp["SurgeryType_Desc"] = dr["SurgeryType_Desc"].ToString();
                try { dtTemp.Rows.Add(drTemp); }
                catch { }
            }
            dsReport.Tables.Add(dtTemp);

            dtTemp = new DataTable("tblApproach");
            gClass.AddColumn(ref dtTemp, "SurgeryType", "System.String", "");
            gClass.AddColumn(ref dtTemp, "Approach", "System.String", "");
            DataColumn[] _Columns = new DataColumn[dtTemp.Columns.Count];
            xh = 0; foreach (DataColumn dc in dtTemp.Columns) _Columns[xh++] = dc;
            dtTemp.Constraints.Add(new UniqueConstraint(_Columns));
            foreach (DataRow dr in dsReport.Tables["tblOperations"].Rows)
            {
                DataRow drTemp = dtTemp.NewRow();

                drTemp["SurgeryType"] = dr["SurgeryType"].ToString();
                drTemp["Approach"] = dr["Approach"].ToString();
                try { dtTemp.Rows.Add(drTemp); }
                catch { }
            }
            dsReport.Tables.Add(dtTemp);

            dtTemp = new DataTable("tblCategory");
            gClass.AddColumn(ref dtTemp, "SurgeryType", "System.String", "");
            gClass.AddColumn(ref dtTemp, "Approach", "System.String", "");
            gClass.AddColumn(ref dtTemp, "Category", "System.String", "");
            gClass.AddColumn(ref dtTemp, "Category_Desc", "System.String", "");
            _Columns = new DataColumn[dtTemp.Columns.Count];
            xh = 0; foreach (DataColumn dc in dtTemp.Columns) _Columns[xh++] = dc;
            dtTemp.Constraints.Add(new UniqueConstraint(_Columns));
            foreach (DataRow dr in dsReport.Tables["tblOperations"].Rows)
            {
                DataRow drTemp = dtTemp.NewRow();

                drTemp["SurgeryType"] = dr["SurgeryType"].ToString();
                drTemp["Approach"] = dr["Approach"].ToString();
                drTemp["Category"] = dr["Category"].ToString();
                drTemp["Category_Desc"] = dr["Category_Desc"].ToString();
                try { dtTemp.Rows.Add(drTemp); }
                catch { }
            }
            dsReport.Tables.Add(dtTemp);
        }
        dsReport.AcceptChanges();
    }
    #endregion

    #region private void OperationDurationWithLOS_RDL(DataSet dsReport, string strRDLFilename, string strLanguage)
    private void OperationDurationWithLOS_RDL(DataSet dsReport, string strRDLFilename, string strLanguage)
    {
        System.IO.FileStream stream = System.IO.File.OpenWrite(strRDLFilename);
        System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(stream, System.Text.Encoding.UTF8);

        writer.Formatting = System.Xml.Formatting.Indented;
        writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"utf-8\"");
        writer.WriteStartElement("Report");     //<Report>
        AddReportConfiguration(ref writer, "Operation Duration with LOS", strLanguage, (Decimal)29, (Decimal)0.5, (Decimal)0.5, (Decimal)0.5, (Decimal)0.5, (Decimal)21, "cm");
        AddDataSource(ref writer, dsReport);
        AddDataSets(ref writer, dsReport);
        #region Body
        writer.WriteStartElement("Body");   //<Body>
        writer.WriteElementString("Height", "28cm"); //Report.height - (TopMargin + bottomMargin) == 28 - (0.5+0.5)
        writer.WriteStartElement("ReportItems");//<ReportItems>
        writer.WriteStartElement("Table");          //<Table>
        writer.WriteAttributeString("Name", "tblPageHeader");
        writer.WriteElementString("DataSetName", "tblOperations");
        #region TableColumns
        writer.WriteStartElement("TableColumns");   //<TableColumns>
        AddColumn(ref writer, "1.5cm");   
        AddColumn(ref writer, "0.5cm");   
        AddColumn(ref writer, "10cm");   
        AddColumn(ref writer, "1cm");   
        AddColumn(ref writer, "1cm");   
        AddColumn(ref writer, "1cm");   
        AddColumn(ref writer, "3cm");   
        AddColumn(ref writer, "2cm");   
        writer.WriteEndElement();                   //<TableColumns>
        #endregion 
        
        #region Header
        writer.WriteStartElement("Header");             //<Header>
        writer.WriteElementString("RepeatOnNewPage", "true");
        #region TableRows
        writer.WriteStartElement("TableRows");  //<TableRows>
        #region Row0
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Image", "imgTitle", Request.Url.Scheme + "://"+ Request.Url.Host + Request.ApplicationPath + "/img/print_header_bar.gif", DetailCellStyle("", "", "", ""), "8");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion
        #region Row1
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtSurgeon", "Surgeon", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVSurgeon", titleSurgeon, DetailCellStyle("", "", "", ""), "2");
        AddCell(ref writer, "Textbox", "txtReportTitle", "Operation Duration with LOS", DetailCellStyle("", "", "", ""), "5");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion
        #region Row2
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtHospital", "Hospital", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVHospital", titleHospital, DetailCellStyle("", "", "", ""), "7");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion
        #region Row3
        writer.WriteStartElement("TableRow");       //<TableRow>
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");     //  <TableCells>
        AddCell(ref writer, "Textbox", "txtTDate", "Date", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTDate", titleDate, DetailCellStyle("", "", "", ""), "2");
        AddCell(ref writer, "Textbox", "txtReportDate", "=Fields!ReportDate.Value", DetailCellStyle("Blue", "", "", ""), "5");
        writer.WriteEndElement();                   //  </TableCells>
        writer.WriteEndElement();                   //</TableRow>
        #endregion
                
        #region Row31
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTAge", "Age", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTAge", titleAge, DetailCellStyle("", "", "", ""), "7");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row32
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTBMI", "BMI", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTBMI", titleBMI, DetailCellStyle("", "", "", ""), "7");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 
        
        #region Row33
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTOperation", "Operation", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTOperation", titleOperation, DetailCellStyle("", "", "", ""), "7");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row34
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTApproach", "Approach", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTApproach", titleApproach, DetailCellStyle("", "", "", ""), "7");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row35
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTCategory", "Category", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTCategory", titleCategory, DetailCellStyle("", "", "", ""), "7");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row36
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTGroup", "Group", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTGroup", titleGroup, DetailCellStyle("", "", "", ""), "7");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row37
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTBandType", "Band Type", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTBandType", titleBandType, DetailCellStyle("", "", "", ""), "7");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row38
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTBandSize", "Band Size", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTBandSize", titleBandSize, DetailCellStyle("", "", "", ""), "7");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 
        
        #region Row39
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTSerialNo", "Serial No", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTSerialNo", titleSerialNo, DetailCellStyle("", "", "", ""), "7");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row4
        writer.WriteStartElement("TableRow");           //<TableRow>
        writer.WriteElementString("Height", "0.001cm");
        writer.WriteStartElement("TableCells");         //  <TableCells>
        AddCell(ref writer, "Line", "Line1", "", DetailCellStyle("", "", "", ""), "8");
        writer.WriteEndElement();                       //  </TableCells>
        writer.WriteEndElement();                       //</TableRow>
        #endregion
        writer.WriteEndElement();               //</TableRows>
        #endregion
        writer.WriteEndElement();                       //</Header>
        #endregion 

        #region <TableGroups>
        writer.WriteStartElement("TableGroups");    //<TableGroups>
        
        writer.WriteStartElement("TableGroup");         //<TableGroup>  --  Surgury
        writer.WriteStartElement("Grouping");               //<Grouping>
        writer.WriteAttributeString("Name", "rptSurgeryGroup");
        writer.WriteStartElement("GroupExpressions");           //<GroupExpressions>
        writer.WriteElementString("GroupExpression", "=Fields!SurgeryType.Value");
        writer.WriteEndElement();                               //</GroupExpressions>
        writer.WriteEndElement();                           //</Grouping>
        writer.WriteStartElement("Header");                 //<Header>
        writer.WriteStartElement("TableRows");                  //<TableRows>
        writer.WriteStartElement("TableRow");                       //<TableRow>
        writer.WriteElementString("Height", "0.7cm");                    
        writer.WriteStartElement("TableCells");                         //<TableCells>
        AddCell(ref writer, "Textbox", "lblEmpty1", "", DetailCellStyle("", "", "", "LightSkyBlue"), "6");
        AddCell(ref writer, "Textbox", "lblDurationMins_Header", "Duration (mins)", DetailCellStyle("", "10pt", "Center", "LightSkyBlue"), "");
        AddCell(ref writer, "Textbox", "lblStayDays_Header", "Stay (hours)", DetailCellStyle("", "10pt", "Center", "LightSkyBlue"), "");
        writer.WriteEndElement();                                       //</TableCells>
        writer.WriteEndElement();                                   //</TableRow>
        writer.WriteStartElement("TableRow");                       //<TableRow>
        writer.WriteElementString("Height", "0.7cm");
        writer.WriteStartElement("TableCells");                         //<TableCells>
        AddCell(ref writer, "Textbox", "lblSurgeryDesc", "=Fields!SurgeryType_Desc.Value", DetailCellStyle("", "10pt", "", "LightSkyBlue"), "3");
        AddCell(ref writer, "Textbox", "txtSurgeryCount", "=count(Fields!SurgeryType.Value)", DetailCellStyle("Red", "10pt", "Center", "LightSkyBlue"), "");
        AddCell(ref writer, "Textbox", "lblEmpty2", "", DetailCellStyle("", "", "", "LightSkyBlue"), "2");
        AddCell(ref writer, "Textbox", "txtDurationSurgerySum", "=round(avg(Fields!Duration.Value))", DetailCellStyle("", "10pt", "Center", "LightSkyBlue"), "");
        AddCell(ref writer, "Textbox", "txtStaySurgerySum", "=round(avg(Fields!Stay.Value))", DetailCellStyle("", "10pt", "Center", "LightSkyBlue"), "");
        writer.WriteEndElement();                                       //</TableCells>
        writer.WriteEndElement();                                   //</TableRow>
        writer.WriteEndElement();                               //</TableRows>
        writer.WriteEndElement();                           //</Header>
        writer.WriteEndElement();                       //</TableGroup>

        writer.WriteStartElement("TableGroup");         //<TableGroup>  --  Approach
        writer.WriteStartElement("Grouping");               //<Grouping>
        writer.WriteAttributeString("Name", "rptApproachGroup");
        writer.WriteStartElement("GroupExpressions");           //<GroupExpressions>
        writer.WriteElementString("GroupExpression", "=Fields!Approach.Value");
        writer.WriteEndElement();                               //</GroupExpressions>
        writer.WriteEndElement();                           //</Grouping>
        writer.WriteStartElement("Header");                 //<Header>
        writer.WriteStartElement("TableRows");                  //<TableRows>
        writer.WriteStartElement("TableRow");                       //<TableRow>
        writer.WriteElementString("Height", "0.7cm");
        writer.WriteStartElement("TableCells");                         //<TableCells>
        AddCell(ref writer, "Textbox", "lblEmpty21", "", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "lblApproach", "=Fields!Approach.Value", DetailCellStyle("", "9pt", "", ""), "2");
        AddCell(ref writer, "Textbox", "lblEmpty22", "", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtApproachCount", "=count(Fields!Approach.Value)", DetailCellStyle("Red", "9pt", "Center", ""), "");
        AddCell(ref writer, "Textbox", "lblEmpty23", "", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtDurationApproachSum", "=round(avg(Fields!Duration.Value))", DetailCellStyle("", "9pt", "Center", ""), "");
        AddCell(ref writer, "Textbox", "txtStayApproachSum", "=round(avg(Fields!Stay.Value))", DetailCellStyle("", "9pt", "Center", ""), "");
        writer.WriteEndElement();                                       //</TableCells>
        writer.WriteEndElement();                                   //</TableRow>
        writer.WriteEndElement();                               //</TableRows>
        writer.WriteEndElement();                           //</Header>
        writer.WriteEndElement();                       //</TableGroup>

        writer.WriteStartElement("TableGroup");         //<TableGroup>  --  Category
        writer.WriteStartElement("Grouping");               //<Grouping>
        writer.WriteAttributeString("Name", "rptCategoryGroup");
        writer.WriteStartElement("GroupExpressions");           //<GroupExpressions>
        writer.WriteElementString("GroupExpression", "=Fields!Category_Desc.Value");
        writer.WriteEndElement();                               //</GroupExpressions>
        writer.WriteEndElement();                           //</Grouping>
        writer.WriteStartElement("Header");                 //<Header>
        writer.WriteStartElement("TableRows");                  //<TableRows>
        writer.WriteStartElement("TableRow");                       //<TableRow>
        writer.WriteElementString("Height", "0.7cm");
        writer.WriteStartElement("TableCells");                         //<TableCells>
        AddCell(ref writer, "Textbox", "lblEmpty31", "", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "lblEmpty32", "", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "lblCategory", "=Fields!Category_Desc.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "lblEmpty33", "", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "lblEmpty34", "", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtCategoryCount", "=count(Fields!Category.Value)", DetailCellStyle("Red", "", "Center", ""), "");
        AddCell(ref writer, "Textbox", "txtDurationCategorySum", "=round(avg(Fields!Duration.Value))", DetailCellStyle("", "", "Center", ""), "");
        AddCell(ref writer, "Textbox", "txtStayCategorySum", "=round(avg(Fields!Stay.Value))", DetailCellStyle("", "", "Center", ""), "");
        writer.WriteEndElement();                                       //</TableCells>
        writer.WriteEndElement();                                   //</TableRow>
        writer.WriteEndElement();                               //</TableRows>
        writer.WriteEndElement();                           //</Header>
        writer.WriteEndElement();                       //</TableGroup>
        writer.WriteEndElement();                   //</TableGroups>
        #endregion 

        #region <Footer>
        writer.WriteStartElement("Footer");         //<Footer>
        writer.WriteElementString("RepeatOnNewPage", "false");
        writer.WriteStartElement("TableRows");      //  <TableRows>
        writer.WriteStartElement("TableRow");       //      <TableRow>
        writer.WriteElementString("Height", "1cm");
        writer.WriteStartElement("TableCells");     //          <TableCells>
        AddCell(ref writer, "Textbox", "lblEmptyFooter", "", DetailCellStyle("", "", "", ""), "7");
        AddCell(ref writer, "Image", "imgLogo", Request.Url.Scheme + "://" + Request.Url.Host + Request.ApplicationPath + "/img/banner_sml.gif", DetailCellStyle("", "", "Right", ""), "");
        writer.WriteEndElement( );     //                       </TableCells>
        writer.WriteEndElement();     //                    </TableRow>
        writer.WriteEndElement();     //                </TableRows>
        writer.WriteEndElement( );                  //</Footer>
        #endregion 
        writer.WriteEndElement();                   //<Table>
        writer.WriteEndElement();           //ReportItems
        writer.WriteEndElement();       //</Body>
        #endregion
        writer.WriteEndElement();   //</Report>
        writer.Flush();
        stream.Close();
    }
	#endregion
    #endregion

    #region Patient List Report with RDL Builder
    #region private void PatientList_BuildReport()
    private void PatientList_BuildReport()
    {
        DataSet dsReport;
        SqlCommand cmdSelect = new SqlCommand();

        Int32 monthsFollowup = 0;
        try
        {
            monthsFollowup = Convert.ToInt32(Request.QueryString["M"]);             
        }
        catch (Exception err){}

        InitializeReportCommand(ref cmdSelect, "sp_Rep_PatientList");
        cmdSelect.Parameters.Add("@MonthsFollowUp", SqlDbType.Int).Value = monthsFollowup;

        dsReport = gClass.FetchData(cmdSelect, "tblPatientList");

        DataTable dtTemp = dsReport.Tables["tblPatientList"];
        dtTemp = dtTemp;
        gClass.AddColumn(ref dtTemp, "ReportDate", "System.String", DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToShortTimeString());

        dsReport.AcceptChanges();
        CreateOutputFile("PATIENTLIST", dsReport);
    }
    #endregion 

    #region private void PatientList_RDL(DataSet dsReport, string strRDLFilename, string strLanguage)
    private void PatientList_RDL(DataSet dsReport, string strRDLFilename, string strLanguage)
    {
        System.IO.FileStream stream = System.IO.File.OpenWrite(strRDLFilename);
        System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(stream, System.Text.Encoding.UTF8);

        writer.Formatting = System.Xml.Formatting.Indented;
        writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"utf-8\"");
        #region Report
        writer.WriteStartElement("Report");

        AddReportConfiguration(ref writer, "Patient List", strLanguage, (Decimal)29, (Decimal)0.5, (Decimal)0.5, (Decimal)0.5, (Decimal)0.5, (Decimal)21, "cm");
        AddDataSource(ref writer, dsReport);
        AddDataSets(ref writer, dsReport);

        #region Body
        writer.WriteStartElement("Body");
        writer.WriteElementString("Height", "20cm"); //Report.height - (TopMargin + bottomMargin) == 29.7 - (0.5+0.5)
        writer.WriteStartElement("ReportItems");

        #region Table
        writer.WriteStartElement("Table");
        writer.WriteAttributeString("Name", "rptPatientList");

        #region TableColumns
        writer.WriteStartElement("TableColumns");
        AddColumn(ref writer, "1.5cm");
        AddColumn(ref writer, "0.75cm");
        AddColumn(ref writer, "2cm");
        AddColumn(ref writer, "2.5cm");
        AddColumn(ref writer, "3.25cm");
        AddColumn(ref writer, "2.5cm");
        AddColumn(ref writer, "1cm");
        AddColumn(ref writer, "1.5cm");

        AddColumn(ref writer, "2.25cm");
        AddColumn(ref writer, "2cm");
        AddColumn(ref writer, "1cm");
        AddColumn(ref writer, "1.25cm");
        AddColumn(ref writer, "2cm");
        AddColumn(ref writer, "2.25cm");
        AddColumn(ref writer, "2.25cm");
        writer.WriteEndElement();
        #endregion 

        #region Header
        writer.WriteStartElement("Header");
        writer.WriteElementString("RepeatOnNewPage", "true");

        #region TableRows
        writer.WriteStartElement("TableRows");

        #region Row0
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Image", "imgTitle", Request.Url.Scheme + "://" + Request.Url.Host + Request.ApplicationPath + "/img/print_header_bar.gif", DetailCellStyle("", "", "", ""), "15");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row1
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtSurgeon", "Surgeon", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVSurgeon", titleSurgeon, DetailCellStyle("", "", "", ""), "8");
        AddCell(ref writer, "Textbox", "txtReportTitle", "Patient Details", DetailCellStyle("", "12pt", "", ""), "6");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row2
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtHospital", "Hospital", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVHospital", titleHospital, DetailCellStyle("", "", "", ""), "14");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row21
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtRegion", "Region", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVRegion", titleRegion, DetailCellStyle("", "", "", ""), "14");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row3
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTDate", "Date", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTDate", titleDate, DetailCellStyle("", "", "", ""), "8");
        AddCell(ref writer, "Textbox", "txtReportDate", "=Fields!ReportDate.Value", DetailCellStyle("Blue", "", "", ""), "6");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row31
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTAge", "Age", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTAge", titleAge, DetailCellStyle("", "", "", ""), "14");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row32
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTBMI", "BMI", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTBMI", titleBMI, DetailCellStyle("", "", "", ""), "14");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 
        
        #region Row33
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTOperation", "Operation", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTOperation", titleOperation, DetailCellStyle("", "", "", ""), "14");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row34
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTApproach", "Approach", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTApproach", titleApproach, DetailCellStyle("", "", "", ""), "14");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row35
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTCategory", "Category", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTCategory", titleCategory, DetailCellStyle("", "", "", ""), "14");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row36
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTGroup", "Group", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTGroup", titleGroup, DetailCellStyle("", "", "", ""), "14");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row37
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTBandType", "Band Type", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTBandType", titleBandType, DetailCellStyle("", "", "", ""), "14");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row38
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTBandSize", "Band Size", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTBandSize", titleBandSize, DetailCellStyle("", "", "", ""), "14");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 
                
        #region Row - Number of Patients
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "1cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "lblPatientNumber", "Number of Patients : ", DetailCellStyle("", "9pt", "", ""), "4");
        AddCell(ref writer, "Textbox", "txtPatientNumber", "=CountDistinct(Fields!PatientID.value)", DetailCellStyle("", "9pt", "", ""), "11");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row4
        writer.WriteStartElement("TableRow");           //<TableRow>
        writer.WriteElementString("Height", "0.001cm");
        writer.WriteStartElement("TableCells");         //  <TableCells>
        AddCell(ref writer, "Line", "Line1", "", DetailCellStyle("", "", "", ""), "15");
        writer.WriteEndElement();                       //  </TableCells>
        writer.WriteEndElement();                       //</TableRow>
        #endregion
        
        #region Row5
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");

        AddCell(ref writer, "Textbox", "txtBlank_hr", "", DetailCellStyle("Blue", "", "", "LightGrey"), "12");
        AddCell(ref writer, "Textbox", "txtLastVisitMonth_hr1", "Months Since", DetailCellStyle("Blue", "", "", "LightGrey"), "3");

        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row6
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");

        AddCell(ref writer, "Textbox", "txtPatientID_hr", "ID", DetailCellStyle("Blue", "", "", "LightGrey"), "2");
        AddCell(ref writer, "Textbox", "txtPatientFirstname_hr", "First Name", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtPatientLastname_hr", "Surname", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtAddress_hr", "Address", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtSuburb_hr", "Suburb", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtState_hr", "State", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtPostcode_hr", "Postcode", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtHomePhone_hr", "Home Phone", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtMobilePhone_hr", "Mobile Phone", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtAge_hr", "Age", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtSex_hr", "Gender", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtLastVisitMonth_hr", "Last Visit", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtLastVisitDate_hr", "Last Visit Date", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtNextVisitDate_hr", "Next Visit Date", DetailCellStyle("Blue", "", "", "LightGrey"), "");

        writer.WriteEndElement( );
        writer.WriteEndElement();
        #endregion 
                        
        writer.WriteEndElement();
        #endregion 
        writer.WriteEndElement();
        #endregion 

        #region Deatails
        writer.WriteStartElement("Details");
        writer.WriteStartElement("TableRows");
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.75cm");
        writer.WriteStartElement("TableCells");

        AddCell(ref writer, "Textbox", "txtPatientID", "=Fields!PatientID.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtPatientTitle", "=Fields!PatientTitle.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtPatientFirstname", "=Fields!Firstname.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtPatientLastname", "=Fields!Surname.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtAddress", "=Fields!Street.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtSuburb", "=Fields!Suburb.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtState", "=Fields!State.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtPostcode", "=Fields!Postcode.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtHomePhone", "=Fields!HomePhone.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtMobilePhone", "=Fields!MobilePhone.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtAge", "=Fields!AGE.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtSex", "=Fields!Sex.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtMthsSinceVisit", "=Fields!MthsSinceVisit.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtDateSeen", "=Fields!DateSeen.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtDateNextVisit", "=Fields!DateNextVisit.Value", DetailCellStyle("", "", "", ""), "");

        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 
                
        /*
        #region <Footer>
        writer.WriteStartElement("Footer");         //<Footer>
        writer.WriteElementString("RepeatOnNewPage", "true");
        writer.WriteStartElement("TableRows");      //  <TableRows>
        writer.WriteStartElement("TableRow");       //      <TableRow>
        writer.WriteElementString("Height", "1cm");
        writer.WriteStartElement("TableCells");     //          <TableCells>
        AddCell(ref writer, "Textbox", "txtPageNumber", "=Globals!PageNumber", DetailCellStyle("", "", "Right", ""), "13");
        writer.WriteEndElement();     //                       </TableCells>
        writer.WriteEndElement();     //                    </TableRow>
        writer.WriteEndElement();     //                </TableRows>
        writer.WriteEndElement();                  //</Footer>
        #endregion 
        */

        writer.WriteEndElement();
        #endregion 
        
        writer.WriteEndElement(); 
        writer.WriteEndElement(); 
        #endregion 

        writer.WriteEndElement();
        #endregion 
        writer.Flush();
        stream.Close();
    }
    #endregion 
    #endregion 

    #region Patient list with complication with RDL Builder
    #region private void PatientListWithComplications_BuildReport()
    private void PatientListWithComplications_BuildReport()
    {
        DataSet dsReport;
        SqlCommand cmdSelect = new SqlCommand();
        strXSLTFileName = "GroupReport/PatientListWithComplications/PatientsListWithComplicationsXSLTFile.xsl";

        InitializeReportCommand(ref cmdSelect, "sp_Rep_PatientListWithComplications");
        cmdSelect.Parameters.Add("@ImperialFlag", SqlDbType.Bit).Value = Request.Cookies["Imperial"].Value.Equals("True");
        dsReport = gClass.FetchData(cmdSelect, "tblPatientList");
        DataTable dtTemp = dsReport.Tables["tblPatientList"];

        gClass.AddColumn(ref dtTemp, "ReportDate", "System.String", DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToShortTimeString());
        dtTemp = new DataTable("tblPatient");
        gClass.AddColumn(ref dtTemp, "PatientId", "System.String", "");
        gClass.AddColumn(ref dtTemp, "OperationDate", "System.String", "");
        gClass.AddColumn(ref dtTemp, "Approach", "System.String", "");
        gClass.AddColumn(ref dtTemp, "SurgeryType_Desc", "System.String", "");
        gClass.AddColumn(ref dtTemp, "WeightMeasurment", "System.String", "");
        gClass.AddColumn(ref dtTemp, "WeightLoss", "System.String", "");
        gClass.AddColumn(ref dtTemp, "EWL", "System.String", "");
        gClass.AddColumn(ref dtTemp, "BMI", "System.String", "");
        DataColumn[] _Columns = new DataColumn[dtTemp.Columns.Count];
        Int16 xh = 0; foreach (DataColumn dc in dtTemp.Columns) _Columns[xh++] = dc;
        dtTemp.Constraints.Add(new UniqueConstraint(_Columns));
        foreach (DataRow dr in dsReport.Tables["tblPatientList"].Rows)
        {
            DataRow drTemp = dtTemp.NewRow();

            drTemp["PatientId"] = dr["PatientId"].ToString();
            drTemp["OperationDate"] = dr["OperationDate"].ToString();
            drTemp["Approach"] = dr["Approach"].ToString();
            drTemp["SurgeryType_Desc"] = dr["SurgeryType_Desc"].ToString();
            drTemp["WeightMeasurment"] = dr["WeightMeasurment"].ToString();
            drTemp["WeightLoss"] = dr["WeightLoss"].ToString();
            drTemp["EWL"] = dr["EWL"].ToString();
            drTemp["BMI"] = dr["BMI"].ToString();

            try { dtTemp.Rows.Add(drTemp); }
            catch { }
        }
        dsReport.Tables.Add(dtTemp);
        CreateOutputFile("COEREPORT", dsReport);
    }
    #endregion 

    #region private void PatientListWithComplications_RDL(DataSet dsReport, string strRDLFilename, string strLanguage)
    private void PatientListWithComplications_RDL(DataSet dsReport, string strRDLFilename, string strLanguage)
    {
        System.IO.FileStream stream = System.IO.File.OpenWrite(strRDLFilename);
        System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(stream, System.Text.Encoding.UTF8);

        writer.Formatting = System.Xml.Formatting.Indented;
        writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"utf-8\"");
        writer.WriteStartElement("Report");
        AddReportConfiguration(ref writer, "Patient List", strLanguage, (Decimal)22.5, (Decimal)0.5, (Decimal)0.5, (Decimal)0.5, (Decimal)0.5, (Decimal)29, "cm");
        AddDataSource(ref writer, dsReport);
        AddDataSets(ref writer, dsReport);

        #region body
        writer.WriteStartElement("Body");
        writer.WriteElementString("Height", "28cm"); //Report.height - (TopMargin + bottomMargin) == 28 - (0.5+0.5)
        writer.WriteStartElement("ReportItems");

        #region Table
        writer.WriteStartElement("Table");
        writer.WriteAttributeString("Name", "rptPatientList");
        writer.WriteElementString("DataSetName", "tblPatientList");

        writer.WriteStartElement("TableGroups");    //<TableGroups>
        writer.WriteStartElement("TableGroup");         //<TableGroup>

        writer.WriteStartElement("Header");                 //<Header>
        writer.WriteStartElement("TableRows");                  //<TableRows>
        writer.WriteStartElement("TableRow");                       //<TableRow>
        writer.WriteElementString("Height","0.5cm");                    //</Height>
        writer.WriteStartElement("TableCells");                         //<TableCells>
        AddCell(ref writer, "Textbox", "txtOperationDate1", "=Fields!OperationDate.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtPatientId1", "=Fields!PatientId.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtSurgeryType1", "=Fields!SurgeryType_Desc.Value", DetailCellStyle("Red", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtApproach1", "=Fields!Approach.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtBMI1", "=Fields!BMI.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtWeightLoss1", "=Fields!WeightLoss.Value", DetailCellStyle("Red", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtEWL1", "=Fields!EWL.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtBlank", "", DetailCellStyle("", "", "", ""), "2");
        writer.WriteEndElement();                                       //</TableCells>
        writer.WriteEndElement();                                   //</TableRow>
        writer.WriteEndElement();                               //</TableRows>
        writer.WriteEndElement();                           //</Header>

        writer.WriteStartElement("Sorting");                //<Sorting>
        writer.WriteStartElement("SortBy");                     //<SortBy>
        writer.WriteElementString("SortExpression", "=Fields!OperationDate.Value");
        writer.WriteElementString("Direction", "Ascending");
        writer.WriteEndElement();                               //</SortBy>
        writer.WriteStartElement("SortBy");                     //<SortBy>
        writer.WriteElementString("SortExpression", "=Fields!PatientId.Value");
        writer.WriteElementString("Direction", "Ascending");
        writer.WriteEndElement();                               //</SortBy>
        writer.WriteEndElement();                           //</Sorting>

        writer.WriteStartElement("Grouping");               //<Grouping>
        writer.WriteAttributeString("Name", "rptPatientList_Group1");
        writer.WriteStartElement("GroupExpressions");           //<GroupExpressions>
        writer.WriteElementString("GroupExpression", "=Fields!OperationDate.Value");
        writer.WriteElementString("GroupExpression", "=Fields!PatientId.Value");
        
        writer.WriteEndElement();                               //</GroupExpressions>
        writer.WriteEndElement();                           //</Grouping>
        writer.WriteEndElement();                       //</TableGroup>
        writer.WriteEndElement();                   //</TableGroups>

        #region TableColumns
        writer.WriteStartElement("TableColumns");
        AddColumn(ref writer, "2cm");
        AddColumn(ref writer, "2cm");
        AddColumn(ref writer, "4cm");
        AddColumn(ref writer, "3cm");
        AddColumn(ref writer, "1.5cm");
        AddColumn(ref writer, "2cm");
        AddColumn(ref writer, "1cm");
        AddColumn(ref writer, "2cm");
        AddColumn(ref writer, "3cm");
        writer.WriteEndElement();
        #endregion

        #region Header
        writer.WriteStartElement("Header");
        writer.WriteElementString("RepeatOnNewPage", "true");
        #region TableRows
        writer.WriteStartElement("TableRows");

        #region Row0
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Image", "imgTitle", Request.Url.Scheme + "://" + Request.Url.Host + Request.ApplicationPath + "/img/print_header_bar.gif", DetailCellStyle("", "", "", ""), "9");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row1
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtSurgeon", "Surgeon", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVSurgeon", titleSurgeon, DetailCellStyle("", "", "", ""), "2");
        AddCell(ref writer, "Textbox", "txtReportTitle", "Patient List including Operation, Weight loss and Complications", DetailCellStyle("", "12pt", "", ""), "6");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row2
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtHospital", "Hospital", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVHospital", titleHospital, DetailCellStyle("", "", "", ""), "8");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row3
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTDate", "Date", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTDate", titleDate, DetailCellStyle("", "", "", ""), "2");
        AddCell(ref writer, "Textbox", "txtReportDate", "=Fields!ReportDate.Value", DetailCellStyle("Blue", "", "", ""), "6");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row31
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTAge", "Age", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTAge", titleAge, DetailCellStyle("", "", "", ""), "8");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row32
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTBMI", "BMI", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTBMI", titleBMI, DetailCellStyle("", "", "", ""), "8");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 
        
        #region Row33
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTOperation", "Operation", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTOperation", titleOperation, DetailCellStyle("", "", "", ""), "8");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row34
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTApproach", "Approach", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTApproach", titleApproach, DetailCellStyle("", "", "", ""), "8");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row35
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTCategory", "Category", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTCategory", titleCategory, DetailCellStyle("", "", "", ""), "8");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row36
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTGroup", "Group", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTGroup", titleGroup, DetailCellStyle("", "", "", ""), "8");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row37
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTBandType", "Band Type", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTBandType", titleBandType, DetailCellStyle("", "", "", ""), "8");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row38
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTBandSize", "Band Size", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTBandSize", titleBandSize, DetailCellStyle("", "", "", ""), "8");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row4
        writer.WriteStartElement("TableRow");           //<TableRow>
        writer.WriteElementString("Height", "0.001cm");
        writer.WriteStartElement("TableCells");         //  <TableCells>
        AddCell(ref writer, "Line", "Line1", "", DetailCellStyle("", "", "", ""), "9");
        writer.WriteEndElement();                       //  </TableCells>
        writer.WriteEndElement();                       //</TableRow>
        #endregion



        #region Row3
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "1cm");
        writer.WriteStartElement("TableCells");

        AddCell(ref writer, "Textbox", "txtOperationDate_hr", "Operation Date", DetailCellStyle("", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtPatient_hr", "Patient", DetailCellStyle("", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtSurgery_hr", "Surgery", DetailCellStyle("", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtApproach_hr", "Approach", DetailCellStyle("", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtInitialBMI_hr", "Init BMI", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtWeightLoss_hr", "Weight Loss", DetailCellStyle("", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtEWL_hr", "%EWL", DetailCellStyle("", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtComplication_hr", "Complications / Events", DetailCellStyle("", "", "", "LightGrey"), "2");

        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        writer.WriteEndElement();
        #endregion
        writer.WriteEndElement();
        #endregion

        #region Deatails
        writer.WriteStartElement("Details");
        writer.WriteStartElement("TableRows");
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.75cm");
        writer.WriteStartElement("TableCells");

        AddCell(ref writer, "Textbox", "txtBlankRow", "", DetailCellStyle("", "", "", ""), "7");
        AddCell(ref writer, "Textbox", "txtComplicationDate", "=Fields!ComplicationDate.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtComplication", "=Fields!Complication.Value", DetailCellStyle("", "", "", ""), "");

        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        writer.WriteEndElement();
        #endregion 
        
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        writer.WriteEndElement(); //"Report"
        writer.Flush();
        stream.Close();
    }
    #endregion 
    #endregion

    #region Patient Contact Report with RDL Builder
    #region private void PatientContact_BuildReport()
    private void PatientContact_BuildReport()
    {
        DataSet dsReport;
        SqlCommand cmdSelect = new SqlCommand();

        InitializeReportCommand(ref cmdSelect, "sp_Rep_PatientContact");

        dsReport = gClass.FetchData(cmdSelect, "tblPatientContact");

        DataTable dtTemp = dsReport.Tables["tblPatientContact"];
        dtTemp = dtTemp;
        gClass.AddColumn(ref dtTemp, "ReportDate", "System.String", DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToShortTimeString());

        dsReport.AcceptChanges();
        CreateOutputFile("PatientContact", dsReport);
    }
    #endregion

    #region private void PatientContact_RDL(DataSet dsReport, string strRDLFilename, string strLanguage)
    private void PatientContact_RDL(DataSet dsReport, string strRDLFilename, string strLanguage)
    {
        System.IO.FileStream stream = System.IO.File.OpenWrite(strRDLFilename);
        System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(stream, System.Text.Encoding.UTF8);

        writer.Formatting = System.Xml.Formatting.Indented;
        writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"utf-8\"");
        #region Report
        writer.WriteStartElement("Report");

        AddReportConfiguration(ref writer, "Patient Contact", strLanguage, (Decimal)29, (Decimal)0.5, (Decimal)0.5, (Decimal)0.5, (Decimal)0.5, (Decimal)21, "cm");
        AddDataSource(ref writer, dsReport);
        AddDataSets(ref writer, dsReport);

        #region Body
        writer.WriteStartElement("Body");
        writer.WriteElementString("Height", "20cm"); //Report.height - (TopMargin + bottomMargin) == 29.7 - (0.5+0.5)
        writer.WriteStartElement("ReportItems");

        #region Table
        writer.WriteStartElement("Table");
        writer.WriteAttributeString("Name", "rptPatientList");

        #region TableColumns
        writer.WriteStartElement("TableColumns");
        AddColumn(ref writer, "1.5cm");
        AddColumn(ref writer, "1cm");
        AddColumn(ref writer, "2cm");
        AddColumn(ref writer, "2.5cm");
        AddColumn(ref writer, "1cm");
        AddColumn(ref writer, "3.25cm");
        AddColumn(ref writer, "2.5cm");
        AddColumn(ref writer, "1cm");
        AddColumn(ref writer, "1.5cm");

        AddColumn(ref writer, "2.25cm");
        AddColumn(ref writer, "2cm");
        AddColumn(ref writer, "3.5cm");
        AddColumn(ref writer, "3cm");
        writer.WriteEndElement();
        #endregion

        #region Header
        writer.WriteStartElement("Header");
        writer.WriteElementString("RepeatOnNewPage", "true");

        #region TableRows
        writer.WriteStartElement("TableRows");

        #region Row0
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Image", "imgTitle", Request.Url.Scheme + "://" + Request.Url.Host + Request.ApplicationPath + "/img/print_header_bar.gif", DetailCellStyle("", "", "", ""), "13");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row1
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtSurgeon", "Surgeon", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVSurgeon", titleSurgeon, DetailCellStyle("", "", "", ""), "8");
        AddCell(ref writer, "Textbox", "txtReportTitle", "Patient Details", DetailCellStyle("", "12pt", "", ""), "4");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row2
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtHospital", "Hospital", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVHospital", titleHospital, DetailCellStyle("", "", "", ""), "12");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row21
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtRegion", "Region", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVRegion", titleRegion, DetailCellStyle("", "", "", ""), "12");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion;

        #region Row3
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTDate", "Date", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTDate", titleDate, DetailCellStyle("", "", "", ""), "8");
        AddCell(ref writer, "Textbox", "txtReportDate", "=Fields!ReportDate.Value", DetailCellStyle("Blue", "", "", ""), "4");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row31
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTAge", "Age", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTAge", titleAge, DetailCellStyle("", "", "", ""), "12");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row32
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTBMI", "BMI", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTBMI", titleBMI, DetailCellStyle("", "", "", ""), "12");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row33
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTOperation", "Operation", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTOperation", titleOperation, DetailCellStyle("", "", "", ""), "12");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row34
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTApproach", "Approach", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTApproach", titleApproach, DetailCellStyle("", "", "", ""), "12");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row35
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTCategory", "Category", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTCategory", titleCategory, DetailCellStyle("", "", "", ""), "12");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row36
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTGroup", "Group", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTGroup", titleGroup, DetailCellStyle("", "", "", ""), "12");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row37
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTBandType", "Band Type", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTBandType", titleBandType, DetailCellStyle("", "", "", ""), "12");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row38
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTBandSize", "Band Size", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTBandSize", titleBandSize, DetailCellStyle("", "", "", ""), "12");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row - Number of Patients
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "1cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "lblPatientNumber", "Number of Patients : ", DetailCellStyle("", "9pt", "", ""), "4");
        AddCell(ref writer, "Textbox", "txtPatientNumber", "=CountDistinct(Fields!PatientID.value)", DetailCellStyle("", "9pt", "", ""), "9");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row4
        writer.WriteStartElement("TableRow");           //<TableRow>
        writer.WriteElementString("Height", "0.001cm");
        writer.WriteStartElement("TableCells");         //  <TableCells>
        AddCell(ref writer, "Line", "Line1", "", DetailCellStyle("", "", "", ""), "13");
        writer.WriteEndElement();                       //  </TableCells>
        writer.WriteEndElement();                       //</TableRow>
        #endregion

        #region Row3
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "1cm");
        writer.WriteStartElement("TableCells");

        AddCell(ref writer, "Textbox", "txtPatientID_hr", "ID ", DetailCellStyle("Blue", "", "", "LightGrey"), "2");
        AddCell(ref writer, "Textbox", "txtPatientFirstname_hr", "First Name", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtPatientSurname_hr", "Surname", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtAge_hr", "Age", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtAddress_hr", "Address", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtSuburb_hr", "Suburb", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtState_hr", "State", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtPostcode_hr", "Postcode", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtHomePhone_hr", "Home Phone", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtMobilePhone_hr", "Mobile Phone", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtEmail_hr", "Email Address", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtContact_hr", "Do not Contact", DetailCellStyle("Blue", "", "", "LightGrey"), "");

        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        writer.WriteEndElement();
        #endregion
        writer.WriteEndElement();
        #endregion

        #region Deatails
        writer.WriteStartElement("Details");
        writer.WriteStartElement("TableRows");
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.75cm");
        writer.WriteStartElement("TableCells");

        AddCell(ref writer, "Textbox", "txtPatientID", "=Fields!PatientID.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtPatientTitle", "=Fields!PatientTitle.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtPatientFirstname", "=Fields!Firstname.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtPatientSurname", "=Fields!Surname.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtAge", "=Fields!AGE.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtAddress", "=Fields!Street.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtSuburb", "=Fields!Suburb.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtState", "=Fields!State.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtPostcode", "=Fields!Postcode.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtHomePhone", "=Fields!HomePhone.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtMobilePhone", "=Fields!MobilePhone.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtEmailAddress", "=Fields!EmailAddress.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtContact", "=Fields!FollowupDoNotContact.Value", DetailCellStyle("", "", "", ""), "");

        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        /*
        #region <Footer>
        writer.WriteStartElement("Footer");         //<Footer>
        writer.WriteElementString("RepeatOnNewPage", "true");
        writer.WriteStartElement("TableRows");      //  <TableRows>
        writer.WriteStartElement("TableRow");       //      <TableRow>
        writer.WriteElementString("Height", "1cm");
        writer.WriteStartElement("TableCells");     //          <TableCells>
        AddCell(ref writer, "Textbox", "txtPageNumber", "=Globals!PageNumber", DetailCellStyle("", "", "Right", ""), "13");
        writer.WriteEndElement();     //                       </TableCells>
        writer.WriteEndElement();     //                    </TableRow>
        writer.WriteEndElement();     //                </TableRows>
        writer.WriteEndElement();                  //</Footer>
        #endregion 
        */

        writer.WriteEndElement();
        #endregion

        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        writer.WriteEndElement();
        #endregion
        writer.Flush();
        stream.Close();
    }
    #endregion
    #endregion 
    
    #region Summary By Quarter with RDL Builder
    #region private void SummaryByQuarter_BuildReport()
    private void SummaryByQuarter_BuildReport()
    {
        DataSet dsReport;
        SqlCommand cmdSelect = new SqlCommand();
        strXSLTFileName = "GroupReport/SummaryByQuarter/SummaryByQuarterXSLTFile.xsl";

        InitializeReportCommand(ref cmdSelect, "sp_Rep_SummaryByQuarter");
        cmdSelect.Parameters.Add("@ImperialFlag", SqlDbType.Bit).Value = Request.Cookies["Imperial"].Value.Equals("True");
        cmdSelect.Parameters.Add("@ReportType", SqlDbType.Int).Value = 1;
        dsReport = gClass.FetchData(cmdSelect, "tblPatientList");

        DataTable dtTemp = dsReport.Tables["tblPatientList"];
        gClass.AddColumn(ref dtTemp, "ReportName", "System.String", "SummaryByQuarter");
        gClass.AddColumn(ref dtTemp, "ReportDate", "System.String", DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToShortTimeString());
        gClass.AddColumn(ref dtTemp, "PatientDetailFlag", "System.String", Request.QueryString["PD"]);
        gClass.AddColumn(ref dtTemp, "EWLFlag", "System.String", Request.QueryString["EWL"]);

        dtTemp.Columns["EWLGroup025"].Expression = "iif(EWLL < 25, 1, 0)";
        dtTemp.Columns["EWLGroup2550"].Expression = "iif(EWLL >= 25 and EWLL < 50, 1, 0)";
        dtTemp.Columns["EWLGroup5075"].Expression = "iif(EWLL >= 50 and EWLL < 75, 1, 0)";
        dtTemp.Columns["EWLGroup75100"].Expression = "iif(EWLL >= 75, 1, 0)";

        //// should be comment
        //gClass.AddColumn(ref dtTemp, "WeightLoss", "System.Int16", "0");
        //gClass.AddColumn(ref dtTemp, "BMIChange", "System.Int16", "0");
        //dtTemp.Columns["WeightLoss"].Expression = "StartWeight - Weight";
        //dtTemp.Columns["BMIChange"].Expression = "InitBMI - BMI";

        //// should be comment
        //cmdSelect.Parameters["@ReportType"].Value = 2;
        //dsReport.Tables.Add(gClass.FetchData(cmdSelect, "tblMonth").Tables[0].Copy());
        //DataRelation drMonth = new DataRelation("drMonth", dsReport.Tables["tblMonth"].Columns["VisitMonthsSinceOperation"], dsReport.Tables["tblPatientList"].Columns["VisitMonthsSinceOperation"]);
        //dsReport.Relations.Add(drMonth);
        //dsReport.Tables["tblMonth"].Columns["AgeSTDev"].Expression = "StDev(Child(drMonth).Age)";
        //dsReport.Tables["tblMonth"].Columns["StartWeightSTDev"].Expression = "StDev(Child(drMonth).StartWeight)";
        //dsReport.Tables["tblMonth"].Columns["WeightSTDev"].Expression = "StDev(Child(drMonth).Weight)";
        //dsReport.Tables["tblMonth"].Columns["WeightLossSTDev"].Expression = "StDev(Child(drMonth).WeightLoss)";
        //dsReport.Tables["tblMonth"].Columns["InitBMISTDev"].Expression = "StDev(Child(drMonth).InitBMI)";
        //dsReport.Tables["tblMonth"].Columns["BMISTDev"].Expression = "StDev(Child(drMonth).BMI)";
        //dsReport.Tables["tblMonth"].Columns["BMIChangeSTDev"].Expression = "StDev(Child(drMonth).BMIChange)";
        //dsReport.Tables["tblMonth"].Columns["EWLLSTDev"].Expression = "StDev(Child(drMonth).EWLL)";

        cmdSelect.Parameters["@ReportType"].Value = 3;
        dsReport.Tables.Add(gClass.FetchData(cmdSelect, "tblPatientID").Tables[0].Copy());

        // should be in comment
        cmdSelect.Parameters["@ReportType"].Value = 4;
        dsReport.Tables.Add(gClass.FetchData(cmdSelect, "tblMonth_PatientID").Tables[0].Copy());
        
        cmdSelect.Parameters.Clear();
        InitializeReportCommand(ref cmdSelect, "sp_Rep_SummaryByQuarter_FollowupData");
        dsReport.Tables.Add(gClass.FetchData(cmdSelect, "tblPWDList").Tables[0].Copy());

        // fetch operation summary
        DataSet dsOperationSummary = new DataSet();
        FetchOperationSummary(ref dsOperationSummary);
        for (int Xh = 0; Xh < dsOperationSummary.Tables.Count; Xh++)
            dsReport.Tables.Add(dsOperationSummary.Tables[Xh].Copy());

        dsReport.AcceptChanges();
        ////tcXML.InnerHtml += gClass.ShowSchema(dsReport, Server.MapPath(strXSLTFileName));


        if (Request.QueryString["Format"] == "5")
        {
            #region htmlreport
            int row = 0;
            String rowClass = "";
            bool firstGroup = true;

            Double dblTemp = 0;

            String tempTitle = "";
            String tempResult = "";
            String tempFooter = "";

            String prevMonthNo = "";
            String repMonthNo = "";
            String repGender = "";
            String repPatID = "";
            String repPatName = "";
            Double repAge = 0;
            String repSurgeryDate = "";
            Double repStartWeight = 0;
            Double repCurrentWeight = 0;
            Double repLoss = 0;
            Double repInitBMI = 0;
            Double repBMI = 0;
            Double repBMIChange = 0;
            String repEWL = "";
            Double repEWLL = 0;
            
            Double avgAge = 0;
            Double avgStartWeight = 0;
            Double avgCurrentWeight = 0;
            Double avgLoss = 0;
            Double avgInitBMI = 0;
            Double avgBMI = 0;
            Double avgBMIChange = 0;
            Double avgEWLL = 0;

            Double repEWLGroup025 = 0;
            Double repEWLGroup2550 = 0;
            Double repEWLGroup5075 = 0;
            Double repEWLGroup75100 = 0;

            Double avgEWLGroup025 = 0;
            Double avgEWLGroup2550 = 0;
            Double avgEWLGroup5075 = 0;
            Double avgEWLGroup75100 = 0;

            List<Double> repListAge = new List<Double>();
            List<Double> repListStartWeight = new List<Double>();
            List<Double> repListCurrentWeight = new List<Double>();
            List<Double> repListLoss = new List<Double>();
            List<Double> repListInitBMI = new List<Double>();
            List<Double> repListBMI = new List<Double>();
            List<Double> repListBMIChange = new List<Double>();
            List<Double> repListEWLL = new List<Double>();

            int MonthlyFemaleCnt = 0, MonthlyMaleCnt = 0, MonthlyOtherCnt = 0, MonthlyTotalCnt = 0;
            
            Double MonthlyEWLGroup025 = 0, MonthlyEWLGroup2550 = 0, MonthlyEWLGroup5075 = 0, MonthlyEWLGroup75100 = 0;

            Double MonthlyAge = 0, MonthlyStartWeight = 0, MonthlyCurrentWeight = 0, MonthlyLoss = 0, MonthlyInitBMI = 0, MonthlyBMI = 0, MonthlyBMIChange = 0, MonthlyEWLL = 0;
            Double TotalAge =0, TotalStartWeight = 0, TotalCurrentWeight = 0, TotalLoss = 0, TotalInitBMI = 0, TotalBMI = 0, TotalBMIChange = 0, TotalEWLL = 0;

            tempTitle += "<table width='100%' class'testNameTable' border='0' cellpadding='0' cellspacing='0'>";
            tempTitle += "<tr><td width='14%'></td><td width='20%'></td><td width='7%'></td><td width='9%'></td><td width='9%'></td><td width='9%'></td><td width='5%'></td><td width='6%'></td><td width='5%'></td><td width='7%'></td><td width='5%'></td></tr>";

            //header
            tempTitle += "<br/>";
            tempTitle += "<tr style='font-weight:bold'><td>Surgeon</td><td colspan='3'>" + titleSurgeon + "</td><td colspan='7'>Summary Statistics</td></tr>";
            tempTitle += "<tr style='font-weight:bold'><td>Hospital</td><td colspan='10'>" + titleHospital + "</td></tr>";
            tempTitle += "<tr style='font-weight:bold'><td>Region</td><td colspan='10'>" + titleRegion + "</td></tr>";
            tempTitle += "<tr style='font-weight:bold'><td>Date</td><td colspan='3'>" + titleDate + "</td><td colspan='7' style='color:blue'>" + dsReport.Tables[0].Rows[0]["ReportDate"].ToString() + "</td></tr>";
            tempTitle += "<tr style='font-weight:bold'><td>Age</td><td colspan='10'>" + titleAge + "</td></tr>";
            tempTitle += "<tr style='font-weight:bold'><td>BMI</td><td colspan='10'>" + titleBMI + "</td></tr>";
            tempTitle += "<tr style='font-weight:bold'><td>Operation</td><td colspan='10'>" + titleOperation + "</td></tr>";
            tempTitle += "<tr style='font-weight:bold'><td>Approach</td><td colspan='10'>" + titleApproach + "</td></tr>";
            tempTitle += "<tr style='font-weight:bold'><td>Category</td><td colspan='10'>" + titleCategory + "</td></tr>";
            tempTitle += "<tr style='font-weight:bold'><td>Group</td><td colspan='10'>" + titleGroup + "</td></tr>";
            tempTitle += "<tr style='font-weight:bold'><td>Band Type</td><td colspan='10'>" + titleBandType + "</td></tr>";
            tempTitle += "<tr style='font-weight:bold'><td>Band Size</td><td colspan='10'>" + titleBandSize + "</td></tr>";
            tempTitle += "<tr style='font-weight:bold'><td>Serial No</td><td colspan='10'>" + titleSerialNo + "</td></tr>";

            for (int b = 0; b < dsReport.Tables[0].Rows.Count; b++)
            {
                repMonthNo = dsReport.Tables[0].Rows[b]["GroupByMonthNo"].ToString();
                repPatID = dsReport.Tables[0].Rows[b]["CustomPatientID"].ToString();
                repPatName = dsReport.Tables[0].Rows[b]["PatientName"].ToString();
                repAge = Double.TryParse(dsReport.Tables[0].Rows[b]["Age"].ToString(), out dblTemp) ? dblTemp : 0;
                repSurgeryDate = dsReport.Tables[0].Rows[b]["LapbandDate"].ToString();
                repStartWeight = Double.TryParse(dsReport.Tables[0].Rows[b]["StartWeight"].ToString(), out dblTemp) ? dblTemp : 0;
                repCurrentWeight = Double.TryParse(dsReport.Tables[0].Rows[b]["Weight"].ToString(), out dblTemp) ? dblTemp : 0;
                repLoss = Double.TryParse((repStartWeight - repCurrentWeight).ToString(), out dblTemp) ? dblTemp : 0;
                repInitBMI = Double.TryParse(dsReport.Tables[0].Rows[b]["InitBMI"].ToString(), out dblTemp) ? dblTemp : 0;
                repBMI = Double.TryParse(dsReport.Tables[0].Rows[b]["BMI"].ToString(), out dblTemp) ? dblTemp : 0;
                repBMIChange = Double.TryParse((repInitBMI - repBMI).ToString(), out dblTemp) ? dblTemp : 0;
                repEWL = dsReport.Tables[0].Rows[b]["strEWLL"].ToString();
                repEWLL = Double.TryParse(dsReport.Tables[0].Rows[b]["EWLL"].ToString(), out dblTemp) ? dblTemp : 0;

                repEWLGroup025 = Double.TryParse(dsReport.Tables[0].Rows[b]["EWLGroup025"].ToString(),out dblTemp) ? dblTemp :0;
                repEWLGroup2550 = Double.TryParse(dsReport.Tables[0].Rows[b]["EWLGroup2550"].ToString(),out dblTemp) ? dblTemp :0;
                repEWLGroup5075 = Double.TryParse(dsReport.Tables[0].Rows[b]["EWLGroup5075"].ToString(),out dblTemp) ? dblTemp :0;
                repEWLGroup75100 = Double.TryParse(dsReport.Tables[0].Rows[b]["EWLGroup75100"].ToString(),out dblTemp) ? dblTemp :0;

                repGender = dsReport.Tables[0].Rows[b]["Sex"].ToString();

                
                if (prevMonthNo != repMonthNo)
                {
                    MonthlyTotalCnt = MonthlyMaleCnt + MonthlyFemaleCnt + MonthlyOtherCnt;

                    avgAge = MonthlyAge / MonthlyTotalCnt;
                    avgStartWeight = MonthlyStartWeight / MonthlyTotalCnt;
                    avgCurrentWeight = MonthlyCurrentWeight / MonthlyTotalCnt;
                    avgLoss = MonthlyLoss / MonthlyTotalCnt;
                    avgInitBMI = MonthlyInitBMI / MonthlyTotalCnt;
                    avgBMI = MonthlyBMI / MonthlyTotalCnt;
                    avgBMIChange = MonthlyBMIChange / MonthlyTotalCnt;
                    avgEWLL = MonthlyEWLL / MonthlyTotalCnt;

                    avgEWLGroup025 = Math.Round(100 * MonthlyEWLGroup025 / MonthlyTotalCnt);
                    avgEWLGroup2550 = Math.Round(100 * MonthlyEWLGroup2550 / MonthlyTotalCnt);
                    avgEWLGroup5075 = Math.Round(100 * MonthlyEWLGroup5075 / MonthlyTotalCnt);
                    avgEWLGroup75100 = Math.Round(100 * MonthlyEWLGroup75100 / MonthlyTotalCnt);

                    row = 0;

                    //do not display summary if it is the first group
                    if (firstGroup == false)
                    {
                        if (Request.QueryString["EWL"].Equals("1"))
                        {
                            tempResult += "<tr><td colspan='11'>&nbsp;</td></tr>";
                            tempResult += "<tr><td style='font-size:12pt;font-weight:bold;'>" + prevMonthNo + "</td><td style='font-size:12pt;font-weight:bold;'>Month</td><td>Average</td><td>" + Math.Round(avgAge) + "</td><td>" + (Math.Round(avgStartWeight, 1)).ToString("#0.0") + "</td><td>" + (Math.Round(avgCurrentWeight, 1)).ToString("#0.0") + "</td><td>" + (Math.Round(avgLoss, 1)).ToString("#0.0") + "</td><td>" + (Math.Round(avgInitBMI, 1)).ToString("#0.0") + "</td><td>" + (Math.Round(avgBMI, 1)).ToString("#0.0") + "</td><td>" + (Math.Round(avgBMIChange, 1)).ToString("#0.0") + "</td><td>" + (Math.Round(avgEWLL, 1)).ToString("#0.0") + "</td></tr>";
                            tempResult += "<tr><td>" + MonthlyTotalCnt + "</td><td>Patients</td><td>&nbsp;</td><td>Age</td><td>Start Weight</td><td>Weight</td><td>Loss</td><td>Init BMI</td><td>BMI</td><td>BMI Change</td><td>% EWL</td></tr>";
                            tempResult += "<tr><td colspan='2'>&nbsp;</td><td>S.Deviation</td><td>" + Math.Round(getStandardDeviation(repListAge, avgAge)) + "</td><td>" + (Math.Round(getStandardDeviation(repListStartWeight, avgStartWeight), 1)).ToString("#0.0") + "</td><td>" + (Math.Round(getStandardDeviation(repListCurrentWeight, avgCurrentWeight), 1)).ToString("#0.0") + "</td><td>" + (Math.Round(getStandardDeviation(repListLoss, avgLoss), 1)).ToString("#0.0") + "</td><td>" + (Math.Round(getStandardDeviation(repListInitBMI, avgInitBMI), 1)).ToString("#0.0") + "</td><td>" + (Math.Round(getStandardDeviation(repListBMI, avgBMI), 1)).ToString("#0.0") + "</td><td>" + (Math.Round(getStandardDeviation(repListBMIChange, avgBMIChange), 1)).ToString("#0.0") + "</td><td>" + Math.Round(getStandardDeviation(repListEWLL, avgEWLL)) + "</td></tr>";
                            tempResult += "<tr><td colspan='2'>&nbsp;</td><td colspan='9'><hr/></td></tr>";
                            tempResult += "<tr><td colspan='3'>&nbsp;</td><td align='center' style='color:red'>EWL < 25</td><td>" + avgEWLGroup025 + "</td><td align='center' style='color:red'>EWL 25-50</td><td>" + avgEWLGroup2550 + "</td><td align='center' style='color:red'>EWL 50-75</td><td>" + avgEWLGroup5075 + "</td><td align='center' style='color:red'>EWL > 75</td><td>" + avgEWLGroup75100 + "</td></tr>";
                            tempResult += "<tr><td colspan='2'>&nbsp;</td><td>Total</td><td align='center'>Male</td><td>" + MonthlyMaleCnt + "</td><td align='center'>Female</td><td>" + MonthlyFemaleCnt + "</td></tr>";
                            tempResult += "<tr><td colspan='11'><hr/></td></tr>";
                        }
                    }
                    firstGroup = false;

                    if (Request.QueryString["PD"].Equals("1"))
                    {
                        tempResult += "<tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr><tr style='font-size:13pt;font-weight:bold;background-color:lightgrey'><td>" + repMonthNo + "</td><td colspan='10'>Months</td></tr>";
                        tempResult += "<tr style='color:blue;font-weight:bold;background-color:lightgrey'><td>ID</td><td>Patient Name</td><td>Age</td><td>Surgery Date</td><td>Start Weight</td><td>Current Weight</td><td>Loss</td><td>Init BMI</td><td>BMI</td><td>BMI Change</td><td>%EWL</td></tr>";
                    }
                    
                    //reset all monthly calculation
                    MonthlyMaleCnt = 0;
                    MonthlyFemaleCnt = 0;
                    MonthlyOtherCnt = 0;
                    MonthlyTotalCnt = 0;

                    MonthlyAge = 0;
                    MonthlyStartWeight = 0;
                    MonthlyCurrentWeight = 0;
                    MonthlyLoss = 0;
                    MonthlyInitBMI = 0;
                    MonthlyBMI = 0;
                    MonthlyBMIChange = 0;
                    MonthlyEWLL = 0;

                    MonthlyEWLGroup025 = 0;
                    MonthlyEWLGroup2550 = 0;
                    MonthlyEWLGroup5075 = 0;
                    MonthlyEWLGroup75100 = 0;

                    repListAge = new List<Double>();
                    repListStartWeight = new List<Double>();
                    repListCurrentWeight = new List<Double>();
                    repListLoss = new List<Double>();
                    repListInitBMI = new List<Double>();
                    repListBMI = new List<Double>();
                    repListBMIChange = new List<Double>();
                    repListEWLL = new List<Double>();
                }

                //count male female
                if (repGender == "M")
                    MonthlyMaleCnt++;
                else if (repGender == "F")
                    MonthlyFemaleCnt++;
                else
                    MonthlyOtherCnt++;

                MonthlyAge += repAge;
                MonthlyStartWeight += repStartWeight;
                MonthlyCurrentWeight += repCurrentWeight;
                MonthlyLoss += repLoss;
                MonthlyInitBMI += repInitBMI;
                MonthlyBMI += repBMI;
                MonthlyBMIChange += repBMIChange;
                MonthlyEWLL += repEWLL;

                MonthlyEWLGroup025 += repEWLGroup025;
                MonthlyEWLGroup2550 += repEWLGroup2550;
                MonthlyEWLGroup5075 += repEWLGroup5075;
                MonthlyEWLGroup75100 += repEWLGroup75100;

                repListAge.Add(repAge);
                repListStartWeight.Add(repStartWeight);
                repListCurrentWeight.Add(repCurrentWeight);
                repListLoss.Add(repLoss);
                repListInitBMI.Add(repInitBMI);
                repListBMI.Add(repBMI);
                repListBMIChange.Add(repBMIChange);
                repListEWLL.Add(repEWLL);


                if (row % 2 == 0)
                    rowClass = "row01";
                else
                    rowClass = "row02";


                if (Request.QueryString["PD"].Equals("1"))
                {
                    tempResult += "<tr class='" + rowClass + "'><td>" + repPatID + "</td><td>" + repPatName + "</td><td>" + repAge + "</td><td>" + repSurgeryDate + "</td><td style='color:blue'>" + repStartWeight.ToString("#0.0") + "</td><td style='color:blue'>" + repCurrentWeight.ToString("#0.0") + "</td><td>" + repLoss.ToString("#0.0") + "</td><td>" + repInitBMI.ToString("#0.0") + "</td><td>" + repBMI.ToString("#0.0") + "</td><td>" + repBMIChange.ToString("#0.0") + "</td><td>" + repEWL + "</td></tr>";
                }

                prevMonthNo = repMonthNo;
                row++;
            }

            MonthlyTotalCnt = MonthlyMaleCnt + MonthlyFemaleCnt + MonthlyOtherCnt;
            if (Request.QueryString["EWL"].Equals("1"))
            {
                tempResult += "<tr><td colspan='11'>&nbsp;</td></tr>";
                tempResult += "<tr><td style='font-size:12pt;font-weight:bold;'>" + prevMonthNo + "</td><td style='font-size:12pt;font-weight:bold;'>Month</td><td>Average</td><td>" + Math.Round(avgAge) + "</td><td>" + (Math.Round(avgStartWeight, 1)).ToString("#0.0") + "</td><td>" + (Math.Round(avgCurrentWeight, 1)).ToString("#0.0") + "</td><td>" + (Math.Round(avgLoss, 1)).ToString("#0.0") + "</td><td>" + (Math.Round(avgInitBMI, 1)).ToString("#0.0") + "</td><td>" + (Math.Round(avgBMI, 1)).ToString("#0.0") + "</td><td>" + (Math.Round(avgBMIChange, 1)).ToString("#0.0") + "</td><td>" + (Math.Round(avgEWLL, 1)).ToString("#0.0") + "</td></tr>";
                tempResult += "<tr><td>" + MonthlyTotalCnt + "</td><td>Patients</td><td>&nbsp;</td><td>Age</td><td>Start Weight</td><td>Weight</td><td>Loss</td><td>Init BMI</td><td>BMI</td><td>BMI Change</td><td>% EWL</td></tr>";
                tempResult += "<tr><td colspan='2'>&nbsp;</td><td>S.Deviation</td><td>" + Math.Round(getStandardDeviation(repListAge, avgAge)) + "</td><td>" + (Math.Round(getStandardDeviation(repListStartWeight, avgStartWeight), 1)).ToString("#0.0") + "</td><td>" + (Math.Round(getStandardDeviation(repListCurrentWeight, avgCurrentWeight), 1)).ToString("#0.0") + "</td><td>" + (Math.Round(getStandardDeviation(repListLoss, avgLoss), 1)).ToString("#0.0") + "</td><td>" + (Math.Round(getStandardDeviation(repListInitBMI, avgInitBMI), 1)).ToString("#0.0") + "</td><td>" + (Math.Round(getStandardDeviation(repListBMI, avgBMI), 1)).ToString("#0.0") + "</td><td>" + (Math.Round(getStandardDeviation(repListBMIChange, avgBMIChange), 1)).ToString("#0.0") + "</td><td>" + Math.Round(getStandardDeviation(repListEWLL, avgEWLL)) + "</td></tr>";
                tempResult += "<tr><td colspan='3'>&nbsp;</td><td align='center' style='color:red'>EWL < 25</td><td>" + avgEWLGroup025 + "</td><td align='center' style='color:red'>EWL 25-50</td><td>" + avgEWLGroup2550 + "</td><td align='center' style='color:red'>EWL 50-75</td><td>" + avgEWLGroup5075 + "</td><td align='center' style='color:red'>EWL > 75</td><td>" + avgEWLGroup75100 + "</td></tr>";
                tempResult += "<tr><td colspan='2'>&nbsp;</td><td>Total</td><td align='center'>Male</td><td>" + MonthlyMaleCnt + "</td><td align='center'>Female</td><td>" + MonthlyFemaleCnt + "</td></tr>";
                tempResult += "<tr><td colspan='11'><hr/></td></tr>";
            }

            DataTable dtFooter = dsReport.Tables["tblPatientList"].DefaultView.ToTable(true, new string[] { "PatientID", "AGE", "Sex"});
            DataView dvView = new DataView(dtFooter);
            int FemaleCnt = 0, MaleCnt = 0, OtherCnt = 0, TotalCnt = 0;
            decimal MaleValue = 0, FemaleValue = 0, OtherValue = 0;

            dvView.RowFilter = "Sex = 'F'";
            FemaleCnt = dvView.Count;
            dvView.RowFilter = "Sex = 'M'";
            MaleCnt = dvView.Count;
            dvView.RowFilter = "(Sex <> 'M') and (Sex <> 'F')";
            OtherCnt = dvView.Count;
            TotalCnt = FemaleCnt + MaleCnt + OtherCnt;

            if (MaleCnt > 0)
                MaleValue = Math.Round((decimal)(MaleCnt * 100 / (decimal)(TotalCnt)), 1);

            if (FemaleCnt > 0)
                FemaleValue = Math.Round((decimal)(FemaleCnt * 100 / (decimal)(TotalCnt)), 1);

            if (OtherCnt > 0)
                OtherValue = Math.Round((decimal)(OtherCnt * 100 / (decimal)(TotalCnt)), 1);

            DataView dvViewAll = new DataView(dtTemp);
            Double TotalAll = dvViewAll.Count;

            TotalAge = Convert.ToDouble(dtTemp.Compute("SUM(AGE)", "").ToString());
            TotalStartWeight = Convert.ToDouble(dtTemp.Compute("SUM(StartWeight)", "").ToString());
            TotalCurrentWeight = Convert.ToDouble(dtTemp.Compute("SUM(Weight)", "").ToString());
            TotalLoss = TotalStartWeight - TotalCurrentWeight;
            TotalInitBMI = Convert.ToDouble(dtTemp.Compute("SUM(InitBMI)", "").ToString());
            TotalBMI = Convert.ToDouble(dtTemp.Compute("SUM(BMI)", "").ToString());
            TotalBMIChange = TotalInitBMI - TotalBMI;
            TotalEWLL = Convert.ToDouble(dtTemp.Compute("SUM(EWLL)", "").ToString());

            String strAvgAge = Math.Round((TotalAge / TotalAll), 0).ToString();
            String strAvgStartWeight = Math.Round((TotalStartWeight / TotalAll), 1).ToString("#0.0");
            String strAvgCurrentWeight = Math.Round((TotalCurrentWeight / TotalAll), 1).ToString("#0.0");
            String strAvgLoss = Math.Round((TotalLoss / TotalAll), 1).ToString("#0.0");
            String strAvgInitBMI = Math.Round((TotalInitBMI / TotalAll), 1).ToString("#0.0");
            String strAvgBMI = Math.Round((TotalBMI / TotalAll), 1).ToString("#0.0");
            String strAvgBMIChange = Math.Round((TotalBMIChange / TotalAll), 1).ToString("#0.0");
            String strAvgEWLL = Math.Round((TotalEWLL / TotalAll), 1).ToString("#0.0");


            tempTitle += "<tr><td colspan='11'><hr/></td></tr><tr><td colspan='2'>Number of Patients:</td><td colspan='11'>" + TotalCnt + "</td></tr>";
            tempResult = tempTitle + tempResult + "</table>";
            tempFooter += "<br/><table width='100%' style='background-color:lightgreen'><tr><td width='7%'>Total</td><td width='7%'>Male</td><td width='7%'>Female</td><td width='7%'>Other</td><td width='8%'>&nbsp;</td><td width='7%'>Average</td><td width='7%'>Age</td><td width='7%'>Start Weight</td><td width='8%'>Current Weight</td><td width='7%'>Weight Loss</td><td width='7%'>Init BMI</td><td width='7%'>Current BMI</td><td width='7%'>BMI Change</td><td width='7%'>%EWL</td></tr>";
            tempFooter += "<tr><td colspan='4'><hr/></td><td>&nbsp;</td><td colspan='9'><hr/></td></tr>";
            tempFooter += "<tr><td>&nbsp;</td><td>" + MaleCnt + "(" + (MaleValue).ToString("#0.0") + "%)" + "</td><td>" + FemaleCnt + "(" + (FemaleValue).ToString("#0.0") + "%)" + "</td><td>" + OtherCnt + "(" + (OtherValue).ToString("#0.0") + "%)" + "</td>";
            tempFooter += "<td><td>&nbsp;</td><td>" + strAvgAge + "</td><td>" + strAvgStartWeight + "</td><td>" + strAvgCurrentWeight + "</td><td>" + strAvgLoss + "</td><td>" + strAvgInitBMI + "</td><td>" + strAvgBMI + "</td><td>" + strAvgBMIChange + "</td><td>" + strAvgEWLL + "</td></tr></table>";
            lblResult.Text = tempResult + tempFooter;
            #endregion
        }
        else
        {
            CreateOutputFile("SUMMARYBYQUARTER", dsReport);
        }
    }
    #endregion 

    #region private void SummaryByQuarter_RDL(DataSet dsReport, string strRDLFilename, string strLanguage)
    private void SummaryByQuarter_RDL(DataSet dsReport, string strRDLFilename, string strLanguage)
    {
        System.IO.FileStream stream = System.IO.File.OpenWrite(strRDLFilename);
        System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(stream, System.Text.Encoding.UTF8);

        writer.Formatting = System.Xml.Formatting.Indented;
        writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"utf-8\"");
        writer.WriteStartElement("Report");
        AddReportConfiguration(ref writer, "Summart by quarter", strLanguage, (Decimal)29, (Decimal)0.5, (Decimal)0.5, (Decimal)0.5, (Decimal)0.5, (Decimal)21, "cm");
        AddDataSource(ref writer, dsReport);
        AddDataSets(ref writer, dsReport);
        #region body
        writer.WriteStartElement("Body");   //<Body>
        writer.WriteElementString("Height", "28cm"); //Report.height - (TopMargin + bottomMargin) == 28 - (0.5+0.5)
        writer.WriteStartElement("ReportItems");//<ReportItems>

        writer.WriteStartElement("Table");      //<Table>
        writer.WriteAttributeString("Name", "tblPageHeader");
        writer.WriteElementString("PageBreakAtEnd", "true");
        writer.WriteElementString("DataSetName", "tblPatientList");

        #region TableColumns
        writer.WriteStartElement("TableColumns");   //<TableColumns>
        AddColumn(ref writer, "4cm");   //Patient ID
        AddColumn(ref writer, "5.5cm");   //Pateint Name
        AddColumn(ref writer, "2.5cm");   //AGE
        AddColumn(ref writer, "2.25cm");   //Surgery Date
        AddColumn(ref writer, "2.25cm");   //Start Weight
        AddColumn(ref writer, "2.5cm");   //Current Weight
        AddColumn(ref writer, "1.5cm");   //Loss
        AddColumn(ref writer, "1.75cm");   //Init BMI
        AddColumn(ref writer, "1.5cm");   //BMI
        AddColumn(ref writer, "2.25cm");   //BMI Change
        AddColumn(ref writer, "1.5cm");   //%EWL
        writer.WriteEndElement();                   //<TableColumns>
        #endregion 

        #region Header
        writer.WriteStartElement("Header");
        writer.WriteElementString("RepeatOnNewPage", "true");
        #region TableRows
        writer.WriteStartElement("TableRows");
        #region Row0
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Image", "imgTitle", Request.Url.Scheme + "://" + Request.Url.Host + Request.ApplicationPath + "/img/print_header_bar.gif", DetailCellStyle("", "", "", ""), "11");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row1
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtSurgeon", "Surgeon", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVSurgeon", titleSurgeon, DetailCellStyle("", "", "", ""), "3");
        AddCell(ref writer, "Textbox", "txtReportTitle", "Summary Statistics", DetailCellStyle("", "", "", ""), "7");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row2
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtHospital", "Hospital", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVHospital", titleHospital, DetailCellStyle("", "", "", ""), "10");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row21
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtRegion", "Region", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVRegion", titleRegion, DetailCellStyle("", "", "", ""), "10");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row3
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTDate", "Date", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTDate", titleDate, DetailCellStyle("", "", "", ""), "3");
        AddCell(ref writer, "Textbox", "txtReportDate", "=Fields!ReportDate.Value", DetailCellStyle("Blue", "", "", ""), "7");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row31
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTAge", "Age", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTAge", titleAge, DetailCellStyle("", "", "", ""), "10");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row32
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTBMI", "BMI", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTBMI", titleBMI, DetailCellStyle("", "", "", ""), "10");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 
        
        #region Row33
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTOperation", "Operation", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTOperation", titleOperation, DetailCellStyle("", "", "", ""), "10");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row34
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTApproach", "Approach", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTApproach", titleApproach, DetailCellStyle("", "", "", ""), "10");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row35
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTCategory", "Category", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTCategory", titleCategory, DetailCellStyle("", "", "", ""), "10");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row36
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTGroup", "Group", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTGroup", titleGroup, DetailCellStyle("", "", "", ""), "10");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row37
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTBandType", "Band Type", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTBandType", titleBandType, DetailCellStyle("", "", "", ""), "10");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row38
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "txtTBandSize", "Band Size", DetailCellStyle("", "", "", ""), "1");
        AddCell(ref writer, "Textbox", "txtVTBandSize", titleBandSize, DetailCellStyle("", "", "", ""), "10");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        #region Row4
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.002cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Line", "Line1", "", DetailCellStyle("", "", "", ""), "11");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        #region Row - Number of Patients
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "1cm");
        writer.WriteStartElement("TableCells");
        AddCell(ref writer, "Textbox", "lblPatientNumber", "Number of Patients : ", DetailCellStyle("", "9pt", "", ""), "2");
        AddCell(ref writer, "Textbox", "txtPatientNumber", "=CountDistinct(Fields!PatientID.value)", DetailCellStyle("", "9pt", "", ""), "9");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion 

        if (Request.QueryString["PD"].Equals("1"))
        {
            #region Row2
            writer.WriteStartElement("TableRow");
            writer.WriteElementString("Height", "0.5cm");
            writer.WriteStartElement("TableCells");

            AddCell(ref writer, "Textbox", "lblPatientID", "ID", DetailCellStyle("Blue", "", "", "LightGrey"), "");
            AddCell(ref writer, "Textbox", "lblPatientName", "Patient Name", DetailCellStyle("Blue", "", "", "LightGrey"), "");
            AddCell(ref writer, "Textbox", "lblAge", "Age at Surgery", DetailCellStyle("Blue", "", "", "LightGrey"), "");
            AddCell(ref writer, "Textbox", "lblSurgeryDate", "Surgery Date", DetailCellStyle("Blue", "", "", "LightGrey"), "");
            AddCell(ref writer, "Textbox", "lblSurgeryWeight", "Start Weight", DetailCellStyle("Blue", "", "", "LightGrey"), "");
            AddCell(ref writer, "Textbox", "lblCurrentWeight", "Current Weight", DetailCellStyle("Blue", "", "", "LightGrey"), "");
            AddCell(ref writer, "Textbox", "lblLoss", "Loss", DetailCellStyle("Blue", "", "", "LightGrey"), "");
            AddCell(ref writer, "Textbox", "lblInitBMI", "Init BMI", DetailCellStyle("Blue", "", "", "LightGrey"), "");
            AddCell(ref writer, "Textbox", "lblBMI", "BMI", DetailCellStyle("Blue", "", "", "LightGrey"), "");
            AddCell(ref writer, "Textbox", "lblBMIChange", "BMI Change", DetailCellStyle("Blue", "", "", "LightGrey"), "");
            AddCell(ref writer, "Textbox", "lblEWL", "%EWL", DetailCellStyle("Blue", "", "", "LightGrey"), "");

            writer.WriteEndElement();
            writer.WriteEndElement();
            #endregion
        }
        #region Row6
        writer.WriteStartElement("TableRow");           //<TableRow>
        writer.WriteElementString("Height", "0.002cm");
        writer.WriteStartElement("TableCells");         //  <TableCells>
        AddCell(ref writer, "Line", "Line2", "", DetailCellStyle("", "", "", ""), "11");
        writer.WriteEndElement();                       //  </TableCells>
        writer.WriteEndElement();                       //</TableRow>
        #endregion

        writer.WriteEndElement();
        #endregion
        writer.WriteEndElement();
        #endregion 
        
        #region <TableGroups>
        writer.WriteStartElement("TableGroups");    //<TableGroups>
        writer.WriteStartElement("TableGroup");         //<TableGroup>

        writer.WriteStartElement("Grouping");               //<Grouping>
        writer.WriteAttributeString("Name", "rptMonthGroup");
        writer.WriteStartElement("GroupExpressions");           //<GroupExpressions>
        writer.WriteElementString("GroupExpression", "=Fields!GroupByMonthNo.Value");
        writer.WriteEndElement();                               //</GroupExpressions>
        writer.WriteEndElement();                           //</Grouping>

        writer.WriteStartElement("Sorting");                //<Sorting>
        writer.WriteStartElement("SortBy");                     //<SortBy>
        writer.WriteElementString("SortExpression", "=Fields!GroupByMonthNo.Value");
        writer.WriteElementString("Direction", "Ascending");
        writer.WriteEndElement();                               //</SortBy>
        writer.WriteStartElement("SortBy");                     //<SortBy>
        writer.WriteElementString("SortExpression", "=Fields!PatientID.Value");
        writer.WriteElementString("Direction", "Ascending");
        writer.WriteEndElement();                               //</SortBy>
        writer.WriteStartElement("SortBy");                     //<SortBy>
        writer.WriteElementString("SortExpression", "=Fields!LapbandDate.Value");
        writer.WriteElementString("Direction", "Ascending");
        writer.WriteEndElement();                               //</SortBy>
        writer.WriteEndElement();                           //</Sorting>

        if (Request.QueryString["PD"].Equals("1") || Request.QueryString["EWL"].Equals("1"))
        {
            writer.WriteStartElement("Header");                 //<Header>
            writer.WriteStartElement("TableRows");                  //<TableRows>
            writer.WriteStartElement("TableRow");                       //<TableRow>
            writer.WriteElementString("Height", "0.5cm");                    //</Height>
            writer.WriteStartElement("TableCells");                         //<TableCells>
            AddCell(ref writer, "Textbox", "txtMonths_Header", "=Fields!GroupByMonthNo.Value", DetailCellStyle("", "12pt", "", "LightGrey"), "1");
            AddCell(ref writer, "Textbox", "lblMonths_Header", "=IIF(Fields!GroupByMonthNo.Value > 0, \"Months\", \"Month\")", DetailCellStyle("", "12pt", "", "LightGrey"), "10");
            writer.WriteEndElement();                                       //</TableCells>
            writer.WriteEndElement();                                   //</TableRow>
            writer.WriteEndElement();                               //</TableRows>
            writer.WriteEndElement();                           //</Header>
        }

        if (Request.QueryString["EWL"].Equals("1"))
        {
            writer.WriteStartElement("Footer");                //<Footer>
            writer.WriteStartElement("TableRows");                  //<TableRows>
            

            #region Footer Line
            writer.WriteStartElement("TableRow");
            writer.WriteElementString("Height", "0.001cm");
            writer.WriteStartElement("TableCells");
            AddCell(ref writer, "Image", "imgFooter", Request.Url.Scheme + "://" + Request.Url.Host + Request.ApplicationPath + "/img/print_header_bar.gif", DetailCellStyle("", "", "", ""), "11");
            writer.WriteEndElement();
            writer.WriteEndElement();
            #endregion

            #region Row1
            writer.WriteStartElement("TableRow");
            writer.WriteElementString("Height", "1cm");
            writer.WriteStartElement("TableCells");
            AddCell(ref writer, "Textbox", "txtMonths_Footer", "=Fields!GroupByMonthNo.Value", DetailCellStyle("", "12pt", "", ""), "");
            AddCell(ref writer, "Textbox", "lblMonths_Footer", "Month", DetailCellStyle("", "12pt", "", ""), "");
            AddCell(ref writer, "Textbox", "lblAverage", "Average", DetailCellStyle("", "8pt", "", ""), "");
            AddCell(ref writer, "Textbox", "txtAge_avg", "=round(Avg(Fields!AGE.value))", DetailCellStyle("", "8pt", "", ""), "");
            AddCell(ref writer, "Textbox", "txtStartWeight_avg", "=round(Avg(Fields!StartWeight.value),1)", DetailCellStyle("", "8pt", "", ""), "");
            AddCell(ref writer, "Textbox", "txtWeight_avg", "=round(Avg(Fields!Weight.value),1)", DetailCellStyle("", "8pt", "", ""), "");
            AddCell(ref writer, "Textbox", "txtLoss_avg", "=round(Avg(Fields!StartWeight.Value - Fields!Weight.Value),1)", DetailCellStyle("", "8pt", "", ""), "");
            AddCell(ref writer, "Textbox", "txtInitBMI_avg", "=round(Avg(Fields!InitBMI.Value),1)", DetailCellStyle("", "8pt", "", ""), "");
            AddCell(ref writer, "Textbox", "txtBMI_avg", "=round(Avg(Fields!BMI.Value),1)", DetailCellStyle("", "8pt", "", ""), "");
            AddCell(ref writer, "Textbox", "txtBMIChange_avg", "=round(Avg(Fields!InitBMI.Value - Fields!BMI.Value),1)", DetailCellStyle("", "8pt", "", ""), "");
            AddCell(ref writer, "Textbox", "txtEWL_avg", "=round(Avg(Fields!EWLL.Value),1)", DetailCellStyle("", "8pt", "", ""), "");
            writer.WriteEndElement();
            writer.WriteEndElement();
            #endregion

            #region Row2
            writer.WriteStartElement("TableRow");
            writer.WriteElementString("Height", "0.5cm");
            writer.WriteStartElement("TableCells");
            AddCell(ref writer, "Textbox", "txtPatientsNumber_Footer", "=countdistinct(Fields!PatientID.value)", DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "lblPatientsNumber_Footer", "=IIF(countdistinct(Fields!PatientID.value) > 0, \"Patients\", \"\")", DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "txtEmpty_Footer", "", DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "lblAge_Footer", "Age", DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "lblStartWeight_Footer", "Start Weight", DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "lblWeight_Footer", "Weight", DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "lblLoss_Footer", "Loss", DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "lblInitBMI_Footer", "Init BMI", DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "lblBMI_Footer", "BMI", DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "lblBMIChange_Footer", "BMI Change", DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "lblEWL_Footer", "% EWL", DetailCellStyle("", "", "", ""), "");
            writer.WriteEndElement();
            writer.WriteEndElement();
            #endregion

            #region Row3
            writer.WriteStartElement("TableRow");
            writer.WriteElementString("Height", "0.5cm");
            writer.WriteStartElement("TableCells");

            AddCell(ref writer, "Textbox", "txtEmpty1", "", DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "txtEmpty2", "", DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "lblStandardDeviation", "S.Deviation", DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "lblStDev_Age", "=round(StDevP(Fields!AGE.Value))", DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "lblStDev_StartWeight", "=round(StDevP(Fields!StartWeight.Value),1)", DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "lblStDev_Weight", "=round(StDevP(Fields!Weight.Value),1)", DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "txtStDev_Loss", "=round(StDevP(Fields!StartWeight.Value - Fields!Weight.Value),1)", DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "txtStDev_InitBMI", "=round(StDevP(Fields!InitBMI.Value),1)", DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "txtStDev_BMI", "=round(StDevP(Fields!BMI.Value),1)", DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "txtStDev_BMIChange", "=round(StDevP(Fields!InitBMI.Value - Fields!BMI.Value),1)", DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "txtStDev_EWL", "=round(StDevP(Fields!EWLL.Value))", DetailCellStyle("", "", "", ""), "");

            writer.WriteEndElement();
            writer.WriteEndElement();
            #endregion

            #region Row4
            writer.WriteStartElement("TableRow");
            writer.WriteElementString("Height", "0.5cm");
            writer.WriteStartElement("TableCells");

            AddCell(ref writer, "Textbox", "txtEmpty3", "", DetailCellStyle("", "", "", ""), "3");

            AddCell(ref writer, "Textbox", "lblEWL_25", "EWL < 25", DetailCellStyle("Red", "", "Center", ""), "");
            AddCell(ref writer, "Textbox", "txtEWL_25", "=IIF(countdistinct(Fields!PatientID.value) > 0, round(100 * (sum(Fields!EWLGroup025.Value) / countdistinct(Fields!PatientID.value))), 0)", DetailCellStyle("", "", "", ""), "");

            AddCell(ref writer, "Textbox", "lblEWL_50", "EWL 25~50", DetailCellStyle("Red", "", "Center", ""), "");
            AddCell(ref writer, "Textbox", "txtEWL_50", "=IIF(countdistinct(Fields!PatientID.value) > 0, round(100 * (sum(Fields!EWLGroup2550.Value) / countdistinct(Fields!PatientID.value))), 0)", DetailCellStyle("", "", "", ""), "");

            AddCell(ref writer, "Textbox", "lblEWL_75", "EWL 50~75", DetailCellStyle("Red", "", "Center", ""), "");
            AddCell(ref writer, "Textbox", "txtEWL_75", "=IIF(countdistinct(Fields!PatientID.value) > 0, round(100 * (sum(Fields!EWLGroup5075.Value) / countdistinct(Fields!PatientID.value))), 0)", DetailCellStyle("", "", "", ""), "");

            AddCell(ref writer, "Textbox", "lblEWL_100", "EWL > 75", DetailCellStyle("Red", "", "Center", ""), "");
            AddCell(ref writer, "Textbox", "txtEWL_100", "=IIF(countdistinct(Fields!PatientID.value) > 0, round(100 * (sum(Fields!EWLGroup75100.Value) / countdistinct(Fields!PatientID.value))), 0)", DetailCellStyle("", "", "", ""), "");

            writer.WriteEndElement();
            writer.WriteEndElement();
            #endregion

            #region Row5
            writer.WriteStartElement("TableRow");
            writer.WriteElementString("Height", "0.5cm");
            writer.WriteStartElement("TableCells");

            AddCell(ref writer, "Textbox", "txtEmpty4", "", DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "txtEmpty5", "", DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "lblTotal", "Total", DetailCellStyle("", "", "", ""), "");

            AddCell(ref writer, "Textbox", "lblMale", "Male", DetailCellStyle("", "", "Center", ""), "");
            AddCell(ref writer, "Textbox", "txtMale", "=Sum(IIf(Fields!Sex.value = \"M\",1,0))", DetailCellStyle("", "", "", ""), "");

            AddCell(ref writer, "Textbox", "lblFemale", "Female", DetailCellStyle("", "", "Center", ""), "");
            AddCell(ref writer, "Textbox", "txtFemale", "=Sum(IIf(Fields!Sex.value = \"F\",1,0))", DetailCellStyle("", "", "", ""), "");

            AddCell(ref writer, "Textbox", "txtEmpty6", "", DetailCellStyle("", "", "", ""), "4");

            writer.WriteEndElement();
            writer.WriteEndElement();
            #endregion

            writer.WriteEndElement();                               //</TableRows>
            writer.WriteEndElement();                           //</Footer>
        }

        writer.WriteEndElement();                       //</TableGroup>
        writer.WriteEndElement();                   //</TableGroups>
        #endregion 
        
        if (Request.QueryString["PD"].Equals("1"))
        {
            #region <Details>
            writer.WriteStartElement("Details");
            writer.WriteStartElement("TableRows");
            writer.WriteStartElement("TableRow");
            writer.WriteElementString("Height", "0.75cm");
            writer.WriteStartElement("TableCells");
            AddCell(ref writer, "Textbox", "txtPatientID", "=Fields!CustomPatientID.Value", DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "txtPatientName", "=Fields!PatientName.Value", DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "txtAge", "=Fields!AGE.Value", DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "txtSurgeryDate", "=Fields!LapbandDate.Value", DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "txtSurgeryWeight", "=Fields!StartWeight.Value", DetailCellStyle("Blue", "", "", ""), "");
            AddCell(ref writer, "Textbox", "txtCurrentWeight", "=Fields!Weight.Value", DetailCellStyle("Blue", "", "", ""), "");
            AddCell(ref writer, "Textbox", "txtLoss", "=Fields!StartWeight.Value - Fields!Weight.Value", DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "txtInitBMI", "=Fields!InitBMI.Value", DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "txtBMI", "=Fields!BMI.Value", DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "txtBMIChange", "=Fields!InitBMI.Value - Fields!BMI.Value", DetailCellStyle("", "", "", ""), "");
            AddCell(ref writer, "Textbox", "txtEWL", "=Fields!strEWLL.Value", DetailCellStyle("", "", "", ""), "");
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
            #endregion
        }
        
        #region <Footer>                        
        writer.WriteStartElement("Footer");         //<Footer>
        writer.WriteElementString("RepeatOnNewPage", "false");   
        writer.WriteStartElement("TableRows");      //  <TableRows>
        #region Summery Data in Footer - Row1
        writer.WriteStartElement("TableRow");       //      <TableRow>
        writer.WriteElementString("Height", "1cm");
        writer.WriteStartElement("TableCells");     //          <TableCells>
        writer.WriteStartElement("TableCell");     //              <TableCell>
        writer.WriteElementString("ColSpan", "11");
        writer.WriteStartElement("ReportItems");    //                  <ReportItems>
        writer.WriteStartElement("Rectangle");      //                      <Rectangle>
        writer.WriteAttributeString("Name", "rec1");
        writer.WriteElementString("Height", "4cm");
        writer.WriteStartElement("Style");          //                          <Style>
        writer.WriteElementString("BackgroundColor", "LightGreen");
        writer.WriteEndElement();                   //                          </Style>
        writer.WriteStartElement("ReportItems");      //                        <ReportItems>

        AddReportItem(ref writer, "Textbox", "lblTotal_RepFooter", "Total", ElementChilds("1cm", "0.5cm", "", "1", "0.2cm", "1cm"), DetailCellStyle("", "9pt", "Left", "LightGreen"));
        AddReportItem(ref writer, "Textbox", "lblMale_RepFooter", "Male", ElementChilds("2cm", "0.5cm", "", "1", "0.2cm", "2cm"), DetailCellStyle("", "9pt", "Center", "LightGreen"));
        AddReportItem(ref writer, "Textbox", "lblFemale_RepFooter", "Female", ElementChilds("2cm", "0.5cm", "", "1", "0.2cm", "4cm"), DetailCellStyle("", "9pt", "Center", "LightGreen"));
        AddReportItem(ref writer, "Textbox", "lblOther_RepFooter", "Other", ElementChilds("2cm", "0.5cm", "", "1", "0.2cm", "6cm"), DetailCellStyle("", "9pt", "Center", "LightGreen"));

        AddReportItem(ref writer, "Textbox", "lblAverage_RepFooter", "Average", ElementChilds("2cm", "0.5cm", "", "1", "0.2cm", "10cm"), DetailCellStyle("", "9pt", "Left", "LightGreen"));
        AddReportItem(ref writer, "Textbox", "lblAge_RepFooter", "Age", ElementChilds("1cm", "0.5cm", "", "1", "0.2cm", "12cm"), DetailCellStyle("", "9pt", "Left", "LightGreen"));
        AddReportItem(ref writer, "Textbox", "lblStartWeight_RepFooter", "Start Weight", ElementChilds("2cm", "0.5cm", "", "1", "0.2cm", "13cm"), DetailCellStyle("", "9pt", "Left", "LightGreen"));
        AddReportItem(ref writer, "Textbox", "lblCurrentWeight_RepFooter", "Current Weight", ElementChilds("2.5cm", "0.5cm", "true", "1", "0.2cm", "15cm"), DetailCellStyle("", "9pt", "Left", "LightGreen"));
        AddReportItem(ref writer, "Textbox", "lblWeightLoss_RepFooter", "Weight Loss", ElementChilds("2cm", "0.5cm", "true", "1", "0.2cm", "17.5cm"), DetailCellStyle("", "9pt", "Left", "LightGreen"));
        AddReportItem(ref writer, "Textbox", "lblInitialBMI_RepFooter", "Initial BMI", ElementChilds("2cm", "0.5cm", "true", "1", "0.2cm", "19.5cm"), DetailCellStyle("", "9pt", "Left", "LightGreen"));
        AddReportItem(ref writer, "Textbox", "lblCurrentBMI_RepFooter", "Current BMI", ElementChilds("2cm", "0.5cm", "true", "1", "0.2cm", "21.5cm"), DetailCellStyle("", "9pt", "Left", "LightGreen"));
        AddReportItem(ref writer, "Textbox", "lblBMIChange_RepFooter", "BMI Change", ElementChilds("2cm", "0.5cm", "true", "1", "0.2cm", "23.5cm"), DetailCellStyle("", "9pt", "Left", "LightGreen"));
        AddReportItem(ref writer, "Textbox", "lblEWL_RepFooter", "% EWL", ElementChilds("1.5cm", "0.5cm", "", "1", "0.2cm", "25.5cm"), DetailCellStyle("", "9pt", "Left", "LightGreen"));

        DataTable dtFooter = dsReport.Tables["tblPatientList"].DefaultView.ToTable(true, new string[] { "PatientID", "AGE",  "Sex"});
        
        DataView dvView = new DataView(dtFooter);

        //dvView = dsReport.Tables["tblPatientID"].DefaultView;

        int FemaleCnt = 0, MaleCnt = 0, OtherCnt = 0, TotalCnt = 0;

        decimal MaleValue = 0, FemaleValue = 0, OtherValue = 0;
        //dvView.RowStateFilter = DataViewRowState.OriginalRows;
        dvView.RowFilter = "Sex = 'F'";
        FemaleCnt = dvView.Count;
        dvView.RowFilter = "Sex = 'M'";
        MaleCnt = dvView.Count;
        dvView.RowFilter = "(Sex <> 'M') and (Sex <> 'F')";
        OtherCnt = dvView.Count;
        TotalCnt = FemaleCnt + MaleCnt + OtherCnt;

        if (MaleCnt > 0)
            MaleValue = Math.Round((decimal)(MaleCnt * 100 / (decimal)(TotalCnt)), 1);

        if (FemaleCnt > 0)
            FemaleValue = Math.Round((decimal)(FemaleCnt * 100 / (decimal)(TotalCnt)), 1);

        if (OtherCnt > 0)
            OtherValue = Math.Round((decimal)(OtherCnt * 100 / (decimal)(TotalCnt)), 1);

        AddReportItem(ref writer, "Line", "Line2_RepFooter", "", ElementChilds("7cm", "0cm", "", "", "0.75cm", "1cm"), DetailCellStyle("", "", "", ""));
        AddReportItem(ref writer, "Line", "Line1_RepFooter", "", ElementChilds("17cm", "0cm", "", "", "0.75cm", "10cm"), DetailCellStyle("", "", "", ""));

        AddReportItem(ref writer, "Textbox", "lblMaleResult_RepFooter", MaleCnt.ToString() + " (" + MaleValue.ToString() + "%)", ElementChilds("2cm", "0.5cm", "", "1", "0.8cm", "2cm"), DetailCellStyle("", "9pt", "Center", "LightGreen"));
        AddReportItem(ref writer, "Textbox", "lblFemaleResult_RepFooter", FemaleCnt.ToString() + " (" + FemaleValue.ToString() + "%)", ElementChilds("2cm", "0.5cm", "", "1", "0.8cm", "4cm"), DetailCellStyle("", "9pt", "Center", "LightGreen"));
        AddReportItem(ref writer, "Textbox", "lblOtherResult_RepFooter", OtherCnt.ToString() + " (" + OtherValue.ToString() + "%)", ElementChilds("2cm", "0.5cm", "", "1", "0.8cm", "6cm"), DetailCellStyle("", "9pt", "Center", "LightGreen"));

        AddReportItem(ref writer, "Textbox", "txtAge_RepFooter", "=round(avg(Fields!AGE.value))", ElementChilds("1cm", "0.5cm", "", "1", "0.8cm", "12cm"), DetailCellStyle("", "9pt", "Left", "LightGreen"));
        AddReportItem(ref writer, "Textbox", "txtStartWeight_RepFooter", "=round(avg(Fields!StartWeight.value),1)", ElementChilds("2cm", "0.5cm", "true", "1", "0.8cm", "13cm"), DetailCellStyle("", "9pt", "Center", "LightGreen"));
        AddReportItem(ref writer, "Textbox", "txtCurrentWeight_RepFooter", "=round(avg(Fields!Weight.value),1)", ElementChilds("2.5cm", "0.5cm", "true", "1", "0.8cm", "15cm"), DetailCellStyle("", "9pt", "Center", "LightGreen"));
        AddReportItem(ref writer, "Textbox", "txtWeightLoss_RepFooter", "=round(avg(Fields!StartWeight.value - Fields!Weight.value),1)", ElementChilds("2cm", "0.5cm", "true", "1", "0.8cm", "17.5cm"), DetailCellStyle("", "9pt", "Center", "LightGreen"));
        AddReportItem(ref writer, "Textbox", "txtInitialBMI_RepFooter", "=round(avg(Fields!InitBMI.value),1)", ElementChilds("2cm", "0.5cm", "true", "1", "0.8cm", "19.5cm"), DetailCellStyle("", "9pt", "Center", "LightGreen"));
        AddReportItem(ref writer, "Textbox", "txtCurrentBMI_RepFooter", "=round(avg(Fields!BMI.value),1)", ElementChilds("2cm", "0.5cm", "true", "1", "0.8cm", "21.5cm"), DetailCellStyle("", "9pt", "Center", "LightGreen"));
        AddReportItem(ref writer, "Textbox", "txtBMIChange_RepFooter", "=round(avg(Fields!InitBMI.value - Fields!BMI.value),1)", ElementChilds("2cm", "0.5cm", "true", "1", "0.8cm", "23.5cm"), DetailCellStyle("", "9pt", "Center", "LightGreen"));
        AddReportItem(ref writer, "Textbox", "txtEWL_RepFooter", "=round(avg(Fields!EWLL.value),1)", ElementChilds("1.5cm", "0.5cm", "", "1", "0.8cm", "25.5cm"), DetailCellStyle("", "9pt", "Center", "LightGreen"));

        writer.WriteEndElement();                   //                          </ReportItems>
        writer.WriteEndElement();                   //                      </Rectabgle>
        writer.WriteEndElement();                   //                  </ReportItems>
        writer.WriteEndElement();                   //             </TableCell>
        writer.WriteEndElement();                   //          </TableCells>
        writer.WriteEndElement();                 //        </TableRow>
        #endregion


        writer.WriteEndElement();                   //  </TableRows>
        writer.WriteEndElement();                   //</Footer>
        #endregion
        
        writer.WriteEndElement();               //</Table>

        writer.WriteEndElement();           //ReportItems
        writer.WriteEndElement();       //</Body>
        #endregion
        writer.WriteEndElement(); //"Report"
        writer.Flush();
        stream.Close();
    }
    #endregion 

    #region void LoadFollowpData(ref System.Xml.XmlTextWriter writer, DataSet dsReport, string strTop, int intMinValue, int intMaxValue, List<string> strCaptions)
    private void LoadFollowpData(ref System.Xml.XmlTextWriter writer, DataSet dsReport, string strTop, int intMinValue, int intMaxValue, List<string> strCaptions)
    {
        string      strFilter = String.Empty;
        DataView    dvFollowup = dsReport.Tables["tblPatientID"].DefaultView,
                    dvOperated = dsReport.Tables["tblPWDList"].DefaultView;

        if ((intMaxValue != -1) || (intMinValue != -1))
        {
            dvFollowup.RowStateFilter = DataViewRowState.OriginalRows;
            dvOperated.RowStateFilter = DataViewRowState.OriginalRows;

            if (intMaxValue != -1)
                strFilter = "(FollowupDays > " + intMinValue.ToString() + ") and (FollowupDays <= " + intMaxValue.ToString() + ")";
            else strFilter = "FollowupDays > " + intMinValue.ToString();
            dvFollowup.RowFilter = dvOperated.RowFilter = strFilter;
        }

        AddReportItem(ref writer, "Textbox", strCaptions[0], strCaptions[1], ElementChilds("2cm", "0.5cm", "", "1", strTop, "0.1cm"), DetailCellStyle("", "9pt", "Left", ""));
        AddReportItem(ref writer, "Textbox", strCaptions[2], dvFollowup.Count.ToString(), ElementChilds("1cm", "0.5cm", "", "1", strTop, "3cm"), DetailCellStyle("", "9pt", "Left", ""));
        AddReportItem(ref writer, "Textbox", strCaptions[3], dvOperated.Count.ToString(), ElementChilds("1cm", "0.5cm", "", "1", strTop, "6cm"), DetailCellStyle("", "9pt", "Left", ""));
    }
    #endregion

    #region void LoadDemographicData(ref System.Xml.XmlTextWriter writer, DataSet dsReport)
    private void LoadDemographicData(ref System.Xml.XmlTextWriter writer, DataSet dsReport)
    {
        DataView dvView = dsReport.Tables["tblPatientID"].DefaultView;
        int         FemaleCnt = 0, MaleCnt = 0, OtherCnt = 0;

        dvView.RowStateFilter = DataViewRowState.OriginalRows;
        dvView.RowFilter = "Sex = 'F'";
        FemaleCnt = dvView.Count;
        dvView.RowFilter = "Sex = 'M'";
        MaleCnt = dvView.Count;
        dvView.RowFilter = "(Sex <> 'M') and (Sex <> 'F')";
        OtherCnt = dvView.Count;

        AddReportItem(ref writer, "Textbox", "lblTotal_RepFooter", "Total", ElementChilds("2cm", "0.5cm", "", "1", "0.2cm", "3cm"), DetailCellStyle("", "9pt", "Left", "LightGreen"));
        AddReportItem(ref writer, "Textbox", "lblAge_RepFooter", "Age", ElementChilds("1cm", "0.5cm", "", "1", "0.2cm", "5cm"), DetailCellStyle("", "9pt", "Left", "LightGreen"));
        AddReportItem(ref writer, "Textbox", "lblStartWeight_RepFooter", "Start Weight", ElementChilds("2cm", "0.5cm", "", "1", "0.2cm", "6cm"), DetailCellStyle("", "9pt", "Left", "LightGreen"));
        
        AddReportItem(ref writer, "Textbox", "lblFemales_RepFooter", "Females", ElementChilds("1.5cm", "0.5cm", "", "1", "1.7cm", "2cm"), DetailCellStyle("", "9pt", "Left", ""));
        AddReportItem(ref writer, "Textbox", "lblFemales1_RepFooter", FemaleCnt.ToString(), ElementChilds("1cm", "0.5cm", "", "1", "1.7cm", "3.5cm"), DetailCellStyle("", "9pt", "Left", ""));

        if ((FemaleCnt + MaleCnt + OtherCnt) == 0)
            AddReportItem(ref writer, "Textbox", "lblFemales2_RepFooter", "", ElementChilds("1.5cm", "0.5cm", "", "1", "1.7cm", "4.5cm"), DetailCellStyle("", "9pt", "Left", ""));
        else 
            AddReportItem(ref writer, "Textbox", "lblFemales2_RepFooter", "%" + Math.Round((decimal)(FemaleCnt * 100 / (FemaleCnt+MaleCnt+OtherCnt))).ToString(), ElementChilds("1.5cm", "0.5cm", "", "1", "1.7cm", "4.5cm"), DetailCellStyle("", "9pt", "Left", ""));
        
        AddReportItem(ref writer, "Textbox", "lblMales_RepFooter", "Males", ElementChilds("1cm", "0.5cm", "", "1", "1.7cm", "6cm"), DetailCellStyle("", "9pt", "Left", ""));
        AddReportItem(ref writer, "Textbox", "lblMales1_RepFooter", MaleCnt.ToString(), ElementChilds("1cm", "0.5cm", "", "1", "1.7cm", "7cm"), DetailCellStyle("", "9pt", "Left", ""));
        AddReportItem(ref writer, "Textbox", "lblOthers_RepFooter", "Others", ElementChilds("1.25cm", "0.5cm", "", "1", "1.7cm", "8cm"), DetailCellStyle("", "9pt", "Left", ""));
        AddReportItem(ref writer, "Textbox", "lblOthers1_RepFooter", OtherCnt.ToString(), ElementChilds("1cm", "0.5cm", "", "1", "1.7cm", "9.25cm"), DetailCellStyle("", "9pt", "Left", ""));
    }
    #endregion

    #region private void LoadBMIDistribution(ref System.Xml.XmlTextWriter writer, DataSet dsReport)
    private void LoadBMIDistribution(ref System.Xml.XmlTextWriter writer, DataSet dsReport)
    {
        int BMI35, BMI39, BMI49, BMI59, BMI69, BMI70, BMITotal;
        DataView dvView = dsReport.Tables["tblPatientList"].DefaultView;

        BMITotal = dvView.Count;
        BMI35 = BMI39 = BMI49 = BMI59 = BMI69 = BMI70 = 0;
        dvView.RowStateFilter = DataViewRowState.OriginalRows;

        AddReportItem(ref writer, "Textbox", "lblBMIDistribution_RepFooter", "BMI Distribution", ElementChilds("10cm", "0.8cm", "", "1", "0.1cm", "0.1cm"), DetailCellStyle("", "12pt", "Left", "LightGrey"));
        AddReportItem(ref writer, "Textbox", "lblMinBMI_RepFooter", "Min.", ElementChilds("1cm", "0.8cm", "", "1", "1cm", "1cm"), DetailCellStyle("Red", "9pt", "Left", ""));
        AddReportItem(ref writer, "Textbox", "txtMinBMI_RepFooter", "=round(min(Fields!InitBMI.value))", ElementChilds("1cm", "0.8cm", "", "1", "1cm", "2cm"), DetailCellStyle("Red", "9pt", "Left", ""));
        AddReportItem(ref writer, "Textbox", "lblMeanBMI_RepFooter", "Mean", ElementChilds("1cm", "0.8cm", "", "1", "1cm", "4cm"), DetailCellStyle("Red", "9pt", "Left", ""));
        AddReportItem(ref writer, "Textbox", "txtMeanBMI_RepFooter", "=round(avg(Fields!InitBMI.value))", ElementChilds("1cm", "0.8cm", "", "1", "1cm", "5cm"), DetailCellStyle("Red", "9pt", "Left", ""));
        AddReportItem(ref writer, "Textbox", "lblMaxBMI_RepFooter", "Max", ElementChilds("1cm", "0.8cm", "", "1", "1cm", "7cm"), DetailCellStyle("Red", "9pt", "Left", ""));
        AddReportItem(ref writer, "Textbox", "txtMaxBMI_RepFooter", "=round(Max(Fields!InitBMI.value))", ElementChilds("1cm", "0.8cm", "", "1", "1cm", "8cm"), DetailCellStyle("Red", "9pt", "Left", ""));

        dvView.RowFilter = "InitBMI < 35"; BMI35 = dvView.Count;
        dvView.RowFilter = "InitBMI >= 35 and InitBMI < 40"; BMI39 = dvView.Count;
        dvView.RowFilter = "InitBMI >= 40 and InitBMI < 50"; BMI49 = dvView.Count;
        dvView.RowFilter = "InitBMI >= 50 and InitBMI < 60"; BMI59 = dvView.Count;
        dvView.RowFilter = "InitBMI >= 60 and InitBMI < 70"; BMI69 = dvView.Count;
        dvView.RowFilter = "InitBMI > 70"; BMI70 = dvView.Count;

        AddReportItem(ref writer, "Textbox", "lblBMI35_RepFooter", "BMI < 35", ElementChilds("2cm", "0.8cm", "", "1", "2cm", "1cm"), DetailCellStyle("", "9pt", "Left", ""));
        AddReportItem(ref writer, "Textbox", "lblBMI39_RepFooter", "BMI 35~39", ElementChilds("2cm", "0.8cm", "", "1", "2cm", "4cm"), DetailCellStyle("", "9pt", "Left", ""));
        AddReportItem(ref writer, "Textbox", "lblBMI49_RepFooter", "BMI 40~49", ElementChilds("2cm", "0.8cm", "", "1", "2cm", "7cm"), DetailCellStyle("", "9pt", "Left", ""));
        AddReportItem(ref writer, "Textbox", "txtBMI35_RepFooter", BMI35.ToString(), ElementChilds("2cm", "0.8cm", "", "1", "2.7cm", "1cm"), DetailCellStyle("", "9pt", "Left", ""));
        AddReportItem(ref writer, "Textbox", "txtBMI39_RepFooter", BMI39.ToString(), ElementChilds("2cm", "0.8cm", "", "1", "2.7cm", "4cm"), DetailCellStyle("", "9pt", "Left", ""));
        AddReportItem(ref writer, "Textbox", "txtBMI49_RepFooter", BMI49.ToString(), ElementChilds("2cm", "0.8cm", "", "1", "2.7cm", "7cm"), DetailCellStyle("", "9pt", "Left", ""));

        if (BMITotal != 0)
        {
            AddReportItem(ref writer, "Textbox", "txtBMI35_Percent_RepFooter", "%" + Math.Round((decimal)BMI35 * 100 / (decimal)BMITotal, 1).ToString(), ElementChilds("2cm", "0.8cm", "", "1", "3.3cm", "1cm"), DetailCellStyle("", "9pt", "Left", ""));
            AddReportItem(ref writer, "Textbox", "txtBMI39_Percent_RepFooter", "%" + Math.Round((decimal)BMI39 * 100 / (decimal)BMITotal, 1).ToString(), ElementChilds("2cm", "0.8cm", "", "1", "3.3cm", "4cm"), DetailCellStyle("", "9pt", "Left", ""));
            AddReportItem(ref writer, "Textbox", "txtBMI49_Percent_RepFooter", "%" + Math.Round((decimal)BMI49 * 100 / (decimal)BMITotal, 1).ToString(), ElementChilds("2cm", "0.8cm", "", "1", "3.3cm", "7cm"), DetailCellStyle("", "9pt", "Left", ""));
        }
        else
        {
            AddReportItem(ref writer, "Textbox", "txtBMI35_Percent_RepFooter", "", ElementChilds("2cm", "0.8cm", "", "1", "3.3cm", "1cm"), DetailCellStyle("", "9pt", "Left", ""));
            AddReportItem(ref writer, "Textbox", "txtBMI39_Percent_RepFooter", "", ElementChilds("2cm", "0.8cm", "", "1", "3.3cm", "4cm"), DetailCellStyle("", "9pt", "Left", ""));
            AddReportItem(ref writer, "Textbox", "txtBMI49_Percent_RepFooter", "", ElementChilds("2cm", "0.8cm", "", "1", "3.3cm", "7cm"), DetailCellStyle("", "9pt", "Left", ""));
        }

        AddReportItem(ref writer, "Textbox", "lblBMI59_RepFooter", "BMI 50~59", ElementChilds("2cm", "0.8cm", "", "1", "4cm", "1cm"), DetailCellStyle("", "9pt", "Left", ""));
        AddReportItem(ref writer, "Textbox", "lblBMI69_RepFooter", "BMI 60~69", ElementChilds("2cm", "0.8cm", "", "1", "4cm", "4cm"), DetailCellStyle("", "9pt", "Left", ""));
        AddReportItem(ref writer, "Textbox", "lblBMI70_RepFooter", "BMI > 70", ElementChilds("2cm", "0.8cm", "", "1", "4cm", "7cm"), DetailCellStyle("", "9pt", "Left", ""));

        AddReportItem(ref writer, "Textbox", "txtBMI59_RepFooter", BMI59.ToString(), ElementChilds("2cm", "0.8cm", "", "1", "4.7cm", "1cm"), DetailCellStyle("", "9pt", "Left", ""));
        AddReportItem(ref writer, "Textbox", "txtBMI69_RepFooter", BMI69.ToString(), ElementChilds("2cm", "0.8cm", "", "1", "4.7cm", "4cm"), DetailCellStyle("", "9pt", "Left", ""));
        AddReportItem(ref writer, "Textbox", "txtBMI70_RepFooter", BMI70.ToString(), ElementChilds("2cm", "0.8cm", "", "1", "4.7cm", "7cm"), DetailCellStyle("", "9pt", "Left", ""));

        if (BMITotal != 0)
        {
            AddReportItem(ref writer, "Textbox", "txtBMI59_Percent_RepFooter", "%" + Math.Round((decimal)BMI59 * 100 / (decimal)BMITotal, 1).ToString(), ElementChilds("2cm", "0.8cm", "", "1", "5.3cm", "1cm"), DetailCellStyle("", "9pt", "Left", ""));
            AddReportItem(ref writer, "Textbox", "txtBMI69_Percent_RepFooter", "%" + Math.Round((decimal)BMI69 * 100 / (decimal)BMITotal, 1).ToString(), ElementChilds("2cm", "0.8cm", "", "1", "5.3cm", "4cm"), DetailCellStyle("", "9pt", "Left", ""));
            AddReportItem(ref writer, "Textbox", "txtBMI70_Percent_RepFooter", "%" + Math.Round((decimal)BMI70 * 100 / (decimal)BMITotal, 1).ToString(), ElementChilds("2cm", "0.8cm", "", "1", "5.3cm", "7cm"), DetailCellStyle("", "9pt", "Left", ""));
        }
        else
        {
            AddReportItem(ref writer, "Textbox", "txtBMI59_Percent_RepFooter", "" , ElementChilds("2cm", "0.8cm", "", "1", "5.3cm", "1cm"), DetailCellStyle("", "9pt", "Left", ""));
            AddReportItem(ref writer, "Textbox", "txtBMI69_Percent_RepFooter", "" , ElementChilds("2cm", "0.8cm", "", "1", "5.3cm", "4cm"), DetailCellStyle("", "9pt", "Left", ""));
            AddReportItem(ref writer, "Textbox", "txtBMI70_Percent_RepFooter", "" , ElementChilds("2cm", "0.8cm", "", "1", "5.3cm", "7cm"), DetailCellStyle("", "9pt", "Left", ""));
        }

    }
    #endregion

    #region private void OperationSubReport_RDL(DataSet dsReport, string strRDLFilename, string strLanguage)
    private void OperationSubReport_RDL(DataSet dsReport, string strRDLFilename, string strLanguage)
    {
        System.IO.FileStream stream = System.IO.File.OpenWrite(strRDLFilename);
        System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(stream, System.Text.Encoding.UTF8);

        writer.Formatting = System.Xml.Formatting.Indented;
        writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"utf-8\"");
        writer.WriteStartElement("Report");     //<Report>
        AddReportConfiguration(ref writer, "Operation Duration with LOS", strLanguage, (Decimal)22.5, (Decimal)0.5, (Decimal)0.5, (Decimal)0.5, (Decimal)0.5, (Decimal)29, "cm");
        AddDataSource(ref writer, dsReport);
        AddDataSets(ref writer, dsReport);
        #region Body
        writer.WriteStartElement("Body");   //<Body>
        writer.WriteElementString("Height", "28cm"); //Report.height - (TopMargin + bottomMargin) == 28 - (0.5+0.5)
        writer.WriteStartElement("ReportItems");//<ReportItems>

        writer.WriteStartElement("Table");          //<Table>
        writer.WriteAttributeString("Name", "tblOperation");
        writer.WriteElementString("DataSetName", "tblOperations");
        #region TableColumns
        writer.WriteStartElement("TableColumns");   //<TableColumns>
        AddColumn(ref writer, "0.5cm");
        AddColumn(ref writer, "0.5cm");
        AddColumn(ref writer, "6cm");
        AddColumn(ref writer, "1cm");
        writer.WriteEndElement();                   //<TableColumns>
        #endregion

        #region Header
        writer.WriteStartElement("Header");         //<Header>
        writer.WriteElementString("RepeatOnNewPage", "true");
        writer.WriteStartElement("TableRows");          //<TableRows>
        writer.WriteStartElement("TableRow");           //  <TableRow>
        writer.WriteElementString("Height", "1cm");
        writer.WriteStartElement("TableCells");         //      <TableCells>
        AddCell(ref writer, "Textbox", "txtSurgeonName", "=Fields!DoctorName_Title.Value", DetailCellStyle("", "", "", ""), "4");
        writer.WriteEndElement();                       //      </TableCells>
        writer.WriteEndElement();                       //  </TableRow>
        writer.WriteEndElement();                       //</TableRows>
        writer.WriteEndElement();                   //</Header>
        #endregion

        #region <TableGroups>
        writer.WriteStartElement("TableGroups");    //<TableGroups>

        writer.WriteStartElement("TableGroup");         //<TableGroup>  --  Surgury
        writer.WriteStartElement("Grouping");               //<Grouping>
        writer.WriteAttributeString("Name", "rptSurgeryGroup");
        writer.WriteStartElement("GroupExpressions");           //<GroupExpressions>
        writer.WriteElementString("GroupExpression", "=Fields!SurgeryType.Value");
        writer.WriteEndElement();                               //</GroupExpressions>
        writer.WriteEndElement();                           //</Grouping>
        writer.WriteStartElement("Header");                 //<Header>
        writer.WriteStartElement("TableRows");                  //<TableRows>

        writer.WriteStartElement("TableRow");                       //<TableRow>
        writer.WriteElementString("Height", "0.7cm");
        writer.WriteStartElement("TableCells");                         //<TableCells>
        AddCell(ref writer, "Textbox", "lblSurgeryDesc", "=Fields!SurgeryType_Desc.Value", DetailCellStyle("", "10pt", "", "LightSkyBlue"), "3");
        AddCell(ref writer, "Textbox", "txtSurgeryCount", "=count(Fields!SurgeryType.Value)", DetailCellStyle("Red", "10pt", "Center", "LightSkyBlue"), "");
        writer.WriteEndElement();                                       //</TableCells>
        writer.WriteEndElement();                                   //</TableRow>
        writer.WriteEndElement();                               //</TableRows>
        writer.WriteEndElement();                           //</Header>
        writer.WriteEndElement();                       //</TableGroup>

        writer.WriteStartElement("TableGroup");         //<TableGroup>  --  Approach
        writer.WriteStartElement("Grouping");               //<Grouping>
        writer.WriteAttributeString("Name", "rptApproachGroup");
        writer.WriteStartElement("GroupExpressions");           //<GroupExpressions>
        writer.WriteElementString("GroupExpression", "=Fields!Approach.Value");
        writer.WriteEndElement();                               //</GroupExpressions>
        writer.WriteEndElement();                           //</Grouping>
        writer.WriteStartElement("Header");                 //<Header>
        writer.WriteStartElement("TableRows");                  //<TableRows>
        writer.WriteStartElement("TableRow");                       //<TableRow>
        writer.WriteElementString("Height", "0.7cm");
        writer.WriteStartElement("TableCells");                         //<TableCells>
        AddCell(ref writer, "Textbox", "lblEmpty21", "", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "lblApproach", "=Fields!Approach.Value", DetailCellStyle("", "9pt", "", ""), "2");
        AddCell(ref writer, "Textbox", "txtApproachCount", "=count(Fields!Approach.Value)", DetailCellStyle("Red", "9pt", "Center", ""), "");
        writer.WriteEndElement();                                       //</TableCells>
        writer.WriteEndElement();                                   //</TableRow>
        writer.WriteEndElement();                               //</TableRows>
        writer.WriteEndElement();                           //</Header>
        writer.WriteEndElement();                       //</TableGroup>

        writer.WriteStartElement("TableGroup");         //<TableGroup>  --  Category
        writer.WriteStartElement("Grouping");               //<Grouping>
        writer.WriteAttributeString("Name", "rptCategoryGroup");
        writer.WriteStartElement("GroupExpressions");           //<GroupExpressions>
        writer.WriteElementString("GroupExpression", "=Fields!Category_Desc.Value");
        writer.WriteEndElement();                               //</GroupExpressions>
        writer.WriteEndElement();                           //</Grouping>
        writer.WriteStartElement("Header");                 //<Header>
        writer.WriteStartElement("TableRows");                  //<TableRows>
        writer.WriteStartElement("TableRow");                       //<TableRow>
        writer.WriteElementString("Height", "0.7cm");
        writer.WriteStartElement("TableCells");                         //<TableCells>
        AddCell(ref writer, "Textbox", "lblEmpty31", "", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "lblEmpty32", "", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "lblCategory", "=Fields!Category_Desc.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtCategoryCount", "=count(Fields!Category.Value)", DetailCellStyle("Red", "", "Center", ""), "");
        writer.WriteEndElement();                                       //</TableCells>
        writer.WriteEndElement();                                   //</TableRow>
        writer.WriteEndElement();                               //</TableRows>
        writer.WriteEndElement();                           //</Header>
        writer.WriteEndElement();                       //</TableGroup>
        writer.WriteEndElement();                   //</TableGroups>
        #endregion
        writer.WriteEndElement();                   //</Table>

        
        writer.WriteEndElement();           //ReportItems
        writer.WriteEndElement();       //</Body>
        #endregion
        writer.WriteEndElement();   //</Report>
        writer.Flush();
        stream.Close();
    }
    #endregion
    #endregion

    #region private void BMIEWLGraph_BuildReport()
    private void BMIEWLGraph_BuildReport()
    {
        DataSet dsReport;
        SqlCommand cmdSelect = new SqlCommand();

        chartEWL.Visible = true;
        chartBMI.Visible = true;
        InitializeReportCommand(ref cmdSelect, "sp_Rep_SummaryByQuarter");
        cmdSelect.Parameters.Add("@ImperialFlag", SqlDbType.Bit).Value = Request.Cookies["Imperial"].Value.Equals("True");
        cmdSelect.Parameters.Add("@ReportType", SqlDbType.Int).Value = 5;
        
        dsReport = gClass.FetchData(cmdSelect, "tblEWLGraph");
        SetChartConfig(chartEWL, dsReport, "% Excess Weight Loss", "Months since operations", "% EWL", "VisitMonthsSinceOperation", "avgEWL");
        SetChartConfig(chartBMI, dsReport, "Change in BMI per Quarter", "Months since operations", "BMI", "VisitMonthsSinceOperation", "avgBMI");
    }
    #endregion

    #region private void SetChartConfig(DataSet dsReport, string strChartTitle, string strXAxis , string strYAxis)
    private void SetChartConfig(Chart tChart, DataSet dsReport, string strChartTitle, string strXAxis , string strYAxis, string strXFieldName, string strYFieldName)
    {
        tChart.TempDirectory = "~/temp";
        tChart.Mentor = false;
        tChart.Use3D = false;
        tChart.Width = 800;
        tChart.Height = 450;
        tChart.Debug = true;
        tChart.LegendBox.Visible = false;

        tChart.Title = strChartTitle;
        tChart.DefaultSeries.Type = SeriesType.Bar;
        tChart.XAxis.Label.Text = strXAxis;//"Quarter since operations";
        tChart.YAxis.Label.Text = strYAxis;//"% EWL";
        SeriesCollection mySC = getEWLData(dsReport, strXFieldName, strYFieldName);
        tChart.SeriesCollection.Add(mySC);
        return;
    }
    #endregion 

    #region private SeriesCollection getEWLData(DataSet dsReport)
    private SeriesCollection getEWLData(DataSet dsReport, string strXFieldName, string strYFieldName)
    {
        SeriesCollection SC = new SeriesCollection();
        Series s = new Series();
        s.Name = "%EWL Series";

        Double value = 0;
        for (int xh = 0; xh < dsReport.Tables[0].Rows.Count; xh++)
        {
            try
            {
                dotnetCHARTING.Element e = new dotnetCHARTING.Element();
                e.ShowValue = true;
                e.Name = dsReport.Tables[0].Rows[xh]["VisitMonthsSinceOperation"].ToString();
                e.XValue = Convert.ToInt32(dsReport.Tables[0].Rows[xh][strXFieldName].ToString());
                value = Math.Round(Convert.ToDouble(dsReport.Tables[0].Rows[xh][strYFieldName].ToString()),1);
                e.YValue = value;
                e.SmartLabel.Text = value.ToString("#.#");
                s.Elements.Add(e);
            }
            catch { /*Response.Write(err.ToString());*/ }
        }
        SC.Add(s);
        SC[0].PaletteName = Palette.Two;
        return SC;
    }
    #endregion 

    #region private void BaseLineComorbidities_BuildReport
    private void BaseLineComorbidities_BuildReport(GlobalClass gClass)
    {
        SqlCommand cmdSelect = new SqlCommand();
        DataSet dsReport = new DataSet("dsSchema");
        DataColumn dcRootURL = new DataColumn("RootURL", Type.GetType("System.String"));

        dcRootURL.DefaultValue = Request.Url.Scheme + "://" + Request.Url.Host + "/";

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_Rep_BaselineComorbidities", true);
        cmdSelect.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value); 
        cmdSelect.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["PID"]); 
        cmdSelect.Parameters.Add("@ImperialFlag", SqlDbType.Bit).Value = Request.Cookies["Imperial"].Value.Equals("True");

        dsReport.Tables.Add(gClass.FetchData(cmdSelect, "tblReport").Tables[0].Copy());
        dsReport.Tables[0].Columns.Add(dcRootURL);
        dsReport.AcceptChanges();
        tcXML.InnerHtml = gClass.ShowSchema(dsReport, Server.MapPath(strXSLTFileName));
        return;
    }
    #endregion 

    #region private void ComorbidityFUAssessment_BuildReport
    private void ComorbidityFUAssessment_BuildReport(GlobalClass gClass)
    {
        SqlCommand      cmdSelect = new SqlCommand(), cmdSelectComplication = new SqlCommand(), cmdSysConfig = new SqlCommand();
        DataSet         dsReport = new DataSet("dsSchema");
        DataColumn      dcRootURL = new DataColumn("RootURL", Type.GetType("System.String"));

        dcRootURL.DefaultValue = Request.Url.Scheme + "://" + Request.Url.Host + "/";

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_Rep_ComorbidityFUAssessment", true);
        gClass.MakeStoreProcedureName(ref cmdSysConfig, "sp_SystemConfig", true);
        gClass.MakeStoreProcedureName(ref cmdSelectComplication, "sp_Rep_ComplicationEventPerPatient", true);

        cmdSelect.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
        cmdSelect.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);

        dsReport.Tables.Add(gClass.FetchData(cmdSelect, "tblReport").Tables[0].Copy());
        dsReport.Tables[0].Columns.Add(dcRootURL);
        dsReport.AcceptChanges();
        dsReport.Tables.Add(gClass.FetchData(cmdSysConfig, "tblSysConfig").Tables[0].Copy());

        cmdSelectComplication.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSelectComplication.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
        cmdSelectComplication.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
        cmdSelectComplication.Parameters.Add("@TypeCode", SqlDbType.VarChar).Value = 'c';

        dsReport.Tables.Add(gClass.FetchData(cmdSelectComplication, "tblComplication").Tables[0].Copy());

        cmdSelectComplication.Parameters["@TypeCode"].Value = 'e';
        dsReport.Tables.Add(gClass.FetchData(cmdSelectComplication, "tblEvent").Tables[0].Copy());

        tcXML.InnerHtml = gClass.ShowSchema(dsReport, Server.MapPath(strXSLTFileName));
        return;
    }
    #endregion

    #region private void GroupComorbidity_BuildReport
    private void GroupComorbidity_BuildReport(GlobalClass gClass)
    {
        SqlCommand cmdSelect = new SqlCommand();
        DataSet dsReport = new DataSet("dsSchema");
        string[] Fields = new string[]{"SurgonFlag", "HospitalFlag", "SurgeryTypeFlag", "ApproachFlag", "CategoryFlag", "GroupFlag"};

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_Rep_GroupComorbidity", true);

        dsReport.Tables.Add(gClass.FetchData(cmdSelect, "tblReport").Tables[0].Copy());
        for (int Xh = 0; Xh < 6; Xh++)
        {
            DataColumn myDC = new DataColumn(Fields[Xh], Type.GetType("System.Char"));
            myDC.DefaultValue = Request.Cookies["GroupComParam"].Value[Xh];
            dsReport.Tables[0].Columns.Add(myDC);
        }
        dsReport.Tables[0].AcceptChanges();
        //tcXML.Text = gClass.ShowSchema(dsReport, Server.MapPath(strXSLTFileName));
        tcXML.InnerHtml = gClass.ShowSchema(dsReport, Server.MapPath(strXSLTFileName));
        return;
    }
    #endregion 

    #region private void OperationDetails_BuildReport
    private void OperationDetails_BuildReport(GlobalClass gClass, string strParam)
    {
        DataSet         dsReport;
        SqlCommand      cmdSelectOperationData = new SqlCommand(),
                        cmdSelectPatientData = new SqlCommand(),
                        cmdSelectPatientComplication = new SqlCommand();

        strXSLTFileName = @"OperationDetails/en_OperationDetailsXSLTFile.xsl";

        gClass.MakeStoreProcedureName(ref cmdSelectPatientData, "sp_Rep_FollowUpAssessment", true);
        gClass.MakeStoreProcedureName(ref cmdSelectOperationData , "sp_Operation_SelectPatientOperationsList", true);
        gClass.MakeStoreProcedureName(ref cmdSelectPatientComplication, "sp_ConsultFU1_Complications_LoadData", true);

        cmdSelectPatientData.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = intOrganizationCode;
        cmdSelectPatientData.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = intUserPracticeCode;
        cmdSelectPatientData.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["PID"]);
        cmdSelectPatientData.Parameters.Add("@ImperialFlag", SqlDbType.Bit).Value = Request.Cookies["Imperial"].Value.Equals("True");

        cmdSelectOperationData.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSelectOperationData.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
        cmdSelectOperationData.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["PID"]);
        cmdSelectOperationData.Parameters.Add("@ImperialFlag", SqlDbType.Bit).Value = Request.Cookies["Imperial"].Value.Equals("True");

        cmdSelectPatientComplication.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSelectPatientComplication.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
        cmdSelectPatientComplication.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["PID"]);

        dsReport = gClass.FetchData(cmdSelectPatientData, "tblPatientData");
        dsReport.Tables.Add(gClass.FetchData(cmdSelectOperationData, "tblPatientOperation").Tables[0].Copy());

        tcXML.InnerHtml = gClass.ShowSchema(dsReport, Server.MapPath(strXSLTFileName));
        return;
    }
    #endregion 
        
    #region private void ACS_BuildReport()
    private void ACS_BuildReport()
    {
        string form = Request.QueryString["F"];
        string period = Request.QueryString["P"];
        string periodNum = Request.QueryString["PN"];

        string imperialMode = (Request.Cookies["Imperial"].Value == "True") ? "1" : "0";

        Phrase phrase = new Phrase("");
        DataSet dsPatientData = new DataSet();
        DataSet dsPatientEMR = new DataSet();
        DataSet dsPatientComorbidity = new DataSet();
        DataSet dsPatientBold = new DataSet();
        DataSet dsPatientVisit = new DataSet();
        DataSet dsPatientEvents = new DataSet();
        DataSet dsPatientFollowUp = new DataSet();
        String title1 = "ACS BARIATRIC SURGERY CENTER";
        String title2 = "NETWORK ACCREDITATION PROGRAM";
        String title3 = "Data Collection Worksheet";
        String title4 = "BSCN Case Number: ";
        String title5 = "ACS NSQIP Case Number: ";
        String title6 = "Last Name: ";
        String title7 = "First Name: ";
        String formTitle = "";
        String patientAlive = "Yes";
        String patientReadmitted = "No";
        String patientReoprtated = "No";

        try
        {
            dsPatientData = LoadPatientData();
            dsPatientEMR = LoadPatientEMR();
            dsPatientComorbidity = LoadPatientComorbidity();
            dsPatientBold = LoadPatientBold();
            dsPatientFollowUp = LoadPatientFollowUp();
            dsPatientEvents = LoadPatientEvents("", 0);

            if (dsPatientData.Tables.Count > 0)
            {
                string patientHeightMeasurment = (imperialMode == "1" ? "inches" : "cm");
                string patientWeightMeasurment = (imperialMode == "1" ? "lbs" : "kg");
                Decimal decTemp = 0m;

                //patient alive, patient readmitted, patient surgical
                for (int Xh = 0; Xh < dsPatientEvents.Tables[0].Rows.Count; Xh++)
                {
                    if (UppercaseFirst(dsPatientEvents.Tables[0].Rows[Xh]["Complication"].ToString()).IndexOf("Death") >= 0)
                        patientAlive = "No";

                    if (dsPatientEvents.Tables[0].Rows[Xh]["Readmitted"].ToString().Equals(Boolean.TrueString))
                        patientReadmitted = "Yes";

                    if (dsPatientEvents.Tables[0].Rows[Xh]["ReOperation"].ToString().Equals(Boolean.TrueString))
                        patientReoprtated = "Yes";
                }

                string[,] comorbiditiesACSArr = {{"ACS_Smoke","Current Smoker within 1 year","Pulmonary"},{"ACS_Oxy","Oxygen Dependent","Pulmonary"},{"ACS_Embo","History of Pulmonary Embolism","Pulmonary"},
                    {"ACS_Copd","History of Severe COPD","Pulmonary"},{"ACS_Cpap","Obstructive Sleep Apnea req. CPAP or BiPAP","Pulmonary"},{"ACS_Gerd","GERD req. medications","Gastrointestinal"},
                    {"ACS_Gal","Gallstone Disease","Gastrointestinal"},{"ACS_Muscd","Musculoskeletal Disease","Musculoskeletal"},{"ACS_Pain","Activity limited by pain","Musculoskeletal"},
                    {"ACS_Meds","Requires daily medication","Musculoskeletal"},{"ACS_Surg","Surgical intervention planned or performed","Musculoskeletal"},{"ACS_Mob","Uses mobility device","Musculoskeletal"},
                    {"GEN_RenalInsuff","Renal Insufficiency (Creat >2)","RENAL"},{"GEN_RenalFail","Renal Failure req. dialysis","RENAL"},{"ACS_Uri","Urinary Stress Incontinence","RENAL"},
                    {"ACS_Myo","History of Myocardinal Infarction","Cardiac"},{"ACS_Pci","Previous PCI","Cardiac"},{"ACS_Csurg","Previous Cardiac Surgery","Cardiac"},
                    {"ACS_Lipid","Hyperlipidemia req. medications","Cardiac"},{"ACS_Hyper","Hypertension req. medications","Cardiac"},{"ACS_Dvt","History of DVT requiring therapy","Vascular"},
                    {"ACS_Venous","Venous Stasis","Vascular"},{"ACS_Health","Functional Health Status Prior to Surgery","Other"},{"ACS_Diab","Diabetes Mellitus","Other"},
                    {"GEN_Steroid","Chronic Steroids/ Immunosuppression","Other"},{"GEN_Therapeutic","Therapeutic anticogulation","Other"},{"ACS_Obese","Previous obesity/ foregut surgery","Other"}
                    };

                #region declare table and width

                string fileName = Request.Cookies["PatientID"].Value + "-" + gClass.OrganizationCode;
                string saveFilePath = Server.MapPath(".\\.\\..\\temp\\") + fileName + ".pdf";
                string openFilePath = "..//temp//" + fileName + ".pdf";
                iTextSharp.text.Document oDoc = new iTextSharp.text.Document(PageSize.LETTER, 0, 0, 30f, 30f);
                PdfWriter.GetInstance(oDoc, new FileStream(saveFilePath, FileMode.Create));
                oDoc.Open();
                Cell cell = new Cell("");
                Font H2 = new Font(Font.HELVETICA, 10, Font.BOLD);
                Font H3 = new Font(Font.HELVETICA, 9, Font.BOLD);
                Font normalFont = new Font(Font.HELVETICA, 8, 0);
                Font normalBoldFont = new Font(Font.HELVETICA, 8, Font.BOLD);

                //create header
                iTextSharp.text.Table tableTitle = new iTextSharp.text.Table(3, 4);
                iTextSharp.text.Table tableDemographic = new iTextSharp.text.Table(3, 7);
                iTextSharp.text.Table tableSurgical = new iTextSharp.text.Table(1, 7);
                iTextSharp.text.Table tablePreoperativeInformation = new iTextSharp.text.Table(1, 40);
                iTextSharp.text.Table tablePreoperativeLabs = new iTextSharp.text.Table(3, 3);
                iTextSharp.text.Table tableMeasurement = new iTextSharp.text.Table(3, 7);
                iTextSharp.text.Table tableMedication = new iTextSharp.text.Table(1, 34);
                iTextSharp.text.Table tableOperativeInformation = new iTextSharp.text.Table(2, 50);
                iTextSharp.text.Table tablePostOperativeInformation = new iTextSharp.text.Table(3, 30);
                iTextSharp.text.Table tableFollowUp = new iTextSharp.text.Table(3, 30);
                iTextSharp.text.Table tableReadmission = new iTextSharp.text.Table(3, 30);
                iTextSharp.text.Table tableReoperation = new iTextSharp.text.Table(3, 30);
                iTextSharp.text.Table tableContact = new iTextSharp.text.Table(1, 10);
                iTextSharp.text.Table tableMortality = new iTextSharp.text.Table(1, 3);

                // set *column* widths
                float[] titleWidths = { .32f, .4f, .28f };
                tableTitle.Spacing = 1;
                tableTitle.Widths = titleWidths;
                tableTitle.DefaultCell.BorderWidth = 0;
                tableTitle.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                tableTitle.TableFitsPage = true;
                tableTitle.BorderColor = new Color(255, 255, 255);

                float[] demographicWidths = { .3f, .3f, .4f};
                tableDemographic.Spacing = 1;
                tableDemographic.Widths = demographicWidths;
                tableDemographic.DefaultCell.BorderWidth = 0;
                tableDemographic.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                tableDemographic.TableFitsPage = true;
                tableDemographic.BorderColor = new Color(255, 255, 255);

                float[] surgicalWidths = { 1f };
                tableSurgical.Spacing = 1;
                tableSurgical.Widths = surgicalWidths;
                tableSurgical.DefaultCell.BorderWidth = 0;
                tableSurgical.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                tableSurgical.TableFitsPage = true;
                tableSurgical.BorderColor = new Color(255, 255, 255);

                float[] preoperativeInformationWidths = { 1f };
                tablePreoperativeInformation.Spacing = 1;
                tablePreoperativeInformation.Widths = preoperativeInformationWidths;
                tablePreoperativeInformation.DefaultCell.BorderWidth = 0;
                tablePreoperativeInformation.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                tablePreoperativeInformation.TableFitsPage = true;
                tablePreoperativeInformation.BorderColor = new Color(255, 255, 255);

                float[] preoperativeLabsWidths = { .2f, .15f , .65f };
                tablePreoperativeLabs.Spacing = 1;
                tablePreoperativeLabs.Widths = preoperativeLabsWidths;
                tablePreoperativeLabs.DefaultCell.BorderWidth = 0;
                tablePreoperativeLabs.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                tablePreoperativeLabs.TableFitsPage = true;
                tablePreoperativeLabs.BorderColor = new Color(255, 255, 255);

                float[] medicationWidths = { 1f };
                tableMedication.Spacing = 1;
                tableMedication.Widths = medicationWidths;
                tableMedication.DefaultCell.BorderWidth = 0;
                tableMedication.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                tableMedication.TableFitsPage = true;
                tableMedication.BorderColor = new Color(255, 255, 255);

                float[] operativeInformationWidths = { .25f, .75f };
                tableOperativeInformation.Spacing = 1;
                tableOperativeInformation.Widths = operativeInformationWidths;
                tableOperativeInformation.DefaultCell.BorderWidth = 0;
                tableOperativeInformation.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                tableOperativeInformation.TableFitsPage = true;
                tableOperativeInformation.BorderColor = new Color(255, 255, 255);
                
                float[] postOperativeInformationWidths = { .4f, .15f, .45f};
                tablePostOperativeInformation.Spacing = 1;
                tablePostOperativeInformation.Widths = postOperativeInformationWidths;
                tablePostOperativeInformation.DefaultCell.BorderWidth = 0;
                tablePostOperativeInformation.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                tablePostOperativeInformation.TableFitsPage = true;
                tablePostOperativeInformation.BorderColor = new Color(255, 255, 255);
                
                float[] followupWidths = { .2f, .4f, .4f };
                tableFollowUp.Spacing = 1;
                tableFollowUp.Widths = followupWidths;
                tableFollowUp.DefaultCell.BorderWidth = 0;
                tableFollowUp.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                tableFollowUp.TableFitsPage = true;
                tableFollowUp.BorderColor = new Color(255, 255, 255);

                float[] readmissionWidths = { .2f, .2f, .6f };
                tableReadmission.Spacing = 1;
                tableReadmission.Widths = readmissionWidths;
                tableReadmission.DefaultCell.BorderWidth = 0;
                tableReadmission.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                tableReadmission.TableFitsPage = true;
                tableReadmission.BorderColor = new Color(255, 255, 255);

                float[] reOperationWidths = { .2f, .2f, .6f };
                tableReoperation.Spacing = 1;
                tableReoperation.Widths = reOperationWidths;
                tableReoperation.DefaultCell.BorderWidth = 0;
                tableReoperation.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                tableReoperation.TableFitsPage = true;
                tableReoperation.BorderColor = new Color(255, 255, 255);

                float[] mortalityWidths = { 1f };
                tableMortality.Spacing = 1;
                tableMortality.Widths = mortalityWidths;
                tableMortality.DefaultCell.BorderWidth = 0;
                tableMortality.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                tableMortality.TableFitsPage = true;
                tableMortality.BorderColor = new Color(255, 255, 255);

                float[] contactWidths = { 1f };
                tableContact.Spacing = 1;
                tableContact.Widths = contactWidths;
                tableContact.DefaultCell.BorderWidth = 0;
                tableContact.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                tableContact.TableFitsPage = true;
                tableContact.BorderColor = new Color(255, 255, 255);
                #endregion

                #region var declaration BOLD
                //patient BOLD declaration
                string demographicSSN = "";
                if (dsPatientBold.Tables.Count > 0 && (dsPatientBold.Tables[0].Rows.Count > 0))
                {
                    demographicSSN = dsPatientBold.Tables[0].Rows[0]["SocialSecurityNumber"].ToString();
                }
                #endregion


                #region var declaration Demographic
                //var declaration Demographic
                string demographicTitle = "Demographic";
                string demographicSurname = dsPatientData.Tables[0].Rows[0]["Surname"].ToString();
                string demographicFirstname = dsPatientData.Tables[0].Rows[0]["Firstname"].ToString();
                string demographicBirthDate = dsPatientData.Tables[0].Rows[0]["BirthDate"].ToString();
                string demographicDoctorName = dsPatientData.Tables[0].Rows[0]["DoctorName"].ToString().Trim();
                string demographicStreet = dsPatientData.Tables[0].Rows[0]["Street"].ToString().Trim();
                string demographicSuburb = dsPatientData.Tables[0].Rows[0]["Suburb"].ToString().Trim();
                string demographicState = dsPatientData.Tables[0].Rows[0]["State"].ToString().Trim();
                string demographicPostCode = dsPatientData.Tables[0].Rows[0]["PostCode"].ToString().Trim();

                string demographicHomePhone = dsPatientData.Tables[0].Rows[0]["HomePhone"].ToString().Trim();
                string demographicWorkPhone = dsPatientData.Tables[0].Rows[0]["WorkPhone"].ToString().Trim();
                string demographicMobile = dsPatientData.Tables[0].Rows[0]["MobilePhone"].ToString().Trim();
                string demographicGender = dsPatientData.Tables[0].Rows[0]["Sex"].ToString().Equals("M") ? "Male" : "Female";
                string demographicRace = dsPatientData.Tables[0].Rows[0]["Race"].ToString() != "" ? getDescription(dsPatientData.Tables[0].Rows[0]["Race"].ToString(), "demo") : "";

                #endregion
                #region form 1
                if (form == "1")
                {
                    formTitle = "Form 1: Preoperative Information";
                    
                    #region Demographic
                    //Demographic-----------------------------------------------------------------------------------------

                    phrase = new Phrase("\n" + demographicTitle, H3);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 3;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("SSN: " + demographicSSN, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 3;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("Last Name: " + demographicSurname, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("First Name: " + demographicFirstname, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("Address: " + demographicStreet, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 3;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("City: " + demographicSuburb, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("State: " + demographicState, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("PostCode: " + demographicPostCode, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("DOB: " + demographicBirthDate, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("Gender: " + demographicGender, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("Race: " + demographicRace, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);
                    #endregion

                    #region var declaration Surgical Profile
                    //var declaration Surgical Profile
                    string surgicalTitle = "Surgical Profile";
                    string surgicalOperationDate = dsPatientData.Tables[0].Rows[0]["strLapBandDate"].ToString();
                    string surgicalProcedure = dsPatientData.Tables[0].Rows[0]["SurgeryType_Desc"].ToString();
                    string surgicalBandType = dsPatientData.Tables[0].Rows[0]["BandType_Desc"].ToString();
                    string surgicalDoctorName = dsPatientData.Tables[0].Rows[0]["DoctorName"].ToString().Trim();

                    #endregion
                    #region Surgical Profile
                    //Surgical Profile------------------------------------------------------------------------------------

                    phrase = new Phrase("\n" + surgicalTitle, H3);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableSurgical.AddCell(cell);

                    phrase = new Phrase("Date of Hospital Presentation: " + surgicalOperationDate, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableSurgical.AddCell(cell);

                    phrase = new Phrase("Date of Operation: " + surgicalOperationDate, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableSurgical.AddCell(cell);

                    phrase = new Phrase("Principal Procedure: " + surgicalProcedure, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableSurgical.AddCell(cell);

                    phrase = new Phrase("Band Brand: " + surgicalBandType, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableSurgical.AddCell(cell);

                    phrase = new Phrase("Surgeon: " + surgicalDoctorName, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableSurgical.AddCell(cell);
                    #endregion

                    #region var declaration Preoperative Information
                    //var declaration Preoperative Information
                    string preopTitle = "Preoperative Information";
                    string comorbidities = "";
                    string comorbiditiesName = "";
                    string comorbiditiesChoice = "";
                    string comorbiditiesNotes = "";
                    string preopPreviousSection = "";
                    string preopCurrentSection = "";

                    string preopHeight = dsPatientData.Tables[0].Rows[0]["Height"].ToString();
                    string preopWeight = dsPatientData.Tables[0].Rows[0]["OpWeight"].ToString();
                    
                    if (imperialMode.Equals("1"))
                    {
                        preopHeight = Decimal.TryParse(preopHeight, out decTemp) ? (decTemp / (Decimal)0.0254).ToString(strNumberFormat) : "0";
                        preopWeight = Decimal.TryParse(preopWeight, out decTemp) ? (decTemp / (Decimal)0.45359237).ToString(strNumberFormat) : "0";
                    }
                    else
                    {
                        preopHeight = Decimal.TryParse(preopHeight, out decTemp) ? (decTemp * 100).ToString(strNumberFormat) : "0";
                        preopWeight = Decimal.TryParse(preopWeight, out decTemp) ? decTemp.ToString(strNumberFormat) : "0";
                    }

                    #endregion
                    #region Preoperative Information
                    //Preoperative Information-----------------------------------------------------------------------------
                    phrase = new Phrase("\n" + preopTitle, H3);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tablePreoperativeInformation.AddCell(cell);

                    phrase = new Phrase("Height: " + preopHeight + " " + patientHeightMeasurment, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tablePreoperativeInformation.AddCell(cell);

                    phrase = new Phrase("Weight: " + preopWeight + " " + patientWeightMeasurment, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tablePreoperativeInformation.AddCell(cell);

                    if (dsPatientComorbidity.Tables.Count > 0 && (dsPatientComorbidity.Tables[0].Rows.Count > 0))
                    {
                        for (int Idx = 0; Idx < comorbiditiesACSArr.Length / 3; Idx++)
                        {
                            comorbidities = comorbiditiesACSArr[Idx, 0];
                            comorbiditiesName = comorbiditiesACSArr[Idx, 1];
                            comorbiditiesNotes = "";
                            preopCurrentSection = comorbiditiesACSArr[Idx, 2];


                            if (dsPatientComorbidity.Tables[0].Rows[0][comorbidities].ToString() != "")
                            {
                                if (preopPreviousSection != preopCurrentSection)
                                {
                                    phrase = new Phrase(preopCurrentSection, normalBoldFont);
                                    cell = new Cell(phrase);
                                    cell.BorderWidth = 0;
                                    tablePreoperativeInformation.AddCell(cell);
                                }

                                comorbiditiesChoice = getDescription(dsPatientComorbidity.Tables[0].Rows[0][comorbidities].ToString(), "com");

                                if (comorbidities == "ACS_Hyper")
                                {
                                    comorbiditiesNotes = ", # of antihypertensive meds: " + dsPatientComorbidity.Tables[0].Rows[0][comorbidities + "_Notes"].ToString().Trim();
                                }
                                if (comorbiditiesName != "")
                                {
                                    phrase = new Phrase("- " + comorbiditiesName + ": " + comorbiditiesChoice + comorbiditiesNotes, normalFont);
                                    cell = new Cell(phrase);
                                    cell.BorderWidth = 0;
                                    tablePreoperativeInformation.AddCell(cell);
                                }
                            }
                            preopPreviousSection = preopCurrentSection;
                        }
                    }
                    #endregion

                    #region var declaration Preoperative Labs
                    //var declaration Preoperative Labs
                    string preopLabsTitle = "Preoperative Labs";
                    string preopLabsHematocrit = dsPatientData.Tables[0].Rows[0]["PreopHematocrit"].ToString();
                    string preopLabsAlbumin = dsPatientData.Tables[0].Rows[0]["PreopAlbumin"].ToString();
                    string preopLabsHematocritDate = dsPatientData.Tables[0].Rows[0]["strPreopHematocritDate"].ToString();
                    string preopLabsAlbuminDate = dsPatientData.Tables[0].Rows[0]["strPreopAlbuminDate"].ToString();
                    #endregion
                    #region Preoperative Labs
                    //Preoperative Labs----------------------------------------------------------------------------------

                    phrase = new Phrase("\n" + preopLabsTitle, H3);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 3;
                    tablePreoperativeLabs.AddCell(cell);

                    phrase = new Phrase("Preop Hematocrit:", normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tablePreoperativeLabs.AddCell(cell);

                    phrase = new Phrase(preopLabsHematocritDate, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tablePreoperativeLabs.AddCell(cell);

                    phrase = new Phrase(preopLabsHematocrit, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tablePreoperativeLabs.AddCell(cell);

                    phrase = new Phrase("Preop Albumin:", normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tablePreoperativeLabs.AddCell(cell);

                    phrase = new Phrase(preopLabsAlbuminDate, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tablePreoperativeLabs.AddCell(cell);

                    phrase = new Phrase(preopLabsAlbumin, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tablePreoperativeLabs.AddCell(cell);
                    #endregion

                    #region var declaration Medication
                    //var declaration Medication
                    string medicationTitle = "Current Medications";
                    string medicationName = "";
                    string medicationDosage = "";
                    string medicationFrequency = "";
                    String[] medications;
                    String[] medication;

                    #endregion
                    #region Medications
                    //Medications-----------------------------------------------------------------------------------
                    phrase = new Phrase("\n" + medicationTitle, H3);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableMedication.AddCell(cell);

                    if (dsPatientEMR.Tables.Count > 0 && (dsPatientEMR.Tables[0].Rows.Count > 0))
                    {
                        if (dsPatientEMR.Tables[0].Rows[0]["Medication"].ToString().Trim() != "")
                        {
                            medications = dsPatientEMR.Tables[0].Rows[0]["Medication"].ToString().Split('+');

                            foreach (string tempMedications in medications)
                            {
                                medication = tempMedications.Split('*');
                                medicationName = medication[0];
                                medicationDosage = medication[1].Trim() != "" ? ", " + medication[1].Trim() : "";
                                medicationFrequency = medication[2].Trim() != "" ? ", " + medication[2].Trim() : "";

                                if (medicationName != "")
                                {
                                    phrase = new Phrase("- " + medicationName + medicationDosage + medicationFrequency, normalFont);
                                    cell = new Cell(phrase);
                                    cell.BorderWidth = 0;
                                    tableMedication.AddCell(cell);
                                }
                            }
                        }
                    }
                    #endregion
                }
                #endregion
                
                #region form 2
                else if (form == "2")
                {
                    formTitle = "Form 2: Intraoperative Information";

                    #region var declaration Operative Information
                    //var declaration Operative Information
                    string opInfoASA = dsPatientData.Tables[0].Rows[0]["ASACode"].ToString() != "" ? getDescription(dsPatientData.Tables[0].Rows[0]["ASACode"].ToString(), "asa") : "";
                    String[] opAssistants;
                    string opAllAssistants = "";
                    string opInfoApproach = dsPatientData.Tables[0].Rows[0]["Approach_Desc"].ToString().Trim();

                    string opTimeTitle = "Operative Times";
                    string opTimePatientIn = dsPatientData.Tables[0].Rows[0]["InRoomTime"].ToString().Trim();
                    string opTimePatientOut = dsPatientData.Tables[0].Rows[0]["OutRoomTime"].ToString().Trim();
                    string opTimeSurgeryStart = dsPatientData.Tables[0].Rows[0]["SurgeryStartTime"].ToString().Trim();
                    string opTimeSurgeryEnd = dsPatientData.Tables[0].Rows[0]["SurgeryEndTime"].ToString().Trim();

                    string opInfoTranfusion = dsPatientData.Tables[0].Rows[0]["NumberIntraopTranfusion"].ToString().Trim();

                    string opOtherProcedureTitle = "Other Procedures";
                    string opOtherProcedureName = "";
                    string opOtherProcedureCode = "";
                    String[] opOtherProcedures;
                    String[] opOtherProcedure;

                    string opConcurrentProcedureTitle = "Concurrent Procedures";
                    string opConcurrentProcedureName = "";
                    String[] opConcurrentProcedures;

                    if (dsPatientData.Tables[0].Rows[0]["FirstAssistant"].ToString().Trim() != "")
                    {
                        opAssistants = dsPatientData.Tables[0].Rows[0]["FirstAssistant"].ToString().Split('-');
                        foreach (string tempAssistants in opAssistants)
                        {
                            opAllAssistants += getDescription(tempAssistants, "assistant") + ", ";
                        }
                        opAllAssistants = opAllAssistants.Substring(0, opAllAssistants.Length - 2);
                    }
                    
                    #endregion
                    #region Operative Information
                    //Operative Information-----------------------------------------------------------------------

                    phrase = new Phrase("\n" + "ASA Class: " + opInfoASA, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    tableOperativeInformation.AddCell(cell);

                    phrase = new Phrase("First Assistant: " + opAllAssistants, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    tableOperativeInformation.AddCell(cell);

                    phrase = new Phrase("Approach: " + opInfoApproach, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    tableOperativeInformation.AddCell(cell);

                    phrase = new Phrase("\n" + opTimeTitle, H3);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    tableOperativeInformation.AddCell(cell);
                    
                    phrase = new Phrase("Patient In Room Time:", normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableOperativeInformation.AddCell(cell);

                    phrase = new Phrase(opTimePatientIn, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableOperativeInformation.AddCell(cell);

                    phrase = new Phrase("Surgery Start Time:", normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableOperativeInformation.AddCell(cell);

                    phrase = new Phrase(opTimeSurgeryStart, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableOperativeInformation.AddCell(cell);

                    phrase = new Phrase("Surgery Finish Time:", normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableOperativeInformation.AddCell(cell);

                    phrase = new Phrase(opTimeSurgeryEnd, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableOperativeInformation.AddCell(cell);

                    phrase = new Phrase("Patient Out Room Time:", normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableOperativeInformation.AddCell(cell);

                    phrase = new Phrase(opTimePatientOut, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableOperativeInformation.AddCell(cell);
                    
                    phrase = new Phrase("\n" + "Blood Tranfusions: " + opInfoTranfusion, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    tableOperativeInformation.AddCell(cell);


                    phrase = new Phrase("\n" + opConcurrentProcedureTitle, H3);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    tableOperativeInformation.AddCell(cell);

                    if (dsPatientData.Tables[0].Rows[0]["Concurrent"].ToString().Trim() != "")
                    {
                        opConcurrentProcedures = dsPatientData.Tables[0].Rows[0]["Concurrent"].ToString().Split(';');

                        foreach (string tempOpConcurrentProcedures in opConcurrentProcedures)
                        {
                            opConcurrentProcedureName = tempOpConcurrentProcedures;

                            if (opConcurrentProcedureName != "")
                            {
                                phrase = new Phrase("- " + getDescription(opConcurrentProcedureName,"concurrent"), normalFont);
                                cell = new Cell(phrase);
                                cell.BorderWidth = 0;
                                cell.Colspan = 2;
                                tableOperativeInformation.AddCell(cell);
                            }
                        }
                    }

                    phrase = new Phrase("\n" + opOtherProcedureTitle, H3);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    tableOperativeInformation.AddCell(cell);
                    if (dsPatientData.Tables[0].Rows[0]["OtherProcedure"].ToString().Trim() != "")
                    {
                        opOtherProcedures = dsPatientData.Tables[0].Rows[0]["OtherProcedure"].ToString().Split('+');

                        foreach (string tempOpOtherProcedures in opOtherProcedures)
                        {
                            opOtherProcedure = tempOpOtherProcedures.Split('*');
                            opOtherProcedureName = opOtherProcedure[0];
                            opOtherProcedureCode = opOtherProcedure[1].Trim() != "" ? " (" + opOtherProcedure[1].Trim() + ")" : "";

                            if (opOtherProcedureName != "")
                            {
                                phrase = new Phrase("- " + opOtherProcedureName + opOtherProcedureCode, normalFont);
                                cell = new Cell(phrase);
                                cell.BorderWidth = 0;
                                cell.Colspan = 2;
                                tableOperativeInformation.AddCell(cell);
                            }
                        }
                    }
                    #endregion
                }
                #endregion

                #region form 3
                else if (form == "3")
                {
                    formTitle = "Form 3: Discharge & 30-day Postop Occurence Information";

                    #region var declaration Postoperative Information
                    //var declaration Postoperative Information
                    string postopTitle = "30-day Postoperative Occurence";
                    string postopAdditionalTitle = "Additional Postoperative Hospital Information from Bariatric Index Procedure Stay";
                    string postopAdditionalBlood = dsPatientData.Tables[0].Rows[0]["NumberBloodTranfusion"].ToString();
                    string postopAdditionalUnplannedAdmission = dsPatientData.Tables[0].Rows[0]["UnplannedAdmission"].ToString().Equals(Boolean.TrueString) ? "Yes" : "No";
                    string postopAdditionalTransferAcute = dsPatientData.Tables[0].Rows[0]["TransferAcuteCare"].ToString().Equals(Boolean.TrueString) ? "Yes" : "No";
                    string postopAdditionalDischargeDate = dsPatientData.Tables[0].Rows[0]["strDischargeDate"].ToString();

                    #endregion
                    #region Postoperative Information
                    //Postoperative Operation---------------------------------------------------------------------

                    phrase = new Phrase("\n" + postopTitle, H3);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 3;
                    tablePostOperativeInformation.AddCell(cell);

                    if (dsPatientEvents.Tables.Count > 0 && (dsPatientEvents.Tables[0].Rows.Count > 0))
                    {
                        for (int Xh = 0; Xh < dsPatientEvents.Tables[0].Rows.Count; Xh++)
                        {
                            phrase = new Phrase("- " + dsPatientEvents.Tables[0].Rows[Xh]["Complication"].ToString().Trim(), normalFont);
                            cell = new Cell(phrase);
                            cell.BorderWidth = 0;
                            tablePostOperativeInformation.AddCell(cell);

                            phrase = new Phrase(dsPatientEvents.Tables[0].Rows[Xh]["strComplicationDate"].ToString(), normalFont);
                            cell = new Cell(phrase);
                            cell.BorderWidth = 0;
                            tablePostOperativeInformation.AddCell(cell);

                            phrase = new Phrase(dsPatientEvents.Tables[0].Rows[Xh]["Notes"].ToString().Trim(), normalFont);
                            cell = new Cell(phrase);
                            cell.BorderWidth = 0;
                            tablePostOperativeInformation.AddCell(cell);
                        }
                    }
                    else
                    {
                        phrase = new Phrase("- " , normalFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        cell.Colspan = 3;
                        tablePostOperativeInformation.AddCell(cell);
                    }

                    phrase = new Phrase("\n" + postopAdditionalTitle, H3);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 3;
                    tablePostOperativeInformation.AddCell(cell);
                    
                    phrase = new Phrase("Number of blood tranfusions within first 72 hours: " + postopAdditionalBlood , normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 3;
                    tablePostOperativeInformation.AddCell(cell);
                    
                    phrase = new Phrase("Unplanned Admission to the ICU within 30 days: " +postopAdditionalUnplannedAdmission, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 3;
                    tablePostOperativeInformation.AddCell(cell);

                    phrase = new Phrase("Transfer to Acute Care Hospital within 30 days: " + postopAdditionalTransferAcute, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 3;
                    tablePostOperativeInformation.AddCell(cell);

                    phrase = new Phrase("Date of Hospital Discharge: ", normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 3;
                    tablePostOperativeInformation.AddCell(cell);

                    phrase = new Phrase(postopAdditionalDischargeDate, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tablePostOperativeInformation.AddCell(cell);
                    #endregion
                }
                #endregion
                
                #region form 4 5
                else if (form == "4" || form == "5")
                {
                    if (form == "4")
                    {
                        dsPatientVisit = LoadPatientVisit("d", 35);
                        formTitle = "Form 4: 30-day Follow-up Information";

                        period = "days";
                        periodNum = "30";
                    }
                    else if (form == "5")
                    {
                        if (periodNum == "")
                            periodNum = "0";

                        dsPatientVisit = LoadPatientVisit(period, Convert.ToInt32(periodNum));
                        formTitle = "Form 5: Long-Term Follow-up Information";

                        if (period == "d")
                            period = "days";
                        else if (period =="m")
                            period = "months";
                        else if (period =="y")
                            period = "years";
                    }                                  

                    #region var declaration Follow Up
                    //var declaration Follow Up
                    string followupWeight = "";
                    string followupHeight = dsPatientData.Tables[0].Rows[0]["Height"].ToString();
                    if (imperialMode.Equals("1"))
                        followupHeight = Decimal.TryParse(followupHeight, out decTemp) ? (decTemp / (Decimal)0.0254).ToString(strNumberFormat) : "0";
                    else
                        followupHeight = Decimal.TryParse(followupHeight, out decTemp) ? (decTemp * 100).ToString(strNumberFormat) : "0";
                    
                    string followupComorbiditiesTitle = "Comorbidity List";
                    string followupComorbidities = "";
                    string followupComorbiditiesName = "";
                    string followupComorbiditiesChoice = "";
                    int followupComorbidityVisitRecord = -1;

                    string followupReadmissionTitle = "Readmissions/Reoperations";

                    #endregion

                    #region Follow Up
                    phrase = new Phrase("\n" + "Period: " + periodNum.ToString() + " " + period, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 3;
                    tableFollowUp.AddCell(cell);

                    phrase = new Phrase("\n" + "Is the patient alive?: " + patientAlive, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 3;
                    tableFollowUp.AddCell(cell);

                    if (dsPatientVisit.Tables.Count > 0 && (dsPatientVisit.Tables[0].Rows.Count > 0))
                    {
                        phrase = new Phrase("\n" + "Height: " + followupHeight + " " + patientHeightMeasurment, normalFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        cell.Colspan = 3;
                        tableFollowUp.AddCell(cell);

                        for (int Xh = 0; Xh < dsPatientVisit.Tables[0].Rows.Count; Xh++)
                        {
                            followupWeight = dsPatientVisit.Tables[0].Rows[Xh]["weight"].ToString();
                            
                            if (imperialMode.Equals("1"))
                                followupWeight = Decimal.TryParse(followupWeight, out decTemp) ? (decTemp / (Decimal)0.45359237).ToString(strNumberFormat) : "0";
                            else
                                followupWeight = Decimal.TryParse(followupWeight, out decTemp) ? decTemp.ToString(strNumberFormat) : "0";
                            
                            phrase = new Phrase("Weight: " + followupWeight + " " + patientWeightMeasurment, normalFont);
                            cell = new Cell(phrase);
                            cell.BorderWidth = 0;
                            tableFollowUp.AddCell(cell);

                            phrase = new Phrase("Date weight taken: " + dsPatientVisit.Tables[0].Rows[Xh]["strDateSeen"].ToString(), normalFont);
                            cell = new Cell(phrase);
                            cell.BorderWidth = 0;
                            tableFollowUp.AddCell(cell);

                            phrase = new Phrase("BMI: " + dsPatientVisit.Tables[0].Rows[Xh]["VisitBMI"].ToString(), normalFont);
                            cell = new Cell(phrase);
                            cell.BorderWidth = 0;
                            tableFollowUp.AddCell(cell);

                            if (dsPatientVisit.Tables[0].Rows[Xh]["ComorbidityVisit"].ToString().Equals(Boolean.TrueString))
                                followupComorbidityVisitRecord = Xh;
                        }

                        if (followupComorbidityVisitRecord >= 0)
                        {
                            phrase = new Phrase("\n" + followupComorbiditiesTitle, H3);
                            cell = new Cell(phrase);
                            cell.BorderWidth = 0;
                            cell.Colspan = 3;
                            tableFollowUp.AddCell(cell);

                            for (int Idx = 0; Idx < comorbiditiesACSArr.Length / 3; Idx++)
                            {
                                followupComorbidities = comorbiditiesACSArr[Idx, 0];
                                followupComorbiditiesName = comorbiditiesACSArr[Idx, 1];

                                if (dsPatientVisit.Tables[0].Rows[followupComorbidityVisitRecord][followupComorbidities].ToString() != "")
                                {
                                    followupComorbiditiesChoice = getDescription(dsPatientVisit.Tables[0].Rows[followupComorbidityVisitRecord][followupComorbidities].ToString(), "com");

                                    if (followupComorbiditiesName != "")
                                    {
                                        phrase = new Phrase("- " + followupComorbiditiesName + ": " + followupComorbiditiesChoice, normalFont);
                                        cell = new Cell(phrase);
                                        cell.BorderWidth = 0;
                                        cell.Colspan = 3;
                                        tableFollowUp.AddCell(cell);
                                    }
                                }
                            }
                        }
                    }

                    phrase = new Phrase("\n" + followupReadmissionTitle, H3);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 3;
                    tableFollowUp.AddCell(cell);

                    phrase = new Phrase("Was the patient readmitted to the hospital following discharge from the index bariatric hospital stay? " + patientReadmitted, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 3;
                    tableFollowUp.AddCell(cell);

                    phrase = new Phrase("Did the patient have any surgical operations performed after the index bariatric procedure? " + patientReoprtated, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 3;
                    tableFollowUp.AddCell(cell);

                    #endregion
                }
                #endregion

                #region form 6
                else if (form == "6")
                {
                    formTitle = "Form 6: Readmission Information";
                    
                    if (periodNum == "")
                        periodNum = "0";

                    dsPatientEvents = LoadPatientEvents(period, Convert.ToInt32(periodNum));

                    if (period == "d")
                        period = "days";
                    else if (period == "m")
                        period = "months";
                    else if (period == "y")
                        period = "years";

                    #region var declaration Hospital Readmission
                    //var declaration Hospital Readmission
                    string readmissionTitle = "Hospital Readmission";
                    string readmissionReason = "";
                    int readmissionNum = 0;
                    #endregion
                    #region Hospital Readmission

                    phrase = new Phrase("\n" + "Period: " + periodNum.ToString() + " " + period, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 3;
                    tableReadmission.AddCell(cell);

                    phrase = new Phrase("\n" + readmissionTitle, H3);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 3;
                    tableReadmission.AddCell(cell);

                    if (dsPatientEvents.Tables.Count > 0 && (dsPatientEvents.Tables[0].Rows.Count > 0))
                    {
                        for (int Xh = 0; Xh < dsPatientEvents.Tables[0].Rows.Count; Xh++)
                        {
                            if (dsPatientEvents.Tables[0].Rows[Xh]["Readmitted"].ToString().Equals(Boolean.TrueString))
                            {
                                readmissionNum++;

                                phrase = new Phrase("Readmission #" + readmissionNum + ": ", normalFont);
                                cell = new Cell(phrase);
                                cell.BorderWidth = 0;
                                tableReadmission.AddCell(cell);

                                phrase = new Phrase("Date of Admission:", normalFont);
                                cell = new Cell(phrase);
                                cell.BorderWidth = 0;
                                tableReadmission.AddCell(cell);

                                phrase = new Phrase(dsPatientEvents.Tables[0].Rows[Xh]["strAdmitDate"].ToString(), normalFont);
                                cell = new Cell(phrase);
                                cell.BorderWidth = 0;
                                tableReadmission.AddCell(cell);

                                phrase = new Phrase("", normalFont);
                                cell = new Cell(phrase);
                                cell.BorderWidth = 0;
                                tableReadmission.AddCell(cell);

                                phrase = new Phrase("Date of Discharge:", normalFont);
                                cell = new Cell(phrase);
                                cell.BorderWidth = 0;
                                tableReadmission.AddCell(cell);

                                phrase = new Phrase(dsPatientEvents.Tables[0].Rows[Xh]["strDischargeDate"].ToString(), normalFont);
                                cell = new Cell(phrase);
                                cell.BorderWidth = 0;
                                tableReadmission.AddCell(cell);

                                phrase = new Phrase("", normalFont);
                                cell = new Cell(phrase);
                                cell.BorderWidth = 0;
                                tableReadmission.AddCell(cell);

                                phrase = new Phrase("Suspected Reason:", normalFont);
                                cell = new Cell(phrase);
                                cell.BorderWidth = 0;
                                tableReadmission.AddCell(cell);

                                readmissionReason = dsPatientEvents.Tables[0].Rows[Xh]["Reason"].ToString() == "" ? "" : getDescription(dsPatientEvents.Tables[0].Rows[Xh]["Reason"].ToString(), "reason");

                                phrase = new Phrase(readmissionReason, normalFont);
                                cell = new Cell(phrase);
                                cell.BorderWidth = 0;
                                tableReadmission.AddCell(cell);

                                phrase = new Phrase("\n", normalFont);
                                cell = new Cell(phrase);
                                cell.BorderWidth = 0;
                                cell.Colspan = 3;
                                tableReadmission.AddCell(cell);
                            }
                        }
                    }
                    #endregion
                }
                #endregion

                #region form 7
                else if (form == "7")
                {
                    formTitle = "Form 7: Reoperation/ Intervention Information";

                    if (periodNum == "")
                        periodNum = "0";

                    dsPatientEvents = LoadPatientEvents(period, Convert.ToInt32(periodNum));

                    if (period == "d")
                        period = "days";
                    else if (period == "m")
                        period = "months";
                    else if (period == "y")
                        period = "years";

                    #region var declaration Reoperation
                    //var declaration Reoperation
                    string reoperationTitle = "Surgical Procedure performed after the Index Bariatric procedure";
                    string reoperationReason = "";
                    string reoperationName = "";
                    string[] reoperations;
                    int reoperationNum = 0;
                    #endregion
                    #region Reoperation

                    phrase = new Phrase("\n" + "Period: " + periodNum.ToString() + " " + period, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 3;
                    tableReoperation.AddCell(cell);

                    phrase = new Phrase("\n" + reoperationTitle, H3);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 3;
                    tableReoperation.AddCell(cell);

                    if (dsPatientEvents.Tables.Count > 0 && (dsPatientEvents.Tables[0].Rows.Count > 0))
                    {
                        for (int Xh = 0; Xh < dsPatientEvents.Tables[0].Rows.Count; Xh++)
                        {
                            if (dsPatientEvents.Tables[0].Rows[Xh]["ReOperation"].ToString().Equals(Boolean.TrueString))
                            {
                                reoperationName = "";
                                if (dsPatientEvents.Tables[0].Rows[0]["AdverseSurgery"].ToString().Trim() != "")
                                {
                                    reoperations = dsPatientEvents.Tables[0].Rows[0]["AdverseSurgery"].ToString().Split(';');

                                    foreach (string tempReoperations in reoperations)
                                    {
                                        if (tempReoperations != "")
                                            reoperationName += getDescription(tempReoperations, "adevst") + ", ";
                                    }
                                    reoperationName = reoperationName.Substring(0, reoperationName.Length - 2);
                                }
                                reoperationNum++;

                                phrase = new Phrase("Reoperation #" + reoperationNum + ": ", normalFont);
                                cell = new Cell(phrase);
                                cell.BorderWidth = 0;
                                tableReoperation.AddCell(cell);

                                phrase = new Phrase("Date Performed:", normalFont);
                                cell = new Cell(phrase);
                                cell.BorderWidth = 0;
                                tableReoperation.AddCell(cell);

                                phrase = new Phrase(dsPatientEvents.Tables[0].Rows[Xh]["strPerformDate"].ToString(), normalFont);
                                cell = new Cell(phrase);
                                cell.BorderWidth = 0;
                                tableReoperation.AddCell(cell);

                                phrase = new Phrase("", normalFont);
                                cell = new Cell(phrase);
                                cell.BorderWidth = 0;
                                tableReoperation.AddCell(cell);

                                phrase = new Phrase("Suspected Reason:", normalFont);
                                cell = new Cell(phrase);
                                cell.BorderWidth = 0;
                                tableReoperation.AddCell(cell);

                                reoperationReason = dsPatientEvents.Tables[0].Rows[Xh]["Reason"].ToString() == "" ? "" : getDescription(dsPatientEvents.Tables[0].Rows[Xh]["Reason"].ToString(), "reason");

                                phrase = new Phrase(reoperationReason, normalFont);
                                cell = new Cell(phrase);
                                cell.BorderWidth = 0;
                                tableReoperation.AddCell(cell);

                                phrase = new Phrase("", normalFont);
                                cell = new Cell(phrase);
                                cell.BorderWidth = 0;
                                tableReoperation.AddCell(cell);

                                phrase = new Phrase("Reoperation:", normalFont);
                                cell = new Cell(phrase);
                                cell.BorderWidth = 0;
                                tableReoperation.AddCell(cell);

                                phrase = new Phrase(reoperationName, normalFont);
                                cell = new Cell(phrase);
                                cell.BorderWidth = 0;
                                tableReoperation.AddCell(cell);

                                phrase = new Phrase("\n", normalFont);
                                cell = new Cell(phrase);
                                cell.BorderWidth = 0;
                                cell.Colspan = 3;
                                tableReoperation.AddCell(cell);
                            }
                        }
                    }
                    #endregion
                }
                #endregion

                #region form 8
                else if (form == "8")
                {
                    formTitle = "Form 8: Attempts to Contact Patient";

                    #region var declaration Contact
                    //var declaration Contact
                    string contactTitle = "Attempts by the Bariatric Center to Contact Patient";
                    string contacts = "";
                    string contactName = "";
                    string contactChoice = "";
                    string contactNotes = "";

                    string[,] ContactArr = {{"FollowupAppointment","Was a follow-up appointment made but patient did not show for appointment?"},
                    {"FollowupPhone","Was a phone call placed to the patient?"}, {"FollowupLetterPatient","Was a letter sent to the patient?"}, 
                    {"FollowupLetterPhysician","Was a letter sent to the patient's primary physician?"}, 
                    {"FollowupTransfer","Was the patient's care transferred to another bariatric specialist?"}, 
                    {"FollowupRefuse","Is patient refusing long-term follow-up?"}, {"FollowupLost","Is the patient lost to follow-up?"}
                    };
                    #endregion

                    #region Contact
                    //Contact-----------------------------------------------------------------------------
                    phrase = new Phrase("\n" + contactTitle, H3);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableContact.AddCell(cell);

                    if (dsPatientFollowUp.Tables.Count > 0 && (dsPatientFollowUp.Tables[0].Rows.Count > 0))
                    {
                        for (int Idx = 0; Idx < ContactArr.Length / 2; Idx++)
                        {
                            contacts = ContactArr[Idx, 0];
                            contactName = ContactArr[Idx, 1];
                            contactNotes = "";

                            if (dsPatientFollowUp.Tables[0].Rows[0][contacts].ToString() != "")
                            {
                                contactChoice = getDescription(dsPatientFollowUp.Tables[0].Rows[0][contacts].ToString(), "contact");

                                if (contacts == "FollowupTransfer" && dsPatientFollowUp.Tables[0].Rows[0][contacts].ToString() == "Y")
                                {
                                    contactNotes = ", Name: " + dsPatientFollowUp.Tables[0].Rows[0]["FollowupTransferName"].ToString().Trim();
                                }

                                if (contactName != "")
                                {
                                    phrase = new Phrase("- " + contactName + ": " + contactChoice + contactNotes, normalFont);
                                    cell = new Cell(phrase);
                                    cell.BorderWidth = 0;
                                    tableContact.AddCell(cell);
                                }
                            }
                        }
                    }
                    #endregion
                }
                #endregion

                #region form 9
                else if (form == "9")
                {
                    formTitle = "Form 9: Mortality Information";

                    #region var declaration Postoperative Information
                    //var declaration Postoperative Information
                    string tempComplication = "";
                    #endregion
                    #region Mortality Information

                    if (dsPatientEvents.Tables.Count > 0 && (dsPatientEvents.Tables[0].Rows.Count > 0))
                    {
                        for (int Xh = 0; Xh < dsPatientEvents.Tables[0].Rows.Count; Xh++)
                        {
                            if (UppercaseFirst(dsPatientEvents.Tables[0].Rows[Xh]["Complication"].ToString()).IndexOf("Death") >= 0)
                            {
                                phrase = new Phrase("\n" + "Date of Death: " + dsPatientEvents.Tables[0].Rows[Xh]["strComplicationDate"].ToString(), normalFont);
                                cell = new Cell(phrase);
                                cell.BorderWidth = 0;
                                tableMortality.AddCell(cell);

                                if (dsPatientEvents.Tables[0].Rows[Xh]["Reason"].ToString() != "")
                                {
                                    phrase = new Phrase("Suspected Cause of Death: " + getDescription(dsPatientEvents.Tables[0].Rows[Xh]["Reason"].ToString(), "reason"), normalFont);
                                    cell = new Cell(phrase);
                                    cell.BorderWidth = 0;
                                    tableMortality.AddCell(cell);
                                }
                            }
                        }
                    }
                    #endregion
                }
                #endregion
                
                phrase = new Phrase(title1, normalBoldFont);
                cell = new Cell(phrase);
                cell.BorderWidth = 0;
                tableTitle.AddCell(cell);

                phrase = new Phrase(formTitle, H3);
                cell = new Cell(phrase);
                cell.BorderWidth = 0;
                cell.Rowspan = 2;
                cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_TOP;
                tableTitle.AddCell(cell);

                phrase = new Phrase(title4, normalBoldFont);
                cell = new Cell(phrase);
                cell.BorderWidth = 0;
                tableTitle.AddCell(cell);

                phrase = new Phrase(title2, normalBoldFont);
                cell = new Cell(phrase);
                cell.BorderWidth = 0;
                tableTitle.AddCell(cell);

                phrase = new Phrase(title5, normalBoldFont);
                cell = new Cell(phrase);
                cell.BorderWidth = 0;
                tableTitle.AddCell(cell);

                phrase = new Phrase(title3, normalBoldFont);
                cell = new Cell(phrase);
                cell.BorderWidth = 0;
                tableTitle.AddCell(cell);

                phrase = new Phrase("", H3);
                cell = new Cell(phrase);
                cell.BorderWidth = 0;
                tableTitle.AddCell(cell);

                phrase = new Phrase(title6 + demographicSurname, normalBoldFont);
                cell = new Cell(phrase);
                cell.BorderWidth = 0;
                tableTitle.AddCell(cell);

                phrase = new Phrase("", normalBoldFont);
                cell = new Cell(phrase);
                cell.BorderWidth = 0;
                tableTitle.AddCell(cell);

                phrase = new Phrase("", H3);
                cell = new Cell(phrase);
                cell.BorderWidth = 0;
                tableTitle.AddCell(cell);

                phrase = new Phrase(title7 + demographicFirstname, normalBoldFont);
                cell = new Cell(phrase);
                cell.BorderWidth = 0;
                tableTitle.AddCell(cell);

                oDoc.Add(tableTitle);
                
                if (form == "1")
                {
                    oDoc.Add(tableDemographic);
                    oDoc.Add(tableSurgical);
                    oDoc.Add(tablePreoperativeInformation);
                    oDoc.Add(tablePreoperativeLabs);
                    oDoc.Add(tableMedication);
                }
                else if (form == "2")
                    oDoc.Add(tableOperativeInformation);
                else if (form == "3")
                    oDoc.Add(tablePostOperativeInformation);
                else if (form == "4")
                    oDoc.Add(tableFollowUp);
                else if (form == "5")
                    oDoc.Add(tableFollowUp);
                else if (form == "6")
                    oDoc.Add(tableReadmission);
                else if (form == "7")
                    oDoc.Add(tableReoperation);
                else if (form == "8")
                    oDoc.Add(tableContact);
                else if (form == "9")
                    oDoc.Add(tableMortality);



                oDoc.Close();
                Response.Redirect(openFilePath);
            }
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Patient Data Form", "Loading Patient Data - LoadPatientData function", err.ToString());
        }
    }
    #endregion

    #region private void OREG1_BuildReport()
    private void OREG1_BuildReport()
    {
        try
        {
            string reportFormat = "";

            if (Request.QueryString["Format"] == "3")//pdf
                reportFormat = "pdf";

            string imperialMode = (Request.Cookies["Imperial"].Value == "True") ? "1" : "0";

            Phrase phrase = new Phrase("");
            DataSet dsPatientData = new DataSet();
            DataSet dsPatientEMR = new DataSet();
            DataSet dsPatientBold = new DataSet();
            DataSet dsPatientOperation = new DataSet();

            Decimal decTemp = 0;

            dsPatientData = LoadPatientData();
            dsPatientEMR = LoadPatientEMR();
            dsPatientBold = LoadPatientBold();
            dsPatientOperation = LoadPatientOperation();

            if (dsPatientData.Tables.Count > 0)
            {
                string patientHeightMeasurment = (imperialMode == "1" ? "inches" : "cm");
                string patientWeightMeasurment = (imperialMode == "1" ? "lbs" : "kg");

                string fileName = "OREG-" + Request.Cookies["PatientID"].Value + "-" + gClass.OrganizationCode;
                string saveFilePath = Server.MapPath(".\\.\\..\\temp\\") + fileName + "." + reportFormat;
                string openFilePath = "..//temp//" + fileName + "." + reportFormat;

                string titleIntro = "Operation Form";

                string pageFooter = "Thank you for completing this form";

                string demographicPatientID = "";
                string demographicSurname = dsPatientData.Tables[0].Rows[0]["Surname"].ToString();
                string demographicFirstname = dsPatientData.Tables[0].Rows[0]["Firstname"].ToString();
                string demographicGender = dsPatientData.Tables[0].Rows[0]["Sex"].ToString().Equals("M") ? "Male" : "Female";
                string demographicBirthDate = dsPatientData.Tables[0].Rows[0]["BirthDate"].ToString().Trim();
                string demographicStreet = dsPatientData.Tables[0].Rows[0]["Street"].ToString().Trim() + "  " + dsPatientData.Tables[0].Rows[0]["Suburb"].ToString().Trim();
                string demographicPostcode = dsPatientData.Tables[0].Rows[0]["SuburbValue"].ToString().Trim();
                string demographicHomePhone = dsPatientData.Tables[0].Rows[0]["HomePhone"].ToString().Trim();
                string demographicWorkPhone = dsPatientData.Tables[0].Rows[0]["WorkPhone"].ToString().Trim();
                string demographicMobilePhone = dsPatientData.Tables[0].Rows[0]["MobilePhone"].ToString().Trim();
                string demographicMedicareID = dsPatientEMR.Tables[0].Rows[0]["Details_MedicareNumber"].ToString().Trim();

                string operationHospital = dsPatientOperation.Tables[0].Rows[0]["HospitalName"].ToString().Trim();
                string operationRegion = dsPatientOperation.Tables[0].Rows[0]["RegionName"].ToString().Trim();
                string operationSurgeon = dsPatientOperation.Tables[0].Rows[0]["SurgeonName"].ToString().Trim();
                string operationWeight = dsPatientOperation.Tables[0].Rows[0]["OperationWeight"].ToString().Trim();

                string operationSurgeryTypeCode = dsPatientOperation.Tables[0].Rows[0]["SurgeryType"].ToString().Trim();
                string operationSurgeryType = dsPatientOperation.Tables[0].Rows[0]["SurgeryTypeRegistryDesc"].ToString().Trim();
                string operationApproach = dsPatientOperation.Tables[0].Rows[0]["ApproachDesc"].ToString().Trim();
                string operationBandType = "";
                string operationBandSize = "";
                string operationSerialNo = "";
                string operationOperationDate = dsPatientOperation.Tables[0].Rows[0]["strLapBandDate"].ToString().Trim();

                string baselineSurgeryType = dsPatientData.Tables[0].Rows[0]["SurgeryTypeRegistryDesc"].ToString().Trim();
                string baselineOperationDate = dsPatientData.Tables[0].Rows[0]["strLapBandDate"].ToString().Trim();
                string secondaryProcedureTitle = "";
                string secondaryProcedure = "";

                string registryProcedure = dsPatientOperation.Tables[0].Rows[0]["RegistryProcedureDesc"].ToString().Trim();

                switch (operationSurgeryTypeCode)
                {
                    case "BAA1060":
                        operationSerialNo = "    Serial No: " + dsPatientOperation.Tables[0].Rows[0]["SerialNo"].ToString().Trim();
                        break;

                    case "BAA1061":
                        operationSerialNo = "    Serial No: " + dsPatientOperation.Tables[0].Rows[0]["SerialNo"].ToString().Trim();
                        operationBandType = "    Band Type: " + dsPatientOperation.Tables[0].Rows[0]["BandTypeDesc"].ToString().Trim();
                        operationBandSize = "    Model / Band Size: " + dsPatientOperation.Tables[0].Rows[0]["BandSizeDesc"].ToString().Trim();
                        break;

                    case "ADDBST4":
                        operationSerialNo = "    Serial No: " + dsPatientOperation.Tables[0].Rows[0]["SerialNo"].ToString().Trim();
                        operationBandType = "    Band Type: " + dsPatientOperation.Tables[0].Rows[0]["BandTypeDesc"].ToString().Trim();
                        operationBandSize = "    Model / Band Size: " + dsPatientOperation.Tables[0].Rows[0]["BandSizeDesc"].ToString().Trim();
                        break;
                }

                if (baselineSurgeryType == operationSurgeryType && baselineOperationDate == operationOperationDate)
                {
                    secondaryProcedureTitle = "";
                    secondaryProcedure = "";
                }
                else
                {
                    secondaryProcedureTitle = "Secondary Procedure";
                    secondaryProcedure = operationSurgeryType;
                }

                string baselineHeight = dsPatientData.Tables[0].Rows[0]["Height"].ToString();
                string baselineWeight = dsPatientData.Tables[0].Rows[0]["StartWeight"].ToString();

                Int32 totalPatientData = Convert.ToInt32(dsPatientData.Tables[0].Rows.Count.ToString());
                string registryDiabetes = dsPatientData.Tables[0].Rows[totalPatientData - 1]["RegistryDiabetesDetail"].ToString().Equals("True") ? "Yes" : "No";
                string registryTreatment = "";

                if (dsPatientData.Tables[0].Rows[totalPatientData - 1]["RegistryDiabetesDetail"].ToString().Equals("True"))
                {
                    registryTreatment = "    Treatment: ";
                    registryTreatment += dsPatientData.Tables[0].Rows[totalPatientData - 1]["RegistryTreatmentOral"].ToString().Equals("True") ? "Oral(tablets), " : "";
                    registryTreatment += dsPatientData.Tables[0].Rows[totalPatientData - 1]["RegistryTreatmentInsulin"].ToString().Equals("True") ? "Insulin, " : "";
                    registryTreatment += dsPatientData.Tables[0].Rows[totalPatientData - 1]["RegistryTreatmentCombination"].ToString().Equals("True") ? "Combination (of pharmacoptherapies), " : "";
                    registryTreatment += dsPatientData.Tables[0].Rows[totalPatientData - 1]["RegistryTreatmentDiet"].ToString().Equals("True") ? "Diet & Exercise, " : "";
                    registryTreatment += dsPatientData.Tables[0].Rows[totalPatientData - 1]["RegistryTreatmentOther"].ToString().Equals("True") ? "Other(e.g. Pump), " : "";
                    registryTreatment = registryTreatment.Substring(0, registryTreatment.Length - 2);
                }




                string baselineFirstVisitWeight = dsPatientData.Tables[0].Rows[totalPatientData-1]["Weight"].ToString();

                if (imperialMode.Equals("1"))
                {
                    baselineHeight = Decimal.TryParse(baselineHeight, out decTemp) ? (decTemp / (Decimal)0.0254).ToString(strNumberFormat) : "0";
                    baselineWeight = Decimal.TryParse(baselineFirstVisitWeight, out decTemp) ? (decTemp / (Decimal)0.45359237).ToString(strNumberFormat) : "0";
                }
                else
                {
                    baselineHeight = Decimal.TryParse(baselineHeight, out decTemp) ? (decTemp * 100).ToString(strNumberFormat) : "0";
                    baselineWeight = Decimal.TryParse(baselineFirstVisitWeight, out decTemp) ? decTemp.ToString(strNumberFormat) : "0";
                }



                if (reportFormat == "pdf")
                {
                    #region declare table and width
                    iTextSharp.text.Document oDoc = new iTextSharp.text.Document(PageSize.LETTER, 0, 0, 30f, 30f);
                    PdfWriter.GetInstance(oDoc, new FileStream(saveFilePath, FileMode.Create));

                    Cell cell = new Cell("");
                    
                    Font H2 = new Font(Font.HELVETICA, 13, Font.BOLD);
                    Font H3 = new Font(Font.HELVETICA, 11, Font.BOLD);
                    Font smallFont = new Font(Font.HELVETICA, 6, 0);
                    Font smallItalicFont = new Font(Font.HELVETICA, 8, Font.ITALIC);
                    Font normalFont = new Font(Font.HELVETICA, 10, 0);
                    Font normalBoldFont = new Font(Font.HELVETICA, 10, Font.BOLD);

                    Phrase footPhrase = new Phrase(pageFooter, smallItalicFont);
                    HeaderFooter footer = new HeaderFooter(footPhrase, false);
                    footer.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    footer.Alignment = iTextSharp.text.Element.ALIGN_BOTTOM;
                    footer.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    oDoc.Footer = footer;

                    oDoc.Open();

                    iTextSharp.text.Table tableTitle = new iTextSharp.text.Table(2, 3);
                    iTextSharp.text.Table tableDemographic = new iTextSharp.text.Table(2, 30);
                    iTextSharp.text.pdf.draw.LineSeparator line = new iTextSharp.text.pdf.draw.LineSeparator(1f, 95f, new Color(255, 105, 180), iTextSharp.text.Element.ALIGN_CENTER, 1);
                    

                    float[] titleWidths = { .35f, .65f };
                    tableTitle.Spacing = 1;
                    tableTitle.Widths = titleWidths;
                    tableTitle.DefaultCell.BorderWidth = 0;
                    tableTitle.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_TOP;
                    tableTitle.DefaultCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    tableTitle.TableFitsPage = true;
                    tableTitle.BorderColor = new Color(255, 255, 255);
                
                    float[] demographicWidths = { .5f, .5f};
                    tableDemographic.Spacing = 1;
                    tableDemographic.Widths = demographicWidths;
                    tableDemographic.DefaultCell.BorderWidth = 0;
                    tableDemographic.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    tableDemographic.TableFitsPage = true;
                    tableDemographic.BorderColor = new Color(255, 255, 255);

                    #endregion

                    //Title-----------------------------------------------------------------------------------------------
                    phrase = new Phrase("Patient Information", normalBoldFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableTitle.AddCell(cell);

                    phrase = new Phrase("BARIATRIC SURGERY REGISTRY", H2);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Rowspan = 2;
                    tableTitle.AddCell(cell);

                    phrase = new Phrase("Operation Form", normalBoldFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableTitle.AddCell(cell);

                    phrase = new Phrase("\n", normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    tableTitle.AddCell(cell);

                    oDoc.Add(tableTitle);

                    oDoc.Add(new Chunk(line));

                    phrase = new Phrase("\n", normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("Patient Detail", H3);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("Procedure", H3);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("    Surname: "+demographicSurname, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("    " + registryProcedure, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("    Given Name: " + demographicFirstname, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("    Gender: " + demographicGender, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("    Primary Procedure", normalBoldFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("    DOB: " + demographicBirthDate, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("        " + baselineSurgeryType, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("    Hospital MR No: " + demographicPatientID, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("    " + secondaryProcedureTitle, normalBoldFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("    Medicare ID: " + demographicMedicareID, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("        " + secondaryProcedure, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("\n", normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("Contact Detail", H3);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("Procedure Detail", H3);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("    Home Phone: " + demographicHomePhone, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("    Operation Date: " + operationOperationDate, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("    Mobile Phone: " + demographicMobilePhone, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("    Operation Weight: " + operationWeight + " " + patientWeightMeasurment, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("    Address: " + demographicStreet, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("    Surgeon: " + operationSurgeon, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("    Postcode: " + demographicPostcode, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("    Hospital: " + operationHospital, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("", normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("    State: " + operationRegion, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("Measurement", H3);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("    Approach: " + operationApproach, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("    Height: " + baselineHeight + " " + patientHeightMeasurment, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase(operationBandType, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("    Pre-Op Weight: " + baselineWeight + " " + patientWeightMeasurment, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase(operationBandSize, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("", normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase(operationSerialNo, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("    Diabetes: " + registryDiabetes, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase(registryTreatment, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    tableDemographic.AddCell(cell);

                    oDoc.Add(tableDemographic);
                    oDoc.Close();
                    Response.Redirect(openFilePath);
                }
            }
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Patient Data Form", "Loading Patient Data - LoadPatientData function", err.ToString());
        }
    }
    #endregion

    #region private void OREG2_BuildReport()
    private void OREG2_BuildReport()
    {
        try
        {
            string reportFormat = "";
            Int32 xAnnual = 1;
            Int32 additionalMonth = 0;

            if (Request.QueryString["Format"] == "3")//pdf
                reportFormat = "pdf";

            xAnnual = Convert.ToInt32(Request.QueryString["Annual"]);

            if (xAnnual > 1)
                additionalMonth = 12 * (xAnnual -1);

            string imperialMode = (Request.Cookies["Imperial"].Value == "True") ? "1" : "0";

            Phrase phrase = new Phrase("");
            DataSet dsPatientData = new DataSet();
            DataSet dsPatientEMR = new DataSet();
            DataSet dsPatientBold = new DataSet();
            DataSet dsPatientOperation = new DataSet();
            DataSet dsLastVisitDays = new DataSet();
            DataSet dsLastVisitYear = new DataSet();
            DataSet dsCompLastVisitDays = new DataSet();
            DataSet dsCompLastVisitYear = new DataSet();

            Decimal decTemp = 0;

            dsPatientData = LoadPatientData();
            dsPatientEMR = LoadPatientEMR();
            dsPatientBold = LoadPatientBold();
            dsPatientOperation = LoadPatientOperation();
            dsLastVisitDays = LoadLastVisit("seldate", "m", 1, 2, dsPatientOperation.Tables[0].Rows[0]["shortOperationDate"].ToString().Trim());
            dsLastVisitYear = LoadLastVisit("seldate", "m", (11 + additionalMonth), (14 + additionalMonth), dsPatientOperation.Tables[0].Rows[0]["shortOperationDate"].ToString().Trim());
            dsCompLastVisitDays = LoadLastComplication("seldate", "m", 1, 2, dsPatientOperation.Tables[0].Rows[0]["shortOperationDate"].ToString().Trim());
            dsCompLastVisitYear = LoadLastComplication("seldate", "m", (11 + additionalMonth), (14 + additionalMonth), dsPatientOperation.Tables[0].Rows[0]["shortOperationDate"].ToString().Trim());

            if (dsPatientData.Tables.Count > 0)
            {
                string patientHeightMeasurment = (imperialMode == "1" ? "inches" : "cm");
                string patientWeightMeasurment = (imperialMode == "1" ? "lbs" : "kg");

                string fileName = "OREG2-" + Request.Cookies["PatientID"].Value + "-" + gClass.OrganizationCode;
                string saveFilePath = Server.MapPath(".\\.\\..\\temp\\") + fileName + "." + reportFormat;
                string openFilePath = "..//temp//" + fileName + "." + reportFormat;

                string titleIntro = "Operation Followup";

                string pageFooter = "Thank you for completing this form";

                string demographicPatientID = dsPatientData.Tables[0].Rows[0]["LapbasePatientID"].ToString();
                string demographicSurname = dsPatientData.Tables[0].Rows[0]["Surname"].ToString();
                string demographicFirstname = dsPatientData.Tables[0].Rows[0]["Firstname"].ToString();
                string demographicGender = dsPatientData.Tables[0].Rows[0]["Sex"].ToString().Equals("M") ? "Male" : "Female";
                string demographicBirthDate = dsPatientData.Tables[0].Rows[0]["BirthDate"].ToString().Trim();
                string demographicDeceasedDate = dsPatientEMR.Tables[0].Rows[0]["strDeceasedDate"].ToString().Trim();
                string demographicDeceasedPrimaryProcedure = dsPatientEMR.Tables[0].Rows[0]["Details_DeceasedPrimaryProcedure"].ToString().Trim();
                string demographicDeceasedNote = dsPatientEMR.Tables[0].Rows[0]["Details_DeceasedNote"].ToString().Trim();

                if (demographicDeceasedPrimaryProcedure == "True")
                    demographicDeceasedPrimaryProcedure = "Death is related to bariatric procedure";
                else
                    demographicDeceasedPrimaryProcedure = "Death is not related to bariatric procedure";


                string operationWeight = dsPatientOperation.Tables[0].Rows[0]["OperationWeight"].ToString().Trim();
                string operationOperationDate = dsPatientOperation.Tables[0].Rows[0]["strLapBandDate"].ToString().Trim();

                string baselineOperationDate = dsPatientData.Tables[0].Rows[0]["strLapBandDate"].ToString().Trim();

                string compLastVisitDays = "";
                string compLastVisitYear = "";
                string lastVisitDaysWeight = "";
                string lastVisitYearDate = "";
                string lastVisitYearWeight = "";
                string lastVisitYearDiabetes = "";
                string lastVisitYearDiabetesTreatment = "";
                string lastVisitYearReoperation = "";
                string lastVisitYearReoperationNote = "";

                string lastVisitDaysDate = "";
                string lastVisitDaysSentinelEventDetail = "";
                string lastVisitDaysSentinelEventList = "";
                string lastVisitDaysSentinelEventNote = "";

                if (dsCompLastVisitDays.Tables.Count >0)
                {
                    if (dsCompLastVisitDays.Tables[0].Rows.Count > 0)
                    {
                        for (int Xh = 0; Xh < dsCompLastVisitDays.Tables[0].Rows.Count; Xh++)
                        {
                            compLastVisitDays += "    -" + dsCompLastVisitDays.Tables[0].Rows[Xh]["strDateSeen"] + ": " + dsCompLastVisitDays.Tables[0].Rows[Xh]["Complication"] + "\n";
                        }
                    }
                }

                if (dsCompLastVisitYear.Tables.Count > 0)
                {
                    if (dsCompLastVisitYear.Tables[0].Rows.Count > 0)
                    {
                        for (int Xh = 0; Xh < dsCompLastVisitYear.Tables[0].Rows.Count; Xh++)
                        {
                            compLastVisitYear += "    - " + dsCompLastVisitYear.Tables[0].Rows[Xh]["strDateSeen"] + ": " + dsCompLastVisitYear.Tables[0].Rows[Xh]["Complication"] + "\n";
                        }
                    }
                }
                
                if (dsLastVisitDays.Tables.Count > 0)
                {
                    if (dsLastVisitDays.Tables[0].Rows.Count > 0)
                    {
                        lastVisitDaysDate = dsLastVisitDays.Tables[0].Rows[0]["strDateSeen"].ToString().Trim();
                        lastVisitDaysWeight = dsLastVisitDays.Tables[0].Rows[0]["Weight"].ToString().Trim();

                        lastVisitDaysSentinelEventDetail = dsLastVisitDays.Tables[0].Rows[0]["RegistrySEDetail"].ToString().Trim();
                        if (lastVisitDaysSentinelEventDetail == "True")
                        {
                            lastVisitDaysSentinelEventList = dsLastVisitDays.Tables[0].Rows[0]["RegistrySEListDesc"].ToString().Trim();
                            lastVisitDaysSentinelEventNote = dsLastVisitDays.Tables[0].Rows[0]["RegistrySENote"].ToString().Trim();
                        }
                    }
                }

                if (dsLastVisitYear.Tables.Count > 0)
                {
                    if (dsLastVisitYear.Tables[0].Rows.Count > 0)
                    {
                        lastVisitYearDate = dsLastVisitYear.Tables[0].Rows[0]["strDateSeen"].ToString().Trim();
                        lastVisitYearWeight = dsLastVisitYear.Tables[0].Rows[0]["Weight"].ToString().Trim();

                        lastVisitYearDiabetes = dsLastVisitYear.Tables[0].Rows[0]["RegistryDiabetesDetail"].ToString().Equals("True") ? "Yes" : "No";
                        if (lastVisitYearDiabetes == "Yes")
                        {
                            lastVisitYearDiabetesTreatment += dsLastVisitYear.Tables[0].Rows[0]["RegistryTreatmentOral"].ToString().Equals("True") ? "    Oral(tablets)\n" : "";
                            lastVisitYearDiabetesTreatment += dsLastVisitYear.Tables[0].Rows[0]["RegistryTreatmentInsulin"].ToString().Equals("True") ? "    Insulin, " : "";
                            lastVisitYearDiabetesTreatment += dsLastVisitYear.Tables[0].Rows[0]["RegistryTreatmentCombination"].ToString().Equals("True") ? "    Combination (of pharmacoptherapies)\n" : "";
                            lastVisitYearDiabetesTreatment += dsLastVisitYear.Tables[0].Rows[0]["RegistryTreatmentDiet"].ToString().Equals("True") ? "    Diet & Exercise\n" : "";
                            lastVisitYearDiabetesTreatment += dsLastVisitYear.Tables[0].Rows[0]["RegistryTreatmentOther"].ToString().Equals("True") ? "    Other(e.g. Pump)\n" : "";
                            lastVisitYearDiabetesTreatment = lastVisitYearDiabetesTreatment.Substring(0, lastVisitYearDiabetesTreatment.Length - 2);

                            if (lastVisitYearDiabetesTreatment != "")
                            {
                                lastVisitYearDiabetesTreatment = "Treatment:\n" + lastVisitYearDiabetesTreatment;
                            }
                        }

                        lastVisitYearReoperation = dsLastVisitYear.Tables[0].Rows[0]["RegistryReoperation"].ToString().Trim();
                        lastVisitYearReoperationNote = dsLastVisitYear.Tables[0].Rows[0]["RegistryReoperationNote"].ToString().Trim();
                    }
                }

                if (imperialMode.Equals("1"))
                {
                    operationWeight = Decimal.TryParse(operationWeight, out decTemp) ? (decTemp / (Decimal)0.45359237).ToString(strNumberFormat) : "0";
                    lastVisitDaysWeight = Decimal.TryParse(lastVisitDaysWeight, out decTemp) ? (decTemp / (Decimal)0.45359237).ToString(strNumberFormat) : "0";
                    lastVisitYearWeight = Decimal.TryParse(lastVisitYearWeight, out decTemp) ? (decTemp / (Decimal)0.45359237).ToString(strNumberFormat) : "0";
                }
                else
                {
                    operationWeight = Decimal.TryParse(operationWeight, out decTemp) ? decTemp.ToString(strNumberFormat) : "0";
                    lastVisitDaysWeight = Decimal.TryParse(lastVisitDaysWeight, out decTemp) ? decTemp.ToString(strNumberFormat) : "0";
                    lastVisitYearWeight = Decimal.TryParse(lastVisitYearWeight, out decTemp) ? decTemp.ToString(strNumberFormat) : "0";
                }

                if (reportFormat == "pdf")
                {
                    #region declare table and width
                    iTextSharp.text.Document oDoc = new iTextSharp.text.Document(PageSize.LETTER, 0, 0, 30f, 30f);
                    PdfWriter.GetInstance(oDoc, new FileStream(saveFilePath, FileMode.Create));

                    Cell cell = new Cell("");
                    Font H2 = new Font(Font.HELVETICA, 13, Font.BOLD);
                    Font H3 = new Font(Font.HELVETICA, 11, Font.BOLD);
                    Font smallFont = new Font(Font.HELVETICA, 6, 0);
                    Font smallItalicFont = new Font(Font.HELVETICA, 8, Font.ITALIC);
                    Font normalFont = new Font(Font.HELVETICA, 10, 0);
                    Font normalBoldFont = new Font(Font.HELVETICA, 10, Font.BOLD);

                    Phrase footPhrase = new Phrase(pageFooter, smallItalicFont);
                    HeaderFooter footer = new HeaderFooter(footPhrase, false);
                    footer.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    footer.Alignment = iTextSharp.text.Element.ALIGN_BOTTOM;
                    footer.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    oDoc.Footer = footer;

                    oDoc.Open();

                    iTextSharp.text.Table tableTitle = new iTextSharp.text.Table(2, 3);
                    iTextSharp.text.Table tableDemographic = new iTextSharp.text.Table(2, 30);
                    iTextSharp.text.pdf.draw.LineSeparator line = new iTextSharp.text.pdf.draw.LineSeparator(1f, 95f, new Color(64, 224, 208), iTextSharp.text.Element.ALIGN_CENTER, 1);
                    
                    float[] titleWidths = { .35f, .65f };
                    tableTitle.Spacing = 1;
                    tableTitle.Widths = titleWidths;
                    tableTitle.DefaultCell.BorderWidth = 0;
                    tableTitle.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_TOP;
                    tableTitle.DefaultCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    tableTitle.TableFitsPage = true;
                    tableTitle.BorderColor = new Color(255, 255, 255);

                    float[] demographicWidths = { .5f, .5f };
                    tableDemographic.Spacing = 1;
                    tableDemographic.Widths = demographicWidths;
                    tableDemographic.DefaultCell.BorderWidth = 0;
                    tableDemographic.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    tableDemographic.TableFitsPage = true;
                    tableDemographic.BorderColor = new Color(255, 255, 255);

                    #endregion

                    //Title-----------------------------------------------------------------------------------------------
                    phrase = new Phrase("Follow-up Form", normalBoldFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableTitle.AddCell(cell);

                    phrase = new Phrase("BARIATRIC SURGERY REGISTRY", H2);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Rowspan = 2;
                    tableTitle.AddCell(cell);

                    phrase = new Phrase("Adverse Event Form", normalBoldFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableTitle.AddCell(cell);

                    phrase = new Phrase("\n", normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    tableTitle.AddCell(cell);

                    oDoc.Add(tableTitle);

                    oDoc.Add(new Chunk(line));

                    phrase = new Phrase("\n", normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("Patient Detail", H3);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("Operation Detail", H3);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("    Surname: " + demographicSurname, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("    Operation Date: " + operationOperationDate, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("    Given Name: " + demographicFirstname, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("    Operation Weight: " + operationWeight + " " + patientWeightMeasurment, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("    Gender: " + demographicGender, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("    DOB: " + demographicBirthDate, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("    Baseline Operation Date: " + baselineOperationDate, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("\n\n", normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("30 Day Follow-Up", H3);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("Annual Follow-Up, Year " + xAnnual, H3);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("    Date of Follow-Up: " + lastVisitDaysDate, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("    Date of Follow-Up: " + lastVisitYearDate, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("    Patient Weight: " + (lastVisitDaysWeight != "0" ? lastVisitDaysWeight + " " + patientWeightMeasurment : ""), normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("    Patient Weight: " + (lastVisitYearWeight != "0" ? lastVisitYearWeight + " " + patientWeightMeasurment : ""), normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    //sentinel event
                    phrase = new Phrase("    Sentinel Event", normalBoldFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);
                    
                    //diabetes
                    phrase = new Phrase("    Diabetes", normalBoldFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    //sentinel event
                    phrase = new Phrase("    - " + lastVisitDaysSentinelEventList, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    //diabetes
                    phrase = new Phrase("    - " + lastVisitYearDiabetes, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    //sentinel event
                    phrase = new Phrase("    " + lastVisitDaysSentinelEventNote, normalFont);
                    cell = new Cell(phrase);
                    cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_TOP;
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    //diabetes
                    phrase = new Phrase("    " + lastVisitYearDiabetesTreatment, normalFont);
                    cell = new Cell(phrase);
                    cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_TOP;
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    //break line
                    phrase = new Phrase(" ", normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase(" ", normalBoldFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    //reoperation
                    phrase = new Phrase("    Re-operation", normalBoldFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase(" ", normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    //reoperation
                    phrase = new Phrase("    - " + lastVisitYearReoperation, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase(" ", normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    //reoperation
                    phrase = new Phrase("    " + lastVisitYearReoperationNote, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("\n", normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("    Complications", normalBoldFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("    Complications", normalBoldFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase(compLastVisitDays, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase(compLastVisitYear, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("\n\n", normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase("    Mortality", normalBoldFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    tableDemographic.AddCell(cell);

                    if (demographicDeceasedDate != "")
                    {
                        phrase = new Phrase("    Deceased Date: " + demographicDeceasedDate, normalFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        cell.Colspan = 2;
                        tableDemographic.AddCell(cell);

                        phrase = new Phrase("    " + demographicDeceasedPrimaryProcedure, normalFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        cell.Colspan = 2;
                        tableDemographic.AddCell(cell);

                        phrase = new Phrase("    " + demographicDeceasedNote, normalFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        cell.Colspan = 2;
                        tableDemographic.AddCell(cell);
                    }
                    else
                    {
                        phrase = new Phrase("    No mortality occured", normalFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        cell.Colspan = 2;
                        tableDemographic.AddCell(cell);

                        phrase = new Phrase("", normalFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        cell.Colspan = 2;
                        cell.Rowspan = 2;
                        tableDemographic.AddCell(cell);
                    }

                    oDoc.Add(tableDemographic);
                    oDoc.Close();
                    Response.Redirect(openFilePath);
                }
            }
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Patient Data Form", "Loading Patient Data - LoadPatientData function", err.ToString());
        }
    }
    #endregion

    #region private void EMR_BuildReport()
    private void EMR_BuildReport()
    {
        string reportFormat = "";

        if (Request.QueryString["Format"] == "3")//pdf
            reportFormat = "pdf";
        else if (Request.QueryString["Format"] == "4")//word document
            reportFormat = "doc";

        string strDoc = "";

        string imperialMode = (Request.Cookies["Imperial"].Value == "True") ? "1" : "0";

        Phrase phrase = new Phrase("");
        DataSet dsPatientData = new DataSet();
        DataSet dsPatientEMR = new DataSet();
        DataSet dsPatientComorbidity = new DataSet();
        DataSet dsPatientBold = new DataSet();
        DataSet dsSystemDetail = new DataSet();
        DataSet dsLogo = new DataSet();
        string LogoPath = "";

        try
        {
            dsPatientData = LoadPatientData();
            dsPatientEMR = LoadPatientEMR();
            dsPatientComorbidity = LoadPatientComorbidity();
            dsPatientBold = LoadPatientBold();
            dsSystemDetail = LoadSystemDetail();

            if (dsPatientData.Tables.Count > 0)
            {
                string patientHeightMeasurment = (imperialMode == "1" ? "inches" : "cm");
                string patientWeightMeasurment = (imperialMode == "1" ? "lbs" : "kg");

                string fileName = Request.Cookies["PatientID"].Value + "-" + gClass.OrganizationCode;
                string saveFilePath = Server.MapPath(".\\.\\..\\temp\\") + fileName + "." + reportFormat;
                string openFilePath = "..//temp//" + fileName + "." + reportFormat;
                
                string titleIntro = "Initial Medical Assessment";
                
                #region var declaration Logo
                //var declaration Logo
                if (Request.QueryString["Logo"] != "")
                {
                    dsLogo = LoadLogoByID(Convert.ToInt32(Request.QueryString["Logo"]));
                    LogoPath = dsLogo.Tables[0].Rows[0]["LogoPath"].ToString();
                    LogoPath = LogoPath.Substring(IndexOfOccurence(LogoPath, "/", 2));

                }
                #endregion

                #region var declaration System Detail
                //var declaration System Detail
                string facilityName = dsSystemDetail.Tables[0].Rows[0]["FacilityName"].ToString().Trim();
                string facilityAddress = dsSystemDetail.Tables[0].Rows[0]["Street"].ToString().Trim();
                string facilitySuburb = dsSystemDetail.Tables[0].Rows[0]["Suburb"].ToString().Trim();
                string facilityState = dsSystemDetail.Tables[0].Rows[0]["State"].ToString().Trim();
                string facilityPostCode = dsSystemDetail.Tables[0].Rows[0]["Postcode"].ToString().Trim();
                string facilityPhone = dsSystemDetail.Tables[0].Rows[0]["Phone"].ToString().Trim();
                string facilityFax = dsSystemDetail.Tables[0].Rows[0]["Fax"].ToString().Trim();
                
                string facilityAddress2 = facilitySuburb;
                facilityAddress2 += facilityState != "" ? ", " + facilityState : "";
                facilityAddress2 += facilityPostCode != "" ? " " + facilityPostCode : "";

                string facilityContact = facilityPhone != "" ? "Phone: " + facilityPhone + "   ": "";
                facilityContact += facilityFax != "" ? "Fax: " + facilityFax : "";

                Int32 margin = 0;
                try { margin = Convert.ToInt32(dsSystemDetail.Tables[0].Rows[0]["LetterheadMargin"].ToString().Trim()); }
                catch { margin = 0; }

                #endregion

                #region var declaration Demographic
                //var declaration Demographic
                string demographicSurname = dsPatientData.Tables[0].Rows[0]["Surname"].ToString();
                string demographicFirstname = dsPatientData.Tables[0].Rows[0]["Firstname"].ToString();
                string demographicNameTitle = dsPatientData.Tables[0].Rows[0]["Title"].ToString();
                string demographicAge = dsPatientData.Tables[0].Rows[0]["Age"].ToString();
                string demographicCustomId = dsPatientData.Tables[0].Rows[0]["Patient_CustomID"].ToString();

                string demographicBirthDate = dsPatientData.Tables[0].Rows[0]["BirthDate"].ToString().Trim();
                string demographicConsultationDate = dsPatientData.Tables[0].Rows[0]["ConsultationDate"].ToString().Trim();
                string demographicDoctorName = dsPatientData.Tables[0].Rows[0]["DoctorName"].ToString().Trim();
                string demographicStatus = dsPatientEMR.Tables[0].Rows[0]["FamilyStructure"].ToString().Trim();
                string demographicChildren = dsPatientEMR.Tables[0].Rows[0]["Details_Children"].ToString().Trim();
                int demographicChildrenNumber = demographicChildren != "" ?demographicChildren.Split('+').Length : 0;

                string child = "";
                if (demographicChildrenNumber == 1)
                    child = "with " + demographicChildrenNumber + " child";
                else if (demographicChildrenNumber > 1)
                    child = "with " + demographicChildrenNumber + " children";
                string demographicStatusComplete = demographicStatus + " " + child;

                string demographicGender = dsPatientData.Tables[0].Rows[0]["Sex"].ToString().Equals("M") ? "He" : "She";
                string demographicGender2 = dsPatientData.Tables[0].Rows[0]["Sex"].ToString().Equals("M") ? "his" : "her";
                string demographicGender3 = dsPatientData.Tables[0].Rows[0]["Sex"].ToString().Equals("M") ? "man" : "woman";
                string demographicGender4 = dsPatientData.Tables[0].Rows[0]["Sex"].ToString().Equals("M") ? "male" : "female";

                

                if (demographicNameTitle == "1")
                    demographicNameTitle = "Mr.";
                else if (demographicNameTitle == "2")
                    demographicNameTitle = "Mrs.";
                else if (demographicNameTitle == "3")
                    demographicNameTitle = "Miss";
                else if (demographicNameTitle == "4")
                    demographicNameTitle = "Dr.";
                else if (demographicNameTitle == "5")
                    demographicNameTitle = "Ms.";
                else if (demographicNameTitle == "6")
                    demographicNameTitle = "Mstr";
                else
                    demographicNameTitle = "";

                string demographicPatientIntro = "Patient Name: ";
                string demographicPatientName = demographicNameTitle + " " + demographicFirstname + " " + demographicSurname;
                string demographicPatientNameFooter = demographicSurname + ", " + demographicFirstname + ", " + demographicCustomId;
                string demographicDOBIntro = "Date of Birth: ";
                string demographicConsultIntro = "Consultation Date: ";
                string demographicStatusIntro = "Status: ";

                string demographicConsutlationComplete = demographicConsultationDate != "" ? demographicConsultIntro + demographicConsultationDate : "";

                string demographicMedicalSummary = dsPatientData.Tables[0].Rows[0]["MedicalSummary"].ToString();

                #endregion
                
                #region var declaration Presenting Complaint
                //var declaration Presenting Complaint
                string complaintTitle = "Presenting Complaint: ";
                string complaint = dsPatientEMR.Tables[0].Rows[0]["Complaint_Desc"].ToString();
                string complaintNotes = dsPatientEMR.Tables[0].Rows[0]["complaintNotes"].ToString();
                string complaintIntro = demographicNameTitle + " " + demographicFirstname + " " + demographicSurname + " is a " + demographicAge + " years old " + demographicGender4 + " patient.";
                #endregion

                #region var declaration Measurement
                //var declaration Measurement
                string measurementTitle = "Measurement:";

                Decimal decTemp = 0m;

                string measurementBMIHeight = dsPatientData.Tables[0].Rows[0]["BMIHeight"].ToString().Equals("") ? "0" : dsPatientData.Tables[0].Rows[0]["BMIHeight"].ToString();
                string measurementBMI = dsPatientData.Tables[0].Rows[0]["InitBMI"].ToString().Equals("") ? "0" : (Decimal.TryParse(dsPatientData.Tables[0].Rows[0]["InitBMI"].ToString(), out decTemp) ? decTemp.ToString(strNumberFormat) : "0");
                string measurementHeight = dsPatientData.Tables[0].Rows[0]["Height"].ToString();
                string measurementWeight = dsPatientData.Tables[0].Rows[0]["StartWeight"].ToString();
                string measurementEWeight = dsPatientData.Tables[0].Rows[0]["ExcessWeight"].ToString();
                string measurementIWeight = dsPatientData.Tables[0].Rows[0]["IdealWeight"].ToString();
                string measurementRefBMI = gClass.SD_ReferenceBMI.ToString();

                string measurementNeck = dsPatientData.Tables[0].Rows[0]["StartNeck"].ToString();
                string measurementWaist = dsPatientData.Tables[0].Rows[0]["StartWaist"].ToString();
                string measurementHip = dsPatientData.Tables[0].Rows[0]["StartHip"].ToString();

                string measurementPR = dsPatientData.Tables[0].Rows[0]["StartPR"].ToString();
                string measurementRR = dsPatientData.Tables[0].Rows[0]["StartRR"].ToString();
                string measurementBPLower = dsPatientData.Tables[0].Rows[0]["StartBP1"].ToString();
                string measurementBPUpper = dsPatientData.Tables[0].Rows[0]["StartBP2"].ToString();

                string weightHistoryGainWeight = dsPatientEMR.Tables[0].Rows[0]["WeightHistory_GainWeight"].ToString();

                string measurementHeightIntro = "Height: ";
                string measurementNeckIntro = "Neck: ";
                string measurementPRIntro = "PR: ";
                string measurementWeightIntro = "Weight: ";
                string measurementWaistIntro = "Waist: ";
                string measurementRRIntro = "RR: ";

                string measurementBMIIntro = "BMI: ";
                string measurementHipIntro = "Hip: ";
                string measurementBPIntro = "BP: ";

                string measurementIWeightIntro = "Ideal Weight: ";
                string measurementWHRIntro = "WHR: ";
                string measurementEWeightIntro = "Excess Weight: ";

                if (Decimal.TryParse(measurementIWeight, out decTemp))
                {
                    if (decTemp == 0)
                    {
                        Int16 intRefBMI;

                        Int16.TryParse(measurementRefBMI, out intRefBMI);

                        if (Decimal.TryParse(measurementBMIHeight, out decTemp))
                            measurementIWeight = (intRefBMI * Math.Pow((double)decTemp, (double)2)).ToString();
                        else
                            measurementIWeight = "0";
                    }
                }
                else measurementIWeight = "0";

                if (imperialMode.Equals("1"))
                {
                    measurementHeight = Decimal.TryParse(measurementHeight, out decTemp) ? (decTemp / (Decimal)0.0254).ToString(strNumberFormat) : "0";
                    measurementWeight = Decimal.TryParse(measurementWeight, out decTemp) ? (decTemp / (Decimal)0.45359237).ToString(strNumberFormat) : "0";
                    measurementIWeight = Decimal.TryParse(measurementIWeight, out decTemp) ? (decTemp / (Decimal)0.45359237).ToString(strNumberFormat) : "0";
                    measurementEWeight = Decimal.TryParse(measurementEWeight, out decTemp) ? (decTemp / (Decimal)0.45359237).ToString(strNumberFormat) : "0";

                    measurementNeck = Decimal.TryParse(measurementNeck, out decTemp) ? (decTemp / (Decimal)2.54).ToString(strNumberFormat) : "0";
                    measurementWaist = Decimal.TryParse(measurementWaist, out decTemp) ? (decTemp / (Decimal)2.54).ToString(strNumberFormat) : "0";
                    measurementHip = Decimal.TryParse(measurementHip, out decTemp) ? (decTemp / (Decimal)2.54).ToString(strNumberFormat) : "0";

                    weightHistoryGainWeight = Decimal.TryParse(weightHistoryGainWeight, out decTemp) ? (decTemp / (Decimal)0.45359237).ToString(strNumberFormat) : "0";
                }
                else
                {
                    measurementHeight = Decimal.TryParse(measurementHeight, out decTemp) ? (decTemp * 100).ToString(strNumberFormat) : "0";
                    measurementWeight = Decimal.TryParse(measurementWeight, out decTemp) ? decTemp.ToString(strNumberFormat) : "0";
                    measurementIWeight = Decimal.TryParse(measurementIWeight, out decTemp) ? decTemp.ToString(strNumberFormat) : "0";
                    measurementEWeight = Decimal.TryParse(measurementEWeight, out decTemp) ? decTemp.ToString(strNumberFormat) : "0";

                    measurementNeck = Decimal.TryParse(measurementNeck, out decTemp) ? decTemp.ToString(strNumberFormat) : "0";
                    measurementWaist = Decimal.TryParse(measurementWaist, out decTemp) ? decTemp.ToString(strNumberFormat) : "0";
                    measurementHip = Decimal.TryParse(measurementHip, out decTemp) ? decTemp.ToString(strNumberFormat) : "0";

                    weightHistoryGainWeight = Decimal.TryParse(weightHistoryGainWeight, out decTemp) ? decTemp.ToString(strNumberFormat) : "0";
                }

                decimal decMeasurementNeck = measurementNeck.Trim() == "" ? 0 : Math.Round(Decimal.Parse(measurementNeck), 1);
                decimal decMeasurementWaist = measurementWaist.Trim() == "" ? 0 : Math.Round(Decimal.Parse(measurementWaist), 1);
                decimal decMeasurementHip = measurementHip.Trim() == "" ? 0 : Math.Round(Decimal.Parse(measurementHip), 1);
                decimal decWHR = 0;
                decimal decMeasurementWHR = 0;


                if (decMeasurementWaist > 0 && decMeasurementHip > 0)
                {
                    decWHR = decMeasurementWaist / decMeasurementHip;
                    decMeasurementWHR = Math.Round(decWHR, 1);
                }

                #endregion

                #region var declaration Comorbidities
                //var declaration Comorbidities
                string comorbiditiesTitle = "Current Health Status:";
                Boolean comorbiditiesExist = false;
                string comorbidities = "";
                string comorbiditiesName = "";
                string comorbiditiesChoice = "";
                string comorbiditiesRank = "";
                string comorbiditiesNotes = "";
                
                string comorbiditiesMedsIntro = "# of antihypertensive meds: ";
                string comorbiditiesData = " has the following medical problems:";
                string comorbiditiesNoData = " has no associated medical problems.";

                string[,] comorbiditiesACSArr = {{"ACS_Smoke","Current Smoker within 1 year","Pulmonary"},{"ACS_Oxy","Oxygen Dependent","Pulmonary"},{"ACS_Embo","History of Pulmonary Embolism","Pulmonary"},
                {"ACS_Copd","History of Severe COPD","Pulmonary"},{"ACS_Cpap","Obstructive Sleep Apnea req. CPAP or BiPAP","Pulmonary"},{"ACS_Sho","Shortness of Breath with Exertion","Pulmonary"},{"ACS_Gerd","GERD req. medications","Gastrointestinal"},
                {"ACS_Gal","Gallstone Disease","Gastrointestinal"},{"ACS_Muscd","Musculoskeletal Disease","Musculoskeletal"},{"ACS_Pain","Activity limited by pain","Musculoskeletal"},
                {"ACS_Meds","Requires daily medication","Musculoskeletal"},{"ACS_Surg","Surgical intervention planned or performed","Musculoskeletal"},{"ACS_Mob","Uses mobility device","Musculoskeletal"},
                {"GEN_RenalInsuff","Renal Insufficiency (Creat >2)","RENAL"},{"GEN_RenalFail","Renal Failure req. dialysis","RENAL"},{"ACS_Uri","Urinary Stress Incontinence","RENAL"},
                {"ACS_Myo","History of Myocardinal Infarction","Cardiac"},{"ACS_Pci","Previous PCI","Cardiac"},{"ACS_Csurg","Previous Cardiac Surgery","Cardiac"},
                {"ACS_Lipid","Hyperlipidemia req. medications","Cardiac"},{"ACS_Hyper","Hypertension req. medications","Cardiac"},{"ACS_Dvt","History of DVT requiring therapy","Vascular"},
                {"ACS_Venous","Venous Stasis","Vascular"},{"ACS_Health","Functional Health Status Prior to Surgery","Other"},{"ACS_Diab","Diabetes Mellitus","Other"},
                {"GEN_Steroid","Chronic Steroids/ Immunosuppression","Other"},{"GEN_Therapeutic","Therapeutic anticogulation","Other"},{"ACS_Obese","Previous obesity/ foregut surgery","Other"} ,{"ACS_Fat","Fatigue","General"}
                };

                string[,] comorbiditiesArr = {{"AUS_EndDiab","Diabetes","Endocrine"},{"AUS_EndThy","Thyroid disease","Endocrine"},{"AUS_EndOtherDesc","AUS_EndOtherName","Endocrine"},
                {"AUS_PulAsthma","Asthma","Pulmonary"},{"AUS_PulApnea","Sleep Apnea","Pulmonary"},{"AUS_PulEmb","Pulmonary emboli","Pulmonary"},{"AUS_PulOtherDesc","AUS_PulOtherName","Pulmonary"},
                {"AUS_GasRef","Heartburn or reflux","Gastrointestinal"},{"AUS_GasUlc","Peptic ulcer","Gastrointestinal"},{"AUS_GasGall","Gallstones","Gastrointestinal"},{"AUS_GasHep","Hepatitis","Gastrointestinal"},{"AUS_GasOtherDesc","AUS_GasOtherName","Gastrointestinal"},
                {"AUS_CvsIsc","Ischaemic heart disease","Cardiovascular"},{"AUS_CvsBlood","High blood pressure","Cardiovascular"},{"AUS_CvsCol","Abnormal cholesterol or lipids","Cardiovascular"},{"AUS_CvsDVT","History of DVT","Cardiovascular"},
                {"AUS_CvsVen","Venous stasis","Cardiovascular"},{"AUS_CvsAnti","Anticoagulant therapy","Cardiovascular"},{"AUS_CvsOtherDesc","AUS_CvsOtherName","Cardiovascular"},
                {"AUS_PsyDep","Depression","Psychiatric And Neurological"},{"AUS_PsyAnx","Anxiety states","Psychiatric And Neurological"},{"AUS_PsyPhob","Phobias","Psychiatric And Neurological"},{"AUS_PsyEat","Binge eating disorder","Psychiatric And Neurological"},
                {"AUS_PsyHead","Headache","Psychiatric And Neurological"},{"AUS_PsyStroke","Stroke","Psychiatric And Neurological"},{"AUS_PsyOtherDesc","AUS_PsyOtherName","Psychiatric And Neurological"},
                {"AUS_MuscBack","Back pain","Musculoskeletal"},{"AUS_MuscHip","Hip pain","Musculoskeletal"},{"AUS_MuscKnee","Knee pain","Musculoskeletal"},
                {"AUS_MuscFeet","Pain in feet","Musculoskeletal"},{"AUS_MuscFibr","Fibromyalgia","Musculoskeletal"},{"AUS_MuscOtherDesc","AUS_MuscOtherName","Musculoskeletal"},
                {"AUS_GenInf","Infertility","Genitourinary"},{"AUS_GenRen","Renal insufficiency","Genitourinary"},{"AUS_GenUri","Urinary incontinence","Genitourinary"},
                {"AUS_OtherPso","Psoriasis","Other"},{"AUS_OtherSkin","Other skin disorders","Other"},{"AUS_OtherCancer","History of cancer","Other"},
                {"AUS_OtherAnemia","History of anemia","Other"},{"AUS_OtherOtherDesc","AUS_OtherOtherName","Other"}
                };

                string[,] comorbiditiesBoldArr = {{"CVS_Hypertension","Hypertension","Cardiovascular Disease"},{"CVS_Congestive","Congestive Heart Failure","Cardiovascular Disease"}
                ,{"CVS_Ischemic","Ischemic Heart Disease","Cardiovascular Disease"},{"CVS_Angina","Angina Assessment","Cardiovascular Disease"},{"CVS_Peripheral","Peripheral Vascular Disease","Cardiovascular Disease"}
                ,{"CVS_Lower","Lower Extremity Edema","Cardiovascular Disease"},{"CVS_DVT","DVT/PE","Cardiovascular Disease"}
                ,{"MET_Glucose","Diabetes","Metabolic"},{"MET_Lipids","Lipids (Dyslipidemia or Hyperlipidemia)","Metabolic"},{"MET_Gout","Gout Hyperuricemia","Metabolic"}
                ,{"PUL_Obstructive","Obstructive Sleep Apnea","Pulmonary"},{"PUL_Obesity","Obesity Hypoventilation Syndrome","Pulmonary"}
                ,{"PUL_PulHypertension","Pulmonary Hypertension","Pulmonary"},{"PUL_Asthma","Asthma","Pulmonary"}
                ,{"GAS_Gerd","GERD","Gastrointestinal"},{"GAS_Cholelithiasis","Cholelithiasis","Gastrointestinal"},{"GAS_Liver","Liver Disease","Gastrointestinal"}
                ,{"MUS_BackPain","Back Pain","Musculoskeletal"},{"MUS_Musculoskeletal","Musculoskeletal Disease","Musculoskeletal"},{"MUS_Fibromyalgia","Fibromyalgia","Musculoskeletal"}
                ,{"REPRD_Polycystic","Polycystic Ovary Syndrome","Reproductive"},{"REPRD_Menstrual","Menstrual Irregularities (not PCOS)","Reproductive"}
                ,{"PSY_Impairment","Psychosocial Impairment","Psychosocial"},{"PSY_Depression","Depression","Psychosocial"},{"PSY_MentalHealth","Confirmed Mental Health Diagnosis","Psychosocial"}
                ,{"PSY_Alcohol","Alcohol Use","Psychosocial"},{"PSY_Tobacoo","Tobacco Use","Psychosocial"},{"PSY_Abuse","Substance Abuse","Psychosocial"}
                ,{"GEN_Stress","Stress Urinary Incontinence","General"},{"GEN_Cerebri","Pseudotumor Cerebr","General"},{"GEN_Hernia","Abdominal Hernia","General"}
                ,{"GEN_Functional","Functional Status","General"},{"GEN_Skin","Abdominal Skin/Pannus","General"}                        
                ,{"GEN_RenalInsuff","Renal Insufficiency / Creatinine > 2","General"},{"GEN_RenalFail","Renal Failure Requiring Dialysis","General"}                        
                ,{"GEN_Steroid","Chronic Steroid Use and or Immunosuppresant use","General"},{"GEN_Therapeutic","Therapeutic Anticoagulation","General"}
                ,{"GEN_PrevPCISurgery","Previous PCI and Previous Cardiac Surgery","General"}
                };

                string comorbiditiesChoiceDisplay = "PSY_MentalHealth";

                string comorbiditiesReview = "\nOn review of systems there were no other relevant health problems.";
                string introNotes = "";

                #endregion
                
                #region var declaration Medication
                //var declaration Medication
                string medicationTitle = "Current Medications:";
                string medicationName = "";
                string medicationDosage = "";
                string medicationFrequency = "";
                String[] medications;
                String[] medication;
                string medicationIntro = " is currently not under any medication.";
                #endregion

                #region var declaration Weight History
                //var declaration Weight History
                string weightHistoryTitle = "Weight Loss History:";
                string weightHistoryLoseYears = dsPatientEMR.Tables[0].Rows[0]["WeightHistory_LoseYears"].ToString().Trim() == "" ? "" : " for " + dsPatientEMR.Tables[0].Rows[0]["WeightHistory_LoseYears"].ToString().Trim() + " years";
                Boolean weightHistoryExist = false;
                string weightHistoryExistString = "";

                string weighHistoryTryMethod = dsPatientEMR.Tables[0].Rows[0]["WeightHistory_TryMethod"].ToString();
                string weighHistoryTryMethodDisplay = "";

                string[] weighHistoryTryMethodArr = weighHistoryTryMethod.Split(new char[] { '-' });

                string demographicHasTried = " has also tried the following:";
                string demographicTriedHowLong = " has been trying to lose weight";
                string demographicGroupListDisplayIntro = "Commercial weight loss groups: ";
                string demographicDietPillsListDisplayIntro = "Diet Pills: ";
                string demographicAdviceListDisplayIntro = "Professional advice: ";
                string demographicDietListDisplayIntro = "Very low calorie diets: ";
                string demographicOtherListDisplayIntro = "Others medical treatments: ";
                string demographicTreatmentListDisplay = "Surgical treatments: ";
                string demographicCosmeticProcedureDisplay = "Cosmetic procedures: ";



                weightHistoryGainWeight = weightHistoryGainWeight == "" ? "" : " gain of approximately " + weightHistoryGainWeight + " " +patientWeightMeasurment; 
                string weightHistoryGainYears = dsPatientEMR.Tables[0].Rows[0]["WeightHistory_GainYears"].ToString().Trim() == "" ? "" : " in the last " + dsPatientEMR.Tables[0].Rows[0]["WeightHistory_GainYears"].ToString().Trim() + " years.";

                string weightHistoryGain = (weightHistoryGainWeight == "" && weightHistoryGainYears == "") ? "" : "There is a history of weight gain in recent years with a known weight" + weightHistoryGainWeight + weightHistoryGainYears;

                if (weightHistoryGain != "")
                {
                    weightHistoryExist = true;
                }

                if (weighHistoryTryMethod.Trim() != "")
                {
                    foreach (string tempWeight in weighHistoryTryMethodArr)
                    {
                        if (tempWeight.ToLower() == "chkweighthistorylossdiet")
                        {
                            if (weighHistoryTryMethodDisplay == "")
                                weighHistoryTryMethodDisplay += " by";
                            weighHistoryTryMethodDisplay += " dieting";
                        }

                        if (tempWeight.ToLower() == "chkweighthistorylossexercise")
                        {
                            if (weighHistoryTryMethodDisplay == "")
                                weighHistoryTryMethodDisplay += " by";
                            else
                                weighHistoryTryMethodDisplay += " and";
                            weighHistoryTryMethodDisplay += " exercise";
                        }
                    }
                }

                string weighHistoryGroupList = dsPatientEMR.Tables[0].Rows[0]["WeightHistory_GroupList"].ToString();
                string weighHistoryGroupListDisplay = "";

                string[] weighHistoryGroupListArr = weighHistoryGroupList.Split(new char[] { '-' });

                if (weighHistoryGroupList.Trim() != "")
                {
                    weighHistoryGroupListDisplay = "";
                    weightHistoryExist = true;
                    foreach (string tempGroupList in weighHistoryGroupListArr)
                    {
                        if (tempGroupList.ToLower() == "chkweighthistorylossgrpjg")
                            weighHistoryGroupListDisplay += "Jenny Craig, ";
                        if (tempGroupList.ToLower() == "chkweighthistorylossgrpww")
                            weighHistoryGroupListDisplay += "Weight Watchers, ";
                        if (tempGroupList.ToLower() == "chkweighthistorylossgrpgm")
                            weighHistoryGroupListDisplay += "Gloria Marshall, ";
                        if (tempGroupList.ToLower() == "chkweighthistorylossgrple")
                            weighHistoryGroupListDisplay += "Lite n'easy, ";
                        if (tempGroupList.ToLower() == "chkweighthistorylossgrpns")
                            weighHistoryGroupListDisplay += "Nutrisystem, ";
                        if (tempGroupList.ToLower() == "chkweighthistorylossgrptc")
                            weighHistoryGroupListDisplay += "TOWN Club, ";
                        if (tempGroupList.ToLower() == "chkweighthistorylossgrphl")
                            weighHistoryGroupListDisplay += "Herbal Life, ";
                        if (tempGroupList.ToLower() == "chkweighthistorylossgrpgb")
                            weighHistoryGroupListDisplay += "Gut Busters, ";
                        if (tempGroupList.ToLower() == "chkweighthistorylossgrpss")
                            weighHistoryGroupListDisplay += "SureSlim, ";
                        if (tempGroupList.ToLower() == "chkweighthistorylossgrpad")
                            weighHistoryGroupListDisplay += "Atkins Diet, ";
                        if (tempGroupList.ToLower() == "chkweighthistorylossgrpgd")
                            weighHistoryGroupListDisplay += "Grapefruit Diet, ";
                        if (tempGroupList.ToLower() == "chkweighthistorylossgrplw")
                            weighHistoryGroupListDisplay += "LA Weight Loss, ";
                        if (tempGroupList.ToLower() == "chkweighthistorylossgrpmf")
                            weighHistoryGroupListDisplay += "Medifast, ";
                        if (tempGroupList.ToLower() == "chkweighthistorylossgrpml")
                            weighHistoryGroupListDisplay += "Metabolife, ";
                        if (tempGroupList.ToLower() == "chkweighthistorylossgrpof")
                            weighHistoryGroupListDisplay += "Optifast, ";
                        if (tempGroupList.ToLower() == "chkweighthistorylossgrpp")
                            weighHistoryGroupListDisplay += "Protein, ";
                        if (tempGroupList.ToLower() == "chkweighthistorylossgrpsf")
                            weighHistoryGroupListDisplay += "Slim Fast, ";
                        if (tempGroupList.ToLower() == "chkweighthistorylossgrpsb")
                            weighHistoryGroupListDisplay += "South Beach, ";
                        if (tempGroupList.ToLower() == "chkweighthistorylossgrpt")
                            weighHistoryGroupListDisplay += "TOPS, ";
                    }

                    weighHistoryGroupListDisplay = weighHistoryGroupListDisplay.Substring(0, weighHistoryGroupListDisplay.Length - 2);
                }
                if (dsPatientEMR.Tables[0].Rows[0]["WeightHistory_GroupOther"].ToString().Trim() != "")
                {
                    weighHistoryGroupListDisplay += ", " + dsPatientEMR.Tables[0].Rows[0]["WeightHistory_GroupOther"].ToString();
                    weightHistoryExist = true;
                }

                string weighHistoryPillList = dsPatientEMR.Tables[0].Rows[0]["WeightHistory_PillList"].ToString();
                string weighHistoryPillListDisplay = "";

                string[] weighHistoryPillListArr = weighHistoryPillList.Split(new char[] { '-' });

                if (weighHistoryPillList.Trim() != "")
                {
                    weightHistoryExist = true;
                    foreach (string tempPillList in weighHistoryPillListArr)
                    {
                        if (tempPillList.ToLower() == "chkweighthistorylossdieta")
                            weighHistoryPillListDisplay += "Adifax, ";
                        if (tempPillList.ToLower() == "chkweighthistorylossdietam")
                            weighHistoryPillListDisplay += "Amphetamines, ";
                        if (tempPillList.ToLower() == "chkweighthistorylossdietd")
                            weighHistoryPillListDisplay += "Duromine, ";
                        if (tempPillList.ToLower() == "chkweighthistorylossdietm")
                            weighHistoryPillListDisplay += "Meridia, ";
                        if (tempPillList.ToLower() == "chkweighthistorylossdietpf")
                            weighHistoryPillListDisplay += "Phen-Fen, ";
                        if (tempPillList.ToLower() == "chkweighthistorylossdietp")
                            weighHistoryPillListDisplay += "Phentermine, ";
                        if (tempPillList.ToLower() == "chkweighthistorylossdietr")
                            weighHistoryPillListDisplay += "Reductil, ";
                        if (tempPillList.ToLower() == "chkweighthistorylossdietre")
                            weighHistoryPillListDisplay += "Redux, ";
                        if (tempPillList.ToLower() == "chkweighthistorylossdiett")
                            weighHistoryPillListDisplay += "Tenuate, ";
                        if (tempPillList.ToLower() == "chkweighthistorylossdietx")
                            weighHistoryPillListDisplay += "Xenical, ";
                    }

                    weighHistoryPillListDisplay = weighHistoryPillListDisplay.Substring(0, weighHistoryPillListDisplay.Length - 2);
                }

                if (dsPatientEMR.Tables[0].Rows[0]["WeightHistory_PillOther"].ToString().Trim() != "")
                {
                    weighHistoryPillListDisplay += ", " + dsPatientEMR.Tables[0].Rows[0]["WeightHistory_PillOther"].ToString();
                    weightHistoryExist = true;
                }

                string weighHistoryAdviceList = dsPatientEMR.Tables[0].Rows[0]["WeightHistory_AdviceList"].ToString();
                string weighHistoryAdviceListDisplay = "";

                string[] weighHistoryAdviceListArr = weighHistoryAdviceList.Split(new char[] { '-' });

                if (weighHistoryAdviceList.Trim() != "")
                {
                    weightHistoryExist = true;
                    foreach (string tempAdviceList in weighHistoryAdviceListArr)
                    {
                        if (tempAdviceList.ToLower() == "chkweighthistorylosspald")
                            weighHistoryAdviceListDisplay += "Local Doctor, ";
                        if (tempAdviceList.ToLower() == "chkweighthistorylosspad")
                            weighHistoryAdviceListDisplay += "Dietitian, ";
                        if (tempAdviceList.ToLower() == "chkweighthistorylosspan")
                            weighHistoryAdviceListDisplay += "Naturopath, ";
                        if (tempAdviceList.ToLower() == "chkweighthistorylosspah")
                            weighHistoryAdviceListDisplay += "Hypnotherapist, ";
                        if (tempAdviceList.ToLower() == "chkweighthistorylosspap")
                            weighHistoryAdviceListDisplay += "Psychologist, ";
                        if (tempAdviceList.ToLower() == "chkweighthistorylosspaa")
                            weighHistoryAdviceListDisplay += "Acupuncture, ";
                    }

                    weighHistoryAdviceListDisplay = weighHistoryAdviceListDisplay.Substring(0, weighHistoryAdviceListDisplay.Length - 2);
                }

                if (dsPatientEMR.Tables[0].Rows[0]["WeightHistory_AdviceOther"].ToString().Trim() != "")
                {
                    weighHistoryAdviceListDisplay += ", " + dsPatientEMR.Tables[0].Rows[0]["WeightHistory_AdviceOther"].ToString();
                    weightHistoryExist = true;
                }

                string weighHistoryDietList = dsPatientEMR.Tables[0].Rows[0]["WeightHistory_DietList"].ToString();
                string weighHistoryDietListDisplay = "";

                string[] weighHistoryDietListArr = weighHistoryDietList.Split(new char[] { '-' });

                if (weighHistoryDietList.Trim() != "")
                {
                    weightHistoryExist = true;
                    foreach (string tempDietList in weighHistoryDietListArr)
                    {
                        if (tempDietList.ToLower() == "chkweighthistorylosslcmd")
                            weighHistoryDietListDisplay += "Optifast, ";
                        if (tempDietList.ToLower() == "chkweighthistorylosslcot")
                            weighHistoryDietListDisplay += "Other, ";
                    }

                    weighHistoryDietListDisplay = weighHistoryDietListDisplay.Substring(0, weighHistoryDietListDisplay.Length - 2);
                }

                string weighHistoryOtherList = dsPatientEMR.Tables[0].Rows[0]["WeightHistory_Other"].ToString();
                string weighHistoryOtherListDisplay = "";

                string[] weighHistoryOtherListArr = weighHistoryOtherList.Split(new char[] { '-' });

                if (weighHistoryOtherList.Trim() != "")
                {
                    weightHistoryExist = true;
                    foreach (string tempOtherList in weighHistoryOtherListArr)
                    {
                        if (tempOtherList.ToLower() == "chkweighthistorylossoit")
                            weighHistoryOtherListDisplay += "Injection therapy, ";
                        if (tempOtherList.ToLower() == "chkweighthistorylossohr")
                            weighHistoryOtherListDisplay += "Herbal remedies, ";
                        if (tempOtherList.ToLower() == "chkweighthistorylossowl")
                            weighHistoryOtherListDisplay += "Weight loss devices, ";
                    }

                    weighHistoryOtherListDisplay = weighHistoryOtherListDisplay.Substring(0, weighHistoryOtherListDisplay.Length - 2);
                }

                string weighHistoryTreatmentList = dsPatientEMR.Tables[0].Rows[0]["WeightHistory_TreatmentList"].ToString();
                string weighHistoryTreatmentListDisplay = "";

                string[] weighHistoryTreatmentListArr = weighHistoryTreatmentList.Split(new char[] { '-' });

                if (weighHistoryTreatmentList.Trim() != "")
                {
                    weightHistoryExist = true;
                    foreach (string tempTreatmentList in weighHistoryTreatmentListArr)
                    {
                        if (tempTreatmentList.ToLower() == "chkweighthistorylosssss")
                            weighHistoryTreatmentListDisplay += "Stomach stapling, ";
                        if (tempTreatmentList.ToLower() == "chkweighthistorylosssfg")
                            weighHistoryTreatmentListDisplay += "Fixed gastric banding, ";
                        if (tempTreatmentList.ToLower() == "chkweighthistorylosssag")
                            weighHistoryTreatmentListDisplay += "Adjustable Gastric Banding, ";
                        if (tempTreatmentList.ToLower() == "chkweighthistorylosssb")
                            weighHistoryTreatmentListDisplay += "Small bowel bypass, ";
                        if (tempTreatmentList.ToLower() == "chkweighthistorylosssa")
                            weighHistoryTreatmentListDisplay += "Apronectomy, ";
                        if (tempTreatmentList.ToLower() == "chkweighthistorylosssl")
                            weighHistoryTreatmentListDisplay += "Liposuction, ";
                    }

                    weighHistoryTreatmentListDisplay = weighHistoryTreatmentListDisplay.Substring(0, weighHistoryTreatmentListDisplay.Length - 2);
                }

                string weighHistoryCosmeticProcedureDisplay = "";
                if (dsPatientEMR.Tables[0].Rows[0]["WeightHistory_CosmeticList"].ToString().Trim() != "")
                {
                    weighHistoryCosmeticProcedureDisplay = dsPatientEMR.Tables[0].Rows[0]["WeightHistory_CosmeticList"].ToString();
                    weightHistoryExist = true;
                }
                #endregion
                
                #region var declaration Background
                //var declaration Background
                string backgroundTitle = "Relevant family and past medical or surgical history:";
                string backgroundFamilyHistory = dsPatientEMR.Tables[0].Rows[0]["Background_FamilyHistory"].ToString().Trim();
                
                string backgroundPastHealthTitle = "Past history re health:";
                string backgroundPastHealth = dsPatientEMR.Tables[0].Rows[0]["Background_PastHealth"].ToString().Trim();

                string backgroundNoHistory = "There is not any relevant history background.";

                string[,] backgroundHistory = new string[,] { {"Background_Diabetes", "Diabetes"}, {"Background_HeartDisease","Heart Disease"}, {"Background_Hypertension","Hypertension"}, 
                    {"Background_Gout","Gout"}, {"Background_Obesity","Obesity"}, {"Background_Snoring","Snoring"}, {"Background_Asthma","Asthma"}, {"Background_HighCholesterol","High Cholesterol"}};
                
                int backgroundCount = 0;

                string backgroundName = "";
                string tempBackground = "";
                string familyMembers = "";
                string tempMember = "";
                string tempFamilyMember = "";
                string familyBackground = "";
                string[] familyMemberArr;

                #endregion

                #region var declaration Previous Operation
                //var declaration Previous Operation
                string operationPBS = "";
                string operationPBNS = "";
                if (dsPatientBold.Tables[0].Rows.Count > 0)
                {
                    operationPBS = dsPatientBold.Tables[0].Rows[0]["PBS_Procedure"].ToString().Trim();
                    operationPBNS = dsPatientBold.Tables[0].Rows[0]["PNBS_Procedure"].ToString().Trim();
                }

                String[] nonBariatrics;
                string nonBariatricSurgeryName = "";

                String[] bariatrics;
                String[] bariatric;
                String bariatricSurgeryYear;
                String bariatricSurgeryName;
                String[] operationPBSArr;
                String[] operationPBNSArr;         
       
                string operationPBSIntro = "Past Bariatric Surgeries: ";
                string operationPNBSIntro = "Relevant Non-Bariatric Surgeries: ";
                string operationNoSurgery = "There is not any relevant history.";
                #endregion

                #region var declaration Allergies
                //var declaration Allergies
                string allergiesTitle = "Allergies:";
                Boolean allergiesExist = false;
                Boolean allergiesHaveAllergy = dsPatientEMR.Tables[0].Rows[0]["Allergy_HaveAllergy"].ToString().Equals(Boolean.TrueString);
                string allergiesListAllergy = dsPatientEMR.Tables[0].Rows[0]["Allergy_ListAllergy"].ToString().Trim() == "" ? "" : " to " + dsPatientEMR.Tables[0].Rows[0]["Allergy_ListAllergy"].ToString().Trim();
                Boolean allergiesHaveMedication = dsPatientEMR.Tables[0].Rows[0]["Allergy_HaveMedication"].ToString().Equals(Boolean.TrueString);
                string allergiesListMedication = dsPatientEMR.Tables[0].Rows[0]["Allergy_ListMedication"].ToString().Trim() == "" ? "" : "; " + dsPatientEMR.Tables[0].Rows[0]["Allergy_ListMedication"].ToString().Trim();
                Boolean allergiesLatex = dsPatientEMR.Tables[0].Rows[0]["Allergy_Latex"].ToString().Equals(Boolean.TrueString);
                string allergiesAnesthetic = dsPatientEMR.Tables[0].Rows[0]["Allergy_Anesthetic"].ToString().Trim();
                Boolean allergiesExcessBleed = dsPatientEMR.Tables[0].Rows[0]["Allergy_ExcessBleed"].ToString().Equals(Boolean.TrueString);
                string allergiesAnestheticRisk = dsPatientEMR.Tables[0].Rows[0]["Allergy_AnestheticRisk"].ToString().Trim();

                string allergiesHaveAlleryIntro = " has some allergies";
                string allergiesMedicationIntro = " had an allergic reaction to some medications";
                string allergiesLatexIntro = " is allergic to latex";
                string allergiesAnestheticIntro1 = " or any of ";
                string allergiesAnestheticIntro2 = " family had an adverse reaction to an anesthetic; ";
                string allergiesAnestheticRiskIntro = " knows any other surgical or anaesthetic risks; ";
                string allergiesRiskIntro = " has no associated allergies or special risk factor.";
                string allergiesExcessBleedIntro = " has a tendency to bleed excessively";
                string allergiesNoAnesthesia = " has no problems with anesthesia.";
                
                #endregion

                #region var declaration Physical Examination
                //var Physical Examination
                string examinationTitle = "Physical Examination:";
                string examination = "";
                string examinationResult = "";
                string[] examinationArr = { "GAO", "HNG", "HNH", "HNM", "HNN", "CH", "CP", "AA", "LSR", "LSB", "LSA", "MSO", "MF" };
                string examinationNotes = dsPatientEMR.Tables[0].Rows[0]["Exam_Notes"].ToString();
                #endregion
                                
                #region var declaration Labs
                //var declaration Labs
                string labsTitle = "Labs:";
                string labsNotes = dsPatientEMR.Tables[0].Rows[0]["lab_notes"].ToString();
                #endregion

                #region var declaration Investigations
                //var declaration Investigations
                string investigationTitle = "Investigations and Referrals:";
                Boolean investigationExist = false;
                string investigation = "";
                string investigationTitle2 = "The following investigations are planned: ";
                string investigationSuggested = "";
                string investigationAction = "";
                string[,] investigationArr = { { "RBS", "Routine blood studies"}, {"ABS","Additional blood studies"}, {"RFT","Respiratory function tests"},
                    {"ABG","Arterial blood gases"}, {"EET","Exercise EKG testing"}, {"BM","Barium meal"}, 
                    {"EMS","Esophageal manometry/pH studies"}, {"EKG","EKG"}, {"CX","Chest X-ray"}};
                //, {"P","Polysomnography"} -- remove Polysomnography
                #endregion

                #region var declaration Special Investigations
                //var declaration Special Investigations
                string investigationNoData = "There are no investigations planned.";
                string specInvestigation = "";
                string specInvestigationAction = "";
                string specInvestigationSuggested = "";
                string[,] specInvestigationArr = { {"SS", "Sleep Study"}, {"GE","Upper GI Endoscopy"}, {"SP","Spirometry"}, 
                    {"NA","Nutrition Assessment"}, {"PS","Psychological Assessment"}, {"OA","Other Assessments"}};
                #endregion

                #region var declaration Referrals
                //var declaration Referrals
                string referralTitle = "The following referrals are planned:";
                string referralNoData = "There are no referrals planned.";
                Boolean referralExist = false;
                string referral = "";
                string referralResult = "";
                string[,] referralArr = { { "PA", "Psychiatry assessment"}, {"C","Cardiologist"}, {"RP","Respiratory physician"},
                    {"E","Endocrinologist"}, {"AP","Adolescent physician"}, {"GM","General Medical"},{"O","Other"}};
                #endregion

                #region var declaration Management Plan
                //var declaration Management Plan
                string managementTitle = "Management Plan:";
                string management = dsPatientEMR.Tables[0].Rows[0]["Management"].ToString().Trim() == "" ? "" : dsPatientEMR.Tables[0].Rows[0]["Management"].ToString();
                string managementIntro1 = "A weight loss plan was discussed with " + demographicFirstname + " " + demographicSurname + " at today's visit including the following:";
                string managementIntro2 = "1. A diet of increased protein, decreased carbohydrates and increased water, vegetable, and fruit intake that should consist of between 1200 and 1500 calories per day.";
                string managementIntro3 = "2. Exercise. The patient was encouraged to get 30 minutes of cardiovascular exercises three to five times per week.";
                string managementIntro4 = "3. Eating habit changes with the band were discussed including dime-sized bites with adequate chewing, appropriate food choices, and adequate meal plan.";
                string managementIntro5 = "The patient can return in one month for additional weight loss visits as necessary.";
                string managementIntro6 = "Due to the fact that the patient has been unseccessful with medical and dietry regimens, and has a status of medical necessity, "+demographicGender.ToLower()+" is recommended for minimally invasive adjustable laproscopic gastric banding, pending results of the medical clearance.";
                #endregion

                #region var declaration Signature
                //var declaration Signature
                string signatureMedicalProvider = "Medical Provider";
                string signatureDegree = dsPatientData.Tables[0].Rows[0]["Degrees"].ToString().Trim();
                string signatureSpeciality = dsPatientData.Tables[0].Rows[0]["Speciality"].ToString().Trim();
                string signatureAddress1 = dsPatientData.Tables[0].Rows[0]["DrAddress1"].ToString().Trim();
                string signatureAddress2 = dsPatientData.Tables[0].Rows[0]["DrAddress2"].ToString().Trim();
                string signatureAddress3 = dsPatientData.Tables[0].Rows[0]["DrAddress3"].ToString().Trim();
                #endregion
                
                if (reportFormat == "pdf")
                {
                    #region declare table and width
                    iTextSharp.text.Document oDoc = new iTextSharp.text.Document(PageSize.LETTER, 0, 0, 30f, 30f);
                    PdfWriter.GetInstance(oDoc, new FileStream(saveFilePath, FileMode.Create));

                    Cell cell = new Cell("");
                    Font H2 = new Font(Font.HELVETICA, 13, Font.BOLD);
                    Font H3 = new Font(Font.HELVETICA, 11, Font.BOLD);
                    Font smallFont = new Font(Font.HELVETICA, 6, 0);
                    Font normalFont = new Font(Font.HELVETICA, 10, 0);
                    Font normalBoldFont = new Font(Font.HELVETICA, 10, Font.BOLD);
                    

                    Phrase footPhrase = new Phrase(demographicPatientNameFooter + "                                                                                                                " + demographicConsutlationComplete + "                                                                                                                Page: ", smallFont);
                    HeaderFooter footer = new HeaderFooter(footPhrase, true);
                    footer.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    footer.Alignment = iTextSharp.text.Element.ALIGN_BOTTOM;
                    footer.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    oDoc.Footer = footer;

                    oDoc.Open();

                    //create header
                    iTextSharp.text.Table tableLogo = new iTextSharp.text.Table(1, 1);
                    iTextSharp.text.Table tableFacility = new iTextSharp.text.Table(1, 7);
                    iTextSharp.text.Table tableMargin = new iTextSharp.text.Table(1, 1);
                    iTextSharp.text.Table tableTitle = new iTextSharp.text.Table(1, 1);
                    iTextSharp.text.Table tableDemographic = new iTextSharp.text.Table(2, 6);
                    iTextSharp.text.Table tableMeasurement = new iTextSharp.text.Table(3, 7);
                    iTextSharp.text.Table tableComplaint = new iTextSharp.text.Table(1, 3);
                    iTextSharp.text.Table tableComorbidities = new iTextSharp.text.Table(1, 5);
                    iTextSharp.text.Table tableComorbiditiesResult = new iTextSharp.text.Table(1, 30);
                    iTextSharp.text.Table tableMedication = new iTextSharp.text.Table(1, 34);
                    iTextSharp.text.Table tableWeightHistory = new iTextSharp.text.Table(1, 10);
                    iTextSharp.text.Table tableBackground = new iTextSharp.text.Table(1, 10);
                    iTextSharp.text.Table tablePastHistory = new iTextSharp.text.Table(1, 3);
                    iTextSharp.text.Table tableAllergies = new iTextSharp.text.Table(1, 7);
                    iTextSharp.text.Table tablePreviousOperation = new iTextSharp.text.Table(1, 12);
                    iTextSharp.text.Table tableExamination = new iTextSharp.text.Table(1, 15);
                    iTextSharp.text.Table tableLabs = new iTextSharp.text.Table(1, 2);
                    iTextSharp.text.Table tableInvestigation = new iTextSharp.text.Table(1, 5);
                    iTextSharp.text.Table tableInvestigationResult = new iTextSharp.text.Table(1, 43);
                    iTextSharp.text.Table tableReferral = new iTextSharp.text.Table(1, 5);
                    iTextSharp.text.Table tableReferralResult = new iTextSharp.text.Table(1, 12);
                    iTextSharp.text.Table tableManagement = new iTextSharp.text.Table(1, 10);
                    iTextSharp.text.Table tableSignature = new iTextSharp.text.Table(2, 12);

                    // set *column* widths
                    float[] logoWidths = { 1f };
                    tableLogo.Spacing = 1;
                    tableLogo.Padding = margin;
                    tableLogo.Widths = logoWidths;
                    tableLogo.DefaultCell.BorderWidth = 0;
                    tableLogo.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    tableLogo.DefaultCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    tableLogo.TableFitsPage = true;
                    tableLogo.BorderColor = new Color(255, 255, 255);
                                       
                    // set *column* widths
                    float[] marginWidths = { 1f };
                    tableMargin.Spacing = 1;
                    tableMargin.Padding = margin;
                    tableMargin.Widths = marginWidths;
                    tableMargin.DefaultCell.BorderWidth = 0;
                    tableMargin.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    tableMargin.DefaultCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    tableMargin.TableFitsPage = true;
                    tableMargin.BorderColor = new Color(255, 255, 255);

                    float[] titleWidths = { 1f };
                    tableTitle.Spacing = 1;
                    tableTitle.Widths = titleWidths;
                    tableTitle.DefaultCell.BorderWidth = 0;
                    tableTitle.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    tableTitle.DefaultCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    tableTitle.TableFitsPage = true;
                    tableTitle.BorderColor = new Color(255, 255, 255);
                    
                    float[] facilityWidths = { 1f };
                    tableFacility.Spacing = 1;
                    tableFacility.Widths = facilityWidths;
                    tableFacility.DefaultCell.BorderWidth = 0;
                    tableFacility.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    tableFacility.TableFitsPage = true;
                    tableFacility.BorderColor = new Color(255, 255, 255);

                    float[] demographicWidths = { .45f, .55f };
                    tableDemographic.Spacing = 1;
                    tableDemographic.Widths = demographicWidths;
                    tableDemographic.DefaultCell.BorderWidth = 0;
                    tableDemographic.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    tableDemographic.TableFitsPage = true;
                    tableDemographic.BorderColor = new Color(255, 255, 255);

                    float[] measurementWidths = { .3f, .3f, .4f };
                    tableMeasurement.Spacing = 1;
                    tableMeasurement.Widths = measurementWidths;
                    tableMeasurement.DefaultCell.BorderWidth = 0;
                    tableMeasurement.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    tableMeasurement.TableFitsPage = true;
                    tableMeasurement.BorderColor = new Color(255, 255, 255);

                    float[] complaintWidths = { 1f };
                    tableComplaint.Spacing = 1;
                    tableComplaint.Widths = complaintWidths;
                    tableComplaint.DefaultCell.BorderWidth = 0;
                    tableComplaint.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    tableComplaint.TableFitsPage = true;
                    tableComplaint.BorderColor = new Color(255, 255, 255);

                    float[] comorbiditiesWidths = { 1f };
                    tableComorbidities.Spacing = 1;
                    tableComorbidities.Widths = comorbiditiesWidths;
                    tableComorbidities.DefaultCell.BorderWidth = 0;
                    tableComorbidities.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    tableComorbidities.TableFitsPage = true;
                    tableComorbidities.BorderColor = new Color(255, 255, 255);

                    float[] comorbiditiesResultWidths = { 1f };
                    tableComorbiditiesResult.Spacing = 1;
                    tableComorbiditiesResult.Widths = comorbiditiesResultWidths;
                    tableComorbiditiesResult.DefaultCell.BorderWidth = 0;
                    tableComorbiditiesResult.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    tableComorbiditiesResult.TableFitsPage = true;
                    tableComorbiditiesResult.BorderColor = new Color(255, 255, 255);

                    float[] medicationWidths = { 1f };
                    tableMedication.Spacing = 1;
                    tableMedication.Widths = medicationWidths;
                    tableMedication.DefaultCell.BorderWidth = 0;
                    tableMedication.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    tableMedication.TableFitsPage = true;
                    tableMedication.BorderColor = new Color(255, 255, 255);

                    float[] weightHistoryWidths = { 1f };
                    tableWeightHistory.Spacing = 1;
                    tableWeightHistory.Widths = weightHistoryWidths;
                    tableWeightHistory.DefaultCell.BorderWidth = 0;
                    tableWeightHistory.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    tableWeightHistory.TableFitsPage = true;
                    tableWeightHistory.BorderColor = new Color(255, 255, 255);

                    float[] backgroundWidths = { 1f };
                    tableBackground.Spacing = 1;
                    tableBackground.Widths = backgroundWidths;
                    tableBackground.DefaultCell.BorderWidth = 0;
                    tableBackground.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    tableBackground.TableFitsPage = true;
                    tableBackground.BorderColor = new Color(255, 255, 255);

                    float[] pastHistoryWidths = { 1f };
                    tablePastHistory.Spacing = 1;
                    tablePastHistory.Widths = pastHistoryWidths;
                    tablePastHistory.DefaultCell.BorderWidth = 0;
                    tablePastHistory.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    tablePastHistory.TableFitsPage = true;
                    tablePastHistory.BorderColor = new Color(255, 255, 255);

                    float[] allergiesWidths = { 1f };
                    tableAllergies.Spacing = 1;
                    tableAllergies.Widths = allergiesWidths;
                    tableAllergies.DefaultCell.BorderWidth = 0;
                    tableAllergies.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    tableAllergies.TableFitsPage = true;
                    tableAllergies.BorderColor = new Color(255, 255, 255);

                    float[] operationWidths = { 1f };
                    tablePreviousOperation.Spacing = 1;
                    tablePreviousOperation.Widths = operationWidths;
                    tablePreviousOperation.DefaultCell.BorderWidth = 0;
                    tablePreviousOperation.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    tablePreviousOperation.TableFitsPage = true;
                    tablePreviousOperation.BorderColor = new Color(255, 255, 255);

                    float[] examinationWidths = { 1f };
                    tableExamination.Spacing = 1;
                    tableExamination.Widths = examinationWidths;
                    tableExamination.DefaultCell.BorderWidth = 0;
                    tableExamination.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    tableExamination.TableFitsPage = true;
                    tableExamination.BorderColor = new Color(255, 255, 255);

                    float[] labsWidths = { 1f };
                    tableLabs.Spacing = 1;
                    tableLabs.Widths = labsWidths;
                    tableLabs.DefaultCell.BorderWidth = 0;
                    tableLabs.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    tableLabs.TableFitsPage = true;
                    tableLabs.BorderColor = new Color(255, 255, 255);

                    float[] investigationWidths = { 1f };
                    tableInvestigation.Spacing = 1;
                    tableInvestigation.Widths = investigationWidths;
                    tableInvestigation.DefaultCell.BorderWidth = 0;
                    tableInvestigation.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    tableInvestigation.TableFitsPage = true;
                    tableInvestigation.BorderColor = new Color(255, 255, 255);

                    float[] investigationResultWidths = { 1f };
                    tableInvestigationResult.Spacing = 1;
                    tableInvestigationResult.Widths = investigationResultWidths;
                    tableInvestigationResult.DefaultCell.BorderWidth = 0;
                    tableInvestigationResult.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    tableInvestigationResult.TableFitsPage = true;
                    tableInvestigationResult.BorderColor = new Color(255, 255, 255);

                    float[] referralWidths = { 1f };
                    tableReferral.Spacing = 1;
                    tableReferral.Widths = referralWidths;
                    tableReferral.DefaultCell.BorderWidth = 0;
                    tableReferral.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    tableReferral.TableFitsPage = true;
                    tableReferral.BorderColor = new Color(255, 255, 255);

                    float[] referralResultWidths = { 1f };
                    tableReferralResult.Spacing = 1;
                    tableReferralResult.Widths = referralResultWidths;
                    tableReferralResult.DefaultCell.BorderWidth = 0;
                    tableReferralResult.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    tableReferralResult.TableFitsPage = true;
                    tableReferralResult.BorderColor = new Color(255, 255, 255);

                    float[] managementWidths = { 1f };
                    tableManagement.Spacing = 1;
                    tableManagement.Widths = managementWidths;
                    tableManagement.DefaultCell.BorderWidth = 0;
                    tableManagement.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    tableManagement.TableFitsPage = true;
                    tableManagement.BorderColor = new Color(255, 255, 255);

                    float[] signatureWidths = { .7f, .3f };
                    tableSignature.Spacing = 1;
                    tableSignature.Widths = signatureWidths;
                    tableSignature.DefaultCell.BorderWidth = 0;
                    tableSignature.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    tableSignature.TableFitsPage = true;
                    tableSignature.BorderColor = new Color(255, 255, 255);
                    #endregion

                    #region Logo
                    //Logo----------------------------------------------------------------------------------------------
                    if (LogoPath != "")
                    {
                        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~" + LogoPath));
                        logo.ScalePercent(50);
                        //phrase = new Phrase(logo, normalFont);
                        cell = new Cell(logo);
                        cell.BorderWidth = 0;
                        tableLogo.AddCell(cell);
                        oDoc.Add(tableLogo);
                    }
                    #endregion
                    
                    #region Margin
                    //Margin----------------------------------------------------------------------------------------------
                    phrase = new Phrase("", normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableMargin.AddCell(cell);
                    oDoc.Add(tableMargin);                
                    #endregion

                    #region Facility
                    //Facility--------------------------------------------------------------------------------------------
                    
                    if (facilityName != "")
                    {
                        phrase = new Phrase(facilityName, normalBoldFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tableFacility.AddCell(cell);
                    }
                    if (facilityAddress != "")
                    {
                        phrase = new Phrase(facilityAddress, normalBoldFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tableFacility.AddCell(cell);
                    }
                    if (facilityAddress2 != "")
                    {
                        phrase = new Phrase(facilityAddress2, normalBoldFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tableFacility.AddCell(cell);
                    }
                    if (facilityContact != "")
                    {
                        phrase = new Phrase(facilityContact, normalBoldFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tableFacility.AddCell(cell);
                    }

                    oDoc.Add(tableFacility);
                    #endregion

                    #region Title
                    //Title-----------------------------------------------------------------------------------------------
                    phrase = new Phrase("\n" + titleIntro, H2);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableTitle.AddCell(cell);
                    oDoc.Add(tableTitle);
                    #endregion

                    #region Demographic
                    //Demographic-----------------------------------------------------------------------------------------



                    phrase = new Phrase("\n\n\n" + demographicMedicalSummary + "\n\n", normalFont);
                    cell = new Cell(phrase);
                    cell.Colspan = 2;
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase(demographicPatientIntro + demographicPatientName, normalBoldFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase(demographicDOBIntro + demographicBirthDate, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    phrase = new Phrase(demographicConsultIntro + demographicConsultationDate, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    tableDemographic.AddCell(cell);

                    /*
                    phrase = new Phrase(demographicStatusIntro + demographicStatusComplete, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    tableDemographic.AddCell(cell);
                    */
                  
                    phrase = new Phrase(complaintTitle + complaint, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableDemographic.AddCell(cell);

                    oDoc.Add(tableDemographic);
                    #endregion

                    #region Presenting Complaint
                    //Presenting Complaint--------------------------------------------------------------------------------
                    //row1------------------------------------------------------------------------------------------------
                    /*
                    phrase = new Phrase("\n" + complaintIntro, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableComplaint.AddCell(cell);
                    */

                    phrase = new Phrase(complaintNotes, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableComplaint.AddCell(cell);

                    oDoc.Add(tableComplaint);

                    #endregion

                    #region Measurement

                    //Measurement-----------------------------------------------------------------------------------------
                    //row1------------------------------------------------------------------------------------------------
                    phrase = new Phrase("\n" + measurementTitle, normalBoldFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 3;
                    tableMeasurement.AddCell(cell);
                    
                    //row2------------------------------------------------------------------------------------------------
                    phrase = new Phrase(measurementHeightIntro + measurementHeight + " " + patientHeightMeasurment, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableMeasurement.AddCell(cell);

                    phrase = new Phrase(measurementNeckIntro + decMeasurementNeck + " " + patientHeightMeasurment, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableMeasurement.AddCell(cell);

                    phrase = new Phrase(measurementPRIntro + measurementPR, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableMeasurement.AddCell(cell);

                    //row3------------------------------------------------------------------------------------------------
                    phrase = new Phrase(measurementWeightIntro + measurementWeight + " " + patientWeightMeasurment, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableMeasurement.AddCell(cell);

                    phrase = new Phrase(measurementWaistIntro + decMeasurementWaist + " " + patientHeightMeasurment, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableMeasurement.AddCell(cell);

                    phrase = new Phrase(measurementRRIntro + measurementRR, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableMeasurement.AddCell(cell);

                    //row4------------------------------------------------------------------------------------------------
                    phrase = new Phrase(measurementBMIIntro + measurementBMI, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableMeasurement.AddCell(cell);

                    phrase = new Phrase(measurementHipIntro + decMeasurementHip + " " + patientHeightMeasurment, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableMeasurement.AddCell(cell);

                    phrase = new Phrase(measurementBPIntro + measurementBPLower + " / " + measurementBPUpper, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableMeasurement.AddCell(cell);

                    //row5------------------------------------------------------------------------------------------------
                    phrase = new Phrase(measurementIWeightIntro + measurementIWeight + " " + patientWeightMeasurment, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableMeasurement.AddCell(cell);

                    phrase = new Phrase(measurementWHRIntro + decMeasurementWHR, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    tableMeasurement.AddCell(cell);

                    //row6------------------------------------------------------------------------------------------------
                    phrase = new Phrase(measurementEWeightIntro + measurementEWeight + " " + patientWeightMeasurment, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 3;
                    tableMeasurement.AddCell(cell);

                    oDoc.Add(tableMeasurement);
                    #endregion

                    #region Comorbidities
                    //Comorbidities-----------------------------------------------------------------------------------
                    phrase = new Phrase("\n" + comorbiditiesTitle, normalBoldFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableComorbidities.AddCell(cell);
                    
                    if (Request.Cookies["SubmitData"].Value.IndexOf("acs") >= 0)
                    {
                        comorbiditiesReview = "";
                        if (dsPatientComorbidity.Tables.Count > 0 && (dsPatientComorbidity.Tables[0].Rows.Count > 0))
                        {
                            for (int Idx = 0; Idx < comorbiditiesACSArr.Length / 3; Idx++)
                            {
                                introNotes = "";
                                comorbidities = comorbiditiesACSArr[Idx, 0];
                                comorbiditiesName = comorbiditiesACSArr[Idx, 1];

                                if (dsPatientComorbidity.Tables[0].Rows[0][comorbidities].ToString() != "")
                                {
                                    comorbiditiesRank = getDescription(dsPatientComorbidity.Tables[0].Rows[0][comorbidities].ToString(), "comRank");

                                    if (comorbiditiesRank != "0")
                                    {
                                        comorbiditiesExist = true;
                                        comorbiditiesChoice = getDescription(dsPatientComorbidity.Tables[0].Rows[0][comorbidities].ToString(), "com");

                                        if (comorbidities == "ACS_Hyper")
                                            introNotes = comorbiditiesMedsIntro;

                                        comorbiditiesNotes = dsPatientComorbidity.Tables[0].Rows[0][comorbidities + "_Notes"].ToString().Trim() == "" ? "" : ": " + introNotes + dsPatientComorbidity.Tables[0].Rows[0][comorbidities + "_Notes"].ToString().Trim();
                                        if (comorbiditiesName != "")
                                        {
                                            //display comorbidities choice instead of name if its mental health disorder
                                            if (comorbiditiesChoiceDisplay == comorbidities)
                                                comorbiditiesName = comorbiditiesChoice;

                                            phrase = new Phrase("- " + comorbiditiesName + comorbiditiesNotes, normalFont);
                                            cell = new Cell(phrase);
                                            cell.BorderWidth = 0;
                                            tableComorbiditiesResult.AddCell(cell);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (Request.Cookies["SubmitData"].Value.IndexOf("bold") >= 0)
                    {
                        if (dsPatientComorbidity.Tables.Count > 0 && (dsPatientComorbidity.Tables[0].Rows.Count > 0))
                        {
                            if (dsPatientComorbidity.Tables[0].Rows[0]["Comorbidity_Review"].ToString().Equals(Boolean.FalseString))
                                comorbiditiesReview = "";

                            for (int Idx = 0; Idx < comorbiditiesBoldArr.Length / 3; Idx++)
                            {
                                comorbidities = comorbiditiesBoldArr[Idx, 0];
                                comorbiditiesName = comorbiditiesBoldArr[Idx, 1];

                                if (dsPatientComorbidity.Tables[0].Rows[0][comorbidities].ToString() != "")
                                {
                                    comorbiditiesRank = getDescription(dsPatientComorbidity.Tables[0].Rows[0][comorbidities].ToString(), "comRank");

                                    if (comorbiditiesRank != "0")
                                    {
                                        comorbiditiesExist = true;
                                        comorbiditiesChoice = getDescription(dsPatientComorbidity.Tables[0].Rows[0][comorbidities].ToString(), "com");
                                        comorbiditiesNotes = dsPatientComorbidity.Tables[0].Rows[0][comorbidities + "_Notes"].ToString().Trim() == "" ? "" : ": " + dsPatientComorbidity.Tables[0].Rows[0][comorbidities + "_Notes"].ToString().Trim();
                                        if (comorbiditiesName != "")
                                        {
                                            //display comorbidities choice instead of name if its mental health disorder
                                            if (comorbiditiesChoiceDisplay == comorbidities)
                                                comorbiditiesName = comorbiditiesChoice;

                                            phrase = new Phrase("- " + comorbiditiesName + comorbiditiesNotes, normalFont);
                                            cell = new Cell(phrase);
                                            cell.BorderWidth = 0;
                                            tableComorbiditiesResult.AddCell(cell);
                                        }
                                    }
                                }
                            }

                            String[] extraComorbidities;
                            String[] extraComorbidity;
                            String comorbidityNotes;
                            if (dsPatientComorbidity.Tables[0].Rows[0]["Extra_Comorbidity"].ToString().Trim() != "")
                            {
                                extraComorbidities = dsPatientComorbidity.Tables[0].Rows[0]["Extra_Comorbidity"].ToString().Split('+');

                                foreach (string tempComorbidities in extraComorbidities)
                                {
                                    extraComorbidity = tempComorbidities.Split('*');
                                    comorbiditiesName = extraComorbidity[0];
                                    comorbiditiesNotes = extraComorbidity[1];

                                    if (comorbiditiesName != "")
                                    {
                                        comorbiditiesExist = true;

                                        phrase = new Phrase("", normalFont);
                                        cell = new Cell(phrase);
                                        cell.BorderWidth = 0;
                                        tableComorbiditiesResult.AddCell(cell);

                                        phrase = new Phrase("- " + comorbiditiesName + ": " + comorbiditiesNotes, normalFont);
                                        cell = new Cell(phrase);
                                        cell.BorderWidth = 0;
                                        tableComorbiditiesResult.AddCell(cell);
                                    }
                                }
                            }
                        }
                    }

                    if (Request.Cookies["SubmitData"].Value == "")
                    {
                        if (dsPatientComorbidity.Tables.Count > 0 && (dsPatientComorbidity.Tables[0].Rows.Count > 0))
                        {
                            for (int Idx = 0; Idx < comorbiditiesArr.Length / 3; Idx++)
                            {
                                comorbidities = comorbiditiesArr[Idx, 0];
                                comorbiditiesName = comorbiditiesArr[Idx, 1];

                                if (dsPatientComorbidity.Tables[0].Rows[0][comorbidities].ToString().Trim() != "")
                                {
                                    comorbiditiesExist = true;

                                    comorbiditiesNotes = ": " + dsPatientComorbidity.Tables[0].Rows[0][comorbidities].ToString().Trim();

                                    if (comorbiditiesName.IndexOf("OtherName") > -1)
                                    {
                                        comorbiditiesName = dsPatientComorbidity.Tables[0].Rows[0][comorbiditiesName].ToString().Trim();
                                    }

                                    phrase = new Phrase("- " + comorbiditiesName + comorbiditiesNotes, normalFont);
                                    cell = new Cell(phrase);
                                    cell.BorderWidth = 0;
                                    tableComorbiditiesResult.AddCell(cell);
                                }
                            }
                        }
                    }

                    if (comorbiditiesExist == true)
                    {
                        phrase = new Phrase(demographicGender + comorbiditiesData, normalFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tableComorbidities.AddCell(cell);
                    }
                    else
                    {
                        phrase = new Phrase(demographicGender + comorbiditiesNoData, normalFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tableComorbidities.AddCell(cell);
                    }


                    phrase = new Phrase(comorbiditiesReview, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableComorbiditiesResult.AddCell(cell);

                    oDoc.Add(tableComorbidities);
                    oDoc.Add(tableComorbiditiesResult);
                    #endregion

                    #region Medications
                    //Medications-----------------------------------------------------------------------------------
                    phrase = new Phrase("\n" + medicationTitle, normalBoldFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableMedication.AddCell(cell);

                    if (dsPatientEMR.Tables[0].Rows[0]["Medication"].ToString().Trim() != "")
                    {
                        medications = dsPatientEMR.Tables[0].Rows[0]["Medication"].ToString().Split('+');

                        foreach (string tempMedications in medications)
                        {
                            medication = tempMedications.Split('*');
                            medicationName = medication[0];
                            medicationDosage = medication[1].Trim() != "" ? ", " + medication[1].Trim() : "";
                            medicationFrequency = medication[2].Trim() != "" ? ", " + medication[2].Trim() : "";

                            if (medicationName != "")
                            {
                                phrase = new Phrase(medicationName + medicationDosage + medicationFrequency, normalFont);
                                cell = new Cell(phrase);
                                cell.BorderWidth = 0;
                                tableMedication.AddCell(cell);
                            }
                        }
                    }
                    else
                    {
                        phrase = new Phrase(demographicGender + medicationIntro, normalFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tableMedication.AddCell(cell);
                    }

                    oDoc.Add(tableMedication);
                    #endregion

                    #region Weight History
                    //Weight History-----------------------------------------------------------------------------------
                    //row1
                    if (weightHistoryExist == true)
                    {
                        phrase = new Phrase("\n" + weightHistoryTitle, normalBoldFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tableWeightHistory.AddCell(cell);

                        if (weightHistoryGain != "")
                        {
                            phrase = new Phrase(weightHistoryGain, normalFont);
                            cell = new Cell(phrase);
                            cell.BorderWidth = 0;
                            tableWeightHistory.AddCell(cell);
                        }

                        if (weightHistoryExist == true)
                            weightHistoryExistString = " " + demographicGender + demographicHasTried;

                        if (weightHistoryLoseYears != "" || weighHistoryTryMethodDisplay != "")
                        {
                            phrase = new Phrase(demographicGender + demographicTriedHowLong + weightHistoryLoseYears + weighHistoryTryMethodDisplay + "." + weightHistoryExistString, normalFont);
                            cell = new Cell(phrase);
                            cell.BorderWidth = 0;
                            tableWeightHistory.AddCell(cell);
                        }

                        if (weighHistoryGroupListDisplay != "")
                        {
                            phrase = new Phrase(demographicGroupListDisplayIntro + weighHistoryGroupListDisplay, normalFont);
                            cell = new Cell(phrase);
                            cell.BorderWidth = 0;
                            tableWeightHistory.AddCell(cell);
                        }

                        if (weighHistoryPillListDisplay != "")
                        {
                            phrase = new Phrase(demographicDietPillsListDisplayIntro + weighHistoryPillListDisplay, normalFont);
                            cell = new Cell(phrase);
                            cell.BorderWidth = 0;
                            tableWeightHistory.AddCell(cell);
                        }

                        if (weighHistoryAdviceListDisplay != "")
                        {
                            phrase = new Phrase(demographicAdviceListDisplayIntro + weighHistoryAdviceListDisplay, normalFont);
                            cell = new Cell(phrase);
                            cell.BorderWidth = 0;
                            tableWeightHistory.AddCell(cell);
                        }

                        if (weighHistoryDietListDisplay != "")
                        {
                            phrase = new Phrase(demographicDietListDisplayIntro + weighHistoryDietListDisplay, normalFont);
                            cell = new Cell(phrase);
                            cell.BorderWidth = 0;
                            tableWeightHistory.AddCell(cell);
                        }

                        if (weighHistoryOtherListDisplay != "")
                        {
                            phrase = new Phrase(demographicOtherListDisplayIntro + weighHistoryOtherListDisplay, normalFont);
                            cell = new Cell(phrase);
                            cell.BorderWidth = 0;
                            tableWeightHistory.AddCell(cell);
                        }

                        if (weighHistoryTreatmentListDisplay != "")
                        {
                            phrase = new Phrase(demographicTreatmentListDisplay + weighHistoryTreatmentListDisplay, normalFont);
                            cell = new Cell(phrase);
                            cell.BorderWidth = 0;
                            tableWeightHistory.AddCell(cell);
                        }

                        if (weighHistoryCosmeticProcedureDisplay != "")
                        {
                            phrase = new Phrase(demographicCosmeticProcedureDisplay + weighHistoryCosmeticProcedureDisplay, normalFont);
                            cell = new Cell(phrase);
                            cell.BorderWidth = 0;
                            tableWeightHistory.AddCell(cell);
                        }

                        oDoc.Add(tableWeightHistory);
                    }
                    #endregion

                    #region Background
                    //Background------------------------------------------------------------------------------------------
                    //row1------------------------------------------------------------------------------------------------
                    phrase = new Phrase("\n" + backgroundTitle, normalBoldFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableBackground.AddCell(cell);

                    //row2-----------------------------------------------------------------------------------------------
                    phrase = new Phrase(backgroundFamilyHistory, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableBackground.AddCell(cell);
                    
                    
                    for (int Idx = 0; Idx < backgroundHistory.Length / 2; Idx++)
                    {
                        familyBackground = "";
                        tempBackground = backgroundHistory[Idx, 0];
                        backgroundName = backgroundHistory[Idx, 1];
                        familyMembers = dsPatientEMR.Tables[0].Rows[0][tempBackground].ToString().Trim();
                        if (familyMembers != "")
                        {
                            familyMemberArr = familyMembers.Split(new char[] { '-' });

                            foreach (string familyMember in familyMemberArr)
                            {
                                tempMember = familyMember.Substring(familyMember.Length - 1, 1);
                                tempFamilyMember = getDescription(tempMember, "background");
                                if (tempFamilyMember != "")
                                familyBackground += tempFamilyMember + ", ";
                            }

                            if (familyBackground.Trim() != "")
                            {
                                backgroundCount++;
                                familyBackground = familyBackground.Substring(0, familyBackground.Length - 2);
                                
                                phrase = new Phrase(backgroundName + ": " + familyBackground, normalFont);
                                cell = new Cell(phrase);
                                cell.BorderWidth = 0;
                                tableBackground.AddCell(cell);
                            }
                        }
                    }

                    if (backgroundCount == 0 && backgroundFamilyHistory.Trim() == "")
                    {
                        phrase = new Phrase(backgroundNoHistory, normalFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tableBackground.AddCell(cell);
                        
                    }
                    
                    phrase = new Phrase("\n", normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableBackground.AddCell(cell);

                    oDoc.Add(tableBackground);
                    #endregion

                    #region Past History
                    //Past History------------------------------------------------------------------------------------------
                    
                    phrase = new Phrase(backgroundPastHealthTitle, normalBoldFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tablePastHistory.AddCell(cell);


                    if (backgroundPastHealth.Trim() != "")
                    {
                        phrase = new Phrase(backgroundPastHealth, normalFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tablePastHistory.AddCell(cell);
                    }

                    oDoc.Add(tablePastHistory);
                    #endregion

                    #region Previous Operation
                    //Previous Operation-----------------------------------------------------------------------------------       
                    if (Request.Cookies["SubmitData"].Value.IndexOf("bold") >= 0 || Request.Cookies["SubmitData"].Value == "")
                    {
                        operationPBSArr = operationPBS.Split(';');

                        if (operationPBS != "")
                        {
                            phrase = new Phrase(operationPBSIntro, normalFont);
                            cell = new Cell(phrase);
                            cell.BorderWidth = 0;
                            tablePreviousOperation.AddCell(cell);

                            foreach (string tempPBS in operationPBSArr)
                            {
                                if (tempPBS != "")
                                {
                                    phrase = new Phrase("- " + getDescription(tempPBS, "pbs"), normalFont);
                                    cell = new Cell(phrase);
                                    cell.BorderWidth = 0;
                                    tablePreviousOperation.AddCell(cell);
                                }
                            }
                        }
                    }
                    else if (Request.Cookies["SubmitData"].Value.IndexOf("acs") >= 0)
                    {
                        if (dsPatientEMR.Tables[0].Rows[0]["Background_PreviousBariatric"].ToString().Trim() != "")
                        {
                            phrase = new Phrase(operationPBSIntro, normalFont);
                            cell = new Cell(phrase);
                            cell.BorderWidth = 0;
                            tablePreviousOperation.AddCell(cell);

                            bariatrics = dsPatientEMR.Tables[0].Rows[0]["Background_PreviousBariatric"].ToString().Split('+');

                            foreach (string tempBariatric in bariatrics)
                            {
                                bariatric = tempBariatric.Split('*');

                                bariatricSurgeryYear = bariatric[0];
                                bariatricSurgeryName = bariatric[5];

                                if (bariatricSurgeryName.Trim() != "")
                                {
                                    phrase = new Phrase("- " + bariatricSurgeryYear + ", " + bariatricSurgeryName, normalFont);
                                    cell = new Cell(phrase);
                                    cell.BorderWidth = 0;
                                    tablePreviousOperation.AddCell(cell);
                                }
                            }
                        }
                    }

                    if (Request.Cookies["SubmitData"].Value.IndexOf("bold") >= 0 || Request.Cookies["SubmitData"].Value == "")
                    {
                        operationPBNSArr = operationPBNS.Split(';');

                        if (operationPBNS != "")
                        {
                            phrase = new Phrase(operationPNBSIntro, normalFont);
                            cell = new Cell(phrase);
                            cell.BorderWidth = 0;
                            tablePreviousOperation.AddCell(cell);

                            foreach (string tempPBNS in operationPBNSArr)
                            {
                                if (tempPBNS != "")
                                {
                                    phrase = new Phrase("- " + getDescription(tempPBNS, "pbns"), normalFont);
                                    cell = new Cell(phrase);
                                    cell.BorderWidth = 0;
                                    tablePreviousOperation.AddCell(cell);
                                }
                            }
                        }
                    }
                    else if (Request.Cookies["SubmitData"].Value.IndexOf("acs") >= 0)
                    {
                        if (dsPatientEMR.Tables[0].Rows[0]["Background_PreviousNonBariatric"].ToString().Trim() != "")
                        {
                            phrase = new Phrase(operationPNBSIntro, normalFont);
                            cell = new Cell(phrase);
                            cell.BorderWidth = 0;
                            tablePreviousOperation.AddCell(cell);

                            nonBariatrics = dsPatientEMR.Tables[0].Rows[0]["Background_PreviousNonBariatric"].ToString().Split('+');

                            foreach (string tempBariatric in nonBariatrics)
                            {
                                nonBariatricSurgeryName = tempBariatric;

                                if (nonBariatricSurgeryName != "")
                                {
                                    phrase = new Phrase("- " + nonBariatricSurgeryName, normalFont);
                                    cell = new Cell(phrase);
                                    cell.BorderWidth = 0;
                                    tablePreviousOperation.AddCell(cell);
                                }
                            }
                        }
                    }

                    if (backgroundPastHealth.Trim() == "" && operationPBS.Trim() == "" && operationPBNS.Trim() == "")
                    {
                        phrase = new Phrase(operationNoSurgery, normalFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tablePreviousOperation.AddCell(cell);
                    }
                    oDoc.Add(tablePreviousOperation);

                    #endregion

                    #region Allergies
                    //Allergies-------------------------------------------------------------------------------------------
                    //row1------------------------------------------------------------------------------------------------
                    phrase = new Phrase("\n" + allergiesTitle, normalBoldFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableAllergies.AddCell(cell);

                    //row2------------------------------------------------------------------------------------------------
                    if (allergiesHaveAllergy == true)
                    {
                        allergiesExist = true;
                        phrase = new Phrase(demographicGender + allergiesHaveAlleryIntro + allergiesListAllergy, normalFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tableAllergies.AddCell(cell);
                    }

                    //row3------------------------------------------------------------------------------------------------
                    if (allergiesHaveMedication == true)
                    {
                        allergiesExist = true;
                        phrase = new Phrase(demographicGender + allergiesMedicationIntro + allergiesListMedication, normalFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tableAllergies.AddCell(cell);
                    }

                    //row4------------------------------------------------------------------------------------------------
                    if (allergiesLatex == true)
                    {
                        allergiesExist = true;
                        phrase = new Phrase(demographicGender + allergiesLatexIntro, normalFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tableAllergies.AddCell(cell);
                    }

                    //row5------------------------------------------------------------------------------------------------
                    if (allergiesAnesthetic != "")
                    {
                        allergiesExist = true;
                        phrase = new Phrase(demographicGender + allergiesAnestheticIntro1 + demographicGender2 + allergiesAnestheticIntro2 + allergiesAnesthetic, normalFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tableAllergies.AddCell(cell);
                    }

                    //row6------------------------------------------------------------------------------------------------
                    if (allergiesExcessBleed == true)
                    {
                        allergiesExist = true;
                        phrase = new Phrase(demographicGender + allergiesExcessBleedIntro, normalFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tableAllergies.AddCell(cell);
                    }

                    //row7-----------------------------------------------------------------------------------------------
                    if (allergiesAnestheticRisk != "")
                    {
                        allergiesExist = true;
                        phrase = new Phrase(demographicGender + allergiesAnestheticRiskIntro + allergiesAnestheticRisk, normalFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tableAllergies.AddCell(cell);
                    }


                    if (allergiesExist == false)
                    {
                        phrase = new Phrase(demographicGender + allergiesRiskIntro, normalFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tableAllergies.AddCell(cell);
                    }

                    if (allergiesAnesthetic.Trim() == "" && allergiesAnestheticRisk.Trim() == "")
                    {
                        phrase = new Phrase(demographicGender + allergiesNoAnesthesia, normalFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tableAllergies.AddCell(cell);
                    }

                    oDoc.Add(tableAllergies);
                    #endregion

                    #region Physical Examination
                    //Physical Examination-----------------------------------------------------------------------------------
                    phrase = new Phrase("\n" + examinationTitle, normalBoldFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableExamination.AddCell(cell);

                    if (examinationNotes.Trim() != "")
                    {
                        phrase = new Phrase(examinationNotes + "\n", normalFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tableExamination.AddCell(cell);
                    }

                    for (int Idx = 0; Idx < examinationArr.Length; Idx++)
                    {
                        examination = examinationArr[Idx];
                        examinationResult = dsPatientEMR.Tables[0].Rows[0]["Exam_" + examination].ToString().Trim();
                        if (examinationResult != "")
                        {
                            phrase = new Phrase("- " + examinationResult, normalFont);
                            cell = new Cell(phrase);
                            cell.BorderWidth = 0;
                            tableExamination.AddCell(cell);
                        }
                    }
                    oDoc.Add(tableExamination);
                    #endregion
                    
                    #region Labs
                    //Labs------------------------------------------------------------------------------------------------
                    //row1------------------------------------------------------------------------------------------------
                    phrase = new Phrase("\n" + labsTitle, normalBoldFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableLabs.AddCell(cell);

                    phrase = new Phrase(labsNotes, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableLabs.AddCell(cell);

                    oDoc.Add(tableLabs);
                    #endregion

                    #region Investigations
                    //Investigations-----------------------------------------------------------------------------------
                    phrase = new Phrase("\n" + investigationTitle, normalBoldFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableInvestigation.AddCell(cell);

                    for (int Idx = 0; Idx < investigationArr.Length / 2; Idx++)
                    {
                        investigation = investigationArr[Idx, 0];
                        investigationSuggested = dsPatientEMR.Tables[0].Rows[0]["Investigation_" + investigation].ToString().Trim() == "" ? "" : ": " + dsPatientEMR.Tables[0].Rows[0]["Investigation_" + investigation].ToString().Trim();
                        investigationAction = dsPatientEMR.Tables[0].Rows[0]["Investigation_Action" + investigation].ToString().Trim();

                        if (investigationAction.ToLower() == "t")
                        {
                            investigationExist = true;

                            phrase = new Phrase("- " + investigationArr[Idx, 1] + investigationSuggested, normalFont);
                            cell = new Cell(phrase);
                            cell.BorderWidth = 0;
                            tableInvestigationResult.AddCell(cell);
                        }
                    }
                    #endregion

                    #region Special Investigations
                    //Special Investigations-----------------------------------------------------------------------------------
                    for (int Idx = 0; Idx < specInvestigationArr.Length / 2; Idx++)
                    {
                        specInvestigation = specInvestigationArr[Idx, 0];
                        specInvestigationSuggested = dsPatientEMR.Tables[0].Rows[0]["SpecialInvestigation_Suggested" + specInvestigation].ToString().Trim() == "" ? "" : ": " + dsPatientEMR.Tables[0].Rows[0]["SpecialInvestigation_Suggested" + specInvestigation].ToString().Trim();
                        specInvestigationAction = dsPatientEMR.Tables[0].Rows[0]["SpecialInvestigation_Action" + specInvestigation].ToString().Trim();

                        if (specInvestigationAction.ToLower() == "t")
                        {
                            investigationExist = true;

                            phrase = new Phrase("- " + specInvestigationArr[Idx, 1] + specInvestigationSuggested, normalFont);
                            cell = new Cell(phrase);
                            cell.BorderWidth = 0;
                            tableInvestigationResult.AddCell(cell);
                        }
                    }

                    if (investigationExist == true)
                    {
                        phrase = new Phrase(investigationTitle2, normalFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tableInvestigation.AddCell(cell);
                    }
                    else
                    {
                        phrase = new Phrase(investigationNoData, normalFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tableInvestigation.AddCell(cell);
                    }

                    oDoc.Add(tableInvestigation);
                    oDoc.Add(tableInvestigationResult);
                    #endregion

                    #region Referrals
                    //Referrals-----------------------------------------------------------------------------------
                    for (int Idx = 0; Idx < referralArr.Length / 2; Idx++)
                    {
                        referral = referralArr[Idx, 0];
                        referralResult = dsPatientEMR.Tables[0].Rows[0]["Referrals_" + referral].ToString().Trim() == "" ? "" : ": " + dsPatientEMR.Tables[0].Rows[0]["Referrals_" + referral].ToString().Trim();
                        if (dsPatientEMR.Tables[0].Rows[0]["Referrals_" + referral] != DBNull.Value)
                        {
                            referralExist = true;
                            referralResult = referralResult.Trim();
                            phrase = new Phrase("- " + referralArr[Idx, 1] + referralResult, normalFont);
                            cell = new Cell(phrase);
                            cell.BorderWidth = 0;
                            tableReferralResult.AddCell(cell);
                        }
                    }
                    if (referralExist == true)
                    {
                        phrase = new Phrase("\n" + referralTitle, normalFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tableReferral.AddCell(cell);
                    }
                    else
                    {
                        phrase = new Phrase("\n" + referralNoData, normalFont);
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tableReferral.AddCell(cell);
                    }

                    oDoc.Add(tableReferral);
                    oDoc.Add(tableReferralResult);
                    #endregion

                    #region Management Plan
                    //Management Plan-------------------------------------------------------------------------------------
                    //row1------------------------------------------------------------------------------------------------
                    phrase = new Phrase("\n" + managementTitle, normalBoldFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableManagement.AddCell(cell);
                    /*
                    phrase = new Phrase(managementIntro1, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableManagement.AddCell(cell);

                    phrase = new Phrase(managementIntro2, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableManagement.AddCell(cell);

                    phrase = new Phrase(managementIntro3, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableManagement.AddCell(cell);

                    phrase = new Phrase(managementIntro4, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableManagement.AddCell(cell);

                    phrase = new Phrase(managementIntro5, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableManagement.AddCell(cell);

                    phrase = new Phrase(managementIntro6, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableManagement.AddCell(cell);
                    */
                    //row2------------------------------------------------------------------------------------------------
                    phrase = new Phrase(management, normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableManagement.AddCell(cell);

                    oDoc.Add(tableManagement);
                    #endregion

                    #region Signature
                    //Signature ------------------------------------------------------------------------------------------
                    //row1------------------------------------------------------------------------------------------------
                    phrase = new Phrase("\n\n\n\n\n\n" + "____________________", normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    tableSignature.AddCell(cell);
                    
                    //row2------------------------------------------------------------------------------------------------
                    phrase = new Phrase(demographicDoctorName, normalBoldFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    tableSignature.AddCell(cell);
                    
                    phrase = new Phrase(signatureDegree, normalBoldFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    tableSignature.AddCell(cell);

                    phrase = new Phrase(signatureSpeciality, normalBoldFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    tableSignature.AddCell(cell);
                    
                    phrase = new Phrase(signatureAddress1, normalBoldFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    tableSignature.AddCell(cell);

                    phrase = new Phrase(signatureAddress2, normalBoldFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    tableSignature.AddCell(cell);

                    phrase = new Phrase(signatureAddress3, normalBoldFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    tableSignature.AddCell(cell);
                    
                    phrase = new Phrase("\n\n\n\n" + "____________________", normalFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    tableSignature.AddCell(cell);

                    phrase = new Phrase(signatureMedicalProvider, normalBoldFont);
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    tableSignature.AddCell(cell);

                    oDoc.Add(tableSignature);
                    #endregion

                    oDoc.Close();

                    Response.Redirect(openFilePath);
                }
                else
                {
                    string tempStr = "";
                    string url = "";
                    #region Logo
                    //Logo----------------------------------------------------------------------------------------------
                    if (LogoPath != "")
                    {
                        url = HttpContext.Current.Request.Url.AbsoluteUri;
                        url = url.Substring(0, url.IndexOf("?"));
                        strDoc += "<table><tr><td><img src='" + url + "/../.." + LogoPath + "'/></td></tr></table>";
                    }
                    #endregion

                    #region Margin
                    strDoc += "<table><tr><td style='margin-top:"+margin+"cm'>&nbsp;</td></tr></table>";
                    #endregion

                    #region Facility
                    strDoc += "<table>";
                    if (facilityName != "")
                    {
                        strDoc += "<tr><td style='font-weight:bold'>" + facilityName + "</td></tr>";
                    }
                    if (facilityAddress != "")
                    {
                        strDoc += "<tr><td style='font-weight:bold'>" + facilityAddress + "</td></tr>";
                    }
                    if (facilityAddress2 != "")
                    {
                        strDoc += "<tr><td style='font-weight:bold'>" + facilityAddress2 + "</td></tr>";
                    }
                    if (facilityContact != "")
                    {
                        strDoc += "<tr><td style='font-weight:bold'>" + facilityContact + "</td></tr>";
                    }
                    strDoc += "</table>";
                    #endregion

                    #region Title
                    //Title-----------------------------------------------------------------------------------------------
                    strDoc += "<table><tr><td style='width:600px; font-size:20;font-weight:bold' align='center'>" + titleIntro + "<br><br><br></td></tr></table>";
                    #endregion

                    #region Demographic
                    //Demographic-----------------------------------------------------------------------------------------------          
                    strDoc += "<table><tr><td colspan='2'>" + demographicMedicalSummary + "</td></tr>";
                    strDoc += "<tr><td colspan='2'>&nbsp;</td></tr>";          
                    strDoc += "<tr><td style='width:350px; font-weight:bold'>" + demographicPatientIntro + demographicPatientName + "</td>";
                    strDoc += "<td>"+demographicDOBIntro + demographicBirthDate+"</td></tr>";
                    strDoc += "<tr><td>" + demographicConsultIntro + demographicConsultationDate + "</td><td></td></tr>";
                    /*
                    strDoc += "<tr><td>" + demographicStatusIntro + demographicStatusComplete + "</td><td></td></tr>";
                    */ 
                    strDoc += "<tr><td>" + complaintTitle + complaint + "</td><td></td></tr></table>";
                    #endregion

                    #region Presenting Complaint
                    //Presenting Complaint--------------------------------------------------------------------------------
                    //strDoc += "<table><tr><td><br>" + complaintIntro + "</td></tr>";
                    strDoc += "<table><tr><td>" + complaintNotes + "</td></tr></table>";
                    #endregion
                    
                    #region Measurement
                    //Measurement-----------------------------------------------------------------------------------------
                    strDoc += "<table><tr><td colspan='3' style='font-weight:bold'><br><br>" + measurementTitle + "</td></tr>";
                    strDoc += "<tr><td style='width:200px;'>" + measurementHeightIntro + measurementHeight + " " + patientHeightMeasurment + "</td>";
                    strDoc += "<td style='width:200px;'>" + measurementNeckIntro + decMeasurementNeck + " " + patientHeightMeasurment + "</td>";
                    strDoc += "<td style='width:200px;'>"+measurementPRIntro + measurementPR+"</td></tr>";
                    strDoc += "<tr><td>"+measurementWeightIntro + measurementWeight + " " + patientWeightMeasurment+"</td><td>"+measurementWaistIntro + decMeasurementWaist + " " + patientHeightMeasurment+"</td><td>"+measurementRRIntro + measurementRR+"</td></tr>";
                    strDoc += "<tr><td>"+measurementBMIIntro + measurementBMI+"</td><td>"+measurementHipIntro + decMeasurementHip + " " + patientHeightMeasurment+"</td><td>"+measurementBPIntro + measurementBPLower + " / " + measurementBPUpper+"</td></tr>";
                    strDoc += "<tr><td>"+measurementIWeightIntro + measurementIWeight + " " + patientWeightMeasurment+"</td><td>"+measurementWHRIntro + decMeasurementWHR+"</td></tr>";
                    strDoc += "<tr><td>" + measurementEWeightIntro + measurementEWeight + " " + patientWeightMeasurment + "</td><td></td><td></td></tr></table>";
                    #endregion

                    #region Comorbidities
                    //Comorbidities-----------------------------------------------------------------------------------
                    tempStr = "";
                    strDoc += "<table><tr><td style='font-weight:bold'><br><br>" + comorbiditiesTitle + "</td></tr>";
                    
                    if (Request.Cookies["SubmitData"].Value.IndexOf("acs") >= 0)
                    {
                        comorbiditiesReview = "";
                        if (dsPatientComorbidity.Tables.Count > 0 && (dsPatientComorbidity.Tables[0].Rows.Count > 0))
                        {
                            for (int Idx = 0; Idx < comorbiditiesACSArr.Length / 3; Idx++)
                            {
                                introNotes = "";
                                comorbidities = comorbiditiesACSArr[Idx, 0];
                                comorbiditiesName = comorbiditiesACSArr[Idx, 1];

                                if (dsPatientComorbidity.Tables[0].Rows[0][comorbidities].ToString() != "")
                                {
                                    comorbiditiesRank = getDescription(dsPatientComorbidity.Tables[0].Rows[0][comorbidities].ToString(), "comRank");

                                    if (comorbiditiesRank != "0")
                                    {
                                        comorbiditiesExist = true;
                                        comorbiditiesChoice = getDescription(dsPatientComorbidity.Tables[0].Rows[0][comorbidities].ToString(), "com");

                                        if (comorbidities == "ACS_Hyper")
                                            introNotes = comorbiditiesMedsIntro;

                                        comorbiditiesNotes = dsPatientComorbidity.Tables[0].Rows[0][comorbidities + "_Notes"].ToString().Trim() == "" ? "" : ": " + introNotes + dsPatientComorbidity.Tables[0].Rows[0][comorbidities + "_Notes"].ToString().Trim();
                                        if (comorbiditiesName != "")
                                        {
                                            //display comorbidities choice instead of name if its mental health disorder
                                            if (comorbiditiesChoiceDisplay == comorbidities)
                                                comorbiditiesName = comorbiditiesChoice;

                                            tempStr += "<tr><td>" + "- " + comorbiditiesName + comorbiditiesNotes + "</td></tr>";
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (Request.Cookies["SubmitData"].Value.IndexOf("bold") >= 0)
                    {
                        if (dsPatientComorbidity.Tables.Count > 0 && (dsPatientComorbidity.Tables[0].Rows.Count > 0))
                        {
                            if (dsPatientComorbidity.Tables[0].Rows[0]["Comorbidity_Review"].ToString().Equals(Boolean.FalseString))
                                comorbiditiesReview = "";

                            for (int Idx = 0; Idx < comorbiditiesBoldArr.Length / 3; Idx++)
                            {
                                comorbidities = comorbiditiesBoldArr[Idx, 0];
                                comorbiditiesName = comorbiditiesBoldArr[Idx, 1];

                                if (dsPatientComorbidity.Tables[0].Rows[0][comorbidities].ToString() != "")
                                {
                                    comorbiditiesRank = getDescription(dsPatientComorbidity.Tables[0].Rows[0][comorbidities].ToString(), "comRank");

                                    if (comorbiditiesRank != "0")
                                    {
                                        comorbiditiesExist = true;
                                        comorbiditiesChoice = getDescription(dsPatientComorbidity.Tables[0].Rows[0][comorbidities].ToString(), "com");
                                        comorbiditiesNotes = dsPatientComorbidity.Tables[0].Rows[0][comorbidities + "_Notes"].ToString().Trim() == "" ? "" : ": " + dsPatientComorbidity.Tables[0].Rows[0][comorbidities + "_Notes"].ToString().Trim();
                                        if (comorbiditiesName != "")
                                        {
                                            //display comorbidities choice instead of name if its mental health disorder
                                            if (comorbiditiesChoiceDisplay == comorbidities)
                                                comorbiditiesName = comorbiditiesChoice;

                                            tempStr += "<tr><td>" + "- " + comorbiditiesName + comorbiditiesNotes + "</td></tr>";
                                        }
                                    }
                                }
                            }

                            String[] extraComorbidities;
                            String[] extraComorbidity;
                            String comorbidityNotes;
                            if (dsPatientComorbidity.Tables[0].Rows[0]["Extra_Comorbidity"].ToString().Trim() != "")
                            {
                                extraComorbidities = dsPatientComorbidity.Tables[0].Rows[0]["Extra_Comorbidity"].ToString().Split('+');

                                foreach (string tempComorbidities in extraComorbidities)
                                {
                                    extraComorbidity = tempComorbidities.Split('*');
                                    comorbiditiesName = extraComorbidity[0];
                                    comorbiditiesNotes = extraComorbidity[1];

                                    if (comorbiditiesName != "")
                                    {
                                        comorbiditiesExist = true;
                                        tempStr += "<tr><td>" + "- " + comorbiditiesName + ": " + comorbiditiesNotes + "</td></tr>";
                                    }
                                }
                            }
                        }
                    }

                    if (Request.Cookies["SubmitData"].Value == "")
                    {
                        if (dsPatientComorbidity.Tables.Count > 0 && (dsPatientComorbidity.Tables[0].Rows.Count > 0))
                        {
                            for (int Idx = 0; Idx < comorbiditiesArr.Length / 3; Idx++)
                            {
                                comorbidities = comorbiditiesArr[Idx, 0];
                                comorbiditiesName = comorbiditiesArr[Idx, 1];

                                if (dsPatientComorbidity.Tables[0].Rows[0][comorbidities].ToString().Trim() != "")
                                {
                                    comorbiditiesExist = true;

                                    comorbiditiesNotes = ": " + dsPatientComorbidity.Tables[0].Rows[0][comorbidities].ToString().Trim();

                                    if (comorbiditiesName.IndexOf("OtherName") > -1)
                                    {
                                        comorbiditiesName = dsPatientComorbidity.Tables[0].Rows[0][comorbiditiesName].ToString().Trim();
                                    }

                                    tempStr += "<tr><td>" + "- " + comorbiditiesName + comorbiditiesNotes + "</td></tr>";
                                }
                            }
                        }
                    }

                    if (comorbiditiesExist == true)
                        strDoc += "<tr><td>" + demographicGender + comorbiditiesData + "</td></tr>";
                    else
                        strDoc += "<tr><td>" + demographicGender + comorbiditiesNoData + "</td></tr>";

                    tempStr += "<tr><td>" + comorbiditiesReview + "</td></tr>";

                    strDoc += tempStr + "</table>";

                    #endregion

                    #region Medications
                    //Medications-----------------------------------------------------------------------------------

                    strDoc += "<table><tr><td style='font-weight:bold'><br><br>" + medicationTitle + "</td></tr>";

                    if (dsPatientEMR.Tables[0].Rows[0]["Medication"].ToString().Trim() != "")
                    {
                        medications = dsPatientEMR.Tables[0].Rows[0]["Medication"].ToString().Split('+');

                        foreach (string tempMedications in medications)
                        {
                            medication = tempMedications.Split('*');
                            medicationName = medication[0];
                            medicationDosage = medication[1].Trim() != "" ? ", " + medication[1].Trim() : "";
                            medicationFrequency = medication[2].Trim() != "" ? ", " + medication[2].Trim() : "";

                            if (medicationName != "")
                            {
                                strDoc += "<tr><td>" + medicationName + medicationDosage + medicationFrequency + "</td></tr>";
                            }
                        }
                    }
                    else
                    {
                        strDoc += "<tr><td>" + demographicGender + medicationIntro + "</td></tr>";
                    }
                    strDoc += "</table>";
                    #endregion
                    
                    #region Weight History
                    //Weight History-----------------------------------------------------------------------------------
                    if (weightHistoryExist == true)
                    {
                        strDoc += "<table><tr><td style='font-weight:bold'><br><br>" + weightHistoryTitle + "</td></tr>";
                        if (weightHistoryGain != "")
                            strDoc += "<tr><td>" + weightHistoryGain + "</td></tr>";

                        if (weightHistoryExist == true)
                            weightHistoryExistString = " " + demographicGender + demographicHasTried;

                        if (weightHistoryLoseYears != "" || weighHistoryTryMethodDisplay != "")
                            strDoc += "<tr><td>" + demographicGender + demographicTriedHowLong + weightHistoryLoseYears + weighHistoryTryMethodDisplay + "." + weightHistoryExistString + "</td></tr>";

                        if (weighHistoryGroupListDisplay != "")
                            strDoc += "<tr><td>" + demographicGroupListDisplayIntro + weighHistoryGroupListDisplay + "</td></tr>";

                        if (weighHistoryPillListDisplay != "")
                            strDoc += "<tr><td>" + demographicDietPillsListDisplayIntro + weighHistoryPillListDisplay + "</td></tr>";

                        if (weighHistoryAdviceListDisplay != "")
                            strDoc += "<tr><td>" + demographicAdviceListDisplayIntro + weighHistoryAdviceListDisplay + "</td></tr>";

                        if (weighHistoryDietListDisplay != "")
                            strDoc += "<tr><td>" + demographicDietListDisplayIntro + weighHistoryDietListDisplay + "</td></tr>";

                        if (weighHistoryOtherListDisplay != "")
                            strDoc += "<tr><td>" + demographicOtherListDisplayIntro + weighHistoryOtherListDisplay + "</td></tr>";

                        if (weighHistoryTreatmentListDisplay != "")
                            strDoc += "<tr><td>" + demographicTreatmentListDisplay + weighHistoryTreatmentListDisplay + "</td></tr>";

                        if (weighHistoryCosmeticProcedureDisplay != "")
                            strDoc += "<tr><td>" + demographicCosmeticProcedureDisplay + weighHistoryCosmeticProcedureDisplay + "</td></tr>";

                        strDoc += "</table>";
                    }
                    #endregion

                    #region Background
                    //Background------------------------------------------------------------------------------------------
                    strDoc += "<table><tr><td style='font-weight:bold'><br><br>" + backgroundTitle + "</td></tr>";

                    //row2-----------------------------------------------------------------------------------------------
                    strDoc += "<tr><td>" + backgroundFamilyHistory + "</td></tr>";
                    
                    for (int Idx = 0; Idx < backgroundHistory.Length / 2; Idx++)
                    {
                        familyBackground = "";
                        tempBackground = backgroundHistory[Idx, 0];
                        backgroundName = backgroundHistory[Idx, 1];
                        familyMembers = dsPatientEMR.Tables[0].Rows[0][tempBackground].ToString().Trim();
                        if (familyMembers != "")
                        {
                            familyMemberArr = familyMembers.Split(new char[] { '-' });

                            foreach (string familyMember in familyMemberArr)
                            {
                                tempMember = familyMember.Substring(familyMember.Length - 1, 1);
                                tempFamilyMember = getDescription(tempMember, "background");
                                if (tempFamilyMember != "")
                                    familyBackground += tempFamilyMember + ", ";
                            }

                            if (familyBackground.Trim() != "")
                            {
                                backgroundCount++;
                                familyBackground = familyBackground.Substring(0, familyBackground.Length - 2);

                                strDoc += "<tr><td>" + backgroundName + ": " + familyBackground + "</td></tr>";
                            }
                        }
                    }

                    if (backgroundCount == 0 && backgroundFamilyHistory.Trim() == "")
                    {
                        strDoc += "<tr><td>" + backgroundNoHistory + "</td></tr>";
                    }

                    strDoc += "<tr><td>&nbsp;</td></tr>";
                    strDoc += "</table>";
                    #endregion

                    #region Past History
                    //Past History------------------------------------------------------------------------------------------
                    strDoc += "<table><tr><td style='font-weight:bold'>" + backgroundPastHealthTitle + "</td></tr>";

                    if (backgroundPastHealth.Trim() != "")
                    {
                        strDoc += "<tr><td>" + backgroundPastHealth + "</td></tr>";
                    }

                    strDoc += "<tr><td>&nbsp;</td></tr></table>";
                    #endregion

                    #region Previous Operation
                    //Previous Operation-----------------------------------------------------------------------------------      
                    strDoc += "<table>";
                    if (Request.Cookies["SubmitData"].Value.IndexOf("bold") >= 0 || Request.Cookies["SubmitData"].Value == "")
                    {
                        operationPBSArr = operationPBS.Split(';');

                        if (operationPBS != "")
                        {
                            strDoc += "<tr><td>" + operationPBSIntro + "</td></tr>";

                            foreach (string tempPBS in operationPBSArr)
                            {
                                if (tempPBS != "")
                                {
                                    strDoc += "<tr><td>" + "- " + getDescription(tempPBS, "pbs") + "</td></tr>";
                                }
                            }
                        }
                    }
                    else if (Request.Cookies["SubmitData"].Value.IndexOf("acs") >= 0)
                    {
                        if (dsPatientEMR.Tables[0].Rows[0]["Background_PreviousBariatric"].ToString().Trim() != "")
                        {
                            strDoc += "<tr><td>" + operationPBSIntro + "</td></tr>";
                            bariatrics = dsPatientEMR.Tables[0].Rows[0]["Background_PreviousBariatric"].ToString().Split('+');

                            foreach (string tempBariatric in bariatrics)
                            {
                                bariatric = tempBariatric.Split('*');

                                bariatricSurgeryYear = bariatric[0];
                                bariatricSurgeryName = bariatric[5];

                                if (bariatricSurgeryName.Trim() != "")
                                {
                                    strDoc += "<tr><td>" + "- " + bariatricSurgeryYear + ", " + bariatricSurgeryName + "</td></tr>";
                                }
                            }
                        }
                    }

                    if (Request.Cookies["SubmitData"].Value.IndexOf("bold") >= 0 || Request.Cookies["SubmitData"].Value == "")
                    {
                        operationPBNSArr = operationPBNS.Split(';');

                        if (operationPBNS != "")
                        {
                            strDoc += "<tr><td>" + operationPNBSIntro + "</td></tr>";

                            foreach (string tempPBNS in operationPBNSArr)
                            {
                                if (tempPBNS != "")
                                {
                                    strDoc += "<tr><td>" + "- " + getDescription(tempPBNS, "pbns") + "</td></tr>";
                                }
                            }
                        }
                    }
                    else if (Request.Cookies["SubmitData"].Value.IndexOf("acs") >= 0)
                    {
                        if (dsPatientEMR.Tables[0].Rows[0]["Background_PreviousNonBariatric"].ToString().Trim() != "")
                        {
                            strDoc += "<tr><td>" + operationPNBSIntro + "</td></tr>";

                            nonBariatrics = dsPatientEMR.Tables[0].Rows[0]["Background_PreviousNonBariatric"].ToString().Split('+');

                            foreach (string tempBariatric in nonBariatrics)
                            {
                                nonBariatricSurgeryName = tempBariatric;

                                if (nonBariatricSurgeryName != "")
                                {
                                    strDoc += "<tr><td>" + "- " + nonBariatricSurgeryName + "</td></tr>";
                                }
                            }
                        }
                    }

                    if (backgroundPastHealth.Trim() == "" && operationPBS.Trim() == "" && operationPBS.Trim() == "")
                    {
                        strDoc += "<tr><td>" + operationNoSurgery + "</td></tr>";
                    }
                    strDoc += "</table>";

                    #endregion

                    #region Allergies
                    //Allergies-------------------------------------------------------------------------------------------

                    strDoc += "<table><tr><td style='font-weight:bold'><br><br>" + allergiesTitle + "</td></tr>";
                    //row2------------------------------------------------------------------------------------------------
                    if (allergiesHaveAllergy == true)
                    {
                        allergiesExist = true;
                        strDoc += "<tr><td>" + demographicGender + allergiesHaveAlleryIntro + allergiesListAllergy + "</td></tr>";
                    }

                    //row3------------------------------------------------------------------------------------------------
                    if (allergiesHaveMedication == true)
                    {
                        allergiesExist = true;
                        strDoc += "<tr><td>" + demographicGender + allergiesMedicationIntro + allergiesListMedication + "</td></tr>";
                    }

                    //row4------------------------------------------------------------------------------------------------
                    if (allergiesLatex == true)
                    {
                        allergiesExist = true;
                        strDoc += "<tr><td>" + demographicGender + allergiesLatexIntro +"</td></tr>";
                    }

                    //row5------------------------------------------------------------------------------------------------
                    if (allergiesAnesthetic != "")
                    {
                        allergiesExist = true;
                        strDoc += "<tr><td>" + demographicGender + allergiesAnestheticIntro1 + demographicGender2 + allergiesAnestheticIntro2 + allergiesAnesthetic +"</td></tr>";
                    }

                    //row6------------------------------------------------------------------------------------------------
                    if (allergiesExcessBleed == true)
                    {
                        allergiesExist = true;
                        strDoc += "<tr><td>" + demographicGender + allergiesExcessBleedIntro + "</td></tr>";
                    }

                    //row7-----------------------------------------------------------------------------------------------
                    if (allergiesAnestheticRisk != "")
                    {
                        allergiesExist = true;
                        strDoc += "<tr><td>" + demographicGender + allergiesAnestheticRiskIntro + allergiesAnestheticRisk +"</td></tr>";
                    }

                    if (allergiesExist == false)
                    {
                        strDoc += "<tr><td>" + demographicGender + allergiesRiskIntro +"</td></tr>";
                    }

                    if (allergiesAnesthetic.Trim() == "" && allergiesAnestheticRisk.Trim() == "")
                    {
                        strDoc += "<tr><td>" + demographicGender + allergiesNoAnesthesia + "</td></tr>";
                    }

                    strDoc += "</table>";
                    #endregion

                    #region Physical Examination
                    //Physical Examination-----------------------------------------------------------------------------------
                    strDoc += "<table><tr><td style='font-weight:bold'><br><br>" + examinationTitle + "</td></tr>";
                    strDoc += "<tr><td>"+examinationNotes+"</td></tr>";

                    for (int Idx = 0; Idx < examinationArr.Length; Idx++)
                    {
                        examination = examinationArr[Idx];
                        examinationResult = dsPatientEMR.Tables[0].Rows[0]["Exam_" + examination].ToString().Trim();
                        if (examinationResult != "")
                        {
                            strDoc += "<tr><td>" + examinationResult + "</td></tr>";
                        }
                    }

                    strDoc += "</table>";

                    #endregion

                    #region Labs
                    //Labs--------------------------------------------------------------------------------------------------
                    strDoc += "<table><tr><td style='font-weight:bold'><br><br>" + labsTitle + "</td></tr>";
                    strDoc += "<tr><td>" + labsNotes + "</td></tr></table>";
                    #endregion

                    #region Investigations
                    //Investigations-----------------------------------------------------------------------------------
                    strDoc += "<table><tr><td style='font-weight:bold'><br><br>" + investigationTitle + "</td></tr>";
                    tempStr = "";

                    for (int Idx = 0; Idx < investigationArr.Length / 2; Idx++)
                    {
                        investigation = investigationArr[Idx, 0];
                        investigationSuggested = dsPatientEMR.Tables[0].Rows[0]["Investigation_" + investigation].ToString().Trim() == "" ? "" : ": " + dsPatientEMR.Tables[0].Rows[0]["Investigation_" + investigation].ToString().Trim();
                        investigationAction = dsPatientEMR.Tables[0].Rows[0]["Investigation_Action" + investigation].ToString().Trim();

                        if (investigationAction.ToLower() == "t")
                        {
                            investigationExist = true;
                            tempStr += "<tr><td>" + "- " + investigationArr[Idx, 1] + investigationSuggested + "</td></tr>";
                        }
                    }
                    #endregion

                    #region Special Investigations
                    //Special Investigations-----------------------------------------------------------------------------------
                    for (int Idx = 0; Idx < specInvestigationArr.Length / 2; Idx++)
                    {
                        specInvestigation = specInvestigationArr[Idx, 0];
                        specInvestigationSuggested = dsPatientEMR.Tables[0].Rows[0]["SpecialInvestigation_Suggested" + specInvestigation].ToString().Trim() == "" ? "" : ": " + dsPatientEMR.Tables[0].Rows[0]["SpecialInvestigation_Suggested" + specInvestigation].ToString().Trim();
                        specInvestigationAction = dsPatientEMR.Tables[0].Rows[0]["SpecialInvestigation_Action" + specInvestigation].ToString().Trim();

                        if (specInvestigationAction.ToLower() == "t")
                        {
                            investigationExist = true;
                            tempStr += "<tr><td>" + "- " + specInvestigationArr[Idx, 1] + specInvestigationSuggested + "</td></tr>";
                        }
                    }

                    if (investigationExist == true)
                    {
                        strDoc += "<tr><td>" + investigationTitle2 + "</td></tr>";
                    }
                    else
                    {
                        strDoc += "<tr><td>" + investigationNoData + "</td></tr>";
                    }
                    strDoc += tempStr + "</table>";
                    #endregion

                    #region Referrals
                    //Referrals-----------------------------------------------------------------------------------
                    tempStr = "";
                    strDoc += "<table>";
                    for (int Idx = 0; Idx < referralArr.Length / 2; Idx++)
                    {
                        referral = referralArr[Idx, 0];
                        referralResult = dsPatientEMR.Tables[0].Rows[0]["Referrals_" + referral].ToString().Trim() == "" ? "" : ": " + dsPatientEMR.Tables[0].Rows[0]["Referrals_" + referral].ToString().Trim();
                        if (dsPatientEMR.Tables[0].Rows[0]["Referrals_" + referral] != DBNull.Value)
                        {
                            referralExist = true;
                            referralResult = referralResult.Trim();

                            tempStr += "<tr><td>"+"- " + referralArr[Idx, 1] + referralResult+"</td></tr>";
                        }
                    }

                    if (referralExist == true)
                        strDoc += "<tr><td>" + referralTitle + "</td></tr>";
                    else
                        strDoc += "<tr><td>" + referralNoData + "</td></tr>";

                    strDoc += tempStr + "</table>";
                    #endregion

                    #region Management Plan
                    //Management Plan-------------------------------------------------------------------------------------
                    strDoc += "<table><tr><td style='font-weight:bold'><br><br>" + managementTitle + "</td></tr>";
                    /*
                    strDoc += "<tr><td>" + managementIntro1 + "</td></tr>";
                    strDoc += "<tr><td>" + managementIntro2 + "</td></tr>";
                    strDoc += "<tr><td>" + managementIntro3 + "</td></tr>";
                    strDoc += "<tr><td>" + managementIntro4 + "</td></tr>";
                    strDoc += "<tr><td>" + managementIntro5 + "</td></tr>";
                    strDoc += "<tr><td>" + managementIntro6 + "</td></tr>";       
                     */
                    strDoc += "<tr><td><br>" + management + "<br><br><br><br><br><br></td></tr></table>";
                    #endregion
                    
                    #region Signature
                    //Signature ------------------------------------------------------------------------------------------
                    strDoc += "<table><tr><td style='width:500px; font-weight:bold'>" + "____________________" + "</td>";
                    strDoc += "<td style='width:100px;'></td></tr>";
                    strDoc += "<tr><td style='font-weight:bold' colspan='2'>" + demographicDoctorName + "</td>";
                    strDoc += "<tr><td style='font-weight:bold' colspan='2'>" + signatureDegree + "</td></tr>";
                    strDoc += "<tr><td style='font-weight:bold' colspan='2'>" + signatureSpeciality + "</td></tr>";
                    strDoc += "<tr><td style='font-weight:bold' colspan='2'>" + signatureAddress1 + "</td></tr>";
                    strDoc += "<tr><td style='font-weight:bold' colspan='2'>" + signatureAddress2 + "</td></tr>";
                    strDoc += "<tr><td style='font-weight:bold' colspan='2'>" + signatureAddress3 + "</td></tr>";

                    strDoc += "<tr><td style='font-weight:bold' colspan='2'><br><br><br><br>____________________</td></tr>";
                    strDoc += "<tr><td style='font-weight:bold' colspan='2'>" + signatureMedicalProvider + "</td></tr></table>";
                    #endregion

                    //Response.AppendHeader("Content-Type", "application/msword");
                    //Response.AppendHeader("Content-disposition", "attachment; filename=" + fileName + "." + reportFormat);
                    //Response.Write(strDoc);

                    StringBuilder sbTop = new System.Text.StringBuilder();
                    sbTop.Append(@"
                    <html 
                    xmlns:o='urn:schemas-microsoft-com:office:office' 
                    xmlns:w='urn:schemas-microsoft-com:office:word'
                    xmlns='http://www.w3.org/TR/REC-html40'>
                    <head><title></title>

                    <!--[if gte mso 9]>
                    <xml>
                    <w:WordDocument>
                    <w:View>Print</w:View>
                    <w:Zoom>90</w:Zoom>
                    <w:DoNotOptimizeForBrowser/>
                    </w:WordDocument>
                    </xml>
                    <![endif]-->


                    <style>
                    p.MsoFooter, li.MsoFooter, div.MsoFooter
                    {
                    margin:0in;
                    margin-bottom:.0001pt;
                    mso-pagination:widow-orphan;
                    tab-stops:center 3.0in right 6.0in;
                    font-size:12.0pt;
                    }
                    <style>

                    <!-- /* Style Definitions */

                    @page Section1
                    {
                    margin:1.0in 0.5in 1.0in 0.5in ;
                    mso-header-margin:.5in;
                    mso-header:h1;
                    mso-footer: f1; 
                    mso-footer-margin:.5in;
                    }


                    div.Section1
                    {
                    page:Section1;
                    }

                    table#hrdftrtbl
                    {
                    margin:0in 0in 0in 9in;
                    }
                    -->
                    </style></head>

                    <body lang=EN-US style='tab-interval:.3in'>");
                  
                    sbTop.Append(strDoc);

                    sbTop.Append(@"
                    <div class=Section1>
                    <tr>
                    <td>
                    <div style='mso-element:footer' id=f1>
                    <p class=MsoFooter>");
                    //sbTop.Append(@"<span>" + demographicPatientNameFooter + "</span><span style='mso-tab-count:1'>&nbsp;</span>" + demographicConsutlationComplete+"<span style='mso-tab-count:1'>&nbsp;</span>page: <span style='mso-field-code: PAGE'></span>of <span style='mso-field-code: NUMPAGES '></span>");
                    sbTop.Append(@"<table width='100%'><tr><td width='40%' height='20'>" + demographicPatientNameFooter + "&nbsp;</td><td width='35%' align='center'>" + demographicConsutlationComplete + "&nbsp;</td><td width='25%' align='right'>page: <span style='mso-field-code: PAGE'></span>of <span style='mso-field-code: NUMPAGES '></span>&nbsp;</td></tr></table>");
                    sbTop.Append(@"</p></div></td></tr>
                    </div>
                    </body></html>
                    ");

                    string strBody = sbTop.ToString();
                    Response.AppendHeader("Content-Type", "application/msword");
                    Response.AppendHeader("Content-disposition", "attachment; filename=" + fileName + "." + reportFormat);
                    Response.Write(strBody);
                }
            }
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Patient Data Form", "Loading Patient Data - LoadPatientData function", err.ToString());
        }
    }
    #endregion

    #region Visit List Report with RDL Builder
    #region private void RepVisitList_BuildReport()
    private void RepVisitList_BuildReport()
    {
        DataSet dsReport;
        SqlCommand cmdSelect = new SqlCommand();

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_ConsultFU1_LoadAllConsult", true);

        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);

        dsReport = gClass.FetchData(cmdSelect, "tblVisitList");

        DataTable dtTemp = dsReport.Tables["tblVisitList"];
        gClass.AddColumn(ref dtTemp, "ReportDate", "System.String", DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToShortTimeString());

        dsReport.AcceptChanges();
        CreateOutputFile("REPVISITLIST", dsReport);
    }
    #endregion
    #region private void RepVisitList_RDL(DataSet dsReport, string strRDLFilename, string strLanguage)
    private void RepVisitList_RDL(DataSet dsReport, string strRDLFilename, string strLanguage)
    {
        System.IO.FileStream stream = System.IO.File.OpenWrite(strRDLFilename);
        System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(stream, System.Text.Encoding.UTF8);

        writer.Formatting = System.Xml.Formatting.Indented;
        writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"utf-8\"");
        #region Report
        writer.WriteStartElement("Report");

        AddReportConfiguration(ref writer, "Visit List", strLanguage, (Decimal)29, (Decimal)0.5, (Decimal)0.5, (Decimal)0.5, (Decimal)0.5, (Decimal)21, "cm");
        AddDataSource(ref writer, dsReport);
        AddDataSets(ref writer, dsReport);

        #region Body
        writer.WriteStartElement("Body");
        writer.WriteElementString("Height", "20cm"); //Report.height - (TopMargin + bottomMargin) == 29.7 - (0.5+0.5)
        writer.WriteStartElement("ReportItems");

        #region Table
        writer.WriteStartElement("Table");
        writer.WriteAttributeString("Name", "rptVisitList");

        #region TableColumns
        writer.WriteStartElement("TableColumns");
        AddColumn(ref writer, "2cm");
        AddColumn(ref writer, "0.75cm");
        AddColumn(ref writer, "4cm");
        AddColumn(ref writer, "5cm");
        AddColumn(ref writer, "3.25cm");
        AddColumn(ref writer, "2.5cm");
        AddColumn(ref writer, "6cm");
        
        writer.WriteEndElement();
        #endregion

        #region Header
        writer.WriteStartElement("Header");
        writer.WriteElementString("RepeatOnNewPage", "true");

        #region TableRows
        writer.WriteStartElement("TableRows");

        #region Row6
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");

        AddCell(ref writer, "Textbox", "txtPatientID_hr", "ID", DetailCellStyle("Blue", "", "", "LightGrey"), "2");
        AddCell(ref writer, "Textbox", "txtPatientFirstname_hr", "First Name", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtPatientLastname_hr", "Surname", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtPatientVisitDate_hr", "Visit Date", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtPatientWeight_hr", "Weight", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtPatientSeenBy_hr", "Seen By", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        writer.WriteEndElement();
        #endregion
        writer.WriteEndElement();
        #endregion

        #region Deatails
        writer.WriteStartElement("Details");
        writer.WriteStartElement("TableRows");
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.75cm");
        writer.WriteStartElement("TableCells");

        AddCell(ref writer, "Textbox", "txtPatientID", "=Fields!PatientID.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtPatientTitle", "=Fields!PatientTitle.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtPatientFirstname", "=Fields!Firstname.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtPatientLastname", "=Fields!Surname.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtPatientVisitDate", "=Fields!strDateSeen.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtPatientWeight", "=Fields!BMIWeight.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtPatientSeenBy", "=Fields!DoctorName.Value", DetailCellStyle("", "", "", ""), "");
        
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        writer.WriteEndElement();
        #endregion

        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        writer.WriteEndElement();
        #endregion
        writer.Flush();
        stream.Close();
    }
    #endregion
    #endregion

    #region Patient List Report with RDL Builder
    #region private void RepPatientList_BuildReport()
    private void RepPatientList_BuildReport()
    {
        DataSet dsReport;
        SqlCommand cmdSelect = new SqlCommand();

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_Rep_PatientList", true);

        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSelect.Parameters.Add("@SurgeonId", SqlDbType.Int).Value = 0;
        cmdSelect.Parameters.Add("@HospitalCode", SqlDbType.VarChar, 6).Value = "";
        cmdSelect.Parameters.Add("@RegionId", SqlDbType.VarChar, 6).Value = "";

        cmdSelect.Parameters.Add("@FDate", SqlDbType.VarChar).Value = "";
        cmdSelect.Parameters.Add("@TDate", SqlDbType.VarChar).Value = "";

        cmdSelect.Parameters.Add("@FAge", SqlDbType.Int).Value = 0;
        cmdSelect.Parameters.Add("@TAge", SqlDbType.Int).Value = 0;
        cmdSelect.Parameters.Add("@FBMI", SqlDbType.Int).Value = 0;
        cmdSelect.Parameters.Add("@TBMI", SqlDbType.Int).Value = 0;

        cmdSelect.Parameters.Add("@GroupCode", SqlDbType.VarChar, 10).Value = "";
        cmdSelect.Parameters.Add("@SurgeryType", SqlDbType.VarChar, 10).Value = "";
        cmdSelect.Parameters.Add("@BandType", SqlDbType.VarChar, 10).Value = "";

        cmdSelect.Parameters.Add("@Approach", SqlDbType.VarChar, 10).Value = "";
        cmdSelect.Parameters.Add("@Category", SqlDbType.VarChar, 10).Value = "";
        cmdSelect.Parameters.Add("@BandSize", SqlDbType.VarChar, 10).Value = "";
        cmdSelect.Parameters.Add("@SerialNo", SqlDbType.VarChar, 10).Value = "";

        cmdSelect.Parameters.Add("@MonthsFollowUp", SqlDbType.Int).Value = 0;

        dsReport = gClass.FetchData(cmdSelect, "tblPatientList");

        DataTable dtTemp = dsReport.Tables["tblPatientList"];
        gClass.AddColumn(ref dtTemp, "ReportDate", "System.String", DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToShortTimeString());

        dsReport.AcceptChanges();
        CreateOutputFile("REPPATIENTLIST", dsReport);
    }
    #endregion

    #region private void RepPatientList_RDL(DataSet dsReport, string strRDLFilename, string strLanguage)
    private void RepPatientList_RDL(DataSet dsReport, string strRDLFilename, string strLanguage)
    {
        System.IO.FileStream stream = System.IO.File.OpenWrite(strRDLFilename);
        System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(stream, System.Text.Encoding.UTF8);

        writer.Formatting = System.Xml.Formatting.Indented;
        writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"utf-8\"");
        #region Report
        writer.WriteStartElement("Report");

        AddReportConfiguration(ref writer, "Patient List", strLanguage, (Decimal)29, (Decimal)0.5, (Decimal)0.5, (Decimal)0.5, (Decimal)0.5, (Decimal)21, "cm");
        AddDataSource(ref writer, dsReport);
        AddDataSets(ref writer, dsReport);

        #region Body
        writer.WriteStartElement("Body");
        writer.WriteElementString("Height", "20cm"); //Report.height - (TopMargin + bottomMargin) == 29.7 - (0.5+0.5)
        writer.WriteStartElement("ReportItems");

        #region Table
        writer.WriteStartElement("Table");
        writer.WriteAttributeString("Name", "rptPatientList");

        #region TableColumns
        writer.WriteStartElement("TableColumns");
        AddColumn(ref writer, "1.5cm");
        AddColumn(ref writer, "0.75cm");
        AddColumn(ref writer, "3cm");
        AddColumn(ref writer, "3.5cm");
        AddColumn(ref writer, "4.5cm");
        AddColumn(ref writer, "3.25cm");
        AddColumn(ref writer, "2.5cm");
        AddColumn(ref writer, "1cm");
        AddColumn(ref writer, "1.5cm");
        AddColumn(ref writer, "2.25cm");
        AddColumn(ref writer, "2cm");
        AddColumn(ref writer, "1cm");
        AddColumn(ref writer, "1.25cm");
        writer.WriteEndElement();
        #endregion

        #region Header
        writer.WriteStartElement("Header");
        writer.WriteElementString("RepeatOnNewPage", "true");

        #region TableRows
        writer.WriteStartElement("TableRows");

        #region Row6
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");

        AddCell(ref writer, "Textbox", "txtPatientID_hr", "ID", DetailCellStyle("Blue", "", "", "LightGrey"), "2");
        AddCell(ref writer, "Textbox", "txtPatientFirstname_hr", "First Name", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtPatientLastname_hr", "Surname", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtDOB_hr", "Date of Birth", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtAddress_hr", "Address", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtSuburb_hr", "Suburb", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtState_hr", "State", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtPostcode_hr", "Postcode", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtHomePhone_hr", "Home Phone", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtMobilePhone_hr", "Mobile Phone", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtAge_hr", "Age", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtSex_hr", "Gender", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        writer.WriteEndElement();
        #endregion
        writer.WriteEndElement();
        #endregion

        #region Deatails
        writer.WriteStartElement("Details");
        writer.WriteStartElement("TableRows");
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.75cm");
        writer.WriteStartElement("TableCells");

        AddCell(ref writer, "Textbox", "txtPatientID", "=Fields!PatientID.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtPatientTitle", "=Fields!PatientTitle.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtPatientFirstname", "=Fields!Firstname.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtPatientLastname", "=Fields!Surname.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtPatientDOB", "=Fields!StrBirthdate.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtAddress", "=Fields!Street.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtSuburb", "=Fields!Suburb.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtState", "=Fields!State.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtPostcode", "=Fields!Postcode.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtHomePhone", "=Fields!HomePhone.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtMobilePhone", "=Fields!MobilePhone.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtAge", "=Fields!AGE.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtSex", "=Fields!Sex.Value", DetailCellStyle("", "", "", ""), "");
        
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        writer.WriteEndElement();
        #endregion

        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        writer.WriteEndElement();
        #endregion
        writer.Flush();
        stream.Close();
    }
    #endregion
    #endregion 

    #region Operaiton Detail List Report with RDL Builder
    #region private void RepOperationList_BuildReport()
    private void RepOperationList_BuildReport()
    {
        DataSet dsReport = new DataSet();
        SqlCommand cmdSelect = new SqlCommand();

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_Rep_OperationDetails", true);

        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSelect.Parameters.Add("@SurgeonId", SqlDbType.Int).Value = 0;
        cmdSelect.Parameters.Add("@HospitalCode", SqlDbType.VarChar, 6).Value = "";
        cmdSelect.Parameters.Add("@RegionId", SqlDbType.VarChar, 6).Value = "";

        cmdSelect.Parameters.Add("@FDate", SqlDbType.VarChar).Value = "";
        cmdSelect.Parameters.Add("@TDate", SqlDbType.VarChar).Value = "";

        cmdSelect.Parameters.Add("@FAge", SqlDbType.Int).Value = 0;
        cmdSelect.Parameters.Add("@TAge", SqlDbType.Int).Value = 0;
        cmdSelect.Parameters.Add("@FBMI", SqlDbType.Int).Value = 0;
        cmdSelect.Parameters.Add("@TBMI", SqlDbType.Int).Value = 0;

        cmdSelect.Parameters.Add("@GroupCode", SqlDbType.VarChar, 10).Value = "";
        cmdSelect.Parameters.Add("@SurgeryType", SqlDbType.VarChar, 10).Value = "";
        cmdSelect.Parameters.Add("@BandType", SqlDbType.VarChar, 10).Value = "";

        cmdSelect.Parameters.Add("@Approach", SqlDbType.VarChar, 10).Value = "";
        cmdSelect.Parameters.Add("@Category", SqlDbType.VarChar, 10).Value = "";
        cmdSelect.Parameters.Add("@BandSize", SqlDbType.VarChar, 10).Value = "";
        cmdSelect.Parameters.Add("@SerialNo", SqlDbType.VarChar, 10).Value = "";

        dsReport = gClass.FetchData(cmdSelect, "tblOperationDetails");

        DataTable dtTemp = dsReport.Tables["tblOperationDetails"];
        gClass.AddColumn(ref dtTemp, "ReportDate", "System.String", DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToShortTimeString());

        dsReport.AcceptChanges();

        CreateOutputFile("REPOPERATIONLIST", dsReport);
    }
    #endregion

    #region private void RepOperationList_RDL(DataSet dsReport, string strRDLFilename, string strLanguage)
    private void RepOperationList_RDL(DataSet dsReport, string strRDLFilename, string strLanguage)
    {
        System.IO.FileStream stream = System.IO.File.OpenWrite(strRDLFilename);
        System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(stream, System.Text.Encoding.UTF8);

        #region Report
        writer.Formatting = System.Xml.Formatting.Indented;
        writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"utf-8\"");
        writer.WriteStartElement("Report");     //<Report>
        AddReportConfiguration(ref writer, "Operation Details", strLanguage, (Decimal)29, (Decimal)0.5, (Decimal)0.5, (Decimal)0.5, (Decimal)0.5, (Decimal)21, "cm");
        AddDataSource(ref writer, dsReport);
        AddDataSets(ref writer, dsReport);

        #region Body
        writer.WriteStartElement("Body");
        writer.WriteElementString("Height", "20cm"); //Report.height - (TopMargin + bottomMargin) == 29.7 - (0.5+0.5)
        writer.WriteStartElement("ReportItems");

        #region Table
        writer.WriteStartElement("Table");
        writer.WriteAttributeString("Name", "rptOperationDetails");

        #region TableColumns
        writer.WriteStartElement("TableColumns");
        AddColumn(ref writer, "1.5cm");
        AddColumn(ref writer, "0.75cm");
        AddColumn(ref writer, "2cm");
        AddColumn(ref writer, "2.5cm");
        AddColumn(ref writer, "2.5cm");
        AddColumn(ref writer, "4cm");
        AddColumn(ref writer, "3cm");
        AddColumn(ref writer, "3cm");
        AddColumn(ref writer, "3cm");
        AddColumn(ref writer, "3cm");
        AddColumn(ref writer, "2cm");
        AddColumn(ref writer, "2cm");
        writer.WriteEndElement();
        #endregion

        #region Header
        writer.WriteStartElement("Header");
        writer.WriteElementString("RepeatOnNewPage", "true");

        #region TableRows
        writer.WriteStartElement("TableRows");

        #region Row5
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.5cm");
        writer.WriteStartElement("TableCells");

        AddCell(ref writer, "Textbox", "txtPatientID_hr", "ID", DetailCellStyle("Blue", "", "", "LightGrey"), "2");
        AddCell(ref writer, "Textbox", "txtPatientFirstname_hr", "First Name", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtPatientSurname_hr", "Surname", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtSurgeryDate_hr", "Operation Date", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtSurgeryType_hr", "Operation", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtSurgerySurgeon_hr", "Surgeon", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtSurgeryApproach_hr", "Approach", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtSurgeryCategory_hr", "Category", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtSurgeryHospital_hr", "Hospital", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtSurgeryBandTypehr", "Band Type", DetailCellStyle("Blue", "", "", "LightGrey"), "");
        AddCell(ref writer, "Textbox", "txtSurgeryBandSize_hr", "Band Size", DetailCellStyle("Blue", "", "", "LightGrey"), "");

        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        writer.WriteEndElement();
        #endregion
        writer.WriteEndElement();
        #endregion

        #region Details
        writer.WriteStartElement("Details");
        writer.WriteStartElement("TableRows");
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.75cm");
        writer.WriteStartElement("TableCells");

        AddCell(ref writer, "Textbox", "txtPatientID", "=Fields!PatientID.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtPatientTitle", "=Fields!PatientTitle.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtPatientFirstName", "=Fields!Firstname.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtPatientSurname", "=Fields!Surname.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtSurgeryDate", "=Fields!OperationDate.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtSurgery", "=Fields!SurgeryType_Desc.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtSurgeonName", "=Fields!DoctorName.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtApproach", "=Fields!Approach_Desc.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtCategory", "=Fields!Category_Desc.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtHospitalDesc", "=Fields!HospitalName.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtBandType", "=Fields!BandType_Desc.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtBandSize", "=Fields!BandSize_Desc.Value", DetailCellStyle("", "", "", ""), "");

        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        writer.WriteEndElement();
        #endregion

        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        writer.WriteEndElement();
        #endregion
        writer.Flush();
        stream.Close();
    }
    #endregion
    #endregion


    #region BSR Report with RDL Builder
    #region private void BSRReport_BuildReport()
    private void BSRReport_BuildReport()
    {
        DataSet dsReport = new DataSet();
        SqlCommand cmdSelect = new SqlCommand();

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_Rep_BSRReport", true);
        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                
        dsReport = gClass.FetchData(cmdSelect, "tblPatientBSR");

        DataTable dtTemp = dsReport.Tables["tblPatientBSR"];
        gClass.AddColumn(ref dtTemp, "ReportDate", "System.String", DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToShortTimeString());
        dsReport.AcceptChanges();

        CreateOutputFile("BSRREPORT", dsReport);
    }
    #endregion

    #region private void BSRReport_RDL(DataSet dsReport, string strRDLFilename, string strLanguage)
    private void BSRReport_RDL(DataSet dsReport, string strRDLFilename, string strLanguage)
    {
        System.IO.FileStream stream = System.IO.File.OpenWrite(strRDLFilename);
        System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(stream, System.Text.Encoding.UTF8);

        #region Report
        writer.Formatting = System.Xml.Formatting.Indented;
        writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"utf-8\"");
        writer.WriteStartElement("Report");     //<Report>
        AddReportConfiguration(ref writer, "BSR Report", strLanguage, (Decimal)29, (Decimal)0.5, (Decimal)0.5, (Decimal)0.5, (Decimal)0.5, (Decimal)21, "cm");
        AddDataSource(ref writer, dsReport);
        AddDataSets(ref writer, dsReport);

        #region Body
        writer.WriteStartElement("Body");
        writer.WriteElementString("Height", "20cm"); //Report.height - (TopMargin + bottomMargin) == 29.7 - (0.5+0.5)
        writer.WriteStartElement("ReportItems");

        #region Table
        writer.WriteStartElement("Table");
        writer.WriteAttributeString("Name", "rptBSRReport");

        #region TableColumns
        writer.WriteStartElement("TableColumns");

        AddColumn(ref writer, "2cm");
        AddColumn(ref writer, "2cm");
        AddColumn(ref writer, "2cm");
        AddColumn(ref writer, "2cm");
        AddColumn(ref writer, "2cm");
        AddColumn(ref writer, "2cm");
        AddColumn(ref writer, "2cm");
        AddColumn(ref writer, "2cm");
        AddColumn(ref writer, "2cm");
        AddColumn(ref writer, "2cm");
        AddColumn(ref writer, "2cm");
        AddColumn(ref writer, "2cm");
        AddColumn(ref writer, "2cm");
        AddColumn(ref writer, "2cm");
        AddColumn(ref writer, "2cm");
        AddColumn(ref writer, "2cm");
        AddColumn(ref writer, "2cm");
        AddColumn(ref writer, "2cm");
        writer.WriteEndElement();
        #endregion

        #region Header
        writer.WriteStartElement("Header");
        writer.WriteElementString("RepeatOnNewPage", "true");

        #region TableRows
        writer.WriteStartElement("TableRows");


        #region Row1
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "1cm");
        writer.WriteStartElement("TableCells");

        AddCell(ref writer, "Textbox", "txtSurname_hr", "Surname", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtFirstname_hr", "Firstname", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtBirthDate_hr", "BirthDate", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtStartWeight_hr", "StartWeight", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtHeight_hr", "Height", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtOpWeight_hr", "OperationWeight", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtOpDate_hr", "OperationDate", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtSurgeryType_hr", "SurgeryType", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtApproach_hr", "Approach", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtBandType_hr", "BandType", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtBandSize_hr", "BandSize", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtRegistryProcedure_hr", "RegistryProcedure", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtIntraEvents_hr", "IntraEvents", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtPredischargeEvents_hr", "PredischargeEvents", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtLastVisitWeight_hr", "LastVisitWeight", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtLastVisitDate_hr", "LastVisitDate", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtLastVisitSinceOperation_hr", "LastVisitSinceOperation", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtComplication_hr", "Complication", DetailCellStyle("", "", "", ""), "");
        
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        writer.WriteEndElement();
        #endregion
        writer.WriteEndElement();
        #endregion

        #region Details
        writer.WriteStartElement("Details");
        writer.WriteStartElement("TableRows");
        writer.WriteStartElement("TableRow");
        writer.WriteElementString("Height", "0.75cm");
        writer.WriteStartElement("TableCells");

        AddCell(ref writer, "Textbox", "txtSurname", "=Fields!surname.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtFirstname", "=Fields!firstname.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtBirthDate", "=Fields!BirthDate.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtStartWeight", "=Fields!startweight.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtHeight", "=Fields!height.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtOpWeight", "=Fields!opweight.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtOpDate", "=Fields!OperationDate.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtSurgeryType", "=Fields!SurgeryType.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtApproach", "=Fields!Approach.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtBandType", "=Fields!bandtype.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtBandSize", "=Fields!bandsize.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtRegistryProcedure", "=Fields!registryProcedure.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtIntraEvents", "=Fields!intraevents.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtPredischargeEvents", "=Fields!predischargeevents.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtLastVisitWeight", "=Fields!LastVisitWeight.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtLastVisitDate", "=Fields!LastVisitDate.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtLastVisitSinceOperation", "=Fields!LastVisitSinceOperation.Value", DetailCellStyle("", "", "", ""), "");
        AddCell(ref writer, "Textbox", "txtComplication", "=Fields!Complication.Value", DetailCellStyle("", "", "", ""), "");
        


        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        writer.WriteEndElement();
        #endregion

        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion

        writer.WriteEndElement();
        #endregion
        writer.Flush();
        stream.Close();
    }
    #endregion
    #endregion

    #region private DataSet LoadPatientData()
    private DataSet LoadPatientData()
    {
        SqlCommand cmdSelect = new SqlCommand();
        DataSet dsPatient = new DataSet();

        try
        {
            gClass.MakeStoreProcedureName(ref cmdSelect, "sp_PatientData_LoadData", true);
            cmdSelect.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
            cmdSelect.Parameters.Add("@UserPracticeCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
            cmdSelect.Parameters.Add("@PatientID", System.Data.SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
            if (Request.Cookies["PatientCustomID"]!= null)
                cmdSelect.Parameters.Add("@Patient_CustomID", System.Data.SqlDbType.VarChar, 20).Value = Request.Cookies["PatientCustomID"].Value;

            dsPatient = gClass.FetchData(cmdSelect, "tblPatient");
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Patient Data Form", "Loading Patient Data - LoadPatientData function", err.ToString());
        }
        return dsPatient;
    }
    #endregion

    #region private DataSet LoadPatientComorbidity()
    private DataSet LoadPatientComorbidity()
    {
        SqlCommand cmdSelect = new SqlCommand();
        DataSet dsPatient = new DataSet();

        try
        {
            cmdSelect.Parameters.Clear();
            gClass.MakeStoreProcedureName(ref cmdSelect, "sp_ConsultFU1_ProgressNotes_LoadBoldComorbidityData", true);
            cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
            cmdSelect.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
            cmdSelect.Parameters.Add("@ConsultId", SqlDbType.Int).Value = 0;
            dsPatient = gClass.FetchData(cmdSelect, "tblPatient");
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Patient Comorbidity", "Loading Patient Comorbidity", err.ToString());
        }
        return dsPatient;
    }
    #endregion

    #region private DataSet LoadPatientBold()
    private DataSet LoadPatientBold()
    {
        SqlCommand cmdSelect = new SqlCommand();
        DataSet dsPatient = new DataSet();

        try
        {
            cmdSelect.Parameters.Clear();
            gClass.MakeStoreProcedureName(ref cmdSelect, "sp_PatientData_LoadBoldData", true);
            cmdSelect.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
            cmdSelect.Parameters.Add("@PatientID", System.Data.SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
            dsPatient = gClass.FetchData(cmdSelect, "tblPatient_BoldData");
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Patient Bold", "Loading Patient Bold", err.ToString());
        }
        return dsPatient;
    }
    #endregion

    #region private DataSet LoadPatientEMR()
    private DataSet LoadPatientEMR()
    {        
        SqlCommand cmdSelect = new SqlCommand();
        DataSet dsPatient = new DataSet();

        try
        {
            gClass.MakeStoreProcedureName(ref cmdSelect, "sp_PatientEMR_LoadData", true);
            cmdSelect.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
            cmdSelect.Parameters.Add("@PatientID", System.Data.SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
            dsPatient = gClass.FetchData(cmdSelect, "tblPatient");
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Patient EMR", "Loading Patient EMR", err.ToString());
        }
        return dsPatient;
    }
    #endregion

    #region private DataSet LoadPatientOperation()
    private DataSet LoadPatientOperation()
    {
        SqlCommand cmdSelect = new SqlCommand();
        DataSet dsPatientOperation = new DataSet();

        try
        {
            gClass.MakeStoreProcedureName(ref cmdSelect, "sp_Operation_SelectPatientOperationData", true);
            cmdSelect.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
            cmdSelect.Parameters.Add("@AdmitID", System.Data.SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["RecID"]);
            cmdSelect.Parameters.Add("@ImperialFlag", SqlDbType.Bit).Value = Request.Cookies["Imperial"].Value.Equals("True");
            dsPatientOperation = gClass.FetchData(cmdSelect, "tblPatient");
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Patient Operation", "Loading Patient Operation", err.ToString());
        }
        return dsPatientOperation;
    }
    #endregion

    #region private DataSet LoadSystemDetail()
    private DataSet LoadSystemDetail()
    {
        SqlCommand cmdSelect = new SqlCommand();
        DataSet dsPatient = new DataSet();

        try
        {
            gClass.MakeStoreProcedureName(ref cmdSelect, "sp_SystemDetails_LoadData", true);

            cmdSelect.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
            dsPatient = gClass.FetchData(cmdSelect, "tblPatient");
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Patient EMR", "Loading System Detail", err.ToString());
        }
        return dsPatient;
    }
    #endregion

    #region private DataSet LoadPatientVisit(String period, int periodNum)
    private DataSet LoadPatientVisit(String period, int periodNum)
    {
        SqlCommand cmdSelect = new SqlCommand();
        DataSet dsPatient = new DataSet();

        try
        {
            cmdSelect.Parameters.Clear();
            gClass.MakeStoreProcedureName(ref cmdSelect, "sp_ConsultFU1_ProgressNotes_LoadByDate", true);
            cmdSelect.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
            cmdSelect.Parameters.Add("@PatientID", System.Data.SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
            cmdSelect.Parameters.Add("@Period", System.Data.SqlDbType.VarChar, 10).Value = period.Trim();
            cmdSelect.Parameters.Add("@PeriodNum", System.Data.SqlDbType.Int).Value = periodNum;
            cmdSelect.Parameters.Add("@ImperialFlag", SqlDbType.Bit).Value = Request.Cookies["Imperial"].Value.Equals("True");
            dsPatient = gClass.FetchData(cmdSelect, "tblPatient_Visit");
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Patient Visit", "Loading Patient Visit", err.ToString());
        }
        return dsPatient;
    }
    #endregion

    #region private DataSet LoadPatientFollowUp()
    private DataSet LoadPatientFollowUp()
    {
        SqlCommand cmdSelect = new SqlCommand();
        DataSet dsPatient = new DataSet();

        try
        {
            cmdSelect.Parameters.Clear();
            gClass.MakeStoreProcedureName(ref cmdSelect, "sp_FollowUpCheck_LoadData", true);
            cmdSelect.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
            cmdSelect.Parameters.Add("@PatientID", System.Data.SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
            dsPatient = gClass.FetchData(cmdSelect, "tblPatient_FollowUp");
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Patient Follow up", "Loading Patient Followup", err.ToString());
        }
        return dsPatient;
    }
    #endregion

    #region private DataSet LoadPatientEvents(String period, int periodNum)
    private DataSet LoadPatientEvents(String period, int periodNum)
    {
        SqlCommand cmdSelect = new SqlCommand();
        DataSet dsPatient = new DataSet();

        try
        {
            cmdSelect.Parameters.Clear();
            gClass.MakeStoreProcedureName(ref cmdSelect, "sp_ConsultFU1_Complications_LoadDataAfterBaselineOperation", true);
            cmdSelect.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
            cmdSelect.Parameters.Add("@PatientID", System.Data.SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
            cmdSelect.Parameters.Add("@Period", System.Data.SqlDbType.VarChar, 10).Value = period.Trim();
            cmdSelect.Parameters.Add("@PeriodNum", System.Data.SqlDbType.Int).Value = periodNum;
            dsPatient = gClass.FetchData(cmdSelect, "tblPatient_Events");
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Patient Events", "Loading Patient Events", err.ToString());
        }
        return dsPatient;
    }
    #endregion

    #region private DataSet LoadLogoByID(Int32 LogoID)
    private DataSet LoadLogoByID(Int32 LogoID)
    {
        SqlCommand cmdSelect = new SqlCommand();
        DataSet dsLogo = new DataSet();

        try
        {
            gClass.MakeStoreProcedureName(ref cmdSelect, "sp_Logos_LoadDataByID", true);
            cmdSelect.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
            cmdSelect.Parameters.Add("@LogoID", System.Data.SqlDbType.Int).Value = LogoID;
            dsLogo = gClass.FetchData(cmdSelect, "tblLogo");
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Practice Logo", "Loading Logo", err.ToString());
        }
        return dsLogo;
    }
    #endregion

    #region private DataSet LoadLastVisit(string startDateType, string period, int periodNumMin, int periodNumMax, string checkDate)
    private DataSet LoadLastVisit(string startDateType, string period, int periodNumMin, int periodNumMax, string checkDate)
    {
        SqlCommand cmdSelect = new SqlCommand();
        DataSet dsPatientVisit = new DataSet();

        try
        {
            gClass.MakeStoreProcedureName(ref cmdSelect, "sp_ConsultFU1_ProgressNotes_LoadLastSinceOperation", true);
            cmdSelect.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
            cmdSelect.Parameters.Add("@PatientID", System.Data.SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
            cmdSelect.Parameters.Add("@StartDateType", System.Data.SqlDbType.VarChar, 10).Value = startDateType; // latest or baseline
            cmdSelect.Parameters.Add("@Period", System.Data.SqlDbType.VarChar, 10).Value = period;
            cmdSelect.Parameters.Add("@PeriodNumMin", System.Data.SqlDbType.Int).Value = periodNumMin;
            cmdSelect.Parameters.Add("@PeriodNumMax", System.Data.SqlDbType.Int).Value = periodNumMax;
            cmdSelect.Parameters.Add("@CheckDate", System.Data.SqlDbType.VarChar, 10).Value = checkDate;
            dsPatientVisit = gClass.FetchData(cmdSelect, "tblVisit");
            dsPatientVisit = dsPatientVisit;
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Patient Operation", "Loading Patient Operation", err.ToString());
        }
        return dsPatientVisit;
    }
    #endregion

    #region private DataSet LoadLastComplication(string startDateType, string period, int periodNumMin, int periodNumMax, string checkDate)
    private DataSet LoadLastComplication(string startDateType, string period, int periodNumMin, int periodNumMax, string checkDate)
    {
        SqlCommand cmdSelect = new SqlCommand();
        DataSet dsPatientComplication = new DataSet();

        try
        {
            gClass.MakeStoreProcedureName(ref cmdSelect, "sp_ConsultFU1_Complications_LoadSinceOperation", true);
            cmdSelect.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
            cmdSelect.Parameters.Add("@PatientID", System.Data.SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
            cmdSelect.Parameters.Add("@StartDateType", System.Data.SqlDbType.VarChar, 10).Value = startDateType; // latest or baseline
            cmdSelect.Parameters.Add("@Period", System.Data.SqlDbType.VarChar, 10).Value = period;
            cmdSelect.Parameters.Add("@PeriodNumMin", System.Data.SqlDbType.Int).Value = periodNumMin;
            cmdSelect.Parameters.Add("@PeriodNumMax", System.Data.SqlDbType.Int).Value = periodNumMax;
            cmdSelect.Parameters.Add("@CheckDate", System.Data.SqlDbType.VarChar, 10).Value = checkDate;
            dsPatientComplication = gClass.FetchData(cmdSelect, "tblComplication");
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Patient Complication", "Loading Patient Complication", err.ToString());
        }
        return dsPatientComplication;
    }
    #endregion

    #region private void SB_BuildReport(string strParam)
    private void SB_BuildReport(string strParam)
    {
        string[] rptParam = strParam.Split(new char[] {'-'});
        string[] reportType = { "99204 - Extended Consultation - New Patient", "99213 - Detailed Consultation - Established Patient", "99214 - Extended Consultation - Established Patient", "S2083 - Adjustment of Gastric Band (43999)", "74230 - Swallowing Function with Cineradiography", "77002 - Fluoroscopic Guidance", "99212 - Limited Consultation - Established Patient" };
        string reportName = "";
        bool firstReport = true;
        bool adjustmentExist = false;
        
        DataSet dsLogo = new DataSet();
        String logoID;
        String LogoPath = "";

        DataSet dsReport;
        SqlCommand cmdSelect = new SqlCommand();

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_ConsultFu1_ProgressNotes_LoadSingleData", true);
        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt64(gClass.OrganizationCode);
        cmdSelect.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt64(Request.Cookies["UserPracticeCode"].Value);
        cmdSelect.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["PID"]);
        cmdSelect.Parameters.Add("@ImperialFlag", SqlDbType.Bit).Value = Request.Cookies["Imperial"].Value.Equals("True");

        cmdSelect.Parameters.Add("@VisitWeeksFlag", SqlDbType.VarChar).Value = Request.Cookies["VisitWeeksFlag"].Value;
        cmdSelect.Parameters.Add("@ConsultID", SqlDbType.Int).Value = rptParam[0];

        dsReport = gClass.FetchData(cmdSelect, "tblVisitList");

        string customPatientID = dsReport.Tables[0].Rows[0]["CustomPatientID"].ToString();
        string patientName = dsReport.Tables[0].Rows[0]["PatientName"].ToString();
        string age = dsReport.Tables[0].Rows[0]["Age"].ToString();
        string dob = dsReport.Tables[0].Rows[0]["BirthDate"].ToString();
        string address = dsReport.Tables[0].Rows[0]["Street"].ToString().Trim().Replace("`", "'") + " " + dsReport.Tables[0].Rows[0]["Suburb"].ToString().Trim().Replace("`", "'") + " " + dsReport.Tables[0].Rows[0]["State"].ToString().Trim().Replace("`", "'") + " " + dsReport.Tables[0].Rows[0]["Postcode"].ToString().Trim().Replace("`", "'");

        string patientHomePhone = dsReport.Tables[0].Rows[0]["HomePhone"].ToString();
        string patientWorkPhone = dsReport.Tables[0].Rows[0]["WorkPhone"].ToString();
        string patientMobilePhone = dsReport.Tables[0].Rows[0]["MobilePhone"].ToString();
        string patientEmail = dsReport.Tables[0].Rows[0]["EmailAddress"].ToString();

        string surgeryType = dsReport.Tables[0].Rows[0]["SurgeryType_Desc"].ToString();
        string surgeryDate = dsReport.Tables[0].Rows[0]["LapbandDate"].ToString();
        string surgeryCategory = dsReport.Tables[0].Rows[0]["Category_Desc"].ToString();
        string surgeryApproach = dsReport.Tables[0].Rows[0]["Approach"].ToString();
        string surgeonName = dsReport.Tables[0].Rows[0]["SurgeonName"].ToString(); 

        string patientHeight = dsReport.Tables[0].Rows[0]["Height"].ToString();
        decimal decPatientHeight = Math.Round(Decimal.Parse(patientHeight),1);
        string patientHeightMeasurment = dsReport.Tables[0].Rows[0]["HeightMeasurment"].ToString();

        string patientWeight = dsReport.Tables[0].Rows[0]["StartWeight"].ToString();
        decimal decPatientWeight = Math.Round(Decimal.Parse(patientWeight), 1);

        string patientTargetWeight = dsReport.Tables[0].Rows[0]["TargetWeight"].ToString();
        decimal decPatientTargetWeight = Math.Round(Decimal.Parse(patientTargetWeight), 1);

        string patientIdealWeight = dsReport.Tables[0].Rows[0]["IdealWeight"].ToString();
        decimal decPatientIdealWeight = Math.Round(Decimal.Parse(patientIdealWeight), 1);

        string patientWeightMeasurment = dsReport.Tables[0].Rows[0]["WeightMeasurment"].ToString();

        string patientBMI = dsReport.Tables[0].Rows[0]["initBMI"].ToString();
        decimal decPatientBMI = Math.Round(Decimal.Parse(patientBMI), 0);



        string consultID = dsReport.Tables[0].Rows[0]["ConsultID"].ToString();
        string visitDate = dsReport.Tables[0].Rows[0]["strDateSeen"].ToString();
        string visitHeight = dsReport.Tables[0].Rows[0]["Height"].ToString();
        string visitWeight = dsReport.Tables[0].Rows[0]["Weight"].ToString();
        decimal decVisitHeight = Math.Round(Decimal.Parse(visitHeight), 1);
        decimal decVisitWeight = Math.Round(Decimal.Parse(visitWeight), 1);
        string RV = dsReport.Tables[0].Rows[0]["ReservoirVolume"].ToString();
        string visitDoctor = dsReport.Tables[0].Rows[0]["DoctorName"].ToString();
        string MedicalProvider = dsReport.Tables[0].Rows[0]["MedicalProviderName"].ToString();
        string nextVisit = dsReport.Tables[0].Rows[0]["DateNextVisit"].ToString();
        string chiefComplaint = dsReport.Tables[0].Rows[0]["ChiefComplaintDesc"].ToString();
        string chiefComplaintCode = dsReport.Tables[0].Rows[0]["ChiefComplaintDesc2"].ToString();
        string generalNotes = dsReport.Tables[0].Rows[0]["Notes"].ToString();
        string lapbandAdjustment = dsReport.Tables[0].Rows[0]["LapbandAdjustment"].ToString();


        string adjustment = "";
        string adjustmentResult = "";
        string adjAnesthesiaVol = dsReport.Tables[0].Rows[0]["AdjAnesthesiaVol"].ToString() == "" ? "0" : dsReport.Tables[0].Rows[0]["AdjAnesthesiaVol"].ToString();
        string adjInitialVol = dsReport.Tables[0].Rows[0]["AdjInitialVol"].ToString() == "" ? "0" : dsReport.Tables[0].Rows[0]["AdjInitialVol"].ToString();
        string adjAddVol = dsReport.Tables[0].Rows[0]["AdjAddVol"].ToString() == "" ? "0" : dsReport.Tables[0].Rows[0]["AdjAddVol"].ToString();
        string adjRemoveVol = dsReport.Tables[0].Rows[0]["AdjRemoveVol"].ToString() == "" ? "0" : dsReport.Tables[0].Rows[0]["AdjRemoveVol"].ToString();
        decimal decAdjAnesthesiaVol = Math.Round(Decimal.Parse(adjAnesthesiaVol), 2);
        decimal decAdjInitialVol = Math.Round(Decimal.Parse(adjInitialVol), 2);
        decimal decAdjAddVol = Math.Round(Decimal.Parse(adjAddVol), 2);
        decimal decAdjRemoveVol = Math.Round(Decimal.Parse(adjRemoveVol), 2);
        decimal decAdjFinalVol = Math.Round((decAdjInitialVol + decAdjAddVol - decAdjRemoveVol), 2);
        decimal decAdjExtraVol = Math.Abs(decAdjAddVol - decAdjRemoveVol);

        string extraVol = decAdjAddVol - decAdjRemoveVol>0?" Added ":" Removed ";

        string visitAdjustmentInitialVol = "      Initial Volume " + decAdjInitialVol + " ml";
        string visitAdjustmentExtraVol = "      Volume " + extraVol + decAdjExtraVol + " ml";
        string visitAdjustmentFinalVol = "      Final Volume " + decAdjFinalVol + " ml";
        string visitAdjustmentNoAdjustment = "No Adjustment";

        string[] visitAdjustmentArr = { "AdjConsent", "AdjAntiseptic", "AdjAnesthesia", "AdjNeedle", "AdjVolume", "AdjTolerate", "AdjBarium", "AdjOmni" };
        string[] visitAdjustmentResultArr = { "Consent form reviewed and signed", "Antiseptic skin prep performed", "Non coring needle used to access the port " + decAdjAnesthesiaVol + " ml 1% lidocaine provided", "Non coring needle used to access the port", "Adjust volume with saline:", "Patient tolerated the procedure well, could drink water easily at completion and was given post-adjustment instructions", "Barium or gastrograffin to perform swallow per standing medical order", "Omnipaque for suspected leak per standing medical order" };
               

        string visitSatietyStaging = dsReport.Tables[0].Rows[0]["SatietyStaging"].ToString();
        string visitPR = dsReport.Tables[0].Rows[0]["PulseRate"].ToString();
        string visitRR = dsReport.Tables[0].Rows[0]["RespiratoryRate"].ToString();
        string visitBPLower = dsReport.Tables[0].Rows[0]["BloodPressureLower"].ToString();
        string visitBPUpper = dsReport.Tables[0].Rows[0]["BloodPressureUpper"].ToString();
        string visitNeck = dsReport.Tables[0].Rows[0]["Neck"].ToString();
        string visitWaist = dsReport.Tables[0].Rows[0]["Waist"].ToString();
        string visitHip = dsReport.Tables[0].Rows[0]["Hip"].ToString();

        decimal decVisitNeck = Math.Round(Decimal.Parse(visitNeck), 1);
        decimal decVisitWaist = Math.Round(Decimal.Parse(visitWaist), 1);
        decimal decVisitHip = Math.Round(Decimal.Parse(visitHip), 1);
        decimal decWHR = 0;
        decimal decVisitWHR = 0;

        string clinicReview = "";
        string clinicReviewResult = "";
        string[] visitReviewArr = { "PFSH", "General", "Gastro", "Cardiovascular", "Respiratory", "Musculoskeletal", "Genito", "Skin", "Neurological", "Psychiatric", "Endocrine", "Hematologic", "ENT", "Eyes", "Medications" };
        string[] visitReviewTitleArr = { "PFSH", "General", "Gastrointestinal", "Cardiovascular", "Respiratory", "Musculoskeletal", "Genitourinary", "Skin and integuments", "Neurological", "Psychiatric", "Endocrine", "Hematologic / Lymphatic", "ENT", "Eyes", "Adjust medications" };


        if (decVisitWaist > 0 && decVisitHip > 0)
        {
            decWHR = decVisitWaist / decVisitHip;
            decVisitWHR = Math.Round(decWHR, 1);
        }

        try
        {
            logoID = rptParam[3].ToString();
            if (logoID != "000")
            {
                dsLogo = LoadLogoByID(Convert.ToInt32(logoID));
                LogoPath = dsLogo.Tables[0].Rows[0]["LogoPath"].ToString();
                LogoPath = LogoPath.Substring(IndexOfOccurence(LogoPath, "/", 2));
            }
        
            string fileName = customPatientID+"-"+consultID;
            string saveFilePath = Server.MapPath(".\\.\\..\\temp\\") + fileName + ".pdf";
            string openFilePath = "..//temp//" + fileName + ".pdf";
            iTextSharp.text.Document oDoc = new iTextSharp.text.Document(PageSize.LETTER, 0,0, 30f, 30f);
            PdfWriter.GetInstance(oDoc, new FileStream(saveFilePath, FileMode.Create));
            oDoc.Open();
            Cell cell = new Cell("");

            //create header
            iTextSharp.text.Table tableLogo = new iTextSharp.text.Table(1, 1);
            iTextSharp.text.Table tableHeader = new iTextSharp.text.Table(7, 7);
            iTextSharp.text.Table tableVisit = new iTextSharp.text.Table(5, 9);
            iTextSharp.text.Table tableVisitDetail = new iTextSharp.text.Table(8, 4);
            iTextSharp.text.Table tableVisitAdjustment = new iTextSharp.text.Table(1, 11);
            iTextSharp.text.Table tableVisitClinicReview = new iTextSharp.text.Table(2, 16);
            iTextSharp.text.Table tableVisitSignature = new iTextSharp.text.Table(2, 2);
            iTextSharp.text.Table tableSuperCode = new iTextSharp.text.Table(1, 1);
            
            // set *column* widths
            float[] logoWidths = { 1f };
            tableLogo.Spacing = 1;
            tableLogo.Widths = logoWidths;
            tableLogo.DefaultCell.BorderWidth = 0;
            tableLogo.DefaultCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
            tableLogo.DefaultCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
            tableLogo.TableFitsPage = true;
            tableLogo.BorderColor = new Color(255, 255, 255);

            // set *column* widths
            float[] widths = { .05f, .33f, .2f, .12f, .1f, .1f, .1f};
            tableHeader.Spacing = 1;
            tableHeader.Widths = widths;
            tableHeader.DefaultCell.BorderWidth = 0;
            tableHeader.TableFitsPage = true;
            tableHeader.BorderColor = new Color(255, 255, 255);

            float[] visitWidths = { .15f, .30f, .15f, .20f, .46f };
            tableVisit.Spacing = 2;
            tableVisit.Widths = visitWidths;
            tableVisit.DefaultCell.BorderWidth = 0;
            tableVisit.TableFitsPage = true;
            tableVisit.BorderColor = new Color(255, 255, 255);

            tableVisit.BorderWidthTop = 1;
            tableVisit.BorderColorTop = new Color(0, 0, 0);

            float[] visitAdjustmentWidths = { 1.0f };
            tableVisitAdjustment.Spacing = 2;
            tableVisitAdjustment.Widths = visitAdjustmentWidths;
            tableVisitAdjustment.DefaultCell.BorderWidth = 0;
            tableVisitAdjustment.TableFitsPage = true;
            tableVisitAdjustment.BorderColor = new Color(255, 255, 255);

            float[] visitDetailWidths = { .12f, .12f, .08f, .12f, .08f, .12f, .08f, .28f };
            tableVisitDetail.Spacing = 2;
            tableVisitDetail.Widths = visitDetailWidths;
            tableVisitDetail.DefaultCell.BorderWidth = 0;
            tableVisitDetail.TableFitsPage = true;
            tableVisitDetail.BorderColor = new Color(255, 255, 255);

            tableVisitDetail.BorderWidthTop = .5f;
            tableVisitDetail.BorderColorTop = new Color(0, 0, 0);

            float[] visitClinicReviewWidths = { .18f, .85f };
            tableVisitClinicReview.Spacing = 2;
            tableVisitClinicReview.Widths = visitClinicReviewWidths;
            tableVisitClinicReview.DefaultCell.BorderWidth = 0;
            tableVisitClinicReview.TableFitsPage = true;
            tableVisitClinicReview.BorderColor = new Color(255, 255, 255);

            tableVisitClinicReview.BorderWidthTop = .5f;
            tableVisitClinicReview.BorderColorTop = new Color(0, 0, 0);

            float[] visitSignatureWidths = { .4f, .6f };
            tableVisitSignature.Spacing = 2;
            tableVisitSignature.Widths = visitSignatureWidths;
            tableVisitSignature.DefaultCell.BorderWidth = 0;
            tableVisitSignature.TableFitsPage = true;
            tableVisitSignature.BorderColor = new Color(255, 255, 255);
            
            float[] superCode = { .5f };
            tableSuperCode.Spacing = 2;
            tableSuperCode.Widths = superCode;
            tableSuperCode.DefaultCell.BorderWidth = 0;
            tableSuperCode.TableFitsPage = true;
            tableSuperCode.BorderColor = new Color(255, 255, 255);


            Phrase phrase = new Phrase("");

            //title page------------------------------------------------------------------------------------------
            #region Logo
            //Logo----------------------------------------------------------------------------------------------
            if (LogoPath != "")
            {
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~" + LogoPath));
                logo.ScalePercent(50);
                //phrase = new Phrase(logo, normalFont);
                cell = new Cell(logo);
                cell.BorderWidth = 0;
                tableLogo.AddCell(cell);
            }
            #endregion
            
            //row1------------------------------------------------------------------------------------------------            
            phrase = new Phrase("\n\nPatient ID: " + customPatientID, new Font(Font.HELVETICA, 6, Font.BOLD));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            cell.Colspan = 7;
            tableHeader.AddCell(cell);
            //row2------------------------------------------------------------------------------------------------            
            phrase = new Phrase(patientName, new Font(Font.HELVETICA, 10, Font.BOLD));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            cell.Colspan = 2;
            tableHeader.AddCell(cell);

            phrase = new Phrase("Operation Detail", new Font(Font.HELVETICA, 9, Font.BOLD));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            cell.Colspan = 2;
            tableHeader.AddCell(cell);

            phrase = new Phrase("Baseline Report", new Font(Font.HELVETICA, 9, Font.BOLD));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            cell.Colspan = 3;
            tableHeader.AddCell(cell);

            //row4------------------------------------------------------------------------------------------------
            phrase = new Phrase(age + " yrs", new Font(Font.HELVETICA, 5, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            tableHeader.AddCell(cell);

            phrase = new Phrase(dob, new Font(Font.HELVETICA, 5, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            tableHeader.AddCell(cell);

            phrase = new Phrase(surgeryType, new Font(Font.HELVETICA, 5, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            tableHeader.AddCell(cell);

            phrase = new Phrase(surgeryDate, new Font(Font.HELVETICA, 5, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            tableHeader.AddCell(cell);

            phrase = new Phrase("Height: " + decPatientHeight + " " + patientHeightMeasurment, new Font(Font.HELVETICA, 5, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            tableHeader.AddCell(cell);

            phrase = new Phrase("Weight: " + decPatientWeight + " " + patientWeightMeasurment, new Font(Font.HELVETICA, 5, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            tableHeader.AddCell(cell);

            phrase = new Phrase("BMI: " + decPatientBMI, new Font(Font.HELVETICA, 5, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            tableHeader.AddCell(cell);

            //row5------------------------------------------------------------------------------------------------
            cell = new Cell("");
            cell.BorderWidth = 0;
            tableHeader.AddCell(cell);

            phrase = new Phrase(address, new Font(Font.HELVETICA, 5, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            tableHeader.AddCell(cell);

            phrase = new Phrase(surgeryCategory, new Font(Font.HELVETICA, 5, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            tableHeader.AddCell(cell);

            phrase = new Phrase(surgeryApproach, new Font(Font.HELVETICA, 5, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            tableHeader.AddCell(cell);


            phrase = new Phrase("Target Weight: " + decPatientTargetWeight + " " + patientWeightMeasurment + "          Ideal Weight: " + decPatientIdealWeight + " " + patientWeightMeasurment, new Font(Font.HELVETICA, 5, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            cell.Colspan = 3;
            tableHeader.AddCell(cell);

            //row6------------------------------------------------------------------------------------------------
            cell = new Cell("");
            cell.BorderWidth = 0;
            tableHeader.AddCell(cell);

            phrase = new Phrase("Phone (H): " + patientHomePhone + "  (W): " + patientWorkPhone + "  (M): " + patientMobilePhone, new Font(Font.HELVETICA, 5, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            tableHeader.AddCell(cell);

            phrase = new Phrase(surgeonName , new Font(Font.HELVETICA, 5, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            cell.Colspan = 5;
            tableHeader.AddCell(cell);
            
            //row6------------------------------------------------------------------------------------------------
            cell = new Cell("");
            cell.BorderWidth = 0;
            tableHeader.AddCell(cell);

            phrase = new Phrase(patientEmail + "\n ", new Font(Font.HELVETICA, 5, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            tableHeader.AddCell(cell);

            //visit-----------------------------------------------------------------------------------------------
            //row1------------------------------------------------------------------------------------------------
            phrase = new Phrase("\nVisit Detail", new Font(Font.HELVETICA, 9, Font.BOLD));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            cell.Colspan = 5;
            tableVisit.AddCell(cell);

            //row2------------------------------------------------------------------------------------------------
            phrase = new Phrase("Visit Date: ", new Font(Font.HELVETICA, 6, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            tableVisit.AddCell(cell);

            phrase = new Phrase(visitDate, new Font(Font.HELVETICA, 6, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            tableVisit.AddCell(cell);

            // removed height from the following cells (AJ)

            phrase = new Phrase("", new Font(Font.HELVETICA, 6, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            tableVisit.AddCell(cell);

            phrase = new Phrase("", new Font(Font.HELVETICA, 6, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            cell.Colspan = 2;
            tableVisit.AddCell(cell);
            

            //row3------------------------------------------------------------------------------------------------
            phrase = new Phrase("Seen by: ", new Font(Font.HELVETICA, 6, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            tableVisit.AddCell(cell);

            phrase = new Phrase(visitDoctor, new Font(Font.HELVETICA, 6, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            tableVisit.AddCell(cell);


            phrase = new Phrase("Weight: ", new Font(Font.HELVETICA, 6, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            tableVisit.AddCell(cell);


            phrase = new Phrase(decVisitWeight + " " + patientWeightMeasurment, new Font(Font.HELVETICA, 6, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            cell.Colspan = 2;
            tableVisit.AddCell(cell);

            //row3.5------------------------------------------------------------------------------------------------
            phrase = new Phrase("Medical Provider: ", new Font(Font.HELVETICA, 6, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            tableVisit.AddCell(cell);

            phrase = new Phrase(MedicalProvider, new Font(Font.HELVETICA, 6, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            tableVisit.AddCell(cell);

            
            phrase = new Phrase("RV: ", new Font(Font.HELVETICA, 6, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            tableVisit.AddCell(cell);

            phrase = new Phrase(RV, new Font(Font.HELVETICA, 6, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            cell.Colspan = 2;
            tableVisit.AddCell(cell);

            //row4------------------------------------------------------------------------------------------------
            phrase = new Phrase("Next Visit: ", new Font(Font.HELVETICA, 6, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            tableVisit.AddCell(cell);

            phrase = new Phrase(nextVisit, new Font(Font.HELVETICA, 6, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            cell.Colspan = 4;
            tableVisit.AddCell(cell);


            //row5------------------------------------------------------------------------------------------------
            phrase = new Phrase("Chief Complaint: ", new Font(Font.HELVETICA, 6, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            tableVisit.AddCell(cell);

            phrase = new Phrase(chiefComplaintCode + " - " + chiefComplaint, new Font(Font.HELVETICA, 6, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            cell.Colspan = 4;
            tableVisit.AddCell(cell);



            //row6------------------------------------------------------------------------------------------------
            phrase = new Phrase("\nClinical Notes: ", new Font(Font.HELVETICA, 7, Font.BOLD));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            cell.Colspan = 5;
            tableVisit.AddCell(cell);

            phrase = new Phrase(generalNotes, new Font(Font.HELVETICA, 6, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            cell.Colspan = 5;
            tableVisit.AddCell(cell);


            /*
            //row7------------------------------------------------------------------------------------------------
            phrase = new Phrase("\nAdjustment of Band: ", new Font(Font.HELVETICA, 7, Font.BOLD));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            cell.Colspan = 5;
            tableVisit.AddCell(cell);

            phrase = new Phrase(lapbandAdjustment + "\n ", new Font(Font.HELVETICA, 6, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            cell.Colspan = 5;
            tableVisit.AddCell(cell);
            */

            //visit adjustment------------------------------------------------------------------------------------
            //row1------------------------------------------------------------------------------------------------
            phrase = new Phrase("\nAdjustment Performed: ", new Font(Font.HELVETICA, 7, Font.BOLD));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            tableVisitAdjustment.AddCell(cell);

            for (int Idx = 0; Idx < visitAdjustmentArr.Length; Idx++)
            {
                adjustment = visitAdjustmentArr[Idx];
                adjustmentResult = dsReport.Tables[0].Rows[0][adjustment].ToString();
                if (adjustmentResult == "True")
                {
                    adjustmentExist = true;

                    phrase = new Phrase(visitAdjustmentResultArr[Idx], new Font(Font.HELVETICA, 6, 0));
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableVisitAdjustment.AddCell(cell);

                    if (Idx == 4)
                    {
                        phrase = new Phrase(visitAdjustmentInitialVol, new Font(Font.HELVETICA, 6, 0));
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tableVisitAdjustment.AddCell(cell);

                        phrase = new Phrase(visitAdjustmentExtraVol, new Font(Font.HELVETICA, 6, 0));
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tableVisitAdjustment.AddCell(cell);

                        phrase = new Phrase(visitAdjustmentFinalVol, new Font(Font.HELVETICA, 6, 0));
                        cell = new Cell(phrase);
                        cell.BorderWidth = 0;
                        tableVisitAdjustment.AddCell(cell);
                    }
                }
            }

            if(adjustmentExist == false)
            {
                phrase = new Phrase(visitAdjustmentNoAdjustment, new Font(Font.HELVETICA, 6, 0));
                cell = new Cell(phrase);
                cell.BorderWidth = 0;
                tableVisitAdjustment.AddCell(cell);
            }
            cell = new Cell("\n");
            cell.BorderWidth = 0;
            tableVisitAdjustment.AddCell(cell);


            //visit detail----------------------------------------------------------------------------------------
            //row1------------------------------------------------------------------------------------------------
            phrase = new Phrase("Vital Signs", new Font(Font.HELVETICA, 6, Font.BOLD));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            cell.Colspan = 8;
            tableVisitDetail.AddCell(cell);

            //row2------------------------------------------------------------------------------------------------
            phrase = new Phrase("PR: ", new Font(Font.HELVETICA, 6, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            tableVisitDetail.AddCell(cell);

            phrase = new Phrase(visitPR, new Font(Font.HELVETICA, 6, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            tableVisitDetail.AddCell(cell);


            phrase = new Phrase("RR: ", new Font(Font.HELVETICA, 6, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            tableVisitDetail.AddCell(cell);

            phrase = new Phrase(visitRR, new Font(Font.HELVETICA, 6, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            tableVisitDetail.AddCell(cell);


            phrase = new Phrase("BP: ", new Font(Font.HELVETICA, 6, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            tableVisitDetail.AddCell(cell);

            phrase = new Phrase(visitBPUpper + " / " + visitBPLower, new Font(Font.HELVETICA, 6, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            cell.Colspan = 3;
            tableVisitDetail.AddCell(cell);


            //row3------------------------------------------------------------------------------------------------
            phrase = new Phrase("Neck: ", new Font(Font.HELVETICA, 6, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            tableVisitDetail.AddCell(cell);

            phrase = new Phrase(decVisitNeck + " " + patientHeightMeasurment, new Font(Font.HELVETICA, 6, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            tableVisitDetail.AddCell(cell);


            phrase = new Phrase("Waist: ", new Font(Font.HELVETICA, 6, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            tableVisitDetail.AddCell(cell);

            phrase = new Phrase(decVisitWaist + " " + patientHeightMeasurment, new Font(Font.HELVETICA, 6, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            tableVisitDetail.AddCell(cell);


            phrase = new Phrase("Hip: ", new Font(Font.HELVETICA, 6, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            tableVisitDetail.AddCell(cell);

            phrase = new Phrase(decVisitHip + " " + patientHeightMeasurment, new Font(Font.HELVETICA, 6, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            tableVisitDetail.AddCell(cell);


            phrase = new Phrase("WHR: ", new Font(Font.HELVETICA, 6, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            tableVisitDetail.AddCell(cell);

            phrase = new Phrase(decVisitWHR + "", new Font(Font.HELVETICA, 6, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            tableVisitDetail.AddCell(cell);



            //row4------------------------------------------------------------------------------------------------
            phrase = new Phrase("Satiety Staging: ", new Font(Font.HELVETICA, 6, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            tableVisitDetail.AddCell(cell);

            phrase = new Phrase(visitSatietyStaging + " / 100" + "\n ", new Font(Font.HELVETICA, 6, 0));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            cell.Colspan = 7;
            tableVisitDetail.AddCell(cell);


            //visit clinical review-------------------------------------------------------------------------------
            //row1------------------------------------------------------------------------------------------------
            phrase = new Phrase("Clinical Review", new Font(Font.HELVETICA, 6, Font.BOLD));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            cell.Colspan = 2;
            tableVisitClinicReview.AddCell(cell);

            for (int Idx = 0; Idx < visitReviewArr.Length; Idx++)
            {
                clinicReview = visitReviewArr[Idx];
                clinicReviewResult = dsReport.Tables[0].Rows[0][clinicReview+"Review"].ToString();
                if (clinicReviewResult.Trim() != "")
                {
                    phrase = new Phrase(visitReviewTitleArr[Idx] + ": ", new Font(Font.HELVETICA, 6, 0));
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableVisitClinicReview.AddCell(cell);

                    phrase = new Phrase(clinicReviewResult, new Font(Font.HELVETICA, 6, 0));
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    tableVisitClinicReview.AddCell(cell);
                }
            }


            //row3------------------------------------------------------------------------------------------------
            phrase = new Phrase("\n\n\n\n\nSeen by: " + visitDoctor, new Font(Font.HELVETICA, 8, Font.BOLD));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            tableVisitSignature.AddCell(cell);

            if (rptParam[1] == "1")
            {
                phrase = new Phrase("\n\n\n\n\nSigned By: "+Request.Cookies["Logon_UserName"].Value, new Font(Font.HELVETICA, 8, Font.BOLD));
                cell = new Cell(phrase);
                cell.BorderWidth = 0;
                tableVisitSignature.AddCell(cell);
            }
            else
            {
                phrase = new Phrase("\n\n\n\n\nSigned By: ________________________", new Font(Font.HELVETICA, 8, Font.BOLD));
                cell = new Cell(phrase);
                cell.BorderWidth = 0;
                tableVisitSignature.AddCell(cell);
            }

            phrase = new Phrase("Visit Date: " + visitDate, new Font(Font.HELVETICA, 8, Font.BOLD));
            cell = new Cell(phrase);
            cell.BorderWidth = 0;
            tableVisitSignature.AddCell(cell);

            for (int rptIdx = 0; rptIdx < reportType.Length; rptIdx++)
            {
                if (rptParam[2][rptIdx].ToString() == "1")
                {
                    reportName = reportType[rptIdx];

                    if (firstReport == false)
                        oDoc.NewPage();
                    else
                        firstReport = false;

                    tableSuperCode.DeleteAllRows();
                    phrase = new Phrase(reportName, new Font(Font.HELVETICA, 10, Font.BOLD));
                    cell = new Cell(phrase);
                    cell.BorderWidth = 0;
                    cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    tableSuperCode.AddCell(cell);

                    oDoc.Add(tableLogo);
                    oDoc.Add(tableSuperCode);
                    oDoc.Add(tableHeader);
                    oDoc.Add(tableVisit);
                    oDoc.Add(tableVisitAdjustment);
                    oDoc.Add(tableVisitDetail);                    
                    oDoc.Add(tableVisitClinicReview);
                    oDoc.Add(tableVisitSignature);

                }
            }

            gClass.SaveActionLog(gClass.OrganizationCode,
                                 Request.Cookies["UserPracticeCode"].Value,
                                 Request.Url.Host,
                                 System.Configuration.ConfigurationManager.AppSettings["VisitPage"].ToString(),
                                 System.Configuration.ConfigurationManager.AppSettings["LogPrint"].ToString(),
                                 "Load " + System.Configuration.ConfigurationManager.AppSettings["ReportSB"].ToString(),
                                 Request.QueryString["PID"],
                                 rptParam[0]);
            oDoc.Close();
            Response.Redirect(openFilePath);
        }
        catch (Exception ex)
        {
            dsReport = dsReport;
        }
    }
    #endregion

    #region private void CreateOutputFile(DataSet dsReport)
    private void CreateOutputFile(string strReportName, DataSet dsReport)
    {
        try
        {
            ReportViewer rptViewer = new ReportViewer();
            string strOutFileName = "", strRender = "", mimeType, encoding, fileNameExtension;
            string[] streams;
            Microsoft.Reporting.WebForms.Warning[] warnings;
            string strFileName = @"\RDLFiles\" + CheckRDLFilesDirectory();

            switch (Request.QueryString["Format"])
            {
                case "1": //HTML
                    tcXML.InnerHtml += gClass.ShowSchema(dsReport, Server.MapPath(strXSLTFileName));
                    return;
                case "2": //EXCEL
                    strRender = "excel";
                    strOutFileName = Server.MapPath(".") + strFileName + ".xls";
                    break;
                case "3": //PDF
                    strRender = "pdf";
                    strOutFileName = Server.MapPath(".") + strFileName + ".pdf";
                    break;
                case "4": //WORD
                    strRender = "word";
                    strOutFileName = Server.MapPath(".") + strFileName + ".doc";
                    break;
            }

            rptViewer.LocalReport.DataSources.Clear();
            switch (strReportName)
            {
                case "COMPSUM":
                    strReportName = "Complication Summary";
                    ComplicationSummary_RDL(dsReport, Server.MapPath(".") + strFileName + ".rdlc", Request.Cookies["CultureInfo"].Value);
                    break;

                case "COMPSUMBYPATIENT":
                    strReportName = "Complication summary by patient";
                    ComplicationSummaryByPatient_RDL(dsReport, Server.MapPath(".") + strFileName + ".rdlc", Request.Cookies["CultureInfo"].Value);
                    break;

                case "PATIENTLIST":
                    strReportName = "Patient List with last visit date";
                    PatientList_RDL(dsReport, Server.MapPath(".") + strFileName + ".rdlc", Request.Cookies["CultureInfo"].Value);
                    break;

                case "COEREPORT":
                    strReportName = "Patient List with complication";
                    PatientListWithComplications_RDL(dsReport, Server.MapPath(".") + strFileName + ".rdlc", Request.Cookies["CultureInfo"].Value);
                    break;

                case "SUMMARYBYQUARTER":
                    strReportName = "Weight Loss";
                    SummaryByQuarter_RDL(dsReport, Server.MapPath(".") + strFileName + ".rdlc", Request.Cookies["CultureInfo"].Value);
                    break;

                case "SUMMARYONLY":
                    strReportName = "Summary Only";
                    SummaryByQuarter_RDL(dsReport, Server.MapPath(".") + strFileName + ".rdlc", Request.Cookies["CultureInfo"].Value);
                    break;

                case "OPERATIONLOS":
                    strReportName = "Operation duration with LOS";
                    OperationDurationWithLOS_RDL(dsReport, Server.MapPath(".") + strFileName + ".rdlc", Request.Cookies["CultureInfo"].Value);
                    break;

                case "OperationDetails":
                    strReportName = "Operation Detail List";
                    OperationDetailList_RDL(dsReport, Server.MapPath(".") + strFileName + ".rdlc", Request.Cookies["CultureInfo"].Value);
                    break;

                case "PatientProgress":
                    strReportName = "Patient Progress";
                    PatientProgress_RDL(dsReport, Server.MapPath(".") + strFileName + ".rdlc", Request.Cookies["CultureInfo"].Value);
                    break;

                case "PatientContact":
                    strReportName = "Patient Contact";
                    PatientContact_RDL(dsReport, Server.MapPath(".") + strFileName + ".rdlc", Request.Cookies["CultureInfo"].Value);
                    break;

                case "EWLG":
                    strReportName = "% EWL Graph";
                    EWLG_RDL(dsReport, Server.MapPath(".") + strFileName + ".rdlc", Request.Cookies["CultureInfo"].Value);
                    break;

                case "WLG":
                    strReportName = "WL Graph";
                    WLG_RDL(dsReport, Server.MapPath(".") + strFileName + ".rdlc", Request.Cookies["CultureInfo"].Value);
                    break;

                case "IEWLG":
                    strReportName = "IEWLG Graph";
                    IEWLG_RDL(dsReport, Server.MapPath(".") + strFileName + ".rdlc", Request.Cookies["CultureInfo"].Value);
                    break;

                case "REPPATIENTLIST":
                    strReportName = "Patient List";
                    RepPatientList_RDL(dsReport, Server.MapPath(".") + strFileName + ".rdlc", Request.Cookies["CultureInfo"].Value);
                    break;

                case "REPOPERATIONLIST":
                    strReportName = "Operation List";
                    RepOperationList_RDL(dsReport, Server.MapPath(".") + strFileName + ".rdlc", Request.Cookies["CultureInfo"].Value);
                    break;

                case "REPVISITLIST":
                    strReportName = "Visit List";
                    RepVisitList_RDL(dsReport, Server.MapPath(".") + strFileName + ".rdlc", Request.Cookies["CultureInfo"].Value);
                    break;

                case "BSRREPORT":
                    strReportName = "BSR Report";
                    BSRReport_RDL(dsReport, Server.MapPath(".") + strFileName + ".rdlc", Request.Cookies["CultureInfo"].Value);
                    break;
            }
            foreach (DataTable dt in dsReport.Tables)
                rptViewer.LocalReport.DataSources.Add(new ReportDataSource(dt.TableName, dt));

            rptViewer.LocalReport.ReportPath = Server.MapPath(".") + strFileName + ".rdlc";
            rptViewer.LocalReport.EnableExternalImages = true;
            rptViewer.ProcessingMode = ProcessingMode.Local;
            rptViewer.LocalReport.Refresh();

            byte[] fileContent = rptViewer.LocalReport.Render(strRender, null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            
            switch (Request.QueryString["Format"])
            {
                case "2": //EXCEL
                    Response.ContentType = "Application/x-msexcel";
                    Response.AppendHeader("Content-Disposition", "inline;filename=" + Request.Url.Scheme + "://" + Request.Url.Host + "/Reports" + strFileName + ".xls");
                    break;
                case "3": //PDF
                    Response.ContentType = "Application/pdf";
                    //Response.AddHeader("Content-Type", "application/pdf");
                    Response.AppendHeader("Content-Disposition", "inline;filename=" + Request.Url.Scheme + "://" + Request.Url.Host + "/Reports" + strFileName + ".pdf");
                    break;
                case "4": //WORD
                    Response.ContentType = "Application/msword";
                    //Response.AddHeader("Content-Type", "application/pdf");
                    Response.AppendHeader("Content-Disposition", "inline;filename=" + Request.Url.Scheme + "://" + Request.Url.Host + "/Reports" + strFileName + ".doc");
                    break;
            }

            System.IO.File.WriteAllBytes(strOutFileName, fileContent);
            Response.BinaryWrite(fileContent);
            //Response.End();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value,
                                "CreateOutputFile", "CreateOutputFile function", err.ToString());
        }
    }
    #endregion 

    #region private string CheckRDLFilesDirectory()
    private string CheckRDLFilesDirectory()
    {
        bool flag = true;
        string strFilename = "";

        System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(Server.MapPath(".") + @"\RDLFiles");
        if (di.Exists == false) di.Create();
        else
        {
            string[] strFileNames = System.IO.Directory.GetFiles(Server.MapPath(".") + @"\RDLFiles");

            for (int idx = 0; idx < strFileNames.Length; idx++)
                try { System.IO.File.Delete(strFileNames[idx]); }
                catch { }
        }
        Random numRandom = new Random();

        while (flag)
        {
            strFilename = Server.MapPath(".") + @"\RDLFiles\" + numRandom.Next(Int16.MaxValue).ToString();
            flag = System.IO.File.Exists(strFilename + ".*");
        }
        return strFilename.Replace(Server.MapPath(".") + @"\RDLFiles\", "");
    }
    #endregion

    #region public void AddReportConfiguration( ref System.Xml.XmlTextWriter writer, string strReportDescription, string strLanguage, string strPageWidth, string strLeftMargin, string strTopMargin, string strBottomMargin, string strRightMarghin, string strHeight, string strMeasurment)
    private void AddReportConfiguration(ref System.Xml.XmlTextWriter writer, string strReportDescription,string strLanguage,
                                        Decimal PageWidth, Decimal LeftMargin, Decimal TopMargin,
                                        Decimal BottomMargin, Decimal RightMarghin, Decimal Height, string strMeasurment)
    {
        Decimal ReportWidth = PageWidth - (LeftMargin + RightMarghin);

        writer.WriteAttributeString("xmlns", null, "http://schemas.microsoft.com/sqlserver/reporting/2005/01/reportdefinition");
        writer.WriteAttributeString("xmlns:rd", null, "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner");
        writer.WriteAttributeString("Description", null, strReportDescription);
        writer.WriteAttributeString("Author", null, "Lapbase data system");
        writer.WriteElementString("rd:ReportID", Guid.NewGuid().ToString().Replace("-", ""));
        writer.WriteElementString("Language", strLanguage);
        writer.WriteElementString("Width", ReportWidth.ToString() + strMeasurment); // 28
        writer.WriteElementString("TopMargin", TopMargin + strMeasurment); // 2.5
        writer.WriteElementString("LeftMargin", LeftMargin + strMeasurment); //2.5
        writer.WriteElementString("RightMargin", RightMarghin + strMeasurment);//2.5
        writer.WriteElementString("BottomMargin", BottomMargin + strMeasurment);//2.5
        writer.WriteElementString("PageWidth", PageWidth.ToString() + strMeasurment);//29 615pt
        writer.WriteElementString("PageHeight", Height + strMeasurment); //25 1008pt
        writer.WriteElementString("rd:DrawGrid", "true");
        writer.WriteElementString("rd:GridSpacing", "0.1cm");//0.25
        writer.WriteElementString("rd:SnapToGrid", "true");
        writer.WriteElementString("DataTransform", strXSLTFileName);
    }
    #endregion

    #region public void AddDataSource(ref System.Xml.XmlTextWriter writer, DataSet dsReport)
    public void AddDataSource(ref System.Xml.XmlTextWriter writer, DataSet dsReport)
    {
        writer.WriteStartElement("DataSources");
        writer.WriteStartElement("DataSource");
        writer.WriteAttributeString("Name", null, strLapbaseDataSourceName);

        writer.WriteStartElement("ConnectionProperties");
        writer.WriteElementString("ConnectString", GlobalClass.strSqlCnnString);
        writer.WriteElementString("DataProvider", "SQL");
        writer.WriteEndElement();

        writer.WriteEndElement();
        writer.WriteEndElement();
    }
    #endregion

    #region private void AddDataSets(ref System.Xml.XmlTextWriter writer, DataSet dsReport)
    private void AddDataSets(ref System.Xml.XmlTextWriter writer, DataSet dsReport)
    {
        writer.WriteStartElement("DataSets");
        foreach (DataTable dt in dsReport.Tables)
        {
            writer.WriteStartElement("DataSet");
            writer.WriteAttributeString("Name", null, dt.TableName);
            writer.WriteStartElement("Query");
            writer.WriteStartElement("CommandText");
            writer.WriteEndElement();
            writer.WriteElementString("DataSourceName", strLapbaseDataSourceName);
            writer.WriteEndElement();

            writer.WriteStartElement("Fields");
            foreach (DataColumn dc in dt.Columns)
            {
                writer.WriteStartElement("Field");
                writer.WriteAttributeString("Name", null, dc.ColumnName);
                writer.WriteElementString("rd:TypeName", dc.DataType.ToString());
                writer.WriteElementString("DataField", dc.ColumnName);
                writer.WriteEndElement();
            }
            writer.WriteEndElement(); // Fields

            writer.WriteEndElement(); // DataSet
        }
        writer.WriteEndElement(); // DataSets
    }
    #endregion 

    #region private void AddColumn(ref System.Xml.XmlTextWriter writer, string strWidth)
    private void AddColumn(ref System.Xml.XmlTextWriter writer, string strWidth)
    {
        writer.WriteStartElement("TableColumn");
        writer.WriteElementString("Width", strWidth);
        writer.WriteEndElement();
    }
    #endregion

    #region private List<string[,]> DetailCellStyle(string strColor)
    private List<string[,]> DetailCellStyle(string strColor, string strFontSize, string strTextAlign, string strBackgroundColor)
    {
        List<string[,]> strStyles = new List<string[,]>();

        strStyles.Add(new string[,] { { "Color", strColor.Equals("") ? "Black" : strColor } });
        strStyles.Add(new string[,] { { "FontSize", strFontSize.Equals("") ? "8pt" : strFontSize } });
        strStyles.Add(new string[,] { { "TextAlign", strTextAlign.Equals("") ? "Left" : strTextAlign } });
        strStyles.Add(new string[,] { { "BackgroundColor", strBackgroundColor.Equals("") ? "White" : strBackgroundColor } });
        strStyles.Add(new string[,] { { "VerticalAlign", "Middle"} });
        
        return (strStyles);
    }
    #endregion

    #region private List<string[,]> ElementChilds(string strWidth, string strHeight, string strCanGrow, string strZIndex, string strTop, string strLeft)
    private List<string[,]> ElementChilds(string strWidth, string strHeight, string strCanGrow, string strZIndex, string strTop, string strLeft)
    {
        List<string[,]> strChilds = new List<string[,]>();

        if (!strWidth.Equals(String.Empty))     strChilds.Add(new string[,] { { "Width", strWidth } });
        if (!strHeight.Equals(String.Empty))    strChilds.Add(new string[,] { { "Height", strHeight } });
        if (!strCanGrow.Equals(String.Empty))   strChilds.Add(new string[,] { { "CanGrow", strCanGrow } });
        if (!strZIndex.Equals(String.Empty))    strChilds.Add(new string[,] { { "ZIndex", strZIndex } });
        if (!strTop.Equals(String.Empty))       strChilds.Add(new string[,] { { "Top", strTop } });
        if (!strLeft.Equals(String.Empty))      strChilds.Add(new string[,] { { "Left", strLeft } });

        return (strChilds);
    }
    #endregion

    #region private void AddCell(ref System.Xml.XmlTextWriter writer, string strReportItem, string strReportItemName, string strReportItemValue, List<string[,]> strStyles, , string strColSpan)
    private void AddCell(ref System.Xml.XmlTextWriter writer, string strReportItem, string strReportItemName, string strReportItemValue, List<string[,]> strStyles, string strColSpan)
    {
        writer.WriteStartElement("TableCell");
        if (!(strColSpan.Equals("") || strColSpan.Equals("0"))) writer.WriteElementString("ColSpan", strColSpan);
        writer.WriteStartElement("ReportItems");
        writer.WriteStartElement(strReportItem);
        writer.WriteAttributeString("Name", strReportItemName);

        switch (strReportItem.ToUpper())
        {
            case "TEXTBOX":
                writer.WriteElementString("Value", strReportItemValue);
                break;
            case "IMAGE":
                writer.WriteElementString("Value", strReportItemValue);
                writer.WriteElementString("Source", "External");
                break;
        }

        if (strStyles.Count > 0)
        {
            writer.WriteStartElement("Style");
            foreach (string[,] strStyle in strStyles)
            {
                writer.WriteElementString(strStyle[0, 0], strStyle[0, 1]);
            }
            writer.WriteEndElement();
        }
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndElement();
    }
    #endregion 

    #region private void AddReportItem(ref System.Xml.XmlTextWriter writer, string strReportItem, string strReportItemName, string strReportItemValue, List<string[,]> strChildElements)
    private void AddReportItem(ref System.Xml.XmlTextWriter writer, string strReportItem, string strReportItemName, string strReportItemValue, List<string[,]> strChildElements, List<string[,]> strStyles)
    {
        writer.WriteStartElement(strReportItem);
        writer.WriteAttributeString("Name", strReportItemName);

        switch (strReportItem.ToUpper())
        {
            case "TEXTBOX":
                writer.WriteElementString("Value", strReportItemValue);
                break;
            case "IMAGE":
                writer.WriteElementString("Value", strReportItemValue);
                writer.WriteElementString("Source", "External");
                break;
        }

        if (strChildElements.Count > 0)
            foreach (string[,] strChildElement in strChildElements)
                writer.WriteElementString(strChildElement[0, 0], strChildElement[0, 1]);

        if (strStyles.Count > 0)
        {
            writer.WriteStartElement("Style");
            foreach (string[,] strStyle in strStyles)
            {
                writer.WriteElementString(strStyle[0, 0], strStyle[0, 1]);
            }
            writer.WriteEndElement();
        }
        writer.WriteEndElement();
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

    #region private String getDescription(string code, string category){
    private String getDescription(string code, string category)
    {
        try
        {
            if (category == "com")
                return codeComDesc[code];
            else if (category == "comQuest")
                return codeComQuestDesc[code];
            else if (category == "comRank")
                return codeComRank[code];
            else if (category == "adev")
                return codeAdevDesc[code];
            else if (category == "pbs")
                return codePBSDesc[code];
            else if (category == "pbns")
                return codePBNSDesc[code];
            else if (category == "demo")
                return codeDemoDesc[code];
            else if (category == "emp")
                return codeEMPDesc[code];
            else if (category == "dwl")
                return codeDWLDesc[code];
            else if (category == "asa")
                return codeASADesc[code];
            else if (category == "contact")
                return codeContactDesc[code];
            else if (category == "assistant")
                return codeAssistantDesc[code];
            else if (category == "concurrent")
                return codeConcurrentDesc[code];
            else if (category == "reason")
                return codeReasonDesc[code];
            else if (category == "adevst")
                return codeAdevSTDesc[code];
            else if (category == "background")
                return codeBackgroundDesc[code];
            else
                return "";
        }
        catch (Exception err)
        {
            return "";
        }

    }
    #endregion

    #region private Dictionary<string, string> buildCodeDictionary(string categoryCode, string type)
    private Dictionary<string, string> buildCodeDictionary(string categoryCode, string type)
    {
        SqlCommand cmdSelect = new SqlCommand();
        DataSet dsCode = new DataSet();
        Dictionary<string, string> tempCodeDesc = new Dictionary<string, string>();

        try
        {
            if (categoryCode == "contact")
            {

                tempCodeDesc.Add("Y", "Yes");
                tempCodeDesc.Add("N", "No");
                tempCodeDesc.Add("O", "Once");
                tempCodeDesc.Add("T", "Twice");
                tempCodeDesc.Add("NVR", "Never");
            }
            else if(categoryCode == "assistant")
            {
                tempCodeDesc.Add("chkAssistantNone", "None");
                tempCodeDesc.Add("chkAssistantPA", "PA/ NP/ RNFA");
                tempCodeDesc.Add("chkAssistantJunior", "Junior Resident (PGY 1-3)");
                tempCodeDesc.Add("chkAssistantSenior", "Senior Resident (PGY 4-5)");
                tempCodeDesc.Add("chkAssistantMIS", "MIS Fellow");
                tempCodeDesc.Add("chkAssistantAttendingSurgeon", "Attending - Weight Loss Surgeon");
                tempCodeDesc.Add("chkAssistantAttendingOther", "Attending - Other ");
            }
            else if (categoryCode == "background")
            {
                tempCodeDesc.Add("F", "Father");
                tempCodeDesc.Add("M", "Mother");
                tempCodeDesc.Add("S", "Sibling / Child");
                tempCodeDesc.Add("N", "No Family History");
                tempCodeDesc.Add("D", "Don't Know");
            }
            else
            {
                gClass.MakeStoreProcedureName(ref cmdSelect, "sp_Codes_LoadAllData", true);
                cmdSelect.Parameters.Add("@CategoryCode", SqlDbType.VarChar).Value = categoryCode;
                dsCode = gClass.FetchData(cmdSelect, "tblCodes");

                for (int Xh = 0; Xh < dsCode.Tables[0].Rows.Count; Xh++)
                {
                    if (type == "rank")
                        tempCodeDesc.Add(dsCode.Tables[0].Rows[Xh]["Code"].ToString(), dsCode.Tables[0].Rows[Xh]["Rank"].ToString());
                    else
                        tempCodeDesc.Add(dsCode.Tables[0].Rows[Xh]["Code"].ToString(), dsCode.Tables[0].Rows[Xh]["Description"].ToString());
                }
            }
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Load Code Description", "Load Code Description", err.ToString());
        }

        return tempCodeDesc;
    }
    #endregion

    #region private int IndexOfOccurence(string s, string match, int occurence)
    private int IndexOfOccurence(string s, string match, int occurence)
    {
        int i = 1;
        int index = 0;
        while (i <= occurence && (index = s.IndexOf(match, index + 1)) != -1)
        {
            if (i == occurence)
                return index;

            i++;
        }
        return -1;
    }
    #endregion

    #region private double getStandardDeviation(List<double> doubleList,double average)
    private double getStandardDeviation(List<double> doubleList, double average)
    {
        double sumOfDerivation = 0;
        foreach (double value in doubleList)
        {
            sumOfDerivation += (value - average) * (value - average);
        }
        double sumOfDerivationAverage = sumOfDerivation / doubleList.Count;
        return Math.Sqrt(sumOfDerivationAverage);
    }
    #endregion
}