<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ExternalService.Master" CodeBehind="AddExternal.aspx.vb" Inherits="Comunita_OnLine.AddExternal" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ MasterType VirtualPath="~/ExternalService.Master" %>
<%@ Register TagPrefix="CTRL" TagName="Categories" Src="~/Modules/Ticket/UC/UC_CategoryDDL.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="MailSets" Src="~/Modules/Ticket/UC/UC_MailSettings.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="TopBar" Src="~/Modules/Ticket/UC/UC_ExternalTopBar.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Editor" Src="~/Modules/Common/Editor/UC_Editor.ascx" %>


<%@ Register TagPrefix="CTRL" TagName="AttachmentsHeader" Src="~/Modules/Repository/Common/UC_ModuleAttachmentJqueryHeaderCommands.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="AttachmentsCommands" Src="~/Modules/Repository/Common/UC_ModuleAttachmentInlineCommands.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Attachments" Src="~/Modules/Ticket/UC/UC_TicketAddAttachment.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="AttachmentsView" Src="~/Modules/Ticket/UC/UC_AttachmentsView.ascx" %>


<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="MailSettings" Src="~/Modules/Ticket/UC/UC_MailSettingsNew.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
	<asp:Literal ID="LTpageTitle_t" runat="server">*Crea Ticket</asp:Literal>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
	<asp:Literal ID="LTtitle_t" runat="server">*Crea Ticket</asp:Literal>
</asp:Content>

<asp:Content ContentPlaceHolderID="TopBarContent" ID="CNTtopBar" runat="server">
	<CTRL:TopBar ID="CTRLtopBar" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PreHeaderContent" runat="server">
	
	<link rel="Stylesheet" href="../../Graphics/Generics/css/4_UI_Elements.css<%=CssVersion()%>" />
	<link rel="Stylesheet" href="../../Graphics/Modules/Ticket/Css/tickets.css<%=CssVersion()%>" />

	<!-- JS service -->
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.simpleEqualize.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.blockableFieldset.js"></script> 
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.inputActivator.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.collapsableTreeAdv.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.ddbuttonlist.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>
	<link rel="stylesheet" href="../../Jscript/Modules/Common/Choosen/chosen.css<%=CssVersion()%>">
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.treeTable.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Ticket/tickets.js"></script>

	<CTRL:AttachmentsHeader ID="CTRLattachmentsHeader" runat="server" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
	<!-- Top menu -->
	<div class="DivEpButton top">
		<asp:LinkButton ID="LNBdelete" runat="server" CssClass="linkMenu"></asp:LinkButton>
		<asp:HyperLink ID="HYPbackTop" runat="server" CssClass="linkMenu"></asp:HyperLink>
	</div>
	<!-- End Top menu -->
	<asp:literal ID="LTmessageheaderCss" runat="server" Visible="false">fieldobject _fieldgroup hide</asp:literal>
	<div id="DVmessages" class="fieldobject fieldgroup hide" runat="server">
		<div class="fieldrow">
			<CTRL:Messages ID="CTRLmessagesInfo" runat="server" Visible="false" />
		</div>
	</div>
	<asp:Panel ID="PNLcontent" runat="server" CssClass="ticketview create clearfix" Visible="true">
	<div class="fieldobject fieldgroup">
		<div class="fieldrow">
			<asp:Label ID="LBsender_t" runat="server" CssClass="title">*Mittente</asp:Label>
		</div>
		<div class="fieldrow">
			<asp:Label ID="LBlanguage_t" runat="server" CssClass="fieldlabel">*Lingua</asp:Label>
			<asp:DropDownList ID="DDLlanguage" runat="server">
			</asp:DropDownList>
		</div>
		<div class="fieldrow">
			<asp:Label ID="LBname_t" runat="server" CssClass="fieldlabel" AssociatedControlID="TXBname">*Nome</asp:Label>
			<asp:TextBox ID="TXBname" runat="server" CssClass="fieldinput"></asp:TextBox>
		</div>
		<div class="fieldrow">
			<asp:Label ID="LBsname_t" runat="server" CssClass="fieldlabel" AssociatedControlID="TXBsname">*Cognome</asp:Label>
			<asp:TextBox ID="TXBsname" runat="server" CssClass="fieldinput"></asp:TextBox>
		</div>
		<div class="fieldrow">
			<asp:Label ID="LBmail_t" runat="server" CssClass="fieldlabel" AssociatedControlID="TXBmail">*Mail</asp:Label>
			<asp:TextBox ID="TXBmail" runat="server" CssClass="fieldinput"></asp:TextBox>
		</div>
		
		<div class="fieldrow">
			<asp:Label ID="LBLmailSettings" runat="server" CssClass="fieldlabel" AssociatedControlID="CTRLmailSetOwner">*Mail notifications</asp:Label>
			
			<div class="fieldinput options email">
				<span class="inlinewrapper">
					<CTRL:MailSettings ID="CTRLmailSetOwner" runat="server" />
				</span>
			</div>
		</div>

	</div>


	<div class="fieldobject fieldgroup">
		<div class="fieldrow">
			<asp:Label ID="LBcontent_t" runat="server" CssClass="title">*Contenuto</asp:Label>
		</div>
		<div class="fieldrow">
			<asp:Label ID="LBtitle_t" runat="server" CssClass="fieldlabel" AssociatedControlID="TXBtitle">*Titolo</asp:Label>
			<asp:TextBox ID="TXBtitle" runat="server" CssClass="fieldinput"></asp:TextBox>
			<asp:Label ID="LBtitleRequired" runat="server" CssClass="error" Visible="false">*</asp:Label>
		</div>
