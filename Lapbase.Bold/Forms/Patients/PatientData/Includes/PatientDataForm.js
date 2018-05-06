var LoadTimer;          // used to check if data are loaded, each 1 second
var LoadFlag = false;
var ajax_optionDiv = false; // ajax_optionDiv and ajax_optionDiv_iframe are used for floading div
var ajax_optionDiv_iframe = false;
var FillDivDropdownList = false;
var LimitToList = false;
var ColumnWidth = new String();
var DivButtonNo = 0; // this is used when user clicks on sub-titles under Patient details (they are Demographics, Height/Weight/Notes, Major Comorbidity and minor comorbidity)
var AppSchema_ButtonNo = 0; // this is used when user clicks on application schema menu-bar, (they are Patient list, patient detail, visit, operations, file and complication)

var tempTable = null;
var AppSchemaMenu = new Array("AppSchemaMenu_a_Home", "AppSchemaMenu_a_Baseline", "AppSchemaMenu_a_Visits", 
                        "AppSchemaMenu_a_Operations", "AppSchemaMenu_a_Files", "AppSchemaMenu_a_Complications", "AppSchemaMenu_a_EMR");
var AppSchemaMenu_href = new Array("/Forms/PatientsVisits/PatientsVisitsForm.aspx", 
                        "/Forms/Patients/PatientData/PatientDataForm.aspx", 
                        "/Forms/PatientsVisits/ConsultFU1/ConsultFU1Form.aspx", 
                        "/Forms/Patients/Operations/PatientOperationForm.aspx", 
                        "/Forms/FileManagement/FileManagementForm.aspx?ReLoad=1&QSN=Type&SD=0", 
                        "/Forms/Adverse/AdverseForm.aspx", 
                        "/Forms/EMR/EMRForm.aspx");

var divDemos = "div_vDemographics";
var divDetails = "div_vDetails";
var divBold = "div_vBoldData";
var divComorbidity = "div_vBoldComorbidity";
var divMedications = "div_vMedications";
var divComorbidityNotes = "div_vBoldComorbidityNotes";
var divPrevSurgery = "div_vPreviousSurgery";
    
    
var activeItem = 0, activeItemInDiv = 0, RecHeight = 20, RecNoInDiv = parseInt(200 / RecHeight) ; //200 is height of divScroll in Scroll-rel.css
var scrollbuttonDirection = 0;
var scrollbuttonSpeed = 2;  // How fast the content scrolls when you click the scroll buttons(Up and down arrows)
var scrollTimer = 10;	    // Also how fast the content scrolls. By decreasing this value, the content will move faster	
var mainDropDownList = false;
var intSubPageQty = 9; // number of sub-pages

var loadInputValue = "";
var section = divDemos;

//---------------------------------------------------------------------------------------------------------------
/*
this funcion is called when page is loaded and calls the function to load all captions based of selected language.
a timer function checks the process of translating is done or not.
*/
function InitializePage()
{
    SetEvents();
    //iTimerID = window.setInterval("IsTitlesLoaded();", 1000,"javascript");
    //FetchFieldsCaption(false,  $get("txtHCulture").value, document.frmPatientData.name);
    
    FillBoldLists();
    SetAllHyperLinkStatus(false);
    
    if(loadInputValue == "")
        loadInputValue = LoadTempDetails();
        
    return;
}

//----------------------------------------------------------------------------------------------------------------
/*
this function is to check whether all captions are loaded or not. if so, load patient data of demographic sub-page
*/
function IsTitlesLoaded(){
    if ($get("TitleLoaded").value == "1"){
        window.clearInterval(iTimerID);
        FillBoldLists();
        SetAllHyperLinkStatus(false);
//        LoadPatientData(1);
    }
    return;
}

//---------------------------------------------------------------------------------------------------------------
/*
this function and the next function "FillBoldList" are to fill Prev Bariatric, non-Bariatric and Adverse Events lists
*/
function FillBoldLists(){
    FillBoldList(document.forms[0].cmbPrevBariatricSurgery_SystemCodeList, document.forms[0].listPrevBariatricSurgery);
    FillBoldList(document.forms[0].cmbPrevNonBariatricSurgery_SystemCodeList, document.forms[0].listPrevNonBariatricSurgery);
    FillBoldList(document.forms[0].cmbAdverseEvents_SystemCodeList, document.forms[0].listAdverseEvents);
}

function FillBoldList(listSource, listDest){
    // The First Row (xh = 0) is "Select from...", we don't add this row to the Dest list
    for(xh = 1; xh < listSource.options.length; xh++){
        var option = document.createElement("OPTION");
        
        option.value = listSource.options[xh].value;
        option.text = listSource.options[xh].text;
        listDest.options.add(option);
    }
}


//---------------------------------------------------------------------------------------------------------------
/*
because of using ASP.NET Components, this function is to set client-side events and functions for these components
*/
function SetEvents()
{
    //for (Xh = 0; Xh < AppSchemaMenu.length; Xh++)
    //    $get(AppSchemaMenu[Xh]).href = "#";
        
    document.onclick  = function(){
        if (ajax_optionDiv && ajax_optionDiv.style.display == "block"){
            ajax_optionDiv.style.display = "none";
            if (ajax_optionDiv_iframe)
                ajax_optionDiv_iframe.style.display = "none";
        }
    }

    
    $get("txtBirthDate_txtGlobal").onchange = function(){txtBirthDate_onchange();}
    $get("txtHeight_txtGlobal").onchange = function(){txtHeight_onchange();}
    $get("txtStartWeight_txtGlobal").onchange = function(){txtStartWeight_onchange();}
    $get("txtIdealWeight_txtGlobal").onchange = function(){txtIdealWeight_onchange();}
    $get("txtTargetWeightBaseline_txtGlobal").onchange = function(){txtTargetWeight_onchange();}

    $get("txtReferredBy_txtGlobal").onkeyup = function(e){
        txtObject = $get("txtReferredBy_txtGlobal");
        txtHiddenObject = $get('txtHReferredBy');
        LimitToList = true;
        ColumnWidth = "20%;50%";
        ajax_showOptions(e , "cmbReferredDoctorsList");
    }
   
    $get("txtOtherDoctors1_txtGlobal").onkeyup = function(e){
        txtObject = $get("txtOtherDoctors1_txtGlobal");
        txtHiddenObject = $get('txtHOtherDoctors1');
        LimitToList = true;
        ColumnWidth = "20%;50%";
        ajax_showOptions(e , "cmbReferredDoctorsList");
    }

    $get("txtOtherDoctors2_txtGlobal").onkeyup = function(e){
        txtObject = $get("txtOtherDoctors2_txtGlobal");
        txtHiddenObject = $get('txtHOtherDoctors2');
        LimitToList = true;
        ColumnWidth = "20%;50%";
        ajax_showOptions(e , "cmbReferredDoctorsList");
    }

    $get("txtCity_txtGlobal").onkeyup = function(e){
        txtObject = $get("txtCity_txtGlobal");
        txtHiddenObject = $get('txtHCity');
        LimitToList = false;
        ColumnWidth = "40%;60%";
        ajax_showOptions(e , "cmbCity");
    }

    $get("txtInsurance_txtGlobal").onkeyup = function(e){
        txtObject = $get("txtInsurance_txtGlobal");
        txtHiddenObject = $get('txtHInsurance');
        LimitToList = false;
        ColumnWidth = "0;100%";
        ajax_showOptions(e , "cmbInsurance");
    }
    
    SetPatientTitleEvents();
    var objPatientID = $get("txtPatientID");
    $get("tblPatientTitle_div_PatientTitle").style.display = ((objPatientID.value == "0") || (objPatientID.value == "")) ? "none" : "block";

    $get("txtPrevBariatricSearch_txtGlobal").onkeyup = function(){
        FilterListBySearchWord("txtPrevBariatricSearch_txtGlobal", "cmbPrevBariatricSurgery_SystemCodeList", "listPrevBariatricSurgery");
    }

    $get("txtPrevNonBarSurgeriesSearch_txtGlobal").onkeyup = function(){
        FilterListBySearchWord("txtPrevNonBarSurgeriesSearch_txtGlobal", "cmbPrevNonBariatricSurgery_SystemCodeList", "listPrevNonBariatricSurgery");
    }

    $get("txtAdverseEventsSearch_txtGlobal").onkeyup = function(){
        FilterListBySearchWord("txtAdverseEventsSearch_txtGlobal", "cmbAdverseEvents_SystemCodeList", "listAdverseEvents");
    }
    return;
}

