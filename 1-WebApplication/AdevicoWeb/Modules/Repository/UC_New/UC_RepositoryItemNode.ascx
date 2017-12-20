<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_RepositoryItemNode.ascx.vb" Inherits="Comunita_OnLine.UC_RepositoryItemNode" %>

<div class="header<%=SelectedCssClass()%>">
    <span class="handle folder expander">&nbsp;</span>
    <span class="item">
        <span class="selection">
            <input type="checkbox" runat="server" id="CBselect" />
        </span>
        <span class="text">
            <asp:Literal ID="LTanchor" runat="server"><a name="folder_{0}"></a></asp:Literal><asp:Literal ID="LTfolder" runat="server" Visible="false" ></asp:Literal>
            <asp:Literal ID="LTfoldername" runat="server" Visible="true"><span class="img_span ico_folder_s"></span><span class="name">{0}</span></asp:Literal>
            <asp:HyperLink ID="HYPfolder" runat="server" CssClass="namelink"  Visible="false">
                <span class="fileIco folder"></span>
                <span class="name">{0}</span></asp:HyperLink>
            <asp:LinkButton ID="LNBfolder" runat="server" CssClass="namelink"  Visible="false">
                <span class="fileIco folder"></span>
                <span class="name">{0}</span></asp:LinkButton>
            <span class="details">
                <asp:Literal ID="LTdetails" runat="server"></asp:Literal>
            </span>
        </span>
    </span>
</div><asp:Literal ID="LTselectedItemCssClass" runat="server" Visible="false">active</asp:Literal>