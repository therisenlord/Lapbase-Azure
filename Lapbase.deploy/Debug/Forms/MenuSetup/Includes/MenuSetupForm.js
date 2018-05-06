// JScript File
var old_onblur;
    
//---------------------------------------------------------------------------------------------------------------
function InitialPage(){
    old_onblur = document.getElementById("txtRefSurName_Search_txtGlobal").onblur;
    FetchFieldsCaption(false,  document.getElementById("txtHCulture").value, document.frmMenuSetup.name);
    iTimerID = window.setInterval("IsTitlesLoaded();", 1000);
   
    return;
}

//----------------------------------------------------------------------------------------------------------------
function IsTitlesLoaded(){
    if (document.getElementById("TitleLoaded").value == "1")
    {
            document.getElementById("txtRefSurName_Search_txtGlobal").onblur = function(){
            old_onblur;
            LoadDoctors(true, document.getElementById("txtRefSurName_Search_txtGlobal").value);
        }
        
    var currentTimerRefDr = 0;
            document.getElementById("txtRefSurName_Search_txtGlobal").onkeyup = function(){
            clearTimeout(currentTimerRefDr);
            currentTimerRefDr = setTimeout("LoadDoctors(true, document.getElementById('txtRefSurName_Search_txtGlobal').value);", 600);
        }
    }
    return;
}


//---------------------------------------------------------------------------------------------------------------
function LoadDiv(DivNo)
{
    var div_array = new Array("divDoctor", "divAllDoctors", "divHospitals", "divRefDoctor", "divAllRefDoctors", "divGroups", "divUserFields", "divUserSettings", "divImportPatient", "divCategory", "divDeletedPatient", "divBiochemistry", "divTemplate", "divAllTemplates", "divRegions", "divLogo", "divAllLogos", "divActionLog"); 
    var intTabQty = 18;
    
    document.getElementById("divDeletedPatientNote").style.display = "none";
    ShowErrorMessageDiv("none", "");
    document.forms[0].btnUpdateUserSettings.Disabled = true;
    
    for (Xh = 1; Xh <= intTabQty; Xh++){
        try{document.getElementById("li_Div" + Xh).className = "";}catch(e){}
        try{document.getElementById(div_array[Xh-1]).style.display = "none";}catch(e){}
    }
        
    document.getElementById("li_Div" + DivNo).className = "current";
    for (Xh = 1; Xh <= 9; Xh++){
        document.getElementById("li_SubDiv" + Xh).className = "";
        document.getElementById("li_SubDiv" + Xh).style.display = "none";
    }
    
    switch(DivNo)
    {
        case 1 : // Doctors
            document.getElementById("li_SubDiv1").className = "";
            document.getElementById("li_SubDiv1").style.display = "block";
            document.getElementById("li_SubDiv2").className = "current";
            document.getElementById("li_SubDiv2").style.display = "block";
            
            document.getElementById("divDoctor").style.display = "none";
            document.getElementById("divAllDoctors").style.display = "block";
            LoadDoctors(false, "");
            break;
            
        case 2 : // Hospital
            document.getElementById("divHospitals").style.display = "block";
            ClearHospitalFields();
            LoadHospitals();
            break;
            
        case 3 : // Ref Doctors
            document.getElementById("li_SubDiv3").className = "";
            document.getElementById("li_SubDiv3").style.display = "block";
            document.getElementById("li_SubDiv4").className = "current";
            document.getElementById("li_SubDiv4").style.display = "block";
            
            document.getElementById("divRefDoctor").style.display = "none";
            document.getElementById("divAllRefDoctors").style.display = "block";
            LoadDoctors(true, document.getElementById("txtRefSurName_Search_txtGlobal").value);
            
            break;
            
        case 5 :
            document.getElementById("divGroups").style.display = "block";
            ClearGroupFields();
            LoadGroups();
            break;
            
        case 6 :
            document.getElementById("divUserFields").style.display = "block";
            break;
            
        case 7 :
            document.forms[0].btnUpdateUserSettings.Disabled = false;
            document.getElementById("divUserSettings").style.display = "block";
            break;
            
        case 8 :
            document.getElementById("divImportPatient").style.display = "block";
            break;
                        
        case 9 :
            document.getElementById("divCategory").style.display = "block";
            ClearCategoryFields();
            LoadCategory();
            break;
             
        case 10 :
            document.getElementById("divDeletedPatientNote").style.display = "block";
            document.getElementById("divDeletedPatient").style.display = "block";
            LoadDeletedPatient();
            break;
        case 11 :
            document.getElementById("divBiochemistry").style.display = "block";
            //__doPostBack('linkbtnLoadBiochemistry','');
            break;
        case 12 :
            document.getElementById("li_SubDiv5").className = "";
            document.getElementById("li_SubDiv5").style.display = "block";
            document.getElementById("li_SubDiv6").className = "current";
            document.getElementById("li_SubDiv6").style.display = "block";
            
            document.getElementById("divTemplate").style.display = "none";
            document.getElementById("divAllTemplates").style.display = "block";
            LoadTemplates();
            break;
            
        case 13 :
            document.getElementById("divRegions").style.display = "block";
            ClearRegionFields();
            LoadRegions();
            break;
                    
        case 14 :
            document.getElementById("li_SubDiv8").className = "";
            document.getElementById("li_SubDiv8").style.display = "block";
            document.getElementById("li_SubDiv9").className = "current";
            document.getElementById("li_SubDiv9").style.display = "block";
            
            document.getElementById("divLogo").style.display = "none";
            document.getElementById("divAllLogos").style.display = "block";
            LoadLogos();
            break;
        case 15 :
            document.getElementById("divActionLog").style.display = "block";
            LoadActionLog();
            break;
    }
    return;   
}