//---------------------------------------------------------------------------------------------------------------
/*
    because of using autosave function, we need to save patient data whenever the application menu bar items are clicked.
    the application menu-bar items are : Patients list, Patient details, visits, operations, files and complications.
    whenever user click these items, the patient data shoud be saved.
    the saving data procedure is done by calling client-side function "btnSave_onclick()"
*/
function navList_onclick(buttonNo)
{
    AppSchema_ButtonNo = buttonNo;
    btnSave_onclick();
}

//---------------------------------------------------------------------------------------------------------------
/*
    this function is the same with navList_onclick, but just for title hyperlinks of Patient detials, 
    they are 1)Demogrphics 2) Height,Weight and notes 3) Major comorbidity 4) Minor comorbidity
    
    the only difference between this function and "navList_onclick(buttonNo)" function is the achievment after saving data,
    if users click each Application Menu-bar items, the appropiate page should be loaded 
    but for links inside patient details, only appropiate division is displayed and others are hidden
*/
function controlBar_Buttons_OnClick(buttonNo)
{
    DivButtonNo = buttonNo;
    document.forms[0].txtSelectedPageNo.value = buttonNo;
    btnSave_onclick();
    return;
}

//---------------------------------------------------------------------------------------------------------------
function SetDivsStatus(buttonNo){
    var idx = 1;
    for(; idx <= intSubPageQty; idx++) 
        try{
            if (idx != buttonNo) {
                $get('ub_mnuItem0' + idx).style.fontWeight = "normal";
                $get('li_Div' + idx).className = "";
            }
            else{
                $get('ub_mnuItem0' + idx).style.fontWeight = "bold";
                $get('li_Div' + idx).className = "current";
            }
        }
        catch(e){}

    try{$get(divDemos).style.display = (buttonNo == 1) ? "block" : "none";}catch(e){}
    try{$get(divDetails).style.display = (buttonNo == 2) ? "block" : "none";}catch(e){}
    try{$get(divBold).style.display = (buttonNo == 5) ? "block" : "none";}catch(e){}
    try{$get(divComorbidity).style.display = (buttonNo == 6) ? "block" : "none";}catch(e){}
    try{$get(divMedications).style.display = (buttonNo == 7) ? "block" : "none";}catch(e){}
    try{$get(divComorbidityNotes).style.display = (buttonNo == 8) ? "block" : "none";}catch(e){}
    try{$get(divPrevSurgery).style.display = (buttonNo == 9) ? "block" : "none";}catch(e){}
    
    switch(parseInt(buttonNo))
    {
        case 1:
        section = divDemos;
        break;        
        case 2:
        section = divDetails;
        break;
        case 5:
        section = divBold;
        break;
        case 6:
        section = divComorbidity;
        break;
        case 7:
        section = divMedications;
        break;
        case 8:
        section = divComorbidityNotes;
        break;
        case 9:
        section = divPrevSurgery;
        break;
    }
    
    loadInputValue = LoadTempDetails();
    
    $get("txtHPageNo").value = buttonNo;
}

//---------------------------------------------------------------------------------------------------------------
/*
    all hyperlinks should be disable while data are loaded.
    because while the page is empty and user clicks any hyperlink, the empty page is saved, so data will be missed
*/
function SetAllHyperLinkStatus(disabled){
    var Xh = 0;
    
    for (Xh = 0; Xh < AppSchemaMenu.length; Xh++)
        $get(AppSchemaMenu[Xh]).disabled = disabled;

    if (disabled){
        $get("ub_mnuItem01").onclick = "";
        $get("ub_mnuItem02").onclick = "";
        
        for (Xh = 0; Xh < AppSchemaMenu.length; Xh++)
            $get(AppSchemaMenu[Xh]).onclick = "";
    }
    else{
        for (Xh = 0; Xh < AppSchemaMenu.length; Xh++)
            $get(AppSchemaMenu[Xh]).onclick = function() {navList_onclick(this.name.replace("a", ""));};

        $get("ub_mnuItem01").onclick = function() {controlBar_Buttons_OnClick(1);};
        $get("ub_mnuItem02").onclick = function() {controlBar_Buttons_OnClick(2);};
        $get("ub_mnuItem03").onclick = function() {controlBar_Buttons_OnClick(3);};
        $get("ub_mnuItem04").onclick = function() {controlBar_Buttons_OnClick(4);};
        $get("ub_mnuItem05").onclick = function() {controlBar_Buttons_OnClick(5);};
        $get("ub_mnuItem06").onclick = function() {controlBar_Buttons_OnClick(6);};
        $get("ub_mnuItem08").onclick = function() {controlBar_Buttons_OnClick(8);};
        $get("ub_mnuItem09").onclick = function() {controlBar_Buttons_OnClick(9);};
    }
    return;
}

//-----------------------------------------------------------------------------------------------------------------
function SetListFieldValue( strObjectName, strValue, strOriginalList){
    //var cmbReferredDoctorsList = $get("cmbReferredDoctorsList");
    var cmbOriginalList = $get(strOriginalList);
    
    txtObject = $get(strObjectName);  
    for (Xh = 1; Xh < cmbOriginalList.options.length; Xh++)
        if (cmbOriginalList.options[Xh].value == strValue)
            txtObject.value = cmbOriginalList.options[Xh].text;
    return;
}

//---------------------------------------------------------------------------------------------------------------
/*
this function and "CalculateOtherWeights" function are to calculate other data by using Height value on Height, Weight and Note sub-page.
*/
function txtHeight_onchange()
{
    var flag = false; Idx = 0; 
    if (document.forms[0].txtUseImperial.value == '1') {
        document.forms[0].txtHHeight.value = document.forms[0].txtHeight_txtGlobal.value * 0.0254; 
        document.forms[0].txtBMIHeight.value = document.forms[0].txtHeight_txtGlobal.value * 0.0254; 
    }
    else {
        document.forms[0].txtBMIHeight.value = document.forms[0].txtHeight_txtGlobal.value / 100; 
        document.forms[0].txtHHeight.value = document.forms[0].txtHeight_txtGlobal.value / 100; 
    }
    $get("tblPatientTitle_txtBMIHeight").value = document.forms[0].txtBMIHeight.value;
    txtStartWeight_onchange();
    CalculateOtherWeights();
    return;
}

