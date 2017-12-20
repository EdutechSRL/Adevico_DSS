<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_modificaTipoComunita.ascx.vb" Inherits="Comunita_OnLine.UC_modificaTipoComunita" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
	<script type="text/javascript" language="javascript">
	
//	    function SelezioneRiga(elementi){
//		  var HIDcheckbox;
//		  var totale, selezionati;
//		  //eval('HIDcheckbox= this.document.forms[0].<%=Me.HIDcheckbox.clientID%>');
//		  HIDcheckbox = this.document.getElementById('<%=Me.HIDcheckbox.ClientID%>');
//		  totale = 0
//		  selezionati = 0
//		  deselezionati=0
//   		  for(i=0;i< document.forms[0].length; i++){ 
//			 e=document.forms[0].elements[i];
//			 if ( e.type=='checkbox' && elementi.indexOf(',' + e.name + ',') != -1 ) {
//				totale = totale + 1
//				if (e.checked==true)
//				    selezionati = selezionati + 1
//				else
//				    deselezionati = deselezionati + 1
//			 } 
//		  }
//		  if (totale >0 && totale == selezionati){
//			   for(i=0;i< document.forms[0].length; i++){ 
//				e=document.forms[0].elements[i];
//				if ( e.type=='checkbox' && elementi.indexOf(',' + e.name + ',') != -1 ) {
//				    e.checked= false;
//				    HIDcheckbox.value = HIDcheckbox.value.replace(',' + e.value + ',',',')
//				    } 
//			 }
//		  }
//		  else{
//			 for(i=0;i< document.forms[0].length; i++){ 
//				e=document.forms[0].elements[i];
//				if ( e.type=='checkbox' && elementi.indexOf(',' + e.name + ',') != -1 ) {
//				    e.checked= true;
//				    if (HIDcheckbox.value == "")
//					   HIDcheckbox.value = ',' + e.value +','
//				    else{
//					   pos1 = HIDcheckbox.value.indexOf(',' + e.value + ',')
//					   if (pos1==-1)
//						  HIDcheckbox.value = HIDcheckbox.value + e.value +','
//					   }
//				}
//			 } 
//		    }
//		  
//		if (HIDcheckbox.value==",")
//			HIDcheckbox.value = "";
//		return false
//	}
//	
//	  function SelezioneColonna(NomeColonna){
//		  var HIDcheckbox;
//		  var totale, selezionati;
//		  //eval('HIDcheckbox= this.document.forms[0].<%=Me.HIDcheckbox.clientID%>');
//		  HIDcheckbox = this.document.getElementById('<%=Me.HIDcheckbox.ClientID%>');
//		  totale = 0
//		  selezionati = 0
//		  deselezionati=0

//   		  for(i=0;i< document.forms[0].length; i++){ 
//			 e=document.forms[0].elements[i];
//			 
//			 if ( e.type=='checkbox' && e.name.indexOf('<%=me.clientID & me.IdSeparator.ToString%>' + NomeColonna + '_') != -1 ) {
//				totale = totale + 1
//				if (e.checked==true)
//				    selezionati = selezionati + 1
//				else
//				    deselezionati = deselezionati + 1
//			 } 
//		  }
//		  if (totale >0 && totale == selezionati){
//			   for(i=0;i< document.forms[0].length; i++){ 
//				e=document.forms[0].elements[i];
//				if ( e.type=='checkbox' && e.name.indexOf('<%=me.clientID & me.IdSeparator.ToString%>' + NomeColonna + '_') != -1 ) {
//				    e.checked= false;
//				    HIDcheckbox.value = HIDcheckbox.value.replace(',' + e.value + ',',',')
//				    } 
//			 }
//		  }
//		  else{
//			 for(i=0;i< document.forms[0].length; i++){ 
//				e=document.forms[0].elements[i];
//				if ( e.type=='checkbox' && e.name.indexOf('<%=me.clientID & me.IdSeparator.ToString%>' + NomeColonna + '_') != -1 ) {
//				    e.checked= true;
//				    if (HIDcheckbox.value == "")
//					   HIDcheckbox.value = ',' + e.value +','
//				    else{
//					   pos1 = HIDcheckbox.value.indexOf(',' + e.value + ',')
//					   if (pos1==-1)
//						  HIDcheckbox.value = HIDcheckbox.value + e.value +','
//					   }
//				}
//			 } 
//		    }
//		  
//		if (HIDcheckbox.value==",")
//			HIDcheckbox.value = "";

