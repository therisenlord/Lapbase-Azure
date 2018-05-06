var NewWin = false;

//----------------------------------------------------------------------------------------------------------------
function InitializePage(){
    if (document.getElementById("cmbLanguage").style.display.toLowerCase() == "block")
        FetchFieldsCaption(false, document.getElementById("cmbLanguage").value, document.frmLogon.name);
    else{
        var strCultureInfo = document.all ? document.getElementById("lblCultureInfo").innerText : document.getElementById("lblCultureInfo").textContent;
        FetchFieldsCaption(false, strCultureInfo, document.frmLogon.name);
    }
    iTimerID = window.setInterval("IsTitlesLoaded();", 1000,"javascript");
}

//----------------------------------------------------------------------------------------------------------------
function IsTitlesLoaded(){
    if (document.getElementById("TitleLoaded").value == "1"){
        window.clearInterval(iTimerID);
        SetEvents();
    }
    return;
}

//----------------------------------------------------------------------------------------------------------------
function SetEvents(){
    document.getElementById("txtUserID_txtGlobal").onkeydown = function(e){
        if (document.all) e = event;
        if (e.keyCode == 13) __doPostBack('btnLinkLogin','');
    }
    
    document.getElementById("txtUserPW_txtGlobal").onkeydown = function(e){
        if (document.all) e = event;
        if (e.keyCode == 13) __doPostBack('btnLinkLogin','');
    }
    return;
}

//----------------------------------------------------------------------------------------------------------------
function cmbLanguage_onchange(){
    FetchFieldsCaption(document.getElementById("bodyLogon"), document.getElementById("cmbLanguage").value, document.frmLogon.name);
}