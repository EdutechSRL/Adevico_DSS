Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.CL_permessi
Imports COL_BusinessLogic_v2.Comunita

Partial Public Class UC_Fase1DatiComunita
    Inherits BaseControlSession

#Region "Context"
    Private _service As lm.Comol.Core.Business.BaseModuleManager

    Private ReadOnly Property service As lm.Comol.Core.Business.BaseModuleManager
        Get
            If IsNothing(_service) Then
                Me._service = New lm.Comol.Core.Business.BaseModuleManager(PageUtility.CurrentContext)
            End If
            Return _service
        End Get
    End Property


#End Region

	Public Event AggiornamentoVisualizzazione(ByVal InsertEnabled As Boolean, ByVal Info As WizardComunita_Message)

#Region "Proprietà"
	Public Property ComunitaID() As Integer
		Get
			Try
				ComunitaID = DirectCast(Me.ViewState("ComunitaID"), Integer)
			Catch ex As Exception
				Me.ViewState("ComunitaID") = 0
				ComunitaID = 0
			End Try
		End Get
		Set(ByVal value As Integer)
			Me.ViewState("ComunitaID") = value
		End Set
	End Property
	Public ReadOnly Property ComunitaNome() As String
		Get
			ComunitaNome = Me.TXBCmntNome.Text
		End Get
	End Property
	Public ReadOnly Property Comunita_IDpadre() As Integer
		Get
			Try
				Comunita_IDpadre = DirectCast(Me.ViewState("Comunita_IDpadre"), Integer)
			Catch ex As Exception
				Me.ViewState("Comunita_IDpadre") = 0
				Comunita_IDpadre = 0
			End Try
		End Get
	End Property
	Public Property isSubscripted() As Boolean
		Get
			isSubscripted = Me.CBXiscrivimi.Checked
		End Get
		Set(ByVal Value As Boolean)
			Me.CBXiscrivimi.Checked = Value
		End Set
	End Property
	Public ReadOnly Property isChiusa() As Boolean
		Get
			isChiusa = (RBapertachiusa.SelectedIndex = 1)
		End Get
	End Property
	Public ReadOnly Property TipoComunita_ID() As Integer
        Get
            If DDLtipoComunita.SelectedIndex > -1 Then
                Return DDLtipoComunita.SelectedValue
            Else
                Return -1
            End If
        End Get
	End Property

	Private Property OrganizzazioneID() As Integer
		Get
			Try
				OrganizzazioneID = DirectCast(Me.ViewState("OrganizzazioneID"), Integer)
			Catch ex As Exception
			End Try
		End Get
		Set(ByVal value As Integer)
			Me.ViewState("OrganizzazioneID") = value
		End Set
	End Property
	Private Property CommunityTypeID() As Integer
		Get
			Try
				CommunityTypeID = DirectCast(Me.ViewState("CommunityTypeID"), Integer)
			Catch ex As Exception
			End Try
		End Get
		Set(ByVal value As Integer)
			Me.ViewState("CommunityTypeID") = value
		End Set
	End Property
	Private Property CommunityFatherID() As Integer
		Get
			Try
				CommunityFatherID = DirectCast(Me.ViewState("CommunityFatherID"), Integer)
			Catch ex As Exception
			End Try
		End Get
		Set(ByVal value As Integer)
			Me.ViewState("CommunityFatherID") = value
		End Set
	End Property
