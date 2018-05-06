<%@ page language="C#" autoeventwireup="true" inherits="Forms_PatientsVisits_ConsultFU1_ConsultFU1Form, Lapbase.deploy" enableviewstate="true" enableEventValidation="false" viewStateEncryptionMode="Always" %>

<%@ Register TagPrefix="wucDoctor" TagName="cmbDoctorsList" Src="~/UserControl/DoctorsListWUCtrl.ascx" %>
<%@ Register TagPrefix="wucSystemCode" TagName="SystemCodeList" Src="~/UserControl/SystemCodeWUCtrl.ascx" %>
<%@ Register TagPrefix="wucPatient" TagName="PatientTitle" Src="~/UserControl/PatientTitleDataWUCtrl.ascx" %>
<%@ Register TagPrefix="wucTextBox" TagName="TextBox" Src="~/UserControl/TextBoxWUCtrl.ascx" %>
<%@ Register TagPrefix="wucMenu" TagName="Menu" Src="~/UserControl/MenuWUCtrl.ascx" %>
<%@ Register TagPrefix="wucAppSchema" TagName="AppSchema" Src="~/UserControl/AppSchemaWUCtrl.ascx" %>
<%@ Register TagPrefix="wucTextArea" TagName="TextArea" Src="~/UserControl/TextAreaWUCtrl.ascx" %>
<%@ Register TagPrefix="wucCode" TagName="CodeList" Src="~/UserControl/CodeWUCtrl.ascx" %>
<%@ Register TagPrefix="wucLogo" TagName="LogoList" Src="~/UserControl/LogoListWUCtrl.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>LapBase - A Data Manager for Bariatric Surgery</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <!-- flooding div stylesheet-->
    <link rel="stylesheet" href="../../../css/FloatingDIV/default.css" media="screen,projection"
        type="text/css" runat="server" />
    <link rel="stylesheet" href="../../../css/FloatingDIV/lightbox.css" media="screen,projection"
        type="text/css" runat="server" />
    <!-- CSS -->
    <link href='<%$ AppSettings:CssUrl%>' rel="stylesheet" type="text/css" />
    <link href="~/css/FileManagement.css" rel="Stylesheet" type="text/css" runat="server" />
    <link rel="stylesheet" href="../../../css/Slider/swing.css" type="text/css" runat="server" />
    <!-- JavaScript -->

    <script type="text/javascript" src="../../../Scripts/Global.js"></script>

    <script type="text/javascript" src="../../../Scripts/ufo.js"></script>

    <script type="text/javascript" src="Includes/ConsultFU1.js"></script>

    <script type="text/javascript" src="Includes/UploadDocument.js"></script>

    <script type="text/javascript" src="../../../Scripts/Slider/range.js"></script>

    <script type="text/javascript" src="../../../Scripts/Slider/slider.js"></script>

    <script type="text/javascript" src="../../../Scripts/Slider/timer.js"></script>

    <!-- floating div javascript-->

    <script type="text/javascript" src="../../../scripts/FloatingDIV/prototype.js"></script>

    <script type="text/javascript" src="../../../scripts/FloatingDIV/lightbox.js"></script>

    <!-- Calendar -->
    <link rel="stylesheet" href="../../../css/Calendar/calendar.css" media="screen" />

    <script type="text/javascript" src="../../../Scripts/Calendar/calendar.js"></script>

