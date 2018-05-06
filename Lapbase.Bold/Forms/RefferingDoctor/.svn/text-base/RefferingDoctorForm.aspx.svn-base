<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RefferingDoctorForm.aspx.cs" Inherits="Forms_RefferingDoctor_RefferingDoctorForm" %>
<%@ Register TagPrefix = "wucTextBox" TagName = "TextBox" Src = "~/UserControl/TextBoxWUCtrl.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link href='<%$ AppSettings:CssUrl%>' rel="stylesheet" type="text/css" /> 
    <script src = "../../Scripts/Global.js" type = "text/javascript" ></script>
</head>
<body id = "bodyRefferingDoctor" >
    <form id="frmRefferingDoctor" runat="server">
        <div class="greyBorder" style ="padding-top:3px; padding-left:3px; padding-right:3px; padding-bottom:3px"> 
            <table width="100%">
                <tr>
                    <td align="left" valign="top">
                        <div class="mainContentPanel">
                            <div class="mainContentColour">
                                <table class="mainContent" style="width:90%" border="0" cellpadding = "0" cellspacing = "0">
                                    <tr style="height:30px">
                                        <td><label runat = "server" id = "lblRefSurname">Surname</label></td>
                                        <td colspan="3">
                                            <wucTextBox:TextBox runat = "server" ID = "txtRefSurname" width = "50%"/>
                                        </td>
                                        <td style="width:15%" rowspan="10" />
                                    </tr>
                                    <tr style="height:30px">
                                        <td><label runat = "server" id = "lblRefFirstname">Firstname</label></td>
                                        <td colspan="3">
                                            <wucTextBox:TextBox runat = "server" ID = "txtRefFirstname"  width = "50%"/>
                                        </td>
                                    </tr>
                                    <tr style="height:30px">
                                        <td style="width:20%"><label runat = "server" id = "lblRefTitle">Title</label></td>
                                        <td style="width:30%">
                                            <wucTextBox:TextBox runat = "server" ID = "txtRefTitle"  width = "90%"/>
                                        </td>
                                        <td style="width:15%"><label runat = "server" id = "lblRefUseFirst">Use First</label></td>
                                        <td style="width:20%">
                                            <input type = "checkbox" id = "chkRefUseFirst" />
                                        </td>
                                    </tr>
                                    <tr style="height:30px">
                                        <td><label runat = "server" id = "lblRefPhone">Phone</label></td>
                                        <td colspan="3">
                                            <wucTextBox:TextBox runat = "server" ID = "txtRefPhone"  width = "50%"/>
                                        </td>
                                    </tr>
                                    <tr style="height:30px">
                                        <td><label runat = "server" id = "lblRefFax">Fax</label></td>
                                        <td colspan="3">
                                            <wucTextBox:TextBox runat = "server" ID = "txtRefFax"  width = "50%"/>
                                        </td>
                                    </tr>
                                    <tr style="height:30px">
                                        <td><label runat = "server" id = "lblRefAddress1">Address1</label></td>
                                        <td colspan="3">
                                            <wucTextBox:TextBox runat = "server" ID = "txtRefAddress1"  width = "75%"/>
                                        </td>
                                    </tr>
                                    <tr style="height:30px">
                                        <td><label runat = "server" id = "lblRefAddress2">Address2</label></td>
                                        <td colspan="3">
                                            <wucTextBox:TextBox runat = "server" ID = "txtRefAddress2"  width = "75%"/>
                                        </td>
                                    </tr>
                                    <tr style="height:30px">
                                        <td><label runat = "server" id = "lblRefDrCity">City</label></td>
                                        <td colspan="3">
                                            <wucTextBox:TextBox runat = "server" ID = "txtRefCity"  width = "50%"/>
                                        </td>
                                    </tr>
                                    <tr style="height:30px">
                                        <td><label runat = "server" id = "lblRefPostalCode">Postal Code</label></td>
                                        <td colspan="3">
                                            <wucTextBox:TextBox runat = "server" ID = "txtRefPostalCode"  width = "25%"/>
                                        </td>
                                    </tr>
                                    <tr style="height:30px">
                                        <td><label runat = "server" id = "lblRefState">State</label></td>
                                        <td colspan="3">
                                            <wucTextBox:TextBox runat = "server" ID = "txtRefState"  width = "50%"/>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
            <a href="#" class="lbAction" rel="deactivate" style="float:right"><button style="width:100px"  id = "btnCancel">Cancel</button></a>
            <%--<a href="#" class="lbAction" rel="deactivate" style="float:right">--%>
                <input type="button" id = "btnRefDrSave" value="Save" style ="float:right;width:100px" onclick="javascript:btnRefDrSave_onclick();"/>
            <%--</a>--%>
        </div>
    </form>
</body>
</html>
