using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;
using Microsoft.Web.UI;
using System.IO;
using System.Globalization;
using System.Xml;
using Lapbase.Configuration.ConfigurationApplication;

public partial class Forms_Patients_Operations_PatientOperationForm : System.Web.UI.Page
{
    GlobalClass gClass = new GlobalClass();
    String BoldErrorMsg = "";

    #region protected void Page_Load(object sender, EventArgs e)
    protected void Page_Load(object sender, EventArgs e)
    {
            
        try
        {            
            Response.CacheControl = "no-cache";
            Response.AddHeader("Pragma", "no-cache");
            Response.Expires = -1;
            
            Page.Culture = Request.Cookies["CultureInfo"].Value;
            txtHCurrentDate.Value = DateTime.Now.ToShortDateString();

            gClass.LanguageCode = Request.Cookies["LanguageCode"].Value;
            gClass.OrganizationCode = Request.Cookies["OrganizationCode"].Value;
            txtHCulture.Value = Request.Cookies["CultureInfo"].Value;

            //split feature
            string Feature = Request.Cookies["Feature"].Value;
            string[] Features = Feature.Split(new string[] { "**" }, StringSplitOptions.None);

            string ShowRegistry = Features[1];

            if (ShowRegistry == "True"){
                btnPrintOreg1.Style["display"] = "block";
                btnPrintOreg2.Style["display"] = "block";
                txtAnnualOreg.Style["display"] = "block";
                lblAnnualOreg.Style["display"] = "block";
            } else if (ShowRegistry == "False"){
                btnPrintOreg1.Style["display"] = "none";
                btnPrintOreg2.Style["display"] = "none";
                txtAnnualOreg.Style["display"] = "none";
                lblAnnualOreg.Style["display"] = "none";
            }

            if (Request.Cookies["SubmitData"].Value.IndexOf("acs") >= 0)
            {
                assistantDefault.Style["display"] = "none";
            }
            else
            {
                intraopTranfusionACS.Style["display"] = "none";
                assistantACS.Style["display"] = "none";
                operativeTimeACS.Style["display"] = "none";
                preopHematocritACS.Style["display"] = "none";
                preopAlbuminACS.Style["display"] = "none";

                bloodTranfusionACS.Style["display"] = "none";
                unplannedAdmissionACS.Style["display"] = "none";
                transferACS.Style["display"] = "none";
            }
                
            if (gClass.IsUserLogoned(Session.SessionID, Request.Cookies["UserPracticeCode"].Value.ToString(), Request.Url.Host))
            {
                txtHApplicationURL.Value = Request.Url.Scheme + "://" + Request.Url.Host + Request.ApplicationPath + "/";
                txtUseImperial.Value = (Request.Cookies["Imperial"].Value == "True") ? "1" : "0";
                SetAttributes();
                if (!IsPostBack)
                {
                    gClass.SaveUserLogFile( Request.Cookies["UserPracticeCode"].Value, 
                                            Request.Cookies["Logon_UserName"].Value, 
                                            Request.Url.Host,
                                            "Operation Form", 2, "Browse", "PatientCode", Request.Cookies["PatientID"].Value);


                    gClass.SaveActionLog(gClass.OrganizationCode,
                                            Context.Request.Cookies["UserPracticeCode"].Value,
                                            Context.Request.Url.Host,
                                            System.Configuration.ConfigurationManager.AppSettings["OperationPage"].ToString(),
                                            System.Configuration.ConfigurationManager.AppSettings["LogRead"].ToString(),
                                            "Load " + System.Configuration.ConfigurationManager.AppSettings["OperationPage"].ToString() + " List ",
                                            Context.Request.Cookies["PatientID"].Value,
                                            "");

                    try{txtHAdmitID.Value = (Request.QueryString.Count > 0) ? Request.QueryString["OP"].ToString() : "0";}
                    catch{txtHAdmitID.Value = "0";}

                    txtHPermissionLevel.Value = Request.Cookies["PermissionLevel"].Value;
                    txtHDataClamp.Value = Request.Cookies["DataClamp"].Value;

                    if (Request.Cookies["SubmitData"].Value.IndexOf("submitbold") >= 0 || Request.Cookies["PermissionLevel"].Value == "1o" || Request.Cookies["PermissionLevel"].Value == "2t" || Request.Cookies["PermissionLevel"].Value == "3f" || Request.Cookies["PermissionLevel"].Value == "4s")
                        btnDelete.Style["display"] = "none";

                    if (Request.Cookies["PermissionLevel"].Value == "1o")
                    {
                        btnSave.Style["display"] = "none";
                        btnCancel.Style["display"] = "none";
                        btnAddOperation.Style["display"] = "none";
                    }
                }
            }
            else
            {
                gClass.ReturnToLoginPage(Request.Url.Host, Request.Cookies["LanguageCode"].Value, Response);
                return;
            }

            if (Request.Cookies["SubmitData"].Value.IndexOf("bold") >= 0)
            {
                LoadBoldList();
            }
        }
        catch (Exception err)
        {
            string strLanguageCode;
            try
            {
                strLanguageCode = Request.Cookies["LanguageCode"].Value;
                gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Patient List Form", "Loading Patient List (Page_Load function)", err.ToString());
            }
            catch { strLanguageCode = "en-US"; }
            gClass.ReturnToLoginPage(Request.Url.Host, strLanguageCode, Response);
        }
        return;
    }
    #endregion

    #region private void SetAttributes()
    private void SetAttributes()
    {
        bodyPatientOperation.Attributes.Add("onload", "javascript:InitializePage();");
        bodyPatientOperation.Style.Add("Direction", Request.Cookies["Direction"].Value);
        string strScript = "javascript:FetchFieldsCaption(false,  document.getElementById('txtHCulture').value, document.frmPatientOperation.name);";
        ScriptManager.RegisterStartupScript(linkBtnLoad, linkBtnLoad.GetType(), Guid.NewGuid().ToString(), strScript, true);
            
        //lblOperationNotes.Attributes.Add("onclick", "ShowOperationNotesDiv();");
        //lblOperationNotes.Attributes.Add("onmouseover", "this.style.cursor = 'pointer';");
        //lblOperationNotes.Attributes.Add("onmouseout", "this.style.cursor = '';");

        System.Globalization.CultureInfo myCI = new System.Globalization.CultureInfo(Request.Cookies["CultureInfo"].Value, false);
        lblDateFormat.Text = lblAdmissionDateFormat.Text = lblDischargeDateFormat.Text = myCI.DateTimeFormat.ShortDatePattern.ToLower();
        txtOperationDate.toolTip = myCI.DateTimeFormat.ShortDatePattern;
        return;
    }
    #endregion

    #region private void FillBoldList(UserControl_SystemCodeWUCtrl ddlSource, HtmlSelect listDest)
    private void FillBoldList(UserControl_SystemCodeWUCtrl ddlSource, HtmlSelect listDest)
    {
        foreach (ListItem sourceItem in ddlSource.Items)
        {
            ListItem destItem = new ListItem();
            destItem.Value = sourceItem.Value;
            destItem.Text = sourceItem.Text;
            listDest.Items.Add(destItem);
        }
    }
    #endregion 

