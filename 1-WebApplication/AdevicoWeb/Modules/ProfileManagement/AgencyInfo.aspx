<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPopup.Master" CodeBehind="AgencyInfo.aspx.vb" Inherits="Comunita_OnLine.AgencyInfo" %>

<%@ MasterType VirtualPath="~/AjaxPopup.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="UserInfo.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <div class="Info_Main">
    <asp:MultiView ID="MLVcontent" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWmessages" runat="server">
            <br />
            <br />
            <br />
            <br />
            <br />
            <asp:Label ID="LBinfoMessage" runat="server"></asp:Label>
            <br />
            <br />
            <br />
            <br />
            <br />
        </asp:View>
        <asp:View ID="VIWagency" runat="server">
            <div class="InfoContent">
                <span class="Field_Row">
                    <asp:Label ID="LBagencyName_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBagencyName">Name:</asp:Label>
                    <asp:Label ID="LBagencyName" runat="server" CssClass="Testo_Campo "></asp:Label>
                </span>
                <span class="Field_Row">
                    <asp:Label ID="LBagencyDescription_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBagencyDescription">Description:</asp:Label>
                    <asp:Label ID="LBagencyDescription" runat="server" CssClass="Testo_Campo agencylist"></asp:Label>
                </span>
                <span class="Field_Row" id="SPNtaxCode" runat="server" visible="false">
                    <asp:Label ID="LBagencyTaxCode_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBtaxCode">Codice fiscale:</asp:Label>
                    <asp:Label ID="LBtaxCode" runat="server" CssClass="Testo_Campo"></asp:Label>
                </span>
                <span class="Field_Row" id="SPNexternalCode" runat="server" visible="false">
                    <asp:Label ID="LBagencyExternalCode_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBexternalCode">External code:</asp:Label>
                    <asp:Label ID="LBexternalCode" runat="server" CssClass="Testo_Campo"></asp:Label>
                </span>
                <span class="Field_Row" id="SPNnationalCode" runat="server" visible="false">
                    <asp:Label ID="LBagencyNationalCode_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBnationalCode">Istat code:</asp:Label>
                    <asp:Label ID="LBnationalCode" runat="server" CssClass="Testo_Campo"></asp:Label>
                </span>
                <span class="Field_Row" runat="server" id="SPNalwaysAvailable" visible="false">
                    <asp:Label ID="LBalwaysAvailable_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBalwaysAvailable">Always available:</asp:Label>
                    <asp:Label ID="LBalwaysAvailable" runat="server" CssClass="Testo_Campo"></asp:Label>
                </span>
                <span class="Field_Row" runat="server" id="SPNorganizations" visible="false">
                    <asp:Label ID="LBorganizations_t" runat="server" CssClass="Titolo_campo" >Organizations:</asp:Label>
                    <asp:Repeater ID="RPTorganizations" runat="server">
                        <HeaderTemplate>
                            <ul class="Testo_Campo agencylist">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <li> <%#Container.DataItem.Name%></li>
                        </ItemTemplate>
                        <FooterTemplate>
                            </ul>
                        </FooterTemplate>
                    </asp:Repeater>
                </span>
                <span class="Field_Row">
                    <asp:Label ID="LBisEditable_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBisEditable">Editable:</asp:Label>
                    <asp:Label ID="LBisEditable" runat="server" CssClass="Testo_Campo"></asp:Label>
                </span>
                <span class="Field_Row" runat="server" id="SPNdefault" visible="false">
                    <asp:Label ID="LBdefault_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBdefault">Default agency:</asp:Label>
                    <asp:Label ID="LBdefault" runat="server" CssClass="Testo_Campo"></asp:Label>
                </span>
                <span class="Field_Row" runat="server" id="SPNempty" visible="false">
                    <asp:Label ID="LBdempty_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBdempty">Empty agency:</asp:Label>
                    <asp:Label ID="LBdempty" runat="server" CssClass="Testo_Campo"></asp:Label>
                </span>
            </div>
        </asp:View>
    </asp:MultiView>
    </div>
</asp:Content>