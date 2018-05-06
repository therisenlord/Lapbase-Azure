var LoadTimer;          // used to check if data are loaded, each 1 second
var LoadFlag = false;
var ajax_optionDiv = false; // ajax_optionDiv and ajax_optionDiv_iframe are used for floading div
var ajax_optionDiv_iframe = false;
var FillDivDropdownList = false;
var LimitToList = false;
var ColumnWidth = new String();
var DivButtonNo = 0; // this is used when user clicks on sub-titles under Patient details (they are Demographics, Height/Weight/Notes, Major Comorbidity and minor comorbidity)

var tempTable = null;

var activeItem = 0, activeItemInDiv = 0, RecHeight = 20, RecNoInDiv = parseInt(200 / RecHeight) ; //200 is height of divScroll in Scroll-rel.css
var scrollbuttonDirection = 0;
var scrollbuttonSpeed = 2;  // How fast the content scrolls when you click the scroll buttons(Up and down arrows)
var scrollTimer = 10;	    // Also how fast the content scrolls. By decreasing this value, the content will move faster	
var mainDropDownList = false;

//---------------------------------------------------------------------------------------------------------------
/*
this funcion is called when page is loaded and calls the function to load all captions based of selected language.
a timer function checks the process of translating is done or not.
*/
function InitializePage()
{
    SetEvents();
    return;
}

//---------------------------------------------------------------------------------------------------------------
/*
because of using ASP.NET Components, this function is to set client-side events and functions for these components
*/
function SetEvents()
{
    document.onclick  = function(){
        if (ajax_optionDiv && ajax_optionDiv.style.display == "block"){
            ajax_optionDiv.style.display = "none";
            if (ajax_optionDiv_iframe)
                ajax_optionDiv_iframe.style.display = "none";
        }
    }

    $get("txtRefCity_txtGlobal").onkeyup = function(e){
        txtObject = $get("txtRefCity_txtGlobal");
        txtHiddenObject = $get('txtHRefCity');
        LimitToList = false;
        ColumnWidth = "40%;60%";
        ajax_showOptions(e , "cmbRefCity");
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

//--------------------------------------------------------------------------------------------------------------
function IsAnyFieldsFilled(){
    var flag = false;
    flag |= ($get("cmbRefCity").selectedIndex > 0);
    
    return(flag);
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
                if (txtObject.id.toLowerCase() == "txtRefCity_txtglobal") LoadStatePostCode(txtHiddenObject.value);
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
    $get("txtRefPostalCode_txtGlobal").value = strStatePostCode.substring(0, strStatePostCode.indexOf(";"));
    $get("txtRefState_txtGlobal").value = strStatePostCode.substring(strStatePostCode.indexOf(";")+1);
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
            case "txtRefCity_txtGlobal" :
                LoadStatePostCode(txtHiddenObject.value);
                break;
        }
    }
}