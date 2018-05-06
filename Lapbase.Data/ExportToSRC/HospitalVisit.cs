using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Lapbase.Data
{
    public class HospitalVisit : ObjectBase
    {
        #region variables
        private Int32 mintPatientID
                    , mintOrganizationCode
                    , mintAdmitID
                    , mintID
                    , mintTimeAfterSurgery;

        private String[] mstrConcurrentProcedureCodes
                        , mstrDVTProphylaxisTherapyCodes  
                        , mstrIntraOpAdverseEventCodes
                        , mstrAdverseEventsBeforeDischarge
                        , mstrSurgeryCodes
                        ;

        private String mstrHospitalID
                        , mstrASAClassificationCode
                        , mstrDischargeLocationCode 
                        , mstrFacilityCOEID  
                        , mstrSurgeonCOEID
                        , mstrBariatricProcedureCode
                        , mstrBariatricTechniqueCode
                        , mstrTimeAfterMeasurement
                        , mstrPreDischargeSurgeon
                        ;

        private Decimal mdecBloodTransfusionInUnits
                        , mdecDurationOfAnesthesia 
                        , mdecDurationOfSurgery 
                        , mdecEstimatedBloodLossInCC
                        , mdecLastWeightBeforeSurgery
                        ;
        private DateTime mdtmDateOfAdmission
                        , mdtmDateOfLastWeight
                        , mdtmDischargeDate
                        , mdtmSurgeryDate
                        ;

        private Boolean mblnRevision 
                        , mblnSurgicalFellowParticiated
                        , mblnSurgicalResidentParticipated
                        , mblnBoldFlag
                        ;

        #endregion

        #region Properties
        /// <summary>
        /// Gets ot sets the selected Patient's Id.
        /// </summary>
        public Int32 PatientID
        {
            set { mintPatientID = value; }
        }

        /// <summary>
        /// Gets ot sets the selected Organization's Code.
        /// </summary>
        public Int32 OrganizationCode
        {
            set { mintOrganizationCode = value; }
        }


        /// <summary>
        /// Gets or sets the HospitalID.
        /// </summary>
        public String HospitalID
        {
            get { return mstrHospitalID; }
            set { mstrHospitalID = value; }
        }


        /// <summary>
        /// Gets ot sets the selected admit Id.
        /// </summary>
        public Int32 AdmitID
        {
            get { return mintAdmitID; }
            set { mintAdmitID = value; }
        }

        /// <summary>
        /// Gets or sets the ID
        /// </summary>
        public Int32 ID
        {
            get { return mintID; }
            set { mintID = value; }
        }

        /// <summary>
        /// Gets or sets the ASAClassificationCode
        /// </summary>
        public String ASAClassificationCode
        {
            get { return mstrASAClassificationCode; }
            set { mstrASAClassificationCode = value; }
        }

        /// <summary>
        /// Gets or sets the DischargeLocationCode
        /// </summary>
        public String DischargeLocationCode
        {
            get { return mstrDischargeLocationCode; }
            set { mstrDischargeLocationCode = value; }
        }

        /// <summary>
        /// Gets or sets the FacilityCOEID
        /// </summary>
        public String FacilityCOEID
        {
            get { return mstrFacilityCOEID; }
            set { mstrFacilityCOEID = value; }
        }

        /// <summary>
        /// Gets or sets the SurgeonCOEID
        /// </summary>
        public String SurgeonCOEID
        {
            get { return mstrSurgeonCOEID; }
            set { mstrSurgeonCOEID = value; }
        }

        /// <summary>
        /// Sets the ConCurrentProcedureCodes
        /// </summary>
        private String AllConcurrentProcedureCodes
        {
            set
            {
                base.SplitString(value, ref mstrConcurrentProcedureCodes, ";");
            }
        }

        /// <summary>
        /// Gets or sets the ConcurrentProcedureCodes
        /// </summary>
        public String[] ConcurrentProcedureCodes
        {
            get { return mstrConcurrentProcedureCodes; }
        }

        /// <summary>
        /// Gets or sets the DVTProphylaxisTherapyCodes
        /// </summary>
        private String AllDVTProphylaxisTherapyCodes
        {
            set 
            {
                base.SplitString(value, ref mstrDVTProphylaxisTherapyCodes, ";");
            }
        }

        /// <summary>
        /// Gets or sets the DVTProphylaxisTherapyCodes
        /// </summary>
        public String[] DVTProphylaxisTherapyCodes
        {
            get { return mstrDVTProphylaxisTherapyCodes; }
        }

        /// <summary>
        /// Gets and sets the Bariatric Procedure Codes
        /// </summary>
        public String BariatricProcedureCode
        {
            get
            {
                return mstrBariatricProcedureCode;
            }
            set
            {
                mstrBariatricProcedureCode = value;
            }
        }

        /// <summary>
        /// sets the IntraOpAdverseEventCodes
        /// </summary>
        private String AllIntraOpAdverseEventCodes
        {
            set 
            {
                base.SplitString(value, ref mstrIntraOpAdverseEventCodes, ";");
            }
        }

        /// <summary>
        /// Gets the IntraOpAdverseEventCodes
        /// </summary>
        public String[] IntraOpAdverseEventCodes
        {
            get { return mstrIntraOpAdverseEventCodes; }
        }

        /// <summary>
        /// Sets the AdverseEventsBeforeDischarge
        /// </summary>
        private String AllAdverseEventsBeforeDischarge
        {
            set
            {
                base.SplitString(value, ref mstrAdverseEventsBeforeDischarge, ";");
            }
        }

        /// <summary>
        /// Gets or sets the Bariatric Technique Code
        /// </summary>
        public String BariatricTechniqueCode
        {
            get { return mstrBariatricTechniqueCode; }
            set { mstrBariatricTechniqueCode = value; }
        }

        /// <summary>
        /// Gets the AdverseEventsBeforeDischarge
        /// </summary>
        public String[] AdverseEventsBeforeDischarge
        {
            get { return mstrAdverseEventsBeforeDischarge; }
        }

        /// <summary>
        /// Gets or sets the BariatricTechniqueCode
        /// </summary>
        public Decimal BloodTransfusionInUnits 
        {
            get { return mdecBloodTransfusionInUnits; }
            set { mdecBloodTransfusionInUnits = value; }
        }

        /// <summary>
        /// Gets or sets the DateOfAdmission
        /// </summary>
        public DateTime DateOfAdmission
        {
            get { return mdtmDateOfAdmission; }
            set { mdtmDateOfAdmission = value; }
        }

        /// <summary>
        /// Gets or sets the DateOfLastWeight
        /// </summary>
        public DateTime DateOfLastWeight
        {
            get { return mdtmDateOfLastWeight; }
            set { mdtmDateOfLastWeight = value; }
        }

        /// <summary>
        /// Gets or sets the DischargeDate
        /// </summary>
        public DateTime DischargeDate
        {
            get { return mdtmDischargeDate; }
            set { mdtmDischargeDate = value; }
        }

        /// <summary>
        /// Gets or sets the DischargeDate
        /// </summary>
        public DateTime SurgeryDate
        {
            get { return mdtmSurgeryDate; }
            set { mdtmSurgeryDate = value; }
        }

        /// <summary>
        /// Gets or sets the DurationOfAnesthesia
        /// </summary>
        public Decimal DurationOfAnesthesia
        {
            get { return mdecDurationOfAnesthesia; }
            set { mdecDurationOfAnesthesia = value; }
        }

        /// <summary>
        /// Gets or sets the DurationOfSurgery
        /// </summary>
        public Decimal DurationOfSurgery
        {
            get { return mdecDurationOfSurgery; }
            set { mdecDurationOfSurgery = value; }
        }

        /// <summary>
        /// Gets or sets the DurationOfSurgery
        /// </summary>
        public Decimal EstimatedBloodLossInCC
        {
            get { return mdecEstimatedBloodLossInCC; }
            set { mdecEstimatedBloodLossInCC = value; }
        }

        /// <summary>
        /// Gets or sets the LastWeightBeforeSurgery
        /// </summary>
        public Decimal LastWeightBeforeSurgery
        {
            get { return mdecLastWeightBeforeSurgery; }
            set { mdecLastWeightBeforeSurgery = value; }
        }
        
        /// <summary>
        /// Gets or sets the Revision
        /// </summary>
        public Boolean Revision
        {
            get { return mblnRevision; }
            set { mblnRevision  = value; }
        }

        /// <summary>
        /// Gets or sets the SurgicalFellowParticipated
        /// </summary>
        public Boolean SurgicalFellowParticiated
        {
            get { return mblnSurgicalFellowParticiated; }
            set { mblnSurgicalFellowParticiated = value; }
        }

        /// <summary>
        /// Gets or sets the SurgicalResidentParticipated
        /// </summary>
        public Boolean SurgicalResidentParticipated
        {
            get { return mblnSurgicalResidentParticipated; }
            set { mblnSurgicalResidentParticipated = value; }
        }

        /// <summary>
        /// Gets or sets the Time After Surgery.
        /// </summary>
        public Int32 TimeAfterSurgery
        {
            get { return mintTimeAfterSurgery; }
            set { mintTimeAfterSurgery = value; }
        }
        
        /// <summary>
        /// Gets or sets the Time After Surgery Measurement
        /// </summary>
        public String TimeAfterMeasurement
        {
            get { return mstrTimeAfterMeasurement; }
            set { mstrTimeAfterMeasurement = value; }
        }

        /// <summary>
        /// Gets or sets the PreDischarge Surgeon
        /// </summary>
        public String PreDischargeSurgeon
        {
            get { return mstrPreDischargeSurgeon; }
            set { mstrPreDischargeSurgeon = value; }
        }

        private String AllSurgeryCodes
        {
            set { base.SplitString(value, ref mstrSurgeryCodes, ";"); }
        }

        /// <summary>
        /// Gets the SurgeryCodes.
        /// </summary>
        public String[] SurgeryCodes
        {
            get { return mstrSurgeryCodes; }
        }

        /// <summary>
        /// Gets or sets the Bold_Flag
        /// </summary>
        public Boolean Bold_Flag
        {
            get { return mblnBoldFlag; }
            set { mblnBoldFlag = value; }
        }
        #endregion

        #region override
        public override void Clear()
        {
            mintAdmitID = 0;
            mintID = 0;

            mstrHospitalID = String.Empty;
            mstrASAClassificationCode = String.Empty;
            mstrDischargeLocationCode = String.Empty;
            mstrFacilityCOEID  = String.Empty;
            mstrSurgeonCOEID = String.Empty;
            mstrBariatricProcedureCode = String.Empty;
                        
            mdecBloodTransfusionInUnits = 0m;
            mdecDurationOfAnesthesia = 0m;
            mdecDurationOfSurgery = 0m;
            mdecEstimatedBloodLossInCC = 0m;
            mdecLastWeightBeforeSurgery = 0m;

            mdtmDateOfAdmission = DateTime.Now;
            mdtmDateOfLastWeight = DateTime.Now;
            mdtmDischargeDate = DateTime.Now;
            mdtmSurgeryDate = DateTime.Now;

            mblnRevision = false;
            mblnSurgicalFellowParticiated = false;
            mblnSurgicalResidentParticipated = false;
            mblnBoldFlag = false;
            base.Clear();
        }

        protected internal override bool SetProperty(string vName, object vValue)
        {
            bool returnValue = false;

            switch (vName.ToLower())
            {
                case "hospitalid":
                    if (vValue != System.DBNull.Value)
                    {
                        Int32 intTemp = 0;

                        Int32.TryParse(Convert.ToString(vValue), out intTemp);
                        this.ID = intTemp;

                        this.HospitalID = vValue.ToString();
                        returnValue = true;
                    }
                    break;

                case "admitid":
                    if (vValue != System.DBNull.Value)
                    {
                        Int32 intTemp = 0;

                        Int32.TryParse(Convert.ToString(vValue), out intTemp);
                        this.AdmitID = intTemp;
                        returnValue = true;
                    }
                break;

                case "admitdate":
                    if (vValue != System.DBNull.Value)
                    {
                        DateTime dtmTemp;
                        if (DateTime.TryParse(vValue.ToString(), out dtmTemp))
                            this.DateOfAdmission = dtmTemp;
                        else
                            this.DateOfAdmission = DateTime.MinValue;

                        returnValue = true;
                    }
                    break;

                case "surgerydate":
                    if (vValue != System.DBNull.Value)
                    {
                        DateTime dtmTemp;
                        if (DateTime.TryParse(vValue.ToString(), out dtmTemp))
                            this.SurgeryDate  = dtmTemp;
                        else
                            this.SurgeryDate = DateTime.MinValue;

                        returnValue = true;
                    }
                    break;
                    
                case "dischargedate":
                    if (vValue != System.DBNull.Value)
                    {
                        DateTime dtmTemp;
                        if (DateTime.TryParse(vValue.ToString(), out dtmTemp))
                            this.DischargeDate = dtmTemp;
                        else
                            this.DischargeDate = DateTime.MinValue;

                        returnValue = true;
                    }
                    break;

                case "hospitalboldcode" :
                    if (vValue != System.DBNull.Value)
                    {
                        this.FacilityCOEID = vValue.ToString();
                        returnValue = true;
                    }
                    break;
                
                case "doctorboldcode":
                    if (vValue != System.DBNull.Value)
                    {
                        this.SurgeonCOEID = vValue.ToString();
                        returnValue = true;
                    }
                    break;
                /*
                case "HospitalCode":
                    if (vValue != System.DBNull.Value)
                    {
                        this.FacilityCOEID = vValue.ToString();
                        returnValue = true;
                    }
                    break;
                 
                case "surgeonid" :
                    if (vValue != System.DBNull.Value)
                    {
                        this.SurgeonCOEID = vValue.ToString();
                        returnValue = true;
                    }
                    break;
                 */
                case "duration" :
                    if (vValue != System.DBNull.Value)
                    {
                        Decimal decTemp;
                        
                        Decimal.TryParse(vValue.ToString(), out decTemp);
                        this.DurationOfSurgery = decTemp;
                        returnValue = true;
                    }
                    break;

                case "durationofanesthesia" :
                    if (vValue != System.DBNull.Value)
                    {
                        Decimal decTemp;
                        
                        Decimal.TryParse(vValue.ToString(), out decTemp);
                        this.DurationOfAnesthesia = decTemp;
                        returnValue = true;
                    }
                    break;

                case "dischargelocationcode":
                    if (vValue != System.DBNull.Value)
                    {
                        this.DischargeLocationCode = vValue.ToString();
                        returnValue = true;
                    }
                    break;

                case "asaclassificationcode" :
                    if (vValue != System.DBNull.Value)
                    {
                        this.ASAClassificationCode = vValue.ToString();
                        returnValue = true;
                    }
                    break;

                case "dvtprophylaxistherapycodes" :
                    if (vValue != System.DBNull.Value)
                    {
                        this.AllDVTProphylaxisTherapyCodes = vValue.ToString();
                        returnValue = true;
                    }
                    break;

                case "bloodtransfusioninunits" :
                    if (vValue != System.DBNull.Value)
                    {
                        Decimal decTemp;
                        
                        Decimal.TryParse(vValue.ToString(), out decTemp);
                        this.BloodTransfusionInUnits = decTemp;
                        returnValue = true;
                    }
                    break;

                case "lastweightbeforesurgery" :
                    if (vValue != System.DBNull.Value)
                    {
                        Decimal decTemp;
                        
                        Decimal.TryParse(vValue.ToString(), out decTemp);
                        this.LastWeightBeforeSurgery = decTemp;
                        returnValue = true;
                    }
                    break;

                case "estimatedbloodlossincc" :
                    if (vValue != System.DBNull.Value)
                    {
                        Decimal decTemp;

                        Decimal.TryParse(vValue.ToString(), out decTemp);
                        this.EstimatedBloodLossInCC = decTemp;
                        returnValue = true;
                    }
                    break;

                case "concurrentprocedurecodes" :
                    if (vValue != System.DBNull.Value)
                    {
                        this.AllConcurrentProcedureCodes = vValue.ToString();
                        returnValue = true;
                    }
                    break;

                case "bariatricprocedurecode":
                    if (vValue != System.DBNull.Value)
                    {
                        this.BariatricProcedureCode = vValue.ToString();
                        returnValue = true;
                    }
                    break;

                case "intraopadverseeventcodes" :
                    if (vValue != System.DBNull.Value)
                    {
                        this.AllIntraOpAdverseEventCodes = vValue.ToString();
                        returnValue = true;
                    }
                    break;

                case "adverseeventsbeforedischarge":
                    if (vValue != System.DBNull.Value)
                    {
                        this.AllAdverseEventsBeforeDischarge = vValue.ToString();
                        returnValue = true;
                    }
                    break;

                case "dateoflastweight":
                    if (vValue != System.DBNull.Value)
                    {
                        DateTime dtmTemp;
                        if (DateTime.TryParse(vValue.ToString(), out dtmTemp))
                            this.DateOfLastWeight = dtmTemp;
                        else
                            this.DateOfLastWeight = DateTime.MinValue;
                        returnValue = true;
                    }
                    break;

                case "bariatrictechniquecode" :
                    if (vValue != System.DBNull.Value)
                    {
                        this.BariatricTechniqueCode = vValue.ToString();
                        returnValue = true;
                    }
                    break;

                case "surgicalfellowparticipated":
                    if (vValue != System.DBNull.Value)
                    {
                        Boolean blnTemp;
                        if (Boolean.TryParse(vValue.ToString(), out blnTemp))
                            this.SurgicalFellowParticiated = blnTemp;
                        else
                            this.SurgicalFellowParticiated = false;

                        returnValue = true;
                    }
                    break;

                case "surgicalresidentparticipated":
                    if (vValue != System.DBNull.Value)
                    {
                        Boolean blnTemp;
                        if (Boolean.TryParse(vValue.ToString(), out blnTemp))
                            this.SurgicalResidentParticipated = blnTemp;
                        else
                            this.SurgicalResidentParticipated = false;

                        returnValue = true;
                    }
                    break;

                case "timeaftersurgery":
                    if (vValue != System.DBNull.Value)
                    {
                        Int32 intTemp = 0;

                        Int32.TryParse(Convert.ToString(vValue), out intTemp);
                        this.TimeAfterSurgery = intTemp;
                        returnValue = true;
                    }
                    break;

                case "timeaftermeasurement":
                    if (vValue != System.DBNull.Value)
                    {
                        this.TimeAfterMeasurement = Convert.ToString(vValue);
                        returnValue = true;
                    }
                    break;

                case "predischargesurgeonboldcode":
                    if (vValue != System.DBNull.Value)
                    {
                        this.PreDischargeSurgeon = vValue.ToString();
                        returnValue = true;
                    }
                    break;

                case "predischargesurgery":
                    if (vValue != System.DBNull.Value)
                    {
                        this.AllSurgeryCodes = vValue.ToString();
                        returnValue = true;
                    }
                    break;

                case "bold_flag":
                    if (vValue != System.DBNull.Value)
                    {
                        Boolean blnTemp;
                        if (Boolean.TryParse(vValue.ToString(), out blnTemp))
                            this.Bold_Flag = blnTemp;
                        else
                            this.Bold_Flag = false;
                        
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

        #region methods
        public HospitalVisit() { Clear(); }

        /// <summary>
        /// Returns the hospital visit for SRC integration
        /// </summary>
        /// <returns></returns>
        public Boolean LoadHospitalVisitData()
        {
            Database objectDatabase = base.ObjectDatabase;

            DbCommand command = objectDatabase.GetStoredProcCommand(base.GetStoreProcedureName("sp_SRCHospitalVisitGet", false));
            base.ObjectDatabase.AddInParameter(command, "@vintOrganizationCode", DbType.Int32, this.mintOrganizationCode);
            base.ObjectDatabase.AddInParameter(command, "@vintPatientId", DbType.Int32, this.mintPatientID);

            return this.GetProperties(command);
        }


        /// <summary>
        /// Returns the hospital visit for SRC integration
        /// </summary>
        /// <returns></returns>
        public Boolean LoadHospitalVisitDataSingle(Int32 intAdmitID)
        {
            Database objectDatabase = base.ObjectDatabase;

            DbCommand command = objectDatabase.GetStoredProcCommand(base.GetStoreProcedureName("sp_SRCHospitalVisitGetSingle", false));
            base.ObjectDatabase.AddInParameter(command, "@vintOrganizationCode", DbType.Int32, this.mintOrganizationCode);
            base.ObjectDatabase.AddInParameter(command, "@vintPatientId", DbType.Int32, this.mintPatientID);
            base.ObjectDatabase.AddInParameter(command, "@vintAdmitID", DbType.Int32, intAdmitID);

            return this.GetProperties(command);
        }

        /// <summary>
        /// Flag Patient Operation as sent to BOLD
        /// </summary>
        /// <returns></returns>
        public void FlagHospitalVisitData(Int32 organizationCode, Int32 patientID, Int32 admitID, Decimal lastWeight, DateTime lastVisitDate, String hospitalID)
        {
            Database objectDatabase = base.ObjectDatabase;

            DbCommand command = objectDatabase.GetStoredProcCommand(base.GetStoreProcedureName("sp_SRCFlagOpEvents", false));
            base.ObjectDatabase.AddInParameter(command, "@vintOrganizationCode", DbType.Int32, organizationCode);
            base.ObjectDatabase.AddInParameter(command, "@vintPatientId", DbType.Int32, patientID);
            base.ObjectDatabase.AddInParameter(command, "@vintAdmitID", DbType.Int32, admitID);
            base.ObjectDatabase.AddInParameter(command, "@vdecLastWeight", DbType.Decimal, lastWeight);
            base.ObjectDatabase.AddInParameter(command, "@vdtLastSeen", DbType.DateTime, lastVisitDate);
            base.ObjectDatabase.AddInParameter(command, "@vstrHospitalID", DbType.String, hospitalID);

            this.Save(command);
        }
        #endregion 
    }
}
