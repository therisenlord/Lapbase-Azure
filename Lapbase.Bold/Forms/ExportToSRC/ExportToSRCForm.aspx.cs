using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Lapbase.Configuration.ConfigurationApplication;

public partial class Forms_ExportToSRC_ExportToSRCForm : Lapbase.Business.BasePage
{
    GlobalClass gClass = new GlobalClass();
    String[] PreOpComorbidityList = { "CVS_Hypertension", "CVS_Congestive", "CVS_Ischemic", "CVS_Angina", "CVS_Peripheral", "CVS_Lower", "CVS_DVT", "MET_Glucose", "MET_Lipids", "MET_Gout", "PUL_Obstructive", "PUL_Obesity", "PUL_PulHypertension", "PUL_Asthma", "GAS_Gerd", "GAS_Cholelithiasis", "GAS_Liver", "MUS_BackPain", "MUS_Musculoskeletal", "MUS_Fibromyalgia", "REPRD_Polycystic", "REPRD_Menstrual", "PSY_Impairment", "PSY_Depression", "PSY_MentalHealth", "PSY_Alcohol", "PSY_Tobacoo", "PSY_Abuse", "GEN_Stress", "GEN_Cerebri", "GEN_Hernia", "GEN_Functional", "GEN_Skin" };
    String[] PostOpComorbidityList = { "CVS_Hypertension", "CVS_Congestive", "CVS_Ischemic", "CVS_Angina", "CVS_Peripheral", "CVS_Lower", "CVS_DVT", "MET_Glucose", "MET_Lipids", "MET_Gout", "PUL_Obstructive", "PUL_Obesity", "PUL_PulHypertension", "PUL_Asthma", "GAS_Gerd", "GAS_Cholelithiasis", "GAS_Liver", "MUS_BackPain", "MUS_Musculoskeletal", "MUS_Fibromyalgia", "REPRD_Polycystic", "REPRD_Menstrual", "PSY_Impairment", "PSY_Depression", "PSY_MentalHealth", "PSY_Alcohol", "PSY_Tobacoo", "PSY_Abuse", "GEN_Stress", "GEN_Cerebri", "GEN_Hernia", "GEN_Functional", "GEN_Skin" };
    String deceasedCode = "DAA1062";

    String OpVisitExist = " &nbsp; &nbsp; <b>Operative Visit already Exist on BOLD Database</b><br/>";
    String PreOpVisitExist = " &nbsp; &nbsp; <b>Pre-Operative Visit already Exist on BOLD Database</b><br/>";
    String PostOpVisitExist = " &nbsp; &nbsp; <b>Post-Operative Visit already Exist on BOLD Database</b><br/>";
    String OpVisitBefore = " can not add visit ";


    String OpVisitSigned = " &nbsp; &nbsp; <b>Operative Visit already Exist and Signed on BOLD Database</b><br/>";

    DataSet dsValidate = new DataSet();
    DateTime dtLastPreOpVisit = new DateTime();
    DateTime dtOpVisit = new DateTime();
    DateTime dtLastPostOpVisit = new DateTime();
    Boolean blnOpSent = false;
    Boolean blnOpFlag = false;

    String strLastPreOpVisit;
    String strOpVisit;
    String strLastPostOpVisit;
    
    DataSet dsPatient = new DataSet();
    DataSet dsPreOpVisit = new DataSet();
    DataSet dsOpVisit = new DataSet();
    DataSet dsPostOpVisit = new DataSet();
    DataSet dsAdverseEvent = new DataSet();

    Boolean blnValidPatientData = false;
    Boolean blnValidPreOpVisit = false;
    Boolean blnValidPostOpVisit = false;
    Boolean blnValidOpVisit = false;
    Boolean blnValidAdverseEvent = false;
    Boolean blnSucceedFlag = false;


