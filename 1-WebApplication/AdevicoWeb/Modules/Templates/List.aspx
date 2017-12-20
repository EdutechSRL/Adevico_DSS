<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="List.aspx.vb" Inherits="Comunita_OnLine.TemplateList" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="CTRL" TagName="List" Src="~/Modules/Templates/UC/UC_TemplateList.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Graphics/Modules/TemplateMessages/css/TemplateMessages.css" rel="Stylesheet" />
    <link rel="stylesheet" href="../../Graphics/Plugins/jquery.treeTable/jquery.treeTable.css"/>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.collapsableTreeAdv.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.treeTable.js"></script>
     <script language="javascript" type="text/javascript">
        $(function(){
            $("table.treetable").treeTable({clickableNodeNames:true});
        })
     </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <div class="DivEpButton">
        <asp:HyperLink ID="HYPaddTemplate" runat="server" Text="*Add template" CssClass="Link_Menu" Visible="false"></asp:HyperLink>
    </div>
    <div class="contentwrapper clearfix">
        <CTRL:List id="CTRLtemplatesList" runat="server" RaiseApplyFiltersEvent="true" RaisePageChangedEvent="true" RaiseSessionTimeoutEvent="true"/>
    </div>
</asp:Content>