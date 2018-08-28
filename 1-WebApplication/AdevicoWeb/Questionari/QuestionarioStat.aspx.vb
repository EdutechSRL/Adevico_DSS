Imports COL_Questionario

Partial Public Class QuestionarioStat
    Inherits PageBaseQuestionario

    Dim iDom As Integer
    Dim oGestioneRisposte As New GestioneRisposte
#Region "Context"
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = lm.Comol.UI.Presentation.SessionHelpers.CurrentUserContext, .DataContext = lm.Comol.UI.Presentation.SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
#End Region

#Region "Control"
    Private ReadOnly Property pageMode()
        Get
            Return Request.QueryString("mode")
        End Get
    End Property

    Private Property isCompiling()
        Get
            Return (Request.QueryString("comp") = "1" OrElse Request.QueryString("comp") = "2")
        End Get
        Set(ByVal value)
            ViewState("comp") = Request.QueryString("comp")
        End Set
    End Property
    Protected ReadOnly Property CookieName As String
        Get
            Return "QuestionarioStat_" '& LoaderGuid.ToString
        End Get
    End Property
    Protected ReadOnly Property DisplayMessageToken() As String
        Get
            Return Resource.getValue("DisplayStatisticsToken.Message")
        End Get
    End Property
    Protected ReadOnly Property DisplayTitleToken() As String
        Get
            Return Resource.getValue("DisplayStatisticsToken.Title")
        End Get
    End Property
    Protected Property AllowedStandardActionType() As lm.Comol.Core.DomainModel.StandardActionType
        Get
            Return ViewStateOrDefault("AllowedStandardActionType", lm.Comol.Core.DomainModel.StandardActionType.None)
        End Get
        Set(value As lm.Comol.Core.DomainModel.StandardActionType)
            ViewState("AllowedStandardActionType") = value
        End Set
    End Property




    'Protected Property LoaderGuid() As System.Guid
    '    Get
    '        Return ViewStateOrDefault("LoaderGuid", Guid.NewGuid)
    '    End Get
    '    Set(value As System.Guid)
    '        ViewState("LoaderGuid") = value
    '    End Set
    'End Property
