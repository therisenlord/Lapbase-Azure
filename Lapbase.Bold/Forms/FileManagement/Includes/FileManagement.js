﻿// JScript File 
var intDocumentColQty = 4;
var FileManagementSubMenu_array = new Array("subContent_mnuItem01", "subContent_mnuItem02");
var ShowBy_array = new Array("tdType", "tdTypeDate", "tdDate"); 
var LoadingType = "Thumbnail";
var floodDiv = false;
var floodDiv_iframe = false;
var IsFileDeleted = false;


//------------------------------------------------------------------------------------------------
function InitializePage(strShowby){
    
    iTimerID = window.setInterval("IsTitlesLoaded('" + strShowby + "');", 1000,"javascript");
    FetchFieldsCaption(false,  document.getElementById("txtHCulture").value, document.frmFileManagement.name);
    
    document.getElementById("AppSchemaMenu_a_Files").href = "#";
    document.getElementById("show_by").value = strShowby.toUpperCase();
    ShowBy(strShowby);
}

//----------------------------------------------------------------------------------------------------------------
function IsTitlesLoaded(strShowby){
    if (document.getElementById("TitleLoaded").value == "1"){
        window.clearInterval(iTimerID);
        document.getElementById("AppSchemaMenu_a_Files").href = "#";
        document.getElementById("show_by").value = strShowby.toUpperCase();
        ShowBy(strShowby);
    }
    return;
}


//------------------------------------------------------------------------------------------------
function FileManagementSubMenu_onclick(IdxButton)
{
    var DocumentType, EventDate;
    
    SetFileManagementSubMenuButton(IdxButton);
    DocumentType = parseInt(document.getElementById("txtHDocumentTypeID").value);
    EventDate = document.getElementById("txtHEventDate").value;
    
    if (EventDate == "")
        LoadDocumentItems(DocumentType);
    else if (EventDate != "")
            LoadDocumentItems_SpecificDate(DocumentType, EventDate);
    return;
}

//------------------------------------------------------------------------------------------------
function SetFileManagementSubMenuButton(IdxButton){
    document.getElementById("li_Thumbnails").className = "";
    document.getElementById("li_Details").className = "";
    switch(IdxButton)
    {
        case 0 :
            LoadingType = "Thumbnail";
            document.getElementById("li_Thumbnails").className = "current";
            break;
        case 1 :
            LoadingType = "Detail";
            document.getElementById("li_Details").className = "current";
            break;
    }
    return;
}

//------------------------------------------------------------------------------------------------
function ShowBy(strReportType){
    var requestURL = document.getElementById("txtHApplicationURL").value + "Forms/FileManagement/FileManagementForm.aspx?ReLoad=0&QSN="+strReportType;
    requestURL += "&SD=" + document.getElementById("txtShowDeleted").value;
    
    ShowDivMessage("Loadings files...", false);
    document.getElementById("txtHDocumentTypeID").value = "0";
    document.getElementById("txtHEventDate").value = "";
    document.getElementById("txtHEventLink").value = "";
    document.getElementById("txtHShowBy").value = strReportType;
    SetUploadURLParameters();
    XmlHttpSubmit(requestURL,  ShowBy_callback);
}

//------------------------------------------------------------------------------------------------
function ShowBy_callback(){
    if (XmlHttp.readyState == 4){
        document.getElementById("txtTest").value = XmlHttp.responseText;
        if (XmlHttp.status == 200){
            HideDivMessage();
            document.getElementById("div_DocumentsList").style.display = "block";
            document.getElementById("div_DocumentsList").innerHTML = String(XmlHttp.responseText);
            ResizeDetailDiv(document.getElementById("div_DocumentsList"));
            
            if (document.getElementById("txtFirstType"))  // by load the first document type files 
                LoadDocumentItems(document.getElementById("txtFirstType").value);
        }
        else
        {
            document.getElementById("div_DocumentsList").innerHTML = "";
            document.getElementById("div_DocumentsList").style.display = "none";
        }
    }
    return;
}

//------------------------------------------------------------------------------------------------
function ShowDeletedFile(DeleteFlag){
    document.getElementById("txtShowDeleted").value = DeleteFlag;
    ShowBy(document.getElementById("txtHShowBy").value);
}

