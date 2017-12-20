<%@ Control Language="C#" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Editor" TagPrefix="tools" %>

<script type="text/javascript">
	(function($, undefined) {
		Type.registerNamespace("Telerik.Web.UI.Dialogs");

		var $T = Telerik.Web.UI;
		var Dialogs = $T.Dialogs;

		Dialogs.MobileTableProperties = function(element) {
			Dialogs.MobileTableProperties.initializeBase(this, [element]);
		}

		Dialogs.MobileTableProperties.prototype = {
			initialize: function() {
				//make sure all base mobile dialog has been initialized, including ok/cancel events
				Dialogs.MobileTableProperties.callBaseMethod(this, "initialize");

				this.findUI();
			},
			clientInit: function(clientParameters) {
				this.updateUI(clientParameters);
			},

			collectResult: function() {
				var result = {
					width: this.getValue("width"),
					height: this.getValue("height"),
					padding: this.getValue("padding"),
					spacing: this.getValue("spacing")
				};

				return result;
			},

			findUI: function() {
				this.widthBox = $find("widthBox");
				this.heightBox = $find("heightBox");
				this.paddingBox = $find("paddingBox");
				this.spacingBox = $find("spacingBox");
			},

			updateUI: function(values) {
				this.setValue("width", values.width);
				this.setValue("height", values.height);
				this.setValue("padding", values.padding);
				this.setValue("spacing", values.spacing);
			},

			getValue: function(prop) {
				var box = this.getBox(prop);
				return box.get_value();
			},
			setValue: function(prop, value) {
				var box = this.getBox(prop);
				if(value || value === 0) {
					box.set_value(value);
				}
			},
			getBox: function(prop) {
				return this[prop + "Box"];
			},

			dispose: function() {
				this.widthBox = undefined;
				this.heightBox = undefined;
				this.paddingBox = undefined;
				this.spacingBox = undefined;

				Dialogs.MobileTableProperties.callBaseMethod(this, "dispose");
			}
		}

		Dialogs.MobileTableProperties.registerClass('Telerik.Web.UI.Dialogs.MobileTableProperties', Dialogs.MobileDialogBase, Telerik.Web.IParameterConsumer);
	})($telerik.$);
</script>
<ul class="reToolList reDialogSettings">
	<li>
		<span class="reToolText reToolLabel"><script>document.write(localization["Width"])</script></span>
		<span class="reToolValue reToolInput"><tools:EditorSpinBox ID="widthBox" runat="server" RenderMode="Mobile" /></span>
	</li>
	<li>
		<span class="reToolText reToolLabel"><script>document.write(localization["Height"])</script></span>
		<span class="reToolValue reToolInput"><tools:EditorSpinBox ID="heightBox" runat="server" RenderMode="Mobile" /></span>
	</li>
</ul>
<ul class="reToolList reDialogSettings">
	<li>
		<span class="reToolText reToolLabel"><script>document.write(localization["CellPadding"])</script></span>
		<span class="reToolValue reToolInput"><tools:EditorSpinBox ID="paddingBox" runat="server" RenderMode="Mobile" /></span>
	</li>
	<li>
		<span class="reToolText reToolLabel"><script>document.write(localization["CellSpacing"])</script></span>
		<span class="reToolValue reToolInput"><tools:EditorSpinBox ID="spacingBox" runat="server" RenderMode="Mobile" /></span>
	</li>
</ul>
