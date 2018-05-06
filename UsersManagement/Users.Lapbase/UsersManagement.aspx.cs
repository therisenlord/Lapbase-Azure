using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.Web.UI;

public partial class UsersManagement : System.Web.UI.Page
{
    
    #region Variables
    GlobalClass gClass = new GlobalClass();
    String strUserID = String.Empty, strUserPW = String.Empty;
    const String M_ADMINISTRATOR_ID = "administrator";
    #endregion 

    #region Properties
    private String UserID
    {
        get { return strUserID.ToLower(); }
        set { strUserID = value.Trim().Replace("'", ""); }
    }
    private String UserPW
    {
        get { return strUserPW; }
        set { strUserPW = value.Trim().Replace("'", ""); }
    }
    /// <summary>
    /// This is the predefined key in Web.Config file.
    /// </summary>
    private String PredefineKey
    {
        get { return System.Configuration.ConfigurationManager.AppSettings["UserManagementKey"].ToString(); }
    }
    
    #endregion

    #region protected void Page_Load(object sender, EventArgs e)
    protected void Page_Load(object sender, EventArgs e)
    {
        string strScript = String.Empty;

        try
        {
            if (!IsPostBack)
            {
                txtHost.Value = Request.Url.Host.ToLower();
                //MakeConnectionString("SqlDBConnectionString");
                LoadAllOrganizationData();
                //LoadAllUsersData();
                //FillOrganizationList();
                FillLanguageList();
                FillDefaultSortList();
                FillSubmitDataList();
            }
        }
        catch (Exception ex)
        {
            strScript = "HideDivMessage();document.getElementById('divErrorMessage').style.display = 'block';SetInnerText(document.getElementById('pErrorMessage'), 'Error in Saving Organization data ...');ActivateElements(true);";
            gClass.AddErrorLogData("0", Request.Url.Host, "", "linkbtnSaveOrganization_onclick", "Add new organization", ex.ToString());
        }
    }
    #endregion

    #region private void MakeConnectionString
    private void MakeConnectionString(string strConnection)
    {
        string strConnectionString = System.Configuration.ConfigurationManager.AppSettings[strConnection];
        GlobalClass.strLapbaseCnnString = strConnectionString;
    }
    #endregion

    #region private void FillOrganizationList()
    private void FillOrganizationList()
    {
        SqlCommand cmdSelect = new SqlCommand();
        cmdSelect.CommandType = CommandType.StoredProcedure;

        cmdSelect.CommandText = "dbo.sp_UsersManagement_LoadOrganizationData";
        cmdSelect.Parameters.Add("@JustLoadVersion", SqlDbType.Bit).Value = 0;
        cmbOrganization.DataSource = gClass.FetchData(cmdSelect, "tblOrganizations");
        cmbOrganization.DataMember = "tblOrganizations";
        cmbOrganization.DataTextField = "OrgDomainName";
        cmbOrganization.DataValueField = "Code";
        cmbOrganization.DataBind();
        cmbOrganization.Items.Insert(0, new ListItem("Choose from ..."));
        cmbOrganization.Items[0].Selected = true;

        cmbUserOrganization.DataSource = gClass.FetchData(cmdSelect, "tblUserOrganizations");
        cmbUserOrganization.DataMember = "tblUserOrganizations";
        cmbUserOrganization.DataTextField = "OrgDomainName";
        cmbUserOrganization.DataValueField = "Code";
        cmbUserOrganization.DataBind();
        cmbUserOrganization.Items.Insert(0, new ListItem("Choose from ..."));
        cmbUserOrganization.Items[0].Selected = true;
    }
    #endregion 

    #region private void FillLanguageList()
    private void FillLanguageList()
    {
        SqlCommand cmdSelect = new SqlCommand();

        cmdSelect.CommandType = CommandType.StoredProcedure;
        cmdSelect.CommandText = "dbo.sp_FetchLanguage";
        DataView dvLangauge = gClass.FetchData(cmdSelect, "tblLanguages").Tables[0].DefaultView;
        cmbLanguage.DataSource = dvLangauge;
        cmbLanguage.DataMember = "tblLanguages";
        cmbLanguage.DataTextField = "Language_Name";
        cmbLanguage.DataValueField = "LanguageInfo";
        cmbLanguage.DataBind();
        cmbLanguage.Items.Insert(0, new ListItem("Choose from ...", ""));
        cmbLanguage.Items[0].Selected = true;
    }
    #endregion

    #region private void FillDefaultSortList()
    private void FillDefaultSortList()
    {
        SqlCommand cmdSelect = new SqlCommand();

        cmdSelect.CommandType = CommandType.StoredProcedure;
        cmdSelect.CommandText = "dbo.sp_FetchSystemCode";
        cmdSelect.Parameters.Add("@CategoryCode", SqlDbType.VarChar).Value = "SORT";
        DataView dvDefaultSort = gClass.FetchData(cmdSelect, "tblDefaultSort").Tables[0].DefaultView;
        cmbSort.DataSource = dvDefaultSort;
        cmbSort.DataMember = "tblDefaultSort";
        cmbSort.DataTextField = "Description";
        cmbSort.DataValueField = "Code";
        cmbSort.DataBind();
        cmbSort.Items.Insert(0, new ListItem("Choose from ...", ""));
        cmbSort.Items[0].Selected = true;
    }
    #endregion
    
