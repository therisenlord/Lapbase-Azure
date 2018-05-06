<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test2.aspx.cs" Inherits="Default2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
<form id="form1" runat="server">
   <div>
        <table>
         <tr>
          <td>
           <asp:FileUpload ID="FileUpload1" runat="server" />
          </td>
          <td>
           <asp:Button ID="Button2" runat="server" OnClick="btnDisplay" Text="Display" />
          </td>
         </tr>
         <tr>
           <td colspan="2">
            <asp:Label ID="lblMsg" Text="Please Select Proper File" runat="server" BorderColor="White"
                 Font-Bold="True" ForeColor="Red" Visible="false"></asp:Label>
           </td>
         </tr>
         <tr>
           <td>
           <asp:TextBox ID="txtbx" runat="server" />
            <asp:GridView ID="gv" runat="server"></asp:GridView>
           </td>
         </tr>
         <asp:Label ID="lblMessage" runat="server" />
         <asp:Label ID="lblDisplay" runat="server" />
        </table>
   </div>
   </form>
</body>
</html>