//		return false
//	}
	
	
	function RestaSelezionato(){
	 var HIDcheckbox;
	 
	 //eval('HIDcheckbox= this.document.forms[0].<%=me.HDNruoliAssociati.clientID%>')
	 HIDcheckbox = this.document.getElementById('<%=Me.HDNruoliAssociati.clientID%>');
		for(i=0;i< document.forms[0].length; i++){ 
			e=document.forms[0].elements[i];
			if ( e.type=='checkbox' && e.name.indexOf("<%=me.CBLtipoRuolo.clientID%>") != -1 ){
		
				if  ( HIDcheckbox.value.indexOf(',' + e.value + ',') != -1 ){
					
					e.checked==true;
					return false;
					}	
				else{
					return true;
					}
					
			}
		}
		return true;
		
	 }
	 
//	 function SelectFromNameAndAssocia(Nome,value){
//		var HIDcheckbox;
//		//eval('HIDcheckbox= this.document.forms[0].<%=Me.HIDcheckbox.clientID%>');
//		HIDcheckbox = this.document.getElementById('<%=Me.HIDcheckbox.clientID%>');
//   		for(i=0;i< document.forms[0].length; i++){ 
//			e=document.forms[0].elements[i];
//				
//			if ( e.type=='checkbox' && e.name == '<%=me.clientID & me.IdSeparator.ToString%>CB_' +value) {//"CBXassocia"
//				if (e.checked==true){
//					if (HIDcheckbox.value == "")
//						HIDcheckbox.value = ',' + value +','
//					else{
//						pos1 = HIDcheckbox.value.indexOf(',' + value+',')
//						if (pos1==-1)
//						HIDcheckbox.value = HIDcheckbox.value + value +','
//						}
//					}
//				else{
//					valore = HIDcheckbox.value;
//					pos1 = HIDcheckbox.value.indexOf(',' + value+',')
//					if (pos1!=-1){
//						stringa = ',' + value+','
//						HIDcheckbox.value = HIDcheckbox.value.substring(0,pos1)
//						HIDcheckbox.value = HIDcheckbox.value + valore.substring(pos1+value.length+1,valore.length)
//						}
//					}
//				}
//		}
//		if (HIDcheckbox.value==",")
//			HIDcheckbox.value = "";
//	}
//	
	
	 </script>
     <script type="text/javascript" language="javascript">

         function SelezioneRiga(elementi) {
             var HIDcheckbox;
             var totale, selezionati;
             HIDcheckbox = this.document.getElementById('<%=Me.HIDcheckbox.ClientID%>');
             totale = 0
             selezionati = 0
             deselezionati = 0
             for (i = 0; i < document.forms[0].length; i++) {
                 e = document.forms[0].elements[i];
                 /*  if (e.type == 'checkbox')
                 alert(e.name);*/
                 if (e.type == 'checkbox' && elementi.indexOf(',' + e.value + ',') != -1) {
                     totale = totale + 1
                     if (e.checked == true)
                         selezionati = selezionati + 1
                     else
                         deselezionati = deselezionati + 1
                 }
             }
             if (totale > 0 && totale == selezionati) {
                 for (i = 0; i < document.forms[0].length; i++) {
                     e = document.forms[0].elements[i];
                     if (e.type == 'checkbox' && elementi.indexOf(',' + e.value + ',') != -1) {
                         e.checked = false;
                         HIDcheckbox.value = HIDcheckbox.value.replace(',' + e.value + ',', ',')
                     }
                 }
             }
             else {
                 for (i = 0; i < document.forms[0].length; i++) {
                     e = document.forms[0].elements[i];
                     if (e.type == 'checkbox' && elementi.indexOf(',' + e.value + ',') != -1) {
                         e.checked = true;
                         if (HIDcheckbox.value == "")
                             HIDcheckbox.value = ',' + e.value + ','
                         else {
                             pos1 = HIDcheckbox.value.indexOf(',' + e.value + ',')
                             if (pos1 == -1)
                                 HIDcheckbox.value = HIDcheckbox.value + e.value + ','
                         }
                     }
                 }
             }

             if (HIDcheckbox.value == ",")
                 HIDcheckbox.value = "";
             return false
         }

         function SelezioneColonna(NomeColonna) {
             var HIDcheckbox;
             var totale, selezionati;
             //eval('HIDcheckbox= this.document.forms[0].<%=Me.HIDcheckbox.clientID%>');
             HIDcheckbox = this.document.getElementById('<%=Me.HIDcheckbox.ClientID%>');
             totale = 0
             selezionati = 0
             deselezionati = 0

             for (i = 0; i < document.forms[0].length; i++) {
                 e = document.forms[0].elements[i];
                 if (e.type == 'checkbox' && e.value.indexOf(NomeColonna + '_') != -1) {
                     totale = totale + 1
                     if (e.checked == true)
                         selezionati = selezionati + 1
                     else
                         deselezionati = deselezionati + 1
                 }
             }
             if (totale > 0 && totale == selezionati) {
                 for (i = 0; i < document.forms[0].length; i++) {
                     e = document.forms[0].elements[i];
                     if (e.type == 'checkbox' && e.value.indexOf(NomeColonna + '_') != -1) {
                         e.checked = false;
                         HIDcheckbox.value = HIDcheckbox.value.replace(',' + e.value + ',', ',')
                     }
                 }
             }
             else {
                 for (i = 0; i < document.forms[0].length; i++) {
                     e = document.forms[0].elements[i];
                     if (e.type == 'checkbox' && e.value.indexOf(NomeColonna + '_') != -1) {
                         e.checked = true;
                         if (HIDcheckbox.value == "")
                             HIDcheckbox.value = ',' + e.value + ','
                         else {
                             pos1 = HIDcheckbox.value.indexOf(',' + e.value + ',')
                             if (pos1 == -1)
                                 HIDcheckbox.value = HIDcheckbox.value + e.value + ','
                         }
                     }
                 }
             }

             if (HIDcheckbox.value == ",")
                 HIDcheckbox.value = "";
             return false
         }



         function SelectFromNameAndAssocia(value) {
             var HIDcheckbox;
             //eval('HIDcheckbox= this.document.forms[0].<%=Me.HIDcheckbox.clientID%>');
             HIDcheckbox = this.document.getElementById('<%=Me.HIDcheckbox.ClientID%>');
             for (i = 0; i < document.forms[0].length; i++) {
                 e = document.forms[0].elements[i];

                 if (e.type == 'checkbox' && e.value == value) {//"CBXassocia"
                     if (e.checked == true) {
                         if (HIDcheckbox.value == "")
                             HIDcheckbox.value = ',' + value + ','
                         else {
                             pos1 = HIDcheckbox.value.indexOf(',' + value + ',')
                             if (pos1 == -1)
                                 HIDcheckbox.value = HIDcheckbox.value + value + ','
                         }
                     }
                     else {
                         valore = HIDcheckbox.value;
                         pos1 = HIDcheckbox.value.indexOf(',' + value + ',')
                         if (pos1 != -1) {
                             stringa = ',' + value + ','
                             HIDcheckbox.value = HIDcheckbox.value.substring(0, pos1)
                             HIDcheckbox.value = HIDcheckbox.value + valore.substring(pos1 + value.length + 1, valore.length)
                         }
                     }
                 }
             }
             if (HIDcheckbox.value == ",")
                 HIDcheckbox.value = "";
         }
	
	
	 </script>
