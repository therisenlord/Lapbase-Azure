<%@ page language="C#" autoeventwireup="true" inherits="Forms_ImportExportData_ImportFile, Lapbase.deploy" enableEventValidation="false" viewStateEncryptionMode="Always" %>
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
<body runat = "server" id= "bodyImportFile">

    <wucMenu:Menu runat = "server" ID = "mainMenu"/>
    <div class="tabMenus">
	    <wucAppSchemaFunc:AppSchemaFunc runat = "server" ID = "AppSchemaFuncMenu" currentItem = "ImportFile" />
	</div>

    <form runat = "server" id = "frmImportFile">
        <asp:ScriptManager ID="_ScriptManager" runat="server" AsyncPostBackTimeout = "60000" />
	    <div class="contentArea">
		    <div class="greyContentWrap">
			    <div class="importExport">
			        <table width="100%">
			            <tr>			            
                            <td style="width:1%" rowspan = "10"/>
                            <td style="width:25%">
                                <asp:label runat = "server" ID = "lblDocumentFile" Text = "Path of Lapdata Folder to be imported: " />
                            </td>
                            <td style="width:74%" align="left">
                                <asp:TextBox Width="300" runat="server" ID= "txtFolder" /> &nbsp; <a runat="server" onserverclick="CheckVisitRecord">calculate number of visit file</a>
                            </td>
			            </tr>
			            <tr>
			                <td>
                                <input type="button" id="btnImport" value="Import Baseline File" runat="server" onserverclick="ExportFromSqlServerToMDB"/>
                            </td>
                            <td align="right">
                                <asp:TextBox runat="server" Width="60" ID="txtStartRecord"/> &nbsp; - &nbsp; <asp:TextBox runat="server" Width="60" ID="txtEndRecord"/> (<asp:Label runat="server" id="lblNumofRec" text="0"/>) Records
                                &nbsp; &nbsp; &nbsp; &nbsp; <input type="button" id="btnImportVisit" value="Import Visit Video File" runat="server" onserverclick="ImportVisitDocument"/>
                            </td>
			            </tr>
			            <tr>			            
                            <td>
                                <input type="button" id="btnImportImageVisit" value="Import Visit Image File" runat="server" onserverclick="ImportVisitImageDocument"/>
                            </td>
			            </tr>
			            <tr><td>&nbsp;</td></tr>
			            <tr>
			                <td valign="top" colspan="2"><div runat="server" id="txtErrNotes" style="color:red"/></td>
			            </tr>
			            <tr><td>&nbsp;</td></tr>
			            <tr><td>
			                <asp:RadioButton id="radPhoto" Text="Photos" Checked="False" GroupName="doctype" runat="server"/>
			                <asp:RadioButton id="radDoc" Text="Documents" Checked="False" GroupName="doctype" runat="server"/>
			                <asp:RadioButton id="radVideo" Text="Videos" Checked="True" GroupName="doctype" runat="server"/>
                            <input type="button" runat="server" value="Delete Imported File" id="btnDeleteFile" onserverclick="DeleteDocument"/>
			                </td></tr>
			            <tr>
			                <td valign="top">Notes:</td>
			            </tr>
			            <tr>
			                <td colspan="2"><textarea runat="server" id="txtNotes" cols="113" rows="20"/></td>
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
