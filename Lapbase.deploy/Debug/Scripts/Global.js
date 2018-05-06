﻿// JScript File
var XmlHttp = null;
var XmlDoc;
var divMessage = false;
var ajax_list_MSIE = false;
var txtObject;
var txtHiddenObject;
var DocumentBody = false;
var tempColor;
var iTimerID; 

//https redirection for production site
if(window.location.protocol != "https:" && document.URL.indexOf("192.168") === -1){
    var oldURL = window.location.href
    var newURL = oldURL.replace("http:","https:");
    window.location = newURL;
}

if ( (navigator.userAgent.indexOf('MSIE') >= 0) && (navigator.userAgent.indexOf('Opera') < 0)) 
    ajax_list_MSIE = true;


//----------------------------------------------------------------------------------------------------------------
//Creating and setting the instance of appropriate XMLHTTP Request object to a “XmlHttp” variable  
function CreateXmlHttp()
{
	//Creating object of XMLHTTP in IE
	//if (XmlHttp) return;
	try
	{
		XmlHttp = new ActiveXObject("Msxml2.XMLHTTP");
	}
	catch(e)
	{
		try
		{
			XmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
		} 
		catch(oc)
		{
			XmlHttp = null;
		}
	}
	//Creating object of XMLHTTP in Mozilla and Safari 
	if(!XmlHttp && typeof XMLHttpRequest != "undefined") 
	{
		XmlHttp = new XMLHttpRequest();
	}
}

//---------------------------------------------------------------------------------------------------------------
function XmlHttpSubmit(requestURL, Function_CallBack){
    CreateXmlHttp(); 
    if(XmlHttp)  
    { 
        XmlHttp.onreadystatechange = Function_CallBack;  
        XmlHttp.open("GET", requestURL ,  true);  
        XmlHttp.send(null);
    }
    return;
}

//----------------------------------------------------------------------------------------------------------------
function SubmitSOAPXmlHttp(strSOAP, Function_CallBack, strMethodURL, strSOAPAction){
    var xmlSOAP;
    
    CreateXmlHttp(); 
    if (document.all){
        xmlSOAP = new ActiveXObject("MSXML2.DOMDocument");
        xmlSOAP.loadXML(strSOAP);
    }
    else
    {
        var DomParser = new DOMParser();
        xmlSOAP = document.implementation.createDocument("", "", null);
        xmlSOAP = DomParser.parseFromString(strSOAP , "text/xml");
    }
    
    if (Function_CallBack != null)
        XmlHttp.onreadystatechange = Function_CallBack;  
    XmlHttp.open("POST",strMethodURL ,true);
    XmlHttp.setRequestHeader("SOAPAction", strSOAPAction);
    XmlHttp.setRequestHeader("Content-Type", "text/xml; charset=utf-8");
    XmlHttp.send(xmlSOAP);
}

//----------------------------------------------------------------------------------------------------------------
function CreateXmlDocument(){
    if (window.ActiveXObject) // code for IE
    {
        XmlDoc = new ActiveXObject("Microsoft.XMLDOM");
        XmlDoc.async = false;
        flag = XmlDoc.loadXML("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?>" + XmlHttp.responseText);
    }
    else // code for Mozilla, etc.
    {
        var DomParser = new DOMParser();
        XmlDoc = DomParser.parseFromString("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?>" + XmlHttp.responseText , "text/xml");
    }
}

//----------------------------------------------------------------------------------------------------------------
function ReCreateXmlDocument(strXmlDoc){
    if (window.ActiveXObject) // code for IE
    {
        XmlDoc = new ActiveXObject("Microsoft.XMLDOM");
        XmlDoc.async = false;
        flag = XmlDoc.loadXML("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?>" + strXmlDoc);
    }
    else // code for Mozilla, etc.
    {
        var DomParser = new DOMParser();
        XmlDoc = DomParser.parseFromString("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?>" + strXmlDoc , "text/xml");
    }
}

