<%@ Control Language="C#" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<table id="Table" class="rieDialogsTable rieDialogsThumbs" border="0" cellpadding="0"
	cellspacing="0" runat="server">
	<tr>
		<td class="rieDialogsThumbsLeft">
			<a id="FlipNone" runat="server" href="javascript:void(0)" class="rieThumbsLink" title="Flip None">
				<img id="ImgFlipNone" runat="server" alt="Flip None" />
			</a>
		</td>
		<td>
			<a id="FlipHorizontal" runat="server" href="javascript:void(0)" class="rieThumbsLink"
				title="Flip Horizontally">
				<img id="ImgFlipHorizontal" runat="server" alt="Flip Horizontally" />
			</a>
		</td>
	</tr>
	<tr class="rieThumbsBottomRow">
		<td class="rieDialogsThumbsLeft">
			<a id="FlipVertical" runat="server" href="javascript:void(0)" class="rieThumbsLink"
				title="Flip Vertically">
				<img id="ImgFlipVertical" runat="server" alt="Flip Vertically" />
			</a>
		</td>
		<td>
			<a id="FlipBoth" runat="server" href="javascript:void(0)" class="rieThumbsLink" title="Flip Horizontally and Vertically">
				<img id="ImgFlipBoth" runat="server" alt="Flip Horizontally and Vertically" />
			</a>
		</td>
	</tr>
</table>
<script type="text/javascript">
	//<![CDATA[
	//Register the class only if it has not been defined
	if (typeof (Telerik.Web.UI.ImageEditor.Flip) === "undefined")
	{
		Type.registerNamespace("Telerik.Web.UI.ImageEditor");
		(function ($, $T, $IE)
		{
			$IE.Flip = function (imageEditor)
			{
				$IE.Flip.initializeBase(this, [imageEditor]);

				this._anchors = null;
			}

			$IE.Flip.prototype =
			{
				initialize: function ()
				{
					$IE.Flip.callBaseMethod(this, "initialize");

					this._attachHandlers(true);
				},

				dispose: function ()
				{
					this._attachHandlers(false);

					$IE.Flip.callBaseMethod(this, "dispose");
				},

				//flips the image in the specified direction
				flip: function (flipDirection)
				{
					var imageEditor = this.get_imageEditor();
					imageEditor.flipImage(flipDirection, true, true);
				},

				get_table: function ()
				{
					if (!this._table) { this._table = this._getChildElement("Table"); }
					return this._table;
				},

				_getChildElement: function (id, parentElement)
				{
					return $get(this.get_parentId() + id, parentElement);
				},

				get_allAnchors: function ()
				{
					if (!this._anchors) { this._anchors = this.get_table().getElementsByTagName("a"); }
					return this._anchors;
				},

				get_activeAnchor: function ()
				{
					if (!this._activeAnchor)
					{
						var anchors = this.get_allAnchors();
						var length = anchors.length;
						for (var i = 0; i < length; i++)
						{
							if (anchors[i].className.indexOf("rieActiveThumbsLink") != -1)
							{
								this._activeAnchor = anchors[i];
								return this._activeAnchor;
							}
						}
					}
					return this._activeAnchor;
				},

				_attachHandlers: function (toAttach)
				{
					var i, length;
					var anchors = this.get_allAnchors();
					if (toAttach)
					{
						if (anchors)
						{
							this._anchorClickDelegate = Function.createDelegate(this, this._anchorClick);
							length = anchors.length;
							for (i = 0; i < length; i++)
							{
								$addHandler(anchors[i], "click", this._anchorClickDelegate);
							}
						}
					}
					else
					{
						if (anchors)
						{
							length = anchors.length;
							for (i = 0; i < length; i++)
							{
								$removeHandler(anchors[i], "click", this._anchorClickDelegate);
							}
							this._anchorClickDelegate = null;
						}
					}
				},

				_anchorClick: function (args)
				{
					var targetElement = args.rawEvent.currentTarget;
					//IE6,7,8 do not have currentTarget
					if (!targetElement)
					{
						targetElement = args.rawEvent.srcElement;
						if (typeof (targetElement.src) != 'undefined')
							targetElement = targetElement.parentNode;
					}

					this._activateAnchor(targetElement);
					this._UIupdated = true;

					var fDirection = this._getFlipDirectionFromId(targetElement.id.substring(this.get_parentId().length));
					this.flip(fDirection);

					$telerik.cancelRawEvent(args.rawEvent);
					return false;
				},

				_updateUI: function ()
				{
					if (this._UIupdated)
					{
						this._UIupdated = false;
						return;
					}

					//get the id of the active anchor based on the current image settings
					var editableImage = this.get_imageEditor().getEditableImage();
					var flipDirection = editableImage.get_flipDirection();
					var isHorizontal = editableImage._getIsHorizontal();
					var id = this._getAnchorID(editableImage._getFlipDirections(flipDirection), isHorizontal);

					//make the new anchor active
					this._activateAnchor(this._getChildElement(id, this.get_table()));
				},

				_activateAnchor: function (element)
				{
					//if there is an active anchor deactivate it
					var activeAnchor = this.get_activeAnchor();
					if (activeAnchor) $(activeAnchor).removeClass("rieActiveThumbsLink");

					//make the new anchor active
					this._activeAnchor = element;
					$(element).addClass("rieActiveThumbsLink");
				},

				_getAnchorID: function (fDirs, isHorizontal)
				{
					var fH = isHorizontal ? fDirs.flipH : fDirs.flipV;
					var fV = isHorizontal ? fDirs.flipV : fDirs.flipH;
					return fV && fH ? "FlipBoth" : (fV ? "FlipVertical" : (fH ? "FlipHorizontal" : "FlipNone"));
				},

				_getFlipDirectionFromId: function (id)
				{
					return (id.indexOf("FlipB") != -1) ? $IE.FlipDirection.Both : (id.indexOf("FlipV") != -1 ? $IE.FlipDirection.Vertical : (id.indexOf("FlipH") != -1 ? $IE.FlipDirection.Horizontal : $IE.FlipDirection.None));
				},

				_flipInternal: function (image, flipV, flipH)
				{
				},

				/*=================================  Telerik.Web.UI.ImageEditor.IToolWidged members  ===============================*/
				get_name: function ()
				{
					return "Flip";
				},

				updateUI: function ()
				{
					this._updateUI();
				},

				onOpen: function ()
				{
					$IE.Flip.callBaseMethod(this, "onOpen");
				}
				/*=================================  Telerik.Web.UI.ImageEditor.IToolWidged members  ===============================*/
			};
			$IE.Flip.registerClass("Telerik.Web.UI.ImageEditor.Flip", $IE.ToolWidget, $IE.IToolWidget);
		})($telerik.$, Telerik.Web.UI, Telerik.Web.UI.ImageEditor);
	}
	//]]>
</script>
