﻿var WhereFlag, LoadingTimer;
var VisitRowColor;
var floodDiv = false;
var floodDiv_iframe = false;
var IsVisitSelected = false;
var oInterval = "";
var SelectedVisit = 1;

var defaultReviewInput = "";
var reviewIndex;
var visitReview = new Array("General","Cardio","Resp","Gastro","Genito","Extr","Neuro","Musculo","Skin","Psych","Endo","Hema","ENT","Eyes","PFSH","Meds");

var defaultGeneralReview = "Happy with progress and pleased with health improvement; O/E: normal appearance and affect; obese; no acute distress.";
var defaultCardioReview = "No new symptoms; O/E: RRR; HS – dual rhythm; no M/R/G; no SOA; carotid pulses normal; abdominal aorta not enlarged.";
var defaultRespReview = "No new symptoms; O/E: PN – resonant all over; no fremitus or rubs; vesicular BS and no adventitial sounds; equal effort bilaterally.";
var defaultGastroReview = "See “Notes”, O/E: abdomen soft, non-tender; Access port – no inflam or swelling; no HSM; normal bowel sounds; scars well healed.";
var defaultGenitoReview = "No new symptoms; O/E: No renal tenderness; no masses in loins; genitalia – NAD.";
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

var detStartWeight;
var detIdealWeight;
var detHeight;
var detFirstVisitDate;
var detImperialFlag;

var monthGroup = 0;

var registryDone = "";
//---------------------------------------------------------------------------------------------------------------
function InitializePage(){
    //get client date
    document.getElementById("txtHCurrentClientDate").value = GetClientDate(GetCookie("CultureInfo"));
    
    if($get("chkSuper").checked == false){
        $get("chkFollowUpDetails").checked = true;
        $get("ReportCode").value = "FUA";
        $get("chkLetterToDoctor").checked = false;
        $get("chkGraphs").checked = false;
        $get("chkIncludeProgressNotes").checked = true;
        $get("chkIncludePatientNote").checked = true;    
        $get("chkSuper").checked = false;
    }
    
    $get("AppSchemaMenu_a_Visits").href = "#";
    ProgressNotes_ClearFields();
    CheckVisitWeightCalculation();
    CleanVisitDetailHeader(0);
    
    if(GetCookie("SubmitData").indexOf("submitbold") >-1)
    VisitPagesClick(2);
        
    SetEvents();
    $get("btnDownloadPdf").style.visibility = 'hidden';
    $get("btnDownloadExcel").style.visibility = 'hidden';
    registryDone = document.getElementById("txtHRegistryDone").value;
    FetchFieldsCaption(false,  document.getElementById("txtHCulture").value, document.frmConsultFU1.name);
}

//---------------------------------------------------------------------------------------------------------------
function SetEvents(){

    CheckAgeToShow();
    document.forms[0].txtMonthWeek_PN_txtGlobal.onchange = function(){ComputeNextVisitDate();}
    document.forms[0].txtPatientNotes_txtGlobal.onchange = function(){txtPatientNotes_onchange();}
    document.forms[0].txtWeight_PN_txtGlobal.onchange = function(){CheckVisitWeightCalculation();}
    document.forms[0].txtDateSeen_PN_txtGlobal.onchange = function(){CheckVisitWeightCalculation();checkRegistryDiabetes();checkRegistrySE();}
    
    document.getElementById("txtMedicationSearch_txtGlobal").onkeyup = function(){
        FilterListBySearchWord("txtMedicationSearch_txtGlobal", "cmbMedication_SystemCodeList", "listMedication");
    }
}

//---------------------------------------------------------------------------------------------------------------
// if the age of patient is more than 18, the HIEGHT field should be hidden
function CheckAgeToShow(){
    var objAge = $get("tblPatientTitle_lblAge_Value");
    var strAge = document.all ? objAge.innerText : objAge.textContent;
    var strDisplay = (parseInt(strAge) > 18) ? "none" : "block";

    $get("divHeight").style.display = $get("lblHeight").style.display = strDisplay ;
}

//---------------------------------------------------------------------------------------------------------------
function controlBar_Buttons_OnClick(buttonNo)
{
    var idx = 1;
    
    for(; idx <= 5; idx++) 
        try{$get('li_Div' + idx).className = (idx != buttonNo) ? "" : "current";}
        catch(e){}
        
    try{$get("div_vProgress_Notes").style.display = (buttonNo == 1) ? "block" : "none";}catch(e){}
    try{$get("div_vComplications").style.display = (buttonNo == 3) ? "block" : "none";}catch(e){}
    try{$get("div_vPatientReport").style.display = (buttonNo == 4) ? "block" : "none";}catch(e){}
    try{$get("div_vFollowup").style.display = (buttonNo == 5) ? "block" : "none";}catch(e){}
    $get("txtHPageNo").value = buttonNo;
    
    switch(buttonNo){
        case 3 :
            LoadPatientComplicationHistoryData("");
            break;
        case 4 :
            // __doPostBack('btnLoadRefDoctorData','');
            /*if ($get("txtDoctorsList").value.length == 0)
                LoadDoctorsList();
            else
                ReLoadDoctorsList();*/
            break;
    }
    return;
}

//-----------------------------------------------------------------------------------------------------------------
function ShowPatientNotesDiv( ){ 
    var flag = ($get('divPatientNotes').style.display == 'none');
    
    $get('imgPatientNotes').src = flag ? '../../../img/button_minus.gif' : '../../../img/button_plus.gif';
    $get('divPatientNotes').style.display= flag ? 'block' : 'none';
    $get('btnStartDate').style.display = flag ? 'block' : 'none';
    $get('btnEndDate').style.display = flag ? 'block' : 'none';
}

//-----------------------------------------------------------------------------------------------------------------
function ShowOperationListDiv( ){ 
    var flag = ($get('divOperationList').style.display == 'block');
    
    $get('imgOperationList').src = flag ? '../../../img/button_plus.gif' : '../../../img/button_minus.gif';
    $get('divOperationList').style.display = flag ? 'none' : 'block';
}
//-----------------------------------------------------------------------------------------------------------------
function ProgressNotes_ClearFields(){
    ShowVisitFormDiv(0); 
    SetEvents();
    
    VisitDivsInitialSetting();
    document.forms[0].txtDateSeen_PN_txtGlobal.value = document.getElementById("txtHCurrentClientDate").value; 

    // BOLD Comorbidity 
    document.forms[0].cmbHypertension_SystemCodeList.value = "";
    document.forms[0].cmbCongestive_SystemCodeList.value = "";
    document.forms[0].cmbIschemic_SystemCodeList.value = "";
    document.forms[0].cmbAngina_SystemCodeList.value = "";
    document.forms[0].cmbPeripheral_SystemCodeList.value = "";
    document.forms[0].cmbLower_SystemCodeList.value = "";
    document.forms[0].cmbDVT_SystemCodeList.value = "";
    document.forms[0].cmbGlucose_SystemCodeList.value = "";
    document.forms[0].cmbLipids_SystemCodeList.value = "";
    document.forms[0].cmbGout_SystemCodeList.value = "";
    document.forms[0].cmbObstructive_SystemCodeList.value = "";
    document.forms[0].cmbObesity_SystemCodeList.value = "";
    document.forms[0].cmbPulmonary_SystemCodeList.value = "";
    document.forms[0].cmbAsthma_SystemCodeList.value = "";
    document.forms[0].cmbGred_SystemCodeList.value = "";
    document.forms[0].cmbCholelithiasis_SystemCodeList.value = "";
    document.forms[0].cmbLiver_SystemCodeList.value = "";
    document.forms[0].cmbBackPain_SystemCodeList.value = "";
    document.forms[0].cmbMusculoskeletal_SystemCodeList.value = "";
    document.forms[0].cmbFibro_SystemCodeList.value = "";
    document.forms[0].cmbPolycystic_SystemCodeList.value = "";
    document.forms[0].cmbMenstrual_SystemCodeList.value = "";
    document.forms[0].cmbPsychosocial_SystemCodeList.value = "";
    document.forms[0].cmbDepression_SystemCodeList.value = "";
    document.forms[0].cmbConfirmed_SystemCodeList.value = "";
    document.forms[0].cmbAlcohol_SystemCodeList.value = "";
    document.forms[0].cmbTobacco_SystemCodeList.value = "";
    document.forms[0].cmbAbuse_SystemCodeList.value = "";
    document.forms[0].cmbStressUrinary_SystemCodeList.value = "";
    document.forms[0].cmbCerebri_SystemCodeList.value = "";
    document.forms[0].cmbHernia_SystemCodeList.value = "";
    document.forms[0].cmbFunctional_SystemCodeList.value = "";
    document.forms[0].cmbSkin_SystemCodeList.value = "";
    
    document.forms[0].txtCVSNote_txtGlobal.value = "";
	document.forms[0].txtMETNote_txtGlobal.value = "";
    document.forms[0].txtPULNote_txtGlobal.value = "";
    document.forms[0].txtGASNote_txtGlobal.value = "";
	document.forms[0].txtMUSNote_txtGlobal.value = "";
    document.forms[0].txtFEMNote_txtGlobal.value = "";
    document.forms[0].txtPSYNote_txtGlobal.value = "";
    document.forms[0].txtGENNote_txtGlobal.value = "";          
    
    // MEDICATION & VITAMINS
    document.forms[0].chkMutipleVitamin.checked = false;
    document.forms[0].chkCalcium.checked = false;
    document.forms[0].chkVitaminB12.checked = false;
    document.forms[0].chkIron.checked = false;
    document.forms[0].chkVitaminD.checked = false;
    document.forms[0].chkVitaminADE.checked = false;
    document.forms[0].chkCalciumVitaminD.checked = false;
    document.forms[0].txtMedicationNotes_txtGlobal.value = "";
    document.forms[0].txtMedicationSearch_txtGlobal.value = "";

    // visit data YELLOW section
    document.forms[0].txtWeight_PN_txtGlobal.value = ""; 
    document.forms[0].txtRV_PN_txtGlobal.value = ""; 
    document.forms[0].txtNextVisitDate_PN.value = ""; 
    document.forms[0].txtNotes_PN_txtGlobal.value = "";
    document.forms[0].txtLapbandAdjustment_PN_txtGlobal.value = "";
    document.forms[0].txtHConsultID.value = "0";
    document.forms[0].txtHCommentID.value = "0";
    document.forms[0].txtHSaveFlag.value = "0";
    document.forms[0].cmbMonthWeek.selectedIndex = 0;
    document.forms[0].txtMonthWeek_PN_txtGlobal.value = "";
    document.forms[0].chkComorbidity.checked = false;
    document.forms[0].chkLetterSent.checked = false;
    document.forms[0].chkReview.checked = false;
    
    document.forms[0].chkRegistryReview.checked = false;
    document.forms[0].cmbRegistrySleepApnea_SystemCodeList.value = "";
    document.forms[0].cmbRegistryGerd_SystemCodeList.value = "";
    document.forms[0].cmbRegistryHyperlipidemia_SystemCodeList.value = "";
    document.forms[0].cmbRegistryDiabetes_SystemCodeList.value = "";
    
    
    
    // visit data adjustment section    
    document.forms[0].chkAdjConsent.checked = false;
    document.forms[0].chkAdjAntiseptic.checked = false;
    document.forms[0].chkAdjAnesthesia.checked = false;
    document.forms[0].chkAdjNeedle.checked = false;
    document.forms[0].chkAdjVolume.checked = false;
    document.forms[0].chkAdjTolerate.checked = false;
    document.forms[0].chkAdjBarium.checked = false;
    document.forms[0].chkAdjOmni.checked = false;
    document.forms[0].chkAdjProtocol.checked = false;
    document.forms[0].txtAdjAnesthesiaVol_PN.value = ""; 
    document.forms[0].txtInitialVol_PN.value = ""; 
    document.forms[0].txtAddVol_PN.value = ""; 
    document.forms[0].txtRemoveVol_PN.value = ""; 

    
    //visit data review section    
    document.forms[0].txtPR_PN_txtGlobal.value = ""; 
    document.forms[0].txtRR_PN_txtGlobal.value = ""; 
    document.forms[0].txtBP1_PN_txtGlobal.value = ""; 
    document.forms[0].txtBP2_PN_txtGlobal.value = ""; 
    document.forms[0].txtNeck_PN_txtGlobal.value = ""; 
    document.forms[0].txtWaist_PN.value = ""; 
    document.forms[0].txtHip_PN.value = ""; 
    document.forms[0].rbRegistryDiabetesN.checked = true;
    document.forms[0].rbRegistryReoperationN.checked = true;
    checkRegistryDiabetes();
    document.forms[0].chkRegistryDiet.checked = false;
    document.forms[0].chkRegistryOral.checked = false;
    document.forms[0].chkRegistryInsulin.checked = false;
    document.forms[0].chkRegistryOther.checked = false;
    document.forms[0].chkRegistryCombination.checked = false;
    document.forms[0].txtRegistryReoperationReason_txtGlobal.value = "";
    
    document.forms[0].rbRegistrySEN.checked = true;
    document.forms[0].cmbSEList_SystemCodeList.value = "";
    document.forms[0].txtRegistrySEReason_txtGlobal.value = "";
    checkRegistrySE();
    
    document.forms[0].txtHDateCreated.value = "";    
    document.forms[0].btnAddVisit.disabled = false;
    
    var defReview = "";
    for (var i = 0;i<visitReview.length;i++)
    {
        defReview = "document.forms[0].txtDet"+visitReview[i]+"_PN.value=''";
        eval(defReview);
                
        defReview = "document.forms[0].chkDet"+visitReview[i]+".checked=false";
        eval(defReview);
    }
              
    document.forms[0].cmbChiefComplaint_SystemCodeList.selectedIndex = 0;
    document.forms[0].txtHSatiety_PN.value=0;
              
    SetInnerText($get("lblNextVisitDate_PN_Value"), "");
    
    $get("divErrorMessage").style.display = "none";
    SetInnerText($get("pErrorMessage"), "");
    
    CheckRVStatus();
    CheckRegistryData();
    return;
}

