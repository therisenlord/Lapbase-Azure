using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.Web.UI;
public partial class Reports_GroupReport_BSRForm : Lapbase.Business.BasePage
{
    GlobalClass gClass = new GlobalClass();
    int intNumberPerSheet = 10, NumberOfPages = 0, cntPatient = 0;

    protected override void OnLoad(EventArgs e)
    {
        gClass.LanguageCode = base.LanguageCode;
        gClass.OrganizationCode = Request.Cookies["OrganizationCode"].Value;
        txtHCulture.Value = base.CultureInfo;
        txtHApplicationURL.Value = Request.Url.Scheme + "://" + Request.Url.Host + Request.ApplicationPath + "/";
        if (!Page.IsPostBack)
        {
            IntialiseFormSetting();
            //gClass.SetListItems(frmPatientsList.ID, cmbSortBy, 1);
            LoadLanguageCharactors();
            PreLoadPatientsList();
        }
        base.OnLoad(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Culture = Request.Cookies["CultureInfo"].Value;
        bodyGroupReport.Attributes.Add("onload", "javascript:InitialPage();");

        if (!gClass.IsUserLogoned(Session.SessionID, Request.Cookies["UserPracticeCode"].Value, Request.Url.Host))
            gClass.ReturnToLoginPage(Request.Url.Host, Request.Cookies["LanguageCode"].Value, Response);

        //Response.CacheControl = "no-cache";
        //Response.AddHeader("Pragma", "no-cache");
        Response.Expires = -1;

        bool LoadFlag = true;

        /*IMPORTANT : because of using ajax function (javascript and asp.net), the browser MUST have no-cache setting*/
        if (!Page.IsPostBack)
        {
            gClass.SaveUserLogFile(base.UserPracticeCode.ToString(), Request.Cookies["Logon_UserName"].Value, base.DomainURL, "Patient List Form", 2, "Browse", "", "");
        }
    }

    #region private void IntialiseFormSetting
    /*this function is to initialize some controls by default values*/
    private void IntialiseFormSetting()
    {
        bodyGroupReport.Style.Add("Direction", Request.Cookies["Direction"].Value);
        txtHShowType.Value = "ShowAll";
        txtHSortOrder.Value = "ASC";
        txtHPageNo.Value = "1";
        txtHPageQty.Value = "0";

        cmbSortBy.SelectedValue = "1";
        txtHSortOrder.Value = "ASC";
        /*
        cmbSortBy.SelectedValue = Request.Cookies["DefaultSort"].Value;
        if (Request.Cookies["DefaultSort"].Value == "3")
            txtHSortOrder.Value = "DESC";
        */
        return;
    }
    #endregion 

    /// <summary>
    /// this function is used to load all letters and characters of the selected language (merging XML document(of letter dataset) and XSL file)
    /// </summary>
    private void LoadLanguageCharactors()
    {
        divCharacters.InnerHtml = gClass.ShowSchema(gClass.LoadLanguageCharacters(), Server.MapPath(".") + @"\includes\CharactersXSLTFile.xsl");
    }

    #region protected void btnLoad_onlick(object sender, EventArgs e)
    ///<summary>
    /// this function is to load patients list and pagination, but it's called in different situation,
    /// because this page is an asp.net ajax form, so we need to define an EVENT for asp:updatepanel component, 
    /// if you have a look at script file of this page, PatientVisits.js, when user enters some values for patient name, surname and press SHOW ALL in letters table, 
    /// the patients list should be reloaded. because all these events are done at the client-side, we need to call a function at the server-side to reload the patients list, 
    /// so for all these client-side events, we use only one server-side function of btnLoad, this asp.net component is hidden and its onclick event is used to reload data.
    ///</summary>
    protected void btnLoad_onlick(object sender, EventArgs e)
    {
        PreLoadPatientsList();
    }
    #endregion

    #region private void PreLoadPatientsList()
    /*this function is to load the patients list*/
    private void PreLoadPatientsList()
    {
        string strScript = "";
        try
        {
            LoadPatientsList();
            strScript = "HideDivMessage();";
        }
        catch (Exception err)
        {
            strScript = "HideDivMessage();document.getElementById('divErrorMessage').style.display = 'block';SetInnerText(document.getElementById('pErrorMessage'), 'Error in loading patients list...');";
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Patient List Form", "btnLoad_onlick function", err.ToString());
        }
        ScriptManager.RegisterStartupScript(up_PatientsList, btnLoad.GetType(), Guid.NewGuid().ToString(), strScript, true);
    }
    #endregion

    #region void LoadPatientsList()
    ///<summary>
    /// this is the main function of loading patients list, first we find out the number of patients for doing pagination, then we load all patients data.
    /// notice: the patients list is created by using XML document (from dataset) and XSL file, the result of this merging (string) is used as InnerHTML of div_patientslist
    ///</summary>
    private void LoadPatientsList()
    {
        String strSql = String.Empty
                , strTemp = String.Empty
                , strOrderBy = String.Empty;

        String radioList = "";

        System.Data.DataSet dsPatient = new System.Data.DataSet();
        System.Data.SqlClient.SqlCommand cmdSql = new System.Data.SqlClient.SqlCommand();

        cmdSql.CommandType = System.Data.CommandType.StoredProcedure;
        gClass.MakeStoreProcedureName(ref cmdSql, "sp_PatientsList_LoadPatientsList", true);

        cmdSql.Parameters.Add("@vstrShowType", System.Data.SqlDbType.VarChar, 50).Value = txtHShowType.Value.ToUpper();
        cmdSql.Parameters.Add("@vstrLetter", System.Data.SqlDbType.VarChar, 5).Value = String.Format("{0}%", txtHText.Value);
        strTemp = txtSurName.Text.Trim().Replace("'", "`");
        cmdSql.Parameters.Add("@vstrSurName", System.Data.SqlDbType.VarChar, 50).Value = strTemp.Equals(String.Empty) ? String.Empty : String.Format("{0}%", strTemp);
        strTemp = txtName.Text.Trim().Replace("'", "`");
        cmdSql.Parameters.Add("@vstrFirstName", System.Data.SqlDbType.VarChar, 50).Value = strTemp.Equals(String.Empty) ? String.Empty : String.Format("{0}%", strTemp);
        cmdSql.Parameters.Add("@vintOrganizationCode", System.Data.SqlDbType.VarChar, 10).Value = gClass.OrganizationCode;
        cmdSql.Parameters.Add("@vboolLoadPageCount", System.Data.SqlDbType.Bit).Value = 1;

        cmdSql.Parameters.Add("@vintSortBy", System.Data.SqlDbType.Int).Value = 0;
        cmdSql.Parameters.Add("@vstrSortOrder", System.Data.SqlDbType.VarChar, 50).Value = String.Empty;
        cmdSql.Parameters.Add("@vintPageNo", System.Data.SqlDbType.Int).Value = 0;
        cmdSql.Parameters.Add("@vintNumberPerSheet", System.Data.SqlDbType.Int).Value = intNumberPerSheet;
        cmdSql.Parameters.Add("@vintDoctorID", System.Data.SqlDbType.VarChar, 10).Value = ((Request.Cookies["PermissionLevel"].Value == "1o" || Request.Cookies["PermissionLevel"].Value == "2t" || Request.Cookies["PermissionLevel"].Value == "3f") && Request.Cookies["SurgeonID"].Value != "0") ? Request.Cookies["SurgeonID"].Value : String.Empty;


        gClass.FetchData(cmdSql, "tblPatientQty");
        cntPatient = Convert.ToInt32(gClass.dsGlobal.Tables["tblPatientQty"].Rows[0][0].ToString());
        CreatePagesNo();

        //2) then we fetch all patients
        strSql = "With rstPatientList as (";
        strSql += "SELECT  A.[Patient Id] as PatientID, dbo.udfGetPatientCustomId(A.OrganizationCode, A.[Patient Id]) as Patient_CustomId, A.Surname, A.Firstname, A.Street, A.Suburb, A.Birthdate, A.OrganizationCode, ";
        strSql += " C.LapBandDate, C.SurgeryType, A.[Date Last Visit] as DateLastVisit, C.SurgeryType_Description ";
        strSql += " , '' as tempBirthdate, '' as tempLapBandDate, '' as tempDateLastVisit,  dbo.fn_GetDoctorInitial(A.[Doctor Id], A.OrganizationCode) as DoctorInitial , ";
        strSql += "Row_Number() over ( ";

        strOrderBy = GetOrderBy();
        strSql += " " + strOrderBy;
        strSql += txtHSortOrder.Value + ", A.Firstname ) RowNumber  ";

        AddExteraParameter(ref strSql);
        strSql += String.Format(") Select PL.*, dbo.fn_GetBSForm(PL.PatientID, PL.OrganizationCode, 'Reg1Y1', BS.Reg1Y1, PL.LapbandDate) as Reg1Y1, dbo.fn_GetBSForm(PL.PatientID, PL.OrganizationCode, 'Reg2Y1', BS.Reg2Y1, PL.LapbandDate) as Reg2Y1, dbo.fn_GetBSForm(PL.PatientID, PL.OrganizationCode, 'Reg2Y2', BS.Reg2Y2, PL.LapbandDate) as Reg2Y2, dbo.fn_GetBSForm(PL.PatientID, PL.OrganizationCode, 'Reg2Y3', BS.Reg2Y3, PL.LapbandDate) as Reg2Y3, dbo.fn_GetBSForm(PL.PatientID, PL.OrganizationCode, 'Reg2Y4', BS.Reg2Y4, PL.LapbandDate) as Reg2Y4, dbo.fn_GetBSForm(PL.PatientID, PL.OrganizationCode, 'Reg2Y5', BS.Reg2Y5, PL.LapbandDate) as Reg2Y5, dbo.fn_GetBSForm(PL.PatientID, PL.OrganizationCode, 'Reg2Y6', BS.Reg2Y6, PL.LapbandDate) as Reg2Y6, dbo.fn_GetBSForm(PL.PatientID, PL.OrganizationCode, 'Reg2Y7', BS.Reg2Y7, PL.LapbandDate) as Reg2Y7, dbo.fn_GetBSForm(PL.PatientID, PL.OrganizationCode, 'Reg2Y8', BS.Reg2Y8, PL.LapbandDate) as Reg2Y8, dbo.[fn_GetBaselineOperationID](PL.OrganizationCode, PL.PatientID) as baselineOperationID from rstPatientList PL with (nowait, nolock) left join tblPatientBSForm BS with (nowait, nolock) ON (PL.PatientId = BS.[Patient Id] and PL.organizationcode = BS.organizationcode) where RowNumber between {0} and {1}", ((Convert.ToInt16(txtHPageNo.Value) - 1) * intNumberPerSheet + 1).ToString(), ((Convert.ToInt16(txtHPageNo.Value)) * intNumberPerSheet).ToString());

        strSql = strSql;
        cmdSql.Parameters.Clear();
        cmdSql.CommandType = System.Data.CommandType.Text;
        cmdSql.CommandText = strSql;
        gClass.FetchData(cmdSql, "tblPatients");
        dsPatient = gClass.dsGlobal;
        if (dsPatient.Tables.Count > 0)
        {
            Page.Culture = Request.Cookies["CultureInfo"].Value;
            for (int Idx = 0; Idx < dsPatient.Tables[0].Rows.Count; Idx++)
            {
                dsPatient.Tables[0].Rows[Idx]["Surname"] = dsPatient.Tables[0].Rows[Idx]["Surname"].ToString().Trim().Replace("`", "'");
                dsPatient.Tables[0].Rows[Idx]["Firstname"] = dsPatient.Tables[0].Rows[Idx]["Firstname"].ToString().Trim().Replace("`", "'");
                dsPatient.Tables[0].Rows[Idx]["Street"] = dsPatient.Tables[0].Rows[Idx]["Street"].ToString().Trim().Replace("`", "'");
                dsPatient.Tables[0].Rows[Idx]["Suburb"] = dsPatient.Tables[0].Rows[Idx]["Suburb"].ToString().Trim().Replace("`", "'");
            }
            dsPatient.Tables[0].AcceptChanges();
        }
        //reset hidden value
        txtHSubmitID.Value = "";

        div_PatientsList.InnerHtml = gClass.ShowSchema(dsPatient, Server.MapPath(".") + @"\includes\PatientsListBSRXSLTFile.xsl");
        return;
    }
    #endregion

    #region
    private String GetOrderBy()
    {
        String strOrderBy = String.Empty;

        switch (cmbSortBy.SelectedValue)
        {
            case "0":
                strOrderBy = " ORDER BY A.[Patient Id], A.Surname, A.Firstname ";
                break;
            case "1":
                strOrderBy = " ORDER BY  A.[Patient Id] ";
                break;
            case "2":
                strOrderBy = " ORDER BY A.Surname, A.Firstname ";
                break;
            case "3":
                strOrderBy = " ORDER BY C.LapBandDate ";
                break;
            case "4":
                strOrderBy = " ORDER BY A.Firstname, A.Surname ";
                break;
            default:
                strOrderBy = " ORDER BY A.[Patient Id], A.Surname, A.Firstname ";
                break;
        }

        return strOrderBy;
    }
    #endregion

    #region private void CreatePagesNo()
    ///<summary>
    /// this function and AddPageNavigator (the next function) are to do pagination of patients list, setting style and events for each page no and first, prev, next and last icons.
    ///</summary>
    private void CreatePagesNo()
    {
        int intPageCnt = 10;

        if (cntPatient % intNumberPerSheet == 0)
            NumberOfPages = cntPatient / intNumberPerSheet;
        else
            NumberOfPages = (int)(cntPatient / intNumberPerSheet) + 1;

        txtHPageQty.Value = NumberOfPages.ToString();
        if (NumberOfPages > 1)
        {
            int Xh = 0, Idx = 0;

            System.Web.UI.HtmlControls.HtmlGenericControl lblPageIntro = new System.Web.UI.HtmlControls.HtmlGenericControl("LABEL");
            System.Web.UI.HtmlControls.HtmlGenericControl lblPage = new System.Web.UI.HtmlControls.HtmlGenericControl("LABEL");
            System.Web.UI.HtmlControls.HtmlGenericControl lblBlank = new System.Web.UI.HtmlControls.HtmlGenericControl("LABEL");
            System.Web.UI.WebControls.TextBox txtGoTo = new System.Web.UI.WebControls.TextBox();

            lblPageIntro.InnerText = "Page : ";
            lblPageIntro.Attributes.Add("style", "font-size:8pt;font-weight:bold");
            div_PagesNo.Controls.Add(lblPageIntro);

            //goto page
            txtGoTo.Attributes.Add("value", txtHPageNo.Value);
            txtGoTo.Attributes.Add("id", "txtGoToPage");
            txtGoTo.Attributes.Add("style", "font-size:7.5pt");
            txtGoTo.Attributes.Add("size", "2");
            div_PagesNo.Controls.Add(txtGoTo);
            AddPageNavigator("G", true);

            lblBlank.InnerText = " ";
            div_PagesNo.Controls.Add(lblBlank);


            if (Convert.ToInt16(txtHPageNo.Value) > intPageCnt) Xh = Convert.ToInt16(txtHPageNo.Value) - intPageCnt + 1;
            else Xh = 1;

            if (NumberOfPages > intPageCnt)
            {
                AddPageNavigator("F", (Xh > 1) || (Convert.ToInt16(txtHPageNo.Value) > 1));
                AddPageNavigator("P", (Xh > 1) || (Convert.ToInt16(txtHPageNo.Value) > 1));
            }

            for (; (Xh <= NumberOfPages) && (++Idx <= intPageCnt); Xh++)
            {
                System.Web.UI.HtmlControls.HtmlAnchor aPage = new System.Web.UI.HtmlControls.HtmlAnchor();
                aPage.HRef = "#";
                if (txtHPageNo.Value.Equals(Xh.ToString()))
                {
                    aPage.InnerText = String.Format("[{0}]", Xh.ToString());
                    aPage.Attributes.Add("class", "selected");
                    aPage.Attributes.Add("style", "font-size:8pt;font-weight:bold");
                }
                else
                {
                    aPage.InnerText = Xh.ToString();
                    aPage.Attributes.Add("style", "font-size:8pt;font-weight:normal");
                }
                aPage.Attributes.Add("onclick", String.Format("javascript:LoadRecordsOfPage({0}, {1});", NumberOfPages, Xh.ToString()));
                div_PagesNo.Controls.Add(aPage);
            }

            if (NumberOfPages > intPageCnt)
            {
                AddPageNavigator("N", (Convert.ToInt16(txtHPageNo.Value) < Convert.ToInt16(txtHPageQty.Value)));
                AddPageNavigator("L", (Convert.ToInt16(txtHPageNo.Value) < Convert.ToInt16(txtHPageQty.Value)));
            }

            lblPage = new System.Web.UI.HtmlControls.HtmlGenericControl("LABEL");
            lblPage.InnerText = String.Format("   of {0} (Patients: {1})", txtHPageQty.Value, cntPatient.ToString());
            lblPage.Attributes.Add("style", "font-size:8pt;font-weight:bold");
            div_PagesNo.Controls.Add(lblPage);
        }
    }
    #endregion

    #region private void AddPageNavigator(string strNavigateCode)
    /*
     * this function and CreatePagesNo (the previous function) are to do pagination of patients list, setting style and events for each page no and first, prev, next and last icons.
     */
    private void AddPageNavigator(string strNavigateCode, bool ShowFlag)
    {
        if (!ShowFlag) return;
        System.Web.UI.HtmlControls.HtmlAnchor aPage = new System.Web.UI.HtmlControls.HtmlAnchor();
        switch (strNavigateCode)
        {
            case "F":
                aPage.InnerText = "<<";
                break;
            case "P":
                aPage.InnerText = "<";
                break;
            case "N":
                aPage.InnerText = ">";
                break;
            case "L":
                aPage.InnerText = ">>";
                break;
            case "G":
                aPage.InnerText = "Go";
                break;
        }
        aPage.Attributes.Add("class", "squareBtn");
        aPage.Attributes.Add("style", "font-size:7.5pt;font-weight:normal");
        aPage.HRef = "#";
        aPage.Attributes.Add("onclick", "javascript:PageNavigator('" + strNavigateCode + "')");
        div_PagesNo.Controls.Add(aPage);
    }
    #endregion

    #region private void AddExteraParameter(ref string strSql)
    ///<summary>
    /// this function is called by "LoadPatientsList" function to add some criteria and filtering conditions
    ///</summary>
    private void AddExteraParameter(ref string strSql)
    {
        bool whereFlag = false;

        strSql += "FROM ";
        strSql += "tblPatients A with (nowait,nolock) left join ";
        strSql += "(Select B.[Patient Id], B.UserPracticeCode, B.LapBandDate, B.SurgeryType, B.NextComorbidVisit, ";
        strSql += "dbo.fn_GetSystemDescription_Bold (B.SurgeryType, 'BST', 'BST') as SurgeryType_Description ";
        strSql += "from tblPatientWeightData B with(nowait,nolock) where organizationcode = " + gClass.OrganizationCode + ") C ";
        strSql += "ON ((A.[Patient Id] = C.[Patient Id])) ";

        switch (txtHShowType.Value.ToUpper())
        {
            case "LOADBYFIRSTLETTER":
                strSql += "where (A.Surname Like '" + txtHText.Value + "%') ";
                whereFlag = true;
                break;

            case "SHOWALL":
                if (txtSurName.Text.Trim().Length > 0)
                {
                    strSql += " where (A.Surname like '%" + txtSurName.Text.Trim().Replace("'", "`") + "%') ";
                    whereFlag = true;
                }

                if (txtName.Text.Trim().Length > 0)
                {
                    if (whereFlag)
                        strSql += " and (A.FirstName like '%" + txtName.Text.Replace("'", "`") + "%') ";
                    else
                    {
                        strSql += " where (A.FirstName like '%" + txtName.Text.Replace("'", "`") + "%') ";
                        whereFlag = true;
                    }
                }
                break;
        }

        strSql += (whereFlag ? " and " : " where ") + "(A.OrganizationCode = " + gClass.OrganizationCode + ") AND A.DateDeleted is null";
        strSql += ((Request.Cookies["PermissionLevel"].Value == "1o" || Request.Cookies["PermissionLevel"].Value == "2t" || Request.Cookies["PermissionLevel"].Value == "3f") && Request.Cookies["SurgeonID"].Value != "0") ? " AND (A.[Doctor ID] = " + Request.Cookies["SurgeonID"].Value + ")" : "";
    }
    #endregion

    #region private void divErrorMessageScriptBuilder(ref string strScript, string strErrorMessage)
    private void divErrorMessageScriptBuilder(ref string strScript, string strErrorMessage)
    {
        strScript = "SetEvents();";
        strScript += "HideDivMessage();";
        strScript += "document.getElementById('divErrorMessage').style.display = 'block';";
        strScript += String.Format("SetInnerText(document.getElementById('pErrorMessage'), '{0}...');", strErrorMessage);
        return;
    }
    #endregion 

    #region protected void btnSubmit_onserverclick(object sender, EventArgs e)
    protected void btnSubmit_onserverclick(object sender, EventArgs e)
    {
        try
        {
            //save to db
            System.Data.SqlClient.SqlCommand cmdUpdate = new System.Data.SqlClient.SqlCommand();

            string submitValue = "";
            String[] tempValue;
            String[] tempValueRecord;
            string patientid = "";
            string regYear = "";
            string sql = "";

            submitValue = txtHSubmitID.Value;
            submitValue = submitValue.Substring(0, submitValue.Length - 1);
            tempValue = submitValue.Split(',');

            foreach (string tempRecord in tempValue)
            {
                tempValueRecord = tempRecord.Split('-');

                patientid = tempValueRecord[0].ToString();
                regYear = tempValueRecord[1].ToString() + tempValueRecord[2].ToString();

                if (patientid != "" && regYear != "")
                {
                    cmdUpdate.Parameters.Clear();
                    gClass.MakeStoreProcedureName(ref cmdUpdate, "sp_PatientBSR_SaveData", true);
                    cmdUpdate.Parameters.Add("@OrganizationCode", System.Data.SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                    cmdUpdate.Parameters.Add("@PatientID", System.Data.SqlDbType.Int).Value = Convert.ToInt32(patientid);
                    cmdUpdate.Parameters.Add("@Reg", System.Data.SqlDbType.VarChar, 20).Value = regYear;
                    gClass.ExecuteDMLCommand(cmdUpdate);
                }
            }
            PreLoadPatientsList();
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "BSR Form", "Submit BSR Form", err.ToString());
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


}