//---------------------------------------------------------------------------------------------------------------
function LoadSubDiv(SubDivNo){
    for (Xh = 1; Xh <= 9; Xh++){
        document.getElementById("li_SubDiv" + Xh).className = "";
        document.getElementById("li_SubDiv" + Xh).style.display = "none";
        
        switch(SubDivNo)
        {
            case 1 : 
                ClearDoctorFields();
            case 2 :
                document.getElementById("li_SubDiv1").style.display = "block";
                document.getElementById("li_SubDiv2").style.display = "block";
                if (SubDivNo == 1){
                    document.getElementById("divDoctor").style.display = "block";
                    ClearDoctorFields();
                }
                else document.getElementById("divDoctor").style.display = "none";
                
                if (SubDivNo == 2){
                    document.getElementById("divAllDoctors").style.display = "block";
                    LoadDoctors(false, "");
                }
                else document.getElementById("divAllDoctors").style.display = "none";
                break;
                
            case 3 :
                ClearRefDoctorFields();
            case 4 :
                document.getElementById("li_SubDiv3").style.display = "block";
                document.getElementById("li_SubDiv4").style.display = "block";
                if (SubDivNo == 3){
                    document.getElementById("divRefDoctor").style.display =  "block" ;
                    ClearRefDoctorFields();
                }
                else document.getElementById("divRefDoctor").style.display =  "none";
                
                if (SubDivNo == 4){
                    document.getElementById("divAllRefDoctors").style.display = "block";
                    LoadDoctors(true, document.getElementById("txtRefSurName_Search_txtGlobal").value);
                }
                else document.getElementById("divAllRefDoctors").style.display = "none";
                break;
            case 5 :
                ClearTemplateFields();
            case 6 :
                document.getElementById("li_SubDiv5").style.display = "block";
                document.getElementById("li_SubDiv6").style.display = "block";
                if (SubDivNo == 5){
                    document.getElementById("divTemplate").style.display =  "block" ;
                    ClearTemplateFields();
                }
                else document.getElementById("divTemplate").style.display =  "none";
                
                if (SubDivNo == 6){
                    document.getElementById("divAllTemplates").style.display = "block";
                    LoadTemplates();
                }
                else document.getElementById("divAllTemplates").style.display = "none";
                break;
            case 8 :
                ClearLogoFields();
            case 9 :
                document.getElementById("li_SubDiv8").style.display = "block";
                document.getElementById("li_SubDiv9").style.display = "block";
                if (SubDivNo == 8){
                    document.getElementById("divLogo").style.display =  "block" ;
                    ClearLogoFields();
                }
                else document.getElementById("divLogo").style.display =  "none";
                
                if (SubDivNo == 9){
                    document.getElementById("divAllLogos").style.display = "block";
                    LoadLogos();
                }
                else document.getElementById("divAllLogos").style.display = "none";
                break;
        }
    }
    document.getElementById("li_SubDiv" + SubDivNo).className = "current";
}

function btnAddNewDoctor(){
    ClearDoctorFields();
    document.getElementById("li_SubDiv2").className = "";
    document.getElementById("li_SubDiv1").className = "current";
    document.getElementById("li_SubDiv1").style.display = "block";    
    document.getElementById("divDoctor").style.display = "block";
    document.getElementById("divAllDoctors").style.display = "none";
    document.getElementById("btnAddDoctor").value = "Add new doctor";
}

function btnAddNewRefDoctor(){
    ClearRefDoctorFields();
    document.getElementById("li_SubDiv4").className = "";
    document.getElementById("li_SubDiv3").className = "current";
    document.getElementById("li_SubDiv3").style.display = "block";    
    document.getElementById("divRefDoctor").style.display = "block";
    document.getElementById("divAllRefDoctors").style.display = "none";
    document.getElementById("btnAddRefDoctor").value = "Add new doctor";
}

function btnAddNewTemplate(){
    ClearTemplateFields();
    document.getElementById("li_SubDiv6").className = "";
    document.getElementById("li_SubDiv5").className = "current";
    document.getElementById("li_SubDiv5").style.display = "block";    
    document.getElementById("divTemplate").style.display = "block";
    document.getElementById("divAllTemplates").style.display = "none";
    document.getElementById("btnAddTemplate").value = "Add new template";
}

function btnAddNewLogo(){
    ClearLogoFields();
    document.getElementById("li_SubDiv9").className = "";
    document.getElementById("li_SubDiv8").className = "current";
    document.getElementById("li_SubDiv8").style.display = "block";    
    document.getElementById("divLogo").style.display = "block";
    document.getElementById("divAllLogos").style.display = "none";
    document.getElementById("btnAddLogo").value = "Add new logo";
}

