var WhereFlag, LoadingTimer;
var VisitRowColor;
var floodDiv = false;
var floodDiv_iframe = false;
var IsVisitSelected = false;
var oInterval = "";
var SelectedVisit = 1;

//---------------------------------------------------------------------------------------------------------------
function InitializePage(){
    document.getElementById("chkFollowUpDetails").checked = false;
    document.getElementById("chkLetterToDoctor").checked = false;
    document.getElementById("chkGraphs").checked = false;
    /*document.getElementById("tblPatientTitle_cmbGroup_CodeList").onchange = function(){
        tblPatientTitle_cmbGroup_onchange();
    }*/
    
    document.getElementById("AppSchemaMenu_a_Visits").href = "#";
    ProgressNotes_ClearFields();
    
    FetchFieldsCaption(false,  document.getElementById("txtHCulture").value, document.frmConsultFU1.name);
}

//---------------------------------------------------------------------------------------------------------------
function controlBar_Buttons_OnClick(buttonNo)
{
    var idx = 1;
    
    for(; idx <= 4; idx++) 
        try{document.getElementById('li_Div' + idx).className = (idx != buttonNo) ? "" : "current";}
        catch(e){}
        
    try{document.getElementById("div_vProgress_Notes").style.display = (buttonNo == 1) ? "block" : "none";}catch(e){}
    try{document.getElementById("div_vComplications").style.display = (buttonNo == 3) ? "block" : "none";}catch(e){}
    try{document.getElementById("div_vPatientReport").style.display = (buttonNo == 4) ? "block" : "none";}catch(e){}
    document.getElementById("txtHPageNo").value = buttonNo;

    
    switch(buttonNo){
        case 3 :
            LoadPatientComplicationHistoryData("");
            break;
        case 4 :
            if (document.getElementById("txtDoctorsList").value.length == 0)
                LoadDoctorsList();
            else
                ReLoadDoctorsList();
            break;
    }
    return;
}

//-----------------------------------------------------------------------------------------------------------------
function ShowPatientNotesDiv( ){ 
    var flag = (document.getElementById('divPatientNotes').style.display == 'none');
    
    document.getElementById('imgPatientNotes').src = flag ? '../../../img/button_minus.gif' : '../../../img/button_plus.gif';
    document.getElementById('divPatientNotes').style.display= flag ? 'block' : 'none';
    document.getElementById('btnStartDate').style.display = flag ? 'block' : 'none';
    document.getElementById('btnEndDate').style.display = flag ? 'block' : 'none';
}

//-----------------------------------------------------------------------------------------------------------------
function ProgressNotes_ClearFields(){
    ShowVisitFormDiv(0); 
    
    VisitDivsInitialSetting();
    document.getElementById("txtDateSeen_PN_txtGlobal").value = document.getElementById("txtHCurrentDate").value; 
    document.getElementById("txtAsthmaResolvedDate_txtGlobal").value = "";
    document.getElementById("txtRefluxResolvedDate_txtGlobal").value = "";
    document.getElementById("txtSleepResolvedDate_txtGlobal").value = "";
    document.getElementById("txtFertilityResolvedDate_txtGlobal").value = "";
    document.getElementById("txtIncontinenceResolvedDate_txtGlobal").value = "";
    document.getElementById("txtArthritisResolvedDate_txtGlobal").value = "";
    document.getElementById("txtCVDLevelResolvedDate_txtGlobal").value = "";
    document.getElementById("txtBackResolvedDate_txtGlobal").value = "";
    document.getElementById("txtHypertensionResolvedDate_txtGlobal").value = "";
    document.getElementById("txtDiabetesResolvedDate_txtGlobal").value = "";
    document.getElementById("txtLipidsResolvedDate_txtGlobal").value = "";
    
    document.getElementById("txtSystolicBP_txtGlobal").value = "";
    document.getElementById("txtDiastolicBP_txtGlobal").value = "";
    document.getElementById("txtBPRxDetails_txtGlobal").value = "";
    document.getElementById("chkHypertensionResolved").checked = false;
    document.getElementById("txtHypertensionResolvedDate_txtGlobal").value = "";
   
    document.getElementById("txtFBloodGlucose_txtGlobal").value = "";
    document.getElementById("txtHBA1C_txtGlobal").value = "";
    document.getElementById("txtDiabetesRxDetails_txtGlobal").value = "";
    document.getElementById("chkDiabetesResolved").checked = false;
    document.getElementById("txtDiabetesResolvedDate_txtGlobal").value = "";
    
    document.getElementById("txtTotalCholesterol_txtGlobal").value = "";
    document.getElementById("txtTriglycerides_txtGlobal").value = "";
    document.getElementById("txtHDLCholesterol_txtGlobal").value = "";
    document.getElementById("txtLipidRxDetails_txtGlobal").value = "";
    document.getElementById("chkLipidsResolved").value = false;
    document.getElementById("txtLipidsResolvedDate_txtGlobal").value = "";
    
    document.getElementById("txtAsthmaResolvedDate_txtGlobal").value = "";
    document.getElementById("cmbAsthmaCurrentLevel_SystemCodeList").value = "";
    document.getElementById("txtAsthmaResolvedDate_txtGlobal").value = "";
    document.getElementById("txtBaseAsthmaDetails_txtGlobal").value = "";
    document.getElementById("txtRefluxResolvedDate_txtGlobal").value = "";
    document.getElementById("cmbRefluxCurrentLevel_SystemCodeList").value = "";
    document.getElementById("txtBaseRefluxDetails_txtGlobal").value = "";
    document.getElementById("txtSleepResolvedDate_txtGlobal").value = "";
    document.getElementById("cmbSleepCurrentLevel_SystemCodeList").value = "";
    document.getElementById("txtBaseSleepDetails_txtGlobal").value = "";
    
    // Minor Comorbidity
    document.getElementById("cmbFertilityCurrentLevel_SystemCodeList").value = "";
    document.getElementById("txtBaseFertilityDetails_txtGlobal").value = "";
    document.getElementById("cmbIncontinenceCurrentLevel_SystemCodeList").value = "";
    document.getElementById("txtBaseIncontinenceDetails_txtGlobal").value = "";
    document.getElementById("cmbBackCurrentLevel_SystemCodeList").value = "";
    document.getElementById("txtBaseBackDetails_txtGlobal").value = "";
    document.getElementById("cmbArthritisCurrentLevel_SystemCodeList").value = "";
    document.getElementById("txtBaseArthritisDetails_txtGlobal").value = "";
    document.getElementById("txtBaseCVDDetails_txtGlobal").value = "";
    document.getElementById("txtBaseOtherDetails_txtGlobal").value = "";
    
    // BOLD Comorbidity

    // Visit Data YELLOW Section
    document.getElementById("txtWeight_PN_txtGlobal").value = ""; 
    document.getElementById("txtRV_PN_txtGlobal").value = ""; 
    document.getElementById("txtNextVisitDate_PN").value = ""; 
    document.getElementById("txtNotes_PN_txtGlobal").value = "";
    document.getElementById("txtHConsultID").value = "0";
    document.getElementById("txtHSaveFlag").value = "0";
    document.getElementById("cmbMonthWeek").selectedIndex = 0;
    document.getElementById("txtMonthWeek_PN_txtGlobal").value = "";
    document.getElementById("chkComorbidity").checked = false;

    SetInnerText(document.getElementById("lblNextVisitDate_PN_Value"), "");
    document.getElementById("txtMonthWeek_PN_txtGlobal").onchange = function(){ComputeNextVisitDate();}
    document.getElementById("txtPatientNotes_txtGlobal").onchange = function(){txtPatientNotes_onchange();}
    
    document.getElementById("divErrorMessage").style.display = "none";
    SetInnerText(document.getElementById("pErrorMessage"), "");
    
    CheckRVStatus();
    return;
}

