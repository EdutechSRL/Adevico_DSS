<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_TextareaEditor.ascx.vb" Inherits="Comunita_OnLine.UC_textareaEditor" %>

<span class="textareaeditor ">
    <asp:TextBox ID="TXBeditor" runat="server"  CssClass="textarea">
    
    </asp:TextBox>
    <span class="maxchar" runat="server" id="SPNmaxChars"  Visible="false">
        <asp:Literal ID="LTmaxCharsmultiline" runat="server"></asp:Literal>
        <span class="availableitems">{available}</span>/<span class="totalitems">{total}</span>
    </span>
</span>