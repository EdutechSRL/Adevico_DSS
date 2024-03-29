Type.registerNamespace("Telerik.Web.UI");
(function(){var a=Telerik.Web.UI;
a.RadEditor=function(b){a.RadEditor.initializeBase(this,[b]);
this._textArea=null;
this._saveContentDelegate=null;
this._contentSaved=null;
};
a.RadEditor.prototype={initialize:function(){a.RadEditor.callBaseMethod(this,"initialize");
this._textArea=$get(this.get_id()+"TextArea");
this._registerPostBackHandlers();
if(this._textArea&&this._textArea.value){this._contentSaved=false;
var b=this.decodePostbackContent(this._textArea.value);
this.set_html(b);
}},dispose:function(){if(this._textArea&&this._textArea.form&&this._saveContentDelegate){$telerik.removeExternalHandler(this._textArea.form,"submit",this._saveContentDelegate);
}a.RadEditor.callBaseMethod(this,"dispose");
},saveContent:function(c,b){if(this._saveContentDelegate&&this._textArea&&!this._contentSaved){htmlValue=this.get_html();
this._textArea.value=this.encodePostbackContent(htmlValue);
this._contentSaved=true;
}},_registerPostBackHandlers:function(){this._saveContentDelegate=Function.createDelegate(this,this.saveContent);
var c=this._textArea.form;
if(c){$telerik.addExternalHandler(c,"submit",this._saveContentDelegate);
}if(typeof(__doPostBack)!="undefined"){var h=this._saveContentDelegate;
var f=__doPostBack;
__doPostBack=function(k,j){h();
f(k,j);
};
}if(typeof(Sys.WebForms)!="undefined"&&typeof(Sys.WebForms.PageRequestManager)!="undefined"){var e=Sys.WebForms.PageRequestManager.getInstance();
if(e){if(null!=e._originalDoPostBack){var i=this._saveContentDelegate;
var g=e._originalDoPostBack;
e._originalDoPostBack=function(k,j){i();
g(k,j);
};
}if(null!=e._onSubmitStatements&&!$telerik.isIE){var d=this;
var b={};
Array.add(e._onSubmitStatements,function(){d.saveContent(null,b);
return true;
});
}else{e.add_initializeRequest(this._saveContentDelegate);
}}e=null;
}c=null;
},get_mode:function(){},set_mode:function(b){},get_textArea:function(){return this._textArea;
},set_html:function(b){var c=b.replace(/\n?\<br[^\>]*?>/gi,"\n");
if(!(/\<[^\>]+\>/gi).test(c)){b=c;
}this.get_textArea().value=b;
},get_html:function(b){var c=this.get_textArea().value;
if(!(/\<[^\>]+\>/gi).test(c)){c=c.replace(/\n/g,"\n<br/>");
}return c;
},get_text:function(){return this.get_textArea().value;
},setSize:function(c,b){},pasteHtml:function(f,e,d,c,b){},get_contentArea:function(){return this.get_textArea();
},get_document:function(){return this.get_element().document;
},_encodeHtmlContent:function(c,f){var b=new Array("%","<",">","!",'"',"#","$","&","'","(",")",",",":",";","=","?","[","]","\\","^","`","{","|","}","~","+");
var e=c;
var d;
if(f){for(d=0;
d<b.length;
d++){e=e.replace(new RegExp("\\x"+b[d].charCodeAt(0).toString(16),"ig"),"%"+b[d].charCodeAt(0).toString(16));
}}else{for(d=b.length-1;
d>=0;
d--){e=e.replace(new RegExp("%"+b[d].charCodeAt(0).toString(16),"ig"),b[d]);
}}return e;
},encodePostbackContent:function(b){return this._encodeHtmlContent(b,true);
},decodePostbackContent:function(b){return this._encodeHtmlContent(b,false);
},setActive:function(){},set_editable:function(b){},get_editable:function(){},get_initialContent:function(){},add_firstShow:function(b){},remove_firstShow:function(b){},add_pasteHtml:function(b){},remove_pasteHtml:function(b){},add_editReady:function(b){},remove_editReady:function(b){},add_submit:function(b){},remove_submit:function(b){},add_commandExecuted:function(b){},remove_commandExecuted:function(b){},add_commandExecuting:function(b){},remove_commandExecuting:function(b){},add_selectionChange:function(b){},remove_selectionChange:function(b){},add_init:function(b){},remove_init:function(b){},add_load:function(b){},remove_load:function(b){},add_modeChange:function(b){},remove_modeChange:function(b){},add_toggleScreenMode:function(b){},remove_toggleScreenMode:function(b){},add_spellCheckLoaded:function(b){},remove_spellCheckLoaded:function(b){}};
a.RadEditor.registerClass("Telerik.Web.UI.RadEditor",a.RadWebControl);
})();
