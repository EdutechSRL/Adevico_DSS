<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_AddTaskAssignment.ascx.vb" Inherits="Comunita_OnLine.UC_AddTaskAssignment" %>
<%@ Register TagPrefix="CTRL" TagName="USERlist" Src="../../UC/UC_SearchUserByCommunities.ascx" %>


<div style="height:24px; ">
    <div style="text-align:left; float:left;width:65px; padding:5px;">
        <asp:Label ID="LBrole" runat="server" CssClass="Titolo_campo">Role:</asp:Label>
    </div>
    <div style="text-align:left;float:left;width:75px; padding:5px;">
        <asp:DropDownList ID="DDLrole" runat="server" AutoPostBack="true"></asp:DropDownList>  
    </div>   

    <div style="text-align:left;float:left; padding: 5 5 5 15;">
         <asp:CheckBox ID="CKBisResource" runat="server" ></asp:CheckBox>
    </div>                      
</div>
<div id="DIV1" style=" padding: 0px 5px 5px 5px; text-align:left; clear:both;" runat="server">
    &nbsp;&nbsp;&nbsp;
</div> 
<div style="text-align:left; padding:5px;"  >
    <CTRL:USERlist ID="CTRLsearchUser" runat="server" />
</div>