Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.CL_permessi
Imports COL_BusinessLogic_v2.UCServices
Imports Telerik.WebControls



Public Class RicercaComunitaAlbero
    Inherits System.Web.UI.Page
    Private oResource As ResourceManager
    Private _PageUtility As OLDpageUtility

    Private ReadOnly Property PageUtility() As OLDpageUtility
        Get
            If IsNothing(_PageUtility) Then
                _PageUtility = New OLDpageUtility(Me.Context)
            End If
            Return _PageUtility
        End Get
    End Property

    Protected Enum AzioneTree
        Aggiorna = 1
        Dettagli = 2
        Entra = 3
        Iscrivi = 4
        Novità = 5
    End Enum
    Private Enum StringaVisualizza
        nascondi = 0
        mostra = 1
    End Enum
    Private Enum stringaNodoBase
        iscritte = 1
        nonIscritte = 0
        tutte = -1
    End Enum
    Private Enum Iscrizioni_code
        IscrizioniAperteIl = 0
        IscrizioniChiuse = 1
        IscrizioniComplete = 2
        IscrizioniEntro = 3
    End Enum
    Private Enum StringaAbilitato
        abilitato = 1
        bloccato = 0
        inAttesa = -1
        NoComunità = 2
    End Enum
    Private Enum stringaRegistrazione
        errore = 0
        inAttesa = 1
        limiteIscrizione = 2
        iscritto = 3
    End Enum
    Private Enum stringaTitolo
        forSubscribe = 0
        Subscribed = 1
        standard = 2
    End Enum
#Region "FORM PERMESSI"
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
#End Region

    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel
    'Protected WithEvents LBtitolo As System.Web.UI.WebControls.Label

    Protected WithEvents PNLmenu As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBlista As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBalberoGerarchico As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBalbero As System.Web.UI.WebControls.LinkButton




    Protected WithEvents PNLmenuDettagli As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBannullaDettagli As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBentra As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBiscrivi As System.Web.UI.WebControls.LinkButton

    Protected WithEvents PNLmenuMessaggio As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBannullaMessaggio As System.Web.UI.WebControls.LinkButton

    Protected WithEvents PNLmenuConferma As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBannullaConferma As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBiscriviConferma As System.Web.UI.WebControls.LinkButton


#Region "Filtro"
    Protected WithEvents CBXautoUpdate As System.Web.UI.WebControls.CheckBox
    Protected WithEvents TBRchiudiFiltro As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRfiltri As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRfiltro As System.Web.UI.WebControls.TableRow
    Protected WithEvents LNBchiudiFiltro As System.Web.UI.WebControls.LinkButton
    Protected WithEvents TBRapriFiltro As System.Web.UI.WebControls.TableRow
    Protected WithEvents LNBapriFiltro As System.Web.UI.WebControls.LinkButton


    Protected WithEvents TBLfiltroNew As System.Web.UI.WebControls.Table
    Protected WithEvents LBnoCorsi As System.Web.UI.WebControls.Label
    Protected WithEvents TBRsimple As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRorgnCorsi As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBLcorsi As System.Web.UI.WebControls.Table
    Protected WithEvents TBLtipoCorsoDiStudi As System.Web.UI.WebControls.Table

    Protected WithEvents DDLorganizzazione As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLannoAccademico As System.Web.UI.WebControls.DropDownList

    Protected WithEvents LBtipoComunita_c As System.Web.UI.WebControls.Label
    Protected WithEvents TBCtipoRicerca_c As System.Web.UI.WebControls.TableCell
    Protected WithEvents LBtipoRicerca_c As System.Web.UI.WebControls.Label

    Protected WithEvents LBvalore_c As System.Web.UI.WebControls.Label
    Protected WithEvents TBCvalore_c As System.Web.UI.WebControls.TableCell
    Protected WithEvents LBricercaByIscrizione_c As System.Web.UI.WebControls.Label

    Protected WithEvents RBLricercaByIscrizione As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents DDLTipo As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLTipoRicerca As System.Web.UI.WebControls.DropDownList

    Protected WithEvents DDLresponsabile As System.Web.UI.WebControls.DropDownList
    Protected WithEvents TXBValore As System.Web.UI.WebControls.TextBox
    Protected WithEvents BTNCerca As System.Web.UI.WebControls.Button

    Protected WithEvents LBorganizzazione_c As System.Web.UI.WebControls.Label
    Protected WithEvents LBannoAccademico_c As System.Web.UI.WebControls.Label

    Protected WithEvents DDLperiodo As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LBperiodo_c As System.Web.UI.WebControls.Label

    Protected WithEvents LBcorsoDiStudi_t As System.Web.UI.WebControls.Label
    Protected WithEvents DDLtipoCorsoDiStudi As System.Web.UI.WebControls.DropDownList

    Protected WithEvents LBstatoComunita_t As System.Web.UI.WebControls.Label
    Protected WithEvents RBLstatoComunita As System.Web.UI.WebControls.RadioButtonList

#Region "Filtri automatici"
    Protected WithEvents HDNselezionato As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroFacolta As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroTipoRicerca As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroValore As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroResponsabileID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroLaureaID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroTipoCdl As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroAnno As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroPeriodo As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroTipoComunitaID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroRicercaByIscrizione As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroStatus As System.Web.UI.HtmlControls.HtmlInputHidden
#End Region

#End Region

#Region "FORM TreeView"
    Protected WithEvents PNLtreeView As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBespandi As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBcomprimi As System.Web.UI.WebControls.LinkButton
    ' Protected WithEvents LNBelenco As System.Web.UI.WebControls.LinkButton

    Protected WithEvents RDTcomunita As Telerik.WebControls.RadTreeView
    Protected WithEvents HDN_Path As System.Web.UI.HtmlControls.HtmlInputHidden
    '  Protected WithEvents HDNisChiusa As System.Web.UI.HtmlControls.HtmlInputHidden
#End Region

#Region "Form Dettagli"
    Protected WithEvents PNLdettagli As System.Web.UI.WebControls.Panel
    Protected WithEvents TBLdettagli As System.Web.UI.WebControls.Table
    Protected WithEvents CTRLDettagli As Comunita_OnLine.UC_DettagliComunita

    Protected WithEvents LBlegenda As System.Web.UI.WebControls.Label
    Protected WithEvents HDNcmnt_ID As System.Web.UI.HtmlControls.HtmlInputHidden
#End Region

#Region "Form messaggio"
    Protected WithEvents PNLmessaggio As System.Web.UI.WebControls.Panel
    Protected WithEvents LBmessaggio As System.Web.UI.WebControls.Label
    Protected WithEvents LBtreeView As System.Web.UI.WebControls.Label
#End Region

#Region "conferma"
    Protected WithEvents PNLconferma As System.Web.UI.WebControls.Panel
    Protected WithEvents LBconferma As System.Web.UI.WebControls.Label
    Protected WithEvents BTNannullaConferma As System.Web.UI.WebControls.Button
    Protected WithEvents BTNiscriviConferma As System.Web.UI.WebControls.Button

    Protected WithEvents HDisChiusa As System.Web.UI.HtmlControls.HtmlInputHidden
#End Region

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If

        If Me.SessioneScaduta() Then
            Exit Sub
        End If

        Try
            Dim iCMNT_ID As Integer
            Dim iCMNT_PAth, elenco() As String

            If Page.IsPostBack = False Then
                Dim HasPermessi As Boolean = True
                Dim HasNoPermessi As Boolean = False
                Dim ForIscrizione As Boolean = True
                Dim oServizioElencaComunita As New Services_ElencaComunita
                Dim oServizioIscrizione As New Services_IscrizioneComunita

                Me.SetupInternazionalizzazione()
                Session("Azione") = "load"
                Session("AdminForChange") = False
                Session("idComunita_forAdmin") = ""
                Session("CMNT_path_forAdmin") = ""
                Session("CMNT_path_forNews") = ""
                Session("CMNT_ID_forNews") = ""

                oServizioIscrizione = Me.ImpostaPermessiIscrizione
                oServizioElencaComunita = Me.ImpostaPermessiElenco

                Try
                    If IsNumeric(Me.Request.Cookies("RicercaComunitaUtente")("RBLricercaByIscrizione")) Then
                        Try
                            Me.RBLricercaByIscrizione.SelectedValue = Me.Request.Cookies("RicercaComunitaUtente")("RBLricercaByIscrizione")
                        Catch ex As Exception

                        End Try
                    End If
                Catch ex As Exception

                End Try
                If Me.RBLricercaByIscrizione.SelectedValue <> 0 Then
                    ForIscrizione = False
                End If
                If ForIscrizione Then
                    HasPermessi = (oServizioIscrizione.Admin Or oServizioIscrizione.List)
                Else
                    HasPermessi = (oServizioElencaComunita.Admin Or oServizioElencaComunita.List)
                End If
                HasNoPermessi = Not (oServizioElencaComunita.Admin Or oServizioElencaComunita.List Or oServizioIscrizione.Admin Or oServizioIscrizione.List)

                If HasPermessi Then
                    Me.LNBalbero.Visible = False
                    Me.LNBalberoGerarchico.Visible = False

                    If Request.QueryString("from") = "ricerca" Then
                        If Request.QueryString("show") = "1" Then
                            Me.LNBalberoGerarchico.Visible = True
                        Else
                            Me.LNBalbero.Visible = True
                        End If
                    ElseIf Request.QueryString("re_set") <> "true" Then
                        If Request.QueryString("show") = "1" Then
                            Me.LNBalberoGerarchico.Visible = True
                        Else
                            Me.LNBalbero.Visible = True
                        End If
                    End If
                    Me.SetupFiltri()
                    If Me.LNBalbero.Visible Or Me.LNBalberoGerarchico.Visible Then
                        Me.Bind_TreeView(True)
                    Else
                        Me.PNLmenuDettagli.Visible = False
                        Me.PNLtreeView.Visible = False
                    End If
                Else
                    Reset_NoPermessi(HasNoPermessi)
                End If
				If ForIscrizione Then
					Me.PageUtility.AddActionToModule(Me.PageUtility.GetModule(Services_IscrizioneComunita.Codex).ID, IIf(HasPermessi, Services_IscrizioneComunita.ActionType.CommunityList, Services_IscrizioneComunita.ActionType.NoPermission))
				Else
					Me.PageUtility.AddActionToModule(Me.PageUtility.GetModule(Services_ElencaComunita.Codex).ID, IIf(HasPermessi, Services_ElencaComunita.ActionType.List, Services_ElencaComunita.ActionType.NoPermission))
				End If
			End If
		Catch ex As Exception

        End Try

        Me.Page.Form.DefaultButton = Me.BTNCerca.UniqueID
        Me.Page.Form.DefaultFocus = Me.TXBValore.UniqueID
        Me.Master.Page.Form.DefaultButton = Me.BTNCerca.UniqueID
        Me.Master.Page.Form.DefaultFocus = Me.TXBValore.UniqueID

	End Sub

	Private Function ImpostaPermessiIscrizione() As Services_IscrizioneComunita
		Dim ComunitaID As Integer = 0
		Dim iResponse As String = "00000000000000000000000000000000"
		Dim oServizioIscrizione As New Services_IscrizioneComunita
		Dim oPersona As COL_Persona

		Try
			oPersona = Session("objPersona")
			ComunitaID = Session("idComunita")
		Catch ex As Exception
			ComunitaID = 0
		End Try
		Try
			If ComunitaID = 0 Then
				Session("Limbo") = True
				If oPersona.TipoPersona.ID = Main.TipoPersonaStandard.Guest Then
					iResponse = "00000000000000000000000000000000"
				Else
					oServizioIscrizione.Admin = False
					oServizioIscrizione.List = True
					Return oServizioIscrizione
				End If
			Else
				iResponse = Permessi(oServizioIscrizione.Codex, Me.Page)
			End If
			If (iResponse = "") Then
				iResponse = "00000000000000000000000000000000"
			End If
		Catch ex As Exception
			iResponse = "00000000000000000000000000000000"
		End Try
		oServizioIscrizione.PermessiAssociati = iResponse
		Return oServizioIscrizione
	End Function
	Private Function ImpostaPermessiElenco() As Services_ElencaComunita
		Dim ComunitaID As Integer = 0
		Dim ForAdmin As Boolean = False
		Dim iResponse As String = "00000000000000000000000000000000"
		Dim oServizioElencaComunita As New Services_ElencaComunita
		Dim oPersona As COL_Persona

		Try
			oPersona = Session("objPersona")
			ComunitaID = Session("idComunita")
		Catch ex As Exception
			ComunitaID = 0
		End Try
		Try
			If ComunitaID = 0 Then
				Session("Limbo") = True
				If oPersona.TipoPersona.ID = Main.TipoPersonaStandard.Guest Then
					iResponse = "00000000000000000000000000000000"
				Else
					oServizioElencaComunita.Admin = False
					oServizioElencaComunita.List = True
					Return oServizioElencaComunita
				End If
			Else
				iResponse = Permessi(oServizioElencaComunita.Codex, Me.Page)
			End If

			If (iResponse = "") Then
				iResponse = "00000000000000000000000000000000"
			End If
		Catch ex As Exception
			iResponse = "00000000000000000000000000000000"
		End Try
		oServizioElencaComunita.PermessiAssociati = iResponse
		Return oServizioElencaComunita
	End Function
	Private Function SessioneScaduta() As Boolean
		Dim oPersona As COL_Persona
		Dim isScaduta As Boolean = True
		Try
			oPersona = Session("objPersona")
			If oPersona.ID > 0 Then
				isScaduta = False
				Return False
			End If
		Catch ex As Exception

		End Try
		If isScaduta Then
			Dim alertMSG As String
			alertMSG = oResource.getValue("LogoutMessage")
			If alertMSG <> "" Then
				alertMSG = alertMSG.Replace("'", "\'")
			Else
				alertMSG = "Session timeout"
            End If
            Dim UrlRedirect As String = PageUtility.GetDefaultLogoutPage ' Me.DefaultUrl
            Response.Write("<script language='javascript'>function AlertLogout(Messaggio,pagina){" & vbCrLf & "alert(Messaggio);" & vbCrLf & "document.location.replace(pagina);" & vbCrLf & "} " & vbCrLf & "AlertLogout('" & alertMSG & "','" & UrlRedirect & "');</script>")
			Return True
		End If
	End Function

