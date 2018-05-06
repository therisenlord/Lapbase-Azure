<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImportCSV.aspx.cs" Inherits="Forms_ImportExportData_ImportCSV" Debug="true" %>
<%@ Register TagPrefix = "wucMenu" TagName = "Menu" Src = "~/UserControl/MenuWUCtrl.ascx" %>
<%@ Register TagPrefix = "wucTextArea" TagName = "TextArea" Src = "~/UserControl/TextAreaWUCtrl.ascx" %>

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
    <form runat = "server" id = "frmCSVFile">
        <asp:ScriptManager ID="_ScriptManager" runat="server" AsyncPostBackTimeout = "60000" />
	    <div class="contentArea">
            <div class="greyContentWrap">
                <div class="visits">
			        <div class="importExport" id="divImportCSV">
		                <table width="100%">
		                    <tr>			            
                                <td style="width:1%" rowspan = "10"/>
                                <td style="width:25%">
                                    <asp:label runat = "server" ID = "lblDocumentFile" Text = "Path of CSV File to be imported: " />
                                </td>                            
		                    </tr>
		                    <tr>
		                        <td>
		                            <input type="file" ID="textFile" runat="server" size="80" />
                                    <input type="button" id="btnImport" value="Import CSV File" runat="server" onserverclick="ImportCSV"/>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
		                    <tr>
		                        <td valign="top" colspan="2"><div runat="server" id="txtErrNotes" style="color:red"/></td>
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
        </div>
	</form>
</body>
</html>
