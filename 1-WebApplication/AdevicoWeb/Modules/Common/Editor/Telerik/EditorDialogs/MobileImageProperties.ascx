<%@ Control Language="C#" %>

<script type="text/javascript">
	(function($, undefined) {
		Type.registerNamespace("Telerik.Web.UI.Dialogs");

		var $T = Telerik.Web.UI;
		var Dialogs = $T.Dialogs;

		Dialogs.MobileImageProperties = function(element) {
			Dialogs.MobileImageProperties.initializeBase(this, [element]);
		}

		Dialogs.MobileImageProperties.prototype = {
			initialize: function() {
				Dialogs.MobileImageProperties.callBaseMethod(this, "initialize");

				this.collectUI();
			},

			clientInit: function(clientParameters) {
				this.updateUI(clientParameters);
			},

			collectResult: function() {
				var result = {
					alt: this.altInput.value,
					title: this.titleInput.value,
					longdesc: this.descriptionInput.value
				};

				return result;
			},

			dispose: function() {
				this.altInput = undefined;
				this.titleInput = undefined;
				this.descriptionInput = undefined;
			},

			collectUI: function() {
				this.altInput = $get("altInput");
				this.titleInput = $get("titleInput");
				this.descriptionInput = $get("descriptionInput");
			},
			
			updateUI: function(values) {
				var e_val = function(v) { return v || ""; }

				this.altInput.value = e_val(values.alt);
				this.titleInput.value = e_val(values.title);
				this.descriptionInput.value = e_val(values.description);
			}
		}

		Dialogs.MobileImageProperties.registerClass('Telerik.Web.UI.Dialogs.MobileImageProperties', Dialogs.MobileDialogBase, Telerik.Web.IParameterConsumer);
	})($telerik.$);
</script>
<ul class="reToolList reDialogSettings">
	<li>
		<span class="reToolText reToolLabel"><script>document.write(localization["ImageAltText"])</script></span>
		<span class="reToolValue reToolInput"><input id="altInput" type="text" class="t-flex" /></span>
	</li>
	<li id="linkTextContainer">
		<span class="reToolText reToolLabel"><script>document.write(localization["ImageTitleText"])</script></span>
		<span class="reToolValue reToolInput"><input id="titleInput" type="text" class="t-flex" /></span>
	</li>
	<li>
		<span class="reToolText reToolLabel"><script>document.write(localization["Description"])</script></span>
		<span class="reToolValue reToolInput"><input id="descriptionInput" type="text" class="t-flex" /></span>
	</li>
</ul>
