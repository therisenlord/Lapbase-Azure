<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImportExportReport.aspx.cs" Inherits="Forms_ImportExportData_ImportExportReport" %>
<%@ Register TagPrefix = "wucMenu" TagName = "Menu" Src = "~/UserControl/MenuWUCtrl.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>LapBase - A Data Manager for Bariatric Surgery</title>
	<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <link href='<%$ AppSettings:CssUrl%>' rel="stylesheet" type="text/css" />
    <script src="Includes/ImpExpPopup.js" type="text/javascript" language="javascript"></script>
    <script src="Includes/ImportExportReport.js" type="text/javascript" language="javascript"></script>
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
				    <li class="current" id="li_Div1"><a href="#this" onclick="javascript:controlBar_Buttons_OnClick(1);">Export</a></li>
				    <li class="" style="display:none" id="li_Div2"><a href="#this" onclick="javascript:controlBar_Buttons_OnClick(2);">Import / Export MDB</a></li>
				    
			    </ul>
		    </div>
	    </div>
	    <div class="contentArea">
		      <div class="greyContentWrap">
    			
    			
			    <div class="groupReportsSection" id="div_vExport">
			        <ul>
			            <li>
                            <input type="radio" name="rdGroupReports" onclick="javascript:CheckGroupReportRadioBtn(0);" />&nbsp;
                            <label id="Label5" onmouseover="javascript:this.style.cursor='pointer';" onmouseout="javascript:this.style.cursor='';"
                                onclick="javascript:CheckGroupReportRadioBtn(0);">
                                Patient List</label>
                        </li>
                        <li>
                            <input type="radio" name="rdGroupReports" onclick="javascript:CheckGroupReportRadioBtn(1);" />&nbsp;
                            <label id="Label6" onmouseover="javascript:this.style.cursor='pointer';" onmouseout="javascript:this.style.cursor='';"
                                onclick="javascript:CheckGroupReportRadioBtn(1);">
                                Visit List</label>
                        </li>
                        <li>
                            <input type="radio" name="rdGroupReports" onclick="javascript:CheckGroupReportRadioBtn(2);" />&nbsp;
                            <label id="Label1" onmouseover="javascript:this.style.cursor='pointer';" onmouseout="javascript:this.style.cursor='';"
                                onclick="javascript:CheckGroupReportRadioBtn(2);">
                                Operation List</label>
                        </li>
                    </ul>
                    <iframe id="frameReport" style="width: 0; height: 0; visibility: hidden"></iframe>
                    <input type="button" id="btnDownload" value="Download" style="width: 80px" onclick="javascript:BuildReport(1);"/>
			    </div>
			    
			    
			    <div class="importExport" id="div_vImportExportMDB" style="display:none">
				    <div class="import">
                	    <h3>Import</h3>
                        <h2>Import data from your Microsoft Access Version <br/>to LapBase Web Edition</h2>
                   	    <img id="Img1" src="~/img/import_img.gif" style="width:396px;height:140px" runat = "server" alt="Import"/>
                        <p>Click below to Import data from your exsiting Microsoft Access Version (LAPDATA.MDB)</p>
                        
                        <div class="importBtn">
                            <input id = "btnImportData" name="btnImportData" value="Import Data" type="button" onclick = "javascript:document.location.assign('ImportExportForm.aspx');" />
                        </div>
                    </div>
    				
				    <div class="export">
                	    <h3>Export</h3>
                        <h2>Export data from LapBase Web Edition <br/>to your Microsoft Access Version</h2>
                        <img id="Img2" src="~/img/export_img.gif" style="width:396px;height:140px" runat = "server" alt="Export"/>
                        <p>Click below to Export data from LapBase Web Edition&nbsp;</p>
                        <asp:Button Id="btnCheckFile" Text="Click to check if file ready" runat = "server" OnClick="btnCheckExportData_OnClick"/>
                        <p><asp:Label runat="server" ID="lblProgress" /></p>
                        <div class="exportBtn">
                            <asp:UpdatePanel runat = "Server" id = "updatePanel1">
                                <ContentTemplate>
                                    <asp:Button Id="btnExportData" Text="Export Data" runat = "server" OnClick="btnExportData_OnClick" OnClientClick="javascript:btnExportData_onclick();"/>
                                    <asp:HyperLink runat = "server" id = "linkLocalMDB" NavigateUrl="#" />
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
        <asp:UpdatePanel runat="Server" ID="up_HiddenFields" EnableViewState="false">
            <ContentTemplate>
                <asp:HiddenField runat="server" ID="txtHPageNo" Value="1" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <input type="text" id="ReportCode" value="" style="display: none" runat="server" />
        <!--RepPatientList
        RepVisitList
        RepOperationList -->
	</form>
</body>
</html>