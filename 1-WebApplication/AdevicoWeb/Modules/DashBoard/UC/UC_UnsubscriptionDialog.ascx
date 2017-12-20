<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_UnsubscriptionDialog.ascx.vb" Inherits="Comunita_OnLine.UC_UnsubscriptionDialog" %>
<div class="dlgconfirmunsubscription hiddendialog" title="<%=GetDialogTitle() %>">
 <div class="fieldobject options">
    <div class="fieldrow message" id="DVdescription" runat="server" visible="false">
        <asp:Label ID="LBdescription" runat="server" CssClass="description"></asp:Label>
    </div>
    <asp:Repeater ID="RPTactions" runat="server">
        <ItemTemplate>
            <div class="fieldrow option">
                <input type="radio" id="RDremoveaction-<%#CInt(Container.DataItem) %>" name="RDremoveaction" value="<%#CInt(Container.DataItem) %>" <%# SetChecked(Container.DataItem)%> />
                <label for="RDremoveaction-<%#CInt(Container.DataItem) %>"><%#Resource.getValue("name.RemoveAction." & Container.DataItem.ToString())%></label>
                <asp:Label ID="LBactionDescription" runat="server" cssclass="option description"> </asp:Label>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>
<div class="fieldobject clearfix" id="DVcommands" runat="server">
    <div class="fieldrow buttons right">
        <asp:Button ID="BTNapplyUnsubscribeFromCommunity" runat="server" CssClass="linkMenu" />
        <asp:HyperLink ID="HYPcloseUnsubscribeFromCommunityDialog" runat="server" CssClass="linkMenu close" Text="*Close"></asp:HyperLink>
    </div>
</div>
</div>
<asp:Literal ID="LTcssClassDialog" runat="server" Visible="false">.dlgconfirmunsubscription</asp:Literal>
<asp:Literal ID="LTtemplateMessageDetails" runat="server" Visible="false"><ul class="messagedetails">{0}</ul></asp:Literal>
<asp:Literal ID="LTtemplateMessageDetail" runat="server" Visible="false"><li class="messagedetail">{0}</li></asp:Literal>