<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="MultimediaSettings.aspx.vb" Inherits="Comunita_OnLine.MultimediaSettings" %>

<%@ Register TagPrefix="CTRL" TagName="CTRLheader" Src="~/Modules/Repository/UC_New/UC_RepositoryHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <CTRL:CTRLheader runat="server" ID="CTRLheader" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <div class="DivEpButton" id="DVmenu" runat="server" visible="true">
        <asp:HyperLink ID="HYPbackToPreviousUrl" runat="server" CssClass="linkMenu" Visible="false">*Back</asp:HyperLink>
        <asp:Button ID="BTNsaveDefaultDocument" runat="server" CssClass="linkMenu" Text="*Save" Visible="false"></asp:Button>
    </div>
    <CTRL:Messages ID="CTRLmessages" runat="server" Visible="false" />
    <asp:MultiView ID="MLVcontent" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWcontent" runat="server">
            <div class="fieldobject multimediasettings">
                <div class="fieldrow description">
                    <asp:Label ID="LBdescriptionMultimediaSettings"  runat="server" Text="*" CssClass="description"></asp:Label>
                </div>
                <div class="fieldrow currentselected">
                    <asp:Label ID="LBdefaultDocument_t" runat="server" AssociatedControlID="LBdefaultDocument" CssClass="fieldlabel"></asp:Label>
                    <asp:Label ID="LBdefaultDocument" runat="server"></asp:Label>
                    <asp:Label ID="LBdefaultDocumentPath" runat="server" CssClass="documentpath"></asp:Label>
                    <input type="hidden" class="hidselectedfile" runat="server" id="HDNidSelectedFile" />
                </div>
                 <div class="fieldrow selector">
                    <div class="fileselector">
                        <div class="filerepository mainview container_12 clearfix">
                            <div class="content">
                                <div class="section box height treefile grid_12 alpha omega">
                                    <div class="sectioninner">
                                        <div class="treewrapper">
                                            <asp:Literal ID="LTrenderTree" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:View>
        <asp:View ID="VIWempty" runat="server"></asp:View>
    </asp:MultiView><asp:Literal ID="LTtemplateFile" runat="server" Visible="false"><span class="name"><span class="actionbuttons"><span class="#ico#"></span></span><span class="text">#name#</span></span></asp:Literal><asp:Literal ID="LTitemExtensionCssClass" runat="server" Visible="false">fileIco ext</asp:Literal><asp:Literal ID="LTitemFolderCssClass" runat="server" Visible="false">fileIco folder</asp:Literal><asp:Literal ID="LTitemUrlCssClass" runat="server" Visible="false">fileIco extlink</asp:Literal><asp:Literal ID="LTitemScormPackageCssClass" runat="server" Visible="false">fileIco scorm</asp:Literal><asp:Literal ID="LTitemMultimediaCssClass" runat="server" Visible="false">fileIco multimedia</asp:Literal>
<asp:Literal runat="server" ID="LTtreeRoot" Visible="false">
<ul class="nestedtree noselect directories tree root" data-cookie-prefix="multimedia-#uniqueIdVersion#_">
    #nodes#
</ul>
</asp:Literal>
<asp:Literal runat="server" ID="LTtreeFolderNode" Visible="false">
<li class="treenode directory autoOpen" id="folder-#idItem#">
    <div class="content">
        <div class="header">
            <span class="handle folder expander">&nbsp;</span>
            <span class="item">
                <span class="selection"><!--<input type="checkbox" />--></span>
                <span class="text">
                    <a href="#" class="namelink">
                        <span class="fileIco folder"></span>
                        <span class="name">#foldername#</span>
                    </a>
                    <span class="details"></span>
                </span>
            </span>
        </div>
        <div class="footer">
            #childrennodes#
        </div>
    </div>
</li>
</asp:Literal>
<asp:Literal runat="server" ID="LTtreeChildrenNodes" Visible="false">
<ul class="nestedtree directories">
    #childrennodes#
</ul>
</asp:Literal>
<asp:Literal runat="server" ID="LTtreeFileNode" Visible="false">
<li class="treenode file autoOpen" id="file-#idItem#">
    <div class="content">
        <div class="header#active#">
            <span class="handle folder expander">&nbsp;</span>
            <span class="item">
                <span class="selection"></span>
                <span class="text">
                    <a href="#" class="namelink">
                        <span class="iteminfo nodot" style="word-wrap: break-word;">
                            <span class="name">
                                <span class="actionbuttons">
                                    <span class="fileIco ext#ico#">&nbsp;</span>
                                </span>
                                <span class="text">#name#<span class="extension"><span class="dot">.</span>#extension#</span></span>
                            </span>
                            <span class="itemdetail">&nbsp;</span>
                        </span>
                    </a>
                </span>
            </span>
        </div>
        <div class="footer"></div>
    </div>
</li>
</asp:Literal>
<asp:Literal ID="LTcookieTemplate" runat="server" Visible="false">comol_multimedia-#uniqueIdVersion#CollapsableStatus[{0}]</asp:Literal>
</asp:Content>