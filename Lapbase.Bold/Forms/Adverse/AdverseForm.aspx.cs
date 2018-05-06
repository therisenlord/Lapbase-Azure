using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Microsoft.Web.UI;

public partial class Forms_Adverse_AdverseForm : System.Web.UI.Page
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
                                            "Complication Form", 2, "Browse", "PatientCode",
                                            Request.Cookies["PatientID"].Value);

                    gClass.SaveActionLog(gClass.OrganizationCode,
                                                        Request.Cookies["UserPracticeCode"].Value,
                                                        Request.Url.Host,
                                                        System.Configuration.ConfigurationManager.AppSettings["ComplicationPage"].ToString(),
                                                        System.Configuration.ConfigurationManager.AppSettings["LogRead"].ToString(),
                                                        "Load " + System.Configuration.ConfigurationManager.AppSettings["ComplicationPage"].ToString() + " List ",
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

                    lblNotes_com.Text = "Notes/Treatment";
                    if (Request.Cookies["SubmitData"].Value.IndexOf("acs") < 0)
                    {
                        lblNotes_com.Text = "Notes";
                        tblPostopACS.Style["display"] = "none";
                        trAdmissionACS.Style["display"] = "none";
                        trDischargeACS.Style["display"] = "none";
                        trPerformACS.Style["display"] = "none";
                        trReasonACS.Style["display"] = "none";
                    }

                    if (Request.Cookies["SubmitData"].Value.IndexOf("bold") >= 0)
                    {
                        LoadBoldList();
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
        CheckRequestQueryString();
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
    
    #region private void RegisterClientScriptBlock()
    private void RegisterClientScriptBlock()
    {
        txtHApplicationURL.Value = Request.Url.Scheme + "://" + Request.Url.Host + Request.ApplicationPath + "/"; 
        Page.Culture = Request.Cookies["CultureInfo"].Value;

        System.Globalization.CultureInfo myCI = new System.Globalization.CultureInfo(Request.Cookies["CultureInfo"].Value, false);
        lblDateFormat.Text = myCI.DateTimeFormat.ShortDatePattern.ToLower();
        txtDate_com.toolTip = myCI.DateTimeFormat.ShortDatePattern;

        bodyComplication.Attributes.Add("onload", "javascript:InitializePage();");
        chkReAdmitted_com.Attributes.Add("onclick", "javascript:chkReOperation_com_OnClick();");
        chkReOperation_com.Attributes.Add("onclick", "javascript:chkReOperation_com_OnClick();");

        txtHCurrentDate.Value = DateTime.Now.ToShortDateString();


        SqlCommand cmdSelect = new SqlCommand();
        DataSet dsOpVisit = new DataSet();
        if (Request.Cookies["SubmitData"].Value.IndexOf("bold") >= 0)
        {
            gClass.MakeStoreProcedureName(ref cmdSelect, "sp_SRCHospitalVisitGet", false);
            cmdSelect.Parameters.Add("@vintOrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
            cmdSelect.Parameters.Add("@vintPatientId", System.Data.SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
            dsOpVisit = gClass.FetchData(cmdSelect, "tblOpVisit");


            if (dsOpVisit.Tables.Count > 0 && (dsOpVisit.Tables[0].Rows.Count > 0))
            {
                txtHOperationDischargeDate.Value = gClass.TruncateDate(dsOpVisit.Tables[0].Rows[0]["DischargeDate"].ToString().Trim(), Request.Cookies["CultureInfo"].Value, 1);
            }
        }
    }
    #endregion

    #region private void CheckRequestQueryString()
    private void CheckRequestQueryString()
    {
        if (Request.QueryString.Count > 0)
        {
            Response.Clear();
            switch (Request.QueryString["QSN"].ToString().ToUpper()) //QSN
            {
                case "LOADCOMPLICATIONHISTORY":
                    try
                    {
                        LoadComplicationHistory();
                    }
                    catch (Exception err)
                    {
                        Response.Write(err.ToString());
                    }
                    break;
            }
            Response.End();
        }
    }
    #endregion

    #region private string LoadComplicationHistory()
    private void LoadComplicationHistory()
    {
        SqlCommand cmdSelect = new SqlCommand();
        DataSet dsComorbidity = new DataSet();

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_ConsultFU1_Complications_LoadData", true);
        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSelect.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
        cmdSelect.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt64(Request.Cookies["PatientID"].Value);
        dsComorbidity = gClass.FetchData(cmdSelect, "tblComplications");

        DataColumn dcTemp = new DataColumn();
        dcTemp.DataType = Type.GetType("System.String");
        dcTemp.DefaultValue = 0;
        dcTemp.ColumnName = "str_ComplicationDate";
        dcTemp.Caption = "str_ComplicationDate";
        dsComorbidity.Tables[0].Columns.Add(dcTemp);
        
        dcTemp = new DataColumn();
        dcTemp.DataType = Type.GetType("System.String");
        dcTemp.DefaultValue = 0;
        dcTemp.ColumnName = "str_AdmitDate";
        dcTemp.Caption = "str_AdmitDate";
        dsComorbidity.Tables[0].Columns.Add(dcTemp);

        dcTemp = new DataColumn();
        dcTemp.DataType = Type.GetType("System.String");
        dcTemp.DefaultValue = 0;
        dcTemp.ColumnName = "str_DischargeDate";
        dcTemp.Caption = "str_DischargeDate";
        dsComorbidity.Tables[0].Columns.Add(dcTemp);

        dcTemp = new DataColumn();
        dcTemp.DataType = Type.GetType("System.String");
        dcTemp.DefaultValue = 0;
        dcTemp.ColumnName = "str_PerformDate";
        dcTemp.Caption = "str_PerformDate";
        dsComorbidity.Tables[0].Columns.Add(dcTemp);

        dcTemp = new DataColumn();
        dcTemp.DataType = Type.GetType("System.String");
        dcTemp.DefaultValue = 0;
        dcTemp.ColumnName = "DateCreatedFormated";
        dcTemp.Caption = "DateCreatedFormated";
        dsComorbidity.Tables[0].Columns.Add(dcTemp);

        dcTemp = new DataColumn();
        dcTemp.DataType = Type.GetType("System.String");
        dcTemp.DefaultValue = 0;
        dcTemp.ColumnName = "str_OperationDischargeDate";
        dcTemp.Caption = "str_OperationDischargeDate";
        dsComorbidity.Tables[0].Columns.Add(dcTemp);

        for (int Idx = 0; Idx < dsComorbidity.Tables[0].Rows.Count; Idx++)
        {
            dsComorbidity.Tables[0].Rows[Idx]["str_ComplicationDate"] = gClass.TruncateDate(dsComorbidity.Tables[0].Rows[Idx]["ComplicationDate"].ToString().Trim(), Request.Cookies["CultureInfo"].Value, 1);
            dsComorbidity.Tables[0].Rows[Idx]["str_AdmitDate"] = gClass.TruncateDate(dsComorbidity.Tables[0].Rows[Idx]["AdmitDate"].ToString().Trim(), Request.Cookies["CultureInfo"].Value, 1);
            dsComorbidity.Tables[0].Rows[Idx]["str_DischargeDate"] = gClass.TruncateDate(dsComorbidity.Tables[0].Rows[Idx]["DischargeDate"].ToString().Trim(), Request.Cookies["CultureInfo"].Value, 1);
            dsComorbidity.Tables[0].Rows[Idx]["str_PerformDate"] = gClass.TruncateDate(dsComorbidity.Tables[0].Rows[Idx]["PerformDate"].ToString().Trim(), Request.Cookies["CultureInfo"].Value, 1);
            dsComorbidity.Tables[0].Rows[Idx]["DateCreatedFormated"] = gClass.TruncateDate(dsComorbidity.Tables[0].Rows[Idx]["DateCreated"].ToString().Trim(), Request.Cookies["CultureInfo"].Value, 1);
        }

        dsComorbidity.Tables[0].AcceptChanges();
        Response.Write(gClass.ShowSchema(dsComorbidity, Server.MapPath(".") + @"/ComplicationXSLTFile.xsl"));
        dsComorbidity.Dispose();
    }
    #endregion

    #region private void RemoveFirstItemFromDropDownList()
    private void RemoveFirstItemFromDropDownList()
    {
        if (listSurgery.Items.Count == 0)
            FillBoldList(cmbAdverseSurgery, listSurgery);
    }
    #endregion

    #region private void FillSelectedLists(HtmlSelect listOrigin, HtmlSelect listSelected, String strListValue)
    private void FillSelectedLists(UserControl_SystemCodeWUCtrl listOrigin, HtmlSelect listSelected, String strListValue)
    {
        System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(";");
        String[] strItemCollection = regex.Split(strListValue);
        System.Collections.Generic.List<String[,]> strOptionCollection = new System.Collections.Generic.List<string[,]>();
        String strScript = String.Empty;

        listSelected.Items.Clear();
        listOrigin.FetchSystemCodeListData();
        foreach (String strItem in strItemCollection)
            foreach (ListItem item in listOrigin.Items)
                if ((strItem.Length > 0) && item.Value.Equals(strItem))
                    strOptionCollection.Add(new String[,] { { item.Value, item.Text } });

        strScript = "var objList = document.forms[0]." + listSelected.ClientID + ", option;";
        strScript += "objList.length = 0;";
        foreach (String[,] tempString in strOptionCollection)
        {
            strScript += "option = document.createElement('OPTION');";
            strScript += "option.value = '" + tempString[0, 0] + "';";
            strScript += "option.text = '" + tempString[0, 1] + "';";
            strScript += "objList.options.add(option);";
        }

        ScriptManager.RegisterStartupScript(linkBtnLoad, linkBtnLoad.GetType(), Guid.NewGuid().ToString(), strScript, true);
    }
    #endregion
        
    #region protected void linkBtnLoadOperation_OnClick(object sender, EventArgs e)
    protected void linkBtnLoadOperation_OnClick(object sender, EventArgs e)
    {
        FillSelectedLists(cmbAdverseSurgery, listSurgery_Selected, txtHSurgery_Selected.Value);
    }
    #endregion

    #region protected void btnDeleteEvent_onserverclick(object sender, EventArgs e)
    /*
     * this function is to delete visit data
     */
    protected void btnDeleteEvent_onserverclick(object sender, EventArgs e)
    {
        if (txtHDelete.Value == "1")
        {
            Event_DeleteProc();
            //LoadAllVisits("");
        }
    }
    #endregion

    #region protected void Event_DeleteProc()
    protected void Event_DeleteProc()
    {

        SqlCommand cmdUpdate = new SqlCommand();
        string strScript = String.Empty;

        gClass.MakeStoreProcedureName(ref cmdUpdate, "sp_ConsultFU1_Complications_DeleteData", true);
        cmdUpdate.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdUpdate.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["PatientID"].Value);
        cmdUpdate.Parameters.Add("@ComplicationNum", SqlDbType.Int).Value = Convert.ToInt32(txtHComplicationID.Value);

        cmdUpdate.Parameters.Add("@DeletedByUser", SqlDbType.VarChar, 50).Value = Context.Request.Cookies["Logon_UserName"].Value;
        cmdUpdate.Parameters.Add("@DateDeleted", SqlDbType.DateTime).Value = Convert.ToDateTime(txtHCurrentDate.Value);

        try
        {
            gClass.ExecuteDMLCommand(cmdUpdate);
            gClass.SaveUserLogFile(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Cookies["Logon_UserName"].Value,
                                    Context.Request.Url.Host, "Complication Form", 2, "Delete Complication Data", "Complication Code", txtHComplicationID.Value);

            gClass.SaveActionLog(gClass.OrganizationCode,
                                            Request.Cookies["UserPracticeCode"].Value,
                                            Request.Url.Host,
                                            System.Configuration.ConfigurationManager.AppSettings["ComplicationPage"].ToString(),
                                            System.Configuration.ConfigurationManager.AppSettings["LogDelete"].ToString(),
                                            "Delete " + System.Configuration.ConfigurationManager.AppSettings["ComplicationPage"].ToString() +" Data ", 
                                            Context.Request.Cookies["PatientID"].Value,
                                            Convert.ToString(txtHComplicationID.Value));
            //strScript = "javascript:SetEvents();UpdateOtherFieldsBasedOnSelectedSurgeryType();ClearFields();controlBar_Buttons_OnClick(1);";
            // strScript += "linkBtnSave_CallBack(true);";
            strScript = "javascript:Complication_ClearFields;";
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                        Context.Request.Cookies["Logon_UserName"].Value, "Complication PID : " + Context.Request.Cookies["PatientID"].Value, "Data deleting complication - Event_DeleteProc function", err.ToString());
            //strScript = "javascript:linkBtnSave_CallBack(false);";
        }

        ScriptManager.RegisterStartupScript(btnDeleteComplication, btnDeleteComplication.GetType(), Guid.NewGuid().ToString(), strScript, true);
        return;
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
