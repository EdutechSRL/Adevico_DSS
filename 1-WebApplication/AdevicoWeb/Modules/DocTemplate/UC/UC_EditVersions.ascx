<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_EditVersions.ascx.vb" Inherits="Comunita_OnLine.UC_EditVersions" %>

<asp:Repeater ID="RPTsubVersion" runat="server">
    <HeaderTemplate>
        <table class="revisions minimal">
            <thead>
                <tr>
                    <th class="cbx"><asp:CheckBox ID="CBXall" CssClass="cbxall" runat="server" onclick="checkAll(this);"/></th>
                    <th class="code">
                        <asp:Literal ID="LITcode_t" runat="server">#Code</asp:Literal>
                    </th>
                    <th class="revision">
                        <asp:Literal ID="LITrevision_t" runat="server">#Revision</asp:Literal>
                    </th>
                    <th class="actions">
                        <asp:Literal ID="LITaction_t" runat="server">#Actions</asp:Literal>
                    </th>
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
                <tr class="first">
                    <td class="cbx">
                        <asp:CheckBox ID="CBXselect" runat="server" CssClass="cbxsel" />
                    </td>
                    <td class="code">
                        <asp:label id="LBLcode" runat="server"></asp:label>
                    </td>
                    <td class="revision">
                        <asp:label id="LBLdateTime" runat="server"></asp:label>
                    </td>
                    <td class="actions">
                        <span class="icons">
                            <asp:LinkButton ID="LKBrecover" runat="server" CssClass="icon edit">[r]</asp:LinkButton>
                            <asp:LinkButton ID="LKBdelete" runat="server" CssClass="icon delete">[d]</asp:LinkButton>
                    	</span>
                    </td>
                </tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody>
        </table>
    </FooterTemplate>
</asp:Repeater>
<div class="buttonwrapper">
    <asp:linkbutton ID="LKBdeleteSel" runat="server" CssClass="linkMenu">#Delete Selected</asp:linkbutton>
</div>