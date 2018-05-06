<%@ page language="C#" autoeventwireup="true" inherits="Forms_Comorbidity_ComorbidityForm, Lapbase.deploy" enableEventValidation="false" viewStateEncryptionMode="Always" %>
<%@ Register TagPrefix = "wucMenu" TagName = "Menu" Src = "~/UserControl/MenuWUCtrl.ascx" %>
<%@ Register TagPrefix = "wucPatient" TagName = "PatientTitle" Src = "~/UserControl/PatientTitleDataWUCtrl.ascx" %>
<%@ Register TagPrefix = "wucAppSchema" TagName = "AppSchema" Src = "~/UserControl/AppSchemaWUCtrl.ascx" %>
<%@ Register TagPrefix = "wucTextBox" TagName = "TextBox" Src = "~/UserControl/TextBoxWUCtrl.ascx" %>
<%@ Register TagPrefix = "wucSystemCode" TagName = "SystemCodeList" Src = "~/UserControl/SystemCodeWUCtrl.ascx"%>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>LapBase - A Data Manager for Bariatric Surgery</title>
    <link href='<%$ AppSettings:CssUrl%>' rel="stylesheet" type="text/css" />
    <link href="~/css/Comorbidity.css" type="text/css" rel="stylesheet" runat="server"/>
    <script language = "javascript" type="text/javascript" src="../../Scripts/Global.js" ></script>
    <script language = "javascript" type="text/javascript" src="Includes/Comorbidity.js" ></script>
    <script language = "javascript" type="text/javascript" src="../../Scripts/ScrollTable.js" ></script>