</head>
<body runat="server" id="bodyConsultFU1">
    <wucMenu:Menu runat="server" ID="mainMenu" />
    <div class="tabMenus">
        <wucAppSchema:AppSchema runat="server" ID="AppSchemaMenu" currentItem="Visits" />
        <div class="manilaTabMenu">
            <ul>
                <li id="li_Div1" class="current"><a href="#this" onclick="javascript:controlBar_Buttons_OnClick(1);">
                    <img runat="server" src="~/img/tab_progress_notes.gif" alt="Home" height="16" width="16" /><span
                        id="lblProgressNotes_M">Progress Notes</span></a></li>
                <li id="li_Div5"><a href="#this" onclick="javascript:controlBar_Buttons_OnClick(5);">
                    <img runat="server" src="~/img/tab_progress_notes.gif" alt="Home" height="16" width="16" /><span
                        id="lblFollowup_M">Follow Up</span></a></li>
                <li id="li_Div3" style="display: none"><a href="#this" onclick="javascript:controlBar_Buttons_OnClick(3);">
                    <img runat="server" src="~/img/tab_complications.gif" alt="Home" height="16" width="16" /><span
                        id="lblComplications_M">Complications</span></a></li>
                <li id="li_Div4"><a href="#this" onclick="javascript:controlBar_Buttons_OnClick(4);">
                    <img runat="server" src="~/img/tab_reports.gif" alt="Home" height="16" width="16" /><span
                        id="lblReports_M">Reports</span></a></li>
            </ul>
        </div>
    </div>
    <form id="frmConsultFU1" runat="server">
        <div class="contentArea">
            <wucPatient:PatientTitle runat="server" ID="tblPatientTitle" />
            <div class="greyContentWrap">
                <asp:ScriptManager ID="_ScriptManager" runat="server">
                </asp:ScriptManager>
                <div class="visits" id="div_vProgress_Notes">
                    <div class="expandListManila">
                        <div class="boxTop">
                            <div class="expandListManilaTitle">
                                <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                    <tbody>
                                        <tr style="vertical-align: middle;">
                                            <td style="width: 55%;">
                                                <asp:UpdatePanel runat="server" ID="upnl_BMICalculation" UpdateMode="conditional">
                                                    <ContentTemplate>
                                                        <label id="lblViewVisitDataForm" runat="Server" style="font-weight: bold">
                                                            Visit Details</label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:Label ID="lblBMI" runat="server" Font-Bold="true" Text="" />&nbsp;
                                                        <asp:Label ID="lblBMI_Value" runat="server" />&nbsp;&nbsp;
                                                        <asp:Label ID="lblLossLastVisit" runat="server" Font-Bold="true" Text="" />&nbsp;
                                                        <asp:Label ID="lblLossLastVisit_Value" runat="server" Text="" />&nbsp;&nbsp;
                                                        <asp:Label ID="lblTotalLoss" runat="server" Font-Bold="true" Text="" />&nbsp;
                                                        <asp:Label ID="lblTotalLoss_Value" runat="server" Text="" />&nbsp;&nbsp;
                                                        <asp:Label ID="lblEWLPercentage" runat="server" Font-Bold="true" Text="" />&nbsp;
                                                        <asp:Label ID="lblEWLPercentage_Value" runat="server" Text="" />&nbsp;&nbsp;
                                                        <asp:Label ID="lblWeeks" runat="server" Font-Bold="true" Text="" />&nbsp;
                                                        <asp:Label ID="lblWeeks_Value" runat="server" Text="" />
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnCalculateWeightOtherData" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td style="width: 45%;" align="right">
                                                <div style="float: right" id="divBtnCancel" runat="server">
                                                    <input id="btnCancel" value="Add as new" runat="server" onclick="javascript:btnCancelVisit_onclick(this);"
                                                        type="button" style="display: block; width: 100px" />
                                                </div>
                                                <div style="float: right" id="divSeparator" runat="server" visible="true">
                                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                                </div>
                                                <div style="float: right" id="divBtnCancelComment" runat="server">
                                                    <input id="btnCancelComment" value="Add as new" runat="server" onclick="javascript:btnCancelVisit_onclick(this);"
                                                        type="button" style="display: block; width: 130px" />
                                                </div>
                                                <div style="float: right" id="divBtnAdd" runat="server">
                                                    <input id="btnAddVisit" value="Save" type="button" style="width: 100px; display: none"
                                                        runat="server" onclick="javascript:btnAddVisit_onclick( );" onserverclick="btnAddVisit_onserverclick" />
                                                </div>
                                                <div style="float: right" id="divbtnSuperBill" runat="server">
                                                    <input id="btnSuperBill" value="Print Super Bill" type="button" style="width: 100px;
                                                        display: none" runat="server" onclick="javascript:btnAddVisit_onclick( );" onserverclick="btnPrintSuperBill_onserverclick" />
                                                </div>
                                                <div style="float: right" id="divBtnDelete" runat="server">
                                                    <input type="button" value="Delete" runat="server" id="btnDeleteVisit" style="width: 100px;
                                                        display: none" onclick="javascript:btnDelete_onclick();" onserverclick="btnDeleteVisit_onserverclick" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div class="addVisitDetails" id="divVisitDataForm" style="display: block">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tbody>
                                        <tr>
                                            <td colspan="11" style="vertical-align: middle">
                                                <div id="divErrorMessage" style="display: none;" runat="server">
                                                    <span>
                                                        <p id="pErrorMessage" runat="server">
                                                        </p>
                                                    </span>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="11" style="display:none">
                                                <input type="checkbox" id="chkCommentOnly" runat="server" onclick="javascript:btnCommentOnly_onclick(this);" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label id="lblDateSeen_PN">
                                                    Date</label></td>
                                            <td id="tdHeight">
                                                <label id="lblHeight" style="display: none">
                                                    Height</label></td>
                                            <td id="tdWeight">
                                                <label id="lblWeight_PN">
                                                    Weight</label></td>
                                            <td id="tdRV">
                                                <label id="lblRV_PN">
                                                    R.V</label></td>
                                            <td style="display: none">
                                                <label id="lblBloodPressure_PN">
                                                    B.P</label>
                                            </td>
                                            <td>
                                                <label id="lblSeenBy_PN">
                                                    Seen By</label></td>
                                            <td colspan="2" id="tdNextVisit">
                                                <label id="lblNextVisitDate_PN">
                                                    Next Visit After (M/W)</label>
                                            </td>
                                            <td colspan="3" id="tdFile">
                                                <label id="lblPhoto_PN" runat="server">
                                                    Files</label></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 12%">
                                                <asp:UpdatePanel runat="server" ID="up_txtDateSeen_PN" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <wucTextBox:TextBox runat="server" ID="txtDateSeen_PN" width="60%" />
                                                        <a href="#this" type="button" id="aCalendar" onclick="javascript:aCalendar_onclick(this, 'txtDateSeen_PN');">
                                                            [...]</a>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnAddVisit" EventName="serverclick" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnAddFile" EventName="serverclick" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                                <asp:Label runat="Server" ID="lblDateFormat" Text="" Style="display: none" />
                                            </td>
                                            <td style="width: 7%" id="tdHeightValue">
                                                <asp:UpdatePanel runat="server" ID="up_Height" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <div id="divHeight" style="display: none">
                                                            <wucTextBox:TextBox runat="Server" ID="txtHeight_PN" width="40px" maxLength="3" />
                                                            <asp:HiddenField runat="server" ID="txtHHeight_PN" Value="0" />
                                                            <label runat="server" id="lblHeightUnit_PN">
                                                                cm</label>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td style="width: 7%;" id="tdWeightValue">
                                                <asp:UpdatePanel runat="server" ID="panel2" UpdateMode="Conditional">
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnAddVisit" EventName="serverclick" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnAddFile" EventName="serverclick" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnCalculateWeightOtherData" EventName="Click" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <wucTextBox:TextBox runat="Server" ID="txtWeight_PN" width="50%" maxLength="5" />
                                                        <asp:HiddenField runat="server" ID="txtHWeight_PN" Value="0" />
                                                        <asp:Label runat="server" Text="kg" ID="lblWeightUnit_PN" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td style="width: 5%;" id="tdRVValue">
                                                <asp:UpdatePanel runat="server" ID="panel3" UpdateMode="Conditional">
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnAddVisit" EventName="serverclick" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnAddFile" EventName="serverclick" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <wucTextBox:TextBox runat="server" ID="txtRV_PN" width="80%" maxLength="5" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td style="width: 6%; display: none">
                                                <asp:UpdatePanel runat="server" ID="up_BloodPressure" UpdateMode="Conditional">
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnAddVisit" EventName="serverclick" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnAddFile" EventName="serverclick" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <wucTextBox:TextBox runat="server" ID="txtBloodPressure_PN" width="80%" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td style="width: 24%;">
                                                <asp:UpdatePanel runat="server" ID="panel4" UpdateMode="Conditional">
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnAddVisit" EventName="serverclick" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnAddFile" EventName="serverclick" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <wucDoctor:cmbDoctorsList runat="server" ID="cmbDoctorList_PN" Width="95" IsHide="False" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td style="width: 10%;" id="tdNextVisitValue">
                                                <wucTextBox:TextBox runat="server" ID="txtMonthWeek_PN" maxLength="2" width="20%" />
                                                <select name="cmbMonthWeek" id="cmbMonthWeek" class="Input" onchange="javascript:cmbMonthWeek_OnChange(this);"
                                                    style="width: 65%;">
                                                    <option value="">...</option>
                                                    <option value="1">Month(s)</option>
                                                    <option value="2">Week(s)</option>
                                                </select>
                                            </td>
                                            <td style="width: 7%;">
                                                <asp:Label runat="server" ID="lblNextVisitDate_PN_Value" />
                                                <asp:UpdatePanel runat="server" ID="panel5" UpdateMode="Conditional">
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnAddVisit" EventName="serverclick" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnAddFile" EventName="serverclick" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <asp:HiddenField runat="server" ID="txtNextVisitDate_PN" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td style="width: 10%;" id="tdFileValue">
                                                <a id="linkUpload" class="lbOn">
                                                    <input type="button" id="btnAddFile" value="Add File" runat="server" onclick="javascript:btnAddFile_OnClick();"
                                                        onserverclick="btnAddVisit_onserverclick" />
                                                </a>
                                            </td>
                                            <td style="width: 5%;">
                                                &nbsp;</td>
                                            <td style="width: 8%;">
                                                &nbsp;</td>
                                        </tr>
                                        
                                        <tr>
                                            <td colspan="11">
                                                <div id="divAddInfo1" style="display: block">
                                                    <table>
                                                        <tr runat="server">
                                                            <td colspan="3">
                                                                <br />
                                                                <label id="lblComorbidity">
                                                                    Review Comorbidity?</label>
                                                            </td>
                                                            <td colspan="4" id="rowlblChiefComplaint">
                                                                <br />
                                                                <label id="lblChiefComplaint_PN">
                                                                    Chief Complaint</label>
                                                            </td>
                                                            <td colspan="3" id="rowlblMedicalProvider">
                                                                <br />
                                                                <label id="lblMedicalProvider_PN">
                                                                    Medical Provider</label>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server">
                                                            <td colspan="2">
                                                                <input type="checkbox" id="chkComorbidity" onclick="javascript:CheckComorbidityVisit(this);" runat="server" />
                                                            </td>
                                                            <td>
                                                                &nbsp;</td>
                                                            <td colspan="3" id="rowcmbChiefComplaint">
                                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbChiefComplaint" CriteriaString="CCOM" />
                                                            </td>
                                                            <td>
                                                                &nbsp;</td>
                                                            <td colspan="2" id="rowcmbMedicalProvider">
                                                                <wucDoctor:cmbDoctorsList runat="server" ID="cmbMedicalProvider_PN" Width="95" IsHide="False" />
                                                            </td>
                                                            <td>
                                                                &nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <br />
                                                                <br />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="10">
                                                                <div class="visitReview">
                                                                    <table>
                                                                        <tr>
                                                                            <td colspan="7">
                                                                                &nbsp;</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="7">
                                                                                <label id="lblVitalSign"><b>&nbsp; Vital Signs</b></label></td>
                                                                            <td>
                                                                                <label id="lblSatietyStaging"><b>Satiety Staging</b></label></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td width="4%">
                                                                                &nbsp; <label id="lblPR">PR</label>:</td>
                                                                            <td width="13%">
                                                                                <wucTextBox:TextBox runat="server" ID="txtPR_PN" maxLength="4" width="25%" />
                                                                            </td>
                                                                            <td width="4%">
                                                                                <label id="lblRR">RR</label>:</td>
                                                                            <td width="13%">
                                                                                <wucTextBox:TextBox runat="server" ID="txtRR_PN" maxLength="4" width="25%" />
                                                                            </td>
                                                                            <td width="4%">
                                                                                <label id="lblBP">BP</label>:</td>
                                                                            <td width="13%">
                                                                                <wucTextBox:TextBox runat="server" ID="txtBP1_PN" maxLength="4" width="25%" />
                                                                                /
                                                                                <wucTextBox:TextBox runat="server" ID="txtBP2_PN" maxLength="4" width="25%" />
                                                                            </td>
                                                                            <td width="22%">
                                                                                &nbsp;</td>
                                                                            <td width="30%">
                                                                                <div class="slider" id="slider-1" style="display: block">
                                                                                    <input class="slider-input" id="slider-input-1" /></div>

                                                                                <script type="text/javascript">
                                                                            var s = new Slider(document.getElementById("slider-1"), document.getElementById("slider-input-1"));
                                                                            var satietyStaging;
                                                                            s.onchange = function () {
                                                                                document.getElementById("txtHSatiety_PN").value = s.getValue();
                                                                                document.getElementById("satietyValue").value = s.getValue();
                                                                            };
                                                                                                    
                                                                            function changeSatiety(){                                          
                                                                                satietyStaging = document.getElementById("txtHSatiety_PN").value.replace(/^\s*/, "").replace(/\s*$/, "");
                                                                                if(satietyStaging == "")
                                                                                    s.setValue(1);
                                                                                else                                                                
                                                                                    s.setValue(satietyStaging);                                                                              
                                                                                                
                                                                                document.getElementById("satietyValue").value = s.getValue();                                                                            
                                                                            }
                                                                                </script>

                                                                                <input type="hidden" runat="server" id="txtHSatiety_PN" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                &nbsp; <label id="lblNeck">Neck</label>:</td>
                                                                            <td>
                                                                                <wucTextBox:TextBox runat="server" ID="txtNeck_PN" maxLength="4" width="25%" />
                                                                                <label runat="server" id="lblNeckUnit_PN">
                                                                                    cm</label></td>
                                                                            <td>
                                                                                <label id="lblWaist">Waist</label>:</td>
                                                                            <td>
                                                                                <input type="text" runat="server" id="txtWaist_PN" size="3" maxlength="4" onkeyup="javascript:calculateWHR();" />
                                                                                <label runat="server" id="lblWaistUnit_PN">
                                                                                    cm</label></td>
                                                                            <td>
                                                                                <label id="lblHip">Hip</label>:</td>
                                                                            <td>
                                                                                <input type="text" runat="server" id="txtHip_PN" size="3" maxlength="4" onkeyup="javascript:calculateWHR();" />
                                                                                <label runat="server" id="lblHipUnit_PN">
                                                                                    cm</label></td>
                                                                            <td>
                                                                                <label id="lblWHRTitle">WHR</label>:
                                                                                <label id="lblWHR" runat="server" />
                                                                                <asp:HiddenField runat="server" ID="txtHNeck_PN" Value="0" />
                                                                                <asp:HiddenField runat="server" ID="txtHWaist_PN" Value="0" />
                                                                                <asp:HiddenField runat="server" ID="txtHHip_PN" Value="0" />
                                                                            </td>
                                                                            <td align='center'>
                                                                                <input id="satietyValue" onkeyup="s.setValue(parseInt(this.value))" size="2" /></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                &nbsp;</td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        
                                        
                                        <tr>
                                            <td colspan="10">
                                                <div id="div_SEDetail" style="display: none">
                                                    <table>
                                                        <tr>
                                                            <td width="32%">
                                                                <asp:Label ID="lblRegistrySEDetail" Text="Has this patient had a SE?" runat="server" /></td>
                                                                <td width="63%">
                                                                <input type="radio" name="rbRegistrySE" id="rbRegistrySEY" value="Y" runat="server" onclick="checkRegistrySE();" />
                                                                                    &nbsp;Yes &nbsp; &nbsp;
                                                                                <input type="radio" name="rbRegistrySE" id="rbRegistrySEN" value="N" checked=true runat="server" onclick="checkRegistrySE();"/>
                                                                                    &nbsp;No &nbsp; &nbsp; &nbsp; &nbsp; <wucSystemCode:SystemCodeList runat="server" ID="cmbSEList" CriteriaString="SE" BoldData="SE" Width="50" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <wucTextArea:TextArea runat="server" ID="txtRegistrySEReason" width="99%" rows="1" runat="server"/>
                                                                <br />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="10">
                                                <div id="div_diabetesDetail" style="display: none">
                                                    <table>
                                                        <tr>
                                                            <td width="20%">
                                                                <asp:Label ID="lblRegistryDiabetesDetail" Text="Does this patient have diabetes?" runat="server" /></td>
                                                                <td width="80%">
                                                                <input type="radio" name="rbRegistryDiabetes" id="rbRegistryDiabetesY" value="Y" runat="server" onclick="checkRegistryDiabetes();" />
                                                                                    &nbsp;Yes &nbsp; &nbsp;
                                                                                <input type="radio" name="rbRegistryDiabetes" id="rbRegistryDiabetesN" value="N" checked=true runat="server" onclick="checkRegistryDiabetes();"/>
                                                                                    &nbsp;No &nbsp; &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" rowspan="5">
                                                                <asp:Label ID="lblRegistryTreatment" Text="Treatment" runat="server" />
                                                            </td>
                                                            <td>
                                                                <input type="checkbox" runat="server" id="chkRegistryOral" /> &nbsp;<asp:label ID="lblRegistryOral" runat="server" Text="Oral (tablets)" /> &nbsp; &nbsp; &nbsp; <input type="checkbox" runat="server" id="chkRegistryInsulin" /> &nbsp;<asp:label ID="lblRegistryInsulin" runat="server" Text="Insulin" /> &nbsp; &nbsp; &nbsp; <input type="checkbox" runat="server" id="chkRegistryCombination" /> &nbsp;<asp:label ID="lblRegistryCombination" runat="server" Text="Combination (of pharmacoptherapies)" /> &nbsp; &nbsp; &nbsp; <input type="checkbox" runat="server" id="chkRegistryOther" /> &nbsp;<asp:label ID="lblRegistryOther" runat="server" Text="Other (e.g. Pump)" /> &nbsp; &nbsp; &nbsp; <input type="checkbox" runat="server" id="chkRegistryDiet" /> &nbsp;<asp:label ID="lblRegistryDiet" runat="server" Text="Diet & Exercise alone" />
                                                                <br />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="10">
                                                <div id="div_repoerationDetail" style="display: none">
                                                    <table>
                                                        <tr>
                                                            <td width="32%">
                                                                <asp:Label ID="lblRegistryReoperationDetail" Text="Has this patient had a re-operation in the past 12 Months?" runat="server" /></td>
                                                                <td width="63%">
                                                                <input type="radio" name="rbRegistryReoperation" id="rbRegistryReoperationY" value="Y" runat="server" onclick="checkRegistryDiabetes();" />
                                                                                    &nbsp;Yes &nbsp; &nbsp;
                                                                                <input type="radio" name="rbRegistryReoperation" id="rbRegistryReoperationN" value="N" checked=true runat="server" onclick="checkRegistryDiabetes();"/>
                                                                                    &nbsp;No &nbsp; &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <wucTextArea:TextArea runat="server" ID="txtRegistryReoperationReason" width="99%" rows="1" runat="server"/>
                                                                <br /><br />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="10" style="vertical-align: top;">
                                                <b><label runat="server" ID="lblProgressNote_PN">Clinical Notes</label></b>
                                                <asp:UpdatePanel runat="server" ID="panel6">
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnAddVisit" EventName="serverclick" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnAddFile" EventName="serverclick" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <wucTextArea:TextArea runat="server" ID="txtNotes_PN" width="99%" rows="8" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="11" style="color:blue;font-size:14px">
                                                <br />
                                                <div style="display:none" id="divRegistryNote"><b>Please complete Registry Review</b></div>
                                            </td>
                                        </tr>
                                        <tr id="rowRegistry">
                                            <td colspan="11">
                                                <div id="divRegistry" style="display:none">
                                                    <br />Registry Review &nbsp; <input type="checkbox" id="chkRegistryReview" runat="server" onclick="javascript:CheckReviewRegistry();"/>
                                                    <div id="divRegistryReview" style="display:none">
                                                        <table width="100%">
                                                            <tr>
                                                                <td width="20%">
                                                                    <div id="lblRegistrySleepApnea" style="display:none">
                                                                        Sleep Apnea
                                                                    </div>
                                                                </td>
                                                                <td width="80%">
                                                                    <div id="selRegistrySleepApnea" style="display:none">
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbRegistrySleepApnea" Width="40" CriteriaString="REG" BoldData="REG" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="20%">
                                                                    <div id="lblRegistryGerd" style="display:none">
                                                                        GERD Requiring Medications
                                                                    </div>
                                                                </td>
                                                                <td width="80%">
                                                                    <div id="selRegistryGerd" style="display:none">
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbRegistryGerd" Width="40" CriteriaString="REG" BoldData="REG" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="20%">
                                                                    <div id="lblRegistryHyperlipidemia" style="display:none">
                                                                        Hyperlipidemia
                                                                    </div>
                                                                </td>
                                                                <td width="80%">
                                                                    <div id="selRegistryHyperlipidemia" style="display:none">
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbRegistryHyperlipidemia" Width="40" CriteriaString="REG" BoldData="REG" />
                                                                    </div>
                                                                </td>
                                                            </tr><tr>
                                                                <td width="20%">
                                                                    <div id="lblRegistryHypertension" style="display:none">
                                                                        Hypertension
                                                                    </div>
                                                                </td>
                                                                <td width="80%">
                                                                    <div id="selRegistryHypertension" style="display:none">
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbRegistryHypertension" Width="40" CriteriaString="REG" BoldData="REG" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="20%">
                                                                    <div id="lblRegistryDiabetes" style="display:none">
                                                                        Diabetes
                                                                    </div>
                                                                </td>
                                                                <td width="80%">
                                                                    <div id="selRegistryDiabetes" style="display:none">
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbRegistryDiabetes" Width="40" CriteriaString="REG" BoldData="REG" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="11">
                                                <div id="divAddInfo2">
                                                    <table>
                                                        <tr id="rowlblLapbandAdjustment" runat="server" style="display: none">
                                                            <td colspan="10" style="vertical-align: top;">
                                                                <br />
                                                                <asp:Label runat="server" Text="Adjustment of Band" ID="lblLapbandAdjustment_PN"
                                                                    Font-Bold="true" />
                                                                <contenttemplate>
                                                            <wucTextArea:textarea runat="server" id="txtLapbandAdjustment_PN" width="99%" rows="2" />
                                                        </contenttemplate>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server">
                                                            <td colspan="10">
                                                                <br />
                                                                <br />
                                                                <label id="lblAdjustmentPerformed" style="color: Blue" onclick="javascript:showAdjustment();" onmouseover="javascript:this.style.cursor='pointer';"
                                                                    onmouseout="javascript:this.style.cursor='';">
                                                                    <img src="../../../img/button_plus.gif" id="imgShowAdjustment" width="11" height="11"
                                                                        border="0" />
                                                                    <b>Adjustment Performed</b></label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="10">
                                                                <div id="div_adjustmentDetail" style="display: block">
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td style="width: 20px; height: 20px">
                                                                                            <input type="checkbox" id="chkAdjConsent" runat="server" /></td>
                                                                                        <td>
                                                                                            <label id="lblConsent">Consent form reviewed and signed</label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="height: 20px">
                                                                                            <input type="checkbox" id="chkAdjAntiseptic" runat="server" /></td>
                                                                                        <td>
                                                                                            <label id="lblAntiseptic">Antiseptic skin prep performed</label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="height: 20px">
                                                                                            <input type="checkbox" id="chkAdjAnesthesia" runat="server" /></td>
                                                                                        <td>
                                                                                            <label id="lblLocalAnesthesia">Local anesthesia with</label>
                                                                                            <input id="txtAdjAnesthesiaVol_PN" style="font-size: x-small; width: 25px" type="text"
                                                                                                maxlength="4" runat="server" />
                                                                                            <label id="lblLocalAnesthesia2">ml 1% lidocaine provided</label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="height: 20px">
                                                                                            <input type="checkbox" id="chkAdjNeedle" runat="server" /></td>
                                                                                        <td>
                                                                                            <label id="lblNonCoring">Non coring needle used to access the port</label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="height: 20px" rowspan="5" valign="top">
                                                                                            <input type="checkbox" id="chkAdjVolume" runat="server" /></td>
                                                                                        <td>
                                                                                            <label id="lblAdjustVolume">Adjust volume with saline:</label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="height: 20px">
                                                                                            <label id="lblInitialVolume">Initial volume</label>
                                                                                            <input runat="server" id="txtInitialVol_PN" style="font-size: x-small; width: 25px"
                                                                                                type="text" maxlength="4" onkeyup="javascript:calculateVol();" />
                                                                                            ml</td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="height: 20px">
                                                                                            <label id="lblVolumeAdded">Volume added</label>
                                                                                            <input runat="server" id="txtAddVol_PN" style="font-size: x-small; width: 25px" type="text"
                                                                                                maxlength="4" onkeyup="javascript:calculateVol();" />
                                                                                            ml</td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="height: 20px">
                                                                                            <label id="lblVolumeRemoved">Volume removed</label>
                                                                                            <input runat="server" id="txtRemoveVol_PN" style="font-size: x-small; width: 25px"
                                                                                                type="text" maxlength="4" onkeyup="javascript:calculateVol();" />
                                                                                            ml</td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="height: 20px">
                                                                                            <label id="lblFinalVolume">Final volume</label> <b>
                                                                                                <label id="lblFinalVol" runat="server" />
                                                                                            </b>ml</td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="height: 20px">
                                                                                            <input type="checkbox" id="chkAdjTolerate" runat="server" /></td>
                                                                                        <td>
                                                                                            <label id="lblPatientTolerated">Patient tolerated the procedure well, could drink water easily at completion and
                                                                                            was given post-adjustment instructions</label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="height: 20px">
                                                                                            <input type="checkbox" id="chkAdjBarium" runat="server" /></td>
                                                                                        <td>
                                                                                            <label id="lblBarium">Barium or gastrograffin to perform swallow per standing medical order</label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="height: 20px">
                                                                                            <input type="checkbox" id="chkAdjOmni" runat="server" /></td>
                                                                                        <td>
                                                                                            <label id="lblOmnipaque">Omnipaque for suspected leak per standing medical order</label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="width: 20px; height: 20px">
                                                                                            <input type="checkbox" id="chkAdjProtocol" runat="server" /></td>
                                                                                        <td>
                                                                                            <label id="lblAdjustment">Adjustment according to protocol</label></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server">
                                                            <td colspan="10">
                                                                <br />
                                                                <br />
                                                                <label id="lblShowDetails" style="color: Blue" onclick="javascript:showDetails();" onmouseover="javascript:this.style.cursor='pointer';"
                                                                    onmouseout="javascript:this.style.cursor='';">
                                                                    <img src="../../../img/button_plus.gif" id="imgShowDetails" width="11" height="11"
                                                                        border="0" />
                                                                    <b>Show Details</b></label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="10">
                                                                <div id="div_visitDetails" style="display: block">
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td id="rowlblSupportGroup" colspan="2">
                                                                                <br />
                                                                                <label id="lblSupportGroup_PN">
                                                                                    Support Group</label>
                                                                            </td>
                                                                            <td colspan="2">
                                                                                <br />
                                                                                <label id="lblReview">
                                                                                Reviewed the progress notes for this patient?</label>
                                                                            </td>
                                                                            <td>
                                                                                <br />
                                                                                <label id="lblLetterSent">
                                                                                Letter Sent?</label>
                                                                            </td>
                                                                            <td width="40%">&nbsp;</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td width="30%">
                                                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbSupportGroup" FirstRow="False"
                                                                                    CriteriaString="SG" BoldData="SG" />
                                                                            </td>
                                                                            <td width="2%">&nbsp;</td>
                                                                            <td width="26%">
                                                                                <input type="checkbox" id="chkReview" runat="server" />
                                                                            </td>
                                                                            <td width="2%">&nbsp;</td>
                                                                            <td width="10%">
                                                                                <input type="checkbox" id="chkLetterSent" runat="server" />
                                                                            </td>
                                                                            <td width="30%">&nbsp;</td>
                                                                        </tr>
                                                                        <tr>
                                                                        <td>
                                                                        <br /><br />&nbsp;</td>
                                                                        </tr>
                                                                    </table>
                                                                    <table>
                                                                        <tr>
                                                                            <td colspan="2">
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <label id="lblPFSH">PFSH</label></td>
                                                                                        <td>
                                                                                            <input type="checkbox" id="chkDetPFSH" onclick="javascript:checkTextBox('PFSH','check');" />
                                                                                            &nbsp;
                                                                                            <input type="text" runat="server" id="txtDetPFSH_PN" size="100" onkeyup="javascript:checkTextBox('PFSH','input');" /></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <label id="lblGeneral">General</label></td>
                                                                                        <td>
                                                                                            <input type="checkbox" id="chkDetGeneral" onclick="javascript:checkTextBox('General','check');" />
                                                                                            &nbsp;
                                                                                            <input type="text" runat="server" id="txtDetGeneral_PN" size="100" onkeyup="javascript:checkTextBox('General','input');" /></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <label id="lblGastrointestinal">Gastrointestinal</label></td>
                                                                                        <td>
                                                                                            <input type="checkbox" id="chkDetGastro" onclick="javascript:checkTextBox('Gastro','check');" />
                                                                                            &nbsp;
                                                                                            <input type="text" runat="server" id="txtDetGastro_PN" size="100" onkeyup="javascript:checkTextBox('Gastro','input');" /></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <label id="lblCardiovascular">Cardiovascular</label></td>
                                                                                        <td>
                                                                                            <input type="checkbox" id="chkDetCardio" onclick="javascript:checkTextBox('Cardio','check');" />
                                                                                            &nbsp;
                                                                                            <input type="text" runat="server" id="txtDetCardio_PN" size="100" onkeyup="javascript:checkTextBox('Cardio','input');" /></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <label id="lblRespiratory">Respiratory</label></td>
                                                                                        <td>
                                                                                            <input type="checkbox" id="chkDetResp" onclick="javascript:checkTextBox('Resp','check');" />
                                                                                            &nbsp;
                                                                                            <input type="text" runat="server" id="txtDetResp_PN" size="100" onkeyup="javascript:checkTextBox('Resp','input');" /></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <label id="lblMusculoskeletal">Musculoskeletal</label></td>
                                                                                        <td>
                                                                                            <input type="checkbox" id="chkDetMusculo" onclick="javascript:checkTextBox('Musculo','check');" />
                                                                                            &nbsp;
                                                                                            <input type="text" runat="server" id="txtDetMusculo_PN" size="100" onkeyup="javascript:checkTextBox('Musculo','input');" /></td>
                                                                                    </tr>
                                                                                    <tr style="display: none">
                                                                                        <td>
                                                                                            <label id="lblExtremities">Extremities</label></td>
                                                                                        <td>
                                                                                            <input type="checkbox" id="chkDetExtr" onclick="javascript:checkTextBox('Extr','check');" />
                                                                                            &nbsp;
                                                                                            <input type="text" runat="server" id="txtDetExtr_PN" size="100" onkeyup="javascript:checkTextBox('Extr','input');" /></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <label id="lblGenitourinary">Genitourinary</label></td>
                                                                                        <td>
                                                                                            <input type="checkbox" id="chkDetGenito" onclick="javascript:checkTextBox('Genito','check');" />
                                                                                            &nbsp;
                                                                                            <input type="text" runat="server" id="txtDetGenito_PN" size="100" onkeyup="javascript:checkTextBox('Genito','input');" /></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <label id="lblSkinInteguments">Skin and integuments</label></td>
                                                                                        <td>
                                                                                            <input type="checkbox" id="chkDetSkin" onclick="javascript:checkTextBox('Skin','check');" />
                                                                                            &nbsp;
                                                                                            <input type="text" runat="server" id="txtDetSkin_PN" size="100" onkeyup="javascript:checkTextBox('Skin','input');" /></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <label id="lblNeurological">Neurological</label></td>
                                                                                        <td>
                                                                                            <input type="checkbox" id="chkDetNeuro" onclick="javascript:checkTextBox('Neuro','check');" />
                                                                                            &nbsp;
                                                                                            <input type="text" runat="server" id="txtDetNeuro_PN" size="100" onkeyup="javascript:checkTextBox('Neuro','input');" /></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <label id="lblPsychiatric">Psychiatric</label></td>
                                                                                        <td>
                                                                                            <input type="checkbox" id="chkDetPsych" onclick="javascript:checkTextBox('Psych','check');" />
                                                                                            &nbsp;
                                                                                            <input type="text" runat="server" id="txtDetPsych_PN" size="100" onkeyup="javascript:checkTextBox('Psych','input');" /></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <label id="lblEndocrine">Endocrine</label></td>
                                                                                        <td>
                                                                                            <input type="checkbox" id="chkDetEndo" onclick="javascript:checkTextBox('Endo','check');" />
                                                                                            &nbsp;
                                                                                            <input type="text" runat="server" id="txtDetEndo_PN" size="100" onkeyup="javascript:checkTextBox('Endo','input');" /></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <label id="lblHematologic">Hematologic / Lymphatic</label></td>
                                                                                        <td>
                                                                                            <input type="checkbox" id="chkDetHema" onclick="javascript:checkTextBox('Hema','check');" />
                                                                                            &nbsp;
                                                                                            <input type="text" runat="server" id="txtDetHema_PN" size="100" onkeyup="javascript:checkTextBox('Hema','input');" /></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <label id="lblENT">ENT</label></td>
                                                                                        <td>
                                                                                            <input type="checkbox" id="chkDetENT" onclick="javascript:checkTextBox('ENT','check');" />
                                                                                            &nbsp;
                                                                                            <input type="text" runat="server" id="txtDetENT_PN" size="100" onkeyup="javascript:checkTextBox('ENT','input');" /></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <label id="lblEyes">Eyes</label></td>
                                                                                        <td>
                                                                                            <input type="checkbox" id="chkDetEyes" onclick="javascript:checkTextBox('Eyes','check');" />
                                                                                            &nbsp;
                                                                                            <input type="text" runat="server" id="txtDetEyes_PN" size="100" onkeyup="javascript:checkTextBox('Eyes','input');" /></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <label id="lblAdjustMedications">Adjust medications</label></td>
                                                                                        <td>
                                                                                            <input type="checkbox" id="chkDetMeds" onclick="javascript:checkTextBox('Meds','check');" />
                                                                                            &nbsp;
                                                                                            <input type="text" runat="server" id="txtDetMeds_PN" size="100" onkeyup="javascript:checkTextBox('Meds','input');" /></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                </table>
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
                    <div class="darkGreyTabMenuWrap">
                        <div class="darkGreyTabMenu">
                            <ul>
                                <li class="current"><a href="#this" id="li_VisitList" onclick="javascript:VisitPagesClick(1);">
                                    Visit List</a></li>
                                <li style="display: block"><a href="#this" id="li_ComorbidityList" onclick="javascript:VisitPagesClick(2);">
                                    Comorbidity List</a></li>
                                <li id="li_BloodPressure" style="display: none"><a href="#this" onclick="javascript:ComorbidityPagesClick(1);">
                                    Blood Pressure</a></li>
                                <li id="li_MajorComorbidity" style="display: none"><a href="#this" onclick="javascript:ComorbidityPagesClick(2);">
                                    Major Comorbidity</a></li>
                                <li id="li_BoldComorbidity" style="display: none"><a href="#this" onclick="javascript:ComorbidityPagesClick(3);">
                                    BOLD Comorbidity</a></li>
                                <li id="li_Vitamins" style="display: none"><a href="#this" onclick="javascript:ComorbidityPagesClick(4);">
                                    Medication / Vitamins & Minerals</a></li>
                            </ul>
                        </div>
                        <div class="clr">
                        </div>
                    </div>
                    <div class="expandList" id="div_Lists">
                        <div class="boxTop">
                            <div class="expandListTitle">
                                <table width="100%" border="0" cellpadding="3" cellspacing="0">
                                    <tr style="vertical-align: middle;">
                                        <td style="width: 10%">
                                            <asp:Label Text="Date" ID="lblDateSeen_TH" runat="server" /></td>
                                        <td style="width: 5%">
                                            <asp:Label Text="Weeks" ID="lblWeeks_TH" runat="server" /></td>
                                        <td style="width: 5%">
                                            <asp:Label Text="Weight" ID="lblWeight_TH" runat="server" /></td>
                                        <td style="width: 5%">
                                            <asp:Label Text="R.V" ID="lblReservoirVolume_TH" runat="server" /></td>
                                        <td style="width: 5%">
                                            <asp:Label Text="Total loss" ID="lblWeightLoss_TH" runat="server" /></td>
                                        <td style="width: 10%; display: none;">
                                            <asp:Label Text="Loss/week" ID="lblWeightLossPerWeek_TH" runat="server" /></td>
                                        <td style="width: 5%">
                                            <asp:Label Text="BMI" ID="lblBMI_TH" runat="server" /></td>
                                        <td style="width: 5%">
                                            <asp:Label Text="% EWL" ID="lblEWL_TH" runat="server" /></td>
                                        <td style="width: 5%; display: none;">
                                            <asp:Label Text="B.P" ID="lblBloodPressure_TH" runat="server" /></td>
                                        <td style="width: 13%">
                                            <asp:Label Text="Seen By" ID="lblSeenBy_TH" runat="server" /></td>
                                        <td style="width: 7%">
                                            <asp:Label Text="Next Visit" ID="lblNextVisitDate_TH" runat="server" /></td>
                                        <td style="width: 20%">
                                            <asp:Label Text="Documents" ID="lblInvestigations_TH" runat="server" /></td>
                                    </tr>
                                </table>
                            </div>
                            <asp:UpdatePanel runat="server" ID="updatePanelList" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnAddVisit" EventName="serverclick" />
                                    <asp:AsyncPostBackTrigger ControlID="btnAddFile" EventName="serverclick" />
                                </Triggers>
                                <ContentTemplate>
                                    <div class="expandListScroll" id="div_VisitsList" runat="server" style="display: none" enableviewstate="false">
                                    </div>
                                    <div class="expandListScroll" id="div_ComorbidityVisitsList" runat="server" style="display: none" enableviewstate="false">
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div style="display: none; text-align:right" id="div_LoadAllVisit" enableviewstate = "false" runat="server">
                                There are more visits recorded for this patient. It will take one minute or two to retrieve these visits from archives. Click <a href="#this" onclick="javascript:LoadAllVisit();">"HERE"</a> to upload these earlier visits.
                            </div>
                            <div class="boxBtm">
                            </div>
                        </div>
                    </div>
                    <div id="div_Comorbidity" style="display: none" class="comorbiditiesHistory">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div style="text-align: center; display: none" runat="server" id="divBaselineSetup">
                                    <br />
                                    No baseline comorbidities set for patient - please set them up in Patient Details
                                    > Baseline Comorbidity
                                    <br />
                                    <br />
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnLoadVisitData" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnLoadBaselineComorbidity" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                        <div id="div_BoldComorbidity" class="comorbiditiesHistory" style="display: block">
                            <asp:UpdatePanel runat="server" ID="up_Hypertension" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnAddVisit" EventName="serverclick" />
                                    <asp:AsyncPostBackTrigger ControlID="btnAddFile" EventName="serverclick" />
                                    <asp:AsyncPostBackTrigger ControlID="btnLoadVisitData" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnLoadBaselineComorbidity" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                                    <div id="comorbidityBold" runat="Server">
                                        <div class="sectionBox1" id="divCardiovascularDisease" runat="server" style="display: none">
                                            <div class="boxTop">
                                                <table width="880" border="0" cellpadding="4" cellspacing="4">
                                                    <tr>
                                                        <td>
                                                            <h3 id="hCardiovascularDisease">
                                                                CARDIOVASCULAR DISEASE</h3>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 300px; vertical-align: top">
                                                            <asp:Table ID="tblHypertension" Width="100%" BorderWidth="0" CellPadding="0" CellSpacing="0"
                                                                CssClass="inner_table" runat="server" Style="display: block">
                                                                <asp:TableRow>
                                                                    <asp:TableCell ColumnSpan="2" Style="height: 29px; text-align: left">Current visit</asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow Style="height: 29px; vertical-align: top; text-align: left" runat="server"
                                                                    ID="tr_cmbHypertension">
                                                                    <asp:TableCell Style="width: 100px">
                                                                        <label id="lblHypertension" runat="server">
                                                                            Hypertension</label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell Style="width: 200px">
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbHypertension" CriteriaString="HYPER"
                                                                            BoldData="CVS" SCode="PRE" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow Style="height: 29px; vertical-align: top; text-align: left" runat="server"
                                                                    ID="tr_cmbCongestive">
                                                                    <asp:TableCell>
                                                                        <label id="lblCongestiveHeartFailure" runat="server">
                                                                            Congestive Heart Failure</label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbCongestive" CriteriaString="CONG"
                                                                            BoldData="CVS" SCode="PRE" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow Style="height: 29px; vertical-align: top; text-align: left" runat="server"
                                                                    ID="tr_cmbIschemic">
                                                                    <asp:TableCell>
                                                                        <label id="lblIschemicHeartDisease" runat="server">
                                                                            Ischemic Heart Disease</label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbIschemic" CriteriaString="ISCH"
                                                                            BoldData="CVS" SCode="PRE" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow Style="height: 29px; vertical-align: top; text-align: left" ID="tr_cmbAngina"
                                                                    runat="server">
                                                                    <asp:TableCell>
                                                                        <label id="lblAnginaAssessment" runat="server">
                                                                            Angina Assessment</label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbAngina" CriteriaString="ANGI"
                                                                            BoldData="CVS" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow Style="height: 29px; vertical-align: top; text-align: left" runat="server"
                                                                    ID="tr_cmbPeripheral">
                                                                    <asp:TableCell>
                                                                        <label id="lblPeripheralVascularDisease" runat="Server">
                                                                            Peripheral Vascular Disease</label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbPeripheral" CriteriaString="PERI"
                                                                            BoldData="CVS" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow Style="height: 29px; vertical-align: top; text-align: left" runat="server"
                                                                    ID="tr_cmbLower">
                                                                    <asp:TableCell>
                                                                        <label id="lblLowerExtremityEdema" runat="Server">
                                                                            Lower Extremity Edema</label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbLower" CriteriaString="LOWE"
                                                                            BoldData="CVS" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow Style="height: 29px; vertical-align: top; text-align: left" runat="server"
                                                                    ID="tr_cmbDVT">
                                                                    <asp:TableCell>
                                                                        <label id="lblDVT" runat="server">
                                                                            DVT/PE</label></asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbDVT" CriteriaString="DVTP" BoldData="CVS"
                                                                            SCode="PRE" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                            </asp:Table>
                                                        </td>
                                                        <td style="width: 300px; vertical-align: top">
                                                            <div runat="server" id="divCVSHistoryData" />
                                                        </td>
                                                        <td style="width: 300px; vertical-align: top">
                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="inner_table"
                                                                runat="server" id="tblCSVNote">
                                                                <tr>
                                                                    <td style="width: 300px; text-align: left; height: 29px; vertical-align: top">
                                                                        Notes:</td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="text-align: left; vertical-align: top">
                                                                        <wucTextBox:TextBox runat="server" ID="txtCVSNote" textMode="MultiLine" width="100%"
                                                                            maxLength="512" rows="5" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div class="boxBtm">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="sectionBox1" runat="server" id="divMetabolic" style="width: 900px; display: none">
                                            <div class="boxTop" style="width: 900px;">
                                                <table width="880" border="0" cellpadding="4" cellspacing="4">
                                                    <tr>
                                                        <td>
                                                            <h3 id="hMetabolic">
                                                                METABOLIC</h3>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 300px; vertical-align: top">
                                                            <asp:Table ID="tblMetabolic" Width="100%" BorderWidth="0" CellPadding="0" CellSpacing="0"
                                                                CssClass="inner_table" runat="server" Style="display: block; text-align: left">
                                                                <asp:TableRow>
                                                                    <asp:TableCell ColumnSpan="2" Style="height: 29px; text-align: left">Current visit</asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow Style="vertical-align: top; text-align: left" runat="server" ID="tr_cmbGlucose">
                                                                    <asp:TableCell Style="width: 100px">
                                                                        <label id="lblGlucoseMetabolism" runat="server">
                                                                            Glucose Metabolism</label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell Style="width: 200px">
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbGlucose" CriteriaString="GLUC"
                                                                            BoldData="MET" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow Style="vertical-align: top; text-align: left" runat="server" ID="tr_cmbLipids">
                                                                    <asp:TableCell>
                                                                        <label id="lblLipids" runat="server">
                                                                            Lipids (Dyslipidemia or Hyperlipidemia)</label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbLipids" CriteriaString="LIPI"
                                                                            BoldData="MET" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow Style="vertical-align: top; text-align: left" runat="server" ID="tr_cmbGout">
                                                                    <asp:TableCell>
                                                                        <label id="lblGoutHyperuricemia" runat="server">
                                                                            Gout Hyperuricemia</label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbGout" CriteriaString="GOUT" BoldData="MET" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                            </asp:Table>
                                                        </td>
                                                        <td style="width: 300px; vertical-align: top">
                                                            <div runat="server" id="divMETHistoryData" />
                                                        </td>
                                                        <td style="width: 300px; vertical-align: top">
                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="inner_table"
                                                                runat="server" id="tblMETNote">
                                                                <tr>
                                                                    <td style="width: 300px; text-align: left; height: 29px; vertical-align: top">
                                                                        Notes:</td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="text-align: left; vertical-align: top">
                                                                        <wucTextBox:TextBox runat="server" ID="txtMETNote" textMode="MultiLine" width="100%"
                                                                            maxLength="512" rows="5" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div class="boxBtm">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="sectionBox1" runat="server" id="divPulmonary" style="width: 900px; display: none">
                                            <div class="boxTop" style="width: 900px;">
                                                <table width="880" border="0" cellpadding="4" cellspacing="4">
                                                    <tr>
                                                        <td>
                                                            <h3 id="hPulmonary">
                                                                PULMONARY</h3>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 300px; vertical-align: top">
                                                            <asp:Table ID="tblPulmonary" Width="100%" BorderWidth="0" CellPadding="0" CellSpacing="0"
                                                                CssClass="inner_table" runat="server" Style="display: block; text-align: left">
                                                                <asp:TableRow>
                                                                    <asp:TableCell ColumnSpan="2" Style="height: 29px; text-align: left">Current visit</asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbObstructive" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell Style="width: 100px">
                                                                        <label id="lblObstructiveSleepApneaSyndrome" runat="server">
                                                                            Obstructive Sleep Apnea</label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell Style="width: 200px">
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbObstructive" CriteriaString="OBST"
                                                                            BoldData="PUL" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbObesity" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblObesityHypoventilationSyndrome" runat="server">
                                                                            Obesity Hypoventilation Syndrome</label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbObesity" CriteriaString="OBES"
                                                                            BoldData="PUL" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbPulmonary" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblPulmonaryHypertension" runat="server">
                                                                            Pulmonary Hypertension</label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbPulmonary" CriteriaString="PULM"
                                                                            BoldData="PUL" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbAsthma" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblAsthma" runat="server">
                                                                            Asthma</label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbAsthma" CriteriaString="ASTH"
                                                                            BoldData="PUL" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                            </asp:Table>
                                                        </td>
                                                        <td style="width: 300px; vertical-align: top">
                                                            <div runat="server" id="divPULHistoryData" />
                                                        </td>
                                                        <td style="width: 300px; vertical-align: top">
                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="inner_table"
                                                                runat="server" id="tblPULNote">
                                                                <tr>
                                                                    <td style="width: 300px; text-align: left; height: 29px; vertical-align: top">
                                                                        Notes:</td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="text-align: left; vertical-align: top">
                                                                        <wucTextBox:TextBox runat="server" ID="txtPULNote" textMode="MultiLine" width="100%"
                                                                            maxLength="512" rows="5" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div class="boxBtm">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="sectionBox1" runat="server" id="divGastroIntestinal" style="width: 900px;
                                            display: none">
                                            <div class="boxTop" style="width: 900px">
                                                <table width="880" border="0" cellpadding="4" cellspacing="4">
                                                    <tr>
                                                        <td>
                                                            <h3 id="hGastroIntestinal">
                                                                GASTROINTESTINAL</h3>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 300px; vertical-align: top">
                                                            <asp:Table ID="tblGastroIntestinal" Width="100%" BorderWidth="0" CellPadding="0"
                                                                CellSpacing="0" CssClass="inner_table" runat="server" Style="display: block;
                                                                text-align: left">
                                                                <asp:TableRow>
                                                                    <asp:TableCell ColumnSpan="2" Style="height: 29px; text-align: left">Current visit</asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbGred" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell Style="width: 100px">
                                                                        <label id="lblGerd" runat="server">
                                                                            GERD</label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell Style="width: 200px">
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbGred" CriteriaString="GERD" BoldData="GAS"
                                                                            SCode="PRE" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbCholelithiasis" Style="vertical-align: top;
                                                                    text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblCholelithiasis" runat="server">
                                                                            Cholelithiasis</label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbCholelithiasis" CriteriaString="CHOL"
                                                                            BoldData="GAS" SCode="PRE" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbLiver" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblLiverDisease" runat="Server">
                                                                            Liver Disease</label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbLiver" CriteriaString="LIVE"
                                                                            BoldData="GAS" SCode="PRE" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                            </asp:Table>
                                                        </td>
                                                        <td style="width: 300px; vertical-align: top">
                                                            <div runat="server" id="divGASHistoryData" />
                                                        </td>
                                                        <td style="width: 300px; vertical-align: top">
                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="inner_table"
                                                                runat="server" id="tblGASNote">
                                                                <tr>
                                                                    <td style="width: 300px; text-align: left; height: 29px; vertical-align: top">
                                                                        Notes:</td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="text-align: left; vertical-align: top">
                                                                        <wucTextBox:TextBox runat="server" ID="txtGASNote" textMode="MultiLine" width="100%"
                                                                            maxLength="512" rows="5" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div class="boxBtm">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="sectionBox1" runat="server" id="divMusculoskeletal" style="width: 900px;
                                            display: none">
                                            <div class="boxTop" style="width: 900px">
                                                <table width="880" border="0" cellpadding="4" cellspacing="4">
                                                    <tr>
                                                        <td>
                                                            <h3 id="hMusculoskeletal">
                                                                MUSCULOSKELETAL</h3>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 300px; vertical-align: top">
                                                            <asp:Table ID="tblMusculoskeletal" Width="100%" BorderWidth="0" CellPadding="0" CellSpacing="0"
                                                                CssClass="inner_table" runat="server" Style="display: block; text-align: left">
                                                                <asp:TableRow>
                                                                    <asp:TableCell ColumnSpan="2" Style="height: 29px; text-align: left">Current visit</asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbBackPain" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell Style="width: 100px">
                                                                        <label id="lblBackPain" runat="Server">
                                                                            Back Pain</label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell Style="width: 200px">
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbBackPain" CriteriaString="BACK"
                                                                            BoldData="MUS" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbMusculoskeletal" Style="vertical-align: top;
                                                                    text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblMusculoskeletalDisease" runat="server">
                                                                            Musculoskeletal Disease</label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbMusculoskeletal" CriteriaString="MUSC"
                                                                            BoldData="MUS" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbFibro" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblFibromyalgia" runat="server">
                                                                            Fibromyalgia</label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbFibro" CriteriaString="FIBR"
                                                                            BoldData="MUS" SCode="PRE" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                            </asp:Table>
                                                        </td>
                                                        <td style="width: 300px; vertical-align: top">
                                                            <div runat="server" id="divMUSHistoryData" />
                                                        </td>
                                                        <td style="width: 300px; vertical-align: top">
                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="inner_table"
                                                                runat="server" id="tblMUSNote">
                                                                <tr>
                                                                    <td style="width: 300px; text-align: left; height: 29px; vertical-align: top">
                                                                        Notes:</td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="text-align: left; vertical-align: top">
                                                                        <wucTextBox:TextBox runat="server" ID="txtMUSNote" textMode="MultiLine" width="100%"
                                                                            maxLength="512" rows="5" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div class="boxBtm">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="sectionBox1" runat="server" id="divReproductive" style="width: 900px;
                                            display: none">
                                            <div class="boxTop" style="width: 900px">
                                                <table width="880" border="0" cellpadding="4" cellspacing="4">
                                                    <tr>
                                                        <td>
                                                            <h3 id="hReproductive">
                                                                REPRODUCTIVE</h3>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 300px; vertical-align: top">
                                                            <asp:Table ID="tblReproductive" Width="100%" BorderWidth="0" CellPadding="0" CellSpacing="0"
                                                                CssClass="inner_table" runat="server" Style="display: block; text-align: left">
                                                                <asp:TableRow>
                                                                    <asp:TableCell ColumnSpan="2" Style="height: 29px; text-align: left">Current visit</asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbPolycystic" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell Style="width: 100px">
                                                                        <label id="lblPolycysticOverianSyndrome" runat="server">
                                                                            Polycystic Ovary Syndrome</label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell Style="width: 200px">
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbPolycystic" CriteriaString="POLY"
                                                                            BoldData="FEM" SCode="PRE" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbMenstrual" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblMenstrualIrregularities" runat="server">
                                                                            Menstrual Irregularities (not PCOS)</label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbMenstrual" CriteriaString="MENS"
                                                                            BoldData="FEM" SCode="PRE" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                            </asp:Table>
                                                        </td>
                                                        <td style="width: 300px; vertical-align: top">
                                                            <label runat="server" id="lblREPRDHistoryData" />
                                                            <div runat="server" id="divREPRDHistoryData" />
                                                        </td>
                                                        <td style="width: 300px; vertical-align: top">
                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="inner_table"
                                                                runat="server" id="tblFEMNote">
                                                                <tr>
                                                                    <td style="width: 300px; text-align: left; height: 29px; vertical-align: top">
                                                                        Notes:</td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="text-align: left; vertical-align: top">
                                                                        <wucTextBox:TextBox runat="server" ID="txtFEMNote" textMode="MultiLine" width="100%"
                                                                            maxLength="512" rows="5" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div class="boxBtm">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="sectionBox1" runat="server" id="divPsychosocial" style="width: 900px;
                                            display: none">
                                            <div class="boxTop" style="width: 900px">
                                                <table width="880" border="0" cellpadding="4" cellspacing="4">
                                                    <tr>
                                                        <td>
                                                            <h3 id="hPsychosocial">
                                                                PSYCHOSOCIAL</h3>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 300px; vertical-align: top">
                                                            <asp:Table ID="tblPsychosocial" Width="100%" BorderWidth="0" CellPadding="0" CellSpacing="0"
                                                                CssClass="inner_table" runat="server" Style="display: block; text-align: left">
                                                                <asp:TableRow>
                                                                    <asp:TableCell ColumnSpan="2" Style="height: 29px; text-align: left">Current visit</asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbPsychosocial" Style="vertical-align: top;
                                                                    text-align: left">
                                                                    <asp:TableCell Style="width: 100px">
                                                                        <label id="lblPsychosocialImpairment" runat="server">
                                                                            Psychosocial Impairment</label></asp:TableCell>
                                                                    <asp:TableCell Style="width: 200px">
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbPsychosocial" CriteriaString="PSYC"
                                                                            BoldData="PSY" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbDepression" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblDepression" runat="server">
                                                                            Depression</label></asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbDepression" CriteriaString="DEPR"
                                                                            BoldData="PSY" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbConfirmed" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblConfirmedMentalHealthDiagnosis" runat="server">
                                                                            Confirmed Mental Health Diagnosis</label></asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbConfirmed" CriteriaString="MENT"
                                                                            BoldData="PSY" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbAlcohol" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblAlcoholUse" runat="server">
                                                                            Alcohol Use</label></asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbAlcohol" CriteriaString="ALCO"
                                                                            BoldData="PSY" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbTobacco" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblTobaccoUse" runat="server">
                                                                            Tobacco Use</label></asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbTobacco" CriteriaString="TOBA"
                                                                            BoldData="PSY" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbAbuse" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblSubstanceAbuse" runat="Server">
                                                                            Substance Abuse</label></asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbAbuse" CriteriaString="SUBS"
                                                                            BoldData="PSY" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                            </asp:Table>
                                                        </td>
                                                        <td style="width: 300px; vertical-align: top">
                                                            <div runat="Server" id="divPSYCHHistorydata" />
                                                        </td>
                                                        <td style="width: 300px; vertical-align: top">
                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="inner_table"
                                                                runat="server" id="tblPSYNote">
                                                                <tr>
                                                                    <td style="width: 300px; text-align: left; height: 29px; vertical-align: top">
                                                                        Notes:</td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="text-align: left; vertical-align: top">
                                                                        <wucTextBox:TextBox runat="server" ID="txtPSYNote" textMode="MultiLine" width="100%"
                                                                            maxLength="512" rows="5" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div class="boxBtm">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="sectionBox1" runat="Server" id="divGeneral" style="width: 900px; display: none">
                                            <div class="boxTop" style="width: 900px">
                                                <table width="880" border="0" cellpadding="4" cellspacing="4">
                                                    <tr>
                                                        <td>
                                                            <h3 id="hGeneral">
                                                                GENERAL</h3>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 300px; vertical-align: top">
                                                            <asp:Table ID="tblGeneral" Width="100%" BorderWidth="0" CellPadding="0" CellSpacing="0"
                                                                CssClass="inner_table" runat="server" Style="display: block; text-align: left">
                                                                <asp:TableRow>
                                                                    <asp:TableCell ColumnSpan="2" Style="height: 29px; text-align: left">Current visit</asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbStressUrinary" Style="vertical-align: top;
                                                                    text-align: left">
                                                                    <asp:TableCell Style="width: 100px">
                                                                        <label id="lblStressUrinaryIncontinence" runat="server">
                                                                            Stress Urinary Incontinence</label></asp:TableCell>
                                                                    <asp:TableCell Style="width: 200px">
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbStressUrinary" CriteriaString="STRE"
                                                                            BoldData="GEN" SCode="PRE" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbCerebri" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblCerebi" runat="server">
                                                                            Pseudotumor Cerebri</label></asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbCerebri" CriteriaString="PSEU"
                                                                            BoldData="GEN" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbHernia" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblAbdominalHernia" runat="Server">
                                                                            Abdominal Hernia</label></asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbHernia" CriteriaString="ABDO"
                                                                            BoldData="GEN" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbFunctional" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblFunctionalStatus" runat="server">
                                                                            Functional Status</label></asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbFunctional" CriteriaString="FUNC"
                                                                            BoldData="GEN" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbSkin" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblAbdominal" runat="server">
                                                                            Abdominal Skin/Pannus</label></asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbSkin" CriteriaString="ABDP" BoldData="GEN" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbRenalInsuf" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblRenalInsuf" runat="server">
                                                                            Renal Insufficiency / Creatinine > 2</label></asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbRenalInsuff" CriteriaString="RENI"
                                                                            BoldData="GEN" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbRenalFail" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblRenalFail" runat="server">
                                                                            Renal Failure Requiring Dialysis</label></asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbRenalFail" CriteriaString="RENF"
                                                                            BoldData="GEN" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbSteroid" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblSteroid" runat="server">
                                                                            Chronic Steroid Use and or Immunosuppresant use</label></asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbSteroid" CriteriaString="CHRO"
                                                                            BoldData="GEN" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbTherapeutic" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblTherapeutic" runat="server">
                                                                            Therapeutic Anticoagulation</label></asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbTherapeutic" CriteriaString="THER"
                                                                            BoldData="GEN" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbPrevPCISurgery" Style="vertical-align: top;
                                                                    text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblPrevPCISurgery" runat="server">
                                                                            Previous PCI and Previous Cardiac Surgery</label></asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbPrevPCISurgery" CriteriaString="PREV"
                                                                            BoldData="GEN" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                            </asp:Table>
                                                        </td>
                                                        <td style="width: 300px; vertical-align: top">
                                                            <div runat="server" id="divGENHistoryData" />
                                                        </td>
                                                        <td style="width: 300px; vertical-align: top">
                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="inner_table"
                                                                runat="server" id="tblGenNote">
                                                                <tr>
                                                                    <td style="width: 300px; text-align: left; height: 29px; vertical-align: top">
                                                                        Notes:</td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="text-align: left; vertical-align: top">
                                                                        <wucTextBox:TextBox runat="server" ID="txtGENNote" textMode="MultiLine" width="100%"
                                                                            maxLength="512" rows="5" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div class="boxBtm">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="comorbidityACS" runat="Server">
                                        <div class="sectionBox1" runat="Server" id="divPulmonaryACS" style="width: 900px;
                                            display: none">
                                            <div class="boxTop" style="width: 900px">
                                                <table width="880" border="0" cellpadding="4" cellspacing="4">
                                                    <tr>
                                                        <td>
                                                            <h3 id="H3_1">
                                                                PULMONARY</h3>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 300px; vertical-align: top">
                                                            <asp:Table ID="tblPulmonaryACS" Width="100%" BorderWidth="0" CellPadding="0" CellSpacing="0"
                                                                CssClass="inner_table" runat="server" Style="display: block; text-align: left">
                                                                <asp:TableRow>
                                                                    <asp:TableCell ColumnSpan="2" Style="height: 29px; text-align: left">Current visit</asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbSmokerACS" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell Style="width: 100px">
                                                                        <label id="lblSmokerACS" runat="server">
                                                                            Current Smoker within 1 year</label></asp:TableCell>
                                                                    <asp:TableCell Style="width: 200px">
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbSmokerACS" CriteriaString="SMOKEACS"
                                                                            BoldData="PULACS " />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbOxygenACS" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblOxygenACS" runat="server">
                                                                            Oxygen Dependent</label></asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbOxygenACS" CriteriaString="OXYACS"
                                                                            BoldData="PULACS" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbEmbolismACS" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblEmbolismACS" runat="Server">
                                                                            History of Pulmonary Embolism</label></asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbEmbolismACS" CriteriaString="EMBOACS"
                                                                            BoldData="PULACS" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbCopdACS" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblCopdACS" runat="server">
                                                                            History of Severe COPD</label></asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbCopdACS" CriteriaString="COPDACS"
                                                                            BoldData="PULACS" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbCpapACS" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblCpapACS" runat="server">
                                                                            Obstructive Sleep Apnea req. CPAP or BiPAP</label></asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbCpapACS" CriteriaString="CPAPACS"
                                                                            BoldData="PULACS" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbShoACS" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblShoACS" runat="server">
                                                                            Shortness of Breath with Exertion</label></asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbShoACS" CriteriaString="SHOACS"
                                                                            BoldData="PULACS" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                            </asp:Table>
                                                        </td>
                                                        <td style="width: 300px; vertical-align: top">
                                                            <div runat="server" id="divPULACSHistoryData" />
                                                        </td>
                                                        <td style="width: 300px; vertical-align: top">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div class="boxBtm">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="sectionBox1" runat="Server" id="divGastroACS" style="width: 900px; display: none">
                                            <div class="boxTop" style="width: 900px">
                                                <table width="880" border="0" cellpadding="4" cellspacing="4">
                                                    <tr>
                                                        <td>
                                                            <h3 id="H3_2">
                                                                GASTROINTESTINAL</h3>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 300px; vertical-align: top">
                                                            <asp:Table ID="tblGastroACS" Width="100%" BorderWidth="0" CellPadding="0" CellSpacing="0"
                                                                CssClass="inner_table" runat="server" Style="display: block; text-align: left">
                                                                <asp:TableRow>
                                                                    <asp:TableCell ColumnSpan="2" Style="height: 29px; text-align: left">Current visit</asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbGerdACS" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell Style="width: 100px">
                                                                        <label id="lblGerdACS" runat="server">
                                                                            GERD req. medications</label></asp:TableCell>
                                                                    <asp:TableCell Style="width: 200px">
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbGerdACS" CriteriaString="GERDACS"
                                                                            BoldData="GASACS " />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbGallstoneACS" Style="vertical-align: top;
                                                                    text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblGallstoneACS" runat="server">
                                                                            Gallstone Disease</label></asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbGallstoneACS" CriteriaString="GALACS"
                                                                            BoldData="GASACS" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                            </asp:Table>
                                                        </td>
                                                        <td style="width: 300px; vertical-align: top">
                                                            <div runat="server" id="divGASACSHistoryData" />
                                                        </td>
                                                        <td style="width: 300px; vertical-align: top">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div class="boxBtm">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="sectionBox1" runat="Server" id="divMusculoACS" style="width: 900px; display: none">
                                            <div class="boxTop" style="width: 900px">
                                                <table width="880" border="0" cellpadding="4" cellspacing="4">
                                                    <tr>
                                                        <td>
                                                            <h3 id="H3_3">
                                                                MUSCULOSKELETAL</h3>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 300px; vertical-align: top">
                                                            <asp:Table ID="tblMusculoACS" Width="100%" BorderWidth="0" CellPadding="0" CellSpacing="0"
                                                                CssClass="inner_table" runat="server" Style="display: block; text-align: left">
                                                                <asp:TableRow>
                                                                    <asp:TableCell ColumnSpan="2" Style="height: 29px; text-align: left">Current visit</asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbMusculoDiseaseACS" Style="vertical-align: top;
                                                                    text-align: left">
                                                                    <asp:TableCell Style="width: 100px">
                                                                        <label id="lblMusculoDiseaseACS" runat="server">
                                                                            Musculoskeletal Disease</label></asp:TableCell>
                                                                    <asp:TableCell Style="width: 200px">
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbMusculoDiseaseACS" CriteriaString="MUSCDACS"
                                                                            BoldData="MUSCACS " />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbActivityLimitedACS" Style="vertical-align: top;
                                                                    text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblActivityLimitedACS" runat="server">
                                                                            Activity limited by pain</label></asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbActivityLimitedACS" CriteriaString="PAINACS"
                                                                            BoldData="MUSCACS" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbDailyMedACS" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblDailyMedACS" runat="server">
                                                                            Requires daily medication</label></asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbDailyMedACS" CriteriaString="MEDSACS"
                                                                            BoldData="MUSCACS" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbSurgicalACS" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblSurgicalACS" runat="server">
                                                                            Surgical intervention planned or performed</label></asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbSurgicalACS" CriteriaString="SURGACS"
                                                                            BoldData="MUSCACS" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbMobilityACS" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblMobilityACS" runat="server">
                                                                            Uses mobility device</label></asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbMobilityACS" CriteriaString="MOBACS"
                                                                            BoldData="MUSCACS" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                            </asp:Table>
                                                        </td>
                                                        <td style="width: 300px; vertical-align: top">
                                                            <div runat="server" id="divMUSCACSHistoryData" />
                                                        </td>
                                                        <td style="width: 300px; vertical-align: top">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div class="boxBtm">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="sectionBox1" runat="Server" id="divRenalACS" style="width: 900px; display: none">
                                            <div class="boxTop" style="width: 900px">
                                                <table width="880" border="0" cellpadding="4" cellspacing="4">
                                                    <tr>
                                                        <td>
                                                            <h3 id="H3_4">
                                                                RENAL</h3>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 300px; vertical-align: top">
                                                            <asp:Table ID="tblRenalACS" Width="100%" BorderWidth="0" CellPadding="0" CellSpacing="0"
                                                                CssClass="inner_table" runat="server" Style="display: block; text-align: left">
                                                                <asp:TableRow>
                                                                    <asp:TableCell ColumnSpan="2" Style="height: 29px; text-align: left">Current visit</asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbRenalInsuffACS" Style="vertical-align: top;
                                                                    text-align: left">
                                                                    <asp:TableCell Style="width: 100px">
                                                                        <label id="lblRenalInsuffACS" runat="server">
                                                                            Renal Insufficiency (Creat >2)</label></asp:TableCell>
                                                                    <asp:TableCell Style="width: 200px">
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbRenalInsuffACS" CriteriaString="RENI"
                                                                            BoldData="GEN " />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbRenalFailACS" Style="vertical-align: top;
                                                                    text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblRenalFailACS" runat="server">
                                                                            Renal Failure req. dialysis</label></asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbRenalFailACS" CriteriaString="RENF"
                                                                            BoldData="GEN" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbUrinaryACS" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblUrinaryACS" runat="Server">
                                                                            Urinary Stress Incontinence</label></asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbUrinaryACS" CriteriaString="URIACS"
                                                                            BoldData="RENACS" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                            </asp:Table>
                                                        </td>
                                                        <td style="width: 300px; vertical-align: top">
                                                            <div runat="server" id="divRENACSHistoryData" />
                                                        </td>
                                                        <td style="width: 300px; vertical-align: top">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div class="boxBtm">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="sectionBox1" runat="Server" id="divCardiacACS" style="width: 900px; display: none">
                                            <div class="boxTop" style="width: 900px">
                                                <table width="880" border="0" cellpadding="4" cellspacing="4">
                                                    <tr>
                                                        <td>
                                                            <h3 id="H3_5">
                                                                CARDIAC</h3>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 300px; vertical-align: top">
                                                            <asp:Table ID="tblCardiacACS" Width="100%" BorderWidth="0" CellPadding="0" CellSpacing="0"
                                                                CssClass="inner_table" runat="server" Style="display: block; text-align: left">
                                                                <asp:TableRow>
                                                                    <asp:TableCell ColumnSpan="2" Style="height: 29px; text-align: left">Current visit</asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbMyocardinalACS" Style="vertical-align: top;
                                                                    text-align: left">
                                                                    <asp:TableCell Style="width: 100px">
                                                                        <label id="lblMyocardinalACS" runat="server">
                                                                            History of Myocardinal Infarction</label></asp:TableCell>
                                                                    <asp:TableCell Style="width: 200px">
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbMyocardinalACS" CriteriaString="MYOACS"
                                                                            BoldData="CARDACS " />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbPrevPCIACS" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblPrevPCIACS" runat="server">
                                                                            Previous PCI</label></asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbPrevPCIACS" CriteriaString="PCIACS"
                                                                            BoldData="CARDACS" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbPrevCardiacACS" Style="vertical-align: top;
                                                                    text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblPrevCardiacACS" runat="Server">
                                                                            Previous Cardiac Surgery</label></asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbPrevCardiacACS" CriteriaString="CSURGACS"
                                                                            BoldData="CARDACS" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbHyperlipidemiaACS" Style="vertical-align: top;
                                                                    text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblHyperlipidemiaACS" runat="Server">
                                                                            Hyperlipidemia req. medications</label></asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbHyperlipidemiaACS" CriteriaString="LIPIDACS"
                                                                            BoldData="CARDACS" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbHypertensionACS" Style="vertical-align: top;
                                                                    text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblHypertensionACS" runat="Server">
                                                                            Hypertension req. medications</label></asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbHypertensionACS" CriteriaString="HYPERACS"
                                                                            BoldData="CARDACS" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                            </asp:Table>
                                                        </td>
                                                        <td style="width: 300px; vertical-align: top">
                                                            <div runat="server" id="divCARDACSHistoryData" />
                                                        </td>
                                                        <td style="width: 300px; vertical-align: top">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div class="boxBtm">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="sectionBox1" runat="Server" id="divVascularACS" style="width: 900px;
                                            display: none">
                                            <div class="boxTop" style="width: 900px">
                                                <table width="880" border="0" cellpadding="4" cellspacing="4">
                                                    <tr>
                                                        <td>
                                                            <h3 id="H3_6">
                                                                VASCULAR</h3>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 300px; vertical-align: top">
                                                            <asp:Table ID="tblVascularACS" Width="100%" BorderWidth="0" CellPadding="0" CellSpacing="0"
                                                                CssClass="inner_table" runat="server" Style="display: block; text-align: left">
                                                                <asp:TableRow>
                                                                    <asp:TableCell ColumnSpan="2" Style="height: 29px; text-align: left">Current visit</asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbDVTACS" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell Style="width: 100px">
                                                                        <label id="lblDVTACS" runat="server">
                                                                            History of DVT requiring therapy</label></asp:TableCell>
                                                                    <asp:TableCell Style="width: 200px">
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbDVTACS" CriteriaString="DVTACS"
                                                                            BoldData="VASCACS " />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbVenousACS" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblVenousACS" runat="server">
                                                                            Venous Stasis</label></asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbVenousACS" CriteriaString="VENOUSACS"
                                                                            BoldData="VASCACS" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                            </asp:Table>
                                                        </td>
                                                        <td style="width: 300px; vertical-align: top">
                                                            <div runat="server" id="divVASCACSHistoryData" />
                                                        </td>
                                                        <td style="width: 300px; vertical-align: top">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div class="boxBtm">
                                                </div>
                                            </div>
                                        </div>                                        
                                        <div class="sectionBox1" runat="Server" id="divOtherACS" style="width: 900px; display: none">
                                            <div class="boxTop" style="width: 900px">
                                                <table width="880" border="0" cellpadding="4" cellspacing="4">
                                                    <tr>
                                                        <td>
                                                            <h3 id="H3_7">
                                                                OTHER</h3>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 300px; vertical-align: top">
                                                            <asp:Table ID="tblOtherACS" Width="100%" BorderWidth="0" CellPadding="0" CellSpacing="0"
                                                                CssClass="inner_table" runat="server" Style="display: block; text-align: left">
                                                                <asp:TableRow>
                                                                    <asp:TableCell ColumnSpan="2" Style="height: 29px; text-align: left">Current visit</asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbHealthStatusACS" Style="vertical-align: top;
                                                                    text-align: left">
                                                                    <asp:TableCell Style="width: 100px">
                                                                        <label id="lblHealthStatusACS" runat="server">
                                                                            Functional Health Status Prior to Surgery</label></asp:TableCell>
                                                                    <asp:TableCell Style="width: 200px">
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbHealthStatusACS" CriteriaString="HEALTHACS"
                                                                            BoldData="OTHERACS " />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbDiabetesACS" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblDiabetesACS" runat="server">
                                                                            Diabetes Mellitus</label></asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbDiabetesACS" CriteriaString="DIABACS"
                                                                            BoldData="OTHERACS" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbSteroidACS" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblSteroidACS" runat="Server">
                                                                            Chronic Steroids/ Immunosuppression</label></asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbSteroidACS" CriteriaString="CHRO"
                                                                            BoldData="GEN" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbTherapeuticACS" Style="vertical-align: top;
                                                                    text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblTherapeuticACS" runat="Server">
                                                                            Therapeutic anticogulation</label></asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbTherapeuticACS" CriteriaString="THER"
                                                                            BoldData="GEN" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbObesityACS" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell>
                                                                        <label id="lblObesityACS" runat="Server">
                                                                            Previous obesity/ foregut surgery</label></asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbObesityACS" CriteriaString="OBESEACS"
                                                                            BoldData="OTHERACS" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                            </asp:Table>
                                                        </td>
                                                        <td style="width: 300px; vertical-align: top">
                                                            <div runat="server" id="divOTHERACSHistoryData" />
                                                        </td>
                                                        <td style="width: 300px; vertical-align: top">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div class="boxBtm">
                                                </div>
                                            </div>
                                        </div>
                                        
                                        <div class="sectionBox1" runat="Server" id="divGenACS" style="width: 900px; display: none">
                                            <div class="boxTop" style="width: 900px">
                                                <table width="880" border="0" cellpadding="4" cellspacing="4">
                                                    <tr>
                                                        <td>
                                                            <h3 id="H3_8">
                                                                General</h3>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 300px; vertical-align: top">
                                                            <asp:Table ID="tblGeneralACS" Width="100%" BorderWidth="0" CellPadding="0" CellSpacing="0"
                                                                CssClass="inner_table" runat="server" Style="display: block; text-align: left">
                                                                <asp:TableRow>
                                                                    <asp:TableCell ColumnSpan="2" Style="height: 29px; text-align: left">Current visit</asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow runat="server" ID="tr_cmbFatACS" Style="vertical-align: top; text-align: left">
                                                                    <asp:TableCell Style="width: 100px">
                                                                        <label id="lblFatACS" runat="server">
                                                                            Fatigue</label></asp:TableCell>
                                                                    <asp:TableCell Style="width: 200px">
                                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbFatACS" CriteriaString="FATACS"
                                                                            BoldData="GENACS " />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                            </asp:Table>
                                                        </td>
                                                        
                                                        <td style="width: 300px; vertical-align: top">
                                                            <div runat="server" id="divGENACSHistoryData" />
                                                        </td>
                                                        <td style="width: 300px; vertical-align: top">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div class="boxBtm">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div id="div_Vitamins" class="comorbiditiesBlood" style="display: none">
                            <asp:UpdatePanel runat="Server" ID="up_MutipleVitamin" UpdateMode="Always">
                                <ContentTemplate>
                                    <div class="leftColumn">
                                        <div class="sectionBox1">
                                            <div class="boxTop">
                                                <h3 id="hMedicationVitaminsMinerals">
                                                    Medication / Vitamins & Minerals</h3>
                                                <table style="width: 290px" border="0">
                                                    <tr>
                                                        <td>
                                                            <input type="checkbox" runat="server" id="chkMutipleVitamin" />
                                                        </td>
                                                        <td>
                                                            <label id="lblMutipleVitamin" onclick="javascript:MedicationVitamin('chkMutipleVitamin');">
                                                                Multiple Vitamin</label></td>
                                                        <td>
                                                            <input type="checkbox" runat="server" id="chkCalcium" />
                                                        </td>
                                                        <td>
                                                            <label id="lblCalcium" onclick="javascript:MedicationVitamin('chkCalcium');">
                                                                Calcium</label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <input type="checkbox" runat="server" id="chkVitaminB12" />
                                                        </td>
                                                        <td>
                                                            <label id="lblVitaminB12" onclick="javascript:MedicationVitamin('chkVitaminB12');">
                                                                Vitamin B-12</label></td>
                                                        <td>
                                                            <input type="checkbox" runat="server" id="chkIron" />
                                                        </td>
                                                        <td>
                                                            <label id="lblIron" onclick="javascript:MedicationVitamin('chkIron');">
                                                                Iron</label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <input type="checkbox" runat="server" id="chkVitaminD" />
                                                        </td>
                                                        <td>
                                                            <label id="lblVitaminD" onclick="javascript:MedicationVitamin('chkVitaminD');">
                                                                Vitamin D</label></td>
                                                        <td>
                                                            <input type="checkbox" runat="server" id="chkVitaminADE" />
                                                        </td>
                                                        <td>
                                                            <label id="lblVitaminADE" onclick="javascript:MedicationVitamin('chkVitaminADE');">
                                                                Vitamin A, D, E Combo</label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <input type="checkbox" runat="server" id="chkCalciumVitaminD" />
                                                        </td>
                                                        <td>
                                                            <label id="lblCaciumVitaminD" onclick="javascript:MedicationVitamin('chkCalciumVitaminD');">
                                                                Calcium with Vitamin D</label></td>
                                                    </tr>
                                                </table>
                                                <div class="boxBtm">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="sectionBox3">
                                            <div class="boxTop">
                                                <h3 id="hMedicationNotes">
                                                    Notes</h3>
                                                <table style="width: 260px" border="0">
                                                    <tr>
                                                        <td>
                                                            <wucTextBox:TextBox runat="server" ID="txtMedicationNotes" textMode="MultiLine" rows="5"
                                                                width="260px" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div class="boxBtm">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="sectionBox1">
                                            <div class="boxTop">
                                                <h3>
                                                    Medications</h3>
                                                <table style="width: 500px">
                                                    <tr style="vertical-align: top">
                                                        <td style="width: 48%">
                                                            <label id="lblMedicationSearch">
                                                                Search by
                                                            </label>
                                                            <wucTextBox:TextBox runat="server" ID="txtMedicationSearch" />
                                                        </td>
                                                        <td style="width: 4%" />
                                                        <td style="width: 48%" />
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <select id="listMedication" multiple="true" style="width: 100%" size="5" runat="server"
                                                                ondblclick="javascript:BoldLists_dblclick('listMedication', 'listMedication_Selected', 'Add');" />
                                                            <wucSystemCode:SystemCodeList runat="server" ID="cmbMedication" CriteriaString="MED"
                                                                BoldData="MED" Display="false" FirstRow="false" />
                                                        </td>
                                                        <td style="vertical-align: middle; text-align: center">
                                                            <a href="#this" onclick="javascript:BoldLButtonLinks_click('listMedication', 'listMedication_Selected', 'Add', false);">
                                                                ></a><br />
                                                            <a href="#this" onclick="javascript:BoldLButtonLinks_click('listMedication', 'listMedication_Selected', 'Add', true);">
                                                                >></a><br />
                                                            <a href="#this" onclick="javascript:BoldLButtonLinks_click('listMedication_Selected', 'listMedication', 'Remove', false);">
                                                                <</a><br />
                                                            <a href="#this" onclick="javascript:BoldLButtonLinks_click('listMedication_Selected', 'listMedication', 'Remove', true);">
                                                                <<</a><br />
                                                        </td>
                                                        <td>
                                                            <select id="listMedication_Selected" multiple="true" style="width: 100%" size="5"
                                                                runat="server" ondblclick="javascript:BoldLists_dblclick('listMedication_Selected', 'listMedication', 'Remove');" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div class="boxBtm">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnAddVisit" EventName="serverclick" />
                                    <asp:AsyncPostBackTrigger ControlID="btnAddFile" EventName="serverclick" />
                                    <asp:AsyncPostBackTrigger ControlID="btnLoadVisitData" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="expandList">
                        <div class="boxTop">
                            <div class="expandListTitle">
                                <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                    <tbody>
                                        <tr style="vertical-align: middle;">
                                            <td style="width: 50%;">
                                                <img id="imgPatientNotes" runat="server" src="~/img/button_Minus.gif" alt="." onmouseover="javascript:this.style.cursor='pointer';"
                                                    onmouseout="javascript:this.style.cursor='';" />&nbsp; <a id="linkPatientNotes" runat="Server"
                                                        href="#this" onclick="javascript:ShowPatientNotesDiv();">General Notes</a>
                                            </td>
                                            <!-- <td style="width: 50%;text-align:right;display:block" >
						                <div style="float:right">
						                    <input value="End date" id="btnEndDate" onclick="javascript:AddDateToPatientNotes('End');" type="button"  style="display:block;width:80px"/>
						                </div>
						                <div style="float:right">
						                    <input value="Start date" id="btnStartDate" onclick="javascript:AddDateToPatientNotes('Start');" type="button" style="display:block;width:80px"/>
						                </div>
					                </td> -->
                                            <!--<td style="display:none">					                
						                <div style="float:right">
						                    <asp:Button Text="Save" ID="btnSave" style="display:block;width:60px" runat="Server" OnClick = "linkBtnSaveNotes_OnClick"/>
						                </div>
					                </td>	-->
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div class="addVisitDetails" id="divPatientNotes" style="display: block">
                                <table>
                                    <tbody>
                                        <tr style="vertical-align: top;">
                                            <td>
                                                <asp:Label Text="Notes" ID="lblPatientNotes" runat="server" />
                                                <wucTextBox:TextBox runat="server" ID="txtPatientNotes" textMode="MultiLine" rows="6"
                                                    width="100%" />
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
                                <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                    <tbody>
                                        <tr style="vertical-align: middle;">
                                            <td style="width: 50%;">
                                                <img id="imgOperationList" runat="server" src="~/img/button_Plus.gif" alt="." onmouseover="javascript:this.style.cursor='pointer';"
                                                    onmouseout="javascript:this.style.cursor='';" />&nbsp; <a id="linkOperationList" runat="Server"
                                                        href="#this" onclick="javascript:ShowOperationListDiv();">Operation List</a>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div id="divOperationList" style="display:none">
                                <div class="expandListTitle" id="divOperationListTitle" runat="server" style="display: block">
                                    <table border="0" cellpadding="3" cellspacing="0" width="100%">
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
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                                <div class="addVisitDetails" id="divOperationListDetail" runat="server" style="display: block">
                                </div>
                            </div>
                            <div class="boxBtm">
                            </div>
                        </div>
                    </div>
                </div>
                </div>
                <div id="div_vPatientReport" style="display: none" class="patientDetails">
                    <table style="width: 100%">
                        <tr>
                            <td colspan="2">
                                <div id="divErrorMessageReport" style="display: none;" runat="server">
                                    <span>
                                        <p id="pErrorMessageReport" runat="server">
                                        </p>
                                    </span>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="column01" style="width: 30%; vertical-align: top">
                                <table id="tblReportTitles" style="width: 95%">
                                    <tr onclick="javascript:ShowDivReportItems('FollowUpDetails');document.getElementById('ReportCode').value = 'FUA';">
                                        <td style="width: 5%">
                                            <input type="checkbox" id="chkFollowUpDetails" /></td>
                                        <td>
                                            <img src="~/img/ico_follow_up_details.gif" class="floatLeft" height="16" width="16"
                                                runat="server" alt="." onmouseover="javascript:this.style.cursor = 'pointer';"
                                                onmouseout="javascript:this.style.cursor = '';" />&nbsp;
                                            <label id="lblFollowUpDetails" onmouseover="javascript:this.style.cursor = 'pointer';"
                                                onmouseout="javascript:this.style.cursor = '';">
                                                Follow Up Details</label>
                                        </td>
                                    </tr>
                                    <tr onclick="javascript:ShowDivReportItems('LetterToDoctor');document.getElementById('ReportCode').value = 'RDL';">
                                        <td>
                                            <input type="checkbox" id="chkLetterToDoctor" /></td>
                                        <td>
                                            <img src="~/img/ico_letter_to_doctor.gif" class="floatLeft" height="16" width="16"
                                                runat="server" alt="." onmouseover="javascript:this.style.cursor = 'pointer';"
                                                onmouseout="javascript:this.style.cursor = '';" />&nbsp;
                                            <label id="lblLetterToDoctor" onmouseover="javascript:this.style.cursor = 'pointer';"
                                                onmouseout="javascript:this.style.cursor = '';">
                                                Letter To Doctor</label>
                                        </td>
                                    </tr>
                                    <tr onclick="javascript:ShowDivReportItems('Graphs');">
                                        <td>
                                            <input type="checkbox" id="chkGraphs" /></td>
                                        <td>
                                            <img id="Img1" src="~/img/ico_graphs.gif" class="floatLeft" height="16" width="16"
                                                runat="server" alt="." onmouseover="javascript:this.style.cursor = 'pointer';"
                                                onmouseout="javascript:this.style.cursor = '';" />&nbsp;
                                            <label id="lblGraphs" onmouseover="javascript:this.style.cursor = 'pointer';" onmouseout="javascript:this.style.cursor = '';">
                                                Graphs</label>
                                        </td>
                                    </tr>
                                    <tr onclick="javascript:ShowDivReportItems('Super');" id="rowchkSuperBill" runat="server">
                                        <td>
                                            <input type="checkbox" id="chkSuper" /></td>
                                        <td>
                                            <img id="Img2" src="~/img/tab_progress_notes.gif" class="floatLeft" height="16" width="16"
                                                runat="server" alt="." onmouseover="javascript:this.style.cursor = 'pointer';"
                                                onmouseout="javascript:this.style.cursor = '';" />&nbsp;
                                            <label id="Label1" onmouseover="javascript:this.style.cursor = 'pointer';" onmouseout="javascript:this.style.cursor = '';">
                                                Super Bill</label>
                                        </td>
                                    </tr>
                                    <tr style="visibility: hidden; display: none">
                                        <td>
                                            <input type="checkbox" id="chkComorbidities" onclick="javascript:ShowDivReportItems('Comorbidities');" />
                                        </td>
                                        <td>
                                            <label id="lblComorbidities" onclick="javascript:ShowDivReportItems('Comorbidities');"
                                                onmouseover="javascript:this.style.cursor = 'pointer';" onmouseout="javascript:this.style.cursor = '';">
                                                Comorbidities</label>
                                        </td>
                                    </tr>
                                    <tr style="visibility: hidden; display: none">
                                        <td>
                                            <input type="checkbox" id="chkOtherReports" onclick="javascript:ShowDivReportItems('OtherReports');" />
                                        </td>
                                        <td>
                                            <label id="lblOtherReports" onclick="javascript:ShowDivReportItems('OtherReports');"
                                                onmouseover="javascript:this.style.cursor = 'pointer';" onmouseout="javascript:this.style.cursor = '';">
                                                Other Reports</label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="column02" style="vertical-align: top">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="vertical-align: top">
                                            <div id="divFollowUpDetails" style="display: block">
                                                <table style="width: 100%" border="0">
                                                    <tr>
                                                        <td width="100%">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td style="width: 4%">
                                                                        <input type="checkbox" id="chkIncludeLast10Visit" />
                                                                    </td>
                                                                    <td>
                                                                        <label id="lblIncludeLast10Visit" onclick="document.getElementById('chkIncludeLast10Visit').checked = !document.getElementById('chkIncludeLast10Visit').checked;"
                                                                            onmouseover="javascript:this.style.cursor = 'pointer';" onmouseout="javascript:this.style.cursor = '';">
                                                                            Last 10 Visits Only</label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 4%">
                                                                        <input type="checkbox" id="chkIncludeProgressNotes" />
                                                                    </td>
                                                                    <td>
                                                                        <label id="lblIncludeProgressNotes" onclick="document.getElementById('chkIncludeProgressNotes').checked = !document.getElementById('chkIncludeProgressNotes').checked;"
                                                                            onmouseover="javascript:this.style.cursor = 'pointer';" onmouseout="javascript:this.style.cursor = '';">
                                                                            Include Progress Notes</label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 4%">
                                                                        <input type="checkbox" id="chkIncludePatientNote" />
                                                                    </td>
                                                                    <td>
                                                                        <label id="lblIncludePatientNote" onclick="document.getElementById('chkIncludePatientNote').checked = !document.getElementById('chkIncludePatientNote').checked;"
                                                                            onmouseover="javascript:this.style.cursor = 'pointer';" onmouseout="javascript:this.style.cursor = '';">
                                                                            Include Patient Note</label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 4%">
                                                                        <input type="checkbox" id="chkIncludeComments" />
                                                                    </td>
                                                                    <td>
                                                                        <label id="lblIncludeComments" onclick="document.getElementById('chkIncludeComments').checked = !document.getElementById('chkIncludeComments').checked;"
                                                                            onmouseover="javascript:this.style.cursor = 'pointer';" onmouseout="javascript:this.style.cursor = '';">
                                                                            Include Comments</label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 4%">
                                                                        <input type="checkbox" id="chkIncludePhoto" />
                                                                    </td>
                                                                    <td>
                                                                        <label id="lblIncludePhoto" onclick="document.getElementById('chkIncludePhoto').checked = !document.getElementById('chkIncludePhoto').checked;"
                                                                            onmouseover="javascript:this.style.cursor = 'pointer';" onmouseout="javascript:this.style.cursor = '';">
                                                                            Include Photo</label>
                                                                    </td>
                                                                </tr>
                                                                <tr runat="server" id="rowLogoFollowup">
                                                                    <td><br />
                                                                        <b><label id="lblLogo">Logo:</label></b>
                                                                    </td>
                                                                    <td><br />
                                                                        <wucLogo:LogoList runat="Server" ID="cmbLogoFollowup" Width='40'/>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div id="divLetterToDoctor" style="display: none;">
                                                <table style="width: 100%" border="0">
                                                    <tr>
                                                        <td width="50%" valign="top">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td style="width: 2%">
                                                                        <input type="checkbox" id="chkCurrentVisit" onclick="document.getElementById('chkLast10Visit').checked = false" /></td>
                                                                    <td>
                                                                        <label id="lblCurrentVisit" onclick="document.getElementById('chkCurrentVisit').checked = !document.getElementById('chkCurrentVisit').checked;
                                                                        document.getElementById('chkLast10Visit').checked = false" onmouseover="javascript:this.style.cursor = 'pointer';"
                                                                            onmouseout="javascript:this.style.cursor = '';">
                                                                            Current Visit Only</label>
                                                                    </td>
                                                                    <td style="width: 2%">
                                                                        <input type="checkbox" id="chkLast10Visit" onclick="document.getElementById('chkCurrentVisit').checked = false" />
                                                                    </td>
                                                                    <td>
                                                                        <label id="lblLast10Visit" onclick="document.getElementById('chkLast10Visit').checked = !document.getElementById('chkLast10Visit').checked;
                                                                        document.getElementById('chkCurrentVisit').checked = false" onmouseover="javascript:this.style.cursor = 'pointer';"
                                                                            onmouseout="javascript:this.style.cursor = '';">
                                                                            Last 10 Visits Only</label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 2%">
                                                                        <input type="checkbox" id="chkProgressNotes" /></td>
                                                                    <td>
                                                                        <label id="lblProgressNotes" onclick="document.getElementById('chkProgressNotes').checked = !document.getElementById('chkProgressNotes').checked;"
                                                                            onmouseover="javascript:this.style.cursor = 'pointer';" onmouseout="javascript:this.style.cursor = '';">
                                                                            Include Progress Notes</label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 2%">
                                                                        <input type="checkbox" id="chkPatientNote" /></td>
                                                                    <td>
                                                                        <label id="lblPatientNote" onclick="document.getElementById('chkPatientNote').checked = !document.getElementById('chkPatientNote').checked;"
                                                                            onmouseover="javascript:this.style.cursor = 'pointer';" onmouseout="javascript:this.style.cursor = '';">
                                                                            Include Patient Note</label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 2%">
                                                                        <input type="checkbox" id="chkComments" /></td>
                                                                    <td>
                                                                        <label id="lblComments" onclick="document.getElementById('chkComments').checked = !document.getElementById('chkComments').checked;"
                                                                            onmouseover="javascript:this.style.cursor = 'pointer';" onmouseout="javascript:this.style.cursor = '';">
                                                                            Include Comments</label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td width="25%" valign="top">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <label id="lblLetterTo" style="font-weight: bold">
                                                                            Letter to:</label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <asp:Label ID="lblWarning" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="2%">
                                                                        <input type="checkbox" id="cmbDoctor1" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblDoctor1" runat="server" onclick="document.getElementById('cmbDoctor1').checked = !document.getElementById('cmbDoctor1').checked;"
                                                                            onmouseover="javascript:this.style.cursor = 'pointer';" onmouseout="javascript:this.style.cursor = '';" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="2%">
                                                                        <input type="checkbox" id="cmbDoctor2" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblDoctor2" runat="server" onclick="document.getElementById('cmbDoctor2').checked = !document.getElementById('cmbDoctor2').checked;"
                                                                            onmouseover="javascript:this.style.cursor = 'pointer';" onmouseout="javascript:this.style.cursor = '';" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="2%">
                                                                        <input type="checkbox" id="cmbDoctor3" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblDoctor3" runat="server" onclick="document.getElementById('cmbDoctor3').checked = !document.getElementById('cmbDoctor3').checked;"
                                                                            onmouseover="javascript:this.style.cursor = 'pointer';" onmouseout="javascript:this.style.cursor = '';" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td width="25%" valign="top">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <label id="lblLetterFrom" style="font-weight: bold">
                                                                            Letter From:</label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label runat="server" ID="lblEmpty" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <wucDoctor:cmbDoctorsList runat="server" ID="cmbLetterFrom_PN" Width="95" IsHide="False" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <label id="lblLetterContent" style="font-weight: bold">
                                                                Content:</label></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <wucTextBox:TextBox runat="server" ID="txtFollowUpNotes_PN" textMode="MultiLine"
                                                                width="100%" maxLength="1024" rows="5" />
                                                        </td>
                                                    </tr>
                                                    <tr runat="server" id="rowLogoLetterDoctor">
                                                        <td colspan="3">
                                                            <b>Logo:</b> <wucLogo:LogoList runat="Server" ID="cmbLogoLetterDoctor" Width='40'/>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div id="divGraphs" style="display: none; visibility: hidden">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td width="100%">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td style="width: 2%">
                                                                        <input type="radio" name="rbGraphs" onclick="javascript:document.getElementById('ReportCode').value = 'EWLG';$get('chkIncActualLoss').checked = false;"
                                                                            checked />
                                                                    </td>
                                                                    <td>
                                                                        <label id="lblEWL" onclick="document.getElementsByName('rbGraphs')[0].checked = true;document.getElementById('ReportCode').value = 'EWLG';$get('chkIncActualLoss').checked = false;"
                                                                            onmouseover="javascript:this.style.cursor = 'pointer';" onmouseout="javascript:this.style.cursor = '';">
                                                                            % EWL Graph</label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 2%">
                                                                        <input type="radio" name="rbGraphs" onclick="document.getElementById('ReportCode').value = 'WLG';$get('chkIncActualLoss').checked = false;" />
                                                                    </td>
                                                                    <td>
                                                                        <label id="lblWL" onclick="document.getElementsByName('rbGraphs')[1].checked = true;document.getElementById('ReportCode').value = 'WLG';$get('chkIncActualLoss').checked = false;"
                                                                            onmouseover="javascript:this.style.cursor = 'pointer';" onmouseout="javascript:this.style.cursor = '';">
                                                                            Weight Loss Graph</label>
                                                                    </td>
                                                                </tr>
                                                                <tr id="IdealEWL" runat="server">
                                                                    <td style="width: 2%">
                                                                        <input type="radio" name="rbGraphs" onclick="javascript:document.getElementById('ReportCode').value = 'IEWLG';"/>
                                                                    </td>
                                                                    <td>
                                                                        <label id="lblIEWL" onclick="document.getElementsByName('rbGraphs')[2].checked = true;document.getElementById('ReportCode').value = 'IEWLG';"
                                                                            onmouseover="javascript:this.style.cursor = 'pointer';" onmouseout="javascript:this.style.cursor = '';">
                                                                            Target % Excess Weight Loss Graph</label>
                                                                    </td>
                                                                </tr>
                                                                <tr id="IncludeActualWeightLoss" runat="server">
                                                                    <td style="width: 2%">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <input type="checkbox" id="chkIncActualLoss" onclick="javascript:document.getElementsByName('rbGraphs')[2].checked = true;document.getElementById('ReportCode').value = 'IEWLG';"/>
                                                                        <label id="lblIAEWL" onclick="document.getElementsByName('rbGraphs')[2].checked = true;document.getElementById('ReportCode').value = 'IEWLG';$get('chkIncActualLoss').checked = !$get('chkIncActualLoss').checked;"
                                                                            onmouseover="javascript:this.style.cursor = 'pointer';" onmouseout="javascript:this.style.cursor = '';">
                                                                            Include Target Actual Weight Loss Graph</label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                    <br />
                                                                        <label id="lblGraphInfo">The graph is using the <b>Date</b> set in <b>Calculate visit weeks and weight loss</b> as the baseline to calculate weight loss</label><br /><label id="lblGraphInfo2">To generate the graph, please ensure the <b>Date</b> in <b>Calculate visit weeks and weight loss</b> is filled</label>
                                                                    </td>
                                                                </tr>
                                                                <tr style="visibility: hidden; display: none">
                                                                    <td style="width: 2%">
                                                                        <input type="radio" name="rbGraphs" />
                                                                    </td>
                                                                    <td>
                                                                        <label id="lblBloodPressure" onclick="document.getElementsByName('rbGraphs')[2].checked = true;document.getElementById('ReportCode').value = '';"
                                                                            onmouseover="javascript:this.style.cursor = 'pointer';" onmouseout="javascript:this.style.cursor = '';">
                                                                            Weight Pressure, Lipids, Diabetes</label>
                                                                    </td>
                                                                </tr>
                                                                <tr style="visibility: hidden; display: none">
                                                                    <td style="width: 2%">
                                                                        <input type="radio" name="rbGraphs" />
                                                                    </td>
                                                                    <td>
                                                                        <label id="lblHematology" onclick="document.getElementsByName('rbGraphs')[3].checked = true;document.getElementById('ReportCode').value = '';"
                                                                            onmouseover="javascript:this.style.cursor = 'pointer';" onmouseout="javascript:this.style.cursor = '';">
                                                                            Hematology, LFT Body Composition</label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div id="divSuper" style="display: none; visibility: hidden">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td width="100%">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td style="width: 2%">
                                                                        <input type="checkbox" id="chk99204" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lbl99204" runat="server" onclick="document.getElementById('chk99204').checked = !document.getElementById('chk99204').checked;"
                                                                            onmouseover="javascript:this.style.cursor = 'pointer';" onmouseout="javascript:this.style.cursor = '';">99204 - Extended Consultation - New Patient</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 2%">
                                                                        <input type="checkbox" id="chk99213" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lbl99213" runat="server" onclick="document.getElementById('chk99213').checked = !document.getElementById('chk99213').checked;"
                                                                            onmouseover="javascript:this.style.cursor = 'pointer';" onmouseout="javascript:this.style.cursor = '';">99213 - Detailed Consultation - Established Patient</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 2%">
                                                                        <input type="checkbox" id="chk99214" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lbl99214" runat="server" onclick="document.getElementById('chk99214').checked = !document.getElementById('chk99214').checked;"
                                                                            onmouseover="javascript:this.style.cursor = 'pointer';" onmouseout="javascript:this.style.cursor = '';">99214 - Extended Consultation - Established Patient</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 2%">
                                                                        <input type="checkbox" id="chkS2083" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblS2083" runat="server" onclick="document.getElementById('chkS2083').checked = !document.getElementById('chkS2083').checked;"
                                                                            onmouseover="javascript:this.style.cursor = 'pointer';" onmouseout="javascript:this.style.cursor = '';">S2083 or 43999 - Adjustment of Gastric Band</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 2%">
                                                                        <input type="checkbox" id="chk74230" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lbl74230" runat="server" onclick="document.getElementById('chk74230').checked = !document.getElementById('chk74230').checked;"
                                                                            onmouseover="javascript:this.style.cursor = 'pointer';" onmouseout="javascript:this.style.cursor = '';">74230 - Swallowing Function with Cineradiography</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 2%">
                                                                        <input type="checkbox" id="chk77002" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lbl77002" runat="server" onclick="document.getElementById('chk77002').checked = !document.getElementById('chk77002').checked;"
                                                                            onmouseover="javascript:this.style.cursor = 'pointer';" onmouseout="javascript:this.style.cursor = '';">77002 - Fluoroscopic Guidance</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 2%">
                                                                        <input type="checkbox" id="chk99212" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lbl99212" runat="server" onclick="document.getElementById('chk99212').checked = !document.getElementById('chk99212').checked;"
                                                                            onmouseover="javascript:this.style.cursor = 'pointer';" onmouseout="javascript:this.style.cursor = '';">99212 - Limited Consultation - Established Patient</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 2%">
                                                                        <input type="checkbox" id="chkSignBy" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblSignBy" runat="server" onclick="document.getElementById('chkSignBy').checked = !document.getElementById('chkSignBy').checked;"
                                                                            onmouseover="javascript:this.style.cursor = 'pointer';" onmouseout="javascript:this.style.cursor = '';"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr runat="server" id="rowLogoSuperBill">
                                                        <td colspan="3">
                                                            <b>Logo:</b> <wucLogo:LogoList runat="Server" ID="cmbLogoSuperBill" Width='40'/>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div id="divPrintBillBtns" style="display: none; visibility: hidden" class="printButtons">
                                                <iframe id="frameBillReport" style="width: 0; height: 0; visibility: hidden"></iframe>
                                                <input type="button" id="btnPrintBill" value="Preview" runat="server" onclick="document.getElementById('ReportCode').value = 'SB';javascript:BuildReport(1);"
                                                    style="width: 80px" />
                                            </div>
                                            <div id="divComorbidities" style="display: none; visibility: hidden">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 5%">
                                                            <input type="radio" name="rbComorbidities" onclick="document.getElementById('ReportCode').value = 'BLC';" />
                                                        </td>
                                                        <td>
                                                            <label id="lblBaseline" onclick="document.getElementsByName('rbComorbidities')[0].checked = true;"
                                                                onmouseover="javascript:this.style.cursor = 'pointer';" onmouseout="javascript:this.style.cursor = '';">
                                                                Baseline report</label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 5%">
                                                            <input type="radio" name="rbComorbidities" />
                                                        </td>
                                                        <td>
                                                            <label id="lblFollowUp" onclick="document.getElementsByName('rbComorbidities')[1].checked = true;"
                                                                onmouseover="javascript:this.style.cursor = 'pointer';" onmouseout="javascript:this.style.cursor = '';">
                                                                Follow up report</label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 5%">
                                                            <input type="radio" name="rbComorbidities" />
                                                        </td>
                                                        <td>
                                                            <label id="lblSF36" onclick="document.getElementsByName('rbComorbidities')[2].checked = true;"
                                                                onmouseover="javascript:this.style.cursor = 'pointer';" onmouseout="javascript:this.style.cursor = '';">
                                                                SF36 Graph</label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div id="divOtherReports" style="display: none; visibility: hidden">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 5%">
                                                            <input type="checkbox" id="chkOperation" onclick="javascript:CheckOperation(this, 0);" />
                                                        </td>
                                                        <td>
                                                            <label id="lblOperation" onclick="javascript:CheckOperation(document.getElementById('chkOperation'), 1); "
                                                                onmouseover="javascript:this.style.cursor = 'pointer';" onmouseout="javascript:this.style.cursor = '';">
                                                                Operation Report</label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 5%">
                                                            <input type="checkbox" id="chkPhotos" />
                                                        </td>
                                                        <td>
                                                            <label id="lblPhotos" onclick="document.getElementById('chkPhotos').checked = !document.getElementById('chkPhotos').checked;"
                                                                onmouseover="javascript:this.style.cursor = 'pointer';" onmouseout="javascript:this.style.cursor = '';">
                                                                Photos</label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div id="divPrintBtns" style="display: block" class="printButtons">
                                                <iframe id="frameReport" style="width: 0; height: 0; border-style: none"></iframe>
                                                <input type="button" id="btnDownloadExcel"  runat="server" value="Download Excel" onclick="javascript:BuildReport(2);"
                                                    style="width: 110px"/>
                                                <input type="button" id="btnDownloadPdf" value="Download PDF" onclick="javascript:BuildReport(3);"
                                                    style="width: 110px"/>
                                                <input type="button" id="btnDownloadWord" value="Download Word" onclick="javascript:BuildReport(4);"
                                                    style="width: 110px"/>
                                                &nbsp;<input type="button" id="btnPreview" value="Preview" onclick="javascript:BuildReport(1);"
                                                    style="width: 80px" />
                                                &nbsp;<input type="button" id="btnPrint" value="Print" onclick="javascript:BuildReport(0);"
                                                    style="width: 80px" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="clr">
                </div>
            </div>
            <div class="greyContentWrap" id="div_vFollowup" style="display: none">
                <asp:UpdatePanel runat="server" ID="UpdateFollowup">
                    <ContentTemplate>
                        <div id="div_vComplaint" class="bold">
                            <div class="btnWrap" align="right">
                                <table class="header">
                                    <tr>
                                        <td align="right">
                                            <input type="button" id="btnSaveFollowup" runat="server" value="Save" style="width: 100px"
                                                onserverclick="btnAddFollowUp_onserverclick" /></td>
                                    </tr>
                                </table>
                            </div>
                            <div class="wideBox">
                                <div class="boxTop">
                                    <h3 id="lblAttemptToContact">
                                        Attempts to Contact Patient</h3>
                                    <div class="addVisitDetails">
                                        <table class="full">
                                            <tr>
                                                <td style="width: 45%">
                                                    <label id="lblFollowupDate">Follow Up Date</label>
                                                </td>
                                                <td style="width: 55%">                                                
                                                    <wucTextBox:TextBox runat="server" ID="txtFollowupDate" width="20%" />
                                                    <a href="#this" type="button" id="a1" onclick="javascript:aCalendar_onclick(this, 'txtFollowupDate');">
                                                            [...]</a>
                                                </td>
                                             </tr>
                                             <tr>
                                                <td>
                                                    <label id="lblFollowupAppointment">Was a follow-up appointment made but patient did not show for appointment?</label>
                                                </td>
                                                <td>
                                                    <input type="radio" name="rbAppointment" id="rbAppointmentY" value="Y" runat="server" />
                                                    &nbsp;<label id="lblAppointmentYes">Yes</label> &nbsp; &nbsp;
                                                    <input type="radio" name="rbAppointment" id="rbAppointmentN" value="N" runat="server" />
                                                    &nbsp;<label id="lblAppointmentNo">No</label> &nbsp; &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblPhoneCall">Was a phone call placed to the patient?</label>
                                                </td>
                                                <td>
                                                    <input type="radio" name="rbPhone" id="rbPhoneT" value="T" runat="server" />
                                                    &nbsp;<label id="lblPhoneCallTwice">Twice</label> &nbsp; &nbsp;
                                                    <input type="radio" name="rbPhone" id="rbPhoneO" value="O" runat="server" />
                                                    &nbsp;<label id="lblPhoneCallOnce">Once</label> &nbsp; &nbsp;
                                                    <input type="radio" name="rbPhone" id="rbPhoneNVR" value="NVR" runat="server" />
                                                    &nbsp;<label id="lblPhoneCallNever">Never</label> &nbsp; &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblLetterSentPatient">Was a letter sent to the patient?</label>
                                                </td>
                                                <td>
                                                    <input type="radio" name="rbLetterPatient" id="rbLetterPatientT" value="T" runat="server" />
                                                    &nbsp;<label id="lblLetterSentPatientTwice">Twice</label> &nbsp; &nbsp;
                                                    <input type="radio" name="rbLetterPatient" id="rbLetterPatientO" value="O" runat="server" />
                                                    &nbsp;<label id="lblLetterSentPatientOnce">Once</label> &nbsp; &nbsp;
                                                    <input type="radio" name="rbLetterPatient" id="rbLetterPatientNVR" value="NVR" runat="server" />
                                                    &nbsp;<label id="lblLetterSentPatientNever">Never</label> &nbsp; &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblLetterSentPhysician">Was a letter sent to the patient's primary care physician?</label>
                                                </td>
                                                <td>
                                                    <input type="radio" name="rbLetterPhysician" id="rbLetterPhysicianT" value="T" runat="server" />
                                                    &nbsp;<label id="lblLetterSentPhysicianTwice">Twice</label> &nbsp; &nbsp;
                                                    <input type="radio" name="rbLetterPhysician" id="rbLetterPhysicianO" value="O" runat="server" />
                                                    &nbsp;<label id="lblLetterSentPhysicianOnce">Once</label> &nbsp; &nbsp;
                                                    <input type="radio" name="rbLetterPhysician" id="rbLetterPhysicianNVR" value="NVR"
                                                        runat="server" />
                                                &nbsp;<label id="lblLetterSentPhysicianNever">Never</label> &nbsp; &nbsp;
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblPatientTransferred">Was the patient's care transferred to another bariatric specialist?</label>
                                                </td>
                                                <td>
                                                    <input type="radio" name="rbTransfer" id="rbTransferY" value="Y" runat="server" />
                                                    &nbsp;<label id="lblPatientTransferredYes">Yes</label> &nbsp; &nbsp;
                                                    <input type="radio" name="rbTransfer" id="rbTransferN" value="N" runat="server" />
                                                    &nbsp;<label id="lblPatientTransferredNo">No</label> &nbsp; &nbsp; &nbsp; &nbsp; <label id="lblPatientTransferredName">Name:</label>
                                                    <input type="text" runat="server" id="txtLetterPhysician" size="30" /></td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblPatientRefuseFollowup">Is patient refusing long-term follow-up?</label>
                                                </td>
                                                <td>
                                                    <input type="radio" name="rbRefuseFup" id="rbRefuseFupY" value="Y" runat="server" />
                                                    &nbsp;<label id="lblPatientRefuseFollowupYes">Yes</label> &nbsp; &nbsp;
                                                    <input type="radio" name="rbRefuseFup" id="rbRefuseFupN" value="N" runat="server" />
                                                    &nbsp;<label id="lblPatientRefuseFollowupNo">No</label> &nbsp; &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblPatientLoseFollowup">Is the patient lost to follow-up?</label>
                                                </td>
                                                <td>
                                                    <input type="radio" name="rbLostFup" id="rbLostFupY" value="Y" runat="server" />
                                                    &nbsp;<label id="lblPatientLoseFollowupYes">Yes</label> &nbsp; &nbsp;
                                                    <input type="radio" name="rbLostFup" id="rbLostFupN" value="N" runat="server" />
                                                    &nbsp;<label id="lblPatientLoseFollowupNo">No</label> &nbsp; &nbsp;
                                                </td>
                                            </tr>
                                            <tr><td>&nbsp;</td></tr>                                            
                                            <tr>
                                                <td>
                                                    <label id="lblDonotContact">Do not contact?</label>
                                                </td>
                                                <td>
                                                    <input type="checkbox" id="chkDoNotContact" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div class="boxBtm">
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
                <!--<asp:AsyncPostBackTrigger ControlID="linkBtnSave" EventName="Click" /> -->
                <div class="clr">
                </div>
            </div>
        </div>
        <asp:UpdatePanel runat="server" ID="UpdatePanel41" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnAddVisit" EventName="serverclick" />
                <asp:AsyncPostBackTrigger ControlID="btnAddFile" EventName="serverclick" />
                <asp:AsyncPostBackTrigger ControlID="btnLoadVisitData" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnCalculateWeightOtherData" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnLoadBaselineComorbidity" EventName="Click" />
            </Triggers>
            <ContentTemplate>
                <textarea runat="server" id="txtTest" rows="5" cols="10" style="display: none" />
                <asp:HiddenField runat="Server" ID="txtHConsultID" Value="0" />
                <asp:HiddenField runat="Server" ID="txtHCommentID" Value="0" />
                <asp:LinkButton runat="server" ID="btnLoadVisitData" Text="Load Visit Data" OnClick="btnLoadVisitData_OnClick" />
                <asp:LinkButton runat="server" ID="btnCalculateWeightOtherData" OnClick="btnCalculateWeightOtherData_OnClick" />
                <asp:LinkButton runat="server" ID="btnLoadBaselineComorbidity" OnClick="btnLoadBaselineComorbidity_OnClick" />
                <asp:LinkButton runat="server" ID="btnSaveReportNotes" OnClick="btnSaveReportNotes_OnClick" />
                <asp:LinkButton runat="server" ID="btnLoadAllVisit" OnClick = "btnLoadAllVisit_OnClick" style="display:none;"/>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:HiddenField runat="Server" ID="txtHType" />
        <asp:HiddenField runat="server" ID="txtHComplication" />
        <asp:HiddenField runat="server" ID="txtHSaveResult" Value="0" />
        <asp:HiddenField runat="server" ID="txtHApplicationURL" />
        <asp:HiddenField runat="server" ID="txtHPatientID" Value="0" />
        <asp:HiddenField runat="server" ID="txtHCurrentClientDate" Value="" />
        <asp:HiddenField runat="server" ID="txtHCurrentDate" Value="" />
        <asp:HiddenField runat="server" ID="txtHDateCreated" />
        <asp:HiddenField runat="server" ID="txtHBaselineOperationDate" Value="" />
        <asp:HiddenField runat="server" ID="txtHDiabetesPeriodMin" Value="" />
        <asp:HiddenField runat="server" ID="txtHDiabetesPeriodMax" Value="" />
        <asp:HiddenField runat="server" ID="txtHDateFirstVisit" Value="" />
        <asp:HiddenField runat="server" ID="txtHLatestOperationDate" Value="" />
        <asp:HiddenField runat="server" ID="txtHSEPeriodMin" Value="" />
        <asp:HiddenField runat="server" ID="txtHSEPeriodMax" Value="" />
        <asp:HiddenField runat="server" ID="txtHPermissionLevel" />
        <asp:HiddenField runat="server" ID="txtHDataClamp" />
        <asp:HiddenField runat="Server" ID="txtHPageNo" Value="1" />
        <asp:HiddenField runat="server" ID="txtUseImperial" Value="0" />
        <asp:HiddenField runat="server" ID="txtHSaveFlag" Value="0" />
        <asp:HiddenField runat="server" ID="ReportCode" Value="" />
        <asp:HiddenField runat="Server" ID="txtHCulture" />
        <asp:HiddenField runat="Server" ID="TitleLoaded" Value="0" />
        <asp:HiddenField runat="Server" ID="txtHDelete" Value="0" />
        <asp:HiddenField runat="Server" ID="txtHTempConsultID" Value="0" />
        <asp:HiddenField runat="Server" ID="txtCurrList" Value="visit" />
        <asp:HiddenField runat="Server" ID="txtHFinalVol" Value="0" />
        <asp:HiddenField runat="server" ID="txtMedication_Selected" />
        <asp:HiddenField runat="server" ID="txtHSuperBill" Value="0"/>
        <asp:HiddenField runat="server" ID="txtHLogoMandatory" Value="0"/>
        <asp:HiddenField runat="server" ID="txtHInitRegistrySleepApnea" Value=""/>
        <asp:HiddenField runat="server" ID="txtHInitRegistryGerd" Value=""/>
        <asp:HiddenField runat="server" ID="txtHInitRegistryHyperlipidemia" Value=""/>
        <asp:HiddenField runat="server" ID="txtHInitRegistryDiabetes" Value=""/>
        <asp:HiddenField runat="server" ID="txtHInitRegistryHypertension" Value=""/>
        <asp:HiddenField runat="server" ID="txtHRegistryDone" Value=""/>
        <textarea id="txtDoctorsList" rows="10" style="width: 100%; display: none" cols="80"></textarea>
    </form>
</body>
</html>
