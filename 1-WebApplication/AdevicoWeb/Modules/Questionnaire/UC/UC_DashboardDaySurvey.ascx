<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_DashboardDaySurvey.ascx.vb" Inherits="Comunita_OnLine.UC_DashboardDaySurvey" %>
<div class="homesurvey clearfix" id="DVdaySurvey" runat="server">
    <asp:Label ID="LBdaySurvey" runat="server" CssClass="fieldlabel">Poll of the week:</asp:Label>
    <asp:LinkButton  runat="server" ID="LNBviewDaySurvey" cssclass="survey"></asp:LinkButton>
    <span class="icons">
        <asp:Label ID="LBisNewSurvay" runat="server" CssClass="icon hasnew"></asp:Label>
    </span>
</div>