//---------------------------------------------------------------------------------------------------------------
function ComputeTargetWeight_EWV(EWV, StartWeight, IdealWeight){
    var intEW = 0;
    
    try{
        if (EWV == 66){
            intEW = parseInt (2 * (StartWeight - IdealWeight) / 3);
        }
        else{
            intEW = parseInt((EWV * (StartWeight - IdealWeight)) / 100);
        }
        
        intEW = StartWeight - intEW;
        //return(StartWeight - intEW);
        return (intEW.toFixed(1));
    }
    catch(e){return(0);}
    return;
}

//---------------------------------------------------------------------------------------------------------------
function ComputeTargetWeight_BMI(BMI, BMIHeight, UseImperial){
    var TargetWeight = new Number();
    
    try{
        TargetWeight = BMI * Math.pow(BMIHeight, 2);
        if (UseImperial == 1)
            TargetWeight = parseFloat(TargetWeight / 0.45359237);
        TargetWeight = parseInt(TargetWeight);
    }
    catch(e){alert (e.message); TargetWeight = 0;}
    return (TargetWeight);
}

//---------------------------------------------------------------------------------------------------------------
function SelectDropDownItemByText(DropDownList, txtOption){
    for (Idx = 0;Idx < DropDownList.options.length; Idx++){
        if (DropDownList.options[Idx].text == txtOption){
            DropDownList.selectedIndex = Idx;
            return;
        }
    }
}

//---------------------------------------------------------------------------------------------------------------
function SetCursor(CursorStyle){
    document.body.style.cursor = CursorStyle;
}

//---------------------------------------------------------------------------------------------------------------
function RemoveDecimalDigits(intValue)
{
    var strValue = new String(), Idx = 0;
    
    strValue = intValue.toString();
    Idx = strValue.indexOf(".");
    if (Idx > 0)
        return(strValue.substr(0, strValue.indexOf(".")));
    else
        return(strValue);
}

//-----------------------------------------------------------------------------------------------------------------
function ShowDivMessage(strMessage, AutoHidden)
{
    SetCursor("wait");
    
    if (! divMessage){
        divMessage = document.createElement("DIV");
        divMessage.id = "divMessage";
	    document.body.appendChild(divMessage);
	}
	else
	    document.getElementById('divMessage').style.display = 'block';
    	
	SetInnerText(divMessage, strMessage);
	if (AutoHidden)
	    window.setTimeout("javascript:HideDivMessage();", 2000);
    return;
}

//-----------------------------------------------------------------------------------------------------------------
function HideDivMessage(){
    if (divMessage){
        divMessage.style.display = "none";
        divMessage.style.visibility = "hidden";
        divMessage = false;
    }
    SetCursor("");
}

//----------------------------------------------------------------------------------------------------------------
// this function loads all fields caption based on Selected Language in logon form
function FetchFieldsCaption(documentBody, strCulture, strFormID)
{
    var requestURL = document.getElementById("txtHApplicationURL").value + "Forms/GlobalForms/GlobalPage.aspx?QSN=LFC&Culture=" + strCulture+"&FormID="+ strFormID;
    //DocumentBody = documentBody;
    XmlHttpSubmit(requestURL,  FetchFieldsCaption_CallBack);
}

//-------------------------------------------------------------------------------------------------------------
function FetchFieldsCaption_CallBack()
{
    var intChildQty = 0, Obj;
    
    if(XmlHttp.readyState == 4)  {
        if(XmlHttp.status == 200)  
        try{
            if (document.getElementById("TitleLoaded")) document.getElementById("TitleLoaded").value = "1";
            CreateXmlDocument();
            if (document.all)  
                intChildQty = XmlDoc.documentElement.childNodes.length;
            else
                intChildQty = parseInt(XmlDoc.documentElement.childNodes.length / 2);
            
            if (DocumentBody) DocumentBody.style.direction = XmlDoc.getElementsByTagName("strDirection")[0].firstChild.data;
            if (!HasHistory("tblAPPSCHEMA_CODE")) return;// No Visit History
            
            for (Xh = 0; Xh < intChildQty; Xh++)
                try{
                    Obj = document.getElementById(XmlDoc.getElementsByTagName("Field_ID")[Xh].firstChild.data);
                    
                    if (XmlDoc.getElementsByTagName("FIELD_CAPTION")[Xh].hasChildNodes()){
                        switch(Obj.tagName.toUpperCase())
                        {
                            case "INPUT" : 
                                document.getElementById(XmlDoc.getElementsByTagName("Field_ID")[Xh].firstChild.data).value = XmlDoc.getElementsByTagName("FIELD_CAPTION")[Xh].firstChild.data;
                                break;
                            default :
                                SetInnerText(document.getElementById(XmlDoc.getElementsByTagName("Field_ID")[Xh].firstChild.data), XmlDoc.getElementsByTagName("FIELD_CAPTION")[Xh].firstChild.data);
                                break;
                        }
                    }
                } 
                catch(e){}
                //catch(e){alert(XmlDoc.getElementsByTagName("Field_ID")[Xh].firstChild.data + "\n" + e.message);}
        } // try
        catch(e){}
    }
    return;
}

