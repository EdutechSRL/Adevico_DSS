<%@ Control Language="C#" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Editor" TagPrefix="tools" %>

<script type="text/javascript">
	(function($, undefined) {
		Type.registerNamespace("Telerik.Web.UI.Dialogs");

		var $T = Telerik.Web.UI;
		var Dialogs = $T.Dialogs;

		Dialogs.InsertTable = function(element) {
			Dialogs.InsertTable.initializeBase(this, [element]);
		}

		Dialogs.InsertTable.prototype = {
			initialize: function() {
				//make sure all base mobile dialog has been initialized, including ok/cancel events
				Dialogs.InsertTable.callBaseMethod(this, "initialize");

				this.findUI();
			},
			clientInit: function(clientParameters) {
				this.resetUI();
			},

			collectResult: function() {
				var result = {
					cols: this.getValue("columns"),
					rows: this.getValue("rows"),
					padding: this.getValue("padding"),
					spacing: this.getValue("spacing")
				};

				return result;
			},

			getValue: function(prop) {
				var box = this[prop + "Box"];
				return box.get_value();
			},

			findUI: function() {
				this.columnsBox = $find("columnsBox");
				this.rowsBox = $find("rowsBox");
				this.paddingBox = $find("paddingBox");
				this.spacingBox = $find("spacingBox");
			},

			resetUI: function() {
				this.columnsBox.set_value("");
				this.rowsBox.set_value("");
				this.paddingBox.set_value("");
				this.spacingBox.set_value("");
			},

			dispose: function() {
				this.columnsBox = undefined;
				this.rowsBox = undefined;
				this.paddingBox = undefined;
				this.spacingBox = undefined;

				Dialogs.InsertTable.callBaseMethod(this, "dispose");
			}
		}

		Dialogs.InsertTable.registerClass('Telerik.Web.UI.Dialogs.InsertTable', Dialogs.MobileDialogBase, Telerik.Web.IParameterConsumer);
	})($telerik.$);
</script>
<ul class="reToolList reDialogSettings">
	<li>
		<span class="reToolText reToolLabel"><script>document.write(localization["Columns"])</script></span>
		<span class="reToolValue reToolInput"><tools:EditorSpinBox ID="columnsBox" runat="server" RenderMode="Mobile" /></span>
	</li>
	<li>
		<span class="reToolText reToolLabel"><script>document.write(localization["Rows"])</script></span>
		<span class="reToolValue reToolInput"><tools:EditorSpinBox ID="rowsBox" runat="server" RenderMode="Mobile" /></span>
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
