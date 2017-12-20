<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="NoticeboardDashboard.aspx.vb" Inherits="Comunita_OnLine.NoticeboardDashboard" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLrender" Src="./UC/UC_NoticeboardRender.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="Stylesheet" type="text/css" href="<%=PageUtility.ApplicationUrlBase & "/Graphics/Modules/NoticeBoard/noticeboard.lb.css"%>" />
    <link rel="Stylesheet" type="text/css" href="<%=PageUtility.ApplicationUrlBase & "/Style/Latex/Latex.css"%>" />
     <script type="text/javascript" src="<%=PageUtility.ApplicationUrlBase & "Jscript/Modules/Common/jquery.ddbuttonlist.js"%>"></script>
     <script type="text/javascript" src="<%=PageUtility.ApplicationUrlBase & "Jscript/Modules/Noticeboard/noticeboard.js"%>"></script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:MultiView ID="MLVcontent" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWcontainer" runat="server">
            <div class="fieldobject toolbar clearfix">
                <div class="fieldrow left">
                    <div class="ddbuttonlist" id="DVeditButtons" runat="server" visible="false" ><!--
                    --><asp:HyperLink ID="HYPeditWihAdvancedEditor" runat="server" CssClass="linkMenu ddbutton active">*Edit</asp:HyperLink><!--
                    <asp:HyperLink ID="HYPeditWihSimpleEditor" runat="server" CssClass="linkMenu ddbutton">*Edit</asp:HyperLink>
                --></div>
                    <asp:LinkButton ID="LNBsetActive" Visible="false" runat="server" CssClass="linkMenu">*Set active</asp:LinkButton>
                    <asp:LinkButton ID="LNBvirtualDeleteMessage" Visible="false" runat="server" CssClass="linkMenu">*Delete</asp:LinkButton>
                    <div class="ddbuttonlist enabled" id="DVvirtualUndeleteButtons" visible="false" runat="server"><!--
                    --><asp:LinkButton ID="LNBvirtualUndeleteMessage" runat="server" CssClass="linkMenu">*Undelete</asp:LinkButton><!--
                    --><asp:LinkButton ID="LNBvirtualUndeleteAndActivate"  runat="server" CssClass="linkMenu">*Undelete and activate</asp:LinkButton><!--
                --></div>
                    <asp:HyperLink ID="HYPprintNoticeboard" runat="server" Visible="true" CssClass="linkMenu print">*Print</asp:HyperLink>
                    <div class="ddbuttonlist enabled" id="DVaddButtons"  visible="false" runat="server"><!--
                    --><asp:LinkButton ID="LNBaddEmptyMessage" runat="server" CssClass="linkMenu ddbutton">*New empty message</asp:LinkButton><!--
                    --><asp:HyperLink ID="HYPnewWihAdvancedEditor" runat="server" CssClass="linkMenu ddbutton active">*New message</asp:HyperLink><!--
                    <asp:HyperLink ID="HYPnewWihSimpleEditor" runat="server" CssClass="linkMenu ddbutton ">*New html message</asp:HyperLink>
                --></div>
                </div>
                <div class="fieldrow right">
                     <asp:HyperLink ID="HYPbackToPreviousPage" runat="server" Visible="false" CssClass="linkMenu">*Back</asp:HyperLink>
                </div>
            </div>
            <CTRL:Messages ID="CTRLmessages"  runat="server" Visible="false" />
            <div class="noticeboard advanced">
                <CTRL:CTRLrender id="CTRLrender" runat="server" IsForManagement="true" RaiseEvents="true"></CTRL:CTRLrender>
            </div>
        </asp:View>
        <asp:View ID="VIWnoPermissions" runat="server">
            <div class="fieldobject toolbar clearfix">
                <div class="fieldrow left">
                </div>
                <div class="fieldrow right">
                    <asp:HyperLink ID="HYPbackToPreviousPageNoPermissions" runat="server" Visible="false" CssClass="linkMenu">*Back</asp:HyperLink>
                </div>
            </div>
            <div class="noticeboard advanced">
                <asp:Literal ID="LTmessage" runat="server"></asp:Literal>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content> 