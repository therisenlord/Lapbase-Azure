using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

namespace Lapbase.Data 
{
    public class AdverseEventPostOperativeCollection : ObjectBaseCollection<AdverseEventPostOperative>
    {
        /// <summary>
        /// This function is to load all Operative Visit (Pre-operative
        /// </summary>
        /// <param name="mintOrganizationCode">Organization Code - Interger value</param>
        /// <param name="mintPatientID">Patient's ID - Integer value</param>
        /// <returns></returns>
        public Boolean Load(Int32 mintOrganizationCode, Int32 mintPatientID)
        {
            /** /
            String strSQL = "sp_SRCOperativeVisitDataGet";
            
            DbCommand command = base.ObjectDatabase.GetStoredProcCommand(strSQL);
            base.ObjectDatabase.AddInParameter(command, "@vintOrganizationCode", DbType.Int32, mintOrganizationCode);
            base.ObjectDatabase.AddInParameter(command, "@vintPatientId", DbType.Int32, mintPatientID);
            base.ObjectDatabase.AddInParameter(command, "@vblnImperial", DbType.Boolean, mblnImperial);
            base.ObjectDatabase.AddInParameter(command, "@vblnPreOperative", DbType.Boolean, mVisitType == Common.VisitType.PreOperative);
            return this.GetProperties(command);

            Database objectDatabase = base.ObjectDatabase;
            /**/

            //DbCommand command = base.ObjectDatabase.GetStoredProcCommand(base.GetStoreProcedureName("sp_SRCAdverseEventPostOperativeDataGet", false));
            DbCommand command = base.ObjectDatabase.GetStoredProcCommand("sp_SRCAdverseEventPostOperativeDataGet");
            base.ObjectDatabase.AddInParameter(command, "@vintOrganizationCode", DbType.Int32, mintOrganizationCode);
            base.ObjectDatabase.AddInParameter(command, "@vintPatientId", DbType.Int32, mintPatientID);

            return this.GetProperties(command);
        }

        /// <summary>
        /// Create a new Adverse Event Post Operative object.
        /// </summary>
        /// <returns></returns>
        protected override AdverseEventPostOperative CreateCollectionItem()
        {
            return new AdverseEventPostOperative();
        }
    }
}
