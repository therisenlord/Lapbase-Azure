// JScript File
var LoadingTimer = 0;

//----------------------------------------------------------------------------------------------------------------
function InitialPage(){
    FetchFieldsCaption(false,  document.getElementById("txtHCulture").value, document.frmGroupReport.name);
    iTimerID = window.setInterval("IsTitlesLoaded();", 1000,"javascript");
    
    document.getElementById("cmbSurgeon_DoctorsList").onchange = function(){
        if (document.getElementById("cmbSurgeon_DoctorsList").selectedIndex == 0){
            document.forms[0].txtSDate_txtGlobal.value = '';
            document.forms[0].txtEDate_txtGlobal.value = '';
        }
        __doPostBack('btnSurgeon','');
    }
    return;
}

function LoadDiv(DivNo)
{
    var div_array = new Array("divGroupReport", "divACSReport", "divBSRReport"); 
    var intTabQty = 3;
        
    for (Xh = 1; Xh <= intTabQty; Xh++){
        try{document.getElementById("li_Div" + Xh).className = "";}catch(e){}
        try{document.getElementById(div_array[Xh-1]).style.display = "none";}catch(e){}
    }
    document.getElementById("li_Div" + DivNo).className = "current";
    
    switch(DivNo)
    {
        case 1 :
            document.getElementById("divGroupReport").style.display = "block";
            break;
            
        case 2 :
            document.getElementById("divACSReport").style.display = "block";            
            LoadDataProc();
            break;
            
        case 3 :
            document.getElementById("divBSRReport").style.display = "block";            
            document.forms[0].ReportCode.value = "BSRREPORT";    
            break;
    }
    return;   
}

//----------------------------------------------------------------------------------------------------------------
function CheckGroupReportRadioBtn(idx){
    document.forms[0].rdGroupReports[idx].checked = true;
    
    $get("chkPatientDetails").disabled = $get("lblPatientDetails").disabled = (idx != 0);
    $get("chkEWLdistribution").disabled = $get("lblEWLDistribution").disabled = (idx != 0);
    
    $get("chkPatientFollowUp").disabled = $get("lblPatientFollowUp").disabled = (idx != 9);
    $get("monthsFollowup").disabled = $get("lblPatientFollowUp").disabled = (idx != 9);
    
    if (idx != 0){
        $get("chkPatientDetails").checked = false;
        $get("chkEWLdistribution").checked = false;
    }
    
    if(idx != 9){
        $get("chkPatientFollowUp").checked = false;
        $get("monthsFollowup").value = "";
    }
    
    switch(idx){     
    
        case 0 :
            document.forms[0].ReportCode.value = 'SummaryByQuarter';
            break;
            
        case 1 : 
            document.forms[0].ReportCode.value = 'COMPSUM';
            break;
            
        case 2 : 
            document.forms[0].ReportCode.value = 'COMPSUMBYPATIENT';
            break;
            
        case 3 :
            document.forms[0].ReportCode.value = 'OperationLOS';
            break;
            
        case 4 :
            document.forms[0].ReportCode.value = 'COEReport';
            break;            
            
        case 5 :
            document.forms[0].ReportCode.value = 'SUMMARYONLY';
            break;             
            
        case 6 :
            document.forms[0].ReportCode.value = 'BMIEWLGraph';
            break;
                     
        case 7 : 
            document.forms[0].ReportCode.value = 'OperationDetails';
            break;
            
        case 8 : 
            document.forms[0].ReportCode.value = 'PatientProgress';
            break;
            
        case 9 : 
            document.forms[0].ReportCode.value = 'PatientList';
            break;
            
        case 10 : 
            document.forms[0].ReportCode.value = 'PatientContact';
            break;
    }
    return;
}

//----------------------------------------------------------------------------------------------------------------
function CheckSelectedReport(){
    if (document.forms[0].ReportCode.value.length == 0)
        for(xh = 0; xh < document.forms[0].rdGroupReports.length; xh++)
            if (document.forms[0].rdGroupReports[xh].checked){
               CheckGroupReportRadioBtn(xh);
               break;
            }
}

