<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_FuzzyInputIntermediateValues.ascx.vb" Inherits="Comunita_OnLine.UC_FuzzyInputIntermediateValues" %>
<div id="tab-4" data-template="{val1} / {val2}" data-fuzzy="4">
    <div class="description" id="DVdescription" runat="server">
        <asp:Literal ID="LTfuzzyInputDescription" runat="server"></asp:Literal>
    </div>
    <asp:Label ID="LBfuzzyInputTitle" runat="server" AssociatedControlID="">*The value is between</asp:Label>
    <select data-mapping="{val1}" id="SLratingValuesFrom" runat="server"></select>
    <asp:Label ID="LBfuzzyInputTitleBetween" runat="server" AssociatedControlID="">*and</asp:Label>
    <select data-mapping="{val2}" id="SLratingValuesTo" runat="server"></select>
</div>