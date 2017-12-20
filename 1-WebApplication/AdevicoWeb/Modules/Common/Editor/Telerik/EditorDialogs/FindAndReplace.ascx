<%@ Control Language="C#" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Editor" TagPrefix="tools" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Widgets" TagPrefix="widgets" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Dialogs" TagPrefix="dialogs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<script type="text/javascript">
	//<!--
	Type.registerNamespace("Telerik.Web.UI.Widgets");

	Telerik.Web.UI.Widgets.FindAndReplace = function(element)
	{
		Telerik.Web.UI.Widgets.FindAndReplace.initializeBase(this, [element]);
		this._clientParameters = null;
		this._findButton = null;
		this._rFindButton = null;
		this._replaceButton = null;
		this._replaceAllButton = null;
	}

	Telerik.Web.UI.Widgets.FindAndReplace.prototype = {
		initialize: function ()
		{
			Telerik.Web.UI.Widgets.FindAndReplace.callBaseMethod(this, "initialize");
			this.setupChildren();
		},

		dispose: function ()
		{
			$clearHandlers(this._findButton);
			$clearHandlers(this._rFindButton);
			$clearHandlers(this._replaceButton);
			$clearHandlers(this._replaceAllButton);
			$clearHandlers(this._closeButton);

			this._clientParameters = null;

			this._findReplaceEngine.dispose();
			Telerik.Web.UI.Widgets.FindAndReplace.callBaseMethod(this, "dispose");
		},

		clientInit: function (clientParameters)
		{
			this._clientParameters = clientParameters;
			var editor = clientParameters.editor;

			//feature - copy selected text into the find boxes if it is one line
			this._tab.set_selectedIndex(0);
			var selectedText = editor.getSelection().getText();
			if (selectedText.indexOf("\n") == -1)
			{
				this._rFind.value = selectedText;
				this._find.value = selectedText;
				this._rReplace.value = "";
			}
			else
			{
				this._rFind.value = "";
				this._find.value = "";
				this._rReplace.value = "";
			}

			this._wholeWordFind.checked = false;
			this._wholeWordReplace.checked = false;
			this._matchCaseReplace.checked = false;
			this._matchCaseFind.checked = false;
			this._upReplace.checked = false;
			this._upFind.checked = false;
			this._downReplace.checked = true;
			this._downFind.checked = true;

			this._wrapping = false;

			this.initFindReplaceEngine(editor);
		},

		initFindReplaceEngine: function(editor) {
			this._findReplaceEngine = $create(Telerik.Web.UI.Editor.EditorFindReplaceEngine, {
				editor: editor
			}, null, null, null);
		},

		_showMessageBox: function (msg)
		{
			window.alert(msg);
		},

		execFind: function ()
		{
			var findEngine = this._findReplaceEngine;
			var stringToFind = "";
			var backwards = false;
			var wholeWord = false;
			var caseSensitive = false;

			//find out which tab is active
			if (this._tab.get_selectedIndex() == 0)
			{
				stringToFind = this._find.value;
				wholeWord = this._wholeWordFind.checked;
				caseSensitive = this._matchCaseFind.checked;
				backwards = this._upFind.checked;
			}
			else
			{
				stringToFind = this._rFind.value;
				wholeWord = this._wholeWordReplace.checked;
				caseSensitive = this._matchCaseReplace.checked;
				backwards = this._upReplace.checked;
			}

			findEngine.set_wholeWord(wholeWord);
			findEngine.set_caseSensitive(caseSensitive);

			var result = findEngine.find(stringToFind, backwards);
			this._showFindReplaceResult(result);
		},

		execReplace: function () {
			var replaceEngine = this._findReplaceEngine;
			replaceEngine.set_wholeWord(this._wholeWordReplace.checked);
			replaceEngine.set_caseSensitive(this._matchCaseReplace.checked);


			var result = replaceEngine.replace(this._rFind.value, this._rReplace.value, this._upReplace.checked);
			this._showFindReplaceResult(result);
		},

		execReplaceAll: function ()
		{
			var replaceEngine = this._findReplaceEngine;
			replaceEngine.set_wholeWord(this._wholeWordReplace.checked);
			replaceEngine.set_caseSensitive(this._matchCaseReplace.checked);

			//up/down does not matter here so we send "false" (down)
			//focus the find box so the editor cursor position is lost
			if (this._rFind.focus)
				this._rFind.focus();

			var result = replaceEngine.replaceAll(this._rFind.value, this._rReplace.value, this._upReplace.checked);
			this._showFindReplaceResult(result);
		},

		_showFindReplaceResult: function(result) {
			if(result) {
				if(typeof(result) === "string") {
					this._showMessageBox(localization[result]);
				}
				else if(result.replaceCount) {
					this._showMessageBox(String.format(localization[result.message], result.replaceCount));
				}
			}
		},

		setupChildren: function ()
		{
			//dialog main controls
			this._tab = $find("dialogtabstrip");
			this._tab.add_tabSelected(Function.createDelegate(this, this._tabChangedHandler));

			this._closeButton = $get("CloseButton");
			this._closeButton.title = localization["Close"];

			//dialog buttons
			this._findButton = $get("FindButton");
			this._findButton.setAttribute("unselectable", "on");
			this._findButton.title = localization["FindNext"];
			this._rFindButton = $get("rFindButton");
			this._rFindButton.setAttribute("unselectable", "on");
			this._rFindButton.title = localization["FindNext"];
			this._replaceButton = $get("ReplaceButton");
			this._replaceButton.setAttribute("unselectable", "on");
			this._replaceButton.title = localization["Replace"];
			this._replaceAllButton = $get("ReplaceAllButton");
			this._replaceAllButton.setAttribute("unselectable", "on");
			this._replaceAllButton.title = localization["ReplaceAll"];

			//dialog form controls
			this._upReplace = $get("upReplace");
			this._upFind = $get("upFind");
			this._downReplace = $get("downReplace");
			this._downFind = $get("downFind");
			this._find = $get("find");
			this._rFind = $get("rFind");
			this._rReplace = $get("rReplace");
			this._wholeWordFind = $get("wholeWordFind");
			this._wholeWordReplace = $get("wholeWordReplace");
			this._matchCaseReplace = $get("matchCaseReplace");
			this._matchCaseFind = $get("matchCaseFind");

			//preselect "down" radio button
			this._downReplace.checked = true;
			this._downFind.checked = true;

			if (!$telerik.isIE)
			{
				//hide Whole word on non-IE browsers (not supported)
				$get("wwReplaceContainer").style.visibility = "hidden";
				$get("wwFindContainer").style.visibility = "hidden";
			}
			this._initializeChildEvents();
		},

		_initializeChildEvents: function ()
		{
			$addHandlers(this._findButton, { "click": this._findClickHandler }, this);
			$addHandlers(this._rFindButton, { "click": this._findClickHandler }, this);
			$addHandlers(this._replaceButton, { "click": this._replaceClickHandler }, this);
			$addHandlers(this._replaceAllButton, { "click": this._replaceAllClickHandler }, this);
			$addHandlers(this._closeButton, { "click": this._cancelClickHandler }, this);
		},

		_tabChangedHandler: function (sender, args)
		{
			if (sender.get_selectedIndex() == 0)
			{
				this._find.value = this._rFind.value;
				this._wholeWordFind.checked = this._wholeWordReplace.checked;
				this._matchCaseFind.checked = this._matchCaseReplace.checked;
				this._upFind.checked = this._upReplace.checked;
				this._downFind.checked = this._downReplace.checked;
			}
			else
			{
				this._rFind.value = this._find.value;
				this._matchCaseReplace.checked = this._matchCaseFind.checked;
				this._wholeWordReplace.checked = this._wholeWordFind.checked;
				this._upReplace.checked = this._upFind.checked;
				this._downReplace.checked = this._downFind.checked;
			}
		},

		_cancelClickHandler: function (e)
		{
			Telerik.Web.UI.Dialogs.CommonDialogScript.get_windowReference().close();
		},

		_findClickHandler: function ()
		{
			this.execFind();
			this._ensureDialogIsVisible();
		},

		_replaceClickHandler: function ()
		{
			this.execReplace();
			this._ensureDialogIsVisible();
		},

		_replaceAllClickHandler: function ()
		{
			this.execReplaceAll();
			this._ensureDialogIsVisible();
		},

		_ensureDialogIsVisible: function ()
		{
			var dialogWindow = this._getRadWindow();
			if (!$telerik.isScrolledIntoView(dialogWindow.get_popupElement()))
			{
				dialogWindow.center();
			}
		},

		_getRadWindow: function () //mandatory for the RadWindow dialogs functionality
		{
			if (window.radWindow)
			{
				return window.radWindow;
			}
			if (window.frameElement && window.frameElement.radWindow)
			{
				return window.frameElement.radWindow;
			}
			return null;
		},
		isHtmlMode: function() {
			return this._clientParameters.editor.get_mode() == Telerik.Web.UI.EditModes.Html;
		}
	}

	Telerik.Web.UI.Widgets.FindAndReplace.registerClass("Telerik.Web.UI.Widgets.FindAndReplace", Telerik.Web.UI.RadWebControl, Telerik.Web.IParameterConsumer);
	// -->
