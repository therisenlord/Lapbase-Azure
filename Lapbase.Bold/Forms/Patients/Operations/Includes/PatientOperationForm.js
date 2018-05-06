﻿// JScript File
var OperationsListPageNo = 4;
var OperationDetailPageNo = 1;
var OperationOtherDetailPageNo = 2;
var OperationEventPageNo = 3;
var intButtonNo = 0;
var tempColor;
var AppSchemaMenu = new Array("AppSchemaMenu_a_Home", "AppSchemaMenu_a_Baseline", "AppSchemaMenu_a_Visits", 
                        "AppSchemaMenu_a_Operations", "AppSchemaMenu_a_Files", "AppSchemaMenu_a_Complications", 
                        "AppSchemaMenu_a_Comorbidities", "AppSchemaMenu_a_Biochemistry", "AppSchemaMenu_a_EMR", "AppSchemaMenu_a_Labs");
var AppSchemaMenu_href = new Array("Forms/PatientsVisits/PatientsVisitsForm.aspx", 
                        "Forms/Patients/PatientData/PatientDataForm.aspx", 
                        "Forms/PatientsVisits/ConsultFU1/ConsultFU1Form.aspx", 
                        "Forms/Patients/Operations/PatientOperationForm.aspx", 
                        "Forms/FileManagement/FileManagementForm.aspx?ReLoad=1&QSN=Type&SD=0", 
                        "Forms/Adverse/AdverseForm.aspx", 
                        "Forms/Comorbidity/ComorbidityForm.aspx", 
                        "Forms/Biochemistry/BiochemistryForm.aspx",
                        "Forms/EMR/EMRForm.aspx",
                        "Forms/Labs/LabForm.aspx");
var AppSchema_ButtonNo = 0;

var OperationBPD = "BAA1058";
var OperationGastricBaloon = "BAA1060";
var OperationGastricAdjustable = "BAA1061";
var OperationGastricBypassRoux = "BAA1063";
var OperationSleeveGastrectomy = "BAA1067";
var OperationBandingRevision = "ADDBST4";

//---------------------------------------------------------------------------------------------------------------
function InitializePage(){
    SetEvents();
    FetchFieldsCaption(false,  document.getElementById("txtHCulture").value, document.frmPatientOperation.name);
    //iTimerID = window.setInterval("IsTitlesLoaded();", 1000,"javascript");
    ShowHide_AllDivFields(false);
    SetDivsStatus(OperationsListPageNo);
    FillBoldLists();
}

//----------------------------------------------------------------------------------------------------------------
function IsTitlesLoaded(){
    /** /
    if (document.getElementById("TitleLoaded").value == "1"){
        window.clearInterval(iTimerID);
        ShowHide_AllDivFields(false);
        SetDivsStatus(OperationsListPageNo);
    }
    /**/
    return;
}

//---------------------------------------------------------------------------------------------------------------
function navList_onclick(buttonNo)
{
    intButtonNo = 0;
    AppSchema_ButtonNo = buttonNo;
    document.location.assign(document.getElementById("txtHApplicationURL").value + AppSchemaMenu_href[AppSchema_ButtonNo-1]);
    //btnSave_onclick();
}

//---------------------------------------------------------------------------------------------------------------
function controlBar_Buttons_OnClick(buttonNo)
{
    AppSchema_ButtonNo = 0;
    intButtonNo = buttonNo;
    btnSave_onclick();
    return;
}

//---------------------------------------------------------------------------------------------------------------
function check_OperationBaseline()
{
    var objSurgeryDate = $get("tblPatientTitle_lblSurgeryDate_Value");
    var SurgeryDate = (document.all) ? objSurgeryDate.innerText : objSurgeryDate.textContent;
    
    document.getElementById("chkUpdateWeight").checked = false;
        
    if(SurgeryDate == ""){
        document.getElementById("chkUpdateWeight").checked = true;
        $get("chkUpdateWeight").style.display = "none";
        $get("lblUpdateWeight").style.display = "none";  
    }        
}

