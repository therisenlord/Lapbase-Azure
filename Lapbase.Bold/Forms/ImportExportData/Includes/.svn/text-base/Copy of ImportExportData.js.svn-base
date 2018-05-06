var ImportPathologyPageNo = 1;
var PathologyListPageNo = 2;

function controlBar_Buttons_OnClick(buttonNo)
{
    var idx = 1;
    for(; idx <= 2; idx++) 
        try{
            if (idx != buttonNo) 
                document.getElementById('li_Div' + idx).className = "";
            else
                document.getElementById('li_Div' + idx).className = "current";
        }
        catch(e){}
        
        try{document.getElementById("divImportPathology").style.display = (buttonNo == ImportPathologyPageNo ) ? "block" : "none";}catch(e){}
        try{document.getElementById("divPathologyList").style.display = (buttonNo == PathologyListPageNo ) ? "block" : "none";}catch(e){}
    return;
}



function InitializePage(){
    Lab_ClearFields();
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
    document.getElementById("btnCancelLab").style.display = (ShowFlag == 1 && document.forms[0].txtHLabID.value > 0) ? "block" : "none";
    document.getElementById("btnDeleteLab").style.display = (ShowFlag == 1 && document.forms[0].txtHLabID.value > 0) ? "block" : "none";
    
    document.getElementById("div_LabData").style.display = (ShowFlag == 1 && document.forms[0].txtHLabID.value == 0) ? "block" : "none";
    document.getElementById("div_LabList").style.display = (ShowFlag == 1 && document.forms[0].txtHLabID.value > 0) ? "block" : "none";

    return;
}

function btnCancelLab_onclick(btnCancel){
    if (document.getElementById("btnDeleteLab").style.display == "none"){
        ShowLabFormDiv(1); 
        Lab_ClearFields();
    }
    else
    {
        ShowLabFormDiv(0); 
        Lab_ClearFields();        
    }
    return;
}