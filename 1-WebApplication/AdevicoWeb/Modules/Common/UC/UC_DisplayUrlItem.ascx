<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_DisplayUrlItem.ascx.vb" Inherits="Comunita_OnLine.UC_DisplayUrlItem" %>
<asp:MultiView ID="MLVcontrol" runat="server" ActiveViewIndex="0">
    <asp:View ID="VIWempty" runat="server">
        <span class="objectRender <%= ContainerCSS %>">
            <asp:Label ID="LBempty" runat="server"></asp:Label>
        </span>
    </asp:View>
    <asp:View ID="VIWdata" runat="server">
        <span class="objectRender ">
            <asp:Literal ID="LTidentifier" runat="server"></asp:Literal>
            <span class="leftDetail" id="SPNdetail" runat="server">
                <asp:Label ID="LBplace0" runat="server" CssClass="plh plh0" Visible="false"></asp:Label>
                <span class="itemTitle" runat="server" id="SPNitemTitle">
                    <asp:Label ID="LBurl" runat="server" Visible="false"></asp:Label>
                    <asp:HyperLink ID="HYPurl" CssClass="wrapper" runat="server" Target="_blank"></asp:HyperLink>
                </span>
                <asp:Label ID="LBplace1" runat="server" CssClass="plh plh1" Visible="false"></asp:Label>
                <span class="itemDetails" runat="server" id="SPNitemDetails">
                    <asp:Label ID="LBuploadedBy" runat="server" CssClass="itemDetail author"></asp:Label>
                    <asp:Label ID="LBuploadedOn" runat="server" CssClass="itemDetail upload"></asp:Label>
                </span>
                <asp:Label ID="LBplace2" runat="server" CssClass="plh plh2" Visible="false"></asp:Label>
            </span>
            <asp:Label ID="LBplace3" runat="server" CssClass="plh plh3" Visible="false"></asp:Label>
            <asp:Label ID="LBplace4" runat="server" CssClass="plh plh4" Visible="false"></asp:Label>
        </span>
    </asp:View>
</asp:MultiView>
<asp:Literal ID="LTtemplateAuthor" runat="server" Visible="false"><span class="text">{0}</span><span class="name">{1}</span></asp:Literal>
<asp:Literal ID="LTtemplateUploadOn" runat="server" Visible="false"><span class="text">{0}</span><span class="date">{1}</span></asp:Literal>