#End Region
    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Master.ShowDocType = True
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'If Me.QuestionarioCorrente.id = 0 Then 'con l'hasPermessi "owned" non dovrebbe piu' servire
        '    Me.QuestionarioCorrente.id = Request.QueryString("idq")
        'End If
        Select Case Me.pageMode
            Case TipoStatistiche.Utenti
                If Not IsNothing(Me.QuestionarioCorrente) AndAlso Not String.IsNullOrEmpty(Me.QuestionarioCorrente.nome) AndAlso Me.QuestionarioCorrente.ownerType <> OwnerType_enum.None Then
                    Me.Master.ServiceTitle = Me.QuestionarioCorrente.nome
                    'If QuestionarioCorrente.tipo = QuestionnaireType.RandomMultipleAttempts OrElse QuestionarioCorrente.tipo = QuestionnaireType.Random Then
                    '    Me.Master.ServiceTitleToolTip = Resource.getValue("GeneralStatisticsTitle.Random")
                    'Else
                    Me.Master.ServiceTitleToolTip = GetServiceTitle()
                    '    End If
                End If
                addUCStatUtenti()
            Case TipoStatistiche.Utente
                addUCStatUtenti()
            Case TipoStatistiche.GeneraliNoData
                LBerrorMSG.Text = String.Format(Me.Resource.getValue("MSGNoDate"), Me.QuestionarioCorrente.dataFine.ToString)
                LBerrorMSG.Visible = True
                addUCStatGenerali()
            Case Else
                addUCStatGenerali()
        End Select
    End Sub

    Private Sub addUCStatGenerali()
        If QuestionarioCorrente.tipo = QuestionnaireType.RandomMultipleAttempts OrElse QuestionarioCorrente.tipo = QuestionnaireType.Random Then
            Me.Master.ServiceTitle = Resource.getValue("GeneralStatisticsTitle.Random")
        End If
        PHStat.Controls.Clear()
        Dim ctrl As New Control
        ctrl = Page.LoadControl(RootObject.ucStatisticheGenerali)
        ctrl.ID = "ucStatGenerali"
        PHStat.Controls.Add(ctrl)
    End Sub

    Private Sub addUCStatUtenti()
        'PNLmenu.Visible = False
        PHStat.Controls.Clear()
        Dim ctrl As ucStatisticheUtenti = Page.LoadControl(RootObject.ucStatisticheUtenti)
        ctrl.ID = "ucStatUtenti"
        ctrl.AllowedStandardActionType = AllowedStandardActionType
        AddHandler ctrl.GetBlockUIinfos, AddressOf GetBlockUIinfos
        PHStat.Controls.Add(ctrl)
    End Sub

    Public Sub GetBlockUIinfos(ByRef name As String, ByRef value As String)
        name = CookieName
        value = HDNdownloadTokenValue.Value
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm( _
   ByVal control As Control _
)

    End Sub

    Public Overrides Sub BindDati()

        Me.MLVquestionari.SetActiveView(Me.VIWdati)

    End Sub

    Public Overrides Sub BindNoPermessi()
        Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)

    End Sub

    Public Overrides Function HasPermessi() As Boolean
        If qs_ownerTypeId = OwnerType_enum.None Then
            If Not qs_questId < 1 Then
                'se il questionario con id in querystring non e' della comunita' corrente ritorna false
                Dim oQuest As New Questionario
                oQuest = DALQuestionario.readQuestionarioByPersona(PageUtility.CurrentContext, False, qs_questId, LinguaID, UtenteCorrente.ID, 0)
                'se il quest e' della com corrente...
                If DALQuestionarioGruppo.ComunitaByGruppo(oQuest.idGruppo) = ComunitaCorrenteID Then
                    '..e ho i permessi necessari nella comunita'
                    If Me.QuestionarioCorrente.tipo = COL_Questionario.Questionario.TipoQuestionario.Sondaggio OrElse Me.QuestionarioCorrente.tipo = COL_Questionario.Questionario.TipoQuestionario.Meeting Then
                        If MyBase.Servizio.Compila Then
                            'metto il quest in sessione e vado avanti
                            Me.QuestionarioCorrente = oQuest
                            Return True
                        End If
                    Else
                        If (MyBase.Servizio.Admin OrElse MyBase.Servizio.GestioneDomande OrElse MyBase.Servizio.VisualizzaStatistiche) Then
                            'metto il quest in sessione e vado avanti
                            Me.QuestionarioCorrente = oQuest
                            Return True
                        End If
                    End If

                    'altrimenti nego i permessi
                    Return False
                Else
                    'altrimenti nego i permessi
                    Return False
                End If
            Else
                If Me.QuestionarioCorrente.tipo = COL_Questionario.Questionario.TipoQuestionario.Sondaggio OrElse Me.QuestionarioCorrente.tipo = COL_Questionario.Questionario.TipoQuestionario.Meeting Then
                    Return MyBase.Servizio.Compila
                Else
                    Return (MyBase.Servizio.Admin Or MyBase.Servizio.GestioneDomande Or MyBase.Servizio.VisualizzaStatistiche)
                End If
            End If
        Else
            Dim oDestinationObject As New lm.Comol.Core.DomainModel.ModuleObject()
            oDestinationObject = lm.Comol.Core.DomainModel.ModuleObject.CreateLongObject(qs_questId, COL_BusinessLogic_v2.UCServices.Services_Questionario.ObjectType.Questionario, ComunitaCorrenteID, COL_BusinessLogic_v2.UCServices.Services_Questionario.Codex)
            Dim oSourceObject As New lm.Comol.Core.DomainModel.ModuleObject()
            oSourceObject = lm.Comol.Core.DomainModel.ModuleObject.CreateLongObject(qs_ownerId, qs_ownerTypeId, ComunitaCorrenteID, COL_BusinessLogic_v2.UCServices.Services_EduPath.Codex)
            If qs_questId < 1 Then
                Return False
            ElseIf pageMode = TipoStatistiche.Utente Then
                If qs_PersonaId = 0 OrElse CurrentContext.UserContext.CurrentUserID = qs_PersonaId Then


                    'If allowStandardAction(lm.Comol.Core.DomainModel.StandardActionType.ViewPersonalStatistics, UtenteCorrente.ID, 0, oSourceObject, oDestinationObject) Then
                    '    Dim oQuest As New Questionario
                    '    oQuest = DALQuestionario.readQuestionarioByPersona(PageUtility.CurrentContext, False, qs_questId, LinguaID, IIf(qs_PersonaId = 0, Me.UtenteCorrente.ID, qs_PersonaId), 0)
                    '    Me.QuestionarioCorrente = oQuest
                    '    AllowedStandardActionType = lm.Comol.Core.DomainModel.StandardActionType.ViewPersonalStatistics
                    '    Return True
                    'Else : Return False
                    'End If

                    Dim oQuest As New Questionario
                    oQuest = DALQuestionario.readQuestionarioByPersona(PageUtility.CurrentContext, False, qs_questId, LinguaID, IIf(qs_PersonaId = 0, Me.UtenteCorrente.ID, qs_PersonaId), 0)
                    Me.QuestionarioCorrente = oQuest

                    If allowStandardAction(lm.Comol.Core.DomainModel.StandardActionType.ViewPersonalStatistics, UtenteCorrente.ID, 0, oSourceObject, oDestinationObject) Then
                        AllowedStandardActionType = lm.Comol.Core.DomainModel.StandardActionType.ViewPersonalStatistics
                        Return True
                    ElseIf oQuest.DisplayScoreToUser Then
                        Return True
                    Else Return False
                    End If

                ElseIf allowStandardAction(lm.Comol.Core.DomainModel.StandardActionType.ViewUserStatistics, UtenteCorrente.ID, 0, oSourceObject, oDestinationObject) Then
                    Dim oQuest As New Questionario
                    oQuest = DALQuestionario.readQuestionarioByPersona(Me.PageUtility.CurrentContext, False, qs_questId, LinguaID, IIf(qs_PersonaId = 0, Me.UtenteCorrente.ID, qs_PersonaId), 0)
                    Me.QuestionarioCorrente = oQuest
                    AllowedStandardActionType = lm.Comol.Core.DomainModel.StandardActionType.ViewUserStatistics
                    Return True
                Else : Return False
                End If
            ElseIf pageMode = TipoStatistiche.Utenti OrElse pageMode = TipoStatistiche.Generali Then
                If allowStandardAction(lm.Comol.Core.DomainModel.StandardActionType.ViewUserStatistics, UtenteCorrente.ID, 0, oSourceObject, oDestinationObject) Then
                    Dim oQuest As New Questionario
                    oQuest = DALQuestionario.readQuestionarioBYLingua(Me.PageUtility.CurrentContext, qs_questId, LinguaID, False, True)
                    Me.QuestionarioCorrente = oQuest
                    AllowedStandardActionType = lm.Comol.Core.DomainModel.StandardActionType.ViewUserStatistics
                    Return True
                Else : Return False
                End If
            End If
        End If
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_QuestionarioStat", "Questionari")

    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        '   SetServiceTitle(Master)
        'If QuestionarioCorrente.tipo = QuestionnaireType.RandomMultipleAttempts OrElse QuestionarioCorrente.tipo = QuestionnaireType.Random Then
        '    Me.Master.ServiceTitle = Resource.getValue("GeneralStatisticsTitle.Random")
        'Else
        Me.Master.ServiceTitle = GetServiceTitle()
        '    End If
    End Sub

    Public Overrides ReadOnly Property isCompileForm() As Boolean
        Get
            isCompileForm = False
        End Get
    End Property

    Public Overrides Sub SetControlliByPermessi()

    End Sub

    Private Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        Master.ShowDocType = True
        If isCompiling AndAlso isAnonymousCompiler Then
            MasterPageFile = BaseUrl & RootObject.MasterPageUI
        End If

    End Sub
    Public Overrides ReadOnly Property LoadDataByUrl As Boolean
        Get
            Return False
        End Get
    End Property
End Class