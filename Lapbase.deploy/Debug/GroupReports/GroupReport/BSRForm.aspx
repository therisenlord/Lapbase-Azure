<%@ page language="C#" autoeventwireup="true" inherits="Reports_GroupReport_BSRForm, Lapbase.deploy" enableEventValidation="false" viewStateEncryptionMode="Always" %>

<%@ Register TagPrefix="wucMenu" TagName="Menu" Src="~/UserControl/MenuWUCtrl.ascx" %>
<%@ Register TagPrefix="wucTextBox" TagName="TextBox" Src="~/UserControl/TextBoxWUCtrl.ascx" %>
<%@ Register TagPrefix="wucHospital" TagName="HospitalList" Src="~/UserControl/HospitalListWUCtrl.ascx" %>
<%@ Register TagPrefix="wucRegion" TagName="RegionList" Src="~/UserControl/RegionListWUCtrl.ascx" %>
<%@ Register TagPrefix="wucDoctor" TagName="DoctorList" Src="~/UserControl/DoctorsListWUCtrl.ascx" %>
<%@ Register TagPrefix="wucCode" TagName="CodeList" Src="~/UserControl/CodeWUCtrl.ascx" %>
<%@ Register TagPrefix="wucSystemCode" TagName="SystemCodeList" Src="~/UserControl/SystemCodeWUCtrl.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>LapBase - A Data Manager for Bariatric Surgery</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <link href='<%$ AppSettings:CssUrl%>' rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="../../Scripts/Global.js"></script>

    <script type="text/javascript" src="Includes/BSReport.js"></script>

    <style type="text/css">
	    .Paginationbody {
		    margin: 20px;
		    padding: 0;
		    color: #5f626a;
		    font-size: 76%;
		    behavior: url(csshover.htc);
		    font-family: "lucida grande", arial, verdana, helvetica, sans-serif;
		    background: #fff url(../img/bg.gif) repeat-x top left;
		    text-align: left;
		    }
    </style>
