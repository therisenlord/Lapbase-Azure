﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UpdateCurrentWeight.aspx.cs" Inherits="Forms_ImportExportData_UpdateCurrentWeight" %>
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
	    <wucAppSchemaFunc:AppSchemaFunc runat = "server" ID = "AppSchemaFuncMenu" currentItem = "UpdateCurrentWeight" />
	</div>
	
    <form runat = "server" id = "frmUpdateCurrentWeight">
        <asp:ScriptManager ID="_ScriptManager" runat="server" AsyncPostBackTimeout = "60000" />
	    <div class="contentArea">
		    <div class="greyContentWrap">
			    <div>
			        <table width="100%">
			            <tr>
			                <td>
                                <input type="button" id="btnCheck" value="Check" runat="server" onserverclick="checkCurrentWeight"/>			                
			                </td>
			                <td>
                                <input type="button" id="btnUpdate" value="Update" runat="server" onserverclick="updateCurrentWeight"/>
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