    #region protected void linkBtnSave_OnClick(object sender, EventArgs e)
    protected void linkBtnSave_OnClick(object sender, EventArgs e)
    {
        string strReturn = "", strScript = String.Empty;

        String createdDate = txtHDateCreated.Value;
        String currentDate = txtHCurrentDate.Value;

        Boolean blnInsert;
        //if dataclamp is activated, permission lvl 2 or 3
        //check for created date for this patient
        Boolean allowSave = true;

        if (txtHDataClamp.Value.ToLower() == "true" && (txtHPermissionLevel.Value == "2t" || txtHPermissionLevel.Value == "3f"))
        {
            if (createdDate != "")
            {
                if (createdDate != currentDate)
                    allowSave = false;
            }
        }

        //if got permission to save
        if (Request.Cookies["PermissionLevel"].Value != "1o" && allowSave == true)
        {
            Int32 intTemp;
            Int32 intAnesthesiaDurationTemp;
            Int32 intSurgeryDurationTemp;
            Int32 intAfterSurgery;
            Decimal decTemp = 0;
            SqlCommand cmdSave = new SqlCommand();
            SqlCommand cmdSelect = new SqlCommand();
            DataSet dsBoldCode = new DataSet();
            DataSet dsOpVisit = new DataSet();
            String tempDVT = "";
            DateTime admissionDate = DateTime.MinValue;
            DateTime surgeryDate = DateTime.MinValue;
            DateTime dischargeDate = DateTime.MinValue;
            DateTime lastOperationDischargeDate = DateTime.MinValue;
            DateTime firstVisitDate = DateTime.MinValue;
            String admitID = "";


            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(Request.Cookies["CultureInfo"].Value, true);
            SqlCommand cmdOperation = new SqlCommand();
            AddOleCommandParameters(gClass, txtHAdmitID.Value, ref cmdOperation);
            TestText.Value = "";
            try
            {
                cmdOperation.Parameters["@PageNo"].Value = Convert.ToInt32(txtHPageNo.Value);
                cmdOperation.Parameters["@OrganizationCode"].Value = Convert.ToInt32(gClass.OrganizationCode);
                cmdOperation.Parameters["@UserPracticeCode"].Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
                cmdOperation.Parameters["@PatientId"].Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
                cmdOperation.Parameters["@SurgeonId"].Value = Convert.ToInt32(cmbSurgeon.SelectedValue); // SurgeonId
                cmdOperation.Parameters["@HospitalCode"].Value = cmbHospital.SelectedValue; //

                try { surgeryDate = Convert.ToDateTime(txtOperationDate.Text); cmdOperation.Parameters["@OperationDate"].Value = surgeryDate; }
                catch { cmdOperation.Parameters["@OperationDate"].Value = DBNull.Value; }

                try { cmdOperation.Parameters["@Duration"].Value = Convert.ToInt32(txtDuration.Text); }
                catch { cmdOperation.Parameters["@Duration"].Value = 0; }

                cmdOperation.Parameters["@Bougie"].Value = txtSleeveBougie.Text.ToString().Trim();


                try { cmdOperation.Parameters["@NumberBloodTranfusion"].Value = Convert.ToInt32(txtNumberBloodTranfusion.Text); }
                catch { cmdOperation.Parameters["@NumberBloodTranfusion"].Value = DBNull.Value; }

                try { cmdOperation.Parameters["@NumberIntraopTranfusion"].Value = Convert.ToInt32(txtIntraopTranfusion.Text); }
                catch { cmdOperation.Parameters["@NumberIntraopTranfusion"].Value = DBNull.Value; }

                cmdOperation.Parameters["@UnplannedAdmission"].Value = rbUnplannedAdmissionY.Checked ? 1 : 0;
                cmdOperation.Parameters["@TransferAcuteCare"].Value = rbTransferY.Checked ? 1 : 0;

                cmdOperation.Parameters["@InRoomTime"].Value = txtInRoomTimeH.Value.ToString().Trim() + ":" + txtInRoomTimeM.Value.ToString().Trim();
                cmdOperation.Parameters["@OutRoomTime"].Value = txtOutRoomTimeH.Value.ToString().Trim() + ":" + txtOutRoomTimeM.Value.ToString().Trim();
                cmdOperation.Parameters["@SurgeryStartTime"].Value = txtSurgeryStartH.Value.ToString().Trim() + ":" + txtSurgeryStartM.Value.ToString().Trim();
                cmdOperation.Parameters["@SurgeryEndTime"].Value = txtSurgeryEndH.Value.ToString().Trim() + ":" + txtSurgeryEndM.Value.ToString().Trim();
                cmdOperation.Parameters["@FirstAssistant"].Value = ListCheckBoxAnswer(new String[] { "chkAssistantNone", "chkAssistantPA", "chkAssistantJunior", "chkAssistantSenior", "chkAssistantMIS", "chkAssistantAttendingSurgeon", "chkAssistantAttendingOther" });
                cmdOperation.Parameters["@PreopHematocrit"].Value = txtPreopHematocrit.Value.ToString().Trim();
                cmdOperation.Parameters["@PreopAlbumin"].Value = txtPreopAlbumin.Value.ToString().Trim();

                try { cmdOperation.Parameters["@PreopHematocritDate"].Value = Convert.ToDateTime(txtPreopHematocritDate.Text); }
                catch { cmdOperation.Parameters["@PreopHematocritDate"].Value = DBNull.Value; }
                
                try { cmdOperation.Parameters["@PreopAlbuminDate"].Value = Convert.ToDateTime(txtPreopAlbuminDate.Text); }
                catch { cmdOperation.Parameters["@PreopAlbuminDate"].Value = DBNull.Value; }

                cmdOperation.Parameters["@OtherProcedure"].Value = txtDetailOtherProcedure.Value.ToString().Trim();

                cmdOperation.Parameters["@SurgeryType"].Value = cmbSurgeryType.SelectedValue;
                cmdOperation.Parameters["@Approach"].Value = cmbApproach.SelectedValue;
                cmdOperation.Parameters["@Category"].Value = cmbCategory.SelectedValue;
                cmdOperation.Parameters["@Group"].Value = cmbGroup.SelectedValue;
                cmdOperation.Parameters["@BloodLoss"].Value = Decimal.TryParse(txtBloodLoss.Text, out decTemp) ? decTemp : 0;
                cmdOperation.Parameters["@Banded"].Value = chkBanded.Checked;
                cmdOperation.Parameters["@TubeSize"].Value = txtTubeSize.Text;
                cmdOperation.Parameters["@BPDIlealLength"].Value = txtBPDIlealLength.Text;
                cmdOperation.Parameters["@VBGStomaWrap"].Value = cmbVBGStomaWrap.SelectedValue;
                cmdOperation.Parameters["@RouxLimbLength"].Value = txtRouxLimbLength.Text;
                cmdOperation.Parameters["@BPDChannelLength"].Value = txtBPDChannelLength.Text;
                cmdOperation.Parameters["@VBGStomaSize"].Value = txtVBGStomaSize.Text;
                cmdOperation.Parameters["@RouxEnterostomy"].Value = cmbRouxEnterostomy.SelectedValue;
                cmdOperation.Parameters["@ReservoirSite"].Value = cmbReservoirSite.SelectedValue;
                cmdOperation.Parameters["@BPDDuodenalSwitch"].Value = chkBPDDuodenalSwitch.Checked;
                cmdOperation.Parameters["@RouxColic"].Value = cmbRouxColic.SelectedValue;
                cmdOperation.Parameters["@BalloonVolume"].Value = Decimal.TryParse(txtBalloonVolume.Text, out decTemp) ? decTemp : 0;
                cmdOperation.Parameters["@BPDStomachSize"].Value = Decimal.TryParse(txtBPDStomachSize.Text, out decTemp) ? decTemp : 0;

                cmdOperation.Parameters["@BandSize"].Value = cmbBandSize.SelectedValue;
                cmdOperation.Parameters["@RouxGastric"].Value = cmbRouxGastric.SelectedValue;
                cmdOperation.Parameters["@Pathway"].Value = cmbPathway.SelectedValue;
                cmdOperation.Parameters["@GeneralNotes"].Value = "";
                cmdOperation.Parameters["@BandType"].Value = ddlBandType.SelectedValue;

                cmdOperation.Parameters["@DaysInHospital"].Value = int.TryParse(txtLosPostOp.Text, out intTemp) ? intTemp : 0;
                cmdOperation.Parameters["@OpWeight"].Value = Decimal.TryParse(txtHWeight.Value, out decTemp) ? decTemp : 0;
                try { admissionDate = Convert.ToDateTime(txtAdmissionDate.Text); cmdOperation.Parameters["@AdmitDate"].Value = admissionDate; }
                catch { cmdOperation.Parameters["@AdmitDate"].Value = DBNull.Value; }
                
                //** get DVT Bold code
                gClass.MakeStoreProcedureName(ref cmdSelect, "sp_SystemCodes_LoadData_Bold", true);
                cmdSelect.Parameters.Add("@GroupCode", SqlDbType.VarChar, 10).Value = "DVT";
                cmdSelect.Parameters.Add("@CategoryCode", SqlDbType.VarChar, 10).Value = "DVT";
                cmdSelect.Parameters.Add("@SCode", SqlDbType.VarChar, 10).Value = String.Empty;
                dsBoldCode = gClass.FetchData(cmdSelect, "tblBoldCode");
                
                //construct DVT
                if (chkAntigloculan.Checked)
                    tempDVT += dsBoldCode.Tables[0].Rows[0]["Code"].ToString() + ";";
                if (chkCompress.Checked)
                    tempDVT += dsBoldCode.Tables[0].Rows[1]["Code"].ToString() + ";";
                if (chkFootPump.Checked)
                    tempDVT += dsBoldCode.Tables[0].Rows[2]["Code"].ToString() + ";";
                if (chkTED.Checked)
                    tempDVT += dsBoldCode.Tables[0].Rows[3]["Code"].ToString() + ";";
                

                gClass.MakeStoreProcedureName(ref cmdSave, "sp_PatientOperations_SaveBoldData", true);
                cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                cmdSave.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
                cmdSave.Parameters.Add("@PatientId", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
                cmdSave.Parameters.Add("@LastConsultIDBeforeOperation", SqlDbType.Int).Value = 0;

                cmdSave.Parameters.Add("@AdmitDate", SqlDbType.DateTime);
                try { cmdSave.Parameters["@AdmitDate"].Value = Convert.ToDateTime(txtAdmissionDate.Text); }
                catch { cmdSave.Parameters["@AdmitDate"].Value = DBNull.Value; }

                cmdSave.Parameters.Add("@OperationDate", SqlDbType.DateTime);
                try { cmdSave.Parameters["@OperationDate"].Value = Convert.ToDateTime(txtOperationDate.Text); }
                catch { cmdSave.Parameters["@OperationDate"].Value = DBNull.Value; }

                cmdSave.Parameters.Add("@HospitalCode", SqlDbType.VarChar, 6).Value = cmbHospital.SelectedValue;
                cmdSave.Parameters.Add("@SurgeonId", SqlDbType.Int).Value = Convert.ToInt32(cmbSurgeon.SelectedValue);
                cmdSave.Parameters.Add("@Duration", SqlDbType.Int).Value = Int32.TryParse(txtDuration.Text, out intSurgeryDurationTemp) ? intSurgeryDurationTemp : 0;
                cmdSave.Parameters.Add("@SurgeryType", SqlDbType.VarChar, 10).Value = cmbSurgeryType.SelectedValue;
                cmdSave.Parameters.Add("@Approach", SqlDbType.VarChar, 50).Value = cmbApproach.SelectedValue;
                cmdSave.Parameters.Add("@BloodLoss", SqlDbType.Decimal).Value = Decimal.TryParse(txtBloodLoss.Text, out decTemp) ? decTemp : 0;

                cmdSave.Parameters.Add("@AnesthesiaDuration", SqlDbType.Int).Value = Int32.TryParse(txtAnesthesiaDuration.Text, out intAnesthesiaDurationTemp) ? intAnesthesiaDurationTemp : 0;
                cmdSave.Parameters.Add("@DVTCode", SqlDbType.VarChar, 50).Value = tempDVT;
                cmdSave.Parameters.Add("@ASACode", SqlDbType.VarChar, 10).Value = cmbAsaClassification.SelectedValue;


                cmdSave.Parameters.Add("@DVTAntigloculan", SqlDbType.Bit).Value = chkAntigloculan.Checked;
                cmdSave.Parameters.Add("@DVTTED", SqlDbType.Bit).Value = chkTED.Checked;
                cmdSave.Parameters.Add("@DVTFootPump", SqlDbType.Bit).Value = chkFootPump.Checked;
                cmdSave.Parameters.Add("@DVTCompress", SqlDbType.Bit).Value = chkCompress.Checked;

                cmdSave.Parameters.Add("@SurgicalResidentParticipated", SqlDbType.Bit).Value = chkResident.Checked;
                cmdSave.Parameters.Add("@SurgicalFellowParticipated", SqlDbType.Bit).Value = chkFellow.Checked;

                TestText.Value += " " + txtIntraOperativeEvents_Selected.Value + "  " + txtPreDischargeEvents_Selected.Value;
                cmdSave.Parameters.Add("@IntraEvents", SqlDbType.VarChar, 1024).Value = txtIntraOperativeEvents_Selected.Value;
                cmdSave.Parameters.Add("@PreDischargeEvents", SqlDbType.VarChar, 1024).Value = txtPreDischargeEvents_Selected.Value;

                cmdSave.Parameters.Add("@Concurrent", SqlDbType.VarChar, 1024).Value = txtConcurrent_Selected.Value;

                cmdSave.Parameters.Add("@TimeAfterSurgery", SqlDbType.Int).Value = Int32.TryParse(txtAfterSurgery.Text, out intTemp) ? intTemp : 0;
                cmdSave.Parameters.Add("@TimeAfterSurgeryMeasurement", SqlDbType.VarChar, 50).Value = cmbAfterSurgeryMeasurement.SelectedValue;

                cmdSave.Parameters.Add("@PreDischargeSurgeon", SqlDbType.Int).Value = Int32.TryParse(cmbAdverseSurgeon.SelectedValue, out intTemp) ? intTemp : 0;
                cmdSave.Parameters.Add("@PreDischargeSurgery", SqlDbType.VarChar, 255).Value = cmbAdverseSurgery.SelectedValue;

                cmdSave.Parameters.Add("@DischargeDate", SqlDbType.DateTime);
                
                try { dischargeDate = Convert.ToDateTime(txtDischargeDate.Text); cmdSave.Parameters["@DischargeDate"].Value = dischargeDate;}
                catch { cmdSave.Parameters["@DischargeDate"].Value = DBNull.Value; }

                cmdSave.Parameters.Add("@DischargeTo", SqlDbType.VarChar, 10).Value = cmbDischargeTo.SelectedValue;
                cmdSave.Parameters.Add("@BloodTransfusion", SqlDbType.Decimal).Value = Decimal.TryParse(txtBloodTransfusion.Text, out decTemp) ? decTemp : 0;
                cmdSave.Parameters.Add("@SerialNo", SqlDbType.VarChar, 50).Value = txtSerialNo.Text;
                cmdSave.Parameters.Add("@BandType", SqlDbType.VarChar, 10).Value = ddlBandType.SelectedValue;

                //save registry details
                cmdSave.Parameters.Add("@RegistryProcedure", SqlDbType.VarChar, 50).Value = cmbRegistryProcedure.SelectedValue;
                

                //BOLD validation
                //only check if BOLD mode is on
                if (Request.Cookies["SubmitData"].Value.IndexOf("submitbold") >= 0)
                {
                    cmdSelect.Parameters.Clear();

                    gClass.MakeStoreProcedureName(ref cmdSelect, "sp_SRCHospitalVisitCheck", false);
                    cmdSelect.Parameters.Add("@vintOrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                    cmdSelect.Parameters.Add("@vintPatientId", System.Data.SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
                    dsOpVisit = gClass.FetchData(cmdSelect, "tblOpVisit");

                    try { lastOperationDischargeDate = Convert.ToDateTime(gClass.TruncateDate(dsOpVisit.Tables[0].Rows[0]["DischargeDate"].ToString().Trim(), Request.Cookies["CultureInfo"].Value, 1)); }
                    catch { }
                    try { firstVisitDate = Convert.ToDateTime(gClass.TruncateDate(dsOpVisit.Tables[0].Rows[0]["FirstVisitDate"].ToString().Trim(), Request.Cookies["CultureInfo"].Value, 1)); }
                    catch { }
                    try { admitID = dsOpVisit.Tables[0].Rows[0]["AdmitID"].ToString().Trim(); }
                    catch { }

                    BoldErrorMsg += cmbSurgeryType.SelectedValue != "" ? "" : "<br/>- Operation is required";
                    BoldErrorMsg += cmbApproach.SelectedValue != "" ? "" : "<br/>- Approach is required";
                    BoldErrorMsg += cmbHospital.SelectedValue != "" ? "" : "<br/>- Facility is required";
                    BoldErrorMsg += Convert.ToInt32(cmbSurgeon.SelectedValue) > 0 ? "" : "<br/>- Surgeon is required";
                    BoldErrorMsg += cmbAsaClassification.SelectedValue != "" ? "" : "<br/>- ASA Classification is required";
                    //BoldErrorMsg += tempDVT != "" ? "" : "<br/>- DVT is required";
                    BoldErrorMsg += cmbDischargeTo.SelectedValue != "" ? "" : "<br/>- Discharge To is required";
                    
                    BoldErrorMsg += intSurgeryDurationTemp > 0 ? "" : "<br/>- Invalid duration of surgery. Please enter a positive value for duration of surgery";
                    BoldErrorMsg += intAnesthesiaDurationTemp > 0 ? "" : "<br/>- Invalid duration of anesthesia. Please enter a positive value for duration of anesthesia";

                    BoldErrorMsg += intAnesthesiaDurationTemp > intSurgeryDurationTemp ? "" : "<br/>- Anesthesia duration can not be less than surgery duration";
                    BoldErrorMsg += admissionDate == DateTime.MinValue ? "<br/>- Date of Admission is required" : "";
                    BoldErrorMsg += surgeryDate == DateTime.MinValue ? "<br/>- Date of Operation is required" : "";
                    BoldErrorMsg += dischargeDate == DateTime.MinValue ? "<br/>- Date of Discharge is required" : "";

                    BoldErrorMsg += admissionDate > surgeryDate ? "<br/>- Date of Surgery must be equal to or after the Date of Admission" : "";
                    BoldErrorMsg += admissionDate > dischargeDate ? "<br/>- Date of Discharge must be equal to or after the Date of Admission" : "";
                    BoldErrorMsg += surgeryDate > dischargeDate ? "<br/>- Date of Discharge must be equal to or after the Date of Surgery" : "";

                    BoldErrorMsg += firstVisitDate == DateTime.MinValue ? "<br/>- Patient must have at least one preoperative visit" : "";
                    BoldErrorMsg += admissionDate < firstVisitDate ? "<br/>- Date of admission and date of surgery must be after all preoperative visits" : "";

                    if (admitID != txtHAdmitID.Value)
                    BoldErrorMsg += admissionDate < lastOperationDischargeDate ? "<br/>- Date of admission and date of surgery must be after all prior facility stays" : "";


                    if (surgeryDate != DateTime.MinValue && dischargeDate != DateTime.MinValue)
                    {
                        TimeSpan span = dischargeDate.Subtract(surgeryDate);
                        if (span.Days >= 365 && txtIntraOperativeEvents_Selected.Value == "" && txtPreDischargeEvents_Selected.Value == "")
                        {
                            BoldErrorMsg += "<br/>- This patient had a length of stay of one year or more but have not reported any adverse events. Please enter the adverse events that caused this patient to stay in the hospital for at least one year or correct the discharge date";
                        }
                    }


                    BoldErrorMsg += txtHDoctorBoldList.Value.IndexOf(cmbSurgeon.SelectedValue + "-") < 0 ? "<br/>- Surgeon must have BOLD ID and can not be other. Please fill it in on Settings" : "";
                    BoldErrorMsg += txtHHospitalBoldList.Value.IndexOf(cmbHospital.SelectedValue + "-") < 0 ? "<br/>- Facility must not have BOLD ID and can not be other. Please fill it in on Settings" : "";

                    if (cmbAdverseSurgery.SelectedValue != "" || cmbAdverseSurgeon.SelectedValue != "0")
                    {
                        BoldErrorMsg += txtHDoctorBoldListWithOther.Value.IndexOf(cmbAdverseSurgeon.SelectedValue + "-") < 0 ? "<br/>- Re-operation Surgeon must have BOLD ID. Please fill it in on Settings" : "";
                        BoldErrorMsg += cmbAdverseSurgery.SelectedValue.IndexOf("ADD") >= 0 ? "<br/>- The Surgical Intervention is not BOLD compliance": "";

                        if (cmbAdverseSurgery.SelectedValue == "" || cmbAdverseSurgeon.SelectedValue != "0")
                            BoldErrorMsg += "<br/>- Re-operation Surgeon and Surgical Intervention are required. Either fill both Surgical Intervention and Re-operation Surgeon or empty both";
                    }

                    if (cmbSurgeryType.SelectedValue.IndexOf("ADD") >= 0)
                        BoldErrorMsg += "<br/>- The Operation is not BOLD compliance";
                    
                    if (cmbApproach.SelectedValue.IndexOf("ADD") >= 0)
                        BoldErrorMsg += "<br/>- The Approach is not BOLD compliance";

                    if (txtConcurrent_Selected.Value.IndexOf("ADD") >= 0)
                        BoldErrorMsg += "<br/>- The Concurrent Procedure is not BOLD compliance";

                    if (txtIntraOperativeEvents_Selected.Value.IndexOf("ADD") >= 0)
                        BoldErrorMsg += "<br/>- The Intra-Operative Events is not BOLD compliance";

                    if (txtPreDischargeEvents_Selected.Value.IndexOf("ADD") >= 0)
                        BoldErrorMsg += "<br/>- The Pre-Operative Events is not BOLD compliance";

                    if (txtPreDischargeEvents_Selected.Value.Trim() != "")
                    {
                        try { intAfterSurgery = Convert.ToInt32(txtAfterSurgery.Text); }
                        catch { intAfterSurgery = 0; }

                        if (intAfterSurgery <= 0)
                            BoldErrorMsg += "<br/>- Time occurred after surgery is required and must be a whole number greater than zero";

                        if (cmbAfterSurgeryMeasurement.SelectedValue.ToString() == "")
                            BoldErrorMsg += "<br/>- Time Measurement for events occurred after surgery is required";
                    }
                }

                if (BoldErrorMsg == "")
                {

                    if (!txtHAdmitID.Value.Equals("0"))
                    {
                        //insert log
                        try
                        {
                            SqlCommand cmdSaveLog = new SqlCommand();
                            gClass.MakeStoreProcedureName(ref cmdSaveLog, "sp_PatientOperations_InsertDataLog", true);
                            cmdSaveLog.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                            cmdSaveLog.Parameters.Add("@AdmitID", SqlDbType.Int).Value = Convert.ToInt32(txtHAdmitID.Value);
                            cmdSaveLog.Parameters.Add("@LogUserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
                            cmdSaveLog.Parameters.Add("@LogDateTime", SqlDbType.DateTime).Value = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
                            gClass.ExecuteDMLCommand(cmdSaveLog);


                            SqlCommand cmdSaveBoldLog = new SqlCommand();
                            gClass.MakeStoreProcedureName(ref cmdSaveBoldLog, "sp_PatientOperations_SaveBoldDataLog", true);
                            cmdSaveBoldLog.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                            cmdSaveBoldLog.Parameters.Add("@AdmitID", SqlDbType.Int).Value = Convert.ToInt32(txtHAdmitID.Value);
                            cmdSaveBoldLog.Parameters.Add("@LogUserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
                            cmdSaveBoldLog.Parameters.Add("@LogDateTime", SqlDbType.DateTime).Value = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
                            gClass.ExecuteDMLCommand(cmdSaveBoldLog);
                        }
                        catch (Exception err)
                        {
                            gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                                        Context.Request.Cookies["Logon_UserName"].Value, "Operation  PID : " + Context.Request.Cookies["PatientID"].Value, "Data saving Operation Log- SaveOperationProc function", err.ToString());
                        }
                    }


                    strReturn = gClass.SaveOperationData(cmdOperation);
                    UpdatePatientData_InPatientWeightDataTable();


                    if (txtHAdmitID.Value.Equals("0")) // means new Patient Data, data must be inserted
                    {
                        txtHAdmitID.Value = strReturn;
                        gClass.SaveUserLogFile(Context.Request.Cookies["UserPracticeCode"].Value,
                                                Context.Request.Cookies["Logon_UserName"].Value,
                                                Context.Request.Url.Host,
                                                "Operation Form", 2, "Add Operation - PID : ", "Operation Code",
                                                strReturn);

                        blnInsert = true;
                    }
                    else
                    {
                        strReturn = txtHAdmitID.Value; // UPDATE, DON'T CHANGE IT!!!!
                        gClass.SaveUserLogFile(Context.Request.Cookies["UserPracticeCode"].Value,
                                                Context.Request.Cookies["Logon_UserName"].Value,
                                                Context.Request.Url.Host,
                                                "Operation Form", 2, "Modify Operation", "Operation Code", txtHAdmitID.Value);

                        blnInsert = false;
                    }

                    gClass.SaveActionLog(gClass.OrganizationCode,
                                            Context.Request.Cookies["UserPracticeCode"].Value,
                                            Context.Request.Url.Host,
                                            System.Configuration.ConfigurationManager.AppSettings["OperationPage"].ToString(),
                                            ((blnInsert) ? System.Configuration.ConfigurationManager.AppSettings["LogCreate"].ToString() : System.Configuration.ConfigurationManager.AppSettings["LogUpdate"].ToString()),
                                            "Save " + System.Configuration.ConfigurationManager.AppSettings["OperationPage"].ToString() + " Data ",
                                            Context.Request.Cookies["PatientID"].Value,
                                            strReturn);

                    //save BOLD
                    try
                    {
                        cmdSave.Parameters.Add("@AdmitId", SqlDbType.Int).Value = Convert.ToInt32(txtHAdmitID.Value);

                        gClass.ExecuteDMLCommand(cmdSave);
                        gClass.SaveUserLogFile(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Cookies["Logon_UserName"].Value,
                                                Context.Request.Url.Host, "Operatoin Form", 2, "Save Operation BOLD Data", "Operation Code", txtHAdmitID.Value);


                        gClass.SaveActionLog(gClass.OrganizationCode,
                                                Context.Request.Cookies["UserPracticeCode"].Value,
                                                Context.Request.Url.Host,
                                                System.Configuration.ConfigurationManager.AppSettings["OperationPage"].ToString(),
                                                ((blnInsert) ? System.Configuration.ConfigurationManager.AppSettings["LogCreate"].ToString() : System.Configuration.ConfigurationManager.AppSettings["LogUpdate"].ToString()),
                                                "Save " + System.Configuration.ConfigurationManager.AppSettings["OperationPage"].ToString() + " BOLD Data ",
                                                Context.Request.Cookies["PatientID"].Value,
                                                strReturn);

                        //submit to BOLD
                        if (Request.Cookies["SubmitData"].Value.IndexOf("submitbold") >= 0)
                        {
                            SubmitBOLDHospitalVisitData(Convert.ToInt32(txtHAdmitID.Value));
                        }
                    }
                    catch (Exception err)
                    {
                        gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                                    Context.Request.Cookies["Logon_UserName"].Value, "Operation  PID : " + Context.Request.Cookies["PatientID"].Value, "Data saving Operation - SaveOperationProc function", err.ToString());
                    }

                    // the selected lists should be reloaded
                    FillSelectedLists(cmbIntraOperativeEvents, listIntraOperativeEvents_Selected, txtIntraOperativeEvents_Selected.Value);
                    FillSelectedLists(cmbPreDischargeEvents, listPreDischargeEvents_Selected, txtPreDischargeEvents_Selected.Value);
                    FillSelectedLists(cmbConcurrent, listConcurrent_Selected, txtConcurrent_Selected.Value);
                    strScript = "javascript:SetEvents();UpdateOtherFieldsBasedOnSelectedSurgeryType();";
                    strScript += "linkBtnSave_CallBack(true);";
                }
                else
                {
                    BoldErrorMsg = "<br/>Please check the following:" + BoldErrorMsg;

                    strScript = "javascript:SetEvents();UpdateOtherFieldsBasedOnSelectedSurgeryType();";
                    strScript += "linkBtnSave_CallBack(false,'" + BoldErrorMsg + "');";
                    ScriptManager.RegisterStartupScript(linkBtnSave, linkBtnSave.GetType(), Guid.NewGuid().ToString(), strScript, true);
                    div_vDetail.Style["display"] = "block";
                }
            }
            catch (Exception err)
            {
                gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                            Context.Request.Cookies["Logon_UserName"].Value, "Operation  PID : " + Context.Request.Cookies["PatientID"].Value, "Data saving Operation - SaveOperationProc function", err.ToString());
                strScript += "linkBtnSave_CallBack(false);";
            }
            ScriptManager.RegisterStartupScript(linkBtnSave, linkBtnSave.GetType(), Guid.NewGuid().ToString(), strScript, true);
        }
        else
        {
            strScript = "javascript:SetEvents();linkBtnSave_CallBack('load');";
            ScriptManager.RegisterStartupScript(linkBtnSave, linkBtnSave.GetType(), Guid.NewGuid().ToString(), strScript, true);
        }
    }

    #endregion 

    #region private void UpdatePatientData_InPatientWeightDataTable()
    private void UpdatePatientData_InPatientWeightDataTable()
    {
        GlobalClass gClass = new GlobalClass();
        gClass.OrganizationCode = Request.Cookies["OrganizationCode"].Value;
        SqlCommand cmdSelect = new SqlCommand();
        SqlCommand cmdUpdate = CreatePatientDataUpdateCommand();
        Decimal decResult = 0;
        DateTime dtResult = DateTime.Now;
        int intUserPracticeCode;

        int.TryParse(Request.Cookies["UserPracticeCode"].Value, out intUserPracticeCode);
        gClass.FetchSystemDetails(intUserPracticeCode); // We call it to get Imperial status

            
        cmdUpdate.Parameters["@OpWeight"].Value = Decimal.TryParse(txtHWeight.Value, out decResult) ? decResult : 0;

        if (chkUpdateWeight.Checked)
        {
            decResult = Decimal.TryParse(txtHWeight.Value, out decResult) ? decResult : 0;
            cmdUpdate.Parameters["@StartWeight"].Value = decResult;
            cmdUpdate.Parameters["@StartBMIWeight"].Value = decResult;

            cmdUpdate.Parameters["@SurgeryType"].Value = cmbSurgeryType.SelectedValue;
            cmdUpdate.Parameters["@BandType"].Value = ddlBandType.SelectedValue;
            cmdUpdate.Parameters["@BandSize"].Value = cmbBandSize.SelectedValue;
            cmdUpdate.Parameters["@Approach"].Value = cmbApproach.SelectedValue;
            cmdUpdate.Parameters["@Category"].Value = cmbCategory.SelectedValue;
            cmdUpdate.Parameters["@Group"].Value = cmbGroup.SelectedValue;

            cmdUpdate.Parameters["@StartNeck"].Value = DBNull.Value;
            cmdUpdate.Parameters["@StartWaist"].Value = DBNull.Value;
            cmdUpdate.Parameters["@StartHip"].Value = DBNull.Value;

            if (DateTime.TryParse(txtOperationDate.Text, out dtResult))
                cmdUpdate.Parameters["@OperationDate"].Value = dtResult;
            else cmdUpdate.Parameters["@OperationDate"].Value = DBNull.Value;


            cmdUpdate.Parameters["@InRoomTime"].Value = txtInRoomTimeH.Value.ToString().Trim() + ":" + txtInRoomTimeM.Value.ToString().Trim();
            cmdUpdate.Parameters["@OutRoomTime"].Value = txtOutRoomTimeH.Value.ToString().Trim() + ":" + txtOutRoomTimeM.Value.ToString().Trim();
            cmdUpdate.Parameters["@SurgeryStartTime"].Value = txtSurgeryStartH.Value.ToString().Trim() + ":" + txtSurgeryStartM.Value.ToString().Trim();
            cmdUpdate.Parameters["@SurgeryEndTime"].Value = txtSurgeryEndH.Value.ToString().Trim() + ":" + txtSurgeryEndM.Value.ToString().Trim();
            cmdUpdate.Parameters["@FirstAssistant"].Value = ListCheckBoxAnswer(new String[] { "chkAssistantNone", "chkAssistantPA", "chkAssistantJunior", "chkAssistantSenior", "chkAssistantMIS", "chkAssistantAttendingSurgeon", "chkAssistantAttendingOther" });
            cmdUpdate.Parameters["@PreopHematocrit"].Value = txtPreopHematocrit.Value.ToString().Trim();
            cmdUpdate.Parameters["@PreopAlbumin"].Value = txtPreopAlbumin.Value.ToString().Trim();
            cmdUpdate.Parameters["@ASACode"].Value = cmbAsaClassification.SelectedValue;
            cmdUpdate.Parameters["@Concurrent"].Value = txtConcurrent_Selected.Value;
            cmdUpdate.Parameters["@OtherProcedure"].Value = txtDetailOtherProcedure.Value.ToString().Trim();
            cmdUpdate.Parameters["@BloodTransfusion"].Value = Decimal.TryParse(txtBloodLoss.Text, out decResult) ? decResult : 0;

            try { cmdUpdate.Parameters["@PreopHematocritDate"].Value = Convert.ToDateTime(txtPreopHematocritDate.Text); }
            catch { cmdUpdate.Parameters["@PreopHematocritDate"].Value = DBNull.Value; }

            try { cmdUpdate.Parameters["@PreopAlbuminDate"].Value = Convert.ToDateTime(txtPreopAlbuminDate.Text); }
            catch { cmdUpdate.Parameters["@PreopAlbuminDate"].Value = DBNull.Value; }

            try { cmdUpdate.Parameters["@DischargeDate"].Value = Convert.ToDateTime(txtDischargeDate.Text); }
            catch { cmdUpdate.Parameters["@DischargeDate"].Value = DBNull.Value; }

            try { cmdUpdate.Parameters["@NumberBloodTranfusion"].Value = Convert.ToInt32(txtNumberBloodTranfusion.Text); }
            catch { cmdUpdate.Parameters["@NumberBloodTranfusion"].Value = DBNull.Value; }

            try { cmdUpdate.Parameters["@NumberIntraopTranfusion"].Value = Convert.ToInt32(txtIntraopTranfusion.Text); }
            catch { cmdUpdate.Parameters["@NumberIntraopTranfusion"].Value = DBNull.Value; }

            cmdUpdate.Parameters["@UnplannedAdmission"].Value = rbUnplannedAdmissionY.Checked ? 1 : 0;
            cmdUpdate.Parameters["@TransferAcuteCare"].Value = rbTransferY.Checked ? 1 : 0;
        }
        else
        {
            cmdUpdate.Parameters["@StartBMIWeight"].Value = DBNull.Value;
            cmdUpdate.Parameters["@StartNeck"].Value = DBNull.Value;
            cmdUpdate.Parameters["@StartWaist"].Value = DBNull.Value;
            cmdUpdate.Parameters["@StartHip"].Value = DBNull.Value;
            cmdUpdate.Parameters["@Group"].Value = DBNull.Value;

            cmdUpdate.Parameters["@StartWeight"].Value = DBNull.Value;
            cmdUpdate.Parameters["@SurgeryType"].Value = DBNull.Value;
            cmdUpdate.Parameters["@Approach"].Value = DBNull.Value;
            cmdUpdate.Parameters["@OperationDate"].Value = DBNull.Value;
            cmdUpdate.Parameters["@BandType"].Value = DBNull.Value;
            cmdUpdate.Parameters["@BandSize"].Value = DBNull.Value;
            cmdUpdate.Parameters["@Category"].Value = DBNull.Value;

            cmdUpdate.Parameters["@InRoomTime"].Value = DBNull.Value;
            cmdUpdate.Parameters["@OutRoomTime"].Value = DBNull.Value;
            cmdUpdate.Parameters["@SurgeryStartTime"].Value = DBNull.Value;
            cmdUpdate.Parameters["@SurgeryEndTime"].Value = DBNull.Value;
            cmdUpdate.Parameters["@FirstAssistant"].Value = DBNull.Value;
            cmdUpdate.Parameters["@PreopHematocrit"].Value = DBNull.Value;
            cmdUpdate.Parameters["@PreopHematocritDate"].Value = DBNull.Value;
            cmdUpdate.Parameters["@PreopAlbumin"].Value = DBNull.Value;
            cmdUpdate.Parameters["@PreopAlbuminDate"].Value = DBNull.Value;
            cmdUpdate.Parameters["@ASACode"].Value = DBNull.Value;
            cmdUpdate.Parameters["@Concurrent"].Value = DBNull.Value;
            cmdUpdate.Parameters["@OtherProcedure"].Value = DBNull.Value;
            cmdUpdate.Parameters["@BloodTransfusion"].Value = DBNull.Value;
            cmdUpdate.Parameters["@DischargeDate"].Value = DBNull.Value;

            cmdUpdate.Parameters["@NumberBloodTranfusion"].Value = DBNull.Value;
            cmdUpdate.Parameters["@NumberIntraopTranfusion"].Value = DBNull.Value;
            cmdUpdate.Parameters["@UnplannedAdmission"].Value = DBNull.Value;
            cmdUpdate.Parameters["@TransferAcuteCare"].Value = DBNull.Value;
        }

        //group should only be updated upon clicking the update baseline
        //request from Margaret
        //cmdUpdate.Parameters["@Group"].Value = cmbGroup.SelectedValue;

        try
        {
            gClass.ExecuteDMLCommand(cmdUpdate);

            gClass.SaveUserLogFile(Context.Request.Cookies["UserPracticeCode"].Value,
                            Context.Request.Cookies["Logon_UserName"].Value,
                            Context.Request.Url.Host,
                            "Operation Form", 2, "Modify StartWeight, BMIWeight, SurgeryType, Approach and etc in tblPatientWeightData", "PatientID",
                            Context.Request.Cookies["PatientID"].Value + "  Org : " + gClass.OrganizationCode);



            gClass.SaveActionLog(gClass.OrganizationCode,
                                    Context.Request.Cookies["UserPracticeCode"].Value,
                                    Context.Request.Url.Host,
                                    System.Configuration.ConfigurationManager.AppSettings["OperationPage"].ToString(),
                                    System.Configuration.ConfigurationManager.AppSettings["LogUpdate"].ToString(),
                                    "Save " + System.Configuration.ConfigurationManager.AppSettings["PatientWeightData"].ToString(),
                                    Context.Request.Cookies["PatientID"].Value,
                                    "");
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                        Context.Request.Cookies["Logon_UserName"].Value, "Operation  PID : " + Context.Request.Cookies["PatientID"].Value, "Data saving Operation - UpdatePatientData_InPatientWeightDataTable function", err.ToString());
        }

        return;
    }
    #endregion

