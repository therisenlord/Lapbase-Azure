using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Microsoft.Web.UI;

public partial class Forms_Biochemistry_BiochemistryForm : System.Web.UI.Page
{
    GlobalClass gClass = new GlobalClass();

    #region protected void Page_Load(object sender, EventArgs e)
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "no-cache");
        Response.Expires = -1;
        try
        {
            gClass.LanguageCode = Request.Cookies["LanguageCode"].Value;
            gClass.OrganizationCode = Request.Cookies["OrganizationCode"].Value;
            txtHCulture.Value = Request.Cookies["CultureInfo"].Value;
            if (gClass.IsUserLogoned(Session.SessionID, Request.Cookies["UserPracticeCode"].Value, Request.Url.Host))
            {
                RegisterClientScriptBlock();
                if (!IsPostBack)
                {
                    gClass.SaveUserLogFile(Request.Cookies["UserPracticeCode"].Value,
                                            Request.Cookies["Logon_UserName"].Value,
                                            Request.Url.Host,
                                            "Biochemistry Form", 2, "Browse", "PatientCode",
                                            Request.Cookies["PatientID"].Value);

                    gClass.SaveActionLog(gClass.OrganizationCode,
                                                        Request.Cookies["UserPracticeCode"].Value,
                                                        Request.Url.Host,
                                                        System.Configuration.ConfigurationManager.AppSettings["BiochemistryPage"].ToString(),
                                                        System.Configuration.ConfigurationManager.AppSettings["LogRead"].ToString(),
                                                        "Load " + System.Configuration.ConfigurationManager.AppSettings["BiochemistryPage"].ToString() + " List ",
                                                        Request.Cookies["PatientID"].Value,
                                                        "");

                    txtHPermissionLevel.Value = Request.Cookies["PermissionLevel"].Value;
                    txtHDataClamp.Value = Request.Cookies["DataClamp"].Value;

                    if (Request.Cookies["SubmitData"].Value.IndexOf("submitbold") >= 0 || Request.Cookies["PermissionLevel"].Value == "1o" || Request.Cookies["PermissionLevel"].Value == "2t" || Request.Cookies["PermissionLevel"].Value == "3f" || Request.Cookies["PermissionLevel"].Value == "4s")
                        divBtnDelete.Style["display"] = "none";

                    if (Request.Cookies["PermissionLevel"].Value == "1o")
                    {
                        btnColl.Style["display"] = "none";
                    }
                }
            }
            else { gClass.ReturnToLoginPage(Request.Url.Host, Request.Cookies["LanguageCode"].Value, Response); return; }
        }
        catch (Exception err)
        {
            string strLanguageCode;
            try
            {
                strLanguageCode = Request.Cookies["LanguageCode"].Value;
                gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Complication Form", "Loading Complication List (Page_Load function)", err.ToString());
            }
            catch { strLanguageCode = "en-US"; }
            gClass.ReturnToLoginPage(Request.Url.Host, strLanguageCode, Response);
        }
        LoadBiochemistryList();
        LoadPatientBiochemistryHistory();
        return;
    }
    #endregion

    #region private void RegisterClientScriptBlock()
    private void RegisterClientScriptBlock()
    {
        txtHApplicationURL.Value = Request.Url.Scheme + "://" + Request.Url.Host + Request.ApplicationPath + "/";
        Page.Culture = Request.Cookies["CultureInfo"].Value;

        System.Globalization.CultureInfo myCI = new System.Globalization.CultureInfo(Request.Cookies["CultureInfo"].Value, false);
        lblDateFormat.Text = myCI.DateTimeFormat.ShortDatePattern.ToLower();
        txtDate_com.toolTip = myCI.DateTimeFormat.ShortDatePattern;

        //bodyComplication.Attributes.Add("onload", "javascript:InitializePage();");

        txtHCurrentDate.Value = DateTime.Now.ToShortDateString();
    }
    #endregion

    #region private string LoadBiochemistryList()
    private void LoadBiochemistryList()
    {
        SqlCommand cmdSelect = new SqlCommand();
        SqlCommand cmdSelectOrganization = new SqlCommand();
        DataSet dsBiochemistryList = new DataSet();
        DataSet dsOrganizationBiochemistry = new DataSet();
        String tempType = "";
        String tempCode = "";
        String tempChoiceCode = "";
        String tempChoiceDescription = "";
        String tempDescription = "";
        String tempGroup = "";
        String tempPrevGroup = "";
        String tempRow = "";
        String tempPrevCode = "";
        Int32 tempCount = 0;
        Int32 blankCol = 0;
        String biochemistryList = "";
        String biochemistryChoiceList = "";
        String organizationBiochemistry = "";
        Boolean displayAll = true;

        int Xr = 0;

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_Biochemistry_LoadData", true);
        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSelect.Parameters.Add("@Choice", SqlDbType.Int).Value = 1;

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

                //loop for biochemistry with input text
                for (int Xh = 0; Xh < dsBiochemistryList.Tables[0].Rows.Count; Xh++)
                {
                    tempRow = "";
                    tempType = dsBiochemistryList.Tables[0].Rows[Xh]["Type"].ToString();
                    tempCode = dsBiochemistryList.Tables[0].Rows[Xh]["Code"].ToString();
                    tempDescription = dsBiochemistryList.Tables[0].Rows[Xh]["Description"].ToString();
                    tempGroup = dsBiochemistryList.Tables[0].Rows[Xh]["BiochemistryGroup"].ToString();

                    if (Xh == 0)
                        tempPrevGroup = tempGroup;

                    if (tempGroup != tempPrevGroup)
                    {
                        //print new line

                        blankCol = 3 - tempCount;
                        for (int Xi = 0; Xi < blankCol; Xi++)
                        {
                            biochemistryList += "<td colspan =" + blankCol * 2 + ">&nbsp;</td><td></td>";
                        }
                        biochemistryList += "<tr><td colspan=6>&nbsp;</td></tr>";

                        tempCount = 0;
                        Xr = 0;
                    }

                    if (tempGroup == tempPrevGroup || Xr == 0)
                    {
                        tempCount++;
                        if (displayAll == true || (ContainsWord(tempCode, organizationBiochemistry, '_')))
                        {
                            if (Xr % 3 == 0 && Xr != 0)
                            {
                                tempCount = 0;
                                tempRow = "</tr><tr>";
                            }
                            else if (Xr % 3 == 0)
                                tempRow = "<tr>";

                            tempRow += "<td style='width:140px'>" + tempDescription + "&nbsp;&nbsp;</td><td><input style='width:50px' runat='server' type='text' id='" + tempCode + "'/></td>";

                            biochemistryList += tempRow;

                            Xr++;
                        }
                    }
                    tempPrevGroup = tempGroup;
                }
                biochemistryList += "</tr>";
                biochemistryList += "</table>";
                tblBiochemistryList.InnerHtml = biochemistryList;


                biochemistryChoiceList = "<table width='100%'>";
                Xr = 0;
                tempRow = "";
                //loop for biochemistry with combobox
                for (int Xh = 0; Xh < dsBiochemistryList.Tables[0].Rows.Count; Xh++)
                {
                    tempType = dsBiochemistryList.Tables[0].Rows[Xh]["Type"].ToString();
                    if (tempType.ToUpper() == "C")
                    {
                        tempCode = dsBiochemistryList.Tables[0].Rows[Xh]["Code"].ToString();
                        tempDescription = dsBiochemistryList.Tables[0].Rows[Xh]["Description"].ToString();
                        tempChoiceCode = dsBiochemistryList.Tables[0].Rows[Xh]["ChoiceCode"].ToString();
                        tempChoiceDescription = dsBiochemistryList.Tables[0].Rows[Xh]["ChoiceDescription"].ToString();

                        if (displayAll == true || (ContainsWord(tempCode, organizationBiochemistry, '_')))
                        {
                            /*
                            if (Xr % 3 == 0 && Xr != 0)
                                tempRow += "</tr><tr>";
                            else if (Xr % 3 == 0)
                                tempRow += "<tr>";
                            */

                            if (tempPrevCode != tempCode && tempPrevCode != "")
                            {
                                //print closing option and closing td
                                tempRow += "</select></td></tr>";
                            }
                            if (tempPrevCode != tempCode || tempPrevCode == "")
                            {
                                //print description, openning td and openning option
                                tempRow += "<tr><td style='width:140px'>" + tempDescription + "&nbsp;&nbsp;</td>";
                                tempRow += "<td><select id='"+ tempCode +"'>";

                                Xr++;
                            }
                            tempRow += "<option value='" + tempChoiceCode + "'>" + tempChoiceDescription + "</option>";

                            tempPrevCode = tempCode;
                        }
                    }
                }

                if (tempPrevCode != tempCode)
                {
                    //print closing option and closing td
                    tempRow += "</select></td></tr>";
                }
                biochemistryChoiceList += tempRow;
                biochemistryChoiceList += "</table>";
            }
            tblBiochemistryChoiceList.InnerHtml = biochemistryChoiceList;

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

    #region protected void linkBtnLoadBiochemistry_OnClick(object sender, EventArgs e)
    protected void linkBtnLoadBiochemistry_OnClick(object sender, EventArgs e)
    {
        //FillSelectedLists(cmbAdverseSurgery, listSurgery_Selected, txtHSurgery_Selected.Value);
    }
    #endregion

    #region protected void linkBtnSave_OnClick(object sender, EventArgs e)
    protected void linkBtnSave_OnClick(object sender, EventArgs e)
    {
        String strScript = String.Empty;
        String createdDate = txtHDateCreated.Value;
        String currentDate = txtHCurrentDate.Value;

        //if dataclamp is activated, permission lvl 2 or 3
        //check for created date for this patient
        Boolean allowSave = true;
        Boolean blnInsert;
        Int64 intBiochemistryNum = 0;
        DataSet dsBiochemistry = new DataSet();
        SqlCommand cmdSelect = new SqlCommand();

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
            try
            {
                SqlCommand cmdSave = new SqlCommand();
                gClass.MakeStoreProcedureName(ref cmdSave, "sp_PatientBiochemistry_SaveData", true);

                cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                cmdSave.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
                cmdSave.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt64(Request.Cookies["PatientID"].Value);
                cmdSave.Parameters.Add("@BiochemistryNum", SqlDbType.Int).Value = Convert.ToInt64(txtHPatientBiochemistryID.Value);
                cmdSave.Parameters.Add("@BiochemistryValue", SqlDbType.VarChar,2000).Value = txtHPatientBiochemistryValue.Value;
                       
                cmdSave.Parameters.Add("@BiochemistryDate", SqlDbType.DateTime);
                try { cmdSave.Parameters["@BiochemistryDate"].Value = Convert.ToDateTime(txtDate_com.Text); }
                catch { cmdSave.Parameters["@BiochemistryDate"].Value = DBNull.Value; }

                cmdSave.Parameters.Add("@User", SqlDbType.VarChar, 50).Value = Context.Request.Cookies["Logon_UserName"].Value;
                cmdSave.Parameters.Add("@Date", SqlDbType.DateTime).Value = Convert.ToDateTime(txtHCurrentDate.Value);

                blnInsert = Convert.ToInt64(txtHPatientBiochemistryID.Value) == 0 ? true : false;

                if (!blnInsert)
                {
                    //insert log
                    //before updating data
                    SqlCommand cmdSaveLog = new SqlCommand();
                    gClass.MakeStoreProcedureName(ref cmdSaveLog, "sp_PatientBiochemistryLog_InsertData", true);
                    cmdSaveLog.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                    cmdSaveLog.Parameters.Add("@BiochemistryNum", SqlDbType.Int).Value = Convert.ToInt64(txtHPatientBiochemistryID.Value);
                    cmdSaveLog.Parameters.Add("@LogUserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
                    cmdSaveLog.Parameters.Add("@LogDateTime", SqlDbType.DateTime).Value = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
                    gClass.ExecuteDMLCommand(cmdSaveLog);
                }

                gClass.ExecuteDMLCommand(cmdSave);
                gClass.SaveUserLogFile(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Cookies["Logon_UserName"].Value,
                                        Context.Request.Url.Host, "Biochemistry Form", 2, "Save Patient Biochemistry Data", "PID: ", Context.Request.Cookies["PatientID"].Value);



                if (blnInsert)
                {
                    gClass.MakeStoreProcedureName(ref cmdSelect, "sp_PatientBiochemistry_GetLastBiochemistryID", true);
                    cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                    cmdSelect.Parameters.Add("@PatientId", SqlDbType.Int).Value = Convert.ToInt32(Context.Request.Cookies["PatientID"].Value);
                    dsBiochemistry = gClass.FetchData(cmdSelect, "tblPatientBiochemistry");
                    if (dsBiochemistry.Tables[0].Rows.Count > 0)
                    {
                        intBiochemistryNum = Convert.ToInt64(dsBiochemistry.Tables[0].Rows[0]["BiochemistryNum"].ToString());
                    }
                }
                else
                    intBiochemistryNum = Convert.ToInt64(txtHPatientBiochemistryID.Value);

                gClass.SaveActionLog(gClass.OrganizationCode,
                                            Context.Request.Cookies["UserPracticeCode"].Value,
                                            Context.Request.Url.Host,
                                            System.Configuration.ConfigurationManager.AppSettings["BiochemistryPage"].ToString(),
                                            ((blnInsert) ? System.Configuration.ConfigurationManager.AppSettings["LogCreate"].ToString() : System.Configuration.ConfigurationManager.AppSettings["LogUpdate"].ToString()),
                                            "Save " + System.Configuration.ConfigurationManager.AppSettings["BiochemistryPage"].ToString() + " Data ",
                                            Context.Request.Cookies["PatientID"].Value,
                                            Convert.ToString(intBiochemistryNum));
                blnInsert = false;

                strScript = "javascript:linkBtnSave_CallBack(true);";
                //LoadPatientBiochemistryHistory();
            }
            catch (Exception err)
            {
                gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                            Context.Request.Cookies["Logon_UserName"].Value, "PID : " + Context.Request.Cookies["PatientID"].Value, "Data saving Patient Biochemistry - SavePatientBiochemistry function", err.ToString());
                strScript = "javascript:linkBtnSave_CallBack(false);";

            }
        }
        else
        {
            strScript = "javascript:linkBtnSave_CallBack('load');";
        }
        ScriptManager.RegisterStartupScript(linkBtnSave, linkBtnSave.GetType(), Guid.NewGuid().ToString(), strScript, true);
    }
    #endregion

    #region private string LoadPatientBiochemistryHistory()
    private void LoadPatientBiochemistryHistory()
    {
        SqlCommand cmdSelect = new SqlCommand();
        DataSet dsPatientBiochemistry = new DataSet();

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_PatientBiochemistry_LoadData", true);
        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSelect.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt64(Request.Cookies["PatientID"].Value);
        dsPatientBiochemistry = gClass.FetchData(cmdSelect, "tblPatientBiochemistry");

        DataColumn dcTemp = new DataColumn();
        dcTemp.DataType = Type.GetType("System.String");
        dcTemp.DefaultValue = 0;
        dcTemp.ColumnName = "str_BiochemistryDate";
        dcTemp.Caption = "str_BiochemistryDate";
        dsPatientBiochemistry.Tables[0].Columns.Add(dcTemp);

        dcTemp = new DataColumn();
        dcTemp.DataType = Type.GetType("System.String");
        dcTemp.DefaultValue = 0;
        dcTemp.ColumnName = "DateCreatedFormated";
        dcTemp.Caption = "DateCreatedFormated";
        dsPatientBiochemistry.Tables[0].Columns.Add(dcTemp);

        for (int Idx = 0; Idx < dsPatientBiochemistry.Tables[0].Rows.Count; Idx++)
        {
            dsPatientBiochemistry.Tables[0].Rows[Idx]["str_BiochemistryDate"] = gClass.TruncateDate(dsPatientBiochemistry.Tables[0].Rows[Idx]["BiochemistryDate"].ToString().Trim(), Request.Cookies["CultureInfo"].Value, 1);
            dsPatientBiochemistry.Tables[0].Rows[Idx]["DateCreatedFormated"] = gClass.TruncateDate(dsPatientBiochemistry.Tables[0].Rows[Idx]["DateCreated"].ToString().Trim(), Request.Cookies["CultureInfo"].Value, 1);
        }

        dsPatientBiochemistry.Tables[0].AcceptChanges();
        div_PatientBiochemistryList.InnerHtml = gClass.ShowSchema(dsPatientBiochemistry, Server.MapPath(".") + @"/BiochemistryXSLTFile.xsl");
        dsPatientBiochemistry.Dispose();
    }
    #endregion
    
    #region protected string linkBtnDelete_OnClick(object sender, EventArgs e)
    protected void linkBtnDelete_OnClick(object sender, EventArgs e)
    {
        SqlCommand cmdUpdate = new SqlCommand();
        string strScript = String.Empty;
        
        gClass.MakeStoreProcedureName(ref cmdUpdate, "sp_PatientBiochemistry_DeleteData", true);
        cmdUpdate.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdUpdate.Parameters.Add("@BiochemistryNum", SqlDbType.Int).Value = Convert.ToInt64(txtHPatientBiochemistryID.Value);

        cmdUpdate.Parameters.Add("@DeletedByUser", SqlDbType.VarChar, 50).Value = Context.Request.Cookies["Logon_UserName"].Value;
        cmdUpdate.Parameters.Add("@DateDeleted", SqlDbType.DateTime).Value = Convert.ToDateTime(txtHCurrentDate.Value);

        try
        {
            gClass.ExecuteDMLCommand(cmdUpdate);
            gClass.SaveUserLogFile(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Cookies["Logon_UserName"].Value,
                                    Context.Request.Url.Host, "Biochemistry Form", 2, "Delete Patient Biochemistry Data", "Biochemistry ID", txtHPatientBiochemistryID.Value);

            gClass.SaveActionLog(gClass.OrganizationCode,
                                            Context.Request.Cookies["UserPracticeCode"].Value,
                                            Context.Request.Url.Host,
                                            System.Configuration.ConfigurationManager.AppSettings["BiochemistryPage"].ToString(),
                                            System.Configuration.ConfigurationManager.AppSettings["LogDelete"].ToString(),
                                            "Delete " + System.Configuration.ConfigurationManager.AppSettings["BiochemistryPage"].ToString() + " Data ",
                                            Context.Request.Cookies["PatientID"].Value,
                                            Convert.ToString(txtHPatientBiochemistryID.Value));

            strScript = "javascript:linkBtnSave_CallBack('delete');";
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                        Context.Request.Cookies["Logon_UserName"].Value, "Biochemistry ID : " + txtHPatientBiochemistryID.Value, "Data deleting PatientBiochemistry", err.ToString());
            strScript = "javascript:linkBtnSave_CallBack(false);";
        }

        ScriptManager.RegisterStartupScript(linkBtnSave, linkBtnSave.GetType(), Guid.NewGuid().ToString(), strScript, true);
        return;
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
}
