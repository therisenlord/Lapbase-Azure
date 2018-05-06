using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Lapbase.Business.SRCWebReference;
using Lapbase.Data;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;


namespace Lapbase.Business
{
    public delegate void ObjectInitiliazeFunction<T>(T objSRC);

    public class TrustAllCertificatePolicy : System.Net.ICertificatePolicy
    {
        public TrustAllCertificatePolicy() { }
        public bool CheckValidationResult(ServicePoint sp,
            X509Certificate cert,
            WebRequest req,
            int problem)
        {
            return true;
        }
    }

    [Serializable]
    public class SRCErrorMessageClass
    {
        private String strErrorMessage;

        /// <summary>
        /// Gets or Sets SRC Error Message.
        /// </summary>
        public String ErrorMessage
        {
            get { return strErrorMessage; }
            set { strErrorMessage = value; }
        }
    }

    public class SRCObject
    {
        #region Constants
        private const String M_STR_ISCHEMIC_HEART = "ISCHHD"
                            , M_STR_DVT_PE = "DVTPE";
        #endregion

        #region Object Classes
        Lapbase.Data.Baseline objBaseLine;
        Lapbase.Data.OperativeVisit objOperativeVisit;
        Lapbase.Data.OperativeVisitCollection objOperativeVisitCollection;
        Lapbase.Data.HospitalVisit objHospitalVisit;
        Lapbase.Data.AdverseEventPostOperative objAdverseEventPostOperative;
        Lapbase.Data.AdverseEventPostOperativeCollection objAdverseEventPostOperativeCollection;
        //Lapbase.Data.AdverseEventPostOperative objAdverseEventPostOperative;
        #endregion

        #region variables
        private List<SRCErrorMessageClass>  PatientErrorsList = new List<SRCErrorMessageClass>()
                                            , PreOperativeVisitErrorsList = new List<SRCErrorMessageClass>()
                                            , HospitalVisitErrorsList = new List<SRCErrorMessageClass>()
                                            , PostOperativeVisitErrorsList = new List<SRCErrorMessageClass>()
                                            , AdverseEventPostOperativeErrorsList = new List<SRCErrorMessageClass>()
                                            , PatientSignErrorsList = new List<SRCErrorMessageClass> ()
                                            ;
        private List<String> 
                             strPreOperativeVisitErrorsList = new List<string>()
                            , strHospitalVisitErrorsList = new List<string>()
                            , strPostOperativeVisitErrorsList = new List<string>();

        private Boolean mblnImperial = false;
        private Int32 mintPatientID = 0;
        private Int32 mintOrganizationCode = 0;
        private Int32 mintHospitalID = 0;
        private String mstrVendorCode = String.Empty
                        , mstrPracticeCEO = String.Empty
                        , mstrSurgeonCEO = String.Empty
                        , mstrFacilityCEO = String.Empty
                        , mstrSRCUserName = String.Empty
                        , mstrSRCPassword = String.Empty
                        , mstrPatientChartNumber = String.Empty
                        ;

        // BOLD variables
        BoldService objBoldService = new BoldService();

        // Save Request and Response
        SavePatientRequest objSavePatientRequest = new SavePatientRequest();
        SavePatientResponse objSavePatientResponse = new SavePatientResponse();

        SavePreOpVisitRequest objSavePreOpVisitRequest = new SavePreOpVisitRequest();
        SavePreOpVisitResponse objSavePreOpVisitResponse = new SavePreOpVisitResponse();

        SavePostOpVisitRequest objSavePostOpVisitRequest = new SavePostOpVisitRequest();
        SavePostOpVisitResponse objSavePostOpVisitResponse = new SavePostOpVisitResponse();

        SaveHospitalVisitRequest objSaveHospitalVisitRequest = new SaveHospitalVisitRequest();
        SaveHospitalVisitResponse objSaveHospitalVisitResponse = new SaveHospitalVisitResponse();

        SavePostOpAdverseEventRequest objSavePostOpAdverseEventRequest = new SavePostOpAdverseEventRequest();
        SavePostOpAdverseEventResponse objSavePostOpAdverseEventResponse = new SavePostOpAdverseEventResponse();

        PatientResponse objPatientResponse = new PatientResponse();
        PatientRequest objPatientRequest = new PatientRequest();

        // Patient object for SRC
        dtoPatient objdtopatient = new dtoPatient();
        //dtoPreOperativeVisit objdtoPreOperativeVisit ;//= new dtoPreOperativeVisit();
        List<dtoPreOperativeVisit> objdtoPreOperativeVisitCollection;
        List<dtoPostOperativeVisit> objdtoPostOperativeVisitCollection;


        List<Int32> preOperativeVisitConsultIDCollection;
        List<Int32> postOperativeVisitConsultIDCollection;
        List<Int32> hospitalVisitAdmitID;
        List<Int32> adverseEventComplicationIDCollection;
        Boolean OpBoldFlag = false;

        //dtoPostOperativeVisit objdtoPostOperativeVisit ;//= new dtoPostOperativeVisit();
        dtoHospitalVisit objdtoHospitalVisit = new dtoHospitalVisit();

        List<dtoAdverseEventPostOperative> objdtoAdverseEventPostOperativeCollection; 
        //dtoAdverseEventPostOperative objdtoAdverseEventPostOperative = new dtoAdverseEventPostOperative();

        dtoMetricUnit MetricUnit = new dtoMetricUnit();
        eMetricUnitType mutMetricUnitType;

        dtoPatientInsurance PatientInsurance = new dtoPatientInsurance();
        #endregion

        #region methods
        public SRCObject() 
        {
            objBaseLine = new Lapbase.Data.Baseline();
            objOperativeVisit = new Lapbase.Data.OperativeVisit();
            objOperativeVisitCollection = new Lapbase.Data.OperativeVisitCollection();
            objHospitalVisit = new Lapbase.Data.HospitalVisit();
            objAdverseEventPostOperative = new Lapbase.Data.AdverseEventPostOperative();
            objAdverseEventPostOperativeCollection = new AdverseEventPostOperativeCollection();
        }
        #endregion
        
        #region Properties
        
        /// <summary>
        /// Sets Imperial mode.
        /// </summary>
        public Boolean Imperial
        {
            get { return mblnImperial; }
            set { 
                mblnImperial = value;
                mutMetricUnitType = value ? eMetricUnitType.Standard : eMetricUnitType.Metric;
            }
        }

        /// <summary>
        /// Sets the SRC Vendor Code.
        /// </summary>
        public String VendorCode
        {
            set { mstrVendorCode = value; }
            get { return mstrVendorCode; }
        }