//-----------------------------------------------------------------------------------------------------------------------
function HasHistory(strFieldName){
    if (XmlDoc.getElementsByTagName(strFieldName).length == 0) return(false);
    if (XmlDoc.getElementsByTagName(strFieldName)[0].hasChildNodes()) 
        if ((XmlDoc.getElementsByTagName(strFieldName)[0].firstChild.nodeValue == "0") || (XmlDoc.getElementsByTagName(strFieldName)[0].firstChild.nodeValue == ""))
            return(false);
    return(true);
}

//-----------------------------------------------------------------------------------------------------------------------
function SetInnerText(obj, strText){
    if (document.all)
        obj.innerText = strText;
    else
        obj.textContent = strText;
}

//-----------------------------------------------------------------------------------------------------------------------
function findPosX(obj)
{
    var curleft = 0;
    if(obj.offsetParent)
        while(1) 
        {
          curleft += obj.offsetLeft;
          if(!obj.offsetParent)
            break;
          obj = obj.offsetParent;
        }
    else if(obj.x)
        curleft += obj.x;
    return curleft;
}

//-----------------------------------------------------------------------------------------------------------------------
function findPosY(obj)
{
    var curtop = 0;
    if(obj.offsetParent)
        while(1)
        {
          curtop += obj.offsetTop;
          if(!obj.offsetParent)
            break;
          obj = obj.offsetParent;
        }
    else if(obj.y)
        curtop += obj.y;
    return curtop;
}

//---------------------------------------------------------------------------------------------------------------
function LoadingFloodingDiv(strFileName){
    var FileType = CheckFileType(strFileName);
    
    switch(FileType){
        case 1 :    // Image Files
            LoadFloodingDiv_ImageVideo(strFileName, FileType);
            break;
            
        case 2 :    // Video Files
            if (strFileName.toLowerCase().indexOf(".flv") > -1)
                LoadFloodingDiv_ImageVideo(strFileName, FileType);
            else
                window.open(strFileName, "","titlebar=0,toolbar=0,fullscreen=0,resizable=1");
            break;
        case 3 :    // other Files
            window.open(strFileName, "","titlebar=0,toolbar=0,fullscreen=0,resizable=1");
            break;
    }
	return;
}

