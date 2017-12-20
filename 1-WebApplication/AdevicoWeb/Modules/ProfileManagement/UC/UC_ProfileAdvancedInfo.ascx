<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ProfileAdvancedInfo.ascx.vb"
    Inherits="Comunita_OnLine.UC_ProfileAdvancedInfo" %>
<asp:MultiView ID="MLVinfo" runat="server" ActiveViewIndex="0">
    <asp:View ID="VIWdefault" runat="server">
    </asp:View>
    <asp:View ID="VIWdata" runat="server">
        <div class="InfoContent">
            <span class="Field_Row" id="SPNtaxCode" runat="server">
                <asp:Label ID="LBtaxCode_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBtaxCode">Codice fiscale:</asp:Label>
                <asp:Label ID="LBtaxCode" runat="server" CssClass="Testo_Campo"></asp:Label>
            </span>
            <asp:MultiView ID="MLVprofileAdvancedInfo" runat="server" ActiveViewIndex="0">
                <asp:View ID="VIWempty" runat="server">
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
                        <asp:Label ID="LBcompanyTaxCode_t" runat="server" CssClass="Titolo_campo large" AssociatedControlID="LBcompanyTaxCode">Company P.Iva/ C.F.:</asp:Label>
                        <asp:Label ID="LBcompanyTaxCode" runat="server" CssClass="Testo_Campo"></asp:Label>
                    </span>
                </asp:View>
                <asp:View ID="VIWemployee" runat="server">
                    <span class="Field_Row">
                        <asp:Label ID="LBagency_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBagency">Agency:</asp:Label>
                        <asp:Label ID="LBagency" runat="server" CssClass="Testo_Campo agencylist"></asp:Label>
                    </span>
                </asp:View> 
            </asp:MultiView>
            <asp:MultiView ID="MLVinfoData" runat="server" ActiveViewIndex="0">
                <asp:View ID="VIWinfoFields" runat="server">
                    <br />
                    <span class="Field_Row">
                        <asp:Label ID="LBnote_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBnote">   Note:</asp:Label>
                       <asp:Label ID="LBnote" runat="server" CssClass="Testo_Campo"></asp:Label>
                    </span><span class="Field_Row">
                        <asp:Label ID="LBprovince_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBprovince">   Province:</asp:Label>
                        <asp:Label ID="LBprovince" runat="server" CssClass="Testo_Campo"></asp:Label>
                    </span><span class="Field_Row">
                        <asp:Label ID="LBnations_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBnations">   Province:</asp:Label>
                       <asp:Label ID="LBnations" runat="server" CssClass="Testo_Campo"></asp:Label>
                    </span><span class="Field_Row">
                        <asp:Label ID="LBbirthDate_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBbirthDate">(*)Birth Date:</asp:Label>
                        <asp:Label ID="LBbirthDate" runat="server" CssClass="Testo_Campo"></asp:Label>
                    </span><span class="Field_Row">
                        <asp:Label ID="LBbirthPlace_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBbirthPlace">   Birth place:</asp:Label>
                       <asp:Label ID="LBbirthPlace" runat="server" CssClass="Testo_Campo"></asp:Label>
                    </span>
                    <span class="Field_Row">
                        <asp:Label ID="LBaddress_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBaddress">   Address:</asp:Label>
                        <asp:Label ID="LBaddress" runat="server" CssClass="Testo_Campo"></asp:Label>
                    </span><span class="Field_Row">
                        <asp:Label ID="LBpostCode_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBpostCode">   Address:</asp:Label>
                        <asp:Label ID="LBpostCode" runat="server" CssClass="Testo_Campo"></asp:Label>
                    </span><span class="Field_Row">
                        <asp:Label ID="LBcity_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBcity">   City:</asp:Label>
                        <asp:Label ID="LBcity" runat="server" CssClass="Testo_Campo"></asp:Label>
                    </span><span class="Field_Row">
                        <asp:Label ID="LBofficePhone_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBofficePhone">   Phone:</asp:Label>
                        <asp:Label ID="LBofficePhone" runat="server" CssClass="Testo_Campo"></asp:Label>
                    </span><span class="Field_Row">
                        <asp:Label ID="LBhomePhone_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBhomePhone">   Phone 2:</asp:Label>
                      <asp:Label ID="LBhomePhone" runat="server" CssClass="Testo_Campo"></asp:Label>
                    </span><span class="Field_Row">
                        <asp:Label ID="LBmobile_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBmobile">   Mobile phone:</asp:Label>
                       <asp:Label ID="LBmobile" runat="server" CssClass="Testo_Campo"></asp:Label>
                    </span><span class="Field_Row">
                        <asp:Label ID="LBfax_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBfax">   Mobile phone:</asp:Label>
                        <asp:Label ID="LBfax" runat="server" CssClass="Testo_Campo"></asp:Label>
                    </span><span class="Field_Row">
                        <asp:Label ID="LBhomePage_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBhomePage">   Homepage:</asp:Label>
                       <asp:Label ID="LBhomePage" runat="server" CssClass="Testo_Campo"></asp:Label>
                    </span>
                </asp:View>
                <asp:View ID="VIWnoFields" runat="server">
                </asp:View>
            </asp:MultiView>
        </div>
    </asp:View>
</asp:MultiView>