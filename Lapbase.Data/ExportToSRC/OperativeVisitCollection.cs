using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

namespace Lapbase.Data
{
    public class OperativeVisitCollection : ObjectBaseCollection<OperativeVisit>
    {
        /// <summary>
        /// This function is to load all Operative Visit (Pre-operative
        /// </summary>
        /// <returns></returns>
        public Boolean Load(Int32 mintOrganizationCode, Int32 mintPatientID, Boolean mblnImperial, Lapbase.Data.Common.VisitType mVisitType)
        {
            String strSQL = "sp_SRCOperativeVisitDataGet";
            /*
            switch (mVisitType)
            {
                case Common.VisitType.PreOperative :
                    strSQL = "sp_SRCPreOperativeVisitDataGet"; 
                    break;
                case Common.VisitType.PostOperative :
                    strSQL = "sp_SRCPostOperativeVisitDataGet";
                    break;
            }
             */
            DbCommand command = base.ObjectDatabase.GetStoredProcCommand(strSQL);
            base.ObjectDatabase.AddInParameter(command, "@vintOrganizationCode", DbType.Int32, mintOrganizationCode);
            base.ObjectDatabase.AddInParameter(command, "@vintPatientId", DbType.Int32, mintPatientID);
            base.ObjectDatabase.AddInParameter(command, "@vblnImperial", DbType.Boolean, mblnImperial);
            base.ObjectDatabase.AddInParameter(command, "@vblnPreOperative", DbType.Boolean, mVisitType == Common.VisitType.PreOperative);
            return this.GetProperties(command);
        }

        /// <summary>
        /// Create a new Operative Visit object.
        /// </summary>
        /// <returns></returns>
        protected override OperativeVisit CreateCollectionItem()
        {
            return new OperativeVisit();
        }
    }
}
