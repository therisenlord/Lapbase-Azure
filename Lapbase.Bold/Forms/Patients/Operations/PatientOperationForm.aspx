<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PatientOperationForm.aspx.cs"
    Inherits="Forms_Patients_Operations_PatientOperationForm" ValidateRequest="false" EnableViewState="true"%>

<%@ Register TagPrefix="wucPatient" TagName="PatientTitle" Src="~/UserControl/PatientTitleDataWUCtrl.ascx" %>
<%@ Register TagPrefix="wucDoctor" TagName="DoctorList" Src="~/UserControl/DoctorsListWUCtrl.ascx" %>
<%@ Register TagPrefix="wucSystemCode" TagName="SystemCodeList" Src="~/UserControl/SystemCodeWUCtrl.ascx" %>
<%@ Register TagPrefix="wucCode" TagName="CodeList" Src="~/UserControl/CodeWUCtrl.ascx" %>
<%@ Register TagPrefix="wucHospital" TagName="HospitalList" Src="~/UserControl/HospitalListWUCtrl.ascx" %>
<%@ Register TagPrefix="wucTextBox" TagName="TextBox" Src="~/UserControl/TextBoxWUCtrl.ascx" %>
<%@ Register TagPrefix="wucMenu" TagName="Menu" Src="~/UserControl/MenuWUCtrl.ascx" %>
<%@ Register TagPrefix="wucAppSchema" TagName="AppSchema" Src="~/UserControl/AppSchemaWUCtrl.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>LapBase - A Data Manager for Bariatric Surgery</title>
    <link href='<%$ AppSettings:CssUrl%>' rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript" src="../../../Scripts/Global.js"></script>

    <script language="javascript" type="text/javascript" src="Includes/PatientOperationForm.js"></script>

    <!-- Calendar -->
    <link rel="stylesheet" href="../../../css/Calendar/calendar.css" media="screen" />

    <script type="text/javascript" src="../../../Scripts/Calendar/calendar.js"></script>

