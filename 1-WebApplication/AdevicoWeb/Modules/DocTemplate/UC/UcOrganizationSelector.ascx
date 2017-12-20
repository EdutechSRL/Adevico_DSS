<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UcOrganizationSelector.ascx.vb" Inherits="Comunita_OnLine.UcOrganizationSelector" %>
 <asp:Repeater ID="Rpt_OrgnMod" runat="server" EnableViewState="true">
    <HeaderTemplate>
        <ul class="UL_Orgn">
    </HeaderTemplate>
    <ItemTemplate>
            <li>
                <asp:LinkButton ID="LKBselOrgn" runat="server" CommandName="Select">#Orgn Name</asp:LinkButton>
            </li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>