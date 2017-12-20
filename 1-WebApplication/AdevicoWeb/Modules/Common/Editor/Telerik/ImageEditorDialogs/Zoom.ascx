<%@ Control Language="C#" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<table id="Table" class="rieDialogsTable rieZoomTable" border="0" cellpadding="0" cellspacing="0"
	runat="server">
	<tr>
		<td>
			<telerik:RadSlider ID="sliderZoom" runat="server" EnableViewState="false" MinimumValue="25"
				MaximumValue="400" Value="100" Width="180px">
			</telerik:RadSlider>
		</td>
		<td>
			<asp:TextBox ID="TxtZoom" runat="server" Text="100" ToolTip="Zoom" Width="35px" EnableViewState="false" />
			<asp:Label ID="lblPercent" Text="%" runat="server" AssociatedControlID="TxtZoom"
				Style="padding: 0 2px 0 2px" />
		</td>
	</tr>
	<tr>
		<td colspan="2">
			<telerik:RadButton ID="btnActualSize" runat="server" ToolTip="Actual Size" AutoPostBack="false"
				CausesValidation="false" EnableViewState="false" CommandName="ActualSize" Width="20px"
				CssClass="rieZIndex_IE9">
				<Icon PrimaryIconCssClass="rieActualSize" PrimaryIconTop="4px" PrimaryIconLeft="4px" />
			</telerik:RadButton>
			<label id="lblActualSize" runat="server">
				Actual Size</label>
			<telerik:RadButton ID="btnFitImage" runat="server" ToolTip="Best Fit" AutoPostBack="false"
				CausesValidation="false" EnableViewState="false" CommandName="BestFit" Width="20px"
				Style="margin-left: 7px;" CssClass="rieZIndex_IE9">
				<Icon PrimaryIconCssClass="rieBestFit" PrimaryIconTop="4px" PrimaryIconLeft="5px" />
			</telerik:RadButton>
			<label id="lblBestFit" runat="server">
				Best Fit</label>
		</td>
	</tr>
