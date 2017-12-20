<%@ Control Language="C#" %>

<script type="text/javascript">
	(function($, undefined) {
		Type.registerNamespace("Telerik.Web.UI.Dialogs");

		var $T = Telerik.Web.UI;
		var Dialogs = $T.Dialogs;
		var delegate = Function.createDelegate;

		Dialogs.FindReplaceSettingsDialog = function(element)
		{
			Dialogs.FindReplaceSettingsDialog.initializeBase(this, [element]);

			this.mode = "find";
		}

		Dialogs.FindReplaceSettingsDialog.prototype = {
			initialize: function() {
				Dialogs.FindReplaceSettingsDialog.callBaseMethod(this, "initialize");

				this.findChildren();
				this.attachEventHandlers();
			},

			clientInit: function(clientParameters) {
				//clientParameters are passed from the Dialog Opener
			},

			collectResult: function() {
				var result = {
					mode: this.mode,
					matchCase: this.matchCaseCheck.checked
				};

				return result;
			},

			attachEventHandlers: function() {
				var modeNodes = this._getModeNodes();
				
				this.modesDelegate = delegate(this, this._modesHandler);

				for(var i = 0; i < modeNodes.length; i++) {
					$telerik.onEvent(modeNodes[i], "click", this.modesDelegate);
				}
			},

			detachEventHandlers: function() {
				var modeNodes = this._getModeNodes();

				for(var i = 0; i < modeNodes.length; i++) {
					$telerik.offEvent(modeNodes[i], "click", this.modesDelegate);
				}

				this.modesDelegate = undefined;
			},

			dispose: function() {
				this.detachEventHandlers();
			},

			findChildren: function() {
				this.modesList = $get("findReplaceMode");
				this.matchCaseCheck = $get("matchCaseCheckbox");
			},

			_modesHandler: function(e) {
				var modeNode = e.currentTarget;
				var selectedNode = this._getSelectedMode();

				if(modeNode != selectedNode[0]) {
					this._deselectMode(selectedNode);
					this._selectMode(modeNode);
				}

				this.mode = $(modeNode).data("mode");
			},

			_getModeNodes: function() {
				var list = $(this.modesList);
				return list.find("li");
			},

			_getSelectedMode: function() {
				var list = $(this.modesList);
				return list.find("li:has(span.reIconSelected)");
			},

			_getModeIcon: function(item) {
				return $(item).find("span.reIcon");
			},

			_selectMode: function(node) {
				var icon = this._getModeIcon(node);
				icon.addClass("reIconSelected");
			},

			_deselectMode: function(node) {
				var icon = this._getModeIcon(node);
				icon.removeClass("reIconSelected");
			}
		}

		Dialogs.FindReplaceSettingsDialog.registerClass('Telerik.Web.UI.Dialogs.FindReplaceSettingsDialog', Dialogs.MobileDialogBase, Telerik.Web.IParameterConsumer);
	})($telerik.$);
</script>
<ul id="findReplaceMode" class="reToolList reDialogSettings">
	<li data-mode="find">
		<span class="reToolText"><script>document.write(localization["Find"])</script></span>
		<span class="reIcon reIconSelected"></span>
	</li>
	<li data-mode="replace">
		<span class="reToolText"><script>document.write(localization["FindAndReplace"])</script></span>
		<span class="reIcon"></span>
	</li>
</ul>
<ul class="reToolList reDialogSettings">
	<li>
		<span class="reToolText"><script>document.write(localization["MatchCase"])</script></span>
		<input type="checkbox" id="matchCaseCheckbox" />
	</li>
</ul>
