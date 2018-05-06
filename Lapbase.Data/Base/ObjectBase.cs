using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace Lapbase.Data
{
    public class ObjectBase
    {
        #region constants
        private const String COMMA_PATTERN = ",";
        protected const String mDATABASE_NAME = "Lapbase";
        protected const String mDB_SCHEMA = "[dbo].";
        #endregion

        #region Private Members
        [NonSerializedAttribute()]
        private Database mObjectDatabase = DatabaseFactory.CreateDatabase(mDATABASE_NAME);
        private bool mNewRecord = true;
        #endregion

        #region Properties
        /// <summary>
        /// Accessor for the ObjectDatabase object.
        /// </summary>
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

        /// <summary>
        /// Indicates whether this is a new or previously existing record.
        /// </summary>
        protected bool NewRecord
        {
            get
            {
                return mNewRecord;
            }
            set
            {
                mNewRecord = value;
            }
        }

        #endregion 

        #region Methods
        /// <summary>
        /// Returns the Stored Procedure Name with Prefix (VersionNo) if Prefix is TRUE.
        /// </summary>
        /// <param name="strStoredProcedureName">Core Stored Procedure Name</param>
        /// <param name="Prefix">Has Prefix (Version No)</param>
        /// <returns></returns>
        protected String GetStoreProcedureName(String strStoredProcedureName, Boolean Prefix)
        {
            String strName;
            if (Prefix)
                strName = String.Format("{0}{1}", 
                            mDB_SCHEMA ,
                            System.Configuration.ConfigurationManager.AppSettings["ApplicationVersion"].Replace(".", "_") + "_" + strStoredProcedureName);
            else
                strName = String.Format("{0}{1}", mDB_SCHEMA, strStoredProcedureName);

            return strName; 
        }

        public virtual void Clear(){}

        protected internal virtual bool SetProperty(string vName, object vValue)
        {
            return false;
        }

        protected virtual void Save(DbCommand rCommand)
        {
            this.GetProperties(rCommand);
        }

        protected virtual bool GetProperties(DbCommand vCommand)
        {
            return this.GetProperties(true, vCommand);
        }

        protected virtual bool GetProperties(bool vClear, DbCommand vCommand)
        {
            bool instanceLoaded = false;
            bool isNextResult = true;
            IDataReader dataReader = null;

            try
            {
                if (vClear)
                {
                    this.Clear();
                }

                dataReader = mObjectDatabase.ExecuteReader(vCommand);

                if (dataReader != null)
                {
                    while (isNextResult)
                    {
                        //Build the collection of objects from the data returned above.
                        while (dataReader.Read())
                        {
                            for (Int32 intCurrentColumn = 0; intCurrentColumn < dataReader.FieldCount; intCurrentColumn++)
                            {
                                if (this.SetProperty(dataReader.GetName(intCurrentColumn), dataReader.GetValue(intCurrentColumn)))
                                {
                                    instanceLoaded = true;
                                }
                            }
                        }
                        isNextResult = dataReader.NextResult();
                    }
                }
            }
            finally
            {
                if (dataReader != null && !dataReader.IsClosed)
                {
                    dataReader.Close();
                }

                dataReader = null;
            }

            //Return true if there was some data loaded, otherwise return false.
            return instanceLoaded;
        }

        /// <summary>
        /// Returns a splitted string into Array of String.
        /// </summary>
        /// <param name="strSource">Input String having all codes separated with ','</param>
        /// <param name="strReturn">Array of splitted string.</param>
        public void SplitString(String strSource, ref String[] strReturn)
        {
            RemoveLastSeparator(ref strSource, COMMA_PATTERN);
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(COMMA_PATTERN);
            strReturn = regex.Split(strSource);
        }

        /// <summary>
        /// Returns a splitted string into Array of String.
        /// </summary>
        /// <param name="strSource">Input String having all codes separated with ','</param>
        /// <param name="strReturn">Array of splitted string.</param>
        /// <param name="strSeparator"></param>
        public void SplitString(String strSource, ref String[] strReturn, String strSeparator)
        {

            if (!object.ReferenceEquals(strSource, String.Empty) || strSource.Length != 0)
            {
                RemoveLastSeparator(ref strSource, strSeparator);
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(strSeparator);
                strReturn = regex.Split(strSource);
            }
            else
                strSource = null;
        }

        /// <summary>
        /// Removes the last Separator from end of Source.
        /// </summary>
        /// <param name="strSource"></param>
        /// <param name="strSeparator"></param>
        private void RemoveLastSeparator(ref String strSource, String strSeparator)
        {
            if (! strSource.Equals(String.Empty) && (strSource.LastIndexOf(strSeparator) == strSource.Length - 1))
                strSource = strSource.Substring(0, strSource.LastIndexOf(strSeparator));
        }
        #endregion
    }
}
