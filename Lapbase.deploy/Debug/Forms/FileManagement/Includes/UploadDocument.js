// JScript File

//---------------------------------------------------------------------------------------------------------------
function UploadFile_onload(){
    document.getElementById("txtDate_txtGlobal").onchange = function(){
        UploadFile_txtDate_onchange();
    }
}

//---------------------------------------------------------------------------------------------------------------
function listEventName_onchange(listEventName){
    if (listEventName.value == "B"){
        document.getElementById("listEventDate").options.length = 0;
    }
    else
    {
        document.getElementById("listEventDate").selectedIndex = 0;
        document.getElementById("listEventDate").disabled = false;
        //var requestURL = document.getElementById("txtHApplicationURL").value + "Forms/UploadDocument/UploadDocumentForm.aspx?ET="+listEventName.value;
        var requestURL = "UploadDocumentForm.aspx?ET="+listEventName.value;
        XmlHttpSubmit(requestURL,  listEventName_onchange_callback);
    }
    return;
}

//---------------------------------------------------------------------------------------------------------------
function listEventName_onchange_callback(){
    if (XmlHttp.readyState == 4)
        if (XmlHttp.status == 200){
            CreateXmlDocument();
            FillEventDateList();
        }
}

//---------------------------------------------------------------------------------------------------------------
function FillEventDateList(){
    var intChildQty = new Number(),
        listEventDate = document.getElementById("listEventDate");
    
    if (document.all)  
        intChildQty = XmlDoc.documentElement.childNodes.length;
    else
        intChildQty = parseInt(XmlDoc.documentElement.childNodes.length / 2);
    listEventDate.options.length = 0;
    for(Xh = 0; Xh < intChildQty; Xh++){
        var oOption = document.createElement("OPTION");
        
        oOption.value = XmlDoc.getElementsByTagName("EventID")[Xh].firstChild.nodeValue;
        oOption.text = XmlDoc.getElementsByTagName("EventDate")[Xh].firstChild.nodeValue;
        listEventDate.options.add(oOption);
    }
}

//---------------------------------------------------------------------------------------------------------------
function btnUpload_onclick(){
    var uploadFiles = document.getElementById("uploadDocFile");
        
    //filePath = uploadFiles.value.replace(/\\/g, "\\\\");
    if (uploadFiles.value.replace(/\\/g, "\\\\").length == 0)
    {
        alert("Please choose the file ...");
        document.getElementById("frmUploadDocument").onsubmit = function (){return false;}
    }
    else 
        {
            ShowDivMessage("The File is being uploaded...", false);
            var listEventDate = document.getElementById("listEventDate");
            var listEventName = document.getElementById("listEventName");
            //SetButtonsState(true);
            document.getElementById("txtHUploadResult").value = "0";
            if (listEventName.disabled == false){ // It means that the page isn't called by VISIT FORM or OPERATION FORM
                if (listEventName.selectedIndex > 0){ // Operation or Visit
                    if (listEventDate.selectedIndex >= 0){
                        document.getElementById("txtHEventID").value = listEventDate.options[listEventDate.selectedIndex].value;
                        document.getElementById("txtHEventDate").value = listEventDate.options[listEventDate.selectedIndex].text;
                    }
                }
                else{
                    document.getElementById("txtHEventID").value = 0;
                    document.getElementById("txtHEventDate").value = "";
                }
            }
            document.getElementById("txtHFileType").value = document.getElementById("listDocType_DocumentType").value;
            
            var theForm = document.forms["frmUploadDocument"];
            theForm.onsubmit = function (){return true;}
            if (!theForm.onsubmit || (theForm.onsubmit() != false)) {
                theForm.__EVENTTARGET.value = 'linkBtn';
                theForm.__EVENTARGUMENT.value = '';
                theForm.submit();
            }
        } 
    return;
}

//---------------------------------------------------------------------------------------------------------------
function SetButtonsState(disabled)
{
    document.getElementById("btnUpload").disabled = disabled;
	document.getElementById("btnClose").disabled = disabled;
	return;
}

//---------------------------------------------------------------------------------------------------------------
function UploadFile_txtDate_onchange(){
    var txtDate = document.getElementById("txtDate_txtGlobal");
    
    if (txtDate.value.length == 0){
        txtDate.value = document.getElementById("txtHCurrentDate").value;
    }
    return;
}

