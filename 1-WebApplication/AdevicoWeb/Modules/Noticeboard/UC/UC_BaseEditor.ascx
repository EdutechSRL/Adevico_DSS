<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_BaseEditor.ascx.vb" Inherits="Comunita_OnLine.UC_BaseEditor" %>

<script type="text/javascript" language="javascript">
        function AddMessageCode(code,promptText, InsertText) {
	    var oAnteprima = document.getElementById('<%=TXBanteprima.ClientID%>')
	    if (code != "") {
		    insertCode = prompt(promptText, InsertText);
			    if ((insertCode != null) && (insertCode != "")){
				    oAnteprima.value += "<" + code + ">" + insertCode + "</"+ code + "> ";
			    }
	    }		
	    oAnteprima.focus();
	    return false;
    }
    
    function AggiornaAlign(allineamento){
	   var oAnteprima = document.getElementById('<%=TXBanteprima.ClientID%>')
	   var oImageLeft = document.getElementById('<%=IMBleft.ClientID%>')
	   var oImageCenter = document.getElementById('<%=IMBcenter.ClientID%>')
	   var oImageRight = document.getElementById('<%=IMBright.ClientID%>')
	   var oAlign = document.getElementById('<%=HDNalign.ClientID%>')
	   
	   oAnteprima.style.textAlign=allineamento
	   oAlign.value = allineamento
	   oImageLeft.src='<%=me.baseurl%>images/Editors/left.gif'
	   oImageCenter.src='<%=me.baseurl%>images/Editors/centrato.gif'
	   oImageRight.src= '<%=me.baseurl%>images/Editors/right.gif'
	   if (allineamento == "left")
		    oImageLeft.src='<%=me.baseurl%>images/Editors/left.gif'
	   else   
		  if (allineamento == "center")
			   oImageCenter.src='<%=me.baseurl%>images/Editors/centratoO.gif'
		  else
			 oImageRight.src= '<%=me.baseurl%>images/Editors/rightO.gif'
	   	
	   return false;
    }
    
    

    function AddUrl(code,promptText, InsertText) {
	    var oAnteprima = document.getElementById('<%=TXBanteprima.ClientID%>')
	    if (code != "") {
		    insertCode = prompt(promptText + "\n" + "http://www.google.it", InsertText);
			    if ((insertCode != null) && (insertCode != "")){
				    oAnteprima.value += "http://" + insertCode;
			    }
	    }		
	    oAnteprima.focus();
	    return false;
    }
    
    function AggiornaFontColor(colore){
	   var oAnteprima = document.getElementById('<%=TXBanteprima.ClientID%>')
	   oAnteprima.style.color=colore;
	   if ((colore == 'white') && oAnteprima.style.backgroundColor=='white')
		  oAnteprima.style.backgroundColor='black'
	   else 
		  if ((colore == 'black') && oAnteprima.style.backgroundColor=='black')
			 oAnteprima.style.backgroundColor='white'
	   
	   var oColore = document.getElementById('<%=HDNcolor.ClientID%>')
	   var oColoreSfondo = document.getElementById('<%=HDNbackground.ClientID%>')
	   oColore.value =  colore
	   oColoreSfondo.value = oAnteprima.style.backgroundColor

	   return false;
    }
    
     function AggiornaFontSfondo(colore){
	   var oAnteprima = document.getElementById('<%=TXBanteprima.ClientID%>')
       var oColore = document.getElementById('<%=HDNcolor.ClientID%>')
	   var oColoreSfondo = document.getElementById('<%=HDNbackground.ClientID%>')
   
	   if ((colore == 'white') && oAnteprima.style.color=='white')
		  oAnteprima.style.color='black'
	   else 
		  if ((colore == 'black') && oAnteprima.style.color=='black')
			 oAnteprima.style.color='white'
	   oAnteprima.style.backgroundColor =colore
	   oColore.value =  oAnteprima.style.color
	   oColoreSfondo.value = colore

	   return false;
    }
    
    function AggiornaFont(){
	    var size;
	    var oAnteprima = document.getElementById('<%=TXBanteprima.ClientID%>')
	    oAnteprima.style.fontFamily =document.getElementById('<%=me.DDLFont.clientID%>').item(document.getElementById('<%=me.DDLFont.clientID%>').selectedIndex).value
    	    size = document.getElementById('<%=me.DDLFontSize.clientID%>').item(document.getElementById('<%=me.DDLFontSize.clientID%>').selectedIndex).value
	    if (size=='1'){
	    //	oAnteprima.cols=180
		    oAnteprima.style.fontSize = '8pt';}
	    else if (size=='2'){
	    //	oAnteprima.cols=143
		    oAnteprima.style.fontSize = '10pt';}
	    else if (size=='3'){
	    //	oAnteprima.cols=117
		    oAnteprima.style.fontSize = '12pt';}
	    else if (size=='4'){
	    //	oAnteprima.cols=88
		    oAnteprima.style.fontSize = '14pt';}
	    else if (size=='5'){
	    //	oAnteprima.cols=68
		    oAnteprima.style.fontSize = '18pt';	}
	    else if (size=='6'){
	    //	oAnteprima.cols=50
		    oAnteprima.style.fontSize = '24pt';	}
	    else if (size=='7'){
	    //	oAnteprima.cols=33
		    oAnteprima.style.fontSize = '36pt';	
		    }							
    }
    
	   var browser='IE';
	    function addTag(tag,InsertText){
            text=getSelectedText();
            if(text!='')
                setSelectedText('<'+tag+'>'+text+'</'+tag+'>');                
            else
                getTextArea().value+='<'+tag+'> ' + InsertText + ' </'+tag+'>';
        }
        function getTextArea(){
            obj=document.getElementById('<%=TXBanteprima.ClientID%>');
            return obj;
        }
        
        function getSelectedText(){
            var browserName=navigator.appName;
            if (browserName=="Microsoft Internet Explorer"){
                st=getSelectedIE();                
                browser='IE';
            }else{
                st=getSelected(getTextArea());
                browser='Firefox';
            }            
            return st;          
        }
        
        function setSelectedText(text){
            if (browser=='IE'){
                if(text!=''){
                    document.selection.createRange().text=text;
                }
            }else{
                setSelectedTextFF(text);                
            }            
        }          
        
     function setSelectedTextFF(text){
            el=getTextArea();
            inizio=el.selectionStart;
            fine=el.selectionEnd;            
            st1=(el.value).substring(0,inizio);            
            st2=(el.value).substring(fine,(el.value).length);            
            el.value=st1+text+st2;           
        }
        
        function getSelected(el){
            text=(el.value).substring(el.selectionStart,el.selectionEnd);
            return text;
        }
        
        function getSelectedIE(){
            return  (document.all) ? document.selection.createRange().text : document.getSelection();
        }

