<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_AggiungiUtenteForum.ascx.vb" Inherits="Comunita_OnLine.UC_AggiungiUtenteForum" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script language="javascript" type="text/javascript">
function SelectMe(Me){
		var HIDcheckbox,selezionati;
		eval('HIDcheckbox= this.document.forms[0].<%=Me.HDabilitato.ClientID%>')
			selezionati = 0
			for(i=0;i< document.forms[0].length; i++){ 
				e=document.forms[0].elements[i];
				if ( e.type=='checkbox' && e.name.indexOf("CBabilitato") != -1 ){
					if (e.checked==true){
						selezionati++
						if (HIDcheckbox.value == ""){
							HIDcheckbox.value = ',' + e.value+','
						}	  
						else{
							pos1 = HIDcheckbox.value.indexOf(',' + e.value+',')
							if (pos1==-1)
								HIDcheckbox.value = HIDcheckbox.value + e.value +','
							}
					}
					else{
						valore = HIDcheckbox.value
						pos1 = HIDcheckbox.value.indexOf(',' + e.value+',')
						if (pos1!=-1){
							stringa = ',' + e.value
							HIDcheckbox.value = HIDcheckbox.value.substring(0,pos1)
							HIDcheckbox.value = HIDcheckbox.value + valore.substring(pos1+e.value.length+1,valore.length)
							}
					}
				}  
			}
			if (HIDcheckbox.value==",")
				HIDcheckbox.value = ""
		}
		
		function SelectAll( SelectAllBox ){
			var actVar = SelectAllBox.checked ;
			var TBcheckbox;
			eval('HDabilitato= this.document.forms[0].<%=Me.HDabilitato.ClientID%>')
			HDabilitato.value = ""
			for(i=0;i< document.forms[0].length; i++){ 
				e=document.forms[0].elements[i];
				if ( e.type=='checkbox' && e.name.indexOf("CBabilitato") != -1 ){
					e.checked= actVar ;
					if (e.checked==true)
						if (HDabilitato.value == "")
							HDabilitato.value = ',' + e.value+','
						else
							HDabilitato.value = HDabilitato.value + e.value +','
				}
			}
		}