//---------------------------------------------------------------------------------------------------------------
function SetEvents()
{
    document.getElementById("AppSchemaMenu_a_Operations").href = "#";
    document.getElementById("txtOperationDate_txtGlobal").onchange = function(){txtOperationDate_onchange();}
    document.forms[0].txtWeight_txtGlobal.onchange = function(){
        if(document.getElementById("txtWeight_txtGlobal").value == "" || document.getElementById("txtWeight_txtGlobal").value == 0)
            document.getElementById("chkUpdateWeight").checked = true;
        document.getElementById("chkUpdateWeight").disabled = document.getElementById("txtWeight_txtGlobal").value == "";
        
        try{
            if (document.getElementById("txtUseImperial").value != '1') 
                document.getElementById("txtHWeight").value = parseFloat(document.getElementById("txtWeight_txtGlobal").value);
            else 
                document.getElementById("txtHWeight").value = parseFloat(document.getElementById("txtWeight_txtGlobal").value) * 0.45359237; 
        }
        catch(e){}
    }
    
    document.getElementById("chkPrevAbdoSurgery").onclick = function(){
        if (!document.getElementById("chkPrevAbdoSurgery").checked){
            document.getElementById("cmbPrevAbdoSurgery1_CodeList").value = 0; 
            document.getElementById("cmbPrevAbdoSurgery2_CodeList").value = 0; 
            document.getElementById("cmbPrevAbdoSurgery3_CodeList").value = 0; 
            document.getElementById("txtPrevAbdoSurgeryNotes_txtGlobal").value = ''; 
        }
        document.getElementById("cmbPrevAbdoSurgery1_CodeList").disabled = !document.getElementById("chkPrevAbdoSurgery").checked; 
        document.getElementById("cmbPrevAbdoSurgery2_CodeList").disabled = !document.getElementById("chkPrevAbdoSurgery").checked; 
        document.getElementById("cmbPrevAbdoSurgery3_CodeList").disabled = !document.getElementById("chkPrevAbdoSurgery").checked; 
        document.getElementById("txtPrevAbdoSurgeryNotes_txtGlobal").disabled = !document.getElementById("chkPrevAbdoSurgery").checked; 
    }
    
    document.getElementById("chkPrevPelvicSurgery").onclick = function(){
        if (!document.getElementById("chkPrevPelvicSurgery").checked)
        {
           document.getElementById("cmbPrevPelvicSurgery1_CodeList").value = 0; 
           document.getElementById("cmbPrevPelvicSurgery2_CodeList").value = 0; 
           document.getElementById("cmbPrevPelvicSurgery3_CodeList").value = 0; 
           document.getElementById("txtPrevPelvicSurgeryNotes_txtGlobal").value = ''; 
        }
        document.getElementById("cmbPrevPelvicSurgery1_CodeList").disabled = !document.getElementById("chkPrevPelvicSurgery").checked; 
        document.getElementById("cmbPrevPelvicSurgery2_CodeList").disabled = !document.getElementById("chkPrevPelvicSurgery").checked; 
        document.getElementById("cmbPrevPelvicSurgery3_CodeList").disabled = !document.getElementById("chkPrevPelvicSurgery").checked; 
        document.getElementById("txtPrevPelvicSurgeryNotes_txtGlobal").disabled = !document.getElementById("chkPrevPelvicSurgery").checked; 
    }
    
    document.getElementById("chkComcomitantSurgery").onclick = function(){
        if (!document.getElementById("chkComcomitantSurgery").checked)
        {
            document.getElementById("cmbComcomitantSurgery1_CodeList").selectedIndex = 0; 
            document.getElementById("cmbComcomitantSurgery2_CodeList").selectedIndex = 0; 
            document.getElementById("cmbComcomitantSurgery3_CodeList").selectedIndex = 0; 
            document.getElementById("txtComcomitantSurgeryNotes_txtGlobal").value = ''; 
        }
        document.getElementById("cmbComcomitantSurgery1_CodeList").disabled = !document.getElementById("chkComcomitantSurgery").checked; 
        document.getElementById("cmbComcomitantSurgery2_CodeList").disabled = !document.getElementById("chkComcomitantSurgery").checked; 
        document.getElementById("cmbComcomitantSurgery3_CodeList").disabled = !document.getElementById("chkComcomitantSurgery").checked; 
        document.getElementById("txtComcomitantSurgeryNotes_txtGlobal").disabled = !document.getElementById("chkComcomitantSurgery").checked;
    }
    
    document.getElementById("cmbSurgeon_DoctorsList").onchange = function(){cmbSurgeon_onchange();}
    
    document.getElementById("cmbSurgeryType_SystemCodeList").onchange = function(){cmbSurgeryType_onchange();    }

    document.getElementById("txtStartHip_txtGlobal").onchange = function(){
        CalculateWHRatio(document.getElementById("txtStartHip_txtGlobal").value, document.getElementById("txtStartWaist_txtGlobal").value);
    }

    document.getElementById("txtStartWaist_txtGlobal").onchange = function(){
        CalculateWHRatio(document.getElementById("txtStartHip_txtGlobal").value, document.getElementById("txtStartWaist_txtGlobal").value);
    }
    SetPatientTitleEvents();
    
    document.getElementById("txtIntraOperativeSearch_txtGlobal").onkeyup = function(){
        FilterListBySearchWord("txtIntraOperativeSearch_txtGlobal", "cmbIntraOperativeEvents_SystemCodeList", "listIntraOperativeEvents");
    }
    
    document.getElementById("txtPreDischargeSearch_txtGlobal").onkeyup = function(){
        FilterListBySearchWord("txtPreDischargeSearch_txtGlobal", "cmbPreDischargeEvents_SystemCodeList", "listPreDischargeEvents");
    }
    
    for (Xh = 0; Xh < AppSchemaMenu.length; Xh++)
        try{document.getElementById(AppSchemaMenu[Xh]).onclick = function() {navList_onclick(this.name.replace("a", ""));};}catch(e){}
    return;
}

//-----------------------------------------------------------------------------------------------------------------
function txtOperationDate_onchange()
{
    var strSOAP = 
            '<?xml version="1.0" encoding="utf-8"?>'+
            '<soap:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" ' + 
                            'xmlns:xsd="http://www.w3.org/2001/XMLSchema" ' + 
                            'xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">'+
	            '<soap:Body>'+
		            '<CheckDate xmlns="http://tempuri.org/">'+
		                '<strDate>' + document.getElementById("txtOperationDate_txtGlobal").value + '</strDate>' +
		            '</CheckDate>'+
	            '</soap:Body>'+
            '</soap:Envelope>';
    SubmitSOAPXmlHttp(strSOAP, txtOperationDate_onchange_callback, document.getElementById("txtHApplicationURL").value + "WebServices/GlobalWebService.asmx", "http://tempuri.org/CheckDate");
}

//-----------------------------------------------------------------------------------------------------------------
function txtOperationDate_onchange_callback(){
    if (XmlHttp.readyState == 4)
        if (XmlHttp.status == 200)
        {
            var response  = XmlHttp.responseXML.documentElement, strResult = "";
            
            if (response.getElementsByTagName("ReturnValue")[0].hasChildNodes())
                strResult = response.getElementsByTagName('ReturnValue')[0].firstChild.data;
            switch(strResult)
            {
                case "E1" :
                    alert("The Operation date is not correct ...");
                    document.getElementById("txtOperationDate_txtGlobal").value = "";
                    document.getElementById("txtOperationDate_txtGlobal").focus();
                    break;
                case "E2" : 
                    alert("The Operation date is greater than current date ...");
                    document.getElementById("txtOperationDate_txtGlobal").value = "";
                    document.getElementById("txtOperationDate_txtGlobal").focus();
                    break;
            }
        }
    return;
}

//---------------------------------------------------------------------------------------------------------------
function SetDivsStatus(buttonNo){
    var idx = 1;

    for(; idx <= 6; idx++) 
        try{
            if (idx != buttonNo) 
                document.getElementById('li_Div' + idx).className = "";
            else
                document.getElementById('li_Div' + idx).className = "current";
        }
        catch(e){}
        
    try{document.getElementById("div_vDetail").style.display = (buttonNo == OperationDetailPageNo ) ? "block" : "none";}catch(e){}
    try{document.getElementById("div_vOtherDetails").style.display = (buttonNo == OperationOtherDetailPageNo ) ? "block" : "none";}catch(e){}
    try{document.getElementById("div_vOperationsList").style.display = (buttonNo ==  OperationsListPageNo ) ? "block" : "none";}catch(e){}

    
    if (AppSchema_ButtonNo == 0) // it means user has clicked the operation buttons not top-level buttons(baseline, visit and etc)
        switch(buttonNo)
        {
            case OperationsListPageNo :
                SetTopLevelHREF(true);
                LoadPatientOperationList();
                break;
            case OperationDetailPageNo :
                SetTopLevelHREF(false);
                check_OperationBaseline();
                break;
        }
    document.getElementById("txtHPageNo").value = buttonNo;
    return;
}

//---------------------------------------------------------------------------------------------------------------
function cmbSurgeon_onchange(){
    var xmlSOAP;
    
    cmbSurgeon = document.getElementById("cmbSurgeon_DoctorsList");
    if (cmbSurgeon.selectedIndex > 0){
        var strSOAP = 
            '<?xml version="1.0" encoding="utf-8"?>'+
            '<soap:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" ' + 
                            'xmlns:xsd="http://www.w3.org/2001/XMLSchema" ' + 
                            'xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">'+
	            '<soap:Body>'+
		            '<CheckSurgeon xmlns="http://tempuri.org/">'+
			            '<SurgeonCode>' + cmbSurgeon.value + '</SurgeonCode>'+
		            '</CheckSurgeon>'+
	            '</soap:Body>'+
            '</soap:Envelope>';
        SubmitSOAPXmlHttp(strSOAP, cmbSurgeon_onchange_CallBack, "Includes/PatientOperationWebService.asmx", "http://tempuri.org/CheckSurgeon");
    }
    return;
}

