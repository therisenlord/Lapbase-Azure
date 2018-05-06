<%@ control language="C#" autoeventwireup="true" inherits="UserControl_MenuWUCtrl, Lapbase.deploy" %>

<div class="headerArea">
    <div class="logoArea">
        <a id="A1" runat="server" href = "~/Forms/PatientsVisits/PatientsVisitsForm.aspx">
            <img runat = "server" id = "imgLogo" src="~/img/logo.gif" alt="" style="border:0"/>
        </a>
    </div>
    <div class="menuArea">
        <div id="mainMnu">
            <ul id="navlist">
                <li><a id = "mnuItem01" runat = "server" href= "~/Forms/PatientsVisits/PatientsVisitsForm.aspx" >
                        <img src="~/img/btn_Patients.gif" alt="Home" height="16" width="16" runat = "server"/><span id = "lblHome">Patients</span></a></li>
                <li id = "menuReports" runat="server"><a id = "mnuItem02" runat = "server" href= "~/GroupReports/GroupReport/GroupReportForm.aspx" >
                        <img src="~/img/btn_reports.gif" alt="Reports" height="16" width="16" runat = "server"/><span id = "lblReports">Reports</span></a></li>                
                 <li><a id = "mnuItem03" runat = "server" href= "~/Forms/MenuSetup/MenuSetupForm.aspx" >
                        <img src="~/img/btn_settings.gif" alt="Settings" height="16" width="16" runat = "server"/><span id = "lblSettings">Settings</span></a></li>
                <li id = "menuBSR" runat="server"><a id = "mnuItem07" runat = "server" href= "~/GroupReports/GroupReport/BSRForm.aspx" >
                        <img id="Img2" src="~/img/btn_reports.gif" alt="Bariatric Surgery Report" height="16" width="16" runat = "server"/><span id = "Span1">BSR</span></a></li>                
               <!--
                <li><a id = "mnuItem04" runat = "server" href="#">
                        <img src="~/img/btn_security.gif" alt="Security" height="16" width="16" runat = "server"/><span id = "lblSecurity">Security</span></a></li>
                -->
                <li id = "menuImportCSV" runat="server"><a id = "A2" href = "~/Forms/ImportExportData/ImportCSV.aspx"  runat = "server">
                    <img id="Img1" src="~/img/btn_import_export.gif" alt="Import CSV" height="16" width="16" runat = "server"/><span id = "lblImportCSV">Import</span></a></li>
                <li id = "menuExportCSV" runat="server"><a id = "A3" href = "~/Forms/ImportExportData/ImportExportDatabase.aspx"  runat = "server">
                    <img id="Img3" src="~/img/btn_import_export.gif" alt="Export" height="16" width="16" runat = "server"/><span id = "lblExport">Export</span></a></li>
                <li id = "menuImport" runat="server"><a id = "mnuItem06" href = "~/Forms/ImportExportData/ImportPathology.aspx"  runat = "server">
                    <img src="~/img/btn_import_export.gif" alt="Pathology" height="16" width="16" runat = "server"/><span id = "lblImport">Pathology</span></a></li>
                <li><a id = "mnuItem05" runat = "server" href="~/Default.aspx">
                        <img src="~/img/btn_log_out.gif" alt="Log Out" height="16" width="16" runat = "server"/><span id = "lblLogout" >Log out</span></a></li>
            </ul>                     
        </div>
    </div>
</div>