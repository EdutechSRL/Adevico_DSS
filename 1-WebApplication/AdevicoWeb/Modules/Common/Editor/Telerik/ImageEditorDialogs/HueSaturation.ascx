<%@ Control Language="C#" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<table id="Table" class="rieDialogsTable rieHueSaturationTable" border="0" cellpadding="0" cellspacing="0"
	runat="server">
	<tr>
		<td>
			<asp:Literal ID="hueLiteral" runat="server">H</asp:Literal>:
		</td>
		<td>
			<telerik:RadSlider ID="hueSlider" runat="server" EnableViewState="false" MinimumValue="0" MaximumValue="360" Value="0" Width="180px" />
		</td>
		<td>
			<asp:TextBox ID="hueTxt" runat="server" Text="0" ToolTip="Hue" Width="35px" EnableViewState="false" />
		</td>
	</tr>
	<tr>
		<td>
			<asp:Literal ID="saturationLiteral" runat="server">S</asp:Literal>:
		</td>
		<td>
			<telerik:RadSlider ID="saturationSlider" runat="server" EnableViewState="false"  MinimumValue="-100" MaximumValue="100" Value="0" SelectedRegionStartValue="0" Width="180px" />
		</td>
		<td>
			<asp:TextBox ID="saturationTxt" runat="server" Text="0" ToolTip="Saturation" Width="35px" EnableViewState="false" />
		</td>
	</tr>
	<tr style="text-align: right;">
		<td colspan="3">
			<telerik:RadButton ID="applyBtn" runat="server" ToolTip="Apply" AutoPostBack="false"
				CausesValidation="false" EnableViewState="false" CommandName="Apply" Text="Apply" />
			
			<telerik:RadButton ID="resetBtn" runat="server" ToolTip="Reset" AutoPostBack="false"
				CausesValidation="false" EnableViewState="false" CommandName="Reset" Text="Reset" />
		</td>
	</tr>