</script>
<input type="hidden" id="HDNcolor" runat="server" />
<input type="hidden" id="HDNbackground" runat="server" />
<input type="hidden" id="HDNalign" runat="server" />

<div id="DIVeditorHTML" runat="server" align="center">
    <div class="menuEditorHtml" align="left">
	  <asp:dropdownlist runat="server" ID="DDLfont">
		  <asp:ListItem>Arial</asp:ListItem>
         <asp:ListItem>Courier New</asp:ListItem>
         <asp:ListItem>Helvetica</asp:ListItem>
         <asp:ListItem>Lucida</asp:ListItem>
         <asp:ListItem >Palatino Linotype</asp:ListItem>
         <asp:ListItem Selected="True">Verdana</asp:ListItem>
		 <asp:ListItem>Tahoma</asp:ListItem>
         <asp:ListItem>Times New Roman</asp:ListItem>
	  </asp:dropdownlist>&nbsp;
	  <asp:dropdownlist runat="server" ID="DDLFontSize">
		    <asp:ListItem Value="1" > 1(8pt)</asp:ListItem>
		    <asp:ListItem Value="2">2(10pt)</asp:ListItem>
		    <asp:ListItem Value="3">3(12pt)</asp:ListItem>
		    <asp:ListItem Value="4">4(14pt)</asp:ListItem>
		    <asp:ListItem Value="5">5(18pt)</asp:ListItem>
		    <asp:ListItem Value="6">6(24pt)</asp:ListItem>
		    <asp:ListItem Value="7">7(36pt)</asp:ListItem>
	  </asp:dropdownlist>&nbsp;
    <asp:imagebutton ID="IMBGrassetto" ImageUrl="~/images/Editors/grassetto.gif" runat="server" AlternateText="grassetto" CausesValidation="false"/><asp:imagebutton ID="IMBitalic" ImageUrl="~/images/Editors/corsivo.gif" runat="server" AlternateText="sottolineato" CausesValidation="false"/><asp:imagebutton ID="IMBsottolineato" ImageUrl="~/images/Editors/sottolineato.gif" runat="server" AlternateText="sottolineato" CausesValidation="false"/><asp:imagebutton ID="IMBleft" ImageUrl="~/images/Editors/left.gif" runat="server" AlternateText="allineamenti a sinistra"  CausesValidation="false"/><asp:imagebutton ID="IMBcenter" ImageUrl="~/images/Editors/centrato.gif" runat="server" AlternateText="allineamento al centro" CausesValidation="false"/><asp:imagebutton ID="IMBright" ImageUrl="~/images/Editors/right.gif" runat="server" AlternateText="allineamento a destra"  CausesValidation="false"/>
 <asp:Image ID="IMGcolor" runat="server"  ImageUrl="~/images/Editors/fontColor.gif" style="margin-left: 4px;"/><asp:ImageButton runat="server" ID="IMBcolorN" ImageUrl="~/images/Editors/nero.gif" CausesValidation="false" /><asp:ImageButton runat="server" ID="IMBcolorG" ImageUrl="~/images/Editors/grigio.gif" CausesValidation="false" /><asp:ImageButton runat="server" ID="IMBcolorB" ImageUrl="~/images/Editors/bianco.gif" CausesValidation="false" /><asp:ImageButton runat="server" ID="IMBcolorR" ImageUrl="~/images/Editors/rosso.gif"  CausesValidation="false"/><asp:ImageButton runat="server" ID="IMBcolorV" ImageUrl="~/images/Editors/verde.gif" CausesValidation="false"/><asp:ImageButton runat="server" ID="IMBcolorBlu" ImageUrl="~/images/Editors/blu.gif"  CausesValidation="false"/>
 <asp:Image ID="IMGsfondo" runat="server"  ImageUrl="~/images/Editors/sfondo.gif" style="margin-left: 4px;"/><asp:ImageButton runat="server" ID="IMGunderColoreB" ImageUrl="~/images/Editors/bianco.gif" CausesValidation="false"/><asp:ImageButton runat="server" ID="IMGunderColoreN" ImageUrl="~/images/Editors/nero2.gif" CausesValidation="false" />&nbsp;
</div>			 
    <div class="contenitoreEditor">
	   <asp:TextBox ID="TXBanteprima"  Rows=10 Wrap=true TextMode=MultiLine runat="server" CssClass="testoEditor" SkinID="Editor"></asp:TextBox> 
    </div>
</div>