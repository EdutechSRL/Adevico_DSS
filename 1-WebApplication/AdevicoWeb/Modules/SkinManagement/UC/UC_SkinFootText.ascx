<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_SkinFootText.ascx.vb" Inherits="Comunita_OnLine.UC_SkinFootText" %>
<%@ Register Src="~/Modules/Common/Editor/UC_Editor.ascx" TagName="CTRLeditor" TagPrefix="CTRL" %>

<asp:Repeater ID="RPTlanguages" runat="server" EnableViewState="true">
    <ItemTemplate>
        <div class="footer_text_info">
            <asp:LinkButton ID="LNBdeleteFootText" runat="server" CssClass="DeleteSmall">#X</asp:LinkButton>
            <asp:Label ID="LBlanguageName_t" runat="server" CssClass="Titolo_Campo">*Numero</asp:Label>
        </div>
        <div class="footer_text_button">
            <asp:LinkButton ID="LNBsaveFootText" runat="server" CssClass="Link_Menu">#Save</asp:LinkButton>
        </div>
        <div class="footer_text_editor">
             <CTRL:CTRLeditor id="CTRLeditor" runat="server" ContainerCssClass="containerclass" 
                LoaderCssClass="loadercssclass" EditorHeight="250px" 
                FontSizes="2,3,4" AllAvailableFontnames="true">
            </CTRL:CTRLeditor>
        </div>
        <hr />
    </ItemTemplate>
</asp:Repeater>