//---------------------------------------------------------------------------------------------------------------
function ClearDoctorFields(){
    document.getElementById("txtHDoctorID").value = "0";
    document.getElementById("txtSurName_txtGlobal").value = "";
    document.getElementById("txtFirstName_txtGlobal").value = "";
    document.getElementById("txtTitle_txtGlobal").value = "";
    document.getElementById("txtDoctorBoldCode_txtGlobal").value = "";
    SetInnerText(document.getElementById("lblDoctorName_Value"), "");
    document.getElementById("txtSpeciality_txtGlobal").value = "";
    document.getElementById("txtDegrees_txtGlobal").value = "";
    document.getElementById("txtAddress1_txtGlobal").value = "";
    document.getElementById("txtAddress2_txtGlobal").value = "";
    document.getElementById("txtSuburb_txtGlobal").value = "";
    document.getElementById("txtState_txtGlobal").value = "";
    document.getElementById("txtPostCode_txtGlobal").value = "";
    document.getElementById("chkUseOwnLetterHead").checked = "";
    document.getElementById("cmbSurgeryType_SystemCodeList").selectedIndex = 0;
    document.getElementById("cmbApproach_SystemCodeList").selectedIndex = 0;
    document.getElementById("cmbCategory_CodeList").selectedIndex = 0;
    document.getElementById("cmbGroup_CodeList").selectedIndex = 0;
    document.getElementById("chkIsHide").checked = false;
    return;
}

//---------------------------------------------------------------------------------------------------------------
function ClearRefDoctorFields(){
    document.getElementById("txtHRefDoctorID").value = "";
    document.getElementById("txtRefSurName_txtGlobal").value = "";
    document.getElementById("txtRefFirstName_txtGlobal").value = "";
    document.getElementById("txtRefTitle_txtGlobal").value = "";
    SetInnerText(document.getElementById("lblRefDoctorName_Value"), "");
    document.getElementById("txtRefPhone_txtGlobal").value = "";
    document.getElementById("txtRefFax_txtGlobal").value = "";
    document.getElementById("txtRefAddress1_txtGlobal").value = "";
    document.getElementById("txtRefAddress2_txtGlobal").value = "";
    document.getElementById("txtRefSuburb_txtGlobal").value = "";
    document.getElementById("txtRefState_txtGlobal").value = "";
    document.getElementById("txtRefPostCode_txtGlobal").value = "";
    document.getElementById("chkUseFirst").checked = false;
    document.getElementById("chkIsRefHide").checked = false;
    return;
}

//---------------------------------------------------------------------------------------------------------------
function ClearTemplateFields(){
    document.getElementById("txtHTemplateID").value = "0";
    document.getElementById("txtTemplateName_txtGlobal").value = "";
    document.getElementById("txtTemplateText_txtGlobal").value = "";
    return;
}


//---------------------------------------------------------------------------------------------------------------
function ClearLogoFields(){
    document.getElementById("txtHLogoID").value = "0";
    document.getElementById("txtLogoName_txtGlobal").value = "";
    return;
}
//---------------------------------------------------------------------------------------------------------------
function LoadDoctors(RefDoctorFlag, Surname){
    var requestURL = document.getElementById("txtHApplicationURL").value + "Forms/MenuSetup/MenuSetupAjaxForm.aspx?QSN=";
    
    if (RefDoctorFlag)
        requestURL += "RefDoctor&SName=" + Surname;
    else
        requestURL += "Doctor";
        
    ShowDivMessage(RefDoctorFlag ? "Loading referring doctors data..." : "Loading doctors data...", false);
    
    XmlHttpSubmit(requestURL,  RefDoctorFlag ? LoadRefDoctors_CallBack : LoadDoctors_CallBack );
    
}

//---------------------------------------------------------------------------------------------------------------
function LoadTemplates(){
    var requestURL = document.getElementById("txtHApplicationURL").value + "Forms/MenuSetup/MenuSetupAjaxForm.aspx?QSN=Template";
        
    ShowDivMessage("Loading templates data...", false);
    
    XmlHttpSubmit(requestURL,  LoadTemplates_CallBack);
    
}

//---------------------------------------------------------------------------------------------------------------
function LoadLogos(){
    var requestURL = document.getElementById("txtHApplicationURL").value + "Forms/MenuSetup/MenuSetupAjaxForm.aspx?QSN=Logo";
        
    ShowDivMessage("Loading logos data...", false);
    
    XmlHttpSubmit(requestURL,  LoadLogos_CallBack);
    
}

//---------------------------------------------------------------------------------------------------------------
function LoadActionLog(){
    var requestURL = document.getElementById("txtHApplicationURL").value + "Forms/MenuSetup/MenuSetupAjaxForm.aspx?QSN=ActionLog";
        
    ShowDivMessage("Loading actionlog data...", false);
    
    XmlHttpSubmit(requestURL,  LoadActionLog_CallBack);
    
}

//---------------------------------------------------------------------------------------------------------------
function LoadDoctors_CallBack(){
    if (XmlHttp.readyState == 4)
        if (XmlHttp.status == 200){
            HideDivMessage();
            document.getElementById("div_tblDoctorsList").innerHTML = String(XmlHttp.responseText);
        }
    return;
}

//---------------------------------------------------------------------------------------------------------------
function LoadRefDoctors_CallBack(){
    if (XmlHttp.readyState == 4)
        if (XmlHttp.status == 200){
            HideDivMessage();
            document.getElementById("div_tblRefDoctorsList").innerHTML = String(XmlHttp.responseText);
        }
    return;
}

//---------------------------------------------------------------------------------------------------------------
function LoadTemplates_CallBack(){
    if (XmlHttp.readyState == 4)
        if (XmlHttp.status == 200){
            HideDivMessage();
            document.getElementById("div_tblTemplatesList").innerHTML = String(XmlHttp.responseText);
        }
    return;
}

