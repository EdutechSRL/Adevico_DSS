Imports COL_Questionario

Partial Public Class QuestionarioAdd
    Inherits PageBaseQuestionario

    Protected Sub CUVNome_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        Dim oQuest As Questionario = Me.QuestionarioCorrente
        Dim oGestioneQuest As New GestioneQuestionario

        args.IsValid = Not oGestioneQuest.IsDuplicatedName(oQuest.id, oQuest.tipo, TXBNome_WQ.Text) 'oGestioneQuest.controllaNome(Nome)
    End Sub
    Protected Sub CUVdate_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        Dim DataInizio As Date
        Dim DataFine As Date
        DataInizio = RDPDataInizio_WQ.DateInput.SelectedDate.AddHours(DDLOraInizio_WQ.SelectedValue).AddMinutes(DDLMinutiInizio_WQ.SelectedValue)
        DataFine = RDPDataFine_WQ.DateInput.SelectedDate.AddHours(DDLOraFine_WQ.SelectedValue).AddMinutes(DDLMinutiFine_WQ.SelectedValue)
        If DateDiff(DateInterval.Second, DataInizio, DataFine) < 0 And Not DataFine = Date.MaxValue Then
            args.IsValid = False
            Exit Sub
        End If
    End Sub

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Master.ShowDocType = True
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("isZero") = "true" And Me.ComunitaCorrenteID > 0 Then
            GoToPortale()
            Me.QuestionarioCorrente.idGruppo = DALQuestionarioGruppo.GruppoPrincipaleByComunita_Id(Me.ComunitaCorrente.Id)
        End If
        If Not Page.IsPostBack Then
            Me.PaginaCorrenteID = 0
            MLVquestionari.SetActiveView(VIWSceltaQuestionario)
            If Me.qs_questIdType = Questionario.TipoQuestionario.Compilabili Then
                'LBSondaggio.Visible = False
                'LNBSondaggioStandard.Visible = False
                'LNBSondaggioWIZ.Visible = False
                'LBSondaggioDescrizione.Visible = False
                'LBLibreria.Visible = False
                'LNBlibreriaStandard.Visible = False
                'LBLibreriaDescrizione.Visible = False
                'LBModello.Visible = False
                'LNBmodelloStandard.Visible = False
                'LBModelloDescrizione.Visible = False
                TRpollsEmpty.Visible = False
                TRpolls.Visible = False
                TRmodels.Visible = False
                TRlibraryEmpty.Visible = False
                TRlibrary.Visible = False
            Else
                TRpollsEmpty.Visible = True
                TRpolls.Visible = True
                TRmodels.Visible = True
                TRlibraryEmpty.Visible = True
                TRlibrary.Visible = True
            End If
        End If
    End Sub
    Public Overrides Sub BindDati()

    End Sub
    Public Overrides Sub BindNoPermessi()
        MLVquestionari.SetActiveView(VIWMessaggi)
        LBErrore.Text = Me.Resource.getValue("MSGNoPermessi")
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return (MyBase.Servizio.Admin)
    End Function
    Public Overrides ReadOnly Property isCompileForm() As Boolean
        Get

        End Get
    End Property
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Protected Sub bindDateTime()
        Dim counter As Int32
        For counter = 0 To 9
            DDLOraInizio_WQ.Items.Add(counter)
            DDLOraFine_WQ.Items.Add(counter)
            DDLOraInizio_WQ.Items(counter).Text = "0" & counter
            DDLOraInizio_WQ.Items(counter).Value = counter
            DDLOraFine_WQ.Items(counter).Text = "0" & counter
            DDLOraFine_WQ.Items(counter).Value = counter
        Next
        For counter = 10 To 23
            DDLOraInizio_WQ.Items.Add(counter)
            DDLOraFine_WQ.Items.Add(counter)
        Next
        For counter = 0 To 1
            'visualizza 00 e 05 per i minuti
            DDLMinutiInizio_WQ.Items.Add(counter)
            DDLMinutiInizio_WQ.Items(counter).Text = "0" & counter * 5
            DDLMinutiInizio_WQ.Items(counter).Value = counter * 5
            DDLMinutiFine_WQ.Items.Add(counter)
            DDLMinutiFine_WQ.Items(counter).Text = "0" & counter * 5
            DDLMinutiFine_WQ.Items(counter).Value = counter * 5
        Next
        For counter = 2 To 11
            DDLMinutiInizio_WQ.Items.Add(counter)
            DDLMinutiInizio_WQ.Items(counter).Text = counter * 5
            DDLMinutiInizio_WQ.Items(counter).Value = counter * 5
            DDLMinutiFine_WQ.Items.Add(counter)
            DDLMinutiFine_WQ.Items(counter).Text = counter * 5
            DDLMinutiFine_WQ.Items(counter).Value = counter * 5
        Next

        RDPDataInizio_WQ.SelectedDate = Now.Date
        RDPDataFine_WQ.SelectedDate = Date.MaxValue.Date
        DDLOraInizio_WQ.SelectedValue = Now.Hour
        DDLOraFine_WQ.SelectedValue = Date.MaxValue.Hour

        Dim minIndex As Int16
        minIndex = Now.Minute / 5
        If minIndex = 12 Then
            minIndex = 11
        End If
        DDLMinutiInizio_WQ.SelectedIndex = minIndex
        minIndex = Date.MaxValue.Minute / 5
        If minIndex = 12 Then
            minIndex = 11
        End If
        DDLMinutiFine_WQ.SelectedIndex = minIndex
    End Sub
    Protected Sub bindDDLLingua()
        DDLLingua_WQ.DataSource = DALQuestionario.readLingueNonPresentiQuestionario(Me.QuestionarioCorrente.id)
        DDLLingua_WQ.DataBind()
    End Sub
    Public Sub SetControlliWIZByTipoQuestionario()
        setInternazionalizzazioneWIZ()
        bindDDLLingua()
        bindDateTime()
        CHKvisualizzaRisposta_WQ.Attributes.Add("onclick", "JSvisualizzaRisposta(this,'" & CHKvisualizzaCorrezione_WQ.ClientID & "','" & CHKeditaRisposta_WQ.ClientID & "','" & CHKvisualizzaSuggerimenti_WQ.ClientID & "'); return true;")
        CHKvisualizzaCorrezione_WQ.Attributes.Add("onclick", "JSvisualizzaCorrezione(this,'" & CHKvisualizzaRisposta_WQ.ClientID & "','" & CHKeditaRisposta_WQ.ClientID & "','" & CHKvisualizzaSuggerimenti_WQ.ClientID & "'); return true;")
        CHKvisualizzaSuggerimenti_WQ.Attributes.Add("onclick", "JSvisualizzaSuggerimenti(this,'" & CHKvisualizzaRisposta_WQ.ClientID & "','" & CHKvisualizzaCorrezione_WQ.ClientID & "','" & CHKeditaRisposta_WQ.ClientID & "'); return true;")
        CHKeditaRisposta_WQ.Attributes.Add("onclick", "JSeditaRisposta(this,'" & CHKvisualizzaSuggerimenti_WQ.ClientID & "','" & CHKvisualizzaCorrezione_WQ.ClientID & "','" & CHKvisualizzaRisposta_WQ.ClientID & "'); return true;")

        Select Case Me.QuestionarioCorrente.tipo
            Case Questionario.TipoQuestionario.Autovalutazione
                CHKvisualizzaRisposta_WQ.Visible = False
                CHKvisualizzaRisposta_WQ.Checked = True
                LBvisualizzaRisposta_WQ.Visible = False
                LBvisualizzaCorrezione_WQ.Text = Me.Resource.getValue("LBvisualizzaCorrezioneAutovalutazione.text")
                CHKeditaRisposta_WQ.Checked = False
                CHKeditaRisposta_WQ.Visible = False
                LBeditaRisposta_WQ.Visible = False
                LBavvisoBloccato_WQ.Visible = False
                LBanonymousResults_WQ.Visible = False
                CBXanonymousResults_WQ.Visible = False
            Case Questionario.TipoQuestionario.Sondaggio
                'LBDurata_WQ.Visible = False
                'TBDurata_WQ.Visible = False
                'CHKvisualizzaRisposta_WQ.Visible = True
                'CHKvisualizzaRisposta_WQ.Checked = True
                'LBvisualizzaRisposta_WQ.Visible = True
                'LBvisualizzaCorrezione_WQ.Visible = False
                'CHKeditaRisposta_WQ.Checked = False
                'CHKeditaRisposta_WQ.Visible = False
                'LBeditaRisposta_WQ.Visible = False
                'LBavvisoBloccato_WQ.Visible = False

            Case Questionario.TipoQuestionario.Random
                LBOrdineDomandeRandom_WQ.Visible = False
                CHKOrdineDomandeRandom_WQ.Visible = False
        End Select
    End Sub

    Private Sub WIZStatico_NextButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs) Handles WIZStatico.NextButtonClick
        If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Sondaggio Then
            If WIZStatico.ActiveStepIndex = 0 Then
                WIZStatico.ActiveStepIndex = 2
            End If
        End If
    End Sub

    Private Sub WIZStatico_PreviousButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs) Handles WIZStatico.PreviousButtonClick
        If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Sondaggio Then
            If WIZStatico.ActiveStepIndex = 2 Then
                WIZStatico.ActiveStepIndex = 0
            End If
        End If
    End Sub


    Public Overrides Sub SetControlliByPermessi()
        'solo il sysadmin può creare modelli pubblici, e li puo' creare solo prima di accedere a una comunita'
        'non esiste una comunita', quindi non ha senso "compilabile dagli utenti della comunita'"
        If Me.ComunitaCorrenteID = 0 And MyBase.Servizio.Admin Then
            CHKUtentiComunita_WQ.Visible = False
            If Me.QuestionarioCorrente.id = 0 Then
                'viene impostato solo se il questionario e' appena stato creato
                CHKUtentiNonComunita_WQ.Checked = True
            End If
        Else
            CHKUtentiNonComunita_WQ.Checked = False
        End If
    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_QuestionarioAdd", "Questionari")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()


        With Me.Resource
            Master.ServiceTitle = .getValue("ServiceTitle")
            .setLabel(LBQuestionario)
            .setLabel(LBQuestionarioDescrizione)
            .setLabel(LBAutovalutazione)
            .setLabel(LBAutovalutazioneDescrizione)
            .setLabel(LBLibreria)
            .setLabel(LBLibreriaDescrizione)
            .setLabel(LBModello)
            .setLabel(LBModelloDescrizione)
            .setLabel(LBRandom)
            .setLabel(LBRandomDescrizione)
            .setLabel(LBSceltaQuestionario)
            .setLabel(LBSondaggio)
            .setLabel(LBSondaggioDescrizione)

            .setLinkButton(LNBAutovalutazioneStandard, False, True)
            .setLinkButton(LNBlibreriaStandard, False, True)
            .setLinkButton(LNBmodelloStandard, False, True)
            .setLinkButton(LNBQuestionarioStandard, False, True)
            .setLinkButton(LNBRandomStandard, False, True)
            .setLinkButton(LNBSondaggioStandard, False, True)
            .setLinkButton(LNBAutovalutazioneWIZ, False, True)
            .setLinkButton(LNBQuestionarioWIZ, False, True)
            .setLinkButton(LNBRandomWIZ, False, True)
            .setLinkButton(LNBSondaggioWIZ, False, True)
        End With
    End Sub
    Public Sub setInternazionalizzazioneWIZ()
        With Me.Resource
            .setLabel(LBDescrizioneQuestionario_WQ)
            .setLabel(LBNome_WQ)
            .setCustomValidator(CUVNome_WQ)
            .setRequiredFieldValidator(RFVNome_WQ, True, False)
            .setLabel(LBDataInizioTitolo_WQ)
            .setLabel(LBDataFineTitolo_WQ)
            .setCustomValidator(CUVdate_WQ)
            .setLabel(LBDurata_WQ)
            .setLabel(LBDurataDopo_WQ)
            .setLabel(LBScalaValutazione_WQ)
            .setLabel(LBLinguaDefault_WQ)
            .setLabel(LBvisualizzazione_WQ)
            .setLabel(LBvisualizzaRisposta_WQ)
            .setLabel(LBvisualizzaCorrezione_WQ)
            .setLabel(LBvisualizzaSuggerimenti_WQ)
            .setLabel(LBeditaRisposta_WQ)
            .setLabel(LBtitoloDestinatari_WQ)
            .setCompareValidator(COVDurataInt_WQ)
            .setCompareValidator(COVScalaValutazioneInt_WQ)
            .setLabel(LBOrdineDomandeRandom_WQ)
            .setLabel(LBtitoloS1)
            .setLabel(LBdescrizioneS1)
            .setLabel(LBtitoloS2)
            .setLabel(LBdescrizioneS2)
            .setLabel(LBtitoloS3)
            .setLabel(LBdescrizioneS3)

            .setCheckBox(CHKUtentiComunita_WQ)
            .setCheckBox(CHKUtentiNonComunita_WQ)
            .setCheckBox(CHKUtentiInvitati_WQ)
            .setCheckBox(CHKUtentiEsterni_WQ)

            .setLabel(LBavvisoBloccato_WQ)
            .setLabel(LBanonymousResults_WQ)

            WIZStatico.StartNextButtonText = .getValue("StartNextButtonText")
            WIZStatico.StepNextButtonText = .getValue("StepNextButtonText")
            WIZStatico.StepPreviousButtonText = .getValue("StepPreviousButtonText")
            WIZStatico.FinishPreviousButtonText = .getValue("StepPreviousButtonText")
            WIZStatico.FinishCompleteButtonText = .getValue("FinishCompleteButtonText")
        End With
    End Sub
    Private Sub LNBQuestionarioStandard_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBQuestionarioStandard.Click
        Me.QuestionarioCorrente = New Questionario
        Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Questionario
        Me.RedirectToUrl(RootObject.QuestionarioAdmin + "?type=0")
    End Sub
    Private Sub LNBQuestionarioWIZ_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBQuestionarioWIZ.Click
        Me.QuestionarioCorrente = New Questionario
        Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Questionario
        SetControlliWIZByTipoQuestionario()
        MLVquestionari.SetActiveView(VIWWizardQuestionario)
    End Sub
    Private Sub LNBAutovalutazioneStandard_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBAutovalutazioneStandard.Click
        Me.QuestionarioCorrente = New Questionario
        Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Autovalutazione
        Me.QuestionarioCorrente.visualizzaCorrezione = True
        Me.QuestionarioCorrente.visualizzaSuggerimenti = True
        Me.RedirectToUrl(RootObject.QuestionarioAdmin + "?type=5")
    End Sub
    Private Sub LNBAutovalutazioneWIZ_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBAutovalutazioneWIZ.Click
        Me.QuestionarioCorrente = New Questionario
        Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Autovalutazione
        Me.QuestionarioCorrente.visualizzaCorrezione = True
        Me.QuestionarioCorrente.visualizzaSuggerimenti = True
        SetControlliWIZByTipoQuestionario()
        MLVquestionari.SetActiveView(VIWWizardQuestionario)
    End Sub
    Private Sub LNBRandomStandard_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBRandomStandard.Click
        Me.QuestionarioCorrente = New Questionario
        Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Random
        Me.RedirectToUrl(RootObject.QuestionarioAdmin + "?type=4")
    End Sub
    Private Sub LNBRandomWIZ_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBRandomWIZ.Click
        Me.QuestionarioCorrente = New Questionario
        Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Random
        SetControlliWIZByTipoQuestionario()
        MLVquestionari.SetActiveView(VIWWizardQuestionario)
    End Sub
    Private Sub LNBSondaggioStandard_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBSondaggioStandard.Click
        Me.QuestionarioCorrente = New Questionario
        Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Sondaggio
        Me.RedirectToUrl(RootObject.SondaggioAdmin + "?type=2")
    End Sub
    Private Sub LNBSondaggioWIZ_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBSondaggioWIZ.Click
        Me.QuestionarioCorrente = New Questionario
        Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Sondaggio
        MyBase.SetCulture("pg_QuestionarioAdd_Sondaggio", "Questionari")
        SetControlliWIZByTipoQuestionario()
        MLVquestionari.SetActiveView(VIWWizardQuestionario)
    End Sub
    Private Sub LNBLibreriaStandard_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBlibreriaStandard.Click
        Me.QuestionarioCorrente = New Questionario
        Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.LibreriaDiDomande
        Me.RedirectToUrl(RootObject.QuestionarioAdmin + "?type=1")
    End Sub
    Private Sub LNBModelloStandard_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBmodelloStandard.Click
        Me.QuestionarioCorrente = New Questionario
        Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Modello
        Me.RedirectToUrl(RootObject.QuestionarioAdmin + "?type=3")
    End Sub
    Protected Sub WIZStatico_FinishButtonClick(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs) Handles WIZStatico.FinishButtonClick
        Dim saveOK As Boolean
        Dim oGestioneQuest As New GestioneQuestionario
        If Me.QuestionarioCorrente Is Nothing Then
            Me.QuestionarioCorrente = New Questionario
        End If
        Me.QuestionarioCorrente.nome = TXBNome_WQ.Text
        Me.QuestionarioCorrente.descrizione = RootObject.removeBRfromStringEnd(CTRLeditorDescrizioneQuestionario.HTML)

        Me.QuestionarioCorrente.risultatiAnonimi = Me.CBXanonymousResults_WQ.Checked
        Me.QuestionarioCorrente.dataInizio = RDPDataInizio_WQ.DbSelectedDate.AddHours(DDLOraInizio_WQ.SelectedValue).AddMinutes(DDLMinutiInizio_WQ.SelectedValue)
        Me.QuestionarioCorrente.dataFine = RDPDataFine_WQ.DbSelectedDate.AddHours(DDLOraFine_WQ.SelectedValue).AddMinutes(DDLMinutiFine_WQ.SelectedValue)
        Me.QuestionarioCorrente.idLingua = DDLLingua_WQ.SelectedValue
        If TXBScalaValutazione_WQ.Text.Trim(" ") = String.Empty Then
            Me.QuestionarioCorrente.scalaValutazione = RootObject.scalaValutazione
        Else
            Me.QuestionarioCorrente.scalaValutazione = TXBScalaValutazione_WQ.Text
        End If
       
        If TBDurata_WQ.Text.Trim(" ") = String.Empty Then
            Me.QuestionarioCorrente.durata = 0
        Else
            Me.QuestionarioCorrente.durata = TBDurata_WQ.Text
        End If
        Me.QuestionarioCorrente.isReadOnly = False
        Me.QuestionarioCorrente.visualizzaRisposta = CHKvisualizzaRisposta_WQ.Checked
        Me.QuestionarioCorrente.visualizzaCorrezione = CHKvisualizzaCorrezione_WQ.Checked
        Me.QuestionarioCorrente.visualizzaSuggerimenti = CHKvisualizzaSuggerimenti_WQ.Checked
        Me.QuestionarioCorrente.editaRisposta = CHKeditaRisposta_WQ.Checked
        Me.QuestionarioCorrente.isRandomOrder = CHKOrdineDomandeRandom_WQ.Checked
        Me.QuestionarioCorrente.forUtentiComunita = CHKUtentiComunita_WQ.Checked
        Me.QuestionarioCorrente.forUtentiPortale = CHKUtentiNonComunita_WQ.Checked
        Me.QuestionarioCorrente.forUtentiInvitati = CHKUtentiInvitati_WQ.Checked
        Me.QuestionarioCorrente.forUtentiEsterni = CHKUtentiEsterni_WQ.Checked
        Me.QuestionarioCorrente.idPersonaEditor = Me.UtenteCorrente.ID
        Me.QuestionarioCorrente.dataModifica = Now
        Me.QuestionarioCorrente.idGruppo = DALQuestionarioGruppo.GruppoPrincipaleByComunita_Id(Me.ComunitaCorrente.Id)
        Me.QuestionarioCorrente.idPersonaCreator = Me.UtenteCorrente.ID

        If Me.QuestionarioCorrente.tipo = Me.QuestionarioCorrente.TipoQuestionario.Questionario Or Me.QuestionarioCorrente.tipo = Me.QuestionarioCorrente.TipoQuestionario.Sondaggio Then
            Me.QuestionarioCorrente.isBloccato = True
        Else
            Me.QuestionarioCorrente.isBloccato = False
        End If

        If Me.QuestionarioCorrente.tipo = Me.QuestionarioCorrente.TipoQuestionario.Sondaggio Then
            Me.QuestionarioCorrente.isBloccato = True
            Me.QuestionarioCorrente.visualizzaRisposta = True
        End If

        If CUVdate_WQ.IsValid And CUVNome_WQ.IsValid Then
            If Not Me.QuestionarioCorrente.nome.TrimStart("") Is String.Empty Then
                saveOK = oGestioneQuest.salvaQuestionario(Me.QuestionarioCorrente)
            End If
        End If

        If Not DDLLingua_WQ.SelectedValue = Me.QuestionarioCorrente.idLingua Then
            DALQuestionario.updateQuestionarioDefault(Me.QuestionarioCorrente.id, Integer.Parse(DDLLingua_WQ.SelectedValue))
        End If
        If saveOK Then
            Me.RedirectToUrl(RootObject.QuestionarioEdit & "?" & qs_questType & Me.QuestionarioCorrente.tipo)
        End If

    End Sub

    Public Overrides ReadOnly Property LoadDataByUrl As Boolean
        Get
            Return False
        End Get
    End Property
End Class