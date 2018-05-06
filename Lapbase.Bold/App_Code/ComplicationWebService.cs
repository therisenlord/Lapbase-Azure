using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Globalization;
using Lapbase.Business;
using Lapbase.Configuration.ConfigurationApplication;

/// <summary>
/// Summary description for ComplicationWebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class ComplicationWebService : System.Web.Services.WebService {

    GlobalClass gClass = new GlobalClass();

    #region public ComplicationWebService
    public ComplicationWebService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 

        gClass.OrganizationCode = Context.Request.Cookies["OrganizationCode"].Value;
    }
    #endregion
    
    /*************************************************************************************************************
    * This Section contains all function about Complication for each Patient
    *************************************************************************************************************/
    #region [WebMethod] public XmlDocument Complication_SaveProc
    [WebMethod]
    public XmlDocument Complication_SaveProc(string ComplicationCode, string ComplicationDate, string TypeCode, string Complication, int Readmitted, int ReOperation, 
                                            string Notes, int ComplicationID, String FacilityCode, String Facility_Other, String AdverseSurgery, String DoctorID, String DateCreated, 
                                            string AdmitDate, string DischargeDate, string PerformDate, string Reason)
    {
        int Result = Complication_CanToSave(ComplicationDate);
        string temp;
        try
        {
            if (Result == 0) // All data are OK and ready to save
            {
                SqlCommand cmdSave = new SqlCommand();
                SqlCommand cmdBoldSave = new SqlCommand();
                Int32 intTemp = 0;
                DateTime dtTemp;
                SqlCommand cmdSelect = new SqlCommand();
                Boolean blnInsert;
                Int32 intCompNum = 0;
                DataSet dsAdverseEvent = new DataSet();

                if (ComplicationID == 0)
                {
                    gClass.MakeStoreProcedureName(ref cmdSave, "sp_ConsultFU1_Complications_InsertData", true);
                    cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                    cmdSave.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Context.Request.Cookies["UserPracticeCode"].Value);
                    cmdSave.Parameters.Add("@DateCreated", SqlDbType.DateTime);
                    if (DateTime.TryParse(DateCreated, out dtTemp))
                        cmdSave.Parameters["@DateCreated"].Value = dtTemp;
                    else cmdSave.Parameters["@DateCreated"].Value = DBNull.Value;
                    blnInsert = true;
                }
                else
                {
                    gClass.MakeStoreProcedureName(ref cmdSave, "sp_ConsultFU1_Complications_UpdateData", true);
                    cmdSave.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                    cmdSave.Parameters.Add("@tblComplicationsID", SqlDbType.Int).Value = Convert.ToInt32(ComplicationID);
                    blnInsert = false;
                }

                cmdSave.Parameters.Add("@PatientId", SqlDbType.Int).Value = Convert.ToInt32(Context.Request.Cookies["PatientID"].Value);
                cmdSave.Parameters.Add("@ComplicationDate", SqlDbType.DateTime);

                if (DateTime.TryParse(ComplicationDate, out dtTemp))
                    cmdSave.Parameters["@ComplicationDate"].Value = dtTemp;
                else cmdSave.Parameters["@ComplicationDate"].Value = DBNull.Value;

                cmdSave.Parameters.Add("@ComplicationCode", SqlDbType.VarChar, 10).Value = ComplicationCode;
                cmdSave.Parameters.Add("@TypeCode", SqlDbType.VarChar, 1).Value = TypeCode;
                cmdSave.Parameters.Add("@Notes", SqlDbType.VarChar, 255).Value = Notes.Replace("'", "`");
                cmdSave.Parameters.Add("@Complication", SqlDbType.VarChar, 100).Value = Complication; ;
                cmdSave.Parameters.Add("@Readmitted", SqlDbType.Bit).Value = Readmitted;
                cmdSave.Parameters.Add("@ReOperation", SqlDbType.Bit).Value = ReOperation;

                cmdSave.Parameters.Add("@AdmitDate", SqlDbType.DateTime);
                cmdSave.Parameters.Add("@DischargeDate", SqlDbType.DateTime);
                cmdSave.Parameters.Add("@PerformDate", SqlDbType.DateTime);
                
                if (DateTime.TryParse(AdmitDate, out dtTemp))
                    cmdSave.Parameters["@AdmitDate"].Value = dtTemp;
                else cmdSave.Parameters["@AdmitDate"].Value = DBNull.Value;

                if (DateTime.TryParse(DischargeDate, out dtTemp))
                    cmdSave.Parameters["@DischargeDate"].Value = dtTemp;
                else cmdSave.Parameters["@DischargeDate"].Value = DBNull.Value;

                if (DateTime.TryParse(PerformDate, out dtTemp))
                    cmdSave.Parameters["@PerformDate"].Value = dtTemp;
                else cmdSave.Parameters["@PerformDate"].Value = DBNull.Value;

                cmdSave.Parameters.Add("@Reason", SqlDbType.VarChar, 10).Value = Reason;

                cmdSave.Parameters.Add("@FacilityCode", SqlDbType.VarChar, 10).Value = FacilityCode;
                cmdSave.Parameters.Add("@Facility_Other", SqlDbType.VarChar, 100).Value = Facility_Other;
                cmdSave.Parameters.Add("@AdverseSurgery", SqlDbType.VarChar, 255).Value = AdverseSurgery;
                cmdSave.Parameters.Add("@DoctorID", SqlDbType.Int).Value = (Int32.TryParse(DoctorID, out intTemp) ? intTemp : 0);



                //insert log
                //before updating data
                if (blnInsert == false)
                {
                    try
                    {
                        SqlCommand cmdSaveLog = new SqlCommand();
                        gClass.MakeStoreProcedureName(ref cmdSaveLog, "sp_ConsultFU1_ComplicationsLog_InsertData", true);
                        cmdSaveLog.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                        cmdSaveLog.Parameters.Add("@ComplicationNum", SqlDbType.Int).Value = Convert.ToInt32(ComplicationID);
                        cmdSaveLog.Parameters.Add("@LogUserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Context.Request.Cookies["UserPracticeCode"].Value);
                        cmdSaveLog.Parameters.Add("@LogDateTime", SqlDbType.DateTime).Value = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
                        gClass.ExecuteDMLCommand(cmdSaveLog);
                    }
                    catch (Exception err)
                    {
                        gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                                    Context.Request.Cookies["Logon_UserName"].Value, "Complication  PID : " + Context.Request.Cookies["PatientID"].Value, "Data Saving Complication Log", err.ToString());
                    }
                }
                gClass.ExecuteDMLCommand(cmdSave);

                gClass.SaveUserLogFile(Context.Request.Cookies["UserPracticeCode"].Value,
                                            Context.Request.Cookies["Logon_UserName"].Value,
                                            Context.Request.Url.Host,
                                            "Complication Form", 2, "Save Complication Data " + ((ComplicationID == 0) ? "Insert" : "Update"), "PatientCode",
                                            Context.Request.Cookies["PatientID"].Value);

                if (blnInsert)
                {
                    gClass.MakeStoreProcedureName(ref cmdSelect, "sp_ConsultFU1_Complications_GetLastComplicationID", true);
                    cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                    cmdSelect.Parameters.Add("@PatientId", SqlDbType.Int).Value = Convert.ToInt32(Context.Request.Cookies["PatientID"].Value);
                    dsAdverseEvent = gClass.FetchData(cmdSelect, "tblAdverseEvent");
                    if (dsAdverseEvent.Tables[0].Rows.Count > 0)
                    {
                        intCompNum = Convert.ToInt32(dsAdverseEvent.Tables[0].Rows[0]["ComplicationNum"].ToString());
                    }
                }
                else
                {
                    intCompNum = Convert.ToInt32(ComplicationID);
                }

                gClass.SaveActionLog(gClass.OrganizationCode,
                                            Context.Request.Cookies["UserPracticeCode"].Value,
                                            Context.Request.Url.Host,
                                            System.Configuration.ConfigurationManager.AppSettings["ComplicationPage"].ToString(),
                                            ((blnInsert) ? System.Configuration.ConfigurationManager.AppSettings["LogCreate"].ToString() : System.Configuration.ConfigurationManager.AppSettings["LogUpdate"].ToString()),
                                            "Save " + System.Configuration.ConfigurationManager.AppSettings["ComplicationPage"].ToString() + " Data ", 
                                            Context.Request.Cookies["PatientID"].Value,
                                            Convert.ToString(intCompNum));

                //save to bold table
                gClass.MakeStoreProcedureName(ref cmdBoldSave, "sp_ConsultFU1_Complications_SaveBoldData", true);
                cmdBoldSave.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
                cmdBoldSave.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Context.Request.Cookies["UserPracticeCode"].Value);
                cmdBoldSave.Parameters.Add("@ComplicationNum", SqlDbType.Int).Value = intCompNum;
                cmdBoldSave.Parameters.Add("@PatientId", SqlDbType.Int).Value = Convert.ToInt32(Context.Request.Cookies["PatientID"].Value);
                cmdBoldSave.Parameters.Add("@ComplicationCode", SqlDbType.VarChar, 10).Value = ComplicationCode;
                cmdBoldSave.Parameters.Add("@ComplicationDate", SqlDbType.DateTime);
                if (DateTime.TryParse(ComplicationDate, out dtTemp))
                    cmdBoldSave.Parameters["@ComplicationDate"].Value = dtTemp;
                else cmdBoldSave.Parameters["@ComplicationDate"].Value = DBNull.Value;
                cmdBoldSave.Parameters.Add("@FacilityCode", SqlDbType.VarChar, 10).Value = FacilityCode;
                cmdBoldSave.Parameters.Add("@AdverseSurgery", SqlDbType.VarChar, 255).Value = AdverseSurgery;
                cmdBoldSave.Parameters.Add("@DoctorID", SqlDbType.Int).Value = (Int32.TryParse(DoctorID, out intTemp) ? intTemp : 0);

                gClass.ExecuteDMLCommand(cmdBoldSave);
                
                //BOLD validation
                //only check if BOLD mode is on
                if (Context.Request.Cookies["SubmitData"].Value.IndexOf("submitbold") >= 0)
                {
                    SubmitBOLDAdverseEventData(intCompNum);
                }
                gClass.SaveUserLogFile(Context.Request.Cookies["UserPracticeCode"].Value,
                                            Context.Request.Cookies["Logon_UserName"].Value,
                                            Context.Request.Url.Host,
                                            "Complication Form", 2, "Save Complication Bold Data " + ((ComplicationID == 0) ? "Insert" : "Update"), "PatientCode",
                                            Context.Request.Cookies["PatientID"].Value);


                gClass.SaveActionLog(gClass.OrganizationCode,
                                            Context.Request.Cookies["UserPracticeCode"].Value,
                                            Context.Request.Url.Host,
                                            System.Configuration.ConfigurationManager.AppSettings["ComplicationPage"].ToString(),
                                            ((blnInsert) ? System.Configuration.ConfigurationManager.AppSettings["LogCreate"].ToString() : System.Configuration.ConfigurationManager.AppSettings["LogUpdate"].ToString()),
                                            "Save " + System.Configuration.ConfigurationManager.AppSettings["ComplicationPage"].ToString() + " BOLD Data ", 
                                            Context.Request.Cookies["PatientID"].Value,
                                            Convert.ToString(intCompNum));

            }
            temp = Result.ToString();
        }
        catch (Exception err)
        {
            temp = err.ToString();
            gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host, Context.Request.Cookies["Logon_UserName"].Value, "Complication Form", "Complication_SaveProc Proc", err.ToString());
        }
        return gClass.GetXmlDocument(Guid.NewGuid(), temp);
    }
    #endregion

    #region private void Complication_SaveData
    private void Complication_SaveData(string ComplicationCode, string ComplicationDate, string TypeCode, string Complication, int Readmitted, int ReOperation, string Notes, int ComplicationID)
    {
        
    }
    #endregion

    #region private int Complication_CanToSave()
    private int Complication_CanToSave(string ComplicationDate)
    {
        int flag = 0;

        System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo(Context.Request.Cookies["CultureInfo"].Value, true);
        try { DateTime dtTemp = Convert.ToDateTime(ComplicationDate); }
        catch { flag += 1; }
        return (flag);
    }
    #endregion
    
    #region private void SubmitBOLDAdverseEventData(Int32 intCompNum)
    private void SubmitBOLDAdverseEventData(Int32 intCompNum)
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
            objSRC.SaveAdverseEvent(intCompNum);

            if (objSRC.AdverseEventPostOperativeErrors.Count > 0)
            {
                if (objSRC.AdverseEventPostOperativeErrors.Count > 0)
                {
                    for (int Xh = 0; Xh < objSRC.AdverseEventPostOperativeErrors.Count; Xh++)
                    {
                        gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                                          Context.Request.Cookies["Logon_UserName"].Value, "Patient ID : " + intPatientID.ToString() + ", Complication ID:" + intCompNum.ToString(), "BOLD - Data saving Events ", objSRC.AdverseEventPostOperativeErrors[Xh].ErrorMessage.ToString());
                    }
                }
            }
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData(Context.Request.Cookies["UserPracticeCode"].Value, Context.Request.Url.Host,
                        Context.Request.Cookies["Logon_UserName"].Value, "Patient ID : " + intPatientID.ToString() + ", Complication ID:" + intCompNum.ToString(), "BOLD - Data saving Events ", err.ToString());
        }
        //----------------------------------------------------------------------------------------------------
    }
    #endregion
}