#Region "Bind_Filtri"
	Private Sub Reset_NoPermessi(ByVal NessunPermesso As Boolean)

		If Not NessunPermesso Then
			Me.PNLcontenuto.Visible = True
			Me.RDTcomunita.Visible = False
			Me.BTNCerca.Enabled = False
			Me.CBXautoUpdate.Checked = True
			Me.CBXautoUpdate.Enabled = False
			Me.RBLstatoComunita.Enabled = False
			Me.DDLannoAccademico.Enabled = False
			Me.DDLorganizzazione.Enabled = False
			Me.DDLperiodo.Enabled = False
			Me.DDLresponsabile.Enabled = False
			Me.DDLTipo.Enabled = False
			Me.DDLtipoCorsoDiStudi.Enabled = False
			Me.DDLTipoRicerca.Enabled = False
			Me.TBRsimple.Visible = False

			Me.PNLmenu.Visible = False
			Me.PNLmenuDettagli.Visible = False
			Me.PNLmenuMessaggio.Visible = False
			Me.PNLmenuConferma.Visible = False
		End If
		Me.PNLpermessi.Visible = True
	End Sub
	Private Sub SetupFiltri()
		Dim oImpostazioni As New COL_ImpostazioniUtente
		Me.Bind_TipiComunita()
		Me.Bind_Organizzazioni()
		Me.Bind_StatusComunità()

		If Request.QueryString("from") = "ricerca" Then
			' Setto l'anno accademico
			Try
				If IsNumeric(Me.Request.Cookies("RicercaComunitaUtente")("DDLorganizzazione")) Then
					Try
						Me.DDLorganizzazione.SelectedValue = Me.Request.Cookies("RicercaComunitaUtente")("DDLorganizzazione")
					Catch ex As Exception

					End Try
				End If
			Catch ex As Exception

			End Try
	
			Me.SetupSearchParameters()

		ElseIf Not IsNothing(Session("oImpostazioni")) Then
			Try
				oImpostazioni = Session("oImpostazioni")
				Me.Bind_Organizzazioni()
				With oImpostazioni
					Try
						If Me.RBLricercaByIscrizione.SelectedIndex = 0 Then
							' a  cui iscriversi
							Me.DDLorganizzazione.SelectedValue = .Organizzazione_Ricerca
						Else
							Me.DDLorganizzazione.SelectedValue = .Organizzazione_Iscritto
						End If
					Catch ex As Exception

					End Try

					Try
						If Me.RBLricercaByIscrizione.SelectedIndex = 0 Then
							' a  cui iscriversi
							Me.DDLTipo.SelectedValue = .TipoComunita_Ricerca
						Else
							Me.DDLTipo.SelectedValue = .TipoComunita_Iscritto
						End If
					Catch ex As Exception

					End Try

				

					Me.TBLcorsi.Visible = False
					Me.TBLtipoCorsoDiStudi.Visible = False
                End With
			Catch ex As Exception

			End Try
			Me.SetupSearchParameters()
        End If

		If Me.Request.QueryString("re_set") <> "true" Then
			If Session("limbo") = True Then
				Me.RBLstatoComunita.SelectedIndex = 0
			Else
				Dim oComunita As New COL_Comunita
				oComunita.Id = Session("idComunita")
				If oComunita.isBloccata() Then
					Try
						Me.RBLstatoComunita.SelectedValue = 2
					Catch ex As Exception
						Me.RBLstatoComunita.SelectedIndex = 0
					End Try

				ElseIf oComunita.isArchiviata() Then
					Try
						Me.RBLstatoComunita.SelectedValue = 1
					Catch ex As Exception
						Me.RBLstatoComunita.SelectedIndex = 0
					End Try
				Else
					Me.RBLstatoComunita.SelectedIndex = 0
				End If
			End If
			Me.Bind_Responsabili(, Me.RBLricercaByIscrizione.SelectedValue)
		End If

        Me.TBLcorsi.Visible = False
        Me.TBLtipoCorsoDiStudi.Visible = False
        Me.LBnoCorsi.Visible = True

        Me.DDLresponsabile.AutoPostBack = Me.CBXautoUpdate.Checked
		Me.RBLstatoComunita.AutoPostBack = Me.CBXautoUpdate.Checked
		Try
			Me.HDN_filtroAnno.Value = Me.DDLannoAccademico.SelectedValue
			Me.HDN_filtroFacolta.Value = Me.DDLorganizzazione.SelectedValue
			Me.HDN_filtroPeriodo.Value = Me.DDLperiodo.SelectedValue
			Me.HDN_filtroResponsabileID.Value = Me.DDLresponsabile.SelectedValue
			Me.HDN_filtroRicercaByIscrizione.Value = Me.RBLricercaByIscrizione.SelectedValue
			Me.HDN_filtroTipoCdl.Value = Me.DDLtipoCorsoDiStudi.SelectedValue
			Me.HDN_filtroTipoComunitaID.Value = Me.DDLTipo.SelectedValue
			Me.HDN_filtroTipoRicerca.Value = Me.DDLTipoRicerca.SelectedValue
			Me.HDN_filtroValore.Value = Me.TXBValore.Text
			Me.HDNselezionato.Value = Me.HDN_filtroTipoRicerca.Value
			Me.HDN_filtroStatus.Value = Me.RBLstatoComunita.SelectedValue
		Catch ex As Exception

		End Try
	End Sub
	Private Sub Bind_StatusComunità()
		Dim oPersona As New COL_Persona
		Dim totale, TotaleArchiviate, totaleBloccate As Integer
		Try
			Dim oListItem_Archiviate, oListItem_Bloccate As ListItem
			oPersona = Session("objPersona")

			If Me.RBLricercaByIscrizione.SelectedValue = 0 Then
                oPersona.StatusComunitaNonIscritto(oPersona.ID, totale, TotaleArchiviate, totaleBloccate)
				Me.RBLstatoComunita.SelectedIndex = 0
				Me.RBLstatoComunita.Enabled = False
			Else
				oPersona.StatusComunitaIscritto(oPersona.ID, totale, TotaleArchiviate, totaleBloccate)
				Me.RBLstatoComunita.Enabled = True
			End If


			oListItem_Archiviate = Me.RBLstatoComunita.Items.FindByValue(1)
			oListItem_Bloccate = Me.RBLstatoComunita.Items.FindByValue(2)
			If totaleBloccate = 0 Then
				If Not IsNothing(oListItem_Bloccate) Then
					If Me.RBLstatoComunita.SelectedValue = 2 Then
						Me.RBLstatoComunita.SelectedIndex = 0
					End If
					Me.RBLstatoComunita.Items.Remove(oListItem_Bloccate)
				End If
			Else
				If IsNothing(oListItem_Bloccate) Then
					If IsNothing(oListItem_Archiviate) Then
						Me.RBLstatoComunita.Items.Insert(1, New ListItem("Bloccate", 2))
					Else
						Me.RBLstatoComunita.Items.Insert(2, New ListItem("Bloccate", 2))
					End If
					oResource.setRadioButtonList(RBLstatoComunita, 2)
				End If
			End If

			If TotaleArchiviate = 0 Then
				If Not IsNothing(oListItem_Archiviate) Then
					If Me.RBLstatoComunita.SelectedValue = 1 Then
						Me.RBLstatoComunita.SelectedIndex = 0
					End If
					Me.RBLstatoComunita.Items.Remove(oListItem_Archiviate)
				End If
			Else
				If IsNothing(oListItem_Archiviate) Then
					Me.RBLstatoComunita.Items.Insert(1, New ListItem("Archiviate", 1))
					oResource.setRadioButtonList(Me.RBLstatoComunita, 1)
				End If
			End If
		Catch ex As Exception

		End Try
	End Sub
	Private Sub Bind_TipiComunita()
		'...nella ddl che mi farà da filtro delle tipologie di utenti associate al tipo comunità
		Dim oDataSet As New DataSet
		Dim oTipoComunita As New COL_Tipo_Comunita


		Try
			oDataSet = oTipoComunita.ElencaForFiltri(Session("LinguaID"), True)
			If oDataSet.Tables(0).Rows.Count > 0 Then
				DDLTipo.DataSource = oDataSet
				DDLTipo.DataTextField() = "TPCM_descrizione"
				DDLTipo.DataValueField() = "TPCM_id"
				DDLTipo.DataBind()

				'aggiungo manualmente elemento che indica tutti i tipi di comunità
				DDLTipo.Items.Insert(0, New ListItem("-- Tutti --", -1))
			End If
		Catch ex As Exception
			DDLTipo.Items.Insert(0, New ListItem("-- Tutti --", -1))
		End Try
		oResource.setDropDownList(Me.DDLTipo, -1)
	End Sub
	Private Sub Bind_Organizzazioni()
		Dim oDataset As New DataSet
		Dim oPersona As New COL_Persona

		Me.DDLorganizzazione.Items.Clear()
		Try
			oPersona = Session("objPersona")
			oDataset = oPersona.GetOrganizzazioniAssociate()

			If oDataset.Tables(0).Rows.Count > 0 Then
				Dim oComunita As New COL_Comunita

				Dim ArrComunita(,) As String
				Dim FacoltaID As Integer = -1
				Dim show As Boolean = False
				Try

					If IsArray(Session("ArrComunita")) And Session("limbo") = False Then
						ArrComunita = Session("ArrComunita")
						oComunita.Id = ArrComunita(0, 0)
						oComunita.Estrai()
						FacoltaID = oComunita.Organizzazione.Id
						show = False
					ElseIf Session("limbo") = True Then
						show = True
						FacoltaID = -1
					End If
				Catch ex As Exception
					Try
						FacoltaID = Session("ORGN_id")
						show = False
					Catch exc As Exception
						FacoltaID = -1
					End Try

				End Try

				Me.DDLorganizzazione.DataValueField = "ORGN_id"
				Me.DDLorganizzazione.DataTextField = "ORGN_ragioneSociale"
				Me.DDLorganizzazione.DataSource = oDataset
				Me.DDLorganizzazione.DataBind()
				'If oDataset.Tables(0).Rows.Count > 1 Then
				'    Me.DDLorganizzazione.Items.Insert(0, New ListItem("<< tutte >>", -1))
				'End If
				If Me.DDLorganizzazione.Items.Count > 1 Then
					Me.DDLorganizzazione.Enabled = show

					If FacoltaID >= 0 Then
						Try
							Me.DDLorganizzazione.SelectedValue = FacoltaID
						Catch ex As Exception
							Me.DDLorganizzazione.Items.Clear()
							Me.DDLorganizzazione.Items.Add(New ListItem(oComunita.Nome, FacoltaID))
							Me.DDLorganizzazione.SelectedIndex = 0
						End Try
					Else
						Me.DDLorganizzazione.SelectedIndex = 0
					End If
				Else
					Me.DDLorganizzazione.Enabled = False
				End If
			Else
				Me.DDLorganizzazione.Items.Add(New ListItem("< nessuna >", 0))
				Me.DDLorganizzazione.Enabled = False
			End If
		Catch ex As Exception
			Me.DDLorganizzazione.Items.Clear()
			Me.DDLorganizzazione.Items.Add(New ListItem("< nessuna >", 0))
			Me.DDLorganizzazione.Enabled = False
		End Try
		oResource.setDropDownList(Me.DDLorganizzazione, -1)
		oResource.setDropDownList(Me.DDLorganizzazione, 0)
	End Sub

	Private Sub Bind_Responsabili(Optional ByVal DocenteID As Integer = -1, Optional ByVal FiltroIscrizione As Main.FiltroRicercaComunitaByIscrizione = Main.FiltroRicercaComunitaByIscrizione.nonIscritto)
		Dim oDataSet As New DataSet
		Dim oComunita As COL_Comunita
		Dim FacoltaID As Integer = -1
		Dim ComunitaID As Integer = -1
		Try
			Dim TipoComuniaID As Integer = -1
			Dim TipoCdlID As Integer = -1
			Dim AnnoAcc As Integer = -1
			Dim PeriodoID As Integer = -1
			Dim StatusID As Integer = 0

			Me.DDLresponsabile.Items.Clear()
			Try
				FacoltaID = Me.DDLorganizzazione.SelectedValue
			Catch ex As Exception
				FacoltaID = -1
			End Try
			Try
				If Session("IdComunita") > 0 Then
					ComunitaID = Session("IdComunita")
				End If
			Catch ex As Exception

			End Try
			Try
				TipoComuniaID = Me.DDLTipo.SelectedValue
			Catch ex As Exception

			End Try
			Try
				StatusID = Me.RBLstatoComunita.SelectedValue
			Catch ex As Exception

			End Try

	
				oDataSet = oComunita.RicercaResponsabili(FacoltaID, ComunitaID, Session("objPersona").id, TipoComuniaID, , , , , StatusID, FiltroIscrizione)


			If oDataSet.Tables(0).Rows.Count > 0 Then
				DDLresponsabile.DataSource = oDataSet
				DDLresponsabile.DataTextField() = "Anagrafica"
				DDLresponsabile.DataValueField() = "PRSN_ID"
				DDLresponsabile.DataBind()

				'aggiungo manualmente elemento che indica tutti i tipi di comunità
				DDLresponsabile.Items.Insert(0, New ListItem("-- Tutti --", -1))
			End If
		Catch ex As Exception
			Me.DDLresponsabile.Items.Insert(0, New ListItem("-- Tutti --", -1))
		End Try
		oResource.setDropDownList(Me.DDLresponsabile, -1)
		If DocenteID > 0 Then
			Try
				Me.DDLresponsabile.SelectedValue = DocenteID
			Catch ex As Exception

			End Try
		End If

		Try
			If Me.DDLTipoRicerca.SelectedValue = Main.FiltroComunita.IDresponsabile Then
				Me.DDLresponsabile.Visible = True
				Me.TXBValore.Visible = False
				Me.TXBValore.Text = ""
			Else
				Me.DDLresponsabile.Visible = False
				Me.TXBValore.Visible = True
			End If
		Catch ex As Exception

		End Try
	End Sub


	Private Sub CBXautoUpdate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBXautoUpdate.CheckedChanged
		Me.DDLannoAccademico.AutoPostBack = Me.CBXautoUpdate.Checked
		Me.DDLtipoCorsoDiStudi.AutoPostBack = Me.CBXautoUpdate.Checked
		Me.DDLperiodo.AutoPostBack = Me.CBXautoUpdate.Checked
		Me.DDLresponsabile.AutoPostBack = Me.CBXautoUpdate.Checked
		Me.RBLstatoComunita.AutoPostBack = Me.CBXautoUpdate.Checked
		Me.Bind_TreeView(True)
	End Sub

	Private Sub RBLstatoComunita_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLstatoComunita.SelectedIndexChanged
        If Me.CBXautoUpdate.Checked Then
            Me.Bind_TreeView(True)
        End If
	End Sub
	Private Sub DDLorganizzazione_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLorganizzazione.SelectedIndexChanged
        Me.Bind_Responsabili(Me.DDLresponsabile.SelectedValue, Me.RBLricercaByIscrizione.SelectedValue)

		If Me.CBXautoUpdate.Checked Then
			Me.Bind_TreeView(True)
		End If
	End Sub
	Private Sub DDLperiodo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLperiodo.SelectedIndexChanged
		If Me.CBXautoUpdate.Checked Then
			Me.Bind_TreeView(True)
		End If
	End Sub
	Private Sub DDLannoAccademico_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLannoAccademico.SelectedIndexChanged
		If Me.CBXautoUpdate.Checked Then
			Me.Bind_TreeView(True)
		End If
	End Sub
	Private Sub DDLTipo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLTipo.SelectedIndexChanged
		'Dim showFiltroCorso As Boolean = False

		'If Session("limbo") = True Then
		'    showFiltroCorso = True
		'Else
		'    Try
		'        Dim oComunita As New COL_Comunita
		'        oComunita.Id = Session("IDComunita")
		'        oComunita.Estrai()
		'        If oComunita.Livello = 0 Or oComunita.Livello = 1 Then
		'            showFiltroCorso = True
		'        End If
		'    Catch ex As Exception

		'    End Try
		'End If

		Me.TBLcorsi.Visible = False
		Me.TBLtipoCorsoDiStudi.Visible = False
		Me.LBnoCorsi.Visible = False

	
			Me.DDLtipoCorsoDiStudi.SelectedIndex = -1
			Me.DDLannoAccademico.SelectedIndex = -1
			Me.DDLperiodo.SelectedIndex = -1
			Me.DDLtipoCorsoDiStudi.SelectedIndex = -1
			Me.LBnoCorsi.Visible = True

		If Me.CBXautoUpdate.Checked Then
			Me.Bind_TreeView(True)
		End If
	End Sub

	Private Sub RBLricercaByIscrizione_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLricercaByIscrizione.SelectedIndexChanged
		Dim oServizioElencaComunita As New Services_ElencaComunita
		Dim oServizioIscrizione As New Services_IscrizioneComunita
		Dim HasPermessi As Boolean = True
		Dim HasNoPermessi As Boolean = False
		Dim ForIscrizione As Boolean = True

		oServizioIscrizione = Me.ImpostaPermessiIscrizione
		oServizioElencaComunita = Me.ImpostaPermessiElenco
		If Me.RBLricercaByIscrizione.SelectedValue = 0 Then
            'oResource.setLabel_To_Value(Me.LBtitolo, "LBtitolo." & Me.stringaTitolo.forSubscribe)
            Me.Master.ServiceTitle = oResource.getValue("LBtitolo." & Me.stringaTitolo.forSubscribe)
		ElseIf Me.RBLricercaByIscrizione.SelectedValue = 1 Then
			ForIscrizione = False
            'oResource.setLabel_To_Value(Me.LBtitolo, "LBtitolo." & Me.stringaTitolo.Subscribed)
            Me.Master.ServiceTitle = oResource.getValue("LBtitolo." & Me.stringaTitolo.Subscribed)
		Else
            'oResource.setLabel_To_Value(Me.LBtitolo, "LBtitolo." & Me.stringaTitolo.Subscribed)
            Me.Master.ServiceTitle = oResource.getValue("LBtitolo." & Me.stringaTitolo.Subscribed)
		End If

		If ForIscrizione Then
			HasPermessi = (oServizioIscrizione.Admin Or oServizioIscrizione.List)
		Else
			HasPermessi = (oServizioElencaComunita.Admin Or oServizioElencaComunita.List)
		End If
		HasNoPermessi = Not (oServizioElencaComunita.Admin Or oServizioElencaComunita.List Or oServizioIscrizione.Admin Or oServizioIscrizione.List)

		If HasPermessi Then
			Me.Bind_StatusComunità()

			Me.CBXautoUpdate.Enabled = True
			Me.DDLTipoRicerca.Enabled = True
			Me.BTNCerca.Enabled = True
			If Me.DDLannoAccademico.Items.Count > 0 Then
				Me.DDLannoAccademico.Enabled = True
			End If
			If Me.DDLorganizzazione.Items.Count > 1 Then
				Me.DDLorganizzazione.Enabled = True
			End If
			If Me.DDLperiodo.Items.Count > 1 Then
				Me.DDLperiodo.Enabled = True
			End If
			If Me.DDLresponsabile.Items.Count > 1 Then
				Me.DDLresponsabile.Enabled = True
			End If
			If Me.DDLTipo.Items.Count > 1 Then
				Me.DDLTipo.Enabled = True
			End If
			If Me.DDLtipoCorsoDiStudi.Items.Count > 1 Then
				Me.DDLtipoCorsoDiStudi.Enabled = True
			End If
			Me.DDLTipoRicerca.Enabled = True
			Me.TBRsimple.Visible = True
			Me.PNLmenu.Visible = True
			Try
				If Me.DDLresponsabile.SelectedValue > 0 Then
					Me.Bind_Responsabili(Me.DDLresponsabile.SelectedValue, Me.RBLricercaByIscrizione.SelectedValue)
				Else
					Me.Bind_Responsabili(, Me.RBLricercaByIscrizione.SelectedValue)
				End If

			Catch ex As Exception
				Me.Bind_Responsabili(, Me.RBLricercaByIscrizione.SelectedValue)
			End Try

			Me.RDTcomunita.Visible = True
			If Me.CBXautoUpdate.Checked Then
				Me.Bind_TreeView(True)
			End If
		Else
			Reset_NoPermessi(HasNoPermessi)
		End If

	End Sub

	Private Sub DDLtipoCorsoDiStudi_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLtipoCorsoDiStudi.SelectedIndexChanged
		If Me.CBXautoUpdate.Checked Then
			Me.Bind_TreeView(True)
		End If
	End Sub

	Private Sub DDLresponsabile_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLresponsabile.SelectedIndexChanged
		If Me.CBXautoUpdate.Checked Then
			Me.Bind_TreeView(True)
		End If
	End Sub

	Private Sub BTNCerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNCerca.Click
		Me.Bind_TreeView(True)
	End Sub
	Private Sub LNBalberoGerarchico_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBalberoGerarchico.Click
		Me.SaveSearchParameters(2)
		Me.TBRfiltro.Visible = True
		Me.LNBalberoGerarchico.Visible = False
		Me.LNBalbero.Visible = True
		Me.Bind_TreeView(True)
	End Sub
	Private Sub LNBalbero_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBalbero.Click
		Me.SaveSearchParameters(1)
		Me.TBRfiltro.Visible = False
		Me.LNBalberoGerarchico.Visible = True
		Me.LNBalbero.Visible = False
		Me.Bind_TreeView(True)
	End Sub
	Private Sub LNBlista_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBlista.Click
		Me.SaveSearchParameters(3)
		Me.PageUtility.RedirectToUrl("comunita/FindCommunity.aspx?re_set=true")
	End Sub
