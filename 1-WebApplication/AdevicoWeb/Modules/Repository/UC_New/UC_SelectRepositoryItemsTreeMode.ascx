<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_SelectRepositoryItemsTreeMode.ascx.vb" Inherits="Comunita_OnLine.UC_SelectRepositoryItemsTreeMode" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<div class="fieldobject">
    <div class="fieldrow selector">
        <div class="fileselector">
            <div class="filerepository mainview container_12 clearfix">
                <div class="content">
                    <div class="section box height treefile grid_12 alpha omega">
                        <div class="sectioninner">
                            <div class="treewrapper">
                                <telerik:RadTreeView ID="RDTtree" runat="server" CssClass="nestedtree directories tree root"  >

                                </telerik:RadTreeView>
                                <asp:Literal ID="LTrenderTree" runat="server" Visible="false"></asp:Literal>
                                <CTRL:Messages ID="CTRLmessages" runat="server" Visible="false" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<asp:Literal ID="LTtreeMode" runat="server" Visible="false">noselect|singleselect|select|treeselect|treeselect</asp:Literal>
<asp:Literal ID="LTreadonlyMode" runat="server" Visible="false">readonly disabled</asp:Literal>
<asp:Literal runat="server" ID="LTtreeRoot" Visible="false">
<ul class="nestedtree #selectmode# directories tree root" data-cookie-prefix="treeselect-#uniqueIdVersion#">
    <li class="treenode directory autoOpen" id="folder-0">
        <div class="content">
            <div class="header">
                <span class="handle folder expander">&nbsp;</span>
                <span class="item">
                    <span class="selection">#select#</span>
                    <span class="text">
                        <a href="#" class="namelink">
                            <span class="fileIco folder"></span>
                            <span class="name">#name#</span>
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
</ul>
</asp:Literal>
<asp:Literal runat="server" ID="LTtreeFolderNode" Visible="false">
 <li class="treenode directory autoOpen" id="folder-#dataid#">
    <div class="content">
        <div class="header">
            <span class="handle folder expander">&nbsp;</span>
            <span class="item">
                <span class="selection">#select#</span>
                <span class="text">
                    <a href="#" class="namelink">
                        <span class="fileIco folder"></span>
                        <span class="name">#name#</span>
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
<asp:Literal runat="server" Visible="false" ID="LTtreeFileNode">
<li class="treenode file autoOpen" id="file-#dataid#">
    <div class="content">
        <div class="header">
            <span class="handle folder expander">&nbsp;</span>
            <span class="item">
                <span class="selection">#select#</span>
                <span class="text">
                    <a href="#" class="namelink">
                        <span class="iteminfo notdot">
                            #name#
                            <span class="itemdetail">#filesize#</span>
                        </span>
                    </a>
                    <span class="details"></span>
                </span>
            </span>
        </div>
        <div class="footer"></div>
    </div>
</li>
</asp:Literal>
<asp:Literal ID="LTmultiselectItem" runat="server" Visible="false"><input type="checkbox" name="CBitem_#idItem#" #enabled# #checked#/><input type="hidden" name="HDNitem_#idItem#" value="#idItem#" /></asp:Literal>
<asp:Repeater ID="RPTitems" runat="server" Visible="false">
    <ItemTemplate>
        <asp:Literal ID="LTpath" runat="server" Text='<%#Container.DataItem.Path%>'></asp:Literal>
        <asp:Literal ID="LTitem" runat="server" Visible="false" Text='<%#Container.DataItem.Id & "|" & Container.DataItem.Type.ToString() & "|" & Container.DataItem.UniqueId.ToString & "|" & Container.DataItem.IdVersion & "|" & Container.DataItem.UniqueIdVersion.ToString & "|" & Container.DataItem.IgnoreVersion.ToString%>'></asp:Literal>
    </ItemTemplate>
</asp:Repeater>
<asp:Literal ID="LTtemplateItemFolder" runat="server"  Visible="false"><span class="text"><span class="iteminfo notdot"><span class="text">#name#</span></span></span></asp:Literal>
<asp:Literal ID="LTtemplateItemFile" runat="server"  Visible="false"><span class="text"><span class="iteminfo notdot">#name#<span class="itemdetail">#filesize#</span></span></span></asp:Literal>
<asp:Literal ID="LTtemplateFile" runat="server"  Visible="false"><span class="name"><span class="actionbuttons"><span class="#ico#"></span></span><span class="text">#name#</span></span></asp:Literal><asp:Literal ID="LTitemExtensionCssClass" runat="server" Visible="false">fileIco ext</asp:Literal><asp:Literal ID="LTitemFolderCssClass" runat="server" Visible="false">fileIco folder</asp:Literal><asp:Literal ID="LTitemUrlCssClass" runat="server" Visible="false">fileIco extlink</asp:Literal><asp:Literal ID="LTitemScormPackageCssClass" runat="server" Visible="false">fileIco scorm</asp:Literal><asp:Literal ID="LTitemMultimediaCssClass" runat="server" Visible="false">fileIco multimedia</asp:Literal>