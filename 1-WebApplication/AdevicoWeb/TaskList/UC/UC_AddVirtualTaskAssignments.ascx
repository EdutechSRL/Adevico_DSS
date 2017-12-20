<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_AddVirtualTaskAssignments.ascx.vb" Inherits="Comunita_OnLine.UC_AddVirtualTaskAssignments" %>
<%@ Register TagPrefix="CTRL" TagName="USERlist" Src="../../UC/UC_SearchUserByCommunities.ascx" %>


<div style="height:24px; ">
    <div style="text-align:left; float:left;width:100px; padding:5px;">
        <asp:Label ID="LBrole" runat="server" CssClass="Titolo_campo" Text="*Role:"></asp:Label>
    </div>
    <div style="text-align:left; padding:5px;">
        <asp:DropDownList ID="DDLrole" runat="server" AutoPostBack="true"></asp:DropDownList>  
    </div>                     
</div>
<div id="DIV1" style=" padding: 0px 5px 5px 5px; text-align:left; clear:both;" runat="server">
    &nbsp;&nbsp;&nbsp;
</div> 
<div style="text-align:left;padding:5px;" >
    <CTRL:USERlist ID="CTRLsearchUser" runat="server" />
</div>