//----------------------------------------------------------------------------------------------------------------
function BuildReport(PreviewFlag){
    CheckSelectedReport();
    if (PreviewFlag == 0){
        var frameSRC = "../BuildReportPage.aspx?RP=" + document.forms[0].ReportCode.value;
        document.getElementById('frameReport').src = frameSRC;
        LoadingTimer = window.setInterval("CheckReportIsLoaded();", 1000, "JavaScript");
    }
    else{
        var strParam = CheckParams(), strURL = '../BuildReportPage.aspx?RP=' + document.forms[0].ReportCode.value + ((strParam.length > 0) ? ("&" + strParam) : "");
        //alert(strURL);
        window.open(strURL, null, 'scrollbars=yes,fullscreen=yes,toolbar=no,menubar=no,location=no,resizable=yes');
    }
}

//----------------------------------------------------------------------------------------------------------------
function CheckReportIsLoaded(objFrame){
    if (document.getElementById('frameReport').contentDocument.body.offsetHeight > 0){
        window.clearInterval(LoadingTimer);
        parent.frames[0].print();
    }
}

//----------------------------------------------------------------------------------------------------------------
function CheckParams(){
    var strParam = "";
    
    switch(document.forms[0].ReportCode.value)
    {
        case "SummaryByQuarter" :
            strParam += (document.forms[0].chkPatientDetails.checked ? "PD=1" : "PD=0") + "&";
            strParam += (document.forms[0].chkEWLdistribution.checked ? "EWL=1" : "EWL=0") + "&";
            break;
        
        case "SummaryOnly":
            strParam += "PD=0&EWL=0&";
            break;
            
        case "PatientList" :
            strParam += (document.forms[0].chkPatientFollowUp.checked ? "M="+$get("monthsFollowup").value : "M=0") + "&";
            break;
    }
    //strParam += "Format=1" + GetCriteria();
    if(document.forms[0].ReportCode.value == "BSRREPORT")
        strParam += "Format=2";
    else
        strParam += "Format=" + document.forms[0].cmbReportFormat.value + GetCriteria();
    
    return strParam;
}

//----------------------------------------------------------------------------------------------------------------
function GetCriteria(){
    var strCriteria = "";
    
    strCriteria += (document.forms[0].cmbSurgeon_DoctorsList.selectedIndex == 0) ? "&Surgeon=0" : "&Surgeon="+document.forms[0].cmbSurgeon_DoctorsList.value;
    
    if ((document.forms[0].cmbHospital_HospitalsList.selectedIndex == 0) || (document.forms[0].cmbHospital_HospitalsList.options.length == 0))
        strCriteria += "&Hospital=0" ;
    else strCriteria += "&Hospital="+document.forms[0].cmbHospital_HospitalsList.value;
    
    if ((document.forms[0].cmbRegion_RegionsList.selectedIndex == 0) || (document.forms[0].cmbRegion_RegionsList.options.length == 0))
        strCriteria += "&Region=0" ;
    else strCriteria += "&Region="+document.forms[0].cmbRegion_RegionsList.value;
    
    strCriteria += (document.forms[0].txtSDate_txtGlobal.value.length == 0) ? "&FDate=0" : "&FDate="+document.forms[0].txtSDate_txtGlobal.value;
    strCriteria += (document.forms[0].txtEDate_txtGlobal.value.length == 0) ? "&TDate=0" : "&TDate="+document.forms[0].txtEDate_txtGlobal.value;
    
    strCriteria += (document.forms[0].txtMinAge_txtGlobal.value.length == 0) ? "&FAge=0" : "&FAge="+document.forms[0].txtMinAge_txtGlobal.value;
    strCriteria += (document.forms[0].txtMaxAge_txtGlobal.value.length == 0) ? "&TAge=0" : "&TAge="+document.forms[0].txtMaxAge_txtGlobal.value;
    
    strCriteria += (document.forms[0].txtMinBMI_txtGlobal.value.length == 0) ? "&FBMI=0" : "&FBMI="+document.forms[0].txtMinBMI_txtGlobal.value;
    strCriteria += (document.forms[0].txtMaxBMI_txtGlobal.value.length == 0) ? "&TBMI=0" : "&TBMI="+document.forms[0].txtMaxBMI_txtGlobal.value;    
    
    if ((document.forms[0].cmbGroup_CodeList.selectedIndex == 0) || (document.forms[0].cmbGroup_CodeList.options.length == 0))
        strCriteria += "&Grp=0" ;
    else strCriteria += "&Grp="+document.forms[0].cmbGroup_CodeList.value;
    
    if ((document.forms[0].cmbSurgeryType_SystemCodeList.selectedIndex == 0) || (document.forms[0].cmbSurgeryType_SystemCodeList.options.length == 0))
        strCriteria += "&Sur=0" ;
    else strCriteria += "&Sur="+document.forms[0].cmbSurgeryType_SystemCodeList.value;
    
    if ((document.forms[0].ddlBandType_SystemCodeList.selectedIndex == 0) || (document.forms[0].ddlBandType_SystemCodeList.options.length == 0))
        strCriteria += "&Band=0" ;
    else strCriteria += "&Band="+document.forms[0].ddlBandType_SystemCodeList.value;
        
    if ((document.forms[0].cmbApproach_SystemCodeList.selectedIndex == 0) || (document.forms[0].cmbApproach_SystemCodeList.options.length == 0))
        strCriteria += "&App=0" ;
    else strCriteria += "&App="+document.forms[0].cmbApproach_SystemCodeList.value;
    
    if ((document.forms[0].cmbCategory_CodeList.selectedIndex == 0) || (document.forms[0].cmbCategory_CodeList.options.length == 0))
        strCriteria += "&Cat=0" ;
    else strCriteria += "&Cat="+document.forms[0].cmbCategory_CodeList.value;
    
    if ((document.forms[0].cmbBandSize_SystemCodeList.selectedIndex == 0) || (document.forms[0].cmbBandSize_SystemCodeList.options.length == 0))
        strCriteria += "&Size=0" ;
    else strCriteria += "&Size="+document.forms[0].cmbBandSize_SystemCodeList.value;
    
    strCriteria += (document.forms[0].txtSerialNo_txtGlobal.value.length == 0) ? "&SerNo=0" : "&SerNo="+document.forms[0].txtSerialNo_txtGlobal.value;
    
    return strCriteria;
}
















