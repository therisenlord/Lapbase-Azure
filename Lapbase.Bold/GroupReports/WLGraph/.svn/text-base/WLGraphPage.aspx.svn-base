<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WLGraphPage.aspx.cs" Inherits="Reports_WLGraph_WLGraphPage" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register TagPrefix="dotnet"  Namespace="dotnetCHARTING" Assembly="dotnetCHARTING"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>LapBase - A Data Manager for Bariatric Surgery</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
    <link href='<%$ AppSettings:CssUrl%>' rel="stylesheet" type="text/css" />
    <script type = "text/javascript" src = "../../Scripts/Global.js"></script>
</head>
<body onload = "javascript:InitializePage('rptWLGraph');">
    <form id="rptWLGraph" runat="server" >
        <div class="printPreview">
            <div class="printPreviewContent">
                <img src="~/img/print_header_bar.gif" style="width:800px;height:8px" runat = "server" alt=""/>
                
                <div class="logoArea" style ="width:800px">
                    <img src="~/img/logo.gif" alt="" runat= "Server"/>
                    <input value="Close" onclick="javascript:window.close();" type="button" style="float:right;width:75px" id = "btnClose"/>
                    <input value="Print" onclick="javascript:window.print();" type="button" style="float:right;width:75px" id = "btnPrint"/>
                </div>
                
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td>&nbsp;</td>
                        <td colspan="2"><h3><asp:Label runat ="server" ID = "valueOfPatientName" /></h3></td>
                    </tr>
                    <tr>
                        <td style="width:2%">&nbsp;</td>
			            <td style="width:26%"><label id = "lblAge">AGE : </label><asp:Label runat = "server" ID = "valueOfAGE" /></td>
			            <td style="width:72%"><asp:Label id = "lblSurgeryDate" runat="server"/><asp:Label runat = "Server" ID = "valueOfLapBandDate" /></td>
			        </tr>
                    <tr>
                        <td>&nbsp;</td>
			            <td>&nbsp;</td>
			            <td>&nbsp;</td>
                    </tr>
                    
                    <tr>
		                <td>&nbsp;</td>
			            <td><label id = "lblStartWeight">Start Weight : </label><asp:label runat = "server" ID= "valueOfStartWeight"/>&#160;&#160;<asp:Label runat="server" ID="valueOfStartWeight_Unit" /></td>
			            <td><label id = "lblInitialBMI">Initial BMI : </label><asp:Label runat ="server" ID = "valueOfInitBMI"/></td>
			        </tr>
			        
			        <tr>
		                <td>&nbsp;</td>
			            <td><label id = "lblCurrentWeight">Current Weight : </label><asp:Label runat="server" ID="valueOfCurrentWeight"/>&#160;&#160;<asp:Label runat = "server" ID ="valueOfCurrentWeight_Unit" /></td>
			            <td><label id = "lblTargetWeight" >Target Weight : </label><asp:Label runat="server" ID="valueOfTargetWeight"/>&#160;&#160;<asp:Label runat = "server" ID = "valueOfTargetWeight_Unit"/></td>
			        </tr>
			        <tr>
                        <td>&nbsp;</td>
			            <td>&nbsp;</td>
			            <td>&nbsp;</td>
                    </tr>
                </table>                

                <dotnet:Chart ID = "chartWL" runat = "server" width="750px" height="421px" CssClass="graph"  />
            </div>
        </div>
        <asp:HiddenField runat = "Server" id = "txtHCulture" />
        <asp:HiddenField runat = "Server" ID = "TitleLoaded" Value = "0" />
        <asp:HiddenField runat = "server" ID = "txtHApplicationURL"  />
    </form>
</body>
</html>
