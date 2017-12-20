var info = false;
var finestra;
var elencoDestinatari;
var noSMS = false;
elencoDestinatari = new Array()
elencoCampi = new Array()
CampiRubrica = new Array()


function LimitTextWithNoCrLf(event, fieldObj, maxChars) {
    var numCharCode;
    var elTarget;
    var strType;

    if (!e) var e = event;
		
    if (e.keyCode) numCharCode = e.keyCode;
    else if (e.which) numCharCode = e.which;

    if (e.target) elTarget = e.target;
    else if (e.srcElement) elTarget = e.srcElement;

    if ( numCharCode == 13 ){
        return false;
      }
           

    var retval=true;
    if (fieldObj.value.length >= maxChars){
	    fieldObj.focus();
        	alert ("Numero di caratteri eccessivo!!!");
		fieldObj.value=fieldObj.value.substring(0,maxChars)
		retval=true;
	    }
    return retval;
    }

function LimitText(fieldObj,maxChars) //per validare la lunghezza massima della texarea indicata
{
  var retval=true;
  if (fieldObj.value.length >= maxChars)
	{
	    fieldObj.focus();
        	alert ("Numero di caratteri eccessivo!!!");
		fieldObj.value=fieldObj.value.substring(0,maxChars)
		retval=true;
	}
return retval;
}

function OpenWin(link,oWidth,oHeight,oResize,oScroll){
	info = true;
	finestra = window.open(link,'Win','menubar=no,resizable='+oResize+',location=no,toolbar=no,scrollbars='+oScroll+',width='+oWidth+',height='+oHeight);
	finestra.focus();
}

	
function CloseWin(){
	if (info && finestra ) finestra.close();
}

function OpenWindow(name,link,oWidth,oHeight,oResize,oScroll,menubar){
	info = true;
	finestra = window.open(link,name,'menubar='+menubar+',resizable='+oResize+',location=no,toolbar=no,scrollbars='+oScroll+',width='+oWidth+',height='+oHeight);
	finestra.focus();
}

function SetValueAndSubmitWithAzione(oo,value,objAzione,Azione){
		var TButente,TBazione
		eval('TButente= this.document.forms[0].' + oo);
		eval('TBazione= this.document.forms[0].' + objAzione);
		TButente.value = value;
		TBazione.value = Azione;
		this.document.forms[0].submit();
	}

function SetValueAndSubmitWithAzioneWithConfirm(oo,value,objAzione,Azione,textConfirm){
		var TButente,TBazione
		
		if (confirm(textConfirm)==true){
			eval('TButente= this.document.forms[0].' + oo);
			eval('TBazione= this.document.forms[0].' + objAzione);
			TButente.value = value;
			TBazione.value = Azione;
			this.document.forms[0].submit();
			}
	}	
	
