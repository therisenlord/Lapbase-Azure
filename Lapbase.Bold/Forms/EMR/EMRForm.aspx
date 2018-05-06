<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EMRForm.aspx.cs" Inherits="Forms_EMR_EMRForm"
    ValidateRequest="false" EnableViewState="true"%>

<%@ Register TagPrefix="wucSystemCode" TagName="SystemCodeList" Src="~/UserControl/SystemCodeWUCtrl.ascx" %>
<%@ Register TagPrefix="wucDoctor" TagName="DoctorList" Src="~/UserControl/DoctorsListWUCtrl.ascx" %>
<%@ Register TagPrefix="wucLogo" TagName="LogoList" Src="~/UserControl/LogoListWUCtrl.ascx" %>
<%@ Register TagPrefix="wucCode" TagName="CodeList" Src="~/UserControl/CodeWUCtrl.ascx" %>
<%@ Register TagPrefix="wucTextBox" TagName="TextBox" Src="~/UserControl/TextBoxWUCtrl.ascx" %>
<%@ Register TagPrefix="wucTextArea" TagName="TextArea" Src="~/UserControl/TextAreaWUCtrl.ascx" %>
<%@ Register TagPrefix="wucMenu" TagName="Menu" Src="~/UserControl/MenuWUCtrl.ascx" %>
<%@ Register TagPrefix="wucPatient" TagName="PatientTitle" Src="~/UserControl/PatientTitleDataWUCtrl.ascx" %>
<%@ Register TagPrefix="wucAppSchema" TagName="AppSchema" Src="~/UserControl/AppSchemaWUCtrl.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title>LapBase - A Data Manager for Bariatric Surgery</title>
    <!-- flooding div stylesheet-->
    <link rel="stylesheet" href="../../css/FloatingDIV/default.css" media="screen,projection"
        type="text/css" />
    <link rel="stylesheet" href="../../css/FloatingDIV/lightbox.css" media="screen,projection"
        type="text/css" />
    <link id="Link1" href="~/css/MultiColumn.css" rel="stylesheet" type="text/css" runat="server" />
    <link href='<%$ AppSettings:CssUrl%>' rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript" src="../../Scripts/Global.js"></script>

    <script language="javascript" type="text/javascript" src="Includes/EMR.js"></script>

    <script language="javascript" type="text/javascript" src="../RefferingDoctor/Includes/RefferingDoctor.js"></script>

    <!-- flooding div javascript-->

    <script type="text/javascript" src="../../scripts/FloatingDIV/prototype.js"></script>

    <script type="text/javascript" src="../../scripts/FloatingDIV/lightbox.js"></script>

    <!-- Calendar -->
    <link rel="stylesheet" href="../../css/Calendar/calendar.css" media="screen" />

    <script type="text/javascript" src="../../Scripts/Calendar/calendar.js"></script>

