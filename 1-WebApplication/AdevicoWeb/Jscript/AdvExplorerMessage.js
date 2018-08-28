/*(function () {
    $(document).ready(function () {
        if (!isTodayViewed()) {
            $("body").append($(
                `<div id="messaggioManutenzione" onclick="$(this).remove()" style="position:fixed;left:0;top:0;bottom:0;right:0;background-color:#fff;background-color:rgba(255,255,255,0.7);z-index:11111;">
                    <div style="margin: 10% 24%;color: #333;padding: 24px;background-color: #fff;text-align: center;border: solid 1px #cecece;">
                        <h1>
                            ATTENZIONE!<br />Lunedì 20.08.2018 dalle ore 13.00 alle ore 18.00 la piattaforma NON SARA’ DISPONIBILE per manutenzione.<br><br>
                            <button type="button">OK</button>
                        </h1>
                    </div>
                </div>`
            ));
            setCookie('messaggioManutenzione', '1', 1);
        }
    });
    function isTodayViewed() {
        return getCookie('messaggioManutenzione');
    }
    function setCookie(cname, cvalue, exdays) {
        var d = new Date();
        d.setHours(0, 0, 0);
        d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
        var expires = "expires=" + d.toUTCString();
        document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
    }
    function getCookie(cname) {
        var name = cname + "=";
        var decodedCookie = decodeURIComponent(document.cookie);
        var ca = decodedCookie.split(';').reverse();
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
})();
*/

/*
    en-US
    de-DE
    it-IT
    es-ES
*/
/*
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
		if(rv > -1)
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



//in explorer tolgo gli openmodal da FullPlay.aspx
$(document).ready(function () {
    if((location.href+"").indexOf("/FullPlay.aspx") > 0){
        var IEversion = detectIE();
        if (IEversion) {
            var items = jQuery("ul.activities .iteminfo a.openmodal");
            if(items.size() > 0){
                items.each(function(index, item){
					var el = jQuery(item);
					var hrefA = el.attr("href");
					hrefA = (hrefA+"").replace("/PlayerOnModal.aspx","/Player.aspx");
					hrefA = (hrefA+"").replace("onModalPage=True","onModalPage=False");
					el.attr("href", hrefA);
					el.removeClass("openmodal");
					el.attr("target","_blank");
                    el.unbind("click");
                });
            }
        }
    }
});
*/