    #region private void FillSubmitDataList()
    private void FillSubmitDataList()
    {
        SqlCommand cmdSelect = new SqlCommand();

        cmdSelect.CommandType = CommandType.StoredProcedure;
        cmdSelect.CommandText = "dbo.sp_FetchSystemCode";
        cmdSelect.Parameters.Add("@CategoryCode", SqlDbType.VarChar).Value = "SUBMIT";
        DataView dvDefaultSort = gClass.FetchData(cmdSelect, "tblSubmitData").Tables[0].DefaultView;
        cmbSubmitData.DataSource = dvDefaultSort;
        cmbSubmitData.DataMember = "tblSubmitData";
        cmbSubmitData.DataTextField = "Description";
        cmbSubmitData.DataValueField = "Code";
        cmbSubmitData.DataBind();
        cmbSubmitData.Items.Insert(0, new ListItem("None", ""));
        cmbSubmitData.Items[0].Selected = true;
    }
    #endregion

    #region protected void OrganizationChange(object sender, EventArgs e)
    protected void OrganizationChange(object sender, EventArgs e)
    {
        String strScript = String.Empty;
        Int32 intOrganizationCode = Convert.ToInt32(cmbOrganization.SelectedValue);

        DataSet dsDoctor;
        System.Data.SqlClient.SqlCommand cmdSelect = new System.Data.SqlClient.SqlCommand();

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_Doctors_LoadData", false);
        cmdSelect.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.Int).Value = intOrganizationCode;
        cmdSelect.Parameters.Add("@vblnIsSurgeon", System.Data.SqlDbType.Bit).Value = true;
        cmdSelect.Parameters.Add("@vblnIsHide", System.Data.SqlDbType.Bit).Value = false;
        dsDoctor = gClass.FetchData(cmdSelect, "tblSurgon");
        for (int Xh = 0; Xh < dsDoctor.Tables["tblSurgon"].Rows.Count; Xh++)
        {            
            dsDoctor.Tables["tblSurgon"].Rows[Xh]["DoctorName"] = dsDoctor.Tables["tblSurgon"].Rows[Xh]["DoctorName"].ToString().Replace("`", "'");
        }

        cmbSurgeon.DataSource = dsDoctor;
        cmbSurgeon.DataMember = "tblSurgon";
        cmbSurgeon.DataTextField = "DoctorName";
        cmbSurgeon.DataValueField = "DoctorID";
        cmbSurgeon.DataBind();
        cmbSurgeon.Items.Insert(0, new ListItem(""));