//----------------------------------------------------------------------------------------------------------------
function CheckRVStatus(){
    var RVs = document.getElementsByName("ReservoirVolume");
    
    // The R.V fields are only visible when Surgery is equal to LAGB - Lapband, the code is 1
    document.getElementById("lblRV_PN").style.visibility = (document.getElementById("tblPatientTitle_txtSurgeryType_Code").value == "1") ? "visible" : "hidden";
    document.getElementById("txtRV_PN_txtGlobal").style.visibility = (document.getElementById("tblPatientTitle_txtSurgeryType_Code").value == "1") ? "visible" : "hidden";
    document.getElementById("lblReservoirVolume_TH").style.visibility = (document.getElementById("tblPatientTitle_txtSurgeryType_Code").value == "1") ? "visible" : "hidden";
    
    if (document.getElementById("tblPatientTitle_txtSurgeryType_Code").value != "1") 
        for (xh = 0; xh < RVs.length; xh++)
            SetInnerText(RVs[xh], "");
}

//----------------------------------------------------------------------------------------------------------------
function btnCancelVisit_onclick(btnCancel){
    if (document.getElementById("btnAddVisit").style.display == "none"){
        ShowVisitFormDiv(1);
        btnCancel.value = "Cancel";
    }
    else{
        btnCancel.value = "Add as new";
        ProgressNotes_ClearFields();
        RemoveUploadLinkSetting();
    }
    return;
}

//----------------------------------------------------------------------------------------------------------------
function btnAddVisit_onclick(){
    Prepare2SaveData();
}

//----------------------------------------------------------------------------------------------------------------
function Prepare2SaveData(){
    var tempWeight = 0;
    
    document.getElementById("txtHSaveResult").value = "0";
    document.getElementById("divErrorMessage").style.display = "none";
    
    if (ProgressNotes_CanToSave())
    {
        tempWeight = parseInt(document.getElementById("txtWeight_PN_txtGlobal").value);
    
        document.getElementById("txtHSaveFlag").value = "1";
        if (document.getElementById("tblPatientTitle_txtUseImperial").value == "1") // Imperial Mode
            tempWeight = tempWeight * 0.45359237;

        document.getElementById("txtHWeight_PN").value = tempWeight;
    }
    else{
        document.getElementById("divErrorMessage").style.display = "block";
        SetInnerText(document.getElementById("pErrorMessage"), "Please enter WEIGHT data...");
    }
}

//---------------------------------------------------------------------------------------------------------------
function btnAddFile_OnClick(){
    var linkUpload = document.getElementById("linkUpload");
    
    try{valid.dispose(linkUpload);}catch(e){}
    
    Prepare2SaveData();
    //linkUpload.href = "../../UploadDocument/UploadDocumentForm.aspx?PCode=1&EID=" + document.getElementById("txtHConsultID").value+"&ET=V"; 
    linkUpload.href = "UploadDocumentForm.aspx?PCode=1&EID=" + document.getElementById("txtHConsultID").value+"&ET=V"; 
    initialize();
}

//---------------------------------------------------------------------------------------------------------------
function RemoveUploadLinkSetting(){
    var linkUpload = document.getElementById("linkUpload");
    
    try{
        valid.dispose(linkUpload);
    }
    catch(e){}
}

//-----------------------------------------------------------------------------------------------------------------
function ShowVisitFormDiv(ShowFlag){  
    document.getElementById("btnAddVisit").style.display = (ShowFlag == 1) ? "block" : "none";
    document.getElementById("btnCancel").value = (ShowFlag == 1) ? "Cancel" : "Add new visit"; 
    document.getElementById("divVisitDataForm").style.display = (ShowFlag == 1) ? "block" : "none";
    
    return;
}

//----------------------------------------------------------------------------------------------------------------
function VisitRow_onclick(strConsultID)
{
    IsVisitSelected = true;
    ShowVisitFormDiv(1);
    LoadVisitData("tblVisitRow_" + strConsultID ,  strConsultID );
}

//-----------------------------------------------------------------------------------------------------------------
function LoadVisitData(tblVisitRow, strConsultID) 
{ 
    var Rows = document.getElementById(tblVisitRow).rows; 
    
    document.getElementById("txtHConsultID").value = strConsultID; 
    document.getElementById("txtNotes_PN_txtGlobal").value = "";
    for (rowIdx = 0; rowIdx < Rows.length; rowIdx++) 
    { 
        var Cells = Rows[rowIdx].cells; 
        for (cellIdx = 0; cellIdx < Cells.length; cellIdx++) 
            if (rowIdx == 0) 
            { 
                switch(cellIdx) 
                { 
                    case 0 :   // document.all means IE browser
                        document.getElementById("txtDateSeen_PN_txtGlobal").value = document.all ? Cells[cellIdx].innerText : Cells[cellIdx].textContent; 
                        break; 
                    case 2 : 
                        document.getElementById("txtWeight_PN_txtGlobal").value = document.all ? Cells[cellIdx].innerText : Cells[cellIdx].textContent; 
                        break; 
                    case 3 : 
                        document.getElementById("txtRV_PN_txtGlobal").value = document.all ? Cells[cellIdx].innerText : Cells[cellIdx].textContent; 
                        break; 
                    case 8 : 
                        document.getElementById("cmbDoctorList_PN_DoctorsList").value = document.getElementById("txtDoctorId_" + strConsultID).value;
                        break; 
                    case 9 : 
                        document.getElementById("txtNextVisitDate_PN").value = document.all ? Cells[cellIdx].innerText : Cells[cellIdx].textContent; 
                        if (document.all)
                            document.getElementById("lblNextVisitDate_PN_Value").innerText = Cells[cellIdx].innerText; 
                        else
                            document.getElementById("lblNextVisitDate_PN_Value").textContent = Cells[cellIdx].textContent; 
                        break; 
                } 
            } 
            else{ 
                if (cellIdx == 1) 
                { 
                    document.getElementById("txtNotes_PN_txtGlobal").value = document.all ? Cells[cellIdx].innerText : Cells[cellIdx].textContent; 
                } 
            } 
    } 
    
    if (document.getElementById("div_ComorbidityVisitsList").style.display.toUpperCase() == "BLOCK"){
        document.getElementById("chkComorbidity").checked = true;
        CheckComorbidityVisit(document.getElementById("chkComorbidity"));
        XmlHttpSubmit("Includes/VisitAjaxForm.aspx?QSN=LOADVISITDATA&CID=" + document.getElementById("txtHConsultID").value,  LoadVisitData_CallBack);
    }
    return;
}

