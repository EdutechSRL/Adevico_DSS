<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_FuzzyInputExtended.ascx.vb" Inherits="Comunita_OnLine.UC_FuzzyInputExtended" %>
<div id="tab-2" data-template="{val1} &rarr; {val2}" data-fuzzy="2">
    <div class="description" id="DVdescription" runat="server">
        <asp:Literal ID="LTfuzzyInputDescription" runat="server"></asp:Literal>
    </div>
    <asp:Label ID="LBfuzzyInputTitle" runat="server" AssociatedControlID="">*The value is</asp:Label>
    <select data-mapping="{val1}" id="SLratingValuesFrom" runat="server"></select>
    <asp:Label ID="LBfuzzyInputTitleTo" runat="server" AssociatedControlID="">*to</asp:Label>
    <select data-mapping="{val2}" id="SLratingValuesTo" runat="server"></select>
</div>