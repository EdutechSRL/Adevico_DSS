<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_SelectMessage.ascx.vb" Inherits="Comunita_OnLine.UC_SelectMessage" %>
<div class="tablewrapper">
    <table class="table light fullwidth msglistmessages">
        <thead>
            <tr>
                <th class="check">
                    <span class="headercheckbox">
                        <input id="CBheader" type="checkbox" />
                    </span>
                </th>
                <th class="name">
                    <asp:Literal ID="LTmessageSentName_t" runat="server">*Name</asp:Literal>
                </th>
                <th class="datecol">
                    <asp:Literal ID="LTmessageSentOn_t" runat="server">*Date</asp:Literal>
                </th>
                <th class="actions">
                    <asp:Literal ID="LTmessageSentAction_t" runat="server">*Actions</asp:Literal>
                </th>
            </tr>
        </thead>
        <tbody>
        <asp:Repeater id="RPTmessages" runat="server">
            <ItemTemplate>
            <tr class="message initialized <%#MessageCssClass(Container.DataItem) %>">
                <td class="check">
                     <input type="checkbox" id="CBXmessage" runat="server"/>
                </td>
                <td class="name">
                    <span class="message">
                        <asp:Literal ID="LTidMessage" runat="server" Text='<%#Container.DataItem.Id %>' Visible="false"></asp:Literal>
                        <asp:Label ID="LBmessageSentName" runat="server" CssClass="name"></asp:Label>
                        <asp:Label ID="LBmessageSeparator" runat="server" CssClass="sep" Visible="false"></asp:Label>
                        <asp:Label ID="LBmessageTemplateName" runat="server" CssClass="template" Visible="false"></asp:Label>
                    </span>
                </td>
                <td class="datecol">
                    <asp:Label ID="LBmessageSentOn" runat="server" CssClass="date"></asp:Label>
                </td>
                <td class="actions">
                    <span class="icons">
                        <asp:HyperLink ID="HYPviewMessage" runat="server" CssClass="icon view" Target="_blank"></asp:HyperLink>
                    </span>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            <tr id="TRempty" runat="server" visible="false">
                <td colspan="4">
                    <asp:Label ID="LBmessageSentEmptyItems" runat="server">*No message found.</asp:Label>
                </td>
            </tr>
        </FooterTemplate>
    </asp:Repeater>
        </tbody>
    </table>
</div>