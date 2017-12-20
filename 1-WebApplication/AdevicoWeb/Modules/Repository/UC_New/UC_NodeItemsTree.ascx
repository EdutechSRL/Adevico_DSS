<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_NodeItemsTree.ascx.vb" Inherits="Comunita_OnLine.UC_NodeItemsTree" %>
<%@ Register TagPrefix="CTRL" TagName="Folder" Src="~/Modules/Repository/UC_New/UC_RepositoryItemNode.ascx" %>
<ul class="nestedtree noselect directories tree root">
    <asp:Repeater ID="RPTchildren" runat="server">
        <ItemTemplate>
            <asp:MultiView ID="MLVnode" runat="server">
                <asp:View ID="VIWtypeOpenItemNode" runat="server">
                     <li class="treenode separator" runat="server" id="LIseparator" visible="false"></li>
                    <asp:Literal ID="LTnodeOpenItemNode" runat="server"><li class="treenode directory {0}" id="{1}"><div class="content"></asp:Literal>
                </asp:View>
                <asp:View ID="VIWtypeItem" runat="server">
                    <CTRL:Folder ID="CTRLnode" runat="server" onSelectedFolder="ItemSelectedFolder" />
                </asp:View>
                <asp:View ID="VIWtypeNoChildren" runat="server">
                    <div class="footer"></div>
                </asp:View>
                <asp:View ID="VIWtypeOpenChildren" runat="server">
                    <div class="footer">
                        <ul class="nestedtree directories">
                </asp:View>
                <asp:View ID="VIWtypeCloseChildren" runat="server">
                        </ul>
                    </div>
                </asp:View>
                    <asp:View ID="VIWtypeCloseNode" runat="server">
                    </div></li>
                </asp:View>
            </asp:MultiView>
        </ItemTemplate>
    </asp:Repeater>
</ul>
<asp:Literal ID="LTtemplateFolderTypelinksCssClass" runat="server" Visible="false">specialfolder links</asp:Literal><asp:Literal ID="LTtemplateFolderTyperecycleBinCssClass" runat="server" Visible="false">specialfolder recyclebin</asp:Literal><asp:Literal ID="LTtemplateFolderTypemodulesCssClass" runat="server" Visible="false">specialfolder modules</asp:Literal><asp:Literal ID="LTtemplateFolderTypemoduleCssClass" runat="server" Visible="false">specialfolder module</asp:Literal><asp:Literal ID="LTtemplateFolderTypetagsCssClass" runat="server" Visible="false">specialfolder tags</asp:Literal><asp:Literal ID="LTtemplateFolderTypetagCssClass" runat="server" Visible="false">specialfolder tag</asp:Literal><asp:Literal ID="LTtreetypeCssClassNoSelect" runat="server" Visible="false">noselect</asp:Literal><asp:Literal ID="LTtreeAutoOpenCssClass" runat="server" Visible="false">autoOpen</asp:Literal><asp:Literal ID="LTtreeKeepAutoOpenCssClass" runat="server" Visible="false">keepOpen</asp:Literal><asp:Literal ID="LTselectedItemCssClass" runat="server" Visible="false">active</asp:Literal><asp:Literal ID="LTemptyItemCssClass" runat="server" Visible="false">empty</asp:Literal>