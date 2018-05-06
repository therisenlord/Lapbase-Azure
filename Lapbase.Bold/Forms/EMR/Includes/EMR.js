﻿var ajax_optionDiv = false; // ajax_optionDiv and ajax_optionDiv_iframe are used for floading div
var ajax_optionDiv_iframe = false;
var timer;

var autoSaveFunction = GetCookie("AutoSave");
    
if(autoSaveFunction == "True")
    timer = setTimeout("controlBar_Buttons_OnClick('1')", 120000);
    
var divDemographics = "div_vDemographics";
var divBackground = "div_vBackground";
var divComplaint = "div_vComplaint";
var divComorbidity = "div_vComorbidity";
var divRegistry = "div_vRegistry";
var divPathology = "div_vPathology";
var divMedications = "div_vMedications";
var divAllergies = "div_vAllergies";
var divWeightHistory = "div_vWeightHistory";
var divExamination = "div_vExamination";
var divMeasurement = "div_vMeasurement";
var divSpecialInvestigation = "div_vSpecialInvestigation";
var divManagement = "div_vManagement";
var divReport = "upReport";

var loadInputValue = "";

var subPage = new Array(new Array('Demographic','chkReportDemographic','DEM')
,new Array('Measurement','chkReportMeasurement','MEA')
,new Array('Presenting Complaint','chkReportComplaint','PCO')
,new Array('Comorbidities','chkReportComorbidities','COM')
,new Array('Medications','chkReportMedications','MED')
,new Array('Weight Loss History','chkReportWeightLoss','WLH')
,new Array('Past & Family History','chkReportBackground','BKG')
,new Array('Allergies','chkReportAllergies','ALG')
,new Array('Pathology','chkReportPathology','PAT')
,new Array('Investigations & Referrals','chkReportSpecialInvestigations','SIN')
,new Array('Physical Examination','chkReportExamination','PEX')
,new Array('Management Plan','chkReportManagement','MGM'));
    
var defaultGeneralReview = "Happy with progress and pleased with health improvement; O/E: normal appearance and affect; obese; no acute distress.";
var defaultCardioReview = "No new symptoms; O/E: RRR; HS – dual rhythm; no M/R/G; no SOA; carotid pulses normal; abdominal aorta not enlarged.";
var defaultRespReview = "No new symptoms; O/E: PN – resonant all over; no fremitus or rubs; vesicular BS and no adventitial sounds; equal effort bilaterally.";
var defaultGastroReview = "See “Notes”, O/E: abdomen soft, non-tender; Access port – no inflam or swelling; no HSM; normal bowel sounds; scars well healed.";
var defaultGenitoReview = "No new symptoms; O/E: No renal tenderness; no masses found; NAD.";
var defaultExtrReview = "No new symptoms; physical exam within normal limits";
var defaultNeuroReview = "No new symptoms; O/E: 2nd cranial nerve – stable; 5th nerve function normal; facial nerve function normal; peripheral sensation normal; reflexes normal.";
var defaultMusculoReview = "No new symptoms; O/E: fully mobile; normal gait; normal range of movement; no joint instability; muscle tone normal.";
var defaultSkinReview = "No new symptoms; O/E: no rashes; no leg ulcers; no new lesions; no subcutaneous lesions; breasts – no masses.";
var defaultPsychReview = "No new symptoms; O/E: oriented in time and place, judgement and insight normal, good recent and remote memory, mood and affect appear normal.";
var defaultEndoReview = "No new symptoms; O/E: Gen. appearance normal, thyroid NAD, no tremor, no tachycardia; no tremor; no exopthalmos or ophthalmoplegia.";
var defaultHemaReview = "No new symptoms; O/E: spleen not palpable, no abnormal LN in neck or groin.";
var defaultENTReview = "No new symptoms; O/E: adequate hearing; mouth, tongue, teeth and gums normal; no cervical LN.";
var defaultEyesReview = "No new symptoms; O/E: No change in vision; PERLA; 3rd, 4th and 6Th cranial nerves normal; conjunctiva and lids – NAD.";
var defaultPFSHReview = "No change in past, family or social history. Meds as per listing.";
var defaultMedsReview = "No new symptoms";

var ComorbidityDropDownList = new Array(
        "cmbHypertension", "cmbCongestive", "cmbIschemic", "cmbAngina", 
        "cmbPeripheral", "cmbLower", "cmbDVT", "cmbGlucose", "cmbLipids", 
        "cmbGout", "cmbGred", "cmbCholelithiasis", "cmbLiver", "cmbBackPain", 
        "cmbMusculoskeletal", "cmbFibro", "cmbPsychosocial", "cmbDepression", 
        "cmbConfirmed", "cmbAlcohol", "cmbTobacco", "cmbAbuse", "cmbStressUrinary", 
        "cmbCerebri", "cmbHernia", "cmbFunctional", "cmbSkin", "cmbObstructive", 
        "cmbObesity", "cmbPulmonary", "cmbAsthma", "cmbPolycystic", "cmbMenstrual",
        "cmbRenalInsuff", "cmbRenalFail", "cmbSteroid", "cmbTherapeutic", "cmbPrevPCISurgery",
        "cmbSmokerACS", "cmbOxygenACS", "cmbEmbolismACS", "cmbCopdACS", "cmbCpapACS", 
        "cmbGerdACS", "cmbGallstoneACS", "cmbMusculoDiseaseACS", "cmbActivityLimitedACS", 
        "cmbDailyMedACS", "cmbSurgicalACS", "cmbMobilityACS", "cmbRenalInsuffACS", 
        "cmbRenalFailACS", "cmbUrinaryACS", "cmbMyocardinalACS", "cmbPrevPCIACS", 
        "cmbPrevCardiacACS", "cmbHyperlipidemiaACS", "cmbHypertensionACS", "cmbDVTACS", 
        "cmbVenousACS", "cmbHealthStatusACS", "cmbHealthStatusACS", "cmbDiabetesACS", 
        "cmbSteroidsACS", "cmbAnticogulationACS", "cmbObesityACS", "cmbShoACS", "cmbFatACS"
        );
    