//------------------------------------------------------------------------------------------------
function DocumentTypeCaption_onclick(oThis, intDocumentTypeID)
{
    SetInnerText(document.getElementById("div_DocumentTitle"), document.all ? oThis.innerText : oThis.textContent);
    LoadDocumentItems(intDocumentTypeID);
}


//------------------------------------------------------------------------------------------------
function LoadDocumentItems(intDocumentTypeID)
{
    var requestURL = document.getElementById("txtHApplicationURL").value + "Forms/FileManagement/FileManagementForm.aspx?ReLoad=0&QSN=LOADDOCUMENTITEMS&QSV="+String(intDocumentTypeID);
    requestURL += "&LT=" + LoadingType + "&SD=" + document.getElementById("txtShowDeleted").value;;
    
    document.getElementById("txtHDocumentTypeID").value = intDocumentTypeID;
    document.getElementById("txtHEventDate").value = "";
    
    ShowDivMessage("Loading Patient's Documents ...", false);
    XmlHttpSubmit(requestURL,  LoadDocumentItems_callback);
}

//------------------------------------------------------------------------------------------------
function LoadDocumentItems_callback()
{
    ChangeRightFrameSetting(false);
    if (XmlHttp.readyState == 4)
        if (XmlHttp.status == 200){
            SetFileManagementSubMenuButton((LoadingType == "Thumbnail") ? 0 : 1);
            document.getElementById("div_DocumentItemsList").style.display = "block";
            CreateXmlDocument();
            try {
                var obj = false;
                document.getElementById("div_DocumentItemsList_Thumbnail").style.display = (LoadingType == "Thumbnail") ? "block" : "none";
                document.getElementById("div_DocumentItemsList_Detail").style.display = (LoadingType == "Thumbnail") ? "none" : "block";
                
                if (LoadingType == "Thumbnail") {
                    document.getElementById("txtTest").value = XmlHttp.responseText;
                    MakeDocumentItems_RightFrame();
                    obj = document.getElementById("div_DocumentItemsList_Thumbnail");
                }
                else{
                    obj = document.getElementById("div_DocumentItemsList_Detail");
                    document.getElementById("div_DocumentItemsList_Detail").innerHTML = XmlHttp.responseText;
                }
                HideDivMessage();
                ResizeDetailDiv(obj);
            }catch (e){};
        }
        else
            document.getElementById("div_DocumentItemsList").style.display = "none";
    return;
}

//------------------------------------------------------------------------------------------------
function ResizeDetailDiv(obj)
{
    if (window.innerHeight) // 22 is height of lower red controlbar
        obj.style.height = String(window.innerHeight - findPosY(obj) - 22) + "px";
    else
        obj.style.height = String(document.documentElement.clientHeight - findPosY(obj) - 22) + "px";
    return;
}

//------------------------------------------------------------------------------------------------
function ChangeRightFrameSetting(viewstat)
{
    document.getElementById("div_DocumentItemsList").style.display = viewstat ? "block" : "none";
    document.getElementById("div_DocumentItemsDetailInformation").style.display = viewstat ? "block" : "none";
    return;
}

