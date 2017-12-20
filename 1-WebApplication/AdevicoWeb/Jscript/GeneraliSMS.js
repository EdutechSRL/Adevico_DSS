var info = false;
var finestra;
var elencoDestinatari;
var noSMS = false;
elencoDestinatari = new Array()
elencoCampi = new Array()
CampiRubrica = new Array()
			
function OpenWin(link,oWidth,oHeight,oResize,oScroll){
	info = true;
	finestra = window.open(link,'Win','menubar=no,resizable='+oResize+',location=no,toolbar=no,scrollbars='+oScroll+',width='+oWidth+',height='+oHeight);
	finestra.focus();
}
	
function CloseWin(){
	if (info && finestra ) finestra.close();
}

/*
elencoCampi[0]='" & Me.TBmittente.UniqueID
elencoCampi[1]='" & Me.TBmessaggio.UniqueID 
elencoCampi[2]='" & Me.TBdestinatari.UniqueID 
elencoCampi[3]='" & Me.TBrlpc_elenco.UniqueID 
elencoCampi[4]='" & Me.TBsmsdisponibili_n.UniqueID 
elencoCampi[5]='" & Me.BTsendSMS.UniqueID 
elencoCampi[6]='" & Me.TBmaxMitt.UniqueID 
elencoCampi[7]='" & Me.TBmaxMSG.UniqueID 
elencoCampi[8]='" & Me.TBcellulare.UniqueID 

*/
	function getTBmittente(){
		return elencoCampi[0]
	}
	function getTBdestinatari(){
		return elencoCampi[2]
	}
	
	function getTBrlpc_elenco(){
		return elencoCampi[3]
	}
	
	function getTBcellulare(){
		return elencoCampi[8]
	}
	
	function setdestinatariFromRubrica(elencoDest){
		var max,TotDestinatari,TotDisponibili,TotRimanenti;
		elencoDestinatari = elencoDest
		
		var TBdestinatari,BTsendSMS,TBrlpc_elenco,TBsmsdisponibili_n,TBmittente,TBmessaggio
		eval('TBdestinatari= this.document.forms[0].' + elencoCampi[2])
		eval('BTsendSMS= this.document.forms[0].' + elencoCampi[5])
		eval('HDNtotaleDestinatari= this.document.forms[0].' + elencoCampi[3])
		eval('TBsmsdisponibili_n= this.document.forms[0].' + elencoCampi[4])
		eval('TBmittente= this.document.forms[0].' + elencoCampi[0])
		eval('TBmessaggio= this.document.forms[0].' + elencoCampi[1])
		
		max = TBmessaggio.value.length + TBmittente.value.length
		TotDestinatari = HDNtotaleDestinatari.value
		TotDisponibili = TBsmsdisponibili_n.value
					
		if (noSMS == false){
			if (TotDisponibili==0)
				alert('Non ci sono SMS disponibili !!');
			else{
				TotRimanenti = TotDestinatari - TotDisponibili
				if (TotRimanenti == 0)
					alert('Non ci sono SMS disponibili !!');
				if (TotRimanenti == 1)
					alert('Non ci sono SMS disponibili per inviare SMS a tutti i destinatari (uno deve essere eliminato dall\'elenco)');
				if (TotRimanenti > 1)
					alert('Non ci sono SMS disponibili per inviare SMS a tutti i destinatari ( '+TotRimanenti+' devono essere eliminati dall\'elenco)');
				}
		}
		
		if (max==0 || TBdestinatari.value.length==0 || TotDisponibili == 0 || TotDisponibili<TotDestinatari){
			BTsendSMS.disabled = true
			noSMS = true;}
		else{
			BTsendSMS.disabled = false
			noSMS = false;}
	}
			
	function setdestinatariToRubrica(){
		return elencoDestinatari;
	}
	
	function CheckAndSetValue(obj,objText,valueCheck,valueNoCheck){
		var TButente
		eval('TButente= this.document.forms[0].' + objText)
		if (obj.checked){
			TButente.value = valueCheck
			TButente.disabled = true
			}
		else
			{
			TButente.value=valueNoCheck
			TButente.disabled = false
			}
	}
	
	function SetValueAndSubmitWithAzione(oo,value,objAzione,Azione){
		var TButente,TBazione
		eval('TButente= this.document.forms[0].' + oo);
		eval('TBazione= this.document.forms[0].' + objAzione);
		TButente.value = value;
		TBazione.value = Azione;
		this.document.forms[0].submit();
	}
	
	function SetValueAndSubmit(oo,value){
		var TButente
		eval('TButente= this.document.forms[0].' + oo)
	
		TButente.value = value;
		this.document.forms[0].submit();
	}
	
	function calcola() {
		var TBmittente,TBmaxMitt,TBmessaggio,TBmaxMSG,BTsendSMS,TBsmsdisponibili_n
		eval('TBmittente= this.document.forms[0].' + elencoCampi[0])
		eval('TBmessaggio= this.document.forms[0].' + elencoCampi[1])
		eval('TBdestinatari= this.document.forms[0].' + elencoCampi[2])
		eval('TBmaxMitt= this.document.forms[0].' + elencoCampi[6])
		eval('TBmaxMSG= this.document.forms[0].' + elencoCampi[7])
		eval('BTsendSMS= this.document.forms[0].' + elencoCampi[5])
		
		eval('TBsmsdisponibili_n= this.document.forms[0].' + elencoCampi[4])
		eval('HDNtotaleDestinatari= this.document.forms[0].' + elencoCampi[3])
		
		var max_mitt,max;
		if (TBmittente){
			max_mitt = 18
			if (TBmittente.value.length > max_mitt) {
				TBmittente.value = TBmittente.value.substring(0,max_mitt);
				rest = 0;
			} else {
				rest = max_mitt - TBmittente.value.length;
				}
			TBmaxMitt.value = rest				
		}
		if (TBmessaggio){
			max_mitt = 140
			if (TBmessaggio.value.length > max_mitt) {
				TBmessaggio.value = TBmessaggio.value.substring(0,max_mitt);
				rest = 0;
			} else {
				rest = max_mitt - TBmessaggio.value.length;
				}
			TBmaxMSG.value = rest
		}
		max = TBmessaggio.value.length+TBmittente.value.length
		
		if (max==0 || TBmessaggio.value.length==0 || TBdestinatari.value.length==0 || TBsmsdisponibili_n.value == 0 || Number(TBsmsdisponibili_n.value)<Number(HDNtotaleDestinatari.value)){
			BTsendSMS.disabled = true
			noSMS = true;}
		else{
			BTsendSMS.disabled = false
			noSMS = false;}
	}
	
	function SelectAll( SelectAllBox ){
		var actVar = SelectAllBox.checked ;
		var TBcheckbox;
		eval('HIDcheckbox= this.document.forms[0].' + elencoCampi[9])
		HIDcheckbox.value = ""
		for(i=0;i< document.forms[0].length; i++){ 
			e=document.forms[0].elements[i];
			if ( e.type=='checkbox' && e.name.indexOf("CBselect") != -1 ){
				e.checked= actVar ;
				if (e.checked==true)
					if (HIDcheckbox.value == "")
						HIDcheckbox.value = ',' + e.value+','
					else
						HIDcheckbox.value = HIDcheckbox.value + e.value +','
				}
		}
	}
	
	function checkTipoRuolo(checkboxTipoRuolo){
	//	var checkboxTipoRuolo;
	//	eval('checkboxTipoRuolo= this.document.forms[0].' + elencoCampi[9])
		for(i=0;i< checkboxTipoRuolo.length; i++){ 
			alert(checkboxTipoRuolo.elements[i]);
		}		
	}
	
	function SelectAllNoDisabled( SelectAllBox ){
		var actVar = SelectAllBox.checked ;
		var TBcheckbox;
		eval('HIDcheckbox= this.document.forms[0].' + elencoCampi[9])
		HIDcheckbox.value = ""
		for(i=0;i< document.forms[0].length; i++){ 
			e=document.forms[0].elements[i];
			if ( e.type=='checkbox' && e.name.indexOf("CBselect") != -1 && e.disabled == false ){
				e.checked= actVar ;
				if (e.checked==true)
					if (HIDcheckbox.value == "")
						HIDcheckbox.value = ',' + e.value+','
					else
						HIDcheckbox.value = HIDcheckbox.value + e.value +','
				}
		}
	}
	function SelectMeNoDisabled(Me){
		var HIDcheckbox;
		eval('HIDcheckbox= this.document.forms[0].' + elencoCampi[9])

   		for(i=0;i< document.forms[0].length; i++){ 
			e=document.forms[0].elements[i];
			if ( e.type=='checkbox' && e.name.indexOf("CBselect") != -1 && e.disabled == false){
				if (e.checked==true){
					if (HIDcheckbox.value == "")
						HIDcheckbox.value = ',' + e.value+','
					else{
						pos1 = HIDcheckbox.value.indexOf(',' + e.value+',')
						if (pos1==-1)
						HIDcheckbox.value = HIDcheckbox.value + e.value +','
						}
					}
				else{
					valore = HIDcheckbox.value;
					pos1 = HIDcheckbox.value.indexOf(',' + e.value+',')
					if (pos1!=-1){
						stringa = ',' + e.value+','
						HIDcheckbox.value = HIDcheckbox.value.substring(0,pos1)
						HIDcheckbox.value = HIDcheckbox.value + valore.substring(pos1+e.value.length+1,valore.length)
						}
					}
				}
		}
		if (HIDcheckbox.value==",")
			HIDcheckbox.value = "";
	}


	
	function SelectMe(Me){
		var HIDcheckbox;
		eval('HIDcheckbox= this.document.forms[0].' + elencoCampi[9])

   		for(i=0;i< document.forms[0].length; i++){ 
			e=document.forms[0].elements[i];
			if ( e.type=='checkbox' && e.name.indexOf("CBselect") != -1 ){
				if (e.checked==true){
					if (HIDcheckbox.value == "")
						HIDcheckbox.value = ',' + e.value+','
					else{
						pos1 = HIDcheckbox.value.indexOf(',' + e.value+',')
						if (pos1==-1)
						HIDcheckbox.value = HIDcheckbox.value + e.value +','
						}
					}
				else{
					valore = HIDcheckbox.value;
					pos1 = HIDcheckbox.value.indexOf(',' + e.value+',')
					if (pos1!=-1){
						stringa = ',' + e.value+','
						HIDcheckbox.value = HIDcheckbox.value.substring(0,pos1)
						HIDcheckbox.value = HIDcheckbox.value + valore.substring(pos1+e.value.length+1,valore.length)
						}
					}
				}
		}
		if (HIDcheckbox.value==",")
			HIDcheckbox.value = "";
	}

	function SelectMeAssocia(Me,value){
		var HIDcheckbox;
		eval('HIDcheckbox= this.document.forms[0].' + elencoCampi[9])

   		for(i=0;i< document.forms[0].length; i++){ 
			e=document.forms[0].elements[i];
			
			if ( e.type=='checkbox' && e.name.indexOf("CBXassocia") != -1 ){
				if (e.checked==true){
					if (HIDcheckbox.value == "")
						HIDcheckbox.value = ',' + value +','
					else{
						pos1 = HIDcheckbox.value.indexOf(',' + value+',')
						if (pos1==-1)
						HIDcheckbox.value = HIDcheckbox.value + value +','
						}
					}
				else{
					valore = HIDcheckbox.value;
					pos1 = HIDcheckbox.value.indexOf(',' + value+',')
					if (pos1!=-1){
						stringa = ',' + value+','
						HIDcheckbox.value = HIDcheckbox.value.substring(0,pos1)
						HIDcheckbox.value = HIDcheckbox.value + valore.substring(pos1+value.length+1,valore.length)
						}
					}
				}
		}

		if (HIDcheckbox.value==",")
			HIDcheckbox.value = "";
	}
 

	
	var checkSelezionate
	checkSelezionate = new Array()
	
	function reloadCheck(){
		for(i=0;i< document.forms[0].length; i++){ 
			e=document.forms[0].elements[i];
			if ( checkSelezionate[i] != "" )
				checkSelezionate[i].checked = true ;
		}
	}