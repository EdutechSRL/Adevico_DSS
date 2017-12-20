<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_StepProfileInfo.ascx.vb"
    Inherits="Comunita_OnLine.UC_AuthenticationStepProfileInfo" %>
    <%@ register tagprefix="telerik" namespace="Telerik.Web.UI" assembly="Telerik.Web.UI" %>
    <%@ Register TagPrefix="CTRL" TagName="AutoCompleteField" Src="~/Modules/Common/UC/UC_AutoCompleteField.ascx" %>
<div class="StepData">
    <span class="Fieldrow">
    <asp:Label ID="LBnome_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBname">(*)Name:</asp:Label>
    <asp:TextBox ID="TXBname" runat="server" Columns="40" CssClass="Testo_Campo" MaxLength="40"></asp:TextBox>
    <asp:RequiredFieldValidator ID="RFVname" runat="server" CssClass="Validatori" ControlToValidate="TXBname"
        Display="Dynamic">*</asp:RequiredFieldValidator>
    </span>
    <span class="Fieldrow">
        <asp:Label ID="LBcognome_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBsurname">Surname:</asp:Label>
        <asp:TextBox ID="TXBsurname" runat="server" Columns="40" CssClass="Testo_Campo" MaxLength="40"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RFVsurname" runat="server" CssClass="Validatori"
            ControlToValidate="TXBsurname" Display="Dynamic">*</asp:RequiredFieldValidator>
    </span>
    <span class="Fieldrow" id="SPNtaxCode" runat="server">
        <asp:Label ID="LBtaxCode_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBtaxCode">Codice fiscale:</asp:Label>
        <asp:TextBox ID="TXBtaxCode" runat="server" Columns="40" CssClass="Testo_Campo" MaxLength="16"></asp:TextBox>
        <asp:Label ID="LBtaxCodeDuplicate" runat="server" CssClass="Testo_Campo" Visible="false" AssociatedControlID="TXBtaxCode"></asp:Label>
        <asp:RequiredFieldValidator ID="RFVcodiceFiscale" runat="server" CssClass="Validatori"
            ControlToValidate="TXBtaxCode" Display="Dynamic">*</asp:RequiredFieldValidator>
    </span>
    <span class="Fieldrow">
        <asp:Label ID="LBlingua_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="DDLlanguage">Language:</asp:Label>
        <asp:DropDownList ID="DDLlanguage" runat="server" CssClass="Testo_Campo" Width="258px"
            AutoPostBack="true">
        </asp:DropDownList>
    </span>
    <span class="Fieldrow">
        <asp:Label ID="LBmail_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBmail">Mail:</asp:Label>
        <asp:TextBox ID="TXBmail" runat="server" Columns="40" CssClass="Testo_Campo" MaxLength="255"></asp:TextBox>
        <asp:Label ID="LBmailDuplicate" runat="server" CssClass="Testo_Campo" Visible="false" AssociatedControlID="TXBmail">></asp:Label>
        <asp:RequiredFieldValidator ID="RFVmail" runat="server" CssClass="Validatori" ControlToValidate="TXBmail"
            Display="Dynamic">*</asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="REVmail" runat="server" CssClass="Validatori"
            ControlToValidate="TXBmail" Display="dynamic" ValidationExpression="[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?">*</asp:RegularExpressionValidator>
    </span>
    <span class="Fieldrow">
        <asp:Label ID="LBmostraMail" runat="server" CssClass="Titolo_campo" AssociatedControlID="CBXshowMail"> Mostra e-Mail</asp:Label>
        <asp:CheckBox ID="CBXshowMail" runat="server" Checked="false" CssClass="Testo_Campo">
        </asp:CheckBox>
        <span class="FieldInfo">
            <asp:Label ID="LBmailInfo" runat="server" CssClass="Testo_Campo"></asp:Label>
        </span>
    </span>
