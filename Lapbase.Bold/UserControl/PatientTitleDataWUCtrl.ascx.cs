using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class UserControl_PatientTitleDataWUCtrl : System.Web.UI.UserControl
{
    GlobalClass gClass = new GlobalClass();
    String strNumberFormat = System.Configuration.ConfigurationManager.AppSettings["NumberFormat"].ToString();

    Dictionary<string, string> codeConcurrentDesc = new Dictionary<string, string>();

    #region protected void Page_Load(object sender, EventArgs e)
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            gClass.OrganizationCode = Request.Cookies["OrganizationCode"].Value;

            if (gClass.IsUserLogoned(Session.SessionID, Request.Cookies["UserPracticeCode"].Value, Request.Url.Host))
            {
                //cmbGroup.Attributes.Add("onblur", "javascript:tblPatientTitle_cmbGroup_onchange();"); 
                if (!IsPostBack)
                    LoadPatientData();

                if (lblPatientID_Value.Text.Equals("") || lblPatientID_Value.Text.Equals("0"))
                    div_PatientTitle.Style["display"] = "none";

                gClass.FetchSystemDetails(Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value));
                txtHTargetBMI.Value = gClass.SD_TargetBMI > 0 ? gClass.SD_TargetBMI.ToString() : "27";
                lblBMI_Value.Text = txtHTargetBMI.Value;
            }
        }
        catch 
        {

        }
    }
    #endregion

    #region private void LoadPatientData()
    private void LoadPatientData()
    {

        SqlCommand cmdSelect = new SqlCommand();
        DataView    dvPatient;
        decimal visitWeight;

        string appendConcurrent = "";
        string concurrent = "";
        String[] concurrentArr;
        bool displayBand = false;
        
        codeConcurrentDesc = buildCodeDictionary("concurrent", "desc");

        cmdSelect.CommandType = CommandType.StoredProcedure;
        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_PatientTitle_LoadData", true);

        try{Response.SetCookie(new HttpCookie("PatientID", Request.Cookies["PatientID"].Value));}
        catch { Response.Cookies.Add(new HttpCookie("PatientID", Request.Cookies["PatientID"].Value));  }

        try{Response.SetCookie(new HttpCookie("UserPracticeCode", Request.Cookies["UserPracticeCode"].Value));}
        catch{Response.Cookies.Add(new HttpCookie("UserPracticeCode", Request.Cookies["UserPracticeCode"].Value));}

        //try { Response.SetCookie(new HttpCookie("OrganizationCode", gClass.OrganizationCode)); }
        //catch { Response.Cookies.Add(new HttpCookie("OrganizationCode", gClass.OrganizationCode)); }

        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt64(gClass.OrganizationCode); 
        cmdSelect.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt64(Response.Cookies["UserPracticeCode"].Value); 
        cmdSelect.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt64(Response.Cookies["PatientID"].Value); 
        cmdSelect.Parameters.Add("@ImperialFlag", SqlDbType.Bit).Value = Request.Cookies["Imperial"].Value.Equals("True");

        dvPatient = gClass.FetchData(cmdSelect, "tblPatient").Tables[0].DefaultView;


        if (dvPatient.Count > 0)
        {
            //set cookies for custom patient id
            try { Response.SetCookie(new HttpCookie("PatientCustomID", dvPatient[0]["Patient_CustomID"].ToString())); }
            catch { Response.Cookies.Add(new HttpCookie("PatientCustomID", dvPatient[0]["Patient_CustomID"].ToString())); }

            lblPatientID_Value.Text = dvPatient[0]["Patient_CustomID"].ToString();// Response.Cookies["PatientID"].Value;
            lblDOB_Value.Text = gClass.TruncateDate(dvPatient[0]["BirthDate"].ToString(), Request.Cookies["CultureInfo"].Value, 1);
            lblCreatedDate_Value.Text = gClass.TruncateDate(dvPatient[0]["DateCreated"].ToString(), Request.Cookies["CultureInfo"].Value, 1);
            lblPatientName_Value.Text = dvPatient[0]["strTitle"].ToString() + " " + dvPatient[0]["Firstname"].ToString().Trim().Replace("`", "'") + " " + dvPatient[0]["Surname"].ToString().Trim().Replace("`", "'");
            lblAddress_Value.Text = dvPatient[0]["street"].ToString().Trim().Replace("`", "'") + " " + dvPatient[0]["Suburb"].ToString().Trim().Replace("`", "'") + " " + dvPatient[0]["state"].ToString().Trim().Replace("`", "'") + " " + dvPatient[0]["Postcode"].ToString().Trim().Replace("`", "'");
            lblDoctor_Value.Text = dvPatient[0]["DoctorName"].ToString().Replace("`", "'");
            txtSurgeryType_Code.Value = dvPatient[0]["SurgeryType"].ToString();
            lblSurgeryType_Value.Text = dvPatient[0]["SurgeryType_Desc"].ToString();
            lblApproach_Value.Text = dvPatient[0]["Approach"].ToString();
            lblCategory_Value.Text = dvPatient[0]["Category_Desc"].ToString();
            lblGroup_Value.Text = dvPatient[0]["Group_Desc"].ToString();

            lblDeceased_Value.Text = gClass.TruncateDate(dvPatient[0]["DeceasedDate"].ToString(), Request.Cookies["CultureInfo"].Value, 1);
            if (lblDeceased_Value.Text.Trim() != "")
            {
                lblDeceased.Text = "Deceased: ";
            }

            //changes in this list need to be repeated in the checking below
            if (dvPatient[0]["LastSurgeryType"].ToString() == "BAA1061" || dvPatient[0]["LastSurgeryType"].ToString() == "BAA1062" || dvPatient[0]["LastSurgeryType"].ToString() == "BAA1065" || dvPatient[0]["LastSurgeryType"].ToString() == "ADDBST1" || dvPatient[0]["LastSurgeryType"].ToString() == "ADDBST2" || dvPatient[0]["LastSurgeryType"].ToString() == "ADDBST3" || dvPatient[0]["LastSurgeryType"].ToString() == "ADDBST4" || dvPatient[0]["LastSurgeryType"].ToString() == "ADDBST6" || dvPatient[0]["LastSurgeryType"].ToString() == "ADDBST7" || dvPatient[0]["LastSurgeryType"].ToString() == "ADDBST8" || dvPatient[0]["LastSurgeryType"].ToString() == "BAA1069")
            {
                displayBand = true;
                //if the last is "other", check the "second last"
                if (dvPatient[0]["LastSurgeryType"].ToString() == "BAA1069")
                {
                    displayBand = false;
                    if (dvPatient[0]["SecondLastSurgeryType"].ToString() == "BAA1061" || dvPatient[0]["SecondLastSurgeryType"].ToString() == "BAA1062" || dvPatient[0]["SecondLastSurgeryType"].ToString() == "BAA1065" || dvPatient[0]["SecondLastSurgeryType"].ToString() == "ADDBST1" || dvPatient[0]["SecondLastSurgeryType"].ToString() == "ADDBST2" || dvPatient[0]["SecondLastSurgeryType"].ToString() == "ADDBST3" || dvPatient[0]["SecondLastSurgeryType"].ToString() == "ADDBST4" || dvPatient[0]["SecondLastSurgeryType"].ToString() == "ADDBST6" || dvPatient[0]["SecondLastSurgeryType"].ToString() == "ADDBST7" || dvPatient[0]["SecondLastSurgeryType"].ToString() == "ADDBST8")
                    {
                        displayBand = true;
                    }
                }
            }
            if (displayBand == true)
            {
                lblBandType_Value.Text = dvPatient[0]["BandType_Desc"].ToString() != "" ? "Band Type: " + dvPatient[0]["BandType_Desc"].ToString() : "";
                lblBandSize_Value.Text = dvPatient[0]["BandSize_Desc"].ToString() != "" ? "Band Size: " + dvPatient[0]["BandSize_Desc"].ToString() : "";
            }

            lblSurgeryDate_Value.Text = gClass.TruncateDate(dvPatient[0]["LapBandDate"].ToString(), Request.Cookies["CultureInfo"].Value, 1);
            lblHomePhone_Value.Text = dvPatient[0]["HomePhone"].ToString().Trim().Replace("`", "'");
            lblMobilePhone_Value.Text = dvPatient[0]["MobilePhone"].ToString().Trim().Replace("`", "'");

            concurrent = dvPatient[0]["Concurrent"].ToString().Trim();
            concurrentArr = concurrent.Split(';');
            if (concurrent != "")
            {
                appendConcurrent = "Concurrent Procedure: ";
                foreach (string tempConcurrent in concurrentArr)
                {
                    if (tempConcurrent != "")
                    {
                        appendConcurrent += getDescription(tempConcurrent, "concurrent") + ", ";
                    }
                }
                appendConcurrent = appendConcurrent.Substring(0, appendConcurrent.Length - 2);
                lblConcurrent_Value.Text = appendConcurrent;
            }



            if (dvPatient[0]["VisitWeeksFlag"].ToString().Trim() != "")
            {
                try { Response.SetCookie(new HttpCookie("VisitWeeksFlag", dvPatient[0]["VisitWeeksFlag"].ToString())); }
                catch { Response.Cookies.Add(new HttpCookie("VisitWeeksFlag", dvPatient[0]["VisitWeeksFlag"].ToString())); }

                lblCalculateVisitIntro.Text = "Calculate visit weeks and weight loss from:";
                lblCalculateVisit.Text = dvPatient[0]["VisitWeeksFlag_Desc"].ToString().Trim();
                lblCalculateVisitDate.Text = gClass.TruncateDate(dvPatient[0]["CalculateVisitDate"].ToString(), Request.Cookies["CultureInfo"].Value, 1);                
            }

            if (dvPatient[0]["AGE"].ToString() != string.Empty)
                lblAge_Value.Text = dvPatient[0]["AGE"].ToString() + " yrs";
            else
                lblAge_Value.Text = "0";

            txtBMIHeight.Value = dvPatient[0]["BMIHeight"].ToString().Replace(",", ".");
            lblStartBMI_Value.Text = (dvPatient[0]["StartBMI"].ToString().Length > 0) ? Convert.ToDecimal(dvPatient[0]["StartBMI"].ToString()).ToString(strNumberFormat).Replace(",", ".") : "0";

            try 
            {
                lblStartWeight_Value.Text = Convert.ToDecimal(dvPatient[0]["StartWeight"].ToString()).ToString(strNumberFormat).Replace(",", ".");
                lblStartWeight_Unit.Text = dvPatient[0]["WeightMeasurment"].ToString();
            }
            catch { lblStartWeight_Value.Text = ""; lblStartWeight_Unit.Text = ""; }
            try 
            {
                lblIdealWeight_Value.Text = Convert.ToDecimal(dvPatient[0]["IdealWeight"].ToString()).ToString(strNumberFormat).Replace(",", ".");
                lblIdealWeight_Unit.Text = dvPatient[0]["WeightMeasurment"].ToString();
            }
            catch { lblIdealWeight_Value.Text = ""; lblIdealWeight_Unit.Text = ""; }
            try 
            {
                //lblTargetWeight_Value.Text = Convert.ToDecimal(dvPatient[0]["TargetWeight"].ToString()).ToString(strNumberFormat).Replace(",", ".");
                //lblTargetWeight_Unit.Text = dvPatient[0]["WeightMeasurment"].ToString();

                txtTargetWeight.Text = Convert.ToDecimal(dvPatient[0]["TargetWeight"].ToString()).ToString(strNumberFormat).Replace(",", "."); 
            }
            catch { //lblTargetWeight_Value.Text = ""; lblIdealWeight_Unit.Text = "";
                txtTargetWeight.Text = "";
            }
        }
        dvPatient.Dispose();
        
        txtUseImperial.Value = (Request.Cookies["Imperial"].Value == "True") ? "1" : "0";
        return;
    }
    #endregion

    #region private string CalculateBMI(decimal StartWeight, BMIHeight)
    private string CalculateBMI(decimal StartWeight, decimal BMIHeight)
    {
        decimal BMI = (decimal)Math.Round( (double)StartWeight/ Math.Pow((double)BMIHeight, (double) 2));
        return BMI.ToString();
    }
    #endregion

    public string SurgeryType
    {
        get { return lblSurgeryType_Value.Text.Trim(); }
    }

    #region private String getDescription(string code, string category){
    private String getDescription(string code, string category)
    {
        try
        {
            if (category == "concurrent")
                return codeConcurrentDesc[code];
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
        catch (Exception err)
        {
            gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "Load Code Description", "Load Code Description", err.ToString());
        }

        return tempCodeDesc;
    }
    #endregion
}
