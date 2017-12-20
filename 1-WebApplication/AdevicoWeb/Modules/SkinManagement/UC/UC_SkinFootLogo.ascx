<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_SkinFootLogo.ascx.vb" Inherits="Comunita_OnLine.UC_SkinFootLogo" %>

<div class="DataSmall footer_logos">
    <div class="data_field img_override clearfix">
        <asp:Label ID="LBoverrideVoid_t" runat="server" CssClass="Titolo_Campo" AssociatedControlID="CBXovverride">*Sovrascrivi con vuoto</asp:Label>
        <asp:checkbox ID="CBXovverride" runat="server" />
    </div>
</div>
<hr />
<div class="DataSmall footer_logos" id="DVnewLogo" runat="server">

    <div class="data_field img_new clearfix">
        <asp:Label ID="LBnewLogoInfo_t" runat="server" CssClass="Titolo_Campo">Nuovo logo</asp:Label>    
    </div>
    <div class="data_field img_lang">
        <asp:Label ID="LBlanguageToAssociate_t" runat="server" CssClass="Titolo_Campo" AssociatedControlID="CBLlanguages">*Lingue associate:</asp:Label>
        <asp:CheckBoxList ID="CBLlanguages" runat="server" CssClass="Testo_Campo" RepeatLayout="Flow" RepeatColumns="5">
        </asp:CheckBoxList>
    </div>
    <div class="data_field img_command">
        <asp:LinkButton ID="LNBuploadFooterLogo" runat="server" CssClass="Link_Menu">#CreateNew</asp:LinkButton> 
    </div>
    <div class="data_field img_image">
        <asp:FileUpload ID="FUPlogo" runat="server" /> 
    </div>
           
    <div class="data_field img_data">
        <asp:Label ID="LBlogoLink_t" runat="server" CssClass="Titolo_Campo" AssociatedControlID="TXBfooterLogoLink">*Link:</asp:Label>
        <asp:TextBox ID="TXBfooterLogoLink" runat="server" CssClass="Testo_Campo" Columns="60"></asp:TextBox>
        <br />
        <asp:Label ID="LBlogoToolTip_t" runat="server" CssClass="Titolo_Campo" AssociatedControlID="TXBfooterLogoToolTip">*Alt:</asp:Label>
        <asp:TextBox ID="TXBfooterLogoToolTip" runat="server" CssClass="Testo_Campo" Columns="60"></asp:TextBox> 
    </div>

</div>
<hr />
<asp:Repeater ID="RPTfooterLogos" runat="server">
    <ItemTemplate>
        <div class="DataSmall footer_logos">
            <div class="data_field img_lang">
                <asp:Label ID="LBlanguageToAssociate_t" runat="server" CssClass="Titolo_Campo"  AssociatedControlID="CBLlanguages">*Lingue associate:</asp:Label>
                <asp:CheckBoxList ID="CBLlanguages" runat="server" CssClass="Testo_Campo" RepeatLayout="Flow" RepeatColumns="5">
                </asp:CheckBoxList>
            </div>
            <div class="data_field img_command">
                <asp:LinkButton ID="LNBsaveFooterLogo" runat="server" CssClass="Link_Menu" CommandName="save">#Save</asp:LinkButton>
            </div>
            <div class="data_field img_image">
                <asp:LinkButton ID="LNBdeleteFooterLogo" runat="server" CssClass="DeleteSmall" CommandName="delete">X</asp:LinkButton>
                <asp:Image ID="IMGlogo" runat="server" />
                <br />
                <asp:FileUpload ID="FUPuploadFooterLogo" runat="server" />
                <asp:LinkButton ID="LNBeditFooterLogo" runat="server" CssClass="Link_Menu" CommandName="edit">#Upload</asp:LinkButton>
            </div>
           
            <div class="data_field img_data">
                <asp:Label ID="LBlogoLink_t" runat="server" CssClass="Titolo_Campo" AssociatedControlID="TXBfooterLogoLink">*Link:</asp:Label>
                <asp:TextBox ID="TXBfooterLogoLink" runat="server" CssClass="Testo_Campo" Columns="60"></asp:TextBox>
                <br />
                <asp:Label ID="LBlogoToolTip_t" runat="server" CssClass="Titolo_Campo" AssociatedControlID="TXBfooterLogoToolTip">*Alt:</asp:Label>
                <asp:TextBox ID="TXBfooterLogoToolTip" runat="server" CssClass="Testo_Campo" Columns="60"></asp:TextBox>
            </div>
        </div>
        <hr />    
    </ItemTemplate>
</asp:Repeater>