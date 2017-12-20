<%@ Control Language="C#" %>

<script type="text/javascript">
	(function ($, $T, global, undefined) {
		Type.registerNamespace("Telerik.Web.UI.Dialogs");

		var Dialogs = $T.Dialogs;
		var delegate = Function.createDelegate;
		var onEvent = $telerik.onEvent;
		var offEvent = $telerik.offEvent;
		var iconSelectedCssClass = "reIconSelected";
		var hiddenCssClass = "reHidden";
		var iconSelector = ".reIcon";
		var click = "click.reMobileImageManagerDialog";
		var gridView = "grid";
		var thumbnailsView = "thumbnails";
		var disabledIconCssClass = "reIconDisabled";

		Dialogs.MobileImageManager = function (element) {
			Dialogs.MobileImageManager.initializeBase(this, [element]);
		};

		Dialogs.MobileImageManager.prototype = {
			initialize: function () {
				var that = this;
				Dialogs.MobileImageManager.callBaseMethod(that, "initialize");
				that.collectUI();
				that.localizeUI();
				that.initilaizeAsyncUploader();
				that.attachEventHandlers();
				that.setViewButtonState();

				that._updateDeleteButtonState();
			},

			collectUI: function () {
				var that = this;
				that.uploadBtn = $get("uploadBtn");
				that.deleteBtn = $get("deleteBtn");
				that.gridViewBtn = $get("iconGridBtn");
				that.thumbnailViewBtn = $get("iconThumbnailBtn");
				that.uploadContainer = $get("reUploderContainer");
				that.asyncUpload = that.get_fileBrowser().get_asyncUpload();

				that.jqUploadBtnIcon = $(that.uploadBtn).find(iconSelector);
				that.jqDeleteBtnIcon = $(that.deleteBtn).find(iconSelector);
				that.jqGridViewBtnIcon = $(that.gridViewBtn).find(iconSelector);
				that.jqThumbnailViewBtnIcon = $(that.thumbnailViewBtn).find(iconSelector);
				that.jqViewButtonIcons = that.jqGridViewBtnIcon.add(that.jqThumbnailViewBtnIcon);
			},

			localizeUI: function () {
				var localizationStr = global.localization;
				var that = this;

				$(that.uploadBtn).attr("title", localizationStr["Upload"]);
				$(that.deleteBtn).attr("title", localizationStr["Delete"]);
				$(that.gridViewBtn).attr("title", localizationStr["GridView"]);
				$(that.thumbnailViewBtn).attr("title", localizationStr["ThumbnailsView"]);
			},

			initilaizeAsyncUploader: function () {
				var that = this;
				var jqUploadContainer = $(that.uploadContainer);
				if (that.asyncUpload) {
					jqUploadContainer.prepend(that.asyncUpload.get_element());
				}
				else {
					$(that.uploadBtn).closest("li").addClass(hiddenCssClass);
				}
				jqUploadContainer.addClass(hiddenCssClass);
			},

			attachEventHandlers: function () {
				var that = this;
				
				that.deleteBtnClickDelegate = delegate(that, that._deleteBtnClickHandler);
				that.iconGridBtnClickDelegate = delegate(that, that._iconGridBtnClickHandler);
				that.iconThumbnailBtnClickDelegate = delegate(that, that._iconThumbnailBtnClickHandler);

				onEvent(that.deleteBtn, click, that.deleteBtnClickDelegate);
				onEvent(that.gridViewBtn, click, that.iconGridBtnClickDelegate);
				onEvent(that.thumbnailViewBtn, click, that.iconThumbnailBtnClickDelegate);

				if (that.asyncUpload) {
					that.uploadBtnClickDelegate = delegate(that, that._uploadBtnClickHandler);
					onEvent(that.uploadBtn, click, that.uploadBtnClickDelegate);
				}

				var titlebar = that.get_titlebar();
				titlebar.add_ok(delegate(that, that._clearAsyncUploadItems));
				titlebar.add_cancel(delegate(that, that._clearAsyncUploadItems));

				var fileBrowser = that.get_fileBrowser();
				that.updateDeleteButtonStateDelegate = delegate(that, that._updateDeleteButtonState);

				fileBrowser.add_itemSelected(that.updateDeleteButtonStateDelegate);
				fileBrowser.add_folderChange(that.updateDeleteButtonStateDelegate);

				onEvent(fileBrowser.get_element(), click, that.updateDeleteButtonStateDelegate);
			},

			detachEventHandlers: function () {
				var that = this;
				
				offEvent(that.deleteBtn, click, that.deleteBtnClickDelegate);
				offEvent(that.gridViewBtn, click, that.iconGridBtnClickDelegate);
				offEvent(that.thumbnailViewBtn, click, that.iconThumbnailBtnClickDelegate);
				offEvent(that.get_fileBrowser().get_element(), click, that.updateDeleteButtonStateDelegate);
				
				delete that.deleteBtnClickDelegate;
				delete that.iconGridBtnClickDelegate;
				delete that.iconThumbnailBtnClickDelegate;
				delete that.updateDeleteButtonStateDelegate;

				if (that.asyncUpload) {
					offEvent(that.uploadBtn, click, that.uploadBtnClickDelegate);
					delete that.uploadBtnClickDelegate;
				}
			},

			collectResult: function() {
				var that = this;
				var selectedItems = that.get_fileBrowser().get_selectedItems();
				var result = [];

				Array.forEach(selectedItems, function(item) {
					if(!item.isDirectory()) {
						var image = new Image();
						image.src = item.get_url();
						image.setAttribute("alt", "");
						result.push(image);
					}
				});

				return result;
			},

			dispose: function () {
				var that = this;
				
				that.detachEventHandlers();
				Dialogs.MobileImageManager.callBaseMethod(that, "dispose");
				
				delete that.uploadBtn;
				delete that.deleteBtn;
				delete that.gridViewBtn;
				delete that.thumbnailViewBtn;
				delete that.uploadContainer;
				delete that.asyncUpload;
				delete that.jqUploadBtnIcon;
				delete that.jqDeleteBtnIcon;
				delete that.jqGridViewBtnIcon;
				delete that.jqThumbnailViewBtnIcon;
				delete that.jqViewButtonIcons;
			},

			_uploadBtnClickHandler: function (ev) {
				var that = this;

				that.jqUploadBtnIcon.toggleClass(iconSelectedCssClass);
				$(that.uploadContainer).toggleClass(hiddenCssClass);
			},

			_clearAsyncUploadItems: function () {
				var that = this;
				that.jqUploadBtnIcon.removeClass(iconSelectedCssClass);
				var jqUploadContainer = $(that.uploadContainer);
				if (!jqUploadContainer.hasClass(hiddenCssClass)) {
					jqUploadContainer.addClass(hiddenCssClass);
				}

				var asyncUpload = that.asyncUpload;
				if (asyncUpload) {
					asyncUpload.deleteAllFileInputs();
				}
			},

			_deleteBtnClickHandler: function () {
				var that = this;
				if (that._enabled) {
					var fileBrowser = that.get_fileBrowser();
					if (global.confirm(fileBrowser.get_localization()["ConfirmDelete"])) {
						fileBrowser.deleteSelectedItems(false);
						that.jqDeleteBtnIcon.addClass(disabledIconCssClass);
					}
				}
			},

			_iconGridBtnClickHandler: function () {
				this._switchToView(gridView);
			},

			_iconThumbnailBtnClickHandler: function () {
				this._switchToView(thumbnailsView);
			},

			_switchToView: function (viewName) {
				var that = this;
				var fileList = that.get_fileBrowser().get_fileList();
				if (fileList.getFileListViewByName(viewName) !== fileList.get_view()) {
					fileList.chooseFileListView(viewName);
					that.setViewButtonState();
				}
			},

			_updateDeleteButtonState: function () {
				var that = this;
				that._enabled = that._shouldEnableDelete();
				that.jqDeleteBtnIcon[that._enabled ? "removeClass" : "addClass"](disabledIconCssClass);
			},

			_shouldEnableDelete: function() {
				var items = this.get_fileBrowser().get_fileList().get_selectedItems();
				if (items.length) {
					for (var i = 0; i < items.length; i++) {
						if (items[i].Name === "..") {
							return false;
						}
					}
					return true;
				}
				return false;
			},

			setViewButtonState: function () {
				var that = this;
				var fileList = that.get_fileBrowser().get_fileList();
				var view = fileList.get_view();

				that.jqViewButtonIcons.removeClass(iconSelectedCssClass);

				if (fileList.getFileListViewByName(thumbnailsView) === view) {
					that.jqThumbnailViewBtnIcon.addClass(iconSelectedCssClass);
				}
				else if (fileList.getFileListViewByName(gridView) === view) {
					that.jqGridViewBtnIcon.addClass(iconSelectedCssClass);
				}
			}
		};

		Dialogs.MobileImageManager.registerClass('Telerik.Web.UI.Dialogs.MobileImageManager', 
			$T.Widgets.MobileFileBrowser, Telerik.Web.IParameterConsumer);
	})($telerik.$, Telerik.Web.UI, window);