//---------------------------------------------------------------------------------------------------------------
function cmbSurgeon_onchange_CallBack(){
    if(XmlHttp.readyState == 4) 
        if(XmlHttp.status == 200){
            var xmlSOAP, strReturn;
            
            //document.getElementById("TestText").value = XmlHttp.responseText;
            if (document.all){
                xmlSOAP = new ActiveXObject("MSXML2.DOMDocument");
                xmlSOAP = XmlHttp.responseXML;
                xmlSOAP.loadXML("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?>" + xmlSOAP.getElementsByTagName("ReturnValue")[0].firstChild.nodeValue);
	        }
	        else
	        {
	            xmlSOAP = document.implementation.createDocument("", "", null);
                xmlSOAP = XmlHttp.responseXML;
                var DomParser = new DOMParser();
                xmlSOAP = DomParser.parseFromString("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?>" + xmlSOAP.getElementsByTagName("ReturnValue")[0].firstChild.nodeValue , "text/xml");
	        }
	        
            if (document.getElementById("cmbSurgeryType_SystemCodeList").selectedIndex == 0){
                document.getElementById("cmbSurgeryType_SystemCodeList").value = xmlSOAP.getElementsByTagName("PrefSurgeryType")[0].firstChild.nodeValue;
                UpdateOtherFieldsBasedOnSelectedSurgeryType();
            }
            
            if (document.getElementById("cmbApproach_SystemCodeList").selectedIndex == 0){
                document.getElementById("cmbApproach_SystemCodeList").value = xmlSOAP.getElementsByTagName("PrefApproach")[0].firstChild.nodeValue;
            }

            if (document.getElementById("cmbCategory_CodeList").selectedIndex == 0){
                document.getElementById("cmbCategory_CodeList").value = xmlSOAP.getElementsByTagName("PrefCategory")[0].firstChild.nodeValue;
            }
            
            if (document.getElementById("cmbGroup_CodeList").selectedIndex == 0){
                document.getElementById("cmbGroup_CodeList").value = xmlSOAP.getElementsByTagName("PrefGroup")[0].firstChild.nodeValue;
            }
        }
//        else
//            alert(XmlHttp.responseText);
    return;
}

//-------------------------------------------------------------------------------------------------------------
function cmbSurgeryType_onchange(){
    UpdateOtherFieldsBasedOnSelectedSurgeryType();
}

//-------------------------------------------------------------------------------------------------------------
function UpdateOtherFieldsBasedOnSelectedSurgeryType()
{
    ShowHide_AllDivFields(false);
    switch (document.getElementById("cmbSurgeryType_SystemCodeList").value)
    {
        case OperationGastricBaloon :
            ShowHide_DivFields("div_SerialNo", true);
            document.getElementById("lblSerialNo").style.display = "block";
            document.getElementById("txtSerialNo_txtGlobal").style.display = "block";
            break;
            
            
        case OperationBandingRevision:
        case OperationGastricAdjustable : // formerly 1, 11, 12
            ShowHide_DivFields("div_lblReservoirSite", true);
            document.getElementById("lblReservoirSite").style.display = "block";
            document.getElementById("cmbReservoirSite").style.display = "block";

            ShowHide_DivFields("div_lblBalloonVolume", true);
            document.getElementById("lblBalloonVolume").style.display = "block";
            document.getElementById("txtBalloonVolume_txtGlobal").style.display = "block";

            ShowHide_DivFields("div_lblPathway", true);
            document.getElementById("lblPathway").style.display = "block";
            document.getElementById("cmbPathway").style.display = "block";
            
            //ShowHide_DivFields("div_lblLapBandSerialNumber", true);
            //document.getElementById("lblLapBandSerialNumber").style.display = "block";
            //document.getElementById("cmbLapBandSerialNumber").style.display = "block";
            
            ShowHide_DivFields("div_lblBandSize", true);
            document.getElementById("lblBandSize").style.display = "block";
            document.getElementById("cmbBandSize_SystemCodeList").style.display = "block";
            
            ShowHide_DivFields("div_BandType", true);
            document.getElementById("lblBandType").style.display = "block";
            document.getElementById("ddlBandType_SystemCodeList").style.display = "block";
            
            ShowHide_DivFields("div_SerialNo", true);
            document.getElementById("lblSerialNo").style.display = "block";
            document.getElementById("txtSerialNo_txtGlobal").style.display = "block";
            break;

        case OperationGastricBypassRoux : // formerly 2, 3
            ShowHide_DivFields("div_lblRouxLimbLength", true);
            document.getElementById("lblRouxLimbLength").style.display = "block";
            document.getElementById("txtRouxLimbLength_txtGlobal").style.display = "block";
            
            ShowHide_DivFields("div_lblRouxEnterostomy", true);
            document.getElementById("lblRouxEnterostomy").style.display = "block";
            document.getElementById("cmbRouxEnterostomy_SystemCodeList").style.display = "block";
            
            ShowHide_DivFields("div_lblRouxColic", true);
            document.getElementById("lblRouxColic").style.display = "block";
            document.getElementById("cmbRouxColic").style.display = "block";
            
            ShowHide_DivFields("div_lblRouxGastric", true);
            document.getElementById("lblRouxGastric").style.display = "block";
            document.getElementById("cmbRouxGastric").style.display = "block";
            
            ShowHide_DivFields("div_lblBanded", true);
            document.getElementById("lblBanded").style.display = "block";
            document.getElementById("chkBanded").style.display = "block";
            
            break;


        case OperationBPD :    // formerly 6
            ShowHide_DivFields("div_lblBPDIlealLength", true);
            document.getElementById("lblBPDIlealLength").style.display = "block";
            document.getElementById("txtBPDIlealLength_txtGlobal").style.display = "block";
            
            ShowHide_DivFields("div_lblBPDChannelLength", true);
            document.getElementById("lblBPDChannelLength").style.display = "block";
            document.getElementById("txtBPDChannelLength_txtGlobal").style.display = "block";
            
            ShowHide_DivFields("div_lblBPDDuodenalSwitch", true);
            document.getElementById("lblBPDDuodenalSwitch").style.display = "block";
            document.getElementById("chkBPDDuodenalSwitch").style.display = "block";
            
            ShowHide_DivFields("div_lblBPDStomachSize", true);
            document.getElementById("lblBPDStomachSize").style.display = "block";
            document.getElementById("txtBPDStomachSize_txtGlobal").style.display = "block";
            
            break;
            
            
        case OperationSleeveGastrectomy :
            ShowHide_DivFields("div_lblSleeveBougie", true);
            document.getElementById("lblSleeveBougie").style.display = "block";
            document.getElementById("txtSleeveBougie_txtGlobal").style.display = "block";
            
            
            break;
    }
    return;
}

