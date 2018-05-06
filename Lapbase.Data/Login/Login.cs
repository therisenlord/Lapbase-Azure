using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Lapbase.Data
{
    public class Login : ObjectBase
    {
        #region enum
        public enum LoginStatus
        {
            UserNotFound = -2
            , UserCredentialIsNotValid = -1
            , UserCredentialIsValid = 0
            , UserCredentialIsSuspended = 1
        }
        #endregion 

        #region Variables
        private String mstrUserID
                        , mstrPassword
                        , mstrPasswordFromDatabase
                        , mstrLanguageCode
                        , mstrCultureInfo
                        , mstrPageDirection
                        , mstrOrganizationName
                        , mstrUserFullName
                        , mstrPermissionLevel
                        , mstrDefaultSort
                        , mstrVisitWeeksFlag
                        , mstrSubmitData
                        , mstrPracticeBoldCode;
                        
        private Int32 mintUserPracticeCode
                        , mintOrganizationCode
                        , mintSurgeonID;
        private Int16 mintValidDays
                        , mintPermission;
        private Boolean mblnLogin
                        , mblnImperial
                        , mblnLoginPage
                        , mblnNewUser
                        , mblnPasswordIsDuplicate
                        , mblnSuperBill
                        , mblnEMR
                        , mblnDataClamp
                        , mblnAutoSave
                        , mblnShowLog
                        , mblnShowRegistry
                        , mblnShowDownloadExcelGraph
                        , mblnExport
                        , mblnBSRExport;
        #endregion

        #region Properties
        
        /// <summary>
        /// Sets the URL for current organization name.
        /// </summary>
        public String OrganizationName
        {
            set { mstrOrganizationName = value; }
        }

        /// <summary>
        /// Sets the User's Name.
        /// </summary>
        public String UserID
        {
            get { return mstrUserID; }
            set { mstrUserID = value.Trim() ; }
        }

        /// <summary>
        /// Sets the User's Password from GUI.
        /// </summary>
        public String Password
        {
            get { return mstrPassword; }
            set { mstrPassword  = HexEncoding.ToString(Lapbase.Data.Common.GetSecureBinaryData(value.Trim())); }
        }

        /// <summary>
        /// Gets or sets the User's password from database.
        /// </summary>
        public String UserPassword
        {
            get { return mstrPasswordFromDatabase; }
            //set { 
            //    //mstrPasswordFromDatabase = HexEncoding.ToString(Lapbase.Data.Common.GetSecureBinaryData(value.Trim()));
            //    mstrPasswordFromDatabase = HexEncoding.ToString((byte[])value).Substring(0, mstrPassword.Length);
            //}
        }

        private byte[] UserPasswordHexCode
        {
            set
            {
                //mstrPasswordFromDatabase = HexEncoding.ToString(Lapbase.Data.Common.GetSecureBinaryData(value.Trim()));
                //mstrPasswordFromDatabase = HexEncoding.ToString((byte[])value).Substring(0, mstrPassword.Length);
                mstrPasswordFromDatabase = HexEncoding.ToString(value).Substring(0, mstrPassword.Length);
            }
        }

        /// <summary>
        /// Sets the Language for current Organization/Domain.
        /// </summary>
        public String LanguageCode
        {
            set { mstrLanguageCode = value; }
        }

        /// <summary>
        /// Returns the result of user's credential process.
        /// </summary>
        public LoginStatus LoggedIn
        {
            get 
            {
                if (mintOrganizationCode == 0)
                    return LoginStatus.UserNotFound;
                else if (mintPermission == 0)
                    return LoginStatus.UserCredentialIsSuspended;
                else
                    if (mstrPassword.Equals(mstrPasswordFromDatabase))
                        return LoginStatus.UserCredentialIsValid;
                    else
                        return LoginStatus.UserCredentialIsNotValid;
            }
        }

        /// <summary>
        /// Sets TRUE if the current page is LOGIN page.
        /// </summary>
        public Boolean LoginPage
        {
            set { mblnLoginPage = value; }
        }

        /// <summary>
        /// Sets the permission for current user.
        /// </summary>
        private Int16 Permission
        {
            set { mintPermission = value; }
        }

        public Int32 UserPracticeCode
        {
            get { return mintUserPracticeCode; }
            set 
            { 
                mintUserPracticeCode = value;
                CheckUserIsNewUser();
            }
        }

        public String UserFullName
        {
            get { return mstrUserFullName; }
            set { mstrUserFullName = value; }
        }

        public String SubmitData
        {
            get { return mstrSubmitData; }
            set { mstrSubmitData = value; }
        }

        public Int32 OrganizationCode
        {
            get { return mintOrganizationCode; }
            set { mintOrganizationCode = value; }
        }

        public String CultureInfo
        {
            get { return mstrCultureInfo; }
            set { mstrCultureInfo = value; }
        }

        public String PageDirection
        {
            get { return mstrPageDirection; }
            set { mstrPageDirection = value; }
        }

        public Int16 ValidDays
        {
            get { return mintValidDays; }
            set { mintValidDays = value; }
        }

        public Boolean Imperial
        {
            get { return mblnImperial; }
            set { mblnImperial = value; }
        }

        public String VisitWeeks
        {
            get { return mstrVisitWeeksFlag; }
            set { mstrVisitWeeksFlag = value; }
        }

        public Boolean NewUser
        {
            get { return mblnNewUser; }
            set { mblnNewUser = value; }
        }

        private Boolean PasswordIsDuplicate
        {
            get { return mblnPasswordIsDuplicate; }
            set { mblnPasswordIsDuplicate = value; }
        }

        public Boolean NewPasswordIsDuplicate
        {
            get 
            {
                return this.PasswordIsDuplicate;
            }
        }

        public Boolean SuperBill
        {
            get { return mblnSuperBill; }
            set { mblnSuperBill = value; }
        }

        public Boolean EMR
        {
            get { return mblnEMR; }
            set { mblnEMR = value; }
        }

        public String PermissionLevel
        {
            get { return mstrPermissionLevel; }
            set { mstrPermissionLevel = value; }
        }

        public Int32 SurgeonID
        {
            get { return mintSurgeonID; }
            set { mintSurgeonID = value; }
        }

        public Boolean DataClamp
        {
            get { return mblnDataClamp; }
            set { mblnDataClamp = value; }
        }
        
        public Boolean AutoSave
        {
            get { return mblnAutoSave; }
            set { mblnAutoSave = value; }
        }
        
        public Boolean ShowLog
        {
            get { return mblnShowLog; }
            set { mblnShowLog = value; }
        }

        public Boolean ShowRegistry
        {
            get { return mblnShowRegistry; }
            set { mblnShowRegistry = value; }
        }

        public Boolean ShowDownloadExcelGraph
        {
            get { return mblnShowDownloadExcelGraph; }
            set { mblnShowDownloadExcelGraph = value; }
        }

        public String DefaultSort
        {
            get { return mstrDefaultSort; }
            set { mstrDefaultSort = value; }
        }
        
        public String PracticeBoldCode
        {
            get { return mstrPracticeBoldCode; }
            set { mstrPracticeBoldCode = value; }
        }
        #endregion

        public Boolean Export
        {
            get { return mblnExport; }
            set { mblnExport = value; }
        }

        public Boolean BSRExport
        {
            get { return mblnBSRExport; }
            set { mblnBSRExport = value; }
        }
        #region overrides
        /// <summary>
        /// Set the Baseline object to an empty instance.
        /// </summary>
        /// <history>
        /// 	<change user="ALI-87AB9129BB0\Administrator" date="24 Jun 2008">Initial version. (CS 2.1.0)</change>
        /// </history>
        public override void Clear()
        {
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

            switch (vName.ToLower())
            {
                case "userpracticecode":
                    if (vValue != System.DBNull.Value)
                    {
                        Int32 intTemp;

                        if (Int32.TryParse(vValue.ToString(), out intTemp))
                        {
                            this.UserPracticeCode = intTemp;
                        }
                        returnValue = true;
                    }
                    break;

                case "userfullname":
                    if (vValue != System.DBNull.Value)
                    {
                        this.UserFullName = vValue.ToString();
                        returnValue = true;
                    }
                    break;

                case "userpw":
                    if (vValue != System.DBNull.Value)
                    {
                        this.UserPasswordHexCode = (byte[])vValue;
                        returnValue = true;
                    }
                    break;

                case "submitdata":
                    if (vValue != System.DBNull.Value)
                    {
                        this.SubmitData = vValue.ToString();
                        returnValue = true;
                    }
                    break;

                case "organizationcode":
                    if (vValue != System.DBNull.Value)
                    {
                        Int32 intTemp = 0;

                        Int32.TryParse(vValue.ToString(), out intTemp);
                        this.OrganizationCode = intTemp;
                        returnValue = true;
                    }
                    break;

                case "cultureinfo" :
                    if (vValue != System.DBNull.Value)
                    {
                        this.CultureInfo = vValue.ToString();
                        returnValue = true;
                    }
                    break;

                case "direction":
                    if (vValue != System.DBNull.Value)
                    {
                        this.PageDirection = vValue.ToString();
                        returnValue = true;
                    }
                    break;

                case "language_code":
                    if (vValue != System.DBNull.Value)
                    {
                        this.LanguageCode = vValue.ToString();
                        returnValue = true;
                    }    
                    break;
                case "validdays" :
                    if (vValue != System.DBNull.Value)
                    {
                        Int16 intTemp = 0;

                        Int16.TryParse(vValue.ToString(), out intTemp);
                        this.ValidDays = intTemp;
                        returnValue = true;
                    }
                    break;

                case "imperial" :
                    if (vValue != System.DBNull.Value)
                    {
                        this.Imperial = vValue.ToString().Equals(String.Empty) || vValue.ToString().Equals(Boolean.TrueString);
                        returnValue = true;
                    }
                    break;

                case "visitweeksflag":
                    if (vValue != System.DBNull.Value)
                    {
                        this.VisitWeeks = vValue.ToString();
                        returnValue = true;
                    }
                    break;

                case "newuser" :
                    if (vValue != System.DBNull.Value)
                    {
                        this.NewUser = vValue.ToString().Equals(Boolean.TrueString);
                        returnValue = true;
                    }
                    break;

                case "permission_flag":
                    if (vValue != System.DBNull.Value)
                    {
                        Int16 intTemp = 0;

                        Int16.TryParse(vValue.ToString(), out intTemp);
                        this.Permission = intTemp;
                        returnValue = true;
                    }
                    break;

                case "passwordisduplicate" :
                    if (vValue != System.DBNull.Value)
                    {
                        this.PasswordIsDuplicate = vValue.ToString().Equals(Boolean.TrueString);
                        returnValue = true;
                    }
                    break;

                case "superbill":
                    if (vValue != System.DBNull.Value)
                    {
                        this.SuperBill = vValue.ToString().Equals(String.Empty) || vValue.ToString().Equals(Boolean.TrueString);
                        returnValue = true;
                    }
                    break;

                case "emr":
                    if (vValue != System.DBNull.Value)
                    {
                        this.EMR = vValue.ToString().Equals(String.Empty) || vValue.ToString().Equals(Boolean.TrueString);
                        returnValue = true;
                    }
                    break;

                case "export":
                    if (vValue != System.DBNull.Value)
                    {
                        this.Export = vValue.ToString().Equals(String.Empty) || vValue.ToString().Equals(Boolean.TrueString);
                        returnValue = true;
                    }
                    break;

                case "bsrexport":
                    if (vValue != System.DBNull.Value)
                    {
                        this.BSRExport = vValue.ToString().Equals(String.Empty) || vValue.ToString().Equals(Boolean.TrueString);
                        returnValue = true;
                    }
                    break;

                case "permissionlevel":
                    if (vValue != System.DBNull.Value)
                    {
                        this.PermissionLevel = vValue.ToString();
                        returnValue = true;
                    }
                    break;
                case "surgeonid":
                    if (vValue != System.DBNull.Value)
                    {
                        Int32 intTemp;

                        if (Int32.TryParse(vValue.ToString(), out intTemp))
                        {
                            this.SurgeonID = intTemp;
                        }
                        returnValue = true;
                    }
                    break;
                case "dataclamp":
                    if (vValue != System.DBNull.Value)
                    {
                        this.DataClamp = vValue.ToString().Equals(String.Empty) || vValue.ToString().Equals(Boolean.TrueString);
                        returnValue = true;
                    }
                    break;
                case "defaultsort":
                    if (vValue != System.DBNull.Value)
                    {
                        this.DefaultSort = vValue.ToString();
                        returnValue = true;
                    }
                    break;
                    
                case "practiceboldcode":
                    if (vValue != System.DBNull.Value)
                    {
                        this.PracticeBoldCode = vValue.ToString();
                        returnValue = true;
                    }
                    break;

                case "autosave":
                    if (vValue != System.DBNull.Value)
                    {
                        this.AutoSave = vValue.ToString().Equals(String.Empty) || vValue.ToString().Equals(Boolean.TrueString);
                        returnValue = true;
                    }
                    break;
                    
                case "showlog":
                    if (vValue != System.DBNull.Value)
                    {
                        this.ShowLog = vValue.ToString().Equals(String.Empty) || vValue.ToString().Equals(Boolean.TrueString);
                        returnValue = true;
                    }
                    break;

                case "showregistry":
                    if (vValue != System.DBNull.Value)
                    {
                        this.ShowRegistry = vValue.ToString().Equals(String.Empty) || vValue.ToString().Equals(Boolean.TrueString);
                        returnValue = true;
                    }
                    break;

                case "showdownloadexcelgraph":
                    if (vValue != System.DBNull.Value)
                    {
                        this.ShowDownloadExcelGraph = vValue.ToString().Equals(String.Empty) || vValue.ToString().Equals(Boolean.TrueString);
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
        public Login()
        {
            ObjectInitialize();
        }

        /// <summary>
        /// Gets user's data.
        /// </summary>
        /// <returns></returns>
        public bool GetUserCredential( )
        {
            DbCommand command = base.ObjectDatabase.GetStoredProcCommand(base.GetStoreProcedureName("sp_Logon_CheckUserCredentials", false));

            base.ObjectDatabase.AddInParameter(command, "@strOrgDomainName", DbType.String, mstrOrganizationName);
            base.ObjectDatabase.AddInParameter(command, "@UserID", DbType.String, mstrUserID);
            base.ObjectDatabase.AddInParameter(command, "@LoginFlag", DbType.Boolean, mblnLoginPage);

            return this.GetProperties(command);
        }

        /// <summary>
        /// Returns TRUE if the user is a new user. 
        /// </summary>
        /// <returns></returns>
        public bool CheckUserIsNewUser()
        {
            DbCommand command = base.ObjectDatabase.GetStoredProcCommand(base.GetStoreProcedureName("sp_UserManagement_IsNewUser", true));

            base.ObjectDatabase.AddInParameter(command, "@UserPracticeCode", DbType.Int32, this.UserPracticeCode);
            return this.GetProperties(command);
        }

        /// <summary>
        /// Checks the new password with user's previous passwords.
        /// </summary>
        /// <returns>TRUE is the new password is duplicate.</returns>
        public Boolean CheckUserPasswordHistory(String strPassword)
        {
            DbCommand command = base.ObjectDatabase.GetStoredProcCommand(base.GetStoreProcedureName("sp_UsersManagement_GetLast5Password", true));
            base.ObjectDatabase.AddInParameter(command, "@UserPracticeCode", DbType.Int32, this.UserPracticeCode);
            base.ObjectDatabase.AddInParameter(command, "@vbytePassword", DbType.Binary, Common.GetSecureBinaryData(strPassword));
            return this.GetProperties(command);
        }

        /// <summary>
        /// Adds the current user's new password into password history
        /// </summary>
        public void UpdatePasswordHistory(String strPassword)
        {
            DbCommand command = base.ObjectDatabase.GetStoredProcCommand(base.GetStoreProcedureName("sp_UsersManagement_UpdatePassword", false));

            base.ObjectDatabase.AddInParameter(command, "@UserPracticeCode", DbType.Int32, this.UserPracticeCode);
            base.ObjectDatabase.AddInParameter(command, "@NewPassword", DbType.Binary, Common.GetSecureBinaryData(strPassword));
            base.ObjectDatabase.ExecuteNonQuery(command);
            return;
        }

        private void ObjectInitialize()
        {
            mstrOrganizationName = String.Empty;
            mstrLanguageCode = String.Empty;
            mstrUserFullName = String.Empty;
            mstrCultureInfo = String.Empty;
            mstrPageDirection = String.Empty;
            mstrLanguageCode = String.Empty;
            mstrPermissionLevel = String.Empty;
            mstrDefaultSort = String.Empty;
            mstrVisitWeeksFlag = String.Empty;
            mstrSubmitData = String.Empty;
            mstrPracticeBoldCode = String.Empty;

            mintUserPracticeCode = 0;
            mintOrganizationCode = 0;
            mintSurgeonID = 0;
            mintValidDays = 0;
            mintPermission = 0;

            mblnLogin = false;
            mblnImperial = false;
            mblnNewUser = false;
            mblnPasswordIsDuplicate = false;
            mblnSuperBill = false;
            mblnEMR = false;
            mblnDataClamp = false;
            mblnAutoSave = false;
            mblnShowLog = false;
            mblnShowRegistry = false;
            mblnBSRExport = false;
            mblnExport = false;
            mblnShowDownloadExcelGraph = false;
            return;
        }
        #endregion
    }
}