</table>
<script type="text/javascript">
	//<![CDATA[
	//Register the class only if it has not been defined
	if (typeof (Telerik.Web.UI.ImageEditor.Zoom) === "undefined")
	{
		Type.registerNamespace("Telerik.Web.UI.ImageEditor");
		(function ($, $T, $IE)
		{
			$IE.Zoom = function (imageEditor)
			{
				$IE.Zoom.initializeBase(this, [imageEditor]);

				this._actualSizeBtn = null;
				this._fitImageBtn = null;
				this._zoomSlider = null;
			}

			$IE.Zoom.prototype =
			{
				initialize: function ()
				{
					$IE.Zoom.callBaseMethod(this, "initialize");

					this._attachHandlers(true);
					//decorate the textbox
					this.get_imageEditor().get_formDecorator().decorate(this.get_textBox().parentNode);

					//We need a small timeout for the Slider to load the CSS and compute its dimensions
					var zoomSlider = this.get_zoomSlider();
					window.setTimeout(function () {
						if (zoomSlider) zoomSlider.repaint();
					}, 100);
				},

				dispose: function ()
				{
					this._attachHandlers(false);

					$IE.Zoom.callBaseMethod(this, "dispose");
				},

				//zooms the image to the specified zoom level
				zoom: function (zoomLevel)
				{
					var imageEditor = this.get_imageEditor();

					imageEditor._zoomImage(zoomLevel);
				},

				//gets the textbox that shows the current opacity on the image
				get_textBox: function ()
				{
					if (!this._textBox)
					{
						this._textBox = $get(this.get_parentId() + "TxtZoom");
						if (this._textBox)
							this._textBox.setAttribute("autoComplete", "off");
					}
					return this._textBox;
				},

				get_actualSizeBtn: function ()
				{
					if (!this._actualSizeBtn)
					{
						this._actualSizeBtn = $find(this.get_parentId() + "btnActualSize");
					}
					return this._actualSizeBtn;
				},

				get_fitImageBtn: function ()
				{
					if (!this._fitImageBtn)
					{
						this._fitImageBtn = $find(this.get_parentId() + "btnFitImage");
					}
					return this._fitImageBtn;
				},

				get_zoomSlider: function ()
				{
					if (!this._zoomSlider)
					{
						this._zoomSlider = $find(this.get_parentId() + "sliderZoom");
					}
					return this._zoomSlider;
				},

				get_max: function ()
				{
					var slider = this.get_zoomSlider();
					return slider.get_maximumValue();
				},

				get_min: function ()
				{
					var slider = this.get_zoomSlider();
					return slider.get_minimumValue();
				},

				_attachHandlers: function (toAttach)
				{
					var actualSizeBtn = this.get_actualSizeBtn();
					var fitImageBtn = this.get_fitImageBtn();
					var zoomSlider = this.get_zoomSlider();
					var txt = this.get_textBox();
					if (toAttach)
					{
						this._buttonClickDelegate = Function.createDelegate(this, this._buttonClick);
						if (actualSizeBtn) actualSizeBtn.add_clicked(this._buttonClickDelegate);
						if (fitImageBtn) fitImageBtn.add_clicked(this._buttonClickDelegate);

						if (zoomSlider)
						{
							this._valueChangedDelegate = Function.createDelegate(this, this._valueChanged);
							zoomSlider.add_valueChanged(this._valueChangedDelegate);
						}
						if (txt)
						{
							this._textBoxKeyDownDelegate = Function.createDelegate(this, this._textBoxKeyDown);
							$addHandler(txt, "keydown", this._textBoxKeyDownDelegate);
						}
					}
					else
					{
						if (actualSizeBtn) actualSizeBtn.remove_clicked(this._buttonClickDelegate);
						if (fitImageBtn) fitImageBtn.remove_clicked(this._buttonClickDelegate);
						this._buttonClickDelegate = null;

						if (zoomSlider)
						{
							zoomSlider.remove_valueChanged(this._valueChangedDelegate);
							this._valueChangedDelegate = null;
						}
						if (txt)
						{
							$removeHandler(txt, "keydown", this._textBoxKeyDownDelegate);
							this._textBoxKeyDownDelegate = null;
						}
					}
				},

				_buttonClick: function (sender, args)
				{
					var slider = this.get_zoomSlider();
					if (args.get_commandName() == "ActualSize") { slider.set_value(100); }
					else
					{
						var imageEditor = this.get_imageEditor();
						if (imageEditor) imageEditor.zoomBestFit();
					}
				},

				_valueChanged: function (sender, args)
				{
					this.zoom(sender.get_value());
					this.get_textBox().value = sender.get_value();
				},

				_textBoxKeyDown: function (args)
				{
					var keyCode = args.keyCode;
					var txt = this.get_textBox();
					var slider = this.get_zoomSlider();

					//if up arrow or down arrow pressed
					if (keyCode == 38 && this.incrementValue(txt, this.get_max())) { slider.set_value(txt.value); }
					else if (keyCode == 40 && this.decrementValue(txt, this.get_min())) { slider.set_value(txt.value); }

					if (keyCode == 13) //enter
					{
						var value = this.checkValue(txt, this.get_min(), this.get_max());
						if (value === false) { value = slider.get_value(); }
						else
						{
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

				/*=================================  Telerik.Web.UI.ImageEditor.IToolWidged members  ===============================*/
				get_name: function ()
				{
					return "Zoom";
				},

				updateUI: function ()
				{
					var imageEditor = this.get_imageEditor();
					var editableImage = imageEditor ? imageEditor.getEditableImage() : null;
					var value = editableImage ? editableImage.get_zoomLevel() : 100;
					var zoomSlider = this.get_zoomSlider()
					zoomSlider.set_value(value);
				}
				/*=================================  Telerik.Web.UI.ImageEditor.IToolWidged members  ===============================*/
			};
			$IE.Zoom.registerClass("Telerik.Web.UI.ImageEditor.Zoom", $IE.ToolWidget, $IE.IToolWidget);
		})($telerik.$, Telerik.Web.UI, Telerik.Web.UI.ImageEditor);
	}
	//]]>
</script>
