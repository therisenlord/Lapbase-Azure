// JScript File
//-----------------------------------------------------------------------------------------------------------------
function btnRefDrSave_onclick(){
    if (RefDr_CanToSave())
    {    
            var oOption = document.createElement("OPTION");
            var strSurname = document.getElementById("txtRefSurname_txtGlobal").value;
            var strFirstname = document.getElementById("txtRefFirstname_txtGlobal").value;
            var strRefID = ((strSurname.length > 4) ? strSurname.substring(0, 4) : strSurname) + strFirstname.substring(0, 1);
            
            var address1 = document.getElementById("txtRefAddress1_txtGlobal").value.replace("'", "`");
            address1 = address1.replace("&", "&amp;");
            
            var address2 = document.getElementById("txtRefAddress2_txtGlobal").value.replace("'", "`");
            address2 = address2.replace("&", "&amp;");            
     
            document.getElementById("txtPatientRefSurname").value = strSurname;
            document.getElementById("txtPatientRefFirstname").value = strFirstname;
            document.getElementById("txtPatientRefTitle").value = document.getElementById("txtRefTitle_txtGlobal").value.replace("'", "`");
            document.getElementById("txtPatientRefUseFirst").value = document.getElementById("chkRefUseFirst").checked;
            document.getElementById("txtPatientRefAddress1").value = address1;
            document.getElementById("txtPatientRefAddress2").value = address2;
            document.getElementById("txtPatientRefSuburb").value = document.getElementById("txtRefCity_txtGlobal").value.replace("'", "`");
            document.getElementById("txtPatientRefPostalCode").value = document.getElementById("txtRefPostalCode_txtGlobal").value.replace("'", "`");
            document.getElementById("txtPatientRefState").value = document.getElementById("txtRefState_txtGlobal").value.replace("'", "`");
            document.getElementById("txtPatientRefPhone").value = document.getElementById("txtRefPhone_txtGlobal").value.replace("'", "`");
            document.getElementById("txtPatientRefFax").value = document.getElementById("txtRefFax_txtGlobal").value.replace("'", "`");
            document.getElementById("txtPatientRefDrID").value = strRefID;
            
            
            oOption.value = ((strSurname.length > 4) ? strSurname.substring(0, 4) : strSurname) + strFirstname.substring(0, 1);
            oOption.text = strSurname + "  " + strFirstname + "  " + document.getElementById("txtRefTitle_txtGlobal").value ;
            
            valid.deactivate();
            
            txtObject.value = oOption.text;
            txtHiddenObject.value = oOption.value; 
            
            __doPostBack('LinkBtnSave_RefDr','');            
    }
    return;
}

//-----------------------------------------------------------------------------------------------------------------
function RefDr_CanToSave()
{
    var flag = true;
    var ErrorMessage = "";
    
    document.getElementById("lblRefSurname").style.color  = "";
    document.getElementById("lblRefFirstname").style.color  = "";
    if (document.getElementById("txtRefSurname_txtGlobal").value.length == 0){
        flag = false;
        ErrorMessage = "Surname ";
        document.getElementById("lblRefSurname").style.color  = "RED";
    }
    if (document.getElementById("txtRefFirstname_txtGlobal").value.length == 0){
        flag = false;
        ErrorMessage = (ErrorMessage.length == 0) ? "Firstname" : (ErrorMessage + ", " + "Firstname");
        document.getElementById("lblRefFirstname").style.color  = "RED";
    }
    
    if (!flag) {
        alert("Please enter " + ErrorMessage + " ...");
    }
    return(flag);
}