#End Region

#Region "Setup Parametri Ricerca"
	Private Sub SaveSearchParameters(ByVal visualizza As Integer)
		Try
			Me.Response.Cookies("RicercaComunitaUtente")("DDLannoAccademico") = Me.DDLannoAccademico.SelectedValue
			Me.Response.Cookies("RicercaComunitaUtente")("DDLtipoCorsoDiStudi") = Me.DDLtipoCorsoDiStudi.SelectedValue
			Me.Response.Cookies("RicercaComunitaUtente")("DDLorganizzazione") = Me.DDLorganizzazione.SelectedValue
			Me.Response.Cookies("RicercaComunitaUtente")("DDLperiodo") = Me.DDLperiodo.SelectedValue
			Me.Response.Cookies("RicercaComunitaUtente")("DDLTipo") = Me.DDLTipo.SelectedValue
			Me.Response.Cookies("RicercaComunitaUtente")("DDLTipoRicerca") = Me.DDLTipoRicerca.SelectedValue
			Me.Response.Cookies("RicercaComunitaUtente")("TXBValore") = Me.TXBValore.Text
			Me.Response.Cookies("RicercaComunitaUtente")("RBLvisualizza") = visualizza
			Me.Response.Cookies("RicercaComunitaUtente")("RBLricercaByIscrizione") = Me.RBLricercaByIscrizione.SelectedValue

			Me.Response.Cookies("RicercaComunitaUtente")("TBRapriFiltro") = Me.TBRapriFiltro.Visible
			Me.Response.Cookies("RicercaComunitaUtente")("RBLstatoComunita") = Me.RBLstatoComunita.SelectedValue
			Me.Response.Cookies("RicercaComunitaUtente")("CBXautoUpdate") = Me.CBXautoUpdate.Checked
			Me.Response.Cookies("RicercaComunitaUtente")("DDLresponsabile") = Me.DDLresponsabile.SelectedValue
		Catch ex As Exception

		End Try
	End Sub
	Private Sub SetupSearchParameters()
		Try
			Try
				Me.TXBValore.Text = Me.Request.Cookies("RicercaComunitaUtente")("TXBValore")
			Catch ex As Exception
				Me.TXBValore.Text = ""
			End Try

			' Setto l'anno accademico
			Try
				If IsNumeric(Me.Request.Cookies("RicercaComunitaUtente")("DDLannoAccademico")) Then
					Try
						Me.DDLannoAccademico.SelectedValue = Me.Request.Cookies("RicercaComunitaUtente")("DDLannoAccademico")
					Catch ex As Exception

					End Try
				End If
			Catch ex As Exception

			End Try

			Try
				Me.TBRapriFiltro.Visible = Me.Request.Cookies("RicercaComunitaUtente")("TBRapriFiltro")
				Me.TBRfiltri.Visible = Not Me.TBRapriFiltro.Visible
				Me.TBRchiudiFiltro.Visible = Not Me.TBRapriFiltro.Visible
			Catch ex As Exception
				Me.TBRapriFiltro.Visible = False
				Me.TBRchiudiFiltro.Visible = True
				Me.TBRfiltri.Visible = True
			End Try

			Try
				Me.RBLstatoComunita.SelectedValue = Me.Request.Cookies("RicercaComunitaUtente")("RBLstatoComunita")
			Catch ex As Exception
				Me.RBLstatoComunita.SelectedIndex = 0
			End Try


			' Setto il periodo
			Try
				If IsNumeric(Me.Request.Cookies("RicercaComunitaUtente")("DDLperiodo")) Then
					Try
						Me.DDLperiodo.SelectedValue = Me.Request.Cookies("RicercaComunitaUtente")("DDLperiodo")
					Catch ex As Exception

					End Try
				End If
			Catch ex As Exception

			End Try
			Try
				If IsNumeric(Me.Request.Cookies("RicercaComunitaUtente")("RBLricercaByIscrizione")) Then
					Try
						Me.RBLricercaByIscrizione.SelectedValue = Me.Request.Cookies("RicercaComunitaUtente")("RBLricercaByIscrizione")
					Catch ex As Exception

					End Try
				End If
			Catch ex As Exception

			End Try

			' Setto l'organizzazione
			Try
				If IsNumeric(Me.Request.Cookies("RicercaComunitaUtente")("DDLorganizzazione")) Then
					Try
						Me.DDLorganizzazione.SelectedValue = Me.Request.Cookies("RicercaComunitaUtente")("DDLorganizzazione")
					Catch ex As Exception

					End Try
				End If
			Catch ex As Exception

			End Try

			' Setto l'anno accademico
			Try
				If IsNumeric(Me.Request.Cookies("RicercaComunitaUtente")("DDLtipoCorsoDiStudi")) Then
					Try
						Me.DDLtipoCorsoDiStudi.SelectedValue = Me.Request.Cookies("RicercaComunitaUtente")("DDLtipoCorsoDiStudi")
					Catch ex As Exception

					End Try
				End If
			Catch ex As Exception

			End Try

			Me.TBLcorsi.Visible = False
			Me.TBLtipoCorsoDiStudi.Visible = False
			Me.LBnoCorsi.Visible = False
			Try
				' If IsNumeric(Me.Request.Cookies("ListaComunita")("tipo")) Then
				Me.DDLTipo.SelectedValue = Me.Request.Cookies("RicercaComunitaUtente")("DDLTipo")
            Catch ex As Exception

            End Try
            Me.DDLtipoCorsoDiStudi.SelectedIndex = -1
            Me.DDLannoAccademico.SelectedIndex = -1
            Me.DDLperiodo.SelectedIndex = -1
            Me.DDLtipoCorsoDiStudi.SelectedIndex = -1
            Me.LBnoCorsi.Visible = True
			' Setto il tipo di ricerca
			Try
				If IsNumeric(Me.Request.Cookies("RicercaComunitaUtente")("DDLTipoRicerca")) Then
					Me.DDLTipoRicerca.SelectedValue = Me.Request.Cookies("RicercaComunitaUtente")("DDLTipoRicerca")
				End If
			Catch ex As Exception

			End Try

			If Me.DDLTipoRicerca.SelectedValue = Main.FiltroComunita.IDresponsabile Then
				Me.DDLresponsabile.Visible = True
				Me.TXBValore.Visible = False
				Me.LBvalore_c.Visible = False
			End If
			Try
				Me.CBXautoUpdate.Checked = Me.Response.Cookies("RicercaComunitaUtente")("CBXautoUpdate")
			Catch ex As Exception
				Me.CBXautoUpdate.Checked = True
			End Try

			Try
				Me.Bind_Responsabili(Me.Request.Cookies("RicercaComunitaUtente")("DDLresponsabile"), Me.RBLricercaByIscrizione.SelectedValue)
			Catch ex As Exception
				Me.Bind_Responsabili(, Me.RBLricercaByIscrizione.SelectedValue)
			End Try
		Catch ex As Exception

		End Try
	End Sub
#End Region