//---------------------------------------------------------------------------------------------------------------
function LoadLogos_CallBack(){
    if (XmlHttp.readyState == 4)
        if (XmlHttp.status == 200){
            HideDivMessage();
            document.getElementById("div_tblLogosList").innerHTML = String(XmlHttp.responseText);
        }
    return;
}

//---------------------------------------------------------------------------------------------------------------
function LoadActionLog_CallBack(){
    if (XmlHttp.readyState == 4)
        if (XmlHttp.status == 200){
            HideDivMessage();
            document.getElementById("div_tblActionLogList").innerHTML = String(XmlHttp.responseText);
        }
    return;
}

//---------------------------------------------------------------------------------------------------------------
function tblDoctorXSLT_onclick(intDoctorID)
{
    $get("li_SubDiv1").className = "current";
    $get("li_SubDiv2").className = "";
    $get("divDoctor").style.display = "block";
    $get("divAllDoctors").style.display = "none";

    document.forms[0].txtHDoctorID.value = intDoctorID;
    ShowDivMessage("Loading doctor data...", false);
    __doPostBack('linkBtnDoctorLoad','');
}

//---------------------------------------------------------------------------------------------------------------
function btnAddDoctor_onclick(){
    var ErrMessage = "";
    
    document.getElementById("divErrorMessage").style.display = "none";
    if (document.getElementById("txtSurName_txtGlobal").value.length == 0)
        ErrMessage = "Surname";
        
    if (document.getElementById("txtFirstName_txtGlobal").value.length == 0)
        ErrMessage += (ErrMessage.length == 0 ? "" : ", ") + "Firstname";
        
    if (ErrMessage.length == 0){
        ShowDivMessage("Saving doctor data...");
        __doPostBack('linkBtnDoctorSave','');
    }
    else
        ShowErrorMessageDiv("block", "Please enter " + ErrMessage + " ...");
    return;
}

//---------------------------------------------------------------------------------------------------------------
function btnRefSurName_Search_OnClick(){
    LoadDoctors(true, document.getElementById("txtRefSurName_Search_txtGlobal").value);
}

//---------------------------------------------------------------------------------------------------------------
function tblRefDoctorXSLT_onclick(RefDrId){

    document.getElementById("li_SubDiv3").className = "current";
    document.getElementById("li_SubDiv4").className = "";
    document.getElementById("divRefDoctor").style.display = "block";
    document.getElementById("divAllRefDoctors").style.display = "none";

    document.forms[0].txtHRefDoctorID.value = RefDrId;
    ShowDivMessage("Loading referring doctor data...", false);
    __doPostBack('linkBtnRefDoctorLoad','');
}

//---------------------------------------------------------------------------------------------------------------
function btnAddRefDoctor_onclick(){
    var ErrMessage = "";

    document.getElementById("divErrorMessage").style.display = "none";
    if (document.getElementById("txtRefSurName_txtGlobal").value.length == 0)
        ErrMessage = "surname";
        
    if (document.getElementById("txtRefFirstName_txtGlobal").value.length == 0)
        ErrMessage += (ErrMessage.length == 0 ? "" : ", ") + "Firstname";
        
    if (ErrMessage.length == 0){
        ShowDivMessage("Saving referring doctor data...");
        __doPostBack('linkBtnRefDoctorSave','');
    }
    else
        ShowErrorMessageDiv("block", "Please enter " + ErrMessage + " ...");
    return;
}

//---------------------------------------------------------------------------------------------------------------
function tblTemplateXSLT_onclick(intTemplateID)
{
    $get("li_SubDiv5").className = "current";
    $get("li_SubDiv6").className = "";
    $get("divTemplate").style.display = "block";
    $get("divAllTemplates").style.display = "none";

    document.forms[0].txtHTemplateID.value = intTemplateID;
    ShowDivMessage("Loading template data...", false);
    __doPostBack('linkBtnTemplateLoad','');
}

//---------------------------------------------------------------------------------------------------------------
function tblLogoXSLT_onclick(intLogoID)
{
    $get("li_SubDiv8").className = "current";
    $get("li_SubDiv9").className = "";
    $get("divLogo").style.display = "block";
    $get("divAllLogos").style.display = "none";

    document.forms[0].txtHLogoID.value = intLogoID;
    ShowDivMessage("Loading logo data...", false);
    __doPostBack('linkBtnLogoLoad','');
}

//---------------------------------------------------------------------------------------------------------------
function btnAddTemplate_onclick(){
    var ErrMessage = "";

    document.getElementById("divErrorMessage").style.display = "none";
    if (document.getElementById("txtTemplateName_txtGlobal").value.length == 0)
        ErrMessage = "name";
        
    if (document.getElementById("txtTemplateText_txtGlobal").value.length == 0)
        ErrMessage += (ErrMessage.length == 0 ? "" : ", ") + "text";
        
    if (ErrMessage.length == 0){
        ShowDivMessage("Saving template data...");
        __doPostBack('linkBtnTemplateSave','');
    }
    else
        ShowErrorMessageDiv("block", "Please enter " + ErrMessage + " ...");
    return;
}

