<%@ page language="C#" autoeventwireup="true" inherits="Forms_FileManagement_FileManagementForm, Lapbase.deploy" enableEventValidation="false" viewStateEncryptionMode="Always" %>
<%@ Register TagPrefix = "wucMenu" TagName = "Menu" Src = "~/UserControl/MenuWUCtrl.ascx" %>
<%@ Register TagPrefix = "wucAppSchema" TagName = "AppSchema" Src = "~/UserControl/AppSchemaWUCtrl.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>LapBase - A Data Manager for Bariatric Surgery</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <!-- flooding div stylesheet-->
    <link rel="stylesheet" href="../../css/FloatingDIV/default.css" media="screen,projection" type="text/css" />
    <link rel="stylesheet" href="../../css/FloatingDIV/lightbox.css" media="screen,projection" type="text/css" />
    <!-- End of flooding DIV Stylesheet -->
    
    <link href='<%$ AppSettings:CssUrl%>' rel="stylesheet" type="text/css" />
	<link href="~/css/FileManagement.css" rel ="Stylesheet" type="text/css" runat="server"/> 
	
	<script type = "text/javascript" src = "../../Scripts/Global.js"></script>
	<script type = "text/javascript" src = "../../Scripts/ufo.js"></script>
	<script type = "text/javascript" src = "Includes/FileManagement.js"></script>
	<script type = "text/javascript" src = "Includes/UploadDocument.js"></script>
	
	<!-- floating div javascript-->
    <script type="text/javascript" src="../../scripts/FloatingDIV/prototype.js"></script>
    <script type="text/javascript" src="../../scripts/FloatingDIV/lightbox.js"></script>
    <!-- End of floating DIV javascript -->
</head>
<body id = "bodyFileManagement" runat = "server">
    <wucMenu:Menu runat = "server" ID = "mainMenu" />
    <wucAppSchema:AppSchema runat = "server" ID = "AppSchemaMenu" currentItem = "Files" />

	<form id = "frmFileManagement" runat = "server" >
	    <div class="contentArea">
	        <div id = "divErrorMessage" style ="display:none"></div>
	        <div class="patientDetails"><span id = "lblPatientID_label">Patient ID: </span><asp:Label runat = "server" id = "lblPatientID" />
                
	        </div>
	        <div class="greyContentWrap">
                <div class="baselineFiles">
                    <div class="leftColumn">
                        <div class="sortFiles">
                            <div class="boxTop">
							    <h3 id = "hSortFiles">Sort Files</h3>
                                <table class="sortFilesFrm" >
                                    <tr>
                                        <td><label for="show_by"><span id = "lblShowBy">Show by:</span></label></td>
                                        <td><select id="show_by" onchange = "javascript:show_by_onchange(this);" >
		                                        <option value="TYPE" >Type</option>
		                                        <option value="TYPEDATE">Type &amp; Date</option>
		                                        <option value="DATE">Date</option>
	                                        </select></td>
                                    </tr>
                                    <tr>
                                        <td><label for="show_files"><span id = "lblShowDeleteFiles">Show Deleted Files:</span></label></td>
                                        <td><select id="show_files" onchange = "javascript:show_files_onchange(this);">
		                                        <option value="1">Yes</option>
		                                        <option value="0" selected>No</option>
	                                        </select></td>
                                    </tr>
                                </table>
                                <div class="boxBtm"></div>
						    </div>
					    </div>
    					
					    <div class="fileList">
						    <div class="boxTop">
							    <h3 id = "hFileList">File List</h3>
							    <div id = "div_DocumentsList" class = "fileGroup" style="overflow-y:auto" ></div>
							    <div class="boxBtm"></div>
						    </div>
					    </div>
				    </div>
    				
				    <div class="rightColumn">
				        <div class="boxTop">
				            <a href = "#" id = "linkUpload" style="float:right">
                                <input type = "button" runat="server" id = "btnUpload" value = "Upload" onclick="javascript:SetUploadURLParameters();" />
                            </a>
				            <div class="innerTabs" id = "div_DocumentItemsList">
				                <div class="innerManilaTabMenu">
				                    <ul>
								        <li class="current" id = "li_Thumbnails">
								            <a href="#" id="subContent_mnuItem01" onclick="javascript:FileManagementSubMenu_onclick(0);">Thumbnails </a></li>
								        <li id = "li_Details">
								            <a href="#" id="subContent_mnuItem02" onclick="javascript:FileManagementSubMenu_onclick(1);">Details</a></li>      
							        </ul>
							    </div>
    							
							    <div class="innerTabsContent">
							        <div class="thumbnailsDetails">
                                        <div style="font-size:large; font-weight:bold" id = "div_DocumentTitle"></div>
                                        <div id = "div_DocumentItemsList_Thumbnail" style="overflow-y:auto; display:none">
                                            <table id = "tbl_DocumentItemsList" border="0" ></table>
                                        </div>
                                        <div id = "div_DocumentItemsList_Detail" style="height:200px;overflow-y:auto; display:none"></div>
							        </div>
							    </div>
						    </div>
						    
						    <div id = "div_DocumentItemsDetailInformation" style="display:none">
						    </div>
						    <div class="boxBtm"></div>
					    </div>
				    </div>
				    <div class="clr"></div>
			    </div>
			    <div class="clr"></div>
		    </div>
	    </div>
	    <asp:HiddenField runat = "server" ID = "txtHApplicationURL"  />
        <asp:HiddenField runat = "server" ID = "txtHDocumentTypeID" value="0" />
        <asp:HiddenField runat = "server" ID = "txtHEventDate" value = ""/>
        <asp:HiddenField runat = "server" ID = "txtHEventLink" value = ""/>
        <asp:HiddenField runat = "server" ID =  "txtHDocumentName" Value = "" />
        <asp:HiddenField runat = "server" ID =  "txtHDocumentID" Value = "0" />
        <asp:HiddenField runat = "server" id = "txtHShowBy" value = "" />
        <asp:HiddenField runat = "Server" id = "txtShowDeleted" value = "0" />
        <textarea id = "txtTest" cols = "80" rows = "10" style="width:100%;visibility:hidden;display:none" ></textarea>
        <asp:HiddenField runat = "Server" ID = "txtHCulture" />
        <asp:HiddenField runat = "Server" ID = "TitleLoaded" Value = "0" />
	</form>
</body>
</html>
