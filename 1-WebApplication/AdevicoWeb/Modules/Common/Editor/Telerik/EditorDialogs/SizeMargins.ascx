<%@ Control Language="C#" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Editor" TagPrefix="tools" %>

<script type="text/javascript">
	(function($, undefined) {
		Type.registerNamespace("Telerik.Web.UI.Dialogs");

		var $T = Telerik.Web.UI;
		var Dialogs = $T.Dialogs;
		var delegate = Function.createDelegate;

		var size = ["width", "height"];
		var boxes = size.concat(["marginTop", "marginBottom", "marginLeft", "marginRight"]);

		Dialogs.SizeMargins = function(element) {
			Dialogs.SizeMargins.initializeBase(this, [element]);
		}

		Dialogs.SizeMargins.prototype = {
			initialize: function() {
				//make sure all base mobile dialog has been initialized, including ok/cancel events
				Dialogs.SizeMargins.callBaseMethod(this, "initialize");

				this.findUI();
				this.attachEventHandlers();
			},
			clientInit: function(params) {
				this.updateUI(params);

				this.calculateRatio();
			},

			collectResult: function() {
				var dialog = this;
				var result = {};

				eachName(function(name) {
					result[name] = dialog.getValue(name);
				});

				return result;
			},

			attachEventHandlers: function() {
				var dialog = this;
				var ratioBtn = dialog.ratioBtn;

				Array.forEach(size, function(dim) {
					var box = dialog.getBox(dim);
					var boxDelegate = dialog[dim + "Delegate"] = delegate(dialog, dialog[dim + "Handler"]);

					box.add_valueSelected(boxDelegate);
				});

				dialog.ratioDelegate = delegate(dialog, dialog.ratioHandler);
				ratioBtn.add_checkedChanged(dialog.ratioDelegate);
			},

			detachEventHandlers: function() {
				var dialog = this;

				Array.forEach(size, function(dim) {
					var box = dialog.getBox(dim);
					var delegateName = dim + "Delegate";

					box.remove_valueSelected(dialog[delegateName]);
					dialog[delegateName] = undefined;
				});

				ratioBtn.remove_checkedChanged(dialog.ratioDelegate);
				dialog.ratioDelegate = undefined;
			},

			findUI: function() {
				var dialog = this;

				eachBoxName(function(name) {
					dialog[name] = $find(name);
				});

				this.ratioBtn = $find("ratioBtn");
			},

			updateUI: function(values) {
				var dialog = this;

				eachName(function(name) {
					dialog.setValue(name, values[name]);
				});
			},

			getValue: function(prop) {
				var box = this.getBox(prop);
				return int(box.get_value());
			},
			setValue: function(prop, value) {
				var box = this.getBox(prop);
				if(value || value === 0) {
					box.set_value(int(value));
				}
				else {
					box.set_value("");
				}
			},
			getBox: function(prop) {
				return this[boxName(prop)];
			},

			dispose: function() {
				this.detachEventHandlers();

				eachBoxName(function(name) {
					dialog[name] = undefined;
				});

				Dialogs.SizeMargins.callBaseMethod(this, "dispose");
			},

			widthHandler: function(spinBox, args) {
				if(!this.keepRatio()) {
					return;
				}
				var width = this.getValue("width");
				this.setValue("height", width / this.sizeRatio);

			},

			heightHandler: function() {
				if(!this.keepRatio()) {
					return;
				}
				var height = this.getValue("height");
				this.setValue("width", height * this.sizeRatio);
			},

			ratioHandler: function(btn, args) {
				if(btn.get_checked) {
					this.calculateRatio();
				}
			},

			keepRatio: function() {
				return this.ratioBtn.get_checked();
			},
			calculateRatio: function() {
				var width = this.getValue("width");
				var height = this.getValue("height");

				this.sizeRatio = width / height;
			}
		}

		Dialogs.SizeMargins.registerClass('Telerik.Web.UI.Dialogs.SizeMargins', Dialogs.MobileDialogBase, Telerik.Web.IParameterConsumer);

		function eachBoxName(callback) {
			eachName(function(prop) { callback(boxName(prop)); });
		}
		function eachName(callback) {
			Array.forEach(boxes, function(prop) { callback(prop); });
		}
		function boxName(prefix) {
			return prefix + "Box";
		}

		function int(n) { return parseInt(n, 10); }
	})($telerik.$);
</script>
<ul class="reToolList reDialogSettings">
	<li>
		<div class="reToolText reToolLabel"><script>document.write(localization["Width"])</script></div>
		<div class="reToolValue reToolInput"><tools:EditorSpinBox ID="widthBox" runat="server" RenderMode="Mobile" /></div>
		<telerik:RadButton ID="ratioBtn" CssClass="reRatioLinkButton" runat="server" ToggleType="CheckBox" Checked="true" RenderMode="Lightweight" AutoPostBack="false">
			<ToggleStates>
				<telerik:RadButtonToggleState PrimaryIconCssClass="reRatioLink" />
				<telerik:RadButtonToggleState PrimaryIconCssClass="reRatioUnlink" />
			</ToggleStates>
		</telerik:RadButton>
	</li>
	<li>
		<div class="reToolText reToolLabel"><script>document.write(localization["Height"])</script></div>
		<div class="reToolValue reToolInput"><tools:EditorSpinBox ID="heightBox" runat="server" RenderMode="Mobile" /></div>
	</li>
</ul>
<ul class="reToolList reDialogSettings">
	<li>
		<div class="reToolText reToolLabel"><script>document.write(localization["Top"])</script></div>
		<div class="reToolValue reToolInput"><tools:EditorSpinBox ID="marginTopBox" runat="server" RenderMode="Mobile" /></div>
	</li>
	<li>
		<div class="reToolText reToolLabel"><script>document.write(localization["Bottom"])</script></div>
		<div class="reToolValue reToolInput"><tools:EditorSpinBox ID="marginBottomBox" runat="server" RenderMode="Mobile" /></div>
	</li>
	<li>
		<div class="reToolText reToolLabel"><script>document.write(localization["Left"])</script></div>
		<div class="reToolValue reToolInput"><tools:EditorSpinBox ID="marginLeftBox" runat="server" RenderMode="Mobile" /></div>
	</li>
	<li>
		<div class="reToolText reToolLabel"><script>document.write(localization["Right"])</script></div>
		<div class="reToolValue reToolInput"><tools:EditorSpinBox ID="marginRightBox" runat="server" RenderMode="Mobile" /></div>
	</li>
</ul>
