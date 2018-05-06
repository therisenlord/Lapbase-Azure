<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImportExportDatabase.aspx.cs" Inherits="Forms_ImportExportData_ImportExportDatabase" %>
<%@ Register TagPrefix = "wucMenu" TagName = "Menu" Src = "~/UserControl/MenuWUCtrl.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>LapBase - A Data Manager for Bariatric Surgery</title>
	<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <link href='<%$ AppSettings:CssUrl%>' rel="stylesheet" type="text/css" />
    <script src="Includes/ImpExpPopup.js" type="text/javascript" language="javascript"></script>
    <script type = "text/javascript" src = "../../Scripts/Global.js"></script>
</head>
<body runat = "server" id= "bodyImportExportData">
    <form runat = "server" id = "frmImportExportData">
    <wucMenu:Menu runat = "server" ID = "mainMenu"/>
    <asp:ScriptManager ID="_ScriptManager" runat="server" AsyncPostBackTimeout = "60000" />
    <div class="tabMenus">
	    <div class="greyTabMenu">
			<ul>
			    <%--<li id = "li_Home" ><a id="a_Home" href="~/Forms/PatientsVisits/PatientsVisitsForm.aspx" runat = "server" >Patient List</a></li>--%>
				<li class="current"><a href="#">Export to MDB</a></li>
			</ul>
		</div>
	</div>
	<div class="contentArea">
		  <div class="greyContentWrap">
			<div class="importExport">
				<div class="import" id="importMDB" runat="server" visible="false">
                	<h3>Import</h3>
                    <h2>Import data from your Microsoft Access Version <br/>to LapBase Web Edition</h2>
                   	<img src="~/img/import_img.gif" style="width:396px;height:140px" runat = "server" alt="Import"/>
                    <p>Click below to Import data from your exsiting Microsoft Access Version (LAPDATA.MDB)</p>
                    
                    <div class="importBtn">
                        <input id = "btnImportData" name="btnImportData" value="Import Data" type="button" onclick = "javascript:document.location.assign('ImportExportForm.aspx');" />
                    </div>
                </div>
				
				<div class="export">
                	<h3>Export</h3>
                    <h2>Export data from LapBase Web Edition <br/>to your Microsoft Access Version</h2>
                    <img src="~/img/export_img.gif" style="width:396px;height:140px" runat = "server" alt="Export"/>
                    <asp:Button Id="btnCheckFile" Text="Click to check if file ready" runat = "server" OnClick="btnCheckExportData_OnClick" style="display: none"/>
                    
                    <p><asp:Label runat="server" ID="lblProgress" /></p>
                    <div class="exportBtn">
                        <asp:UpdatePanel runat = "Server" id = "updatePanel1">
                            <ContentTemplate>
                                <asp:Button Id="btnExportData" Text="Export" runat = "server" OnClick="btnExportData_OnClick" OnClientClick="javascript:btnExportDataJS_onclick();"/>
                                <br /><asp:HyperLink runat = "server" id = "linkLocalMDB" NavigateUrl="#" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID = "btnExportData" EventName = "Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>      
                </div>
				
				<div class="clr"></div>
			</div>
		</div>
	</div>
	</form>
</body>
</html>