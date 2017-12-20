/*
	   _   ___  _____   _   ____
	  (_) / _ \ \_   \ / | | ___|
	  | |/ /_\/  / /\/ | | |___ \
	  | / /_\\/\/ /_   | |_ ___) |
	 _/ \____/\____/   |_(_)____/
	|__/
	jsGanttImproved 1.5
	Copyright (c) 2013-2014, Paul Geldart All rights reserved.

	The current version of this code can be found at https://code.google.com/p/jsgantt-improved/

	***IMPORTANT***
	As written, there is no protection from XSS as input data is coming from known
	good, hard coded sources. If you are accepting data for display from untrusted
	sources you must sanitise it before use.
	***************

	* Copyright (c) 2013-2014, Paul Geldart.
	* All rights reserved.
	*
	* Redistribution and use in source and binary forms, with or without
	* modification, are permitted provided that the following conditions are met:
	*     * Redistributions of source code must retain the above copyright
	*       notice, this list of conditions and the following disclaimer.
	*     * Redistributions in binary form must reproduce the above copyright
	*       notice, this list of conditions and the following disclaimer in the
	*       documentation and/or other materials provided with the distribution.
	*     * Neither the name of Paul Geldart nor the names of its contributors
	*       may be used to endorse or promote products derived from this software
	*       without specific prior written permission.
	*
	* THIS SOFTWARE IS PROVIDED BY PAUL GELDART. ''AS IS'' AND ANY EXPRESS OR
	* IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
	* OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
	* IN NO EVENT SHALL SHLOMY GANTZ/BLUEBRICK INC. BE LIABLE FOR ANY DIRECT,
	* INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
	* (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
	* LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
	* ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
	* (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
	* SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

	This project is based on jsGantt 1.2, (which can be obtained from
	https://code.google.com/p/jsgantt/ ) and remains under the original BSD license.
	The original project license follows:

	Copyright (c) 2009, Shlomy Gantz BlueBrick Inc. All rights reserved.

	* Redistribution and use in source and binary forms, with or without
	* modification, are permitted provided that the following conditions are met:
	*     * Redistributions of source code must retain the above copyright
	*       notice, this list of conditions and the following disclaimer.
	*     * Redistributions in binary form must reproduce the above copyright
	*       notice, this list of conditions and the following disclaimer in the
	*       documentation and/or other materials provided with the distribution.
	*     * Neither the name of Shlomy Gantz or BlueBrick Inc. nor the
	*       names of its contributors may be used to endorse or promote products
	*       derived from this software without specific prior written permission.
	*
	* THIS SOFTWARE IS PROVIDED BY SHLOMY GANTZ/BLUEBRICK INC. ''AS IS'' AND ANY
	* EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
	* WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
	* DISCLAIMED. IN NO EVENT SHALL SHLOMY GANTZ/BLUEBRICK INC. BE LIABLE FOR ANY
	* DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
	* (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
	* LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
	* ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
	* (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
	* SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

var JSGantt; if (!JSGantt) JSGantt = {};

var vTimeout = 0;
var vBenchTime = new Date().getTime();

JSGantt.isIE = function ()
{
	if(typeof document.all != 'undefined')
	{
		if ('pageXOffset' in window) return false;	// give IE9 and above the benefit of the doubt!
		else return true;
	}
	else return false;
}

JSGantt.TaskItem = function(pID, pName, pStart, pEnd, pClass, pLink, pMile, pRes, pComp, pGroup, pParent, pOpen, pDepend, pCaption, pNotes)
{

	var vID = parseInt(document.createTextNode(pID).data);
	var vName = document.createTextNode(pName).data;
	var vStart = new Date(0);
	var vEnd = new Date(0);
	var vClass = document.createTextNode(pClass).data;
	var vLink = document.createTextNode(pLink).data;
	var vMile = parseInt(document.createTextNode(pMile).data);
	var vRes = document.createTextNode(pRes).data;
	var vComp = parseInt(document.createTextNode(pComp).data);
	var vGroup = parseInt(document.createTextNode(pGroup).data);
	var vParent = document.createTextNode(pParent).data;
	var vOpen = (vGroup==2)?1:parseInt(document.createTextNode(pOpen).data);
	var vDepend = new Array();
	var vDependType = new Array();
	var vCaption = document.createTextNode(pCaption).data;
	var vDuration = '';
	var vLevel = 0;
	var vNumKid = 0;
	var vVisible = 1;
	var vSortIdx = 0;
	var vToDelete = false;
	var x1, y1, x2, y2;
	var vNotes;
	var vParItem = null;
	var vCellDiv = null;

	if ( pNotes != null )
	{
		vNotes = document.createElement('span');
		vNotes.className='gTaskNotes';
		vNotes.innerHTML = pNotes;
		JSGantt.stripUnwanted(vNotes);
	}

	if (vGroup != 1)
	{
		vStart = JSGantt.parseDateStr(document.createTextNode(pStart).data,g.getDateInputFormat());
		vEnd   = JSGantt.parseDateStr(document.createTextNode(pEnd).data,g.getDateInputFormat());
	}

	if ( pDepend != null )
	{
		var vDependStr = pDepend + '';
		var vDepList = vDependStr.split(',');
		var n = vDepList.length;

		for(var k=0;k<n;k++)
		{
			if(vDepList[k].toUpperCase().indexOf('SS')!=-1)
			{
				vDepend[k]=vDepList[k].substring(0,vDepList[k].toUpperCase().indexOf('SS'));
				vDependType[k]='SS';
			}
			else if(vDepList[k].toUpperCase().indexOf('FF')!=-1)
			{
				vDepend[k]=vDepList[k].substring(0,vDepList[k].toUpperCase().indexOf('FF'));
				vDependType[k]='FF';
			}
			else if(vDepList[k].toUpperCase().indexOf('SF')!=-1)
			{
				vDepend[k]=vDepList[k].substring(0,vDepList[k].toUpperCase().indexOf('SF'));
				vDependType[k]='SF';
			}
			else if(vDepList[k].toUpperCase().indexOf('FS')!=-1)
			{
				vDepend[k]=vDepList[k].substring(0,vDepList[k].toUpperCase().indexOf('FS'));
				vDependType[k]='FS';
			}
			else
			{
				vDepend[k]=vDepList[k];
				vDependType[k]='FS';
			}
		}
	}

	this.getID		= function(){ return vID };
	this.getName	= function(){ return vName };
	this.getStart	= function(){ return vStart};
	this.getEnd		= function(){ return vEnd };
	this.getClass	= function(){ return vClass};
	this.getLink	= function(){ return vLink };
	this.getMile	= function(){ return vMile };
	this.getDepend	= function(){ if(vDepend) return vDepend; else return null };
	this.getDepType	= function(){ if(vDependType) return vDependType; else return null };
	this.getCaption	= function(){ if(vCaption) return vCaption; else return ''; };
	this.getResource	= function(){ if(vRes) return vRes; else return '\u00A0'; };
	this.getCompVal	= function(){ if(vComp) return vComp; else return 0; };
	this.getCompStr	= function(){ if(vComp) return vComp+'%'; else return ''; };
	this.getNotes	= function(){ return vNotes; };
	this.getSortIdx	= function(){ return vSortIdx; };
	this.getToDelete	= function(){ return vToDelete; };

	this.getDuration = function(pFormat, pLang)
	{
		if (vMile)
		{
			vDuration = '-';
		}
		else
		{
			var vTaskEnd=new Date(this.getEnd().getTime())
			var vUnits=null;
			switch( pFormat )
			{
				case 'week':  vUnits='day'; break;
				case 'month':  vUnits='week'; break;
				case 'quarter': vUnits='month'; break;
				default: vUnits=pFormat; break;
			}

			if ((vTaskEnd.getTime()-(vTaskEnd.getTimezoneOffset()*60000)) % (86400000) == 0)
			{
				vTaskEnd = new Date(vTaskEnd.getFullYear(), vTaskEnd.getMonth(), vTaskEnd.getDate()+1, vTaskEnd.getHours(), vTaskEnd.getMinutes(), vTaskEnd.getSeconds());
			}
			tmpPer = (JSGantt.getOffset(this.getStart(), vTaskEnd, 999, vUnits))/1000;
			if(Math.floor(tmpPer) != tmpPer) tmpPer=Math.round(tmpPer*10)/10;
			switch( vUnits )
			{
				case 'hour': vDuration=tmpPer+' '+((tmpPer != 1)?pLang['hrs']:pLang['hr']); break;
				case 'day': vDuration=tmpPer+' '+((tmpPer != 1)?pLang['dys']:pLang['dy']); break;
				case 'week': vDuration=tmpPer+' '+((tmpPer != 1)?pLang['wks']:pLang['wk']); break;
				case 'month': vDuration=tmpPer+' '+((tmpPer != 1)?pLang['mths']:pLang['mth']); break;
				case 'quarter': vDuration=tmpPer+' '+((tmpPer != 1)?pLang['qtrs']:pLang['qtr']); break;
			}
		}
		return( vDuration )
	};

	this.getParent	= function(){ return vParent };
	this.getGroup	= function(){ return vGroup };
	this.getOpen	= function(){ return vOpen };
	this.getLevel	= function(){ return vLevel };
	this.getNumKids	= function(){ return vNumKid };
	this.getStartX	= function(){ return x1 };
	this.getStartY	= function(){ return y1 };
	this.getEndX	= function(){ return x2 };
	this.getEndY	= function(){ return y2 };
	this.getVisible	= function(){ return vVisible };
	this.getParItem	= function(){ return vParItem };
	this.getCellDiv	= function(){ return vCellDiv };
	this.setDepend	= function(pDepend){ vDepend = pDepend;};
	this.setStart	= function(pStart){ vStart = pStart;};
	this.setEnd		= function(pEnd){ vEnd = pEnd; };
	this.setLevel	= function(pLevel){ vLevel = pLevel;};
	this.setNumKid	= function(pNumKid){ vNumKid = pNumKid;};
	this.setCompVal	= function(pCompVal){ vComp = pCompVal;};
	this.setStartX	= function(pX){ x1 = pX; };
	this.setStartY	= function(pY){ y1 = pY; };
	this.setEndX	= function(pX){ x2 = pX; };
	this.setEndY	= function(pY){ y2 = pY; };
	this.setOpen	= function(pOpen){ vOpen = pOpen; };
	this.setVisible	= function(pVisible){ vVisible = pVisible; };
	this.setSortIdx	= function(pSortIdx){ vSortIdx = pSortIdx; };
	this.setToDelete	= function(pToDelete){ vToDelete = pToDelete; };
	this.setParItem	= function(pParItem){ vParItem = pParItem };
	this.setCellDiv	= function(pCellDiv){ vCellDiv = pCellDiv };
	this.setGroup	= function(pGroup){ vGroup = pGroup };
}

// function that loads the main gantt chart properties and functions
// pDiv: (required) this is a div object created in HTML
// pFormat: (required) - used to indicate whether chart should be drawn in "day", "week", "month", or "quarter" format
JSGantt.GanttChart = function( pDiv, pFormat )
{
	var vDiv	= pDiv;
	var vFormat	= pFormat;
	var vDivId	= null;
	var vUseFade	= 1;
	var vUseMove	= 1;
	var vUseRowHlt	= 1;
	var vUseToolTip	= 1;
	var vUseSort	= 1;
	var vUseSingleCell	= 25000;
	var vShowRes	= 1;
	var vShowDur	= 1;
	var vShowComp	= 1;
	var vShowStartDate	= 1;
	var vShowEndDate	= 1;
	var vShowEndWeekDate	= 1;
	var vShowTaskInfoRes	= 1;
	var vShowTaskInfoDur	= 1;
	var vShowTaskInfoComp	= 1;
	var vShowTaskInfoStartDate	= 1;
	var vShowTaskInfoEndDate	= 1;
	var vShowTaskInfoNotes	= 1;
	var vShowTaskInfoLink	= 0;
	var vShowDeps	= 1;
	var vShowSelector = new Array("top");
	var vDateInputFormat = "yyyy-mm-dd";
	var vDateTaskTableDisplayFormat = JSGantt.parseDateFormatStr("dd/mm/yyyy");
	var vDateTaskDisplayFormat = JSGantt.parseDateFormatStr("dd month yyyy");
	var vHourMajorDateDisplayFormat = JSGantt.parseDateFormatStr("day dd month yyyy");
	var vHourMinorDateDisplayFormat = JSGantt.parseDateFormatStr("HH");
	var vDayMajorDateDisplayFormat = JSGantt.parseDateFormatStr("dd/mm/yyyy");
	var vDayMinorDateDisplayFormat = JSGantt.parseDateFormatStr("dd");
	var vWeekMajorDateDisplayFormat = JSGantt.parseDateFormatStr("yyyy");
	var vWeekMinorDateDisplayFormat = JSGantt.parseDateFormatStr("dd/mm");
	var vMonthMajorDateDisplayFormat = JSGantt.parseDateFormatStr("yyyy");
	var vMonthMinorDateDisplayFormat = JSGantt.parseDateFormatStr("mon");
	var vQuarterMajorDateDisplayFormat = JSGantt.parseDateFormatStr("yyyy");
	var vQuarterMinorDateDisplayFormat = JSGantt.parseDateFormatStr("qq");
	var vUseFullYear = JSGantt.parseDateFormatStr("dd/mm/yyyy");
	var vCaptionType;
	var vDepId = 1;
	var vTaskList = new Array();
	var vFormatArr = new Array("hour","day","week","month","quarter");
	var vMonthDaysArr = new Array(31,28,31,30,31,30,31,31,30,31,30,31);
	var vProcessNeeded = true;
	var vMinGpLen = 8;
	var vScrollTo = '';
	var vHourColWidth = 18;
	var vDayColWidth = 18;
	var vWeekColWidth = 36;
	var vMonthColWidth = 36;
	var vQuarterColWidth = 18;
	var vRowHeight = 20;
	var vTodayPx = -1;
	var vLangs = { 'en':
			{'format':'Format','hour':'Hour','day':'Day','week':'Week','month':'Month','quarter':'Quarter','hours':'Hours','days':'Days',
			 'weeks':'Weeks','months':'Months','quarters':'Quarters','hr':'Hr','dy':'Day','wk':'Wk','mth':'Mth','qtr':'Qrt','hrs':'Hrs',
			 'dys':'Days','wks':'Wks','mths':'Mths','qtrs':'Qrts','resource':'Resource','duration':'Duration','comp':'% Comp.',
			 'completion':'Completion','startdate':'Start Date','enddate':'End Date','moreinfo':'More Information','notes':'Notes',
			 'january':'January','february':'February','march':'March','april':'April','maylong':'May','june':'June','july':'July',
			 'august':'August','september':'September','october':'October','november':'November','december':'December','jan':'Jan',
			 'feb':'Feb','mar':'Mar','apr':'Apr','may':'May','jun':'Jun','jul':'Jul','aug':'Aug','sep':'Sep','oct':'Oct','nov':'Nov',
			 'dec':'Dec','sunday':'Sunday','monday':'Monday','tuesday':'Tuesday','wednesday':'Wednesday','thursday':'Thursday',
			 'friday':'Friday','saturday':'Saturday','sun':'Sun','mon':'Mon','tue':'Tue','wed':'Wed','thu':'Thu','fri':'Fri','sat':'Sat'}
		};
	var vLang='en';


	this.setUseFade = function(pVal){ vUseFade = pVal; };
	this.setUseMove = function(pVal){ vUseMove = pVal; };
	this.setUseRowHlt = function(pVal){ vUseRowHlt = pVal; };
	this.setUseToolTip = function(pVal){ vUseToolTip = pVal; };
	this.setUseSort = function(pVal){ vUseSort = pVal; };
	this.setUseSingleCell = function(pVal){ vUseSingleCell = pVal*1; };
	this.setFormatArr = function()
	{
		var vValidFormats = "hour day week month quarter";
		vFormatArr = new Array();
		for(var i = 0, j = 0; i < arguments.length; i++)
		{
			if (vValidFormats.indexOf(arguments[i].toLowerCase())!=-1 && arguments[i].length>1)
			{
				vFormatArr[j++] = arguments[i].toLowerCase();
				vRegExp = new RegExp('(?:^|\s)' + arguments[i] + '(?!\S)', 'g');
				vValidFormats = vValidFormats.replace( vRegExp , '' );
			}
		}
	};
	this.setShowRes = function(pVal){ vShowRes = pVal; };
	this.setShowDur = function(pVal){ vShowDur = pVal; };
	this.setShowComp = function(pVal){ vShowComp = pVal; };
	this.setShowStartDate = function(pVal){ vShowStartDate = pVal; };
	this.setShowEndDate = function(pVal){ vShowEndDate = pVal; };
	this.setShowTaskInfoRes = function(pVal){ vShowTaskInfoRes = pVal; };
	this.setShowTaskInfoDur = function(pVal){ vShowTaskInfoDur = pVal; };
	this.setShowTaskInfoComp = function(pVal){ vShowTaskInfoComp = pVal; };
	this.setShowTaskInfoStartDate = function(pVal){ vShowTaskInfoStartDate = pVal; };
	this.setShowTaskInfoEndDate = function(pVal){ vShowTaskInfoEndDate = pVal; };
	this.setShowTaskInfoNotes = function(pVal){ vShowTaskInfoNotes = pVal; };
	this.setShowTaskInfoLink = function(pVal){ vShowTaskInfoLink = pVal; };
	this.setShowEndWeekDate = function(pVal){ vShowEndWeekDate = pVal; };
	this.setShowSelector = function()
	{
		var vValidSelectors = "top bottom";
		vShowSelector = new Array();
		for(var i = 0, j = 0; i < arguments.length; i++)
		{
			if (vValidSelectors.indexOf(arguments[i].toLowerCase())!=-1 && arguments[i].length>1)
			{
				vShowSelector[j++] = arguments[i].toLowerCase();
				vRegExp = new RegExp('(?:^|\s)' + arguments[i] + '(?!\S)', 'g');
				vValidSelectors = vValidSelectors.replace( vRegExp , '' );
			}
		}
	};
	this.setShowDeps = function(pVal){ vShowDeps = pVal };
	this.setDateInputFormat = function(pVal){ vDateInputFormat = pVal; };
	this.setDateTaskTableDisplayFormat = function(pVal){ vDateTaskTableDisplayFormat = JSGantt.parseDateFormatStr(pVal); };
	this.setDateTaskDisplayFormat = function(pVal){ vDateTaskDisplayFormat = JSGantt.parseDateFormatStr(pVal); };
	this.setHourMajorDateDisplayFormat = function(pVal){ vHourMajorDateDisplayFormat = JSGantt.parseDateFormatStr(pVal); };
	this.setHourMinorDateDisplayFormat = function(pVal){ vHourMinorDateDisplayFormat = JSGantt.parseDateFormatStr(pVal); };
	this.setDayMajorDateDisplayFormat = function(pVal){ vDayMajorDateDisplayFormat = JSGantt.parseDateFormatStr(pVal); };
	this.setDayMinorDateDisplayFormat = function(pVal){ vDayMinorDateDisplayFormat = JSGantt.parseDateFormatStr(pVal); };
	this.setWeekMajorDateDisplayFormat = function(pVal){ vWeekMajorDateDisplayFormat = JSGantt.parseDateFormatStr(pVal); };
	this.setWeekMinorDateDisplayFormat = function(pVal){ vWeekMinorDateDisplayFormat = JSGantt.parseDateFormatStr(pVal); };
	this.setMonthMajorDateDisplayFormat = function(pVal){ vMonthMajorDateDisplayFormat = JSGantt.parseDateFormatStr(pVal); };
	this.setMonthMinorDateDisplayFormat = function(pVal){ vMonthMinorDateDisplayFormat = JSGantt.parseDateFormatStr(pVal); };
	this.setQuarterMajorDateDisplayFormat = function(pVal){ vQuarterMajorDateDisplayFormat = JSGantt.parseDateFormatStr(pVal); };
	this.setQuarterMinorDateDisplayFormat = function(pVal){ vQuarterMinorDateDisplayFormat = JSGantt.parseDateFormatStr(pVal); };
	this.setCaptionType = function(pType){ vCaptionType = pType };
	this.setFormat = function(pFormat)
	{
		vFormat = pFormat;
		this.Draw();
	};
	this.setMinGpLen = function(pMinGpLen){ vMinGpLen = pMinGpLen; };
	this.setScrollTo = function(pDate){ vScrollTo = pDate };
	this.setHourColWidth = function(pWidth){ vHourColWidth = pWidth };
	this.setDayColWidth = function(pWidth){ vDayColWidth = pWidth };
	this.setWeekColWidth = function(pWidth){ vWeekColWidth = pWidth };
	this.setMonthColWidth = function(pWidth){ vMonthColWidth = pWidth };
	this.setQuarterColWidth = function(pWidth){ vQuarterColWidth = pWidth };
	this.setRowHeight = function(pHeight){ vRowHeight = pHeight };
	this.setLang = function(pLang){ if(vLangs[pLang])vLang = pLang };
	this.addLang = function(pLang, pVals){
		if(!vLangs[pLang])
		{
			vLangs[pLang] = new Object();
			for(var vKey in vLangs['en'])vLangs[pLang][vKey]=(pVals[vKey])?pVals[vKey]:vLangs['en'][vKey];
		}
	};

	this.getDivId = function(){ return vDivId };
	this.getUseFade = function(){ return vUseFade };
	this.getUseMove = function(){ return vUseMove };
	this.getUseRowHlt = function(){ return vUseRowHlt };
	this.getUseToolTip = function(){ return vUseToolTip };
	this.getUseSort = function(){ return vUseSort };
	this.getUseSingleCell = function(){ return vUseSingleCell };
	this.getFormatArr = function(){ return vFormatArr };
	this.getShowRes = function(){ return vShowRes };
	this.getShowDur = function(){ return vShowDur };
	this.getShowComp = function(){ return vShowComp };
	this.getShowStartDate = function(){ return vShowStartDate };
	this.getShowEndDate = function(){ return vShowEndDate };
	this.getShowTaskInfoRes = function(){ return vShowTaskInfoRes };
	this.getShowTaskInfoDur = function(){ return vShowTaskInfoDur };
	this.getShowTaskInfoComp = function(){ return vShowTaskInfoComp };
	this.getShowTaskInfoStartDate = function(){ return vShowTaskInfoStartDate };
	this.getShowTaskInfoEndDate = function(){ return vShowTaskInfoEndDate };
	this.getShowTaskInfoNotes = function(){ return vShowTaskInfoNotes };
	this.getShowTaskInfoLink = function(){ return vShowTaskInfoLink };
	this.getShowEndWeekDate = function(){ return vShowEndWeekDate };
	this.getShowSelector = function(){ return vShowSelector };
	this.getShowDeps = function(){ return vShowDeps };
	this.getDateInputFormat = function(){ return vDateInputFormat };
	this.getDateTaskTableDisplayFormat = function(){ return vDateTaskTableDisplayFormat };
	this.getDateTaskDisplayFormat = function(){ return vDateTaskDisplayFormat };
	this.getHourMajorDateDisplayFormat = function(){ return vHourMajorDateDisplayFormat };
	this.getHourMinorDateDisplayFormat = function(){ return vHourMinorDateDisplayFormat };
	this.getDayMajorDateDisplayFormat = function(){ return vDayMajorDateDisplayFormat };
	this.getDayMinorDateDisplayFormat = function(){ return vDayMinorDateDisplayFormat };
	this.getWeekMajorDateDisplayFormat = function(){ return vWeekMajorDateDisplayFormat };
	this.getWeekMinorDateDisplayFormat = function(){ return vWeekMinorDateDisplayFormat };
	this.getMonthMajorDateDisplayFormat = function(){ return vMonthMajorDateDisplayFormat };
	this.getMonthMinorDateDisplayFormat = function(){ return vMonthMinorDateDisplayFormat };
	this.getQuarterMajorDateDisplayFormat = function(){ return vQuarterMajorDateDisplayFormat };
	this.getQuarterMinorDateDisplayFormat = function(){ return vQuarterMinorDateDisplayFormat };
	this.getCaptionType = function(){ return vCaptionType };
	this.getMinGpLen = function(){ return vMinGpLen };
	this.getScrollTo = function(){ return vScrollTo };
	this.getHourColWidth = function(){ return vHourColWidth };
	this.getDayColWidth = function(){ return vDayColWidth };
	this.getWeekColWidth = function(){ return vWeekColWidth };
	this.getMonthColWidth = function(){ return vMonthColWidth };
	this.getQuarterColWidth = function(){ return vQuarterColWidth };
	this.getRowHeight = function(){ return vRowHeight };

	this.CalcTaskXY = function()
	{
		var vList = this.getList();
		var vBarDiv;
		var vTaskDiv;
		var vParDiv;
		var vLeft, vTop, vHeight, vWidth;
		var vHeight = Math.floor((this.getRowHeight()/2));

		for(i = 0; i < vList.length; i++)
		{
			vID = vList[i].getID();
			vBarDiv = JSGantt.findObj(vDivId+"bardiv_"+vID);
			vTaskDiv = JSGantt.findObj(vDivId+"taskbar_"+vID);
			if((vList[i].getParItem() && vList[i].getParItem().getGroup()==2))
			{
				vParDiv = JSGantt.findObj(vDivId+"childrow_"+vList[i].getParItem().getID());
			}
			else vParDiv = JSGantt.findObj(vDivId+"childrow_"+vID);

			if(vBarDiv)
			{
				vList[i].setStartX( vBarDiv.offsetLeft+1 );
				vList[i].setStartY( vParDiv.offsetTop+vBarDiv.offsetTop+vHeight-1 );
				vList[i].setEndX( vBarDiv.offsetLeft + vBarDiv.offsetWidth+1 );
				vList[i].setEndY( vParDiv.offsetTop+vBarDiv.offsetTop+vHeight-1 );
			}
		}
	}

	this.AddTaskItem = function(value)
	{
		vTaskList.push(value);
		vProcessNeeded = true;
	}

	this.RemoveTaskItem = function(pID)
	{
		// simply mark the task for removal at this point - actually remove it next time we re-draw the chart
		for (var i = 0; i < vTaskList.length; i++)
		{
			if (vTaskList[i].getID() == pID) vTaskList[i].setToDelete(true);
			else if (vTaskList[i].getParent() == pID) this.RemoveTaskItem(vTaskList[i].getID());
		}
		vProcessNeeded = true;
	}

	this.getList = function(){ return vTaskList };

	this.clearDependencies = function()
	{
		var parent = JSGantt.findObj(vDivId+'gchartbody');
		var depLine;
		var vMaxId = vDepId;
		for ( i=1; i<vMaxId; i++ )
		{
			depLine = JSGantt.findObj(vDivId+"line"+i);
			if (depLine){ parent.removeChild(depLine); }
		}
		vDepId = 1;
	}


	// sLine: Draw a straight line (colored one-pixel wide div), need to parameterize doc item
	this.sLine = function(x1,y1,x2,y2,pClass)
	{
		vLeft = Math.min(x1,x2);
		vTop = Math.min(y1,y2);
		vWid = Math.abs(x2-x1) + 1;
		vHgt = Math.abs(y2-y1) + 1;

		vDoc = JSGantt.findObj(vDivId+'gchartbody');

		var vTmpDiv = document.createElement('div');
		vTmpDiv.id = vDivId+"line"+vDepId++;
		vTmpDiv.style.position = "absolute";
		vTmpDiv.style.overflow = "hidden";
		vTmpDiv.style.zIndex = 0;
		vTmpDiv.style.left = vLeft + "px";
		vTmpDiv.style.top = vTop + "px";
		vTmpDiv.style.width = vWid + "px";
		vTmpDiv.style.height = vHgt + "px";

		vTmpDiv.style.visibility = "visible";

		if (vWid==1) vTmpDiv.className= "glinev";
		else vTmpDiv.className= "glineh";

		if (pClass) vTmpDiv.className+= " "+pClass;

		vDoc.appendChild(vTmpDiv);
	}

	// dLine: Draw a diagonal line (calc line x,y pairs and draw multiple one-by-one sLines)
	this.dLine = function(x1,y1,x2,y2,pClass)
	{
		var dx = x2 - x1;
		var dy = y2 - y1;
		var x = x1;
		var y = y1;

		var n = Math.max(Math.abs(dx),Math.abs(dy));
		dx = dx / n;
		dy = dy / n;
		for ( i = 0; i <= n; i++ )
		{
			vx = Math.round(x);
			vy = Math.round(y);
			this.sLine(vx,vy,vx,vy,pClass);
			x += dx;
			y += dy;
		}
	}

	this.drawDependency =function(x1,y1,x2,y2,pType,pClass)
	{
		var vDir = 1;
		var vBend = false;
		var vShort = 4;
		var vRow = Math.floor(this.getRowHeight()/2);

		if( y2 < y1 ) vRow *= -1;

		switch( pType )
		{
			case 'SF':
				if(x1 - 10 > x2)
				{
					vShort *= -1;
				}
				else
				{
					vBend=true;
					vShort *= -1;
				}
				vDir = -1;
				break;
			case 'SS':
				if ( x1 < x2 ) vShort*=-1;
				else vShort = x2-x1-(2*vShort);
				break;
			case 'FF':
				if ( x1 <= x2 ) vShort =x2-x1+(2*vShort);
				vDir = -1;
				break;
			default:
				if(x1 + 10 >= x2) vBend=true;
				break;
		}

		if (vBend)
		{
			this.sLine(x1,y1,x1+vShort,y1,pClass);
			this.sLine(x1+vShort,y1,x1+vShort,y2-vRow,pClass);
			this.sLine(x1+vShort,y2-vRow,x2-(vShort*2),y2-vRow,pClass);
			this.sLine(x2-(vShort*2),y2-vRow,x2-(vShort*2),y2,pClass);
			this.sLine(x2-(vShort*2),y2,x2,y2,pClass);
		}
		else
		{
			this.sLine(x1,y1,x1+vShort,y1,pClass);
			this.sLine(x1+vShort,y1,x1+vShort,y2,pClass);
			this.sLine(x1+vShort,y2,x2,y2,pClass);
		}

		this.dLine(x2,y2,x2-(3*vDir),y2-(3*vDir),pClass);
		this.dLine(x2,y2,x2-(3*vDir),y2+(3*vDir),pClass);
		this.dLine(x2-(1*vDir),y2,x2-(3*vDir),y2-(2*vDir),pClass);
		this.dLine(x2-(1*vDir),y2,x2-(3*vDir),y2+(2*vDir),pClass);
	}

	this.DrawDependencies = function ()
	{
		if (this.getShowDeps()==1)
		{
			//First recalculate the x,y
			this.CalcTaskXY();
			this.clearDependencies();

			var vList = this.getList();
			for(var i = 0; i < vList.length; i++)
			{
				vDepend = vList[i].getDepend();
				vDependType = vList[i].getDepType();
				var n = vDepend.length;

				if(n>0 && vList[i].getVisible()==1)
				{
					for(var k=0;k<n;k++)
					{
						vTask = this.getArrayLocationByID(vDepend[k]);
						if (vTask>=0 && vList[vTask].getGroup()!=2)
						{
							if(vList[vTask].getVisible()==1)
							{
								if(vDependType[k]=='SS')this.drawDependency(vList[vTask].getStartX()-1,vList[vTask].getStartY(),vList[i].getStartX()-1,vList[i].getStartY(),'SS','gDepSS');
								else if(vDependType[k]=='FF')this.drawDependency(vList[vTask].getEndX(),vList[vTask].getEndY(),vList[i].getEndX(),vList[i].getEndY(),'FF','gDepFF');
								else if(vDependType[k]=='SF')this.drawDependency(vList[vTask].getStartX()-1,vList[vTask].getStartY(),vList[i].getEndX(),vList[i].getEndY(),'SF','gDepSF');
								else if(vDependType[k]=='FS')this.drawDependency(vList[vTask].getEndX(),vList[vTask].getEndY(),vList[i].getStartX()-1,vList[i].getStartY(),'FS','gDepFS');
							}
						}
					}
				}
			}
		}
		// draw the current date line
		if (vTodayPx >= 0) this.sLine(vTodayPx, 0, vTodayPx, JSGantt.findObj(vDivId+'chartTable').offsetHeight - 1, 'gCurDate');
	}

	this.getArrayLocationByID = function(pId)
	{
		var vList = this.getList();
		for(var i = 0; i < vList.length; i++)
		{
			if(vList[i].getID()==pId)
			return i;
		}
		return -1;
	}

	this.newNode = function(pParent, pNodeType, pId, pClass, pText, pWidth, pLeft, pDisplay, pColspan, pAttribs)
	{
		var vNewNode = pParent.appendChild(document.createElement(pNodeType));
		if (pAttribs)
		{
			for (var i=0; i+1<pAttribs.length; i+=2)
			{
				vNewNode.setAttribute(pAttribs[i],pAttribs[i+1]);
			}
		}
		// I wish I could do this with setAttribute but older IEs don't play nice
		if (pId)vNewNode.id = pId;
		if (pClass)vNewNode.className = pClass;
		if (pWidth)vNewNode.style.width = (isNaN(pWidth*1))?pWidth:pWidth+'px';
		if (pLeft)vNewNode.style.left = (isNaN(pLeft*1))?pLeft:pLeft+'px';
		if (pText)vNewNode.appendChild(document.createTextNode(pText));
		if (pDisplay)vNewNode.style.display = pDisplay;
		if (pColspan)vNewNode.colSpan = pColspan;
		return vNewNode;
	}

	this.Draw = function()
	{
		var vMaxDate = new Date();
		var vMinDate = new Date();
		var vTmpDate = new Date();
		var vTaskLeftPx = 0;
		var vTaskRightPx = 0;
		var vTaskWidth = 1;
		var vNumCols = 0;
		var vNumRows = 0;
		var vSingleCell = false;
		var vID = 0;
		var vMainTable = "";
		var vDateRow = null;
		var vFirstCellItemRowStr = "";
		var vItemRowStr = "";
		var vColWidth = 0;
		var vColUnit = 0;
		var vChild;
		var vGroup;
		var vTaskDiv;
		var vParDiv;

		if(vTaskList.length > 0)
		{
			// Process all tasks, reset parent date and completion % if task list has altered
			if (vProcessNeeded)	JSGantt.processRows(vTaskList, 0, -1, 1, 1, this.getUseSort());
			vProcessNeeded=false;

			// get overall min/max dates plus padding
			vMinDate = JSGantt.getMinDate(vTaskList, vFormat);
			vMaxDate = JSGantt.getMaxDate(vTaskList, vFormat);

			// Calculate chart width variables.
			if(vFormat == 'day') vColWidth = vDayColWidth;
			else if(vFormat == 'week') vColWidth = vWeekColWidth;
			else if(vFormat == 'month') vColWidth = vMonthColWidth;
			else if(vFormat == 'quarter') vColWidth = vQuarterColWidth;
			else if(vFormat == 'hour') vColWidth = vHourColWidth;

			// DRAW the Left-side of the chart (names, resources, comp%)
			var vLeftHeader = document.createDocumentFragment();

			var vTmpDiv = this.newNode(vLeftHeader, 'div', vDivId+'glisthead', 'glistlbl gcontainercol');
			var vTmpTab = this.newNode(vTmpDiv, 'table', null, 'gtasktableh');
			var vTmpTBody = this.newNode(vTmpTab, 'tbody');
			var vTmpRow = this.newNode(vTmpTBody, 'tr');
			var vTmpCell = this.newNode(vTmpRow, 'td', null, 'gtasklist', '\u00A0');
			vTmpCell = this.newNode(vTmpRow, 'td', null, 'gspanning gtaskname');
			vTmpCell.appendChild(this.drawSelector( 'top' ));
			if(vShowRes ==1)vTmpCell = this.newNode(vTmpRow, 'td', null, 'gspanning gresource', '\u00A0');
			if(vShowDur ==1)vTmpCell = this.newNode(vTmpRow, 'td', null, 'gspanning gduration', '\u00A0');
			if(vShowComp ==1)vTmpCell = this.newNode(vTmpRow, 'td', null, 'gspanning gpccomplete', '\u00A0');
			if(vShowStartDate ==1)vTmpCell = this.newNode(vTmpRow, 'td', null, 'gspanning gstartdate', '\u00A0');
			if(vShowEndDate ==1)vTmpCell = this.newNode(vTmpRow, 'td', null, 'gspanning genddate', '\u00A0');

			vTmpRow = this.newNode(vTmpTBody, 'tr');
			vTmpCell = this.newNode(vTmpRow, 'td', null, 'gtasklist', '\u00A0');
			vTmpCell = this.newNode(vTmpRow, 'td', null, 'gtaskname', '\u00A0');
			if(vShowRes ==1)vTmpCell = this.newNode(vTmpRow, 'td', null, 'gtaskheading gresource', vLangs[vLang]['resource']);
			if(vShowDur ==1)vTmpCell = this.newNode(vTmpRow, 'td', null, 'gtaskheading gduration', vLangs[vLang]['duration']);
			if(vShowComp ==1)vTmpCell = this.newNode(vTmpRow, 'td', null, 'gtaskheading gpccomplete', vLangs[vLang]['comp']);
			if(vShowStartDate ==1)vTmpCell = this.newNode(vTmpRow, 'td', null, 'gtaskheading gstartdate', vLangs[vLang]['startdate']);
			if(vShowEndDate ==1)vTmpCell = this.newNode(vTmpRow, 'td', null, 'gtaskheading genddate', vLangs[vLang]['enddate']);

			vTmpDiv = this.newNode(vLeftHeader, 'div', null, 'glabelfooter');

			var vLeftTable = document.createDocumentFragment();
			var vTmpDiv2 = this.newNode(vLeftTable, 'div', vDivId+'glistbody', 'glistgrid gcontainercol');
			vTmpTab = this.newNode(vTmpDiv2, 'table', null, 'gtasktable');
			vTmpTBody = this.newNode(vTmpTab, 'tbody');

			for(i = 0; i < vTaskList.length; i++)
			{
				if( vTaskList[i].getGroup()==1) vBGColor = "ggroupitem";
				else vBGColor = "glineitem";

				vID = vTaskList[i].getID();

				if((!(vTaskList[i].getParItem() && vTaskList[i].getParItem().getGroup()==2)) || vTaskList[i].getGroup()==2)
				{
					if(vTaskList[i].getVisible() == 0) vTmpRow = this.newNode(vTmpTBody, 'tr', vDivId+'child_' + vID, 'gname ' + vBGColor, null, null, null, 'none');
					else vTmpRow = this.newNode(vTmpTBody, 'tr', vDivId+'child_' + vID, 'gname ' + vBGColor);
					vTmpCell = this.newNode(vTmpRow, 'td', null, 'gtasklist', '\u00A0');
					vTmpCell = this.newNode(vTmpRow, 'td', null, 'gtaskname');

					vCellContents ='';
					for(j=1; j<vTaskList[i].getLevel(); j++)
					{
						vCellContents += '\u00A0\u00A0\u00A0\u00A0';
					}

					if( vTaskList[i].getGroup()==1)
					{
						vTmpDiv = this.newNode(vTmpCell, 'div', null, null, vCellContents);
						this.newNode(vTmpDiv, 'span', vDivId+'group_' + vID, 'gfoldercollapse', ( vTaskList[i].getOpen() == 1)?'-':'+');
						vTmpDiv.appendChild(document.createTextNode('\u00A0'+vTaskList[i].getName()));
					}
					else
					{
						vCellContents += '\u00A0\u00A0\u00A0\u00A0';
						vTmpDiv = this.newNode(vTmpCell, 'div', null, null, vCellContents+vTaskList[i].getName());
					}

					if(vShowRes ==1)
					{
						vTmpCell = this.newNode(vTmpRow, 'td', null, 'gresource');
						vTmpDiv = this.newNode(vTmpCell, 'div', null, null, vTaskList[i].getResource());
					}
					if(vShowDur ==1)
					{
						vTmpCell = this.newNode(vTmpRow, 'td', null, 'gduration');
						vTmpDiv = this.newNode(vTmpCell, 'div', null, null, vTaskList[i].getDuration(vFormat, vLangs[vLang]));
					}
					if(vShowComp==1)
					{
						vTmpCell = this.newNode(vTmpRow, 'td', null, 'gpccomplete');
						vTmpDiv = this.newNode(vTmpCell, 'div', null, null, vTaskList[i].getCompStr());
					}
					if(vShowStartDate==1)
					{
						vTmpCell = this.newNode(vTmpRow, 'td', null, 'gstartdate');
						vTmpDiv = this.newNode(vTmpCell, 'div', null, null, JSGantt.formatDateStr( vTaskList[i].getStart(), vDateTaskTableDisplayFormat, vLangs[vLang]));
					}
					if(vShowEndDate==1)
					{
						vTmpCell = this.newNode(vTmpRow, 'td', null, 'genddate');
						vTmpDiv = this.newNode(vTmpCell, 'div', null, null, JSGantt.formatDateStr( vTaskList[i].getEnd(), vDateTaskTableDisplayFormat, vLangs[vLang]));
					}
					vNumRows++;
				}
			}

			// DRAW the date format selector at bottom left.
			vTmpRow = this.newNode(vTmpTBody, 'tr');
			vTmpCell = this.newNode(vTmpRow, 'td', null, 'gtasklist', '\u00A0');
			vTmpCell = this.newNode(vTmpRow, 'td', null, 'gspanning gtaskname');
			vTmpCell.appendChild(this.drawSelector( 'bottom' ));
			if(vShowRes ==1)vTmpCell = this.newNode(vTmpRow, 'td', null, 'gspanning gresource', '\u00A0');
			if(vShowDur ==1)vTmpCell = this.newNode(vTmpRow, 'td', null, 'gspanning gduration', '\u00A0');
			if(vShowComp ==1)vTmpCell = this.newNode(vTmpRow, 'td', null, 'gspanning gpccomplete', '\u00A0');
			if(vShowStartDate ==1)vTmpCell = this.newNode(vTmpRow, 'td', null, 'gspanning gstartdate', '\u00A0');
			if(vShowEndDate ==1)vTmpCell = this.newNode(vTmpRow, 'td', null, 'gspanning genddate', '\u00A0');
			// Add some white space so the vertical scroll distance should always be greater
			// than for the right pane (keep to a minimum as it is seen in unconstrained height designs)
			this.newNode(vTmpDiv2, 'br');
			this.newNode(vTmpDiv2, 'br');

			// Draw the Chart Rows
			var vRightHeader = document.createDocumentFragment();
			vTmpDiv = this.newNode(vRightHeader, 'div', vDivId+'gcharthead', 'gchartlbl gcontainercol');
			vTmpTab = this.newNode(vTmpDiv, 'table', vDivId+'chartTableh', 'gcharttableh');
			vTmpTBody = this.newNode(vTmpTab, 'tbody');
			vTmpRow = this.newNode(vTmpTBody, 'tr');

			vTmpDate.setFullYear(vMinDate.getFullYear(), vMinDate.getMonth(), vMinDate.getDate());
			if(vFormat == 'hour')vTmpDate.setHours(vMinDate.getHours());
			else vTmpDate.setHours(0);
			vTmpDate.setMinutes(0);
			vTmpDate.setSeconds(0);
			vTmpDate.setMilliseconds(0);

			// Major Date Header
			while(vTmpDate.getTime() <= vMaxDate.getTime())
			{
				vHeaderCellClass = "gmajorheading";
				vCellContents = '';

				if(vFormat == 'day')
				{
					vTmpCell = this.newNode(vTmpRow, 'td', null, vHeaderCellClass, null, null, null, null, 7);
					vCellContents += JSGantt.formatDateStr(vTmpDate,vDayMajorDateDisplayFormat,vLangs[vLang]) ;
					vTmpDate.setDate(vTmpDate.getDate() + 6);

					if ( vShowEndWeekDate == 1 ) vCellContents += ' - ' +JSGantt.formatDateStr(vTmpDate, vDayMajorDateDisplayFormat,vLangs[vLang]);

					this.newNode(vTmpCell, 'div', null, null, vCellContents, vColWidth*7);
					vTmpDate.setDate(vTmpDate.getDate() + 1);
				}
				else if(vFormat == 'week')
				{
					vTmpCell = this.newNode(vTmpRow, 'td', null, vHeaderCellClass, null, vColWidth);
					this.newNode(vTmpCell, 'div', null, null, JSGantt.formatDateStr(vTmpDate,vWeekMajorDateDisplayFormat,vLangs[vLang]), vColWidth);
					vTmpDate.setDate(vTmpDate.getDate() + 7);
				}
				else if(vFormat == 'month')
				{
					var vColSpan = (12 - vTmpDate.getMonth());
					if (vTmpDate.getFullYear() == vMaxDate.getFullYear()) vColSpan -= (11 - vMaxDate.getMonth());
					vTmpCell = this.newNode(vTmpRow, 'td', null, vHeaderCellClass, null, null, null, null, vColSpan);
					this.newNode(vTmpCell, 'div', null, null, JSGantt.formatDateStr(vTmpDate,vMonthMajorDateDisplayFormat,vLangs[vLang]), vColWidth*vColSpan);
					vTmpDate.setFullYear(vTmpDate.getFullYear()+1,0,1);
				}
				else if(vFormat == 'quarter')
				{
					var vColSpan = (4 - Math.floor(vTmpDate.getMonth()/3));
					if (vTmpDate.getFullYear() == vMaxDate.getFullYear()) vColSpan -= (3 - Math.floor(vMaxDate.getMonth()/3));
					vTmpCell = this.newNode(vTmpRow, 'td', null, vHeaderCellClass, null, null, null, null, vColSpan);
					this.newNode(vTmpCell, 'div', null, null, JSGantt.formatDateStr(vTmpDate,vQuarterMajorDateDisplayFormat,vLangs[vLang]), vColWidth*vColSpan);
					vTmpDate.setFullYear(vTmpDate.getFullYear()+1,0,1);
				}
				else if(vFormat == 'hour')
				{
					var vColSpan = (24 - vTmpDate.getHours());
					if (vTmpDate.getFullYear() == vMaxDate.getFullYear() &&
						vTmpDate.getMonth() == vMaxDate.getMonth() &&
						vTmpDate.getDate() == vMaxDate.getDate()) vColSpan -= (23 - vMaxDate.getHours());
					vTmpCell = this.newNode(vTmpRow, 'td', null, vHeaderCellClass, null, null, null, null, vColSpan);
					this.newNode(vTmpCell, 'div', null, null, JSGantt.formatDateStr(vTmpDate,vHourMajorDateDisplayFormat,vLangs[vLang]), vColWidth*vColSpan);
					vTmpDate.setHours(0);
					vTmpDate.setDate(vTmpDate.getDate() + 1);
				}
			}

			vTmpRow = this.newNode(vTmpTBody, 'tr');

			// Minor Date header and Cell Rows
			vTmpDate.setFullYear(vMinDate.getFullYear(), vMinDate.getMonth(), vMinDate.getDate(), vMinDate.getHours());
			if(vFormat == 'hour')vTmpDate.setHours(vMinDate.getHours());
			vNumCols = 0;

			while(vTmpDate.getTime() <= vMaxDate.getTime())
			{
				vHeaderCellClass = "gminorheading";
				vCellClass = "gtaskcell";

				if(vFormat == 'day' )
				{
					if(vTmpDate.getDay() % 6 == 0)
					{
						vHeaderCellClass += "wkend";
						vCellClass += "wkend";
					}

					if(vTmpDate <= vMaxDate)
					{
						vTmpCell = this.newNode(vTmpRow, 'td', null, vHeaderCellClass);
						this.newNode(vTmpCell, 'div', null, null, JSGantt.formatDateStr(vTmpDate,vDayMinorDateDisplayFormat,vLangs[vLang]), vColWidth);
						vNumCols++;
					}

					vTmpDate.setDate(vTmpDate.getDate() + 1);
				}
				else if(vFormat == 'week')
				{
					if(vTmpDate <= vMaxDate)
					{
						vTmpCell = this.newNode(vTmpRow, 'td', null, vHeaderCellClass);
						this.newNode(vTmpCell, 'div', null, null, JSGantt.formatDateStr(vTmpDate,vWeekMinorDateDisplayFormat,vLangs[vLang]), vColWidth);
						vNumCols++;
					}

					vTmpDate.setDate(vTmpDate.getDate() + 7);
				}
				else if(vFormat == 'month')
				{
					if(vTmpDate <= vMaxDate)
					{
						vTmpCell = this.newNode(vTmpRow, 'td', null, vHeaderCellClass);
						this.newNode(vTmpCell, 'div', null, null, JSGantt.formatDateStr(vTmpDate,vMonthMinorDateDisplayFormat,vLangs[vLang]), vColWidth);
						vNumCols++;
					}

					vTmpDate.setDate(vTmpDate.getDate() + 1);

					while(vTmpDate.getDate() > 1)
					{
						vTmpDate.setDate(vTmpDate.getDate() + 1);
					}
				}
				else if(vFormat == 'quarter')
				{
					if(vTmpDate <= vMaxDate)
					{
						vTmpCell = this.newNode(vTmpRow, 'td', null, vHeaderCellClass);
						this.newNode(vTmpCell, 'div', null, null, JSGantt.formatDateStr(vTmpDate,vQuarterMinorDateDisplayFormat,vLangs[vLang]), vColWidth);
						vNumCols++;
					}

					vTmpDate.setDate(vTmpDate.getDate() + 81);

					while(vTmpDate.getDate() > 1) vTmpDate.setDate(vTmpDate.getDate() + 1);
				}
				else if(vFormat == 'hour')
				{
					for(i=vTmpDate.getHours();i<24;i++)
					{
						vTmpDate.setHours(i);//works around daylight savings but may look a little odd on days where the clock goes forward
						if(vTmpDate <= vMaxDate)
						{
							vTmpCell = this.newNode(vTmpRow, 'td', null, vHeaderCellClass);
							this.newNode(vTmpCell, 'div', null, null, JSGantt.formatDateStr(vTmpDate,vHourMinorDateDisplayFormat,vLangs[vLang]), vColWidth);
							vNumCols++;
						}
					}
					vTmpDate.setHours(0);
					vTmpDate.setDate(vTmpDate.getDate() + 1);
				}
			}
			vDateRow = vTmpRow;

			vTaskLeftPx = (vNumCols *(vColWidth + 1))+1;

			if( vUseSingleCell !=0 && vUseSingleCell<(vNumCols*vNumRows))vSingleCell = true;

			this.newNode(vTmpDiv, 'div', null, 'rhscrpad', null, null, vTaskLeftPx+1);

			var vRightTable = document.createDocumentFragment();
			vTmpDiv = this.newNode(vRightTable, 'div', vDivId+'gchartbody', 'gchartgrid gcontainercol');
			vTmpTab = this.newNode(vTmpDiv, 'table', vDivId+'chartTable', 'gcharttable', null, vTaskLeftPx);
			this.newNode(vTmpDiv, 'div', null, 'rhscrpad', null, null, vTaskLeftPx+1);
			vTmpTBody = this.newNode(vTmpTab, 'tbody');

			// Draw each row

			for(i = 0; i < vTaskList.length; i++)
			{
				var curTaskStart = vTaskList[i].getStart();
				var curTaskEnd = vTaskList[i].getEnd();
				if ((curTaskEnd.getTime()-(curTaskEnd.getTimezoneOffset()*60000)) % (86400000) == 0) curTaskEnd = new Date(curTaskEnd.getFullYear(), curTaskEnd.getMonth(), curTaskEnd.getDate()+1, curTaskEnd.getHours(), curTaskEnd.getMinutes(), curTaskEnd.getSeconds()); // add 1 day here to simplify calculations below

				vTaskLeftPx = JSGantt.getOffset(vMinDate, curTaskStart, vColWidth, vFormat);
				vTaskRightPx = JSGantt.getOffset(curTaskStart, curTaskEnd, vColWidth, vFormat);

				vID = vTaskList[i].getID();
				var vComb=(vTaskList[i].getParItem() && vTaskList[i].getParItem().getGroup()==2);

				if( vTaskList[i].getMile() && !vComb )
				{
					vTmpRow = this.newNode(vTmpTBody, 'tr', vDivId+'childrow_'+vID, 'gmileitem gmile'+vFormat, null, null, null, ((vTaskList[i].getVisible() == 0)? 'none' : null));
					vTmpCell = this.newNode(vTmpRow, 'td', null, 'gtaskcell');
					vTmpDiv = this.newNode(vTmpCell, 'div', null, 'gtaskcelldiv', '\u00A0\u00A0');
					vTmpDiv = this.newNode(vTmpDiv, 'div', vDivId+'bardiv_'+vID, 'gtaskbarcontainer', null, 12, vTaskLeftPx-6);

					vTmpDiv2 = this.newNode(vTmpDiv, 'div', vDivId+'taskbar_'+vID, vTaskList[i].getClass(), null, 12);

					if(vTaskList[i].getCompVal() < 100)
						vTmpDiv2.appendChild(document.createTextNode('\u25CA'));
					else
					{
						vTmpDiv2 = this.newNode(vTmpDiv2, 'div', null, 'gmilediamond');
						this.newNode(vTmpDiv2, 'div', null, 'gmdtop');
						this.newNode(vTmpDiv2, 'div', null, 'gmdbottom');
					}

					if( g.getCaptionType() )
					{
						vCaptionStr = '';
						switch( g.getCaptionType() )
						{
							case 'Caption': vCaptionStr = vTaskList[i].getCaption(); break;
							case 'Resource': vCaptionStr = vTaskList[i].getResource(); break;
							case 'Duration': vCaptionStr = vTaskList[i].getDuration(vFormat, vLangs[vLang]); break;
							case 'Complete': vCaptionStr = vTaskList[i].getCompStr(); break;
						}
						this.newNode(vTmpDiv, 'div', null, 'gmilecaption', vCaptionStr, 120, 12);
					}
					// Add Task Info div for tooltip
					vTmpDiv2 = this.newNode(vTmpDiv, 'div', 'tt'+vDivId+'taskbar_'+vID, null, null, null, null, 'none');
					vTmpDiv2 = this.newNode(vTmpDiv2, 'div', 'tt'+vDivId+'complete_'+vID);
					vTmpDiv2.appendChild(this.createTaskInfo(vTaskList[i]));

					if(!vSingleCell && !vComb)
					{
						var vCellFormat='';
						for(var j=0; j<vNumCols-1; j++)
						{
							if( vFormat=='day'&&((j%7==4)||(j%7==5)) ) vCellFormat='gtaskcellwkend';
							else vCellFormat='gtaskcell';
							this.newNode(vTmpRow, 'td', null, vCellFormat, '\u00A0\u00A0');
						}
					}
				}
				else
				{
					vTaskWidth = vTaskRightPx - 1;

					// Draw Group Bar which has outer div with inner group div and several small divs to left and right to create angled-end indicators
					if( vTaskList[i].getGroup())
					{
						vTaskWidth = (vTaskWidth > vMinGpLen && vTaskWidth < vMinGpLen*2)? vMinGpLen*2 : vTaskWidth; // Expand to show two end points
						vTaskWidth = (vTaskWidth < vMinGpLen)? vMinGpLen : vTaskWidth; // expand to show one end point

						vTmpRow = this.newNode(vTmpTBody, 'tr', vDivId+'childrow_'+vID, ((vTaskList[i].getGroup()==2)?'glineitem gitem':'ggroupitem ggroup')+vFormat, null, null, null, ((vTaskList[i].getVisible() == 0)? 'none' : null));
						vTmpCell = this.newNode(vTmpRow, 'td', null, 'gtaskcell');
						vTmpDiv = this.newNode(vTmpCell, 'div', null, 'gtaskcelldiv', '\u00A0\u00A0');
						vTaskList[i].setCellDiv(vTmpDiv);
						if(vTaskList[i].getGroup()==1)
						{
							vTmpDiv = this.newNode(vTmpDiv, 'div', vDivId+'bardiv_'+vID, 'gtaskbarcontainer', null, vTaskWidth, vTaskLeftPx);
							vTmpDiv2 = this.newNode(vTmpDiv, 'div', vDivId+'taskbar_'+vID, vTaskList[i].getClass(), null, vTaskWidth);

							this.newNode(vTmpDiv2, 'div', vDivId+'complete_'+vID, vTaskList[i].getClass() +'complete', null, vTaskList[i].getCompStr());

							this.newNode(vTmpDiv, 'div', null, vTaskList[i].getClass() +'endpointleft');
							if ( vTaskWidth >= vMinGpLen*2 ) this.newNode(vTmpDiv, 'div', null, vTaskList[i].getClass() +'endpointright');

							if( g.getCaptionType() )
							{
								vCaptionStr = '';
								switch( g.getCaptionType() )
								{
									case 'Caption': vCaptionStr = vTaskList[i].getCaption(); break;
									case 'Resource': vCaptionStr = vTaskList[i].getResource(); break;
									case 'Duration': vCaptionStr = vTaskList[i].getDuration(vFormat, vLangs[vLang]); break;
									case 'Complete': vCaptionStr = vTaskList[i].getCompStr(); break;
								}
								this.newNode(vTmpDiv, 'div', null, 'ggroupcaption', vCaptionStr, 120);
							}
							// Add Task Info div for tooltip
							vTmpDiv2 = this.newNode(vTmpDiv, 'div', 'tt'+vDivId+'taskbar_'+vID, null, null, null, null, 'none');
							vTmpDiv2 = this.newNode(vTmpDiv2, 'div', 'tt'+vDivId+'complete_'+vID);
							vTmpDiv2.appendChild(this.createTaskInfo(vTaskList[i]));
						}

						if(!vSingleCell && !vComb)
						{
							var vCellFormat='';
							for(var j=0; j<vNumCols-1; j++)
							{
								if( vFormat=='day'&&((j%7==4)||(j%7==5)) ) vCellFormat='gtaskcellwkend';
								else vCellFormat='gtaskcell';
								this.newNode(vTmpRow, 'td', null, vCellFormat, '\u00A0\u00A0');
							}
						}
					}
					else
					{
						vTaskWidth = (vTaskWidth <=0)? 1 : vTaskWidth;

						if(vComb)
						{
							vTmpDiv = vTaskList[i].getParItem().getCellDiv();
						}
						else
						{
							vTmpRow = this.newNode(vTmpTBody, 'tr', vDivId+'childrow_'+vID, 'glineitem gitem'+vFormat, null, null, null, ((vTaskList[i].getVisible() == 0)? 'none' : null));
							vTmpCell = this.newNode(vTmpRow, 'td', null, 'gtaskcell');
							vTmpDiv = this.newNode(vTmpCell, 'div', null, 'gtaskcelldiv', '\u00A0\u00A0');
						}
						// Draw Task Bar which has colored bar div, and opaque completion div
						vTmpDiv = this.newNode(vTmpDiv, 'div', vDivId+'bardiv_'+vID, 'gtaskbarcontainer', null, vTaskWidth, vTaskLeftPx);
						vTmpDiv2 = this.newNode(vTmpDiv, 'div', vDivId+'taskbar_'+vID, vTaskList[i].getClass(), null, vTaskWidth);
						this.newNode(vTmpDiv2, 'div', vDivId+'complete_'+vID, vTaskList[i].getClass() +'complete', null, vTaskList[i].getCompStr());

						if( g.getCaptionType() && (!vComb || (vComb && vTaskList[i].getParItem().getEnd() == vTaskList[i].getEnd())))
						{
							vCaptionStr = '';
							var vTmpItem = vTaskList[i];
							if(vComb)vTmpItem = vTaskList[i].getParItem();
							switch( g.getCaptionType() )
							{
								case 'Caption': vCaptionStr = vTmpItem.getCaption(); break;
								case 'Resource': vCaptionStr = vTmpItem.getResource(); break;
								case 'Duration': vCaptionStr = vTmpItem.getDuration(vFormat, vLangs[vLang]); break;
								case 'Complete': vCaptionStr = vTmpItem.getCompStr(); break;
							}
							this.newNode(vTmpDiv, 'div', null, 'gcaption', vCaptionStr, 120);
						}
						// Add Task Info div for tooltip
						vTmpDiv2 = this.newNode(vTmpDiv, 'div', 'tt'+vDivId+'taskbar_'+vID, null, null, null, null, 'none');
						vTmpDiv2 = this.newNode(vTmpDiv2, 'div', 'tt'+vDivId+'complete_'+vID);
						vTmpDiv2.appendChild(this.createTaskInfo(vTaskList[i]));

						if(!vSingleCell && !vComb)
						{
							var vCellFormat='';
							for(var j=0; j<vNumCols-1; j++)
							{
								if( vFormat=='day'&&((j%7==4)||(j%7==5)) ) vCellFormat='gtaskcellwkend';
								else vCellFormat='gtaskcell';
								this.newNode(vTmpRow, 'td', null, vCellFormat, '\u00A0\u00A0');
							}
						}
					}
				}
			}

			if(!vSingleCell) vTmpTBody.appendChild(vDateRow.cloneNode(true));

			while(vDiv.hasChildNodes())vDiv.removeChild(vDiv.firstChild);
			vTmpDiv = this.newNode(vDiv, 'div', null, 'gchartcontainer');
			vTmpDiv.appendChild(vRightHeader);
			vTmpDiv.appendChild(vLeftHeader);
			vTmpDiv.appendChild(vRightTable);
			vTmpDiv.appendChild(vLeftTable);
			this.newNode(vTmpDiv, 'div', null, 'ggridfooter');

/* Quick hack to show the generated HTML on older browsers - add a '/' to the begining of this line to activate
			var tmpGenSrc = document.createElement('textarea');
			tmpGenSrc.appendChild(document.createTextNode(vTmpDiv.innerHTML));
			vDiv.appendChild(tmpGenSrc);
//*/
			// Now all the content exists, register listeners
			for(i = 0; i < vTaskList.length; i++)
			{
				vID = vTaskList[i].getID();
				vChild = JSGantt.findObj(vDivId+"child_"+vID);
				vTaskDiv = JSGantt.findObj(vDivId+"taskbar_"+vID);
				vParDiv = JSGantt.findObj(vDivId+"childrow_"+vID);
				if(vTaskList[i].getGroup()==1)vGroup = JSGantt.findObj(vDivId+"group_"+vID);

				if(vTaskDiv && vUseToolTip==1) JSGantt.addTootltipListeners( this, vTaskDiv );
				if(vChild && vParDiv) JSGantt.addThisRowListeners( this, vChild, vParDiv );
				if(vTaskList[i].getGroup()==1 && vGroup) JSGantt.addFolderListeners( this, vGroup, vID );
			}

			for ( var i = 0; i < vShowSelector.length; i++ )
			{
				for ( var j = 0; j < vFormatArr.length; j++ )
				{
					var vSelectorDisplayFormat = vFormatArr[j].toLowerCase();
					var vSelectorId = vDivId+"format" + vFormatArr[j].toLowerCase() + vShowSelector[i];
					JSGantt.addFormatListeners(this, vSelectorDisplayFormat, vSelectorId);
				}
			}

			JSGantt.addScrollListeners(this);

			// now check if we are actually scrolling the pane
			if ( vScrollTo != '' )
			{
				var vScrollDate = new Date(vMinDate.getTime());
				var vScrollPx = 0;

				if(vScrollTo.substr(0,2) == 'px')
				{
					vScrollPx = parseInt(vScrollTo.substr(2));
				}
				else
				{
					if ( vScrollTo == 'today' ) vScrollDate = new Date();
					else vScrollDate = JSGantt.parseDateStr(vScrollTo, this.getDateInputFormat());

					vScrollDate.setHours( 0,0,0,0 ); // zero any time present

					vScrollPx = JSGantt.getOffset(vMinDate, vScrollDate, vColWidth, vFormat)
				}
				JSGantt.findObj(vDivId+'gchartbody').scrollLeft = vScrollPx;
			}

			if (vMinDate.getTime() <= (new Date()).getTime() && vMaxDate.getTime() >= (new Date()).getTime() ) vTodayPx = JSGantt.getOffset(vMinDate, new Date(), vColWidth, vFormat);
			else vTodayPx = -1;
			this.DrawDependencies();
		}
	} //this.draw

	this.mouseOver = function( pObj1, pObj2 )
	{
		if ( this.getUseRowHlt())
		{
			pObj1.className += " gitemhighlight";
			pObj2.className += " gitemhighlight";
		}
	}

	this.mouseOut = function( pObj1, pObj2 )
	{
		if ( this.getUseRowHlt())
		{
			pObj1.className = pObj1.className.replace( /(?:^|\s)gitemhighlight(?!\S)/g , '' );
			pObj2.className = pObj2.className.replace( /(?:^|\s)gitemhighlight(?!\S)/g , '' );
		}
	}

	this.drawSelector = function( pPos )
	{
		var vOutput = document.createDocumentFragment();
		var vDisplay=false;

		for ( var i = 0; i < vShowSelector.length && !vDisplay; i++ )
		{
			if ( vShowSelector[i].toLowerCase() == pPos.toLowerCase() ) vDisplay=true;
		}

		if ( vDisplay )
		{
			var vTmpDiv = this.newNode(vOutput, 'div', null, 'gselector', vLangs[vLang]['format']+':' );

			if (vFormatArr.join().toLowerCase().indexOf("hour")!=-1)
				this.newNode(vTmpDiv, 'span', vDivId+'formathour'+pPos, 'gformlabel'+((vFormat=='hour')?' gselected':''), vLangs[vLang]['hour'] );

			if (vFormatArr.join().toLowerCase().indexOf("day")!=-1)
				this.newNode(vTmpDiv, 'span', vDivId+'formatday'+pPos, 'gformlabel'+((vFormat=='day')?' gselected':''), vLangs[vLang]['day'] );

			if (vFormatArr.join().toLowerCase().indexOf("week")!=-1)
				this.newNode(vTmpDiv, 'span', vDivId+'formatweek'+pPos, 'gformlabel'+((vFormat=='week')?' gselected':''), vLangs[vLang]['week'] );

			if (vFormatArr.join().toLowerCase().indexOf("month")!=-1)
				this.newNode(vTmpDiv, 'span', vDivId+'formatmonth'+pPos, 'gformlabel'+((vFormat=='month')?' gselected':''), vLangs[vLang]['month'] );

			if (vFormatArr.join().toLowerCase().indexOf("quarter")!=-1)
				this.newNode(vTmpDiv, 'span', vDivId+'formatquarter'+pPos, 'gformlabel'+((vFormat=='quarter')?' gselected':''), vLangs[vLang]['quarter'] );
		}
		else
		{
			this.newNode(vOutput, 'div', null, 'gselector' );
		}
		return vOutput;
	}

	this.createTaskInfo = function(pTask)
	{
		var vTaskInfoBox = document.createDocumentFragment();
		var vTaskInfo = this.newNode(vTaskInfoBox, 'div', null, 'gTaskInfo' );
		this.newNode(vTaskInfo, 'span', null, 'gTtTitle', pTask.getName() );
		if(vShowTaskInfoStartDate==1){
			vTmpDiv = this.newNode(vTaskInfo, 'div', null, 'gTILine gTIsd' );
			this.newNode(vTmpDiv, 'span', null, 'gTaskLabel', vLangs[vLang]['startdate']+': ' );
			this.newNode(vTmpDiv, 'span', null, 'gTaskText', JSGantt.formatDateStr( pTask.getStart(), vDateTaskDisplayFormat,vLangs[vLang]) );
		}
		if(vShowTaskInfoEndDate==1){
			vTmpDiv = this.newNode(vTaskInfo, 'div', null, 'gTILine gTIed' );
			this.newNode(vTmpDiv, 'span', null, 'gTaskLabel', vLangs[vLang]['enddate']+': ' );
			this.newNode(vTmpDiv, 'span', null, 'gTaskText', JSGantt.formatDateStr( pTask.getEnd(), vDateTaskDisplayFormat,vLangs[vLang]) );
		}
		if(vShowTaskInfoDur==1 && !pTask.getMile()){
			vTmpDiv = this.newNode(vTaskInfo, 'div', null, 'gTILine gTId' );
			this.newNode(vTmpDiv, 'span', null, 'gTaskLabel', vLangs[vLang]['duration']+': ' );
			this.newNode(vTmpDiv, 'span', null, 'gTaskText', pTask.getDuration(vFormat, vLangs[vLang]) );
		}
		if(vShowTaskInfoComp==1){
			vTmpDiv = this.newNode(vTaskInfo, 'div', null, 'gTILine gTIc' );
			this.newNode(vTmpDiv, 'span', null, 'gTaskLabel', vLangs[vLang]['completion']+': ' );
			this.newNode(vTmpDiv, 'span', null, 'gTaskText', pTask.getCompStr() );
		}
		if(vShowTaskInfoRes==1){
			vTmpDiv = this.newNode(vTaskInfo, 'div', null, 'gTILine gTIr' );
			this.newNode(vTmpDiv, 'span', null, 'gTaskLabel', vLangs[vLang]['resource']+': ' );
			this.newNode(vTmpDiv, 'span', null, 'gTaskText', pTask.getResource() );
		}
		if(vShowTaskInfoLink ==1 && pTask.getLink() != ''){
			vTmpDiv = this.newNode(vTaskInfo, 'div', null, 'gTILine gTIl' );
			var vTmpNode = this.newNode(vTmpDiv, 'span', null, 'gTaskLabel' );
			vTmpNode = this.newNode(vTmpNode, 'a', null, 'gTaskText', vLangs[vLang]['moreinfo']);
			vTmpNode.setAttribute('href',pTask.getLink());
		}
		if(vShowTaskInfoNotes==1){
			vTmpDiv = this.newNode(vTaskInfo, 'div', null, 'gTILine gTIn' );
			this.newNode(vTmpDiv, 'span', null, 'gTaskLabel', vLangs[vLang]['notes']+': ' );
			if(pTask.getNotes())vTmpDiv.appendChild(pTask.getNotes());
		}
		return vTaskInfoBox;
	}

	if (vDiv && vDiv.nodeName.toLowerCase() == 'div') vDivId = vDiv.id;
} //GanttChart