</head>
<body runat="server" id="bodyPatientData">
    <wucMenu:Menu runat="server" ID="mainMenu" />
    <div class="tabMenus" enableviewstate="false">
        <wucAppSchema:AppSchema runat="server" ID="AppSchemaMenu" currentItem="EMR" />
        <div class="manilaTabMenu" id="menurow1" runat="server">
            <ul>
                <li id="li_Div1"><a href="#this" onclick="javascript:controlBar_Buttons_OnClick(1);"
                    id="ub_mnuItem1">
                    <img id="Img12" src="~/img/tab_demographics.gif" alt="" height="16" width="16" runat="server" /><span
                        id="lblHDemographic">Demographic</span></a> </li>
                <li id="li_Div12"><a href="#this" onclick="javascript:controlBar_Buttons_OnClick(12);"
                    id="ub_mnuItem12">
                    <img id="Img11" src="~/img/tab_demographics.gif" alt="" height="16" width="16" runat="server" /><span
                        id="lblHMeasurement">Measurements</span></a> </li>
                <li id="li_Div3" runat="server"><a href="#this" onclick="javascript:controlBar_Buttons_OnClick(3);"
                    id="ub_mnuItem3">
                    <img id="Img8" src="~/img/tab_progress_notes.gif" alt="" height="16" width="16" runat="server" /><span
                        id="lblHPresentingComplaint">Presenting Complaint</span></a> </li>
                <li id="li_Div5"><a href="#this" onclick="javascript:controlBar_Buttons_OnClick(5);"
                    id="ub_mnuItem5">
                    <img id="Img14" src="~/img/tab_progress_notes.gif" alt="" height="16" width="16"
                        runat="server" /><span id="lblHComorbidities">Comorbidities</span></a> </li>
                <li id="li_Div9" runat="server"><a href="#this" onclick="javascript:controlBar_Buttons_OnClick(9);"
                    id="ub_mnuItem9">
                    <img id="Img4" src="~/img/tab_complications.gif" alt="" height="16" width="16" runat="server" /><span
                        id="lblMedications">Medications</span></a> </li>
                <li id="li_Div11" runat="server"><a href="#this" onclick="javascript:controlBar_Buttons_OnClick(11);"
                    id="ub_mnuItem11">
                    <img id="Img2" src="~/img/tab_progress_notes.gif" alt="" height="16" width="16" runat="server" /><span
                        id="lblHeightWeightNotes">Weight Loss History</span></a></li>
                <li id="li_Div2"><a href="#this" onclick="javascript:controlBar_Buttons_OnClick(2);"
                    id="ub_mnuItem2">
                    <img id="Img9" src="~/img/tab_progress_notes.gif" alt="" height="16" width="16" runat="server" /><span
                        id="lblHPastHistory">Past & Family History</span></a> </li>
            </ul>
        </div>
        <div class="manilaTabMenu" id="menurow2" runat="server">
            <ul>
                <li id="li_Div18" runat="server" style="display:none"><a href="#this" onclick="javascript:controlBar_Buttons_OnClick(18);"
                    id="ub_mnuItem18">
                    <img id="Img18" src="~/img/tab_progress_notes.gif" alt="" height="16" width="16"
                        runat="server" /><span id="lblHRegitryData">Registry Data</span></a> </li>
                <li id="li_Div10"><a href="#this" onclick="javascript:controlBar_Buttons_OnClick(10);"
                    id="ub_mnuItem10">
                    <img id="Img1" src="~/img/tab_progress_notes.gif" alt="" height="16" width="16" runat="server" /><span
                        id="lblAllergies">Allergies</span></a> </li>
                <li id="li_Div13"><a href="#this" onclick="javascript:controlBar_Buttons_OnClick(13);"
                    id="ub_mnuItem13">
                    <img id="Img16" src="~/img/tab_height_weight_notes.gif" alt="" height="16" width="16"
                        runat="server" /><span id="lblHPhysicalExamination">Physical Examination</span></a> </li>
                <li id="li_Div8"><a href="#this" onclick="javascript:controlBar_Buttons_OnClick(8);"
                    id="ub_mnuItem8">
                    <img id="Img10" src="~/img/tab_progress_notes.gif" alt="" height="16" width="16"
                        runat="server" /><span id="lblHLabs">Labs</span></a> </li>
                <li id="li_Div6"><a href="#this" onclick="javascript:controlBar_Buttons_OnClick(6);"
                    id="ub_mnuItem6">
                    <img id="Img15" src="~/img/tab_height_weight_notes.gif" alt="" height="16" width="16"
                        runat="server" /><span id="lblHInvestigation">Investigations & Referrals</span></a> </li>
                <li id="li_Div16"><a href="#this" onclick="javascript:controlBar_Buttons_OnClick(16);"
                    id="ub_mnuItem16">
                    <img id="Img5" src="~/img/tab_height_weight_notes.gif" alt="" height="16" width="16"
                        runat="server" /><span id="lblHManagementPlan">Management Plan</span></a> </li>
                <li id="li_Div17"><a href="#this" onclick="javascript:controlBar_Buttons_OnClick(17);"
                    id="ub_mnuItem17">
                    <img id="Img17" src="~/img/tab_reports.gif" alt="" height="16" width="16" runat="server" /><span
                        id="lblHMedicalReport">Medical Report</span></a> </li>
            </ul>
        </div>
    </div>
    <form id="frmPatientData" runat="server" autocomplete="off" enctype="multipart/form-data">
        <asp:ScriptManager runat="server" ID="scriptManager_Baseline" />
        <asp:UpdatePanel runat="server" ID="upLinkBtnSave">
            <ContentTemplate>
                <asp:LinkButton runat="server" ID="linkBtnSave" OnClick="linkBtnSave_OnClick" />
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
                        <div class="baselineDemographics" id="div_vDemographics"  style="display: none">
                            <div class="btnWrap" align="right">
                                <table class="header" id="tblDemographicsHeader">
                                    <td align="left">
                                        <input type="button" value="Delete" runat="server" id="btnDeletePatient1" style="width: 100px;"
                                            onclick="javascript:btnDelete_onclick();" onserverclick="btnDeletePatient_onserverclick" />
                                    </td>
                                    <td align="right">
                                        <input type="button" id="btnSavePatient1" runat="server" value="Save" style="width: 100px"
                                            onclick="javascript:controlBar_Buttons_OnClick(1);" />
                                    </td>
                                </table>
                            </div>
                            <div class="leftColumn">
                                <div class="personalDetails">
                                    <div class="boxTop">
                                        <h3 id="hPersonalDetails">
                                            Personal Details</h3>
                                        <table>
                                            <tr>
                                                <td valign="top" enableviewstate="false">
                                                    <table border="0">
                                                        <tbody>
                                                            <tr>
                                                                <td style="width: 30%;">
                                                                    <asp:Label runat="server" ID="lblPatientID" Text="Patient ID" /></td>
                                                                <td style="width: 70%;">
                                                                    <wucTextBox:TextBox runat="server" ID="txtPatient_CustomID" Text="" />
                                                                    <asp:HiddenField runat="server" ID="txtPatientID" />
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
                                                        </tbody>
                                                    </table>
                                                </td>
                                                <td valign="top">
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
                                                                            <%--<asp:AsyncPostBackTrigger ControlID = "linkBtnSave" EventName="Click" />
                                                                            <asp:AsyncPostBackTrigger ControlID="linkBtnCalculateAge" EventName="Click" />--%>
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
                                                                    <wucSystemCode:SystemCodeList runat="Server" ID="cmbRace" CriteriaString="RACE" Width="90" />
                                                                </td>
                                                            </tr>
                                                            <tr enableviewstate="false">
                                                                <td>
                                                                    <label id="lblOccupation">Occupation</label>
                                                                </td>
                                                                <td>
                                                                    <wucTextBox:TextBox runat="server" ID="txtOccupation" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label runat="server" ID="lblEmploymentStatus" Text="Employment status" /></td>
                                                                <td>
                                                                    <wucSystemCode:SystemCodeList runat="Server" ID="cmbEmployment" CriteriaString="EMP" />
                                                                </td>
                                                            </tr>
                                                            <tr enableviewstate="false">
                                                                <td style="width: 30%">
                                                                    <asp:Label Text="Consultation Date" runat="server" ID="lblConsultDate" /></td>
                                                                <td style="width: 60%">
                                                                    <wucTextBox:TextBox runat="server" ID="txtConsultationDate" maxLength="10" width="50%" />
                                                                    <a href="#this" id="a5" onclick="javascript:aCalendar_onclick(this,'txtConsultationDate');">
                                                                        [...]</a>&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr enableviewstate="false">
                                                                <td style="width: 30%">
                                                                    <asp:Label Text="Deceased Date" runat="server" ID="lblDeceasedDate" /></td>
                                                                <td style="width: 60%">
                                                                    <wucTextBox:TextBox runat="server" ID="txtDeceasedDate" maxLength="10" width="50%" />
                                                                    <a href="#this" id="a6" onclick="javascript:aCalendar_onclick(this,'txtDeceasedDate');">
                                                                        [...]</a>&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr enableviewstate="false">
                                                                <td colspan="2">
                                                                    <asp:Label Text="Death related to Bariatric procedure? " runat="server" ID="lblDeceasedPrimaryProcedure" /> &nbsp; &nbsp;
                                                                        <input type="radio" name="rbDeceasedPrimaryProcedure" id="rbDeceasedPrimaryProcedureY" value="Y" runat="server" onclick="checkDeceasedPrimaryProcedure();" />
                                                                        &nbsp;<asp:Label Text="Yes" runat="server" ID="lblEMRYes" /> &nbsp;
                                                                        <input type="radio" name="rbDeceasedPrimaryProcedure" id="rbDeceasedPrimaryProcedureN" value="N" runat="server" onclick="checkDeceasedPrimaryProcedure();"/>
                                                                        &nbsp;<asp:Label Text="No" runat="server" ID="lblEMRNo" /> &nbsp;</td>
                                                            </tr>
                                                            <tr enableviewstate="false" runat="server">
                                                                <td colspan="2">
                                                                    <div style="display:none" id="rowDeceasedNote" runat="server">
                                                                        <asp:Label Text="Note: " runat="server" /> <wucTextArea:TextArea runat="server" ID="txtDeceasedNote" width="90%" rows="2" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr><td>&nbsp;</td></tr>
                                                        </tbody>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr enableviewstate="false">
                                                <td colspan="2">
                                                    <table class="full">
                                                        <tbody>
                                                            <tr>
                                                                <td width="17%">
                                                                    <label id="lblFamilyStructure">Family Structure</label>
                                                                </td>
                                                                <td width="83%">
                                                                    <input type="radio" name="rbDemographicFamilyStructure" id="rbDemographicFamilyStructureM"
                                                                        value="M" runat="server" />
                                                                    &nbsp;<label id="lblFamilyMarried">Married</label> &nbsp; &nbsp;
                                                                    <input type="radio" name="rbDemographicFamilyStructure" id="rbDemographicFamilyStructureS"
                                                                        value="S" runat="server" />
                                                                    &nbsp;<label id="lblFamilySingle">Single</label> &nbsp; &nbsp;
                                                                    <input type="radio" name="rbDemographicFamilyStructure" id="rbDemographicFamilyStructureD"
                                                                        value="D" runat="server" />
                                                                    &nbsp;<label id="lblFamilyDivorced">Divorced</label> &nbsp; &nbsp;
                                                                    <input type="radio" name="rbDemographicFamilyStructure" id="rbDemographicFamilyStructureP"
                                                                        value="P" runat="server" />
                                                                    &nbsp;<label id="lblFamilyPartner">Partner / Relationship</label> &nbsp;
                                                                    <input type="radio" name="rbDemographicFamilyStructure" id="rbDemographicFamilyStructureSp"
                                                                        value="sp" runat="server" />
                                                                    &nbsp;<label id="lblFamilySeparated">Separated</label> &nbsp;
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr enableviewstate="false">
                                                <td>
                                                    <table>
                                                        <tbody>
                                                            <tr>
                                                                <td style="width: 30%">
                                                                    <label id="lblLivingAtHome">Living at Home</label>
                                                                </td>
                                                                <td style="width: 70%">
                                                                    <wucTextBox:TextBox runat="server" ID="txtLiveHome" />
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table>
                                                        <tbody>
                                                            <tr>
                                                                <td style="width: 40%">
                                                                    <label id="lblSpouse">Spouse / Partner Name</label>
                                                                </td>
                                                                <td style="width: 60%">
                                                                    <wucTextBox:TextBox runat="server" ID="txtPartnerName" />
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <table class="full">
                                                        <tbody>
                                                            <tr>
                                                                <td style="font-weight: bold">
                                                                    <label id="lblChildren">Children</label><input type="hidden" id="totalChildren" runat="server" value="0" /><input
                                                                        type="hidden" id="txtDetailChildren" runat="server" value="" />
                                                                    <input type="button" onclick="addChildren();" value="+" style="width: 20px" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <div id="childrenDiv" runat="server">
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <div class="boxBtm">
                                        </div>
                                    </div>
                                </div>
                                <div class="personalDetails" enableviewstate="false">
                                    <div class="boxTop">
                                        <h3 id="lblContactPerson">
                                            Contact Persons</h3>
                                        <table border="0" class="full">
                                            <tbody>
                                                <tr>
                                                    <td colspan="4" style="font-weight: bold">
                                                        <label id="lblNextofKin">Next Of Kin</label></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 17%">
                                                        <label id="lblNextofKinName">Name</label></td>
                                                    <td style="width: 33%">
                                                        <wucTextBox:TextBox runat="server" ID="txtNOKName" />
                                                    </td>
                                                    <td style="width: 21%">
                                                        <label id="lblNextofKinRelationship">Relationship</label></td>
                                                    <td style="width: 29%">
                                                        <wucTextBox:TextBox runat="server" ID="txtNOKRelationship" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label id="lblNextofKinAddress">Address</label></td>
                                                    <td colspan="3">
                                                        <input type="text" runat="server" id="txtNOKAddress" size="74" /></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label id="lblNextofKinHPhone">Phone (Home)</label></td>
                                                    <td>
                                                        <wucTextBox:TextBox runat="server" ID="txtNOKHome" />
                                                    </td>
                                                    <td>
                                                        <label id="lblNextofKinWPhone">Phone (Work)</label></td>
                                                    <td>
                                                        <wucTextBox:TextBox runat="server" ID="txtNOKWork" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label id="lblNextofKinMobile">Mobile</label></td>
                                                    <td>
                                                        <wucTextBox:TextBox runat="server" ID="txtNOKMobile" />
                                                    </td>
                                                    <td colspan="2">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" style="font-weight: bold">
                                                        <label id="lblAdditionalContact1">Additional Contact</label></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 17%">
                                                        <label id="lblAdditionalContact1Name">Name</label></td>
                                                    <td style="width: 33%">
                                                        <wucTextBox:TextBox runat="server" ID="txtAddC1Name" />
                                                    </td>
                                                    <td style="width: 21%">
                                                        <label id="lblAdditionalContact1Relationship">Relationship</label></td>
                                                    <td style="width: 29%">
                                                        <wucTextBox:TextBox runat="server" ID="txtAddC1Relationship" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label id="lblAdditionalContact1Address">Address</label></td>
                                                    <td colspan="3">
                                                        <input type="text" runat="server" id="txtAddC1Address" size="74" /></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label id="lblAdditionalContact1HPhone">Phone (Home)</label></td>
                                                    <td>
                                                        <wucTextBox:TextBox runat="server" ID="txtAddC1Home" />
                                                    </td>
                                                    <td>
                                                        <label id="lblAdditionalContact1Mobile">Mobile</label></td>
                                                    <td>
                                                        <wucTextBox:TextBox runat="server" ID="txtAddC1Mob" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" style="font-weight: bold">
                                                        <label id="lblAdditionalContact2">Additional Contactv</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 17%">
                                                        <label id="lblAdditionalContact2Name">Name</label></td>
                                                    <td style="width: 33%">
                                                        <wucTextBox:TextBox runat="server" ID="txtAddC2Name" />
                                                    </td>
                                                    <td style="width: 21%">
                                                        <label id="lblAdditionalContact2Relationship">Relationship</label></td>
                                                    <td style="width: 29%">
                                                        <wucTextBox:TextBox runat="server" ID="txtAddC2Relationship" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label id="lblAdditionalContact2Address">Address</label></td>
                                                    <td colspan="3">
                                                        <input type="text" runat="server" id="txtAddC2Address" size="74" /></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label id="lblAdditionalContact2HPhone">Phone (Home)</label></td>
                                                    <td>
                                                        <wucTextBox:TextBox runat="server" ID="txtAddC2Home" />
                                                    </td>
                                                    <td>
                                                        <label id="lblAdditionalContact2Mobile">Mobile</label></td>
                                                    <td>
                                                        <wucTextBox:TextBox runat="server" ID="txtAddC2Mob" />
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
                                        <h3 id="lblInsuranceInformation">
                                            Insurance Information</h3>
                                        <table border="0" enableviewstate="false">
                                            <tr>
                                                <td>
                                                    <asp:Label Text="Employer" runat="server" ID="lblEmployer" /></td>
                                                <td>
                                                    <wucTextBox:TextBox runat="server" ID="txtEmployer" maxLength="50" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label Text="Insurance" runat="server" ID="lblInsurance" /></td>
                                                <td>
                                                    <wucTextBox:TextBox runat="server" ID="txtInsurance" width="83%" />
                                                    <asp:HiddenField runat="server" ID="txtHInsurance" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label Text="Insurance Number" runat="server" ID="lblInsuranceNumber" /></td>
                                                <td>
                                                    <wucTextBox:TextBox runat="server" ID="txtInsuranceNumber" maxLength="50" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label Text="Medicare Number" runat="server" ID="lblMedicareNumber" /></td>
                                                <td>
                                                    <wucTextBox:TextBox runat="server" ID="txtMedicareNumber" maxLength="50" />
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
                                                    <input type="radio" name="rbHealthInsurance" id="rbHealthInsurance_Yes" value="1"
                                                        runat="server" />
                                                    <label id="lblHealthInsurance_Yes">
                                                        Yes</label>
                                                    &nbsp;
                                                    <input type="radio" name="rbHealthInsurance" id="rbHealthInsurance_No" value="0"
                                                        runat="server" />
                                                    <label id="lblHealthInsurance_No">
                                                        No</label>
                                                    &nbsp;
                                                    <input type="radio" name="rbHealthInsurance" id="rbHealthInsurance_Unknown" value="-1"
                                                        runat="server" checked />
                                                    <label id="lblHealthInsurance_UnKnown">
                                                        Not known</label>
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
                                        </table>
                                        <table border="0" enableviewstate="false">
                                            <tr>
                                                <td colspan="3" style="font-weight: bold">
                                                    <label id="lblPayment">Payment</label></td>
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
                                            <tr>
                                                <td>
                                                    <br />
                                                </td>
                                            </tr>
                                        </table>
                                        <table border="0">
                                            <tr enableviewstate="false">
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
                                            <tr enableviewstate="false">
                                                <td>
                                                    <input type="checkbox" id="chkDurationObesity" runat="server" />
                                                </td>
                                                <td>
                                                    <label id="lblDurationObesity">
                                                        Duration of obesity</label></td>
                                            </tr>
                                            <tr enableviewstate="false">
                                                <td>
                                                    <input type="checkbox" id="chkSmokingCessation" runat="server" />
                                                </td>
                                                <td>
                                                    <label id="lblSmokingCessation">
                                                        Smoking Cessation</label></td>
                                            </tr>
                                            <tr enableviewstate="false">
                                                <td>
                                                    <input type="checkbox" id="chkMentalHealthClearance" runat="server" />
                                                </td>
                                                <td>
                                                    <label id="lblMentalHealthClearance">
                                                        Mental Health Clearance</label></td>
                                            </tr>
                                            <tr enableviewstate="false">
                                                <td>
                                                    <input type="checkbox" id="chkIntelligenceTesting" runat="server" />
                                                </td>
                                                <td>
                                                    <label id="lblIntelligenceTesting">
                                                        Intelligence (IQ) Testing</label></td>
                                            </tr>
                                            <tr enableviewstate="false">
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
                                <div class="insuranceInformation" enableviewstate="false">
                                    <div class="boxTop">
                                        <h3 id="hReportSummary">
                                            Medical Report Summary Statement</h3>
                                        <table border="0" class="full">
                                            <tbody>
                                                <tr>
                                                    <td style="width: 90%">
                                                        <wucTextArea:TextArea runat="server" ID="txtMedicalSummary" width="99%" rows="5" />
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
                                <div class="addressDetails" enableviewstate="false">
                                    <div class="boxTop">
                                        <h3 id="hPhotoDetails">
                                            Additonal Information</h3>
                                            <table border="0">
                                                <tbody>
                                                    <tr>
                                                        <td align="center">
                                                            <div id="divPhoto" runat="server">
                                                                <asp:Image ID="detailsPhoto" runat="server" Width="250" />
                                                            </div>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div id="divUploadFile">
                                                                <asp:Label runat="server" ID="lblPhoto" Text="Photo Path: " />
                                                                <input type="file" id="uploadPhoto" runat="server" size="15" onchange="checkPhotoStatus();"/>
                                                                <a runat="server" id="changeFileUpload" onclick="changeFileInputField('divUploadFile')" href="javascript:noAction();">[Change]</a>
                                                                <a runat="server" id="clearFileUpload" style="display:none" onclick="clearFileInputField('divUploadFile')" href="javascript:noAction();">[Clear]</a>
                                                                <a runat="server" id="cancelFileUpload" style="display:none" onclick="cancelFileInputField('divUploadFile')" href="javascript:noAction();">[Cancel]</a>
                                                                
                                                                <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="linkBtnSave_OnClick" Visible="false"/>
                                                                <input type="hidden" id="tempPhotoStatus" runat="server" value="1" />
                                                                <input type="hidden" id="tempPhotoDisplay" runat="server" value="" />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        <div class="boxBtm">
                                        </div>
                                    </div>
                                </div>              
                                <div class="addressDetails" enableviewstate="false">
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
                                                <tr enableviewstate="false">
                                                    <td>
                                                        <asp:Label Text="Referred By" runat="server" ID="lblReferredBy" /></td>
                                                    <td>
                                                        <wucTextBox:TextBox runat="server" ID="txtReferredBy" width="79%"/>
                                                        <a id="aRefDr1" href="../RefferingDoctor/RefferingDoctorForm.aspx" class="lbOn">[New]</a>
                                                        <asp:HiddenField runat="server" ID="txtHReferredBy" />
                                                    </td>
                                                </tr>
                                                <tr enableviewstate="false">
                                                    <td>
                                                        <asp:Label Text="Referral Date" runat="server" ID="lblRefDate1" /></td>
                                                    <td>
                                                        <wucTextBox:TextBox runat="server" ID="txtRefDate1" maxLength="10" width="50%" />
                                                        <a href="#this" id="a2" onclick="javascript:aCalendar_onclick(this,'txtRefDate1');">
                                                            [...]</a>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label Text="Referral Duration" runat="server" ID="lblRefDuration1" /></td>
                                                    <td>
                                                        <wucSystemCode:SystemCodeList runat="server" ID="txtRefDuration1" CriteriaString="REFDUR"
                                                            Width="85" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label Text="Referral Status" runat="server" ID="lblRefStatus1" /></td>
                                                    <td>
                                                        <wucSystemCode:SystemCodeList runat="server" ID="txtRefStatus1" CriteriaString="REFSTS"
                                                            Width="85" />
                                                    </td>
                                                </tr>
                                                <tr enableviewstate="false">
                                                    <td>
                                                        <br />
                                                        <asp:Label runat="Server" ID="lblOtherDrs1" Text="Other Doctors" /></td>
                                                    <td>
                                                        <br />
                                                        <wucTextBox:TextBox runat="server" ID="txtOtherDoctors1" width="79%" />
                                                        <a id="aRefDr2" href="../RefferingDoctor/RefferingDoctorForm.aspx" class="lbOn">[New]</a>
                                                        <asp:HiddenField runat="server" ID="txtHOtherDoctors1" />
                                                    </td>
                                                </tr>
                                                <tr enableviewstate="false">
                                                    <td>
                                                        <asp:Label Text="Referral Date" runat="server" ID="lblRefDate2" /></td>
                                                    <td>
                                                        <wucTextBox:TextBox runat="server" ID="txtRefDate2" maxLength="10" width="50%" />
                                                        <a href="#this" id="a3" onclick="javascript:aCalendar_onclick(this,'txtRefDate2');">
                                                            [...]</a>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label Text="Referral Duration" runat="server" ID="lblRefDuration2" /></td>
                                                    <td>
                                                        <wucSystemCode:SystemCodeList runat="server" ID="txtRefDuration2" CriteriaString="REFDUR"
                                                            Width="85" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label Text="Referral Status" runat="server" ID="lblRefStatus2" /></td>
                                                    <td>
                                                        <wucSystemCode:SystemCodeList runat="server" ID="txtRefStatus2" CriteriaString="REFSTS"
                                                            Width="85" />
                                                    </td>
                                                </tr>
                                                <tr enableviewstate="false">
                                                    <td>
                                                        <br />
                                                        <asp:Label runat="Server" ID="lblOtherDrs2" Text="Other Doctors" /></td>
                                                    <td>
                                                        <br />
                                                        <wucTextBox:TextBox runat="server" ID="txtOtherDoctors2" width="79%" />
                                                        <a id="aRefDr3" href="../RefferingDoctor/RefferingDoctorForm.aspx" class="lbOn">[New]</a>
                                                        <asp:HiddenField runat="server" ID="txtHOtherDoctors2" />
                                                    </td>
                                                </tr>
                                                <tr enableviewstate="false">
                                                    <td>
                                                        <asp:Label Text="Referral Date" runat="server" ID="lblRefDate3" /></td>
                                                    <td>
                                                        <wucTextBox:TextBox runat="server" ID="txtRefDate3" maxLength="10" width="50%" />
                                                        <a href="#this" id="a4" onclick="javascript:aCalendar_onclick(this,'txtRefDate3');">
                                                            [...]</a>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label Text="Referral Duration" runat="server" ID="lblRefDuration3" /></td>
                                                    <td>
                                                        <wucSystemCode:SystemCodeList runat="server" ID="txtRefDuration3" CriteriaString="REFDUR"
                                                            Width="85" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label Text="Referral Status" runat="server" ID="lblRefStatus3" /></td>
                                                    <td>
                                                        <wucSystemCode:SystemCodeList runat="server" ID="txtRefStatus3" CriteriaString="REFSTS"
                                                            Width="85" />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <div class="boxBtm">
                                        </div>
                                    </div>
                                </div>
                                <div class="contactDetails" enableviewstate="false">
                                    <div class="boxTop">
                                        <h3 id="lblContactDetails">
                                            Contact Details</h3>
                                        <table border="0">
                                            <tbody>
                                                <tr>
                                                    <td style="width: 40%">
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
                                <div class="medicalDetails" enableviewstate="false">
                                    <div class="boxTop">
                                        <h3 id="lblRegistryDetails">
                                            Registry Details</h3>
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
                                                        <asp:Label runat="server" ID="lblChartNumber" Text="ACS Registry Number" /></td>
                                                    <td>
                                                        <wucTextBox:TextBox runat="server" ID="txtChartNumber" maxLength="20" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblBoldChartNumber" Text="Registry Chart Number" /></td>
                                                    <td>
                                                        <asp:Label runat="server" ID="txtBoldChartNumber" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label runat="server" ID="lblSend2SRC" Text="Patient Consent to Registry use of data?" />
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
                                <div class="medicalDetails" enableviewstate="false">
                                    <div class="boxTop">
                                        <h3 id="hSocialHistory">
                                            Social History</h3>
                                        <table border="0">
                                            <tbody>
                                                <tr>
                                                    <td style="width: 80%">
                                                        <wucTextArea:TextArea runat="server" ID="txtSocialHistory" width="100%" rows="9" />
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
                        <asp:PostBackTrigger ControlID="btnUpload" />
                        <asp:AsyncPostBackTrigger ControlID="linkBtnSave" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:UpdatePanel runat="server" ID="upBackground">
                    <ContentTemplate>
                        <div id="div_vBackground" class="bold" style="display: none">
                            <div class="btnWrap" align="right">
                                <table class="header" id="tblBackgroundHeader">
                                    <tr>
                                        <td align="left">
                                            <input type="button" value="Delete" runat="server" id="btnDeletePatient2" style="width: 100px;"
                                                onclick="javascript:btnDelete_onclick();" onserverclick="btnDeletePatient_onserverclick" />
                                        </td>
                                        <td align="right">
                                            <input type="button" id="btnSavePatient2" runat="server" value="Save" style="width: 100px"
                                                onclick="javascript:controlBar_Buttons_OnClick(2);" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="wideBox" enableviewstate="false">
                                <div class="boxTop">
                                    <h3 id="lblBackground">
                                        Background</h3>
                                    <table class="full">
                                        <tbody>
                                            <tr>
                                                <td colspan="7">
                                                    <label id="lblBackgroundFamilyHistory">Do you have a family history of any of the following:</label><br />
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr class="row01" style="font-weight: bold">
                                                <td style="width: 15%">
                                                    &nbsp;</td>
                                                <td align="center" style="width: 17%">
                                                    <label id="lblBackroundFather">Father</label></td>
                                                <td align="center" style="width: 17%">
                                                    <label id="lblBackgroundMother">Mother</label></td>
                                                <td align="center" style="width: 17%">
                                                    <label id="lblBackgroundChild">Sibling / Child</label></td>
                                                <td align="center" style="width: 17%">
                                                    <label id="lblBackgroundNoHistory">No Family History</label></td>
                                                <td align="center" style="width: 17%">
                                                    <label id="lblBackgroundDontKnow">Don't Know</label></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblBackgroundDiabetes">Diabetes</label></td>
                                                <td align="center">
                                                    <input type="checkbox" id="chkBackgroundDF" runat="server" /></td>
                                                <td align="center">
                                                    <input type="checkbox" id="chkBackgroundDM" runat="server" /></td>
                                                <td align="center">
                                                    <input type="checkbox" id="chkBackgroundDS" runat="server" /></td>
                                                <td align="center">
                                                    <input type="checkbox" id="chkBackgroundDN" runat="server" /></td>
                                                <td align="center">
                                                    <input type="checkbox" id="chkBackgroundDD" runat="server" /></td>
                                            </tr>
                                            <tr class="row01">
                                                <td>
                                                    <label id="lblBackgroundHeartDisease">Heart Disease</label></td>
                                                <td align="center">
                                                    <input type="checkbox" id="chkBackgroundHDF" runat="server" /></td>
                                                <td align="center">
                                                    <input type="checkbox" id="chkBackgroundHDM" runat="server" /></td>
                                                <td align="center">
                                                    <input type="checkbox" id="chkBackgroundHDS" runat="server" /></td>
                                                <td align="center">
                                                    <input type="checkbox" id="chkBackgroundHDN" runat="server" /></td>
                                                <td align="center">
                                                    <input type="checkbox" id="chkBackgroundHDD" runat="server" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblBackgroundHypertension">Hypertension</label></td>
                                                <td align="center">
                                                    <input type="checkbox" id="chkBackgroundHF" runat="server" /></td>
                                                <td align="center">
                                                    <input type="checkbox" id="chkBackgroundHM" runat="server" /></td>
                                                <td align="center">
                                                    <input type="checkbox" id="chkBackgroundHS" runat="server" /></td>
                                                <td align="center">
                                                    <input type="checkbox" id="chkBackgroundHN" runat="server" /></td>
                                                <td align="center">
                                                    <input type="checkbox" id="chkBackgroundHD" runat="server" /></td>
                                            </tr>
                                            <tr class="row01">
                                                <td>
                                                    <label id="lblBackgroundGout">Gout</label></td>
                                                <td align="center">
                                                    <input type="checkbox" id="chkBackgroundGF" runat="server" /></td>
                                                <td align="center">
                                                    <input type="checkbox" id="chkBackgroundGM" runat="server" /></td>
                                                <td align="center">
                                                    <input type="checkbox" id="chkBackgroundGS" runat="server" /></td>
                                                <td align="center">
                                                    <input type="checkbox" id="chkBackgroundGN" runat="server" /></td>
                                                <td align="center">
                                                    <input type="checkbox" id="chkBackgroundGD" runat="server" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblBackgroundObesity">Obesity</label></td>
                                                <td align="center">
                                                    <input type="checkbox" id="chkBackgroundOF" runat="server" /></td>
                                                <td align="center">
                                                    <input type="checkbox" id="chkBackgroundOM" runat="server" /></td>
                                                <td align="center">
                                                    <input type="checkbox" id="chkBackgroundOS" runat="server" /></td>
                                                <td align="center">
                                                    <input type="checkbox" id="chkBackgroundON" runat="server" /></td>
                                                <td align="center">
                                                    <input type="checkbox" id="chkBackgroundOD" runat="server" /></td>
                                            </tr>
                                            <tr class="row01">
                                                <td>
                                                    <label id="lblBackgroundSnoring">Snoring / sleep apnoea</label></td>
                                                <td align="center">
                                                    <input type="checkbox" id="chkBackgroundSF" runat="server" /></td>
                                                <td align="center">
                                                    <input type="checkbox" id="chkBackgroundSM" runat="server" /></td>
                                                <td align="center">
                                                    <input type="checkbox" id="chkBackgroundSS" runat="server" /></td>
                                                <td align="center">
                                                    <input type="checkbox" id="chkBackgroundSN" runat="server" /></td>
                                                <td align="center">
                                                    <input type="checkbox" id="chkBackgroundSD" runat="server" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblBackgroundAsthma">Asthma</label></td>
                                                <td align="center">
                                                    <input type="checkbox" id="chkBackgroundAF" runat="server" /></td>
                                                <td align="center">
                                                    <input type="checkbox" id="chkBackgroundAM" runat="server" /></td>
                                                <td align="center">
                                                    <input type="checkbox" id="chkBackgroundAS" runat="server" /></td>
                                                <td align="center">
                                                    <input type="checkbox" id="chkBackgroundAN" runat="server" /></td>
                                                <td align="center">
                                                    <input type="checkbox" id="chkBackgroundAD" runat="server" /></td>
                                            </tr>
                                            <tr class="row01">
                                                <td>
                                                    <label id="lblBackgroundCholesterol">High Cholesterol</label></td>
                                                <td align="center">
                                                    <input type="checkbox" id="chkBackgroundHCF" runat="server" /></td>
                                                <td align="center">
                                                    <input type="checkbox" id="chkBackgroundHCM" runat="server" /></td>
                                                <td align="center">
                                                    <input type="checkbox" id="chkBackgroundHCS" runat="server" /></td>
                                                <td align="center">
                                                    <input type="checkbox" id="chkBackgroundHCN" runat="server" /></td>
                                                <td align="center">
                                                    <input type="checkbox" id="chkBackgroundHCD" runat="server" /></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <table class="full">
                                        <tr>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label id="lblBackgroundFamily">Family, social, employment history</label> &nbsp;
                                                <wucTextBox:TextBox ID="txtBackgroundFamilyHistory" runat="server" textMode="MultiLine"
                                                    rows="2" width="900px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label id="lblBackgroundPastHistory">Past history re health</label> &nbsp;
                                                <wucTextBox:TextBox ID="txtBackgroundPastHealth" runat="server" textMode="MultiLine"
                                                    rows="2" width="900px" />
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                            </div>
                            <div class="wideBox" enableviewstate="false">
                                <div class="boxTop">
                                    <h3 id="hPreviousBariatricSurgeries">
                                        Previous Bariatric Surgeries</h3>
                                    <div class="addVisitDetails">
                                        <table border="0" style="width: 900px">
                                            <tr id="bariatricDefault" runat="server">
                                                <td>
                                                    <input type="button" onclick="addBariatric();" value="+" style="width: 20px; height: 20px" />
                                                    <input type="hidden" id="totalBariatric" runat="server" value="0" /><input type="hidden"
                                                        id="txtDetailBariatric" runat="server" value="" />
                                                    <div id="bariatricDiv" runat="server">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr id="bariatricBold" runat="server">
                                                <td>
                                                    <table>
                                                        <tr enableviewstate="false">
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
                                                        <tr enableviewstate="false">
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
                                                            <td rowspan="4">
                                                                <select id="listAdverseEvents" multiple="true" size="10" runat="server" style="width: 225px"
                                                                    ondblclick="javascript:BoldLists_dblclick('listAdverseEvents', 'listAdverseEvents_Selected', 'Add');" />
                                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbAdverseEvents" CriteriaString="ADEV"
                                                                    BoldData="ADEV" Display="false" />
                                                            </td>
                                                            <td rowspan="4" style="vertical-align: middle; text-align: center">
                                                                <a href="#this" onclick="javascript:BoldLButtonLinks_click('listAdverseEvents', 'listAdverseEvents_Selected', 'Add', false);">
                                                                    ></a><br />
                                                                <a href="#this" onclick="javascript:BoldLButtonLinks_click('listAdverseEvents', 'listAdverseEvents_Selected', 'Add', true);">
                                                                    >></a><br />
                                                                <a href="#this" onclick="javascript:BoldLButtonLinks_click('listAdverseEvents_Selected', 'listAdverseEvents', 'Remove', false);">
                                                                    <</a><br />
                                                                <a href="#this" onclick="javascript:BoldLButtonLinks_click('listAdverseEvents_Selected', 'listAdverseEvents', 'Remove', true);">
                                                                    <<</a><br />
                                                            </td>
                                                            <td rowspan="4">
                                                                <select id="listAdverseEvents_Selected" multiple="true" size="10" runat="server"
                                                                    style="width: 225px" ondblclick="javascript:BoldLists_dblclick('listAdverseEvents_Selected', 'listAdverseEvents', 'Add');" />
                                                            </td>
                                                        </tr>
                                                        <tr enableviewstate="false">
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
                                                        <tr enableviewstate="false">
                                                            <td>
                                                                <label id="lblPBSSurgeonID">
                                                                    Surgeon</label></td>
                                                            <td>
                                                                <wucDoctor:DoctorList runat="server" ID="cmbSurgeon" Width="75" IsSurgeon="True" />
                                                            </td>
                                                        </tr>
                                                        <tr enableviewstate="false">
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
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                            </div>
                            <div class="wideBox" style="display: none" enableviewstate="false">
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
                            <div class="wideBox" enableviewstate="false">
                                <div class="boxTop">
                                    <h3 id="lblRelatedNonBariatricSurgeries">
                                        Relevant Non-Bariatric Surgeries</h3>
                                    <div class="addVisitDetails" style="width: 900px">
                                        <table border="0" class="nonBariatric">
                                            <tr id="nonbariatricBold" runat="server">
                                                <td>
                                                    <table>
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
                                                </td>
                                            </tr>
                                            
                                            <tr id="nonbariatricDefault" runat="server">
                                                <td>
                                                    <input type="hidden" id="totalNonBariatric" runat="server" value="0" /><input type="hidden"
                                                        id="txtDetailNonBariatric" runat="server" value="" />
                                                    <input type="button" onclick="addNonBariatric();" value="+" style="width: 20px; height: 20px" />
                                                    <div id="nonBariatricDiv" runat="server">
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="linkBtnSave" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                <div id="div_vPathology" class="bold" style="display: none" enableviewstate="false">
                    <div class="btnWrap" align="right">
                        <table class="header">
                            <tr>
                                <td align="left">
                                    <input type="button" value="Delete" runat="server" id="btnDeletePatient8" style="width: 100px;"
                                        onclick="javascript:btnDelete_onclick();" onserverclick="btnDeletePatient_onserverclick" />
                                </td>
                                <td align="right">
                                    <input type="button" id="btnSavePatient8" runat="server" value="Save" style="width: 100px"
                                        onserverclick="linkBtnSave_OnClick" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="wideBox">
                        <div class="boxTop">
                            <h3 id="lblPathology">
                                Pathology</h3>
                            <div class="addVisitDetails">
                                <table class="full">
                                    <tr >
                                        <td>
                                            <asp:Label runat="server" ID="lblDocumentFile" Text="Path of Pathology File to be imported: " />
                                            <input type="file" id="txtFile" runat="server" />
                                        </td>
                                    </tr>
                                    <tr style="display:none">
                                        <td>
                                            <div id="pathologyResultTable" runat="server">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label id="lblLabNotes">Notes</label>
                                            <wucTextBox:TextBox ID="txtLabNotes" runat="server" textMode="MultiLine" rows="3"
                                                width="900px" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="boxBtm">
                        </div>
                    </div>
                </div>
                <asp:UpdatePanel runat="server" ID="upMedications" EnableViewState="false">
                    <ContentTemplate>
                        <div id="div_vMedications" class="bold" style="display: none">
                            <div class="btnWrap" align="right">
                                <table class="header">
                                    <tr>
                                        <td align="left">
                                            <input type="button" value="Delete" runat="server" id="btnDeletePatient9" style="width: 100px;"
                                                onclick="javascript:btnDelete_onclick();" onserverclick="btnDeletePatient_onserverclick" />
                                        </td>
                                        <td align="right">
                                            <input type="button" id="btnSavePatient9" runat="server" value="Save" style="width: 100px"
                                                onclick="javascript:controlBar_Buttons_OnClick(9);" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="wideBox">
                                <div class="boxTop">
                                    <h3 id="lblMedicationsTitle">
                                        Medications</h3>
                                    <div class="addVisitDetails">
                                        <div>
                                            <input type="hidden" id="totalMedications" runat="server" value="0" /><input type="hidden"
                                                id="txtDetailMedications" runat="server" value="" /></h3>
                                        </div>
                                        <div runat="server">
                                            <table class="full">
                                                <tr class="row01" style="font-weight: bold">
                                                    <td style="width: 45%">
                                                        <label id="lblMedication">Medication</label></td>
                                                    <td align="center" style="width: 25%">
                                                        <label id="lblMedicationDosage">Dosage</label></td>
                                                    <td align="center" style="width: 25%">
                                                        <label id="lblMedicationFrequency">Frequency</label></td>
                                                    <td align="center" style="width: 5%">
                                                        <input type="button" onclick="addMedications();" value="+" style="width: 20px; height: 20px" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="medicationDiv" runat="server">
                                        </div>
                                    </div>
                                </div>
                                <div class="boxBtm">
                                </div>
                            </div>
                        </div>
                        <asp:LinkButton runat="server" ID="linkBtnSaveMedication" OnClick="linkBtnSave_OnClick" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="linkBtnSaveMedication" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:UpdatePanel runat="server" ID="upComplaint">
                    <ContentTemplate>
                        <div id="div_vComplaint" class="bold" style="display: none">
                            <div class="btnWrap" align="right">
                                <table class="header">
                                    <tr>
                                        <td align="left">
                                            <input type="button" value="Delete" runat="server" id="btnDeletePatient3" style="width: 100px;"
                                                onclick="javascript:btnDelete_onclick();" onserverclick="btnDeletePatient_onserverclick" />
                                        </td>
                                        <td align="right">
                                            <input type="button" id="btnSavePatient3" runat="server" value="Save" style="width: 100px"
                                                onclick="javascript:controlBar_Buttons_OnClick(3);" /></td>
                                    </tr>
                                </table>
                            </div>
                            <div class="wideBox">
                                <div class="boxTop">
                                    <h3 id="lblPresentingComplaint">
                                        Presenting Complaint</h3>
                                    <div class="addVisitDetails">
                                        <table class="full">
                                            <tr>
                                                <td>
                                                    <label id="lblComplaint">Complaint</label> &nbsp;
                                                    <wucSystemCode:SystemCodeList Width="30" runat="server" FirstRow="false" ID="cmbComplaint"
                                                        CriteriaString="CCOM" />
                                                    <br />
                                                    <br />
                                                    <wucTextBox:TextBox ID="txtComplaint" runat="server" textMode="MultiLine" rows="8"
                                                        width="900px" EnableViewState="false"/>
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
                        <asp:AsyncPostBackTrigger ControlID="linkBtnSave" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:UpdatePanel runat="server" ID="upComorbidity" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div id="div_vComorbidity" style="display: none" class="bold">
                            <div class="btnWrap" align="right">
                                <table class="header">
                                    <tr>
                                        <td align="left">
                                            <input type="button" value="Delete" runat="server" id="btnDeletePatient5" style="width: 100px;"
                                                onclick="javascript:btnDelete_onclick();" onserverclick="btnDeletePatient_onserverclick" /></td>
                                        <td align="right">
                                            <input type="button" id="btnSavePatient5" runat="server" value="Mark everything not set as no history or symptoms"
                                                onclick="javascript:btnComorbiditySetDefault_onclick();" />
                                            <input type="button" id="Button21" runat="server" value="Save" style="width: 100px"
                                                onclick="javascript:controlBar_Buttons_OnClick(5);" /></td>
                                    </tr>
                                </table>
                            </div>
                            <div class="wideBox" id="comorbidityBold" runat="server">
                                <div class="boxTop">
                                    <h3 id="hCardiovascularDisease">
                                        CARDIOVASCULAR DISEASE</h3>
                                    <table border="0">
                                        <tr>
                                            <td style="width: 25%">
                                                <label id="lblHypertension">
                                                    Hypertension</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbHypertension" CriteriaString="HYPER"
                                                    BoldData="CVS" SCode="PRE" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbHypertension" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
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
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbCongestive" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
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
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbIschemic" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
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
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbAngina" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
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
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbPeripheral" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
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
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbLower" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
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
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbDVT" runat="server" textMode="MultiLine" rows="2" width="650px" />
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                                <div class="boxTop">
                                    <h3 id="hMetabolic">
                                        METABOLIC</h3>
                                    <table border="0">
                                        <tr>
                                            <td style="width: 25%">
                                                <label id="lblGlucoseMetabolism">
                                                    Diabetes</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbGlucose" CriteriaString="GLUC"
                                                    BoldData="MET" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbGlucose" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
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
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbLipids" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
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
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbGout" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                                <div class="boxTop">
                                    <h3 id="hGastroIntestinal">
                                        GASTROINTESTINAL</h3>
                                    <table>
                                        <tr>
                                            <td style="width: 25%">
                                                <label id="lblGerd">
                                                    GERD</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbGred" CriteriaString="GERD" BoldData="GAS"
                                                    SCode="PRE" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbGred" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
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
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbCholelithiasis" runat="server" textMode="MultiLine"
                                                    rows="2" width="650px" />
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
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbLiver" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                                <div class="boxTop">
                                    <h3 id="hMusculoskeletal">
                                        MUSCULOSKELETAL</h3>
                                    <table border="0">
                                        <tr>
                                            <td style="width: 25%">
                                                <label id="lblBackPain">
                                                    Back Pain</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbBackPain" CriteriaString="BACK"
                                                    BoldData="MUS" SCode="PRE" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbBackPain" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
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
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbMusculoskeletal" runat="server" textMode="MultiLine"
                                                    rows="2" width="650px" />
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
                                            <tr enableviewstate="false">
                                                <td>
                                                    &nbsp;</td>
                                                <td>
                                                    <wucTextBox:TextBox ID="txtcmbFibro" runat="server" textMode="MultiLine" rows="2"
                                                        width="650px" />
                                                </td>
                                            </tr>
                                        </tr>
                                    </table>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                                <div class="boxTop">
                                    <h3 id="hPsychosocial">
                                        PSYCHOSOCIAL</h3>
                                    <table border="0">
                                        <tr>
                                            <td style="width: 25%">
                                                <label id="lblPsychosocialImpairment">
                                                    Psychosocial Impairment</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbPsychosocial" CriteriaString="PSYC"
                                                    BoldData="PSY" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbPsychosocial" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
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
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbDepression" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
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
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbConfirmed" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
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
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbAlcohol" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
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
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbTobacco" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
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
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbAbuse" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                                <div class="boxTop">
                                    <h3 id="hGeneral">
                                        GENERAL</h3>
                                    <table border="0">
                                        <tr>
                                            <td style="width: 25%">
                                                <label id="lblStressUrinaryIncontinence">
                                                    Stress Urinary Incontinence</label></td>
                                            <td style="width: 75%">
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbStressUrinary" CriteriaString="STRE"
                                                    BoldData="GEN" SCode="PRE" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbStressUrinary" runat="server" textMode="MultiLine"
                                                    rows="2" width="650px" />
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
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbCerebri" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
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
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbHernia" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
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
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbFunctional" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
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
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbSkin" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label id="lblRenalInsuff">
                                                    Renal Insufficiency / Creatinine > 2</label></td>
                                            <td>
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbRenalInsuff" CriteriaString="RENI"
                                                    BoldData="GEN" SCode="PRE" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbRenalInsuff" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label id="lblRenalFail">
                                                    Renal Failure Requiring Dialysis</label></td>
                                            <td>
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbRenalFail" CriteriaString="RENF"
                                                    BoldData="GEN" SCode="PRE" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbRenalFail" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label id="lblSteroid">
                                                    Chronic Steroid Use and or Immunosuppresant use</label></td>
                                            <td>
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbSteroid" CriteriaString="CHRO"
                                                    BoldData="GEN" SCode="PRE" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbSteroid" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label id="lblTherapeutic">
                                                    Therapeutic Anticoagulation</label></td>
                                            <td>
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbTherapeutic" CriteriaString="THER"
                                                    BoldData="GEN" SCode="PRE" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbTherapeutic" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label id="lblPrevPCISurgery">
                                                    Previous PCI and Previous Cardiac Surgery</label></td>
                                            <td>
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbPrevPCISurgery" CriteriaString="PREV"
                                                    BoldData="GEN" SCode="PRE" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbPrevPCISurgery" runat="server" textMode="MultiLine"
                                                    rows="2" width="650px" />
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                                <div class="boxTop">
                                    <h3 id="hPulmonary">
                                        PULMONARY</h3>
                                    <table border="0">
                                        <tr>
                                            <td style="width: 25%">
                                                <label id="lblObstructiveSleepApneaSyndrome">
                                                    Obstructive Sleep Apnea</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbObstructive" CriteriaString="OBST"
                                                    BoldData="PUL" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbObstructive" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
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
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbObesity" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
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
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbPulmonary" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
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
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbAsthma" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                                <div class="boxTop">
                                    <h3 id="hReproductive">
                                        REPRODUCTIVE</h3>
                                    <table border="0">
                                        <tr>
                                            <td style="width: 25%">
                                                <label id="lblPolycysticOverianSyndrome">
                                                    Polycystic Ovary Syndrome</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbPolycystic" CriteriaString="POLY"
                                                    BoldData="FEM" SCode="PRE" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbPolycystic" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
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
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbMenstrual" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                                <div class="boxTop" enableviewstate="false">
                                    <h3 id="hOther">
                                        Other Relevant Diagnostic
                                        <input type="button" onclick="addComorbidities();" value="+" style="width: 20px;
                                            height: 20px" /><input type="hidden" id="totalComorbidities" runat="server" value="0" /><input
                                                type="hidden" id="txtDetailComorbidities" runat="server" value="" /></h3>
                                    <div id="otherComorbiditiesDiv" runat="server">
                                    </div>
                                    <div>
                                        <table>
                                            <tr>
                                                <td>
                                                    <br />
                                                    <br />
                                                    <input type="checkbox" id="chkComorbidityReview" runat="server" />
                                                    &nbsp; On review of systems, no other significant health problems were identified.</td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                                <asp:HiddenField runat="Server" ID="IsViewPage6" Value="0" />
                            </div>
                            <div class="wideBox" id="comorbidityACS" runat="server">
                                <div class="boxTop">
                                    <h3 id="hPulmonaryACS">
                                        PULMONARY</h3>
                                    <table border="0">
                                        <tr>
                                            <td style="width: 25%">
                                                <label id="lblSmokerACS">
                                                    Current Smoker within 1 year</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbSmokerACS" CriteriaString="SMOKEACS"
                                                    BoldData="PULACS" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbSmokerACS" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%">
                                                <label id="lblOxygenACS">
                                                    Oxygen Dependent</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbOxygenACS" CriteriaString="OXYACS"
                                                    BoldData="PULACS" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbOxygenACS" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%">
                                                <label id="lblEmbolismACS">
                                                    History of Pulmonary Embolism</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbEmbolismACS" CriteriaString="EMBOACS"
                                                    BoldData="PULACS" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbEmbolismACS" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%">
                                                <label id="lblCopdACS">
                                                    History of Severe COPD</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbCopdACS" CriteriaString="COPDACS"
                                                    BoldData="PULACS" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbCopdACS" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%">
                                                <label id="lblCpapACS">
                                                    Obstructive Sleep Apnea req. CPAP or BiPAP</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbCpapACS" CriteriaString="CPAPACS"
                                                    BoldData="PULACS" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbCpapACS" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%">
                                                <label id="lblShoACS">
                                                    Shortness of Breath with Exertion</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbShoACS" CriteriaString="SHOACS"
                                                    BoldData="PULACS" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbShoACS" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                                <div class="boxTop">
                                    <h3 id="hGastroACS">
                                        GASTROINTESTINAL</h3>
                                    <table border="0">
                                        <tr>
                                            <td style="width: 25%">
                                                <label id="lblGerdACS">
                                                    GERD req. medications</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbGerdACS" CriteriaString="GERDACS"
                                                    BoldData="GASACS" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbGerdACS" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%">
                                                <label id="lblGallstoneACS">
                                                    Gallstone Disease</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbGallstoneACS" CriteriaString="GALACS"
                                                    BoldData="GASACS" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbGallstoneACS" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                                <div class="boxTop">
                                    <h3 id="hMusculoACS">
                                        MUSCULOSKELETAL</h3>
                                    <table border="0">
                                        <tr>
                                            <td style="width: 25%">
                                                <label id="lblMusculoDiseaseACS">
                                                    Musculoskeletal Disease</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbMusculoDiseaseACS" CriteriaString="MUSCDACS"
                                                    BoldData="MUSCACS" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbMusculoDiseaseACS" runat="server" textMode="MultiLine"
                                                    rows="2" width="650px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%">
                                                <label id="lblActivityLimitedACS">
                                                    Activity limited by pain</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbActivityLimitedACS" CriteriaString="PAINACS"
                                                    BoldData="MUSCACS" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbActivityLimitedACS" runat="server" textMode="MultiLine"
                                                    rows="2" width="650px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%">
                                                <label id="lblDailyMedACS">
                                                    Requires daily medication</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbDailyMedACS" CriteriaString="MEDSACS"
                                                    BoldData="MUSCACS" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbDailyMedACS" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%">
                                                <label id="lblSurgicalACS">
                                                    Surgical intervention planned or performed</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbSurgicalACS" CriteriaString="SURGACS"
                                                    BoldData="MUSCACS" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbSurgicalACS" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%">
                                                <label id="lblMobilityACS">
                                                    Uses mobility device</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbMobilityACS" CriteriaString="MOBACS"
                                                    BoldData="MUSCACS" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbMobilityACS" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                                <div class="boxTop">
                                    <h3 id="hRenalACS">
                                        RENAL</h3>
                                    <table border="0">
                                        <tr>
                                            <td style="width: 25%">
                                                <label id="lblRenalInsuffACS">
                                                    Renal Insufficiency (Creat >2)</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbRenalInsuffACS" CriteriaString="RENI"
                                                    BoldData="GEN" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbRenalInsuffACS" runat="server" textMode="MultiLine"
                                                    rows="2" width="650px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%">
                                                <label id="lblRenalFailACS">
                                                    Renal Failure req. dialysis</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbRenalFailACS" CriteriaString="RENF"
                                                    BoldData="GEN" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbRenalFailACS" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%">
                                                <label id="lblUrinaryACS">
                                                    Urinary Stress Incontinence</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbUrinaryACS" CriteriaString="URIACS"
                                                    BoldData="RENACS" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbUrinaryACS" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                                <div class="boxTop">
                                    <h3 id="hCardiacACS">
                                        CARDIAC</h3>
                                    <table border="0">
                                        <tr>
                                            <td style="width: 25%">
                                                <label id="lblMyocardinalACS">
                                                    History of Myocardinal Infarction</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbMyocardinalACS" CriteriaString="MYOACS"
                                                    BoldData="CARDACS" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbMyocardinalACS" runat="server" textMode="MultiLine"
                                                    rows="2" width="650px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%">
                                                <label id="lblPrevPCIACS">
                                                    Previous PCI</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbPrevPCIACS" CriteriaString="PCIACS"
                                                    BoldData="CARDACS" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbPrevPCIACS" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%">
                                                <label id="lblPrevCardiacACS">
                                                    Previous Cardiac Surgery</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbPrevCardiacACS" CriteriaString="CSURGACS"
                                                    BoldData="CARDACS" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbPrevCardiacACS" runat="server" textMode="MultiLine"
                                                    rows="2" width="650px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%">
                                                <label id="lblHyperlipidemiaACS">
                                                    Hyperlipidemia req. medications</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbHyperlipidemiaACS" CriteriaString="LIPIDACS"
                                                    BoldData="CARDACS" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbHyperlipidemiaACS" runat="server" textMode="MultiLine"
                                                    rows="2" width="650px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%">
                                                <label id="lblHypertensionACS">
                                                    Hypertension req. medications</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbHypertensionACS" CriteriaString="HYPERACS"
                                                    BoldData="CARDACS" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                # of antihypertensive meds
                                            </td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbHypertensionACS" runat="server" textMode="MultiLine"
                                                    rows="2" width="650px" />
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                                <div class="boxTop">
                                    <h3 id="hVascularACS">
                                        VASCULAR</h3>
                                    <table border="0">
                                        <tr>
                                            <td style="width: 25%">
                                                <label id="lblDVTACS">
                                                    History of DVT requiring therapy</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbDVTACS" CriteriaString="DVTACS"
                                                    BoldData="VASCACS" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbDVTACS" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%">
                                                <label id="lblVenousACS">
                                                    Venous Stasis</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbVenousACS" CriteriaString="VENOUSACS"
                                                    BoldData="VASCACS" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbVenousACS" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                                <div class="boxTop">
                                    <h3 id="hOtherACS">
                                        OTHER</h3>
                                    <table border="0">
                                        <tr>
                                            <td style="width: 25%">
                                                <label id="lblHealthStatusACS">
                                                    Functional Health Status Prior to Surgery</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbHealthStatusACS" CriteriaString="HEALTHACS"
                                                    BoldData="OTHERACS" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbHealthStatusACS" runat="server" textMode="MultiLine"
                                                    rows="2" width="650px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%">
                                                <label id="lblDiabetesACS">
                                                    Diabetes Mellitus</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbDiabetesACS" CriteriaString="DIABACS"
                                                    BoldData="OTHERACS" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbDiabetesACS" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%">
                                                <label id="lblSteroidsACS">
                                                    Chronic Steroids/ Immunosuppression</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbSteroidsACS" CriteriaString="CHRO"
                                                    BoldData="GEN" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbSteroidsACS" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%">
                                                <label id="lblAnticogulationACS">
                                                    Therapeutic anticogulation</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbAnticogulationACS" CriteriaString="THER"
                                                    BoldData="GEN" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbAnticogulationACS" runat="server" textMode="MultiLine"
                                                    rows="2" width="650px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%">
                                                <label id="lblObesityACS">
                                                    Previous obesity/ foregut surgery</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbObesityACS" CriteriaString="OBESEACS"
                                                    BoldData="OTHERACS" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbObesityACS" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                                <div class="boxTop">
                                    <h3 id="hNotesACS">
                                        General</h3>
                                    <table border="0">
                                        <tr>
                                            <td style="width: 25%">
                                                <label id="lblFatACS">
                                                    Fatigue</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucSystemCode:SystemCodeList runat="server" ID="cmbFatACS" CriteriaString="FATACS"
                                                    BoldData="GENACS" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtcmbFatACS" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
                                            </td>
                                        </tr>
                                        <tr enableviewstate="false">
                                            <td style="width: 25%" rowspan="2">
                                                <label id="Label3">
                                                    Notes</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucTextBox:TextBox ID="txtcmbNotesACS" runat="server" textMode="MultiLine" rows="2"
                                                    width="650px" />
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                            </div>
                            <div class="wideBox" id="comorbidityDefault" runat="server" enableviewstate="false">
                                <div class="boxTop">
                                    <h3 id="hEndDefault">
                                        ENDOCRINE</h3>
                                    <table border="0">
                                        <tr>
                                            <td style="width: 25%">
                                                <input type="checkbox" id="chkDiabDef" runat="server" />&nbsp;
                                                <label id="lblDiabDef">
                                                    Diabetes</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucTextBox:TextBox ID="txtDiabDef" runat="server" width="650px" rows="3" textMode="MultiLine"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="checkbox" id="chkThyDef" runat="server" />&nbsp;
                                                <label id="lblThyDef">
                                                    Thyroid disease</label>
                                            </td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtThyDef" runat="server" width="650px" rows="3" textMode="MultiLine"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <input type="checkbox" id="chkEndOtherDef" runat="server" />&nbsp;
                                                <label id="lblEndOtherDef">
                                                    Other endocrine disorders</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <wucTextBox:TextBox ID="txtEndOtherNameDef" runat="server" width="150px" />
                                            </td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtEndOtherDescDef" runat="server" width="650px" rows="3" textMode="MultiLine"/>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                                <div class="boxTop">
                                    <h3 id="hPulDefault">
                                        PULMONARY</h3>
                                    <table border="0">
                                        <tr>
                                            <td style="width: 25%">
                                                <input type="checkbox" id="chkAsthmaDef" runat="server" />&nbsp;
                                                <label id="lblAsthmaDef">
                                                    Asthma</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucTextBox:TextBox ID="txtAsthmaDef" runat="server" width="650px" rows="3" textMode="MultiLine"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="checkbox" id="chkApneaDef" runat="server" />&nbsp;
                                                <label id="lblApneaDef">
                                                    Sleep Apnea</label>
                                            </td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtApneaDef" runat="server" width="650px" rows="3" textMode="MultiLine"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="checkbox" id="chkEmbDef" runat="server" />&nbsp;
                                                <label id="lblEmbDef">
                                                    Pulmonary emboli</label>
                                            </td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtEmbDef" runat="server" width="650px" rows="3" textMode="MultiLine"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <input type="checkbox" id="chkPulOtherDef" runat="server" />&nbsp;
                                                <label id="lblPulOtherDef">
                                                    Other lung disease</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <wucTextBox:TextBox ID="txtPulOtherNameDef" runat="server" width="150px" />
                                            </td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtPulOtherDescDef" runat="server" width="650px" rows="3" textMode="MultiLine"/>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                                <div class="boxTop">
                                    <h3 id="hGasDefault">
                                        GASTROINTESTINAL</h3>
                                    <table border="0">
                                        <tr>
                                            <td style="width: 25%">
                                                <input type="checkbox" id="chkRefDef" runat="server" />&nbsp;
                                                <label id="lblRefDef">
                                                    Heartburn or reflux</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucTextBox:TextBox ID="txtRefDef" runat="server" width="650px" rows="3" textMode="MultiLine"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="checkbox" id="chkUlcDef" runat="server" />&nbsp;
                                                <label id="lblUlcDef">
                                                    Peptic ulcer</label>
                                            </td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtUlcDef" runat="server" width="650px" rows="3" textMode="MultiLine"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="checkbox" id="chkGallDef" runat="server" />&nbsp;
                                                <label id="lblGallDef">
                                                    Gallstones</label>
                                            </td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtGallDef" runat="server" width="650px" rows="3" textMode="MultiLine"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="checkbox" id="chkHepDef" runat="server" />&nbsp;
                                                <label id="lblHepDef">
                                                    Hepatitis</label>
                                            </td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtHepDef" runat="server" width="650px" rows="3" textMode="MultiLine"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <input type="checkbox" id="chkGasOtherDef" runat="server" />&nbsp;
                                                <label id="lblGasOtherDef">
                                                    Other GI or liver disease</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <wucTextBox:TextBox ID="txtGasOtherNameDef" runat="server" width="150px" />
                                            </td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtGasOtherDescDef" runat="server" width="650px" rows="3" textMode="MultiLine"/>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                                <div class="boxTop">
                                    <h3 id="hCvsDefault">
                                        CARDIOVASCULAR</h3>
                                    <table border="0">
                                        <tr>
                                            <td style="width: 25%">
                                                <input type="checkbox" id="chkIscDef" runat="server" />&nbsp;
                                                <label id="lblIscDef">
                                                    Ischaemic heart disease</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucTextBox:TextBox ID="txtIscDef" runat="server" width="650px" rows="3" textMode="MultiLine"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="checkbox" id="chkBloodDef" runat="server" />&nbsp;
                                                <label id="lblBloodDef">
                                                    High blood pressure</label>
                                            </td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtBloodDef" runat="server" width="650px" rows="3" textMode="MultiLine"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="checkbox" id="chkColDef" runat="server" />&nbsp;
                                                <label id="lblColDef">
                                                    Abnormal cholesterol or lipids</label>
                                            </td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtColDef" runat="server" width="650px" rows="3" textMode="MultiLine"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="checkbox" id="chkDVTDef" runat="server" />&nbsp;
                                                <label id="lblDVTDef">
                                                    History of DVT</label>
                                            </td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtDVTDef" runat="server" width="650px" rows="3" textMode="MultiLine"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="checkbox" id="chkVenDef" runat="server" />&nbsp;
                                                <label id="lblVenDef">
                                                    Venous stasis</label>
                                            </td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtVenDef" runat="server" width="650px" rows="3" textMode="MultiLine"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="checkbox" id="chkAntiDef" runat="server" />&nbsp;
                                                <label id="lblAntiDef">
                                                    Anticoagulant therapy</label>
                                            </td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtAntiDef" runat="server" width="650px" rows="3" textMode="MultiLine"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <input type="checkbox" id="chkCvsOtherDef" runat="server" />&nbsp;
                                                <label id="lblCvsOtherDef">
                                                    Other cardiovascular problems</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <wucTextBox:TextBox ID="txtCvsOtherNameDef" runat="server" width="150px" />
                                            </td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtCvsOtherDescDef" runat="server" width="650px" rows="3" textMode="MultiLine"/>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                                <div class="boxTop">
                                    <h3 id="hPsyDefault">
                                        PSYCHIATRIC AND NEUROLOGICAL</h3>
                                    <table border="0">
                                        <tr>
                                            <td style="width: 25%">
                                                <input type="checkbox" id="chkDepDef" runat="server" />&nbsp;
                                                <label id="lblDepDef">
                                                    Depression</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucTextBox:TextBox ID="txtDepDef" runat="server" width="650px" rows="3" textMode="MultiLine"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="checkbox" id="chkAnxDef" runat="server" />&nbsp;
                                                <label id="lblAnxDef">
                                                    Anxiety states</label>
                                            </td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtAnxDef" runat="server" width="650px" rows="3" textMode="MultiLine"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="checkbox" id="chkPhobDef" runat="server" />&nbsp;
                                                <label id="lblPhobDef">
                                                    Phobias</label>
                                            </td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtPhobDef" runat="server" width="650px" rows="3" textMode="MultiLine"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="checkbox" id="chkEatDef" runat="server" />&nbsp;
                                                <label id="lblEatDef">
                                                    Binge eating disorder</label>
                                            </td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtEatDef" runat="server" width="650px" rows="3" textMode="MultiLine"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="checkbox" id="chkHeadDef" runat="server" />&nbsp;
                                                <label id="lblHeadDef">
                                                    Headache</label>
                                            </td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtHeadDef" runat="server" width="650px" rows="3" textMode="MultiLine"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="checkbox" id="chkStrokeDef" runat="server" />&nbsp;
                                                <label id="lblStrokeDef">
                                                    Stroke</label>
                                            </td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtStrokeDef" runat="server" width="650px" rows="3" textMode="MultiLine"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <input type="checkbox" id="chkPsyOtherDef" runat="server" />&nbsp;
                                                <label id="lblPsyOtherDef">
                                                    Other psychiatric or neurological problems</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <wucTextBox:TextBox ID="txtPsyOtherNameDef" runat="server" width="150px" />
                                            </td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtPsyOtherDescDef" runat="server" width="650px" rows="3" textMode="MultiLine"/>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                                <div class="boxTop">
                                    <h3 id="hMuscDefault">
                                        MUSCULOSKELETAL</h3>
                                    <table border="0">
                                        <tr>
                                            <td style="width: 25%">
                                                <input type="checkbox" id="chkBackDef" runat="server" />&nbsp;
                                                <label id="lblBackDef">
                                                    Back pain</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucTextBox:TextBox ID="txtBackDef" runat="server" width="650px" rows="3" textMode="MultiLine"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="checkbox" id="chkHipDef" runat="server" />&nbsp;
                                                <label id="lblHipDef">
                                                    Hip pain</label>
                                            </td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtHipDef" runat="server" width="650px" rows="3" textMode="MultiLine"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="checkbox" id="chkKneeDef" runat="server" />&nbsp;
                                                <label id="lblKneeDef">
                                                    Knee pain</label>
                                            </td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtKneeDef" runat="server" width="650px" rows="3" textMode="MultiLine"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="checkbox" id="chkFeetDef" runat="server" />&nbsp;
                                                <label id="lblFeetDef">
                                                    Pain in feet</label>
                                            </td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtFeetDef" runat="server" width="650px" rows="3" textMode="MultiLine"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="checkbox" id="chkFibrDef" runat="server" />&nbsp;
                                                <label id="lblFibrDef">
                                                    Fibromyalgia</label>
                                            </td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtFibrDef" runat="server" width="650px" rows="3" textMode="MultiLine"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <input type="checkbox" id="chkMuscOtherDef" runat="server" />&nbsp;
                                                <label id="lblMuscOtherDef">
                                                    Other musculoskeletal disorders</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <wucTextBox:TextBox ID="txtMuscOtherNameDef" runat="server" width="150px" />
                                            </td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtMuscOtherDescDef" runat="server" width="650px" rows="3" textMode="MultiLine"/>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                                <div class="boxTop">
                                    <h3 id="hGenDefault">
                                        GENITOURINARY</h3>
                                    <table border="0">
                                        <tr>
                                            <td style="width: 25%">
                                                <input type="checkbox" id="chkInfDef" runat="server" />&nbsp;
                                                <label id="lblInfDef">
                                                    Infertility</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucTextBox:TextBox ID="txtInfDef" runat="server" width="650px" rows="3" textMode="MultiLine"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="checkbox" id="chkRenDef" runat="server" />&nbsp;
                                                <label id="lblRenDef">
                                                    Renal insufficiency</label>
                                            </td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtRenDef" runat="server" width="650px" rows="3" textMode="MultiLine"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="checkbox" id="chkUriDef" runat="server" />&nbsp;
                                                <label id="lblUriDef">
                                                    Urinary incontinence</label>
                                            </td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtUriDef" runat="server" width="650px" rows="3" textMode="MultiLine"/>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                                <div class="boxTop">
                                    <h3 id="hOtherDefault">
                                        OTHER</h3>
                                    <table border="0">
                                        <tr>
                                            <td style="width: 25%">
                                                <input type="checkbox" id="chkPsoDef" runat="server" />&nbsp;
                                                <label id="lblPsoDef">
                                                    Psoriasis</label>
                                            </td>
                                            <td style="width: 75%">
                                                <wucTextBox:TextBox ID="txtPsoDef" runat="server" width="650px" rows="3" textMode="MultiLine"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="checkbox" id="chkSkinDef" runat="server" />&nbsp;
                                                <label id="lblSkinDef">
                                                    Other skin disorders</label>
                                            </td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtSkinDef" runat="server" width="650px" rows="3" textMode="MultiLine"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="checkbox" id="chkCancerDef" runat="server" />&nbsp;
                                                <label id="lblCancerDef">
                                                    History of cancer</label>
                                            </td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtCancerDef" runat="server" width="650px" rows="3" textMode="MultiLine"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="checkbox" id="chkAnemiaDef" runat="server" />&nbsp;
                                                <label id="lblAnemiaDef">
                                                    History of anemia</label>
                                            </td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtAnemiaDef" runat="server" width="650px" rows="3" textMode="MultiLine"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <input type="checkbox" id="chkOtherOtherDef" runat="server" />&nbsp;
                                                <label id="lblOtherOtherDef">
                                                    Other disorders</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <wucTextBox:TextBox ID="txtOtherOtherNameDef" runat="server" width="150px" />
                                            </td>
                                            <td>
                                                <wucTextBox:TextBox ID="txtOtherOtherDescDef" runat="server" width="650px" rows="3" textMode="MultiLine"/>
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
                        <asp:AsyncPostBackTrigger ControlID="linkBtnSave" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:UpdatePanel runat="server" ID="upRegistry" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div id="div_vRegistry" style="display: none" class="bold">
                            <div class="btnWrap" align="right">
                                <table class="header">
                                    <tr>
                                        <td align="left">
                                            <input type="button" value="Delete" runat="server" id="btnDeletePatient18" style="width: 100px;"
                                                onclick="javascript:btnDelete_onclick();" onserverclick="btnDeletePatient_onserverclick" /></td>
                                        <td align="right">
                                            <input type="button" id="Button1" runat="server" value="Save" style="width: 100px"
                                                onclick="javascript:controlBar_Buttons_OnClick(18);" /></td>
                                    </tr>
                                </table>
                            </div>
                            <div class="wideBox" id="registry" runat="server">
                                <div class="boxTop">
                                    <h3 id="hRegistry">
                                        Registry Data</h3>
                                    <table class="full">
                                        <tbody>
                                            <tr class="row01" style="font-weight: bold">
                                                <td style="width:70%">
                                                    &nbsp;</td>
                                                <td align="center">
                                                    Yes</td>
                                                <td align="center">
                                                    No</td>
                                                <td align="center">
                                                    Not Documented</td>
                                            </tr>
                                            <tr class="row02">
                                                <td>
                                                    <b>Sleep Apnea</b></td>
                                                <td align="center">
                                                    <input type="radio" name="rdRegistrySleepApnea" id="rdRegistrySleepApnea_Yes"
                                                        value="yes" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdRegistrySleepApnea" id="rdRegistrySleepApnea_No"
                                                        value="no" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdRegistrySleepApnea" id="rdRegistrySleepApnea_Notdocumented"
                                                        value="notdocumented" runat="server" /></td>
                                            </tr>
                                            <tr class="row01">
                                                <td>
                                                    <b>GERD Requiring Medications</b></td>
                                                <td align="center">
                                                    <input type="radio" name="rdRegistryGerd" id="rdRegistryGerd_Yes"
                                                        value="yes" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdRegistryGerd" id="rdRegistryGerd_No"
                                                        value="no" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdRegistryGerd" id="rdRegistryGerd_Notdocumented"
                                                        value="notdocumented" runat="server" /></td>
                                            </tr>
                                            <tr class="row02">
                                                <td>
                                                    <b>Hyperlipidemia</b></td>
                                                <td align="center">
                                                    <input type="radio" name="rdRegistryHyperlipidemia" id="rdRegistryHyperlipidemia_Yes"
                                                        value="yes" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdRegistryHyperlipidemia" id="rdRegistryHyperlipidemia_No"
                                                        value="no" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdRegistryHyperlipidemia" id="rdRegistryHyperlipidemia_Notdocumented"
                                                        value="notdocumented" runat="server" /></td>
                                            </tr><tr class="row01">
                                                <td>
                                                    <b>Hypertension</b></td>
                                                <td align="center">
                                                    <input type="radio" name="rdRegistryHypertension" id="rdRegistryHypertension_Yes"
                                                        value="yes" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdRegistryHypertension" id="rdRegistryHypertension_No"
                                                        value="no" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdRegistryHypertension" id="rdRegistryHypertension_Notdocumented"
                                                        value="notdocumented" runat="server" /></td>
                                            </tr>
                                            <tr class="row02">
                                                <td colspan="4">
                                                 &nbsp;&nbsp;&nbsp;&nbsp;Number of antihypertensives medications <input type="text" id="txtAntihypertensives" maxlength="3" size="3" runat="server"/>
                                                </td>
                                            </tr>
                                            <tr class="row01">
                                                <td>
                                                    <b>Diabetes</b></td>
                                                <td align="center">
                                                    <input type="radio" name="rdRegistryDiabetes" id="rdRegistryDiabetes_Yes"
                                                        value="yes" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdRegistryDiabetes" id="rdRegistryDiabetes_No"
                                                        value="no" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdRegistryDiabetes" id="rdRegistryDiabetes_Notdocumented"
                                                        value="notdocumented" runat="server" /></td>
                                            </tr>
                                            <tr class="row02">
                                                <td colspan="4"> &nbsp;&nbsp;&nbsp;&nbsp;
                                                    <div style="float:left; width:150px"> &nbsp;&nbsp;&nbsp;&nbsp;Current medication type </div>
                                                    <div style="float:left; width:150px"><select id="rdRegistryMedication" style="width: 100%" runat="server">
                                                    <option value="">Select...</option>
                                                    <option value="rdRegistryDiabetesNil">Nil</option>
                                                    <option value="rdRegistryDiabetesNonInsulin">Non-Insulin</option>
                                                    <option value="rdRegistryDiabetesInsulin">Insulin</option>
                                                    </select>
                                                    </div>
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
                        <asp:AsyncPostBackTrigger ControlID="linkBtnSave" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:UpdatePanel runat="server" ID="upAllergies" UpdateMode="Conditional" EnableViewState="false">
                    <ContentTemplate>
                        <div class="baselineDemographics" id="div_vAllergies" style="display: none;">
                            <div class="btnWrap" align="right">
                                <table class="header">
                                    <tr>
                                        <td align="left">
                                            <input type="button" value="Delete" runat="server" id="btnDeletePatient10" style="width: 100px;"
                                                onclick="javascript:btnDelete_onclick();" onserverclick="btnDeletePatient_onserverclick" />
                                        </td>
                                        <td align="right">
                                            <input type="button" id="btnSavePatient10" runat="server" value="Save" style="width: 100px"
                                                onclick="javascript:controlBar_Buttons_OnClick(10);" /></td>
                                    </tr>
                                </table>
                            </div>
                            <div class="leftColumn">
                                <div class="personalDetails">
                                    <div class="boxTop">
                                        <h3 id="lblAllergiesRisk">
                                            Allergies & Special Risk Factor</h3>
                                        <table width="100%">
                                            <tbody>
                                                <tr style="vertical-align: top;" runat="server">
                                                    <td>
                                                        <asp:Label ID="lblHaveAllergy" Text="Do you have any allergies?" runat="server" />
                                                        &nbsp;
                                                        <input type="radio" name="rdAllergy" id="rdAllergyY" value="Y" runat="server" />
                                                        &nbsp;<label id="lblAllergiesYes">Yes</label>&nbsp;
                                                        <input type="radio" name="rdAllergy" id="rdAllergyN" value="N" runat="server" />
                                                        &nbsp;<label id="lblAllergiesNo">No&nbsp;
                                                        <br />
                                                        <asp:Label ID="lblAllergyList" Text="Please List" runat="server" />
                                                        <wucTextBox:TextBox ID="txtAllergy" runat="server" textMode="MultiLine" rows="4"
                                                            width="600px" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr style="vertical-align: top;" runat="server">
                                                    <td>
                                                        <asp:Label ID="lblHaveMedication" Text="Have you had an allergic reaction to any medications?" runat="server" />
                                                        &nbsp;
                                                        <input type="radio" name="rdMedication" id="rdMedicationY" value="Y" runat="server" />
                                                        &nbsp;<label id="lblMedicationYes">Yes</label>&nbsp;
                                                        <input type="radio" name="rdMedication" id="rdMedicationN" value="N" runat="server" />
                                                        &nbsp;<label id="lblMedicationNo">No</label>&nbsp;
                                                        <br />
                                                        <asp:Label id="lblMedicationList" Text="Please List" runat="server" />
                                                        <wucTextBox:TextBox ID="txtMedication" runat="server" textMode="MultiLine" rows="4"
                                                            width="600px" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr style="vertical-align: top;" runat="server">
                                                    <td>
                                                        <asp:Label id="lblHaveLatex" Text="Are you allergic to latex?" runat="server" />
                                                        &nbsp;
                                                        <input type="radio" name="rdLatex" id="rdLatexY" value="Y" runat="server" />
                                                        &nbsp;<label id="lblLatexYes">Yes</label>&nbsp;
                                                        <input type="radio" name="rdLatex" id="rdLatexN" value="N" runat="server" />
                                                        &nbsp;<label id="lblLatexNo">No</label>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr style="vertical-align: top;" runat="server">
                                                    <td>
                                                        <asp:Label ID="lblHaveFamilyReaction" Text="Have you or any of your family had an adverse reaction to an anesthetic? Please provide details"
                                                            runat="server" />
                                                        <wucTextBox:TextBox ID="txtAnesthetic" runat="server" textMode="MultiLine" rows="4"
                                                            width="600px" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr style="vertical-align: top;" runat="server">
                                                    <td>
                                                        <asp:Label ID="lblTendencyBleed" Text="Do you have a tendency to bleed excessively?" runat="server" />
                                                        &nbsp;
                                                        <input type="radio" name="rdBleedExcess" id="rdBleedExcessY" value="Y" runat="server" />
                                                        &nbsp;<label id="lblTendencyBleedYes">Yes</label>&nbsp;
                                                        <input type="radio" name="rdBleedExcess" id="rdBleedExcessN" value="N" runat="server" />
                                                        &nbsp;<label id="lblTendencyBleedNo">No</label>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr style="vertical-align: top;" runat="server">
                                                    <td>
                                                        <asp:Label ID="lblSurgicalRisk" Text="Do you know of any other surgical or anaesthetic risks?" runat="server" />
                                                        <wucTextBox:TextBox ID="txtAnestheticRisk" runat="server" textMode="MultiLine" rows="4"
                                                            width="600px" />
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
                                        <h3 id="lblAlcohol">
                                            Alcohol</h3>
                                        <table style="width: 100%;" border="0">
                                            <tbody>
                                                <tr style="vertical-align: top;" runat="server">
                                                    <td>
                                                        <asp:Label id="lblDrinkAlcohol" Text="Do you drink Alcohol" runat="server" />&nbsp;
                                                        <input type="radio" name="rdAlergyAlcoholDoYouDrink" id="rdAlergyAlcoholDoYouDrink_Regularly"
                                                            value="regularly" runat="server" />
                                                        &nbsp;<asp:Label ID="lblDrinkAlcoholRegularly" Text="Regularly" runat="server" />&nbsp;
                                                        <input type="radio" name="rdAlergyAlcoholDoYouDrink" id="rdAlergyAlcoholDoYouDrink_Rarely"
                                                            value="rarely" runat="server" />
                                                        &nbsp;<asp:Label ID="lblDrinkAlcoholRarely" Text="Rarely" runat="server" />&nbsp;
                                                        <input type="radio" name="rdAlergyAlcoholDoYouDrink" id="rdAlergyAlcoholDoYouDrink_Never"
                                                            value="never" runat="server" />
                                                        &nbsp;<asp:Label ID="lblDrinkAlcoholNever" Text="Never" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr style="vertical-align: top;" runat="server">
                                                    <td>
                                                        <asp:Label ID="lblDrinkPerDay" Text="How many glasses do you drink per day" runat="server" />&nbsp;
                                                        <wucTextBox:TextBox runat="server" ID="txtAlergyDrinkPerDay" maxLength="5" width="60px" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr style="vertical-align: top;" runat="server">
                                                    <td>
                                                        <asp:Label ID="lblDrinkPerWeek" Text="How many days do you drink per week" runat="server" />&nbsp;
                                                        <wucTextBox:TextBox runat="server" ID="txtAlergyDrinkPerWeek" maxLength="5" width="60px" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr style="vertical-align: top;" runat="server">
                                                    <td>
                                                        <asp:Label ID="lblDoDrink" Text="Do you drink" runat="server" />&nbsp;
                                                        <input type="checkbox" id="chkAlergyDrinkTypeBeer" runat="server" />
                                                        &nbsp;<asp:Label id="lblDrinkBeer" Text="Beer" runat="server" />&nbsp;
                                                        <input type="checkbox" id="chkAlergyDrinkTypeWine" runat="server" />
                                                        &nbsp;<asp:Label ID="lblDrinkWine" Text="Wine" runat="server" />&nbsp;
                                                        <input type="checkbox" id="chkAlergyDrinkTypeSpirits" runat="server" />
                                                        &nbsp;<asp:Label ID="lblDrinkSpirits" Text="Spirits" runat="server" />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <div class="boxBtm">
                                        </div>
                                    </div>
                                </div>
                                <div class="medicalDetails">
                                    <div class="boxTop">
                                        <h3 id="hNotes">
                                            Smoking</h3>
                                        <table border="0">
                                            <tbody>
                                                <tr style="vertical-align: top;" runat="server">
                                                    <td>
                                                        <asp:Label ID="lblDoSmoke" Text="Do you smoke" runat="server" />&nbsp;
                                                        <input type="radio" name="rdAlergySmokeDoYouSmoke" id="rdAlergySmokeDoYouSmoke_Yes"
                                                            value="yes" runat="server" />
                                                        &nbsp;<asp:Label ID="lblDoSmokeYes" Text="Yes" runat="server" />&nbsp;
                                                        <input type="radio" name="rdAlergySmokeDoYouSmoke" id="rdAlergySmokeDoYouSmoke_No"
                                                            value="no" runat="server" />
                                                        &nbsp;<asp:Label ID="lblDoSmokeNo" Text="No" runat="server" />&nbsp;
                                                        <input type="radio" name="rdAlergySmokeDoYouSmoke" id="rdAlergySmokeDoYouSmoke_Never"
                                                            value="never" runat="server" />
                                                        &nbsp;<asp:Label ID="lblDoSmokeNever" Text="Never" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;" runat="server">
                                                    <td>
                                                        <asp:Label id="lblSmokePerDay" Text="If yes, how many per day" runat="server" />&nbsp;
                                                        <wucTextBox:TextBox runat="server" ID="txtAlergySmokePerDay" maxLength="5" width="60px" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr style="vertical-align: top;" runat="server">
                                                    <td>
                                                        <asp:Label ID="lblSmokePast" Text="If no, have you smoked in the past" runat="server" />&nbsp;
                                                        <input type="radio" name="rdAlergySmokeHaveYouSmoke" id="rdAlergySmokeHaveYouSmoke_Yes"
                                                            value="1" runat="server" />
                                                        &nbsp;<asp:Label id="lblSmokePastYes" Text="Yes" runat="server" />&nbsp;
                                                        <input type="radio" name="rdAlergySmokeHaveYouSmoke" id="rdAlergySmokeHaveYouSmoke_No"
                                                            value="0" runat="server" />
                                                        &nbsp;<asp:Label id="lblSmokePastNo" Text="No" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;" runat="server">
                                                    <td>
                                                        <asp:Label ID="lblSmokePastPerDay" Text="If so, how many per day" runat="server" />&nbsp;
                                                        <wucTextBox:TextBox runat="server" ID="txtAlergySmokePastPerDay" maxLength="5" width="60px" />
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;" runat="server">
                                                    <td>
                                                        <asp:Label id="lblSmokePastHowManyYears" Text="For how many years" runat="server" />&nbsp;
                                                        <wucTextBox:TextBox runat="server" ID="txtAlergySmokeHowmanyYears" maxLength="5"
                                                            width="60px" />
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;" runat="server">
                                                    <td>
                                                        <asp:Label id="lblSmokeStop" Text="When did you stop smoking" runat="server" />&nbsp;
                                                        <wucTextBox:TextBox runat="server" ID="txtAlergySmokeWhenStop" maxLength="5" width="60px" />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <div class="boxBtm">
                                        </div>
                                    </div>
                                </div>
                                <div class="medicalDetails">
                                    <div class="boxTop">
                                        <h3 id="lblIllegalDrugs">
                                            Illegal Drugs / Tobacco
                                        </h3>
                                        <table border="0">
                                            <tbody>
                                                <tr style="vertical-align: top;" runat="server">
                                                    <td>
                                                        <asp:Label ID="lblUseIllegalDrugs" Text="Do you use illegal drugs / tobacco" runat="server" />&nbsp;
                                                        <input type="radio" name="rdAllergyDrugsDoYou" id="rdAllergyDrugsDoYouY" value="Y"
                                                            runat="server" />
                                                        &nbsp;<asp:Label ID="lblUseIllegalDrugsYes" Text="Yes" runat="server" />&nbsp;
                                                        <input type="radio" name="rdAllergyDrugsDoYou" id="rdAllergyDrugsDoYouN" value="N"
                                                            runat="server" />
                                                        &nbsp;<asp:Label ID="lblUseIllegalDrugsNo" Text="No" runat="server" />&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <wucTextBox:TextBox runat="server" ID="txtAlergyDrugsNotes" width="250px" rows="1"
                                                            textMode="MultiLine" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;</td>
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
                        <asp:AsyncPostBackTrigger ControlID="linkBtnSave" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:UpdatePanel runat="server" ID="upWeightHistory" UpdateMode="Conditional" EnableViewState="false">
                    <ContentTemplate>
                        <div class="bold" id="div_vWeightHistory" runat="server" style="display: none;">
                            <div class="btnWrap" align="right">
                                <table class="header">
                                    <tr>
                                        <td align="left">
                                            <input type="button" value="Delete" runat="server" id="btnDeletePatient11" style="width: 100px;"
                                                onclick="javascript:btnDelete_onclick();" onserverclick="btnDeletePatient_onserverclick" /></td>
                                        <td align="right">
                                            <input type="button" id="btnSavePatient11" runat="server" value="Save" style="width: 100px"
                                                onclick="javascript:controlBar_Buttons_OnClick(11);" /></td>
                                    </tr>
                                </table>
                            </div>
                            <div class="wideBox">
                                <div class="boxTop">
                                    <h3 id="lblWeightHistory">
                                        Weight History</h3>
                                    <table class="full">
                                        <tbody>
                                            <tr style="vertical-align: top;" runat="server">
                                                <td colspan="5">
                                                    <label id="lblWeightHistoryInfo1">Please enter data on weight changes in recent years using the possible format:</label><br />
                                                    <label id="lblWeightHistoryInfo2">There is a history of weight gain in recent years with a known weight gain of approximately</label>
                                                    &nbsp;<wucTextBox:TextBox runat="server" ID="txtWeightHistoryGainWeight" maxLength="3"
                                                        width="40px" />
                                                    &nbsp;
                                                    <asp:Label runat="server" ID="lblWeightHistoryGainWeightUnit" Text="lbs" />
                                                    <label id="lblWeightHistoryInfo3">in the last</label>
                                                    &nbsp;<wucTextBox:TextBox runat="server" ID="txtWeightHistoryGainYears" maxLength="3" width="25px" />
                                                    &nbsp;<label id="lblWeightHistoryInfo4">years</label> <br />
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr class="row01" style="font-weight: bold">
                                                <td>
                                                    &nbsp;</td>
                                                <td align="center">
                                                    <label id="lblWeightHistoryBelowAverage">Below Average</label></td>
                                                <td align="center">
                                                    <label id="lblWeightHistoryAverage">Average Weight</label></td>
                                                <td align="center">
                                                    <label id="lblWeightHistoryAboveAverage">Above Average</label></td>
                                                <td align="center">
                                                    <label id="lblWeightHistoryVeryHeavy">Very Heavy</label></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblWeightHistory1">Birth Weight</label></td>
                                                <td align="center">
                                                    <input type="radio" name="rdWeightHistoryHistoryIndicateBirth" id="rdWeightHistoryHistoryIndicateBirth_Below"
                                                        value="below" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdWeightHistoryHistoryIndicateBirth" id="rdWeightHistoryHistoryIndicateBirth_Average"
                                                        value="average" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdWeightHistoryHistoryIndicateBirth" id="rdWeightHistoryHistoryIndicateBirth_Above"
                                                        value="above" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdWeightHistoryHistoryIndicateBirth" id="rdWeightHistoryHistoryIndicateBirth_Very"
                                                        value="very" runat="server" /></td>
                                            </tr>
                                            <tr class="row01">
                                                <td>
                                                    <label id="lblWeightHistory2">Weight at starting school (5-6 years)</label></td>
                                                <td align="center">
                                                    <input type="radio" name="rdWeightHistoryHistoryIndicateStart" id="rdWeightHistoryHistoryIndicateStart_Below"
                                                        value="below" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdWeightHistoryHistoryIndicateStart" id="rdWeightHistoryHistoryIndicateStart_Average"
                                                        value="average" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdWeightHistoryHistoryIndicateStart" id="rdWeightHistoryHistoryIndicateStart_Above"
                                                        value="above" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdWeightHistoryHistoryIndicateStart" id="rdWeightHistoryHistoryIndicateStart_Very"
                                                        value="very" runat="server" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblWeightHistory3">Weight at beginning of high school (10-12 yrs)</label></td>
                                                <td align="center">
                                                    <input type="radio" name="rdWeightHistoryHistoryIndicateHighStart" id="rdWeightHistoryHistoryIndicateHighStart_Below"
                                                        value="below" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdWeightHistoryHistoryIndicateHighStart" id="rdWeightHistoryHistoryIndicateHighStart_Average"
                                                        value="average" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdWeightHistoryHistoryIndicateHighStart" id="rdWeightHistoryHistoryIndicateHighStart_Above"
                                                        value="above" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdWeightHistoryHistoryIndicateHighStart" id="rdWeightHistoryHistoryIndicateHighStart_Very"
                                                        value="very" runat="server" /></td>
                                            </tr>
                                            <tr class="row01">
                                                <td>
                                                    <label id="lblWeightHistory4">Weight at end of high school (15-18 years)</label></td>
                                                <td align="center">
                                                    <input type="radio" name="rdWeightHistoryHistoryIndicateHighEnd" id="rdWeightHistoryHistoryIndicateHighEnd_Below"
                                                        value="below" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdWeightHistoryHistoryIndicateHighEnd" id="rdWeightHistoryHistoryIndicateHighEnd_Average"
                                                        value="average" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdWeightHistoryHistoryIndicateHighEnd" id="rdWeightHistoryHistoryIndicateHighEnd_Above"
                                                        value="above" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdWeightHistoryHistoryIndicateHighEnd" id="rdWeightHistoryHistoryIndicateHighEnd_Very"
                                                        value="very" runat="server" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblWeightHistory5">Weight at time of commencing work (21 years)</label></td>
                                                <td align="center">
                                                    <input type="radio" name="rdWeightHistoryHistoryIndicateWork" id="rdWeightHistoryHistoryIndicateWork_Below"
                                                        value="below" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdWeightHistoryHistoryIndicateWork" id="rdWeightHistoryHistoryIndicateWork_Average"
                                                        value="average" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdWeightHistoryHistoryIndicateWork" id="rdWeightHistoryHistoryIndicateWork_Above"
                                                        value="above" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdWeightHistoryHistoryIndicateWork" id="rdWeightHistoryHistoryIndicateWork_Very"
                                                        value="very" runat="server" /></td>
                                            </tr>
                                            <tr class="row01">
                                                <td>
                                                    <label id="lblWeightHistory6">Weight at time of marriage (if applicable)</label></td>
                                                <td align="center">
                                                    <input type="radio" name="rdWeightHistoryHistoryIndicateMarriage" id="rdWeightHistoryHistoryIndicateMarriage_Below"
                                                        value="below" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdWeightHistoryHistoryIndicateMarriage" id="rdWeightHistoryHistoryIndicateMarriage_Average"
                                                        value="average" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdWeightHistoryHistoryIndicateMarriage" id="rdWeightHistoryHistoryIndicateMarriage_Above"
                                                        value="above" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdWeightHistoryHistoryIndicateMarriage" id="rdWeightHistoryHistoryIndicateMarriage_Very"
                                                        value="very" runat="server" /></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                            </div>
                            <div class="wideBox">
                                <div class="boxTop">
                                    <h3>
                                        <label id="lblWeightLossHistory">Weight Loss History</label></h3>
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <table class="half">
                                                        <tr style="vertical-align: top;" runat="server">
                                                            <td>
                                                                <label id="lblWeightLossHistoryLoseWeight">How long have you been trying to lose weight?</label> &nbsp;
                                                                <wucTextBox:TextBox runat="server" ID="txtWeightHistoryLossTryLose" maxLength="3"
                                                                    width="25px" />
                                                                &nbsp;<label id="lblWeightLossHistoryLoseWeight2">years</label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;</td>
                                                        </tr>
                                                        <tr id="Tr2" style="vertical-align: top;" runat="server">
                                                            <td>
                                                                <label id="lblWeightLossHistoryTry">Which of the following have you tried at some time:</label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <input type="checkbox" id="chkWeightHistoryLossDiet" runat="server" />&nbsp;<label id="lblWeightLossHistoryDiet">Dieting
                                                                - any effort to reduce your food intake</label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <input type="checkbox" id="chkWeightHistoryLossExercise" runat="server" />&nbsp;<label id="lblWeightLossHistoryExercise">Exercise
                                                                - increased activity eg walking, sports, etc</label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <table class="half">
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <label id="lblWeightLossHistoryCommercial">Which commercial weight loss groups have you attended:</label></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <input type="checkbox" id="chkWeightHistoryLossGrpAD" runat="server" />
                                                                            Atkins Diet</td>
                                                                        <td>
                                                                            <input type="checkbox" id="chkWeightHistoryLossGrpNS" runat="server" />
                                                                            Nutrisystem</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <input type="checkbox" id="chkWeightHistoryLossGrpGM" runat="server" />
                                                                            Gloria Marshall</td>
                                                                        <td>
                                                                            <input type="checkbox" id="chkWeightHistoryLossGrpOF" runat="server" />
                                                                            Optifast</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <input type="checkbox" id="chkWeightHistoryLossGrpGD" runat="server" />
                                                                            Grapefruit Diet</td>
                                                                        <td>
                                                                            <input type="checkbox" id="chkWeightHistoryLossGrpP" runat="server" />
                                                                            Protein</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <input type="checkbox" id="chkWeightHistoryLossGrpGB" runat="server" />
                                                                            Gut Busters</td>
                                                                        <td>
                                                                            <input type="checkbox" id="chkWeightHistoryLossGrpSF" runat="server" />
                                                                            Slim Fast</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <input type="checkbox" id="chkWeightHistoryLossGrpHL" runat="server" />
                                                                            Herbal Life</td>
                                                                        <td>
                                                                            <input type="checkbox" id="chkWeightHistoryLossGrpSB" runat="server" />
                                                                            South Beach</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <input type="checkbox" id="chkWeightHistoryLossGrpJG" runat="server" />
                                                                            Jenny Craig</td>
                                                                        <td>
                                                                            <input type="checkbox" id="chkWeightHistoryLossGrpSS" runat="server" />
                                                                            SureSlim</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <input type="checkbox" id="chkWeightHistoryLossGrpLW" runat="server" />
                                                                            LA Weight Loss</td>
                                                                        <td>
                                                                            <input type="checkbox" id="chkWeightHistoryLossGrpT" runat="server" />
                                                                            TOPS</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <input type="checkbox" id="chkWeightHistoryLossGrpLE" runat="server" />
                                                                            Lite n’easy</td>
                                                                        <td>
                                                                            <input type="checkbox" id="chkWeightHistoryLossGrpTC" runat="server" />
                                                                            TOWN Club</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <input type="checkbox" id="chkWeightHistoryLossGrpMF" runat="server" />
                                                                            Medifast</td>
                                                                        <td>
                                                                            <input type="checkbox" id="chkWeightHistoryLossGrpWW" runat="server" />
                                                                            Weight Watchers</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <input type="checkbox" id="chkWeightHistoryLossGrpML" runat="server" />
                                                                            Metabolife</td>
                                                                        <td>
                                                                            &nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <label id="lblWeightLossHistoryCommercialOther">Others</label> &nbsp;
                                                                            <wucTextBox:TextBox runat="server" ID="txtWeightHistoryLossGrpOther" width="280px" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="CommWeightLoss" runat="server" visible="false">
                                                                        <td colspan="2">
                                                                            Duration with commercial weight loss groups? &nbsp;
                                                                            <wucTextBox:TextBox runat="server" ID="txtWeightHistoryGroupDuration" maxLength="3"
                                                                                width="25px" />
                                                                            &nbsp;years
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="CommDurationLoss" runat="server" visible="false">
                                                                        <td colspan="2">
                                                                            Weight loss with commercial weight loss groups? &nbsp;
                                                                            <wucTextBox:TextBox runat="server" ID="txtWeightHistoryGroupLose" maxLength="3"
                                                                                width="25px" />
                                                                            &nbsp;kg
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <label id="lblWeightLossHistoryDietPill">Diet Pills:</label></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <input type="checkbox" id="chkWeightHistoryLossDietA" runat="server" />
                                                                            Adifax</td>
                                                                        <td>
                                                                            <input type="checkbox" id="chkWeightHistoryLossDietP" runat="server" />
                                                                            Phentermine</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <input type="checkbox" id="chkWeightHistoryLossDietAm" runat="server" />
                                                                            Amphetamines</td>
                                                                        <td>
                                                                            <input type="checkbox" id="chkWeightHistoryLossDietR" runat="server" />
                                                                            Reductil</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <input type="checkbox" id="chkWeightHistoryLossDietD" runat="server" />
                                                                            Duromine</td>
                                                                        <td>
                                                                            <input type="checkbox" id="chkWeightHistoryLossDietRe" runat="server" />
                                                                            Redux</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <input type="checkbox" id="chkWeightHistoryLossDietM" runat="server" />
                                                                            Meridia</td>
                                                                        <td>
                                                                            <input type="checkbox" id="chkWeightHistoryLossDietT" runat="server" />
                                                                            Tenuate</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <input type="checkbox" id="chkWeightHistoryLossDietPf" runat="server" />
                                                                            Phen-Fen</td>
                                                                        <td>
                                                                            <input type="checkbox" id="chkWeightHistoryLossDietX" runat="server" />
                                                                            Xenical</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <label id="lblWeightLossHistoryDietPillOther">Others</label> &nbsp;
                                                                            <wucTextBox:TextBox runat="server" ID="txtWeightHistoryLossDietOther" width="280px" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table class="half">
                                                        <tr>
                                                            <td colspan="2">
                                                                <label id="lblWeightLossHistoryProfessionalAdvice">Professional Advice:</label></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <input type="checkbox" id="chkWeightHistoryLossPAA" runat="server" />
                                                                <label id="lblWeightLossHistoryProfessionalAcupuncture">Acupuncture</label></td>
                                                            <td>
                                                                <input type="checkbox" id="chkWeightHistoryLossPALD" runat="server" />
                                                                <label id="lblWeightLossHistoryProfessionalLocalDoctor">Local Doctor</label></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <input type="checkbox" id="chkWeightHistoryLossPAD" runat="server" />
                                                                <label id="lblWeightLossHistoryProfessionalDietitan">Dietitian</label></td>
                                                            <td>
                                                                <input type="checkbox" id="chkWeightHistoryLossPAN" runat="server" />
                                                                <label id="lblWeightLossHistoryProfessionalNeuropath">Naturopath</label></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <input type="checkbox" id="chkWeightHistoryLossPAH" runat="server" />
                                                                <label id="lblWeightLossHistoryProfessionalHypnotherapist">Hypnotherapist</label></td>
                                                            <td>
                                                                <input type="checkbox" id="chkWeightHistoryLossPAP" runat="server" />
                                                                <label id="lblWeightLossHistoryProfessionalPsychologist">Psychologist</label></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <label id="lblWeightLossHistoryProfessionalOther">Others</label> &nbsp;
                                                                <wucTextBox:TextBox runat="server" ID="txtWeightHistoryLossPAOther" width="280px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label id="lblWeightLossHistoryLowCalorieDiet">Very low calorie diets:</label></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <input type="checkbox" id="chkWeightHistoryLossLCMD" runat="server" />
                                                                Optifast</td>
                                                            <td>
                                                                <input type="checkbox" id="chkWeightHistoryLossLCOT" runat="server" />
                                                                <label id="lblWeightLossHistoryLowCalorieOther">Other</label></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <bold>
                                                                <label id="lblWeightLossHistoryOtherOther">Others</label>:</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <input type="checkbox" id="chkWeightHistoryLossOHR" runat="server" />
                                                                <label id="lblWeightLossHistoryOtherHerbal">Herbal remedies</label></td>
                                                            <td>
                                                                <input type="checkbox" id="chkWeightHistoryLossOWL" runat="server" />
                                                                <label id="lblWeightLossHistoryOtherDevice">Weight loss devices</label></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <input type="checkbox" id="chkWeightHistoryLossOIT" runat="server" />
                                                                <label id="lblWeightLossHistoryOtherInjection">Injection therapy</label></td>
                                                            <td>
                                                                &nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <label id="lblWeightLossHistoryOtherSurgicalTreatment">Surgical Treatments :</label></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <input type="checkbox" id="chkWeightHistoryLossSAG" runat="server" />
                                                                <label id="lblWeightLossHistoryOtherAdjustableBand">Adjustable Gastric Banding</label></td>
                                                            <td>
                                                                <input type="checkbox" id="chkWeightHistoryLossSL" runat="server" />
                                                                <label id="lblWeightLossHistoryOtherLiposuction">Liposuction</label></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <input type="checkbox" id="chkWeightHistoryLossSA" runat="server" />
                                                                <label id="lblWeightLossHistoryOtherApronectomy">Apronectomy</label></td>
                                                            <td>
                                                                <input type="checkbox" id="chkWeightHistoryLossSB" runat="server" />
                                                                <label id="lblWeightLossHistoryOtherSmallBypass">Small bowel bypass</label></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <input type="checkbox" id="chkWeightHistoryLossSFG" runat="server" />
                                                                <label id="lblWeightLossHistoryOtherFixedBand">Fixed gastric banding</label></td>
                                                            <td>
                                                                <input type="checkbox" id="chkWeightHistoryLossSSS" runat="server" />
                                                                <label id="lblWeightLossHistoryOtherStomachStapling">Stomach stapling</label></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <label id="lblWeightLossHistoryCosmetic">Cosmetic procedures - List:</label> &nbsp;
                                                                <wucTextBox:TextBox runat="server" ID="txtWeightHistoryLossCosmetic" width="280px" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="linkBtnSave" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:UpdatePanel runat="server" ID="upExamination" EnableViewState="false">
                    <ContentTemplate>
                        <div id="div_vExamination" class="bold" style="display: none">
                            <div class="btnWrap" align="right">
                                <table class="header">
                                    <tr>
                                        <td align="left">
                                            <input type="button" value="Delete" runat="server" id="btnDeletePatient13" style="width: 100px;"
                                                onclick="javascript:btnDelete_onclick();" onserverclick="btnDeletePatient_onserverclick" />
                                        </td>
                                        <td align="right">
                                            <input type="button" id="btnSavePatient13" runat="server" value="Save" style="width: 100px"
                                                onclick="javascript:controlBar_Buttons_OnClick(13);" /></td>
                                    </tr>
                                </table>
                            </div>
                            <div class="wideBox">
                                <div class="boxTop">
                                    <h3 id="lblPhysicalExamination">
                                        Physical Examination</h3>
                                    <div class="addVisitDetails">
                                        <table class="full">
                                            <tr>
                                                <td colspan="2">
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style="font-weight: bold">
                                                    <label id="lblPhysicalGeneralAppearance">General Appearance</label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <input type="checkbox" runat="server" id="chkExamGAO" onclick="javascript:checkReadOnly('ExamGAO');" />
                                                    &nbsp;
                                                    <input type="text" runat="server" id="txtExamGAO" size="130" disabled="true" value="Obese. Otherwise normal appearance and affect; no acute distress" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style="font-weight: bold">
                                                    <label id="lblPhysicalHeadNeck">Head and Neck</label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <input type="checkbox" id="chkExamHNG" runat="server" onclick="javascript:checkReadOnly('ExamHNG');" />
                                                    &nbsp;
                                                    <input type="text" runat="server" id="txtExamHNG" size="130" disabled="true" value="Gen. appearance normal, No change in vision; Normal eye movements; conjunctiva and lids - NAD; 5th nerve function normal; facial nerve function normal" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <input type="checkbox" id="chkExamHNH" runat="server" onclick="javascript:checkReadOnly('ExamHNH');" />
                                                    &nbsp;
                                                    <input type="text" runat="server" id="txtExamHNH" size="130" disabled="true" value="Adequate hearing" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <input type="checkbox" id="chkExamHNM" runat="server" onclick="javascript:checkReadOnly('ExamHNM');" />
                                                    &nbsp;
                                                    <input type="text" runat="server" id="txtExamHNM" size="130" disabled="true" value="Mouth, tongue, teeth normal" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <input type="checkbox" id="chkExamHNN" runat="server" onclick="javascript:checkReadOnly('ExamHNN');" />
                                                    &nbsp;
                                                    <input type="text" runat="server" id="txtExamHNN" size="130" disabled="true" value="Neck - no thyroid enlargement or nodules; no lymphadenopathy, carotid pulses normal, no bruit" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style="font-weight: bold">
                                                    <label id="lblPhysicalChest">Chest</label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <input type="checkbox" id="chkExamCH" runat="server" onclick="javascript:checkReadOnly('ExamCH');" />
                                                    &nbsp;
                                                    <input type="text" runat="server" id="txtExamCH" size="130" disabled="true" value="HS - dual rhythm; no murmurs" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <input type="checkbox" id="chkExamCP" runat="server" onclick="javascript:checkReadOnly('ExamCP');" />
                                                    &nbsp;
                                                    <input type="text" runat="server" id="txtExamCP" size="130" disabled="true" value="PN - resonant all over; no fremitus or rubs; vesicular BS and no adventitial sounds" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style="font-weight: bold">
                                                    <label id="lblPhysicalAbdominal">Abdominal</label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <input type="checkbox" id="chkExamAA" runat="server" onclick="javascript:checkReadOnly('ExamAA');" />
                                                    &nbsp;
                                                    <input type="text" runat="server" id="txtExamAA" size="130" disabled="true" value="Abdomen soft, non-tender; no hepatosplenomegaly; normal bowel sounds, no renal tenderness; no masses found; NAD" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style="font-weight: bold">
                                                    <label id="lblPhysicalLimbSkin">Limbs and Skin</label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <input type="checkbox" id="chkExamLSR" runat="server" onclick="javascript:checkReadOnly('ExamLSR');" />
                                                    &nbsp;
                                                    <input type="text" runat="server" id="txtExamLSR" size="130" disabled="true" value="No rashes; peripheral pulses normal, no leg ulcers; no skin or subcutaneous lesions" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <input type="checkbox" id="chkExamLSB" runat="server" onclick="javascript:checkReadOnly('ExamLSB');" />
                                                    &nbsp;
                                                    <input type="text" runat="server" id="txtExamLSB" size="130" disabled="true" value="Breasts - no masses" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <input type="checkbox" id="chkExamLSA" runat="server" onclick="javascript:checkReadOnly('ExamLSA');" />
                                                    &nbsp;
                                                    <input type="text" runat="server" id="txtExamLSA" size="130" disabled="true" value="No swelling of ankles" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style="font-weight: bold">
                                                    <label id="lblPhysicalMentalStatus">Mental Status</label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <input type="checkbox" id="chkExamMSO" runat="server" onclick="javascript:checkReadOnly('ExamMSO');" />
                                                    &nbsp;
                                                    <input type="text" runat="server" id="txtExamMSO" size="130" disabled="true" value="Oriented in time and place, judgment and insight normal, good recent and remote memory, mood and affect appear normal" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style="font-weight: bold">
                                                    <label id="lblPhysicalMobility">Mobility</label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <input type="checkbox" id="chkExamMF" runat="server" onclick="javascript:checkReadOnly('ExamMF');" />
                                                    &nbsp;
                                                    <input type="text" runat="server" id="txtExamMF" size="130" disabled="true" value="Fully mobile; normal gait; normal range of movement; no joint instability; muscle tone normal" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="font-weight: bold">
                                                    <label id="lblPhysicalNotes">Notes</label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <wucTextBox:TextBox ID="txtExamNotes" runat="server" textMode="MultiLine" rows="3"
                                                        width="900px" />
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
                        <asp:AsyncPostBackTrigger ControlID="linkBtnSave" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:UpdatePanel runat="server" ID="upMeasurement" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="bold" id="div_vMeasurement" style="display: none">
                            <div class="btnWrap" align="right">
                                <table class="header">
                                    <tr>
                                        <td align="left">
                                            <input type="button" value="Delete" runat="server" id="btnDeletePatient12" style="width: 100px;"
                                                onclick="javascript:btnDelete_onclick();" onserverclick="btnDeletePatient_onserverclick" /></td>
                                        <td align="right">
                                            <input type="button" id="btnSavePatient12" runat="server" value="Save" style="width: 100px"
                                                onclick="javascript:controlBar_Buttons_OnClick(12);" /></td>
                                    </tr>
                                </table>
                            </div>
                            <div class="baseline">
                                <div class="boxTop">
                                    <h3 id="lblBaseline">
                                        Baseline</h3>
                                    <table class="full" border="0">
                                        <tbody>
                                            <tr>
                                                <td style="width: 50%" enableviewstate="false">
                                                    <table>
                                                        <tr>
                                                            <td style="width: 40%;">
                                                                <label id="lblMeasurementHeight">Height</label></td>
                                                            <td style="width: 20%;">
                                                                <wucTextBox:TextBox runat="server" ID="txtMeasurementHeight" width="90%" Text="0" />
                                                            </td>
                                                            <td style="width: 20%;">
                                                                <asp:Label runat="server" ID="lblMeasurementHeightUnit" Text="Inches" />
                                                            </td>
                                                            <td style="width: 20%;">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label id="lblMeasurementWeight">Weight</label></td>
                                                            <td>
                                                                <wucTextBox:TextBox runat="server" ID="txtMeasurementWeight" width="90%" Text="0" />
                                                            </td>
                                                            <td>
                                                                <asp:Label runat="server" ID="lblMeasurementWeightUnit" Text="lbs" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label id="lblMeasurementIdealWeight">Ideal Weight (BMI</label>
                                                                <asp:Label runat="server" ID="lblMeasurementIBMI" />)
                                                                <td>
                                                                    <asp:Label runat="server" ID="lblMeasurementIWeight" Text="1" />
                                                                    <wucTextBox:TextBox runat="server" ID="txtIdealWeight" width="90%" Text="0" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label runat="server" ID="lblMeasurementIWeightUnit" Text="lbs" />
                                                                </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label id="lblMeasurementBMI">BMI</label></td>
                                                            <td>
                                                                <wucTextBox:TextBox runat="server" ID="txtBMI" width="90%" ReadOnly="true" Text="0"
                                                                    Enabled="true" />
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label id="lblMeasurementExcessWeight">Excess Weight</label></td>
                                                            <td>
                                                                <wucTextBox:TextBox runat="server" ID="txtMeasurementEWeight" width="90%" maxLength="5"
                                                                    Text="0" />
                                                            </td>
                                                            <td>
                                                                <asp:Label runat="server" ID="lblMeasurementEWeightUnit" Text="lbs" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label id="lblMeasurementTargetWeight">Target Weight</label></td>
                                                            <td>
                                                                <wucTextBox:TextBox runat="server" ID="txtMeasurementTWeight" width="90%" Text="0" />
                                                            </td>
                                                            <td>
                                                                <asp:Label runat="server" ID="lblMeasurementTWeightUnit" Text="lbs" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <input type="button" id="btnMeasurementBMI" value="" onclick="javascript:btnBMIBaseline_onclick();"
                                                                    style="width: 20px" />
                                                                <label id="lblMeasurementBMI2">BMI</label>&nbsp;
                                                                <asp:Label runat="server" ID="lblMeasurementTBMI" ForeColor="blue" />
                                                            </td>
                                                            <td colspan="2">
                                                                <input type="button" id="btnMeasurementEWeight" value="" onclick="javascript:btnExcessWeightBaseline_onclick();"
                                                                    style="width: 20px" />
                                                                <asp:Label runat="server" ID="lblMeasurementEWeight_Value" Text="66" ForeColor="blue" />&nbsp;
                                                                <label id="lblMeasurementExcessWeight2">% Excess Wt</label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td valign="top">
                                                    <table>
                                                        <tbody>
                                                            <tr enableviewstate="false">
                                                                <td style="width: 50%;">
                                                                    <label id="lblMeasurementNeck">Neck</label></td>
                                                                <td style="width: 30%;">
                                                                    <wucTextBox:TextBox runat="server" ID="txtMeasurementNeck" maxLength="4" width="90%"
                                                                        Text="0" />
                                                                </td>
                                                                <td style="width: 20%;">
                                                                    <asp:Label runat="server" ID="lblMeasurementNeckUnit" Text="Inches" />
                                                                </td>
                                                            </tr>
                                                            <tr enableviewstate="false">
                                                                <td>
                                                                    <label id="lblMeasurementWaist">Waist</label></td>
                                                                <td>
                                                                    <wucTextBox:TextBox runat="server" ID="txtMeasurementWaist" maxLength="4" width="90%"
                                                                        Text="0" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label runat="server" ID="lblMeasurementWaistUnit" Text="Inches" />
                                                                </td>
                                                            </tr>
                                                            <tr enableviewstate="false">
                                                                <td>
                                                                    <label id="lblMeasurementHip">Hip</label></td>
                                                                <td>
                                                                    <wucTextBox:TextBox runat="server" ID="txtMeasurementHip" maxLength="4" width="90%"
                                                                        Text="0" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label runat="server" ID="lblMeasurementHipUnit" Text="Inches" />
                                                                </td>
                                                            </tr>
                                                            <tr enableviewstate="false">
                                                                <td>
                                                                    <label id="lblMeasurementWHRTitle">WHR:</label>
                                                                </td>
                                                                <td colspan="2">
                                                                    <label id="lblMeasurementWHR" runat="server" />
                                                                    <asp:HiddenField runat="server" ID="txtHMeasurementNeck" Value="0" />
                                                                    <asp:HiddenField runat="server" ID="txtHMeasurementWaist" Value="0" />
                                                                    <asp:HiddenField runat="server" ID="txtHMeasurementHip" Value="0" />
                                                                </td>
                                                            </tr>
                                                            <tr enableviewstate="false">
                                                                <td colspan="3">
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr enableviewstate="false">
                                                                <td>
                                                                    <label id="lblMeasurementZeroDate">Zero Date:</label>
                                                                </td>
                                                                <td>
                                                                    <wucTextBox:TextBox runat="server" ID="txtZeroDate" maxLength="10" width="90%" />
                                                                </td>
                                                                <td>
                                                                    <a href="#this" id="a1" onclick="javascript:aCalendar_onclick(this,'txtZeroDate');">
                                                                        [...]</a>&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr enableviewstate="false">
                                                                <td colspan="3">
                                                                    <br />
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
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                            </div>
                            <div class="vitalsign" enableviewstate="false">
                                <div class="boxTop">
                                    <h3>
                                        <label id="lblMeasurementVitalSign">Vital Signs</label></h3>
                                    <table border="0">
                                        <tbody>
                                            <tr>
                                                <td style="width: 15%">
                                                    <label id="lblMeasurementPR">PR</label></td>
                                                <td>
                                                    <wucTextBox:TextBox runat="server" ID="txtMeasurementPR" maxLength="4" width="10%" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 15%">
                                                    <label id="lblMeasurementRR">RR</label></td>
                                                <td>
                                                    <wucTextBox:TextBox runat="server" ID="txtMeasurementRR" maxLength="4" width="10%" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 15%">
                                                    <label id="lblMeasurementBP">BP</label></td>
                                                <td>
                                                    <wucTextBox:TextBox runat="server" ID="txtMeasurementBPLow" maxLength="4" width="10%" />
                                                    /
                                                    <wucTextBox:TextBox runat="server" ID="txtMeasurementBPHigh" maxLength="4" width="10%" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                            </div>
                            <div class="vitalsign" id="divGraph" enableviewstate="false" runat="server">
                                <div class="boxTop">
                                    <h3 id="lblMeasurementGraph">
                                        Graphs</h3>
                                    <table border="0">
                                        <tbody> 
                                            <tr id="IdealEWL" runat="server">
                                                <td>
                                                    <label id="lblIEWL">
                                                        Target % Excess Weight Loss Graph</label>
                                                    <input type="hidden" id="ReportCode" value="IEWLG" />
                                                </td>
                                            </tr>
                                            <tr id="IncludeActualWeightLoss" runat="server">
                                                <td>
                                                    <input type="checkbox" id="chkIncActualLoss"/>
                                                    <label id="lblIAEWL" onclick="$get('chkIncActualLoss').checked = !$get('chkIncActualLoss').checked;"
                                                        onmouseover="javascript:this.style.cursor = 'pointer';" onmouseout="javascript:this.style.cursor = '';">
                                                        Include Target Actual Weight Loss Graph</label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                <br />
                                                    <label id="lblMeasurementGraphInfo">Graph was drawn using the last saved measurement</label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <input type="button" id="btnDownloadExcel" runat="server" value="Download Excel" onclick="javascript:BuildGraph(2);"
                                                    style="width: 110px"/>
                                                    <input type="button" id="btnBuildGraph" value="Build Graph" onclick="javascript:BuildGraph(1);"
                                                    style="width: 80px" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                            </div>
                            
                            <div class="wideBox" enableviewstate="false">
                                <div class="boxTop">
                                    <h3 id="lblMeasurementNotes">
                                        Notes</h3>
                                    <table border="0">
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <wucTextBox:TextBox runat="server" ID="txtMeasurementNotes" width="100%" height="100%"
                                                        textMode="MultiLine" maxLength="2048" rows="9" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <div class="boxBtm">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="linkBtnSave" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:UpdatePanel runat="server" ID="upSpecialInvestigation" EnableViewState="false">
                    <ContentTemplate>
                        <div id="div_vSpecialInvestigation" class="bold" style="display: none">
                            <div class="btnWrap" align="right">
                                <table class="header">
                                    <tr>
                                        <td align="left">
                                            <input type="button" value="Delete" runat="server" id="btnDeletePatient6" style="width: 100px;"
                                                onclick="javascript:btnDelete_onclick();" onserverclick="btnDeletePatient_onserverclick" />
                                        </td>
                                        <td align="right">
                                            <input type="button" id="btnSavePatient6" runat="server" value="Save" style="width: 100px"
                                                onclick="javascript:controlBar_Buttons_OnClick(6);" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="wideBox">
                                <div class="boxTop">
                                    <h3 id="lblInvestigationRequired">
                                        Investigations Required</h3>
                                    <div class="addVisitDetails">
                                        <table class="full">
                                            <tr class="row01" style="font-weight: bold">
                                                <td style="width: 25%">
                                                    &nbsp;</td>
                                                <td align="center" style="width: 45%">
                                                    <label id="lblInvestigationComment">Comments</label></td>
                                                <td align="center" style="width: 10%">
                                                    <label id="lblInvestigationOrder">To Be Ordered</label></td>
                                                <td align="center" style="width: 10%">
                                                    <label id="lblInvestigationDone">Already Done</label></td>
                                                <td align="center" style="width: 10%">
                                                    <label id="lblInvestigationNotNeeded">Not Needed</label></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblRoutineBloodStudies">Routine blood studies</label></td>
                                                <td align="center">
                                                    <input type="text" runat="server" id="txtInvestigationRBS" size="65" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdInvestigationRBS" id="rdInvestigationRBST" value="T"
                                                        runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdInvestigationRBS" id="rdInvestigationRBSA" value="A"
                                                        runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdInvestigationRBS" id="rdInvestigationRBSN" value="N"
                                                        runat="server" /></td>
                                            </tr>
                                            <tr class="row01">
                                                <td>
                                                    <label id="lblAdditionalBloodStudies">Additional blood studies</label></td>
                                                <td align="center">
                                                    <input type="text" runat="server" id="txtInvestigationABS" size="65" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdInvestigationABS" id="rdInvestigationABST" value="T"
                                                        runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdInvestigationABS" id="rdInvestigationABSA" value="A"
                                                        runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdInvestigationABS" id="rdInvestigationABSN" value="N"
                                                        runat="server" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblRespiratoryTest">Respiratory function tests</label></td>
                                                <td align="center">
                                                    <input type="text" runat="server" id="txtInvestigationRFT" size="65" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdInvestigationRFT" id="rdInvestigationRFTT" value="T"
                                                        runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdInvestigationRFT" id="rdInvestigationRFTA" value="A"
                                                        runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdInvestigationRFT" id="rdInvestigationRFTN" value="N"
                                                        runat="server" /></td>
                                            </tr>
                                            <tr class="row01">
                                                <td>
                                                    <label id="lblArterialBlood">Arterial blood gases</label></td>
                                                <td align="center">
                                                    <input type="text" runat="server" id="txtInvestigationABG" size="65" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdInvestigationABG" id="rdInvestigationABGT" value="T"
                                                        runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdInvestigationABG" id="rdInvestigationABGA" value="A"
                                                        runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdInvestigationABG" id="rdInvestigationABGN" value="N"
                                                        runat="server" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblExerciseEKG">Exercise EKG testing</label></td>
                                                <td align="center">
                                                    <input type="text" runat="server" id="txtInvestigationEET" size="65" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdInvestigationEET" id="rdInvestigationEETT" value="T"
                                                        runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdInvestigationEET" id="rdInvestigationEETA" value="A"
                                                        runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdInvestigationEET" id="rdInvestigationEETN" value="N"
                                                        runat="server" /></td>
                                            </tr>
                                            <tr class="row01">
                                                <td>
                                                    <label id="lblBariumMeal">Barium meal</label></td>
                                                <td align="center">
                                                    <input type="text" runat="server" id="txtInvestigationBM" size="65" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdInvestigationBM" id="rdInvestigationBMT" value="T" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdInvestigationBM" id="rdInvestigationBMA" value="A" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdInvestigationBM" id="rdInvestigationBMN" value="N" runat="server" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblEsophageal">Esophageal manometry/pH studies</label></td>
                                                <td align="center">
                                                    <input type="text" runat="server" id="txtInvestigationEMS" size="65" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdInvestigationEMS" id="rdInvestigationEMST" value="T"
                                                        runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdInvestigationEMS" id="rdInvestigationEMSA" value="A"
                                                        runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdInvestigationEMS" id="rdInvestigationEMSN" value="N"
                                                        runat="server" /></td>
                                            </tr>
                                            <tr class="row01" style="display:none">
                                                <td>
                                                    <label id="lblPolysomnography">Polysomnography</label></td>
                                                <td align="center">
                                                    <input type="text" runat="server" id="txtInvestigationP" size="65" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdInvestigationP" id="rdInvestigationPT" value="T" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdInvestigationP" id="rdInvestigationPA" value="A" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdInvestigationP" id="rdInvestigationPN" value="N" runat="server" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblEKG">EKG</label></td>
                                                <td align="center">
                                                    <input type="text" runat="server" id="txtInvestigationEKG" size="65" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdInvestigationEKG" id="rdInvestigationEKGT" value="T"
                                                        runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdInvestigationEKG" id="rdInvestigationEKGA" value="A"
                                                        runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdInvestigationEKG" id="rdInvestigationEKGN" value="N"
                                                        runat="server" /></td>
                                            </tr>
                                            <tr class="row01">
                                                <td>
                                                    <label id="lblChestXray">Chest X-ray</label></td>
                                                <td align="center">
                                                    <input type="text" runat="server" id="txtInvestigationCX" size="65" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdInvestigationCX" id="rdInvestigationCXT" value="T" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdInvestigationCX" id="rdInvestigationCXA" value="A" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdInvestigationCX" id="rdInvestigationCXN" value="N" runat="server" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblSleepStudy">Sleep Study</label></td>
                                                <td align="center">
                                                    <input type="text" runat="server" id="txtSpecialInvestigationSSS" size="65" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdSpecialInvestigationSS" id="rdSpecialInvestigationSST"
                                                        value="T" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdSpecialInvestigationSS" id="rdSpecialInvestigationSSA"
                                                        value="A" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdSpecialInvestigationSS" id="rdSpecialInvestigationSSN"
                                                        value="N" runat="server" /></td>
                                            </tr>
                                            <tr class="row01">
                                                <td>
                                                    <label id="lblUpperGI">Upper GI Endoscopy</label></td>
                                                <td align="center">
                                                    <input type="text" runat="server" id="txtSpecialInvestigationGES" size="65" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdSpecialInvestigationGE" id="rdSpecialInvestigationGET"
                                                        value="T" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdSpecialInvestigationGE" id="rdSpecialInvestigationGEA"
                                                        value="A" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdSpecialInvestigationGE" id="rdSpecialInvestigationGEN"
                                                        value="N" runat="server" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblSpirometry">Spirometry</label></td>
                                                <td align="center">
                                                    <input type="text" runat="server" id="txtSpecialInvestigationSPS" size="65" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdSpecialInvestigationSP" id="rdSpecialInvestigationSPT"
                                                        value="T" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdSpecialInvestigationSP" id="rdSpecialInvestigationSPA"
                                                        value="A" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdSpecialInvestigationSP" id="rdSpecialInvestigationSPN"
                                                        value="N" runat="server" /></td>
                                            </tr>
                                            <tr class="row01">
                                                <td>
                                                    <label id="lblNutritionAssessment">Nutrition Assessment</label></td>
                                                <td align="center">
                                                    <input type="text" runat="server" id="txtSpecialInvestigationNAS" size="65" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdSpecialInvestigationNA" id="rdSpecialInvestigationNAT"
                                                        value="T" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdSpecialInvestigationNA" id="rdSpecialInvestigationNAA"
                                                        value="A" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdSpecialInvestigationNA" id="rdSpecialInvestigationNAN"
                                                        value="N" runat="server" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblPsychologicalAssessment">Psychological Assessment</label></td>
                                                <td align="center">
                                                    <input type="text" runat="server" id="txtSpecialInvestigationPSS" size="65" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdSpecialInvestigationPS" id="rdSpecialInvestigationPST"
                                                        value="T" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdSpecialInvestigationPS" id="rdSpecialInvestigationPSA"
                                                        value="A" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdSpecialInvestigationPS" id="rdSpecialInvestigationPSN"
                                                        value="N" runat="server" /></td>
                                            </tr>
                                            <tr class="row01">
                                                <td>
                                                    <label id="lblOtherAssessment">Other Assessments</label></td>
                                                <td align="center">
                                                    <input type="text" runat="server" id="txtSpecialInvestigationOAS" size="65" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdSpecialInvestigationOA" id="rdSpecialInvestigationOAT"
                                                        value="T" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdSpecialInvestigationOA" id="rdSpecialInvestigationOAA"
                                                        value="A" runat="server" /></td>
                                                <td align="center">
                                                    <input type="radio" name="rdSpecialInvestigationOA" id="rdSpecialInvestigationOAN"
                                                        value="N" runat="server" /></td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div class="boxBtm">
                                </div>
                            </div>
                            <div class="wideBox">
                                <div class="boxTop">
                                    <h3 id="lblReferrals">
                                        Referrals</h3>
                                    <div class="addVisitDetails">
                                        <table class="full">
                                            <tr>
                                                <td style="width: 25%">
                                                    <label id="lblPsychiatryAssessment">Psychiatry assessment</label></td>
                                                <td style="width: 75%">
                                                    <input type="checkbox" id="chkReferralsPA" runat="server" onclick="javascript:checkTextBox('ReferralsPA','check');" />
                                                    &nbsp;
                                                    <input type="text" runat="server" id="txtReferralsPA" size="95" onkeyup="javascript:checkTextBox('ReferralsPA','input');" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblCardiologist">Cardiologist</label></td>
                                                <td>
                                                    <input type="checkbox" id="chkReferralsC" runat="server" onclick="javascript:checkTextBox('ReferralsC','check');" />
                                                    &nbsp;
                                                    <input type="text" runat="server" id="txtReferralsC" size="95" onkeyup="javascript:checkTextBox('ReferralsC','input');" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblRespiratoryPhysician">Respiratory physician</label></td>
                                                <td>
                                                    <input type="checkbox" id="chkReferralsRP" runat="server" onclick="javascript:checkTextBox('ReferralsRP','check');" />
                                                    &nbsp;
                                                    <input type="text" runat="server" id="txtReferralsRP" size="95" onkeyup="javascript:checkTextBox('ReferralsRP','input');" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblEndocrinologist">Endocrinologist</label></td>
                                                <td>
                                                    <input type="checkbox" id="chkReferralsE" runat="server" onclick="javascript:checkTextBox('ReferralsE','check');" />
                                                    &nbsp;
                                                    <input type="text" runat="server" id="txtReferralsE" size="95" onkeyup="javascript:checkTextBox('ReferralsE','input');" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblAdolescentPhysician">Adolescent physician</label></td>
                                                <td>
                                                    <input type="checkbox" id="chkReferralsAP" runat="server" onclick="javascript:checkTextBox('ReferralsAP','check');" />
                                                    &nbsp;
                                                    <input type="text" runat="server" id="txtReferralsAP" size="95" onkeyup="javascript:checkTextBox('ReferralsAP','input');" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblGeneralMedical">General Medical</label></td>
                                                <td>
                                                    <input type="checkbox" id="chkReferralsGM" runat="server" onclick="javascript:checkTextBox('ReferralsGM','check');" />
                                                    &nbsp;
                                                    <input type="text" runat="server" id="txtReferralsGM" size="95" onkeyup="javascript:checkTextBox('ReferralsGM','input');" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblOtherReferrals">Other</label></td>
                                                <td>
                                                    <input type="checkbox" id="chkReferralsO" runat="server" onclick="javascript:checkTextBox('ReferralsO','check');" />
                                                    &nbsp;
                                                    <input type="text" runat="server" id="txtReferralsO" size="95" onkeyup="javascript:checkTextBox('ReferralsO','input');" /></td>
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
                        <asp:AsyncPostBackTrigger ControlID="linkBtnSave" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:UpdatePanel runat="server" ID="upManagement" EnableViewState="false">
                    <ContentTemplate>
                        <div id="div_vManagement" class="bold" style="display: none">
                            <div class="btnWrap" align="right">
                                <table class="header">
                                    <tr>
                                        <td align="left">
                                            <input type="button" value="Delete" runat="server" id="btnDeletePatient16" style="width: 100px;"
                                                onclick="javascript:btnDelete_onclick();" onserverclick="btnDeletePatient_onserverclick" />
                                        </td>
                                        <td align="right">
                                            <input type="button" id="btnSavePatient16" runat="server" value="Save" style="width: 100px"
                                                onclick="javascript:controlBar_Buttons_OnClick(16);" /></td>
                                    </tr>
                                </table>
                            </div>
                            <div class="wideBox">
                                <div class="boxTop">
                                    <h3 id="lblManagementPlanTitle">
                                        Management Plan</h3>
                                    <div class="addVisitDetails">
                                        <table>
                                            <tr>
                                                <td>
                                                    <label id="lblManagementPlanTitle2">Management Plan</label>
                                                    <wucTextBox:TextBox ID="txtManagement" runat="server" textMode="MultiLine" rows="10"
                                                        width="900px" />
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
                        <asp:AsyncPostBackTrigger ControlID="linkBtnSave" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                <div id="upReport" class="bold" style="display: none">
                    <div class="btnWrap" align="right">
                        <table class="header">
                            <tr>
                                <td align="left">
                                    <input type="button" value="Delete" runat="server" id="btnDeletePatient17" style="width: 100px;"
                                        onclick="javascript:btnDelete_onclick();" onserverclick="btnDeletePatient_onserverclick" />
                                </td>
                                <td align="right">
                                    <input type="button" id="btnSavePatient17" runat="server" value="Save" style="width: 100px"
                                        onclick="javascript:controlBar_Buttons_OnClick(17);" /></td>
                            </tr>
                        </table>
                    </div>
                    <div class="wideBox">
                        <div class="boxTop">
                            <h3>
                                Medical Report</h3>
                            <div class="addVisitDetails">
                                <table class="full">
                                    <tr>
                                        <td style="width: 130px">
                                            <label id="lblReportFormat">Select report format</label> :
                                        </td>
                                        <td>
                                            <select id="cmbReportFormat" style="display: block; width: 200px">
                                                <option value="4" selected="true">Word Document</option>
                                                <option value="3">PDF</option>
                                            </select>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="rowLogo">
                                        <td>
                                            <label id="lblLogo">Logo</label> :
                                        </td>
                                        <td>
                                            <wucLogo:LogoList runat="Server" ID="cmbLogo" Width='40'/>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div id="divReportTemplates" style="display:none">
                                                <div>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="font-weight: bold">
                                                                EMR Report:
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <select id="templateDemographic" style="width: 150px" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Demographic
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <select id="templateMeasurement" style="width: 150px" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Measurement
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <select id="templateComplaint" style="width: 150px" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Presenting Complaint
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <select id="templateComorbidities" style="width: 150px" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Comorbidities
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <select id="templateMedications" style="width: 150px" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Medications
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <select id="templateWeightLossHistory" style="width: 150px" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Weight Loss History
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <select id="templatePastFamilyHistory" style="width: 150px" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Past & Family History
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <select id="templateAllergies" style="width: 150px" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Allergies
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <select id="templatePhysicalExamination" style="width: 150px" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Physical Examination
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <select id="templateLabs" style="width: 150px" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Labs
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <select id="templateInvestigationsRefferals" style="width: 150px" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label id="lblInvestigation">Investigations & Referrals</label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <select id="templateManagementPlan" style="width: 150px" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label id="lblManagementPlan">Management Plan</label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan='2'>
                                            <!--<iframe id='frameReport' style='width: 0; height: 0; visibility: hidden'></iframe>-->
                                            <div id='divPrintBtns' style='display: block' class='printEMRButtons'>
                                                <input type='button' id='btnPreview' value='Preview' onclick='javascript:BuildReport(1);'
                                                    style='width: 80px' />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="boxBtm">
                        </div>
                    </div>
                </div>
                <div class="clr">
                </div>
            </div>
            <asp:UpdatePanel runat="Server" ID="up_HiddenFields" EnableViewState="false">
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
                    <asp:HiddenField runat="server" ID="txtHSaveResult" Value="0" />
                    <asp:DropDownList runat="server" ID="cmbReferredDoctorsList" Style="display: none" />
                    <asp:HiddenField runat="server" ID="txtHApplicationURL" />
                    <asp:HiddenField runat="server" ID="txtHLapbandDate" Value="0" />
                    <asp:DropDownList runat="server" ID="cmbCity" Style="display: none" />
                    <asp:DropDownList runat="server" ID="cmbInsurance" Style="display: none" />
                    <asp:HiddenField runat="server" ID="txtUseImperial" Value="0" />
                    <asp:HiddenField runat="server" ID="txtHIdealWeight" Value="0" />
                    <asp:HiddenField runat="server" ID="txtHTargetWeight" Value="0" />
                    <asp:HiddenField runat="Server" ID="txtHRefBMI" Value="0" />
                    <asp:HiddenField runat="Server" ID="txtHTargetBMI" Value="0" />
                    <asp:HiddenField runat="server" ID="txtHPermissionLevel" />
                    <asp:HiddenField runat="server" ID="txtHDataClamp" />
                    <asp:HiddenField runat="Server" ID="txtHCulture" />
                    <asp:HiddenField runat="Server" ID="TitleLoaded" Value="0" />
                    <asp:HiddenField runat="server" ID="txtHWeightHistoryGainWeight" Value="0" />
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
                    <asp:HiddenField runat="server" ID="txtHLogoMandatory" Value="0"/>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="linkBtnSave" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>