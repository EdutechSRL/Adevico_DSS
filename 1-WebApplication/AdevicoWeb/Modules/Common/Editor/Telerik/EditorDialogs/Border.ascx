<%@ Control Language="C#" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Editor" TagPrefix="tools" %>

<script type="text/javascript">
	(function($, undefined) {
		Type.registerNamespace("Telerik.Web.UI.Dialogs");

		var $T = Telerik.Web.UI;
		var Dialogs = $T.Dialogs;
		var delegate = Function.createDelegate;
		var CHANGE = "change";

		Dialogs.Border = function(element) {
			Dialogs.Border.initializeBase(this, [element]);

			this._onWidhtSpinBoxValueSelectedDelegate = delegate(this, this._onWidhtSpinBoxValueSelected);
			this._onBorderStyleBoxValueSelectedDelegate = delegate(this, this._onBorderStyleBoxValueSelected);
			this._onColorValueSelectedDelegate = delegate(this, this._onColorValueSelected);
			this._result = {
				borderWidth: "",
				borderStyle: "",
				borderColor: ""
			}
		}

		Dialogs.Border.prototype = {
			initialize: function() {
				Dialogs.Border.callBaseMethod(this, "initialize");

				this.collectUI();
				this.attachEventHandlers();
				this.colorBox.set_currentColorText(localization["NoColor"]);
				this.colorBox.set_addCustomColorText(localization["AddCustomColor"]);
			},

			collectUI: function() {
				this.widthBox = $find("widhtSpinBox");
				this.colorBox = $find("colorBox");
				this.borderStyleBox = $get("borderStyleBox");
			},

			clientInit: function(clientParameters) {
				var element = this.element = clientParameters.value;
				var style = element.style;
				var color = style.borderColor;
				var width = style.borderWidth || 0;
				var borderStyle = style.borderStyle || "";

				this._result.borderWidth = width;
				this._result.borderColor = color;
				this._result.borderStyle = borderStyle;

				this.colorBox.set_items(clientParameters.Colors || []);
				this.colorBox.set_value(color);
				this.widthBox.set_value(parseInt(width, 10));
				this.borderStyleBox.value = borderStyle;
			},

			collectResult: function() {
				this.element = null;
				return this._result;
			},

			attachEventHandlers: function() {
				this.widthBox.add_valueSelected(this._onWidhtSpinBoxValueSelectedDelegate);
				this.colorBox.add_valueSelected(this._onColorValueSelectedDelegate);
				$(this.borderStyleBox).on(CHANGE, this._onBorderStyleBoxValueSelectedDelegate);
			},

			detachEventHandlers: function() {
				this.widthBox.remove_valueSelected(this._onWidhtSpinBoxValueSelectedDelegate);
				this.colorBox.remove_valueSelected(this._onColorValueSelectedDelegate);
				$(this.borderStyleBox).off(CHANGE, this._onBorderStyleBoxValueSelectedDelegate);
			},

			_onWidhtSpinBoxValueSelected: function(widthBox) {
				this._result.borderWidth = widthBox.get_value() + "px";
			},

			_onBorderStyleBoxValueSelected: function(){
				this._result.borderStyle = this.borderStyleBox.value;
			},

			_onColorValueSelected: function(colorBox) {
				this._result.borderColor = colorBox.get_value();
			},

			dispose: function() {
				this.detachEventHandlers();
				this.widthBox = undefined;
				this.borderStyleBox = undefined;
				this.colorPicker = undefined;
			}
		}

		Dialogs.Border.registerClass('Telerik.Web.UI.Dialogs.Border', Dialogs.MobileDialogBase, Telerik.Web.IParameterConsumer);
	})($telerik.$);
</script>
<style>
	.reDialogSettings {
		height: 85vh;
		overflow-y: auto;
	}

	.reMobileDialog select.reSelect {
		width: 193px;
	}
</style>
<ul class="reToolList reDialogSettings">
	<li>
		<div class="reToolText reToolLabel">
			<script>document.write(localization["Width"])</script>
		</div>
		<div class="reToolValue reToolInput">
			<tools:EditorSpinBox ID="widhtSpinBox" runat="server" RenderMode="Mobile" />
		</div>
	</li>
	<li>
		<div class="reToolText reToolLabel">
			<script>document.write(localization["BorderStyle"])</script>
		</div>
		<div class="reToolInput">
				<select id="borderStyleBox" class="reSelect">
					<option value=""><script>document.write(localization["NoStyle"])</script></option>
					<option value="none">None</option>
					<option value="solid">Solid</option>
					<option value="dotted">Dotted</option>
					<option value="dashed">Dashed</option>
					<option value="double">Double</option>
					<option value="groove">Groove</option>
					<option value="ridge">Ridge</option>
					<option value="inset">Inset</option>
					<option value="outset">Outset</option>
					<option value="hidden">Hidden</option>
					<option value="initial">Initial</option>
					<option value="inherit">Inherit</option>
				</select>
		</div>
	</li>
	<li class="reColorBoxWrapper">
		<tools:EditorColorList ID="colorBox" runat="server" RenderMode="Mobile"></tools:EditorColorList>
	</li>
</ul>