JSGantt.updateFlyingObj = function (e, pGanttChartObj, pTimer) {
	var vCurTopBuf = 3;
	var vCurLeftBuf = 5;
	var vCurBotBuf = 3;
	var vCurRightBuf = 15;
	var vMouseX = (e)?e.clientX:window.event.clientX;
	var vMouseY = (e)?e.clientY:window.event.clientY;
	var vViewportX=document.documentElement.clientWidth||document.getElementsByTagName('body')[0].clientWidth;
	var vViewportY=document.documentElement.clientHeight||document.getElementsByTagName('body')[0].clientHeight;
	var vNewX = vMouseX;
	var vNewY = vMouseY;

	if (navigator.appName.toLowerCase () == "microsoft internet explorer") {
		// the clientX and clientY properties include the left and top borders of the client area
		vMouseX -= document.documentElement.clientLeft;
		vMouseY -= document.documentElement.clientTop;

		var vZoomFactor = JSGantt.getZoomFactor ();
		if (vZoomFactor != 1) { // IE 7 at non-default zoom level
			vMouseX = Math.round (vMouseX / vZoomFactor);
			vMouseY = Math.round (vMouseY / vZoomFactor);
		}
	}

	var vScrollPos = JSGantt.getScrollPositions();

	/* Code for positioned right of the mouse by default*/
	/*
	if ( vMouseX + vCurRightBuf + pGanttChartObj.vTool.offsetWidth > vViewportX )
	{
		if ( vMouseX - vCurLeftBuf - pGanttChartObj.vTool.offsetWidth < 0) vNewX = vScrollPos.x ;
		else vNewX = vMouseX + vScrollPos.x - vCurLeftBuf - pGanttChartObj.vTool.offsetWidth ;
	}
	else vNewX = vMouseX + vScrollPos.x + vCurRightBuf ;
	*/

	/* Code for positioned left of the mouse by default */
	if ( vMouseX - vCurLeftBuf - pGanttChartObj.vTool.offsetWidth < 0)
	{
		if ( vMouseX + vCurRightBuf + pGanttChartObj.vTool.offsetWidth > vViewportX ) vNewX = vScrollPos.x ;
		else vNewX = vMouseX + vScrollPos.x + vCurRightBuf ;
	}
	else vNewX = vMouseX + vScrollPos.x - vCurLeftBuf - pGanttChartObj.vTool.offsetWidth ;

	/* Code for positioned below the mouse by default */
	if ( vMouseY + vCurBotBuf + pGanttChartObj.vTool.offsetHeight > vViewportY )
	{
		if (vMouseY - vCurTopBuf - pGanttChartObj.vTool.offsetHeight < 0) vNewY = vScrollPos.y ;
		else vNewY = vMouseY + vScrollPos.y - vCurTopBuf - pGanttChartObj.vTool.offsetHeight ;
	}
	else vNewY = vMouseY + vScrollPos.y + vCurBotBuf ;

	/* Code for positioned above the mouse by default */
	/*
	if ( vMouseY - vCurTopBuf - pGanttChartObj.vTool.offsetHeight < 0)
	{
		if ( vMouseY + vCurBotBuf + pGanttChartObj.vTool.offsetHeight > vViewportY ) vNewY = vScrollPos.y ;
		else vNewY = vMouseY + vScrollPos.y + vCurBotBuf ;
	}
	else vNewY = vMouseY + vScrollPos.y - vCurTopBuf - pGanttChartObj.vTool.offsetHeight ;
	*/

	if (pGanttChartObj.getUseMove())
	{
		clearInterval(pGanttChartObj.vTool.moveInterval);
		pGanttChartObj.vTool.moveInterval = setInterval(function(){JSGantt.moveToolTip(vNewX, vNewY, pGanttChartObj.vTool, pTimer)},pTimer);
	}
	else
	{
		pGanttChartObj.vTool.style.left = vNewX +'px';
		pGanttChartObj.vTool.style.top = vNewY +'px';
	}
}