//-----------------------------------------------------------------------------------------------------------------
function LoadVisitData_CallBack(){
    if (XmlHttp.readyState == 4)  
        if ((XmlHttp.status == 200) && (XmlHttp.responseText != "-1")){
            var intChildQty = 0;
            CreateXmlDocument();
            if (document.all) // code for IE
                intChildQty = XmlDoc.documentElement.childNodes.length;
            else
                intChildQty = parseInt(XmlDoc.documentElement.childNodes.length / 2);
            if (intChildQty > 0) LoadVisitComorbidityData(XmlDoc.documentElement);
        }
    return;
}

//-----------------------------------------------------------------------------------------------------------------
function LoadVisitComorbidityData(dvXML){
    // Blood Pressure
    document.getElementById("txtSystolicBP_txtGlobal").value = dvXML.getElementsByTagName("SystolicBP")[0].hasChildNodes() ? dvXML.getElementsByTagName("SystolicBP")[0].firstChild.nodeValue : "";
    document.getElementById("txtDiastolicBP_txtGlobal").value = dvXML.getElementsByTagName("DiastolicBP")[0].hasChildNodes() ? dvXML.getElementsByTagName("DiastolicBP")[0].firstChild.nodeValue : "";
    document.getElementById("txtBPRxDetails_txtGlobal").value = dvXML.getElementsByTagName("BPRxDetails")[0].hasChildNodes() ? dvXML.getElementsByTagName("BPRxDetails")[0].firstChild.nodeValue : "";
    document.getElementById("chkHypertensionResolved").checked = dvXML.getElementsByTagName("HypertensionResolved")[0].hasChildNodes() ? eval(dvXML.getElementsByTagName("HypertensionResolved")[0].firstChild.nodeValue) : false;
    document.getElementById("txtHypertensionResolvedDate_txtGlobal").value = dvXML.getElementsByTagName("strHypertensionResolvedDate")[0].hasChildNodes() ? dvXML.getElementsByTagName("strHypertensionResolvedDate")[0].firstChild.nodeValue : "";
   
    document.getElementById("txtFBloodGlucose_txtGlobal").value = dvXML.getElementsByTagName("FBloodGlucose")[0].hasChildNodes() ? dvXML.getElementsByTagName("FBloodGlucose")[0].firstChild.nodeValue : "";
    document.getElementById("txtHBA1C_txtGlobal").value = dvXML.getElementsByTagName("HBA1C")[0].hasChildNodes() ? dvXML.getElementsByTagName("HBA1C")[0].firstChild.nodeValue : "";
    document.getElementById("txtDiabetesRxDetails_txtGlobal").value = dvXML.getElementsByTagName("DiabetesRxDetails")[0].hasChildNodes() ? dvXML.getElementsByTagName("DiabetesRxDetails")[0].firstChild.nodeValue : "";
    document.getElementById("chkDiabetesResolved").checked = dvXML.getElementsByTagName("DiabetesResolved")[0].hasChildNodes() ? eval(dvXML.getElementsByTagName("DiabetesResolved")[0].firstChild.nodeValue) : false;
    document.getElementById("txtDiabetesResolvedDate_txtGlobal").value = dvXML.getElementsByTagName("strDiabetesResolvedDate")[0].hasChildNodes() ? dvXML.getElementsByTagName("strDiabetesResolvedDate")[0].firstChild.nodeValue : "";
    
    document.getElementById("txtTotalCholesterol_txtGlobal").value = dvXML.getElementsByTagName("TotalCholesterol")[0].hasChildNodes() ? dvXML.getElementsByTagName("TotalCholesterol")[0].firstChild.nodeValue : "";
    document.getElementById("txtTriglycerides_txtGlobal").value = dvXML.getElementsByTagName("Triglycerides")[0].hasChildNodes() ? dvXML.getElementsByTagName("Triglycerides")[0].firstChild.nodeValue : "";
    document.getElementById("txtHDLCholesterol_txtGlobal").value = dvXML.getElementsByTagName("HDLCholesterol")[0].hasChildNodes() ? dvXML.getElementsByTagName("HDLCholesterol")[0].firstChild.nodeValue : "";
    document.getElementById("txtLipidRxDetails_txtGlobal").value = dvXML.getElementsByTagName("LipidRxDetails")[0].hasChildNodes() ? dvXML.getElementsByTagName("LipidRxDetails")[0].firstChild.nodeValue : "";
    document.getElementById("chkLipidsResolved").checked = dvXML.getElementsByTagName("LipidsResolved")[0].hasChildNodes() ? eval(dvXML.getElementsByTagName("LipidsResolved")[0].firstChild.nodeValue) : "";
    document.getElementById("txtLipidsResolvedDate_txtGlobal").value = dvXML.getElementsByTagName("strLipidsResolvedDate")[0].hasChildNodes() ? dvXML.getElementsByTagName("strLipidsResolvedDate")[0].firstChild.nodeValue : "";
    
    // Major Comorbidity
    document.getElementById("txtAsthmaResolvedDate_txtGlobal").value = dvXML.getElementsByTagName("AsthmaResolved")[0].hasChildNodes() ? dvXML.getElementsByTagName("AsthmaResolved")[0].firstChild.nodeValue : "";
    LoadBaseLevels(document.getElementById("cmbAsthmaCurrentLevel_SystemCodeList"), document.getElementById("lblBaseAsthmaLevel_Value"), dvXML, "BaseAsthmaLevel");
    document.getElementById("cmbAsthmaCurrentLevel_SystemCodeList").value = dvXML.getElementsByTagName("AsthmaCurrentLevel")[0].hasChildNodes() ? dvXML.getElementsByTagName("AsthmaCurrentLevel")[0].firstChild.nodeValue : "";
    document.getElementById("txtAsthmaResolvedDate_txtGlobal").value = dvXML.getElementsByTagName("strAsthmaResolvedDate")[0].hasChildNodes() ? dvXML.getElementsByTagName("strAsthmaResolvedDate")[0].firstChild.nodeValue : "";
    
    document.getElementById("txtBaseAsthmaDetails_txtGlobal").value = dvXML.getElementsByTagName("BaseAsthmaDetails")[0].hasChildNodes() ? dvXML.getElementsByTagName("BaseAsthmaDetails")[0].firstChild.nodeValue : "";
    LoadBaseLevels(document.getElementById("cmbRefluxCurrentLevel_SystemCodeList"), document.getElementById("lblBaseRefluxLevel_Value"), dvXML, "BaseRefluxLevel");
    document.getElementById("txtRefluxResolvedDate_txtGlobal").value = dvXML.getElementsByTagName("strRefluxResolvedDate")[0].hasChildNodes() ? dvXML.getElementsByTagName("strRefluxResolvedDate")[0].firstChild.nodeValue : "";
    document.getElementById("cmbRefluxCurrentLevel_SystemCodeList").value = dvXML.getElementsByTagName("RefluxCurrentLevel")[0].hasChildNodes() ? dvXML.getElementsByTagName("RefluxCurrentLevel")[0].firstChild.nodeValue : "";
    
    document.getElementById("txtBaseRefluxDetails_txtGlobal").value = dvXML.getElementsByTagName("BaseRefluxDetails")[0].hasChildNodes() ? dvXML.getElementsByTagName("BaseRefluxDetails")[0].firstChild.nodeValue : "";
    LoadBaseLevels(document.getElementById("cmbSleepCurrentLevel_SystemCodeList"), document.getElementById("lblBaseSleepLevel_Value"), dvXML, "BaseSleepLevel");
    document.getElementById("txtSleepResolvedDate_txtGlobal").value = dvXML.getElementsByTagName("strSleepResolvedDate")[0].hasChildNodes() ? dvXML.getElementsByTagName("strSleepResolvedDate")[0].firstChild.nodeValue : "";
    document.getElementById("cmbSleepCurrentLevel_SystemCodeList").value = dvXML.getElementsByTagName("SleepCurrentLevel")[0].hasChildNodes() ? dvXML.getElementsByTagName("SleepCurrentLevel")[0].firstChild.nodeValue : "";
    document.getElementById("txtBaseSleepDetails_txtGlobal").value = dvXML.getElementsByTagName("BaseSleepDetails")[0].hasChildNodes() ? dvXML.getElementsByTagName("BaseSleepDetails")[0].firstChild.nodeValue : "";
    
    // Minor Comorbidity
    document.getElementById("cmbFertilityCurrentLevel_SystemCodeList").value = dvXML.getElementsByTagName("FertilityCurrentLevel")[0].hasChildNodes() ? dvXML.getElementsByTagName("FertilityCurrentLevel")[0].firstChild.nodeValue : "";
    document.getElementById("txtFertilityResolvedDate_txtGlobal").value = dvXML.getElementsByTagName("strFertilityResolvedDate")[0].hasChildNodes() ? dvXML.getElementsByTagName("strFertilityResolvedDate")[0].firstChild.nodeValue : "";
    document.getElementById("txtBaseFertilityDetails_txtGlobal").value = dvXML.getElementsByTagName("BaseFertilityDetails")[0].hasChildNodes() ? dvXML.getElementsByTagName("BaseFertilityDetails")[0].firstChild.nodeValue : "";
    
    document.getElementById("cmbIncontinenceCurrentLevel_SystemCodeList").value = dvXML.getElementsByTagName("IncontinenceCurrentLevel")[0].hasChildNodes() ? dvXML.getElementsByTagName("IncontinenceCurrentLevel")[0].firstChild.nodeValue : "";
    document.getElementById("txtIncontinenceResolvedDate_txtGlobal").value = dvXML.getElementsByTagName("strIncontinenceResolvedDate")[0].hasChildNodes() ? dvXML.getElementsByTagName("strIncontinenceResolvedDate")[0].firstChild.nodeValue : "";
    document.getElementById("txtBaseIncontinenceDetails_txtGlobal").value = dvXML.getElementsByTagName("BaseIncontinenceDetails")[0].hasChildNodes() ? dvXML.getElementsByTagName("BaseIncontinenceDetails")[0].firstChild.nodeValue : "";
    
    document.getElementById("cmbBackCurrentLevel_SystemCodeList").value = dvXML.getElementsByTagName("BackCurrentLevel")[0].hasChildNodes() ? dvXML.getElementsByTagName("BackCurrentLevel")[0].firstChild.nodeValue : "";
    document.getElementById("txtBackResolvedDate_txtGlobal").value = dvXML.getElementsByTagName("strBackResolvedDate")[0].hasChildNodes() ? dvXML.getElementsByTagName("strBackResolvedDate")[0].firstChild.nodeValue : "";
    document.getElementById("txtBaseBackDetails_txtGlobal").value = dvXML.getElementsByTagName("BaseBackDetails")[0].hasChildNodes() ? dvXML.getElementsByTagName("BaseBackDetails")[0].firstChild.nodeValue : "";
    
    document.getElementById("cmbArthritisCurrentLevel_SystemCodeList").value = dvXML.getElementsByTagName("ArthritisCurrentLevel")[0].hasChildNodes() ? dvXML.getElementsByTagName("ArthritisCurrentLevel")[0].firstChild.nodeValue : "";
    document.getElementById("txtArthritisResolvedDate_txtGlobal").value = dvXML.getElementsByTagName("strArthritisResolvedDate")[0].hasChildNodes() ? dvXML.getElementsByTagName("strArthritisResolvedDate")[0].firstChild.nodeValue : "";
    document.getElementById("txtBaseArthritisDetails_txtGlobal").value = dvXML.getElementsByTagName("BaseArthritisDetails")[0].hasChildNodes() ? dvXML.getElementsByTagName("BaseArthritisDetails")[0].firstChild.nodeValue : "";
    
    document.getElementById("cmbCVDLevel_SystemCodeList").value = dvXML.getElementsByTagName("CVDLevelCurrentLevel")[0].hasChildNodes() ? dvXML.getElementsByTagName("CVDLevelCurrentLevel")[0].firstChild.nodeValue : "";
    document.getElementById("txtCVDLevelResolvedDate_txtGlobal").value = dvXML.getElementsByTagName("strCVDLevelResolvedDate")[0].hasChildNodes() ? dvXML.getElementsByTagName("strCVDLevelResolvedDate")[0].firstChild.nodeValue : "";
    document.getElementById("txtBaseCVDDetails_txtGlobal").value = dvXML.getElementsByTagName("BaseCVDDetails")[0].hasChildNodes() ? dvXML.getElementsByTagName("BaseCVDDetails")[0].firstChild.nodeValue : "";
    
    document.getElementById("txtBaseOtherDetails_txtGlobal").value = dvXML.getElementsByTagName("BaseOtherDetails")[0].hasChildNodes() ? dvXML.getElementsByTagName("BaseOtherDetails")[0].firstChild.nodeValue : "";
}

