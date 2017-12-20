<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="Edit.aspx.vb" Inherits="Comunita_OnLine.RepositoryItemEdit" MaintainScrollPositionOnPostback="true"  %>
<%@ Register TagPrefix="CTRL" TagName="CTRLheader" Src="~/Modules/Repository/UC_New/UC_RepositoryHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLdetails" Src="~/Modules/Repository/UC_New/UC_ItemDetails.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLscorm" Src="~/Modules/Repository/UC_New/UC_ItemScormDetails.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLmultimedia" Src="~/Modules/Repository/UC_New/UC_ItemMultimediaDetails.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLversions" Src="~/Modules/Repository/UC_New/UC_ItemVersions.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLpermissions" Src="~/Modules/Repository/UC_New/UC_ItemPermissions.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="ModalPlayerHeader" Src="~/Modules/Repository/UC_New/UC_ModalPlayerHeader.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <CTRL:CTRLheader runat="server" ID="CTRLheader"/>
    <!-- MODAL SCRIPT START -->
    <CTRL:ModalPlayerHeader ID="CTRLmodalHeader" runat="server" />
    <!-- MODAL SCRIPT END -->
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <div class="DivEpButton" id="DVmenu" runat="server"  Visible="true">
        <asp:HyperLink ID="HYPbackToPreviousUrl" runat="server" CssClass="linkMenu" Visible="false">*Back</asp:HyperLink>
        <asp:HyperLink ID="HYPgotoViewDetailsPage" runat="server" CssClass="linkMenu" Visible="false">*View Details</asp:HyperLink>
        <asp:Button ID="BTNhideItem" runat="server" CssClass="linkMenu" Text="*Hide" Visible="false"></asp:Button>
        <asp:Button ID="BTNshowItem" runat="server" CssClass="linkMenu" Text="*Show" Visible="false"></asp:Button>
        <asp:Button ID="BTNsaveItemDetails" runat="server" CssClass="linkMenu" Text="*Save" Visible="false"></asp:Button>
    </div>
    <CTRL:Messages ID="CTRLmessages" runat="server" Visible="false" />
    <asp:MultiView ID="MLVcontent" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWcontent" runat="server">
            <div class="filerepository fileinfo container_12 clearfix">
                <div class="section title grid_12">
                    <h1>
                        <asp:Literal ID="LTfilename" runat="server"></asp:Literal>
                    </h1>                        
                </div>
               <CTRL:CTRLdetails id="CTRLitemDetails" runat="server"></CTRL:CTRLdetails>
               <CTRL:CTRLpermissions id="CTRLitemPermissions" runat="server" Visible="false"></CTRL:CTRLpermissions>
               <CTRL:CTRLversions id="CTRLitemVersions" runat="server" Visible="false"></CTRL:CTRLversions>
               <CTRL:CTRLscorm id="CTRLitemScorm" runat="server" Visible="false"></CTRL:CTRLscorm>
               <CTRL:CTRLmultimedia id="CTRLitemMultimedia" runat="server" Visible="false"></CTRL:CTRLmultimedia>
            </div>
        </asp:View>
        <asp:View ID="VIWempty" runat="server"></asp:View>
    </asp:MultiView><asp:Literal ID="LTtemplateFile" runat="server"  Visible="false"><span class="iteminfo"><span class="name"><span class="actionbuttons"><span class="#ico#"></span></span><span class="text">#name#</span></span></span></asp:Literal><asp:Literal ID="LTitemExtensionCssClass" runat="server" Visible="false">fileIco ext</asp:Literal><asp:Literal ID="LTitemFolderCssClass" runat="server" Visible="false">fileIco folder</asp:Literal><asp:Literal ID="LTitemUrlCssClass" runat="server" Visible="false">fileIco extlink</asp:Literal><asp:Literal ID="LTitemScormPackageCssClass" runat="server" Visible="false">fileIco scorm</asp:Literal><asp:Literal ID="LTitemMultimediaCssClass" runat="server" Visible="false">fileIco multimedia</asp:Literal>

</asp:Content>