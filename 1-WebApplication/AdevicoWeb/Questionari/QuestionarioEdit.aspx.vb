Imports COL_Questionario

Partial Public Class QuestionarioEdit
    Inherits PageBaseQuestionario


#Region "Context"
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = lm.Comol.UI.Presentation.SessionHelpers.CurrentUserContext, .DataContext = lm.Comol.UI.Presentation.SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Private _Service As COL_Questionario.Business.ServiceQuestionnaire
    Private ReadOnly Property CurrentService() As COL_Questionario.Business.ServiceQuestionnaire
        Get
            If IsNothing(_Service) Then
                _Service = New COL_Questionario.Business.ServiceQuestionnaire(Me.CurrentContext)
            End If
            Return _Service
        End Get
    End Property
#End Region

    Private Property CurrentFilterQuestion() As Integer
        Get
            Return ViewStateOrDefault("CurrentFilterQuestion", 1)
        End Get
        Set(ByVal value As Integer)
            ViewState("CurrentFilterQuestion") = value
        End Set
    End Property

    'Dim oQuest As New Questionario
    Public Shared isAperto As Boolean
    Public Shared _iPag As Integer ' indice pagina
    Public Shared iDom As Integer ' indice domanda
    'Public Shared idQ As String
    Dim oGestioneDomande As New GestioneDomande
    'Dim oGestioneRisposte As New GestioneRisposte
    Dim oGestioneQuest As New GestioneQuestionario
    Private _SmartTagsAvailable As SmartTags
    Private Property isAdmin As Boolean
        Get
            If IsNumeric(Session("qsIsAdmin_" & Me.QuestionarioCorrente.id)) Then
                Return Session("qsIsAdmin_" & Me.QuestionarioCorrente.id)
            Else
                isAdmin = False
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            Session("qsIsAdmin_" & Me.QuestionarioCorrente.id) = value
        End Set
    End Property
    Public Property isCopyVisible() As Boolean
        Get
            Return ViewState("isCopyVisible")
        End Get
        Set(ByVal value As Boolean)
            ViewState("isCopyVisible") = value
        End Set
    End Property
    Public Property isDeleteVisible() As Boolean
        Get
            Return ViewState("isDeleteVisible")
        End Get
        Set(ByVal value As Boolean)
            ViewState("isDeleteVisible") = value
        End Set
    End Property
    Private Property actionType() As Azioni
        Get
            Return ViewState("copyType")
        End Get
        Set(ByVal value As Azioni)
            ViewState("copyType") = value
        End Set
    End Property
    Private Property listaLibrerie() As List(Of Questionario)
        Get
            Return ViewState("listaLibrerie")
        End Get
        Set(ByVal value As List(Of Questionario))
            ViewState("listaLibrerie") = value
        End Set
    End Property
    Private Property iPag() As Integer
        Get
            Return _iPag
        End Get
        Set(ByVal value As Integer)
            _iPag = value
        End Set
    End Property
    Private Property listaDomandeSelezionate() As List(Of Domanda)
        Get
            Return ViewState("domandeSelezionate")
        End Get
        Set(ByVal value As List(Of Domanda))
            ViewState("domandeSelezionate") = value
        End Set
    End Property


    'Private ReadOnly Property listaDomandeSelezionate() As List(Of Domanda)
    '    Get
    '        'Return ViewState("domandeSelezionate")

    '        listaDomandeSelezionate = New List(Of Domanda)
    '        ' ciclo il datalist delle domande
    '        For Each oItem As DataListItem In DLPagine.Items


    '            Dim dlDomande As DataList
    '            dlDomande = DirectCast(oItem.FindControl("DLDomande"), DataList)

    '            Dim indice As Integer = 0

    '            For Each oItemD As DataListItem In dlDomande.Items
    '                ' prelevo le domande selezionate da aggiungere al questionario
    '                If DirectCast(oItemD.FindControl("CHKSelect"), CheckBox).Checked Then
    '                    Dim idDomandaSel = dlDomande.DataKeys.Item(indice)
    '                    Dim oDomandaSel As New Domanda

    '                    Dim iPagCount As Integer = Me.QuestionarioCorrente.pagine.Count()

    '                    If iPagCount > 0 Then
    '                        For pageIndex As Integer = 0 To (iPagCount - 1)
    '                            oDomandaSel = Domanda.findDomandaBYID(Me.QuestionarioCorrente.pagine(iPag).domande, idDomandaSel)
    '                            If Not IsNothing(oDomandaSel) Then
    '                                Exit For
    '                            End If
    '                        Next
    '                    End If

    '                    If Not IsNothing(oDomandaSel) Then
    '                        listaDomandeSelezionate.Add(oDomandaSel)
    '                    End If

    '                End If
    '                indice = indice + 1
    '            Next

    '        Next
    '    End Get
    '    'Set(ByVal value As List(Of Domanda))
    '    '    ViewState("domandeSelezionate") = value
    '    'End Set
    'End Property




    Public ReadOnly Property SmartTagsAvailable() As Comol.Entity.SmartTags
        Get
            If _SmartTagsAvailable Is Nothing Then
                _SmartTagsAvailable = ManagerConfiguration.GetSmartTags(Me.ApplicationUrlBase())
            End If
            Return _SmartTagsAvailable
        End Get
    End Property
    Private Property HasUserResponses() As Boolean
        Get
            Return ViewStateOrDefault("HasUserResponses", False)
        End Get
        Set(ByVal value As Boolean)
            ViewState("HasUserResponses") = value
        End Set
    End Property
    Private Enum Azioni
        CreaDomanda = 0
        AggiungiDomandeDaLibreria = 1
        AggiungiDomandeDaQuestionario = 2
        CopiaDomandeInLibreria = 3
        CopiaDomandeInQuestionario = 4
        EliminaDomande = 5
    End Enum
    Private Sub caricaDati(Optional ByRef oQuestPagine As List(Of QuestionarioPagina) = Nothing)
        Dim oImg As ImageButton
        oImg = Me.IMBHelp

        oImg.Attributes.Add("onclick", RootObject.apriPopUp(RootObject.helpQuestionariEdit, "target", "yes", "yes"))

        If Me.QuestionarioCorrente.isReadOnly Then
            LNBAggiungiPagina.Visible = False
        End If
        If Not Me.QuestionarioCorrente Is Nothing Then
            If Me.QuestionarioCorrente.pagine.Count = 0 Then
                Me.PaginaCorrenteID = 0
                Me.RedirectToUrl(RootObject.PaginaEdit)
            End If
            isAperto = Not Me.QuestionarioCorrente.isReadOnly
            If oQuestPagine Is Nothing Then
                DLPagine.DataSource = Me.QuestionarioCorrente.pagine
            Else
                DLPagine.DataSource = oQuestPagine
            End If

            setDomandeSelezionate()

            DLPagine.DataBind()
            DLPagine = oGestioneDomande.setDomandePaginaEdit(DLPagine)
            LBTitolo.Text = Me.QuestionarioCorrente.nome
            If DLPagine.Items.Count > 2 Then
                For i As Integer = 1 To DLPagine.Items.Count - 2
                    DirectCast(DLPagine.Items(i).FindControl("RBLfiltraDomande"), RadioButtonList).Visible = False
                Next
            End If
            'If Me.QuestionarioCorrente.domande.Count > 0 Then
            '    DirectCast(DLPagine.Items(iPag).FindControl("LBScegliAzioneBottom"), Label).Visible = True
            '    DirectCast(DLPagine.Items(iPag).FindControl("DDLAzioniBottom"), DropDownList).Visible = True
            '    DirectCast(DLPagine.Items(iPag).FindControl("LNBConfermaAzioneBottom"), LinkButton).Visible = True
            'Else
            '    DirectCast(DLPagine.Items(iPag).FindControl("LBScegliAzioneBottom"), Label).Visible = False
            '    DirectCast(DLPagine.Items(iPag).FindControl("DDLAzioniBottom"), DropDownList).Visible = False
            '    DirectCast(DLPagine.Items(iPag).FindControl("LNBConfermaAzioneBottom"), LinkButton).Visible = False
            'End If
        End If
    End Sub
    Private Sub caricaDatiQuestionarioRandom()
        If Me.QuestionarioCorrente.librerieQuestionario.Count > 0 And Not Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Autovalutazione Then
            Me.MLVquestionari.SetActiveView(Me.VIWImpostaLibrerie)
            Me.RPTLibrerieQuestionario.DataSource = Me.QuestionarioCorrente.librerieQuestionario
            Me.RPTLibrerieQuestionario.DataBind()
            BTNSalvaLibrerie.Visible = Not Me.QuestionarioCorrente.isReadOnly AndAlso Not HasUserResponses
            BTNSelezionaLibrerie.Visible = Not Me.QuestionarioCorrente.isReadOnly AndAlso Not HasUserResponses
            LBMessaggioImpostaLibrerie.Visible = True
            LBMessaggioImpostaLibrerie.Text = Me.Resource.getValue("msgHelpSelezionaDomande")
        Else
            listaLibrerie = DALQuestionario.readLibrerieQuestionarioByComunita(Me.PageUtility.CurrentContext, Me.QuestionarioCorrente, Me.ComunitaCorrenteID)
            GRVLibrerie.DataSource = listaLibrerie
            GRVLibrerie.DataBind()
            Me.BTNAggiungiLibreria.Visible = Not HasUserResponses
        End If
    End Sub
    Protected Sub loadDomandeOpzioni(ByVal sender As Object, ByVal e As DataListItemEventArgs)
        iDom = e.Item.ItemIndex
        'oQuest = Session("oQuest")
        Dim oQuestionario As New Questionario
        oQuestionario.pagine = DLPagine.DataSource
        DLPagine.Controls(iPag).FindControl("DLDomande").Controls(iDom).FindControl("PHOpzioni").Controls.Add(oGestioneDomande.loadDomandeOpzioni(oQuestionario, iPag, iDom, True))
        Dim LBtestoDopoDomanda As New Label
        LBtestoDopoDomanda.Text = Me.QuestionarioCorrente.pagine(iPag).domande(iDom).testoDopo
        If Not LBtestoDopoDomanda.Text Is String.Empty Then
            Dim aCapo As New LiteralControl
            aCapo.Text = "<br>"
            DLPagine.Controls(iPag).FindControl("DLDomande").Controls(iDom).FindControl("PHOpzioni").Controls.Add(LBtestoDopoDomanda)
        End If
        SetInternazionalizzazioneDLDomande(e)
    End Sub
    Protected ReadOnly Property MandatoryDisplay(ByVal question As Domanda) As String
        Get
            Dim mandatory As String = ""
            If question.isObbligatoria Then
                'mandatory = "<span class=""mandatory"" title=""" & Me.Resource.getValue("isMandatory.Title") & """>" & Me.Resource.getValue("isMandatory") & "</span>"
                mandatory = "<span class=""mandatory"">" & Me.Resource.getValue("isMandatory") & "</span>"
            End If
            Return mandatory
        End Get
    End Property
    Protected ReadOnly Property MandatoryToolTip(ByVal question As Domanda) As String
        Get
            Return IIf(question.isObbligatoria, Me.Resource.getValue("isMandatory.Title"), "")
        End Get
    End Property
    Protected Sub DLPagine_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles DLPagine.ItemDataBound
        iPag = e.Item.ItemIndex
        If isCopyVisible Then
            Dim ddlCopia As DropDownList
            ' carico la drop down dei questionari di destinazione in base a quello che ho selezionato nella DDLAzioni (Libreria  o QUest)
            ddlCopia = DirectCast(e.Item.FindControl("DDLQuestionariDestinazione"), DropDownList)
            Select Case actionType
                Case actionType.CopiaDomandeInQuestionario
                    ddlCopia.DataSource = DALQuestionario.readQuestionariByComunita(Me.ComunitaCorrenteID)
                Case actionType.CopiaDomandeInLibreria
                    ddlCopia.DataSource = DALQuestionario.readLibrerieByComunita(PageUtility.CurrentContext, Me.ComunitaCorrenteID, Me.QuestionarioCorrente.idLingua)
            End Select
            ddlCopia.DataBind()
        End If
        DirectCast(e.Item.FindControl("DDLAzioni"), DropDownList).SelectedValue = Me.actionType
        Dim dlDomande As New DataList
        dlDomande = DLPagine.Controls(iPag).FindControl("DLDomande")
        'dlDomande.DataSource = Me.QuestionarioCorrente.pagine.Item(iPag).domande
        Dim oPagineList As New List(Of QuestionarioPagina)
        oPagineList = DLPagine.DataSource
        dlDomande.DataSource = oPagineList.Item(iPag).domande
        dlDomande.DataBind()
        SetInternazionalizzazioneDLPagine(e)
        If Me.qs_questIdType = Questionario.TipoQuestionario.LibreriaDiDomande Then
            e.Item.FindControl("IMBPagina").Visible = False
            e.Item.FindControl("IMBEliminaPag").Visible = False
        End If
        If Me.qs_questIdType = COL_Questionario.Questionario.TipoQuestionario.Sondaggio And Me.QuestionarioCorrente.pagine(0).domande.Count > 0 Then
            DirectCast(e.Item.FindControl("DDLAzioni"), DropDownList).Enabled = False
            DirectCast(e.Item.FindControl("LNBConfermaAzione"), LinkButton).Enabled = False
            DirectCast(e.Item.FindControl("DDLAzioniBottom"), DropDownList).Enabled = False
            DirectCast(e.Item.FindControl("LNBConfermaAzioneBottom"), LinkButton).Enabled = False
        End If

        DirectCast(e.Item.FindControl("LBScegliAzioneBottom"), Label).Visible = (dlDomande.Items.Count > 1)
        DirectCast(e.Item.FindControl("DDLAzioniBottom"), DropDownList).Visible = (dlDomande.Items.Count > 1)
        DirectCast(e.Item.FindControl("LNBConfermaAzioneBottom"), LinkButton).Visible = (dlDomande.Items.Count > 1)


        Try
            DirectCast(e.Item.FindControl("RBLfiltraDomande"), RadioButtonList).Visible = (dlDomande.Items.Count > 1)
            DirectCast(e.Item.FindControl("RBLfiltraDomande"), RadioButtonList).SelectedValue = CurrentFilterQuestion
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub DLDomandeEditCommand(ByVal sender As Object, ByVal e As DataListCommandEventArgs)

        'oQuest = Session("oQuest")
        Dim redirectUrl As String = oGestioneDomande.DLDomandeEditCommand(sender, e, Me.QuestionarioCorrente)
        Session("isPrimoAccesso") = True
        If Not redirectUrl = String.Empty Then
            Me.RedirectToUrl(redirectUrl)
        End If

        caricaQuestionario()

    End Sub
    Protected Sub caricaQuestionario()
        Me.QuestionarioCorrente = DALQuestionario.readQuestionarioBYLingua(Me.PageUtility.CurrentContext, Me.QuestionarioCorrente.id, Me.QuestionarioCorrente.idLingua, False)

        caricaDati()
    End Sub
    Protected Sub filtraDomande(ByVal RBLfiltro As RadioButtonList, ByVal e As System.EventArgs)
        CurrentFilterQuestion = RBLfiltro.SelectedValue
        Select Case RBLfiltro.SelectedValue
            Case 1
                caricaDati()
            Case 2
                Dim oPaginaList As New List(Of QuestionarioPagina)
                For Each oPagina As QuestionarioPagina In Me.QuestionarioCorrente.pagine
                    Dim oPaginaNew As New QuestionarioPagina
                    For Each oDomanda As Domanda In oPagina.domande
                        If oDomanda.difficolta = 0 Then
                            oPaginaNew.domande.Add(oDomanda)
                        End If
                    Next
                    oPaginaList.Add(oPaginaNew)
                Next
                caricaDati(oPaginaList)
            Case 3
                Dim oPaginaList As New List(Of QuestionarioPagina)
                For Each oPagina As QuestionarioPagina In Me.QuestionarioCorrente.pagine
                    Dim oPaginaNew As New QuestionarioPagina
                    For Each oDomanda As Domanda In oPagina.domande
                        If oDomanda.difficolta = 1 Then
                            oPaginaNew.domande.Add(oDomanda)
                        End If
                    Next
                    oPaginaList.Add(oPaginaNew)
                Next
                caricaDati(oPaginaList)
            Case 4
                Dim oPaginaList As New List(Of QuestionarioPagina)
                For Each oPagina As QuestionarioPagina In Me.QuestionarioCorrente.pagine
                    Dim oPaginaNew As New QuestionarioPagina
                    For Each oDomanda As Domanda In oPagina.domande
                        If oDomanda.difficolta = 2 Then
                            oPaginaNew.domande.Add(oDomanda)
                        End If
                    Next
                    oPaginaList.Add(oPaginaNew)
                Next
                caricaDati(oPaginaList)
        End Select
    End Sub
    Protected Sub confermaAzione(ByVal dropAzione As DropDownList, ByVal e As DataListCommandEventArgs)
        Dim pageNumber As Integer = 1
        Integer.TryParse(DirectCast(e.Item.FindControl("LTpageNumber"), Literal).Text, pageNumber)

        Select Case dropAzione.SelectedValue
            Case Azioni.CreaDomanda  ' crea domanda
                Me.PaginaCorrenteID = DLPagine.DataKeys.Item(e.Item.ItemIndex)
                Me.DomandaCorrente = New Domanda
                Me.DomandaCorrente.id = 0

                AddAction(PaginaCorrenteID, ModuleQuestionnaire.ObjectType.Page, ModuleQuestionnaire.ActionType.QuestionStartAdding, QuestionarioCorrente.tipo)
                Me.RedirectToUrl(RootObject.DomandaAdd)
            Case Azioni.AggiungiDomandeDaLibreria  ' aggiungi domande da libreria
                Me.PaginaCorrenteID = DLPagine.DataKeys.Item(e.Item.ItemIndex)
                AddAction(PaginaCorrenteID, ModuleQuestionnaire.ObjectType.Page, ModuleQuestionnaire.ActionType.QuestionsSelectFromLibrary)
                Me.RedirectToUrl(RootObject.LibreriaDomandeSelect & "?type=1" & "&pI=" & pageNumber)
            Case Azioni.AggiungiDomandeDaQuestionario  ' aggiungi domande da questionario
                Me.PaginaCorrenteID = DLPagine.DataKeys.Item(e.Item.ItemIndex)
                AddAction(PaginaCorrenteID, ModuleQuestionnaire.ObjectType.Page, ModuleQuestionnaire.ActionType.QuestionsSelectFromQuestionnaire)
                Me.RedirectToUrl(RootObject.LibreriaDomandeSelect + "?type=0" & "&pI=" & pageNumber)
            Case Azioni.CopiaDomandeInLibreria  ' copia domande in libreria
                Me.PaginaCorrenteID = DLPagine.DataKeys.Item(e.Item.ItemIndex)
                actionType = dropAzione.SelectedValue
                isCopyVisible = True
                AddAction(PaginaCorrenteID, ModuleQuestionnaire.ObjectType.Page, ModuleQuestionnaire.ActionType.QuestionsSelectForCopyToLibrary)
                getDomandeSelezionate()
            Case Azioni.CopiaDomandeInQuestionario  ' copia domande in questionario
                Me.PaginaCorrenteID = DLPagine.DataKeys.Item(e.Item.ItemIndex)
                actionType = dropAzione.SelectedValue
                isCopyVisible = True
                AddAction(PaginaCorrenteID, ModuleQuestionnaire.ObjectType.Page, ModuleQuestionnaire.ActionType.QuestionsSelectForCopyToQuestionnaire)
                getDomandeSelezionate()
            Case Azioni.EliminaDomande
                actionType = dropAzione.SelectedValue
                isCopyVisible = False
                isDeleteVisible = True
                AddAction(PaginaCorrenteID, ModuleQuestionnaire.ObjectType.Page, ModuleQuestionnaire.ActionType.QuestionsSelectForDelete)
                getDomandeSelezionate()
        End Select
    End Sub
    Protected Sub DLPagineEditCommand(ByVal sender As Object, ByVal e As DataListCommandEventArgs)

        'oGestioneDomande.DLPagineEditCommand(DLPagine, e, LBMessage, Me.QuestionarioCorrente)
        Select Case e.CommandName
            Case "elimina"
                If Me.QuestionarioCorrente.pagine(e.Item.ItemIndex).domande.Count > 0 Then
                    CTRLmessages.Visible = True
                    CTRLmessages.InitializeControl(Resource.getValue("PageWithQuestionWarning"), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
                Else
                    AddAction(Me.QuestionarioCorrente.pagine(e.Item.ItemIndex).id, ModuleQuestionnaire.ObjectType.Page, ModuleQuestionnaire.ActionType.PagePhisicalDelete)
                    DALPagine.Pagina_Delete(Me.QuestionarioCorrente, Me.QuestionarioCorrente.pagine(e.Item.ItemIndex).id, Me.QuestionarioCorrente.pagine(e.Item.ItemIndex).numeroPagina)
                End If
            Case "paginaEdit"
                Me.PaginaCorrenteID = DLPagine.DataKeys.Item(e.Item.ItemIndex)
                AddAction(PaginaCorrenteID, ModuleQuestionnaire.ObjectType.Page, ModuleQuestionnaire.ActionType.PageStartEditing)

                Me.RedirectToUrl(RootObject.PaginaEdit)
            Case "ConfermaAzione"
                Dim dropAzione As DropDownList
                dropAzione = DirectCast(DLPagine.Items(e.Item.ItemIndex).FindControl("DDLAzioni"), DropDownList)
                confermaAzione(dropAzione, e)
            Case "ConfermaAzioneBottom"
                Dim dropAzione As DropDownList
                dropAzione = DirectCast(DLPagine.Items(e.Item.ItemIndex).FindControl("DDLAzioniBottom"), DropDownList)
                confermaAzione(dropAzione, e)
            Case "CopiaDomande"
                Me.PaginaCorrenteID = DLPagine.DataKeys.Item(e.Item.ItemIndex)

                Dim destPageId As Integer = 0

                Dim idQuestDest As Integer
                Dim ddlQuest As DropDownList = DirectCast(DLPagine.Items(e.Item.ItemIndex).FindControl("DDLQuestionariDestinazione"), DropDownList)

                idQuestDest = ddlQuest.SelectedValue

                If idQuestDest = Me.QuestionarioCorrente.id Then
                    destPageId = Me.PaginaCorrenteID
                End If

                copiaDomandeSelezionate(idQuestDest, destPageId)
            Case "ConfermaElimina"
                getDomandeSelezionate()
                eliminaDomandeSelezionate()
            Case "AnnullaElimina"
                isDeleteVisible = False
        End Select
        ' ricarico i dati
        caricaQuestionario()

    End Sub
    Protected Sub copiaDomandeSelezionate(ByVal idQuestDestinazione As Integer, ByVal idPaginaTarget As Integer)

        'Dim idNuovaPagina As Integer = 0



        If listaDomandeSelezionate.Count > 0 Then

            Dim questionarioDestinazione As New Questionario
            Dim questionadded As Integer = 0

            questionarioDestinazione = DALQuestionario.readQuestionarioBYLingua(Me.PageUtility.CurrentContext, idQuestDestinazione, Me.LinguaQuestionario, False)

            If IsNothing(questionarioDestinazione) OrElse Not questionarioDestinazione.pagine.Any() Then
                Return
            End If

            'contorllo che la pagina esista nel questionario
            If Not questionarioDestinazione.pagine.Any(Function(pg) pg.id = idPaginaTarget) Then
                idPaginaTarget = 0
            End If

            'Se ho Id = 0, prendo l'ultima pagina del questionario
            If idPaginaTarget = 0 Then
                idPaginaTarget = questionarioDestinazione.pagine.LastOrDefault().id
            End If

            Dim PaginaTarget As QuestionarioPagina = questionarioDestinazione.pagine.FirstOrDefault(Function(p) p.id = idPaginaTarget)


            Dim newIndex As Integer = 0
            Dim addQuestion As Integer = listaDomandeSelezionate.Count

            If PaginaTarget.domande.Any() Then
                newIndex = PaginaTarget.domande.LastOrDefault().numero
            Else
                newIndex = questionarioDestinazione.pagine.OrderByDescending(Function(p) p.numeroPagina).FirstOrDefault(Function(p) p.numeroPagina < PaginaTarget.numeroPagina AndAlso p.domande.Any).domande.LastOrDefault().numero
            End If

            'Dim newDomande As New List(Of Domanda)()
            Dim currentIndex As Integer = newIndex

            For Each oDomandaSel As Domanda In listaDomandeSelezionate
                currentIndex += 1

                oDomandaSel.id = 0
                oDomandaSel.idQuestionario = questionarioDestinazione.id

                oDomandaSel.idPagina = idPaginaTarget                   'questionarioDestinazione.pagine(questionarioDestinazione.pagine.Count - 1).id
                oDomandaSel.numeroPagina = PaginaTarget.numeroPagina    'questionarioDestinazione.pagine(questionarioDestinazione.pagine.Count - 1).numeroPagina

                oDomandaSel.numero = currentIndex


                PaginaTarget.domande.Add(oDomandaSel)   '??? vediamo...
                'questionarioDestinazione.pagine(questionarioDestinazione.pagine.Count - 1).domande.Add(oDomandaSel)

                'VErificare COME aggiorna i numeri domande delle domande precedenti!!!
                DALDomande.Salva(oDomandaSel, False, Me.QuestionarioCorrente.tipo = COL_Questionario.Questionario.TipoQuestionario.LibreriaDiDomande)

                questionadded += 1
            Next

            'Aggiorno la tabella LK_Quest_Domanda
            'SE copio nell'ultima pagina (vuota), questa cosa non serve.
            'DALDomande.DomandaAggiornaNumeri_Update(questionarioDestinazione.id, newIndex, questionadded)

            'aggiorno i numeri di domande per pagina, rinumerando TUTTE le pagina da quella aggiornata
            Dim lastDomandaIndex As Integer = 0
            Dim pgUpdate As Boolean = False
            For Each oPg As QuestionarioPagina In questionarioDestinazione.pagine.OrderBy(Function(pg) pg.numeroPagina)

                'Inizio dalla pagina TAGET
                If oPg.id = idPaginaTarget Then
                    If oPg.dallaDomanda = 0 Then
                        oPg.dallaDomanda = newIndex + 1
                        oPg.allaDomanda = oPg.dallaDomanda + (questionadded - 1)
                    Else
                        oPg.allaDomanda = oPg.allaDomanda + (questionadded)
                    End If

                    lastDomandaIndex = oPg.allaDomanda

                    DALPagine.Pagina_Update(oPg)
                    pgUpdate = True

                    'le precedenti le ignoro
                ElseIf pgUpdate Then
                    If oPg.dallaDomanda > 0 Then
                        Dim opgDomande As Integer = oPg.allaDomanda - oPg.dallaDomanda
                        oPg.dallaDomanda = lastDomandaIndex + 1
                        oPg.allaDomanda = oPg.dallaDomanda + opgDomande
                        DALPagine.Pagina_Update(oPg)
                    Else
                        oPg.dallaDomanda = 0
                        oPg.allaDomanda = 0
                        DALPagine.Pagina_Update(oPg)
                    End If
                End If

                lastDomandaIndex = oPg.allaDomanda
            Next


            'Dim oPagina As New QuestionarioPagina
            'oPagina = questionarioDestinazione.pagine.FirstOrDefault(Function(qs) qs.id =)

            'oPagina.allaDomanda = oPagina.allaDomanda + questionadded
            'DALPagine.Pagina_Update(oPagina)

            'caricaQuestionario()

            If idQuestDestinazione = QuestionarioCorrente.id Then
                QuestionarioCorrente = DALQuestionario.readQuestionarioBYLingua(Me.PageUtility.CurrentContext, idQuestDestinazione, 0, False)
            End If

            If questionarioDestinazione.tipo = Questionario.TipoQuestionario.LibreriaDiDomande Then
                AddAction(questionarioDestinazione.id, ModuleQuestionnaire.ObjectType.Questionario, ModuleQuestionnaire.ActionType.QuestionsCopyToLibrary)
                RedirectToUrl(RootObject.QuestionarioEdit & "?confirm=2&type=" + QuestionarioCorrente.tipo.ToString())
            Else
                AddAction(questionarioDestinazione.id, ModuleQuestionnaire.ObjectType.Questionario, ModuleQuestionnaire.ActionType.QuestionsCopyToQuestionnaire)
                RedirectToUrl(RootObject.QuestionarioEdit & "?confirm=1&type=" + QuestionarioCorrente.tipo.ToString())
            End If

        Else

        End If

    End Sub
    Protected Sub eliminaDomandeSelezionate()

        If listaDomandeSelezionate.Count > 0 Then

            For Each oDomandaSel As Domanda In listaDomandeSelezionate

                ' rimuovo la domanda
                'Me.QuestionarioCorrente.pagine(oDomandaSel.numeroPagina - 1).domande.RemoveAt(e.Item.ItemIndex)

                Domanda.removeDomandaBYID(Me.QuestionarioCorrente.pagine(oDomandaSel.numeroPagina - 1).domande, oDomandaSel.id)

                For Each oPag As QuestionarioPagina In Me.QuestionarioCorrente.pagine
                    If Integer.Parse(oPag.numeroPagina) >= Integer.Parse(oDomandaSel.numeroPagina) Then
                        For Each oDom As Domanda In oPag.domande
                            If Integer.Parse(oDom.numero) > Integer.Parse(oDomandaSel.numero) Then
                                oDom.numero = oDom.numero - 1
                            End If
                        Next
                        If oPag.domande.Count > 0 Then
                            oPag.dallaDomanda = oPag.domande(0).numero
                            oPag.allaDomanda = oPag.domande(oPag.domande.Count - 1).numero
                        Else
                            oPag.dallaDomanda = 0
                            oPag.allaDomanda = 0
                        End If
                        DALPagine.Pagina_Update(oPag)
                    End If
                Next

                If QuestionarioCorrente.tipo = QuestionnaireType.QuestionLibrary Then
                    AddAction(oDomandaSel.id, ModuleQuestionnaire.ObjectType.Domanda, ModuleQuestionnaire.ActionType.QuestionVirtualRemoveFromLibrary)
                    DALDomande.DomandaQuestionarioLK_Set_isOld(oDomandaSel.id, oDomandaSel.numero, QuestionarioCorrente.id)
                Else
                    If DALDomande.Domanda_Delete(QuestionarioCorrente.id, oDomandaSel.numero, oDomandaSel.id) > 0 Then
                        AddAction(oDomandaSel.id, ModuleQuestionnaire.ObjectType.Domanda, ModuleQuestionnaire.ActionType.QuestionDelete)
                    End If
                End If

                'DALDomande.Domanda_Delete(Me., oDomandaSel.numero, oDomandaSel.id)
            Next

            isDeleteVisible = False

        Else
            isDeleteVisible = False
        End If

    End Sub
    'Protected Sub getDomandeSelezionate()

    '    'listaDomandeSelezionate = New List(Of Domanda)
    '    '' ciclo il datalist delle domande
    '    'For Each oItem As DataListItem In DLPagine.Items


    '    '    Dim dlDomande As DataList
    '    '    dlDomande = DirectCast(oItem.FindControl("DLDomande"), DataList)

    '    '    Dim indice As Integer = 0

    '    '    For Each oItemD As DataListItem In dlDomande.Items
    '    '        ' prelevo le domande selezionate da aggiungere al questionario
    '    '        If DirectCast(oItemD.FindControl("CHKSelect"), CheckBox).Checked Then
    '    '            Dim idDomandaSel = dlDomande.DataKeys.Item(indice)
    '    '            Dim oDomandaSel As New Domanda

    '    '            Dim iPagCount As Integer = Me.QuestionarioCorrente.pagine.Count()

    '    '            If iPagCount > 0 Then
    '    '                For pageIndex As Integer = 0 To (iPagCount - 1)
    '    '                    oDomandaSel = Domanda.findDomandaBYID(Me.QuestionarioCorrente.pagine(iPag).domande, idDomandaSel)
    '    '                    If Not IsNothing(oDomandaSel) Then
    '    '                        Exit For
    '    '                    End If
    '    '                Next
    '    '            End If

    '    '            If Not IsNothing(oDomandaSel) Then
    '    '                listaDomandeSelezionate.Add(oDomandaSel)
    '    '            End If

    '    '        End If
    '    '        indice = indice + 1
    '    '    Next

    '    'Next

    'End Sub
    Protected Sub setDomandeSelezionate()
        If Not listaDomandeSelezionate Is Nothing Then
            For Each oDomandaSel As Domanda In listaDomandeSelezionate
                Dim oDomandaQuest As New Domanda
                oDomandaQuest = Domanda.findDomandaBYID(Me.QuestionarioCorrente.pagine(iPag).domande, oDomandaSel.id)
                If Not oDomandaQuest Is Nothing Then
                    oDomandaQuest.isSelected = True
                End If
            Next
        End If



    End Sub

    Protected Sub getDomandeSelezionate()
        Dim currentSelection As New List(Of Domanda)
        'listaDomandeSelezionate = New List(Of Domanda)
        ' ciclo il datalist delle domande
        For Each oItem As DataListItem In DLPagine.Items


            Dim dlDomande As DataList
            dlDomande = DirectCast(oItem.FindControl("DLDomande"), DataList)

            Dim indice As Integer = 0

            For Each oItemD As DataListItem In dlDomande.Items
                ' prelevo le domande selezionate da aggiungere al questionario
                If DirectCast(oItemD.FindControl("CHKSelect"), CheckBox).Checked Then
                    Dim idDomandaSel = dlDomande.DataKeys.Item(indice)
                    Dim oDomandaSel As New Domanda

                    Dim iPagCount As Integer = Me.QuestionarioCorrente.pagine.Count()

                    If iPagCount > 0 Then
                        For pageIndex As Integer = 0 To (iPagCount - 1)
                            oDomandaSel = Domanda.findDomandaBYID(Me.QuestionarioCorrente.pagine(pageIndex).domande, idDomandaSel)
                            If Not IsNothing(oDomandaSel) Then
                                Exit For
                            End If
                        Next
                    End If

                    If Not IsNothing(oDomandaSel) Then
                        currentSelection.Add(oDomandaSel)
                    End If

                End If
                indice = indice + 1
            Next

        Next


        listaDomandeSelezionate = currentSelection
    End Sub
    Protected Sub LNBCartellaPrincipale_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBCartellaPrincipale.Click
        Select Case Me.QuestionarioCorrente.tipo
            Case COL_Questionario.Questionario.TipoQuestionario.Modello
                Me.RedirectToUrl(RootObject.ModelliGestioneList + "&" & qs_questType + CType(COL_Questionario.Questionario.TipoQuestionario.Modello, String))
            Case COL_Questionario.Questionario.TipoQuestionario.LibreriaDiDomande
                Me.RedirectToUrl(RootObject.QuestionariGestioneList + "&" & qs_questType + CType(COL_Questionario.Questionario.TipoQuestionario.LibreriaDiDomande, String))
            Case COL_Questionario.Questionario.TipoQuestionario.Sondaggio
                Me.RedirectToUrl(RootObject.QuestionariGestioneList + "&" & qs_questType + CType(COL_Questionario.Questionario.TipoQuestionario.Sondaggio, String))
            Case COL_Questionario.Questionario.TipoQuestionario.Meeting
                Me.RedirectToUrl(RootObject.QuestionariGestioneList + "&" & qs_questType + CType(COL_Questionario.Questionario.TipoQuestionario.Meeting, String))
            Case Else
                Me.RedirectToUrl(RootObject.QuestionariGestioneList + "&" & qs_questType + CType(COL_Questionario.Questionario.TipoQuestionario.Questionario, String))
        End Select
    End Sub
    Protected Sub LNBGestioneQuestionario_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBGestioneQuestionario.Click
        Me.RedirectToUrl(RootObject.QuestionarioAdmin + "?" & qs_questType + Me.QuestionarioCorrente.tipo.ToString() & "&IdQ=" & Me.QuestionarioCorrente.id & "&idLanguage=" & Me.QuestionarioCorrente.idLingua)
    End Sub
    Protected Sub LNBAggiungiPagina_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBAggiungiPagina.Click
        Me.PaginaCorrenteID = 0
        AddAction(QuestionarioCorrente.id, ModuleQuestionnaire.ObjectType.Questionario, ModuleQuestionnaire.ActionType.PageAdd)
        Me.RedirectToUrl(RootObject.PaginaEdit)
    End Sub
    Public Overrides Sub BindDati()
        Dim nRisposte As Integer = DALRisposte.countRisposteBYIDQuestionario(Me.QuestionarioCorrente.id)
        HasUserResponses = (nRisposte > 0)

        If (nRisposte > 0 AndAlso Not Me.QuestionarioCorrente.isReadOnly) Then
            If qs_ownerId > 0 AndAlso (Me.qs_questIdType = QuestionnaireType.Random OrElse Me.qs_questIdType = QuestionnaireType.RandomMultipleAttempts) AndAlso HasPermissionByExternalObject() Then
                Me.MLVquestionari.SetActiveView(Me.VIWSelezionaLibrerie)
                caricaDatiQuestionarioRandom()
            Else
                LBIsRisposte.Text = String.Format(LBIsRisposte.Text, nRisposte.ToString())
                PNLisRisposte.Visible = True
                Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
            End If

        Else
            If Me.qs_questIdType = QuestionnaireType.Random OrElse Me.qs_questIdType = QuestionnaireType.RandomMultipleAttempts Then
                Me.MLVquestionari.SetActiveView(Me.VIWSelezionaLibrerie)
                caricaDatiQuestionarioRandom()
            ElseIf Me.qs_questIdType = QuestionnaireType.AutoEvaluation Then
                Me.MLVquestionari.SetActiveView(Me.VIWSelezionaLibrerie)
                caricaDatiQuestionarioRandom()
                'GRVLibrerie.Columns(2).Visible = False
                'GRVLibrerie.Columns(3).Visible = False
                'GRVLibrerie.Columns(4).Visible = False
                For Each oRow As GridViewRow In GRVLibrerie.Rows
                    For Each oLib As LibreriaQuestionario In Me.QuestionarioCorrente.librerieQuestionario
                        If oLib.idLibreria = GRVLibrerie.DataKeys(oRow.RowIndex).Value Then
                            DirectCast(oRow.FindControl("CHKisSelected"), CheckBox).Checked = True
                        End If
                    Next
                Next
            Else
                Me.MLVquestionari.SetActiveView(Me.VIWdati)
                caricaDati()
            End If

        End If
        'se e' un nuovo sondaggio si crea direttamente la domanda
        If (QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Sondaggio OrElse QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Meeting) AndAlso Me.QuestionarioCorrente.pagine(0).domande.Count = 0 Then
            Me.PaginaCorrenteID = DLPagine.DataKeys.Item(0)
            Me.DomandaCorrente = New Domanda
            Me.DomandaCorrente.id = 0
            Me.RedirectToUrl(RootObject.DomandaAdd)
        End If
    End Sub
    Public Overrides Sub BindNoPermessi()
        CTRLerrorMessage.Visible = True
        CTRLerrorMessage.InitializeControl(Resource.getValue("LBerrore.text"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
        Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)

    End Sub
    Public Overrides Function HasPermessi() As Boolean
        If MyBase.Servizio.Admin Then
            isAdmin = True
        End If
        Return (isAdmin Or MyBase.Servizio.GestioneDomande)
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        Try
            Select Case Me.QuestionarioCorrente.tipo
                Case Questionario.TipoQuestionario.LibreriaDiDomande
                    MyBase.SetCulture("pg_LibreriaDomandeEdit", "Questionari")
                Case Questionario.TipoQuestionario.Sondaggio
                    MyBase.SetCulture("pg_SondaggioEdit", "Questionari")
                Case Else
                    MyBase.SetCulture("pg_QuestionarioEdit", "Questionari")
            End Select
        Catch ex As Exception
            MyBase.SetCulture("pg_QuestionarioEdit", "Questionari")
        End Try
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLinkButton(LNBCartellaPrincipale, False, False)
            .setLinkButton(LNBGestioneQuestionario, False, False)
            .setLinkButton(LNBAggiungiPagina, False, False)
            .setLabel(LBTitolo)
            '.setLabel(LBMessage)
            .setLabel(LBHelp)
            .setImageButton(IMBHelp, False, False, True, False)
            .setButton(BTNCopiaEEdita)
            .setButton(BTNCancellaRisposte, False, True, True, False)
            .setButton(BTNreadOnly)
            .setButton(BTNConferma)
            .setLabel(LBCopiaQuestionario)
            .setLabel(LBCancellaRisposte)
            .setLabel(LBreadOnly)
            .setLabel(LBIsRisposte)
            If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Modello Then
                LNBGestioneQuestionario.Text = .getValue("LNBGestioneModello")
            End If
            .setHeaderGridView(Me.GRVLibrerie, 1, "headerNome", True)
            .setHeaderGridView(Me.GRVLibrerie, 2, "headerDiffBassa", True)
            .setHeaderGridView(Me.GRVLibrerie, 3, "headerDiffMedia", True)
            .setHeaderGridView(Me.GRVLibrerie, 4, "headerDiffAlta", True)
            .setHeaderGridView(Me.GRVLibrerie, 0, "headerSeleziona", True)
            .setButton(BTNAggiungiLibreria)
            .setButton(BTNSelezionaLibrerie)
            .setButton(BTNSalvaLibrerie)
            .setLabel(LBMessaggioLibrerie)
            .setLabel(LBHeaderNome)
            .setLabel(LBHeaderDiffBassa)
            .setLabel(LBHeaderDiffMedia)
            .setLabel(LBHeaderDiffAlta)
            .setLabel(LBHeaderElimina)
            .setLabel(LBMessaggioErrore)
            Select Case qs_questIdType
                Case Questionario.TipoQuestionario.Random, Questionario.TipoQuestionario.RandomRepeat
                    Master.ServiceTitle = .getValue("ServiceTitle.Questions.FromLibrary")
                Case Else
                    Master.ServiceTitle = .getValue("ServiceTitle.Questions")
            End Select
        End With
        '   SetServiceTitle(Master)
    End Sub
    Public Sub SetInternazionalizzazioneDLPagine(ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)
        With Me.Resource
            .setImageButton(e.Item.FindControl("IMBPagina"), False, True, True, False)
            .setImageButton(e.Item.FindControl("IMBEliminaPag"), False, True, True, True)
            .setImageButton(e.Item.FindControl("IMBLibrary"), False, True, True, False)
            .setLabel(e.Item.FindControl("LBScegliAzione"))
            .setLinkButton(e.Item.FindControl("LNBConfermaAzione"), False, False)
            .setLinkButton(e.Item.FindControl("LNBConfermaAzioneBottom"), False, False)
            .setLinkButton(e.Item.FindControl("LNBCopiaDomande"), False, False)
            .setLabel(e.Item.FindControl("LBCopia"))
            DirectCast(e.Item.FindControl("LBScegliAzioneBottom"), Label).Text = .getValue("LBScegliAzione.text")
            DirectCast(e.Item.FindControl("LNBEliminaSi"), LinkButton).Text = .getValue("LNBEliminaSi.text")
            DirectCast(e.Item.FindControl("LNBEliminaNo"), LinkButton).Text = .getValue("LNBEliminaNo.text")
            .setLabel(e.Item.FindControl("LBMessaggioElimina"))

            If Not DirectCast(e.Item.FindControl("DDLQuestionariDestinazione"), DropDownList).Items.Count > 0 Then
                DirectCast(e.Item.FindControl("LBCopia"), Label).Text = Me.Resource.getValue("LBCopiaNoQuestionari")
                DirectCast(e.Item.FindControl("DDLQuestionariDestinazione"), DropDownList).Visible = False
                DirectCast(e.Item.FindControl("LNBCopiaDomande"), LinkButton).Visible = False
            End If

            If qs_questIdType = Questionario.TipoQuestionario.Sondaggio Then
                DirectCast(e.Item.FindControl("DDLAzioni"), DropDownList).Items.Clear()
                Dim counter As Integer
                For counter = 0 To 2
                    Dim item1 As New ListItem
                    item1.Value = counter
                    item1.Text = .getValue(DirectCast(e.Item.FindControl("DDLAzioni"), DropDownList), counter)
                    DirectCast(e.Item.FindControl("DDLAzioni"), DropDownList).Items.Add(item1)
                    DirectCast(e.Item.FindControl("DDLAzioniBottom"), DropDownList).Items.Add(item1)
                Next
                DirectCast(e.Item.FindControl("RBLfiltraDomande"), RadioButtonList).Visible = False
            Else
                For Each item As ListItem In DirectCast(e.Item.FindControl("DDLAzioni"), DropDownList).Items
                    item.Text = .getValue(DirectCast(e.Item.FindControl("DDLAzioni"), DropDownList), item.Value)
                Next
                For Each item As ListItem In DirectCast(e.Item.FindControl("DDLAzioniBottom"), DropDownList).Items
                    item.Text = .getValue(DirectCast(e.Item.FindControl("DDLAzioni"), DropDownList), item.Value)
                Next
                For Each item As ListItem In DirectCast(e.Item.FindControl("RBLfiltraDomande"), RadioButtonList).Items
                    item.Text = .getValue(item.Value & ".RBLfiltraDomande")
                Next
                If Me.QuestionarioCorrente.isReadOnly Then
                    'se e' readonly vengono eliminate le opzioni di inserimento/importazione domande
                    Dim counter As Integer
                    For counter = 1 To 3
                        ' elimino i primi 3 item dalla drop down
                        DirectCast(e.Item.FindControl("DDLAzioni"), DropDownList).Items.RemoveAt(0)
                        DirectCast(e.Item.FindControl("DDLAzioniBottom"), DropDownList).Items.RemoveAt(0)
                    Next
                    ' elimino l'elimina domande dalla drop down
                    DirectCast(e.Item.FindControl("DDLAzioni"), DropDownList).Items.RemoveAt(2)
                    DirectCast(e.Item.FindControl("DDLAzioniBottom"), DropDownList).Items.RemoveAt(2)

                End If
            End If
        End With
    End Sub
    Public Sub SetInternazionalizzazioneDLDomande(ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)
        With Me.Resource
            .setImageButton(e.Item.FindControl("IMBEdit"), False, True, True, False)
            .setImageButton(e.Item.FindControl("IMBElimina"), False, True, True, False)
            .setImageButton(e.Item.FindControl("IMBSpostaSu"), False, True, True, False)
            .setImageButton(e.Item.FindControl("IMBSpostaGiu"), False, True, True, False)
            .setLabel(e.Item.FindControl("LBSelezionaDomanda"))
        End With
    End Sub
    Public Overrides ReadOnly Property isCompileForm() As Boolean
        Get
            isCompileForm = False
        End Get
    End Property

    Private Function HasPermissionByExternalObject() As Boolean
        Dim result As Boolean = False
        Dim oSourceObject As New lm.Comol.Core.DomainModel.ModuleObject()

        oSourceObject.ObjectTypeID = qs_ownerTypeId
        oSourceObject.ObjectLongID = qs_ownerId
        oSourceObject.ServiceCode = COL_BusinessLogic_v2.UCServices.Services_EduPath.Codex
        oSourceObject.CommunityID = ComunitaCorrenteID

        Dim oDestinationObject As New lm.Comol.Core.DomainModel.ModuleObject()
        oDestinationObject = lm.Comol.Core.DomainModel.ModuleObject.CreateLongObject(qs_questId, COL_BusinessLogic_v2.UCServices.Services_Questionario.ObjectType.Questionario, ComunitaCorrenteID, COL_BusinessLogic_v2.UCServices.Services_Questionario.Codex)

        result = allowStandardAction(lm.Comol.Core.DomainModel.StandardActionType.Edit, UtenteCorrente.ID, ComunitaCorrenteID, oSourceObject, oDestinationObject)
        Return result
    End Function
    Private Sub BTNCopiaEEdita_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNCopiaEEdita.Click
        Dim oGestioneDomande As New GestioneDomande
        Me.QuestionarioCorrente.isBloccato = True
        DALQuestionario.UpdateNome(Me.QuestionarioCorrente) 'sarebbe sufficiente una SP che fa update del solo isChiuso
        oGestioneQuest.copiaQuestionarioCorrente()
        Me.QuestionarioCorrente = DALQuestionario.readQuestionarioBYLingua(Me.PageUtility.CurrentContext, Me.QuestionarioCorrente.id, Me.LinguaQuestionario, False)
        Response.Redirect(RootObject.QuestionarioEditShort & "?" & qs_questType & Me.QuestionarioCorrente.tipo)
    End Sub
    Private Sub BTNCancellaRisposte_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNCancellaRisposte.Click

        If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Random OrElse Me.QuestionarioCorrente.tipo = QuestionnaireType.RandomMultipleAttempts OrElse Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Autovalutazione Then
            DALRisposte.cancellaRisposteBYIDQuestionarioRandom(Me.PageUtility.CurrentContext, Me.QuestionarioCorrente.id)
        Else
            DALRisposte.cancellaRisposteBYIDQuestionario(Me.QuestionarioCorrente.id, Me.UtenteCorrente.ID)
        End If
        oGestioneQuest.DeleteAllAnswersActionAdd()
        Response.Redirect(RootObject.QuestionarioEditShort & "?" & qs_questType & Me.QuestionarioCorrente.tipo)
    End Sub
    Private Sub BTNreadOnly_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNreadOnly.Click
        Me.QuestionarioCorrente.isReadOnly = True
        MLVquestionari.SetActiveView(Me.VIWdati)
        BindDati()
    End Sub
    Public Overrides Sub SetControlliByPermessi()
        If (Me.qs_questIdType = QuestionnaireType.QuestionLibrary OrElse Me.qs_questIdType = QuestionnaireType.AutoEvaluation OrElse Me.qs_questIdType = QuestionnaireType.Random OrElse Me.qs_questIdType = QuestionnaireType.RandomMultipleAttempts OrElse Me.qs_questIdType = QuestionnaireType.Poll) Then
            LNBAggiungiPagina.Visible = False
        Else
            LNBAggiungiPagina.Visible = (MyBase.Servizio.Admin Or MyBase.Servizio.GestioneDomande) And Not Me.QuestionarioCorrente.isReadOnly
        End If
        LNBGestioneQuestionario.Visible = MyBase.Servizio.Admin Or MyBase.Servizio.CopiaQuestionario Or MyBase.Servizio.QuestionariSuInvito
        If Me.qs_questIdType = Questionario.TipoQuestionario.Autovalutazione Then
            BTNAggiungiLibreria.Visible = False
            BTNConferma.Visible = True
        End If
        If Me.QuestionarioCorrente.ownerType = OwnerType_enum.EduPathSubActivity OrElse Me.QuestionarioCorrente.ownerType = OwnerType_enum.EduPathActivity Then
            LNBCartellaPrincipale.Visible = False
        End If
    End Sub
    Private Sub readFields()
        For Each oItem As RepeaterItem In RPTLibrerieQuestionario.Items
            Me.QuestionarioCorrente.librerieQuestionario(oItem.ItemIndex).idQuestionario = Me.QuestionarioCorrente.id
            Me.QuestionarioCorrente.librerieQuestionario(oItem.ItemIndex).idLibreria = Integer.Parse(DirectCast(oItem.FindControl("LBIDLibreriaQuest"), Label).Text)
            Me.QuestionarioCorrente.librerieQuestionario(oItem.ItemIndex).nDomandeDiffBassa = RootObject.parseInt(DirectCast(oItem.FindControl("TXBDiffBassa"), TextBox).Text)
            Me.QuestionarioCorrente.librerieQuestionario(oItem.ItemIndex).nDomandeDiffMedia = RootObject.parseInt(DirectCast(oItem.FindControl("TXBDiffMedia"), TextBox).Text)
            Me.QuestionarioCorrente.librerieQuestionario(oItem.ItemIndex).nDomandeDiffAlta = RootObject.parseInt(DirectCast(oItem.FindControl("TXBDiffAlta"), TextBox).Text)
        Next
    End Sub

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Master.ShowDocType = True
    End Sub
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Select Case Request.QueryString("confirm")
            Case "1"
                CTRLmessages.InitializeControl(Resource.getValue("MSGconfermaCopiaQuestionario"), lm.Comol.Core.DomainModel.Helpers.MessageType.success)
                CTRLmessages.Visible = True
            Case "2"
                CTRLmessages.InitializeControl(Resource.getValue("MSGconfermaCopiaLibreria"), lm.Comol.Core.DomainModel.Helpers.MessageType.success)
                CTRLmessages.Visible = True
            Case Else
                CTRLmessages.Visible = False
        End Select

    End Sub
    Private Sub RPTLibrerieQuestionario_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTLibrerieQuestionario.ItemDataBound
        DirectCast(e.Item.FindControl("TXBDiffBassa"), TextBox).Enabled = Not Me.QuestionarioCorrente.isReadOnly AndAlso Not HasUserResponses
        DirectCast(e.Item.FindControl("TXBDiffMedia"), TextBox).Enabled = Not Me.QuestionarioCorrente.isReadOnly AndAlso Not HasUserResponses
        DirectCast(e.Item.FindControl("TXBDiffAlta"), TextBox).Enabled = Not Me.QuestionarioCorrente.isReadOnly AndAlso Not HasUserResponses
    End Sub

#Region "Librerie"
    Protected Sub BTNSelezionaLibrerie_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNSelezionaLibrerie.Click
        Page.Validate()
        If Not Page.IsValid Then
            LBMessaggioErrore.Visible = True
        Else
            LBMessaggioErrore.Visible = False
            Me.MLVquestionari.SetActiveView(Me.VIWSelezionaLibrerie)
            readFields()
            listaLibrerie = DALQuestionario.readLibrerieByComunita(PageUtility.CurrentContext, Me.ComunitaCorrenteID, QuestionarioCorrente.idLingua)
            GRVLibrerie.DataSource = listaLibrerie
            GRVLibrerie.DataBind()
            For Each oRow As GridViewRow In GRVLibrerie.Rows
                For Each oLib As LibreriaQuestionario In Me.QuestionarioCorrente.librerieQuestionario
                    If oLib.idLibreria = GRVLibrerie.DataKeys(oRow.RowIndex).Value Then
                        DirectCast(oRow.FindControl("CHKisSelected"), CheckBox).Checked = True
                    End If
                Next
            Next
        End If
    End Sub
    Protected Sub GRVLibrerie_PageIndexChanging(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GRVLibrerie.PageIndexChanging
        GRVLibrerie.PageIndex = e.NewPageIndex
        GRVLibrerie.DataSource = Me.listaLibrerie
        GRVLibrerie.DataBind()
    End Sub
    Private Sub RPTLibrerieQuestionario_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTLibrerieQuestionario.ItemCommand
        If e.CommandName = "elimina" Then
            Dim idL As String = DirectCast(RPTLibrerieQuestionario.Items(e.Item.ItemIndex).FindControl("LBIDLibreriaQuest"), Label).Text

            LibreriaQuestionario.removeLibreriaQuestionariBYIDLibreria(Me.QuestionarioCorrente.librerieQuestionario, idL)

            RPTLibrerieQuestionario.DataSource = Me.QuestionarioCorrente.librerieQuestionario
            RPTLibrerieQuestionario.DataBind()
        End If
    End Sub


    Private Sub BTNConferma_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNConferma.Click
        listaLibrerie.Clear()
        'Dim items As New List(Of LibreriaQuestionario)
        For Each riga As GridViewRow In GRVLibrerie.Rows
            Dim library As New LibreriaQuestionario
            library.nomeLibreria = riga.Cells(1).Text
            library.idQuestionario = Me.QuestionarioCorrente.id
            library.idLibreria = GRVLibrerie.DataKeys(riga.RowIndex).Value
            library.nDomandeDiffAlta = Integer.MinValue
            library.nDomandeDiffBassa = Integer.MinValue
            library.nDomandeDiffMedia = Integer.MinValue
            'If QuestionarioCorrente.tipo = QuestionnaireType.AutoEvaluation Then
            '    library.nDomandeDiffBassaDisponibili = riga.Cells(2).Text 'listaLibrerie(riga.RowIndex).nDomandeDiffBassa
            '    library.nDomandeDiffMediaDisponibili = riga.Cells(3).Text 'listaLibrerie(riga.RowIndex).nDomandeDiffMedia
            '    library.nDomandeDiffAltaDisponibili = riga.Cells(4).Text 'list
            'End If
            Dim idLingua As Integer = Me.PageUtility.LinguaID

            Dim oLiteral As Literal = DirectCast(riga.FindControl("LTidLanguage"), Literal)
            If Not String.IsNullOrEmpty(oLiteral.Text) AndAlso IsNumeric(oLiteral.Text) Then
                library.idLingua = CInt(oLiteral.Text)
            End If

            Dim idExists As Boolean = Me.QuestionarioCorrente.librerieQuestionario.Where(Function(l) l.idLibreria = library.idLibreria).Any()
            If Not DirectCast(riga.Cells(0).FindControl("CHKisSelected"), CheckBox).Checked Then
                Me.QuestionarioCorrente.librerieQuestionario = Me.QuestionarioCorrente.librerieQuestionario.Where(Function(l) Not (l.idLibreria = library.idLibreria AndAlso l.idLingua = library.idLingua)).ToList()
            ElseIf idExists Then
                If Not Me.QuestionarioCorrente.librerieQuestionario.Where(Function(l) l.idLibreria = library.idLibreria AndAlso library.idLingua = QuestionarioCorrente.idLingua).Any() Then
                    Me.QuestionarioCorrente.librerieQuestionario = Me.QuestionarioCorrente.librerieQuestionario.Where(Function(l) Not (l.idLibreria = library.idLibreria AndAlso l.idLingua <> library.idLingua)).ToList()
                    Me.QuestionarioCorrente.librerieQuestionario.Add(library)
                End If
            Else
                Me.QuestionarioCorrente.librerieQuestionario.Add(library)
            End If
            'If DirectCast(riga.Cells(0).FindControl("CHKisSelected"), CheckBox).Checked Then
            '    If Not idExists Then
            '        Me.QuestionarioCorrente.librerieQuestionario.Add(nuovaLibreria)
            '    End If
            'Else
            '    If idExists Then
            '        LibreriaQuestionario.removeLibreriaQuestionariBYIDLibreria(Me.QuestionarioCorrente.librerieQuestionario, nuovaLibreria.idLibreria)
            '    End If
            'End If
            '   items.Add(library)
        Next

        DALQuestionario.QuestionarioLibrerie_Delete(Me.QuestionarioCorrente.id)
        DALQuestionario.QuestionarioLibreria_Insert(Me.QuestionarioCorrente.librerieQuestionario)
        'creaTabellaLibrerie()
        If Me.QuestionarioCorrente.tipo = Me.QuestionarioCorrente.TipoQuestionario.Autovalutazione Then
            Me.RedirectToUrl(RootObject.QuestionarioAdmin + "?" & qs_questType + Me.QuestionarioCorrente.tipo.ToString())
        End If


    End Sub
    Protected Sub BTNSalvaLibrerie_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNSalvaLibrerie.Click

        Page.Validate()
        Dim nDomandeSelezionate As Integer = 0

        If Not Page.IsValid Then
            LBMessaggioErrore.Visible = True
        Else
            LBMessaggioErrore.Visible = False
            readFields()
            For Each oLib As LibreriaQuestionario In Me.QuestionarioCorrente.librerieQuestionario
                nDomandeSelezionate = nDomandeSelezionate + oLib.nDomandeTotali
            Next

            If nDomandeSelezionate > 0 Then
                DALQuestionario.QuestionarioLibrerie_Delete(Me.QuestionarioCorrente.id)
                DALQuestionario.QuestionarioLibreria_Insert(Me.QuestionarioCorrente.librerieQuestionario)

                Me.RedirectToUrl(RootObject.QuestionarioAdmin + "?" & qs_questType + Me.QuestionarioCorrente.tipo.ToString())
            Else
                LBMessaggioErrore.Text = Me.Resource.getValue("msgNessunaDomandaSelezionata")
                LBMessaggioErrore.Visible = True
            End If
        End If

    End Sub
    Protected Sub BTNAggiungiLibreria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNAggiungiLibreria.Click

        Me.MLVquestionari.SetActiveView(Me.VIWImpostaLibrerie)

        listaLibrerie.Clear()
        For Each riga As GridViewRow In GRVLibrerie.Rows
            Dim library As New LibreriaQuestionario
            library.nomeLibreria = riga.Cells(1).Text
            library.nDomandeDiffBassaDisponibili = riga.Cells(2).Text 'listaLibrerie(riga.RowIndex).nDomandeDiffBassa
            library.nDomandeDiffMediaDisponibili = riga.Cells(3).Text 'listaLibrerie(riga.RowIndex).nDomandeDiffMedia
            library.nDomandeDiffAltaDisponibili = riga.Cells(4).Text 'listaLibrerie(riga.RowIndex).nDomandeDiffAlta
            library.idQuestionario = Me.QuestionarioCorrente.id
            library.idLibreria = GRVLibrerie.DataKeys(riga.RowIndex).Value


            Dim idLingua As Integer = Me.PageUtility.LinguaID
            Dim oLiteral As Literal = DirectCast(riga.FindControl("LTidLanguage"), Literal)
            If Not String.IsNullOrEmpty(oLiteral.Text) AndAlso IsNumeric(oLiteral.Text) Then
                library.idLingua = CInt(oLiteral.Text)
            End If

            Dim idExists As Boolean = Me.QuestionarioCorrente.librerieQuestionario.Where(Function(l) l.idLibreria = library.idLibreria).Any()
            If Not DirectCast(riga.Cells(0).FindControl("CHKisSelected"), CheckBox).Checked Then
                Me.QuestionarioCorrente.librerieQuestionario = Me.QuestionarioCorrente.librerieQuestionario.Where(Function(l) Not (l.idLibreria = library.idLibreria AndAlso l.idLingua = library.idLingua)).ToList()
            ElseIf idExists Then
                If Not Me.QuestionarioCorrente.librerieQuestionario.Where(Function(l) l.idLibreria = library.idLibreria AndAlso library.idLingua = QuestionarioCorrente.idLingua).Any() Then
                    Me.QuestionarioCorrente.librerieQuestionario = Me.QuestionarioCorrente.librerieQuestionario.Where(Function(l) Not (l.idLibreria = library.idLibreria AndAlso l.idLingua <> library.idLingua)).ToList()
                    Me.QuestionarioCorrente.librerieQuestionario.Add(library)
                End If
            Else
                Me.QuestionarioCorrente.librerieQuestionario.Add(library)
            End If
            'Dim nuovaLibreria As New LibreriaQuestionario
            'nuovaLibreria.nomeLibreria = riga.Cells(1).Text
            'nuovaLibreria.nDomandeDiffBassaDisponibili = riga.Cells(2).Text 'listaLibrerie(riga.RowIndex).nDomandeDiffBassa
            'nuovaLibreria.nDomandeDiffMediaDisponibili = riga.Cells(3).Text 'listaLibrerie(riga.RowIndex).nDomandeDiffMedia
            'nuovaLibreria.nDomandeDiffAltaDisponibili = riga.Cells(4).Text 'listaLibrerie(riga.RowIndex).nDomandeDiffAlta
            'nuovaLibreria.idQuestionario = Me.QuestionarioCorrente.id
            'nuovaLibreria.idLibreria = GRVLibrerie.DataKeys(riga.RowIndex).Value

            'Dim idExists As Boolean = Me.QuestionarioCorrente.librerieQuestionario.Where(Function(l) l.idLibreria = nuovaLibreria.idLibreria).Any()

            ''If Not LibreriaQuestionario.findLibreriaQuestionariBYIDLibreria(Me.QuestionarioCorrente.librerieQuestionario, nuovaLibreria.idLibreria) Is Nothing Then
            ''    idExists = True
            ''End If

            'If DirectCast(riga.Cells(0).FindControl("CHKisSelected"), CheckBox).Checked Then
            '    If Not idExists Then
            '        Me.QuestionarioCorrente.librerieQuestionario.Add(nuovaLibreria)
            '    End If
            'Else
            '    If idExists Then
            '        LibreriaQuestionario.removeLibreriaQuestionariBYIDLibreria(Me.QuestionarioCorrente.librerieQuestionario, nuovaLibreria.idLibreria)
            '    End If
            'End If
        Next

        RPTLibrerieQuestionario.DataSource = Me.QuestionarioCorrente.librerieQuestionario
        RPTLibrerieQuestionario.DataBind()


        'creaTabellaLibrerie()

    End Sub
#End Region

    Public Overrides ReadOnly Property LoadDataByUrl As Boolean
        Get
            Return False
        End Get
    End Property
    Public Sub AddAction(idItem As Integer, ByVal oType As COL_Questionario.ModuleQuestionnaire.ObjectType, ByVal action As COL_Questionario.ModuleQuestionnaire.ActionType, type As QuestionnaireType)
        Select Case type
            Case QuestionnaireType.QuestionLibrary
                Select Case action
                    Case ModuleQuestionnaire.ActionType.QuestionAdd
                        AddAction(idItem, oType, ModuleQuestionnaire.ActionType.QuestionAddToLibrary)
                    Case ModuleQuestionnaire.ActionType.QuestionDelete
                        AddAction(idItem, oType, ModuleQuestionnaire.ActionType.QuestionDeleteFromLibrary)
                    Case ModuleQuestionnaire.ActionType.QuestionEdit
                        AddAction(idItem, oType, ModuleQuestionnaire.ActionType.QuestionEditFromLibrary)
                    Case ModuleQuestionnaire.ActionType.QuestionVirtualRemove
                        AddAction(idItem, oType, ModuleQuestionnaire.ActionType.QuestionVirtualRemoveFromLibrary)
                    Case Else
                        AddAction(idItem, oType, action)
                End Select
            Case Else
                AddAction(idItem, oType, action)
        End Select
    End Sub
    Public Sub AddAction(idItem As Integer, ByVal oType As COL_Questionario.ModuleQuestionnaire.ObjectType, ByVal action As COL_Questionario.ModuleQuestionnaire.ActionType)
        Me.PageUtility.AddAction(action, PageUtility.CreateObjectsList(oType, idItem), lm.ActionDataContract.InteractionType.UserWithLearningObject)
    End Sub

End Class