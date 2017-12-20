(function(b,a){a(["./kendo.dataviz.chart","./kendo.drawing"],b);
})(function(){(function(a,aB){var G=Math,A=window.kendo,r=A.deepExtend,aC=A.util,c=aC.append,v=A.drawing,x=A.geometry,q=A.dataviz,e=q.AreaSegment,f=q.Axis,g=q.AxisGroupRangeTracker,h=q.BarChart,j=q.Box2D,m=q.CategoryAxis,k=q.CategoricalChart,l=q.CategoricalPlotArea,n=q.ChartElement,p=q.CurveProcessor,u=q.DonutSegment,C=q.LineChart,D=q.LineSegment,F=q.LogarithmicAxis,H=q.NumericAxis,J=q.PlotAreaBase,K=q.PlotAreaFactory,L=q.Point2D,ap=q.Ring,ar=q.ScatterChart,at=q.ScatterLineChart,au=q.SeriesBinder,av=q.ShapeBuilder,aA=q.SplineSegment,ax=q.SplineAreaSegment,y=q.getSpacing,w=q.filterSeriesByType,B=aC.limitValue,aq=q.round;
var d="arc",i="#000",o=q.COORD_PRECISION,s=0.15,t=G.PI/180,E="log",I="plotAreaClick",M="polarArea",O="polarLine",P="polarScatter",Z="radarArea",ab="radarColumn",ac="radarLine",aw="smooth",aD="x",aF="y",aG="zero",N=[M,O,P],aa=[Z,ab,ac];
var z={createGridLines:function(X){var Y=this,aK=Y.options,aL=G.abs(Y.box.center().y-X.lineBox().y1),aI,aJ,aM=false,aH=[];
if(aK.majorGridLines.visible){aI=Y.majorGridLineAngles(X);
aM=true;
aH=Y.renderGridLines(aI,aL,aK.majorGridLines);
}if(aK.minorGridLines.visible){aJ=Y.minorGridLineAngles(X,aM);
c(aH,Y.renderGridLines(aJ,aL,aK.minorGridLines));
}return aH;
},renderGridLines:function(X,aM,aL){var aN={stroke:{width:aL.width,color:aL.color,dashType:aL.dashType}};
var Y=this.box.center();
var aH=new x.Circle([Y.x,Y.y],aM);
var aI=this.gridLinesVisual();
for(var aJ=0;
aJ<X.length;
aJ++){var aK=new v.Path(aN);
aK.moveTo(aH.center).lineTo(aH.pointAt(X[aJ]));
aI.append(aK);
}return aI.children;
},gridLineAngles:function(X,aJ,aI){var Y=this,aH=Y.intervals(aJ,aI);
return a.map(aH,function(aL){var aK=Y.intervalAngle(aL);
if(!X.options.visible||aK!==90){return aK;
}});
}};
var ag=m.extend({options:{startAngle:90,labels:{margin:y(10)},majorGridLines:{visible:true},justified:true},range:function(){return{min:0,max:this.options.categories.length};
},reflow:function(X){this.box=X;
this.reflowLabels();
},lineBox:function(){return this.box;
},reflowLabels:function(){var X=this,aJ=new j(),aI=X.labels,aH,Y;
for(Y=0;
Y<aI.length;
Y++){aI[Y].reflow(aJ);
aH=aI[Y].box;
aI[Y].reflow(X.getSlot(Y).adjacentBox(0,aH.width(),aH.height()));
}},intervals:function(aP,aO){var Y=this,aM=Y.options,aH=aM.categories.length,X=0,aN=0,aJ=aH/aP||1,aI=360/aJ,aK=[],aL;
if(aO){aN=360/(aH/aO);
}for(aL=0;
aL<aJ;
aL++){X=aq(X,o);
if(X%aN!==0){aK.push(X%360);
}if(aM.reverse){X=360+X-aI;
}else{X+=aI;
}}return aK;
},majorIntervals:function(){return this.intervals(1);
},minorIntervals:function(){return this.intervals(0.5);
},intervalAngle:function(X){return(360+X+this.options.startAngle)%360;
},majorAngles:function(){return a.map(this.majorIntervals(),a.proxy(this.intervalAngle,this));
},createLine:function(){return[];
},majorGridLineAngles:function(X){return this.gridLineAngles(X,1);
},minorGridLineAngles:function(X,Y){return this.gridLineAngles(X,0.5,Y?1:0);
},createPlotBands:function(){var X=this,aK=X.options,aL=aK.plotBands||[],aJ,Y,aO,aN,aI,aP;
var aH=this._plotbandGroup=new v.Group({zIndex:-1});
for(aJ=0;
aJ<aL.length;
aJ++){Y=aL[aJ];
aO=X.plotBandSlot(Y);
aN=X.getSlot(Y.from);
aI=Y.from-G.floor(Y.from);
aO.startAngle+=aI*aN.angle;
aP=G.ceil(Y.to)-Y.to;
aO.angle-=(aP+aI)*aN.angle;
var aM=av.current.createRing(aO,{fill:{color:Y.color,opacity:Y.opacity},stroke:{opacity:Y.opacity}});
aH.append(aM);
}X.appendVisual(aH);
},plotBandSlot:function(X){return this.getSlot(X.from,X.to-1);
},getSlot:function(aJ,aP){var Y=this,aL=Y.options,aK=aL.justified,aH=Y.box,aI=Y.majorAngles(),aQ=aI.length,aN,aM=360/aQ,aO,X;
if(aL.reverse&&!aK){aJ=(aJ+1)%aQ;
}aJ=B(G.floor(aJ),0,aQ-1);
aO=aI[aJ];
if(aK){aO=aO-aM/2;
if(aO<0){aO+=360;
}}aP=B(G.ceil(aP||aJ),aJ,aQ-1);
aN=aP-aJ+1;
X=aM*aN;
return new ap(aH.center(),0,aH.height()/2,aO,X);
},slot:function(Y,aK){var aH=this.options;
var aI=this.getSlot(Y,aK);
var aJ=aI.startAngle+180;
var X=aJ+aI.angle;
return new x.Arc([aI.c.x,aI.c.y],{startAngle:aJ,endAngle:X,radiusX:aI.r,radiusY:aI.r});
},pointCategoryIndex:function(aJ){var X=this,aH=null,Y,aI=X.options.categories.length,aK;
for(Y=0;
Y<aI;
Y++){aK=X.getSlot(Y);
if(aK.containsPoint(aJ)){aH=Y;
break;
}}return aH;
}});
r(ag.fn,z);
var al={options:{majorGridLines:{visible:true}},createPlotBands:function(){var Y=this,aN=Y.options,aO=aN.plotBands||[],aS=aN.majorGridLines.type,X=Y.plotArea.polarAxis,aM=X.majorAngles(),aJ=X.box.center(),aL,aH,aI,aR,aP;
var aK=this._plotbandGroup=new v.Group({zIndex:-1});
for(aL=0;
aL<aO.length;
aL++){aH=aO[aL];
aI={fill:{color:aH.color,opacity:aH.opacity},stroke:{opacity:aH.opacity}};
aR=Y.getSlot(aH.from,aH.to,true);
aP=new ap(aJ,aJ.y-aR.y2,aJ.y-aR.y1,0,360);
var aQ;
if(aS===d){aQ=av.current.createRing(aP,aI);
}else{aQ=v.Path.fromPoints(Y.plotBandPoints(aP,aM),aI).close();
}aK.append(aQ);
}Y.appendVisual(aK);
},plotBandPoints:function(aM,X){var aJ=[],aL=[];
var Y=[aM.c.x,aM.c.y];
var aI=new x.Circle(Y,aM.ir);
var aK=new x.Circle(Y,aM.r);
for(var aH=0;
aH<X.length;
aH++){aJ.push(aI.pointAt(X[aH]));
aL.push(aK.pointAt(X[aH]));
}aJ.reverse();
aJ.push(aJ[0]);
aL.push(aL[0]);
return aL.concat(aJ);
},createGridLines:function(X){var Y=this,aM=Y.options,aK=Y.radarMajorGridLinePositions(),aJ=X.majorAngles(),aL,aH=X.box.center(),aI=[];
if(aM.majorGridLines.visible){aI=Y.renderGridLines(aH,aK,aJ,aM.majorGridLines);
}if(aM.minorGridLines.visible){aL=Y.radarMinorGridLinePositions();
c(aI,Y.renderGridLines(aH,aL,aJ,aM.minorGridLines));
}return aI;
},renderGridLines:function(aI,aQ,Y,aM){var aH=this,aP,aO,X;
var aN={stroke:{width:aM.width,color:aM.color,dashType:aM.dashType}};
var aK=this.gridLinesVisual();
for(aO=0;
aO<aQ.length;
aO++){aP=aI.y-aQ[aO];
if(aP>0){var aJ=new x.Circle([aI.x,aI.y],aP);
if(aM.type===d){aK.append(new v.Circle(aJ,aN));
}else{var aL=new v.Path(aN);
for(X=0;
X<Y.length;
X++){aL.lineTo(aJ.pointAt(Y[X]));
}aL.close();
aK.append(aL);
}}}return aK.children;
},getValue:function(aS){var aH=this,aR=aH.options,aO=aH.lineBox(),Y=aH.plotArea.polarAxis,aP=Y.majorAngles(),aJ=Y.box.center(),aT=aS.distanceTo(aJ),aK=aT;
if(aR.majorGridLines.type!==d&&aP.length>1){var aL=aS.x-aJ.x,aM=aS.y-aJ.y,aU=(G.atan2(aM,aL)/t+540)%360;
aP.sort(function(aV,aW){return b(aV,aU)-b(aW,aU);
});
var aQ=b(aP[0],aP[1])/2,X=b(aU,aP[0]),aN=90-aQ,aI=180-X-aN;
aK=aT*(G.sin(aI*t)/G.sin(aN*t));
}return aH.axisType().fn.getValue.call(aH,new L(aO.x1,aO.y2-aK));
}};
var ak=H.extend({radarMajorGridLinePositions:function(){return this.getTickPositions(this.options.majorUnit);
},radarMinorGridLinePositions:function(){var X=this,aH=X.options,Y=0;
if(aH.majorGridLines.visible){Y=aH.majorUnit;
}return X.getTickPositions(aH.minorUnit,Y);
},axisType:function(){return H;
}});
r(ak.fn,al);
var aj=F.extend({radarMajorGridLinePositions:function(){var X=this,Y=[];
X.traverseMajorTicksPositions(function(aH){Y.push(aH);
},X.options.majorGridLines);
return Y;
},radarMinorGridLinePositions:function(){var X=this,Y=[];
X.traverseMinorTicksPositions(function(aH){Y.push(aH);
},X.options.minorGridLines);
return Y;
},axisType:function(){return F;
}});
r(aj.fn,al);
var S=f.extend({init:function(Y){var X=this;
f.fn.init.call(X,Y);
Y=X.options;
Y.minorUnit=Y.minorUnit||X.options.majorUnit/2;
},options:{type:"polar",startAngle:0,reverse:false,majorUnit:60,min:0,max:360,labels:{margin:y(10)},majorGridLines:{color:i,visible:true,width:1},minorGridLines:{color:"#aaa"}},getDivisions:function(X){return H.fn.getDivisions.call(this,X)-1;
},reflow:function(X){this.box=X;
this.reflowLabels();
},reflowLabels:function(){var X=this,aK=new j(),Y=X.majorIntervals(),aJ=X.labels,aI,aH;
for(aH=0;
aH<aJ.length;
aH++){aJ[aH].reflow(aK);
aI=aJ[aH].box;
aJ[aH].reflow(X.getSlot(Y[aH]).adjacentBox(0,aI.width(),aI.height()));
}},lineBox:function(){return this.box;
},intervals:function(aM,aL){var Y=this,aK=Y.options,aH=Y.getDivisions(aM),X=aK.min,aI=[],aJ;
if(aL){aL=aL/aM;
}for(aJ=0;
aJ<aH;
aJ++){if(aJ%aL!==0){aI.push((360+X)%360);
}X+=aM;
}return aI;
},majorIntervals:function(){return this.intervals(this.options.majorUnit);
},minorIntervals:function(){return this.intervals(this.options.minorUnit);
},intervalAngle:function(X){return(360+X-this.options.startAngle)%360;
},majorAngles:ag.fn.majorAngles,createLine:function(){return[];
},majorGridLineAngles:function(X){return this.gridLineAngles(X,this.options.majorUnit);
},minorGridLineAngles:function(X,Y){return this.gridLineAngles(X,this.options.minorUnit,Y?this.options.majorUnit:0);
},createPlotBands:ag.fn.createPlotBands,plotBandSlot:function(X){return this.getSlot(X.from,X.to);
},getSlot:function(X,aH){var Y=this,aJ=Y.options,aK=aJ.startAngle,aI=Y.box,aL;
X=B(X,aJ.min,aJ.max);
aH=B(aH||X,X,aJ.max);
if(aJ.reverse){X*=-1;
aH*=-1;
}X=(540-X-aK)%360;
aH=(540-aH-aK)%360;
if(aH<X){aL=X;
X=aH;
aH=aL;
}return new ap(aI.center(),0,aI.height()/2,X,aH-X);
},slot:function(Y,aN){var aJ=this.options;
var aL=360-aJ.startAngle;
var aK=this.getSlot(Y,aN);
var aM;
var X;
var aI;
var aH;
if(!q.util.defined(aN)){aN=Y;
}aI=G.min(Y,aN);
aH=G.max(Y,aN);
if(aJ.reverse){aM=aI;
X=aH;
}else{aM=360-aH;
X=360-aI;
}aM=(aM+aL)%360;
X=(X+aL)%360;
return new x.Arc([aK.c.x,aK.c.y],{startAngle:aM,endAngle:X,radiusX:aK.r,radiusY:aK.r});
},getValue:function(aK){var X=this,aJ=X.options,Y=X.box.center(),aH=aK.x-Y.x,aI=aK.y-Y.y,aM=G.round(G.atan2(aI,aH)/t),aL=aJ.startAngle;
if(!aJ.reverse){aM*=-1;
aL*=-1;
}return(aM+aL+360)%360;
},range:H.fn.range,labelsCount:H.fn.labelsCount,createAxisLabel:H.fn.createAxisLabel});
r(S.fn,z);
var ah=n.extend({options:{gap:1,spacing:0},reflow:function(aM){var aH=this,aL=aH.options,Y=aH.children,aJ=aL.gap,aQ=aL.spacing,aI=Y.length,aO=aI+aJ+(aQ*(aI-1)),aN=aM.angle/aO,aP,X=aM.startAngle+aN*(aJ/2),aK;
for(aK=0;
aK<aI;
aK++){aP=aM.clone();
aP.startAngle=X;
aP.angle=aN;
if(Y[aK].sector){aP.r=Y[aK].sector.r;
}Y[aK].reflow(aP);
Y[aK].sector=aP;
X+=aN+(aN*aQ);
}}});
var ao=n.extend({reflow:function(aL){var aM=this,aK=aM.options.isReversed,X=aM.children,Y=X.length,aH,aJ,aI=aK?Y-1:0,aN=aK?-1:1;
aM.box=new j();
for(aJ=aI;
aJ>=0&&aJ<Y;
aJ+=aN){aH=X[aJ].sector;
aH.startAngle=aL.startAngle;
aH.angle=aL.angle;
}}});
var an=u.extend({init:function(Y,X){u.fn.init.call(this,Y,null,X);
},options:{overlay:{gradient:null},labels:{distance:10}}});
var af=h.extend({pointType:function(){return an;
},clusterType:function(){return ah;
},stackType:function(){return ao;
},categorySlot:function(X,Y){return X.getSlot(Y);
},pointSlot:function(X,aH){var Y=X.clone(),aI=X.c.y;
Y.r=aI-aH.y1;
Y.ir=aI-aH.y2;
return Y;
},reflow:k.fn.reflow,reflowPoint:function(X,Y){X.sector=Y;
X.reflow();
},options:{clip:false,animation:{type:"pie"}},createAnimation:function(){this.options.animation.center=this.box.toRect().center();
h.fn.createAnimation.call(this);
}});
var ai=C.extend({options:{clip:false},pointSlot:function(X,aI){var aH=X.c.y-aI.y1,Y=L.onCircle(X.c,X.middle(),aH);
return new j(Y.x,Y.y,Y.x,Y.y);
},createSegment:function(Y,X,aJ){var aI,aH,aK=X.style;
if(aK==aw){aH=aA;
}else{aH=D;
}aI=new aH(Y,X,aJ);
if(Y.length===X.data.length){aI.options.closed=true;
}return aI;
}});
var ae=e.extend({points:function(){return D.fn.points.call(this,this.stackPoints);
}});
var az=ax.extend({areaPoints:function(){return[];
}});
var ad=ai.extend({createSegment:function(aI,Y,aM,aK){var X=this,aJ=X.options,aH=aJ.isStacked,aN,aL,aO=(Y.line||{}).style;
if(aO===aw){aL=new az(aI,aK,aH,Y,aM);
aL.options.closed=true;
}else{if(aH&&aM>0&&aK){aN=aK.linePoints.slice(0).reverse();
}aI.push(aI[0]);
aL=new ae(aI,aN,Y,aM);
}return aL;
},seriesMissingValues:function(X){return X.missingValues||aG;
}});
var W=ar.extend({pointSlot:function(Y,aH){var aI=Y.c.y-aH.y1,X=L.onCircle(Y.c,Y.startAngle,aI);
return new j(X.x,X.y,X.x,X.y);
},options:{clip:false}});
var T=at.extend({pointSlot:W.fn.pointSlot,options:{clip:false}});
var R=e.extend({points:function(){var aK=this,Y=aK.parent,aH=Y.plotArea,aJ=aH.polarAxis,X=aJ.box.center(),aL=aK.stackPoints,aI=D.fn.points.call(aK,aL);
aI.unshift([X.x,X.y]);
aI.push([X.x,X.y]);
return aI;
}});
var ay=ax.extend({areaPoints:function(){var aJ=this,Y=aJ.parent,aH=Y.plotArea,aI=aH.polarAxis,X=aI.box.center();
return[X];
},points:function(){var aM=this,Y=aM.parent,aK=Y.plotArea,aL=aK.polarAxis,X=aL.box.center(),aH,aI=new p(false),aJ=D.fn.points.call(this);
aJ.push(X);
aH=aI.process(aJ);
aH.splice(aH.length-3,aH.length-1);
aM.curvePoints=aH;
return aH;
}});
var Q=T.extend({createSegment:function(Y,X,aI){var aH,aJ=(X.line||{}).style;
if(aJ==aw){aH=new ay(Y,null,false,X,aI);
}else{aH=new R(Y,[],X,aI);
}return aH;
},seriesMissingValues:function(X){return X.missingValues||aG;
},sortPoints:function(X){return X.sort(aE);
}});
var V=J.extend({init:function(aH,X){var Y=this;
Y.valueAxisRangeTracker=new g();
J.fn.init.call(Y,aH,X);
},render:function(){var X=this;
X.addToLegend(X.series);
X.createPolarAxis();
X.createCharts();
X.createValueAxis();
},createValueAxis:function(){var aJ=this,aL=aJ.valueAxisRangeTracker,aI=aL.query(),aK,aM,Y=aJ.valueAxisOptions({roundToMajorUnit:false,zIndex:-1}),aH,X;
if(Y.type===E){aH=aj;
X={min:0.1,max:1};
}else{aH=ak;
X={min:0,max:1};
}aK=aL.query(name)||aI||X;
if(aK&&aI){aK.min=G.min(aK.min,aI.min);
aK.max=G.max(aK.max,aI.max);
}aM=new aH(aK.min,aK.max,Y);
aJ.valueAxis=aM;
aJ.appendAxis(aM);
},reflowAxes:function(){var aL=this,aJ=aL.options.plotArea,aN=aL.valueAxis,aM=aL.polarAxis,Y=aL.box,aH=G.min(Y.width(),Y.height())*s,aK=y(aJ.padding||{},aH),X=Y.clone().unpad(aK),aO=X.clone().shrink(0,X.height()/2);
aM.reflow(X);
aN.reflow(aO);
var aI=aN.lineBox().height()-aN.box.height();
aN.reflow(aN.box.unpad({top:aI}));
aL.axisBox=X;
aL.alignAxes(X);
},alignAxes:function(){var aH=this,aK=aH.valueAxis,aI=aK.getSlot(aK.options.min),aJ=aK.options.reverse?2:1,Y=aH.polarAxis.getSlot(0).c,X=aK.box.translate(Y.x-aI[aD+aJ],Y.y-aI[aF+aJ]);
aK.reflow(X);
},backgroundBox:function(){return this.box;
}});
var am=V.extend({options:{categoryAxis:{categories:[]},valueAxis:{}},createPolarAxis:function(){var Y=this,X;
X=new ag(Y.options.categoryAxis);
Y.polarAxis=X;
Y.categoryAxis=X;
Y.appendAxis(X);
},valueAxisOptions:function(X){var Y=this;
if(Y._hasBarCharts){r(X,{majorGridLines:{type:d},minorGridLines:{type:d}});
}if(Y._isStacked100){r(X,{roundToMajorUnit:false,labels:{format:"P0"}});
}return r(X,Y.options.valueAxis);
},appendChart:l.fn.appendChart,createCharts:function(){var Y=this,aH=Y.filterVisibleSeries(Y.series),X=Y.panes[0];
Y.createAreaChart(w(aH,[Z]),X);
Y.createLineChart(w(aH,[ac]),X);
Y.createBarChart(w(aH,[ab]),X);
},chartOptions:function(aI){var aH={series:aI};
var Y=aI[0];
if(Y){var X=this.filterVisibleSeries(aI);
var aJ=Y.stack;
aH.isStacked=aJ&&X.length>1;
aH.isStacked100=aJ&&aJ.type==="100%"&&X.length>1;
if(aH.isStacked100){this._isStacked100=true;
}}return aH;
},createAreaChart:function(aH,Y){if(aH.length===0){return;
}var X=new ad(this,this.chartOptions(aH));
this.appendChart(X,Y);
},createLineChart:function(aH,Y){if(aH.length===0){return;
}var X=new ai(this,this.chartOptions(aH));
this.appendChart(X,Y);
},createBarChart:function(aJ,aI){if(aJ.length===0){return;
}var Y=aJ[0];
var aH=this.chartOptions(aJ);
aH.gap=Y.gap;
aH.spacing=Y.spacing;
var X=new af(this,aH);
this.appendChart(X,aI);
this._hasBarCharts=true;
},seriesCategoryAxis:function(){return this.categoryAxis;
},click:function(Y,aI){var aJ=this,aH=Y._eventCoordinates(aI),aK=new L(aH.x,aH.y),X,aL;
X=aJ.categoryAxis.getCategory(aK);
aL=aJ.valueAxis.getValue(aK);
if(X!==null&&aL!==null){Y.trigger(I,{element:a(aI.target),category:X,value:aL});
}},createCrosshairs:a.noop});
var U=V.extend({options:{xAxis:{},yAxis:{}},createPolarAxis:function(){var X=this,Y;
Y=new S(X.options.xAxis);
X.polarAxis=Y;
X.axisX=Y;
X.appendAxis(Y);
},valueAxisOptions:function(X){var Y=this;
return r(X,{majorGridLines:{type:d},minorGridLines:{type:d}},Y.options.yAxis);
},createValueAxis:function(){var X=this;
V.fn.createValueAxis.call(X);
X.axisY=X.valueAxis;
},appendChart:function(X,Y){var aH=this;
aH.valueAxisRangeTracker.update(X.yAxisRanges);
J.fn.appendChart.call(aH,X,Y);
},createCharts:function(){var Y=this,aH=Y.filterVisibleSeries(Y.series),X=Y.panes[0];
Y.createLineChart(w(aH,[O]),X);
Y.createScatterChart(w(aH,[P]),X);
Y.createAreaChart(w(aH,[M]),X);
},createLineChart:function(aI,Y){if(aI.length===0){return;
}var aH=this,X=new T(aH,{series:aI});
aH.appendChart(X,Y);
},createScatterChart:function(aI,X){if(aI.length===0){return;
}var Y=this,aH=new W(Y,{series:aI});
Y.appendChart(aH,X);
},createAreaChart:function(aI,Y){if(aI.length===0){return;
}var aH=this,X=new Q(aH,{series:aI});
aH.appendChart(X,Y);
},click:function(X,aH){var aI=this,Y=X._eventCoordinates(aH),aJ=new L(Y.x,Y.y),aK,aL;
aK=aI.axisX.getValue(aJ);
aL=aI.axisY.getValue(aJ);
if(aK!==null&&aL!==null){X.trigger(I,{element:a(aH.target),x:aK,y:aL});
}},createCrosshairs:a.noop});
function aE(X,Y){return X.value.x-Y.value.x;
}function b(X,Y){return 180-G.abs(G.abs(X-Y)-180);
}K.current.register(U,N);
K.current.register(am,aa);
au.current.register(N,[aD,aF],["color"]);
au.current.register(aa,["value"],["color"]);
q.DefaultAggregates.current.register(aa,{value:"max",color:"first"});
r(q,{PolarAreaChart:Q,PolarAxis:S,PolarLineChart:T,PolarPlotArea:U,RadarAreaChart:ad,RadarBarChart:af,RadarCategoryAxis:ag,RadarClusterLayout:ah,RadarLineChart:ai,RadarNumericAxis:ak,RadarPlotArea:am,SplinePolarAreaSegment:ay,SplineRadarAreaSegment:az,RadarStackLayout:ao});
})(window.kendo.jQuery);
return window.kendo;
},typeof define=="function"&&define.amd?define:function(a,b){b();
});