</table>
<script type="text/javascript">
	//<![CDATA[
	//Register the class only if it has not been defined
	if (typeof (Telerik.Web.UI.ImageEditor.HueSaturation) === "undefined") {
		Type.registerNamespace("Telerik.Web.UI.ImageEditor");
		(function ($, $T, $IE) {
			var edgeValues = {
				b: { min: 0, max: 360 },
				c: { min: -100, max: 100 }
			};

			$IE.HueSaturation = function (imageEditor) {
				$IE.HueSaturation.initializeBase(this, [imageEditor]);
				this._editableImage = imageEditor.getEditableImage();
				this._storeOriginalPixels();
			};

			$IE.HueSaturation.prototype =
			{
				initialize: function () {
					$IE.HueSaturation.callBaseMethod(this, "initialize");
					this._initializeControls();

					this._attachHandlers(true);
					//decorate the textbox

					this.get_imageEditor().get_formDecorator().decorate(this._getControlFromParent("Table"));

					//We need a small timeout for the Slider to load the CSS and compute its dimensions
					this._repaintSlider(this._hueSlider);
					this._repaintSlider(this._saturationSlider);
				},

				apply: function () {
					this._ensureHueSaturationFilter();
					var filterOptions = this.hueSaturationFilter.options;
					filterOptions.initialData = null;
					var filterData = { hue: filterOptions.hue, saturation: filterOptions.saturation/*, initialData: this._collectEditablePixels()*/ };

					this.close();
					this._imageEditor.applyFilter(new $IE.Filters.HueSaturationFilter(filterData));
				},

				reset: function () {
					this._restoreOriginalPixels();
					this._resetSliders();
					this.hueSaturationFilter = null;
				},

				onOpen: function () {
					this._storeOriginalPixels();
					this.hueSaturationFilter = null;
					this._ensureHueSaturationFilter();
				},

				onClose: function () {
					this.reset();
					this._originalPixels = null;
				},

				get_hueSlider: function () { return this._hueSlider; },
				get_hueTxt: function () { return this._hueTxt; },

				get_saturationSlider: function () { return this._saturationSlider; },
				get_saturationTxt: function () { return this._saturationTxt; },

				get_applyButton: function () { return this._applyBtn; },
				get_resetButton: function () { return this._resetBtn; },

				dispose: function () {
					this._attachHandlers(false);
					$IE.HueSaturation.callBaseMethod(this, "dispose");
				},

				_initializeControls: function () {
					this._hueSlider = this._findControlFromParent("hueSlider");
					this._hueTxt = this._getControlFromParent("hueTxt");
					this._saturationSlider = this._findControlFromParent("saturationSlider");
					this._saturationTxt = this._getControlFromParent("saturationTxt");

					this._applyBtn = this._findControlFromParent("applyBtn");
					this._resetBtn = this._findControlFromParent("resetBtn");
				},

				_attachHandlers: function (toAttach) {
					if (toAttach) {
						this._hueSliderValueChangedDelegate = Function.createDelegate(this, this._hueSliderValueChanged);
						this._hueSlider.add_valueChanged(this._hueSliderValueChangedDelegate);

						this._hueTxtKeyboardDelegate = Function.createDelegate(this, this._hueTxtKeyboard);
						$telerik.addHandler(this._hueTxt, "keyup", this._hueTxtKeyboardDelegate);

						this._saturationSliderValueChangedDelegate = Function.createDelegate(this, this._saturationSliderValueChanged);
						this._saturationSlider.add_valueChanged(this._saturationSliderValueChangedDelegate);

						this._saturationTxtKeyboardDelegate = Function.createDelegate(this, this._saturationTxtKeyboard);
						$telerik.addHandler(this._saturationTxt, "keyup", this._saturationTxtKeyboardDelegate);

						this._applyClickDelegate = Function.createDelegate(this, this._applyClick);
						this._applyBtn.add_clicked(this._applyClickDelegate);

						this._resetClickDelegate = Function.createDelegate(this, this._resetClick);
						this._resetBtn.add_clicked(this._resetClickDelegate);
					}
					else {
						this._hueSlider.add_valueChanged(this._hueSliderValueChangedDelegate);
						this._resetBtn.remove_clicked(this._resetClickDelegate);
						$telerik.removeHandler(this._hueTxt, "keyup", this._hueTxtKeyboardDelegate);

						this._hueTxtKeyboardDelegate =
						this._hueSliderValueChangedDelegate =
						this._resetClickDelegate = null;
					}
				},

				_ensureHueSaturationFilter: function () {
					if (!this.hueSaturationFilter)
						this.hueSaturationFilter = new $IE.Filters.HueSaturationFilter({ initialData: this._collectEditablePixels() });
				},
				_hueSliderValueChanged: function (slider, args) {
					this._ensureHueSaturationFilter();

					var value = slider.get_value();
					this._changeUIToHueLevel(value);
					this._changeHueLevel(value);
				},

				_hueTxtKeyboard: function (event) {
					var value = this._getInputValueHandler(event, edgeValues.b);
					this._changeUIToHueLevel(value);
				},

				_changeUIToHueLevel: function (level) {
					this._hueTxt.value = level;
					this._hueSlider.set_value(level);
				},
				_changeHueLevel: function (level) {
					var filter = this.hueSaturationFilter;

					filter.options.hue = level;
					this._editableImage.executeFilter(filter);
				},

				_saturationSliderValueChanged: function (slider, args) {
					this._ensureHueSaturationFilter();

					var value = slider.get_value();
					this._changeUIToSaturationLevel(value);
					this._changeSaturationLevel(value);
				},

				_saturationTxtKeyboard: function (event) {
					var value = this._getInputValueHandler(event, edgeValues.c);
					this._changeUIToSaturationLevel(value);
				},

				_changeUIToSaturationLevel: function (level) {
					this._saturationTxt.value = level;
					this._saturationSlider.set_value(level);
				},
				_changeSaturationLevel: function (level) {
					var filter = this.hueSaturationFilter;

					filter.options.saturation = level;
					this._editableImage.executeFilter(filter);
				},

				_applyClick: function () {
					this.apply();
				},
				_resetClick: function () {
					this.reset();
				},
				_storeOriginalPixels: function () {
					this._originalPixels = this._collectEditablePixels();
				},
				_collectEditablePixels: function () {
					return this._editableImage.getImageDataAll();
				},
				_restoreOriginalPixels: function () {
					if (!this._originalPixels) return;

					var context = this._editableImage.getCanvasContext();
					context.putImageData(this._originalPixels, 0, 0);
				},

				_repaintSlider: function (slider) {
					window.setTimeout(function () {
						if (slider)
							slider.repaint();
					}, 200);
				},

				_resetSliders: function () {
					//should remove the event listening, otherwise incorrect filtering will be applied, which breaks the applying of the real filter
					this._hueSlider.remove_valueChanged(this._hueSliderValueChangedDelegate);
					this._changeUIToHueLevel(0);
					this._hueSlider.add_valueChanged(this._hueSliderValueChangedDelegate);

					this._saturationSlider.remove_valueChanged(this._saturationSliderValueChangedDelegate);
					this._changeUIToSaturationLevel(0);
					this._saturationSlider.add_valueChanged(this._saturationSliderValueChangedDelegate);
				},

				/*=================================  Telerik.Web.UI.ImageEditor.IToolWidged members  ===============================*/
				get_name: function () { return "HueSaturation"; },
				updateUI: function () {
					this._ensureHueSaturationFilter();
				}
				/*=================================  Telerik.Web.UI.ImageEditor.IToolWidged members  ===============================*/
			};
			$IE.HueSaturation.registerClass("Telerik.Web.UI.ImageEditor.HueSaturation", $IE.ToolWidget, $IE.IToolWidget);
		})($telerik.$, Telerik.Web.UI, Telerik.Web.UI.ImageEditor);
	}
	//]]>
</script>