using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;


namespace Lapbase.Data
{
    public class LapbaseMessage : ObjectBase
    {
        #region variables
        private String mstrMessageContent;
        const String M_NOT_COMPLETE_DATA = "E0001"
                    , M_NOT_VALID_DATA = "E0002"
                    , M_SUSPENDED_DATA = "E0003"
                    , M_NOT_FOUND_DATA = "E0007";
        #endregion 

        #region enums
        public enum MessageType{
            NotFoundData
            , NotCompleteData 
            , NotValidData 
            , SuspendedData 
        }
        #endregion 

        #region properties
        /// <summary>
        /// Gets the message content.
        /// </summary>
        public String MessageContent
        {
            get { return mstrMessageContent; }
            set { mstrMessageContent = value; }
        }
        #endregion 

        #region overrides
        public override void  Clear()
        {
            mstrMessageContent = String.Empty;
 	        base.Clear();
        }

        protected internal override bool SetProperty(string vName, object vValue)
        {
            bool returnValue = false;

            switch (vName.ToLower())
            {
                case "messagecontent":
                    if (vValue != System.DBNull.Value)
                    {
                        this.MessageContent = Convert.ToString(vValue);
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
        public bool GetMessage(MessageType MessageCode)
        {
            String strMessageCode = String.Empty;
            switch (MessageCode)
            {
                case MessageType.NotFoundData:
                    strMessageCode = M_NOT_FOUND_DATA;
                    break;
                case MessageType.NotCompleteData:
                    strMessageCode = M_NOT_COMPLETE_DATA;
                    break;
                case MessageType.NotValidData:
                    strMessageCode = M_NOT_VALID_DATA;
                    break;
                case MessageType.SuspendedData:
                    strMessageCode = M_SUSPENDED_DATA;
                    break;
            }

            DbCommand command = base.ObjectDatabase.GetStoredProcCommand(base.GetStoreProcedureName("sp_GetLapbaseMessage", false));

            base.ObjectDatabase.AddInParameter(command, "@vstrMessageCode", DbType.String, strMessageCode);
            return this.GetProperties(command);
        }
        #endregion 
    }
}
