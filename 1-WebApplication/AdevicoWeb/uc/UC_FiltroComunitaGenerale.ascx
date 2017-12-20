<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_FiltroComunitaGenerale.ascx.vb" Inherits="Comunita_OnLine.UC_FiltroComunitaGenerale" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>

<%@ Register TagPrefix="radTree" Namespace="Telerik.WebControls" Assembly="RadTreeView.net2" %>
<script language="javascript" type="text/javascript">

	function SetTextBox(id, s){
		
		document.getElementById(id).value = CodificaHTML(s);
		HideTreeView();
		}
	
	function CodificaHTML(stringa){
	var pos;
	var s;
	
	pos = stringa.indexOf("&lt;")
	if (pos > 0)
		s = stringa.substring(0,pos)
	else{
		pos = stringa.indexOf("<")
		if (pos > 0)
			s = stringa.substring(0,pos)
		else
			s = stringa	
		}	
	function CodificaHTML(stringa){
	var pos;
	var s;
	
	pos = stringa.indexOf("&lt;")
	if (pos > 0)
		s = stringa.substring(0,pos)
	else{
		pos = stringa.indexOf("<")
		if (pos > 0)
			s = stringa.substring(0,pos)
		else
			s = stringa	
		}
	s = s.replace("&#32;"," ")
	s = s.replace("&nbsp;"," ")
	s = s.replace("&#160;"," ")
	s = s.replace("&#39;","'")
	s = s.replace("&#45;","-")
	s = s.replace("&shy;","­")
	s = s.replace("&#173;","­")
	s = s.replace("&#33;","!")
	s = s.replace("&#34;","\"")
	s = s.replace("&quot;","\"")
	s = s.replace("&#35;","#")
	s = s.replace("&#36;","$")
	s = s.replace("&#37;","%")
	s = s.replace("&#38;","&")
	s = s.replace("&amp;","&")
	s = s.replace("&#40;","(")
	s = s.replace("&#41;",")")
	s = s.replace("&#42;","*")
	s = s.replace("&#44;",",")
	s = s.replace("&#46;",".")
	s = s.replace("&#47;","/")
	s = s.replace("&#58;",":")
	s = s.replace("&#59;",";")
	s = s.replace("&#63;","?")
	s = s.replace("&#64;","@")
	s = s.replace("&#91;","[")
	s = s.replace("&#92;","\\")
	s = s.replace("&#93;","]")
	s = s.replace("&#94;","^")
	s = s.replace("&#95;","_")
	s = s.replace("&#96;","`")
	s = s.replace("&#123;","{")
	s = s.replace("&#124;","|")
	s = s.replace("&#125;","}")
	s = s.replace("&#126;","~")
	s = s.replace("&#161;","¡")
	s = s.replace("&iexcl;","¡")
	s = s.replace("&#166;","¦")
	s = s.replace("&brvbar;","¦")
	s = s.replace("&#168;","¨")
	s = s.replace("&uml;","¨")
	s = s.replace("&macr;","¯")
	s = s.replace("&#175;","¯")
	s = s.replace("&#180;","´")
	s = s.replace("&acute;","´")
	s = s.replace("&cedil;","¸")
	s = s.replace("&#184;","¸")
	s = s.replace("&#191;","¿")
	s = s.replace("&iquest;","¿")
	s = s.replace("&#43;","+")
	s = s.replace("&#60;","<")
	s = s.replace("&lt;","<")
	s = s.replace("&#61;","=")
	s = s.replace("&gt;",">")
	s = s.replace("&#62;",">")
	s = s.replace("&#177;","±")
	s = s.replace("&plusmn;","±")
	s = s.replace("&#171;","«")
	s = s.replace("&#187;","»")
	s = s.replace("&raquo;","»")
	s = s.replace("&times;","×")
	s = s.replace("&#215;","×")
	s = s.replace("&#247;","÷")
	s = s.replace("&divide;","÷")
	s = s.replace("&cent;","¢")
	s = s.replace("&#162;","¢")
	s = s.replace("&pound;","£")
	s = s.replace("&#163;","£")
	s = s.replace("&curren;","¤")
	s = s.replace("&#164;","¤")
	s = s.replace("&yen;","¥")
	s = s.replace("&#165;","¥")
	s = s.replace("&#167;","§")
	s = s.replace("&sect;","§")
s = s.replace("&copy;","©")
s = s.replace("&#169;","©")
s = s.replace("&#172;","¬")
s = s.replace("&not;","¬")
s = s.replace("&reg;","®")
s = s.replace("&#174;","®")
s = s.replace("&deg;","°")
s = s.replace("&#176;","°")
s = s.replace("&micro;","µ")
s = s.replace("&#181;","µ")
s = s.replace("&#182;","¶")
s = s.replace("&para;","¶")
s = s.replace("&#183;","·")
s = s.replace("&middot;","·")
s = s.replace("&euro;","€")
s = s.replace("&#48;","0")
s = s.replace("&#188;","¼")
s = s.replace("&frac14;","¼")
s = s.replace("&frac12;","½")
s = s.replace("&#189;","½")
s = s.replace("&frac34;","¾")
s = s.replace("&#190;","¾")
s = s.replace("&#49;","1")
s = s.replace("&#185;","¹")
s = s.replace("&sup1;","¹")
s = s.replace("&#178;","²")
s = s.replace("&#50;","2")
s = s.replace("&sup2;","²")
s = s.replace("&#179;","³")
s = s.replace("&#51;","3")
s = s.replace("&sup3;","³")
s = s.replace("&#52;","4")
s = s.replace("&#53;","5")
s = s.replace("&#54;","6")
s = s.replace("&#55;","7")
s = s.replace("&#56;","8")
s = s.replace("&#57;","9")
s = s.replace("&#65;","A")
s = s.replace("&#97;","a")
s = s.replace("&#170;","ª")
s = s.replace("&ordf;","ª")
s = s.replace("&#225;","á")
s = s.replace("&aacute;","á")
s = s.replace("&Aacute;","Á")
s = s.replace("&#193;","Á")
s = s.replace("&agrave;","à")
s = s.replace("&#224;","à")
s = s.replace("&Agrave;","À")
s = s.replace("&#192;","À")
s = s.replace("&acirc;","â")
s = s.replace("&#194;","Â")
s = s.replace("&#226;","â")
s = s.replace("&auml;","ä")
s = s.replace("&Auml;","Ä")
s = s.replace("&#228;","ä")
s = s.replace("&#196;","Ä")
s = s.replace("&#259;","a")
s = s.replace("&#258;","A")
s = s.replace("&#257;","a")
s = s.replace("&#256;","A")
s = s.replace("&atilde;","ã")
s = s.replace("&#227;","ã")
s = s.replace("&#195;","Ã")
s = s.replace("&Atilde;","Ã")
s = s.replace("&aring;","å")
s = s.replace("&#197","Å")
s = s.replace("&#229;","å")
s = s.replace("&Aring;","Å")
s = s.replace("&#260;","A")
s = s.replace("&#261;","a")
s = s.replace("&aelig;","æ")
s = s.replace("&#198;","Æ")
s = s.replace("&AElig;","Æ")
s = s.replace("&#230;","æ")
s = s.replace("&#66;","B")
s = s.replace("&#98;","b")
s = s.replace("&#67;","C")
s = s.replace("&#99;","c")
s = s.replace("&#263;","c")
s = s.replace("&#262;","C")
s = s.replace("&#266;","C")
s = s.replace("&#267;","c")
s = s.replace("&#264;","C")
s = s.replace("&#265;","c")
s = s.replace("&#268;","C")
s = s.replace("&#269;","c")
s = s.replace("&ccedil;","ç")
s = s.replace("&#231;","ç")
s = s.replace("&Ccedil;","Ç")
s = s.replace("&#199;","Ç")
s = s.replace("&#100;","d")
s = s.replace("&#68;","D")
s = s.replace("&#270;","D")
s = s.replace("&#271;","d")
s = s.replace("&#272;","Ð")
s = s.replace("&#273;","d")
s = s.replace("&eth;","ð")
s = s.replace("&ETH;","Ð")
s = s.replace("&#240;","ð")
s = s.replace("&#208;","Ð")
s = s.replace("&#69;","E")
s = s.replace("&#101;","e")
s = s.replace("&#233;","é")
s = s.replace("&Eacute;","É")
s = s.replace("&#201;","É")
s = s.replace("&eacute;","é")
s = s.replace("&Egrave;","È")
s = s.replace("&#200;","È")
s = s.replace("&egrave;","è")
s = s.replace("&#232;","è")
s = s.replace("&#278;","E")
s = s.replace("&#279;","e")
s = s.replace("&#202;","Ê")
s = s.replace("&ecirc;","ê")
s = s.replace("&#234;","ê")
s = s.replace("&Ecirc;","Ê")
s = s.replace("&euml;","ë")
s = s.replace("&#203;","Ë")
s = s.replace("&#235;","ë")
s = s.replace("&#282;","E")
s = s.replace("&#283;","e")
s = s.replace("&#276;","E")
s = s.replace("&#277","e")
s = s.replace("&#274;","E")
s = s.replace("&#275;","e")
s = s.replace("&#280;","E")
s = s.replace("&#281;","e")
s = s.replace("&#102;","f")
s = s.replace("&#70;","F")
s = s.replace("&#103;","g")
s = s.replace("&#71;","G")
s = s.replace("&#288;","G")
s = s.replace("&#289;","g")
s = s.replace("&#284;","G")
s = s.replace("&#285;","g")
s = s.replace("&#286;","G")
s = s.replace("&#287;","g")
s = s.replace("&#290;","G")
s = s.replace("&#291;","g")
s = s.replace("&#104;","h")
s = s.replace("&#72;","H")
s = s.replace("&#293;","h")
s = s.replace("&#292;","H")
s = s.replace("&#295;","h")
s = s.replace("&#294;","H")
s = s.replace("&#105;","i")
s = s.replace("&#73;","I")
s = s.replace("&#305;","i")
s = s.replace("&#237","í")
s = s.replace("&#205;","Í")
s = s.replace("&Iacute;","Í")
s = s.replace("&iacute;","í")
s = s.replace("&Igrave;","Ì")
s = s.replace("&#204;","Ì")
s = s.replace("&igrave;","ì")
s = s.replace("&#236;","ì")
s = s.replace("&#304;","I")
s = s.replace("&#206;","Î")
s = s.replace("&icirc;","î")
s = s.replace("&#238;","î")
s = s.replace("&Icirc;","Î")
s = s.replace("&iuml;","ï")
s = s.replace("&#239;","ï")
s = s.replace("&Iuml;","Ï")
s = s.replace("&#207;","Ï")
s = s.replace("&#301;","i")
s = s.replace("&#300;","I")
s = s.replace("&#298;","I")
s = s.replace("&#299;","i")
s = s.replace("&#296;","I")
s = s.replace("&#297;","i")
s = s.replace("&#302;","I")
s = s.replace("&#303;","i")
s = s.replace("&#306;","?")
s = s.replace("&#307;","?")
s = s.replace("&#106;","j")
s = s.replace("&#74;","J")
s = s.replace("&#309;","j")
s = s.replace("&#308;","J")
s = s.replace("&#107;","k")
s = s.replace("&#75;","K")
s = s.replace("&#312;","?")
s = s.replace("&#311;","k")
s = s.replace("&#310;","K")
s = s.replace("&#76;","L")
s = s.replace("&#108;","l")
s = s.replace("&#314;","l")
s = s.replace("&#313;","L")
s = s.replace("&#319;","?")
s = s.replace("&#320;","?")
s = s.replace("&#317","L")
s = s.replace("&#318;","l")
s = s.replace("&#316;","l")
s = s.replace("&#315;","L")
s = s.replace("&#321;","L")
s = s.replace("&#322;","l")
s = s.replace("&#77;","M")
s = s.replace("&#109;","m")
s = s.replace("&#110;","n")
s = s.replace("&#78;","N")
s = s.replace("&#323;","N")
s = s.replace("&#324;","n")
s = s.replace("&#327;","N")
s = s.replace("&#328;","n")
s = s.replace("&#241;","ñ")
s = s.replace("&Ntilde;","Ñ")
s = s.replace("&ntilde;","ñ")
s = s.replace("&#209;","Ñ")
s = s.replace("&#325;","N")
s = s.replace("&#326;","n")
s = s.replace("&#329;","?")
s = s.replace("&#330;","?")
s = s.replace("&#331;","?")
s = s.replace("&#111;","o")
s = s.replace("&#79;","O")
s = s.replace("&#186;","º")
s = s.replace("&ordm;","º")
s = s.replace("&Oacute;","Ó")
s = s.replace("&#243;","ó")
s = s.replace("&oacute;","ó")
s = s.replace("&#211;","Ó")
s = s.replace("&Ograve;","Ò")
s = s.replace("&ograve;","ò")
s = s.replace("&#210;","Ò")
s = s.replace("&#242;","ò")
s = s.replace("&#212;","Ô")
s = s.replace("&Ocirc;","Ô")
s = s.replace("&ocirc;","ô")
s = s.replace("&#244;","ô")
s = s.replace("&ouml;","ö")
s = s.replace("&Ouml;","Ö")
s = s.replace("&#246;","ö")
s = s.replace("&#214;","Ö")
s = s.replace("&#334;","O")
s = s.replace("&#335;","o")
s = s.replace("&#332;","O")
s = s.replace("&#333;","o")
s = s.replace("&#245;","õ")
s = s.replace("&#213;","Õ")
s = s.replace("&otilde;","õ")
s = s.replace("&Otilde;","Õ")
s = s.replace("&#336;","O")
s = s.replace("&#337;","o")
s = s.replace("&#248;","ø")
s = s.replace("&oslash;","ø")
s = s.replace("&Oslash;","Ø")
s = s.replace("&#216;","Ø")
s = s.replace("&#338;","Œ")
s = s.replace("&#339;","œ")
s = s.replace("&#112;","p")
s = s.replace("&#80;","P")
s = s.replace("&#113;","q")
s = s.replace("&#81;","Q")
s = s.replace("&#114;","r")
s = s.replace("&#82;","R")
s = s.replace("&#341;","r")
s = s.replace("&#341;","r")
s = s.replace("&#340;","R")
s = s.replace("&#340;","R")
s = s.replace("&#344;","R")
s = s.replace("&#345;","r")
s = s.replace("&#344;","R")
s = s.replace("&#345;","r")
s = s.replace("&#342;","R")
s = s.replace("&#342;","R")
s = s.replace("&#343;","r")
s = s.replace("&#343;","r")
s = s.replace("&#83;","S")
s = s.replace("&#115;","s")
s = s.replace("&#347;","s")
s = s.replace("&#346;","S")
s = s.replace("&#347;","s")
s = s.replace("&#346;","S")
s = s.replace("&#348;","S")
s = s.replace("&#349;","s")
s = s.replace("&#349;","s")
s = s.replace("&#348;","S")
s = s.replace("&#353;","š")
s = s.replace("&#352;","Š")
s = s.replace("&#352;","Š")
s = s.replace("&#353;","š")
s = s.replace("&#350;","S")
s = s.replace("&#350;","S")
s = s.replace("&#351;","s")
s = s.replace("&#351;","s")
s = s.replace("&#223;","ß")
s = s.replace("&szlig;","ß")
s = s.replace("&#383;","?")
s = s.replace("&#383;","?")
s = s.replace("&#84;","T")
s = s.replace("&#116;","t")
s = s.replace("&#577;","t")
s = s.replace("&#356;","T")
s = s.replace("&#356;","T")
s = s.replace("&#357","t")
s = s.replace("&#354;","T")
s = s.replace("&#355;","t")
s = s.replace("&#355;","t")
s = s.replace("&#354;","T")
s = s.replace("&#222;","Þ")
s = s.replace("&THORN;","Þ")
s = s.replace("&#254;","þ")
s = s.replace("&thorn;","þ")
s = s.replace("&#359;","t")
s = s.replace("&#358;","T")
s = s.replace("&#358;","T")
s = s.replace("&#359;","t")
s = s.replace("&#117;","u")
s = s.replace("&#85;","U")
s = s.replace("&Uacute;","Ú")
s = s.replace("&#218;","Ú")
s = s.replace("&#250;","ú")
s = s.replace("&uacute;","ú")
s = s.replace("&ugrave;","ù")
s = s.replace("&#217;","Ù")
s = s.replace("&#249;","ù")
s = s.replace("&Ugrave;","Ù")
s = s.replace("&Ucirc;","Û")
s = s.replace("&#251;","û")
s = s.replace("&#219;","Û")
s = s.replace("&ucirc;","û")
s = s.replace("&#252;","ü")
s = s.replace("&Uuml;","Ü")
s = s.replace("&#220;","Ü")
s = s.replace("&uuml;","ü")
s = s.replace("&#364;","U")
s = s.replace("&#365;","u")
s = s.replace("&#364;","U")
s = s.replace("&#365;","u")
s = s.replace("&#363;","u")
s = s.replace("&#362;","U")
s = s.replace("&#362;","U")
s = s.replace("&#363;","u")
s = s.replace("&#361;","u")
s = s.replace("&#360;","U")
s = s.replace("&#361;","u")
s = s.replace("&#360;","U")
s = s.replace("&#367;","u")
s = s.replace("&#367;","u")
s = s.replace("&#366;","U")
s = s.replace("&#366;","U")
s = s.replace("&#370;","U")
s = s.replace("&#371;","u")
s = s.replace("&#371;","u")
s = s.replace("&#370;","U")
s = s.replace("&#368;","U")
s = s.replace("&#369;","u")
s = s.replace("&#368;","U")
s = s.replace("&#369;","u")
s = s.replace("&#118;","v")
s = s.replace("&#86;","V")
s = s.replace("&#119;","w")
s = s.replace("&#87;","W")
s = s.replace("&#373;","w")
s = s.replace("&#373;","w")
s = s.replace("&#372;","W")
s = s.replace("&#372;","W")
s = s.replace("&#88;","X")
s = s.replace("&#120;","x")
s = s.replace("&#89;","Y")
s = s.replace("&#121;","y")
s = s.replace("&yacute;","ý")
s = s.replace("&#221;","Ý")
s = s.replace("&Yacute;","Ý")
s = s.replace("&#253;","ý")
s = s.replace("&#375;","y")
s = s.replace("&#375;","y")
s = s.replace("&#374;","Y")
s = s.replace("&#374;","Y")
s = s.replace("&#376;","Ÿ")
s = s.replace("&#376;","Ÿ")
s = s.replace("&#255;","ÿ")
s = s.replace("&#90;","Z")
s = s.replace("&#122;","z")
s = s.replace("&#378;","z")
s = s.replace("&#377","Z")
s = s.replace("&#378;","z")
s = s.replace("&#377;","Z")
s = s.replace("&#380;","z")
s = s.replace("&#379;","Z")
s = s.replace("&#379;","Z")
s = s.replace("&#380;","z")
s = s.replace("&#382;","ž")
s = s.replace("&#381;","Ž")
s = s.replace("&#382;","ž")
s = s.replace("&#381;","Ž")
	return s
}	

function ToggleTreeView(){
	if (document.getElementById("divDestinatario").style.visibility == "visible")
		HideTreeView();      
	else      
		ShowTreeView();
	}

	function ShowTreeView(){
		document.getElementById("divDestinatario").style.visibility = "visible";
	}
		
	function HideTreeView(){
		document.getElementById("divDestinatario").style.visibility = "hidden";
	}
		
	function ProcessClientClick(node,eventArgs){ 
		if (node.Value =="" || node.Category < 1)
			return false
		document.forms[0].<%= HDN_valoreDestinatario.ClientID %>.value=node.Value
		
		SetTextBox('<%= TXBdestinatario.ClientID %>', node.Text);
		HideTreeView();         
		return true;
	}
</script>
<INPUT id="HDN_ComunitaAttualeID" type="hidden" name="HDN_ComunitaAttualeID" runat="server"/>
<INPUT id="HDN_ComunitaAttualePercorso" type="hidden" name="HDN_ComunitaAttualePercorso" runat="server"/>
<INPUT id="HDN_Livello" type="hidden" name="HDN_Livello" runat="server"/>
<INPUT id="HDN_valoreDestinatario" type="hidden" name="HDN_valoreDestinatario" runat="server"/>
<INPUT id="HDN_ServizioCode" type="hidden" name="HDN_ServizioCode" runat="server"/>
<INPUT id="HDN_hasComunitaForServizio" type="hidden" name="HDN_hasComunitaForServizio" runat="server"/>

<asp:Table ID="TBLFiltroCom" Runat="server" CellPadding="0" CellSpacing="0">
		
	<asp:TableRow id="TBRchiudiFiltro" Height=22px Visible=False>
		<asp:TableCell CssClass="Filtro_CellSelezionato" HorizontalAlign=Center Width=150px Height=22px VerticalAlign=Middle >
			<asp:LinkButton ID="LNBchiudiFiltro" Runat=server CssClass="Filtro_Link" CausesValidation=False>Chiudi Filtri</asp:LinkButton>
		</asp:TableCell>
		<asp:TableCell CssClass="Filtro_CellDeSelezionato" Width=750px Height=22px>&nbsp;
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow id="TBRapriFiltro" Height=22px>
		<asp:TableCell ColumnSpan=1 CssClass="Filtro_CellApriFiltro" HorizontalAlign=Center Width=150px Height=22px>
			<asp:LinkButton ID="LNBapriFiltro" Runat=server CssClass="Filtro_Link" CausesValidation=False >Apri Filtri</asp:LinkButton>
		</asp:TableCell>
		<asp:TableCell CssClass="Filtro_Cellnull" Width=750px Height=22px>&nbsp;
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow ID="TBRfiltri" Visible="False">
		<asp:TableCell HorizontalAlign="Center" ColumnSpan="2" CssClass="Filtro_CellFiltri" width="100%">
			
			<asp:Table ID="TBLfiltro" Runat="server" HorizontalAlign="Center" width="100%">
				<asp:TableRow>
					<asp:TableCell>
						<table cellpadding=0 cellspacing=0>
							<tr>
								<td>
									<asp:Label ID="LBorganizzazione_c" Runat="server" CssClass="FiltroVoceSmall10">Organizzazione:&nbsp;</asp:Label>
									&nbsp;
									<asp:DropDownList ID="DDLorganizzazione" Runat="server" AutoPostBack="True" CssClass="FiltroCampoSmall10"></asp:DropDownList>		
								</td>
								<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
								<td>
									<asp:Label ID="LBtipoComunita_c" Runat="server" CssClass="FiltroVoceSmall10">Tipo Comunita</asp:Label>
									&nbsp;
									<asp:dropdownlist id="DDLTipo" runat="server" CssClass="FiltroCampoSmall10" AutoPostBack="true"></asp:dropdownlist>
								</td>
							</tr>
						</table>
					</asp:TableCell>
					<asp:TableCell HorizontalAlign="Right">
						<asp:button id="BTNCerca" Runat="server" CssClass="PulsanteFiltro" Text="Cerca"  CausesValidation=False></asp:button>
					</asp:TableCell>
				</asp:tablerow>
				<asp:TableRow>
					<asp:TableCell ColumnSpan="2">
						<table cellpadding=0 cellspacing=0>
							<tr>
								<td>
									<asp:Label ID="LBtipoRicerca_c" Runat="server" CssClass="FiltroVoceSmall10">Tipo Ricerca</asp:Label>
									&nbsp;
									<asp:dropdownlist id="DDLTipoRicerca" Runat="server" CssClass="FiltroCampoSmall10">
										<asp:ListItem Value="-2" Selected="true">Nome inizia per</asp:ListItem>
										<asp:ListItem Value="-7">Nome contiene</asp:ListItem>
										<asp:ListItem Value="-3">Creata dopo il</asp:ListItem>
										<asp:ListItem Value="-4">Creata prima del</asp:ListItem>
									</asp:dropdownlist>
								</td>
								<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
								<td>
									<asp:Label ID="LBvalore_c" Runat="server" CssClass="FiltroVoceSmall10">Valore</asp:Label>
									&nbsp;
									<asp:textbox id="TXBValore" Runat="server" CssClass="FiltroCampoSmall10" MaxLength="100" AutoPostBack="False" Columns=50></asp:textbox>
								</td>
								<td>&nbsp;</td>
								<td>
									<asp:Label ID="LBstatoComunita_t" Runat="server" CssClass="FiltroVoceSmall">Stato:</asp:Label>&nbsp;
									<asp:dropdownlist ID="DDLstatoComunita" Runat=server CssClass="FiltroCampoSmall" AutoPostBack=True >
										<asp:ListItem Value=0 Selected=true>Attivate</asp:ListItem>
										<asp:ListItem Value=1>Archiviate</asp:ListItem>
										<asp:ListItem Value=2>Bloccate</asp:ListItem>
									</asp:dropdownlist>
								</td>
							</tr>
						</table>
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow Runat="server" ID="TBRorgnCorsi">
					<asp:TableCell ColumnSpan=2>
						<asp:Table ID="TBLcorsi" CellPadding="2" CellSpacing="2" BorderStyle="None" Runat="server" Visible="False">
							<asp:TableRow>
								<asp:TableCell>
									<asp:Label ID="LBannoAccademico_c" Runat="server" CssClass="FiltroVoceSmall10">A.A.:</asp:Label>&nbsp;
								</asp:TableCell>
								<asp:TableCell>
									<asp:DropDownList ID="DDLannoAccademico" Runat="server" AutoPostBack="True" CssClass="FiltroCampoSmall10"></asp:DropDownList>
								</asp:TableCell>
								<asp:TableCell>
									<asp:Label ID="LBperiodo_c" Runat="server" CssClass="FiltroVoceSmall10">Periodo:&nbsp;</asp:Label>
								</asp:TableCell>
								<asp:TableCell>
									<asp:DropDownList ID="DDLperiodo" Runat="server" AutoPostBack="True" CssClass="FiltroCampoSmall10"></asp:DropDownList>
								</asp:TableCell>
							</asp:TableRow>
						</asp:Table>
						<asp:Table ID="TBLcorsiDiStudio" CellPadding="2" CellSpacing="2" BorderStyle="None" Runat="server" Visible="False">
							<asp:TableRow>
								<asp:TableCell>
									<asp:Label ID="LBcorsoDiStudi_t" Runat="server" CssClass="FiltroVoceSmall10">A.A.:&nbsp;</asp:Label>
								</asp:TableCell>
								<asp:TableCell>
									<asp:DropDownList ID="DDLtipoCorsoDiStudi" Runat="server" AutoPostBack="True" CssClass="FiltroCampoSmall10"></asp:DropDownList>
								</asp:TableCell>
							</asp:TableRow>
						</asp:Table>			
					</asp:TableCell>
				</asp:TableRow>		
			</asp:Table>
		</asp:TableCell>
	</asp:TableRow>
</asp:Table>
<asp:table ID="TBLDdlCom" Runat="server" >
	<asp:TableRow>
		<asp:TableCell>
			<table cellpadding=0 cellspacing=0 border=0>
				<tr>
					<td valign="middle">
						<asp:Label ID="LBcomunita_t" Runat=server CssClass="FiltroVoceSmall">Community</asp:Label>&nbsp;
					</td>
					<td valign="middle">
						<asp:TextBox ID="TXBdestinatario" runat="server" ReadOnly="True" Columns="60" CssClass="FiltroCampoSmall10"></asp:TextBox>
						<img alt="" onclick="ToggleTreeView('divDestinatario')" src="./../RadControls/TreeView/Skins/Comunita/boxArrow.gif" style="margin-left: -3px;" align="middle"/>
						<div></div>
						<div id="divDestinatario" style="position:absolute; visibility: hidden; border: solid 1px; width:<%=me.RDTcomunita.Width%>px; background: white; height: 250px; zIndex=1;">
							<radTree:RadTreeView ID="RDTcomunita" runat="server" Height="250px" Width="700px" AutoPostBack="True"
								BeforeClientClick="ProcessClientClick" CssFile="~/RadControls/TreeView/Skins/Comunita/Style.css"
								ImagesBaseDir="~/RadControls/TreeView/Skins/Comunita/" skin="Comunita" CausesValidation=False/>
						</div>
					</td>
				</tr>
			</table>
		</asp:TableCell>
	</asp:TableRow>
</asp:table>