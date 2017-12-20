<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_SkinImages.ascx.vb" Inherits="Comunita_OnLine.UC_SkinImages" %>
<asp:Panel ID="DVaddImage" runat="server">
    <div>
        <span class="LoadImage">
            <asp:Label ID="LBaddNewImage_t" runat="server">Select file:</asp:Label>
            <asp:FileUpload ID="FUPimageFile" runat="server" />
            <asp:Button id="BTNaddImageFile" runat="server" CssClass="Menu_Link"/>
        </span>
        <span style="display:none;">
            <asp:Label ID="LBaddZip_t" runat="server">Select zip:</asp:Label>
            <asp:FileUpload ID="FUPzipFile" runat="server" />
            <asp:Button id="BTNaddZipFile" runat="server" CssClass="Menu_Link"/>
        </span>
    </div>
</asp:Panel>
<asp:Repeater ID="RPTimages" runat="server">
    <HeaderTemplate>
        <table border="1" cellpadding="0" cellspacing="0" class="SkinTable">
            <thead>
                <tr class="ROW_header_Small">
                    <th>&nbsp;</th>
                    <th>
                        <asp:Label ID="LBimageName_t" runat="server"></asp:Label>
                    </th>
                    <th>
                        <asp:Label ID="LBimageSize_t" runat="server"></asp:Label>
                    </th>
                    <th>
                        <asp:Label ID="LBimageDisplay_t" runat="server"></asp:Label>
                    </th>
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td>
                <asp:LinkButton ID="LNBremoveImage" runat="server" CssClass="Delete">x</asp:LinkButton>
            </td>
            <td>
                <asp:Label ID="LBimageName" runat="server"></asp:Label>
            </td>
            <td>
                <asp:Label ID="LBimageSize" runat="server"></asp:Label>
            </td>
            <td>
                <asp:Image ID="IMGpreviewImage" runat="server" />
            </td>
        </tr>    
    </ItemTemplate>
    <FooterTemplate>
        </tbody>
    </table>
    </FooterTemplate>
</asp:Repeater>
<asp:Label ID="LBnoImages" runat="server" CssClass="Titolo_Campo NoImages"></asp:Label>