//---------------------------------------------------------------------------------------------------------------
function btnAddLogo_onclick(){
    var ErrMessage = "";

    document.getElementById("divErrorMessage").style.display = "none";
    if (document.getElementById("txtLogoName_txtGlobal").value.length == 0)
        ErrMessage = " name,";
        
    if($get("tempPhotoStatus").value == "0")
        ErrMessage += " proper logo. Logo must be a .jpg, .jpeg, .bmp , .gif or .png file ";
        
    if (ErrMessage.length == 0){
        ShowDivMessage("Saving logo data...");
        __doPostBack('btnUpload','');
    }
    else
        ShowErrorMessageDiv("block", "Please enter" + ErrMessage.substring(0, ErrMessage.length-1) + " ...");
    return;
}

//---------------------------------------------------------------------------------------------------------------
function LoadHospitals(){
    var requestURL = document.getElementById("txtHApplicationURL").value + "Forms/MenuSetup/MenuSetupAjaxForm.aspx?QSN=LoadHospital";

    ShowDivMessage("Loading hospital data...", false);
    XmlHttpSubmit(requestURL,  LoadHospital_CallBack);
}

//---------------------------------------------------------------------------------------------------------------
function LoadHospital_CallBack(){
    if (XmlHttp.readyState == 4)
        if (XmlHttp.status == 200){
            document.getElementById("div_tblHospitalList").innerHTML = String(XmlHttp.responseText);
            HideDivMessage();
        }
    return;
}

//---------------------------------------------------------------------------------------------------------------
function ClearHospitalFields(){
    document.getElementById("txtHHospitalID").value = "";
    document.getElementById("txtHospitalID_txtGlobal").value = "";
    document.getElementById("txtHospitalName_txtGlobal").value = "";
    document.getElementById("txtHospitalBoldCode_txtGlobal").value = "";
    document.getElementById("cmbRegion_RegionsList").selectedIndex = 0;
    document.getElementById("btnAddHospital").value = "Add hospital";
}

//---------------------------------------------------------------------------------------------------------------
function tblHospitalXSLT_onclick(HID, HName, HBoldCode, HRegionId){
    document.getElementById("txtHHospitalID").value = HID;
    document.getElementById("txtHospitalID_txtGlobal").value = HID;
    document.getElementById("txtHospitalName_txtGlobal").value = HName;
    document.getElementById("txtHospitalBoldCode_txtGlobal").value = HBoldCode;
    document.getElementById("cmbRegion_RegionsList").value = HRegionId;
    document.getElementById("btnAddHospital").value = "Update hospital";
}

//---------------------------------------------------------------------------------------------------------------
function btnAddHospital_onclick(){
    var strErrorMessage = "";
    
    document.getElementById("divErrorMessage").style.display = "none";
    if (document.getElementById("txtHospitalID_txtGlobal").value.length == 0)   strErrorMessage = "Hospital ID";
    if (document.getElementById("txtHospitalName_txtGlobal").value.length == 0) strErrorMessage += (strErrorMessage.length == 0) ? "Hospital name" : ", Hospital name";
    if (strErrorMessage.length == 0){
        ShowDivMessage("Saving hospital data...");
        var strSOAP = 
            '<?xml version="1.0" encoding="utf-8"?>'+
            '<soap:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" ' + 
                            'xmlns:xsd="http://www.w3.org/2001/XMLSchema" ' + 
                            'xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">'+
                '<soap:Body>'+
	                '<SaveHospitalProc xmlns="http://tempuri.org/">'+
	                    '<strHospitalID_OldValue>' + document.getElementById("txtHHospitalID").value + '</strHospitalID_OldValue>'+
	                    '<strHospitalID>' + document.getElementById("txtHospitalID_txtGlobal").value + '</strHospitalID>'+
		                '<strHospitalName>' + document.getElementById("txtHospitalName_txtGlobal").value + '</strHospitalName>'+
		                '<strHospitalBoldCode>' + document.getElementById("txtHospitalBoldCode_txtGlobal").value + '</strHospitalBoldCode>'+
		                '<strRegionId>' + document.getElementById("cmbRegion_RegionsList").value + '</strRegionId>'+
	                '</SaveHospitalProc>'+
                '</soap:Body>'+
            '</soap:Envelope>';
        SubmitSOAPXmlHttp(strSOAP, btnAddHospital_onclick_CallBack, "MenuSetupWebService.asmx", "http://tempuri.org/SaveHospitalProc");
    }
    else
    {
        document.getElementById("divErrorMessage").style.display = "block";
        SetInnerText(document.getElementById("pErrorMessage"), "Please enter " + strErrorMessage);
    }
}

//---------------------------------------------------------------------------------------------------------------
function btnAddHospital_onclick_CallBack(){
    document.getElementById("divErrorMessage").style.display = "none";
    if (XmlHttp.readyState == 4)
        if (XmlHttp.status == 200){
            var response  = XmlHttp.responseXML.documentElement,
                SaveResult = response.getElementsByTagName('ReturnValue')[0].firstChild.data;
            
            HideDivMessage();
            if (SaveResult == "0"){
                ClearHospitalFields();
                LoadHospitals();
            }
            else{
                document.getElementById("divErrorMessage").style.display = "block";
                SetInnerText(document.getElementById("pErrorMessage"), "Error is saving hospital data...");
            }
        }
        else {
            document.getElementById("divErrorMessage").style.display = "block";
            SetInnerText(document.getElementById("pErrorMessage"), "Error is saving hospital data...");
        }
    return;
}