/** /
//-------------------------------------------------------------------------------------------------------------
function UpdateOtherFieldsBasedOnSelectedSurgeryType()
{
    ShowHide_AllDivFields(false);
    switch (parseInt(document.getElementById("cmbSurgeryType_SystemCodeList").value))
    {
        case 1 :
        case 12 :
            ShowHide_DivFields("div_lblReservoirSite", "div_cmbReservoirSite", true);
            document.getElementById("lblReservoirSite").style.display = "block";
            document.getElementById("cmbReservoirSite").style.display = "block";

            ShowHide_DivFields("div_lblBalloonVolume", "div_txtBalloonVolume", true);
            document.getElementById("lblBalloonVolume").style.display = "block";
            document.getElementById("txtBalloonVolume_txtGlobal").style.display = "block";

            ShowHide_DivFields("div_lblPathway", "div_cmbPathway", true);
            document.getElementById("lblPathway").style.display = "block";
            document.getElementById("cmbPathway").style.display = "block";
            
            ShowHide_DivFields("div_lblLapBandSerialNumber", "div_rblLapBandSerialNumber", true);
            document.getElementById("lblLapBandSerialNumber").style.display = "block";
            document.getElementById("cmbLapBandSerialNumber").style.display = "block";
            
            ShowHide_DivFields("div_lblBandSize", "div_cmbBandSize", true);
            document.getElementById("lblBandSize").style.display = "block";
            document.getElementById("cmbBandSize").style.display = "block";
            break;

        case 11 :
            ShowHide_DivFields("div_lblReservoirSite", "div_cmbReservoirSite", true);
            document.getElementById("lblReservoirSite").style.display = "block";
            document.getElementById("cmbReservoirSite").style.display = "block";
            
            ShowHide_DivFields("div_lblBalloonVolume", "div_txtBalloonVolume", true);
            document.getElementById("lblBalloonVolume").style.display = "block";
            document.getElementById("txtBalloonVolume_txtGlobal").style.display = "block";
            
            ShowHide_DivFields("div_lblPathway", "div_cmbPathway", true);
            document.getElementById("lblPathway").style.display = "block";
            document.getElementById("cmbPathway").style.display = "block";
            
            ShowHide_DivFields("div_lblLapBandSerialNumber", "div_rblLapBandSerialNumber", true);
            document.getElementById("lblLapBandSerialNumber").style.display = "block";
            document.getElementById("cmbLapBandSerialNumber").style.display = "block";
            break;

        case 2 :
        case 3 :
            ShowHide_DivFields("div_lblRouxLimbLength", "div_txtRouxLimbLength", true);
            document.getElementById("lblRouxLimbLength").style.display = "block";
            document.getElementById("txtRouxLimbLength_txtGlobal").style.display = "block";
            
            ShowHide_DivFields("div_lblRouxEnterostomy", "div_cmbRouxEnterostomy", true);
            document.getElementById("lblRouxEnterostomy").style.display = "block";
            document.getElementById("cmbRouxEnterostomy_SystemCodeList").style.display = "block";
            
            ShowHide_DivFields("div_lblRouxColic", "div_cmbRouxColic", true);
            document.getElementById("lblRouxColic").style.display = "block";
            document.getElementById("cmbRouxColic").style.display = "block";
            
            ShowHide_DivFields("div_lblRouxGastric", "div_rblRouxGastric", true);
            document.getElementById("lblRouxGastric").style.display = "block";
            document.getElementById("cmbRouxGastric").style.display = "block";
            
            ShowHide_DivFields("div_lblBanded", "div_chkBanded", true);
            document.getElementById("lblBanded").style.display = "block";
            document.getElementById("chkBanded").style.display = "block";
            
            break;

        case 5 :
            ShowHide_DivFields("div_lblVBGStomaSize", "div_txtVBGStomaSize", true);
            document.getElementById("lblVBGStomaSize").style.display = "block";
            document.getElementById("txtVBGStomaSize_txtGlobal").style.display = "block";
            
            ShowHide_DivFields("div_lblVBGStomaWrap", "div_cmbVBGStomaWrap", true);
            document.getElementById("lblVBGStomaWrap").style.display = "block";
            document.getElementById("cmbVBGStomaWrap_SystemCodeList").style.display = "block";
            
            break;

        case 6 :
            ShowHide_DivFields("div_lblBPDIlealLength", "div_txtBPDIlealLength", true);
            document.getElementById("lblBPDIlealLength").style.display = "block";
            document.getElementById("txtBPDIlealLength_txtGlobal").style.display = "block";
            
            ShowHide_DivFields("div_lblBPDChannelLength", "div_txtBPDChannelLength", true);
            document.getElementById("lblBPDChannelLength").style.display = "block";
            document.getElementById("txtBPDChannelLength_txtGlobal").style.display = "block";
            
            ShowHide_DivFields("div_lblBPDDuodenalSwitch", "div_chkBPDDuodenalSwitch", true);
            document.getElementById("lblBPDDuodenalSwitch").style.display = "block";
            document.getElementById("chkBPDDuodenalSwitch").style.display = "block";
            
            ShowHide_DivFields("div_lblBPDStomachSize", "div_txtBPDStomachSize", true);
            document.getElementById("lblBPDStomachSize").style.display = "block";
            document.getElementById("txtBPDStomachSize_txtGlobal").style.display = "block";
            
            break;

        case 61 : //SLEEVS
            ShowHide_DivFields("div_lblTubeSize", "div_txtTubeSize", true);
            document.getElementById("lblTubeSize").style.display = "block";
            document.getElementById("txtTubeSize_txtGlobal").style.display = "block";
            
            break;

        case 8 : //BALLOON
//            document.getElementById("lblBloodLoss").disabled = true;
//            document.getElementById("txtBloodLoss_txtGlobal").disabled = true;
//            document.getElementById("lblMLS").disabled = true;
            break;
    }
    return;
}
/**/

//-------------------------------------------------------------------------------------------------------------
function ShowHide_AllDivFields(show)
{
    var divFields = new Array   
            ("div_lblPathway",             
             "div_lblBPDDuodenalSwitch",   
             "div_lblReservoirSite",       
             "div_lblBalloonVolume",      
             "div_lblPosteriorFixation",   
             "div_lblLapBandSerialNumber", 
             "div_lblBandSize",            
             "div_lblBanded",              
             "div_lblTubeSize",            
             "div_lblRouxLimbLength",      
             "div_lblRouxEnterostomy",      
             "div_lblRouxColic",           
             "div_lblRouxGastric",         
             "div_lblVBGStomaSize",        
             "div_lblVBGStomaWrap",        
             "div_lblBPDIlealLength",      
             "div_lblBPDChannelLength",    
             "div_lblBPDDuodenalSwitch",   
             "div_lblBPDStomachSize",      
             "div_lblLapBandSerialNumber",  
             "div_lblLGBilealSegment",     
             "div_lblLGBomentalpatch",     
             "div_lblLGBreinforcedsutures",
             "div_lblSleeveBougie",
             "div_BandType",
             "div_SerialNo"
             );

    for ( LenIdx = 0; LenIdx < divFields.length; LenIdx++)
        document.getElementById(divFields[LenIdx]).style.display = show ? "block" : "none";
    return;
}