//---------------------------------------------------------------------------------------------------------------
function CalculateOtherWeights(){


    if(parseFloat(document.forms[0].txtHRefBMI.value) == 0)
        document.forms[0].txtHRefBMI.value = 25;
        
    if (document.forms[0].txtUseImperial.value == '1') {
        document.forms[0].txtIdealWeight_txtGlobal.value = ((parseFloat(document.forms[0].txtHRefBMI.value) * Math.pow(parseFloat(document.forms[0].txtBMIHeight.value), 2)) * 2.2).toFixed(1);
        document.forms[0].txtHIdealWeight.value = (document.forms[0].txtIdealWeight_txtGlobal.value * 0.45359237).toFixed(1);
    }
    else {
        document.forms[0].txtIdealWeight_txtGlobal.value = (parseFloat(document.forms[0].txtHRefBMI.value) * Math.pow(parseFloat(document.forms[0].txtBMIHeight.value), 2)).toFixed(1);
        document.forms[0].txtHIdealWeight.value = document.forms[0].txtIdealWeight_txtGlobal.value;
    }
    SetInnerText($get("lblIdealWeight_Value"), document.forms[0].txtIdealWeight_txtGlobal.value);
    if (isNaN(parseFloat(document.forms[0].txtStartWeight_txtGlobal.value))) 
        document.forms[0].txtStartWeight_txtGlobal.value = 0; 
    
    document.forms[0].txtExcessWeight_txtGlobal.value = (parseFloat(document.forms[0].txtStartWeight_txtGlobal.value) - parseFloat(document.forms[0].txtIdealWeight_txtGlobal.value)).toFixed(1); 
    document.forms[0].txtStartWeight_txtGlobal.focus();
    return;
}

//---------------------------------------------------------------------------------------------------------------
function txtStartWeight_onchange()
{
    try{
        var StartWeight = $get("txtStartWeight_txtGlobal").value;
        var Height = $get("txtHeight_txtGlobal").value;
        
        StartWeight = isNaN(StartWeight) ? 0 : eval(StartWeight);
        Height = isNaN(Height) ? 0 : eval(Height);
        
        $get("txtStartWeight_txtGlobal").value = StartWeight;
        $get("txtHeight_txtGlobal").value = Height;
        if (Height != 0){
            if ($get("txtUseImperial").value != '1') {
                $get("txtHStartWeight").value = StartWeight;
                $get("txtStartBMIWeight").value = StartWeight; 
                $get("txtBMI_txtGlobal").value = Math.round(StartWeight / Math.pow(Height / 100, 2)); 
            }
            else {
                $get("txtHStartWeight").value = StartWeight * 0.45359237; 
                $get("txtStartBMIWeight").value = StartWeight * 0.45359237; 
                $get("txtBMI_txtGlobal").value = Math.round(703 * StartWeight / Math.pow(Height, 2)); 
            }
            txtIdealWeight_onchange();
        }
    }
    catch(e){}
    return;
}

//---------------------------------------------------------------------------------------------------------------
function txtIdealWeight_onchange(){
    try{
        var StartWeight = $get("txtStartWeight_txtGlobal").value;
        var IdealWeight = $get("txtIdealWeight_txtGlobal").value;
        
        StartWeight = isNaN(StartWeight) ? 0 : eval(StartWeight);
        IdealWeight = isNaN(IdealWeight) ? 0 : eval(IdealWeight);
        
        $get("txtStartWeight_txtGlobal").value = StartWeight;
        $get("txtIdealWeight_txtGlobal").value = IdealWeight;
        
        if ($get("txtUseImperial").value == '1')
            $get("txtHIdealWeight").value = (IdealWeight * 0.45359237).toFixed(1);
        else 
            $get("txtHIdealWeight").value = IdealWeight;
        
        $get("txtExcessWeight_txtGlobal").value = (StartWeight - IdealWeight).toFixed(1);
    }
    catch(err){}
    return;
}

//---------------------------------------------------------------------------------------------------------------
function txtTargetWeight_onchange(){
    try{    
    
        var TargetWeight = $get("txtTargetWeightBaseline_txtGlobal").value;
            
        if ($get("txtUseImperial").value != '1')
            $get("txtHTargetWeight").value = TargetWeight;        
        else 
            $get("txtHTargetWeight").value = (TargetWeight * 0.45359237).toFixed(1);
    }
    catch(err){}
    return;
}

//-----------------------------------------------------------------------------------------------------------------
/*
The main function of autosave process.
*/
function btnSave_onclick()
{
    CanToSave();
    SavePatientDataProc( );
}

//-----------------------------------------------------------------------------------------------------------------
/*
this function and "IsAnyFieldsFilled" are to check constraints and whether data is ready to save or not, 
Patient first and surname should be entered.

because of autosave process in this page, we check whether any field has value or not, if so, we check that whether
patient name and surname have values or not,
if these 2 fields have values, we save data into tblPatients and we return the "patient id", for the current patient
*/
function CanToSave()
{
    var flag = true;
    var ErrorMessage = "";

    $get("divErrorMessage").style.display = "none";  
      
    switch(parseInt($get("txtHPageNo").value))
    {
        case 1:
            $get("lblFirstName").style.color = "";
            $get("lblSurName").style.color = "";
            
            if (IsAnyFieldsFilled()){
                if ($get("txtFirstName_txtGlobal").value.length == 0)
                {
                    flag = false;
                    $get("lblFirstName").style.color = "RED";
                    ErrorMessage = "First name";
                }
                if ($get("txtSurName_txtGlobal").value.length == 0)
                {
                    flag = false;
                    $get("lblSurName").style.color = "RED";
                    ErrorMessage += ((ErrorMessage.length > 0) ? ", " : "") + "Surname";
                }
                $get("divErrorMessage").style.display = !flag ? "block" : "none";
                SetInnerText($get("pErrorMessage"), !flag ? ("Please enter " + ErrorMessage) : "");
            }
            
            break;
    }
}

//--------------------------------------------------------------------------------------------------------------
function IsAnyFieldsFilled(){
    var flag = false;
    
    flag |= ($get("txtFirstName_txtGlobal").value.length > 0);
    flag |= ($get("txtSurName_txtGlobal").value.length > 0);
    flag |= ($get("txtBirthDate_txtGlobal").value.length > 0);
    flag |= ($get("txtStreet_txtGlobal").value.length > 0);
    flag |= ($get("txtCity_txtGlobal").value.length > 0);
    flag |= ($get("cmbCity").selectedIndex > 0);
    flag |= ($get("txtInsurance_txtGlobal").value.length > 0);
    flag |= ($get("cmbInsurance").selectedIndex > 0);
    flag |= ($get("txtState_txtGlobal").value.length > 0);
    flag |= ($get("txtPostCode_txtGlobal").value.length > 0);
    flag |= ($get("txtPhone_H_txtGlobal").value.length > 0);
    flag |= ($get("txtPhone_W_txtGlobal").value.length > 0);
    flag |= ($get("cmbRace_SystemCodeList").selectedIndex > 0);
    flag |= ($get("cmbSurgon_DoctorsList").selectedIndex > 0);
    flag |= ($get("txtHReferredBy").value.length > 0);
    flag |= ($get("txtHOtherDoctors1").value.length > 0);
    flag |= ($get("txtHOtherDoctors2").value.length > 0);
    
    flag |= ($get("txtMobile_txtGlobal").value.length > 0);
    flag |= ($get("txtEmail_txtGlobal").value.length > 0);
    flag |= ($get("rblTitle").selectedIndex > 0);
    return(flag);
}


