<%@ page language="C#" autoeventwireup="true" inherits="_InactivePractice, Lapbase.deploy" enableviewstate="true" enableEventValidation="false" viewStateEncryptionMode="Always" %>
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
					    <h3 id = "lblLogin">Inactive Practice</h3>
					    <div id = "divLogin" style ="display:block">
					        <p id = "lblForm">Your practice has been marked inactive. <br />Please contact help@lapbase.com for further information.</p>
					    </div>
					    <div class="boxBtm"></div>
					</div>
                </div>
            </div>
	    </div>
    </form>
</body>
</html>