<%@ page language="C#" autoeventwireup="true" inherits="Forms_MenuSetup_MenuSetupForm, Lapbase.deploy" validaterequest="false" enableEventValidation="false" viewStateEncryptionMode="Always" %>

<%@ Register TagPrefix="wucSystemCode" TagName="SystemCodeList" Src="~/UserControl/SystemCodeWUCtrl.ascx" %>
<%@ Register TagPrefix="wucCode" TagName="CodeList" Src="~/UserControl/CodeWUCtrl.ascx" %>
<%@ Register TagPrefix="wucTextBox" TagName="TextBox" Src="~/UserControl/TextBoxWUCtrl.ascx" %>
<%@ Register TagPrefix="wucTextArea" TagName="TextArea" Src="~/UserControl/TextAreaWUCtrl.ascx" %>
<%@ Register TagPrefix="wucRegion" TagName="RegionList" Src="~/UserControl/RegionListWUCtrl.ascx" %>
<%@ Register TagPrefix="wucMenu" TagName="Menu" Src="~/UserControl/MenuWUCtrl.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>LapBase - A Data Manager for Bariatric Surgery</title>
    <link href='<%$ AppSettings:CssUrl%>' rel="stylesheet" type="text/css" />
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />

    <script language="javascript" type="text/javascript" src="../../Scripts/Global.js"></script>

    <script language="javascript" type="text/javascript" src="Includes/MenuSetupForm.js"></script>