//----------------------------------------------------------------------------------------------------------------
function CheckVisitWeightCalculation(){
    var flag = (document.forms[0].txtWeight_PN_txtGlobal.value == "" || document.forms[0].txtWeight_PN_txtGlobal.value == 0);

    Prepare2SaveData();
    
    if (!flag && document.forms[0].txtHSaveFlag.value == "1") {
    //if (document.forms[0].txtHSaveFlag.value == "1") {
        ShowDivMessage("Calculating BMI, Total Loss and %EWL using current weight ...", false);   
        __doPostBack('btnCalculateWeightOtherData','');
    }
    else{
        CleanVisitDetailHeader(1);
    }
}

function CleanVisitDetailHeader(option){    
    SetInnerText($get("lblBMI"), "" );
    SetInnerText($get("lblBMI_Value"), "" );
    SetInnerText($get("lblLossLastVisit"), "" );
    SetInnerText($get("lblLossLastVisit_Value"), "" );
    SetInnerText($get("lblTotalLoss"), "" );
    SetInnerText($get("lblTotalLoss_Value"), "" );
    SetInnerText($get("lblEWLPercentage"), "" );
    SetInnerText($get("lblEWLPercentage_Value"), "" );
    SetInnerText($get("lblWeeks"), "" );
    SetInnerText($get("lblWeeks_Value"), "" );
    SetInnerText($get("lblWHR"), "" );
    
    if(option == 0)
    {
        document.forms[0].btnCancel.style.display = "block";
        document.forms[0].btnCancelComment.style.display = "block";
    }
}

//----------------------------------------------------------------------------------------------------------------
function CheckRVStatus(){
    var objSurgeryType = $get("tblPatientTitle_lblSurgeryType_Value");
    var SurgeryDesc = (document.all) ? objSurgeryType.innerText : objSurgeryType.textContent;
    var flag = SurgeryDesc.toLowerCase().indexOf("gastric banding, adjustable") >= 0;

    //$get("lblReservoirVolume_TH").style.display = ;
    $get("lblRV_PN").style.display = document.forms[0].txtRV_PN_txtGlobal.style.display =  flag ? "block" : "none";
}

//----------------------------------------------------------------------------------------------------------------
function btnCancelVisit_onclick(btnCancel){
    CleanVisitDetailHeader(0);
    $get("txtDateSeen_PN_txtGlobal").value = document.getElementById("txtHCurrentClientDate").value;
    checkRegistryDiabetes();
    checkRegistrySE();
    
    if (document.forms[0].btnAddVisit.style.display == "none"){
        CheckVisitWeightCalculation();
        ShowVisitFormDiv(1);
        if(btnCancel.id == "btnCancel"){
            //SetInnerText($get("lblProgressNote_PN"), "Clinical Notes");
            document.forms[0].btnCancelComment.style.display = "none";
            document.forms[0].btnCancel.style.display = "block";
            document.forms[0].chkCommentOnly.checked = false;
            btnCommentOnly_onclick(document.forms[0].chkCommentOnly);
        }else{
            //SetInnerText($get("lblProgressNote_PN"), "Notes");
            document.forms[0].btnCancel.style.display = "none";
            document.forms[0].btnCancelComment.style.display = "block";
            document.forms[0].chkCommentOnly.checked = true;
            btnCommentOnly_onclick(document.forms[0].chkCommentOnly);
        }
        document.forms[0].btnCancel.value = "Cancel";
        document.forms[0].btnCancelComment.value = "Cancel";
        $get("div_visitDetails").style.display = "none";
        $get("div_adjustmentDetail").style.display = "none";
        $get("divSeparator").style.display = "none";
    }
    else{
        document.forms[0].btnCancel.style.display = "block";
        document.forms[0].btnCancelComment.style.display = "block";
        document.forms[0].btnCancel.value = "Add as visit";
        document.forms[0].btnCancelComment.value = "Add comment";
        
        $get("divSeparator").style.display = "block";
        
        ProgressNotes_ClearFields();
        RemoveUploadLinkSetting();
    }
    
    SetEvents();
    return;
}
//----------------------------------------------------------------------------------------------------------------
function btnAddVisit_onclick(){
    Prepare2SaveData();
}

//----------------------------------------------------------------------------------------------------------------
function Prepare2SaveData(){
    var tempWeight = 0, tempHeight = 0;
    var errorMsg = "";
    
    document.forms[0].txtHSaveResult.value = "0";
    $get("divErrorMessage").style.display = "none";
    
    if (ProgressNotes_CanToSave())
    {
        if(document.forms[0].chkCommentOnly.checked == false)
        {
            tempWeight = parseFloat(document.forms[0].txtWeight_PN_txtGlobal.value);
            tempHeight = parseFloat(document.forms[0].txtHeight_PN_txtGlobal.value);
            
            tempNeck = parseFloat(document.forms[0].txtNeck_PN_txtGlobal.value);
            tempWaist = parseFloat(document.forms[0].txtWaist_PN.value);
            tempHip = parseFloat(document.forms[0].txtHip_PN.value);
            
            if ($get("tblPatientTitle_txtUseImperial").value == "1") // Imperial Mode
            {
                tempWeight = tempWeight * 0.45359237;
                tempHeight = tempHeight * 0.0254;
                
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
                document.forms[0].txtHSaveFlag.value = "0";        
                $get("divErrorMessage").style.display = "block";
                SetInnerText($get("pErrorMessage"), "Please enter the proper"+errorMsg.substring(0, errorMsg.length-1));
                $get("frmConsultFU1").onsubmit = function (){return false};
                return false;
            }
            
            
            $get("txtHWeight_PN").value = tempWeight;
            $get("txtHHeight_PN").value = tempHeight;
            $get("txtHNeck_PN").value = tempNeck;
            $get("txtHWaist_PN").value = tempWaist;
            $get("txtHHip_PN").value = tempHip;
            
            MakeStringSelectedItems(document.forms[0].listMedication_Selected);
        }
        document.forms[0].txtHSaveFlag.value = "1";
    }
    else{    
        document.forms[0].txtHSaveFlag.value = "0";
        $get("divErrorMessage").style.display = "block";
        SetInnerText($get("pErrorMessage"), "Please enter Visit Date...");
        $get("frmConsultFU1").onsubmit = function (){return false;}
        return false;
    }
    //showDetails();
}

//---------------------------------------------------------------------------------------------------------------
function btnAddFile_OnClick(){
    var linkUpload = $get("linkUpload");
    
    try{valid.dispose(linkUpload);}catch(e){}
    
    Prepare2SaveData();
    
    var id = 0;
    var type = "";
    
    if(document.forms[0].chkCommentOnly.checked == true){
        id = $get("txtHCommentID").value;
        type = "C";
    }
    else{
        id = $get("txtHConsultID").value;
        type = "V";
    }
    linkUpload.href = "UploadDocumentForm.aspx?PCode=1&EID=" + id + "&ET=" + type; 
    initialize();
}

//---------------------------------------------------------------------------------------------------------------
function RemoveUploadLinkSetting(){
    var linkUpload = $get("linkUpload");
    
    try{
        valid.dispose(linkUpload);
    }
    catch(e){}
}

//-----------------------------------------------------------------------------------------------------------------
function ShowVisitFormDiv(ShowFlag){  
    document.forms[0].btnSuperBill.style.display = (ShowFlag == 1) ? "block" : "none";
    document.forms[0].btnAddVisit.style.display = (ShowFlag == 1) ? "block" : "none";
    document.forms[0].btnDeleteVisit.style.display = (ShowFlag == 1 && (document.forms[0].txtHConsultID.value >0 || document.forms[0].txtHCommentID.value >0)) ? "block" : "none";

    document.forms[0].btnCancel.value = (ShowFlag == 1) ? "Cancel" : "Add new visit"; 
    document.forms[0].btnCancelComment.value = (ShowFlag == 1) ? "Cancel" : "Add comment"; 
    $get("divSeparator").style.display = (ShowFlag == 1) ? "none" : "block";
    //$get("divVisitDataForm").style.display = (ShowFlag == 1) ? "block" : "none";
    $get("divVisitDataForm").style.display = (ShowFlag == 1) ? "block" : "none";
    //only allow add file upon editting
    $get("tdFileValue").style.display = (ShowFlag == 1 && (document.forms[0].txtHConsultID.value >0 || document.forms[0].txtHCommentID.value >0)) ? "block" : "none";
    $get("tdFile").style.display = (ShowFlag == 1 && (document.forms[0].txtHConsultID.value >0 || document.forms[0].txtHCommentID.value >0)) ? "block" : "none";
    
    return;
}
//----------------------------------------------------------------------------------------------------------------
function VisitRowCheck_onclick(strConsultID,chkCommentOnly){
    if(chkCommentOnly == 1)
        CommentRow_onclick(strConsultID);
    else
        VisitRow_onclick(strConsultID);
    
}
//----------------------------------------------------------------------------------------------------------------
function CommentRow_onclick(strCmmentID)
{
    SetInnerText($get("lblProgressNote_PN"), "Notes");
    IsVisitSelected = true;
    document.forms[0].txtHConsultID.value = 0;
    document.forms[0].txtHCommentID.value = strCmmentID; 
    ShowVisitFormDiv(1);
    LoadVisitData("tblVisitRow_" + strCmmentID ,  strCmmentID );
    document.forms[0].btnCancel.style.display = "none";
}

//----------------------------------------------------------------------------------------------------------------
function VisitRow_onclick(strConsultID)
{
    SetInnerText($get("lblProgressNote_PN"), "Clinical Notes");
    IsVisitSelected = true;
    document.forms[0].txtHCommentID.value = 0;
    document.forms[0].txtHConsultID.value = strConsultID; 
    ShowVisitFormDiv(1);
    LoadVisitData("tblVisitRow_" + strConsultID ,  strConsultID );
    document.forms[0].btnCancelComment.style.display = "none";
    
    SetEvents();
}