// JScript File
var PatientRowColor;
var SortPatientID = 1;
var SortPatientSurname = 2;
var SortPatientVisitDate = 3;
var SortPatientFirstname = 4;

var CookieSortBy = "";
var CookieSortOrder = "";
var CookiePageno = 1;
var CookieSurname = "";
var CookieFirstname = "";
var CookieShowType = "";
var CookieFirstletter = "";

var CookieSortDelimiter = "**";


document.onkeypress = CheckEnterKey; 
function CheckEnterKey(evt)
{
    if (!evt) evt = window.event;
    
    if(evt.keyCode == 13){ //if character code is equal to ascii 13 (if enter key) 
        __doPostBack('btnLoad','');
        return false;
    }
}

// this function is called when the page is loaded on client-side
function LoadDataProc()
{
    SetEvents();
    FetchFieldsCaption(false,  document.getElementById("txtHCulture").value, document.frmGroupReport.name);
    iTimerID = window.setInterval("IsTitlesLoaded();", 1000,"javascript");
}

//----------------------------------------------------------------------------------------------------------------
function IsTitlesLoaded(){
    if (document.getElementById("TitleLoaded").value == "1"){
        window.clearInterval(iTimerID);
    }
    return;
}

//----------------------------------------------------------------------------------------------------------------
function SetEvents(){
    document.getElementById('cmbSortBy_SystemCodeList').onchange = function(){cmbSortBy_onchange();}

    var sortBy = "ASC";
        
    if(document.getElementById('cmbSortBy_SystemCodeList').value == SortPatientVisitDate)
        sortBy = "DESC";
    
    var currentTimerPatientID = 0;
    var currentTimerFirstname = 0;
    var currentTimerSurname = 0;
    
    
    //set default value from cookies
    getOnloadCookie();
    
       
    document.getElementById("txtSurName_txtGlobal").onkeyup = function(){
        document.getElementById('cmbSortBy_SystemCodeList').value = SortPatientSurname;
        document.getElementById("lblName_TC").className = "listDown";
        document.getElementById("lblFName_TC").className = "";
        //document.getElementById("lblSurgeryDate_TC").className = "";
        document.getElementById("lblPatientID_TC").className = "";
        clearTimeout(currentTimerSurname);
        currentTimerSurname = setTimeout("document.forms[0].txtHPageNo.value = '1'; LoadAllPatients('ShowAll', document.getElementById('cmbSortBy_SystemCodeList').value, 'ASC');", 600);
    }
    
    document.getElementById("txtName_txtGlobal").onkeyup = function(){
        document.getElementById('cmbSortBy_SystemCodeList').value = SortPatientFirstname;        
        document.getElementById("lblFName_TC").className = "listDown";
        document.getElementById("lblName_TC").className = "";
        //document.getElementById("lblSurgeryDate_TC").className = "";
        document.getElementById("lblPatientID_TC").className = "";
        clearTimeout(currentTimerFirstname);
        currentTimerFirstname = setTimeout("document.forms[0].txtHPageNo.value = '1'; LoadAllPatients('ShowAll', document.getElementById('cmbSortBy_SystemCodeList').value, 'ASC');", 600);
    }
    return;
}