#End Region


	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If Page.IsPostBack = False Then
			Me.SetInternazionalizzazione()
		End If
		Me.BindDati()
	End Sub

	Public Overrides Sub BindDati()
		If Me.RBLnumeroIscritti.SelectedValue = 0 Then
			Me.TXBmaxIscritti.Enabled = False
		Else
			Me.TXBmaxIscritti.Enabled = True
		End If
		Me.TXBCmntStatuto.Attributes.Add("onkeypress", "return(LimitText(this,4000));")

		Me.TXBCmntNome.Attributes.Add("onkeypress", "return(LimitTextWithNoCrLf(event,this,200));")
    End Sub

	Public Overrides Sub SetCultureSettings()
		MyBase.SetCulture("pg_UC_Fase1DatiComunita", "Comunita", "UC_WizardComunita")
	End Sub
	Public Overrides Sub SetInternazionalizzazione()
		With MyBase.Resource
			.setLabel(LBtipoComunita_t)
			.setLabel(LBnome_t)
			.setLabel(LBdatatermine_t)
            .setLabel(LBdataInizioIscr_t)
			.setLabel(LBdataFineIscr_t)
			.setLabel(LBstatuto_t)
			.setLabel(LBmaxChar)
			.setLabel(LBisAperta_t)
			.setLabel(LBmaxIscritti)
			
			.setValidationSummary(VLDSum)
			.setRadioButtonList(RBapertachiusa, False)
			.setRadioButtonList(RBapertachiusa, True)
            .setLabel(LBiscrizioni_t)
			.setCheckBox(Me.CBXcanSubscribe)
			.setCheckBox(CBXcanUnSubscribe)
			.setLabel(Me.LBmaxIscrittiOver_t)
			.setCheckBox(CBXiscrivimi)
			.setLabel(Me.LBiscrivimi_t)

			.setLabel(Me.LBaccessiSpeciali_t)
			.setCheckBox(Me.CBXaccessoCopisteria)
			.setCheckBox(Me.CBXaccessoLibero)
			.setLabel(Me.LBstatus_t)
			.setRadioButtonList(Me.RBLstatus, 0)
			.setRadioButtonList(Me.RBLstatus, 1)
			.setRadioButtonList(Me.RBLstatus, 2)
			.setLabel(LBavvisoStatus)
			.setLabel(Me.LBnumIscrizioni)
			.setRadioButtonList(Me.RBLnumeroIscritti, 0)
            .setRadioButtonList(Me.RBLnumeroIscritti, 1)
            .setLabel(LBtags_t)
		End With
	End Sub



	Public Function SetupIniziale(ByVal ComunitaID As Integer, ByVal ComunitaPadreID As Integer, ByVal ForInsert As Boolean) As Boolean
		Dim iResponse As Boolean = False
		Try
			Me.HidePanel()
			iResponse = Me.Bind_Dettagli(ComunitaID, ComunitaPadreID, ForInsert)
		Catch ex As Exception

		End Try
		Return iResponse
	End Function


#Region "Visualizzazione"
	Private Sub HidePanel()
        Me.TBRdataTermine.Visible = False
		Me.TBRiscrivimi.Visible = False
		Me.TBRaccessiSpeciali.Visible = False
		Me.TBRiscritti.Visible = False
	End Sub
	'Attiva Le righe comuni
	Private Sub ShowStandardPanel()

		Me.TBRiscritti.Visible = True
		Me.TBRiscrizioni.Visible = True

		Try
			If Me.ComunitaID > 0 Then
				Me.TBRiscrivimi.Visible = False
			Else
				Me.TBRiscrivimi.Visible = True
			End If
		Catch ex As Exception
			Me.TBRiscrivimi.Visible = True
		End Try



		Me.TBRaccessiSpeciali.Visible = True
		Me.TBRisAperta.Visible = True
		Me.TBRdataTermine.Visible = True
		Me.TBRdataIscrizione.Visible = True

	End Sub
    Private Sub AggiornaPanel(ByVal ForInsert As Boolean, Optional ByVal reloadTags As Boolean = True)
        Dim info As WizardComunita_Message = WizardComunita_Message.NesunaOperazione
        Dim iResponse As Boolean = False
        Me.HidePanel()
        If (Me.ComunitaID = 0 And ForInsert) Or Not ForInsert Then  'significa che sto ancora inserendo la comunità
            Me.CBXaccessoCopisteria.Enabled = True
            Select Case Me.DDLtipoComunita.SelectedItem.Value
                Case Main.TipoComunitaStandard.GruppoDiLavoro
                    Me.RBapertachiusa.SelectedIndex = 0
                    Me.ShowStandardPanel()
                    Me.TBRstart.Visible = True
                    Me.CBXaccessoCopisteria.Checked = False
                    iResponse = True
                    info = WizardComunita_Message.NesunaOperazione
                Case Else
                    Me.CBXaccessoCopisteria.Checked = True
                    Me.RBapertachiusa.SelectedIndex = 0
                    Me.ShowStandardPanel()
                    Me.TBRstart.Visible = True
                    iResponse = True
                    info = WizardComunita_Message.NesunaOperazione
            End Select
            If reloadTags Then
                CTRLtagsSelector.ReloadTags(CInt(Me.DDLtipoComunita.SelectedItem.Value))
            End If
        Else 'la comunità è inserita e devo mostrare i panel per l'aggiunta delle comunità padre
            iResponse = False
            info = WizardComunita_Message.ComunitaAppenaCreata
        End If

        Me.CBXcanSubscribe.Enabled = True
                Me.CBXcanUnSubscribe.Enabled = True
                Me.CBXcanSubscribe.Checked = True
                Me.CBXcanUnSubscribe.Checked = True

            RaiseEvent AggiornamentoVisualizzazione(iResponse, info)
    End Sub
