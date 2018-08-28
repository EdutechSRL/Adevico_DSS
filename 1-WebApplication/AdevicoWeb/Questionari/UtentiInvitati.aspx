<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
	CodeBehind="UtentiInvitati.aspx.vb" Inherits="Comunita_OnLine.UtentiInvitati" ValidateRequest="false" MaintainScrollPositionOnPostback="true" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register Src="~/uc/UC_SearchUserByCommunities.ascx" TagName="UC_SearchUser" TagPrefix="uc1" %>
<%@ Register Src="~/Modules/Common/UC/UC_MailEditor.ascx" TagName="CTRLmailEditor" TagPrefix="CTRL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
	<link media="screen" href="../Modules/Common/UC/UC_MailEditor.css?v=201604071200lm" type="text/css" rel="StyleSheet" />
   <link media="screen" href="stile.css?v=201604071200lm" type="text/css" rel="StyleSheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
	<script type="text/javascript">
		String.prototype.replaceAll = function (s1, s2) { return this.replace(new RegExp(s1, "g"), s2); }

		$(document).ready(function () {
			$(".view-modal").dialog({
				appendTo: "form",
				closeOnEscape: false,
				modal: true,
				width: 850,
				height: 450,
				minHeight: 400,
				minWidth: 840,
				open: function (type, data) {
					//$(this).parent().appendTo("form");
					$(this).parent().children().children('.ui-dialog-titlebar-close').hide();
				}
			});
		});

//        window.onload = LoadPreview();
//        
//        function LoadPreview() {
//            if (getTextArea()){
//                alert("text");
//                extractInfo();
//                }
//            if (CTRLmailEditor.EditorHTMLClientId){
//                alert("html");
//                extractInfoTelerik();
//            }
//        }

		function TelerikEvents(editor, args) {
			editor.AttachEventHandler("onkeyup", function (e) {
				extractInfoTelerik();
			});
		}

		function getTextArea() {
			obj = document.getElementById('<%=me.CTRLmailEditor.EditorStandardClientId  %>');
			return obj;
		}

		function getPreviewTelerik() {
			obj = $(".preview");
			return obj;
		}

		function extractInfoTelerik() {
			var editor = <%=  CTRLmailEditor.EditorHTMLClientId %>; //$find("<%=  CTRLmailEditor.EditorHTMLClientId %>"); // 
			getPreviewTelerik().html(editor.GetHtml());
			vars = arrayvars.split("|");
			for (i = 0; i < vars.length; i++) {
				values = vars[i].split("=");
				getPreviewTelerik().html(getPreviewTelerik().html().replaceAll(values[0], values[1]));
			}
			return false;
		}

		function extractInfo() {
			getPreviewTelerik().html(getTextArea().value.replaceAll("\n","</br>"));
			vars = arrayvars.split("|");
			for (i = 0; i < vars.length; i++) {
				values = vars[i].split("=");
				getPreviewTelerik().html(getPreviewTelerik().html().replaceAll(values[0], values[1]));
			}
			return false;
		}

		function ChangeCheckBoxState(id, checkState) {
			var cb = document.getElementById(id);
			if (cb != null)
				cb.checked = checkState;
		}

		function ChangeAllCheckBoxStates(checkState) {
			// Toggles through all of the checkboxes defined in the CheckBoxIDs array
			// and updates their value to the checkState input parameter
			if (CheckBoxIDs != null) {
				for (var i = 0; i < CheckBoxIDs.length; i++)
					ChangeCheckBoxState(CheckBoxIDs[i], checkState);
			}
		}

		function ChangeHeaderAsNeeded() {
			// Whenever a checkbox in the GridView is toggled, we need to
			// check the Header checkbox if ALL of the GridView checkboxes are
			// checked, and uncheck it otherwise
			if (CheckBoxIDs != null) {
				// check to see if all other checkboxes are checked
				for (var i = 1; i < CheckBoxIDs.length; i++) {
					var cb = document.getElementById(CheckBoxIDs[i]);
					if (!cb.checked) {
						// Whoops, there is an unchecked checkbox, make sure
						// that the header checkbox is unchecked
						ChangeCheckBoxState(CheckBoxIDs[0], false);
						return;
					}
				}

				// If we reach here, ALL GridView checkboxes are checked
				ChangeCheckBoxState(CheckBoxIDs[0], true);
			}
		}

//        function insertAtCursor(stringa) {
//            var campo = document.getElementById('<%=me.CTRLmailEditor.EditorClientId %>');

