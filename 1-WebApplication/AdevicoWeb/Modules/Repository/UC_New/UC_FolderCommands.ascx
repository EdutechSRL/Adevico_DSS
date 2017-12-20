<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_FolderCommands.ascx.vb" Inherits="Comunita_OnLine.UC_FolderCommands" %>
<!--FOLDER COMMANDS-->
<div class="toolbar grouped">
    <asp:MultiView ID="MLVcommands" runat="server">
        <asp:View ID="VIWempty" runat="server"></asp:View>
        <asp:View ID="VIWcommands" runat="server">
            <span class="icons first" runat="server" id="SPNfolder">
                <asp:Label ID="LBcommandFolder_t" runat="server" CssClass="title">*Folder:</asp:Label>
                <asp:Linkbutton id="LNBgotofolderfather" runat="server" Visible="false"><span class="icon parentfolder"></span></asp:Linkbutton>
                <asp:hyperlink runat="server" ID="HYPgotofolderfather" ToolTip="*Upper folder"><span class="icon parentfolder"></span></asp:hyperlink>
                <asp:hyperlink runat="server" ID="HYPaddFolder" ToolTip="*Add folder"><span class="icon newfolder"></span></asp:hyperlink>
            </span>
            <span class="icons" runat="server" id="SPNresources">
                <asp:Label ID="LBcommandResources_t" runat="server" CssClass="title">*Resources:</asp:Label>
                <asp:hyperlink runat="server" ID="HYPupload" ToolTip="*Add file"><span class="icon fileupload"></span></asp:hyperlink>
                <asp:hyperlink runat="server" ID="HYPaddlink" ToolTip="*Add links"><span class="icon newlink"></span></asp:hyperlink>
             <%--   <span class="icon share"></span>
                <span class="icon infoxs permission"></span>--%>
            </span>
            <span class="icons last selecteditems" runat="server" id="SPNselected">
                <asp:Label ID="LBcommandSelectedResources_t" runat="server" CssClass="title">*Selected resources:</asp:Label>
                <asp:LinkButton ID="LNBhideItems" runat="server" ToolTip="*Hide items" CssClass="aicon"><span class="icon showselected"></span></asp:LinkButton>
                <asp:LinkButton ID="LNBshowItems" runat="server" ToolTip="*Show items" CssClass="aicon"><span class="icon hideselected"></span></asp:LinkButton>
                <asp:LinkButton ID="LNBvirtualDeleteItems" runat="server" ToolTip="*Virtual delete items" CssClass="aicon"><span class="icon virtualdelete"></span></asp:LinkButton>
                <asp:LinkButton ID="LNBvirtualUndeleteItems" runat="server" ToolTip="*Undelete items" CssClass="aicon"><span class="icon recover"></span></asp:LinkButton>
                <asp:LinkButton ID="LNBphisicalDeleteItems" runat="server" ToolTip="*Phisical delete items" CssClass="aicon"><span class="icon delete"></span></asp:LinkButton>
            </span>
        </asp:View>
    </asp:MultiView>
</div>
<!--FOLDER COMMANDS-->