function InitializePage(){
    SetEvents();
    //FillBoldLists();
    var prevsection = getSectionFromURL('section');
    
    if(prevsection == "")
        prevsection = "1";
    
    setDivStatus(prevsection);
    
    if(loadInputValue == "")
        loadInputValue = LoadTempDetails();
        
    FetchFieldsCaption(false,  document.getElementById("txtHCulture").value, document.frmPatientData.name);       
}

//---------------------------------------------------------------------------------------------------------------
function SetEvents()
{
    //hide all tabs, except demographic
    if($get("txtPatientID").value == "0")
    {
        $get("li_Div12").style.display="none";
        $get("li_Div2").style.display="none";
        $get("li_Div3").style.display="none";
        $get("li_Div5").style.display="none";
        $get("li_Div9").style.display="none";
        $get("li_Div11").style.display="none";
        $get("li_Div18").style.display="none";
        $get("menurow2").style.display="none";
    }
    
    $get("txtWeightHistoryGainWeight_txtGlobal").onchange = function(){txtGainWeight_onchange();}

    $get("txtBirthDate_txtGlobal").onchange = function(){txtBirthDate_onchange();}
    $get("txtMeasurementHeight_txtGlobal").onchange = function(){txtHeight_onchange();}
    $get("txtMeasurementWeight_txtGlobal").onchange = function(){txtStartWeight_onchange();}
    $get("txtIdealWeight_txtGlobal").onchange = function(){txtIdealWeight_onchange();}
    $get("txtMeasurementTWeight_txtGlobal").onchange = function(){txtTargetWeight_onchange();}
    
    $get("txtMeasurementWaist_txtGlobal").onchange = function(){calculateWHR();}
    $get("txtMeasurementHip_txtGlobal").onchange = function(){calculateWHR();}
    calculateWHR();
        
    for (var xh=0; xh < ComorbidityDropDownList.length; xh++)
    {    
        var test = ("document.getElementById('"+ComorbidityDropDownList[xh]+"_SystemCodeList').onchange = function(){cmbComorbidity_onchange('"+ComorbidityDropDownList[xh]+"');};");
        eval(test);
        cmbComorbidity_onchange(ComorbidityDropDownList[xh]);
     }           
     
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
}

function cmbComorbidity_onchange(id){
    var visibility;
    
    if(document.getElementById(id+"_SystemCodeList").selectedIndex > 1 )
        visibility = 'block';
    else
        visibility = 'none';
        
    document.getElementById("txt"+id+"_txtGlobal").style.display = visibility;        
}


function setDivStatus(buttonNo){     
    $get("txtHPageNo").value = buttonNo;   
    
    //save it in a particular time interval 5000 = 5 seconds
    clearTimeout(timer);
    
    if(autoSaveFunction == "True")
        timer = setTimeout("controlBar_Buttons_OnClick('"+$get("txtHPageNo").value+"')", 120000);
    
    document.forms[0].txtSelectedPageNo.value = buttonNo;
    var idx = 1;
    for(; idx <= (19); idx++) 
        try{
            if (idx != buttonNo) {
                $get('ub_mnuItem' + idx).style.fontWeight = "normal";
                $get('li_Div' + idx).className = "";
            }
            else{
                $get('ub_mnuItem' + idx).style.fontWeight = "bold";
                $get('li_Div' + idx).className = "current";
            }
        }
        catch(e){}
        
        /*
    //if dataclamp is activated, permission lvl 2 or 3
    //check for created date for this patient
    if(document.getElementById("txtHDataClamp").value.toLowerCase() == "true" && (document.getElementById("txtHPermissionLevel").value == "2t" || document.getElementById("txtHPermissionLevel").value == "3f"))
    {
        var createdDate = document.forms[0].txtHDateCreated.value;        
        
        var currentTime = new Date();
        var currentDate = document.forms[0].txtHCurrentDate.value;

        if(createdDate != currentDate)
            document.forms[0].btnAddVisit.disabled = true;
        else
            document.forms[0].btnAddVisit.disabled = false;
    }*/
    
    
    try{
        $get(divDemographics).style.display = "none";
        $get(divBackground).style.display = "none";
        $get(divComplaint).style.display = "none";
        $get(divComorbidity).style.display = "none";
        $get(divSpecialInvestigation).style.display = "none";
        $get(divPathology).style.display = "none";
        $get(divRegistry).style.display = "none";
        $get(divMedications).style.display = "none";
        $get(divAllergies).style.display = "none";
        $get(divWeightHistory).style.display = "none";
        $get(divMeasurement).style.display = "none";
        $get(divExamination).style.display = "none";
        $get(divManagement).style.display = "none";
        $get(divReport).style.display = "none";
        
        $get("upLinkBtnSave").style.display = "none";
        $get("upDemographics").style.display = "none";
        $get("upBackground").style.display = "none";
        $get("upComplaint").style.display = "none";
        $get("upComorbidity").style.display = "none";
        $get("upRegistry").style.display = "none";
        $get("upSpecialInvestigation").style.display = "none";
        //$get("upPathology").style.display = "none";
        $get("upMedications").style.display = "none";
        $get("upAllergies").style.display = "none";
        $get("upWeightHistory").style.display = "none";
        $get("upMeasurement").style.display = "none";
        $get("upExamination").style.display = "none";
        $get("upManagement").style.display = "none";
        $get("upReport").style.display = "none";    
                
        switch(parseInt(buttonNo))
        {
            case 1:
                $get(divDemographics).style.display = "block";
                $get("upDemographics").style.display = "block";   
                section = divDemographics;
                checkDeceasedPrimaryProcedure();
            break;  
            case 2:
                $get(divBackground).style.display = "block";
                $get("upBackground").style.display = "block"; 
                section = divBackground;    
            break;  
            case 3:
                $get(divComplaint).style.display = "block";
                $get("upComplaint").style.display = "block";  
                section = divComplaint;
            break;  
            case 5:
                $get(divComorbidity).style.display = "block";
                $get("upComorbidity").style.display = "block";
                section = divComorbidity;
            break;   
            case 6:
                $get(divSpecialInvestigation).style.display = "block";
                $get("upSpecialInvestigation").style.display = "block";
                section = divSpecialInvestigation;
            break;   
            case 8:
                $get(divPathology).style.display = "block";  
                //$get("upPathology").style.display = "block";   
                section = divPathology;
            break;   
            case 9:
                $get(divMedications).style.display = "block";  
                $get("upMedications").style.display = "block";  
                section = divMedications;
            break;       
            case 10:
                $get(divAllergies).style.display = "block";    
                $get("upAllergies").style.display = "block";    
                section = divAllergies;
            break;        
            case 11:
                $get(divWeightHistory).style.display = "block";
                $get("upWeightHistory").style.display = "block";
                section = divWeightHistory;
            break; 
            case 12:
                $get(divMeasurement).style.display = "block";  
                $get("upMeasurement").style.display = "block";  
                section = divMeasurement;
            break;
            case 13:
                $get(divExamination).style.display = "block"; 
                $get("upExamination").style.display = "block";  
                section = divExamination; 
            break;
            case 16:
                $get(divManagement).style.display = "block";
                $get("upManagement").style.display = "block";
                section = divManagement;
            break;
            case 17:
                $get(divReport).style.display = "block";
                $get("upReport").style.display = "block";     
                section = divReport;       
            break;
            case 18:
                $get(divRegistry).style.display = "block";
                $get("upRegistry").style.display = "block";
                section = divRegistry;
            break; 
        }
    
    }catch(e){}
    loadInputValue = LoadTempDetails();
}