//---------------------------------------------------------------------------------------------------------------
function LoadRegions(){
    var requestURL = document.getElementById("txtHApplicationURL").value + "Forms/MenuSetup/MenuSetupAjaxForm.aspx?QSN=LoadRegion";

    ShowDivMessage("Loading region data...", false);
    XmlHttpSubmit(requestURL,  LoadRegion_CallBack);
}

//---------------------------------------------------------------------------------------------------------------
function LoadRegion_CallBack(){
    if (XmlHttp.readyState == 4)
        if (XmlHttp.status == 200){
            document.getElementById("div_tblRegionList").innerHTML = String(XmlHttp.responseText);
            HideDivMessage();
        }
    return;
}

//---------------------------------------------------------------------------------------------------------------
function ClearRegionFields(){
    document.getElementById("txtHRegionID").value = "";
    document.getElementById("txtRegionID_txtGlobal").value = "";
    document.getElementById("txtRegionName_txtGlobal").value = "";
    document.getElementById("btnAddRegion").value = "Add region";
}

//---------------------------------------------------------------------------------------------------------------
function tblRegionXSLT_onclick(RID, RName){
    document.getElementById("txtHRegionID").value = RID;
    document.getElementById("txtRegionID_txtGlobal").value = RID;
    document.getElementById("txtRegionName_txtGlobal").value = RName;
    document.getElementById("btnAddRegion").value = "Update region";
}

//---------------------------------------------------------------------------------------------------------------
function btnAddRegion_onclick(){
    var strErrorMessage = "";
    
    document.getElementById("divErrorMessage").style.display = "none";
    if (document.getElementById("txtRegionID_txtGlobal").value.length == 0)   strErrorMessage = "Region ID";
    if (document.getElementById("txtRegionName_txtGlobal").value.length == 0) strErrorMessage += (strErrorMessage.length == 0) ? "Region name" : ", Region name";
    if (strErrorMessage.length == 0){
        ShowDivMessage("Saving region data...");
        var strSOAP = 
            '<?xml version="1.0" encoding="utf-8"?>'+
            '<soap:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" ' + 
                            'xmlns:xsd="http://www.w3.org/2001/XMLSchema" ' + 
                            'xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">'+
                '<soap:Body>'+
	                '<SaveRegionProc xmlns="http://tempuri.org/">'+
	                    '<strRegionID_OldValue>' + document.getElementById("txtHRegionID").value + '</strRegionID_OldValue>'+
	                    '<strRegionID>' + document.getElementById("txtRegionID_txtGlobal").value + '</strRegionID>'+
		                '<strRegionName>' + document.getElementById("txtRegionName_txtGlobal").value + '</strRegionName>'+
	                '</SaveRegionProc>'+
                '</soap:Body>'+
            '</soap:Envelope>';
        SubmitSOAPXmlHttp(strSOAP, btnAddRegion_onclick_CallBack, "MenuSetupWebService.asmx", "http://tempuri.org/SaveRegionProc");
    }
    else
    {
        document.getElementById("divErrorMessage").style.display = "block";
        SetInnerText(document.getElementById("pErrorMessage"), "Please enter " + strErrorMessage);
    }
}

//---------------------------------------------------------------------------------------------------------------
function btnAddRegion_onclick_CallBack(){
    document.getElementById("divErrorMessage").style.display = "none";
    if (XmlHttp.readyState == 4)
        if (XmlHttp.status == 200){
            var response  = XmlHttp.responseXML.documentElement,
                SaveResult = response.getElementsByTagName('ReturnValue')[0].firstChild.data;
            
            HideDivMessage();
            if (SaveResult == "0"){
                ClearRegionFields();
                LoadRegions();
            }
            else{
                document.getElementById("divErrorMessage").style.display = "block";
                SetInnerText(document.getElementById("pErrorMessage"), "Error is saving region data...");
            }
        }
        else {
            document.getElementById("divErrorMessage").style.display = "block";
            SetInnerText(document.getElementById("pErrorMessage"), "Error is saving region data...");
        }
    return;
}
//---------------------------------------------------------------------------------------------------------------
function LoadGroups(){
    var requestURL = document.getElementById("txtHApplicationURL").value + "Forms/MenuSetup/MenuSetupAjaxForm.aspx?QSN=LoadGroup";
    ShowDivMessage("Loading group data...", false);
    XmlHttpSubmit(requestURL,  LoadGroups_CallBack);
}

//---------------------------------------------------------------------------------------------------------------
function LoadGroups_CallBack(){
    if (XmlHttp.readyState == 4)
        if (XmlHttp.status == 200){
            document.getElementById("div_tblGroupsList").innerHTML = String(XmlHttp.responseText);
            HideDivMessage();
        }
}

//---------------------------------------------------------------------------------------------------------------
function tblGroupXSLT_onclick(strCode, strDescription){
    document.getElementById("txtHGroupCode").value = strCode;
    document.getElementById("txtCode_Group_txtGlobal").value = strCode;
    document.getElementById("txtDescription_Group_txtGlobal").value = strDescription;
    document.getElementById("btnAddGroup").value = "Update group";
    return;
}

//---------------------------------------------------------------------------------------------------------------
function ClearGroupFields(){
    document.getElementById("txtHGroupCode").value = "";
    document.getElementById("txtCode_Group_txtGlobal").value = "";
    document.getElementById("txtDescription_Group_txtGlobal").value = "";
    document.getElementById("btnAddGroup").value = "Add new group";
    return;
}

