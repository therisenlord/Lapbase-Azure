using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;

namespace Lapbase.Data
{
    [Serializable]
    public class Baseline : ObjectBase
    {
        #region variables
        private Boolean mblnCharity
                        , mblnConsentRecieved
                        , mblnDeceased
                        , mblnInsuranceCoversProcedure
                        , mblnSelfPay;
        #endregion 

        #region properties
        /// <summary>
        /// Gets or sets the Charity
        /// </summary>
        public Boolean Charity
        {
            get{return mblnCharity;}
            set{mblnCharity = value;}
        }

        /// <summary>
        /// Gets or sets the "Consent Recieved"
        /// </summary>
        public Boolean ConsentRecieved
        {
            get {return mblnConsentRecieved;}
            set {mblnConsentRecieved = value;}
        }

        /// <summary>
        /// Gets or sets Deceased.
        /// </summary>
        public Boolean Deceased
        {
            get {return mblnDeceased;}
            set {mblnDeceased = value;}
        }

        /// <summary>
        /// Gets or sets "Insurance Covers Procedure"
        /// </summary>
        public Boolean InsuranceCoversProcedure
        {
            get { return mblnInsuranceCoversProcedure; }
            set { mblnInsuranceCoversProcedure = value; }
        }

        /// <summary>
        /// Gets or sets Selfpay.
        /// </summary>
        public Boolean SelfPay
        {
            get { return mblnSelfPay; }
            set { mblnSelfPay = value; }
        }
        
        #endregion

        /// <summary>
        /// Returns TRUE if the Chart number is unique in database.
        /// </summary>
        /// <param name="intOrganizationCode">Organization Code for current organization.</param>
        /// <param name="strChartNumer">Chart Number of Patient's BOLD Data.</param>
        /// <returns>True if the chart number is unique in database.</returns>
        public bool IsChartNumberUnique(int intOrganizationCode, String strChartNumer)
        {
            bool result = false;
            Database objectDatabase = base.ObjectDatabase;

            DbCommand command = objectDatabase.GetStoredProcCommand(base.GetStoreProcedureName("sp_ChartNumberGet", false));
            base.ObjectDatabase.AddInParameter(command, "@vintOrganizationCode", DbType.Int32, intOrganizationCode);
            base.ObjectDatabase.AddInParameter(command, "@vstrChartNumber", DbType.String, strChartNumer);
            base.ObjectDatabase.AddOutParameter(command, "@rblnIsChartNumberUnique", DbType.Boolean, 1);

            objectDatabase.ExecuteNonQuery(command);
            result = Convert.ToBoolean(objectDatabase.GetParameterValue(command, "@rblnIsChartNumberUnique"));
            return result;
        }

        /// <summary>
        /// Saves BOLD data using SRC Web service.
        /// </summary>
        /// <returns></returns>
        public bool SaveSRCData()
        {
            Boolean saveFlag = false;

            
            
            ////String[] strPreviousNonBariatricSurgeryCodes = new string();
            ////String[] strRaceCodes = new string();
            ////String[] strAdverseEventCodes = new string();
            //String strSuffix = String.Empty;


            //dtopatient.DOB = DateTime.Now;
            //dtopatient.Employer = String.Empty;
            //dtopatient.EmploymentStatusCode = String.Empty;
            //dtopatient.FirstName = String.Empty;
            //dtopatient.GenderCode = String.Empty;
            
            //dtopatient.LastName = String.Empty;
            //dtopatient.MiddleInitial = String.Empty;
            ////dtopatient.PatientInsuranceCodes = String.Empty;
            ////dtopatient.PreviousBariatricSurgeries = previousBariatricSurgery;
            ////
            ////previousBariatricSurgery.AdverseEventCodes = strAdverseEventCodes;
            //previousBariatricSurgery.LowestWeightAchieved = MetricUnit;
            ////
            ////dtopatient.PreviousNonBariatricSurgeryCodes = strPreviousNonBariatricSurgeryCodes;
            ////dtopatient.RaceCodes = strRaceCodes;
            
            //dtopatient.Suffix = strSuffix;

            //savePatientRequest.Patient = dtopatient;
            //savePatientRequest.PatientChartNumber = String.Empty;
            //savePatientRequest.PracticeCOEID = String.Empty;
            //savePatientRequest.RequestId = String.Empty;
            //savePatientRequest.SecurityToken = String.Empty;
            //savePatientRequest.VendorCode = String.Empty;
            //savePatientRequest.Version = String.Empty;
            

            return saveFlag;
        }
    }
}