//-------------------------------------------------------------------------------------------------------------
function ShowHide_DivFields(strLabel, show)
{
    document.getElementById(strLabel).style.display = show ? "block" : "none";
    return;
}

//-------------------------------------------------------------------------------------------------------------
function btnSave_onclick(){
    SaveOperationData();
    return;
}

//-------------------------------------------------------------------------------------------------------------
function SaveOperationData(){
    var strSOAP = "", strAction, flag = CanToSave_Operation();
                    
    switch(parseInt(document.getElementById("txtHPageNo").value)){
        case OperationDetailPageNo :
            if (flag == 0){
                prepareSaveOtherProcedure();
                MakeStringSelectedItems(document.forms[0].listIntraOperativeEvents_Selected);
                MakeStringSelectedItems(document.forms[0].listPreDischargeEvents_Selected);                
                MakeStringSelectedItems(document.forms[0].listConcurrent_Selected);
                
                //clear unrelated fields
                OperationTypeCheckFields();
                
                __doPostBack('linkBtnSave','');
            }
            else{
                if (AppSchema_ButtonNo != 0) // top-level menu bar (PatientList, Baseline, Visit, ..)
                    document.location.assign(document.getElementById("txtHApplicationURL").value + AppSchemaMenu_href[AppSchema_ButtonNo-1]);
                else
                    if (intButtonNo != 0)
                        SetDivsStatus(intButtonNo);
            }
            break;
        case OperationOtherDetailPageNo :
            break;
        default :
            SetDivsStatus(intButtonNo);
            return;
    }
    //if (strSOAP != "") SubmitSOAPXmlHttp(strSOAP, SaveOperationData_CallBack, "Includes/PatientOperationWebService.asmx", strAction);
    return;
}

//-------------------------------------------------------------------------------------------------------------
function CanToSave_Operation(){
    var flag = 2, strErrorMessage = "";
    
    // we should check the value of fields when we are in Operation Detail page
    switch(parseInt(document.getElementById("txtHPageNo").value))
    {
        case OperationDetailPageNo :
            flag = 0;
            document.getElementById("lblOperationDate").style.color = "";
            document.getElementById("lblOperation").style.color = "";
            document.getElementById("lblSurgeon").style.color = "";
            
            if (IsAnyFieldsFilled()){
                if (document.getElementById("txtOperationDate_txtGlobal").value.length == 0){
                    flag = 1;
                    strErrorMessage = "Operation date";
                }
                
                if (document.getElementById("cmbSurgeryType_SystemCodeList").selectedIndex == 0){
                    flag = 1;
                    strErrorMessage += ((strErrorMessage.length == 0) ? "" : ", " ) + "Operation";
                }
                
                if (document.getElementById("txtWeight_txtGlobal").value <= 0){
                    flag = 1;
                    strErrorMessage += ((strErrorMessage.length == 0) ? "" : ", " ) + "Weight";
                }
                
                if (document.getElementById("cmbSurgeon_DoctorsList").selectedIndex == 0){
                    flag = 1;
                    strErrorMessage += ((strErrorMessage.length == 0) ? "" : ", " ) + "Surgeon";
                }
                
                document.getElementById("divErrorMessage").style.display = (flag == 1) ? "block" : "none";
                SetInnerText(document.getElementById("pErrorMessage"), (flag == 1) ? ("Please enter " + strErrorMessage) : "");
            }
            else flag = 2;
            break;
        case OperationOtherDetailPageNo :
            if (parseInt(document.getElementById("txtHAdmitID").value) > 0) // it means an operation has been chosen or added recently
                flag = 0;
            else
                flag = 2;
            break;
    }
    
    return(flag);
}

//-------------------------------------------------------------------------------------------------------------
function IsAnyFieldsFilled(){
    var flag = false;
    
    flag |= (document.getElementById("cmbSurgeon_DoctorsList").selectedIndex > 0);
    flag |= (document.getElementById("txtOperationDate_txtGlobal").value.length > 0);
    flag |= (document.getElementById("txtDuration_txtGlobal").value.length > 0);
    flag |= (document.getElementById("cmbSurgeryType_SystemCodeList").selectedIndex > 0); 
    flag |= (document.getElementById("cmbApproach_SystemCodeList").selectedIndex > 0); 
    flag |= (document.getElementById("cmbCategory_CodeList").selectedIndex > 0); 
    flag |= (document.getElementById("cmbGroup_CodeList").selectedIndex > 0); 
    flag |= (document.getElementById("txtTubeSize_txtGlobal").value.length > 0);
    flag |= (document.getElementById("txtBPDIlealLength_txtGlobal").value.length > 0);
    flag |= (document.getElementById("txtRouxLimbLength_txtGlobal").value.length > 0);
    flag |= (document.getElementById("txtBPDChannelLength_txtGlobal").value.length > 0);
    flag |= (document.getElementById("txtVBGStomaSize_txtGlobal").value.length > 0);
    flag |= (document.getElementById("txtBalloonVolume_txtGlobal").value.length > 0);
    flag |= (document.getElementById("txtBPDStomachSize_txtGlobal").value.length > 0);
    flag |= (document.getElementById("cmbHospital_HospitalsList").selectedIndex > 0); 
    flag |= (document.getElementById("txtLosPostOp_txtGlobal").value.length > 0);
    flag |= (document.getElementById("txtWeight_txtGlobal").value.length > 0);
    //flag |= (document.getElementById("txtGeneralNotes_txtGlobal").value.length > 0);
    return(flag);
}

//----------------------------------------------------------------------------------------------------------------
function LoadPatientOperationList()
{
    ShowDivMessage("Loading Patient's Operations...", false);
    __doPostBack('linkbtnLoadOperationsList','');
}

//----------------------------------------------------------------------------------------------------------------
function LoadOperationData(strAdmitID){
    //ClearFields();
    //controlBar_Buttons_OnClick(1); // Show the Operation Data form
    SetDivsStatus(OperationDetailPageNo);
    document.getElementById("div_vOperationsList").style.display = "none";
    document.getElementById("txtHAdmitID").value = strAdmitID;
    __doPostBack('linkBtnLoad','');
}