<asp:MultiView ID="MLVuserInfo" runat="server" ActiveViewIndex="0">
    <asp:View ID="VIWinternalAuthentication" runat="server">
        <span class="Fieldrow">
            <asp:Label ID="LBlogin_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBlogin">(*)Login:</asp:Label>
            <asp:TextBox ID="TXBlogin" runat="server" Columns="40" CssClass="Testo_Campo" MaxLength="50"></asp:TextBox>
                <asp:Label ID="LBloginDuplicate" runat="server" CssClass="Testo_Campo" Visible="false" AssociatedControlID="TXBlogin"></asp:Label>
            <asp:RequiredFieldValidator ID="RFVlogin" runat="server" CssClass="Validatori" ControlToValidate="TXBlogin"
                Display="Dynamic">*</asp:RequiredFieldValidator>
        </span><span class="Fieldrow" id="SPNpassword" runat="server">
            <asp:Label ID="LBpwd" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBpassword">(*)Password:</asp:Label>
            <asp:TextBox ID="TXBpassword" runat="server" Columns="40" CssClass="Testo_Campo"
                MaxLength="20" TextMode="Password">
            </asp:TextBox>
            <asp:RequiredFieldValidator ID="RFVpassword" runat="server" CssClass="Validatori"
                ControlToValidate="TXBpassword" Display="Dynamic"> *</asp:RequiredFieldValidator>
        </span><span class="Fieldrow" id="SPNconfirmPassword" runat="server">
            <asp:Label ID="LBconfirmpwd" runat="server" CssClass="Titolo_campo">(*)Confirm Password:</asp:Label>
            <asp:TextBox ID="TXBconfirmPwd" runat="server" Columns="40" CssClass="Testo_Campo"
                MaxLength="20" TextMode="Password">
            </asp:TextBox>
            <asp:RequiredFieldValidator ID="RFVconfirm" runat="server" CssClass="Validatori"
                ControlToValidate="TXBconfirmPwd" Display="Dynamic"> *</asp:RequiredFieldValidator>
            <asp:CompareValidator ID="CMVpassword" runat="server" CssClass="Validatori" ControlToValidate="TXBconfirmPwd"
                Display="Dynamic" Type="String" ControlToCompare="TXBpassword">*</asp:CompareValidator>

        </span>
    </asp:View>
    <asp:View ID="VIWexternalAuthentication" runat="server">
    </asp:View>
    <asp:View ID="VIWadminInsert" runat="server">
        <span class="Fieldrow" id="SPNexternalLong" runat="server">
            <asp:Label ID="LBexternalLong_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBexternalLong">Identificativo numerico (long):</asp:Label>
            <asp:TextBox ID="TXBexternalLong" runat="server" Columns="40" CssClass="Testo_Campo" MaxLength="50"></asp:TextBox>
            <asp:Label ID="LBexternalDuplicateLong" runat="server" CssClass="Testo_Campo" Visible="false" AssociatedControlID="TXBexternalLong"></asp:Label>
            <asp:RequiredFieldValidator ID="RFVexternalLong" runat="server" ControlToValidate="TXBexternalLong"></asp:RequiredFieldValidator>
            <asp:RangeValidator id="RNVexternalLong" runat="server" SetFocusOnError="true" ControlToValidate="TXBexternalLong" MinimumValue="0" Type="Integer"></asp:RangeValidator>
        </span>
        <span class="Fieldrow" id="SPNexternalString" runat="server">
            <asp:Label ID="LBexternalString_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBexternalString">Identificativo stringa:</asp:Label>
            <asp:TextBox ID="TXBexternalString" runat="server" Columns="40" CssClass="Testo_Campo" MaxLength="50"></asp:TextBox>
            <asp:Label ID="LBexternalDuplicateString" runat="server" CssClass="Testo_Campo" Visible="false" AssociatedControlID="TXBexternalString"></asp:Label>
            <asp:RequiredFieldValidator ID="RFVexternalString" runat="server" ControlToValidate="TXBexternalString"></asp:RequiredFieldValidator>
        </span>
    </asp:View>
</asp:MultiView>
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
            <asp:TextBox ID="TXBcompanyName" runat="server" Columns="100" CssClass="Testo_Campo" MaxLength="500" Height="50px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RFVcompanyName" runat="server" CssClass="Validatori"
                ControlToValidate="TXBcompanyName" Display="Dynamic">*</asp:RequiredFieldValidator>
        </span>
        <span class="Fieldrow">
            <asp:Label ID="LBcompanyTaxCode_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBcompanytaxCode">Company P.Iva/ C.F.:</asp:Label>
            <asp:TextBox ID="TXBcompanytaxCode" runat="server" Columns="100" CssClass="Testo_Campo" MaxLength="500"></asp:TextBox>
        </span>
        <span class="Fieldrow">
            <asp:Label ID="LBcompanyReaNumber_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBreaNumber">Company R.E.A.:</asp:Label>
            <asp:TextBox ID="TXBreaNumber" runat="server" Columns="100" CssClass="Testo_Campo"
                MaxLength="250"></asp:TextBox>
        </span>
        <span class="Fieldrow">
            <asp:Label ID="LBcompanyAddress_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBcompanyAddress">Company address:</asp:Label>
            <asp:TextBox ID="TXBcompanyAddress" runat="server" Columns="100" CssClass="Testo_Campo" MaxLength="500"></asp:TextBox>
        </span>
        <span class="Fieldrow">
            <asp:Label ID="LBcompanyCity_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBcompanyCity">Company city:</asp:Label>
            <asp:TextBox ID="TXBcompanyCity" runat="server" Columns="100" CssClass="Testo_Campo" MaxLength="500"></asp:TextBox>
        </span>
        <span class="Fieldrow">
            <asp:Label ID="LBcompanyRegion_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBcompanyRegion">Company region/province:</asp:Label>
            <asp:TextBox ID="TXBcompanyRegion" runat="server" Columns="100" CssClass="Testo_Campo" MaxLength="500"></asp:TextBox>
        </span>
       
        <span class="Fieldrow">
            <asp:Label ID="LBassociationCategories_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBassociationCategories">Associations:</asp:Label>
            <asp:TextBox ID="TXBassociationCategories" runat="server" height="40px" Columns="100" CssClass="Testo_Campo"
                MaxLength="4000"></asp:TextBox>
        </span>
    </asp:View>
    <asp:View ID="VIWemployee" runat="server">
        <span class="Fieldrow">
            <asp:Label ID="LBagency_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="CTRLagency">Agency:</asp:Label>
            <CTRL:AutoCompleteField runat="server" id="CTRLagency"
            TextClass="AgencyText" ValueClass="AgencyValue"
            CssClass="Testo_Campo"></CTRL:AutoCompleteField>
         </span>
    </asp:View>
    </asp:MultiView>

    <span class="Fieldrow">
        <asp:Label ID="LBJob_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBJob">*Mansione</asp:Label>
         <asp:TextBox ID="TXBJob" runat="server" Columns="40" CssClass="Testo_Campo" MaxLength="255"></asp:TextBox>
    </span>
    <span class="Fieldrow">
        <asp:Label ID="LBSector_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBSector">*Settore</asp:Label>
        <asp:TextBox ID="TXBSector" runat="server" Columns="40" CssClass="Testo_Campo" MaxLength="255"></asp:TextBox>
    </span>
</div>