<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="Add.aspx.vb" Inherits="Comunita_OnLine.Add" %>
<%--Nomi Standard: OK--%>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>


<%@ Register TagPrefix="CTRL" TagName="Categories" Src="~/Modules/Ticket/UC/UC_CategoryDDL.ascx" %>
<%--<%@ Register TagPrefix="CTRL" TagName="MailSets" Src="~/Modules/Ticket/UC/UC_MailSettings.ascx" %>--%>

<%--<%@ Register TagPrefix="CTRL" TagName="ComSelectOld" Src="~/uc/UC_SearchCommunityByService.ascx" %>--%>
<%@ Register TagPrefix="CTRL" TagName="CommunitySelectorHeader" Src="~/Modules/Common/UC/UC_ModalCommunitySelectorHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CommunitySelector" Src="~/Modules/Common/UC/UC_ModalCommunitySelector.ascx" %>


<%@ Register TagPrefix="CTRL" TagName="Editor" Src="~/Modules/Common/Editor/UC_Editor.ascx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<%@ Register TagPrefix="CTRL" TagName="AttachmentsHeader" Src="~/Modules/Repository/Common/UC_ModuleAttachmentJqueryHeaderCommands.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="AttachmentsCommands" Src="~/Modules/Repository/Common/UC_ModuleAttachmentInlineCommands.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Attachments" Src="~/Modules/Ticket/UC/UC_TicketAddAttachment.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="AttachmentsView" Src="~/Modules/Ticket/UC/UC_AttachmentsView.ascx" %>


<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>

<%@ Register TagPrefix="CTRL" TagName="SelectPerson" Src="~/Modules/Common/UC/UC_SelectUsers.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="SelectPersonHeader" Src="~/Modules/Common/UC/UC_SelectUsersHeader.ascx" %>