JSGantt.showToolTip = function(pGanttChartObj, e, pContents, pWidth, pContType, pTimer){
	var vTtDivId = pGanttChartObj.getDivId() + 'JSGanttToolTip';
	var vMaxW = 500;
	var vMaxAlpha = 100;

	if(pGanttChartObj.getUseToolTip())
	{
		if(pGanttChartObj.vTool == null){
			pGanttChartObj.vTool = document.createElement('div');
			pGanttChartObj.vTool.id = vTtDivId;
			pGanttChartObj.vTool.className = 'JSGanttToolTip';
			pGanttChartObj.vTool.vToolCont = document.createElement('div');
			pGanttChartObj.vTool.vToolCont.id = vTtDivId + 'cont';
			pGanttChartObj.vTool.vToolCont.className = 'JSGanttToolTipcont';
			pGanttChartObj.vTool.vToolCont.setAttribute('showing','');
			pGanttChartObj.vTool.appendChild(pGanttChartObj.vTool.vToolCont);
			document.body.appendChild(pGanttChartObj.vTool);
			pGanttChartObj.vTool.style.opacity = 0;
			pGanttChartObj.vTool.setAttribute('currentOpacity',0);
			pGanttChartObj.vTool.setAttribute('fadeIncrement',10);
			pGanttChartObj.vTool.setAttribute('moveSpeed',10);
			pGanttChartObj.vTool.style.filter = 'alpha(opacity=0)';
			pGanttChartObj.vTool.style.display = 'none'
			pGanttChartObj.vTool.style.left = Math.floor(((e)?e.clientX:window.event.clientX)/2)+'px';
			pGanttChartObj.vTool.style.top = Math.floor(((e)?e.clientY:window.event.clientY)/2)+'px';
			JSGantt.addListener( 'mouseover', function () { clearTimeout(pGanttChartObj.vTool.delayTimeout); }, pGanttChartObj.vTool );
			JSGantt.addListener( 'mouseout', function () { JSGantt.delayedHide(pGanttChartObj, pGanttChartObj.vTool, pTimer); }, pGanttChartObj.vTool );
		}
		clearTimeout(pGanttChartObj.vTool.delayTimeout);
		if(pGanttChartObj.vTool.vToolCont.getAttribute('showing') != pContents || pGanttChartObj.vTool.style.display != 'block')
		{
			if ( pGanttChartObj.vTool.vToolCont.getAttribute('showing') == pContents )
			{
				pGanttChartObj.vTool.foundContent=true;
			}
			else
			{
				pGanttChartObj.vTool.vToolCont.setAttribute('showing',pContents);

				if (pContType == 'id')
				{
					if (JSGantt.findObj(pContents))
					{
						pGanttChartObj.vTool.vToolCont.innerHTML = JSGantt.findObj(pContents).innerHTML;
						// as we are allowing arbitrary HTML we should remove any tag ids to prevent duplication
						JSGantt.stripIds(pGanttChartObj.vTool.vToolCont);
						pGanttChartObj.vTool.foundContent=true;
					}
					else
					{
						pGanttChartObj.vTool.foundContent=false;
					}
				}
				else
				{
					pGanttChartObj.vTool.vToolCont.innerHTML = pContents;
					pGanttChartObj.vTool.foundContent=true;
				}
			}

			if ( pGanttChartObj.vTool.foundContent )
			{
				pGanttChartObj.vTool.style.display = 'block';
				// Rather than follow the mouse just have it stay put
				JSGantt.updateFlyingObj(e, pGanttChartObj, pTimer);
				pGanttChartObj.vTool.style.width = pWidth ? pWidth + 'px' : 'auto';
				if(!pWidth && JSGantt.isIE()){
					pGanttChartObj.vTool.style.width = pGanttChartObj.vTool.offsetWidth;
				}
				if(pGanttChartObj.vTool.offsetWidth > vMaxW){pGanttChartObj.vTool.style.width = vMaxW + 'px'}
				h = parseInt(pGanttChartObj.vTool.offsetHeight);
			}
		}
		else
		{ // just make sure it's still visible
			pGanttChartObj.vTool.foundContent=true;
		}

		if ( pGanttChartObj.vTool.foundContent )
		{
			if (pGanttChartObj.getUseFade())
			{
				clearInterval(pGanttChartObj.vTool.fadeInterval);
				pGanttChartObj.vTool.fadeInterval = setInterval(function(){JSGantt.fadeToolTip(1, pGanttChartObj.vTool, vMaxAlpha)},pTimer);
			}
			else
			{
				pGanttChartObj.vTool.style.opacity = vMaxAlpha * .01;
				pGanttChartObj.vTool.style.filter = 'alpha(opacity=' + vMaxAlpha + ')';
			}
		}
	}
}