//---------------------------------------------------------------------------------------------------------------
function LoadFloodingDiv_ImageVideo(strFileName, FileType){
    var divWidth = 650;
    var divHeight = 450;
    var tblDoc = document.createElement("table"), trRow, tcCell, 
        btnClose = document.createElement("input"), divDoc = document.createElement("DIV");
    
	if ( !floodDiv ){
		floodDiv = document.createElement('DIV');
		floodDiv.id = 'floodDiv';
		floodDiv.style.width = divWidth + "px";
		floodDiv.style.height = divHeight + "px";
		floodDiv.style.verticalAlign = "middle";
		floodDiv.style.textAlign = "center";
		document.body.appendChild(floodDiv);
		
		if(document.all){
			floodDiv_iframe = document.createElement('IFRAME');
			floodDiv_iframe.border='0';
			floodDiv_iframe.style.width = floodDiv.clientWidth + 'px';
			floodDiv_iframe.style.height = floodDiv.clientHeight + 'px';
			floodDiv_iframe.id = 'floodDiv_iframe';
			document.body.appendChild(floodDiv_iframe);
		}
	}
	
	if (window.innerHeight) {
        floodDiv.style.top = String((window.innerHeight - divHeight)/2)+ "px";
        floodDiv.style.left = String((window.innerWidth - divWidth)/2) + "px";
    }
    else{
        floodDiv.style.top = String((document.documentElement.clientHeight - divHeight)/2) + "px";
        floodDiv.style.left = String((document.documentElement.clientWidth - divWidth)/2) + "px";
    }
	if(floodDiv_iframe){
		floodDiv_iframe.style.left = floodDiv.style.left;
		floodDiv_iframe.style.top = floodDiv.style.top;
	}
	
	tblDoc.border = "0";
	tblDoc.style.width = (divWidth-5) + "px";
	tblDoc.style.hiegth = (divHeight-5) + "px";
	trRow = tblDoc.insertRow(tblDoc.rows.length);
    tcCell = trRow.insertCell(trRow.cells.length);
    tcCell.style.textAlign = "right";
    btnClose.type = "button";
    btnClose.value = "Close";
    btnClose.onclick = function(){
        floodDiv.style.visibility = "hidden";
        floodDiv.style.display = "none";
        floodDiv = false;
        if(floodDiv_iframe){
            floodDiv_iframe.style.visibility = "hidden";
            floodDiv_iframe.style.display = "none";
            floodDiv_iframe = false;
        }
        
            divDoc.removeChild(pFLV);
    }

    tcCell.appendChild(btnClose);
    trRow = tblDoc.insertRow(tblDoc.rows.length);
    tcCell = trRow.insertCell(trRow.cells.length);
    tcCell.style.textAlign = "center";
    //tcCell.style.verticalAlign = "center"; // this command doesn't work in IE
    divDoc.style.width = "100%";
    divDoc.style.height = "100%";
    //switch(parseInt(document.getElementById("txtHDocumentTypeID").value))
    //switch(CheckFileType(strFileName))
    switch(FileType)
    {
        case 1 : // Image Document
            var imgDoc = document.createElement("img");
            imgDoc.src = strFileName;
            imgDoc.style.width = (divWidth-30) + "px";
            divDoc.appendChild(imgDoc);
            break;
            
        case 2 : // Movie Document        
            var pFLV = document.createElement("P");
            pFLV.id = "FLVPlayer";
            divDoc.appendChild(pFLV);
            var FO = {	movie:"flvplayer.swf",width:"500",height:"400",majorversion:"7",build:"0",bgcolor:"#FFFFFF",
                flashvars:"file=" + strFileName + "&showdigits=true&autostart=false&showfsbutton=false" };
            UFO.create(	FO, "FLVPlayer");
            break;
    }
    tcCell.appendChild(divDoc);
    
	floodDiv.appendChild(tblDoc);
    return;
}


//---------------------------------------------------------------------------------------------------------------
function CheckFileType(strFileName){
    var strExtention = strFileName.substring(strFileName.lastIndexOf(".") + 1);
    var intFileType = new Number();
    
    switch(strExtention.toUpperCase()){
        case "AVI" :
        case "MP4" :
        case "MPEG4" :
        case "MPG" :
        case "FLV" :
            intFileType = 2;
            break;
        case "BMP":
        case "JPG":
        case "JPEG":
        case "GIF":
        case "TIFF":
        case "PNG":
            intFileType = 1;
            break;
        case "PDF":
        case "DOC":
        case "DOCX":
        case "XLS":
        case "XLSX":
        case "TXT":
            intFileType = 3;
            break;
        default:
            intFileType = 0;
            break;
    }
    
    return(intFileType);
}

//--------------------------------------------------------------------------------------------------------------
function TableRow_onmouseover(objRow){
    tempColor = objRow.style.backgroundColor;
    objRow.style.backgroundColor = "#ccc";
    objRow.style.cursor = "pointer";
}

//--------------------------------------------------------------------------------------------------------------
function TableRow_onmouseout(objRow){
    objRow.style.backgroundColor = tempColor;
    objRow.style.cursor = "";
}