        /// <summary>
        /// Sets the SRC Practice CEO Code.
        /// </summary>
        public String PracticeCEO
        {
            set { mstrPracticeCEO = value; }
            get { return mstrPracticeCEO; }
        }

        /// <summary>
        /// Sets the SRC Surgeon CEO Code.
        /// </summary>
        public String SurgeonCEO
        {
            set { mstrSurgeonCEO = value; }
            get { return mstrSurgeonCEO; }
        }

        /// <summary>
        /// Sets the SRC Facility CEO Code.
        /// </summary>
        public String FacilityCEO
        {
            set { mstrFacilityCEO = value; }
            get { return mstrFacilityCEO; }
        }

        /// <summary>
        /// Sets the SRC User's Name.
        /// </summary>
        public String SRCUserName
        {
            set { mstrSRCUserName = value; }
            get { return mstrSRCUserName; }
        }

        /// <summary>
        /// Sets the SRC User's Password.
        /// </summary>
        public String SRCPassword
        {
            set { mstrSRCPassword = value; }
            get { return mstrSRCPassword; }
        }

        /// <summary>
        /// Gets ot sets the selected Patient's Id.
        /// </summary>
        public Int32 PatientID
        {
            set { mintPatientID = value; }
            get { return mintPatientID; }
        }

        /// <summary>
        /// Gets ot sets the selected Hospital ID
        /// </summary>
        public Int32 HospitalID
        {
            set { mintHospitalID = value; }
            get { return mintHospitalID; }
        }

        /// <summary>
        /// Gets ot sets the selected Organization's Code.
        /// </summary>
        public Int32 OrganizationCode
        {
            set { mintOrganizationCode = value; }
            get { return mintOrganizationCode; }
        }

        /// <summary>
        /// Gets or sets the Patient's Chart Number
        /// </summary>
        public String PatientChartNumber
        {
            set { mstrPatientChartNumber = value; }
            get { return mstrPatientChartNumber; }
        }
        
        /// <summary>
        /// Gets or sets SRC Error list for Patient's data integrtation
        /// </summary>
        public List<SRCErrorMessageClass> PatientErrors
        {
            get { return PatientErrorsList; }
        }

        /// <summary>
        /// Gets the Pre-Operative Visit Errors from SRC data integration
        /// </summary>
        public List<SRCErrorMessageClass> PreOperativeVisitErrors
        {
            get { return PreOperativeVisitErrorsList; }
        }

        /// <summary>
        /// Gets the Post-Operative Visit Errors from SRC data integration
        /// </summary>
        public List<SRCErrorMessageClass> PostOperativeVisitErrors
        {
            get { return PostOperativeVisitErrorsList; }
        }


        /// <summary>
        /// Gets the Hospital Visit Errors from SRC data integration
        /// </summary>
        public List<SRCErrorMessageClass> HospitalVisitErrors
        {
            get { return HospitalVisitErrorsList; }
        }

        /// <summary>
        /// Gets the Adverse Event Post Operative Errors from SRC data integration 
        /// </summary>
        public List<SRCErrorMessageClass> AdverseEventPostOperativeErrors
        {
            get { return AdverseEventPostOperativeErrorsList; }
        }

        /// <summary>
        /// Gets the Patient's Sign Errors from SRC data integration 
        /// </summary>
        public List<SRCErrorMessageClass> PatientSignErrors
        {
            get { return PatientSignErrorsList; }
        }
        #endregion

        #region Modules
        private void BoldServiceLogin()
        {
            System.Net.ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy();
            objBoldService.Credentials = new System.Net.NetworkCredential(this.mstrSRCUserName, this.mstrSRCPassword);
            return;
        }

