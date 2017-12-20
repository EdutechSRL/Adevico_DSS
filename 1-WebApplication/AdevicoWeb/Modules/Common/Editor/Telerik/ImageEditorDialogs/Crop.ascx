<%@ Control Language="C#" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<table id="Table" class="rieDialogsTable" border="0" cellpadding="0" cellspacing="0"
	runat="server">
	<tr>
		<td class="rieRightAligned">
			<label id="lblAspectRatio" runat="server">
				Aspect Ratio:</label>
		</td>
		<td colspan="2">
			<telerik:RadComboBox ID="rieAspectRatio" runat="server" AutoPostBack="false" EnableViewState="false"
				CausesValidation="false" Width="140px">
				<Items>
					<telerik:RadComboBoxItem Text="1024x768px" Value="1024,768" />
					<telerik:RadComboBoxItem Text="800x600px" Value="800,600" />
					<telerik:RadComboBoxItem Text="1440x900px" Value="1440,900" />
					<telerik:RadComboBoxItem Text="1280x1024px" Value="1280,1024" />
				</Items>
			</telerik:RadComboBox>
		</td>
	</tr>
	<tr>
		<td class="rieRightAligned">
			<asp:Label ID="lblWidth" Text="Width:" runat="server" AssociatedControlID="txtWidth" />
		</td>
		<td class="rieInputDimension">
			<asp:TextBox ID="txtWidth" runat="server" ToolTip="Width" Width="45px" />
			<asp:Literal ID="Literal1" Text=" px" runat="server" />
		</td>
		<td rowspan="2">
			<div class="rieResizeButtons">
				<telerik:RadButton ID="btnConstraint" runat="server" ToolTip="Constrain Proportions"
					ToggleType="CheckBox" Checked="true" Width="20px" EnableViewState="false" AutoPostBack="false"
					CausesValidation="false">
					<ToggleStates>
						<telerik:RadButtonToggleState PrimaryIconCssClass="rieConstrainBtn" PrimaryIconTop="3px" />
						<telerik:RadButtonToggleState PrimaryIconCssClass="rieRemoveConstrainBtn" PrimaryIconTop="4px" />
					</ToggleStates>
				</telerik:RadButton>
				<telerik:RadButton ID="btnSwap" runat="server" ToolTip="Swap Values" AutoPostBack="false"
					CausesValidation="false" EnableViewState="false" Width="20px">
					<Icon PrimaryIconCssClass="rieSwapValues" PrimaryIconTop="4px" PrimaryIconLeft="4px" />
				</telerik:RadButton>
			</div>
		</td>
	</tr>
	<tr>
		<td class="rieRightAligned">
			<asp:Label ID="lblHeight" Text="Height:" runat="server" AssociatedControlID="txtHeight" />
		</td>
		<td class="rieInputDimension">
			<asp:TextBox ID="txtHeight" runat="server" ToolTip="Height" Width="45px" />
			<asp:Literal ID="Literal4" Text=" px" runat="server" />
		</td>
	</tr>
	<tr>
		<td class="rieRightAligned">
			<asp:Literal ID="lPosition" Text="Position:" runat="server" />
		</td>
		<td colspan="2">
			<asp:Label ID="lblX" Text="X " runat="server" AssociatedControlID="txtX" />
			<asp:TextBox ID="txtX" runat="server" ToolTip="Left" Width="35px" />
			<asp:Literal ID="Literal2" Text=" px" runat="server" />
			&nbsp;&nbsp;&nbsp;
			<asp:Label ID="lblY" Text="Y " runat="server" AssociatedControlID="txtY" />
			<asp:TextBox ID="txtY" runat="server" ToolTip="Left" Width="35px" />
			<asp:Literal ID="Literal3" Text=" px" runat="server" />
		</td>
	</tr>
	<tr>
		<td class="rieRightAligned" colspan="3">
			<telerik:RadButton ID="btnApply" runat="server" Text="Crop" ToolTip="Crop" AutoPostBack="false"
				CausesValidation="false" EnableViewState="false" CommandName="Crop">
			</telerik:RadButton>
			<telerik:RadButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" AutoPostBack="false"
				CausesValidation="false" EnableViewState="false" CommandName="Cancel">
			</telerik:RadButton>
		</td>
	</tr>