//-----------------------------------------------------------------------------------------------------------------------
function LoadAllPatients(strShowType, sortBy, sortOrder)
{       
    CookieSurname = "";
    CookieFirstname = "";
    CookieShowType = "";
    CookieFirstLetter = "";
    
    getCookieSorting();
   
    CookieSortBy = sortBy;
    CookieSortOrder = sortOrder;
    CookiePageno = document.getElementById("txtHPageNo").value;
    
    getCookieSorting();
    
    if(strShowType.toLowerCase() == "showall")
    {            
        CookieSurname = document.getElementById("txtSurName_txtGlobal").value;
        CookieFirstname = document.getElementById("txtName_txtGlobal").value;
        CookieShowType = "showall";
        
        getCookieSorting();
    }
    else if(strShowType.toLowerCase() =="loadbyfirstletter")
    {                    
        CookieFirstLetter = document.getElementById("txtHText").value;
        CookieShowType = "firstletter";
        getCookieSorting();
        
        document.getElementById("txtSurName_txtGlobal").value = "";
        document.getElementById("txtName_txtGlobal").value = "";
    }
    
    document.getElementById("div_PagesNo").innerHtml = "";
    document.forms[0].txtHShowType.value = strShowType.toUpperCase();
    document.forms[0].txtHSortOrder.value = sortOrder;
    document.forms[0].cmbSortBy_SystemCodeList.value = sortBy;
    
    ShowDivMessage("Loading Patients ...", false);
    __doPostBack('btnLoad','');

    return;
}

//-----------------------------------------------------------------------------------------------------------------------
function btnShowAll_onclick(){
    document.getElementById("txtSurName_txtGlobal").value = "";
    document.getElementById("txtName_txtGlobal").value = "";
    document.getElementById("txtHText").value = "";
   
    document.forms[0].txtHPageNo.value = 1;
    
    LoadAllPatients("ShowAll", document.getElementById("cmbSortBy_SystemCodeList").value, "ASC");
};

//-----------------------------------------------------------------------------------------------------------------------
function cmbSortBy_onchange()
{
    var sortBy = "ASC";
    
    document.getElementById("lblPatientID_TC").className = parseInt(document.getElementById("cmbSortBy_SystemCodeList").value) == SortPatientID ? "listDown" : "";
    document.getElementById("lblName_TC").className = parseInt(document.getElementById("cmbSortBy_SystemCodeList").value) == SortPatientSurname ? "listDown" : "";
    //document.getElementById("lblSurgeryDate_TC").className = parseInt(document.getElementById("cmbSortBy_SystemCodeList").value) == SortPatientVisitDate ? "listDown" : "";
    document.getElementById("lblFName_TC").className = parseInt(document.getElementById("cmbSortBy_SystemCodeList").value) == SortPatientFirstname ? "listDown" : "";
    
    if(parseInt(document.getElementById("cmbSortBy_SystemCodeList").value) == SortPatientVisitDate)
        sortBy = "DESC";    
    
    if (document.getElementById("txtHText").value == "")
        LoadAllPatients("ShowAll", document.getElementById("cmbSortBy_SystemCodeList").value, sortBy);
    else
        LoadAllPatients("LOADBYFIRSTLETTER", document.getElementById("cmbSortBy_SystemCodeList").value, sortBy);     
}

//-----------------------------------------------------------------------------------------------------------------------
function SortByPatientID(obj)
{
    var sortOrder = "";
    
    document.getElementById("lblName_TC").className = "";
    document.getElementById("lblFName_TC").className = "";
    //document.getElementById("lblSurgeryDate_TC").className = "";
    
    if (obj.className == "listDown"){
        obj.className = "listUp";
        sortOrder = "DESC";    
    }
    else if (obj.className == "listUP"){
        obj.className = "listDown";
        sortOrder = "ASC";
    }
    else
    {
        obj.className = "listDown";
        sortOrder = "ASC";
    }
    
    if (document.getElementById("txtHText").value == "")
        LoadAllPatients("ShowAll", SortPatientID, sortOrder);
    else
        LoadAllPatients("LOADBYFIRSTLETTER", SortPatientID, sortOrder);
}

