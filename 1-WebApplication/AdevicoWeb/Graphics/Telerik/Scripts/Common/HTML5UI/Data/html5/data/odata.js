(function(b,a){a(["./kendo.core"],b);
})(function(){(function(a,j){var d=window.kendo,c=a.extend,f={eq:"eq",neq:"ne",gt:"gt",gte:"ge",lt:"lt",lte:"le",contains:"substringof",doesnotcontain:"substringof",endswith:"endswith",startswith:"startswith"},g=c({},f,{contains:"contains"}),e={pageSize:a.noop,page:a.noop,filter:function(l,k,m){if(k){l.$filter=i(k,m);
}},sort:function(m,l){var k=a.map(l,function(o){var n=o.field.replace(/\./g,"/");
if(o.dir==="desc"){n+=" desc";
}return n;
}).join(",");
if(k){m.$orderby=k;
}},skip:function(k,l){if(l){k.$skip=l;
}},take:function(k,l){if(l){k.$top=l;
}}},b={read:{dataType:"jsonp"}};
function i(l,v){var t=[],r=l.logic||"and",o,q,k,u,n,s,w,p,m=l.filters;
for(o=0,q=m.length;
o<q;
o++){l=m[o];
k=l.field;
w=l.value;
s=l.operator;
if(l.filters){l=i(l,v);
}else{p=l.ignoreCase;
k=k.replace(/\./g,"/");
l=f[s];
if(v){l=g[s];
}if(l&&w!==j){u=a.type(w);
if(u==="string"){n="'{1}'";
w=w.replace(/'/g,"''");
if(p===true){k="tolower("+k+")";
}}else{if(u==="date"){if(v){n="{1:yyyy-MM-ddTHH:mm:ss+00:00}";
}else{n="datetime'{1:yyyy-MM-ddTHH:mm:ss}'";
}}else{n="{1}";
}}if(l.length>3){if(l!=="substringof"){n="{0}({2},"+n+")";
}else{n="{0}("+n+",{2})";
if(s==="doesnotcontain"){if(v){n="{0}({2},'{1}') eq -1";
l="indexof";
}else{n+=" eq false";
}}}}else{n="{2} {0} "+n;
}l=d.format(n,l,w,k);
}}t.push(l);
}l=t.join(" "+r+" ");
if(t.length>1){l="("+l+")";
}return l;
}function h(l){for(var k in l){if(k.indexOf("@odata")===0){delete l[k];
}}}c(true,d.data,{schemas:{odata:{type:"json",data:function(k){return k.d.results||[k.d];
},total:"d.__count"}},transports:{odata:{read:{cache:true,dataType:"jsonp",jsonp:"$callback"},update:{cache:true,dataType:"json",contentType:"application/json",type:"PUT"},create:{cache:true,dataType:"json",contentType:"application/json",type:"POST"},destroy:{cache:true,dataType:"json",type:"DELETE"},parameterMap:function(m,o,p){var n,q,l,k;
m=m||{};
o=o||"read";
k=(this.options||b)[o];
k=k?k.dataType:"json";
if(o==="read"){n={$inlinecount:"allpages"};
if(k!="json"){n.$format="json";
}for(l in m){if(e[l]){e[l](n,m[l],p);
}else{n[l]=m[l];
}}}else{if(k!=="json"){throw new Error("Only json dataType can be used for "+o+" operation.");
}if(o!=="destroy"){for(l in m){q=m[l];
if(typeof q==="number"){m[l]=q+"";
}}n=d.stringify(m);
}}return n;
}}}});
c(true,d.data,{schemas:{"odata-v4":{type:"json",data:function(k){k=a.extend({},k);
h(k);
if(k.value){return k.value;
}return[k];
},total:function(k){return k["@odata.count"];
}}},transports:{"odata-v4":{read:{cache:true,dataType:"json"},update:{cache:true,dataType:"json",contentType:"application/json;IEEE754Compatible=true",type:"PUT"},create:{cache:true,dataType:"json",contentType:"application/json;IEEE754Compatible=true",type:"POST"},destroy:{cache:true,dataType:"json",type:"DELETE"},parameterMap:function(k,m){var l=d.data.transports.odata.parameterMap(k,m,true);
if(m=="read"){l.$count=true;
delete l.$inlinecount;
}return l;
}}}});
})(window.kendo.jQuery);
return window.kendo;
},typeof define=="function"&&define.amd?define:function(a,b){b();
});
