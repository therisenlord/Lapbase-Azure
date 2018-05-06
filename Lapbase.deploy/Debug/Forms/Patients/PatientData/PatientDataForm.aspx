<%@ page language="C#" autoeventwireup="true" inherits="Forms_Patients_PatientData_PatientDataForm, Lapbase.deploy" validaterequest="false" enableEventValidation="false" viewStateEncryptionMode="Always" %>

<%@ Register TagPrefix="wucSystemCode" TagName="SystemCodeList" Src="~/UserControl/SystemCodeWUCtrl.ascx" %>
<%@ Register TagPrefix="wucDoctor" TagName="DoctorList" Src="~/UserControl/DoctorsListWUCtrl.ascx" %>
<%@ Register TagPrefix="wucCode" TagName="CodeList" Src="~/UserControl/CodeWUCtrl.ascx" %>
<%@ Register TagPrefix="wucTextBox" TagName="TextBox" Src="~/UserControl/TextBoxWUCtrl.ascx" %>
<%@ Register TagPrefix="wucTextArea" TagName="TextArea" Src="~/UserControl/TextAreaWUCtrl.ascx" %>
<%@ Register TagPrefix="wucMenu" TagName="Menu" Src="~/UserControl/MenuWUCtrl.ascx" %>
<%@ Register TagPrefix="wucPatient" TagName="PatientTitle" Src="~/UserControl/PatientTitleDataWUCtrl.ascx" %>
<%@ Register TagPrefix="wucAppSchema" TagName="AppSchema" Src="~/UserControl/AppSchemaWUCtrl.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title>LapBase - A Data Manager for Bariatric Surgery</title>
    <!-- flooding div stylesheet-->
    <link rel="stylesheet" href="../../../css/FloatingDIV/default.css" media="screen,projection"
        type="text/css" />
    <link rel="stylesheet" href="../../../css/FloatingDIV/lightbox.css" media="screen,projection"
        type="text/css" />
    <link href="~/css/MultiColumn.css" rel="stylesheet" type="text/css" runat="server" />
    <link href='<%$ AppSettings:CssUrl%>' rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript" src="../../../Scripts/Global.js"></script>

    <script language="javascript" type="text/javascript" src="Includes/PatientDataForm.js"></script>

    <script language="javascript" type="text/javascript" src="../../RefferingDoctor/Includes/RefferingDoctor.js"></script>

    <!-- flooding div javascript-->

    <script type="text/javascript" src="../../../scripts/FloatingDIV/prototype.js"></script>

    <script type="text/javascript" src="../../../scripts/FloatingDIV/lightbox.js"></script>

    <!-- Calendar -->
    <link rel="stylesheet" href="../../../css/Calendar/calendar.css" media="screen" />

    <script type="text/javascript" src="../../../Scripts/Calendar/calendar.js"></script>