JSGantt.stripIds = function(pNode){
	for(var i=0; i < pNode.childNodes.length; i++)
	{
		if ('removeAttribute' in pNode.childNodes[i]) pNode.childNodes[i].removeAttribute('id');
		if (pNode.childNodes[i].hasChildNodes()) JSGantt.stripIds(pNode.childNodes[i]);
	}
}

JSGantt.stripUnwanted = function(pNode){
	var vAllowedTags = new Array('#text','p','br','ul','ol','li','div','span','img');
	for(var i=0; i < pNode.childNodes.length; i++)
	{
		/* versions of IE < 9 don't support indexOf on arrays so add trailing comma to the joined array and lookup value to stop substring matches */
		if ((vAllowedTags.join().toLowerCase() + ',').indexOf(pNode.childNodes[i].nodeName.toLowerCase() + ',') ==-1 )
		{
			pNode.replaceChild(document.createTextNode(pNode.childNodes[i].outerHTML), pNode.childNodes[i]);
		}
		if (pNode.childNodes[i].hasChildNodes()) JSGantt.stripUnwanted(pNode.childNodes[i]);
	}
}

JSGantt.delayedHide = function(pGanttChartObj, pTool, pTimer){
	var vDelay = 1500;
	pTool.delayTimeout = setTimeout(function(){JSGantt.hideToolTip(pGanttChartObj, pTool, pTimer)}, vDelay)
}

