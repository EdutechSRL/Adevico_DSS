Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.UCServices.Services_IscrizioneComunita


    Public Class IscrizioneComunita
        Inherits System.Web.UI.Page
	Private oResource As ResourceManager

	Private _PageUtility As PresentationLayer.OLDpageUtility
	Private ReadOnly Property PageUtility(Optional ByVal oContext As HttpContext = Nothing) As PresentationLayer.OLDpageUtility
		Get
			If IsNothing(_PageUtility) OrElse Not IsNothing(oContext) Then
				_PageUtility = New OLDpageUtility(HttpContext.Current)
			End If
			Return _PageUtility
		End Get
	End Property


        Private Enum Iscrizioni_code
            IscrizioniAperteIl = 0
            IscrizioniChiuse = 1
            IscrizioniComplete = 2
            IscrizioniEntro = 3
        End Enum

        Private Enum maxIscritti_code
            illimitati = 0
            limiteSuperato1 = 1
            limiteSuperato2 = 2
            postoLibero = 3
            postiLiberi = 4
            limiteRaggiunto = 5
        End Enum

        Private Enum StringaOrdinamento
            Crescente = 0
            Decrescente = 1
            Corrente = 2
        End Enum
        'Private Enum StringaAbilitato
        '    abilitato = 1
        '    bloccato = 0
        '    inAttesa = -1
        '    errore = 2
        '    noCommunity = 3
        'End Enum

        Private Enum stringaRegistrazione
            errore = 0
            inAttesa = 1
            limiteIscrizione = 2
            iscritto = 3
        End Enum
        Private Enum stringaTitolo
            altraFacoltà = 0
            sottoComunita = 1
            sottoComunitaDi = 2
            comunita = 3
        End Enum
        Private Enum stringaMessaggio
            ok = 0
            indietro = 1
        End Enum
        Private Enum StringaElenco
            noCommunityForFilter = 0
            noCommunity = 1
        End Enum

#Region "Filtri"
        Protected WithEvents TBLfiltroNew As System.Web.UI.WebControls.Table
        Protected WithEvents TBRchiudiFiltro As System.Web.UI.WebControls.TableRow
        Protected WithEvents LNBchiudiFiltro As System.Web.UI.WebControls.LinkButton
        Protected WithEvents TBRapriFiltro As System.Web.UI.WebControls.TableRow
        Protected WithEvents LNBapriFiltro As System.Web.UI.WebControls.LinkButton
        Protected WithEvents TBRfiltri As System.Web.UI.WebControls.TableRow
        Protected WithEvents TBLfiltro As System.Web.UI.WebControls.Table

        Protected WithEvents LBtipoComunita_c As System.Web.UI.WebControls.Label

        Protected WithEvents TBCtipoRicerca_c As System.Web.UI.WebControls.TableCell
        Protected WithEvents LBtipoRicerca_c As System.Web.UI.WebControls.Label

        Protected WithEvents LBvalore_c As System.Web.UI.WebControls.Label
        Protected WithEvents TBCvalore_c As System.Web.UI.WebControls.TableCell
        Protected WithEvents LBvuota_c As System.Web.UI.WebControls.Label

        Protected WithEvents TBCannoAccademico_info As System.Web.UI.WebControls.TableCell
        Protected WithEvents TBCannoAccademico As System.Web.UI.WebControls.TableCell


    Protected WithEvents TBRorgnCorsi As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBannoAccademico_c As System.Web.UI.WebControls.Label
    Protected WithEvents LBperiodo_c As System.Web.UI.WebControls.Label
    Protected WithEvents DDLperiodo As System.Web.UI.WebControls.DropDownList
    Protected WithEvents TBLcorsiDiStudio As System.Web.UI.WebControls.Table
    Protected WithEvents LBtipoCorsoDiStudi_t As System.Web.UI.WebControls.Label
    Protected WithEvents DDLtipoCorsoDiStudi As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LBnoCorsi As System.Web.UI.WebControls.Label
    Protected WithEvents DDLTipo As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLTipoRicerca As System.Web.UI.WebControls.DropDownList
    Protected WithEvents TXBValore As System.Web.UI.WebControls.TextBox
    Protected WithEvents DDLresponsabile As System.Web.UI.WebControls.DropDownList
        Protected WithEvents BTNCerca As System.Web.UI.WebControls.Button

    Protected WithEvents TBLcorsi As System.Web.UI.WebControls.Table
    Protected WithEvents CBXautoUpdate As System.Web.UI.WebControls.CheckBox
    Protected WithEvents DDLannoAccademico As System.Web.UI.WebControls.DropDownList
    Protected WithEvents CBXmostraPadre As System.Web.UI.WebControls.CheckBox

#Region "Lettere"
    Protected WithEvents LKBtutti As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBaltro As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBa As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBb As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBc As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBd As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBe As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBf As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBg As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBh As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBi As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBj As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBk As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBl As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBm As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBn As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBo As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBp As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBq As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBr As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBs As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBt As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBu As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBv As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBw As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBx As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBy As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBz As System.Web.UI.WebControls.LinkButton
#End Region

    Protected WithEvents HDNselezionato As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNcomunitaSelezionate As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroFacolta As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroTipoRicerca As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroValore As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroResponsabileID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroLaureaID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroTipoCdl As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroAnno As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroPeriodo As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroTipoComunitaID As System.Web.UI.HtmlControls.HtmlInputHidden
#End Region

#Region "Griglia"
    Protected WithEvents LBnumeroRecord_1 As System.Web.UI.WebControls.Label
    Protected WithEvents DDLNumeroRecord As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DGComunita As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LBmsgDG As System.Web.UI.WebControls.Label
#End Region

    Protected WithEvents PNLmenuIscritto As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBelencoIscritte As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBiscriviAltre As System.Web.UI.WebControls.LinkButton

    Protected WithEvents LBconfermaMultipla As System.Web.UI.WebControls.Label
    Protected WithEvents LBMessaggi As System.Web.UI.WebControls.Label
    Protected WithEvents PNLmessaggi As System.Web.UI.WebControls.Panel
    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel

    'Protected WithEvents LBtitolo As System.Web.UI.WebControls.Label
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel

    Protected WithEvents PNLmenuDefault As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBiscriviMultipli As System.Web.UI.WebControls.LinkButton

    Protected WithEvents PNLmenuAccesso As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBannulla As System.Web.UI.WebControls.LinkButton

    Protected WithEvents PNLiscrizioneAvvenuta As System.Web.UI.WebControls.Panel
    Protected WithEvents LBiscrizione As System.Web.UI.WebControls.Label


#Region "conferma"
    Protected WithEvents PNLmenuConferma As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBannullaConferma As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBiscriviConferma As System.Web.UI.WebControls.LinkButton

    Protected WithEvents PNLconferma As System.Web.UI.WebControls.Panel
    Protected WithEvents LBconferma As System.Web.UI.WebControls.Label
#End Region

#Region "Form Dettagli"
    Protected WithEvents PNLmenuDettagli As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBannullaDettagli As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBiscriviDettagli As System.Web.UI.WebControls.LinkButton

    Protected WithEvents LBlegend As System.Web.UI.WebControls.Label
    Protected WithEvents PNLdettagli As System.Web.UI.WebControls.Panel
    Protected WithEvents TBLdettagli As System.Web.UI.WebControls.Table

    Protected WithEvents CTRLDettagli As Comunita_OnLine.UC_DettagliComunita

    Protected WithEvents HDNcmnt_ID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNcmnt_Path As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNisChiusaForPadre As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNisChiusa As System.Web.UI.HtmlControls.HtmlInputHidden
#End Region

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

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

        Page.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Expires = 0
        Response.Cache.SetNoStore()
        Response.AppendHeader("Pragma", "no-cache")

        If Not IsPostBack Then

            Me.SetupInternazionalizzazione()
            Dim oServizioIscrizione As New Services_IscrizioneComunita
            oServizioIscrizione = Me.ImpostaPermessi

            Session("azione") = "load"
            If oServizioIscrizione.List Or oServizioIscrizione.Admin Then
                Me.ViewState("intCurPage") = 0
                Me.ViewState("intAnagrafica") = CType(Main.FiltroComunita.tutti, Main.FiltroComunita)
                Me.LKBtutti.CssClass = "lettera_Selezionata"
                Me.TBRapriFiltro.Visible = False
                Me.TBRchiudiFiltro.Visible = True
                Me.TBRfiltri.Visible = True


                Me.ViewState("SortExspression") = "CMNT_Nome"
                Me.ViewState("SortDirection") = "asc"
                Me.PNLmenuDefault.Visible = True
                Me.PNLcontenuto.Visible = True
                Me.PNLpermessi.Visible = False
                Me.Bind_Dati()
            Else
                Me.PNLmenuDefault.Visible = False
                Me.PNLcontenuto.Visible = False
                Me.PNLpermessi.Visible = True
            End If

            ' REGISTRAZIONE EVENTO
            Try
				Dim stringaMessaggio As String = ""
				If Me.PNLpermessi.Visible = True Then
					'errore accesso !
					stringaMessaggio = "Errore_Permessi"
				ElseIf Session("Limbo") = True Then
					stringaMessaggio = "Comunità_Base"
				Else
					stringaMessaggio = "Comunità"
				End If
				PageUtility.AddAction(IIf(Me.PNLpermessi.Visible, ActionType.NoPermission, ActionType.CommunityList))
			Catch ex As Exception

			End Try
        End If

        Me.Page.Form.DefaultButton = Me.BTNCerca.UniqueID
        Me.Page.Form.DefaultFocus = Me.TXBValore.UniqueID 'Me.TXBvalue.UniqueID
        Me.Master.Page.Form.DefaultButton = Me.BTNCerca.UniqueID
        Me.Master.Page.Form.DefaultFocus = Me.TXBValore.UniqueID 'Me.TXBvalue.UniqueID

    End Sub

    Private Function ImpostaPermessi() As Services_IscrizioneComunita
        Dim ComunitaID As Integer = 0
        Dim iResponse As String = "00000000000000000000000000000000"
        Dim oServizioIscrizione As New Services_IscrizioneComunita
        Dim oPersona As New COL_Persona

        Try
            oPersona = Session("objPersona")
            ComunitaID = Session("idComunita")
        Catch ex As Exception
            ComunitaID = 0
        End Try
        Try
            If ComunitaID = 0 Then
                Session("Limbo") = True
                If oPersona.TipoPersona.id = Main.TipoPersonaStandard.Guest Then
                    iResponse = "00000000000000000000000000000000"
                Else
                    oServizioIscrizione.Admin = False
                    oServizioIscrizione.List = True
                    Return oServizioIscrizione
                End If
            Else
                iResponse = Permessi(Services_IscrizioneComunita.Codex, Me.Page)
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

    Private Function SessioneScaduta() As Boolean
        Dim oPersona As COL_Persona
        Dim isScaduta As Boolean = True
        Try
            oPersona = Session("objPersona")
            If oPersona.Id > 0 Then
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
            Dim PageUtility As New OLDpageUtility(Me.Context)
            Dim UrlRedirect As String = PageUtility.GetDefaultLogoutPage ' Me.DefaultUrl
            Response.Write("<script language='javascript'>function AlertLogout(Messaggio,pagina){" & vbCrLf & "alert(Messaggio);" & vbCrLf & "document.location.replace(pagina);" & vbCrLf & "} " & vbCrLf & "AlertLogout('" & alertMSG & "','" & UrlRedirect & "');</script>")
            Return True
        Else
            Return True
        End If
    End Function
    Private Function GetPermessiForPage(ByVal Codex As String) As String
        Dim oPersona As New COL_Persona
        Dim oRuoloComunita As New COL_RuoloPersonaComunita
        Dim CMNT_id As Integer

        Dim PermessiAssociati As String

        Try
            oPersona = Session("objPersona")
            PermessiAssociati = Permessi(Codex, Me.Page)

            If (PermessiAssociati = "") Then
                PermessiAssociati = "00000000000000000000000000000000"
            End If
        Catch ex As Exception
            PermessiAssociati = "00000000000000000000000000000000"
        End Try

        If Session("AdminForChange") = False Then
            Try
                CMNT_id = Session("IdComunita")
                PermessiAssociati = Permessi(Codex, Me.Page)
                If (PermessiAssociati = "") Then
                    PermessiAssociati = "00000000000000000000000000000000"
                End If
            Catch ex As Exception
                PermessiAssociati = "00000000000000000000000000000000"
            End Try

            Try
                oRuoloComunita.EstraiByLinguaDefault(CMNT_id, oPersona.Id)
                Me.ViewState("PRSN_TPRL_Gerarchia") = oRuoloComunita.TipoRuolo.Gerarchia

            Catch ex As Exception
                Me.ViewState("PRSN_TPRL_Gerarchia") = "99999"
            End Try
        Else
            Dim oComunita As New COL_Comunita
            oComunita.Id = Session("idComunita_forAdmin")

            'Vengo dalla pagina di amministrazione generale
            Try
                PermessiAssociati = oComunita.GetPermessiForServizioByCode(Main.TipoRuoloStandard.AdminComunità, Codex)
                If (PermessiAssociati = "") Then
                    PermessiAssociati = "00000000000000000000000000000000"
                End If
            Catch ex As Exception
                PermessiAssociati = "00000000000000000000000000000000"

            End Try
        End If
        Return PermessiAssociati
    End Function

