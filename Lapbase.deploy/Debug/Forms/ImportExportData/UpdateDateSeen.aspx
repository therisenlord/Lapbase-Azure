<%@ page language="C#" autoeventwireup="true" inherits="Forms_ImportExportData_UpdateDateSeen, Lapbase.deploy" enableEventValidation="false" viewStateEncryptionMode="Always" %>
<%@ Register TagPrefix = "wucMenu" TagName = "Menu" Src = "~/UserControl/MenuWUCtrl.ascx" %>
<%@ Register TagPrefix = "wucTextArea" TagName = "TextArea" Src = "~/UserControl/TextAreaWUCtrl.ascx" %>
<%@ Register TagPrefix = "wucAppSchemaFunc" TagName = "AppSchemaFunc" Src = "~/UserControl/AppSchemaFuncWUCtrl.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>LapBase - A Data Manager for Bariatric Surgery</title>
	<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <link href='<%$ AppSettings:CssUrl%>' rel="stylesheet" type="text/css" />
    <script src="../../Scripts/Global.js" type="text/javascript" language="javascript"></script>
    
</head>
<body runat = "server">

    <wucMenu:Menu runat = "server" ID = "Menu1"/>
    <div class="tabMenus">
	    <wucAppSchemaFunc:AppSchemaFunc runat = "server" ID = "AppSchemaFuncMenu" currentItem = "UpdateDateSeen" />
	</div>
	
    <form runat = "server" id = "frmUpdateDate">
        <asp:ScriptManager ID="_ScriptManager" runat="server" AsyncPostBackTimeout = "60000" />
	    <div class="contentArea">
		    <div class="greyContentWrap">
			    <div>
			        <table width="100%">
			            <tr>
			                <td>
                                <input type="button" id="btnCheckFirstVisitDate" value="Check First Visit Date" runat="server" onserverclick="checkFirstVisitDate"/>	                
			                </td>
			                <td>
                                <input type="button" id="btnUpdateFirstVisitDate" value="Update First Visit Date" runat="server" onserverclick="updateFirstVisitDate"/>		
                            </td>
                        </tr>
			            <tr>
			                <td>
                                <input type="button" id="btnCheck" value="Check Last Visit Date" runat="server" onserverclick="checkDate"/>	                
			                </td>
			                <td>
                                <input type="button" id="btnUpdate" value="Update Last Visit Date" runat="server" onserverclick="updateDate"/>		
                            </td>
                        </tr>
			            <tr>
			                <td valign="top">Notes:</td>
			            </tr>
			            <tr>
			                <td colspan="2"><textarea runat="server" id="txtNotes" cols="113" rows="50"/></td>
			            </tr>
			            <tr>
			                <td colspan="2" align="right">
			                    <input type="button" value="Clear Result Notes" onclick="this.form.elements['txtNotes'].value=''"/>
			                </td>
			            </tr>
			        </table>
			    </div>
			</div>
        </div>
	</form>
</body>
</html>