#Region "Gestione Comunità"
	Private Sub VisualizzaDettagli(ByVal CMNT_Path As String, ByVal isIscritto As Boolean, ByVal isChiusa As Boolean)
		Dim ComunitaID As Integer
		Dim Elenco() As String

		Try
			Elenco = CMNT_Path.Split(".")
			ComunitaID = Elenco(UBound(Elenco) - 1)

			Dim oComunita As New COL_Comunita
			Me.Reset_Todettagli()

			Me.CTRLDettagli.SetupDettagliComunita(ComunitaID)
			Me.HDNcmnt_ID.Value = ComunitaID
			oComunita.Id = ComunitaID
			oComunita.Estrai()


			If oComunita.Errore = Errori_Db.None Then
				Dim canSubscribe As Boolean = True
				Dim canEnter As Boolean = False

				Me.HDN_Path.Value = CMNT_Path
				If isIscritto Then
					canEnter = (ComunitaID <> Session("idComunita"))
					canSubscribe = False
				Else
					canSubscribe = (oComunita.CanSubscribe)
					canEnter = False
				End If

				If oComunita.DataInizioIscrizione > Now Then
					canSubscribe = False
				Else

					If Not Equals(New Date, oComunita.DataFineIscrizione) Then
						Dim DataTemp As DateTime
						DataTemp = oComunita.DataFineIscrizione.Date
						DataTemp = DataTemp.AddHours(23)
						DataTemp = DataTemp.AddMinutes(59)
						If DataTemp < Now Then
							canSubscribe = False
						End If
					End If
				End If

				Me.LNBiscrivi.Visible = False
				Me.LNBentra.Visible = False
				If canSubscribe Then
					Me.LNBiscrivi.Visible = True
					Me.LNBiscrivi.Enabled = Not (oComunita.Bloccata Or oComunita.Archiviata)
				End If
				If canEnter Then
					Me.LNBentra.Visible = True
					Me.LNBentra.Enabled = Not (oComunita.Bloccata)
				End If

			Else
				Me.LNBentra.Visible = False
				Me.LNBiscrivi.Visible = False
			End If
			Me.HDisChiusa.Value = isChiusa
		Catch ex As Exception
			Me.ResetForm(False)
			Me.HDN_Path.Value = ""
			Me.HDisChiusa.Value = ""
		End Try
	End Sub
    Private Sub Entra_Comunita(ByVal CMNT_Path As String)
        'Dim oResourceConfig As New ResourceManager
        'oResourceConfig = GetResourceConfig(Session("LinguaCode"))
        'Dim status As lm.Comol.Core.DomainModel.SubscriptionStatus
        'Dim idPerson As Integer = PageUtility.CurrentUser.ID
        'Dim array() As String = {"."}
        'Dim idCommunity As Integer = 0
        'idCommunity = (From s In (path.Split(array, StringSplitOptions.RemoveEmptyEntries)) Select CInt(s)).ToList().FirstOrDefault()


        'Dim oComunita As New COL_Comunita With {.Id = idCommunity}
        'oComunita.Estrai()
        'oComunita.TipoComunita.Icona = "./../" & oComunita.TipoComunita.Icona



        'Dim oTreeComunita As New COL_TreeComunita
        'Try
        '    oTreeComunita.Directory = Server.MapPath(PageUtility.BaseUrl & "profili/") & idPerson & "\"
        '    oTreeComunita.Nome = idPerson & ".xml"
        'Catch ex As Exception

        'End Try

        'If oComunita.Errore = Errori_Db.None Then
        '    Dim oRuolo As New COL_RuoloPersonaComunita

        '    oRuolo.EstraiByLinguaDefault(idCommunity, idPerson)
        '    status = PageUtility.AccessToCommunity(idPerson, idCommunity, path, oResourceConfig, True)
        '    Select Case status
        '        Case lm.Comol.Core.DomainModel.SubscriptionStatus.activemember
        '            Exit Sub
        '        Case lm.Comol.Core.DomainModel.SubscriptionStatus.blocked
        '            Me.Reset_ToMessaggi()
        '            Me.HDN_Path.Value = ""
        '            oResource.setLabel_To_Value(Me.LBmessaggi, "abilitato." & Me.StringaAbilitato.bloccato)
        '            oTreeComunita.CambiaAttivazione(idCommunity, False, oResource)

        '        Case lm.Comol.Core.DomainModel.SubscriptionStatus.waiting
        '            Me.Reset_ToMessaggi()
        '            Me.HDN_Path.Value = ""
        '            oResource.setLabel_To_Value(Me.LBmessaggi, "abilitato." & Me.StringaAbilitato.inAttesa)
        '            oTreeComunita.CambiaAbilitazione(idCommunity, False)

        '        Case lm.Comol.Core.DomainModel.SubscriptionStatus.communityblocked
        '            'Spiacente, non si ha accesso alla comunità
        '            Me.Reset_ToMessaggi()
        '            Me.HDN_Path.Value = ""
        '            oResource.setLabel_To_Value(Me.LBmessaggi, "abilitato." & Me.StringaAbilitato.errore)
        '            oTreeComunita.CambiaIsBloccata(idCommunity, True)
        '        Case Else
        '            Me.Reset_ToMessaggi()
        '            Me.HDN_Path.Value = ""
        '    End Select
        'Else
        '    ' la comunità non esiste più !!
        '    oTreeComunita.Delete(idCommunity, path)
        '    Me.Reset_ToMessaggi()

        '    oResource.setLabel_To_Value(Me.LBmessaggi, "abilitato." & Me.StringaAbilitato.noCommunity)
        '    '   Me.LBmessaggio.Text = "ATTENZIONE: errore di accesso al sistema, la comunità prescelta sembra non essere presente nel sistema."
        '    Me.HDN_Path.Value = ""
        'End If

        Dim CMNT_ID, PRSN_ID As Integer
        Dim Elenco_CMNT_ID() As String
        Dim oTreeComunita As New COL_TreeComunita
        Dim oPersona As New COL_Persona
        Dim oComunita As New COL_Comunita

        Elenco_CMNT_ID = CMNT_Path.Split(".")
        CMNT_ID = Elenco_CMNT_ID(UBound(Elenco_CMNT_ID) - 1)

        Try

            oPersona = Session("objPersona")
            PRSN_ID = oPersona.ID

            oTreeComunita.Directory = Server.MapPath("./../profili/") & PRSN_ID & "\"
            oTreeComunita.Nome = PRSN_ID & ".xml"
        Catch ex As Exception

        End Try


        Try
            oComunita.Id = CMNT_ID
            oComunita.Estrai()
            oComunita.TipoComunita.Icona = "./../" & oComunita.TipoComunita.Icona
            GenericCacheManager.PurgeCacheItems(CachePolicy.PermessiServizioUtente())
            If oComunita.Errore = Errori_Db.None Then
                Dim i, j, totale, ORGN_ID, RuoloID As Integer
                Dim oRuolo As New COL_RuoloPersonaComunita
                'Dim oResponsabile As New COL_Persona
                'Dim Responsabile As String

                'oResponsabile = oComunita.GetResponsabile()
                'If oResponsabile.Nome <> "" And oResponsabile.Cognome <> "" Then
                '    Responsabile = oResponsabile.Cognome & " " & oResponsabile.Nome
                'ElseIf oResponsabile.Nome <> "" Then
                '    Responsabile = oResponsabile.Nome
                'ElseIf oResponsabile.Cognome <> "" Then
                '    Responsabile = oResponsabile.Cognome
                'Else
                '    Responsabile = "n.d."
                'End If

                oRuolo.EstraiByLinguaDefault(CMNT_ID, PRSN_ID)
                If oRuolo.Errore = Errori_Db.None Then
                    RuoloID = oRuolo.Id
                    If oRuolo.Attivato And oRuolo.Abilitato Then
                        With oComunita
                            Me.PageUtility.AddActionToModule(Me.PageUtility.GetModule(Services_ElencaComunita.Codex).ID, Services_ElencaComunita.ActionType.Access, Me.PageUtility.CreateObjectsList(Services_ElencaComunita.ObjectType.Community, CMNT_ID))

                            Session("IdComunita") = CMNT_ID
                            Session("ORGN_id") = .Organizzazione.Id
                            Session("RLPC_id") = oRuolo.Id
                            Session("IdRuolo") = oRuolo.TipoRuolo.Id

                            'carico il ruolo che la persona adempie nella comunità selezionata
                            Try

                                Dim oServizio As New COL_Servizio
                                Dim oDataSet As New DataSet
                                oDataSet = oServizio.ElencaByTipoRuoloByComunita(Session("IdRuolo"), CMNT_ID)
                                totale = oDataSet.Tables(0).Rows.Count - 1

                                Dim ArrPermessi(totale, 2) As String
                                For i = 0 To totale
                                    Dim oRow As DataRow
                                    oRow = oDataSet.Tables(0).Rows(i)
                                    ArrPermessi(i, 0) = oRow.Item("SRVZ_Codice") 'CODICE servizio
                                    ArrPermessi(i, 1) = oRow.Item("SRVZ_ID") 'id servizio
                                    ArrPermessi(i, 2) = oRow.Item("LKSC_Permessi") 'valore servizio
                                Next
                                Session("ArrPermessi") = ArrPermessi
                            Catch ex As Exception

                            End Try

                            Try
                                If Session("LogonAs") = False Then
                                    oRuolo.UpdateUltimocollegamento()
                                End If

                            Catch ex As Exception

                            End Try

                            'Aggiorno gli array relativi al menu history !!!

                            Dim ArrComunita(,) As String
                            Dim tempArray(,), Path As String

                            ' RIMOSSO PER PROBLEMA history.back()
                            j = 0
                            For i = 0 To UBound(Elenco_CMNT_ID) - 1

                                If IsNumeric(Elenco_CMNT_ID(i)) Then
                                    If Elenco_CMNT_ID(i) > 0 Then
                                        ReDim Preserve ArrComunita(3, j)
                                        ArrComunita(0, j) = Elenco_CMNT_ID(i)
                                        ArrComunita(1, j) = COL_Comunita.EstraiNomeBylingua(Elenco_CMNT_ID(i), Session("LinguaID"))

                                        If Path = "" Then
                                            Path = "." & Elenco_CMNT_ID(i) & "."
                                        Else
                                            Path = Path & Elenco_CMNT_ID(i) & "."
                                        End If
                                        ArrComunita(2, j) = Path

                                        ' Ruolo svolto..........
                                        ArrComunita(3, j) = oPersona.GetIDRuoloForComunita(Elenco_CMNT_ID(i))
                                        j = j + 1
                                    End If
                                End If
                            Next

                            Session("ArrComunita") = ArrComunita
                            Session("limbo") = False


                        End With

                        Session("RLPC_ID") = oRuolo.Id
                        Dim oUtility As New OLDpageUtility(Me.Context)

                        oComunita.RegistraAccesso(CMNT_ID, PRSN_ID, oUtility.SystemSettings.CodiceDB)
                        Me.PageUtility.SendNotificationUpdateCommunityAccess(PRSN_ID, CMNT_ID, oRuolo.UltimoCollegamento)
                        oTreeComunita.Update(oComunita, CMNT_Path, oComunita.GetNomeResponsabile_NomeCreatore, oRuolo)

                        Session("AdminForChange") = False
                        Session("CMNT_path_forAdmin") = ""
                        Session("idComunita_forAdmin") = ""
                        ' REGISTRAZIONE EVENTO
                        Session("TPCM_ID") = oComunita.TipoComunita.ID

                        Dim defaultUrl As String = PageUtility.GetCommunityDefaultPage(CMNT_ID, PRSN_ID)


                        If oComunita.ShowCover(CMNT_ID, PRSN_ID) Then
                            If oRuolo.SaltaCopertina Then
                                Me.PageUtility.RedirectToUrl(defaultUrl)
                                'Me.PageUtility.RedirectToUrl(RedirectToDefaultPage(CMNT_ID, PRSN_ID))
                            Else
                                Me.PageUtility.RedirectToUrl("Generici/Cover.aspx")
                            End If
                        Else
                            Me.PageUtility.RedirectToUrl(defaultUrl)
                            '    Me.PageUtility.RedirectToUrl(RedirectToDefaultPage(CMNT_ID, PRSN_ID)) ' se non faccio il redirect mi esegue prima il page_load dell'header e quindi vedo l'id della comunità a cui ero loggato e non quella corrente
                        End If
                        Else
                            ' non si ha accesso alla comunità
                            Me.PNLdettagli.Visible = False
                            Me.PNLmessaggio.Visible = True
                            Me.PNLtreeView.Visible = False
                            Me.HDN_Path.Value = ""
                            If Not (oRuolo.Attivato) Then
                                oResource.setLabel_To_Value(Me.LBmessaggio, "LBMessaggi." & Me.StringaAbilitato.inAttesa)
                                'Me.LBmessaggio.Text = "ATTENZIONE: non è possibile accedere alla comunità selezionata, si è in attesa di attivazione da parte del relativo amministratore/responsabile."
                            ElseIf Not oRuolo.Abilitato Then
                                oResource.setLabel_To_Value(Me.LBmessaggio, "LBMessaggi." & Me.StringaAbilitato.bloccato)
                                'Me.LBmessaggio.Text = "ATTENZIONE: non è possibile accedere alla comunità selezionata, l'accesso è stato bloccato dal relativo amministratore/responsabile."
                            Else
                                oResource.setLabel_To_Value(Me.LBmessaggio, "LBMessaggi." & Me.StringaAbilitato.bloccato)
                                'Me.LBmessaggio.Text = "ATTENZIONE: l'accesso alla comunità non è al momento consentito."
                            End If
                            oTreeComunita.Update(oComunita, CMNT_Path, oComunita.GetNomeResponsabile_NomeCreatore, oRuolo)
                        End If
                Else
                    'Spiacente, non si ha accesso alla comunità
                    Me.PNLtreeView.Visible = False
                    Me.PNLmessaggio.Visible = True
                    Me.PNLdettagli.Visible = False
                    Me.HDN_Path.Value = ""
                    '  Me.LBmessaggio.Text = "ATTENZIONE: l'accesso alla comunità non è al momento consentito."

                    '  oResource.setButtonByValue(Me.BTNMessaggi, stringaMessaggio.indietro, True)
                    oResource.setLabel_To_Value(Me.LBmessaggio, "LBMessaggi." & Me.StringaAbilitato.NoComunità)
                    oTreeComunita.Delete(CMNT_ID, CMNT_Path)
                End If
            Else
                ' la comunità non esiste più !!
                oTreeComunita.Delete(CMNT_ID, CMNT_Path)
                Me.PNLtreeView.Visible = False
                Me.PNLmessaggio.Visible = True
                oResource.setLabel_To_Value(Me.LBmessaggio, "LBMessaggi." & Me.StringaAbilitato.NoComunità)
                Me.HDN_Path.Value = ""
            End If
        Catch ex As Exception
            Me.PNLtreeView.Visible = False
            Me.PNLmessaggio.Visible = True
            Me.HDN_Path.Value = ""
            oResource.setLabel_To_Value(Me.LBmessaggio, "LBMessaggi." & Me.StringaAbilitato.NoComunità)
        End Try
    End Sub
#End Region

