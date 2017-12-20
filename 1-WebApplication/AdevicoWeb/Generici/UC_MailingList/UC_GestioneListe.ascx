<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_GestioneListe.ascx.vb" Inherits="Comunita_OnLine.UC_GestioneListe" %>
    
<asp:Panel ID="PNLContenitore" runat="server" HorizontalAlign="Center" >
    <div>
        <br />
        <asp:Label ID="LBL_NewLista_t" runat="server" CssClass="Titolo_campo">Nuova lista:</asp:Label>
        <asp:TextBox ID="TXB_NewListaName" runat="server" MaxLength="30" Width="360px" CssClass="Testo_campo">
        </asp:TextBox>
        <asp:Button ID="BTN_AddNew" runat="server" CssClass="PulsanteFiltro" Text="Inserisci" />
        <br /> <br />
    </div>
    <div class="DivGridview">
        <asp:GridView ID="GRVListe" runat="server" 
            DataKeyNames="Id"  align="center"
            AutoGenerateColumns="False"
            AllowPaging="True" AllowSorting="True" 
	        Width="650px" Font-Size="11px" PageSize="20" 
	        PagerSettings-Position="TopAndBottom" PagerSettings-Mode="Numeric"
	        CssClass="DataGrid_Generica"> 
            <HeaderStyle CssClass="ROW_header_Small_Center"></HeaderStyle>
            <AlternatingRowStyle CssClass="ROW_Alternate_Small"></AlternatingRowStyle>
			<RowStyle CssClass="ROW_Normal_Small" Height="22px"></RowStyle>
			<PagerStyle CssClass="ROW_Page_Small" HorizontalAlign="Right" height="18px" VerticalAlign="Bottom" ></PagerStyle>
	        
            <Columns>
                <asp:CommandField ButtonType="Link" ShowEditButton="True"  EditImageUrl="~/images/DG/m.gif" ItemStyle-Width="18" HeaderText="M" />
		        <asp:CommandField ButtonType="Link" ShowDeleteButton="True" DeleteImageUrl="~/images/DG/x.gif" ItemStyle-Width="18" HeaderText="C" />
		        <asp:BoundField DataField="Nome" HeaderText="Nome lista" SortExpression="Nome" ItemStyle-Width="550">
		            <ItemStyle Font-Size="11px" />
                </asp:BoundField>
                <asp:TemplateField ItemStyle-CssClass="ROW_TD_Small" HeaderText="Seleziona" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:CheckBox ID="CBX_Selected" runat="server"/>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <br />
        <table align="right">
            <tr><td>
                <asp:Button ID="BTN_Inserisci" runat="server" Text="Add Selezionati" CssClass="pulsante" /> 
            </td></tr>
        </table>
    </div>
    
    <div>
        <br /><br /><br /><br />
        <asp:Label ID="LBLMessage" runat="server" Visible="false" CssClass="messaggio"></asp:Label>
        <br /><br />
    </div>
</asp:Panel>