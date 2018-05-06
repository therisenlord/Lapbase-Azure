<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImportData.aspx.cs" Inherits="Forms_ImportExportData_ImportData" Debug="true" %>
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
    <div class="tabMenus">
        <div class="greyTabMenu">
            <ul>
                <li id="li_Div1" class="current"><a id="a_ImportFile" href="#">
                    Import</a></li>
            </ul>
        </div>
    </div>
    
    <form runat = "server" id = "frmCSVFile">
        <asp:ScriptManager ID="_ScriptManager" runat="server" AsyncPostBackTimeout = "60000" />
	    <div class="contentArea">
            <div class="greyContentWrap">
                <div class="visits">
			        <div class="importExport" id="divImportCSV">
		                <table width="100%">
		                    <tr>
		                        <td>
		                            <asp:label runat = "server" ID = "lblSelectImport" Text = "Import Type: " />
                                    <asp:DropDownList ID="selectType" runat="server">
                                        <asp:ListItem Text="Patient Demographic" Value="patient" />
                                        <asp:ListItem Text="Patient Visit" Value="visit" />
                                        <asp:ListItem Text="Operation Duration" Value="operationduration" />
                                        <asp:ListItem Text="Operation Type" Value="operationtype" />
                                    </asp:DropDownList>
                                    <a href="#" id="aTemplate" runat="server" onserverclick="DownloadFile">Download Template</a>
                                </td>
                            </tr>
		                    <tr>			            
                                <td>
                                    <asp:label runat = "server" ID = "lblDocumentFile" Text = "Path of CSV File to be imported: " /> <input type="file" ID="textFile" runat="server" size="80" />
                                </td>                            
		                    </tr>
                            <tr>
                                <td>
                                    <input type="button" id="btnImportFile" value="Import" runat="server" onserverclick="ImportFile"/>   
                                </td>
                            </tr>
                                    <%--<input type="button" id="btnImport" value="Import Patient File" runat="server" onserverclick="ImportCSV"/>
                                    <input type="button" id="btnImportOperationDuration" value="Import Operation Duration File" runat="server" onserverclick="ImportOperationDurationCSV"/>
                                    <input type="button" id="btnImportOperationType" value="Import Operation Type File" runat="server" onserverclick="ImportOperationTypeCSV"/>
                                    <input type="button" id="btnImportVisit" value="Import Visit" runat="server" onserverclick="ImportVisitCSV"/>
                                    --%>
		                    <tr>
		                        <td valign="top" colspan="2"><div runat="server" id="txtErrNotes" style="color:red"/></td>
		                    </tr>			            
		                    <tr>
		                        <td colspan="2"><textarea runat="server" id="txtNotes" cols="113" rows="15"/></td>
		                    </tr>
		                    <tr>
		                        <td colspan="2" align="right">
		                            <input type="button" value="Download Result Notes" onserverclick="DownloadResult" runat="server"/>
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
