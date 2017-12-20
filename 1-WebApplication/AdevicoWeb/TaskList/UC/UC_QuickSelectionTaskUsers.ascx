<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_QuickSelectionTaskUsers.ascx.vb" Inherits="Comunita_OnLine.UC_QuickSelectionTaskUsers"  %>


<div runat="server" id="DIVquickUsersSelection">
    
    <div style="text-align:left; float:left;width:65px; padding:5px; display:block;">
        <asp:Label ID="LBquickRole" runat="server" CssClass="Titolo_campo">**Role:</asp:Label>
    </div>
    <div style="text-align:left;float:left; display:block; width:75px; padding:5px;">
        <asp:DropDownList ID="DDLquickRole" runat="server" AutoPostBack="true" ></asp:DropDownList>  
    </div>    
    <div style="display:block; text-align: right; float: right; width: 80% " >    
        <asp:CheckBoxList id="CBLquickSelection" runat="server" AutoPostBack="false"  DataTextField="Name" DataValueField="Id" Visible="true" RepeatDirection="Vertical">
           
        </asp:CheckBoxList>         
    </div>
</div>