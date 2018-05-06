// JScript File

/****************************************************************************************************************
This section contains all function about Biochemistry Page
****************************************************************************************************************/
String.prototype.trim = function () {
    return this.replace(/^\s*/, "").replace(/\s*$/, "");
}

function aCalendar_onclick(obj){
    var strDateformat = document.getElementById("lblDateFormat");
    languageCode = document.forms[0].txtHCulture.value.substr(0, 2);
    displayCalendar(document.forms[0].txtDate_com_txtGlobal, document.all ? strDateformat.innerText : strDateformat.textContent, obj)
} 

//----------------------------------------------------------------------------------------------------------------
function InitializePage(){
    Biochemistry_ClearFields();
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
function btnSaveBiochemistry_onclick() {
    var tempBiochemistryValue = "";
    var tempValue = "";
    
    //construct BiochemistryValue    
    biochemList = document.getElementById("tblBiochemistryList").getElementsByTagName("input");
    for(i=0; i < biochemList.length; i++)
    {
        tempValue = biochemList[i].value.trim();
        if(tempValue != "")
            tempBiochemistryValue += biochemList[i].id + "_" + tempValue + "__";
    }
    
    biochemChoiceList = document.getElementById("tblBiochemistryChoiceList").getElementsByTagName("select");
    for(i=0; i < biochemChoiceList.length; i++)
    {
        tempValue = biochemChoiceList[i].value.trim();
        if(tempValue != "")
            tempBiochemistryValue += biochemChoiceList[i].id + "_" + tempValue + "__";            
    }    
    document.forms[0].txtHPatientBiochemistryValue.value = tempBiochemistryValue;

    __doPostBack('linkBtnSave','');
    return;
}

//----------------------------------------------------------------------------------------------------------------
function btnCancelBiochemistry_onclick(btnCancel){   
    if (document.getElementById("btnSaveBiochemistry").style.display == "none"){
        ShowBiochemistryFormDiv(1); 
        btnCancel.value = "Cancel";
        document.forms[0].txtDate_com_txtGlobal.value = document.getElementById("txtHCurrentDate").value;
    }
    else
    {
        Biochemistry_ClearFields();        
        document.forms[0].btnSaveBiochemistry.disabled = false;
    }
    return;
}

//----------------------------------------------------------------------------------------------------------------
function ShowBiochemistryFormDiv(ShowFlag){ 
    document.getElementById("btnCancelBiochemistry").value = (ShowFlag == 1) ? "Cancel" : "Add new Biochemistry"; 
    document.getElementById("btnSaveBiochemistry").style.display = (ShowFlag == 1) ? "block" : "none"; 
    document.getElementById("btnDeleteBiochemistry").style.display = (ShowFlag == 1 && document.forms[0].txtHPatientBiochemistryID.value >0) ? "block" : "none"; 
    document.getElementById("div_BiochemistryData").style.display = (ShowFlag == 1) ? "block" : "none";
    
    return;
}

//----------------------------------------------------------------------------------------------------------------
function Biochemistry_ClearFields(){
    ShowBiochemistryFormDiv(0); 
    
    
    document.getElementById('divErrorMessage').style.display = 'none';
    document.forms[0].txtHDateCreated.value = 0;
    document.forms[0].txtHPatientBiochemistryID.value = 0; 
    document.forms[0].txtHPatientBiochemistryValue.value = "";     
    document.forms[0].txtDate_com_txtGlobal.value = document.getElementById("txtHCurrentDate").value; 
    Biochemistry_ClearList();
}

function Biochemistry_ClearList(){
    //clear all biochemistry value
    biochemList = document.getElementById("tblBiochemistryList").getElementsByTagName("input");
    for(i=0; i < biochemList.length; i++)
    {
        tempBiochemistryValue = "document.forms[0]."+biochemList[i].id+".value=''";
        eval(tempBiochemistryValue);
    }
    
    biochemChoiceList = document.getElementById("tblBiochemistryChoiceList").getElementsByTagName("select");
    for(i=0; i < biochemChoiceList.length; i++)
    {     
        tempBiochemistryValue = "document.forms[0]."+biochemChoiceList[i].id+".selectedIndex='0'";        
        eval(tempBiochemistryValue);    
    }
    
}

//-----------------------------------------------------------------------------------------------------------------
function PatientBiochemistryRow_onclick(strBiochemistryID){
    document.getElementById("txtHPatientBiochemistryID").value = strBiochemistryID;
    ShowBiochemistryFormDiv(1);
    LoadPatientBiochemistryData("tblPatientBiochemistryRow_" + strBiochemistryID ,  strBiochemistryID );
    return;
}


//-----------------------------------------------------------------------------------------------------------------
function LoadPatientBiochemistryData(tblBiochemistryRow, strBiochemistryID) 
{ 
    var Rows = document.getElementById(tblBiochemistryRow).rows; 
    var biochemistryValue;
    var biochemistryArr;
    var biochemistryCodeValue;    
    
    Biochemistry_ClearList();
    
    document.getElementById("txtHPatientBiochemistryID").value = strBiochemistryID; 
    
    for (rowIdx = 0; rowIdx < Rows.length; rowIdx++) 
    { 
        var Cells = Rows[rowIdx].cells; 
        var element = "";
        var j=0;
        for (cellIdx = 0; cellIdx < Cells.length; cellIdx++) 
            if (rowIdx == 0) 
                switch(cellIdx) 
                { 
                    case 0 : // document.all means IE browser
                        document.forms[0].txtDate_com_txtGlobal.value = document.all ? Cells[cellIdx].innerText : Cells[cellIdx].textContent; 
                        break; 
                    case 1 : 
                        biochemistryValue = document.all ? Cells[cellIdx].innerText : Cells[cellIdx].textContent;
                        biochemistryArr = biochemistryValue.split("__");

                        //biochemList = document.getElementById("tblBiochemistryList").getElementsByTagName("input");
                        for(i=0; i < biochemistryArr.length; i++)
                        {
                            //check if biochemistry still displayed
                            biochemistryCodeValue = biochemistryArr[i].split("_");
                            
                            tempString = "document.forms[0]."+biochemistryCodeValue[0];
                            element = eval(tempString);
                            if(element){    
                                if(element.type == "select-one"){
                                    for(j=0; j<element.options.length; j++){                                     
                                        if(biochemistryCodeValue[1] == element.options[j].value){
                                            element.options[j].selected = "selected";
                                        }
                                    }
                                }
                                else
                                {      
                                    tempString = "document.forms[0]."+biochemistryCodeValue[0]+".value = '"+biochemistryCodeValue[1]+"'";
                                    
                                    eval(tempString);
                                }
                            }
                        }                    
                        break;                        
                }
    }
    
    //if dataclamp is activated, permission lvl 2 or 3
    //check for created date for this patient
    if(document.getElementById("txtHDataClamp").value.toLowerCase() == "true" && (document.getElementById("txtHPermissionLevel").value == "2t" || document.getElementById("txtHPermissionLevel").value == "3f"))
    {
        var createdDate = document.forms[0].txtHDateCreated.value;        
        
        var currentTime = new Date();
        var currentDate = document.forms[0].txtHCurrentDate.value;

        
        if(createdDate != "" && createdDate != currentDate)
            document.forms[0].btnSaveBiochemistry.disabled = true;            
        else
            document.forms[0].btnSaveBiochemistry.disabled = false;
    }
    
    return;
}

//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
function linkBtnSave_CallBack(ReturnFlag){
    var Idx = 0;
    if (ReturnFlag == true || ReturnFlag == 'delete' || ReturnFlag =='load'){
        if(ReturnFlag && ReturnFlag!= 'load'){
            Biochemistry_ClearFields();
            ShowDivMessage('Data saving is done successfully...', true);
            window.location.reload();
        }
        else if(ReturnFlag == 'delete'){
            Biochemistry_ClearFields();        
            ShowDivMessage('Data deleting is done successfully...', true);        
            window.location.reload();        
        }       
    }
    else{
        document.getElementById('divErrorMessage').style.display = 'block';
        SetInnerText(document.getElementById('pErrorMessage'), 'Error is saving operation data...');
    }
    return;
}

//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
function btnDelete_onclick(){
    var answer = confirm ("It will permanently delete this operation. Do you want to proceed?")
    if (answer){
        __doPostBack('linkBtnDelete','');
    }
    return;
}