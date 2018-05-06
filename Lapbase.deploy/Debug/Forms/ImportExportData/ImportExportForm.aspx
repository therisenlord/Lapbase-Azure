<%@ page language="C#" autoeventwireup="true" inherits="Forms_ImportExportData_ImportExportForm, Lapbase.deploy" enableEventValidation="false" viewStateEncryptionMode="Always" %>
<%@ register tagprefix="radu" namespace="Telerik.WebControls" assembly="RadUpload.Net2" %>
<%@ Register TagPrefix = "wucMenu" TagName = "Menu" Src = "~/UserControl/MenuWUCtrl.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>LapBase - A Data Manager for Bariatric Surgery</title>
	<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <link href='<%$ AppSettings:CssUrl%>' rel="stylesheet" type="text/css" />
    <script src="../../Scripts/Global.js" type="text/javascript" language="javascript"></script>
    <script src="Includes/ImpExpPopup.js" type="text/javascript" language="javascript"></script>
    
</head>
<body runat = "server" id= "bodyImportExport">
    <form id="frmImportExport" runat="server" enctype ="multipart/form-data">
        <wucMenu:Menu runat = "server" ID = "mainMenu" />
        <asp:ScriptManager ID="_ScriptManager" runat="server" AsyncPostBackTimeout = "60000" />
        <div class="tabMenus">
		    <div class="greyTabMenu">
			    <ul>
				    <li class="current"><a href="#" id = "a_Import">Import / Export</a></li>
			    </ul>
		    </div>
	    </div>
	    
	    <div class="contentArea">
            <div class="greyContentWrap">
                <div class="importExport">
                    <asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div id = "divErrorMessage" style ="display:none" ></div>
                        </ContentTemplate>
                        <Triggers>
                                <asp:PostBackTrigger ControlID = "btnUploadMDB" />
                            </Triggers>
                    </asp:UpdatePanel>
                    
                    <div class = "importStep1" id = "div_importStep1A" runat = "server">
                        <h3 id = "hImportStep1A">Import (Step 1a)</h3>
                        <p id = "pBrowse">Please click browse to ...</p>
                        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
                                <div class="fileUploadMde">
            	                    <label id = "lblUploadMDE">Upload MDB</label>
                                    <input type="file" runat="server" id="inputFile1" />
                                </div>
                        
                                <div class="fileUploadMdb" style="display:none">   
                                    <label id = "lblUploadMDB">Upload MDE</label>
                                    <input type="file" runat="server" id="inputFile2" />
                                </div>
                                <%--
                                <asp:CustomValidator id = "CustomValidator1" ControlToValidate = "inputFile1" ClientValidationFunction="IsFileTypeValied" runat = "server" />
	                            <asp:CustomValidator id = "CustomValidator2" ControlToValidate = "inputFile2" ClientValidationFunction="IsFileTypeValied" runat = "server" />
	                            --%>
                                <input type="button" runat = "Server" onserverclick = "btnUploadMDB_onclick" id = "btnUploadMDB" value = "Submit" disabled="false" 
                                    onclick="javascript:document.getElementById('div_importStep1A').style.display = 'none';document.getElementById('div_importStep1B').style.display = 'block';"/>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID = "btnUploadMDB" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    
                    <div class = "importStep1" style="display:none" id = "div_importStep1B" runat = "server">
                        <h3 id = "h_ImportStep1B">Import (Step 1b)</h3>
                        
                        <radu:radprogressmanager id="Radprogressmanager1" runat="server" />
                        <radu:radprogressarea id="RadProgressArea1" runat="server"></radu:radprogressarea>
                    </div>
                        
                    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div class = "importStep2" style="display:none" runat = "server" id = "div_importStep2A">
                                <h3 id = "h_ImpoertStep2A">Import (Step 2a)</h3>
                                <asp:Repeater runat = "server" ID = "repeaterUpload" >
	                                <HeaderTemplate>
	                                    <p id = "p_UploadedFiles">Uploaded File(s) Are : </label><br /><br />
	                                </HeaderTemplate>
	                                <ItemTemplate>
	                                    <p id = "p_FileName" >Name : <asp:Label runat = "server" ID = "lblFileName_value" Text = '<%#DataBinder.Eval(Container.DataItem, "FileName")%>'/></p><br />
	                                    <p id = "p_FileSize" >Size : <asp:Label runat = "server" ID = "lblFileSize_value" Text = '<%#DataBinder.Eval(Container.DataItem, "ContentLength").ToString() + " bytes"%>'/></p>
	                                    <br /><br />
	                                </ItemTemplate>
                                </asp:Repeater>
                                <asp:HiddenField runat = "server" id = "txtHFileName" value = "" />
                                <input type="button" id = "btnImport" value = "Import data from local MDB" runat = "server"
                                    onclick = "javascript:btnImport_onclick();" onserverclick = "btnImport_onclick" style ="width:200px"/>
                            </div>
                            
                            <div class = "importStep2" style="display:none" id = "div_ImportStep2B" runat = "server">
                                <h3 id = "h_ImportStep2B">Import (Step 2b)</h3>
                                <p><br><br><img src="~/img/large_loader.gif" id = "imgImport" alt="." runat = "server"/></p>
                                <p id = "p_WaitForImporting">Please wait, LapBase is importing your data...</p>
                            </div>
                            
                            <div class = "importStep2" style="display:none" id = "div_ImportStep2C" runat = "server">
                                <h3 id = "h_ImportStep2C">Import (Step 2c)</h3>
                                <asp:Label runat = "server" ID = "Label1" /> <br /><br />
                                <br/><br/>
                                
                                <table style ="width:420px">
    	                            <tr>
    	                                <td>
    	                                    <table style="width:100%">
    	                                        <tr>
    	                                            <td style="width:60%"><asp:Label runat = "server" ID = "lblPatients"  Text = "Patients Qty : "/></td>
    	                                            <td style="width:20%"><asp:Label runat = "Server" ID = "lblPatients_value" /></td>
    	                                            <td style="width:20%"><img runat = "server" id = "imgPatients" alt="." src="~/img/tick.gif"/></td>
    	                                        </tr>
    	                                    </table>
    	                                </td>
    	                                <p class="errorMsg" runat = "server" id = "p_Patients"></p>
    	                            </tr>
    	                            
    	                            <tr>
    	                                <td>
    	                                    <table style="width:100%">
    	                                        <tr>
    	                                            <td style="width:60%"><asp:Label runat = "server" ID = "lblPWD"  Text = "Patient Weight Data Rows Qty : "/></td>
    	                                            <td style="width:20%"><asp:Label runat = "Server" ID = "lblPWD_value" /></td>
    	                                            <td style="width:20%"><img runat = "server" id = "imgPWD" alt="." src="~/img/tick.gif"/></td>
    	                                        </tr>
    	                                    </table>
    	                                </td>
    	                                <p class="errorMsg" runat = "server" id = "p_PWD"></p>
    	                            </tr>
    	                            
    	                            <tr>
    	                                <td>
    	                                    <table style="width:100%">
    	                                        <tr>
    	                                            <td style="width:60%"><asp:Label runat = "server" ID = "lblVisits"  Text = "Visits Qty : "/></td>
    	                                            <td style="width:20%"><asp:Label runat = "Server" ID = "lblVisits_value" /></td>
    	                                            <td style="width:20%"><img runat = "server" id = "imgVisit" alt="." src="~/img/tick.gif"/></td>
    	                                        </tr>
    	                                    </table>
    	                                    <p class="errorMsg" runat = "server" id = "p_Visits"></p>
    	                                </td>
    	                            </tr>
    	                            
    	                            <tr >
    	                                <td>
    	                                    <table style="width:100%">
    	                                        <tr>
    	                                            <td style="width:60%"><asp:Label runat = "server" ID = "lblOperations"  Text = "Operations Qty : "/></td>
    	                                            <td style="width:20%"><asp:Label runat = "Server" ID = "lblOperations_value" /></td>
    	                                            <td style="width:20%"><img runat = "server" id = "imgOperations" alt="." src="~/img/tick.gif"/></td>
    	                                        </tr>
    	                                    </table>
    	                                    <p class="errorMsg" runat = "server" id = "p_Operations"></p>
    	                                </td>
    	                            </tr>
        	                        
    	                            <tr >
    	                                <td>
    	                                    <table style="width:100%">
    	                                        <tr>
    	                                            <td style="width:60%"><asp:Label runat = "server" ID = "lblComplications"  Text = "Complications Qty : "/></td>
    	                                            <td style="width:20%"><asp:Label runat = "Server" ID = "lblComplications_value" /></td>
    	                                            <td style="width:20%"><img runat = "server" id = "imgComplications" alt="." src="~/img/tick.gif"/></td>
    	                                        </tr>
    	                                    </table>
    	                                    <p class="errorMsg" runat = "server" id = "p_Complications"></p>
    	                                </td>
    	                            </tr>
        	                        
    	                            <tr >
    	                                <td>
    	                                    <table style="width:100%">
    	                                        <tr>
    	                                            <td style="width:60%"><asp:Label runat = "server" ID = "lblDoctors"  Text = "Doctors Qty : "/></td>
    	                                            <td style="width:20%"><asp:Label runat = "Server" ID = "lblDoctors_value" /></td>
    	                                            <td style="width:20%"><img runat = "server" id = "imgDoctors" alt="." src="~/img/tick.gif"/></td>
    	                                        </tr>
    	                                    </table>
    	                                    <p class="errorMsg" runat = "server" id = "p_Doctors"></p>
    	                                </td>
    	                            </tr>
        	                        
    	                            <tr >
    	                                <td>
    	                                    <table style="width:100%">
    	                                        <tr>
    	                                            <td style="width:60%"><asp:Label runat = "server" ID = "lblRefDoctors"  Text = "Reffering Doctors Qty : "/></td>
    	                                            <td style="width:20%"><asp:Label runat = "Server" ID = "lblRefDoctors_value" /></td>
    	                                            <td style="width:20%"><img runat = "server" id = "imgRefDoctors" alt="." src="~/img/tick.gif"/></td>
    	                                        </tr>
    	                                    </table>
    	                                    <p class="errorMsg" runat = "server" id = "p_RefDoctors"></p>
    	                                </td>
    	                            </tr>
        	                        
    	                            <tr style="display:none;">
    	                                <td>
    	                                    <table style="width:100%">
    	                                        <tr>
    	                                            <td style="width:60%"><asp:Label runat = "server" ID = "lblSystemCodes"  Text = "System Codes Qty : "/></td>
    	                                            <td style="width:20%"><asp:Label runat = "Server" ID = "lblSystemCodes_value"  /></td>
    	                                            <td style="width:20%"><img runat = "server" id = "imgSystemCodes" alt="." src="~/img/tick.gif"/></td>
    	                                        </tr>
    	                                    </table>
    	                                    <p class="errorMsg" runat = "server" id = "p_SystemCodes"></p>
    	                                </td>
    	                            </tr>
        	                        
    	                            <tr >
    	                                <td>
    	                                    <table style="width:100%">
    	                                        <tr>
    	                                            <td style="width:60%"><asp:Label runat = "server" ID = "lblCodes"  Text = "Codes Qty : "/></td>
    	                                            <td style="width:20%"><asp:Label runat = "Server" ID = "lblCodes_value" /></td>
    	                                            <td style="width:20%"><img runat = "server" id = "imgCodes" alt="." src="~/img/tick.gif"/></td>
    	                                        </tr>
    	                                    </table>
    	                                    <p class="errorMsg"></p>
    	                                </td>
    	                            </tr>
        	                        
    	                            <tr >
    	                                <td>
    	                                    <table style="width:100%">
    	                                        <tr>
    	                                            <td style="width:60%"><asp:Label runat = "server" ID = "lblHospitals"  Text = "Hospitals Qty : "/></td>
    	                                            <td style="width:20%"><asp:Label runat = "Server" ID = "lblHospitals_value"/></td>
    	                                            <td style="width:20%"><img runat = "server" id = "imgHospitals" alt="." src="~/img/tick.gif"/></td>
    	                                        </tr>
    	                                    </table>
    	                                    <p class="errorMsg"></p>
    	                                </td>
    	                            </tr>
    	                        </table>
    	                        <textarea runat = "server" id = "txtTest" rows = "10" cols = "80" style="display:none"/>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    
                    <div class = "clr"></div>
                </div>
            </div>
        </div>
        <asp:HiddenField runat = "server" ID = "txtHApplicationURL"  />
    </form>
</body>
</html>