        public void SavePatientData()
        {
            BoldServiceLogin();
            LoadPatientBaselineData();
            
            // SEND SEND REQUEST TO SAVE PATIENT 
            objSavePatientRequest.Patient = objdtopatient;
            objSavePatientRequest.PracticeCOEID = this.PracticeCEO;
            objSavePatientRequest.VendorCode = this.VendorCode;
            objSavePatientRequest.PatientChartNumber = this.PatientChartNumber;
            objSavePatientRequest.RequestId = String.Empty; //????????????????????
            objSavePatientRequest.SecurityToken = String.Empty; //??????????????????
            objSavePatientRequest.Version = String.Empty; //????????????????????

            objSavePatientResponse = objBoldService.SavePatient(objSavePatientRequest);

            if (objSavePatientResponse.Acknowledge == AcknowledgeType.Failure)
            {
                foreach (String strError in objSavePatientResponse.Messages)
                {
                    SRCErrorMessageClass objSRCError = new SRCErrorMessageClass();
                    objSRCError.ErrorMessage = strError;
                    this.PatientErrorsList.Add(objSRCError);
                }
                return;
            }
        }        
        public void SaveAdverseEvent(Int32 intCompNum)
        {
            BoldServiceLogin();
            LoadPatientBaselineData();
            LoadAdverseEventPostOperativeData();

            int countAdverseEvent = 0;
            foreach (dtoAdverseEventPostOperative objdtoAdverseEventPostOperative in objdtoAdverseEventPostOperativeCollection)
            {
                Int32 tempComplicationNum = adverseEventComplicationIDCollection[countAdverseEvent];
                if (tempComplicationNum == intCompNum)
                {
                    objSavePostOpAdverseEventRequest.PostOperativeAdverseEvent = objdtoAdverseEventPostOperative;
                    objSavePostOpAdverseEventRequest.PatientChartNumber = this.PatientChartNumber;
                    objSavePostOpAdverseEventRequest.PracticeCOEID = this.PracticeCEO;
                    objSavePostOpAdverseEventRequest.RequestId = String.Empty; // ??????????????????????
                    objSavePostOpAdverseEventRequest.SecurityToken = String.Empty; //????????????????
                    objSavePostOpAdverseEventRequest.VendorCode = this.VendorCode;
                    objSavePostOpAdverseEventRequest.Version = String.Empty; //???????????????????????????????

                    objSavePostOpAdverseEventResponse = objBoldService.SavePostOperativeAdverseEvent(objSavePostOpAdverseEventRequest);
                    if (objSavePostOpAdverseEventResponse.Acknowledge == AcknowledgeType.Failure)
                    {
                        foreach (String strError in objSavePostOpAdverseEventResponse.Messages)
                        {
                            SRCErrorMessageClass objSRCError = new SRCErrorMessageClass();
                            objSRCError.ErrorMessage = strError;
                            this.AdverseEventPostOperativeErrorsList.Add(objSRCError);
                        }
                    }
                    else
                    {
                        String strBoldAdverseEventID = objSavePostOpAdverseEventResponse.AdverseEventID.ToString();
                        objAdverseEventPostOperative.FlagAdverseEventData(this.mintOrganizationCode, this.mintPatientID, tempComplicationNum, strBoldAdverseEventID);
                    }
                }
                countAdverseEvent++;
            }
        }
        public void SaveHospitalVisit(Int32 intAdmitID)
        {
            BoldServiceLogin();
            LoadPatientBaselineData();
            LoadHospitalVisitDataSingle(intAdmitID);

            objSaveHospitalVisitRequest.HospitalVisit = objdtoHospitalVisit;
            objSaveHospitalVisitRequest.PracticeCOEID = this.PracticeCEO;
            objSaveHospitalVisitRequest.VendorCode = this.VendorCode;
            objSaveHospitalVisitRequest.RequestId = String.Empty; // ??????????????????????
            objSaveHospitalVisitRequest.SecurityToken = String.Empty; //????????????????
            objSaveHospitalVisitRequest.PatientChartNumber = this.PatientChartNumber;
            objSaveHospitalVisitRequest.Version = String.Empty; //???????????????????????????????

            objSaveHospitalVisitResponse = objBoldService.SaveHospitalVisit(objSaveHospitalVisitRequest);

            if (objSaveHospitalVisitResponse.Acknowledge == AcknowledgeType.Failure)
            {
                foreach (String strError in objSaveHospitalVisitResponse.Messages)
                {
                    SRCErrorMessageClass objSRCError = new SRCErrorMessageClass();
                    objSRCError.ErrorMessage = strError;
                    this.HospitalVisitErrorsList.Add(objSRCError);
                }
            }
            else
            {
                String strHospitalID = objSaveHospitalVisitResponse.HospitalID.ToString();

                Decimal decLastWeight = 0m;
                Decimal.TryParse(objdtoHospitalVisit.LastWeightBeforeSurgery.MetricValue.ToString(), out decLastWeight);

                DateTime dtLastVisitDate = DateTime.Now;
                DateTime.TryParse(objdtoHospitalVisit.DateOfLastWeight.ToString(), out dtLastVisitDate);

                objHospitalVisit.FlagHospitalVisitData(this.mintOrganizationCode, this.mintPatientID, intAdmitID, decLastWeight, dtLastVisitDate, strHospitalID);
            }
        }
        public void SavePatientPreOpVisit(Int32 intConsultID)
        {
            BoldServiceLogin();
            LoadPatientBaselineData();
            LoadPreOperativeVisitData();

            // SEND SAVE REQUEST TO SAVE PRE-OPERATIVE VISIT
            int countPreOp = 0;

            foreach (dtoPreOperativeVisit objdtoPreOperativeVisit in objdtoPreOperativeVisitCollection)
            {
                Int32 tempConsultID = preOperativeVisitConsultIDCollection[countPreOp];
                if (tempConsultID == intConsultID)
                {
                    objSavePreOpVisitRequest.PreOperativeVisit = objdtoPreOperativeVisit;
                    objSavePreOpVisitRequest.PatientChartNumber = this.PatientChartNumber;
                    objSavePreOpVisitRequest.PracticeCOEID = this.PracticeCEO;
                    objSavePreOpVisitRequest.RequestId = String.Empty; //???????????????????
                    objSavePreOpVisitRequest.SecurityToken = String.Empty; //????????????????
                    objSavePreOpVisitRequest.VendorCode = this.VendorCode;
                    objSavePreOpVisitRequest.Version = String.Empty; //????????????????

                    objSavePreOpVisitResponse = objBoldService.SavePreOperativeVisit(objSavePreOpVisitRequest);
                    if (objSavePreOpVisitResponse.Acknowledge == AcknowledgeType.Failure)
                    {
                        foreach (String strError in objSavePreOpVisitResponse.Messages)
                        {
                            SRCErrorMessageClass objSRCError = new SRCErrorMessageClass();
                            objSRCError.ErrorMessage = strError;
                            this.PreOperativeVisitErrorsList.Add(objSRCError);
                        }
                    }
                    else
                    {
                        String strBoldVisitID = objSavePreOpVisitResponse.VisitID.ToString();

                        Decimal decHeight = 0m;
                        Decimal.TryParse(objdtoPreOperativeVisit.Height.MetricValue.ToString(), out decHeight);

                        Decimal decWeight = 0m;
                        Decimal.TryParse(objdtoPreOperativeVisit.Weight.MetricValue.ToString(), out decWeight);

                        DateTime dtVisitDate = DateTime.Now;
                        DateTime.TryParse(objdtoPreOperativeVisit.VisitDate.ToString(), out dtVisitDate);

                        objOperativeVisit.FlagPatientOperativeVisitData(this.mintOrganizationCode, this.mintPatientID, tempConsultID, decHeight, decWeight, dtVisitDate, strBoldVisitID, "");

                    }
                }
                countPreOp++;
            }
        }
        public void SavePatientPostOpVisit(Int32 intConsultID)
        {
            BoldServiceLogin();
            LoadPatientBaselineData();
            LoadPostOperativeVisitData();

            // SEND SAVE REQUEST TO SAVE POST-OPERATIVE VISIT
            int countPostOp = 0;
            foreach (dtoPostOperativeVisit objdtoPostOperativeVisit in objdtoPostOperativeVisitCollection)
            {
                Int32 tempConsultID = postOperativeVisitConsultIDCollection[countPostOp];
                if (tempConsultID == intConsultID)
                {
                    objSavePostOpVisitRequest.PostOperativeVisit = objdtoPostOperativeVisit;
                    objSavePostOpVisitRequest.PatientChartNumber = this.PatientChartNumber;
                    objSavePostOpVisitRequest.PracticeCOEID = this.PracticeCEO;
                    objSavePostOpVisitRequest.RequestId = String.Empty; //???????????????????
                    objSavePostOpVisitRequest.SecurityToken = String.Empty; //????????????????
                    objSavePostOpVisitRequest.VendorCode = this.VendorCode;
                    objSavePostOpVisitRequest.Version = String.Empty; //????????????????

                    objSavePostOpVisitResponse = objBoldService.SavePostOperativeVisit(objSavePostOpVisitRequest);



                    if (objSavePostOpVisitResponse.Acknowledge == AcknowledgeType.Failure)
                    {
                        foreach (String strError in objSavePostOpVisitResponse.Messages)
                        {
                            SRCErrorMessageClass objSRCError = new SRCErrorMessageClass();
                            objSRCError.ErrorMessage = strError;
                            this.PostOperativeVisitErrorsList.Add(objSRCError);
                        }
                    }
                    else
                    {
                        String strBoldVisitID = objSavePostOpVisitResponse.VisitID.ToString();
                        String strSupportGroupFrequency = objdtoPostOperativeVisit.SupportGroupFrequency.ToString();

                        Decimal decHeight = 0m;
                        Decimal.TryParse(objdtoPostOperativeVisit.Height.MetricValue.ToString(), out decHeight);

                        Decimal decWeight = 0m;
                        Decimal.TryParse(objdtoPostOperativeVisit.Weight.MetricValue.ToString(), out decWeight);

                        DateTime dtVisitDate = DateTime.Now;
                        DateTime.TryParse(objdtoPostOperativeVisit.VisitDate.ToString(), out dtVisitDate);

                        objOperativeVisit.FlagPatientOperativeVisitData(this.mintOrganizationCode, this.mintPatientID, tempConsultID, decHeight, decWeight, dtVisitDate, strBoldVisitID, strSupportGroupFrequency);
                    }
                }
                countPostOp++;
            }
        }