</head>
<body runat="server" id="bodyPatientOperation">
    <wucMenu:Menu runat="server" ID="mainMenu" />
    <div class="tabMenus">
        <wucAppSchema:AppSchema runat="server" ID="AppSchemaMenu" currentItem="Operations" />
        <div class="manilaTabMenu">
            <ul>
                <li id="li_Div4" class="current"><a href="#this" onclick="javascript:controlBar_Buttons_OnClick(4);">
                    <img src="~/img/tab_progress_notes.gif" runat="server" alt="" height="16" width="16" /><span
                        id="lblOperationList">Operation List</span></a></li>
                <li id="li_Div1"><a href="#this" onclick="javascript:ClearFields();controlBar_Buttons_OnClick(1);">
                    <img src="~/img/tab_complications.gif" runat="server" alt="" height="16" width="16" /><span
                        id="lblOperationDetails">Operation Details</span></a></li>
                <li id="li_Div2" style="display: none"><a href="#this" onclick="javascript:controlBar_Buttons_OnClick(2);">
                    <img src="~/img/tab_reports.gif" runat="server" alt="" height="16" width="16" /><span
                        id="lblOtherDetails">Other Details</span></a></li>
                <li id="li_Div3" style="display: none"><a href="#this" onclick="javascript:controlBar_Buttons_OnClick(3);">
                    <img src="~/img/tab_reports.gif" runat="server" alt="" height="16" width="16" /><span
                        id="lblComplicationsEventsOperationNotes">Complications / Events Operation Notes</span></a></li>
            </ul>
        </div>
    </div>
    <form id="frmPatientOperation" runat="server">
        <asp:ScriptManager ID="_ScriptManager" runat="server">
        </asp:ScriptManager>
        <div class="contentArea">
            <div id="divErrorMessage" style="display: none;">
                <span>
                    <p id="pErrorMessage">
                    </p>
                </span>
            </div>
            <wucPatient:PatientTitle runat="server" ID="tblPatientTitle" />
            <div class="greyContentWrap">
                <div class="operationsList" id="div_vOperationsList" style="display: block">
                    <div class="btnWrap">
                        <input type="button" id="btnAddOperation" runat="server" value="Add new operation"
                            style="width: 150px" onclick="javascript:ClearFields();controlBar_Buttons_OnClick(1);" />
                    </div>
                    <div class="expandList">
                        <div class="boxTop">
                            <div class="expandListTitle">
                                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                    <tbody>
                                        <tr>
                                            <td style="width: 80px">
                                                <asp:Label ID="lblOperationDate_TC" runat="server" Text="Operation Date" />
                                            </td>
                                            <td style="width: 160px">
                                                <asp:Label ID="lblOperation_TC" runat="server" Text="Operation" />
                                            </td>
                                            <td style="width: 150px">
                                                <asp:Label ID="lblApproach_TC" runat="server" Text="Approach" />
                                            </td>
                                            <td style="width: 180px">
                                                <asp:Label ID="lblSurgeon_TC" runat="server" Text="Surgeon" />
                                            </td>
                                            <td style="width: 180px">
                                                <asp:Label ID="lblHospital_TC" runat="server" Text="Hospital" />
                                            </td>
                                            <td style="width: 130px">
                                                <asp:Label ID="lblWeight_TC" runat="Server" Text="Weight" />
                                            </td>
                                            <!-- <td style="width:130px">
                                        <asp:Label id = "lblLosPostOps_TC" runat = "Server" Text = "Los Post Ops (Days)" />
                                    </td> -->
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <asp:UpdatePanel runat="server" ID="upOperationList" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="expandListScroll" style="height: 200px" id="div_OperationList" runat="server" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="linkbtnLoadOperationsList" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                            <div class="boxBtm" runat="server" />
                        </div>
                    </div>
                </div>
                <asp:UpdatePanel runat="server" ID="up_OperationDetails" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="operationsDetails" id="div_vDetail" style="display: none" runat="server">
                            <div class="btnWrap">
                                <table width="100%" class="full">
                                    <tr>
                                        <td align="left">
                                            <input type="button" runat="server" id="btnDelete" value="Delete" style="width: 100px"
                                                onclick="javascript:btnDelete_onclick();" /></td>
                                        <td align="right">
                                            <input type="button" runat="server" id="btnPrintOreg1" value="Operation Reg 1" style="width: 150px;display:none"
                                                onclick="javascript:BuildReport(1);" /></td>
                                        <td align="right">
                                            <input type="button" runat="server" id="btnPrintOreg2" value="Operation Reg 2" style="width: 150px;display:none"
                                                onclick="javascript:BuildReport(2);" /></td>
                                        <td align="right">
                                                 <asp:Label id="lblAnnualOreg" Text="Which Annual? 1 or 2 or .." style="display:none" runat="server"/></td>
                                        <td align="right">
                                            <input type="text" runat="server" id="txtAnnualOreg" value="1" style="width: 20px;display:none" /></td>
                                        <td align="right">
                                            <input type="button" runat="server" id="btnSave" value="Save" style="width: 150px"
                                                onclick="javascript:btnSave_onclick();" />
                                            <asp:Button runat="server" ID="btnCancel" Text="Cancel" Width="150px" OnClick="linkBtnLoadOperation_OnClick" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="leftColumn">
                                <div class="operationNotes">
                                    <div class="boxTop">
                                        <h3 id="h_General Details">
                                            General Details</h3>
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tbody>
                                                <tr>
                                                    <td style="width: 15%;">
                                                        <asp:Label ID="lblOperationDate" runat="Server" Text="Operation Date" /></td>
                                                    <td style="width: 38%;">
                                                        <wucTextBox:TextBox ID="txtOperationDate" runat="server" maxLength="10" width="25%" />
                                                        &nbsp;
                                                        <asp:Label runat="Server" ID="lblDateFormat" Text="" Style="display: none" />
                                                        <a href="#this" id="aCalendar" onclick="javascript:aCalendar_onclick(this,'txtOperationDate');">
                                                        [...]</label>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblWeight" runat="Server" Text="Weight" />
                                                    </td>
                                                    <td>
                                                        <wucTextBox:TextBox ID="txtWeight" runat="server" maxLength="12" width="15%" />
                                                        &nbsp;
                                                        <asp:CheckBox runat="server" ID="chkUpdateWeight" />
                                                        <asp:Label runat="server" ID="lblUpdateWeight" Text="Update Baseline Data?" Font-Size="7pt" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblSurgeon" runat="Server" Text="Surgeon" />
                                                    </td>
                                                    <td>
                                                        <wucDoctor:DoctorList runat="server" ID="cmbSurgeon" Width="75" IsSurgeon="True" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblHospital" runat="Server" Text="Facility" />
                                                    </td>
                                                    <td>
                                                        <wucHospital:HospitalList runat="Server" ID="cmbHospital" Width="75" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblLosPostOp" runat="Server" Text="LOS Post Op." />
                                                    </td>
                                                    <td>
                                                        <wucTextBox:TextBox runat="server" ID="txtLosPostOp" width="40px" maxLength="5" />
                                                        &nbsp;&nbsp;
                                                        <label>
                                                            hours</label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblAdmissionDate" runat="Server" Text="Date of Admission" />
                                                    </td>
                                                    <td>
                                                        <wucTextBox:TextBox ID="txtAdmissionDate" runat="server" maxLength="10" width="25%" />
                                                        &nbsp;
                                                        <asp:Label runat="Server" ID="lblAdmissionDateFormat" Text="" Style="display: none" />
                                                        <a href="#this" id="aAdmissionCalendar" onclick="javascript:aCalendar_onclick(this,'txtAdmissionDate');">
                                                        [...]</label>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblAnesthesiaDuration" runat="Server" Text="Duration of Anesthesia" />
                                                    </td>
                                                    <td>
                                                        <wucTextBox:TextBox runat="server" ID="txtAnesthesiaDuration" width="40px" maxLength="5" />
                                                        &nbsp;&nbsp;<label>minutes</label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblBloodTransfusion" runat="Server" Text="Blood Transfusion" />
                                                    </td>
                                                    <td>
                                                        <wucTextBox:TextBox runat="server" ID="txtBloodTransfusion" width="40px" maxLength="5" />
                                                    </td>
                                                </tr>
                                                <tr id="intraopTranfusionACS" runat="server">
                                                    <td>
                                                        <asp:Label ID="lblIntraopTransfusion" runat="Server" Text="# Intraoperative Transfusion" />
                                                    </td>
                                                    <td>
                                                        <wucTextBox:TextBox runat="server" ID="txtIntraopTranfusion" width="40px" maxLength="5" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblAsaClassification" runat="Server" Text="ASA Classification" />
                                                    </td>
                                                    <td>
                                                        <wucSystemCode:SystemCodeList runat="Server" ID="cmbAsaClassification" CriteriaString="ASA"
                                                            BoldData="ASA" Width="75" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblDVTProphylaxis" runat="Server" Text="DVT Prophylaxis" />
                                                    </td>
                                                    <td>
                                                        <input type="checkbox" runat="server" id="chkAntigloculan" />
                                                        Anticoagulation &nbsp; &nbsp;
                                                        <input type="checkbox" runat="server" id="chkTED" />
                                                        TED Stocking &nbsp; &nbsp;
                                                        <input type="checkbox" runat="server" id="chkFootPump" />
                                                        Foot Pump &nbsp; &nbsp;
                                                        <input type="checkbox" runat="server" id="chkCompress" />
                                                        Compress Device
                                                    </td>
                                                </tr>
                                                <tr id="assistantACS" runat="server">
                                                    <td valign="top">
                                                        <asp:Label ID="lblFirstAssistant" runat="Server" Text="First Assistant - level of training" />
                                                    </td>
                                                    <td>
                                                        <input type="checkbox" runat="server" id="chkAssistantNone" />
                                                        None &nbsp; &nbsp;
                                                        <input type="checkbox" runat="server" id="chkAssistantPA" />
                                                        PA/ NP/ RNFA &nbsp; &nbsp;
                                                        <input type="checkbox" runat="server" id="chkAssistantJunior" />
                                                        Junior Resident (PGY 1-3)<br />
                                                        <input type="checkbox" runat="server" id="chkAssistantSenior" />
                                                        Senior Resident (PGY 4-5)
                                                        <input type="checkbox" runat="server" id="chkAssistantMIS" />
                                                        MIS Fellow &nbsp; &nbsp;
                                                        <input type="checkbox" runat="server" id="chkAssistantAttendingSurgeon" />
                                                        Attending - Weight Loss Surgeon<br />
                                                        <input type="checkbox" runat="server" id="chkAssistantAttendingOther" />
                                                        Attending - Other
                                                    </td>
                                                </tr>
                                                <tr id="assistantDefault" runat="server">
                                                    <td>
                                                        <asp:Label ID="lblSurgicalAssistance" runat="Server" Text="Surgical Assistance" />
                                                    </td>
                                                    <td>
                                                        <input type="checkbox" runat="server" id="chkResident" />
                                                        Surgical Resident Participated &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                                        <input type="checkbox" runat="server" id="chkFellow" />
                                                        Surgical Fellow Participated
                                                    </td>
                                                </tr>
                                                <tr id="operativeTimeACS" runat="server">
                                                    <td colspan="2">
                                                        <table>
                                                            <tr>
                                                                <td style="width: 15%">
                                                                    Patient In Room Time
                                                                </td>
                                                                <td style="width: 38%">
                                                                    <input type="text" size="2" maxlength="2" id="txtInRoomTimeH" runat="server" />&nbsp;
                                                                    :&nbsp;
                                                                    <input type="text" size="2" maxlength="2" id="txtInRoomTimeM" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Surgery Start Time
                                                                </td>
                                                                <td>
                                                                    <input type="text" size="2" maxlength="2" id="txtSurgeryStartH" runat="server" />&nbsp;
                                                                    :&nbsp;
                                                                    <input type="text" size="2" maxlength="2" id="txtSurgeryStartM" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Surgery End Time
                                                                </td>
                                                                <td>
                                                                    <input type="text" size="2" maxlength="2" id="txtSurgeryEndH" runat="server" />&nbsp;
                                                                    :&nbsp;
                                                                    <input type="text" size="2" maxlength="2" id="txtSurgeryEndM" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Patient Out Room Time
                                                                </td>
                                                                <td>
                                                                    <input type="text" size="2" maxlength="2" id="txtOutRoomTimeH" runat="server" />&nbsp;
                                                                    :&nbsp;
                                                                    <input type="text" size="2" maxlength="2" id="txtOutRoomTimeM" runat="server" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr id="preopHematocritACS" runat="server">
                                                    <td valign="top">
                                                        <asp:Label ID="lblPreopHematocrit" runat="Server" Text="Preop Hematocrit" />
                                                    </td>
                                                    <td>
                                                        <input type="text" runat="server" id="txtPreopHematocrit" size="40"/>
                                                        <wucTextBox:TextBox ID="txtPreopHematocritDate" runat="server" maxLength="10" width="25%" />
                                                        &nbsp; <a href="#this" id="aPreopHematocritDate" onclick="javascript:aCalendar_onclick(this,'txtPreopHematocritDate');">
                                                            [...]</a>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr id="preopAlbuminACS" runat="server">
                                                    <td valign="top">
                                                        <asp:Label ID="lblPreopAlbumin" runat="Server" Text="Preop Albumin" />
                                                    </td>
                                                    <td>
                                                        <input type="text" runat="server" id="txtPreopAlbumin" size="40"/>
                                                        <wucTextBox:TextBox ID="txtPreopAlbuminDate" runat="server" maxLength="10" width="25%" />
                                                        &nbsp; <a href="#this" id="aPreopAlbuminDate" onclick="javascript:aCalendar_onclick(this,'txtPreopAlbuminDate');">
                                                            [...]</a>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblConcurrentProcedure" runat="Server" Text="Concurrent Procedure" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <table>
                                                            <tr>
                                                                <td style="width: 47%">
                                                                    <select id="listConcurrent" multiple="true" size="10" runat="server" style="width: 280px"
                                                                        ondblclick="javascript:BoldLists_dblclick('listConcurrent', 'listConcurrent_Selected', 'Add');" />
                                                                    <wucSystemCode:SystemCodeList runat="server" ID="cmbConcurrent" CriteriaString="CONCURRENT"
                                                                        BoldData="CONCURRENT" Display="false" />
                                                                </td>
                                                                <td style="text-align: center; vertical-align: middle">
                                                                    <a href="#this" onclick="javascript:BoldLButtonLinks_click('listConcurrent', 'listConcurrent_Selected', 'Add', false);">
                                                                        ></a><br />
                                                                    <a href="#this" onclick="javascript:BoldLButtonLinks_click('listConcurrent', 'listConcurrent_Selected', 'Add', true);">
                                                                        >></a><br />
                                                                    <a href="#this" onclick="javascript:BoldLButtonLinks_click('listConcurrent_Selected', 'listConcurrent', 'Remove', false);">
                                                                        <</a><br />
                                                                    <a href="#this" onclick="javascript:BoldLButtonLinks_click('listConcurrent_Selected', 'listConcurrent', 'Remove', true);">
                                                                        <<</a><br />
                                                                </td>
                                                                <td style="width: 47%">
                                                                    <select id="listConcurrent_Selected" multiple="true" size="10" runat="server" style="width: 280px"
                                                                        ondblclick="javascript:BoldLists_dblclick('listConcurrent_Selected', 'listConcurrent', 'Remove');" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>                                                
                                                <tr id="otherProcedureACS" runat="server">
                                                    <td valign="top" colspan="2">
                                                    <input type="hidden" id="totalOtherProcedures" runat="server" value="0" /><input type="hidden"
                                                        id="txtDetailOtherProcedure" runat="server" value="" />
                                                        <br /><asp:Label ID="lblOtherProcedureACS" runat="Server" Text="Other Procedure" /> <input type="button" onclick="addOtherProcedure();" value="+" style="width: 20px; height: 20px" /><br />                                                      
                                                        <div id="otherProcedureDiv" runat="server">
                                                        </div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <div class="boxBtm">
                                        </div>
                                    </div>
                                </div>
                                <div class="operationNotes">
                                    <div class="boxTop">
                                        <h3>
                                            <label runat="server" id="lblIntraOperative">
                                                HOSPITAL STAY Intra-operative Adverse Events</label></h3>
                                        <div id="divIntraOperative" style="display: block">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tbody>
                                                    <tr>
                                                        <td colspan="3">
                                                            <label id="lblIntraOperativeSearch">
                                                                Search by
                                                            </label>
                                                            <wucTextBox:TextBox runat="server" ID="txtIntraOperativeSearch" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 47%">
                                                            <select id="listIntraOperativeEvents" multiple="true" size="10" runat="server" style="width: 280px"
                                                                ondblclick="javascript:BoldLists_dblclick('listIntraOperativeEvents', 'listIntraOperativeEvents_Selected', 'Add');" />
                                                            <wucSystemCode:SystemCodeList runat="server" ID="cmbIntraOperativeEvents" CriteriaString="ADEV"
                                                                BoldData="ADEV" Display="false" />
                                                        </td>
                                                        <td style="text-align: center; vertical-align: middle">
                                                            <a href="#this" onclick="javascript:BoldLButtonLinks_click('listIntraOperativeEvents', 'listIntraOperativeEvents_Selected', 'Add', false);">
                                                                ></a><br />
                                                            <a href="#this" onclick="javascript:BoldLButtonLinks_click('listIntraOperativeEvents', 'listIntraOperativeEvents_Selected', 'Add', true);">
                                                                >></a><br />
                                                            <a href="#this" onclick="javascript:BoldLButtonLinks_click('listIntraOperativeEvents_Selected', 'listIntraOperativeEvents', 'Remove', false);">
                                                                <</a><br />
                                                            <a href="#this" onclick="javascript:BoldLButtonLinks_click('listIntraOperativeEvents_Selected', 'listIntraOperativeEvents', 'Remove', true);">
                                                                <<</a><br />
                                                        </td>
                                                        <td style="width: 47%">
                                                            <select id="listIntraOperativeEvents_Selected" multiple="true" size="10" runat="server"
                                                                style="width: 280px" ondblclick="javascript:BoldLists_dblclick('listIntraOperativeEvents_Selected', 'listIntraOperativeEvents', 'Remove');" />
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                        <div class="boxBtm">
                                        </div>
                                    </div>
                                </div>
                                <div class="operationNotes">
                                    <div class="boxTop">
                                        <h3>
                                            <label runat="server" id="lblPreDischarge">
                                                HOSPITAL STAY Pre-discharge Adverse Events</label></h3>
                                        <div id="div1" style="display: block">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tbody>
                                                    <tr>
                                                        <td colspan="3">
                                                            <label id="Label8">
                                                                Search by
                                                            </label>
                                                            <wucTextBox:TextBox runat="server" ID="txtPreDischargeSearch" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 47%">
                                                            <select id="listPreDischargeEvents" multiple="true" size="10" runat="server" style="width: 280px"
                                                                ondblclick="javascript:BoldLists_dblclick('listPreDischargeEvents', 'listPreDischargeEvents_Selected', 'Add');" />
                                                            <wucSystemCode:SystemCodeList runat="server" ID="cmbPreDischargeEvents" CriteriaString="ADEV"
                                                                BoldData="ADEV" Display="false" />
                                                        </td>
                                                        <td style="text-align: center; vertical-align: middle">
                                                            <a href="#this" onclick="javascript:BoldLButtonLinks_click('listPreDischargeEvents', 'listPreDischargeEvents_Selected', 'Add', false);">
                                                                ></a><br />
                                                            <a href="#this" onclick="javascript:BoldLButtonLinks_click('listPreDischargeEvents', 'listPreDischargeEvents_Selected', 'Add', true);">
                                                                >></a><br />
                                                            <a href="#this" onclick="javascript:BoldLButtonLinks_click('listPreDischargeEvents_Selected', 'listPreDischargeEvents', 'Remove', false);">
                                                                <</a><br />
                                                            <a href="#this" onclick="javascript:BoldLButtonLinks_click('listPreDischargeEvents_Selected', 'listPreDischargeEvents', 'Remove', true);">
                                                                <<</a><br />
                                                        </td>
                                                        <td style="width: 47%">
                                                            <select id="listPreDischargeEvents_Selected" multiple="true" size="10" runat="server"
                                                                style="width: 280px" ondblclick="javascript:BoldLists_dblclick('listPreDischargeEvents_Selected', 'listPreDischargeEvents', 'Remove');" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <br />
                                                            <table border="0" cellpadding="-1" cellspacing="-1">
                                                                <tr>
                                                                    <td style="width: 240px">
                                                                        <asp:Label runat="server" ID="lblTimeOccurredAfterSurgery" Text="Time occurred after surgery" />
                                                                    </td>
                                                                    <td>
                                                                        <wucTextBox:TextBox runat="server" ID="txtAfterSurgery" maxLength="2" width="20px" />
                                                                        &nbsp; <wucSystemCode:SystemCodeList runat="server" ID="cmbAfterSurgeryMeasurement" FirstRow="false" CriteriaString="ADEVOC" BoldData="ADEVOC" Width="20"/>&nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label runat="server" ID="lblAdverseSurgeon" Text="Re-operation Surgeon" />
                                                                    </td>
                                                                    <td>
                                                                        <wucDoctor:DoctorList runat="server" ID="cmbAdverseSurgeon" Width="75" IsSurgeon="True" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label runat="server" ID="lblAdverseSurgery" Text="Surgical Intervention" />
                                                                    </td>
                                                                    <td>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbAdverseSurgery" CriteriaString="ADEVST" BoldData="ADEVST"/>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                    <br /><br />
                                                                    </td>
                                                                </tr>
                                                                <tr id="bloodTranfusionACS" runat="server">
                                                                    <td>
                                                                        <asp:Label ID="lblBloodTranfusionACS" runat="Server" Text="# Blood tranfusions within first 72 hours" />
                                                                    </td>
                                                                    <td>
                                                                        <wucTextBox:TextBox runat="server" ID="txtNumberBloodTranfusion" width="40px" maxLength="5" />
                                                                    </td>
                                                                </tr>                                                                
                                                                <tr id="unplannedAdmissionACS" runat="server">
                                                                    <td>
                                                                        <asp:Label ID="lblUnplannedAdmissionACS" runat="Server" Text="Unplanned Admission to ICU within 30 days" />
                                                                    </td>
                                                                    <td>
                                                                        <input type="radio" name="rbUnplannedAdmission" id="rbUnplannedAdmissionY" value="Y" runat="server" />
                                                                            &nbsp;Yes &nbsp; &nbsp;
                                                                        <input type="radio" name="rbUnplannedAdmission" id="rbUnplannedAdmissionN" value="N" runat="server" />
                                                                            &nbsp;No &nbsp; &nbsp;
                                                                    </td>
                                                                </tr>                                                                
                                                                <tr id="transferACS" runat="server">
                                                                    <td>
                                                                        <asp:Label ID="lblTransferACS" runat="Server" Text="Transfer to Acute Care Hospital within 30 days" />
                                                                    </td>
                                                                    <td>
                                                                        <input type="radio" name="rbTransfer" id="rbTransferY" value="Y" runat="server" />
                                                                            &nbsp;Yes &nbsp; &nbsp;
                                                                        <input type="radio" name="rbTransfer" id="rbTransferN" value="N" runat="server" />
                                                                            &nbsp;No &nbsp; &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label runat="server" ID="lblDischargeDate" Text="Discharge Date" />
                                                                    </td>
                                                                    <td>
                                                                        <wucTextBox:TextBox ID="txtDischargeDate" runat="server" maxLength="10" width="25%" />
                                                                        &nbsp;
                                                                        <asp:Label runat="Server" ID="lblDischargeDateFormat" Text="" Style="display: none" />
                                                                        <a href="#this" id="aDischargeCalendar" onclick="javascript:aCalendar_onclick(this,'txtDischargeDate');">
                                                                            [...]</a>&nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label runat="server" ID="lblDischargeTo" Text="Discharge To" />
                                                                    </td>
                                                                    <td>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbDischargeTo" CriteriaString="PREDIS"
                                                                            BoldData="PRED" Width="50" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                        <div class="boxBtm">
                                        </div>
                                    </div>
                                </div>
                                <%--<div class="operationNotes">
                                <div class = "boxTop">
                                    <h3><label runat = "server" id = "lblOperationNotes" >Notes</label></h3>
                                    <div id="divOperationNotes" style ="display:block">
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <wucTextBox:TextBox runat="server" ID="txtGeneralNotes" textMode="MultiLine" rows = "5" width ="100%" />
                                                </td>
                                            </tr>
                                        </tbody>
                                        </table>
                                    </div>
                                    <div class="boxBtm"></div>
                                </div>
                            </div>--%>
                            </div>
                            <div class="rightColumn">
                                <div class="operationDetailsPanel">
                                    <div class="boxTop">
                                        <h3 id="hOperationDetails">
                                            Operation Details</h3>
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <tbody>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblOperation" Text="Operation" runat="server" /></td>
                                                    <td>
                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbSurgeryType" Width="85" CriteriaString="BST"
                                                            BoldData="BST" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblApproach" Text="Approach" runat="server" /></td>
                                                    <td>
                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbApproach" Width="85" CriteriaString="Approach"
                                                            BoldData="Approach" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblCategory" Text="Category" runat="server" />
                                                    </td>
                                                    <td>
                                                        <wucCode:CodeList runat="server" ID="cmbCategory" Width="85" CriteriaString="PC" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblGroup" Text="Group" runat="server" />
                                                    </td>
                                                    <td>
                                                        <wucCode:CodeList runat="server" ID="cmbGroup" Width="85" CriteriaString="GRO" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblDuration" Text="Duration" />
                                                    </td>
                                                    <td>
                                                        <wucTextBox:TextBox runat="server" ID="txtDuration" maxLength="4" width="30%" />
                                                        &nbsp;
                                                        <asp:Label runat="Server" ID="lblMinute" Text="min" />
                                                        &nbsp;
                                                        <asp:Label Text="Blood Loss" runat="Server" ID="lblBloodLoss" Visible="false" />
                                                        &nbsp;
                                                        <wucTextBox:TextBox runat="server" maxLength="3" ID="txtBloodLoss" width="10%" Visible="false" />
                                                        &nbsp;
                                                        <asp:Label runat="server" ID="lblMLS" Text="mls" Visible="false" />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <div id="div_SerialNo">
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblSerialNo" Text="Serial No." />
                                                    </td>
                                                    <td>
                                                        <wucTextBox:TextBox runat="server" ID="txtSerialNo" maxLength="50" width="85%" />
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="div_BandType">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 32%">
                                                        <asp:Label runat="server" ID="lblBandType" Text="Band Type" /></td>
                                                    <td>
                                                        <wucSystemCode:SystemCodeList runat="server" ID="ddlBandType" Width="85" CriteriaString="BandType"
                                                            BoldData="BandType" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="div_lblBanded">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 32%">
                                                        <asp:Label runat="server" ID="lblBanded" Text="Banded" /></td>
                                                    <td>
                                                        <asp:CheckBox runat="server" ID="chkBanded" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="div_lblTubeSize">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 32%">
                                                        <asp:Label runat="server" ID="lblTubeSize" Text="Tube Size" /></td>
                                                    <td>
                                                        <wucTextBox:TextBox runat="server" ID="txtTubeSize" maxLength="10" width="25%" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="div_lblLGBilealSegment">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 32%">
                                                        <asp:Label runat="server" ID="lblLGBilealSegment" Text="Ileal Segment (cms)" />
                                                    </td>
                                                    <td id="div_rblLGBilealSegment">
                                                        <asp:DropDownList runat="server" ID="cmbLGBilealSegment" Width="50%">
                                                            <asp:ListItem Value="110" Text="110" />
                                                            <asp:ListItem Value="120" Text="120" />
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="div_lblBPDIlealLength">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 32%">
                                                        <asp:Label runat="server" ID="lblBPDIlealLength" Text="Ileal Segment (cms)" />
                                                    </td>
                                                    <td id="div_txtBPDIlealLength">
                                                        <wucTextBox:TextBox runat="server" ID="txtBPDIlealLength" width="25%" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="div_lblVBGStomaWrap">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 32%">
                                                        <asp:Label runat="server" ID="lblVBGStomaWrap" Text="Stomal Wrap" />
                                                    </td>
                                                    <td id="div_cmbVBGStomaWrap">
                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbVBGStomaWrap" Width="50" CriteriaString="VPGW" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="div_lblRouxLimbLength">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 32%">
                                                        <asp:Label runat="server" ID="lblRouxLimbLength" Text="Limb length (cms)" />
                                                    </td>
                                                    <td id="div_txtRouxLimbLength">
                                                        <wucTextBox:TextBox runat="server" ID="txtRouxLimbLength" width="25%" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="div_lblBPDChannelLength">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 32%">
                                                        <asp:Label runat="server" ID="lblBPDChannelLength" Text="Channel length (cms)" />
                                                    </td>
                                                    <td id="div_txtBPDChannelLength">
                                                        <wucTextBox:TextBox runat="server" ID="txtBPDChannelLength" maxLength="3" width="25%" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="div_lblLGBomentalpatch">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 32%">
                                                        <asp:Label runat="server" ID="lblOmental_Patch" Text="Omental Patch" />
                                                    </td>
                                                    <td id="div_chkLGBomentalpatch">
                                                        <asp:CheckBox runat="server" ID="chkLGBomentalpatch" Checked="false" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="div_lblVBGStomaSize">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 32%">
                                                        <asp:Label runat="server" ID="lblVBGStomaSize" Text="Stomal Size (mm)" />
                                                    </td>
                                                    <td id="div_txtVBGStomaSize">
                                                        <wucTextBox:TextBox runat="server" ID="txtVBGStomaSize" width="25%" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="div_lblRouxEnterostomy">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 32%">
                                                        <asp:Label runat="server" ID="lblRouxEnterostomy" Text="Gastroenterostomy" />
                                                    </td>
                                                    <td id="div_cmbRouxEnterostomy">
                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbRouxEnterostomy" Width="50" CriteriaString="RYGE" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="div_lblReservoirSite">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 32%">
                                                        <asp:Label runat="server" ID="lblReservoirSite" Text="Reservoir" />
                                                    </td>
                                                    <td id="div_cmbReservoirSite">
                                                        <asp:DropDownList runat="server" ID="cmbReservoirSite" Width="50%">
                                                            <asp:ListItem Text="Loin" Value="L" />
                                                            <asp:ListItem Text="Anterior to Rectus Sheath" Value="A" Selected="True" />
                                                            <asp:ListItem Text="Within Rectus Sheath" Value="P" />
                                                            <asp:ListItem Text="Sternum (lower)" Value="S" />
                                                            <asp:ListItem Text="Stapled" Value="ST" />
                                                            <asp:ListItem Text="Suture" Value="SU" />
                                                            <asp:ListItem Text="Other" Value="O" />
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="div_lblRouxColic">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 32%">
                                                        <asp:Label runat="server" ID="lblRouxColic" Text="Retrocolic / Antecolic" />
                                                    </td>
                                                    <td id="div_cmbRouxColic">
                                                        <asp:DropDownList runat="server" ID="cmbRouxColic" Width="50%">
                                                            <asp:ListItem Text="Retrocolic" Value="R" />
                                                            <asp:ListItem Text="Antecolic" Value="A" />
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="div_lblBPDDuodenalSwitch">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 32%">
                                                        <asp:Label runat="server" ID="lblBPDDuodenalSwitch" Text="Duodenal Switch" />
                                                    </td>
                                                    <td id="div_chkBPDDuodenalSwitch">
                                                        <asp:CheckBox runat="server" ID="chkBPDDuodenalSwitch" Checked="false" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="div_lblBalloonVolume">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 32%">
                                                        <asp:Label runat="server" ID="lblBalloonVolume" Text="Band Fill Volume" />
                                                    </td>
                                                    <td id="div_txtBalloonVolume">
                                                        <wucTextBox:TextBox runat="server" ID="txtBalloonVolume" maxLength="5" width="25%" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="div_lblLGBreinforcedsutures">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 32%">
                                                        <asp:Label runat="server" ID="lblReinforced_sutures" Text="Reinforced sutures" />
                                                    </td>
                                                    <td id="div_chkLGBreinforcedsutures">
                                                        <asp:CheckBox runat="server" ID="chkLGBreinforcedsutures" Checked="false" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="div_lblBPDStomachSize">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 32%">
                                                        <asp:Label runat="server" ID="lblBPDStomachSize" Text="Stomach size" />
                                                    </td>
                                                    <td id="div_txtBPDStomachSize">
                                                        <wucTextBox:TextBox runat="server" ID="txtBPDStomachSize" width="25%" maxLength="5" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="div_lblRouxGastric">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 32%">
                                                        <asp:Label runat="server" ID="lblRouxGastric" Text="RouxGastric" />
                                                    </td>
                                                    <td id="div_rblRouxGastric">
                                                        <asp:DropDownList runat="server" ID="cmbRouxGastric" Width="50%">
                                                            <asp:ListItem Text="Retrogastric" Value="R" />
                                                            <asp:ListItem Text="Antegastric" Value="A" />
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="div_lblLapBandSerialNumber" style="display: none;">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 32%">
                                                        <asp:Label runat="server" ID="lblLapBandSerialNumber" Text="Product Code" />
                                                    </td>
                                                    <td id="div_rblLapBandSerialNumber">
                                                        <asp:DropDownList runat="server" ID="cmbLapBandSerialNumber" Width="50%">
                                                            <asp:ListItem Text="2100-X" Value="2100-X" />
                                                            <asp:ListItem Text="2200-X" Value="2200-X" />
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="div_lblBandSize">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 32%">
                                                        <asp:Label runat="server" ID="lblBandSize" Text="Size" />
                                                    </td>
                                                    <td id="div_cmbBandSize">
                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbBandSize" Width="50" CriteriaString="SIZE"
                                                            SelectedValue="2" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="div_lblPathway">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 32%">
                                                        <asp:Label runat="server" ID="lblPathway" Text="Pathway" />
                                                    </td>
                                                    <td id="div_cmbPathway">
                                                        <asp:DropDownList runat="server" ID="cmbPathway" Width="50%">
                                                            <asp:ListItem Text="pars flaccida" Value="pars flaccida" />
                                                            <asp:ListItem Text="perigastric" Value="perigastric" />
                                                            <asp:ListItem Text="2 step" Value="2 step" />
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="div_lblPosteriorFixation">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 32%">
                                                        <asp:Label runat="server" ID="lblPosterior_Fixation" Text="Posterior Fixation" />
                                                    </td>
                                                    <td id="div_chkPosteriorFixation">
                                                        <asp:CheckBox runat="server" ID="chkPosteriorFixation" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>                                        
                                        <div id="div_lblSleeveBougie">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 32%">
                                                        <asp:Label runat="server" ID="lblSleeveBougie" Text="Bougie" /></td>
                                                    <td>
                                                        <wucTextBox:TextBox runat="server" ID="txtSleeveBougie" maxLength="3" width="25%" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div class="boxBtm">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            
                            <div class="rightColumn">
                                <div class="operationDetailsPanel">
                                    <div class="boxTop">
                                        <h3 id="hRegistryDetails">
                                            Registry Details</h3>
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <tbody>
                                                <tr>
                                                    <td>
                                                    <asp:Label ID="lblRegistryProcedure" Text="Procedure" runat="server" /></td>
                                                    <td>
                                                        <wucSystemCode:SystemCodeList runat="Server" ID="cmbRegistryProcedure" CriteriaString="REGPR" Width="75" /></td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <div class="boxBtm">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="LinkBtnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="linkBtnLoad" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                <div id="div_vOtherDetails" class="operationsOtherDetails" style="display: none">
                    <div class="topFields">
                        <table width="100%">
                            <tr>
                                <td style="width: 15%;">
                                </td>
                                <td style="width: 25%">
                                    <asp:Label runat="server" ID="lblStartNeck" Text="Neck" />
                                    &nbsp;
                                    <wucTextBox:TextBox runat="server" ID="txtStartNeck" width="40%" maxLength="3" />
                                    &nbsp;
                                    <asp:Label runat="server" ID="lbltxtNeck" Text="inches" />
                                </td>
                                <td style="width: 20%">
                                    <asp:Label runat="server" ID="lblStartWaist" Text="Waist" />
                                    &nbsp;
                                    <wucTextBox:TextBox runat="server" ID="txtStartWaist" width="40%" />
                                    &nbsp;
                                    <asp:Label runat="server" ID="lbltxtWaist" Text="inches" />
                                </td>
                                <td style="width: 20%">
                                    <asp:Label runat="server" ID="lblStartHip" Text="Hip" />
                                    &nbsp;
                                    <wucTextBox:TextBox runat="server" ID="txtStartHip" width="40%" maxLength="3" />
                                    &nbsp;
                                    <asp:Label runat="server" ID="lbltxtHips" Text="inches" />
                                </td>
                                <td style="width: 20%">
                                    <asp:Label runat="server" ID="lblWHRatio" Text="WHRatio" />
                                    &nbsp;
                                    <wucTextBox:TextBox runat="server" ID="txtWHRatio" width="40%" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="detailSection">
                        <div class="boxTop">
                            <h3>
                                <asp:Label ID="lblPastAbdominal" Text="Past Abdominal" runat="server" /></h3>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:CheckBox runat="Server" ID="chkPrevAbdoSurgery" />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Text="1:" ID="lbl1" />
                                        &nbsp;
                                        <wucCode:CodeList runat="server" ID="cmbPrevAbdoSurgery1" Width="75" CriteriaString="PAS"
                                            IsEnabled="false" />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Text="2:" ID="lbl2" />
                                        &nbsp;
                                        <wucCode:CodeList runat="server" ID="cmbPrevAbdoSurgery2" Width="80" CriteriaString="PAS"
                                            IsEnabled="false" />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Text="3:" ID="lbl3" />
                                        &nbsp;
                                        <wucCode:CodeList runat="server" ID="cmbPrevAbdoSurgery3" Width="80" CriteriaString="PAS"
                                            IsEnabled="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td colspan="3">
                                        <wucTextBox:TextBox ID="txtPrevAbdoSurgeryNotes" runat="Server" width="80%" textMode="MultiLine"
                                            Enabled="false" rows="4" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="detailSection">
                        <div class="boxTop">
                            <h3>
                                <asp:Label ID="lblPastPelvic" Text="Past Pelvic" runat="server" /></h3>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:CheckBox runat="Server" ID="chkPrevPelvicSurgery" />
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Text="1:" ID="Label1" />
                                        &nbsp;
                                        <wucCode:CodeList runat="server" ID="cmbPrevPelvicSurgery1" Width="75" CriteriaString="PPS"
                                            IsEnabled="false" />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Text="2:" ID="Label2" />
                                        &nbsp;
                                        <wucCode:CodeList runat="server" ID="cmbPrevPelvicSurgery2" Width="80" CriteriaString="PPS"
                                            IsEnabled="false" />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Text="3:" ID="Label3" />
                                        &nbsp;
                                        <wucCode:CodeList runat="server" ID="cmbPrevPelvicSurgery3" Width="80" CriteriaString="PPS"
                                            IsEnabled="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td />
                                    <td colspan="3">
                                        <wucTextBox:TextBox ID="txtPrevPelvicSurgeryNotes" runat="Server" width="80%" textMode="MultiLine"
                                            Enabled="false" rows="4" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="detailSection">
                        <div class="boxTop">
                            <h3>
                                Comcomitant Surgery</h3>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:CheckBox runat="Server" ID="chkComcomitantSurgery" />
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Text="1:" ID="Label4" />
                                        &nbsp;
                                        <wucCode:CodeList runat="server" ID="cmbComcomitantSurgery1" Width="75" CriteriaString="PPS"
                                            IsEnabled="false" />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Text="2:" ID="Label5" />
                                        &nbsp;
                                        <wucCode:CodeList runat="server" ID="cmbComcomitantSurgery2" Width="80" CriteriaString="PPS"
                                            IsEnabled="false" />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Text="3:" ID="Label6" />
                                        &nbsp;
                                        <wucCode:CodeList runat="server" ID="cmbComcomitantSurgery3" Width="80" CriteriaString="PPS"
                                            IsEnabled="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td />
                                    <td colspan="3">
                                        <wucTextBox:TextBox ID="txtComcomitantSurgeryNotes" runat="Server" width="80%" textMode="MultiLine"
                                            Enabled="false" rows="4" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="clr">
                </div>
            </div>
        </div>
        <asp:UpdatePanel runat="server" ID="up_HiddenFields">
            <ContentTemplate>
                <asp:LinkButton runat="server" ID="linkBtnDelete" OnClick="linkBtnDeleteOperation_OnClick" />
                <asp:LinkButton runat="server" ID="linkBtnSave" OnClick="linkBtnSave_OnClick" />
                <asp:LinkButton runat="server" ID="linkBtnLoad" OnClick="linkBtnLoadOperation_OnClick" />
                <asp:LinkButton runat="server" ID="linkbtnLoadOperationsList" OnClick="linkbtnLoadOperationsList_OnClick" />
                <asp:HiddenField runat="server" ID="txtUseImperial" Value="0" />
                <asp:HiddenField runat="Server" ID="txtHWeight" Value="0" />
                <asp:HiddenField runat="Server" ID="txtHType" />
                <asp:HiddenField runat="server" ID="txtHCurrentDate" Value="" />
                <asp:HiddenField runat="server" ID="txtHComplication" />
                <asp:HiddenField runat="server" ID="txtHDateCreated" />
                <asp:HiddenField runat="server" ID="txtHPermissionLevel" />
                <asp:HiddenField runat="server" ID="txtHDataClamp" />
                <input type="text" runat="server" id="txtHAdmitID" value="0" style="display: none" />
                <asp:HiddenField runat="server" ID="txtHApplicationURL" />
                <asp:HiddenField runat="server" ID="txtHPageNo" Value="1" />
                <br />
                <asp:HiddenField runat="Server" ID="txtHCulture" />
                <asp:HiddenField runat="Server" ID="TitleLoaded" Value="0" />
                <textarea id="TestText" rows="10" style="width: 100%; display: none" cols="80" runat="server"></textarea>
                <asp:HiddenField runat="server" ID="txtConcurrent_Selected" />
                <asp:HiddenField runat="server" ID="txtIntraOperativeEvents_Selected" />
                <asp:HiddenField runat="server" ID="txtPreDischargeEvents_Selected" />
                <asp:HiddenField runat="server" ID="txtHDoctorBoldList" />
                <asp:HiddenField runat="server" ID="txtHDoctorBoldListWithOther" />                
                <asp:HiddenField runat="server" ID="txtHHospitalBoldList" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="LinkBtnSave" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="LinkBtnLoad" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="linkbtnLoadOperationsList" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </form>
</body>
</html>