</script>
<asp:Panel ID="PNLfiltro" Runat="server" HorizontalAlign=Center>
    <div class="filtercontainer container_12 clearfix">
        <div class="filter grid_4">
            <asp:label Runat="server" ID="LBtipoRuolo_t" CssClass="label">Tipo Ruolo:</asp:label>&nbsp;
			<asp:dropdownlist id="DDLTipoRuolo" runat="server" CssClass="FiltroCampoSmall" AutoPostBack="true"></asp:dropdownlist>
        </div>
        <div class="filter grid_3">
            <asp:label Runat="server" ID="LBtipoRicerca_t" CssClass="label">Tipo Ricerca:</asp:label>&nbsp;
			<asp:dropdownlist id="DDLTipoRicerca" CssClass="FiltroCampoSmall" Runat="server" AutoPostBack="false">
				<asp:ListItem Selected="true" Value="-2">Nome</asp:ListItem>
				<asp:ListItem Value="-3">Cognome</asp:ListItem>
				<asp:ListItem value="-4">Nome/Cognome</asp:ListItem>
			</asp:dropdownlist>
        </div>
        <div class="filter grid_3">
            <asp:label Runat="server" ID="LBvalore_t" CssClass="label">Valore:</asp:label>&nbsp;
			<asp:textbox id="TXBValore" CssClass="FiltroCampoSmall" Runat="server" MaxLength="50"></asp:textbox>
        </div>
        <div class="filter grid_2">
            <asp:button id="BTNCerca" CssClass="pulsanteFiltro" Runat="server" Text="Cerca"></asp:button>
        </div>
    </div>
    <div class="linewrapper filterletters clearfix">
        <div class="left">
            <asp:linkbutton id="LKBtutti" Runat="server" CssClass="lettera" CommandArgument="-1" OnClick="FiltroLinkLettere_Click">Tutti</asp:linkbutton>
            <asp:linkbutton id="LKBaltro" Runat="server" CssClass="lettera" CommandArgument="0" OnClick="FiltroLinkLettere_Click">Altro</asp:linkbutton>
            <asp:linkbutton id="LKBa" Runat="server" CssClass="lettera" CommandArgument="1" OnClick="FiltroLinkLettere_Click">A</asp:linkbutton>
            <asp:linkbutton id="LKBb" Runat="server" CssClass="lettera" CommandArgument="2" OnClick="FiltroLinkLettere_Click">B</asp:linkbutton>
            <asp:linkbutton id="LKBc" Runat="server" CssClass="lettera" CommandArgument="3" OnClick="FiltroLinkLettere_Click">C</asp:linkbutton>
            <asp:linkbutton id="LKBd" Runat="server" CssClass="lettera" CommandArgument="4" OnClick="FiltroLinkLettere_Click">D</asp:linkbutton>
            <asp:linkbutton id="LKBe" Runat="server" CssClass="lettera" CommandArgument="5" OnClick="FiltroLinkLettere_Click">E</asp:linkbutton>
            <asp:linkbutton id="LKBf" Runat="server" CssClass="lettera" CommandArgument="6" OnClick="FiltroLinkLettere_Click">F</asp:linkbutton>
            <asp:linkbutton id="LKBg" Runat="server" CssClass="lettera" CommandArgument="7" OnClick="FiltroLinkLettere_Click">G</asp:linkbutton>
            <asp:linkbutton id="LKBh" Runat="server" CssClass="lettera" CommandArgument="8" OnClick="FiltroLinkLettere_Click">H</asp:linkbutton>
            <asp:linkbutton id="LKBi" Runat="server" CssClass="lettera" CommandArgument="9" OnClick="FiltroLinkLettere_Click">I</asp:linkbutton>
            <asp:linkbutton id="LKBj" Runat="server" CssClass="lettera" CommandArgument="10" OnClick="FiltroLinkLettere_Click">J</asp:linkbutton>
            <asp:linkbutton id="LKBk" Runat="server" CssClass="lettera" CommandArgument="11" OnClick="FiltroLinkLettere_Click">K</asp:linkbutton>
            <asp:linkbutton id="LKBl" Runat="server" CssClass="lettera" CommandArgument="12" OnClick="FiltroLinkLettere_Click">L</asp:linkbutton>
            <asp:linkbutton id="LKBm" Runat="server" CssClass="lettera" CommandArgument="13" OnClick="FiltroLinkLettere_Click">M</asp:linkbutton>
            <asp:linkbutton id="LKBn" Runat="server" CssClass="lettera" CommandArgument="14" OnClick="FiltroLinkLettere_Click">N</asp:linkbutton>
            <asp:linkbutton id="LKBo" Runat="server" CssClass="lettera" CommandArgument="15" OnClick="FiltroLinkLettere_Click">O</asp:linkbutton>
            <asp:linkbutton id="LKBp" Runat="server" CssClass="lettera" CommandArgument="16" OnClick="FiltroLinkLettere_Click">P</asp:linkbutton>
            <asp:linkbutton id="LKBq" Runat="server" CssClass="lettera" CommandArgument="17" OnClick="FiltroLinkLettere_Click">Q</asp:linkbutton>
            <asp:linkbutton id="LKBr" Runat="server" CssClass="lettera" CommandArgument="18" OnClick="FiltroLinkLettere_Click">R</asp:linkbutton>
            <asp:linkbutton id="LKBs" Runat="server" CssClass="lettera" CommandArgument="19" OnClick="FiltroLinkLettere_Click">S</asp:linkbutton>
            <asp:linkbutton id="LKBt" Runat="server" CssClass="lettera" CommandArgument="20" OnClick="FiltroLinkLettere_Click">T</asp:linkbutton>
            <asp:linkbutton id="LKBu" Runat="server" CssClass="lettera" CommandArgument="21" OnClick="FiltroLinkLettere_Click">U</asp:linkbutton>
            <asp:linkbutton id="LKBv" Runat="server" CssClass="lettera" CommandArgument="22" OnClick="FiltroLinkLettere_Click">V</asp:linkbutton>
            <asp:linkbutton id="LKBw" Runat="server" CssClass="lettera" CommandArgument="23" OnClick="FiltroLinkLettere_Click">W</asp:linkbutton>
            <asp:linkbutton id="LKBx" Runat="server" CssClass="lettera" CommandArgument="24" OnClick="FiltroLinkLettere_Click">X</asp:linkbutton>
            <asp:linkbutton id="LKBy" Runat="server" CssClass="lettera" CommandArgument="25" OnClick="FiltroLinkLettere_Click">Y</asp:linkbutton>
            <asp:linkbutton id="LKBz" Runat="server" CssClass="lettera" CommandArgument="26" OnClick="FiltroLinkLettere_Click">Z</asp:linkbutton>

        </div>
        <div class="right">
            <asp:label Runat="server" ID="LBnumeroRecord_t" CssClass="FiltroVoceSmall">N° Record:</asp:label>&nbsp;
				<asp:dropdownlist id="DDLNumeroRecord" CssClass="FiltroCampoSmall" Runat="server" AutoPostBack="true">
					<asp:ListItem Value="20"> 20</asp:ListItem>
					<asp:ListItem Value="30"> 30</asp:ListItem>
					<asp:ListItem Value="50"> 50</asp:ListItem>
					<asp:ListItem Value="75"> 75</asp:ListItem>
					<asp:ListItem Value="100">100</asp:ListItem>
				</asp:dropdownlist>
        </div>
    </div>    
