<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_WorkBookItemFiles.ascx.vb"
    Inherits="Comunita_OnLine.UC_WorkBookItemFiles" %>
<asp:GridView ID="GDVWorkBookItemFiles" runat="server" DataKeyNames="ID" AutoGenerateColumns="False"
     Width="850px">
    <HeaderStyle cssclass="ROW_header_Small_Center" />

    <Columns>
        <asp:BoundField DataField="ID" Visible="false" />
        <asp:TemplateField HeaderText="E" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px">
            <ItemTemplate>
                &nbsp;<asp:LinkButton ID="LNBvirtualDelete" runat="server" CommandName="virtualdelete"
                    CausesValidation="false"></asp:LinkButton>
                <asp:LinkButton ID="LNBundelete" runat="server" CommandName="undelete" CausesValidation="false"></asp:LinkButton>
                <asp:LinkButton ID="LNBunlink" runat="server" CommandName="unlink" CausesValidation="false"></asp:LinkButton>&nbsp;
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px">
            <ItemTemplate>
                <asp:HyperLink ID="HYPpublishItem" runat="server" Target="_self"></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:Label ID="LBnomeFile" runat="server"></asp:Label>
                <asp:HyperLink runat="server" ID="HYPfile" CssClass="ROW_ItemLink_Small" Visible="False"
                    Target="_blank"></asp:HyperLink>
                <asp:Label ID="LBdimensione" runat="server"></asp:Label>&nbsp;
                <asp:HyperLink runat="server" ID="HYPdownload" Target="_blank" CssClass="ROW_ItemLink_Small"
                    Visible="False">Download</asp:HyperLink>
                <asp:ImageButton ID="IMBcontenutoAttivo" runat="server" Visible="false" CausesValidation="false" CommandName="play" />
                <asp:HyperLink runat="server" ID="HYPcontenutoAttivo" CssClass="ROW_ItemLink_Small" Visible="false"></asp:HyperLink>
                 <asp:HyperLink ID="HYPstatistics" runat="server" Target="_self"></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Added At">
            <ItemTemplate>
                <asp:Label ID="LBdata" runat="server" ></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Added By">
            <ItemTemplate>
                <asp:Label ID="LBauthor" runat="server" ></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="C" ItemStyle-HorizontalAlign="Center" Visible="false"
            ItemStyle-Width="50px">
            <ItemTemplate>
                <asp:LinkButton ID="LNBdelete" runat="server" CausesValidation="false" CommandName="confirmDelete"></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