JSGantt.hideToolTip = function(pGanttChartObj, pTool, pTimer){
	if (pGanttChartObj.getUseFade())
	{
		clearInterval(pTool.fadeInterval);
		pTool.fadeInterval = setInterval(function(){JSGantt.fadeToolTip(-1, pTool, 0)}, pTimer);
	}
	else pTool.style.display = 'none';
}

JSGantt.fadeToolTip = function(pDirection, pTool, pMaxAlpha){
	var vIncrement = parseInt(pTool.getAttribute('fadeIncrement'));
	var vAlpha = pTool.getAttribute('currentOpacity');
	var vCurAlpha = parseInt(vAlpha);
	if((vCurAlpha != pMaxAlpha && pDirection == 1) || (vCurAlpha != 0 && pDirection == -1)){
		var i = vIncrement;
		if(pMaxAlpha - vCurAlpha < vIncrement && pDirection == 1){
			i = pMaxAlpha - vCurAlpha;
		}else if(vAlpha < vIncrement && pDirection == -1){
			i = vCurAlpha;
		}
		vAlpha = vCurAlpha + (i * pDirection);
		pTool.style.opacity = vAlpha * .01;
		pTool.style.filter = 'alpha(opacity=' + vAlpha + ')';
		pTool.setAttribute('currentOpacity', vAlpha);
	}else{
		clearInterval(pTool.fadeInterval);
		if(pDirection == -1){pTool.style.display = 'none';}
	}
}

