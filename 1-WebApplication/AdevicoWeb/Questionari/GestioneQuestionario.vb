Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports COL_Questionario
Imports System.Web.UI.WebControls
Imports System.Math
'Imports COL_BusinessLogic_v2.UCServices
'Imports COL_BusinessLogic_v2.UCServices.Services_Questionario


Public Class GestioneQuestionario
    Inherits PageBaseQuestionario
    Public Function notifyCurrentQuestionnaire()
        Dim oServiceNotification As New QuizPollNotificationUtility(Me.PageUtility)

        'con ida=1 non vengono caricate le risposte quando si accede a un quest notificato, viene inserito un doppione e si incasina tutto
        'Me.QuestionarioCorrente.url = RootObject.compileUrl & "?" & Me.CryptQuerystring("idq=" & Me.QuestionarioCorrente.id & "&idl=" & Me.QuestionarioCorrente.idLingua & "&ida=1", SecretKeyUtil.EncType.Questionario)
        Me.QuestionarioCorrente.url = RootObject.compileUrl & "?" & Me.CryptQuerystring("idq=" & Me.QuestionarioCorrente.id & "&idl=" & Me.QuestionarioCorrente.idLingua, SecretKeyUtil.EncType.Questionario)
        Select Case Me.QuestionarioCorrente.tipo
            Case Questionario.TipoQuestionario.Sondaggio
                If Me.QuestionarioCorrente.forUtentiComunita Then
                    oServiceNotification.NotifyAddPoll(Me.ComunitaCorrenteID, Me.QuestionarioCorrente.id, Me.QuestionarioCorrente.dataInizio, Me.QuestionarioCorrente.dataFine, Me.QuestionarioCorrente.url)
                End If
                If Me.QuestionarioCorrente.forUtentiInvitati Then
                    oServiceNotification.NotifyAddPollToPerson(getInvitedUserIDList, Me.ComunitaCorrenteID, Me.QuestionarioCorrente.id, Me.QuestionarioCorrente.dataInizio, Me.QuestionarioCorrente.dataFine, Me.QuestionarioCorrente.url)
                End If
            Case Questionario.TipoQuestionario.Questionario
                If Me.QuestionarioCorrente.forUtentiComunita Then
                    oServiceNotification.NotifyAddQuestionario(Me.ComunitaCorrenteID, Me.QuestionarioCorrente.id, Me.QuestionarioCorrente.dataInizio, Me.QuestionarioCorrente.dataFine, Me.QuestionarioCorrente.url)
                End If
                If Me.QuestionarioCorrente.forUtentiInvitati Then
                    oServiceNotification.NotifyAddQuestionarioToPerson(getInvitedUserIDList, Me.ComunitaCorrenteID, Me.QuestionarioCorrente.id, Me.QuestionarioCorrente.dataInizio, Me.QuestionarioCorrente.dataFine, Me.QuestionarioCorrente.url)
                End If
            Case Questionario.TipoQuestionario.Meeting
                If Me.QuestionarioCorrente.forUtentiComunita Then
                    oServiceNotification.NotifyAddMeeting(Me.ComunitaCorrenteID, Me.QuestionarioCorrente.id, Me.QuestionarioCorrente.dataInizio, Me.QuestionarioCorrente.dataFine, Me.QuestionarioCorrente.url)
                End If
                If Me.QuestionarioCorrente.forUtentiInvitati Then
                    oServiceNotification.NotifyAddMeetingToPerson(getInvitedUserIDList, Me.ComunitaCorrenteID, Me.QuestionarioCorrente.id, Me.QuestionarioCorrente.dataInizio, Me.QuestionarioCorrente.dataFine, Me.QuestionarioCorrente.url)
                End If
            Case Questionario.TipoQuestionario.Autovalutazione
                If Me.QuestionarioCorrente.forUtentiComunita Then
                    oServiceNotification.NotifyAddSelfTest(Me.ComunitaCorrenteID, Me.QuestionarioCorrente.id, Me.QuestionarioCorrente.dataInizio, Me.QuestionarioCorrente.dataFine, Me.QuestionarioCorrente.url)
                End If
                If Me.QuestionarioCorrente.forUtentiInvitati Then
                    oServiceNotification.NotifyAddSelfTestToPerson(getInvitedUserIDList, Me.ComunitaCorrenteID, Me.QuestionarioCorrente.id, Me.QuestionarioCorrente.dataInizio, Me.QuestionarioCorrente.dataFine, Me.QuestionarioCorrente.url)
                End If
        End Select

    End Function
    Private Function getInvitedUserIDList() As List(Of Integer)
        Dim oInvitedUserList As List(Of UtenteInvitato)
        oInvitedUserList = DALUtenteInvitato.readUtentiInvitatiByIDQuestionario(Me.QuestionarioCorrente.id)
        Dim idList As List(Of Integer) = (From InvitedUsers In oInvitedUserList Where (InvitedUsers.PersonaID > 0) Select InvitedUsers.PersonaID).ToList
        Return idList

    End Function
    Public Function generaQuestionariRandomDestinatario(ByRef UtentiInvitatiList As List(Of UtenteInvitato)) As list(Of Questionario)
        Dim questionari As New list(Of Questionario)
        Dim librerie As New list(Of Questionario)

        DALQuestionario.DeleteQuestionarioRandomNoRisposte_Physical(Me.QuestionarioCorrente.id)
        For Each oLibreria As LibreriaQuestionario In Me.QuestionarioCorrente.librerieQuestionario
            Dim oNewLibreriaDomande As New Questionario
            Dim oLibreriaDomande As New Questionario

            oLibreriaDomande = DALQuestionario.readQuestionarioBYLingua(Me.PageUtility.CurrentContext, oLibreria.idLibreria, Me.LinguaID, False)
            Dim oDomandeFacili As New QuestionarioPagina
            Dim oDomandeMedie As New QuestionarioPagina
            Dim oDomandeDifficili As New QuestionarioPagina
            For Each oDomanda As Domanda In oLibreriaDomande.domande
                Select Case oDomanda.difficolta
                    Case 0
                        oDomandeFacili.domande.Add(oDomanda)
                    Case 1
                        oDomandeMedie.domande.Add(oDomanda)
                    Case 2
                        oDomandeDifficili.domande.Add(oDomanda)
                End Select
            Next
            oNewLibreriaDomande.pagine.Add(oDomandeFacili)
            oNewLibreriaDomande.pagine.Add(oDomandeMedie)
            oNewLibreriaDomande.pagine.Add(oDomandeDifficili)
            librerie.Add(oNewLibreriaDomande)
        Next


        For Each oUtenteInvitato As UtenteInvitato In UtentiInvitatiList
            Dim NewLibrerie As New list(Of Questionario)
            Dim oQuest As New Questionario
            Dim counter As Integer

            oQuest = Me.QuestionarioCorrente
            oQuest.domande.Clear()
            oQuest.pagine.Clear()

            oQuest.tipo = Me.QuestionarioCorrente.TipoQuestionario.Questionario
            oQuest.idDestinatario_Persona = COL_Persona.GetUtenteAnonimo(Me.UtenteCorrente.Lingua).ID
            oQuest.idDestinatario_UtenteInvitato = oUtenteInvitato.ID

            Randomize()
            Dim indexLib As Integer
            Dim indexDom As Integer

            For indexLib = 0 To librerie.Count - 1
                For Each oLibreria As Questionario In librerie
                    Dim oNewLibreriaDomande As New Questionario
                    For Each oPagina As QuestionarioPagina In oLibreria.pagine
                        Dim oPaginaNew As New QuestionarioPagina
                        For Each oDom As Domanda In oPagina.domande
                            oPaginaNew.domande.Add(oDom)
                        Next
                        oNewLibreriaDomande.pagine.Add(oPaginaNew)
                    Next
                    NewLibrerie.Add(oNewLibreriaDomande)
                Next

                For counter = 1 To Math.Min(librerie(indexLib).pagine(0).domande.Count, Me.QuestionarioCorrente.librerieQuestionario(indexLib).nDomandeDiffBassa)
                    indexDom = Math.Round(Rnd() * (NewLibrerie(indexLib).pagine(0).domande.Count - 1))
                    oQuest.domande.Add(NewLibrerie(indexLib).pagine(0).domande(indexDom))
                    NewLibrerie(indexLib).pagine(0).domande.RemoveAt(indexDom)
                Next
                For counter = 1 To Math.Min(librerie(indexLib).pagine(1).domande.Count, Me.QuestionarioCorrente.librerieQuestionario(indexLib).nDomandeDiffMedia)
                    indexDom = Math.Round(Rnd() * (NewLibrerie(indexLib).pagine(1).domande.Count - 1))
                    oQuest.domande.Add(NewLibrerie(indexLib).pagine(1).domande(indexDom))
                    NewLibrerie(indexLib).pagine(1).domande.RemoveAt(indexDom)
                Next
                For counter = 1 To Math.Min(librerie(indexLib).pagine(2).domande.Count, Me.QuestionarioCorrente.librerieQuestionario(indexLib).nDomandeDiffAlta)
                    indexDom = Math.Round(Rnd() * (NewLibrerie(indexLib).pagine(2).domande.Count - 1))
                    oQuest.domande.Add(NewLibrerie(indexLib).pagine(2).domande(indexDom))
                    NewLibrerie(indexLib).pagine(2).domande.RemoveAt(indexDom)
                Next
            Next
            oQuest.domande = DALDomande.randomizeCollectionOfQuestions(oQuest.domande)
            oQuest.idFiglio = DALQuestionario.InsertRandomDestinatario(oQuest)
            For indexDom = 0 To Me.QuestionarioCorrente.domande.Count - 1
                oQuest.domande(indexDom).numero = indexDom + 1
                DALDomande.connectQuestionToSurvey(oQuest.domande(indexDom), oQuest.id, oQuest.idFiglio)
            Next
            'Dim indexPage As Integer
            'For indexPage = 0 To Math.Floor(oQuest.domande.Count / oQuest.nQuestionsPerPage)
            '    Dim oPageSurvey As New QuestionarioPagina
            '    Dim indexQuestion As Integer
            '    oPageSurvey.dallaDomanda = indexPage * oQuest.nQuestionsPerPage + 1
            '    oPageSurvey.allaDomanda = Math.Min((indexPage + 1) * oQuest.nQuestionsPerPage, oQuest.domande.Count)
            '    oPageSurvey.idQuestionarioMultilingua = oQuest.idQuestionarioMultilingua
            '    oPageSurvey.numeroDomande = oQuest.nQuestionsPerPage
            '    oPageSurvey.numeroPagina = indexPage + 1
            '    Dim remainingQuestions As Int16
            '    remainingQuestions = Math.Min(oQuest.nQuestionsPerPage - 1, oQuest.domande.Count - (indexPage * oQuest.nQuestionsPerPage + 1))
            '    For indexQuestion = 0 To remainingQuestions
            '        oPageSurvey.domande.Add(oQuest.domande(indexQuestion + indexPage * oQuest.nQuestionsPerPage))
            '    Next
            '    oQuest.pagine.Add(oPageSurvey)
            'Next
            questionari.Add(oQuest)
        Next

        Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Random

        Return questionari
    End Function

    Public Sub generaQuestionarioRandomDestinatario(ByVal isPreview As Boolean, ByVal type As QuestionnaireType) ' isPreview mi dice se sto randomizzando il questionario per visualizzarlo nell'anteprima (in questo caso non salvo fisicamente il questionario Random su DB)

        'Me.QuestionarioCorrente.idPadre = Me.QuestionarioCorrente.id
        'Me.QuestionarioCorrente.id = 0
        Me.QuestionarioCorrente.tipo = Me.QuestionarioCorrente.TipoQuestionario.Questionario
        Me.QuestionarioCorrente.idDestinatario_Persona = Me.UtenteCorrente.ID
        Me.QuestionarioCorrente.idDestinatario_UtenteInvitato = Me.Invito.ID
        Dim index As Integer

        For Each oLibreria As LibreriaQuestionario In Me.QuestionarioCorrente.librerieQuestionario
            'Dim oLibreriaDomande As New Questionario
            Dim oLibreriaDomande As New Questionario
            Dim counter As Integer

            oLibreriaDomande = DALQuestionario.readQuestionarioBYLingua(Me.PageUtility.CurrentContext, oLibreria.idLibreria, Me.LinguaID, False)
            Dim oDomandeFacili As New QuestionarioPagina
            Dim oDomandeMedie As New QuestionarioPagina
            Dim oDomandeDifficili As New QuestionarioPagina
            For Each oDomanda As Domanda In oLibreriaDomande.domande
                Select Case oDomanda.difficolta
                    Case 0
                        oDomandeFacili.domande.Add(oDomanda)
                    Case 1
                        oDomandeMedie.domande.Add(oDomanda)
                    Case 2
                        oDomandeDifficili.domande.Add(oDomanda)
                End Select
            Next
            oLibreriaDomande.pagine.Clear()
            oLibreriaDomande.pagine.Add(oDomandeFacili)
            oLibreriaDomande.pagine.Add(oDomandeMedie)
            oLibreriaDomande.pagine.Add(oDomandeDifficili)

            'oNewLibreriaDomande = oLibreriaDomande
            Randomize()
            Try
                For counter = 1 To oLibreria.nDomandeDiffBassa
                    index = Math.Round(Rnd() * (oLibreriaDomande.pagine(0).domande.Count - 1))
                    Me.QuestionarioCorrente.domande.Add(oLibreriaDomande.pagine(0).domande(index))
                    oLibreriaDomande.pagine(0).domande.RemoveAt(index)
                Next
                For counter = 1 To oLibreria.nDomandeDiffMedia
                    index = Math.Round(Rnd() * (oLibreriaDomande.pagine(1).domande.Count - 1))
                    Me.QuestionarioCorrente.domande.Add(oLibreriaDomande.pagine(1).domande(index))
                    oLibreriaDomande.pagine(1).domande.RemoveAt(index)
                Next
                For counter = 1 To oLibreria.nDomandeDiffAlta
                    index = Math.Round(Rnd() * (oLibreriaDomande.pagine(2).domande.Count - 1))
                    Me.QuestionarioCorrente.domande.Add(oLibreriaDomande.pagine(2).domande(index))
                    oLibreriaDomande.pagine(2).domande.RemoveAt(index)
                Next
            Catch ex As Exception
                'se si arriva qui probabilmente sono state cancellate domande da una libreria 
                'dopo che era stata fatta l'associazione tra il numero di domande per il questionario random e quelle disponibili nella libreria stessa
            End Try

        Next
        Me.QuestionarioCorrente.domande = DALDomande.randomizeCollectionOfQuestions(Me.QuestionarioCorrente.domande)
        If Not isPreview Then
            Me.QuestionarioCorrente.idFiglio = DALQuestionario.InsertRandomDestinatario(Me.QuestionarioCorrente)
        Else
            Me.QuestionarioCorrente.idFiglio = 0
        End If
        For index = 0 To Me.QuestionarioCorrente.domande.Count - 1
            Me.QuestionarioCorrente.domande(index).numero = index + 1
            If Not isPreview Then
                DALDomande.connectQuestionToSurvey(Me.QuestionarioCorrente.domande(index), Me.QuestionarioCorrente.id, Me.QuestionarioCorrente.idFiglio)
            End If
        Next

        Dim indexPage As Integer
        For indexPage = 0 To Math.Floor((Me.QuestionarioCorrente.domande.Count - 1) / Me.QuestionarioCorrente.nQuestionsPerPage)
            Dim oPageSurvey As New QuestionarioPagina
            Dim indexQuestion As Integer

            oPageSurvey.dallaDomanda = indexPage * Me.QuestionarioCorrente.nQuestionsPerPage + 1
            oPageSurvey.allaDomanda = Math.Min((indexPage + 1) * Me.QuestionarioCorrente.nQuestionsPerPage, Me.QuestionarioCorrente.domande.Count)
            oPageSurvey.idQuestionarioMultilingua = Me.QuestionarioCorrente.idQuestionarioMultilingua
            oPageSurvey.numeroDomande = Me.QuestionarioCorrente.nQuestionsPerPage
            oPageSurvey.numeroPagina = indexPage + 1
            Dim remainingQuestions As Int16
            remainingQuestions = Math.Min(Me.QuestionarioCorrente.nQuestionsPerPage - 1, Me.QuestionarioCorrente.domande.Count - (indexPage * Me.QuestionarioCorrente.nQuestionsPerPage + 1))
            For indexQuestion = 0 To remainingQuestions
                oPageSurvey.domande.Add(Me.QuestionarioCorrente.domande(indexQuestion + indexPage * Me.QuestionarioCorrente.nQuestionsPerPage))
            Next
            Me.QuestionarioCorrente.pagine.Add(oPageSurvey)
        Next
        Me.QuestionarioCorrente.tipo = type 'Questionario.TipoQuestionario.Random
    End Sub

    Public Sub readDomandeQuestionarioRandom()

        Dim oPag As New QuestionarioPagina
        Me.QuestionarioCorrente.pagine.Add(oPag)

        'DALDomande.readDomandeByQuestionario(Me.QuestionarioCorrente)
        For Each oLibQuest As LibreriaQuestionario In Me.QuestionarioCorrente.librerieQuestionario
            Dim oLibreria As New Questionario
            oLibreria = DALQuestionario.readQuestionarioBYLingua(Me.PageUtility.CurrentContext, oLibQuest.idLibreria, 0, False, True)
            For Each oDom As Domanda In oLibreria.domande
                Me.QuestionarioCorrente.pagine(0).domande.Add(oDom)
            Next
        Next

        DALQuestionario.readDomandeOpzioniRisposte(Me.QuestionarioCorrente)

    End Sub

    Public Function domandaSelect(ByRef idCollection As list(Of Integer), ByVal idLingua As Int16) As Domanda
        Dim oDomanda As New Domanda
        Dim index As Integer
        Dim idDomanda As Integer

        Randomize()
        index = Math.Round(Rnd() * (idCollection.Count - 1))
        idDomanda = idCollection(index)
        idCollection.RemoveAt(index)

        oDomanda = DALDomande.readDomandaById(idDomanda, idLingua, Me.QuestionarioCorrente.id)

        Return oDomanda
    End Function


    Public Sub bindFieldQuestionario(ByVal FRVQuestionario As FormView, ByRef PNLDestinatari As Panel, ByVal oQuest As Questionario, ByVal idGruppo As Integer)

        Dim listDom As New list(Of Questionario)
        Try
            listDom.Add(oQuest)
            FRVQuestionario.DataSource = listDom
            FRVQuestionario.DataBind()
        Catch ex As Exception
            Dim aa As String = ex.Message
        End Try
        If oQuest.id = 0 Then
            Dim ddllingua As New DropDownList
            ddllingua = DirectCast(FRVQuestionario.FindControl("DDLLingua"), DropDownList)
            If Not ddllingua Is Nothing Then
                ddllingua.DataSource = DALQuestionario.readLingue(0)
                ddllingua.DataBind()
            End If
            DirectCast(PNLDestinatari.FindControl("LBTitoloUrl"), Label).Visible = False
            DirectCast(PNLDestinatari.FindControl("HYPUrl"), HyperLink).Visible = False
            If Not DirectCast(FRVQuestionario.FindControl("LBLingua"), Label) Is Nothing Then
                DirectCast(FRVQuestionario.FindControl("LBLingua"), Label).Visible = False
            End If
        Else
            Dim ddllingua As New DropDownList
            ddllingua = DirectCast(FRVQuestionario.FindControl("DDLLingua"), DropDownList)
            If Not ddllingua Is Nothing Then
                ddllingua.DataSource = Me.QuestionarioCorrente.linguePresenti
                ddllingua.DataBind()
                ddllingua.SelectedValue = Me.QuestionarioCorrente.idLingua
            End If
            If Me.QuestionarioCorrente.idLingua > 0 Then
                Dim lbLingua As New Label
                lbLingua = DirectCast(FRVQuestionario.FindControl("LBLingua"), Label)
                If Not lbLingua Is Nothing Then
                    lbLingua.Text = String.Format(lbLingua.Text, ManagerLingua.FindByIDfromList(Me.QuestionarioCorrente.linguePresenti, Me.QuestionarioCorrente.idLingua).Nome)
                End If
            End If
        End If

    End Sub
    Private Sub checkGruppoCorrente()
        If Not Me.GruppoCorrente.idComunita = Me.ComunitaCorrenteID Then
            Me.GruppoCorrente = DALQuestionarioGruppo.GruppoPrincipaleByComunita(Me.ComunitaCorrenteID)
            If Me.GruppoCorrente.id = 0 Then
                creaGruppoDefault(Me.ComunitaCorrenteID)
            End If
        End If
    End Sub
    Private Function getSafeGroupId() As Integer
        checkGruppoCorrente()
        Return GruppoCorrente.id
    End Function
    Public Function salvaQuestionario(ByVal FRVQuestionario As FormView, ByVal oQuest As Questionario) As Boolean
        Dim saveOK As Boolean
        oQuest.nome = DirectCast(FRVQuestionario.FindControl("TXBnome"), TextBox).Text
        oQuest.descrizione = RootObject.removeBRfromStringEnd(DirectCast(FRVQuestionario.FindControl("CTRLeditorDescrizioneQuestionario"), Comunita_OnLine.UC_Editor).HTML)

        'If Not oQuest.isChiuso Then 'se il questionario e' chiuso si aggiorna solo il nome
        oQuest.dataInizio = DirectCast(FRVQuestionario.FindControl("RDPDataInizio"), Telerik.WebControls.RadDatePicker).DateInput.SelectedDate.AddHours(DirectCast(FRVQuestionario.FindControl("DDLOraInizio"), DropDownList).SelectedValue).AddMinutes(DirectCast(FRVQuestionario.FindControl("DDLMinutiInizio"), DropDownList).SelectedValue)
        oQuest.dataFine = DirectCast(FRVQuestionario.FindControl("RDPDataFine"), Telerik.WebControls.RadDatePicker).DateInput.SelectedDate.AddHours(DirectCast(FRVQuestionario.FindControl("DDLOraFine"), DropDownList).SelectedValue).AddMinutes(DirectCast(FRVQuestionario.FindControl("DDLMinutiFine"), DropDownList).SelectedValue)
        'oQuest.tipo = COL_Questionario.Questionario.TipoQuestionario.Questionario
        If oQuest.id = 0 Then
            If Not DirectCast(FRVQuestionario.FindControl("DDLLingua"), DropDownList) Is Nothing Then
                oQuest.idLingua = DirectCast(FRVQuestionario.FindControl("DDLLingua"), DropDownList).SelectedValue
            Else
                oQuest.idLingua = LinguaID
            End If
        End If

        oQuest.isReadOnly = DirectCast(FRVQuestionario.FindControl("CKisChiuso"), CheckBox).Checked
        oQuest.risultatiAnonimi = DirectCast(FRVQuestionario.FindControl("CBXanonymousResults"), CheckBox).Checked
        If IsNumeric(DirectCast(FRVQuestionario.FindControl("TBDurata"), TextBox).Text) Then
            oQuest.durata = DirectCast(FRVQuestionario.FindControl("TBDurata"), TextBox).Text
        Else
            oQuest.durata = 0
        End If
        oQuest.visualizzaRisposta = DirectCast(FRVQuestionario.FindControl("CHKvisualizzaRisposta"), CheckBox).Checked
        oQuest.visualizzaCorrezione = DirectCast(FRVQuestionario.FindControl("CHKvisualizzaCorrezione"), CheckBox).Checked
        oQuest.visualizzaSuggerimenti = DirectCast(FRVQuestionario.FindControl("CHKvisualizzaSuggerimenti"), CheckBox).Checked
        oQuest.editaRisposta = DirectCast(FRVQuestionario.FindControl("CHKeditaRisposta"), CheckBox).Checked
        oQuest.isBloccato = DirectCast(FRVQuestionario.FindControl("CKisBloccato"), CheckBox).Checked
        oQuest.isRandomOrder = DirectCast(FRVQuestionario.FindControl("CHKOrdineDomandeRandom"), CheckBox).Checked

        Dim tValue As String = DirectCast(FRVQuestionario.FindControl("TXBScalaValutazione"), TextBox).Text
        If String.IsNullOrEmpty(tValue) OrElse Not IsNumeric(tValue) Then
            oQuest.scalaValutazione = RootObject.scalaValutazione
        Else
            oQuest.scalaValutazione = CInt(tValue)
        End If

        Dim dScore As CheckBox = DirectCast(FRVQuestionario.FindControl("CBXdisplayScoreToUser"), CheckBox)
        If Not IsNothing(dScore) Then
            oQuest.DisplayScoreToUser = dScore.Checked
        Else
            oQuest.DisplayScoreToUser = (oQuest.tipo = QuestionnaireType.Random OrElse oQuest.tipo = QuestionnaireType.RandomMultipleAttempts OrElse oQuest.tipo = QuestionnaireType.Standard)
        End If

        Dim dAttemptScore As CheckBox = DirectCast(FRVQuestionario.FindControl("CBXdisplayAttemptScoreToUser"), CheckBox)
        If Not IsNothing(dScore) Then
            oQuest.DisplayAttemptScoreToUser = dAttemptScore.Checked
        Else
            oQuest.DisplayAttemptScoreToUser = (oQuest.tipo = QuestionnaireType.Random OrElse oQuest.tipo = QuestionnaireType.RandomMultipleAttempts OrElse oQuest.tipo = QuestionnaireType.Standard)
        End If

        Dim dAvailableAttempts As CheckBox = DirectCast(FRVQuestionario.FindControl("CBXdisplayAvailableAttemptsToUser"), CheckBox)
        If Not IsNothing(dAvailableAttempts) Then
            oQuest.DisplayAvailableAttempts = dAvailableAttempts.Checked
        Else
            oQuest.DisplayAvailableAttempts = (oQuest.tipo = QuestionnaireType.RandomMultipleAttempts)
        End If
       oQuest.DisplayResultsStatus = (oQuest.tipo = QuestionnaireType.RandomMultipleAttempts)

        Dim dCurrentAttempts As CheckBox = DirectCast(FRVQuestionario.FindControl("CBXdisplayCurrentAttempts"), CheckBox)
        If Not IsNothing(dCurrentAttempts) Then
            oQuest.DisplayCurrentAttempts = dCurrentAttempts.Checked
        Else
            oQuest.DisplayCurrentAttempts = (oQuest.tipo = QuestionnaireType.RandomMultipleAttempts)
        End If

            If oQuest.tipo = QuestionnaireType.RandomMultipleAttempts Then
                tValue = DirectCast(FRVQuestionario.FindControl("TXBminScore"), TextBox).Text
                If String.IsNullOrEmpty(tValue) OrElse Not IsNumeric(tValue) Then
                    oQuest.MinScore = oQuest.scalaValutazione
                Else
                    oQuest.MinScore = CInt(tValue)
                End If
                If oQuest.MinScore > oQuest.scalaValutazione Then
                    oQuest.MinScore = oQuest.scalaValutazione
                End If

                tValue = DirectCast(FRVQuestionario.FindControl("TXBmaxAttempts"), TextBox).Text
                If String.IsNullOrEmpty(tValue) OrElse Not IsNumeric(tValue) Then
                    oQuest.MaxAttempts = 0
                Else
                    oQuest.MaxAttempts = CInt(tValue)
                End If
                If oQuest.MaxAttempts < 0 Then
                    oQuest.MaxAttempts = 0
                End If
            End If

            oQuest.idGruppo = getSafeGroupId()

            If DirectCast(FRVQuestionario.FindControl("CUVdate"), System.Web.UI.WebControls.CustomValidator).IsValid And DirectCast(FRVQuestionario.FindControl("CUVNome"), System.Web.UI.WebControls.CustomValidator).IsValid Then
                If Not oQuest.nome.TrimStart(" ") Is String.Empty Then
                    Try
                        If oQuest.id = 0 Then
                            ' aggiungo l'azione di creazione del questionario
                            CreateActionAdd()
                        End If
                        Dim idQuestML As Integer = DALQuestionario.Salva(PageUtility.CurrentContext, oQuest)
                        If idQuestML > 0 Then
                            oQuest.idQuestionarioMultilingua = idQuestML
                        End If
                        Me.QuestionarioCorrente = oQuest
                        saveOK = True
                    Catch ex As Exception
                        saveOK = False
                    End Try
                End If
            Else
                DirectCast(FRVQuestionario.FindControl("RDPDataInizio"), Telerik.WebControls.RadDatePicker).SelectedDate = DateTime.Parse(oQuest.dataInizio).Date
                DirectCast(FRVQuestionario.FindControl("DDLOraInizio"), DropDownList).SelectedValue = DateTime.Parse(oQuest.dataInizio).Hour
                DirectCast(FRVQuestionario.FindControl("DDLMinutiInizio"), DropDownList).SelectedValue = DateTime.Parse(oQuest.dataInizio).Minute
                DirectCast(FRVQuestionario.FindControl("RDPDataFine"), Telerik.WebControls.RadDatePicker).SelectedDate = oQuest.dataFine
                DirectCast(FRVQuestionario.FindControl("DDLOraFine"), DropDownList).SelectedValue = DateTime.Parse(oQuest.dataFine).Hour
                DirectCast(FRVQuestionario.FindControl("DDLMinutiFine"), DropDownList).SelectedValue = DateTime.Parse(oQuest.dataFine).Minute
                saveOK = False
            End If

            Return saveOK
    End Function

    Public Function salvaQuestionario(ByVal oQuest) As Boolean
        Dim saveOK As Boolean

        Try
            If oQuest.id = 0 Then
                ' aggiungo l'azione di creazione del questionario
                CreateActionAdd()
            End If
            oQuest.idGruppo = getSafeGroupId()
            Dim idQuestML As Integer = DALQuestionario.Salva(PageUtility.CurrentContext, oQuest)
            If idQuestML > 0 Then
                oQuest.idQuestionarioMultilingua = idQuestML
            End If
            Me.QuestionarioCorrente = oQuest
            saveOK = True
        Catch ex As Exception
            saveOK = False
        End Try

        Return saveOK
    End Function


    Public Function IsDuplicatedName(idQuest As Integer, idtype As Integer, ByVal name As String) As Boolean
        Return DALQuestionario.GetDuplicatedItemsByName(Me.ComunitaCorrenteID, idQuest, idtype, name).Any
    End Function
    'Public Function controllaNome(ByVal nome As String) As Boolean
    '    Dim QuantiNomi As Integer
    '    QuantiNomi = contaNome(nome)
    '    If Not Me.QuestionarioCorrente Is Nothing Then
    '        If Me.QuestionarioCorrente.nome = nome Then
    '            QuantiNomi = QuantiNomi - 1
    '        End If
    '    End If
    '    If Not QuantiNomi < 1 Then
    '        Return False
    '    End If
    '    Return True
    'End Function

    'Public Function contaNome(ByVal nome As String) As Integer
    '    Dim QuantiNomi As Integer
    '    QuantiNomi = DALQuestionario.controllaNome(Me.ComunitaCorrenteID, nome)
    '    Return QuantiNomi
    'End Function

    Public Sub bindFieldLibreriaDomande(ByVal FRVQuestionario As FormView, ByVal oQuest As Questionario)

        Dim listDom As New list(Of Questionario)
        listDom.Add(oQuest)
        FRVQuestionario.DataSource = listDom
        FRVQuestionario.DataBind()

    End Sub


    Public Function copiaQuestionarioCorrente(Optional ByRef isBloccato As Boolean = True)
        'esegue la copia di me.questionariocorrente

        Dim copiePresenti As Integer = 1
        Dim quanteLingue = Me.QuestionarioCorrente.linguePresenti.Count - 1
        Dim SalvaEnabled As Boolean = False
        Dim indexLingua As Integer
        Dim questOrigineId As Integer
        Dim questDestinazioneId As Integer
        Dim questNome As String
        Dim lingue As New List(Of Lingua)
        lingue = Me.QuestionarioCorrente.linguePresenti

        Dim idQuestType As Integer = Me.QuestionarioCorrente.tipo
        questOrigineId = Me.QuestionarioCorrente.id
        questNome = ""

        For indexLingua = 0 To quanteLingue
            If indexLingua = 0 Then
                Dim idCommunity As Integer = DALQuestionarioGruppo.ComunitaByGruppo(Me.QuestionarioCorrente.idGruppo)
                'la lingua di default e' gia' caricata, si evita caricamento da DB, si imposta il nome che rimane anche per le altre lingue
                If Not DALQuestionario.IsDuplicatedName(idCommunity, questOrigineId, idQuestType, "Copia di " & Me.QuestionarioCorrente.nome) Then
                    questNome = "Copia di " & Me.QuestionarioCorrente.nome
                    Me.QuestionarioCorrente.nome = questNome
                Else
                    Do
                        copiePresenti = copiePresenti + 1
                    Loop While DALQuestionario.IsDuplicatedName(idCommunity, questOrigineId, idQuestType, "Copia (" & copiePresenti & ") di " & Me.QuestionarioCorrente.nome)
                    'Loop While Not DALQuestionario.controllaNome(DALQuestionarioGruppo.ComunitaByGruppo(Me.QuestionarioCorrente.idGruppo), "Copia (" & copiePresenti & ") di " & Me.QuestionarioCorrente.nome) = 0
                    Me.QuestionarioCorrente.nome = "Copia (" & copiePresenti & ") di " & Me.QuestionarioCorrente.nome
                    questNome = Me.QuestionarioCorrente.nome
                End If

                'se nome e data sono corretti abilita il salvataggio
                If Not Me.QuestionarioCorrente.nome.TrimStart(" ") Is String.Empty Then
                    Me.QuestionarioCorrente.id = 0
                    Me.QuestionarioCorrente.isBloccato = isBloccato
                    ' se il questionario ha una sola lingua attivo l'editing delle domande, altrimenti rimarrà chiuso
                    If Me.QuestionarioCorrente.linguePresenti.Count = 1 Then
                        Me.QuestionarioCorrente.isReadOnly = False
                    End If
                    Me.QuestionarioCorrente.idQuestionarioMultilingua = DALQuestionario.Salva(PageUtility.CurrentContext, Me.QuestionarioCorrente)
                    questDestinazioneId = Me.QuestionarioCorrente.id
                    For Each oPagina As QuestionarioPagina In Me.QuestionarioCorrente.pagine
                        oPagina.id = 0
                        oPagina.idQuestionarioMultilingua = Me.QuestionarioCorrente.idQuestionarioMultilingua
                        DALPagine.Pagina_Insert(oPagina)
                        For Each oDomanda As Domanda In oPagina.domande
                            oDomanda.id = 0
                            oDomanda.idQuestionario = Me.QuestionarioCorrente.id
                            DALDomande.Salva(oDomanda, Me.QuestionarioCorrente.isReadOnly, Me.QuestionarioCorrente.tipo = COL_Questionario.Questionario.TipoQuestionario.LibreriaDiDomande)
                        Next
                    Next
                    If Me.QuestionarioCorrente.librerieQuestionario.Any Then
                        For Each libraryItem As LibreriaQuestionario In Me.QuestionarioCorrente.librerieQuestionario
                            libraryItem.id = 0
                            libraryItem.idQuestionario = Me.QuestionarioCorrente.id
                        Next
                        DALQuestionario.QuestionarioLibreria_Insert(Me.QuestionarioCorrente.librerieQuestionario)
                        Me.QuestionarioCorrente.librerieQuestionario = DALQuestionario.readQuestionarioLibrerie(Me.QuestionarioCorrente)
                    End If
                  


                    If DALQuestionarioGruppo.ComunitaByGruppo(Me.QuestionarioCorrente.idGruppo) = Me.ComunitaCorrenteID Then 'non deve cambiare idGruppo quando passa da una comunita' all'altra
                        Me.GruppoQuestionariID = Me.QuestionarioCorrente.idGruppo
                    End If
                    SalvaEnabled = True
                End If

            ElseIf SalvaEnabled Then 'se data e nome sono corretti salva le altre lingue
                Me.QuestionarioCorrente = DALQuestionario.readQuestionarioBYLingua(Me.PageUtility.CurrentContext, questOrigineId, lingue(indexLingua).ID, False)
                Me.QuestionarioCorrente.nome = questNome & " (" & lingue.Item(indexLingua).Nome & ")"
                'Me.QuestionarioCorrente.url = ""

                Me.QuestionarioCorrente.id = questDestinazioneId
                Me.QuestionarioCorrente.idQuestionarioMultilingua = DALQuestionario.QuestionarioMultiLingua_Insert(Me.QuestionarioCorrente)
                For Each oPagina As QuestionarioPagina In Me.QuestionarioCorrente.pagine
                    oPagina.id = 0
                    oPagina.idQuestionarioMultilingua = Me.QuestionarioCorrente.idQuestionarioMultilingua
                    DALPagine.Pagina_Insert(oPagina)
                    For Each oDomanda As Domanda In oPagina.domande
                        oDomanda.id = 0
                        oDomanda.idQuestionario = Me.QuestionarioCorrente.id
                        DALDomande.Salva(oDomanda, Me.QuestionarioCorrente.isReadOnly, Me.QuestionarioCorrente.tipo = COL_Questionario.Questionario.TipoQuestionario.LibreriaDiDomande)
                    Next
                Next
                If Me.QuestionarioCorrente.librerieQuestionario.Any Then
                    For Each libraryItem As LibreriaQuestionario In Me.QuestionarioCorrente.librerieQuestionario
                        libraryItem.id = 0
                        libraryItem.idQuestionario = Me.QuestionarioCorrente.id
                    Next
                    DALQuestionario.QuestionarioLibreria_Insert(Me.QuestionarioCorrente.librerieQuestionario)
                    Me.QuestionarioCorrente.librerieQuestionario = DALQuestionario.readQuestionarioLibrerie(Me.QuestionarioCorrente)
                End If

                If DALQuestionarioGruppo.ComunitaByGruppo(Me.QuestionarioCorrente.idGruppo) = Me.ComunitaCorrenteID Then 'non deve cambiare idGruppo quando passa da una comunita' all'altra
                    Me.GruppoQuestionariID = Me.QuestionarioCorrente.idGruppo
                End If
                'oQuest.isChiuso = False
                End If

        Next
        'Me.QuestionarioCorrente = New Questionario
        If Me.ComunitaCorrenteID = DALQuestionarioGruppo.ComunitaByGruppo(Me.QuestionarioCorrente.idGruppo) Then 'se e' stato copiato in altra comunita deve dare messaggio di conferma
            'Server.Transfer(RootObject.QuestionariGestioneList)
            Return RootObject.QuestionariGestioneList + "&" & qs_questType + Me.QuestionarioCorrente.tipo.ToString()
        End If

        Return String.Empty
    End Function

    Public Function copiaQuestionario(ByVal FRVQuestionario As FormView) As String
        'esegue la copia del questionario prendendo i dati da un FormView (nome e simili potrebbero esser stati modificati)
        Dim copiePresenti As Integer = 1
        Dim quanteLingue = Me.QuestionarioCorrente.linguePresenti.Count - 1
        Dim SalvaEnabled As Boolean = False
        Dim indexLingua As Integer
        Dim questOrigineId As Integer
        Dim questDestinazioneId As Integer
        Dim questNome As String
        Dim lingue As New List(Of Lingua)
        lingue = Me.QuestionarioCorrente.linguePresenti
        Dim idQuestType As Integer = Me.QuestionarioCorrente.id
        questOrigineId = Me.QuestionarioCorrente.id
        questNome = ""

        Dim idCommunity = DALQuestionarioGruppo.ComunitaByGruppo(Me.QuestionarioCorrente.idGruppo)
        For indexLingua = 0 To quanteLingue
            If indexLingua = 0 Then
                'la lingua di default e' gia' caricata, si evita caricamento da DB, si imposta il nome che rimane anche per le altre lingue
                'If DALQuestionario.controllaNome(DALQuestionarioGruppo.ComunitaByGruppo(Me.QuestionarioCorrente.idGruppo), "Copia di " & Me.QuestionarioCorrente.nome) = 0 Then
                If Not DALQuestionario.IsDuplicatedName(idCommunity, questOrigineId, idQuestType, "Copia di " & Me.QuestionarioCorrente.nome) Then
                    questNome = "Copia di " & Me.QuestionarioCorrente.nome
                    Me.QuestionarioCorrente.nome = questNome
                Else
                    Do
                        copiePresenti = copiePresenti + 1
                        ' Loop While Not DALQuestionario.controllaNome(DALQuestionarioGruppo.ComunitaByGruppo(Me.QuestionarioCorrente.idGruppo), "Copia (" & copiePresenti & ") di " & Me.QuestionarioCorrente.nome) = 0
                    Loop While DALQuestionario.IsDuplicatedName(idCommunity, questOrigineId, idQuestType, "Copia (" & copiePresenti & ") di " & Me.QuestionarioCorrente.nome)
                    Me.QuestionarioCorrente.nome = "Copia (" & copiePresenti & ") di " & Me.QuestionarioCorrente.nome
                    questNome = Me.QuestionarioCorrente.nome
                End If
                Me.QuestionarioCorrente.descrizione = DirectCast(FRVQuestionario.FindControl("CTRLeditorDescrizioneQuestionario"), Comunita_OnLine.UC_Editor).HTML
                Me.QuestionarioCorrente.dataInizio = DirectCast(FRVQuestionario.FindControl("RDPDataInizio"), Telerik.WebControls.RadDatePicker).DateInput.SelectedDate.AddHours(DirectCast(FRVQuestionario.FindControl("DDLOraInizio"), DropDownList).SelectedValue).AddMinutes(DirectCast(FRVQuestionario.FindControl("DDLMinutiInizio"), DropDownList).SelectedValue)
                Me.QuestionarioCorrente.dataFine = DirectCast(FRVQuestionario.FindControl("RDPDataFine"), Telerik.WebControls.RadDatePicker).DateInput.SelectedDate.AddHours(DirectCast(FRVQuestionario.FindControl("DDLOraFine"), DropDownList).SelectedValue).AddMinutes(DirectCast(FRVQuestionario.FindControl("DDLMinutiFine"), DropDownList).SelectedValue)
                Me.QuestionarioCorrente.isBloccato = DirectCast(FRVQuestionario.FindControl("CKisBloccato"), CheckBox).Checked
                Me.QuestionarioCorrente.durata = DirectCast(FRVQuestionario.FindControl("TBDurata"), TextBox).Text
                'Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Questionario 'quando si usera' verificare che serva settarlo qui

                'se nome e data sono corretti abilita il salvataggio
                If DirectCast(FRVQuestionario.FindControl("CUVdate"), System.Web.UI.WebControls.CustomValidator).IsValid And DirectCast(FRVQuestionario.FindControl("CUVNome"), System.Web.UI.WebControls.CustomValidator).IsValid Then
                    If Not Me.QuestionarioCorrente.nome.TrimStart(" ") Is String.Empty Then
                        Me.QuestionarioCorrente.id = 0
                        Me.QuestionarioCorrente.idQuestionarioMultilingua = DALQuestionario.Salva(PageUtility.CurrentContext, Me.QuestionarioCorrente)
                        questDestinazioneId = Me.QuestionarioCorrente.id
                        For Each oPagina As QuestionarioPagina In Me.QuestionarioCorrente.pagine
                            oPagina.id = 0
                            oPagina.idQuestionarioMultilingua = Me.QuestionarioCorrente.idQuestionarioMultilingua
                            DALPagine.Pagina_Insert(oPagina)
                            For Each oDomanda As Domanda In oPagina.domande
                                oDomanda.id = 0
                                'oDomanda.idQuestionarioMultilingua = Me.QuestionarioCorrente.idQuestionarioMultilingua
                                oDomanda.idQuestionario = Me.QuestionarioCorrente.id
                                DALDomande.Salva(oDomanda, Me.QuestionarioCorrente.isReadOnly, Me.QuestionarioCorrente.tipo = COL_Questionario.Questionario.TipoQuestionario.LibreriaDiDomande)
                            Next
                        Next
                        If DALQuestionarioGruppo.ComunitaByGruppo(Me.QuestionarioCorrente.idGruppo) = Me.ComunitaCorrenteID Then 'non deve cambiare idGruppo quando passa da una comunita' all'altra
                            Me.GruppoQuestionariID = Me.QuestionarioCorrente.idGruppo
                        End If
                        SalvaEnabled = True
                    End If
                Else
                    DirectCast(FRVQuestionario.FindControl("RDPDataInizio"), Telerik.WebControls.RadDatePicker).SelectedDate = DateTime.Parse(Me.QuestionarioCorrente.dataInizio).Date
                    DirectCast(FRVQuestionario.FindControl("DDLOraInizio"), DropDownList).SelectedValue = DateTime.Parse(Me.QuestionarioCorrente.dataInizio).Hour
                    DirectCast(FRVQuestionario.FindControl("DDLMinutiInizio"), DropDownList).SelectedValue = DateTime.Parse(Me.QuestionarioCorrente.dataInizio).Minute
                    DirectCast(FRVQuestionario.FindControl("RDPDataFine"), Telerik.WebControls.RadDatePicker).SelectedDate = Me.QuestionarioCorrente.dataFine
                    DirectCast(FRVQuestionario.FindControl("DDLOraFine"), DropDownList).SelectedValue = DateTime.Parse(Me.QuestionarioCorrente.dataFine).Hour
                    DirectCast(FRVQuestionario.FindControl("DDLMinutiFine"), DropDownList).SelectedValue = DateTime.Parse(Me.QuestionarioCorrente.dataFine).Minute
                End If
            ElseIf SalvaEnabled Then 'se data e nome sono corretti salva le altre lingue
                Me.QuestionarioCorrente = DALQuestionario.readQuestionarioBYLingua(Me.PageUtility.CurrentContext, questOrigineId, lingue(indexLingua).ID, False)
                Me.QuestionarioCorrente.nome = questNome & " (" & lingue.Item(indexLingua).Nome & ")"
                'Me.QuestionarioCorrente.url = ""

                Me.QuestionarioCorrente.id = questDestinazioneId
                Me.QuestionarioCorrente.idQuestionarioMultilingua = DALQuestionario.QuestionarioMultiLingua_Insert(Me.QuestionarioCorrente)
                For Each oPagina As QuestionarioPagina In Me.QuestionarioCorrente.pagine
                    oPagina.id = 0
                    oPagina.idQuestionarioMultilingua = Me.QuestionarioCorrente.idQuestionarioMultilingua
                    DALPagine.Pagina_Insert(oPagina)
                    For Each oDomanda As Domanda In oPagina.domande
                        oDomanda.id = 0
                        'oDomanda.idQuestionarioMultilingua = Me.QuestionarioCorrente.idQuestionarioMultilingua
                        oDomanda.idQuestionario = Me.QuestionarioCorrente.id
                        DALDomande.Salva(oDomanda, Me.QuestionarioCorrente.isReadOnly, Me.QuestionarioCorrente.tipo = COL_Questionario.Questionario.TipoQuestionario.LibreriaDiDomande)
                    Next
                Next


                If DALQuestionarioGruppo.ComunitaByGruppo(Me.QuestionarioCorrente.idGruppo) = Me.ComunitaCorrenteID Then 'non deve cambiare idGruppo quando passa da una comunita' all'altra
                    Me.GruppoQuestionariID = Me.QuestionarioCorrente.idGruppo
                End If
                'oQuest.isChiuso = False
            End If

        Next
        If Me.QuestionarioCorrente.tipo = QuestionnaireType.QuestionLibrary Then
            Dim s As New COL_Questionario.Business.ServiceQuestionnaire(PageUtility.CurrentContext)
            s.CloneLibraryAvailability(questOrigineId, Me.QuestionarioCorrente.id)
        End If
        'Me.QuestionarioCorrente = New Questionario
        If Me.ComunitaCorrenteID = DALQuestionarioGruppo.ComunitaByGruppo(Me.QuestionarioCorrente.idGruppo) Then 'se e' stato copiato in altra comunita deve dare messaggio di conferma
            'Server.Transfer(RootObject.QuestionariGestioneList)
            Return RootObject.QuestionariGestioneList + "&" & qs_questType + Me.QuestionarioCorrente.tipo.ToString()
        End If

        Return String.Empty
    End Function
    ''' <summary>
    ''' Importa un questioanario in un altro module
    ''' </summary>
    ''' <returns>Id del nuovo questionario</returns>
    ''' <remarks></remarks>
    Public Function importaQuestionario(ByRef idQuestionario As Integer, ByRef owType As Integer, ByRef owID As Long) As Integer
        Me.QuestionarioCorrente = DALQuestionario.readQuestionarioBYLingua(Me.PageUtility.CurrentContext, idQuestionario, LinguaID, False)
        QuestionarioCorrente.ownerId = owID
        QuestionarioCorrente.ownerType = owType
        QuestionarioCorrente.dataInizio = ""
        QuestionarioCorrente.dataFine = ""

        copiaQuestionarioCorrente(False)
        Return Me.QuestionarioCorrente.id
    End Function

    Public Function copiaQuestionarioMultilingua(ByVal FRVQuestionario As FormView) As String

        If DirectCast(FRVQuestionario.FindControl("CUVdate"), System.Web.UI.WebControls.CustomValidator).IsValid Then
            If Not Me.QuestionarioCorrente.nome.TrimStart(" ") Is String.Empty Then
                Me.QuestionarioCorrente.isDefault = False
                Dim tipoQuest As String = Me.QuestionarioCorrente.tipo.ToString()
                Dim idQuest As Integer = DALQuestionario.QuestionarioMultiLingua_Insert(Me.QuestionarioCorrente)
                For Each oPagina As QuestionarioPagina In Me.QuestionarioCorrente.pagine
                    oPagina.id = 0
                    oPagina.idQuestionarioMultilingua = idQuest
                    DALPagine.Pagina_Insert(oPagina)
                    For Each oDomanda As Domanda In oPagina.domande
                        'oDomanda.idDomandaPadre = oDomanda.id
                        'oDomanda.id = 0
                        oDomanda.idLingua = Me.QuestionarioCorrente.idLingua
                        DALDomande.SalvaMultilingua(oDomanda, Me.QuestionarioCorrente.isReadOnly)
                    Next
                Next
                Me.QuestionarioCorrente = New Questionario
                If tipoQuest = Questionario.TipoQuestionario.Random Then
                    tipoQuest = Questionario.TipoQuestionario.Questionario
                End If
                Return RootObject.QuestionariGestioneList + "&" & qs_questType + tipoQuest
            End If
        Else
            DirectCast(FRVQuestionario.FindControl("RDPDataInizio"), Telerik.WebControls.RadDatePicker).SelectedDate = DateTime.Parse(Me.QuestionarioCorrente.dataInizio).Date
            DirectCast(FRVQuestionario.FindControl("DDLOraInizio"), DropDownList).SelectedValue = DateTime.Parse(Me.QuestionarioCorrente.dataInizio).Hour
            DirectCast(FRVQuestionario.FindControl("DDLMinutiInizio"), DropDownList).SelectedValue = DateTime.Parse(Me.QuestionarioCorrente.dataInizio).Minute
            DirectCast(FRVQuestionario.FindControl("RDPDataFine"), Telerik.WebControls.RadDatePicker).SelectedDate = Me.QuestionarioCorrente.dataFine
            DirectCast(FRVQuestionario.FindControl("DDLOraFine"), DropDownList).SelectedValue = DateTime.Parse(Me.QuestionarioCorrente.dataFine).Hour
            DirectCast(FRVQuestionario.FindControl("DDLMinutiFine"), DropDownList).SelectedValue = DateTime.Parse(Me.QuestionarioCorrente.dataFine).Minute
        End If

        Return String.Empty
    End Function

    Public Function setLibreriaDomande(ByVal FRVQuestionario As FormView, ByVal oQuest As Questionario) As Questionario

        oQuest.nome = DirectCast(FRVQuestionario.FindControl("TXBNome"), TextBox).Text
        oQuest.descrizione = DirectCast(FRVQuestionario.FindControl("TXBDescrizione"), TextBox).Text
        oQuest.tipo = Questionario.TipoQuestionario.LibreriaDiDomande
        Return oQuest
    End Function

    Public Sub creaGruppoDefault(ByVal idComunita As Integer)
        If Me.GruppoCorrente Is Nothing Then
            Dim oGruppo As New QuestionarioGruppo
            Me.GruppoCorrente = oGruppo
        End If
        Me.GruppoCorrente.idComunita = idComunita
        Me.GruppoCorrente.nome = COL_Questionario.RootObject.nomeGruppoDefault
        Me.GruppoCorrente.idGruppoPadre = 0
        Me.GruppoCorrente.id = DALQuestionarioGruppo.GruppoPrincipaleByComunita_Id(Me.ComunitaCorrenteID)
        'If Me.GruppoCorrente.id = 0 Then
        '    Me.GruppoCorrente.id = DALQuestionarioGruppo.InsertGruppo(Me.GruppoCorrente)
        'End If
        'Return idGruppo
    End Sub

    Public Function copiaModello() As String

        Me.QuestionarioCorrente.id = 0
        Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Questionario
        Me.QuestionarioCorrente.idGruppo = getSafeGroupId()
        If Not Me.GruppoCorrente.idComunita = Me.ComunitaCorrenteID Then
            Me.GruppoCorrente = DALQuestionarioGruppo.GruppoPrincipaleByComunita(Me.ComunitaCorrenteID)
            If Me.GruppoCorrente.id = 0 Then
                creaGruppoDefault(Me.ComunitaCorrenteID)
            End If
        End If
        Dim saveOK As Boolean
        Try
            Dim oGestione As New GestioneDomande
            Dim idQuestML As Integer = DALQuestionario.Salva(PageUtility.CurrentContext, Me.QuestionarioCorrente)
            If idQuestML > 0 Then
                Me.QuestionarioCorrente.idQuestionarioMultilingua = idQuestML
            End If
            For Each oPagina As QuestionarioPagina In Me.QuestionarioCorrente.pagine
                oPagina.id = 0
                oPagina.idQuestionarioMultilingua = Me.QuestionarioCorrente.idQuestionarioMultilingua
                DALPagine.Pagina_Insert(oPagina)
                For Each oDomanda As Domanda In oPagina.domande
                    oDomanda.id = 0
                    oDomanda.idQuestionario = Me.QuestionarioCorrente.id
                    DALDomande.Salva(oDomanda, Me.QuestionarioCorrente.isReadOnly, Me.QuestionarioCorrente.tipo = COL_Questionario.Questionario.TipoQuestionario.LibreriaDiDomande)
                Next
            Next
            saveOK = True
            Return RootObject.QuestionarioAdmin + "?type=0&IdQ=" & Me.QuestionarioCorrente.id & "&idLanguage=" & Me.QuestionarioCorrente.idLingua

        Catch ex As Exception
            saveOK = False
            Return String.Empty
        End Try
    End Function

    Public Sub setCampiRispostaQuestionario(ByVal isUI As Boolean)

        'Dim ipClient, ipProxy As String
        'MyBase.GetIp(ipClient, ipProxy)

        Me.QuestionarioCorrente.rispostaQuest.ultimaRisposta = 0
        Me.QuestionarioCorrente.rispostaQuest.idQuestionario = Me.QuestionarioCorrente.id
        If isUI Then
            Me.QuestionarioCorrente.rispostaQuest.idUtenteInvitato = Me.Invito.ID
        Else
            Me.QuestionarioCorrente.rispostaQuest.idUtenteInvitato = Me.Invito.ID
            Me.QuestionarioCorrente.rispostaQuest.idPersona = Me.UtenteCorrente.ID
        End If
        Me.QuestionarioCorrente.rispostaQuest.dataModifica = Now()
        Me.QuestionarioCorrente.rispostaQuest.indirizzoIPEdit = OLDpageUtility.ProxyIPadress() & " / " & OLDpageUtility.ClientIPadress
        Me.QuestionarioCorrente.rispostaQuest.idQuestionarioRandom = Me.QuestionarioCorrente.idFiglio

    End Sub

#Region "Action"
    Public Sub CreateActionAdd()
        Select Case Me.QuestionarioCorrente.tipo
            Case Questionario.TipoQuestionario.Sondaggio
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.CreateSondaggio, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Questionario
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.CreateQuestionario, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Meeting
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.CreateMeetingPoll, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.LibreriaDiDomande
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.CreateLibreria, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Autovalutazione
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.CreateTestAutovalutazione, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Else
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.CreateElse, , lm.ActionDataContract.InteractionType.UserWithUser)
        End Select
    End Sub
    Public Sub CompileStartActionAdd()
        Select Case Me.QuestionarioCorrente.tipo
            Case Questionario.TipoQuestionario.Questionario
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.CompileStartQuestionario, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Sondaggio
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.CompileStartSondaggio, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Meeting
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.CompileStartMeetingPoll, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Autovalutazione
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.CompileStartTestAutovalutazione, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Else
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.CompileStartElse, , lm.ActionDataContract.InteractionType.UserWithUser)
        End Select
    End Sub
    Public Sub CompileEndActionAdd()
        Select Case Me.QuestionarioCorrente.tipo
            Case Questionario.TipoQuestionario.Sondaggio
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.CompileEndSondaggio, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Questionario
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.CompileEndQuestionario, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Meeting
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.CompileEndMeetingPoll, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Autovalutazione
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.CompileEndTestAutovalutazione, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Else
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.CompileEndElse, , lm.ActionDataContract.InteractionType.UserWithUser)
        End Select
    End Sub
    Public Sub DeleteOneAnswerActionAdd()
        Select Case Me.QuestionarioCorrente.tipo
            Case Questionario.TipoQuestionario.Sondaggio
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.DeleteOneAnswerSondaggio, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Questionario
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.DeleteOneAnswerQuestionario, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Meeting
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.DeleteOneAnswerMeetingPoll, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Autovalutazione
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.DeleteOneAnswerTestAutovalutazione, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Else
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.DeleteOneAnswerElse, , lm.ActionDataContract.InteractionType.UserWithUser)
        End Select
    End Sub
    'Public Sub DeleteOneAnswerActionAdd(ByVal idQuestionnaire As Integer, idAnswer As Integer, idModule As Integer, idCommunity As Integer)
    '    Dim objects As New List(Of WS_Actions.ObjectAction)
    '    objects.Add(New WS_Actions.ObjectAction With {.ModuleID = idModule, .ValueID = idQuestionnaire, .ObjectTypeId = CInt(COL_Questionario.ModuleQuestionnaire.ObjectType.Questionario)})
    '    objects.Add(New WS_Actions.ObjectAction With {.ModuleID = idModule, .ValueID = idAnswer, .ObjectTypeId = CInt(COL_Questionario.ModuleQuestionnaire.ObjectType.QuestionnaireAnswer)})

    '    Select Case Me.QuestionarioCorrente.tipo
    '        Case Questionario.TipoQuestionario.Sondaggio
    '            Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.DeleteOneAnswerSondaggio, , lm.ActionDataContract.InteractionType.UserWithUser)
    '        Case Questionario.TipoQuestionario.Questionario
    '            Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.DeleteOneAnswerQuestionario, , lm.ActionDataContract.InteractionType.UserWithUser)
    '        Case Questionario.TipoQuestionario.Meeting
    '            Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.DeleteOneAnswerMeetingPoll, , lm.ActionDataContract.InteractionType.UserWithUser)
    '        Case Questionario.TipoQuestionario.Autovalutazione
    '            Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.DeleteOneAnswerTestAutovalutazione, , lm.ActionDataContract.InteractionType.UserWithUser)
    '        Case Else
    '            Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.DeleteOneAnswerElse, , lm.ActionDataContract.InteractionType.UserWithUser)
    '    End Select
    'End Sub
    Public Sub DeleteAllAnswersActionAdd()
        Select Case Me.QuestionarioCorrente.tipo
            Case Questionario.TipoQuestionario.Sondaggio
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.DeleteAllAnswersSondaggio, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Questionario
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.DeleteAllAnswersQuestionario, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Meeting
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.DeleteAllAnswersMeetingPoll, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Autovalutazione
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.DeleteAllAnswersTestAutovalutazione, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Else
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.DeleteAllAnswersElse, , lm.ActionDataContract.InteractionType.UserWithUser)
        End Select
    End Sub
    Public Sub DeleteActionAdd(ByVal idQuestionnaire As Integer, idModule As Integer, idCommunity As Integer)
        Dim objects As New List(Of WS_Actions.ObjectAction)
        objects.Add(New WS_Actions.ObjectAction With {.ModuleID = idModule, .ValueID = idQuestionnaire, .ObjectTypeId = CInt(COL_Questionario.ModuleQuestionnaire.ObjectType.Questionario)})

        InternalDeleteActionAdd(Me.QuestionarioCorrente.tipo, objects, idModule, idCommunity)
    End Sub
    Public Sub DeleteActionAdd(ByVal idQuestionnaire As Integer, idLanguage As Integer, idModule As Integer, idCommunity As Integer)
        Dim objects As New List(Of WS_Actions.ObjectAction)
        objects.Add(New WS_Actions.ObjectAction With {.ModuleID = idModule, .ValueID = idQuestionnaire, .ObjectTypeId = CInt(COL_Questionario.ModuleQuestionnaire.ObjectType.Questionario)})
        objects.Add(New WS_Actions.ObjectAction With {.ModuleID = idModule, .ValueID = idLanguage, .ObjectTypeId = CInt(COL_Questionario.ModuleQuestionnaire.ObjectType.Language)})
        InternalDeleteActionAdd(Me.QuestionarioCorrente.tipo, objects, idModule, idCommunity)
    End Sub
    Public Sub DeleteActionAdd(ByVal idQuestionnaire As Integer, idLanguage As Integer, idQuestLanguage As Integer, idModule As Integer, idCommunity As Integer)
        Dim objects As New List(Of WS_Actions.ObjectAction)
        objects.Add(New WS_Actions.ObjectAction With {.ModuleID = idModule, .ValueID = idQuestionnaire, .ObjectTypeId = CInt(COL_Questionario.ModuleQuestionnaire.ObjectType.Questionario)})
        objects.Add(New WS_Actions.ObjectAction With {.ModuleID = idModule, .ValueID = idLanguage, .ObjectTypeId = CInt(COL_Questionario.ModuleQuestionnaire.ObjectType.Language)})
        objects.Add(New WS_Actions.ObjectAction With {.ModuleID = idModule, .ValueID = idQuestLanguage, .ObjectTypeId = CInt(COL_Questionario.ModuleQuestionnaire.ObjectType.QuestionnaireTranslation)})

        InternalDeleteActionAdd(Me.QuestionarioCorrente.tipo, objects, idModule, idCommunity)
    End Sub
    Private Sub InternalDeleteActionAdd(type As Questionario.TipoQuestionario, objects As List(Of WS_Actions.ObjectAction), idModule As Integer, idCommunity As Integer)
        Dim action As Questionario.TipoQuestionario = Questionario.TipoQuestionario.Questionario
        Select Case Me.QuestionarioCorrente.tipo
            Case Questionario.TipoQuestionario.Questionario
                action = COL_Questionario.ModuleQuestionnaire.ActionType.DeleteQuestionario
            Case Questionario.TipoQuestionario.Sondaggio
                action = COL_Questionario.ModuleQuestionnaire.ActionType.DeleteSondaggio
            Case Questionario.TipoQuestionario.Meeting
                action = COL_Questionario.ModuleQuestionnaire.ActionType.DeleteMeetingPoll
            Case Questionario.TipoQuestionario.LibreriaDiDomande
                action = COL_Questionario.ModuleQuestionnaire.ActionType.DeleteLibreria
            Case Questionario.TipoQuestionario.Autovalutazione
                action = COL_Questionario.ModuleQuestionnaire.ActionType.DeleteTestAutovalutazione
            Case Else
                action = COL_Questionario.ModuleQuestionnaire.ActionType.DeleteElse
        End Select
        PageUtility.AddActionToModule(idCommunity, idModule, action, objects, lm.ActionDataContract.InteractionType.UserWithUser)
    End Sub
    Public Sub ExportActionAdd(ByVal idQuestionnaire As Integer, idModule As Integer, idCommunity As Integer)
        Dim objects As New List(Of WS_Actions.ObjectAction)
        objects.Add(New WS_Actions.ObjectAction With {.ModuleID = idModule, .ValueID = idQuestionnaire, .ObjectTypeId = CInt(COL_Questionario.ModuleQuestionnaire.ObjectType.Questionario)})
        Me.PageUtility.AddActionToModule(idCommunity, idModule, COL_Questionario.ModuleQuestionnaire.ActionType.ExportResults, objects, lm.ActionDataContract.InteractionType.UserWithUser)
    End Sub
    Public Sub ViewListActionAdd(ByVal questType As Integer)
        Select Case questType
            Case Questionario.TipoQuestionario.Questionario
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.QuestionariList, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Sondaggio
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.SondaggiList, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Meeting
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.MeetingPollList, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.LibreriaDiDomande
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.LibrerieList, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Autovalutazione
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.TestAutovalutazioneList, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Else
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.ElseList, , lm.ActionDataContract.InteractionType.UserWithUser)
        End Select
    End Sub
    Public Sub ViewAdminListActionAdd(ByVal questType As Integer)
        Select Case questType
            Case Questionario.TipoQuestionario.Questionario
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.QuestionariAdminList, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Sondaggio
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.SondaggiAdminList, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Meeting
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.MeetingPollAdminList, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.LibreriaDiDomande
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.LibrerieAdminList, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Autovalutazione
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.TestAutovalutazioneAdminList, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Else
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.ElseAdminList, , lm.ActionDataContract.InteractionType.UserWithUser)
        End Select
    End Sub
    Public Sub EditLibraryAssociationActionAdd()
        Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.EditLibraryAssociation, , lm.ActionDataContract.InteractionType.UserWithLearningObject)

    End Sub


#End Region

    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub BindNoPermessi()

    End Sub

    Public Overrides Function HasPermessi() As Boolean

    End Function

    Public Overrides ReadOnly Property isCompileForm() As Boolean
        Get

        End Get
    End Property

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetControlliByPermessi()

    End Sub

    Public Overrides Sub SetCultureSettings()

    End Sub

    Public Overrides Sub SetInternazionalizzazione()

    End Sub
    Public Overrides ReadOnly Property LoadDataByUrl As Boolean
        Get
            Return False
        End Get
    End Property
End Class