//-----------------------------------------------------------------------------------------------------------------
function LoadBaseLevels(cmbBaseLevel, txtBaseLevel, dvXML, strBaseLevelName)
{
    var strValue = "";
    
    if (dvXML.getElementsByTagName(strBaseLevelName)[0].hasChildNodes()){
        strValue = dvXML.getElementsByTagName(strBaseLevelName)[0].firstChild.nodeValue;
        cmbBaseLevel.value = strValue;
        strValue = (cmbBaseLevel.selectedIndex > -1) ? cmbBaseLevel.options[cmbBaseLevel.selectedIndex].text : ""; 
    }
    SetInnerText(txtBaseLevel, strValue);
    return;
}

//-----------------------------------------------------------------------------------------------------------------
function cmbMonthWeek_OnChange() 
{ 
    ComputeNextVisitDate();
    return;
} 

//---------------------------------------------------------------------------------------------------------------
function ComputeNextVisitDate()
{
    var cmbMonthWeek = document.getElementById("cmbMonthWeek"),
        strMonthWeek = document.getElementById("txtMonthWeek_PN_txtGlobal").value,
        strDateSeen = document.getElementById("txtDateSeen_PN_txtGlobal").value;
    var xmlSOAP;
    
    if ((parseFloat(strMonthWeek) != "NaN") && (cmbMonthWeek.selectedIndex > 0)){
        SetCursor("wait");
        var strSOAP = 
            '<?xml version="1.0" encoding="utf-8"?>'+
            '<soap:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" ' + 
                            'xmlns:xsd="http://www.w3.org/2001/XMLSchema" ' + 
                            'xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">'+
	            '<soap:Body>'+
		            '<UpdateNextDateVisit xmlns="http://tempuri.org/">'+
			            '<strDateSeen>' + strDateSeen + '</strDateSeen>'+
			            '<strMonthWeek>' + strMonthWeek + '</strMonthWeek>'+
			            '<strMonthWeekFlag>' + cmbMonthWeek.options[cmbMonthWeek.selectedIndex].value + '</strMonthWeekFlag>'+
		            '</UpdateNextDateVisit>'+
	            '</soap:Body>'+
            '</soap:Envelope>';
	            
	    SubmitSOAPXmlHttp(strSOAP, ComputeNextVisitDate_CallBack, "Includes/ConsultFU1WebService.asmx", "http://tempuri.org/UpdateNextDateVisit");
    }
    return;
}

