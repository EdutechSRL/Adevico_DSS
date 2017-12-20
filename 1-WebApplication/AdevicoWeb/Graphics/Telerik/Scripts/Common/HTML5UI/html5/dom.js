(function(b,a){a(["./kendo.core"],b);
})(function(){(function(e){function f(){this.node=null;
}f.prototype={remove:function(){this.node.parentNode.removeChild(this.node);
},attr:{}};
function h(){}h.prototype={nodeName:"#null",attr:{style:{}},children:[],remove:function(){}};
var g=new h();
function b(n,l,m){this.nodeName=n;
this.attr=l||{};
this.children=m||[];
}b.prototype=new f();
b.prototype.appendTo=function(o){var n=document.createElement(this.nodeName);
var l=this.children;
for(var m=0;
m<l.length;
m++){l[m].render(n,g);
}o.appendChild(n);
return n;
};
b.prototype.render=function(s,l){var r;
if(l.nodeName!==this.nodeName){l.remove();
r=this.appendTo(s);
}else{r=l.node;
var p;
var o=this.children;
var q=o.length;
var m=l.children;
var n=m.length;
if(Math.abs(n-q)>2){this.render({appendChild:function(t){s.replaceChild(t,l.node);
}},g);
return;
}for(p=0;
p<q;
p++){o[p].render(r,m[p]||g);
}for(p=q;
p<n;
p++){m[p].remove();
}}this.node=r;
this.syncAttributes(l.attr);
this.removeAttributes(l.attr);
};
b.prototype.syncAttributes=function(m){var l=this.attr;
for(var o in l){var p=l[o];
var n=m[o];
if(o==="style"){this.setStyle(p,n);
}else{if(p!==n){this.setAttribute(o,p,n);
}}}};
b.prototype.setStyle=function(o,l){var n=this.node;
var m;
if(l){for(m in o){if(o[m]!==l[m]){n.style[m]=o[m];
}}}else{for(m in o){n.style[m]=o[m];
}}};
b.prototype.removeStyle=function(l){var o=this.attr.style||{};
var n=this.node;
for(var m in l){if(o[m]===undefined){n.style[m]="";
}}};
b.prototype.removeAttributes=function(m){var l=this.attr;
for(var n in m){if(n==="style"){this.removeStyle(m.style);
}else{if(l[n]===undefined){this.removeAttribute(n);
}}}};
b.prototype.removeAttribute=function(l){var m=this.node;
if(l==="style"){m.style.cssText="";
}else{if(l==="className"){m.className="";
}else{m.removeAttribute(l);
}}};
b.prototype.setAttribute=function(m,o,l){var n=this.node;
if(n[m]!==undefined){n[m]=o;
}else{n.setAttribute(m,o);
}};
function j(l){this.nodeValue=l;
}j.prototype=new f();
j.prototype.nodeName="#text";
j.prototype.render=function(n,l){var m;
if(l.nodeName!==this.nodeName){l.remove();
m=document.createTextNode(this.nodeValue);
n.appendChild(m);
}else{m=l.node;
if(this.nodeValue!==l.nodeValue){m.nodeValue=this.nodeValue;
}}this.node=m;
};
function d(l){this.html=l;
}d.prototype={nodeName:"#html",attr:{},remove:function(){for(var l=0;
l<this.nodes.length;
l++){this.nodes[l].parentNode.removeChild(this.nodes[l]);
}},render:function(o,l){if(l.nodeName!==this.nodeName||l.html!==this.html){l.remove();
var n=o.lastChild;
o.insertAdjacentHTML("beforeend",this.html);
this.nodes=[];
for(var m=n?n.nextSibling:o.firstChild;
m;
m=m.nextSibling){this.nodes.push(m);
}}else{this.nodes=l.nodes.slice(0);
}}};
function c(l){return new d(l);
}function a(n,l,m){return new b(n,l,m);
}function i(l){return new j(l);
}function k(l){this.root=l;
this.children=[];
}k.prototype={html:c,element:a,text:i,render:function(m){var l=this.children;
var n;
var o;
for(n=0,o=m.length;
n<o;
n++){m[n].render(this.root,l[n]||g);
}for(n=o;
n<l.length;
n++){l[n].remove();
}this.children=m;
}};
e.dom={html:c,text:i,element:a,Tree:k};
})(window.kendo);
return window.kendo;
},typeof define=="function"&&define.amd?define:function(a,b){b();
});