        /// <summary>
        /// Gets Patient's Baseline(Demo graphic) data for selected patients.
        /// </summary>
        private void LoadPatientBaselineData()
        {
            String[] emptyArray = new String[0];
            // Loads Baseline data for the current patient
            objBaseLine.PatientID = this.mintPatientID;
            objBaseLine.OrganizationCode = this.mintOrganizationCode;

            objBaseLine.LoadPatientBaselineData();
            objdtopatient.ConsentRecieved = objBaseLine.ConsentRecieved;
            //objdtopatient.Deceased = objBaseLine.Deceased;
            objdtopatient.YearOfBirth = objBaseLine.YearOfBirth;
            objdtopatient.Employer = objBaseLine.Employer;
            objdtopatient.EmploymentStatusCode = objBaseLine.EmploymentStatusCode;
            objdtopatient.FirstName = objBaseLine.FirstName;
            objdtopatient.GenderCode = objBaseLine.GenderCode;
            objdtopatient.LastName = objBaseLine.LastName;
            objdtopatient.MiddleInitial = objBaseLine.MiddleName;
            objdtopatient.RaceCodes = objBaseLine.RaceCodes;

            dtoPatientInsurance objdtoPatientInsurance = new dtoPatientInsurance();
            objdtoPatientInsurance.PaymentCodes = objBaseLine.PaymentCodes;
            objdtoPatientInsurance.PreCertMentalHealth = objBaseLine.PreCertMentalHealth;
            if (objBaseLine.PreCertProgramCode.ToString() != "")
                objdtoPatientInsurance.PreCertProgramCode = objBaseLine.PreCertProgramCode;

            if (objBaseLine.PreOperativeWeightLoss > 0)
            {
                objdtoPatientInsurance.WeightLossAmount = new dtoMetricUnit();
                objdtoPatientInsurance.WeightLossAmount.MetricValue = objBaseLine.PreOperativeWeightLoss;
                objdtoPatientInsurance.WeightLossAmount.UnitType = mutMetricUnitType;
            }
            objdtopatient.Insurance = objdtoPatientInsurance;
            
            //objdtopatient.Charity = objBaseLine.Charity;
            //objdtopatient.SelfPay = objBaseLine.SelfPay;
            //objdtopatient.PatientInsuranceCodes = objBaseLine.PatientInsuranceCodes;
            //objdtopatient.InsuranceCoversProcedure = objBaseLine.InsuranceCoversProcedure;
            objdtopatient.Suffix = objBaseLine.Suffix;
            objdtopatient.PreviousNonBariatricSurgeryCodes = objBaseLine.PreviousNonBariatricSurgeryCodes == null ? emptyArray : objBaseLine.PreviousNonBariatricSurgeryCodes;
            this.PatientChartNumber = objBaseLine.ChartNumber;

            // Loads BoldData for the current patient
            //objdtopatient.PreviousBariatricSurgeries = new dtoPreviousBariatricSurgery[] { new dtoPreviousBariatricSurgery() };
            //objdtopatient.PreviousBariatricSurgeries.Initialize();
            List<dtoPreviousBariatricSurgery> listPreviousBariatricSurgery = new List<dtoPreviousBariatricSurgery>();
            if (objBaseLine.PreviousBariatricSurgeries != null)
                foreach (String strPBS in objBaseLine.PreviousBariatricSurgeries)
                {
                    if (strPBS != String.Empty)
                    {
                        dtoPreviousBariatricSurgery objdtoPBS = new dtoPreviousBariatricSurgery();
                        objdtoPBS.Code = strPBS;
                        objdtoPBS.Name = null;
                        objdtoPBS.AdverseEventCodes = objBaseLine.PBS_AdverseEventCodes;
                        objdtoPBS.SurgeonID = objBaseLine.PBS_Surgeon;
                        objdtoPBS.Year = objBaseLine.PBS_Year;

                        objdtoPBS.OriginalWeight = new dtoMetricUnit();
                        objdtoPBS.OriginalWeight.MetricValue = objBaseLine.PBS_OriginalWeight;
                        objdtoPBS.OriginalWeight.Estimated = objBaseLine.PBS_OriginalWeight_Estimated;
                        objdtoPBS.OriginalWeight.UnitType = mutMetricUnitType;

                        objdtoPBS.LowestWeightAchieved = new dtoMetricUnit();
                        objdtoPBS.LowestWeightAchieved.MetricValue = objBaseLine.PBS_LowestWeightAchieved;
                        objdtoPBS.LowestWeightAchieved.Estimated = objBaseLine.PBS_LowestWeightAchieved_Estimated;
                        objdtoPBS.LowestWeightAchieved.UnitType = mutMetricUnitType;

                        listPreviousBariatricSurgery.Add(objdtoPBS);
                    }
                }
            objdtopatient.PreviousBariatricSurgeries = listPreviousBariatricSurgery.ToArray();

            return;
        }