//---------------------------------------------------------------------------------------------------------------
function ComputeNextVisitDate_CallBack() 
{ 
    var strDate = new String();
    SetCursor("default");
    if(XmlHttp.readyState == 4)  
        if(XmlHttp.status == 200)  {
            var response  = XmlHttp.responseXML.documentElement,
                strReturnValue = response.getElementsByTagName('ReturnValue')[0].firstChild.data;
            switch(strReturnValue) { 
                case "E1" : //DateSeen is wrong
                    document.getElementById("lblDateSeen_PN").style.color = "RED"; 
                    document.getElementById("txtDateSeen_PN_txtGlobal").focus(); 
                    SetInnerText(document.getElementById("lblNextVisitDate_PN_Value"), "");
                    document.getElementById("txtNextVisitDate_PN").value = "";
                    break; 
                    
                case "E2" : // the Month or Week value is wrong
                    document.getElementById("lblNextVisitDate_PN").style.color = "RED"; 
                    document.getElementById("txtMonthWeek_PN_txtGlobal").focus(); 
                    SetInnerText(document.getElementById("lblNextVisitDate_PN_Value"), "");
                    document.getElementById("txtNextVisitDate_PN").value = "";
                    break;
       
                default :
                    strDate = strReturnValue;
                    document.getElementById("lblDateSeen_PN").style.color = "";
                    document.getElementById("lblNextVisitDate_PN").style.color = ""; 
                    document.getElementById("txtNextVisitDate_PN").value = strDate.substring(0, strDate.indexOf(";")) ; 
                    SetInnerText(document.getElementById("lblNextVisitDate_PN_Value"), strDate.substring(strDate.indexOf(";")+1));
                    break;  
            }  
        }
    return;
}

//----------------------------------------------------------------------------------------------------------------
function ProgressNotes_CanToSave(){
    var flag = new Boolean();
    
    flag = true;
    
    document.getElementById("lblDateSeen_PN").style.color = "";
    if (document.getElementById("txtDateSeen_PN_txtGlobal").value.length == 0){
        document.getElementById("lblDateSeen_PN").style.color = "RED";
        flag = false;
    }
    
    document.getElementById("lblWeight_PN").style.color = "";
    if (document.getElementById("txtWeight_PN_txtGlobal").value.length == 0){
        document.getElementById("lblWeight_PN").style.color = "RED";
        flag = false;
    }
    return(flag);
}

//----------------------------------------------------------------------------------------------------------------
function CheckAllVisitDocument(){
    var cmbDocument = document.getElementsByName("cmbDocument");
    
    for (Xh = 0; Xh < cmbDocument.length; Xh++){
        cmbDocument[Xh].style.display = (cmbDocument[Xh].options.length > 0) ? "block" : "none";
        if (cmbDocument[Xh].options.length > 0){
            var objOption = document.createElement("OPTION");
            objOption.selected = true;
            objOption.value = 0;
            objOption.text = cmbDocument[Xh].options.length + " " + ((cmbDocument[Xh].options.length == 1) ? "File" : "Files");
            cmbDocument[Xh].options.add(objOption, 0);
        }
        else{ 
            // if cmdDocument doesn't have any options, when user click the document cell, the visit data should be loaded
            var VisitID = cmbDocument[Xh].id.substring(cmbDocument[Xh].id.lastIndexOf("_") + 1);
            var tdDocument = document.getElementById("tdDocument_" + VisitID);
            tdDocument.onclick = function(){
                VisitRow_onclick(this.id.substring(this.id.lastIndexOf("_") + 1));
            }
        }
    }
    document.getElementById("div_VisitsList").style.display = "block";
}

//----------------------------------------------------------------------------------------------------------------
function AddImageVideoButton(tcTemp, strConsultID, FileType, strImageVideoFileName){
    var btnVideo = document.createElement("input");
    
    btnVideo.type = "button";
    btnVideo.value = strImageVideoFileName;
    btnVideo.style.width = "95%";
    btnVideo.onclick = function(){
        var strOption = "titlebar=0,toolbar=0,scrollbars=1";
        switch(FileType.toUpperCase()){
            case "VIDEO" :
                window.open("../../../Videos/flvplayer.html", "titlebar=1,toolbar=1,fullscreen=1,resizable=1");
                break;
                
            case "PHOTO" :
                window.open("VisitDocuments/ViewDocumentsForm.aspx?CID=" + strConsultID + "&DT=1", "", strOption);
                break;
        }
    }
    tcTemp.appendChild(btnVideo);
    return;
}

