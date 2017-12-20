<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="ManagerDashboard.aspx.vb" Inherits="Comunita_OnLine.ManagerDashboard" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="TopControls" Src="~/Modules/ProjectManagement/UC/List/UC_DasboardListTopControl.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="List" Src="~/Modules/ProjectManagement/UC/List/UC_TaskListPlain.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Tree" Src="~/Modules/ProjectManagement/UC/List/UC_TaskListTree.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="MultipleTree" Src="~/Modules/ProjectManagement/UC/List/UC_TaskListMultipleTree.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Dialog" Src="~/Modules/ProjectManagement/UC/List/UC_TaskInfoHeader.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
  <link href="../../Graphics/Modules/ProjectManagement/Css/ProjectManagement.css" rel="Stylesheet" />
    <link href="../../Jscript/Modules/Common/Choosen/chosen.css" rel="Stylesheet" />
    <link rel="stylesheet" href="../../Graphics/Plugins/jquery.treeTable/jquery.treeTable.css" />
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.simpleEqualize.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.blockableFieldset.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.inputActivator.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.collapsableTreeAdv.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.treeTable.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.ddbuttonlist.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.autoresize.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/ProjectManagement/projectmanagement.js"></script>
    <CTRL:Dialog id="CTRLdialogHeader" runat="server"></CTRL:Dialog>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
  <asp:MultiView ID="MLVprojectList" runat="server" ActiveViewIndex="1">
        <asp:View ID="VIWempty" runat="server">
            <br />
            <br />
            <br />
            <br />
            <asp:Label ID="LBemptyMessage" runat="server"></asp:Label>
            <br />
            <br />
            <br />
            <br />
        </asp:View>
        <asp:View ID="VIWcontent" runat="server">
            <div class="DivEpButton DivEpButtonTop">
                <asp:HyperLink ID="HYPprojectListResourceTop" class="linkMenu" runat="server" Text="*Projects list" Visible="false"></asp:HyperLink>
                <asp:HyperLink ID="HYPprojectListManagerTop" class="linkMenu" runat="server" Text="*Projects list" Visible="false"></asp:HyperLink>
            </div>
            <CTRL:Messages ID="CTRLmessages" runat="server" Visible="false" />
            <CTRL:TopControls id="CTRLtopControls" runat="server"></CTRL:TopControls>
            <CTRL:List id="CTRLlistPlain" runat="server" Visible="false"></CTRL:List>
            <CTRL:Tree id="CTRLlistTree" runat="server" Visible="false"></CTRL:Tree>
            <CTRL:MultipleTree id="CTRLlistMultipleTree" runat="server" Visible="false"></CTRL:MultipleTree>
            <div class="DivEpButton DivEpButtonBottom" runat="server" visible="false" id="DVcommandsBottom">
                <asp:HyperLink ID="HYPprojectListResourceBottom" class="linkMenu" runat="server" Text="*Projects list" Visible="false"></asp:HyperLink>
                <asp:HyperLink ID="HYPprojectListManagerBottom" class="linkMenu" runat="server" Text="*Projects list" Visible="false"></asp:HyperLink>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>