#Region "Bind_Dati"

    Private Sub Bind_Dati()
        Dim oComunita As New COL_Comunita

        Try
            If Session("Limbo") = True And Me.Request.QueryString("show") = 0 Then
                ' trattasi di mostrare l'elenco delle organizzazioni
                'oResource.setLabel_To_Value(Me.LBtitolo, "LBtitolo." & stringaTitolo.altraFacoltà)
                Me.Master.ServiceTitle = ("LBtitolo." & stringaTitolo.altraFacoltà)
                Me.CBXmostraPadre.Visible = False
            ElseIf Session("Limbo") Then
                'oResource.setLabel_To_Value(Me.LBtitolo, "LBtitolo." & stringaTitolo.sottoComunita)
                Me.Master.ServiceTitle = ("LBtitolo." & stringaTitolo.sottoComunita)
                Me.CBXmostraPadre.Visible = True
            Else
                Dim ArrComunita(,) As String
                ArrComunita = Nothing
                Try

                    If IsArray(Session("ArrComunita")) And Session("limbo") = False Then
                        ArrComunita = Session("ArrComunita")
                        'Me.LBtitolo.Text = "- Iscrizione alle sotto-comunità di : " & Left(ArrComunita(1, ArrComunita.Length - 1), 30) & " -"
                        'oResource.setLabel_To_Value(Me.LBtitolo, "LBtitolo." & stringaTitolo.sottoComunitaDi)
                        'Me.LBtitolo.Text = Me.LBtitolo.Text.Replace("#%%#", Left(oComunita.Nome, 30))
                        Me.Master.ServiceTitle = oResource.getValue("LBtitolo." & stringaTitolo.sottoComunitaDi).Replace("#%%#", Left(oComunita.Nome, 30))
                    Else
                        'oResource.setLabel_To_Value(Me.LBtitolo, "LBtitolo." & stringaTitolo.sottoComunita)
                        Me.Master.ServiceTitle = oResource.getValue("LBtitolo." & stringaTitolo.sottoComunita)
                    End If
                Catch ex As Exception
                    Try
                        oComunita.Id = CInt(ArrComunita(0, 0))
                        oComunita.Estrai()
                        'oResource.setLabel_To_Value(Me.LBtitolo, "LBtitolo." & stringaTitolo.sottoComunitaDi)
                        'Me.LBtitolo.Text = Me.LBtitolo.Text.Replace("#%%#", Left(oComunita.Nome, 30))
                        Me.Master.ServiceTitle = oResource.getValue("LBtitolo." & stringaTitolo.sottoComunitaDi).Replace("#%%#", Left(oComunita.Nome, 30))
                    Catch exc As Exception
                        'oResource.setLabel_To_Value(Me.LBtitolo, "LBtitolo." & stringaTitolo.sottoComunita)
                        Me.Master.ServiceTitle = oResource.getValue("LBtitolo." & stringaTitolo.sottoComunita)
                    End Try
                End Try
            End If
        Catch ex As Exception
            'oResource.setLabel_To_Value(Me.LBtitolo, "LBtitolo." & stringaTitolo.comunita)
            Me.Master.ServiceTitle = oResource.getValue("LBtitolo." & stringaTitolo.comunita)
        End Try

        Me.SetupFiltri()
        Me.Bind_Griglia(True)
    End Sub

    Private Sub SetupFiltri()
        Dim oComunita As New COL_Comunita
        Me.Bind_TipiComunita()

        If Session("limbo") = True And Me.Request.QueryString("show") = 0 Then
            'altra facoltà
            Try
                Me.DDLTipo.SelectedValue = Main.TipoComunitaStandard.Organizzazione
            Catch ex As Exception

            End Try
            Me.DDLTipo.Enabled = False
            Me.TBRorgnCorsi.Visible = False
        Else
            Me.DDLTipo.Enabled = True
            Me.TBRorgnCorsi.Visible = False
        End If
     
      
        Dim oImpostazioni As New COL_ImpostazioniUtente
        Dim FacoltaID As Integer = -1
        Try

            Dim ArrComunita(,) As String
            oImpostazioni = Session("oImpostazioni")

            If Session("Limbo") = False And Session("idComunita") > 0 Then
                If IsArray(Session("ArrComunita")) Then
                    Try
                        Dim oComunitaOrganizzazione As New COL_Comunita
                        ArrComunita = Session("ArrComunita")
                        oComunitaOrganizzazione.Id = ArrComunita(0, 0)

                        FacoltaID = oComunitaOrganizzazione.GetOrganizzazioneID
                    Catch ex As Exception
                        FacoltaID = -1
                    End Try
                End If

                Try
                    Me.DDLTipo.SelectedValue = oImpostazioni.TipoComunita_Ricerca
                Catch ex As Exception

                End Try
            End If

    
            Try
                Me.ChangeNumeroRecord(oImpostazioni.Organizzazione_Ricerca)
            Catch ex As Exception

            End Try
        Catch ex As Exception

        End Try
        Me.Bind_Responsabili()
        Me.DDLTipoRicerca.Attributes.Add("onchange", "return AggiornaForm();")
        Me.HDNselezionato.Value = Me.DDLTipoRicerca.SelectedValue

        Me.DDLperiodo.AutoPostBack = Me.CBXautoUpdate.Checked
        Me.DDLannoAccademico.AutoPostBack = Me.CBXautoUpdate.Checked
        Me.DDLtipoCorsoDiStudi.AutoPostBack = Me.CBXautoUpdate.Checked
        Me.DDLresponsabile.AutoPostBack = Me.CBXautoUpdate.Checked

        Try
            Me.HDN_filtroFacolta.Value = FacoltaID
            Me.HDN_filtroTipoComunitaID.Value = Me.DDLTipo.SelectedValue
            Me.HDN_filtroTipoRicerca.Value = Me.DDLTipoRicerca.SelectedValue
            Me.HDN_filtroValore.Value = Me.TXBValore.Text
            Me.HDN_filtroResponsabileID.Value = Me.DDLresponsabile.SelectedValue
            Me.HDN_filtroTipoCdl.Value = Me.DDLtipoCorsoDiStudi.SelectedValue
            Me.HDN_filtroAnno.Value = Me.DDLannoAccademico.SelectedValue
            Me.HDN_filtroPeriodo.Value = Me.DDLperiodo.SelectedValue
            Me.HDNselezionato.Value = Me.HDN_filtroTipoRicerca.Value
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ChangeNumeroRecord(ByVal num As Integer)
        Try
            Me.DDLNumeroRecord.SelectedValue = num

        Catch ex As Exception
            Dim i, totale As Integer
            totale = Me.DDLNumeroRecord.Items.Count - 1
            For i = 0 To totale
                If Me.DDLNumeroRecord.Items(0).Value <= num Then
                    Me.DDLNumeroRecord.SelectedIndex = -1
                    Me.DDLNumeroRecord.Items(0).Selected = True
                Else
                    Exit For
                End If
            Next
        End Try
    End Sub
    Private Sub ChangeTipoComunita()
        Dim showFiltroCorso As Boolean = False

        'If Session("limbo") = True Then
        showFiltroCorso = True
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
        Me.TBLcorsiDiStudio.Visible = False
        Me.LBnoCorsi.Visible = False

       
            Me.LBnoCorsi.Visible = True
            Me.DDLtipoCorsoDiStudi.SelectedIndex = 0
            Me.DDLannoAccademico.SelectedIndex = 0
            Me.DDLperiodo.SelectedIndex = 0
            Me.TBRorgnCorsi.Visible = False


        If Me.CBXautoUpdate.Checked Then
            Try
                Me.HDN_filtroTipoComunitaID.Value = Me.DDLTipo.SelectedValue
                Me.HDN_filtroTipoRicerca.Value = Me.DDLTipoRicerca.SelectedValue
                Me.HDN_filtroValore.Value = Me.TXBValore.Text
                Me.HDN_filtroResponsabileID.Value = Me.DDLresponsabile.SelectedValue
                Me.HDN_filtroTipoCdl.Value = Me.DDLtipoCorsoDiStudi.SelectedValue
                Me.HDN_filtroAnno.Value = Me.DDLannoAccademico.SelectedValue
                Me.HDN_filtroPeriodo.Value = Me.DDLperiodo.SelectedValue
            Catch ex As Exception

            End Try
        End If
    End Sub
    Private Sub Bind_TipiComunita()
        Dim oDataSet As New DataSet
        Dim oTipoComunita As New COL_Tipo_Comunita

        Try
            oDataSet = COL_Tipo_Comunita.ElencaForFiltri(Session("LinguaID"), True)
            If oDataSet.Tables(0).Rows.Count > 0 Then
                DDLTipo.DataSource = oDataSet
                DDLTipo.DataTextField() = "TPCM_descrizione"
                DDLTipo.DataValueField() = "TPCM_id"
                DDLTipo.DataBind()

                If Me.DDLTipo.Items.Count > 1 Then
                    DDLTipo.Items.Insert(0, New ListItem("-- Tutti --", -1))
                End If
            End If
        Catch ex As Exception
            DDLTipo.Items.Insert(0, New ListItem("-- Tutti --", -1))
        End Try
        oResource.setDropDownList(Me.DDLTipo, -1)
    End Sub

    Private Sub Bind_Responsabili(Optional ByVal DocenteID As Integer = -1)
        Dim oDataSet As New DataSet
        Dim FacoltaID As Integer = -1
        Dim ComunitaID As Integer = -1
        Try
            Me.DDLresponsabile.Items.Clear()

            If IsArray(Session("ArrComunita")) Then
                Dim ArrComunita(,) As String
                Try
                    Dim oComunitaOrganizzazione As New COL_Comunita
                    ArrComunita = Session("ArrComunita")
                    oComunitaOrganizzazione.Id = ArrComunita(0, 0)

                    FacoltaID = oComunitaOrganizzazione.GetOrganizzazioneID
                Catch ex As Exception

                End Try
            End If

            Dim TipoComuniaID As Integer = -1
            Dim TipoCdlID As Integer = -1
            Dim AnnoAcc As Integer = -1
            Dim PeriodoID As Integer = -1
            Try
                If Session("IdComunita") > 0 Then
                    ComunitaID = Session("IdComunita")
                End If
            Catch ex As Exception

            End Try
            Try
                If Me.CBXautoUpdate.Checked Then
                    TipoComuniaID = Me.DDLTipo.SelectedValue
                Else
                    TipoComuniaID = Me.HDN_filtroTipoComunitaID.Value
                End If
            Catch ex As Exception

            End Try
            'Try
            '    If Me.CBXautoUpdate.Checked Then
            '        TipoCdlID = Me.DDLtipoCorsoDiStudi.SelectedValue
            '    Else
            '        TipoCdlID = Me.HDN_filtroTipoCdl.Value
            '    End If
            'Catch ex As Exception

            'End Try
            'Try
            '    If Me.CBXautoUpdate.Checked Then
            '        AnnoAcc = Me.DDLannoAccademico.SelectedValue
            '    Else
            '        AnnoAcc = -1Me.HDN_filtroAnno.Value
            '    End If
            'Catch ex As Exception

            'End Try
            'Try
            '    If Me.CBXautoUpdate.Checked Then
            '        PeriodoID = Me.DDLperiodo.SelectedValue
            '    Else
            '        PeriodoID = Me.HDN_filtroPeriodo.Value
            '    End If
            'Catch ex As Exception

            'End Try

        
                oDataSet = COL_Comunita.RicercaResponsabili(FacoltaID, ComunitaID, Session("objPersona").id, TipoComuniaID, , , , , Main.FiltroStatoComunita.Attiva)
          

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
    Private Function FiltraggioDati(Optional ByVal ApplicaFiltri As Boolean = False) As DataSet
        '  Dim oTreeComunita As New COL_TreeComunita
        Dim oPersona As New COL_Persona

        Dim i, totale As Integer
        Dim oDataset As New DataSet

        Try
            Dim valore As String = ""
            Dim oFiltroTipoRicerca As Main.FiltroComunita = Main.FiltroComunita.tutti
            Dim oFiltroLettera As Main.FiltroComunita = Main.FiltroComunita.tutti

            oPersona = Session("objPersona")
            Try
                If IsNumeric(Me.ViewState("intAnagrafica")) Then
                    oFiltroLettera = CType(Me.ViewState("intAnagrafica"), Main.FiltroComunita)
                Else
                    oFiltroLettera = Main.FiltroComunita.tutti
                    Me.SelezionaLink_All()
                End If
            Catch ex As Exception
                oFiltroLettera = Main.FiltroComunita.tutti
                Me.SelezionaLink_All()
            End Try

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
            Try
                ComunitaPadreID = Session("idComunita")
                If ComunitaPadreID < 1 Then
                    ComunitaPadreID = -1
                End If
            Catch ex As Exception
                ComunitaPadreID = -1
            End Try

            Dim FacoltaID, LaureaID, PeriodoID, AAid, TipocomunitaID, TipoCdlID As Integer
            If Me.CBXautoUpdate.Checked Or ApplicaFiltri = True Then
                Me.HDN_filtroTipoRicerca.Value = Me.DDLTipoRicerca.SelectedValue
                Me.HDN_filtroValore.Value = Me.TXBValore.Text
                Me.HDN_filtroResponsabileID.Value = Me.DDLresponsabile.SelectedValue
                Me.HDN_filtroTipoCdl.Value = Me.DDLtipoCorsoDiStudi.SelectedValue
                Me.HDN_filtroAnno.Value = Me.DDLannoAccademico.SelectedValue
                Me.HDN_filtroPeriodo.Value = Me.DDLperiodo.SelectedValue
                Me.HDN_filtroTipoComunitaID.Value = Me.DDLTipo.SelectedValue
            End If
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


            Dim oComunita As New COL_Comunita
            If Session("limbo") = True And Me.Request.QueryString("show") = 0 Then
                'se vengo dal limbo allora mostro tutte le comunità a cui appartengo, ordinate per data ultimo accesso
                oDataset = COL_Comunita.RicercaComunitaOrganizzazioni(Main.FiltroRicercaComunitaByIscrizione.nonIscritto, Session("LinguaID"), oPersona.Id, oFiltroTipoRicerca, oFiltroLettera, valore, Main.FiltroStatoComunita.Attiva)
            Else
                oDataset = COL_Comunita.RicercaComunita(Main.FiltroRicercaComunitaByIscrizione.nonIscritto, Session("LinguaID"), FacoltaID, ComunitaPadreID, oPersona.ID, oFiltroTipoRicerca, oFiltroLettera, valore, TipocomunitaID, , , , , Main.FiltroStatoComunita.Attiva)
            End If

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


            Dim oDataview As New DataView
            Dim ElencoComunitaID As String = ","

            oDataview = oDataset.Tables(0).DefaultView
            oDataview.AllowDelete = True
            totale = oDataset.Tables(0).Rows.Count

            If Not (Session("limbo") = True And Me.Request.QueryString("show") = 0) Then
                While oDataview.Count > 0
                    Dim ComunitaID As Integer


                    If ElencoComunitaID = "," Then
                        ComunitaID = oDataview.Item(0).Row.Item("CMNT_id")
                        oDataview.RowFilter = "CMNT_ID=" & ComunitaID
                    Else
                        oDataview.RowFilter = "'" & ElencoComunitaID & "' not like '%,' + CMNT_ID + ',%'"

                        If oDataview.Count > 0 Then
                            ComunitaID = oDataview.Item(0).Row.Item("CMNT_id")
                            oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
                        End If
                    End If

                    If oDataview.Count = 1 Then
                        oDataview.RowFilter = ""
                        ElencoComunitaID = ElencoComunitaID & ComunitaID & ","
                    ElseIf oDataview.Count > 1 Then
                        oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=1" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
                        If oDataview.Count = 1 Then
                            oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=0" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'" '' '%," & ComunitaID & ",%' not in ('" & ElencoComunitaID & "')"
                            While oDataview.Count > 0
                                oDataview.Delete(0)
                            End While
                        ElseIf oDataview.Count = 0 Then
                            oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=0" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'" '%," & ComunitaID & ",%' not in ('" & ElencoComunitaID & "')"
                            While oDataview.Count > 1
                                oDataview.Delete(1)
                            End While
                        ElseIf oDataview.Count > 1 Then
                            oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=1" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
                            While oDataview.Count > 1
                                oDataview.Delete(1)
                            End While
                            oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=0" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
                            While oDataview.Count > 0
                                oDataview.Delete(0)
                            End While
                        End If

                        oDataview.RowFilter = ""
                        ElencoComunitaID = ElencoComunitaID & ComunitaID & ","
                    End If

                End While
                oDataview.RowFilter = ""
                oDataset.AcceptChanges()
            End If

            Dim ImageBaseDir, img As String
            ImageBaseDir = GetPercorsoApplicazione(Me.Request)
            ImageBaseDir = ImageBaseDir & "/RadControls/TreeView/Skins/Comunita/logo/"
            ImageBaseDir = Replace(ImageBaseDir, "//", "/")

            totale = oDataset.Tables(0).Rows.Count
            Me.DGComunita.Columns(6).Visible = False

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
                        oRow.Item("AnagraficaResponsabile") = oRow.Item("CMNT_Responsabile")
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
                    img = ImageBaseDir & Mid(img, InStrRev(img, "/", img.Length - 1) + 1, img.Length)
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
                If IsDBNull(oRow.Item("PRDO_descrizione")) Then
                    oRow.Item("Periodo") = "&nbsp;"
                Else
                    oRow.Item("Periodo") = oRow.Item("PRDO_descrizione")
                End If
                If Me.CBXmostraPadre.Checked Then
                    If IsDBNull(oRow.Item("CMNT_NomePadre")) Then
                        oRow.Item("CMNT_Esteso") = oRow.Item("CMNT_Nome")
                        oRow.Item("CMNT_EstesoNoSpan") = oRow.Item("CMNT_Nome")
                    Else
                        If oRow.Item("CMNT_NomePadre") = "" Then
                            oRow.Item("CMNT_Esteso") = oRow.Item("CMNT_Nome")
                            oRow.Item("CMNT_EstesoNoSpan") = oRow.Item("CMNT_Nome")
                        Else
                            oRow.Item("CMNT_Esteso") = "<span class=small_Padre>" & oRow.Item("CMNT_nomePadre") & "</span>&gt;&nbsp;" & oRow.Item("CMNT_Nome")
                            oRow.Item("CMNT_EstesoNoSpan") = oRow.Item("CMNT_NomePadre") & "&gt;&nbsp;" & oRow.Item("CMNT_Nome")
                        End If
                    End If
                Else
                    oRow.Item("CMNT_Esteso") = oRow.Item("CMNT_Nome")
                    oRow.Item("CMNT_EstesoNoSpan") = oRow.Item("CMNT_Nome")
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

                        Me.DGComunita.Columns(6).Visible = True
                    End If
                Catch ex As Exception

                End Try
            Next
        Catch ex As Exception

        End Try
        Return oDataset
    End Function
    Private Function FiltraggioDatiRistretto() As DataSet
        Dim oPersona As New COL_Persona

        Dim totale As Integer
        Dim oDataset As New DataSet


        Try

            Dim valore As String = ""
            Dim oFiltroTipoRicerca As Main.FiltroComunita = Main.FiltroComunita.tutti
            Dim oFiltroLettera As Main.FiltroComunita = Main.FiltroComunita.tutti

            oPersona = Session("objPersona")
            Try
                If IsNumeric(Me.ViewState("intAnagrafica")) Then
                    oFiltroLettera = CType(Me.ViewState("intAnagrafica"), Main.FiltroComunita)
                Else
                    oFiltroLettera = Main.FiltroComunita.tutti
                    Me.SelezionaLink_All()
                End If
            Catch ex As Exception
                oFiltroLettera = Main.FiltroComunita.tutti
                Me.SelezionaLink_All()
            End Try

            If Me.CBXautoUpdate.Checked Then
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
            If Me.CBXautoUpdate.Checked Then
                TipoRicercaID = Me.DDLTipoRicerca.SelectedValue
            Else
                Try
                    TipoRicercaID = Me.HDN_filtroTipoRicerca.Value
                Catch ex As Exception
                    TipoRicercaID = -1
                End Try
            End If
            If valore <> "" Or (Me.CBXautoUpdate.Checked And Me.DDLresponsabile.Visible) Or (Not Me.CBXautoUpdate.Checked And Me.HDN_filtroTipoRicerca.Value = Main.FiltroComunita.IDresponsabile) Then
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
            If (Me.CBXautoUpdate.Checked) And valore = "" Then
                Me.TXBValore.Text = valore
                Me.HDN_filtroValore.Value = ""
            End If

            Dim ComunitaPadreID As Integer
            Try
                ComunitaPadreID = Session("idComunita")
                If ComunitaPadreID < 1 Then
                    ComunitaPadreID = -1
                End If
            Catch ex As Exception
                ComunitaPadreID = -1
            End Try

            Dim FacoltaID, LaureaID, PeriodoID, AAid, TipocomunitaID, TipoCdlID As Integer
            If Me.CBXautoUpdate.Checked Then
                Me.HDN_filtroTipoRicerca.Value = Me.DDLTipoRicerca.SelectedValue
                Me.HDN_filtroValore.Value = Me.TXBValore.Text
                Me.HDN_filtroResponsabileID.Value = Me.DDLresponsabile.SelectedValue
                Me.HDN_filtroTipoCdl.Value = Me.DDLtipoCorsoDiStudi.SelectedValue
                Me.HDN_filtroAnno.Value = Me.DDLannoAccademico.SelectedValue
                Me.HDN_filtroPeriodo.Value = Me.DDLperiodo.SelectedValue
                Me.HDN_filtroTipoComunitaID.Value = Me.DDLTipo.SelectedValue
            End If
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

			If valore <> "" Then
				valore = Replace(valore, "'", "''")
			End If
            Dim oComunita As New COL_Comunita
            If Session("limbo") = True And Me.Request.QueryString("show") = 0 Then
                'se vengo dal limbo allora mostro tutte le comunità a cui appartengo, ordinate per data ultimo accesso
                oDataset = COL_Comunita.RicercaComunitaOrganizzazioni(Main.FiltroRicercaComunitaByIscrizione.nonIscritto, Session("LinguaID"), oPersona.Id, oFiltroTipoRicerca, oFiltroLettera, valore, Main.FiltroStatoComunita.Attiva)
            Else
               
                    oDataset = COL_Comunita.RicercaComunita(Main.FiltroRicercaComunitaByIscrizione.nonIscritto, Session("LinguaID"), FacoltaID, ComunitaPadreID, oPersona.Id, oFiltroTipoRicerca, oFiltroLettera, valore, TipocomunitaID, , , , , Main.FiltroStatoComunita.Attiva)

            End If

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


            Dim oDataview As New DataView
            Dim ElencoComunitaID As String = ","

            oDataview = oDataset.Tables(0).DefaultView
            oDataview.AllowDelete = True
            totale = oDataset.Tables(0).Rows.Count

            If Not (Session("limbo") = True And Me.Request.QueryString("show") = 0) Then
                While oDataview.Count > 0
                    Dim ComunitaID As Integer


                    If ElencoComunitaID = "," Then
                        ComunitaID = oDataview.Item(0).Row.Item("CMNT_id")
                        oDataview.RowFilter = "CMNT_ID=" & ComunitaID
                    Else
                        oDataview.RowFilter = "'" & ElencoComunitaID & "' not like '%,' + CMNT_ID + ',%'"

                        If oDataview.Count > 0 Then
                            ComunitaID = oDataview.Item(0).Row.Item("CMNT_id")
                            oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
                        End If
                    End If

                    If oDataview.Count = 1 Then
                        oDataview.RowFilter = ""
                        ElencoComunitaID = ElencoComunitaID & ComunitaID & ","
                    ElseIf oDataview.Count > 1 Then
                        oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=1" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
                        If oDataview.Count = 1 Then
                            oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=0" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'" '' '%," & ComunitaID & ",%' not in ('" & ElencoComunitaID & "')"
                            While oDataview.Count > 0
                                oDataview.Delete(0)
                            End While
                        ElseIf oDataview.Count = 0 Then
                            oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=0" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'" '%," & ComunitaID & ",%' not in ('" & ElencoComunitaID & "')"
                            While oDataview.Count > 1
                                oDataview.Delete(1)
                            End While
                        ElseIf oDataview.Count > 1 Then
                            oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=1" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
                            While oDataview.Count > 1
                                oDataview.Delete(1)
                            End While
                            oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=0" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
                            While oDataview.Count > 0
                                oDataview.Delete(0)
                            End While
                        End If

                        oDataview.RowFilter = ""
                        ElencoComunitaID = ElencoComunitaID & ComunitaID & ","
                    End If

                End While
                oDataview.RowFilter = ""
                oDataset.AcceptChanges()
            End If
        Catch ex As Exception

        End Try
        Return oDataset
    End Function
    Private Sub Bind_Griglia(Optional ByVal ApplicaFiltri As Boolean = False)
        Dim oDataset As DataSet
        Dim totale As Integer

        Try

            oDataset = Me.FiltraggioDati(ApplicaFiltri)
            totale = oDataset.Tables(0).Rows.Count
            If totale = 0 Then 'se datagrid vuota
                Me.DGComunita.Visible = False
                Me.LBmsgDG.Visible = True
                Me.LBnumeroRecord_1.Visible = False
                Me.DDLNumeroRecord.Visible = False
                Me.DGComunita.PagerStyle.Position = PagerPosition.Top
                Me.LNBiscriviMultipli.Enabled = False
                oResource.setLabel_To_Value(Me.LBmsgDG, "elenco." & StringaElenco.noCommunityForFilter)
            Else
                Me.DGComunita.Visible = True
                Me.LBmsgDG.Visible = False
                Me.DGComunita.Columns(2).Visible = False
                Me.DGComunita.Columns(3).Visible = False
                Me.PNLmessaggi.Visible = False
                If totale <= Me.DDLNumeroRecord.Items(0).Value Then
                    Me.LBnumeroRecord_1.Visible = False
                    Me.DDLNumeroRecord.Visible = False
                    Me.DGComunita.PagerStyle.Position = PagerPosition.Top
                Else
                    Me.LBnumeroRecord_1.Visible = True
                    Me.DDLNumeroRecord.Visible = True
                    Me.DGComunita.PagerStyle.Position = PagerPosition.TopAndBottom
                End If
                If totale = 1 Then
                    Me.DGComunita.Columns(25).Visible = False
                Else
                    Me.DGComunita.Columns(25).Visible = True
                End If


                Dim oDataview As DataView
                oDataview = oDataset.Tables(0).DefaultView
                If viewstate("SortExspression") = "" Then
                    If Not (Session("limbo") = True And Me.Request.QueryString("show") = 0) Then
                        viewstate("SortExspression") = "CMNT_Livello,CMNT_Nome"
                    Else
                        viewstate("SortExspression") = "CMNT_Nome"
                    End If
                    viewstate("SortDirection") = "asc"
                End If
                oDataview.Sort = viewstate("SortExspression") & " " & viewstate("SortDirection")

                Dim oPersona As New COL_Persona
                oPersona = Session("objPersona")
                If (Session("limbo") = True And Me.Request.QueryString("show") = 0) Then
                    If oPersona.TipoPersona.id = Main.TipoPersonaStandard.Copisteria Then
                        Me.DGComunita.Columns(6).Visible = False
                    Else
                        Me.DGComunita.Columns(6).Visible = True
                    End If
                Else
                    If Session("IdRuolo") = Main.TipoRuoloStandard.Copisteria Then
                        Me.DGComunita.Columns(6).Visible = False
                    Else
                        Me.DGComunita.Columns(6).Visible = True
                    End If
                End If
                DGComunita.DataSource = oDataset
                DGComunita.DataBind()
            End If
        Catch ex As Exception
            Me.DGComunita.Visible = False
            Me.LBmsgDG.Visible = True
            Me.LBnumeroRecord_1.Visible = False
            Me.DDLNumeroRecord.Visible = False
            Me.DGComunita.PagerStyle.Position = PagerPosition.Top
            Me.LNBiscriviMultipli.Enabled = False
            oResource.setLabel_To_Value(Me.LBmsgDG, "elenco." & StringaElenco.noCommunity)
        End Try
    End Sub

