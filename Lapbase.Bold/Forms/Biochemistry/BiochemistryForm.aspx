<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BiochemistryForm.aspx.cs" Inherits="Forms_Biochemistry_BiochemistryForm" %>
<%@ Register TagPrefix = "wucPatient" TagName = "PatientTitle" Src = "~/UserControl/PatientTitleDataWUCtrl.ascx" %>
<%@ Register TagPrefix = "wucTextBox" TagName = "TextBox" Src = "~/UserControl/TextBoxWUCtrl.ascx" %>
<%@ Register TagPrefix = "wucMenu" TagName = "Menu" Src = "~/UserControl/MenuWUCtrl.ascx" %>
<%@ Register TagPrefix = "wucAppSchema" TagName = "AppSchema" Src = "~/UserControl/AppSchemaWUCtrl.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>LapBase - A Data Manager for Bariatric Surgery</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <link href='<%$ AppSettings:CssUrl%>' rel="stylesheet" type="text/css" />
    <!-- JavaScript -->
    <script type = "text/javascript" src = "../../Scripts/Global.js"></script>
    <script type = "text/javascript" src = "Includes/Biochemistry.js"></script>
    
    <!-- Calendar -->
    <link rel="stylesheet" href="../../css/Calendar/calendar.css" media="screen"/>
	<script type="text/javascript" src="../../Scripts/Calendar/calendar.js"></script>
</head>
<body runat = "server" id = "bodyBiochemistry" >
    <wucMenu:Menu runat = "server" ID = "mainMenu" />
    <wucAppSchema:AppSchema runat = "server" ID = "AppSchemaMenu" currentItem = "Biochemistry" />

    <form id="frmComplication" runat="server">
    <asp:ScriptManager ID="_ScriptManager" runat="server" ></asp:ScriptManager>
    <div class="contentArea">
        <wucPatient:PatientTitle runat = "server" ID = "tblPatientTitle" />	
            <div class="greyContentWrap">
            <div id = "div_vBiochemistry" class = "visits" runat="server">
                <div class = "expandListManila">
                    <div class = "boxTop">
                        <div class = "expandListManilaTitle" >
                            <table border="0" cellpadding="3" cellspacing="0" width="100%">
                            <tbody>
							    <tr style="vertical-align: middle;">
							        <td style ="width:50%;text-align:left">
                                        <label id = "lblBiochemistryDataForm" runat = "Server" style="font-weight:bold">Biochemistry Detail</label>
                                    </td>
                                    <td style="text-align:right" runat="server" id="btnColl">
                                        <div style="float:right">
									        <input id="btnCancelBiochemistry" value="Add new Biochemistry" onclick="javascript:btnCancelBiochemistry_onclick(this);" type="button" style="display:block; width:150px" />
									    </div>									    
                                        <div style="float:right" runat="server" id="divBtnDelete">
									        <input id="btnDeleteBiochemistry" value = "Delete" onclick = "javascript:btnDelete_onclick();" type="button" style="width:150px;display:none;" />
									    </div>
									    <div style="float:right">
									        <input id="btnSaveBiochemistry" value="Save" onclick="javascript:btnSaveBiochemistry_onclick();" type="button" style="width:150px;display:none;"/>
									    </div>
                                    </td>
							    </tr>
							</tbody>
							</table>
                        </div>
                        <div class = "addVisitDetails" id = "div_BiochemistryData" style ="display:none">
                            <table width="90%" border="0" cellpadding = "0" cellspacing = "0">
                            <tbody>
                                <tr>
                                    <td colspan="2" style="vertical-align:middle">
                                        <div id = "divErrorMessage" style ="display:none;" runat = "server"><span><p id = "pErrorMessage" runat  = "server"></p></span></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="Width:120px"><asp:Label runat = "server" Text = "Date" ID = "lblDate_Com" /></td>
                                </tr>
                                <tr>                                
                                    <td>
                                        <wucTextBox:TextBox runat = "server" ID = "txtDate_com" Width = "80px" MaxLength = "10" />&nbsp;
                                        <asp:Label runat = "Server" ID= "lblDateFormat" Text = "" style="display:none"/>
                                        <a href="#this" type="button" id = "aCalendar" onclick="javascript:aCalendar_onclick(this);">[...]</a>&nbsp;
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <br /><br />     
                                        <b>Biochemistry List</b>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div id="tblBiochemistryList" runat="server"></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">                                    
                                        <div id="tblBiochemistryChoiceList" runat="server"></div>
                                    </td>
                                </tr>
                            </tbody>
                            </table>
                        </div>
                        <div class = "boxBtm"></div>
                    </div>
                </div>
                <div class = "expandList">
                    <div class = "boxTop">
                        <div class = "expandListTitle">
                            <table width = "100%" border = "0" cellpadding = "4" cellspacing = "4" >
                            <tbody>
                                <tr style="vertical-align:middle" >
                                    <th style =" width:100%">
                                        <asp:Label Text = "Date" runat = "server" ID = "lblDate_Complications" />
                                    </th>
                                </tr>
                            </tbody>
                            </table>
                        </div>
                        
                        <div class = "expandListScroll" id = "div_PatientBiochemistryList" runat="server">
                            <table runat = "server" id = "tblPatientBiochemistry" width = "100%" cellpadding = "0" cellspacing = "0" border="0" ></table>
                        </div>
                    
                        <div class = "boxBtm"></div>
                        <asp:HiddenField ID = "txtHPatientBiochemistryID" runat = "server" Value = "0" />
                    </div>
                </div>
            </div>
            <div class="clr"></div>
        </div>
    </div>
    <asp:HiddenField runat = "server" ID = "txtHPatientID" Value = "0" />
    <asp:HiddenField runat = "server" ID = "txtHPatientBiochemistryValue" Value = ""/>
    <asp:HiddenField runat = "server" ID = "txtHDateCreated" />
    <asp:HiddenField runat = "server" ID = "txtHPermissionLevel" />
    <asp:HiddenField runat = "server" ID = "txtHDataClamp" />
    <asp:HiddenField runat = "server" ID = "txtHApplicationURL"  />
    <asp:HiddenField runat = "server" ID = "txtHCurrentDate" Value = "" />
    <asp:HiddenField runat = "Server" ID = "txtHCulture" />
    <asp:HiddenField runat = "Server" ID = "TitleLoaded" Value = "0" />
    <asp:UpdatePanel runat = "server" ID = "up_HiddenFields">
        <ContentTemplate>
            <asp:LinkButton runat = "server" ID = "linkBtnLoad" OnClick = "linkBtnLoadBiochemistry_OnClick" />            
            <asp:LinkButton runat = "server" ID = "linkBtnSave" OnClick = "linkBtnSave_OnClick" />          
            <asp:LinkButton runat = "server" ID = "linkBtnDelete" OnClick = "linkBtnDelete_OnClick" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID = "LinkBtnLoad" EventName = "Click" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
