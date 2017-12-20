<%@ Control Language="C#" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<table id="Table" class="rieDialogsTable rieBrightnessContrastTable" border="0" cellpadding="0" cellspacing="0"
	runat="server">
	<tr>
		<td>
			<asp:Literal ID="brightnessLiteral" runat="server">B</asp:Literal>:
		</td>
		<td>
			<telerik:RadSlider ID="brightnessSlider" runat="server" EnableViewState="false" MinimumValue="-100" MaximumValue="100" Value="0" Width="180px" />
		</td>
		<td>
			<asp:TextBox ID="brightnessTxt" runat="server" Text="0" ToolTip="Brightness" Width="35px" EnableViewState="false" />
		</td>
	</tr>
	<tr>
		<td>
			<asp:Literal ID="contrastLiteral" runat="server">C</asp:Literal>:
		</td>
		<td>
			<telerik:RadSlider ID="contrastSlider" runat="server" EnableViewState="false"  MinimumValue="-127" MaximumValue="127" Value="0" Width="180px" />
		</td>
		<td>
			<asp:TextBox ID="contrastTxt" runat="server" Text="0" ToolTip="Contrast" Width="35px" EnableViewState="false" />
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
	if (typeof (Telerik.Web.UI.ImageEditor.BrightnessContrast) === "undefined")
	{
		Type.registerNamespace("Telerik.Web.UI.ImageEditor");
		(function ($, $T, $IE)
		{
			var edgeValues = {
				b: { min: -100, max: 100 },
				c: { min: -127, max: 127 }
			};
			
			$IE.BrightnessContrast = function (imageEditor)
			{
				$IE.BrightnessContrast.initializeBase(this, [imageEditor]);
				this._editableImage = imageEditor.getEditableImage();
				this._storeOriginalPixels();
			};

			$IE.BrightnessContrast.prototype =
			{
				initialize: function()
				{
					$IE.BrightnessContrast.callBaseMethod(this, "initialize");
					this._initializeControls();
					
					this._attachHandlers(true);
					//decorate the textbox
					
					this.get_imageEditor().get_formDecorator().decorate(this._getControlFromParent("Table"));

					//We need a small timeout for the Slider to load the CSS and compute its dimensions
					this._repaintSlider(this._brightnessSlider);
					this._repaintSlider(this._contrastSlider);
				},

				apply: function()
				{
					this._ensureBrightnessContrastFilter();
					var filterOptions = this.brightnessContrastFilter.options;
					filterOptions.initialData = null;
					var filterData = {brightness: filterOptions.brightness, contrast: filterOptions.contrast/*, initialData: this._collectEditablePixels()*/};

					this.close();
					this._imageEditor.applyFilter(new $IE.Filters.BrightnessContrastFilter(filterData));
				},
				
				reset: function()
				{
					this._restoreOriginalPixels();
					this._resetSliders();
					this.brightnessContrastFilter = null;
				},
				
				onOpen: function()
				{
					this._storeOriginalPixels();
					this.brightnessContrastFilter = null;
					this._ensureBrightnessContrastFilter();
				},

				onClose: function()
				{
					this.reset();
					this._originalPixels = null;
				},

				get_brightnessSlider: function () { return this._brightnessSlider; },
				get_brightnessTxt: function() { return this._brightnessTxt; },

				get_contrastSlider: function () { return this._contrastSlider; },
				get_contrastTxt: function() { return this._contrastTxt; },

				get_applyButton: function() { return this._applyBtn; },
				get_resetButton: function() { return this._resetBtn; },

				dispose: function()
				{
					this._attachHandlers(false);
					$IE.BrightnessContrast.callBaseMethod(this, "dispose");
				},

				_initializeControls: function()
				{
					this._brightnessSlider = this._findControlFromParent("brightnessSlider");
					this._brightnessTxt = this._getControlFromParent("brightnessTxt");
					this._contrastSlider = this._findControlFromParent("contrastSlider");
					this._contrastTxt = this._getControlFromParent("contrastTxt");

					this._applyBtn = this._findControlFromParent("applyBtn");
					this._resetBtn = this._findControlFromParent("resetBtn");
				},

				_attachHandlers: function (toAttach)
				{
					if (toAttach)
					{
						this._brightnessSliderValueChangedDelegate = Function.createDelegate(this, this._brightnessSliderValueChanged);
						this._brightnessSlider.add_valueChanged(this._brightnessSliderValueChangedDelegate);
						
						this._brightnessTxtKeyboardDelegate = Function.createDelegate(this, this._brightnessTxtKeyboard);
						$telerik.addHandler(this._brightnessTxt, "keyup", this._brightnessTxtKeyboardDelegate);

						this._contrastSliderValueChangedDelegate = Function.createDelegate(this, this._contrastSliderValueChanged);
						this._contrastSlider.add_valueChanged(this._contrastSliderValueChangedDelegate);
						
						this._contrastTxtKeyboardDelegate = Function.createDelegate(this, this._contrastTxtKeyboard);
						$telerik.addHandler(this._contrastTxt, "keyup", this._contrastTxtKeyboardDelegate);

						this._applyClickDelegate = Function.createDelegate(this, this._applyClick);
						this._applyBtn.add_clicked(this._applyClickDelegate);

						this._resetClickDelegate = Function.createDelegate(this, this._resetClick);
						this._resetBtn.add_clicked(this._resetClickDelegate);
					}
					else
					{
						this._brightnessSlider.add_valueChanged(this._brightnessSliderValueChangedDelegate);
						this._resetBtn.remove_clicked(this._resetClickDelegate);
						$telerik.removeHandler(this._brightnessTxt, "keyup", this._brightnessTxtKeyboardDelegate);
						
						this._brightnessTxtKeyboardDelegate =
						this._brightnessSliderValueChangedDelegate =
						this._resetClickDelegate = null;
					}
				},

				_ensureBrightnessContrastFilter: function()
				{
					if(!this.brightnessContrastFilter)
						this.brightnessContrastFilter = new $IE.Filters.BrightnessContrastFilter({ initialData: this._collectEditablePixels() });
				},
				_brightnessSliderValueChanged: function(slider, args)
				{
					this._ensureBrightnessContrastFilter();
					
					var value = slider.get_value();
					this._changeUIToBrightnessLevel(value);
					this._changeBrightnessLevel(value);
				},

				_brightnessTxtKeyboard: function(event)
				{
					var value = this._getInputValueHandler(event, edgeValues.b);
					this._changeUIToBrightnessLevel(value);
				},

				_changeUIToBrightnessLevel: function(level)
				{
					this._brightnessTxt.value = level;
					this._brightnessSlider.set_value(level);
				},
				_changeBrightnessLevel: function(level)
				{
					var filter = this.brightnessContrastFilter;

					filter.options.brightness = level;
					this._editableImage.executeFilter(filter);
				},

				_contrastSliderValueChanged: function(slider, args)
				{
					this._ensureBrightnessContrastFilter();

					var value = slider.get_value();
					this._changeUIToContrastLevel(value);
					this._changeContrastLevel(value);
				},

				_contrastTxtKeyboard: function(event)
				{
					var value = this._getInputValueHandler(event, edgeValues.c);
					this._changeUIToContrastLevel(value);
				},

				_changeUIToContrastLevel: function(level)
				{
					this._contrastTxt.value = level;
					this._contrastSlider.set_value(level);
				},
				_changeContrastLevel: function(level)
				{
					var filter = this.brightnessContrastFilter;

					filter.options.contrast = this._normalizeContrastValue(level);
					this._editableImage.executeFilter(filter);
				},

				_applyClick: function()
				{
					this.apply();
				},
				_resetClick: function()
				{
					this.reset();
				},


				_normalizeContrastValue: function(value)
				{
					if(value == 127)
						return 127;
					else if(value > 0)
						return Math.pow(1.08, value - 67) + 1;
					else
						return 1 + (value/127); //formula 1 - (Math.abs(value)/127), but since value is negative we can skip the abs
				},

				_storeOriginalPixels: function()
				{
					this._originalPixels = this._collectEditablePixels();
				},
				_collectEditablePixels: function()
				{
					return this._editableImage.getImageDataAll();
				},
				_restoreOriginalPixels: function()
				{
					if(!this._originalPixels) return;

					var context = this._editableImage.getCanvasContext();
					context.putImageData(this._originalPixels, 0, 0);
				},

				_repaintSlider: function(slider)
				{
					window.setTimeout(function () {
						if (slider)
							slider.repaint();
					}, 200);
				},

				_resetSliders: function()
				{
					//should remove the event listening, otherwise incorrect filtering will be applied, which breaks the applying of the real filter
					this._brightnessSlider.remove_valueChanged(this._brightnessSliderValueChangedDelegate);
					this._changeUIToBrightnessLevel(0);
					this._brightnessSlider.add_valueChanged(this._brightnessSliderValueChangedDelegate);

					this._contrastSlider.remove_valueChanged(this._contrastSliderValueChangedDelegate);
					this._changeUIToContrastLevel(0);
					this._contrastSlider.add_valueChanged(this._contrastSliderValueChangedDelegate);
				},

				/*=================================  Telerik.Web.UI.ImageEditor.IToolWidged members  ===============================*/
				get_name: function () { return "BrightnessContrast"; },
				updateUI: function ()
				{
					this._ensureBrightnessContrastFilter();
				}
				/*=================================  Telerik.Web.UI.ImageEditor.IToolWidged members  ===============================*/
			};
			$IE.BrightnessContrast.registerClass("Telerik.Web.UI.ImageEditor.BrightnessContrast", $IE.ToolWidget, $IE.IToolWidget);
		})($telerik.$, Telerik.Web.UI, Telerik.Web.UI.ImageEditor);
	}
	//]]>
</script>
