<%@ page language="C#" autoeventwireup="true" inherits="_Default, Lapbase.deploy" enableviewstate="true" enableEventValidation="false" viewStateEncryptionMode="Always" %>
<%@ Register TagPrefix = "wucTextBox" TagName = "TextBox" Src = "~/UserControl/TextBoxWUCtrl.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1" />
    <%--<link href="http://lapbase.com/app_css/v2/admin_common.css" rel="stylesheet" type="text/css" />--%>
    <link href='<%$ AppSettings:CssUrl%>' rel="stylesheet" type="text/css" />
    <script language="javascript" src = "Scripts/Global.js" type="text/javascript" ></script>
    <script language="javascript" src = "Login/Login.js" type="text/javascript" ></script>
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
					    <h3 id = "lblLogin">LapBase Login</h3>
					    <div id = "divLogin" style ="display:block">
					        <p id = "lblForm">Please enter your username and password to login</p>

				            <table border ="0" style="width:100%">
                            <tbody>
                                <tr>
                                    <td colspan="2" align="center"></td>
                                </tr>
                                
                                <tr>
                                    <td style="width:40%">
                                        <asp:Label runat ="server" id="lblUserName" Text = "Username" />
                                    </td>
                                    <td style="width:60%">
                                        <asp:UpdatePanel runat = "server" ID = "updatePanel1" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <wucTextBox:TextBox ID="txtUserID" runat="server" maxLength="25" TabIndex="1" width= "75%"/>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnLogonServer" EventName = "Click"/>
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
					            </tr>
    					        
					            <tr>
                                    <td >
                                        <asp:Label runat = "server" id = "lblUserPW" Text = "Password" />
                                    </td>
                                    <td >
                                        <asp:UpdatePanel runat = "server" ID = "updatePanel2" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <wucTextBox:TextBox id ="txtUserPW" Runat="server" Direction = "LTR" MaxLength="8" TextMode="Password" tabindex="2"  Width= "75%"/>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnLogonServer" EventName = "Click"/>
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                
                                <tr style ="Height:30px" >
                                    <td>
                                        <asp:Label runat = "server" ID = "lblLanguage" Text = "Select language" />
                                    </td>
                                    <td >
                                        <asp:DropDownList runat = "server" ID= "cmbLanguage" Width = "95%" AutoPostBack = "false" onchange="javascript:cmbLanguage_onchange();" TabIndex="3"/>
                                        <asp:Label runat = "server" ID = "lblLangauge" />
                                        
                                    </td>
					            </tr>
    					    </tbody>
    					    </table>
    					</div>
    					
    					<div id = "divChangePassword" style ="display:none">
                            <p id = "P3">You are required to change your password at first login...</p>
                            <table border ="0" style="width:100%">
                            <tbody>
                                <tr>
                                    <td colspan="2" align="center"></td>
                                </tr>
					            <tr>
                                    <td style="width:40%" >
                                        <asp:Label runat = "server" id = "lblUserNewPW" Text = "New password" />
                                    </td>
                                    <td style="width:60%" >
                                        <asp:UpdatePanel runat = "server" ID = "updatePanel4" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <wucTextBox:TextBox id ="txtNewUserPW" Runat="server" Direction = "LTR" MaxLength="8" TextMode="Password" Width= "75%" TabIndex="4"/>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnLogonServer" EventName = "Click"/>
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                            
                                <tr >
                                    <td >
                                        <asp:Label runat = "server" id = "lblUserNewPW_Confirm" Text = "Confirm new password" />
                                    </td>
                                    <td >
                                        <asp:UpdatePanel runat = "server" ID = "updatePanel5" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <wucTextBox:TextBox id ="txtNewUserPW_Confirm" Runat="server" Direction = "LTR" MaxLength="8" TextMode="Password" Width= "75%" TabIndex="5"/>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnLogonServer" EventName = "Click"/>
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </tbody>
                            </table>
                        </div>      
                        <div>
                            <table>
                            <tbody>
					            <tr>
                                    <td style="Height:30; text-align : center; color:Red" class = "ErrorMessage">
                                        <div id = "divErrorMessage" style ="display:none;" runat = "server"><span><p id = "pErrorMessage"></p></span></div>
                                    </td>
                                </tr>
                                <tr style ="Height:10px" >
                                    <td style="text-align:right" >&nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:UpdatePanel runat = "server" ID = "updatePanel3" UpdateMode="always">
                                            <ContentTemplate>
                                                <asp:TextBox ID = "lblCultureInfo" runat = "server" Text = ""/>
                                                <textarea runat = "server" id = "txtTest" rows = "10" style="width:100%;display:none" cols = "80"></textarea>
                                                <asp:Button id = "btnLogonServer" onclick="btnLogonServer_OnClick"  Text = "LOGIN" runat = "server" TabIndex="6"/>
                                                <asp:LinkButton ID = "btnLinkLogin" runat = "server" OnClick= "btnLogonServer_OnClick" Text = "Login Link" style="display:none"/>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnLogonServer" EventName = "Click"/>
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr id="divForgotPassword" runat="server">
                                    <td align="center"><br />
                                        <a href="#" onclick="window.open('ForgotPassword.aspx', '_self'); return false;"><label id="lblForgotPassword" runat="server"/></a>
                                    </td>
                                </tr>
                                <tr>
                                    <td><br /><br />
                                        <asp:Label id="lblChangeLog" runat="server"/>
                                    </td>
                                </tr>
                                <tr>
                                <td>
                                <br /><br /><br />
                                    <asp:Label id="lblUserAgreement" runat="server" />
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
        
        <asp:HiddenField runat="server" ID="txtHApplicationURL" />
        <asp:HiddenField runat = "Server" ID = "TitleLoaded" value = "0"/>
    </form>
</body>
</html>