//-----------------------------------------------------------------------------------------------------------------
function LoadVisitData(tblVisitRow, strConsultID) 
{ 
    var Rows = $get(tblVisitRow).rows; 
    var ChiefCompTemp;
    var tempCheck;
    var res;
    var resSplit;
    monthGroup = 0;
    
    document.forms[0].txtNotes_PN_txtGlobal.value = "";
    
    for (rowIdx = 0; rowIdx < Rows.length; rowIdx++) 
    { 
        var Cells = Rows[rowIdx].cells; 
        for (cellIdx = 0; cellIdx < Cells.length; cellIdx++) 
            if (rowIdx == 0) 
            { 
                switch(cellIdx) 
                { 
                    case 0 :   // document.all means IE browser
                        document.forms[0].txtDateSeen_PN_txtGlobal.value = document.all ? Cells[cellIdx].innerText : Cells[cellIdx].textContent; 
                        var detDateSeen = document.all ? Cells[cellIdx].innerText : Cells[cellIdx].textContent;
                        break; 
                    case 2 : 
                        document.forms[0].txtWeight_PN_txtGlobal.value = document.all ? Cells[cellIdx].innerText.trim() : Cells[cellIdx].textContent.trim(); 
                        break; 
                    case 3 : 
                        document.forms[0].txtRV_PN_txtGlobal.value = document.all ? Cells[cellIdx].innerText.trim() : Cells[cellIdx].textContent.trim(); 
                        break; 
                    case 8 : 
                        document.forms[0].txtBloodPressure_PN_txtGlobal.value = document.all ? Cells[cellIdx].innerText.trim() : Cells[cellIdx].textContent.trim(); 
                        break; 
                    case 9 : 
                        document.forms[0].cmbDoctorList_PN_DoctorsList.value = $get("txtDoctorId_" + strConsultID).value;
                        break; 
                    case 10 : 
                        document.forms[0].txtNextVisitDate_PN.value = document.all ? Cells[cellIdx].innerText : Cells[cellIdx].textContent; 
                        if (document.all)
                            //$get("lblNextVisitDate_PN_Value").innerText = Cells[cellIdx].innerText; 
                            $get("lblNextVisitDate_PN_Value").innerText = Cells[cellIdx].innerText; 
                        else
                            //$get("lblNextVisitDate_PN_Value").textContent = Cells[cellIdx].textContent; 
                            $get("lblNextVisitDate_PN_Value").textContent = Cells[cellIdx].textContent; 
                        break; 
                        
                    case 12 : 
                        res = document.all ? Cells[cellIdx].innerText : Cells[cellIdx].textContent;
                        resSplit = res.split('*^*');
                        document.forms[0].txtPR_PN_txtGlobal.value = checkNan(resSplit[0]);
                        document.forms[0].txtRR_PN_txtGlobal.value = checkNan(resSplit[1]);
                        document.forms[0].txtBP1_PN_txtGlobal.value = checkNan(resSplit[2]);
                        document.forms[0].txtBP2_PN_txtGlobal.value = checkNan(resSplit[3]);
                        document.forms[0].txtNeck_PN_txtGlobal.value = checkNan(resSplit[4]);
                        document.forms[0].txtWaist_PN.value = checkNan(resSplit[5]);
                        document.forms[0].txtHip_PN.value = checkNan(resSplit[6]);
                        document.forms[0].txtDetGeneral_PN.value = resSplit[7].trim();
                        document.forms[0].txtDetCardio_PN.value = resSplit[8].trim();
                        document.forms[0].txtDetResp_PN.value = resSplit[9].trim();
                        document.forms[0].txtDetGastro_PN.value = resSplit[10].trim();
                        document.forms[0].txtDetGenito_PN.value = resSplit[11].trim();
                        document.forms[0].txtDetExtr_PN.value = resSplit[12].trim();
                        document.forms[0].txtDetNeuro_PN.value = resSplit[13].trim();
                        document.forms[0].txtDetMusculo_PN.value = resSplit[14].trim();
                        document.forms[0].txtDetSkin_PN.value = resSplit[15].trim();
                        document.forms[0].txtDetPsych_PN.value = resSplit[16].trim();
                        document.forms[0].txtDetEndo_PN.value = resSplit[17].trim();
                        document.forms[0].txtDetHema_PN.value = resSplit[18].trim();
                        document.forms[0].txtDetENT_PN.value = resSplit[19].trim();
                        document.forms[0].txtDetEyes_PN.value = resSplit[20].trim();
                        document.forms[0].txtDetPFSH_PN.value = resSplit[21].trim();
                        document.forms[0].txtDetMeds_PN.value = resSplit[22].trim();
                        document.forms[0].txtHSatiety_PN.value = resSplit[23];
                            changeSatiety();
                            
                        document.forms[0].cmbChiefComplaint_SystemCodeList.value = "";       
                        ChiefCompTemp = resSplit[24];
                        if(ChiefCompTemp.trim() != "") 
                            document.forms[0].cmbChiefComplaint_SystemCodeList.value = ChiefCompTemp;
                            
                        document.forms[0].txtLapbandAdjustment_PN_txtGlobal.value = resSplit[25];
                        
                        document.forms[0].cmbMedicalProvider_PN_DoctorsList.value = "";              
                        MedProvTemp = resSplit[26];
                        if(MedProvTemp.trim() != "") 
                            document.forms[0].cmbMedicalProvider_PN_DoctorsList.value = MedProvTemp;
                            
                        if(resSplit[27] == 1)                     
                        document.forms[0].chkAdjConsent.checked = true;
                        else                        
                        document.forms[0].chkAdjConsent.checked = false; 
                        
                        if(resSplit[28] == 1)                     
                        document.forms[0].chkAdjAntiseptic.checked = true;
                        else                        
                        document.forms[0].chkAdjAntiseptic.checked = false; 
                        
                        if(resSplit[29] == 1)                     
                        document.forms[0].chkAdjAnesthesia.checked = true;
                        else                        
                        document.forms[0].chkAdjAnesthesia.checked = false; 
                        
                        document.forms[0].txtAdjAnesthesiaVol_PN.value = resSplit[30];
                        
                        if(resSplit[31] == 1)                     
                        document.forms[0].chkAdjNeedle.checked = true;
                        else                        
                        document.forms[0].chkAdjNeedle.checked = false; 
                        
                        if(resSplit[32] == 1)                     
                        document.forms[0].chkAdjVolume.checked = true;
                        else                        
                        document.forms[0].chkAdjVolume.checked = false; 
                     
                        document.forms[0].txtInitialVol_PN.value = resSplit[33];
                        document.forms[0].txtAddVol_PN.value = resSplit[34];
                        document.forms[0].txtRemoveVol_PN.value = resSplit[35];
                        
                        if(resSplit[36] == 1)                     
                        document.forms[0].chkAdjTolerate.checked = true;
                        else                        
                        document.forms[0].chkAdjTolerate.checked = false; 
                          
                        if(resSplit[37] == 1)                     
                        document.forms[0].chkLetterSent.checked = true;
                        else                        
                        document.forms[0].chkLetterSent.checked = false; 
                        
                        detFirstVisitDate = resSplit[38];
                        detStartWeight = resSplit[39];
                        detIdealWeight = resSplit[40];
                        detHeight = resSplit[41];
                        detImperialFlag = resSplit[43];
                        var detWeight = resSplit[44];
                        var detPrevWeight = resSplit[45];
                        document.forms[0].txtHDateCreated.value = resSplit[46];
                        
                        document.forms[0].cmbSupportGroup_SystemCodeList.selectedIndex = 0;   
                        SupportGroupTemp = resSplit[47];
                        if(SupportGroupTemp.trim() != "") {
                            document.forms[0].cmbSupportGroup_SystemCodeList.value = SupportGroupTemp.trim();
                        }
                        
                        if(resSplit[48] == 1)                     
                        document.forms[0].chkAdjBarium.checked = true;
                        else                        
                        document.forms[0].chkAdjBarium.checked = false; 
                        
                        if(resSplit[49] == 1)                     
                        document.forms[0].chkAdjOmni.checked = true;
                        else                        
                        document.forms[0].chkAdjOmni.checked = false;
                        
                        if(resSplit[50] == 1)                     
                        document.forms[0].chkReview.checked = true;
                        else                        
                        document.forms[0].chkReview.checked = false; 
                        
                        if(resSplit[51] == 1)                     
                        document.forms[0].chkAdjProtocol.checked = true;
                        else                        
                        document.forms[0].chkAdjProtocol.checked = false; 
                                     
                        if(resSplit[52] == 1)                     
                        {
                            document.forms[0].chkCommentOnly.checked = true;
                            btnCommentOnly_onclick(document.forms[0].chkCommentOnly);
                        }
                        else         
                        {             
                            document.forms[0].chkCommentOnly.checked = false; 
                            btnCommentOnly_onclick(document.forms[0].chkCommentOnly);
                        }
                        
                        if(resSplit[53] == 1)                     
                        document.forms[0].chkRegistryReview.checked = true;
                        else                        
                        document.forms[0].chkRegistryReview.checked = false; 
                              
                        document.forms[0].cmbRegistrySleepApnea_SystemCodeList.selectedIndex = 0;              
                        RegistryTemp = resSplit[54];
                        if(RegistryTemp.trim() != "") {
                            document.forms[0].cmbRegistrySleepApnea_SystemCodeList.value = RegistryTemp.trim();
                            }
                                           
                        document.forms[0].cmbRegistryGerd_SystemCodeList.selectedIndex = 0;              
                        RegistryTemp = resSplit[55];
                        if(RegistryTemp.trim() != "") {
                            document.forms[0].cmbRegistryGerd_SystemCodeList.value = RegistryTemp.trim();
                            }
                                            
                        document.forms[0].cmbRegistryHyperlipidemia_SystemCodeList.selectedIndex = 0;              
                        RegistryTemp = resSplit[56];
                        if(RegistryTemp.trim() != "") {
                            document.forms[0].cmbRegistryHyperlipidemia_SystemCodeList.value = RegistryTemp.trim();
                            } 
                                          
                        document.forms[0].cmbRegistryDiabetes_SystemCodeList.selectedIndex = 0;              
                        RegistryTemp = resSplit[57];
                        if(RegistryTemp.trim() != "") {
                            document.forms[0].cmbRegistryDiabetes_SystemCodeList.value = RegistryTemp.trim();
                            }  
                            
                        monthGroup = resSplit[58];
                                  
                        if(resSplit[59] == 1)  
                        document.forms[0].rbRegistryDiabetesY.checked = true;
                        else            
                        document.forms[0].rbRegistryDiabetesN.checked = true; 
                        checkRegistryDiabetes();
                        
                        if(resSplit[60] == 1)                     
                        document.forms[0].chkRegistryDiet.checked = true;
                        else                        
                        document.forms[0].chkRegistryDiet.checked = false; 
                        
                        if(resSplit[61] == 1)                     
                        document.forms[0].chkRegistryOral.checked = true;
                        else                        
                        document.forms[0].chkRegistryOral.checked = false; 
                        
                        if(resSplit[62] == 1)                     
                        document.forms[0].chkRegistryInsulin.checked = true;
                        else                        
                        document.forms[0].chkRegistryInsulin.checked = false; 
                        
                        if(resSplit[63] == 1)                     
                        document.forms[0].chkRegistryOther.checked = true;
                        else                        
                        document.forms[0].chkRegistryOther.checked = false; 
                        
                        if(resSplit[64] == 1)                     
                        document.forms[0].chkRegistryCombination.checked = true;
                        else                        
                        document.forms[0].chkRegistryCombination.checked = false; 
                           
                        if(resSplit[65] == 1)  
                        document.forms[0].rbRegistryReoperationY.checked = true;
                        else            
                        document.forms[0].rbRegistryReoperationN.checked = true; 
                        checkRegistryDiabetes();
                        
                        document.forms[0].txtRegistryReoperationReason_txtGlobal.value = resSplit[66];
                        
                        if(resSplit[67] == 1)  
                        document.forms[0].rbRegistrySEY.checked = true;
                        else            
                        document.forms[0].rbRegistrySEN.checked = true; 
                        checkRegistrySE();
                        
                        document.forms[0].cmbSEList_SystemCodeList.selectedIndex = 0;              
                        RegistryTemp = resSplit[68];
                        if(RegistryTemp.trim() != "") {
                            document.forms[0].cmbSEList_SystemCodeList.value = RegistryTemp.trim();
                            }
                            
                        document.forms[0].txtRegistrySEReason_txtGlobal.value = resSplit[69];
                    break;          
                    
//                    case 12 : 
//                        document.forms[0].txtPR_PN_txtGlobal.value = document.all ? checkNan(Cells[cellIdx].innerText) : checkNan(Cells[cellIdx].textContent);
//                        break; 
//                    case 13 : 
//                        document.forms[0].txtRR_PN_txtGlobal.value = document.all ? checkNan(Cells[cellIdx].innerText) : checkNan(Cells[cellIdx].textContent);
//                        break; 
//                    case 14 : 
//                        document.forms[0].txtBP1_PN_txtGlobal.value = document.all ? checkNan(Cells[cellIdx].innerText) : checkNan(Cells[cellIdx].textContent);
//                        break; 
//                    case 15 : 
//                        document.forms[0].txtBP2_PN_txtGlobal.value = document.all ? checkNan(Cells[cellIdx].innerText) : checkNan(Cells[cellIdx].textContent);
//                        break; 
//                    case 16 : 
//                        document.forms[0].txtNeck_PN_txtGlobal.value = document.all ? checkNan(Cells[cellIdx].innerText) : checkNan(Cells[cellIdx].textContent);
//                        break; 
//                    case 17 : 
//                        document.forms[0].txtWaist_PN.value = document.all ? checkNan(Cells[cellIdx].innerText) : checkNan(Cells[cellIdx].textContent);
//                        break; 
//                    case 18 : 
//                        document.forms[0].txtHip_PN.value = document.all ? checkNan(Cells[cellIdx].innerText) : checkNan(Cells[cellIdx].textContent);
//                        break;                         
//                    case 19 : 
//                        document.forms[0].txtDetGeneral_PN.value = document.all ? Cells[cellIdx].innerText.trim() : Cells[cellIdx].textContent.trim();
//                        break; 
//                    case 20 : 
//                        document.forms[0].txtDetCardio_PN.value = document.all ? Cells[cellIdx].innerText.trim() : Cells[cellIdx].textContent.trim();
//                        break; 
//                    case 21 : 
//                        document.forms[0].txtDetResp_PN.value = document.all ? Cells[cellIdx].innerText.trim() : Cells[cellIdx].textContent.trim();
//                        break; 
//                    case 22 : 
//                        document.forms[0].txtDetGastro_PN.value = document.all ? Cells[cellIdx].innerText.trim() : Cells[cellIdx].textContent.trim();
//                        break; 
//                    case 23 : 
//                        document.forms[0].txtDetGenito_PN.value = document.all ? Cells[cellIdx].innerText.trim() : Cells[cellIdx].textContent.trim();
//                        break; 
//                    case 24 : 
//                        document.forms[0].txtDetExtr_PN.value = document.all ? Cells[cellIdx].innerText.trim() : Cells[cellIdx].textContent.trim();
//                        break;               
//                    case 25 : 
//                        document.forms[0].txtDetNeuro_PN.value = document.all ? Cells[cellIdx].innerText.trim() : Cells[cellIdx].textContent.trim();
//                        break;            
//                    case 26 : 
//                        document.forms[0].txtDetMusculo_PN.value = document.all ? Cells[cellIdx].innerText.trim() : Cells[cellIdx].textContent.trim();
//                        break; 
//                    case 27 : 
//                        document.forms[0].txtDetSkin_PN.value = document.all ? Cells[cellIdx].innerText.trim() : Cells[cellIdx].textContent.trim();
//                        break; 
//                    case 28 : 
//                        document.forms[0].txtDetPsych_PN.value = document.all ? Cells[cellIdx].innerText.trim() : Cells[cellIdx].textContent.trim();
//                        break; 
//                    case 29 : 
//                        document.forms[0].txtDetEndo_PN.value = document.all ? Cells[cellIdx].innerText.trim() : Cells[cellIdx].textContent.trim();
//                        break; 
//                    case 30 : 
//                        document.forms[0].txtDetHema_PN.value = document.all ? Cells[cellIdx].innerText.trim() : Cells[cellIdx].textContent.trim();
//                        break; 
//                    case 31 : 
//                        document.forms[0].txtDetENT_PN.value = document.all ? Cells[cellIdx].innerText.trim() : Cells[cellIdx].textContent.trim();
//                        break; 
//                    case 32 : 
//                        document.forms[0].txtDetEyes_PN.value = document.all ? Cells[cellIdx].innerText.trim() : Cells[cellIdx].textContent.trim();
//                        break;
//                    case 33 : 
//                        document.forms[0].txtDetPFSH_PN.value = document.all ? Cells[cellIdx].innerText.trim() : Cells[cellIdx].textContent.trim();
//                        break; 
//                    case 34 : 
//                        document.forms[0].txtDetMeds_PN.value = document.all ? Cells[cellIdx].innerText.trim() : Cells[cellIdx].textContent.trim();
//                        break;       
//                    case 35 : 
//                        document.forms[0].txtHSatiety_PN.value = document.all ? Cells[cellIdx].innerText : Cells[cellIdx].textContent;
//                        changeSatiety();
//                        break;
//                    case 36 :                        
//                        document.forms[0].cmbChiefComplaint_SystemCodeList.value = "";              
//                        ChiefCompTemp = document.all ? Cells[cellIdx].innerText : Cells[cellIdx].textContent;
//                        if(ChiefCompTemp.trim() != "") 
//                            document.forms[0].cmbChiefComplaint_SystemCodeList.value = ChiefCompTemp;
//                        break;                            
//                    case 37 : 
//                        document.forms[0].txtLapbandAdjustment_PN_txtGlobal.value = document.all ? Cells[cellIdx].innerText : Cells[cellIdx].textContent;
//                        break;                          
//                    case 38 :          
//                        document.forms[0].cmbMedicalProvider_PN_DoctorsList.value = "";              
//                        MedProvTemp = document.all ? Cells[cellIdx].innerText : Cells[cellIdx].textContent;
//                        if(MedProvTemp.trim() != "") 
//                            document.forms[0].cmbMedicalProvider_PN_DoctorsList.value = MedProvTemp;
//                        break; 
//                    case 39 :                 
//                        if(document.all ? Cells[cellIdx].innerText == 1: Cells[cellIdx].textContent == 1)                     
//                        document.forms[0].chkAdjConsent.checked = true;
//                        else                        
//                        document.forms[0].chkAdjConsent.checked = false; 
//                        break;                
//                    case 40 :                                 
//                        if(document.all ? Cells[cellIdx].innerText == 1: Cells[cellIdx].textContent == 1)                     
//                        document.forms[0].chkAdjAntiseptic.checked = true;
//                        else                        
//                        document.forms[0].chkAdjAntiseptic.checked = false; 
//                        break;          
//                    case 41 :                                 
//                        if(document.all ? Cells[cellIdx].innerText == 1: Cells[cellIdx].textContent == 1)                     
//                        document.forms[0].chkAdjAnesthesia.checked = true;
//                        else                        
//                        document.forms[0].chkAdjAnesthesia.checked = false; 
//                        break;          
//                    case 42 :                    
//                        document.forms[0].txtAdjAnesthesiaVol_PN.value = document.all ? Cells[cellIdx].innerText: Cells[cellIdx].textContent;
//                        break;    
//                    case 43 :                                 
//                        if(document.all ? Cells[cellIdx].innerText == 1: Cells[cellIdx].textContent == 1)                     
//                        document.forms[0].chkAdjNeedle.checked = true;
//                        else                        
//                        document.forms[0].chkAdjNeedle.checked = false; 
//                        break;          
//                    case 44 :                                 
//                        if(document.all ? Cells[cellIdx].innerText == 1: Cells[cellIdx].textContent == 1)                     
//                        document.forms[0].chkAdjVolume.checked = true;
//                        else                        
//                        document.forms[0].chkAdjVolume.checked = false; 
//                        break;      
//                    case 45 :                    
//                        document.forms[0].txtInitialVol_PN.value = document.all ? Cells[cellIdx].innerText: Cells[cellIdx].textContent;
//                        break;    
//                    case 46 :                    
//                        document.forms[0].txtAddVol_PN.value = document.all ? Cells[cellIdx].innerText: Cells[cellIdx].textContent;
//                        break;
//                    case 47 :                    
//                        document.forms[0].txtRemoveVol_PN.value = document.all ? Cells[cellIdx].innerText: Cells[cellIdx].textContent;
//                        break;       
//                    case 48 :                                 
//                        if(document.all ? Cells[cellIdx].innerText == 1: Cells[cellIdx].textContent == 1)                     
//                        document.forms[0].chkAdjTolerate.checked = true;
//                        else                        
//                        document.forms[0].chkAdjTolerate.checked = false; 
//                        break;
//                    case 49 :                                 
//                        if(document.all ? Cells[cellIdx].innerText == 1: Cells[cellIdx].textContent == 1)                     
//                        document.forms[0].chkLetterSent.checked = true;
//                        else                        
//                        document.forms[0].chkLetterSent.checked = false; 
//                        break;                       
//                    case 50 :                    
//                        detFirstVisitDate = document.all ? Cells[cellIdx].innerText : Cells[cellIdx].textContent;
//                        break;                    
//                    case 51 :                    
//                        detStartWeight = document.all ? Cells[cellIdx].innerText : Cells[cellIdx].textContent;
//                        break;                   
//                    case 52 :                    
//                        detIdealWeight = document.all ? Cells[cellIdx].innerText : Cells[cellIdx].textContent;
//                        break;                   
//                    case 53 :                    
//                        detHeight = document.all ? Cells[cellIdx].innerText : Cells[cellIdx].textContent;
//                        break;              
//                    case 55 :                    
//                        detImperialFlag = document.all ? Cells[cellIdx].innerText : Cells[cellIdx].textContent;
//                        break;    
//                    case 56 :                    
//                        var detWeight = document.all ? Cells[cellIdx].innerText : Cells[cellIdx].textContent;
//                        break; 
//                    case 57 :                    
//                        var detPrevWeight = document.all ? Cells[cellIdx].innerText : Cells[cellIdx].textContent;
//                        break; 
//                    case 58 :                    
//                        document.forms[0].txtHDateCreated.value = document.all ? Cells[cellIdx].innerText: Cells[cellIdx].textContent;
//                        break; 
//                    case 59 :                          
//                        document.forms[0].cmbSupportGroup_SystemCodeList.selectedIndex = 0;              
//                        SupportGroupTemp = document.all ? Cells[cellIdx].innerText : Cells[cellIdx].textContent;
//                        if(SupportGroupTemp.trim() != "") {
//                            document.forms[0].cmbSupportGroup_SystemCodeList.value = SupportGroupTemp.trim();
//                            }
//                        break;                        
//                    case 60 :                                 
//                        if(document.all ? Cells[cellIdx].innerText == 1: Cells[cellIdx].textContent == 1)                     
//                        document.forms[0].chkAdjBarium.checked = true;
//                        else                        
//                        document.forms[0].chkAdjBarium.checked = false; 
//                        break;  
//                    case 61 :                                 
//                        if(document.all ? Cells[cellIdx].innerText == 1: Cells[cellIdx].textContent == 1)                     
//                        document.forms[0].chkAdjOmni.checked = true;
//                        else                        
//                        document.forms[0].chkAdjOmni.checked = false; 
//                        break;  
//                    case 62 :                                 
//                        if(document.all ? Cells[cellIdx].innerText == 1: Cells[cellIdx].textContent == 1)                     
//                        document.forms[0].chkReview.checked = true;
//                        else                        
//                        document.forms[0].chkReview.checked = false; 
//                        break; 
//                    case 63 :                                 
//                        if(document.all ? Cells[cellIdx].innerText == 1: Cells[cellIdx].textContent == 1)                     
//                        document.forms[0].chkAdjProtocol.checked = true;
//                        else                        
//                        document.forms[0].chkAdjProtocol.checked = false; 
//                        break;   
//                    case 64 :                        
//                        if(document.all ? Cells[cellIdx].innerText == 1: Cells[cellIdx].textContent == 1)                     
//                        {
//                            document.forms[0].chkCommentOnly.checked = true;
//                            btnCommentOnly_onclick(document.forms[0].chkCommentOnly);
//                        }
//                        else         
//                        {               
//                            document.forms[0].chkCommentOnly.checked = false; 
//                            btnCommentOnly_onclick(document.forms[0].chkCommentOnly);
//                        }
//                        break; 
//                    case 65 :   
//                        if(document.all ? Cells[cellIdx].innerText == 1: Cells[cellIdx].textContent == 1)                     
//                        document.forms[0].chkRegistryReview.checked = true;
//                        else                        
//                        document.forms[0].chkRegistryReview.checked = false; 
//                        break; 
//                    case 66 :                          
//                        document.forms[0].cmbRegistrySleepApnea_SystemCodeList.selectedIndex = 0;              
//                        RegistryTemp = document.all ? Cells[cellIdx].innerText : Cells[cellIdx].textContent;
//                        if(RegistryTemp.trim() != "") {
//                            document.forms[0].cmbRegistrySleepApnea_SystemCodeList.value = RegistryTemp.trim();
//                            }
//                        break;    
//                    case 67 :                          
//                        document.forms[0].cmbRegistryGerd_SystemCodeList.selectedIndex = 0;              
//                        RegistryTemp = document.all ? Cells[cellIdx].innerText : Cells[cellIdx].textContent;
//                        if(RegistryTemp.trim() != "") {
//                            document.forms[0].cmbRegistryGerd_SystemCodeList.value = RegistryTemp.trim();
//                            }
//                        break;  
//                    case 68 :                          
//                        document.forms[0].cmbRegistryHyperlipidemia_SystemCodeList.selectedIndex = 0;              
//                        RegistryTemp = document.all ? Cells[cellIdx].innerText : Cells[cellIdx].textContent;
//                        if(RegistryTemp.trim() != "") {
//                            document.forms[0].cmbRegistryHyperlipidemia_SystemCodeList.value = RegistryTemp.trim();
//                            }
//                        break;  
//                    case 69 :                          
//                        document.forms[0].cmbRegistryDiabetes_SystemCodeList.selectedIndex = 0;              
//                        RegistryTemp = document.all ? Cells[cellIdx].innerText : Cells[cellIdx].textContent;
//                        if(RegistryTemp.trim() != "") {
//                            document.forms[0].cmbRegistryDiabetes_SystemCodeList.value = RegistryTemp.trim();
//                            }
//                        break;  
//                    case 70 :                                      
//                        monthGroup = document.all ? Cells[cellIdx].innerText : Cells[cellIdx].textContent;
//                        break;
//                    case 71 :                                 
//                        if(document.all ? Cells[cellIdx].innerText == 1: Cells[cellIdx].textContent == 1)  
//                        document.forms[0].rbRegistryDiabetesY.checked = true;
//                        else            
//                        document.forms[0].rbRegistryDiabetesN.checked = true; 
//                        checkRegistryDiabetes();
//                        break;   
//                    case 72 :                                 
//                        if(document.all ? Cells[cellIdx].innerText == 1: Cells[cellIdx].textContent == 1)                     
//                        document.forms[0].chkRegistryDiet.checked = true;
//                        else                        
//                        document.forms[0].chkRegistryDiet.checked = false; 
//                        break;   
//                    case 73 :                                 
//                        if(document.all ? Cells[cellIdx].innerText == 1: Cells[cellIdx].textContent == 1)                     
//                        document.forms[0].chkRegistryOral.checked = true;
//                        else                        
//                        document.forms[0].chkRegistryOral.checked = false; 
//                        break;   
//                    case 74 :                                 
//                        if(document.all ? Cells[cellIdx].innerText == 1: Cells[cellIdx].textContent == 1)                     
//                        document.forms[0].chkRegistryInsulin.checked = true;
//                        else                        
//                        document.forms[0].chkRegistryInsulin.checked = false; 
//                        break;  
//                    case 75 :                                 
//                        if(document.all ? Cells[cellIdx].innerText == 1: Cells[cellIdx].textContent == 1)                     
//                        document.forms[0].chkRegistryOther.checked = true;
//                        else                        
//                        document.forms[0].chkRegistryOther.checked = false; 
//                        break;  
//                    case 76 :                                 
//                        if(document.all ? Cells[cellIdx].innerText == 1: Cells[cellIdx].textContent == 1)                     
//                        document.forms[0].chkRegistryCombination.checked = true;
//                        else                        
//                        document.forms[0].chkRegistryCombination.checked = false; 
//                        break;  
//                    case 77 :                         
//                        if(document.all ? Cells[cellIdx].innerText == 1: Cells[cellIdx].textContent == 1)  
//                        document.forms[0].rbRegistryReoperationY.checked = true;
//                        else            
//                        document.forms[0].rbRegistryReoperationN.checked = true; 
//                        checkRegistryDiabetes();
//                    case 78 :                                 
//                        document.forms[0].txtRegistryReoperationReason_txtGlobal.value = document.all ? Cells[cellIdx].innerText : Cells[cellIdx].textContent;
//                        break;  
//                    case 79 :                         
//                        if(document.all ? Cells[cellIdx].innerText == 1: Cells[cellIdx].textContent == 1)  
//                        document.forms[0].rbRegistrySEY.checked = true;
//                        else            
//                        document.forms[0].rbRegistrySEN.checked = true; 
//                        checkRegistrySE();
//                    case 80 :                          
//                        document.forms[0].cmbSEList_SystemCodeList.selectedIndex = 0;              
//                        RegistryTemp = document.all ? Cells[cellIdx].innerText : Cells[cellIdx].textContent;
//                        if(RegistryTemp.trim() != "") {
//                            document.forms[0].cmbSEList_SystemCodeList.value = RegistryTemp.trim();
//                            }
//                        break;  
//                    case 81 :                                 
//                        document.forms[0].txtRegistrySEReason_txtGlobal.value = document.all ? Cells[cellIdx].innerText : Cells[cellIdx].textContent;
//                        break;  
                }
            } 
            else
                if (cellIdx == 1) 
                    document.forms[0].txtNotes_PN_txtGlobal.value = document.all ? Cells[cellIdx].innerText : Cells[cellIdx].textContent; 
    }
    
    //check if it is needed to display the registry review
    CheckRegistryData();
    
    
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
    }

    calculateVisitDetail(detDateSeen,detWeight,detPrevWeight);
    calculateWHR(); 
    calculateVol();   
    $get("div_visitDetails").style.display = "none";
    $get("div_adjustmentDetail").style.display = "none";
    
    for (var countRev = 0; countRev < visitReview.length; countRev++)
    {
        checkTextBox(visitReview[countRev],"input");
    }
