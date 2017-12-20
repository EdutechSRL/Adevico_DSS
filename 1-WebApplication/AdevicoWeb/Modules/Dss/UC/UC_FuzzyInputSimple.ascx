<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_FuzzyInputSimple.ascx.vb" Inherits="Comunita_OnLine.UC_FuzzyInputSimple" %>
<div id="tab-1" data-template="{val}" data-fuzzy="1">
    <div class="description" id="DVdescription" runat="server">
        <asp:Literal ID="LTfuzzyInputDescription" runat="server"></asp:Literal>
    </div>
    <asp:Label ID="LBfuzzyInputTitle" runat="server" AssociatedControlID="">*The value is</asp:Label>
    <select data-mapping="{val}" id="SLratingValues" runat="server"></select>
</div>