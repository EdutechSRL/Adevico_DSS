<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="EditAgency.aspx.vb" Inherits="Comunita_OnLine.EditAgency" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Graphics/Modules/ProfileManagement/css/ProfileManagement.css?v=201604071200lm" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">


</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="DVmenu" class="DVmenu" runat="server">
        <asp:HyperLink ID="HYPbackToManagement" runat="server" CssClass="Link_Menu" Text="Back"
            Height="18px" CausesValidation="false"></asp:HyperLink>
        <asp:LinkButton ID="LNBsaveTop" runat="server"  CssClass="Link_Menu" > </asp:LinkButton>
    </div>
    <div style="width: 900px; text-align: center; padding-top: 5px; margin: 0px auto;">
        <asp:MultiView ID="MLVagency" runat="server" ActiveViewIndex="1">
            <asp:View ID="VIWdefault" runat="server">
                <br />
                <br />
                <br />
                <br />
                <br />
                <asp:Label ID="LBmessage" runat="server"></asp:Label>
                <br />
                <br />
                <br />
                <br />
                <br />
            </asp:View>
            <asp:View ID="VIWedit" runat="server">
                <CTRL:Messages ID="CTRLmessages"  runat="server" Visible="false" />
                    <span class="Fieldrow">
                        <asp:Label ID="LBagencyName_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBname">(*)Name:</asp:Label>
                        <asp:TextBox ID="TXBname" runat="server" Columns="80" CssClass="Testo_Campo" MaxLength="40"></asp:TextBox>
                        <asp:Label ID="LBagencyNameDuplicated" runat="server" CssClass="Testo_Campo" Visible="false" AssociatedControlID="TXBname"></asp:Label>
                        <asp:RequiredFieldValidator ID="RFVname" runat="server" CssClass="Validatori" ControlToValidate="TXBname"
                        Display="Dynamic">*</asp:RequiredFieldValidator>
                    </span>
                     <span class="Fieldrow">
                        <asp:Label ID="LBagencyDescription_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBdescription">Description</asp:Label>
                        <asp:TextBox ID="TXBdescription" runat="server" Columns="77" CssClass="Testo_Campo"  Rows="4" TextMode="MultiLine"></asp:TextBox>
                    </span>
                    <span class="Fieldrow">
                        <asp:Label ID="LBagencyTaxCode_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBtaxCode">Codice fiscale:</asp:Label>
                        <asp:TextBox ID="TXBtaxCode" runat="server" Columns="60" CssClass="Testo_Campo" MaxLength="50"></asp:TextBox>
                        <asp:Label ID="LBtaxCodeDuplicate" runat="server" CssClass="Testo_Campo" Visible="false" AssociatedControlID="TXBtaxCode"></asp:Label>
                    </span>
                    <span class="Fieldrow">
                        <asp:Label ID="LBagencyExternalCode_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBexternalCode">Codice esterno:</asp:Label>
                        <asp:TextBox ID="TXBexternalCode" runat="server" Columns="60" CssClass="Testo_Campo" MaxLength="50"></asp:TextBox>
                        <asp:Label ID="LBexternalCodeDuplicate" runat="server" CssClass="Testo_Campo" Visible="false" AssociatedControlID="TXBexternalCode"></asp:Label>
                    </span>
                     <span class="Fieldrow">
                        <asp:Label ID="LBagencyNationalCode_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBnationalCode">Codice esterno:</asp:Label>
                        <asp:TextBox ID="TXBnationalCode" runat="server" Columns="60" CssClass="Testo_Campo" MaxLength="50"></asp:TextBox>
                        <asp:Label ID="LBnationalCodeDuplicate" runat="server" CssClass="Testo_Campo" Visible="false" AssociatedControlID="TXBnationalCode"></asp:Label>
                    </span>
                    <span class="Fieldrow">
                        <asp:Label ID="LBdefault_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="CBXdefault">Di default:</asp:Label>
                        <asp:CheckBox ID="CBXdefault" runat="server" CssClass="Testo_Campo"  />
                    </span>
                    <span class="Fieldrow">
                        <asp:Label ID="LBdempty_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="CBXempty">Ente vuoto:</asp:Label>
                        <asp:CheckBox ID="CBXempty" runat="server" CssClass="Testo_Campo"  />
                    </span>
                    <span class="Fieldrow">
                        <asp:Label ID="LBalwaysAvailable_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="CBXalwaysAvailable">Sempre disponbile:</asp:Label>
                        <asp:CheckBox ID="CBXalwaysAvailable" runat="server" CssClass="Testo_Campo" AutoPostBack="true"  />
                    </span>
                    <span class="Fieldrow" runat="server" id="SPNorganizations">
                        <asp:Label ID="LBavailableForOrganizations_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="CBLorganizations">Sempre disponbile:</asp:Label>  
                        <asp:CheckBoxList ID="CBLorganizations" runat="server" CssClass="Testo_Campo"  RepeatDirection="Horizontal" RepeatColumns="3" RepeatLayout="Flow"></asp:CheckBoxList>
                    </span>
            </asp:View>
        </asp:MultiView>
    </div>
    <div style="width: 900px; text-align: right; padding-top: 5px; margin: 0px auto;">
        <asp:HyperLink ID="HYPbackToManagementBottom" runat="server" CssClass="Link_Menu" Text="Back"
            Height="18px" CausesValidation="false"></asp:HyperLink>
        <asp:LinkButton ID="LNBsaveBottom" runat="server" CssClass="Link_Menu"> </asp:LinkButton>
    </div>
</asp:Content>