//-----------------------------------------------------------------------------------------------------------------------
function SortByName(obj)
{
    var sortOrder = "";
    
    document.getElementById("lblPatientID_TC").className = "";
    document.getElementById("lblFName_TC").className = "";
    //document.getElementById("lblSurgeryDate_TC").className = "";
    
    if (obj.className == "listDown"){
        obj.className = "listUp";
        sortOrder = "DESC";    
    }
    else if (obj.className == "listUP"){
        obj.className = "listDown";
        sortOrder = "ASC";
    }
    else
    {
        obj.className = "listDown";
        sortOrder = "ASC";
    }
    
    if (document.getElementById("txtHText").value == "")
        LoadAllPatients("ShowAll", SortPatientSurname, sortOrder);
    else
        LoadAllPatients("LOADBYFIRSTLETTER", SortPatientSurname, sortOrder);
}


function SortByFName(obj)
{
    var sortOrder = "";
    
    document.getElementById("lblPatientID_TC").className = "";
    document.getElementById("lblName_TC").className = "";
    //document.getElementById("lblSurgeryDate_TC").className = "";
    
    if (obj.className == "listDown"){
        obj.className = "listUp";
        sortOrder = "DESC";    
    }
    else if (obj.className == "listUP"){
        obj.className = "listDown";
        sortOrder = "ASC";
    }
    else
    {
        obj.className = "listDown";
        sortOrder = "ASC";
    }
    
    if (document.getElementById("txtHText").value == "")
        LoadAllPatients("ShowAll", SortPatientFirstname, sortOrder);
    else
        LoadAllPatients("LOADBYFIRSTLETTER", SortPatientFirstname, sortOrder);
}
//-----------------------------------------------------------------------------------------------------------------------
function SortBySurgeryDate(obj)
{
    var sortOrder = "";
    
    document.getElementById("lblPatientID_TC").className = "";
    document.getElementById("lblName_TC").className = "";
    document.getElementById("lblFName_TC").className = "";
    
    if (obj.className == "listDown"){
        obj.className = "listUp";
        sortOrder = "ASC";
    }
    else if (obj.className == "listUP"){
        obj.className = "listDown";
        sortOrder = "DESC";
    }
    else
    {
        obj.className = "listDown";
        sortOrder = "DESC";
    }
    
    if (document.getElementById("txtHText").value == "")
        LoadAllPatients("ShowAll", SortPatientVisitDate, sortOrder);
    else
        LoadAllPatients("LOADBYFIRSTLETTER", SortPatientVisitDate, sortOrder);
}

//-----------------------------------------------------------------------------------------------------------------------
function Character_onclick(strCharacter){
    document.getElementById("txtHText").value = strCharacter;
    document.forms[0].txtHPageNo.value = "1";
    LoadAllPatients("LoadByFirstLetter", document.getElementById("cmbSortBy_SystemCodeList").value, "ASC");
}

//-----------------------------------------------------------------------------------------------------------------------
function LoadRecordsOfPage(PageQty, PageNo){
    CookiePageno = PageNo;
    getCookieSorting();
        
    var aPages = document.getElementsByName("aPage");
    
    document.forms[0].txtHPageNo.value = PageNo;
    ShowDivMessage("Loading Patients ...", false);
    __doPostBack('btnLoad','');
}

//-----------------------------------------------------------------------------------------------------------------------
function PageNavigator(strNavigator){
    var PageNo = parseInt(document.forms[0].txtHPageNo.value ),
        PageQty = parseInt(document.forms[0].txtHPageQty.value );
        
    switch(strNavigator){
        case "F":
            PageNo = 1;
            break;
        case "P":
            if (--PageNo < 1) PageNo = 1;
            break;
        case "N":
            if (++PageNo > PageQty) PageNo = PageQty;
            break;
        case "L" :
            PageNo = document.forms[0].txtHPageQty.value ;
            break;
        case "G" :
            PageNo = Math.round(document.forms[0].txtGoToPage.value);
            break;
    }
    document.forms[0].txtHPageNo.value = PageNo;
    ShowDivMessage("Loading Patients ...", false);
    __doPostBack('btnLoad','');
}