JSGantt.moveToolTip = function(pNewX, pNewY, pTool){
	var vSpeed = parseInt(pTool.getAttribute('moveSpeed'));
	vOldX = parseInt(pTool.style.left);
	vOldY = parseInt(pTool.style.top);

	if ( pTool.style.display != 'block' )
	{
		pTool.style.left = pNewX +'px';
		pTool.style.top = pNewY +'px';
		clearInterval(pTool.moveInterval);
	}
	else
	{
		if(pNewX != vOldX && pNewY != vOldY)
		{
			vOldX += Math.ceil((pNewX - vOldX)/vSpeed);
			vOldY += Math.ceil((pNewY - vOldY)/vSpeed);
			pTool.style.left = vOldX +'px';
			pTool.style.top = vOldY +'px';
		}
		else
		{
			clearInterval(pTool.moveInterval);
		}
	}
}

JSGantt.getZoomFactor = function() {
	var vFactor = 1;
	if (document.body.getBoundingClientRect)
	{
		// rect is only in physical pixel size in IE before version 8
		var vRect = document.body.getBoundingClientRect ();
		var vPhysicalW = vRect.right - vRect.left;
		var vLogicalW = document.body.offsetWidth;

		// the zoom level is always an integer percent value
		vFactor = Math.round ((vPhysicalW / vLogicalW) * 100) / 100;
	}
	return vFactor;
}

JSGantt.getScrollPositions = function() {
	if ('pageXOffset' in window)	// all browsers, except IE before version 9
	{
		var vScrollLeft = window.pageXOffset;
		var vScrollTop = window.pageYOffset;
	}
	else	// Internet Explorer before version 9
	{
		var vZoomFactor = JSGantt.getZoomFactor ();
		var vScrollLeft = Math.round (document.documentElement.scrollLeft / vZoomFactor);
		var vScrollTop = Math.round (document.documentElement.scrollTop / vZoomFactor);
	}
	return {x : vScrollLeft, y : vScrollTop};
}

JSGantt.getOffset = function(pStartDate, pEndDate, pColWidth, pFormat)
{
	var vMonthDaysArr = new Array(31,28,31,30,31,30,31,31,30,31,30,31);
	var curTaskStart = new Date(pStartDate.getTime());
	var curTaskEnd = new Date(pEndDate.getTime());
	var vTaskRightPx = 0;
	var tmpTaskStart = Date.UTC(curTaskStart.getFullYear(), curTaskStart.getMonth(), curTaskStart.getDate(), curTaskStart.getHours(), 0, 0);
	var tmpTaskEnd = Date.UTC(curTaskEnd.getFullYear(), curTaskEnd.getMonth(), curTaskEnd.getDate(), curTaskEnd.getHours(), 0, 0);

	vTaskRight = (tmpTaskEnd-tmpTaskStart)/3600000 // Length of task in hours

	if(pFormat == 'day')
	{
		vTaskRightPx = Math.ceil((vTaskRight/24) * (pColWidth + 1));
	}
	else if(pFormat == 'week')
	{
		vTaskRightPx = Math.ceil(((vTaskRight/24) * (pColWidth + 1))/7);
	}
	else if(pFormat == 'month')
	{
		var vMonthsDiff = (12 * (curTaskEnd.getFullYear() - curTaskStart.getFullYear())) + (curTaskEnd.getMonth() - curTaskStart.getMonth());
		var vPosTmpDate = new Date(curTaskEnd.getTime());
		vPosTmpDate.setDate(curTaskStart.getDate());
		var vDaysCrctn = (curTaskEnd.getTime()- vPosTmpDate.getTime())/ (86400000);

		vTaskRightPx = Math.ceil((vMonthsDiff * (pColWidth + 1)) + (vDaysCrctn * (pColWidth/vMonthDaysArr[curTaskEnd.getMonth()])));
	}
	else if(pFormat == 'quarter')
	{
		var vMonthsDiff = (12 * (curTaskEnd.getFullYear() - curTaskStart.getFullYear())) + (curTaskEnd.getMonth() - curTaskStart.getMonth());
		var vPosTmpDate = new Date(curTaskEnd.getTime());
		vPosTmpDate.setDate(curTaskStart.getDate());
		var vDaysCrctn = (curTaskEnd.getTime()- vPosTmpDate.getTime())/ (86400000);

		vTaskRightPx = Math.ceil((vMonthsDiff * ((pColWidth + 1)/3)) + (vDaysCrctn * (pColWidth/90)));
	}
	else if(pFormat == 'hour')
	{
		// can't just calculate sum because of daylight savings changes
		var vPosTmpDate = new Date(curTaskEnd.getTime());
		vPosTmpDate.setMinutes(curTaskStart.getMinutes(), 0);
		var vMinsCrctn = (curTaskEnd.getTime()- vPosTmpDate.getTime())/(3600000);

		vTaskRightPx = Math.ceil((vTaskRight * (pColWidth + 1)) + (vMinsCrctn * (pColWidth)));
	}
	return vTaskRightPx;
}