#End Region

#Region "Filtro"
    Private Sub CBXautoUpdate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBXautoUpdate.CheckedChanged
        Me.DDLannoAccademico.AutoPostBack = Me.CBXautoUpdate.Checked
        Me.DDLtipoCorsoDiStudi.AutoPostBack = Me.CBXautoUpdate.Checked
        Me.DDLperiodo.AutoPostBack = Me.CBXautoUpdate.Checked
        Me.DDLresponsabile.AutoPostBack = Me.CBXautoUpdate.Checked
        Me.HDNcomunitaSelezionate.Value = ""
        Me.Bind_Griglia(True)
    End Sub
    Private Sub DDLresponsabile_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLresponsabile.SelectedIndexChanged
        If Me.CBXautoUpdate.Checked Then
            DGComunita.PageSize = DDLNumeroRecord.SelectedItem.Value
            DGComunita.CurrentPageIndex = 0
            Me.ViewState("intCurPage") = 0
            Me.HDNcomunitaSelezionate.Value = ""
            Me.Bind_Griglia(True)
        End If
    End Sub
    Private Sub DDLTipoRicerca_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLTipoRicerca.SelectedIndexChanged
        If Me.DDLTipoRicerca.SelectedValue = Main.FiltroComunita.IDresponsabile Then
            Me.DDLresponsabile.Visible = True
            Me.TXBValore.Text = ""
            Me.TXBValore.Visible = False
        Else
            Me.DDLresponsabile.Visible = False
            Me.TXBValore.Visible = True
        End If
        If Me.CBXautoUpdate.Checked Then
            Me.HDNcomunitaSelezionate.Value = ""
        End If
        Me.Bind_Griglia()
    End Sub
    Private Sub DDLTipo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DDLTipo.SelectedIndexChanged
      
            Me.TBLcorsi.Visible = False
            Me.TBLcorsiDiStudio.Visible = False
            Me.LBnoCorsi.Visible = True
            Me.DDLtipoCorsoDiStudi.SelectedIndex = 0
            Me.DDLannoAccademico.SelectedIndex = 0
            Me.DDLperiodo.SelectedIndex = 0
            Me.TBRorgnCorsi.Visible = False


        If Me.CBXautoUpdate.Checked Then
            DGComunita.PageSize = DDLNumeroRecord.SelectedItem.Value
            DGComunita.CurrentPageIndex = 0
            Me.ViewState("intCurPage") = 0
            Me.HDNcomunitaSelezionate.Value = ""
        End If
        Me.Bind_Responsabili()
        Me.Bind_Griglia()
    End Sub
    Private Sub DDLNumeroRecord_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DDLNumeroRecord.SelectedIndexChanged
        Me.ViewState("intCurPage") = 0
        Me.DDLNumeroRecord.SelectedIndex = Me.DDLNumeroRecord.SelectedIndex
        DGComunita.PageSize = DDLNumeroRecord.SelectedItem.Value
        DGComunita.CurrentPageIndex = 0
        Bind_Griglia(True)
    End Sub
    Private Sub DDLannoAccademico_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLannoAccademico.SelectedIndexChanged
        If Me.CBXautoUpdate.Checked Then
            DGComunita.PageSize = DDLNumeroRecord.SelectedItem.Value
            DGComunita.CurrentPageIndex = 0
            Me.ViewState("intCurPage") = 0
            Me.HDNcomunitaSelezionate.Value = ""
            Me.Bind_Griglia(True)
        End If
    End Sub
    Private Sub DDLperiodo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLperiodo.SelectedIndexChanged
        If Me.CBXautoUpdate.Checked Then
            DGComunita.PageSize = DDLNumeroRecord.SelectedItem.Value
            DGComunita.CurrentPageIndex = 0
            Me.ViewState("intCurPage") = 0
            Me.HDNcomunitaSelezionate.Value = ""
            Me.Bind_Griglia(True)
        End If
    End Sub
    Private Sub DDLtipoCorsoDiStudi_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLtipoCorsoDiStudi.SelectedIndexChanged
        If Me.CBXautoUpdate.Checked Then
            DGComunita.PageSize = DDLNumeroRecord.SelectedItem.Value
            DGComunita.CurrentPageIndex = 0
            Me.ViewState("intCurPage") = 0
            Me.HDNcomunitaSelezionate.Value = ""
            Me.Bind_Griglia(True)
        End If
    End Sub

    Private Sub CBXmostraPadre_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBXmostraPadre.CheckedChanged
        If Me.DDLNumeroRecord.SelectedValue <> Me.DGComunita.PageSize Then
            DGComunita.PageSize = DDLNumeroRecord.SelectedItem.Value
            DGComunita.CurrentPageIndex = 0
            Me.ViewState("intCurPage") = 0
        End If
        Me.Bind_Griglia()
    End Sub

    Public Sub FiltroLink_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBaltro.Click
        If sender.commandArgument <> "" Then
            Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
            Me.ViewState("intAnagrafica") = sender.commandArgument
            sender.CssClass = "lettera_Selezionata"
        Else
            Me.ViewState("intAnagrafica") = -1
            Me.LKBtutti.CssClass = "lettera_Selezionata"
        End If
        Me.ViewState("intCurPage") = 0
        Me.DGComunita.CurrentPageIndex = 0
        Me.Bind_Griglia()
    End Sub
    Private Sub DeselezionaLink(ByVal Lettera As String)
        Lettera = CType(CInt(Lettera), Main.FiltroComunita).ToString

        Dim oLink As System.Web.UI.WebControls.LinkButton
        oLink = Me.FindControlRecursive(Me.Master, "LKB" & Lettera)
        If IsNothing(oLink) = False Then
            oLink.CssClass = "lettera"
        End If
    End Sub
    Private Sub SelezionaLink_All()
        Dim i As Integer
        Try
            For i = Asc("a") To Asc("z") 'status dei link button delle lettere
                Dim oLinkButton As New LinkButton
                oLinkButton = FindControlRecursive(Me.Master, "LKB" & Chr(i))
                Dim Carattere As String = Chr(i)
                If IsNothing(oLinkButton) = False Then
                    oLinkButton.CssClass = "lettera"
                End If
            Next
            Me.LKBaltro.CssClass = "lettera"
            Me.ViewState("intAnagrafica") = -1
            Me.LKBtutti.CssClass = "lettera_Selezionata"
        Catch ex As Exception

        End Try
    End Sub
    Private Sub BTNCerca_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNCerca.Click
        Me.DGComunita.PageSize = DDLNumeroRecord.SelectedItem.Value
        Me.DGComunita.CurrentPageIndex = 0
        Me.ViewState("intCurPage") = 0
        Me.HDNcomunitaSelezionate.Value = ""
        Me.Bind_Griglia(True)

    End Sub
