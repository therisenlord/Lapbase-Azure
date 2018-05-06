<%@ control language="C#" autoeventwireup="true" inherits="UserControl_PatientTitleDataWUCtrl, Lapbase.deploy" %>
<%@ Register TagPrefix = "wucTextBox" TagName = "TextBox" Src = "~/UserControl/TextBoxWUCtrl.ascx" %>
<%@ Register TagPrefix = "wucCode" TagName = "CodeList" Src = "~/UserControl/CodeWUCtrl.ascx" %>
<script language="javascript" type = "text/javascript">
//------------------------------------------------------------------------------------------------------------
function btnExcessWeight_onclick(){
    var EWV = new Number(), StartWeight = new Number(), IdealWeight = new Number(), TargetWeight = 0;

    if (document.all){
        EWV = parseFloat(document.getElementById("tblPatientTitle_lblExcessWeight_Value").innerText);
        StartWeight = parseFloat(document.getElementById("tblPatientTitle_lblStartWeight_Value").innerText);
        IdealWeight = parseFloat(document.getElementById("tblPatientTitle_lblIdealWeight_Value").innerText);
    }
    else{
        EWV = parseFloat(document.getElementById("tblPatientTitle_lblExcessWeight_Value").textContent);
        StartWeight = parseFloat(document.getElementById("tblPatientTitle_lblStartWeight_Value").textContent);
        IdealWeight = parseFloat(document.getElementById("tblPatientTitle_lblIdealWeight_Value").textContent);
    }
    TargetWeight = ComputeTargetWeight_EWV(EWV, StartWeight, IdealWeight);
    
    if (!isNaN(TargetWeight)){
        document.getElementById("tblPatientTitle_txtTargetWeight_txtGlobal").value = TargetWeight;
        UpdateGroupTargetWeight();
    }
    else
        document.getElementById("tblPatientTitle_txtTargetWeight_txtGlobal").value = 0;
    return;
}
    
//------------------------------------------------------------------------------------------------------------
function btnBMI_onclick(){
    var BMI = new Number(), BMIHeight = new Number(), UseImperial  = new Number(), TargetWeight = 0;
    
    if (document.all)
        BMI = parseFloat(document.getElementById("tblPatientTitle_lblBMI_Value").innerText);
    else
        BMI = parseFloat(document.getElementById("tblPatientTitle_lblBMI_Value").textContent);
        
    BMIHeight = parseFloat(document.getElementById("tblPatientTitle_txtBMIHeight").value);
    UseImperial = parseInt(document.getElementById("tblPatientTitle_txtUseImperial").value);
    
    TargetWeight = ComputeTargetWeight_BMI(BMI, BMIHeight, UseImperial);
    
    if (!isNaN(TargetWeight)){
        document.getElementById("tblPatientTitle_txtTargetWeight_txtGlobal").value = TargetWeight;
        UpdateGroupTargetWeight();
    }
    else
        document.getElementById("tblPatientTitle_txtTargetWeight_txtGlobal").value = 0;
    return;
}

//------------------------------------------------------------------------------------------------------------
function tblPatientTitle_txtTargetWeight_onchange(){
    UpdateGroupTargetWeight();
}

//------------------------------------------------------------------------------------------------------------
function UpdateGroupTargetWeight(){
    var TargetWeight = 0;
    
    if (Request.Cookies["PermissionLevel"].Value != "1o")
    {
        if (parseFloat(document.getElementById("tblPatientTitle_txtTargetWeight_txtGlobal").value) != 'NaN'){
            TargetWeight = document.getElementById("tblPatientTitle_txtTargetWeight_txtGlobal").value;
            if (document.getElementById("tblPatientTitle_txtUseImperial").value == "1") // Imperial Mode
                //TargetWeight = Math.round(parseFloat(TargetWeight) * 0.45359237);
                TargetWeight = (parseFloat(TargetWeight) * 0.45359237).toFixed(1);
        }
        else
            TargetWeight = 0;
        var strSOAP = 
                '<?xml version="1.0" encoding="utf-8"?>'+
                '<soap:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" ' + 
                                'xmlns:xsd="http://www.w3.org/2001/XMLSchema" ' + 
                                'xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">'+
	                '<soap:Body>'+
		                '<UpdatePatientTargetWeight_PWD xmlns="http://tempuri.org/">'+
		                    '<Group>' + document.getElementById("tblPatientTitle_cmbGroup_CodeList").value + '</Group>'+
			                '<TargetWeight>' + TargetWeight + '</TargetWeight>'+
		                '</UpdatePatientTargetWeight_PWD>'+
	                '</soap:Body>'+
                '</soap:Envelope>';
        SubmitSOAPXmlHttp(strSOAP, null, document.getElementById("txtHApplicationURL").value + "WebServices/GlobalWebService.asmx", "http://tempuri.org/UpdatePatientTargetWeight_PWD");
        
    }
}
</script>

