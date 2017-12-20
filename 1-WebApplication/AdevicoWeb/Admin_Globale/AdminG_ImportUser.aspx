<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin_Globale/AdminPortal.Master"
    CodeBehind="AdminG_ImportUser.aspx.vb" Inherits="Comunita_OnLine.AdminG_ImportUser" %>

<%@ MasterType VirtualPath="~/Admin_Globale/AdminPortal.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="radt" Namespace="Telerik.WebControls" Assembly="RadTreeView.NET2" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server" ID="Content1">
    <script type="text/javascript" language="Javascript" src="./../jscript/generali.js"></script>
    <script type="text/javascript" language="javascript">
        function SetTextBox(id, s, nomeDiv) {
            document.getElementById(id).value = CodificaHTML(s);
            HideTreeView(nomeDiv);
        }

        function CodificaHTML(stringa) {
            var pos;
            var s;

            pos = stringa.indexOf("&lt;")
            if (pos > 0)
                s = stringa.substring(0, pos)
            else {
                pos = stringa.indexOf("<")
                if (pos > 0)
                    s = stringa.substring(0, pos)
                else
                    s = stringa
            }

            s = s.replace("&#32;", " ")
            s = s.replace("&nbsp;", " ")
            s = s.replace("&#160;", " ")
            s = s.replace("&#39;", "'")
            s = s.replace("&#45;", "-")
            s = s.replace("&shy;", "­")
            s = s.replace("&#173;", "­")
            s = s.replace("&#33;", "!")
            s = s.replace("&#34;", "\"")
            s = s.replace("&quot;", "\"")
            s = s.replace("&#35;", "#")
            s = s.replace("&#36;", "$")
            s = s.replace("&#37;", "%")
            s = s.replace("&#38;", "&")
            s = s.replace("&amp;", "&")
            s = s.replace("&#40;", "(")
            s = s.replace("&#41;", ")")
            s = s.replace("&#42;", "*")
            s = s.replace("&#44;", ",")
            s = s.replace("&#46;", ".")
            s = s.replace("&#47;", "/")
            s = s.replace("&#58;", ":")
            s = s.replace("&#59;", ";")
            s = s.replace("&#63;", "?")
            s = s.replace("&#64;", "@")
            s = s.replace("&#91;", "[")
            s = s.replace("&#92;", "\\")
            s = s.replace("&#93;", "]")
            s = s.replace("&#94;", "^")
            s = s.replace("&#95;", "_")
            s = s.replace("&#96;", "`")
            s = s.replace("&#123;", "{")
            s = s.replace("&#124;", "|")
            s = s.replace("&#125;", "}")
            s = s.replace("&#126;", "~")
            s = s.replace("&#161;", "¡")
            s = s.replace("&iexcl;", "¡")
            s = s.replace("&#166;", "¦")
            s = s.replace("&brvbar;", "¦")
            s = s.replace("&#168;", "¨")
            s = s.replace("&uml;", "¨")
            s = s.replace("&macr;", "¯")
            s = s.replace("&#175;", "¯")
            s = s.replace("&#180;", "´")
            s = s.replace("&acute;", "´")
            s = s.replace("&cedil;", "¸")
            s = s.replace("&#184;", "¸")
            s = s.replace("&#191;", "¿")
            s = s.replace("&iquest;", "¿")
            s = s.replace("&#43;", "+")
            s = s.replace("&#60;", "<")
            s = s.replace("&lt;", "<")
            s = s.replace("&#61;", "=")
            s = s.replace("&gt;", ">")
            s = s.replace("&#62;", ">")
            s = s.replace("&#177;", "±")
            s = s.replace("&plusmn;", "±")
            s = s.replace("&#171;", "«")
            s = s.replace("&#187;", "»")
            s = s.replace("&raquo;", "»")
            s = s.replace("&times;", "×")
            s = s.replace("&#215;", "×")
            s = s.replace("&#247;", "÷")
            s = s.replace("&divide;", "÷")
            s = s.replace("&cent;", "¢")
            s = s.replace("&#162;", "¢")
            s = s.replace("&pound;", "£")
            s = s.replace("&#163;", "£")
            s = s.replace("&curren;", "¤")
            s = s.replace("&#164;", "¤")
            s = s.replace("&yen;", "¥")
            s = s.replace("&#165;", "¥")
            s = s.replace("&#167;", "§")
            s = s.replace("&sect;", "§")
            s = s.replace("&copy;", "©")
            s = s.replace("&#169;", "©")
            s = s.replace("&#172;", "¬")
            s = s.replace("&not;", "¬")
            s = s.replace("&reg;", "®")
            s = s.replace("&#174;", "®")
            s = s.replace("&deg;", "°")
            s = s.replace("&#176;", "°")
            s = s.replace("&micro;", "µ")
            s = s.replace("&#181;", "µ")
            s = s.replace("&#182;", "¶")
            s = s.replace("&para;", "¶")
            s = s.replace("&#183;", "·")
            s = s.replace("&middot;", "·")
            s = s.replace("&euro;", "€")
            s = s.replace("&#48;", "0")
            s = s.replace("&#188;", "¼")
            s = s.replace("&frac14;", "¼")
            s = s.replace("&frac12;", "½")
            s = s.replace("&#189;", "½")
            s = s.replace("&frac34;", "¾")
            s = s.replace("&#190;", "¾")
            s = s.replace("&#49;", "1")
            s = s.replace("&#185;", "¹")
            s = s.replace("&sup1;", "¹")
            s = s.replace("&#178;", "²")
            s = s.replace("&#50;", "2")
            s = s.replace("&sup2;", "²")
            s = s.replace("&#179;", "³")
            s = s.replace("&#51;", "3")
            s = s.replace("&sup3;", "³")
            s = s.replace("&#52;", "4")
            s = s.replace("&#53;", "5")
            s = s.replace("&#54;", "6")
            s = s.replace("&#55;", "7")
            s = s.replace("&#56;", "8")
            s = s.replace("&#57;", "9")
            s = s.replace("&#65;", "A")
            s = s.replace("&#97;", "a")
            s = s.replace("&#170;", "ª")
            s = s.replace("&ordf;", "ª")
            s = s.replace("&#225;", "á")
            s = s.replace("&aacute;", "á")
            s = s.replace("&Aacute;", "Á")
            s = s.replace("&#193;", "Á")
            s = s.replace("&agrave;", "à")
            s = s.replace("&#224;", "à")
            s = s.replace("&Agrave;", "À")
            s = s.replace("&#192;", "À")
            s = s.replace("&acirc;", "â")
            s = s.replace("&#194;", "Â")
            s = s.replace("&#226;", "â")
            s = s.replace("&auml;", "ä")
            s = s.replace("&Auml;", "Ä")
            s = s.replace("&#228;", "ä")
            s = s.replace("&#196;", "Ä")
            s = s.replace("&#259;", "a")
            s = s.replace("&#258;", "A")
            s = s.replace("&#257;", "a")
            s = s.replace("&#256;", "A")
            s = s.replace("&atilde;", "ã")
            s = s.replace("&#227;", "ã")
            s = s.replace("&#195;", "Ã")
            s = s.replace("&Atilde;", "Ã")
            s = s.replace("&aring;", "å")
            s = s.replace("&#197", "Å")
            s = s.replace("&#229;", "å")
            s = s.replace("&Aring;", "Å")
            s = s.replace("&#260;", "A")
            s = s.replace("&#261;", "a")
            s = s.replace("&aelig;", "æ")
            s = s.replace("&#198;", "Æ")
            s = s.replace("&AElig;", "Æ")
            s = s.replace("&#230;", "æ")
            s = s.replace("&#66;", "B")
            s = s.replace("&#98;", "b")
            s = s.replace("&#67;", "C")
            s = s.replace("&#99;", "c")
            s = s.replace("&#263;", "c")
            s = s.replace("&#262;", "C")
            s = s.replace("&#266;", "C")
            s = s.replace("&#267;", "c")
            s = s.replace("&#264;", "C")
            s = s.replace("&#265;", "c")
            s = s.replace("&#268;", "C")
            s = s.replace("&#269;", "c")
            s = s.replace("&ccedil;", "ç")
            s = s.replace("&#231;", "ç")
            s = s.replace("&Ccedil;", "Ç")
            s = s.replace("&#199;", "Ç")
            s = s.replace("&#100;", "d")
            s = s.replace("&#68;", "D")
            s = s.replace("&#270;", "D")
            s = s.replace("&#271;", "d")
            s = s.replace("&#272;", "Ð")
            s = s.replace("&#273;", "d")
            s = s.replace("&eth;", "ð")
            s = s.replace("&ETH;", "Ð")
            s = s.replace("&#240;", "ð")
            s = s.replace("&#208;", "Ð")
            s = s.replace("&#69;", "E")
            s = s.replace("&#101;", "e")
            s = s.replace("&#233;", "é")
            s = s.replace("&Eacute;", "É")
            s = s.replace("&#201;", "É")
            s = s.replace("&eacute;", "é")
            s = s.replace("&Egrave;", "È")
            s = s.replace("&#200;", "È")
            s = s.replace("&egrave;", "è")
            s = s.replace("&#232;", "è")
            s = s.replace("&#278;", "E")
            s = s.replace("&#279;", "e")
            s = s.replace("&#202;", "Ê")
            s = s.replace("&ecirc;", "ê")
            s = s.replace("&#234;", "ê")
            s = s.replace("&Ecirc;", "Ê")
            s = s.replace("&euml;", "ë")
            s = s.replace("&#203;", "Ë")
            s = s.replace("&#235;", "ë")
            s = s.replace("&#282;", "E")
            s = s.replace("&#283;", "e")
            s = s.replace("&#276;", "E")
            s = s.replace("&#277", "e")
            s = s.replace("&#274;", "E")
            s = s.replace("&#275;", "e")
            s = s.replace("&#280;", "E")
            s = s.replace("&#281;", "e")
            s = s.replace("&#102;", "f")
            s = s.replace("&#70;", "F")
            s = s.replace("&#103;", "g")
            s = s.replace("&#71;", "G")
            s = s.replace("&#288;", "G")
            s = s.replace("&#289;", "g")
            s = s.replace("&#284;", "G")
            s = s.replace("&#285;", "g")
            s = s.replace("&#286;", "G")
            s = s.replace("&#287;", "g")
            s = s.replace("&#290;", "G")
            s = s.replace("&#291;", "g")
            s = s.replace("&#104;", "h")
            s = s.replace("&#72;", "H")
            s = s.replace("&#293;", "h")
            s = s.replace("&#292;", "H")
            s = s.replace("&#295;", "h")
            s = s.replace("&#294;", "H")
            s = s.replace("&#105;", "i")
            s = s.replace("&#73;", "I")
            s = s.replace("&#305;", "i")
            s = s.replace("&#237", "í")
            s = s.replace("&#205;", "Í")
            s = s.replace("&Iacute;", "Í")
            s = s.replace("&iacute;", "í")
            s = s.replace("&Igrave;", "Ì")
            s = s.replace("&#204;", "Ì")
            s = s.replace("&igrave;", "ì")
            s = s.replace("&#236;", "ì")
            s = s.replace("&#304;", "I")
            s = s.replace("&#206;", "Î")
            s = s.replace("&icirc;", "î")
            s = s.replace("&#238;", "î")
            s = s.replace("&Icirc;", "Î")
            s = s.replace("&iuml;", "ï")
            s = s.replace("&#239;", "ï")
            s = s.replace("&Iuml;", "Ï")
            s = s.replace("&#207;", "Ï")
            s = s.replace("&#301;", "i")
            s = s.replace("&#300;", "I")
            s = s.replace("&#298;", "I")
            s = s.replace("&#299;", "i")
            s = s.replace("&#296;", "I")
            s = s.replace("&#297;", "i")
            s = s.replace("&#302;", "I")
            s = s.replace("&#303;", "i")
            s = s.replace("&#306;", "?")
            s = s.replace("&#307;", "?")
            s = s.replace("&#106;", "j")
            s = s.replace("&#74;", "J")
            s = s.replace("&#309;", "j")
            s = s.replace("&#308;", "J")
            s = s.replace("&#107;", "k")
            s = s.replace("&#75;", "K")
            s = s.replace("&#312;", "?")
            s = s.replace("&#311;", "k")
            s = s.replace("&#310;", "K")
            s = s.replace("&#76;", "L")
            s = s.replace("&#108;", "l")
            s = s.replace("&#314;", "l")
            s = s.replace("&#313;", "L")
            s = s.replace("&#319;", "?")
            s = s.replace("&#320;", "?")
            s = s.replace("&#317", "L")
            s = s.replace("&#318;", "l")
            s = s.replace("&#316;", "l")
            s = s.replace("&#315;", "L")
            s = s.replace("&#321;", "L")
            s = s.replace("&#322;", "l")
            s = s.replace("&#77;", "M")
            s = s.replace("&#109;", "m")
            s = s.replace("&#110;", "n")
            s = s.replace("&#78;", "N")
            s = s.replace("&#323;", "N")
            s = s.replace("&#324;", "n")
            s = s.replace("&#327;", "N")
            s = s.replace("&#328;", "n")
            s = s.replace("&#241;", "ñ")
            s = s.replace("&Ntilde;", "Ñ")
            s = s.replace("&ntilde;", "ñ")
            s = s.replace("&#209;", "Ñ")
            s = s.replace("&#325;", "N")
            s = s.replace("&#326;", "n")
            s = s.replace("&#329;", "?")
            s = s.replace("&#330;", "?")
            s = s.replace("&#331;", "?")
            s = s.replace("&#111;", "o")
            s = s.replace("&#79;", "O")
            s = s.replace("&#186;", "º")
            s = s.replace("&ordm;", "º")
            s = s.replace("&Oacute;", "Ó")
            s = s.replace("&#243;", "ó")
            s = s.replace("&oacute;", "ó")
            s = s.replace("&#211;", "Ó")
            s = s.replace("&Ograve;", "Ò")
            s = s.replace("&ograve;", "ò")
            s = s.replace("&#210;", "Ò")
            s = s.replace("&#242;", "ò")
            s = s.replace("&#212;", "Ô")
            s = s.replace("&Ocirc;", "Ô")
            s = s.replace("&ocirc;", "ô")
            s = s.replace("&#244;", "ô")
            s = s.replace("&ouml;", "ö")
            s = s.replace("&Ouml;", "Ö")
            s = s.replace("&#246;", "ö")
            s = s.replace("&#214;", "Ö")
            s = s.replace("&#334;", "O")
            s = s.replace("&#335;", "o")
            s = s.replace("&#332;", "O")
            s = s.replace("&#333;", "o")
            s = s.replace("&#245;", "õ")
            s = s.replace("&#213;", "Õ")
            s = s.replace("&otilde;", "õ")
            s = s.replace("&Otilde;", "Õ")
            s = s.replace("&#336;", "O")
            s = s.replace("&#337;", "o")
            s = s.replace("&#248;", "ø")
            s = s.replace("&oslash;", "ø")
            s = s.replace("&Oslash;", "Ø")
            s = s.replace("&#216;", "Ø")
            s = s.replace("&#338;", "Œ")
            s = s.replace("&#339;", "œ")
            s = s.replace("&#112;", "p")
            s = s.replace("&#80;", "P")
            s = s.replace("&#113;", "q")
            s = s.replace("&#81;", "Q")
            s = s.replace("&#114;", "r")
            s = s.replace("&#82;", "R")
            s = s.replace("&#341;", "r")
            s = s.replace("&#341;", "r")
            s = s.replace("&#340;", "R")
            s = s.replace("&#340;", "R")
            s = s.replace("&#344;", "R")
            s = s.replace("&#345;", "r")
            s = s.replace("&#344;", "R")
            s = s.replace("&#345;", "r")
            s = s.replace("&#342;", "R")
            s = s.replace("&#342;", "R")
            s = s.replace("&#343;", "r")
            s = s.replace("&#343;", "r")
            s = s.replace("&#83;", "S")
            s = s.replace("&#115;", "s")
            s = s.replace("&#347;", "s")
            s = s.replace("&#346;", "S")
            s = s.replace("&#347;", "s")
            s = s.replace("&#346;", "S")
            s = s.replace("&#348;", "S")
            s = s.replace("&#349;", "s")
            s = s.replace("&#349;", "s")
            s = s.replace("&#348;", "S")
            s = s.replace("&#353;", "š")
            s = s.replace("&#352;", "Š")
            s = s.replace("&#352;", "Š")
            s = s.replace("&#353;", "š")
            s = s.replace("&#350;", "S")
            s = s.replace("&#350;", "S")
            s = s.replace("&#351;", "s")
            s = s.replace("&#351;", "s")
            s = s.replace("&#223;", "ß")
            s = s.replace("&szlig;", "ß")
            s = s.replace("&#383;", "?")
            s = s.replace("&#383;", "?")
            s = s.replace("&#84;", "T")
            s = s.replace("&#116;", "t")
            s = s.replace("&#577;", "t")
            s = s.replace("&#356;", "T")
            s = s.replace("&#356;", "T")
            s = s.replace("&#357", "t")
            s = s.replace("&#354;", "T")
            s = s.replace("&#355;", "t")
            s = s.replace("&#355;", "t")
            s = s.replace("&#354;", "T")
            s = s.replace("&#222;", "Þ")
            s = s.replace("&THORN;", "Þ")
            s = s.replace("&#254;", "þ")
            s = s.replace("&thorn;", "þ")
            s = s.replace("&#359;", "t")
            s = s.replace("&#358;", "T")
            s = s.replace("&#358;", "T")
            s = s.replace("&#359;", "t")
            s = s.replace("&#117;", "u")
            s = s.replace("&#85;", "U")
            s = s.replace("&Uacute;", "Ú")
            s = s.replace("&#218;", "Ú")
            s = s.replace("&#250;", "ú")
            s = s.replace("&uacute;", "ú")
            s = s.replace("&ugrave;", "ù")
            s = s.replace("&#217;", "Ù")
            s = s.replace("&#249;", "ù")
            s = s.replace("&Ugrave;", "Ù")
            s = s.replace("&Ucirc;", "Û")
            s = s.replace("&#251;", "û")
            s = s.replace("&#219;", "Û")
            s = s.replace("&ucirc;", "û")
            s = s.replace("&#252;", "ü")
            s = s.replace("&Uuml;", "Ü")
            s = s.replace("&#220;", "Ü")
            s = s.replace("&uuml;", "ü")
            s = s.replace("&#364;", "U")
            s = s.replace("&#365;", "u")
            s = s.replace("&#364;", "U")
            s = s.replace("&#365;", "u")
            s = s.replace("&#363;", "u")
            s = s.replace("&#362;", "U")
            s = s.replace("&#362;", "U")
            s = s.replace("&#363;", "u")
            s = s.replace("&#361;", "u")
            s = s.replace("&#360;", "U")
            s = s.replace("&#361;", "u")
            s = s.replace("&#360;", "U")
            s = s.replace("&#367;", "u")
            s = s.replace("&#367;", "u")
            s = s.replace("&#366;", "U")
            s = s.replace("&#366;", "U")
            s = s.replace("&#370;", "U")
            s = s.replace("&#371;", "u")
            s = s.replace("&#371;", "u")
            s = s.replace("&#370;", "U")
            s = s.replace("&#368;", "U")
            s = s.replace("&#369;", "u")
            s = s.replace("&#368;", "U")
            s = s.replace("&#369;", "u")
            s = s.replace("&#118;", "v")
            s = s.replace("&#86;", "V")
            s = s.replace("&#119;", "w")
            s = s.replace("&#87;", "W")
            s = s.replace("&#373;", "w")
            s = s.replace("&#373;", "w")
            s = s.replace("&#372;", "W")
            s = s.replace("&#372;", "W")
            s = s.replace("&#88;", "X")
            s = s.replace("&#120;", "x")
            s = s.replace("&#89;", "Y")
            s = s.replace("&#121;", "y")
            s = s.replace("&yacute;", "ý")
            s = s.replace("&#221;", "Ý")
            s = s.replace("&Yacute;", "Ý")
            s = s.replace("&#253;", "ý")
            s = s.replace("&#375;", "y")
            s = s.replace("&#375;", "y")
            s = s.replace("&#374;", "Y")
            s = s.replace("&#374;", "Y")
            s = s.replace("&#376;", "Ÿ")
            s = s.replace("&#376;", "Ÿ")
            s = s.replace("&#255;", "ÿ")
            s = s.replace("&#90;", "Z")
            s = s.replace("&#122;", "z")
            s = s.replace("&#378;", "z")
            s = s.replace("&#377", "Z")
            s = s.replace("&#378;", "z")
            s = s.replace("&#377;", "Z")
            s = s.replace("&#380;", "z")
            s = s.replace("&#379;", "Z")
            s = s.replace("&#379;", "Z")
            s = s.replace("&#380;", "z")
            s = s.replace("&#382;", "ž")
            s = s.replace("&#381;", "Ž")
            s = s.replace("&#382;", "ž")
            s = s.replace("&#381;", "Ž")
            return s
        }
        function ToggleTreeView(nomeDiv) {
            if (nomeDiv == "divSorgente") {
                if (document.getElementById("divSorgente").style.visibility == "visible")
                    HideTreeView(nomeDiv);
                else
                    ShowTreeView(nomeDiv);
            }
            else {
                if (document.getElementById("divDestinatario").style.visibility == "visible")
                    HideTreeView(nomeDiv);
                else
                    ShowTreeView(nomeDiv);
            }

        }

        function ShowTreeView(nomeDiv) {
            if (nomeDiv == "divSorgente")
                document.getElementById("divSorgente").style.visibility = "visible";
            else
                document.getElementById("divDestinatario").style.visibility = "visible";
        }

        function HideTreeView(nomeDiv) {
            if (nomeDiv == "divSorgente")
                document.getElementById("divSorgente").style.visibility = "hidden";
            else
                document.getElementById("divDestinatario").style.visibility = "hidden";
        }

        function ProcessClientClick(node, nomeDiv) {
            if (node.Category == "0" || node.Category == "") {
                if (document.getElementById("divSorgente").style.visibility == "visible")
                    alert('Seleziona una comunità sorgente !');
                else
                    alert('Seleziona una comunità di destinazione!');

            } else {
                if ((document.getElementById("divSorgente").style.visibility == "visible") && (node.Value == document.forms[0].HDN_valoreDestinatario.value))
                    alert('Seleziona una comunità sorgente diversa da quella di destinazione !');
                else if (node.Value == document.forms[0].HDN_valoreSorgente.value)
                    alert('Seleziona una comunità destinazione diversa da quella sorgente !');
                else {
                    if (document.getElementById("divSorgente").style.visibility == "visible") {
                        document.forms[0].HDN_valoreSorgente.value = node.Value
                        SetTextBox('<%= TXBsorgente.ClientID %>', node.Text, 'divSorgente');
                    }
                    else {
                        document.forms[0].HDN_valoreDestinatario.value = node.Value
                        SetTextBox('<%= TXBdestinatario.ClientID %>', node.Text, 'divDestinatario');
                    }
                    HideTreeView(nomeDiv);
                }
            }

            return false;
        }


        function SelectMe(Me) {
            var HIDcheckbox, selezionati;
            //eval('HIDcheckbox= this.document.forms[0].HDabilitato')
            //eval('HIDcheckbox=<%=Me.HDabilitato.ClientID%>')
            HIDcheckbox = this.document.getElementById('<%=Me.HDabilitato.ClientID%>');
            selezionati = 0
            for (i = 0; i < document.forms[0].length; i++) {
                e = document.forms[0].elements[i];
                if (e.type == 'checkbox' && e.name.indexOf("CBabilitato") != -1) {
                    if (e.checked == true) {
                        selezionati++
                        if (HIDcheckbox.value == "") {
                            HIDcheckbox.value = ',' + e.value + ','
                        }
                        else {
                            pos1 = HIDcheckbox.value.indexOf(',' + e.value + ',')
                            if (pos1 == -1)
                                HIDcheckbox.value = HIDcheckbox.value + e.value + ','
                        }
                    }
                    else {
                        valore = HIDcheckbox.value
                        pos1 = HIDcheckbox.value.indexOf(',' + e.value + ',')
                        if (pos1 != -1) {
                            stringa = ',' + e.value
                            HIDcheckbox.value = HIDcheckbox.value.substring(0, pos1)
                            HIDcheckbox.value = HIDcheckbox.value + valore.substring(pos1 + e.value.length + 1, valore.length)
                        }
                    }
                }
            }
            if (HIDcheckbox.value == ",")
                HIDcheckbox.value = ""
        }

        function SelectAll(SelectAllBox) {
            var actVar = SelectAllBox.checked;
            var TBcheckbox;
            //eval('HDabilitato= this.document.forms[0].HDabilitato')
            //eval('HIDcheckbox=<%=Me.HDabilitato.ClientID%>')
            HIDcheckbox = this.document.getElementById('<%=Me.HDabilitato.ClientID%>');
            HDabilitato.value = ""
            for (i = 0; i < document.forms[0].length; i++) {
                e = document.forms[0].elements[i];
                if (e.type == 'checkbox' && e.name.indexOf("CBabilitato") != -1) {
                    e.checked = actVar;
                    if (e.checked == true)
                        if (HDabilitato.value == "")
                            HDabilitato.value = ',' + e.value + ','
                        else
                            HDabilitato.value = HDabilitato.value + e.value + ','
                    }
                }
            }
		
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <table width="900px" align="center">
        <tr>
            <td bgcolor="#a3b2cd" height="1">
            </td>
        </tr>
        <%--		<tr>
			<td class="titolo" align="center"><asp:label id="LBtitolo" CssClass="TitoloServizio" Runat="server">- Import utenti -</asp:label></td>
		</tr>--%>
        <tr>
            <td bgcolor="#a3b2cd" height="1">
            </td>
        </tr>
        <tr>
            <td align="center">
                <input type="hidden" id="HDN_valoreSorgente" runat="server" />
                <input type="hidden" id="HDN_valoreDestinatario" runat="server" />
                <input id="HDabilitato" type="hidden" name="HDabilitato" runat="server" />
                <input id="HDattivato" type="hidden" name="HDattivato" runat="server" />
                <input id="HDtutti" type="hidden" name="HDtutti" runat="server" />
                <asp:Panel ID="PNLpermessi" runat="server" HorizontalAlign="Center" Visible="False">
                    <table align="center">
                        <tr>
                            <td height="50">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="LBNopermessi" runat="server" CssClass="messaggio"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" height="50">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PNLcontenuto" runat="server" HorizontalAlign="Center" Visible="False">
                    <table align="center" cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td colspan="5">
                                <telerik:RadTabStrip ID="TBSmenu" runat="server" Align="Justify" Width="100%" Height="26px"
                                    CausesValidation="false" AutoPostBack="true" Skin="Outlook" EnableEmbeddedSkins="true">
                                    <Tabs>
                                        <telerik:RadTab Text="Sorgente - Destinazione" Value="TABsorgente">
                                        </telerik:RadTab>
                                        <telerik:RadTab Text="Scelta Utenti" Value="TAButenti">
                                        </telerik:RadTab>
                                        <telerik:RadTab Text="Definizione Ruoli" Value="TABruoli">
                                        </telerik:RadTab>
                                        <telerik:RadTab Text="Informazioni" Value="TABfinale">
                                        </telerik:RadTab>
                                    </Tabs>
                                </telerik:RadTabStrip>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Panel ID="PNLsorgente" runat="server" Visible="False" HorizontalAlign="Center">
                                    <table align="center" border="0" cellspacing="0">
                                        <tr>
                                            <td colspan="5">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="10px" height="10px" class="nosize0">
                                                &nbsp;
                                            </td>
                                            <td valign="middle" align="center" rowspan="5" class="importTitolo_Sorgente">
                                                <asp:Label ID="LBsorgente" runat="server">Sorgente</asp:Label>
                                            </td>
                                            <td colspan="2" class="importSeparatore_Sorgente" height="10px">
                                                &nbsp;
                                            </td>
                                            <td width="10px" height="10px" class="importSeparatore_Sorgente">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="10px">
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Table ID="TBLsorgente" runat="server">
                                                    <asp:TableRow ID="TBRfiltro">
                                                        <asp:TableCell ColumnSpan="2" HorizontalAlign="Center">
                                                            <asp:Table ID="TBLfiltro" runat="server" HorizontalAlign="Center">
                                                                <asp:TableRow Height="25px">
                                                                    <asp:TableCell ColumnSpan="5">
                                                                        <asp:Table runat="server" CellPadding="2" CellSpacing="2" BorderStyle="None" ID="Table1">
                                                                            <asp:TableRow>
                                                                                <asp:TableCell ID="TBCorganizzazione_c">
                                                                                    <asp:Label ID="LBorganizzazione_c" runat="server" CssClass="FiltroVoce">Organizzazione:&nbsp;</asp:Label>
                                                                                </asp:TableCell>
                                                                                <asp:TableCell ID="TBCorganizzazione">
                                                                                    <asp:DropDownList ID="DDLorganizzazione" runat="server" AutoPostBack="True" CssClass="FiltroCampo">
                                                                                    </asp:DropDownList>
                                                                                </asp:TableCell>
                                                                                <asp:TableCell>
                                                                                    <asp:Label ID="LBtipoFiltro" runat="server" CssClass="FiltroVoce">Tipo Comunità:&nbsp;</asp:Label>
                                                                                </asp:TableCell>
                                                                                <asp:TableCell>
                                                                                    <asp:DropDownList ID="DDLTipo" runat="server" CssClass="FiltroCampo" AutoPostBack="true">
                                                                                    </asp:DropDownList>
                                                                                </asp:TableCell>
                                                                            </asp:TableRow>
                                                                        </asp:Table>
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow ID="TBRcorsi" runat="server" Visible="False">
                                                                    <asp:TableCell ColumnSpan="5" Height="25px">
                                                                        <asp:Table ID="TBLcorsi" CellPadding="2" CellSpacing="2" BorderStyle="None" runat="server">
                                                                            <asp:TableRow>
                                                                                <asp:TableCell>
                                                                                    <asp:Label ID="LBannoAccademico_c" runat="server" CssClass="FiltroVoce">A.A.:&nbsp;</asp:Label>
                                                                                </asp:TableCell>
                                                                                <asp:TableCell>
                                                                                    <asp:DropDownList ID="DDLannoAccademico" runat="server" AutoPostBack="True" CssClass="FiltroCampo">
                                                                                    </asp:DropDownList>
                                                                                </asp:TableCell>
                                                                                <asp:TableCell>
                                                                                    <asp:Label ID="LBperiodo_c" runat="server" CssClass="FiltroVoce">Periodo:&nbsp;</asp:Label>
                                                                                </asp:TableCell>
                                                                                <asp:TableCell>
                                                                                    <asp:DropDownList ID="DDLperiodo" runat="server" AutoPostBack="True" CssClass="FiltroCampo">
                                                                                    </asp:DropDownList>
                                                                                </asp:TableCell>
                                                                            </asp:TableRow>
                                                                        </asp:Table>
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow ID="TBRcorsoDiStudi" runat="server" Visible="False">
                                                                    <asp:TableCell ColumnSpan="5" Height="25px">
                                                                        <asp:Table ID="Table2" CellPadding="2" CellSpacing="2" BorderStyle="None" runat="server">
                                                                            <asp:TableRow>
                                                                                <asp:TableCell>
                                                                                    <asp:Label ID="LBtipoCorsoDiStudi" runat="server" CssClass="FiltroVoce">Tipologia:&nbsp;</asp:Label>
                                                                                </asp:TableCell>
                                                                                <asp:TableCell>
                                                                                    <asp:DropDownList ID="DDLtipoCorsoStudi" runat="server" AutoPostBack="True" CssClass="FiltroCampo">
                                                                                    </asp:DropDownList>
                                                                                </asp:TableCell>
                                                                            </asp:TableRow>
                                                                        </asp:Table>
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                            </asp:Table>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                            <td width="10px" class="importSeparatore_Sorgente">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="10px">
                                                &nbsp;
                                            </td>
                                            <td width="10px">
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TXBsorgente" runat="server" ReadOnly="True" Columns="70" CssClass="FiltroCampo"></asp:TextBox>
                                                <img onclick="ToggleTreeView('divSorgente')" src="./../RadControls/TreeView/Skins/Comunita/boxArrow.gif"
                                                    style="margin-left: -3px;" alt="" />
                                                <br />
                                                <div id="divSorgente" style="position: absolute; visibility: hidden; border: solid 1px;
                                                    width: 500px; background: white; height: 350px; zindex=1000;">
                                                    <radt:RadTreeView ID="RDTcomunita" runat="server" Height="350px" Width="500px" AutoPostBack="True"
                                                        BeforeClientClick="ProcessClientClick" CssFile="~/RadControls/TreeView/Skins/Comunita/StyleImport.css"
                                                        ImagesBaseDir="~/RadControls/TreeView/Skins/Comunita/" Skin="Comunita" />
                                                </div>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" EnableClientScript="True"
                                                    ControlToValidate="TXBsorgente" runat="server" Text="*"></asp:RequiredFieldValidator>
                                            </td>
                                            <td width="10px" class="importSeparatore_Sorgente">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="10px">
                                                &nbsp;
                                            </td>
                                            <td colspan="2">
                                                &nbsp;
                                            </td>
                                            <td width="10px" class="importSeparatore_Sorgente">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="10px">
                                                &nbsp;
                                            </td>
                                            <td width="10px" class="importSeparatore_Sorgente">
                                                &nbsp;
                                            </td>
                                            <td class="importSeparatore_Sorgente">
                                                &nbsp;
                                            </td>
                                            <td width="10px" class="importSeparatore_Sorgente">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="10px" height="10px" class="nosize0">
                                                &nbsp;
                                            </td>
                                            <td valign="middle" align="center" rowspan="5" class="importTitolo_Destinatario">
                                                <asp:Label ID="LBdestinatario" runat="server">Destinatario</asp:Label>
                                            </td>
                                            <td colspan="2" class="importSeparatore_Destinatario" height="10px">
                                                &nbsp;
                                            </td>
                                            <td width="10px" height="10px" class="importSeparatore_Destinatario">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="10px">
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Table ID="TBLdestinatario" runat="server">
                                                    <asp:TableRow ID="TBRfiltroDest">
                                                        <asp:TableCell ColumnSpan="2" HorizontalAlign="Center">
                                                            <asp:Table ID="TBLfiltroDest" runat="server" HorizontalAlign="Center">
                                                                <asp:TableRow Height="25px">
                                                                    <asp:TableCell ColumnSpan="5">
                                                                        <asp:Table runat="server" CellPadding="2" CellSpacing="2" BorderStyle="None" ID="Table4">
                                                                            <asp:TableRow>
                                                                                <asp:TableCell ID="TBCorganizzazioneDest_c">
                                                                                    <asp:Label ID="Label1" runat="server" CssClass="FiltroVoce">Organizzazione:&nbsp;</asp:Label>
                                                                                </asp:TableCell>
                                                                                <asp:TableCell ID="TBCorganizzazioneDest">
                                                                                    <asp:DropDownList ID="DDLorganizzazioneDest" runat="server" AutoPostBack="True" CssClass="FiltroCampo">
                                                                                    </asp:DropDownList>
                                                                                </asp:TableCell>
                                                                                <asp:TableCell>
                                                                                    <asp:Label ID="LBtipoFiltroDest" runat="server" CssClass="FiltroVoce">Tipo Comunità:&nbsp;</asp:Label>
                                                                                </asp:TableCell>
                                                                                <asp:TableCell>
                                                                                    <asp:DropDownList ID="DDLTipoDest" runat="server" CssClass="FiltroCampo" AutoPostBack="true">
                                                                                    </asp:DropDownList>
                                                                                </asp:TableCell>
                                                                            </asp:TableRow>
                                                                        </asp:Table>
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow ID="TBRcorsiDest" runat="server" Visible="False">
                                                                    <asp:TableCell ColumnSpan="5" Height="25px">
                                                                        <asp:Table ID="Table5" CellPadding="2" CellSpacing="2" BorderStyle="None" runat="server">
                                                                            <asp:TableRow>
                                                                                <asp:TableCell>
                                                                                    <asp:Label ID="LBannoAccademicoDest_c" runat="server" CssClass="FiltroVoce">A.A.:&nbsp;</asp:Label>
                                                                                </asp:TableCell>
                                                                                <asp:TableCell>
                                                                                    <asp:DropDownList ID="DDLannoAccademicoDest" runat="server" AutoPostBack="True" CssClass="FiltroCampo">
                                                                                    </asp:DropDownList>
                                                                                </asp:TableCell>
                                                                                <asp:TableCell>
                                                                                    <asp:Label ID="LBperiodoDest_c" runat="server" CssClass="FiltroVoce">Periodo:&nbsp;</asp:Label>
                                                                                </asp:TableCell>
                                                                                <asp:TableCell>
                                                                                    <asp:DropDownList ID="DDLperiodoDest" runat="server" AutoPostBack="True" CssClass="FiltroCampo">
                                                                                    </asp:DropDownList>
                                                                                </asp:TableCell>
                                                                            </asp:TableRow>
                                                                        </asp:Table>
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow ID="TBRcorsoDiStudiDest" runat="server" Visible="False">
                                                                    <asp:TableCell ColumnSpan="5" Height="25px">
                                                                        <asp:Table ID="Table6" CellPadding="2" CellSpacing="2" BorderStyle="None" runat="server">
                                                                            <asp:TableRow>
                                                                                <asp:TableCell>
                                                                                    <asp:Label ID="LBtipoCorsoDiStudiDest" runat="server" CssClass="FiltroVoce">Tipologia:&nbsp;</asp:Label>
                                                                                </asp:TableCell>
                                                                                <asp:TableCell>
                                                                                    <asp:DropDownList ID="DDLtipoCorsoStudiDest" runat="server" AutoPostBack="True" CssClass="FiltroCampo">
                                                                                    </asp:DropDownList>
                                                                                </asp:TableCell>
                                                                            </asp:TableRow>
                                                                        </asp:Table>
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                            </asp:Table>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                            <td width="10px" class="importSeparatore_Destinatario">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="10px">
                                                &nbsp;
                                            </td>
                                            <td width="10px">
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TXBdestinatario" runat="server" ReadOnly="True" Columns="70" CssClass="FiltroCampo"></asp:TextBox>
                                                <img onclick="ToggleTreeView('divDestinatario')" src="./../RadControls/TreeView/Skins/Comunita/boxArrow.gif"
                                                    style="margin-left: -3px;" alt="" />
                                                <br />
                                                <div id="divDestinatario" style="position: absolute; visibility: hidden; border: solid 1px;
                                                    width: 500px; background: white; height: 350px; zindex=1000;">
                                                    <radt:RadTreeView ID="RDTdestinatario" runat="server" Height="350px" Width="500px"
                                                        AutoPostBack="True" BeforeClientClick="ProcessClientClick" CssFile="~/RadControls/TreeView/Skins/Comunita/StyleImport.css"
                                                        ImagesBaseDir="~/RadControls/TreeView/Skins/Comunita/" Skin="Comunita" />
                                                </div>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" EnableClientScript="True"
                                                    ControlToValidate="TXBdestinatario" runat="server" Text="*"></asp:RequiredFieldValidator>
                                            </td>
                                            <td width="10px" class="importSeparatore_Destinatario">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="10px">
                                                &nbsp;
                                            </td>
                                            <td width="10px">
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td width="10px" class="importSeparatore_Destinatario">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="10px">
                                                &nbsp;
                                            </td>
                                            <td colspan="4" class="importSeparatore_Destinatario">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="5" height="30px">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="10px">
                                                &nbsp;
                                            </td>
                                            <td colspan="3" align="right">
                                                <asp:Button ID="BTNfase0ToFase1" runat="server" Text="Avanti" CausesValidation="True"
                                                    CssClass="Pulsante"></asp:Button>
                                            </td>
                                            <td width="10px">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <input type="hidden" id="HDNsorgente_ID" runat="server" />
                                <input type="hidden" id="HDNdestinazione_ID" runat="server" name="Hidden1" />
                                <asp:Panel ID="PNLutenti" runat="server" Visible="False" HorizontalAlign="Center">
                                    <table width="100%" align="center" border="0">
                                        <tr>
                                            <td align="center">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <table cellspacing="1" cellpadding="1" width="100%" border="0">
                                                    <tr>
                                                        <td class="FiltroVoce" align="left">
                                                            <asp:Label ID="LBsorgenteCmnt_t" runat="server">Comunità Sorgente:</asp:Label>
                                                        </td>
                                                        <td class="FiltroCampo">
                                                            <asp:Label ID="LBsorgenteCmnt" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="FiltroVoce" align="left">
                                                            <asp:Label ID="LBdestinazioneCmnt_t" runat="server">Comunità Destinazione:</asp:Label>
                                                        </td>
                                                        <td class="FiltroCampo">
                                                            <asp:Label ID="LBdestinazioneCmnt" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="LBsceltaUtenti" runat="server" CssClass="FiltroVoce">Scelta utenti:</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:RadioButtonList ID="RBLutenti" runat="server" CssClass="FiltroCampo" RepeatDirection="Horizontal"
                                                                AutoPostBack="True">
                                                                <asp:ListItem Value="0" Selected="True">Seleziona Singoli</asp:ListItem>
                                                                <asp:ListItem Value="1">Seleziona per Ruolo</asp:ListItem>
                                                                <asp:ListItem Value="2">Seleziona Tutti</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Panel ID="PNLfiltro" runat="server">
                                                    <fieldset>
                                                        <legend class="tableLegend">Filtro</legend>
                                                        <br />
                                                        <table cellspacing="1" cellpadding="1" width="100%" border="0">
                                                            <tr>
                                                                <td class="FiltroVoce">
                                                                    <asp:Label ID="LBtipoRuolo" runat="server">Tipo Ruolo:</asp:Label>
                                                                    <asp:Label ID="LBtipoPersona" runat="server" Visible="False">Tipo Persona:</asp:Label>
                                                                </td>
                                                                <td class="FiltroVoce">
                                                                    Numero Record
                                                                </td>
                                                                <td class="FiltroVoce">
                                                                    Tipo Ricerca
                                                                </td>
                                                                <td class="FiltroVoce">
                                                                    Valore
                                                                </td>
                                                                <td class="FiltroVoce">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownList ID="DDLTipoRuolo" runat="server" CssClass="FiltroCampo" AutoPostBack="true">
                                                                    </asp:DropDownList>
                                                                    <asp:DropDownList ID="DDLTipoPersona" runat="server" CssClass="FiltroCampo" AutoPostBack="true"
                                                                        Visible="False">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="DDLNumeroRecord" CssClass="FiltroCampo" runat="server" AutoPostBack="true">
                                                                        <asp:ListItem Value="20" Selected="true"></asp:ListItem>
                                                                        <asp:ListItem Value="40"></asp:ListItem>
                                                                        <asp:ListItem Value="80"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="DDLTipoRicerca" CssClass="FiltroCampo" runat="server" AutoPostBack="false">
                                                                        <asp:ListItem Value="-2" Selected="true">Nome</asp:ListItem>
                                                                        <asp:ListItem Value="-3">Cognome</asp:ListItem>
                                                                        <asp:ListItem Value="-4">Cognome/nome</asp:ListItem>
                                                                        <asp:ListItem Value="-5">Data di nascita(gg/mm/aaaa)</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="TXBValore" CssClass="FiltroCampo" runat="server"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="BTNCerca" CssClass="pulsante" runat="server" Text="Cerca"></asp:Button>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="5">
                                                                    <table width="60%" align="center">
                                                                        <tr>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBtutti" runat="server" CssClass="lettera" CommandArgument="-1"
                                                                                    OnClick="FiltroLink_Click">Tutti</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBaltro" runat="server" CssClass="lettera" CommandArgument="0"
                                                                                    OnClick="FiltroLink_Click">Altro</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBa" runat="server" CssClass="lettera" CommandArgument="1" OnClick="FiltroLink_Click">A</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBb" runat="server" CssClass="lettera" CommandArgument="2" OnClick="FiltroLink_Click">B</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBc" runat="server" CssClass="lettera" CommandArgument="3" OnClick="FiltroLink_Click">C</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBd" runat="server" CssClass="lettera" CommandArgument="4" OnClick="FiltroLink_Click">D</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBe" runat="server" CssClass="lettera" CommandArgument="5" OnClick="FiltroLink_Click">E</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBf" runat="server" CssClass="lettera" CommandArgument="6" OnClick="FiltroLink_Click">F</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBg" runat="server" CssClass="lettera" CommandArgument="7" OnClick="FiltroLink_Click">G</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBh" runat="server" CssClass="lettera" CommandArgument="8" OnClick="FiltroLink_Click">H</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBi" runat="server" CssClass="lettera" CommandArgument="9" OnClick="FiltroLink_Click">I</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBj" runat="server" CssClass="lettera" CommandArgument="10"
                                                                                    OnClick="FiltroLink_Click">J</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBk" runat="server" CssClass="lettera" CommandArgument="11"
                                                                                    OnClick="FiltroLink_Click">K</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBl" runat="server" CssClass="lettera" CommandArgument="12"
                                                                                    OnClick="FiltroLink_Click">L</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBm" runat="server" CssClass="lettera" CommandArgument="13"
                                                                                    OnClick="FiltroLink_Click">M</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBn" runat="server" CssClass="lettera" CommandArgument="14"
                                                                                    OnClick="FiltroLink_Click">N</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBo" runat="server" CssClass="lettera" CommandArgument="15"
                                                                                    OnClick="FiltroLink_Click">O</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBp" runat="server" CssClass="lettera" CommandArgument="16"
                                                                                    OnClick="FiltroLink_Click">P</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBq" runat="server" CssClass="lettera" CommandArgument="17"
                                                                                    OnClick="FiltroLink_Click">Q</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBr" runat="server" CssClass="lettera" CommandArgument="18"
                                                                                    OnClick="FiltroLink_Click">R</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBs" runat="server" CssClass="lettera" CommandArgument="19"
                                                                                    OnClick="FiltroLink_Click">S</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBt" runat="server" CssClass="lettera" CommandArgument="20"
                                                                                    OnClick="FiltroLink_Click">T</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBu" runat="server" CssClass="lettera" CommandArgument="21"
                                                                                    OnClick="FiltroLink_Click">U</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBv" runat="server" CssClass="lettera" CommandArgument="22"
                                                                                    OnClick="FiltroLink_Click">V</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBw" runat="server" CssClass="lettera" CommandArgument="23"
                                                                                    OnClick="FiltroLink_Click">W</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBx" runat="server" CssClass="lettera" CommandArgument="24"
                                                                                    OnClick="FiltroLink_Click">X</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBy" runat="server" CssClass="lettera" CommandArgument="25"
                                                                                    OnClick="FiltroLink_Click">Y</asp:LinkButton>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="LKBz" runat="server" CssClass="lettera" CommandArgument="26"
                                                                                    OnClick="FiltroLink_Click">Z</asp:LinkButton>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                    <table cellspacing="1" cellpadding="1" width="100%" border="0">
                                        <tr>
                                            <td align="center">
                                                <asp:DataGrid ID="DGPersone" runat="server" DataKeyField="PRSN_id" AllowPaging="true"
                                                    AutoGenerateColumns="False" ShowFooter="false" AllowSorting="true" CssClass="DataGrid_Generica">
                                                    <AlternatingItemStyle CssClass="Righe_Alternate_Center"></AlternatingItemStyle>
                                                    <HeaderStyle CssClass="Riga_Header"></HeaderStyle>
                                                    <ItemStyle CssClass="Righe_Normali_center"></ItemStyle>
                                                    <PagerStyle CssClass="Riga_Paginazione" Position="Bottom" Mode="NumericPages" Visible="true"
                                                        HorizontalAlign="Right" Height="25px" VerticalAlign="Bottom"></PagerStyle>
                                                    <Columns>
                                                        <asp:BoundColumn DataField="PRSN_id" Visible="False"></asp:BoundColumn>
                                                        <asp:TemplateColumn runat="server" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="IMBmodifica" runat="server" CausesValidation="False" CommandName="Aggiungi"
                                                                    ImageUrl="./../images/add.gif"></asp:ImageButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn runat="server" HeaderText="" ItemStyle-HorizontalAlign="Center"
                                                            ItemStyle-Width="10">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="IMBinfo" runat="server" CausesValidation="False" CommandName="infoPersona"
                                                                    ImageUrl="../images/proprieta.gif"></asp:ImageButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="PRSN_Anagrafica" HeaderText="Anagrafica" SortExpression="PRSN_Anagrafica"
                                                            ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="PRSN_login" HeaderText="Login" SortExpression="PRSN_login"
                                                            ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
                                                        <asp:TemplateColumn runat="server" HeaderText="Mail" ItemStyle-HorizontalAlign="Center"
                                                            ItemStyle-Width="10">
                                                            <ItemTemplate>
                                                                <asp:HyperLink NavigateUrl='<%# "mailto:" & Container.Dataitem("PRSN_mail")%>' Text='<%# Container.Dataitem("PRSN_mail")%>'
                                                                    runat="server" ID="HYPMail" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn runat="server" HeaderText="Nato il" ItemStyle-HorizontalAlign="Center"
                                                            ItemStyle-Width="10" SortExpression="PRSN_datanascita" Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="LBdataNascitaGriglia">
																	<%# DataBinder.Eval(Container.DataItem, "oPRSN_datanascita") %>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="CMNT_nome" HeaderText="Comunità" SortExpression="CMNT_nome"
                                                            Visible="False"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="TPRL_nome" HeaderText="Ruolo" SortExpression="TPRL_nome"
                                                            ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="PRSN_TPPR_id" Visible="False"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="TPPR_descrizione" HeaderText="Tipo Persona" SortExpression="TPPR_descrizione"
                                                            ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
                                                        <asp:TemplateColumn ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                <input type="checkbox" id="SelectAll2" onclick="SelectAll(this);" runat="server"
                                                                    name="SelectAll" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <input type="checkbox" value='<%# DataBinder.Eval(Container.DataItem, "PRSN_id") %>'
                                                                    id="CBabilitato" name="CBabilitato" <%# DataBinder.Eval(Container.DataItem, "oCheckAbilitato") %>
                                                                    onclick="SelectMe(this);">
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                    </Columns>
                                                    <PagerStyle Width="600px" PageButtonCount="5" Mode="NumericPages"></PagerStyle>
                                                </asp:DataGrid>
                                                <asp:Panel ID="PNLNoUsers" runat="server" Visible="False">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <br />
                                                                <br />
                                                                <asp:Label ID="LBMessaggio" CssClass="avviso" runat="server"></asp:Label>
                                                                <br />
                                                                <br />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <asp:Panel ID="PNLsceltaRuoli" runat="server" HorizontalAlign="Center">
                                                    <table align="center">
                                                        <tr>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="top">
                                                                <asp:Label ID="LBsceltaRuoli" CssClass="FiltroVoce" runat="server">Scelta ruoli da importare:</asp:Label>
                                                            </td>
                                                            <td class="top">
                                                                <asp:CheckBoxList ID="CBLsceltaRuoli" runat="server" RepeatDirection="Horizontal"
                                                                    RepeatColumns="2">
                                                                </asp:CheckBoxList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <asp:Label ID="LBsceltaRuoliDesc" runat="server">
																	ATTENZIONE: Sono visibili i soli ruoli della comunità sorgente 
																	con degli utenti associati e non presenti nella comunità di destinazione.
                                                                </asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Button ID="BTNfase1ToFase2" runat="server" CssClass="Pulsante" Text="Avanti">
                                                </asp:Button>
                                                &nbsp;&nbsp;&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="PNLruoliDestinazione" runat="server" HorizontalAlign="Center" Visible="False">
                                    <table width="100%" align="center" border="0">
                                        <tr>
                                            <td align="center" colspan="4">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="4">
                                                <asp:Label ID="LBtitoloRuoli" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td colspan="2">
                                                <asp:DataGrid ID="DGruoli" runat="server" BorderColor="#8080FF" DataKeyField="TPRL_ID"
                                                    AllowPaging="true" AutoGenerateColumns="False" BackColor="transparent" ShowFooter="false"
                                                    Font-Size="8" AllowSorting="true" Width="100%">
                                                    <AlternatingItemStyle CssClass="Righe_Alternate_Center"></AlternatingItemStyle>
                                                    <HeaderStyle CssClass="Riga_Header"></HeaderStyle>
                                                    <ItemStyle CssClass="Righe_Normali_center"></ItemStyle>
                                                    <PagerStyle CssClass="Riga_Paginazione" Position="Bottom" Mode="NumericPages" Visible="true"
                                                        HorizontalAlign="Right" Height="25px" VerticalAlign="Bottom"></PagerStyle>
                                                    <Columns>
                                                        <asp:BoundColumn DataField="TPRL_ID" Visible="False"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="TPRL_Gerarchia" Visible="False"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="TPRL_Nome"></asp:BoundColumn>
                                                        <asp:TemplateColumn ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <img src="./../images/doppiafreccia.gif" alt="" align="middle">
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="DDLruoloDest" runat="server" CssClass="FiltroCampo">
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                    </Columns>
                                                </asp:DataGrid>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td colspan="2" align="right">
                                                <asp:Button ID="BTNimporta" CssClass="Pulsante" runat="server" Text="Importa"></asp:Button>
                                                &nbsp;&nbsp;&nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="PNLfinale" runat="server" HorizontalAlign="Center" Visible="False">
                                    <table width="100%" align="center" border="0">
                                        <tr>
                                            <td align="center" colspan="4">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td colspan="2" align="center">
                                                <asp:Label ID="LBfinale" CssClass="avviso_normal" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td colspan="2" align="right">
                                                <asp:Button ID="BTNreImport" Text="Importa Nuovi utenti" CssClass="Pulsante" CausesValidation="False"
                                                    runat="server"></asp:Button>
                                                &nbsp;&nbsp;&nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>
