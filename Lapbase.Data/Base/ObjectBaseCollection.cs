using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Data;

/// <summary>
/// ObjectBaseCollection collection Class definition. To be inherited by most business class collections.
/// </summary>
[Serializable()]
public abstract class ObjectBaseCollection<T> : System.Collections.Generic.List<T> where T : Lapbase.Data.ObjectBase
{
    #region Constants

    //TODO: Base class: Modify database factory name if required.
    protected const string mDATABASE_NAME = "Lapbase";

    //TODO: Base class: Modify database schema name if required.
    protected const string mDB_SCHEMA = "[dbo].";

    #endregion

    #region Private Members
    [NonSerializedAttribute()]
    private Database mObjectDatabase = DatabaseFactory.CreateDatabase(mDATABASE_NAME);
    #endregion

    #region Properties

    /// <summary>
    /// Accessor for the ObjectDatabase object.
    /// </summary>
    /// <history>
    /// 	<change user="Ali Farahani" date="15 Aug 2008">Initial version.</change>
    /// </history>
    protected Database ObjectDatabase
    {
        get
        {
            if (mObjectDatabase == null) mObjectDatabase = DatabaseFactory.CreateDatabase(mDATABASE_NAME);
            return mObjectDatabase;
        }
        set
        {
            mObjectDatabase = value;
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Create and initialize the object that must be added to the collection.
    /// </summary>
    /// <returns>An object that is inherited from ObjectBase.</returns>
    protected abstract T CreateCollectionItem();

    /// <summary>
    /// Load the data from the database and populate the collection with objects from the data.
    /// </summary>
    /// <param name="vCommand">DbCommand to populate the list.</param>
    /// <returns>Boolean. True if the list contains objects.</returns>
    /// <remarks></remarks>
    /// <history>
    /// 	<change user="Ali Farahani" date="15 Aug 2008">Initial version (CS2.0.1).</change>
    /// </history>
    protected bool GetProperties(DbCommand vCommand)
    {
        IDataReader dataReader = null;

        try
        {
            this.Clear();
            dataReader = mObjectDatabase.ExecuteReader(vCommand);

            if (dataReader != null)
            {
                //Build the collection of objects from the data returned above.
                T thisBusinessObject;

                while (dataReader.Read())
                {
                    thisBusinessObject = this.CreateCollectionItem();
                    for (Int32 intColumnCount = 0; intColumnCount < dataReader.FieldCount; intColumnCount++)
                    {
                        thisBusinessObject.SetProperty(dataReader.GetName(intColumnCount), dataReader.GetValue(intColumnCount));
                    }
                    this.Add(thisBusinessObject);
                }
            }

            //Return true if there was some data loaded, otherwise return false.
            return (this.Count > 0);
        }
        finally
        {
            if (dataReader != null && !dataReader.IsClosed)
            {
                dataReader.Close();
            }
            dataReader = null;
        }
    }
    #endregion
}