<%--		<div class="fieldrow">
			<asp:Label ID="LBLcommunity_t" runat="server" CssClass="fieldlabel" AssociatedControlID="CTRLddlCat">*Comunità</asp:Label>
			<asp:Label ID="LBLcommunity" runat="server" CssClass="fieldinput">#nome comunità</asp:Label>
			<asp:HyperLink ID="HYPopenCommunities" runat="server" CssClass="linkMenu showcommunities">*Cambia comunità</asp:HyperLink>
		</div>--%>
		<div class="fieldrow">
			<asp:Label ID="LBcategory_t" runat="server" CssClass="fieldlabel" AssociatedControlID="CTRLddlCat">*Categoria</asp:Label>
			<CTRL:Categories id="CTRLddlCat" runat="server" RequiredValidator="true"></CTRL:Categories>
			<asp:Label ID="LBcatRequired" runat="server" CssClass="error" Visible="false">*</asp:Label>
		</div>
		<div class="fieldrow">
			<asp:Label ID="LBtext_t" runat="server" CssClass="fieldlabel" AssociatedControlID="CTRLEditorText">*Testo</asp:Label>

			<CTRL:Editor ID="CTRLEditorText" runat="server" AllowAnonymous="true"
				ContainerCssClass="containerclass" LoaderCssClass="loadercssclass fieldinput inputtext" 
				EditorCssClass="editorcssclass" AllAvailableFontnames="false"
				AutoInitialize="true"   MaxHtmlLength="800000" />

			<asp:Label ID="LBtextRequired" runat="server" CssClass="error" Visible="false">*</asp:Label>
		</div>
	</div>
	<div class="fieldobject fieldgroup attachments" id="DIVattachments" runat="server">

		<div class="fieldrow">
			<asp:Label ID="LBattachment_t" runat="server" CssClass="title">*Allegati</asp:Label>
		</div>
		<div class="fieldrow">
			<div class="fieldobject clearfix">
				<div id="DIVatchCom" runat="server" class="fieldrow attachments empty">
					<CTRL:AttachmentsView ID="CTRLattView" runat="server" /> 
				</div>
				<div class="fieldrow right">
					<CTRL:AttachmentsCommands ID="CTRLcommands" runat="server" Visible="true" />
				</div>
			</div>

		</div>
	</div>

		

	<div class="fieldobject fieldgroup" style="display: none">
		<div class="fieldrow">
			<asp:Label ID="LBnotificheMail_t" runat="server" CssClass="title">*Notifiche mail</asp:Label>
			<CTRL:MailSets ID="CTRLmailSets" runat="server"></CTRL:MailSets>
		</div>
	</div>

	<div class="fieldobject">
		<div class="fieldrow left">

		</div>
		<div class="fieldrow right">
			<asp:LinkButton ID="LNBsaveDraft" runat="server" CssClass="linkMenu">*Salva bozza</asp:LinkButton>
			<asp:LinkButton ID="LNBsend" runat="server" CssClass="linkMenu">*Invia</asp:LinkButton>
		</div>
	</div>

	<%--da rivedere--%>
	<asp:Label ID="LBError" runat="server" CssClass="error"></asp:Label>

	</asp:Panel>
	<asp:Panel ID="PNLerror" runat="server">
		<div class="error">
			<asp:Label ID="LBmainError" CssClass="errormessage" runat="server"></asp:Label>
		</div>
	</asp:Panel>
	<CTRL:Attachments ID="CTRLinternalUpload" runat="server" visible="false" UploaderCssClass="dlguploadtomoduleitem" />
	<asp:UpdatePanel ID="UPTempo" runat="server">
		<Triggers>
			<asp:AsyncPostBackTrigger ControlID="TMsession" EventName="Tick" />
		</Triggers>
	</asp:UpdatePanel>
	<asp:Timer ID="TMsession" runat="server">
	</asp:Timer>
</asp:Content>