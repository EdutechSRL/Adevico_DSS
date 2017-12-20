<%@ Control Language="C#" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<table id="Table" class="rieDialogsTable" border="0" cellpadding="0" cellspacing="0"
	runat="server">
	<tr>
		<td colspan="3">
			<asp:Label ID="lblTextArea" Text="Your Text Here..." runat="server" Style="display: none;"
				AssociatedControlID="textContent" />
			<textarea id="textContent" runat="server" rows="3" cols="20">Your Text Here...</textarea>
		</td>
	</tr>
	<tr>
		<td class="rieRightAligned">
			<label id="lblFontFamily" runat="server">
				Font Family:</label>
		</td>
		<td>
			<telerik:RadComboBox ID="fontFamily" runat="server" AutoPostBack="false" EnableViewState="false"
				CausesValidation="false" Width="130px">
				<Items>
					<telerik:RadComboBoxItem Text="Arial" Value="arial" />
					<telerik:RadComboBoxItem Text="Times New Roman" Value="times new roman" />
					<telerik:RadComboBoxItem Text="Verdana" Value="verdana" />
					<telerik:RadComboBoxItem Text="Tahoma" Value="tahoma" />
				</Items>
			</telerik:RadComboBox>
		</td>
	</tr>
	<tr>
		<td class="rieRightAligned">
			<asp:Label ID="lblFontSize" Text="Font Size:" runat="server" AssociatedControlID="size" />
		</td>
		<td>
			<asp:TextBox ID="size" runat="server" ToolTip="Font Size ToolTip" Width="45px" autocomplete="off" />
			<asp:Literal ID="Literal1" Text=" px" runat="server" />
		</td>
	</tr>
	<tr>
		<td class="rieRightAligned">
			<asp:Label ID="lblColor" Text="Color:" runat="server" AssociatedControlID="size" />
		</td>
		<td class="rieColorValue">
			<asp:TextBox ID="color" runat="server" ToolTip="Font Size ToolTip" Width="45px" autocomplete="off" />
			<telerik:RadColorPicker ID="colorPicker" runat="server" ShowIcon="true" AutoPostBack="false"
				CssClass="rieColorPicker" EnableViewState="false">
				<Localization PickColorText="Pick Color" CurrentColorText="(Current color is {0})" />
			</telerik:RadColorPicker>
		</td>
	</tr>
	<tr>
		<td class="rieRightAligned">
			<asp:Literal ID="lPosition" Text="Position:" runat="server" />
		</td>
		<td>
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
		<td class="rieRightAligned" colspan="2">
			<telerik:RadButton ID="btnApply" runat="server" Text="Insert" ToolTip="Insert" AutoPostBack="false"
				CausesValidation="false" EnableViewState="false">
			</telerik:RadButton>
			<telerik:RadButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" AutoPostBack="false"
				CausesValidation="false" EnableViewState="false">
			</telerik:RadButton>
		</td>
	</tr>