    #region private void AddOleCommandParameters(GlobalClass gClass, string AdmitID, ref SqlCommand cmdOperation)
    private void AddOleCommandParameters(GlobalClass gClass, string AdmitID, ref SqlCommand cmdOperation)
    {
        if (AdmitID == "0")
            gClass.MakeStoreProcedureName(ref cmdOperation, "sp_PatientOperations_InsertData", true);
        else
            gClass.MakeStoreProcedureName(ref cmdOperation, "sp_PatientOperations_UpdateData", true);

        cmdOperation.Parameters.Add("@PageNo", SqlDbType.Int);
        cmdOperation.Parameters.Add("@OrganizationCode", SqlDbType.Int);
        cmdOperation.Parameters.Add("@UserPracticeCode", SqlDbType.Int);
        cmdOperation.Parameters.Add("@PatientId", SqlDbType.Int);
        cmdOperation.Parameters.Add("@SurgeonId", SqlDbType.Int);
        cmdOperation.Parameters.Add("@HospitalCode", SqlDbType.VarChar, 6);
        cmdOperation.Parameters.Add("@OperationDate", SqlDbType.DateTime);
        cmdOperation.Parameters.Add("@Duration", SqlDbType.Int);
        cmdOperation.Parameters.Add("@SurgeryType", SqlDbType.VarChar, 10);
        cmdOperation.Parameters.Add("@Approach", SqlDbType.VarChar, 50);
        cmdOperation.Parameters.Add("@Category", SqlDbType.VarChar, 10);
        cmdOperation.Parameters.Add("@Group", SqlDbType.VarChar, 3);
        cmdOperation.Parameters.Add("@BloodLoss", SqlDbType.Decimal);
        cmdOperation.Parameters.Add("@Banded", SqlDbType.Bit);
        cmdOperation.Parameters.Add("@TubeSize", SqlDbType.VarChar, 10);
        cmdOperation.Parameters.Add("@BPDIlealLength", SqlDbType.VarChar, 3);
        cmdOperation.Parameters.Add("@VBGStomaWrap", SqlDbType.VarChar, 10);
        cmdOperation.Parameters.Add("@RouxLimbLength", SqlDbType.VarChar, 3);
        cmdOperation.Parameters.Add("@BPDChannelLength", SqlDbType.VarChar, 3);
        cmdOperation.Parameters.Add("@VBGStomaSize", SqlDbType.VarChar, 3);
        cmdOperation.Parameters.Add("@RouxEnterostomy", SqlDbType.VarChar, 10);
        cmdOperation.Parameters.Add("@ReservoirSite", SqlDbType.VarChar, 30);
        cmdOperation.Parameters.Add("@BPDDuodenalSwitch", SqlDbType.Bit);
        cmdOperation.Parameters.Add("@RouxColic", SqlDbType.VarChar, 10);
        cmdOperation.Parameters.Add("@BalloonVolume", SqlDbType.Decimal);
        cmdOperation.Parameters.Add("@BPDStomachSize", SqlDbType.Decimal);
        cmdOperation.Parameters.Add("@BandType", SqlDbType.VarChar, 10);
        cmdOperation.Parameters.Add("@BandSize", SqlDbType.VarChar, 30);
        cmdOperation.Parameters.Add("@RouxGastric", SqlDbType.VarChar, 10);
        cmdOperation.Parameters.Add("@Pathway", SqlDbType.VarChar, 20);
        cmdOperation.Parameters.Add("@GeneralNotes", SqlDbType.VarChar, 1024);
        cmdOperation.Parameters.Add("@DaysInHospital", SqlDbType.Int);
        cmdOperation.Parameters.Add("@OpWeight", SqlDbType.Decimal);
        cmdOperation.Parameters.Add("@AdmitDate", SqlDbType.DateTime);
        cmdOperation.Parameters.Add("@Bougie", SqlDbType.VarChar,5);

        cmdOperation.Parameters.Add("@NumberBloodTranfusion", SqlDbType.Int);
        cmdOperation.Parameters.Add("@NumberIntraopTranfusion", SqlDbType.Int);
        cmdOperation.Parameters.Add("@UnplannedAdmission", SqlDbType.Bit);
        cmdOperation.Parameters.Add("@TransferAcuteCare", SqlDbType.Bit);

        cmdOperation.Parameters.Add("@InRoomTime", SqlDbType.VarChar, 5);
        cmdOperation.Parameters.Add("@OutRoomTime", SqlDbType.VarChar, 5);
        cmdOperation.Parameters.Add("@SurgeryStartTime", SqlDbType.VarChar, 5);
        cmdOperation.Parameters.Add("@SurgeryEndTime", SqlDbType.VarChar, 5);
        cmdOperation.Parameters.Add("@FirstAssistant", SqlDbType.VarChar);
        cmdOperation.Parameters.Add("@PreopHematocrit", SqlDbType.VarChar, 1024);
        cmdOperation.Parameters.Add("@PreopHematocritDate", SqlDbType.DateTime);
        cmdOperation.Parameters.Add("@PreopAlbumin", SqlDbType.VarChar, 1024);
        cmdOperation.Parameters.Add("@PreopAlbuminDate", SqlDbType.DateTime);

        cmdOperation.Parameters.Add("@OtherProcedure", SqlDbType.VarChar);

        cmdOperation.Parameters.Add("@StartNeck", SqlDbType.Decimal);
        cmdOperation.Parameters.Add("@StartWaist", SqlDbType.Decimal);
        cmdOperation.Parameters.Add("@StartHip", SqlDbType.Decimal);
        cmdOperation.Parameters.Add("@PreviousSurgery", SqlDbType.Bit);
        cmdOperation.Parameters.Add("@PrevAbdoSurgery1", SqlDbType.VarChar, 10);
        cmdOperation.Parameters.Add("@PrevAbdoSurgery2", SqlDbType.VarChar, 10);
        cmdOperation.Parameters.Add("@PrevAbdoSurgery3", SqlDbType.VarChar, 10);
        cmdOperation.Parameters.Add("@PrevAbdoSurgeryNotes", SqlDbType.VarChar, 255);
        cmdOperation.Parameters.Add("@PrevPelvicSurgery", SqlDbType.Bit);
        cmdOperation.Parameters.Add("@PrevPelvicSurgery1", SqlDbType.VarChar, 10);
        cmdOperation.Parameters.Add("@PrevPelvicSurgery2", SqlDbType.VarChar, 10);
        cmdOperation.Parameters.Add("@PrevPelvicSurgery3", SqlDbType.VarChar, 10);
        cmdOperation.Parameters.Add("@PrevPelvicSurgeryNotes", SqlDbType.VarChar, 255);
        cmdOperation.Parameters.Add("@ComcomitantSurgery", SqlDbType.Bit);
        cmdOperation.Parameters.Add("@ComcomitantSurgery1", SqlDbType.VarChar, 10);
        cmdOperation.Parameters.Add("@ComcomitantSurgery2", SqlDbType.VarChar, 10);
        cmdOperation.Parameters.Add("@ComcomitantSurgery3", SqlDbType.VarChar, 10);
        cmdOperation.Parameters.Add("@ComcomitantSurgeryNotes", SqlDbType.VarChar, 255);

        gClass.InitialParameters(ref cmdOperation);

        if (AdmitID != "0") cmdOperation.Parameters.Add("@AdmitID", SqlDbType.Int).Value = Convert.ToInt32(AdmitID);

        gClass.AddLogParameters(ref cmdOperation,
                Context.Request.Cookies["Logon_UserName"].Value,
                Context.Request.UserHostAddress,
                (AdmitID == "0") ? "insert" : "update");
    }
    #endregion

