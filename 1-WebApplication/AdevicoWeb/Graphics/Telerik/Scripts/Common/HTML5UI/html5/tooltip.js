(function(b,a){a(["./kendo.core","./kendo.popup"],b);
})(function(){(function(a,E){var q=window.kendo,F=q.ui.Widget,s=q.ui.Popup,m=q.isFunction,o=a.isPlainObject,j=a.extend,u=a.proxy,h=a(document),n=q.isLocalUrl,b="_tt_active",e="aria-describedby",B="show",k="hide",i="error",c="contentLoad",v="requestStart",p="k-content-frame",C='<div role="tooltip" class="k-widget k-tooltip#if (!autoHide) {# k-tooltip-closable#}#">#if (!autoHide) {# <div class="k-tooltip-button"><a href="\\#" class="k-icon k-i-close">close</a></div> #}#<div class="k-tooltip-content"></div>#if (callout){ #<div class="k-callout k-callout-#=dir#"></div>#}#</div>',l=q.template("<iframe frameborder='0' class='"+p+"' src='#= content.url #'>This page requires frames in order to show content</iframe>"),r=".kendoTooltip",t={bottom:{origin:"bottom center",position:"top center"},top:{origin:"top center",position:"bottom center"},left:{origin:"center left",position:"center right",collision:"fit flip"},right:{origin:"center right",position:"center left",collision:"fit flip"},center:{position:"center center",origin:"center center"}},y={top:"bottom",bottom:"top",left:"right",right:"left",center:"center"},g={bottom:"n",top:"s",left:"e",right:"w",center:"n"},f={horizontal:{offset:"top",size:"outerHeight"},vertical:{offset:"left",size:"outerWidth"}},d=function(G){return G.target.data(q.ns+"title");
};
function w(G){while(G.length){x(G);
G=G.parent();
}}function x(G){var H=G.data(q.ns+"title");
if(H){G.attr("title",H);
G.removeData(q.ns+"title");
}}function z(G){var H=G.attr("title");
if(H){G.data(q.ns+"title",H);
G.attr("title","");
}}function A(G){while(G.length&&!G.is("body")){z(G);
G=G.parent();
}}var D=F.extend({init:function(H,I){var J=this,G;
F.fn.init.call(J,H,I);
G=J.options.position.match(/left|right/)?"horizontal":"vertical";
J.dimensions=f[G];
J._documentKeyDownHandler=u(J._documentKeyDown,J);
J.element.on(J.options.showOn+r,J.options.filter,u(J._showOn,J)).on("mouseenter"+r,J.options.filter,u(J._mouseenter,J));
if(this.options.autoHide){J.element.on("mouseleave"+r,J.options.filter,u(J._mouseleave,J));
}},options:{name:"Tooltip",filter:"",content:d,showAfter:100,callout:true,position:"bottom",showOn:"mouseenter",autoHide:true,width:null,height:null,animation:{open:{effects:"fade:in",duration:0},close:{effects:"fade:out",duration:40,hide:true}}},events:[B,k,c,i,v],_mouseenter:function(G){A(a(G.currentTarget));
},_showOn:function(H){var I=this;
var G=a(H.currentTarget);
if(I.options.showOn&&I.options.showOn.match(/click|focus/)){I._show(G);
}else{clearTimeout(I.timeout);
I.timeout=setTimeout(function(){I._show(G);
},I.options.showAfter);
}},_appendContent:function(K){var L=this,G=L.options.content,H=L.content,J=L.options.iframe,I;
if(o(G)&&G.url){if(!("iframe" in L.options)){J=!n(G.url);
}L.trigger(v,{options:G,target:K});
if(!J){H.empty();
q.ui.progress(H,true);
L._ajaxRequest(G);
}else{H.hide();
I=H.find("."+p)[0];
if(I){I.src=G.url||I.src;
}else{H.html(l({content:G}));
}H.find("."+p).off("load"+r).on("load"+r,function(){L.trigger(c);
H.show();
});
}}else{if(G&&m(G)){G=G({sender:this,target:K});
H.html(G||"");
}else{H.html(G);
}}L.angular("compile",function(){return{elements:H};
});
},_ajaxRequest:function(G){var H=this;
jQuery.ajax(j({type:"GET",dataType:"html",cache:false,error:function(J,I){q.ui.progress(H.content,false);
H.trigger(i,{status:I,xhr:J});
},success:u(function(I){q.ui.progress(H.content,false);
H.content.html(I);
H.trigger(c);
},H)},G));
},_documentKeyDown:function(G){if(G.keyCode===q.keys.ESC){this.hide();
}},refresh:function(){var H=this,G=H.popup;
if(G&&G.options.anchor){H._appendContent(G.options.anchor);
}},hide:function(){if(this.popup){this.popup.close();
}},show:function(G){G=G||this.element;
A(G);
this._show(G);
},_show:function(H){var I=this,G=I.target();
if(!I.popup){I._initPopup();
}if(G&&G[0]!=H[0]){I.popup.close();
I.popup.element.kendoStop(true,true);
}if(!G||G[0]!=H[0]){I._appendContent(H);
I.popup.options.anchor=H;
}I.popup.one("deactivate",function(){w(H);
H.removeAttr(e);
this.element.removeAttr("id").attr("aria-hidden",true);
h.off("keydown"+r,I._documentKeyDownHandler);
});
I.popup.open();
},_initPopup:function(){var H=this,G=H.options,I=a(q.template(C)({callout:G.callout&&G.position!=="center",dir:g[G.position],autoHide:G.autoHide}));
H.popup=new s(I,j({activate:function(){var J=this.options.anchor,K=J[0].id||H.element[0].id;
if(K){J.attr(e,K+b);
this.element.attr("id",K+b);
}if(G.callout){H._positionCallout();
}this.element.removeAttr("aria-hidden");
h.on("keydown"+r,H._documentKeyDownHandler);
H.trigger(B);
},close:function(){H.trigger(k);
},copyAnchorStyles:false,animation:G.animation},t[G.position]));
I.css({width:G.width,height:G.height});
H.content=I.find(".k-tooltip-content");
H.arrow=I.find(".k-callout");
if(G.autoHide){I.on("mouseleave"+r,u(H._mouseleave,H));
}else{I.on("click"+r,".k-tooltip-button",u(H._closeButtonClick,H));
}},_closeButtonClick:function(G){G.preventDefault();
this.hide();
},_mouseleave:function(G){if(this.popup){var H=a(G.currentTarget),I=H.offset(),J=G.pageX,K=G.pageY;
I.right=I.left+H.outerWidth();
I.bottom=I.top+H.outerHeight();
if(J>I.left&&J<I.right&&K>I.top&&K<I.bottom){return;
}this.popup.close();
}else{w(a(G.currentTarget));
}clearTimeout(this.timeout);
},_positionCallout:function(){var Q=this,P=Q.options.position,K=Q.dimensions,M=K.offset,O=Q.popup,G=O.options.anchor,H=a(G).offset(),I=parseInt(Q.arrow.css("border-top-width"),10),L=a(O.element).offset(),J=g[O.flipped?y[P]:P],N=H[M]-L[M]+(a(G)[K.size]()/2)-I;
Q.arrow.removeClass("k-callout-n k-callout-s k-callout-w k-callout-e").addClass("k-callout-"+J).css(M,N);
},target:function(){if(this.popup){return this.popup.options.anchor;
}return null;
},destroy:function(){var G=this.popup;
if(G){G.element.off(r);
G.destroy();
}this.element.off(r);
h.off("keydown"+r,this._documentKeyDownHandler);
F.fn.destroy.call(this);
}});
q.ui.plugin(D);
})(window.kendo.jQuery);
return window.kendo;
},typeof define=="function"&&define.amd?define:function(a,b){b();
});
