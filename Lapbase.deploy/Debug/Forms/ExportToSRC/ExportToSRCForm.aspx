<%@ page language="C#" autoeventwireup="true" inherits="Forms_ExportToSRC_ExportToSRCForm, Lapbase.deploy" title="" validaterequest="false" enableEventValidation="false" viewStateEncryptionMode="Always" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>LapBase - A Data Manager for Bariatric Surgery</title>
    <link href='<%$ AppSettings:CssUrl%>' rel="stylesheet" type="text/css" />
</head>
<body>
    <Lapbase:SystemMenu runat = "Server" ID = "mainMenu" />
	<div class="tabMenus">
	    <Lapbase:AppMenu runat = "server" ID = "AppSchemaMenu" currentItem = "ExportToSRC" />
	</div>
	<div class="contentArea">
	    <div class="greyContentWrap">
	        <div id="validateMessage">
	            <b>SRC Patient Data:<br /></b>
	            <asp:Label ID="lblValidatePatientData" runat="server"></asp:Label>
	            <b><br /><br />SRC Patient Pre-Operative Visit:<br /></b>
	            <asp:Label ID="lblValidatePreOp" runat="server"></asp:Label>
	            <asp:Label ID="lblValidatePreOpWarning" runat="server"></asp:Label>
	            <b><br /><br />SRC Patient Operative Visit:<br /></b>
	            <asp:Label ID="lblValidateOp" runat="server"></asp:Label>	            
	            <b><br /><br />SRC Patient Post-Operative Visit:<br /></b>
	            <asp:Label ID="lblValidatePostOp" runat="server"></asp:Label>   
	            <asp:Label ID="lblValidatePostOpWarning" runat="server"></asp:Label>  
	            <b><br /><br />SRC Patient Adverse Event:<br /></b>
	            <asp:Label ID="lblValidateAdverseEvent" runat="server"></asp:Label>
	            <br /><br />
	        </div>
	        
	        <input type="button" runat="server" id="btnValidateSRC" value="Validate Data" onserverclick="ValidateDataToExportToSRC"/>
	        <input type="button" runat="server" id="btnSyncSRC" value="Sync with BOLD" onserverclick="ExtractDataToExportToSRC" disabled="false"/>
	        <input type="button" runat="server" id="btnSignSRC" value="Sign patient Record" onserverclick="SignPatientSRC" disabled="false" visible="false"/>    
	        
            <form id="frmExportToSRC" runat="server">
                <asp:Literal runat = "server" ID = "ltrPatientDataError" Text = "SRC Errors for patient's data transfering" />
                <asp:Repeater runat = "server" ID = "rptPatientDataError" >
                    <HeaderTemplate>
                        <ul>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <li><%#DataBinder.Eval(Container.DataItem, "ErrorMessage")%></li>
                    </ItemTemplate>
                    <FooterTemplate>
                        </ul>
                    </FooterTemplate>
                </asp:Repeater>
                
                <asp:Literal runat = "server" ID = "ltrPreOperativeVisitError" Text = "SRC Errors for patient's pre-operative visit" />
                <asp:Repeater runat = "server" ID = "rptPreOperativeVisitError" >
                    <HeaderTemplate>
                        <ul>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <li><%#DataBinder.Eval(Container.DataItem, "ErrorMessage")%></li>
                    </ItemTemplate>
                    <FooterTemplate>
                        </ul>
                    </FooterTemplate>
                </asp:Repeater>
                
                <asp:Literal runat = "server" ID = "ltrHospitalVisitError" Text = "SRC Errors for patient's hospital visit" />
                <asp:Repeater runat = "server" ID = "rptHospitalVisitError" >
                    <HeaderTemplate>
                        <ul>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <li><%#DataBinder.Eval(Container.DataItem, "ErrorMessage")%></li>
                    </ItemTemplate>
                    <FooterTemplate>
                        </ul>
                    </FooterTemplate>
                </asp:Repeater>
                
                <asp:Literal runat = "server" ID = "ltrPostOperativeVisitError" Text = "SRC Errors for patient's post-operative visit" />
                <asp:Repeater runat = "server" ID = "rptPostOperativeVisitError" >
                    <HeaderTemplate>
                        <ul>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <li><%#DataBinder.Eval(Container.DataItem, "ErrorMessage")%></li>
                    </ItemTemplate>
                    <FooterTemplate>
                        </ul>
                    </FooterTemplate>
                </asp:Repeater>

                <asp:Literal runat = "server" ID = "ltrAdverseEventPostOperativeError" Text = "SRC Errors for patient's Adverse Event Post Operative visit" />
                <asp:Repeater runat = "server" ID = "rptAdverseEventPostOperativeError" >
                    <HeaderTemplate>
                        <ul>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <li><%#DataBinder.Eval(Container.DataItem, "ErrorMessage")%></li>
                    </ItemTemplate>
                    <FooterTemplate>
                        </ul>
                    </FooterTemplate>
                </asp:Repeater>
                
                <asp:Literal runat = "server" ID = "ltrPatientSignError" Text = "SRC Errors for patient's Sign" />
                <asp:Repeater runat = "server" ID = "rptPatientSignError" >
                    <HeaderTemplate>
                        <ul>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <li><%#DataBinder.Eval(Container.DataItem, "ErrorMessage")%></li>
                    </ItemTemplate>
                    <FooterTemplate>
                        </ul>
                    </FooterTemplate>
                </asp:Repeater>
                
                <asp:Literal runat = "server" ID = "ltrSRCSucceed" Text = "SRC Errors for patient's Sign" Visible = "false"/>
            </form>
        </div>
    </div>
</body>
</html>
