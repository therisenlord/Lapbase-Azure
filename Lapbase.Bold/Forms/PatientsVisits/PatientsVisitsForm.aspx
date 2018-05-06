<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PatientsVisitsForm.aspx.cs" Inherits="Forms_PatientsVisits_PatientsVisitsForm" EnableViewState = "true"%>
<%@ Register TagPrefix = "wucTextBox" TagName = "TextBox" Src = "~/UserControl/TextBoxWUCtrl.ascx" %>
<%@ Register TagPrefix = "wucMenu" TagName = "Menu" Src = "~/UserControl/MenuWUCtrl.ascx" %>
<%@ Register TagPrefix = "wucSystemCode" TagName = "SystemCodeList" Src = "~/UserControl/SystemCodeWUCtrl.ascx"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>LapBase - A Data Manager for Bariatric Surgery</title>
    <link href='<%$ AppSettings:CssUrl%>' rel="stylesheet" type="text/css" />
	<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
	<script type = "text/javascript" src = "../../Scripts/Global.js"></script>
	<script type = "text/javascript" src = "Includes/PatientsVisits.js"></script>
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
<body runat = "server" id="bodyPatientsVistisPage">
	<wucMenu:Menu runat = "server" ID = "mainMenu"/>
	
	<div class="tabMenus">
		<div class="greyTabMenu">
			<ul>
				<%--<li class="current"><a href="#" id = "aPatientList" >Patient List</a></li>--%>
			</ul>
		</div>
	</div>
	
    <form runat = "Server" id = "frmPatientsList">
	<div class = "contentArea">
	    <asp:ScriptManager ID="_ScriptManager" runat="server" AsyncPostBackTimeout="60000" />
	    <div id = "divErrorMessage" style ="display:none;" runat = "server"><p id = "pErrorMessage"></p></div>
	    <div class="greyContentWrap">
	        <div class="home">
	            <div class="homeTopPanelWrap">
					<div class="homeTopPanel">
						<div class="boxTop">
							<div class="frontSearchBox">
								<h3 id = "hPatientSearch">Patient Search</h3>
								<div class="frontSearchBoxBg">
								    <table width = "385" border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width:70px" align="right"><asp:Label text="Patient ID" Id = "lblPatientID" runat ="server"/></td>
                                            <td style="width:240px" align="right">
                                                <wucTextBox:TextBox runat = "server" ID= "txtPatientID" width="95%" AutoPostBack = "false"/> 
                                            </td>
                                            <td style="width:75px" align="right"/>
                                        </tr>                   
                                        <tr>
                                            <td align="right"><asp:Label text="Surname" Id = "lblSurName" runat ="server"/></td>
                                            <td align="right">
                                                <wucTextBox:TextBox runat = "server" ID= "txtSurName" width="95%" AutoPostBack = "false"/> 
                                            </td>
                                            <td align="right">                                            
                                                <input style="width:60px" type = "button" id="btnSearchPatient"  value = "Search" onclick="javascript:LoadAllPatients('ShowAll', document.getElementById('cmbSortBy').value, document.getElementById('cmbSortBy').value == 3 ? 'DESC' : 'ASC');"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right"><asp:Label text="Firstname" Id = "lblName" runat = "server"/></td>
                                            <td align="right">
                                                <wucTextBox:TextBox runat = "server" ID= "txtName" width="95%" AutoPostBack = "false"/>
                                            </td>
                                            <td align="right">                                                
                                                <input style="width:60px" id="btnClear" type = "button" value = "Clear" onclick="javascript:btnShowAll_onclick();"/>
                                                <input type="button" value = "Search" id = "btnSearch" style ="display:none"/>
                                            </td>
                                        </tr>               
                                    </table>
                                </div>
							</div>
							<div class="quickLinkPanel">
							    <h3 id = "hBrowseByFirstLetter">Browse patients by first letter of surname</h3>
							    <div class="letterSort" id = "divCharacters" runat = "server"></div>
							</div>
							
							<div class="addNewPatient" runat="server" id="divAddNewPatient"> 
								<img src="../../img/user_add.gif" alt=""/><h3 id = "hAddNewPatient">Add New Patient</h3>
								<div class="addNewPatientBg">
								    <asp:Button runat = "server" ID = "btnAddNewPatient" Text = "Add New Patient"
								        OnClientClick = "javascript:document.getElementById('txtHPatientId').value = '0';" OnClick = "btnAddNewPatient_OnClick"  />
									<input name="Submit" value="Add New Patient" type="button" id = "_btnAddNewPatient" style="display:none"
									    onclick="javascript:window.location.replace('../Patients/PatientData/PatientDataForm.aspx?PID=0');" />
								</div>
							</div>
							
							<div class="addNewVisit" style="display:none">
								<img src="../../img/visit_add.gif" alt=""/><h3>Add New Visit</h3>
								
								<div class="addNewVisitBg">
									<input name="Submit" value="Add New Visit" type="button" id = "btnAddNewVisit"/>
								</div>								
							</div>
							<div class="boxBtm"></div>
						</div>
					</div>
					<div class="clr"></div>
				</div>
				
				<div class="visitSortBy">
					<label for="cmbSortBy">
						<asp:Label runat = "server" ID="lblSortBy" text = "Sort by" />
						<wucSystemCode:SystemCodeList runat = "server" ID = "cmbSortBy" Width="30" FirstRow="false" autoPostBack="false" CriteriaString = "SORT" onchange = "javascript:cmbSortBy_onchange();" />
					</label>
				</div>
				
				<div class="viewPatientList">
					<div class="boxTop">
						<div class="viewPatientListTitle">
						    <table cellpadding ="0" cellspacing = "1" border="0" >
                                <tr>
                                    <td colspan = "4" style="font-weight:bold;font-size:medium">
                                        <asp:Label Text = "Patient Details" runat = "Server" ID = "lblPatientDetails" />
                                    </td>
                                    <td colspan = "2" style="font-weight:bold;font-size:medium">
                                        <asp:Label Text = "Surgery Details" runat = "server" ID = "lblSurgeryDetails" />
                                    </td>
                                    <td style="Width :90px">
                                    </td>
                                    <td style="width:20px" rowspan="2"></td>
                                </tr>
                                <tr >
                                    <td style="Width :95px">
                                        <a href = "#" onclick = "javascript:SortByPatientID(this);" id = "lblPatientID_TC" runat = "server">Patient ID</a>
                                    </td>
                                    <td style="Width :155px" >
                                        <a href = "#" onclick = "javascript:SortByName(this);" id = "lblName_TC" runat = "server">Surname</a>
                                    </td>
                                    <td style="Width :90px" >
                                        <a href = "#" onclick = "javascript:SortByFName(this);" id = "lblFName_TC" runat = "server">First Name</a>
                                    </td>
                                    <td style="Width :90px">
                                        <asp:Label Text = "Suburb"  runat = "server" ID = "lblSuburb_TC" />
                                    </td>
                                    <td style="Width :80px">
                                        <asp:Label Text = "Birth date"  runat = "server" ID = "lblBirthDate_TC"  />
    	                            </td>
    	                            <td style="Width :80px">
    	                                <a href = "#" onclick = "javascript:SortBySurgeryDate(this);" class = "listDown" id = "lblSurgeryDate_TC" runat = "server">Date</a>
                                    </td>
                                    <td style="Width :155px">
                                        <asp:Label Text = "Type"  runat = "server" ID = "lblSurgeryType_TC" />
                                    </td>
                                    <td style="Width :50px">
                                        <asp:Label Text = "Surgeon"  runat = "server" ID = "lblDoctor_TC" />
                                    </td>
                                    <td style="Width :125px">
                                        <asp:Label Text = "Last Visit Date"  runat = "server" ID = "lblVisitLastDate_TC" />
                                    </td>
                                </tr>
                            </table>
						</div>
						
						<asp:UpdatePanel runat = "server" ID = "up_PatientsList" UpdateMode = "Conditional">
						    <ContentTemplate>
						        <div class="visitDisplayScroll" id = "div_PatientsList" runat = "server"/>
						        <div class="Paginationbody">
						            <div id = "div_PagesNo" runat = "Server" class = "pagination" style="width:100%;text-align:center"/>
						        </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID = "btnLoad" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID = "btnCheckPatientID" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                        
                        <div class="boxBtm"></div>
					</div>
				</div>
				
				<div class="clr"></div>
            </div>
	    </div>
	</div>
	<asp:UpdatePanel runat = "server" ID = "upHidden" UpdateMode = "Conditional">
	    <ContentTemplate>
	        <asp:HiddenField runat = "Server" ID = "txtHPageNo" Value = "1" />
	        <asp:HiddenField runat = "server" ID = "txtHText" />
            <asp:HiddenField runat = "server" ID = "txtHAction" />
            <asp:HiddenField runat = "server" ID = "txtHSurgeryType" />
            <asp:HiddenField runat = "Server" ID = "txtHApplicationURL" />
            <asp:HiddenField runat = "Server" ID = "txtHCulture" />
            <asp:HiddenField runat = "Server" ID = "TitleLoaded" value = "0"/>
            <asp:HiddenField runat = "server" id = "txtHShowType" value = "" />
            <asp:HiddenField runat = "server" id = "txtHSortOrder" value = "" />
            <asp:HiddenField runat = "server" id = "txtHPageQty" value = "0" />
            <asp:HiddenField runat = "server" ID = "txtHPatientId" Value = "0" />
            <asp:LinkButton ID = "btnLoad" runat = "server" Text="My Link" OnClick = "btnLoad_onlick" style="display:none;"/>
	        <asp:LinkButton ID = "btnCheckPatientID" runat = "server" Text="Check Patient Id" OnClick = "btnCheckPatientID_onclick" style="display:none;"/>
	        <asp:LinkButton ID = "btnGoToVisit" runat = "server" OnClick = "btnGoToVisit_OnClick" style="display:none;"/>
	    </ContentTemplate>
	    <Triggers>
	        <asp:AsyncPostBackTrigger ControlID = "btnLoad" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID = "btnCheckPatientID" EventName="Click" />
	    </Triggers>
	</asp:UpdatePanel>
	
	
    </form>
</body>
</html>