//----------------------------------------------------------------------------------------------------------------
function SetTopLevelHREF(setflag){
    for (Xh = 0; Xh < AppSchemaMenu.length; Xh++)
        try{document.getElementById(AppSchemaMenu[Xh]).href = setflag ? document.getElementById("txtHApplicationURL").value + AppSchemaMenu_href[Xh] : "#";}catch(e){}
}
//----------------------------------------------------------------------------------------------------------------
function OperationTypeCheckFields(){
    var SurgeryType = document.getElementById("cmbSurgeryType_SystemCodeList").value;
    
    if(SurgeryType != OperationBPD)
    {
        document.forms[0].txtBPDIlealLength_txtGlobal.value = "";
        document.forms[0].txtBPDChannelLength_txtGlobal.value = "";
        document.forms[0].chkBPDDuodenalSwitch.checked = false;
        document.forms[0].txtBPDStomachSize_txtGlobal.value = "";        
    }
    
    if(SurgeryType != OperationGastricBaloon && SurgeryType != OperationGastricAdjustable && SurgeryType != OperationBandingRevision)    
        document.forms[0].txtSerialNo_txtGlobal.value  = "";
    
    if(SurgeryType != OperationGastricAdjustable && SurgeryType != OperationBandingRevision)
    {
        document.forms[0].cmbReservoirSite.selectedIndex = 0;
        document.forms[0].txtBalloonVolume_txtGlobal.value = "";
        document.forms[0].cmbPathway.selectedIndex = 0;
        document.forms[0].cmbBandSize_SystemCodeList.selectedIndex = 0;
        document.forms[0].ddlBandType_SystemCodeList.selectedIndex = 0;        
    }    
    
    if(SurgeryType != OperationGastricBypassRoux)
    {
        document.forms[0].txtRouxLimbLength_txtGlobal.value = "";
        document.forms[0].cmbRouxEnterostomy_SystemCodeList.value = "";
        document.forms[0].cmbRouxColic.selectedIndex = 0;
        document.forms[0].cmbRouxGastric.selectedIndex = 0;
        document.forms[0].chkBanded.checked = false;    
    }
    
    if(SurgeryType != OperationSleeveGastrectomy)
    {
        document.forms[0].txtSleeveBougie_txtGlobal.value = "";
    }
}
//----------------------------------------------------------------------------------------------------------------
function ClearFields(){
    SetTopLevelHREF(false);
    
    
    document.forms[0].btnSave.disabled = false;
    
    document.forms[0].txtHAdmitID.value = "0";
    document.forms[0].cmbSurgeon_DoctorsList.selectedIndex = 0;
    document.forms[0].txtOperationDate_txtGlobal.value = "";
    document.forms[0].txtDuration_txtGlobal.value = "";
    document.forms[0].cmbSurgeryType_SystemCodeList.selectedIndex = 0;
    document.forms[0].cmbApproach_SystemCodeList.selectedIndex = 0;
    document.forms[0].cmbCategory_CodeList.selectedIndex = 0;
    document.forms[0].cmbGroup_CodeList.selectedIndex = 0;
    //document.getElementById("txtBloodLoss_txtGlobal").value = "0";
    document.forms[0].chkBanded.checked = false;
    document.forms[0].txtTubeSize_txtGlobal.value = "";
    document.forms[0].txtBPDIlealLength_txtGlobal.value = "";
    document.forms[0].cmbVBGStomaWrap_SystemCodeList.selectedIndex = 0;
    document.forms[0].txtRouxLimbLength_txtGlobal.value = "";
    document.forms[0].txtBPDChannelLength_txtGlobal.value = "";
    document.forms[0].txtVBGStomaSize_txtGlobal.value = "";
    document.forms[0].cmbRouxEnterostomy_SystemCodeList.value = "";
    document.forms[0].cmbReservoirSite.selectedIndex = 0;
    document.forms[0].chkBPDDuodenalSwitch.checked = false;
    document.forms[0].cmbRouxColic.selectedIndex = 0;
    document.forms[0].txtBalloonVolume_txtGlobal.value = "";
    document.forms[0].txtBPDStomachSize_txtGlobal.value = "";
    document.forms[0].txtSleeveBougie_txtGlobal.value = "";
    document.forms[0].cmbBandSize_SystemCodeList.selectedIndex = 0;
    document.forms[0].cmbRouxGastric.selectedIndex = 0;
    document.forms[0].cmbPathway.selectedIndex = 0;
    //document.getElementById("txtGeneralNotes_txtGlobal.value = "";
    document.forms[0].cmbHospital_HospitalsList.selectedIndex = 0;
    document.forms[0].txtLosPostOp_txtGlobal.value = "";
    document.forms[0].txtWeight_txtGlobal.value  = "";
    document.forms[0].txtSerialNo_txtGlobal.value  = "";
    
    document.forms[0].cmbAsaClassification_SystemCodeList.selectedIndex = 0;
       
    document.forms[0].txtAfterSurgery_txtGlobal.value  = "";
    document.forms[0].cmbAfterSurgeryMeasurement_SystemCodeList.selectedIndex = 0;
    
    document.forms[0].cmbAdverseSurgeon_DoctorsList.selectedIndex = 0;
    document.forms[0].cmbAdverseSurgery_SystemCodeList.selectedIndex = 0;
    
    
    document.forms[0].cmbRegistryProcedure_SystemCodeList.selectedIndex = 0;
    
    //Page 2
    /** /
    document.getElementById("txtStartNeck_txtGlobal").value = "";
    document.getElementById("txtStartWaist_txtGlobal").value = "";
    document.getElementById("txtStartHip_txtGlobal").value = "";
    document.getElementById("txtWHRatio_txtGlobal").value = "";
    
    document.getElementById("chkPrevAbdoSurgery").checked = false;
    document.getElementById("cmbPrevAbdoSurgery1_CodeList").selectedIndex = 0;
    document.getElementById("cmbPrevAbdoSurgery2_CodeList").selectedIndex = 0;
    document.getElementById("cmbPrevAbdoSurgery3_CodeList").selectedIndex = 0;
    document.getElementById("txtPrevAbdoSurgeryNotes_txtGlobal").value = "";
    document.getElementById("cmbPrevAbdoSurgery1_CodeList").disabled = true;
    document.getElementById("cmbPrevAbdoSurgery2_CodeList").disabled = true;
    document.getElementById("cmbPrevAbdoSurgery3_CodeList").disabled = true;
    document.getElementById("txtPrevAbdoSurgeryNotes_txtGlobal").disabled = true;
    
    document.getElementById("chkPrevPelvicSurgery").checked = false;
    document.getElementById("cmbPrevPelvicSurgery1_CodeList").selectedIndex = 0;
    document.getElementById("cmbPrevPelvicSurgery2_CodeList").selectedIndex = 0;
    document.getElementById("cmbPrevPelvicSurgery3_CodeList").selectedIndex = 0;
    document.getElementById("txtPrevPelvicSurgeryNotes_txtGlobal").value = "";
    document.getElementById("cmbPrevPelvicSurgery1_CodeList").disabled = true;
    document.getElementById("cmbPrevPelvicSurgery2_CodeList").disabled = true;
    document.getElementById("cmbPrevPelvicSurgery3_CodeList").disabled = true;
    document.getElementById("txtPrevPelvicSurgeryNotes_txtGlobal").disabled = true;
    
    document.getElementById("chkComcomitantSurgery").checked = false;
    document.getElementById("cmbComcomitantSurgery1_CodeList").selectedIndex = 0;
    document.getElementById("cmbComcomitantSurgery2_CodeList").selectedIndex = 0;
    document.getElementById("cmbComcomitantSurgery3_CodeList").selectedIndex = 0;
    document.getElementById("txtComcomitantSurgeryNotes_txtGlobal").value = "";
    document.getElementById("cmbComcomitantSurgery1_CodeList").disabled = true;
    document.getElementById("cmbComcomitantSurgery2_CodeList").disabled = true;
    document.getElementById("cmbComcomitantSurgery3_CodeList").disabled = true;
    document.getElementById("txtComcomitantSurgeryNotes_txtGlobal").disabled = true;
    /**/
}

