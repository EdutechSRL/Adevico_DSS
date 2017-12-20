<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ModuleRenderAction.ascx.vb" Inherits="Comunita_OnLine.UC_RepositoryRenderAction" %>
<%@ Register TagPrefix="CTRL" TagName="Menu" Src="~/Modules/Repository/UC_New/UC_ItemMenu.ascx" %>
<span class="filecontainer<%= CssClass %><%=CssAvailability%>">
<span class="iteminfo">
    <asp:Literal ID="LTanchor" runat="server"><a name="contentitem_{0}"></a></asp:Literal>
    <asp:Label ID="LBplace0" runat="server" CssClass="plh plh0" Visible="false"></asp:Label>
<asp:HyperLink ID="HYPitemUrl" runat="server"><span class="name"><span class="actionbuttons"><span class="#ico#"></span></span><span class="text">#name#</span></span></asp:HyperLink>
<asp:linkbutton id="LNBitemUrl" runat="server" Visible="false"><span class="name"><span class="actionbuttons"><span class="#ico#"></span></span><span class="text">#name#</span></span></asp:linkbutton>
    <asp:Literal ID="LTfileName" runat="server" Visible="false"></asp:Literal>
    <asp:Label ID="LBplace1" runat="server" CssClass="plh plh1" Visible="false"></asp:Label>
    <asp:Label ID="LBfileSize" runat="server" CssClass="itemdetail" Visible="false"></asp:Label>
    <asp:Label ID="LBplace2" runat="server" CssClass="plh plh2" Visible="false"></asp:Label>
</span>
<asp:Label ID="LBplace3" runat="server" CssClass="plh plh3" Visible="false"></asp:Label>
<CTRL:Menu ID="CTRLmenu" runat="server" Visible="false" />
<asp:Repeater ID="RPTactions" runat="server" Visible="false"><HeaderTemplate><span class="itemActions"></HeaderTemplate><ItemTemplate><span class="action"><asp:HyperLink ID="HYPaction" runat="server">&nbsp;</asp:HyperLink></span></ItemTemplate><FooterTemplate></span></FooterTemplate></asp:Repeater>
<asp:Label ID="LBplace4" runat="server" CssClass="plh plh4" Visible="false"></asp:Label>
<div class="extrainfowrapper" id="DVextraInfo" runat="server" visible="false">
    <div class="extrainfo description" id="DVdescription" runat="server">
        <div class="description">
            <asp:Literal ID="LTdescription" runat="server"></asp:Literal>
        </div>
    </div>
    <div class="extrainfo" id="DVtag" runat="server" visible="false">
        <asp:Label ID="LBitemTagsTitle" CssClass="label" runat="server">*Tags:</asp:Label>
        <span class="tags">
            <asp:Label ID="LBtagItemType" runat="server" CssClass="tag type" Visible="false"></asp:Label>
        <asp:Repeater ID="RPTtags" runat="server">
            <ItemTemplate>
                <span class="tag"><%#Container.DataItem %></span>
            </ItemTemplate>
            <SeparatorTemplate>
                <span class="sep">|</span>
            </SeparatorTemplate>
        </asp:Repeater>
        </span>
    </div>
    <div class="extrainfo author" id="DVauthor" runat="server" visible="false">
        <asp:Label ID="LBauthor" runat="server" CssClass="text"></asp:Label>
    </div>
    <div class="extrainfo linkedby" id="DVlinkedBy" runat="server" visible="false">
        <asp:Label ID="LBlinkedBy" runat="server" CssClass="text"></asp:Label>
    </div>
</div>
<asp:Label ID="LBplace5" runat="server" CssClass="plh plh5" Visible="false"></asp:Label>
</span>
<asp:Literal ID="LTitemExtensionCssClass" runat="server" Visible="false">fileIco ext</asp:Literal><asp:Literal ID="LTitemFolderCssClass" runat="server" Visible="false">fileIco folder</asp:Literal><asp:Literal ID="LTitemUrlCssClass" runat="server" Visible="false">fileIco extlink</asp:Literal><asp:Literal ID="LTitemScormPackageCssClass" runat="server" Visible="false">fileIco scorm</asp:Literal><asp:Literal ID="LTitemMultimediaCssClass" runat="server" Visible="false">fileIco multimedia</asp:Literal><asp:Literal ID="LTpreviewImages" runat="server" Visible="false">rel:inline-preview|class:lightbox image</asp:Literal><asp:Literal ID="LTmodalCssClass" runat="server" Visible="false">openmodal</asp:Literal>