function LoadTempDetails(){
    var tempValue = "";
    var tempInputValue = "";
       
    inputList = document.getElementById(section).getElementsByTagName("input");
    for(i=0; i < inputList.length; i++)
    {
        
        if(inputList[i].type=="checkbox")
            tempInputValue += inputList[i].id + ":" + inputList[i].checked + "__";
        else if(inputList[i].type=="radio")
            tempInputValue += inputList[i].id + ":" + inputList[i].checked + "__";
        else 
            tempInputValue += inputList[i].id + ":" + inputList[i].value.trim() + "__";
    }
     
    inputList = document.getElementById(section).getElementsByTagName("select");
    for(i=0; i < inputList.length; i++)
    {
        tempInputValue += inputList[i].id + ":" + inputList[i].value.trim() + "__";
    }
    
    textAreaList = document.getElementById(section).getElementsByTagName("textarea");
    for(i=0; i < textAreaList.length; i++)
    {
        tempInputValue += textAreaList[i].id + ":" + textAreaList[i].value.trim() + "__";
    }
        
    return tempInputValue;
}

//-----------------------------------------------------------------------------------------------------------------
/*
    This function is to create a SOAP document containing all data that should be saved and calls a XML Web service.
    for each sub-page, there is a saving procedure inside the XML Web Service.
*/
function SavePatientDataProc(){
    var requestURL  = "", strFunctionName;
    saveInputValue = LoadTempDetails();
    if(loadInputValue != saveInputValue)
    {
        switch (parseInt($get("txtHPageNo").value))
        {
            case 1 :
                __doPostBack('linkBtnSave','');
                break;
                
            case 2 :
                document.forms[0].txtHBMI.value = $get("txtBMI_txtGlobal").value;
                __doPostBack('LinkBtnSave_Height','');
                return;
                
            case 3 : 
                __doPostBack('','');
                break;
                
            case 4 :
                __doPostBack('','');
                break;
                
            case 5 :
                MakeStringSelectedItems(document.forms[0].listPrevNonBariatricSurgery_Selected);
                __doPostBack('LinkBtnSave_BoldData','');
                break;
                
            case 6 :
                __doPostBack('LinkBtnSave_BoldComorbidity','');
                break;
                
            case 7 :
                __doPostBack('LinkBtnSave_Medication','');
                break;
                
            case 8 :      
                __doPostBack('LinkBtnSave_BoldComorbidityNotes','');
            break
            
            case 9 :      
                MakeStringSelectedItems(document.forms[0].listPrevBariatricSurgery_Selected);
                MakeStringSelectedItems(document.forms[0].listAdverseEvents_Selected);
                __doPostBack('LinkBtnSave_BoldPreviousSurgery','');
            break
           
            default:
                if (AppSchema_ButtonNo != 0) // top-level menu bar (PatientList, Baseline, Visit, ..)
                    document.location.assign($get("txtHApplicationURL").value + AppSchemaMenu_href[AppSchema_ButtonNo-1]);
                else if (DivButtonNo != 0)  SetDivsStatus(DivButtonNo);
                return;
        }
    }
    else{
        if (AppSchema_ButtonNo != 0) // top-level menu bar (PatientList, Baseline, Visit, ..)
            document.location.assign($get("txtHApplicationURL").value + AppSchemaMenu_href[AppSchema_ButtonNo-1]);
        SetDivsStatus($get("txtSelectedPageNo").value);
    }
    
    return;
}

//-----------------------------------------------------------------------------------------------------------------
function UpdatePatientTitle(){
    var strTitle = ($get("rblTitle").selectedIndex > 0) ? ($get("rblTitle").options[$get("rblTitle").selectedIndex].text + " ") : "";
        strTitle += $get("txtFirstName_txtGlobal").value + " " + $get("txtSurName_txtGlobal").value;
    var strAddress = $get("txtStreet_txtGlobal").value + " " + $get("txtCity_txtGlobal").value + " " + $get("txtState_txtGlobal").value + " " + $get("txtPostCode_txtGlobal").value;
    var strDoctorName = ($get("cmbSurgon_DoctorsList").selectedIndex > 0) ? $get("cmbSurgon_DoctorsList").options[$get("cmbSurgon_DoctorsList").selectedIndex].text : "";
    
    //Page1
    SetInnerText($get("tblPatientTitle_lblPatientName_Value"), strTitle);
    SetInnerText($get("tblPatientTitle_lblPatientID_Value"), $get("txtPatient_CustomID_txtGlobal").value);
    SetInnerText($get("tblPatientTitle_lblAddress_Value"), strAddress);
    SetInnerText($get("tblPatientTitle_lblHomePhone_Value"), $get("txtPhone_H_txtGlobal").value);
    SetInnerText($get("tblPatientTitle_lblMobilePhone_Value"), $get("txtMobile_txtGlobal").value);
    SetInnerText($get("tblPatientTitle_lblDoctor_Value"), strDoctorName);
    //ShowPatientAge();
    
    //Page2
    SetInnerText($get("tblPatientTitle_lblStartBMI_Value"), $get("txtBMI_txtGlobal").value);
    SetInnerText($get("tblPatientTitle_lblStartWeight_Value"), $get("txtStartWeight_txtGlobal").value);
    SetInnerText($get("tblPatientTitle_lblIdealWeight_Value"), $get("txtIdealWeight_txtGlobal").value);    
    document.getElementById("tblPatientTitle_txtTargetWeight_txtGlobal").value = $get("txtTargetWeightBaseline_txtGlobal").value;

    //$get("tblPatientTitle_txtTargetWeight").value = $get("txtTargetWeight_txtGlobal").value;
    return;
}

//-----------------------------------------------------------------------------------------------------------------
/*
in Patients List, when user clicks "Add New Patient" button, the "PatientDataForm" is loaded, because the patient is new one, 
other schemas in Application Schema menu are hidden (Visit, Operation, File and complication),
and when the new patient data is saved, automatically all schemas in application schema menu are displayed,
this function is to display all schemas after patient data saving
*/
function LoadApplicationSchemas(){
    for (Xh = 0; Xh < AppSchemaMenu.length; Xh++)
        $get(AppSchemaMenu[Xh]).style.display = "block";
	return;
}

//-----------------------------------------------------------------------------------------------------------------
/*
this function is to send a SOAP document to "PatientDataWebService.asmx" to calculate AGE (in front of BirthDate and in Patient title section - grey section)
*/
function ShowPatientAge( )
{
    var strSOAP = 
            '<?xml version="1.0" encoding="utf-8"?>'+
            '<soap:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" ' + 
                            'xmlns:xsd="http://www.w3.org/2001/XMLSchema" ' + 
                            'xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">'+
	            '<soap:Body>'+
		            '<CalculateAge xmlns="http://tempuri.org/">'+
		                '<strBirthDate>' + $get("txtBirthDate_txtGlobal").value + '</strBirthDate>' +
		            '</CalculateAge>'+
	            '</soap:Body>'+
            '</soap:Envelope>';
	
    SubmitSOAPXmlHttp(strSOAP, ShowPatientAge_CallBack, document.forms[0].txtHApplicationURL.value + "WebServices/GlobalWebService.asmx", "http://tempuri.org/CalculateAge");
    return;
}