//---------------------------------------------------------------------------------------------------------------
function btnAddGroup_onclick()
{
    var ErrMessage = "";
    
    document.getElementById("divErrorMessage").style.display = "none";
    if (document.getElementById("txtCode_Group_txtGlobal").value.length == 0)
        ErrMessage = "Group Code";
        
    if (document.getElementById("txtDescription_Group_txtGlobal").value.length == 0)
        ErrMessage = (ErrMessage.length == 0 ? "" : ", ") + "Description";
    
    if (ErrMessage.length == 0)
    {
        ShowDivMessage("Saving group data...");
        var strSOAP = 
            '<?xml version="1.0" encoding="utf-8"?>'+
            '<soap:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" ' + 
                            'xmlns:xsd="http://www.w3.org/2001/XMLSchema" ' + 
                            'xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">'+
	            '<soap:Body>'+
		            '<SaveGroupProc xmlns="http://tempuri.org/">'+
		                '<strOldCode>' + document.getElementById("txtHGroupCode").value + '</strOldCode>'+
		                '<strCode>' + document.getElementById("txtCode_Group_txtGlobal").value + '</strCode>'+
			            '<strDescription>' + document.getElementById("txtDescription_Group_txtGlobal").value + '</strDescription>'+
		            '</SaveGroupProc>'+
	            '</soap:Body>'+
            '</soap:Envelope>';
	    SubmitSOAPXmlHttp(strSOAP, btnAddGroup_onclick_CallBack, "MenuSetupWebService.asmx", "http://tempuri.org/SaveGroupProc");
    }
    else{
        document.getElementById("divErrorMessage").style.display = "block";
        SetInnerText(document.getElementById("pErrorMessage"), "Please enter " + ErrMessage + " ...");
    }
}

//---------------------------------------------------------------------------------------------------------------
function btnAddGroup_onclick_CallBack(){
    document.getElementById("divErrorMessage").style.display = "none";
    if (XmlHttp.readyState == 4)
        if (XmlHttp.status == 200){
            
            var SaveResult = XmlHttp.responseXML.documentElement.getElementsByTagName('ReturnValue')[0].firstChild.data;
            
            HideDivMessage();
            if (SaveResult == "0"){
                ClearGroupFields();
                LoadGroups();
            }
            else{
                document.getElementById("divErrorMessage").style.display = "block";
                SetInnerText(document.getElementById("pErrorMessage"), "Error is saving group data...");
            }
        }
    return;
}

//---------------------------------------------------------------------------------------------------------------
function LoadCategory(){
    var requestURL = document.getElementById("txtHApplicationURL").value + "Forms/MenuSetup/MenuSetupAjaxForm.aspx?QSN=LoadCategory";
    ShowDivMessage("Loading category data...", false);
    XmlHttpSubmit(requestURL,  LoadCategory_CallBack);
}

//---------------------------------------------------------------------------------------------------------------
function LoadCategory_CallBack(){
    if (XmlHttp.readyState == 4)
        if (XmlHttp.status == 200){
            document.getElementById("div_tblCategoryList").innerHTML = String(XmlHttp.responseText);
            HideDivMessage();
        }
}

//---------------------------------------------------------------------------------------------------------------
function LoadDeletedPatient(){
    var requestURL = document.getElementById("txtHApplicationURL").value + "Forms/MenuSetup/MenuSetupAjaxForm.aspx?QSN=LoadDeletedPatient";
    ShowDivMessage("Loading deleted patient list...", false);
    XmlHttpSubmit(requestURL,  LoadDeletedPatient_CallBack);
}

//---------------------------------------------------------------------------------------------------------------
function LoadDeletedPatient_CallBack(){
    if (XmlHttp.readyState == 4)
        if (XmlHttp.status == 200){
            document.getElementById("div_tblDeletedPatientList").innerHTML = String(XmlHttp.responseText);
            HideDivMessage();
        }
}

//---------------------------------------------------------------------------------------------------------------
function ActivatePatient_onclick(patientID){
    document.forms[0].txtHPatientID.value = patientID;
    ShowDivMessage("Activating patient...");
    __doPostBack('linkBtnActivatePatient','');    
}

//---------------------------------------------------------------------------------------------------------------
function DeleteLogo_onclick(logoID){
    document.forms[0].txtHLogoID.value = logoID;
    ShowDivMessage("Deleting Logo...");
    __doPostBack('linkBtnDeleteLogo','');    
}


//---------------------------------------------------------------------------------------------------------------
function tblCategoryXSLT_onclick(strCode, strDescription){
    document.getElementById("txtHCategoryCode").value = strCode;
    document.getElementById("txtCode_Category_txtGlobal").value = strCode;
    document.getElementById("txtDescription_Category_txtGlobal").value = strDescription;
    document.getElementById("btnAddCategory").value = "Update category";
    return;
}

//---------------------------------------------------------------------------------------------------------------
function ClearCategoryFields(){
    document.getElementById("txtHCategoryCode").value = "";
    document.getElementById("txtCode_Category_txtGlobal").value = "";
    document.getElementById("txtDescription_Category_txtGlobal").value = "";
    document.getElementById("btnAddCategory").value = "Add new category";
    return;
}