//            if (document.selection) {
//                campo.focus();
//                sel = document.selection.createRange();
//                sel.text = stringa;
//            }
//            else if (campo.selectionStart || campo.selectionStart == '0') {
//                var startPos = campo.selectionStart;
//                var endPos = campo.selectionEnd;
//                campo.value = campo.value.substring(0, startPos)
//                          + stringa
//                          + campo.value.substring(endPos, campo.value.length);
//            } else {
//                campo.value += stringa;
//            }

//            extractInfo();
//        }
	</script>
	<asp:Panel ID="PNLmenu" runat="server" Width="100%" HorizontalAlign="right">
		<asp:LinkButton ID="LNBGestioneMail" Visible="true" runat="server" CssClass="Link_Menu"
			CausesValidation="false"></asp:LinkButton>
		<asp:LinkButton ID="LNBQuestionarioAdmin" Visible="true" runat="server" CssClass="Link_Menu"
			CausesValidation="false"></asp:LinkButton>
		<asp:LinkButton ID="LNBNuovoUtente" Visible="true" runat="server" CssClass="Link_Menu"></asp:LinkButton>
		<asp:LinkButton ID="LNBImportaCSV" Visible="false" runat="server" CssClass="Link_Menu"
			CausesValidation="false"></asp:LinkButton>
		<asp:LinkButton ID="LNBcommunityImport" Visible="false" runat="server" CssClass="Link_Menu"
			CausesValidation="false"></asp:LinkButton>
		<asp:LinkButton ID="LNBSalva" Visible="true" runat="server" CssClass="Link_Menu"></asp:LinkButton>
		<asp:LinkButton ID="LNBGestioneRubrica" runat="server" CssClass="Link_Menu" Visible="true"
			CausesValidation="false"></asp:LinkButton>
		<asp:LinkButton ID="LNBSelezionaDestinatari" runat="server" CausesValidation="false"
			CssClass="Link_Menu" Visible="False"></asp:LinkButton>
		<%--<asp:LinkButton ID="LNKStampa" Visible="false" runat="server" CssClass="Link_Menu">
		</asp:LinkButton>&nbsp;--%>
		<asp:LinkButton ID="LNBIndietro" runat="server" CausesValidation="false" CssClass="Link_Menu"
			Visible="true"></asp:LinkButton>
	</asp:Panel>
	<br />
	<br />
	<asp:MultiView runat="server" ID="MLVquestionari" ActiveViewIndex="0">
		<asp:View runat="server" ID="VIWmessaggi">
			<br />
			<br />
			<asp:Label ID="LBerrore" runat="server" Visible="false"></asp:Label>
			<asp:Label ID="LBconferma" runat="server" Visible="false"></asp:Label>
		</asp:View>
		<asp:View ID="VIWlista" runat="server">
			<asp:Label runat="server" ID="LBlistaMessaggi" Visible="false" CssClass="TestoRosso"></asp:Label>
			<asp:Panel runat="server" ID="PNLUtentiInvitati" Width="100%">
				<h2><asp:Label runat="server" ID="LBTitolo"></asp:Label> </h2>
				<asp:GridView Width="100%" ID="GRVElenco" runat="server" DataKeyNames="ID" AutoGenerateColumns="false"
					ShowFooter="false" BackColor="transparent"
					CssClass="table light fullwidth" AllowPaging="True" PageSize="20" AllowSorting="True">
					<Columns>
						<asp:TemplateField ItemStyle-Width="20">
							<HeaderTemplate>
								<asp:CheckBox runat="server" ID="CHKInvitaHeader" />
							</HeaderTemplate>
							<ItemTemplate>
								<asp:CheckBox runat="server" ID="CHKInvitaRow" Checked='<%#Eval("isSelezionato")%>' />
							</ItemTemplate>
						</asp:TemplateField>
						<asp:BoundField DataField="cognome" />
						<asp:BoundField DataField="nome" />
						<asp:BoundField DataField="mail" />
						<asp:BoundField DataField="url" Visible="false" ItemStyle-CssClass="tdurl"/>
						<asp:BoundField DataField="descrizione" Visible="false" ItemStyle-CssClass="tddesc"/>
						<asp:TemplateField Visible="false">
							<ItemTemplate>
								<asp:ImageButton runat="server" AlternateText="" ImageUrl="img/modifica-documento.gif"
									ID="IMBGestione" CommandName="Modifica"></asp:ImageButton>
								<asp:ImageButton runat="server" AlternateText="" ImageUrl="img/elimina.gif" ID="IMBElimina"
									CommandName="Elimina"></asp:ImageButton>
							</ItemTemplate>
						</asp:TemplateField>
					</Columns>
					<EditRowStyle BackColor="#2461BF" />
					<SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
					<PagerStyle CssClass="ROW_Page_Small" HorizontalAlign="Right" Height="25px" VerticalAlign="Bottom" />
				</asp:GridView>
				<asp:Literal ID="CheckBoxIDsArray" runat="server"></asp:Literal>
				<asp:LinkButton ID="LNKConfermaUtenti" CssClass="Link_Menu" runat="server"></asp:LinkButton>
				<asp:LinkButton ID="LNKStampaUtenti" CssClass="Link_Menu" runat="server" Visible="false"
					Text="Stampa"></asp:LinkButton>
			</asp:Panel>
			<asp:CheckBox runat="server" ID="CHKisPassword" AutoPostBack="True" />
			<br />
			<br />
			<asp:Panel ID="PNLgeneraQuestionari" runat="server" BorderWidth="1" Style="padding: 3px">
				<asp:Button runat="server" ID="BTNgeneraQuestionari" Style="margin-bottom: 3px" />
				<br />
				<asp:Label runat="server" ID="LBgeneraQuestionari"></asp:Label>
			</asp:Panel>
		</asp:View>
		<asp:View ID="VIWdettagli" runat="server">
			<table border="0" width="100%" cellpadding="0" cellspacing="0">
				<tr>
					<td valign="middle" align="right"  style="display:none">
						<asp:Label runat="server" ID="LBAiutoDettagli"></asp:Label>
					</td>
					<td width="30px" align="right"  style="display:none">
						<div class="DIVHelp">
							<asp:ImageButton ID="IMBHelpDettagli" runat="server" ImageUrl="img/Help.png" />
						</div>
					</td>
				</tr>
			</table>
			<asp:FormView ID="FRVUtente" runat="server" CellPadding="4" ForeColor="#333333" Width="450">
				<ItemTemplate>
					<asp:Label ID="LBTitoloUtente" runat="server" Font-Bold="true"></asp:Label><br />
					<br />
					<asp:Label ID="LBCognome" runat="server"></asp:Label><br />
					<asp:TextBox ID="TXBCognome" runat="server" Text='<%#Eval("cognome")%>' Width="300"></asp:TextBox>
					<asp:RequiredFieldValidator runat="server" ID="RFVCognome" ControlToValidate="TXBCognome"></asp:RequiredFieldValidator>
					<br />
					<br />
					<asp:Label ID="LBNome" runat="server"></asp:Label><br />
					<asp:TextBox ID="TXBNome" runat="server" Text='<%#Eval("nome")%>' Width="300"></asp:TextBox>
					<asp:RequiredFieldValidator runat="server" ID="RFVNome" ControlToValidate="TXBNome"></asp:RequiredFieldValidator>
					<br />
					<br />
					<asp:Label ID="LBEmail" runat="server"></asp:Label><br />
					<asp:TextBox ID="TXBEmail" runat="server" Text='<%#Eval("mail")%>' Width="300"></asp:TextBox>
					<asp:RequiredFieldValidator runat="server" ID="RFVEmail" ControlToValidate="TXBEmail"></asp:RequiredFieldValidator>
					 <asp:regularexpressionvalidator id="REVmail" runat="server" CssClass="errore" ControlToValidate="TXBEmail"
				Display="dynamic" ValidationExpression="[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?">*</asp:regularexpressionvalidator>
				<%--	<asp:regularexpressionvalidator id="REVmail" runat="server" CssClass="errore" ControlToValidate="TXBEmail"
				Display="dynamic" ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$">*</asp:regularexpressionvalidator>--%>

					<br />
					<br />
					<asp:Label ID="LBDescrizione" runat="server"></asp:Label><br />
					<asp:TextBox ID="TXBDescrizione" runat="server" Width="300" Text='<%#Eval("descrizione")%>'
						Rows="4" TextMode="MultiLine"></asp:TextBox>
					<br />
					<br />
				</ItemTemplate>
				<FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
				<EditRowStyle BackColor="#2461BF" />
				<RowStyle BackColor="#EFF3FB" />
				<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
				<HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
			</asp:FormView>
			<asp:Button ID="BTNAggiungiUtente" runat="server" />
			<br />
			<br />
		</asp:View>
		<asp:View ID="VIWMail" runat="server">
			<asp:Label ID="LBErroreNoTag" runat="server" ForeColor="red" Font-Bold="true" Visible="false"></asp:Label>
			<asp:Label ID="LBErroreGenerico" runat="server" Font-Bold="true" ForeColor="red"
				Visible="false"></asp:Label>
			<table border="0" width="100%" cellpadding="0" cellspacing="0">
				<tr>
					<td valign="middle" align="right">
						<asp:Label runat="server" ID="LBAiutoMail"  style="display:none"></asp:Label>
					</td>
					<td width="30px" align="right">
						<div class="DIVHelp"  style="display:none">
							<asp:ImageButton ID="IMBHelpMail" runat="server" ImageUrl="img/Help.png" />
						</div>
					</td>
				</tr>
				<tr>
					<td>
						<asp:Label runat="server" ID="LBMsgQuestionarioBloccato" ForeColor="red"></asp:Label>
					</td>
				</tr>
			</table>
			<br />
			<asp:Panel runat="server" ID="PNLTemplate" BorderColor="black" Style="padding: 5px"
				BorderWidth="1">
				<asp:Label ID="LBTitoloTemplate" runat="server"></asp:Label><br />
				<br />
				<asp:DropDownList ID="DDLTemplate" runat="server" DataTextField="nome" DataValueField="id">
				</asp:DropDownList>
				<asp:Button ID="BTNLoadTemplate" runat="server" Text="OK" />
				<asp:Button ID="BTNElimina" runat="server" />
				<asp:Button ID="BTNNuovo" runat="server" />
			</asp:Panel>
			<br />
			<br />
			<asp:Label ID="LBDestinatario" runat="server"></asp:Label><br />
			<asp:TextBox ID="TXBDestinatario" Width="100%" TextMode="MultiLine" runat="server"
				Enabled="false" BackColor="LightGray" ForeColor="black"></asp:TextBox><br />
			<span class="selectbuttons">
				<span>
					<asp:LinkButton runat="server" ID="LKBSelezionaUtenti"></asp:LinkButton>
					 &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
				</span>
				<br />
				<span>
					<asp:LinkButton ID="LNBaddNotStartedNotCompleted" runat="server"></asp:LinkButton>
					&nbsp; &nbsp; &nbsp;&nbsp;
				</span>
				<br />
				<span>
					<asp:LinkButton ID="LNBaddNotStarted" runat="server"></asp:LinkButton>
					&nbsp; &nbsp; &nbsp;&nbsp;
				</span>
				<br />
				<span>
					<asp:LinkButton ID="LNBaddNotCompleted" runat="server"></asp:LinkButton>
					&nbsp; &nbsp; &nbsp;&nbsp;
				</span>
				<br />
				<span>
					<asp:LinkButton ID="LNBaddCompleted" runat="server"></asp:LinkButton>
					&nbsp; &nbsp; &nbsp;&nbsp;
				</span>
				<span>
					<asp:LinkButton runat="server" ID="LKBAggiungiTutti"></asp:LinkButton>&nbsp; &nbsp; &nbsp; &nbsp;
				</span>
				<span>
					<asp:LinkButton runat="server" ID="LKBAggiungiNonInvitati"></asp:LinkButton><br />
				</span>
			</span>
			<br />
			<br />
			<asp:Panel runat="server" ID="PNLMail" BorderWidth="1" Style="padding-left: 10px"
				BackColor="#EFF3FB">
				<br /><br />
				<CTRL:CTRLmailEditor ID="CTRLmailEditor" RaiseUpdateEvent="true" TelerikOnClientCommandExecuted="extractInfoTelerik();" HTMLMailOnKeyUpScript="extractInfoTelerik();"
				TelerikClientLoadScript="TelerikEvents(editor, args);"
				 ContainerLeft="" ContainerRight="" StandardMailOnKeyUpScript="extractInfo();"
				  AllowCopyToSender="False" AllowNotifyToSender="False" AllowValidation="True" runat="server" />
				<br />
				<asp:Literal ID="LTRvariables" runat="server"></asp:Literal>
				<table width="98%">
				  <%--   <tr>
						<td> StandardMailOnKeyUpScript="extractInfo();"
							<asp:Label ID="LBIntestazione" runat="server"></asp:Label>
						</td>
					</tr>--%>
					<tr>
						<td>
							<asp:Label runat="server" ID="LBAnteprima" class="Titolo_Campo RowCellLeft"></asp:Label>
							<asp:Button ID="BTNpreview" runat="server" Text="Preview" CausesValidation="false" Visible="false"/>
							<asp:Label ID="LBpreviewDisplay" runat="server" CssClass="preview"></asp:Label>