#Region "Gestione Treeview"
	Private Sub Bind_TreeView(Optional ByVal ApplicaFiltri As Boolean = False)
		Try
			Me.GenerateTreeView(ApplicaFiltri)
			Me.PNLtreeView.Visible = True
		Catch ex As Exception

		End Try
	End Sub

	Private Function Filtraggio_Dati(Optional ByVal ApplicaFiltri As Boolean = False) As DataSet
		Dim oDataset As New DataSet
		Dim i, totale, totaleHistory As Integer
		Dim Path As String

		Try
			Dim valore As String = ""
			Dim oFiltroTipoRicerca As Main.FiltroComunita = Main.FiltroComunita.tutti
			Dim oFiltroLettera As Main.FiltroComunita = Main.FiltroComunita.tutti
			Dim oPersona As New COL_Persona

			oPersona = Session("objPersona")
			If Me.CBXautoUpdate.Checked Or ApplicaFiltri = True Then
				If Me.TXBValore.Text <> "" Then
					Me.TXBValore.Text = Trim(Me.TXBValore.Text)
				End If
				valore = Me.TXBValore.Text
			Else
				Try
					valore = Trim(Me.HDN_filtroValore.Value)
				Catch ex As Exception

				End Try
			End If

			Dim TipoRicercaID As Integer
			If Me.CBXautoUpdate.Checked Or ApplicaFiltri = True Then
				TipoRicercaID = Me.DDLTipoRicerca.SelectedValue
			Else
				Try
					TipoRicercaID = Me.HDN_filtroTipoRicerca.Value
				Catch ex As Exception
					TipoRicercaID = -1
				End Try
			End If
			If valore <> "" Or (Me.CBXautoUpdate.Checked And Me.DDLresponsabile.Visible) Or (Not Me.CBXautoUpdate.Checked And TipoRicercaID = Main.FiltroComunita.IDresponsabile) Then
				Select Case TipoRicercaID
					Case Main.FiltroComunita.nome
						oFiltroTipoRicerca = Main.FiltroComunita.nome
					Case Main.FiltroComunita.creataDopo
						If IsDate(valore) = False Then
							valore = ""
						Else
							oFiltroTipoRicerca = Main.FiltroComunita.creataDopo
						End If
					Case Main.FiltroComunita.creataPrima
						If IsDate(valore) = False Then
							valore = ""
						Else
							oFiltroTipoRicerca = Main.FiltroComunita.creataPrima
						End If
					Case Main.FiltroComunita.dataIscrizioneDopo
						If IsDate(valore) = False Then
							valore = ""
						Else
							oFiltroTipoRicerca = Main.FiltroComunita.dataIscrizioneDopo
						End If
					Case Main.FiltroComunita.dataFineIscrizionePrima
						If IsDate(valore) = False Then
							valore = ""
						Else
							oFiltroTipoRicerca = Main.FiltroComunita.dataFineIscrizionePrima
						End If
					Case Main.FiltroComunita.contiene
						oFiltroTipoRicerca = Main.FiltroComunita.contiene
					Case Main.FiltroComunita.cognomeDocente
						oFiltroTipoRicerca = Main.FiltroComunita.cognomeDocente
					Case Main.FiltroComunita.IDresponsabile
						Try
							If Me.CBXautoUpdate.Checked Then
								valore = Me.DDLresponsabile.SelectedValue
							Else
								valore = Me.HDN_filtroResponsabileID.Value
							End If

							oFiltroTipoRicerca = Main.FiltroComunita.IDresponsabile
						Catch ex As Exception
							valore = -1
						End Try
					Case Else
						valore = ""
				End Select
			End If
			If (Me.CBXautoUpdate.Checked Or ApplicaFiltri = True) And valore = "" Then
				Me.TXBValore.Text = valore
				Me.HDN_filtroValore.Value = ""
			End If

			Dim ComunitaPadreID As Integer
			ComunitaPadreID = -1

			Dim FacoltaID, LaureaID, PeriodoID, AAid, TipocomunitaID, TipoCdlID, StatusID As Integer
			If Me.CBXautoUpdate.Checked Or ApplicaFiltri = True Then
				Me.HDN_filtroTipoRicerca.Value = Me.DDLTipoRicerca.SelectedValue
				Me.HDN_filtroValore.Value = Me.TXBValore.Text
				Me.HDN_filtroResponsabileID.Value = Me.DDLresponsabile.SelectedValue
				Me.HDN_filtroTipoCdl.Value = Me.DDLtipoCorsoDiStudi.SelectedValue
				Me.HDN_filtroAnno.Value = Me.DDLannoAccademico.SelectedValue
				Me.HDN_filtroPeriodo.Value = Me.DDLperiodo.SelectedValue
				Me.HDN_filtroTipoComunitaID.Value = Me.DDLTipo.SelectedValue
				Me.HDN_filtroFacolta.Value = Me.DDLorganizzazione.SelectedValue
				Me.HDN_filtroRicercaByIscrizione.Value = Me.RBLricercaByIscrizione.SelectedValue
				Me.HDN_filtroStatus.Value = Me.RBLstatoComunita.SelectedValue
			End If

			Try
				FacoltaID = Me.HDN_filtroFacolta.Value
			Catch ex As Exception
				FacoltaID = -1
			End Try
			Try
				LaureaID = Me.HDN_filtroLaureaID.Value
			Catch ex As Exception
				LaureaID = -1
			End Try
			Try
				AAid = Me.HDN_filtroAnno.Value
			Catch ex As Exception
				AAid = -1
			End Try
			Try
				PeriodoID = Me.HDN_filtroPeriodo.Value
			Catch ex As Exception
				PeriodoID = -1
			End Try
			Try
				TipocomunitaID = Me.HDN_filtroTipoComunitaID.Value
			Catch ex As Exception
				TipocomunitaID = -1
			End Try
			Try
				TipoCdlID = Me.HDN_filtroTipoCdl.Value
			Catch ex As Exception
				TipoCdlID = -1
			End Try


			Try
				StatusID = Me.HDN_filtroStatus.Value
			Catch ex As Exception
				StatusID = 0
			End Try
			Dim ComunitaPath As String = ""
			If IsArray(Session("ArrComunita")) And Me.LNBalberoGerarchico.Visible Then
				Try
					Dim ArrComunita(,) As String
					ArrComunita = Session("ArrComunita")
					totaleHistory = UBound(ArrComunita, 2)
					ComunitaPath = ArrComunita(2, totaleHistory)
				Catch ex As Exception

				End Try

				ComunitaPadreID = Session("ComunitaID")
			End If


			Dim oComunita As New COL_Comunita
			'If ComunitaPath <> "" Then
			'Select Case Me.HDN_filtroTipoComunitaID.Value
			'    Case Main.TipoComunitaStandard.Corso
			'        oDataset = oComunita.RicercaComunitaForManagement(Session("LinguaID"), FacoltaID, ComunitaPath, ComunitaPadreID, oPersona.Id, , oFiltroLettera, , , , , , , Me.DDLstatoComunita.SelectedValue)
			'    Case Main.TipoComunitaStandard.CorsoDiLaurea
			'        oDataset = oComunita.RicercaComunitaForManagement(Session("LinguaID"), FacoltaID, ComunitaPath, ComunitaPadreID, oPersona.Id, , oFiltroLettera, , , , , , , Me.DDLstatoComunita.SelectedValue)
			'    Case Else


			'If IsArray(Session("ArrComunita")) Then
			'    Try
			'        ComunitaPadreID = Session("ArrComunita")(2, UBound(Session("ArrComunita"), 2))
			'    Catch ex As Exception
			'    End Try
			'End If

			oDataset = oComunita.RicercaComunitaAlberoForManagement(Session("LinguaID"), FacoltaID, ComunitaPath, ComunitaPadreID, oPersona.ID, , , , , , , , , StatusID)
			' End Select
			'  End If
			oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_Esteso"))
			oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_EstesoNoSpan"))
			oDataset.Tables(0).Columns.Add(New DataColumn("Proprieta"))
			oDataset.Tables(0).Columns.Add(New DataColumn("Alternative"))
			oDataset.Tables(0).Columns.Add(New DataColumn("Iscritti"))

			If Not oDataset.Tables(0).Columns.Contains("AnnoAccademico") Then
				oDataset.Tables(0).Columns.Add(New DataColumn("AnnoAccademico"))
			End If
			If Not oDataset.Tables(0).Columns.Contains("Periodo") Then
				oDataset.Tables(0).Columns.Add(New DataColumn("Periodo"))
			End If
			If Not oDataset.Tables(0).Columns.Contains("CMNT_Iscritti") Then
				oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_Iscritti"))
			End If
			If Not oDataset.Tables(0).Columns.Contains("CMNT_Totale") Then
				oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_Totale"))
			End If
			If Not oDataset.Tables(0).Columns.Contains("AnagraficaResponsabile") Then
				oDataset.Tables(0).Columns.Add(New DataColumn("AnagraficaResponsabile"))
			End If
			If Not oDataset.Tables(0).Columns.Contains("CMNT_dataCreazioneText") Then
				oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_dataCreazioneText"))
			End If
			If Not oDataset.Tables(0).Columns.Contains("CMNT_dataCessazioneText") Then
				oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_dataCessazioneText"))
			End If
			If Not oDataset.Tables(0).Columns.Contains("HasNews") Then
				oDataset.Tables(0).Columns.Add(New DataColumn("HasNews"))
			End If
			If Not oDataset.Tables(0).Columns.Contains("NoDelete") Then
				oDataset.Tables(0).Columns.Add(New DataColumn("NoDelete"))
			End If
			Dim oDataview As New DataView
			Dim ElencoComunitaID As String = ","
			Dim Livello As Integer
			oDataview = oDataset.Tables(0).DefaultView
			oDataview.AllowDelete = True
			totale = oDataset.Tables(0).Rows.Count

			oDataview.Sort = "ALCM_Livello DESC"
			Try
				Livello = oDataview.Item(0).Item("ALCM_Livello")

				oDataview.RowFilter = "NoDelete <>1 and ALCM_Livello=" & Livello
			Catch ex As Exception
				Livello = -1
			End Try
			oDataview.Sort = "ALCM_Path"
			While Livello > 0
				While oDataview.Count > 0
					Dim ComunitaID, ComunitaTipoID As Integer
					Dim ComunitaPercorso, PercorsoPadre As String
					Dim isDelete As Boolean = False
					Dim isChiusa As Boolean = False

					ComunitaID = oDataview.Item(0).Row.Item("CMNT_id")
					ComunitaTipoID = oDataview.Item(0).Row.Item("CMNT_TPCM_id")
					ComunitaPercorso = oDataview.Item(0).Row.Item("ALCM_Path")
					PercorsoPadre = oDataview.Item(0).Row.Item("ALCM_RealPath")
					isChiusa = oDataview.Item(0).Row.Item("ALCM_isChiusaForPadre")
					If HDN_filtroRicercaByIscrizione.Value = 0 Then
						If Not IsDBNull(oDataview.Item(0).Row.Item("RLPC_TPRL_id")) Then
							If oDataview.Item(0).Row.Item("RLPC_TPRL_id") > -1 Then
								oDataview.Delete(0)
								isDelete = True
							End If
						End If
						If Not isDelete Then
							If oPersona.TipoPersona.ID = Main.TipoPersonaStandard.Copisteria Then
								If oDataview.Item(0).Row.Item("CMNT_AccessoCopisteria") = 0 Then
									oDataview.Delete(0)
									isDelete = True
								End If
							End If
						End If
						'If Not isDelete Then
						'    If oDataview.Item(0).Row.Item("CMNT_TPCM_ID") = Main.TipoComunitaStandard.Coordinamento Then
						'        oDataview.Delete(0)
						'        isDelete = True
						'    End If
						'End If
						If Not isDelete Then
							Dim Filtro As String
							Filtro = oDataview.RowFilter

							oDataview.RowFilter = "ALCM_Path='" & PercorsoPadre & "'"

							Try
								If oDataview.Count = 1 Then
									If isChiusa Then
										If IsDBNull(oDataview.Item(0).Item("RLPC_TPRL_id")) Then
											isDelete = True
										ElseIf Not (oDataview.Item(0).Item("RLPC_TPRL_id") > -1 And oDataview.Item(0).Item("RLPC_Attivato") = 1 And oDataview.Item(0).Item("RLPC_abilitato") = 1) Then
											isDelete = True
										End If
									Else
										If (oDataview.Item(0).Item("RLPC_TPRL_id") > -1 And oDataview.Item(0).Item("RLPC_Attivato") = 0 And oDataview.Item(0).Item("RLPC_abilitato") = 0) Then
											isDelete = True
										End If
									End If
								End If
							Catch ex As Exception

							End Try

							If isDelete Then
								oDataview.RowFilter = "ALCM_Path='" & ComunitaPercorso & "'"
								oDataview.Delete(0)
							End If
							oDataview.RowFilter = Filtro
						End If
					Else
						If IsDBNull(oDataview.Item(0).Row.Item("RLPC_TPRL_id")) Then
							oDataview.Delete(0)
							isDelete = True
						Else
							If oDataview.Item(0).Row.Item("RLPC_TPRL_id") < 0 Then
								oDataview.Delete(0)
								isDelete = True
							End If
						End If
					End If
					If TipocomunitaID <> -1 And Not isDelete Then
						If TipocomunitaID <> ComunitaTipoID Then
							oDataview.Delete(0)
							isDelete = True
						Else
							
						End If
					ElseIf Not isDelete Then

					End If
					If Not isDelete Then
						Select Case oFiltroTipoRicerca
							Case Main.FiltroComunita.contiene
								If valore <> "" Then
									If InStr(oDataview.Item(0).Row.Item("CMNT_Nome"), valore) < 1 Then
										oDataview.Delete(0)
										isDelete = True
									End If
								End If
							Case Main.FiltroComunita.IDresponsabile
								If IsNumeric(valore) And Me.HDN_filtroResponsabileID.Value <> "-1" Then
									If oDataview.Item(0).Row.Item("ALCM_ResponsabileID") <> Me.HDN_filtroResponsabileID.Value Then
										oDataview.Delete(0)
										isDelete = True
									End If
								End If
							Case Main.FiltroComunita.nome
								If valore <> "" Then
									If InStr(oDataview.Item(0).Row.Item("CMNT_Nome"), valore) <> 1 Then
										oDataview.Delete(0)
										isDelete = True
									End If
								End If
							Case Main.FiltroComunita.creataDopo
								Try
									If valore < oDataview.Item(0).Row.Item("CMNT_dataCreazione") Then
										oDataview.Delete(0)
										isDelete = True
									End If
								Catch ex As Exception

								End Try
							Case Main.FiltroComunita.creataPrima
								Try
									If valore > oDataview.Item(0).Row.Item("CMNT_dataCreazione") Then
										oDataview.Delete(0)
										isDelete = True
									End If
								Catch ex As Exception

								End Try
							Case Main.FiltroComunita.dataFineIscrizionePrima
								Try
									If valore > oDataview.Item(0).Row.Item("CMNT_dataInizioIscrizione") Then
										oDataview.Delete(0)
										isDelete = True
									End If
								Catch ex As Exception

								End Try
							Case Main.FiltroComunita.dataIscrizioneDopo
								Try
									If valore < oDataview.Item(0).Row.Item("CMNT_dataFineIscrizione") Then
										oDataview.Delete(0)
										isDelete = True
									End If
								Catch ex As Exception

								End Try
						End Select
					End If

					If Not isDelete Then
						Dim Filtro As String
						Dim j As Integer

						Filtro = oDataview.RowFilter
						oDataview.RowFilter = "NoDelete <> 1 and ('" & ComunitaPercorso & "' like '%.' + CMNT_ID + '.%')"
						While oDataview.Count > 0
							oDataview.Item(0).Item("NoDelete") = 1
							oDataview.RowFilter = "NoDelete <> 1 and ('" & ComunitaPercorso & "' like '%.' + CMNT_ID + '.%')"
						End While
						oDataview.RowFilter = Filtro
					End If
				End While
				Livello -= 1
				oDataview.RowFilter = "ALCM_Livello=" & Livello & " AND NoDelete <> 1"
			End While
			oDataview.RowFilter = ""
			oDataset.AcceptChanges()

			If Me.LNBalbero.Visible = True Then
				Me.GeneraNodiOrganizzativi(oDataset)
			End If
			Dim ImageBaseDir, img As String
			ImageBaseDir = GetPercorsoApplicazione(Me.Request)
			ImageBaseDir = ImageBaseDir & "/RadControls/TreeView/Skins/Comunita/logo/"
			ImageBaseDir = Replace(ImageBaseDir, "//", "/")

			totale = oDataset.Tables(0).Rows.Count

            For i = 0 To totale - 1
                Dim oRow As DataRow

                oRow = oDataset.Tables(0).Rows(i)

                If IsDBNull(oRow.Item("CMNT_Responsabile")) Then
                    oRow.Item("AnagraficaResponsabile") = oResource.getValue("creata")
                    oRow.Item("AnagraficaResponsabile") = oRow.Item("AnagraficaResponsabile").Replace("#%%#", oRow.Item("AnagraficaCreatore"))
                Else
                    If oRow.Item("CMNT_Responsabile") = "" Then
                        oRow.Item("AnagraficaResponsabile") = oResource.getValue("creata")
                        oRow.Item("AnagraficaResponsabile") = oRow.Item("AnagraficaResponsabile").Replace("#%%#", oRow.Item("AnagraficaCreatore"))
                    Else
                        oRow.Item("AnagraficaResponsabile") = "&nbsp;(" & oRow.Item("CMNT_Responsabile") & ") &nbsp;"
                    End If
                End If
                If oRow.Item("CMNT_IsChiusa") = True Then
                    oRow.Item("Proprieta") = oResource.getValue("stato.image." & oRow.Item("CMNT_IsChiusa"))
                    oRow.Item("Alternative") = oResource.getValue("stato." & oRow.Item("CMNT_IsChiusa"))
                ElseIf oRow.Item("ALCM_isChiusaForPadre") = True Then
                    oRow.Item("Proprieta") = oResource.getValue("stato.image." & oRow.Item("ALCM_isChiusaForPadre"))
                    oRow.Item("Alternative") = oResource.getValue("stato." & oRow.Item("ALCM_isChiusaForPadre"))
                Else
                    oRow.Item("Proprieta") = oResource.getValue("stato.image." & oRow.Item("CMNT_IsChiusa"))
                    oRow.Item("Alternative") = oResource.getValue("stato." & oRow.Item("CMNT_IsChiusa"))
                End If
                If IsDBNull(oRow.Item("TPCM_icona")) = False Then
                    img = oRow.Item("TPCM_icona")
                    'img = ImageBaseDir & Mid(img, InStrRev(img, "/", img.Length - 1) + 1, img.Length)
                    img = Mid(img, InStrRev(img, "/", img.Length - 1) + 1, img.Length)
                    oRow.Item("TPCM_icona") = img
                End If
                If IsDate(oRow.Item("CMNT_dataInizioIscrizione")) Then
                    If Not Equals(New Date, oRow.Item("CMNT_dataInizioIscrizione")) Then
                        oRow.Item("CMNT_dataInizioIscrizione") = FormatDateTime(oRow.Item("CMNT_dataInizioIscrizione"), DateFormat.GeneralDate)
                    End If
                End If
                If IsDate(oRow.Item("CMNT_dataFineIscrizione")) Then
                    If Not Equals(New Date, oRow.Item("CMNT_dataFineIscrizione")) Then
                        oRow.Item("CMNT_dataFineIscrizione") = FormatDateTime(oRow.Item("CMNT_dataFineIscrizione"), DateFormat.GeneralDate)
                    End If
                End If
                If IsDBNull(oRow.Item("CMNT_AnnoAccademico")) Then
                    oRow.Item("AnnoAccademico") = "&nbsp;"
                Else
                    oRow.Item("AnnoAccademico") = oRow.Item("CMNT_AnnoAccademico")
                End If
                Try
                    Dim numIscritti, maxIscritti As Integer
                    maxIscritti = oRow.Item("CMNT_MaxIscritti")
                    numIscritti = oRow.Item("CMNT_Iscritti")
                    Try
                        oRow.Item("Iscritti") = numIscritti
                    Catch ex As Exception
                        oRow.Item("Iscritti") = 0
                        numIscritti = 0
                    End Try

                    If (maxIscritti <= 0) Then
                        oRow.Item("CMNT_Iscritti") = 0
                    Else
                        If numIscritti > maxIscritti Then
                            oRow.Item("CMNT_Iscritti") = maxIscritti - numIscritti
                            oRow.Item("Iscritti") = oResource.getValue("limiti.superato")
                            oRow.Item("Iscritti") = Replace(oRow.Item("Iscritti"), "#num1#", "<b>" & numIscritti & "</b>")
                            oRow.Item("Iscritti") = Replace(oRow.Item("Iscritti"), "#limite#", maxIscritti)
                            oRow.Item("Iscritti") = Replace(oRow.Item("Iscritti"), "#numOver#", numIscritti - maxIscritti)
                        ElseIf numIscritti = maxIscritti Then
                            oRow.Item("CMNT_Iscritti") = -1
                            oRow.Item("Iscritti") = numIscritti & " " & oResource.getValue("limiti") & " <b>" & maxIscritti & "</b>"
                        Else
                            oRow.Item("CMNT_Iscritti") = maxIscritti - numIscritti
                            oRow.Item("Iscritti") = numIscritti & " " & oResource.getValue("limiti") & " <b>" & maxIscritti & "</b>"
                        End If
                    End If
                Catch ex As Exception

                End Try

                Try
                    If Me.PageUtility.CommunityNewsCount(Me.PageUtility.CurrentUser.ID, CInt(oRow.Item("CMNT_ID"))).Count <= 0 Then
                        oRow.Item("HasNews") = False
                    Else
                        oRow.Item("HasNews") = True
                    End If
                Catch ex As Exception
                    oRow.Item("HasNews") = False
                End Try
                Try
                    If oRow.Item("ALCM_Path") = oRow.Item("ALCM_RealPath") Then
                        oRow.Item("ALCM_RealPath") = ""
                    End If
                Catch ex As Exception

                End Try
            Next
		Catch ex As Exception

		End Try
		Return oDataset
	End Function

	Private Sub GenerateTreeView(Optional ByVal ApplicaFiltri As Boolean = False)
		Dim oPersona As New COL_Persona
		Dim oDataset As New DataSet
		Dim i, totale As Integer
		Try
			oPersona = Session("objPersona")
			Dim oFiltroAlbero As Main.ElencoRecord = Main.ElencoRecord.AdAlbero
			If Me.LNBalbero.Visible = True Then
				oFiltroAlbero = Main.ElencoRecord.AdAlberoOrganizzativo
			End If

			oDataset = Filtraggio_Dati(ApplicaFiltri)
			Me.RDTcomunita.Nodes.Clear()


			Dim nodeRoot As New RadTreeNode
			nodeRoot.Expanded = True
			nodeRoot.ImageUrl = "folder.gif"
			nodeRoot.Value = ""
			Try
				nodeRoot.Text = oResource.getValue("oRootNode." & Me.RBLricercaByIscrizione.SelectedValue & ".Text")
				If nodeRoot.Text = "" Then
					nodeRoot.Text = "Comunità: "
				End If
			Catch ex As Exception
				nodeRoot.Text = "Comunità: "
			End Try
			Try
				nodeRoot.ToolTip = oResource.getValue("oRootNode." & Me.RBLricercaByIscrizione.SelectedValue & ".ToolTip")
				If nodeRoot.ToolTip = "" Then
					If Me.RBLricercaByIscrizione.SelectedValue = Me.stringaNodoBase.iscritte Then
						nodeRoot.ToolTip = "Elenco comunità a cui si è iscritti"
					ElseIf Me.RBLricercaByIscrizione.SelectedValue = Me.stringaNodoBase.nonIscritte Then
						nodeRoot.ToolTip = "Elenco comunità a cui iscriversi"
					Else
						nodeRoot.ToolTip = "Elenco comunità"
					End If

				End If
			Catch ex As Exception
				If nodeRoot.ToolTip = "" Then
					If Me.RBLricercaByIscrizione.SelectedValue = Me.stringaNodoBase.iscritte Then
						nodeRoot.ToolTip = "Elenco comunità a cui si è iscritti"
					ElseIf Me.RBLricercaByIscrizione.SelectedValue = Me.stringaNodoBase.nonIscritte Then
						nodeRoot.ToolTip = "Elenco comunità a cui iscriversi"
					Else
						nodeRoot.ToolTip = "Elenco comunità"
					End If
				End If
			End Try

			Me.CreateContextMenu(nodeRoot, False, True)
			Me.RDTcomunita.Nodes.Add(nodeRoot)
			If oDataset.Tables(0).Rows.Count = 0 Then
				' nessuna comunità a cui si è iscritti
				Me.GeneraNoNode()
			Else
				oDataset.Relations.Add("NodeRelation", oDataset.Tables(0).Columns("ALCM_PAth"), oDataset.Tables(0).Columns("ALCM_RealPath"), False)

				Dim ComunitaPath As String = ""
				If IsArray(Session("ArrComunita")) Then
					Try
						Dim ArrComunita(,) As String
						ArrComunita = Session("ArrComunita")
						ComunitaPath = ArrComunita(2, UBound(ArrComunita, 2))
					Catch ex As Exception

					End Try
				End If

				Dim dbRow As DataRow
				For Each dbRow In oDataset.Tables(0).Rows
					If dbRow("ALCM_PadreVirtuale_ID") = 0 Or ComunitaPath = dbRow("ALCM_RealPath") Then
						Dim node As RadTreeNode = CreateNode(dbRow, True)
						nodeRoot.Nodes.Add(node)
						RecursivelyPopulate(dbRow, node)
					End If
				Next dbRow
				Me.PNLtreeView.Visible = True
			End If
		Catch ex As Exception
			Me.PNLtreeView.Visible = False
		End Try
	End Sub	'GenerateTreeView

	Private Sub RecursivelyPopulate(ByVal dbRow As DataRow, ByVal node As RadTreeNode)
		Dim childRow As DataRow

		For Each childRow In dbRow.GetChildRows("NodeRelation")
			Dim childNode As RadTreeNode = CreateNode(childRow, False)
			node.Nodes.Add(childNode)
			RecursivelyPopulate(childRow, childNode)
			If childNode.Nodes.Count = 0 Then
				childNode.Value = childNode.Value
			End If
		Next childRow
	End Sub
	Private Function CreateNode(ByVal dbRow As DataRow, ByVal expanded As Boolean) As RadTreeNode
		Dim node As New RadTreeNode

		Dim start As Integer
		Dim [continue] As Boolean = False
		Dim numIscritti, maxIscritti, iscritti As Integer
		start = 0

		Dim CMNT_id, RLPC_TPRL_id As Integer
		Dim CMNT_Responsabile, img As String
		Dim isIscritto As Boolean = False

		Try
			CMNT_id = dbRow.Item("CMNT_id")
			If IsDBNull(dbRow.Item("RLPC_TPRL_id")) Then
				RLPC_TPRL_id = -1
			Else
				RLPC_TPRL_id = dbRow.Item("RLPC_TPRL_id")

				If RLPC_TPRL_id > -1 Then
					isIscritto = True
				End If
			End If

			Dim ImageBaseDir As String
			ImageBaseDir = GetPercorsoApplicazione(Me.Request)
			ImageBaseDir = ImageBaseDir & Me.RDTcomunita.ImagesBaseDir().Replace("~", "")

			Dim CMNT_Nome, CMNT_NomeVisibile, CMNT_REALpath, CMNT_path As String
			Dim CMNT_IsChiusa As Boolean

			CMNT_Nome = dbRow.Item("CMNT_Nome")
			CMNT_NomeVisibile = CMNT_Nome
			CMNT_IsChiusa = dbRow.Item("CMNT_IsChiusa")
			If dbRow.Item("ALCM_isChiusaForPadre") = True Then
				CMNT_IsChiusa = True
			End If

			If CMNT_id > 0 Then
				If IsDBNull(dbRow.Item("CMNT_Iscritti")) = False Then
					maxIscritti = dbRow.Item("CMNT_MaxIscritti")
					numIscritti = dbRow.Item("CMNT_Iscritti")

					If maxIscritti <= 0 Then
						dbRow.Item("CMNT_Iscritti") = 0
						iscritti = 0
					Else
						If numIscritti > maxIscritti Then
							dbRow.Item("CMNT_Iscritti") = maxIscritti - numIscritti
							iscritti = maxIscritti - numIscritti
						ElseIf numIscritti = maxIscritti Then
							dbRow.Item("CMNT_Iscritti") = -1
							iscritti = -1
						Else
							dbRow.Item("CMNT_Iscritti") = maxIscritti - numIscritti
							iscritti = maxIscritti - numIscritti
						End If
					End If
				Else
					dbRow.Item("CMNT_Iscritti") = 0
				End If

				CMNT_Responsabile = dbRow.Item("AnagraficaResponsabile")

				If IsDBNull(dbRow.Item("TPCM_icona")) Then
					img = ""
				Else
					img = "./logo/" & dbRow.Item("TPCM_icona")
				End If

				CMNT_Nome = CMNT_Nome & CMNT_Responsabile
				CMNT_NomeVisibile = CMNT_Nome

				CMNT_Nome = CMNT_Nome & Me.GenerateImage(ImageBaseDir & oResource.getValue("stato.image." & CMNT_IsChiusa), oResource.getValue("stato." & CMNT_IsChiusa))

				'If dbRow.IsNull("CMNT_AnnoAccademico") = False Then
				'    If dbRow.Item("CMNT_AnnoAccademico") <> "" Then
				'        CMNT_Nome = CMNT_Nome & "&nbsp;(" & dbRow.Item("CMNT_AnnoAccademico") & ")&nbsp;"
				'    End If
				'End If
			Else
				CMNT_NomeVisibile = CMNT_Nome
			End If
			'If IsDBNull(dbRow.Item("CMNT_path")) Then
			'    CMNT_path = "." & CMNT_idPadre & "."
			'Else
			'    CMNT_path = oRow.Item("CMNT_path")
			'End If

			'If IsDBNull(oRow.Item("CMNT_REALpath")) Then
			'    CMNT_REALpath = "." & CMNT_idPadre & "."
			'Else
			CMNT_path = dbRow.Item("ALCM_path")
			CMNT_REALpath = dbRow.Item("ALCM_path")	'dbRow.Item("ALCM_REALpath")
			'End If


			Dim ForSubscribe As Boolean = False
			Dim ForEnter As Boolean = False
			Dim ForDetails As Boolean = True
			Dim dataStringa As String = ""

			If CMNT_id > 0 Then
				If isIscritto And RLPC_TPRL_id <> -2 And RLPC_TPRL_id <> -3 Then
					ForEnter = True
				Else
					Dim CMNT_dataInizioIscrizione, CMNT_dataFineIscrizione As DateTime

					If dbRow.Item("CMNT_Iscritti") = 0 Or dbRow.Item("CMNT_Iscritti") > 0 Then
						If IsDate(dbRow.Item("CMNT_dataInizioIscrizione")) Then
							CMNT_dataInizioIscrizione = dbRow.Item("CMNT_dataInizioIscrizione")
							If CMNT_dataInizioIscrizione > Now Then
								'' devo iscrivermi, ma iscrizioni non aperte !
								'dataStringa = oResource.getValue("iscrizioni." & Me.Iscrizioni_code.IscrizioniAperteIl)
								'dataStringa = dataStringa.Replace("#%%#", CMNT_dataInizioIscrizione)
								'CMNT_Nome = CMNT_Nome & "&nbsp;&nbsp;" & dataStringa
							Else
								If IsDate(dbRow.Item("CMNT_dataFineIscrizione")) Then
									Dim DataTemp As DateTime
									CMNT_dataFineIscrizione = dbRow.Item("CMNT_dataFineIscrizione")

									DataTemp = CMNT_dataFineIscrizione.Date()
									DataTemp = DataTemp.AddHours(23)
									DataTemp = DataTemp.AddMinutes(59)
									CMNT_dataFineIscrizione = DataTemp

									If CMNT_dataFineIscrizione < Now Then
										'CMNT_Nome = CMNT_Nome & "&nbsp;&nbsp;" & oResource.getValue("iscrizioni." & Me.Iscrizioni_code.IscrizioniChiuse)
									Else
										'dataStringa = oResource.getValue("iscrizioni." & Me.Iscrizioni_code.IscrizioniEntro)
										'dataStringa = dataStringa.Replace("#%%#", CMNT_dataFineIscrizione)
										'CMNT_Nome = CMNT_Nome & "&nbsp;&nbsp;" & dataStringa
										ForSubscribe = True
									End If
								Else
									ForSubscribe = True
								End If
							End If
						Else
							ForSubscribe = True
						End If
					ElseIf RLPC_TPRL_id = -2 Then
						ForSubscribe = True
					Else
						'Non c'è spazio per nuovi iscritti !!!
						'CMNT_Nome = CMNT_Nome & "&nbsp;&nbsp;" & oResource.getValue("iscrizioni." & Me.Iscrizioni_code.IscrizioniComplete)
					End If
				End If
			End If

			If CMNT_id > 0 Then
				Dim HasNews As Boolean = False
				Try
					If dbRow.Item("HasNews") = True And ForEnter Then
						CMNT_Nome = CMNT_Nome & "&nbsp;" & Me.GenerateImage("./../images/HasNews.gif", "")
						HasNews = True
					End If
				Catch ex As Exception

				End Try

				If dbRow.Item("CMNT_CanSubscribe") = False Then
					ForSubscribe = False
				End If
				If ForSubscribe Then
					If dbRow.Item("CMNT_Archiviata") Or dbRow.Item("CMNT_Bloccata") Then
						ForSubscribe = False
					End If
				End If
				If dbRow.Item("CMNT_Bloccata") Then
					ForEnter = False
					HasNews = False
				End If

				If dbRow.Item("CMNT_AccessoCopisteria") = 0 And Session("objPersona").tipoPersona.id = Main.TipoPersonaStandard.Copisteria Then
					ForSubscribe = False
					HasNews = False
				End If
				Select Case Me.RBLstatoComunita.SelectedValue
					Case 0 'attivate
						If dbRow.Item("CMNT_Bloccata") Then
							CMNT_Nome = CMNT_Nome & "&nbsp;" & Me.oResource.getValue("status.Bloccata")
						ElseIf dbRow.Item("CMNT_Archiviata") Then
							CMNT_Nome = CMNT_Nome & "&nbsp;" & Me.oResource.getValue("status.Archiviata")
						End If
					Case 1 ' archiviate
						If dbRow.Item("CMNT_Bloccata") Then
							CMNT_Nome = CMNT_Nome & "&nbsp;" & Me.oResource.getValue("status.Bloccata")
						End If
					Case 2 ' bloccate
						If dbRow.Item("CMNT_Archiviata") Then
							CMNT_Nome = CMNT_Nome & "&nbsp;" & Me.oResource.getValue("status.Archiviata")
						End If
				End Select


			
					Try
                    If Session("idComunita") = CMNT_id Then
                        Me.CreateContextMenu(node, False, False, False, False, False)
                    Else
                        Me.CreateContextMenu(node, ForDetails, False, ForEnter, ForSubscribe, HasNews)
                    End If
					Catch ex As Exception

					End Try


			End If

			node.Text = CMNT_Nome
			node.Value = CMNT_id & "," & CMNT_REALpath
			node.Expanded = expanded
			node.ImageUrl = img
			node.ToolTip = CMNT_NomeVisibile
			node.Category = CMNT_IsChiusa


			node.Checkable = True
			If RLPC_TPRL_id = -2 Then
				node.CssClass = "TreeNodeDisabled"
			ElseIf RLPC_TPRL_id = -3 Then
				node.CssClass = "TreeNodeDisabled"
			ElseIf isIscritto = False Then
				node.CssClass = "TreeNodeDisabled"
			End If
			node.Checkable = True

		Catch ex As Exception
			Return Nothing
		End Try

		Return node
	End Function 'CreateNode


	Private Function GeneraNoNode()
		Dim oRootNode As New RadTreeNode
		Dim oNode As New RadTreeNode

		oRootNode = New RadTreeNode
		oRootNode.Value = ""
		oRootNode.Expanded = True
		oRootNode.ImageUrl = "folder.gif"

		oRootNode.Category = 0
		Try
			oRootNode.Text = oResource.getValue("oRootNode." & Me.RBLricercaByIscrizione.SelectedValue & ".Text")
			If oRootNode.Text = "" Then
				oRootNode.Text = "Comunità: "
			End If
		Catch ex As Exception
			oRootNode.Text = "Comunità: "
		End Try
		Try
			oRootNode.ToolTip = oResource.getValue("oRootNode." & Me.RBLricercaByIscrizione.SelectedValue & ".ToolTip")
			If oRootNode.ToolTip = "" Then
				If Me.RBLricercaByIscrizione.SelectedValue = Me.stringaNodoBase.iscritte Then
					oRootNode.ToolTip = "Elenco comunità a cui si è iscritti"
				ElseIf Me.RBLricercaByIscrizione.SelectedValue = Me.stringaNodoBase.nonIscritte Then
					oRootNode.ToolTip = "Elenco comunità a cui iscriversi"
				Else
					oRootNode.ToolTip = "Elenco comunità"
				End If

			End If
		Catch ex As Exception
			If oRootNode.ToolTip = "" Then
				If Me.RBLricercaByIscrizione.SelectedValue = Me.stringaNodoBase.iscritte Then
					oRootNode.ToolTip = "Elenco comunità a cui si è iscritti"
				ElseIf Me.RBLricercaByIscrizione.SelectedValue = Me.stringaNodoBase.nonIscritte Then
					oRootNode.ToolTip = "Elenco comunità a cui iscriversi"
				Else
					oRootNode.ToolTip = "Elenco comunità"
				End If

			End If
		End Try
		Me.CreateContextMenu(oRootNode, False, True)

		oNode = New RadTreeNode
		oNode.Expanded = True
		oNode.Value = ""
		oNode.Category = 0
		oNode.Checkable = False

		Try
			oNode.Text = oResource.getValue("oNode." & Me.RBLricercaByIscrizione.SelectedValue & ".Text")
			If oNode.Text = "" Then
				If Me.RBLricercaByIscrizione.SelectedValue = Me.stringaNodoBase.iscritte Then
					oNode.Text = "Non si è iscritti ad alcuna comunità"
				ElseIf Me.RBLricercaByIscrizione.SelectedValue = Me.stringaNodoBase.nonIscritte Then
					oNode.Text = "Non vi è alcuna comunità a cui iscriversi"
				Else
					oNode.Text = "Non vi è alcuna comunità"
				End If
			End If
		Catch ex As Exception
			oNode.Text = "Non vi è alcuna comunità"
		End Try

		Try
			oNode.ToolTip = oResource.getValue("oNode." & Me.RBLricercaByIscrizione.SelectedValue & ".ToolTip")
			If oNode.ToolTip = "" Then
				If Me.RBLricercaByIscrizione.SelectedValue = Me.stringaNodoBase.iscritte Then
					oNode.ToolTip = "Non si è iscritti ad alcuna comunità"
				ElseIf Me.RBLricercaByIscrizione.SelectedValue = Me.stringaNodoBase.nonIscritte Then
					oNode.ToolTip = "Non vi è alcuna comunità a cui iscriversi"
				Else
					oNode.ToolTip = "Non vi è alcuna comunità"
				End If

			End If
		Catch ex As Exception
			If oRootNode.ToolTip = "" Then
				If Me.RBLricercaByIscrizione.SelectedValue = Me.stringaNodoBase.iscritte Then
					oNode.ToolTip = "Non si è iscritti ad alcuna comunità"
				ElseIf Me.RBLricercaByIscrizione.SelectedValue = Me.stringaNodoBase.nonIscritte Then
					oNode.ToolTip = "Non vi è alcuna comunità a cui iscriversi"
				Else
					oNode.ToolTip = "Non vi è alcuna comunità"
				End If
			End If
		End Try
		oRootNode.Nodes.Add(oNode)

		Me.RDTcomunita.Nodes.Clear()
		Me.RDTcomunita.Nodes.Add(oRootNode)
	End Function
	Private Function GenerateImage(ByVal ImageName As String, Optional ByVal Status As String = "") As String
		Dim imageUrl As String
		Dim quote As String
		quote = """"

		imageUrl = "<img  align=absmiddle src=" & quote & ImageName & quote & " alt=" & quote & Status & quote

		imageUrl = imageUrl & " " & " onmouseover=" & quote & "window.status='" & Replace(Status, "'", "\'") & "';return true;" & quote & " "
		imageUrl = imageUrl & " " & " onfocus=" & quote & "window.status='" & Replace(Status, "'", "\'") & "';return true;" & quote & " "
		imageUrl = imageUrl & " " & " onmouseout=" & quote & "window.status='';return true;" & """" & " "
		imageUrl = imageUrl & " >"

		Return imageUrl
	End Function

	Private Sub RDTcomunita_NodeContextClick(ByVal o As Object, ByVal e As Telerik.WebControls.RadTreeNodeEventArgs) Handles RDTcomunita.NodeContextClick
		Dim isChiusa, isIscritto As Boolean
		Dim ComunitaID As Integer
		Dim ComunitaPath, Elenco() As String
		Dim oNode As Telerik.WebControls.RadTreeNode
		oNode = e.NodeClicked

		Try
			Dim oRuoloComunita As New COL_RuoloPersonaComunita

			isChiusa = CBool(oNode.Category)
			isIscritto = False
			Elenco = oNode.Value.Split(",")

			ComunitaID = CInt(Elenco(0))
			oRuoloComunita.EstraiByLinguaDefault(ComunitaID, Session("objPersona").id)
			If oRuoloComunita.Errore = Errori_Db.None Then
				If oRuoloComunita.TipoRuolo.Id > 0 Then
					isIscritto = True
				End If
			End If

			ComunitaPath = Elenco(1)
			If InStr(ComunitaPath, "-") > 0 Then
				Dim start, endRiga As Integer
				start = InStr(ComunitaPath, "-") - 1
				endRiga = InStr(start + 1, ComunitaPath, ".")
				ComunitaPath = ComunitaPath.Remove(start, endRiga - start)
			End If
			Select Case e.ContextMenuItemID
				Case AzioneTree.Aggiorna
					Me.Bind_TreeView(True)
				Case AzioneTree.Dettagli
					Me.PageUtility.AddActionToModule(Me.PageUtility.GetModule(Services_ElencaComunita.Codex).ID, Services_ElencaComunita.ActionType.CommunityDetails, Me.PageUtility.CreateObjectsList(Services_ElencaComunita.ObjectType.Community, ComunitaID))

					Me.HDN_Path.Value = ComunitaPath
					Me.HDisChiusa.Value = isChiusa
					Me.HDNcmnt_ID.Value = ComunitaID
					Me.PNLmessaggio.Visible = False

					Me.VisualizzaDettagli(ComunitaPath, isIscritto, isChiusa)
				Case AzioneTree.Entra

					Me.PageUtility.AddActionToModule(Me.PageUtility.GetModule(Services_ElencaComunita.Codex).ID, Services_ElencaComunita.ActionType.Access, Me.PageUtility.CreateObjectsList(Services_ElencaComunita.ObjectType.Community, ComunitaID))
					Me.HDN_Path.Value = ComunitaPath
					Me.PNLmessaggio.Visible = False
					Session("Azione") = "entra"
					Me.Entra_Comunita(ComunitaPath)
				Case AzioneTree.Iscrivi
					Dim oComunita As New COL_Comunita
					Dim oImpostazioni As New COL_ImpostazioniUtente
					Dim exitSub As Boolean = False
					Try
						oImpostazioni = Session("oImpostazioni")
						exitSub = Not oImpostazioni.ShowConferma
					Catch ex As Exception
						exitSub = False
					End Try

					oComunita.Id = ComunitaID
					oComunita.Estrai()
					If oComunita.Errore = Errori_Db.None Then
						If (oComunita.CanSubscribe And oComunita.Archiviata = False And oComunita.Bloccata = False) Then
							If Not exitSub Then
								Session("azione") = "iscrivi"
								Me.HDNcmnt_ID.Value = ComunitaID
								Me.HDisChiusa.Value = isChiusa
								Me.HDN_Path.Value = ComunitaPath


								Me.ResetForm_toConferma()
								Me.oResource.setLabel(Me.LBconferma)
								Me.LBconferma.Text = Replace(Me.LBconferma.Text, "#nomeComunita#", oComunita.Nome)
								Me.LBconferma.Text = Replace(Me.LBconferma.Text, "#nomeResponsabile#", oComunita.GetNomeResponsabile_NomeCreatore)
							Else
								If Session("azione") <> "iscrivi" Then
									Session("azione") = "iscrivi"
									Me.HDNcmnt_ID.Value = ComunitaID
									Me.HDisChiusa.Value = isChiusa
									Me.HDN_Path.Value = ComunitaPath
									Me.Iscrivi(ComunitaID, ComunitaPath)
								Else
									Me.ResetForm(True)
								End If
							End If
						Else
							Dim alertMSG As String = ""
							If Not oComunita.Bloccata Then
								alertMSG = oResource.getValue("messaggio.Bloccata")
								If alertMSG <> "" Then
									alertMSG = alertMSG.Replace("'", "\'")
								Else
									alertMSG = "Non è possibile iscriversi ad una comunità bloccata !"
								End If
							ElseIf Not oComunita.Archiviata Then
								alertMSG = oResource.getValue("messaggio.Archiviata")
								If alertMSG <> "" Then
									alertMSG = alertMSG.Replace("'", "\'")
								Else
									alertMSG = "Non è possibile iscriversi ad una comunità archiviata !"
								End If
							Else
								alertMSG = oResource.getValue("messaggio.NoIscrizione")
								If alertMSG <> "" Then
									alertMSG = alertMSG.Replace("'", "\'")
								Else
									alertMSG = "Non è possibile iscriversi alla comunità selezionata !"
								End If
							End If
							Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
							Me.ResetForm(False)
						End If
					Else
						Me.ResetForm(True)
					End If

				Case AzioneTree.Novità
					If Me.LNBalbero.Visible Then
						Me.SaveSearchParameters(2)
					Else
						Me.SaveSearchParameters(1)
					End If

					Session("CMNT_path_forNews") = ComunitaPath
					Session("CMNT_ID_forNews") = ComunitaID
					PageUtility.RedirectToUrl("generici/News_Comunita.aspx?from=RicercaComunitaAlbero")
			End Select
		Catch ex As Exception

		End Try
	End Sub

	Private Sub GeneraNodiOrganizzativi(ByVal oDataset As DataSet)
		Try
			Dim VirtualeID As Integer = -100
            Me.GeneraNodiAltreComunita(oDataset, VirtualeID)
			VirtualeID -= 1
		Catch ex As Exception

		End Try
	End Sub
    Private Sub GeneraNodiAltreComunita(ByVal oDataset As DataSet, ByRef VirtualeID As Integer)
        Try
            Try
                Dim oDataview As DataView
                Dim Filtro As String
                oDataview = oDataset.Tables(0).DefaultView

                oDataview.RowFilter = "ALCM_Livello=1 AND ALCM_RealPath not like '%-%' AND CMNT_ID > 0"
                If oDataview.Count > 0 Then
                    Dim ALCM_RealPath, ALCM_Path As String
                    VirtualeID -= 1

                    Filtro = oDataview.RowFilter
                    While oDataview.Count > 0
                        ALCM_RealPath = oDataview.Item(0).Item("ALCM_RealPath")
                        ALCM_Path = ALCM_RealPath & VirtualeID & "."

                        Me.GeneraNodiAltriTipiComunita(oDataset, oDataview, VirtualeID)
                        oDataview.RowFilter = "ALCM_Livello=1 AND ALCM_RealPath not like '%-%'  AND CMNT_ID > 0"
                        If oDataview.Count > 0 Then
                            VirtualeID -= 1
                        End If
                    End While
                End If
            Catch ex As Exception

            End Try
        Catch ex As Exception

        End Try
    End Sub

	Private Sub GeneraNodiAltriTipiComunita(ByVal oDataset As DataSet, ByVal oDataview As DataView, ByRef VirtualeID As Integer)
		Try
			While oDataview.Count > 0
				Dim oRow As DataRow
				Dim oRowTesi As DataRow
				oRow = oDataset.Tables(0).NewRow
				oRow.Item("ALCM_RealPath") = oDataview.Item(0).Item("ALCM_RealPath")
				oRow.Item("ALCM_Path") = oDataview.Item(0).Item("ALCM_RealPath") & VirtualeID & "."
				oRow.Item("CMNT_ID") = VirtualeID
				oRow.Item("CMNT_Nome") = oDataview.Item(0).Item("TPCM_Descrizione")
				oRow.Item("ALCM_PadreVirtuale_ID") = oDataview.Item(0).Item("ALCM_PadreVirtuale_ID")
				oRow.Item("ALCM_PadreID") = oDataview.Item(0).Item("ALCM_PadreVirtuale_ID")
				oRow.Item("ALCM_PercorsoDiretto") = True
				oRow.Item("CMNT_IsChiusa") = False
				oRow.Item("ALCM_isChiusaForPadre") = False
				oDataset.Tables(0).Rows.Add(oRow)

				For Each oRowTesi In oDataset.Tables(0).Select("ALCM_Livello=1 AND CMNT_TPCM_ID=" & oDataview.Item(0).Item("CMNT_TPCM_ID") & " AND ALCM_RealPath='" & oRow.Item("ALCM_RealPath") & "'")
					oRowTesi.Item("ALCM_RealPath") = oRow.Item("ALCM_Path")
				Next

				oDataview.RowFilter &= " AND ALCM_RealPath='" & oRow.Item("ALCM_RealPath") & "' AND ALCM_RealPath<>'" & oRow.Item("ALCM_Path") & "' AND CMNT_TPCM_ID<>" & oRow.Item("CMNT_TPCM_ID")
				If oDataview.Count > 0 Then
					VirtualeID -= 1
				End If
			End While

		Catch ex As Exception

		End Try
	End Sub
	Private Sub GeneraNodiTipoCorsoDiLaurea(ByVal oDataset As DataSet, ByVal oDataview As DataView, ByRef VirtualeID As Integer)
		Try
			While oDataview.Count > 0
				Dim oRow As DataRow
				Dim oRowTesi As DataRow
				oRow = oDataset.Tables(0).NewRow
				oRow.Item("ALCM_RealPath") = oDataview.Item(0).Item("ALCM_RealPath")
				oRow.Item("ALCM_Path") = oDataview.Item(0).Item("ALCM_RealPath") & VirtualeID & "."
				oRow.Item("CMNT_ID") = VirtualeID
				oRow.Item("CMNT_Nome") = oDataview.Item(0).Item("TPCS_nome")
				oRow.Item("ALCM_PadreVirtuale_ID") = oDataview.Item(0).Item("ALCM_PadreVirtuale_ID")
				oRow.Item("ALCM_PadreID") = oDataview.Item(0).Item("ALCM_PadreVirtuale_ID")
				oRow.Item("ALCM_PercorsoDiretto") = True
				oRow.Item("CMNT_IsChiusa") = False
				oRow.Item("ALCM_isChiusaForPadre") = False
				oDataset.Tables(0).Rows.Add(oRow)



				oDataview.RowFilter &= " AND ALCM_RealPath='" & oRow.Item("ALCM_RealPath") & "' AND ALCM_RealPath<>'" & oRow.Item("ALCM_Path") & "' AND TPCS_nome<>'" & oRow.Item("CMNT_Nome") & "'"
				If oDataview.Count > 0 Then
					VirtualeID -= 1
				End If
			End While

		Catch ex As Exception

		End Try
	End Sub
