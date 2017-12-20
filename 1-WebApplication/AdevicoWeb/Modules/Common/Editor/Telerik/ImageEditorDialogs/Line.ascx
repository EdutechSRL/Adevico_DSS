<%@ Control Language="C#" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<table id="Table" class="rieDialogsTable" border="0" cellpadding="0" cellspacing="0"
	runat="server">
	<tr>
		<td class="rieRightAligned">
			<label id="lblColor" runat="server">
				Color:</label>
		</td>
		<td>
			<telerik:RadColorPicker ID="DrawColorPicker" runat="server" ShowIcon="true" AutoPostBack="false"
				CssClass="rieColorPicker" EnableViewState="false" SelectedColor="#000">
				<Localization PickColorText="Pick Color" CurrentColorText="(Current color is {0})" />
			</telerik:RadColorPicker>
		</td>
	</tr>
	<tr>
		<td class="rieRightAligned">
			<label id="lblLineSize" runat="server">
				Line size:</label>
		</td>
		<td>
			<telerik:RadComboBox ID="SizeCombo" runat="server" Width="70" RenderMode="Lightweight" EnableViewState="false">
				<Items>
					<telerik:RadComboBoxItem Text="1" Value="1" />
					<telerik:RadComboBoxItem Text="3" Value="3" />
					<telerik:RadComboBoxItem Text="7" Value="7" />
					<telerik:RadComboBoxItem Text="10" Value="10" />
				</Items>
			</telerik:RadComboBox>
		</td>
	</tr>
	<tr>
		<td colspan="2">
			<hr />
		</td>
	</tr>
	<tr>
		<td class="rieRightAligned">
			<label id="lblTransform" runat="server">
				Transform:</label>
		</td>
		<td>
			<telerik:RadButton ID="transformButton" runat="server" Text="Apply" Enabled="false" AutoPostBack="false" EnableViewState="false" />
		</td>
	</tr>
</table>
<script type="text/javascript">
	//<![CDATA[
	//Register the class only if it has not been defined
	if (typeof (Telerik.Web.UI.ImageEditor.Line) === "undefined")
	{
		Type.registerNamespace("Telerik.Web.UI.ImageEditor");
		(function ($, $T, $IE)
		{
			$IE.Line = function (imageEditor)
			{
				this._originalToolsPanelWidth = imageEditor.get_toolsPanel().get_width();
				
				$IE.Line.initializeBase(this, [imageEditor]);

				imageEditor.get_toolsPanel().set_width(190);
			}
			$IE.Line.prototype =
			{
				initialize: function ()
				{
					$IE.Line.callBaseMethod(this, "initialize");

					this.colorPicker = this._findControlFromParent("DrawColorPicker");
					this.sizeCombo = this._findControlFromParent("SizeCombo");
					this.transformButton = this._findControlFromParent("transformButton");
					
					this.onOpen();
					this.attachEventHandlers();
				},

				dispose: function ()
				{
					this.detachEventHandlers();

					$IE.Line.callBaseMethod(this, "dispose");
				},

				onClose: function ()
				{
					this.drawTool.dispose();
					
					var imageEditor = this.get_imageEditor();
					imageEditor._scrollExtender.enable();
					imageEditor.get_toolsPanel().set_width(this._originalToolsPanelWidth);
				},

				onOpen: function ()
				{
					try
					{
						this.createDrawTool();
						this.transformButton.set_enabled(false);
						this.prepareEditorForDraw();
					}
					catch (e) {}
				},
				prepareEditorForDraw: function()
				{
					var imageEditor = this.get_imageEditor();
					imageEditor._scrollExtender.disable();
					imageEditor.get_toolsPanel().set_width(190);
				},

				applyDraw: function(drawTool)
				{
					this.get_imageEditor()._drawOnCanvas(this.drawTool.drawableContext.canvas, true, false, this.get_name());
					this.drawTool.clearCanvas();
					this.transformButton.set_enabled(false);
				},

				createDrawTool: function()
				{
					this.drawTool = new $IE.DrawTools.LineTool(this.get_imageEditor().getCanvasContext(), { coreFactory: new $IE.GraphicsCore.CanvasGraphicsCoreFactory() });
					
					this.drawTool.setupDrawCanvas();
					this.drawTool.onEnd = Function.createDelegate(this, this._onDrawEnd);
					this.drawTool.setColor(this.colorPicker.get_selectedColor());
					this.drawTool.setLineSize(this.sizeCombo.get_selectedItem().get_value());
				},

				attachEventHandlers: function()
				{
					this._changeColorDelegate = Function.createDelegate(this, this._changeColorHandler);
					this.colorPicker.add_colorChange(this._changeColorDelegate);

					this._transformButtonClickedDelegate = Function.createDelegate(this, this._transformButtonClicked);
					this.transformButton.add_clicked(this._transformButtonClickedDelegate);

					this._sizeChangedDelegate = Function.createDelegate(this, this._sizeChangedHandler);
					this.sizeCombo.add_selectedIndexChanged(this._sizeChangedDelegate);
				},
				detachEventHandlers: function()
				{
					this.colorPicker.remove_colorChange(this._changeColorDelegate);
					delete this._changeColorDelegate;

					this.transformButton.remove_clicked(this._transformButtonClickedDelegate);
					delete this._transformButtonClickedDelegate;
				},

				_changeColorHandler: function(colorPicker, args)
				{
					this.drawTool.setColor(colorPicker.get_selectedColor());
				},
				_sizeChangedHandler: function(combo, args)
				{
					this.drawTool.setLineSize(args.get_item().get_value());
				},

				_onDrawEnd: function(drawTool, e)
				{
					this.transformButton.set_enabled(true);
					this.drawTool.showOpacityBackground();
				},

				_transformButtonClicked: function(button, args)
				{
					this.applyDraw();
				},

				/*=================================  Telerik.Web.UI.ImageEditor.IToolWidged members  ===============================*/
				get_name: function () { return "Line"; },

				updateUI: function () {}
				/*=================================  Telerik.Web.UI.ImageEditor.IToolWidged members  ===============================*/
			};
			$IE.Line.registerClass("Telerik.Web.UI.ImageEditor.Line", $IE.ToolWidget, $IE.IToolWidget);
		})($telerik.$, Telerik.Web.UI, Telerik.Web.UI.ImageEditor);
	}
	//]]>
</script>