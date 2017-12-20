<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_File_StatPers.ascx.vb" Inherits="Comunita_OnLine.UC_File_StatPers" %>
<div>
    <br />
     <asp:Label ID="LBL_UserName" runat="server" cssclass="Titolo_campo">##NomeUtente</asp:Label>
     <br /><br />
     <asp:Label id="LBL_NumPlay_t" runat="server" cssclass="Titolo_campo">Numero totale di play:</asp:Label>
    &nbsp;
    <asp:Label id="LBL_NumPlay" runat="server" cssclass="Testo_campo">###</asp:Label>
    <br /><br />
</div>
<div>
        <asp:DataGrid ID="Dg_StatUserFile" runat="server" 
            AutoGenerateColumns="false" ShowFooter="false"
            ShowHeader="true"
            CssClass="DataGrid_Generica">
        <AlternatingItemStyle CssClass="ROW_Alternate_Small"></AlternatingItemStyle>
		<HeaderStyle CssClass="ROW_header_Small_Center"></HeaderStyle>
		<ItemStyle CssClass="ROW_Normal_Small" Height="22px"></ItemStyle>
        <Columns>
            <asp:TemplateColumn>
                <HeaderTemplate>
                    <asp:Label ID="Lbl_FileName_Dg" runat="server">Nome File</asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="Lbl_FileName" runat="server">#########.####</asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderTemplate>
                    <asp:Label ID="Lbl_Scaricamenti_Dg" runat="server">Scaricamenti</asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="Lbl_Scaricamenti" runat="server">##:##.##</asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderTemplate>
                    <asp:Label ID="Lbl_LastDownload_Dg" runat="server">Ultimo Download</asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="Lbl_LastDownload" runat="server">##:##.##</asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid> 
</div>