</table>
<script type="text/javascript">
	//<![CDATA[
	//Register the class only if it has not been defined
	if (typeof (Telerik.Web.UI.ImageEditor.Crop) === "undefined")
	{
		Type.registerNamespace("Telerik.Web.UI.ImageEditor");
		(function ($, $T, $IE)
		{
			$IE.Crop = function (imageEditor)
			{
				$IE.Crop.initializeBase(this, [imageEditor]);
			}
			$IE.Crop.prototype =
			{
				initialize: function ()
				{
					$IE.Crop.callBaseMethod(this, "initialize");

					this._xTxt = this._getControlFromParent("txtX");
					this._yTxt = this._getControlFromParent("txtY");
					this._widthTxt = this._getControlFromParent("txtWidth");
					this._heightTxt = this._getControlFromParent("txtHeight");
					this._presetDD = this._findControlFromParent("rieAspectRatio");
					this._cropBtn = this._findControlFromParent("btnApply");
					this._cancelBtn = this._findControlFromParent("btnCancel");
					this._constraintBtn = this._findControlFromParent("btnConstraint");
					this._swapBtn = this._findControlFromParent("btnSwap");
					this._sizeRatio = null;
					this._zoomLevel = this._getImageZoomLevel();

					this._createCropBox();
					this._updateControlsFromCropBox();
					this._constraintBtnClick(this._constraintBtn);
					this._attachHandlers(true);
					this.get_imageEditor().get_formDecorator().decorate($get(this.get_parentId() + "Table"));
				},

				dispose: function ()
				{
					this._attachHandlers(false);
					this._disposeCropBox();

					$IE.Crop.callBaseMethod(this, "dispose");
				},

				onClose: function ()
				{
					this._cropBox.hide();
					this._presetDD.hideDropDown();
				},

				onOpen: function ()
				{
					try
					{
						this._cropBox.show();

						var pos = this._getViewportScroll();
						this._xTxt.value = pos.x;
						this._yTxt.value = pos.y;
						this._updateCropBoxFromControls();
					}
					catch (e) { }
				},

				crop: function (bounds)
				{
					if (bounds == false) return;

					this.get_imageEditor()._cropImage(bounds, true);
					var updatedBounds = new Sys.UI.Bounds(0, 0, bounds.width, bounds.height);
					this._updateControlsWithFixedBounds(updatedBounds);
					this._updateCropBoxFromControls();
					this._setCropBoxConstraints(this._toScaledBounds(updatedBounds));
				},

				set_width: function (value)
				{
					var imageSize = this.get_imageSize();
					var bounds = this._cropBox.getBounds();
					value = this._restrictValue(value, 0, imageSize.width - bounds.x);
					value = this._applyWidthConstraintRatio(value, imageSize.height - bounds.y);

					this.set_inputValue(this._widthTxt, value);
				},
				set_height: function (value)
				{
					var imageSize = this.get_imageSize();
					var bounds = this._cropBox.getBounds();
					value = this._restrictValue(value, 0, imageSize.height - bounds.y);
					value = this._applyHeightConstraintRatio(value, imageSize.width - bounds.y);

					this.set_inputValue(this._heightTxt, value);
				},
				set_x: function (value)
				{
					var imageSize = this.get_imageSize();
					var bounds = this._cropBox.getBounds();
					value = this._restrictValue(value, 0, imageSize.width - bounds.width);

					this.set_inputValue(this._xTxt, value);
				},
				set_y: function (value)
				{
					var imageSize = this.get_imageSize();
					var bounds = this._cropBox.getBounds();
					value = this._restrictValue(value, 0, imageSize.height - bounds.height);

					this.set_inputValue(this._yTxt, value);
				},
				set_inputValue: function (input, value)
				{
					input.value = parseInt(value);
					this._updateCropBoxFromControls();
				},
				_restrictValue: function (value, min, max)
				{
					return Math.max(min, Math.min(max, value));
				},

				get_sizeRatio: function ()
				{
					return this._sizeRatio;
				},

				get_freshSizeRatio: function ()
				{
					return parseInt(this._widthTxt.value) / parseInt(this._heightTxt.value);
				},

				get_imageSize: function ()
				{
					var image = this.get_imageEditor().getEditableImage();
					return { width: image.get_width(), height: image.get_height() };
				},

				_applyWidthConstraintRatio: function (value, maxHeight)
				{
					if (this._constraintBtn.get_checked())
					{
						var ratio = this.get_sizeRatio();
						var height = this._restrictValue(parseInt(value / ratio), 0, maxHeight);
						this._heightTxt.value = height;

						if (height >= maxHeight)
							value = height * ratio;
					}

					return value;
				},
				_applyHeightConstraintRatio: function (value, maxWidth)
				{
					if (this._constraintBtn.get_checked())
					{
						var ratio = this.get_sizeRatio();
						var width = this._restrictValue(value * ratio, 0, maxWidth);
						this._widthTxt.value = width;

						if (width >= maxWidth)
							value = width / ratio;
					}

					return value;
				},

				_attachHandlers: function (toAttach)
				{
					this._attachButtonHandler(this._cropBtn, "_cropBtnClick", toAttach);
					this._attachButtonHandler(this._cancelBtn, "_cancelBtnClick", toAttach);
					this._attachButtonHandler(this._constraintBtn, "_constraintBtnClick", toAttach);
					this._attachButtonHandler(this._swapBtn, "_swapBtnClick", toAttach);

					if (toAttach)
					{
						this._presetDDDelegate = Function.createDelegate(this, this._presetDDHandler);
						this._presetDD.add_selectedIndexChanged(this._presetDDDelegate);

						this._changeXDelegate = Function.createDelegate(this, this._changeXHandler);
						$telerik.addHandler(this._xTxt, "keyup", this._changeXDelegate);
						this._changeYDelegate = Function.createDelegate(this, this._changeYHandler);
						$telerik.addHandler(this._yTxt, "keyup", this._changeYDelegate);
						this._changeWidthDelegate = Function.createDelegate(this, this._changeWidthHandler);
						$telerik.addHandler(this._widthTxt, "keyup", this._changeWidthDelegate);
						this._changeHeightDelegate = Function.createDelegate(this, this._changeHeightHandler);
						$telerik.addHandler(this._heightTxt, "keyup", this._changeHeightDelegate);

						this.updateCropBoxFromImageDelegate = Function.createDelegate(this, this._updateCropBoxFromImageHandler);
						this.get_imageEditor().getEditableImage().add_imageReload(this.updateCropBoxFromImageDelegate);
					}
					else
					{
						$telerik.removeHandler(this._xTxt, "keyup", this._changeXDelegate);
						$telerik.removeHandler(this._yTxt, "keyup", this._changeYDelegate);
						$telerik.removeHandler(this._widthTxt, "keyup", this._changeWidthDelegate);
						$telerik.removeHandler(this._heightTxt, "keyup", this._changeHeightDelegate);
						this._changeXDelegate = this._changeYDelegate = this._changeWidthDelegate = this._changeHeightDelegate = null;


						this.get_imageEditor().getEditableImage().remove_imageReload(this.updateCropBoxFromImageDelegate);
						delete this.updateCropBoxFromImageDelegate;
					}
				},
				_attachButtonHandler: function (button, handlerName, toAttach)
				{
					var delegateName = handlerName + "Delegate";
					if (toAttach)
					{
						var delegate = this[delegateName] = Function.createDelegate(this, this[handlerName]);
						button.add_clicked(delegate);
					}
					else
					{
						button.remove_clicked(this[delegateName]);
						delete this[delegateName];
					}
				},

				_changeXHandler: function (event) { this._changeDimensionHandler("set_x", event); },
				_changeYHandler: function (event) { this._changeDimensionHandler("set_y", event); },
				_changeWidthHandler: function (event) { this._changeDimensionHandler("set_width", event); },
				_changeHeightHandler: function (event) { this._changeDimensionHandler("set_height", event); },
				_changeDimensionHandler: function (method, event)
				{
					var value = this._getInputValueHandler(event);
					if (!isNaN(value))
						this[method](value);
				},

				_getInputValueHandler: function (event)
				{
					var keyCode = event.keyCode;
					var input = event.target;

					switch (keyCode)
					{
						case 38: //up key
							$telerik.cancelRawEvent(event.rawEvent);
							return this._incrementInputValue(input);
						case 40: //down key
							$telerik.cancelRawEvent(event.rawEvent);
							return this._decrementInputValue(input);
						case 27: //escape key
							$telerik.cancelRawEvent(event.rawEvent);
							return;
						default:
							if (this.isNumeric(keyCode) || this.isBackspaceOrDelete(keyCode))
								return parseInt(input.value);
					}
				},
				_incrementInputValue: function (input, setter)
				{
					return parseInt(input.value) + 1;
				},
				_decrementInputValue: function (input, setter)
				{
					return parseInt(input.value) - 1;
				},
				_presetDDHandler: function (combo, args)
				{
					var values = args.get_item().get_value().split(",");
					if (values.length == 2)
					{
						var presetSize = { width: parseInt(values[0]), height: parseInt(values[1]) };
						var presetRatio = presetSize.width / presetSize.height;
						var maxRatio = this._cropBox.get_maxWidth() / this._cropBox.get_maxHeight();
						var leadingDimensionName = presetRatio > maxRatio ? "width" : "height";

						this._constraintBtn.set_checked(true);
						this._sizeRatio = presetRatio;
						this._setCropBoxRatio(presetRatio);
						this["set_" + leadingDimensionName](presetSize[leadingDimensionName]);

						this._updateCropBoxFromControls();
					}
				},
				_cropBtnClick: function (button, args)
				{
					this.crop(this._collectBounds());
				},
				_cancelBtnClick: function (button, args)
				{
					this.close();
				},
				_swapBtnClick: function (button, args)
				{
					this._setCropSize(this._heightTxt.value, this._widthTxt.value);
					this._updateCropBoxFromControls();
				},
				_constraintBtnClick: function (button, args)
				{
					var ratio = button.get_checked() ? this.get_freshSizeRatio() : null;
					this._setCropBoxRatio(ratio);
					this._sizeRatio = ratio;
				},
				_setCropBoxRatio: function (ratio)
				{
					this._cropBox.set_sizeRatio(ratio);
				},

				_createCropBox: function ()
				{
					//this._disposeViewportControl();

					if (this._cropBox)
						this._disposeCropBox();

					this._cropBox = new $IE.DraggableResizeBox(this._imageEditor.get_viewport());
					this._cropBoxBoundsChangedDelegate = Function.createDelegate(this, this._cropBoxBoundsChangedHandler);
					this._cropBox.add_boundsChanged(this._cropBoxBoundsChangedDelegate);
					this._setCropBoxConstraints();
				},
				_setCropBoxConstraints: function (bounds)
				{
					var image = this._imageEditor.getEditableImage();
					var zoomLevel = image.get_zoomLevel() / 100;
					this._cropBox.set_constraints(bounds || new Sys.UI.Bounds(0, 0, parseInt(image.get_width() * zoomLevel), parseInt(image.get_height() * zoomLevel)));
				},
				_zoomCropBox: function ()
				{
					this._updateCropBoxFromControls();
				},
				_disposeCropBox: function ()
				{
					//this._disposeViewportControl();
					this._cropBoxBoundsChangedDelegate = this._cropBox.remove_boundsChanged(this._cropBoxBoundsChangedDelegate);
					this._cropBox = this._cropBox.dispose();
				},
				_disposeViewportControl: function ()
				{
					try
					{
						var cropBoxContainer = this._imageEditor.get_viewport();
						if (typeof (cropBoxContainer.control) !== "undefined")//this is a temporary fix
							delete cropBoxContainer.control;
					}
					catch (e) { }
				},

				_setCropSize: function (width, height)
				{
					this._widthTxt.value = width;
					this._heightTxt.value = height;
				},
				_updateCropBoxFromControls: function ()
				{
					var bounds = this._collectBounds();
					if (bounds == false) return;

					var scaledBounds = this._toScaledBounds(bounds);
					this._cropBox.setBounds(scaledBounds);
				},
				_collectBounds: function ()
				{
					var x = parseInt(this._xTxt.value);
					var y = parseInt(this._yTxt.value);
					var w = parseInt(this._widthTxt.value);
					var h = parseInt(this._heightTxt.value);
					if (isNaN(x) || isNaN(y) || isNaN(w) || isNaN(h)) return false;
					return new Sys.UI.Bounds(x, y, w, h);
				},
				_updateControlsFromCropBox: function ()
				{
					var actualBounds = this._fromScaledBounds(this._cropBox.getBounds());
					this._updateControlsWithFixedBounds(actualBounds);
				},
				_cropBoxBoundsChangedHandler: function (cropBox, args)
				{
					this._updateControlsWithBounds(args.getBounds());
				},
				_updateControlsWithBounds: function (bounds)
				{
					var actualBounds = this._fromScaledBounds(bounds);

					this._updateControlsWithFixedBounds(actualBounds);
				},
				_updateControlsWithFixedBounds: function (bounds)
				{
					for (var prop in bounds)
						this["_" + prop + "Txt"].value = bounds[prop];
				},
				_updateCropBoxFromImageHandler: function (event)
				{
					this._setCropBoxConstraints();
				},
				_fromScaledBounds: function (bounds)
				{
					return this._multiplyBounds(bounds, 100 / this._zoomLevel);
				},
				_toScaledBounds: function (bounds)
				{
					return this._multiplyBounds(bounds, this._zoomLevel / 100);
				},
				_multiplyBounds: function (bounds, zoom)
				{
					return new Sys.UI.Bounds(
						Math.round(bounds.x * zoom),
						Math.round(bounds.y * zoom),
						Math.round(bounds.width * zoom),
						Math.round(bounds.height * zoom));
				},
				_getImageZoomLevel: function ()
				{
					return this.get_imageEditor().getEditableImage().get_zoomLevel();
				},
				_getViewportScroll: function ()
				{
					var viewport = this.get_imageEditor().get_viewport();
					return { x: viewport.scrollLeft, y: viewport.scrollTop };
				},

				/*=================================  Telerik.Web.UI.ImageEditor.IToolWidged members  ===============================*/
				get_name: function () { return "Crop"; },

				updateUI: function ()
				{
					try
					{
						if (this._getImageZoomLevel() != this._zoomLevel)
						{
							this._zoomLevel = this._getImageZoomLevel();
							this._updateCropBoxFromControls();
						}
						else
						{
							this._updateControlsFromCropBox();
						}

						this._setCropBoxConstraints();
					}
					catch (e) { }
				}
				/*=================================  Telerik.Web.UI.ImageEditor.IToolWidged members  ===============================*/
			};
			$IE.Crop.registerClass("Telerik.Web.UI.ImageEditor.Crop", $IE.ToolWidget, $IE.IToolWidget);
		})($telerik.$, Telerik.Web.UI, Telerik.Web.UI.ImageEditor);
	}
	//]]>
</script>