//    if (document.getElementById("div_ComorbidityVisitsList").style.display.toUpperCase() == "BLOCK"){
//        document.getElementById("chkComorbidity").checked = true;
//        CheckComorbidityVisit(document.getElementById("chkComorbidity"));
//        ShowDivMessage("Loading Comorbidity history data ...", false);
//        __doPostBack('btnLoadVisitData','');
//    }
    if ($get("div_ComorbidityVisitsList").style.display.toUpperCase() == "BLOCK"){
        $get("chkComorbidity").checked = true;
        CheckComorbidityVisit($get("chkComorbidity"));
        ShowDivMessage("Loading Comorbidity history data ...", false);
        __doPostBack('btnLoadVisitData','');
    }
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
    var cmbMonthWeek = $get("cmbMonthWeek"),
        strMonthWeek = $get("txtMonthWeek_PN_txtGlobal").value,
        strDateSeen = $get("txtDateSeen_PN_txtGlobal").value;
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
                    $get("lblDateSeen_PN").style.color = "RED"; 
                    $get("txtDateSeen_PN_txtGlobal").focus(); 
                    SetInnerText($get("lblNextVisitDate_PN_Value"), "");
                    $get("txtNextVisitDate_PN").value = "";
                    break; 
                    
                case "E2" : // the Month or Week value is wrong
                    $get("lblNextVisitDate_PN").style.color = "RED"; 
                    $get("txtMonthWeek_PN_txtGlobal").focus(); 
                    SetInnerText($get("lblNextVisitDate_PN_Value"), "");
                    $get("txtNextVisitDate_PN").value = "";
                    break;
       
                default :
                    strDate = strReturnValue;
                    $get("lblDateSeen_PN").style.color = "";
                    $get("lblNextVisitDate_PN").style.color = ""; 
                    $get("txtNextVisitDate_PN").value = strDate.substring(0, strDate.indexOf(";")) ; 
                    SetInnerText($get("lblNextVisitDate_PN_Value"), strDate.substring(strDate.indexOf(";")+1));
                    break;  
            }  
        }
    return;
}

