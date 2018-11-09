Imports COL_Questionario

Partial Public Class ucDaySurvey
    Inherits BaseControlQuestionario
    Implements COL_Questionario.IviewDaySurvey


    Private _SmartTagsAvailable As SmartTags
    Dim oGestioneDomande As New GestioneDomande
    Dim oGestioneRisposte As New GestioneRisposte
    Public Shared iDom As Integer
    Public Shared _iPag As Integer
    Public Shared startTime As DateTime
    Public Shared isFirstRun As Boolean
    Public Shared isCorrezione As Boolean
    Dim oPagedDataSource As New PagedDataSource
    Dim bindDone As Boolean
    Public Shared idCollection As New List(Of Integer)
    Private _Presenter As COL_Questionario.PresenterDaySurvey


    Public ReadOnly Property isCompileForm() As Boolean
        Get
            isCompileForm = True
        End Get
    End Property

    Public ReadOnly Property isAutovalutazione() As Boolean
        Get
            If Me.CurrentSurvey.tipo = Questionario.TipoQuestionario.Autovalutazione Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property
    Public ReadOnly Property SmartTagsAvailable() As Comol.Entity.SmartTags
        Get
            If _SmartTagsAvailable Is Nothing Then
                _SmartTagsAvailable = ManagerConfiguration.GetSmartTags(Me.ApplicationUrlBase())
            End If
            Return _SmartTagsAvailable
        End Get
    End Property

    Private Property iPag() As Integer
        Get
            Return ViewState("_iPag")
        End Get
        Set(ByVal value As Integer)
            ViewState("_iPag") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ucDaySurvey", "Questionari")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        LNBOtherSurveys1.Text = Me.Resource.getValue("LNBAltriSondaggi")
        LNBOtherSurveys2.Text = Me.Resource.getValue("LNBAltriSondaggi")
        BTNFine.Text = Me.Resource.getValue("Invia")
    End Sub

    Public ReadOnly Property CurrentCommunity() As Comol.Entity.Community Implements COL_Questionario.IviewDaySurvey.CurrentCommunity
        Get
            Return New Community(Me.ComunitaCorrenteID)
        End Get
    End Property

    Public ReadOnly Property CurrentUser() As Comol.Entity.Person Implements COL_Questionario.IviewDaySurvey.CurrentUser
        Get
            If MyBase.UtenteCorrente Is Nothing Then
                Dim oPersona As COL_Persona = COL_Persona.GetUtenteAnonimo(MyBase.UserSessionLanguage)
                If IsNothing(oPersona) Then
                    Return Nothing
                Else
                    Return New Person(oPersona.ID, oPersona.Nome, oPersona.Cognome)
                End If
            Else
                Return New Person(MyBase.UtenteCorrente.ID, MyBase.UtenteCorrente.Nome, MyBase.UtenteCorrente.Cognome)
            End If
        End Get
    End Property


    Protected Sub loadDomandeOpzioni(ByVal sender As Object, ByVal e As DataListItemEventArgs)

        Try
            iDom = e.Item.ItemIndex

            oGestioneDomande.loadDomandeOpzioni(Me.CurrentSurvey, iPag, iDom, False)
            DLPagine.Controls(0).FindControl("DLDomande").Controls(iDom).FindControl("PHOpzioni").Controls.Add(oGestioneDomande.loadDomandeOpzioni(Me.CurrentSurvey, iPag, iDom, False))
            Dim LBtestoDopoDomanda As New Label
            LBtestoDopoDomanda.Text = Me.CurrentSurvey.pagine(iPag).domande(iDom).testoDopo
            If Not LBtestoDopoDomanda.Text Is String.Empty Then
                Dim aCapo As New LiteralControl
                aCapo.Text = "<br>"
                DLPagine.Controls(0).FindControl("DLDomande").Controls(iDom).FindControl("PHOpzioni").Controls.Add(aCapo)
                DLPagine.Controls(0).FindControl("DLDomande").Controls(iDom).FindControl("PHOpzioni").Controls.Add(LBtestoDopoDomanda)
            End If
            If Questionario.TipoQuestionario.Autovalutazione = Me.CurrentSurvey.tipo Then
                DirectCast(DLPagine.Controls(0).FindControl("DLDomande").Controls(0).FindControl("LBSuggerimento"), Label).Text = Me.Resource.getValue("LBSuggerimento.text") & DirectCast(DLPagine.Controls(0).FindControl("DLDomande").Controls(0).FindControl("LBSuggerimento"), Label).Text
                DirectCast(DLPagine.Controls(0).FindControl("DLDomande").Controls(0).FindControl("LBsuggerimentoOpzione"), Label).Text = Me.Resource.getValue("LBsuggerimentoOpzione.text") & DirectCast(DLPagine.Controls(0).FindControl("DLDomande").Controls(0).FindControl("LBsuggerimentoOpzione"), Label).Text
            End If

        Catch ex As Exception
            'inviaMailErrore(ex)
        End Try

    End Sub
    Protected Sub DLPagine_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles DLPagine.ItemDataBound

        Try
            Dim dlDomande As New DataList
            dlDomande = DLPagine.Controls(0).FindControl("DLDomande")

            dlDomande.DataSource = Me.CurrentSurvey.pagine.Item(iPag).domande
            dlDomande.DataBind()
        Catch ex As Exception
            'inviaMailErrore(ex)
        End Try

    End Sub
    Public ReadOnly Property CurrentPresenter() As COL_Questionario.PresenterDaySurvey Implements COL_Questionario.IviewDaySurvey.CurrentPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New PresenterDaySurvey(Me)
            End If
            Return _Presenter
        End Get
    End Property
    Public Sub Init(ByVal ShowPreview As Boolean, ByVal SurveyType As COL_Questionario.Questionario.TipoQuestionario, ByVal Width As Integer, ByVal Height As Integer, ByVal SurveyLanguageId As Integer) Implements COL_Questionario.IviewDaySurvey.Init

    End Sub
    Public Sub BindDataList(ByVal oQuest As COL_Questionario.Questionario) Implements COL_Questionario.IviewDaySurvey.BindDataList
        If oQuest.domande.Count > 0 Then
            Me.MLVquestionari.SetActiveView(VIWdatalist)
            Me.CurrentSurvey = oQuest
            LBTitolo.Text = oQuest.nome
            DLPagine.DataSource = oQuest.pagine
            DLPagine.DataBind()
        End If
    End Sub
    Public Sub BindLinks(ByVal oQuestList As System.Collections.Generic.List(Of COL_Questionario.Questionario)) Implements COL_Questionario.IviewDaySurvey.BindLinks
        If oQuestList.Count = 0 Then
            divDaySurvey.Style("display") = "none"
        Else
            divDaySurvey.Style("display") = "block"
            Me.MLVquestionari.SetActiveView(VIWlink)
            DLLinks.DataSource = oQuestList
            DLLinks.DataBind()
        End If
    End Sub
    Public Sub BindPollResults(ByVal oQuest As COL_Questionario.Questionario) Implements COL_Questionario.IviewDaySurvey.BindPollResults
        Me.MLVquestionari.SetActiveView(VIWRisultati)
        Me.CurrentSurvey = oQuest
        DLDomandeRisultati.DataSource = oQuest.domande
        DLDomandeRisultati.DataBind()
    End Sub
    Protected Sub loadRisposteOpzioni(ByVal sender As Object, ByVal e As DataListItemEventArgs)

        iDom = e.Item.ItemIndex

        Dim tabellaRisposte As New Table
        tabellaRisposte = oGestioneRisposte.loadTabellaRisposte(Me.CurrentSurvey.domande(iDom))
        tabellaRisposte.Width = Me.Width
        DLDomandeRisultati.Controls(iDom).FindControl("PHOpzioni").Controls.Add(tabellaRisposte)

    End Sub
    Protected Sub BTNFine_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNFine.Click
        Try
            Dim isValida As Boolean = True
            Me.QuestionarioCorrente = Me.CurrentSurvey

            Dim ObbligatorieSaltate As Integer = 0

            Me.QuestionarioCorrente.rispostaQuest = oGestioneRisposte.getRisposte(DLPagine, isValida, ObbligatorieSaltate)




            If isValida Then
                Me.QuestionarioCorrente.rispostaQuest.ultimaRisposta = 0
                Me.QuestionarioCorrente.rispostaQuest.idQuestionario = Me.QuestionarioCorrente.id
                Me.QuestionarioCorrente.rispostaQuest.idUtenteInvitato = Me.Invito.ID
                Me.QuestionarioCorrente.rispostaQuest.idPersona = Me.UtenteCorrente.ID
                Me.QuestionarioCorrente.rispostaQuest.indirizzoIPStart = Request.ServerVariables("REMOTE_ADDRESS")
                Me.QuestionarioCorrente.rispostaQuest.dataModifica = Now()
                Me.QuestionarioCorrente.rispostaQuest.indirizzoIPEdit = OLDpageUtility.ProxyIPadress() & " / " & OLDpageUtility.ClientIPadress
                Me.QuestionarioCorrente.rispostaQuest.dataFine = Now()
                Me.QuestionarioCorrente.rispostaQuest.indirizzoIPEnd = OLDpageUtility.ProxyIPadress() & " / " & OLDpageUtility.ClientIPadress
                Me.QuestionarioCorrente.rispostaQuest.idQuestionarioRandom = Me.QuestionarioCorrente.idFiglio

                Dim oGestioneRisposte As New GestioneRisposte
                oGestioneRisposte.SalvaRisposta(Me.QuestionarioCorrente, UserId, True)
                If Me.QuestionarioCorrente.tipo = COL_Questionario.Questionario.TipoQuestionario.Sondaggio And Me.QuestionarioCorrente.visualizzaRisposta Then
                    Response.Redirect(RootObject.QuestionarioStatisticheGeneraliShort + "&comp=1")
                End If
            Else

                Me.QuestionarioCorrente = Nothing
                BindDataList(Me.CurrentSurvey)
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Protected Sub DLLinksCommand(ByVal sender As Object, ByVal e As DataListCommandEventArgs)

        If e.CommandName = "OtherSurveys" Then
            If Me.CurrentSurvey Is Nothing Then
                Me.CurrentSurvey = New Questionario
                Me.CurrentSurvey.tipo = Me.SurveyType
                Me.CurrentSurvey.id = DLLinks.DataKeys.Item(e.Item.ItemIndex)
            End If
            If Me.NumberOfDaySurveys > 1 Then
                Me.CurrentPresenter.CountCommunitySurveys()
                'RedirectToUrl(RootObject.QuestionariList & "?type=" & Me.CurrentSurvey.tipo.ToString())
            Else
                RedirectToPath(1)
            End If
        End If

    End Sub
    Public Sub RedirectToPath(ByVal nQuest As Integer) Implements COL_Questionario.IviewDaySurvey.RedirectToPath
        If nQuest = 1 Then
            Me.CurrentSurvey.url = Me.EncryptedUrl(RootObject.compileUrl, "idq=" & Me.CurrentSurvey.id, SecretKeyUtil.EncType.Questionario)
            If Me.CurrentSurvey.rispostaQuest.id > 0 Then
                Me.QuestionarioCorrente = Me.CurrentSurvey
                Me.QuestionarioCorrente.url = Me.CurrentSurvey.url
                Me.RedirectToUrl(RootObject.QuestionarioStatisticheGenerali + "&comp=1")
            Else
                Response.Redirect(Me.CurrentSurvey.url)
            End If
        Else
            RedirectToUrl(RootObject.QuestionariList & "?type=" & Me.CurrentSurvey.tipo.ToString())
        End If
    End Sub
    Protected Sub RedirectToList()
        RedirectToUrl(RootObject.QuestionariList & "?type=" & Me.CurrentSurvey.tipo.ToString())
    End Sub
    Protected Sub LNBOtherSurveys1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBOtherSurveys1.Click
        RedirectToList()
    End Sub
    Protected Sub LNBOtherSurveys2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBOtherSurveys2.Click
        RedirectToList()
    End Sub
    Public Property Height() As Integer Implements COL_Questionario.IviewDaySurvey.Height
        Get
            Return Me.divDaySurvey.Style("Height")
        End Get
        Set(ByVal value As Integer)
            Me.divDaySurvey.Style("Height") = value
        End Set
    End Property
    Public Property NumberOfDaySurveys() As Integer Implements COL_Questionario.IviewDaySurvey.NumberOfDaySurveys
        Get
            Return Me.ViewState("NumberOfDaySurveys")
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("NumberOfDaySurveys") = value
        End Set
    End Property
    Public Property ShowPreview() As Boolean Implements COL_Questionario.IviewDaySurvey.ShowPreview
        Get
            Return Me.ViewState("ShowPreview")
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("ShowPreview") = value
        End Set
    End Property
    Public ReadOnly Property SurveyLanguageId() As Integer Implements COL_Questionario.IviewDaySurvey.SurveyLanguageId
        Get
            Return Me.LinguaID
        End Get
    End Property
    Public Property SurveyType() As Integer Implements COL_Questionario.IviewDaySurvey.SurveyType
        Get
            Return Me.ViewState("SurveyType")
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("SurveyType") = value
        End Set
    End Property
    Public Property Width() As Integer Implements COL_Questionario.IviewDaySurvey.Width
        Get
            Return Me.divDaySurvey.Style("Width")
        End Get
        Set(ByVal value As Integer)
            Me.divDaySurvey.Style("Width") = value
        End Set
    End Property
    Public Property CurrentSurvey() As COL_Questionario.Questionario Implements COL_Questionario.IviewDaySurvey.CurrentSurvey
        Get
            Return Me.ViewState("CurrentSurvey")
        End Get
        Set(ByVal value As COL_Questionario.Questionario)
            Me.ViewState("CurrentSurvey") = value
        End Set
    End Property
    Public Property ShowResults() As Boolean Implements COL_Questionario.IviewDaySurvey.ShowResults
        Get
            Return Me.ViewState("ShowResults")
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("ShowResults") = value
        End Set
    End Property
    Public Property IsActive() As Boolean Implements COL_Questionario.IviewDaySurvey.IsActive
        Get
            Return Me.ViewState("IsActive")
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("IsActive") = value
        End Set
    End Property
    Public ReadOnly Property LinkText() As String Implements COL_Questionario.IviewDaySurvey.LinkText
        Get
            Return Me.Resource.getValue("LinkDaySurveyText")
        End Get
    End Property
    Public Overrides Sub SetControlliByPermessi()
    End Sub


    ''' <summary>
    ''' Aggiunto per il salva!
    ''' Risulta NECESSARIO VERIFICARE che le risposte SALVATE siano effettivamente
    ''' quelle dell'utente corrente, per EVITARE sovrascritture o cancellazioni incongrue.
    ''' </summary>
    ''' <returns></returns>
    Private ReadOnly Property UserId As Integer
        Get
            Return Me.UtenteCorrente.ID
        End Get
    End Property
End Class