</asp:Panel>
<asp:Panel ID="PNLpersone" Runat=server HorizontalAlign=Center>
	<INPUT id="HDabilitato" type="hidden" name="HDabilitato" runat="server"/>
				<asp:datagrid CssClass="table light fullwidth persons" id="DGPersone" runat="server" BorderColor="#8080FF" DataKeyField="PRSN_id" AllowPaging="true"
					AutoGenerateColumns="False" BackColor="transparent" ShowFooter="false" UseAccessibleHeader="true"
					Font-Size="8" AllowSorting="true" PageSize="20" AllowCustomPaging="True">
					<AlternatingItemStyle CssClass="ROW_Alternate_Small"></AlternatingItemStyle>
					<HeaderStyle CssClass=""></HeaderStyle>
					<ItemStyle CssClass="ROW_Normal_Small" Height="22px"></ItemStyle>
					<PagerStyle CssClass="ROW_Page_Small" Position=TopAndBottom Mode="NumericPages" Visible="true"
					HorizontalAlign="Right" Height="18px" VerticalAlign="Bottom"></PagerStyle>
					<Columns>
						<asp:BoundColumn DataField="PRSN_id" Visible="False"></asp:BoundColumn>
						<asp:TemplateColumn runat="server" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10">
							<ItemTemplate>
								<asp:ImageButton id="IMBaggiungi" Runat="server" CausesValidation="False" CommandName="Aggiungi"
									ImageUrl="./../../images/add.gif"></asp:ImageButton>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn runat="server" HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10">
							<ItemTemplate>
								<asp:ImageButton id="IMBinfo" Runat="server" CausesValidation="False" CommandName="infoPersona" ImageUrl="./../../images/proprieta.gif"></asp:ImageButton>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:BoundColumn DataField="PRSN_Anagrafica" HeaderText="Anagrafica" HeaderStyle-CssClass="ROW_HEADER_Small" ItemStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
						<asp:BoundColumn DataField="PRSN_login" HeaderText="Login" SortExpression="PRSN_login" ItemStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
						<asp:BoundColumn DataField="TPRL_nome" HeaderText="Ruolo" SortExpression="TPRL_nome" ItemStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
						<asp:BoundColumn DataField="PRSN_TPPR_id" Visible="False"></asp:BoundColumn>
						<asp:BoundColumn DataField="TPPR_descrizione" HeaderText="Tipo Persona" SortExpression="TPPR_descrizione" ItemStyle-CssClass=ROW_TD_Small></asp:BoundColumn>
						<asp:BoundColumn DataField="RLPC_id" Visible=False></asp:BoundColumn>
						<asp:TemplateColumn ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass=ROW_TD_Small>
							<HeaderTemplate>
								<input type="checkbox" id="SelectAll2" onclick="SelectAll(this);" runat="server" NAME="SelectAll"/>
							</HeaderTemplate>
							<ItemTemplate>
								<input type="checkbox" value=<%# DataBinder.Eval(Container.DataItem, "RLPC_id") %> id="CBabilitato" name="CBabilitato" <%# DataBinder.Eval(Container.DataItem, "oCheck") %> onclick="SelectMe(this);">
							</ItemTemplate>
						</asp:TemplateColumn>
					</Columns>
					<PagerStyle Width="780px" PageButtonCount="5" mode="NumericPages"></PagerStyle>
				</asp:datagrid>
</asp:Panel>
<asp:Panel id="PNLNoUsers" Runat="server" Visible="False">
	<table>
		<tr>
			<td>
				<br/>
				<br/>
				<asp:Label id="LBMessaggio" CssClass="avviso11" Runat="server"></asp:Label>
				<br/>
				<br/>
			</td>
		</tr>
	</table>
</asp:Panel>
<asp:Panel ID="PNLruoloUtente" Visible="False" Runat="server" Width=100% HorizontalAlign="Center">
    <div class="fieldobject adduser">
        <div class="fieldrow">
            <asp:label ID="LBdescrizione_t" Runat="server" CssClass="fieldlabel" AssociatedControlID="LBdescrizione">Aggiungi con ruolo di:&nbsp;</asp:label>
            <asp:Label ID="LBdescrizione" Runat="server" CssClass="Testo_CampoSmall"></asp:Label>
        </div>
        <div class="fieldrow">
            <asp:label ID="LBtiporuoloAggiungi_t" Runat="server" CssClass="fieldlabel" AssociatedControlID="DDLtipoRuoloAggiungi">Aggiungi con ruolo di:&nbsp;</asp:label>
            <asp:DropDownList ID="DDLtipoRuoloAggiungi" Runat="server" CssClass="Testo_CampoSmall"></asp:DropDownList>
        </div>
    </div>	
</asp:Panel>