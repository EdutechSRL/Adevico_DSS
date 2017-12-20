<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_Interni.ascx.vb" Inherits="Comunita_OnLine.UC_Interni" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLsorgenteComunita" Src="../../UC/UC_FiltroComunitaByServizio_NEW.ascx" %>

<asp:Panel ID="Ricerca" runat="server">

    <table width="850">
        <tr>
            <td colspan="6">
                <asp:Label ID="LBL_R_Cerca_t" runat="server" CssClass="Titolo_campo">Cerca nominativo</asp:Label>    
            </td>
        </tr><tr>
            <td align="right">
                <asp:Label ID="LBL_R_Nome_t" runat="server" CssClass="Titolo_campoSmall">Nome: </asp:Label>
            </td><td>
                <asp:TextBox ID="TXB_R_Nome" runat="server" CssClass="FiltroCampoSmall"></asp:TextBox>
            </td><td align="right">
                <asp:Label ID="LBL_R_Cognome_t" runat="server" CssClass="Titolo_campoSmall">Cognome: </asp:Label>
            </td><td>
                <asp:TextBox ID="TXB_R_Cognome" runat="server" CssClass="FiltroCampoSmall"></asp:TextBox>
            </td><td align="right">
                <asp:Label ID="LBL_R_Matricola_t" runat="server" CssClass="Titolo_campoSmall">Matricola: </asp:Label>
            </td><td>
                <asp:TextBox ID="TXB_R_Matricola" runat="server" CssClass="FiltroCampoSmall"></asp:TextBox>
            </td>
        </tr><tr>
            <td align="right">
                <asp:Label ID="LBL_R_Mail_t" runat="server" CssClass="Titolo_campoSmall">Mail: </asp:Label>
            </td><td>
                <asp:TextBox ID="TXB_R_Mail" runat="server" CssClass="FiltroCampoSmall"></asp:TextBox>
            </td><td align="right">
                <asp:Label ID="LBL_R_Login_t" runat="server" CssClass="Titolo_campoSmall">Login: </asp:Label>
            </td><td>
                <asp:TextBox ID="TXB_R_Login" runat="server" CssClass="FiltroCampoSmall"></asp:TextBox>
            </td><td align="right">
                <asp:Label ID="LBL_R_Tipo_t" runat="server" CssClass="Titolo_campoSmall">Tipo: </asp:Label>
            </td><td>
                <asp:DropDownList id="DDLtipoPersona" Runat="server" CssClass="FiltroCampoSmall"></asp:DropDownList>
            </td>
        </tr>
    </table>
    <table width="850">
        <tr>
            <td valign="top" width="100">
                <asp:CheckBox ID="CBX_R_Comunita" runat="server" Text="Comunita" AutoPostBack="true" CssClass="Titolo_campoSmall" />
            </td><td width="700">
                <asp:Panel ID="PNL_R_Comunita" runat="server">
                    <CTRL:CTRLsorgenteComunita id="CTRLsorgenteComunita" runat="server" Width="650px" LarghezzaFinestraAlbero="650px" ColonneNome="100"></CTRL:CTRLsorgenteComunita>
                </asp:Panel>
            </td><td valign="bottom">
                <asp:Button ID="BTN_R_Submit" runat="server" Text="Cerca" CssClass="PulsanteFiltro" />
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="PNLContenuto" runat="server">
        <div style="position: static; display: inline; text-align: right; width:850px;">
            <asp:Label ID="LBL_RecPerPage_t" runat="server" CssClass="Filtro_RecordPaginazione">Num. rec.</asp:Label>
		    <asp:DropDownList ID="DDLNumRec" runat="server" AutoPostBack="True" CssClass="Filtro_RecordPaginazione">
		        <asp:ListItem Text="15" Value="15"></asp:ListItem>
			    <asp:ListItem Text="25" Value="25" Selected="True"></asp:ListItem>
			    <asp:ListItem Text="50" Value="50"></asp:ListItem>
			    <asp:ListItem Text="100" Value="100"></asp:ListItem>
		    </asp:DropDownList>
		    &nbsp;
        </div>
<asp:panel id="PNLpersona" Runat="server">
    <asp:GridView 
        ID="GRVInterni" 
        runat="server" 
        AllowPaging="True"  
        AllowSorting="true" 
        ShowHeader="true"
        AutoGenerateColumns="False"
        DataKeyNames="Id" 
        CellPadding="2"
        CssClass="DataGrid_Generica"> 
		
        <AlternatingRowStyle Height="22px" CssClass="ROW_Alternate_Small"/>
        <HeaderStyle Font-Size="11px" BackColor="Navy" ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
        <RowStyle CssClass="ROW_Normal_Small" Height="22px"/>
        <PagerStyle CssClass="ROW_Page_Small" HorizontalAlign="Right" height="18px" VerticalAlign="Bottom" ></PagerStyle>
        <PagerSettings Position="TopAndBottom" />
    	
        <Columns>
            <asp:BoundField ItemStyle-CssClass="ROW_TD_Small" HeaderText="Cognome" SortExpression="Cognome" DataField="Cognome" />
            <asp:BoundField ItemStyle-CssClass="ROW_TD_Small" HeaderText="Nome" SortExpression="Nome" DataField="Nome" />
            <asp:BoundField ItemStyle-CssClass="ROW_TD_Small" HeaderText="Mail" SortExpression="Mail" DataField="Mail" />
            <asp:TemplateField ItemStyle-CssClass="ROW_TD_Small" ItemStyle-HorizontalAlign="Center" HeaderText="Seleziona" HeaderStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:CheckBox ID="CBX_Selected" runat="server"/>
                </ItemTemplate>
             </asp:TemplateField>
		</Columns>
    </asp:GridView>
</asp:panel>

<table width="850" align="right">
	<tr>
	    <td align="center">
	        <asp:Label id="LBnorecord" Runat="server" CssClass="NoRecord">No record.</asp:Label>
	    </td>
	</tr><tr>
	    <td align="center">
	        <asp:Label ID="LBL_Message" Visible="false" runat="server"></asp:Label>
	    </td>
	</tr><tr>
        <td align="right">
            <asp:Button id="BTN_AnnullaInt" Runat="server" Text="Annulla" CssClass="PulsanteFiltro"></asp:Button>
	        <asp:Button id="BTN_ConfermaSel" Runat="server" Text="Conferma" CssClass="PulsanteFiltro"></asp:Button>
        </td>
    </tr>
</table>
<input type="hidden" id="HDN_ComunitaPadreID" runat="server" name="HDN_ComunitaPadreID"/></asp:Panel>