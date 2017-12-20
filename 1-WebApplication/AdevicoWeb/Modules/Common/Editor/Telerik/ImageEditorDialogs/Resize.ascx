<%@ Control Language="C#" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<table id="Table" class="rieDialogsTable" border="0" cellpadding="0" cellspacing="0"
	runat="server">
	<tr>
		<td class="rieRightAligned">
			<label id="lblPresetSizes" runat="server">
				Preset Sizes:</label>
		</td>
		<td colspan="2">
			<telerik:RadComboBox ID="PresetSizes" runat="server" AutoPostBack="false" EnableViewState="false"
				CausesValidation="false" Width="140px">
				<Items>
					<telerik:RadComboBoxItem Text="Original W x H" Value="original" />
					<telerik:RadComboBoxItem Text="Custom W x H" Value="custom" />
					<telerik:RadComboBoxItem Text="800x600px" Value="800,600" />
					<telerik:RadComboBoxItem Text="1024x768px" Value="1024,768" />
					<telerik:RadComboBoxItem Text="1280x1024px" Value="1280,1024" />
					<telerik:RadComboBoxItem Text="1440x900px" Value="1440,900" />
				</Items>
			</telerik:RadComboBox>
		</td>
	</tr>
	<tr>
		<td class="rieRightAligned">
			<asp:Label ID="lblWidth" Text="Width:" runat="server" AssociatedControlID="TxtWidth" />
		</td>
		<td class="rieInputDimension">
			<asp:TextBox ID="TxtWidth" runat="server" ToolTip="Width" Width="45px" />
			<asp:Literal ID="Literal1" Text=" px" runat="server" />
		</td>
		<td rowspan="2">
			<div class="rieResizeButtons">
				<telerik:RadButton ID="BtnConstraint" runat="server" ToolTip="Constrain Proportions"
					ToggleType="CheckBox" Checked="true" Width="20px" EnableViewState="false" AutoPostBack="false"
					CausesValidation="false">
					<ToggleStates>
						<telerik:RadButtonToggleState PrimaryIconCssClass="rieConstrainBtn" PrimaryIconTop="3px" />
						<telerik:RadButtonToggleState PrimaryIconCssClass="rieRemoveConstrainBtn" PrimaryIconTop="4px" />
					</ToggleStates>
				</telerik:RadButton>
				<telerik:RadButton ID="BtnSwap" runat="server" ToolTip="Swap Values" AutoPostBack="false"
					CausesValidation="false" EnableViewState="false" Width="20px">
					<Icon PrimaryIconCssClass="rieSwapValues" PrimaryIconTop="4px" PrimaryIconLeft="4px" />
				</telerik:RadButton>
			</div>
		</td>
	</tr>
	<tr>
		<td class="rieRightAligned">
			<asp:Label ID="lblHeight" Text="Height:" runat="server" AssociatedControlID="TxtHeight" />
		</td>
		<td class="rieInputDimension">
			<asp:TextBox ID="TxtHeight" runat="server" ToolTip="Height" Width="45px" />
			<asp:Literal ID="Literal4" Text=" px" runat="server" />
		</td>
	</tr>
	<tr>
		<td class="rieRightAligned">
			<asp:Label ID="lblPercent" Text="Percentage:" runat="server" AssociatedControlID="TxtPercent" />
		</td>
		<td colspan="2">
			<asp:TextBox ID="TxtPercent" runat="server" ToolTip="Percentage" Width="45px" />
			<asp:Literal ID="Literal2" Text=" %" runat="server" />
		</td>
	</tr>
	<tr>
		<td class="rieRightAligned" colspan="3">
			<telerik:RadButton ID="BtnResize" runat="server" Text="Resize" ToolTip="Resize" AutoPostBack="false"
				CausesValidation="false" EnableViewState="false" CommandName="Resize">
			</telerik:RadButton>
			<telerik:RadButton ID="BtnCancel" runat="server" Text="Cancel" ToolTip="Cancel" AutoPostBack="false"
				CausesValidation="false" EnableViewState="false" CommandName="Cancel">
			</telerik:RadButton>
		</td>
	</tr>