function SelectMeAndValida(Me,value,objToValidate,Validator){
		var HIDcheckbox,objValidator;
	//	var objToValidate;
		eval('HIDcheckbox= this.document.forms[0].' + elencoCampi[9])
		eval('objValidator= this.document.forms[0].' + Validator)
		
   		for(i=0;i< document.forms[0].length; i++){ 
			e=document.forms[0].elements[i];
						
			if ( e.type=='hidden' && e.name.indexOf("HDNvalidate") != -1 ){
				//controllo da validare
				
			}
			if (Me ==e) {
				if ( e.type=='checkbox' && e.name.indexOf("CBXassocia") != -1 ){
					if (e.checked==true){
						if (HIDcheckbox.value == "")
							HIDcheckbox.value = ',' + value +','
						else{
							pos1 = HIDcheckbox.value.indexOf(',' + value+',')
							if (pos1==-1)
							HIDcheckbox.value = HIDcheckbox.value + value +','
							}
						if (objValidator.value == "")
							objValidator.value = ',' + objToValidate +','
						else{
							pos1 = objValidator.value.indexOf(',' + objToValidate+',')
							if (pos1==-1)
								objValidator.value = objValidator.value + objToValidate +','
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
						valore = objValidator.value;
						pos1 = objValidator.value.indexOf(',' + objToValidate +',')
						if (pos1!=-1){
							stringa = ',' + objToValidate+','
							objValidator.value = objValidator.value.substring(0,pos1)
							objValidator.value = objValidator.value + valore.substring(pos1+value.length+1,valore.length)
							}
						}
					}
				}
		}

		if (HIDcheckbox.value==",")
			HIDcheckbox.value = "";
		if (objValidator.value==",")
			objValidator.value = "";

	}
	
function SelectMeAssocia(Me,value){
		var HIDcheckbox;
		eval('HIDcheckbox= this.document.forms[0].' + elencoCampi[9])

   		for(i=0;i< document.forms[0].length; i++){ 
			e=document.forms[0].elements[i];
			
			if ( e.type=='checkbox' && e.name.indexOf("CBXassocia") != -1 ){//
				if (e == Me){
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
		}
		if (HIDcheckbox.value==",")
			HIDcheckbox.value = "";
	}
 
 function SelectFromNameAndAssocia(Nome,value){
		var HIDcheckbox;
		eval('HIDcheckbox= this.document.forms[0].' + elencoCampi[9])

   		for(i=0;i< document.forms[0].length; i++){ 
			e=document.forms[0].elements[i];
			
			if ( e.type=='checkbox' && e.name ==Nome) {//"CBXassocia"

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
	
	function SelectFromNameAndAssociaIntoControl(Nome,value,controllo){
		var HIDcheckbox;
		eval('HIDcheckbox= this.document.forms[0].' + controllo)
   		for(i=0;i< document.forms[0].length; i++){ 
			e=document.forms[0].elements[i];
			
			if ( e.type=='checkbox' && e.name ==Nome) {//"CBXassocia"
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
 function ClickAndEnable_Disable(objToDisable,value){
		var oDisable;
		objToDisable.disabled = !(objToDisable.disabled)
		objToDisable.value = value
}

 function TOOLTIP() {
//----------------------------------------------------------------------------------------------------
// Configuration
//----------------------------------------------------------------------------------------------------
    this.width = 150;                     // width (pixels)
    this.bgColor = '#FFFFC0';             // background color
    this.textColor = '#00008b';           // text color
    this.borderColor = '#00008b';         // border color
    this.opacity = 100;                    // opacity (percent) - doesn't work with all browsers
    this.cursorDistance = 5;              // distance from cursor (pixels)

    // don't change
    this.text = '';
    this.obj = 0;
    this.sobj = 0;
    this.active = false;

// -------------------------------------------------------------------------------------------------------
// Functions
// -------------------------------------------------------------------------------------------------------
    this.create = function() {
      if(!this.sobj) this.init();

      var t = '<table border=0 cellspacing=0 cellpadding=4 width=' + this.width + ' bgcolor=' + this.bgColor + '><tr>' +
              '<td style="BORDER-BOTTOM: navy 2px solid" bgcolor=#191970 align=center><font color=' + this.textColor + '>' + this.text + '</font></td></tr></table>';

      if(document.layers) {
        t = '<table border=0 cellspacing=0 cellpadding=1><tr><td bgcolor=' + this.borderColor + '>' + t + '</td></tr></table>';
        this.sobj.document.write(t);
        this.sobj.document.close();
      }
      else {
        this.sobj.border = '1px solid ' + this.borderColor;
        this.setOpacity();
        if(document.getElementById) document.getElementById('ToolTip').innerHTML = t;
        else document.all.ToolTip.innerHTML = t;
      }
      this.show();
    }

    this.init = function() {
      if(document.getElementById) {
        this.obj = document.getElementById('ToolTip');
        this.sobj = this.obj.style;
      }
      else if(document.all) {
        this.obj = document.all.ToolTip;
        this.sobj = this.obj.style;
      }
      else if(document.layers) {
        this.obj = document.ToolTip;
        this.sobj = this.obj;
      }
    }

    this.show = function() {
      var ext = (document.layers ? '' : 'px');
      var left = mouseX;

      if(left + this.width + this.cursorDistance > winX) left -= this.width + this.cursorDistance;
      else left += this.cursorDistance;

      this.sobj.left = left + ext;
      this.sobj.top = mouseY + this.cursorDistance + ext;

      if(!this.active) {
        this.sobj.visibility = 'visible';
        this.active = true;
      }
    }

    this.hide = function() {
      if(this.sobj) this.sobj.visibility = 'hidden';
      this.active = false;
    }

    this.setOpacity = function() {
      this.sobj.filter = 'alpha(opacity=' + this.opacity + ')';
      this.sobj.mozOpacity = '.1';
      if(this.obj.filters) this.obj.filters.alpha.opacity = this.opacity;
      if(!document.all && this.sobj.setProperty) this.sobj.setProperty('-moz-opacity', this.opacity / 100, '');
    }
  }

//----------------------------------------------------------------------------------------------------
// Build layer, get mouse coordinates and window width, create tooltip-object
//----------------------------------------------------------------------------------------------------
  var tooltip = mouseX = mouseY = winX = 0;

  if(document.layers) {
    document.write('<layer id="ToolTip"></layer>');
    document.captureEvents(Event.MOUSEMOVE);
  }
  else document.write('<div id="ToolTip" style="position:absolute; z-index:99"></div>');
  document.onmousemove = getMouseXY;

  function getMouseXY(e) {
    try {
      if(document.all) {
          mouseX = event.clientX + document.body.scrollLeft;
          mouseY = event.clientY + document.body.scrollTop;
        }
      else {
        mouseX = e.pageX;
        mouseY = e.pageY;
        }
        }
        catch (exception) {
     }
  
    if(mouseX < 0) mouseX = 0;
    if(mouseY < 0) mouseY = 0;

    if(document.body && document.body.offsetWidth) winX = document.body.offsetWidth - 25;
    else if(window.innerWidth) winX = window.innerWidth - 25;
    else winX = screen.width - 25;

    if(tooltip && tooltip.active) tooltip.show();
  }

  function toolTip(text, width, opacity) {
    if(text) {
      tooltip = new TOOLTIP();
      tooltip.text = text;
      if(width) tooltip.width = width;
      if(opacity) tooltip.opacity = opacity;
      tooltip.create();
    }
    else if(tooltip) tooltip.hide();
}
  
  
  
  
  
  
     function NoClick(event, accetta, oggetto) {
        if (accetta == true) {
            if (document.all) {
                if (event.keyCode == 13) {
                    oggetto.value = oggetto.value + "\r\n"

                    event.returnValue = false;
                    event.cancel = true;
                    return false;
                }
            }
            else if (document.getElementById) {
                if (event.which == 13) {
                    oggetto.value = oggetto.value + "\r\n"

                    event.returnValue = false;
                    event.cancel = true;
                    return false;
                }
            }
            else if (document.layers) {
                if (event.which == 13) {
                    oggetto.value = oggetto.value + "\r\n"

                    event.returnValue = false;
                    event.cancel = true;
                    return false;
                }
            }
            return true
        }

        if (document.all) {
            if (event.keyCode == 13) {
                event.returnValue = false;
                event.cancel = true;
                return false;
            }
        }
        else if (document.getElementById) {
            if (event.which == 13) {
                event.returnValue = false;
                event.cancel = true;
                return false;
            }
        }
        else if (document.layers) {
            if (event.which == 13) {
                event.returnValue = false;
                event.cancel = true;
                return false;
            }
        }
        else
            return true;
    }