//------------------------------------------------------------------------------------------------
function MakeDocumentItems_RightFrame(){
    var intChildQty = new Number(),
        tblDocumentItems = document.getElementById("tbl_DocumentItemsList"), trRow, tcCell, 
        div_DocumentItemsList = document.getElementById("div_DocumentItemsList_Thumbnail");
    var strDocumentFileName;
    
    if (document.all)  
        intChildQty = XmlDoc.documentElement.childNodes.length;
    else
        intChildQty = parseInt(XmlDoc.documentElement.childNodes.length / 2);
    
    while (tblDocumentItems.rows.length > 0) tblDocumentItems.deleteRow(0);
    if (intChildQty == 0)   return;
    
    SetInnerText(document.getElementById("div_DocumentTitle"), XmlDoc.getElementsByTagName("Type_Name")[0].firstChild.nodeValue);
    tblDocumentItems.style.width = ((intChildQty > intDocumentColQty) ? String(100 * intDocumentColQty) : String(100 * intChildQty)) + "px";
    for(Xh = 0; Xh < intChildQty; Xh++){
        var tempTable = document.createElement("Table"), 
            tempTR, tempTC, tempDIV = null, tempSpan, tempLink, tempObj, IsDeleted = new Boolean();
        
        if (Xh % intDocumentColQty == 0){
            trRow = tblDocumentItems.insertRow(tblDocumentItems.rows.length);
            trRow.style.verticalAlign = "top";
        }
        
        tcCell = trRow.insertCell(trRow.cells.length);
        tcCell.style.textAlign = "center";
        tcCell.style.width = String(100 / intDocumentColQty) + "%";
        
        tempTable.border = "0";
        tempTable.style.width = "100%";
        tempTR = tempTable.insertRow(tempTable.rows.length);
        
        tempTC = tempTR.insertCell(tempTR.cells.length);
        tempTC.style.width = "15%";
        
        tempTC = tempTR.insertCell(tempTR.cells.length);
        tempTC.style.width = "70%";
        
        tempDIV = document.createElement("Div");
        if (XmlDoc.getElementsByTagName("Doc_Description")[Xh].hasChildNodes()) 
            tempDIV.title = XmlDoc.getElementsByTagName("Doc_Description")[Xh].firstChild.nodeValue;
        else
            tempDIV.title = "";
        tempDIV.style.width = "100px";
        tempDIV.style.height = "100px";
        tempDIV.style.borderStyle = "solid";
        tempDIV.style.borderColor = "black";
        tempDIV.style.borderWidth = "1pt";
        tempDIV.style.textAlign = "center";
        strDocumentFileName = XmlDoc.getElementsByTagName("DocumentFileName")[Xh].firstChild.nodeValue;
        tempDIV.innerHTML = "<br /><br />" + strDocumentFileName.substring(strDocumentFileName.lastIndexOf(".")+1);
        tempDIV.id = XmlDoc.getElementsByTagName("tblPatientDocumentsID")[Xh].firstChild.nodeValue + "_";
        tempDIV.id += XmlDoc.getElementsByTagName("IsDeleted")[Xh].firstChild.nodeValue;
        
        tempDIV.onclick = function(){
            DocumentDiv_onclick(this.id);
        }
        tempDIV.onmouseover = function(){
            this.style.cursor = "pointer";
        }
        tempDIV.onmouseout = function(){
            this.style.cursor = "";
        }
        
        tempTC.appendChild(tempDIV);
        
        tempTC = tempTR.insertCell(tempTR.cells.length);
        tempTC.style.width = "15%";
        tcCell.appendChild(tempTable);
        
        tempLink = document.createElement("A");
        tempLink.href = "#";
        if (XmlDoc.getElementsByTagName("DocumentName")[Xh].hasChildNodes()) 
            SetInnerText(tempLink, XmlDoc.getElementsByTagName("DocumentName")[Xh].firstChild.nodeValue);
        else
            SetInnerText(tempLink, XmlDoc.getElementsByTagName("DocumentFileName")[Xh].firstChild.nodeValue);
            
            
        tempLink.id = XmlDoc.getElementsByTagName("tblPatientDocumentsID")[Xh].firstChild.nodeValue + "_LargeSize";        
        tempLink.name = XmlDoc.getElementsByTagName("DocumentFileName")[Xh].firstChild.nodeValue;
        
        tempLink.onclick = function()
        {
            setDocumentName(this.name);
            DocumentName_onclick(this.id);
        }
        tcCell.appendChild(tempLink);
        tcCell.appendChild(document.createElement("BR"));
        /** /
        tempLink = document.createElement("A");
        tempLink.href = "#";
        SetInnerText(tempLink, "Full Size");
        tempLink.id = XmlDoc.getElementsByTagName("tblPatientDocumentsID")[Xh].firstChild.nodeValue + "_FullSize";
        tempLink.onclick = function()
        {
            DocumentName_onclick(this.id);
        }
        tcCell.appendChild(tempLink);
        /**/
    }
    div_DocumentItemsList.appendChild(tblDocumentItems);
}

//---------------------------------------------------------------------------------------------------------------
function DocumentName_onclick(strDocumentID){
    if (strDocumentID.indexOf("_LargeSize") > 0) // Large Size View
    {
        strDocumentID = strDocumentID.replace("_LargeSize", "");
        linkLarge_onclick(strDocumentID);
    }
    /** /
    else if (strDocumentID.indexOf("_FullSize") > 0) // Full Size View
    {
        strDocumentID = strDocumentID.replace("_FullSize", "");
        linkFullSize_onclick(strDocumentID);
    }
    /**/
}