<asp:Table Runat=server id="TBLinserimento" CellPadding=0 CellSpacing=0 Width=750px>
	<asp:TableRow>
		<asp:TableCell>&nbsp;</asp:TableCell>
		<asp:TableCell HorizontalAlign=left >
			<asp:Table HorizontalAlign=left Runat=server ID="TBLdati" Width=750px Visible=False>
				<asp:TableRow>
					<asp:TableCell width=120px >
						<asp:Label ID="LBtipoComunita_t" Runat=server CssClass="Titolo_CampoSmall">Nome:</asp:Label>
					</asp:TableCell>
					<asp:TableCell>
						<asp:TextBox ID="TXBtipoComunita" Runat=server CssClass="Testo_campo_obbligatorioSmall" MaxLength=100 Columns=60></asp:TextBox>
						<asp:requiredfieldvalidator id="RFVnome" runat="server" CssClass="Validatori" ControlToValidate="TXBtipoComunita" Display="Dynamic" EnableClientScript=True>*</asp:requiredfieldvalidator>
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow>
					<asp:TableCell width=120px >&nbsp;</asp:TableCell>
					<asp:TableCell CssClass=top>
						<table border=1 align=left bgcolor="#FFFBF7" style="border-color:#CCCCCC" cellpadding=0 cellspacing=0>
							<tr>
								<td>
									<table border=0 align=left bgcolor="#FFFBF7" cellpadding=0 cellspacing=0>
										<asp:Repeater id="RPTnome" Runat="server">
											<HeaderTemplate>
												<tr>
													<td colspan=2 height=20px>
														<asp:Label ID="LBlinguaNome_t" Runat=server CssClass="Titolo_campoSmall">&nbsp;Traduzioni(°):</asp:Label>
													</td>
												</tr>
											</HeaderTemplate>
											<ItemTemplate>
												<tr>
													<td align=right width=120px height=22px>
														<asp:Label id="LBlinguaID" Text='<%# Databinder.eval(Container.DataItem, "LNGU_ID")%>' runat="server" Visible=false />
														<asp:Label id="LBlingua_Nome" Text='<%# Databinder.eval(Container.DataItem, "LNGU_nome")%>' runat="server" Visible=true CssClass=Repeater_VoceLingua/>&nbsp;
													</td>
													<td align=left height=22px>
														<asp:TextBox ID="TXBtermine" Runat="server" CssClass="Testo_campoSmall" MaxLength="100" Columns="60" Text='<%# Databinder.eval(Container.DataItem, "Nome")%>'></asp:TextBox>&nbsp;&nbsp;
													</td>
												</tr>
											</ItemTemplate>
											<FooterTemplate>
												<tr><td colspan=2 class=nosize0>&nbsp;</td></tr>
											</FooterTemplate>
										</asp:Repeater>
									</table>
								</td>
							</tr>
						</table>								
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow ID="TBRicona_attuale">
					<asp:TableCell width=120px >
						<asp:Label ID="LBicona_t" Runat=server CssClass="Titolo_CampoSmall">Icona:</asp:Label>
					</asp:TableCell>
					<asp:TableCell>
						<asp:Image ID=IMicona Runat=server ></asp:Image>
						&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<INPUT id="TXBFile" type="file" runat="server" NAME="TXBFile" Class="Testo_campoSmall" size=40/>
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow>
					<asp:TableCell CssClass=top width=120px >
						<asp:Label ID="LBtipoSottoComunita_t" Runat=server CssClass="Titolo_CampoSmall">Sotto comunità:</asp:Label>
					</asp:TableCell>
					<asp:TableCell CssClass=top>
						<asp:CheckBoxList ID=CHLtipoSottoComunita Runat=server CssClass="Testo_campoSmall" RepeatDirection=Horizontal RepeatColumns=3 RepeatLayout=Table></asp:CheckBoxList>
					</asp:TableCell>				
				</asp:TableRow>
			</asp:Table>
			<asp:Table HorizontalAlign=left Runat=server ID="TBLcategoriaFile" Width=750px Visible=False >
				<asp:TableRow>
					<asp:TableCell  CssClass="top">
						<asp:Label ID="LBcategoriaFile_t" Runat=server CssClass="Titolo_CampoSmall">Categorie file Associati:</asp:Label>
					</asp:TableCell>
					<asp:TableCell  CssClass="top">
						<asp:CheckBoxList ID="CBLcategoriaFile" Runat=server CssClass="Testo_campoSmall" RepeatDirection=Vertical RepeatColumns=3 RepeatLayout=Table >
						</asp:CheckBoxList>
					</asp:TableCell>
				</asp:TableRow>
			</asp:Table>
			<asp:Table HorizontalAlign=left Runat=server ID="TBLtipoRuolo" Width=750px Visible=False >
				<asp:TableRow>
					<asp:TableCell>
						<asp:Label ID="LBruoloDefault_t" Runat=server CssClass="Titolo_CampoSmall">Ruolo default:</asp:Label>
					</asp:TableCell>
					<asp:TableCell>
						<asp:DropDownList ID="DDLruoloDefault" Runat=server CssClass="Testo_campoSmall"></asp:DropDownList>
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow>
					<asp:TableCell  CssClass="top" Width=150px Wrap=False >
						<asp:Label ID="LBtipiRuolo_t" Runat=server CssClass="Titolo_CampoSmall">Tipo Ruolo Associati:</asp:Label>
					</asp:TableCell>
					<asp:TableCell  CssClass="top">
						<table>
							<tr>
								<td>
									<asp:CheckBoxList ID="CBLtipoRuolo" Runat=server CssClass="Testo_campoSmall" DataValueField ="TPRL_ID" RepeatLayout=Table RepeatColumns=3 RepeatDirection=Horizontal>
									</asp:CheckBoxList>
								</td>
							</tr>
						</table>
					</asp:TableCell>
				</asp:TableRow>
			</asp:Table>
			<asp:Table HorizontalAlign=left Runat=server ID="TBLruoliAllways" Width=800px Visible=False >
				<asp:TableRow>
					<asp:TableCell Height=40px>&nbsp;</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow>
					<asp:TableCell>
						<asp:Label ID="LBtipiRuoloAll_t" Runat=server CssClass="Titolo_CampoSmall">Selezionare i Ruoli che si intendono rendere sempre disponibili anche nella definizione dei profili personali.</asp:Label>
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow>
					<asp:TableCell  CssClass="top">
						<table cellpadding=0 cellspacing=0>
							<tr>
								<td>
									<asp:CheckBoxList ID="CBLtipoRuoloAll" Runat=server CssClass="Testo_campoSmall" DataValueField ="TPRL_ID" RepeatLayout=Table RepeatColumns=4 RepeatDirection=Horizontal>
									</asp:CheckBoxList>
								</td>
							</tr>
						</table>
					</asp:TableCell>
				</asp:TableRow>
			</asp:Table>
			<asp:Table HorizontalAlign=left Runat=server ID="TBLmodelli" Width=750px Visible=False >
				<asp:TableRow>
					<asp:TableCell CssClass=top width=150px Wrap=False >
						<asp:Label ID="LBmodelloDefault_t" Runat=server CssClass="Titolo_CampoSmall">Modello default:</asp:Label>
					</asp:TableCell>
					<asp:TableCell>
						<asp:DropDownList ID="DDLmodelloDefault" Runat=server CssClass="Testo_campoSmall"></asp:DropDownList>
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow>
					<asp:TableCell  CssClass="top" Width=150px Wrap=False >
						<asp:Label ID="LBmodelli_t" Runat=server CssClass="Titolo_CampoSmall">Modelli Comunità Associati:</asp:Label>
					</asp:TableCell>
					<asp:TableCell  CssClass="top">
						<asp:CheckBoxList ID="CBLmodelli" Runat=server CssClass="Testo_campoSmall" RepeatDirection=Vertical>
						</asp:CheckBoxList>
					</asp:TableCell>
				</asp:TableRow>
			</asp:Table>
			<asp:Table ID="TBLservizio" Runat=server Width=750px Visible=False GridLines=none>
				<asp:TableRow Visible=False >
					<asp:TableCell  CssClass="top" Width=150px Wrap=False >
						<asp:Label ID="LBserviziAttivi_t" Runat=server CssClass="Titolo_CampoSmall">Servizi:</asp:Label>
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow>
					<asp:TableCell>
						<asp:Table HorizontalAlign=left Runat=server ID="TBLtipoComunita" GridLines=none >
							<asp:TableRow>
								<asp:TableCell>
									<asp:Label ID="LBorganizzazione_t" Runat=server CssClass="Titolo_campoSmall">Organizzazione:</asp:Label>
								</asp:TableCell>
								<asp:TableCell>
									<asp:DropDownList ID="DDLorganizzazione" AutoPostBack=True CssClass="Testo_campoSmall" Runat=server ></asp:DropDownList>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow>
								<asp:TableCell ColumnSpan=2 CssClass="nosize0" Height=10px>&nbsp;</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow>
								<asp:TableCell ColumnSpan=2>
									<table>
									<asp:Repeater id="RPTservizio" Runat="server">
										<HeaderTemplate>
											<tr>
												<td class="Header_Repeater" width=200px align=left nowrap="nowrap">
													<asp:Label ID="LBservizio_t" Runat=server CssClass="titolo_campoSmall">Servizio:&nbsp;&nbsp;</asp:Label>
												</td>
												<td align=center class="Header_Repeater" width=70px>
													<asp:Label ID="LBassociato_t" Runat=server CssClass="titolo_campoSmall">Associa</asp:Label>
												</td>
												<td align=center class="Header_Repeater" width=130px>
													<asp:Label ID="LBattiva_t" Runat=server CssClass="titolo_campoSmall">Attiva di Default</asp:Label>
												</td>
											</tr>
										</HeaderTemplate>
										<ItemTemplate>
											<tr>
												<asp:Label id="LBsrvz_ID" Text='<%# Databinder.eval(Container.DataItem, "SRVZ_ID")%>' runat="server" Visible=false />
												<asp:Label id="LBLKST_id" Text='<%# Databinder.eval(Container.DataItem, "LKST_id")%>' runat="server" Visible=false />
												<td align=left width=200px nowrap="nowrap">
													<asp:Label id="LBservizio" Text='<%# Databinder.eval(Container.DataItem, "SRVZ_Nome")%>' runat="server" Visible=true CssClass=ROW_TD_Small/>
												</td>
												<td align=center width=70px>
													<asp:CheckBox ID="CBXservizioAssociato" Runat=server Text="Si" CssClass="ROW_TD_Small" Checked=<%# DataBinder.Eval(Container.DataItem, "oCheckAssociato") %> ></asp:CheckBox>
												</td>
												<td align=center width=130px>
													<asp:CheckBox ID="CBXservizioAttivato" Runat=server Text="Si" CssClass="ROW_TD_Small" Checked=<%# DataBinder.Eval(Container.DataItem, "oCheckDefault") %> ></asp:CheckBox>
												</td>
											</tr>
										</ItemTemplate>
									</asp:Repeater>
									</table>
								</asp:TableCell>
							</asp:TableRow>
						</asp:Table>
					</asp:TableCell>
				</asp:TableRow>
			</asp:Table>
			<asp:Table ID="TBLpermessi" Runat="server" Visible="False" HorizontalAlign="Left">
            <asp:TableRow>
								<asp:TableCell>
                <div>
                    <div style="float:left;">
                        <asp:label ID="LBorganizations_t" CssClass="Titolo_campo" runat="server" AssociatedControlID="DDLorganization">Organization:</asp:label>
                        <asp:DropDownList ID="DDLorganization" AutoPostBack="true" Runat="server"></asp:DropDownList>
                    </div>
                    <div style="float:left; padding-left:5px;">
                    <asp:label ID="LBmodules_t" CssClass="Titolo_campo" runat="server" AssociatedControlID="DDLmodules">Module:</asp:label>
                    <asp:DropDownList ID="DDLmodules" AutoPostBack="true" Runat="server"></asp:DropDownList>
                    </div>
                </div>
                <br />
                <div style="clear:both;">
                    <asp:Repeater ID="RPTtemplatePermission" runat="server">
                        <HeaderTemplate>
                        </HeaderTemplate>
                        <itemtemplate>
                             <table class="table" border="1" cellpadding="0" cellspacing="0">
                                <thead>
                                    <tr class="ROW_header_Small FirstRow">
                                        <th class="FirstCellRow">
                                            <asp:Label ID="LBcommunityType_t" runat="server">Community</asp:Label>
                                        </th>
                                        <asp:Repeater ID="RPTpermissionName" runat="server" DataSource="<%#Container.DataItem.Columns%>" OnItemDataBound="RPTpermissionName_ItemDataBound">
                                            <ItemTemplate>
                                                <th class="<%#GetBackground(Container.ItemType)%>">
                                                   <asp:button ID="BTNpermission" runat="server" CausesValidation="false" Text="<%#Container.Dataitem.Nome %>" />
                                                </th>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <th>
                                            <asp:Label ID="LBactions" runat="server">A</asp:Label>
                                        </th>
                                    </tr>        
                                </thead>
                                <tbody>
                                <asp:Repeater ID="RPTtypes" runat="server" DataSource="<%#Container.DataItem.Rows%>" OnItemDataBound="RPTtypes_ItemDataBound">
                                    <ItemTemplate>
                                    <tr>
                                        <td class="FirstColumn">
                                            <asp:Literal ID="LTidRole" runat="server" Visible="false" Text="<%#Container.DataItem.RoleId%>" />
                                             <asp:Label ID="LBroleName" runat="server" Text="<%#Container.DataItem.RoleName%>"></asp:Label>
                                        </td>
                                        <asp:Repeater ID="RPTpermissionValue" runat="server" datasource="<%#Container.DataItem.Positions%>" OnItemDataBound="RPTpermissionValue_ItemDataBound">
                                            <ItemTemplate>
                                                <td class="<%#GetBackground(Container.ItemType)%>">
                                                    <asp:Literal ID="LTposition" runat="server" Visible="false" Text="<%#Container.DataItem.IdPosition%>"/>
                                                    <asp:PlaceHolder ID="PLHpermission" runat="server"></asp:PlaceHolder>
                                                </td>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <td>
                                            <asp:Literal ID="LTempty" runat="server">&nbsp;</asp:Literal>
                                            <asp:button ID="BTNsetAll" runat="server" Text="Sel/Desel"></asp:button>
                                        </td>
                                </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                 </tbody>
                            </table>
                        </itemtemplate>

                        <FooterTemplate>
                   
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
			<%--	<asp:TableRow>
					<asp:TableCell>
						<asp:Label ID="LBorganizzazionePermessi_t" Runat=server CssClass="Titolo_campoSmall">Organizzazione:</asp:Label>
					</asp:TableCell>
					<asp:TableCell>
						<asp:DropDownList ID="DDLorganizzazionePermessi" AutoPostBack=True CssClass="Testo_campoSmall" Runat=server ></asp:DropDownList>
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow>
					<asp:TableCell>
						<asp:Label ID="LBserviziPermessi_t" Runat=server CssClass="Titolo_campoSmall">Organizzazione:</asp:Label>
					</asp:TableCell>
					<asp:TableCell>
						<asp:DropDownList ID="DDLserviziPermessi" AutoPostBack=True CssClass="Testo_campoSmall" Runat=server ></asp:DropDownList>
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow>
					<asp:TableCell ColumnSpan=3 CssClass="nosize0" Height=10px>&nbsp;</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow>
					<asp:TableCell ColumnSpan=2>
						<asp:Table Runat=server HorizontalAlign=left ID="TBLpermessiRuoli" GridLines=Both CellSpacing=0 CellPadding=2>
																					
						</asp:Table>
					</asp:TableCell>
				</asp:TableRow>--%>
                    </asp:TableCell>
                </asp:TableRow>
			</asp:Table>
		</asp:TableCell>
		<asp:TableCell Width=5px>&nbsp;</asp:TableCell>
	</asp:TableRow>
</asp:Table>
<asp:validationsummary id="VLDSum" runat="server" HeaderText="Attenzione! Sono state rilevate delle imprecisioni nella compilazione del form. Controlla i valori inseriti in corrisponsenza degli *"
	ShowSummary="false" ShowMessageBox="true" DisplayMode="BulletList"></asp:validationsummary>
<input type=hidden id="HDNtpcm_id" runat=server NAME="HDNtpcm_id"/>
<input type=hidden id="HDNcategoriaAssociate" runat=server NAME="HDNcategoriaAssociate"/>
<input type=hidden id="HDNmodelliAssociati" runat=server NAME="HDNmodelliAssociati"/>
<input type=hidden id="HDNruoliAssociati" runat=server NAME="HDNruoliAssociati"/>
<input type=hidden id="HDNruoloNONdeassociabili" runat=server name="HDNruoloNONdeassociabili"/>
<input type=hidden id="HDNsottoComunitaAssociate" runat=server NAME="HDNsottoComunitaAssociate"/>
<input type=hidden id="HIDcheckbox" runat=server NAME="HIDcheckbox"/>
