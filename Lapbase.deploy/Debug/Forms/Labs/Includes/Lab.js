// JScript File

/****************************************************************************************************************
This section contains all function about Lab Page
****************************************************************************************************************/
//----------------------------------------------------------------------------------------------------------------
function InitializePage(){
    Lab_ClearFields();
    
    FetchFieldsCaption(false,  document.getElementById("txtHCulture").value, document.frmLab.name);     
    return;
}

//----------------------------------------------------------------------------------------------------------------
function IsTitlesLoaded(){
    if (document.getElementById("TitleLoaded").value == "1"){
        window.clearInterval(iTimerID);
    }
    return;
}

//----------------------------------------------------------------------------------------------------------------
function btnCancelLab_onclick(btnCancel){
    if (document.getElementById("btnSaveLab").style.display == "none" && document.getElementById("btnDeleteLab").style.display == "none"){
        ShowLabFormDiv(1); 
        Lab_ClearFields();
        btnCancel.value = "Cancel";
    }
    else
    {
        ShowLabFormDiv(0); 
        Lab_ClearFields();        
        document.forms[0].btnSaveLab.disabled = false;
    }
    return;
}

//-----------------------------------------------------------------------------------------------------------------
function LabRow_onclick(strLabID){
    $get("txtHLabID").value = strLabID;
    __doPostBack('linkBtnLoad','');
    return;
}

//----------------------------------------------------------------------------------------------------------------
function Lab_ClearFields(){
    document.forms[0].txtHLabID.value = 0;

    return;
}


//----------------------------------------------------------------------------------------------------------------
function ShowLabFormDiv(ShowFlag){ 
    //document.getElementById("btnCancelLab").value = (ShowFlag == 1) ? "Cancel" : "Add new Lab"; 
    document.getElementById("btnCancelLab").style.display = (ShowFlag == 1 && document.forms[0].txtHLabID.value > 0) ? "block" : "none";
    
    //document.getElementById("btnDeleteLab").style.display = (ShowFlag == 1 && document.forms[0].txtHLabID.value >0) ? "block" : "none";
    //document.getElementById("btnSaveLab").style.display = (ShowFlag == 1 && document.getElementById("btnDeleteLab").style.display == "none") ? "block" : "none"; 
    document.getElementById("div_LabData").style.display = (ShowFlag == 1 && document.forms[0].txtHLabID.value == 0) ? "block" : "none";
    document.getElementById("div_LabList").style.display = (ShowFlag == 1 && document.forms[0].txtHLabID.value > 0) ? "block" : "none";

    //if(document.forms[0].txtHIsBaseline.value == "1")
    //    document.getElementById("btnDeleteLab").style.display = "none";
        
    return;
}

function btnDelete_onclick(){
    $get("txtHDelete").value=0; 
    var answer = confirm ("It will permanently delete this event. Do you want to proceed?")
    if (answer){
        document.getElementById("pathologyResultTable").innerHTML = '';
        $get("txtHDelete").value = 1;
    }
    return;
}

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