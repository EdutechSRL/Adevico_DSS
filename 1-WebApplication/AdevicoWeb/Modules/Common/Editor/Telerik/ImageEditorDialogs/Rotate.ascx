<%@ Control Language="C#" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<table id="Table" class="rieDialogsTable rieRotateTable" border="0" cellpadding="0" cellspacing="0"
	runat="server">
	<tr>
		<td>
			<telerik:RadButton ID="RotateRight" runat="server" ToolTip="Rotate Right" AutoPostBack="false"
				CausesValidation="false" EnableViewState="false" CommandName="RotateRight" Width="20px"
				CssClass="rieZIndex_IE9">
				<Icon PrimaryIconCssClass="rieRotateRight" PrimaryIconTop="4px" PrimaryIconLeft="5px" />
			</telerik:RadButton>
			<telerik:RadButton ID="RotateLeft" runat="server" ToolTip="Rotate Left" AutoPostBack="false"
				CausesValidation="false" EnableViewState="false" CommandName="RotateLeft" Width="20px"
				Style="margin-right: 20px;" CssClass="rieZIndex_IE9">
				<Icon PrimaryIconCssClass="rieRotateLeft" PrimaryIconTop="4px" PrimaryIconLeft="5px" />
			</telerik:RadButton>
			<telerik:RadComboBox ID="DdlDegrees" runat="server" AutoPostBack="false" EnableViewState="false"
				Width="60px">
				<Items>
					<telerik:RadComboBoxItem Text="0" Value="0" />
					<telerik:RadComboBoxItem Text="90" Value="90" />
					<telerik:RadComboBoxItem Text="180" Value="180" />
					<telerik:RadComboBoxItem Text="270" Value="270" />
				</Items>
			</telerik:RadComboBox>
			<asp:Label ID="Label1" Text="&deg;" runat="server" Style="font-size: 20px; padding-left: 3px;" />
		</td>
	</tr>
</table>
<script type="text/javascript">
	//<![CDATA[
	//Register the class only if it has not been defined
	if (typeof (Telerik.Web.UI.ImageEditor.Rotate) === "undefined")
	{
		Type.registerNamespace("Telerik.Web.UI.ImageEditor");
		(function ($, $T, $IE)
		{
			$IE.Rotate = function (imageEditor)
			{
				$IE.Rotate.initializeBase(this, [imageEditor]);

				this._rotateLeftBtn = null;
				this._rotateRightBtn = null;
			}

			$IE.Rotate.prototype =
			{
				initialize: function ()
				{
					$IE.Rotate.callBaseMethod(this, "initialize");

					this._attachHandlers(true);
				},

				dispose: function ()
				{
					this._attachHandlers(false);

					$IE.Rotate.callBaseMethod(this, "dispose");
				},

				onClose: function ()
				{
					this.get_ddlDegrees().hideDropDown();
				},

				get_ddlDegrees: function ()
				{
					if (!this._ddlDegrees)
					{
						this._ddlDegrees = $find(this.get_parentId() + "DdlDegrees");
					}
					return this._ddlDegrees;
				},

				get_rotateLeftBtn: function ()
				{
					if (!this._rotateLeftBtn)
					{
						this._rotateLeftBtn = $find(this.get_parentId() + "RotateLeft");
					}
					return this._rotateLeftBtn;
				},

				get_rotateRightBtn: function ()
				{
					if (!this._rotateRightBtn)
					{
						this._rotateRightBtn = $find(this.get_parentId() + "RotateRight");
					}
					return this._rotateRightBtn;
				},

				_attachHandlers: function (toAttach)
				{
					var rotateLeftBtn = this.get_rotateLeftBtn();
					var rotateRightBtn = this.get_rotateRightBtn();
					var ddlDegrees = this.get_ddlDegrees();
					if (toAttach)
					{
						this._btnClickDelegate = Function.createDelegate(this, this._btnClick);
						if (rotateLeftBtn && rotateRightBtn)
						{
							rotateRightBtn.add_clicked(this._btnClickDelegate);
							rotateLeftBtn.add_clicked(this._btnClickDelegate);
						}
						if (ddlDegrees)
						{
							this._degreeChangedDelegate = Function.createDelegate(this, this._degreeChanged);
							ddlDegrees.add_selectedIndexChanged(this._degreeChangedDelegate);
						}
					}
					else
					{
						if (rotateLeftBtn && rotateRightBtn)
						{
							rotateRightBtn.remove_clicked(this._btnClickDelegate);
							rotateLeftBtn.remove_clicked(this._btnClickDelegate);
						}
						this._btnClickDelegate = null;
						if (ddlDegrees)
						{
							ddlDegrees.remove_selectedIndexChanged(this._degreeChangedDelegate);
							this._degreeChangedDelegate = null;
						}
					}
				},

				_btnClick: function (sender, args)
				{
					var imageEditor = this.get_imageEditor();
					if (args.get_commandName() == "RotateRight") { imageEditor.rotateRight90(); }
					else { imageEditor.rotateLeft90(); }
				},

				_degreeChanged: function (sender, args)
				{
					if (!this._alreadyRotated)
					{
						this.get_imageEditor()._rotateImage(null, true, false, parseInt(args.get_item().get_value()));
					}
				},

				/*=================================  Telerik.Web.UI.ImageEditor.IToolWidged members  ===============================*/
				get_name: function ()
				{
					return "Rotate";
				},

				updateUI: function ()
				{
					var dock = this.get_imageEditor().get_toolsPanel();
					var dockWidth = parseInt(dock.get_width());
					if (dockWidth == 270)
					{
						dock.set_width(180);
						dock.repaint();
					}
					else if (dockWidth == 360)
					{
						dock.set_width(220);
						dock.repaint();
					}
					//Bootstrap Case
					else if (dockWidth == 310)
					{
						dock.set_width(200);
						dock.repaint();
					}
					//change combo
					var editableImage = this.get_imageEditor().getEditableImage();
					var selectedIndex = parseInt(editableImage ? (editableImage.get_rotationAngle() / 90) : 0);
					var combo = this.get_ddlDegrees();
					this._alreadyRotated = true;
					combo.get_items()._array[selectedIndex].select();
					this._alreadyRotated = false;
				}
				/*=================================  Telerik.Web.UI.ImageEditor.IToolWidged members  ===============================*/
			};
			$IE.Rotate.registerClass("Telerik.Web.UI.ImageEditor.Rotate", $IE.ToolWidget, $IE.IToolWidget);
		})($telerik.$, Telerik.Web.UI, Telerik.Web.UI.ImageEditor);
	}
	//]]>
</script>
