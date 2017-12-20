<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ModuleToRepositoryDisplay.ascx.vb"
    Inherits="Comunita_OnLine.UC_ModuleToRepositoryDisplay" %>
 
 <asp:MultiView ID="MLVdisplayMode" runat="server" ActiveViewIndex="0">
    <asp:View ID="VIWinline" runat="server">
        <asp:Literal ID="LTitem" runat="server"></asp:Literal>
    </asp:View>
    <asp:View ID="VIWtable" runat="server">
       <%-- <td>--%>
            <asp:HyperLink ID="HYPeditPermission" runat="server" Target="_self" CssClass="ROW_ItemLink_Small" Visible="false"></asp:HyperLink>
       <%-- </td>--%>
        <td>
            <asp:Literal ID="LTnoFile" runat="server"></asp:Literal>
            <asp:Literal ID="LTitemAction" runat="server"></asp:Literal>
            <asp:HyperLink runat="server" ID="HYPitemAction" CssClass="ROW_ItemLink_Small " Target="_blank" Visible="false"></asp:HyperLink>
          <%--  <div>
                <span><asp:Label ID="LBdimensione" runat="server"></asp:Label>&nbsp;</span>
                <span><asp:Label ID="LBscaricamenti" runat="server"></asp:Label>&nbsp;</span>
            </div>--%>
        </td>
        <td>
            &nbsp;<asp:HyperLink ID="HYPdownloadItem" runat="server"  Target="_blank" CssClass="ROW_ItemLink_Small img_link" Visible="False">&nbsp;</asp:HyperLink>&nbsp;
        </td>
        <td>
            &nbsp;<asp:HyperLink ID="HYPstatistics" runat="server" Target="_self" CssClass="ROW_ItemLink_Small img_link" Visible="False">&nbsp;</asp:HyperLink>&nbsp;
        </td>
        <td>
            &nbsp;<asp:HyperLink ID="HYPsettings" runat="server" Target="_self" CssClass="ROW_ItemLink_Small img_link" Visible="False">&nbsp;</asp:HyperLink>&nbsp;
        </td>
    </asp:View>
</asp:MultiView>