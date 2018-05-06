using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Lapbase.Data
{
/// <summary>
/// Properties and methods to handle the tblUserApplicationData object.
/// </summary>
/// <history>
/// 	<change user="ALI-87AB9129BB0\Administrator" date="24 Jun 2008">Initial version. (CS 2.1.0)</change>
/// </history>
public class UserApplicationData : ObjectBase
{

#region Private Members 
	
	private string mSessionID;
	private Nullable<int> mOrganizationCode;
	private Nullable<int> mUserPracticeCode;
	private Nullable<int> mPatientID;
	private Nullable<int> mCunsultID;
	private Nullable<int> mOperationID;
	private Nullable<int> mComplicationID;
	private bool mImperial;
	private string mLanguageCode;
	private string mCultureInfo;
	private string mDirection;
	private string mVisitWeeksFlag;
    private string mUserName;

#endregion

#region Properties

	/// <summary>
	/// Gets or sets the SessionID. 
	/// </summary>
	/// <history>
	/// 	<change user="ALI-87AB9129BB0\Administrator" date="24 Jun 2008">Initial version. (CS 2.1.0)</change>
	/// </history>
	public string SessionID 
	{
		get
		{
			return mSessionID;
		}
		set
		{
			mSessionID = value;
		}
	}

	/// <summary>
	/// Gets or sets the OrganizationCode. 
	/// </summary>
	/// <history>
	/// 	<change user="ALI-87AB9129BB0\Administrator" date="24 Jun 2008">Initial version. (CS 2.1.0)</change>
	/// </history>
	public Nullable<int> OrganizationCode 
	{
		get
		{
			return mOrganizationCode;
		}
		set
		{
			mOrganizationCode = value;
		}
	}

	/// <summary>
	/// Gets or sets the UserPracticeCode. 
	/// </summary>
	/// <history>
	/// 	<change user="ALI-87AB9129BB0\Administrator" date="24 Jun 2008">Initial version. (CS 2.1.0)</change>
	/// </history>
	public Nullable<int> UserPracticeCode 
	{
		get
		{
			return mUserPracticeCode;
		}
		set
		{
			mUserPracticeCode = value;
		}
	}

	/// <summary>
	/// Gets or sets the PatientID. 
	/// </summary>
	/// <history>
	/// 	<change user="ALI-87AB9129BB0\Administrator" date="24 Jun 2008">Initial version. (CS 2.1.0)</change>
	/// </history>
	public Nullable<int> PatientID 
	{
		get
		{
			return mPatientID;
		}
		set
		{
			mPatientID = value;
		}
	}

	/// <summary>
	/// Gets or sets the CunsultID. 
	/// </summary>
	/// <history>
	/// 	<change user="ALI-87AB9129BB0\Administrator" date="24 Jun 2008">Initial version. (CS 2.1.0)</change>
	/// </history>
	public Nullable<int> CunsultID 
	{
		get
		{
			return mCunsultID;
		}
		set
		{
			mCunsultID = value;
		}
	}

	/// <summary>
	/// Gets or sets the OperationID. 
	/// </summary>
	/// <history>
	/// 	<change user="ALI-87AB9129BB0\Administrator" date="24 Jun 2008">Initial version. (CS 2.1.0)</change>
	/// </history>
	public Nullable<int> OperationID 
	{
		get
		{
			return mOperationID;
		}
		set
		{
			mOperationID = value;
		}
	}

	/// <summary>
	/// Gets or sets the ComplicationID. 
	/// </summary>
	/// <history>
	/// 	<change user="ALI-87AB9129BB0\Administrator" date="24 Jun 2008">Initial version. (CS 2.1.0)</change>
	/// </history>
	public Nullable<int> ComplicationID 
	{
		get
		{
			return mComplicationID;
		}
		set
		{
			mComplicationID = value;
		}
	}

	/// <summary>
	/// Gets or sets the Imperial. 
	/// </summary>
	/// <history>
	/// 	<change user="ALI-87AB9129BB0\Administrator" date="24 Jun 2008">Initial version. (CS 2.1.0)</change>
	/// </history>
	public bool Imperial 
	{
		get
		{
			return mImperial;
		}
		set
		{
			mImperial = value;
		}
	}

	/// <summary>
	/// Gets or sets the LanguageCode. 
	/// </summary>
	/// <history>
	/// 	<change user="ALI-87AB9129BB0\Administrator" date="24 Jun 2008">Initial version. (CS 2.1.0)</change>
	/// </history>
	public string LanguageCode 
	{
		get
		{
			return mLanguageCode;
		}
		set
		{
			mLanguageCode = value;
		}
	}

	/// <summary>
	/// Gets or sets the CultureInfo. 
	/// </summary>
	/// <history>
	/// 	<change user="ALI-87AB9129BB0\Administrator" date="24 Jun 2008">Initial version. (CS 2.1.0)</change>
	/// </history>
	public string CultureInfo 
	{
		get
		{
			return mCultureInfo;
		}
		set
		{
			mCultureInfo = value;
		}
	}

	/// <summary>
	/// Gets or sets the Direction. 
	/// </summary>
	/// <history>
	/// 	<change user="ALI-87AB9129BB0\Administrator" date="24 Jun 2008">Initial version. (CS 2.1.0)</change>
	/// </history>
	public string Direction 
	{
		get
		{
			return mDirection;
		}
		set
		{
			mDirection = value;
		}
	}

	/// <summary>
	/// Gets or sets the VisitWeeksFlag. 
	/// </summary>
	/// <history>
	/// 	<change user="ALI-87AB9129BB0\Administrator" date="24 Jun 2008">Initial version. (CS 2.1.0)</change>
	/// </history>
	public string VisitWeeksFlag 
	{
		get
		{
			return mVisitWeeksFlag;
		}
		set
		{
			mVisitWeeksFlag = value;
		}
	}

    /// <summary>
    /// Gets or sets the UserName. 
    /// </summary>
    /// <history>
    /// 	<change user="ALI-87AB9129BB0\Administrator" date="24 Jun 2008">Initial version. (CS 2.1.0)</change>
    /// </history>
    public string UserName
    {
        get
        {
            return mUserName;
        }
        set
        {
            mUserName = value;
        }
    }

#endregion

#region Methods

#region Override
	
	/// <summary>
	/// Set the tblUserApplicationData object to an empty instance.
	/// </summary>
	/// <history>
	/// 	<change user="ALI-87AB9129BB0\Administrator" date="24 Jun 2008">Initial version. (CS 2.1.0)</change>
	/// </history>
	public override void Clear()
	{
		mSessionID = string.Empty;
		mOrganizationCode = null;
		mUserPracticeCode = null;
		mPatientID = null;
		mCunsultID = null;
		mOperationID = null;
		mComplicationID = null;
		mImperial = false;
		mLanguageCode = string.Empty;
		mCultureInfo = string.Empty;
		mDirection = string.Empty;
		mVisitWeeksFlag = string.Empty;
        mUserName = string.Empty;
	
		base.Clear();
	}
	
    /// <summary>
    /// Uses a given data set cell (from a dataset, datareader ...) to set one of the tblUserApplicationData object property.
    /// </summary>
    /// <param name="vName">Name of the column holding the data.</param>
    /// <param name="vValue">Value used to set one of the object properties.</param>
    /// <returns>Boolean. True if the vName parameter has been found and processed and is representative of the object/record.</returns>
    /// <history>
	/// 	<change user="AFarahani" date="24 Jun 2008">Initial version. (CS 2.1.0)</change>
    /// </history>
    protected internal override bool SetProperty(string vName, object vValue)
	{
        bool returnValue = false;

        switch(vName.ToLower())
		{
            case "sessionid":
                if(vValue != System.DBNull.Value)
				{
                    mSessionID = Convert.ToString(vValue);
                }
				break;

            case "organizationcode":
                if(vValue != System.DBNull.Value)
				{
                    mOrganizationCode = Convert.ToInt32(vValue);
                    returnValue = true;
                }
				break;

            case "userpracticecode":
                if(vValue != System.DBNull.Value)
				{
                    mUserPracticeCode = Convert.ToInt32(vValue);
                    returnValue = true;
                }
				break;

            case "patientid":
                if(vValue != System.DBNull.Value)
				{
                    mPatientID = Convert.ToInt32(vValue);
                }
				break;

            case "cunsultid":
                if(vValue != System.DBNull.Value)
				{
                    mCunsultID = Convert.ToInt32(vValue);
                }
				break;

            case "operationid":
                if(vValue != System.DBNull.Value)
				{
                    mOperationID = Convert.ToInt32(vValue);
                }
				break;

            case "complicationid":
                if(vValue != System.DBNull.Value)
				{
                    mComplicationID = Convert.ToInt32(vValue);
                }
				break;

            case "imperial":
                if(vValue != System.DBNull.Value)
				{
                    mImperial = Convert.ToBoolean(vValue);
                }
				break;

            case "languagecode":
                if(vValue != System.DBNull.Value)
				{
                    mLanguageCode = Convert.ToString(vValue);
                }
				break;

            case "cultureinfo":
                if(vValue != System.DBNull.Value)
				{
                    mCultureInfo = Convert.ToString(vValue);
                }
				break;

            case "direction":
                if(vValue != System.DBNull.Value)
				{
                    mDirection = Convert.ToString(vValue);
                }
				break;

            case "visitweeksflag":
                if(vValue != System.DBNull.Value)
				{
                    mVisitWeeksFlag = Convert.ToString(vValue);
                }
				break;
            case "username":
                if (vValue != System.DBNull.Value)
                {
                    mUserName = Convert.ToString(vValue);
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

	/// <summary>
	/// Load a single tblUserApplicationData object from the database.
	/// </summary>
	/// <param name="vOrganizationCode"></param>
	/// <param name="vUserPracticeCode"></param>
	/// <returns>Boolean. True if the tblUserApplicationData was loaded; False otherwise.</returns>
	/// <history>
	/// 	<change user="ALI-87AB9129BB0\Administrator" date="24 Jun 2008">Initial version. (CS 2.1.0)</change>
	///	</history>
	public bool Load(Nullable<int> vOrganizationCode, Nullable<int> vUserPracticeCode) 
	{
		string SQL = base.GetStoreProcedureName("UserApplicationDataGet", false);
		DbCommand command = base.ObjectDatabase.GetStoredProcCommand(SQL);
		
		base.ObjectDatabase.AddInParameter(command, "@vintOrganizationCode", DbType.Int32, vOrganizationCode);
		base.ObjectDatabase.AddInParameter(command, "@vintUserPracticeCode", DbType.Int32, vUserPracticeCode);
        return this.GetProperties(command);
	}

    /// <summary>
    /// Load a single tblUserApplicationData object from the database using SessionID.
    /// </summary>
    public bool Load(String vSessionID)
    {
        string SQL = base.GetStoreProcedureName("sp_UserApplicationDataGetBySessionID", false);
        DbCommand command = base.ObjectDatabase.GetStoredProcCommand(SQL);
		
		base.ObjectDatabase.AddInParameter(command, "@vstrSessionID", DbType.String, vSessionID);
        return this.GetProperties(command);
    }
    
	/// <summary>
	/// Save the tblUserApplicationData object back to the database.
	/// </summary>
	/// <history>
	/// 	<change user="ALI-87AB9129BB0\Administrator" date="24 Jun 2008">Initial version. (CS 2.1.0)</change>
	/// </history>
	public void Save() 
	{
        string sql = base.GetStoreProcedureName("sp_UserApplicationDataSave", false);
		DbCommand command = base.ObjectDatabase.GetStoredProcCommand(sql);

		base.ObjectDatabase.AddInParameter(command, "@vstrSessionID", DbType.String, mSessionID);
		base.ObjectDatabase.AddInParameter(command, "@vintOrganizationCode", DbType.Int32, mOrganizationCode);
		base.ObjectDatabase.AddInParameter(command, "@vintUserPracticeCode", DbType.Int32, mUserPracticeCode);
		base.ObjectDatabase.AddInParameter(command, "@vintPatientID", DbType.Int32, mPatientID);
		base.ObjectDatabase.AddInParameter(command, "@vintCunsultID", DbType.Int32, mCunsultID);
		base.ObjectDatabase.AddInParameter(command, "@vintOperationID", DbType.Int32, mOperationID);
		base.ObjectDatabase.AddInParameter(command, "@vintComplicationID", DbType.Int32, mComplicationID);
		base.ObjectDatabase.AddInParameter(command, "@vblnImperial", DbType.Boolean, mImperial);
		base.ObjectDatabase.AddInParameter(command, "@vstrLanguageCode", DbType.String, mLanguageCode);
		base.ObjectDatabase.AddInParameter(command, "@vstrCultureInfo", DbType.String, mCultureInfo);
		base.ObjectDatabase.AddInParameter(command, "@vstrDirection", DbType.String, mDirection);
		base.ObjectDatabase.AddInParameter(command, "@vstrVisitWeeksFlag", DbType.String, mVisitWeeksFlag);
        base.ObjectDatabase.AddInParameter(command, "@vstrUserName", DbType.String, mUserName);

		base.Save(command);
	}

	/// <summary>
	/// Delete a single tblUserApplicationData row from the database.
	/// </summary>
	/// <param name="vOrganizationCode"></param>
	/// <param name="vUserPracticeCode"></param>
	/// <returns>Boolean. True if the tblUserApplicationData row was deleted; False otherwise.</returns>
	/// <history>
	/// 	<change user="ALI-87AB9129BB0\Administrator" date="24 Jun 2008">Initial version. (CS 2.1.0)</change>
	/// </history>
	public bool Delete(Nullable<int> vOrganizationCode, Nullable<int> vUserPracticeCode)
	{
		string SQL = base.GetStoreProcedureName("UserApplicationDataDelete", false);
		int recordsAffected = 0;
		Database objectDatabase = DatabaseFactory.CreateDatabase(mDATABASE_NAME);
		DbCommand command = objectDatabase.GetStoredProcCommand(SQL);
		
		objectDatabase.AddInParameter(command, "@vintOrganizationCode", DbType.Int32, vOrganizationCode);
		objectDatabase.AddInParameter(command, "@vintUserPracticeCode", DbType.Int32, vUserPracticeCode);
		try
		{
			recordsAffected = Convert.ToInt32(objectDatabase.ExecuteScalar(command));
		}
		catch(SqlException exception)
		{
		}
		
		return (recordsAffected > 0);
	}

#endregion

}
}