//---------------------------------------------------------------------------------------------------------------
function DocumentDiv_onclick(documentID){
    tempDIV_onclick(documentID.substring(documentID, documentID.indexOf("_") ), documentID.substring(documentID.indexOf("_") + 1) == "true");
}

//---------------------------------------------------------------------------------------------------------------
function tempDIV_onclick(DocumentID, IsDeleted, DocumentType){
    var requestURL = document.getElementById("txtHApplicationURL").value;
        
   // if(selected_text.indexOf('.DOCX')>= 0 || selected_text.indexOf('.XLSX')>= 0)
   //     window.open(requestURL + "Forms/FileManagement/Documents/"+cmbDocument.value+'_'+selected_text, 'download')
        
    document.getElementById("txtHDocumentID").value = DocumentID;
    requestURL += "Forms/FileManagement/FileManagementForm.aspx?ReLoad=0&QSN=LOADDOCUMENTDETAIL&QSV=";
    requestURL += DocumentID;
    IsFileDeleted = IsDeleted;
    if (!isNaN(DocumentType))
        document.getElementById("txtHDocumentTypeID").value = DocumentType;
    
    XmlHttpSubmit(requestURL,  tempDIV_onclick_callback);
}

//---------------------------------------------------------------------------------------------------------------
function tempDIV_onclick_callback(){
    ChangeRightFrameSetting(false);
    if (XmlHttp.readyState == 4)
        if (XmlHttp.status == 200){
            document.getElementById("txtTest").value = XmlHttp.responseText;
            document.getElementById("div_DocumentItemsDetailInformation").style.display = "block";
            document.getElementById("div_DocumentItemsDetailInformation").innerHTML = String(XmlHttp.responseText);
            document.getElementById("btnDelete").value = IsFileDeleted ? "Undelete" : "Delete";
        }
        else
            document.getElementById("div_DocumentItemsDetailInformation").style.display = "none";
    return;
}

//---------------------------------------------------------------------------------------------------------------
function LoadEventData(intDocumentID, EventLink){
    if (document.getElementById("div_EventDetail").style.display == "none"){
        var requestURL = document.getElementById("txtHApplicationURL").value;
        requestURL += "Forms/FileManagement/FileManagementForm.aspx?ReLoad=0&QSN=LOADEVENTDATA&QSV=" + String(intDocumentID);
        requestURL += "&EL=" + EventLink;
        
        XmlHttpSubmit(requestURL, LoadEventData_callback);
    }
    else
    {
        document.getElementById("imgEventDetails").src = "../../img/button_plus.gif";
        document.getElementById("div_EventDetail").style.display = "none";
        document.getElementById("div_EventDetail").innerHTML = "";
    }
}

//---------------------------------------------------------------------------------------------------------------
function LoadEventData_callback(){
    if (XmlHttp.readyState == 4)
        if (XmlHttp.status == 200){
            document.getElementById("imgEventDetails").src = "../../img/button_minus.gif";
            document.getElementById("div_EventDetail").style.display = "block";
            document.getElementById("div_EventDetail").innerHTML = XmlHttp.responseText;
        }
        else{
            document.getElementById("imgEventDetails").src = "../../img/button_plus.gif";
            document.getElementById("div_EventDetail").style.display = "none";
            document.getElementById("div_EventDetail").innerHTML = "";
        }
    return;
}

//---------------------------------------------------------------------------------------------------------------
function btnSaveDocument_onclick(DocumentsID){
    var strSOAP = 
            '<?xml version="1.0" encoding="utf-8"?>'+
            '<soap:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" ' + 
                            'xmlns:xsd="http://www.w3.org/2001/XMLSchema" ' + 
                            'xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">'+
	            '<soap:Body>'+
		            '<SaveDocumentDetail xmlns="http://tempuri.org/">'+
		                '<strDocumentID>' + String(DocumentsID) + '</strDocumentID>'+
			            '<strDocumentName>' + document.getElementById("txtDocumentName").value + '</strDocumentName>'+
			            '<strDoc_Description>' + document.getElementById("txtDoc_Description").value + '</strDoc_Description>'+
		            '</SaveDocumentDetail>'+
	            '</soap:Body>'+
            '</soap:Envelope>';
            
    document.getElementById("divErrorMessage").style.display = "none";
	SubmitSOAPXmlHttp(strSOAP, btnSaveDocument_onclick_callback, "Includes/FileManagementWebService.asmx", "http://tempuri.org/SaveDocumentDetail");
}