        /// <summary>
        /// Gets single Hospital visit data for current patient
        /// </summary>
        private void LoadHospitalVisitDataSingle(Int32 intAdmitID)
        {
            objHospitalVisit.OrganizationCode = this.OrganizationCode;
            objHospitalVisit.PatientID = this.PatientID;
            objHospitalVisit.LoadHospitalVisitDataSingle(intAdmitID);
            
            List<dtoAdverseEventBeforeDischarge> listAdverseEventBeforeDischarge = new List<dtoAdverseEventBeforeDischarge>();
            if (objHospitalVisit.AdverseEventsBeforeDischarge != null)
                foreach (String strADVE in objHospitalVisit.AdverseEventsBeforeDischarge)
                {
                    if (strADVE != String.Empty)
                    {
                        dtoAdverseEventBeforeDischarge objdtoADVE = new dtoAdverseEventBeforeDischarge();
                        objdtoADVE.AdverseEventCode = strADVE;
                        objdtoADVE.TimeAfterSurgery = objHospitalVisit.TimeAfterSurgery;
                        objdtoADVE.TimeAfterMeasurement = objHospitalVisit.TimeAfterMeasurement.ToString().ToLower() == eTimeMeasurement.DAYS.ToString().ToLower() ? eTimeMeasurement.DAYS : eTimeMeasurement.HOURS;
                        objdtoADVE.SurgeonCOEID = objHospitalVisit.PreDischargeSurgeon;
                        objdtoADVE.SurgeryCodes = objHospitalVisit.SurgeryCodes;

                        listAdverseEventBeforeDischarge.Add(objdtoADVE);
                    }
                }
            objdtoHospitalVisit.AdverseEventsBeforeDischarge = listAdverseEventBeforeDischarge.ToArray();
            
            objdtoHospitalVisit.ID = objHospitalVisit.HospitalID;
            objdtoHospitalVisit.ASAClassificationCode = objHospitalVisit.ASAClassificationCode;
            objdtoHospitalVisit.BariatricProcedureCode = objHospitalVisit.BariatricProcedureCode;
            objdtoHospitalVisit.BariatricTechniqueCode = objHospitalVisit.BariatricTechniqueCode;
            objdtoHospitalVisit.BloodTransfusionInUnits = objHospitalVisit.BloodTransfusionInUnits;
            objdtoHospitalVisit.ConcurrentProcedureCodes = objHospitalVisit.ConcurrentProcedureCodes;
            objdtoHospitalVisit.DateOfAdmission = objHospitalVisit.DateOfAdmission;
            objdtoHospitalVisit.DateOfLastWeight = objHospitalVisit.DateOfLastWeight;
            objdtoHospitalVisit.DischargeDate = objHospitalVisit.DischargeDate;
            objdtoHospitalVisit.DischargeLocationCode = objHospitalVisit.DischargeLocationCode;
            objdtoHospitalVisit.DurationOfAnesthesia = objHospitalVisit.DurationOfAnesthesia;
            objdtoHospitalVisit.DurationOfSurgery = objHospitalVisit.DurationOfSurgery;
            objdtoHospitalVisit.DVTProphylaxisTherapyCodes = objHospitalVisit.DVTProphylaxisTherapyCodes;
            objdtoHospitalVisit.EstimatedBloodLossInCC = objHospitalVisit.EstimatedBloodLossInCC;
            objdtoHospitalVisit.FacilityCOEID = objHospitalVisit.FacilityCOEID;
            //objdtoHospitalVisit.FacilityCOEID = this.FacilityCEO;
            objdtoHospitalVisit.IntraOpAdverseEventCodes = objHospitalVisit.IntraOpAdverseEventCodes;
            objdtoHospitalVisit.LastWeightBeforeSurgery = new dtoMetricUnit();
            objdtoHospitalVisit.LastWeightBeforeSurgery.UnitType = mutMetricUnitType;
            objdtoHospitalVisit.LastWeightBeforeSurgery.MetricValue = objHospitalVisit.LastWeightBeforeSurgery;
            objdtoHospitalVisit.Revision = objHospitalVisit.Revision; //??????
            objdtoHospitalVisit.SurgeonCOEID = objHospitalVisit.SurgeonCOEID;
            //objdtoHospitalVisit.SurgeonCOEID = this.SurgeonCEO;
            objdtoHospitalVisit.SurgeryDate = objHospitalVisit.SurgeryDate;
            objdtoHospitalVisit.SurgicalFellowParticiated = objHospitalVisit.SurgicalFellowParticiated;
            objdtoHospitalVisit.SurgicalResidentParticipated = objHospitalVisit.SurgicalResidentParticipated;

            OpBoldFlag = objHospitalVisit.Bold_Flag;
            return;
        }




