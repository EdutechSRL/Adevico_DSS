/**
 * Created by roberto.maschio on 26/11/13.
 */

$(function(){

    $.DivasCookies({
        bannerText: "\
        Il Sito utilizza cookies di sessione e tecnici. \
        Se prosegui nella navigazione di questo sito, acconsenti all’utilizzo dei cookies. <br>\
        The website uses session and technic cookies. \
        By continuing your visit on the website, you consent to the use of the cookies. <br>\
        ",
        cookiePolicyLink: "~/privacy/privacy.html",
        saveUserPreferences: true,
        cookiePolicyLinkText: "Privacy Policy",
        acceptButtonText: "Ok"    
    });    

});