    #region private OleDbCommand CreatePatientDataUpdateCommand()
    private SqlCommand CreatePatientDataUpdateCommand()
    {
        SqlCommand cmdUpdate = new SqlCommand();

        gClass.MakeStoreProcedureName(ref cmdUpdate, "sp_PatientData_cmdUpdateDataFromOperationEvent", true);
        cmdUpdate.Parameters.Add("@OperationDate", SqlDbType.DateTime);
        cmdUpdate.Parameters.Add("@OpWeight", SqlDbType.Decimal);
        cmdUpdate.Parameters.Add("@StartBMIWeight", SqlDbType.Decimal);
        cmdUpdate.Parameters.Add("@StartNeck", SqlDbType.Decimal);
        cmdUpdate.Parameters.Add("@StartWaist", SqlDbType.Decimal);
        cmdUpdate.Parameters.Add("@StartHip", SqlDbType.Decimal);
        cmdUpdate.Parameters.Add("@StartWeight", SqlDbType.Decimal);
        cmdUpdate.Parameters.Add("@SurgeryType", SqlDbType.VarChar, 10);
        cmdUpdate.Parameters.Add("@BandType", SqlDbType.VarChar, 10);
        cmdUpdate.Parameters.Add("@BandSize", SqlDbType.VarChar, 10);
        cmdUpdate.Parameters.Add("@Approach", SqlDbType.VarChar, 50);
        cmdUpdate.Parameters.Add("@Category", SqlDbType.VarChar, 10);
        cmdUpdate.Parameters.Add("@Group", SqlDbType.VarChar, 3);

        cmdUpdate.Parameters.Add("@InRoomTime", SqlDbType.VarChar, 5);
        cmdUpdate.Parameters.Add("@OutRoomTime", SqlDbType.VarChar, 5);
        cmdUpdate.Parameters.Add("@SurgeryStartTime", SqlDbType.VarChar, 5);
        cmdUpdate.Parameters.Add("@SurgeryEndTime", SqlDbType.VarChar, 5);
        cmdUpdate.Parameters.Add("@FirstAssistant", SqlDbType.VarChar);
        cmdUpdate.Parameters.Add("@PreopHematocrit", SqlDbType.VarChar, 1024);
        cmdUpdate.Parameters.Add("@PreopHematocritDate", SqlDbType.DateTime);
        cmdUpdate.Parameters.Add("@PreopAlbumin", SqlDbType.VarChar, 1024);
        cmdUpdate.Parameters.Add("@PreopAlbuminDate", SqlDbType.DateTime);
        cmdUpdate.Parameters.Add("@ASACode", SqlDbType.VarChar, 10);
        cmdUpdate.Parameters.Add("@Concurrent", SqlDbType.VarChar, 1024);
        cmdUpdate.Parameters.Add("@OtherProcedure", SqlDbType.VarChar);
        cmdUpdate.Parameters.Add("@BloodTransfusion", SqlDbType.Decimal);
        cmdUpdate.Parameters.Add("@DischargeDate", SqlDbType.DateTime);

        cmdUpdate.Parameters.Add("@NumberBloodTranfusion", SqlDbType.Int);
        cmdUpdate.Parameters.Add("@NumberIntraopTranfusion", SqlDbType.Int);
        cmdUpdate.Parameters.Add("@UnplannedAdmission", SqlDbType.Bit);
        cmdUpdate.Parameters.Add("@TransferAcuteCare", SqlDbType.Bit);

        
        cmdUpdate.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(Context.Request.Cookies["PatientID"].Value);
        cmdUpdate.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Context.Request.Cookies["UserPracticeCode"].Value);
        cmdUpdate.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);

        return (cmdUpdate);
    }
    #endregion

    #region protected void linkBtnLoadOperation_OnClick(object sender, EventArgs e)
    protected void linkBtnLoadOperation_OnClick(object sender, EventArgs e)
    {
        SqlCommand cmdSelect = new SqlCommand();
        GlobalClass gClass = new GlobalClass();
        gClass.OrganizationCode = Request.Cookies["OrganizationCode"].Value;
        DataSet dsTemp;
        String strScript = String.Empty;

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_Operation_SelectPatientOperationData", true);
        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSelect.Parameters.Add("@AdmitId", SqlDbType.Int).Value = Convert.ToInt32(txtHAdmitID.Value);
        cmdSelect.Parameters.Add("@ImperialFlag", SqlDbType.Bit).Value = Request.Cookies["Imperial"].Value.Equals("True");

        try
        {
            dsTemp = gClass.FetchData(cmdSelect, "tblPatientOperation");
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                DataColumn dcStrDate = new DataColumn()
                            , dcstrAdmitDate = new DataColumn()
                            , dcstrDischargeDate = new DataColumn();

                dcStrDate.ColumnName = "strDateOperation";
                dcStrDate.DataType = Type.GetType("System.String");
                dcstrAdmitDate.ColumnName = "strAdmitDate";
                dcstrAdmitDate.DataType = Type.GetType("System.String");
                dcstrDischargeDate.ColumnName = "strDischargeDate";
                dcstrDischargeDate.DataType = Type.GetType("System.String");

                dsTemp.Tables[0].Columns.Add(dcStrDate);
                dsTemp.Tables[0].Columns.Add(dcstrAdmitDate);
                dsTemp.Tables[0].Columns.Add(dcstrDischargeDate);

                for (int Xh = 0; Xh < dsTemp.Tables[0].Rows.Count; Xh++)
                {
                    dsTemp.Tables[0].Rows[Xh]["strDateOperation"] = gClass.TruncateDate(dsTemp.Tables[0].Rows[Xh]["OperationDate"].ToString(), Request.Cookies["CultureInfo"].Value, 1);
                    dsTemp.Tables[0].Rows[Xh]["strAdmitDate"] = gClass.TruncateDate(dsTemp.Tables[0].Rows[Xh]["AdmitDate"].ToString(), Request.Cookies["CultureInfo"].Value, 1);
                    dsTemp.Tables[0].Rows[Xh]["strDischargeDate"] = gClass.TruncateDate(dsTemp.Tables[0].Rows[Xh]["DischargeDate"].ToString(), Request.Cookies["CultureInfo"].Value, 1);
                }

                dsTemp.AcceptChanges();
                LoadOperationData(dsTemp.Tables[0].DefaultView);
            }
            else
            {
                RemoveFirstItemFromDropDownList();
                btnSave.Disabled = false;
            }
            dsTemp.Dispose();
            strScript = "javascript:SetEvents();UpdateOtherFieldsBasedOnSelectedSurgeryType();";
            TestText.Value = strScript;
            ScriptManager.RegisterStartupScript(linkBtnLoad, linkBtnLoad.GetType(), Guid.NewGuid().ToString(), strScript, true);
            div_vDetail.Style["display"] = "block";
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                Context.Request.Cookies["Logon_UserName"].Value, "Operation form (Ajax form)",
                "Load Operation data - LoadOperationData function", err.ToString());
        }
        return;
    }
    #endregion

    #region private void LoadOperationData(DataView dvOperation)
    private void LoadOperationData(DataView dvOperation)
    {
        HtmlInputCheckBox tempControlCheckbox = new HtmlInputCheckBox();
        String tempCheckBox = "";

        cmbSurgeon.SelectedValue = dvOperation[0]["SurgeonId"].ToString();
        //txtOperationDate.Text = dvOperation[0]["strDateOperation"].ToString();
        txtOperationDate.Text = Lapbase.Data.Common.TruncateDate(dvOperation[0]["strDateOperation"].ToString(), Request.Cookies["CultureInfo"].Value);//XXX
        txtDuration.Text = dvOperation[0]["Duration"].ToString();
        cmbSurgeryType.SelectedValue = dvOperation[0]["SurgeryType"].ToString();

        if (cmbApproach.Items.FindByValue(dvOperation[0]["Approach"].ToString()) != null)
            cmbApproach.SelectedValue = dvOperation[0]["Approach"].ToString();

        cmbGroup.SelectedValue = dvOperation[0]["Group"].ToString();
        cmbCategory.SelectedValue = dvOperation[0]["Category"].ToString();
        //txtBloodLoss.Text = dvOperation[0]["Group"].ToString();
        chkBanded.Checked = dvOperation[0]["Banded"].ToString().ToLower().Equals("true");
        txtTubeSize.Text = dvOperation[0]["TubeSize"].ToString();
        txtBPDIlealLength.Text = dvOperation[0]["BPDIlealLength"].ToString();
        cmbVBGStomaWrap.SelectedValue = dvOperation[0]["VBGStomaWrap"].ToString();
        txtRouxLimbLength.Text = dvOperation[0]["RouxLimbLength"].ToString();
        txtBPDChannelLength.Text = dvOperation[0]["BPDChannelLength"].ToString();
        txtVBGStomaSize.Text = dvOperation[0]["VBGStomaSize"].ToString();
        cmbRouxEnterostomy.SelectedValue = dvOperation[0]["RouxEnterostomy"].ToString();
        cmbReservoirSite.SelectedValue = dvOperation[0]["ReservoirSite"].ToString();
        chkBPDDuodenalSwitch.Checked = dvOperation[0]["BPDDuodenalSwitch"].ToString().ToLower().Equals("true");
        cmbRouxColic.SelectedValue = dvOperation[0]["RouxColic"].ToString();
        txtBalloonVolume.Text = dvOperation[0]["BalloonVolume"].ToString();
        txtBPDStomachSize.Text = dvOperation[0]["BPDStomachSize"].ToString();
        cmbBandSize.SelectedValue = dvOperation[0]["BandSize"].ToString();
        cmbRouxGastric.SelectedValue = dvOperation[0]["RouxGastric"].ToString();
        cmbPathway.SelectedValue = dvOperation[0]["Pathway"].ToString();
        cmbHospital.SelectedValue = dvOperation[0]["HospitalCode"].ToString();
        txtLosPostOp.Text = dvOperation[0]["DaysInHospital"].ToString();
        Decimal decTemp = 0;
        txtHWeight.Value = Decimal.TryParse(dvOperation[0]["OpWeight"].ToString(), out decTemp) ? Math.Round(decTemp,1).ToString() : "0";
        txtWeight.Text = Decimal.TryParse(dvOperation[0]["OperationWeight"].ToString(), out decTemp) ? Math.Round(decTemp,1).ToString() : "0"; //dvOperation[0]["OperationWeight"].ToString();
        if (txtWeight.Text == "0.0" || txtWeight.Text == "0")
            txtWeight.Text = "";
        txtAdmissionDate.Text = Lapbase.Data.Common.TruncateDate(dvOperation[0]["AdmitDate"].ToString(), Request.Cookies["CultureInfo"].Value);//XXX
        txtBloodTransfusion.Text = dvOperation[0]["BloodTransfusion"].ToString();
        txtAnesthesiaDuration.Text = dvOperation[0]["AnesthesiaDuration"].ToString();
        cmbAsaClassification.SelectedValue = dvOperation[0]["ASACode"].ToString();

        chkResident.Checked = dvOperation[0]["SurgicalResidentParticipated"].ToString().ToLower().Equals("true");
        chkFellow.Checked = dvOperation[0]["SurgicalFellowParticipated"].ToString().ToLower().Equals("true");


        chkAntigloculan.Checked = dvOperation[0]["DVTAntigloculan"].ToString().ToLower().Equals("true");
        chkTED.Checked = dvOperation[0]["DVTTED"].ToString().ToLower().Equals("true");
        chkFootPump.Checked = dvOperation[0]["DVTFootPump"].ToString().ToLower().Equals("true");
        chkCompress.Checked = dvOperation[0]["DVTCompress"].ToString().ToLower().Equals("true");

        txtAfterSurgery.Text = dvOperation[0]["TimeAfterSurgery"].ToString();

        if (cmbAfterSurgeryMeasurement.Items.FindByValue(dvOperation[0]["TimeAfterSurgeryMeasurement"].ToString()) != null)
            cmbAfterSurgeryMeasurement.SelectedValue = dvOperation[0]["TimeAfterSurgeryMeasurement"].ToString();

        cmbAdverseSurgeon.SelectedValue = dvOperation[0]["PreDischargeSurgeon"].ToString();
        cmbAdverseSurgery.SelectedValue = dvOperation[0]["PreDischargeSurgery"].ToString();

        txtDischargeDate.Text = Lapbase.Data.Common.TruncateDate(dvOperation[0]["DischargeDate"].ToString(), Request.Cookies["CultureInfo"].Value);//XXX
        
        cmbDischargeTo.SelectedValue = dvOperation[0]["DischargeTo"].ToString();
        txtSerialNo.Text = dvOperation[0]["SerialNo"].ToString();
        if (ddlBandType.Items.FindByValue(dvOperation[0]["LapbandType"].ToString()) != null)
            ddlBandType.SelectedValue = dvOperation[0]["LapbandType"].ToString();

        txtIntraOperativeEvents_Selected.Value = dvOperation[0]["IntraEvents"].ToString();
        txtPreDischargeEvents_Selected.Value = dvOperation[0]["PreDischargeEvents"].ToString();
        FillSelectedLists(cmbIntraOperativeEvents, listIntraOperativeEvents_Selected, txtIntraOperativeEvents_Selected.Value);
        FillSelectedLists(cmbPreDischargeEvents, listPreDischargeEvents_Selected, txtPreDischargeEvents_Selected.Value);


        txtConcurrent_Selected.Value = dvOperation[0]["Concurrent"].ToString();
        FillSelectedLists(cmbConcurrent, listConcurrent_Selected, txtConcurrent_Selected.Value);

        txtHDateCreated.Value = Lapbase.Data.Common.TruncateDate(dvOperation[0]["DateCreated"].ToString(), Request.Cookies["CultureInfo"].Value);
        String createdDate = txtHDateCreated.Value;
        String currentDate = txtHCurrentDate.Value;

        txtSleeveBougie.Text = dvOperation[0]["Bougie"].ToString().Trim();

        txtNumberBloodTranfusion.Text = dvOperation[0]["NumberBloodTranfusion"].ToString().Trim();
        txtIntraopTranfusion.Text = dvOperation[0]["NumberIntraopTranfusion"].ToString().Trim();
        rbUnplannedAdmissionY.Checked = dvOperation[0]["UnplannedAdmission"].ToString().Equals(Boolean.TrueString);
        rbUnplannedAdmissionN.Checked = dvOperation[0]["UnplannedAdmission"].ToString().Equals(Boolean.FalseString);
        rbTransferY.Checked = dvOperation[0]["TransferAcuteCare"].ToString().Equals(Boolean.TrueString);
        rbTransferN.Checked = dvOperation[0]["TransferAcuteCare"].ToString().Equals(Boolean.FalseString);


        txtPreopHematocrit.Value = dvOperation[0]["PreopHematocrit"].ToString().Trim();
        txtPreopHematocritDate.Text = Lapbase.Data.Common.TruncateDate(dvOperation[0]["PreopHematocritDate"].ToString(), Request.Cookies["CultureInfo"].Value);
        txtPreopAlbumin.Value = dvOperation[0]["PreopAlbumin"].ToString().Trim();
        txtPreopAlbuminDate.Text = Lapbase.Data.Common.TruncateDate(dvOperation[0]["PreopAlbuminDate"].ToString(), Request.Cookies["CultureInfo"].Value);


        //load registry
        cmbRegistryProcedure.SelectedValue = dvOperation[0]["RegistryProcedure"].ToString();


        String inRoomTime = dvOperation[0]["InRoomTime"].ToString().Trim();
        String outRoomTime = dvOperation[0]["OutRoomTime"].ToString().Trim();
        String startSurgeryTime = dvOperation[0]["SurgeryStartTime"].ToString().Trim();
        String endSurgeryTime = dvOperation[0]["SurgeryEndTime"].ToString().Trim();

        String[] fullInRoomTime;
        String[] fullOutRoomTime;
        String[] fullStartSurgeryTime;
        String[] fullEndSurgeryTime;

        if (inRoomTime != "")
        {
            fullInRoomTime = inRoomTime.Split(':');
            txtInRoomTimeH.Value = fullInRoomTime[0];
            txtInRoomTimeM.Value = fullInRoomTime[1];
        }

        if (outRoomTime != "")
        {
            fullOutRoomTime = outRoomTime.Split(':');
            txtOutRoomTimeH.Value = fullOutRoomTime[0];
            txtOutRoomTimeM.Value = fullOutRoomTime[1];
        }

        if (startSurgeryTime != "")
        {
            fullStartSurgeryTime = startSurgeryTime.Split(':');
            txtSurgeryStartH.Value = fullStartSurgeryTime[0];
            txtSurgeryStartM.Value = fullStartSurgeryTime[1];
        }

        if (endSurgeryTime != "")
        {
            fullEndSurgeryTime = endSurgeryTime.Split(':');
            txtSurgeryEndH.Value = fullEndSurgeryTime[0];
            txtSurgeryEndM.Value = fullEndSurgeryTime[1];
        }


        //FirstAssistant
        string[] tempFirstAssistant;
        if (dvOperation[0]["FirstAssistant"].ToString().Trim() != "")
        {
            tempFirstAssistant = dvOperation[0]["FirstAssistant"].ToString().Split('-');
            foreach (string firstAssistant in tempFirstAssistant)
            {
                tempControlCheckbox = (HtmlInputCheckBox)FindControlRecursive(this.Page, firstAssistant);
                tempControlCheckbox.Checked = true;
            }
        }




        String[] otherProcedures;
        String[] otherProcedure;
        String otherProcedureName;
        String otherProcedureCode;
        String tempOtherProcedureList = "";
        Int32 totalOtherProcedure = 0;

        otherProcedureDiv.InnerHtml = "";
        if (dvOperation[0]["OtherProcedure"].ToString().Trim() != "")
        {
            otherProcedures = dvOperation[0]["OtherProcedure"].ToString().Split('+');

            foreach (string tempOtherProcedure in otherProcedures)
            {
                totalOtherProcedure++;
                otherProcedure = tempOtherProcedure.Split('*');
                otherProcedureName = otherProcedure[0];
                otherProcedureCode = otherProcedure[1];

                tempOtherProcedureList += "<div id='idOtherProcedure" + totalOtherProcedure + "div'><input type=hidden name=delOtherProcedure" + totalOtherProcedure + " value=no>Procedure: <input type=textbox runat='server' name='txtOtherProcedureName" + totalOtherProcedure + "' value='" + otherProcedureName + "' size='55'> &nbsp; &nbsp; CPT: <input type=textbox runat='server' name='txtOtherProcedureCode" + totalOtherProcedure + "' value='" + otherProcedureCode + "' size='5'><input type=button name=remove" + totalOtherProcedure + " value=' - ' onclick='javascript:removeOtherProcedure(" + totalOtherProcedure + ",\"idOtherProcedure" + totalOtherProcedure + "div\")'></div>";
            }
            otherProcedureDiv.InnerHtml = tempOtherProcedureList;
        }
        totalOtherProcedures.Value = totalOtherProcedure.ToString();

        //if dataclamp is activated, permission lvl 2 or 3
        //check for created date for this patient
        btnSave.Disabled = false;
        if (txtHDataClamp.Value.ToLower() == "true" && (txtHPermissionLevel.Value == "2t" || txtHPermissionLevel.Value == "3f"))
        {
            if (createdDate != "" && createdDate != currentDate)
                btnSave.Disabled = true;        
        }


        RemoveFirstItemFromDropDownList();


        /** /
         * //Page 2
            try{document.getElementById("txtStartNeck_txtGlobal").value = XmlDoc.getElementsByTagName("StartNeck")[0].hasChildNodes() ?  XmlDoc.getElementsByTagName("StartNeck")[0].firstChild.nodeValue : "";}catch(e){}
            try{document.getElementById("txtStartWaist_txtGlobal").value = XmlDoc.getElementsByTagName("StartWaist")[0].hasChildNodes() ?  XmlDoc.getElementsByTagName("StartWaist")[0].firstChild.nodeValue : "";}catch(e){}
            try{document.getElementById("txtStartHip_txtGlobal").value = XmlDoc.getElementsByTagName("StartHip")[0].hasChildNodes() ?  XmlDoc.getElementsByTagName("StartHip")[0].firstChild.nodeValue : "";}catch(e){}
            
            CalculateWHRatio(document.getElementById("txtStartHip_txtGlobal").value, document.getElementById("txtStartWaist_txtGlobal").value);
            
            try{document.getElementById("chkPrevAbdoSurgery").checked = XmlDoc.getElementsByTagName("PreviousSurgery")[0].hasChildNodes() ?  (XmlDoc.getElementsByTagName("PreviousSurgery")[0].firstChild.nodeValue == "true") : false;}catch(e){}  
            try{document.getElementById("cmbPrevAbdoSurgery1_CodeList").value = XmlDoc.getElementsByTagName("PrevAbdoSurgery1")[0].hasChildNodes() ?  XmlDoc.getElementsByTagName("PrevAbdoSurgery1")[0].firstChild.nodeValue : "";}catch(e){}
            try{document.getElementById("cmbPrevAbdoSurgery2_CodeList").value = XmlDoc.getElementsByTagName("PrevAbdoSurgery2")[0].hasChildNodes() ?  XmlDoc.getElementsByTagName("PrevAbdoSurgery2")[0].firstChild.nodeValue : "";}catch(e){}
            try{document.getElementById("cmbPrevAbdoSurgery3_CodeList").value= XmlDoc.getElementsByTagName("PrevAbdoSurgery3")[0].hasChildNodes() ?  XmlDoc.getElementsByTagName("PrevAbdoSurgery3")[0].firstChild.nodeValue : "";}catch(e){}
            try{document.getElementById("txtPrevAbdoSurgeryNotes_txtGlobal").value = XmlDoc.getElementsByTagName("PrevAbdoSurgeryNotes")[0].hasChildNodes() ?  XmlDoc.getElementsByTagName("PrevAbdoSurgeryNotes")[0].firstChild.nodeValue : "";}catch(e){}
            try{document.getElementById("cmbPrevAbdoSurgery1_CodeList").disabled = XmlDoc.getElementsByTagName("PreviousSurgery")[0].hasChildNodes() ?  (XmlDoc.getElementsByTagName("PreviousSurgery")[0].firstChild.nodeValue != "true") : false;}catch(e){}
            try{document.getElementById("cmbPrevAbdoSurgery2_CodeList").disabled = XmlDoc.getElementsByTagName("PreviousSurgery")[0].hasChildNodes() ?  (XmlDoc.getElementsByTagName("PreviousSurgery")[0].firstChild.nodeValue != "true") : false;}catch(e){}
            try{document.getElementById("cmbPrevAbdoSurgery3_CodeList").disabled = XmlDoc.getElementsByTagName("PreviousSurgery")[0].hasChildNodes() ?  (XmlDoc.getElementsByTagName("PreviousSurgery")[0].firstChild.nodeValue != "true") : false;}catch(e){}
            try{document.getElementById("txtPrevAbdoSurgeryNotes_txtGlobal").disabled = XmlDoc.getElementsByTagName("PreviousSurgery")[0].hasChildNodes() ?  (XmlDoc.getElementsByTagName("PreviousSurgery")[0].firstChild.nodeValue != "true") : false;}catch(e){}
            
            try{document.getElementById("chkPrevPelvicSurgery").checked = XmlDoc.getElementsByTagName("PrevPelvicSurgery")[0].hasChildNodes() ?  (XmlDoc.getElementsByTagName("PrevPelvicSurgery")[0].firstChild.nodeValue == "true") : false;}catch(e){}
            try{document.getElementById("cmbPrevPelvicSurgery1_CodeList").value = XmlDoc.getElementsByTagName("PrevPelvicSurgery1")[0].hasChildNodes() ?  XmlDoc.getElementsByTagName("PrevPelvicSurgery1")[0].firstChild.nodeValue : "";}catch(e){}
            try{document.getElementById("cmbPrevPelvicSurgery2_CodeList").value = XmlDoc.getElementsByTagName("PrevPelvicSurgery2")[0].hasChildNodes() ?  XmlDoc.getElementsByTagName("PrevPelvicSurgery2")[0].firstChild.nodeValue : "";}catch(e){}
            try{document.getElementById("cmbPrevPelvicSurgery3_CodeList").value = XmlDoc.getElementsByTagName("PrevPelvicSurgery3")[0].hasChildNodes() ?  XmlDoc.getElementsByTagName("PrevPelvicSurgery3")[0].firstChild.nodeValue : "";}catch(e){}
            try{document.getElementById("txtPrevPelvicSurgeryNotes_txtGlobal").value = XmlDoc.getElementsByTagName("PrevPelvicSurgeryNotes")[0].hasChildNodes() ?  XmlDoc.getElementsByTagName("PrevPelvicSurgeryNotes")[0].firstChild.nodeValue : "";}catch(e){}
            try{document.getElementById("cmbPrevPelvicSurgery1_CodeList").disabled = XmlDoc.getElementsByTagName("PrevPelvicSurgery")[0].hasChildNodes() ?  (XmlDoc.getElementsByTagName("PrevPelvicSurgery")[0].firstChild.nodeValue != "true") : false;}catch(e){}
            try{document.getElementById("cmbPrevPelvicSurgery2_CodeList").disabled = XmlDoc.getElementsByTagName("PrevPelvicSurgery")[0].hasChildNodes() ?  (XmlDoc.getElementsByTagName("PrevPelvicSurgery")[0].firstChild.nodeValue != "true") : false;}catch(e){}
            try{document.getElementById("cmbPrevPelvicSurgery3_CodeList").disabled = XmlDoc.getElementsByTagName("PrevPelvicSurgery")[0].hasChildNodes() ?  (XmlDoc.getElementsByTagName("PrevPelvicSurgery")[0].firstChild.nodeValue != "true") : false;}catch(e){}
            try{document.getElementById("txtPrevPelvicSurgeryNotes_txtGlobal").disabled = XmlDoc.getElementsByTagName("PrevPelvicSurgeryNotes")[0].hasChildNodes() ?  (XmlDoc.getElementsByTagName("PrevPelvicSurgeryNotes")[0].firstChild.nodeValue == "true") : false;}catch(e){}
            
            try{document.getElementById("chkComcomitantSurgery").checked = XmlDoc.getElementsByTagName("ComcomitantSurgery")[0].hasChildNodes() ?  (XmlDoc.getElementsByTagName("ComcomitantSurgery")[0].firstChild.nodeValue == "true") : false;}catch(e){}
            try{document.getElementById("cmbComcomitantSurgery1_CodeList").value = XmlDoc.getElementsByTagName("ComcomitantSurgery1")[0].hasChildNodes() ?  XmlDoc.getElementsByTagName("ComcomitantSurgery1")[0].firstChild.nodeValue : "";}catch(e){}
            try{document.getElementById("cmbComcomitantSurgery2_CodeList").value  = XmlDoc.getElementsByTagName("ComcomitantSurgery2")[0].hasChildNodes() ? XmlDoc.getElementsByTagName("ComcomitantSurgery2")[0].firstChild.nodeValue : "";}catch(e){}
            try{document.getElementById("cmbComcomitantSurgery3_CodeList").value  = XmlDoc.getElementsByTagName("ComcomitantSurgery3")[0].hasChildNodes() ? XmlDoc.getElementsByTagName("ComcomitantSurgery3")[0].firstChild.nodeValue : "";}catch(e){}
            try{document.getElementById("txtComcomitantSurgeryNotes_txtGlobal").value = XmlDoc.getElementsByTagName("ComcomitantSurgeryNotes")[0].hasChildNodes() ?  XmlDoc.getElementsByTagName("ComcomitantSurgeryNotes")[0].firstChild.nodeValue : "";}catch(e){}
            try{document.getElementById("cmbComcomitantSurgery1_CodeList").disabled = XmlDoc.getElementsByTagName("ComcomitantSurgery")[0].hasChildNodes() ?  (XmlDoc.getElementsByTagName("ComcomitantSurgery")[0].firstChild.nodeValue != "true") : false;}catch(e){}
            try{document.getElementById("cmbComcomitantSurgery2_CodeList").disabled  = XmlDoc.getElementsByTagName("ComcomitantSurgery")[0].hasChildNodes() ?  (XmlDoc.getElementsByTagName("ComcomitantSurgery")[0].firstChild.nodeValue != "true") : false;}catch(e){}
            try{document.getElementById("cmbComcomitantSurgery3_CodeList").disabled  = XmlDoc.getElementsByTagName("ComcomitantSurgery")[0].hasChildNodes() ?  (XmlDoc.getElementsByTagName("ComcomitantSurgery")[0].firstChild.nodeValue != "true") : false;}catch(e){}
            try{document.getElementById("txtComcomitantSurgeryNotes_txtGlobal").disabled = XmlDoc.getElementsByTagName("ComcomitantSurgery")[0].hasChildNodes() ?  (XmlDoc.getElementsByTagName("ComcomitantSurgery")[0].firstChild.nodeValue != "true") : false;}catch(e){}
            
            //Based on Selected Surgeory, other fields should be vidible/hidden
            UpdateOtherFieldsBasedOnSelectedSurgeryType();
        /**/
    }
    #endregion

    #region private void RemoveFirstItemFromDropDownList()
    private void RemoveFirstItemFromDropDownList()
    {
        if (listPreDischargeEvents.Items.Count == 0)
            FillBoldList(cmbPreDischargeEvents, listPreDischargeEvents);

        if (listIntraOperativeEvents.Items.Count == 0)
            FillBoldList(cmbIntraOperativeEvents, listIntraOperativeEvents);

        if (listConcurrent.Items.Count == 0)
            FillBoldList(cmbConcurrent, listConcurrent);

        //listPreDischargeEvents.Items.RemoveAt(0);
        //listIntraOperativeEvents.Items.RemoveAt(0);
        //listConcurrent.Items.RemoveAt(0);
    }
    #endregion 

    #region private void FillSelectedLists(HtmlSelect listOrigin, HtmlSelect listSelected, String strListValue)
    private void FillSelectedLists(UserControl_SystemCodeWUCtrl listOrigin, HtmlSelect listSelected, String strListValue)
    {
        System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(";");
        String[] strItemCollection = regex.Split(strListValue);
        System.Collections.Generic.List<String[,]> strOptionCollection = new System.Collections.Generic.List<string[,]>();
        String strScript = String.Empty;

        TestText.Value += "  " + strListValue;
        listSelected.Items.Clear();
        listOrigin.FetchSystemCodeListData();
        foreach (String strItem in strItemCollection)
            foreach (ListItem item in listOrigin.Items)
                if ((strItem.Length > 0) && item.Value.Equals(strItem))
                    strOptionCollection.Add(new String[,] {{item.Value, item.Text}});

        strScript = "var objList = document.forms[0]." + listSelected.ClientID + ", option;";
        strScript += "objList.length = 0;";
        foreach (String[,] tempString in strOptionCollection)
        {
            strScript += "option = document.createElement('OPTION');";
            strScript += "option.value = '" + tempString[0,0] + "';";
            strScript += "option.text = '" + tempString[0,1] + "';";
            strScript += "objList.options.add(option);";
        }
        TestText.Value = strScript;
        ScriptManager.RegisterStartupScript(linkBtnLoad, linkBtnLoad.GetType(), Guid.NewGuid().ToString(), strScript, true);
    }
    #endregion

    #region protected void linkbtnLoadOperationsList_OnClick(object sender, EventArgs e)
    protected void linkbtnLoadOperationsList_OnClick(object sender, EventArgs e)
    {
        SqlCommand  cmdSelect = new SqlCommand();
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
                dsTemp.Tables[0].Rows[Xh]["strDateOperation"] = gClass.TruncateDate(dsTemp.Tables[0].Rows[Xh]["OperationDate"].ToString(), Request.Cookies["CultureInfo"].Value, 1);
            dsTemp.AcceptChanges();
            strReturn = gClass.ShowSchema(dsTemp, Server.MapPath(".") + @"\Includes\OperationsListXSLTFile.xsl");
        }
        catch (Exception err)
        {
            strReturn = String.Empty;

            gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                Context.Request.Cookies["Logon_UserName"].Value, "Operation form (Ajax form)",
                "Load Patient's Operation List - LoadPatientOperations function", err.ToString());
        }
        TestText.Value = strReturn;
        ScriptManager.RegisterStartupScript(linkbtnLoadOperationsList, linkbtnLoadOperationsList.GetType(), Guid.NewGuid().ToString(), "javascript:HideDivMessage();", true);
        div_OperationList.InnerHtml = strReturn;
        return;
    }
    #endregion 

    #region protected void linkBtnDeleteOperation_OnClick(object sender, EventArgs e)
    protected void linkBtnDeleteOperation_OnClick(object sender, EventArgs e)
    {
        SqlCommand cmdUpdate = new SqlCommand();
        string strScript = String.Empty;


        gClass.MakeStoreProcedureName(ref cmdUpdate, "sp_PatientOperations_DeleteData", true);
        cmdUpdate.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdUpdate.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
        cmdUpdate.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
        cmdUpdate.Parameters.Add("@AdmitId", SqlDbType.Int).Value = Convert.ToInt32(txtHAdmitID.Value);

        cmdUpdate.Parameters.Add("@DeletedByUser", SqlDbType.VarChar, 50).Value = Context.Request.Cookies["Logon_UserName"].Value;
        cmdUpdate.Parameters.Add("@DateDeleted", SqlDbType.DateTime).Value = Convert.ToDateTime(txtHCurrentDate.Value);

        cmdUpdate.Parameters.Add("@SurgeryType", SqlDbType.VarChar, 50).Value = cmbSurgeryType.SelectedValue;
        cmdUpdate.Parameters.Add("@OperationDate", SqlDbType.DateTime).Value = Convert.ToDateTime(txtOperationDate.Text);

        try
        {
            DataSet dsTemp = gClass.FetchData(cmdUpdate, "tblPatientTitle");
            gClass.SaveUserLogFile(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Cookies["Logon_UserName"].Value,
                                    Context.Request.Url.Host, "Operation Form", 2, "Delete Operation BOLD Data", "Operation Code", txtHAdmitID.Value);


            gClass.SaveActionLog(gClass.OrganizationCode,
                                    Context.Request.Cookies["UserPracticeCode"].Value,
                                    Context.Request.Url.Host,
                                    System.Configuration.ConfigurationManager.AppSettings["OperationPage"].ToString(),
                                    System.Configuration.ConfigurationManager.AppSettings["LogDelete"].ToString(),
                                    "Delete " + System.Configuration.ConfigurationManager.AppSettings["OperationPage"].ToString() + " List ",
                                    Context.Request.Cookies["PatientID"].Value,
                                    txtHAdmitID.Value);

            strScript = "javascript:ClearFields();";
            if (dsTemp.Tables[0].Rows[0]["LapBandDate"].ToString() == "")
            {
                strScript += "linkBtnSave_CallBack('delete');";
            }
            strScript += "window.location.reload();";
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                        Context.Request.Cookies["Logon_UserName"].Value, "Operation PID : " + Context.Request.Cookies["PatientID"].Value, "Data deleting Operation - DeleteOperationProc function", err.ToString());
            //strScript = "javascript:linkBtnSave_CallBack(false);";
        }

        ScriptManager.RegisterStartupScript(linkBtnSave, linkBtnSave.GetType(), Guid.NewGuid().ToString(), strScript, true);
        return;
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
        
    #region private void SubmitBOLDHospitalVisitData(Int32 intAdmitID)
    private void SubmitBOLDHospitalVisitData(Int32 intAdmitID)
    {
        Int32 intPatientID;
        Int32.TryParse(Context.Request.Cookies["PatientID"].Value.ToString(), out intPatientID);
        try
        {
            //save to BOLD----------------------------------------------------------------------------------------
            Lapbase.Business.SRCObject objSRC = new Lapbase.Business.SRCObject();

            objSRC.PatientID = intPatientID;
            objSRC.OrganizationCode = Convert.ToInt32(gClass.OrganizationCode);
            objSRC.Imperial = Convert.ToBoolean((Context.Request.Cookies["Imperial"].Value == "True") ? 1 : 0);
            objSRC.VendorCode = LapbaseConfiguration.SRCVendorCode;
            objSRC.PracticeCEO = LapbaseConfiguration.PracticeCEOCode;
            objSRC.SurgeonCEO = LapbaseConfiguration.SurgeonCEOCode;
            objSRC.FacilityCEO = LapbaseConfiguration.FacilityCEOCode;
            objSRC.SRCUserName = LapbaseConfiguration.SRCUserName;
            objSRC.SRCPassword = LapbaseConfiguration.SRCPassword;
            objSRC.SaveHospitalVisit(intAdmitID);
            
            if (objSRC.HospitalVisitErrors.Count > 0)
            {
                for (int Xh = 0; Xh < objSRC.HospitalVisitErrors.Count; Xh++)
                {
                    gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                                      Context.Request.Cookies["Logon_UserName"].Value, "Patient ID : " + intPatientID.ToString() + ", Admit ID:" + intAdmitID.ToString(), "BOLD - Data saving Operation ", objSRC.HospitalVisitErrors[Xh].ErrorMessage.ToString());
                }            
            }
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                        Context.Request.Cookies["Logon_UserName"].Value, "Patient ID : " + intPatientID.ToString() + ", Admit ID:" + intAdmitID.ToString(), "BOLD - Data saving Operation ", err.ToString());
        }
        //----------------------------------------------------------------------------------------------------
    }
    #endregion
        
    #region private void LoadBoldList()
    private void LoadBoldList()
    {
        SqlCommand cmdSelect = new SqlCommand();
        string strReturn = String.Empty;
        string doctor = "";
        string hospital = "";
        string doctorList = "";
        string doctorListWithOther = "";
        string hospitalList = "";
        int Xh;

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_Doctors_LoadData", true);
        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSelect.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);

        DataSet dsDoctorTemp = gClass.FetchData(cmdSelect, "tblDoctor");

        for (Xh = 0; Xh < dsDoctorTemp.Tables[0].Rows.Count; Xh++)
        {
            doctor = dsDoctorTemp.Tables[0].Rows[Xh]["DoctorBoldCode"].ToString().Trim();
            if (doctor != "" && doctor.ToLower() != "other")
                doctorList += dsDoctorTemp.Tables[0].Rows[Xh]["DoctorID"].ToString().Trim() + "-";
            if (doctor != "")
                doctorListWithOther += dsDoctorTemp.Tables[0].Rows[Xh]["DoctorID"].ToString().Trim() + "-";
        }
        txtHDoctorBoldList.Value = doctorList;
        txtHDoctorBoldListWithOther.Value = doctorListWithOther;

        cmdSelect.Parameters.Clear();
        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_Hospitals_LoadData", true);
        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);

        DataSet dsHospitalTemp = gClass.FetchData(cmdSelect, "tblHospital");

        for (Xh = 0; Xh < dsHospitalTemp.Tables[0].Rows.Count; Xh++)
        {
            hospital = dsHospitalTemp.Tables[0].Rows[Xh]["HospitalBoldCode"].ToString().Trim();
            if (hospital != "" && hospital.ToLower() != "other")
                hospitalList += dsHospitalTemp.Tables[0].Rows[Xh]["Hospital ID"].ToString().Trim() + "-";
        }
        txtHHospitalBoldList.Value = hospitalList;
    }
    #endregion

}