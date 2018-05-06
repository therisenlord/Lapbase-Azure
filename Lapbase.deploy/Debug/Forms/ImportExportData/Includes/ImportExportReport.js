function controlBar_Buttons_OnClick(buttonNo)
{
    var idx = 1;
    
    for(; idx <= 2; idx++) 
        try{$get('li_Div' + idx).className = (idx != buttonNo) ? "" : "current";}
        catch(e){}
        
    try{$get("div_vExport").style.display = (buttonNo == 1) ? "block" : "none";}catch(e){}
    try{$get("div_vImportExportMDB").style.display = (buttonNo == 2) ? "block" : "none";}catch(e){}
    $get("txtHPageNo").value = buttonNo;
    
    return;
}

function CheckGroupReportRadioBtn(idx){
    document.forms[0].rdGroupReports[idx].checked = true;
    
    switch(idx){     
    
        case 0 :
            document.forms[0].ReportCode.value = 'RepPatientList';
            break;
            
        case 1 : 
            document.forms[0].ReportCode.value = 'RepVisitList';
            break;
            
        case 2 : 
            document.forms[0].ReportCode.value = 'RepOperationList';
            break;
    }
    return;
}

//----------------------------------------------------------------------------------------------------------------
function BuildReport(PreviewFlag){
    if (PreviewFlag == 0){
        var frameSRC = "../../GroupReports/BuildReportPage.aspx?RP=" + document.forms[0].ReportCode.value + "&Format=2";
        document.getElementById('frameReport').src = frameSRC;
        LoadingTimer = window.setInterval("CheckReportIsLoaded();", 1000, "JavaScript");
    }
    else{
        var strURL = '../../GroupReports/BuildReportPage.aspx?RP=' + document.forms[0].ReportCode.value + "&Format=2";
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