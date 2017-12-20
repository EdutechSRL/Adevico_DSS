<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="View.aspx.vb" Inherits="Comunita_OnLine.ViewMenubar" %>
<%@ Register TagPrefix="COL" TagName="Dialog" Src="~/Modules/EduPath/UC/UCDialog.ascx" %>
<%@ Register TagPrefix="COL" Assembly="Comunita_OnLine" Namespace="Comunita_OnLine.MyUC" %>
<%@ Register Src="UC/UC_Tree.ascx" TagName="MenubarTree" TagPrefix="CTRL" %>
<%@ Register Src="UC/UC_ModuleAssignment.ascx" TagName="Modules" TagPrefix="CTRL" %>
<%@ Register Src="UC/UC_ProfileTypeAssignment.ascx" TagName="ProfileTypes" TagPrefix="CTRL" %>
<%@ Register Src="UC/UC_Translation.ascx" TagName="Translations" TagPrefix="CTRL" %>
<%@ Register Src="UC/UC_Menubar.ascx" TagName="MenubarInfo" TagPrefix="CTRL" %>
<%@ Register Src="UC/UC_Column.ascx" TagName="Column" TagPrefix="CTRL" %>
<%@ Register Src="UC/UC_TopItem.ascx" TagName="TopItem" TagPrefix="CTRL" %>
<%@ Register Src="UC/UC_MenuItem.ascx" TagName="MenuItem" TagPrefix="CTRL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="Stylesheet" href="Menu.Common.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="DVmenu" style="width: 900px; text-align: right;" align="center" runat="server">
        <asp:HyperLink ID="HYPbackToManagement" runat="server" CssClass="Link_Menu" Text="Back"
            Height="18px" CausesValidation="false"></asp:HyperLink>&nbsp;
    </div>
    <div>
        <asp:UpdateProgress ID="UPBloadItems" runat="server" AssociatedUpdatePanelID="UDPdata"
        DisplayAfter="0" DynamicLayout="true">
        <ProgressTemplate>
            <asp:Literal ID="LTloadingMessage" runat="server"></asp:Literal>&nbsp&nbsp&nbsp&nbsp<img alt="Waiting..." src="./../../images/ajax/loading5.gif" style="width: 43px; height: 11px" />

        </ProgressTemplate>

        </asp:UpdateProgress>
        <asp:UpdatePanel ID="UDPdata" runat="server" RenderMode="Inline" ChildrenAsTriggers="true" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="RadTreeViewContainer">
                    <CTRL:MenubarTree ID="CTRLmenubarTree" runat="Server" EnableDragAndDrop="false"></CTRL:MenubarTree>
                </div>
                <div style="width: 500px; overflow: auto; float: left;">
                    <asp:MultiView ID="MLVitemInfo" runat="server" ActiveViewIndex="0">
                        <asp:View ID="VIWinfoEmpty" runat="server">
                        </asp:View>
                        <asp:View ID="VIWmenubarInfo" runat="server">
                            <CTRL:MenubarInfo ID="CTRLmenubarInfo" runat="Server"></CTRL:MenubarInfo>
                        </asp:View>
                        <asp:View ID="VIWcolumnInfo" runat="server">
                            <CTRL:Column ID="CTRLcolumn" runat="Server"></CTRL:Column>
                        </asp:View>
                        <asp:View ID="VIWtopItemInfo" runat="server">
                            <CTRL:TopItem ID="CTRLtopItem" runat="Server"></CTRL:TopItem>
                        </asp:View>
                        <asp:View ID="VIWmenuInfo" runat="server">
                            <CTRL:MenuItem ID="CTRLmenuItem" runat="Server"></CTRL:MenuItem>
                        </asp:View>
                        <asp:View ID="VIWunknown" runat="server">
                            <asp:Literal ID="LTitemUnknown" runat="server"></asp:Literal>
                        </asp:View>
                    </asp:MultiView>
                    <asp:MultiView ID="MLVitemData" runat="server" ActiveViewIndex="0">
                        <asp:View ID="VIWstandard" runat="server">
                        </asp:View>
                        <asp:View ID="VIWassignments" runat="server">
                            <CTRL:Modules ID="CTRLmodules" runat="Server"></CTRL:Modules>
                            <CTRL:ProfileTypes ID="CTRLprofileTypes" runat="Server" Visible="false"></CTRL:ProfileTypes>
                            <CTRL:Translations ID="CTRLtranslations" runat="Server"></CTRL:Translations>
                        </asp:View>
                    </asp:MultiView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>