</script>

<ul class="reToolList reImageManagerHeader t-hbox">
	<li>
		<span id="uploadBtn" role="button" title="Upload" class="reButton reUpload">
			<span class="reIcon reIconUpload" unselectable="on"></span>
		</span>
	</li>
	<li>
		<span id="deleteBtn" role="button" title="Delete" class="reButton reDelete">
			<span class="reIcon reIconDelete" unselectable="on"></span>
		</span>
	</li>
	<li class="t-spacer"></li>
	<li>
		<span id="iconGridBtn" role="button" title="GridView" class="reButton reListView" unselectable="on" aria-pressed="true">
			<span class="reIcon reIconList" unselectable="on"></span>
		</span>
	</li>
	<li>
		<span id="iconThumbnailBtn" role="button" title="ThumbnailView" class="reButton reThumbnailView" unselectable="on" aria-pressed="true">
			<span class="reIcon reIconThumbnail" unselectable="on"></span>
		</span>
	</li>
</ul>
<ul class="reToolList reDialogSettings reUploadContainer">
	<li id="reUploderContainer">
		<telerik:RadButton ID="UploadButton" runat="server" RenderMode="Mobile" CausesValidation="false" CssClass="reUpload" />
	</li>
	<li class="reFileExplorerContainer">
		<telerik:RadFileExplorer ID="RadFileExplorer1" runat="Server" Width="100%" Height="200px" EnableAsyncUpload="true" RenderMode="Mobile" />
	</li>
</ul>