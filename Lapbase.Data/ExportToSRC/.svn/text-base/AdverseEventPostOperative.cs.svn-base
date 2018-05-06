using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Lapbase.Data 
{
    public class AdverseEventPostOperative : ObjectBase
    {
        #region variables
        private Int32 mintPatientID
                    , mintOrganizationCode
                    , mintComplicationNum;
        private String mstrAdverseEventCode;
        private String mstrAdverseEventID;
        private DateTime mdtmDateOfEvent;
        private String mstrFacilityCOEID;
        private String mstrSurgeonCOEID;
        private String[] mstrSurgeryCodes;
        #endregion 

        #region Properties
        /// <summary>
        /// Gets or sets the AdverseEventCode.
        /// </summary>
        public String AdverseEventCode
        {
            get { return mstrAdverseEventCode; }
            set { mstrAdverseEventCode = value; }
        }

        /// <summary>
        /// Gets or sets the AdverseEventID.
        /// </summary>
        public String AdverseEventID
        {
            get { return mstrAdverseEventID; }
            set { mstrAdverseEventID = value; }
        }

        /// <summary>
        /// Gets ot sets the selected ComplicationNum.
        /// </summary>
        public Int32 ComplicationNum
        {
            get { return mintComplicationNum; }
            set { mintComplicationNum = value; }
        }

        /// <summary>
        /// Gets or sets the DateOfEvent.
        /// </summary>
        public DateTime DateOfEvent
        {
            get { return mdtmDateOfEvent; }
            set { mdtmDateOfEvent = value; }
        }

        /// <summary>
        /// Gets or sets the FacilityCOEID.
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
        #endregion 

        #region overrides
        public override void Clear()
        {
            mintPatientID = 0;
            mintOrganizationCode = 0;
            mintComplicationNum = 0;
            mstrAdverseEventCode = String.Empty;
            mstrAdverseEventID = String.Empty;
            mdtmDateOfEvent = DateTime.Now;
            mstrFacilityCOEID = null;
            mstrSurgeonCOEID = String.Empty;
            base.Clear();
        }

        protected internal override bool SetProperty(string vName, object vValue)
        {
            bool returnValue = false;

            switch (vName.ToLower())
            {
                case "complicationnum":
                    if (vValue != System.DBNull.Value)
                    {
                        Int32 intTemp = 0;

                        Int32.TryParse(Convert.ToString(vValue), out intTemp);
                        this.ComplicationNum = intTemp;
                        returnValue = true;
                    }
                    break;

                case "dateofevent":
                    if (vValue != System.DBNull.Value)
                    {
                        DateTime dtmTemp;
                        if (DateTime.TryParse(vValue.ToString(), out dtmTemp))
                            this.DateOfEvent = dtmTemp;
                        else
                            this.DateOfEvent = DateTime.MinValue;

                        returnValue = true;
                    }
                    break;

                case "adverseeventcode":
                    if (vValue != System.DBNull.Value)
                    {
                        this.AdverseEventCode = vValue.ToString();
                        returnValue = true;
                    }
                    break;

                case "hospitalboldcode":
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
                case "facilitycoeid":
                    if (vValue != System.DBNull.Value)
                    {
                        this.FacilityCOEID = vValue.ToString();
                        returnValue = true;
                    }
                    break;

                case "surgeoncoeid":
                    if (vValue != System.DBNull.Value)
                    {
                        this.SurgeonCOEID = vValue.ToString();
                        returnValue = true;
                    }
                    break;
                */

                case "surgerycodes":
                    if (vValue != System.DBNull.Value)
                    {
                        this.AllSurgeryCodes = vValue.ToString();
                        returnValue = true;
                    }
                    break;

                case "adverseeventid":
                    if (vValue != System.DBNull.Value)
                    {
                        this.AdverseEventID = vValue.ToString();
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
        public AdverseEventPostOperative()
        {
            this.Clear();
        }

        /// <summary>
        /// Returns the Adverse Event Post Operative data for SRC Integration
        /// </summary>
        /// <returns></returns>
        public Boolean LoadAdverseEventPostOperativeData()
        {
            Database objectDatabase = base.ObjectDatabase;

            DbCommand command = objectDatabase.GetStoredProcCommand(base.GetStoreProcedureName("sp_SRCAdverseEventPostOperativeDataGet", false));
            base.ObjectDatabase.AddInParameter(command, "@vintOrganizationCode", DbType.Int32, this.mintOrganizationCode);
            base.ObjectDatabase.AddInParameter(command, "@vintPatientId", DbType.Int32, this.mintPatientID);

            return this.GetProperties(command);
        }

        /// <summary>
        /// Flag Patient Operation as sent to BOLD
        /// </summary>
        /// <returns></returns>
        public void FlagAdverseEventData(Int32 organizationCode, Int32 patientID, Int32 complicationNum, String adverseEventID)
        {
            Database objectDatabase = base.ObjectDatabase;

            DbCommand command = objectDatabase.GetStoredProcCommand(base.GetStoreProcedureName("sp_SRCFlagAdverseEvent", false));
            base.ObjectDatabase.AddInParameter(command, "@vintOrganizationCode", DbType.Int32, organizationCode);
            base.ObjectDatabase.AddInParameter(command, "@vintPatientId", DbType.Int32, patientID);
            base.ObjectDatabase.AddInParameter(command, "@vintComplicationNum", DbType.Int32, complicationNum);
            base.ObjectDatabase.AddInParameter(command, "@vstrAdverseEventID", DbType.String, adverseEventID);

            this.Save(command);
        }
        #endregion 
    }
}