//----------------------------------------------------------------------------------------------------------------
function ProgressNotes_CanToSave(){
    var flag = new Boolean();
    
    flag = true;
    
    $get("txtDateSeen_PN_txtGlobal").value = $get("txtDateSeen_PN_txtGlobal").value.trim();
    $get("lblDateSeen_PN").style.color = "";
    if ($get("txtDateSeen_PN_txtGlobal").value == ''){
        $get("lblDateSeen_PN").style.color = "RED";
        flag = false;
    }
    /*
    $get("lblWeight_PN").style.color = "";
    if ($get("txtWeight_PN_txtGlobal").value.length == 0 || $get("txtWeight_PN_txtGlobal").value > 3000){
        $get("lblWeight_PN").style.color = "RED";
        flag = false;
    }*/
    return(flag);
}

//----------------------------------------------------------------------------------------------------------------
function CheckAllVisitDocument(){
    var cmbDocument = document.getElementsByName("cmbDocument");
    var VisitID = '';
    var tdDocument = '';
    
    for (Xh = 0; Xh < cmbDocument.length; Xh++){
        VisitID = cmbDocument[Xh].id.substring(cmbDocument[Xh].id.lastIndexOf("_") + 1);
        tdDocument = $get("tdDocument_" + VisitID);
        cmbDocument[Xh].style.display = (cmbDocument[Xh].options.length > 0) ? "block" : "none";
        if (cmbDocument[Xh].options.length > 0){
            var objOption = document.createElement("OPTION");
            objOption.selected = true;
            objOption.value = 0;
            objOption.text = cmbDocument[Xh].options.length + " " + ((cmbDocument[Xh].options.length == 1) ? "File" : "Files");
            cmbDocument[Xh].options.add(objOption, 0);
            tdDocument.onclick = function(){
            }
        }
        else{
        }
    }
    $get("div_VisitsList").style.display = "block";
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
                $get("txtPatientNotes_txtGlobal").value = strReturnValue + "\r \t" + $get("txtPatientNotes_txtGlobal").value;
            else
                $get("txtPatientNotes_txtGlobal").value = $get("txtPatientNotes_txtGlobal").value + "\r" + strReturnValue ;
                
            // after adding Date at the Start or End of Patient Notes, we should update the "NOTES" in "tblPatientWeightData"
            txtPatientNotes_onchange();
        } // if status == 200
    return;
}

//----------------------------------------------------------------------------------------------------------------
function txtPatientNotes_onchange(){
    var strNotes = $get("txtPatientNotes_txtGlobal").value;
    strNotes = strNotes.replace(/&/,"&amp;")
    var strSOAP = 
            '<?xml version="1.0" encoding="utf-8"?>'+
            '<soap:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" ' + 
                            'xmlns:xsd="http://www.w3.org/2001/XMLSchema" ' + 
                            'xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">'+
	            '<soap:Body>'+
		            '<UpdatePatientNotes xmlns="http://tempuri.org/">'+
		            '   <strPatientNotes>' + strNotes + '</strPatientNotes>' + 
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
    $get('ReportCode').value = obj.checked ? 'OD' : '';
    return;
}

//----------------------------------------------------------------------------------------------------------------
function BuildReport(PreviewFlag){
    var strParam = ReportParams();
    
    __doPostBack('btnSaveReportNotes','');
                
    var strURL = '../../../GroupReports/BuildReportPage.aspx?RP=' + $get('ReportCode').value;
    strURL += "&PID=" + $get("txtHPatientID").value + "&Preview="+PreviewFlag+"&Param=" + strParam+ "&Format="+PreviewFlag;
    
    if($get("divErrorMessageReport").style.display == "none")
    {
        if (PreviewFlag == 0){        
            var browserCheck = (document.all) ? 1 : 0; // ternary operator statements to check if IE. set 1 if is, set 0 if not.
            //
            if (/Firefox[\/\s](\d+\.\d+)/.test(navigator.userAgent)){        
                newWin = window.open(strURL, null, 'height=5, width=5,toolbar=no,menubar=no,location=no,resizable=no,status=no');
                newWin.moveTo(1,1);
                LoadingTimer = window.setInterval("newWin.print();window.clearInterval(LoadingTimer);newWin.close();", 18000);
            }
            else
            {
                $get('frameReport').src = strURL;
                LoadingTimer = window.setInterval("CheckReportIsLoaded();", 2000);
            }
        }
        else if(PreviewFlag == 4){        
            var browserCheck = (document.all) ? 1 : 0; // ternary operator statements to check if IE. set 1 if is, set 0 if not.
            //
            if (/Firefox[\/\s](\d+\.\d+)/.test(navigator.userAgent)){        
                newWin = window.open(strURL, null, 'scrollbars=yes,fullscreen=yes,toolbar=no,menubar=no,location=no,resizable=yes');
                LoadingTimer = window.setInterval("window.clearInterval(LoadingTimer);newWin.close();", 15000);
            }
            else
            {
                $get('frameReport').src = strURL;
                LoadingTimer = window.setInterval("CheckReportIsLoaded();", 15000);
            }
        }
        else{        
            window.open(strURL, null, 'scrollbars=yes,fullscreen=yes,toolbar=no,menubar=no,location=no,resizable=yes');
        }
    }
}