//-----------------------------------------------------------------------------------------------------------------
function ShowPatientAge_CallBack(){
    if (XmlHttp.readyState == 4)
        if (XmlHttp.status == 200){
            var response  = XmlHttp.responseXML.documentElement, intAge = 0;
            
            if (response.getElementsByTagName("ReturnValue")[0].hasChildNodes())
                intAge = parseInt(response.getElementsByTagName('ReturnValue')[0].firstChild.data);
            if (intAge > 0)
            {
                SetInnerText($get("tblPatientTitle_lblAge_Value"), intAge + " yrs");
                SetInnerText($get("lblAge"), intAge);
            }
            else{
                SetInnerText($get("tblPatientTitle_lblAge_Value"), "");
                SetInnerText($get("lblAge"), "");
            }
        }
}


//--------------------------------------------------------------------------------------------------------------
/*
this function is to change "GENDER" dropdownlist when user change the "TITLE" dropdownlist,
*/
function rblTitle_onchange(rblTitle){
    if (parseInt(rblTitle.value) == 1 || parseInt(rblTitle.value) == 6)
        $get("rblGender").value = "M";
    else
        switch(parseInt(rblTitle.value)){
            case 2 :
            case 3 :
            case 5 :
                $get("rblGender").value = "F";
                break;
        }    
    return;
}

//--------------------------------------------------------------------------------------------------------------
/*
this function is to load a floading div, when user starts to type data in
1) City 2)Insurance 3) 3 Referring Doctors
*/
function ajax_showOptions(e, txtDropdownList)
{
    mainDropDownList = $get(txtDropdownList);
    if (document.all) e = event;
	if ( !ajax_optionDiv ){
		ajax_optionDiv = document.createElement('DIV');
		ajax_optionDiv.id = 'ajax_listOfOptions';
		
		document.body.appendChild(ajax_optionDiv);
		
		if(document.all){
			ajax_optionDiv_iframe = document.createElement('IFRAME');
			ajax_optionDiv_iframe.border='0';
			ajax_optionDiv_iframe.style.width = ajax_optionDiv.clientWidth + 'px';
			ajax_optionDiv_iframe.style.height = ajax_optionDiv.clientHeight + 'px';
			ajax_optionDiv_iframe.id = 'ajax_listOfOptions_iframe';
			document.body.appendChild(ajax_optionDiv_iframe);
		}
		
	    var oldonkeydown = document.body.onkeydown;
	    if(typeof oldonkeydown != 'function'){
	        document.body.onkeydown = ajax_option_keyNavigation;
	    }else{
		    document.body.onkeydown = function(){
			    oldonkeydown();
		        ajax_option_keyNavigation(e) ;
		    }
	    }
	}
	
	var oldonblur = txtObject.onblur;
    if (typeof oldonblur == "function")
        txtObject.onblur = function(){
            txtObject.style.background = ""; // this is body of onblur function in TextWUControl
            CheckEnteredValue(false);
        }
        
	ajax_optionDiv.style.top = (ajax_getTopPos(txtObject) + txtObject.offsetHeight) + 'px';
	ajax_optionDiv.style.left = ajax_getLeftPos(txtObject) + 'px';
	
	if(ajax_optionDiv_iframe){
		ajax_optionDiv_iframe.style.left = ajax_optionDiv.style.left;
		ajax_optionDiv_iframe.style.top = ajax_optionDiv.style.top;
	}
	ajax_optionDiv.onselectstart =  ajax_list_cancelEvent;
    FillDivContent(txtObject.value, $get(txtDropdownList), e);
	return;
}

//----------------------------------------------------------------------------------------------------------------
function ajax_option_keyNavigation(e)
{
	if(document.all)e = event;
	
	if(!ajax_optionDiv)return;
	if(ajax_optionDiv.style.display=='none')return;
	
	if (e.keyCode == 38) {	// Up arrow
	    SetRefDivScroll(true);
	}

	if (e.keyCode == 40) {	// Down arrow
	    SetRefDivScroll(false);
	    e.preventDefault();
	    e.stopPropagation();
	}

	if ((e.keyCode == 27) || (e.keyCode == 13) || (e.keyCode == 9)){	// Escape key or Enter key or tab key
	    CheckEnteredValue(e.keyCode == 13);
		ajax_options_hide();
		if(e.keyCode==13)return false; else return true;
	}
}

//----------------------------------------------------------------------------------------------------------------
function SetRefDivScroll(MoveUp){
    tempTable = $get("tblList");
    var scrollbuttonActive = false;
    
    if (tempTable.rows.length > 0){
        if ((activeItem > 0) && (activeItem <= tempTable.rows.length)){
            tempTable.rows[activeItem-1].style.backgroundColor = (activeItem % 2 == 1) ? "#eee" : "";
        }
        
        if (MoveUp){
            if (--activeItem <= 0) {
                activeItem = 1;
                activeItemInDiv = 1;
                tempTable.rows[activeItem-1].style.backgroundColor = "#ccc";
            }
            if (activeItemInDiv > 1) --activeItemInDiv;
            
            if ((activeItemInDiv == 1) && (activeItem > 0))
                scrollbuttonActive = true;
        }
        else {
            if (++activeItem >= tempTable.rows.length) {
                activeItem = tempTable.rows.length;
                tempTable.rows[activeItem-1].style.backgroundColor = "#ccc";
                return;
            }
            
            if (activeItemInDiv < RecNoInDiv) activeItemInDiv++; else activeItemInDiv = RecNoInDiv ;
            
            if (activeItemInDiv % RecNoInDiv == 0) {
		        scrollbuttonDirection = scrollbuttonSpeed; 
		        scrollbuttonActive = true;
            }
        }
        tempTable.rows[activeItem-1].style.backgroundColor = "#ccc";
        
        if (scrollbuttonActive)
		    ScrollByArrowKeys(!MoveUp);
    }
    else activeItem = 0;
}

//----------------------------------------------------------------------------------------------------------------
function ScrollByArrowKeys(MoveDownFlag){
    var Factor = MoveDownFlag ? 1 : -1;
    var divOption = $get('ajax_listOfOptions');
    var scrollTop = 0;

    if (MoveDownFlag)
        if (activeItem == tempTable.rows.length - 1)
            scrollTop = ajax_optionDiv.scrollHeight;
        else 
            scrollTop = Factor * parseInt(ajax_optionDiv.scrollHeight / tempTable.rows.length); 
	else
	    if (activeItem == tempTable.rows.length - 1)
            scrollTop = 0;
        else 
            scrollTop = Factor * parseInt(ajax_optionDiv.scrollHeight / tempTable.rows.length); 
	    
	ajax_optionDiv.scrollTop += scrollTop;
}

