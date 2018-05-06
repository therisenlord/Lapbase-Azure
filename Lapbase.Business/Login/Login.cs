using System;
using System.Collections.Generic;
using System.Text;

namespace Lapbase.Business
{
    public class Login : Lapbase.Data.Login
    {
        #region variables
        private String mstrMessage;
        #endregion 

        #region properties
        /// <summary>
        /// Gets the message content.
        /// </summary>
        public String MessageContent
        {
            get{return mstrMessage;}
        }
        #endregion 

        #region Methods
        /// <summary>
        /// Checks the user's credential.
        /// </summary>
        /// <returns>TRUE if the user's credential are correct.</returns>
        public Boolean CheckUserCredential()
        {
            Boolean Loginflag = true;
            Lapbase.Data.LapbaseMessage objLapbaseMessage = new Lapbase.Data.LapbaseMessage();
            
            if (base.UserID.Equals(String.Empty) || base.Password.Equals(String.Empty))
            {
                objLapbaseMessage.GetMessage(Lapbase.Data.LapbaseMessage.MessageType.NotCompleteData);
                mstrMessage = objLapbaseMessage.MessageContent;
                Loginflag = false;
            }
            else
            {
                base.GetUserCredential();
                switch (base.LoggedIn)
                {
                    case LoginStatus.UserNotFound :
                        objLapbaseMessage.GetMessage(Lapbase.Data.LapbaseMessage.MessageType.NotFoundData);
                        mstrMessage = objLapbaseMessage.MessageContent;
                        Loginflag = false;
                        break;

                    case LoginStatus.UserCredentialIsNotValid:
                        objLapbaseMessage.GetMessage(Lapbase.Data.LapbaseMessage.MessageType.NotValidData);
                        mstrMessage = objLapbaseMessage.MessageContent;
                        Loginflag = false;
                        break;

                    case LoginStatus.UserCredentialIsValid:
                        Loginflag = true;
                        break;

                    case LoginStatus.UserCredentialIsSuspended:
                        objLapbaseMessage.GetMessage(Lapbase.Data.LapbaseMessage.MessageType.SuspendedData);
                        mstrMessage = objLapbaseMessage.MessageContent;
                        Loginflag = false;
                        break;
                }
            }
            
            return Loginflag;
        }

        /// <summary>
        /// Checks the new user's password with his/her previous passwords.
        /// </summary>
        /// <returns></returns>
        public new Boolean CheckUserPasswordHistory(String strNewPassword)
        {
            base.CheckUserPasswordHistory(strNewPassword);
            return base.NewPasswordIsDuplicate;
        }

        /// <summary>
        /// Updates current user's password history
        /// </summary>
        public new void UpdatePasswordHistory(String strNewPassword)
        {
            base.UpdatePasswordHistory(strNewPassword);
        }
        #endregion
    }
}