        public void LoadPatientData()
        {
            BoldServiceLogin();
            LoadPatientBaselineData();
            LoadPreOperativeVisitData();
            LoadHospitalVisitData();
            LoadPostOperativeVisitData();
            LoadAdverseEventPostOperativeData();

            // SEND SEND REQUEST TO SAVE PATIENT 
            objSavePatientRequest.Patient = objdtopatient;
            objSavePatientRequest.PracticeCOEID = this.PracticeCEO;
            objSavePatientRequest.VendorCode = this.VendorCode;
            objSavePatientRequest.PatientChartNumber = this.PatientChartNumber;
            objSavePatientRequest.RequestId = String.Empty; //????????????????????
            objSavePatientRequest.SecurityToken = String.Empty; //??????????????????
            objSavePatientRequest.Version = String.Empty; //????????????????????

            objSavePatientResponse = objBoldService.SavePatient(objSavePatientRequest);

            if (objSavePatientResponse.Acknowledge == AcknowledgeType.Failure)
            {
                foreach (String strError in objSavePatientResponse.Messages)
                {
                    SRCErrorMessageClass objSRCError = new SRCErrorMessageClass();
                    objSRCError.ErrorMessage = strError;
                    this.PatientErrorsList.Add(objSRCError);
                }
                return;
            }


            // SEND SAVE REQUEST TO SAVE PRE-OPERATIVE VISIT
            int countPreOp = 0;

            foreach (dtoPreOperativeVisit objdtoPreOperativeVisit in objdtoPreOperativeVisitCollection)
            {
                objSavePreOpVisitRequest.PreOperativeVisit = objdtoPreOperativeVisit;
                objSavePreOpVisitRequest.PatientChartNumber = this.PatientChartNumber;
                objSavePreOpVisitRequest.PracticeCOEID = this.PracticeCEO;
                objSavePreOpVisitRequest.RequestId = String.Empty; //???????????????????
                objSavePreOpVisitRequest.SecurityToken = String.Empty; //????????????????
                objSavePreOpVisitRequest.VendorCode = this.VendorCode;
                objSavePreOpVisitRequest.Version = String.Empty; //????????????????

                objSavePreOpVisitResponse = objBoldService.SavePreOperativeVisit(objSavePreOpVisitRequest);
                if (objSavePreOpVisitResponse.Acknowledge == AcknowledgeType.Failure)
                {
                    foreach (String strError in objSavePreOpVisitResponse.Messages)
                    {
                        SRCErrorMessageClass objSRCError = new SRCErrorMessageClass();
                        objSRCError.ErrorMessage = strError;
                        this.PreOperativeVisitErrorsList.Add(objSRCError);
                    }
                }
                else
                {
                    String strBoldVisitID = objSavePreOpVisitResponse.VisitID.ToString();

                    Decimal decHeight = 0m;
                    Decimal.TryParse(objdtoPreOperativeVisit.Height.MetricValue.ToString(), out decHeight);

                    Decimal decWeight = 0m;
                    Decimal.TryParse(objdtoPreOperativeVisit.Weight.MetricValue.ToString(), out decWeight);

                    DateTime dtVisitDate = DateTime.Now;
                    DateTime.TryParse(objdtoPreOperativeVisit.VisitDate.ToString(), out dtVisitDate);

                    Int32 tempConsultID = preOperativeVisitConsultIDCollection[countPreOp];
                    objOperativeVisit.FlagPatientOperativeVisitData(this.mintOrganizationCode, this.mintPatientID, tempConsultID, decHeight, decWeight, dtVisitDate, strBoldVisitID, "");

                }
                countPreOp++;
            }

            // SEND SAVE REQUEST TO SAVE HOSPITAL VISIT
            //no more check if sign = false
            //if (objSaveHospitalVisitRequest != null && OpBoldFlag == false)
            if (objSaveHospitalVisitRequest != null)
            {
                objSaveHospitalVisitRequest.HospitalVisit = objdtoHospitalVisit;
                objSaveHospitalVisitRequest.PracticeCOEID = this.PracticeCEO;
                objSaveHospitalVisitRequest.VendorCode = this.VendorCode;
                objSaveHospitalVisitRequest.RequestId = String.Empty; // ??????????????????????
                objSaveHospitalVisitRequest.SecurityToken = String.Empty; //????????????????
                objSaveHospitalVisitRequest.PatientChartNumber = this.PatientChartNumber;
                objSaveHospitalVisitRequest.Version = String.Empty; //???????????????????????????????

                objSaveHospitalVisitResponse = objBoldService.SaveHospitalVisit(objSaveHospitalVisitRequest);

                if (objSaveHospitalVisitResponse.Acknowledge == AcknowledgeType.Failure)
                {
                    foreach (String strError in objSaveHospitalVisitResponse.Messages)
                    {
                        SRCErrorMessageClass objSRCError = new SRCErrorMessageClass();
                        objSRCError.ErrorMessage = strError;
                        this.HospitalVisitErrorsList.Add(objSRCError);
                    }
                }
                else
                {
                    String strHospitalID = objSaveHospitalVisitResponse.HospitalID.ToString();

                    Decimal decLastWeight = 0m;
                    Decimal.TryParse(objdtoHospitalVisit.LastWeightBeforeSurgery.MetricValue.ToString(), out decLastWeight);

                    DateTime dtLastVisitDate = DateTime.Now;
                    DateTime.TryParse(objdtoHospitalVisit.DateOfLastWeight.ToString(), out dtLastVisitDate);

                    Int32 AdmitID = hospitalVisitAdmitID[0];
                    objHospitalVisit.FlagHospitalVisitData(this.mintOrganizationCode, this.mintPatientID, AdmitID, decLastWeight, dtLastVisitDate, strHospitalID);
                }
            }

            // SEND SAVE REQUEST TO SAVE POST-OPERATIVE VISIT
            int countPostOp = 0;
            foreach (dtoPostOperativeVisit objdtoPostOperativeVisit in objdtoPostOperativeVisitCollection)
            {
                objSavePostOpVisitRequest.PostOperativeVisit = objdtoPostOperativeVisit;
                objSavePostOpVisitRequest.PatientChartNumber = this.PatientChartNumber;
                objSavePostOpVisitRequest.PracticeCOEID = this.PracticeCEO;
                objSavePostOpVisitRequest.RequestId = String.Empty; //???????????????????
                objSavePostOpVisitRequest.SecurityToken = String.Empty; //????????????????
                objSavePostOpVisitRequest.VendorCode = this.VendorCode;
                objSavePostOpVisitRequest.Version = String.Empty; //????????????????

                objSavePostOpVisitResponse = objBoldService.SavePostOperativeVisit(objSavePostOpVisitRequest);



                if (objSavePostOpVisitResponse.Acknowledge == AcknowledgeType.Failure)
                {
                    foreach (String strError in objSavePostOpVisitResponse.Messages)
                    {
                        SRCErrorMessageClass objSRCError = new SRCErrorMessageClass();
                        objSRCError.ErrorMessage = strError;
                        this.PostOperativeVisitErrorsList.Add(objSRCError);
                    }
                }
                else
                {
                    String strBoldVisitID = objSavePostOpVisitResponse.VisitID.ToString();
                    String strSupportGroupFrequency = objdtoPostOperativeVisit.SupportGroupFrequency.ToString();

                    Decimal decHeight = 0m;
                    Decimal.TryParse(objdtoPostOperativeVisit.Height.MetricValue.ToString(), out decHeight);

                    Decimal decWeight = 0m;
                    Decimal.TryParse(objdtoPostOperativeVisit.Weight.MetricValue.ToString(), out decWeight);

                    DateTime dtVisitDate = DateTime.Now;
                    DateTime.TryParse(objdtoPostOperativeVisit.VisitDate.ToString(), out dtVisitDate);

                    Int32 tempConsultID = postOperativeVisitConsultIDCollection[countPostOp];
                    objOperativeVisit.FlagPatientOperativeVisitData(this.mintOrganizationCode, this.mintPatientID, tempConsultID, decHeight, decWeight, dtVisitDate, strBoldVisitID, strSupportGroupFrequency);
                }
                countPostOp++;
            }

            // SEND SAVE REQUEST TO SAVE ADVERSE EVENT POST OPERATIVE DATA
            int countAdverseEvent = 0;
            foreach (dtoAdverseEventPostOperative objdtoAdverseEventPostOperative in objdtoAdverseEventPostOperativeCollection)
            {
                objSavePostOpAdverseEventRequest.PostOperativeAdverseEvent = objdtoAdverseEventPostOperative;
                objSavePostOpAdverseEventRequest.PatientChartNumber = this.PatientChartNumber;
                objSavePostOpAdverseEventRequest.PracticeCOEID = this.PracticeCEO;
                objSavePostOpAdverseEventRequest.RequestId = String.Empty; // ??????????????????????
                objSavePostOpAdverseEventRequest.SecurityToken = String.Empty; //????????????????
                objSavePostOpAdverseEventRequest.VendorCode = this.VendorCode;
                objSavePostOpAdverseEventRequest.Version = String.Empty; //???????????????????????????????

                objSavePostOpAdverseEventResponse = objBoldService.SavePostOperativeAdverseEvent(objSavePostOpAdverseEventRequest);
                if (objSavePostOpAdverseEventResponse.Acknowledge == AcknowledgeType.Failure)
                {
                    foreach (String strError in objSavePostOpAdverseEventResponse.Messages)
                    {
                        SRCErrorMessageClass objSRCError = new SRCErrorMessageClass();
                        objSRCError.ErrorMessage = strError;
                        this.AdverseEventPostOperativeErrorsList.Add(objSRCError);
                    }
                }
                else
                {
                    String strBoldAdverseEventID = objSavePostOpAdverseEventResponse.AdverseEventID.ToString();
                    Int32 tempComplicationNum = adverseEventComplicationIDCollection[countAdverseEvent];
                    objAdverseEventPostOperative.FlagAdverseEventData(this.mintOrganizationCode, this.mintPatientID, tempComplicationNum, strBoldAdverseEventID);
                }
                countAdverseEvent++;
            }
           
            return;
        }

