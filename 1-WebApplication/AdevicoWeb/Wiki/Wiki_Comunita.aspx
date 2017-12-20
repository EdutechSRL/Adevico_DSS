<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="Wiki_Comunita.aspx.vb" Inherits="Comunita_OnLine.Wiki_Comunita" ValidateRequest="false" %>
<%@ Register TagPrefix="rade" Namespace="Telerik.WebControls" Assembly="RadEditor.Net2" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLsearch" Src="../UC/UC_SearchCommunityByService.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLMessage" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %> 
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register Src="~/Modules/Common/Editor/UC_Editor.ascx" TagName="CTRLeditor" TagPrefix="CTRL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language=JavaScript>
	        function CheckAll(checkAllBox, fieldName) {
	            var frm = document.aspnetForm;
	            var actVar = checkAllBox.checked;
	            for (i = 0; i < frm.length; i++) {
	                e = frm.elements[i];
	                if (e.type == 'CBX_selected' && e.name.indexOf(fieldName) != -1)
	                    e.checked = actVar;
	            }
	        }
	        
	        
	        
        </script>

    <link href="../Graphics/Modules/Wiki/wiki.new.css?v=201508040900lm" rel="Stylesheet" />    
    <link href="./../dhtmlcentral.css" rel="STYLESHEET" type="text/css" />
    <link href="./../RadControls/Editor/Skins/Default/Controls.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
	<div id="DVmenu" class="DVmenu">
        <asp:CheckBox ID="CBX_SezVisEliminate" runat="server" AutoPostBack="True" 
            Text="Vedi sezioni e topic eliminati" />
        &nbsp;
		<asp:Button ID="BTN_ModWiki" Text="Modifica Wiki" CssClass="Pulsante" runat="server" />
		<asp:Button ID="BTN_AddSezione" runat="server" CssClass="Pulsante"  Text="Nuova Sezione" />
		<asp:Button ID="BTN_Torna" runat="server" CssClass="Pulsante"  Text="Torna alla sezione" />
		<asp:Button ID="BTN_Addtopic" Text="Nuovo Topic" CssClass="Pulsante" runat="server" />
		<asp:Button ID="BTN_Import" Text="Importa topic" CssClass="Pulsante" runat="server" />
		<asp:Button ID="BTN_Home" Text="Wiki Home" CssClass="Pulsante" runat="server" 
            Visible="False" />
	</div>
	<div class="content">
        <asp:Panel ID="PNL_NoPermessi" runat="server">
            <div class="messages">
            <div class="message error">
                <span class="icons"><span class="icon">&nbsp;</span></span>
                <asp:Label ID="LBL_NoPermessi" runat="server"> Non si dispone dei permessi necessari per visualizzare la pagina. </asp:Label>
            </div>
        </div>
			
		</asp:Panel>
							   
		<asp:Panel ID="PNL_InfoImport" runat="server" >
			
            <asp:Label 
                ID="LBL_proce" runat="server" Text="Procedura di importazione guidata TOPIC nella sezione corrente"></asp:Label>
            &nbsp;(<asp:Label 
                ID="LBL_NomeSezione" runat="server" cssclass="Titolo_campo"> </asp:Label>
            )
            <asp:Label ID="LBL_proce1" runat="server" Text="della comunità corrente"></asp:Label>
                (<asp:Label ID="LBL_NomeComunita" runat="server" 
                cssclass="Titolo_campo"> </asp:Label>
            )
            <ul class="steps">
                <li class="step">
                    <asp:Label 
			            ID="LBL_PassoUno" 
			            runat="server"
			            cssclass="Titolo_campo">Passo 1: Seleziona la comunita da cui importare i topic desiderati</asp:Label> 
                </li>
                <li class="step">
                    <asp:Label ID="LBL_PassoDue" runat="server" cssclass="Titolo_campo">Passo 2: Seleziona i topic che desideri importare e premi IMPORTA</asp:Label>
                </li>
            </ul>

		</asp:Panel>
							    
		<asp:Panel ID="PNL_search" runat="server" >
						                            
            <div class="fieldobject search clearfix">
                <div class="fieldrow left">
                    &nbsp;
                </div>
                <div class="fieldrow right">
                    <asp:Label 
						ID="LBL_ricerca" 
						runat="server" 
						Text="Cerca: "
						CssClass="fieldlabel" AssociatedControlID="DDL_ricerca"></asp:Label>
                    <asp:DropDownList ID="DDL_ricerca" runat="server">
                        <asp:ListItem>(Topic)</asp:ListItem>
                        <asp:ListItem Selected="True">Inizia per</asp:ListItem>
                        <asp:ListItem>Finisce per</asp:ListItem>
                        <asp:ListItem>Contiene</asp:ListItem>
                    </asp:DropDownList> 
                    <asp:TextBox ID="TXB_search" runat="server" CssClass="" MaxLength="60"> </asp:TextBox>
                    <asp:Button ID="BTN_search" runat="server"  CommandArgument="TXB_search.text" 
                        CommandName="Cerca" CssClass="PulsanteFiltro" Height="21px" Text="Cerca" />
                </div>
            </div>
        </asp:Panel>
                                                
        <asp:Panel ID="PNL_ListaComunita" runat="server">
                    <CTRL:CTRLsearch   id="CTRLcommunity" runat="server" SelectionMode="single" AllowMultipleOrganizationSelection="false"  AllowCommunityChangedEvent="true"    />
                </asp:Panel>

        <asp:Panel ID="PNL_ViewImport" runat="server">
            <asp:DataList ID="DLS_topicsImport" UseAccessibleHeader="true" runat="server" CellPadding="4" DataKeyField="id" 
                        Width="100%" ShowFooter="true">
                        <AlternatingItemStyle CssClass="ROW_Alternate_Small" />
                        <HeaderStyle CssClass="" />
                        <ItemStyle CssClass="ROW_Normal_Small" Height="22px" />
                        <SelectedItemStyle CssClass="ROW_Evidenziate_Small" />
                 
                        <HeaderTemplate>
                <table class="table light fullwidth wikitable importtopics">
                    <tr class="tableheader tablerow">
                        <th class="actions">
                        </th>
                        <th class="topic">
                            <asp:Label ID="LBL_intVoce1" runat="server" Text="Voce"></asp:Label>
                        </th>
                        <th class="section">
                            <asp:Label ID="LBL_intSezione1" runat="server" Text="Sezione"></asp:Label>
                        </th>
                        <%--<td>
                            <asp:Label ID="Label4" runat="server" ForeColor="White" Text="Comunità"></asp:Label>
                        </td>--%>
                    </tr>                
            </HeaderTemplate>
                        <ItemTemplate>                
                    <tr class="tablerow">
                        <td class="actions">
                            <asp:CheckBox ID="CBX_selected" runat="server" />
                                </a>
                        </td>
                        <td class="topic">
                            <span class="Titolo_Campo" style="text-align: left;" style="left">
                            <a href='<%=Me.BaseUrl %>wiki/Wiki_Comunita.aspx?id=<%#DataBinder.Eval(Container.DataItem, "ID")%>'>
                            <%#DataBinder.Eval(Container.DataItem, "Nome")%></a> </span>
                            </td>
                        <td class="section">
                                    <span class="Testo_Campo" style="text-align:left;" width="right">
                                    <a name='<%#DataBinder.Eval(Container.DataItem, "ID")%>'>
                                    <%#DataBinder.Eval(Container.DataItem, "Sezione.NomeSezione")%></a> </span>
                                </td>
                            <%--<td>
                                                    
                                <asp:Button ID="BTNImport" runat="server" CommandName="Importa" Text="Importa"
                                        CssClass="PulsanteFiltro" Height="21px" Visible="true" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID".toString)%>'/>
                            </td>--%>
                                                        
                                <%-- <td>
                                    <span class="Testo_Campo" style="text-align:left;" width="right">
                                    <a name='<%#DataBinder.Eval(Container.DataItem, "ID")%>'>
                                    <%#DataBinder.Eval(Container.DataItem, "Comunita")%></a> </span>
                                </td>--%>
                            
                        </tr>
                    
                </ItemTemplate>
                        <FooterTemplate>
