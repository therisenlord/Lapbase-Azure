using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;

namespace Lapbase.Data
{
    [Serializable]
    public class Domain : ObjectBase
    {
        #region modules
        public bool CheckDomainURL(String strDomainURL, Nullable<Int32> intUserPracticeCode)
        {
            bool result = false;
            Database objectDatabase = base.ObjectDatabase;

            DbCommand command = objectDatabase.GetStoredProcCommand(base.GetStoreProcedureName("sp_UsersManagement_CheckUserDomain", false));
            base.ObjectDatabase.AddInParameter(command, "@strDomainName", DbType.String, strDomainURL);
            base.ObjectDatabase.AddInParameter(command, "@UserPracticeCode", DbType.Int32, intUserPracticeCode.Value);
            base.ObjectDatabase.AddOutParameter(command, "@rblnIsDomainOK", DbType.Boolean, 1);

            objectDatabase.ExecuteNonQuery(command);

            result = Convert.ToBoolean(objectDatabase.GetParameterValue(command, "@rblnIsDomainOK"));

            return result;
        }
        #endregion
    }
}
