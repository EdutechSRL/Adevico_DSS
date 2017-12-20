<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_AttachmentsView.ascx.vb" Inherits="Comunita_OnLine.UC_AttachmentsView" %>
<%@ Register TagPrefix="CTRL" TagName="DisplayItem" Src="~/Modules/Repository/Common/UC_ModuleRenderAction.ascx" %>
<asp:Panel ID="PNLfilesContainer" runat="server" CssClass="fieldrow attachments">
	<asp:Repeater ID="RPTattachments" runat="server">
		<ItemTemplate>
			<span class="renderedfile">
				<CTRL:DisplayItem ID="CTRLdisplayItem" runat="server" />
				<span class="icons visibility">
					<asp:LinkButton ID="LNBshowHide" runat="server" CssClass="icon"></asp:LinkButton>
					<asp:LinkButton ID="LNBdelete" runat="server" CssClass="icon delete"></asp:LinkButton>
				</span>
			</span>
		</ItemTemplate>
	</asp:Repeater>
</asp:Panel>