</head>
<body runat="server" id="bodyPatientData">
    <wucMenu:Menu runat="server" ID="mainMenu" />
    <div class="tabMenus">
        <wucAppSchema:AppSchema runat="server" ID="AppSchemaMenu" currentItem="Baseline" />
        <div class="manilaTabMenu">
            <ul>
                <li class="current" id="li_Div1"><a href="#this" onclick="javascript:controlBar_Buttons_OnClick(1);"
                    id="ub_mnuItem01">
                    <img src="~/img/tab_demographics.gif" alt="" height="16" width="16" runat="server" /><span
                        id="lblDemographics">Demographics</span></a> </li>
                <li id="li_Div2"><a href="#this" onclick="javascript:controlBar_Buttons_OnClick(2);"
                    id="ub_mnuItem02">
                    <img src="~/img/tab_height_weight_notes.gif" alt="" height="16" width="16" runat="server" /><span
                        id="lblHeightWeightNotes">Height / Weight / Notes</span></a> </li>
                <li id="li_Div3" style="display: none"><a href="#this" onclick="javascript:controlBar_Buttons_OnClick(3);"
                    id="ub_mnuItem03">
                    <img src="~/img/tab_height_weight_notes.gif" alt="" height="16" width="16" runat="server" /><span
                        id="lblMajorComorbidity">Major Comorbidity</span></a> </li>
                <li id="li_Div4" style="display: none"><a href="#this" onclick="javascript:controlBar_Buttons_OnClick(4);"
                    id="ub_mnuItem04">
                    <img id="Img1" src="~/img/tab_height_weight_notes.gif" alt="" height="16" width="16"
                        runat="server" /><span id="lblMinorComorbidity">Minor Comorbidity</span></a>
                </li>
                <li id="li_Div5"><a href="#this" onclick="javascript:controlBar_Buttons_OnClick(5);"
                    id="ub_mnuItem05">
                    <img id="Img2" src="~/img/tab_height_weight_notes.gif" alt="" height="16" width="16"
                        runat="server" /><span id="Span1">BOLD Data</span></a> </li>
                <li id="li_Div9"><a href="#this" onclick="javascript:controlBar_Buttons_OnClick(9);"
                    id="ub_mnuItem09">
                    <img id="Img6" src="~/img/tab_height_weight_notes.gif" alt="" height="16" width="16"
                        runat="server" /><span id="Span5">Previous Surgeries</span></a> </li>
                <li id="li_Div6"><a href="#this" onclick="javascript:controlBar_Buttons_OnClick(6);"
                    id="ub_mnuItem06">
                    <img id="Img3" src="~/img/tab_height_weight_notes.gif" alt="" height="16" width="16"
                        runat="server" /><span id="Span2">Baseline Comorbidity</span></a> </li>
                <!-- <li id = "li_Div7" style ="display:block">
				    <a href="#this" onclick="javascript:controlBar_Buttons_OnClick(7);" id="ub_mnuItem07" >
				        <img id="Img4" src="~/img/tab_height_weight_notes.gif" alt="" height="16" width="16" runat = "server"/><span id = "Span3">Medications / Vitamins</span></a> 
				</li> -->
                <li id="li_Div8"><a href="#this" onclick="javascript:controlBar_Buttons_OnClick(8);"
                    id="ub_mnuItem08">
                    <img id="Img5" src="~/img/tab_progress_notes.gif" alt="" height="16" width="16" runat="server" /><span
                        id="Span4">Comorbidity Notes</span></a> </li>
            </ul>
        </div>
    </div>
    <form id="frmPatientData" runat="server" autocomplete="off">
        <asp:ScriptManager runat="server" ID="scriptManager_Baseline" />
        <asp:UpdatePanel runat="server" ID="upLinkBtnSave">
            <ContentTemplate>
                <asp:LinkButton runat="server" ID="linkBtnSave" OnClick="linkBtnSave_OnClick" />
                <asp:LinkButton runat="server" ID="LinkBtnSave_Height" OnClick="linkBtnSave_OnClick" />
                <asp:LinkButton runat="server" ID="LinkBtnSave_BoldData" OnClick="linkBtnSave_OnClick" />
                <asp:LinkButton runat="server" ID="LinkBtnSave_BoldPreviousSurgery" OnClick="linkBtnSave_OnClick" />
                <asp:LinkButton runat="server" ID="LinkBtnSave_BoldComorbidity" OnClick="linkBtnSave_OnClick" />
                <asp:LinkButton runat="server" ID="LinkBtnSave_Medication" OnClick="linkBtnSave_OnClick" />
                <asp:LinkButton runat="server" ID="linkBtnCalculateAge" OnClick="linkBtnCalculateAge_OnClick" />
                <asp:LinkButton runat="server" ID="LinkBtnSave_BoldComorbidityNotes" OnClick="linkBtnSave_OnClick" />
                <asp:LinkButton runat="server" ID="LinkBtnSave_RefDr" OnClick="linkBtnRefDoctorSave_OnClick" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="linkBtnSave" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <div class="contentArea">
            <div id="divErrorMessage" style="display: none;">
                <span>
                    <p id="pErrorMessage">
                    </p>
                </span>
            </div>
            <wucPatient:PatientTitle runat="server" ID="tblPatientTitle" />
            <div class="greyContentWrap">
                <asp:UpdatePanel runat="server" ID="upDemographics">
                    <ContentTemplate>
                        <div class="baselineDemographics" id="div_vDemographics">
                            <div class="btnWrap" align="right">
                                <input type="button" value="Delete" runat="server" id="btnDeletePatient1" style="width: 100px;"
                                    onclick="javascript:btnDelete_onclick();" onserverclick="btnDeletePatient_onserverclick" />
                                <input type="button" id="Button2" runat="server" value="Save" style="width: 100px"
                                    onclick="javascript:controlBar_Buttons_OnClick(1);" />
                            </div>
                            <div class="leftColumn">
                                <div class="personalDetails">
                                    <div class="boxTop">
                                        <h3 id="hPersonalDetails">
                                            Personal Details</h3>
                                        <table border="0">
                                            <tbody>
                                                <tr>
                                                    <td style="width: 30%;">
                                                        <asp:Label runat="server" ID="lblPatientID" Text="Patient ID" /></td>
                                                    <td style="width: 60%;">
                                                        <wucTextBox:TextBox runat="server" ID="txtPatient_CustomID" Text="" />
                                                        <asp:HiddenField runat="server" ID="txtPatientID" Value="0" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblSurName" Text="Surname" /></td>
                                                    <td>
                                                        <wucTextBox:TextBox runat="server" ID="txtSurName" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblFirstName" Text="Firstname" /></td>
                                                    <td>
                                                        <wucTextBox:TextBox runat="server" ID="txtFirstName" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblTitle" Text="Title" /></td>
                                                    <td>
                                                        <select runat="server" id="rblTitle" onchange="javascript:rblTitle_onchange(this);">
                                                            <option value="0">Select...</option>
                                                            <option value="1">Mr.</option>
                                                            <option value="2">Mrs.</option>
                                                            <option value="3">Miss</option>
                                                            <option value="5">Ms.</option>
                                                            <option value="4">Dr.</option>
                                                            <option value="6">Mstr</option>
                                                        </select>
                                                    </td>
                                                </tr>
                                                <tr style="height: 20px;">
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr style="height: 20px;">
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <table border="0">
                                            <tbody>
                                                <tr>
                                                    <td style="width: 40%;">
                                                        <asp:Label runat="Server" ID="lblBirthDate" Text="Birthdate" />&nbsp;
                                                        <asp:Label runat="Server" ID="lblDateFormat" Text="" Style="display: none" />
                                                    </td>
                                                    <td style="width: 60%;">
                                                        <asp:UpdatePanel runat="server" ID="upBirthDate">
                                                            <ContentTemplate>
                                                                <wucTextBox:TextBox runat="server" ID="txtBirthDate" maxLength="10" width="50%" />
                                                                <a href="#this" id="aCalendar" onclick="javascript:aCalendar_onclick(this,'txtBirthDate');">
                                                                    [...]</a>&nbsp;
                                                                <label id="lblAge" runat="Server">
                                                                    0</label>&nbsp;<asp:Label runat="server" ID="lblyears" Text="yrs" />
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <%--<asp:AsyncPostBackTrigger ControlID = "linkBtnSave" EventName="Click" />--%>
                                                                <asp:AsyncPostBackTrigger ControlID="linkBtnCalculateAge" EventName="Click" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblGender" Text="Gender" /></td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="rblGender">
                                                            <asp:ListItem Value="M" Selected="true">Male</asp:ListItem>
                                                            <asp:ListItem Value="F">Female</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblRace" Text="Ethnicity" /></td>
                                                    <td>
                                                        <wucSystemCode:SystemCodeList runat="Server" ID="cmbRace" CriteriaString="RACE" />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <div class="boxBtm">
                                        </div>
                                    </div>
                                </div>
                                <div class="addressDetails">
                                    <div class="boxTop">
                                        <h3 id="hAddressDetails">
                                            Address Details</h3>
                                        <table border="0">
                                            <tbody>
                                                <tr>
                                                    <td style="width: 30%">
                                                        <asp:Label runat="server" ID="lblStreet" Text="Street" /></td>
                                                    <td style="width: 60%">
                                                        <wucTextBox:TextBox runat="server" ID="txtStreet" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblCity" Text="City" /></td>
                                                    <td>
                                                        <wucTextBox:TextBox runat="server" ID="txtCity" />
                                                        <asp:HiddenField runat="server" ID="txtHCity" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblState" Text="State" /></td>
                                                    <td>
                                                        <wucTextBox:TextBox runat="server" ID="txtState" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblPostCode" Text="Zip / PostCode" /></td>
                                                    <td>
                                                        <wucTextBox:TextBox runat="server" ID="txtPostCode" />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <div class="boxBtm">
                                        </div>
                                    </div>
                                </div>
                                <div class="contactDetails">
                                    <div class="boxTop">
                                        <h3 id="hContactDetails">
                                            Contact Details</h3>
                                        <table border="0">
                                            <tbody>
                                                <tr>
                                                    <td style="width: 30%">
                                                        <asp:Label runat="server" ID="lblEmail" Text="Email" /></td>
                                                    <td style="width: 60%">
                                                        <wucTextBox:TextBox runat="server" ID="txtEmail" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblPhone_H" Text="Phone (Home)" /></td>
                                                    <td>
                                                        <wucTextBox:TextBox runat="server" ID="txtPhone_H" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblPhone_W" Text="(Work)" /></td>
                                                    <td>
                                                        <wucTextBox:TextBox runat="server" ID="txtPhone_W" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblMobile" Text="Cell / Mobile" /></td>
                                                    <td>
                                                        <wucTextBox:TextBox runat="server" ID="txtMobile" />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <div class="boxBtm">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="rightColumn">
                                <div class="medicalDetails">
                                    <div class="boxTop">
                                        <h3 id="hMedicalDetails">
                                            Medical Details</h3>
                                        <table border="0">
                                            <tbody>
                                                <tr>
                                                    <td style="width: 30%">
                                                        <asp:Label Text="Surgeon" runat="server" ID="lblSurgeon" /></td>
                                                    <td style="width: 60%">
                                                        <wucDoctor:DoctorList runat="server" ID="cmbSurgon" Width="90" IsSurgeon="True" IsHide="False" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label Text="Referred By" runat="server" ID="lblReferredBy" /></td>
                                                    <td>
                                                        <wucTextBox:TextBox runat="server" ID="txtReferredBy" width="79%" />
                                                        <a id="aRefDr1" href="../../RefferingDoctor/RefferingDoctorForm.aspx" class="lbOn">[New]</a>
                                                        <asp:HiddenField runat="server" ID="txtHReferredBy" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label runat="Server" ID="lblOtherDrs" Text="Other Doctors" /></td>
                                                    <td>
                                                        <wucTextBox:TextBox runat="server" ID="txtOtherDoctors1" width="79%" />
                                                        <a id="aRefDr2" href="../../RefferingDoctor/RefferingDoctorForm.aspx" class="lbOn">[New]</a>
                                                        <asp:HiddenField runat="server" ID="txtHOtherDoctors1" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <wucTextBox:TextBox runat="server" ID="txtOtherDoctors2" width="79%" />
                                                        <a id="aRefDr3" href="../../RefferingDoctor/RefferingDoctorForm.aspx" class="lbOn">[New]</a>
                                                        <asp:HiddenField runat="server" ID="txtHOtherDoctors2" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%">
                                                        <asp:Label Text="Insurance" runat="server" ID="lblInsurance" /></td>
                                                    <td style="width: 60%">
                                                        <wucTextBox:TextBox runat="server" ID="txtInsurance" width="79%" />
                                                        <asp:HiddenField runat="server" ID="txtHInsurance" />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <div class="boxBtm">
                                        </div>
                                    </div>
                                </div>
                                <div class="contactDetails">
                                    <div class="boxTop">
                                        <h3 id="hSocialHistory">
                                            Social History</h3>
                                        <table border="0">
                                            <tbody>
                                                <tr>
                                                    <td style="width: 90%">
                                                        <wucTextArea:TextArea runat="server" ID="txtSocialHistory" width="99%" rows="4" />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <div class="boxBtm">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <asp:HiddenField runat="Server" ID="IsViewPage1" Value="0" />
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="linkBtnSave" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:UpdatePanel runat="server" ID="upDetails" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="baselineNotes" id="div_vDetails" style="display: none">
                            <div class="btnWrap" align="right">
                                <input type="button" value="Delete" runat="server" id="btnDeletePatient2" style="width: 100px;"
                                    onclick="javascript:btnDelete_onclick();" onserverclick="btnDeletePatient_onserverclick" />
                                <input type="button" id="Button1" runat="server" value="Save" style="width: 100px"
                                    onclick="javascript:controlBar_Buttons_OnClick(2);" />
                            </div>
                            <div class="leftColumn">
                                <div class="heightWeight">
                                    <div class="boxTop">
                                        <h3 id="hHeightWeight">
                                            Baseline</h3>
                                        <table style="width: 100%;" border="0">
                                            <tbody>
                                                <tr>
                                                    <td colspan="4">
                                                        <asp:Label Text="BaseLine" ID="lblBaseLine" ForeColor="BLUE" runat="Server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 40%;">
                                                        <asp:Label runat="server" ID="lblHeight" Text="Height" />
                                                    </td>
                                                    <td style="width: 30%;">
                                                        <wucTextBox:TextBox runat="server" ID="txtHeight" width="90%" Text="0" />
                                                    </td>
                                                    <td style="width: 20%;">
                                                        <asp:Label runat="server" ID="lblTxtHeight" Text="Inches" />
                                                    </td>
                                                    <td style="width: 10%;">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblStartWeight" Text="Weight" />
                                                    </td>
                                                    <td>
                                                        <wucTextBox:TextBox runat="server" ID="txtStartWeight" width="90%" Text="0" />
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblWeightSize" Text="lbs" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <asp:Label Text="Ideal Weight based on BMI of 25" ID="lblIdealWeightTitle" ForeColor="BLUE"
                                                            runat="Server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblIdealWeight" Text="Ideal" />
                                                    </td>
                                                    <td>
                                                        <label id="lblIdealWeight_Value" runat="server">
                                                            0</label>
                                                        <wucTextBox:TextBox runat="server" ID="txtIdealWeight" width="90%" Text="0" />
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblTxtIWeight" Text="lbs" /></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblBMI" Text="BMI" /></td>
                                                    <td>
                                                        <wucTextBox:TextBox runat="server" ID="txtBMI" width="90%" ReadOnly="true" Text="0"
                                                            Enabled="true" />
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblExcessWeight" Text="Excess Weight" /></td>
                                                    <td>
                                                        <wucTextBox:TextBox runat="server" ID="txtExcessWeight" width="90%" maxLength="5"
                                                            Text="0" />
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblTxtEWeight" Text="lbs" /></td>
                                                </tr>
                                                <tr style="display: none">
                                                    <td colspan="3">
                                                        <asp:Label runat="server" ID="lblCurrentWeightTitle" Text="CURRENT" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblCurrentWeight" Text="Current Weight" Style="color: Blue;" /></td>
                                                    <td>
                                                        <label id="lblCurrentWeight_Value" runat="server">
                                                            0</label>
                                                        <wucTextBox:TextBox runat="server" ID="txtCurrentWeight" width="90%" Text="0" />
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="Server" ID="lblCurrentEWL1" Text="" /></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblTargetWeight" Text="Target Weight" Style="color: Blue;" /></td>
                                                    <td>
                                                        <wucTextBox:TextBox runat="server" ID="txtTargetWeightBaseline" width="90%" Text="0" />
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblTxtETargetWeight" Text="lbs" /></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <input type="button" id="btnBMI" value="" onclick="javascript:btnBMIBaseline_onclick();"
                                                            style="width: 20%" />
                                                        <asp:Label runat="server" ID="Label2" Text="BMI" />&nbsp;&nbsp;
                                                        <asp:Label runat="server" ID="lblBMI_Value" Text="27" ForeColor="blue" />
                                                    </td>
                                                    <td colspan="2">
                                                        <input type="button" id="btnExcessWeight" value="" onclick="javascript:btnExcessWeightBaseline_onclick();"
                                                            style="width: 20%" />
                                                        <asp:Label runat="server" ID="lblExcessWeight_Value" Text="66" ForeColor="blue" />&nbsp;
                                                        <asp:Label runat="server" ID="Label1" Text="% Excess Wt" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3" style="color: blue">
                                                        Zero Date
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label Text="Date" runat="server" ID="lblZeroDate" /></td>
                                                    <td colspan="2">
                                                        <wucTextBox:TextBox runat="server" ID="txtZeroDate" maxLength="10" width="50%" />
                                                        <a href="#this" id="a1" onclick="javascript:aCalendar_onclick(this,'txtZeroDate');">
                                                            [...]</a>&nbsp;
                                                    </td>
                                                </tr>                                                
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblVisitWeeksFlag" runat="Server" Text="Calculate visit weeks from" />
                                                    </td>
                                                    <td colspan="2">
                                                        <wucSystemCode:SystemCodeList runat="server" ID="cmbVisitWeeks" Width="100" CriteriaString="VISITWEEK"
                                                            FirstRow="false" />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <div class="boxBtm">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="rightColumn">
                                <div class="notes">
                                    <div class="boxTop">
                                        <h3 id="hNotes">
                                            Notes</h3>
                                        <table style="height: 214px; width: 600px" border="0">
                                            <tbody>
                                                <tr>
                                                    <td>
                                                        <wucTextBox:TextBox runat="server" ID="txtNotes" width="100%" height="100%" textMode="MultiLine"
                                                            maxLength="2048" rows="13" />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <div class="boxBtm">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <asp:HiddenField runat="Server" ID="IsViewPage2" Value="0" />
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="LinkBtnSave_Height" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:UpdatePanel runat="server" ID="upBoldData">
                    <ContentTemplate>
                        <div id="div_vBoldData" class="bold" style="display: none">
                            <div class="btnWrap" align="right">
                                <input type="button" value="Delete" runat="server" id="btnDeletePatient3" style="width: 100px;"
                                    onclick="javascript:btnDelete_onclick();" onserverclick="btnDeletePatient_onserverclick" />
                                <input type="button" id="Button3" runat="server" value="Save" style="width: 100px"
                                    onclick="javascript:controlBar_Buttons_OnClick(5);" />
                            </div>
                            <div class="personalDetails">
                                <div class="boxTop">
                                    <h3 id="hMedicalDetails_Bold">
                                        Personal Details</h3>
                                    <table border="0">
                                        <tbody>
                                            <tr>
                                                <td style="width: 40%;">
                                                    <asp:Label runat="server" ID="lblSSN" Text="SSN" /></td>
                                                <td style="width: 60%;">
                                                    <wucTextBox:TextBox runat="server" ID="txtSSN" maxLength="11" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="lblChartNumber" Text="Chart Number" /></td>
                                                <td>
                                                    <wucTextBox:TextBox runat="server" ID="txtChartNumber" maxLength="20" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="lblBoldChartNumber" Text="BOLD Chart Number" /></td>
                                                <td>
                                                    <asp:Label runat="server" ID="txtBoldChartNumber" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="lblEmploymentStatus" Text="Employment status" /></td>
                                                <td>
                                                    <wucSystemCode:SystemCodeList runat="Server" ID="cmbEmployment" CriteriaString="EMP" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label runat="server" ID="lblSend2SRC" Text="Patient Consent to SRC use of data?" />
                                                    &nbsp;&nbsp;
                                                    <div class="inline">
                                                        <input type="radio" name="rdSend2SRC" id="rdSend2SRC_Yes" value="1" runat="server" />
                                                        &nbsp;
                                                        <label id="lblSend2SRC_Yes">
                                                            Yes</label>&nbsp;&nbsp;
                                                        <input type="radio" name="rdSend2SRC" id="rdSend2SRC_No" value="0" runat="server"
                                                            checked />
                                                        &nbsp;
                                                        <label id="lblSend2SRC_No">
                                                            No</label>
                                                    </div>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                            </div>
                            <div class="insuranceInformation">
                                <div class="boxTop">
                                    <h3 id="hInsuranceInformation">
                                        Insurance Information</h3>
                                    <table border="0">
                                        <tr>
                                            <td>
                                                <asp:Label Text="Employer" runat="server" ID="lblEmployer" /></td>
                                            <td>
                                                <wucTextBox:TextBox runat="server" ID="txtEmployer" maxLength="50" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <label id="lblHealthInsuranceCover">
                                                    Does health insurance cover the procedure?</label>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div class="inline">
                                                    <input type="radio" name="rbHealthInsurance" id="rbHealthInsurance_Yes" value="1"
                                                        runat="server" />&nbsp;
                                                    <label id="lblHealthInsurance_Yes">
                                                        Yes</label>
                                                </div>
                                                <div class="inline">
                                                    <input type="radio" name="rbHealthInsurance" id="rbHealthInsurance_No" value="0"
                                                        runat="server" />&nbsp;
                                                    <label id="lblHealthInsurance_No">
                                                        No</label>
                                                </div>
                                                <div class="inline">
                                                    <input type="radio" name="rbHealthInsurance" id="rbHealthInsurance_Unknown" value="-1"
                                                        runat="server" checked />&nbsp;
                                                    <label id="lblHealthInsurance_UnKnown">
                                                        Not known</label>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label Text="Sec. Coverage" runat="server" ID="lblSecondaryCoverage" /></td>
                                            <td>
                                                <wucTextBox:TextBox runat="server" ID="txtSecondaryCoverage" maxLength="50" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label Text="Tert. Coverage" runat="server" ID="lblTertiaryCoverage" /></td>
                                            <td>
                                                <wucTextBox:TextBox runat="server" ID="txtTertiaryCoverage" maxLength="50" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2"><table border="0">
                                            <tr>
                                                <td colspan="3" style="font-weight: bold">
                                                    Payment</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <input type="checkbox" id="chkPrivateIns" runat="server" />
                                                    &nbsp;<asp:Label Text="Private insurance" runat="server" ID="lblPrivateIns" /></td>
                                                <td>
                                                    <input type="checkbox" id="chkMedicaid" runat="server" />
                                                    &nbsp;<asp:Label Text="Medicaid" runat="server" ID="lblMedicaid" />
                                                </td>
                                                <td>
                                                    <input type="checkbox" id="chkSelfPay" runat="server" />
                                                    &nbsp;<asp:Label Text="Self Pay" runat="server" ID="lblSelfPay" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <input type="checkbox" id="chkGovernmentIns" runat="server" />
                                                    &nbsp;<asp:Label Text="Government insurance" runat="server" ID="lblGovernmentIns" /></td>
                                                <td>
                                                    <input type="checkbox" id="chkMedicare" runat="server" />
                                                    &nbsp;<asp:Label Text="Medicare" runat="server" ID="lblMedicare" /></td>
                                                <td>
                                                    <input type="checkbox" id="chkCharity" runat="server" />
                                                    &nbsp;<asp:Label Text="Charity" runat="server" ID="lblCharity" /></td>
                                            </tr>
                                            </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <table border="0">
                                        <tr>
                                            <td style="width: 7%">
                                                <input type="checkbox" id="chkPreOPWeightLoss" runat="server" />
                                            </td>
                                            <td>
                                                <label id="lblPreOPWeightLoss">
                                                    Pre-operative weight loss</label>
                                                &nbsp;
                                                <wucTextBox:TextBox runat="server" ID="txtPreOpWeightLoss" width="50px" maxLength="6" />
                                                &nbsp;
                                                <asp:Label runat="server" ID="lblPreOpWeightLoss_Unit" Text="kg" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="checkbox" id="chkDietWeightLoss" runat="server" />
                                            </td>
                                            <td>
                                                <label id="lblDietaryWeightLoss">
                                                    Dietary weight loss</label>
                                                &nbsp;
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbDietaryWeightLoss" CriteriaString="DWL"
                                                    Width="50" BoldData="DWL" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="checkbox" id="chkDurationObesity" runat="server" />
                                            </td>
                                            <td>
                                                <label id="lblDurationObesity">
                                                    Duration of obesity</label></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="checkbox" id="chkSmokingCessation" runat="server" />
                                            </td>
                                            <td>
                                                <label id="lblSmokingCessation">
                                                    Smoking Cessation</label></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="checkbox" id="chkMentalHealthClearance" runat="server" />
                                            </td>
                                            <td>
                                                <label id="lblMentalHealthClearance">
                                                    Mental Health Clearance</label></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="checkbox" id="chkIntelligenceTesting" runat="server" />
                                            </td>
                                            <td>
                                                <label id="lblIntelligenceTesting">
                                                    Intelligence (IQ) Testing</label></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="checkbox" id="chkPreCert_Other" onclick="javascript:chkPreCert_Other_onclick(this, 'txtPreCert_Other_txtGlobal')"
                                                    runat="server" /></td>
                                            <td>
                                                <label id="lblPreCert_Other">
                                                    Other</label>&nbsp;
                                                <wucTextBox:TextBox runat="server" ID="txtPreCert_Other" maxLength="50" Enabled="false" />
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                            </div>
                            <div class="wideBox">
                                <div class="boxTop">
                                    <h3 id="hPreviousNonBariatricSurgeries">
                                        Previous Non-Bariatric Surgeries</h3>
                                    <div class="addVisitDetails">
                                        <table border="0">
                                            <tr style="vertical-align: top">
                                                <td style="width: 48%">
                                                    <label id="lblPrevNonBarSurgeriesSearch">
                                                        Search by
                                                    </label>
                                                    <wucTextBox:TextBox runat="server" ID="txtPrevNonBarSurgeriesSearch" />
                                                </td>
                                                <td style="width: 4%" />
                                                <td style="width: 48%" />
                                            </tr>
                                            <tr>
                                                <td>
                                                    <select id="listPrevNonBariatricSurgery" multiple="true" style="width: 100%" size="5"
                                                        runat="server" ondblclick="javascript:BoldLists_dblclick('listPrevNonBariatricSurgery', 'listPrevNonBariatricSurgery_Selected', 'Add');" />
                                                    <wucSystemCode:SystemCodeList runat="server" ID="cmbPrevNonBariatricSurgery" CriteriaString="NBST"
                                                        BoldData="NBST" Display="false" />
                                                </td>
                                                <td style="vertical-align: middle; text-align: center">
                                                    <a href="#this" onclick="javascript:BoldLButtonLinks_click('listPrevNonBariatricSurgery', 'listPrevNonBariatricSurgery_Selected', 'Add', false);">
                                                        ></a><br />
                                                    <a href="#this" onclick="javascript:BoldLButtonLinks_click('listPrevNonBariatricSurgery', 'listPrevNonBariatricSurgery_Selected', 'Add', true);">
                                                        >></a><br />
                                                    <a href="#this" onclick="javascript:BoldLButtonLinks_click('listPrevNonBariatricSurgery_Selected', 'listPrevNonBariatricSurgery', 'Remove', false);">
                                                        <</a><br />
                                                    <a href="#this" onclick="javascript:BoldLButtonLinks_click('listPrevNonBariatricSurgery_Selected', 'listPrevNonBariatricSurgery', 'Remove', true);">
                                                        <<</a><br />
                                                </td>
                                                <td>
                                                    <select id="listPrevNonBariatricSurgery_Selected" multiple="true" style="width: 100%"
                                                        size="5" runat="server" ondblclick="javascript:BoldLists_dblclick('listPrevNonBariatricSurgery_Selected', 'listPrevNonBariatricSurgery', 'Remove');" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                            </div>
                            <asp:HiddenField runat="Server" ID="IsViewPage5" Value="0" />
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="LinkBtnSave_BoldData" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:UpdatePanel runat="server" ID="upPreviousSurgery">
                    <ContentTemplate>
                        <div id="div_vPreviousSurgery" class="bold" style="display: none">
                            <div class="btnWrap" align="right">
                                <input type="button" value="Delete" runat="server" id="btnDeletePatient4" style="width: 100px;"
                                    onclick="javascript:btnDelete_onclick();" onserverclick="btnDeletePatient_onserverclick" />
                                <input type="button" id="Button7" runat="server" value="Save" style="width: 100px"
                                    onclick="javascript:controlBar_Buttons_OnClick(9);" />
                            </div>
                            <div class="wideBox">
                                <div class="boxTop">
                                    <h3 id="hPreviousBariatricSurgeries">
                                        Previous Bariatric Surgeries</h3>
                                    <div class="addVisitDetails">
                                        <table border="0" style="width: 900px">
                                            <tr>
                                                <td style="width: 15%">
                                                    <label id="lblPrevBarSurgery_Year">
                                                        Year</label></td>
                                                <td style="width: 27%">
                                                    <wucTextBox:TextBox runat="server" width="50px" ID="txtPrevBarSurgery_Year" maxLength="4" />
                                                </td>
                                                <td colspan="3">
                                                    <label id="lblAdverseEventsSearch">
                                                        Adverse Events associated - Search by
                                                    </label>
                                                    <wucTextBox:TextBox runat="server" width="120px" ID="txtAdverseEventsSearch" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100px">
                                                    <label id="lblOrgWeight">
                                                        Original Weight</label></td>
                                                <td style="height: 22px">
                                                    <wucTextBox:TextBox runat="server" width="50px" ID="txtOrgWeight" maxLength="5" />
                                                    &nbsp;
                                                    <asp:Label ID="lblOrgWeight_Unit" runat="server" Text="kg" CssClass="inline" />&nbsp;&nbsp;
                                                    <input type="radio" name="rbOrgWeight" id="rbOrgWeight_Estimated" runat="server"
                                                        checked />&nbsp;
                                                    <label id="lblOrgWeight_Estimated">
                                                        Estimated</label>&nbsp;
                                                    <input type="radio" name="rbOrgWeight" id="rbOrgWeight_Actual" runat="server" />&nbsp;
                                                    <label id="lblOrgWeight_Actual">
                                                        Actual</label>
                                                </td>
                                                <td rowspan="3">
                                                    <select id="listAdverseEvents" multiple="true" size="10" runat="server" style="width: 225px"
                                                        ondblclick="javascript:BoldLists_dblclick('listAdverseEvents', 'listAdverseEvents_Selected', 'Add');" />
                                                    <wucSystemCode:SystemCodeList runat="server" ID="cmbAdverseEvents" CriteriaString="ADEV"
                                                        BoldData="ADEV" Display="false" />
                                                </td>
                                                <td rowspan="3" style="vertical-align: middle; text-align: center">
                                                    <a href="#this" onclick="javascript:BoldLButtonLinks_click('listAdverseEvents', 'listAdverseEvents_Selected', 'Add', false);">
                                                        ></a><br />
                                                    <a href="#this" onclick="javascript:BoldLButtonLinks_click('listAdverseEvents', 'listAdverseEvents_Selected', 'Add', true);">
                                                        >></a><br />
                                                    <a href="#this" onclick="javascript:BoldLButtonLinks_click('listAdverseEvents_Selected', 'listAdverseEvents', 'Remove', false);">
                                                        <</a><br />
                                                    <a href="#this" onclick="javascript:BoldLButtonLinks_click('listAdverseEvents_Selected', 'listAdverseEvents', 'Remove', true);">
                                                        <<</a><br />
                                                </td>
                                                <td rowspan="3">
                                                    <select id="listAdverseEvents_Selected" multiple="true" size="10" runat="server"
                                                        style="width: 225px" ondblclick="javascript:BoldLists_dblclick('listAdverseEvents_Selected', 'listAdverseEvents', 'Add');" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblLowestWeightAchieved">
                                                        Lowest weight achieved</label></td>
                                                <td>
                                                    <wucTextBox:TextBox runat="server" width="50px" ID="txtLowestWeightAchieved" maxLength="5" />
                                                    &nbsp;
                                                    <asp:Label ID="lblLowestWeightAchieved_Unit" runat="server" Text="kg" />&nbsp;&nbsp;
                                                    <input type="radio" name="rbLowestWeightAchieved" id="rbLowestWeightAchieved_Estimated"
                                                        runat="server" checked />&nbsp;
                                                    <label id="lblLowestWeightAchieved_Estimated">
                                                        Estimated</label>&nbsp;&nbsp;
                                                    <input type="radio" name="rbLowestWeightAchieved" id="rbLowestWeightAchieved_Actual"
                                                        runat="server" />&nbsp;
                                                    <label id="lblLowestWeightAchieved_Actual">
                                                        Actual</label>&nbsp;&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <table cellpadding="0" cellspacing="0" border="0" style="width: 400px">
                                                        <tr>
                                                            <td colspan="3">
                                                                <label id="lblPrevBariatricSearch">
                                                                    Previous Bariatric Surgeries - Search by
                                                                </label>
                                                                <wucTextBox:TextBox runat="server" ID="txtPrevBariatricSearch" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td rowspan="2" style="width: 48%">
                                                                <select id="listPrevBariatricSurgery" multiple="true" size="5" runat="server" style="width: 180px"
                                                                    ondblclick="javascript:BoldLists_dblclick('listPrevBariatricSurgery', 'listPrevBariatricSurgery_Selected', 'Add');" />
                                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbPrevBariatricSurgery" CriteriaString="PBS"
                                                                    BoldData="PBS" Display="false" />
                                                            </td>
                                                            <td rowspan="2" style="width: 4%; vertical-align: middle; text-align: center">
                                                                <a href="#this" onclick="javascript:BoldLButtonLinks_click('listPrevBariatricSurgery', 'listPrevBariatricSurgery_Selected', 'Add', false);">
                                                                    ></a><br />
                                                                <a href="#this" onclick="javascript:BoldLButtonLinks_click('listPrevBariatricSurgery', 'listPrevBariatricSurgery_Selected', 'Add', true);">
                                                                    >></a><br />
                                                                <a href="#this" onclick="javascript:BoldLButtonLinks_click('listPrevBariatricSurgery_Selected', 'listPrevBariatricSurgery', 'Remove', false);">
                                                                    <</a><br />
                                                                <a href="#this" onclick="javascript:BoldLButtonLinks_click('listPrevBariatricSurgery_Selected', 'listPrevBariatricSurgery', 'Remove', true);">
                                                                    <<</a><br />
                                                            </td>
                                                            <td rowspan="2" style="width: 48%;">
                                                                <select id="listPrevBariatricSurgery_Selected" multiple="true" size="5" runat="server"
                                                                    style="width: 190px" ondblclick="javascript:BoldLists_dblclick('listPrevBariatricSurgery_Selected', 'listPrevBariatricSurgery', 'Remove');" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                            </div>
                            <div class="wideBox" style="display: none">
                                <div class="boxTop">
                                    <h3 id="hAdverseEvents">
                                        Adverse Events associated with Previous Bariatric Surgery</h3>
                                    <div class="addVisitDetails">
                                        <table border="0">
                                            <tr style="vertical-align: top">
                                                <td style="width: 48%" />
                                                <td style="width: 4%" />
                                                <td style="width: 48%" />
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                            </div>
                            <asp:HiddenField runat="Server" ID="IsViewPage9" Value="0" />
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="LinkBtnSave_BoldPreviousSurgery" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:UpdatePanel runat="server" ID="upBoldComorbidity" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div id="div_vBoldComorbidity" style="display: none" class="comorbiditiesMinor">
                            <div class="btnWrap" align="right">
                                <input type="button" value="Delete" runat="server" id="btnDeletePatient5" style="width: 100px;"
                                    onclick="javascript:btnDelete_onclick();" onserverclick="btnDeletePatient_onserverclick" />
                                <input type="button" id="Button4" runat="server" value="Mark everything not set as no history or symptoms"
                                    onclick="javascript:btnComorbiditySetDefault_onclick();" />
                                <input type="button" id="Button6" runat="server" value="Save" style="width: 100px"
                                    onclick="javascript:controlBar_Buttons_OnClick(6);" />
                            </div>
                            <div class="leftColumn">
                                <div class="sectionBox1">
                                    <div class="boxTop">
                                        <h3 id="hCardiovascularDisease">
                                            CARDIOVASCULAR DISEASE</h3>
                                        <table style="width: 290px" border="0">
                                            <tr>
                                                <td style="width: 90px">
                                                    <label id="lblHypertension">
                                                        Hypertension</label>
                                                </td>
                                                <td style="width: 200px">
                                                    <wucSystemCode:SystemCodeList runat="server" ID="cmbHypertension" CriteriaString="HYPER"
                                                        BoldData="CVS" SCode="PRE" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblCongestiveHeartFailure">
                                                        Congestive Heart Failure</label></td>
                                                <td>
                                                    <wucSystemCode:SystemCodeList runat="server" ID="cmbCongestive" CriteriaString="CONG"
                                                        BoldData="CVS" SCode="PRE" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblIschemicHeartDisease">
                                                        Ischemic Heart Disease</label></td>
                                                <td>
                                                    <wucSystemCode:SystemCodeList runat="server" ID="cmbIschemic" CriteriaString="ISCH"
                                                        BoldData="CVS" SCode="PRE" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblAnginaAssessment">
                                                        Angina Assessment</label></td>
                                                <td>
                                                    <wucSystemCode:SystemCodeList runat="server" ID="cmbAngina" CriteriaString="ANGI"
                                                        BoldData="CVS" SCode="PRE" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblPeripheralVascularDisease">
                                                        Peripheral Vascular Disease</label></td>
                                                <td>
                                                    <wucSystemCode:SystemCodeList runat="server" ID="cmbPeripheral" CriteriaString="PERI"
                                                        BoldData="CVS" SCode="PRE" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblLowerExtremityEdema">
                                                        Lower Extremity Edema</label></td>
                                                <td>
                                                    <wucSystemCode:SystemCodeList runat="server" ID="cmbLower" CriteriaString="LOWE"
                                                        BoldData="CVS" SCode="PRE" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblDVT">
                                                        DVT/PE</label></td>
                                                <td>
                                                    <wucSystemCode:SystemCodeList runat="server" ID="cmbDVT" CriteriaString="DVTP" BoldData="CVS"
                                                        SCode="PRE" />
                                                </td>
                                            </tr>
                                        </table>
                                        <div class="boxBtm">
                                        </div>
                                    </div>
                                </div>
                                <div class="sectionBox2">
                                    <div class="boxTop">
                                        <h3 id="hMetabolic">
                                            METABOLIC</h3>
                                        <table style="width: 290px" border="0">
                                            <tr>
                                                <td style="width: 90px">
                                                    <label id="lblGlucoseMetabolism">
                                                        Glucose Metabolism</label>
                                                </td>
                                                <td style="width: 200px">
                                                    <wucSystemCode:SystemCodeList runat="server" ID="cmbGlucose" CriteriaString="GLUC"
                                                        BoldData="MET" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblLipids">
                                                        Lipids (Dyslipidemia or Hyperlipidemia)</label>
                                                </td>
                                                <td>
                                                    <wucSystemCode:SystemCodeList runat="server" ID="cmbLipids" CriteriaString="LIPI"
                                                        BoldData="MET" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblGoutHyperuricemia">
                                                        Gout Hyperuricemia</label>
                                                </td>
                                                <td>
                                                    <wucSystemCode:SystemCodeList runat="server" ID="cmbGout" CriteriaString="GOUT" BoldData="MET" />
                                                </td>
                                            </tr>
                                        </table>
                                        <div class="boxBtm">
                                        </div>
                                    </div>
                                </div>
                                <div class="sectionBox1">
                                    <div class="boxTop">
                                        <h3 id="hGastroIntestinal">
                                            GASTROINTESTINAL</h3>
                                        <table style="width: 290px">
                                            <tr>
                                                <td style="width: 70px">
                                                    <label id="lblGerd">
                                                        GERD</label>
                                                </td>
                                                <td style="width: 220px">
                                                    <wucSystemCode:SystemCodeList runat="server" ID="cmbGred" CriteriaString="GERD" BoldData="GAS"
                                                        SCode="PRE" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblCholelithiasis">
                                                        Cholelithiasis</label>
                                                </td>
                                                <td>
                                                    <wucSystemCode:SystemCodeList runat="server" ID="cmbCholelithiasis" CriteriaString="CHOL"
                                                        BoldData="GAS" SCode="PRE" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblLiverDisease">
                                                        Liver Disease</label>
                                                </td>
                                                <td>
                                                    <wucSystemCode:SystemCodeList runat="server" ID="cmbLiver" CriteriaString="LIVE"
                                                        BoldData="GAS" SCode="PRE" />
                                                </td>
                                            </tr>
                                        </table>
                                        <div class="boxBtm">
                                        </div>
                                    </div>
                                </div>
                                <div class="sectionBox2">
                                    <div class="boxTop">
                                        <h3 id="hMusculoskeletal">
                                            MUSCULOSKELETAL</h3>
                                        <table border="0" style="width: 290px">
                                            <tr>
                                                <td style="width: 70px">
                                                    <label id="lblBackPain">
                                                        Back Pain</label>
                                                </td>
                                                <td style="width: 220px">
                                                    <wucSystemCode:SystemCodeList runat="server" ID="cmbBackPain" CriteriaString="BACK"
                                                        BoldData="MUS" SCode="PRE" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblMusculoskeletalDisease">
                                                        Musculoskeletal Disease</label>
                                                </td>
                                                <td>
                                                    <wucSystemCode:SystemCodeList runat="server" ID="cmbMusculoskeletal" CriteriaString="MUSC"
                                                        BoldData="MUS" SCode="PRE" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblFibromyalgia">
                                                        Fibromyalgia</label>
                                                </td>
                                                <td>
                                                    <wucSystemCode:SystemCodeList runat="server" ID="cmbFibro" CriteriaString="FIBR"
                                                        BoldData="MUS" SCode="PRE" />
                                                </td>
                                            </tr>
                                        </table>
                                        <div class="boxBtm">
                                        </div>
                                    </div>
                                </div>
                                <div class="sectionBox1">
                                    <div class="boxTop">
                                        <h3 id="hPsychosocial">
                                            PSYCHOSOCIAL</h3>
                                        <table style="width: 290px" border="0">
                                            <tr>
                                                <td style="width: 90px">
                                                    <label id="lblPsychosocialImpairment">
                                                        Psychosocial Impairment</label>
                                                </td>
                                                <td style="width: 200px">
                                                    <wucSystemCode:SystemCodeList runat="server" ID="cmbPsychosocial" CriteriaString="PSYC"
                                                        BoldData="PSY" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblDepression">
                                                        Depression</label></td>
                                                <td>
                                                    <wucSystemCode:SystemCodeList runat="server" ID="cmbDepression" CriteriaString="DEPR"
                                                        BoldData="PSY" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblConfirmedMentalHealthDiagnosis">
                                                        Confirmed Mental Health Diagnosis</label></td>
                                                <td>
                                                    <wucSystemCode:SystemCodeList runat="server" ID="cmbConfirmed" CriteriaString="MENT"
                                                        BoldData="PSY" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblAlcoholUse">
                                                        Alcohol Use</label></td>
                                                <td>
                                                    <wucSystemCode:SystemCodeList runat="server" ID="cmbAlcohol" CriteriaString="ALCO"
                                                        BoldData="PSY" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblTobaccoUse">
                                                        Tobacco Use</label></td>
                                                <td>
                                                    <wucSystemCode:SystemCodeList runat="server" ID="cmbTobacco" CriteriaString="TOBA"
                                                        BoldData="PSY" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblSubstanceAbuse">
                                                        Substance Abuse</label></td>
                                                <td>
                                                    <wucSystemCode:SystemCodeList runat="server" ID="cmbAbuse" CriteriaString="SUBS"
                                                        BoldData="PSY" />
                                                </td>
                                            </tr>
                                        </table>
                                        <div class="boxBtm">
                                        </div>
                                    </div>
                                </div>
                                <div class="sectionBox2">
                                    <div class="boxTop">
                                        <h3 id="hGeneral">
                                            GENERAL</h3>
                                        <table style="width: 290px" border="0">
                                            <tr>
                                                <td style="width: 90px">
                                                    <label id="lblStressUrinaryIncontinence">
                                                        Stress Urinary Incontinence</label></td>
                                                <td style="width: 200px">
                                                    <wucSystemCode:SystemCodeList runat="server" ID="cmbStressUrinary" CriteriaString="STRE"
                                                        BoldData="GEN" SCode="PRE" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lbl">
                                                        Pseudotumor Cerebri</label></td>
                                                <td>
                                                    <wucSystemCode:SystemCodeList runat="server" ID="cmbCerebri" CriteriaString="PSEU"
                                                        BoldData="GEN" SCode="PRE" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblAbdominalHernia">
                                                        Abdominal Hernia</label></td>
                                                <td>
                                                    <wucSystemCode:SystemCodeList runat="server" ID="cmbHernia" CriteriaString="ABDO"
                                                        BoldData="GEN" SCode="PRE" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblFunctionalStatus">
                                                        Functional Status</label></td>
                                                <td>
                                                    <wucSystemCode:SystemCodeList runat="server" ID="cmbFunctional" CriteriaString="FUNC"
                                                        BoldData="GEN" SCode="PRE" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblAbdominal">
                                                        Abdominal Skin/Pannus</label></td>
                                                <td>
                                                    <wucSystemCode:SystemCodeList runat="server" ID="cmbSkin" CriteriaString="ABDP" BoldData="GEN"
                                                        SCode="PRE" />
                                                </td>
                                            </tr>
                                        </table>
                                        <div class="boxBtm">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="rightColumn">
                                <div class="sectionBox3">
                                    <div class="boxTop">
                                        <h3 id="hPulmonary">
                                            PULMONARY</h3>
                                        <table style="width: 290px" border="0">
                                            <tr>
                                                <td style="width: 90px">
                                                    <label id="lblObstructiveSleepApneaSyndrome">
                                                        Obstructive Sleep Apnea</label>
                                                </td>
                                                <td style="width: 200px">
                                                    <wucSystemCode:SystemCodeList runat="server" ID="cmbObstructive" CriteriaString="OBST"
                                                        BoldData="PUL" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblObesityHypoventilationSyndrome">
                                                        Obesity Hypoventilation Syndrome</label>
                                                </td>
                                                <td>
                                                    <wucSystemCode:SystemCodeList runat="server" ID="cmbObesity" CriteriaString="OBES"
                                                        BoldData="PUL" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblPulmonary Hypertension">
                                                        Pulmonary Hypertension</label>
                                                </td>
                                                <td>
                                                    <wucSystemCode:SystemCodeList runat="server" ID="cmbPulmonary" CriteriaString="PULM"
                                                        BoldData="PUL" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblAsthma">
                                                        Asthma</label>
                                                </td>
                                                <td>
                                                    <wucSystemCode:SystemCodeList runat="server" ID="cmbAsthma" CriteriaString="ASTH"
                                                        BoldData="PUL" />
                                                </td>
                                            </tr>
                                        </table>
                                        <div class="boxBtm">
                                        </div>
                                    </div>
                                </div>
                                <div class="sectionBox3">
                                    <div class="boxTop">
                                        <h3 id="hReproductive">
                                            REPRODUCTIVE</h3>
                                        <table style="width: 290px" border="0">
                                            <tr>
                                                <td style="width: 90px">
                                                    <label id="lblPolycysticOverianSyndrome">
                                                        Polycystic Ovary Syndrome</label>
                                                </td>
                                                <td style="width: 200px">
                                                    <wucSystemCode:SystemCodeList runat="server" ID="cmbPolycystic" CriteriaString="POLY"
                                                        BoldData="FEM" SCode="PRE" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblMenstrualIrregularities">
                                                        Menstrual Irregularities (not PCOS)</label>
                                                </td>
                                                <td>
                                                    <wucSystemCode:SystemCodeList runat="server" ID="cmbMenstrual" CriteriaString="MENS"
                                                        BoldData="FEM" SCode="PRE" />
                                                </td>
                                            </tr>
                                        </table>
                                        <div class="boxBtm">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <asp:HiddenField runat="Server" ID="IsViewPage6" Value="0" />
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="LinkBtnSave_BoldComorbidity" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:UpdatePanel runat="server" ID="upBoldComorbidityNotes" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="visits" id="div_vBoldComorbidityNotes" style="display: none;">
                            <div class="btnWrap" align="right">
                                <input type="button" value="Delete" runat="server" id="btnDeletePatient6" style="width: 100px;"
                                    onclick="javascript:btnDelete_onclick();" onserverclick="btnDeletePatient_onserverclick" />
                                <input type="button" id="Button5" runat="server" value="Save" style="width: 100px"
                                    onclick="javascript:controlBar_Buttons_OnClick(8);" />
                            </div>
                            <div class="expandList">
                                <div class="boxTop" id="hCardiovascularDiseaseNotes" style="display: none;" runat="server">
                                    <div class="expandListTitle">
                                        <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                            <tbody>
                                                <tr style="vertical-align: middle;">
                                                    <td>
                                                        <b>CARDIOVASCULAR DISEASE</b></td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                    <div class="addVisitDetails">
                                        <table width="100%">
                                            <tbody>
                                                <tr style="vertical-align: top;" id="lnHypertensionNotes" runat="server">
                                                    <td style="width: 100%">
                                                        <asp:Label Text="Hypertension" ID="lblHypertensionNotes" runat="server" />
                                                        <wucTextBox:TextBox runat="server" ID="cmbHypertensionNotes" textMode="MultiLine"
                                                            rows="1" width="910px" />
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;" id="lnCongestiveNotes" runat="server">
                                                    <td>
                                                        <asp:Label Text="Congestive Heart Failure" ID="lblCongestiveHeartFailureNotes" runat="server" />
                                                        <wucTextBox:TextBox runat="server" ID="cmbCongestiveNotes" textMode="MultiLine" rows="1"
                                                            width="910px" />
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;" id="lnIschemicNotes" runat="server">
                                                    <td>
                                                        <asp:Label Text="Ischemic Heart Disease" ID="lblIschemicHeartDiseaseNotes" runat="server" />
                                                        <wucTextBox:TextBox runat="server" ID="cmbIschemicNotes" textMode="MultiLine" rows="1"
                                                            width="910px" />
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;" id="lnAnginaNotes" runat="server">
                                                    <td>
                                                        <asp:Label Text="Angina Assessment" ID="lblAnginaAssessmentNotes" runat="server" />
                                                        <wucTextBox:TextBox runat="server" ID="cmbAnginaNotes" textMode="MultiLine" rows="1"
                                                            width="910px" />
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;" id="lnPeripheralNotes" runat="server">
                                                    <td>
                                                        <asp:Label Text="Peripheral Vascular Disease" ID="lblPeripheralVascularDiseaseNotes"
                                                            runat="server" />
                                                        <wucTextBox:TextBox runat="server" ID="cmbPeripheralNotes" textMode="MultiLine" rows="1"
                                                            width="910px" />
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;" id="lnLowerNotes" runat="server">
                                                    <td>
                                                        <asp:Label Text="Lower Extremity Edema" ID="lblLowerExtremityEdemaNotes" runat="server" />
                                                        <wucTextBox:TextBox runat="server" ID="cmbLowerNotes" textMode="MultiLine" rows="1"
                                                            width="910px" />
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;" id="lnDVTNotes" runat="server">
                                                    <td>
                                                        <asp:Label Text="DVT/PE" ID="lblDVTNotes" runat="server" />
                                                        <wucTextBox:TextBox runat="server" ID="cmbDVTNotes" textMode="MultiLine" rows="1"
                                                            width="910px" />
                                                        <br />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                                <div class="boxTop" id="hMetabolicNotes" style="display: none;" runat="server">
                                    <div class="expandListTitle">
                                        <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                            <tbody>
                                                <tr style="vertical-align: middle;">
                                                    <td>
                                                        <b>METABOLIC</b></td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                    <div class="addVisitDetails">
                                        <table>
                                            <tbody>
                                                <tr style="vertical-align: top;" id="lnGlucoseNotes" runat="server">
                                                    <td>
                                                        <asp:Label Text="Glucose Metabolism" ID="lblGlucoseMetabolismNotes" runat="server" />
                                                        <wucTextBox:TextBox runat="server" ID="cmbGlucoseNotes" textMode="MultiLine" rows="1"
                                                            width="910px" />
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;" id="lnLipidsNotes" runat="server">
                                                    <td>
                                                        <asp:Label Text="Lipids (Dyslipidemia or Hyperlipidemia)" ID="lblLipidsNotes" runat="server" />
                                                        <wucTextBox:TextBox runat="server" ID="cmbLipidsNotes" textMode="MultiLine" rows="1"
                                                            width="910px" />
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;" id="lnGoutNotes" runat="server">
                                                    <td>
                                                        <asp:Label Text="Gout Hyperuricemia" ID="lblGoutHyperuricemiaNotes" runat="server" />
                                                        <wucTextBox:TextBox runat="server" ID="cmbGoutNotes" textMode="MultiLine" rows="1"
                                                            width="910px" />
                                                        <br />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                                <div class="boxTop" id="hPulmonaryNotes" style="display: none;" runat="server">
                                    <div class="expandListTitle">
                                        <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                            <tbody>
                                                <tr style="vertical-align: middle;">
                                                    <td>
                                                        <b>PULMONARY</b></td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                    <div class="addVisitDetails">
                                        <table>
                                            <tbody>
                                                <tr style="vertical-align: top;" id="lnObstructiveNotes" runat="server">
                                                    <td>
                                                        <asp:Label Text="Obstructive Sleep Apnea" ID="lblObstructiveSleepApneaSyndromeNotes"
                                                            runat="server" />
                                                        <wucTextBox:TextBox runat="server" ID="cmbObstructiveNotes" textMode="MultiLine"
                                                            rows="1" width="910px" />
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;" id="lnObesityNotes" runat="server">
                                                    <td>
                                                        <asp:Label Text="Obesity Hypoventilation Syndrome" ID="lblObesityHypoventilationSyndromeNotes"
                                                            runat="server" />
                                                        <wucTextBox:TextBox runat="server" ID="cmbObesityNotes" textMode="MultiLine" rows="1"
                                                            width="910px" />
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;" id="lnPulmonaryNotes" runat="server">
                                                    <td>
                                                        <asp:Label Text="Pulmonary Hypertension" ID="lblPulmonaryHypertensionNotes" runat="server" />
                                                        <wucTextBox:TextBox runat="server" ID="cmbPulmonaryNotes" textMode="MultiLine" rows="1"
                                                            width="910px" />
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;" id="lnAsthmaNotes" runat="server">
                                                    <td>
                                                        <asp:Label Text="Asthma" ID="lblAsthmaNotes" runat="server" />
                                                        <wucTextBox:TextBox runat="server" ID="cmbAsthmaNotes" textMode="MultiLine" rows="1"
                                                            width="910px" />
                                                        <br />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                                <div class="boxTop" id="hGastroIntestinalNotes" style="display: none;" runat="server">
                                    <div class="expandListTitle">
                                        <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                            <tbody>
                                                <tr style="vertical-align: middle;">
                                                    <td>
                                                        <b>GASTROINTESTINAL</b></td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                    <div class="addVisitDetails">
                                        <table>
                                            <tbody>
                                                <tr style="vertical-align: top;" id="lnGredNotes" runat="server">
                                                    <td>
                                                        <asp:Label Text="GERD" ID="lblGerdNotes" runat="server" />
                                                        <wucTextBox:TextBox runat="server" ID="cmbGredNotes" textMode="MultiLine" rows="1"
                                                            width="910px" />
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;" id="lnCholelithiasisNotes" runat="server">
                                                    <td>
                                                        <asp:Label Text="Cholelithiasis" ID="lblCholelithiasisNotes" runat="server" />
                                                        <wucTextBox:TextBox runat="server" ID="cmbCholelithiasisNotes" textMode="MultiLine"
                                                            rows="1" width="910px" />
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;" id="lnLiverNotes" runat="server">
                                                    <td>
                                                        <asp:Label Text="Liver Disease" ID="lblLiverDiseaseNotes" runat="server" />
                                                        <wucTextBox:TextBox runat="server" ID="cmbLiverNotes" textMode="MultiLine" rows="1"
                                                            width="910px" />
                                                        <br />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                                <div class="boxTop" id="hMusculoskeletalNotes" style="display: none;" runat="server">
                                    <div class="expandListTitle">
                                        <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                            <tbody>
                                                <tr style="vertical-align: middle;">
                                                    <td>
                                                        <b>MUSCULOSKELETAL</b></td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                    <div class="addVisitDetails">
                                        <table>
                                            <tbody>
                                                <tr style="vertical-align: top;" id="lnBackPainNotes" runat="server">
                                                    <td>
                                                        <asp:Label Text="Back Pain" ID="lblBackPainNotes" runat="server" />
                                                        <wucTextBox:TextBox runat="server" ID="cmbBackPainNotes" textMode="MultiLine" rows="1"
                                                            width="910px" />
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;" id="lnMusculoskeletalNotes" runat="server">
                                                    <td>
                                                        <asp:Label Text="Musculoskeletal Disease" ID="lblMusculoskeletalDiseaseNotes" runat="server" />
                                                        <wucTextBox:TextBox runat="server" ID="cmbMusculoskeletalNotes" textMode="MultiLine"
                                                            rows="1" width="910px" />
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;" id="lnFibroNotes" runat="server">
                                                    <td>
                                                        <asp:Label Text="Fibromyalgia" ID="lblFibromyalgiaNotes" runat="server" />
                                                        <wucTextBox:TextBox runat="server" ID="cmbFibroNotes" textMode="MultiLine" rows="1"
                                                            width="910px" />
                                                        <br />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                                <div class="boxTop" id="hReproductiveNotes" style="display: none;" runat="server">
                                    <div class="expandListTitle">
                                        <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                            <tbody>
                                                <tr style="vertical-align: middle;">
                                                    <td>
                                                        <b>REPRODUCTIVE</b></td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                    <div class="addVisitDetails">
                                        <table>
                                            <tbody>
                                                <tr style="vertical-align: top;" id="lnPolycysticNotes" runat="server">
                                                    <td>
                                                        <asp:Label Text="Polycystic Ovary Syndrome" ID="lblPolycysticOverianSyndromeNotes"
                                                            runat="server" />
                                                        <wucTextBox:TextBox runat="server" ID="cmbPolycysticNotes" textMode="MultiLine" rows="1"
                                                            width="910px" />
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;" id="lnMenstrualNotes" runat="server">
                                                    <td>
                                                        <asp:Label Text="Menstrual Irregularities (not PCOS)" ID="lblMenstrualIrregularitiesNotes"
                                                            runat="server" />
                                                        <wucTextBox:TextBox runat="server" ID="cmbMenstrualNotes" textMode="MultiLine" rows="1"
                                                            width="910px" />
                                                        <br />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                                <div class="boxTop" id="hPsychosocialNotes" style="display: none;" runat="server">
                                    <div class="expandListTitle">
                                        <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                            <tbody>
                                                <tr style="vertical-align: middle;">
                                                    <td>
                                                        <b>PSYCHOSOCIAL</b></td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                    <div class="addVisitDetails">
                                        <table>
                                            <tbody>
                                                <tr style="vertical-align: top;" id="lnPsychosocialNotes" runat="server">
                                                    <td>
                                                        <asp:Label Text="Psychosocial Impairment" ID="lblPsychosocialImpairmentNotes" runat="server" />
                                                        <wucTextBox:TextBox runat="server" ID="cmbPsychosocialNotes" textMode="MultiLine"
                                                            rows="1" width="910px" />
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;" id="lnDepressionNotes" runat="server">
                                                    <td>
                                                        <asp:Label Text="Depression" ID="lblDepressionNotes" runat="server" />
                                                        <wucTextBox:TextBox runat="server" ID="cmbDepressionNotes" textMode="MultiLine" rows="1"
                                                            width="910px" />
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;" id="lnConfirmedNotes" runat="server">
                                                    <td>
                                                        <asp:Label Text="Confirmed Mental Health Diagnosis" ID="lblConfirmedMentalHealthDiagnosisNotes"
                                                            runat="server" />
                                                        <wucTextBox:TextBox runat="server" ID="cmbConfirmedNotes" textMode="MultiLine" rows="1"
                                                            width="910px" />
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;" id="lnAlcoholNotes" runat="server">
                                                    <td>
                                                        <asp:Label Text="Alcohol Use" ID="lblAlcoholUseNotes" runat="server" />
                                                        <wucTextBox:TextBox runat="server" ID="cmbAlcoholNotes" textMode="MultiLine" rows="1"
                                                            width="910px" />
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;" id="lnTobaccoNotes" runat="server">
                                                    <td>
                                                        <asp:Label Text="Tobacco Use" ID="lblTobaccoUseNotes" runat="server" />
                                                        <wucTextBox:TextBox runat="server" ID="cmbTobaccoNotes" textMode="MultiLine" rows="1"
                                                            width="910px" />
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;" id="lnAbuseNotes" runat="server">
                                                    <td>
                                                        <asp:Label Text="Substance Abuse" ID="lblSubstanceAbuseNotes" runat="server" />
                                                        <wucTextBox:TextBox runat="server" ID="cmbAbuseNotes" textMode="MultiLine" rows="1"
                                                            width="910px" />
                                                        <br />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                                <div class="boxTop" id="hGeneralNotes" style="display: none;" runat="server">
                                    <div class="expandListTitle">
                                        <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                            <tbody>
                                                <tr style="vertical-align: middle;">
                                                    <td>
                                                        <b>GENERAL</b></td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                    <div class="addVisitDetails">
                                        <table>
                                            <tbody>
                                                <tr style="vertical-align: top;" id="lnStressUrinaryNotes" runat="server">
                                                    <td>
                                                        <asp:Label Text="Stress Urinary Incontinence" ID="lblStressUrinaryIncontinenceNotes"
                                                            runat="server" />
                                                        <wucTextBox:TextBox runat="server" ID="cmbStressUrinaryNotes" textMode="MultiLine"
                                                            rows="1" width="910px" />
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;" id="lnCerebriNotes" runat="server">
                                                    <td>
                                                        <asp:Label Text="Pseudotumor Cerebri" ID="lblCerebriNotes" runat="server" />
                                                        <wucTextBox:TextBox runat="server" ID="cmbCerebriNotes" textMode="MultiLine" rows="1"
                                                            width="910px" />
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;" id="lnHerniaNotes" runat="server">
                                                    <td>
                                                        <asp:Label Text="Abdominal Hernia" ID="lblAbdominalHerniaNotes" runat="server" />
                                                        <wucTextBox:TextBox runat="server" ID="cmbHerniaNotes" textMode="MultiLine" rows="1"
                                                            width="910px" />
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;" id="lnFunctionalNotes" runat="server">
                                                    <td>
                                                        <asp:Label Text="Functional Status" ID="lblFunctionalStatusNotes" runat="server" />
                                                        <wucTextBox:TextBox runat="server" ID="cmbFunctionalNotes" textMode="MultiLine" rows="1"
                                                            width="910px" />
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;" id="lnSkinNotes" runat="server">
                                                    <td>
                                                        <asp:Label Text="Abdominal Skin/Pannus" ID="lblAbdominalNotes" runat="server" />
                                                        <wucTextBox:TextBox runat="server" ID="cmbSkinNotes" textMode="MultiLine" rows="1"
                                                            width="910px" />
                                                        <br />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="LinkBtnSave_BoldComorbidityNotes" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:UpdatePanel runat="server" ID="upVitamins" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div id="div_vMedications" style="display: none">
                            <div class="leftColumn">
                                <div class="sectionBox1">
                                    <div class="boxTop">
                                        <h3 id="hMedicationVitaminsMinerals">
                                            MEDICATION / VITAMINS & MINERALS</h3>
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
                                    </div>
                                </div>
                                <div class="sectionBox3">
                                    <div class="boxTop">
                                        <h3 id="hMedicationNotes">
                                            Notes</h3>
                                        <table style="width: 400px" border="0">
                                            <tr>
                                                <td>
                                                    <wucTextBox:TextBox runat="server" ID="txtMedicationNotes" textMode="MultiLine" rows="5"
                                                        width="290px" />
                                                </td>
                                            </tr>
                                        </table>
                                        <div id="Div1">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <asp:HiddenField runat="Server" ID="IsViewPage7" Value="0" />
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="LinkBtnSave_Medication" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                <div class="clr">
                </div>
            </div>
            <asp:UpdatePanel runat="Server" ID="up_HiddenFields">
                <ContentTemplate>
                    <asp:HiddenField runat="server" ID="txtSelectedPageNo" Value="1" />
                    <asp:HiddenField runat="server" ID="txtHPageNo" Value="1" />
                    <asp:HiddenField runat="server" ID="txtPrevBariatricSurgery_Selected" />
                    <asp:HiddenField runat="server" ID="txtPrevNonBariatricSurgery_Selected" />
                    <asp:HiddenField runat="server" ID="txtAdverseEvents_Selected" />
                    <asp:HiddenField runat="server" ID="txtHBMI" Value="0" />
                    <asp:HiddenField runat="server" ID="txtHHeight" Value="0" />
                    <asp:HiddenField runat="server" ID="txtHStartWeight" Value="0" />
                    <asp:HiddenField runat="server" ID="txtHCurrentWeight" Value="0" />
                    <asp:HiddenField runat="server" ID="txtStartBMIWeight" Value="0" />
                    <asp:HiddenField runat="server" ID="txtBMIHeight" Value="0" />
                    <asp:DropDownList runat="server" ID="cmbReferredDoctorsList" />
                    <asp:HiddenField runat="server" ID="txtHApplicationURL" />
                    <asp:HiddenField runat="server" ID="txtHLapbandDate" Value="0" />
                    <asp:DropDownList runat="server" ID="cmbCity" />
                    <asp:DropDownList runat="server" ID="cmbInsurance" />
                    <asp:HiddenField runat="server" ID="txtUseImperial" Value="0" />
                    <asp:HiddenField runat="server" ID="txtHIdealWeight" Value="0" />
                    <asp:HiddenField runat="server" ID="txtHTargetWeight" Value="0" />
                    <asp:HiddenField runat="Server" ID="txtHRefBMI" Value="0" />
                    <asp:HiddenField runat="Server" ID="txtHCulture" />
                    <asp:HiddenField runat="Server" ID="TitleLoaded" Value="0" />
                    <asp:HiddenField runat="Server" ID="txtPatientRefSurname" />
                    <asp:HiddenField runat="Server" ID="txtPatientRefFirstname" />
                    <asp:HiddenField runat="Server" ID="txtPatientRefTitle" />
                    <asp:HiddenField runat="Server" ID="txtPatientRefUseFirst" />
                    <asp:HiddenField runat="Server" ID="txtPatientRefAddress1" />
                    <asp:HiddenField runat="Server" ID="txtPatientRefAddress2" />
                    <asp:HiddenField runat="Server" ID="txtPatientRefSuburb" />
                    <asp:HiddenField runat="Server" ID="txtPatientRefPostalCode" />
                    <asp:HiddenField runat="Server" ID="txtPatientRefState" />
                    <asp:HiddenField runat="Server" ID="txtPatientRefPhone" />
                    <asp:HiddenField runat="Server" ID="txtPatientRefFax" />
                    <asp:HiddenField runat="Server" ID="txtPatientRefDrID" />
                    <asp:HiddenField runat="Server" ID="txtHCurrentDate" Value="" />
                    <asp:HiddenField runat="Server" ID="txtHDelete" Value="0" />
                    <asp:HiddenField runat="server" ID="txtHDoctorBoldList" />
                    <asp:HiddenField runat="server" ID="txtHHospitalBoldList" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="linkBtnSave" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