#End Region

	Private Sub LNBiscriviConferma_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBiscriviConferma.Click

		Dim iResponse As Main.ErroriIscrizioneComunita
		Dim oResourceConfig As New ResourceManager
		Dim oPersona As New COL_Persona

		If Session("azione") = "iscrivi" Then
			Me.Iscrivi(Me.HDNcmnt_ID.Value, Me.HDN_Path.Value)
		Else
			Me.ResetForm(True)
		End If
	End Sub
	Private Sub LNBannullaConferma_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBannullaConferma.Click
		Me.ResetForm(False)
	End Sub

	Private Sub Iscrivi(ByVal ComunitaID As Integer, ByVal ComunitaPath As String)
		Dim iResponse As Main.ErroriIscrizioneComunita
		Dim oPersona As New COL_Persona

		If Session("azione") = "iscrivi" Then
			Me.PageUtility.AddActionToModule(Me.PageUtility.GetModule(Services_IscrizioneComunita.Codex).ID, Services_IscrizioneComunita.ActionType.SubscribeCommunity, Me.PageUtility.CreateObjectsList(Services_IscrizioneComunita.ObjectType.Community, ComunitaID))
			Me.PNLconferma.Visible = False
			Try
				Dim oUtility As New OLDpageUtility(Me.Context)
				oPersona = Session("objPersona")

                iResponse = oPersona.IscrizioneComunitaNew(Me.HDNcmnt_ID.Value, Me.HDN_Path.Value, Me.HDisChiusa.Value, Server.MapPath("./../profili/") & oPersona.ID & "\", "./../", Me.Request.Url.Host & GetPercorsoApplicazione(Me.Request) & "/", Session.SessionID, Session("LinguaCode"), oUtility.LocalizedMail, oUtility.SystemSettings.Login.DaysToUpdateProfile)
                lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.BaseModules.CommunityManagement.Domain.CacheKeys.UserCommunitiesTree(oPersona.ID))
				Me.ResetForm_toMessaggio()
				If iResponse <> Main.ErroriIscrizioneComunita.IscrizioneAvvenuta And iResponse <> Main.ErroriIscrizioneComunita.IscrizioneInAttesa Then
					Me.LBmessaggio.Text = Me.oResource.getValue("ErroriIscrizioneComunita." & CType(iResponse, Main.ErroriIscrizioneComunita))
                Else
                    Dim oServiceUtility As New SubscriptionNotificationUtility(Me.PageUtility)
                    If iResponse = Main.ErroriIscrizioneComunita.IscrizioneAvvenuta Then
                        oServiceUtility.NotifyAddSelfSubscription(Me.HDNcmnt_ID.Value, Me.PageUtility.CurrentUser.ID, Me.PageUtility.CurrentUser.Anagrafica)
                    Else
                        oServiceUtility.NotifyAddWaitingSubscription(Me.HDNcmnt_ID.Value, Me.PageUtility.CurrentUser.ID, Me.PageUtility.CurrentUser.Anagrafica)
                    End If
                    Me.LBmessaggio.Text = Me.oResource.getValue("ErroriIscrizioneComunita." & CType(iResponse, Main.ErroriIscrizioneComunita))
				End If
				Me.HDNcmnt_ID.Value = ""
				Me.HDisChiusa.Value = ""
				Me.HDN_Path.Value = ""
			Catch ex As Exception
				Me.ResetForm(True)
			End Try
		Else
			Me.ResetForm(True)
		End If
	End Sub

