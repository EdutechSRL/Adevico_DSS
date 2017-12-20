<%@ Control Language="C#" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<table id="Table" class="rieDialogsTable" border="0" cellpadding="0" cellspacing="0"
	runat="server">
	<tr>
		<td colspan="2">
			<asp:RadioButtonList ID="btnList" runat="server" AutoPostBack="false" EnableViewState="false"
				CausesValidation="false">
				<asp:ListItem Text="Download Image" />
				<asp:ListItem Text="Save Image to Server" />
			</asp:RadioButtonList>
		</td>
	</tr>
	<tr>
		<td class="rieFileName">
		</td>
		<td>
			<asp:Label ID="saveFileName" Text="File Name:" runat="server" AssociatedControlID="TxtFileName" /><br />
			<asp:TextBox ID="TxtFileName" runat="server" Width="150px" EnableViewState="false" ToolTip="Enter File Name" />
			<telerik:RadDropDownList ID="fileExtension" runat="server" Width="67" EnableViewState="false">
				<Items>
					<telerik:DropDownListItem Text=".png" Value="png" />
					<telerik:DropDownListItem Text=".jpg" Value="jpg" />
					<telerik:DropDownListItem Text=".gif" Value="gif" />
					<telerik:DropDownListItem Text=".bmp" Value="bmp" />
				</Items>
			</telerik:RadDropDownList>
		</td>
	</tr>
	<tr>
		<td>
		</td>
		<td class="rieOverwriteFile">
			<asp:CheckBox ID="cbOverwrite" runat="server" Checked="true" Text="Overwrite file"
				AutoPostBack="false" EnableViewState="false" CausesValidation="false" />
		</td>
	</tr>
	<tr>
		<td class="rieRightAligned" colspan="2">
			<telerik:RadButton ID="btnOk" runat="server" Text="OK" ToolTip="OK" AutoPostBack="false"
				CommandName="OK" CausesValidation="false" EnableViewState="false">
			</telerik:RadButton>
			<telerik:RadButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" AutoPostBack="false"
				CommandName="Cancel" CausesValidation="false" EnableViewState="false">
			</telerik:RadButton>
		</td>
	</tr>
</table>
<script type="text/javascript">
	//<![CDATA[
	//Register the class only if it has not been defined
	if (typeof (Telerik.Web.UI.ImageEditor.Export) === "undefined")
	{
		Type.registerNamespace("Telerik.Web.UI.ImageEditor");
		(function ($, $T, $IE)
		{
			$IE.Export = function (imageEditor)
			{
				$IE.Export.initializeBase(this, [imageEditor]);

				this._okBtn = null;
				this._cancelBtn = null;
			}

			$IE.Export.prototype =
			{
				initialize: function ()
				{
					this._ddExtension = this._findControlFromParent("fileExtension");
					
					$IE.Export.callBaseMethod(this, "initialize");

					this._attachHandlers(true);
					//decorate the textbox and other buttons
					this.get_imageEditor().get_formDecorator().decorate(this.get_textBox().parentNode.parentNode.parentNode);
				},

				dispose: function ()
				{
					this._attachHandlers(false);

					$IE.Export.callBaseMethod(this, "dispose");
				},

				//gets the textbox that shows the current opacity on the image
				get_textBox: function ()
				{
					if (!this._textBox)
					{
						this._textBox = $get(this.get_parentId() + "TxtFileName");
					}
					return this._textBox;
				},

				get_radioDownload: function ()
				{
					if (!this._radioDownload)
					{
						this._radioDownload = $get(this.get_parentId() + "btnList_0");
					}
					return this._radioDownload;
				},

				get_checkBoxOverwrite: function ()
				{
					if (!this._cbOverwrite)
					{
						this._cbOverwrite = $get(this.get_parentId() + "cbOverwrite");
					}
					return this._cbOverwrite;
				},

				get_okBtn: function ()
				{
					if (!this._okBtn)
					{
						this._okBtn = $find(this.get_parentId() + "btnOk");
					}
					return this._okBtn;
				},

				get_cancelBtn: function ()
				{
					if (!this._cancelBtn)
					{
						this._cancelBtn = $find(this.get_parentId() + "btnCancel");
					}
					return this._cancelBtn;
				},

				_attachHandlers: function (toAttach)
				{
					var serverBtn = this.get_okBtn();
					var clientBtn = this.get_cancelBtn();

					if (toAttach)
					{
						this._buttonClickDelegate = Function.createDelegate(this, this._buttonClick);
						if (serverBtn) serverBtn.add_clicked(this._buttonClickDelegate);
						if (clientBtn) clientBtn.add_clicked(this._buttonClickDelegate);
					}
					else
					{
						if (serverBtn) serverBtn.remove_clicked(this._buttonClickDelegate);
						if (clientBtn) clientBtn.remove_clicked(this._buttonClickDelegate);
						this._buttonClickDelegate = null;
					}
				},

				_buttonClick: function (sender, args)
				{
					var name = this.get_textBox().value;
					var ext = this._ddExtension ? this._ddExtension.get_selectedItem().get_value() : null;
					var imageEditor = this.get_imageEditor();
					var download = this.get_radioDownload().checked;
					var overwrite = this.get_checkBoxOverwrite().checked;
					if(args.get_commandName() == "OK")
					{
						var fileName = ext ? {name: name, ext: ext} : fileName;
						if(download)
							imageEditor.saveImageOnClient(fileName);
						else
						{
							imageEditor.saveImageOnServer(fileName, overwrite);
						}
					}
					imageEditor.closeToolsPanel();
				},

				/*=================================  Telerik.Web.UI.ImageEditor.IToolWidged members  ===============================*/
				get_name: function ()
				{
					return "Export";
				},

				updateUI: function ()
				{
				}
				/*=================================  Telerik.Web.UI.ImageEditor.IToolWidged members  ===============================*/
			};
			$IE.Export.registerClass("Telerik.Web.UI.ImageEditor.Export", $IE.ToolWidget, $IE.IToolWidget);
		})($telerik.$, Telerik.Web.UI, Telerik.Web.UI.ImageEditor);
	}
	//]]>
</script>