</table>
<script type="text/javascript">
	//<![CDATA[
	//Register the class only if it has not been defined
	if (typeof (Telerik.Web.UI.ImageEditor.AddText) === "undefined")
	{
		Type.registerNamespace("Telerik.Web.UI.ImageEditor");
		(function ($, $T, $IE)
		{
			$IE.AddText = function (imageEditor)
			{
				$IE.AddText.initializeBase(this, [imageEditor]);
			}
			$IE.AddText.prototype =
			{
				initialize: function ()
				{
					$IE.AddText.callBaseMethod(this, "initialize");

					this._textArea = this._getControlFromParent("textContent");
					this._fontsDD = this._findControlFromParent("fontFamily");
					this._sizeTxt = this._getControlFromParent("size");
					this._colorTxt = this._getControlFromParent("color");
					this._colorPicker = this._findControlFromParent("colorPicker");
					this._xTxt = this._getControlFromParent("txtX");
					this._yTxt = this._getControlFromParent("txtY");
					this._btnApply = this._findControlFromParent("btnApply");
					this._btnCancel = this._findControlFromParent("btnCancel");

					this._createDraggableText();
					this._attachEventHandlers(true);
					this._setDefaultValues();

					this.get_imageEditor().get_formDecorator().decorate($get(this.get_parentId() + "Table"));
				},

				apply: function ()
				{
					var x = this.get_x();
					var y = this.get_y();
					var settings = this.get_textSettings();
					if (isNaN(x) || isNaN(y) || isNaN(settings.get_fontSize())) return false;

					this.get_imageEditor().addTextToImage(x, y, settings);
				},
				onOpen: function ()
				{
					var viewport = $(this.get_imageEditor().get_viewport());
					this._draggableText.set_position({ x: viewport.scrollLeft(), y: viewport.scrollTop() });
					this._draggableText.show();
				},
				onClose: function ()
				{
					this._draggableText.hide();
					this._fontsDD.hideDropDown();
				},
				dispose: function ()
				{
					this._draggableText.dispose();
					$IE.AddText.callBaseMethod(this, "dispose");
				},
				set_position: function (pos)
				{
					this._draggableText.set_position(this._getScaledPosition(pos, this._getImageZoom()));
					this._setPositionInternal(pos);
				},
				get_position: function ()
				{
					return this._draggableText.get_position();
				},
				get_x: function ()
				{
					return parseInt(this._xTxt.value);
				},
				set_x: function (value)
				{
					if (!value || isNaN(value)) return;

					this.set_position($.extend(this.get_position(), { x: value }));
				},
				get_y: function ()
				{
					return parseInt(this._yTxt.value);
				},
				set_y: function (value)
				{
					if (!value || isNaN(value)) return;

					this.set_position($.extend(this.get_position(), { y: value }));
				},
				get_textToAdd: function ()
				{
					return this._textArea.value;
				},
				set_textToAdd: function (text)
				{
					this._textArea.value = text;
					this._draggableText.set_text(text);
				},
				set_fontSize: function (value)
				{
					if (!value || isNaN(value) || value < 0) return;

					this._draggableText.set_fontSize(value * this._getImageZoom());
					this._sizeTxt.value = value;
				},
				get_textSettings: function ()
				{
					var textSettings = this._draggableText.get_textSettings();
					textSettings.set_fontSize(parseInt(this._sizeTxt.value));                
                    var draggableTextHeight = this._draggableText.get_size().height;
                    var draggableTextLinesCount = textSettings.get_text().split("\n").length;
                    var draggableTextLineHeight = parseFloat(draggableTextHeight / draggableTextLinesCount);
                    textSettings.set_lineHeight(parseFloat(draggableTextLineHeight));
					return textSettings;
				},
				get_name: function () { return "AddText"; },
				updateUI: function ()
				{
					this.set_fontSize(parseInt(this._sizeTxt.value));
					this.set_position({ x: this.get_x(), y: this.get_y() });
				},

				_createDraggableText: function ()
				{
					this._draggableText = new $IE.DraggableTextBox(this.get_imageEditor().get_viewport());
				},
				_attachEventHandlers: function (toAttach)
				{
					if (toAttach)
					{
						this._changeTextDelegate = Function.createDelegate(this, this._changeTextHandler);
						$telerik.addHandler(this._textArea, "keyup", this._changeTextDelegate);
						this._changeFontSizeDelegate = Function.createDelegate(this, this._changeFontSizeHandler);
						$telerik.addHandler(this._sizeTxt, "keyup", this._changeFontSizeDelegate);
						this._changeXDelegate = Function.createDelegate(this, this._changeXHandler);
						$telerik.addHandler(this._xTxt, "keyup", this._changeXDelegate);
						this._changeYDelegate = Function.createDelegate(this, this._changeYHandler);
						$telerik.addHandler(this._yTxt, "keyup", this._changeYDelegate);

						this._fontFamilyChangedDelegate = Function.createDelegate(this, this._fontFamilyChangedHandler);
						this._fontsDD.add_selectedIndexChanged(this._fontFamilyChangedDelegate);
						this._colorChangeDelegate = Function.createDelegate(this, this._colorChangeHandler);
						this._colorPicker.add_colorChange(this._colorChangeDelegate);
						this._textBoundsChangedDelegate = Function.createDelegate(this, this._textBoundsChangedHandler);
						this._draggableText.add_boundsChanged(this._textBoundsChangedDelegate);

						this._applyBtnDelegate = Function.createDelegate(this, this._applyBtnHandler);
						this._btnApply.add_clicked(this._applyBtnDelegate);
						this._closeDelegate = Function.createDelegate(this, this.close);
						this._btnCancel.add_clicked(this._closeDelegate);
					}
					else
					{
						$telerik.removeHandler(this._textArea, "keyup", this._changeTextDelegate);
						$telerik.removeHandler(this._sizeTxt, "keyup", this._changeXDelegate);
						$telerik.removeHandler(this._xTxt, "keyup", this._changeXDelegate);
						$telerik.removeHandler(this._yTxt, "keyup", this._changeYDelegate);

						this._colorPicker.remove_colorChange(this._colorChangeDelegate);
						this._draggableText.remove_boundsChanged(this._textBoundsChangedDelegate);

						this._changeTextDelegate = this._changeFontSizeDelegate = this._changeXDelegate =
						this._changeYDelegate = this._colorChangeDelegate = this._textBoundsChangedDelegate = null;
					}
				},
				_applyBtnHandler: function (button, args)
				{
					var result = this.apply();
					if (result == false) return;
					this.close();
				},
				_fontFamilyChangedHandler: function (combo, args)
				{
					var option = args.get_item();
					this._draggableText.set_fontFamily(option.get_value());
				},
				_colorChangeHandler: function (picker, args)
				{
					this._colorTxt.value = picker.get_selectedColor();
					this._draggableText.set_color(this._colorTxt.value);
				},
				_textBoundsChangedHandler: function ()
				{
					var dragPosition = this._draggableText.get_position();
					this._setPositionInternal(this._getScaledPosition(dragPosition));
				},
				_setPositionInternal: function (pos)
				{
					this._xTxt.value = pos.x;
					this._yTxt.value = pos.y;
				},
				_setDefaultValues: function ()
				{
					var textSettings = this._getDefaultTextSettings();
					this.set_fontSize(textSettings.get_fontSize());

					this._colorPicker.set_selectedColor(textSettings.get_color());
					var option = this._fontsDD.findItemByValue(textSettings.get_fontFamily());
					if (option)
						option.select();

					textSettings.set_fontSize(parseInt(textSettings.get_fontSize() * this._getImageZoom()));
					this._draggableText.set_textSettings(textSettings);
					this._textArea.value = textSettings.get_text();
					this.set_position({ x: 0, y: 0 });
				},
				_getDefaultTextSettings: function ()
				{
					var textSettings = new $IE.ImageText();
					textSettings.set_fontFamily("verdana");
					textSettings.set_fontSize(12);
					textSettings.set_color("#000");
					textSettings.set_text(this.get_textToAdd()); //set from code behind def. value "Your text here..."

					return textSettings;
				},
				_getScaledPosition: function (pos, scale)
				{
					var zoomScale = scale || 1 / this._getImageZoom();
					return { x: parseInt(pos.x * zoomScale), y: parseInt(pos.y * zoomScale) };
				},
				_getImageZoom: function ()
				{
					return this.get_imageEditor().getEditableImage().get_zoomLevel() / 100;
				},
				/* key event handlers */
				_changeTextHandler: function ()
				{
					this._draggableText.set_text(this._textArea.value);
				},
				_changeFontSizeHandler: function (event)
				{
					this.set_fontSize(this._getInputValueHandler(event));
				},
				_changeXHandler: function (event)
				{
					this.set_x(this._getInputValueHandler(event));
				},
				_changeYHandler: function (event)
				{
					this.set_y(this._getInputValueHandler(event));
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
				}
			}
			$IE.AddText.registerClass("Telerik.Web.UI.ImageEditor.AddText", $IE.ToolWidget, $IE.IToolWidget);
		})($telerik.$, Telerik.Web.UI, Telerik.Web.UI.ImageEditor);
	}
	//]]>
</script>