#Region "Pannello Dettagli"
    Private Sub LNBentra_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBentra.Click
        Me.Entra_Comunita(Me.HDN_Path.Value)
    End Sub
    Private Sub LNBiscrivi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBiscrivi.Click
        Dim oImpostazioni As New COL_ImpostazioniUtente
        Dim exitSub As Boolean = False
        Try
            oImpostazioni = Session("oImpostazioni")
            exitSub = Not oImpostazioni.ShowConferma
        Catch ex As Exception
            exitSub = False
        End Try

        If Session("azione") <> "iscrivi" Then
            Session("azione") = "iscrivi"
            If Not exitSub Then
                Dim oComunita As New COL_Comunita

                Me.ResetForm_toConferma()
                'Me.HDisChiusa.Value = Me.HDNisChiusa.Value
                Me.PNLconferma.Visible = True
                Me.PNLcontenuto.Visible = False

                oComunita.Id = Me.HDNcmnt_ID.Value
                Me.oResource.setLabel(Me.LBconferma)
                Me.LBconferma.Text = Replace(Me.LBconferma.Text, "#nomeComunita#", oComunita.EstraiNomeBylingua(Session("linguaID")))
                Me.LBconferma.Text = Replace(Me.LBconferma.Text, "#nomeResponsabile#", oComunita.GetNomeResponsabile_NomeCreatore)
            Else : Me.Iscrivi(Me.HDNcmnt_ID.Value, Me.HDN_Path.Value)
            End If
        Else : Me.ResetForm(False)
        End If
    End Sub
    Private Sub LNBannullaDettagli_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBannullaDettagli.Click
        Me.ResetForm(False)
    End Sub
