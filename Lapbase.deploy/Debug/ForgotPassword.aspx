<%@ page language="C#" autoeventwireup="true" inherits="_ForgotPassword, Lapbase.deploy" enableviewstate="true" enableEventValidation="false" viewStateEncryptionMode="Always" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix = "wucTextBox" TagName = "TextBox" Src = "~/UserControl/TextBoxWUCtrl.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1" />
    <%--<link href="http://lapbase.com/app_css/v2/admin_common.css" rel="stylesheet" type="text/css" />--%>
    <link href='<%$ AppSettings:CssUrl%>' rel="stylesheet" type="text/css" />
    <script language="javascript" src = "Scripts/Global.js" type="text/javascript" ></script>
    <title>LapBase - A Data Manager for Bariatric Surgery</title>
</head>
<body runat = "server" id="bodyLogon" >
    <form id="frmLogon" runat="server">
        <asp:ScriptManager runat = "server" ID = "scriptManager1" AsyncPostBackTimeout="60" />
        <div class="loginPage">
            <div class="logoArea">
                <img src="img/logo.gif" alt="" />
		    </div>
		    <div class="loginBoxWrap">
			    <div class="loginBox">
				    <div class="boxTop">
					    <h3 id = "lblForgotPassword">Forgot Password or Account Locked</h3>
					    <div id = "divLogin" style ="display:block">
					        <p runat="server" id = "lblEnterForm">Please enter your username and submit your request by clicking on the <b>SUBMIT</b> button once only.</p>
					        <table id="tblContent" border ="0" style="width:100%" runat="server">
                                <tbody>
                                    <tr>
                                        <td colspan="2" align="center"></td>
                                    </tr>
                                    
                                    <tr>
                                        <td style="width:40%">
                                            <asp:Label runat ="server" id="lblUserName" Text = "Username" />
                                        </td>
                                        <td style="width:60%">
                                            <wucTextBox:TextBox ID="txtUserID" runat="server" maxLength="25" TabIndex="1" width= "75%"/>
                                        </td>
					                </tr>
                                    <tr style ="Height:30px" >
                                        <td style="text-align:right" >&nbsp;</td>
                                        <td style="text-align:left" >&nbsp;</td>
                                    </tr>
					                <tr>
					                    <td colspan="2" align="center">
                                            <asp:UpdatePanel runat = "server" ID = "updatePanel2" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Button id = "btnForgotPassword" onclick="btnForgotPassword_OnClick"  Text = "SUBMIT" runat = "server" TabIndex="2"/>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnForgotPassword" EventName = "Click"/>
                                                </Triggers>
                                            </asp:UpdatePanel>
					                    </td>
					                </tr>
                                    <tr style ="Height:30px" >
                                        <td style="text-align:right" >&nbsp;</td>
                                        <td style="text-align:left" >&nbsp;</td>
                                    </tr>
					                <tr>
                                        <td  colspan="2" style="Height:30; text-align : center; color:Red" class = "ErrorMessage">
                                            <div id = "divErrorMessage" style ="display:none;" runat = "server"><span><p id = "pErrorMessage"></p></span></div>
                                        </td>
                                    </tr>
					            </tbody>
					        </table>
					    </div>
					    <div class="boxBtm"></div>
					</div>
                </div>
            </div>
	    </div>
    </form>
</body>
</html>