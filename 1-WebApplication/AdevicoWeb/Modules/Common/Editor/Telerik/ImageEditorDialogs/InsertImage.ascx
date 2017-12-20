<%@ Control Language="C#" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<table id="Table" class="rieDialogsTable" border="0" cellpadding="0" cellspacing="0"
	runat="server">
	<tr>
		<td class="rieRightAligned">
			<asp:Label ID="lblUrl" Text="URL:" runat="server" />
		</td>
		<td colspan="2">
			<asp:TextBox ID="Url" runat="server" Width="135px" ToolTip="URL of an Image to Insert (example http://domain.com/img.png"
				Style="margin: 0;" />
			<telerik:RadButton ID="btnOk" runat="server" Text="Set" AutoPostBack="false" EnableViewState="false"
				CausesValidation="false">
			</telerik:RadButton>
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
					ToggleType="CheckBox" Checked="true" Width="23px" EnableViewState="false" AutoPostBack="false"
					CausesValidation="false">
					<ToggleStates>
						<telerik:RadButtonToggleState PrimaryIconCssClass="rieConstrainBtn" PrimaryIconTop="3px" PrimaryIconLeft="6px" />
						<telerik:RadButtonToggleState PrimaryIconCssClass="rieRemoveConstrainBtn" PrimaryIconTop="4px" PrimaryIconLeft="6px" />
					</ToggleStates>
				</telerik:RadButton>
				<telerik:RadButton ID="btnSwap" runat="server" ToolTip="Swap Values" AutoPostBack="false"
					CausesValidation="false" EnableViewState="false" Width="23px">
					<Icon PrimaryIconCssClass="rieSwapValues" PrimaryIconTop="4px" PrimaryIconLeft="5px" />
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
			<asp:Literal ID="Literal9" Text=" px" runat="server" />
		</td>
	</tr>
	<tr>
		<td class="rieRightAligned" colspan="3">
			<telerik:RadButton ID="btnInsert" runat="server" Text="Insert" ToolTip="Insert" AutoPostBack="false"
				CausesValidation="false" EnableViewState="false" CommandName="Insert">
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
	if (typeof (Telerik.Web.UI.ImageEditor.InsertImage) === "undefined")
	{
		Type.registerNamespace("Telerik.Web.UI.ImageEditor");
		(function ($, $T, $IE)
		{
			$IE.InsertImage = function (imageEditor)
			{
				$IE.InsertImage.initializeBase(this, [imageEditor]);
			}
			$IE.InsertImage.prototype =
			{
				initialize: function ()
				{
					$IE.InsertImage.callBaseMethod(this, "initialize");

					this._xTxt = this._getControlFromParent("txtX");
					this._yTxt = this._getControlFromParent("txtY");
					this._widthTxt = this._getControlFromParent("txtWidth");
					this._heightTxt = this._getControlFromParent("txtHeight");
					this._insertBtn = this._findControlFromParent("btnInsert");
					this._cancelBtn = this._findControlFromParent("btnCancel");
					this._chooseBtn = this._findControlFromParent("btnChooseUrl");
					this._constraintBtn = this._findControlFromParent("btnConstraint");
					this._swapBtn = this._findControlFromParent("btnSwap");
					this._okBtn = this._findControlFromParent("btnOk"); //this._findControlFromParent("ChooseImage_C_btnOk");
					this._txtSrc = this._getControlFromParent("Url"); //this._getControlFromParent("ChooseImage_C_Url");
					this._sizeRatio = null;
					this._zoomLevel = this._getImageZoomLevel();

					//this._dialogChooseImage = this._findControlFromParent("ChooseImage");
					//this._element = this._dialogChooseImage.get_element().parentNode;

					this._createImageBox();
					this._updateControlsFromImageBox();
					this._constraintBtnClick(this._constraintBtn);
					this._attachHandlers(true);
					this.get_imageEditor().get_formDecorator().decorate($get(this.get_parentId() + "Table"));
				},

				dispose: function ()
				{
					//this._appendDialog("dispose");

					this._attachHandlers(false);
					this._disposeImageBox();

					$IE.InsertImage.callBaseMethod(this, "dispose");
				},

				onClose: function ()
				{
					//this._dialogChooseImage.set_closed(true);
					this._imageBox.hide();
				},

				onOpen: function ()
				{
					try
					{
						this._imageBox.show();

						var pos = this._getViewportScroll();
						this._xTxt.value = pos.x;
						this._yTxt.value = pos.y;
						this._updateImageBoxFromControls();
					}
					catch (e) { }
				},

				insertImage: function (bounds)
				{
					if (bounds == false) return false;

					var value = this._imageBox.get_src();
					var operations = [];
					operations[operations.length] = new $IE.ResizeOperation(false, bounds.width, bounds.height);
					this.get_imageEditor()._insertImage(bounds.x, bounds.y, value, operations);
				},

				set_width: function (value)
				{
					var imageSize = this.get_imageSize();
					var bounds = this._imageBox.getBounds();
					value = this._restrictValue(value, 0, imageSize.width - bounds.x);
					value = this._applyWidthConstraintRatio(value, imageSize.height - bounds.y);

					this.set_inputValue(this._widthTxt, value);
				},
				set_height: function (value)
				{
					var imageSize = this.get_imageSize();
					var bounds = this._imageBox.getBounds();
					value = this._restrictValue(value, 0, imageSize.height - bounds.y);
					value = this._applyHeightConstraintRatio(value, imageSize.width - bounds.y);

					this.set_inputValue(this._heightTxt, value);
				},
				set_x: function (value)
				{
					var imageSize = this.get_imageSize();
					var bounds = this._imageBox.getBounds();
					value = this._restrictValue(value, 0, imageSize.width - bounds.width);

					this.set_inputValue(this._xTxt, value);
				},
				set_y: function (value)
				{
					var imageSize = this.get_imageSize();
					var bounds = this._imageBox.getBounds();
					value = this._restrictValue(value, 0, imageSize.height - bounds.height);

					this.set_inputValue(this._yTxt, value);
				},
				set_inputValue: function (input, value)
				{
					input.value = parseInt(value);
					this._updateImageBoxFromControls();
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
					this._attachButtonHandler(this._insertBtn, "_insertBtnClick", toAttach);
					this._attachButtonHandler(this._cancelBtn, "_cancelBtnClick", toAttach);
					//this._attachButtonHandler(this._chooseBtn, "_chooseBtnClick", toAttach);
					this._attachButtonHandler(this._constraintBtn, "_constraintBtnClick", toAttach);
					this._attachButtonHandler(this._swapBtn, "_swapBtnClick", toAttach);
					this._attachButtonHandler(this._okBtn, "_okBtnHandler", toAttach);

					if (toAttach)
					{
						this._changeXDelegate = Function.createDelegate(this, this._changeXHandler);
						$telerik.addHandler(this._xTxt, "keyup", this._changeXDelegate);
						this._changeYDelegate = Function.createDelegate(this, this._changeYHandler);
						$telerik.addHandler(this._yTxt, "keyup", this._changeYDelegate);
						this._changeWidthDelegate = Function.createDelegate(this, this._changeWidthHandler);
						$telerik.addHandler(this._widthTxt, "keyup", this._changeWidthDelegate);
						this._changeHeightDelegate = Function.createDelegate(this, this._changeHeightHandler);
						$telerik.addHandler(this._heightTxt, "keyup", this._changeHeightDelegate);

						this.updateImageBoxFromImageDelegate = Function.createDelegate(this, this._updateImageBoxFromImageHandler);
						this.get_imageEditor().getEditableImage().add_imageReload(this.updateImageBoxFromImageDelegate);
					}
					else
					{
						$telerik.removeHandler(this._xTxt, "keyup", this._changeXDelegate);
						$telerik.removeHandler(this._yTxt, "keyup", this._changeYDelegate);
						$telerik.removeHandler(this._widthTxt, "keyup", this._changeWidthDelegate);
						$telerik.removeHandler(this._heightTxt, "keyup", this._changeHeightDelegate);
						this._changeXDelegate = this._changeYDelegate = this._changeWidthDelegate = this._changeHeightDelegate = null;

						this.get_imageEditor().getEditableImage().remove_imageReload(this.updateImageBoxFromImageDelegate);
						delete this.updateImageBoxFromImageDelegate;
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
						case 13: //enter key
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
				_insertBtnClick: function (button, args)
				{
					if (this._imageBox.IsImageBroken) return;
					//insert image
					if (this.insertImage(this._collectBounds()) == false) return;
					//this._dialogChooseImage.set_closed(true);
					this.close();
				},
				_cancelBtnClick: function (button, args)
				{
					this.close();
				},
				_swapBtnClick: function (button, args)
				{
					this._setImageSize(this._heightTxt.value, this._widthTxt.value);
					this._updateImageBoxFromControls();
				},
				_constraintBtnClick: function (button, args)
				{
					var ratio = button.get_checked() ? this.get_freshSizeRatio() : null;
					this._setImageBoxRatio(ratio);
					this._sizeRatio = ratio;
				},
				_setImageBoxRatio: function (ratio)
				{
					this._imageBox.set_sizeRatio(ratio);
				},
				_chooseBtnClick: function (button, args)
				{
					this._openChooseImageDialog();
				},
				_openChooseImageDialog: function ()
				{
					//			this._dialogChooseImage._form.appendChild(this._dialogChooseImage.get_element());
					//			this._dialogChooseImage.set_closed(false);
					//			if(!this._dialogChooseImage.get_top())
					//			{
					//				var dock = this.get_imageEditor().get_toolsPanel();
					//				this._dialogChooseImage.set_left(dock.get_left());
					//				this._dialogChooseImage.set_top(dock.get_top() + 200);
					//			}
				},
				_okBtnHandler: function (button, args)
				{
					//this._dialogChooseImage.set_closed(true);
					if (this._txtSrc.value)
					{
						this._imageBox.IsImageBroken = false;
						this._imageBox.set_src(this._txtSrc.value);
					}
					this._updateControlsFromImageBox();
				},
				_appendDialog: function (arg)
				{
					//if(arg === true)
					//{
					//	this.get_imageEditor().get_element().appendChild(this._dialogChooseImage.get_element());
					//}
					//else 
					//{
					//	var el = this._dialogChooseImage.get_element();
					//	if(el)
					//	{
					//		this._dialogChooseImage.set_closed(true);
					//		this._element.appendChild(this._dialogChooseImage.get_element());
					//	}
					//}
				},

				_createImageBox: function ()
				{
					if (this._imageBox)
						this._disposeImageBox();

					this._imageBox = new $IE.DraggableImageResizeBox(this._imageEditor.get_viewport());
					this._imageBox.set_src(this.get_imageEditor().get_defaultInsertedImageUrl());
					this._imageBoxBoundsChangedDelegate = Function.createDelegate(this, this._imageBoxBoundsChangedHandler);
					this._imageBox.add_boundsChanged(this._imageBoxBoundsChangedDelegate);
					this._setImageBoxConstraints();
				},
				_setImageBoxConstraints: function (bounds)
				{
					var image = this._imageEditor.getEditableImage();
					var zoomLevel = image.get_zoomLevel() / 100;
					this._imageBox.set_constraints(bounds || new Sys.UI.Bounds(0, 0, parseInt(image.get_width() * zoomLevel), parseInt(image.get_height() * zoomLevel)));
				},
				_zoomImageBox: function ()
				{
					this._updateImageBoxFromControls();
				},
				_disposeImageBox: function ()
				{
					this._imageBoxBoundsChangedDelegate = this._imageBox.remove_boundsChanged(this._imageBoxBoundsChangedDelegate);
					this._imageBox = this._imageBox.dispose();
				},

				_setImageSize: function (width, height)
				{
					this._widthTxt.value = width;
					this._heightTxt.value = height;
				},
				_updateImageBoxFromControls: function ()
				{
					var bounds = this._collectBounds();
					if (bounds == false) return;
					var scaledBounds = this._toScaledBounds(bounds);
					this._imageBox.setBounds(scaledBounds);
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
				_updateControlsFromImageBox: function ()
				{
					var actualBounds = this._fromScaledBounds(this._imageBox.getBounds());
					this._updateControlsWithFixedBounds(actualBounds);
				},
				_imageBoxBoundsChangedHandler: function (imageBox, args)
				{
					this._updateControlsWithBounds(args.getBounds());
					if (args.get_value() === true)
					{
						this._setImageBoxRatio(this._constraintBtn.get_checked() ? this.get_freshSizeRatio() : null);
					}
				},
				_updateControlsWithBounds: function (bounds)
				{
					var actualBounds = this._fromScaledBounds(bounds);

					this._updateControlsWithFixedBounds(actualBounds);
				},
				//updates the TextBoxes with the current position + width and height
				_updateControlsWithFixedBounds: function (bounds)
				{
					for (var prop in bounds)
						this["_" + prop + "Txt"].value = bounds[prop];
				},
				_updateImageBoxFromImageHandler: function (event)
				{
					this._setImageBoxConstraints();
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
				get_name: function () { return "InsertImage"; },

				updateUI: function ()
				{
					try
					{
						if (this._getImageZoomLevel() != this._zoomLevel)
						{
							this._zoomLevel = this._getImageZoomLevel();
							this._updateImageBoxFromControls();
						}
						else
						{
							this._updateControlsFromImageBox();
						}

						this._setImageBoxConstraints();
					}
					catch (e) { }
				}
				/*=================================  Telerik.Web.UI.ImageEditor.IToolWidged members  ===============================*/
			};
			$IE.InsertImage.registerClass("Telerik.Web.UI.ImageEditor.InsertImage", $IE.ToolWidget, $IE.IToolWidget);
		})($telerik.$, Telerik.Web.UI, Telerik.Web.UI.ImageEditor);
	}
	//]]>
</script>
