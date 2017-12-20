<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_Lista.ascx.vb" Inherits="Comunita_OnLine.UC_Lista" %>
<asp:Panel ID="PNLContenitore" runat="server">
<table width="800">
    <tr><td>
        &nbsp;
    </td></tr>
    <tr><td>
        <asp:Panel ID="PNL_NomeLista" runat="server">
            <asp:Label ID="LBL_NomeLista_t" runat="server" CssClass="Titolo_campo">Nome lista:</asp:Label>
            <asp:TextBox ID="TXB_NomeLista" runat="server" MaxLength="30" Width="450px" CssClass="Testo_campo"></asp:TextBox>
            <asp:Button ID="BTN_ModificaNome" runat="server" Text="M" cssclass="PulsanteFiltro"/>
        </asp:Panel>
    </td></tr>
    <tr><td>
        &nbsp;
    </td></tr>
    <tr><td>
         <asp:GridView ID="GRVIndirizzi" 
            runat="server"
            AllowPaging="False" 
            AutoGenerateColumns="False"
		    AllowSorting="True"
		    DataKeyNames="ID"
		    PagerSettings-Mode="Numeric"
		    CssClass="DataGrid_Generica"> 
            
            <HeaderStyle CssClass="ROW_header_Small_Center"></HeaderStyle>
            <AlternatingRowStyle CssClass="ROW_Alternate_Small"></AlternatingRowStyle>
			<RowStyle CssClass="ROW_Normal_Small" Height="22px"></RowStyle>
			<PagerStyle CssClass="ROW_Page_Small" HorizontalAlign="Right" height="18px" VerticalAlign="Bottom" ></PagerStyle>
	        
	    <PagerSettings Position="TopAndBottom" />
    	
        <Columns>
            
            <asp:CommandField ButtonType="Link" ShowEditButton="True"  EditImageUrl="~/images/DG/m.gif" ItemStyle-Width="18" HeaderText="M-" />
		    <asp:CommandField ButtonType="Link" ShowDeleteButton="True" DeleteImageUrl="~/images/DG/x.gif" ItemStyle-Width="18" HeaderText="C-" />
    		
		    <asp:BoundField DataField="Titolo" HeaderText="Titolo" SortExpression="Titolo" HeaderStyle-HorizontalAlign="Left" >
            </asp:BoundField>
            
		    <asp:BoundField DataField="PersonaNome" HeaderText="Nome" SortExpression="PersonaNome" HeaderStyle-HorizontalAlign="Left">
            </asp:BoundField>
            
            <asp:BoundField DataField="PersonaCognome" HeaderText="Cognome" SortExpression="PersonaCognome" HeaderStyle-HorizontalAlign="Left">
            </asp:BoundField>
            
            <asp:BoundField DataField="PersonaMail" HeaderText="Mail" SortExpression="PersonaMail" HeaderStyle-HorizontalAlign="Left">
            </asp:BoundField>
            
            <asp:BoundField DataField="Struttura" HeaderText="Struttura" SortExpression="Struttura" HeaderStyle-HorizontalAlign="Left">
            </asp:BoundField>
            
        </Columns>
    </asp:GridView>
   
    </td></tr>
    <tr><td>
        <asp:Panel ID="PNL_InserimentoModifica" runat="server" Width="850px">
            <div style="width:850px;">
                <div style="width:600px; height:25px; clear:left">
                    <asp:Label ID="LBL_IAddModify" runat="server" CssClass="Titolo_campo">Nuovo iscritto:</asp:Label>
                </div> 
	            <div style="width:850px; height:25px; clear:left">
		            <div style="width:150px; height:25px; float: left;">
		                <asp:Label ID="LBL_ITitolo_t" runat="server" CssClass="Titolo_campo">Titolo:</asp:Label>
		            </div>
		            <div style="width:450px; height:25px; float: left;">
		                <asp:TextBox ID="TXB_ITitolo" runat="server" MaxLength="25" Width="360px" CssClass="Testo_campo"></asp:TextBox>
		            </div>
	            </div>
            	
	            <div style="width:850px; height:25px; clear:left">
		            <div style="width:150px; height:25px; float: left;">
		                <asp:Label ID="LBL_INome_t" runat="server" CssClass="Titolo_campo">Nome:</asp:Label>
		            </div>
		            <div style="width:450px; height:25px; float: left;">
			            <asp:TextBox ID="TXB_INome" runat="server" MaxLength="40" Width="360px" CssClass="Testo_campo"></asp:TextBox>
			            <asp:RequiredFieldValidator ID="RFV_Nome" runat="server" ErrorMessage="*" 
			            ControlToValidate="TXB_INome" Display="Dynamic"></asp:RequiredFieldValidator>
	                </div>
	            </div>
            	
	            <div style="width:850px; height:25px; clear:left">
		            <div style="width:150px; height:25px; float: left;">
		                <asp:Label ID="LBL_ICognome_t" runat="server" CssClass="Titolo_campo">Cognome:</asp:Label>
		            </div>
		            <div style="width:450px; height:25px; float: left;">
			            <asp:TextBox ID="TXB_ICognome" runat="server" MaxLength="40" Width="360px" CssClass="Testo_campo"></asp:TextBox>
	                </div>
	            </div>
            	
	            <div style="width:850px; height:25px; clear:left">
		            <div style="width:150px; height:25px; float: left;">
		                <asp:Label ID="LBL_IMail_t" runat="server" CssClass="Titolo_campo">Mail:</asp:Label>
		            </div>
		            <div style="width:450px; height:25px; float: left;">
			            <asp:TextBox ID="TXB_IMail" runat="server" MaxLength="255" Width="360px" CssClass="Testo_campo"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RFV_Mail" runat="server" ErrorMessage="*" ControlToValidate="TXB_IMail" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="REV_Mail" runat="server" ErrorMessage="Formato Mail non valida" ControlToValidate="TXB_IMail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic"></asp:RegularExpressionValidator>
		            </div>
	            </div>
	            <div style="width:850px; height:25px; clear:left">
		            <div style="width:150px; height:25px; float: left;">
		                <asp:Label ID="LBL_IStruttura_t" runat="server" CssClass="Titolo_campo">Struttura:</asp:Label>
		            </div>
		            <div style="width:450px; height:25px; float: left;">
			            <asp:TextBox ID="TXB_IStruttura" runat="server" MaxLength="100" Width="360px" CssClass="Testo_campo"></asp:TextBox>
	                </div>
	            </div>
	            <div style="width:850px; height:25px; clear:left">
		            <div style="width:150px; height:25px; float: left;"></div>
		            <div style="width:450px; height:25px; float: left; text-align:right;">
			            <asp:button ID="BTNCancel" runat="server" CausesValidation="False" CommandName="Cancel" Text="Annulla" CssClass="PulsanteFiltro"/>
			            <asp:Button id="BTNupdate" runat="server" CausesValidation="True" CommandName="Update" Text="Salva modifiche" CssClass="PulsanteFiltro"/>
                        <asp:Button id="BTNinsert" runat="server" CausesValidation="True" CommandName="Insert" Text="Inserisci" CssClass="PulsanteFiltro"/>
                    </div>
	            </div>
            </div>
        </asp:Panel>
    </td></tr>
    
    <tr><td align="center" >
        <asp:Label ID="LBLMessage" runat="server" Visible="false" CssClass="messaggio"></asp:Label>
    </td></tr>
    
    <tr><td>&nbsp;</td></tr>
    
    <tr><td align="right">
        <asp:Button ID="BTN_InserisciExt" runat="server" CausesValidation="false" CssClass="PulsanteFiltro" Text="Aggiungi esterno" />
        &nbsp;
        <asp:Button ID="BTN_InserisciInt" runat="server" CausesValidation="false" CssClass="PulsanteFiltro" Text="Nuovo esterno" />
    </td></tr>
</table>
</asp:Panel>