//-----------------------------------------------------------------------------------------------------------------------
//cookie function
function getOnloadCookie(){
    //set value from cookies       
    var CookieSorting = GetCookie("sorting") != null?GetCookie("sorting"):"";
    if(CookieSorting != ""){
        var arrSort = CookieSorting.split(CookieSortDelimiter);

        CookieSortBy = arrSort[0] != ""?arrSort[0]:"";
        CookieSortOrder = arrSort[1] != ""?arrSort[1]:"";
        CookiePageno = arrSort[2] != ""?arrSort[2]:1;
        CookieSurname = arrSort[3] != ""?arrSort[3]:"";
        CookieFirstname = arrSort[4] != ""?arrSort[4]:"";
        CookieShowType = arrSort[5] != ""?arrSort[5]:"";
        CookieFirstLetter = arrSort[6] != ""?arrSort[6]:"";
    }
    else
    {        
        CookieSortBy = document.forms[0].cmbSortBy_SystemCodeList.value;
        CookieSortOrder = document.forms[0].txtHSortOrder.value;
    }
    
    var sortArrow = CookieSortOrder == "DESC"?"listDown" : "listUP";
        
    document.getElementById("txtHPageNo").value = CookiePageno;
    document.getElementById('cmbSortBy_SystemCodeList').value = CookieSortBy;
        
    document.getElementById("lblPatientID_TC").className = CookieSortBy == SortPatientID ? sortArrow : "";
    document.getElementById("lblName_TC").className = CookieSortBy == SortPatientSurname ? sortArrow : "";
    document.getElementById("lblFName_TC").className = CookieSortBy == SortPatientFirstname ? sortArrow : "";

    if(CookieShowType == "firstletter")
    {
        document.getElementById("txtHText").value = CookieFirstLetter;        
        LoadAllPatients("LoadByFirstLetter", CookieSortBy, CookieSortOrder);
    }
    else
    {
        document.getElementById("txtSurName_txtGlobal").value = CookieSurname;
        document.getElementById("txtName_txtGlobal").value = CookieFirstname;          
        LoadAllPatients("ShowAll", CookieSortBy, CookieSortOrder);
    }
}

function getCookieSorting(){
    var CookieSorting = CookieSortBy+CookieSortDelimiter+CookieSortOrder+CookieSortDelimiter+CookiePageno+CookieSortDelimiter+CookieSurname+CookieSortDelimiter+CookieFirstname+CookieSortDelimiter+CookieShowType+CookieSortDelimiter+CookieFirstLetter;
    SetCookie("sorting",CookieSorting,"","","","");
}





var today = new Date();
var expiry = new Date(today.getTime() + 365 * 24 * 60 * 60 * 1000);

function getCookieVal (offset) {
    var endstr = document.cookie.indexOf (";", offset);
    if (endstr == -1) { endstr = document.cookie.length; }
    return unescape(document.cookie.substring(offset, endstr));
}

function GetCookie (name) {
    var arg = name + "=";
    var alen = arg.length;
    var clen = document.cookie.length;
    var i = 0;
    while (i < clen) {
        var j = i + alen;
        if (document.cookie.substring(i, j) == arg) {
            return getCookieVal (j);
        }
        i = document.cookie.indexOf(" ", i) + 1;
        if (i == 0) break; 
    }
    return null;
}

function SetCookie (name,value,expires,path,domain,secure) {
    document.cookie = name + "=" + escape (value) +
    ((expires) ? "; expires=" + expires.toGMTString() : "") +
    ((path) ? "; path=" + path : "") +
    ((domain) ? "; domain=" + domain : "") +
    ((secure) ? "; secure" : "");
}

//-----------------------------------------------------------------------------------------------------------------------
function ACSForm_onclick(form,value,patientid){
    document.getElementById("txtHPatientId").value = patientid;
    document.getElementById("txtHACSForm").value = form;
    document.getElementById("txtHACSValue").value = value;
    
    SetCookie("PatientID",patientid,"","","","");
    
    __doPostBack('btnACSForm','');    
}
    
function ACSForm_displayReport(){
    strURL = '../BuildReportPage.aspx?RP=ACS&F='+document.getElementById("txtHACSForm").value+'&PID='+document.getElementById("txtHPatientId").value+'&P='+document.getElementById("form5Period").options[document.getElementById("form5Period").selectedIndex].value+'&PN='+document.getElementById("form5PeriodNum").value;
    window.open(strURL, null, 'scrollbars=yes,fullscreen=yes,toolbar=no,menubar=no,location=no,resizable=yes');  
}