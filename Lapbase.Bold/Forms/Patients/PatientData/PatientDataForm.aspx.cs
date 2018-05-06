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
using Microsoft.Web.UI;
using Lapbase.Business;
using Lapbase.Configuration.ConfigurationApplication;

public partial class Forms_Patients_PatientData_PatientDataForm : BasePage 
{
    Int16 IsDoneSaveFlag = 0;
    Int32 int32Temp = 0;
    Decimal decTemp = 0;
    String strNumberFormat = System.Configuration.ConfigurationManager.AppSettings["NumberFormat"].ToString();
    GlobalClass gClass = new GlobalClass();
    String BoldErrorMsg = "";

    #region events

    protected override void OnLoad(EventArgs e)
    {
        txtHApplicationURL.Value = Request.Url.Scheme + "://" + base.DomainURL + "/";
        txtUseImperial.Value = (Request.Cookies["Imperial"].Value == "True") ? "1" : "0"; //base.Imperial ? "1" : "0"; 
        gClass.LanguageCode = base.LanguageCode;
        txtHCulture.Value = base.CultureInfo;
        InitialiseFormSetting();

        base.OnLoad(e);
    }
    #endregion 

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
            RegisterClientScript();
            if (txtPatientID.Value == "0")
                txtPatientID.Value = (Request.QueryString.Count > 0) ? Request.QueryString["PID"].ToString() : Request.Cookies["PatientID"].Value;
            Response.Cookies["PatientID"].Value = txtPatientID.Value;
            FillDropdownLists();
            if (!IsPostBack)
            {
                if (txtPatientID.Value.Equals("0"))
                {
                    gClass.SaveUserLogFile(base.UserPracticeCode.ToString(),
                                            Request.Cookies["Logon_UserName"].Value,
                                            Request.Url.Host,
                                            "Baseline Form", 2, "Open Form to add new patient", "", "");
                    LoadLastPatientCustomIDFromDatabase();
                }
                else
                {
                    gClass.SaveUserLogFile(base.UserPracticeCode.ToString(),
                                            Request.Cookies["Logon_UserName"].Value,
                                            Request.Url.Host,
                                            "Baseline Form", 2, "Open Form to see patient data", "PatientCode", txtPatientID.Value);
                    LoadPatientData();
                }



                if (Request.Cookies["PermissionLevel"].Value == "1o" || Request.Cookies["SubmitData"].Value.IndexOf("submitbold") >= 0)
                {
                    btnDeletePatient1.Style["display"] = "none";
                    btnDeletePatient2.Style["display"] = "none";
                    btnDeletePatient3.Style["display"] = "none";
                    btnDeletePatient4.Style["display"] = "none";
                    btnDeletePatient5.Style["display"] = "none";
                    btnDeletePatient6.Style["display"] = "none";
                }

                if (Request.Cookies["PermissionLevel"].Value == "1o")
                {
                    Button1.Style["display"] = "none";
                    Button2.Style["display"] = "none";
                    Button3.Style["display"] = "none";
                    Button4.Style["display"] = "none";
                    Button5.Style["display"] = "none";
                    Button6.Style["display"] = "none";
                    Button7.Style["display"] = "none";
                }


                if (Request.Cookies["SubmitData"].Value.IndexOf("bold") >= 0 || Request.Cookies["SubmitData"].Value == "")
                {
                    LoadBoldList();
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

    #region private void RegisterClientScript()
    private void RegisterClientScript()
    {
        Page.Culture = Request.Cookies["CultureInfo"].Value;
        txtHCurrentDate.Value = DateTime.Now.ToShortDateString();

        System.Globalization.CultureInfo myCI = new System.Globalization.CultureInfo(Request.Cookies["CultureInfo"].Value, false);
        lblDateFormat.Text = myCI.DateTimeFormat.ShortDatePattern.ToLower();
        txtBirthDate.toolTip = myCI.DateTimeFormat.ShortDatePattern;
        //txtZeroDate.toolTip = myCI.DateTimeFormat.ShortDatePattern;

        bodyPatientData.Style.Add("Direction", Request.Cookies["Direction"].Value);
        bodyPatientData.Attributes.Add("onload", "javascript:InitializePage();");
        cmbReferredDoctorsList.Style.Add("display", "none");
        cmbCity.Style.Add("display", "none");
        cmbInsurance.Style.Add("display", "none");
        txtCurrentWeight.SetStyle("display", "none");
        txtIdealWeight.SetStyle("display", "none");
        return;
    }
    #endregion

    #region private void FillDropdownLists( )
    private void FillDropdownLists( )
    {
        DataSet dsList = new DataSet();
        int Xt = 0;

        cmbCity.DataSource = gClass.FillCityList(base.OrganizationCode.ToString() , base.UserPracticeCode.ToString()); //intUserPracticeCode.ToString()
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
        dsList = gClass.FillReferredDoctorList(base.OrganizationCode.ToString() , base.UserPracticeCode.ToString()); //intUserPracticeCode.ToString()
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

    #region private void InitialiseFormSetting()
    private void InitialiseFormSetting()
    {
        lblWeightSize.Text = (txtUseImperial.Value == "1") ? "lbs" : "kgs";
        lblTxtIWeight.Text = (txtUseImperial.Value == "1") ? "lbs" : "kgs";
        lblTxtEWeight.Text = (txtUseImperial.Value == "1") ? "lbs" : "kgs";
        lblTxtETargetWeight.Text = (txtUseImperial.Value == "1") ? "lbs" : "kgs";
        lblTxtHeight.Text = (txtUseImperial.Value == "1") ? "inches" : "cms";
        lblCurrentEWL1.Text = (txtUseImperial.Value == "1") ? "lbs" : "kgs";
        lblOrgWeight_Unit.Text = (txtUseImperial.Value == "1") ? "lbs" : "kgs";
        lblPreOpWeightLoss_Unit.Text = (txtUseImperial.Value == "1") ? "lbs" : "kgs";
        lblLowestWeightAchieved_Unit.Text = (txtUseImperial.Value == "1") ? "lbs" : "kgs";

        gClass.FetchSystemDetails(base.UserPracticeCode); //intUserPracticeCode
        txtHRefBMI.Value = gClass.SD_ReferenceBMI.ToString();
        return;
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
        foreach (ListItem sourceItem in ddlSource.Items)
        {
            ListItem destItem = new ListItem();
            destItem.Value = sourceItem.Value;
            destItem.Text = sourceItem.Text;
            listDest.Items.Add(destItem);
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
            if(Convert.ToInt64(dsPatientCustomID.Tables[0].Rows[Xh]["PatientCustomID"].ToString()) > intPatientCustomID)
                intPatientCustomID = Convert.ToInt64(dsPatientCustomID.Tables[0].Rows[Xh]["PatientCustomID"].ToString());
        }

        if (intPatientCustomID == 0)
            LoadLastPatientIDFromDatabase();
        else
        {
            txtPatient_CustomID.Text = (intPatientCustomID+1).ToString();
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
                LoadPatientData_DemoGraphics(dsPatient.Tables[0].DefaultView);
                LoadPatientData_HeightWeightNotes(dsPatient.Tables[0].DefaultView);
                LoadPatientData_BoldDataGroup();
            }
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Patient Data Form", "Loading Patient Data - LoadPatientData function", err.ToString());
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
        txtZeroDate.Text = gClass.TruncateDate(dvPatient[0]["ZeroDate"].ToString().Trim(), Request.Cookies["CultureInfo"].Value, 1);
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
        
        SetReferredDoctors(txtReferredBy, txtHReferredBy.Value.Replace("`", "'"));
        SetReferredDoctors(txtOtherDoctors1, txtHOtherDoctors1.Value.Replace("`", "'"));
        SetReferredDoctors(txtOtherDoctors2, txtHOtherDoctors2.Value.Replace("`", "'"));

        txtSocialHistory.Text = dvPatient[0]["SocialHistory"].ToString();
    }
    #endregion

    #region private void LoadPatientData_HeightWeightNotes(DataView dvPatient)
    private void LoadPatientData_HeightWeightNotes(DataView dvPatient)
    {
        Boolean HasPatientVisit = dvPatient[0]["HasVisit"].ToString().Equals("True");
        Decimal decTemp = 0m;
        
        txtBMIHeight.Value = dvPatient[0]["BMIHeight"].ToString().Equals("") ? "0" : dvPatient[0]["BMIHeight"].ToString();
        txtBMI.Text = dvPatient[0]["InitBMI"].ToString().Equals("") ? "0" : (Decimal.TryParse(dvPatient[0]["InitBMI"].ToString(), out decTemp) ? decTemp.ToString(strNumberFormat) : "0");
        txtHHeight.Value = dvPatient[0]["Height"].ToString();
        txtHStartWeight.Value = dvPatient[0]["StartWeight"].ToString();
        txtHTargetWeight.Value = dvPatient[0]["TargetWeight"].ToString();
        txtHCurrentWeight.Value = dvPatient[0]["LastVisitWeight"].ToString(); // CurrentWeight
        txtExcessWeight.Text = dvPatient[0]["ExcessWeight"].ToString();
        txtHIdealWeight.Value = dvPatient[0]["IdealWeight"].ToString();
        txtTargetWeightBaseline.Text = dvPatient[0]["TargetWeight"].ToString();

        txtHLapbandDate.Value = gClass.TruncateDate(dvPatient[0]["LapbandDate"].ToString().Trim(), Request.Cookies["CultureInfo"].Value, 1);
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

        lblCurrentWeight.Visible = HasPatientVisit;
        txtCurrentWeight.Visible = HasPatientVisit;
        lblCurrentEWL1.Visible = HasPatientVisit;
        lblCurrentWeight_Value.Visible = HasPatientVisit;
        lblCurrentWeightTitle.Visible = HasPatientVisit;

        if (txtUseImperial.Value.Equals("1"))
        {
            txtHeight.Text = Decimal.TryParse(txtHHeight.Value, out decTemp) ? (decTemp / (Decimal)0.0254).ToString(strNumberFormat) : "0";
            txtStartWeight.Text = Decimal.TryParse(txtHStartWeight.Value, out decTemp) ? (decTemp / (Decimal)0.45359237).ToString(strNumberFormat) : "0";
            txtIdealWeight.Text = Decimal.TryParse(txtHIdealWeight.Value, out decTemp) ? (decTemp / (Decimal)0.45359237).ToString(strNumberFormat) : "0";
            txtCurrentWeight.Text = Decimal.TryParse(txtHCurrentWeight.Value, out decTemp) ? (decTemp / (Decimal)0.45359237).ToString(strNumberFormat) : "0";
            txtExcessWeight.Text = Decimal.TryParse(txtExcessWeight.Text, out decTemp) ? (decTemp / (Decimal)0.45359237).ToString(strNumberFormat) : "0";
            txtTargetWeightBaseline.Text = Decimal.TryParse(txtTargetWeightBaseline.Text, out decTemp) ? (decTemp / (Decimal)0.45359237).ToString(strNumberFormat) : "0";   
        }
        else
        {
            txtHeight.Text = Decimal.TryParse(txtHHeight.Value, out decTemp) ? (decTemp * 100).ToString(strNumberFormat) : "0";
            txtStartWeight.Text = Decimal.TryParse(txtHStartWeight.Value, out decTemp) ? decTemp.ToString(strNumberFormat) : "0";
            txtIdealWeight.Text = Decimal.TryParse(txtHIdealWeight.Value, out decTemp) ? decTemp.ToString(strNumberFormat) : "0";
            txtCurrentWeight.Text = Decimal.TryParse(txtHCurrentWeight.Value, out decTemp) ? decTemp.ToString(strNumberFormat) : "0";
            txtExcessWeight.Text = Decimal.TryParse(txtExcessWeight.Text, out decTemp) ? decTemp.ToString(strNumberFormat) : "0";
            txtTargetWeightBaseline.Text = Decimal.TryParse(txtTargetWeightBaseline.Text, out decTemp) ? decTemp.ToString(strNumberFormat) : "0";
        }
        lblCurrentWeight_Value.InnerText = txtCurrentWeight.Text;
        lblIdealWeight_Value.InnerText = txtIdealWeight.Text;
        txtNotes.Text = dvPatient[0]["Notes"].ToString();
        
    }
    #endregion

    #region private void LoadPatientData_BoldDataGroup()
    private void LoadPatientData_BoldDataGroup()
    {
        SqlCommand cmdSelect = new SqlCommand();
        DataSet dsPatient = new DataSet();

        //1) loading Patient BOLD Data (Bold Baseline data)
        cmdSelect.Parameters.Clear();
        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_PatientData_LoadBoldData", true);
        cmdSelect.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSelect.Parameters.Add("@PatientID", System.Data.SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
        dsPatient = gClass.FetchData(cmdSelect, "tblPatient_BoldData");
        if ((dsPatient.Tables.Count > 0) && (dsPatient.Tables[0].Rows.Count > 0))
            LoadPatientData_BOLDData(dsPatient.Tables[0].DefaultView);

        //2) loading Bold Comorbidity and Medications/Vitamins data
        cmdSelect.Parameters.Clear();
        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_ConsultFU1_ProgressNotes_LoadBoldComorbidityData", true);
        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSelect.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
        cmdSelect.Parameters.Add("@ConsultId", SqlDbType.Int).Value = 0;
        dsPatient = gClass.FetchData(cmdSelect, "tblPatient_BoldData");
        if ((dsPatient.Tables.Count > 0) && (dsPatient.Tables[0].Rows.Count > 0))
        {
            LoadPatientData_BOLDComorbidityData(dsPatient.Tables[0].DefaultView);
            LoadPatientData_BOLDComorbidityNotes(dsPatient.Tables[0].DefaultView);
        }
    }
    #endregion 

    #region private void LoadPatientData_BOLDData(DataView dvPatient)
    /// <summary>
    /// this function is to load patient bold data (used for baseline bold data)
    /// </summary>
    /// <history>
    ///     <change author = "Ali Farahani" date="01 Nov 07" version = "1.0"/>
    /// </history>
    /// <param name="dvPatient">Default Data view including BOLD data</param>
    private void LoadPatientData_BOLDData(DataView dvPatient)
    {
        String BoldChartNumber = "";

        //if (Convert.ToInt32(Request.Cookies["PatientID"].Value) > 0)
            //BoldChartNumber = gClass.OrganizationCode.ToString() + "-" + Request.Cookies["PatientID"].Value.ToString();

        if (dvPatient.Count == 0) return;
        txtChartNumber.Text = dvPatient[0]["ChartNumber"].ToString();
  
        txtSSN.Text = dvPatient[0]["SocialSecurityNumber"].ToString();
        cmbEmployment.SelectedValue = dvPatient[0]["EmploymentStatus"].ToString();
        txtEmployer.Text = dvPatient[0]["EmployerName"].ToString();
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

        switch (dvPatient[0]["IsCoverProcedure"].ToString())
        {
            case "1" :
                rbHealthInsurance_Yes.Checked = true;
                break;
            case "0" :
                rbHealthInsurance_No.Checked = true;
                break;
            case "-1" :
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
        txtPrevBariatricSurgery_Selected.Value = dvPatient[0]["PBS_Procedure"].ToString();
        txtPrevBarSurgery_Year.Text = dvPatient[0]["PBS_Year"].ToString();
        txtOrgWeight.Text = dvPatient[0]["OriginalWeight"].ToString();

        if (dvPatient[0]["OriginalWeight_Actual"].ToString().Equals("True")) rbOrgWeight_Actual.Checked = true;
        else rbOrgWeight_Estimated.Checked = true;

        txtLowestWeightAchieved.Text = dvPatient[0]["LowestWeightAchieved"].ToString();

        if (dvPatient[0]["LowestWeightAchieved_Actual"].ToString().Equals("True")) rbLowestWeightAchieved_Actual.Checked = true;
        else rbLowestWeightAchieved_Estimated.Checked = true;

        txtAdverseEvents_Selected.Value = dvPatient[0]["PBS_Event"].ToString();
        txtPrevBariatricSurgery_Selected.Value = dvPatient[0]["PBS_Procedure"].ToString();
        txtPrevNonBariatricSurgery_Selected.Value = dvPatient[0]["PNBS_Procedure"].ToString();

        FillSelectedLists(cmbPrevBariatricSurgery, listPrevBariatricSurgery_Selected,  txtPrevBariatricSurgery_Selected.Value);
        FillSelectedLists(cmbPrevNonBariatricSurgery, listPrevNonBariatricSurgery_Selected, txtPrevNonBariatricSurgery_Selected.Value);
        FillSelectedLists(cmbAdverseEvents, listAdverseEvents_Selected, txtAdverseEvents_Selected.Value);
    }
    #endregion

    #region private void LoadPatientData_BOLDComorbidityData(DataView dvPatient)
    private void LoadPatientData_BOLDComorbidityData(DataView dvPatient)
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
        SetMedicationVitaminValues(dvPatient[0]["VitaminList"].ToString());
        txtMedicationNotes.Text = dvPatient[0]["Vitamin_Description"].ToString();
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

    #region protected void linkBtnSave_OnClick(object sender, EventArgs e)
    protected void linkBtnSave_OnClick(object sender, EventArgs e)
    {
        String strScript = String.Empty;
        decimal visitWeight = 0;

        txtBoldChartNumber.Text = gClass.OrganizationCode.ToString() + "-" + Request.Cookies["PatientID"].Value.ToString();

        if (Request.Cookies["PermissionLevel"].Value != "1o")
        {
            IsDoneSaveFlag = -1;

            switch (txtHPageNo.Value)
            {
                case "1":
                    SavePatientDataProc_Demographics();
                    break;

                case "2":
                    SavePatientDataProc_HeightWeight();
                    break;

                case "5":
                    SavePatientDataProc_BoldData();
                    break;

                case "6": // Because all data of these 2 sub-pages are saved in one table, we save all data in one process

                case "7":
                    SavePatientDataProc_BoldComorbidity();
                    break;
                case "8":
                    SavePatientDataProc_BoldComorbidityNotes();
                    break;
                case "9":
                    SavePatientDataProc_BoldPreviousSurgery();
                    break;
                default:
                    IsDoneSaveFlag = -1;
                    break;
            }
            switch (IsDoneSaveFlag)
            {
                case 1: // Saving data procedure is done sucessfully
                    LoadPatientData();

                    //calculate age
                    DateTime currentDate = Convert.ToDateTime(txtHCurrentDate.Value);
                    DateTime birthDate = Convert.ToDateTime(txtBirthDate.Text);

                    int intAge = currentDate.Year - birthDate.Year;
                    if (currentDate.Month < birthDate.Month)
                        --intAge;
                    else if (currentDate.Month == birthDate.Month)
                        if (currentDate.Day < birthDate.Day)
                            --intAge;


                    if (cmbVisitWeeks.SelectedValue.ToString() != "")
                    {
                        strScript += "SetInnerText($get('tblPatientTitle_lblCalculateVisitIntro'), 'Calculate visit weeks and weight loss from:');";
                        strScript += "SetInnerText($get('tblPatientTitle_lblCalculateVisit'),'" + cmbVisitWeeks.SelectedText + "');";

                        strScript += "SetInnerText($get('tblPatientTitle_lblCalculateVisitDate'),'');";

                        if (cmbVisitWeeks.SelectedValue.ToString() == "3")
                        {
                            strScript += "SetInnerText($get('tblPatientTitle_lblCalculateVisitDate'),'" + txtZeroDate.Text + "');";
                        }
                        else if (cmbVisitWeeks.SelectedValue.ToString() == "4")
                        {
                            strScript += "SetInnerText($get('tblPatientTitle_lblCalculateVisitDate'),'" + txtHLapbandDate.Value + "');";
                        }
                    }
                    Response.Cookies.Set(new HttpCookie("VisitWeeksFlag", cmbVisitWeeks.SelectedValue));  

                    if (intAge > 0)
                    {
                        strScript += "SetInnerText($get('tblPatientTitle_lblAge_Value'), " + intAge.ToString() + " + ' yrs');";
                        strScript += "SetInnerText($get('lblAge'), " + intAge.ToString() + ");";
                    }
                    else{
                        strScript += "SetInnerText($get('tblPatientTitle_lblAge_Value''), '');";
                        strScript += "SetInnerText($get('lblAge'), '');";
                    }


                    strScript += "if (AppSchema_ButtonNo != 0) ";
                    strScript += "  document.location.assign(document.getElementById('txtHApplicationURL').value + AppSchemaMenu_href[AppSchema_ButtonNo-1]);";
                    strScript += "else{";
                    strScript += txtHPageNo.Value.Equals("1") ? "ShowDivMessage('The information has been saved...', true);" : "";
                    strScript += "  ShowDivMessage('The information has been saved...', true); UpdatePatientTitle();";
                    strScript += "  document.getElementById('tblPatientTitle_div_PatientTitle').style.display = 'block';";
                    strScript += "  LoadApplicationSchemas();";
                    strScript += "};";
                    strScript += "if (" + txtSelectedPageNo.Value + "!= 0) SetDivsStatus(" + txtSelectedPageNo.Value + ");";
                    strScript += "if (" + txtSelectedPageNo.Value + "== 8) UpdateComorbidityNotes();";
                    strScript += "SetEvents();";

                    LapbaseSession.PatientId = Convert.ToInt32(txtPatientID.Value);
                    break;

                case 0: // Saving data procedure is not done sucessfully
                    strScript = "document.getElementById('divErrorMessage').style.display = 'block';";
                    strScript += "SetInnerText(document.getElementById('pErrorMessage'), 'Error in save data ..." + BoldErrorMsg + "');";
                    strScript += "SetEvents();";
                    break;

                case -1:   // data is not ready to save because some fields are empty such as Surname, Firstname is DEMOGRAPG sub-page
                    strScript = "if (AppSchema_ButtonNo != 0) ";
                    strScript += "  document.location.assign(document.getElementById('txtHApplicationURL').value + AppSchemaMenu_href[AppSchema_ButtonNo-1]);";
                    strScript += "else if (" + txtSelectedPageNo.Value + "!= 0) SetDivsStatus(" + txtSelectedPageNo.Value + ");";
                    strScript += "SetEvents();";
                    break;
            }
            ScriptManager.RegisterStartupScript(linkBtnSave, linkBtnSave.GetType(), Guid.NewGuid().ToString(), strScript, true);
        }
        else
        {
            strScript = "if (AppSchema_ButtonNo != 0) ";
            strScript += "  document.location.assign(document.getElementById('txtHApplicationURL').value + AppSchemaMenu_href[AppSchema_ButtonNo-1]);";
                    
            strScript += "if (" + txtSelectedPageNo.Value + "!= 0) SetDivsStatus(" + txtSelectedPageNo.Value + ");"; ;
            strScript += "SetEvents();";
            ScriptManager.RegisterStartupScript(linkBtnSave, linkBtnSave.GetType(), Guid.NewGuid().ToString(), strScript, true);
        }
                
    }
    #endregion 

    #region private void SavePatientDataProc_Demographics()
    private void SavePatientDataProc_Demographics()
    {
        SqlCommand cmdSave = new SqlCommand();
        String strNameId = String.Empty;

        strNameId = (txtSurName.Text.Length == 0) ? String.Empty : ((txtSurName.Text.Length > 3) ? txtSurName.Text.Substring(0, 4) : txtSurName.Text);
        strNameId += (txtFirstName.Text.Length == 0) ? String.Empty : txtFirstName.Text.Substring(0, 1);

        if (strNameId.Equals(String.Empty))
        {
            IsDoneSaveFlag = -1;
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
        cmdSave.Parameters.Add("@Surname", SqlDbType.VarChar, 40).Value = txtSurName.Text.Replace("'", "`") ;
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

        cmdSave.Parameters.Add("@Sex", SqlDbType.VarChar, 1).Value  = rblGender.SelectedValue;
        cmdSave.Parameters.Add("@DoctorId", SqlDbType.Int).Value = (cmbSurgon.SelectedIndex > 0) ? Convert.ToInt32(cmbSurgon.SelectedValue) : 0;
        cmdSave.Parameters.Add("@RefDrId1", SqlDbType.VarChar, 10).Value = txtHReferredBy.Value;
        cmdSave.Parameters.Add("@RefDrId2", SqlDbType.VarChar, 10).Value = txtHOtherDoctors1.Value;
        cmdSave.Parameters.Add("@RefDrId3", SqlDbType.VarChar, 10).Value = txtHOtherDoctors2.Value;
        cmdSave.Parameters.Add("@Patient_MDId", SqlDbType.VarChar, 10).Value = "";

        cmdSave.Parameters.Add("@MobilePhone", SqlDbType.VarChar, 30).Value = txtMobile.Text;
        cmdSave.Parameters.Add("@EmailAddress", SqlDbType.VarChar, 100).Value = txtEmail.Text;
        cmdSave.Parameters.Add("@Insurance", SqlDbType.VarChar, 50).Value = txtInsurance.Text;
        cmdSave.Parameters.Add("@Patient_CustomID", SqlDbType.VarChar, 20).Value = txtPatient_CustomID.Text;
        cmdSave.Parameters.Add("@SocialHistory", SqlDbType.VarChar, 2048).Value = txtSocialHistory.Text.Replace("'", "`");  

        gClass.AddLogParameters(ref cmdSave, Request.Cookies["Logon_UserName"].Value, Request.Url.Host, (txtPatientID.Value.Equals("0")) ? "insert" : "update");

        try
        {
            if (txtPatientID.Value.Equals("0")) // means new Patient Data, data must be inserted
            {
                gClass.SavePatientData(1, cmdSave);
                txtPatientID.Value = gClass.PatientID.ToString();
                txtPatient_CustomID.Text = (txtPatient_CustomID.Text.Equals(String.Empty) || txtPatient_CustomID.Text.Equals("0")) ? txtPatientID.Value : txtPatient_CustomID.Text;
                Context.Response.SetCookie(new HttpCookie("PatientID", txtPatientID.Value));
                gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value, Request.Cookies["Logon_UserName"].Value, Request.Url.Host,
                                        "Baseline Form", 2, "Add Data", "PatientCode", Response.Cookies["PatientID"].Value);


                //save to emr
                SaveEMRFromPatientDetails();

                Response.Redirect("~/Forms/Patients/PatientData/PatientDataForm.aspx?PID=" + txtPatientID.Value,false);
            }
            else //data must be Updated
            {
                cmdSave.Parameters.Add("@PatientId", SqlDbType.Int).Value = Convert.ToInt64(txtPatientID.Value);
                cmdSave.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Context.Request.Cookies["UserPracticeCode"].Value);
                cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);

                gClass.SavePatientData(2, cmdSave);
                Context.Response.SetCookie(new HttpCookie("PatientID", txtPatientID.Value));
                gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value, Request.Cookies["Logon_UserName"].Value,
                                        Request.Url.Host, "Baseline Form", 2, "Modify Data", "PatientCode", txtPatientID.Value);
            }
            IsDoneSaveFlag = 1;
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Baseline", "Data saving baseline", err.ToString());
            IsDoneSaveFlag = 0;
        }
        cmdSave.Dispose();
    }
	#endregion

    #region private void SavePatientDataProc_HeightWeight()
    private void SavePatientDataProc_HeightWeight()
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
        cmdSave.Parameters["@StartBMIWeight"].Value = Decimal.TryParse(txtStartBMIWeight.Value, out decTemp) ? decTemp : 0;
        cmdSave.Parameters["@BMIHeight"].Value = Decimal.TryParse(txtBMIHeight.Value, out decTemp) ? decTemp : 0;
        cmdSave.Parameters["@BMI"].Value = Decimal.TryParse(txtHBMI.Value, out decTemp) ? decTemp : 0;
        txtBMI.Text = txtHBMI.Value; // IMPORTANT, DO NOT DELETE OR COMMENT OUT THIS STATEMENT
        cmdSave.Parameters["@Notes"].Value = txtNotes.Text.Replace("'", "`");

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
            gClass.SavePatientWeightData(cmdSave);
            Response.SetCookie(new HttpCookie("PatientID", txtPatientID.Value));
            gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value, Request.Cookies["Logon_UserName"].Value, Request.Url.Host,
                        "Baseline Form", 2, "Modify Height/Weight/Notes data", "PatientCode", txtPatientID.Value);
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

    #region SavePatientDataProc_BoldData
    private void SavePatientDataProc_BoldData()
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
        cmdSave.Parameters.Add("@HasConsentedToSRC", SqlDbType.Bit).Value = rdSend2SRC_Yes.Checked;
	    cmdSave.Parameters.Add("@HasInsurance", SqlDbType.Bit).Value = (txtInsurance.Text.Trim().Length >  0);
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
	    cmdSave.Parameters.Add("@DietryWeightLoss", SqlDbType.VarChar, 10).Value = cmbDietaryWeightLoss.SelectedValue;
	    cmdSave.Parameters.Add("@DurationObesity", SqlDbType.Bit).Value = chkDurationObesity.Checked;
	    cmdSave.Parameters.Add("@SmokingCessation", SqlDbType.Bit).Value = chkSmokingCessation.Checked;
	    cmdSave.Parameters.Add("@MentalHealthClearance", SqlDbType.Bit).Value = chkMentalHealthClearance.Checked;
	    cmdSave.Parameters.Add("@IQTesting", SqlDbType.Bit).Value = chkIntelligenceTesting.Checked;
        cmdSave.Parameters.Add("@PreCertification_Other", SqlDbType.VarChar, 100).Value = txtPreCert_Other.Text.Trim();
        cmdSave.Parameters.Add("@PNBS_Procedure", SqlDbType.VarChar, 100).Value = txtPrevNonBariatricSurgery_Selected.Value;

        try
        {
            gClass.ExecuteDMLCommand(cmdSave);
            gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value, Request.Cookies["Logon_UserName"].Value, Request.Url.Host, "Baseline Form - BOLD data", 2, "Save (Add new/update) bold data", 
                    "PatientCode", txtPatientID.Value);
            IsDoneSaveFlag = 1;
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Baseline Form - BOLD data", "Save (Add new/update) bold data", err.ToString());
            IsDoneSaveFlag = 0;
        }
    }
    #endregion
    
    #region SavePatientDataProc_BoldPreviousSurgery
    private void SavePatientDataProc_BoldPreviousSurgery()
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

        gClass.MakeStoreProcedureName(ref cmdSave, "sp_PatientData_SaveBoldPreviousSurgery", true);
        cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Int32.TryParse(gClass.OrganizationCode, out int32Temp) ? int32Temp : 0;
        cmdSave.Parameters.Add("@PatientId", SqlDbType.Int).Value = Int32.TryParse(txtPatientID.Value, out int32Temp) ? int32Temp : 0;
        cmdSave.Parameters.Add("@PBS_Procedure", SqlDbType.VarChar, 100).Value = txtPrevBariatricSurgery_Selected.Value;
	    cmdSave.Parameters.Add("@PBS_Year", SqlDbType.Int).Value = Int32.TryParse(txtPrevBarSurgery_Year.Text, out int32Temp) ? int32Temp : 0;
        cmdSave.Parameters.Add("@OriginalWeight", SqlDbType.Decimal).Value = Decimal.TryParse(txtOrgWeight.Text, out decTemp) ? decTemp : 0;
	    cmdSave.Parameters.Add("@OriginalWeight_Actual", SqlDbType.Bit).Value = rbOrgWeight_Actual.Checked ? true : (rbOrgWeight_Estimated.Checked ? true : false);
	    cmdSave.Parameters.Add("@LowestWeightAchieved", SqlDbType.Decimal).Value = Decimal.TryParse(txtLowestWeightAchieved.Text, out decTemp) ? decTemp : 0;
	    cmdSave.Parameters.Add("@LowestWeightAchieved_Actual", SqlDbType.Bit).Value = rbLowestWeightAchieved_Actual.Checked ? true : (rbLowestWeightAchieved_Estimated.Checked ? true : false);
        cmdSave.Parameters.Add("@PBS_Event", SqlDbType.VarChar, 100).Value = txtAdverseEvents_Selected.Value;
    
        try
        {
            gClass.ExecuteDMLCommand(cmdSave);
            gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value, Request.Cookies["Logon_UserName"].Value, Request.Url.Host, "Baseline Form - BOLD data", 2, "Save (Add new/update) bold data", 
                    "PatientCode", txtPatientID.Value);
            IsDoneSaveFlag = 1;
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Baseline Form - BOLD data", "Save (Add new/update) bold data", err.ToString());
            IsDoneSaveFlag = 0;
        }
    }
    #endregion

    #region private void SavePatientDataProc_BoldComorbidity()
    private void SavePatientDataProc_BoldComorbidity()
    {
        if (txtPatientID.Value.Equals("0"))
        {
            IsDoneSaveFlag = -1;
            return;
        }

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
        cmdSave.Parameters.Add("@VitaminList", SqlDbType.NVarChar, 50).Value = GetMedicationVitaminValues();
        cmdSave.Parameters.Add("@Vitamin_Description", SqlDbType.NVarChar, 255).Value = txtMedicationNotes.Text;

        try
        {
            gClass.ExecuteDMLCommand(cmdSave);
            gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value, Request.Cookies["Logon_UserName"].Value, Request.Url.Host, "Baseline Form - BOLD Comorbidity ", 2, 
                    "Save (Add new/update) bold comorbidity data", "PatientCode", txtPatientID.Value);
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

    #region protected void linkBtnCalculateAge_OnClick(object sender, EventArgs e)
    protected void linkBtnCalculateAge_OnClick(object sender, EventArgs e)
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo(Request.Cookies["CultureInfo"].Value, true);
        DateTime BirthDate = new DateTime();
        string strBirthDate = txtBirthDate.Text.Trim(), strScript = String.Empty;

        if (strBirthDate == string.Empty)
            lblAge.InnerText = "0";
        else
            try
            {
                BirthDate = Convert.ToDateTime(strBirthDate);
                if (BirthDate > DateTime.Now)
                {
                    lblAge.InnerText = "0";
                    txtBirthDate.Text = "";
                    txtBirthDate.Focus();
                    strScript += "document.getElementById('divErrorMessage').style.display = 'block';";
                    strScript += "SetInnerText(document.getElementById('pErrorMessage'), 'The Birthdate is greater than current date ...');";
                }
                else
                {
                    lblAge.InnerText = CalculateAge(BirthDate);
                    strScript += "SetInnerText(document.getElementById('tblPatientTitle_lblAge_Value'), '" + lblAge.InnerText + " yrs');";
                }
            }
            catch
            {
                txtBirthDate.Text = "";
                txtBirthDate.Focus();
                lblAge.InnerText = "0";
                strScript += "document.getElementById('divErrorMessage').style.display = 'block';";
                strScript += "SetInnerText(document.getElementById('pErrorMessage'), 'The Birthdate is not correct ...');";
            }

        strScript += "document.getElementById('txtBirthDate_txtGlobal').onchange = function(){txtBirthDate_onchange();}";
        ScriptManager.RegisterStartupScript(linkBtnCalculateAge, linkBtnCalculateAge.GetType(), Guid.NewGuid().ToString(), strScript, true);
    }
    #endregion 

    #region private string CalculateAge(DateTime BirthDate)
    private string CalculateAge(DateTime BirthDate)
    {
        int intAge = DateTime.Now.Year - BirthDate.Year;
        if (DateTime.Now.Month < BirthDate.Month)
            --intAge;
        else if (DateTime.Now.Month == BirthDate.Month)
            if (DateTime.Now.Day < BirthDate.Day)
                --intAge;

        return (intAge.ToString());
    }
    #endregion 

    #region private void LoadPatientData_BOLDComorbidityNotes(DataView dvPatient)
    private void LoadPatientData_BOLDComorbidityNotes(DataView dvPatient)
    {
        hCardiovascularDiseaseNotes.Style["display"] = "none";
        hMetabolicNotes.Style["display"] = "none";
        hPulmonaryNotes.Style["display"] = "none";
        hMusculoskeletalNotes.Style["display"] = "none";
        hGastroIntestinalNotes.Style["display"] = "none";
        hReproductiveNotes.Style["display"] = "none";
        hMusculoskeletalNotes.Style["display"] = "none";
        hGeneralNotes.Style["display"] = "none";
        hPsychosocialNotes.Style["display"] = "none";
        hReproductiveNotes.Style["display"] = "none";

        lnHypertensionNotes.Style["display"] = checkIfEmpty(dvPatient[0]["CVS_Congestive"].ToString());
        lnCongestiveNotes.Style["display"] = checkIfEmpty(dvPatient[0]["CVS_Congestive"].ToString());
        lnIschemicNotes.Style["display"] = checkIfEmpty(dvPatient[0]["CVS_Ischemic"].ToString());
        lnAnginaNotes.Style["display"] = checkIfEmpty(dvPatient[0]["CVS_Angina"].ToString());
        lnPeripheralNotes.Style["display"] = checkIfEmpty(dvPatient[0]["CVS_Peripheral"].ToString());
        lnLowerNotes.Style["display"] = checkIfEmpty(dvPatient[0]["CVS_Lower"].ToString());
        lnDVTNotes.Style["display"] = checkIfEmpty(dvPatient[0]["CVS_DVT"].ToString());

        if (lnHypertensionNotes.Style["display"] == "block" || lnCongestiveNotes.Style["display"] == "block" || lnIschemicNotes.Style["display"] == "block" || lnAnginaNotes.Style["display"] == "block" || lnPeripheralNotes.Style["display"] == "block" || lnLowerNotes.Style["display"] == "block" || lnDVTNotes.Style["display"] == "block")
            hCardiovascularDiseaseNotes.Style["display"] = "block";

        cmbHypertensionNotes.Text = dvPatient[0]["CVS_Hypertension_Notes"].ToString();
        cmbCongestiveNotes.Text = dvPatient[0]["CVS_Congestive_Notes"].ToString();
        cmbIschemicNotes.Text = dvPatient[0]["CVS_Ischemic_Notes"].ToString();
        cmbAnginaNotes.Text = dvPatient[0]["CVS_Angina_Notes"].ToString();
        cmbPeripheralNotes.Text = dvPatient[0]["CVS_Peripheral_Notes"].ToString();
        cmbLowerNotes.Text = dvPatient[0]["CVS_Lower_Notes"].ToString();
        cmbDVTNotes.Text = dvPatient[0]["CVS_DVT_Notes"].ToString();



        lnGlucoseNotes.Style["display"] = checkIfEmpty(dvPatient[0]["MET_Glucose"].ToString());
        lnLipidsNotes.Style["display"] = checkIfEmpty(dvPatient[0]["MET_Lipids"].ToString());
        lnGoutNotes.Style["display"] = checkIfEmpty(dvPatient[0]["MET_Gout"].ToString());

        if (lnGlucoseNotes.Style["display"] == "block" || lnLipidsNotes.Style["display"] == "block" || lnGoutNotes.Style["display"] == "block")
            hMetabolicNotes.Style["display"] = "block";

        cmbGlucoseNotes.Text = dvPatient[0]["MET_Glucose_Notes"].ToString();
        cmbLipidsNotes.Text = dvPatient[0]["MET_Lipids_Notes"].ToString();
        cmbGoutNotes.Text = dvPatient[0]["MET_Gout_Notes"].ToString();

        lnObstructiveNotes.Style["display"] = checkIfEmpty(dvPatient[0]["PUL_Obstructive"].ToString());
        lnObesityNotes.Style["display"] = checkIfEmpty(dvPatient[0]["PUL_Obesity"].ToString());
        lnPulmonaryNotes.Style["display"] = checkIfEmpty(dvPatient[0]["PUL_PulHypertension"].ToString());
        lnAsthmaNotes.Style["display"] = checkIfEmpty(dvPatient[0]["PUL_Asthma"].ToString());

        if (lnObstructiveNotes.Style["display"] == "block" || lnObesityNotes.Style["display"] == "block" || lnPulmonaryNotes.Style["display"] == "block" || lnAsthmaNotes.Style["display"] == "block")
            hPulmonaryNotes.Style["display"] = "block";

        cmbObstructiveNotes.Text = dvPatient[0]["PUL_Obstructive_Notes"].ToString();
        cmbObesityNotes.Text = dvPatient[0]["PUL_Obesity_Notes"].ToString();
        cmbPulmonaryNotes.Text = dvPatient[0]["PUL_PulHypertension_Notes"].ToString();
        cmbAsthmaNotes.Text = dvPatient[0]["PUL_Asthma_Notes"].ToString();

        lnGredNotes.Style["display"] = checkIfEmpty(dvPatient[0]["GAS_Gerd"].ToString());
        lnCholelithiasisNotes.Style["display"] = checkIfEmpty(dvPatient[0]["GAS_Cholelithiasis"].ToString());
        lnLiverNotes.Style["display"] = checkIfEmpty(dvPatient[0]["GAS_Liver"].ToString());

        if (lnGredNotes.Style["display"] == "block" || lnCholelithiasisNotes.Style["display"] == "block" || lnLiverNotes.Style["display"] == "block")
            hGastroIntestinalNotes.Style["display"] = "block";

        cmbGredNotes.Text = dvPatient[0]["GAS_Gerd_Notes"].ToString();
        cmbCholelithiasisNotes.Text = dvPatient[0]["GAS_Cholelithiasis_Notes"].ToString();
        cmbLiverNotes.Text = dvPatient[0]["GAS_Liver_Notes"].ToString();



        lnBackPainNotes.Style["display"] = checkIfEmpty(dvPatient[0]["MUS_BackPain"].ToString());
        lnMusculoskeletalNotes.Style["display"] = checkIfEmpty(dvPatient[0]["MUS_Musculoskeletal"].ToString());
        lnFibroNotes.Style["display"] = checkIfEmpty(dvPatient[0]["MUS_Fibromyalgia"].ToString());

        if (lnBackPainNotes.Style["display"] == "block" || lnMusculoskeletalNotes.Style["display"] == "block" || lnFibroNotes.Style["display"] == "block")
            hMusculoskeletalNotes.Style["display"] = "block";

        cmbBackPainNotes.Text = dvPatient[0]["MUS_BackPain_Notes"].ToString();
        cmbMusculoskeletalNotes.Text = dvPatient[0]["MUS_Musculoskeletal_Notes"].ToString();
        cmbFibroNotes.Text = dvPatient[0]["MUS_Fibromyalgia_Notes"].ToString();



        lnPolycysticNotes.Style["display"] = checkIfEmpty(dvPatient[0]["REPRD_Polycystic"].ToString());
        lnMenstrualNotes.Style["display"] = checkIfEmpty(dvPatient[0]["REPRD_Menstrual"].ToString());

        if (lnPolycysticNotes.Style["display"] == "block" || lnMenstrualNotes.Style["display"] == "block")
            hReproductiveNotes.Style["display"] = "block";

        cmbPolycysticNotes.Text = dvPatient[0]["REPRD_Polycystic_Notes"].ToString();
        cmbMenstrualNotes.Text = dvPatient[0]["REPRD_Menstrual_Notes"].ToString();



        lnPsychosocialNotes.Style["display"] = checkIfEmpty(dvPatient[0]["PSY_Impairment"].ToString());
        lnDepressionNotes.Style["display"] = checkIfEmpty(dvPatient[0]["PSY_Depression"].ToString());
        lnConfirmedNotes.Style["display"] = checkIfEmpty(dvPatient[0]["PSY_MentalHealth"].ToString());
        lnAlcoholNotes.Style["display"] = checkIfEmpty(dvPatient[0]["PSY_Alcohol"].ToString());
        lnTobaccoNotes.Style["display"] = checkIfEmpty(dvPatient[0]["PSY_Tobacoo"].ToString());
        lnAbuseNotes.Style["display"] = checkIfEmpty(dvPatient[0]["PSY_Abuse"].ToString());

        if (lnPsychosocialNotes.Style["display"] == "block" || lnDepressionNotes.Style["display"] == "block" || lnConfirmedNotes.Style["display"] == "block" || lnAlcoholNotes.Style["display"] == "block" || lnTobaccoNotes.Style["display"] == "block" || lnAbuseNotes.Style["display"] == "block")
            hPsychosocialNotes.Style["display"] = "block";

        cmbPsychosocialNotes.Text = dvPatient[0]["PSY_Impairment_Notes"].ToString();
        cmbDepressionNotes.Text = dvPatient[0]["PSY_Depression_Notes"].ToString();
        cmbConfirmedNotes.Text = dvPatient[0]["PSY_MentalHealth_Notes"].ToString();
        cmbAlcoholNotes.Text = dvPatient[0]["PSY_Alcohol_Notes"].ToString();
        cmbTobaccoNotes.Text = dvPatient[0]["PSY_Tobacoo_Notes"].ToString();
        cmbAbuseNotes.Text = dvPatient[0]["PSY_Abuse_Notes"].ToString();



        lnStressUrinaryNotes.Style["display"] = checkIfEmpty(dvPatient[0]["GEN_Stress"].ToString());
        lnCerebriNotes.Style["display"] = checkIfEmpty(dvPatient[0]["GEN_Cerebri"].ToString());
        lnHerniaNotes.Style["display"] = checkIfEmpty(dvPatient[0]["GEN_Hernia"].ToString());
        lnFunctionalNotes.Style["display"] = checkIfEmpty(dvPatient[0]["GEN_Functional"].ToString());
        lnSkinNotes.Style["display"] = checkIfEmpty(dvPatient[0]["GEN_Skin"].ToString());

        if (lnStressUrinaryNotes.Style["display"] == "block" || lnCerebriNotes.Style["display"] == "block" || lnHerniaNotes.Style["display"] == "block" || lnFunctionalNotes.Style["display"] == "block" || lnSkinNotes.Style["display"] == "block")
            hGeneralNotes.Style["display"] = "block";

        cmbStressUrinaryNotes.Text = dvPatient[0]["GEN_Stress_Notes"].ToString();
        cmbCerebriNotes.Text = dvPatient[0]["GEN_Cerebri_Notes"].ToString();
        cmbHerniaNotes.Text = dvPatient[0]["GEN_Hernia_Notes"].ToString();
        cmbFunctionalNotes.Text = dvPatient[0]["GEN_Functional_Notes"].ToString();
        cmbSkinNotes.Text = dvPatient[0]["GEN_Skin_Notes"].ToString();

        /*
        SetMedicationVitaminValues(dvPatient[0]["VitaminList"].ToString());
        txtMedicationNotes.Text = dvPatient[0]["Vitamin_Description"].ToString();*/
    }
    #endregion 
   
    #region private String checkIfEmpty(String passedVal)
    private String checkIfEmpty(String passedVal)
    {
        SqlCommand cmdSelect = new SqlCommand();
        DataSet dsCode = new DataSet();

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_SystemCodes_LoadData_Bold_ByBoldCode", true);
        cmdSelect.Parameters.Add("@BoldCode", System.Data.SqlDbType.VarChar, 10).Value = passedVal;

        dsCode = gClass.FetchData(cmdSelect, "tblSystemCode_BoldData");

        if (dsCode == null || dsCode.Tables[0].Rows.Count == 0)
            return "none";
        else
        {
            if (dsCode.Tables[0].DefaultView[0]["Rank"].ToString().Equals("0") || dsCode.Tables[0].DefaultView[0]["Rank"].Equals(null))
                return "none";
            else
                return "block";
        }
    }
    #endregion

    #region private void SavePatientDataProc_BoldComorbidityNotes()
    private void SavePatientDataProc_BoldComorbidityNotes()
    {
        if (txtPatientID.Value.Equals("0"))
        {
            IsDoneSaveFlag = -1;
            return;
        }

        SqlCommand cmdSave = new SqlCommand();
        gClass.MakeStoreProcedureName(ref cmdSave, "sp_ConsultFu1_BoldComorbidityNotes_SaveData", true);
        cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSave.Parameters.Add("@PatientId", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
        cmdSave.Parameters.Add("@ConsultId", SqlDbType.Int).Value = 0;

        cmdSave.Parameters.Add("@CVS_Hypertension", SqlDbType.NVarChar, 1024).Value = cmbHypertensionNotes.Text.Trim();
        cmdSave.Parameters.Add("@CVS_Congestive", SqlDbType.NVarChar, 1024).Value = cmbCongestiveNotes.Text.Trim();
        cmdSave.Parameters.Add("@CVS_Ischemic", SqlDbType.NVarChar, 1024).Value = cmbIschemicNotes.Text.Trim();
        cmdSave.Parameters.Add("@CVS_Angina", SqlDbType.NVarChar, 1024).Value = cmbAnginaNotes.Text.Trim();
        cmdSave.Parameters.Add("@CVS_Peripheral", SqlDbType.NVarChar, 1024).Value = cmbPeripheralNotes.Text.Trim();
        cmdSave.Parameters.Add("@CVS_Lower", SqlDbType.NVarChar, 1024).Value = cmbLowerNotes.Text.Trim();
        cmdSave.Parameters.Add("@CVS_DVT", SqlDbType.NVarChar, 1024).Value = cmbDVTNotes.Text.Trim();
        cmdSave.Parameters.Add("@MET_Glucose", SqlDbType.NVarChar, 1024).Value = cmbGlucoseNotes.Text.Trim();
        cmdSave.Parameters.Add("@MET_Lipids", SqlDbType.NVarChar, 1024).Value = cmbLipidsNotes.Text.Trim();
        cmdSave.Parameters.Add("@MET_Gout", SqlDbType.NVarChar, 1024).Value = cmbGoutNotes.Text.Trim();
        cmdSave.Parameters.Add("@PUL_Obstructive", SqlDbType.NVarChar, 1024).Value = cmbObstructiveNotes.Text.Trim();
        cmdSave.Parameters.Add("@PUL_Obesity", SqlDbType.NVarChar, 1024).Value = cmbObesityNotes.Text.Trim();
        cmdSave.Parameters.Add("@PUL_PulHypertension", SqlDbType.NVarChar, 1024).Value = cmbPulmonaryNotes.Text.Trim();
        cmdSave.Parameters.Add("@PUL_Asthma", SqlDbType.NVarChar, 1024).Value = cmbAsthmaNotes.Text.Trim();
        cmdSave.Parameters.Add("@GAS_Gerd", SqlDbType.NVarChar, 1024).Value = cmbGredNotes.Text.Trim();
        cmdSave.Parameters.Add("@GAS_Cholelithiasis", SqlDbType.NVarChar, 1024).Value = cmbCholelithiasisNotes.Text.Trim();
        cmdSave.Parameters.Add("@GAS_Liver", SqlDbType.NVarChar, 1024).Value = cmbLiverNotes.Text.Trim();
        cmdSave.Parameters.Add("@MUS_BackPain", SqlDbType.NVarChar, 1024).Value = cmbBackPainNotes.Text.Trim();
        cmdSave.Parameters.Add("@MUS_Musculoskeletal", SqlDbType.NVarChar, 1024).Value = cmbMusculoskeletalNotes.Text.Trim();
        cmdSave.Parameters.Add("@MUS_Fibromyalgia", SqlDbType.NVarChar, 1024).Value = cmbFibroNotes.Text.Trim();
        cmdSave.Parameters.Add("@REPRD_Polycystic", SqlDbType.NVarChar, 1024).Value = cmbPolycysticNotes.Text.Trim();
        cmdSave.Parameters.Add("@REPRD_Menstrual", SqlDbType.NVarChar, 1024).Value = cmbMenstrualNotes.Text.Trim();
        cmdSave.Parameters.Add("@PSY_Impairment", SqlDbType.NVarChar, 1024).Value = cmbPsychosocialNotes.Text.Trim();
        cmdSave.Parameters.Add("@PSY_Depression", SqlDbType.NVarChar, 1024).Value = cmbDepressionNotes.Text.Trim();
        cmdSave.Parameters.Add("@PSY_MentalHealth", SqlDbType.NVarChar, 1024).Value = cmbConfirmedNotes.Text.Trim();
        cmdSave.Parameters.Add("@PSY_Alcohol", SqlDbType.NVarChar, 1024).Value = cmbAlcoholNotes.Text.Trim();
        cmdSave.Parameters.Add("@PSY_Tobacoo", SqlDbType.NVarChar, 1024).Value = cmbTobaccoNotes.Text.Trim();
        cmdSave.Parameters.Add("@PSY_Abuse", SqlDbType.NVarChar, 1024).Value = cmbAbuseNotes.Text.Trim();
        cmdSave.Parameters.Add("@GEN_Stress", SqlDbType.NVarChar, 1024).Value = cmbStressUrinaryNotes.Text.Trim();
        cmdSave.Parameters.Add("@GEN_Cerebri", SqlDbType.NVarChar, 1024).Value = cmbCerebriNotes.Text.Trim();
        cmdSave.Parameters.Add("@GEN_Hernia", SqlDbType.NVarChar, 1024).Value = cmbHerniaNotes.Text.Trim();
        cmdSave.Parameters.Add("@GEN_Functional", SqlDbType.NVarChar, 10).Value = cmbFunctionalNotes.Text.Trim();
        cmdSave.Parameters.Add("@GEN_Skin", SqlDbType.NVarChar, 1024).Value = cmbSkinNotes.Text.Trim();

        try
        {
            gClass.ExecuteDMLCommand(cmdSave);
            gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value, Request.Cookies["Logon_UserName"].Value, Request.Url.Host, "Baseline Form - BOLD Comorbidity Notes", 2,
                    "Save (Add new/update) bold notes comorbidity data", "PatientCode", txtPatientID.Value);
            IsDoneSaveFlag = 1;
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Baseline Form - BOLD Comorbidity Notes",
                    "Save (Add new/update) bold notes data", err.ToString());
            IsDoneSaveFlag = 0;
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

            //go back to first page
            strScript = "javascript:document.location.assign(document.getElementById('txtHApplicationURL').value + '/Forms/PatientsVisits/PatientsVisitsForm.aspx');";
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
    
    #region private void SaveEMRFromPatientDetails()
    private void SaveEMRFromPatientDetails()
    {
        SqlCommand cmdSave = new SqlCommand();
        gClass.MakeStoreProcedureName(ref cmdSave, "sp_PatientEMR_cmdInsertFromPatientDetails", true);

        cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSave.Parameters.Add("@PatientId", SqlDbType.Int).Value = Convert.ToInt32(txtPatientID.Value);
        
        try
        {
            gClass.ExecuteDMLCommand(cmdSave);
            gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value, Request.Cookies["Logon_UserName"].Value, Request.Url.Host,
                        "Baseline Form", 2, "Insert EMR from Patient Details", "PatientCode", txtPatientID.Value);
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host,
                                    Request.Cookies["Logon_UserName"].Value, "Baseline", "Insert EMR from Patient Details", err.ToString());
        }
        cmdSave.Dispose();
    }
    #endregion 
    
    #region private Boolean ValidateBOLDPatientData()
    private Boolean ValidateBOLDPatientData()
    {
        BoldErrorMsg += cmbRace.SelectedValue.ToString() != "" ? "" : "race, ";
        BoldErrorMsg += cmbEmployment.SelectedValue.ToString() != "" ? "" : "employment status, ";

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
                        Context.Request.Cookies["Logon_UserName"].Value, "Patient ID : " + intPatientID.ToString(), "BOLD - Data Patient ", err.ToString());
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
}
