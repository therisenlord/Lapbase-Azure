// JScript File
var TabNo = 0;

//-----------------------------------------------------------------------------------------------------------------
function InitialPage()
{
    document.getElementById("AppSchemaMenu_a_Comorbidities").href = "#";
}

//-----------------------------------------------------------------------------------------------------------------
function ComorbiditySubMenuClick(intSubMenuNo){
    document.getElementById("li_Div1").className = "";
    document.getElementById("li_Div2").className = "";
    document.getElementById("li_Div3").className = "";
    document.getElementById("li_Div" + intSubMenuNo).className = "current";
    
    document.getElementById("div_ComorbiditiesList").style.display = (intSubMenuNo == 1) ? "block" : "none";
    document.getElementById("div_MajorComorbidity").style.display = (intSubMenuNo == 2) ? "block" : "none";
    document.getElementById("div_MinorComorbidity").style.display = (intSubMenuNo == 3) ? "block" : "none";
    /*
    switch(intSubMenuNo)
    {
        case 1 :
             LoadComorbidityData();
            break;
        case 2 :
            LoadInvestigationData();
            break;
    }
    */
    return;
}

//-----------------------------------------------------------------------------------------------------------------
function LoadComorbidityData(){
    var requestURL = document.getElementById("txtHApplicationURL").value + "Forms/Comorbidity/ComorbidityForm.aspx?QSN=LoadComorbidity";
    
    TabNo = 1;
    ShowDivMessage("Load comorbidity data ...", false);
    XmlHttpSubmit(requestURL,  LoadInvestigationData_CallBack);
}

//-----------------------------------------------------------------------------------------------------------------
function LoadInvestigationData(){
    var requestURL = document.getElementById("txtHApplicationURL").value + "Forms/Comorbidity/ComorbidityForm.aspx?QSN=LoadInvestigation";
    
    TabNo = 2;
    ShowDivMessage("Load investigation data ...", false);
    XmlHttpSubmit(requestURL,  LoadInvestigationData_CallBack);
    return;
}

//-----------------------------------------------------------------------------------------------------------------
function LoadInvestigationData_CallBack(){
    if (XmlHttp.readyState == 4)
        if (XmlHttp.status == 200){
            HideDivMessage();
            document.getElementById("div_tblComorbiditiesScroll").innerHTML = XmlHttp.responseText;
            if (TabNo == 2) Page_OnLoad();
        }
        else
            document.getElementById("div_tblComorbiditiesScroll").innerHTML = XmlHttp.responseText;
    return;
}