</head>
<body runat = "server" id="bodyComorbidity" >
    <wucMenu:Menu runat = "server" ID = "mainMenu" />
    <div class="tabMenus">
        <wucAppSchema:AppSchema runat = "server" ID = "AppSchemaMenu" currentItem = "Comorbidities" />
		<div class="manilaTabMenu">
			<ul>
				<li class="current" id = "li_Div1">
				    <a href="#" onclick="javascript:ComorbiditySubMenuClick(1);">
				        <img src="~/img/tab_demographics.gif" alt="" height="16" width="16" runat = "server"/><span>Comorbidity list</span></a>
				</li>
				<li id = "li_Div2">
				    <a href="#" onclick="javascript:ComorbiditySubMenuClick(2);">
				        <img src="~/img/tab_height_weight_notes.gif" alt="" height="16" width="16" runat = "server"/><span>Major Comorbidity</span></a>
				</li>
				
				<li id = "li_Div3">
				    <a href="#" onclick="javascript:ComorbiditySubMenuClick(3);">
				        <img src="~/img/tab_height_weight_notes.gif" alt="" height="16" width="16" runat = "server"/><span>Minor Comorbidity</span></a>
				</li>
			</ul>
		</div>
	</div>

    <form id="frmComorbidity" runat="server">
    <div class="contentArea">
        <div id = "divErrorMessage" style ="display:none;"><span><p id = "pErrorMessage"></p></span></div>
        <wucPatient:PatientTitle runat = "server" ID = "tblPatientTitle" />
        <div class="greyContentWrap">
        
            <div class="comorbidities" id = "div_ComorbiditiesList">
                <div class="darkGreyTabMenu">
                    <input name="New Visit" value="New Visit" type="button"/>
				</div>
				
				<div class="comorbiditiesScrollTable" id = "div_tblComorbiditiesScroll">
				</div>
				
				<div class="clr"></div>
			</div>
			
			<div class = "comorbiditiesMajor" id = "div_MajorComorbidity" style ="display:none">
			    <asp:ScriptManager ID="_ScriptManager" runat="server" >
			    </asp:ScriptManager>
			    
			    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
			            <asp:Button runat = "server" id = "btnSaveComorbidity" Text = "Add new comorbidity visit" OnClick="btnSaveComorbidity_OnClick" />
		                <div class = "expandList">
		                    <div class="boxTop">
						        <h3 id = "hbloodPressure">Blood Pressure</h3>
						        <table border='0' >
                                    <tr>
                                        <td >
                                            <label id= "lblSystolic">Systolic</label>
                                        </td>
                                        <td >
                                            <wucTextBox:TextBox runat = "server" ID= "txtSystolicBP" MaxLength = "5"/>
                                        </td>
                                        <td >
                                            <label id = "lblDiastolic" >Diastolic</label>
                                        </td>
                                        <td >
                                            <wucTextBox:TextBox runat = "server" ID="txtDiastolicBP" MaxLength="5"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td >
                                            <label id= "lblBloodTreatment">Treatment</label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan = "4">
                                            <wucTextBox:TextBox runat = "server" ID="txtBPRxDetails" TextMode = "MultiLine" MaxLength="1024" Rows = "3"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <label id = "lblHypertensionResolved">Resolved</label>
                                            <input type="checkbox" id = "chkHypertensionResolved" runat="server"/>
                                        </td>
                                        <td >
                                            <label id = "lblHypertensionResolvedDate" >Date</label>
                                        </td>
                                        <td>
                                            <wucTextBox:TextBox runat = "Server" id = "txtHypertensionResolvedDate" MaxLength = "10" />
                                        </td>
                                    </tr>
                                </table>
						        <div class="boxBtm"></div>
		                    </div>
		                </div>
        			    
		                <div class = "expandList">
		                    <div class="boxTop">
		                        <h3 id = "hDiabetes">Diabetes</h3>
		                        <table border="0" >
                                    <tr>
                                        <td >
                                            <label id= "lblBloodSugar" >F. Glucose</label>
                                        </td>
                                        <td >
                                            <wucTextBox:TextBox runat = "Server" ID= "txtFBloodGlucose" maxLength="5" />
                                        </td>
                                        <td>
                                            <label id="lblDiabetic" >HBA1C</label>
                                        </td>
                                        <td>
                                            <wucTextBox:TextBox runat = "Server" ID= "HBA1C" maxLength="5" />
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td >
                                            <label id= "lblDiabetTreatment" >Treatment</label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan = "2">
                                            <wucTextBox:TextBox runat = "Server" ID= "txtDiabetesRxDetails" MaxLength = "1024" TextMode = "MultiLine" Rows="3"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan = "2">
                                            <label id = "lblDiabetesResolved">Resolved</label>
                                            <input type="checkbox" id = "chkDiabetesResolved" runat="Server"/> 
                                        </td>
                                        <td>
                                            <label id = "lblDiabetesResolvedDate">Date</label>
                                            <wucTextBox:TextBox runat = "Server" id = "txtDiabetesResolvedDate" MaxLength = "10" />
                                        </td>
                                    </tr>
                                </table>
		                        <div class="boxBtm"></div>
		                    </div>
		                </div>
        			    
		                <div class="expandList">
		                    <div class="boxTop">
		                        <h3 id = "hLipids">Lipids</h3>
		                        <table border="0">
                                    <tr>
                                        <td>
                                            <label id= "lblChol" >Chol.</label>
                                        </td>
                                        <td>
                                            <wucTextBox:TextBox runat = "Server" id = "txtTotalCholesterol" />
                                        </td>
                                        <td>
                                            <label id = "lblTriglycerides" >Triglycerides</label>
                                        </td>
                                        <td >
                                            <wucTextBox:TextBox runat = "server" ID= "txtTriglycerides" />
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td >
                                            <label id = "lblHDLChol" >HDL Chol.</label>
                                        </td>
                                        <td >
                                            <wucTextBox:TextBox runat = "server" ID= "txtHDLCholesterol" />
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td >
                                            <label id = "lblLIPIDSTreatment" >Treatment</label>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td colspan = "4" >
                                            <wucTextBox:TextBox runat = "Server" ID = "txtLipidRxDetails" TextMode = "MultiLine" Rows = "3" />
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td colspan = "3">
                                            <label id = "lblLipidsResolved">Resolved</label>
                                            <input type="checkbox" id = "chkLipidsResolved" />
                                        </td>
                                        <td colspan = "3">
                                            <label id = "lblLipidsResolvedDate">Date</label>
                                            <wucTextBox:TextBox runat = "Server" id = "txtLipidsResolvedDate" MaxLength = "10" />
                                        </td>
                                    </tr>
                                </table>
		                        <div class = "boxBtm"></div>
		                    </div>
		                </div>
        			        
		                <div class="expandList">
		                    <div class="boxTop">
		                        <h3 id = "hAsthma">Asthma</h3>
		                        <table border="0">
                                    <tr>
                                        <td></td>
                                        <td colspan="3">
                                            <wucSystemCode:SystemCodeList runat = "server" ID="cmbAsthmaCurrentLevel"  CriteriaString = "QAS" />
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td style="Width :10%">
                                            <label id = "lblAsthmaDateResolved">Date Resolved</label>
                                        </td>
                                        <td style="Width:60%">
                                            <wucTextBox:TextBox runat = "server" ID = "txtAsthmaResolvedDate"  MaxLength = "10" />
                                        </td>
                                        <td>
                                            <label id = "lblBaseAsthmaLevel">Baseline</label>
                                        </td>
                                        <td>
                                            <label id = "lblBaseAsthmaLevel_Value"></label>
                                        </td>
                                    </tr>
                                    
                                    
                                    <tr>
                                        <td style="vertical-align: top;" >Notes</td>
                                        <td colspan="3">
                                            <wucTextBox:TextBox id= "txtBaseAsthmaDetails" runat = "server"  TextMode = "multiLine" Rows = "3" />
                                        </td>
                                    </tr>
                                </table>
		                        <div class = "boxBtm"></div>
		                    </div>
		                </div>
        			        
		                <div class="expandList">
		                    <div class ="boxTop">
		                        <h3 id="hReflux">Reflux</h3>
		                        <table border = "0">
		                            <tr>
                                        <td >
                                        </td>
                                        <td  colspan="3">
                                            <wucSystemCode:SystemCodeList runat = "server" ID="cmbRefluxCurrentLevel"  CriteriaString = "QGE" />
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td>
                                            <label id = "lblRefluxResolvedDate">Date Resolved</label>
                                        </td>
                                        <td>
                                            <wucTextBox:TextBox runat = "server" ID = "txtRefluxResolvedDate"  MaxLength = "10" />
                                        </td>
                                        <td>
                                            <label id = "lblBaseRefluxLevel">Baseline</label>
                                        </td>
                                        <td>
                                            <label id = "lblBaseRefluxLevel_Value"></label>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td style="vertical-align: top;" >Notes</td>
                                        <td colspan="3">
                                            <wucTextBox:TextBox id= "txtBaseRefluxDetails" runat = "server"  TextMode = "multiLine" Rows = "3" />
                                        </td>
                                    </tr>
		                        </table>
		                        <div class="boxBtm"></div>
		                    </div>
		                </div>

                        <div class = "expandList">
		                    <div class="boxTop">
		                        <h3 id = "hSleep">Sleep</h3>
		                        <table border="0">
		                            <tr>
                                        <td >
                                        </td>
                                        <td  colspan="3">
                                            <wucSystemCode:SystemCodeList runat = "server" ID="cmbSleepCurrentLevel"  CriteriaString = "QAS"/>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td>
                                            <label id = "lblSleepResolvedDate">Date Resolved</label>
                                        </td>
                                        <td>
                                            <wucTextBox:TextBox runat = "server" ID = "txtSleepResolvedDate"  MaxLength = "10" />
                                        </td>
                                        <td>
                                            <label id = "lblBaseSleepLevel">Baseline</label>
                                        </td>
                                        <td>
                                            <label id = "lblBaseSleepLevel_Value"></label>
                                        </td>
                                    </tr>
                                                        
                                    <tr>
                                        <td style="vertical-align: top;" >Notes</td>
                                        <td colspan="3">
                                            <wucTextBox:TextBox id= "txtBaseSleepDetails" runat = "server"  TextMode = "multiLine" Rows = "3" />
                                        </td>
                                    </tr>
		                        </table>
		                        <div class="boxBtm"></div>
		                    </div>
		                </div>	
			        </ContentTemplate>
			        <Triggers>
			            <asp:AsyncPostBackTrigger ControlID = "btnSaveComorbidity" EventName="click"/>
			        </Triggers>
			    </asp:UpdatePanel>
			</div>
			
			<div class = "comorbiditiesMinor" id = "div_MinorComorbidity" style ="display:none">
			</div>
		</div>
    </div>
    
    <asp:HiddenField runat = "server" ID = "txtHApplicationURL"  />
    </form>
</body>
<script language="javascript" type = "text/javascript">
function Page_OnLoad(){
	paddingLeft = 10;
	paddingRight = 10;
	paddingTop = 1;
	paddingBottom = 1;
	
	ScrollTableAbsoluteSize(document.getElementById("scrollTable"), 756, 350);

	var theDiv = document.getElementById("dateDiv");
	theDiv.style.verticalAlign = "bottom";
}

Page_OnLoad();
</script>
</html>
