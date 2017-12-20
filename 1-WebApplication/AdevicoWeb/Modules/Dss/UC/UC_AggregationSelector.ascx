<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_AggregationSelector.ascx.vb" Inherits="Comunita_OnLine.UC_DssAggregationSelector" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<div class="fieldobject fuzzyaggregator<%=CssClass%>">
    <div class="fieldrow fuzzydescription" id="DVdescription" runat="server" visible="true">
        <div class="fuzzydescription">
            <asp:Literal ID="LTdescription" runat="server"></asp:Literal>
        </div>
    </div>
    <div class="fieldrow choice">
        <asp:Label ID="LBdssMethods_t" runat="server" CssClass="fieldlabel" AssociatedControlID="DDLmethods"></asp:Label> 
        <div class="inlinewrapper">
            <asp:DropDownList ID="DDLmethods" AutoPostBack="true" runat="server"></asp:DropDownList>
            <span class="icons" runat="server" id="SPNhelp">
                <asp:HyperLink ID="HYPhelp" runat="server" Target="_blank"><span class="icon help"></span></asp:HyperLink>
            </span>
        </div>
    </div>
    <div class="fieldrow ratingset" id="DVratingSet" runat="server">
        <asp:Label ID="LBdssRatingSets_t" runat="server" CssClass="fieldlabel" AssociatedControlID="DDLratingSet"></asp:Label> 
        <div class="inlinewrapper">
            <asp:DropDownList ID="DDLratingSet" runat="server" ></asp:DropDownList>
        </div>
    </div>
</div>
<div class="fieldobject fuzzyweights" id="DVmanualRating" runat="server" visible="false">
    <div class="fieldrow title">
        <asp:Label ID="LBmanualWeightsTitle" runat="server" CssClass="title">*Fuzzy Weights</asp:Label>
    </div>
    <div class="fieldrow info">
        <asp:Label ID="LBmanualWeightsInfo" runat="server"></asp:Label>
    </div>
    <CTRL:Messages ID="CTRLweightsMessage"  runat="server" Visible="false" />
    <asp:Repeater ID="RPTweights" runat="server">
        <ItemTemplate>
            <div class="fieldrow<%#CssItemClass(Container.DataItem) %>">
                <asp:literal ID="LTidItem" runat="server" Visible="false" Text='<%#Container.DataItem.IdObject %>'></asp:literal>
                <asp:literal ID="LTpreviousValue" runat="server" Visible="false" Text='<%#Container.DataItem.Value %>'></asp:literal>
                <asp:Label ID="LBweight" runat="server" CssClass="fieldlabel" AssociatedControlID="TXBweight"><%#Container.DataItem.Name%></asp:Label>
                <asp:TextBox ID="TXBweight" runat="server" CssClass="fieldinput" Text='<%#Container.DataItem.Value%>'></asp:TextBox>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>