// Recursively process task tree ... set min, max dates of parent tasks and identfy task level.
JSGantt.processRows = function(pList, pID, pRow, pLevel, pOpen, pUseSort)
{
	var vMinDate = new Date();
	var vMaxDate = new Date();
	var vVisible = pOpen;
	var vCurItem = null;
	var vCompSum = 0;
	var vMinSet = 0;
	var vMaxSet = 0;
	var vNumKid = 0;
	var vLevel = pLevel;
	var vList = pList;
	var vComb = false;
	var i = 0;

	for(i = 0; i < pList.length; i++)
	{
		if (pList[i].getToDelete())
		{
			pList.splice(i,1);
			i--;
		}
		if (pList[i].getID()==pID)vCurItem = pList[i];
	}

	for(i = 0; i < pList.length; i++)
	{
		if(pList[i].getParent() == pID)
		{
			vVisible = pOpen;
			pList[i].setParItem(vCurItem);
			pList[i].setVisible(vVisible);
			if(vVisible==1 && pList[i].getOpen() == 0) vVisible = 0;

			if(pList[i].getMile() && pList[i].getParItem() && pList[i].getParItem().getGroup()==2)
			{ //remove milestones owned by combined groups
				pList.splice(i,1);
				i--;
				continue;
			}

			pList[i].setLevel(vLevel);
			vNumKid++;

			if(pList[i].getGroup())
			{
				if(pList[i].getParItem() && pList[i].getParItem().getGroup()==2)pList[i].setGroup(2);
				JSGantt.processRows(vList, pList[i].getID(), i, vLevel+1, vVisible, 0);
			}

			if( vMinSet==0 || pList[i].getStart() < vMinDate)
			{
				vMinDate = pList[i].getStart();
				vMinSet = 1;
			}

			if( vMaxSet==0 || pList[i].getEnd() > vMaxDate)
			{
				vMaxDate = pList[i].getEnd();
				vMaxSet = 1;
			}

			vCompSum += pList[i].getCompVal();
			pList[i].setSortIdx(i*pList.length);
		}
	}

	if(pRow >= 0)
	{
		pList[pRow].setStart(vMinDate);
		pList[pRow].setEnd(vMaxDate);
		pList[pRow].setNumKid(vNumKid);
		pList[pRow].setCompVal(Math.ceil(vCompSum/vNumKid));
	}

	if (pID == 0 && pUseSort == 1)
	{
		JSGantt.sortTasks(pList, 0, 0);
		pList.sort(function(a,b){return a.getSortIdx()-b.getSortIdx();});
	}
	if (pID == 0 && pUseSort != 1) // Need to sort combined tasks regardless
	{
		for(i = 0; i < pList.length; i++)
		{
			if (pList[i].getGroup()==2)
			{
				vComb=true;
				JSGantt.sortTasks(pList, pList[i].getID(), pList[i].getSortIdx()+1);
			}
		}
		if(vComb==true) pList.sort(function(a,b){return a.getSortIdx()-b.getSortIdx();});
	}
}

JSGantt.sortTasks = function (pList, pID, pIdx)
{
	var sortIdx = pIdx;
	var sortArr = new Array();

	for(var i=0; i < pList.length; i++)
	{
		if(pList[i].getParent() == pID)sortArr.push(pList[i]);
	}

	if (sortArr.length > 0)
	{
		sortArr.sort(function(a,b){ var i=a.getStart().getTime() - b.getStart().getTime();
									if (i==0) i=a.getEnd().getTime() - b.getEnd().getTime();
									if (i==0) return a.getID() - b.getID();
									else return i; })
	}

	for (var j=0; j < sortArr.length; j++)
	{
		for(i = 0; i < pList.length; i++)
		{
			if(pList[i].getID() == sortArr[j].getID())
			{
				pList[i].setSortIdx(sortIdx++);
				sortIdx = JSGantt.sortTasks(pList, pList[i].getID(), sortIdx)
			}
		}
	}
	return sortIdx;
}

// Used to determine the minimum date of all tasks and set lower bound based on format
JSGantt.getMinDate = function getMinDate(pList, pFormat)
{
	var vDate = new Date();
	vDate.setTime(pList[0].getStart().getTime());

	// Parse all Task End dates to find min
	for(i = 0; i < pList.length; i++)
	{
		if(pList[i].getStart().getTime() < vDate.getTime()) vDate.setTime(pList[i].getStart().getTime());
	}

	// Adjust min date to specific format boundaries (first of week or first of month)
	if (pFormat=='day')
	{
		vDate.setDate(vDate.getDate() - 1);
		while(vDate.getDay() % 7 != 1) vDate.setDate(vDate.getDate() - 1);
	}
	else if (pFormat=='week')
	{
		vDate.setDate(vDate.getDate() - 1);
		while(vDate.getDay() % 7 != 1) vDate.setDate(vDate.getDate() - 1);
	}
	else if (pFormat=='month')
	{
		vDate.setDate(vDate.getDate() - 15);
		while(vDate.getDate() > 1) vDate.setDate(vDate.getDate() - 1);
	}
	else if (pFormat=='quarter')
	{
		vDate.setDate(vDate.getDate() - 31);
		if( vDate.getMonth()==0 || vDate.getMonth()==1 || vDate.getMonth()==2 )
			vDate.setFullYear(vDate.getFullYear(), 0, 1);
		else if( vDate.getMonth()==3 || vDate.getMonth()==4 || vDate.getMonth()==5 )
			vDate.setFullYear(vDate.getFullYear(), 3, 1);
		else if( vDate.getMonth()==6 || vDate.getMonth()==7 || vDate.getMonth()==8 )
			vDate.setFullYear(vDate.getFullYear(), 6, 1);
		else if( vDate.getMonth()==9 || vDate.getMonth()==10 || vDate.getMonth()==11 )
			vDate.setFullYear(vDate.getFullYear(), 9, 1);
	}
	else if (pFormat=='hour')
	{
		vDate.setHours(vDate.getHours() - 1);
		while(vDate.getHours() % 6 != 0) vDate.setHours(vDate.getHours() - 1);
	}

	if(pFormat=='hour')vDate.setMinutes(0,0);
	else vDate.setHours(0,0,0);
	return(vDate);
}

// Used to determine the maximum date of all tasks and set upper bound based on format
JSGantt.getMaxDate = function (pList, pFormat)
{
	var vDate = new Date();

	vDate.setTime(pList[0].getEnd().getTime());

	// Parse all Task End dates to find max
	for(i = 0; i < pList.length; i++)
	{
		if(pList[i].getEnd().getTime() > vDate.getTime()) vDate.setTime(pList[i].getEnd().getTime());
	}

	// Adjust max date to specific format boundaries (end of week or end of month)
	if (pFormat=='day')
	{
		vDate.setDate(vDate.getDate() + 1);

		while(vDate.getDay() % 7 != 0) vDate.setDate(vDate.getDate() + 1);
	}
	else if (pFormat=='week')
	{
		//For weeks, what is the last logical boundary?
		vDate.setDate(vDate.getDate() + 1);

		while(vDate.getDay() % 7 != 0) vDate.setDate(vDate.getDate() + 1);
	}
	else if (pFormat=='month')
	{
		// Set to last day of current Month
		while(vDate.getDay() > 1) vDate.setDate(vDate.getDate() + 1);
		vDate.setDate(vDate.getDate() - 1);
	}
	else if (pFormat=='quarter')
	{
		// Set to last day of current Quarter
		if( vDate.getMonth()==0 || vDate.getMonth()==1 || vDate.getMonth()==2 )
			vDate.setFullYear(vDate.getFullYear(), 2, 31);
		else if( vDate.getMonth()==3 || vDate.getMonth()==4 || vDate.getMonth()==5 )
			vDate.setFullYear(vDate.getFullYear(), 5, 30);
		else if( vDate.getMonth()==6 || vDate.getMonth()==7 || vDate.getMonth()==8 )
			vDate.setFullYear(vDate.getFullYear(), 8, 30);
		else if( vDate.getMonth()==9 || vDate.getMonth()==10 || vDate.getMonth()==11 )
			vDate.setFullYear(vDate.getFullYear(), 11, 31);
	}
	else if (pFormat=='hour')
	{
		if(vDate.getHours()==0)vDate.setDate(vDate.getDate() + 1);
		vDate.setHours(vDate.getHours() + 1);

		while(vDate.getHours() % 6 != 5) vDate.setHours(vDate.getHours() + 1);
	}
	return(vDate);
}

// This function finds the document id of the specified object
JSGantt.findObj = function (theObj, theDoc)
{
	var p, i, foundObj;
	if(!theDoc) theDoc = document;
	if(document.getElementById) foundObj = document.getElementById(theObj);
	return foundObj;
}

JSGantt.changeFormat = function(pFormat,ganttObj)
{
	if(ganttObj) ganttObj.setFormat(pFormat);
	else alert('Chart undefined');
}

// Function to open/close and hide/show children of specified task
JSGantt.folder = function (pID,ganttObj)
{
	var vList = ganttObj.getList();
	var vDivId = ganttObj.getDivId();

	ganttObj.clearDependencies(); // clear these first so slow rendering doesn't look odd

	for(i = 0; i < vList.length; i++)
	{
		if(vList[i].getID() == pID)
		{
			if( vList[i].getOpen() == 1 )
			{
				vList[i].setOpen(0);
				JSGantt.hide(pID,ganttObj);

				if (JSGantt.isIE())
					JSGantt.findObj(vDivId+'group_'+pID).innerText = '+';
				else
					JSGantt.findObj(vDivId+'group_'+pID).textContent = '+';
			}
			else
			{
				vList[i].setOpen(1);

				JSGantt.show(pID, 1, ganttObj);

				if (JSGantt.isIE())
					JSGantt.findObj(vDivId+'group_'+pID).innerText = '-';
				else
					JSGantt.findObj(vDivId+'group_'+pID).textContent = '-';
			}
		}
	}
	ganttObj.DrawDependencies();
}

JSGantt.hide= function (pID,ganttObj)
{
	var vList = ganttObj.getList();
	var vID = 0;
	var vDivId = ganttObj.getDivId();

	for(var i = 0; i < vList.length; i++)
	{
		if(vList[i].getParent() == pID)
		{
			vID = vList[i].getID();
			// it's unlikely but if the task list has been updated since
			// the chart was drawn some of the rows may not exist
			if (JSGantt.findObj(vDivId+'child_'+vID)) JSGantt.findObj(vDivId+'child_'+vID).style.display = "none";
			if (JSGantt.findObj(vDivId+'childrow_'+vID)) JSGantt.findObj(vDivId+'childrow_'+vID).style.display = "none";
			vList[i].setVisible(0);
			if(vList[i].getGroup()) JSGantt.hide(vID,ganttObj);
		}
	}
}

// Function to show children of specified task
JSGantt.show = function (pID, pTop, ganttObj)
{
	var vList = ganttObj.getList();
	var vID = 0;
	var vDivId = ganttObj.getDivId();
	var vState = '';

	if (JSGantt.findObj(vDivId+'group_'+pID))
	{
		if (JSGantt.isIE()) vState=JSGantt.findObj(vDivId+'group_'+pID).innerText;
		else vState=JSGantt.findObj(vDivId+'group_'+pID).textContent;
	}

	for(var i = 0; i < vList.length; i++)
	{
		if(vList[i].getParent() == pID)
		{
			var vChgState = false;
			vID = vList[i].getID();

			if(pTop == 1 && vState == '+')vChgState=true;
			else if( vState == '-')vChgState=true;
			else if( vList[i].getParItem() && vList[i].getParItem().getGroup()==2 )vList[i].setVisible(1);

			if( vChgState )
			{
				if (JSGantt.findObj(vDivId+'child_'+vID)) JSGantt.findObj(vDivId+'child_'+vID).style.display = "";
				if (JSGantt.findObj(vDivId+'childrow_'+vID)) JSGantt.findObj(vDivId+'childrow_'+vID).style.display = "";
				vList[i].setVisible(1);
			}
			if(vList[i].getGroup()) JSGantt.show(vID, 0,ganttObj);
		}
	}
}

// function to open window to display task link
JSGantt.taskLink = function(pRef,pWidth,pHeight)
{

	if(pWidth) vWidth =pWidth; else vWidth =400;
	if(pHeight) vHeight=pHeight; else vHeight=400;

	var OpenWindow=window.open(pRef, "newwin", "height="+vHeight+",width="+vWidth);
}

JSGantt.parseDateStr = function(pDateStr,pFormatStr)
{
	var vDate = null;
	var vDateParts = pDateStr.split(/[^0-9]/);
	while(vDateParts.length < 5)vDateParts.push(0);

	switch(pFormatStr)
	{
		case 'mm/dd/yyyy':
		vDate = new Date(vDateParts[2], vDateParts[0] - 1, vDateParts[1], vDateParts[3], vDateParts[4]);
		break;
		case 'dd/mm/yyyy':
		vDate = new Date(vDateParts[2], vDateParts[1] - 1, vDateParts[0], vDateParts[3], vDateParts[4]);
		break;
		case 'yyyy-mm-dd':
		vDate = new Date(vDateParts[0], vDateParts[1] - 1, vDateParts[2], vDateParts[3], vDateParts[4]);
		break;
	}

	return(vDate);
}

