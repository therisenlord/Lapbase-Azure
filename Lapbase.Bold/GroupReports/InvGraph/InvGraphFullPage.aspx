<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InvGraphFullPage.aspx.cs" Inherits="Reports_InvGraph_InvGraphFullPage" %>
<%@ Register TagPrefix="dotnet"  Namespace="dotnetCHARTING" Assembly="dotnetCHARTING"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>LapBase - A Data Manager for Bariatric Surgery</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
</head>
<body>
    <form id="frmInvGraph" runat="server">
        <table width = "100%">
            <tr>
                <td style ="width:70%; font-size : large; font-weight:bold">
                    <asp:Label runat ="server" ID = "valueOfPatientName" />
                </td>
                <td style ="width:30%" />
            </tr>
            <tr>
              <td colspan = "2">
                <a style= "font-size:medium; font-weight:bold;">AGE : </a>
                <asp:Label runat = "server" ID = "valueOfAGE" /> &nbsp;&nbsp; 
                <a style= "font-size:medium; font-weight:bold;">Surgery Date : </a>
                <asp:Label runat = "Server" ID = "valueOfLapBandDate" />
              </td>
            </tr>
            <tr>
                <td colspan = "2"><br /></td>
            </tr>
            
            <tr>
                <td colspan = "2">
                    <dotnet:Chart id="chartEWL" runat="server" />
                </td>
            </tr>
            
            <tr>
                <td colspan = "2"><br /></td>
            </tr>
            
            <tr>
                <td colspan = "2">
                    <dotnet:Chart id="chartHBA_Insulin_Glucose" runat="server" />
                </td>
            </tr>
            
            <tr>
                <td colspan = "2"><br /></td>
            </tr>
            
            <tr>
                <td colspan = "2">
                    <dotnet:Chart id="chartLIPIDS" runat="server" />
                </td>
            </tr>
            
            <tr>
                <td colspan = "2"><br /></td>
            </tr>
            
            <tr>
                <td colspan = "2">
                    <dotnet:Chart id="chartBloodPressure" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan = "2"><br /></td>
            </tr>
            
            <tr>
                <td colspan = "2">
                    <dotnet:Chart id="chartHematology" runat="server" />
                </td>
            </tr>
            
            <tr>
                <td colspan = "2"><br /></td>
            </tr>
            
            <tr>
                <td colspan = "2">
                    <dotnet:Chart id="chartLiveFunctionTests" runat="server" />
                </td>
            </tr>
            
            <tr>
                <td colspan = "2"><br /></td>
            </tr>
            
            <tr>
                <td colspan = "2">
                    <dotnet:Chart id="chartFatMass" runat="server" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
