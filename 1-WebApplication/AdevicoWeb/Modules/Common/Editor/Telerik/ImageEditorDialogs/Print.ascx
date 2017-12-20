<%@ Control Language="C#" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<table id="Table" class="rieDialogsTable" border="0" cellpadding="0" cellspacing="0"
	runat="server">
	<tr>
		<td class="riePrintContent">
			<asp:Image ID="printImageOverview" ImageUrl="imageurl" runat="server" AlternateText="Printed Image"
				Width="230px" />
		</td>
	</tr>
	<tr>
		<td class="rieRightAligned" style="padding-top: 15px;">
			<telerik:RadButton ID="btnPrint" runat="server" Text="Print" ToolTip="Print" AutoPostBack="false"
				CausesValidation="false" EnableViewState="false" CommandName="Print">
				<Icon PrimaryIconCssClass="rbPrint" />
			</telerik:RadButton>
			<telerik:RadButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" AutoPostBack="false"
				CausesValidation="false" EnableViewState="false" CommandName="Cancel">
				<Icon PrimaryIconCssClass="rbCancel" PrimaryIconTop="4px" PrimaryIconLeft="7px" />
			</telerik:RadButton>
		</td>
	</tr>
</table>
<script type="text/javascript">
	//<![CDATA[
	//Register the class only if it has not been defined
	if (typeof (Telerik.Web.UI.ImageEditor.Print) === "undefined")
	{
		Type.registerNamespace("Telerik.Web.UI.ImageEditor");
		(function ($, $T, $IE)
		{
			$IE.Print = function (imageEditor)
			{
				$IE.Print.initializeBase(this, [imageEditor]);

				this._printBtn = null;
				this._cancelBtn = null;
			}

			$IE.Print.prototype =
			{
				initialize: function ()
				{
					$IE.Print.callBaseMethod(this, "initialize");

					this._printImage = this._getControlFromParent("printImageOverview");
					var canvas = this.get_imageEditor().getCanvasElement();
					if(canvas)
						this._printImage.src = this.get_imageEditor().getDataURL();

					this._attachHandlers(true);
				},

				print: function ()
				{
					try
					{
						if ($telerik.isOpera)//opera would print the whole page if using the iframe mode. Therefore try the funky workaround
							this.printFromCurrentPage();
						else
							this.printFromIframe();
					}
					catch (e) { }
				},
				printFromIframe: function ()
				{
					$("#print-image-frame").remove();
					var iframe = $("<iframe src='about:blank' name='print-image-frame' frameborder='0' scrolling='no' id='print-image-frame'>");
					iframe.css({ border: "none", position: "absolute", width: 0, height: 0, bottom: 0, left: 0 });
					$("body").append(iframe);

					var frameWindow = iframe[0].contentWindow || iframe[0].contentDocument;
					var frameDocument = frameWindow.document || frameWindow;

					if (!$.isWindow(frameWindow))
						frameWindow = window.frames["print-image-frame"];

					this._writeImageToFrameDocument(frameDocument);

					if ($telerik.isIE)
						frameDocument.execCommand("Print", null, false);
					else
						frameWindow.print();
				},
				printFromCurrentPage: function ()
				{
					var image = this._getPrintImage();
					image.attr("id", "print-image").css({ display: "block", position: "absolute", top: "6px", left: "1px", zIndex: 10000, visibility: "visible" });
					var body = $("body");
					var bodyOriginalCSS = {
						visibility: body.css("visibility"),
						overflow: body.css("overflow"),
						background: body.css("background")
					};
					body.css({ visibility: "hidden", overflow: "hidden", background: "transparent" });
					body.append(image);

					window.print();

					image.remove();
					body.css(bodyOriginalCSS);
				},

				dispose: function ()
				{
					this._attachHandlers(false);

					$IE.Print.callBaseMethod(this, "dispose");
				},

				get_printBtn: function ()
				{
					if (!this._printBtn)
					{
						this._printBtn = this._findControlFromParent("btnPrint");
					}
					return this._printBtn;
				},

				get_cancelBtn: function ()
				{
					if (!this._cancelBtn)
					{
						this._cancelBtn = this._findControlFromParent("btnCancel");
					}
					return this._cancelBtn;
				},

				_attachHandlers: function (toAttach)
				{
					var printBtn = this.get_printBtn();
					var cancelBtn = this.get_cancelBtn();

					if (toAttach)
					{
						this._buttonClickDelegate = Function.createDelegate(this, this._buttonClick);
						if (printBtn) printBtn.add_clicked(this._buttonClickDelegate);
						if (cancelBtn) cancelBtn.add_clicked(this._buttonClickDelegate);
					}
					else
					{
						if (printBtn) printBtn.remove_clicked(this._buttonClickDelegate);
						if (cancelBtn) cancelBtn.remove_clicked(this._buttonClickDelegate);
						this._buttonClickDelegate = null;
					}
				},

				_buttonClick: function (sender, args)
				{
					if (args.get_commandName() == "Print")
						this.print();

					this.close();
				},

				_writeImageToFrameDocument: function (frameDocument)
				{
					var image = this._getPrintImage();
					frameDocument.open();
					frameDocument.write($("<div/>").append(image).html());
					frameDocument.close();
				},

				_getPrintImage: function ()
				{
					var imageEditor = this.get_imageEditor();
					var canvas = imageEditor.getCanvasElement();
					var image = !!canvas ?
							$("<img>").prop("src", imageEditor.getDataURL()) :
							$(imageEditor.getEditableImage().getImage()).clone();

					return image;
				},

				/*=================================  Telerik.Web.UI.ImageEditor.IToolWidged members  ===============================*/
				get_name: function ()
				{
					return "Print";
				},

				updateUI: function ()
				{
					//var editableImage = this.get_imageEditor().getEditableImage();
					//editableImage._updateCssTransform(editableImage._flipH, editableImage._flipV, editableImage.get_rotationAngle(), this._printImage);
					//editableImage._changeOpacity(this._printImage, editableImage.get_opacity());
				}
				/*=================================  Telerik.Web.UI.ImageEditor.IToolWidged members  ===============================*/
			};
			$IE.Print.registerClass("Telerik.Web.UI.ImageEditor.Print", $IE.ToolWidget, $IE.IToolWidget);
		})($telerik.$, Telerik.Web.UI, Telerik.Web.UI.ImageEditor);
	}
	//]]>
</script>