</head>
<body runat="server" id="bodyGroupReport">
    <wucMenu:Menu runat="server" ID="mainMenu" />
    <div class="tabMenus">
        <div class="greyTabMenu">
            <ul>
                <li id="li_Div1" runat="server"><a id="a_BSReport" href="#"
                    onclick="javascript:LoadDiv(1);">Bariatric Surgery Reports</a></li>
            </ul>
        </div>
    </div>
    <form id="frmGroupReport" runat="server">
        <asp:ScriptManager ID="_ScriptManager" runat="server" AsyncPostBackTimeout="60000">
        </asp:ScriptManager>
        <div class="contentArea">
            <div id="divErrorMessage" style="display: none;" runat="server">
                <span>
                    <p id="pErrorMessage" runat="server">
                    </p>
                </span>
            </div>
            <div class="greyContentWrap">
                <div class="home" id="divBSReport" style="display: block">
                    <div class="homeTopPanelWrap">
                        <div class="homeTopPanel">
                            <div class="boxTop">
                                <div class="frontSearchBox">
                                    <h3 id="hPatientSearch">
                                        Patient Search</h3>
                                    <div class="frontSearchBoxBg">
                                        <table width="385" border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td align="right">
                                                    <asp:Label Text="Surname" ID="lblSurName" runat="server" /></td>
                                                <td align="right">
                                                    <wucTextBox:TextBox runat="server" ID="txtSurName" width="150px" AutoPostBack="false" />
                                                </td>
                                                <td align="right">
                                                    <input style="width: 60px" type="button" value="Search" onclick="javascript:LoadAllPatients('ShowAll', document.getElementById('cmbSortBy').value, document.getElementById('cmbSortBy').value == 3 ? 'DESC' : 'ASC');" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label Text="Firstname" ID="lblName" runat="server" /></td>
                                                <td align="right">
                                                    <wucTextBox:TextBox runat="server" ID="txtName" width="150px" AutoPostBack="false" />
                                                </td>
                                                <td align="right">
                                                    <input style="width: 60px" type="button" value="Clear" onclick="javascript:btnShowAll_onclick();" />
                                                    <input type="button" value="Search" id="btnSearch" style="display: none" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div class="quickLinkPanel">
                                    <h3 id="hBrowseByFirstLetter">
                                        Browse patients by first letter of surname</h3>
                                    <div class="letterSort" id="divCharacters" runat="server">
                                    </div>
                                </div>
                                <div class="boxBtm">
                                </div>
                            </div>
                        </div>
                        <div class="clr">
                        </div>
                    </div>
                    <div class="visitSortBy" style="display: none">
                        <label for="cmbSortBy">
                            <asp:Label runat="server" ID="lblSortBy" Text="Sort by" />
                            <wucSystemCode:SystemCodeList runat="server" ID="cmbSortBy" Width="30" FirstRow="false"
                                autoPostBack="false" CriteriaString="SORT" onchange="javascript:cmbSortBy_onchange();" />
                        </label>
                    </div>
                    <div class="viewPatientList">
                        <div class="boxTop">
                            <div class="viewPatientListTitle">
                                <table style="width: 900px" cellpadding="0" cellspacing="1" border="0">
                                    <tr>
                                        <td style="width: 85px">
                                        Patient ID
                                            <%--<a href="#" onclick="javascript:SortByPatientID(this);" id="lblPatientID_TC" runat="server">
                                                </a>--%>
                                        </td>
                                        <td style="width: 150px">Surname
                                            <%--<a href="#" onclick="javascript:SortByName(this);" id="lblName_TC" runat="server"></a>--%>
                                        </td>
                                        <td style="width: 116px">First Name
                                           <%-- <a href="#" onclick="javascript:SortByFName(this);" id="lblFName_TC" runat="server">
                                                </a>--%>
                                        </td>
                                        <td style="width: 62px">
                                            Reg 1
                                        </td>
                                        <td style="width: 62px">
                                            Reg 2-1
                                        </td>
                                        <td style="width: 62px">
                                            Reg 2-2
                                        </td>
                                        <td style="width: 62px">
                                            Reg 2-3
                                        </td>
                                        <td style="width: 62px">
                                            Reg 2-4
                                        </td>
                                        <td style="width: 62px">
                                            Reg 2-5
                                        </td>
                                        <td style="width: 62px">
                                            Reg 2-6
                                        </td>
                                        <td style="width: 62px">
                                            Reg 2-7
                                        </td>
                                        <td style="width: 62px">
                                            Reg 2-8
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <asp:UpdatePanel runat="server" ID="up_PatientsList" UpdateMode="Conditional">
                                <ContentTemplate>
                                    
                            <div class="patientBSRScroll" id="div_PatientsList" runat="server" />
                            <div><br /><input id="Button1" style="width: 60px" type="button" value="Submit" runat="server" onserverclick="btnSubmit_onserverclick" /></div>
                            <div class="Paginationbody">
                                <div id="div_PagesNo" runat="Server" class="pagination" style="width: 100%; text-align: center" />
                            </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnLoad" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                            <div class="boxBtm">
                            </div>
                        </div>
                    </div>
                    <div class="clr">
                    </div>
                    <asp:UpdatePanel runat="server" ID="upHidden" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:HiddenField runat="Server" ID="txtHPageNo" Value="1" />
                            <asp:HiddenField runat="server" ID="txtHText" />
                            <asp:HiddenField runat="server" ID="txtHAction" />
                            <asp:HiddenField runat="server" ID="txtHSurgeryType" />
                            <asp:HiddenField runat="Server" ID="txtHApplicationURL" />
                            <asp:HiddenField runat="Server" ID="txtHCulture" />
                            <asp:HiddenField runat="Server" ID="TitleLoaded" Value="0" />
                            <asp:HiddenField runat="server" ID="txtHShowType" Value="" />
                            <asp:HiddenField runat="server" ID="txtHSortOrder" Value="" />
                            <asp:HiddenField runat="server" ID="txtHPageQty" Value="0" />
                            <asp:HiddenField runat="server" ID="txtHSubmitID" Value="" />
                            <asp:LinkButton ID="btnLoad" runat="server" Text="My Link" OnClick="btnLoad_onlick"
                                Style="display: none;" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnLoad" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <input type="text" id="ReportCode" value="" style="display: none" />
    </form>
</body>
</html>