#End Region

#Region "Bind Dati"
    Private Function Bind_Dettagli(ByVal idCommunity As Integer, ByVal idFatherCommunity As Integer, ByVal isForAddNewCommunity As Boolean) As Boolean
        Dim oComunita As New COL_Comunita
        Dim oComunitaPadre As New COL_Comunita
        Try
            Me.TBRiscrivimi.Visible = isForAddNewCommunity
            If isForAddNewCommunity Then
                Me.ComunitaID = 0
                Me.CommunityFatherID = idFatherCommunity
                oComunita.Id = idFatherCommunity
            Else
                Me.ComunitaID = idCommunity
                oComunita.Id = Me.ComunitaID
                CTRLtagsSelector.InitializeControlForCommunity(idCommunity)
            End If
            oComunita.Estrai()

            If oComunita.Errore = Errori_Db.None Then
                CTRLtagsSelector.Enabled = True
                Dim iResponse As Boolean = False
                If isForAddNewCommunity Then
                    iResponse = Me.Bind_TipoComunita()
                    CTRLtagsSelector.InitializeControlForCommunityToAdd(idFatherCommunity, TipoComunita_ID)
                Else
                    Me.DDLtipoComunita.Items.Clear()
                    Me.DDLtipoComunita.Items.Add(New ListItem(oComunita.TipoComunita.Descrizione, oComunita.TipoComunita.ID))
                    iResponse = True
                    AggiornaPanel(False, False)

                End If

                If Not iResponse Then
                    Return iResponse
                Else
                    Dim oItemBloccato As ListItem = Me.RBLstatus.Items.FindByValue(2)

                    Me.OrganizzazioneID = oComunita.Organizzazione.Id
                    If isForAddNewCommunity Then
                        Me.PulisciTextbox()
                        RDPsubscriptionsStart.SelectedDate = Now.Date
                        Me.TXBmaxIscritti.ReadOnly = (Me.RBLnumeroIscritti.SelectedValue = 0)
                        Me.TXBmaxIscrittiOver.ReadOnly = (Me.RBLnumeroIscritti.SelectedValue = 0)

                        If oComunita.HasAccessoCopisteria Then
                            Me.CBXaccessoCopisteria.Enabled = True
                            Try
                                Select Case Me.DDLtipoComunita.SelectedValue
                                    Case Main.TipoComunitaStandard.GruppoDiLavoro
                                        Me.CBXaccessoCopisteria.Checked = False
                                End Select
                            Catch ex As Exception
                                Me.CBXaccessoCopisteria.Checked = True
                            End Try
                        Else
                            Me.CBXaccessoCopisteria.Checked = False
                            Me.CBXaccessoCopisteria.Enabled = False
                        End If
                        Me.TBRstatus.Visible = True
                        Me.IMGavviso.Visible = True
                        Me.LBavvisoStatus.Visible = True
                        Me.RBLstatus.Visible = False
                        If oComunita.Bloccata Then
                            If IsNothing(oItemBloccato) Then
                                Me.RBLstatus.Items.Add(New ListItem("Bloccata", 2))
                                Me.Resource.setRadioButtonList(Me.RBLstatus, 2)
                            End If
                            Me.RBLstatus.SelectedValue = 2
                            Me.RBLstatus.Enabled = False
                        ElseIf oComunita.Archiviata Then
                            Me.RBLstatus.SelectedValue = 1
                            Me.RBLstatus.Enabled = False
                        Else
                            Me.TBRstatus.Visible = False
                            Me.RBLstatus.SelectedValue = 0
                        End If

                    Else
                        Me.OrganizzazioneID = oComunita.Organizzazione.Id
                        Me.CommunityTypeID = oComunita.TipoComunita.ID
                        Me.CommunityFatherID = oComunita.IdPadre
                        Me.DDLtipoComunita.Enabled = False
                        Me.TBRstatus.Visible = True

                        Dim oPersona As New COL_Persona
                        oPersona = Session("objPersona")

                        oComunitaPadre.Id = oComunita.IdPadre
                        If oComunitaPadre.isArchiviata() Or oComunitaPadre.isBloccata Then
                            Me.IMGavviso.Visible = True
                            Me.LBavvisoStatus.Visible = True
                            Me.RBLstatus.Visible = False
                            Me.RBLstatus.Enabled = False
                        Else
                            Me.IMGavviso.Visible = False
                            Me.LBavvisoStatus.Visible = False
                            Me.RBLstatus.Visible = True
                            Me.RBLstatus.Enabled = True
                        End If

                        If oComunita.Bloccata Then
                            Me.RBLstatus.SelectedValue = 2
                            If IsNothing(oItemBloccato) Then
                                Me.RBLstatus.Items.Add(New ListItem("Bloccata", 2))
                                Me.Resource.setRadioButtonList(Me.RBLstatus, 2)
                            End If
                            If oPersona.TipoPersona.ID = Main.TipoPersonaStandard.SysAdmin Then
                                Me.RBLstatus.Enabled = True
                            Else
                                Me.RBLstatus.Enabled = False
                            End If

                        ElseIf oComunita.Archiviata Then
                            Me.RBLstatus.SelectedValue = 1
                            If oPersona.TipoPersona.ID <> Main.TipoPersonaStandard.SysAdmin Then
                                If Not IsNothing(oItemBloccato) Then
                                    Me.RBLstatus.Items.Remove(oItemBloccato)
                                End If
                            End If
                        Else
                            Me.RBLstatus.SelectedValue = 0
                            If oPersona.TipoPersona.ID <> Main.TipoPersonaStandard.SysAdmin Then
                                If Not IsNothing(oItemBloccato) Then
                                    Me.RBLstatus.Items.Remove(oItemBloccato)
                                End If
                            End If
                        End If

                        Me.SetCSSstatus()
                        iResponse = Bind_DettagliComunitaGenerica(idCommunity)

                    End If
                End If
            Return iResponse
            Else
            Me.CBXaccessoCopisteria.Checked = True
            Me.CBXaccessoCopisteria.Enabled = True
            If Not isForAddNewCommunity Then
                CTRLtagsSelector.Enabled = False
            End If
            Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

	Private Function Bind_TipoComunita(Optional ByVal TipoComunitaID As Integer = -1) As Boolean
		Dim oTipoComunita As New COL_Tipo_Comunita
		Dim oComunita As New COL_Comunita
		Dim oDataSet As New DataSet

		Try

			oComunita.Id = Me.CommunityFatherID
			oComunita.Estrai()

			Me.DDLtipoComunita.Items.Clear()

			oDataSet = oTipoComunita.ElencaSottoComunitaForCreation(oComunita.TipoComunita.ID, Session("LinguaID"))
			If oDataSet.Tables(0).Rows.Count > 0 Then
				DDLtipoComunita.DataSource = oDataSet
				DDLtipoComunita.DataTextField() = "TPCM_descrizione"
				DDLtipoComunita.DataValueField() = "TPCM_id"
				DDLtipoComunita.DataBind()

				Try
					If TipoComunitaID > -1 Then
						Me.DDLtipoComunita.SelectedValue = TipoComunitaID
					End If
				Catch ex As Exception

				End Try
				Me.AggiornaPanel(True)
				Return True
			Else
				Me.Resource.setDropDownList(Me.DDLtipoComunita, 0)
				Me.DDLtipoComunita.Items.Add(New ListItem("-- No Item --", 0))
				RaiseEvent AggiornamentoVisualizzazione(False, WizardComunita_Message.NesunaOperazione)
				Return False
			End If
		Catch ex As Exception
			Me.DDLtipoComunita.Items.Add(New ListItem("-- No Item --", 0))
			Me.Resource.setDropDownList(Me.DDLtipoComunita, 0)
			RaiseEvent AggiornamentoVisualizzazione(False, WizardComunita_Message.NesunaOperazione)
			Return False
		End Try

	End Function
    Private Sub PulisciTextbox() 'svuota il contenuto delle textbox

        Me.TXBCmntStatuto.Text = ""
        Me.TXBCmntNome.Text = ""

        Me.RDPsubscriptionsStart.SelectedDate = DateTime.Now.Date
        Me.RDPsubscriptionsEnd.SelectedDate = Nothing
        Me.RDPdatetimeEnd.SelectedDate = Nothing
      
        Try
            Me.DDLtipoComunita.SelectedIndex = 0
        Catch ex As Exception

        End Try
    
        Me.TXBmaxIscritti.Text = ""
        Me.RBLnumeroIscritti.SelectedValue = 0
        Me.TXBmaxIscrittiOver.Text = ""
        Me.TXBmaxIscrittiOver.ReadOnly = True
        Me.TXBmaxIscritti.ReadOnly = True

        Me.RBapertachiusa.SelectedIndex = 0
        Me.CBXcanSubscribe.Checked = True
        Me.CBXcanUnSubscribe.Checked = True
        Me.CBXcanSubscribe.Enabled = True
        Me.CBXcanUnSubscribe.Enabled = True

    End Sub
	Private Sub DDLtipoComunita_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DDLtipoComunita.SelectedIndexChanged
		AggiornaPanel(True)
	End Sub
    Private Function Bind_DettagliComunitaGenerica(ByVal ComunitaID As Integer) As Boolean
        Dim iResponse As Boolean = False
        Try
            Dim oComunita As New COL_Comunita
            oComunita.Id = ComunitaID
            oComunita.EstraiByLingua(Session("LinguaID"))

            Me.TXBCmntStatuto.Text = oComunita.Statuto
            Me.TXBCmntNome.Text = oComunita.Nome
            Me.RBapertachiusa.SelectedValue = oComunita.IsChiusa
            If Not Equals(oComunita.DataInizioIscrizione, New Date) Then
                Me.RDPsubscriptionsStart.SelectedDate = oComunita.DataInizioIscrizione
            End If
            If Not Equals(oComunita.DataFineIscrizione, New Date) Then
                Me.RDPsubscriptionsEnd.SelectedDate = oComunita.DataFineIscrizione
            End If
            If Not Equals(oComunita.DataCessazione, New Date) Then
                Me.RDPdatetimeEnd.SelectedDate = oComunita.DataCessazione
            End If
            If oComunita.MaxIscritti = 0 Then
                Me.RBLnumeroIscritti.SelectedValue = 0
                Me.TXBmaxIscrittiOver.Text = ""
                Me.TXBmaxIscritti.Text = ""
            Else
                Me.TXBmaxIscritti.Text = oComunita.MaxIscritti
                Me.RBLnumeroIscritti.SelectedValue = 1

                If oComunita.OverMaxIscritti <= 0 Then
                    Me.TXBmaxIscrittiOver.Text = "0"
                Else
                    Me.TXBmaxIscrittiOver.Text = oComunita.OverMaxIscritti
                End If
            End If
            Me.CBXcanSubscribe.Checked = oComunita.CanSubscribe
            Me.CBXcanUnSubscribe.Checked = oComunita.CanUnsubscribe

            Me.CBXcanSubscribe.Enabled = True
            Me.CBXcanUnSubscribe.Enabled = True

            Try
                Dim oComunitaPadre As New COL_Comunita
                oComunitaPadre.IdPadre = oComunita.Id
                oComunitaPadre.Estrai()
                Me.CBXaccessoCopisteria.Enabled = oComunitaPadre.HasAccessoCopisteria
            Catch ex As Exception

            End Try

            Me.TXBmaxIscritti.ReadOnly = (Me.RBLnumeroIscritti.SelectedValue = 0)
            Me.TXBmaxIscrittiOver.ReadOnly = (Me.RBLnumeroIscritti.SelectedValue = 0)

            iResponse = True
        Catch ex As Exception

        End Try
        Return iResponse
    End Function