//----------------------------------------------------------------------------------------------------------------
function AddImageVideoDropdownList(tcTemp, strConsultID, FileType, strImageVideoFileName)
{
    var intDocType = 0, intDocumentQty = 0, DocIdx = 0, VisitDocumentQty = 0;
    
    intDocumentQty = XmlDoc.getElementsByTagName("tblDocuments").length;
    switch(FileType)
    {
        case "Photo" :
            intDocType = 1;
            break;
            
        case "Video" :
            intDocType = 2;
            break;
            
        case "Document" :
            intDocType = 3;
            break;
    }
    try{
        var objSelect = document.createElement("SELECT"), objOption = document.createElement("OPTION");
        
        for (DocIdx = 0; DocIdx < intDocumentQty; DocIdx++){
            var ConsultID, DocumentType, DocumentFileName, DocumentName;
            
            if (XmlDoc.getElementsByTagName("tblDocuments")[DocIdx].getElementsByTagName("EventID")[0].hasChildNodes()){
                ConsultID = XmlDoc.getElementsByTagName("tblDocuments")[DocIdx].getElementsByTagName("EventID")[0].firstChild.nodeValue;
                DocumentType  = XmlDoc.getElementsByTagName("tblDocuments")[DocIdx].getElementsByTagName("DocumentType")[0].firstChild.nodeValue;
                DocumentFileName = XmlDoc.getElementsByTagName("tblDocuments")[DocIdx].getElementsByTagName("Consult_DocumentFileName")[0].firstChild.nodeValue;
                DocumentName = XmlDoc.getElementsByTagName("tblDocuments")[DocIdx].getElementsByTagName("Consult_DocumentName")[0].firstChild.nodeValue;
                
                if ((strConsultID == ConsultID) && (intDocType == parseInt(DocumentType)))
                {
                    objOption = document.createElement("OPTION");
                    objOption.value = DocumentFileName;
                    objOption.text = DocumentName;
                    objSelect.options.add(objOption);
                    VisitDocumentQty++;
                }
            }
        }
        if (VisitDocumentQty > 0){
            objSelect.style.width = "100%";
            
            objOption = document.createElement("OPTION");
            objOption.selected = "true";
            objOption.value = 0;
            objOption.text = VisitDocumentQty + " " + ((VisitDocumentQty > 1) ? FileType + "s" : FileType);
            objSelect.options.add(objOption, 0);
            
            objSelect.onchange = function(){
                var strOption = "menubar=0,titlebar=0,toolbar=0,scrollbars=1,resizable=1";
                
                if (objSelect.selectedIndex == 0) return;
                switch(FileType)
                {
                    case "Photo" :
                        window.open("VisitDocuments/ViewDocumentsForm.aspx?CID=" + strConsultID + "&DT=1&Photo=" + objSelect.value, "", strOption);
                        
                        break;
                        
                    case "Video" :
                        window.open("VisitDocuments/ViewDocumentsForm.aspx?CID=" + strConsultID + "&DT=2&Video=" + objSelect.value, "", strOption);
                        break;
                        
                    case "Document" :
                        window.open("VisitDocuments/ViewDocumentsForm.aspx?CID=" + strConsultID + "&DT=3&Document=" + objSelect.value, "", strOption);
                        break;
                }
                objSelect.selectedIndex = 0;
            }
            tcTemp.appendChild(objSelect);
        }
    }
    catch(e){/*alert(e.message);*/}
    return;
}


//----------------------------------------------------------------------------------------------------------------
function AddDateToPatientNotes(where){
    var strSOAP = 
        '<?xml version="1.0" encoding="utf-8"?>'+
        '<soap:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" ' + 
                        'xmlns:xsd="http://www.w3.org/2001/XMLSchema" ' + 
                        'xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">'+
            '<soap:Body>'+
	            '<FetchCurrentDate xmlns="http://tempuri.org/">'+
	            '</FetchCurrentDate>'+
            '</soap:Body>'+
        '</soap:Envelope>';
    
    SetCursor("wait");
    WhereFlag = where;
    SubmitSOAPXmlHttp(strSOAP, AddDateToPatientNotes_CallBack, "Includes/ConsultFU1WebService.asmx", "http://tempuri.org/FetchCurrentDate");
    return;
}

//----------------------------------------------------------------------------------------------------------------
function AddDateToPatientNotes_CallBack(){
    SetCursor("default");
    if(XmlHttp.readyState == 4)  
        if(XmlHttp.status == 200)  {
            var response  = XmlHttp.responseXML.documentElement,
                strReturnValue = response.getElementsByTagName('ReturnValue')[0].firstChild.data;
            
            if (WhereFlag == 'Start')
                document.getElementById("txtPatientNotes_txtGlobal").value = strReturnValue + "\r \t" + document.getElementById("txtPatientNotes_txtGlobal").value;
            else
                document.getElementById("txtPatientNotes_txtGlobal").value = document.getElementById("txtPatientNotes_txtGlobal").value + "\r" + strReturnValue ;
                
            // after adding Date at the Start or End of Patient Notes, we should update the "NOTES" in "tblPatientWeightData"
            txtPatientNotes_onchange();
        } // if status == 200
    return;
}

//----------------------------------------------------------------------------------------------------------------
function txtPatientNotes_onchange(){
    var strSOAP = 
            '<?xml version="1.0" encoding="utf-8"?>'+
            '<soap:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" ' + 
                            'xmlns:xsd="http://www.w3.org/2001/XMLSchema" ' + 
                            'xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">'+
	            '<soap:Body>'+
		            '<UpdatePatientNotes xmlns="http://tempuri.org/">'+
		            '   <strPatientNotes>' + document.getElementById("txtPatientNotes_txtGlobal").value + '</strPatientNotes>' + 
		            '</UpdatePatientNotes>'+
	            '</soap:Body>'+
            '</soap:Envelope>';
    
    SubmitSOAPXmlHttp(strSOAP, null, "Includes/ConsultFU1WebService.asmx", "http://tempuri.org/UpdatePatientNotes");
    return;
}


/*************************************************************************************************************** /
this section is for Report in Visit Page
/***************************************************************************************************************/
//----------------------------------------------------------------------------------------------------------------
function CheckOperation(obj, checked){

    obj.checked = (checked == 1) ? !obj.checked : obj.checked; 
    document.getElementById('ReportCode').value = obj.checked ? 'OD' : '';
    return;
}

//----------------------------------------------------------------------------------------------------------------
function BuildReport(PreviewFlag){
    var strParam = ReportParams( );
    
    if (PreviewFlag == 0){
        var frameSRC = "../../../Reports/BuildReportPage.aspx?RP=" + document.getElementById('ReportCode').value;
        frameSRC += "&PID=" + document.getElementById("txtHPatientID").value + "&Param=" + strParam;
        
        document.getElementById('frameReport').src = frameSRC;
        LoadingTimer = window.setInterval("CheckReportIsLoaded();", 1000, "JavaScript");
    }
    else{
        //var strURL = '../../../Reports/ReportGlobalForm.aspx?RP=' + strReportCode;
        var strURL = '../../../Reports/BuildReportPage.aspx?RP=' + document.getElementById('ReportCode').value;
        strURL += "&PID=" + document.getElementById("txtHPatientID").value + "&Param=" + strParam;
        
        window.open(strURL, null, 'scrollbars=yes,fullscreen=yes,toolbar=no,menubar=no,location=no,resizable=yes');
    }
}