//----------------------------------------------------------------------------------------------------------------
function CalculateWHRatio(StartHip, StartWaist){
    try{
        if (parseInt(StartHip) != 0)
            document.getElementById("txtWHRatio_txtGlobal").value = parseInt((parseInt(StartWaist) / parseInt(StartHip)) * 100) / 100;
    }
    catch(e){
        document.getElementById("txtWHRatio_txtGlobal").value = "";
    }
}

//-----------------------------------------------------------------------------------------------------------------
function ShowOperationNotesDiv( ){ 
    var flag = (document.getElementById('divOperationNotes').style.display == 'none');
    document.getElementById('divOperationNotes').style.display= flag ? 'block' : 'none';
    return;
}

//-----------------------------------------------------------------------------------------------------------------
function OperationRow_onmouseover(objOperationRow){
    tempColor = objOperationRow.style.backgroundColor;
    objOperationRow.style.backgroundColor = "#ccc";
    objOperationRow.style.cursor = "pointer";
    return;
}

//-----------------------------------------------------------------------------------------------------------------
function OperationRow_onmouseout(objOperationRow){
    objOperationRow.style.backgroundColor = tempColor;
    objOperationRow.style.cursor = "";
    return;
}

//-----------------------------------------------------------------------------------------------------------------    
function aCalendar_onclick(obj,inputBoxID){
    var strDateformat = $get("lblDateFormat");
    languageCode = document.forms[0].txtHCulture.value.substr(0, 2);
    var strCmd = "displayCalendar(document.forms[0]."+inputBoxID+"_txtGlobal, document.all ? strDateformat.innerText : strDateformat.textContent, obj)";
    eval(strCmd);
} 

//-----------------------------------------------------------------------------------------------------------------
function aAdmissionCalendar_onclick(obj){
    var objDate;
    var strDateformat;//= document.getElementById("lblDateFormat");
    
        objDate = document.forms[0].txtAdmissionDate_txtGlobal;
        strDateformat = document.getElementById("lblAdmissionDateFormat");
    
    languageCode = document.forms[0].txtHCulture.value.substr(0, 2);
    displayCalendar(document.forms[0].txtAdmissionDate_txtGlobal, document.all ? strDateformat.innerText : strDateformat.textContent, obj)
} 

//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
/*this function is to add/remove the selected item in BOLD lists (Prev Bariatric, non-Bariatric lists and Adverse Event list)*/
function BoldLists_dblclick( listSourceID, listDestID, strAction){
    var listSource = document.getElementById(listSourceID), listDest = document.getElementById(listDestID);
    
    if ((listSource.options.length == 0) || (listSource.selectedIndex == -1)) return;
    
    if (strAction.toLowerCase() == "add"){ // it means we should add the selected item into dest list, when user double-clicks the source list
        if (! IsItemExistsInDestList(listDest, listSource.options[listSource.selectedIndex].value))
            AddItemIntoDestList(listDest, listSource.options[listSource.selectedIndex]);
    }
    else if (strAction.toLowerCase() == "remove")   listSource.options[listSource.selectedIndex] = null;
}

