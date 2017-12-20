$(document).ready(function() {
	var tableSections = $("fieldset.section");
	tableSections.each(function(SecIndex, SectionValue) {
		new calcolaTotaliTabella().calcolaAndObserveList($(SectionValue).find(".tableReport"));
	});

	function calcolaTotaliTabella() {
		var _self = this;
		_self.arrTablesGroup = null;
		_self.input_TimerSync = null;
		_self.rootTable = null;
		_self.ObserveInput = function(thisInput, _TabIndex) {
			thisInput.unbind("keyup");
			thisInput.bind("keyup", function(e) {
				clearTimeout(_self.input_TimerSync);
				if(e.keyCode && (e.keyCode == "37" || e.keyCode == "39" || e.keyCode == "36" || e.keyCode == "38" || e.keyCode == "40"))
					return;
				_self.input_TimerSync = setTimeout(function() {
					try {
						var tempVal = thisInput.val().replace(',', '§').replace(/\,/g, '').replace('§', ',');
						thisInput.val(tempVal.replace(/[^0-9\,]/g, ''));
						var num = eval(thisInput.val() + "+0");
						// if is number
						thisInput.attr("title", "");
						thisInput.css('border', '');
						_self.calcolaAndObserveList(_self.arrTablesGroup);
					} catch (e) {
						thisInput.attr("title", "deve essere un numero");
						thisInput.css('border', 'solid 2px red');
					}
				}, 100);
			});
		};
		_self.calcolaAndObserve = function(thisTable, TabIndex) {
			_self.rootTable = thisTable;
			var trChildren = _self.rootTable.find("> tbody > tr");
			var summaryTotalResult = _self.rootTable.find("span.summaryTotal:first");
			summaryTotalResult.html("0");
			trChildren.each(function(trIndex, trValue) {
				var thisTr = $(trValue);
				var inputQ = thisTr.find("input.quantity:first");
				var inputU = thisTr.find("input.unitycost:first");
				_self.ObserveInput(inputQ, TabIndex);
				_self.ObserveInput(inputU, TabIndex);

				var totalInput = thisTr.find("span.total:first");
				if (totalInput.size() > 0 && inputQ && inputU && inputQ.val() && inputU.val()) {
					var _inputQ = _self.CleanInNumber(inputQ.val());
					var _inputU = _self.CleanInNumber(inputU.val());
					var myTotVal = _self.CleanInValue(eval(_inputQ + "*" + _inputU).toFixed(2));
					totalInput.html(myTotVal);
					myTotVal = _self.CleanInNumber(myTotVal);
					var _summaryTotalResult = _self.CleanInNumber(summaryTotalResult.html());
					var mySumTotVal = eval(_summaryTotalResult + "+" + myTotVal).toFixed(2);
					summaryTotalResult.html(_self.CleanInValue(mySumTotVal + ""));
				}else
					totalInput.html("0");
						
			});

			//_self.CalcolaSumDeiSum(TabIndex);
		};
		_self.CleanInNumber = function(value) {
			if (!value)
				return value;

			rtnValue = value;

			rtnValue = (rtnValue + "").replace(',', '.');
			rtnValue = (rtnValue + "").replace(/\,/g, '');

			return parseFloat(rtnValue);
		};
		_self.CleanInValue = function(value) {
			if (!value)
				return value;

			rtnValue = value;

			rtnValue = (rtnValue + "").replace('.', ',');
			rtnValue = (rtnValue + "").replace(/\./g, '');
			return rtnValue;
		};
		_self.CalcolaSumDeiSum = function(TabIndex) {//    if (TabIndex > 0) {
		//        var SumDeiSum = 0;
		//        for (var i = 0; i < _self.arrTablesGroup.length; i++) {
		//            if (i > TabIndex)
		//                break;

		//            var el = $(_self.arrTablesGroup[i]);
		//            var tempVal = (el.find("span.summaryTotal:first").html() + "").replace(/\,/g, '.');
		//            SumDeiSum = eval(SumDeiSum + "+" + tempVal).toFixed(2);
		//        }
		//        var tabRif = $(_self.arrTablesGroup[TabIndex]);
		//        var elNext = tabRif.next();
		//        if (elNext.hasClass("SumDeiSum"))
		//            elNext.html("Totale sezione: " + ((SumDeiSum + "").replace(/\./g, ',')) + "&euro;");
		//        else
		//            tabRif.after("<div class=\"SumDeiSum\">Totale Totale sezione: " + ((SumDeiSum + "").replace(/\./g, ',')) + "&euro;</div>");
		//    }
		};
		_self.calcolaAndObserveList = function(thisTableList) {
			if (!thisTableList || thisTableList.size() < 1)
				return;

			_self.arrTablesGroup = thisTableList;
			_self.arrTablesGroup.each(function(TabIndex, TabValue) {
				_self.calcolaAndObserve($(TabValue), TabIndex);
			});
		};
	}
});