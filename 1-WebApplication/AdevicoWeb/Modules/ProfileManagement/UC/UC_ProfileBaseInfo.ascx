<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ProfileBaseInfo.ascx.vb"
    Inherits="Comunita_OnLine.UC_ProfileBaseInfo" %>
<asp:MultiView ID="MLVinfo" runat="server" ActiveViewIndex="0">
    <asp:View ID="VIWdefault" runat="server">
    </asp:View>
    <asp:View ID="VIWdata" runat="server">
        <div class="InfoContent">
            <span class="Field_Row" runat="server" id="SPNdefaultOrganization">
                <asp:Label ID="LBprimaryOrganization_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBprimaryOrganization">Organization:</asp:Label>
                <asp:Label ID="LBprimaryOrganization" runat="server" CssClass="Testo_Campo"></asp:Label>
            </span><span class="Field_Row">
                <asp:Label ID="LBname_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBname">Name:</asp:Label>
                <asp:Label ID="LBname" runat="server" CssClass="Testo_Campo"></asp:Label>
            </span><span class="Field_Row">
                <asp:Label ID="LBsurname_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBsurname">Surname:</asp:Label>
                <asp:Label ID="LBsurname" runat="server" CssClass="Testo_Campo"></asp:Label>
            </span>
            <span class="Field_Row" id="SPNtaxCode" runat="server">
                <asp:Label ID="LBtaxCode_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBtaxCode">Codice fiscale:</asp:Label>
                <asp:Label ID="LBtaxCode" runat="server" CssClass="Testo_Campo"></asp:Label>
            </span>
            <span class="Field_Row">
                <asp:Label ID="LBlingua_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBlingua">Language:</asp:Label>
                <asp:Label ID="LBlingua" runat="server" CssClass="Testo_Campo"></asp:Label>
            </span><span class="Field_Row">
                <asp:Label ID="LBmail_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBmail">Mail:</asp:Label>
                <asp:Label ID="LBmail" runat="server" CssClass="Testo_Campo"></asp:Label>
            </span>
            <asp:MultiView ID="MLVprofileAdvancedInfo" runat="server" ActiveViewIndex="0">
                <asp:View ID="VIWempty" runat="server">
                </asp:View>
                <asp:View ID="VIWstudentUNITN" runat="server">
                    <span class="Field_Row">
                        <asp:Label ID="LBmatricola_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBmatricola">(*)Student Number:</asp:Label>
                        <asp:Label ID="LBmatricola" runat="server" CssClass="Testo_Campo"></asp:Label>
                    </span><span class="Field_Row">
                        <asp:Label ID="LBimmatricolazione_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBimmatricolazione">Year:</asp:Label>
                        <asp:Label ID="LBimmatricolazione" runat="server" CssClass="Testo_Campo"></asp:Label>
                    </span>
                </asp:View>
                <asp:View ID="VIWhighSchoolStudent" runat="server">
                    <span class="Field_Row">
                        <asp:Label ID="LBistitutoSTU_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBistitutoSTU">Institute:</asp:Label>
                        <asp:Label ID="LBistitutoSTU" runat="server" CssClass="Testo_Campo"></asp:Label>
                    </span><span class="Field_Row">
                        <asp:Label ID="LBclasseSezione_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBclasseSezione">Class and Section:</asp:Label>
                        <asp:Label ID="LBclasseSezione" runat="server" CssClass="Testo_Campo"></asp:Label>
                    </span>
                </asp:View>
                <asp:View ID="VIWhighSchoolTeacher" runat="server">
                    <span class="Field_Row">
                        <asp:Label ID="LBistituto_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBistituto">Institute:</asp:Label>
                        <asp:Label ID="LBistituto" runat="server" CssClass="Testo_Campo"></asp:Label>
                    </span><span class="Field_Row">
                        <asp:Label ID="LBmatters_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBmatters">Matters:</asp:Label>
                        <asp:Label ID="LBmatters" runat="server" CssClass="Testo_Campo"></asp:Label>
                    </span>
                </asp:View>
                <asp:View ID="VIWphdStudent" runat="server">
                    <span class="Field_Row">
                        <asp:Label ID="LBphd_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBphd">(*)Phd:</asp:Label>
                        <asp:Label ID="LBphd" runat="server" CssClass="Testo_Campo"></asp:Label>
                    </span>
                </asp:View>
                <asp:View ID="VIWtechnician" runat="server">
                    <span class="Field_Row">
                        <asp:Label ID="LBtechnician_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBtechnician">Job:</asp:Label>
                        <asp:Label ID="LBtechnician" runat="server" CssClass="Testo_Campo"></asp:Label>
                    </span>
                </asp:View>
                <asp:View ID="VIWexternal" runat="server">
                    <span class="Field_Row">
                        <asp:Label ID="LBexternalUser_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBexternalUser"></asp:Label>
                        <asp:Label ID="LBexternalUser" runat="server" CssClass="Testo_Campo"></asp:Label>
                    </span>
                </asp:View>
                <asp:View ID="VIWcompany" runat="server">
                    <span class="Field_Row">
                        <asp:Label ID="LBcompanyName_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBcompanyName">Company name:</asp:Label>
                        <asp:Label ID="LBcompanyName" runat="server" CssClass="Testo_Campo"></asp:Label>
                    </span>
                    <span class="Field_Row">
                        <asp:Label ID="LBcompanyTaxCode_t" runat="server" CssClass="Titolo_campo large" AssociatedControlID="LBcompanyTaxCode">Company P.Iva/ C.F.:</asp:Label>
                        <asp:Label ID="LBcompanyTaxCode" runat="server" CssClass="Testo_Campo"></asp:Label>
                    </span>
                    <span class="Field_Row">
                        <asp:Label ID="LBcompanyReaNumber_t" runat="server" CssClass="Titolo_campo large" AssociatedControlID="LBcompanyReaNumber">Company R.E.A.:</asp:Label>
                        <asp:Label ID="LBcompanyReaNumber" runat="server" CssClass="Testo_Campo"></asp:Label>
                    </span>
                    <span class="Field_Row">
                        <asp:Label ID="LBcompanyAddress_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBcompanyAddress">Company address:</asp:Label>
                        <asp:Label ID="LBcompanyAddress" runat="server" CssClass="Testo_Campo"></asp:Label>
                    </span>
                    <span class="Field_Row">
                        <asp:Label ID="LBcompanyCity_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBcompanyCity">Company city:</asp:Label>
                        <asp:Label ID="LBcompanyCity" runat="server" CssClass="Testo_Campo"></asp:Label>
                    </span>
                    <span class="Field_Row">
                        <asp:Label ID="LBcompanyRegion_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBcompanyRegion">Company region/province:</asp:Label>
                        <asp:Label ID="LBcompanyRegion" runat="server" CssClass="Testo_Campo"></asp:Label>
                    </span>
                    <span class="Field_Row">
                        <asp:Label ID="LBassociationCategories_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBassociationCategories">Associations:</asp:Label>
                        <asp:Label ID="LBassociationCategories" runat="server" CssClass="Testo_Campo"></asp:Label>
                    </span>
                </asp:View>
                <asp:View ID="VIWemployee" runat="server">
                    <span class="Field_Row">
                        <asp:Label ID="LBagency_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBagency">Agency:</asp:Label>
                        <asp:Label ID="LBagency" runat="server" CssClass="Testo_Campo agencylist"></asp:Label>
                    </span>
                </asp:View> 
            </asp:MultiView>
            <span class="Field_Row" id="SPNotherOrganizations" runat="server">
                <asp:Label ID="LBotherOrganizations_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBotherOrganizations">Organization:</asp:Label>
                <asp:Label ID="LBotherOrganizations" runat="server" CssClass="Testo_Campo"></asp:Label>
            </span>
        </div>
    </asp:View>
</asp:MultiView>