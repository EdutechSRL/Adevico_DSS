<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_AuthenticationProviderTranslation.ascx.vb" Inherits="Comunita_OnLine.UC_AuthenticationProviderTranslation" %>
<asp:MultiView id="MLVcontrolData" runat="server" ActiveViewIndex="0">
    <asp:View ID="VIWempty" runat="server">
    
    </asp:View>
    <asp:View ID="VIWdata" runat="server">
    <span class="translation_container">
        <span class="Field_Row">
            <asp:Label ID="LBtranslationName_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBname">Name:</asp:Label>
            <asp:TextBox ID="TXBname" runat="server" Columns="80" CssClass="Testo_Campo" MaxLength="250"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RFVuniqueCode" runat="server" CssClass="Validatori" ControlToValidate="TXBname"
                    Display="Dynamic">*</asp:RequiredFieldValidator>
        </span>
        <span class="Field_Row">
            <asp:Label ID="LBtranslationDescription_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBdescription">Description:</asp:Label>
            <asp:TextBox ID="TXBdescription" runat="server" Columns="80" CssClass="Testo_Campo" Rows="5" TextMode="MultiLine"></asp:TextBox>
        </span>
        <span class="Field_Row">
            <asp:Label ID="LBtranslationForSubscribeName_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBforSubscribeName">For Subscribe Name:</asp:Label>
            <asp:TextBox ID="TXBforSubscribeName" runat="server" Columns="80" CssClass="Testo_Campo" MaxLength="250"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="Validatori" ControlToValidate="TXBforSubscribeName"
                    Display="Dynamic">*</asp:RequiredFieldValidator>
        </span>
        <span class="Field_Row">
            <asp:Label ID="LBforSubscribeDescription_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBforSubscribeDescription">For Subscribe Description:</asp:Label>
            <asp:TextBox ID="TXBforSubscribeDescription" runat="server" Columns="80" CssClass="Testo_Campo" Rows="5" TextMode="MultiLine"></asp:TextBox>
        </span>
        <span class="Field_Row" id="SPNfieldLong" runat="server" visible="false">
            <asp:Label ID="LBfieldLong_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBfieldLong">Long identifier:</asp:Label>
            <asp:TextBox ID="TXBfieldLong" runat="server" Columns="80" CssClass="Testo_Campo" MaxLength="350"></asp:TextBox>
        </span>
        <span class="Field_Row" id="SPNfieldString" runat="server" visible="false">
            <asp:Label ID="LBfieldString_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBfieldString">String identifier:</asp:Label>
            <asp:TextBox ID="TXBfieldString" runat="server" Columns="80" CssClass="Testo_Campo" MaxLength="350"></asp:TextBox>
        </span>
    </span>
    </asp:View>
</asp:MultiView>