//---------------------------------------------------------------------------------------------------------------
function btnAddCategory_onclick()
{
    var ErrMessage = "";
    
    document.getElementById("divErrorMessage").style.display = "none";
    if (document.getElementById("txtCode_Category_txtGlobal").value.length == 0)
        ErrMessage = "Category Code";
        
    if (document.getElementById("txtDescription_Category_txtGlobal").value.length == 0)
        ErrMessage = (ErrMessage.length == 0 ? "" : ", ") + "Description";
    
    if (ErrMessage.length == 0)
    {
        ShowDivMessage("Saving category data...");
        var strSOAP = 
            '<?xml version="1.0" encoding="utf-8"?>'+
            '<soap:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" ' + 
                            'xmlns:xsd="http://www.w3.org/2001/XMLSchema" ' + 
                            'xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">'+
	            '<soap:Body>'+
		            '<SaveCategoryProc xmlns="http://tempuri.org/">'+
		                '<strOldCode>' + document.getElementById("txtHCategoryCode").value + '</strOldCode>'+
		                '<strCode>' + document.getElementById("txtCode_Category_txtGlobal").value + '</strCode>'+
			            '<strDescription>' + document.getElementById("txtDescription_Category_txtGlobal").value + '</strDescription>'+
		            '</SaveCategoryProc>'+
	            '</soap:Body>'+
            '</soap:Envelope>';
	    SubmitSOAPXmlHttp(strSOAP, btnAddCategory_onclick_CallBack, "MenuSetupWebService.asmx", "http://tempuri.org/SaveCategoryProc");
    }
    else{
        document.getElementById("divErrorMessage").style.display = "block";
        SetInnerText(document.getElementById("pErrorMessage"), "Please enter " + ErrMessage + " ...");
    }
}

//---------------------------------------------------------------------------------------------------------------
function btnAddCategory_onclick_CallBack(){
    document.getElementById("divErrorMessage").style.display = "none";
    if (XmlHttp.readyState == 4)
        if (XmlHttp.status == 200){
            
            var SaveResult = XmlHttp.responseXML.documentElement.getElementsByTagName('ReturnValue')[0].firstChild.data;
            
            HideDivMessage();
            if (SaveResult == "0"){
                ClearCategoryFields();
                LoadCategory();
            }
            else{
                document.getElementById("divErrorMessage").style.display = "block";
                SetInnerText(document.getElementById("pErrorMessage"), "Error is saving category data...");
            }
        }
    return;
}

//---------------------------------------------------------------------------------------------------------------
function btnAddUserFields_onclick(){
    ShowDivMessage("Saving user's data...");
    __doPostBack('linkBtnUserFieldSave','');
}

//---------------------------------------------------------------------------------------------------------------
function btnUpdateUserSettings_OnClick(){
    __doPostBack('linkbtnUpdateUserSettings','');
}

//---------------------------------------------------------------------------------------------------------------
function btnAddSaveBiochemistry_onclick(){
    ShowDivMessage("Saving biochemistry data...");
    var tempBiochemistryValue = "";
    var tempValue = "";
    
    //construct BiochemistryValue    
    biochemList = document.getElementById("divBiochemistry").getElementsByTagName("input");
    for(i=0; i < biochemList.length; i++)
    {
        tempValue = biochemList[i].checked;
        
        if(tempValue == true)
            tempBiochemistryValue += biochemList[i].id + "_";
    }
    
    document.forms[0].txtHPatientBiochemistryValue.value = tempBiochemistryValue;
    
    __doPostBack('linkBtnBiochemistrySave','');
}

//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
function linkBtnSave_CallBack(ReturnFlag){
    var Idx = 0;
    if (ReturnFlag == true)
        ShowDivMessage('Data saving is done successfully...', true);
    else{
        ShowDivMessage('Error is saving data...', true);
    }
    return;
}


function checkPhotoStatus(){
    var file = $get("uploadPhoto").value;
    var fileExtension = file.substr(file.lastIndexOf("."));
    if(fileExtension.toLowerCase() == ".jpg" || fileExtension.toLowerCase() == ".jpeg" || fileExtension.toLowerCase() == ".bmp" || fileExtension.toLowerCase() == ".png" || fileExtension.toLowerCase() == ".gif")
        $get("tempPhotoStatus").value = "1";
    else
        $get("tempPhotoStatus").value = "0";
} 


function changeFileInputField(tagId) {
    $get("tempPhotoStatus").value = "1";
    document.getElementById(tagId).innerHTML = document.getElementById(tagId).innerHTML;
    $get("lblPhoto").style.display = 'block';
    $get("uploadPhoto").style.display = 'block';
    $get("clearFileUpload").style.display = 'block';
    $get("cancelFileUpload").style.display = 'block';
    $get("divPhoto").style.display = 'none';
    $get("changeFileUpload").style.display = 'none';
    
    $get("tempPhotoDisplay").value = 'none';
}

function clearFileInputField(tagId) {
    $get("tempPhotoStatus").value = "1";
    document.getElementById(tagId).innerHTML = document.getElementById(tagId).innerHTML;
}

function cancelFileInputField(tagId) {
    clearFileInputField(tagId);
    $get("lblPhoto").style.display = 'none';
    $get("uploadPhoto").style.display = 'none';
    $get("clearFileUpload").style.display = 'none';
    $get("cancelFileUpload").style.display = 'none';
    $get("divPhoto").style.display = 'block';
    $get("changeFileUpload").style.display = 'block';
    
    $get("tempPhotoDisplay").value = 'display';
}