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
    FetchFieldsCaption(false,  document.getElementById("txtHCulture").value, document.frmPatientsList.name);
    iTimerID = window.setInterval("IsTitlesLoaded();", 1000);
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
    
    
    document.getElementById("txtPatientID_txtGlobal").onkeyup = function(){
        clearTimeout(currentTimerPatientID);
        currentTimerPatientID = setTimeout("txtPatientID_onchange();", 1200);
    }
    
    document.getElementById("txtSurName_txtGlobal").onkeyup = function(){
        document.getElementById('cmbSortBy_SystemCodeList').value = SortPatientSurname;
        document.getElementById("lblName_TC").className = "listDown";
        document.getElementById("lblFName_TC").className = "";
        document.getElementById("lblSurgeryDate_TC").className = "";
        document.getElementById("lblPatientID_TC").className = "";
        clearTimeout(currentTimerSurname);
        currentTimerSurname = setTimeout("document.forms[0].txtHPageNo.value = '1'; LoadAllPatients('ShowAll', document.getElementById('cmbSortBy_SystemCodeList').value, 'ASC');", 600);
    }
    
    document.getElementById("txtName_txtGlobal").onkeyup = function(){
        document.getElementById('cmbSortBy_SystemCodeList').value = SortPatientFirstname;        
        document.getElementById("lblFName_TC").className = "listDown";
        document.getElementById("lblName_TC").className = "";
        document.getElementById("lblSurgeryDate_TC").className = "";
        document.getElementById("lblPatientID_TC").className = "";
        clearTimeout(currentTimerFirstname);
        currentTimerFirstname = setTimeout("document.forms[0].txtHPageNo.value = '1'; LoadAllPatients('ShowAll', document.getElementById('cmbSortBy_SystemCodeList').value, 'ASC');", 600);
    }
    return;
}


//-----------------------------------------------------------------------------------------------------------------------
function LoadAllPatients(strShowType, sortBy, sortOrder)
{   
    /*
    DeleteCookie("surname","","");
    DeleteCookie("firstname","","");
    DeleteCookie("showtype","","");
    DeleteCookie("firstletter","","");
    */
    
    
    CookieSurname = "";
    CookieFirstname = "";
    CookieShowType = "";
    CookieFirstLetter = "";
    
    getCookieSorting();
    
    if(document.getElementById("txtPatientID_txtGlobal").value != "")
        txtPatientID_onchange();
    else
    {       
        //SetCookie("sortby",sortBy,"","","","");
        //SetCookie("sortorder",sortOrder,"","","",""); 
        //SetCookie("pageno",document.getElementById("txtHPageNo").value,"","","","")
        
        CookieSortBy = sortBy;
        CookieSortOrder = sortOrder;
        CookiePageno = document.getElementById("txtHPageNo").value;
        
        getCookieSorting();
        
        if(strShowType.toLowerCase() == "showall")
        {
            //SetCookie("surname",document.getElementById("txtSurName_txtGlobal").value,"","","","");
            //SetCookie("firstname",document.getElementById("txtName_txtGlobal").value,"","","","");
            //SetCookie("showtype","showall","","","","");
            
            CookieSurname = document.getElementById("txtSurName_txtGlobal").value;
            CookieFirstname = document.getElementById("txtName_txtGlobal").value;
            CookieShowType = "showall";
            
            getCookieSorting();
        }
        else if(strShowType.toLowerCase() =="loadbyfirstletter")
        {        
            //SetCookie("firstletter",document.getElementById("txtHText").value,"","","","");
            //SetCookie("showtype","firstletter","","","","");
            
            CookieFirstLetter = document.getElementById("txtHText").value;
            CookieShowType = "firstletter";
            getCookieSorting();
            
            document.getElementById("txtPatientID_txtGlobal").value = "";
            document.getElementById("txtSurName_txtGlobal").value = "";
            document.getElementById("txtName_txtGlobal").value = "";
        }
        
        document.getElementById("div_PagesNo").innerHtml = "";
        document.forms[0].txtHShowType.value = strShowType.toUpperCase();
        document.forms[0].txtHSortOrder.value = sortOrder;
        document.forms[0].cmbSortBy_SystemCodeList.value = sortBy;
        
        ShowDivMessage("Loading Patients ...", false);
        __doPostBack('btnLoad','');
    }
    return;
}

//-----------------------------------------------------------------------------------------------------------------------
function btnShowAll_onclick(){
    document.getElementById("txtPatientID_txtGlobal").value = "";
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
    document.getElementById("lblSurgeryDate_TC").className = parseInt(document.getElementById("cmbSortBy_SystemCodeList").value) == SortPatientVisitDate ? "listDown" : "";
    document.getElementById("lblFName_TC").className = parseInt(document.getElementById("cmbSortBy_SystemCodeList").value) == SortPatientFirstname ? "listDown" : "";
    
    if(parseInt(document.getElementById("cmbSortBy_SystemCodeList").value) == SortPatientVisitDate)
        sortBy = "DESC";    
    
    if (document.getElementById("txtHText").value == "")
        LoadAllPatients("ShowAll", document.getElementById("cmbSortBy_SystemCodeList").value, sortBy);
    else
        LoadAllPatients("LOADBYFIRSTLETTER", document.getElementById("cmbSortBy_SystemCodeList").value, sortBy);     
}

//-----------------------------------------------------------------------------------------------------------------------
function txtPatientID_onchange(){
    if (document.getElementById("txtPatientID_txtGlobal").value != "")
        __doPostBack('btnCheckPatientID','');
}

//-----------------------------------------------------------------------------------------------------------------------
function SortByPatientID(obj)
{
    var sortOrder = "";
    
    document.getElementById("lblName_TC").className = "";
    document.getElementById("lblFName_TC").className = "";
    document.getElementById("lblSurgeryDate_TC").className = "";
    
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
    document.getElementById("lblSurgeryDate_TC").className = "";
    
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
    document.getElementById("lblSurgeryDate_TC").className = "";
    
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
function PatientRow_onclick(strPatientID){
    $get("txtHPatientId").value = strPatientID;
    __doPostBack('btnGoToVisit','');
    //document.location.assign(document.getElementById("txtHApplicationURL").value + "/Forms/PatientsVisits/ConsultFU1/ConsultFU1Form.aspx?QSN=PID&QSV=" + strPatientID);
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
    document.getElementById("lblSurgeryDate_TC").className = CookieSortBy == SortPatientVisitDate ? sortArrow : "";
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

function DeleteCookie (name,path,domain) {
  if (GetCookie(name)) {
    document.cookie = name + "=" +
    ((path) ? "; path=" + path : "") +
    ((domain) ? "; domain=" + domain : "") +
    "; expires=Thu, 01-Jan-70 00:00:01 GMT";
    }
  }

function SetCookie (name,value,expires,path,domain,secure) {
    document.cookie = name + "=" + escape (value) +
    ((expires) ? "; expires=" + expires.toGMTString() : "") +
    ((path) ? "; path=" + path : "") +
    ((domain) ? "; domain=" + domain : "") +
    ((secure) ? "; secure" : "");
}