<%--               <br />
							<div class="preview" style="background=EFF3FB; width:98%"></div>                <br />
							<asp:TextBox ID="TXBTestoMessaggio" onkeyup="extractInfo();" runat="server" Rows="15"
								TextMode="MultiLine" Width="98%"></asp:TextBox><br />
														  
							<asp:TextBox ID="TXBpreview" runat="server" ReadOnly="true" Rows="15" TextMode="MultiLine"
								BackColor="#EFF3FB" CssClass="preview" Visible="false"></asp:TextBox>
								--%>
						   <%-- <br />--%>
						</td>
					</tr>
					<tr>
						<td>
							<asp:Button ID="BTNSalvaTemplate" runat="server" />
							<asp:Button ID="BTNSalvaTemplateConNome" runat="server" />
							<asp:TextBox ID="TXBNomeTemplate" runat="server" MaxLength="256" Columns="60"></asp:TextBox>
						</td>
					</tr>
					<tr>
						<td align="center">
							<br />
							<asp:CheckBox ID="CHKInoltraMittente" runat="server" />
							<br />
							<br />
							<asp:Label runat="server" ID="LBMsgInvia"></asp:Label>
							<p>
								<asp:Button runat="server" ID="BTNInviaMail" Width="120" Height="40" />
							</p>
							<asp:Label runat="server" ID="LBMsgSbloccaInvia"></asp:Label>
							<p>
								<asp:Button runat="server" ID="BTNSbloccaInvia" Width="120" Height="40" />
							</p>
						</td>
					</tr>
				</table>
			</asp:Panel>
		</asp:View>
		<asp:View ID="VIWupload" runat="server">
			<asp:Label runat="server" ID="LBDownload"></asp:Label>
			<asp:Label runat="server" ID="LBUploadFile"></asp:Label>
			<asp:FileUpload runat="server" Width="80%" ID="FLUcsv" />
			<br />
			<asp:Label runat="server" ID="LBTerminatore"></asp:Label>
			<asp:TextBox runat="server" ID="TXBTerminatore" Text=";" Width="30px"></asp:TextBox>
			<br />
			<asp:Button runat="server" ID="BTNUpload" />
			<br />
			<div>
				<asp:Button runat="server" ID="BTNimportaDaComunita" />
			</div>
		</asp:View>
		<asp:View ID="VIWStampa" runat="server">
			<asp:Panel ID="PNLStampaTutti" runat="server" BorderWidth="1" Style="padding: 3px">
				<asp:LinkButton ID="LNKStampaTutti" runat="server" CssClass="Link_Menu">
				</asp:LinkButton>&nbsp;
				<br />
				<br />
				<asp:Label runat="server" ID="LBStampaTutti"></asp:Label>
			</asp:Panel>
			<br />
			<br />
			<asp:Panel ID="PNLStampaUtentiDomande" runat="server" BorderWidth="1" Style="padding: 3px">
				<asp:LinkButton ID="LNKStampaUtentiDomande" runat="server" CssClass="Link_Menu">
				</asp:LinkButton><br />
				<br />
				<asp:Label runat="server" ID="LBStampaUtentiDomande"></asp:Label>
			</asp:Panel>
			<br />
			<br />
			<asp:Panel ID="PNLStampaSelezionati" runat="server" BorderWidth="1" Style="padding: 3px">
				<asp:LinkButton ID="LNKStampaSelezionati" runat="server" CssClass="Link_Menu" Text="">
				</asp:LinkButton><br />
				<br />
				<asp:Label runat="server" ID="LBStampaSelezionati"></asp:Label>
			</asp:Panel>
		</asp:View>
		<asp:View ID="VIWimportaDaComunita" runat="server">
			<asp:Panel ID="PNLGestioneListe" runat="server">
				<uc1:UC_SearchUser ID="UCsearchUser" runat="server" />
				<br />
				<asp:Button ID="BTNcancel" runat="server" Text="Cancel" CssClass="PulsanteFiltro">
				</asp:Button>
				<br />
				<asp:Button runat="server" ID="BTNconfirm" Text="Conferma selezione" CssClass="PulsanteFiltro" />
			</asp:Panel>
		</asp:View>
		<asp:View ID="VIWselezionaComunita" runat="server">
		</asp:View>
		<asp:View ID="VIWpreview" runat="server">
			<div class="view-modal" style="display:none;">  
				<br />
				<asp:Label ID="LBpreview" runat="server"></asp:Label>
				<br /><br />
				<asp:Button ID="BTNclosePreview" runat="server" />
			</div>
		</asp:View>
	</asp:MultiView>
</asp:Content>
