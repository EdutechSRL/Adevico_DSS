<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_FoldersBreadCrumb.ascx.vb" Inherits="Comunita_OnLine.UC_FoldersBreadcrumb" %>
<asp:MultiView ID="MLVcontent" runat="server">
    <asp:View ID="VIWbreadCrumb" runat="server">
        <div class="breadcrumbwrapper">
            <div class="breadcrumb<%=ContainerCssClass %>">
                <label for=""></label>
                <span class="item first root<%=ContainerRootCssClass%>">
                    <asp:HyperLink ID="HYProotFolder" runat="server"></asp:HyperLink>
                    <asp:LinkButton ID="LNBrootFolder" runat="server" CommandName="0" Visible="false"></asp:LinkButton>
                    <div class="groupedselector nolabel noactive"></div>
                </span><asp:Repeater ID="RPTfolders" runat="server"><ItemTemplate><span class="separator<%#ItemCssClass(Container.DataItem, True, Container.ItemIndex)%>">\</span><span class="item<%#ItemCssClass(Container.DataItem,false)%>">
                    <asp:hyperlink id="HYPfolder" runat="server"><span class="text"><%#Container.DataItem.DisplayName%></span></asp:hyperlink>
                    <asp:linkbutton id="LNBfolder" runat="server" Visible="false" OnClick="LNBrootFolder_Click"><span class="text"><%#Container.DataItem.DisplayName%></span></asp:linkbutton>
                </span></ItemTemplate></asp:Repeater><asp:Label ID="LBlastSeparator" runat="server" CssClass="separator last">\</asp:Label>
            </div><asp:Literal ID="LTrootCssClass" runat="server" Visible="false">root</asp:Literal><asp:Literal ID="LTsingleCssClass" runat="server" Visible="false">single</asp:Literal><asp:Literal ID="LTrootText" runat="server" Visible="false"><span class="text">{0}</span></asp:Literal>
        </div>
    </asp:View>
    <asp:View ID="VIWempty" runat="server"> </asp:View>
</asp:MultiView>