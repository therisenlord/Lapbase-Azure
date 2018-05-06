using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Lapbase.Data
{
    [Serializable]
    public class Baseline : ObjectBase
    {
        #region variables
        
        private Decimal mdecPBS_LowestWeightAchieved
                        , mdecPBS_OriginalWeight;

        private Nullable<Boolean> mblnInsuranceCoversProcedure;

        private Boolean mblnCharity
                        , mblnConsentRecieved
                        , mblnDeceased
                        , mblnSelfPay
                        , mblnPBS_LowestWeightAchieved_Estimated
                        , mblnPBS_OriginalWeight_Estimated
                        , mblnPreCertMentalHealth
                        ;

        private String  mstrFirstName
                        , mstrMiddleName
                        , mstrLastName
                        , mstrSuffix
                        , mstrAllRaceCodes
                        , mstrGenderCode
                        , mstrChartNumber
                        , mstrEmploymentStatusCode
                        , mstrEmployer
                        , mstrPBS_Code
                        , mstrPBS_SurgeonID
                        , mstrPBS_Name
                        , mstrInsurance
                        , mstrPreCertProgramCode
                        ; 

        private String[] mstrPatientInsuranceCodes
                        , mstrPreviousBariatricSurgeries
                        , mstrPreviousNonBariatricSurgeryCodes
                        , mstrRaceCodes
                        , mstrPBS_AdverseEventCodes
                        , mstrPaymentCodes
                        ;

        private Int16   mintCoverProcedure;
        private Int32   mintPatientID
                        , mintOrganizationCode
                        , mintYearOfBirth
                        , mintPBS_Year
                        ;

        private Decimal mdecPreOperativeWeightLoss;
        #endregion

        #region properties
        /// <summary>
        /// Gets ot sets the selected Patient's Id.
        /// </summary>
        public Int32 PatientID
        {
            set { mintPatientID = value; }
            get { return mintPatientID; }
        }

        /// <summary>
        /// Gets ot sets the selected Organization's Code.
        /// </summary>
        public Int32 OrganizationCode
        {
            set { mintOrganizationCode = value; }
        }

        /// <summary>
        /// Gets or sets the Charity
        /// </summary>
        public Boolean Charity
        {
            get { return mblnCharity; }
            set { mblnCharity = value; }
        }

        /// <summary>
        /// Gets or sets the "Consent Recieved"
        /// </summary>
        public Boolean ConsentRecieved
        {
            get { return mblnConsentRecieved; }
            set { mblnConsentRecieved = value; }
        }
        
        /// <summary>
        /// Gets or sets Deceased.
        /// </summary>
        public Boolean Deceased
        {
            get { return mblnDeceased; }
            set { mblnDeceased = value; }
        }

        /// <summary>
        /// Gets or sets "Insurance Covers Procedure"
        /// </summary>
        private Int16 IntInsuranceCoversProcedure
        {
            set
            {
                switch (value)
                {
                    case 1:
                        mblnInsuranceCoversProcedure = true;
                        break;
                    case 0 :
                        mblnInsuranceCoversProcedure = false;
                        break;
                    case -1 :
                        mblnInsuranceCoversProcedure = null;
                        break;
                    default:
                        mblnInsuranceCoversProcedure = null;
                        break;
                }
            }
        }

        /// <summary>
        /// Gets or sets "Insurance Covers Procedure"
        /// </summary>
        public Nullable<Boolean> InsuranceCoversProcedure
        {
            get { return mblnInsuranceCoversProcedure; }
        }

        /// <summary>
        /// Gets or sets Selfpay.
        /// </summary>
        public Boolean SelfPay
        {
            get { return mblnSelfPay; }
            set { mblnSelfPay = value; }
        }

        /// <summary>
        /// Gets or sets the Patient's FirstName.
        /// </summary>
        public String FirstName
        {
            get{ return mstrFirstName;}
            set{ mstrFirstName = value;}
        }

        /// <summary>
        /// Gets or sets the Patient's MiddleName
        /// </summary>
        public String MiddleName
        {
            get{return mstrMiddleName;}
            set{mstrMiddleName = value;}
        }

        /// <summary>
        /// Gets or sets the Patient's LastName
        /// </summary>
        public String LastName
        {
            get{return mstrLastName;}
            set{mstrLastName = value;}
        }
        
        /// <summary>
        /// Gets or sets the Patient's Suffix (Mr., Mrs.,...)
        /// </summary>
        public String Suffix
        {
            get{return mstrSuffix;}
            set{mstrSuffix = value;}
        }

        /// <summary>
        /// Gets or sets the Patient's Birth year(String).
        /// </summary>
        public Int32 YearOfBirth
        {
            get { return mintYearOfBirth; }
            set { mintYearOfBirth = value; }
        }
        
        /// <summary>
        /// Sets the Patient's Race
        /// </summary>
        private String AllRaceCodes
        {
            set 
            {
                SplitString(value, ref mstrRaceCodes);
            }
        }

        /// <summary>
        /// Gets the Patient's Race
        /// </summary>
        public String[] RaceCodes
        {
            get { return mstrRaceCodes; }
        }

        /// <summary>
        /// Gets or sets the Patient's Gender
        /// </summary>
        public String GenderCode
        {
            get{return mstrGenderCode;}
            set{mstrGenderCode = value;}
        }
			
        /// <summary>
        /// Gets or sets the Patient's Chart Number
        /// </summary>
        public String ChartNumber
        {
            get{return mstrChartNumber;}
            set{mstrChartNumber = value;}
        }

        /// <summary>
        /// Gets or sets the Patient's Employment status
        /// </summary>
        public String EmploymentStatusCode 
        {
            get{return mstrEmploymentStatusCode;}
            set{mstrEmploymentStatusCode = value;}
        }

        /// <summary>
        /// Gets or sets the Patient's Employer
        /// </summary>
        public String Employer
        {
            get{return mstrEmployer;}
            set{mstrEmployer = value;}
        }
        
        /// <summary>
        /// Gets or sets the PreCert Program Code
        /// </summary>
        public String PreCertProgramCode
        {
            get { return mstrPreCertProgramCode; }
            set { mstrPreCertProgramCode = value; }
        }

        /// <summary>
        /// Gets or sets the Mental Clearance
        /// </summary>
        public Boolean PreCertMentalHealth
        {
            get { return mblnPreCertMentalHealth; }
            set { mblnPreCertMentalHealth = value; }
        }

        /// <summary>
        /// Gets or sets the Patient's Insurance Cover Procedure.
        /// </summary>
        public Int16 CoverProcedure
        {
            get { return mintCoverProcedure; }
            set { mintCoverProcedure = value; }
        }

        /// <summary>
        /// Gets or sets the Patient's Pre Operative Weight Loss.
        /// </summary>
        public Decimal PreOperativeWeightLoss
        {
            get{return mdecPreOperativeWeightLoss;}
            set{mdecPreOperativeWeightLoss = value;}
        }
        
        /// <summary>
        /// Sets the Payment Codes
        /// </summary>
        private String AllPaymentCodes
        {
            set
            {
                base.SplitString(value, ref mstrPaymentCodes, ",");
            }
        }
        
        /// <summary>
        /// Gets the Payment Codes
        /// </summary>
        public String[] PaymentCodes
        {
            get { return mstrPaymentCodes; }
        }


        /// <summary>
        /// Gets or sets Insurance
        /// </summary>
        public String Insurance
        {
            get { return mstrInsurance; }
            set { mstrInsurance = value; }
        }


        /// <summary>
        /// Sets the Patient's Insurance Codes.
        /// </summary>
        private String AllPatientInsuranceCodes
        {
            set{
                base.SplitString(value, ref mstrPatientInsuranceCodes, ",");
            }
        }

        /// <summary>
        /// Gets the alll Patient's Insurance Codes.
        /// </summary>
        public String[] PatientInsuranceCodes
        {
            get { return mstrPatientInsuranceCodes; }
        }

        /// <summary>
        /// Loads and sets the splitted Adverse Event Codes
        /// </summary>
        private String PBS_AllAdverseEventCodes
        {
            set
            {
                SplitString(value, ref mstrPBS_AdverseEventCodes, ";");
            }
        }

        /// <summary>
        /// Gets the splitted Adverse Event codes of Previous Bariatric Surgeries.
        /// </summary>
        public String[] PBS_AdverseEventCodes
        {
            get { return mstrPBS_AdverseEventCodes; }
        }

        /// <summary>
        /// Gets and sets the code of Previous Bariatric Surgeries.
        /// </summary>
        public String PBS_Code
        {
            get { return mstrPBS_Code;}
            set { mstrPBS_Code = value;}
        }

        /// <summary>
        /// Gets or sets the Lowest Weight Achieved of Previous Bariatric Surgeries.
        /// </summary>
        public Decimal PBS_LowestWeightAchieved
        {
            get{return mdecPBS_LowestWeightAchieved;}
            set{mdecPBS_LowestWeightAchieved = value;}
        }

        /// <summary>
        /// Gets or sets the Estimate flag of the Lowest Weight Achieved of Previous Bariatric Surgeries.
        /// </summary>
        public Boolean PBS_LowestWeightAchieved_Estimated
        {
            get{return mblnPBS_LowestWeightAchieved_Estimated;}
            set{mblnPBS_LowestWeightAchieved_Estimated = value;}
        }

        /// <summary>
        /// Gets or sets the original Weight of Previous Bariatric Surgeries.
        /// </summary>
        public Decimal PBS_OriginalWeight
        {
            get{return mdecPBS_OriginalWeight;}
            set{mdecPBS_OriginalWeight = value;}
        }

        /// <summary>
        /// Gets or sets the Estimate flag of the Original Weight of Previous Bariatric Surgeries.
        /// </summary>
        public Boolean PBS_OriginalWeight_Estimated
        {
            get{return mblnPBS_OriginalWeight_Estimated;}
            set{mblnPBS_OriginalWeight_Estimated = value;}
        }

        /// <summary>
        /// Gets or sets the Year of of Previous Bariatric Surgeries.
        /// </summary>
        public Int32 PBS_Year
        {
            get { return mintPBS_Year; }
            set { mintPBS_Year = value; }
        }

        /// <summary>
        /// Gets or sets the Surgeon's Name of Previous Bariatric Surgeries.
        /// </summary>
        public String PBS_Surgeon
        {
            get { return mstrPBS_SurgeonID; }
            set { mstrPBS_SurgeonID = value; }
        }

        /// <summary>
        /// Gets or sets the Name of Previous Bariatric Surgeries.
        /// </summary>
        public String PBS_Name
        {
            get { return mstrPBS_Name; }
            set { mstrPBS_Name = value; }
        }

        /// <summary>
        /// Sets the Patient's Previous Bariatric Surgeries.
        /// </summary>
        private String AllPreviousBariatricSurgeries
        {
            set
            {
                SplitString(value, ref mstrPreviousBariatricSurgeries, ";");
            }
        }

        /// <summary>
        /// Gets the all Patient's Previous Bariatric Surgeries.
        /// </summary>
        public String[] PreviousBariatricSurgeries
        {
            get{return mstrPreviousBariatricSurgeries;}
        }

        /// <summary>
        /// Sets the all Patient's Previous Non-Bariatric Surgery Codes.
        /// </summary>
        private String AllPreviousNonBariatricSurgeryCodes
        {
            set
            {
                base.SplitString(value, ref mstrPreviousNonBariatricSurgeryCodes, ";");
            }
        }

        /// <summary>
        /// Gets the Patient's Previous Non-Bariatric Surgery Codes.
        /// </summary>
        public String[] PreviousNonBariatricSurgeryCodes
        {
            get { return mstrPreviousNonBariatricSurgeryCodes; }
        }

        /// <summary>
        /// Sets the all Patient's Adverse Events Codes.
        /// </summary>
        private String AllAdverseEventsCodes
        {
            set
            {
                SplitString(value, ref mstrPBS_AdverseEventCodes, ";");
            }
        }

        /// <summary>
        /// Gets the Patient's Previous Non-Bariatric Surgery Codes.
        /// </summary>
        public String[] AdverseEventsCodes
        {
            get { return mstrPBS_AdverseEventCodes; }
        }
        #endregion

        #region overrides
        /// <summary>
	    /// Set the Baseline object to an empty instance.
	    /// </summary>
	    /// <history>
	    /// 	<change user="ALI-87AB9129BB0\Administrator" date="24 Jun 2008">Initial version. (CS 2.1.0)</change>
	    /// </history>
	    public override void Clear()
	    {
            mblnCharity = false;
            mblnConsentRecieved = false;
            mblnDeceased = false;
            mblnInsuranceCoversProcedure = null;
            mblnSelfPay = false;

            mstrFirstName = String.Empty;
            mstrMiddleName= String.Empty;
            mstrLastName = String.Empty;
            mstrSuffix = String.Empty;
            mstrAllRaceCodes = String.Empty;
            mstrGenderCode = String.Empty;
            mstrChartNumber = String.Empty;
            mstrEmploymentStatusCode = String.Empty;
            mstrEmployer = String.Empty;
            mstrPreCertProgramCode = String.Empty;

            mintYearOfBirth = 0;
            mdecPreOperativeWeightLoss = 0;
		    base.Clear();
	    }

        /// <summary>
        /// Uses a given data set cell (from a dataset, datareader ...) to set one of the tblUserApplicationData object property.
        /// </summary>
        /// <param name="vName">Name of the column holding the data.</param>
        /// <param name="vValue">Value used to set one of the object properties.</param>
        /// <returns>Boolean. True if the vName parameter has been found and processed and is representative of the object/record.</returns>
        /// <history>
        /// 	<change user="AFarahani" date="24 Jun 2008">Initial version. (CS 2.1.0)</change>
        /// </history>
        protected internal override bool SetProperty(string vName, object vValue)
        {
            bool returnValue = false;

            switch (vName.ToLower())
            {
                case "firstname":
                    if (vValue != System.DBNull.Value)
                    {
                        this.FirstName = Convert.ToString(vValue);
                    }
                    break;

                case "middlename":
                    if (vValue != System.DBNull.Value)
                    {
                        this.MiddleName = Convert.ToString(vValue);
                        returnValue = true;
                    }
                    break;

                case "lastname":
                    if (vValue != System.DBNull.Value)
                    {
                        this.LastName = Convert.ToString(vValue);
                        returnValue = true;
                    }
                    break;

                case "suffix":
                    if (vValue != System.DBNull.Value)
                    {
                        this.Suffix = Convert.ToString(vValue);
                        returnValue = true;
                    }
                    break;

                case "yearofbirth":
                    if (vValue != System.DBNull.Value)
                    {
                        Int32 intTemp = 0;

                        Int32.TryParse(Convert.ToString(vValue), out intTemp);
                        this.YearOfBirth = intTemp;
                        returnValue = true;
                    }
                    break;

                case "race":
                    if (vValue != System.DBNull.Value)
                    {
                        this.AllRaceCodes = Convert.ToString(vValue);
                        returnValue = true;
                    }
                    break;

                case "gendercode":
                    if (vValue != System.DBNull.Value)
                    {
                        this.GenderCode = Convert.ToString(vValue);
                        returnValue = true;
                    }
                    break;

                case "chartnumber":
                    if (vValue != System.DBNull.Value)
                    {
                        this.ChartNumber = Convert.ToString(vValue);
                        returnValue = true;
                    }
                    break;

                case "employmentstatuscode":
                    if (vValue != System.DBNull.Value)
                    {
                        this.EmploymentStatusCode = Convert.ToString(vValue);
                        returnValue = true;
                    }
                    break;

                case "consentrecieved":
                    if (vValue != System.DBNull.Value)
                    {
                        this.ConsentRecieved = Convert.ToString(vValue).Equals(Boolean.TrueString);
                        returnValue = true;
                    }
                    break;

                case "employer":
                    if (vValue != System.DBNull.Value)
                    {
                        this.Employer = Convert.ToString(vValue);
                        returnValue = true;
                    }
                    break;

                case "insurance":
                    if (vValue != System.DBNull.Value)
                    {
                        this.AllPaymentCodes = vValue.ToString();
                        returnValue = true;
                    }
                    break;

                case "dietryweightloss":
                    if (vValue != System.DBNull.Value)
                    {
                        this.PreCertProgramCode = Convert.ToString(vValue);
                        returnValue = true;
                    }
                    break;

                case "mentalhealthclearance":
                    if (vValue != System.DBNull.Value)
                    {
                        this.PreCertMentalHealth = Convert.ToString(vValue).Equals(Boolean.TrueString);
                        returnValue = true;
                    }
                    break;

                case "insurancecoversprocedure":
                    if (vValue != System.DBNull.Value)
                    {
                        Int16 intTemp;

                        if (Int16.TryParse(Convert.ToString(vValue), out intTemp))
                            this.IntInsuranceCoversProcedure = intTemp;
                        else
                            this.IntInsuranceCoversProcedure = -1;
                        returnValue = true;
                    }
                    break;

                case "selfpay":
                    if (vValue != System.DBNull.Value)
                    {
                        this.SelfPay = Convert.ToString(vValue).Equals(Boolean.TrueString);
                        returnValue = true;
                    }
                    break;

                case "charity":
                    if (vValue != System.DBNull.Value)
                    {
                        this.Charity = Convert.ToString(vValue).Equals(Boolean.TrueString);
                        returnValue = true;
                    }
                    break;

                case "coverprocedure":
                    if (vValue != System.DBNull.Value)
                    {
                        Int16 intTemp = 0;

                        if (Int16.TryParse(Convert.ToString(vValue), out intTemp))
                            this.CoverProcedure = intTemp;
                        else
                            this.CoverProcedure = 0;
                        returnValue = true;
                    }
                    break;

                case "preoperativeweightloss":
                    if (vValue != System.DBNull.Value)
                    {
                        decimal decTemp = 0m;

                        this.PreOperativeWeightLoss = Decimal.TryParse(Convert.ToString(vValue), out decTemp) ? decTemp : 0m;
                        returnValue = true;
                    }
                    break;

                case "patientinsurancecodes":
                    if (vValue != System.DBNull.Value)
                    {
                        this.AllPatientInsuranceCodes = Convert.ToString(vValue);
                        returnValue = true;
                    }
                    break;

                case "previousbariatricsurgeries":
                    if (vValue != System.DBNull.Value)
                    {
                        this.AllPreviousBariatricSurgeries = Convert.ToString(vValue);
                        returnValue = true;
                    }
                    break;

                case "previousnonbariatricsurgerycodes":
                    if (vValue != System.DBNull.Value)
                    {
                        this.AllPreviousNonBariatricSurgeryCodes = Convert.ToString(vValue);
                        returnValue = true;
                    }
                    break;

                case "pbs_year" :
                    if (vValue != System.DBNull.Value)
                    {
                        Int32 intTemp = 0;

                        Int32.TryParse(Convert.ToString(vValue), out intTemp);
                        this.PBS_Year = intTemp;
                        returnValue = true;
                    }
                    break;

                case "pbs_originalweight" :
                    if (vValue != System.DBNull.Value)
                    {
                        Decimal decTemp = 0;

                        Decimal.TryParse(Convert.ToString(vValue), out decTemp);
                        this.PBS_OriginalWeight = decTemp;
                        returnValue = true;
                    }
                    break;

                case "pbs_originalweight_actual":
                    if (vValue != System.DBNull.Value)
                    {
                        this.PBS_OriginalWeight_Estimated = ! vValue.ToString().Equals(Boolean.TrueString);
                        returnValue = true;
                    }
                    break;

                case "pbs_lowestweightachieved" :
                    if (vValue != System.DBNull.Value)
                    {
                        Decimal decTemp = 0;

                        Decimal.TryParse(Convert.ToString(vValue), out decTemp);
                        this.PBS_LowestWeightAchieved = decTemp;
                        returnValue = true;
                    }
                    break;

                case "pbs_lowestweightachieved_actual":
                    if (vValue != System.DBNull.Value)
                    {
                        this.PBS_LowestWeightAchieved_Estimated = !vValue.ToString().Equals(Boolean.TrueString);
                        returnValue = true;
                    }
                    break;

                case "adverseeventscodes":
                    if (vValue != System.DBNull.Value)
                    {
                        this.AllAdverseEventsCodes = Convert.ToString(vValue); 
                        returnValue = true;
                    }
                    break;

                case "pbs_doctorboldcode":
                    if (vValue != System.DBNull.Value)
                    {
                        this.PBS_Surgeon = vValue.ToString();
                        returnValue = true;
                    }
                    break;

                case "pbs_name" :
                    if (vValue != System.DBNull.Value)
                    {
                        this.PBS_Name = Convert.ToString(vValue); 
                        returnValue = true;
                    }
                    break;

                default:
                    returnValue = base.SetProperty(vName, vValue);
                    break;
            }

            base.NewRecord = false;

            return returnValue;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Loads Patient's data to export data into SRC system.
        /// </summary>
        /// <param name="OrganizationCode">The value for current Domain/Organization.</param>
        /// <param name="PatientID">The ID for selected Patient.</param>
        /// <returns></returns>
        public bool LoadPatientBaselineData( )
        {
            Database objectDatabase = base.ObjectDatabase;

            DbCommand command = objectDatabase.GetStoredProcCommand(base.GetStoreProcedureName("sp_SRCPatientDataGet", false));
            base.ObjectDatabase.AddInParameter(command, "@vintOrganizationCode", DbType.Int32, this.mintOrganizationCode);
            base.ObjectDatabase.AddInParameter(command, "@vintPatientId", DbType.Int32, this.mintPatientID);

            return this.GetProperties(command);
        }

        /// <summary>
        /// Flag Patient Visit and hospital visit as sent to BOLD
        /// </summary>
        /// <returns></returns>
        public void SignPatientData()
        {
            Database objectDatabase = base.ObjectDatabase;

            DbCommand command = objectDatabase.GetStoredProcCommand(base.GetStoreProcedureName("sp_SRCSignPatientRecord", false));
            base.ObjectDatabase.AddInParameter(command, "@vintOrganizationCode", DbType.Int32, this.mintOrganizationCode);
            base.ObjectDatabase.AddInParameter(command, "@vintPatientId", DbType.Int32, this.mintPatientID);

            this.Save(command);
        }
        #endregion
    }
}