        public void SignPatientData()
        {
            BoldServiceLogin();
            LoadPatientBaselineData();

            // SEND SIGN PATIENT REQUEST 
            objPatientRequest.PatientChartNumber = this.PatientChartNumber;
            objPatientRequest.PracticeCOEID = this.PracticeCEO;
            objPatientRequest.RequestId = String.Empty;
            objPatientRequest.SecurityToken = String.Empty;
            objPatientRequest.VendorCode = this.VendorCode;
            objPatientRequest.Version = String.Empty;
            objPatientResponse = objBoldService.SignPatientRecord(objPatientRequest);

            if (objPatientResponse.Acknowledge == AcknowledgeType.Failure)
            {
                foreach (String strError in objPatientResponse.Messages)
                {
                    SRCErrorMessageClass objSRCError = new SRCErrorMessageClass();
                    objSRCError.ErrorMessage = strError;
                    this.PatientSignErrorsList.Add(objSRCError);
                }
            }
            else
            {
                objBaseLine.SignPatientData();
            }
            return;
        }
        
        /// <summary>
        /// Gets the pre operative visit data for current patient.
        /// </summary>
        private void LoadPreOperativeVisitData()
        {
            objOperativeVisitCollection.Load(this.OrganizationCode, this.PatientID, this.Imperial , Common.VisitType.PreOperative);
            objdtoPreOperativeVisitCollection = new List<dtoPreOperativeVisit>();
            preOperativeVisitConsultIDCollection = new List<Int32>();
            Int32 tempConsultID;

            foreach (OperativeVisit objOperativeVisit in objOperativeVisitCollection)
            {
                dtoPreOperativeVisit objdtoPreOperativeVisit = new dtoPreOperativeVisit();
                tempConsultID = 0;

                if (objOperativeVisit.ComorbiditiesList != null)
                {
                    List<dtoComorbidity> dtoComorbidityList = new List<dtoComorbidity>();
                    foreach (Lapbase.Data.Comorbidity objComorbidity in objOperativeVisit.ComorbiditiesList)
                    {
                        dtoComorbidity objdtoComorbidity = new dtoComorbidity();

                        objdtoComorbidity.ComorbidityCode = objComorbidity.ComorbidityCode;
                        objdtoComorbidity.StratificationCode = objComorbidity.StratificationCode;
                        dtoComorbidityList.Add(objdtoComorbidity);
                    }
                    if (dtoComorbidityList.Count > 0)
                        objdtoPreOperativeVisit.Comorbidities = dtoComorbidityList.ToArray();

                    objdtoPreOperativeVisit.Height = new dtoMetricUnit();
                    objdtoPreOperativeVisit.Height.UnitType = mutMetricUnitType;
                    objdtoPreOperativeVisit.Height.MetricValue = objOperativeVisit.Height;
                    objdtoPreOperativeVisit.VisitDate = objOperativeVisit.VisitDate;
                    objdtoPreOperativeVisit.Weight = new dtoMetricUnit();
                    objdtoPreOperativeVisit.Weight.UnitType = mutMetricUnitType;
                    objdtoPreOperativeVisit.Weight.MetricValue = objOperativeVisit.Weight;
                    objdtoPreOperativeVisit.ID = objOperativeVisit.VisitID;
                    objdtoPreOperativeVisit.VitaminCodes = objOperativeVisit.VitmainCodes;
                    objdtoPreOperativeVisit.MedicationCodes = objOperativeVisit.MedicationCodes;

                    tempConsultID = objOperativeVisit.ConsultID;
                    //objdtoPreOperativeVisit.ConsultID = objOperativeVisit.ConsultID;
                    //objdtoPreOperativeVisit.MedicationCodes.Initialize(); //?????????????????
                }
                preOperativeVisitConsultIDCollection.Add(tempConsultID);
                objdtoPreOperativeVisitCollection.Add(objdtoPreOperativeVisit);
            }
            return;
        }   

        /// <summary>
        /// Gets the post operative visit data for current patient.
        /// </summary>
        private void LoadPostOperativeVisitData()
        {
            objOperativeVisitCollection.Load(this.OrganizationCode, this.PatientID, this.Imperial, Common.VisitType.PostOperative);
            objdtoPostOperativeVisitCollection = new List<dtoPostOperativeVisit>();
            postOperativeVisitConsultIDCollection = new List<Int32>();
            Int32 tempConsultID;

            foreach (OperativeVisit objOperativeVisit in objOperativeVisitCollection)
            {
                dtoPostOperativeVisit objdtoPostOperativeVisit = new dtoPostOperativeVisit();
                tempConsultID = 0;

                if (objOperativeVisit.ComorbiditiesList != null)
                {
                    List<dtoComorbidity> dtoComorbidityList = new List<dtoComorbidity>();
                    foreach (Lapbase.Data.Comorbidity objComorbidity in objOperativeVisit.ComorbiditiesList)
                    {
                        dtoComorbidity objdtoComorbidity = new dtoComorbidity();

                        objdtoComorbidity.ComorbidityCode = objComorbidity.ComorbidityCode;
                        objdtoComorbidity.StratificationCode = objComorbidity.StratificationCode;
                        dtoComorbidityList.Add(objdtoComorbidity);
                    }
                    if (dtoComorbidityList.Count > 0)
                    {
                        // Remove ISCHEMIC from Cardiovascular
                        dtoComorbidityList.Remove(
                            dtoComorbidityList.Find(
                                delegate(dtoComorbidity objComorbidity) 
                                {
                                    return objComorbidity.ComorbidityCode.Equals(M_STR_ISCHEMIC_HEART);
                                }
                            )
                        );

                        // Remove DVT/PE from Cardiovascular
                        dtoComorbidityList.Remove(
                            dtoComorbidityList.Find(
                                delegate(dtoComorbidity objComorbidity)
                                {
                                    return objComorbidity.ComorbidityCode.Equals(M_STR_DVT_PE);
                                }
                            )
                        );
                        objdtoPostOperativeVisit.Comorbidities = dtoComorbidityList.ToArray();
                    }

                    objdtoPostOperativeVisit.Height = new dtoMetricUnit();
                    objdtoPostOperativeVisit.Height.UnitType = mutMetricUnitType;
                    objdtoPostOperativeVisit.Height.MetricValue = objOperativeVisit.Height;
                    objdtoPostOperativeVisit.VisitDate = objOperativeVisit.VisitDate;
                    objdtoPostOperativeVisit.Weight = new dtoMetricUnit();
                    objdtoPostOperativeVisit.Weight.UnitType = mutMetricUnitType;
                    objdtoPostOperativeVisit.Weight.MetricValue = objOperativeVisit.Weight;
                    objdtoPostOperativeVisit.ID = objOperativeVisit.VisitID;
                    objdtoPostOperativeVisit.VitaminCodes = objOperativeVisit.VitmainCodes;
                    objdtoPostOperativeVisit.MedicationCodes = objOperativeVisit.MedicationCodes;
                    objdtoPostOperativeVisit.SupportGroupFrequency = objOperativeVisit.SupportGroupFrequency;

                    tempConsultID = objOperativeVisit.ConsultID;
                    //objdtoPostOperativeVisit.MedicationCodes.Initialize(); //?????????????????
                }
                postOperativeVisitConsultIDCollection.Add(tempConsultID);
                objdtoPostOperativeVisitCollection.Add(objdtoPostOperativeVisit);
            }
            return;
        }

