function GetRadUpload(I8){return window[I8]; } ; if (typeof(RadUploadNameSpace)=="undefi\x6e\x65d")RadUploadNameSpace= {} ; RadUploadNameSpace.RadUpload= function (J){ this.l9= false; RadControlsNamespace.EventMixin.Initialize(this ); RadControlsNamespace.DomEventsMixin.Initialize(this ); this.Id=J[0]; this.i9(document.getElementById(J[1])); this.I9=J[2]; this.oa=J[3]; this.Enabled=J[4]; this.Oa=J[5]; this.la=J[6]; this.ia=J[7]; if (RadControlsNamespace.Browser.IsSafari || (RadControlsNamespace.Browser.IsOpera && !RadControlsNamespace.Browser.IsOpera9)){ this.ia= false; } this.Ia=J[8]; this.ob=J[9]; this.Ob=J[10]&1; this.lb=J[10]&2; this.ib=J[10]&4; this.OnClientAdded=J[11]; this.OnClientAdding=J[12]; this.OnClientDeleting=J[13]; this.OnClientClearing=J[14]; this.OnClientFileSelected=J[15]; this.OnClientDeletingSelected=J[16]; this.CurrentIndex=0; this.Ib=document.getElementById(this.Id+"B\x75tto\x6e\x41rea"); this.ListContainer=document.getElementById(this.Id+"ListContai\x6e\x65r"); if (!document.readyState || document.readyState=="complete"){ this.InnerConstructor(); }else { this.AttachDomEvent(window,"load","InnerC\x6f\x6estruc\x74\x6fr"); }} ; RadUploadNameSpace.RadUpload.prototype= {InnerConstructor:function (){ this.oc(); this.Oc=this.lc(document.getElementById(this.Id+"\x41ddBut\x74\x6fn"),"Add","\x41ddFileInput"); this.ic=this.lc(document.getElementById(this.Id+"\x44eleteBu\x74\x74on"),"\x44elete","\x44eleteSelect\x65\x64Fil\x65\x49\x6e\160\x75\164s"); this.Ic=this.od(); var Od=this.Oa==0?this.la:Math.min(this.la,this.Oa); for (var i=0; i<Od; i++){ this.AddFileInput(); } this.ld(); this.l9= true; } ,AddFileInput:function (){var l3=this.AddFileInputAt(this.ListContainer.rows.length); if (this.l9){try {l3.focus(); }catch (ex){}}} ,AddFileInputAt:function (index){if (typeof(index)=="\x75ndefined" || index>this.ListContainer.rows.length){index=this.ListContainer.rows.length; }if (this.Oa>0 && index>=this.Oa)return; if (this.l9){var oe=this.RaiseEvent("\x4fnClientAddin\x67",new RadUploadNameSpace.RadUploadEventArgs(null)); if (oe== false){return; }} this.Oe(index); } ,Oe:function (index){var le=this.ListContainer.insertRow(index); this.AttachDomEvent(le,"cli\x63\x6b","RowClick\x65\x64"); var ie; if (this.Ob){ie=le.insertCell(le.cells.length); this.Ie(ie); }ie=le.insertCell(le.cells.length); this.of(ie); if (this.ib){ie=le.insertCell(le.cells.length); this.Of(ie); }if (this.lb){ie=le.insertCell(le.cells.length); this.If(ie); } this.ld(); this.RaiseEvent("\x4fnClientA\x64\x64ed", {Row:le } ); this.CurrentIndex++; return le; } ,Ie:function (container){var og=document.createElement("input"); og.type="check\x62\x6fx"; og.id=this.Id+"\x63heckbox"+this.CurrentIndex; container.appendChild(og); og.className="RadUplo\x61\x64File\x53\x65lect\x6fr"; og.disabled=!this.Enabled; return og; } ,Of:function (container){var button=document.createElement("\x69nput"); button.type="b\x75\x74ton"; button.id=this.Id+"\x63lear"+this.CurrentIndex; container.appendChild(button); this.lc(button,"\x43\x6cear"); button.className="\x52adUploadCl\x65\x61rBu\x74\x74on"; button.name="ClearIn\x70\x75t"; button.disabled=!this.Enabled; return button; } ,If:function (container){var button=document.createElement("\x69nput"); button.type="\x62utton"; button.id=this.Id+"remov\x65"+this.CurrentIndex; container.appendChild(button); button.value=RadUploadNameSpace.Localization[this.I9]["\x52emove"]; button.className="RadUploa\x64\x52emov\x65\x42utt\x6f\156"; button.name="RemoveRo\x77"; button.disabled=!this.Enabled; return button; } ,of:function (container){var l3=this.Og(); this.AttachDomEvent(l3,"change","\x46\x69leSele\x63\x74ed"); if (this.ia){l3.className="RealFileI\x6e\x70ut"; var div=document.createElement("di\x76"); container.appendChild(div); div.style.position="r\x65\x6cative"; div.appendChild(this.Ic.cloneNode( true)); div.appendChild(l3); if (!this.Ia){ this.AttachDomEvent(l3,"keyu\x70","\x53yncFile\x49\x6eputCo\x6e\x74en\x74"); }else { this.AttachDomEvent(l3,"\x6beydown","CancelEve\x6e\x74"); }return div; }else {container.appendChild(l3); l3.className="NoSkinned\x46\x69leUn\x70\x75t"; if (this.Ia){ this.AttachDomEvent(l3,"\x6beydown","Cancel\x45\x76ent"); }return l3; }} ,CancelEvent:function (n){if (!n)n=window.event; if (!n)return false; n.returnValue= false; n.cancelBubble= true; if (n.stopPropagation){n.stopPropagation(); }if (n.preventDefault){n.preventDefault(); }return false; } ,ClearFileInputAt:function (index){var le=this.ListContainer.rows[index]; if (le){var oe=this.RaiseEvent("\x4fnClie\x6e\x74Clear\x69\x6eg",new RadUploadNameSpace.RadUploadEventArgs(this.GetFileInputFrom(le))); if (oe== false){return false; } this.DeleteFileInputAt(index, true); this.Oe(index, true); }} ,od:function (){var lg=document.createElement("d\x69v"); lg.style.position="\x61bsolute"; lg.style.top=0; lg.style.left=0; lg.style.zIndex=1; var Ig=document.createElement("input"); Ig.type="text"; Ig.className="\122\x61\144Upl\x6f\x61dInp\x75\x74Fi\x65\x6cd"; lg.appendChild(Ig); var oh=document.createElement("\x69nput"); oh.type="\x62\165tto\x6e"; lg.appendChild(oh); this.lc(oh,"Select"); oh.disabled=!this.Enabled; oh.className="RadU\x70\x6coadSel\x65\x63tBu\x74\x74on"; return lg; } ,Og:function (){var l3=document.createElement("\151n\x70\x75t"); l3.type="fil\x65"; l3.name=this.GetID("\x66ile"); l3.id=this.GetID("\x66ile"); l3.disabled=!this.Enabled; return l3; } ,DeleteFileInputAt:function (index,Oh){var le=this.ListContainer.rows[index]; if (le){if (!Oh){var oe=this.RaiseEvent("\x4fnC\x6c\x69entDel\x65\x74ing",new RadUploadNameSpace.RadUploadEventArgs(this.GetFileInputFrom(le))); if (oe== false){return false; }}le.parentNode.removeChild(le); this.ld(); }} ,DeleteSelectedFileInputs:function (){var lh=[]; var ih=[]; for (var i=this.ListContainer.rows.length-1; i>=0; i--){var Ih=this.ListContainer.rows[i]; var oi=Ih.cells[0].childNodes[0]; if (oi.checked){ih[ih.length]=i; lh[lh.length]=this.GetFileInputFrom(Ih); }}var oe=this.RaiseEvent("\x4fnClientDel\x65\x74ingS\x65\x6cec\x74\x65d",new RadUploadNameSpace.RadUploadDeleteSelectedEventArgs(lh)); if (oe== false){return; }for (var i=0; i<ih.length; i++){ this.DeleteFileInputAt(ih[i], true); }} ,oc:function (){var Oi=this.ListContainer.rows[0]; Oi.parentNode.removeChild(Oi); } ,FileSelected:function (e){if (this.ia){ this.SyncFileInputContent(e); } this.RaiseEvent("\117nClientFil\x65\123e\x6c\x65ct\x65\144",new RadUploadNameSpace.RadUploadEventArgs(e.srcElement?e.srcElement:e.target)); } ,GetFileInputFrom:function (le){var ii=le.getElementsByTagName("\x69\x6eput"); for (var i=0; i<ii.length; i++){if (ii[i].type=="\x66ile"){return ii[i]; }}return null; } ,GetFileInputs:function (){var O3=[]; for (var i=0; i<this.ListContainer.rows.length; i++){O3[O3.length]=this.GetFileInputFrom(this.ListContainer.rows[i]); }return O3; } ,GetID:function (F){return this.Id+F+this.CurrentIndex; } ,Ii:function (K){if (K){if (K.tagName.toLowerCase()=="t\x72"){return K; }else {return this.Ii(K.parentNode); }}return null; } ,lc:function (button,oj,Oj){if (button){button.value=RadUploadNameSpace.Localization[this.I9][oj]; if (this.Enabled){if (Oj)this.AttachDomEvent(button,"\x63lick",Oj); }else {button.disabled= true; }}return button; } ,IsExtensionValid:function (lj){if (lj=="")return true; for (var i=0; i<this.ob.length; i++){var ij=this.ob[i].substring(2); var Ij=new RegExp("\x2e"+ij+"\x24","i\x67"); if (lj.match(Ij)){return true; }}return false; } ,RowClicked:function (e){var srcElement=e.srcElement?e.srcElement:e.target; var ok=this.Ii(srcElement); if (srcElement.name=="\x52emo\x76\x65Row"){ this.DeleteFileInputAt(ok.rowIndex); }else if (srcElement.name=="ClearInput"){ this.ClearFileInputAt(ok.rowIndex); }} ,ld:function (){ this.Ok(this.ic,this.ListContainer.rows.length>0); this.Ok(this.Oc,(this.Oa<=0) || (this.ListContainer.rows.length<this.Oa)); } ,Ok:function (button,lk){if (button){button.className=lk?"R\x61\144U\x70\x6coadB\x75\x74to\x6e": "RadUploadB\x75\x74tonD\x69\x73abl\x65\144"; }} ,SyncFileInputContent:function (e){var l3=e.srcElement?e.srcElement:e.target; var ik=l3.parentNode.childNodes[0].childNodes[0]; if (l3 !== ik){ik.value=l3.value; ik.title=l3.value; ik.disabled= true; }} ,i9:function (form){if (!form)form=document.forms[0]; form.enctype=form.encoding="multipart/\x66orm-data"; } ,ValidateExtensions:function (){for (var i=0; i<this.ListContainer.rows.length; i++){var Ik=this.GetFileInputFrom(this.ListContainer.rows[i]).value; if (!this.IsExtensionValid(Ik)){return false; }}return true; }} ;;if (typeof window.RadControlsNamespace=="und\x65\x66ined"){window.RadControlsNamespace= {} ; }if (typeof(window.RadControlsNamespace.Browser)=="undefined" || typeof(window.RadControlsNamespace.Browser.Version)==null || window.RadControlsNamespace.Browser.Version<1){window.RadControlsNamespace.Browser= {Version: 1 } ; window.RadControlsNamespace.Browser.ParseBrowserInfo= function (){ this.IsMacIE=(navigator.appName=="\x4dic\x72\x6fsoft I\x6e\x74ern\x65t Explo\x72\145\x72") && ((navigator.userAgent.toLowerCase().indexOf("mac")!=-1) || (navigator.appVersion.toLowerCase().indexOf("\x6dac")!=-1)); this.IsSafari=(navigator.userAgent.toLowerCase().indexOf("\x73afari")!=-1); this.IsMozilla=window.netscape && !window.opera; this.IsNetscape=/\x4e\x65\x74\x73\x63\x61\x70\x65/.test(navigator.userAgent); this.IsOpera=window.opera; this.IsOpera9=window.opera && (parseInt(window.opera.version())>8); this.IsIE=!this.IsMacIE && !this.IsMozilla && !this.IsOpera && !this.IsSafari; this.O=/\x4d\x53\x49\x45\x20\x37/.test(navigator.appVersion); this.StandardsMode=this.IsSafari || this.IsOpera9 || this.IsMozilla || document.compatMode=="CSS\x31\x43ompat"; this.IsMac=/\x4d\x61\x63/.test(navigator.userAgent); };RadControlsNamespace.Browser.ParseBrowserInfo(); };if (typeof window.RadControlsNamespace=="\x75\x6edefined"){window.RadControlsNamespace= {} ; }RadControlsNamespace.DomEventsMixin= function (){} ; RadControlsNamespace.DomEventsMixin.Initialize= function (o){o.AttachDomEvent=this.AttachDomEvent; o.DetachDomEvent=this.DetachDomEvent; o.DisposeDomEvents=this.DisposeDomEvents; o.ClearEventPointers=this.ClearEventPointers; o.RegisterForAutomaticDisposal=this.RegisterForAutomaticDisposal; o.I=this.I; o.CreateEventHandler=this.CreateEventHandler; o.A=this.A; o.ClearEventPointers(); } ; RadControlsNamespace.DomEventsMixin.CreateEventHandler= function (U){var Z=this ; return function (e){if (!e)e=window.event; return Z[U](e); };} ; RadControlsNamespace.DomEventsMixin.AttachDomEvent= function (z,W,w){var V=this.CreateEventHandler(w); this.v[this.v.length]=[z,W,V]; this.A(z,W,V); } ; RadControlsNamespace.DomEventsMixin.A= function (z,W,V){if (z.attachEvent){z.attachEvent("\157\x6e"+W,V); }else if (z.addEventListener){z.addEventListener(W,V, false); }} ; RadControlsNamespace.DomEventsMixin.DetachDomEvent= function (z,W,V){if (z.detachEvent){z.detachEvent("on"+W,V); }} ; RadControlsNamespace.DomEventsMixin.DisposeDomEvents= function (){for (var i=0; i<this.v.length; i++){ this.DetachDomEvent(this.v[i][0],this.v[i][1],this.v[i][2]); } this.ClearEventPointers(); } ; RadControlsNamespace.DomEventsMixin.RegisterForAutomaticDisposal= function (T){var t=this ; var S=this.CreateEventHandler(T); var R= function (){S(); t.DisposeDomEvents(); t=null; } ; this.A(window,"\165\x6eload",R); } ; RadControlsNamespace.DomEventsMixin.ClearEventPointers= function (){ this.v=[]; } ;;if (typeof window.RadControlsNamespace=="unde\x66\x69ned"){window.RadControlsNamespace= {} ; }if (typeof(window.RadControlsNamespace.EventMixin)=="\x75ndefin\x65\x64" || typeof(window.RadControlsNamespace.EventMixin.Version)==null || window.RadControlsNamespace.EventMixin.Version<1){RadControlsNamespace.EventMixin= {Version: 1,Initialize:function (o){o._listeners= {} ; o._eventsEnabled= true; o.AttachEvent=this.AttachEvent; o.DetachEvent=this.DetachEvent; o.RaiseEvent=this.RaiseEvent; o.EnableEvents=this.EnableEvents; o.DisableEvents=this.DisableEvents; } ,DisableEvents:function (){ this._eventsEnabled= false; } ,EnableEvents:function (){ this._eventsEnabled= true; } ,AttachEvent:function (W,r){if (!this._listeners[W]){ this._listeners[W]=[]; } this._listeners[W][this._listeners[W].length]=(RadControlsNamespace.EventMixin.ResolveFunction(r)); } ,DetachEvent:function (W,r){var Q=this._listeners[W]; if (!Q){return false; }var P=RadControlsNamespace.EventMixin.ResolveFunction(r); for (var i=0; i<Q.length; i++){if (P==Q[i]){Q.splice(i,1); return true; }}return false; } ,ResolveFunction:function (N){if (typeof(N)=="\x66uncti\x6f\x6e"){return N; }else if (typeof(window[N])=="\x66unction"){return window[N]; }else {return new Function("\x76ar Sende\x72\x20= a\x72\x67um\x65\x6ets[\x30\x5d; v\x61\162\x20Arg\x75ment\x73\x20= \x61rgume\x6e\164\x73[1];"+N); }} ,RaiseEvent:function (W,n){if (!this._eventsEnabled){return true; }var M= true; if (this[W]){var m=RadControlsNamespace.EventMixin.ResolveFunction(this[W])(this,n); if (typeof(m)=="\x75ndefine\x64"){m= true; }M=M && m; }if (!this._listeners[W])return M; for (var i=0; i<this._listeners[W].length; i++){var r=this._listeners[W][i]; var m=r(this,n); if (typeof(m)=="\x75\156def\x69\x6eed"){m= true; }M=M && m; }return M; }};};if (typeof(RadUploadNameSpace)=="undef\x69\x6eed")RadUploadNameSpace= {} ; if (typeof(RadUploadNameSpace.Localization)=="undefi\x6e\x65d")RadUploadNameSpace.Localization=[]; RadUploadNameSpace.Localization.ProcessRawArray= function (L){var l=L[0]; if (typeof(RadUploadNameSpace.Localization[l])=="u\x6e\144\x65\x66ined"){RadUploadNameSpace.Localization[l]=[]; }for (var i=1; i<L.length; i+=2){RadUploadNameSpace.Localization[l][L[i]]=L[i+1]; }} ;;if (typeof window.RadControlsNamespace=="undefined"){window.RadControlsNamespace= {} ; }if (typeof(window.RadControlsNamespace.Overlay)=="\x75\x6edefine\x64" || typeof(window.RadControlsNamespace.Overlay.Version)==null || window.RadControlsNamespace.Overlay.Version<.11e1){window.RadControlsNamespace.Overlay= function (K){if (!this.SupportsOverlay()){return; } this.Element=K; this.Shim=document.createElement("I\x46\122\x41\x4dE"); this.Shim.src="\x6aavascript\x3a\x27\047\x3b"; this.Element.parentNode.insertBefore(this.Shim,this.Element); if (K.style.zIndex>0){ this.Shim.style.zIndex=K.style.zIndex-1; } this.Shim.style.position="\x61bsolute"; this.Shim.style.border="\x30px"; this.Shim.frameBorder=0; this.Shim.style.filter="\x70rogid:\x44\x58Imag\x65\x54ra\x6esform.M\x69\x63ros\x6f\146t\x2eAlph\x61(styl\x65\075\x30\054\x6f\160a\x63ity=0\x29"; this.Shim.disabled="\x64isab\x6c\x65d"; };window.RadControlsNamespace.Overlay.Version=.11e1; RadControlsNamespace.Overlay.prototype.SupportsOverlay= function (){return RadControlsNamespace.Browser.IsIE || (RadControlsNamespace.Browser.IsMozilla && RadControlsNamespace.Browser.IsMac); };RadControlsNamespace.Overlay.prototype.Update= function (){if (!this.SupportsOverlay()){return; } this.Shim.style.top=this.ToUnit(this.Element.style.top); this.Shim.style.left=this.ToUnit(this.Element.style.left); this.Shim.style.width=this.Element.offsetWidth+"p\x78"; this.Shim.style.height=this.Element.offsetHeight+"\x70x"; };RadControlsNamespace.Overlay.prototype.ToUnit= function (value){if (!value)return "0px"; return parseInt(value)+"\x70\x78"; };RadControlsNamespace.Overlay.prototype.Dispose= function (){if (!this.SupportsOverlay()){return; }if (this.Shim.parentNode){ this.Shim.parentNode.removeChild(this.Shim); } this.Element=null; this.Shim=null; };};if (typeof(RadUploadNameSpace)=="\x75ndefined")RadUploadNameSpace= {} ; RadUploadNameSpace.k="\x50anel"; RadUploadNameSpace.RadProgressArea= function (J){ this.Id=J[0]; this.OnClientProgressUpdating=J[1]; this.OnClientProgressBarUpdating=J[2]; this.H=J[3]; if (!this.H){alert("Co\x75\154\x64\x20not \x66\x69nd\x20\141n\x20\x69nst\x61\156c\x65 of R\x61dProgr\x65\163s\x4danage\x72\040\x6fn th\x65\040\x70age\x2e Ar\x65 you\x20miss\x69ng t\x68e co\x6etro\x6c dec\x6cara\x74ion\x3f"); }RadControlsNamespace.EventMixin.Initialize(this ); RadControlsNamespace.DomEventsMixin.Initialize(this ); this.Element=document.getElementById(this.Id); this.PrimaryProgressBarElement=this.FindElement("Prima\x72\x79Prog\x72\x65ssBa\x72"); this.PrimaryTotalElement=this.FindElement("\x50rimar\x79\x54otal"); this.PrimaryValueElement=this.FindElement("\x50rimaryVal\x75\x65"); this.PrimaryPercentElement=this.FindElement("\120r\x69\x6daryPer\x63\x65nt"); this.SecondaryProgressBarElement=this.FindElement("Sec\x6f\x6edaryPr\x6f\x67ress\x42ar"); this.SecondaryTotalElement=this.FindElement("SecondaryTo\x74\x61l"); this.SecondaryValueElement=this.FindElement("Seconda\x72\x79Value"); this.SecondaryPercentElement=this.FindElement("SecondaryP\x65\x72cent"); this.h=this.FindElement("CurrentOper\x61\x74ion"); this.TimeElapsedElement=this.FindElement("\124im\x65\x45lapsed"); this.TimeEstimatedElement=this.FindElement("TimeEstim\x61\x74ed"); this.SpeedElement=this.FindElement("Speed"); this.CancelButtonElement=this.FindElement("Cancel\x42\x75tton"); this.CancelClicked= false; if (this.CancelButtonElement){ this.AttachDomEvent(this.CancelButtonElement,"\x63\154ick","\x43ancelRequ\x65\x73t"); }if (typeof(RadUploadNameSpace.ProgressAreas)=="undefined"){RadUploadNameSpace.ProgressAreas=[]; } this.RegisterForAutomaticDisposal("Hide"); RadUploadNameSpace.ProgressAreas[RadUploadNameSpace.ProgressAreas.length]=this ; } ; RadUploadNameSpace.RadProgressArea.prototype= {Update:function (G){if (this.RaiseEvent("OnClientPr\x6fgressUpd\x61\x74in\x67", {ProgressData:G } )== false)return; this.Show(); if (this.RaiseEvent("OnClie\x6e\x74Prog\x72\x65ssB\x61\162U\x70\x64ati\x6e\147", {ProgressValue:G.PrimaryPercent,ProgressBarElementName: "\x50rimaryProgr\x65\x73sBar",ProgressBarElement: this.PrimaryProgressBarElement } )!= false){ this.UpdateHorizontalProgressBar(this.PrimaryProgressBarElement,G.PrimaryPercent); }if (this.RaiseEvent("\x4fnClientP\x72\x6fgre\x73\x73Bar\x55\x70dat\x69\156g", {ProgressValue:G.SecondaryPercent,ProgressBarElementName: "\x53econdar\x79\x50rogr\x65\x73sBa\x72",ProgressBarElement: this.SecondaryProgressBarElement } )!= false){ this.UpdateHorizontalProgressBar(this.SecondaryProgressBarElement,G.SecondaryPercent); } this.UpdateTextIndicator(this.PrimaryTotalElement,G.PrimaryTotal); this.UpdateTextIndicator(this.PrimaryValueElement,G.PrimaryValue); this.UpdateTextIndicator(this.PrimaryPercentElement,G.PrimaryPercent); this.UpdateTextIndicator(this.SecondaryTotalElement,G.SecondaryTotal); this.UpdateTextIndicator(this.SecondaryValueElement,G.SecondaryValue); this.UpdateTextIndicator(this.SecondaryPercentElement,G.SecondaryPercent); this.UpdateTextIndicator(this.h,G.CurrentOperationText); this.UpdateTextIndicator(this.TimeElapsedElement,G.TimeElapsed); this.UpdateTextIndicator(this.TimeEstimatedElement,G.TimeEstimated); this.UpdateTextIndicator(this.SpeedElement,G.Speed); } ,Show:function (){ this.Element.style.display=""; if (this.Element.style.position=="\x61bsolute"){if (typeof(this.Overlay)=="\x75\x6edefine\x64"){ this.Overlay=new RadControlsNamespace.Overlay(this.Element); } this.Overlay.Update(); }} ,Hide:function (){ this.Element.style.display="none"; if (this.Overlay){ this.Overlay.Dispose(); this.Overlay=null; }} ,UpdateHorizontalProgressBar:function (K,g){if (K && typeof(g)!="\165ndefined")K.style.width=g+"\x25"; } ,UpdateVerticalProgressBar:function (K,g){if (K && typeof(g)!="\x75ndefined")K.style.height=g+"\x25"; } ,UpdateTextIndicator:function (K,text){if (K && typeof(text)!="undefined"){if (typeof(K.value)=="string")K.value=text; else if (typeof(K.innerHTML)=="\x73tring")K.innerHTML=text; }} ,CancelRequest:function (){ this.CancelClicked= true; } ,FindElement:function (F){var f=this.Id+"\137"+RadUploadNameSpace.k+"_"+F; return document.getElementById(f); }};;if (typeof(RadUploadNameSpace)=="\165nd\x65\x66ined")RadUploadNameSpace= {} ; RadUploadNameSpace.RadProgressManager= function (J){RadControlsNamespace.EventMixin.Initialize(this ); RadControlsNamespace.DomEventsMixin.Initialize(this ); this.D=Math.max(J[0],50); var d=J[1]; this.EnableMemoryOptimizationIdentifier=J[2]; this.UniqueRequestIdentifier=J[3]; this.C=J[4]; this.OnClientProgressStarted=J[5]; this.OnClientProgressUpdating=J[6]; this.FormId=J[7]; this.c=J[8]; this.EnableMemoryOptimization=J[9]; this.SuppressMissingHttpModuleError=J[10]; this.TimeFormat="%HOURS\x25:%MINUTE\x53\x25:%S\x45\103O\x4e\x44S%s"; var form=document.getElementById(this.FormId); if (!form){form=document.forms[0]; } this.B(form); if (this.c== true){ this.RegisterForSubmit(form); } this.o0=this.O0(d); this.l0= false; if (typeof(RadUploadNameSpace.ProgressAreas)=="unde\x66\x69ned"){RadUploadNameSpace.ProgressAreas=[]; }} ; RadUploadNameSpace.RadProgressManager.prototype= {StartProgressPolling:function (){ this.InitSelectedFilesCount(); this.RaiseEvent("\x4fnClientPr\x6f\x67res\x73\x53ta\x72\x74ed"); this.i0=new Date(); this.MakeCallback(); } ,MakeCallback:function (){if (!this.l0){ this.l0= true; this.I0(); }} ,HandleCallback:function (){if (this.o1.readyState!=4)return; this.l0= false; if (this.ErrorOccured())return; var responseText=this.o1.responseText; if (responseText){try {eval(responseText); }catch (ex){ this.O1(); return; }if (rawProgressData){if (this.EnableMemoryOptimization== true && !this.SuppressMissingHttpModuleError && rawProgressData.ProgressError){alert(rawProgressData.ProgressError); return; }if (rawProgressData.InProgress){if (this.l1>0 || rawProgressData.RadProgressContextCustomCounters){ this.ModifyProgressData(rawProgressData); if (!this.UpdateProgressAreas(rawProgressData)){window.location.href=window.location.href; return; }}}}}window.setTimeout(this.CreateEventHandler("\x4dakeCallba\x63\x6b"),this.D); } ,ErrorOccured:function (){if (!document.all)return false; if (this.o1.status==404){ this.i1(); }else if (this.o1.status>0 && this.o1.status!=200){ this.I1(); }else return false; return true; } ,i1:function (){alert("\x72.a.d\x2e\x75ploa\x64\x20Aja\x78 callba\x63\x6b e\x72\x72or\x2e Sou\x72ce ur\x6c\040\x77as n\x6ft fou\x6ed: \012\x0d\012\x0d"+this.o0+"\012\x0d\x0a\015\x44\151d\x20\x79ou\x20\x72eg\x69\x73te\x72\x20th\x65 RadUp\x6coadPr\x6f\147r\x65ssHan\x64ler \x69\156 \x77eb.c\x6fnfi\x67?"+"\015\x0a\x0d\012\x50lease,\x20\x73ee \x74\150e\x20\x68el\x70\x20fo\x72\040m\x6fre de\x74ails:\x20\122a\x64Uplo\x61\144 \x32.0 -\x20Usin\x67 r.\x61.d.u\x70loa\x64 - C\x6fnfi\x67ura\x74ion \x2d R\x61dUp\x6coad\x50rog\x72es\x73Han\x64le\x72."); } ,I1:function (){alert("r\x2e\x61.d.uplo\x61\x64 Aj\x61\x78 c\x61\x6clba\x63\153 \x65\x72r\x6f\162.\x20\123o\x75\162c\x65 url \x72eturn\x65d err\x6fr: "+this.o1.status+"\040\012\x0d\012\x0d"+this.o1.o2+"\x20\012\015\x0a\015"+this.o0+"\012\x0d\x0a\015\x44id you \x72\145g\x69\x73te\x72\x20th\x65\040\x52\x61dU\x70loadP\x72\157g\x72essHa\x6edler\x20in we\x62.con\x66ig?"+"\015\x0a\x0d\012\x50lease,\x20\x73ee\x20\164h\x65\x20he\x6c\160 \x66or mo\x72\145 \x64\145t\x61ils:\x20\122a\x64Uplo\x61d 2.\x30 - U\x73ing\x20r.a.\x64.upl\x6fad \x2d Co\x6efig\x75rat\x69on \x2d Ra\x64Upl\x6fad\x50rog\x72es\x73Ha\x6edle\x72."); } ,O1:function (){alert("r.a.d\x2e\x75pload\x20\x41jax\x20\143a\x6c\x6cbac\x6b\x20er\x72\157\x72\056 \x53ource \x75rl re\x74\165r\x6eed in\x76alid \x63ont\x65nt: \x0a\015\x0a\015"+this.o1.responseText+"\x0a\015\012\x0d"+this.o0+"\012\015\x0a\015\x44\x69d y\x6f\165 \x72\145gi\x73\164e\x72\040t\x68e RadU\x70loadP\x72\157g\x72essHa\x6edler \x69n we\x62.con\x66ig?"+"\015\012\x0d\012\x50\154e\x61\x73e, \x73\145e\x20\x74he\x20\x68el\x70\040f\x6fr mor\x65\040d\x65tails\x3a RadU\x70load\x202.0 -\x20Usi\x6eg r.\x61.d.u\x70load\x20- C\x6fnfi\x67urat\x69on \x2d Ra\x64Up\x6coad\x50rog\x72es\x73Han\x64le\x72."); } ,UpdateProgressAreas:function (rawProgressData){ this.RaiseEvent("\x4fnClientP\x72\x6fgres\x73\x55pd\x61\x74ing", {ProgressData:rawProgressData } ); for (var i=0; i<RadUploadNameSpace.ProgressAreas.length; i++){var O2=RadUploadNameSpace.ProgressAreas[i]; if (O2.CancelClicked){return false; }O2.Update(rawProgressData); }return true; } ,ModifyProgressData:function (rawProgressData){var l2=new Date()-this.i0; if (typeof(rawProgressData.TimeElapsed)=="u\x6edefined")rawProgressData.TimeElapsed=this.GetFormattedTime(this.ToSeconds(l2)); if (rawProgressData.RadUpload){var i2=rawProgressData.RadUpload.RequestSize; var I2=rawProgressData.RadUpload.Bytes; if (typeof(rawProgressData.PrimaryTotal)=="\x75ndefined")rawProgressData.PrimaryTotal=this.FormatBytes(i2); if (typeof(rawProgressData.PrimaryValue)=="\x75ndefine\x64")rawProgressData.PrimaryValue=this.FormatBytes(I2); if (typeof(rawProgressData.PrimaryPercent)=="und\x65\x66ined")rawProgressData.PrimaryPercent=Math.round(100*I2/i2); if (typeof(rawProgressData.SecondaryTotal)=="\165n\x64\x65fined")rawProgressData.SecondaryTotal=this.l1; if (typeof(rawProgressData.SecondaryValue)=="\x75ndefined")rawProgressData.SecondaryValue=rawProgressData.RadUpload.FilesCount; if (typeof(rawProgressData.SecondaryPercent)=="\x75\x6edefined")rawProgressData.SecondaryPercent=Math.round(100*rawProgressData.RadUpload.FilesCount/(this.l1!=0?this.l1: 1)); if (typeof(rawProgressData.CurrentOperationText)=="und\x65\x66ined")rawProgressData.CurrentOperationText=rawProgressData.RadUpload.CurrentFileName; if (typeof(rawProgressData.Speed)=="und\x65\x66ined")rawProgressData.Speed=this.FormatBytes(rawProgressData.RadUpload.Bytes/this.ToSeconds(l2))+"/s"; }if (typeof(rawProgressData.TimeEstimated)=="und\x65\x66ined" && typeof(rawProgressData.PrimaryPercent)=="number")rawProgressData.TimeEstimated=this.GetFormattedTime(this.ToSeconds(l2*(100/rawProgressData.PrimaryPercent-1))); } ,ToSeconds:function (o3){return Math.round(o3/1000); } ,InitSelectedFilesCount:function (){ this.l1=0; var O3=document.getElementsByTagName("input"); for (var i=0; i<O3.length; i++){var l3=O3[i]; if (l3.type=="\x66\151le" && l3.value!=""){ this.l1++; }}} ,I0:function (){if (typeof(XMLHttpRequest)!="undefined"){ this.o1=new XMLHttpRequest(); }else if (typeof(ActiveXObject)!="\x75ndefined"){ this.o1=new ActiveXObject("\x4d\x69crosoft\x2e\x58MLH\x54\x54P"); }else return; this.o1.onreadystatechange=this.CreateEventHandler("H\x61\x6edleCal\x6c\x62ack"); this.o1.open("\x47\x45T",this.i3(), true); this.o1.send(""); } ,I3:function (Z,method){return function (){method.apply(Z,arguments); } ; } ,O0:function (d){var o4=d.indexOf("?")<0?"\x3f": "&"; return d+o4+this.UniqueRequestIdentifier+"\x3d"+this.C; } ,i3:function (){return this.o0+"\x26\x52adUplo\x61\x64Time\x53\x74am\x70\075"+new Date().getTime(); } ,RegisterForSubmit:function (form){var O4=this.CreateEventHandler("StartP\x72\x6fgres\x73\x50oll\x69\156g"); var l4=form.submit; try {form.submit= function (){O4(); form.submit=l4; form.submit(); };}catch (exception){try {var i4=__doPostBack; __doPostBack= function (eventTarget,eventArgument){var I4= true; if (typeof(Page_ClientValidate)=="function"){I4=Page_ClientValidate(); }if (I4){O4(); i4(eventTarget,eventArgument); }} ; }catch (exception){}} this.AttachDomEvent(form,"\x73ubmit","\x53tartProgres\x73\x50oll\x69\x6eg"); } ,B:function (form){if (typeof(form.action)=="undefined")form.action=""; if (form.action.match(/\x3f/)){form.action=this.o5(form.action,this.UniqueRequestIdentifier); form.action=this.o5(form.action,this.EnableMemoryOptimizationIdentifier); if (form.action.substring(form.action.length-1)!="?"){form.action+="\x26"; }}else {form.action+="?";}form.action+=this.UniqueRequestIdentifier+"="+this.C; if (this.EnableMemoryOptimization){form.enctype=form.encoding="\x6dultipart/f\x6f\x72m-da\x74\x61"; }else {form.action+="\x26"+this.EnableMemoryOptimizationIdentifier+"\x3dfalse"; }} ,o5:function (O5,l5){var i5=new RegExp("&?"+l5+"\x3d[^&]*"); if (O5.match(i5)){return O5.replace(i5,""); }return O5; } ,FormatBytes:function (I5){var o6=I5/1024; var O6=o6/1024; if (O6>.8){return ""+Math.round(O6*100)/100+"MB"; }if (o6>.8){return ""+Math.round(o6*100)/100+"kB"; }return ""+I5+" \x62ytes"; } ,GetFormattedTime:function (l6){var i6=this.NormalizeTime(l6); return this.TimeFormat.replace(/\x25\x48\x4f\x55\x52\x53\x25/,i6.I6).replace(/\x25\x4d\x49\x4e\x55\x54\x45\x53\x25/,i6.o7).replace(/\x25\x53\x45\x43\x4f\x4e\x44\x53\x25/,i6.O7); } ,NormalizeTime:function (l7){var l6=l7%60; var i7=Math.floor(l7/60); var I7=i7%60; var o8=Math.floor(i7/60); return {I6:o8,o7:I7,O7:l6 };}} ;;RadUploadNameSpace.RadUploadEventArgs= function (O8){ this.FileInputField=O8; } ; RadUploadNameSpace.RadUploadDeleteSelectedEventArgs= function (l8){ this.FileInputFields=l8; } ;;if (typeof(window.RadControlsNamespace)=="\x75ndefined"){window.RadControlsNamespace=new Object(); } ; RadControlsNamespace.AppendStyleSheet= function (i8,I8,o9){if (!o9){return; }if (!i8){document.write("\x3c"+"link"+"\x20rel=\047\163t\x79\x6cesh\x65et\047\x20type=\x27\x74ex\x74\x2f\x63\163s\x27 href\x3d\047"+o9+"\047\x20\x2f>"); }else {var O9=document.createElement("LINK"); O9.rel="\x73tylesheet"; O9.type="\164e\x78\x74/css"; O9.href=o9; document.getElementById(I8+"StyleShee\x74\x48olde\x72").appendChild(O9); }} ;;if (typeof(Sys) != "undefined"){if (Sys.Application != null && Sys.Application.notifyScriptLoaded != null){Sys.Application.notifyScriptLoaded();}}