JSGantt.formatDateStr = function( pDate, pDateFormatArr, pL )
{
	var vDateStr = '';

	vYear2Str = pDate.getFullYear().toString().substring(2,4);
	vMonthStr = (pDate.getMonth()+1) + '';
	vMonthArr = new Array(pL['january'],pL['february'],pL['march'],pL['april'],pL['maylong'],pL['june'],pL['july'],pL['august'],pL['september'],pL['october'],pL['november'],pL['december']);
	vDayArr = new Array(pL['sunday'],pL['monday'],pL['tuesday'],pL['wednesday'],pL['thursday'],pL['friday'],pL['saturday']);
	vMthArr = new Array(pL['jan'],pL['feb'],pL['mar'],pL['apr'],pL['may'],pL['jun'],pL['jul'],pL['aug'],pL['sep'],pL['oct'],pL['nov'],pL['dec']);
	vDyArr = new Array(pL['sun'],pL['mon'],pL['tue'],pL['wed'],pL['thu'],pL['fri'],pL['sat']);

	for (var i=0; i < pDateFormatArr.length; i++ )
	{
		switch( pDateFormatArr[i] )
		{
			case 'dd':
				if (pDate.getDate() < 10) vDateStr += '0'; // now fall through
			case 'd':
				vDateStr += pDate.getDate();
				break;
			case 'day':
				vDateStr += vDyArr[pDate.getDay()];
				break;
			case 'DAY':
				vDateStr += vDayArr[pDate.getDay()];
				break;
			case 'mm':
				if (vMonthStr < 10) vDateStr += '0'; // now fall through
			case 'm':
				vDateStr += vMonthStr;
				break;
			case 'mon':
				vDateStr += vMthArr[pDate.getMonth()];
				break;
			case 'month':
				vDateStr += vMonthArr[pDate.getMonth()];
				break;
			case 'yyyy':
				vDateStr += pDate.getFullYear();
				break;
			case 'yy':
				vDateStr += vYear2Str;
				break;
			case 'qq':
				vDateStr += 'Q'; // now fall through
			case 'q':
				vDateStr += Math.floor(pDate.getMonth()/3)+1;
				break;
			case 'hh':
				if ((((pDate.getHours()%12)==0)?12:pDate.getHours()%12)<10) vDateStr += '0'; // now fall through
			case 'h':
				vDateStr += ((pDate.getHours()%12)==0)?12:pDate.getHours()%12;
				break;
			case 'HH':
				if ((pDate.getHours()) < 10) vDateStr += '0'; // now fall through
			case 'H':
				vDateStr += (pDate.getHours());
				break;
			case 'MI':
				if (pDate.getMinutes()<10) vDateStr += '0'; // now fall through
			case 'mi':
				vDateStr += pDate.getMinutes();
				break;
			case 'pm':
				vDateStr += ((pDate.getHours()) < 12)?'am':'pm';
				break;
			case 'PM':
				vDateStr += ((pDate.getHours()) < 12)?'AM':'PM';
				break;
			case 'ww':
				if (JSGantt.getIsoWeek(pDate) < 10) vDateStr += '0'; // now fall through
			case 'w':
				vDateStr += JSGantt.getIsoWeek(pDate);
				break;
			case 'week':
				vDateStr += JSGantt.getIsoWeek(pDate);
				var vYear = pDate.getFullYear();
				var vDayOfWeek = ( pDate.getDay() == 0 )? 7 : pDate.getDay();
				if ( vWeekNum >= 52 && vMonthStr == 1 ) vYear--;
				if ( vWeekNum == 1 && vMonthStr == 12 ) vYear++;
				if (vWeekNum < 10) vWeekNum = '0' + vWeekNum;

				vDateStr += vYear + '-W' + vWeekNum + '-' + vDayOfWeek;
				break;
			default:
				vDateStr += pDateFormatArr[i];
				break;
		}
	}
	return vDateStr;
}

JSGantt.parseDateFormatStr = function( pFormatStr )
{
	var vDateStr = '';
	var vComponantStr = '';
	var vCurrChar = '';
	var vSeparators = new RegExp("[\/\\ -.,'\":]");
	var vDateFormatArray = new Array();

	for (var i=0; i < pFormatStr.length; i++ )
	{
		vCurrChar = pFormatStr.charAt(i);
		if ( (vCurrChar.match(vSeparators) ) || (i + 1 == pFormatStr.length) ) // separator or end of string
		{
			if ( (i + 1 == pFormatStr.length) && ( !(vCurrChar.match(vSeparators) ) ) ) // at end of string add any non-separator chars to the current component
			{
				vComponantStr += vCurrChar;
			}
			vDateFormatArray.push( vComponantStr );
			if ( vCurrChar.match(vSeparators) ) vDateFormatArray.push( vCurrChar );
			vComponantStr = '';
		}
		else
		{
			vComponantStr += vCurrChar;
		}

	}
	return vDateFormatArray;
}

JSGantt.parseXML = function(pFile,pGanttVar)
{
	if (window.XMLHttpRequest) {
		xhttp = new XMLHttpRequest();
	} else {	// IE 5/6
		xhttp = new ActiveXObject("Microsoft.XMLHTTP");
	}

	xhttp.open("GET", pFile, false);
	xhttp.send(null);
	xmlDoc = xhttp.responseXML;

	JSGantt.AddXMLTask(pGanttVar);

	xmlDoc=null; // a little tidying
	Task = null;
}

JSGantt.AddXMLTask = function(pGanttVar)
{
	Task=xmlDoc.getElementsByTagName("task");

	var n = xmlDoc.documentElement.childNodes.length;

	for(var i=0;i<n;i++)
	{
		// optional parameters may not have an entry
		// Task ID must NOT be zero otherwise it will be skipped
		try { pID = Task[i].getElementsByTagName("pID")[0].childNodes[0].nodeValue;
		} catch (error)
		{pID =0;}
		pID *= 1;	// make sure that these are numbers rather than strings in order to make jsgantt.js behave as expected.

		if(pID!=0)
		{
			try { pName = Task[i].getElementsByTagName("pName")[0].childNodes[0].nodeValue;
			} catch (error)
			{pName ="No Task Name";}			// If there is no corresponding entry in the XML file the set a default.

			try { pClass = Task[i].getElementsByTagName("pClass")[0].childNodes[0].nodeValue;
			} catch (error)
			{pClass ="ggroupblack";}

			try { pParent = Task[i].getElementsByTagName("pParent")[0].childNodes[0].nodeValue;
			} catch (error)
			{pParent =0;}
			pParent *= 1;

			try { pStart = Task[i].getElementsByTagName("pStart")[0].childNodes[0].nodeValue;
			} catch (error)
			{pStart ="";}

			try { pEnd = Task[i].getElementsByTagName("pEnd")[0].childNodes[0].nodeValue;
			} catch (error)
			{ pEnd ="";}

			try { pLink = Task[i].getElementsByTagName("pLink")[0].childNodes[0].nodeValue;
			} catch (error)
			{ pLink ="";}

			try { pMile = Task[i].getElementsByTagName("pMile")[0].childNodes[0].nodeValue;
			} catch (error)
			{ pMile=0;}
			pMile *= 1;

			try { pRes = Task[i].getElementsByTagName("pRes")[0].childNodes[0].nodeValue;
			} catch (error)
			{ pRes ="";}

			try { pComp = Task[i].getElementsByTagName("pComp")[0].childNodes[0].nodeValue;
			} catch (error)
			{pComp =0;}
			pComp *= 1;

			try { pGroup = Task[i].getElementsByTagName("pGroup")[0].childNodes[0].nodeValue;
			} catch (error)
			{pGroup =0;}
			pGroup *= 1;

			try { pOpen = Task[i].getElementsByTagName("pOpen")[0].childNodes[0].nodeValue;
			} catch (error)
			{ pOpen =1;}
			pOpen *= 1;

			try { pDepend = Task[i].getElementsByTagName("pDepend")[0].childNodes[0].nodeValue;
			} catch (error)
			{ pDepend ='';}
			if (pDepend.length==0)
			{pDepend=''} // need this to draw the dependency lines

			try { pCaption = Task[i].getElementsByTagName("pCaption")[0].childNodes[0].nodeValue;
			} catch (error)
			{ pCaption ="";}

			try { pNotes = Task[i].getElementsByTagName("pNotes")[0].childNodes[0].nodeValue;
			} catch (error)
			{ pNotes ="";}

			// Finally add the task
			pGanttVar.AddTaskItem(new JSGantt.TaskItem(pID , pName, pStart, pEnd, pClass, pLink, pMile, pRes, pComp, pGroup, pParent, pOpen, pDepend, pCaption, pNotes));
		}
	}
}


JSGantt.benchMark = function(pItem)
{
	var vEndTime=new Date().getTime();
	alert(pItem + ': Elapsed time: '+((vEndTime-vBenchTime)/1000)+' seconds.');
	vBenchTime=new Date().getTime();
}

JSGantt.getIsoWeek = function(pDate){
	// We have to compare against the monday of the first week of the year containing 04 jan *not* 01/01
	// 60*60*24*1000 = 86400000
	var dayMiliseconds = 86400000;
	var keyDay = new Date(pDate.getFullYear(),0,4,0,0,0);
	var keyDayOfWeek = (keyDay.getDay()==0) ? 6 : keyDay.getDay() - 1; // define monday as 0
	var firstMondayYearTime = keyDay.getTime() - (keyDayOfWeek * dayMiliseconds);
	var thisDate = new Date(pDate.getFullYear(), pDate.getMonth(),pDate.getDate(),0,0,0); // This at 00:00:00
	var thisTime = thisDate.getTime();
	var daysFromFirstMonday = Math.round(((thisTime - firstMondayYearTime) / dayMiliseconds));
	var lastWeek=99;
	var thisWeek=99;

	var firstMondayYear = new Date(firstMondayYearTime);

	thisWeek = Math.ceil((daysFromFirstMonday+1)/7);

	if ( thisWeek <= 0 ) thisWeek = JSGantt.getIsoWeek(new Date(pDate.getFullYear()-1,11,31,0,0,0));
	else if (thisWeek == 53 && (new Date(pDate.getFullYear(),0,1,0,0,0)).getDay() != 4 && (new Date(pDate.getFullYear(),11,31,0,0,0)).getDay() != 4 ) thisWeek = 1;
	return thisWeek;
}

JSGantt.addListener = function ( eventName, handler, control )
{
	// Check if control is a string
	if (control === String(control)) control = JSGantt.findObj(control);

	if(control.addEventListener) //Standard W3C
	{
		return control.addEventListener(eventName, handler, false);
	}
	else if (control.attachEvent) //IExplore
	{
		return control.attachEvent("on"+eventName, handler);
	}
	else
	{
		return false;
	}
}

JSGantt.addTootltipListeners = function(pGanttChart, pObj)
{
	var vTimer = 20;
	JSGantt.addListener( 'mouseover', function (e) { JSGantt.showToolTip(pGanttChart, e, 'tt' + pObj.id, null, 'id', vTimer); }, pObj );
	JSGantt.addListener( 'mouseout', function (e) { JSGantt.delayedHide(pGanttChart, pGanttChart.vTool, vTimer); }, pObj );
}

JSGantt.addThisRowListeners = function(pGanttChart, pObj1, pObj2)
{
	JSGantt.addListener( 'mouseover', function () { pGanttChart.mouseOver(pObj1, pObj2); }, pObj1 );
	JSGantt.addListener( 'mouseover', function () { pGanttChart.mouseOver(pObj1, pObj2); }, pObj2 );
	JSGantt.addListener( 'mouseout', function () { pGanttChart.mouseOut(pObj1, pObj2); }, pObj1 );
	JSGantt.addListener( 'mouseout', function () { pGanttChart.mouseOut(pObj1, pObj2); }, pObj2 );
}

JSGantt.addFolderListeners = function(pGanttChart, pObj, pID)
{
	JSGantt.addListener( 'click', function () { JSGantt.folder(pID, pGanttChart); }, pObj );
}

JSGantt.addFormatListeners = function(pGanttChart, pFormat, pId)
{
	JSGantt.addListener( 'click', function () { JSGantt.changeFormat(pFormat, pGanttChart); }, JSGantt.findObj(pId) );
}

JSGantt.addScrollListeners = function(pGanttChart)
{
	JSGantt.addListener( 'scroll', function () { JSGantt.findObj(pGanttChart.getDivId()+'gchartbody').scrollTop = JSGantt.findObj(pGanttChart.getDivId()+'glistbody').scrollTop; }, JSGantt.findObj(pGanttChart.getDivId()+'glistbody') );
	JSGantt.addListener( 'scroll', function () { JSGantt.findObj(pGanttChart.getDivId()+'glistbody').scrollTop = JSGantt.findObj(pGanttChart.getDivId()+'gchartbody').scrollTop; }, JSGantt.findObj(pGanttChart.getDivId()+'gchartbody') );
	JSGantt.addListener( 'scroll', function () { JSGantt.findObj(pGanttChart.getDivId()+'gcharthead').scrollLeft = JSGantt.findObj(pGanttChart.getDivId()+'gchartbody').scrollLeft; }, JSGantt.findObj(pGanttChart.getDivId()+'gchartbody') );
	JSGantt.addListener( 'scroll', function () { JSGantt.findObj(pGanttChart.getDivId()+'gchartbody').scrollLeft = JSGantt.findObj(pGanttChart.getDivId()+'gcharthead').scrollLeft; }, JSGantt.findObj(pGanttChart.getDivId()+'gcharthead') );
	JSGantt.addListener( 'resize', function () { JSGantt.findObj(pGanttChart.getDivId()+'gcharthead').scrollLeft = JSGantt.findObj(pGanttChart.getDivId()+'gchartbody').scrollLeft; }, window );
	JSGantt.addListener( 'resize', function () { JSGantt.findObj(pGanttChart.getDivId()+'glistbody').scrollTop = JSGantt.findObj(pGanttChart.getDivId()+'gchartbody').scrollTop; }, window );
}