function controlBar_Buttons_OnClick(buttonNo){    
    var formValid = true;   
    var saved = false;   
    saveInputValue = LoadTempDetails();

    $get("txtSelectedPageNo").value = buttonNo; 
    formValid = prepareSaveDetail();
         
    if(loadInputValue != saveInputValue && formValid)
    {    
        if(document.getElementById("txtHPermissionLevel").value != "1o")
        {
            ShowDivMessage("Processing, please wait...",true);
            saved = true;
        }
        
        switch ($get("txtHPageNo").value)
        {
            case "3" :
            case "6" :          
            case "7" :
            case "10" :
            case "11" :
            case "13" :       
            case "16" :    
            case "17" :  
            case "18" : 
                __doPostBack('linkBtnSave','');
            break;
            case "1" :
                prepareSaveChildren();
                __doPostBack('btnUpload','');
                //__doPostBack('linkBtnSave','');
            break;                  
            
            case "2" :            
                prepareSaveBariatric();
                prepareSaveNonBariatric();
                
                MakeStringSelectedItems(document.forms[0].listPrevBariatricSurgery_Selected);
                MakeStringSelectedItems(document.forms[0].listAdverseEvents_Selected);
                MakeStringSelectedItems(document.forms[0].listPrevNonBariatricSurgery_Selected);
                __doPostBack('linkBtnSave','');
            break;   
                        
            case "5" :
                prepareSaveComorbidities();
                __doPostBack('linkBtnSave','');
            break;   
            
            case "8" :
                __doPostBack('linkBtnSave','');
                
            case "9" :
                prepareSaveMedications();
                __doPostBack('linkBtnSaveMedication','');
            break;    
            
            case "12" :
                if(prepareSaveMeasurement())
                {
                    document.forms[0].txtHBMI.value = $get("txtBMI_txtGlobal").value;       
                    __doPostBack('linkBtnSave','');
                }    
                else
                    formValid = false;            
            break; 
        }
    }
    
    if(formValid && saved == false)
    {
        setDivStatus(buttonNo);        
    }
}

function prepareSaveDetail(){
    var errorMsg = "";    
    $get("divErrorMessage").style.display = "none";
    
    if($get("txtFirstName_txtGlobal").value.trim() == "")
        errorMsg += " Firstname,"
        
    if($get("txtSurName_txtGlobal").value.trim() == "")
        errorMsg += " Surname,"
                    
    var dtBirth = new Date(document.forms[0].txtBirthDate_txtGlobal.value);    
    var now = new Date();
    
    if(dtBirth>now || document.forms[0].txtBirthDate_txtGlobal.value.trim() == "")
        errorMsg += " proper Birthdate,"
    
    if($get("tempPhotoStatus").value == "0")
        errorMsg += " proper photo. Photo must be a .jpg, .jpeg, .bmp or .png file ";
        
    if(errorMsg.length >0){      
        $get("divErrorMessage").style.display = "block";
        SetInnerText($get("pErrorMessage"), "Please enter the"+errorMsg.substring(0, errorMsg.length-1));
        return false;
    }
    
    return true;
}

function prepareSaveChildren(){
    var detailChildren = "";
    for (var currChild = 1; currChild <= $get("totalChildren").value; currChild++){
        if(eval("document.forms['frmPatientData'].elements['del"+currChild+"']"))
        {
            delcounter = "document.forms['frmPatientData'].elements['del"+currChild+"'].value == 'no'";
            if(eval(delcounter)){
                var childAge = eval("document.forms['frmPatientData'].elements['txtChildAge"+currChild+"'].value");
                var childAge = childAge.replace("*"," ");
                var childAge = childAge.replace("+"," ");
                    
                var childGender = eval("document.forms['frmPatientData'].elements['cmbChildGender"+currChild+"'].value");
                var childGender = childGender.replace("*"," ");
                var childGender = childGender.replace("+"," ");
                detailChildren += childAge+"*"+childGender+"+";
            }
        }
    }
    if(detailChildren != "")
        detailChildren = detailChildren.slice(0,detailChildren.length -1); 

    $get("txtDetailChildren").value = detailChildren;
}