</script>
			<telerik:RadTabStrip ShowBaseLine="true" ID="dialogtabstrip" runat="server" MultiPageID="dialogMultiPage"
				SelectedIndex="0">
				<Tabs>
					<telerik:RadTab Text="Find" Value="Find">
					</telerik:RadTab>
					<telerik:RadTab Text="Replace" Value="Replace">
					</telerik:RadTab>
				</Tabs>
			</telerik:RadTabStrip>

<div class="redWrapper reDialog">
	<telerik:RadMultiPage ID="dialogMultiPage" runat="server" SelectedIndex="0">
		<telerik:RadPageView ID="FindPage" runat="server">

			<div class="redSection redSectionTop">
				<div class="redRow redAlign">
					<label for="find" class="redLabel">
						<script type="text/javascript">document.write(localization["Find"]);</script>
					</label>
					<input type="text" id="find" />
				</div>
				<div class="redRow redAlign">
					<button type="button" id="FindButton">Find</button>
					<script type="text/javascript">setInnerHtml("FindButton", localization["Find"]);</script>
				</div>
			</div>

			<div class="redSection">
				<h6>
					<script type="text/javascript">document.write(localization["SearchOptions"]);</script>
				</h6>

				<div class="redRow redRowNoPadding">
					<span class="redSpanLabel">
						<script type="text/javascript">
							document.write(localization["SearchDirection"]);
						</script>
					</span>
					<div class="redMoreShortRow redInlineBlock">
						<input type="radio" name="searchDirection" id="upFind" />
						<label for="upFind">
							<script type="text/javascript">document.write(localization["Up"]);</script>
						</label>
					</div>
					<div class="redMoreShortRow redInlineBlock">
						<input type="radio" name="searchDirection" checked="checked" id="downFind" />
						<label for="downFind">
							<script type="text/javascript">document.write(localization["Down"]);</script>
						</label>
					</div>
				</div>
			
				<div class="redRow">
					<span class="redSpanLabel">&nbsp;</span>
					<div class="redInlineBlock">
						<input type="checkbox" id="matchCaseFind" />
						<label for="matchCaseFind">
							<script type="text/javascript">document.write(localization["MatchCase"]);</script>
						</label>
					</div>
				</div>
			
				<div class="redRow">
					<span class="redSpanLabel">&nbsp;</span>
					<div id="wwFindContainer" class="redInlineBlock">
						<input type="checkbox" id="wholeWordFind" />
						<label for="wholeWordFind">
							<script type="text/javascript">document.write(localization["MatchWholeWords"]);</script>
						</label>
					</div>
				</div>
			</div>

		</telerik:RadPageView>

		<telerik:RadPageView ID="ReplacePage" runat="server">
			
			<div class="redSection redSectionTop">
				<div class="redRow redAlign">
					<label for="rFind" class="redLabel">
						<script type="text/javascript">document.write(localization["Find"]);</script>
					</label>
					<input type="text" id="rFind" />
				</div>

				<div class="redRow redAlign">
					<label for="rReplace" class="redLabel">
						<script type="text/javascript">document.write(localization["ReplaceWith"]);</script>
					</label>
					<input type="text" id="rReplace" />
				</div>

				<div class="redRow redAlign">
					<button type="button" id="ReplaceButton" >Replace</button>
					<script type="text/javascript">setInnerHtml("ReplaceButton", localization["Replace"]);</script>
					<button type="button" id="ReplaceAllButton" >Replace All</button>
					<script type="text/javascript">setInnerHtml("ReplaceAllButton", localization["ReplaceAll"]);</script>
					<button type="button" id="rFindButton" >Find Next</button>
					<script type="text/javascript">setInnerHtml("rFindButton", localization["FindNext"]);</script>
				</div>
			</div>

			<div class="redSection">
					<h6>
						<script type="text/javascript">document.write(localization["SearchOptions"]);</script>
					</h6>

					<div class="redRow redRowNoPadding">
						<span class="redSpanLabel">
							<script type="text/javascript">document.write(localization["SearchDirection"]);</script>
						</span>
						<div class="redMoreShortRow redInlineBlock">
							<input type="radio" name="replaceDirection" id="upReplace" />
							<label for="upReplace">
								<script type="text/javascript">document.write(localization["Up"]);</script>
							</label>
						</div>
						<div class="redMoreShortRow redInlineBlock">
							<input type="radio" name="replaceDirection" checked="checked" id="downReplace" />
							<label for="downReplace">
								<script type="text/javascript">document.write(localization["Down"]);</script>
							</label>
						</div>
					</div>

					<div class="redRow">
						<span class="redSpanLabel">&nbsp;</span>
						<div class="redInlineBlock">
							<input type="checkbox" id="matchCaseReplace" />
							<label for="matchCaseReplace">
								<script type="text/javascript">document.write(localization["MatchCase"]);</script>
							</label>
						</div>
					</div>

					<div class="redRow" id="wwReplaceContainer">
						<span class="redSpanLabel">&nbsp;</span>
						<div class="redInlineBlock">
							<input type="checkbox" id="wholeWordReplace" />
							<label for="wholeWordReplace">
								<script type="text/javascript">document.write(localization["MatchWholeWords"]);</script>
							</label>
						</div>
					</div>

			</div>
	
		</telerik:RadPageView>

	</telerik:RadMultiPage>

	<div class="redActionButtonsWrapper redActionButtonsAbsoluteWrapper">
		<button type="button" id="CloseButton">Close</button>
		<script type="text/javascript">setInnerHtml("CloseButton", localization["Close"]);</script>
	</div>

</div>