</table>
<script type="text/javascript">
	//<![CDATA[
	//Register the class only if it has not been defined
	if (typeof (Telerik.Web.UI.ImageEditor.Resize) === "undefined")
	{
		Type.registerNamespace("Telerik.Web.UI.ImageEditor");
		(function ($, $T, $IE)
		{
			$IE.Resize = function (imageEditor)
			{
				$IE.Resize.initializeBase(this, [imageEditor]);
			}

			$IE.Resize.prototype =
			{
				initialize: function ()
				{
					$IE.Resize.callBaseMethod(this, "initialize");

					this._attachHandlers(true);
					//get editable image
					this._editableImage = this.get_imageEditor().getEditableImage();
					this._percentValue = this.get_percentTxt().value = "100";
					//calculate the ratios
					if (this._editableImage)
					{
						this._originalWidth = this._editableImage.get_width();
						this._originalHeight = this._editableImage.get_height();
						this._updateCoefficients(this._originalWidth, this._originalHeight);
					}

					//decorate the textboxes
					this.get_imageEditor().get_formDecorator().decorate($get(this.get_parentId() + "Table"));
				},

				dispose: function ()
				{
					this._attachHandlers(false);

					$IE.Resize.callBaseMethod(this, "dispose");
				},

				onClose: function ()
				{
					this.get_comboSizes().hideDropDown();
				},

				//resizes the image to the specified width and height
				resize: function (width, height)
				{
					var imageEditor = this.get_imageEditor();

					this._originalWidth = parseInt(width);
					this._originalHeight = parseInt(height);
					imageEditor._resizeImage(this._originalWidth, this._originalHeight, true, false);
					this.get_percentTxt().value = 100;
				},

				get_widthTxt: function ()
				{
					if (!this._widthTxt)
					{
						this._widthTxt = $get(this.get_parentId() + "TxtWidth");
						if (this._widthTxt)
							this._widthTxt.setAttribute("autoComplete", "off");
					}
					return this._widthTxt;
				},

				get_heightTxt: function ()
				{
					if (!this._heightTxt)
					{
						this._heightTxt = $get(this.get_parentId() + "TxtHeight");
						if (this._heightTxt)
							this._heightTxt.setAttribute("autoComplete", "off");
					}
					return this._heightTxt;
				},

				get_percentTxt: function ()
				{
					if (!this._percentTxt)
					{
						this._percentTxt = $get(this.get_parentId() + "TxtPercent");
						if (this._percentTxt)
							this._percentTxt.setAttribute("autoComplete", "off");
					}
					return this._percentTxt;
				},

				get_resizeBtn: function ()
				{
					if (!this._resizeBtn) { this._resizeBtn = $find(this.get_parentId() + "BtnResize"); }
					return this._resizeBtn;
				},

				get_cancelBtn: function ()
				{
					if (!this._cancelBtn) { this._cancelBtn = $find(this.get_parentId() + "BtnCancel"); }
					return this._cancelBtn;
				},

				get_constraintBtn: function ()
				{
					if (!this._constraintBtn) { this._constraintBtn = $find(this.get_parentId() + "BtnConstraint"); }
					return this._constraintBtn;
				},

				get_swapBtn: function ()
				{
					if (!this._swapBtn) { this._swapBtn = $find(this.get_parentId() + "BtnSwap"); }
					return this._swapBtn;
				},

				get_comboSizes: function ()
				{
					if (!this._comboSizes) { this._comboSizes = $find(this.get_parentId() + "PresetSizes"); }
					return this._comboSizes;
				},

				get_max: function ()
				{
					return 9999;
				},

				get_min: function ()
				{
					return 1;
				},

				IsConstrained: function ()
				{
					return this.get_constraintBtn().get_checked();
				},

				_attachHandlers: function (toAttach)
				{
					var resizeBtn = this.get_resizeBtn();
					var cancelBtn = this.get_cancelBtn();
					var txtW = this.get_widthTxt();
					var txtH = this.get_heightTxt();
					var txtP = this.get_percentTxt();
					var comboSizes = this.get_comboSizes();
					var constraintBtn = this.get_constraintBtn();
					var swapBtn = this.get_swapBtn();
					if (toAttach)
					{
						this._resizeBtnClickDelegate = Function.createDelegate(this, this._resizeBtnClick);
						if (resizeBtn && cancelBtn)
						{
							resizeBtn.add_clicked(this._resizeBtnClickDelegate);
							cancelBtn.add_clicked(this._resizeBtnClickDelegate);
						}
						if (txtW)
						{
							this._widthKeyDownDelegate = Function.createDelegate(this, this._widthKeyDown);
							$addHandler(txtW, "keydown", this._widthKeyDownDelegate);
							this._widthKeyUpDelegate = Function.createDelegate(this, this._widthKeyUp);
							$addHandler(txtW, "keyup", this._widthKeyUpDelegate);
						}
						if (txtH)
						{
							this._heightKeyDownDelegate = Function.createDelegate(this, this._heightKeyDown);
							$addHandler(txtH, "keydown", this._heightKeyDownDelegate);
							this._heightKeyUpDelegate = Function.createDelegate(this, this._heightKeyUp);
							$addHandler(txtH, "keyup", this._heightKeyUpDelegate);
						}
						if (txtP)
						{
							this._percentTxtKeyDownDelegate = Function.createDelegate(this, this._percentTxtKeyDown);
							$addHandler(txtP, "keydown", this._percentTxtKeyDownDelegate);
							this._percentTxtKeyUpDelegate = Function.createDelegate(this, this._percentTxtKeyUp);
							$addHandler(txtP, "keyup", this._percentTxtKeyUpDelegate);
						}
						if (comboSizes)
						{
							this._indexChangedDelegate = Function.createDelegate(this, this._indexChanged);
							comboSizes.add_selectedIndexChanged(this._indexChangedDelegate);
						}
						if (swapBtn)
						{
							this._swapBtnClickDelegate = Function.createDelegate(this, this._swapBtnClick);
							swapBtn.add_clicked(this._swapBtnClickDelegate);
						}
						if (constraintBtn)
						{
							this._checkedChangedDelegate = Function.createDelegate(this, this._checkedChanged);
							constraintBtn.add_checkedChanged(this._checkedChangedDelegate);
						}
					}
					else
					{
						if (resizeBtn && cancelBtn)
						{
							resizeBtn.remove_clicked(this._resizeBtnClickDelegate);
							cancelBtn.remove_clicked(this._resizeBtnClickDelegate);
						}
						this._resizeBtnClickDelegate = null;
						if (txtW)
						{
							$removeHandler(txtW, "keydown", this._widthKeyDownDelegate);
							this._widthKeyDownDelegate = null;
							$removeHandler(txtW, "keyup", this._widthKeyUpDelegate);
							this._widthKeyUpDelegate = null;
						}
						if (txtH)
						{
							$removeHandler(txtH, "keydown", this._heightKeyDownDelegate);
							this._heightKeyDownDelegate = null;
							$removeHandler(txtH, "keyup", this._heightKeyUpDelegate);
							this._heightKeyUpDelegate = null;
						}
						if (txtP)
						{
							$removeHandler(txtP, "keydown", this._percentTxtKeyDownDelegate);
							this._percentTxtKeyDownDelegate = null;
							$removeHandler(txtP, "keyup", this._percentTxtKeyUpDelegate);
							this._percentTxtKeyUpDelegate = null;
						}
						if (comboSizes)
						{
							comboSizes.remove_selectedIndexChanged(this._indexChangedDelegate);
							this._indexChangedDelegate = null;
						}
						if (swapBtn)
						{
							swapBtn.remove_clicked(this._swapBtnClickDelegate);
							this._swapBtnClickDelegate = null;
						}
						if (constraintBtn)
						{
							constraintBtn.remove_checkedChanged(this._checkedChangedDelegate);
							this._checkedChangedDelegate = null;
						}
					}
				},

				_resizeBtnClick: function (sender, args)
				{
					if (args.get_commandName() == "Resize")
					{
						var restrictions = {min: this.get_min(), max: this.get_max()};
						this.resize(
							this.restrictInputValue(this.get_widthTxt(), restrictions),
							this.restrictInputValue(this.get_heightTxt(), restrictions));
					}
					else
					{
						this.get_imageEditor().closeToolsPanel();
					}
				},

				_checkedChanged: function (sender, args)
				{
					if (sender.get_checked())
					{
						this._updateCoefficients(this.get_widthTxt().value, this.get_heightTxt().value);
					}
				},

				_widthKeyDown: function (args)
				{
					return this._dimTxtsKeyDown(args, this.get_heightTxt(), this._ratioHW, true);
				},

				_heightKeyDown: function (args)
				{
					return this._dimTxtsKeyDown(args, this.get_widthTxt(), this._ratioWH, false);
				},

				_dimTxtsKeyDown: function (args, txtUpdated, ratio, isWidth)
				{
					var keyCode = args.keyCode;
					var targetTxt = args.target;

					if (keyCode == 38 && this.incrementValue(targetTxt, this.get_max())) //up arrow
					{
						this._setValues(targetTxt, txtUpdated, ratio, isWidth);
					}
					else if (keyCode == 40 && this.decrementValue(targetTxt, this.get_min())) //down arrow
					{
						this._setValues(targetTxt, txtUpdated, ratio, isWidth);
					}

					if (keyCode == 13) //enter
					{
						var value = this.checkValue(targetTxt, this.get_min(), this.get_max());
						if (value === false)
						{
							targetTxt.value = isWidth ? this._width : this._height;
						}
						else
						{
							this._setValues(targetTxt, txtUpdated, ratio, isWidth);
						}

						//Cancel the form submission
						$telerik.cancelRawEvent(args.rawEvent);
						return false;
					}

					if (keyCode == 27) //if 'Escape' pressed cancel
					{
						$telerik.cancelRawEvent(args.rawEvent);
						return false;
					}
					return true;
				},

				_widthKeyUp: function (args)
				{
					var keyCode = args.keyCode;
					var targetTxt = args.target;
					var isBD = this.isBackspaceOrDelete(keyCode);
					if (this.isNumeric(keyCode) || isBD) //only numeric keys allowed
					{
						var value = this.checkValue(targetTxt, this.get_min(), this.get_max(), isBD);
						if (value === false && !isBD) { targetTxt.value = this._width; }
						else { this._setValues(targetTxt, this.get_heightTxt(), this._ratioHW, true); }
					}
				},

				_heightKeyUp: function (args)
				{
					var keyCode = args.keyCode;
					var targetTxt = args.target;
					var isBD = this.isBackspaceOrDelete(keyCode);
					if (this.isNumeric(keyCode) || isBD) //only numeric keys allowed
					{
						var value = this.checkValue(targetTxt, this.get_min(), this.get_max(), isBD);
						if (value === false && !isBD) { targetTxt.value = this._height; }
						else { this._setValues(targetTxt, this.get_widthTxt(), this._ratioWH, false); }
					}
				},

				_setValues: function (targetTxt, txtUpdated, ratio, isWidth)
				{
					if (this._selectedValueCombo != "custom")
					{
						this.get_comboSizes().findItemByValue("custom").select();
						this._selectedValueCombo = "custom";
					}
					if (this.IsConstrained())
					{
						var newVal = Math.floor(parseInt(targetTxt.value) * ratio);
						txtUpdated.value = (isNaN(newVal) ? 0 : newVal) + "";
					}
					if (isWidth) { this._width = targetTxt.value; }
					else { this._height = targetTxt.value; }
				},

				_percentTxtKeyDown: function (args)
				{
					var keyCode = args.keyCode;
					var targetTxt = args.target;

					if (keyCode == 38 && this.incrementValue(targetTxt, 1000)) //up arrow 
					{
						this._percentHelper(parseInt(targetTxt.value) / 100, targetTxt);
					}
					else if (keyCode == 40 && this.decrementValue(targetTxt, 0)) //down arrow
					{
						this._percentHelper(parseInt(targetTxt.value) / 100, targetTxt);
					}

					if (keyCode == 13) //enter
					{
						var value = this.checkValue(targetTxt, 0, 1000);
						if (value === false)
						{
							targetTxt.value = this._percentValue;
						}
						else
						{
							this._percentHelper(value / 100, targetTxt);
						}

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

				_percentTxtKeyUp: function (args)
				{

					var keyCode = args.keyCode;
					var targetTxt = args.target;
					var isBD = this.isBackspaceOrDelete(keyCode);
					if (this.isNumeric(keyCode) || isBD) //only numeric keys allowed
					{
						var value = this.checkValue(targetTxt, 0, 1000, isBD);
						if (value === false && !isBD)
						{
							targetTxt.value = this._percentValue;
						}
						else
						{
							this._percentHelper(value / 100, targetTxt);
						}
					}
				},

				_percentHelper: function (coefficient, targetTxt)
				{
					if (this._selectedValueCombo != "custom")
					{
						this.get_comboSizes().findItemByValue("custom").select();
						this._selectedValueCombo = "custom";
					}
					var width = Math.floor(this._originalWidth * coefficient);
					var height = Math.floor(this._originalHeight * coefficient);
					this._percentValue = parseInt(targetTxt.value);
					this._applyWidthHeight(width, height);
				},

				_indexChanged: function (sender, args)
				{
					var value = args.get_item().get_value();
					this._selectedValueCombo = value;
					if (value == "custom") return;
					if (value == "original")
					{
						if (this._editableImage) this._setWidthHeight(this._editableImage.get_width(), this._editableImage.get_height(), true);
					}
					else
					{
						var values = value.split(',');
						this._setWidthHeight(values[0], values[1], true);
					}

				},

				_swapBtnClick: function (sender, args)
				{
					this._setWidthHeight(this.get_heightTxt().value, this.get_widthTxt().value, true);
				},

				_setWidthHeight: function (width, height, updateCoefficients)
				{
					this._applyWidthHeight(width, height);
					if (updateCoefficients)
					{
						this._updateCoefficients(width, height);
					}
				},

				_applyWidthHeight: function (width, height)
				{
					this._width = this.get_widthTxt().value = width;
					this._height = this.get_heightTxt().value = height;
				},

				_updateCoefficients: function (width, height)
				{
					width = parseInt(width);
					height = parseInt(height);
					this._ratioWH = height == 0 ? 1 : width / height;
					this._ratioHW = width == 0 ? 1 : height / width;
				},

				/*=================================  Telerik.Web.UI.ImageEditor.IToolWidged members  ===============================*/
				get_name: function ()
				{
					return "Resize";
				},

				updateUI: function ()
				{
					if (this._editableImage)
					{
						this.get_comboSizes().get_items()._array[0].select();
						this._originalWidth = this._editableImage.get_width();
						this._originalHeight = this._editableImage.get_height();
						this._updateCoefficients(this._originalWidth, this._originalHeight);
					}
				}
				/*=================================  Telerik.Web.UI.ImageEditor.IToolWidged members  ===============================*/
			};
			$IE.Resize.registerClass("Telerik.Web.UI.ImageEditor.Resize", $IE.ToolWidget, $IE.IToolWidget);
		})($telerik.$, Telerik.Web.UI, Telerik.Web.UI.ImageEditor);
	}
	//]]>
</script>
