<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_StepSummary.ascx.vb"
    Inherits="Comunita_OnLine.UC_AuthenticationStepSummary" %>
<div class="StepData">
    <span class="Fieldrow" id="SPNauthentication" runat="server" visible="false">
        <asp:Label ID="LBauthenticationType_t" runat="server" CssClass="Titolo_campo">Autentication type:</asp:Label>
        <asp:Label ID="LBauthenticationType" runat="server" CssClass="Testo_Campo">Login:</asp:Label>
    </span>
    <span class="Fieldrow">
        <asp:Label ID="LBuserType_t" runat="server" CssClass="Titolo_campo">Profile type:</asp:Label>
        <asp:Label ID="LBuserType" runat="server" CssClass="Testo_Campo"></asp:Label>
    </span>
    <span class="Fieldrow">
        <asp:Label ID="LBnome_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBnome">(*)Name:</asp:Label>
        <asp:Label ID="LBnome" runat="server" CssClass="Testo_Campo"></asp:Label>
    </span>
    <span class="Fieldrow">
        <asp:Label ID="LBcognome_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBcognome">Surname:</asp:Label>
        <asp:Label ID="LBcognome" runat="server" CssClass="Testo_Campo">Surname:</asp:Label>
    </span>
    <span class="Fieldrow" id="SPNtaxCode" runat="server">
        <asp:Label ID="LBtaxCode_t" runat="server" CssClass="Titolo_campo">Codice fiscale:</asp:Label>
        <asp:Label ID="LBtaxCode" runat="server" CssClass="Testo_Campo"></asp:Label>
    </span>
    <span class="Fieldrow">
        <asp:Label ID="LBlingua_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBlingua">Language:</asp:Label>
        <asp:Label ID="LBlingua" runat="server" CssClass="Testo_Campo"></asp:Label>
    </span>
    <span class="Fieldrow">
        <asp:Label ID="LBmail_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBmail">Mail:</asp:Label>
        <asp:Label ID="LBmail" runat="server" CssClass="Testo_Campo">Mail:</asp:Label>
    </span>
    <span class="Fieldrow">
        <asp:Label ID="LBmostraMail" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBmostraMailUserSelection"> Mostra e-Mail</asp:Label>
        <asp:Label ID="LBmostraMailUserSelection" runat="server" CssClass="Testo_Campo" ></asp:Label>
        <span class="FieldInfo">
            <asp:Label ID="LBmailInfo" runat="server" CssClass="Testo_Campo"></asp:Label>
        </span>
    </span>
    <asp:MultiView ID="MLVuserInfo" runat="server" ActiveViewIndex="0">
    <asp:View ID="VIWemptyLoginInfo" runat="server">
        </asp:View>
        <asp:View ID="VIWinternalAuthentication" runat="server">
            <span class="Fieldrow">
                <asp:Label ID="LBlogin_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBlogin">(*)Login:</asp:Label>
                <asp:Label ID="LBlogin" runat="server" CssClass="Testo_Campo" ></asp:Label>
            </span>
        </asp:View>
        <asp:View ID="VIWexternalAuthentication" runat="server">
        </asp:View>
        <asp:View ID="VIWadminInsert" runat="server">
        <span class="Fieldrow" id="SPNexternalLong" runat="server">
            <asp:Label ID="LBexternalLong_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBexternalLong">Identificativo numerico (long):</asp:Label>
            <asp:Label ID="LBexternalLong" runat="server" CssClass="Testo_Campo" ></asp:Label>
        </span>
        <span class="Fieldrow" id="SPNexternalString" runat="server">
            <asp:Label ID="LBexternalString_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBexternalString">Identificativo stringa:</asp:Label>
            <asp:Label ID="LBexternalString" runat="server" CssClass="Testo_Campo" ></asp:Label>
        </span>
    </asp:View>
    </asp:MultiView>
    <asp:MultiView ID="MLVprofileAdvancedInfo" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWempty" runat="server">
        </asp:View>
        <asp:View ID="VIWexternal" runat="server">
            <span class="Fieldrow">
                <asp:Label ID="LBexternalUser_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBexternalUser">(*)Why you want to register:</asp:Label>
                <asp:Label ID="LBexternalUser" runat="server" CssClass="Testo_Campo"></asp:Label>
            </span>
        </asp:View>
        <asp:View ID="VIWcompany" runat="server">
            <span class="Fieldrow">
                <asp:Label ID="LBcompanyName_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBcompanyName">Company name:</asp:Label>
                <asp:Label ID="LBcompanyName" runat="server" CssClass="Testo_Campo">Company name:</asp:Label>
            </span>
            <span class="Fieldrow">
                <asp:Label ID="LBcompanyTaxCode_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBcompanyTaxCode">Company P.Iva/ C.F.:</asp:Label>
                <asp:Label ID="LBcompanyTaxCode" runat="server" CssClass="Testo_Campo"></asp:Label>
            </span>
            <span class="Fieldrow">
                <asp:Label ID="LBcompanyReaNumber_t" runat="server" CssClass="Titolo_campo large" AssociatedControlID="LBcompanyReaNumber">Company R.E.A.:</asp:Label>
                <asp:Label ID="LBcompanyReaNumber" runat="server" CssClass="Testo_Campo"></asp:Label>
            </span>
            <span class="Fieldrow">
                <asp:Label ID="LBcompanyAddress_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBcompanyAddress">Company address:</asp:Label>
                <asp:Label ID="LBcompanyAddress" runat="server" CssClass="Testo_Campo"></asp:Label>
            </span>
            <span class="Fieldrow">
                <asp:Label ID="LBcompanyCity_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBcompanyCity">Company city:</asp:Label>
                <asp:Label ID="LBcompanyCity" runat="server" CssClass="Testo_Campo"></asp:Label>
            </span>
            <span class="Fieldrow">
                <asp:Label ID="LBcompanyRegion_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBcompanyRegion">Company region/province:</asp:Label>
                <asp:Label ID="LBcompanyRegion" runat="server" CssClass="Testo_Campo"></asp:Label>
            </span>
            <span class="Fieldrow">
                <asp:Label ID="LBassociationCategories_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBassociationCategories">Associations:</asp:Label>
                <asp:Label ID="LBassociationCategories" runat="server" CssClass="Testo_Campo"></asp:Label>
            </span>
        </asp:View>
        <asp:View ID="VIWemployee" runat="server">
            <span class="Fieldrow">
                <asp:Label ID="LBagency_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBagency">Agency:</asp:Label>
                <asp:Label ID="LBagency" runat="server" CssClass="Testo_Campo"></asp:Label>
            </span>
        </asp:View> 
    </asp:MultiView>
    <span class="Fieldrow">
        <asp:Label ID="LBorganizationInfo_t" runat="server" CssClass="Titolo_campo">Organization:</asp:Label>
        <asp:Label ID="LBorganizationInfo" runat="server" CssClass="Testo_Campo"></asp:Label>
    </span>
    <span class="Fieldrow" id="SPNotherOrganizations" runat="server" visible="false">
        <asp:Label ID="LBotherOrganizations_t" runat="server" CssClass="Titolo_campo">Other organization:</asp:Label>
        <asp:Label ID="LBotherOrganizations" runat="server" CssClass="Testo_Campo"></asp:Label>
    </span>
    <span class="Fieldrow" id="SPNnotification" runat="server" visible="false">
        <asp:Label ID="LBsendNotification_t" runat="server" CssClass="Titolo_campo">Notify to user:</asp:Label>
        <asp:CheckBox ID="CBXnotify" runat="server" CssClass="Testo_Campo" />
    </span>
</div> 