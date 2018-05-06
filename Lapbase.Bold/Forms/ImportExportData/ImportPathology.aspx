<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImportPathology.aspx.cs" Inherits="Forms_ImportExportData_ImportPathology" Debug="true" %>
<%@ Register TagPrefix = "wucMenu" TagName = "Menu" Src = "~/UserControl/MenuWUCtrl.ascx" %>
<%@ Register TagPrefix = "wucTextArea" TagName = "TextArea" Src = "~/UserControl/TextAreaWUCtrl.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>LapBase - A Data Manager for Bariatric Surgery</title>
	<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <link href='<%$ AppSettings:CssUrl%>' rel="stylesheet" type="text/css" />
    <script src="../../Scripts/Global.js" type="text/javascript" language="javascript"></script>
    <script language="javascript" type="text/javascript" src="Includes/ImportExportData.js"></script>
    
</head>
<body runat = "server" id= "bodyImportFile">

    <wucMenu:Menu runat = "server" ID = "mainMenu"/>
    <div class="tabMenus">
        <div class="manilaTabMenu">
            <ul>
                <li id="li_Div1" class="current"><a href="#this" onclick="javascript:controlBar_Buttons_OnClick(1);">
                    <img id="Img1" src="~/img/tab_progress_notes.gif" runat="server" alt="" height="16" width="16" /><span
                        id="lblImport">Import</span></a></li>
                <li id="li_Div2"><a href="#this" onclick="javascript:controlBar_Buttons_OnClick(2);">
                    <img id="Img2" src="~/img/tab_reports.gif" runat="server" alt="" height="16" width="16" /><span
                        id="lblReport">Report</span></a></li>
            </ul>
        </div>
    </div>
    <form runat = "server" id = "frmPathologyFile">
        <asp:ScriptManager ID="_ScriptManager" runat="server" AsyncPostBackTimeout = "60000" />
	    <div class="contentArea">
            <div class="greyContentWrap">
                <div class="visits">
			        <div class="importExport" id="divImportPathology">
		                <table width="100%">
		                    <tr>			            
                                <td style="width:1%" rowspan = "10"/>
                                <td style="width:25%">
                                    <asp:label runat = "server" ID = "lblDocumentFile" Text = "Path of Pathology File to be imported: " />
                                </td>                            
		                    </tr>
		                    <tr>
		                        <td>
		                            <input type="file" ID="textFile1" runat="server" size="80" />
		                            <input type="file" ID="textFile2" runat="server" size="80" />
		                            <input type="file" ID="textFile3" runat="server" size="80" />
		                            <input type="file" ID="textFile4" runat="server" size="80" />
		                            <input type="file" ID="textFile5" runat="server" size="80" />
		                            <input type="file" ID="textFile6" runat="server" size="80" />
		                            <input type="file" ID="textFile7" runat="server" size="80" />
		                            <input type="file" ID="textFile8" runat="server" size="80" />
		                            <input type="file" ID="textFile9" runat="server" size="80" />
		                            <input type="file" ID="textFile10" runat="server" size="80" />
		                            <!--<asp:TextBox Width="300" runat="server" ID= "txtFolder" />-->
                                    <input type="button" id="btnImport" value="Import Pathology File" runat="server" onserverclick="ImportPathology"/>
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

                    <div id="divPathologyList" style="display:none">
                        <div class="expandListManila">
                            <div class="boxTop">
                                <div class="expandListManilaTitle">
                                    <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                        <tbody>
                                            <tr style="vertical-align: middle;">
                                                <td style="width: 50%; text-align: left">
                                                    <label id="lblComplicationDataForm" runat="Server" style="font-weight: bold">
                                                        Lab Detail</label>
                                                </td>
                                                <td style="text-align: right" runat="server" id="btnColl">
                                                <div style="float: right">
                                                    <input id="btnCancelLab" value="Cancel" onclick="javascript:btnCancelLab_onclick(this);" 
                                                        type="button" style="display: none; width: 150px" />
                                                </div>
                                                    <div style="float: right" runat="server">
                                                        <input type="button" value="Mark as Reviewed" runat="server" id="btnDeleteLab" style="width: 150px;display: none;" onserverclick="btnReviewLab_onserverclick" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                                <div class="addLabDetails" id="div_LabData" style="display: none">
                                    <table width="90%" border="0" cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <div id="divErrorMessage" style="display: none;" runat="server">
                                                        <span>
                                                            <p id="pErrorMessage" runat="server">
                                                            </p>
                                                        </span>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;<asp:Label visible = "false" runat="server" ID="Label1" Text="Path of Pathology File to be imported: " />&nbsp;
                                                    <input visible = "false" type="file" id="txtFile" runat="server" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                                
                                <div class="addLabDetails" id="div_LabList" style="display: none">
                                    <table width="90%" border="0" cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr>
                                                <td>
                                                <div id="pathologyResultTable" runat="server">
                                                </div>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    
                                </div>
                                <div class="boxBtm">
                                </div>
                            </div>
                        </div>
                    
                        <div class = "expandList">
                            <div class="boxTop">
                                <div class="expandListTitle">
                                    <table width="100%" border="0" cellpadding="4" cellspacing="4">
                                        <tbody>
                                            <tr style="vertical-align: middle">
                                                <th width="15%">
                                                    <asp:Label Text="Surname" runat="server"/>
                                                </th>
                                                <th width="15%">
                                                    <asp:Label Text="First Name" runat="server"/>
                                                </th>
                                                <th width="10%">
                                                    <asp:Label Text="Birth date" runat="server"/>
                                                </th>
                                                <th width="19%">
                                                    <asp:Label Text="Referring Doctor" runat="server"/>
                                                </th>
                                                <th width="41%">
                                                    <asp:Label Text="Test Date" runat="server"/>
                                                </th>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                                <div class="expandListScroll" id="div_LabsList" runat="server">
                                </div>
                                <div class="boxBtm">
                                </div>
                            </div>
                        </div>
                    </div>
    	        </div>
			</div>
        </div>
        <asp:HiddenField runat="server" ID="txtHPatientID" Value="0" />
        <asp:HiddenField runat="server" ID="txtHLabID" Value="0" />
        <asp:HiddenField runat="server" ID="txtHIsBaseline" Value="0" />
        <asp:HiddenField runat="server" ID="txtHDeleted" Value="0" />
        <asp:UpdatePanel runat="server" ID="up_HiddenFields">
            <ContentTemplate>
                <asp:LinkButton runat="server" ID="linkBtnLoad" OnClick="linkBtnLoad_OnClick" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="LinkBtnLoad" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
	</form>
</body>
</html>