//----------------------------------------------------------------------------------------------------------------
function ReportParams( ){
    var strParam = new String();
    var cmbDoctor1 = document.getElementById('cmbDoctor1');
    var cmbDoctor2 = document.getElementById('cmbDoctor2');
    var cmbDoctor3 = document.getElementById('cmbDoctor3');
    
    switch(document.getElementById('ReportCode').value)
    {
        case "FUA" :
            strParam  = document.getElementById('chkIncludePhoto').checked ? "1" : "0";
            strParam += document.getElementById('chkIncludeProgressNotes').checked ? "1" : "0";
            strParam += document.getElementById('chkIncludePatientNote').checked ? "1" : "0";
            break;
        case "RDL" :
            strParam  = document.getElementById('chkCurrentVisit').checked ? "1" : "0";
            strParam += document.getElementById('chkProgressNotes').checked ? "1" : "0";
            strParam += document.getElementById('chkPatientNote').checked ? "1" : "0";
            strParam += ((cmbDoctor1.options.length > 1) && (cmbDoctor1.selectedIndex > 0) && (strParam.indexOf(";" + cmbDoctor1.value) == -1)) ? (";" + cmbDoctor1.value) : "";
            strParam += ((cmbDoctor2.options.length > 1) && (cmbDoctor2.selectedIndex > 0) && (strParam.indexOf(";" + cmbDoctor2.value) == -1)) ? (";" + cmbDoctor2.value) : "";
            strParam += ((cmbDoctor3.options.length > 1) && (cmbDoctor3.selectedIndex > 0) && (strParam.indexOf(";" + cmbDoctor3.value) == -1)) ? (";" + cmbDoctor3.value) : "";
            break;
        default :
            strParam = "";
            break;
    }
    return(strParam);
}

//----------------------------------------------------------------------------------------------------------------
function CheckReportIsLoaded(objFrame){
    if (document.getElementById('frameReport').contentDocument.body.offsetHeight > 0){
        window.clearInterval(LoadingTimer);
        parent.frames[0].print();
    }
}

//----------------------------------------------------------------------------------------------------------------
function ShowDivReportItems(strObjName){
    var ReportTitles = new Array("FollowUpDetails", "LetterToDoctor", "Graphs", "Comorbidities", "OtherReports");
    var Xh = 0;
    
    for (; Xh < 5; Xh++){
        document.getElementById("chk" + ReportTitles[Xh]).checked = false;
        document.getElementById("div" + ReportTitles[Xh]).style.display = 'none';
        document.getElementById("div" + ReportTitles[Xh]).style.visibility = 'hidden';
    }
    document.getElementById("chk" + strObjName).checked = true;
    document.getElementById("div" + strObjName).style.display = 'block';
    document.getElementById("div" + strObjName).style.visibility = 'visible';
    GetTheSelectedReportName(strObjName);
    document.getElementById("divPrintBtns").style.display = 'block';
    document.getElementById("divPrintBtns").style.visibility = 'visible';
}

//----------------------------------------------------------------------------------------------------------------
function GetTheSelectedReportName(strObjName)
{
    var rbTemp = document.getElementsByName("rb" + strObjName);
    var GraphsReportCode = new Array("EWLG", "WLG", "", "");
    var ComorbiditiesReportCode = new Array("BLC", "", "");
    var Checked = false;
    
    for (Xh = 0; (Xh < rbTemp.length) && !Checked; Xh++)
        if (rbTemp[Xh].checked ){
            Checked = true;
            
            switch(strObjName){
                case "Graphs" : 
                    document.getElementById("ReportCode").value = GraphsReportCode[Xh];
                    break;
                case "Comorbidities" :
                    document.getElementById("ReportCode").value = ComorbiditiesReportCode[Xh];
                    break;
                case "OtherReports" :
                    document.getElementById('ReportCode').value = '';
                    document.getElementById('chkOperation').checked = false;
                    document.getElementById('chkPhotos').checked = false;
                    break;
            }
        }
}

//----------------------------------------------------------------------------------------------------------------
function LoadDoctorsList(){
    var requestURL = "Includes/VisitAjaxForm.aspx?QSN=LOADDOCTORS";
    SetCursor("wait");
    XmlHttpSubmit(requestURL,  LoadDoctorsList_CallBack);
    return;
}

//----------------------------------------------------------------------------------------------------------------
function LoadDoctorsList_CallBack(){
    if(XmlHttp.readyState == 4) 
        if(XmlHttp.status == 200)  {
            //document.getElementById("txtDoctorsList").value = "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?>" + XmlHttp.responseText;
            document.getElementById("txtDoctorsList").value = XmlHttp.responseText;
            CreateXmlDocument();
            try {FillDoctorDropdownLists();}catch (e){}
        }
    SetCursor("default");
    return;
}

//----------------------------------------------------------------------------------------------------------------
function ReLoadDoctorsList(){
    if (document.getElementById("cmbDoctor1").options.length < 2){
        ReCreateXmlDocument(document.getElementById("txtDoctorsList").value );
        try {FillDoctorDropdownLists();}catch (e){}
    }
}

//----------------------------------------------------------------------------------------------------------------
function FillDoctorDropdownLists(){
    var intChildQty = new Number();
    
    if (document.all) // code for IE
        intChildQty = XmlDoc.documentElement.childNodes.length;
    else
        intChildQty = parseInt(XmlDoc.documentElement.childNodes.length / 2);
        
    try{  
        for(Xh = 0; Xh < intChildQty; Xh++){
            var oOption1 = document.createElement("OPTION"),
                oOption2 = document.createElement("OPTION"),
                oOption3 = document.createElement("OPTION");
            
            if (XmlDoc.getElementsByTagName("RefDrId")[Xh].hasChildNodes())
                oOption1.value = XmlDoc.getElementsByTagName("RefDrId")[Xh].firstChild.nodeValue;
            else
                oOption1.value = "";
                
            if (XmlDoc.getElementsByTagName("Doctor_Name")[Xh].hasChildNodes())
                oOption1.text = XmlDoc.getElementsByTagName("Doctor_Name")[Xh].firstChild.nodeValue;
            else
                oOption1.text = "";
                
            oOption2.value = oOption1.value; 
            oOption2.text = oOption1.text;
            
            oOption3.value = oOption1.value; 
            oOption3.text = oOption1.text;
            
            document.getElementById("cmbDoctor1").options.add(oOption1);
            document.getElementById("cmbDoctor2").options.add(oOption2);
            document.getElementById("cmbDoctor3").options.add(oOption3);
        }
    }catch(e){}
    AddEmtpyOption("cmbDoctor1");
    AddEmtpyOption("cmbDoctor2");
    AddEmtpyOption("cmbDoctor3");
    
    // 2. we should fetch the 3 Doctors for this patient
    var requestURL = "../../Patients/PatientData/Includes/PatientDataAjaxForm.aspx?QSN=LOADPATIENTDATA&PageNo=1";
    XmlHttpSubmit(requestURL,  LoadPatientData_CallBack);
}

//----------------------------------------------------------------------------------------------------------------
function AddEmtpyOption(strDropdownName){
    if (document.getElementById(strDropdownName).options.length > 0){
        var oOption = document.createElement("OPTION");
        
        oOption.value = 0;
        oOption.text = "Select from...";
        oOption.selected = true;
        document.getElementById(strDropdownName).options.add(oOption, 0);
    }
    return;
}
            
