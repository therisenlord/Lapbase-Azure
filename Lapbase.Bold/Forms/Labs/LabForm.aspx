<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LabForm.aspx.cs" Inherits="Forms_Labs_LabForm"%>

<%@ Register TagPrefix="wucPatient" TagName="PatientTitle" Src="~/UserControl/PatientTitleDataWUCtrl.ascx" %>
<%@ Register TagPrefix="wucTextBox" TagName="TextBox" Src="~/UserControl/TextBoxWUCtrl.ascx" %>
<%@ Register TagPrefix="wucMenu" TagName="Menu" Src="~/UserControl/MenuWUCtrl.ascx" %>
<%@ Register TagPrefix="wucSystemCode" TagName="SystemCodeList" Src="~/UserControl/SystemCodeWUCtrl.ascx" %>
<%@ Register TagPrefix="wucAppSchema" TagName="AppSchema" Src="~/UserControl/AppSchemaWUCtrl.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>LapBase - A Data Manager for Bariatric Surgery</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <link href='<%$ AppSettings:CssUrl%>' rel="stylesheet" type="text/css" />
    <!-- JavaScript -->

    <script type="text/javascript" src="../../Scripts/Global.js"></script>

    <script type="text/javascript" src="Includes/Lab.js"></script>

    <!-- Calendar -->
    <link rel="stylesheet" href="../../css/Calendar/calendar.css" media="screen" />

    <script type="text/javascript" src="../../Scripts/Calendar/calendar.js"></script>

</head>
<body runat="server" id="bodyLab">
    <wucMenu:Menu runat="server" ID="mainMenu" />
    <wucAppSchema:AppSchema runat="server" ID="AppSchemaMenu" currentItem="Labs" />
    <form id="frmLab" runat="server">
        <asp:ScriptManager ID="_ScriptManager" runat="server">
        </asp:ScriptManager>
        <div class="contentArea">
            <wucPatient:PatientTitle runat="server" ID="tblPatientTitle" />
            <div class="greyContentWrap">
                <div class="visits">
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
                                                <div style="float: right">
                                                    <input id="btnSaveLab" value="Save" runat="server" onserverclick="linkBtnSave_OnClick"
                                                        type="button" style="width: 150px; display: none;" />
                                                </div>
                                                <div id="divBtnDelete" style="float: right" runat="server">
                                                    <input type="button" value="Delete" runat="server" id="btnDeleteLab" style="width: 150px;
                                                        display: none" onclick="javascript:btnDelete_onclick();" onserverclick="btnDeleteLab_onserverclick" />
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
                                                &nbsp;<asp:Label visible = "false" runat="server" ID="lblDocumentFile" Text="Path of Pathology File to be imported: " />&nbsp;
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
                    <div class="expandList">
                        <div class="boxTop">
                            <div class="expandListTitle">
                                <table width="100%" border="0" cellpadding="4" cellspacing="4">
                                    <tbody>
                                        <tr style="vertical-align: middle">
                                            <th>
                                                <asp:Label Text="Date" runat="server" ID="lblDate_Labs" />
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
                <div class="clr">
                </div>
            </div>
        </div>
        <asp:HiddenField runat="server" ID="txtHPatientID" Value="0" />
        <asp:HiddenField runat="server" ID="IsLabViewed" Value="0" />
        <asp:HiddenField runat="server" ID="txtHLabData" Value="" />
        <asp:HiddenField runat="server" ID="txtHLabID" Value="0" />
        <asp:HiddenField runat="server" ID="txtHDateCreated" />
        <asp:HiddenField runat="server" ID="txtHPermissionLevel" />
        <asp:HiddenField runat="server" ID="txtHDataClamp" />
        <asp:HiddenField runat="server" ID="txtHApplicationURL" />
        <asp:HiddenField runat="server" ID="txtHCurrentDate" Value="" />
        <asp:HiddenField runat="Server" ID="txtHCulture" />
        <asp:HiddenField runat="Server" ID="TitleLoaded" Value="0" />
        <asp:HiddenField runat="Server" ID="txtHDelete" Value="0" />
        <asp:HiddenField runat="server" ID="txtHIsBaseline" Value="0" />
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