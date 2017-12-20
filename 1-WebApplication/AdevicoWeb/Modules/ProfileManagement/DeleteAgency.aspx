<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="DeleteAgency.aspx.vb" Inherits="Comunita_OnLine.DeleteAgency" %>
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
    </div>
    <asp:UpdateProgress AssociatedUpdatePanelID="UDPdeleteAgency" runat="server" ID="UPPagencyLoading"
        DisplayAfter="10">
        <ProgressTemplate>
            <div id="progressBackgroundFilter">
            </div>
            <div id="processMessage">
                <%-- <div id="imgdivLoading" align="center" valign="middle" runat="server" style="border-style: dotted;
                            padding: inherit; margin: auto; position: absolute; visibility: visible; vertical-align: middle;
                            border-color: #000066 black black black; border-width: medium; background-color: Gray; width: 900px;">--%>
                Loading...<br />
                <asp:Image ID="imgLoading" runat="server" ImageUrl="./../../Images/Ajax/loading4.gif" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div class="DeleteProfile_Del">

            <asp:Label ID="LBelimina_t" runat="server"></asp:Label>

        <div class="DeleteAgency_Update">
          <asp:UpdatePanel ID="UDPdeleteAgency" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                    <ContentTemplate>
            <asp:LinkButton CssClass="Link_Menu" runat="server" Text="Delete" ID="LNBconfirmDelete"
                CausesValidation="false" Visible="false"></asp:LinkButton>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>