//----------------------------------------------------------------------------------------------------------------
function CheckEnteredValue(PressEnterFlag){
    if (txtObject.value.length == 0)
        txtHiddenObject.value = "";
    else{
        //var cmbReferredDoctorsList = mainDropDownList, IsOk = false;
        var IsOk = false;
        
        if (PressEnterFlag && (activeItem > 0))  // User press enter on selected row of referring doctors
        {
            tempTable = $get("tblList");
            txtObject.value = document.all ? tempTable.rows[activeItem-1].cells[1].innerText : tempTable.rows[activeItem-1].cells[1].textContent;
        }
        
        for (Xh = 1; !IsOk && (Xh < mainDropDownList.options.length); Xh++)
            if (mainDropDownList.options[Xh].text == txtObject.value){
                IsOk = true;
                txtHiddenObject.value = mainDropDownList.options[Xh].value;
                if (txtObject.id.toLowerCase() == "txtcity_txtglobal") LoadStatePostCode(txtHiddenObject.value);
            }
        
        if (!IsOk)
            if (LimitToList)
                if (txtHiddenObject.value.length > 0)
                    SetListFieldValue(txtObject.id, txtHiddenObject.value, mainDropDownList.id);
                else txtObject.value = ""; 
    }
}

//----------------------------------------------------------------------------------------------------------------
function LoadStatePostCode(strStatePostCode){
    $get("txtPostCode_txtGlobal").value = strStatePostCode.substring(0, strStatePostCode.indexOf(";"));
    $get("txtState_txtGlobal").value = strStatePostCode.substring(strStatePostCode.indexOf(";")+1);
}

//----------------------------------------------------------------------------------------------------------------
function ajax_options_hide()
{
	if (ajax_optionDiv){
	    ajax_optionDiv.style.display = "none";	
	}
	if (ajax_optionDiv_iframe){
	    ajax_optionDiv_iframe.style.display = "none";
	}
}

//----------------------------------------------------------------------------------------------------------------
function ajax_getTopPos(inputObj)
{
    var returnValue = inputObj.offsetTop;
    while((inputObj = inputObj.offsetParent) != null){
        returnValue += inputObj.offsetTop;
    }
    return returnValue;
}

//----------------------------------------------------------------------------------------------------------------
function ajax_getLeftPos(inputObj)
{
    var returnValue = inputObj.offsetLeft;
    while ((inputObj = inputObj.offsetParent) != null){
        returnValue += inputObj.offsetLeft;
    }
    return returnValue;
}

//----------------------------------------------------------------------------------------------------------------
function ajax_list_cancelEvent()
{
    return false;
}

//----------------------------------------------------------------------------------------------------------------
function FillDivContent(txtValue, cmbList, e)
{
    var HasItem = false;
    
    switch(e.keyCode){
        case 27 :   // Esc
        case 9 :    // Tab
        case 13 :   // Enter
        case 38 :   // Up arrow
        case 40 :   // Down arrow
            return;
    }
    ResetDivContent();
    if (e.keyCode == 27 || e.keyCode == 9 || e.keyCode == 13) {
        return;
    }
    if ((tempTable = $get("tblList")) != null)
    {
        while (tempTable.rows.length > 0)
            tempTable.deleteRow(0);
    }
    else{
        tempTable = document.createElement("TABLE");
        tempTable.id = "tblList";
        tempTable.style.width = "100%";
    }
    
    if ((txtValue != "") && (cmbList.options.length > 0)){
        for (Xh = 1; Xh < cmbList.options.length; Xh++)
        {
            if (cmbList.options[Xh].text.toUpperCase().indexOf(txtValue.toUpperCase()) == 0){
                var tempRow = tempTable.insertRow(tempTable.rows.length);
                var tempCell1 = tempRow.insertCell(tempRow.cells.length);
                var tempCell2 = tempRow.insertCell(tempRow.cells.length);
                var tempCell3 = tempRow.insertCell(tempRow.cells.length);
                
                tempRow.style.backgroundColor = (Xh % 2 == 1) ? "#eee" : "";
                SetStyle(tempRow);
                
                if (ColumnWidth.substring(0, ColumnWidth.indexOf(";") ) == "0"){
                    tempCell1.style.visiblity = "hidden";
                    tempCell1.style.display = "none";
                }
                else
                    tempCell1.style.width = ColumnWidth.substring(0, ColumnWidth.indexOf(";") );
                
                if (ColumnWidth.substring(ColumnWidth.indexOf(";") + 1) == "0"){
                    tempCell2.style.visiblity = "hidden";
                    tempCell2.style.display = "none";
                }
                else
                    tempCell2.style.width = ColumnWidth.substring(ColumnWidth.indexOf(";") + 1);
                    
                if (ColumnWidth.substring(ColumnWidth.lastIndexOf(";") + 1) == "0"){
                    tempCell3.style.visiblity = "hidden";
                    tempCell3.style.display = "none";
                }
                else
                    tempCell3.style.width = ColumnWidth.substring(ColumnWidth.lastIndexOf(";") + 1);
                    
                tempCell1.appendChild(document.createTextNode(cmbList.options[Xh].value));
                tempCell2.appendChild(document.createTextNode(cmbList.options[Xh].text));
                tempCell3.appendChild(document.createTextNode(cmbList.options[Xh].title));
                HasItem = true;
            }
        }
        ajax_optionDiv.appendChild(tempTable);
    }
    if (ajax_optionDiv)
        ajax_optionDiv.style.display = HasItem ? "block" : "none";
    if (ajax_optionDiv_iframe)
        ajax_optionDiv_iframe.style.display = HasItem ? "block" : "none";
    return;
}

//----------------------------------------------------------------------------------------------------------------
function ResetDivContent(){
    activeItem = 0;
    activeItemInDiv = 0;
    ajax_optionDiv.scrollTop = 0;
    if ($get("tblList")) ajax_optionDiv.removeChild($get("tblList"));
}

//----------------------------------------------------------------------------------------------------------------
function SetStyle(tempRow){
    var tempColor;
    
    tempRow.onmouseover = function()
    {
        tempColor = this.style.backgroundColor;
        this.style.backgroundColor = "#ccc";
        this.style.cursor = "pointer";
    }
    
    tempRow.onmouseout = function()
    {
        this.style.backgroundColor = tempColor;
        this.style.cursor = "";
    }
    
    tempRow.onclick  = function()
    {
        txtObject.focus();
        txtObject.value = document.all ? tempRow.cells[1].innerText : tempRow.cells[1].textContent;
        txtHiddenObject.value = document.all ? tempRow.cells[0].innerText : tempRow.cells[0].textContent;
        ajax_options_hide();
        switch(txtObject.id)
        {
            case "txtCity_txtGlobal" :
                LoadStatePostCode(txtHiddenObject.value);
                break;
        }
    }
}