</head>
<body runat="server" id="bodyMenuSetup">
    <wucMenu:Menu runat="server" ID="mainMenu" />
    <div class="tabMenus">
        <div class="greyTabMenu">
            <ul>
                <%--<li id = "li_Home"><a id= "a_Home" href="~/Forms/PatientsVisits/PatientsVisitsForm.aspx" runat = "server" >Patient List</a></li>--%>
                <li id="li_Div1"><a id="a_Doctors" href="#" onclick="javascript:LoadDiv(1);">Doctors</a></li>
                <li id="li_Div2"><a id="a_Hospitals" href="#" onclick="javascript:LoadDiv(2);">Hospitals</a></li>
                <li id="li_Div3"><a id="a_ReferringDoctors" href="#" onclick="javascript:LoadDiv(3);">
                    Ref. Doctors</a></li>
                <%--<li id = "li_Div4"><a href="#" onclick="javascript:LoadDiv(4);">Ideal Weight</a></li>--%>
                <li id="li_Div5"><a id="a_Groups" href="#" onclick="javascript:LoadDiv(5);">Groups</a></li>
                <li id="li_Div9"><a id="a_Category" href="#" onclick="javascript:LoadDiv(9);">Category</a></li>
                <li id="li_Div13"><a id="a_Region" href="#" onclick="javascript:LoadDiv(13);">Regions</a></li>
            </ul>
        </div>
        <div class="greyTabMenu">
            <ul>
                <li id="li_Div11"><a id="a_Biochemistry" href="#" onclick="javascript:LoadDiv(11);">
                    Biochemistry</a></li>
                <li id="li_Div10" runat="server"><a id="a_Deleted" href="#" onclick="javascript:LoadDiv(10);">
                    Deleted</a></li>
                <li id="li_Div6" class="current"><a id="a_UserFields" href="#" onclick="javascript:LoadDiv(6);">
                    User's Fields</a></li>
                <li id="li_Div12"><a id="a_Template" href="#" onclick="javascript:LoadDiv(12);">Template</a></li>
                <li id="li_Div14" runat="server"><a id="a_Logo" href="#" onclick="javascript:LoadDiv(14);">Logo</a></li>
                <%--<li id = "li_Div8"><a id= "a_import" href="#" onclick="javascript:LoadDiv(8);">Import Patient</a></li>--%>
                <li id="li_Div7"><a id="a_UserSettings" href="#" onclick="javascript:LoadDiv(7);">Settings</a></li>
                <li id="li_Div15" runat="server" style="display:none"><a id="a_ActionLog" href="#" onclick="javascript:LoadDiv(15);">Action Log</a></li>
            </ul>
        </div>
        <div class="manilaTabMenu">
            <ul>
                <li id="li_SubDiv1" style="display: none"><a href="#" onclick="javascript:LoadSubDiv(1);"
                    id="a_DoctorData">Doctor's Data</a></li>
                <li id="li_SubDiv2" style="display: none"><a href="#" onclick="javascript:LoadSubDiv(2);"
                    id="a_AllDoctors">All Doctors</a></li>
                <li id="li_SubDiv3" style="display: none"><a href="#" onclick="javascript:LoadSubDiv(3);"
                    id="a_RefDoctorsDaya">Ref. Doctor's Data</a></li>
                <li id="li_SubDiv4" style="display: none"><a href="#" onclick="javascript:LoadSubDiv(4);"
                    id="a_AllRefDoctors">All Ref. Doctors</a></li>
                <li id="li_SubDiv5" style="display: none"><a href="#" onclick="javascript:LoadSubDiv(5);"
                    id="a_TemplateData">Template</a></li>
                <li id="li_SubDiv6" style="display: none"><a href="#" onclick="javascript:LoadSubDiv(6);"
                    id="a_AllTemplates">All Templates</a></li>
                <li id="li_SubDiv7" style="display: none"><a href="#" onclick="javascript:LoadSubDiv(7);"
                    id="a_ReportTemplates">Report Templates</a></li>
                <li id="li_SubDiv8" style="display: none"><a href="#" onclick="javascript:LoadSubDiv(8);"
                    id="a_LogoData">Logo</a></li>
                <li id="li_SubDiv9" style="display: none"><a href="#" onclick="javascript:LoadSubDiv(9);"
                    id="a_AllLogos">All Logos</a></li>
            </ul>
        </div>
    </div>
    <div>
        &nbsp; &nbsp; &nbsp; Please refresh the page to update the dropdown listing
    </div>
    <div id="divDeletedPatientNote" style="display:none">
        &nbsp; &nbsp; &nbsp; This list contains the deleted patient in the last 6 months
    </div>
    <form id="frmMenuSetup" runat="server" enctype="multipart/form-data">
        <asp:ScriptManager runat="server" ID="smMenuSetup" AsyncPostBackTimeout="60000" />
        <div class="contentArea">
            <div id="divErrorMessage" style="display: none;" runat="server">
                <span>
                    <p id="pErrorMessage">
                    </p>
                </span>
            </div>
            <div class="greyContentWrap">
                <div class="settings">
                    <div class="generalSettings">
                        <div class="boxTop">
                            <div id="divDoctor" style="display: none">
                                <asp:UpdatePanel runat="server" ID="up_Doctor" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%" border="0">
                                            <tr>
                                                <td colspan="6" style="text-align: right">
                                                    <input type="button" id="btnAddDoctor" style="width: 150px" value="Add new doctor"
                                                        onclick="javascript:btnAddDoctor_onclick();" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 10%">
                                                    <asp:Label runat="Server" ID="lblSurName" Text="SurName" /></td>
                                                <td style="width: 30%">
                                                    <wucTextBox:TextBox runat="server" ID="txtSurName" maxLength="50" width="90%" />
                                                </td>
                                                <td style="width: 10%">
                                                    <asp:Label runat="Server" ID="lblFirstName" Text="First Name" /></td>
                                                <td style="width: 30%">
                                                    <wucTextBox:TextBox runat="server" ID="txtFirstName" maxLength="50" width="90%" />
                                                </td>
                                                <td style="width: 10%">
                                                    <asp:Label runat="server" ID="lblTitle" Text="Title" /></td>
                                                <td style="width: 10%">
                                                    <wucTextBox:TextBox runat="server" ID="txtTitle" maxLength="10" width="90%" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="Server" ID="lblDoctorName" Text="Doctor Name" /></td>
                                                <td>
                                                    <asp:Label runat="Server" ID="lblDoctorName_Value" /></td>
                                                <td>
                                                    <asp:Label runat="Server" ID="lblDoctorBoldCode" Text="Bold Code" /></td>
                                                <td>
                                                    <wucTextBox:TextBox runat="server" ID="txtDoctorBoldCode" maxLength="50" width="90%" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="Server" ID="lblSpeciality" Text="Speciality" /></td>
                                                <td>
                                                    <wucTextBox:TextBox runat="server" ID="txtSpeciality" maxLength="50" width="90%" />
                                                </td>
                                                <td>
                                                    <asp:Label runat="Server" ID="lblDegrees" Text="Degrees" /></td>
                                                <td>
                                                    <wucTextBox:TextBox runat="server" ID="txtDegrees" maxLength="50" width="90%" />
                                                </td>
                                                <td colspan="2">
                                                    <asp:CheckBox runat="server" ID="chkIsSurgeon" />&nbsp;<asp:Label runat="server"
                                                        ID="lblIsSurgeon" Text="Is Surgeon?" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="Server" ID="lblAddress1" Text="Address1" /></td>
                                                <td>
                                                    <wucTextBox:TextBox runat="server" ID="txtAddress1" maxLength="80" width="90%" />
                                                </td>
                                                <td colspan="2">
                                                    &nbsp;</td>
                                                <td colspan="2">
                                                    <asp:CheckBox runat="server" ID="chkIsHide" Checked="false" />&nbsp;<asp:Label runat="server"
                                                        ID="lblIsHide" Text="Delete?" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="Server" ID="lblAddress2" Text="Address2" /></td>
                                                <td>
                                                    <wucTextBox:TextBox runat="server" ID="txtAddress2" maxLength="80" width="90%" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="Server" ID="lblCity" Text="City" /></td>
                                                <td>
                                                    <wucTextBox:TextBox runat="server" ID="txtSuburb" maxLength="50" width="90%" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="Server" ID="lblState" Text="State" /></td>
                                                <td>
                                                    <wucTextBox:TextBox runat="server" ID="txtState" maxLength="10" width="90%" />
                                                </td>
                                                <td>
                                                    <asp:Label runat="Server" ID="lblPostCode" Text="Post Code" /></td>
                                                <td>
                                                    <wucTextBox:TextBox runat="server" ID="txtPostCode" maxLength="10" width="90%" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6" style="font-weight: bold; font-size: medium">
                                                    <asp:Label runat="server" ID="lblUsualOperationDetail" Text="Usual Operation Details..." /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="lblOperation" Text="Operation" /></td>
                                                <td>
                                                    <wucSystemCode:SystemCodeList runat="server" ID="cmbSurgeryType" Width="90" CriteriaString="BST"
                                                        BoldData="BST" />
                                                </td>
                                                <td colspan="2">
                                                    <asp:Label runat="Server" Text="Use Own LetterHead" ID="lblUseOwnLetterHead" />&nbsp;
                                                    <asp:CheckBox runat="server" ID="chkUseOwnLetterHead" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="lblApproach" Text="Approach" /></td>
                                                <td>
                                                    <wucSystemCode:SystemCodeList runat="server" ID="cmbApproach" Width="85" CriteriaString="Approach"
                                                        BoldData="Approach" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="lblCategory" Text="Category" /></td>
                                                <td>
                                                    <wucCode:CodeList runat="server" ID="cmbCategory" Width="90" CriteriaString="PC" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="lblGroup" Text="Group" /></td>
                                                <td>
                                                    <wucCode:CodeList runat="server" ID="cmbGroup" Width="90" CriteriaString="GRO" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="linkBtnDoctorSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="linkBtnDoctorLoad" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                            <div id="divAllDoctors" style="display: none">
                                <div>
                                    <table style="width: 100%">
                                        <tr style="height: 30px">
                                            <td id="lblRow_TC" style="width: 4%">
                                                Row</td>
                                            <td id="lblDoctorName_TC" style="width: 13%">
                                                Doctor Name</td>
                                            <td id="lblSpeciality_TC" style="width: 11%">
                                                Speciality</td>
                                            <td id="lblDegrees_TC" style="width: 10%">
                                                Degrees</td>
                                            <td id="lblOperation_TC" style="width: 25%">
                                                Operation</td>
                                            <td id="lblApproach_TC" style="width: 15%">
                                                Approach</td>
                                            <td id="lblCategory_TC" style="width: 7%" runat="server">
                                                Category</td>
                                            <td id="btnLinkAddNewDoctor" runat="server" style="width: 15%">
                                                <input type="button" value="Add new doctor" onclick="javascript:btnAddNewDoctor();" /></td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="div_tblDoctorsList" style="overflow-y: scroll; width: 100%; height: 250px">
                                </div>
                            </div>
                            <div id="divHospitals" style="display: none; height: 250px">
                                <table width="100%">
                                    <tr>
                                        <td style="width: 5%">
                                            <asp:Label runat="Server" Text="ID" ID="lblHospitalID" />
                                        </td>
                                        <td style="width: 7%">
                                            <asp:HiddenField runat="server" ID="txtHHospitalID" Value="" />
                                            <wucTextBox:TextBox runat="server" ID="txtHospitalID" width="80%" maxLength="6" />
                                        </td>
                                        <td style="width: 10%">
                                            <asp:Label runat="Server" Text="Name" ID="lblHospitalNames" />
                                        </td>
                                        <td style="width: 30%">
                                            <wucTextBox:TextBox runat="server" ID="txtHospitalName" width="90%" maxLength="50" />
                                        </td>
                                        <td style="width: 8%">
                                            <asp:Label runat="Server" Text="Bold Code" ID="lblHospitalBoldCode" />
                                        </td>
                                        <td style="width: 12%">
                                            <asp:HiddenField runat="server" ID="txtHHospitalBoldCode" Value="" />
                                            <wucTextBox:TextBox runat="server" ID="txtHospitalBoldCode"/>
                                        </td>
                                        <td style="width: 8%">
                                            <asp:Label runat="Server" Text="Region" ID="lblHospitalRegion" />
                                        </td>
                                        <td style="width: 20%">
                                            <wucRegion:RegionList runat="Server" ID="cmbRegion"/>
                                        </td>
                                        <td id="btnHospital" runat="server">
                                            <input type="button" id="btnAddHospital" onclick="javascript:btnAddHospital_onclick();"
                                                value="Add new hospital" style="width: 150px" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <div id="div_tblHospitalList" style="overflow-y: scroll">
                                </div>
                            </div>
                            <div id="divRegions" style="display: none; height: 250px">
                                <table width="100%">
                                    <tr>
                                        <td style="width: 10%">
                                            <asp:Label runat="Server" Text="Region ID" ID="lblRegionID" />
                                        </td>
                                        <td style="width: 15%">
                                            <asp:HiddenField runat="server" ID="txtHRegionID" Value="" />
                                            <wucTextBox:TextBox runat="server" ID="txtRegionID" width="80%" maxLength="6" />
                                        </td>
                                        <td style="width: 15%">
                                            <asp:Label runat="Server" Text="Region Name" ID="lblRegionName" />
                                        </td>
                                        <td style="width: 30%">
                                            <wucTextBox:TextBox runat="server" ID="txtRegionName" width="90%" maxLength="50" />
                                        </td>
                                        <td id="btnRegion" runat="server" align="right">
                                            <input type="button" id="btnAddRegion" onclick="javascript:btnAddRegion_onclick();"
                                                value="Add new Region" style="width: 150px" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <div id="div_tblRegionList" style="overflow-y: scroll">
                                </div>
                            </div>
                            <div id="divRefDoctor" style="display: none">
                                <asp:UpdatePanel runat="server" ID="up_RefDoctors" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%" border="0">
                                            <tr>
                                                <td colspan="6" style="text-align: right">
                                                    <input type="button" id="btnAddRefDoctor" style="width: 150px" value="Add new doctor"
                                                        onclick="javascript:btnAddRefDoctor_onclick();" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 10%">
                                                    <asp:Label runat="Server" ID="lblRefSurName" Text="Surname" /></td>
                                                <td style="width: 30%">
                                                    <wucTextBox:TextBox runat="server" ID="txtRefSurName" maxLength="50" width="90%" />
                                                </td>
                                                <td style="width: 10%">
                                                    <asp:Label runat="Server" ID="lblRefFirstName" Text="First Name" /></td>
                                                <td style="width: 30%">
                                                    <wucTextBox:TextBox runat="server" ID="txtRefFirstName" maxLength="50" width="90%" />
                                                </td>
                                                <td style="width: 10%">
                                                    <asp:Label runat="server" ID="lblRefTitle" Text="Title" /></td>
                                                <td style="width: 10%">
                                                    <wucTextBox:TextBox runat="server" ID="txtRefTitle" maxLength="10" width="90%" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="Server" ID="lblRefDoctorName" Text="Doctor Name" /></td>
                                                <td colspan="2">
                                                    <asp:Label runat="Server" ID="lblRefDoctorName_Value" /></td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblUseFirst" Text="Use First" />
                                                    &nbsp;
                                                    <asp:CheckBox runat="server" ID="chkUseFirst" />
                                                </td>
                                                <td colspan="2">
                                                    <asp:Label runat="server" ID="lblIsRefHide" Text="Delete?" />&nbsp;<asp:CheckBox
                                                        runat="server" ID="chkIsRefHide" Checked="false" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="Server" ID="lblRefPhone" Text="Phone" /></td>
                                                <td>
                                                    <wucTextBox:TextBox runat="server" ID="txtRefPhone" maxLength="50" width="90%" />
                                                </td>
                                                <td>
                                                    <asp:Label runat="Server" ID="lblRefFax" Text="Fax" /></td>
                                                <td>
                                                    <wucTextBox:TextBox runat="server" ID="txtRefFax" maxLength="50" width="90%" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="Server" ID="lblRefAddress1" Text="Address1" /></td>
                                                <td>
                                                    <wucTextBox:TextBox runat="server" ID="txtRefAddress1" maxLength="80" width="90%" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="Server" ID="lblRefAddress2" Text="Address2" /></td>
                                                <td>
                                                    <wucTextBox:TextBox runat="server" ID="txtRefAddress2" maxLength="80" width="90%" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="Server" ID="lblRefCity" Text="City" /></td>
                                                <td>
                                                    <wucTextBox:TextBox runat="server" ID="txtRefSuburb" maxLength="50" width="90%" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="Server" ID="lblRefState" Text="State" /></td>
                                                <td>
                                                    <wucTextBox:TextBox runat="server" ID="txtRefState" maxLength="10" width="90%" />
                                                </td>
                                                <td>
                                                    <asp:Label runat="Server" ID="lblRefPostCode" Text="Post Code" /></td>
                                                <td>
                                                    <wucTextBox:TextBox runat="server" ID="txtRefPostCode" maxLength="10" width="90%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="linkBtnRefDoctorSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="linkBtnRefDoctorLoad" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                            <div id="divAllRefDoctors" style="display: none">
                                <div>
                                    <table style="width: 98%">
                                        <tr style="text-align: left; height: 30px">
                                            <td style="width: 5%" id="lblRow_AllRefDoctors">
                                                Row</td>
                                            <td style="width: 45%" id="lblDoctorName_AllRefDoctors">
                                                Doctor Name</td>
                                            <td style="width: 40%">
                                                <asp:Label ID="lblRefSurName_Search" runat="Server" Text="Search by surname" />
                                                &nbsp;
                                                <wucTextBox:TextBox ID="txtRefSurName_Search" runat="Server" width="30%" maxLength="50" />
                                                &nbsp;
                                                <input type="button" id="btnRefSurName_Search" value="search" onclick="javascript:btnRefSurName_Search_OnClick();"
                                                    style="display: none" />
                                            </td>
                                            <td style="width: 8%">
                                                <input type="button" runat="server" id="btnLinkAddNewRefDoctor" value="Add new doctor"
                                                    onclick="javascript:btnAddNewRefDoctor();" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="div_tblRefDoctorsList" style="overflow: scroll; width: 100%; height: 250px">
                                </div>
                            </div>
                            <div id="divGroups" style="display: none; height: 250px">
                                <table width="100%">
                                    <tr>
                                        <td style="width: 10%">
                                            <asp:Label ID="lblCode_Group" runat="Server" Text="Code" />
                                        </td>
                                        <td style="width: 30%">
                                            <asp:HiddenField runat="server" ID="txtHGroupCode" />
                                            <wucTextBox:TextBox runat="server" ID="txtCode_Group" maxLength="6" width="100px"
                                                Text="0" />
                                        </td>
                                        <td style="width: 50%">
                                            <asp:Label ID="lblDescription_Group" runat="Server" Text="Description" />
                                            <wucTextBox:TextBox runat="server" ID="txtDescription_Group" maxLength="100" width="60%" />
                                        </td>
                                        <td id="btnGroup" runat="server" style="width: 10%" align="right">
                                            <input type="button" id="btnAddGroup" value="Add new group" onclick="javascript:btnAddGroup_onclick();"
                                                style="width: 150px" runat="server" /></td>
                                    </tr>
                                </table>
                                <br />
                                <table style="width: 100%" cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td style="width: 5%" />
                                        <td style="width: 10%" id="lblCode_Group_TC">
                                            Code</td>
                                        <td style="width: 40%" id="lblDescription_Group_TC">
                                            Description</td>
                                        <td style="width: 45%" />
                                    </tr>
                                </table>
                                <div id="div_tblGroupsList" style="overflow-y: scroll">
                                </div>
                            </div>
                            <div id="divCategory" style="display: none; height: 250px">
                                <table width="100%">
                                    <tr>
                                        <td style="width: 10%">
                                            <asp:Label ID="lblCode_Category" runat="Server" Text="Code" />
                                        </td>
                                        <td style="width: 30%">
                                            <asp:HiddenField runat="server" ID="txtHCategoryCode" />
                                            <wucTextBox:TextBox runat="server" ID="txtCode_Category" maxLength="6" width="100px"
                                                Text="0" />
                                        </td>
                                        <td style="width: 50%">
                                            <asp:Label ID="lblDescription_Category" runat="Server" Text="Description" />
                                            <wucTextBox:TextBox runat="server" ID="txtDescription_Category" maxLength="100" width="60%" />
                                        </td>
                                        <td id="btnCategory" runat="server" style="width: 10%" align="right">
                                            <input type="button" id="btnAddCategory" value="Add new category" onclick="javascript:btnAddCategory_onclick();"
                                                style="width: 150px" runat="server" /></td>
                                    </tr>
                                </table>
                                <br />
                                <table style="width: 100%" cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td style="width: 5%" />
                                        <td style="width: 10%" id="lblCode_Category_TC">
                                            Code</td>
                                        <td style="width: 40%" id="lblDescription_Category_TC">
                                            Description</td>
                                        <td style="width: 45%" />
                                    </tr>
                                </table>
                                <div id="div_tblCategoryList" style="overflow-y: scroll">
                                </div>
                            </div>
                            <div id="divBiochemistry" style="display: none; height: 500px">
                                <div>
                                    <table style="width: 100%">
                                        <tr style="height: 30px">
                                            <td style="width: 100%" align="right">
                                                <input type="button" id="btnSaveBiochemistry" value="Save" onclick="javascript:btnAddSaveBiochemistry_onclick();"
                                                    runat="server" /></td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="divBiochemistryList" runat="server">
                                </div>
                            </div>
                            <div id="divDeletedPatient" style="display: none; height: 250px">
                                <div>
                                    <table style="width: 100%">
                                        <tr style="height: 30px">
                                            <td style="width: 10%" id="lblPatient_ID_TC">
                                                ID</td>
                                            <td style="width: 20%" id="lblPatient_Name_TC">
                                                Patient Name</td>
                                            <td style="width: 20%" id="lblPatient_DOB_TC">
                                                DOB</td>
                                            <td style="width: 20%" id="lblPatient_DeletedBy_TC">
                                                Deleted By</td>
                                            <td style="width: 20%" id="lblPatient_DateDeleted_TC">
                                                Date Deleted</td>
                                            <td style="width: 10%" id="lblPatient_Activate_TC">
                                                &nbsp;</td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="div_tblDeletedPatientList" style="overflow-y: scroll">
                                </div>
                            </div>
                            <div id="divUserFields" style="display: block">
                                <asp:UpdatePanel runat="server" ID="up_UserFields" UpdateMode="conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td colspan="10" style="text-align: right">
                                                    <input type="button" id="btnAddUserFields" value="Update data" style="width: 150px"
                                                        onclick="javascript:btnAddUserFields_onclick();" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" style="font-weight: bolder">
                                                    <asp:Label runat="server" ID="lblPatientFormOptions" Text="Patient Form Options" />
                                                </td>
                                                <td style="width: 5%" rowspan="12" />
                                                <td style="width: 8%; font-weight: bolder">
                                                    <asp:Label runat="server" ID="lblFacility" Text="Facility" /></td>
                                                <td style="width: 42%" />
                                            </tr>
                                            <tr>
                                                <td style="width: 18%">
                                                    <asp:Label runat="server" ID="lblTargetBMI" Text="Target BMI" />
                                                </td>
                                                <td style="width: 7%">
                                                    <wucTextBox:TextBox runat="server" ID="txtTargetBMI" maxLength="3" width="90%" />
                                                </td>
                                                <td style="width: 20%">
                                                    <asp:Label runat="Server" ID="lblTargetBMIUsed" Text="BMI used to calculate Target Weight" />
                                                </td>
                                                <td>
                                                    Name
                                                </td>
                                                <td>
                                                    <wucTextBox:TextBox runat="server" ID="txtFacilityName" width="70%" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="lblRefBMI" Text="Reference BMI" />
                                                </td>
                                                <td>
                                                    <wucTextBox:TextBox runat="server" ID="txtRefBMI" maxLength="3" width="90%" />
                                                </td>
                                                <td>
                                                    <asp:Label runat="Server" ID="lblRefBMIUsed" Text="BMI used to calculate Ideal Weight" />
                                                </td>
                                                <td>
                                                    Address
                                                </td>
                                                <td>
                                                    <wucTextBox:TextBox runat="server" ID="txtFacilityAddress" width="70%" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="lblBaseIW_BMI" Text="Base Ideal Weight on BMI" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox runat="server" ID="chkBaseIW_BMI" />
                                                </td>
                                                <td>
                                                    <asp:Label runat="Server" ID="lblIWBMIUsed" Text="Use BMI instead of Height tables" />
                                                </td>
                                                <td>
                                                    Suburb
                                                </td>
                                                <td>
                                                    <wucTextBox:TextBox runat="server" ID="txtFacilitySuburb" width="20%" />
                                                    &nbsp; State &nbsp;
                                                    <wucTextBox:TextBox runat="server" ID="txtFacilityState" width="20%" />
                                                    &nbsp; Postcode &nbsp;
                                                    <wucTextBox:TextBox runat="server" ID="txtFacilityPostcode" width="15%" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label ID="lblShowRace" runat="Server" Text="Show Race " />&nbsp;
                                                    <asp:CheckBox ID="chkShowRace" runat="server" />&nbsp;&nbsp;
                                                    <asp:Label ID="lblShowComorbidity" runat="Server" Text="Show Comorbidity" />&nbsp;
                                                    <asp:CheckBox ID="chkShowComorbidity" runat="server" />&nbsp;&nbsp;
                                                    <asp:Label ID="lblShowInvestigations" runat="Server" Text="Show Comorbidity" />&nbsp;
                                                    <asp:CheckBox ID="chkShowInvestigations" runat="server" />
                                                </td>
                                                <td>
                                                    Phone
                                                </td>
                                                <td>
                                                    <wucTextBox:TextBox runat="server" ID="txtFacilityPhone" width="25%" />
                                                    &nbsp; Fax &nbsp;
                                                    <wucTextBox:TextBox runat="server" ID="txtFacilityFax" width="25%" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label ID="lblAutoSave" runat="Server" Text="Auto Save " />&nbsp;
                                                    <asp:CheckBox ID="chkAutoSave" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" style="font-weight: bolder">
                                                    <asp:Label ID="lblVisitFormOptions" runat="Server" Text="Visit Form Options" />&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblComorbidity" runat="Server" Text="Comorbidity" />&nbsp;
                                                </td>
                                                <td>
                                                    <asp:CheckBox runat="Server" ID="chkComorbidity" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblProgressVisits" runat="Server" Text="Progress Visits" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox runat="Server" ID="chkProgressVisits" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblInvesigations" runat="Server" Text="Invesigations" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox runat="Server" ID="chkInvesigations" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblImperial" runat="Server" Text="Imperial Mode" />&nbsp;
                                                </td>
                                                <td>
                                                    <asp:CheckBox runat="Server" ID="chkImperial" />
                                                </td>
                                            </tr>
                                            <tr id="rowVisitWeeksFlag" runat="server" style="display:none">
                                                <td>
                                                    <asp:Label ID="lblVisitWeeksFlag" runat="Server" Text="Calculate visit weeks from" />&nbsp;
                                                </td>
                                                <td>
                                                    <wucSystemCode:SystemCodeList runat="server" ID="cmbVisitWeeks" Width="300" CriteriaString="VISITWEEK"
                                                        FirstRow="false" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" style="font-weight: bolder">
                                                    <asp:Label ID="lblReport" runat="Server" Text="Reports:" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Letterhead Margin
                                                </td>
                                                <td>
                                                    <wucTextBox:TextBox runat="server" ID="txtLetterheadMargin" maxLength="3" width="50%" />
                                                    cm
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="linkBtnUserFieldSave" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                            <div id="divImportPatient" style="display: none">
                                <table width="100%" border="0">
                                    <tr>
                                        <td>
                                            <asp:Label runat="Server" Text="MD File" /></td>
                                        <td>
                                            <asp:FileUpload ID="FileUpload1" runat="server" /></td>
                                        <td style="text-align: right">
                                            <asp:Button ID="btnImportUser" runat="server" OnClick="ImportPatient_OnClick" Text="Import Patient"
                                                Style="width: 150px" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="divUserSettings" style="display: none">
                                <asp:UpdatePanel runat="server" ID="up_UserSettings" UpdateMode="conditional">
                                    <ContentTemplate>
                                        <table width="100%" border="0">
                                            <tr>
                                                <td colspan="3" style="text-align: right">
                                                    <input type="button" id="btnUpdateUserSettings" value="Update Settings" style="width: 150px"
                                                        onclick="javascript:btnUpdateUserSettings_OnClick();" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 15%">
                                                    <asp:Label runat="server" ID="lblOldPassword" Text="Old password" /></td>
                                                <td style="width: 35%">
                                                    <wucTextBox:TextBox ID="txtUserPW" runat="server" Direction="LTR" maxLength="8" textMode="Password"
                                                        width="40%" />
                                                </td>
                                                <td style="width: 40%" rowspan="3" />
                                            </tr>
                                            <tr>
                                                <td style="width: 15%">
                                                    <asp:Label runat="server" ID="lblUserNewPW" Text="New password" /></td>
                                                <td style="width: 35%">
                                                    <wucTextBox:TextBox ID="txtNewUserPW" runat="server" Direction="LTR" maxLength="8"
                                                        textMode="Password" width="40%" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="lblUserNewPW_Confirm" Text="Confirm new password" />
                                                </td>
                                                <td>
                                                    <wucTextBox:TextBox ID="txtNewUserPW_Confirm" runat="server" Direction="LTR" maxLength="8"
                                                        textMode="Password" width="40%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="linkbtnUpdateUserSettings" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                            <div id="divTemplate" style="display: none">
                                <asp:UpdatePanel runat="server" ID="up_Template" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%" border="0">
                                            <tr>
                                                <td colspan="2" style="text-align: right">
                                                    <input type="button" id="btnAddTemplate" style="width: 150px" value="Add new template"
                                                        onclick="javascript:btnAddTemplate_onclick();" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 5%">
                                                    <asp:Label runat="Server" ID="lblTemplateName" Text="Name:" /></td>
                                                <td style="width: 95%">
                                                    <wucTextBox:TextBox runat="server" ID="txtTemplateName" maxLength="20" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top">
                                                    <asp:Label runat="Server" ID="lblTemplateText" Text="Text:" />
                                                </td>
                                                <td>
                                                    <wucTextArea:TextArea runat="server" ID="txtTemplateText" rows="10" width="95%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="linkBtnTemplateSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="linkBtnTemplateLoad" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                            <div id="divAllTemplates" style="display: none">
                                <div>
                                    <table style="width: 100%">
                                        <tr style="height: 30px">
                                            <td id="lblTemplateName_TC" style="width: 20%">
                                                Name</td>
                                            <td id="lblTemplateUser_TC" style="width: 20%">
                                                Created By</td>
                                            <td id="Td1" style="width: 40%">
                                                Text</td>
                                            <td id="btnLinkAddNewTemplate" runat="server" style="width: 20%">
                                                <input type="button" value="Add new Template" onclick="javascript:btnAddNewTemplate();" /></td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="div_tblTemplatesList" style="overflow-y: scroll; width: 100%; height: 250px">
                                </div>
                            </div>                               
                            <div id="divLogo" style="display: none">
                                <asp:UpdatePanel runat="server" ID="up_Logo" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%" border="0">
                                            <tr>
                                                <td colspan="2" style="text-align: right">
                                                    <input type="button" id="btnAddLogo" style="width: 150px" value="Add new logo"
                                                        onclick="javascript:btnAddLogo_onclick();" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 10%">
                                                    <asp:Label runat="Server" ID="lblLogoName" Text="Name:" /></td>
                                                <td style="width: 90%">
                                                    <wucTextBox:TextBox runat="server" ID="txtLogoName" maxLength="20" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <div id="divPhoto" runat="server">
                                                        <asp:Image ID="detailsPhoto" runat="server" Width="250" />
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top">
                                                    <asp:Label runat="server" ID="lblPhoto" Text="Photo Path: " />
                                                </td>
                                                <td valign="top">
                                                    <div id="divUploadFile">
                                                        <input type="file" id="uploadPhoto" runat="server" onchange="checkPhotoStatus();"/>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <a runat="server" id="changeFileUpload" onclick="changeFileInputField('divUploadFile')" href="javascript:noAction();">[Change]</a>
                                                                    <a runat="server" id="clearFileUpload" style="display:none" onclick="clearFileInputField('divUploadFile')" href="javascript:noAction();">[Clear]</a>
                                                                    <a runat="server" id="cancelFileUpload" style="display:none" onclick="cancelFileInputField('divUploadFile')" href="javascript:noAction();">[Cancel]</a>
                                                                    
                                                                    <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="linkBtnLogoSave_OnClick" Visible="false"/>
                                                                    <input type="hidden" id="tempPhotoStatus" runat="server" value="1" />
                                                                    <input type="hidden" id="tempPhotoDisplay" runat="server" value="" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnUpload" />
                                        <asp:AsyncPostBackTrigger ControlID="linkBtnLogoLoad" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                            <div id="divAllLogos" style="display: none">
                                <div>
                                    <table style="width: 100%">
                                        <tr style="height: 30px">
                                            <td id="lblLogoName_TC" style="width: 80%">
                                                Name</td>
                                            <td id="btnLinkAddNewLogo" runat="server" style="width: 20%">
                                                <input type="button" value="Add new Logo" onclick="javascript:btnAddNewLogo();" /></td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="div_tblLogosList" style="overflow-y: scroll; width: 100%; height: 300px">
                                </div>
                            </div> 
                            <div id="divActionLog" style="display: none">
                                <div>
                                    <table style="width: 100%">
                                        <tr style="height: 30px">
                                            <td id="lblUserName_TC" style="width: 8%">
                                                UserName</td>
                                            <td id="lblPage_TC" style="width: 8%">
                                                Page</td>
                                            <td id="lblAction_TC" style="width: 6%">
                                                Action</td>
                                            <td id="lblActionDetail_TC" style="width: 24%">
                                                Action Detail</td>
                                            <td id="lblPatientID_TC" style="width: 14%">
                                                Patient ID</td>
                                            <td id="lblPatientName_TC" style="width: 14%">
                                                Patient Name</td>
                                            <td id="lblRecordID_TC" style="width: 9%">
                                                Record ID</td>
                                            <td id="lblEventTime_TC" style="width: 17%">
                                                Event Time</td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="div_tblActionLogList" style="overflow-y: scroll; width: 100%; height: 250px" runat="server">
                                </div>
                                <div>
                                <br />
                                <b>Export Action Log</b><br />Start Date: <wucTextBox:TextBox ID="txtSDate" runat="server" maxLength="10" width="8%"/>
                                &nbsp; &nbsp; &nbsp; End Date: <wucTextBox:TextBox ID="txtEDate" runat="server" maxLength="10" width="8%"/>
                                <input id="Button2" type="button" runat="server" value="Export" onserverclick="linkBtnExportLogToExcel" />
                                <hr />
                                <div>
                                
                                <b>Export Action Detail Log</b><br />
                                Record Type: 
                                <asp:DropDownList runat = "server" ID = "RecordType">
                                <asp:ListItem Value="pat">Patient Detail</asp:ListItem>
                                <asp:ListItem Value="vst">Visit</asp:ListItem>
                                <asp:ListItem Value="com">Comment</asp:ListItem>
                                <asp:ListItem Value="opr">Operation</asp:ListItem>
                                <asp:ListItem Value="cpl">Complication</asp:ListItem>
                                <asp:ListItem Value="bio">Biochemistry</asp:ListItem></asp:DropDownList>
                                &nbsp; &nbsp; &nbsp; Patient / Record ID: <input type="text" id="RecordID" runat="server" />
                                <input id="Button1" type="button" runat="server" value="Export Record" onserverclick="linkBtnExportDetailLogToExcel" />
                                </div>
                                </div> 
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
        <asp:UpdatePanel runat="server" ID="upButtons">
            <ContentTemplate>
                <asp:LinkButton runat="server" ID="linkBtnDoctorSave" OnClick="linkBtnDoctorSave_OnClick" />
                <asp:LinkButton runat="server" ID="linkBtnDoctorLoad" OnClick="linkBtnDoctorLoad_OnClick" />
                <asp:LinkButton runat="server" ID="linkBtnRefDoctorSave" OnClick="linkBtnRefDoctorSave_OnClick" />
                <asp:LinkButton runat="server" ID="linkBtnRefDoctorLoad" OnClick="linkBtnRefDoctorLoad_OnClick" />
                <asp:LinkButton runat="server" ID="linkbtnUpdateUserSettings" OnClick="linkbtnUpdateUserSettings_OnClick" />
                <asp:LinkButton runat="server" ID="linkBtnUserFieldSave" OnClick="linkBtnUserFieldSave_OnClick" />
                <asp:LinkButton runat="server" ID="linkBtnActivatePatient" OnClick="linkBtnActivatePatient_OnClick" />
                <asp:LinkButton runat="server" ID="linkBtnDeleteLogo" OnClick="linkBtnDeleteLogo_OnClick" />
                <asp:LinkButton runat="server" ID="linkBtnBiochemistrySave" OnClick="linkBtnBiochemistrySave_OnClick" />
                <asp:LinkButton runat="server" ID="linkBtnTemplateSave" OnClick="linkBtnTemplateSave_OnClick" />
                <asp:LinkButton runat="server" ID="linkBtnTemplateLoad" OnClick="linkBtnTemplateLoad_OnClick" />
                <asp:LinkButton runat="server" ID="linkBtnLogoSave" OnClick="linkBtnLogoSave_OnClick" />
                <asp:LinkButton runat="server" ID="linkBtnLogoLoad" OnClick="linkBtnLogoLoad_OnClick" />
                
                
                <asp:HiddenField runat="server" ID="txtHActionLogValue" Value="" />
                <asp:HiddenField runat="server" ID="txtHPageNo" Value="0" />
                <asp:HiddenField runat="server" ID="txtHDoctorID" Value="0" />
                <asp:HiddenField runat="server" ID="txtHRefDoctorID" Value="0" />
                <asp:HiddenField runat="server" ID="txtHPatientID" Value="0" />
                <asp:HiddenField runat="server" ID="txtHSystemDetailsID" Value="0" />
                <asp:HiddenField runat="server" ID="txtHPatientBiochemistryValue" Value="" />
                <asp:HiddenField runat="server" ID="txtHTemplateID" Value="0" />
                <asp:HiddenField runat="server" ID="txtHLogoID" Value="0" />
                <asp:HiddenField runat="server" ID="txtHApplicationURL" />
                <asp:HiddenField runat="Server" ID="txtHCulture" />
                <asp:HiddenField runat="Server" ID="TitleLoaded" Value="0" />
                <textarea runat="server" id="txtTest" rows="5" cols="80" style="display: none" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="linkBtnDoctorSave" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="linkBtnDoctorLoad" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="linkbtnUpdateUserSettings" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="linkBtnTemplateSave" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="linkBtnTemplateLoad" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="linkBtnLogoSave" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="linkBtnLogoLoad" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </form>
</body>
</html>