//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
function BoldLButtonLinks_click(listSourceID, listDestID, strAction, LoadAll){
    var listSource = document.getElementById(listSourceID), listDest = document.getElementById(listDestID);
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


//---------------------------------------------------------------------------------------------------------------
/*this function and the next function "FillBoldList" are to fill Prev Bariatric, non-Bariatric and Adverse Events lists*/
function FillBoldLists(){
    FillBoldList(document.forms[0].cmbIntraOperativeEvents_SystemCodeList, document.forms[0].listIntraOperativeEvents);
    FillBoldList(document.forms[0].cmbPreDischargeEvents_SystemCodeList, document.forms[0].listPreDischargeEvents);
    
    FillBoldList(document.forms[0].cmbConcurrent_SystemCodeList, document.forms[0].listConcurrent);
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

//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
/*this function is to filter BOLD lists (Prev Bariatric, non-Bariatric lists and Adverse Event list), when user starts typing in SEARCH text fields*/
function FilterListBySearchWord(txtSearchID, listSourceID, listDestID){
    var idx = 0;
    var txtSearch = document.getElementById(txtSearchID),
        listSource = document.getElementById(listSourceID),
        listDest = document.getElementById(listDestID);
        
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
function AddItemIntoDestList(listDest, sourceOption ){
    var option = document.createElement("option");

    option.value = sourceOption.value;
    option.text = sourceOption.text;
    listDest.options.add(option);
    
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
function linkBtnSave_CallBack(ReturnFlag,ErrorMsg){
    var Idx = 0;
    if (ReturnFlag == true || ReturnFlag == 'delete' || ReturnFlag =='load'){
        if(ReturnFlag && ReturnFlag!= 'load'){
            ShowDivMessage('Data saving is done successfully...', true);
            if(document.getElementById('tblPatientTitle_lblSurgeryDate_Value').value != "")
                window.location.reload();
        }
        if ((ReturnFlag == true || ReturnFlag =="load") && AppSchema_ButtonNo != 0) // top-level menu bar (PatientList, Baseline, Visit, ..)
            document.location.assign(document.getElementById("txtHApplicationURL").value + AppSchemaMenu_href[AppSchema_ButtonNo-1]);
        else{            
            //if(ReturnFlag == 'delete' && document.getElementById('tblPatientTitle_lblSurgeryDate_Value').text == document.forms[0].txtOperationDate_txtGlobal.value && document.getElementById('tblPatientTitle_lblSurgeryType_Value').text == document.forms[0].cmbSurgeryType_SystemCodeList.options[Idx].text)
            if(ReturnFlag == 'delete')
            {       
                    SetInnerText(document.getElementById('tblPatientTitle_lblSurgeryDate_Value'),"");
                    SetInnerText(document.getElementById('tblPatientTitle_lblSurgeryType_Value'),"");
                    SetInnerText(document.getElementById('tblPatientTitle_lblApproach_Value'),"");
                    SetInnerText(document.getElementById('tblPatientTitle_lblCategory_Value'),"");
                    SetInnerText(document.getElementById('tblPatientTitle_lblGroup_Value'),"");
            }
       }
       /*
       if (ReturnFlag == true && document.forms[0].chkUpdateWeight.checked)
       {
            SetInnerText(document.getElementById('tblPatientTitle_lblSurgeryDate_Value'), document.forms[0].txtOperationDate_txtGlobal.value!=""?document.forms[0].txtOperationDate_txtGlobal.value:'');
                
            Idx = document.forms[0].cmbSurgeryType_SystemCodeList.selectedIndex;     
            var newSurgeryType = document.forms[0].cmbSurgeryType_SystemCodeList.options[Idx].text;
            newSurgeryType = newSurgeryType.substring(newSurgeryType.indexOf("-")+2);
            SetInnerText(document.getElementById('tblPatientTitle_lblSurgeryType_Value'), Idx!=""?newSurgeryType:'');
                
            Idx = document.forms[0].cmbApproach_SystemCodeList.selectedIndex;
            SetInnerText(document.getElementById('tblPatientTitle_lblApproach_Value'), Idx!=""?document.forms[0].cmbApproach_SystemCodeList.options[Idx].text:'');
            Idx = document.forms[0].cmbCategory_CodeList.selectedIndex;
            SetInnerText(document.getElementById('tblPatientTitle_lblCategory_Value'), Idx!=""?document.forms[0].cmbCategory_CodeList.options[Idx].text:'');
            
            Idx = document.forms[0].cmbGroup_SystemCodeList.selectedIndex;
            SetInnerText(document.getElementById('tblPatientTitle_lblGroup_Value'), Idx!=""?document.forms[0].cmbGroup_SystemCodeList.options[Idx].text:'');
            
            document.getElementById('tblPatientTitle_cmbGroup_CodeList').value = document.forms[0].cmbGroup_CodeList.value;
            
            SetInnerText(document.getElementById('tblPatientTitle_lblStartWeight_Value'), document.forms[0].txtWeight_txtGlobal.value);            
        }*/
        if (intButtonNo != 0 && ReturnFlag!='delete') SetDivsStatus(intButtonNo);
    }
    else{
        document.getElementById('divErrorMessage').style.display = 'block';        
        document.getElementById("pErrorMessage").innerHTML = "Error is saving operation data..."+ ErrorMsg;
    }
    return;
}

//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
/*this function is to make string of selected items in Select Destunation lists, we send these string to server and save them in database*/
function MakeStringSelectedItems(listDest){
    var txtObject, xh = 0;

    switch(listDest.id){
        case "listIntraOperativeEvents_Selected" :
            txtObject = document.forms[0].txtIntraOperativeEvents_Selected;
            break;
        case "listPreDischargeEvents_Selected" :
            txtObject = document.forms[0].txtPreDischargeEvents_Selected;
            break;
        case "listConcurrent_Selected" :
            txtObject = document.forms[0].txtConcurrent_Selected;
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
/*this is a function to delete an operation*/
function btnDelete_onclick(){
    var answer = confirm ("It will permanently delete this operation. Do you want to proceed?")
    if (answer){
        __doPostBack('linkBtnDelete','');
    }
    return;
}

function prepareSaveOtherProcedure(){
    var detailOtherProcedure = "";
    for (var currOtherProcedure = 1; currOtherProcedure <= $get("totalOtherProcedures").value; currOtherProcedure++){
        if(eval("document.forms['frmPatientOperation'].elements['delOtherProcedure"+currOtherProcedure+"']"))
        {
            delcounter = "document.forms['frmPatientOperation'].elements['delOtherProcedure"+currOtherProcedure+"'].value == 'no'";
            if(eval(delcounter)){
                var otherProcedureName = eval("document.forms['frmPatientOperation'].elements['txtOtherProcedureName"+currOtherProcedure+"'].value");
                var otherProcedureName = otherProcedureName.replace("*"," ");
                var otherProcedureName = otherProcedureName.replace("+"," ");
                if(otherProcedureName!= "")
                {
                    var otherProcedureCode = eval("document.forms['frmPatientOperation'].elements['txtOtherProcedureCode"+currOtherProcedure+"'].value");
                    var otherProcedureCode = otherProcedureCode.replace("*"," ");
                    var otherProcedureCode = otherProcedureCode.replace("+"," ");
                    
                    
                    detailOtherProcedure += otherProcedureName+"*"+otherProcedureCode+"+";
                }
            }
        }
    }
    if(detailOtherProcedure != "")
        detailOtherProcedure = detailOtherProcedure.slice(0,detailOtherProcedure.length -1); 

    $get("txtDetailOtherProcedure").value = detailOtherProcedure;
}

function addOtherProcedure(){
    var ni = document.getElementById('otherProcedureDiv');
    var numi = document.getElementById('totalOtherProcedures');
    var num = (document.getElementById('totalOtherProcedures').value -1 + 2);
    numi.value = num;
    var newdiv = document.createElement('div');
    var divIdName = 'idOtherProcedure'+num+'div';
    newdiv.setAttribute('id',divIdName);

    newdiv.innerHTML = '<input type=hidden name=delOtherProcedure'+num+' value=no>Procedure: <input type=textbox runat="server" name=txtOtherProcedureName'+num+' size="55"> &nbsp; &nbsp; CPT: <input type=textbox runat="server" size="5" name=txtOtherProcedureCode'+num+'><input type=button name=removeOtherProcedure'+num+' value=" - " onclick="javascript:removeOtherProcedure('+num+',\''+divIdName+'\')">';

    ni.appendChild(newdiv);
}

function removeOtherProcedure(dId,divName){
    delcounter = "document.forms['frmPatientOperation'].elements['delOtherProcedure"+dId+"'].value = 'yes'";
    eval(delcounter);
    delstylecounter = "document.getElementById('"+divName+"').style.display='none'";
    eval(delstylecounter);
}

function BuildReport(Report){
    strReportType = Report==1?"OREG1":"OREG2";
    var intAnnual = parseInt(document.getElementById("txtAnnualOreg").value);
    if (intAnnual <= 0 || isNaN(intAnnual))
        intAnnual = 1;
    strURL = '../../../GroupReports/BuildReportPage.aspx?RP='+strReportType+'&RecID='+document.getElementById("txtHAdmitID").value+"&Format=3"+'&Annual='+intAnnual;
    window.open(strURL, null, 'scrollbars=yes,fullscreen=yes,toolbar=no,menubar=no,location=no,resizable=yes'); 
    //strParam = "&Format=" + document.forms[0].cmbReportFormat.value; //this is removed as a request from paul to remove word doc
}