#End Region

#Region "Gestione Griglia"
    Private Sub DGComunita_ItemCreated(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles DGComunita.ItemCreated
        Dim i As Integer

        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        If e.Item.ItemType = ListItemType.Header Then
            Dim oSortExspression, oSortDirection, oText, StringaMouse As String
            oSortExspression = viewstate("SortExspression")
            oSortDirection = viewstate("SortDirection")


            For i = 0 To sender.columns.count - 1
                If sender.columns(i).SortExpression <> "" Then
                    Dim oWebControl As WebControl
                    Dim oCell As New TableCell
                    Dim oLabelAfter As New System.Web.UI.WebControls.Label
                    Dim oLabelBefore As New System.Web.UI.WebControls.Label

                    oLabelBefore.font.name = "webdings"
                    oLabelBefore.font.size = FontUnit.XSmall
                    oLabelBefore.text = "&nbsp;"

                    oCell = e.Item.Cells(i)
                    If Me.DGComunita.Columns(i).SortExpression <> "" Then
                        If oSortExspression = sender.columns(i).SortExpression Then
                            Try
                                oWebControl = oCell.Controls(0)
                                Dim oLinkbutton As LinkButton
                                oLinkbutton = oWebControl
                                oLinkbutton.CssClass = "ROW_HeaderLink_Small"

                                oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                                If oSortDirection = "asc" Then
                                    oResource.setHeaderOrderbyLink_Datagrid(Me.DGComunita, oLinkbutton, FiltroOrdinamento.Decrescente)
                                Else
                                    oResource.setHeaderOrderbyLink_Datagrid(Me.DGComunita, oLinkbutton, FiltroOrdinamento.Crescente)
                                End If
                                oLabelAfter.CssClass = Me.DGComunita.HeaderStyle.CssClass
                                oLabelAfter.text = oLinkbutton.Text & " "
                                'oLinkbutton.Font.Name = "webdings"
                                'oLinkbutton.Font.Size = FontUnit.XSmall

                                If oSortDirection = "asc" Then
                                    '  oText = "5"
                                    oText = "<img src='./../images/dg/down.gif' id='Image_" & i & "' >"
                                    If oLinkbutton.Attributes.Item("onmouseout") = "" Then
                                        oLinkbutton.Attributes.Add("onmouseout", "document.forms[0].Image_" & i & ".src='./../images/dg/down.gif';return true;")
                                    Else
                                        StringaMouse = oLinkbutton.Attributes.Item("onmouseout")
                                        StringaMouse = StringaMouse.Replace("return true;", "document.forms[0].Image_" & i & ".src='./../images/dg/down.gif';return true;")
                                        oLinkbutton.Attributes.Item("onmouseout") = StringaMouse
                                    End If
                                    If oLinkbutton.Attributes.Item("onmouseover") = "" Then
                                        oLinkbutton.Attributes.Add("onmouseover", "document.forms[0].Image_" & i & ".src='./../images/dg/down_over.gif';return true;")
                                    Else
                                        StringaMouse = oLinkbutton.Attributes.Item("onmouseover")
                                        StringaMouse = StringaMouse.Replace("return true;", "document.forms[0].Image_" & i & ".src='./../images/dg/down_over.gif';return true;")
                                        oLinkbutton.Attributes.Item("onmouseover") = StringaMouse
                                    End If
                                Else
                                    '  oText = "6"
                                    oText = "<img src='./../images/dg/up.gif' id='Image_" & i & "' >"
                                    If oLinkbutton.Attributes.Item("onmouseout") = "" Then
                                        oLinkbutton.Attributes.Add("onmouseout", "document.forms[0].Image_" & i & ".src='./../images/dg/up.gif';return true;")
                                    Else
                                        StringaMouse = oLinkbutton.Attributes.Item("onmouseout")
                                        StringaMouse = StringaMouse.Replace("return true;", "document.forms[0].Image_" & i & ".src='./../images/dg/up.gif';return true;")
                                        oLinkbutton.Attributes.Item("onmouseout") = StringaMouse
                                    End If
                                    If oLinkbutton.Attributes.Item("onmouseover") = "" Then
                                        oLinkbutton.Attributes.Add("onmouseover", "document.forms[0].Image_" & i & ".src='./../images/dg/up_over.gif';return true;")
                                    Else
                                        StringaMouse = oLinkbutton.Attributes.Item("onmouseover")
                                        StringaMouse = StringaMouse.Replace("return true;", "document.forms[0].Image_" & i & ".src='./../images/dg/up_over.gif';return true;")
                                        oLinkbutton.Attributes.Item("onmouseover") = StringaMouse
                                    End If
                                End If
                                oLinkbutton.Text = oText


                                oCell.Controls.AddAt(0, oLabelAfter)
                            Catch ex As Exception
                                oCell.Controls.AddAt(0, oLabelAfter)
                            End Try
                        Else
                            Try
                                oWebControl = oCell.Controls(0)
                                Dim oLinkbutton As LinkButton
                                oLinkbutton = oWebControl
                                oLinkbutton.CssClass = "ROW_HeaderLink_Small"

                                oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                                oResource.setHeaderOrderbyLink_Datagrid(Me.DGComunita, oLinkbutton, FiltroOrdinamento.Crescente)
                                oLabelAfter.CssClass = Me.DGComunita.HeaderStyle.CssClass
                                oLabelAfter.text = oLinkbutton.Text & " "
                                'oLinkbutton.Font.Name = "webdings"
                                'oLinkbutton.Font.Size = FontUnit.XSmall
                                oLinkbutton.Text = "<img src='./../images/dg/up.gif' id='Image_" & i & "' >"
                                If oLinkbutton.Attributes.Item("onmouseout") = "" Then
                                    oLinkbutton.Attributes.Add("onmouseout", "document.forms[0].Image_" & i & ".src='./../images/dg/up.gif';return true;")
                                Else
                                    StringaMouse = oLinkbutton.Attributes.Item("onmouseout")
                                    StringaMouse = StringaMouse.Replace("return true;", "document.forms[0].Image_" & i & ".src='./../images/dg/up.gif';return true;")
                                    oLinkbutton.Attributes.Item("onmouseout") = StringaMouse
                                End If
                                If oLinkbutton.Attributes.Item("onmouseover") = "" Then
                                    oLinkbutton.Attributes.Add("onmouseover", "document.forms[0].Image_" & i & ".src='./../images/dg/up_over.gif';return true;")
                                Else
                                    StringaMouse = oLinkbutton.Attributes.Item("onmouseover")
                                    StringaMouse = StringaMouse.Replace("return true;", "document.forms[0].Image_" & i & ".src='./../images/dg/up_over.gif';return true;")
                                    oLinkbutton.Attributes.Item("onmouseover") = StringaMouse
                                End If

                                oCell.Controls.AddAt(0, oLabelAfter)
                            Catch ex As Exception
                                oCell.Controls.AddAt(0, oLabelAfter)
                            End Try
                        End If
                    End If
                End If
            Next
        End If
        If e.Item.ItemType = ListItemType.Pager Then
            Dim oCell As TableCell
            Dim n As Integer
            oCell = CType(e.Item.Controls(0), TableCell)


            n = oCell.ColumnSpan
            ' Aggiungo riga con descrizione:

            Try
                Dim oRow As TableRow
                Dim oTableCell As New TableCell
                Dim num As Integer = 0
                oRow = oCell.Parent()

                oTableCell.Controls.Add(Me.CreaLegenda)
                If Me.DGComunita.Columns(2).Visible Then
                    num += 1
                End If
                If Me.DGComunita.Columns(3).Visible Then
                    num += 1
                End If
                If Me.DGComunita.Columns(6).Visible Then
                    num += 1
                End If
                If Me.DGComunita.Columns(25).Visible Then
                    num += 1
                End If
                num += 1
                oTableCell.ColumnSpan = num
                oTableCell.HorizontalAlign = HorizontalAlign.Left
                oCell.ColumnSpan = 2
                oRow.Cells.AddAt(0, oTableCell)
                e.Item.Cells(0).Attributes.Item("colspan") = num.ToString
            Catch ex As Exception

            End Try

            For n = 0 To oCell.Controls.Count - 1 Step 2
                Dim szLnk As String
                szLnk = "System.Web.UI.WebControls.DataGridLinkButton"
                Dim oWebControl As WebControl

                oWebControl = oCell.Controls(n)

                If (oWebControl.GetType().ToString() = szLnk) Then
                    oWebControl.CssClass = "ROW_PagerLink_Small"
                End If
                Try
                    Dim oLabel As Label
                    oLabel = oWebControl
                    oLabel.Text = oLabel.Text
                    oLabel.CssClass = "ROW_PagerSpan_Small"
                Catch ex As Exception
                    Dim oLinkbutton As LinkButton
                    oLinkbutton = oWebControl
                    oLinkbutton.CssClass = "ROW_PagerLink_Small"
                    oResource.setPageDatagrid(Me.DGComunita, oLinkbutton)
                End Try
            Next
        End If
        If (e.Item.ItemType = ListItemType.Footer) Then
            e.Item.Cells(0).ColumnSpan = e.Item.Cells.Count
            For i = 1 To e.Item.Cells.Count - 1
                e.Item.Cells.RemoveAt(1)
            Next
        End If
        If (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then
            Try

                Dim cssLink As String = "ROW_ItemLink_Small"
                Dim cssRiga As String = "ROW_TD_Small"
                Dim hasAccesso As Boolean = False
                Dim hasIscrizione As Boolean = False

                Dim CMNT_Nome As String

                Try
                    CMNT_Nome = e.Item.DataItem("CMNT_Esteso")
                    If CMNT_Nome <> "" Then
                        CMNT_Nome = ": " & Replace(CMNT_Nome, "'", "\'")
                    End If
                Catch ex As Exception
                    CMNT_Nome = ""
                End Try

                Dim TestoLinkIscrivi As String = ""

                Dim DataInizioIscrizione As DateTime = Now
                Dim CMNT_dataInizioIscrizione, CMNT_dataFineIscrizione As DateTime

                hasIscrizione = True

                If e.Item.DataItem("CMNT_CanSubscribe") = False Or e.Item.DataItem("CMNT_Bloccata") = True Or e.Item.DataItem("CMNT_Archiviata") = True Then
                    hasIscrizione = False
                Else
                    Dim oPersona As COL_Persona


                    Dim DataTemp As DateTime
                    oPersona = Session("objPersona")
                    If IsDate(e.Item.DataItem("CMNT_dataInizioIscrizione")) Then
                        CMNT_dataInizioIscrizione = e.Item.DataItem("CMNT_dataInizioIscrizione")
                    End If
                    If IsDate(e.Item.DataItem("CMNT_dataFineIscrizione")) Then
                        CMNT_dataFineIscrizione = e.Item.DataItem("CMNT_dataFineIscrizione")
                        DataTemp = CMNT_dataFineIscrizione.Date()
                        DataTemp = DataTemp.AddHours(23)
                        DataTemp = DataTemp.AddMinutes(59)
                        CMNT_dataFineIscrizione = DataTemp
                    End If
                    If CMNT_dataInizioIscrizione > DataInizioIscrizione Then
                        CMNT_Nome &= oResource.getValue("iscrizioni." & Iscrizioni_code.IscrizioniAperteIl)
                        CMNT_Nome = CMNT_Nome.Replace("#%%#", CMNT_dataInizioIscrizione)
                        hasIscrizione = False
                    Else
                        If IsDate(CMNT_dataFineIscrizione) Then
                            If CMNT_dataFineIscrizione < DataInizioIscrizione And Not Equals(New Date, CMNT_dataFineIscrizione) Then
                                TestoLinkIscrivi = oResource.getValue("iscrizioni." & Iscrizioni_code.IscrizioniChiuse)
                                If oPersona.TipoPersona.ID <> Main.TipoPersonaStandard.Copisteria Then
                                    hasIscrizione = False
                                End If
                            End If
                        End If
                    End If

                    If hasIscrizione Then
                        If Not (e.Item.DataItem("CMNT_Iscritti") >= 0) Then
                            If oPersona.TipoPersona.ID <> Main.TipoPersonaStandard.Copisteria Then
                                hasIscrizione = False
                            End If
                        End If
                    End If
                End If

                If e.Item.DataItem("CMNT_CanSubscribe") = False Or e.Item.DataItem("CMNT_Bloccata") = True Or e.Item.DataItem("CMNT_Archiviata") = True Then
                    e.Item.CssClass = "ROW_Disattivate_Small"
                    cssLink = "ROW_ItemLinkDisattivate_Small"
                    cssRiga = "ROW_ItemDisattivate_Small"
                ElseIf hasIscrizione = False Then
                    e.Item.CssClass = "ROW_Disabilitate_Small"
                ElseIf e.Item.ItemType = ListItemType.AlternatingItem Then
                    e.Item.CssClass = "ROW_Alternate_Small"
                Else
                    e.Item.CssClass = "ROW_Normal_Small"
                End If


                Try
                    Dim oTBRnome As TableRow
                    Dim oTBCchiusa, oTBCnome As TableCell

                    oTBRnome = e.Item.Cells(1).FindControl("TBRnome")
                    oTBCchiusa = e.Item.Cells(1).FindControl("TBCchiusa")
                    oTBCnome = e.Item.Cells(1).FindControl("TBCnome")

                    If IsNothing(oTBRnome) = False Then
                        oTBRnome.CssClass = cssRiga
                    End If
                    If IsNothing(oTBCchiusa) = False Then
                        oTBCchiusa.CssClass = cssRiga
                    End If
                    If IsNothing(oTBCnome) = False Then
                        oTBCnome.CssClass = cssRiga
                    End If
                Catch ex As Exception

                End Try

                Try
                    Dim oIMGisChiusa As System.Web.UI.WebControls.Image
                    oIMGisChiusa = e.Item.Cells(1).FindControl("IMGisChiusa")

                    If IsNothing(oIMGisChiusa) = False Then
                        Dim ImageBaseDir As String
                        ImageBaseDir = GetPercorsoApplicazione(Me.Request)
                        ImageBaseDir = ImageBaseDir & "/RadControls/TreeView/Skins/Comunita/"

                        oIMGisChiusa.Visible = True
                        oIMGisChiusa.ImageUrl = ImageBaseDir & oResource.getValue("stato.image." & e.Item.DataItem("CMNT_isChiusa"))
                        oIMGisChiusa.AlternateText = oResource.getValue("stato." & e.Item.DataItem("CMNT_isChiusa"))
                    End If
                Catch ex As Exception

                End Try

                Try
                    Dim oLBcomunitaNome As Label

                    oLBcomunitaNome = e.Item.Cells(1).FindControl("LBcomunitaNome")
                    If IsNothing(oLBcomunitaNome) = False Then
                        oLBcomunitaNome.CssClass = cssRiga
                    End If
                Catch ex As Exception

                End Try


                Dim oCell As New TableCell
                'Link Mostra dettagli
                Try

                    Dim oLNBdettagli As LinkButton
                    oLNBdettagli = e.Item.Cells(1).FindControl("LNBdettagli")
                    If IsNothing(oLNBdettagli) = False Then
                        oResource.setLinkButton(oLNBdettagli, True, True)
                        oLNBdettagli.CssClass = cssLink
                    End If
                Catch ex As Exception

                End Try

                '' SISTEMARE
                Dim CanSubscribe As Boolean = True
                Try
                    Dim oLNBiscrivi As LinkButton
                    Dim oPersona As New COL_Persona
                    ' Link iscrizione comunità
                    oLNBiscrivi = e.Item.Cells(1).FindControl("LNBiscrivi")
                    oLNBiscrivi.CssClass = cssLink
                    oLNBiscrivi.Visible = True

                
                    oPersona = Session("objPersona")
                    If e.Item.DataItem("CMNT_CanSubscribe") Then
                        If oLNBiscrivi.Visible = True Then
                            If IsNothing(oLNBiscrivi) = False Then
                                oResource.setLinkButton(oLNBiscrivi, True, False)
                            End If
                            ' iscrizione alla comunità
                            If CMNT_dataInizioIscrizione > DataInizioIscrizione Then
                                ' devo iscrivermi, ma iscrizioni non aperte !
                                CMNT_Nome = CMNT_Nome = oResource.getValue("iscrizioni." & Iscrizioni_code.IscrizioniAperteIl)
                                CMNT_Nome = CMNT_Nome.Replace("#%%#", CMNT_dataInizioIscrizione)
                                oLNBiscrivi.Enabled = False
                                CanSubscribe = False
                            Else
                                If IsDate(CMNT_dataFineIscrizione) Then
                                    If CMNT_dataFineIscrizione < DataInizioIscrizione And Not Equals(New Date, CMNT_dataFineIscrizione) Then
                                        oLNBiscrivi.Text = oResource.getValue("iscrizioni." & Iscrizioni_code.IscrizioniChiuse)
                                        If oPersona.TipoPersona.id = Main.TipoPersonaStandard.Copisteria Then
                                            oLNBiscrivi.Enabled = True
                                        Else
                                            oLNBiscrivi.Enabled = False
                                            CanSubscribe = False
                                        End If
                                    Else
                                        oLNBiscrivi.Enabled = True
                                    End If
                                Else
                                    oLNBiscrivi.Enabled = True
                                End If
                            End If


                            ' se il numero iscritti è stato superato chiudo la possibilità di iscrivere !!!

                            If e.Item.DataItem("CMNT_Iscritti") >= 0 And oLNBiscrivi.Enabled Then
                                oLNBiscrivi.Enabled = True
                            Else
                                If oPersona.TipoPersona.id = Main.TipoPersonaStandard.Copisteria And oLNBiscrivi.Enabled = True Then
                                    oLNBiscrivi.Enabled = True
                                Else
                                    oLNBiscrivi.Enabled = False
                                    CanSubscribe = False
                                End If
                            End If
                        End If
                        If oLNBiscrivi.Enabled Then
                            If e.Item.DataItem("CMNT_Bloccata") Or e.Item.DataItem("CMNT_Archiviata") Then
                                oLNBiscrivi.Enabled = False
                                CanSubscribe = False
                            End If
                        End If
                    Else
                        If IsNothing(oLNBiscrivi) = False Then
                            oResource.setLinkButton(oLNBiscrivi, True, False)
                            oLNBiscrivi.Enabled = False
                        End If
                    End If


                Catch ex As Exception

                End Try

				CanSubscribe = CanSubscribe And hasIscrizione
                If CanSubscribe Then
                    Me.LNBiscriviMultipli.Enabled = True
                End If
                Try
                    Dim oCheckbox As System.Web.UI.HtmlControls.HtmlInputCheckBox
                    oCheckbox = e.Item.Cells(25).FindControl("CBcorso")
                    If Not IsNothing(oCheckbox) Then
                        Try
                            oCheckbox.Visible = CanSubscribe
                            If InStr(Me.HDNcomunitaSelezionate.Value, "," & e.Item.DataItem("CMNT_ID") & ",") > 0 Then
                                If oCheckbox.Visible Then
                                    oCheckbox.Checked = True
                                Else
                                    oCheckbox.Checked = False
                                    Me.HDNcomunitaSelezionate.Value = Replace(Me.HDNcomunitaSelezionate.Value, "," & e.Item.DataItem("CMNT_ID") & ",", ",")
								End If
							ElseIf Not CanSubscribe Then
								oCheckbox.Checked = False
							End If
                            oCheckbox.Value = e.Item.DataItem("CMNT_ID")
                        Catch ex As Exception

                        End Try
                    End If
                Catch ex As Exception

                End Try
            Catch ex As Exception

            End Try
        End If
    End Sub
    Sub DGComunita_pageindexchanged(ByVal obj As Object, ByVal e As DataGridPageChangedEventArgs) Handles DGComunita.PageIndexChanged
        Me.ViewState("intCurPage") = e.NewPageIndex
        DGComunita.CurrentPageIndex = e.NewPageIndex
        Me.Bind_Griglia()
    End Sub
    Private Sub DGComunita_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGComunita.ItemCommand
		Dim oUtility As New OLDpageUtility(Me.Context)

		If IsNothing(oResource) Then
			Me.SetCulture(Session("LinguaCode"))
		End If

        If e.CommandName = "Iscrivi" Or e.CommandName = "dettagli" Or e.CommandName = "Login" Or e.CommandName = "legginews" Then

            Dim oComunita As New COL_Comunita
            Dim oPersona As New COL_Persona
            Dim isAttivoForIscrizione As Boolean = True
            Dim ComunitaID, PersonaID As Integer
            Dim ComunitaPath As String
            Dim isChiusaForPadre As Boolean

            ComunitaID = CInt(DGComunita.DataKeys.Item(e.Item.ItemIndex))
            ComunitaPath = DGComunita.Items(e.Item.ItemIndex).Cells(11).Text()
            isChiusaForPadre = CBool(e.Item.Cells(17).Text)
            oComunita.Id = ComunitaID
            oComunita.Estrai()
            oPersona = Session("objPersona")
            PersonaID = oPersona.Id

            If oPersona.TipoPersona.id = Main.TipoPersonaStandard.Copisteria Then
                isAttivoForIscrizione = oComunita.HasAccessoCopisteria
            End If
            Select Case e.CommandName
                Case "Iscrivi"
                    'richiamo la sub che effettua l'iscrizione della persona

                    Dim oImpostazioni As New COL_ImpostazioniUtente
                    Dim exitSub As Boolean = False
                    Try
                        oImpostazioni = Session("oImpostazioni")
                        exitSub = Not oImpostazioni.ShowConferma
                    Catch ex As Exception
                        exitSub = False
                    End Try

                    Me.ResetFormAll()
                    If isAttivoForIscrizione And (oComunita.CanSubscribe And oComunita.Archiviata = False And oComunita.Bloccata = False) Then
                        If Not exitSub Then
                            Session("azione") = "iscrivi"
                            Me.HDNcmnt_ID.Value = ComunitaID
                            Me.HDNisChiusa.Value = isChiusaForPadre
                            Me.HDNcmnt_Path.Value = ComunitaPath
                            Me.ResetFormToConferma(False, e.Item.Cells(22).Text, e.Item.Cells(5).Text)
                        Else
                            If Session("azione") <> "iscrivi" Then
                                Dim iResponse As Main.ErroriIscrizioneComunita
                                Dim oResourceConfig As New ResourceManager
                                oResourceConfig = GetResourceConfig(Session("LinguaCode"))
                                Session("azione") = "iscrivi"
                                iResponse = oPersona.IscrizioneComunitaNew(ComunitaID, ComunitaPath, isChiusaForPadre, Server.MapPath("./../profili/") & PersonaID & "\", "./../", oUtility.ApplicationUrlBase, Session.SessionID, oUtility.LinguaCode, oUtility.LocalizedMail, oUtility.SystemSettings.Login.DaysToUpdateProfile)
                                lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.BaseModules.CommunityManagement.Domain.CacheKeys.UserCommunitiesTree(oPersona.ID))
                                If iResponse <> Main.ErroriIscrizioneComunita.IscrizioneAvvenuta And iResponse <> Main.ErroriIscrizioneComunita.IscrizioneInAttesa Then
                                    Me.ResetFormForMessaggio()
                                    Me.LBMessaggi.Text = Me.oResource.getValue("ErroriIscrizioneComunita." & CType(iResponse, Main.ErroriIscrizioneComunita))
                                Else
                                    Dim oServiceUtility As New SubscriptionNotificationUtility(Me.PageUtility)
                                    If iResponse = Main.ErroriIscrizioneComunita.IscrizioneAvvenuta Then
                                        oServiceUtility.NotifyAddSelfSubscription(ComunitaID, Me.PageUtility.CurrentUser.ID, Me.PageUtility.CurrentUser.Anagrafica)
                                    Else
                                        oServiceUtility.NotifyAddWaitingSubscription(ComunitaID, Me.PageUtility.CurrentUser.ID, Me.PageUtility.CurrentUser.Anagrafica)
                                    End If
                                    Me.LBiscrizione.Text = Me.oResource.getValue("ErroriIscrizioneComunita." & CType(iResponse, Main.ErroriIscrizioneComunita))
                                    Me.ResetFormForIscritto()
                                End If
                            Else
                                Session("azione") = "loaded"
                                Me.ResetForm(True)
                            End If
                        End If
                    Else
                        Dim alertMSG As String = ""
                        If Not isAttivoForIscrizione Then
                            alertMSG = oResource.getValue("messaggio.BloccataForCopisteria")
                            If alertMSG <> "" Then
                                alertMSG = alertMSG.Replace("'", "\'")
                            Else
                                alertMSG = "Non è possibile iscriversi ad una comunità bloccata !"
                            End If
                        ElseIf Not oComunita.Bloccata Then
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

                        Me.ResetForm(True)
                    End If

                Case "dettagli"
                    Dim oRuoloComunita As New COL_RuoloPersonaComunita

                    Me.ResetFormAll()
                    Me.PNLdettagli.Visible = True
                    Me.PNLmenuDettagli.Visible = True

                    Try
                        oRuoloComunita.Estrai(ComunitaID, PersonaID)
                        If oRuoloComunita.Errore = Errori_Db.None Then
                            If oRuoloComunita.TipoRuolo.Id > -1 Then
                                Me.LNBiscriviDettagli.Visible = False
                            Else
                                Me.LNBiscriviDettagli.Visible = True
                                Me.LNBiscriviDettagli.Enabled = True
                            End If
                        Else
                            Me.LNBiscriviDettagli.Visible = True
                            Me.LNBiscriviDettagli.Enabled = True
                        End If

                        If Me.LNBiscriviDettagli.Enabled = True Then
                            If oComunita.DataInizioIscrizione > Now Then
                                Me.LNBiscriviDettagli.Visible = False
                            Else
                                If Not Equals(New Date, oComunita.DataFineIscrizione) Then
                                    Dim DataTemp As DateTime
                                    DataTemp = oComunita.DataFineIscrizione.Date
                                    DataTemp = DataTemp.AddHours(23)
                                    DataTemp = DataTemp.AddMinutes(59)
                                    If DataTemp < Now Then
                                        Me.LNBiscriviDettagli.Visible = False
                                    End If
                                End If
                            End If

                        End If
                    Catch ex As Exception
                        Me.LNBiscriviDettagli.Visible = False
                    End Try

                    Me.HDNcmnt_ID.Value = ComunitaID
                    Me.HDNisChiusaForPadre.Value = isChiusaForPadre
                    Me.HDNcmnt_Path.Value = ComunitaPath
                    Me.PNLcontenuto.Visible = False

                    If Me.LNBiscriviDettagli.Enabled = True And (oComunita.Bloccata Or Not oComunita.CanSubscribe Or oComunita.Archiviata Or Not isAttivoForIscrizione) Then
                        Me.LNBiscriviDettagli.Enabled = False
                    End If
                    Me.VisualizzaDettagli(ComunitaID)
            End Select
        End If
    End Sub
    Private Sub DGComunita_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DGComunita.SortCommand
        Dim oSortExpression, oSortDirection As String
        oSortExpression = viewstate("SortExspression")
        oSortDirection = viewstate("SortDirection")
        viewstate("SortExspression") = e.SortExpression

        If e.SortExpression = oSortExpression Then

            If viewstate("SortDirection") = "asc" Then
                viewstate("SortDirection") = "desc"
            Else
                viewstate("SortDirection") = "asc"
            End If
        Else
            viewstate("SortDirection") = "asc"
        End If
        Me.Bind_Griglia()

    End Sub

    Private Function CreaLegenda() As Table
        Dim oTable As New Table
        Dim oRow As New TableRow
        Dim oTableCell As New TableCell

        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If


        oTableCell.Width = System.Web.UI.WebControls.Unit.Pixel(10)
        oTableCell.CssClass = "ROW_Disattivate_Small"
        oTableCell.Text = "&nbsp;"
        oRow.Cells.Add(oTableCell)

        oTableCell = New TableCell
        oTableCell.Text = oResource.getValue("ISCRIZIONIdisattivate")

        oTableCell.CssClass = "ROW_Page_Small"
        oRow.Cells.Add(oTableCell)


        oTableCell = New TableCell
        oTableCell.Width = System.Web.UI.WebControls.Unit.Pixel(10)
        oTableCell.CssClass = "ROW_Disabilitate_Small"
        oTableCell.Text = "&nbsp;"
        oRow.Cells.Add(oTableCell)

        oTableCell = New TableCell
        oTableCell.Text = oResource.getValue("ISCRIZIONInonPossibili")

        oTableCell.CssClass = "ROW_Page_Small"
        oRow.Cells.Add(oTableCell)
        oTable.Rows.Add(oRow)

        Return oTable
    End Function
#End Region

#Region "Dettagli"
    Private Sub LNBiscriviDettagli_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBiscriviDettagli.Click
        Dim oImpostazioni As New COL_ImpostazioniUtente
        Dim exitSub As Boolean = False
        Dim oUtility As New OLDpageUtility(Me.Context)

        Try
            oImpostazioni = Session("oImpostazioni")
            exitSub = Not oImpostazioni.ShowConferma
        Catch ex As Exception
            exitSub = False
        End Try
        Me.PNLdettagli.Visible = False

        If Not exitSub Then
            Session("azione") = "iscrivi"
            Dim oComunita As New COL_Comunita
            oComunita.Id = Me.HDNcmnt_ID.Value

            Me.HDNisChiusa.Value = CBool(Me.HDNisChiusaForPadre.Value)
            Me.ResetFormToConferma(False, oComunita.EstraiNomeBylingua(Session("LinguaID")), oComunita.GetNomeResponsabile_NomeCreatore())
        Else
            If Session("azione") <> "iscrivi" Then
                Session("azione") = "iscrivi"
                Try
                    Dim iResponse As Main.ErroriIscrizioneComunita
                    Dim oPersona As New COL_Persona

                    oPersona = Session("objPersona")

                    iResponse = oPersona.IscrizioneComunitaNew(Me.HDNcmnt_ID.Value, Me.HDNcmnt_Path.Value, Me.HDNisChiusa.Value, Server.MapPath("./../profili/") & oPersona.ID & "\", "./../", oUtility.ApplicationUrlBase, Session.SessionID, oUtility.LinguaCode, oUtility.LocalizedMail, oUtility.SystemSettings.Login.DaysToUpdateProfile)
                    lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.BaseModules.CommunityManagement.Domain.CacheKeys.UserCommunitiesTree(oPersona.ID))
                    If iResponse <> Main.ErroriIscrizioneComunita.IscrizioneAvvenuta And iResponse <> Main.ErroriIscrizioneComunita.IscrizioneInAttesa Then
                        Me.ResetFormForMessaggio()
                        Me.LBMessaggi.Text = Me.oResource.getValue("ErroriIscrizioneComunita." & CType(iResponse, Main.ErroriIscrizioneComunita))
                    Else
                        Dim oServiceUtility As New SubscriptionNotificationUtility(Me.PageUtility)
                        If iResponse = Main.ErroriIscrizioneComunita.IscrizioneAvvenuta Then
                            oServiceUtility.NotifyAddSelfSubscription(Me.HDNcmnt_ID.Value, Me.PageUtility.CurrentUser.ID, Me.PageUtility.CurrentUser.Anagrafica)
                        Else
                            oServiceUtility.NotifyAddWaitingSubscription(Me.HDNcmnt_ID.Value, Me.PageUtility.CurrentUser.ID, Me.PageUtility.CurrentUser.Anagrafica)
                        End If
                        Me.LBiscrizione.Text = Me.oResource.getValue("ErroriIscrizioneComunita." & CType(iResponse, Main.ErroriIscrizioneComunita))
                        Me.ResetFormForIscritto()
                    End If
                    Me.HDNcmnt_ID.Value = ""
                    Me.HDNisChiusa.Value = ""
                    Me.HDNcmnt_Path.Value = ""
                Catch ex As Exception
                    Me.ResetForm(True)
                End Try
            Else
                Me.ResetForm(True)
                Session("azione") = "loaded"
            End If
        End If
    End Sub
    Private Sub LNBannullaDettagli_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBannullaDettagli.Click
        Session("azione") = "loaded"
        Me.ResetForm(True)
    End Sub
    Private Sub LNBiscriviAltre_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBiscriviAltre.Click
        Me.ResetForm(True)
    End Sub
    'Visualizzo i dettagli della comunità
    Private Sub VisualizzaDettagli(ByVal CMNT_Id As Integer)
        Try
            Me.PNLdettagli.Visible = True
            Me.PNLcontenuto.Visible = False

            Me.CTRLDettagli.SetupDettagliComunita(CMNT_Id)
        Catch ex As Exception
            Me.PNLdettagli.Visible = False
            Me.PNLcontenuto.Visible = True
        End Try
    End Sub
#End Region

#Region "Internazionalizzazione"
    Private Sub SetCulture(ByVal Code As String)
        Me.oResource = New ResourceManager

        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_IscrizioneComunita"
        oResource.Folder_Level1 = "Comunita"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            .setLabel(Me.LBNopermessi)
            .setLinkButton(Me.LKBaltro, True, True)
            .setLinkButton(Me.LKBtutti, True, True)
            .setLinkButton(Me.LNBapriFiltro, True, True)
            .setLinkButton(Me.LNBchiudiFiltro, True, True)


            .setLabel(Me.LBannoAccademico_c)
            .setLabel(Me.LBperiodo_c)
            .setLabel(Me.LBtipoCorsoDiStudi_t)
            .setLabel(Me.LBtipoComunita_c)
            .setLabel(Me.LBtipoRicerca_c)
            .setLabel(Me.LBvalore_c)

            .setDropDownList(Me.DDLTipoRicerca, -2)
            .setDropDownList(Me.DDLTipoRicerca, -7)
            .setDropDownList(Me.DDLTipoRicerca, -5)
            .setDropDownList(Me.DDLTipoRicerca, -6)
            .setDropDownList(Me.DDLTipoRicerca, -3)
            .setDropDownList(Me.DDLTipoRicerca, -4)
            .setDropDownList(Me.DDLTipoRicerca, -9)
            Try
                Me.DDLTipoRicerca.Items.FindByValue(-9).Text = .getValue("DDLTipoRicerca.-99")
            Catch ex As Exception

            End Try
            .setCheckBox(Me.CBXautoUpdate)
            .setCheckBox(Me.CBXmostraPadre)
            .setLabel(Me.LBnumeroRecord_1)
            .setLabel(Me.LBlegend)


            .setButton(Me.BTNCerca)
            .setLabel(Me.LBmsgDG)
            .setLinkButton(Me.LNBannullaDettagli, True, True)
            .setLinkButton(Me.LNBiscriviDettagli, True, True)
            .setLinkButton(Me.LNBannullaConferma, True, True)
            .setLinkButton(Me.LNBiscriviConferma, True, True)
            .setLinkButton(Me.LNBiscriviAltre, True, True)
            .setLinkButton(Me.LNBelencoIscritte, True, True)
            .setLinkButton(Me.LNBiscriviMultipli, True, True)
            Me.LNBiscriviMultipli.Attributes.Add("onclick", "return HasComunitaSelezionate(false,'" & Replace(Me.oResource.getValue("MessaggioSelezione"), "'", "\'") & "','');")
            Dim i As Integer
            For i = Asc("a") To Asc("z") 'status dei link button delle lettere
                Dim oLinkButton As New LinkButton
                oLinkButton = FindControlRecursive(Me.Master, "LKB" & Chr(i))
                Dim Carattere As String = Chr(i)
                If IsNothing(oLinkButton) = False Then
                    oResource.setLinkButtonLettera(oLinkButton, "#%%#", Carattere.ToUpper, True, True)
                End If
            Next

            .setHeaderDatagrid(Me.DGComunita, 0, "TPCM_Descrizione", True)
            .setHeaderDatagrid(Me.DGComunita, 1, "CMNT_Nome", True)
            .setHeaderDatagrid(Me.DGComunita, 2, "AnnoAccademico", True)
            .setHeaderDatagrid(Me.DGComunita, 3, "Periodo", True)
            .setHeaderDatagrid(Me.DGComunita, 5, "AnagraficaResponsabile", True)
            .setHeaderDatagrid(Me.DGComunita, 6, "Iscritti", True)
        End With

    End Sub
#End Region

    Private Sub LNBapriFiltro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBapriFiltro.Click
        Me.TBRfiltri.Visible = True
        Me.TBRchiudiFiltro.Visible = True
        Me.TBRapriFiltro.Visible = False
        Me.Bind_Griglia()
    End Sub
    Private Sub LNBchiudiFiltro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBchiudiFiltro.Click
        Me.TBRfiltri.Visible = False
        Me.TBRchiudiFiltro.Visible = False
        Me.TBRapriFiltro.Visible = True
        Me.Bind_Griglia()
    End Sub

    'Conferma iscritti
    Private Sub LNBelencoIscritte_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBelencoIscritte.Click
        Me.PageUtility.RedirectToUrl("Comunita/EntrataComunita.aspx")
    End Sub

    Private Sub LNBiscriviConferma_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBiscriviConferma.Click
        Dim iResponse As Main.ErroriIscrizioneComunita
        Dim oUtility As New OLDpageUtility(Me.Context)
        Dim oPersona As New COL_Persona

        If Session("azione") = "iscrivi" Then
            Me.PNLconferma.Visible = False
            Try
                oPersona = Session("objPersona")

                iResponse = oPersona.IscrizioneComunitaNew(Me.HDNcmnt_ID.Value, Me.HDNcmnt_Path.Value, Me.HDNisChiusa.Value, Server.MapPath("./../profili/") & oPersona.ID & "\", "./../", oUtility.ApplicationUrlBase, Session.SessionID, oUtility.LinguaCode, oUtility.LocalizedMail, oUtility.SystemSettings.Login.DaysToUpdateProfile)
                lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.BaseModules.CommunityManagement.Domain.CacheKeys.UserCommunitiesTree(oPersona.ID))
                If iResponse <> Main.ErroriIscrizioneComunita.IscrizioneAvvenuta And iResponse <> Main.ErroriIscrizioneComunita.IscrizioneInAttesa Then
                    Me.ResetFormForMessaggio()
                    Me.LBMessaggi.Text = Me.oResource.getValue("ErroriIscrizioneComunita." & CType(iResponse, Main.ErroriIscrizioneComunita))
                Else
                    Dim oServiceUtility As New SubscriptionNotificationUtility(Me.PageUtility)
                    If iResponse = Main.ErroriIscrizioneComunita.IscrizioneAvvenuta Then
                        oServiceUtility.NotifyAddSelfSubscription(Me.HDNcmnt_ID.Value, Me.PageUtility.CurrentUser.ID, Me.PageUtility.CurrentUser.Anagrafica)
                    Else
                        oServiceUtility.NotifyAddWaitingSubscription(Me.HDNcmnt_ID.Value, Me.PageUtility.CurrentUser.ID, Me.PageUtility.CurrentUser.Anagrafica)
                    End If
                    Me.LBiscrizione.Text = Me.oResource.getValue("ErroriIscrizioneComunita." & CType(iResponse, Main.ErroriIscrizioneComunita))
                    Me.ResetFormForIscritto()
                End If
                Me.HDNcmnt_ID.Value = ""
                Me.HDNisChiusa.Value = ""
                Me.HDNcmnt_Path.Value = ""
            Catch ex As Exception
                Me.ResetForm(True)
            End Try
        ElseIf Session("azione") = "iscriviMultipli" Then
            Try
                Me.IscrizioneMultipla(False)
            Catch ex As Exception
                Me.ResetForm(True)
            End Try
        Else
            Session("azione") = "loaded"
            Me.ResetForm(True)
        End If
    End Sub
    Private Sub LNBannullaConferma_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBannullaConferma.Click
        Me.ResetForm(True)
    End Sub

    Private Sub ResetFormForMessaggio()
        Me.PNLconferma.Visible = False
        Me.PNLdettagli.Visible = False
        Me.PNLcontenuto.Visible = False
        Me.PNLmessaggi.Visible = True
        Me.PNLmenuAccesso.Visible = True
        Me.PNLmenuDettagli.Visible = False
        Me.PNLmenuIscritto.Visible = False
        Me.PNLmenuConferma.Visible = False
        Me.PNLiscrizioneAvvenuta.Visible = False
        Me.PNLmenuDefault.Visible = False
    End Sub
    Private Sub ResetFormForIscritto()
        Me.PNLconferma.Visible = False
        Me.PNLdettagli.Visible = False
        Me.PNLcontenuto.Visible = False
        Me.PNLmessaggi.Visible = False
        Me.PNLiscrizioneAvvenuta.Visible = True

        Me.PNLmenuAccesso.Visible = False
        Me.PNLmenuDettagli.Visible = False
        Me.PNLmenuConferma.Visible = False
        Me.PNLmenuIscritto.Visible = True
        Me.PNLmenuDefault.Visible = False
    End Sub
    Private Sub ResetFormAll()
        Me.PNLconferma.Visible = False
        Me.PNLdettagli.Visible = False
        Me.PNLcontenuto.Visible = False
        Me.PNLmessaggi.Visible = False
        Me.PNLiscrizioneAvvenuta.Visible = False

        Me.PNLmenuAccesso.Visible = False
        Me.PNLmenuDettagli.Visible = False
        Me.PNLmenuConferma.Visible = False
        Me.PNLmenuIscritto.Visible = False

        Me.PNLmenuDefault.Visible = False
    End Sub
    Private Sub ResetForm(ByVal update As Boolean)
        Session("azione") = "loaded"
        Me.HDNisChiusa.Value = ""
        Me.HDNcmnt_ID.Value = ""
        Me.HDNcmnt_Path.Value = ""

        Me.PNLdettagli.Visible = False
        Me.PNLmessaggi.Visible = False
        Me.PNLdettagli.Visible = False
        Me.PNLconferma.Visible = False
        Me.PNLcontenuto.Visible = True
        Me.PNLiscrizioneAvvenuta.Visible = False

        Me.PNLmenuAccesso.Visible = False
        Me.PNLmenuDettagli.Visible = False
        Me.PNLmenuIscritto.Visible = False
        Me.PNLmenuConferma.Visible = False
        Me.PNLmenuDefault.Visible = True
        If update Then
            Me.Bind_Griglia()
        End If
    End Sub
    Private Sub ResetFormToConferma(ByVal Multiplo As Boolean, ByVal Comunita As String, ByVal Responsabile As String)
        Me.ResetFormAll()
        Me.PNLconferma.Visible = True
        Me.PNLmenuConferma.Visible = True
        If Multiplo Then
            Me.LBconfermaMultipla.Visible = True
            Me.LBconferma.Visible = False
            Me.oResource.setLabel(Me.LBconfermaMultipla)
            Me.LBconfermaMultipla.Text = Me.LBconfermaMultipla.Text & "<br>" & Comunita
        Else
            Me.LBconfermaMultipla.Visible = False
            Me.LBconferma.Visible = True
            Me.oResource.setLabel(Me.LBconferma)
            Me.LBconferma.Text = Replace(Me.LBconferma.Text, "#nomeComunita#", Comunita)
            Me.LBconferma.Text = Replace(Me.LBconferma.Text, "#nomeResponsabile#", Responsabile)
        End If
    End Sub
    Private Sub LNBannulla_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBannulla.Click
        Session("Asione") = "loaded"
        Me.ResetForm(True)
    End Sub

    Private Sub IscrizioneMultipla(ByVal isForconferma As Boolean)
        Dim i, totale As Integer
        Dim oDataset As DataSet
        Dim oDataview As DataView

        oDataset = Me.FiltraggioDatiRistretto
        oDataview = oDataset.Tables(0).DefaultView
        oDataview.RowFilter = "'" & Me.HDNcomunitaSelezionate.Value & "' like '%,' + CMNT_ID +',%'"
        totale = oDataview.Count

        If isForconferma Then
            Dim ListaComunita As String = ""
            For i = 0 To totale - 1
                Dim oRow As DataRow
                oRow = oDataview.Item(i).Row

                If ListaComunita = "" Then
                    ListaComunita = "<ul>" & vbCrLf & "<li>" & oRow.Item("CMNT_Nome") & " - "
                Else
                    ListaComunita &= "<li>" & oRow.Item("CMNT_Nome") & " - "
                End If
                If IsDBNull(oRow.Item("CMNT_Responsabile")) Then
                    ListaComunita &= oRow.Item("AnagraficaCreatore") & vbCrLf
                Else
                    If oRow.Item("CMNT_Responsabile") = "" Then
                        ListaComunita &= oRow.Item("AnagraficaCreatore") & vbCrLf
                    Else
                        ListaComunita &= oRow.Item("CMNT_Responsabile") & vbCrLf
                    End If
                End If
            Next
            If ListaComunita <> "" Then
                ListaComunita &= "</ul>"
            End If
            Me.ResetFormToConferma(True, ListaComunita, "")
        Else
            Dim ListaLimiteSuperato As String = ""
            Dim ListaIscrizioneInAttesa As String = ""
            Dim ListaIscrizioneAvvenuta As String = ""
            Dim ListaErroreGenerico As String = ""
            Dim iResponse As Main.ErroriIscrizioneComunita
            Dim oPersona As New COL_Persona

            oPersona = Session("objPersona")

            Dim oUtility As New OLDpageUtility(Me.Context)
            Dim oServiceUtility As New SubscriptionNotificationUtility(oUtility)
            For i = 0 To totale - 1
                Dim oRow As DataRow
                oRow = oDataview.Item(i).Row

                iResponse = oPersona.IscrizioneComunitaNew(oRow.Item("CMNT_ID"), oRow.Item("ALCM_Path"), oRow.Item("ALCM_isChiusaForPadre"), Server.MapPath("./../profili/") & oPersona.ID & "\", "./../", oUtility.ApplicationUrlBase, Session.SessionID, oUtility.LinguaCode, oUtility.LocalizedMail, oUtility.SystemSettings.Login.DaysToUpdateProfile)

                Select Case iResponse
                    Case Main.ErroriIscrizioneComunita.LimiteSuperato
                        If ListaLimiteSuperato = "" Then
                            ListaLimiteSuperato = "<ul>" & vbCrLf & "<li>" & oRow.Item("CMNT_Nome")
                        Else
                            ListaLimiteSuperato &= vbCrLf & "<li>" & oRow.Item("CMNT_Nome")
                        End If
                    Case Main.ErroriIscrizioneComunita.IscrizioneInAttesa
                        oServiceUtility.NotifyAddWaitingSubscription(oRow.Item("CMNT_ID"), oUtility.CurrentUser.ID, oUtility.CurrentUser.Anagrafica)
                        If ListaIscrizioneInAttesa = "" Then
                            ListaIscrizioneInAttesa = "<ul>" & vbCrLf & "<li>" & oRow.Item("CMNT_Nome")
                        Else
                            ListaIscrizioneInAttesa &= vbCrLf & "<li>" & oRow.Item("CMNT_Nome")
                        End If
                    Case Main.ErroriIscrizioneComunita.IscrizioneAvvenuta
                        oServiceUtility.NotifyAddSelfSubscription(oRow.Item("CMNT_ID"), oUtility.CurrentUser.ID, oUtility.CurrentUser.Anagrafica)
                        If ListaIscrizioneAvvenuta = "" Then
                            ListaIscrizioneAvvenuta = "<ul>" & vbCrLf & "<li>" & oRow.Item("CMNT_Nome")
                        Else
                            ListaIscrizioneAvvenuta &= vbCrLf & "<li>" & oRow.Item("CMNT_Nome")
                        End If
                    Case Main.ErroriIscrizioneComunita.ErroreGenerico
                        If ListaErroreGenerico = "" Then
                            ListaErroreGenerico = "<ul>" & vbCrLf & "<li>" & oRow.Item("CMNT_Nome")
                        Else
                            ListaErroreGenerico &= vbCrLf & "<li>" & oRow.Item("CMNT_Nome")
                        End If
                End Select
            Next
            lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.BaseModules.CommunityManagement.Domain.CacheKeys.UserCommunitiesTree(oPersona.ID))
            Me.HDNcomunitaSelezionate.Value = ""
            Me.ResetFormForIscritto()
            If ListaIscrizioneAvvenuta <> "" Then
                ListaIscrizioneAvvenuta &= "</ul>"
                LBiscrizione.Text = Me.oResource.getValue("ErroriIscrizioneComunita.Multipla." & CType(Main.ErroriIscrizioneComunita.IscrizioneAvvenuta, Main.ErroriIscrizioneComunita))
                LBiscrizione.Text = Replace(LBiscrizione.Text, "#nomeComunita#", ListaIscrizioneAvvenuta) & "<br>"
            End If
            If ListaIscrizioneInAttesa <> "" Then
                ListaIscrizioneInAttesa &= "</ul>"
                LBiscrizione.Text &= Me.oResource.getValue("ErroriIscrizioneComunita.Multipla." & CType(Main.ErroriIscrizioneComunita.IscrizioneInAttesa, Main.ErroriIscrizioneComunita))
                LBiscrizione.Text = Replace(LBiscrizione.Text, "#nomeComunita#", ListaIscrizioneInAttesa) & "<br>"
            End If
            If ListaLimiteSuperato <> "" Then
                ListaLimiteSuperato &= "</ul>"
                LBiscrizione.Text &= Me.oResource.getValue("ErroriIscrizioneComunita.Multipla." & CType(Main.ErroriIscrizioneComunita.LimiteSuperato, Main.ErroriIscrizioneComunita))
                LBiscrizione.Text = Replace(LBiscrizione.Text, "#nomeComunita#", ListaLimiteSuperato) & "<br>"
            End If
            If ListaErroreGenerico <> "" Then
                ListaErroreGenerico &= "</ul>"
                LBiscrizione.Text &= Me.oResource.getValue("ErroriIscrizioneComunita.Multipla." & CType(Main.ErroriIscrizioneComunita.ErroreGenerico, Main.ErroriIscrizioneComunita))
                LBiscrizione.Text = Replace(LBiscrizione.Text, "#nomeComunita#", ListaErroreGenerico) & "<br>"
            End If
        End If
    End Sub

    Private Sub LNBiscriviMultipli_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBiscriviMultipli.Click
        Dim i, totale As Integer
        Dim oDataset As DataSet
        Dim oDataview As DataView

        If Session("azione") <> "iscriviMultipli" Then
            Try
                Dim oImpostazioni As New COL_ImpostazioniUtente
                Dim isRequiredConfirm As Boolean = False
                Dim ListaComunita As String = ""
                Try
                    oImpostazioni = Session("oImpostazioni")
                    isRequiredConfirm = oImpostazioni.ShowConferma
                Catch ex As Exception

                End Try
                Dim iResponse As Main.ErroriIscrizioneComunita
                Dim oPersona As New COL_Persona
                Dim oUtility As New OLDpageUtility(Me.Context)


                oPersona = Session("objPersona")

                oDataset = Me.FiltraggioDatiRistretto
                oDataview = oDataset.Tables(0).DefaultView
                oDataview.RowFilter = "'" & Me.HDNcomunitaSelezionate.Value & "' like '%,' + CMNT_ID +',%'"
                totale = oDataview.Count

                Dim ListaLimiteSuperato, ListaIscrizioneInAttesa, ListaIscrizioneAvvenuta, ListaErroreGenerico As String
                ListaLimiteSuperato = ""
                ListaIscrizioneInAttesa = ""
                ListaIscrizioneAvvenuta = ""
                ListaErroreGenerico = ""
                Dim oServiceUtility As New SubscriptionNotificationUtility(Me.PageUtility)
                For i = 0 To totale - 1
                    Dim oRow As DataRow
                    oRow = oDataview.Item(i).Row

                    If isRequiredConfirm Then
                        If ListaComunita = "" Then
                            ListaComunita = "<ul>" & vbCrLf & "<li>" & oRow.Item("CMNT_Nome") & " - "
                        Else
                            ListaComunita &= "<li>" & oRow.Item("CMNT_Nome") & " - "
                        End If
                        If IsDBNull(oRow.Item("CMNT_Responsabile")) Then
                            ListaComunita &= oRow.Item("AnagraficaCreatore") & vbCrLf
                        Else
                            If oRow.Item("CMNT_Responsabile") = "" Then
                                ListaComunita &= oRow.Item("AnagraficaCreatore") & vbCrLf
                            Else
                                ListaComunita &= oRow.Item("CMNT_Responsabile") & vbCrLf
                            End If
                        End If
                    Else
                        iResponse = oPersona.IscrizioneComunitaNew(oRow.Item("CMNT_ID"), oRow.Item("ALCM_Path"), oRow.Item("ALCM_isChiusaForPadre"), Server.MapPath("./../profili/") & oPersona.ID & "\", "./../", oUtility.ApplicationUrlBase, Session.SessionID, Session("LinguaCode"), oUtility.LocalizedMail, oUtility.SystemSettings.Login.DaysToUpdateProfile)
                        lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.BaseModules.CommunityManagement.Domain.CacheKeys.UserCommunitiesTree(oPersona.ID))
                        Select Case iResponse
                            Case Main.ErroriIscrizioneComunita.LimiteSuperato
                                If ListaLimiteSuperato = "" Then
                                    ListaLimiteSuperato = "<ul>" & vbCrLf & "<li>" & oRow.Item("CMNT_Nome")
                                Else
                                    ListaLimiteSuperato &= vbCrLf & "<li>" & oRow.Item("CMNT_Nome")
                                End If
                            Case Main.ErroriIscrizioneComunita.IscrizioneInAttesa
                                oServiceUtility.NotifyAddWaitingSubscription(oRow.Item("CMNT_ID"), Me.PageUtility.CurrentUser.ID, Me.PageUtility.CurrentUser.Anagrafica)
                                If ListaIscrizioneInAttesa = "" Then
                                    ListaIscrizioneInAttesa = "<ul>" & vbCrLf & "<li>" & oRow.Item("CMNT_Nome")
                                Else
                                    ListaIscrizioneInAttesa &= vbCrLf & "<li>" & oRow.Item("CMNT_Nome")
                                End If
                            Case Main.ErroriIscrizioneComunita.IscrizioneAvvenuta
                                oServiceUtility.NotifyAddSelfSubscription(oRow.Item("CMNT_ID"), Me.PageUtility.CurrentUser.ID, Me.PageUtility.CurrentUser.Anagrafica)
                                If ListaIscrizioneAvvenuta = "" Then
                                    ListaIscrizioneAvvenuta = "<ul>" & vbCrLf & "<li>" & oRow.Item("CMNT_Nome")
                                Else
                                    ListaIscrizioneAvvenuta &= vbCrLf & "<li>" & oRow.Item("CMNT_Nome")
                                End If
                            Case Main.ErroriIscrizioneComunita.ErroreGenerico
                                If ListaErroreGenerico = "" Then
                                    ListaErroreGenerico = "<ul>" & vbCrLf & "<li>" & oRow.Item("CMNT_Nome")
                                Else
                                    ListaErroreGenerico &= vbCrLf & "<li>" & oRow.Item("CMNT_Nome")
                                End If

                        End Select
                    End If
                Next

                If isRequiredConfirm Then
                    Me.IscrizioneMultipla(True)
                    Session("azione") = "iscriviMultipli"
                Else
                    Me.IscrizioneMultipla(False)
                End If
            Catch ex As Exception

            End Try
        Else
            Session("azione") = "loaded"
            Me.ResetForm(True)
        End If
    End Sub


	Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
		PageUtility.CurrentModule = PageUtility.GetModule(Services_IscrizioneComunita.Codex)
	End Sub


    Public ReadOnly Property BodyId As String
        Get
            Return Me.Master.BodyIdCode
        End Get
    End Property
    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AjaxPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AjaxPortal)
        End Get
    End Property

    Private Function FindControlRecursive(ByVal Root As Control, ByVal Id As String) As Control
        If Root.ID = Id Then
            Return Root
        End If

        For Each Ctl As Control In Root.Controls
            Dim FoundCtl As Control = FindControlRecursive(Ctl, Id)
            If FoundCtl IsNot Nothing Then
                Return FoundCtl
            End If
        Next
        Return Nothing
    End Function
End Class



'         <%= Me.BodyId() %>.onkeydown = SubmitRicerca(event);
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