#End Region

#Region "salva Dati"
    Private Function AggiungiComunitaStandard() As Boolean
        Try
            Dim oComunita As New COL_Comunita
            Dim CMNT_DataInizioIscrizione, CMNT_DataFineIscrizione, CMNT_DataCessazione As DateTime
            Dim oCreatore As String
            Dim oPersona As New COL_Persona
            Dim oComunitaPadre As New COL_Comunita
            oPersona = Session("objPersona")

            oCreatore = oPersona.Cognome & " " & oPersona.Nome


            Dim oRuolo As New COL_RuoloPersonaComunita
            Dim oTreeComunita As New COL_TreeComunita
            Dim CMNT_ID, PRSN_ID, CMNT_Livello As Integer


            If RDPsubscriptionsStart.SelectedDate.HasValue Then
                CMNT_DataInizioIscrizione = RDPsubscriptionsStart.SelectedDate
            End If
            If RDPsubscriptionsEnd.SelectedDate.HasValue Then
                CMNT_DataFineIscrizione = RDPsubscriptionsEnd.SelectedDate
            End If
            If RDPdatetimeEnd.SelectedDate.HasValue Then
                CMNT_DataCessazione = RDPdatetimeEnd.SelectedDate
            End If

            oComunitaPadre.Id = Me.CommunityFatherID
            oComunitaPadre.Estrai()
            CMNT_Livello = oComunitaPadre.Livello + 1


            With oComunita
                .TipoComunita.ID = DDLtipoComunita.SelectedValue
                .TipoComunita.Descrizione = Me.DDLtipoComunita.SelectedItem.Text
                .Statuto = Me.TXBCmntStatuto.Text
                .Nome = Replace(Me.TXBCmntNome.Text, vbCrLf, " ")
                .IdPadre = Me.CommunityFatherID
                .IsChiusa = Me.RBapertachiusa.SelectedValue

                .HasAccessoCopisteria = Me.CBXaccessoCopisteria.Checked
                .HasAccessoLibero = Me.CBXaccessoLibero.Checked
                .Livello = CMNT_Livello
                If Not Equals(New Date, CMNT_DataInizioIscrizione) Then
                    .DataInizioIscrizione = CMNT_DataInizioIscrizione
                End If
                If Not Equals(New Date, CMNT_DataFineIscrizione) Then
                    .DataFineIscrizione = CMNT_DataFineIscrizione
                End If
                If Not Equals(New Date, CMNT_DataCessazione) Then
                    .DataCessazione = CMNT_DataCessazione
                End If

                .Organizzazione.Id = Session("ORGN_id")
                If Me.RBLnumeroIscritti.SelectedValue = 0 Then
                    .MaxIscritti = 0
                    .OverMaxIscritti = 0
                Else
                    .MaxIscritti = Me.TXBmaxIscritti.Text
                    If Me.TXBmaxIscrittiOver.Text = "" Then
                        .OverMaxIscritti = 0
                    Else
                        If IsNumeric(Me.TXBmaxIscrittiOver.Text) Then
                            .OverMaxIscritti = Me.TXBmaxIscrittiOver.Text
                        Else
                            .OverMaxIscritti = 0
                        End If
                    End If
                End If
                .CanSubscribe = Me.CBXcanSubscribe.Checked
                .CanUnsubscribe = Me.CBXcanUnSubscribe.Checked
            End With
            oComunita.Aggiungi(oPersona.ID, oPersona.RicezioneSMS, Me.CBXiscrivimi.Checked)

            If oComunita.Errore = Errori_Db.None Then
                'aggiungo record in file XML !!!
                PRSN_ID = oPersona.ID
                CMNT_ID = oComunita.Id
                Me.ComunitaID = CMNT_ID

                Dim ArrComunita(,) As String
                Dim CMNT_Path As String
                Try
                    ArrComunita = Session("ArrComunita")

                    CMNT_Path = ArrComunita(2, UBound(ArrComunita, 2)) & CMNT_ID & "."

                Catch ex As Exception
                    CMNT_Path = "." & CMNT_ID & "."
                End Try

                oRuolo.EstraiByLinguaDefault(CMNT_ID, PRSN_ID)
                oComunita.TipoComunita.Icona = "./../" & oComunita.TipoComunita.Icona
                oTreeComunita.Directory = Server.MapPath(".\..\profili\") & PRSN_ID & "\"
                oTreeComunita.Nome = PRSN_ID & ".xml"
                oTreeComunita.Insert(oComunita, CMNT_Path, oComunita.GetNomeResponsabile_NomeCreatore, oRuolo)
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function
#End Region

	Public Function CreaComunita() As WizardComunita_Message
		Dim isCreata As Boolean = False
		Dim iResponse As WizardComunita_Message = WizardComunita_Message.DatiMancanti

		If Me.TXBmaxIscritti.Text <> "" Or Me.RBLnumeroIscritti.SelectedValue = 0 Then

            isCreata = Me.AggiungiComunitaStandard()
            If isCreata OrElse ComunitaID > 0 Then
                CTRLtagsSelector.ApplyTags(Me.ComunitaID)
                iResponse = WizardComunita_Message.ComunitaCreata
            Else
                iResponse = WizardComunita_Message.DatiMancanti
            End If
        End If
		Return iResponse
	End Function


	Private Sub SetCSSstatus()
		If Me.RBLstatus.SelectedValue = 0 Then
			Me.TBRstatus.CssClass = "StatusAttivo_Riga"
			Me.LBstatus_t.CssClass = "StatusAttivo_Titolo_campoSmall"
			Me.RBLstatus.CssClass = "StatusAttivo_Testo_CampoSmall"
			Me.LBavvisoStatus.CssClass = "StatusAttivo_Testo_Campo"
		ElseIf Me.RBLstatus.SelectedValue = 1 Then
			Me.TBRstatus.CssClass = "StatusArchiviata_Riga"
			Me.LBstatus_t.CssClass = "StatusArchiviata_Titolo_campoSmall"
			Me.RBLstatus.CssClass = "StatusArchiviata_Testo_CampoSmall"
			Me.LBavvisoStatus.CssClass = "StatusArchiviata_Testo_Campo"
		ElseIf Me.RBLstatus.SelectedValue = 2 Then
			Me.TBRstatus.CssClass = "StatusBloccato_Riga"
			Me.LBstatus_t.CssClass = "StatusBloccato_Titolo_campoSmall"
			Me.RBLstatus.CssClass = "StatusBloccato_Testo_CampoSmall"
			Me.LBavvisoStatus.CssClass = "StatusBloccato_Testo_Campo"
		End If
	End Sub

#Region "salva Dati"
    Private Function SalvaModificheGeneriche(ByVal idPadre As Integer, ByVal oTipoComunita As COL_Tipo_Comunita) As Boolean
        Dim oPersona As New COL_Persona
        Dim PRSN_ID As Integer
        oPersona = Session("objPersona")

        PRSN_ID = oPersona.Id
        Dim oComunita As New COL_Comunita
        With oComunita
            .Id = Me.ComunitaID
            oComunita.Estrai()

            .Organizzazione.Id = Me.OrganizzazioneID
            .TipoComunita = oTipoComunita
            .Nome = Replace(Me.TXBCmntNome.Text, vbCrLf, " ")
            If RDPdatetimeEnd.SelectedDate.HasValue Then
                .DataCessazione = RDPdatetimeEnd.SelectedDate
            Else
                .DataCessazione = New Date
            End If
            If RDPsubscriptionsEnd.SelectedDate.HasValue Then
                .DataInizioIscrizione = Me.RDPsubscriptionsStart.SelectedDate
            Else
                .DataInizioIscrizione = Now.Date
                Me.RDPsubscriptionsStart.SelectedDate = Now.Date
            End If
            If RDPsubscriptionsEnd.SelectedDate.HasValue Then
                .DataFineIscrizione = RDPsubscriptionsEnd.SelectedDate
            Else
                .DataFineIscrizione = New Date
            End If
            If TXBCmntStatuto.Text <> "" Then
                .Statuto = TXBCmntStatuto.Text
            End If
            .IsChiusa = Me.RBapertachiusa.SelectedValue
            .ModelloComunita.Id = oTipoComunita.ModelloDefault()
            .IdPadre = idPadre
            If Me.RBLnumeroIscritti.SelectedValue = 0 Then
                .MaxIscritti = 0
                .OverMaxIscritti = 0
            Else
                .MaxIscritti = Me.TXBmaxIscritti.Text
                If Me.TXBmaxIscrittiOver.Text = "" Then
                    .OverMaxIscritti = 0
                Else
                    If IsNumeric(Me.TXBmaxIscrittiOver.Text) Then
                        .OverMaxIscritti = Me.TXBmaxIscrittiOver.Text
                    Else
                        .OverMaxIscritti = 0
                    End If
                End If
            End If

            .CanSubscribe = Me.CBXcanSubscribe.Checked
            .CanUnsubscribe = Me.CBXcanUnSubscribe.Checked
            .HasAccessoCopisteria = Me.CBXaccessoCopisteria.Checked
            .HasAccessoLibero = Me.CBXaccessoLibero.Checked
        End With
        Try
            oComunita.Modifica()
            If oComunita.Errore = Errori_Db.None Then
                If idPadre = 0 AndAlso Not IsNothing(oTipoComunita) AndAlso oTipoComunita.ID = lm.Comol.Core.DomainModel.CommunityTypeStandard.Organization Then
                    Dim organization As lm.Comol.Core.DomainModel.Organization
                    organization = service.Get(Of lm.Comol.Core.DomainModel.Organization)(oComunita.Organizzazione.Id)
                    If Not IsNothing(organization) Then
                        organization.Name = oComunita.Nome
                        Try
                            service.SaveOrUpdate(organization)
                        Catch ex As Exception

                        End Try
                    End If
                End If
                Me.SetStatus(oComunita.Id, oComunita.Archiviata, oComunita.Bloccata)
                Dim oTreeComunita As New COL_TreeComunita
                oTreeComunita.Directory = Server.MapPath(PageUtility.BaseUrl) & "\profili\" & PRSN_ID & "\"
                oTreeComunita.Nome = PRSN_ID & ".xml"
                oTreeComunita.AggiornaInfo(PRSN_ID, Session("LinguaID"))
                Return True
            End If
        Catch ex As Exception

        End Try
        Return False
    End Function
#End Region

	Private Sub SetStatus(ByVal ComunitaID As Integer, ByVal isArchiviata As Boolean, ByVal isBloccata As Boolean)
		Select Case Me.RBLstatus.SelectedValue
			Case 0
				If isArchiviata And Not isBloccata Then
					COL_Comunita.DeArchiviaMi(ComunitaID)
				ElseIf isBloccata Then
					COL_Comunita.DeArchiviaMi(ComunitaID)
					COL_Comunita.SbloccaMi(ComunitaID)
				End If
			Case 1
				If Not isArchiviata Then
					COL_Comunita.ArchiviaMi(ComunitaID)
					If isBloccata Then
						COL_Comunita.SbloccaMi(ComunitaID)
					End If
				End If
			Case 2
				If Not isBloccata Then
					COL_Comunita.BloccaMi(ComunitaID)
				End If
		End Select
	End Sub

	Public Function SalvaModifiche() As WizardComunita_Message
		Dim isModificata As Boolean = False
		Dim iResponse As WizardComunita_Message = ModuloEnum.WizardComunita_Message.DatiMancanti

		If Me.TXBmaxIscritti.Text <> "" Or Me.RBLnumeroIscritti.SelectedValue = 0 Then
			Dim oComunita As New COL_Comunita
			Dim oTipoComunita As New COL_Tipo_Comunita
			Dim idPadre, PRSN_ID As Integer

			idPadre = Me.CommunityFatherID
			oTipoComunita.ID = Me.CommunityTypeID

            isModificata = Me.SalvaModificheGeneriche(idPadre, oTipoComunita)

            If isModificata Then
                CTRLtagsSelector.ApplyTags(Me.ComunitaID)
                Me.SetCSSstatus()
                iResponse = ModuloEnum.WizardComunita_Message.ComunitaModificata
            Else
                iResponse = ModuloEnum.WizardComunita_Message.DatiMancanti
            End If
        End If
        Return iResponse
	End Function

	Private Sub RBLnumeroIscritti_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLnumeroIscritti.SelectedIndexChanged
		Me.TXBmaxIscrittiOver.Enabled = Not (Me.RBLnumeroIscritti.SelectedValue = 0)
		Me.TXBmaxIscritti.Enabled = Not (Me.RBLnumeroIscritti.SelectedValue = 0)
		Me.TXBmaxIscrittiOver.ReadOnly = Not Me.TXBmaxIscrittiOver.Enabled
		Me.TXBmaxIscritti.ReadOnly = Not Me.TXBmaxIscritti.Enabled
		If Me.TXBmaxIscritti.Text = "" Then
			Me.TXBmaxIscritti.Text = 30
		End If
		If Me.TXBmaxIscrittiOver.Text = "" Then
			Me.TXBmaxIscrittiOver.Text = 0
		End If
	End Sub
End Class