function prepareSaveComorbidities(){
    var detailComorbidities = "";
    for (var currComorbidities = 1; currComorbidities <= $get("totalComorbidities").value; currComorbidities++){
        if(eval("document.forms['frmPatientData'].elements['delComorbidities"+currComorbidities+"']"))
        {
            delcounter = "document.forms['frmPatientData'].elements['delComorbidities"+currComorbidities+"'].value == 'no'";
            if(eval(delcounter)){
                var comName = eval("document.forms['frmPatientData'].elements['txtComorbiditiesName"+currComorbidities+"'].value");
                var comName = comName.replace("*"," ");
                var comName = comName.replace("+"," ");
                if(comName!= "")
                {
                    var comNotes = eval("document.forms['frmPatientData'].elements['txtComorbiditiesNotes"+currComorbidities+"'].value");
                    var comNotes = comNotes.replace("*"," ");
                    var comNotes = comNotes.replace("+"," ");
                    detailComorbidities += comName+"*"+comNotes+"+";
                }
            }
        }
    }
    if(detailComorbidities != "")
        detailComorbidities = detailComorbidities.slice(0,detailComorbidities.length -1); 

    $get("txtDetailComorbidities").value = detailComorbidities;
}

function prepareSaveMedications(){
    var detailMedications = "";
    for (var currMedications = 1; currMedications <= $get("totalMedications").value; currMedications++){
        if(eval("document.forms['frmPatientData'].elements['delMedications"+currMedications+"']"))
        {
            delcounter = "document.forms['frmPatientData'].elements['delMedications"+currMedications+"'].value == 'no'";
            if(eval(delcounter)){
                var medName = eval("document.forms['frmPatientData'].elements['txtMedicationsName"+currMedications+"'].value");
                var medName = medName.replace("*"," ");
                var medName = medName.replace("+"," ");
                if(medName!= "")
                {
                    var medDosage = eval("document.forms['frmPatientData'].elements['txtMedicationsDosage"+currMedications+"'].value");
                    var medDosage = medDosage.replace("*"," ");
                    var medDosage = medDosage.replace("+"," ");
                    
                    var medFreq = eval("document.forms['frmPatientData'].elements['txtMedicationsFreq"+currMedications+"'].value");
                    var medFreq = medFreq.replace("*"," ");
                    var medFreq = medFreq.replace("+"," ");
                    
                    detailMedications += medName+"*"+medDosage+"*"+medFreq+"+";
                }
            }
        }
    }
    if(detailMedications != "")
        detailMedications = detailMedications.slice(0,detailMedications.length -1); 

    $get("txtDetailMedications").value = detailMedications;
}


function prepareSaveBariatric(){
    var detailBariatric = "";
    for (var currBariatric = 1; currBariatric <= $get("totalBariatric").value; currBariatric++){
        if(eval("document.forms['frmPatientData'].elements['delBariatric"+currBariatric+"']"))
        {
            delcounter = "document.forms['frmPatientData'].elements['delBariatric"+currBariatric+"'].value == 'no'";
            if(eval(delcounter)){
                var bariatricSurgeries = eval("document.forms['frmPatientData'].elements['txtBariatricSurgeries"+currBariatric+"'].value");
                var bariatricSurgeries = bariatricSurgeries.replace("*"," ");
                var bariatricSurgeries = bariatricSurgeries.replace("+"," ");    
                if(bariatricSurgeries!= "")
                {                    
                    var bariatricYear = eval("document.forms['frmPatientData'].elements['txtBariatricYear"+currBariatric+"'].value");
                    var bariatricYear = bariatricYear.replace("*"," ");
                    var bariatricYear = bariatricYear.replace("+"," ");
                    
                    var bariatricOriginalWeight = eval("document.forms['frmPatientData'].elements['txtBariatricOriginalWeight"+currBariatric+"'].value");
                    var bariatricOriginalWeight = bariatricOriginalWeight.replace("*"," ");
                    var bariatricOriginalWeight = bariatricOriginalWeight.replace("+"," ");
                                        
                    var bariatricOriginalWeightEstimated = eval("document.forms['frmPatientData'].elements['rbOrgWeight_Estimated"+currBariatric+"'].checked");
                                        
                    var bariatricLowestWeight = eval("document.forms['frmPatientData'].elements['txtBariatricLowestWeight"+currBariatric+"'].value");
                    var bariatricLowestWeight = bariatricLowestWeight.replace("*"," ");
                    var bariatricLowestWeight = bariatricLowestWeight.replace("+"," ");
                                        
                    var bariatricLowestWeightEstimated = eval("document.forms['frmPatientData'].elements['rbLowestWeight_Estimated"+currBariatric+"'].checked"); 
                    
                    var bariatricEvents = eval("document.forms['frmPatientData'].elements['txtBariatricEvents"+currBariatric+"'].value");
                    var bariatricEvents = bariatricEvents.replace("*"," ");
                    var bariatricEvents = bariatricEvents.replace("+"," ");
                    
                    detailBariatric += bariatricYear+"*"+bariatricOriginalWeight+"*"+bariatricOriginalWeightEstimated+"*"+bariatricLowestWeight+"*"+bariatricLowestWeightEstimated+"*"+bariatricSurgeries+"*"+bariatricEvents+"+";
                }
            }
        }
    }
    if(detailBariatric != "")
        detailBariatric = detailBariatric.slice(0,detailBariatric.length -1); 
        
    $get("txtDetailBariatric").value = detailBariatric;
}

function prepareSaveNonBariatric(){
    var detailNonBariatric = "";
    for (var currNonBariatric = 1; currNonBariatric <= $get("totalNonBariatric").value; currNonBariatric++){
        if(eval("document.forms['frmPatientData'].elements['delNonBariatric"+currNonBariatric+"']"))
        {
            delcounter = "document.forms['frmPatientData'].elements['delNonBariatric"+currNonBariatric+"'].value == 'no'";
            if(eval(delcounter)){
                var nonBariatricSurgeries = eval("document.forms['frmPatientData'].elements['txtNonBariatric"+currNonBariatric+"'].value");
                var nonBariatricSurgeries = nonBariatricSurgeries.replace("*"," ");
                var nonBariatricSurgeries = nonBariatricSurgeries.replace("+"," ");
                if(nonBariatricSurgeries!= "")
                {
                    detailNonBariatric += nonBariatricSurgeries+"+";
                }
            }
        }
    }
    if(detailNonBariatric != "")
        detailNonBariatric = detailNonBariatric.slice(0,detailNonBariatric.length -1); 

    $get("txtDetailNonBariatric").value = detailNonBariatric;
}

