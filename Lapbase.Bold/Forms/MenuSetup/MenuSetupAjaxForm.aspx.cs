using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Globalization;

public partial class Forms_MenuSetup_MenuSetupAjaxForm : System.Web.UI.Page
{
    GlobalClass gClass = new GlobalClass();

    #region protected void Page_Load(object sender, EventArgs e)
    protected void Page_Load(object sender, EventArgs e)
    {
        gClass.LanguageCode = Request.Cookies["LanguageCode"].Value;
        gClass.OrganizationCode = Request.Cookies["OrganizationCode"].Value;
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "no-cache");
        Response.Expires = -1;

        if (!gClass.IsUserLogoned(Session.SessionID, Request.Cookies["UserPracticeCode"].Value, Request.Url.Host)) return;
        switch(Request.QueryString["QSN"].ToUpper())
        {
            case "REFDOCTOR":
                FetchDoctorsData(true, Request.QueryString["SName"]);
                break;

            case "DOCTOR":
                FetchDoctorsData(false, "");
                break;

            case "LOADDOCTORDATA" :
                FetchDoctorData(Request.QueryString["DRID"], false);
                break;

            case "LOADREFDOCTORDATA" :
                FetchDoctorData(Request.QueryString["REFDRID"], true);
                break;

            case "TEMPLATE":
                FetchTemplateData();
                break;

            case "LOGO":
                FetchLogoData();
                break;

            case "ACTIONLOG":
                FetchActionLogData();
                break;

            case "LOADHOSPITAL" :
                FetchHospitalData();
                break;

            case "LOADREGION":
                FetchRegionData();
                break;

            case "LOADGROUP" :
                FetchGroupData();
                break;

            case "LOADCATEGORY":
                FetchCategoryData();
                break;

            case "LOADDELETEDPATIENT":
                FetchDeletedPatient();
                break;
        }
    }
    #endregion

    #region private void FetchDoctorsData(bool RefDoctorFlag)
    private void FetchDoctorsData(bool RefDoctorFlag, string strSurname)
    {
        SqlCommand cmdSelect = new SqlCommand();

        if (RefDoctorFlag)
        {
            gClass.MakeStoreProcedureName(ref cmdSelect, "sp_ReferredDoctor_LoadData", true);
            cmdSelect.Parameters.Add("@SurName", SqlDbType.VarChar, 50).Value = (strSurname.Trim().Length == 0) ? "" : ("%" + strSurname.Trim() + "%");
            cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
            cmdSelect.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
            cmdSelect.Parameters.Add("@vblnIsHide", System.Data.SqlDbType.Bit).Value = false;
        }
        else
        {
            gClass.MakeStoreProcedureName(ref cmdSelect, "sp_Doctors_LoadData", false);
            cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
            cmdSelect.Parameters.Add("@vblnIsHide", System.Data.SqlDbType.Bit).Value = false;
            //cmdSelect.Parameters.Add("@vblnIsSurgeon", System.Data.SqlDbType.Bit).Value = null;
        }

        Response.Clear();
        Response.Write(gClass.ShowSchema(gClass.FetchData(cmdSelect, RefDoctorFlag ? "tblRefDoctors" : "tblDoctors"), Server.MapPath(".") + (RefDoctorFlag ? @"\Includes\RefDoctorsXSLTFile.xsl" : @"\Includes\DoctorsXSLTFile.xsl")));
        Response.End();
    }
    #endregion

    #region private void FetchDoctorData(string strDrId, bool RefDoctorFlag)
    private void FetchDoctorData(string strDrId, bool RefDoctorFlag)
    {
        SqlCommand cmdSelect = new SqlCommand();
        
        gClass.MakeStoreProcedureName(ref cmdSelect, RefDoctorFlag ? "sp_RefDoctors_SelectRefDoctorByRefDoctorID" : "sp_Doctors_SelectDoctorByDoctorID", true);
        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        if (RefDoctorFlag)
            cmdSelect.Parameters.Add("@RefDrId", SqlDbType.VarChar, 10).Value = strDrId;
        else
            cmdSelect.Parameters.Add("@DoctorID", SqlDbType.Int).Value = Convert.ToInt32(strDrId);

        cmdSelect.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);

        Response.Clear();
        Response.Write(gClass.FetchData(cmdSelect, "tblDoctor").GetXml());
        Response.End();
    }
    #endregion

    #region private void FetchTemplateData()
    private void FetchTemplateData()
    {
        SqlCommand cmdSelect = new SqlCommand();

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_Templates_LoadData", true);
        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSelect.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
        Response.Clear();
        Response.Write(gClass.ShowSchema(gClass.FetchData(cmdSelect, "tblTemplate"), Server.MapPath(".") + @"\Includes\TemplatesXSLTFile.xsl"));
        Response.End();
    }
    #endregion

    #region private void FetchLogoData()
    private void FetchLogoData()
    {
        SqlCommand cmdSelect = new SqlCommand();

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_Logos_LoadData", true);
        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        Response.Clear();
        Response.Write(gClass.ShowSchema(gClass.FetchData(cmdSelect, "tblLogo"), Server.MapPath(".") + @"\Includes\LogosXSLTFile.xsl"));
        Response.End();
    }
    #endregion

    #region private void FetchActionLogData()
    private void FetchActionLogData()
    {
        SqlCommand cmdSelect = new SqlCommand();

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_ActionLog_LoadData", true);
        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSelect.Parameters.Add("@Top", SqlDbType.Int).Value = 1;
        cmdSelect.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = DBNull.Value;
        cmdSelect.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = DBNull.Value;
        
        Response.Clear();
        Response.Write(gClass.ShowSchema(gClass.FetchData(cmdSelect, "tblActionLog"), Server.MapPath(".") + @"\Includes\ActionLogXSLTFile.xsl"));
        Response.End();
    }
    #endregion

    #region private void FetchHospitalData()
    private void FetchHospitalData()
    {
        SqlCommand cmdSelect = new SqlCommand();

        Response.Write("Load Hospital");
        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_Hospitals_LoadData", true);
        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        Response.Clear();
        Response.Write(gClass.ShowSchema(gClass.FetchData(cmdSelect, "tblHospital"), Server.MapPath(".") + @"\Includes\HospitalsXSLTFile.xsl"));
        Response.End();
    }
    #endregion

    #region private void FetchRegionData()
    private void FetchRegionData()
    {
        SqlCommand cmdSelect = new SqlCommand();

        Response.Write("Load Region");
        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_Regions_LoadData", true);
        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        Response.Clear();
        Response.Write(gClass.ShowSchema(gClass.FetchData(cmdSelect, "tblRegion"), Server.MapPath(".") + @"\Includes\RegionsXSLTFile.xsl"));
        Response.End();
    }
    #endregion

    #region private void FetchGroupData()
    private void FetchGroupData()
    {
        SqlCommand cmdSelect = new SqlCommand();

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_Codes_LoadData", true);
        cmdSelect.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
        cmdSelect.Parameters.Add("@CategoryCode", SqlDbType.VarChar, 10).Value = "GRO";
        Response.Clear();
        Response.Write(gClass.ShowSchema(gClass.FetchData(cmdSelect, "tblGroup"), Server.MapPath(".") + @"\Includes\GroupsXSLTFile.xsl"));
        Response.End();
    }
    #endregion

    #region private void FetchCategoryData()
    private void FetchCategoryData()
    {
        SqlCommand cmdSelect = new SqlCommand();

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_Codes_LoadData", true);
        cmdSelect.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
        cmdSelect.Parameters.Add("@CategoryCode", SqlDbType.VarChar, 10).Value = "PC";
        Response.Clear();
        Response.Write(gClass.ShowSchema(gClass.FetchData(cmdSelect, "tblCategory"), Server.MapPath(".") + @"\Includes\CategoryXSLTFile.xsl"));
        Response.End();
    }
    #endregion
    
    #region private void FetchDeletedPatient()
    private void FetchDeletedPatient()
    {
        SqlCommand cmdSelect = new SqlCommand();

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_PatientsList_LoadDeletedPatients", true);
        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        Response.Clear();
        Response.Write(gClass.ShowSchema(gClass.FetchData(cmdSelect, "tblDeletedPatient"), Server.MapPath(".") + @"\Includes\DeletedPatientXSLTFile.xsl"));
        Response.End();
    }
    #endregion
}
