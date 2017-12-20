<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_SkinHeadLogo.ascx.vb" Inherits="Comunita_OnLine.UC_SkinHeadLogo" %>

<div class="DVmenu">
    <asp:LinkButton ID="LNBsaveAll" runat="server" CssClass="Link_Menu" CommandName="save">SaveAll</asp:LinkButton>
</div>
<asp:Repeater ID="RPTheaderLogos" runat="server" EnableViewState="true">
    <HeaderTemplate>
        <hr />
    </HeaderTemplate>
    <ItemTemplate>
        <div class="DataSmall header_logos">
            <div class="data_field img_lang">
                <asp:LinkButton ID="LNBdeleteHeaderLogo" runat="server" CssClass="DeleteSmall" CommandName="delete">X</asp:LinkButton>
                <asp:Label ID="LBlanguageName" runat="server" CssClass="Titolo_Campo">*</asp:Label>
            </div>
            <div class="data_field img_command">
                <asp:Literal ID="LTlanguageCode" runat="server" Visible="false"></asp:Literal>
            
                <asp:LinkButton ID="LNBsaveHeaderLogo" runat="server" CssClass="Link_Menu" CommandName="save">Save</asp:LinkButton>
                <asp:LinkButton ID="LNBcopyFromDefault" runat="server" CssClass="Link_Menu" CommandName="clone">Copia default</asp:LinkButton>
            </div>
            <div class="data_field img_image">
                <asp:Image ID="IMGheaderLogo" runat="server" />
                <br />
                <asp:FileUpload ID="FUPheaderLogo" runat="server" />
            </div>
            <div class="data_field img_data">
                <asp:Label ID="LBlogoLink_t" runat="server" CssClass="Titolo_Campo" AssociatedControlID="TXBheaderLogoLink">*Link:</asp:Label>
                <asp:TextBox ID="TXBheaderLogoLink" runat="server" CssClass="Testo_Campo" Columns="60"></asp:TextBox>
                <br />
                <asp:Label ID="LBlogoToolTip_t" runat="server" CssClass="Titolo_Campo" AssociatedControlID="TXBheaderLogoAlternate">*Alt:</asp:Label>
                <asp:TextBox ID="TXBheaderLogoAlternate" runat="server" CssClass="Testo_Campo" Columns="60"></asp:TextBox>
            </div>
        </div>
        <asp:HiddenField ID="HDFlogoId" runat="server" />
        <hr />    
    </ItemTemplate>
</asp:Repeater>