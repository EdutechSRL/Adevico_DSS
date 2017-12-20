Type.registerNamespace("Telerik.Web.UI.Dialogs");
function initDialog(){var a=$find("initializer");
if(a){a.distributeClientParameters();
}}function setInnerHtml(b,c){var a=document.getElementById(b);
if(a){a.innerHTML=c;
}}Telerik.Web.UI.Dialogs.CommonDialogScript=function(){};
Telerik.Web.UI.Dialogs.CommonDialogScript.get_windowReference=function(){if(window.radWindow){return window.radWindow;
}if(window.frameElement&&window.frameElement.radWindow){return window.frameElement.radWindow;
}if(!window.__localRadEditorRadWindowReference&&window.opener&&window.opener.__getCurrentRadEditorRadWindowReference){window.__localRadEditorRadWindowReference=window.opener.__getCurrentRadEditorRadWindowReference();
}return window.__localRadEditorRadWindowReference;
};
Telerik.Web.UI.Dialogs.CommonDialogScript.registerClass("Telerik.Web.UI.Dialogs.CommonDialogScript",null);
Telerik.Web.IParameterConsumer=function(){};
Telerik.Web.IParameterConsumer.prototype={clientInit:function(a){throw Error.notImplemented();
}};
Telerik.Web.IParameterConsumer.registerInterface("Telerik.Web.IParameterConsumer");
Telerik.Web.UI.Dialogs.DialogInitializer=function(a){Telerik.Web.UI.Dialogs.DialogInitializer.initializeBase(this,[a]);
this._parameterConsumers=[];
};
Telerik.Web.UI.Dialogs.DialogInitializer.prototype={initialize:function(){Telerik.Web.UI.Dialogs.DialogInitializer.callBaseMethod(this,"initialize");
this._appLoadHandlerDelegate=Function.createDelegate(this,this._appLoadHandler);
Sys.Application.add_load(this._appLoadHandlerDelegate);
},get_parameterConsumers:function(){return this._parameterConsumers;
},set_parameterConsumers:function(a){this._parameterConsumers=a;
},get_clientParameters:function(){return Telerik.Web.UI.Dialogs.CommonDialogScript.get_windowReference().ClientParameters;
},autoSizeWindow:function(){if($telerik.isOpera){return;
}try{var f=Telerik.Web.UI.Dialogs.CommonDialogScript.get_windowReference();
if(f){var d=document.body.offsetHeight;
var a=$telerik.getBounds(f.get_contentFrame().parentNode);
var b=d-a.height;
if(b&&b<100&&b>-100){d=f.getWindowBounds().height;
d+=b;
f.set_height(d);
}}}catch(c){}},distributeClientParameters:function(){this.autoSizeWindow();
if(window.focus){window.focus();
}for(var a=0;
a<this._parameterConsumers.length;
a++){var b=$find(this._parameterConsumers[a]);
if(b&&b.clientInit){b.clientInit(this.get_clientParameters());
}}},_appLoadHandler:function(){this.distributeClientParameters();
var c=Telerik.Web.UI.Dialogs.CommonDialogScript.get_windowReference();
if(c&&c.get_contentFrame()&&c.get_contentFrame().contentWindow){var b=c.get_contentFrame().contentWindow.localization;
if(b&&b.Close&&c._getTitleCommandButton){var a=c._getTitleCommandButton("close");
if(a){a.title=b.Close;
}}}Sys.Application.remove_load(this._appLoadHandlerDelegate);
this._appLoadHandlerDelegate=null;
}};
Telerik.Web.UI.Dialogs.DialogInitializer.registerClass("Telerik.Web.UI.Dialogs.DialogInitializer",Telerik.Web.UI.RadWebControl);
Type.registerNamespace("Telerik.Web.UI");
if(typeof(Telerik.Web.UI.EditorCommandEventArgs)=="undefined"){Telerik.Web.UI.EditorCommandEventArgs=function(a,b,c){Telerik.Web.UI.EditorCommandEventArgs.initializeBase(this);
this._name=this._commandName=a;
this._tool=b;
this._value=c;
this.value=c;
this._callbackFunction=null;
};
Telerik.Web.UI.EditorCommandEventArgs.prototype={get_name:function(){return this._name;
},get_commandName:function(){return this._commandName;
},get_tool:function(){return this._tool;
},get_value:function(){return this._value;
},set_value:function(a){this.value=a;
this._value=a;
},set_callbackFunction:function(a){this._callbackFunction=a;
}};
Telerik.Web.UI.EditorCommandEventArgs.registerClass("Telerik.Web.UI.EditorCommandEventArgs",Sys.CancelEventArgs);
}