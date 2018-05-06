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
using System.IO;
using System.Text;
using Microsoft.Web.UI;

public partial class Forms_MenuSetup_MenuSetupForm : System.Web.UI.Page
{
    GlobalClass gClass = new GlobalClass();

    #region protected void Page_Load(object sender, EventArgs e)
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Page.Culture = Request.Cookies["CultureInfo"].Value;
            gClass.LanguageCode = Request.Cookies["LanguageCode"].Value;
            gClass.OrganizationCode = Request.Cookies["OrganizationCode"].Value;
            txtHCulture.Value = Request.Cookies["CultureInfo"].Value;
            if (gClass.IsUserLogoned(Session.SessionID, Request.Cookies["UserPracticeCode"].Value, Request.Url.Host))
            {
                txtHApplicationURL.Value = Request.Url.Scheme + "://" + Request.Url.Host + Request.ApplicationPath + "/";

                if (!IsPostBack)
                {
                    gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value,
                                            Request.Cookies["Logon_UserName"].Value,
                                            Request.Url.Host,
                                            "Setting Form", 2, "Browse", "", "");
                    bodyMenuSetup.Style.Add("Direction", Request.Cookies["Direction"].Value);
                    bodyMenuSetup.Attributes.Add("onload", "javascript:InitialPage();");
                    ShowSystemDetailsData();

                    if (Request.Cookies["PermissionLevel"].Value == "1o" || Request.Cookies["PermissionLevel"].Value == "2t" || Request.Cookies["PermissionLevel"].Value == "3f")
                    {
                        btnAddDoctor.Visible = false;
                        btnHospital.Style["display"] = "none";
                        btnAddRefDoctor.Visible = false;
                        btnGroup.Style["display"] = "none";
                        btnCategory.Style["display"] = "none";
                        btnRegion.Style["display"] = "none";
                        btnAddTemplate.Visible = false;
                        btnLinkAddNewTemplate.Style["display"] = "none";
                        btnLinkAddNewDoctor.Visible = false;
                        btnLinkAddNewRefDoctor.Visible = false;
                        btnSaveBiochemistry.Visible = false;
                        li_Div10.Style["display"] = "none";
                        li_Div14.Style["display"] = "none";
                        lblCategory_TC.Style["width"] = "21%";
                        btnAddTemplate.Visible = false;
                    }
                    //split feature
                    string Feature = Request.Cookies["Feature"].Value;
                    string[] Features = Feature.Split(new string[] { "**" }, StringSplitOptions.None);

                    string ShowLog = Features[0];

                    if (ShowLog == "True")
                        li_Div15.Style["display"] = "block";
                        

                    if (Request.Cookies["PermissionLevel"].Value != "5n")
                    {
                        lblRefBMI.Visible = false;
                        txtRefBMI.Visible = false;
                        lblRefBMIUsed.Visible = false;

                        /*
                        //remove capability to do mass update on visit week
                            rowVisitWeeksFlag.Style["display"] = "none";
                        */
                    }

                }
                loadBiochemistry();
            }
            else
            {
                gClass.ReturnToLoginPage(Request.Url.Host, Request.Cookies["LanguageCode"].Value, Response);
                return;
            }
        }
        catch(Exception err)
        {
            string strLanguageCode;
            try
            {
                strLanguageCode = Request.Cookies["LanguageCode"].Value;
                gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Setting page (MenuSetup Form)", "Loading Patient List (Page_Load function)", err.ToString());
            }
            catch { strLanguageCode = "en-US"; }
            gClass.ReturnToLoginPage(Request.Url.Host, strLanguageCode, Response);
        }
        return;
    }
    #endregion

    #region private void ShowSystemDetailsData()
    private void ShowSystemDetailsData()
    {
        SqlCommand cmdSelect = new SqlCommand();
        DataView dvSystemDetails;

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_SystemDetails_LoadData", true);
        cmdSelect.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
        dvSystemDetails = gClass.FetchData(cmdSelect, "tblSystemDetails").Tables[0].DefaultView;

        if (dvSystemDetails.Count > 0)
        {
            txtHSystemDetailsID.Value = dvSystemDetails[0]["SystemId"].ToString();

            chkShowRace.Checked = dvSystemDetails[0]["UseRace"].ToString() != "0";

            chkShowComorbidity.Checked = dvSystemDetails[0]["PatCOM"].ToString() != "False";
            chkShowInvestigations.Checked = dvSystemDetails[0]["PatInv"].ToString() != "False";

            chkComorbidity.Checked = dvSystemDetails[0]["FUCom"].ToString() != "False";
            chkProgressVisits.Checked = dvSystemDetails[0]["FUPNotes"].ToString() != "False";
            chkInvesigations.Checked = dvSystemDetails[0]["FUinv"].ToString() != "False";
            chkImperial.Checked = dvSystemDetails[0]["Imperial"].ToString() != "False";
            cmbVisitWeeks.SelectedValue = dvSystemDetails[0]["VisitWeeksFlag"].ToString();

            chkAutoSave.Checked = dvSystemDetails[0]["AutoSave"].ToString() != "False";
            
            txtTargetBMI.Text = dvSystemDetails[0]["TargetBMI"].ToString();
            txtRefBMI.Text = dvSystemDetails[0]["ReferenceBMI"].ToString();
            chkBaseIW_BMI.Checked = dvSystemDetails[0]["IdealonBMI"].ToString() != "False";

            txtLetterheadMargin.Text = dvSystemDetails[0]["LetterheadMargin"].ToString();

            txtFacilityName.Text = dvSystemDetails[0]["FacilityName"].ToString();
            txtFacilityAddress.Text = dvSystemDetails[0]["Street"].ToString();
            txtFacilitySuburb.Text = dvSystemDetails[0]["Suburb"].ToString();
            txtFacilityState.Text = dvSystemDetails[0]["State"].ToString();
            txtFacilityPostcode.Text = dvSystemDetails[0]["Postcode"].ToString();
            txtFacilityPhone.Text = dvSystemDetails[0]["Phone"].ToString();
            txtFacilityFax.Text = dvSystemDetails[0]["Fax"].ToString();
        }
        else
            chkImperial.Checked = true;
        return;
    }
    #endregion 

    #region private bool UserFields_CanToSave()
    private bool UserFields_CanToSave()
    {
        bool flag = false;

        flag |= chkShowRace.Checked;
        flag |= chkShowComorbidity.Checked;
        flag |= chkShowInvestigations.Checked;
        flag |= chkComorbidity.Checked;
        flag |= chkProgressVisits.Checked;
        flag |= chkInvesigations.Checked;
        flag |= chkBaseIW_BMI.Checked;

        try
        {
            flag |= (txtTargetBMI.Text.Trim().Length > 0) && (Convert.ToDouble(txtTargetBMI.Text.Trim().Length) > 0); //????
            flag |= (txtRefBMI.Text.Trim().Length > 0) && (Convert.ToDouble(txtRefBMI.Text.Trim().Length) > 0); //???
        }
        catch
        {
            flag = false;
        }
        return (flag);
    }
    #endregion

    #region protected void linkbtnUpdateUserSettings_OnClick(object sender, EventArgs e)
    protected void linkbtnUpdateUserSettings_OnClick(object sender, EventArgs e)
    {
        string strScript = "";
        Boolean flag = (txtUserPW.Text.Trim().Length == 0) || (txtNewUserPW.Text.Trim().Length == 0) || (txtNewUserPW_Confirm.Text.Trim().Length == 0);

        if (flag)
            ScriptBuilder(ref strScript, "block", "Please enter all password fields...");
        else if (!txtNewUserPW.Text.Trim().Equals(txtNewUserPW_Confirm.Text.Trim()))
            ScriptBuilder(ref strScript, "block", "The password was not correctly confirmed. Please ensure that the password and confirmation match exactly....");
        else
            try
            {
                if (IsOldPasswordOK())
                    if (!CheckLast5Password())
                    {
                        UpdatePasswordHistory();
                        ScriptBuilder(ref strScript, "none", "");
                        strScript += "ShowDivMessage('Your new password has been updated successfully ...', true);";
                    }
                    else
                        ScriptBuilder(ref strScript, "block", "The new password is duplicate with your 5 previous passwords...");
                else
                    ScriptBuilder(ref strScript, "block", "The old password is not correct...");
            }
            catch (Exception err)
            {
                ScriptBuilder(ref strScript, "block", "Error in updating new password, please contact System Administrator...");
                gClass.AddErrorLogData(String.Empty, Request.Url.Host, String.Empty, "Login Form", "Update new password function", err.ToString());
            }
        ScriptManager.RegisterStartupScript(linkbtnUpdateUserSettings, linkbtnUpdateUserSettings.GetType(), Guid.NewGuid().ToString(), strScript, true);
    }
    #endregion 

    #region private Boolean CheckLast5Password()
    private Boolean CheckLast5Password()
    {
        Boolean flag = false;
        DataView dvUser;
        SqlCommand cmdCommand = new SqlCommand();

        gClass.MakeStoreProcedureName(ref cmdCommand, "sp_UsersManagement_GetLast5Password", false);
        cmdCommand.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
        dvUser = gClass.FetchData(cmdCommand, "tblUser").Tables[0].DefaultView;

        for (int xh = 0; (xh < dvUser.Count) && !flag; xh++)
        {
            string strHexPassword = HexEncoding.ToString(gClass.GetSecureBinaryData(txtNewUserPW.Text.Trim())),
                    strUserPW = HexEncoding.ToString((byte[])dvUser[xh]["UserPassword"]).Substring(0, strHexPassword.Length);
            flag = strHexPassword.Equals(strUserPW);
        }
        return flag;
    }
    #endregion

    #region private void IsOldPasswordOK() 
    private Boolean IsOldPasswordOK()
    {
        Boolean flag = false;
        SqlCommand cmdCommand = new SqlCommand();

        gClass.MakeStoreProcedureName(ref cmdCommand, "sp_UserManagement_GetUserDataByUserCode", false);
        cmdCommand.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
        DataView dvUser = gClass.FetchData(cmdCommand, "tblUser").Tables[0].DefaultView;

        if (dvUser.Count > 0)
        {
            string  strHexPassword = HexEncoding.ToString(gClass.GetSecureBinaryData(txtUserPW.Text.Trim())),
                    strUserPW = HexEncoding.ToString((byte[])dvUser[0]["UserPW"]).Substring(0, strHexPassword.Length);

            flag = strHexPassword.Equals(strUserPW);
        }
        return flag;
    }
    #endregion 

    #region private void UpdatePasswordHistory()
    private void UpdatePasswordHistory()
    {
        SqlCommand cmdCommand = new SqlCommand();

        gClass.MakeStoreProcedureName(ref cmdCommand, "sp_UsersManagement_UpdatePassword", false);
        cmdCommand.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
        cmdCommand.Parameters.Add("@NewPassword", SqlDbType.VarBinary, 50).Value = gClass.GetSecureBinaryData(txtNewUserPW.Text.Trim().Replace("'", ""));
        gClass.ExecuteDMLCommand(cmdCommand);
    }
    #endregion 

    #region protected void linkBtnDoctorSave_OnClick(object sender, EventArgs e)
    protected void linkBtnDoctorSave_OnClick(object sender, EventArgs e)
    {
        SqlCommand cmdSave = new SqlCommand();
        string strScript = String.Empty;
        Int32 intTemp = 0;

        gClass.MakeStoreProcedureName(ref cmdSave, "sp_Doctors_SaveData", true);

        cmdSave.Parameters.Add("@DoctorID", SqlDbType.Int).Value = Int32.TryParse(txtHDoctorID.Value, out intTemp) ? intTemp : 0;
        cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSave.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
        cmdSave.Parameters.Add("@Surname", SqlDbType.VarChar, 30).Value = txtSurName.Text.Replace("'", "`");
        cmdSave.Parameters.Add("@Firstname", SqlDbType.VarChar, 30).Value = txtFirstName.Text.Replace("'", "`");
        cmdSave.Parameters.Add("@Title", SqlDbType.VarChar, 10).Value = txtTitle.Text.Replace("'", "`");
        cmdSave.Parameters.Add("@DoctorBoldCode", SqlDbType.VarChar, 20).Value = txtDoctorBoldCode.Text.Replace("'", "`");
        cmdSave.Parameters.Add("@Address1", SqlDbType.VarChar, 50).Value = txtAddress1.Text.Replace("'", "`");
        cmdSave.Parameters.Add("@Address2", SqlDbType.VarChar, 50).Value = txtAddress2.Text.Replace("'", "`");
        cmdSave.Parameters.Add("@Suburb", SqlDbType.VarChar, 40).Value = txtSuburb.Text.Replace("'", "`");
        cmdSave.Parameters.Add("@Postcode", SqlDbType.VarChar, 10).Value = txtPostCode.Text.Replace("'", "`");
        cmdSave.Parameters.Add("@State", SqlDbType.VarChar, 10).Value = txtState.Text.Replace("'", "`");
        cmdSave.Parameters.Add("@Degrees", SqlDbType.VarChar, 50).Value = txtDegrees.Text.Replace("'", "`");
        cmdSave.Parameters.Add("@Speciality", SqlDbType.VarChar, 100).Value = txtSpeciality.Text.Replace("'", "`");
        cmdSave.Parameters.Add("@UseOwnLetterHead", SqlDbType.Bit).Value = chkUseOwnLetterHead.Checked;
        cmdSave.Parameters.Add("@PrefSurgeryType", SqlDbType.VarChar, 10).Value = cmbSurgeryType.SelectedValue;
        cmdSave.Parameters.Add("@PrefApproach", SqlDbType.VarChar, 50).Value = cmbApproach.SelectedValue;
        cmdSave.Parameters.Add("@PrefCategory", SqlDbType.VarChar, 10).Value = cmbCategory.SelectedValue;
        cmdSave.Parameters.Add("@PrefGroup", SqlDbType.VarChar, 10).Value = cmbGroup.SelectedValue;
        cmdSave.Parameters.Add("@vblnIsSurgeon", SqlDbType.Bit).Value = chkIsSurgeon.Checked;
        cmdSave.Parameters.Add("@vblnIsHide", SqlDbType.Bit).Value = chkIsHide.Checked;

        try
        {
            gClass.SaveDoctorData(cmdSave, 
                                    Convert.ToInt32(gClass.OrganizationCode),
                                    Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value),
                                    (Convert.ToInt32(txtHDoctorID.Value) == 0));

            gClass.SaveUserLogFile(Context.Request.Cookies["UserPracticeCode"].Value,
                Context.Request.Cookies["Logon_UserName"].Value,
                Context.Request.Url.Host, "MenuSetup Form", 2, "Save Doctor data", "Doctor ID and Name", String.Format("{0}, {0}, {0}", txtHDoctorID.Value , txtFirstName.Text , txtSurName.Text));

            strScript = "HideDivMessage(); ShowDivMessage('Saving data is done successfully...', true);";
            strScript += String.Format("if ($get('{0}').value == '0') ", txtHDoctorID.ClientID);
            strScript += "  $get('btnAddDoctor').value = 'Update Doctor';";
            strScript += String.Format("SetInnerText($get('lblDoctorName_Value'), $get('{0}').value + ' ' + $get('{0}').value + ' ' + $get('{0}').value);", txtTitle.ClientID, txtFirstName.ClientID, txtSurName.ClientID);
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                Context.Request.Cookies["Logon_UserName"].Value, "MenuSetup Form - " + (txtHDoctorID.Value.Equals(String.Empty) ? "Insert" : "Update"),
                "Save Doctor data - SaveDoctorProc function", err.ToString());
            ScriptBuilder(ref strScript, "block", "Error is saving doctor data...");
        }
        ScriptManager.RegisterStartupScript(linkBtnDoctorSave, linkBtnDoctorSave.GetType(), Guid.NewGuid().ToString(), strScript, true);
    }
    #endregion 

    #region protected void linkBtnDoctorLoad_OnClick(object sender, EventArgs e)
    protected void linkBtnDoctorLoad_OnClick(object sender, EventArgs e)
    {
        FetchDoctorData(txtHDoctorID.Value, false);
    }
    #endregion 

    #region private void FetchDoctorData(string strDrId, bool RefDoctorFlag)
    private void FetchDoctorData(string strDrId, bool RefDoctorFlag)
    {
        SqlCommand cmdSelect = new SqlCommand();
        DataView dvDoctor;

        gClass.MakeStoreProcedureName(ref cmdSelect, RefDoctorFlag ? "sp_RefDoctors_SelectRefDoctorByRefDoctorID" : "sp_Doctors_SelectDoctorByDoctorID", true);
        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        if (RefDoctorFlag)
            cmdSelect.Parameters.Add("@RefDrId", SqlDbType.VarChar, 10).Value = strDrId;
        else
            cmdSelect.Parameters.Add("@DoctorID", SqlDbType.Int).Value = Convert.ToInt32(strDrId);

        cmdSelect.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);

        dvDoctor = gClass.FetchData(cmdSelect, "tblDoctor").Tables[0].DefaultView;
        if (RefDoctorFlag)
            LoadRefDoctorData(dvDoctor);
        else
            LoadDoctorData(dvDoctor);
    }
    #endregion

    #region private void LoadDoctorData(DataView dvDoctor)
    private void LoadDoctorData(DataView dvDoctor)
    {
        String strScript = "javascript:$get('btnAddDoctor').value = 'Update doctor';";
        strScript += "SetInnerText($get('lblDoctorName_Value'), '" +dvDoctor[0]["DoctorName"].ToString()+ "');";
        strScript += "HideDivMessage();";
        ScriptManager.RegisterStartupScript(linkBtnDoctorLoad, linkBtnDoctorLoad.GetType(), Guid.NewGuid().ToString(), strScript, true);

        ClearDoctorFields();
        txtHDoctorID.Value = dvDoctor[0]["DoctorID"].ToString();
        txtSurName.Text = dvDoctor[0]["Surname"].ToString();
        txtFirstName.Text = dvDoctor[0]["Firstname"].ToString();
        txtDoctorBoldCode.Text = dvDoctor[0]["DoctorBoldCode"].ToString();
        txtTitle.Text = dvDoctor[0]["Title"].ToString();
        txtSpeciality.Text = dvDoctor[0]["Speciality"].ToString();
        txtDegrees.Text = dvDoctor[0]["Degrees"].ToString();
        txtAddress1.Text = dvDoctor[0]["Address1"].ToString();
        txtAddress2.Text = dvDoctor[0]["Address2"].ToString();
        txtSuburb.Text = dvDoctor[0]["Suburb"].ToString();
        txtState.Text = dvDoctor[0]["State"].ToString();
        txtPostCode.Text = dvDoctor[0]["Postcode"].ToString();
        chkUseOwnLetterHead.Checked = dvDoctor[0]["UseOwnLetterHead"].ToString().Equals(Boolean.TrueString);
        if (cmbSurgeryType.Items.FindByValue(dvDoctor[0]["PrefSurgeryType"].ToString()) != null)
            cmbSurgeryType.SelectedValue = dvDoctor[0]["PrefSurgeryType"].ToString();
        else
            cmbSurgeryType.SelectedIndex = 0;

        if (cmbApproach.Items.FindByValue(dvDoctor[0]["PrefApproach"].ToString()) != null)
            cmbApproach.SelectedValue = dvDoctor[0]["PrefApproach"].ToString();
        else
            cmbApproach.SelectedIndex = 0;

        if (cmbCategory.Items.FindByValue(dvDoctor[0]["PrefCategory"].ToString()) != null)
            cmbCategory.SelectedValue = dvDoctor[0]["PrefCategory"].ToString();
        else
            cmbCategory.SelectedIndex = 0;

        if (cmbGroup.Items.FindByValue(dvDoctor[0]["PrefGroup"].ToString()) != null)
            cmbGroup.SelectedValue = dvDoctor[0]["PrefGroup"].ToString();
        else
            cmbGroup.SelectedIndex = 0;
        chkIsSurgeon.Checked = dvDoctor[0]["IsSurgeon"].ToString().Equals(Boolean.TrueString);
        chkIsHide.Checked = dvDoctor[0]["Hide"].ToString().Equals(Boolean.TrueString);
    }
    #endregion 

    #region private void ClearDoctorFields(){
    private void ClearDoctorFields()
    {
        txtHDoctorID.Value = "0";
        txtSurName.Text = "";
        txtFirstName.Text = "";
        txtTitle.Text = "";
        txtDoctorBoldCode.Text = "";
        txtSpeciality.Text = "";
        txtDegrees.Text = "";
        txtAddress1.Text = "";
        txtAddress2.Text = "";
        txtSuburb.Text = "";
        txtState.Text = "";
        txtPostCode.Text = "";
        chkUseOwnLetterHead.Checked = false;
        cmbSurgeryType.SelectedIndex = 0;
        cmbApproach.SelectedIndex = 0;
        cmbCategory.SelectedIndex = 0;
        cmbGroup.SelectedIndex = 0;
        chkIsHide.Checked = false;
    }
    #endregion 

    #region protected void linkBtnRefDoctorSave_OnClick(object sender, EventArgs e)
    protected void linkBtnRefDoctorSave_OnClick(object sender, EventArgs e)
    {
        SqlCommand cmdSave = new SqlCommand();
        String strRefSurName = txtRefSurName.Text.Trim().Replace("'", "`");
        String strScript = String.Empty;

        gClass.MakeStoreProcedureName(ref cmdSave, "sp_RefDoctors_SaveData", false);
        cmdSave.Parameters.Add("@RefDrID_Old", SqlDbType.VarChar, 10).Value = txtHRefDoctorID.Value.Trim();
        cmdSave.Parameters.Add("@RefDrID", SqlDbType.VarChar, 10).Value = ((strRefSurName.Length > 4) ? strRefSurName.Substring(0, 4) : strRefSurName) + txtRefFirstName.Text.Trim().Replace("'", "`").Substring(0, 1);
        cmdSave.Parameters.Add("@Surname", SqlDbType.VarChar, 50).Value = txtRefSurName.Text.Trim().Replace("'", "`");
        cmdSave.Parameters.Add("@FirstName", SqlDbType.VarChar, 50).Value = txtRefFirstName.Text.Trim().Replace("'", "`");
        cmdSave.Parameters.Add("@Title", SqlDbType.VarChar, 15).Value = txtRefTitle.Text.Trim().Replace("'", "`");
        cmdSave.Parameters.Add("@UseFirst", SqlDbType.Bit).Value = chkUseFirst.Checked;
        cmdSave.Parameters.Add("@Address1", SqlDbType.VarChar, 50).Value = txtRefAddress1.Text.Trim().Replace("'", "`");
        cmdSave.Parameters.Add("@Address2", SqlDbType.VarChar, 50).Value = txtRefAddress2.Text.Trim().Replace("'", "`");
        cmdSave.Parameters.Add("@Suburb", SqlDbType.VarChar, 50).Value = txtRefSuburb.Text.Trim().Replace("'", "`");
        cmdSave.Parameters.Add("@PostalCode", SqlDbType.VarChar, 10).Value = txtRefPostCode.Text.Trim().Replace("'", "`");
        cmdSave.Parameters.Add("@State", SqlDbType.VarChar, 10).Value = txtRefState.Text.Trim().Replace("'", "`");
        cmdSave.Parameters.Add("@Phone", SqlDbType.VarChar, 20).Value = txtRefPhone.Text.Trim().Replace("'", "`");
        cmdSave.Parameters.Add("@Fax", SqlDbType.VarChar, 20).Value = txtRefFax.Text.Trim().Replace("'", "`");
        cmdSave.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
        cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSave.Parameters.Add("@vblnIsHide", SqlDbType.Bit).Value = chkIsRefHide.Checked;

        try
        {
            gClass.ExecuteDMLCommand(cmdSave);

            gClass.SaveUserLogFile(Context.Request.Cookies["UserPracticeCode"].Value,
                Context.Request.Cookies["Logon_UserName"].Value,
                Context.Request.Url.Host, "MenuSetup Form", 2, "Save Ref. Doctor data - " + (txtHRefDoctorID.Value.Trim().Equals(String.Empty) ? "Insert" : "Update"),
                "Ref. Doctor ID and Name", txtHRefDoctorID.Value.Trim() + ", " + txtRefFirstName.Text + ", " + txtRefSurName.Text);

            strScript  = "document.getElementById('divErrorMessage').style.display = 'none';";
            strScript += "HideDivMessage();";
            strScript += "ShowDivMessage('Saving data is done successfully...', true);";
            strScript += "if (document.getElementById('txtHRefDoctorID').value == '')";
            strScript += "  document.getElementById('btnAddRefDoctor').value = 'Update Doctor';";
            strScript += "SetInnerText(document.getElementById('lblRefDoctorName_Value'), '" + txtRefTitle.Text + " " + txtRefFirstName.Text + " " + txtRefSurName.Text + "');";
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                Context.Request.Cookies["Logon_UserName"].Value, "MenuSetup Form", "Save Ref. Doctor data - SaveRefDoctorProc function", err.ToString());
            ScriptBuilder(ref strScript, "block", "Error is saving doctor data...");
        }
        ScriptManager.RegisterStartupScript(linkBtnRefDoctorSave, linkBtnRefDoctorSave.GetType(), Guid.NewGuid().ToString(), strScript, true);
    }
    #endregion 

    #region protected void linkBtnRefDoctorLoad_OnClick(object sender, EventArgs e)
    protected void linkBtnRefDoctorLoad_OnClick(object sender, EventArgs e)
    {
        FetchDoctorData(txtHRefDoctorID.Value, true);
    }
    #endregion 

    #region private void LoadRefDoctorData(DataView dvDoctor)
    private void LoadRefDoctorData(DataView dvDoctor)
    {
        ClearRefDoctorFields();

        String strScript = "javascript:$get('btnAddRefDoctor').value = 'Update doctor';";
        strScript += String.Format("SetInnerText($get('lblRefDoctorName_Value'), '{0}');", dvDoctor[0]["RefDr_Name"].ToString());
        strScript += "HideDivMessage();";
        ScriptManager.RegisterStartupScript(linkBtnRefDoctorLoad, linkBtnRefDoctorLoad.GetType(), Guid.NewGuid().ToString(), strScript, true);

        txtHRefDoctorID.Value = dvDoctor[0]["RefDrId"].ToString();
        txtRefSurName.Text = dvDoctor[0]["Surname"].ToString();
        txtRefFirstName.Text = dvDoctor[0]["FirstName"].ToString();
        txtRefTitle.Text = dvDoctor[0]["Title"].ToString();
        txtRefPhone.Text = dvDoctor[0]["Phone"].ToString();
        txtRefFax.Text = dvDoctor[0]["Fax"].ToString();
        txtRefAddress1.Text = dvDoctor[0]["Address1"].ToString();
        txtRefAddress2.Text = dvDoctor[0]["Address2"].ToString();
        txtRefSuburb.Text = dvDoctor[0]["Suburb"].ToString();
        txtRefState.Text = dvDoctor[0]["State"].ToString();
        txtRefPostCode.Text = dvDoctor[0]["PostalCode"].ToString();
        chkUseFirst.Checked = dvDoctor[0]["UseFirst"].ToString().Equals(Boolean.TrueString);
        chkIsRefHide.Checked = dvDoctor[0]["Hide"].ToString().Equals(Boolean.TrueString);
    }
    #endregion 

    #region private void ClearRefDoctorFields()
    private void ClearRefDoctorFields()
    {
        txtHRefDoctorID.Value = "";
        txtRefSurName.Text = "";
        txtRefFirstName.Text = "";
        txtRefTitle.Text = "";
        txtRefPhone.Text = "";
        txtRefFax.Text = "";
        txtRefAddress1.Text = "";
        txtRefAddress2.Text = "";
        txtRefSuburb.Text = "";
        txtRefState.Text = "";
        txtRefPostCode.Text = "";
        chkUseFirst.Checked = false;
        chkIsRefHide.Checked = false;
    }
    #endregion     

    #region protected void linkBtnActivatePatient_OnClick(object sender, EventArgs e)
    protected void linkBtnActivatePatient_OnClick(object sender, EventArgs e)
    {
        SqlCommand cmdUpdate = new SqlCommand();
        string strScript = String.Empty;

        gClass.MakeStoreProcedureName(ref cmdUpdate, "sp_PatientData_cmdActivate", true);
        cmdUpdate.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdUpdate.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(txtHPatientID.Value.Trim());
            
        try
        {
            gClass.ExecuteDMLCommand(cmdUpdate);
            gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value, Request.Cookies["Logon_UserName"].Value, Request.Url.Host, "Setting - Activate Patient", 2, "Activating Patient Record",
                    "PatientCode", txtHPatientID.Value);


            gClass.SaveActionLog(gClass.OrganizationCode,
                                    Request.Cookies["UserPracticeCode"].Value,
                                    Request.Url.Host,
                                    System.Configuration.ConfigurationManager.AppSettings["SettingPage"].ToString(),
                                    System.Configuration.ConfigurationManager.AppSettings["LogUpdate"].ToString(),
                                    "Reactivate " + System.Configuration.ConfigurationManager.AppSettings["PatientPage"].ToString(),
                                    txtHPatientID.Value.Trim(),
                                    "");

            strScript = "ShowErrorMessageDiv('none', ''); HideDivMessage(); ShowDivMessage('Activating patient is done successfully...', true); LoadDeletedPatient();";
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Setting - Activate Patient", "Activating Patient Record", err.ToString());
            ScriptBuilder(ref strScript, "block", "Error is saving data...");
        }
        ScriptManager.RegisterStartupScript(linkBtnRefDoctorLoad, linkBtnRefDoctorLoad.GetType(), Guid.NewGuid().ToString(), strScript, true);
    }
    #endregion

    #region protected void linkBtnDeleteLogo_OnClick(object sender, EventArgs e)
    protected void linkBtnDeleteLogo_OnClick(object sender, EventArgs e)
    {
        SqlCommand cmdUpdate = new SqlCommand();
        string strScript = String.Empty;

        gClass.MakeStoreProcedureName(ref cmdUpdate, "sp_Logos_DeleteData", true);
        cmdUpdate.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdUpdate.Parameters.Add("@LogoID", SqlDbType.Int).Value = Convert.ToInt32(txtHLogoID.Value.Trim());

        try
        {
            gClass.ExecuteDMLCommand(cmdUpdate);
            gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value, Request.Cookies["Logon_UserName"].Value, Request.Url.Host, "Setting - Delete Logo", 2, "Deleting Logo Record",
                    "PatientCode", txtHLogoID.Value);
            strScript = "ShowErrorMessageDiv('none', ''); HideDivMessage(); ShowDivMessage('Deleting logo is done successfully...', true); LoadLogos();";
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Setting - Delete Logo", "Deleting Logo Record", err.ToString());
            ScriptBuilder(ref strScript, "block", "Error is deleting data...");
        }
        ScriptManager.RegisterStartupScript(linkBtnRefDoctorLoad, linkBtnRefDoctorLoad.GetType(), Guid.NewGuid().ToString(), strScript, true);
    }
    #endregion

    #region protected void linkBtnUserFieldSave_OnClick(object sender, EventArgs e)
    protected void linkBtnUserFieldSave_OnClick(object sender, EventArgs e)
    {
        SqlCommand cmdSave = new SqlCommand();
        string strResult = "", strScript = String.Empty;
        Int32 intTemp = 0;
        Decimal decTemp = 0m;

        
        gClass.MakeStoreProcedureName(ref cmdSave, "sp_SystemDetails_SaveData", true);
        
        cmdSave.Parameters.Add("@SystemID", SqlDbType.Int).Value = Int32.TryParse(txtHSystemDetailsID.Value, out intTemp) ? intTemp : 0;
        cmdSave.Parameters.Add("@UseRace", SqlDbType.SmallInt).Value = chkShowRace.Checked ? 0 : 1;
        cmdSave.Parameters.Add("@PatCOM", SqlDbType.Bit).Value = chkShowComorbidity.Checked;
        cmdSave.Parameters.Add("@PatInv", SqlDbType.Bit).Value = chkShowInvestigations.Checked;
        cmdSave.Parameters.Add("@FUCom", SqlDbType.Bit).Value = chkComorbidity.Checked;
        cmdSave.Parameters.Add("@FUPNotes", SqlDbType.Bit).Value = chkProgressVisits.Checked;
        cmdSave.Parameters.Add("@FUinv", SqlDbType.Bit).Value = chkInvesigations.Checked;
        cmdSave.Parameters.Add("@TargetBMI", SqlDbType.Decimal).Value = Decimal.TryParse(txtTargetBMI.Text, out decTemp) ? decTemp : 0;
        cmdSave.Parameters.Add("@ReferenceBMI", SqlDbType.Decimal).Value = Decimal.TryParse(txtRefBMI.Text, out decTemp) ? decTemp : 0;
        cmdSave.Parameters.Add("@IdealonBMI", SqlDbType.Bit).Value = chkBaseIW_BMI.Checked;
        cmdSave.Parameters.Add("@Imperial", SqlDbType.Bit).Value = chkImperial.Checked;
        cmdSave.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
        cmdSave.Parameters.Add("@VisitWeeksFlag", SqlDbType.Int).Value = cmbVisitWeeks.SelectedValue;
        cmdSave.Parameters.Add("@AutoSave", SqlDbType.Bit).Value = chkAutoSave.Checked;
        cmdSave.Parameters.Add("@LetterheadMargin", SqlDbType.Int, 100);
        try { cmdSave.Parameters["@LetterheadMargin"].Value = Convert.ToInt32(txtLetterheadMargin.Text); }
        catch { cmdSave.Parameters["@LetterheadMargin"].Value = 0; }

        cmdSave.Parameters.Add("@FacilityName", SqlDbType.VarChar, 100).Value = txtFacilityName.Text.Trim();
        cmdSave.Parameters.Add("@Street", SqlDbType.VarChar, 100).Value = txtFacilityAddress.Text.Trim();
        cmdSave.Parameters.Add("@Suburb", SqlDbType.VarChar, 40).Value = txtFacilitySuburb.Text.Trim();
        cmdSave.Parameters.Add("@State", SqlDbType.VarChar, 10).Value = txtFacilityState.Text.Trim();
        cmdSave.Parameters.Add("@Postcode", SqlDbType.VarChar, 10).Value = txtFacilityPostcode.Text.Trim();
        cmdSave.Parameters.Add("@Phone", SqlDbType.VarChar, 30).Value = txtFacilityPhone.Text.Trim();
        cmdSave.Parameters.Add("@Fax", SqlDbType.VarChar, 30).Value = txtFacilityFax.Text.Trim();

        try
        {
            txtTest.Value += "UPC : " + Request.Cookies["UserPracticeCode"].Value + "  " + txtHSystemDetailsID.Value + "  ";
            strResult = gClass.SaveSystemDetails(cmdSave, Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value), txtHSystemDetailsID.Value.Equals("0"));
            Response.Cookies.Set(new HttpCookie("Imperial", chkImperial.Checked ? Boolean.TrueString : Boolean.FalseString));
            Response.Cookies.Set(new HttpCookie("VisitWeeksFlag", cmbVisitWeeks.SelectedValue));
            Response.Cookies.Set(new HttpCookie("AutoSave", chkAutoSave.Checked ? Boolean.TrueString : Boolean.FalseString));

            gClass.SaveUserLogFile(Context.Request.Cookies["UserPracticeCode"].Value,
                Context.Request.Cookies["Logon_UserName"].Value,
                Context.Request.Url.Host, "MenuSetup Form", 2, "Save Users fields data", "", "");
            //txtHSystemDetailsID.Value = strResult;

            strScript = "ShowErrorMessageDiv('none', ''); HideDivMessage(); ShowDivMessage('Saving is done successfully...', true);";
        }
        catch (Exception err)
        {
            txtTest.Value += err.ToString();
            strResult = err.ToString();

            gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                    Context.Request.Cookies["Logon_UserName"].Value, "MenuSetup Form", "Save Users fields data - SaveUserFieldsProc function", err.ToString());
            ScriptBuilder(ref strScript, "block", "Error is saving data...");
        }


        cmdSave.Parameters.Clear();

        /*
        if (Request.Cookies["PermissionLevel"].Value == "5n")
        {
            gClass.MakeStoreProcedureName(ref cmdSave, "sp_SystemCodes_SaveOrganizationVisitWeeksFlag", true);
            cmdSave.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.VarChar, 10).Value = gClass.OrganizationCode;
            cmdSave.Parameters.Add("@VisitWeeksFlag", SqlDbType.Int).Value = cmbVisitWeeks.SelectedValue;
            try
            {
                gClass.ExecuteDMLCommand(cmdSave);
                gClass.SaveUserLogFile(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Cookies["Logon_UserName"].Value,
                                         Context.Request.Url.Host, "User Setting Form", 2, "Save Organization VisitWeeksFlag", "Organization Code: ", gClass.OrganizationCode);
            }
            catch (Exception err)
            {
                gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                            Context.Request.Cookies["Logon_UserName"].Value, "Organization VisitWeeksFlag", "Data saving Organization VisitWeeksFlag", err.ToString());
            }
        }
        */

        ScriptManager.RegisterStartupScript(linkBtnUserFieldSave, linkBtnUserFieldSave.GetType(), Guid.NewGuid().ToString(), strScript, true);
    }
    #endregion

    #region private void ScriptBuilder(ref String strScript, String strDisplay, String strMessage)
    private void ScriptBuilder(ref String strScript, String strDisplay, String strMessage)
    {
        strScript += "javascript:ShowErrorMessageDiv('" + strDisplay + "' , '" + strMessage + "');";
    }
    #endregion 

    protected void ImportPatient_OnClick(object sender, EventArgs e)
    {
        ArrayList fileArr = new ArrayList();
        ArrayList updateArr = new ArrayList();
        ArrayList addArr = new ArrayList();

        String patientID = "";
        String patientTitle;
        String patientFullname;
        String patientSurname;
        String patientFirstname;
        String patientAddress;
        String patientSuburb;
        String patientPostcode;
        String patientDOB;
        String patientHomePhone;
        String patientWorkPhone;
        String patientGender;
        ArrayList newPatientDetail = new ArrayList();

        String dbpatientID;

        Boolean successUpdate = true;
        String a = FileUpload1.FileName;

        //should only allow .in file
        if (FileUpload1.HasFile)
        {
            //get patient list
            System.Data.DataSet dsPatient = new System.Data.DataSet();
            System.Data.SqlClient.SqlCommand cmdSql = new System.Data.SqlClient.SqlCommand();

            cmdSql.CommandType = System.Data.CommandType.StoredProcedure;
            gClass.MakeStoreProcedureName(ref cmdSql, "sp_PatientsList_LoadAllPatient", true);
            cmdSql.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.VarChar, 10).Value = gClass.OrganizationCode;
            gClass.FetchData(cmdSql, "tblPatients");
            dsPatient = gClass.dsGlobal;

            try
            {
                Stream stream = FileUpload1.FileContent;
                StreamReader reader = new StreamReader(stream);
                string strLine = "";
                while ((strLine = reader.ReadLine()) != null)
                {
                    patientID = strLine.Substring(0, 9).Trim();

                    if (fileArr.IndexOf(patientID) == -1)
                    {
                        patientTitle = strLine.Substring(9, 5).Trim();
                        patientSurname = strLine.Substring(14, 30).Trim();
                        patientFirstname = strLine.Substring(44, 30).Trim();
                        patientAddress = strLine.Substring(74, 40).Trim();
                        patientSuburb = strLine.Substring(114, 25).Trim();
                        patientPostcode = strLine.Substring(139, 4).Trim();
                        patientDOB = strLine.Substring(143, 10).Trim();
                        patientHomePhone = strLine.Substring(194, 14).Trim();
                        patientWorkPhone = strLine.Substring(208, 15).Trim();
                        patientGender = strLine.Substring(222, 16).Trim();
                        patientFullname = patientFirstname + patientSurname + patientDOB;

                        //add new detail to array
                        newPatientDetail.Add(patientID);
                        newPatientDetail.Add(patientTitle);
                        newPatientDetail.Add(patientSurname);
                        newPatientDetail.Add(patientFirstname);
                        newPatientDetail.Add(patientAddress);
                        newPatientDetail.Add(patientSuburb);
                        newPatientDetail.Add(patientPostcode);
                        newPatientDetail.Add(patientDOB);
                        newPatientDetail.Add(patientHomePhone);
                        newPatientDetail.Add(patientWorkPhone);
                        newPatientDetail.Add(patientGender);


                        if (dsPatient.Tables[0].Select("[Patient MD Id]='" + patientID + "'").Length > 0)
                        {
                            //if there is any match in md id

                            DataRow[] drPatient = dsPatient.Tables[0].Select("[Patient MD Id]='" + patientID + "'");
                            dbpatientID = drPatient[0]["Patient Id"].ToString();

                            successUpdate = UpdatePatientData_DemoGraphics(dbpatientID, drPatient[0], newPatientDetail);

                            //if success, add to array, display in report
                            if (successUpdate == true)
                                updateArr.Add(patientID);
                        }
                        else if (dsPatient.Tables[0].Select("Fullname='" + patientFullname + "'").Length > 0)
                        {
                            //if there is any match in firstname and surname

                            DataRow[] drPatient = dsPatient.Tables[0].Select("Fullname='" + patientFullname + "'");
                            dbpatientID = drPatient[0]["Patient Id"].ToString();

                            successUpdate = UpdatePatientData_DemoGraphics(dbpatientID, drPatient[0], newPatientDetail);

                            //if success, add to array, display in report
                            if (successUpdate == true)
                                updateArr.Add(patientID);
                        }
                        else
                        {
                            DataTable dt = new DataTable();
                            DataRow drPatient = dt.NewRow();

                            successUpdate = UpdatePatientData_DemoGraphics("0", drPatient, newPatientDetail);

                            //if success, add to array, display in report
                            if (successUpdate == true)
                                updateArr.Add(patientID);
                        }

                        fileArr.Add(patientID);
                    }
                }
            }

            catch (Exception ex)
            {
                //show error div msg
            }
        }
        else
        {
            //show error div msg - no file uploaded
        }
    }

    #region private void UpdatePatientData_DemoGraphics(String dbpatientID, DataRow drPatient, ArrayList newPatientDetail)
    private Boolean UpdatePatientData_DemoGraphics(String dbpatientID, DataRow drPatient, ArrayList newPatientDetail)
    {
        //DateTimeFormatInfo dtInfo = new DateTimeFormatInfo();
        //dtInfo.ShortDatePattern = @"dd/MM/yyyy";


        GlobalClass gClass = new GlobalClass();
        SqlCommand cmdSave = new SqlCommand();

        String newPatientID = newPatientDetail[0].ToString();
        String newPatientTitle = newPatientDetail[1].ToString();
        String newPatientSurname = newPatientDetail[2].ToString();
        String newPatientFirstname = newPatientDetail[3].ToString();
        String newPatientAddress = newPatientDetail[4].ToString();
        String newPatientSuburb = newPatientDetail[5].ToString();
        String newPatientPostcode = newPatientDetail[6].ToString();
        String newPatientDOB = newPatientDetail[7].ToString();
        String newPatientHomePhone = newPatientDetail[8].ToString();
        String newPatientWorkPhone = newPatientDetail[9].ToString();
        String newPatientGender = newPatientDetail[10].ToString();

        String strNameId = String.Empty;

        strNameId = (newPatientSurname.Length == 0) ? String.Empty : ((newPatientSurname.Length > 3) ? newPatientSurname.Substring(0, 4) : newPatientSurname);
        strNameId += (newPatientFirstname.Length == 0) ? String.Empty : newPatientFirstname.Substring(0, 1);

        if (strNameId.Equals(String.Empty))
        {
            //fail to insert-- need to return something
            return false;
        }

        if (dbpatientID == "0")
            gClass.MakeStoreProcedureName(ref cmdSave, "sp_PatientData_cmdInsert", true);
        else
            gClass.MakeStoreProcedureName(ref cmdSave, "sp_PatientData_cmdUpdate", true);

        cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(34);
        cmdSave.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(87);

        cmdSave.Parameters.Add("@Patient_MDId", SqlDbType.VarChar, 7).Value = newPatientID;
        cmdSave.Parameters.Add("@NameId", SqlDbType.VarChar, 7).Value = strNameId;
        cmdSave.Parameters.Add("@Surname", SqlDbType.VarChar, 40).Value = newPatientSurname.Replace("'", "`");
        cmdSave.Parameters.Add("@Firstname", SqlDbType.VarChar, 30).Value = newPatientFirstname.Replace("'", "`");
        cmdSave.Parameters.Add("@Title", SqlDbType.SmallInt).Value = Convert.ToInt32(1);
        cmdSave.Parameters.Add("@Street", SqlDbType.VarChar, 40).Value = newPatientAddress;
        cmdSave.Parameters.Add("@Suburb", SqlDbType.VarChar, 40).Value = newPatientSuburb;
        cmdSave.Parameters.Add("@State", SqlDbType.VarChar, 10).Value = "";
        cmdSave.Parameters.Add("@Postcode", SqlDbType.VarChar, 10).Value = newPatientPostcode;
        cmdSave.Parameters.Add("@HomePhone", SqlDbType.VarChar, 30).Value = newPatientHomePhone;
        cmdSave.Parameters.Add("@WorkPhone", SqlDbType.VarChar, 30).Value = newPatientWorkPhone;
        cmdSave.Parameters.Add("@Race", SqlDbType.VarChar, 3).Value = "";
        cmdSave.Parameters.Add("@Birthdate", SqlDbType.DateTime);

        if (newPatientDOB.Trim() == String.Empty)
            cmdSave.Parameters["@Birthdate"].Value = DBNull.Value;
        else
            try
            {
                if (Convert.ToDateTime(newPatientDOB) < DateTime.Now)
                    cmdSave.Parameters["@Birthdate"].Value = Convert.ToDateTime(newPatientDOB);
                else
                    cmdSave.Parameters["@Birthdate"].Value = DBNull.Value;
            }
            catch { cmdSave.Parameters["@Birthdate"].Value = DBNull.Value; }

        cmdSave.Parameters.Add("@Sex", SqlDbType.VarChar, 1).Value = newPatientGender;
        cmdSave.Parameters.Add("@DoctorId", SqlDbType.Int).Value = Convert.ToInt32(1);
        cmdSave.Parameters.Add("@RefDrId1", SqlDbType.VarChar, 10).Value = "";
        cmdSave.Parameters.Add("@RefDrId2", SqlDbType.VarChar, 10).Value = "";
        cmdSave.Parameters.Add("@RefDrId3", SqlDbType.VarChar, 10).Value = "";

        cmdSave.Parameters.Add("@MobilePhone", SqlDbType.VarChar, 30).Value = "";
        cmdSave.Parameters.Add("@EmailAddress", SqlDbType.VarChar, 100).Value = "";
        cmdSave.Parameters.Add("@Insurance", SqlDbType.VarChar, 50).Value = "";
        cmdSave.Parameters.Add("@Patient_CustomID", SqlDbType.VarChar, 20).Value = "";

        gClass.AddLogParameters(ref cmdSave, Request.Cookies["Logon_UserName"].Value, Request.Url.Host, (dbpatientID.Equals("0")) ? "insert" : "update");

        try
        {
            if (dbpatientID.Equals("0")) // means new Patient Data, data must be inserted
            {
               // gClass.SavePatientData(1, cmdSave);
                Context.Response.SetCookie(new HttpCookie("PatientID", dbpatientID));
                gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value, Request.Cookies["Logon_UserName"].Value, Request.Url.Host,
                                        "Baseline Form", 2, "Add Data", "PatientCode", Response.Cookies["PatientID"].Value);
            }
            else //data must be Updated
            {
                cmdSave.Parameters.Add("@PatientId", SqlDbType.Int).Value = Convert.ToInt64(dbpatientID);

               // gClass.SavePatientData(2, cmdSave);
                Context.Response.SetCookie(new HttpCookie("PatientID", dbpatientID));
                gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value, Request.Cookies["Logon_UserName"].Value,
                                        Request.Url.Host, "Baseline Form", 2, "Modify Data", "PatientCode", dbpatientID);
            }
            //success
            //IsDoneSaveFlag = 1;
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Baseline", "Data saving baseline", err.ToString());
            //IsDoneSaveFlag = 0;
        }
        cmdSave.Dispose();

        return true;
    }
    #endregion
    
    #region private void loadBiochemistry()
    private void loadBiochemistry()
    {
        SqlCommand cmdSelect = new SqlCommand();
        SqlCommand cmdSelectOrganization = new SqlCommand();
        DataSet dsBiochemistryList = new DataSet();
        DataSet dsOrganizationBiochemistry = new DataSet();
        String tempCode = "";
        String tempDescription = "";
        String tempRow = "";
        String biochemistryList = "";
        String organizationBiochemistry = "";
        Boolean displayAll = true;
        String preSelect = "checked";

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_Biochemistry_LoadData", true);
        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);

        gClass.MakeStoreProcedureName(ref cmdSelectOrganization, "sp_OrganizationBiochemistry_LoadData", true);
        cmdSelectOrganization.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);

        try
        {
            dsOrganizationBiochemistry = gClass.FetchData(cmdSelectOrganization, "tblOrganizationBiochemistry");
            if (dsOrganizationBiochemistry.Tables[0].Rows.Count > 0)
            {
                displayAll = false;
                organizationBiochemistry = dsOrganizationBiochemistry.Tables[0].Rows[0]["BiochemistryCode"].ToString();
            }

            dsBiochemistryList = gClass.FetchData(cmdSelect, "tblBiochemistry");
            if (dsBiochemistryList.Tables[0].Rows.Count > 0)
            {
                //build biochemistry list table
                biochemistryList = "<table width='100%'>";
                for (int Xh = 0; Xh < dsBiochemistryList.Tables[0].Rows.Count; Xh++)
                {
                    tempCode = dsBiochemistryList.Tables[0].Rows[Xh]["Code"].ToString();
                    tempDescription = dsBiochemistryList.Tables[0].Rows[Xh]["Description"].ToString();


                    preSelect = "";
                    if (displayAll == true || (ContainsWord(tempCode, organizationBiochemistry, '_')))
                        preSelect = "checked";

                    //build table
                    if (Xh % 3 == 0)
                        biochemistryList += "<tr>";

                    tempRow = "<td style='width:100px'><input type='checkbox'runat='server' id='" + tempCode + "' " + preSelect + ">" + tempDescription + "&nbsp;&nbsp;</td>";

                    if (Xh % 3 == 0)
                        biochemistryList += "</tr>";

                    biochemistryList += tempRow;
                }
                biochemistryList += "</table>";
            }
            divBiochemistryList.InnerHtml = biochemistryList;
            dsBiochemistryList.Dispose();
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                Context.Request.Cookies["Logon_UserName"].Value, "Biochemistry Form",
                "Load Biochemistry List - LoadBiochemistryList function", err.ToString());
        }
        return;
    }
    #endregion

    #region protected void linkBtnBiochemistrySave_OnClick(object sender, EventArgs e)
    protected void linkBtnBiochemistrySave_OnClick(object sender, EventArgs e)
    {
        String strScript = String.Empty;
        try
        {
            SqlCommand cmdSave = new SqlCommand();
            gClass.MakeStoreProcedureName(ref cmdSave, "sp_OrganizationBiochemistry_SaveData", true);

            cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
            cmdSave.Parameters.Add("@BiochemistryCode", SqlDbType.VarChar, 1024).Value = txtHPatientBiochemistryValue.Value;

            gClass.ExecuteDMLCommand(cmdSave);
            gClass.SaveUserLogFile(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Cookies["Logon_UserName"].Value,
                                    Context.Request.Url.Host, "Biochemistry Form", 2, "Save Organization Biochemistry Data", "Organization Code: ", gClass.OrganizationCode);

            strScript = "javascript:linkBtnSave_CallBack(true);";
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                        Context.Request.Cookies["Logon_UserName"].Value, "PID : " + Context.Request.Cookies["PatientID"].Value, "Data saving Organization Biochemistry", err.ToString());
            strScript = "javascript:linkBtnSave_CallBack(false);";
        }
        ScriptManager.RegisterStartupScript(linkBtnBiochemistrySave, linkBtnBiochemistrySave.GetType(), Guid.NewGuid().ToString(), strScript, true);
    }
    #endregion  
    
    #region public bool ContainsWord(String word, String phrase, Char delimiter)
    public bool ContainsWord(String word, String phrase, Char delimiter)
    {
        char[] splitter = { delimiter };

        //Loop through all of the words in the phrase
        for (int i = 0; i < phrase.Split(splitter).Length; i++)
            //If the current word is equal to the word we are trying to find
            if (phrase.Split(splitter)[i].ToString() == word)
                //Return true if it is found
                return true;
        //Return false if it is not found
        return false;
    }
    #endregion
    
    #region protected void linkBtnTemplateSave_OnClick(object sender, EventArgs e)
    protected void linkBtnTemplateSave_OnClick(object sender, EventArgs e)
    {
        SqlCommand cmdSave = new SqlCommand();
        string strScript = String.Empty;
        Int32 intTemp = 0;

        gClass.MakeStoreProcedureName(ref cmdSave, "sp_Templates_SaveData", true);

        cmdSave.Parameters.Add("@TemplateID", SqlDbType.Int).Value = Int32.TryParse(txtHTemplateID.Value, out intTemp) ? intTemp : 0;
        cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSave.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
        cmdSave.Parameters.Add("@TemplateName", SqlDbType.VarChar, 20).Value = txtTemplateName.Text.Replace("'", "`");
        cmdSave.Parameters.Add("@TemplateText", SqlDbType.VarChar).Value = txtTemplateText.Text.Replace("'", "`");
        
        try
        {
            gClass.ExecuteDMLCommand(cmdSave);

            gClass.SaveUserLogFile(Context.Request.Cookies["UserPracticeCode"].Value,
            Context.Request.Cookies["Logon_UserName"].Value,
            Context.Request.Url.Host, "MenuSetup Form", 2, "Save Template data", "Template ID and Name", String.Format("{0}, {0}", txtHTemplateID.Value, txtTemplateName.Text));

            strScript = "HideDivMessage(); ShowDivMessage('Saving data is done successfully...', true);";
            strScript += String.Format("if ($get('{0}').value == '0') ", txtHTemplateID.Value);
            strScript += "  $get('btnAddTemplate').value = 'Update Template';";
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                Context.Request.Cookies["Logon_UserName"].Value, "MenuSetup Form - " + (txtHTemplateID.Value.Equals(String.Empty) ? "Insert" : "Update"),
                "Save Template data - SaveTemplateProc function", err.ToString());
            ScriptBuilder(ref strScript, "block", "Error is saving template data...");
        }
        ScriptManager.RegisterStartupScript(linkBtnDoctorSave, linkBtnDoctorSave.GetType(), Guid.NewGuid().ToString(), strScript, true);
    }
    #endregion

    #region protected void linkBtnTemplateLoad_OnClick(object sender, EventArgs e)
    protected void linkBtnTemplateLoad_OnClick(object sender, EventArgs e)
    {
        FetchTemplateData(txtHTemplateID.Value);
    }
    #endregion 
        
    #region private void FetchTemplateData(string strTemplateId)
    private void FetchTemplateData(string strTemplateId)
    {
        SqlCommand cmdSelect = new SqlCommand();
        DataView dvTemplate;

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_Templates_LoadDataByID", true);
        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSelect.Parameters.Add("@TemplateID", SqlDbType.Int).Value = Convert.ToInt32(strTemplateId);
        cmdSelect.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);

        dvTemplate = gClass.FetchData(cmdSelect, "tblTemplate").Tables[0].DefaultView;

        LoadTemplateData(dvTemplate);
    }
    #endregion

    #region private void LoadTemplateData(DataView dvTemplate)
    private void LoadTemplateData(DataView dvTemplate)
    {
        String strScript = "javascript:$get('btnAddTemplate').value = 'Update template';";
        strScript += "HideDivMessage();";
        ScriptManager.RegisterStartupScript(linkBtnTemplateLoad, linkBtnTemplateLoad.GetType(), Guid.NewGuid().ToString(), strScript, true);

        ClearTemplateFields();
        txtHTemplateID.Value = dvTemplate[0]["TemplateId"].ToString();
        txtTemplateName.Text = dvTemplate[0]["TemplateName"].ToString();
        txtTemplateText.Text = dvTemplate[0]["TemplateText"].ToString();
    }
    #endregion
        
    #region private void ClearTemplateFields(){
    private void ClearTemplateFields()
    {
        txtHTemplateID.Value = "0";
        txtTemplateName.Text = "";
        txtTemplateText.Text = "";
    }
    #endregion 
 
    #region protected void linkBtnLogoSave_OnClick(object sender, EventArgs e)
    protected void linkBtnLogoSave_OnClick(object sender, EventArgs e)
    {
        SqlCommand cmdSave = new SqlCommand();
        string strScript = String.Empty;
        Int32 intTemp = 0;

        gClass.MakeStoreProcedureName(ref cmdSave, "sp_Logos_SaveData", true);

        cmdSave.Parameters.Add("@LogoID", SqlDbType.Int).Value = Int32.TryParse(txtHLogoID.Value, out intTemp) ? intTemp : 0;
        cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSave.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
        cmdSave.Parameters.Add("@LogoName", SqlDbType.VarChar, 20).Value = txtLogoName.Text.Replace("'", "`");

        string strFilePath = "";
        string strURIPath = "";
        string strFileName = "";

        cmdSave.Parameters.Add("@LogoPath", SqlDbType.VarChar, 2048).Value = DBNull.Value;

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

                        strURIPath = "../../Photos/Logo/" + strFileName;

                        cmdSave.Parameters["@LogoPath"].Value = strURIPath;
                    }
                }
            }
            if (strURIPath == "" && tempPhotoDisplay.Value == "none")
                cmdSave.Parameters["@LogoPath"].Value = strURIPath;
            //---------------------
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host,
                                Request.Cookies["Logon_UserName"].Value, "Setting", "Save Logo", err.ToString());
        }

        try
        {
            gClass.ExecuteDMLCommand(cmdSave);

            gClass.SaveUserLogFile(Context.Request.Cookies["UserPracticeCode"].Value,
            Context.Request.Cookies["Logon_UserName"].Value,
            Context.Request.Url.Host, "MenuSetup Form", 2, "Save Logo data", "Logo ID and Name", String.Format("{0}, {0}", txtHLogoID.Value, txtLogoName.Text));

            strScript = "HideDivMessage(); ShowDivMessage('Saving data is done successfully...', true);";
            strScript += String.Format("if ($get('{0}').value == '0') ", txtHLogoID.Value);
            strScript += "  $get('btnAddLogo').value = 'Update Logo';";
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                Context.Request.Cookies["Logon_UserName"].Value, "MenuSetup Form - " + (txtHLogoID.Value.Equals(String.Empty) ? "Insert" : "Update"),
                "Save Logo data - SaveLogoProc function", err.ToString());
            ScriptBuilder(ref strScript, "block", "Error is saving template data...");
        }
        ScriptManager.RegisterStartupScript(linkBtnDoctorSave, linkBtnDoctorSave.GetType(), Guid.NewGuid().ToString(), strScript, true);

    }
    #endregion

    #region protected void linkBtnLogoLoad_OnClick(object sender, EventArgs e)
    protected void linkBtnLogoLoad_OnClick(object sender, EventArgs e)
    {
        FetchLogoData(txtHLogoID.Value);
        setPhotoVisibility();
    }
    #endregion 
        
    #region private void FetchLogoData(string strLogoId)
    private void FetchLogoData(string strLogoId)
    {
        SqlCommand cmdSelect = new SqlCommand();
        DataView dvLogo;

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_Logos_LoadDataByID", true);
        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSelect.Parameters.Add("@LogoID", SqlDbType.Int).Value = Convert.ToInt32(strLogoId);
        
        dvLogo = gClass.FetchData(cmdSelect, "tblLogo").Tables[0].DefaultView;

        LoadLogoData(dvLogo);
    }
    #endregion

    #region private void LoadLogoData(DataView dvLogo)
    private void LoadLogoData(DataView dvLogo)
    {
        String strScript = "javascript:$get('btnAddLogo').value = 'Update logo';";
        strScript += "HideDivMessage();";
        ScriptManager.RegisterStartupScript(linkBtnLogoLoad, linkBtnLogoLoad.GetType(), Guid.NewGuid().ToString(), strScript, true);

        ClearLogoFields();
        txtHLogoID.Value = dvLogo[0]["LogoId"].ToString();
        txtLogoName.Text = dvLogo[0]["LogoName"].ToString();
        detailsPhoto.ImageUrl = dvLogo[0]["LogoPath"].ToString();
        setPhotoVisibility();


    }
    #endregion

    #region private void ClearLogoFields(){
    private void ClearLogoFields()
    {
        txtHLogoID.Value = "0";
        txtLogoName.Text = "";
        //image empty
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

    #region private string GetFilePath
    private string GetFilePath(string fileName)
    {
        int intLastIndex = fileName.LastIndexOf("\\");
        if (intLastIndex == 0)
            intLastIndex = fileName.LastIndexOf("/");

        fileName = fileName.Substring(intLastIndex + 1);
        string datetime = DateTime.Now.ToString("yyyyMMddHHmmssffff");
        return System.IO.Path.Combine(GetDocumentPath(), gClass.OrganizationCode + "_" + datetime + "_" + fileName);
    }
    #endregion

    #region private void GetDocumentPath()
    private string GetDocumentPath()
    {
        string uploadFolder, strFolder = "";
        System.IO.DirectoryInfo di;
        strFolder = "Photos/Logo";

        uploadFolder = System.IO.Path.Combine(this.Context.ApplicationInstance.Request.PhysicalApplicationPath, strFolder);

        di = new System.IO.DirectoryInfo(uploadFolder);
        if (di.Exists == false) // Create the directory only if it does not already exist.
            di.Create();
        return (uploadFolder);
    }
    #endregion

    #region protected void linkBtnExportLogToExcel(object sender, EventArgs e)
    protected void linkBtnExportLogToExcel(object sender, EventArgs e)
    {
        try
        {
            System.IFormatProvider format = new System.Globalization.CultureInfo(Request.Cookies["CultureInfo"].Value.ToString(), true);

            SqlCommand cmdSelect = new SqlCommand();
            DataTable dtTable = new DataTable();
            DataSet dsTable = new DataSet();  

            gClass.MakeStoreProcedureName(ref cmdSelect, "sp_ActionLog_LoadData", true);
            cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
            cmdSelect.Parameters.Add("@Top", SqlDbType.Int).Value = 0;
            cmdSelect.Parameters.Add("@StartDate", SqlDbType.VarChar, 50);
            cmdSelect.Parameters.Add("@EndDate", SqlDbType.VarChar, 50);

            String SDate = txtSDate.Text.Trim();
            String EDate = txtEDate.Text.Trim();
            string parsedSDate = "";
            string parsedEDate = "";
            if (SDate != "")
            {
                DateTime sdt = DateTime.Parse(SDate, format);
                string sSplitDate = sdt.Day.ToString();
                string sSplitMonth = sdt.Month.ToString();
                string sSplitYear = sdt.Year.ToString();
                parsedSDate = sSplitMonth + "/" + sSplitDate + "/" + sSplitYear;
            }
            if (EDate != "")
            {
                DateTime edt = DateTime.Parse(EDate, format);
                string eSplitDate = edt.Day.ToString();
                string eSplitMonth = edt.Month.ToString();
                string eSplitYear = edt.Year.ToString();
                parsedEDate = eSplitMonth + "/" + eSplitDate + "/" + eSplitYear;
            }

            cmdSelect.Parameters["@StartDate"].Value = parsedSDate;
            cmdSelect.Parameters["@EndDate"].Value = parsedEDate;

            dsTable = gClass.FetchData(cmdSelect, "tblActionLog");
            dtTable = dsTable.Tables[0];
            ExportToSpreadsheet(dtTable, "ActionLog-" + gClass.OrganizationCode);
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                Context.Request.Cookies["Logon_UserName"].Value, "MenuSetup Form",
                "Export Log", err.ToString());
        }
    }
    #endregion

    #region protected void linkBtnExportDetailLogToExcel(object sender, EventArgs e)
    protected void linkBtnExportDetailLogToExcel(object sender, EventArgs e)
    {
        try
        {
            SqlCommand cmdSelect = new SqlCommand();
            DataTable dtTable = new DataTable();
            DataSet dsTable = new DataSet();

            gClass.MakeStoreProcedureName(ref cmdSelect, "sp_ActionDetailLog_LoadData", true);
            cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
            cmdSelect.Parameters.Add("@RecordID", SqlDbType.VarChar,50).Value = RecordID.Value;
            cmdSelect.Parameters.Add("@RecordType", SqlDbType.VarChar, 10).Value = RecordType.SelectedValue;
            dsTable = gClass.FetchData(cmdSelect, "tblActionLog");
            dtTable = dsTable.Tables[0];
            ExportToSpreadsheet(dtTable, "ActionLog-" + RecordType.SelectedValue + "-" + RecordID.Value + "-" + gClass.OrganizationCode);
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                Context.Request.Cookies["Logon_UserName"].Value, "MenuSetup Form - " + RecordID.Value,
                "Export Data Detail Log", err.ToString());
        }
    }
    #endregion

    #region public static void ExportToSpreadsheet(DataTable table, string name)
    public static void ExportToSpreadsheet(DataTable table, string name)
    {
        String strResult = "";
        HttpContext context = HttpContext.Current;
        context.Response.Clear();

        foreach (DataColumn column in table.Columns)
        {
            context.Response.Write(column.ColumnName + ",");
        }
        context.Response.Write(Environment.NewLine);
        foreach (DataRow row in table.Rows)
        {
            strResult = "";
            for (int i = 0; i < table.Columns.Count; i++)
            {
                strResult = row[i].ToString().Replace(",", " ");
                strResult = strResult.Replace("\n", " ");
                strResult = strResult.Replace("\r", " ");
                context.Response.Write(strResult + ",");
            }
            context.Response.Write(Environment.NewLine);
        }
        context.Response.ContentType = "text/csv";
        context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + name + ".csv");
        context.Response.End();
    }
    #endregion

}
