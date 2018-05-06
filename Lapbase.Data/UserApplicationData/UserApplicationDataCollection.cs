using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Lapbase.Data
{
/// <summary>
/// Properties and methods to handle collections of tblUserApplicationData objects.
/// </summary>
/// <history>
/// 	<change user="ALI-87AB9129BB0\Administrator" date="24 Jun 2008">Initial version. (CS 2.1.0)</change>
/// </history>
public class UserApplicationDataCollection : ObjectBaseCollection<UserApplicationData>
{

#region Methods

    /// <summary>
    /// Load all tblUserApplicationData rows from the database.
    /// </summary>
    /// <returns>Boolean. True if one or more tblUserApplicationData objects were loaded.</returns>
	/// <history>
    /// 	<change user="ALI-87AB9129BB0\Administrator" date="24 Jun 2008">Initial version. (CS 2.1.0)</change>
    /// </history>
    public bool Load()
	{
        DbCommand command = base.ObjectDatabase.GetStoredProcCommand("[dbo].[tblUserApplicationDataList]");
        return this.GetProperties(command);
    }

    /// <summary>
    /// Create a new tblUserApplicationData object to add to the collection.
    /// </summary>
    /// <returns>The newly created tblUserApplicationData object.</returns>
    /// <history>
    /// 	<change user="ALI-87AB9129BB0\Administrator" date="24 Jun 2008">Initial version. (CS 2.1.0)</change>
    /// </history>
    protected override UserApplicationData CreateCollectionItem()
	{
        return new tblUserApplicationData();
    }

#endregion

}
}