        if (txtHRowClick.Value == "1")
        {
            String surgeonID = txtHSurgeonID.Value.ToString();
            surgeonID = surgeonID.Replace("/n", "");
            surgeonID = surgeonID.Trim();
            if (surgeonID != "")
                cmbSurgeon.SelectedValue = surgeonID;
        }
        txtHRowClick.Value = "0";
    }
    #endregion

    #region protected void UserOrganizationChange(object sender, EventArgs e)
    protected void UserOrganizationChange(object sender, EventArgs e)
    {
        Int32 intOrganizationCode = Convert.ToInt32(cmbUserOrganization.SelectedValue);
        LoadAllUsersData(intOrganizationCode);
    }
    #endregion

    #region private void LoadAllUsersData(Int32 intOrganizationCode)
    private void LoadAllUsersData(Int32 intOrganizationCode)
    {
        DataSet dsPatient = new DataSet();
        SqlCommand cmdSelect = new SqlCommand();

        cmdSelect.CommandType = CommandType.StoredProcedure;
        cmdSelect.CommandText = "dbo.sp_UsersManagement_LoadUsersByOrganization";
        cmdSelect.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.Int).Value = intOrganizationCode;
        dsPatient = gClass.FetchData(cmdSelect, "tblUsers");
        div_UsersList.InnerHtml = gClass.ShowSchema(dsPatient, Server.MapPath(".") + @"\UsersManagement\UsersXSLTFile.xsl");
    }
    #endregion

    #region protected void btnSave_onclick(object sender, EventArgs e)
    protected void btnSave_onclick(object sender, EventArgs e)
    {
        String strScript = String.Empty;
        Int64 intUserPracticeCode = 0;
        try {
            SqlCommand cmdSave = new SqlCommand();
            SqlCommand cmdDelete = new SqlCommand();

            gClass.MakeStoreProcedureName(ref cmdSave, "sp_UsersManagement_SaveData", false);
            cmdSave.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = txtHUCode.Value;
            cmdSave.Parameters["@UserPracticeCode"].Direction = ParameterDirection.InputOutput;

            cmdSave.Parameters.Add("@UserID", SqlDbType.VarChar, 25).Value = txtUsername.Text.Replace("'", "");
            cmdSave.Parameters.Add("@UserPW", SqlDbType.VarBinary, 50).Value = gClass.GetSecureBinaryData(txtPassword.Text.Trim().Replace("'", ""));
            cmdSave.Parameters.Add("@BlankPwd", SqlDbType.Bit).Value = txtPassword.Text.Trim().Replace("'", "") == ""?true:false;
            cmdSave.Parameters.Add("@User_Name", SqlDbType.VarChar, 50).Value = txtFirstname.Text;
            cmdSave.Parameters.Add("@User_SirName", SqlDbType.VarChar, 50).Value = txtLastname.Text;
            cmdSave.Parameters.Add("@PermissionLevel", SqlDbType.VarChar, 3).Value = cmbPermissionLvl.SelectedValue.ToString();
            cmdSave.Parameters.Add("@ShowLog", SqlDbType.Bit).Value = chkShowLog.Checked;
            cmdSave.Parameters.Add("@ShowRegistry", SqlDbType.Bit).Value = chkShowRegistry.Checked;

            String surgeonID = cmbSurgeon.SelectedValue.ToString();
            if (surgeonID.Trim() != "")
                cmdSave.Parameters.Add("@SurgeonID", SqlDbType.Int).Value = Convert.ToInt64(surgeonID);

            gClass.AddNewUser(cmdSave);
            intUserPracticeCode = Convert.ToInt64(cmdSave.Parameters["@UserPracticeCode"].Value);
            cmdSave.Parameters.Clear();
            gClass.MakeStoreProcedureName(ref cmdSave, "sp_UsersManagement_AddUserRole", false);
            cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt64(cmbOrganization.SelectedValue);
            cmdSave.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = intUserPracticeCode.ToString();
            cmdSave.Parameters.Add("@RoleID", SqlDbType.Int).Value = 3;
            gClass.ExecuteDMLCommand(cmdSave);
            
            if (chkChangePassword.Checked == true)
            {
                //delete password history
                gClass.MakeStoreProcedureName(ref cmdDelete, "sp_UsersManagement_DeletePasswordHistory", false);
                cmdDelete.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = intUserPracticeCode;
                gClass.ExecuteDMLCommand(cmdDelete);    
            }

            ////if (chkReplicate.Checked)   NewUserReplicateData();
            cmbUserOrganization.SelectedIndex = cmbOrganization.SelectedIndex;
            LoadAllUsersData(Convert.ToInt32(cmbOrganization.SelectedValue));
            strScript = "UserFields_ClearFields();ShowDivMessage('Saving user data is done successfully...', true);";
            strScript += "document.getElementById('divErrorMessage').style.display = 'none';SetInnerText(document.getElementById('pErrorMessage'), '');ActivateElements(true);";



        
        }
        catch (Exception ex)
        {
            txtTest.Value += ex.ToString();
            gClass.AddErrorLogData("0", Request.Url.Host, "", "btnSave_onclick", "Add new user", ex.ToString());
            strScript = "HideDivMessage();document.getElementById('divErrorMessage').style.display = 'block';SetInnerText(document.getElementById('pErrorMessage'), 'Error in saving user data...');ActivateElements(true);";
        }
        ScriptManager.RegisterStartupScript(UpdatePanel1, btnSave.GetType(), Guid.NewGuid().ToString(), strScript, true);
    }
    #endregion 

    #region private void NewUserReplicateData()
    private void NewUserReplicateData()
    {
        int DemoUserCode = 0;
        System.Collections.Generic.List<string> strTables = new System.Collections.Generic.List<string>();

        strTables.Add("dbo.tblCodes");
        strTables.Add("dbo.tblDoctors");
        strTables.Add("dbo.tblHospitals");
        strTables.Add("dbo.tblIdealWeights");
        strTables.Add("dbo.tblReferringDoctors");
        strTables.Add("dbo.tblSystemCodes");
        strTables.Add("dbo.tblSystemDetails");
        strTables.Add("dbo.tblSystemNormals");
        strTables.Add("dbo.tblPatients");
        strTables.Add("dbo.tblPatientWeightData");
        strTables.Add("dbo.tblPatientConsult");
        strTables.Add("dbo.tblOpEvents");
        strTables.Add("dbo.tblComplications");
        strTables.Add("dbo.tblPatientDocuments");

        try
        {
            using (SqlConnection cnnSql = new SqlConnection(GlobalClass.strLapbaseCnnString))
            {
                cnnSql.Open();
                
                SqlCommand cmdSql = cnnSql.CreateCommand();
                DataSet dsSql = new DataSet();
                SqlDataAdapter daSql = new SqlDataAdapter();
                daSql.SelectCommand = cnnSql.CreateCommand();
                daSql.SelectCommand.CommandType = CommandType.Text;
                cmdSql.CommandType = CommandType.Text;
                
                // Fetch Demo User Code
                cmdSql.CommandText = "Select UserPracticeCode from dbo.tblUsers where UserID = 'demo'";
                try { DemoUserCode = Convert.ToInt32(cmdSql.ExecuteScalar().ToString()); }
                catch (Exception err){ DemoUserCode = 0; }

                SqlTransaction transSql = cnnSql.BeginTransaction(IsolationLevel.ReadCommitted);
                daSql.SelectCommand.Transaction = transSql;
                cmdSql.Transaction = transSql;
                
                foreach (string strTable in strTables)
                {
                    SetIndentityInsert(cmdSql, strTable, " ON");
                    daSql.SelectCommand.CommandText = "Select * from " + strTable + " where UserPracticeCode = " + DemoUserCode; // it should be 13 , the code of demo user
                    dsSql.Tables.Clear();
                    daSql.Fill(dsSql, strTable);
                    AppendDataForNewUser(dsSql, cmdSql);
                    SetIndentityInsert(cmdSql, strTable, " OFF");
                }
                transSql.Commit();
                cnnSql.Close();
            }
        }
        catch (Exception err)
        {

        }
    }
    #endregion 

    #region private void AppendDataForNewUser(DataSet dsSql, SqlCommand cmdSql)
    private void AppendDataForNewUser(DataSet dsSql, SqlCommand cmdSql)
    {
        DataTable dtTable = dsSql.Tables[0];
        string strSqlCommand = "insert into " + dtTable.TableName + "( ";

        for (int Idx = 0; Idx < dtTable.Columns.Count; Idx++)
            strSqlCommand += "[" + dtTable.Columns[Idx].ColumnName + "] , ";
        strSqlCommand = strSqlCommand.Substring(0, strSqlCommand.Length - 2) + ") values (";

        for (int Idx = 0; Idx < dtTable.Rows.Count; Idx++){
            string strSql = strSqlCommand;
            for (int Xh = 0; Xh < dtTable.Columns.Count; Xh++)
                if (dtTable.Columns[Xh].ColumnName.ToLower().Equals("userpracticecode"))
                    strSql += gClass.User_SNo.ToString() + ", ";
                else
                {
                    switch (dtTable.Columns[Xh].DataType.ToString().ToUpper())
                    {
                        case "SYSTEM.DATETIME":
                            Page.Culture = "en-US";
                            if (dtTable.Rows[Idx].IsNull(Xh))
                                strSql += "null, ";
                            else
                                strSql += "'" +  Convert.ToDateTime(dtTable.Rows[Idx][Xh].ToString().Substring(0, dtTable.Rows[Idx][Xh].ToString().IndexOf(" "))).ToShortDateString() + "', ";
                            break;

                        case "SYSTEM.STRING":
                            if (dtTable.Rows[Idx].IsNull(Xh))
                                strSql += "'', ";
                            else
                                strSql += "'" + dtTable.Rows[Idx][Xh].ToString().Replace("'", "`") + "', ";
                            break;

                        case "SYSTEM.BOOLEAN":
                            strSql += (dtTable.Rows[Idx][Xh].ToString().ToUpper().Equals("TRUE") ? "1" : "0") + ", ";
                            break;
                        
                        default:
                            strSql += dtTable.Rows[Idx][Xh].ToString() + ", ";
                            break;
                    }
                }
            
            strSql = strSql.Substring(0, strSql.Length - 2);
            strSql += ")";

            cmdSql.CommandText = strSql;
            cmdSql.ExecuteNonQuery();
        }
    }
    #endregion

    #region private void SetIndentityInsert(string strTableName, string strIndentity)
    private void SetIndentityInsert(SqlCommand cmdDest, string strTableName, string strIndentity)
    {
        switch (strTableName)
        {
            case "dbo.tblDoctors":
            case "dbo.tblSystemDetails":
            case "dbo.tblPatients":
            case "dbo.tblPatientConsult" :
            case "dbo.tblOpEvents" :
            case "dbo.tblComplications" : 
            case "dbo.tblPatientDocuments" :
                cmdDest.CommandText = "Set Identity_Insert " + strTableName + strIndentity;
                cmdDest.ExecuteNonQuery();
                break;
        }
        return;
    }
    #endregion

    #region protected void btnDelete_onlick(object sender, EventArgs e)
    protected void btnDelete_onlick(object sender, EventArgs e)
    {
        SqlCommand cmdSelect = new SqlCommand();
        Int32 int32Temp = 0;
        Int32 idx = txtHUCode.Value.IndexOf(";");
        String strScript = String.Empty, strUserCode = txtHUCode.Value.Substring(0, idx), strPermissionCode = txtHUCode.Value.Substring(idx+1);

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_UsersManagement_ActivateUser", false);
        cmdSelect.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Int32.TryParse(strUserCode, out int32Temp) ? int32Temp : 0;
        cmdSelect.Parameters.Add("@UserID", SqlDbType.VarChar, 25).Value = String.Empty;
        cmdSelect.Parameters.Add("@strOrganizationName", SqlDbType.VarChar, 100).Value = String.Empty;
        cmdSelect.Parameters.Add("@PermissionFlag", SqlDbType.Int).Value = Int32.TryParse(strPermissionCode, out int32Temp) ? int32Temp : 0;
        try
        {
            gClass.ExecuteDMLCommand(cmdSelect);
            LoadAllUsersData(Convert.ToInt32(cmbUserOrganization.SelectedValue));
        }
        catch (Exception ex) {
            gClass.AddErrorLogData("0", Request.Url.Host, "", "Users Management Section, delete user", "btnDelete_onlick", ex.ToString());
            strScript += "document.getElementById('divErrorMessage').style.display='block';";
            strScript += "SetInnerText(document.getElementById('pErrorMessage'), 'Error in deleting user...');";
            ScriptManager.RegisterStartupScript(btnDelete, btnDelete.GetType(), Guid.NewGuid().ToString(), strScript, true);
        }
    }
    #endregion 

    #region protected void btnDeactivate_onlick(object sender, EventArgs e)
    protected void btnDeactivate_onlick(object sender, EventArgs e)
    {
        SqlCommand cmdSelect = new SqlCommand();
        Int32 int32Temp = 0;
        Int32 idx = txtHOrganizationCode.Value.IndexOf(";");
        String strScript = String.Empty, strOrganizationCode = txtHOrganizationCode.Value.Substring(0, idx), strPermissionCode = txtHOrganizationCode.Value.Substring(idx + 1);

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_UsersManagement_ActivateOrganization", false);
        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Int32.TryParse(strOrganizationCode, out int32Temp) ? int32Temp : 0;
        cmdSelect.Parameters.Add("@InactiveFlag", SqlDbType.Int).Value = Int32.TryParse(strPermissionCode, out int32Temp) ? int32Temp : 0;
        try
        {
            gClass.ExecuteDMLCommand(cmdSelect);
            LoadOrganization();
        }
        catch (Exception ex)
        {
            gClass.AddErrorLogData("0", Request.Url.Host, "", "Users Management Section, deactivate organization", "btnDeactivate_onlick", ex.ToString());
            strScript += "document.getElementById('divErrorMessage').style.display='block';";
            strScript += "SetInnerText(document.getElementById('pErrorMessage'), 'Error in deactivating organization...');";
            ScriptManager.RegisterStartupScript(btnDelete, btnDelete.GetType(), Guid.NewGuid().ToString(), strScript, true);
        }
    }
    #endregion 

    #region protected void btnLogonServer_OnClick(object sender, EventArgs e)
    protected void btnLogonServer_OnClick(object sender, EventArgs e)
    {
        string strScript = String.Empty;

        try
        {
            if (CheckUserData())
            {
                strScript += "document.getElementById('div_Login').style.display='none';";
                strScript += "document.getElementById('div_Menu').style.display='block';";
                //strScript += "document.getElementById('div_AllUsers').style.display='block';";
                strScript += "document.getElementById('div_AllOrganizations').style.display='block';";
                strScript += "document.getElementById('divErrorMessage').style.display='none';";
                strScript += "SetInnerText(document.getElementById('pErrorMessage'), '');";
            }
            else
            {
                strScript += "document.getElementById('divErrorMessage').style.display='block';";
                strScript += "SetInnerText(document.getElementById('pErrorMessage'), 'Access denied...');";
            }
            ScriptManager.RegisterStartupScript(btnLogonServer, btnLogonServer.GetType(), "key", strScript, true);
        }
        catch (Exception ex)
        {
            gClass.AddErrorLogData("0", Request.Url.Host, "", "Users Management Section", "btnLogonServer_OnClick", ex.ToString());
        }
    }
    #endregion

    #region private Boolean CheckUserData
    private bool CheckUserData()
    {
        Boolean LoginFlag = true;
        this.UserID = txtUserID.Text;
        this.UserPW = txtUserPW.Text;

        if (this.UserID == String.Empty || this.UserPW == String.Empty) LoginFlag = false;
        else if (! this.UserID.Equals(M_ADMINISTRATOR_ID)) LoginFlag = false;
        else{
            LoginFlag = this.UserID.Equals(M_ADMINISTRATOR_ID);
            LoginFlag &= this.UserPW.Equals(this.PredefineKey);
        }

        //if ((txtUserID.Text.Trim() == "") || (txtUserPW.Text.Trim() == "")) Loginflag = false;
        //else if (!txtUserID.Text.Trim().ToLower().Equals("administrator")) Loginflag = false;
        //else
        //{
        //    gClass.UserID = txtUserID.Text.Trim().Replace("'", "");
        //    gClass.UserPassword = txtUserPW.Text.Trim().Replace("'", "");
        //    gClass.OrganizationCode = String.Empty;
        //    try
        //    {
        //        //switch (gClass.IsValidUserNamePassword(txtUserPW.Text.Trim(), ref txtTest, false))
        //        //{
        //        //    case 0:     // The username or password is incorrect
        //        //        Loginflag = false;
        //        //        break;

        //        //    case 1:     // The user is valied
        //        //        Loginflag = true;
        //        //        break;

        //        //    case 2: // The user is valid but the account is suspended
        //        //        Loginflag = false;
        //        //        break;
        //        //}
        //    }
        //    catch (Exception err)
        //    {
        //        Loginflag = false;
        //        gClass.AddErrorLogData("0", Request.Url.Host, "", "Users Management Section", "CheckUserData function", err.ToString());
        //    }
        //}

        return LoginFlag;
    }
    #endregion

    #region private void LoadAllOrganizationData()
    private void LoadAllOrganizationData()
    {
        string strScript = String.Empty;
        try
        {
        LoadOrganization();
        LoadVersion();
        LoadPermission();

        FillOrganizationList();
        }
        catch (Exception ex)
        {
            strScript = "HideDivMessage();document.getElementById('divErrorMessage').style.display = 'block';SetInnerText(document.getElementById('pErrorMessage'), 'Error in Saving Organization data ...');ActivateElements(true);";
            gClass.AddErrorLogData("0", Request.Url.Host, "", "linkbtnSaveOrganization_onclick", "Add new organization", ex.ToString());
        }
    }
	#endregion
    
    #region private void LoadOrganization()
    private void LoadOrganization()
    {
        string strScript = String.Empty;
        try
        {
            DataSet dsPatient = new DataSet();
            SqlCommand cmdSelect = new SqlCommand();

            cmdSelect.CommandType = CommandType.StoredProcedure;
            cmdSelect.CommandText = "dbo.sp_UsersManagement_LoadOrganizationData";
            cmdSelect.Parameters.Add("@JustLoadVersion", SqlDbType.Bit).Value = 0;
            dsPatient = gClass.FetchData(cmdSelect, "tblOrganizations");
            div_OrganizationsList.InnerHtml = gClass.ShowSchema(dsPatient, Server.MapPath(".") + @"\UsersManagement\OrganizationsXSLTFile.xsl");
        }
        catch (Exception ex)
        {
            strScript = "HideDivMessage();document.getElementById('divErrorMessage').style.display = 'block';SetInnerText(document.getElementById('pErrorMessage'), 'Error in Saving Organization data ...');ActivateElements(true);";
            gClass.AddErrorLogData("0", Request.Url.Host, "", "linkbtnSaveOrganization_onclick", "Add new organization", ex.ToString());
        }
    }
	#endregion

    #region private void LoadVersion()
    private void LoadVersion()
    {
        SqlCommand cmdSelect = new SqlCommand();

        cmdSelect.CommandType = CommandType.StoredProcedure;
        cmdSelect.CommandText = "dbo.sp_UsersManagement_LoadOrganizationData";
        cmdSelect.Parameters.Add("@JustLoadVersion", SqlDbType.Bit).Value = 0;

        cmdSelect.Parameters["@JustLoadVersion"].Value = 1;
        cmbVersion.DataSource = gClass.FetchData(cmdSelect, "tblOrganizations");
        cmbVersion.DataMember = "tblOrganizations";
        cmbVersion.DataTextField = "VersionNo";
        cmbVersion.DataValueField = "RewritingURL";
        cmbVersion.DataBind();
    }
    #endregion

    #region private void LoadPermission()
    private void LoadPermission()
    {
        SqlCommand cmdSelect = new SqlCommand();

        cmdSelect.CommandType = CommandType.StoredProcedure;
        cmdSelect.CommandText = "dbo.sp_UsersManagement_LoadPermissionLevel";

        cmbPermissionLvl.DataSource = gClass.FetchData(cmdSelect, "tblPermission");
        cmbPermissionLvl.DataMember = "tblPermission";
        cmbPermissionLvl.DataTextField = "Description";
        cmbPermissionLvl.DataValueField = "PermissionLvl";
        cmbPermissionLvl.DataBind();
    }
    #endregion

    #region protected void linkbtnSaveOrganization_onclick(object sender, EventArgs e)
    protected void linkbtnSaveOrganization_onclick(object sender, EventArgs e)
    {
        string strScript = String.Empty;
        Int32 currOrganizationCode = Convert.ToInt32(txtHCode.Value);
        /*
        if (!IsDomainURLOK())
        {
            strScript = "HideDivMessage();document.getElementById('divErrorMessage').style.display = 'block';SetInnerText(document.getElementById('pErrorMessage'), 'The domain name is not valid...');ActivateElements(true);";
        }
        else
        {*/
            SqlCommand cmdSave = new SqlCommand();

            strScript = "javascript:controlBar_Buttons_OnClick(3);";
            string strSelectedLanguage = cmbLanguage.SelectedValue, strLanguage = "", strCulturInfo = "";
            if (strSelectedLanguage.Trim().Length > 0)
            {
                int Idx = strSelectedLanguage.IndexOf(" ");

                strLanguage = strSelectedLanguage.Substring(0, Idx).Trim();
                strSelectedLanguage = strSelectedLanguage.Substring(Idx + 1).Trim();
                strCulturInfo = strSelectedLanguage.Substring(0, strSelectedLanguage.IndexOf(" "));
            }
            else { strLanguage = strCulturInfo = String.Empty; }

            gClass.MakeStoreProcedureName(ref cmdSave, "sp_UsersManagement_SaveOrganizationData", false);
            cmdSave.Parameters.Add("@intOrganizationCode", SqlDbType.Int).Value = currOrganizationCode;
            cmdSave.Parameters["@intOrganizationCode"].Direction = ParameterDirection.InputOutput;
            cmdSave.Parameters.Add("@OrgDomainName", SqlDbType.VarChar, 100).Value = txtDomainName.Text.Trim().ToLower().Replace("http://", String.Empty);
            cmdSave.Parameters.Add("@VersionNo", SqlDbType.Decimal).Value = Convert.ToDecimal(cmbVersion.SelectedItem.Text);
            cmdSave.Parameters.Add("@ApplicationMode", SqlDbType.Int).Value = 1; // default value : 1 means this domain is active
            cmdSave.Parameters.Add("@LanguageCode", SqlDbType.VarChar, 10).Value = strLanguage;
            cmdSave.Parameters.Add("@CultureInfo", SqlDbType.VarChar, 10).Value = strCulturInfo;
            cmdSave.Parameters.Add("@SuperBill", SqlDbType.Bit).Value = radioSuperBill_yes.Checked ? true : false;
            cmdSave.Parameters.Add("@DataClamp", SqlDbType.Bit).Value = radioDataClamp_yes.Checked ? true : false;
            cmdSave.Parameters.Add("@DefaultSort", SqlDbType.VarChar, 10).Value = cmbSort.SelectedValue;
            cmdSave.Parameters.Add("@EMR", SqlDbType.Bit).Value = radioEMR_yes.Checked ? true : false;
            cmdSave.Parameters.Add("@Export", SqlDbType.Bit).Value = radioExport_yes.Checked ? true : false;
            cmdSave.Parameters.Add("@BSRExport", SqlDbType.Bit).Value = radioBSRExport_yes.Checked ? true : false;
            cmdSave.Parameters.Add("@SubmitData", SqlDbType.VarChar, 100).Value = cmbSubmitData.SelectedValue;
            cmdSave.Parameters.Add("@PracticeBoldCode", SqlDbType.VarChar, 20).Value = txtBoldCode.Text.Trim();
            cmdSave.Parameters.Add("@AdminEmail", SqlDbType.VarChar, 200).Value = txtAdminEmail.Text.Trim();

            try
            {
                gClass.ExecuteDMLCommand(cmdSave);
                LoadOrganization();
                FillOrganizationList();
                if (currOrganizationCode == 0)
                {
                    CreateDatabasePartition4Organization(currOrganizationCode, Convert.ToInt32(cmdSave.Parameters["@intOrganizationCode"].Value.ToString()));
                }
                
                strScript += "HideDivMessage();UserFields_ClearFields();ShowDivMessage('Saving Organization data is done successfully...', true);";
                strScript += "document.getElementById('divErrorMessage').style.display = 'none';SetInnerText(document.getElementById('pErrorMessage'), '');ActivateElements(true);";
                if (!txtHVersionNo.Value.Equals("0") && cmbVersion.SelectedValue != txtHVersionNo.Value)
                {
                    UpgradeApplication(ref strScript);
                }
                txtTest.Value = strScript;
            }
            catch (Exception ex)
            {
                strScript = "HideDivMessage();document.getElementById('divErrorMessage').style.display = 'block';SetInnerText(document.getElementById('pErrorMessage'), 'Error in Saving Organization data ...');ActivateElements(true);";
                gClass.AddErrorLogData("0", Request.Url.Host, "", "linkbtnSaveOrganization_onclick", "Add new organization", ex.ToString());
            }
        //}
        ScriptManager.RegisterStartupScript(linkbtnSaveOrganization, linkbtnSaveOrganization.GetType(), Guid.NewGuid().ToString(), strScript, true);
    }
    #endregion 

    #region private void UpgradeApplication( )
    private void UpgradeApplication(ref String strScript)
    {

        Decimal decTemp = 0m;
        SqlCommand cmdUpgrade = new SqlCommand();

        Decimal.TryParse(txtHNewVersionNo.Value, out decTemp);
        if (decTemp > 0)
        {
            gClass.MakeStoreProcedureName(ref cmdUpgrade, "sp_ChangeApplicationVersion", false);
            cmdUpgrade.Parameters.Add("@vintOrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(txtHCode.Value);
            cmdUpgrade.Parameters.Add("@vdecVersionNo", SqlDbType.Decimal).Value = Decimal.TryParse(txtHNewVersionNo.Value, out decTemp) ? decTemp : 0m;
            cmdUpgrade.Parameters.Add("@vdecOldVersioNo", SqlDbType.Decimal).Value = Decimal.TryParse(txtHVersionNo.Value, out decTemp) ? decTemp : 0m;
            txtTest.Value += decTemp.ToString();
            gClass.ExecuteDMLCommand(cmdUpgrade);
            strScript += "ShowDivMessage('Upgrading the application from " + txtHVersionNo.Value + " to " + txtHNewVersionNo.Value + " is done successfully...', true);";
        }
    }
    #endregion 

    #region private bool IsDomainURLOK()
    private bool IsDomainURLOK()
    {
        bool flag = false;

        System.IO.Stream sStream;
        System.Net.HttpWebRequest URLReq;
        System.Net.HttpWebResponse URLRes;

        try
        {
            URLReq = (System.Net.HttpWebRequest)System.Net.WebRequest.Create("http://" + txtDomainName.Text.ToLower().Replace("http://", "") + "/WebServices/GlobalWebService.asmx");
            URLRes = (System.Net.HttpWebResponse)URLReq.GetResponse();
            sStream = URLRes.GetResponseStream();
            string reader = new System.IO.StreamReader(sStream).ReadToEnd();
            flag = true;
        }
        catch (Exception ex) 
        {
            flag = false;
            txtTest.Value += "  " + ex.ToString();
            //gClass.AddErrorLogData("0", "", Request.Url.Host, "IsDomainURLOK", "Add new/Update Organization", ex.ToString()); 
        }
        return flag;
    }
    #endregion

    #region CreateDatabasePartition4Organization(Int32 currOrganizationCode, Int32 intOrganizationCode)
    private void CreateDatabasePartition4Organization(Int32 currOrganizationCode, Int32 intOrganizationCode)
    {
        try
        {
            String strOrganizationName = txtDomainName.Text.Trim().ToLower().Replace("http://", String.Empty);
            String strOranizationCode = MakeZeroString(intOrganizationCode);
            String strDataFileName = "datafile_" + strOranizationCode;
            String strFileGroupName = "filegrp_" + strOranizationCode;
            ServerConnection cnnServer = new ServerConnection(System.Configuration.ConfigurationManager.AppSettings["DataCentreHost"], "sa", "q2c4b7m1");
            Server myServer = new Server(cnnServer);
            Database LapbaseDatabase = myServer.Databases[System.Configuration.ConfigurationManager.AppSettings["LapbaseDatabase"]];
            FileGroup orgFileGroup;

            if (LapbaseDatabase.FileGroups.Contains(strFileGroupName))
                orgFileGroup = LapbaseDatabase.FileGroups[strFileGroupName];
            else
            {
                orgFileGroup = new FileGroup(LapbaseDatabase, strFileGroupName);
                orgFileGroup.Create();
            }

            try
            {
                DataFile orgDataFile = new DataFile(orgFileGroup, strDataFileName, LapbaseDatabase.PrimaryFilePath + @"\" + strDataFileName + ".ndf");
                orgDataFile.IsPrimaryFile = false;
                orgDataFile.Size = 10240;   //Initial size: 10 MB
                orgDataFile.Growth = 10240; //Growth by : 10 MB
                orgDataFile.Create();
                PartitionScheme schemePartiton = LapbaseDatabase.PartitionSchemes["scheme_OrganizationCodePartition"];
                try { schemePartiton.NextUsedFileGroup = orgFileGroup.Name; schemePartiton.Alter(); }
                catch (Exception err) { gClass.AddErrorLogData("0", Request.Url.Host, "", "CreateDatabasePartition4Organization", "schemePartiton.NextUsedFileGroup", err.ToString()); }
                PartitionFunction fnPartition = LapbaseDatabase.PartitionFunctions["fn_OrganizationCodePartition"];
                try { fnPartition.SplitRangePartition(intOrganizationCode); fnPartition.Alter(); }
                catch (Exception err) { gClass.AddErrorLogData("0", Request.Url.Host, "", "CreateDatabasePartition4Organization", "fnPartition.SplitRangePartition", err.ToString()); }
            }
            catch (Exception err) { gClass.AddErrorLogData("0", Request.Url.Host, "", "CreateDatabasePartition4Organization", "Add new organization", err.ToString()); }


            LapbaseDatabase.Refresh();
            cnnServer.Disconnect();
        }
        catch (Exception ex) { gClass.AddErrorLogData("0", Request.Url.Host, "", "linkbtnSaveOrganization_onclick", "Add new organization", ex.ToString()); }
    }
    #endregion 

    #region private String StringMakeZeroString(Int32 intOrganizationCode){
    private String MakeZeroString(Int32 intOrganizationCode){
        String strTemp = intOrganizationCode.ToString();

        while (strTemp.Length < 3) strTemp = "0" + strTemp;
        return strTemp;
    }
    #endregion 

    #region protected void btnOrganizationResetData_onclick(object sender, EventArgs e)
    protected void btnOrganizationResetData_onclick(object sender, EventArgs e)
    {
        SqlCommand cmdResetData = new SqlCommand();
        String strScript = String.Empty;

        gClass.MakeStoreProcedureName(ref cmdResetData, "sp_usersManagement_ResetOrganizationData", false);
        cmdResetData.Parameters.Add("@intOrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(txtHCode.Value);
        try{
            gClass.ExecuteDMLCommand(cmdResetData);
            ShrinkDatabase();
            strScript += "UserFields_ClearFields();ShowDivMessage('Reseting Organization data is done successfully...', true);";
            strScript += "document.getElementById('divErrorMessage').style.display = 'none';SetInnerText(document.getElementById('pErrorMessage'), '');";
        }
        catch (Exception ex)
        {
            strScript = "HideDivMessage();document.getElementById('divErrorMessage').style.display = 'block';SetInnerText(document.getElementById('pErrorMessage'), 'Error in Reset Organization data ...');ActivateElements(true);";
            gClass.AddErrorLogData("0", Request.Url.Host, "", "btnOrganizationResetData_onclick", "Add new organization", ex.ToString());
            txtTest.Value = ex.ToString();
        }
        ScriptManager.RegisterStartupScript(btnOrganizationResetData, btnOrganizationResetData.GetType(), Guid.NewGuid().ToString(), strScript, true);
    }
	#endregion

    #region private void ShrinkDatabase()
    private void ShrinkDatabase()
    {
        ServerConnection cnnServer = new ServerConnection(System.Configuration.ConfigurationManager.AppSettings["DataCentreHost"], "sa", "A8f5h7p0");
        Server myServer = new Server(cnnServer);
        Database LapbaseDatabase = myServer.Databases[System.Configuration.ConfigurationManager.AppSettings["LapbaseDatabase"]];

        LapbaseDatabase.Shrink(100, ShrinkMethod.TruncateOnly);
        LapbaseDatabase.TruncateLog();
        LapbaseDatabase.Refresh();
        cnnServer.Disconnect();
    }
    #endregion

    
}