function prepareSaveMeasurement(){
    var tempWeight = 0, tempHeight = 0;
    var errorMsg = "";
    
    $get("divErrorMessage").style.display = "none";

    tempWeight = parseFloat(document.forms[0].txtMeasurementWeight_txtGlobal.value);
    tempHeight = parseFloat(document.forms[0].txtMeasurementHeight_txtGlobal.value);
        
    tempNeck = parseFloat(document.forms[0].txtMeasurementNeck_txtGlobal.value);
    tempWaist = parseFloat(document.forms[0].txtMeasurementWaist_txtGlobal.value);
    tempHip = parseFloat(document.forms[0].txtMeasurementHip_txtGlobal.value);
        
    if ($get("tblPatientTitle_txtUseImperial").value == "1") // Imperial Mode
    {
        tempWeight = tempWeight * 0.45359237;
        tempHeight = tempHeight * 2.54;
            
        tempNeck = tempNeck * 2.54;
        tempWaist = tempWaist * 2.54;
        tempHip = tempHip * 2.54;
    }
    
    if(tempWeight > 600){
        errorMsg += " weight,";
    }
    if(tempHeight > 500){
        errorMsg += " height,";
    }
    if(tempNeck > 500){
        errorMsg += " neck measurement,";
    }
    if(tempWaist > 500){
        errorMsg += " waist measurement,";
    }
    if(tempHip > 500){
        errorMsg += " hip measurement,";
    }
        
    if(errorMsg.length >0){      
        $get("divErrorMessage").style.display = "block";
        SetInnerText($get("pErrorMessage"), "Please enter the proper"+errorMsg.substring(0, errorMsg.length-1));
        return false;
    }      
        
    $get("txtHMeasurementNeck").value = tempNeck;
    $get("txtHMeasurementWaist").value = tempWaist;
    $get("txtHMeasurementHip").value = tempHip;
    return true;
}



function checkTextBox(examReview,type){
    defaultReviewInput = setDefaultReview(examReview);
    
    if(type=="check"){
        if($get("chk"+examReview).checked == false)
            $get("txt"+examReview).value = "";        
        else if($get("chk"+examReview).checked == true){
            $get("txt"+examReview).value = defaultReviewInput;
        }
    }
    else if(type=="input"){
        if($get("txt"+examReview).value.trim() != "")
            $get("chk"+examReview).checked = true;
        else
            $get("chk"+examReview).checked = false;
    }
}

function checkReadOnly(examReview){   
    if($get("chk"+examReview).checked == false)
        $get("txt"+examReview).disabled = true; 
    else if($get("chk"+examReview).checked == true)
        $get("txt"+examReview).disabled = false;
}

function setDefaultReview(examRev){
    var defaultReview ="";
    switch(examRev)       
    {         
        case "DetGeneral":   
            defaultReview = defaultGeneralReview;
            break;                  
        case "DetCardio":      
            defaultReview = defaultCardioReview;
            break;             
        case "DetResp":      
            defaultReview = defaultRespReview;
            break;             
        case "DetGastro":   
            defaultReview = defaultGastroReview;
            break;         
        case "DetGenito":    
            defaultReview = defaultGenitoReview;
            break;              
        case "DetExtr":     
            defaultReview = defaultExtrReview;
            break;             
        case "DetNeuro":    
            defaultReview = defaultNeuroReview;
            break;             
        case "DetMusculo":   
            defaultReview = defaultMusculoReview;
            break;             
        case "DetSkin":        
            defaultReview = defaultSkinReview;
            break;             
        case "DetPsych":       
            defaultReview = defaultPsychReview;
            break;             
        case "DetEndo":      
            defaultReview = defaultEndoReview;
            break;             
        case "DetHema":       
            defaultReview = defaultHemaReview;
            break;             
        case "DetENT":        
            defaultReview = defaultENTReview;
            break;             
        case "DetEyes":      
            defaultReview = defaultEyesReview;
            break;             
        case "DetPFSH":       
            defaultReview = defaultPFSHReview;
            break;             
        case "DetMeds":     
            defaultReview = defaultMedsReview;
            break;            
        
    }
    return defaultReview;

}


function btnExcessWeightBaseline_onclick(){
    var EWV = new Number(), StartWeight = new Number(), IdealWeight = new Number(), TargetWeight = 0;

    if (document.all){
        EWV = parseFloat(document.getElementById("lblMeasurementEWeight_Value").innerText);
        StartWeight = parseFloat($get("txtMeasurementWeight_txtGlobal").value);
        IdealWeight = parseFloat($get("txtIdealWeight_txtGlobal").value);
    }
    else{
        EWV = parseFloat(document.getElementById("lblMeasurementEWeight_Value").textContent);
        StartWeight = parseFloat($get("txtMeasurementWeight_txtGlobal").value);
        IdealWeight = parseFloat($get("txtIdealWeight_txtGlobal").value);
    }
    TargetWeight = ComputeTargetWeight_EWV(EWV, StartWeight, IdealWeight);
    
    if (!isNaN(TargetWeight)){
        document.getElementById("txtMeasurementTWeight_txtGlobal").value = TargetWeight;
        //UpdateGroupTargetWeight();
    }
    else
        document.getElementById("txtMeasurementTWeight_txtGlobal").value = 0;
    
    document.getElementById("tblPatientTitle_txtTargetWeight_txtGlobal").value = $get("txtMeasurementTWeight_txtGlobal").value;
        
    txtTargetWeight_onchange();
        
    return;
}
    
