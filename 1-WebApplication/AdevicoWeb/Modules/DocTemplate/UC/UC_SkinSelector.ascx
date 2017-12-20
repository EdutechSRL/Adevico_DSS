<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_SkinSelector.ascx.vb" Inherits="Comunita_OnLine.UC_SkinSelector" %>
<%@ Register TagPrefix="CTRL" TagName="ComSel" Src="~/uc/UC_SearchCommunityByService.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="OrgnSel" Src="~/Modules/DocTemplate/Uc/UcOrganizationSelector.ascx" %>

<fieldset class="light">
	<legend>
        <asp:literal ID="LITskin_t" runat="server">Selettore skin</asp:literal>
    </legend>
    <div class="fieldobject">
        <div class="fieldrow">
            <asp:Label ID="LBLcommunityID_t" runat="server" CssClass="fieldlabel">*Community :</asp:Label>
            <asp:TextBox ID="TXBcommunityID" runat="server" CssClass="inputtext" Enabled="false"></asp:TextBox>
            <asp:LinkButton ID="LKBshowComSel" runat="server">*Show Community Selector</asp:LinkButton>
            
            <asp:Panel ID="PNLcommunity" runat="server" Visible="false">
                <asp:LinkButton ID="LKBundoCom" runat="server" CssClass="linkMenu RightButton">Deselect community</asp:LinkButton>
                <CTRL:ComSel ID="UCcomSel" runat="server" />
            </asp:Panel>
            
        </div>
        <div class="fieldrow">
            <asp:Label ID="LBLorganizationID_t" runat="server" CssClass="fieldlabel">*Organization :</asp:Label>
            <asp:TextBox ID="TXBorganizationID" runat="server" CssClass="inputtext" Enabled="false"></asp:TextBox>
            <asp:LinkButton ID="LKBshowOrgnSel" runat="server">*Show Organization Selector</asp:LinkButton>

            

            <asp:Panel ID="PNLorganization" runat="server" Visible="false">
                <asp:LinkButton ID="LKBundoOrgn" runat="server" CssClass="linkMenu RightButton">Deselect Organization</asp:LinkButton>
                <CTRL:OrgnSel ID="UCorgSel" runat="server" />
            </asp:Panel>

        </div>
        <div class="fieldrow">
            <asp:Label ID="LBLlanguageCode_t" runat="server" CssClass="fieldlabel">*Language Code:</asp:Label>
            <asp:DropDownList ID="DDL_TestLang" runat="server" CssClass="inputtext">
                <asp:ListItem Value="it-IT" Selected="True"></asp:ListItem>
                <asp:ListItem Value="en-US"></asp:ListItem>
                <asp:ListItem Value="de-DE"></asp:ListItem>
                <asp:ListItem Value="fr-FR"></asp:ListItem>
                <asp:ListItem Value="es-ES"></asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="fieldrow">
            <asp:LinkButton ID="LKBload" runat="server" Text="Load" CssClass="linkMenu"></asp:LinkButton>
            <asp:LinkButton ID="LKBClearCache" runat="server" Text="Clear cache" CssClass="linkMenu" Enabled="false" Visible="false"></asp:LinkButton>
        </div>
        <div class="fieldrow">
            <asp:literal ID="LITnote_t" runat="server">
                Si ricorda che i parametri qui inseriti servono solamente alla visualizzazione dei contenuti di una Skin.
                La singola skin, infatti, non è associata al template corrente.
            </asp:literal>
        </div>
    </div>
</fieldset>

<asp:Panel ID="PNLpreview" runat="server" Visible="false">
    <fieldset class="light">
	    <legend>
            <asp:literal ID="LITheaderLogo_t" runat="server">*Header Logo</asp:literal>
        </legend>
        <div class="fieldobject">
            <asp:Image ID="IMGheaderLogo" runat="server" />
            <asp:Label ID="LBLheaderLogoinfo" runat="server"></asp:Label>
        </div>
    </fieldset>
    <fieldset class="light">
	    <legend>
            <asp:literal ID="LITheaderText_t" runat="server">*Header Text</asp:literal>
        </legend>
        <div class="fieldobject">
            <asp:Label ID="LBLheaderText" runat="server" CssClass="Testo_campo"></asp:Label>
        </div>
    </fieldset>

    <fieldset class="light">
	    <legend>
            <asp:literal ID="LITfooterLogos_t" runat="server">*Footer Logos</asp:literal>
        </legend>
        <div class="fieldobject">
            <asp:Repeater ID="RPTfooterLogos" runat="server">
                <HeaderTemplate><ul></HeaderTemplate>
                <ItemTemplate>
                    <li>
                        <asp:Image ID="IMGfooterLogos" runat="server" />                
                    </li>
                </ItemTemplate>
                <FooterTemplate></ul></FooterTemplate>
            </asp:Repeater>
        </div>
    </fieldset>

    <fieldset class="light">
	    <legend>
            <asp:literal ID="LITfooterText" runat="server">*Footer Text</asp:literal>
        </legend>
        <div class="fieldobject footerText">
            <asp:Label ID="LBLfooterText" runat="server" CssClass="Testo_campo"></asp:Label>        
        </div>
    </fieldset>
</asp:Panel>