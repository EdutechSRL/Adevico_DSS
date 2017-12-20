<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ProfileData.ascx.vb"
    Inherits="Comunita_OnLine.UC_ProfileData" %>
<%@ Register TagPrefix="CTRL" TagName="AutoCompleteField" Src="~/Modules/Common/UC/UC_AutoCompleteField.ascx" %>

<div class="StepData">
    <span class="Fieldrow" id="SPNname" runat="server">
        <asp:Label ID="LBnome_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBname">(*)Name:</asp:Label>
        <asp:TextBox ID="TXBname" runat="server" Columns="40" CssClass="Testo_Campo" MaxLength="40"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RFVname" runat="server" CssClass="Validatori" ControlToValidate="TXBname"
            Display="Dynamic">*</asp:RequiredFieldValidator>
    </span><span class="Fieldrow" id="SPNsurname" runat="server">
        <asp:Label ID="LBcognome_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBsurname">Surname:</asp:Label>
        <asp:TextBox ID="TXBsurname" runat="server" Columns="40" CssClass="Testo_Campo" MaxLength="40"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RFVsurname" runat="server" CssClass="Validatori"
            ControlToValidate="TXBsurname" Display="Dynamic">*</asp:RequiredFieldValidator>
    </span><span class="Fieldrow" id="SPNtaxCode" runat="server">
        <asp:Label ID="LBtaxCode_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBtaxCode">Codice fiscale:</asp:Label>
        <asp:TextBox ID="TXBtaxCode" runat="server" Columns="40" CssClass="Testo_Campo" MaxLength="16"></asp:TextBox>
        <asp:Label ID="LBtaxCodeDuplicate" runat="server" CssClass="Testo_Campo" Visible="false"
            AssociatedControlID="TXBtaxCode"></asp:Label>
        <asp:RequiredFieldValidator ID="RFVcodiceFiscale" runat="server" CssClass="Validatori"
            ControlToValidate="TXBtaxCode" Display="Dynamic">*</asp:RequiredFieldValidator>
    </span><span class="Fieldrow">
        <asp:Label ID="LBlingua_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="DDLlanguage">Language:</asp:Label>
        <asp:DropDownList ID="DDLlanguage" runat="server" CssClass="Testo_Campo" Width="258px"
            AutoPostBack="true">
        </asp:DropDownList>
    </span>
    <span class="Fieldrow">
        <asp:Label ID="LBmail_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBmail">Mail:</asp:Label>
        <asp:TextBox ID="TXBmail" runat="server" Columns="40" CssClass="Testo_Campo" MaxLength="255"></asp:TextBox>
        <asp:Label ID="LBmailDuplicate" runat="server" CssClass="Testo_Campo" Visible="false"
            AssociatedControlID="TXBmail"></asp:Label>
        <asp:Button ID="BTNmailEdit" runat="server" visible="false" CausesValidation="false"/>
        <asp:RequiredFieldValidator ID="RFVmail" runat="server" CssClass="Validatori" ControlToValidate="TXBmail"
            Display="Dynamic">*</asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="REVmail" runat="server" CssClass="Validatori"
            ControlToValidate="TXBmail" Display="dynamic" 
            ValidationExpression="^[a-zA-Z0-9.!#$%&’'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$">*</asp:RegularExpressionValidator>
    </span>
    <span class="Fieldrow">
        <asp:Label ID="LBmostraMail" runat="server" CssClass="Titolo_campo" AssociatedControlID="CBXshowMail"> Mostra e-Mail</asp:Label>
        <asp:CheckBox ID="CBXshowMail" runat="server" CssClass="Testo_Campo">
        </asp:CheckBox>
        <span class="FieldInfo">
            <asp:Label ID="LBmailInfo" runat="server" CssClass="Testo_Campo"></asp:Label>
        </span></span>
    <asp:MultiView ID="MLVprofileAdvancedInfo" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWempty" runat="server">
        </asp:View>
        <asp:View ID="VIWexternal" runat="server">
            <span class="Fieldrow">
                <asp:Label ID="LBexternalUser_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBexternalUser">(*)Why you want to register:</asp:Label>
                <asp:TextBox ID="TXBexternalUser" runat="server" Columns="100" CssClass="Testo_Campo_obbligatorioSmall"
                    MaxLength="100" Height="50px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RVFexternalUser" runat="server" CssClass="Validatori"
                    ControlToValidate="TXBexternalUser" Display="Dynamic">*</asp:RequiredFieldValidator>
            </span>
        </asp:View>
        <asp:View ID="VIWcompany" runat="server">
            <span class="Fieldrow">
                <asp:Label ID="LBcompanyName_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBcompanyName">Company name:</asp:Label>
                <asp:TextBox ID="TXBcompanyName" runat="server" Columns="100" CssClass="Testo_Campo"
                    MaxLength="500" Height="50px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RFVcompanyName" runat="server" CssClass="Validatori"
                    ControlToValidate="TXBcompanyName" Display="Dynamic">*</asp:RequiredFieldValidator>
            </span>
            <span class="Fieldrow">
                <asp:Label ID="LBcompanyTaxCode_t" runat="server" CssClass="Titolo_campo large" AssociatedControlID="TXBcompanytaxCode">Company P.Iva/ C.F.:</asp:Label>
                <asp:TextBox ID="TXBcompanytaxCode" runat="server" Columns="100" CssClass="Testo_Campo"
                    MaxLength="100"></asp:TextBox>
            </span>
            <span class="Fieldrow">
                <asp:Label ID="LBcompanyReaNumber_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBreaNumber">Company R.E.A.:</asp:Label>
                <asp:TextBox ID="TXBreaNumber" runat="server" Columns="100" CssClass="Testo_Campo"  MaxLength="500"></asp:TextBox>
            </span>
            <span class="Fieldrow">
                <asp:Label ID="LBcompanyAddress_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBcompanyAddress">Company address:</asp:Label>
                <asp:TextBox ID="TXBcompanyAddress" runat="server" Columns="100" CssClass="Testo_Campo"
                    MaxLength="500"></asp:TextBox>
            </span>
            <span class="Fieldrow">
                <asp:Label ID="LBcompanyCity_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBcompanyCity">Company city:</asp:Label>
                <asp:TextBox ID="TXBcompanyCity" runat="server" Columns="100" CssClass="Testo_Campo"
                    MaxLength="500"></asp:TextBox>
            </span>
            <span class="Fieldrow">
                <asp:Label ID="LBcompanyRegion_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBcompanyRegion">Company region/province:</asp:Label>
                <asp:TextBox ID="TXBcompanyRegion" runat="server" Columns="100" CssClass="Testo_Campo"
                    MaxLength="500"></asp:TextBox>
            </span>
            <span class="Fieldrow">
                <asp:Label ID="LBassociationCategories_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBassociationCategories">Associations:</asp:Label>
                <asp:TextBox ID="TXBassociationCategories" runat="server" Height="40px" Columns="100" CssClass="Testo_Campo"
                    MaxLength="4000"></asp:TextBox>
            </span>
        </asp:View>
        <asp:View ID="VIWemployee" runat="server">
            <span class="Fieldrow">
                <asp:Label ID="LBagency_t" runat="server" CssClass="Titolo_campo" >Agency:</asp:Label>
                <div style="display:inline-block" runat="server" id="DVagency" visible="false">
                    <CTRL:AutoCompleteField runat="server" id="CTRLagency" TextClass="AgencyText" ValueClass="AgencyValue"
                    CssClass="Testo_Campo" ReadOnly="true"></CTRL:AutoCompleteField>
                    <asp:Button ID="BTNeditAgency" runat="server" Visible="true" Text="Edit"/>
                    <asp:Button ID="BTNundoSaveAgency" runat="server" Visible="false" Text="Undo"/>
                    <asp:Button ID="BTNsaveAgency" runat="server" Visible="false" Text="Save"/>
                </div>
                <asp:Repeater ID="RPTaffiliations" runat="server">
                    <HeaderTemplate>
                        <ul class="Testo_Campo agencylist">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <li><asp:Label ID="LBagency" runat="server"></asp:Label></li>
                    </ItemTemplate>
                    <FooterTemplate>
                        </ul>
                    </FooterTemplate>
                </asp:Repeater>
            </span>
        </asp:View>
    </asp:MultiView>

    <asp:MultiView ID="MLVinfoData" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWinfoFields" runat="server">
        <br />
        <span class="Fieldrow">
            <asp:Label ID="LBnote_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBnote">   Note:</asp:Label>
            <asp:TextBox ID="TXBnote" runat="server" Columns="40" CssClass="Testo_Campo" MaxLength="1000"></asp:TextBox>
        </span>
        <span class="Fieldrow">
            <asp:Label ID="LBprovince_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="DDLprovince">   Province:</asp:Label>
            <asp:dropdownlist id="DDLprovince" Runat="server" CssClass="Testo_Campo" Width="258px" AutoPostBack="false"></asp:dropdownlist>
        </span>
        <span class="Fieldrow">
            <asp:Label ID="LBnations_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="DDLnations">   Province:</asp:Label>
            <asp:dropdownlist id="DDLnations" Runat="server" CssClass="Testo_Campo" Width="258px" AutoPostBack="true"></asp:dropdownlist>
        </span>
        <span class="Fieldrow">
            <asp:Label ID="LBbirthDate_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBbirthDate">(*)Birth Date:</asp:Label>
            <asp:TextBox ID="TXBbirthDate" runat="server" Columns="40" CssClass="Testo_Campo" MaxLength="10"></asp:TextBox>
            <asp:requiredfieldvalidator id="RFVbirthDate" runat="server" CssClass="Validatori" ControlToValidate="TXBbirthDate" Display="dynamic">*</asp:requiredfieldvalidator>
            <asp:rangevalidator id="RNVbirthDate" runat="server" CssClass="Validatori" ControlToValidate="TXBbirthDate" Display="Dynamic" Type="Date" MinimumValue="01/01/1111" MaximumValue="01/01/2999" />
        </span>
        <span class="Fieldrow">
            <asp:Label ID="LBbirthPlace_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBbirthPlace">   Birth place:</asp:Label>
            <asp:TextBox ID="TXBbirthPlace" runat="server" Columns="40" CssClass="Testo_Campo" MaxLength="100"></asp:TextBox>
        </span>
        <span class="Fieldrow">
            <asp:Label ID="LBsector_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBsector">   *Settore:</asp:Label>
            <asp:TextBox ID="TXBsector" runat="server" Columns="40" CssClass="Testo_Campo" MaxLength="100"></asp:TextBox>
        </span>
        <span class="Fieldrow">
            <asp:Label ID="LBjob_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBjob">   *Mansione:</asp:Label>
            <asp:TextBox ID="TXBjob" runat="server" Columns="40" CssClass="Testo_Campo" MaxLength="100"></asp:TextBox>
        </span>
        <span class="Fieldrow" style="display:none;">
            <asp:Label ID="LBgender_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="RBLgender">   Male:</asp:Label>
            <asp:RadioButtonList ID="RBLgender" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="Testo_Campo">
                <asp:ListItem Value="True" Selected="True">M</asp:ListItem>
                <asp:ListItem Value="False" >F</asp:ListItem>
            </asp:RadioButtonList>
        </span>
	    <span class="Fieldrow">
            <asp:Label ID="LBaddress_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBaddress">   Address:</asp:Label>
            <asp:TextBox ID="TXBaddress" runat="server" Columns="40" CssClass="Testo_Campo" MaxLength="255"></asp:TextBox>
        </span>
        <span class="Fieldrow">
            <asp:Label ID="LBpostCode_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBpostCode">   Address:</asp:Label>
            <asp:TextBox ID="TXBpostCode" runat="server" Columns="40" CssClass="Testo_Campo" MaxLength="100"></asp:TextBox>
        </span>
        <span class="Fieldrow">
            <asp:Label ID="LBcity_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBcity">   City:</asp:Label>
            <asp:TextBox ID="TXBcity" runat="server" Columns="40" CssClass="Testo_Campo" MaxLength="100"></asp:TextBox>
        </span>
        <span class="Fieldrow">
            <asp:Label ID="LBofficePhone_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBofficePhone">   Phone:</asp:Label>
            <asp:TextBox ID="TXBofficePhone" runat="server" Columns="40" CssClass="Testo_Campo" MaxLength="100"></asp:TextBox>
        </span>
        <span class="Fieldrow">
            <asp:Label ID="LBhomePhone_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBhomePhone">   Phone 2:</asp:Label>
            <asp:TextBox ID="TXBhomePhone" runat="server" Columns="40" CssClass="Testo_Campo" MaxLength="100"></asp:TextBox>
        </span>
        <span class="Fieldrow">
            <asp:Label ID="LBmobile_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBmobile">   Mobile phone:</asp:Label>
            <asp:TextBox ID="TXBmobile" runat="server" Columns="40" CssClass="Testo_Campo" MaxLength="100"></asp:TextBox>
        </span>
        <span class="Fieldrow">
            <asp:Label ID="LBfax_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBfax">   Mobile phone:</asp:Label>
            <asp:TextBox ID="TXBfax" runat="server" Columns="40" CssClass="Testo_Campo" MaxLength="100"></asp:TextBox>
        </span>
        <span class="Fieldrow">
            <asp:Label ID="LBhomePage_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBhomepage">   Homepage:</asp:Label>
            <asp:TextBox ID="TXBhomepage" runat="server" Columns="40" CssClass="Testo_Campo" MaxLength="255"></asp:TextBox>
        </span>
        </asp:View>
        <asp:View ID="VIWnoFields" runat="server">
        
        </asp:View>
    </asp:MultiView>
</div>