<%@ Register TagPrefix="CTRL" TagName="MailSettings" Src="~/Modules/Ticket/UC/UC_MailSettingsNew.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
	<asp:Literal ID="LTpageTitle_t" runat="server">*Apertura Ticket</asp:Literal>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
	<asp:Literal ID="LTtitle_t" runat="server">*Apertura Ticket</asp:Literal>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
	
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
	
	


	<%--<CTRL:EditorStyle ID="UC_EditorStyle" runat="server" />--%>
	<link rel="Stylesheet" href="../../Graphics/Modules/Ticket/Css/tickets.css<%=CssVersion()%>" />

	<!-- JS service -->
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.simpleEqualize.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.blockableFieldset.js"></script> 
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.inputActivator.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.collapsableTreeAdv.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.ddlist.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.ddbuttonlist.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>
	<link rel="stylesheet" href="../../Jscript/Modules/Common/Choosen/chosen.css<%=CssVersion()%>">
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.treeTable.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Ticket/tickets.js"></script>
	
	<CTRL:CommunitySelectorHeader id="CTRLcommunitySelectorHeader" runat="server" LoadCss="false" LoadScripts="false" Width="940" Height="650" MinHeight="500" MinWidth="750" SelectionMode="Single"></CTRL:CommunitySelectorHeader>
	<CTRL:AttachmentsHeader ID="CTRLattachmentsHeader" runat="server" />
	
	<CTRL:SelectPersonHeader ID="CTRLselectUsersHeader" runat="server" />
	
	<!-- JS pagina -->
	<script type="text/javascript">

		$(function () {
			$(".dialog.addnewfield").dialog();

			$(".showcommunities").click(function () {
				$("<%=CommunityModalId%>").dialog("open");
				return false;
			});

			$(".view-modal.view-users").dialog({
				appendTo: "form",
				closeOnEscape: true,
				modal: true,
				width: 890,
				height: 450,
				minHeight: 300,
				minWidth: 700,
				autoOpen: false,
				title: '<%=Me.InternalUserAddTitle %>'
			});

		});

	</script>

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">

	<!-- Top menu -->
	<div class="DivEpButton top">
		<asp:LinkButton ID="LNBdelete" runat="server" CssClass="linkMenu"></asp:LinkButton>
		<asp:HyperLink ID="HYPbackTop" runat="server" CssClass="linkMenu"></asp:HyperLink>
		
	</div>
	<!-- End Top menu -->
	<asp:literal ID="LTmessageheaderCss" runat="server" Visible="false">fieldobject _fieldgroup hide</asp:literal>

	<asp:Panel ID="PNLcontent" runat="server" CssClass="ticketview create clearfix" Visible="true">

		<div id="DVmessages" class="fieldobject fieldgroup hide" runat="server">
			<div class="fieldrow">
				<CTRL:Messages ID="CTRLmessagesInfo" runat="server" />
			</div>
		</div>
		
		<!--<div class="fieldobject fieldgroup">
			<! -- bottoni?-- >
		</div>-->
		<div class="fieldobject fieldgroup" runat="server" ID="DVsubmitter">
			
			<asp:panel id="PNLbehalf" runat="server" CssClass="DivEpButton toolbar">
				<!-- Create or Change depends if is a on behalf ticket-->
				<asp:Label runat="server" ID="LBbehalfAction_t">*Behalf action: </asp:Label>
				<div class="ddbuttonlist enabled" runat="server" id="DVddlBehalfContainer"><!--
				   --><asp:linkbutton ID="LNBremoveBehalf" runat="server" CssClass="linkMenu">*Remove Behalf</asp:linkbutton><!--
					--><asp:linkbutton ID="LNBaddUsers" runat="server" CssClass="linkMenu" >*Add users</asp:linkbutton><!--
					--><%--<asp:linkbutton ID="LNBhideToOwner" runat="server" CssClass="linkMenu" >*Hide</asp:linkbutton><!--
					--><asp:linkbutton ID="LNBshowToOwner" runat="server" CssClass="linkMenu" >*Show</asp:linkbutton>--%><!--
					--><asp:linkbutton ID="LNBaddTicketUser" runat="server" CssClass="linkMenu" Visible="False">*Add Ticket users</asp:linkbutton><!--
					--><asp:linkbutton ID="LNBaddNewTicketUSer" runat="server" CssClass="linkMenu" Visible="false">*Add NEW ticket users</asp:linkbutton><!--
				--></div>
			</asp:panel>
			
			<div class="fieldrow">
				<asp:Label ID="LBsender_t" runat="server" CssClass="title">*Mittente</asp:Label>
			</div>
			
			<asp:panel ID="PNLbehalfCreator" runat="server" CssClass="fieldrow creator">
				<asp:Label runat="server" ID="LBcreator_t" CssClass="fieldlabel" AssociatedControlID="LTcreator">*Created by:</asp:Label>
				<span class="user"><asp:literal id="LTcreator" runat="server">###</asp:literal></span>
			</asp:panel>

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
			
			<asp:panel ID="PNLbehalfVisibility" runat="server" CssClass="fieldrow visibility">
				<asp:Label ID="LBmailVisibility_t" runat="server" CssClass="fieldlabel" AssociatedControlID="CBXhideToOwner">*Owner visibility</asp:Label>
				<asp:checkbox ID="CBXhideToOwner" runat="server" Checked="True" CssClass="inputgroup" Text="*Hidden" AutoPostBack="True" CausesValidation="False"/>
			</asp:panel>
			

			
		</div>
		
		<div class="fieldobject fieldgroup">
			<div class="fieldrow">
				<asp:Label ID="LBnotificheMail_t" runat="server" CssClass="title">*Notifiche mail</asp:Label>
				<%--<CTRL:MailSets ID="CTRLmailSets" runat="server"></CTRL:MailSets>--%>
			</div>
			<div class="fieldrow notifications clearfix">
				<%--<asp:Label ID="LBLmailSettings" runat="server" CssClass="fieldlabel" AssociatedControlID="CTRLmailSetCreator">*Mail notifications</asp:Label>--%>
				
				<div class="options"><%--fieldinput options email--%>
					<asp:Label runat="server" ID="LBMailSetCreator_t" AssociatedControlID="CTRLmailSetCreator" CssClass="optionlabel">*label</asp:Label>
					<span class="inlinewrapper">
						<CTRL:MailSettings ID="CTRLmailSetCreator" runat="server" />
						<asp:literal ID="LTcreatorInfo" runat="server" Visible="false"></asp:literal>
					</span>
				</div>
				
				<asp:panel ID="PNLownerNotification" runat="server" CssClass="options" Visible="False"><%--fieldinput options email--%>
					<asp:Label runat="server" ID="LBMailSetOwner_t" AssociatedControlID="CTRLmailSetOwner" CssClass="optionlabel">*label</asp:Label>
					<span class="inlinewrapper">
						<CTRL:MailSettings ID="CTRLmailSetOwner" runat="server" />
						<asp:literal ID="LTownerInfo" runat="server" Visible="false"></asp:literal>
					</span>
				</asp:panel>

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
			<div class="fieldrow">
				<asp:Label ID="LBcommunity_t" runat="server" CssClass="fieldlabel" AssociatedControlID="CTRLddlCat">*Comunità</asp:Label>
				<asp:Label ID="LBcommunity" runat="server" CssClass="fieldinput">#nome comunità</asp:Label>
				<div class="ddbuttonlist enabled"><!--
				   --><asp:HyperLink ID="HYPopenCommunities" runat="server" CssClass="linkMenu showcommunities">*Cambia comunità</asp:HyperLink><!--
				   --><asp:LinkButton id="LNBcommunitySetPortal" runat="server" CssClass="linkMenu">#Imposta portale</asp:LinkButton><!--
				   --><asp:LinkButton id="LNBcommunitySetCurrent" runat="server" CssClass="linkMenu">#Imposta corrente</asp:LinkButton><!--
				--></div>
			</div>
			<div class="fieldrow">
				<asp:Label ID="LBcategory_t" runat="server" CssClass="fieldlabel" AssociatedControlID="CTRLddlCat">*Categoria</asp:Label>
				<CTRL:Categories id="CTRLddlCat" runat="server" RequiredValidator="true"></CTRL:Categories>
				<asp:Label ID="LBnoCategory" runat="server" CssClass="error" Visible="false">*No category for selected Community</asp:Label>
				<asp:Label ID="LBcatRequired" runat="server" CssClass="error" Visible="false">*</asp:Label>

			</div>
			<div class="fieldrow">
				<asp:Label ID="LBtext_t" runat="server" CssClass="fieldlabel" AssociatedControlID="CTRLeditorText">*Testo</asp:Label>
			
				<CTRL:Editor ID="CTRLeditorText" runat="server"
					ContainerCssClass="containerclass" LoaderCssClass="loadercssclass fieldinput inputtext" 
					EditorCssClass="editorcssclass" AllAvailableFontnames="false"
					AutoInitialize="true"  MaxHtmlLength="800000" />
			
				<asp:Label ID="LBtextRequired" runat="server" CssClass="error" Visible="false">*</asp:Label>
			</div>
		</div>

		<div class="fieldobject fieldgroup attachments">
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

		<div class="DivEpButton bottom">
			<asp:LinkButton ID="LNBsaveDraft" runat="server" CssClass="linkMenu">*Salva bozza</asp:LinkButton>
			<asp:LinkButton ID="LNBsend" runat="server" CssClass="linkMenu">*Invia</asp:LinkButton>
		</div>

		<%--<asp:Label ID="LBError" runat="server" CssClass="error"></asp:Label>--%>
		<%--<div class="dialog dlgcommunities" title="<%=ComDialogTitle%>">--%>
		<%--OLD COMMUNITY
		<div class="view-modal view-comm dlgcommunities" id="DVselectCom" runat="server" visible="false">
			
			< %--<CTRL:ComSelectOld ID="CTRLcomSelector" runat="server" AdministrationMode="false" AllowAdd="false" />--% >
			< %--<asp:LinkButton ID="LNBconfCom" runat="server">Conferma</asp:LinkButton>--% >

			<div class="fieldobject commands" id="DVcommands" runat="server">
				<div class="fieldrow buttons right">
					<asp:LinkButton id="LNBcommunitySetPortal" runat="server" CssClass="linkMenu">#Imposta portale</asp:LinkButton>
					<asp:LinkButton id="LNBcloseCommunityWindow" runat="server" CssClass="linkMenu">#close</asp:LinkButton>
				</div>
			</div>
			
		</div>--%>
		
		<CTRL:CommunitySelector id="CTRLcommunity" runat="server" visible="false" SelectionMode="Single"></CTRL:CommunitySelector>

	</asp:Panel>

	<CTRL:Attachments ID="CTRLinternalUpload" runat="server" visible="false" UploaderCssClass="dlguploadtomoduleitem" />
	<CTRL:Attachments ID="CTRLcommunityUpload" runat="server" visible="false"  UploaderCssClass="dlguploadtomoduleitemandcommunity"/>
   <CTRL:Attachments ID="CTRLlinkFromCommunity" runat="server" visible="false"  UploaderCssClass="dlglinkfromcommunity"/>

	<asp:UpdatePanel ID="UPTempo" runat="server">
		<Triggers>
			<asp:AsyncPostBackTrigger ControlID="TMsession" EventName="Tick" />
		</Triggers>
	</asp:UpdatePanel>
	<asp:Timer ID="TMsession" runat="server">
	</asp:Timer>
	
	<!--START User DIALOG	-->
	<div class="view-modal view-users" runat="server" id="DIVusers">
		
		<CTRL:SelectPerson ID="CTRLselectUsers" runat="server"
		RaiseCommandEvents="True" DisplayDescription="true"
		DefaultPageSize="20" ShowSubscriptionsProfileTypeColumn="True"
		DefaultMaxPreviewItems="5"
		ShowItemsExceeding="true" ShowSubscriptionsFilterByProfile="True"
		SelectionMode="CommunityUsers" MultipleSelection="True"
		InModalWindow="true"
		/>  
	</div>
<!--END User DIALOG	-->
</asp:Content>