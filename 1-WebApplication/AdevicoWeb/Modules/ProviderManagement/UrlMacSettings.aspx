<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin_Globale/AdminPortal.Master" CodeBehind="UrlMacSettings.aspx.vb" Inherits="Comunita_OnLine.UrlMacSettings" %>
<%@ MasterType VirtualPath="~/Admin_Globale/AdminPortal.Master" %>
<%@ Register TagPrefix="CTRL" TagName="Attribute" Src="UC/UC_EditMacUrlAttribute.ascx" %>
<%@ Register Src="~/Modules/Common/UC/UC_ActionMessages.ascx" TagName="CTRLmessages" TagPrefix="CTRL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Graphics/Modules/ProviderManagement/Css/ProviderManagement.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../../Jscript/Modules/ProviderManagement/ProviderManagement.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.inputActivator.js"></script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <div class="viewbuttons clearfix DVmenu">
        <asp:HyperLink ID="HYPbackToManagementTop" runat="server" CssClass="Link_Menu" Text="Back to management"
            Height="18px" CausesValidation="false" Visible="false"></asp:HyperLink>
        <asp:HyperLink ID="HYPbackToProviderEdit" runat="server" CssClass="Link_Menu" Text="Back to provider"
            Height="18px" CausesValidation="false" Visible="false"></asp:HyperLink>
    </div>
    <asp:MultiView ID ="MLVdata" runat="server" ActiveViewIndex="1">
        <asp:View ID="VIWempty" runat="server">
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
        <asp:View ID="VIWproviderData" runat="server">
            <CTRL:CTRLmessages ID="CTRLmessages" runat="server" Visible="true" />
            <div class="topbar DivEpButton">
                <div class="innerwrapper clearfix">
                    <div class="left">
                        <span class="attributetypeselect">
                            <asp:Label ID="LBmacAttributeType_t" runat="server" AssociatedControlID="DDLattributesType"></asp:Label>
                            <asp:DropDownList ID="DDLattributesType" runat="server"></asp:DropDownList>
                            <asp:Button ID="BTNaddMacAttribute" runat="server" CausesValidation="false" CssClass="Link_Menu" />
                        </span>
                    </div>
                </div>
            </div>
            <asp:Repeater id="RPTattributes" runat="server">
                <HeaderTemplate>
                    <ul class="dynamicfields">
                </HeaderTemplate>

                <ItemTemplate>
                     <li class="dynamicfield <%#DisplayMode(Container.DataItem.Id) %> <%#Container.DataItem.Type.ToString.tolower %>attribute">
                        <a name="attribute_<%#Container.DataItem.Id %>"></a>
                        <ctrl:Attribute id="CTRLattribute" runat="server" OnSavedAttribute="SavedAttribute" OnUndoEditing="UndoEditing" OnEditAttribute="EditAttribute"
                         OnAddOrganizationAttributeItem="AddOrganizationAttributeItem"  OnAddMacAttributeItem="AddMacAttributeItem"
                          OnRemoveAttributeItem="RemoveAttributeItem"  OnVirtualDeleteAttribute="VirtualDeleteAttribute"
                          OnAddCatalogueAttributeItem="AddCatalogueAttributeItem" onAddCompositeAttributeItem="AddCompositeAttributeItem" OnInvalidCode="InvalidCode"  OnUnsavedAttribute="OnUnsavedAttribute"></ctrl:Attribute>
                     </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
             <div class="viewbuttons clearfix DVmenu">
                <asp:HyperLink ID="HYPbackToManagementBottom" runat="server" CssClass="Link_Menu" Text="Back to management"
                    Height="18px" CausesValidation="false" Visible="false"></asp:HyperLink>
                <asp:HyperLink ID="HYPbackToProviderEditBottom" runat="server" CssClass="Link_Menu" Text="Back to provider"
                    Height="18px" CausesValidation="false" Visible="false"></asp:HyperLink>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>