//----------------------------------------------------------------------------------------------------------------
function ReportParams(){
    var strParam = new String();
    var cmbDoctor1 = $get('cmbDoctor1');
    var cmbDoctor2 = $get('cmbDoctor2');
    var cmbDoctor3 = $get('cmbDoctor3');
    var letterFrom = document.forms[0].cmbLetterFrom_PN_DoctorsList.value;     
    if(letterFrom < 10)
        letterFrom = '00'+letterFrom;
    else if(letterFrom < 100)
        letterFrom = '0'+letterFrom;
    if(letterFrom == '')    
        letterFrom = '000';
        
    var letterLogo = "";
    
    $get("divErrorMessageReport").style.display = "none";
    
    
    switch($get('ReportCode').value)
    {
        case "FUA" :
            //AIGB setting
            //if its aigb, and there is logo, must choose
            if($get('txtHLogoMandatory').value == "1" && document.forms[0].cmbLogoFollowup_LogosList.value == "")
            {
                $get("divErrorMessageReport").style.display = "block";
                SetInnerText($get("pErrorMessageReport"), "Please select one of the Logo");
            }
    
            strParam  = $get('chkIncludePhoto').checked ? "1" : "0";
            strParam += $get('chkIncludeProgressNotes').checked ? "1" : "0";
            strParam += $get('chkIncludePatientNote').checked ? "1" : "0";
            strParam += $get('chkIncludeLast10Visit').checked ? "1" : "0";
            strParam += $get('chkIncludeComments').checked ? "1" : "0";
            
            
            letterLogo = document.forms[0].cmbLogoFollowup_LogosList.value;   
            
            if(letterLogo == '')  
                letterLogo = '000';
            else if(letterLogo < 10)
                letterLogo = '00'+letterLogo;
            else if(letterFrom < 100)
                letterLogo = '0'+letterLogo;
        
                
            strParam += letterLogo; 
            break;
        case "RDL" :
            //AIGB setting
            //if its aigb, and there is logo, must choose
            if($get('txtHLogoMandatory').value == "1" && document.forms[0].cmbLogoLetterDoctor_LogosList.value == "")
            {
                $get("divErrorMessageReport").style.display = "block";
                SetInnerText($get("pErrorMessageReport"), "Please select one of the Logo");
            }
            strParam  = $get('chkCurrentVisit').checked ? "1" : "0";
            strParam += $get('chkProgressNotes').checked ? "1" : "0";
            strParam += $get('chkPatientNote').checked ? "1" : "0";
            strParam += $get('chkLast10Visit').checked ? "1" : "0";
            strParam += $get('chkComments').checked ? "1" : "0";
            strParam += letterFrom;
            
            letterLogo = document.forms[0].cmbLogoLetterDoctor_LogosList.value; 
            
            if(letterLogo == '')    
                letterLogo = '000';
            else if(letterLogo < 10)
               letterLogo = '00'+letterLogo;
            else if(letterFrom < 100)
                letterLogo = '0'+letterLogo;
                    
            strParam += letterLogo; 
            
            strParam += ((cmbDoctor1.checked == true) && (strParam.indexOf(";" + cmbDoctor1.value) == -1)) ? (";" + cmbDoctor1.value) : "";
            strParam += ((cmbDoctor2.checked == true) && (strParam.indexOf(";" + cmbDoctor2.value) == -1)) ? (";" + cmbDoctor2.value) : "";
            strParam += ((cmbDoctor3.checked == true) && (strParam.indexOf(";" + cmbDoctor3.value) == -1)) ? (";" + cmbDoctor3.value) : "";
            
                
            //strParam += ((cmbDoctor1.options.length > 1) && (cmbDoctor1.selectedIndex > 0) && (strParam.indexOf(";" + cmbDoctor1.value) == -1)) ? (";" + cmbDoctor1.value) : "";
            //strParam += ((cmbDoctor2.options.length > 1) && (cmbDoctor2.selectedIndex > 0) && (strParam.indexOf(";" + cmbDoctor2.value) == -1)) ? (";" + cmbDoctor2.value) : "";
            //strParam += ((cmbDoctor3.options.length > 1) && (cmbDoctor3.selectedIndex > 0) && (strParam.indexOf(";" + cmbDoctor3.value) == -1)) ? (";" + cmbDoctor3.value) : "";
            break;
        case "IEWLG" :
            strParam = $get('chkIncActualLoss').checked ? "1" : "0";
        break;
        case "SB" :
            if($get('txtHLogoMandatory').value == "1" && document.forms[0].cmbLogoSuperBill_LogosList.value == "")
            {
                $get("divErrorMessageReport").style.display = "block";
                SetInnerText($get("pErrorMessageReport"), "Please select one of the Logo");
            }
            
            strParam = $get('txtHTempConsultID').value;
            strParam += "-";            
            strParam += $get('chkSignBy').checked ? "1" : "0";
            strParam += "-";            
            strParam += $get('chk99204').checked ? "1" : "0";
            strParam += $get('chk99213').checked ? "1" : "0";
            strParam += $get('chk99214').checked ? "1" : "0";
            strParam += $get('chkS2083').checked ? "1" : "0";
            strParam += $get('chk74230').checked ? "1" : "0";
            strParam += $get('chk77002').checked ? "1" : "0";
            strParam += $get('chk99212').checked ? "1" : "0";
            
            letterLogo = document.forms[0].cmbLogoSuperBill_LogosList.value; 
            
            if(letterLogo == '')    
                letterLogo = '000';
            else if(letterLogo < 10)
               letterLogo = '00'+letterLogo;
            else if(letterFrom < 100)
                letterLogo = '0'+letterLogo;
                    
            strParam += "-";            
            strParam += letterLogo; 
            break;             
        default :
            strParam = "";
            break;
    }
    return(strParam);
}

//----------------------------------------------------------------------------------------------------------------
function CheckReportIsLoaded(objFrame){
    window.clearInterval(LoadingTimer);        
    frames["frameReport"].focus();
    frames["frameReport"].print();
    /*else if($get('frameReport').contentDocument.body.offsetHeight > 0){
        window.clearInterval(LoadingTimer);        
        parent.frames[0].body.onload = window.print();
    }*/
}

//----------------------------------------------------------------------------------------------------------------
function ShowDivReportItems(strObjName){
    var ReportTitles = new Array("FollowUpDetails", "LetterToDoctor", "Graphs", "Super", "Comorbidities", "OtherReports");
    var Xh = 0;
    
    for (; Xh < 6; Xh++){
        $get("chk" + ReportTitles[Xh]).checked = false;
        $get("div" + ReportTitles[Xh]).style.display = 'none';
        $get("div" + ReportTitles[Xh]).style.visibility = 'hidden';
    }
    $get("chk" + strObjName).checked = true;
    $get("div" + strObjName).style.display = 'block';
    $get("div" + strObjName).style.visibility = 'visible';
    GetTheSelectedReportName(strObjName);
    
    if(strObjName == "FollowUpDetails")
    {
        $get("btnDownloadExcel").style.visibility = 'hidden';
        $get("btnDownloadWord").style.visibility = 'visible';
        $get("btnDownloadPdf").style.visibility = 'hidden';
    }
    else if(strObjName == "LetterToDoctor")
    {
        $get("btnDownloadExcel").style.visibility = 'hidden';
        $get("btnDownloadWord").style.visibility = 'visible';
        $get("btnDownloadPdf").style.visibility = 'visible';
    }
    else if(strObjName == "Graphs")
    {
        $get("btnDownloadExcel").style.visibility = 'visible';
        $get("btnDownloadWord").style.visibility = 'hidden';
        $get("btnDownloadPdf").style.visibility = 'hidden';
    }
    else
    {
        $get("btnDownloadExcel").style.visibility = 'hidden';
        $get("btnDownloadWord").style.visibility = 'hidden';
        $get("btnDownloadPdf").style.visibility = 'hidden';
    }
    
    if(strObjName == "Super")
    {    
        $get("divPrintBtns").style.display = 'none';
        $get("divPrintBtns").style.visibility = 'hidden';
        
        $get("divPrintBillBtns").style.display = 'block';
        $get("divPrintBillBtns").style.visibility = 'visible';    
    }
    else
    {
        $get("divPrintBillBtns").style.display = 'none';
        $get("divPrintBillBtns").style.visibility = 'hidden';
        
        $get("divPrintBtns").style.display = 'block';
        $get("divPrintBtns").style.visibility = 'visible';
    }
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
                    $get("ReportCode").value = GraphsReportCode[Xh];
                    break;
                case "Comorbidities" :
                    $get("ReportCode").value = ComorbiditiesReportCode[Xh];
                    break;
                case "OtherReports" :
                    $get('ReportCode').value = '';
                    $get('chkOperation').checked = false;
                    $get('chkPhotos').checked = false;
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
            //$get("txtDoctorsList").value = "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?>" + XmlHttp.responseText;
            $get("txtDoctorsList").value = XmlHttp.responseText;
            CreateXmlDocument();
            try {FillDoctorDropdownLists();}catch (e){}
        }
    SetCursor("default");
    return;
}

