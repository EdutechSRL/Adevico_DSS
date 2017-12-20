<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="CategoryAdd.aspx.vb" Inherits="Comunita_OnLine.CategoryAdd" %>
<%--Nomi Standard: OK--%>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<%@ Register TagPrefix="CTRL" TagName="SelectUsers" Src="~/Modules/Common/UC/UC_SelectUsers.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="SelectUsersHeader" Src="~/Modules/Common/UC/UC_SelectUsersHeader.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
	<asp:Literal ID="LTpageTitle_t" runat="server">*Amministrazione globale TicketSystem</asp:Literal>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript">
		$(document).ready(function () {
			$(".view-modal.view-user").dialog({
				appendTo: "form",
				closeOnEscape: true,
				modal: true,
				width: 890,
				height: 450,
				minHeight: 300,
				minWidth: 700,
				title: '<%=Me.UsersDialogTitle%>',
				open: function (type, data) {
					$(this).parent().children().children('.ui-dialog-titlebar-close').hide();
				}
			});
		});
	</script>
	<CTRL:SelectUsersHeader ID="CTRLselectUsersHeader" runat="server" />
	<link rel="Stylesheet" href="../../Graphics/Modules/Ticket/Css/tickets.css<%=CssVersion()%>" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
	<asp:Literal ID="LTtitle_t" runat="server">*Amministrazione globale TicketSystem</asp:Literal>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
<asp:Panel ID="PNLcontent" runat="server">
	<div class="fieldobject toolbar clearfix">
		<div class="fieldrow left">

		</div>
		<div class="fieldrow right">
			<asp:LinkButton ID="LNBadd" runat="server" CssClass="linkMenu">*Add</asp:LinkButton>
			<asp:LinkButton ID="LNBtoList" runat="server" CssClass="linkMenu">*ToList</asp:LinkButton>
		</div>
	</div>



	<div class="fieldobject">
		<div class="fieldrow createname">
			<asp:Label ID="LBnameC_t" runat="server" CssClass="fieldlabel" AssociatedControlID="TXBnameC">*Name</asp:Label>
			<span class="namewrapper">
				<asp:TextBox ID="TXBnameC" runat="server" CssClass="fieldinput"></asp:TextBox>
				<asp:Literal ID="LTnameError" runat="server" Visible="false"><span class="error">*</span></asp:Literal>
			</span>
		</div>
		<div class="fieldrow createdesc">
			<asp:Label ID="LBdescC_t" runat="server" CssClass="fieldlabel" AssociatedControlID="TXBdescC">*Description</asp:Label>
			<span class="descwrapper">
				<asp:TextBox ID="TXBdescC" runat="server" CssClass="fieldinput textarea big" TextMode="MultiLine"></asp:TextBox>
			</span>
		</div>
	</div>

	<div class="fieldobject">
		<div class="fieldrow type">
			<asp:Label ID="LBtype_t" runat="server" CssClass="fieldlabel" AssociatedControlID="DDLtype">*Tipo</asp:Label>
			<span class="typewrapper">
				<asp:DropDownList ID="DDLtype" runat="server">
					<asp:ListItem Text="*Pubblica" Value="1"></asp:ListItem>
					<asp:ListItem Text="*Privata" Value="2"></asp:ListItem>
					<asp:ListItem Text="*Solo Ticket" Value="3"></asp:ListItem>
				</asp:DropDownList>
			</span>
		</div>

	</div>

	<div class="fieldobject">
		<div class="fieldrow createmanager">
			<asp:Label ID="LBcurUser_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBmanager">*Scegli manager</asp:Label>
			<span class="usrtypewrapper">
				<asp:Label ID="LBmanager" runat="server">###</asp:Label>
				<asp:LinkButton ID="LNBchangeUser" runat="server" CssClass="linkMenu">*Cambia</asp:LinkButton>
				<%--<asp:RadioButtonList ID="RBLcurUser" runat="server"
					AutoPostBack="true" RepeatLayout="Flow" RepeatDirection="Horizontal"
					CssClass="rblcuruser">
					<asp:ListItem Selected="True" Value="0" class="rblitem">*Impostami come Manager</asp:ListItem>
					<asp:ListItem Value="1" class="rblitem">*Scegli manager</asp:ListItem>
				</asp:RadioButtonList>--%>
				<asp:LinkButton ID="LNBuserSetCurrent" runat="server" CssClass="linkMenu">*Seleziona corrente</asp:LinkButton>
			</span>
			<asp:PlaceHolder ID="PLH_SelectUser" runat="server" Visible="false">
				
			</asp:PlaceHolder>
		</div>
	</div>

	<div class="view-modal view-user" id="DVselectUsr" runat="server" visible="false">
		<CTRL:SelectUsers ID="CTRLselectUsers" runat="server"
			RaiseCommandEvents="true" DisplayDescription="true"
			DefaultPageSize="20" ShowSubscriptionsProfileTypeColumn="true"
			DefaultMaxPreviewItems="5" InModalWindow="true" MultipleSelection="false"
			ShowItemsExceeding="true" ShowSubscriptionsFilterByProfile="false" />
	   <%-- 
		<div class="fieldobject commands">
			<div class="fieldrow buttons right">
				
				<asp:LinkButton ID="LNBcloseModal" runat="server">*Chiudi</asp:LinkButton>
			</div>
		</div>--%>


	</div>
</asp:Panel>

<%--<asp:Panel ID="PNLnoPermission" runat="server" Visible="false">
	<asp:label ID="LBnoPermission" runat="server">No category type assigned to current community...</asp:label>
</asp:Panel>--%>
</asp:Content>