        /// <summary>
        /// Gets the Hospital visit data for current patient
        /// </summary>
        private void LoadHospitalVisitData()
        {
            objHospitalVisit.OrganizationCode = this.OrganizationCode;
            objHospitalVisit.PatientID = this.PatientID;
            objHospitalVisit.LoadHospitalVisitData();
            hospitalVisitAdmitID = new List<Int32>();

            //objdtoHospitalVisit.AdverseEventsBeforeDischarge[0]. = objHospitalVisit.AdverseEventsBeforeDischarge;

            //////objdtoHospitalVisit.Hospita = objAdverseEventPostOperative.AdverseEventID; //??????????????????????
            objdtoHospitalVisit.ASAClassificationCode = objHospitalVisit.ASAClassificationCode;
            objdtoHospitalVisit.BariatricProcedureCode = objHospitalVisit.BariatricProcedureCode;
            objdtoHospitalVisit.BariatricTechniqueCode = objHospitalVisit.BariatricTechniqueCode;
            objdtoHospitalVisit.BloodTransfusionInUnits = objHospitalVisit.BloodTransfusionInUnits;
            objdtoHospitalVisit.ConcurrentProcedureCodes = objHospitalVisit.ConcurrentProcedureCodes;
            objdtoHospitalVisit.DateOfAdmission = objHospitalVisit.DateOfAdmission;
            objdtoHospitalVisit.DateOfLastWeight = objHospitalVisit.DateOfLastWeight;
            objdtoHospitalVisit.DischargeDate = objHospitalVisit.DischargeDate;
            objdtoHospitalVisit.DischargeLocationCode = objHospitalVisit.DischargeLocationCode;
            objdtoHospitalVisit.DurationOfAnesthesia = objHospitalVisit.DurationOfAnesthesia;
            objdtoHospitalVisit.DurationOfSurgery = objHospitalVisit.DurationOfSurgery;
            objdtoHospitalVisit.DVTProphylaxisTherapyCodes = objHospitalVisit.DVTProphylaxisTherapyCodes;
            objdtoHospitalVisit.EstimatedBloodLossInCC = objHospitalVisit.EstimatedBloodLossInCC;
            objdtoHospitalVisit.FacilityCOEID = objHospitalVisit.FacilityCOEID;
            //objdtoHospitalVisit.FacilityCOEID = this.FacilityCEO;
            objdtoHospitalVisit.IntraOpAdverseEventCodes = objHospitalVisit.IntraOpAdverseEventCodes;
            objdtoHospitalVisit.LastWeightBeforeSurgery = new dtoMetricUnit();
            objdtoHospitalVisit.LastWeightBeforeSurgery.UnitType = mutMetricUnitType;
            objdtoHospitalVisit.LastWeightBeforeSurgery.MetricValue = objHospitalVisit.LastWeightBeforeSurgery;
            objdtoHospitalVisit.Revision = objHospitalVisit.Revision; //??????
            objdtoHospitalVisit.SurgeonCOEID = objHospitalVisit.SurgeonCOEID;
            //objdtoHospitalVisit.SurgeonCOEID = this.SurgeonCEO;
            objdtoHospitalVisit.SurgeryDate = objHospitalVisit.SurgeryDate;
            objdtoHospitalVisit.SurgicalFellowParticiated = objHospitalVisit.SurgicalFellowParticiated;
            objdtoHospitalVisit.SurgicalResidentParticipated = objHospitalVisit.SurgicalResidentParticipated;

            OpBoldFlag = objHospitalVisit.Bold_Flag;
            hospitalVisitAdmitID.Add(objHospitalVisit.AdmitID);
            return;
        }

        /// <summary>
        /// Gets the Adverse Event Post Operative data for current patient.
        /// </summary>
        private void LoadAdverseEventPostOperativeData()
        {
            objAdverseEventPostOperativeCollection.Load(this.OrganizationCode, this.PatientID);
            objdtoAdverseEventPostOperativeCollection = new List<dtoAdverseEventPostOperative>();
            adverseEventComplicationIDCollection = new List<Int32>();

            Int32 tempComplicationNum;
            foreach (AdverseEventPostOperative objAdverseEventPostOperative in objAdverseEventPostOperativeCollection)
            {
                tempComplicationNum = 0;
                dtoAdverseEventPostOperative objdtoAdverseEventPostOperative = new dtoAdverseEventPostOperative();

                objdtoAdverseEventPostOperative.AdverseEventCode = objAdverseEventPostOperative.AdverseEventCode;

                objdtoAdverseEventPostOperative.AdverseEventID = objAdverseEventPostOperative.AdverseEventID; //??????????????????????
                objdtoAdverseEventPostOperative.DateOfEvent = objAdverseEventPostOperative.DateOfEvent;
                objdtoAdverseEventPostOperative.SurgeryCodes = objAdverseEventPostOperative.SurgeryCodes;
                objdtoAdverseEventPostOperative.FacilityCOEID = objAdverseEventPostOperative.FacilityCOEID;
                objdtoAdverseEventPostOperative.SurgeonCOEID = objAdverseEventPostOperative.SurgeonCOEID;
                //objdtoAdverseEventPostOperative.FacilityCOEID = this.FacilityCEO;
                //objdtoAdverseEventPostOperative.SurgeonCOEID = this.SurgeonCEO;

                tempComplicationNum= objAdverseEventPostOperative.ComplicationNum;

                adverseEventComplicationIDCollection.Add(tempComplicationNum);
                objdtoAdverseEventPostOperativeCollection.Add(objdtoAdverseEventPostOperative);
            }
            return;
        }       
        #endregion
    }

}