//-----------------------------------------------------------------------------------------------------------------
function txtBirthDate_onchange()
{
    var dtBirth = new Date(document.forms[0].txtBirthDate_txtGlobal.value);    
    var now = new Date();
    
    try{
        if(dtBirth>now){
            document.getElementById('divErrorMessage').style.display = 'block';
            SetInnerText(document.getElementById('pErrorMessage'), 'The Birthdate is greater than current date ...');        
            SetInnerText(document.getElementById('lblAge'), '0');
        }
        else{       
            document.getElementById('divErrorMessage').style.display = 'none';
            SetInnerText(document.getElementById('pErrorMessage'), '');
            
            tday=now.getDate();
            tmo=(now.getMonth());
            tyr=(now.getFullYear());
            
            bday=dtBirth.getDate();
            bmo=(dtBirth.getMonth());
            byr=(dtBirth.getFullYear());
            
            var age = tyr - byr;
            if (tmo < bmo)
                --age;   
            else if (tmo == bmo)
                if (bday < bday)
                    --age;    

            SetInnerText(document.getElementById('lblAge'), age);      
        }
    }
    catch(err){
        SetInnerText(document.getElementById('lblAge'), '0');
        document.getElementById('divErrorMessage').style.display = 'block';
        SetInnerText(document.getElementById('pErrorMessage'), 'The Birthdate is not correct ...');
    }
            
    //__doPostBack('linkBtnCalculateAge','');
}

//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
function aCalendar_onclick(obj,inputBoxID){
    var strDateformat = $get("lblDateFormat");
    languageCode = document.forms[0].txtHCulture.value.substr(0, 2);
    var strCmd = "displayCalendar(document.forms[0]."+inputBoxID+"_txtGlobal, document.all ? strDateformat.innerText : strDateformat.textContent, obj)";
    eval(strCmd);
} 

//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
/*this function is to add/remove the selected item in BOLD lists (Prev Bariatric, non-Bariatric lists and Adverse Event list)*/
function BoldLists_dblclick( listSourceID, listDestID, strAction){
    var listSource = $get(listSourceID), listDest = $get(listDestID);
    
    if ((listSource.options.length == 0) || (listSource.selectedIndex == -1)) return;
    
    if (strAction.toLowerCase() == "add"){ // it means we should add the selected item into dest list, when user double-clicks the source list
        if (! IsItemExistsInDestList(listDest, listSource.options[listSource.selectedIndex].value))
            AddItemIntoDestList(listDest, listSource.options[listSource.selectedIndex]);
    }
    else if (strAction.toLowerCase() == "remove")   listSource.options[listSource.selectedIndex] = null;
}

//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
function BoldLButtonLinks_click(listSourceID, listDestID, strAction, LoadAll){
    var listSource = $get(listSourceID), listDest = $get(listDestID);
    var idx = 0;
    
    if (LoadAll){
        if (strAction.toLowerCase() == "add"){
            listDest.options.length = 0;
            for (idx = 0; idx < listSource.options.length; idx++)
                AddItemIntoDestList(listDest, listSource.options[idx]);
        }
        else if (strAction.toLowerCase() == "remove") listSource.options.length = 0;
    }
    else {
        if (strAction.toLowerCase() == "add"){
            for (idx = 0; idx < listSource.options.length; idx++)
                if (listSource.options[idx].selected && (! IsItemExistsInDestList(listDest, listSource.options[idx].value)))
                        AddItemIntoDestList(listDest, listSource.options[idx]);
        }
        else if (strAction.toLowerCase() == "remove") {
                idx = 0;
                while (idx < listSource.options.length)
                    if (listSource.options[idx].selected ) {listSource.options[idx] = null; idx = 0;}
                    else idx++;
        }
    }
}


//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
/*this function is to check whether the value of Item exists in dest list or not*/
function IsItemExistsInDestList(listDest, ItemValue){
    var flag = false;
    var xh = 0;
    
    for (xh = 0; !flag && (xh < listDest.options.length); xh++)
        flag = (listDest.options[xh].value == ItemValue);
        
    return flag;
}

//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
function AddItemIntoDestList(listDest, sourceOption ){
    var option = document.createElement("option");

    option.value = sourceOption.value;
    option.text = sourceOption.text;
    listDest.options.add(option);
    
}

//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
/*this function is to make string of selected items in Select Destunation lists, we send these string to server and save them in database*/
function MakeStringSelectedItems(listDest){
    var txtObject, xh = 0;
    
    switch(listDest.id){
        case "listPrevBariatricSurgery_Selected" :
            txtObject = document.forms[0].txtPrevBariatricSurgery_Selected;
            break;
        case "listPrevNonBariatricSurgery_Selected" :
            txtObject = document.forms[0].txtPrevNonBariatricSurgery_Selected;
            break;
        case "listAdverseEvents_Selected" :
            txtObject = document.forms[0].txtAdverseEvents_Selected;
            break;
    }
    txtObject.value = "";
    for (; xh < listDest.options.length; xh++){
        if (txtObject.value.length == 0)
            txtObject.value = ";" + listDest.options[xh].value + ";";
        else
            if (txtObject.value.indexOf(";" + listDest.options[xh].value + ";") == -1)
                txtObject.value += listDest.options[xh].value + ";";
    }
    if (txtObject.value.length > 0)
        txtObject.value = txtObject.value.substr(1);
}

//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
function chkPreCert_Other_onclick(chkOtherObj, txtOtherID){
    var txtOther = $get(txtOtherID);
    txtOther.disabled = !chkOtherObj.checked;
    txtOther.value = !chkOtherObj.checked ? "" : txtOther.value;
}


//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
/*this function is to filter BOLD lists (Prev Bariatric, non-Bariatric lists and Adverse Event list), when user starts typing in SEARCH text fields*/
function FilterListBySearchWord(txtSearchID, listSourceID, listDestID){
    var idx = 0;
    var txtSearch = $get(txtSearchID),
        listSource = $get(listSourceID),
        listDest = $get(listDestID);
        
    listDest.options.length = 0;
    if (txtSearch.value.length == 0){
        for (idx = 0; idx < listSource.options.length; idx++)
            AddItemIntoDestList(listDest, listSource.options[idx]);
    }
    else{
        for (idx = 0; idx < listSource.options.length; idx++)
            if (listSource.options[idx].text.toLowerCase().indexOf(txtSearch.value.toLowerCase()) != -1)
                AddItemIntoDestList(listDest, listSource.options[idx]);
    }
}

//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
/*This function is to reset default value for all comorbidity dropdownlist fields to 'Select...'*/
function btnComorbiditySetDefault_onclick(){
    var xh = 0;
    var ComorbidityDropDownList = new Array(
        "cmbHypertension", "cmbCongestive", "cmbIschemic", "cmbAngina", 
        "cmbPeripheral", "cmbLower", "cmbDVT", "cmbGlucose", "cmbLipids", 
        "cmbGout", "cmbGred", "cmbCholelithiasis", "cmbLiver", "cmbBackPain", 
        "cmbMusculoskeletal", "cmbFibro", "cmbPsychosocial", "cmbDepression", 
        "cmbConfirmed", "cmbAlcohol", "cmbTobacco", "cmbAbuse", "cmbStressUrinary", 
        "cmbCerebri", "cmbHernia", "cmbFunctional", "cmbSkin", "cmbObstructive", 
        "cmbObesity", "cmbPulmonary", "cmbAsthma", "cmbPolycystic", "cmbMenstrual");
    
    
    for (; xh < ComorbidityDropDownList.length; xh++)
        if ($get(ComorbidityDropDownList[xh] + "_SystemCodeList").selectedIndex == 0)
            $get(ComorbidityDropDownList[xh] + "_SystemCodeList").selectedIndex = 1;
    return;
}


