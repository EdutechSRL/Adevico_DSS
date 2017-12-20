/*
    en-US
    de-DE
    it-IT
    es-ES
*/
var generalDictionary = {};
generalDictionary["en-US"] = {
    Title: '*Avviso',
    Description: 'I browser testati e supportati da questa applicazione sono Google Chrome e Mozilla Firefox.<br /><br />Altri browser potrebbero riscontrare anomalie e/o malfunzionamenti con la possibilità di compromettere l\'esperienza o i dati',
    Button: 'Ho capito!'
};
generalDictionary["de-DE"] = {
    Title: '*Avviso',
    Description: 'I browser testati e supportati da questa applicazione sono Google Chrome e Mozilla Firefox.<br /><br />Altri browser potrebbero riscontrare anomalie e/o malfunzionamenti con la possibilità di compromettere l\'esperienza o i dati',
    Button: 'Ho capito!'
};
generalDictionary["it-IT"] = {
    Title: 'Avviso',
    Description: 'I browser testati e supportati da questa applicazione sono Google Chrome e Mozilla Firefox.<br /><br />Altri browser potrebbero riscontrare anomalie e/o malfunzionamenti con la possibilità di compromettere l\'esperienza o i dati',
    Button: 'Ho capito!'
};
generalDictionary["es-ES"] = {
    Title: '*Avviso',
    Description: 'I browser testati e supportati da questa applicazione sono Google Chrome e Mozilla Firefox.<br /><br />Altri browser potrebbero riscontrare anomalie e/o malfunzionamenti con la possibilità di compromettere l\'esperienza o i dati',
    Button: 'Ho capito!'
};

$(document).ready(function () {
    var languageCodeDefault = "it-IT";
    var languageCode = jQuery("#scriptForAdvExplorerMessage").attr("data-language-code");

    var localDictionary = generalDictionary[languageCode];

    if (!localDictionary)
        localDictionary = generalDictionary[languageCodeDefault];

    var giaAvvertito = getCookie("Avvertimento.IE") + "";
    if (giaAvvertito == "") {
        var IEversion = detectIE();
        if (IEversion) {
            jQuery("body").append('<div id="boxAvvertimentioIE" class="box-all-absolute">' +
                '<style type="text/css">.box-all-absolute{top:0;left:0;position:absolute;width:100%;height:100%;background-color:#fff;background-color:RGBA(255,255,255,0.6);z-index:234456;min-width:320px}.box-absolute{position:absolute;left:25%;right:25%;top:25%;background-color:#fff;padding:24px;box-shadow:0 2px 4px 0 #d9d9d9,0 2px 10px 0 #d9d9d9;min-width:320px}.adv-alert-warning{color:#8a6d3b;background-color:#fcf8e3;border-color:#faebcc}.adv-alert{padding:15px;margin-bottom:20px;border:1px solid transparent;border-radius:4px}</style>' +
                '<div class="box-absolute adv-alert adv-alert-warning"><h3>' + localDictionary.Title + '</h3>' +
                localDictionary.Description +
                '<br /><br /><a class="btn btn-default" style="text-decoration:none;" onclick="HoCapitoForIE()" href="#">' + localDictionary.Button + '</a>' +
                '</div>' +
                '</div>');
        }
    }
});

function HoCapitoForIE() {
    setCookie("Avvertimento.IE", "Hocapito!", 30);
    jQuery("#boxAvvertimentioIE").remove();
}
function detectIE() {
    var ua = window.navigator.userAgent;

    var msie = ua.indexOf('MSIE ');
    if (msie > 0) {
        // IE 10 or older => return version number
        return parseInt(ua.substring(msie + 5, ua.indexOf('.', msie)), 10);
    }

    var trident = ua.indexOf('Trident/');
    if (trident > 0) {
        // IE 11 => return version number
        var rv = ua.indexOf('rv:');
        return parseInt(ua.substring(rv + 3, ua.indexOf('.', rv)), 10);
    }

    var edge = ua.indexOf('Edge/');
    if (edge > 0) {
        // Edge (IE 12+) => return version number
        return parseInt(ua.substring(edge + 5, ua.indexOf('.', edge)), 10);
    }

    // other browser
    return false;
}

function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}