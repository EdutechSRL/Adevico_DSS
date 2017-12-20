<%@ Control Language="C#" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<table id="Table" class="rieDialogsTable" border="0" cellpadding="0" cellspacing="0"
	runat="server">
	<tr>
		<td>
			<telerik:RadSlider ID="SliderOpacity" runat="server" EnableViewState="false" AutoPostBack="false"
				Value="100" Width="180px">
			</telerik:RadSlider>
		</td>
		<td>
			<asp:TextBox ID="TxtOpacity" runat="server" Text="100" ToolTip="Opacity" Width="35px"
				EnableViewState="false" />
			<asp:Label ID="lblPercent" Text="%" runat="server" AssociatedControlID="TxtOpacity"
				Style="padding: 0 2px 0 2px" />
		</td>
	</tr>
</table>
<script type="text/javascript">
	//<![CDATA[
	//Register the class only if it has not been defined
	if (typeof (Telerik.Web.UI.ImageEditor.Opacity) === "undefined")
	{
		Type.registerNamespace("Telerik.Web.UI.ImageEditor");
		(function ($, $T, $IE)
		{
			$IE.Opacity = function (imageEditor)
			{
				$IE.Opacity.initializeBase(this, [imageEditor]);

				this._slider = null;
				this._textbBox = null;
				this._shouldChangeOpacity = true;
			}

			$IE.Opacity.prototype =
			{
				initialize: function ()
				{
					$IE.Opacity.callBaseMethod(this, "initialize");

					this._attachHandlers(true);
					//decorate the textbox
					this.get_imageEditor().get_formDecorator().decorate(this.get_textBox().parentNode);

					//We need a small timeout for the Slider to load the CSS and compute its dimensions
					var slider = this.get_slider();
					window.setTimeout(function () {
						if (slider) slider.repaint();
					}, 100);
				},

				dispose: function ()
				{
					this._attachHandlers(false);

					$IE.Opacity.callBaseMethod(this, "dispose");
				},

				//changes the opacity on the image
				changeOpacity: function (opacity, addToStack)
				{
					if (this._shouldChangeOpacity)
					{
						var imageEditor = this.get_imageEditor();
						//Parameters: 1.The actual opacity to apply 2.Should we add the operation to the undoStack
						//3.Should we update the slider and the txt with the opacity
						imageEditor._changeImageOpacity(opacity, addToStack, false);
					}
					this._shouldChangeOpacity = true;
				},

				//gets the slider that changes the opacity on the image
				get_slider: function ()
				{
					if (!this._slider)
						this._slider = $find(this.get_parentId() + "SliderOpacity");
					return this._slider;
				},

				//gets the textbox that shows the current opacity on the image
				get_textBox: function ()
				{
					if (!this._textBox)
					{
						this._textBox = $get(this.get_parentId() + "TxtOpacity");
						if (this._textBox)
							this._textBox.setAttribute("autoComplete", "off");
					}
					return this._textBox;
				},

				_attachHandlers: function (toAttach)
				{
					var slider = this.get_slider();
					var txt = this.get_textBox();
					if (toAttach)
					{
						if (slider)
						{
							this._sliderValueChangedDelegate = Function.createDelegate(this, this._sliderValueChanged);
							slider.add_valueChanged(this._sliderValueChangedDelegate);
							this._sliderSlideEndDelegate = Function.createDelegate(this, this._sliderSlideEnd);
							slider.add_slideEnd(this._sliderSlideEndDelegate);
							this._sliderSlideStartDelegate = Function.createDelegate(this, this._sliderSlideStart);
							slider.add_slideStart(this._sliderSlideStartDelegate);
						}
						if (txt)
						{
							this._textBoxKeyDownDelegate = Function.createDelegate(this, this._textBoxKeyDown);
							$addHandler(txt, "keydown", this._textBoxKeyDownDelegate);
							this._textBoxKeyUpDelegate = Function.createDelegate(this, this._textBoxKeyUp);
							$addHandler(txt, "keyup", this._textBoxKeyUpDelegate);
						}
					}
					else
					{
						if (slider)
						{
							slider.remove_valueChanged(this._sliderValueChangedDelegate);
							this._sliderValueChangedDelegate = null;
							slider.remove_slideEnd(this._sliderSlideEndDelegate);
							this._sliderSlideEndDelegate = null;
							slider.remove_slideStart(this._sliderSlideStartDelegate);
							this._sliderSlideStartDelegate = null;
						}
						if (txt)
						{
							$removeHandler(txt, "keydown", this._textBoxKeyDownDelegate);
							this._textBoxKeyDownDelegate = null;
							$removeHandler(txt, "keyup", this._textBoxKeyUpDelegate);
							this._textBoxKeyUpDelegate = null;
						}
					}
				},

				_sliderValueChanged: function (sender, args)
				{
					var value = sender.get_value();
					this.get_textBox().value = value;
					this.changeOpacity(value, (!this._doNotAddToStack));
				},

				_sliderSlideStart: function ()
				{
					this._doNotAddToStack = true;
				},

				_sliderSlideEnd: function (sender, args)
				{
					var value = sender.get_value();
					this.changeOpacity(value, true);
					this._doNotAddToStack = false;
				},

				_textBoxKeyDown: function (args)
				{
					this._doNotAddToStack = true;
					this._isTxtValueChanged = false;

					var keyCode = args.keyCode;
					var txt = this.get_textBox();
					var slider = this.get_slider();
					if (keyCode == 38 && this.incrementValue(txt, 100)) //up arrow
					{
						slider.set_value(txt.value);
						this._isTxtValueChanged = true;
					}
					else if (keyCode == 40 && this.decrementValue(txt, 0)) //down arrow
					{
						slider.set_value(txt.value);
						this._isTxtValueChanged = true;
					}

					if (keyCode == 13) //enter
					{
						var value = this.checkValue(txt, 0, 100);
						if (value === false) { value = slider.get_value(); }
						else
						{
							if (value != slider.get_value())
								this._isTxtValueChanged = true;
							slider.set_value(value);
						}
						txt.value = value;

						//Cancel the form submission
						$telerik.cancelRawEvent(args.rawEvent);
						return false;
					}

					if (keyCode == 27) //if escape cancel
					{
						$telerik.cancelRawEvent(args.rawEvent);
						return false;
					}
				},

				_textBoxKeyUp: function (args)
				{
					this._doNotAddToStack = false;
					if (this._isTxtValueChanged) this.changeOpacity(this.get_textBox().value, true);
				},

				/*=================================  Telerik.Web.UI.ImageEditor.IToolWidged members  ===============================*/
				get_name: function ()
				{
					return "Opacity";
				},

				updateUI: function ()
				{
					var editableImage = this.get_imageEditor().getEditableImage();
					var opacity = editableImage ? editableImage.get_opacity() : 100;
					this._updateTxtAndSlider(opacity);
				},
				/*=================================  Telerik.Web.UI.ImageEditor.IToolWidged members  ===============================*/

				_updateTxtAndSlider: function (value)
				{
					var slider = this.get_slider();
					this._shouldChangeOpacity = (value == slider.get_value());
					slider.set_value(value);
					this.get_textBox().value = value;
				}
			};
			$IE.Opacity.registerClass("Telerik.Web.UI.ImageEditor.Opacity", $IE.ToolWidget, $IE.IToolWidget);
		})($telerik.$, Telerik.Web.UI, Telerik.Web.UI.ImageEditor);
	}
	//]]>
</script>
