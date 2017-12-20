var cookieName = "fileDownload";
var debug = false;    //mettere a true per abilitare gli Alert di debug
var blockUi = true;  //mettere a true per bloccare la pagina in attesa del cookie
var retValue = true; //mettere a true per far funzionare il link
var refreshPage = true;
var secondsTimeout = 60;

var CookieValue = "";

function Debug(msg) {
    if (debug == true) {
        alert(msg);
    }
}

function GenerateValue(qstr) {
    return qstr;
}

var fileDownloadCheckTimer;
var badCloseWithX;

$(document).ready(function () {
    $("a.fileRepositoryCookie").live("click", function () {
        var href = $(this).attr("href");
        var splitted = href.split("?");
        var url = splitted[0];
        var querystring = splitted[1];

        CookieValue = GenerateValue(querystring);

        Debug(url);
        Debug(querystring);
        Debug(CookieValue);

        StartWaiting(true,true);
        return retValue;
    });
    $("a.fileRepositoryCookieNoRefresh").live("click", function () {
        var href = $(this).attr("href");
        var splitted = href.split("?");
        var url = splitted[0];
        var querystring = splitted[1];

        CookieValue = GenerateValue(querystring);
        Debug(url);
        Debug(querystring);
        Debug(CookieValue);

        StartWaiting(true,false);
        return retValue;
    });
    $("a.fileRepositoryCookieNoBlockUI").live("click", function () {
        var href = $(this).attr("href");
        var splitted = href.split("?");
        var url = splitted[0];
        var querystring = splitted[1];
        CookieValue = GenerateValue(querystring);
        Debug(url);
        Debug(querystring);
        Debug(CookieValue);

        StartWaiting(false, true);
        return retValue;
    });
});

function Lock(l) {
    Debug("Locked");
    if (blockUi == true && l) {
        if (DisplayMessage != null)
            $.blockUI({
                message: DisplayMessage , draggable: false, theme: false
            });
        else
            $.blockUI();
    }
}

function Unlock(l) {
    Debug("Unlocked");
    if (blockUi == true && l) {
        $.unblockUI();
    }
}

function RefreshPage() {
    Debug("Refreshing page");
    if (refreshPage == true) {
        window.location.reload();
    }
}
function StopWaiting(l, refresh) {
    window.clearInterval(fileDownloadCheckTimer);
    $.cookie(cookieName, null, { path: '/' }); //clears this cookie value
    Unlock(l);
    if (refresh)
        RefreshPage();
}
function ClosePlayer(_iframehref){
    window.clearInterval(badCloseWithX);
	var myDomain = location.href.replace("//","§§");
	if(myDomain.indexOf("/") > 0){
		var myDomainPart2 = myDomain.substring(myDomain.indexOf("/"));
		if(myDomain.indexOf("?") >= 0){
			var myDomainParams = myDomainPart2.split("?")[1];
			myDomain = myDomain.split("/")[0];
			myDomain = myDomain.replace("§§","//");
			var iframehref = _iframehref;
			if(jQuery("#iframehrefUrlClosePayer").size() > 0)
				jQuery("#iframehrefUrlClosePayer").remove();
			var ifrm = document.createElement("iframe");
			ifrm.setAttribute("id", "iframehrefUrlClosePayer");
			ifrm.setAttribute("src", iframehref);
			ifrm.style.width = "0px";
			ifrm.style.height = "0px";
			document.body.appendChild(ifrm);
		}
	}
}
function StartWaiting(l, refresh) {
    $.cookie(cookieName, null, { path: '/' }); //clears this cookie value
    Lock(l);
    fileDownloadCheckTimer = window.setInterval(function () {
        //jQuery.cookie.raw = true;
        var value = jQuery.cookie(cookieName);
        var expectedValue = CookieValue;
        var decodedValue = decodeURIComponent(CookieValue);
        var sWaiting = (value == CookieValue || value == decodedValue);

        //console.log(value);
        //console.log(expectedValue);
        //console.log(decodedValue);
        //console.log(sWaiting);

        if (sWaiting == false && value && decodedValue && value != "" && decodedValue != "") {
            if (IsPlayUrl(value, expectedValue)) {
                sWaiting = ValidatePlayUrl(value, expectedValue);
                if (sWaiting == false && expectedValue != decodedValue) {
                    sWaiting = ValidatePlayUrl(value, decodedValue);
                }
            }           
        }
        
        //var msg = [];
        //for (var i in value) msg.push(i + " = " + value[i]);
        //    console.log(msg.join("\n"));
        //var pippo = decodeURIComponent(CookieValue);

        //console.log("Valore cookie:" + value.ToString());
        //console.log("Valore da trovare:" +CookieValue);
        //console.log("Encoded=" + encodeURIComponent(value));
        //console.log("Decoded=" + decodeURIComponent(value));
       
        if (sWaiting) {
            //console.log("==");
            StopWaiting(l, refresh);
        }
        //else {
        //    console.log("!=");
        //}
    }, 1000);
	
	badCloseWithX = setInterval(function(){
		var urlClosePayer = jQuery.cookie("urlClosePayer");
		if(urlClosePayer){				
			$.cookie("urlClosePayer", null, { path: '/' }); //clears this cookie value
			ClosePlayer(urlClosePayer);
		}
	},1000);
}        

function IsPlayUrl(value, expectedValue) {
    return ((ContainsValue(expectedValue, "type=ScormPackage") || ContainsValue(expectedValue, "type=Multimedia")) && (ContainsValue(value, "type=ScormPackage") || ContainsValue(value, "type=Multimedia")));
}
function ValidatePlayUrl(value, expectedValue) {
    var idItem = GetQueryItem(value, "&idItem=");
    var idVersion = GetQueryItem(value, "&idVersion=");
    var uniqueId = GetQueryItem(value, "&uniqueId=");
    var idLink = GetQueryItem(value, "&idLink=");
    var uniqueIdVersion = GetQueryItem(value, "&uniqueIdVersion=");

    var expIdItem = GetQueryItem(expectedValue, "&idItem=");
    var expIdVersion = GetQueryItem(expectedValue, "&idVersion=");
    var expUniqueId = GetQueryItem(expectedValue, "&uniqueId=");
    var expIdLink = GetQueryItem(expectedValue, "&idLink=");
    var expUniqueIdVersion = GetQueryItem(expectedValue, "&uniqueIdVersion=");

    return (idItem == expIdItem && expIdLink == idLink && (expUniqueId == "00000000-0000-0000-0000-000000000000")
            && (expUniqueIdVersion == "" && uniqueIdVersion != ""));
        //console.log(sWaiting);
}
function GetQueryItem(url,queryItem){
    var result = "";
    if (url.indexOf(queryItem)>-1){
        result = url.split(queryItem)[1].split("&")[0];
    }
    return result;
}

function ContainsValue(value, search) {
    if (value){
        return (value.indexOf(search) > -1);
    }
    else{
        return false;
    }
}