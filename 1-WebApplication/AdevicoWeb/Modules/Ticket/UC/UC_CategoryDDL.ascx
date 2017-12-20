<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_CategoryDDL.ascx.vb" Inherits="Comunita_OnLine.UC_CategoryDDL" %>
<%--Nomi Standard: OK--%>
<%@ Register TagPrefix="CTRL" TagName="Item" Src="~/Modules/Ticket/UC/UC_CategoryDDLItem.ascx" %>


<%--<div class="dropdown enabled">--%>
<asp:Panel ID="PNLddl" runat="server" CssClass="dropdown enabled"><!--
	--><input type="hidden" id="HID_Value" runat="server"/><!--
--><span class="ddselector"><asp:literal ID="LTSelect_t" runat="server">*Seleziona</asp:literal></span><!--
--><span class="selector">
		<span class="selectoricon">&nbsp;</span>
		<span class="listwrapper">
			<span class="arrow"></span>
			<ul class="items">
				<asp:Repeater ID="RPTSubCategories" runat="server" EnableViewState="true">
					<ItemTemplate>
						<CTRL:Item id="CTRLitem" runat="server" EnableViewState="true"></CTRL:Item>
					</ItemTemplate>
				</asp:Repeater>
			</ul>
		</span>
	</span>
</asp:Panel>
<%--</div>--%>
<asp:Literal ID="LTreadOnly" runat="server" Visible="false"></asp:Literal>
<%--


--%>