    #region Events
    protected override void OnLoad(EventArgs e)
    {
        SqlCommand cmdSelect = new SqlCommand();

        gClass.OrganizationCode = Request.Cookies["OrganizationCode"].Value;

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_SRCOperativeVisitCheckLast", false);
        cmdSelect.Parameters.Add("@vintOrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSelect.Parameters.Add("@vintPatientId", System.Data.SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
        dsValidate = gClass.FetchData(cmdSelect, "tblPatient");
        if (dsValidate.Tables.Count > 0)
        {
            DateTime.TryParse(dsValidate.Tables[0].Rows[0]["LastPreOpVisit"].ToString(), out dtLastPreOpVisit);
            DateTime.TryParse(dsValidate.Tables[0].Rows[0]["OpVisit"].ToString(), out dtOpVisit);
            DateTime.TryParse(dsValidate.Tables[0].Rows[0]["LastPostOpVisit"].ToString(), out dtLastPostOpVisit);
            //new BOLD version have no "signing patient" anymore. Which means, no data locking
            Boolean.TryParse(dsValidate.Tables[0].Rows[0]["OpSent"].ToString(), out blnOpSent);           
            //Boolean.TryParse(dsValidate.Tables[0].Rows[0]["OpFlag"].ToString(), out blnOpFlag);
        }

        //format last visit date
        strLastPreOpVisit = dtLastPreOpVisit.ToString("dd-MM-yyyy");
        strOpVisit = dtOpVisit.ToString("dd-MM-yyyy");
        strLastPostOpVisit = dtLastPostOpVisit.ToString("dd-MM-yyyy");


        validateSRCData();
        //base.OnLoad(e);
    }
    #endregion
        
    #region private void validateSRCData();
    private void validateSRCData()
    {
        clearSRCError();
        dsPatient = validatePatientDetail();
        dsPreOpVisit = validatePreOpVisit();
        dsOpVisit = validateOpVisit();
        dsPostOpVisit = validatePostOpVisit();
        dsAdverseEvent = validateAdverseEvent();


        //if (blnValidPatientData == true && blnValidPreOpVisit == true && blnValidOpVisit == true)
        //{
            btnSyncSRC.Disabled = false;
            btnSignSRC.Disabled = false;
       // }
    }
    #endregion

    #region private DataSet validatePatientDetail();
    private DataSet validatePatientDetail()
    {
        SqlCommand cmdSelect = new SqlCommand();
        DataSet dsPatient = new DataSet();

        String patientDataErrorMsg = " &nbsp; &nbsp; There is no patient with this ID<br/>";
        String patientDataValidMsg = " &nbsp; &nbsp; Valid patient data<br/>";
        String patientDataChartMsg = " &nbsp; &nbsp; Please specify patient's chart number";
        String patientDataRaceMsg = " &nbsp; &nbsp; Please specify patient's ethnicity";
        String patientDataEmploymentMsg = " &nbsp; &nbsp; Please specify patient's employment status";
        String patientDataDOBMsg = " &nbsp; &nbsp; Please specify patient's date of birth";


        //validate Patient Data
        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_SRCPatientDataGet", false);
        cmdSelect.Parameters.Add("@vintOrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSelect.Parameters.Add("@vintPatientId", System.Data.SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
        dsPatient = gClass.FetchData(cmdSelect, "tblPatient");
        if (dsPatient.Tables.Count > 0)
        {
            patientDataErrorMsg = "";
            patientDataErrorMsg += dsPatient.Tables[0].Rows[0]["ChartNumber"].ToString() != "" ? "" : patientDataChartMsg + "<br/>";
            patientDataErrorMsg += dsPatient.Tables[0].Rows[0]["RACE"].ToString() != "" ? "" : patientDataRaceMsg + "<br/>";
            patientDataErrorMsg += dsPatient.Tables[0].Rows[0]["EmploymentStatusCode"].ToString() != "" ? "" : patientDataEmploymentMsg + "<br/>";
            patientDataErrorMsg += dsPatient.Tables[0].Rows[0]["YearOfBirth"].ToString() != "" ? "" : patientDataDOBMsg + "<br/>";
        }

        //if patient data = valid
        if (patientDataErrorMsg == "")
            blnValidPatientData = true;

        lblValidatePatientData.Text = patientDataErrorMsg == "" ? "<font color='blue'>" + patientDataValidMsg + "</font>" : "<font color='red'>" + patientDataErrorMsg + "</font>";

        cmdSelect.Dispose();

        return dsPatient;
    }
    #endregion

    #region private DataSet validatePreOpVisit();
    private DataSet validatePreOpVisit()
    {
        // validate Pre-Operative Visit
        SqlCommand cmdSelect = new SqlCommand();
        DataSet dsPreOpVisit = new DataSet();

        String patientPreOpErrorMsg = " &nbsp; &nbsp; There is no pre-operative visit recorded for this patient<br/>";
        String patientPreOpValidMsg = " &nbsp; &nbsp; Valid patient pre-operative visit record<br/>";
        String patientPreOpHeightMsg = " &nbsp; &nbsp; Please specify patient's height";
        String patientPreOpWeightMsg = " &nbsp; &nbsp; Please specify patient's weight";
        String patientPreOpComorbidityMsg = " &nbsp; &nbsp; Please complete patient's comorbidity check";
        String patientOpVisitExistMsg = " &nbsp; &nbsp; Hospital Visit has been recorded on BOLD Database, can not add another pre-operative visit record";
        String patientPreOpBeforeMsg = " &nbsp; &nbsp; Last pre-operative visit added to BOLD is ";
        String patientPreOpWarningMsg = "";

        DateTime dtTemp = new DateTime();
        Decimal decTemp = 0;
        String strTemp = "";
        String currCom = "";

        Boolean checkSentOp = false;
        Boolean checkSentPreOp = false;


        /* ----------------------------------------------------------------------------------
         * get all new pre-op visit
         * check if record has been signed
         * check if op visit has been sent         
         *      yes --> "op visit has been sent"
         * loop for pre-op, and find any with BoldVisitID null
         *      if "op visit has been sent", put in warning
         *      else check if we already got pre-op visit sent to bold
         *          yes --> check if pre-op date < last pre-op sent to bold
         *              yes --> "can not add pre-op before last pre-op"         
         *          check if != "can not add pre-op before last pre-op"
         *              yes --> validate input
         * 
         *      check if already sent pre-op before
         *          yes --> check if != "op visit has been sent"
         *                      yes --> "pre-op exist on Bold"
        ---------------------------------------------------------------------------------- */

        //** get all new pre-op visit
        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_SRCPreOperativeVisitDataGet", false);
        cmdSelect.Parameters.Add("@vintOrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSelect.Parameters.Add("@vintPatientID", System.Data.SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
        cmdSelect.Parameters.Add("@vblnImperial", SqlDbType.Bit).Value = Request.Cookies["Imperial"].Value.Equals("True");
        cmdSelect.Parameters.Add("@vblnPreOperative", SqlDbType.Bit).Value = true;
        dsPreOpVisit = gClass.FetchData(cmdSelect, "tblPreOpVisit");


        //** check if op visit has been signed 
        if (blnOpFlag)
        {
            checkSentOp = true;
            patientPreOpErrorMsg = "";
            patientPreOpValidMsg = OpVisitSigned;
        }
        else
        {

            //** check if op visit has been sent 
            if (blnOpSent)
            {
                //** yes --> "op visit has been sent"
                checkSentOp = true;
                patientPreOpErrorMsg = "";
                patientPreOpValidMsg = OpVisitExist;
            }

            if (dsPreOpVisit.Tables.Count > 0)
            {
                if (dsPreOpVisit.Tables[0].Rows.Count > 0)
                {
                    patientPreOpErrorMsg = "";
                    //** loop for all pre-operative visit
                    for (int iCountPreOp = 0; iCountPreOp < dsPreOpVisit.Tables[0].Rows.Count; iCountPreOp++)
                    {
                        checkSentPreOp = false;

                        //format visit date
                        if (DateTime.TryParse(dsPreOpVisit.Tables[0].Rows[iCountPreOp]["VisitDate"].ToString(), out dtTemp))
                            strTemp = dtTemp.ToString("dd-MM-yyyy");

                        //** if "op visit has been sent", put in warning
                        if (checkSentOp == true && dsPreOpVisit.Tables[0].Rows[iCountPreOp]["BoldVisitID"].ToString() == "")
                            patientPreOpWarningMsg += patientOpVisitExistMsg + " (" + strTemp + ")<br />";
                        else
                        {
                            //** else check if we already got pre-op visit sent to bold
                            if (dtLastPreOpVisit > new DateTime())
                            {
                                //** yes --> check if pre-op date < last pre-op sent to bold
                                if (dtTemp < dtLastPreOpVisit && dsPreOpVisit.Tables[0].Rows[iCountPreOp]["BoldVisitID"].ToString() == "")
                                {
                                    //** yes --> "can not add pre-op before last pre-op"
                                    checkSentPreOp = true;
                                    patientPreOpWarningMsg += patientPreOpBeforeMsg + strLastPreOpVisit + OpVisitBefore + strTemp + "<br />"; ;
                                }
                            }

                            //** check if != "can not add pre-op before last pre-op"
                            if (checkSentPreOp == false)
                            {
                                //** yes --> validate input

                                //convert height
                                Decimal.TryParse(dsPreOpVisit.Tables[0].Rows[iCountPreOp]["Height"].ToString(), out decTemp);
                                patientPreOpErrorMsg += decTemp > 0 ? "" : patientPreOpHeightMsg + " for visit " + strTemp + "<br />";

                                //convert weight
                                Decimal.TryParse(dsPreOpVisit.Tables[0].Rows[iCountPreOp]["Weight"].ToString(), out decTemp);
                                patientPreOpErrorMsg += decTemp > 0 ? "" : patientPreOpWeightMsg + " for visit " + strTemp + "<br />";

                                for (int iCountComList = 0; iCountComList < PreOpComorbidityList.Length; iCountComList++)
                                {
                                    //check each comorbidity check
                                    currCom = PreOpComorbidityList[iCountComList];
                                    if (dsPreOpVisit.Tables[0].Rows[iCountPreOp][currCom].ToString() == "")
                                    {
                                        patientPreOpErrorMsg += patientPreOpComorbidityMsg + " for visit " + strTemp + "<br />";
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //** check if already sent pre-op before
            if (dtLastPreOpVisit > new DateTime())
            {
                if (blnOpSent == false)
                {
                    patientPreOpErrorMsg = "";
                    patientPreOpValidMsg = PreOpVisitExist;
                }
            }

            //if PreOpVisit = valid
            if (patientPreOpErrorMsg == "")
                blnValidPreOpVisit = true;
        }
        cmdSelect.Dispose();

        lblValidatePreOpWarning.Text = patientPreOpWarningMsg == "" ? "" : "<font color='saddlebrown'>" + patientPreOpWarningMsg + "</font>";
        lblValidatePreOp.Text = patientPreOpErrorMsg == "" ? "<font color='blue'>" + patientPreOpValidMsg + "</font>" : "<font color='red'>" + patientPreOpErrorMsg + "</font>";

        return dsPreOpVisit;
    }
    #endregion
    
    #region private DataSet validateOpVisit();
    private DataSet validateOpVisit()
    {
        //validate Operative Visit
        SqlCommand cmdSelect = new SqlCommand();
        DataSet dsOpVisit = new DataSet();
        Int32 surgeryDuration;
        Int32 anesthesiaDuration;
        DateTime admisionDate = new DateTime();
        DateTime surgeryDate = new DateTime();
        DateTime dischargeDage = new DateTime();
        DateTime lastWeightDate = new DateTime();

        String patientOpErrorMsg = " &nbsp; &nbsp; There is no operative visit recorded for this patient<br/>";
        String patientOpValidMsg = " &nbsp; &nbsp; Valid patient operative visit record<br/>";
        String patientOpPreOpMsg = " &nbsp; &nbsp; At least 1 pre-operative visit has to exist and completed";
        String patientOpSurgeryNotValidMsg = " &nbsp; &nbsp; Please specify a valid surgery. The Surgery type you specified is not BOLD compliance";
        String patientOpApproachMsg = " &nbsp; &nbsp; Please specify surgery approach";
        String patientOpSurgeryAdmisionDateMsg = " &nbsp; &nbsp; The admission date should come after the date of the last pre-operative visit";
        String patientOpSurgeryAfterMsg = " &nbsp; &nbsp; The date of surgery must be equal to or after the date of admission";
        String patientOpAnesthesiaMsg = " &nbsp; &nbsp; The duration of anesthesia must be greater than or equal to the duration of surgery";
        String patientOpDischargeAfterMsg = " &nbsp; &nbsp; Discharge date must be greater than or equal to the date of surgery";
        String patientOpDVTMsg = " &nbsp; &nbsp; Please specify DVT Prophylaxis Therapys during the surgery";
        String patientOpASAMsg = " &nbsp; &nbsp; Please specify patient's ASA classification code";
        String patientOpConcurrentMsg = " &nbsp; &nbsp; Please specify concurrent procedure during surgery";
        String patientOpDischargeMsg = " &nbsp; &nbsp; Please specify patient's discharge location";


        /* ----------------------------------------------------------------------------------
         * check if op visit has been signed 
         * check if op visit has been sent         
         *      yes --> "op visit has been sent"
         *      no  --> get op visit
         *              check if we already sent "last pre op" or about to send one 
         *                  yes --> validate input
         *                  no  --> error "should have last pre op"
        ---------------------------------------------------------------------------------- */

        //** get op visit
        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_SRCHospitalVisitGet", false);
        cmdSelect.Parameters.Add("@vintOrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSelect.Parameters.Add("@vintPatientId", System.Data.SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
        dsOpVisit = gClass.FetchData(cmdSelect, "tblOpVisit");



                //** check if op visit has been signed 
        if (blnOpFlag)
        {
            patientOpErrorMsg = "";
            patientOpValidMsg = OpVisitSigned;
        }
        else
        {
            //** check if op visit has been sent
            if (blnOpSent)
            {
                //** yes --> "op visit has been sent"
                patientOpErrorMsg = "";
                patientOpValidMsg = OpVisitExist;
            }
            else
            {
                //** no  --> check if we already sent "last pre op" or about to send one 
                if (dsPreOpVisit.Tables.Count > 0 || dtLastPreOpVisit > new DateTime())
                {
                    if (dsOpVisit.Tables[0].Rows.Count > 0)
                    {
                        patientOpErrorMsg = "";
                        if (dsOpVisit.Tables[0].Rows[0]["BariatricProcedureCode"].ToString().IndexOf("ADD") != -1)
                            patientOpErrorMsg += patientOpSurgeryNotValidMsg + "<br/>";
                        
                        //** yes --> validate input
                        patientOpErrorMsg += dsOpVisit.Tables[0].Rows[0]["BariatricTechniqueCode"].ToString() != "" ? "" : patientOpApproachMsg + "<br/>";

                        DateTime.TryParse(dsOpVisit.Tables[0].Rows[0]["AdmitDate"].ToString(), out admisionDate);
                        DateTime.TryParse(dsOpVisit.Tables[0].Rows[0]["SurgeryDate"].ToString(), out surgeryDate);
                        DateTime.TryParse(dsOpVisit.Tables[0].Rows[0]["DischargeDate"].ToString(), out dischargeDage);
                        DateTime.TryParse(dsOpVisit.Tables[0].Rows[0]["DateOfLastWeight"].ToString(), out lastWeightDate);

                        if (dsOpVisit.Tables[0].Rows[0]["DateOfLastWeight"].ToString() == "")
                            patientOpErrorMsg += patientOpPreOpMsg + "<br/>";
                        else
                            patientOpErrorMsg += (admisionDate > lastWeightDate) ? "" : patientOpSurgeryAdmisionDateMsg + "<br/>";

                        patientOpErrorMsg += surgeryDate >= admisionDate ? "" : patientOpSurgeryAfterMsg + "<br/>";
                        patientOpErrorMsg += dischargeDage >= surgeryDate ? "" : patientOpDischargeAfterMsg + "<br/>";

                        Int32.TryParse(dsOpVisit.Tables[0].Rows[0]["Duration"].ToString(), out surgeryDuration);
                        Int32.TryParse(dsOpVisit.Tables[0].Rows[0]["DurationOfAnesthesia"].ToString(), out anesthesiaDuration);
                        patientOpErrorMsg += anesthesiaDuration > surgeryDuration ? "" : patientOpAnesthesiaMsg + "<br/>";

                        patientOpErrorMsg += dsOpVisit.Tables[0].Rows[0]["DVTProphylaxisTherapyCodes"].ToString() != "" ? "" : patientOpDVTMsg + "<br/>";
                        patientOpErrorMsg += dsOpVisit.Tables[0].Rows[0]["ASAClassificationCode"].ToString() != "" ? "" : patientOpASAMsg + "<br/>";
                        patientOpErrorMsg += dsOpVisit.Tables[0].Rows[0]["ConcurrentProcedureCodes"].ToString() != "" ? "" : patientOpConcurrentMsg + "<br/>";
                        patientOpErrorMsg += dsOpVisit.Tables[0].Rows[0]["DischargeLocationCode"].ToString() != "" ? "" : patientOpDischargeMsg + "<br/>";
                    }
                    else
                        patientOpErrorMsg = patientOpPreOpMsg;
                }
                else
                    patientOpErrorMsg = patientOpPreOpMsg;

            }

            //if OpVisit = valid
            if (patientOpErrorMsg == "")
                blnValidOpVisit = true;
        }

        lblValidateOp.Text = patientOpErrorMsg == "" ? "<font color='blue'>" + patientOpValidMsg + "</font>" : "<font color='red'>" + patientOpErrorMsg + "</font>";

        cmdSelect.Dispose();

        return dsOpVisit;
    }
    #endregion
    
    #region private DataSet validatePostOpVisit();
    private DataSet validatePostOpVisit()
    {
        //validate Post-Operative Visit
        SqlCommand cmdSelect = new SqlCommand();
        DataSet dsPostOpVisit = new DataSet();

        String patientPostOpErrorMsg = " &nbsp; &nbsp; There is no new post-operative visit recorded for this patient<br/>";
        String patientPostOpNoRecordValidMsg = patientPostOpErrorMsg;
        String patientPostOpValidMsg = " &nbsp; &nbsp; Valid patient post-operative visit record<br/>";
        String patientPostOpDeceasedMsg = " &nbsp; &nbsp; Could not submit data for deceased patient";
        String patientPostOpDateMsg = " &nbsp; &nbsp; Visit (";
        String patientPostOpDate2Msg = ") can't be after today";
        String patientPostOpHeightMsg = " &nbsp; &nbsp; Please specify patient's height";
        String patientPostOpWeightMsg = " &nbsp; &nbsp; Please specify patient's weight";
        String patientPostOpComorbidityMsg = " &nbsp; &nbsp; Please complete patient's comorbidity check";
        String patientPostOpBeforeMsg = " &nbsp; &nbsp; Last pre-operative visit added to BOLD is ";
        String patientPostOpWarningMsg = "";

        DateTime dtTemp = new DateTime();
        Decimal decTemp = 0;
        String strTemp = "";
        String currCom = "";
        Boolean checkSentPostOp = false;
        Boolean checkDeceased = true;

        /* ----------------------------------------------------------------------------------
         * check if op visit has been sent or about to send one 
         *      yes --> get all new post-op visit
         *              loop for post-op, and find any with Bold_Flag = 0
         *                  check if we already got post-op visit sent to bold
         *                      yes --> check if post-op date < last post-op sent to bold
         *                          yes --> "can not add post-op before last post-op"         
         *                  check if != "can not add post-op before last post-op"
         *                      yes --> validate input
         * 
         * 
         *      check if already sent post-op before
         *          yes --> check if != "op visit has been sent"
         *                      yes --> "post-op exist on Bold"
        ---------------------------------------------------------------------------------- */

        //** check if op visit has been sent or about to send one 
        if (dsOpVisit.Tables[0].Rows.Count > 0 || blnOpSent)
        {
            patientPostOpErrorMsg = "";

            //**  yes --> get all new post-op visit
            gClass.MakeStoreProcedureName(ref cmdSelect, "sp_SRCPreOperativeVisitDataGet", false);
            cmdSelect.Parameters.Add("@vintOrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
            cmdSelect.Parameters.Add("@vintPatientID", System.Data.SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
            cmdSelect.Parameters.Add("@vblnImperial", SqlDbType.Bit).Value = Request.Cookies["Imperial"].Value.Equals("True");
            cmdSelect.Parameters.Add("@vblnPreOperative", SqlDbType.Bit).Value = false;

            dsPostOpVisit = gClass.FetchData(cmdSelect, "tblPostOpVisit");
            if (dsPostOpVisit.Tables[0].Rows.Count > 0)
            {
                //check if patient is not deceased
                //if (!blnOpSent)
                //{
                checkDeceased = false;
                if (dsOpVisit.Tables[0].Rows[0]["DischargeLocationCode"].ToString() != deceasedCode)
                    checkDeceased = true;
                //}

                if (checkDeceased)
                {
                    //** loop for post-op, and find any with Bold_Flag = 0
                    for (int iCountPostOp = 0; iCountPostOp < dsPostOpVisit.Tables[0].Rows.Count; iCountPostOp++)
                    {
                        checkSentPostOp = false;

                        //format visit date
                        if (DateTime.TryParse(dsPostOpVisit.Tables[0].Rows[iCountPostOp]["VisitDate"].ToString(), out dtTemp))
                            strTemp = dtTemp.ToString("dd-MM-yyyy");

                        //** check if we already got post-op visit sent to bold
                        if (dtLastPostOpVisit > new DateTime())
                        {
                            //** yes --> check if post-op date < last post-op sent to bold
                            if (dtTemp < dtLastPostOpVisit && dsPostOpVisit.Tables[0].Rows[iCountPostOp]["BoldVisitID"].ToString() == "")
                            {
                                //** yes --> "can not add post-op before last post-op"
                                checkSentPostOp = true;
                                patientPostOpWarningMsg += patientPostOpBeforeMsg + strLastPostOpVisit + OpVisitBefore + strTemp + "<br />";
                            }
                        }

                        //** check if != "can not add post-op before last post-op"
                        if (checkSentPostOp == false)
                        {
                            //** yes --> validate input


                            //date cant bigger than today
                            patientPostOpErrorMsg += dtTemp <= DateTime.Today ? "" : patientPostOpDateMsg + strTemp + patientPostOpDate2Msg + "<br />";

                            //convert height
                            Decimal.TryParse(dsPostOpVisit.Tables[0].Rows[iCountPostOp]["Height"].ToString(), out decTemp);
                            patientPostOpErrorMsg += decTemp > 0 ? "" : patientPostOpHeightMsg + " for visit " + strTemp + "<br />";

                            //convert weight
                            Decimal.TryParse(dsPostOpVisit.Tables[0].Rows[iCountPostOp]["Weight"].ToString(), out decTemp);
                            patientPostOpErrorMsg += decTemp > 0 ? "" : patientPostOpWeightMsg + " for visit " + strTemp + "<br />";


                            for (int iCountComList = 0; iCountComList < PostOpComorbidityList.Length; iCountComList++)
                            {
                                //check each comorbidity check
                                currCom = PostOpComorbidityList[iCountComList];
                                if (dsPostOpVisit.Tables[0].Rows[iCountPostOp][currCom].ToString() == "")
                                {
                                    patientPostOpErrorMsg += patientPostOpComorbidityMsg + " for visit " + strTemp + "<br />";
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                    patientPostOpErrorMsg += patientPostOpDeceasedMsg + "<br />";
            }
            else
                patientPostOpValidMsg = patientPostOpNoRecordValidMsg;

            //** check if already sent post-op before
            if (dtLastPostOpVisit > new DateTime() && patientPostOpErrorMsg == "")
            {
                patientPostOpValidMsg = PostOpVisitExist;
            }

            cmdSelect.Dispose();
        }
        else
        {
            patientPostOpValidMsg = patientPostOpNoRecordValidMsg;
            patientPostOpErrorMsg = "";
        }

        //if PostOpVisit = valid
        if (patientPostOpErrorMsg == "")
        {
            blnValidPostOpVisit = true;
        }

        lblValidatePostOpWarning.Text = patientPostOpWarningMsg == "" ? "" : "<font color='saddlebrown'>" + patientPostOpWarningMsg + "</font>";
        lblValidatePostOp.Text = patientPostOpErrorMsg == "" ? "<font color='blue'>" + patientPostOpValidMsg + "</font>" : "<font color='red'>" + patientPostOpErrorMsg + "</font>";

        return dsPostOpVisit;
    }
    #endregion
    
    #region private DataSet validateAdverseEvent();
    private DataSet validateAdverseEvent()
    {
        SqlCommand cmdSelect = new SqlCommand();
        DataSet dsAdverseEvent = new DataSet();

        String adverseEventErrorMsg = " &nbsp; &nbsp; There is no new adverse event recorded for this patient<br/>";
        String adverseEventNoRecordValidMsg = adverseEventErrorMsg;
        String adverseEventValidMsg = " &nbsp; &nbsp; Valid patient adverse event record<br/>";
        String adverseEventTypeNotValidMsg = " &nbsp; &nbsp; Please specify a valid event. The Event type you specified is not BOLD compliance";
        String adverseEventSurgeryNotValidMsg = " &nbsp; &nbsp; Please specify a valid surgery for the event. The Surgery type you specified is not BOLD compliance";
        String adverseEventDeceasedMsg = " &nbsp; &nbsp; Could not submit data for deceased patient";
        String adverseEventSurgery = " &nbsp; &nbsp; Please specify the surgery";
        String patientOpMsg = " &nbsp; &nbsp; There should be one operative visit";


        DateTime dtTemp = new DateTime();
        String strTemp = "";

        //validate Post-Operative Visit
        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_SRCAdverseEventPostOperativeDataGet", false);
        cmdSelect.Parameters.Add("@vintOrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSelect.Parameters.Add("@vintPatientID", System.Data.SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);

        dsAdverseEvent = gClass.FetchData(cmdSelect, "tblAdverseEvent");

        adverseEventErrorMsg = "";
        if (dsAdverseEvent.Tables.Count > 0)
        {
            if (dsAdverseEvent.Tables[0].Rows.Count > 0)
            {
                adverseEventErrorMsg = "";

                if (dsOpVisit.Tables[0].Rows.Count > 0)
                {
                    if (dsOpVisit.Tables[0].Rows[0]["DischargeLocationCode"].ToString() != deceasedCode)
                    {
                        //loop for all adverse event
                        for (int iCountAdverseEvent = 0; iCountAdverseEvent < dsAdverseEvent.Tables[0].Rows.Count; iCountAdverseEvent++)
                        {   
                            //format visit date
                            if (DateTime.TryParse(dsAdverseEvent.Tables[0].Rows[iCountAdverseEvent]["DateOfEvent"].ToString(), out dtTemp))
                                strTemp = dtTemp.ToString("dd-MM-yyyy");

                            if (dsAdverseEvent.Tables[0].Rows[iCountAdverseEvent]["AdverseEventCode"].ToString().IndexOf("ADD") != -1)
                                adverseEventErrorMsg += adverseEventTypeNotValidMsg + " for adverse event " + strTemp + "<br />";

                            if (dsAdverseEvent.Tables[0].Rows[iCountAdverseEvent]["SurgeryCodes"].ToString().IndexOf("ADD") != -1)
                                adverseEventErrorMsg += adverseEventSurgeryNotValidMsg + " for adverse event " + strTemp + "<br />";

                            //adverseEventErrorMsg += dsAdverseEvent.Tables[0].Rows[0]["SurgeryCodes"].ToString() != "" ? "" : adverseEventSurgery + " for adverse event " + strTemp + "<br />";
                        }
                    }
                    else
                        adverseEventErrorMsg += adverseEventDeceasedMsg + "<br />";
                }
                else
                    adverseEventErrorMsg = patientOpMsg;
            }
            else
                adverseEventValidMsg = adverseEventNoRecordValidMsg;
        }
        else
            adverseEventValidMsg = adverseEventNoRecordValidMsg;

        //if adverseevent = valid
        if (adverseEventErrorMsg == "")
        {
            blnValidAdverseEvent = true;
        }

        cmdSelect.Dispose();

        lblValidateAdverseEvent.Text = adverseEventErrorMsg == "" ? "<font color='blue'>" + adverseEventValidMsg + "</font>" : "<font color='red'>" + adverseEventErrorMsg + "</font>";

        return dsAdverseEvent;
    }
    #endregion
    
    #region private void clearSRCError();
    private void clearSRCError()
    {
        ltrPatientDataError.Visible = false;
        ltrPreOperativeVisitError.Visible = false;
        ltrHospitalVisitError.Visible = false;
        ltrPostOperativeVisitError.Visible = false;
        ltrAdverseEventPostOperativeError.Visible = false;
        ltrPatientSignError.Visible = false;
    }
    #endregion

    #region Modules
    /// <summary>
    /// Validate SRC Object.
    /// </summary>
    /// 
    protected void ValidateDataToExportToSRC(object sender, EventArgs e)
    {
        validateSRCData();
    }


    protected void SignPatientSRC(object sender, EventArgs e)
    {
        Int32 intPatientID;
        if (Int32.TryParse(Request.Cookies["PatientID"].Value.ToString(), out intPatientID))
        {
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
            objSRC.SignPatientData();

            ltrPatientSignError.Visible = objSRC.PatientSignErrors.Count > 0;
            if (objSRC.PatientSignErrors.Count > 0)
            {
                blnSucceedFlag = false;
                rptPatientSignError.DataSource = objSRC.PatientSignErrors;
                rptPatientSignError.DataBind();
            }
        }
    }

    /// <summary>
    /// Initials the SRC Object and fills it by extracing data.
    /// </summary>
    /// 
    protected void ExtractDataToExportToSRC(object sender, EventArgs e)
    {
        Int32 intPatientID;
        if (Int32.TryParse(Request.Cookies["PatientID"].Value.ToString(), out intPatientID))
        {
            Boolean blnSucceedFlag = true;
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
            objSRC.LoadPatientData();

            ltrPatientDataError.Visible = objSRC.PatientErrors.Count > 0;
            if (ltrPatientDataError.Visible)
            {
                blnSucceedFlag = false;
                rptPatientDataError.DataSource = objSRC.PatientErrors;
                rptPatientDataError.DataBind();
            }

            ltrPreOperativeVisitError.Visible = objSRC.PreOperativeVisitErrors.Count > 0;
            if (ltrPreOperativeVisitError.Visible)
            {
                blnSucceedFlag = false;
                rptPreOperativeVisitError.DataSource = objSRC.PreOperativeVisitErrors;
                rptPreOperativeVisitError.DataBind();
            }

            ltrHospitalVisitError.Visible = objSRC.HospitalVisitErrors.Count > 0;
            if (ltrHospitalVisitError.Visible)
            {
                blnSucceedFlag = false;
                rptHospitalVisitError.DataSource = objSRC.HospitalVisitErrors;
                rptHospitalVisitError.DataBind();
            }

            ltrPostOperativeVisitError.Visible = objSRC.PostOperativeVisitErrors.Count > 0;
            if (ltrPostOperativeVisitError.Visible)
            {
                blnSucceedFlag = false;
                rptPostOperativeVisitError.DataSource = objSRC.PostOperativeVisitErrors;
                rptPostOperativeVisitError.DataBind();
            }

            ltrAdverseEventPostOperativeError.Visible = objSRC.AdverseEventPostOperativeErrors.Count > 0;
            if (objSRC.AdverseEventPostOperativeErrors.Count > 0)
            {
                blnSucceedFlag = false;
                rptAdverseEventPostOperativeError.DataSource = objSRC.AdverseEventPostOperativeErrors;
                rptAdverseEventPostOperativeError.DataBind();
            }

            ltrSRCSucceed.Visible = blnSucceedFlag;
            ltrSRCSucceed.Text = blnSucceedFlag ? "The Data is transfered and saved successfully.." : "";
        }
    }
    #endregion
}
