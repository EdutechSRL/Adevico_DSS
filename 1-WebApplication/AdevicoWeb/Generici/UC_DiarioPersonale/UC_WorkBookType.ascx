<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_WorkBookType.ascx.vb" Inherits="Comunita_OnLine.UC_WorkBookType" %>
<div >
    <div style="width:850px; padding: 10px, auto; margin:0, auto;" id="DIVpersonal" enableviewstate="false" runat="server">
        <div style="width:30%; float:left; text-align:left;">
             <asp:Label runat="server" ID="LBpersonal" Font-Bold="true"></asp:Label><br />
             <asp:LinkButton runat="server" ID="LNBpersonal" CssClass="Link_Menu"></asp:LinkButton>
        </div>
        <div style="width:65%; float:left; text-align:left;" class="bordato">
             <asp:Label ID="LBpersonalDescription" runat="server" Font-Bold="true"></asp:Label>
        </div>
    <div style="clear:both;">&nbsp;</div>
    </div>
    <div style="width:850px; padding: 10px, auto; margin:0, auto; clear:both;" id="DIVpersonalShared" enableviewstate="false" runat="server">
        <div style="width:30%; float:left; text-align:left;">
             <asp:Label runat="server" ID="LBpersonalShared" Font-Bold="true"></asp:Label><br />
             <asp:LinkButton runat="server" ID="LNBpersonalShared" cssClass="Link_Menu"></asp:LinkButton>
        </div>
        <div style="width:65%; float:left; text-align:left;" class="bordato">
             <asp:Label ID="LBpersonalSharedDescription" runat="server" Font-Bold="true"></asp:Label>
        </div>
        <div style="clear:both;">&nbsp;</div>
    </div>
    <div style="width:850px; padding: 5px, auto; margin:0, auto; clear:both;" id="DIVpersonalCommunity" enableviewstate="false" runat="server">
        <div style="width:30%; float:left; text-align:left;">
             <asp:Label runat="server" ID="LBpersonalCommunity" Font-Bold="true"></asp:Label><br />
             <asp:LinkButton runat="server" ID="LNBpersonalCommunity" CssClass="Link_Menu"></asp:LinkButton>
        </div>
        <div style="width:65%; float:left; text-align:left;" class="bordato">
             <asp:Label ID="LBpersonalCommunityDescription" runat="server" Font-Bold="true" ></asp:Label>
        </div>
         <div style="clear:both;">&nbsp;</div>
    </div>
    <div style="width:850px; padding: 5px, auto; margin:0, auto; clear:both;" id="DIVcommunity" enableviewstate="false" runat="server">
        <div style="width:30%; float:left; text-align:left;">
             <asp:Label runat="server" ID="LBcommunity" Font-Bold="true"></asp:Label><br />
             <asp:LinkButton runat="server" ID="LNBcommunity" CssClass="Link_Menu"></asp:LinkButton>
        </div>
         <div style="width:65%; float:left; text-align:left;" class="bordato">
             <asp:Label ID="LBcommunityDescription" runat="server" Font-Bold="true"></asp:Label>
        </div>
        <div style="clear:both;">&nbsp;</div>
    </div>
    <div style="width:850px; padding: 5px, auto; margin:0, auto; clear:both;" id="DIVcommunityOther" enableviewstate="false" runat="server">
        <div style="width:30%; float:left; text-align:left;">
             <asp:Label runat="server" ID="LBcommunityOther" Font-Bold="true"></asp:Label><br />
             <asp:LinkButton runat="server" ID="LNBcommunityOther" CssClass="Link_Menu"></asp:LinkButton>
        </div>
         <div style="width:65%; float:left; text-align:left;" class="bordato">
             <asp:Label ID="LBcommunityOtherDescription" runat="server" Font-Bold="true"></asp:Label>
        </div>
        <div style="clear:both;">&nbsp;</div>
    </div>
    <div style="width:850px; padding: 5px, auto; margin:0, auto; clear:both;" id="DIVcommunityShared" enableviewstate="false" runat="server">
        <div style="width:30%; float:left; text-align:left;">
             <asp:Label runat="server" ID="LBcommunityShared" Font-Bold="true"></asp:Label><br />
             <asp:LinkButton runat="server" ID="LNBcommunityShared"  CssClass="Link_Menu"></asp:LinkButton>
        </div>
         <div style="width:65%; float:left; text-align:left;" class="bordato">
             <asp:Label ID="LBcommunitySharedDescription" runat="server" Font-Bold="true"></asp:Label>
        </div>
        <div style="clear:both;">&nbsp;</div>
    </div>
</div>