<%@ page language="C#" autoeventwireup="true" inherits="Forms_UploadDocument_UploadDocumentForm, Lapbase.deploy" enableEventValidation="false" viewStateEncryptionMode="Always" %>

<%@ Register TagPrefix="wucTextBox" TagName="TextBox" Src="~/UserControl/TextBoxWUCtrl.ascx" %>
<%@ Register TagPrefix="wucDoc" TagName="DocType" Src="~/UserControl/DocumentTypeWUCtrl.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript" src="../../../Scripts/Global.js" language="javascript"></script>

    <script type="text/javascript" src="Includes/UploadDocument.js" language="javascript"></script>

</head>
<body style="width: 100%;" id="bodyUploadFile">
    <form id="frmUploadDocument" runat="server" enctype="multipart/form-data">
        <%--<asp:ScriptManager ID="upload_ScriptManager" runat="server" EnablePartialRendering = "true" ScriptMode="Auto" ></asp:ScriptManager>--%>
        <div class="fakePopUpWindow">
            <div class="fakePopUpWindowTitle">
                <table width="100%">
                    <tr>
                        <td>
                            <h4 id="lblUploadfile">
                                Upload file</h4>
                        </td>
                        <td align="right">
                            <h4>
                                <asp:Label ID="lblPatientID" runat="server" /></h4>
                        </td>
                    </tr>
                </table>
            </div>
            <%--<asp:UpdatePanel ID="UPanel1" UpdateMode="always" runat="server">
                <ContentTemplate>--%>
            <div class="fakePopUpWindowContent">
                <table width="100%" cellpadding="3" cellspacing="0" border="0">
                    <tr style="height: 30px; vertical-align: top;">
                        <td style="width: 1%" rowspan="6" />
                        <td style="width: 20%;">
                            <asp:Label runat="server" ID="lblDocumentFile" Text="File" />
                        </td>
                        <td>
                            <asp:FileUpload runat="server" ID="uploadDocFile" contentEditable="false" Width="100%" />
                        </td>
                    </tr>
                    <tr style="height: 30px; vertical-align: top;">
                        <td>
                            <asp:Label runat="server" ID="R_lblDocumetName" Text="Label" />
                        </td>
                        <td>
                            <wucTextBox:TextBox runat="server" ID="txtDocLabel" width="50%" />
                        </td>
                    </tr>
                    <tr style="height: 30px; vertical-align: top;">
                        <td>
                            <asp:Label runat="server" ID="lblDocType" Text="Type" />
                        </td>
                        <td>
                            <wucDoc:DocType runat="server" ID="listDocType" />
                        </td>
                    </tr>
                    <tr style="height: 30px; vertical-align: top;">
                        <td>
                            <asp:Label runat="server" ID="R_lblDescription" Text="Description" />
                        </td>
                        <td>
                            <wucTextBox:TextBox runat="server" ID="txtDescription" width="90%" textMode="MultiLine"
                                rows="2" />
                        </td>
                    </tr>
                    <tr style="height: 30px; vertical-align: top;">
                        <td>
                            <asp:Label runat="server" ID="lblDate" Text="Date" /></td>
                        <td>
                            <wucTextBox:TextBox runat="server" ID="txtDate" width="25%" textMode="SingleLine"
                                maxLength="10" />
                        </td>
                    </tr>
                    <tr style="height: 30px; vertical-align: top;">
                        <td>
                            <asp:Label runat="server" ID="lblEvent" Text="Event" /></td>
                        <td>
                            <select runat="server" id="listEventName" style="width: 25%" onchange="javascript:listEventName_onchange(this);">
                                <option value="B">Baseline</option>
                                <option value="O">Operation</option>
                                <option value="V">Visit</option>
                                <option value="C">Comment</option>
                            </select>
                            &nbsp;
                            <select runat="server" id="listEventDate" style="width: 30%">
                                <option value="0">Select from ...</option>
                            </select>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="fakePopUpWindowFooter">
                <a href="#" class="lbAction" rel="deactivate" style="float: right">
                    <button id="btnClose" style="width: 100px">
                        Close</button></a>
                <input type="button" runat="server" id="btnUpload" value="Upload" onclick="javascript:btnUpload_onclick();"
                    style="width: 100px; float: right" />
                <asp:LinkButton runat="server" ID="linkBtn" Text="Click me" OnClick="btnUpload_OnClick"
                    Style="display: none" />
            </div>
            <input type="text" runat="server" id="txtHFileType" style="display: none" />
            <input type="text" runat="server" id="txtHEventID" style="display: none" />
            <input type="text" runat="server" id="txtHEventDate" style="display: none" />
            <asp:HiddenField runat="server" ID="txtHApplicationURL" />
            <asp:HiddenField runat="server" ID="txtHDocumentID" Value="0" />
            <input type="text" runat="Server" id="txtHUploadResult" value="0" style="display: none" />
            <asp:HiddenField runat="Server" ID="txtHCurrentDate" Value="" />
            <asp:HiddenField runat="server" ID="txtHParentURL" />
            <asp:HiddenField runat="Server" ID="txtHCulture" />
            <asp:HiddenField runat="Server" ID="TitleLoaded" Value="0" />
            <textarea runat="server" id="txtTest" rows="5" cols="80" style="display: none" />
            <%--</ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID = "linkBtn" />
                </Triggers>
            </asp:UpdatePanel>--%>
        </div>
    </form>
</body>

<script language="javascript" type="text/javascript">
    switch(parseInt(document.getElementById("txtHUploadResult").value)){
        case 0:
            break;
        case 1:
            document.location.assign(document.getElementById("txtHParentURL").value);
            break;
        case 2:
            alert("Error in uploading document...");
            document.location.assign(document.getElementById("txtHParentURL").value);
            break;
    }
</script>

</html>