<div class="patientDetails" id = "div_PatientTitle" runat="server">
<table runat = "server" id = "tblPatient" border="0" cellpadding="0" cellspacing="0" style="width:100%;" >
<tbody>
    <tr>
        <td colspan="3" align="right">Created: <asp:Label runat = "server" ID = "lblCreatedDate_Value" Font-Names="Arial, Helvetica, sans-serif" /> &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Patient ID: <asp:Label runat = "server" ID = "lblPatientID_Value" Font-Names="Arial, Helvetica, sans-serif" /></td>     
    </tr>
    <tr>
        <td class="column01" style="vertical-align:top;width:30%">
            <table style="width: 100%;" border="0" cellpadding="0" cellspacing="0">
            <tbody>
                <tr>
                    <td colspan="2">
                        <h2><asp:Label runat = "server" ID = "lblPatientName_Value" Font-Size="16pt" Font-Names="Arial, Helvetica, sans-serif" /> </h2>
                    </td>
                </tr>
                <tr>
                    <td style ="width: 5%; text-align :center">
                        <asp:Label runat = "Server" ID = "lblAge_Value" />
                    </td>
                    <td style="width:23%;">                    
                        <asp:Label runat = "server" ID = "lblDOB" Text = "Date of Birth : "/>
                        <asp:Label runat = "server" ID = "lblDOB_Value" />
                    </td>                    
                </tr>
				<tr>
                    <td style="text-align: center;">&nbsp;</td>
                    <td >
                        <b>
                        <asp:Label runat = "server" ID = "lblDeceased"/>
                        <asp:Label runat = "server" ID = "lblDeceased_Value" />
                        </b>
                    </td>
				</tr>
                <tr>                
                    <td style="text-align: center;">&nbsp;</td>
                    <td >
                        <asp:Label ID = "lblAddress_Value" runat = "server"   />
                    </td>
				</tr>
				<tr>
                    <td style="text-align: center;">&nbsp;</td>
                    <td >
                        <asp:Label runat = "server" ID = "lblHomePhone" Text = "Home Phone : "/>
                        <asp:Label runat = "server" ID = "lblHomePhone_Value" />
                    </td>
				</tr>
				<tr>
                    <td style="text-align: center;">&nbsp;</td>
                    <td >
                        <asp:Label runat = "server" ID = "lblMobilePhone" Text = "Mobile Phone : "/>
                        <asp:Label runat = "server" ID = "lblMobilePhone_Value" />
                    </td>
				</tr>
            </tbody>
            </table>
        </td>
        <td class="column02" style="vertical-align:top;width:30%">
            <table style="width: 100%;" border="0" cellpadding="0" cellspacing="0">
			<tbody>
			    <tr>
				    <td style="width: 20%;">
				        <asp:Label ID = "lblSurgeryType_Value"  runat= "server" />
                        <asp:HiddenField runat = "server" ID = "txtSurgeryType_Code" value = "0"/>
					</td>
					<td><asp:Label ID = "lblApproach_Value" runat = "Server" /></td>
				</tr>
				<tr>
				    <td style="width: 20%;">
					    <asp:Label runat = "server" ID = "lblSurgeryDate_Value" />
					</td>
					<td style="width: 22%;">
					    <asp:Label  ID = "lblCategory_Value" runat="server" />
					</td>
				</tr>
				
				<tr>
					<td>
					    <asp:Label ID = "lblDoctor_Value"  runat= "server" />
					</td>
					<td>
					    <asp:Label  ID = "lblGroup_Value" runat="server" />
					</td>
				</tr>
				<tr>
					<td>
					    <asp:Label  ID = "lblBandType_Value" runat="server" />
					</td>
					<td>
					    <asp:Label  ID = "lblBandSize_Value" runat="server" />
					</td>
				</tr>
				<tr>
					<td colspan="2">
					    <asp:Label  ID = "lblConcurrent_Value" runat="server" />
					</td>
				</tr>
			</tbody>
			</table>
        </td>
        <td class="column03" style="vertical-align:top;width:30%">
			<table id="Table1" style="width: 100%;" border="0" cellpadding="0" cellspacing="0">
			    <tbody>
			        <tr>
				        <td colspan="3">
				            <h3><asp:Label ID = "lblBaseline" runat = "server" Text = "Baseline report" Font-Size="12pt" Font-Bold="true"/></h3>
				        </td>
					    <td colspan="3">
					        <h3><asp:Label ID = "lblTarget" runat = "Server" Text = "Target " Font-Size="12pt" Font-Bold="true"/></h3>
					</td>
				    </tr>
				    <tr>
					    <td style="width: 7%" >
					        <asp:Label runat = "Server" Text = "BMI" id = "lblStartBMI" />
					    </td>
					    <td style="width: 10%" >
					        <asp:Label runat = "Server" Text = "Weight" id = "lblStartWeight" />
					    </td>
					    <td style="width: 9%" >
					        <asp:Label runat = "Server" Text = "Ideal" id = "lblIdealWeight" /></td>
					    <td rowspan="2" style="width: 7%; text-align: center;" >
					        <wucTextBox:TextBox runat = "server" ID = "txtTargetWeight" Width = "99%" maxLength="3" />
					    </td>
					    <td style="width: 6%; text-align: center;" >
					        <input type = "button" id = "btnExcessWeight" value = "" onclick="javascript:btnExcessWeight_onclick();"  style="width:95%"/>
					    </td>
					    <td style="width: 14%;">
					        <asp:Label runat = "server" ID = "lblExcessWeight_Value" Text = "66" ForeColor="blue"/>&nbsp;
					        <asp:Label runat = "server" ID = "lblExcessWeight" Text = "% Excess Wt" />
					    </td>
				    </tr>
    				
				    <tr>
					    <td>
					        <asp:Label runat = "Server" id = "lblStartBMI_Value"/>
					    </td>
					    <td>
					        <asp:Label runat = "Server" id = "lblStartWeight_Value" />
					        <asp:Label runat = "Server" id = "lblStartWeight_Unit" />
					    </td>
					    <td>
					        <asp:Label runat = "Server" id = "lblIdealWeight_Value" />
					        <asp:Label runat = "Server" id = "lblIdealWeight_Unit" />
					    </td> 
					    <td style="text-align: center;">
					        <input type = "button" id = "btnBMI" value = "" onclick = "javascript:btnBMI_onclick();" style="width:95%"/>
					    </td>   					
					    <td>
					        <asp:Label runat = "server" ID = "lblBMI" Text = "BMI" />&nbsp;&nbsp;
					        <asp:Label runat = "server" id = "lblBMI_Value" ForeColor="blue"/>
					        <asp:HiddenField runat = "server" ID = "txtBMIHeight" Value = "0" />
                            <asp:HiddenField runat = "server" ID = "txtUseImperial" Value = "0" />
					        <asp:HiddenField runat = "server" ID = "txtHTargetBMI" Value = "0" />
					    </td>
			        </tr>
			        <tr>
			            <td colspan="5">
			                <asp:Label runat = "Server" id = "lblCalculateVisitIntro"/>
			            </td>
			        </tr>
			        <tr>
			            <td colspan="5">    			        
			                <asp:Label runat = "Server" id = "lblCalculateVisit" Font-Bold/>
					        <asp:Label runat = "Server" id = "lblCalculateVisitDate" />
			            </td>
			        </tr>
			    </tbody>
			</table>
		</td>
	</tr>
</tbody>
</table>
</div>