//----------------------------------------------------------------------------------------------------------------
function LoadPatientData_CallBack(){
    var intChildQty = 0;
    
    if(XmlHttp.readyState == 4) 
        if(XmlHttp.status == 200)  {
            //document.getElementById("txtDoctorsList").value = "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?>" + XmlHttp.responseText;
            CreateXmlDocument();
            
            if (document.all) // code for IE
                intChildQty = XmlDoc.documentElement.childNodes.length;
            else
                intChildQty = parseInt(XmlDoc.documentElement.childNodes.length / 2);
            
            if (intChildQty > 0) {
                if (XmlDoc.getElementsByTagName("RefDrId1")[0].hasChildNodes())
                    document.getElementById("cmbDoctor1").value = XmlDoc.getElementsByTagName("RefDrId1")[0].firstChild.nodeValue;
                    
                if (XmlDoc.getElementsByTagName("RefDrId2")[0].hasChildNodes())
                    document.getElementById("cmbDoctor2").value = XmlDoc.getElementsByTagName("RefDrId2")[0].firstChild.nodeValue;
                    
                if (XmlDoc.getElementsByTagName("RefDrId3")[0].hasChildNodes())
                    document.getElementById("cmbDoctor3").value = XmlDoc.getElementsByTagName("RefDrId3")[0].firstChild.nodeValue;
            }
        }
    SetCursor("default");
    return;
}

//----------------------------------------------------------------------------------------------------------------
function cmbDocument_onchange(cmbDocument){
    ShowErrorMessageDiv("none", "");
    document.body.style.cursor = "wait";
    if (parseInt(cmbDocument.value) == 0) return;
    var requestURL = document.getElementById("txtHApplicationURL").value + "Forms/FileManagement/FileManagementForm.aspx?ReLoad=0&QSN=LOADINGDOCUMENT";
    requestURL += "&DID=" + cmbDocument.value;
    cmbDocument.selectedIndex = 0; // Set the default row (first option = N Documents)
    XmlHttpSubmit(requestURL,  cmbDocument_onchange_callback);    
}

//---------------------------------------------------------------------------------------------------------------
function cmbDocument_onchange_callback(){
    if (XmlHttp.readyState == 4)
        if (XmlHttp.status == 200)
            LoadingFloodingDiv(XmlHttp.responseText);
        else
            ShowErrorMessageDiv("block", "Unable to load your file...");
    document.body.style.cursor = "";
    return;
}


//---------------------------------------------------------------------------------------------------------------
function CheckComorbidityVisit(chkComorbidity){
    SelectedVisit = !chkComorbidity.checked ? 1 : 2;
    document.getElementById("div_Lists").style.display = !chkComorbidity.checked ? "block" : "none";
    document.getElementById("div_Comorbidity").style.display = chkComorbidity.checked ? "block" : "none";
    
    document.getElementById("li_VisitList").className = !chkComorbidity.checked ? "current" : "";
    document.getElementById("li_ComorbidityList").className = "";
    document.getElementById("li_VisitList").style.display = !chkComorbidity.checked ? "block" : "none";
    document.getElementById("li_ComorbidityList").style.display = !chkComorbidity.checked ? "block" : "none";
    
    document.getElementById("div_VisitsList").style.display = !chkComorbidity.checked ? "block" : "none";
    document.getElementById("div_ComorbidityVisitsList").style.display = chkComorbidity.checked ? "block" : "none";
    
    document.getElementById("li_BloodPressure").className = (chkComorbidity.checked && (document.getElementById("div_BloodPressure").style.display.toUpperCase() == "BLOCK")) ? "current" : "";
    document.getElementById("li_MajorComorbidity").className = (chkComorbidity.checked && (document.getElementById("div_MajorComorbidity").style.display.toUpperCase() == "BLOCK")) ? "current" : "";
    document.getElementById("li_BoldComorbidity").className = (chkComorbidity.checked && (document.getElementById("div_BoldComorbidity").style.display.toUpperCase() == "BLOCK")) ? "current" : "";
    
    document.getElementById("li_BloodPressure").style.display = chkComorbidity.checked ? "block" : "none";
    document.getElementById("li_MajorComorbidity").style.display = chkComorbidity.checked ? "block" : "none";
    document.getElementById("li_BoldComorbidity").style.display = chkComorbidity.checked ? "block" : "none";
}

//---------------------------------------------------------------------------------------------------------------
function VisitPagesClick(idx){
    document.getElementById("li_VisitList").className = (idx == 1) ? "current" : "";
    document.getElementById("li_ComorbidityList").className = (idx == 2) ? "current" : "";
    
    document.getElementById("div_VisitsList").style.display = (idx == 1) ? "block" : "none";
    document.getElementById("div_ComorbidityVisitsList").style.display = (idx == 2) ? "block" : "none";
    SelectedVisit = idx;
}

//---------------------------------------------------------------------------------------------------------------
function ComorbidityPagesClick(idx){
    document.getElementById("li_BloodPressure").className = (idx == 1) ? "current" : "";
    document.getElementById("li_MajorComorbidity").className = (idx == 2) ? "current" : "";
    document.getElementById("li_BoldComorbidity").className = (idx == 3) ? "current" : "";
    document.getElementById("li_Vitamins").className = (idx == 4) ? "current" : "";

    document.getElementById("div_BloodPressure").style.display = (idx == 1) ? "block" : "none";
    document.getElementById("div_MajorComorbidity").style.display = (idx == 2) ? "block" : "none";
    document.getElementById("div_BoldComorbidity").style.display = (idx == 3) ? "block" : "none";
    document.getElementById("div_Vitamins").style.display = (idx == 4) ? "block" : "none";
}

//---------------------------------------------------------------------------------------------------------------
function VisitDivsInitialSetting(){
    document.getElementById("li_VisitList").style.display = "block";
    document.getElementById("li_ComorbidityList").style.display = "block";
    document.getElementById("li_BloodPressure").style.display = "none";
    document.getElementById("li_MajorComorbidity").style.display = "none";
    document.getElementById("li_BoldComorbidity").style.display = "none";
    document.getElementById("li_Vitamins").style.display = "none";

    document.getElementById("div_Lists").style.display = "block";
    document.getElementById("div_Comorbidity").style.display = "none";
    
    document.getElementById("li_VisitList").className = (SelectedVisit == 1) ? "current" : "";
    document.getElementById("li_ComorbidityList").className = (SelectedVisit == 2) ? "current" : "";
    document.getElementById("div_VisitsList").style.display = (SelectedVisit == 1) ? "block" : "none";
    document.getElementById("div_ComorbidityVisitsList").style.display = (SelectedVisit == 2) ? "block" : "none";
}

//-----------------------------------------------------------------------------------------------------------------
function aCalendar_onclick(obj, strDate){
    var strDateformat = document.getElementById("lblDateFormat");
    languageCode = document.forms[0].txtHCulture.value.substr(0, 2);
    displayCalendar(document.getElementById(strDate + "_txtGlobal"), document.all ? strDateformat.innerText : strDateformat.textContent, obj)
} 