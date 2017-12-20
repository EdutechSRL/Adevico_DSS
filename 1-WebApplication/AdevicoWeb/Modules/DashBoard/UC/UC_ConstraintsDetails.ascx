<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ConstraintsDetails.ascx.vb" Inherits="Comunita_OnLine.UC_ConstraintsDetails" %>
<div class="<%=ContainerCssClass %>">
    <div class="messages">
        <div class="message <%=MessageCssClass%>">
            <span class="icons"><span class="icon">&nbsp;</span></span><asp:Literal ID="LTmessage" runat="server"></asp:Literal>
        </div>
    </div>
    <div class="tablewrapper">
        <table class="table minimal fullwidth requirements <%=ContainerCssClass %>">
            <!--<thead>
            <tr>
                <th class="reason">...</th>
                <th class="info">...</th>
            </tr>
            </thead>-->
            <tbody>
            <asp:Repeater ID="RPTconstraints" runat="server">
                <ItemTemplate>
                    <tr>
                        <td class="reason"><asp:Literal ID="LTconstraintReason" runat="server"></asp:Literal></td>
                        <td class="info <%#GetCssClass(Container.DataItem)%>"><span class="<%#GetCssClass(Container.DataItem)%>"><asp:Literal ID="LTconstraintStatus" runat="server"></asp:Literal></span></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            </tbody>
        </table>
    </div>
</div>
<asp:Literal ID="LTcssClassPassed" runat="server" Visible="false">passed</asp:Literal>
<asp:Literal ID="LTcssClassWaiting" runat="server" Visible="false">waiting</asp:Literal>
<asp:Literal ID="LTcssClassNotPassed" runat="server" Visible="false">notpassed</asp:Literal>