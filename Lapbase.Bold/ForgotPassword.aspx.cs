using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Web.UI;
using System.Net.Mail;

public partial class _ForgotPassword : Lapbase.Business.BasePage
{
    GlobalClass gClass = new GlobalClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            InitializePage();
        }
        catch (Exception err)
        {
        }

        /*
        if (!IsPostBack)
        {
            try
            {
                InitializePage();
                GetChangeLog();
            }
            catch (Exception err)
            {
                divErrorMessage.Style["display"] = "block";
                divErrorMessage.InnerText = "Lapbase is currently down for maintenance...";
                gClass.AddErrorLogData("0", Request.Url.Host, "", "login Form", "Page_Load function", err.ToString());
            }
        }
        else
            Response.Cookies.Set(new HttpCookie("LanguageCode", cmbLanguage.SelectedValue));
        return;
         * */
    }

    #region private void InitializePage
    /*
     * this function is to initialize some variables which are used during runtime such as connectionstring
     */
    private void InitializePage()
    {
        MakeConnectionString("DBConnectionString");
        return;
    }
    #endregion

    #region private void MakeConnectionString
    private void MakeConnectionString(string strConnection)
    {
        string strConnectionString = System.Configuration.ConfigurationManager.AppSettings[strConnection];
        GlobalClass.strLapbaseCnnString = System.Configuration.ConfigurationManager.AppSettings[strConnection];
        GlobalClass.strSqlCnnString = System.Configuration.ConfigurationManager.ConnectionStrings["SqlDBConnectionString"].ConnectionString;
    }
    #endregion

    #region protected void btnForgotPassword_OnClick(object sender, EventArgs e)
    protected void btnForgotPassword_OnClick(object sender, EventArgs e)
    {
        String newPassword = "";

        String url = Request.Url.Host;
        String userName = txtUserID.Text.Trim();

        if (userName.Length > 0)
        {
            String strScript = String.Empty;
            SqlCommand cmdSelect = new SqlCommand();
            SqlCommand cmdSave = new SqlCommand();
            SqlCommand cmdDelete = new SqlCommand();
            SqlCommand cmdActivate = new SqlCommand();
            DataSet dsResult = new DataSet();
            Int32 UserPracticeCode = 0;
            String UserFirstName = "";
            String UserSurName = "";
            String UserID = "";
            String AdminEmail = "";
            String EmailBody = "";
            String EmailSubject = "";
            String CCEmail = "help@lapbase.com";
            String FromEmail = "no-reply@lapbase.com";
            String AuthUsername = "no-reply@lapbase.com";
            String AuthPassword = "a2x8y2b3";

            try
            {
                gClass.MakeStoreProcedureName(ref cmdSelect, "sp_UsersManagement_CheckUserExist", true);
                cmdSelect.Parameters.Add("@UserID", SqlDbType.VarChar, 25).Value = userName;
                cmdSelect.Parameters.Add("@OrganizationName", SqlDbType.VarChar, 100).Value = Request.Url.Host.ToLower();
                dsResult = gClass.FetchData(cmdSelect, "tblResult");

                if ((dsResult.Tables.Count > 0) && (dsResult.Tables[0].Rows.Count > 0))
                {
                    UserPracticeCode = Convert.ToInt32(dsResult.Tables[0].Rows[0]["UserPracticeCode"].ToString());
                    UserFirstName = dsResult.Tables[0].Rows[0]["User_Name"].ToString();
                    UserSurName = dsResult.Tables[0].Rows[0]["User_Sirname"].ToString();
                    UserID = dsResult.Tables[0].Rows[0]["UserID"].ToString();
                    AdminEmail = dsResult.Tables[0].Rows[0]["AdminEmail"].ToString();

                    //if there are no admin email, send it to ccemail, and empty the cc email
                    if (AdminEmail == "")
                    {
                        AdminEmail = CCEmail;
                        CCEmail = "";
                    }

                    //delete password history & ask user to change it in next login
                    gClass.MakeStoreProcedureName(ref cmdDelete, "sp_UsersManagement_DeletePasswordHistory", false);
                    cmdDelete.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = UserPracticeCode;
                    gClass.ExecuteDMLCommand(cmdDelete);

                    //set new password
                    newPassword = GeneratePassword(6);
                    gClass.MakeStoreProcedureName(ref cmdSave, "sp_UsersManagement_SaveData", false);
                    cmdSave.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = UserPracticeCode;
                    cmdSave.Parameters.Add("@UserID", SqlDbType.VarChar, 25).Value = UserID; //not used
                    cmdSave.Parameters.Add("@UserPW", SqlDbType.VarBinary, 50).Value = gClass.GetSecureBinaryData(newPassword);
                    cmdSave.Parameters.Add("@BlankPwd", SqlDbType.Bit).Value = false; //not used
                    cmdSave.Parameters.Add("@User_Name", SqlDbType.VarChar, 50).Value = UserFirstName; //not used
                    cmdSave.Parameters.Add("@User_SirName", SqlDbType.VarChar, 50).Value = UserSurName; //not used
                    cmdSave.Parameters.Add("@PermissionLevel", SqlDbType.VarChar, 3).Value = ""; //not used
                    cmdSave.Parameters.Add("@UpdatePassword", SqlDbType.Int).Value = 1;
                    cmdSave.Parameters.Add("@SurgeonID", SqlDbType.Int).Value = 0; //not used
                    cmdSave.Parameters.Add("@ShowLog", SqlDbType.Bit).Value = false; //not used
                    cmdSave.Parameters.Add("@ShowRegistry", SqlDbType.Bit).Value = false; //not used
                    gClass.ExecuteDMLCommand(cmdSave);

                    //activate user
                    gClass.MakeStoreProcedureName(ref cmdActivate, "sp_UsersManagement_ActivateUser", false);
                    cmdActivate.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = UserPracticeCode;
                    cmdActivate.Parameters.Add("@UserID", SqlDbType.VarChar, 25).Value = String.Empty; //not used
                    cmdActivate.Parameters.Add("@strOrganizationName", SqlDbType.VarChar, 100).Value = String.Empty; //not used
                    cmdActivate.Parameters.Add("@PermissionFlag", SqlDbType.Int).Value = 1;
                    gClass.ExecuteDMLCommand(cmdActivate);

                    EmailSubject = "Password change request from " + url;
                    EmailBody = UserID + " has requested reactivating their account. Their new password is " + newPassword + ". Please ensure they are made aware of their new password - this has not been sent to them directly. This is a temporary password which will have to be changed to their own when they first login. Thank you<br><br>Lapbase";
                    
                    //if admin empty, send to help
                    //else send to admin and help
                    //SendMail(AuthUsername, AuthPassword, "pard_f@yahoo.com", FromEmail, "", EmailSubject, EmailBody);

                    SendMail(AuthUsername, AuthPassword, AdminEmail, FromEmail, CCEmail, EmailSubject, EmailBody);
                    
                    //email sent notification
                    strScript = "";
                    strScript += "document.getElementById('divErrorMessage').style.display = 'none';";
                    strScript += "document.getElementById('tblContent').style.display = 'none';";
                    strScript += "SetInnerText(document.getElementById('lblForm'), 'Your request to unlock your account is being processed with a new Password being issued. Once issued, your new password will be sent to your Practice Administrator enabling you to unlock your account.');";
                    ScriptManager.RegisterStartupScript(btnForgotPassword, btnForgotPassword.GetType(), "key", strScript, true);
                }
                else
                {
                    //user not exist
                    strScript = "";
                    strScript += "SetInnerText(document.getElementById('pErrorMessage'), 'Your username does not exist');";
                    strScript += "document.getElementById('divErrorMessage').style.display = 'block';";
                    ScriptManager.RegisterStartupScript(btnForgotPassword, btnForgotPassword.GetType(), "key", strScript, true);
                }
            }
            catch (Exception err)
            {
                gClass.AddErrorLogData("User", Context.Request.Url.Host,
                            "User", userName, "Forgot Password or Activate", "" + "" + err.ToString());
            }
        }
    }
    #endregion

    #region private void SendMail(string authUser, string authPassword, string toList, string from, string ccList, string subject, string body)
    private void SendMail(string authUser, string authPassword, string toList, string from, string ccList, string subject, string body)
    {

        MailMessage message = new MailMessage();
        SmtpClient smtpClient = new SmtpClient();

        string msg = string.Empty;
        try
        {
            MailAddress fromAddress = new MailAddress(from);
            message.From = fromAddress;
            message.To.Add(toList);
            if (ccList != null && ccList != string.Empty)
                message.CC.Add(ccList);
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = body;
            // We use gmail as our smtp client
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = true;
            smtpClient.Credentials = new System.Net.NetworkCredential(authUser, authPassword);

            smtpClient.Send(message);
        }
        catch (Exception err)
        {
            gClass.AddErrorLogData("User", Context.Request.Url.Host,"User", "", "Forgot Password or Activate - Error sending Email Notification", "" + "" + err.ToString());
        }
    }
    #endregion

    #region private string GeneratePassword(int length)
    private string GeneratePassword(int length)
    {
        string allowedLetterChars = "abcdefghijkmnpqrstuvwxyz";
        string allowedNumberChars = "23456789";
        char[] chars = new char[length];
        Random rd = new Random();

        bool useLetter = true;
        for (int i = 0; i < length; i++)
        {
            if (useLetter)
            {
                chars[i] = allowedLetterChars[rd.Next(0, allowedLetterChars.Length)];
                useLetter = false;
            }
            else
            {
                chars[i] = allowedNumberChars[rd.Next(0, allowedNumberChars.Length)];
                useLetter = true;
            }

        }
        return new string(chars);
    }
    #endregion
}