//------------------------------------------------------------------------------------------------------------
function btnBMIBaseline_onclick(){
    var BMI = new Number(), BMIHeight = new Number(), UseImperial  = new Number(), TargetWeight = 0;
    
    if (document.all)
        BMI = parseFloat(document.getElementById("lblMeasurementTBMI").innerText);
    else
        BMI = parseFloat(document.getElementById("lblMeasurementTBMI").textContent);
        
    BMIHeight = parseFloat($get("txtBMIHeight").value);
    UseImperial = parseInt($get("txtUseImperial").value);
    
    TargetWeight = ComputeTargetWeight_BMI(BMI, BMIHeight, UseImperial);
    
    if (!isNaN(TargetWeight)){
        document.getElementById("txtMeasurementTWeight_txtGlobal").value = TargetWeight;
        //UpdateGroupTargetWeight();
    }
    else
        document.getElementById("txtMeasurementTWeight_txtGlobal").value = 0;
    
    document.getElementById("tblPatientTitle_txtTargetWeight_txtGlobal").value = $get("txtMeasurementTWeight_txtGlobal").value;
    
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

//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
/*This function is to reset default value for all comorbidity dropdownlist fields to 'Select...'*/
function btnComorbiditySetDefault_onclick(){
    var xh = 0;
   
    for (; xh < ComorbidityDropDownList.length; xh++)
        if ($get(ComorbidityDropDownList[xh] + "_SystemCodeList").selectedIndex == 0)
            $get(ComorbidityDropDownList[xh] + "_SystemCodeList").selectedIndex = 1;
    return;
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
            if (mainDropDownList.options[Xh].value == txtHiddenObject.value){
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

function SetListFieldValue( strObjectName, strValue, strOriginalList){
    //var cmbReferredDoctorsList = $get("cmbReferredDoctorsList");
    var cmbOriginalList = $get(strOriginalList);
    
    txtObject = $get(strObjectName);  
    for (Xh = 1; Xh < cmbOriginalList.options.length; Xh++)
        if (cmbOriginalList.options[Xh].value == strValue)
            txtObject.value = cmbOriginalList.options[Xh].text;
    return;
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

function addChildren(){
    var ni = document.getElementById('childrenDiv');
    var numi = document.getElementById('totalChildren');
    var num = (document.getElementById('totalChildren').value -1 + 2);
    numi.value = num;
    var newdiv = document.createElement('div');
    var divIdName = 'id'+num+'div';
    newdiv.setAttribute('id',divIdName);

    newdiv.innerHTML = 'Age &nbsp; <input type=hidden name=del'+num+' value=no><input type=textbox runat="server" name=txtChildAge'+num+'> &nbsp; &nbsp; &nbsp; Gender <select runat="server" id=cmbChildGender'+num+'><option value="M" Selected="true">Male</option><option value="F">Female</option></select> &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <input type=button name=remove'+num+' value="-" onclick="javascript:removeChildren('+num+',\''+divIdName+'\')">';

    ni.appendChild(newdiv);
}

function removeChildren(dId,divName){
    delcounter = "document.forms['frmPatientData'].elements['del"+dId+"'].value = 'yes'";
    eval(delcounter);
    delstylecounter = "document.getElementById('"+divName+"').style.display='none'";
    eval(delstylecounter);
}

function addComorbidities(){
    var ni = document.getElementById('otherComorbiditiesDiv');
    var numi = document.getElementById('totalComorbidities');
    var num = (document.getElementById('totalComorbidities').value -1 + 2);
    numi.value = num;
    var newdiv = document.createElement('div');
    var divIdName = 'idCom'+num+'div';
    newdiv.setAttribute('id',divIdName);

    newdiv.innerHTML = '<table><tr><td style="width: 25%" valign="top"><input type=hidden name=delComorbidities'+num+' value=no>Name: <input type=textbox runat="server" name=txtComorbiditiesName'+num+'></td><td valign="top" style="width:5%">Notes:</td><td style="width: 65%" valign="top"><textarea ID="txtComorbiditiesNotes'+num+'" runat="server" rows="2" cols="65"></textarea></td><td width="5%" valign="top"><input type=button name=remove'+num+' value=" - " onclick="javascript:removeComorbidities('+num+',\''+divIdName+'\')"></td></tr><table>';

    ni.appendChild(newdiv);
}

function removeComorbidities(dId,divName){
    delcounter = "document.forms['frmPatientData'].elements['delComorbidities"+dId+"'].value = 'yes'";
    eval(delcounter);
    delstylecounter = "document.getElementById('"+divName+"').style.display='none'";
    eval(delstylecounter);
}

function addMedications(){
    var ni = document.getElementById('medicationDiv');
    var numi = document.getElementById('totalMedications');
    var num = (document.getElementById('totalMedications').value -1 + 2);
    numi.value = num;
    var newdiv = document.createElement('div');
    var divIdName = 'idMed'+num+'div';
    newdiv.setAttribute('id',divIdName);

    newdiv.innerHTML = '<table><tr><td style="width: 45%"><input type=hidden name=delMedications'+num+' value=no><input type=textbox runat="server" name=txtMedicationsName'+num+' size="50"></td><td align="center" style="width:25%"><input type=textbox runat="server" name=txtMedicationsDosage'+num+'></td><td align="center" style="width: 25%"><input type=textbox runat="server" name=txtMedicationsFreq'+num+'></td><td width="5%" align="center"><input type=button name=removeMed'+num+' value=" - " onclick="javascript:removeMedications('+num+',\''+divIdName+'\')"></td></tr><table>';

    ni.appendChild(newdiv);
}

function removeMedications(dId,divName){
    delcounter = "document.forms['frmPatientData'].elements['delMedications"+dId+"'].value = 'yes'";
    eval(delcounter);
    delstylecounter = "document.getElementById('"+divName+"').style.display='none'";
    eval(delstylecounter);
}

function addBariatric(){
    var ni = document.getElementById('bariatricDiv');
    var numi = document.getElementById('totalBariatric');
    var num = (document.getElementById('totalBariatric').value -1 + 2);
    numi.value = num;
    var newdiv = document.createElement('div');
    var divIdName = 'idBariatric'+num+'div';
    newdiv.setAttribute('id',divIdName);

    newdiv.innerHTML = '<table><tr><td style="width: 20%">Year:</td><td style="width: 75%"><input type="textbox" runat="server" name=txtBariatricYear'+num+' size="10"></td><td style="width : 5%">&nbsp;</td></tr><tr><td>Original Weight:</td><td><input type="textbox" runat="server" name=txtBariatricOriginalWeight'+num+' size="10"> &nbsp;'+$get("lblMeasurementWeightUnit").textContent+' &nbsp; &nbsp; <input type="radio" name="rbOrgWeight'+num+'" id="rbOrgWeight_Estimated'+num+'" runat="server" checked />&nbsp; Estimated &nbsp; &nbsp; <input type="radio" name="rbOrgWeight'+num+'" id="rbOrgWeight_Actual'+num+'" runat="server" />&nbsp; Actual</td><td>&nbsp;</td></tr><tr><td>Lowest Weight Acheived:</td><td><input type="textbox" runat="server" name=txtBariatricLowestWeight'+num+' size="10"> &nbsp;'+$get("lblMeasurementWeightUnit").textContent+' &nbsp; &nbsp; <input type="radio" name="rbLowestWeight'+num+'" id="rbLowestWeight_Estimated'+num+'" runat="server" checked />&nbsp; Estimated &nbsp; &nbsp; <input type="radio" name="rbLowestWeight'+num+'" id="rbLowestWeight_Actual'+num+'" runat="server" />&nbsp; Actual</td><td>&nbsp;</td></td><td>&nbsp;</td></tr><tr><td>Previous Bariatric Surgeries:</td><td><input type="textbox" runat="server" name=txtBariatricSurgeries'+num+' size="100"></td><td>&nbsp;</td></tr><tr><td>Adverse Events associated:</td><td><input type="textbox" runat="server" name=txtBariatricEvents'+num+' size="100"></td><td><input type=hidden name=delBariatric'+num+' value=no><input type=button name=removeBariatric'+num+' value=" - " onclick="javascript:removeBariatric('+num+',\''+divIdName+'\')"></td></tr></table>';

    ni.appendChild(newdiv);
}


function removeBariatric(dId,divName){
    delcounter = "document.forms['frmPatientData'].elements['delBariatric"+dId+"'].value = 'yes'";
    eval(delcounter);
    delstylecounter = "document.getElementById('"+divName+"').style.display='none'";
    eval(delstylecounter);
}

function addNonBariatric(){
    var ni = document.getElementById('nonBariatricDiv');
    var numi = document.getElementById('totalNonBariatric');
    var num = (document.getElementById('totalNonBariatric').value -1 + 2);
    numi.value = num;
    var newdiv = document.createElement('div');
    var divIdName = 'idNonBariatric'+num+'div';
    newdiv.setAttribute('id',divIdName);

    newdiv.innerHTML = '<table><tr><td style="width: 90%"><input type=hidden name=delNonBariatric'+num+' value=no><input type=textbox runat="server" name=txtNonBariatric'+num+' size="130"></td><td style="width : 10%"><input type=button name=removeNonBariatric'+num+' value=" - " onclick="javascript:removeNonBariatric('+num+',\''+divIdName+'\')"></td></tr></table>';

    ni.appendChild(newdiv);
}

function removeNonBariatric(dId,divName){
    delcounter = "document.forms['frmPatientData'].elements['delNonBariatric"+dId+"'].value = 'yes'";
    eval(delcounter);
    delstylecounter = "document.getElementById('"+divName+"').style.display='none'";
    eval(delstylecounter);
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

//---------------------------------------------------------------------------------------------------------------
/*
this function and "CalculateOtherWeights" function are to calculate other data by using Height value on Height, Weight and Note sub-page.
*/
function txtHeight_onchange()
{
    var flag = false; Idx = 0; 
    if (document.forms[0].txtUseImperial.value == '1') {
        document.forms[0].txtHHeight.value = document.forms[0].txtMeasurementHeight_txtGlobal.value * 0.0254; 
        document.forms[0].txtBMIHeight.value = document.forms[0].txtMeasurementHeight_txtGlobal.value * 0.0254; 
    }
    else {
        document.forms[0].txtBMIHeight.value = document.forms[0].txtMeasurementHeight_txtGlobal.value / 100; 
        document.forms[0].txtHHeight.value = document.forms[0].txtMeasurementHeight_txtGlobal.value / 100; 
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
    SetInnerText($get("lblMeasurementIWeight"), document.forms[0].txtIdealWeight_txtGlobal.value);
    if (isNaN(parseFloat(document.forms[0].txtMeasurementWeight_txtGlobal.value))) 
        document.forms[0].txtMeasurementWeight_txtGlobal.value = 0; 
    
    document.forms[0].txtMeasurementEWeight_txtGlobal.value = (parseFloat(document.forms[0].txtMeasurementWeight_txtGlobal.value) - parseFloat(document.forms[0].txtIdealWeight_txtGlobal.value)).toFixed(1); 
    document.forms[0].txtMeasurementWeight_txtGlobal.focus();
    return;
}

//---------------------------------------------------------------------------------------------------------------
function txtStartWeight_onchange()
{
    try{
        var StartWeight = $get("txtMeasurementWeight_txtGlobal").value;
        var Height = $get("txtMeasurementHeight_txtGlobal").value;
        
        StartWeight = isNaN(StartWeight) ? 0 : eval(StartWeight);
        Height = isNaN(Height) ? 0 : eval(Height);
        
        var heightValue = $get("txtHHeight").value;
        heightValue = Math.round(heightValue*Math.pow(10,3))/Math.pow(10,3);
        
        $get("txtMeasurementWeight_txtGlobal").value = StartWeight;
        $get("txtMeasurementHeight_txtGlobal").value = Height;
        if (Height != 0){
            if ($get("txtUseImperial").value != '1') {
                $get("txtHStartWeight").value = StartWeight;
                $get("txtStartBMIWeight").value = StartWeight; 
                //$get("txtBMI_txtGlobal").value = (StartWeight / Math.pow(Height / 100, 2)).toFixed(1); 
                
                $get("txtBMI_txtGlobal").value = ($get("txtHStartWeight").value / Math.pow(heightValue, 2)).toFixed(1); 
            }
            else {
                $get("txtHStartWeight").value = StartWeight * 0.45359237; 
                $get("txtStartBMIWeight").value = StartWeight * 0.45359237; 
                //$get("txtBMI_txtGlobal").value = (703 * StartWeight / Math.pow(Height, 2)).toFixed(1); 
                
                $get("txtBMI_txtGlobal").value = ($get("txtHStartWeight").value / Math.pow(heightValue, 2)).toFixed(1); 
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
        var StartWeight = $get("txtMeasurementWeight_txtGlobal").value;
        var IdealWeight = $get("txtIdealWeight_txtGlobal").value;
        
        StartWeight = isNaN(StartWeight) ? 0 : eval(StartWeight);
        IdealWeight = isNaN(IdealWeight) ? 0 : eval(IdealWeight);
        
        $get("txtMeasurementWeight_txtGlobal").value = StartWeight;
        $get("txtIdealWeight_txtGlobal").value = IdealWeight;
        
        if ($get("txtUseImperial").value == '1')
            $get("txtHIdealWeight").value = (IdealWeight * 0.45359237).toFixed(1);
        else 
            $get("txtHIdealWeight").value = IdealWeight;
        
        $get("txtMeasurementEWeight_txtGlobal").value = (StartWeight - IdealWeight).toFixed(1);
    }
    catch(err){}
    return;
}


//---------------------------------------------------------------------------------------------------------------
function txtTargetWeight_onchange(){
    try{    
    
        var TargetWeight = $get("txtMeasurementTWeight_txtGlobal").value;
            
        if ($get("txtUseImperial").value != '1')
            $get("txtHTargetWeight").value = TargetWeight;        
        else 
            $get("txtHTargetWeight").value = (TargetWeight * 0.45359237).toFixed(1);
    }
    catch(err){}
    return;
}

//---------------------------------------------------------------------------------------------------------------
function txtGainWeight_onchange(){
    try{    
    
        var GainWeight = $get("txtWeightHistoryGainWeight_txtGlobal").value;
            
        if ($get("txtUseImperial").value != '1')
            $get("txtHWeightHistoryGainWeight").value = GainWeight;        
        else 
            $get("txtHWeightHistoryGainWeight").value = (GainWeight * 0.45359237).toFixed(1);
    }
    catch(err){}
    return;
}

function calculateWHR(){
    var Waist = parseInt($get("txtMeasurementWaist_txtGlobal").value);
    var Hip = parseInt($get("txtMeasurementHip_txtGlobal").value);

    var WHR = Waist/Hip;
    
    if(Waist > 0 && Hip > 0)
        WHR = WHR.toFixed(1);
    else
        WHR = "";
        
    SetInnerText($get("lblMeasurementWHR"), WHR);
}

//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
function chkPreCert_Other_onclick(chkOtherObj, txtOtherID){
    var txtOther = $get(txtOtherID);
    txtOther.disabled = !chkOtherObj.checked;
    txtOther.value = !chkOtherObj.checked ? "" : txtOther.value;
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
    SetInnerText($get("tblPatientTitle_lblStartWeight_Value"), $get("txtMeasurementWeight_txtGlobal").value);
    SetInnerText($get("tblPatientTitle_lblIdealWeight_Value"), $get("txtIdealWeight_txtGlobal").value);    
    document.getElementById("tblPatientTitle_txtTargetWeight_txtGlobal").value = $get("txtMeasurementTWeight_txtGlobal").value;

    //$get("tblPatientTitle_txtTargetWeight").value = $get("txtTargetWeight_txtGlobal").value;
    return;
}

//----------------------------------------------------------------------------------------------------------------
function BuildReport(PreviewFlag){
    
    //var strParam = CheckParams();
    //if(strParam.length > 0){
    strURL = '../../GroupReports/BuildReportPage.aspx?RP=EMR';
    strParam = "&Format=" + document.forms[0].cmbReportFormat.value; //this is removed as a request from paul to remove word doc
    //strParam = "&Format=3";
    
    strParam += "&Logo=" + document.forms[0].cmbLogo_LogosList.value; 
    strURL+= strParam;
    
    
    
    //AIGB setting
    //if its aigb, and there is logo, must choose
    $get("divErrorMessage").style.display = "none";
    if($get('txtHLogoMandatory').value == "1" && document.forms[0].cmbLogo_LogosList.value == "")
    {
        $get("divErrorMessage").style.display = "block";
        SetInnerText($get("pErrorMessage"), "Please select one of the Logo");
    }
    else
    {
        window.open(strURL, null, 'scrollbars=yes,fullscreen=yes,toolbar=no,menubar=no,location=no,resizable=yes'); 
    }       
    //}
}

function CheckParams(){
    var param = "";
    for(var i = 0; i < subPage.length; i++){
        if($get(subPage[i][1]).checked == true){
            param += subPage[i][2] + "-";
        }        
    }
    
    if(param.length >0)
        param = param.substring(0, param.length-1);
    
    return param;
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


function getSectionFromURL( name )
{
  name = name.replace(/[\[]/,"\\\[").replace(/[\]]/,"\\\]");
  var regexS = "[\\?&]"+name+"=([^&#]*)";
  var regex = new RegExp( regexS );
  var results = regex.exec( window.location.href );
  if( results == null )
    return "";
  else
    return results[1];
}

function checkPhotoStatus(){
    var file = $get("uploadPhoto").value;
    var fileExtension = file.substr(file.lastIndexOf("."));
    if(fileExtension.toLowerCase() == ".jpg" || fileExtension.toLowerCase() == ".jpeg" || fileExtension.toLowerCase() == ".bmp" || fileExtension.toLowerCase() == ".png")
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

//----------------------------------------------------------------------------------------------------------------
function BuildGraph(PreviewFlag){
    if($get("txtHStartWeight").value >0)
    {
        var strParam = $get('chkIncActualLoss').checked ? "1" : "0";;
        
        var strURL = '../../../GroupReports/BuildReportPage.aspx?RP=' + $get('ReportCode').value;
        strURL += "&PID=" + $get("txtPatientID").value + "&Preview="+PreviewFlag + "&Param=" + strParam+ "&Format="+PreviewFlag;
        
        window.open(strURL, null, 'scrollbars=yes,fullscreen=yes,toolbar=no,menubar=no,location=no,resizable=yes');
    }
    else
    {
        alert("You need to record the measurement data first before using this feature");
    }
}

function checkDeceasedPrimaryProcedure(){
    if($get("rbDeceasedPrimaryProcedureY").checked == true){
        $get("rowDeceasedNote").style.display = 'block';
    }else{
        $get("rowDeceasedNote").style.display = 'none';
    }
}