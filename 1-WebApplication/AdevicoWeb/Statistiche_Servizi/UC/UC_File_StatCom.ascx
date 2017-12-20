<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_File_StatCom.ascx.vb" Inherits="Comunita_OnLine.UC_File_StatCom" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="../../UC/UC_PagerControl.ascx" %>

<asp:MultiView ID="MLV_StatFileCom" runat="server" ActiveViewIndex="0">
    <asp:View ID="V_ElencoUtenti" runat="server">
        Elenco Utenti
        
        <asp:Table id="TBLfiltroNew" Runat="server" Width="900px" CellPadding="0" CellSpacing="0">
			<asp:TableRow id="TBRchiudiFiltro" Height="22px">
				<asp:TableCell CssClass="Filtro_CellSelezionato" HorizontalAlign="Center" Width="150px" Height="22px" VerticalAlign="Middle">
					<asp:LinkButton ID="LNBchiudiFiltro" Runat="server" CssClass="Filtro_Link" CausesValidation="false">Chiudi Filtri</asp:LinkButton>
				</asp:TableCell>
				<asp:TableCell CssClass="Filtro_CellDeSelezionato" Width="750px" Height="22px">
				    &nbsp;
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow id="TBRapriFiltro" Visible="False" Height="22px">
				<asp:TableCell ColumnSpan="1" CssClass="Filtro_CellApriFiltro" HorizontalAlign="Center" Width="150px" Height="22px">
					<asp:LinkButton ID="LNBapriFiltro" Runat="server" CssClass="Filtro_Link" CausesValidation="False">Apri Filtri</asp:LinkButton>
				</asp:TableCell>
				<asp:TableCell CssClass="Filtro_Cellnull" Width="700px" Height="22px">&nbsp;
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="TBRfiltri">
				<asp:TableCell CssClass="Filtro_CellFiltri" ColumnSpan="2" Width="900px" HorizontalAlign="center" Height="20px">
					<asp:Table id="TBLfiltro" Runat="server" HorizontalAlign="left" Width="800px" GridLines="None">
						<asp:TableRow>
							<asp:TableCell Width="90px">
								<asp:Label ID="LBtipoRuolo_t" runat="server" CssClass="FiltroVoceSmall" Visible="false">Tipo Ruolo:</asp:Label>&nbsp;&nbsp;
							</asp:TableCell>
							<asp:TableCell>
								<asp:DropDownList id="DDLtipoRuolo" Runat="server" CssClass="FiltroCampoSmall" AutoPostBack="True" Visible="false">
		                            <asp:ListItem Text="Tutti" Value="-1" Selected="True"></asp:ListItem>
		                        </asp:DropDownList>
							</asp:TableCell>
							<asp:TableCell>
								<asp:Label ID="LBtipoRicerca_t" runat="server" CssClass="FiltroVoceSmall">Ricerca per:</asp:Label>
							</asp:TableCell>
							<asp:TableCell>
								<asp:DropDownList id="DDLtipoRicerca" Runat="server" CssClass="FiltroCampoSmall">
									<asp:ListItem Value="-1">tutti</asp:ListItem>
									<asp:ListItem Value="1">Nome</asp:ListItem>
									<asp:ListItem Value="2">Cognome</asp:ListItem>
								</asp:DropDownList>
							</asp:TableCell>
							<asp:TableCell>
								<asp:Label ID="LBvalore_t" runat="server" CssClass="FiltroVoceSmall">Valore:</asp:Label>&nbsp;
							</asp:TableCell>
							<asp:TableCell>
								<asp:TextBox id="TXBvalore" Runat="server" MaxLength="300" CssClass="FiltroCampoSmall" Columns="40"></asp:TextBox>
							</asp:TableCell>
							<asp:TableCell>
								<asp:LinkButton ID="Lkb_Cerca" runat="server" CssClass="PulsanteFiltro" Text="Cerca"></asp:LinkButton>
							</asp:TableCell>
						</asp:TableRow>
					</asp:Table>
				</asp:TableCell>
			</asp:TableRow>
			<asp:tableRow>
				<asp:tableCell ColumnSpan="5">
				    &nbsp;
				</asp:tableCell>
				<asp:TableCell HorizontalAlign="Right" >&nbsp;
					<asp:label ID="LBnumeroRecord" Runat="server" cssclass="Filtro_TestoPaginazione">N° Record</asp:label>&nbsp;
					<asp:dropdownlist id="DDLNumeroRecord" CssClass="Filtro_RecordPaginazione" Runat="server" AutoPostBack="true">
						<asp:ListItem Value="10" Selected="true">10</asp:ListItem>
						<asp:ListItem Value="20">20</asp:ListItem>
						<asp:ListItem Value="35">35</asp:ListItem>
						<asp:ListItem Value="50">50</asp:ListItem>
					</asp:dropdownlist>
				</asp:TableCell>
			</asp:tableRow>
		</asp:Table>
        
        
        <asp:Datagrid ID="DG_UtentiCom" runat="server" AutoGenerateColumns="false" ShowFooter="false"
            ShowHeader="true"
            CssClass="DataGrid_Generica" Width="900px">
            <AlternatingItemStyle CssClass="ROW_Alternate_Small"></AlternatingItemStyle>
		    <HeaderStyle CssClass="ROW_header_Small_Center"></HeaderStyle>
		    <ItemStyle CssClass="ROW_Normal_Small" Height="22px"></ItemStyle>
            <Columns>
               <asp:TemplateColumn>
                    <HeaderTemplate>
                        <asp:Label ID="LBL_DG_Nome_h" runat="server">Nome</asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="LKB_DG_Nome" runat="server">
                            <asp:Label ID="LBL_DG_Nome" runat="server">
                                Nome
                            </asp:Label>                            
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <asp:Label ID="LBL_DG_Cognome_h" runat="server">Cognome</asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="LKB_DG_Cognome" runat="server">
                            <asp:Label ID="LBL_DG_Cognome" runat="server">
                                Cognome
                            </asp:Label>                            
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <asp:Label ID="LBL_DG_Ruolo_h" runat="server">Ruolo</asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="LBL_DG_Ruolo" runat="server">Ruolo</asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <asp:Label ID="LBL_DG_TotDown_h" runat="server">Totale Download</asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="LBL_DG_TotDown" runat="server">###</asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <asp:Label ID="LBL_DG_KbDown_h" runat="server">Totale Kb down</asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="LBL_DG_KbDown" runat="server">###</asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <asp:Label ID="LBL_DG_LastDown_h" runat="server">Ultimo download</asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="LBL_DG_LastDown" runat="server">Ultimo download</asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:Datagrid>
        <br />
        <div style="width:900px; text-align:right; padding-top:5px; clear:both; height:22px;">
            <CTRL:GridPager id="PGgrid_UtentiCom" runat="server" EnableQueryString="false" ></CTRL:GridPager>
        </div>
    </asp:View>
    <asp:View ID="V_StatUtente" runat="server">
        Elenco File di un utente
        <br /><br />
        User Name
        <br /><br />
        <asp:DataGrid ID="Dg_FileUser" runat="server" 
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
                    <asp:LinkButton ID="Lnb_GotoFile" runat="server">
                        <asp:Label ID="Lbl_FileName" runat="server">#########.####</asp:Label>
                    </asp:LinkButton>
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
    </asp:View>
    <asp:View ID="V_StatFile" runat="server">
        Statistiche singolo File
        <br /><br />
        File Name & path(?)
        <br /><br />
        <asp:Datagrid ID="Dg_FileCom" runat="server" AutoGenerateColumns="false" ShowFooter="false"
            ShowHeader="true"
            CssClass="DataGrid_Generica" Width="900px">
            <AlternatingItemStyle CssClass="ROW_Alternate_Small"></AlternatingItemStyle>
		    <HeaderStyle CssClass="ROW_header_Small_Center"></HeaderStyle>
		    <ItemStyle CssClass="ROW_Normal_Small" Height="22px"></ItemStyle>
            <Columns>
               <asp:TemplateColumn>
                    <HeaderTemplate>
                        <asp:Label ID="LBL_DG_Nome_h" runat="server">Nome</asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="LKB_DG_Nome" runat="server">
                            <asp:Label ID="LBL_DG_Nome" runat="server">
                                Nome
                            </asp:Label>                            
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <asp:Label ID="LBL_DG_Cognome_h" runat="server">Cognome</asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="LKB_DG_Cognome" runat="server">
                            <asp:Label ID="LBL_DG_Cognome" runat="server">
                                Cognome
                            </asp:Label>                            
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <asp:Label ID="LBL_DG_Ruolo_h" runat="server">Ruolo</asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="LBL_DG_Ruolo" runat="server">Ruolo</asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <asp:Label ID="LBL_DG_TotDown_h" runat="server">Totale Download</asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="LBL_DG_TotDown" runat="server">###</asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <asp:Label ID="LBL_DG_KbDown_h" runat="server">Totale Kb down</asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="LBL_DG_KbDown" runat="server">###</asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <asp:Label ID="LBL_DG_LastDown_h" runat="server">Ultimo download</asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        ###
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:Datagrid>
    </asp:View>

</asp:MultiView>