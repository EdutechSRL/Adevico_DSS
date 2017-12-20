<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ExternalService.Master" CodeBehind="UserSettingsExternal.aspx.vb" Inherits="Comunita_OnLine.UserSettingsExternal" %>
<%--Nomi Standard: OK--%>
<%@ MasterType VirtualPath="~/ExternalService.Master" %>

<%@ Register TagName="CTRLpwdChange" TagPrefix="CTRL" Src="~/Modules/Ticket/UC/UC_PasswordChange.ascx" %>
<%--<%@ Register TagName="CTRLMailSets" TagPrefix="CTRL" Src="~/Modules/Ticket/UC/UC_MailSettings.ascx" %>--%>
<%@ Register tagPrefix="CTRL" tagName="TkSettings" src="~/Modules/Ticket/UC/UC_TicketUserSettings.ascx" %>


<%@ Register TagPrefix="CTRL" TagName="TopBar" Src="~/Modules/Ticket/UC/UC_ExternalTopBar.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
	<asp:Literal ID="LTpageTitle_t" runat="server">*Ticket esterni</asp:Literal>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PreHeaderContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TopBarContent" runat="server">
	<CTRL:TopBar ID="CTRLtopBar" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="TitleContent" runat="server">
	<asp:Literal ID="LTcontentTitle_t" runat="server">*Impostazioni utente</asp:Literal>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="CPHservice" runat="server">
	
<div class="tickets mgmgt globalsettings">

	<div class="pageheader">
	</div>    
		
	<CTRL:TkSettings runat="server" ID="UCtkSet" />	
	
	<div class="fieldobject changePassword">
		<div class="fieldrow objectheader">
			<h4 class="title">
				<asp:literal ID="LTpasswordTitle" runat="server">*Change Password</asp:literal>
			</h4>
		</div>
		<div class="fieldrow optiongroup user notifications clearfix">
			<CTRL:CTRLpwdChange ID="CTRLpwd" runat="server" ShowMessages="True" />
		</div>
	</div>
<%--			<div class="fieldrow optiongroup user notifications clearfix">
				 <div class="description">
					<asp:literal ID="LTusrDeacription_t" runat="server">*Receive notifications for:</asp:literal>
				</div>
				<div class="options">
					<asp:Label ID="LBnotificheMail_t" runat="server" CssClass="fieldlabel" AssociatedControlID="CTRLmailSets">*Notifiche mail</asp:Label>
					<span class="inlinewrapper">
						<CTRL:CTRLMailSets ID="CTRLmailSets" runat="server"></CTRL:CTRLMailSets>
					</span>
				</div>
				<div class="commands">
					<asp:LinkButton ID="LNBsetMail" runat="server" CssClass="Link_Menu">*Modifica</asp:LinkButton>
					<%--<a class="linkMenu ovverride" href="#">Reset all notifications to system default</a>--% >
				</div>

			</div>--%>

	
</div>
</asp:Content>
