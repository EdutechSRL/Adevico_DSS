<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_EditSignature.ascx.vb" Inherits="Comunita_OnLine.UC_EditSignature" %>

<%--<%@ Register Src="~/UC/Editor/UC_VisualEditor.ascx" TagName="CTRLvisualEditor" TagPrefix="CTRL" %>--%>
<%@ Register Src="~/Modules/Common/Editor/UC_Editor.ascx" TagName="CTRLeditor" TagPrefix="CTRL" %>

<%@ Register TagPrefix="CTRL" TagName="Measures" Src="~/Modules/DocTemplate/Uc/UC_Measure.ascx"%>
<%@ Register TagPrefix="CTRL" TagName="Image" Src="~/Modules/DocTemplate/Uc/UC_EditImage.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="HFPlacing" Src="~/Modules/DocTemplate/Uc/UC_EditPagePlacing.ascx" %>

<script language="javascript" type="text/javascript">
	$(document).ready(function () {

		$("#<%=BTNpgtagCurPage.ClientId %>").click(function () {

			var add = $(this).attr("rel");
			var editor = $find("<%=EditorClientId%>");
			editor.pasteHtml(add);
			return false;
		});

		$("#<%=BTNpgtagCreateDate.ClientId %>").click(function () {

			var add = $(this).attr("rel");
			var editor = $find("<%=EditorClientId%>");
			editor.pasteHtml(add);
			return false;
		});

		$("#<%=BTNpgtagCreateTime.ClientId %>").click(function () {

			var add = $(this).attr("rel");
			var editor = $find("<%=EditorClientId%>");
			editor.pasteHtml(add);
			return false;
		});

	});
</script>

<asp:HiddenField ID="HYDsignId" runat="server" />

<div class="fieldcontent">
			
	<span class="switchcfield handle">+</span>
	
	<div class="internal clearfix">
		<span class="left">
			<span class="code title">
				<asp:Literal ID="LITcode_t" runat="server">Cod.</asp:Literal>
				<span>
					<asp:Literal ID="LITcode" runat="server">##</asp:Literal>    
				</span>
			</span>
		</span>
		<span class="right">
			<span class="icons">
				<asp:LinkButton ID="LKBdeletesignature" runat="server" CssClass="icon delete">X</asp:LinkButton>
			</span>
		</span>
	</div>

	<div class="fielddetails">
		<div class="fieldobject">
			<div class="fieldrow">
				<asp:Label ID="LBLalignment_t" runat="server" CssClass="fieldlabel">*Alignment</asp:Label>
				<asp:RadioButtonList ID="RBLalignment" runat="server" RepeatDirection="Vertical" RepeatLayout="Flow" CssClass="fieldblockwrapper">
					<asp:ListItem Value="-1" Text="*Left"></asp:ListItem>
					<asp:ListItem Value="0" Text="*Center"></asp:ListItem>
					<asp:ListItem Value="1" Text="*Right"></asp:ListItem>
				</asp:RadioButtonList>
			</div>

			<div class="fieldrow">
				<asp:CheckBox ID="CBXuseText" runat="server" Text="*Use Text" autopostback="true" CssClass="fieldblockwrapper"/>
			</div>

			<div class="fieldrow bodytext">
				<asp:Label ID="LBLsignText_t" runat="server" CssClass="fieldlabel">*Body Text</asp:Label>
					<asp:Button ID="BTNpgtagCurPage" runat="server" CausesValidation="false" Text="test"/>
					<asp:Button ID="BTNpgtagCreateDate" runat="server" CausesValidation="false" Text="test"/>
					<asp:Button ID="BTNpgtagCreateTime" runat="server" CausesValidation="false" Text="test"/>
<%--                <CTRL:CTRLvisualEditor ID="CTRLvisualEditorNote" runat="server" FontNames="Verdana"
					FontSizes="2,3,4" ToolsFile="~/RadControls/Editor/Localization/it-IT/ToolsAdvancedEvents.xml"
					ShowScrollingSpeed="false" ShowAddDocument="false" ShowAddImage="false" ShowAddSmartTag="true"
					EditorEnabled="true" AllowPreview="false" EditorHeight="180px" DisabledTags="youtube,slideshare"
					EditorMaxChar="4000" />
					
					EditorWidth="550px" 
					--%>
					<CTRL:CTRLeditor id="CTRLvisualEditorText" runat="server" ContainerCssClass="containerclass" RenderAsDiv="true" 
							LoaderCssClass="loadercssclass" EditorHeight="300px" AllAvailableFontnames="false" 
							AutoInitialize="true" ModuleCode="SRVDOCT" MaxHtmlLength="4000">
					</CTRL:CTRLeditor>
			</div>

			<div class="fieldrow">
				<asp:Label ID="LBLimage_t" runat="server" CssClass="fieldlabel">*Image</asp:Label>
				<span class="fieldblockwrapper contentcontextual image">
					<CTRL:Image Id="UCimage" runat="server" ShowMeasure="True"></CTRL:Image>
				</span>
			</div>
			<div class="fieldrow">
				<asp:CheckBox ID="CBXpdfPositioning" runat="server" Text="Use PDF Positioning" AutoPostBack="true" CssClass="fieldblockwrapper"/>
			</div>
			<div class="fieldrow">
				<span class="fieldblockwrapper">
					<CTRL:Measures ID="UCimgPosX" runat="server" CssClassTextBox="inputtext" Label="*Left" />
					<CTRL:Measures ID="UCimgPosY" runat="server" CssClassTextBox="inputtext" Label="*Bottom" />
				</span>
			</div>
			<div class="fieldrow">
				<asp:Label ID="LBLhfPositionSign_t" CssClass="fieldlabel" runat="server">#Mostra elemento in:</asp:Label>
				<div class="fieldblockwrapper">
					<CTRL:HFPlacing id="UCpagePlacing" runat="server"></CTRL:HFPlacing>
				</div>
			</div>
		</div>
	</div>
</div>