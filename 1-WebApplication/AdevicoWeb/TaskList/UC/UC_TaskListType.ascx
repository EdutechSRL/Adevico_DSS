<%@ Control Language="vb"  AutoEventWireup="false" CodeBehind="UC_TaskListType.ascx.vb" Inherits="Comunita_OnLine.UC_TaskListType" %>
<div >
    <div style="width:850px; padding: 10px, auto; margin:0, auto;" id="DIVpersonal" enableviewstate="false" runat="server">
        <div style="width:30%; float:left; text-align:left;">
             <asp:Label runat="server" ID="LBpersonal" Font-Bold="true" Text="*personal"></asp:Label><br />
             <asp:LinkButton runat="server" ID="LNBpersonal" CssClass="Link_Menu" Text="*Select"></asp:LinkButton>
        </div>
        <div style="width:65%; float:left; text-align:left;" class="bordato">
             <asp:Label ID="LBpersonalDescription" runat="server" Font-Bold="true" Text="*Description"></asp:Label>
        </div>
    <div style="clear:both;">&nbsp;</div>
    </div>
    <div style="width:850px; padding: 5px, auto; margin:0, auto; clear:both;" id="DIVpersonalCommunity" enableviewstate="false" runat="server">
        <div style="width:30%; float:left; text-align:left;">
             <asp:Label runat="server" ID="LBpersonalCommunity" Font-Bold="true" Text="*personalCommunity"></asp:Label><br />
             <asp:LinkButton runat="server" ID="LNBpersonalCommunity" CssClass="Link_Menu" Text="*Select"></asp:LinkButton>
        </div>
        <div style="width:65%; float:left; text-align:left;" class="bordato">
             <asp:Label ID="LBpersonalCommunityDescription" runat="server" Font-Bold="true" Text="*Description" ></asp:Label>
        </div>
         <div style="clear:both;">&nbsp;</div>
    </div>
    <div style="width:850px; padding: 5px, auto; margin:0, auto; clear:both;" id="DIVcommunity" enableviewstate="false" runat="server">
        <div style="width:30%; float:left; text-align:left;">
             <asp:Label runat="server" ID="LBcommunity" Font-Bold="true" Text="*community"></asp:Label><br />
             <asp:LinkButton runat="server" ID="LNBcommunity" CssClass="Link_Menu" Text="*Select"></asp:LinkButton>
        </div>
         <div style="width:65%; float:left; text-align:left;" class="bordato">
             <asp:Label ID="LBcommunityDescription" runat="server" Font-Bold="true" Text="*Description"></asp:Label>
        </div>
        <div style="clear:both;">&nbsp;</div>
    </div>
    
  </div>