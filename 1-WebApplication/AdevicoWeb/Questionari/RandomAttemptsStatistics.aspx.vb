Imports COL_Questionario

Partial Public Class RandomAttemptsStatistics
    Inherits PageBaseQuestionario

    Dim iDom As Integer
    Dim oGestioneRisposte As New GestioneRisposte

#Region "Inherits"
    Public Overrides ReadOnly Property isCompileForm() As Boolean
        Get
            isCompileForm = False
        End Get
    End Property
    Public Overrides ReadOnly Property LoadDataByUrl As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Property"
    Private ReadOnly Property PreloadStatistics() As StatisticsType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of StatisticsType).GetByString(Request.QueryString("mode"), StatisticsType.MyAttempts)
        End Get
    End Property
    Private Property CurrentStatistics As StatisticsType
        Get
            Return ViewStateOrDefault("CurrentStatistics", StatisticsType.MyAttempts)
        End Get
        Set(value As StatisticsType)
            ViewState("CurrentStatistics") = value
        End Set
    End Property
    Private Property ManagePermission As Boolean
        Get
            Return ViewStateOrDefault("ManagePermission", False)
        End Get
        Set(value As Boolean)
            ViewState("ManagePermission") = value
        End Set
    End Property
    Protected ReadOnly Property CookieName As String
        Get
            Return "RandomAttemptsStatistics_" '& LoaderGuid.ToString
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
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
       
    End Sub
    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Master.ShowDocType = True
    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Me.MLVquestionari.SetActiveView(Me.VIWdati)
        Dim title As String = Resource.getValue("StatisticsTitle." & PreloadStatistics.ToString)

        Select Case CurrentStatistics
            Case StatisticsType.MyAttempts
                If Not IsNothing(Me.QuestionarioCorrente) AndAlso Not String.IsNullOrEmpty(Me.QuestionarioCorrente.nome) AndAlso (qs_PersonaId <> PageUtility.CurrentContext.UserContext.CurrentUserID) Then
                    title = String.Format(Resource.getValue("StatisticsTitle." & PreloadStatistics.ToString & "UQ"), Me.QuestionarioCorrente.nome, DALQuestionario.GetPersonName(PageUtility.CurrentContext, qs_PersonaId))
                End If
                Me.CTRLuserStatistics.Visible = True
                Me.CTRLuserStatistics.InitializeControl(qs_PersonaId, False, StatisticsType.MyAttempts)
            Case StatisticsType.UserAttempts
                If Not IsNothing(Me.QuestionarioCorrente) AndAlso Not String.IsNullOrEmpty(Me.QuestionarioCorrente.nome) AndAlso (qs_PersonaId <> PageUtility.CurrentContext.UserContext.CurrentUserID) Then
                    title = String.Format(Resource.getValue("StatisticsTitle." & PreloadStatistics.ToString & "UQ"), Me.QuestionarioCorrente.nome, DALQuestionario.GetPersonName(PageUtility.CurrentContext, qs_PersonaId))
                End If
                Me.CTRLuserStatistics.Visible = True
                Me.CTRLuserStatistics.InitializeControl(qs_PersonaId, ManagePermission, StatisticsType.UserAttempts)
            Case StatisticsType.User
                Me.CTRLuserStatistics.InitializeControl(qs_PersonaId, ManagePermission, StatisticsType.User)
                Me.CTRLuserStatistics.Visible = True
            Case Else
                Me.CTRLuserStatistics.Visible = False
        End Select
        Me.Master.ServiceTitle = Title
    End Sub
    Public Overrides Sub BindNoPermessi()
        Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Me.CurrentStatistics = PreloadStatistics
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
            ElseIf PreloadStatistics = StatisticsType.User OrElse ((PreloadStatistics = StatisticsType.UserAttempts OrElse PreloadStatistics = StatisticsType.MyAttempts) AndAlso PageUtility.CurrentContext.UserContext.CurrentUserID = qs_PersonaId) Then
                If qs_PersonaId = 0 OrElse PageUtility.CurrentContext.UserContext.CurrentUserID = qs_PersonaId Then
                    ManagePermission = allowStandardAction(lm.Comol.Core.DomainModel.StandardActionType.ViewUserStatistics, UtenteCorrente.ID, 0, oSourceObject, oDestinationObject)
                    If allowStandardAction(lm.Comol.Core.DomainModel.StandardActionType.ViewPersonalStatistics, UtenteCorrente.ID, 0, oSourceObject, oDestinationObject) Then
                        Dim oQuest As New Questionario
                        oQuest = DALQuestionario.readQuestionarioByPersona(PageUtility.CurrentContext, False, qs_questId, LinguaID, IIf(qs_PersonaId = 0, Me.UtenteCorrente.ID, qs_PersonaId), 0)
                        Me.QuestionarioCorrente = oQuest

                        Return True
                    Else : Return False
                    End If
                ElseIf allowStandardAction(lm.Comol.Core.DomainModel.StandardActionType.ViewUserStatistics, UtenteCorrente.ID, 0, oSourceObject, oDestinationObject) Then
                    Dim oQuest As New Questionario
                    oQuest = DALQuestionario.readQuestionarioByPersona(Me.PageUtility.CurrentContext, False, qs_questId, LinguaID, IIf(qs_PersonaId = 0, Me.UtenteCorrente.ID, qs_PersonaId), 0)
                    Me.QuestionarioCorrente = oQuest
                    Return True
                Else : Return False
                End If
            ElseIf PreloadStatistics.UserAttempts OrElse PreloadStatistics = StatisticsType.Generics Then
                If allowStandardAction(lm.Comol.Core.DomainModel.StandardActionType.ViewUserStatistics, UtenteCorrente.ID, 0, oSourceObject, oDestinationObject) Then
                    Dim oQuest As New Questionario
                    oQuest = DALQuestionario.readQuestionarioBYLingua(Me.PageUtility.CurrentContext, qs_questId, LinguaID, False, True)
                    Me.QuestionarioCorrente = oQuest
                    ManagePermission = True
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
        '  Me.Master.ServiceTitle = Resource.getValue("StatisticsTitle." & PreloadStatistics.ToString)
    End Sub
    Public Overrides Sub SetControlliByPermessi()

    End Sub
    Public Sub GetBlockUIinfos(ByRef name As String, ByRef value As String)
        name = CookieName
        value = HDNdownloadTokenValue.Value
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm( _
   ByVal control As Control _
)

    End Sub
#End Region

   
    Private Sub RandomAttemptsStatistics_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Master.ShowDocType = True
    End Sub
End Class