//----------------------------------------------------------------------------------------------------------------
function ReLoadDoctorsList(){
    if ($get("cmbDoctor1").options.length < 2){
        ReCreateXmlDocument($get("txtDoctorsList").value );
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
            
            $get("cmbDoctor1").options.add(oOption1);
            $get("cmbDoctor2").options.add(oOption2);
            $get("cmbDoctor3").options.add(oOption3);
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
    if ($get(strDropdownName).options.length > 0){
        var oOption = document.createElement("OPTION");
        
        oOption.value = 0;
        oOption.text = "Select...";
        oOption.selected = true;
        $get(strDropdownName).options.add(oOption, 0);
    }
    return;
}
            
//----------------------------------------------------------------------------------------------------------------
function LoadPatientData_CallBack(){
    var intChildQty = 0;
    
    if(XmlHttp.readyState == 4) 
        if(XmlHttp.status == 200)  {
            //$get("txtDoctorsList").value = "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?>" + XmlHttp.responseText;
            CreateXmlDocument();
            
            if (document.all) // code for IE
                intChildQty = XmlDoc.documentElement.childNodes.length;
            else
                intChildQty = parseInt(XmlDoc.documentElement.childNodes.length / 2);
            
            if (intChildQty > 0) {
                if (XmlDoc.getElementsByTagName("RefDrId1")[0].hasChildNodes())
                    $get("cmbDoctor1").value = XmlDoc.getElementsByTagName("RefDrId1")[0].firstChild.nodeValue;
                    
                if (XmlDoc.getElementsByTagName("RefDrId2")[0].hasChildNodes())
                    $get("cmbDoctor2").value = XmlDoc.getElementsByTagName("RefDrId2")[0].firstChild.nodeValue;
                    
                if (XmlDoc.getElementsByTagName("RefDrId3")[0].hasChildNodes())
                    $get("cmbDoctor3").value = XmlDoc.getElementsByTagName("RefDrId3")[0].firstChild.nodeValue;
            }
        }
    SetCursor("default");
    return;
}

//----------------------------------------------------------------------------------------------------------------
function cmbDocument_onchange(cmbDocument){
    
    var selected_index = cmbDocument.selectedIndex;
    var selected_text = cmbDocument.options[selected_index].id;

    if(selected_text.indexOf('.DOCX')>= 0 || selected_text.indexOf('.docx')>= 0 || selected_text.indexOf('.XLSX')>= 0 || selected_text.indexOf('.xslx')>= 0)
        window.open("../../FileManagement/Documents/"+cmbDocument.value+'_'+selected_text, 'download')
        
        
        
    ShowErrorMessageDiv("none", "");
    document.body.style.cursor = "wait";
    if (parseInt(cmbDocument.value) == 0) return;
    var requestURL = $get("txtHApplicationURL").value + "Forms/FileManagement/FileManagementForm.aspx?ReLoad=0&QSN=LOADINGDOCUMENT";
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
    $get("div_Lists").style.display = !chkComorbidity.checked ? "block" : "none";
    $get("div_Comorbidity").style.display = chkComorbidity.checked ? "block" : "none";
    
    $get("li_VisitList").className = !chkComorbidity.checked ? "current" : "";
    $get("li_ComorbidityList").className = "";
    $get("li_VisitList").style.display = !chkComorbidity.checked ? "block" : "none";
    $get("li_ComorbidityList").style.display = !chkComorbidity.checked ? "block" : "none";
    
    $get("div_VisitsList").style.display = !chkComorbidity.checked ? "block" : "none";
    $get("div_ComorbidityVisitsList").style.display = chkComorbidity.checked ? "block" : "none";
    
    $get("li_BoldComorbidity").className = (chkComorbidity.checked && ($get("div_BoldComorbidity").style.display.toUpperCase() == "BLOCK")) ? "current" : "";
    $get("li_Vitamins").className = (chkComorbidity.checked && ($get("div_Vitamins").style.display.toUpperCase() == "BLOCK")) ? "current" : "";
    
    $get("li_BoldComorbidity").style.display = chkComorbidity.checked ? "block" : "none";
    $get("li_Vitamins").style.display = chkComorbidity.checked ? "block" : "none";
    
    if (chkComorbidity.checked)
        __doPostBack('btnLoadBaselineComorbidity','');
}

//---------------------------------------------------------------------------------------------------------------
function VisitPagesClick(idx){
    $get("li_VisitList").className = (idx == 1) ? "current" : "";
    $get("li_ComorbidityList").className = (idx == 2) ? "current" : "";
    
    $get("div_VisitsList").style.display = (idx == 1) ? "block" : "none";
    $get("div_ComorbidityVisitsList").style.display = (idx == 2) ? "block" : "none";
    
    $get("txtCurrList").value = (idx == 1) ? "visit" : "comorbidity";
    
    
    SelectedVisit = idx;
}

//---------------------------------------------------------------------------------------------------------------
function ComorbidityPagesClick(idx){
//    $get("li_BloodPressure").className = (idx == 1) ? "current" : "";
//    $get("li_MajorComorbidity").className = (idx == 2) ? "current" : "";
    $get("li_BoldComorbidity").className = (idx == 3) ? "current" : "";
    $get("li_Vitamins").className = (idx == 4) ? "current" : "";
    
//    $get("div_BloodPressure").style.display = (idx == 1) ? "block" : "none";
//    $get("div_MajorComorbidity").style.display = (idx == 2) ? "block" : "none";
    $get("div_BoldComorbidity").style.display = (idx == 3) ? "block" : "none";
    $get("div_Vitamins").style.display = (idx == 4) ? "block" : "none";
    
    SetEvents();
}

//---------------------------------------------------------------------------------------------------------------
function VisitDivsInitialSetting(){
    $get("li_VisitList").style.display = "block";
    $get("li_ComorbidityList").style.display = "block";
//    $get("li_BloodPressure").style.display = "none";
///    $get("li_MajorComorbidity").style.display = "none";
    $get("li_BoldComorbidity").style.display = "none";
    $get("li_Vitamins").style.display = "none";

    $get("div_Lists").style.display = "block";
    $get("div_Comorbidity").style.display = "none";
    
    $get("li_VisitList").className = (SelectedVisit == 1) ? "current" : "";
    $get("li_ComorbidityList").className = (SelectedVisit == 2) ? "current" : "";
    $get("div_VisitsList").style.display = (SelectedVisit == 1) ? "block" : "none";
    $get("div_ComorbidityVisitsList").style.display = (SelectedVisit == 2) ? "block" : "none";
}

//-----------------------------------------------------------------------------------------------------------------
function aCalendar_onclick(obj, strDate){
    var strDateformat = $get("lblDateFormat");
    languageCode = document.forms[0].txtHCulture.value.substr(0, 2);
    displayCalendar($get(strDate + "_txtGlobal"), document.all ? strDateformat.innerText : strDateformat.textContent, obj)
} 

//-----------------------------------------------------------------------------------------------------------------
function MedicationVitamin(strCheckBoxID){
    $get(strCheckBoxID).checked = !$get(strCheckBoxID).checked;
}

//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
/*this is a function to delete an visit*/
function btnDelete_onclick(){
    $get("txtHDelete").value=0; 
    var answer = confirm ("It will permanently delete this visit. Do you want to proceed?")
    if (answer)
        $get("txtHDelete").value = 1;
    return;
}

function showDetails(){

    if($get("div_visitDetails").style.display == "none")
    {
        $get("div_visitDetails").style.display = "block";
        $get("imgShowDetails").src="../../../img/button_minus.gif";
    }
    else
    {
        $get("div_visitDetails").style.display = "none";
        $get("imgShowDetails").src="../../../img/button_plus.gif";
    }
      
    for (var countRev = 0; countRev < visitReview.length; countRev++)
    {
        checkTextBox(visitReview[countRev],"input");
    }
}

function showAdjustment(){
        
    if($get("div_adjustmentDetail").style.display == "none")
    {
        $get("div_adjustmentDetail").style.display = "block";
        $get("imgShowAdjustment").src="../../../img/button_minus.gif";
    }
    else
    {
        $get("div_adjustmentDetail").style.display = "none";
        $get("imgShowAdjustment").src="../../../img/button_plus.gif";
    } 
}

function markNoSymptom(){
    for (var countRev = 0; countRev < visitReview.length; countRev++)
    {    
        defaultReviewInput = setDefaultReview(visitReview[countRev]);
        
        if($get("chkDet"+visitReview[countRev]).checked == true && $get("txtDet"+visitReview[countRev]+"_PN").value.trim() == "")
            $get("txtDet"+visitReview[countRev]+"_PN").value = defaultReviewInput;
        else if($get("chkDet"+visitReview[countRev]).checked == false){
            $get("chkDet"+visitReview[countRev]).checked = true;
            $get("txtDet"+visitReview[countRev]+"_PN").value = defaultReviewInput;
        }
    }
}

function checkTextBox(examReview,type){
   
    defaultReviewInput = setDefaultReview(examReview);
    
    if(type=="check"){
        if($get("chkDet"+examReview).checked == false)
            $get("txtDet"+examReview+"_PN").value = "";        
        else if($get("chkDet"+examReview).checked == true){
            $get("txtDet"+examReview+"_PN").value = defaultReviewInput;
        }
    }
    else if(type=="input"){
        if($get("txtDet"+examReview+"_PN").value.trim() != "")
            $get("chkDet"+examReview).checked = true;
        else
            $get("chkDet"+examReview).checked = false;
    }
}

function setDefaultReview(examRev){
    var defaultReview ="";
    switch(examRev)       
    {         
        case "General":   
            defaultReview = defaultGeneralReview;
            break;                  
        case "Cardio":      
            defaultReview = defaultCardioReview;
            break;             
        case "Resp":      
            defaultReview = defaultRespReview;
            break;             
        case "Gastro":   
            defaultReview = defaultGastroReview;
            break;         
        case "Genito":    
            defaultReview = defaultGenitoReview;
            break;              
        case "Extr":     
            defaultReview = defaultExtrReview;
            break;             
        case "Neuro":    
            defaultReview = defaultNeuroReview;
            break;             
        case "Musculo":   
            defaultReview = defaultMusculoReview;
            break;             
        case "Skin":        
            defaultReview = defaultSkinReview;
            break;             
        case "Psych":       
            defaultReview = defaultPsychReview;
            break;             
        case "Endo":      
            defaultReview = defaultEndoReview;
            break;             
        case "Hema":       
            defaultReview = defaultHemaReview;
            break;             
        case "ENT":        
            defaultReview = defaultENTReview;
            break;             
        case "Eyes":      
            defaultReview = defaultEyesReview;
            break;             
        case "PFSH":       
            defaultReview = defaultPFSHReview;
            break;             
        case "Meds":     
            defaultReview = defaultMedsReview;
            break;            
        
    }
    return defaultReview;

}

function setBaselineData(startWeight,idealWeight,prevWeight,height,firstVisitDate,imperialFlag,weight,visitDate,months){
    if(weight >0)
    {
        detStartWeight = startWeight;
        detHeight = height;
        detFirstVisitDate = firstVisitDate;
        detImperialFlag = imperialFlag;
        detIdealWeight = detImperialFlag.toLowerCase() == "false"?idealWeight:idealWeight / 0.45359237;
        calculateVisitDetail(visitDate,weight,prevWeight);
    }
    monthGroup = months;
    CheckRegistryData();
}


function calculateVisitDetail(detDateSeen, detWeight, detPrevWeight){
    var BMI;
    var EWL;
    var totalWeightLoss;
    
    SetInnerText($get("lblLossLastVisit"), "");
    SetInnerText($get("lblTotalLoss"), "");
    SetInnerText($get("lblBMI"), "");
    SetInnerText($get("lblEWLPercentage"), "");
    SetInnerText($get("lblWeeks"), "");    
    
    //calculate weight loss
    weightLoss = detPrevWeight - detWeight;
    weightLoss = detImperialFlag.toLowerCase() == "false"?weightLoss:weightLoss / 0.45359237;
    weightLoss = (Math.round(weightLoss*10))/10; 
    SetInnerText($get("lblLossLastVisit"), "Loss : "+weightLoss);
      
    //calculate total weight loss
    totalWeightLoss = detStartWeight - detWeight;
    totalWeightLoss = detImperialFlag.toLowerCase() == "false"?totalWeightLoss:totalWeightLoss / 0.45359237;
    totalWeightLoss = (Math.round(totalWeightLoss*10))/10;    
    SetInnerText($get("lblTotalLoss"), "Total Loss : "+totalWeightLoss);

    //calculate BMI
    if(Math.floor(detHeight) != 0){
        if(detImperialFlag.toLowerCase() == "false"){
            BMI = detWeight / Math.pow(detHeight,2);
        }
        else{
            BMI = ((detWeight / 0.45359237) / Math.pow(detHeight / 0.0254, 2)) * 703;
        }
        BMI = (Math.round(BMI*10))/10;
        SetInnerText($get("lblBMI"), "BMI : "+BMI);
    }       
    //calculate EWL
    if(detStartWeight - detIdealWeight != 0){
        //round everything before calculating ewl
        EWL = (Math.round(detStartWeight) - Math.round(detWeight)) * 100 / (Math.round(detStartWeight) - Math.round(detIdealWeight));
        // round it down as sqlserver round the result down
        EWL = (Math.floor(EWL*100))/100;
        EWL = EWL.toFixed(1);
        SetInnerText($get("lblEWLPercentage"), "% EWL : "+EWL);
    }
    

    if(detFirstVisitDate != "")
    {
        //calculate Total Weeks   
        dateFirstArr = detFirstVisitDate.split("/");    
        
        if(GetCookie("CultureInfo") == "en-us"){
            dateFirstSeen = dateFirstArr[1];
            monthFirstSeen = dateFirstArr[0];
        }
        else
        {
            dateFirstSeen = dateFirstArr[0];
            monthFirstSeen = dateFirstArr[1];
        }
        yearFirstSeen = dateFirstArr[2];
        
        dateArr = detDateSeen.split("/");   
        
        if(GetCookie("CultureInfo") == "en-us"){
            dateSeen = dateArr[1];
            monthSeen = dateArr[0];
        }
        else{
            dateSeen = dateArr[0];
            monthSeen = dateArr[1];
        }
        yearSeen = dateArr[2];
            
        //Set the two dates
        var currVisitDate =new Date(yearSeen, monthSeen-1, dateSeen); //Month is 0-11 in JavaScript
        var firstVisitDate =new Date(yearFirstSeen, monthFirstSeen-1, dateFirstSeen);   
        var one_day=1000*60*60*24; //Get 1 day in milliseconds

        weekDiff = Math.ceil((currVisitDate.getTime()-firstVisitDate.getTime())/(one_day)/7);   
    }
    else
        weekDiff = "";
        
    SetInnerText($get("lblWeeks"), "Weeks : "+weekDiff);
    
}

function calculateWHR(){
    var Waist = parseInt($get("txtWaist_PN").value);
    var Hip = parseInt($get("txtHip_PN").value);

    var WHR = Waist/Hip;
    
    if(Waist >= 0 && Hip >= 0)
        WHR = WHR.toFixed(1);
    else
        WHR = "";
        
    SetInnerText($get("lblWHR"), WHR);
}

function checkNan(value){
    if(value.trim() == "NaN")
        return "";
    else
        return value.trim();
}

String.prototype.trim = function () {
    return this.replace(/^\s*/, "").replace(/\s*$/, "");
}

function btnPrintSuperBill_onclick(){
    controlBar_Buttons_OnClick(4);
    ShowDivReportItems('Super');        
}

function calculateVol(){
    var InitialVol = parseFloat($get("txtInitialVol_PN").value)>=0?parseFloat($get("txtInitialVol_PN").value):0;
    var VolAdded = parseFloat($get("txtAddVol_PN").value)>=0?parseFloat($get("txtAddVol_PN").value):0;
    var VolRemoved = parseFloat($get("txtRemoveVol_PN").value)>=0?parseFloat($get("txtRemoveVol_PN").value):0;
    var FinalVol = InitialVol + VolAdded - VolRemoved;
    //2 decimal places
    FinalVol = FinalVol.toFixed(2);
    
    SetInnerText($get("lblFinalVol"), FinalVol);
    $get("txtHFinalVol").value = FinalVol;
    
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






//---------------------------------------------------------------------------------------------------------------
/*
this function and the next function "FillBoldList" are to fill Prev Bariatric, non-Bariatric and Adverse Events lists
*/
function FillBoldLists(){
    FillBoldList(document.forms[0].cmbMedication_SystemCodeList, document.forms[0].listMedication);
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
        case "listMedication_Selected" :
            txtObject = document.forms[0].txtMedication_Selected;
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
function btnCommentOnly_onclick(chkCommentOnly){
    if(document.forms[0].txtHSuperBill.value == "1"){
        $get("divbtnSuperBill").style.display = chkCommentOnly.checked ? "none" : "block";
    }
    
    $get("lblBMI").style.visibility = chkCommentOnly.checked ? "hidden" : "visible";
    $get("lblBMI_Value").style.visibility = chkCommentOnly.checked ? "hidden" : "visible";
    $get("lblLossLastVisit").style.visibility = chkCommentOnly.checked ? "hidden" : "visible";
    $get("lblLossLastVisit_Value").style.visibility = chkCommentOnly.checked ? "hidden" : "visible";
    $get("lblTotalLoss").style.visibility = chkCommentOnly.checked ? "hidden" : "visible";
    $get("lblTotalLoss_Value").style.visibility = chkCommentOnly.checked ? "hidden" : "visible";
    $get("lblEWLPercentage").style.visibility = chkCommentOnly.checked ? "hidden" : "visible";
    $get("lblEWLPercentage_Value").style.visibility = chkCommentOnly.checked ? "hidden" : "visible";
    $get("lblWeeks").style.visibility = chkCommentOnly.checked ? "hidden" : "visible";
    $get("lblWeeks_Value").style.visibility = chkCommentOnly.checked ? "hidden" : "visible";
    
    $get("tdWeight").style.visibility = chkCommentOnly.checked ? "hidden" : "visible";
    $get("tdHeight").style.visibility = chkCommentOnly.checked ? "hidden" : "visible";
    $get("tdRV").style.visibility = chkCommentOnly.checked ? "hidden" : "visible";
    $get("tdNextVisit").style.visibility = chkCommentOnly.checked ? "hidden" : "visible";
    $get("tdHeightValue").style.visibility = chkCommentOnly.checked ? "hidden" : "visible";
    $get("tdWeightValue").style.visibility = chkCommentOnly.checked ? "hidden" : "visible";
    $get("tdRVValue").style.visibility = chkCommentOnly.checked ? "hidden" : "visible";
    $get("tdNextVisitValue").style.visibility = chkCommentOnly.checked ? "hidden" : "visible";
    $get("divAddInfo1").style.display = chkCommentOnly.checked ? "none" : "block";
    $get("divAddInfo2").style.display = chkCommentOnly.checked ? "none" : "block";
    
    //$get("divRegistry").style.display = chkCommentOnly.checked ? "none" : "block";
    CheckRegistryData();
    
}


//on add visit, check for the current date, if the monthgroup already within
//if not, display, if yes, hide
function CheckRegistryData()
{
    $get("divRegistry").style.display = "none";
    $get("divRegistryNote").style.display = "none";
    if(document.forms[0].chkCommentOnly.checked == false)
    {
        if(document.getElementById("txtHInitRegistrySleepApnea").value.toUpperCase() == "YES" || document.getElementById("txtHInitRegistryGerd").value.toUpperCase() == "YES" || document.getElementById("txtHInitRegistryHyperlipidemia").value.toUpperCase() == "YES" || document.getElementById("txtHInitRegistryDiabetes").value.toUpperCase() == "YES"|| document.getElementById("txtHInitRegistryHypertension").value.toUpperCase() == "YES")
        {
            if(document.getElementById("chkRegistryReview").checked)
            {
                $get("divRegistry").style.display = "block";
            }
            else
            {
                if((monthGroup>=5 && monthGroup <=8)||(monthGroup>=9 && monthGroup <=15)||monthGroup>=18)
                {
                    //if current month group not in list
                    if(registryDone.indexOf("*"+monthGroup+"*") == -1){
                        $get("divRegistry").style.display = "block";
                        $get("divRegistryNote").style.display = "block";
                    }
                }
            }
            if($get("divRegistry").style.display == "block")
            {
                CheckReviewRegistry();
                if(document.getElementById("txtHInitRegistrySleepApnea").value.toUpperCase() == "YES")
                {
                    $get("lblRegistrySleepApnea").style.display = "block";
                    $get("selRegistrySleepApnea").style.display = "block";
                }
                    
                if(document.getElementById("txtHInitRegistryGerd").value.toUpperCase() == "YES")
                {
                    $get("lblRegistryGerd").style.display = "block";
                    $get("selRegistryGerd").style.display = "block";
                }
                
                if(document.getElementById("txtHInitRegistryHyperlipidemia").value.toUpperCase() == "YES")
                {
                    $get("lblRegistryHyperlipidemia").style.display = "block";
                    $get("selRegistryHyperlipidemia").style.display = "block";
                }
                
                if(document.getElementById("txtHInitRegistryHypertension").value.toUpperCase() == "YES")
                {
                    $get("lblRegistryHypertension").style.display = "block";
                    $get("selRegistryHypertension").style.display = "block";
                }
                
                if(document.getElementById("txtHInitRegistryDiabetes").value.toUpperCase() == "YES")
                {
                    $get("lblRegistryDiabetes").style.display = "block";
                    $get("selRegistryDiabetes").style.display = "block";
                }
            }
        }
        else
        {
            $get("divRegistry").style.display = "none";
            $get("divRegistryNote").style.display = "none";
        }
    }
}
function CheckReviewRegistry()
{
    $get("divRegistryReview").style.display = document.getElementById("chkRegistryReview").checked ? "block" : "none";               
}

function AddRegistryDone(newMonthGroup)
{
    registryDone = registryDone + "*" + newMonthGroup + "*";
    document.getElementById("txtHRegistryDone").value = registryDone;
}

function checkRegistryDiabetes(){
    //only show diabetes detail if its not a comment
    if(document.forms[0].chkCommentOnly.checked == false){
        var dtDateSeen = convertDateFromString($get("txtDateSeen_PN_txtGlobal").value);
        var dtRangeMin = convertDateFromString($get("txtHDiabetesPeriodMin").value);
        var dtRangeMax = convertDateFromString($get("txtHDiabetesPeriodMax").value);
        
        var dtDateFirstVisit = convertDateFromString($get("txtHDateFirstVisit").value);
        
        if((dtDateSeen > dtRangeMin && dtDateSeen <= dtRangeMax) || dtDateSeen <= dtDateFirstVisit || $get("txtHDateFirstVisit").value == "")
        {
            $get("div_diabetesDetail").style.display = "block";
            
            if($get("rbRegistryDiabetesY").checked == true){
                $get("lblRegistryTreatment").style.visibility = 'visible';
                $get("chkRegistryDiet").style.visibility = 'visible';
                $get("lblRegistryDiet").style.visibility = 'visible';
                $get("chkRegistryOral").style.visibility = 'visible';
                $get("lblRegistryOral").style.visibility = 'visible';
                $get("chkRegistryInsulin").style.visibility = 'visible';
                $get("lblRegistryInsulin").style.visibility = 'visible';
                $get("chkRegistryOther").style.visibility = 'visible';
                $get("lblRegistryOther").style.visibility = 'visible';
                $get("chkRegistryCombination").style.visibility = 'visible';
                $get("lblRegistryCombination").style.visibility = 'visible';
            }else{
                $get("lblRegistryTreatment").style.visibility = 'hidden';
                $get("chkRegistryDiet").style.visibility = 'hidden';
                $get("lblRegistryDiet").style.visibility = 'hidden';
                $get("chkRegistryOral").style.visibility = 'hidden';
                $get("lblRegistryOral").style.visibility = 'hidden';
                $get("chkRegistryInsulin").style.visibility = 'hidden';
                $get("lblRegistryInsulin").style.visibility = 'hidden';
                $get("chkRegistryOther").style.visibility = 'hidden';
                $get("lblRegistryOther").style.visibility = 'hidden';
                $get("chkRegistryCombination").style.visibility = 'hidden';
                $get("lblRegistryCombination").style.visibility = 'hidden';
            }
            
            //for registry reoperation
            if(dtDateSeen > dtRangeMin && dtDateSeen <= dtRangeMax){
                $get("div_repoerationDetail").style.display = "block";
                if($get("rbRegistryReoperationY").checked == true){
                    document.getElementById("txtRegistryReoperationReason_txtGlobal").style.visibility = 'visible';
                }
                else{
                    document.getElementById("txtRegistryReoperationReason_txtGlobal").style.visibility = 'hidden';
                }
            }
        }
        else
        {
            $get("div_diabetesDetail").style.display = "none";
            $get("lblRegistryTreatment").style.visibility = 'hidden';
            $get("chkRegistryDiet").style.visibility = 'hidden';
            $get("lblRegistryDiet").style.visibility = 'hidden';
            $get("chkRegistryOral").style.visibility = 'hidden';
            $get("lblRegistryOral").style.visibility = 'hidden';
            $get("chkRegistryInsulin").style.visibility = 'hidden';
            $get("lblRegistryInsulin").style.visibility = 'hidden';
            $get("chkRegistryOther").style.visibility = 'hidden';
            $get("lblRegistryOther").style.visibility = 'hidden';
            $get("chkRegistryCombination").style.visibility = 'hidden';
            $get("lblRegistryCombination").style.visibility = 'hidden';
            
            $get("div_repoerationDetail").style.display = "none";
            $get("txtRegistryReoperationReason_txtGlobal").style.visibility = 'hidden';
        }
    }
}

function checkRegistrySE(){
    //only show se detail if its not a comment
    if(document.forms[0].chkCommentOnly.checked == false){
        var dtDateSeen = convertDateFromString($get("txtDateSeen_PN_txtGlobal").value);
        var dtRangeMin = convertDateFromString($get("txtHSEPeriodMin").value);
        var dtRangeMax = convertDateFromString($get("txtHSEPeriodMax").value);
        
        if(dtDateSeen > dtRangeMin && dtDateSeen <= dtRangeMax)
        {
            $get("div_SEDetail").style.display = "block";
            
            //$get("div_SEDetail").style.display = "block";
            if($get("rbRegistrySEY").checked == true){
                document.getElementById("cmbSEList_SystemCodeList").style.visibility = 'visible';
                document.getElementById("txtRegistrySEReason_txtGlobal").style.visibility = 'visible';
            }else{
                document.getElementById("cmbSEList_SystemCodeList").style.visibility = 'hidden';
                document.getElementById("txtRegistrySEReason_txtGlobal").style.visibility = 'hidden';
            }
        }
        else{
            $get("div_SEDetail").style.display = "none";
        }  
    } 
            
}

function convertDateFromString(detDateSeen){

    dateArr = detDateSeen.split("/");   
    
    if(GetCookie("CultureInfo") == "en-us"){
        dateSeen = dateArr[1];
        monthSeen = dateArr[0];
    }
    else{
        dateSeen = dateArr[0];
        monthSeen = dateArr[1];
    }
    yearSeen = dateArr[2];
        
    //Set the two dates
    var dtDate =new Date(yearSeen, monthSeen-1, dateSeen); //Month is 0-11 in JavaScript
    
    return dtDate;
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

function LoadAllVisit(){
    __doPostBack('btnLoadAllVisit','');
}