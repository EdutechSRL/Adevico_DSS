<%@ Control Language="C#" %>

<script type="text/javascript">
	(function($, undefined) {
		Type.registerNamespace("Telerik.Web.UI.Dialogs");

		var $T = Telerik.Web.UI;
		var Dialogs = $T.Dialogs;
		var delegate = Function.createDelegate;

		Dialogs.MobileLinkManager = function(element) {
			Dialogs.MobileLinkManager.initializeBase(this, [element]);
		}

		Dialogs.MobileLinkManager.prototype = {
			initialize: function() {
				Dialogs.MobileLinkManager.callBaseMethod(this, "initialize");

				this.collectUI();
				this.attachEventHandlers();
			},

			clientInit: function(clientParameters) {
				var link = this.link = clientParameters.value;

				this.updateLinkTextUI(clientParameters);
				this.urlInput.value = link.getAttribute("href") || "http:\/\/";
				this.initializeTargetSelect(link);
				this.updateRemoveLinkBtn(clientParameters);
			},

			initializeTargetSelect: function(link) {
				var target = link.target;
				var targetSelect = this.targetSelect;
				if (this._targetExists(target)) {
					targetSelect.value = target;
				} else {
					this._addCustomTarget(target);
				}
			},

			collectResult: function() {
				this.updateLink();

				var result = this.link;
				this.link = null;

				return result;
			},

			updateLink: function() {
				var link = this.link;
				var url = this.urlInput.value;
				var text = this.textInput.value || (link.showText && url);

				link.href = url;
				if (text) {
					setSanitizedText(link, text);
				}
				this.setLinkTarget();
			},

			setLinkTarget: function() {
				var link = this.link;
				var target = this.targetSelect.value;

				if (target) {
					link.setAttribute("target", target);
				}
				else {
					link.removeAttribute("target");
				}
			},

			attachEventHandlers: function() {
				this.targetChangeDelegate = delegate(this, this._targetChangeHandler);
				this.removeLinkDelegate = delegate(this, this._removeLinkHandler);

				$telerik.onEvent(this.targetSelect, "change", this.targetChangeDelegate);
				$telerik.onEvent(this.removeLinkBtn, "click", this.removeLinkDelegate);
			},

			detachEventHandlers: function() {
				$telerik.offEvent(this.targetSelect, "change", this.targetChangeDelegate);
				$telerik.offEvent(this.removeLinkBtn, "click", this.removeLinkDelegate);

				this.removeLinkDelegate = undefined;
			},

			dispose: function() {
				this.detachEventHandlers();

				this.link = undefined;
				this.urlInput = undefined;
				this.targetSelect = undefined;
				this.removeLinkBtn = undefined;
				this.linkTextContainer = undefined;
			},

			collectUI: function() {
				this.urlInput = $get("urlInput");
				this.textInput = $get("textInput");
				this.targetSelect = $get("targetSelect");
				this.removeLinkBtn = $get("removeLinkBtn");

				this.linkTextContainer = $get("linkTextContainer");
			},

			updateLinkTextUI: function(params) {
				var showText = params.showText;
				var display = showText ? "" : "none";

				this.linkTextContainer.style.display = display;
				this.textInput.value = showText ? this.link.innerHTML : "";
			},

			updateRemoveLinkBtn: function(args) {
				var canRemove = args.canUnlink;

				this.removeLinkBtn.disabled = !canRemove;
			},

			_targetChangeHandler: function(e) {
				var that = this,
					targetSelect = that.targetSelect;

				if (targetSelect.value == "_custom") {
					$(targetSelect).blur();
					window.setTimeout(function() {
						var targetprompt = window.prompt(localization["AddCustomTarget"], "CustomWindow");
						if (targetprompt) {
							that._addCustomTarget(targetprompt);
						} else {
							$(targetSelect).val("");
						}
					}, 0);
				}
			},

			_addCustomTarget: function(target) {
				var that = this,
					targetSelect = this.targetSelect;
				window.setTimeout(function() {
					if (!that._targetExists(target)) {
						$('option:last', targetSelect).before('<option value="' + target + '">' + target + '</option>');
					}
					$(targetSelect).val(target);
				}, 0);
			},

			_targetExists: function(target) {
				return target === "" || $('option[value="' + target + '"]', this.targetSelect).length > 0;
			},

			_removeLinkHandler: function(e) {
				$telerik.cancelRawEvent(e);
				closeDialog("remove");
			}
		}

		Dialogs.MobileLinkManager.registerClass('Telerik.Web.UI.Dialogs.MobileLinkManager', Dialogs.MobileDialogBase, Telerik.Web.IParameterConsumer);

		function setSanitizedText(node, text) {
			var textNode = document.createTextNode(text);

			node.innerHTML = "";
			node.appendChild(textNode);
		}

		function closeDialog(args) {
			Dialogs.CommonDialogScript.get_windowReference().close(args);
		}
	})($telerik.$);
</script>
<ul class="reToolList reDialogSettings">
	<li>
		<div class="reToolText reToolLabel"><script>document.write(localization["Url"])</script></div>
		<div class="reToolValue reToolInput"><input id="urlInput" type="text" value="http://" class="t-flex" /></div>
	</li>
	<li id="linkTextContainer">
		<div class="reToolText reToolLabel"><script>document.write(localization["LinkText"])</script></div>
		<div class="reToolValue reToolInput"><input id="textInput" type="text" class="t-flex" /></div>
	</li>
	<li>
		<div class="reToolText reToolLabel"><script>document.write(localization["LinkTarget"])</script></div>
		<div class="reToolValue reToolInput">
			<select id="targetSelect" class="reSelect t-flex">
				<option value="">None</option>
				<option value="_blank">Blank</option>
				<option value="_self">Self</option>
				<option value="_parent">Parent</option>
				<option value="_top">Top</option>
				<option value="_custom">
					<script>document.write(localization["AddCustomTarget"])</script>
				</option>
			</select>
		</div>
	</li>
	<li class="reToolBtn">
		<button id="removeLinkBtn" disabled="disabled"><script>document.write(localization["RemoveLink"])</script></button>
	</li>
</ul>