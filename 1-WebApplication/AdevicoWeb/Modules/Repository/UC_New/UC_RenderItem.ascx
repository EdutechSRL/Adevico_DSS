<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_RenderItem.ascx.vb" Inherits="Comunita_OnLine.UC_RenderItem" %>
<%@ Register TagPrefix="CTRL" TagName="Menu" Src="~/Modules/Repository/UC_New/UC_ItemMenu.ascx" %>

<span class="iteminfo">
    <asp:Literal ID="LTanchor" runat="server"><a name="contentitem_{0}"></a></asp:Literal>
<asp:HyperLink ID="HYPitemUrl" runat="server">
    <span class="name">
        <span class="actionbuttons">
            <span class="#ico#"></span>
        </span>
        <span class="text">#name#</span>
    </span>
</asp:HyperLink>
    <asp:linkbutton id="LNBitemUrl" runat="server" Visible="false">
         <span class="name">
            <span class="actionbuttons">
                <span class="#ico#"></span>
            </span>
            <span class="text">#name#</span>
        </span>
    </asp:linkbutton>
    <asp:Literal ID="LTfileName" runat="server" Visible="false"></asp:Literal>
    <asp:Label ID="LBfileSize" runat="server" CssClass="itemdetail" Visible="false"></asp:Label>
</span>
<CTRL:Menu ID="CTRLmenu" runat="server" Visible="false" />
<div class="extrainfowrapper">
    <div class="extrainfo path" id="DVpath" runat="server">
        <span class="text"><asp:Literal ID="LTitemPathTitle" runat="server" Text="*Original path"></asp:Literal>&nbsp;<asp:Literal ID="LTitemPath" runat="server" Text="*Original path"></asp:Literal></span>
    </div>
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
    <div class="extrainfo author">
        <asp:Label ID="LBauthor" runat="server" CssClass="text"></asp:Label>
    </div>
</div><asp:Literal ID="LTitemExtensionCssClass" runat="server" Visible="false">fileIco ext</asp:Literal><asp:Literal ID="LTitemFolderCssClass" runat="server" Visible="false">fileIco folder</asp:Literal><asp:Literal ID="LTitemUrlCssClass" runat="server" Visible="false">fileIco extlink</asp:Literal><asp:Literal ID="LTitemScormPackageCssClass" runat="server" Visible="false">fileIco scorm</asp:Literal><asp:Literal ID="LTitemMultimediaCssClass" runat="server" Visible="false">fileIco multimedia</asp:Literal><asp:Literal ID="LTpreviewImages" runat="server" Visible="false">rel:inline-preview|class:lightbox image</asp:Literal><asp:Literal ID="LTmodalCssClass" runat="server" Visible="false">openmodal</asp:Literal>