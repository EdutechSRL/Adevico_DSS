(function(a,c,b,d){$telerik.$.extend(b.MozEmStrongFilter.prototype,{getHtmlContent:function(e){var f=e.replace(new RegExp("<b(\\s([^>])*?)?>","ig"),"<strong$1>");
f=f.replace(new RegExp("</b(\\s([^>])*?)?>","ig"),"</strong$1>");
f=f.replace(new RegExp("<i(\\s([^>])*?)?>","ig"),"<em$1>");
f=f.replace(new RegExp("</i(\\s([^>])*?)?>","ig"),"</em$1>");
return f;
},getDesignContent:function(e){var f=e.replace(new RegExp("<strong(\\s([^>])*?)?>","ig"),"<b$1>");
f=f.replace(new RegExp("</strong(\\s([^>])*?)?>","ig"),"</b$1>");
f=f.replace(new RegExp("<em(\\s([^>])*?)?>","ig"),"<i$1>");
f=f.replace(new RegExp("</em(\\s([^>])*?)?>","ig"),"</i$1>");
return f;
}});
})($telerik.$,Telerik.Web.UI,Telerik.Web.UI.Editor);
(function(a,c,b,d,e){a.extend(d,{Bold:new c.BrowserCommand(null,null,"Bold",null),Italic:new c.BrowserCommand(null,null,"Italic",null),Underline:new c.BrowserCommand(null,null,"Underline",null),StrikeThrough:new c.BrowserCommand(null,null,"StrikeThrough",null),Subscript:new c.BrowserCommand(null,null,"Subscript",null),Superscript:new c.BrowserCommand(null,null,"Superscript",null),Unlink:new c.BrowserCommand(null,null,"Unlink",null)});
b.Bold=b.Italic=b.Underline=b.StrikeThrough=b.Subscript=b.Superscript=function(f,g,h){g.setActive();
g.executeBrowserCommand(f,true,null,null);
return true;
};
b.Unlink=function(f,g,i){g.setActive();
if(!g.isIE){var h=g.getSelectedElement();
if(h&&h.tagName=="A"){g.selectElement(h,false);
}}g.executeBrowserCommand(f,true,null,null);
return true;
};
b.ForeColor=b.BackColor=b.FontName=function(g,h,f){var i=new c.ieFontCommandFix(h);
i.applyFix();
h.executeBrowserCommand(g,true,f.value);
i.cleanUp();
};
b.RealFontSize=function(g,h,f){var s=f.get_value();
var m=new Telerik.Web.UI.Editor.GenericCommand(h.getLocalizedString(g)+' ["'+s+'"]',h.get_contentWindow(),h);
var q=Telerik.Web.UI.Editor.CommandList._markEditorSelection(h);
var l=q.markedElements;
if(l.length>0){var k=[];
for(var n=0;
n<l.length;
n++){var j=l[n];
var o=false;
if($telerik.isSafari){var p=j.parentNode;
var r=p.tagName;
if((r=="FONT"||r=="SPAN")&&p.childNodes.length==1){p.style.fontSize=s;
p.removeAttribute("size");
Telerik.Web.UI.Editor.Utils.removeNode(j);
o=true;
k.push(p);
h.selectElement(p);
}}if(!o){j.style.fontSize=s;
j.removeAttribute("size");
k.push(j);
}}if(!$telerik.isIE){Telerik.Web.UI.Editor.Utils.addElementsToSelection(h.get_contentWindow(),k);
}}else{Telerik.Web.UI.Editor.CommandList._completeEditorSelection(h,"style='font-size:"+s+"'");
}h.executeCommand(m);
Telerik.Web.UI.Editor.Utils.removeInvalidElementsInLists(h.get_document());
};
})($telerik.$,Telerik.Web.UI.Editor,Telerik.Web.UI.Editor.CommandList,Telerik.Web.UI.Editor.UpdateCommandsArray);