//---------------------------------------------------------------------------------------------------------------
function setDocumentName(DocumentName)
{
    document.getElementById("txtHDocumentName").value = DocumentName;
}

//---------------------------------------------------------------------------------------------------------------
function btnSaveDocument_onclick_callback()
{
    if(XmlHttp.readyState == 4){
        document.getElementById("txtTest").value = XmlHttp.responseText;
        if(XmlHttp.status == 200)  {
            var response  = XmlHttp.responseXML.documentElement, intSaveResult = 0;
                
            if (response.getElementsByTagName("ReturnValue")[0].hasChildNodes()) 
                intSaveResult = parseInt(response.getElementsByTagName('ReturnValue')[0].firstChild.data);
                
            if (parseInt(intSaveResult) == -1) 
            {
                document.getElementById("divErrorMessage").style.display = "none";
                SetInnerTime(document.getElementById("divErrorMessage"), "error in save data...");
            }
            else ShowBy(document.getElementById("show_by").value);
        }
    }
    return; 
}

//---------------------------------------------------------------------------------------------------------------
function LoadFileDetailData(strDivID, DocumentID){
    
    document.getElementById("l_FileDetails").className = (strDivID == "div_FileDetails") ? "current" : "";
    document.getElementById("l_FileHistory").className = (strDivID == "div_FileDetails") ? "" : "current";
    
    document.getElementById("div_FileDetails").style.display = (strDivID == "div_FileDetails") ? "block" : "none";
    document.getElementById("div_FileHistory").style.display = (strDivID == "div_FileDetails") ? "none" : "block";
    
    if (strDivID == "div_FileHistory")
        LoadEventHistory(DocumentID);
}

//---------------------------------------------------------------------------------------------------------------
function LoadEventHistory(DocumentID){
    var requestURL = document.getElementById("txtHApplicationURL").value + "Forms/FileManagement/FileManagementForm.aspx";
    requestURL += "?Reload=0&QSN=LOADEVENTHISTORY&QSV=" + String(DocumentID);
    XmlHttpSubmit(requestURL,  LoadEventHistory_callback);
}

//---------------------------------------------------------------------------------------------------------------
function LoadEventHistory_callback()
{
    if (XmlHttp.readyState == 4)
        if (XmlHttp.status == 200){
            document.getElementById("div_FileHistory").style.overflowY = "scroll";
            document.getElementById("div_FileHistory").innerHTML = XmlHttp.responseText;
        }
        else
        alert(XmlHttp.responseText);
    return;
}

//---------------------------------------------------------------------------------------------------------------
function LoadDocumentItems_SpecificDate(intDocumentTypeID, EventDate){
    var requestURL = document.getElementById("txtHApplicationURL").value + "Forms/FileManagement/FileManagementForm.aspx";
    requestURL += "?ReLoad=0&QSN=LOADDOCUMENTITEMS_EVENTDATE&QSV=" + String(intDocumentTypeID);
    requestURL += "&ED=" + EventDate.replace(" ", "") + "&LT=" + LoadingType + "&SD=" + document.getElementById("txtShowDeleted").value;
    
    document.getElementById("txtHDocumentTypeID").value = intDocumentTypeID;
    document.getElementById("txtHEventDate").value = EventDate;
    
    ShowDivMessage("Loading patient's documents...", false);
    XmlHttpSubmit(requestURL,  LoadDocumentItems_callback);
}

//---------------------------------------------------------------------------------------------------------------
function LoadDocumentItems_SpecificEvent(intDocumentTypeID, EventDate, EventLink){
    var requestURL = document.getElementById("txtHApplicationURL").value + "Forms/FileManagement/FileManagementForm.aspx";
    
    requestURL += "?ReLoad=0&QSN=LOADDOCUMENTITEMS_EVENTLINK&QSV=" + String(intDocumentTypeID);
    requestURL += "&ED=" + EventDate.replace(" ", "") + "&EL=" + EventLink + "&LT=" + LoadingType;
    
    document.getElementById("txtHDocumentTypeID").value = intDocumentTypeID;
    document.getElementById("txtHEventDate").value = EventDate;
    document.getElementById("txtHEventLink").value = EventLink;
    
    XmlHttpSubmit(requestURL,  LoadDocumentItems_callback);
}