</table>                                        
                                        
                                        
                        </FooterTemplate>
                    </asp:DataList>
            <div>
                <table width="100%">
                    <tr>
                        <td>
                            <asp:button
                            id="BTN_selTutti"
                            runat="server"
                            CssClass="PulsanteFiltro"
                            Height="21px"
                            CommandName="Seleziona tutti"
                            Visible="false" 
                            Text="Seleziona tutti" CommandArgument="oTopic.ID.ToString"  >
                    </asp:button>
                            <asp:button
                                    id="BTN_deselTutti"
                                    runat="server"
                                    CssClass="PulsanteFiltro"
                                    Height="21px"
                                    CommandName="Deseleziona tutti"
                                    Visible="false" 
                                    Text="Deseleziona tutti" CommandArgument="oTopic.ID.ToString" >
                    </asp:button>
                        </td >
                        <td align="right">
                            <asp:button
                                    id="BTN_importa"
                                    runat="server"
                                    CssClass="PulsanteFiltro"
                                    Height="21px"
                                    CommandName="Importa"
                                                   
                                    Text="Importa" CommandArgument="oTopic.ID.ToString" >
                            </asp:button>
                        </td>
                        <td align="right">
                            <asp:ImageButton ID="IMG_indietro" runat="server" CssClass="PulsanteFiltro" ImageUrl="../questionari/img/indietro.gif" AlternateText="Pagina precedente"/>
                        </td>    
                        <td align="right">
                            <asp:ImageButton ID="IMG_avanti" runat="server" CssClass="PulsanteFiltro" ImageUrl="../questionari/img/avanti.gif" AlternateText="Pagina successiva"/>
                        </td>
                    </tr>
                </table>  
            </div>
        </asp:Panel>
                
        <asp:Panel ID="PNL_ViewTopic" runat="server" CssClass="Editor">
            <div class="fieldobject viewtopic">
                <div class="fieldrow header">
                    <div class="title">
                        <h3><asp:Label ID="LBL_TitoloTopicView" runat="server" Text="" class="Titolo_Campo"></asp:Label></h3>
                    </div>
                    <div class="details">
                        <asp:Label ID="LBL_autore" runat="server" Text="LBL_autore"></asp:Label>
                        <asp:Label ID="LBL_date" runat="server" Text="LBL_autore"></asp:Label>
                    </div>
                </div>                
                <div class="fieldrow content">
                    <div class="renderedtext">
                            <asp:Label ID="LBL_TestView" runat="server" Text="TESTO DI PROVA"></asp:Label> <!-- CssClass="Editor"-->
                        </div>
                </div>
                <div class="fieldrow footer">
                    <div class="commands clearfix">
                        <div class="left">&nbsp;</div>
                        <div class="right">
                            <asp:button
                            id="BTN_EditView"
                            runat="server"
                            CssClass="PulsanteFiltro"
                            Height="21px"
                            CommandName="Modifica"
                            Visible="false" Text="Modifica" CommandArgument="oTopic.ID.ToString" >
                        </asp:button>
                        </div>
                    </div>
                </div>
            </div>
            
        </asp:Panel>

		<asp:Panel ID="PNL_NoWiki" runat="server">
            <div class="messages" runat="server" id="DIV_LBL_NoWiki">
                <div class="message info">
                    
                    <asp:Label 
				        ID="LBL_NoWiki" 
				        runat="server"
				        cssclass="Testo_campo"> Nessuna Wiki inserita </asp:Label>
                </div>
            </div>

			<asp:Panel ID="PNL_NoWiki_Add" runat="server">
				<div class="messages" runat="server" id="DIV_LBL_NoWikiADD">
                    <div class="message info">
                        
                        <asp:Label 
					        ID="LBL_NoWikiADD" 
					        runat="server"
					        cssclass="Testo_campo"> Per inserire una nuova Wiki, mettere il nome e cliccare su Salva. </asp:Label>
                    </div>
                </div>
                <div class="fieldobject wikiadd">
                    <div class="fieldrow">
                        <asp:Label 
					ID="LBL_WikiAdd_Nome_t" 
					runat="server"
					cssclass="fieldlabel"
                            AssociatedControlID="TXB_WikiAdd_Nome"
					> Nome Nuova Wiki: </asp:Label>
                        <asp:TextBox 
					MaxLength="80"
						Columns="80"
					ID="TXB_WikiAdd_Nome"
					runat="server"> </asp:TextBox>
                    </div>
                    <div runat="server" id="DIVtest" class="fieldrow" visible="false">
                        <asp:Label ID="LBL_WikiAdd_showauthors" runat="server" CssClass="fieldlabel">*Authors:</asp:Label>
                        <asp:CheckBox ID="CHB_WikiAdd_showauthors" runat="server" Text="*Hide Topic authors" />
                    </div>
                    <div class="fieldrow buttons">
                        <asp:Button ID="BTN_AddWiki" runat="server" CssClass="PulsanteFiltro" 
                    text="Salva" />
				<asp:Button 
					id="Btn_AnnWiki"
					CssClass="PulsanteFiltro" 
					runat="server" 
					text="Annulla"
				/>
                    </div>
                </div>

			</asp:Panel>
		</asp:Panel>

        <asp:Panel ID="PNL_Wiki" runat="server">
			<div class="wikicontainer clearfix container_12" >	
                
                <div class="header grid_12 alpha omega clearfix">
                    <asp:Panel id="PNL_DeleteTopic" runat="server" Visible="false">
                        <div class="wikimessage">
                            <CTRL:CTRLMessage runat="server" ID="UC_Message" Visible="true" />                            
                            <div runat="server" id="DIV_deletetopic" visible="false">
                                <div class="links">
                                    <asp:Label runat="server" ID="LBL_deletetopicmsg"></asp:Label>                        
                                </div>
                                <asp:Button runat="server" ID="BTN_delete" CommandArgument="" CommandName="delete" Text="*Confirm Delete" />
                                <asp:Button runat="server" ID="BTN_cancel" CommandArgument="" CommandName="cancel" Text="*Cancel" />
                            </div>
                            <div runat="server" id="DIV_missinglinks" visible="false">
                                <div class="links">
                                    <a href="" class="wikilink missing">Link</a>
                                </div>
                                <asp:Button runat="server" ID="BTN_keepedit" CommandArgument="" CommandName="" Text="*Keep editing" />
                                <asp:Button runat="server" ID="BTN_backtolist" CommandArgument="" CommandName="" Text="*Back to list" />
                            </div>
                        </div>
                        
                    </asp:Panel>

                    <asp:Panel ID="PNL_InfoSezione" runat="server" CssClass="InfoSezione">	
                        <div class="sectionintro">
                        <div class="fieldobject section">
                            <div class="fieldrow">
                                <asp:Label ID="LBL_ElencoSezioneNome_t" CssClass="fieldlabel hide" AssociatedControlID="LBL_ElencoSezioneNome" runat="server">
			                        Sezione:
			                    </asp:Label>
                                <h2 class="sectiontitle"><asp:Label 
			                    ID="LBL_ElencoSezioneNome" 
			                    runat="server"> </asp:Label></h2>
                            </div>
                            <div class="fieldrow description" runat="server" id="DIVdescription">
                                <asp:Label 
			                    id="LBL_ElencoSezioneDescrizione_t" 
                                cssclass="Testo_campoSmall hide" 
                                runat="server">
                                    Modificata il:
                            </asp:Label>
                                <div class="renderedtext sectiondescription" runat="server" id="DIVsectiondescription">
			                        <asp:Label
			                            id="LBL_ElencoSezioneDescrizione"
			                            runat="server"
			                            cssclass="descriptionbody">
                                    </asp:Label>                                
                                
                                    </div>
                            </div>
                            <div class="fieldgroup clearfix">

                            
                            <div class="fieldrow left">
                                <span runat="server" id="SPN_SectionAuthor">
	                    	<asp:Label 
			                    id="LBL_ElencoSezionePersona_t" 
                                cssclass="Testo_campoSmall" 
                                runat="server">
                                da
                            </asp:Label>
			                <asp:Label
			                    id="LBL_ElencoSezionePersona"
			                    runat="server"
			                    cssclass="Titolo_campoSmall">
                            </asp:Label></span>                
                            <asp:Label ID="LBL_SezioneElininata" runat="server" CssClass="Titolo_campo_Rosso" Visible="false" Text="<br/>Attenzione questa sezione è attualmente eliminata!"></asp:Label>
                            </div>
                                <div class="fieldrow right">
                                    <div class="LinkTopic" >			                
				            <asp:Button ID="BTN_ModSezione" runat="server" CssClass="PulsanteFiltro" 
                                Text="Modifica Sezione"  />
                            <asp:Button ID="BTN_DelSezione" runat="server" CssClass="PulsanteFiltro" 
                                Text="Cancella Sezione" />
                            <asp:Button ID="BTN_RecSezione" runat="server" CssClass="PulsanteFiltro" visible="false"
                                Text="Recupera Sezione" />
                        </div>
                                </div>
                                </div>
                        </div>                        
			            
			            
                        </div>		            
			        </asp:Panel>
                </div>
                		    
			    <div class="navigator grid_3 alpha">
                    <div class="inner">
			        <asp:Panel ID="PNL_NavigatoreNoTopic" runat="server">
			            <div class="TestoInfo">
                            <asp:label ID="LBL_Nav_NoSezione" runat="server"> Nessuna sezione presente </asp:label>
                        </div>
                    </asp:Panel>          
                    <asp:Panel ID="PNL_Navigatore" runat="server">                        
                        <asp:Label ID="LBL_Navigatore_t" runat="server" CssClass="Titolo_campo"> Elenco sezioni </asp:Label>                        
                        <div class="navigatorwrapper">
                            <ul class="navigatoritems">
                            <asp:Repeater ID="RPT_LinkNavigatore" runat="server">
                                <ItemTemplate>
                                    <li class="navigatoritem <%# Deleted(Container.DataItem.IsDeleted)%>">
                                        <asp:LinkButton ID="LNK_VoceNavi" runat="server" CssClass=""></asp:LinkButton>                                                                                                
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                            </ul>
                        </div>
                    </asp:Panel>
                    </div>
			    </div>
			    <div class="wikicontent grid_9 omega" >			                
			        <asp:MultiView ID="MLV_Contenuto" runat="server">
			            <asp:View ID="V_SezioneNo" runat="server">
			                <div class="TestoInfo">
			                    <asp:Label 
			                        ID="LBL_Con_SezioneNO" 
			                        runat="server" 
			                        CssClass="Testo_campoSmall"> Nessuna sezione presente </asp:Label>
                            </div>
			            </asp:View>
                        <asp:View ID="V_SezioneAddMod" runat="server">				                
                            <div class="fieldobject sezioneaddmod">
                                <div class="fieldrow">
                                    <asp:Label ID="Lbl_NASezione_Nome_t"  runat="server"  Text="Nome sezione:" AssociatedControlID="Txb_NASezione_Nome" cssclass="fieldlabel"></asp:Label>
                                    <asp:TextBox ID="Txb_NASezione_Nome" runat="server" MaxLength="50" Columns="50"></asp:TextBox>
                                </div>
                                <div class="fieldrow">
                                    <asp:Label ID="Lbl_NASezione_Descrizione_t" runat="server" Text="Descrizione : " cssclass="fieldlabel" AssociatedControlID="TXB_NASezione_Descrizione"></asp:Label>
                                    <asp:TextBox ID="TXB_NASezione_Descrizione" runat="server" MaxLength="100" Columns="80" Visible="false"></asp:TextBox>
                                </div>
                                <div class="fieldrow">
                                    
                                    <CTRL:CTRLeditor id="CTRLeditorDescription" runat="server" ContainerCssClass="containerclass" 
                                    LoaderCssClass="loadercssclass" EditorHeight="450px" EditorWidth="660px"  AllAvailableFontnames="true"
                                    AutoInitialize="true" ModuleCode="SRVwiki" >
                                </CTRL:CTRLeditor>
                                </div>
                                <div class="fieldrow">
                                    <label class="fieldlabel">&nbsp;</label>
                                    <div class="inlinewrapper">
                                        <asp:CheckBox ID="CBX_NASezione_IsPubblica_t" runat="server" 
                                            cssclass="Testo_campo" Text="Pubblica " />
                                        <asp:CheckBox ID="CBX_NASezione_IsDefault" runat="server" Text="Di default" cssclass="Testo_campo"/>
                                    </div>
                                </div>
                                <div class="fieldrow buttons">
                                    <asp:button 
                                    id="BTN_Sezione_Salva" 
                                    accessKey="S" 
                                    runat="server" 
                                    CssClass="PulsanteFiltro" 
                                    Height="21px"  
                                    Text="Salva">
                                </asp:button>
                                <asp:button 
                                    id="BTN_Sezione_Annulla" 
                                    accessKey="Q" 
                                    runat="server" 
                                    CssClass="PulsanteFiltro"
                                    Height="21px" 
                                    text="Annulla"
                                    >
                                </asp:button>
                                </div>
                            </div>			                
                                                                
			            </asp:View>
			            <asp:View ID="V_TopicNo" runat="server">
                            <div class="messages">
                            <div class="message info">
			                        <asp:Label 
			                            ID="LBL_Test_titolo" 
			                            runat="server" 
			                            CssClass="Testo_campoSmall"> Nessun Topic presente </asp:Label>
                                <br />
                                    <asp:Label 
                                        ID="LBL_test_testo" 
                                        runat="server" 
                                        CssClass="Testo_campoSmall"> Premere "ADD TOPIC" per aggiungere un nuovo topic. </asp:Label>	
                                
                            </div>
                        </div>
			                
			            </asp:View>
			            <asp:View ID="V_TopicElenco" runat="server">			                
			                <asp:DataList 
			                    CssClass="datagridwiki"
			                    ID="DLS_topics"
			                    runat="server"
			                    CellPadding="4"
			                    DataKeyField="id"
			                    Width="100%"
                                ShowHeader="true"
                                ShowFooter="true"
                                >
			                    <AlternatingItemStyle CssClass="ROW_Alternate_Small"></AlternatingItemStyle>
		                        <HeaderStyle CssClass=""></HeaderStyle>
		                        <ItemStyle CssClass="" Height="22px"></ItemStyle>
		                        <SelectedItemStyle CssClass="" />
