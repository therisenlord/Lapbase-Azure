using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Globalization;
using System.Xml;
using System.IO;
using System.Text;


/// <summary>
/// all webmethod funcions are called by client-side ajax functions, which generates SOAP documents
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class PatientDataWebService : System.Web.Services.WebService {

    GlobalClass gClass = new GlobalClass();
    public PatientDataWebService () {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
        gClass.OrganizationCode = Context.Request.Cookies["OrganizationCode"].Value;
    }

    //#region [WebMethod] public XmlDocument SavePatientDataProc_Demographics
    ///*
    // * this WebMethod function is to save patient data in Demographics sub-page,
    // * Notice : if you want to call a function of XML Web serivce, from client-side, it should be a PUBLIC WEBMETHOD function
    // */
    //[WebMethod]
    //public XmlDocument SavePatientDataProc_Demographics(string PatientID, string NameId, string Surname, string Firstname, string Title, string Birthdate, 
    //            string Sex, string Street, string Suburb, string State, string Postcode, string HomePhone, string WorkPhone, string Race, 
    //            string DoctorId, string RefDrId1, string RefDrId2, string RefDrId3, string MobilePhone, string EmailAddress, string Insurance)
    //{
    //    SqlCommand cmdPatient = new SqlCommand(); 
    //    string strReturn = "";

    //    if ((Surname.Trim().Length == 0) && (Firstname.Trim().Length == 0))
    //    {
    //        return gClass.GetXmlDocument(Guid.NewGuid(), "0");
    //    }

    //    System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo(Context.Request.Cookies["CultureInfo"].Value, true);

        
    //    if ((PatientID == "0") || (PatientID.Trim() == string.Empty))
    //    {
    //        PatientID = "0"; // if PatientID is null, we set it by 0
    //        gClass.MakeStoreProcedureName(ref cmdPatient, "sp_PatientData_cmdInsert", true);
    //        cmdPatient.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
    //        cmdPatient.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Context.Request.Cookies["UserPracticeCode"].Value);
    //    }
    //    else
    //        gClass.MakeStoreProcedureName(ref cmdPatient, "sp_PatientData_cmdUpdate", true);
        
    //    cmdPatient.Parameters.Add("@NameId", SqlDbType.VarChar, 7);
    //    cmdPatient.Parameters.Add("@Surname", SqlDbType.VarChar, 40);
    //    cmdPatient.Parameters.Add("@Firstname", SqlDbType.VarChar, 30);
    //    cmdPatient.Parameters.Add("@Title", SqlDbType.SmallInt);
    //    cmdPatient.Parameters.Add("@Street", SqlDbType.VarChar, 40);
    //    cmdPatient.Parameters.Add("@Suburb", SqlDbType.VarChar, 40);
    //    cmdPatient.Parameters.Add("@State", SqlDbType.VarChar, 10);
    //    cmdPatient.Parameters.Add("@Postcode", SqlDbType.VarChar, 10);
    //    cmdPatient.Parameters.Add("@HomePhone", SqlDbType.VarChar, 30);
    //    cmdPatient.Parameters.Add("@WorkPhone", SqlDbType.VarChar, 30);
    //    cmdPatient.Parameters.Add("@Race", SqlDbType.VarChar, 3);
    //    cmdPatient.Parameters.Add("@Birthdate", SqlDbType.DateTime);
    //    cmdPatient.Parameters.Add("@Sex", SqlDbType.VarChar, 1);
    //    cmdPatient.Parameters.Add("@DoctorId", SqlDbType.Int);
    //    cmdPatient.Parameters.Add("@RefDrId1", SqlDbType.VarChar, 10);
    //    cmdPatient.Parameters.Add("@RefDrId2", SqlDbType.VarChar, 10);
    //    cmdPatient.Parameters.Add("@RefDrId3", SqlDbType.VarChar, 10);
    //    cmdPatient.Parameters.Add("@MobilePhone", SqlDbType.VarChar, 30);
    //    cmdPatient.Parameters.Add("@EmailAddress", SqlDbType.VarChar, 100);
    //    cmdPatient.Parameters.Add("@Insurance", SqlDbType.VarChar, 50);

    //    gClass.AddLogParameters(ref cmdPatient,
    //                Context.Request.Cookies["Logon_UserName"].Value,
    //                Context.Request.Url.Host, (PatientID == "0") ? "insert" : "update");

    //    // ------- Setting Values
        
    //    cmdPatient.Parameters["@NameId"].Value = NameId;
    //    cmdPatient.Parameters["@Surname"].Value = Surname.Replace("'", "`");
    //    cmdPatient.Parameters["@Firstname"].Value = Firstname.Replace("'", "`");
    //    cmdPatient.Parameters["@Title"].Value = Convert.ToInt16(Title);
    //    cmdPatient.Parameters["@Street"].Value = Street.Replace("'", "`");
    //    cmdPatient.Parameters["@Suburb"].Value = Suburb.Replace("'", "`");
    //    cmdPatient.Parameters["@State"].Value = State.Replace("'", "`");
    //    cmdPatient.Parameters["@Postcode"].Value = Postcode.Replace("'", "`");
    //    cmdPatient.Parameters["@HomePhone"].Value = HomePhone.Replace("'", "`");
    //    cmdPatient.Parameters["@WorkPhone"].Value = WorkPhone.Replace("'", "`");
    //    cmdPatient.Parameters["@Race"].Value = Race.Replace("'", "`");

    //    if (Birthdate == string.Empty)
    //        cmdPatient.Parameters["@Birthdate"].Value = DBNull.Value;
    //    else
    //        try 
    //        {
    //            if (Convert.ToDateTime(Birthdate) < DateTime.Now)
    //                cmdPatient.Parameters["@Birthdate"].Value = Convert.ToDateTime(Birthdate);     
    //            else
    //                cmdPatient.Parameters["@Birthdate"].Value = DBNull.Value; 
    //        }
    //        catch { cmdPatient.Parameters["@Birthdate"].Value = DBNull.Value;  }

    //    cmdPatient.Parameters["@Sex"].Value = Sex;
    //    cmdPatient.Parameters["@DoctorId"].Value = Convert.ToInt16(DoctorId);
    //    cmdPatient.Parameters["@RefDrId1"].Value = RefDrId1.Replace("'", "`");
    //    cmdPatient.Parameters["@RefDrId2"].Value = RefDrId2.Replace("'", "`");
    //    cmdPatient.Parameters["@RefDrId3"].Value = RefDrId3.Replace("'", "`");
    //    cmdPatient.Parameters["@MobilePhone"].Value = MobilePhone.Replace("'", "`");
    //    cmdPatient.Parameters["@EmailAddress"].Value = EmailAddress.Replace("'", "`");
    //    cmdPatient.Parameters["@Insurance"].Value = Insurance.Replace("'", "`");

    //    try
    //    {
    //        if (PatientID == "0") // means new Patient Data, data must be inserted
    //        {
    //            gClass.SavePatientData(1, cmdPatient);
    //            strReturn = gClass.PatientID.ToString();
    //            Context.Response.SetCookie(new HttpCookie("PatientID", strReturn));
    //            gClass.SaveUserLogFile( Context.Request.Cookies["UserPracticeCode"].Value, 
    //                                    Context.Request.Cookies["Logon_UserName"].Value,
    //                                    Context.Request.Url.Host, 
    //                                    "Baseline Form", 2, "Add Data", "PatientCode",
    //                                    Context.Response.Cookies["PatientID"].Value);
    //        }
    //        else //data must be Updated
    //        {
    //            cmdPatient.Parameters.Add("@PatientId", SqlDbType.Int).Value = Convert.ToInt64(PatientID);
    //            cmdPatient.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Context.Request.Cookies["UserPracticeCode"].Value);
    //            cmdPatient.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);

    //            gClass.SavePatientData(2, cmdPatient);
    //            Context.Response.SetCookie(new HttpCookie("PatientID", PatientID));
    //            gClass.SaveUserLogFile( Context.Request.Cookies["UserPracticeCode"].Value, 
    //                                    Context.Request.Cookies["Logon_UserName"].Value,
    //                                    Context.Request.Url.Host,
    //                                    "Baseline Form", 2, "Modify Data", "PatientCode", PatientID);
    //            strReturn = PatientID;
    //        }
    //    }
    //    catch (Exception err)
    //    {
    //        gClass.AddErrorLogData( Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host, 
    //                                Context.Request.Cookies["Logon_UserName"].Value, "Baseline", "Data saving baseline", err.ToString());
    //        strReturn = "-1";
    //    }
    //    return gClass.GetXmlDocument(Guid.NewGuid(), strReturn);
    //}
    //#endregion

    //#region [WebMethod] public XmlDocument SavePatientDataProc_HeightWeight
    //[WebMethod]
    ///*
    // * this function is to save data in Height/Weight and Note sub-page
    // */
    //public XmlDocument SavePatientDataProc_HeightWeight(string PatientID, string Height, string StartWeight, 
    //            string IdealWeight, string CurrentWeight, string StartBMIWeight, string BMIHeight, 
    //            string BMI, string Notes)
    //{
    //    SqlCommand cmdPWD = new SqlCommand();
    //    string strReturn = "";

    //    if (PatientID.Equals("0"))  return gClass.GetXmlDocument(Guid.NewGuid(), "0");

    //    cmdPWD_AddParameters(ref cmdPWD);
    //    cmdPWD.Parameters["@OrganizationCode"].Value = Convert.ToInt32(gClass.OrganizationCode);
    //    cmdPWD.Parameters["@UserPracticeCode"].Value = Convert.ToInt32(Context.Request.Cookies["UserPracticeCode"].Value);
    //    cmdPWD.Parameters["@PageNo"].Value = 2;
    //    cmdPWD.Parameters["@PatientId"].Value = Convert.ToInt32(PatientID);

    //    try { cmdPWD.Parameters["@Height"].Value = Convert.ToDecimal(Height); }
    //    catch { cmdPWD.Parameters["@Height"].Value = 0; }

    //    try{cmdPWD.Parameters["@StartWeight"].Value = Convert.ToDecimal(StartWeight); }
    //    catch{cmdPWD.Parameters["@StartWeight"].Value = 0;}

    //    try { cmdPWD.Parameters["@IdealWeight"].Value = Convert.ToDecimal(IdealWeight);} 
    //    catch { cmdPWD.Parameters["@IdealWeight"].Value = 0; }
    //    try { cmdPWD.Parameters["@CurrentWeight"].Value = Convert.ToDecimal(CurrentWeight);}
    //    catch { cmdPWD.Parameters["@CurrentWeight"].Value = 0; }
    //    cmdPWD.Parameters["@losttofollowup"].Value = 0;
    //    try { cmdPWD.Parameters["@StartBMIWeight"].Value = Convert.ToDecimal(StartBMIWeight); }
    //    catch { cmdPWD.Parameters["@StartBMIWeight"].Value = 0; }
    //    try { cmdPWD.Parameters["@BMIHeight"].Value = Convert.ToDecimal(BMIHeight);}
    //    catch { cmdPWD.Parameters["@BMIHeight"].Value = 0; }
        
    //    try{cmdPWD.Parameters["@BMI"].Value = Convert.ToDecimal(BMI);}
    //    catch { cmdPWD.Parameters["@BMI"].Value = 0; }
    //    cmdPWD.Parameters["@Notes"].Value = Notes.Replace("'", "`");

    //    try
    //    {
    //        gClass.SavePatientWeightData(cmdPWD);
    //        strReturn = PatientID;// string.Empty;
    //        Context.Response.SetCookie(new HttpCookie("PatientID", strReturn));
    //        gClass.SaveUserLogFile(Context.Request.Cookies["UserPracticeCode"].Value,
    //                    Context.Request.Cookies["Logon_UserName"].Value,
    //                    Context.Request.Url.Host,
    //                    "Baseline Form", 2, "Modify Height/Weight/Notes data", "PatientCode",
    //                    Context.Response.Cookies["PatientID"].Value);
    //    }
    //    catch (Exception err)
    //    {
    //        gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
    //                                Context.Request.Cookies["Logon_UserName"].Value, "Baseline", "Data saving height/weight/notes", err.ToString());
    //        strReturn = "-1";
    //    }
    //    return gClass.GetXmlDocument(Guid.NewGuid(), strReturn);
    //}
    //#endregion

    //#region [WebMethod] public XmlDocument SavePatientDataProc_MajorComorbidity()
    //[WebMethod]
    //public XmlDocument SavePatientDataProc_MajorComorbidity(
    //                            int PageNo, string PatientID, bool BaseHypertensionProblems, string BaseBloodPressureRxDetails,
    //                            string BaseSystolicBP, string BaseDiastolicBP, bool BaseLipidProblems, string BaseLipidRxDetails, string BaseTriglycerides, 
    //                            string BaseTotalCholesterol, string BaseHDLCholesterol, string BaseLDLCholesterol, string BaseFBloodGlucose, bool BaseDiabetesProblems,
    //                            string BaseDiabetesRxDetails, bool BaseAsthmaProblems, string BaseAsthmaLevel, string BaseAsthmaDetails, bool BaseRefluxProblems, 
    //                            string BaseRefluxLevel, string BaseRefluxDetails, bool BaseSleepProblems, string BaseSleepLevel, string BaseSleepDetails)
    //{
    //    SqlCommand cmdPWD = new SqlCommand();
    //    string strReturn = "";

    //    if (PatientID.Equals("0")) return gClass.GetXmlDocument(Guid.NewGuid(), "0");

    //    cmdPWD_AddParameters(ref cmdPWD);

    //    cmdPWD.Parameters["@OrganizationCode"].Value = Convert.ToInt32(gClass.OrganizationCode);
    //    cmdPWD.Parameters["@UserPracticeCode"].Value = Convert.ToInt32(Context.Request.Cookies["UserPracticeCode"].Value);
    //    cmdPWD.Parameters["@PageNo"].Value = 3;
    //    cmdPWD.Parameters["@PatientId"].Value = Convert.ToInt32(PatientID);

    //    cmdPWD.Parameters["@BaseHypertensionProblems"].Value = BaseHypertensionProblems ? 1 : 0;
    //    cmdPWD.Parameters["@BaseBloodPressureRxDetails"].Value = BaseBloodPressureRxDetails;
    //    try{cmdPWD.Parameters["@BaseSystolicBP"].Value = Convert.ToDecimal(BaseSystolicBP) ;}catch{cmdPWD.Parameters["@BaseSystolicBP"].Value = 0;}
    //    try{cmdPWD.Parameters["@BaseDiastolicBP"].Value = Convert.ToDecimal(BaseDiastolicBP);}catch{cmdPWD.Parameters["@BaseDiastolicBP"].Value = 0;}
    //    cmdPWD.Parameters["@BaseLipidProblems"].Value = BaseLipidProblems ? 1 : 0;
    //    cmdPWD.Parameters["@BaseLipidRxDetails"].Value = BaseLipidRxDetails;
    //    try{cmdPWD.Parameters["@BaseTriglycerides"].Value = Convert.ToDecimal(BaseTriglycerides);}catch{cmdPWD.Parameters["@BaseTriglycerides"].Value = 0;}
    //    try{cmdPWD.Parameters["@BaseTotalCholesterol"].Value = Convert.ToDecimal(BaseTotalCholesterol);}catch{cmdPWD.Parameters["@BaseTotalCholesterol"].Value = 0;}
    //    try{cmdPWD.Parameters["@BaseHDLCholesterol"].Value = Convert.ToDecimal(BaseHDLCholesterol);}catch{cmdPWD.Parameters["@BaseHDLCholesterol"].Value = 0;}
    //    try{cmdPWD.Parameters["@BaseLDLCholesterol"].Value = Convert.ToDecimal(BaseLDLCholesterol);}catch{cmdPWD.Parameters["@BaseLDLCholesterol"].Value = 0;}
    //    try{cmdPWD.Parameters["@BaseFBloodGlucose"].Value = Convert.ToDecimal(BaseFBloodGlucose);}catch{cmdPWD.Parameters["@BaseFBloodGlucose"].Value = 0;}
        
    //    cmdPWD.Parameters["@BaseDiabetesProblems"].Value = BaseDiabetesProblems ? 1 : 0;
    //    cmdPWD.Parameters["@BaseDiabetesRxDetails"].Value = BaseDiabetesRxDetails;

    //    cmdPWD.Parameters["@BaseAsthmaProblems"].Value = BaseAsthmaProblems ? 1 : 0;
    //    cmdPWD.Parameters["@BaseAsthmaLevel"].Value = BaseAsthmaLevel;
    //    cmdPWD.Parameters["@BaseAsthmaDetails"].Value = BaseAsthmaDetails;
    //    cmdPWD.Parameters["@BaseRefluxProblems"].Value = BaseRefluxProblems ? 1 : 0;
    //    cmdPWD.Parameters["@BaseRefluxLevel"].Value = BaseRefluxLevel;
    //    cmdPWD.Parameters["@BaseRefluxDetails"].Value = BaseRefluxDetails;
    //    cmdPWD.Parameters["@BaseSleepProblems"].Value = BaseRefluxProblems ? 1 : 0;
    //    cmdPWD.Parameters["@BaseSleepLevel"].Value = BaseSleepLevel;
    //    cmdPWD.Parameters["@BaseSleepDetails"].Value = BaseSleepDetails;

    //    try
    //    {
    //        gClass.SavePatientWeightData(cmdPWD);
    //        strReturn = PatientID;
    //        Context.Response.SetCookie(new HttpCookie("PatientID", strReturn));
    //        gClass.SaveUserLogFile(Context.Request.Cookies["UserPracticeCode"].Value,
    //            Context.Request.Cookies["Logon_UserName"].Value,
    //            Context.Request.Url.Host,
    //            "Baseline Form", 2, "Modify Major comorbidity data", "PatientCode",
    //            Context.Response.Cookies["PatientID"].Value);
    //    }
    //    catch (Exception err)
    //    {
    //        gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
    //                                Context.Request.Cookies["Logon_UserName"].Value, "Baseline - Major Comoborbidity", "SavePatientDataProc_MajorComorbidity function", err.ToString());
    //        strReturn = "-1";
    //    }
    //    return gClass.GetXmlDocument(Guid.NewGuid(), strReturn);
    //}
    //#endregion

    //#region [WebMethod] public XmlDocument SavePatientDataProc_MinorComorbidity()
    //[WebMethod]
    //public XmlDocument SavePatientDataProc_MinorComorbidity(
    //        int PageNo, string PatientID, bool BaseFertilityProblems, string BaseFertilityDetails, 
    //        bool BaseArthritisProblems, string BaseArthritisDetails, bool BaseIncontinenceProblems,
    //        string BaseIncontinenceDetails, bool BaseBackProblems, string BaseBackDetails, 
    //        bool BaseCVDProblems, string BaseCVDDetails, string BaseOtherDetails , 
    //        string BaseFertilityLevel, string BaseIncontinenceLevel, string BaseArthritisLevel, 
    //        string BaseCVDLevel, string BaseBackPainLevel)
    //{
    //    SqlCommand cmdPWD = new SqlCommand();
    //    string strReturn = "";

    //    if (PatientID.Equals("0")) return gClass.GetXmlDocument(Guid.NewGuid(), "0");

    //    cmdPWD_AddParameters(ref cmdPWD);

    //    cmdPWD.Parameters["@OrganizationCode"].Value = Convert.ToInt32(gClass.OrganizationCode);
    //    cmdPWD.Parameters["@UserPracticeCode"].Value = Convert.ToInt32(Context.Request.Cookies["UserPracticeCode"].Value);
    //    cmdPWD.Parameters["@PageNo"].Value = 4;
    //    cmdPWD.Parameters["@PatientId"].Value = Convert.ToInt32(PatientID);

    //    cmdPWD.Parameters["@BaseFertilityProblems"].Value = BaseFertilityProblems;
    //    cmdPWD.Parameters["@BaseFertilityDetails"].Value = BaseFertilityDetails;
    //    cmdPWD.Parameters["@BaseArthritisProblems"].Value = BaseArthritisProblems;
    //    cmdPWD.Parameters["@BaseArthritisDetails"].Value = BaseArthritisDetails;
    //    cmdPWD.Parameters["@BaseIncontinenceProblems"].Value = BaseIncontinenceProblems;
    //    cmdPWD.Parameters["@BaseIncontinenceDetails"].Value = BaseIncontinenceDetails;
    //    cmdPWD.Parameters["@BaseBackProblems"].Value = BaseBackProblems;
    //    cmdPWD.Parameters["@BaseBackDetails"].Value = BaseBackDetails;
    //    cmdPWD.Parameters["@BaseCVDProblems"].Value = BaseCVDProblems;
    //    cmdPWD.Parameters["@BaseCVDDetails"].Value = BaseCVDDetails;
    //    cmdPWD.Parameters["@BaseOtherDetails"].Value = BaseOtherDetails;

    //    cmdPWD.Parameters["@BaseFertilityLevel"].Value = BaseFertilityLevel;
    //    cmdPWD.Parameters["@BaseArthritisLevel"].Value = BaseArthritisLevel;
    //    cmdPWD.Parameters["@BaseIncontinenceLevel"].Value = BaseIncontinenceLevel;
    //    cmdPWD.Parameters["@BaseCVDLevel"].Value = BaseCVDLevel;
    //    cmdPWD.Parameters["@BaseBackPainLevel"].Value = BaseBackPainLevel;

    //    try
    //    {
    //        gClass.SavePatientWeightData(cmdPWD);
    //        strReturn = PatientID;
    //        Context.Response.SetCookie(new HttpCookie("PatientID", strReturn));
    //        gClass.SaveUserLogFile(Context.Request.Cookies["UserPracticeCode"].Value,
    //            Context.Request.Cookies["Logon_UserName"].Value,
    //            Context.Request.Url.Host,
    //            "Baseline Form", 2, "Modify Monir comorbidity data", "PatientCode",
    //            Context.Response.Cookies["PatientID"].Value);
    //    }
    //    catch (Exception err)
    //    {
    //        gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
    //                                Context.Request.Cookies["Logon_UserName"].Value, "Baseline - Minor Comoborbidity", "SavePatientDataProc_MinorComorbidity function", err.ToString());
    //        strReturn = "-1";
    //    }
    //    return gClass.GetXmlDocument(Guid.NewGuid(), strReturn);
    //}
    //#endregion

    //#region private void cmdPWD_AddParameters(ref oledbcomm cmdPWD)
    ///*
    // * this function is to initialize the SqlCommand .
    // * all parameters of ths cmdPWD, are used in sub-pages' fields except DEMOGRAPHICS sub-page.
    // * Data of DEMOGRAPHICS are saveed into tblPatients,
    // * other data of othere sub-pages are saved into tblPatientWeightData (PWD)
    // */
    //private void cmdPWD_AddParameters(ref SqlCommand cmdPWD)
    //{
    //    gClass.MakeStoreProcedureName(ref cmdPWD, "sp_PatientWeightData_cmdInsert", true);

    //    cmdPWD.Parameters.Add("@OrganizationCode", SqlDbType.Int);
    //    cmdPWD.Parameters.Add("@UserPracticeCode", SqlDbType.Int);
    //    cmdPWD.Parameters.Add("@PatientId", SqlDbType.Int);
    //    cmdPWD.Parameters.Add("@PageNo", SqlDbType.Int);
    //    // PAGE 2
    //    cmdPWD.Parameters.Add("@Height", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@StartWeight", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@IdealWeight", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@CurrentWeight", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@losttofollowup", SqlDbType.Bit);
    //    cmdPWD.Parameters.Add("@StartBMIWeight", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@BMIHeight", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@BaseHypertensionProblems", SqlDbType.Bit);
    //    cmdPWD.Parameters.Add("@BaseBloodPressureRxDetails", SqlDbType.VarChar, 255);
    //    cmdPWD.Parameters.Add("@BaseSystolicBP", SqlDbType.Int);
    //    cmdPWD.Parameters.Add("@BaseDiastolicBP", SqlDbType.Int);
    //    cmdPWD.Parameters.Add("@BaseLipidProblems", SqlDbType.Bit);
    //    cmdPWD.Parameters.Add("@BaseLipidRxDetails", SqlDbType.VarChar, 255);
    //    cmdPWD.Parameters.Add("@BaseTriglycerides", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@BaseTotalCholesterol", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@BaseHDLCholesterol", SqlDbType.Decimal);

    //    cmdPWD.Parameters.Add("@BaseDiabetesProblems", SqlDbType.Bit);
    //    cmdPWD.Parameters.Add("@BaseDiabetesRxDetails", SqlDbType.VarChar, 255);

    //    cmdPWD.Parameters.Add("@LastImageDate", SqlDbType.DateTime);
    //    cmdPWD.Parameters.Add("@LastImageLocation", SqlDbType.VarChar, 150);
    //    cmdPWD.Parameters.Add("@BMI", SqlDbType.Decimal);

    //    // PAGE 3
    //    cmdPWD.Parameters.Add("@BaseBMR", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@BaseImpedance", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@BaseFatPerCent", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@BaseFreeFatMass", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@BaseTotalBodyWater", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@BaseHomocysteine", SqlDbType.Int);
    //    cmdPWD.Parameters.Add("@BaseTSH", SqlDbType.Int);
    //    cmdPWD.Parameters.Add("@BaseT4", SqlDbType.Int);
    //    cmdPWD.Parameters.Add("@BaseT3", SqlDbType.Int);
    //    cmdPWD.Parameters.Add("@BaseHBA1C", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@BaseFSerumInsulin", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@BaseFBloodGlucose", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@BaseIron", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@BaseFerritin", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@BaseTransferrin", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@BaseIBC", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@BaseFolate", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@BaseB12", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@BaseHemoglobin", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@BasePlatelets", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@BaseWCC", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@BaseCalcium", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@BasePhosphate", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@BaseVitD", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@BaseBilirubin", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@BaseTProtein", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@BaseAlkPhos", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@BaseALT", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@BaseAST", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@BaseGGT", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@BaseAlbumin", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@BaseSodium", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@BasePotassium", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@BaseChloride", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@BaseBicarbonate", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@BaseUrea", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@BaseCreatinine", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@BaseFatMass", SqlDbType.Decimal);
    //    cmdPWD.Parameters.Add("@BaseLDLCholesterol", SqlDbType.Decimal);

    //    // PAGE 4
    //    cmdPWD.Parameters.Add("@BaseUserField1", SqlDbType.VarChar, 25);
    //    cmdPWD.Parameters.Add("@BaseUserField2", SqlDbType.VarChar, 25);
    //    cmdPWD.Parameters.Add("@BaseUserField3", SqlDbType.VarChar, 25);
    //    cmdPWD.Parameters.Add("@BaseUserField4", SqlDbType.VarChar, 25);
    //    cmdPWD.Parameters.Add("@BaseUserField5", SqlDbType.VarChar, 25);
    //    cmdPWD.Parameters.Add("@BaseUserMemoField1", SqlDbType.VarChar, 1024);
    //    cmdPWD.Parameters.Add("@BaseUserMemoField2", SqlDbType.VarChar, 1024);

    //    // PAGE 5
    //    cmdPWD.Parameters.Add("@BaseAsthmaProblems", SqlDbType.Bit);
    //    cmdPWD.Parameters.Add("@BaseAsthmaLevel", SqlDbType.VarChar, 2);
    //    cmdPWD.Parameters.Add("@BaseAsthmaDetails", SqlDbType.VarChar, 255);

    //    cmdPWD.Parameters.Add("@BaseRefluxProblems", SqlDbType.Bit);
    //    cmdPWD.Parameters.Add("@BaseRefluxLevel", SqlDbType.VarChar, 2);
    //    cmdPWD.Parameters.Add("@BaseRefluxDetails", SqlDbType.VarChar, 255);

    //    cmdPWD.Parameters.Add("@BaseSleepProblems", SqlDbType.Bit);
    //    cmdPWD.Parameters.Add("@BaseSleepLevel", SqlDbType.VarChar, 2);
    //    cmdPWD.Parameters.Add("@BaseSleepDetails", SqlDbType.VarChar, 255);

    //    cmdPWD.Parameters.Add("@BaseFertilityProblems", SqlDbType.Bit);
    //    cmdPWD.Parameters.Add("@BaseFertilityDetails", SqlDbType.VarChar, 255);
    //    cmdPWD.Parameters.Add("@BaseArthritisProblems", SqlDbType.Bit);
    //    cmdPWD.Parameters.Add("@BaseArthritisDetails", SqlDbType.VarChar, 255);
    //    cmdPWD.Parameters.Add("@BaseIncontinenceProblems", SqlDbType.Bit);
    //    cmdPWD.Parameters.Add("@BaseIncontinenceDetails", SqlDbType.VarChar, 255);
    //    cmdPWD.Parameters.Add("@BaseBackProblems", SqlDbType.Bit);
    //    cmdPWD.Parameters.Add("@BaseBackDetails", SqlDbType.VarChar, 255);
    //    cmdPWD.Parameters.Add("@BaseCVDProblems", SqlDbType.Bit);
    //    cmdPWD.Parameters.Add("@BaseCVDDetails", SqlDbType.VarChar, 255);
    //    cmdPWD.Parameters.Add("@BaseOtherDetails", SqlDbType.VarChar, 255);

    //    cmdPWD.Parameters.Add("@BaseFertilityLevel", SqlDbType.VarChar, 2);
    //    cmdPWD.Parameters.Add("@BaseArthritisLevel", SqlDbType.VarChar, 2);
    //    cmdPWD.Parameters.Add("@BaseIncontinenceLevel", SqlDbType.VarChar, 2);
    //    cmdPWD.Parameters.Add("@BaseBackPainLevel", SqlDbType.VarChar, 2);
    //    cmdPWD.Parameters.Add("@BaseCVDLevel", SqlDbType.VarChar, 2);

    //    // Page 6
    //    cmdPWD.Parameters.Add("@Notes", SqlDbType.VarChar, 1024);

    //    // Initialize Parameters
    //    gClass.InitialParameters(ref cmdPWD);
    //    return;
    //}
    //#endregion

    /*
    #region [WebMethod] public XmlDocument CalculateAge
    [WebMethod]
    public XmlDocument CalculateAge(string strBirthDate)
    {
        string strResult;
        System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo(Context.Request.Cookies["CultureInfo"].Value, true);
        if (strBirthDate.Trim() == string.Empty)
            strResult = "";
        else
        {
            try{strResult = CalculateAge(Convert.ToDateTime(strBirthDate));}
            catch { strResult = ""; }
        }

        return gClass.GetXmlDocument(Guid.NewGuid(), strResult);
    }
    #endregion

    #region [WebMethod] public XmlDocument CheckBirthDate
    [WebMethod]
    public XmlDocument CheckBirthDate(string strBirthDate)
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo(Context.Request.Cookies["CultureInfo"].Value, true);
        DateTime BirthDate = new DateTime();
        string strResult = "";

        if (strBirthDate == string.Empty)
            strResult = "";
        else
            try
            {
                BirthDate = Convert.ToDateTime(strBirthDate);
                if (BirthDate > DateTime.Now)
                    strResult = "E2";
                else
                    strResult = CalculateAge(BirthDate);
            }
            catch { strResult = "E1";}
        return gClass.GetXmlDocument(Guid.NewGuid(), strResult);
    }
    #endregion

    #region private string CalculateAge
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
    #endregion*/
}