//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
/* This function is to update comorbidity notes*/
function UpdateComorbidityNotes(){
    var ComorbidityList = new Array(
        "Hypertension", "Congestive", "Ischemic", "Angina", 
        "Peripheral", "Lower", "DVT", "Glucose", "Lipids", 
        "Gout", "Gred", "Cholelithiasis", "Liver", "BackPain", 
        "Musculoskeletal", "Fibro", "Psychosocial", "Depression", 
        "Confirmed", "Alcohol", "Tobacco", "Abuse", "StressUrinary", 
        "Cerebri", "Hernia", "Functional", "Skin", "Obstructive", 
        "Obesity", "Pulmonary", "Asthma", "Polycystic", "Menstrual");

    var ComorbidityHeader = new Array(
        "hCardiovascularDiseaseNotes", "hMetabolicNotes", "hPulmonaryNotes", "hMusculoskeletalNotes", 
        "hGastroIntestinalNotes", "hReproductiveNotes", "hMusculoskeletalNotes", "hGeneralNotes", "hPsychosocialNotes", "hReproductiveNotes");
            
    for (var count = 0; count < ComorbidityList.length; count++)
    {
        if($get("cmb"+ComorbidityList[count]+"_SystemCodeList").selectedIndex>1)
            $get("ln"+ComorbidityList[count]+"Notes").style.display = "block";
        else
            $get("ln"+ComorbidityList[count]+"Notes").style.display = "none"; 
    }
 
    for (var countHeader = 0; countHeader < ComorbidityHeader.length; countHeader++)
    {
        $get(ComorbidityHeader[countHeader]).style.display = "none"; 
    }
    
            
    if ($get("lnHypertensionNotes").style.display == "block" || $get("lnCongestiveNotes").style.display == "block" || $get("lnIschemicNotes").style.display == "block"
    || $get("lnAnginaNotes").style.display == "block" || $get("lnPeripheralNotes").style.display == "block" || $get("lnLowerNotes").style.display == "block" || $get("lnDVTNotes").style.display == "block")
        $get("hCardiovascularDiseaseNotes").style.display = "block"; 
        
    if ($get("lnGlucoseNotes").style.display == "block" || $get("lnLipidsNotes").style.display == "block" || $get("lnGoutNotes").style.display == "block")
        $get("hMetabolicNotes").style.display = "block"; 
        
    if ($get("lnObstructiveNotes").style.display == "block" || $get("lnObesityNotes").style.display == "block" || $get("lnPulmonaryNotes").style.display == "block" || $get("lnAsthmaNotes").style.display == "block")
        $get("hPulmonaryNotes").style.display = "block"; 

    if ($get("lnGredNotes").style.display == "block" || $get("lnCholelithiasisNotes").style.display == "block" || $get("lnLiverNotes").style.display == "block")
        $get("hGastroIntestinalNotes").style.display = "block"; 
      
    if ($get("lnBackPainNotes").style.display == "block" || $get("lnMusculoskeletalNotes").style.display == "block" || $get("lnFibroNotes").style.display == "block")
        $get("hMusculoskeletalNotes").style.display = "block"; 

    if ($get("lnPolycysticNotes").style.display == "block" || $get("lnMenstrualNotes").style.display == "block")
        $get("hReproductiveNotes").style.display = "block"; 
       
    if ($get("lnPsychosocialNotes").style.display == "block" || $get("lnDepressionNotes").style.display == "block" || $get("lnConfirmedNotes").style.display == "block" || $get("lnAlcoholNotes").style.display == "block" || $get("lnTobaccoNotes").style.display == "block" || $get("lnAbuseNotes").style.display == "block")
        $get("hPsychosocialNotes").style.display = "block"; 
    
    if ($get("lnStressUrinaryNotes").style.display == "block" || $get("lnCerebriNotes").style.display == "block" || $get("lnHerniaNotes").style.display == "block" || $get("lnFunctionalNotes").style.display == "block" || $get("lnSkinNotes").style.display == "block")
        $get("hGeneralNotes").style.display = "block"; 
                    
    return;
}

function addRefDrCombo(){
    var oOption = document.createElement("OPTION");           
    
    var strRefSurname = $get("txtPatientRefSurname").value;
    var strRefFirstName = $get("txtPatientRefFirstname").value;
    var strRefTitle = $get("txtPatientRefTitle").value;            
    var strRefSuburb = $get("txtPatientRefSuburb").value;    
    
    oOption.value = ((strRefSurname.length > 4) ? strRefSurname.substring(0, 4) : strRefSurname) + strRefFirstName.substring(0, 1);
    oOption.text = strRefSurname + "  " + strRefFirstName + "  " + strRefTitle ;
    oOption.title = strRefSuburb;    
            
    document.getElementById("cmbReferredDoctorsList").options.add(oOption);
}


function btnExcessWeightBaseline_onclick(){
    var EWV = new Number(), StartWeight = new Number(), IdealWeight = new Number(), TargetWeight = 0;

    if (document.all){
        EWV = parseFloat(document.getElementById("lblExcessWeight_Value").innerText);
        StartWeight = parseFloat($get("txtStartWeight_txtGlobal").value);
        IdealWeight = parseFloat($get("txtIdealWeight_txtGlobal").value);
    }
    else{
        EWV = parseFloat(document.getElementById("lblExcessWeight_Value").textContent);
        StartWeight = parseFloat($get("txtStartWeight_txtGlobal").value);
        IdealWeight = parseFloat($get("txtIdealWeight_txtGlobal").value);
    }
    TargetWeight = ComputeTargetWeight_EWV(EWV, StartWeight, IdealWeight);
    
    if (!isNaN(TargetWeight)){
        document.getElementById("txtTargetWeightBaseline_txtGlobal").value = TargetWeight;
        //UpdateGroupTargetWeight();
    }
    else
        document.getElementById("txtTargetWeightBaseline_txtGlobal").value = 0;
    
    document.getElementById("tblPatientTitle_txtTargetWeight_txtGlobal").value = $get("txtTargetWeightBaseline_txtGlobal").value;
                        
    txtTargetWeight_onchange();
    
    return;
}
    
//------------------------------------------------------------------------------------------------------------
function btnBMIBaseline_onclick(){
    var BMI = new Number(), BMIHeight = new Number(), UseImperial  = new Number(), TargetWeight = 0;
    
    if (document.all)
        BMI = parseFloat(document.getElementById("lblBMI_Value").innerText);
    else
        BMI = parseFloat(document.getElementById("lblBMI_Value").textContent);
        
    BMIHeight = parseFloat($get("txtBMIHeight").value);
    UseImperial = parseInt($get("txtUseImperial").value);
    
    TargetWeight = ComputeTargetWeight_BMI(BMI, BMIHeight, UseImperial);
        
    if (!isNaN(TargetWeight)){
        document.getElementById("txtTargetWeightBaseline_txtGlobal").value = TargetWeight;
        //UpdateGroupTargetWeight();
    }
    else
        document.getElementById("txtTargetWeightBaseline_txtGlobal").value = 0;
    
    document.getElementById("tblPatientTitle_txtTargetWeight_txtGlobal").value = $get("txtTargetWeightBaseline_txtGlobal").value;
        
    txtTargetWeight_onchange();
    
    return;
}

/*this is a function to delete an visit*/
function btnDelete_onclick(){
    $get("txtHDelete").value=0; 
    var answer = confirm ("It will permanently delete this patient. Do you want to proceed?")
    if (answer)
        $get("txtHDelete").value = 1;
    return;
}