<%--                                    ForeColor="#333333"
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />

                                <AlternatingItemStyle BackColor="#EFF3FB" />
                                <ItemStyle BackColor="White" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />--%>
                                <HeaderTemplate>
                                    <table class="table fullwidth wikitable topics">
                                        <tr class="hide">
                                            <th class="initial">
                                                &nbsp;
                                            </th>
                                            <th class="spacer">
                                                &nbsp;
                                            </th>
                                            <th class="content">
                                                <asp:Label ID="LBL_intest1" runat="server"  Text="Voce"></asp:Label>
                                            </th>
                                            <th class="actions" colspan="3"></th>
                                            <%--<td width="180px">
                                                <asp:Label ID="Label1" runat="server" ForeColor="White" Text="Autore"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label2" runat="server" ForeColor="White" Text="Ultima modifica"></asp:Label>
                                            </td>--%>
                                        </tr>
                                    
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%--<asp:Panel runat="server" ID="PNL_DSL_item">--%>
                                    
                                        <asp:Label ID="LBL_separatore" runat="server" 
                                            Text="&lt;tr class='tablerow separator'&gt;&lt;td colspan='6' &gt;&lt;hr/&gt;&lt;/td&gt;&lt;/tr&gt;" 
                                            Visible="fALSE" />
                                    
                                    </td>
                                        <tr class="tablerow topic <%# Deleted(Container.DataItem.IsCancellato)%>">
                                            <td class="initial">
                                                <asp:Label ID="LBL_Iniziale" runat="server" Text="A" 
                                                    Visible="true" />
                                            </td>
                                            <td class="spacer">
                                                <%--<a href="<%=Me.BaseUrl %>wiki/wiki_Home.aspx?id=<%#DataBinder.Eval(Container.DataItem, "ID")%>"><img src="../images/search.gif" alt="Visualizza voce:<%#DataBinder.Eval(Container.DataItem, "Nome")%>"/> </a>--%>
                                            </td>
                                            <td class="content">
                                                <span class="Titolo_Campo" style="text-align: left;">
                                                <a href='<%=Me.BaseUrl %>wiki/Wiki_Comunita.aspx?id=<%#DataBinder.Eval(Container.DataItem, "ID")%>'>
                                                <asp:Label ID="LBL_topic" runat="server" 
                                                    Text='<%#DataBinder.Eval(Container.DataItem, "Nome")%>' />
                                                    <asp:Label ID="LBL_iscancelled" runat="server"  CssClass="Titolo_campo_Rosso"
                                                    Text="(Eliminata)" Visible='false'/><%--<%#DataBinder.Eval(Container.DataItem, "IsCancellato")%> --%>
                                                </a></span>
                                                        
                                                        
                                                <td class="actions"  colspan="3" align="right">
                                                    <span class="onhover">

                                                    
                                                    <asp:Button ID="BTNEdit"  runat="server" CommandName="Modifica"  Text="Modifica"
                                                        CssClass="PulsanteFiltro" Visible="true" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID".toString)%>'/>
                                                            
                                                    <asp:Button ID="BTNCrono" runat="server" CommandName="Cronologia" Text="Cronologia"
                                                        CssClass="PulsanteFiltro"  Visible="true" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID".toString)%>'/>
                                                            
                                                    <asp:Button ID="BTNRip" runat="server" CommandName="Ripristina" Text="Ripristina" Visible='<%#DataBinder.Eval(Container.DataItem, "IsCancellato")%>'
                                                        CssClass="PulsanteFiltro" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID".toString)%>'/>
                                                            
                                                    <asp:Button ID="BTNDel" runat="server" CommandName="Elimina" Text="Elimina" Visible='<%#Not DataBinder.Eval(Container.DataItem,  "IsCancellato")%>'
                                                        CssClass="PulsanteFiltro"   CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID".toString)%>'/>
                                                    </span>        
                                                                
                                                </td>
                                                            
                                                </td>
                                            </tr>
                                            <tr class="tablerow info <%# Deleted(Container.DataItem.IsCancellato)%>">
                                                <td></td><td></td>
                                                <td colspan="2" class="details">
                                                        <span class="Testo_Campo" style="text-align:left;" width="right" runat="server" id="SPN_Author">
                                                            <strong><%#DataBinder.Eval(Container.DataItem, "Persona.anagrafica")%></strong> 
                                                        </span>
                                                        <span class="Testo_Campo" style="text-align:left;" width="right">
                                                            <%#DataBinder.Eval(Container.DataItem, "DataModifica".toString)%> </span>
                                                        </td>
                                                        
                                            </tr>
                                        
                                        <%--</asp:Panel>--%>
                                    </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                                </asp:DataList>
			                <br />
			            </asp:View>
			            <asp:View ID="V_TopicAddMod" runat="server">
                            <div class="fieldobject">
                                <div class="fieldrow">
                                    <asp:Label ID="LBL_ModTopic_Titolo_t" runat="server" Text="Titolo topic :" cssclass="fieldlabel" AssociatedControlID="TXB_TitoloTopic"></asp:Label>
                                    <asp:TextBox ID="TXB_TitoloTopic" runat="server" cssclass="Titolo_campo" Columns="60" MaxLength="100"></asp:TextBox>
                                    
                                </div>
                                <div class="fieldrow">
                                    <asp:CheckBox ID="CBX_Topic_IsPubblico" runat="server" Text="Topic pubblico" />
                                </div>
                                <div class="fieldrow">
                                    <CTRL:CTRLeditor id="CTRLeditor" runat="server" ContainerCssClass="containerclass" 
                                    LoaderCssClass="loadercssclass" EditorHeight="450px" EditorWidth="690px"  AllAvailableFontnames="true"
                                    AutoInitialize="true" ModuleCode="SRVwiki" >
                                </CTRL:CTRLeditor>
                                </div>
                                <div class="fieldrow buttons">
                                    <asp:button id="BTNSaveQuit" accessKey="Q" runat="server" CssClass="PulsanteFiltro" Height="21px" CommandName="ripristina" Text="Salva ed Esci"></asp:button>
                                <asp:button id="BTNSaveContinua" accessKey="S" runat="server" CssClass="PulsanteFiltro" Height="21px" CommandName="elimina" text="Salva e continua"></asp:button>
                                <asp:button id="BTNAnnulla" runat="server" CssClass="PulsanteFiltro" Height="21px" CommandName="elimina" text="Annulla"></asp:button>
                                </div>
                            </div>
			            </asp:View>
			            <asp:View ID="V_TopicCrono" runat="server">		
                            <div class="messages" runat="server" id="DIV_Lbl_NoCronologia">
                                <div class="message info">
                                    <asp:Label ID="Lbl_NoCronologia" runat="server" CssClass="Testo_campoSmall">Cronologia non presente.</asp:Label>
                                </div>
                            </div>	                
			                
                            
                                <asp:Repeater runat="server" ID="DL_TopicCrono">
                                    <HeaderTemplate>
                                        <ul class="historyitems">
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <li class="historyitem">
                                        <div class="content">
                                            <div class="fieldrow header">
                                                <div class="clearfix">
                                                    <div class="left title">
                                                        <span class="Titolo_Campo"><%#DataBinder.Eval(Container.DataItem, "Nome")%></span>
                                                    </div>
                                                    <div class="right details">
                                                        <span class="Testo_Campo">(<%#DataBinder.Eval(Container.DataItem, "DataModifica")%> 
                                                            -
                                                        <asp:Label ID="Lbl_Crono_Anagrafica" class="Testo_Campo" runat="server"></asp:Label>)</span>
                                                    </div>
                                                </div>                                                
                                            </div>
                                            <div class="fieldrow content">
                                                <span class="Testo_Campo"><div class="renderedtext"><%#DataBinder.Eval(Container.DataItem, "Contenuto")%></div>
                                                </span>
                                            </div>
                                            <div class="fieldrow footer">
                                                <div class="clearfix">
                                                    <div class="left">&nbsp;</div>
                                                    <div class="right">
                                                <asp:Button ID="BTNRipristina" runat="server" accessKey="E" 
                                                    CommandName="ripristina" CssClass="PulsanteFiltro" Height="21px" 
                                                    Visible="false" />
                                                <asp:Button ID="BTNElimina" runat="server" accessKey="E" CommandName="elimina" 
                                                    CssClass="PulsanteFiltro" Height="21px" Visible="false" />
                                                        </div>
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </ul>
                                    </FooterTemplate>                                    
                                </asp:Repeater>

			                
                                
			            </asp:View>	                    
                        <asp:View ID="V_TopicShow" runat="server">
			                    &nbsp;
			                        
			            </asp:View>
			            <asp:View runat="server" ID="V_TopicSearched" >
                            <div class="topicsearched">
			                <asp:Label ID="LBL_RisultatoRicerca" runat="server" CssClass="Titolo_campo"> Elenco dei topic trovati: </asp:Label>
                            <asp:DataList ID="DLS_result" runat="server" CellPadding="4" DataKeyField="id" 
                                Width="100%" UseAccessibleHeader="true" ShowFooter="true">
                                <AlternatingItemStyle CssClass="ROW_Alternate_Small"></AlternatingItemStyle>
		                        <HeaderStyle CssClass=""></HeaderStyle>
		                        <ItemStyle CssClass="" Height="22px"></ItemStyle>
		                        <SelectedItemStyle CssClass="ROW_Evidenziate_Small" />
                                <%--ForeColor="#333333" 
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <AlternatingItemStyle BackColor="White" />
                                <ItemStyle BackColor="#EFF3FB" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />--%>
                                <HeaderTemplate>
                                    <table class="table fullwidth wikitable light">
                                        <tr class="tablerow tableheader">
                                            <th width="28px">
                                            </th>
                                            <th width="250px">
                                                <asp:Label ID="LBL_intVoce" runat="server" Text="Voce"></asp:Label>
                                            </th>
                                            <th width="180px">
                                                <asp:Label ID="LBL_intSezione" runat="server" Text="Sezione"></asp:Label>
                                            </th>
                                            <th>
                                                <asp:Label ID="LBL_intComunita" runat="server" Text="Comunità"></asp:Label>
                                            </th>
                                        </tr>
                                    
                                </HeaderTemplate>
                                <ItemTemplate>
                                    
                                        <tr class="tablerow">
                                            <td width="28px">
                                                <a href='<%=Me.BaseUrl %>wiki/Wiki_Comunita.aspx?id=<%#DataBinder.Eval(Container.DataItem, "ID")%>'>
                                                <img alt='Visualizza voce:<%#DataBinder.Eval(Container.DataItem, "Nome")%>' 
                                                    src="../images/search.gif" /> </a>
                                            </td>
                                            <td width="250px">
                                                <span class="Titolo_Campo" style="text-align: left;" style="left">
                                                <a href='<%=Me.BaseUrl %>wiki/Wiki_Comunita.aspx?id=<%#DataBinder.Eval(Container.DataItem, "ID")%>'>
                                                <%#DataBinder.Eval(Container.DataItem, "Nome")%></a> </span>
                                                </td>
                                                    <td width="180px">
                                                        <span class="Testo_Campo" style="text-align:left;" width="right">
                                                        <a name='<%#DataBinder.Eval(Container.DataItem, "ID")%>'>
                                                        <%#DataBinder.Eval(Container.DataItem, "Sezione.NomeSezione")%></a> </span>
                                                    </td>
                                                    <td>
                                                        <span class="Testo_Campo" style="text-align:left;" width="right">
                                                        <a name='<%#DataBinder.Eval(Container.DataItem, "ID")%>'>
                                                        <%#DataBinder.Eval(Container.DataItem, "Comunita")%></a> </span>
                                                    </td>
                                                </td>
                                            </tr>
                                        
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:DataList>
                            </div>
			            </asp:View>
                        <asp:View ID="V_TopicsImport" runat="server">
                            
                        </asp:View>
                    </asp:MultiView>
		        </div>
	        </div>
        </asp:Panel>
    
    </div>


    <asp:UpdatePanel ID="UPP_TakeSession" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="TMR_Session" EventName="Tick" />
        </Triggers>
        <ContentTemplate>
            <%--Il pannello è stato aggiornato:--%> 
            <asp:Label Visible="False" ID="LBL_Ajax" runat="server"></asp:Label>
        </ContentTemplate>
                
    </asp:UpdatePanel>
    <asp:Timer ID="TMR_Session" runat="server" Interval="900000">
                
    </asp:Timer>
</asp:Content>




