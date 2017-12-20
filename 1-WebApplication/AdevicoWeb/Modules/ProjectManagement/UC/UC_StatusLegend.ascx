<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_StatusLegend.ascx.vb"
    Inherits="Comunita_OnLine.UC_StatusLegend" %>
    <%@ Import Namespace="lm.Comol.Modules.Standard.ProjectManagement.Domain" %>
<span class="fieldrow legend hor">
    <asp:Label ID="LBlegendLabel" runat="server" CssClass="fieldlabel">*Legend</asp:Label>
    <span class="group first">
        <span class="legenditem<%=IsAvailable(FieldStatus.completed) %>" title="<%=GetStatusToolTip(FieldStatus.completed) %>">
            <span class="legendicon green">&nbsp;</span>
            <asp:Label ID="LBlegendtextcompleted" runat="server" CssClass="legendtext">completed</asp:Label>
        </span>
        <span class="legenditem<%=IsAvailable(FieldStatus.started) %>" title="<%=GetStatusToolTip(FieldStatus.started) %>">
            <span class="legendicon yellow">&nbsp;</span>
            <asp:Label ID="LBlegendtextstarted" runat="server" CssClass="legendtext">started</asp:Label>
        </span>
        <span class="legenditem<%=IsAvailable(FieldStatus.notstarted) %>" title="<%=GetStatusToolTip(FieldStatus.notstarted) %>">
            <span class="legendicon gray">&nbsp;</span>
            <asp:Label ID="LBlegendtextnotstarted" runat="server" CssClass="legendtext">not started</asp:Label>
        </span>
    </span>
    <span class="group last">
        <span class="legenditem<%=IsAvailable(FieldStatus.recalc) %>" title="<%=GetStatusToolTip(FieldStatus.recalc) %>">
            <span class="legendicon recalc">&nbsp;</span>
            <asp:Label ID="LBlegendtextrecalc" runat="server" CssClass="legendtext">recalculated</asp:Label>
        </span>
        <span class="legenditem<%=IsAvailable(FieldStatus.updated) %>" title="<%=GetStatusToolTip(FieldStatus.updated) %>">
            <span class="legendicon updated">&nbsp;</span>
            <asp:Label ID="LBlegendtextupdated" runat="server" CssClass="legendtext">updated</asp:Label>
        </span>
        <span class="legenditem<%=IsAvailable(FieldStatus.error) %>" title="<%=GetStatusToolTip(FieldStatus.error) %>">
            <span class="legendicon error">&nbsp;</span>
            <asp:Label ID="LBlegendtexterror" runat="server" CssClass="legendtext">error</asp:Label>
        </span>
    </span>
</span>