#End Region

#Region "Reset_Form"
    Private Sub Reset_HideAll()
        Me.PNLconferma.Visible = False
        Me.PNLdettagli.Visible = False
        Me.PNLtreeView.Visible = False

        Me.PNLmenu.Visible = False
        Me.PNLmenuConferma.Visible = False
        Me.PNLmenuMessaggio.Visible = False
        Me.PNLmenuDettagli.Visible = False
    End Sub
    Private Sub _ResetForm_toDettagli()
        Me.PNLtreeView.Visible = False
        Me.PNLdettagli.Visible = True
        Me.PNLmessaggio.Visible = False
        Me.PNLconferma.Visible = False

        Me.PNLmenu.Visible = False
        Me.PNLmenuDettagli.Visible = True
        Me.PNLmenuMessaggio.Visible = False
        Me.PNLmenuConferma.Visible = False
    End Sub
    Private Sub ResetForm_toConferma()
        Me.PNLtreeView.Visible = False
        Me.PNLdettagli.Visible = False
        Me.PNLmessaggio.Visible = False
        Me.PNLconferma.Visible = True

        Me.PNLmenu.Visible = False
        Me.PNLmenuDettagli.Visible = False
        Me.PNLmenuMessaggio.Visible = False
        Me.PNLmenuConferma.Visible = True
    End Sub
    Private Sub ResetForm_toMessaggio()
        Me.PNLtreeView.Visible = False
        Me.PNLdettagli.Visible = False
        Me.PNLmessaggio.Visible = True
        Me.PNLconferma.Visible = False

        Me.PNLmenu.Visible = False
        Me.PNLmenuDettagli.Visible = False
        Me.PNLmenuMessaggio.Visible = True
        Me.PNLmenuConferma.Visible = False
    End Sub
    Private Sub ResetForm(ByVal updateTree As Boolean)
        Me.HDN_Path.Value = ""
        Me.Reset_HideAll()
        Me.PNLtreeView.Visible = True
        Me.PNLmenu.Visible = True
        Me.HDisChiusa.Value = ""
        Me.HDN_Path.Value = ""
        Me.HDNcmnt_ID.Value = ""
        'Me.HDNisChiusa.Value = ""
        Session("azione") = "loaded"
        If updateTree Then
            Me.Bind_TreeView(True)
        End If
    End Sub

    Private Sub Reset_Todettagli()
        Me.Reset_HideAll()
        Me.PNLdettagli.Visible = True
        Me.PNLmenuDettagli.Visible = True
    End Sub
#End Region

#Region "Internazionalizzazione"
    ' Inizializzazione oggetto risorse: SEMPRE DA FARE
    Private Sub SetCulture(ByVal Code As String)
        Me.oResource = New ResourceManager

        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_RicercaComunitaAlbero"
        oResource.Folder_Level1 = "Comunita"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            '.setLabel(Me.LBtitolo)
            Me.Master.ServiceTitle = .getValue("LBtitolo.text")
            .setLabel(Me.LBNopermessi)
            .setLinkButton(Me.LNBcomprimi, True, True)
            Me.LNBcomprimi.Attributes.Add("onclick", "window.status='';CollapseAll();return false;")

            .setLinkButton(Me.LNBespandi, True, True)
            Me.LNBespandi.Attributes.Add("onclick", "window.status='';ExpandAll();return false;")

            .setDropDownList(Me.DDLTipoRicerca, -2)
            .setDropDownList(Me.DDLTipoRicerca, -7)
            .setDropDownList(Me.DDLTipoRicerca, -5)
            .setDropDownList(Me.DDLTipoRicerca, -6)
            .setDropDownList(Me.DDLTipoRicerca, -3)
            .setDropDownList(Me.DDLTipoRicerca, -4)
            .setDropDownList(Me.DDLTipoRicerca, -9)

            .setLabel(Me.LBannoAccademico_c)
            .setLabel(Me.LBperiodo_c)
            .setLabel(Me.LBtipoComunita_c)
            .setLabel(Me.LBtipoRicerca_c)
            .setLabel(Me.LBvalore_c)
            .setLabel(Me.LBricercaByIscrizione_c)
            .setButton(Me.BTNCerca)
            .setRadioButtonList(Me.RBLricercaByIscrizione, 1)
            .setRadioButtonList(Me.RBLricercaByIscrizione, 0)
            .setLabel(Me.LBcorsoDiStudi_t)
            .setLabel(Me.LBlegenda)


            .setLinkButton(Me.LNBespandi, True, True)
            .setLinkButton(Me.LNBcomprimi, True, True)
            Me.LNBcomprimi.Attributes.Add("onclick", "window.status='';CollapseAll();return false;")
            Me.LNBespandi.Attributes.Add("onclick", "window.status='';ExpandAll();return false;")

            .setLinkButton(Me.LNBlista, True, True)
            .setLinkButton(Me.LNBalbero, True, True)
            .setLinkButton(Me.LNBalberoGerarchico, True, True)
            .setLinkButton(Me.LNBentra, True, True)
            .setLinkButton(Me.LNBiscrivi, True, True)
            .setLinkButton(Me.LNBannullaDettagli, True, True)
            .setLinkButton(Me.LNBannullaMessaggio, True, True)
            .setLabel(Me.LBstatoComunita_t)

            .setCheckBox(Me.CBXautoUpdate)
            .setRadioButtonList(Me.RBLstatoComunita, -1)
            .setRadioButtonList(Me.RBLstatoComunita, 0)
            .setRadioButtonList(Me.RBLstatoComunita, 1)
            .setRadioButtonList(Me.RBLstatoComunita, 2)

            .setLinkButton(Me.LNBapriFiltro, True, True)
            .setLinkButton(Me.LNBchiudiFiltro, True, True)
            .setLinkButton(Me.LNBiscriviConferma, True, True)
            .setLinkButton(Me.LNBannullaConferma, True, True)
        End With
    End Sub
#End Region

    Private Function CreateContextMenu(ByVal childNode As RadTreeNode, ByVal ForDetails As Boolean, ByVal ForUpdate As Boolean, Optional ByVal ForEntra As Boolean = False, Optional ByVal ForIscrivi As Boolean = False, Optional ByVal HasNews As Boolean = False)
        Dim contextMenus As New ArrayList
        Dim nodeMenu As New Telerik.WebControls.RadTreeViewContextMenu.ContextMenu

        Dim NomeContextMenu As String = "_"

        Dim iMenuDettagli As RadTreeViewContextMenu.ContextMenuItem
        Dim iMenuNews As RadTreeViewContextMenu.ContextMenuItem
        Dim iMenuEntra As RadTreeViewContextMenu.ContextMenuItem
        Dim iMenuAggiorna As RadTreeViewContextMenu.ContextMenuItem
        Dim iMenuIscrivi As RadTreeViewContextMenu.ContextMenuItem
        If ForDetails Then
            iMenuDettagli = New RadTreeViewContextMenu.ContextMenuItem
            iMenuDettagli.Image = "./images/12.gif"
            iMenuDettagli.PostBack = True
            iMenuDettagli.ID = 2
            iMenuDettagli.Text = oResource.getValue("menu." & Me.AzioneTree.Dettagli)
            NomeContextMenu = NomeContextMenu & "1_"
        End If
        If HasNews Then
            iMenuNews = New RadTreeViewContextMenu.ContextMenuItem
            iMenuNews.Image = "./images/0.gif"
            iMenuNews.PostBack = True
            iMenuNews.ID = 5
            iMenuNews.Text = oResource.getValue("menu." & Me.AzioneTree.Novità)
            NomeContextMenu = NomeContextMenu & "5_"
        End If
        If ForUpdate Then
            iMenuAggiorna = New RadTreeViewContextMenu.ContextMenuItem
            iMenuAggiorna.Image = "./images/14.gif"
            iMenuAggiorna.PostBack = True
            iMenuAggiorna.ID = 1
            iMenuAggiorna.Text = oResource.getValue("menu." & Me.AzioneTree.Aggiorna)
            NomeContextMenu = NomeContextMenu & "2_"
        End If
        If ForEntra Then
            iMenuEntra = New RadTreeViewContextMenu.ContextMenuItem
            iMenuEntra.Image = "./images/0.gif"
            iMenuEntra.PostBack = True
            iMenuEntra.ID = 3
            iMenuEntra.Text = oResource.getValue("menu." & Me.AzioneTree.Entra)
            NomeContextMenu = NomeContextMenu & "3_"
        End If
        If ForIscrivi Then
            iMenuIscrivi = New RadTreeViewContextMenu.ContextMenuItem
            iMenuIscrivi.Image = "./images/4.gif"
            iMenuIscrivi.PostBack = True
            iMenuIscrivi.ID = 4
            iMenuIscrivi.Text = oResource.getValue("menu." & Me.AzioneTree.Iscrivi)
            NomeContextMenu = NomeContextMenu & "4_"
        End If

        If Not IsNothing(iMenuEntra) Then
            nodeMenu.Items.Add(iMenuEntra)
        End If
        If Not IsNothing(iMenuIscrivi) Then
            nodeMenu.Items.Add(iMenuIscrivi)
        End If
        If Not IsNothing(iMenuAggiorna) Then
            nodeMenu.Items.Add(iMenuAggiorna)
        End If
        If Not IsNothing(iMenuIscrivi) Or Not IsNothing(iMenuEntra) Then
            If Not IsNothing(iMenuDettagli) Then
                Dim iMenuBlanck As New RadTreeViewContextMenu.ContextMenuItem
                iMenuBlanck.Text = ""
                iMenuBlanck.Image = ""
                iMenuBlanck.PostBack = False
                nodeMenu.Items.Add(iMenuBlanck)

                nodeMenu.Items.Add(iMenuDettagli)
            End If
        ElseIf Not IsNothing(iMenuDettagli) Then
            nodeMenu.Items.Add(iMenuDettagli)
        End If
        If Not IsNothing(iMenuNews) Then
            nodeMenu.Items.Add(iMenuNews)
        End If
        contextMenus = Me.RDTcomunita.ContextMenus
        If contextMenus.Count = 0 Then
            nodeMenu.Name = NomeContextMenu
            contextMenus.Add(nodeMenu)
        Else
            Dim i, totale As Integer
            Dim found As Boolean = False
            totale = contextMenus.Count - 1
            For i = 0 To contextMenus.Count - 1
                If contextMenus.Item(i).Name = NomeContextMenu Then
                    found = True
                    Exit For
                End If
            Next
            If Not found Then
                nodeMenu.Name = NomeContextMenu
                contextMenus.Add(nodeMenu)
            End If
        End If
        childNode.ContextMenuName = NomeContextMenu
        Me.RDTcomunita.ContextMenus = contextMenus
    End Function

    Private Sub LNBapriFiltro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBapriFiltro.Click
        Me.TBRfiltri.Visible = True
        Me.TBRchiudiFiltro.Visible = True
        Me.TBRapriFiltro.Visible = False
    End Sub
    Private Sub LNBchiudiFiltro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBchiudiFiltro.Click
        Me.TBRfiltri.Visible = False
        Me.TBRchiudiFiltro.Visible = False
        Me.TBRapriFiltro.Visible = True
    End Sub

    Private Sub LNBannullaMessaggio_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBannullaMessaggio.Click
        Me.ResetForm(True)
    End Sub


    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AjaxPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AjaxPortal)
        End Get
    End Property

End Class

'function SubmitRicerca(event){
'	 if (document.all){
'		if (event.keyCode == 13){
'			event.returnValue=false;
'			event.cancel = true;
'			try{
'				document.forms[0].BTNCerca.click();}
'			catch (ex){
'				return false;}
'			}
'		}
'	else if (document.getElementById){
'		if (event.which == 13){
'			event.returnValue=false;
'			event.cancel = true;
'			try{
'				document.forms[0].BTNCerca.click();}
'			catch(ex){
'				return false;}
'			}
'		}
'	else if(document.layers){
'		if(event.which == 13){
'			event.returnValue=false;
'			event.cancel = true;
'				try{
'				document.forms[0].BTNCerca.click();}
'			catch(ex){
'				return false;}
'			}
'		}
'	else
'		return true;
'}