using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Lapbase.Data
{
    public class Patient : ObjectBase
    {
        #region variables
        private String mstrPatientCustomId, mstrPermissionLevel = String.Empty;
        private Int32 mintOrganizationCode = 0
                        , mintUserPracticeCode = 0
                        , mintPatientId = 0
                        , mintSurgeonId = 0
                        ;
        private Decimal mdecLastVisitWeight = 0m;
        private Boolean mblnHasVisit = false;
        #endregion

        #region properties
        /// <summary>
        /// Gets or sets whether Patient has visit or not.
        /// </summary>
        public Boolean HasVisit
        {
            get { return mblnHasVisit; }
            set { mblnHasVisit = value; }
        }

        /// <summary>
        /// Gets or sets patient's last visit weight
        /// </summary>
        public Decimal LastVisitWeight
        {
            get { return mdecLastVisitWeight; }
            set { mdecLastVisitWeight = value; }
        }

        /// <summary>
        /// Gets or sets the Patient's Custom Id.
        /// </summary>
        public String PatientCustomId
        {
            get { return mstrPatientCustomId; }
            set { mstrPatientCustomId = value; }
        }

        /// <summary>
        /// Gets or sets Organization code.
        /// </summary>
        public Int32 OrganizationCode
        {
            set { mintOrganizationCode = value; }
            get { return mintOrganizationCode; }
        }

        /// <summary>
        /// Gets or sets User's Practice Code.
        /// </summary>
        public Int32 UserPracticeCode
        {
            set { mintUserPracticeCode = value; }
            get { return mintUserPracticeCode; }
        }

        /// <summary>
        /// Gets or sets Patient's ID
        /// </summary>
        public Nullable<Int32> PatientId
        {
            set { mintPatientId = value.HasValue ? value.Value : 0; }
            get { return mintPatientId; }
        }
        
        /// <summary>
        /// Gets or sets the Surgeon ID
        /// </summary>
        public Int32 SurgeonID
        {
            get { return mintSurgeonId; }
            set { mintSurgeonId = value; }
        }

        /// <summary>
        /// Gets or sets Permission Level
        /// </summary>
        public String PermissionLevel
        {
            set { mstrPermissionLevel = value; }
            get { return mstrPermissionLevel; }
        }


        #endregion

        #region overrides
        public override void Clear()
        {
            mstrPatientCustomId = String.Empty;
            mstrPermissionLevel = String.Empty;
            mintOrganizationCode = 0;
            mintUserPracticeCode = 0;
            mintPatientId = 0;
            mintSurgeonId = 0;

            base.Clear();
        }

        protected internal override bool SetProperty(string vName, object vValue)
        {
            bool returnValue = false;

            switch (vName.ToLower())
            {
                case "patientid":
                    if (vValue != System.DBNull.Value)
                    {
                        Int32 intTemp = 0;
                        Int32.TryParse(vValue.ToString(), out intTemp);
                        this.PatientId = intTemp;
                        returnValue = true;
                    }
                    break;
                case "hasvisit":
                    if (vValue != System.DBNull.Value)
                    {
                        this.HasVisit = vValue.ToString().Equals(Boolean.TrueString);
                        returnValue = true;
                    }
                    break;
                case "patientcustomid" :
                    if (vValue != System.DBNull.Value)
                    {
                        this.PatientCustomId = vValue.ToString();
                        returnValue = true;
                    }
                    break;
                case "organizationcode" :
                    if (vValue != System.DBNull.Value)
                    {
                        Int32 intTemp = 0;
                        Int32.TryParse(vValue.ToString(), out intTemp);
                        this.OrganizationCode = intTemp;
                        returnValue = true;
                    }
                    break;
                case "userpracticecode" :
                    if (vValue != System.DBNull.Value)
                    {
                        Int32 intTemp = 0;
                        Int32.TryParse(vValue.ToString(), out intTemp);
                        this.UserPracticeCode = intTemp;
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
        protected Boolean GetPatientByPatientCustomID()
        {
            Database objectDatabase = base.ObjectDatabase;

            DbCommand command = objectDatabase.GetStoredProcCommand(base.GetStoreProcedureName("sp_PatientData_LoadData", true));
            base.ObjectDatabase.AddInParameter(command, "@OrganizationCode", DbType.Int32, this.OrganizationCode);
            base.ObjectDatabase.AddInParameter(command, "@UserPracticeCode", DbType.Int32, this.UserPracticeCode);
            base.ObjectDatabase.AddInParameter(command, "@PatientID", DbType.Int32, this.PatientId);
            base.ObjectDatabase.AddInParameter(command, "@Patient_CustomID", DbType.Int32, this.PatientCustomId);
            if ((this.PermissionLevel == "1o" || this.PermissionLevel == "2t" || this.PermissionLevel == "3f") && this.SurgeonID > 0)
                base.ObjectDatabase.AddInParameter(command, "@SurgeonID", DbType.Int32, this.SurgeonID);

            return this.GetProperties(command);
        }
        #endregion
    }
}