//---------------------------------------------------------------------------------------------------------------
function btnUpload_onclick(){
    var strOption = "channelmode=0,menubar=0,titlebar=0,toolbar=0,scrollbars=0,resizable=0,fullscreen=0";

    //window.open("UploadDocument/UploadDocumentForm.aspx", "", strOption);
    window.open("UploadDocumentForm.aspx", "", strOption);
    return;
}

//---------------------------------------------------------------------------------------------------------------
function DeleteDocument(oThis){
    var DeleteAction = (oThis.value == "Delete"); 
    var requestURL = document.getElementById("txtHApplicationURL").value + "Forms/FileManagement/FileManagementForm.aspx";
    requestURL += "?ReLoad=0&QSN=DELETEDOCUMENT&QSV=" + document.getElementById("txtHDocumentID").value +"&AC="+DeleteAction;
    
    XmlHttpSubmit(requestURL,  DeleteDocument_callback);
    return;
}

//---------------------------------------------------------------------------------------------------------------
function DeleteDocument_callback(){
    if (XmlHttp.readyState == 4)
        if (XmlHttp.status == 200) {
            SetReadyOnly(document.getElementById("btnDelete").value == "Delete");
            ShowBy(document.getElementById("show_by").value);
        }
    return;
}

//---------------------------------------------------------------------------------------------------------------
function SetReadyOnly(IsReadOnly){
    document.getElementById("btnDelete").value = IsReadOnly ? "Undelete" : "Delete";
    SetInnerText(document.getElementById("lblDeleted"), IsReadOnly ? "<DELETED - ReadOnly>" : "");
    document.getElementById("txtDocumentName").disabled = IsReadOnly;
    document.getElementById("txtDoc_Description").disabled = IsReadOnly;
    document.getElementById("btnSaveDocument").disabled = IsReadOnly;
    return;
}

//---------------------------------------------------------------------------------------------------------------
function linkLarge_onclick(DocumentID){
    if(document.getElementById("txtHDocumentName").value.indexOf('.DOCX')>= 0 || document.getElementById("txtHDocumentName").value.indexOf('.XLSX')>= 0)
        window.open("./Documents/"+DocumentID+'_'+document.getElementById("txtHDocumentName").value, 'download')        
        
    var requestURL = document.getElementById("txtHApplicationURL").value + "Forms/FileManagement/FileManagementForm.aspx?ReLoad=0&QSN=LOADINGDOCUMENT";
    requestURL += "&DID=" + DocumentID;
    XmlHttpSubmit(requestURL,  linkLarge_onclick_callback);    
}

//---------------------------------------------------------------------------------------------------------------
function linkLarge_onclick_callback(){
    if (XmlHttp.readyState == 4)
        if (XmlHttp.status == 200)
            LoadingFloodingDiv(XmlHttp.responseText);
}

//---------------------------------------------------------------------------------------------------------------
function show_by_onchange(objShowBy){
    ShowBy(objShowBy.value);
}

//---------------------------------------------------------------------------------------------------------------
function show_files_onchange(objShowFiles){
    ShowDeletedFile(objShowFiles.value);
}

//---------------------------------------------------------------------------------------------------------------
function SetUploadURLParameters(){
    var linkUpload = document.getElementById("linkUpload");
    //linkUpload.href = "../UploadDocument/UploadDocumentForm.aspx?PCode=2&QSN=" + document.getElementById("txtHShowBy").value ;
    linkUpload.href = "UploadDocumentForm.aspx?PCode=2&QSN=" + document.getElementById("txtHShowBy").value ;
    linkUpload.href += "&SD=" + document.getElementById("txtShowDeleted").value;
    
    if (linkUpload.className != "lbOn"){
        linkUpload.className = "lbOn";
        initialize();
    }
    return;
}

//---------------------------------------------------------------------------------------------------------------
function RemoveUploadLinkSetting(){
    var linkUpload = document.getElementById("linkUpload");
    
    try{
        valid.dispose(linkUpload);
        linkUpload.className = "";
    }
    catch(e){}
}