//----------------------------------------------------------------------------------------------------------------
function RowTable_onmouseover(tblRowID)
{
    var tblRowID = document.getElementById(tblRowID);
    
    tempColor = tblRowID.style.backgroundColor;
    tblRowID.style.backgroundColor = "#ccc";
    tblRowID.style.cursor = "pointer";
    
    if (tblRowID.rows.length == 1) return;
    if (document.all) // IE
    {
        if (tblRowID.rows[1].cells[0].innerText != "")
            tblRowID.rows[1].cells[0].style.backgroundColor = "#999";
    }
    else
    {
        if (tblRowID.rows[1].cells[0].textContent != "")
            tblRowID.rows[1].cells[0].style.backgroundColor = "#999";
    }
    return;
}

//----------------------------------------------------------------------------------------------------------------
function RowTable_onmouseout(tblRowID){
    var tblRowID = document.getElementById(tblRowID);
    
    tblRowID.style.backgroundColor = tempColor;
    tblRowID.style.cursor = "";
    
    if (tblRowID.rows.length == 1) return;
    if (document.all) // IE
    {
        if (tblRowID.rows[1].cells[0].innerText != "")
            tblRowID.rows[1].cells[0].style.backgroundColor = "#999";
        else
            tblRowID.rows[1].cells[0].style.backgroundColor = "";
    }
    else
    {
        if (tblRowID.rows[1].cells[0].textContent != "")
            tblRowID.rows[1].cells[0].style.backgroundColor = "#999";
        else
            tblRowID.rows[1].cells[0].style.backgroundColor = "";
    }
    return;
}

//----------------------------------------------------------------------------------------------------------------
function SetPatientTitleEvents(){
    return;
}

//------------------------------------------------------------------------------------------------
function InitializePage(strAppAchemaID){
    if (strAppAchemaID == 'frmUploadDocument'){
        
        if (document.getElementById("txtHConsultID")) // check the parentcode is 
            document.getElementById("txtHEventID").value = document.getElementById("txtHConsultID").value;
    }
    iTimerID = window.setInterval("IsTitlesLoaded( );", 100);
    //FetchFieldsCaption(false,  document.getElementById("txtHCulture").value, strAppAchemaID);
}

//----------------------------------------------------------------------------------------------------------------
function IsTitlesLoaded(){
    if (document.getElementById("TitleLoaded").value == "1")
        window.clearInterval(iTimerID);
    return;
}

//------------------------------------------------------------------------------------------------
function InitializePage_FloatingDiv(strAppAchemaID){
    iTimerID = window.setInterval("IsTitlesLoaded_FloatingDive( );", 100);
    FetchFieldsCaption(false,  document.getElementById("txtHCulture").value, strAppAchemaID);
}

//----------------------------------------------------------------------------------------------------------------
function IsTitlesLoaded_FloatingDive(){
    if (document.getElementById("TitleLoaded").value == "1")
        window.clearInterval(iTimerID);
}

//----------------------------------------------------------------------------------------------------------------
function IsFloadingDivLoaded(strAppAchemaID){
    iTimerID = window.setInterval("FloadingDivLoaded('" + strAppAchemaID + "')", 100);
}

//----------------------------------------------------------------------------------------------------------------
function FloadingDivLoaded(strAppAchemaID){
    if (document.getElementById(strAppAchemaID)){
        window.clearInterval(iTimerID);
        iTimerID = false;
        InitializePage_FloatingDiv(strAppAchemaID);
    }
}

//----------------------------------------------------------------------------------------------------------------
function ShowErrorMessageDiv(strDisplay, strErrorMessage){
    document.getElementById("divErrorMessage").style.display = strDisplay;
    SetInnerText(document.getElementById("pErrorMessage"), strErrorMessage);
}

//----------------------------------------------------------------------------------------------------------------
function GetClientDate(strCultureInfo){
    //get client date
    var DateTime = new Date();
    var strYear= DateTime.getFullYear();
    var strMonth= DateTime.getMonth() +1;
    var strDay = DateTime.getDate();
    var returnDate = "";
    
    if(strCultureInfo == "en-us"){
        returnDate = strMonth + "/" + strDay + "/" + strYear;
    }
    else{
        returnDate = strDay + "/" + strMonth + "/" + strYear;
    }
    return returnDate;
}