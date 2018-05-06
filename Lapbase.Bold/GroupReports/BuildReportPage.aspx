<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BuildReportPage.aspx.cs" Inherits="Reports_BuildReportPage" %>
<%@ Register TagPrefix="dotnet"  Namespace="dotnetCHARTING" Assembly="dotnetCHARTING"%>
<%@ Register TagPrefix="rv"  Namespace="Microsoft.Reporting.WebForms" Assembly="Microsoft.ReportViewer.WebForms, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server" id = "PageHeader">
    <title>LapBase - A Data Manager for Bariatric Surgery</title>
    <link href='<%$ AppSettings:CssUrl%>' rel="stylesheet" type="text/css" />
    <style type="text/css" media="screen">
	    body {
		    margin: 0;
		    padding: 40px 20px 20px 20px;
		    color: #000;
		    font-size: 76%;
		    font-family: "lucida grande", arial, verdana, helvetica, sans-serif;
		    }
	</style>
	<style type="text/css" media="print">
	    body {
		    margin: 0;
		    padding: 0;
		    color: #000;
		    font-size: 76%;
		    font-family: arial, verdana, helvetica, sans-serif;
		    }
    </style>
    <script type = "text/javascript" src = "../Scripts/Global.js"></script>
    <script type = "text/javascript" language = "javascript">
        function body_onload(){
            //InitializePage('frmBuildReport');
            window.setTimeout("javascript:btnBuildReport_onclientclick();", 10);
        }
        function btnBuildReport_onclientclick(){
            ShowDivMessage("The '" + document.title + "' is being built...", false);
            __doPostBack('btnBuildReport','');
        }
    </script>
</head>
<body id = "bodyReport" runat = "server">
    <form id="frmBuildReport" runat="server">
        <asp:LinkButton runat = "server" ID = "btnBuildReport" Text = "Build Report" OnClick = "btnBuildReport_OnClick" style="display:none" OnClientClick="javascript:btnBuildReport_onclientclick();"/>
        <div style="width:100%;text-align:right" class="noPrint">
            <input value="Print" onclick="javascript:window.print();" type="button" style="width:75px" id="btnPrint"/>
	        <input value="Close" onclick="javascript:window.close();" type="button" style="width:75px" id="btnClose"/>
        </div>
        <span runat = "server" id = "tcXML"></span>
        <dotnet:Chart ID = "chartBMI" runat = "server"  width="750px" height="421px" CssClass="graph" Visible="false" />
        <dotnet:Chart ID = "chartEWL" runat = "server"  width="750px" height="421px" CssClass="graph" Visible="false" />
        <asp:HiddenField runat = "Server" ID = "TitleLoaded" Value = "0" />
        <asp:HiddenField runat = "server" id = "txtReportCode" value = ""/>
        <asp:HiddenField runat = "server" id = "txtReportName" value = ""/>
        <asp:label ID="lblResult" runat="server"></asp:label>
    </form>
</body>
<script type = "text/javascript" language = "javascript">
    document.title = document.forms[0].txtReportName.value;
</script>
</html>