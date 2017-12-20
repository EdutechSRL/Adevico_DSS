Imports lm.Comol.Modules.Base.Presentation.TaskList
Imports lm.Comol.UI.Presentation
Imports lm.ActionDataContract
Imports lm.Comol.Core.DomainModel
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.Comunita

Partial Public Class CreateUpdateTask
    Inherits PageBase
    Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewCreateUpdateTask


#Region "Base Context"
    Private _presenter As CreateUpdateTaskPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Servizio As Services_TaskList
    Private _BaseUrl As String

    Public Overloads ReadOnly Property BaseUrl() As String
        Get
            If _BaseUrl = "" Then
                _BaseUrl = Me.PageUtility.BaseUrl
            End If
            Return _BaseUrl
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As CreateUpdateTaskPresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New CreateUpdateTaskPresenter(Me.CurrentContext, Me)
            End If
            Return _presenter
        End Get
    End Property
    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property

    Private ReadOnly Property CurrentService() As Services_TaskList
        Get
            If IsNothing(_Servizio) Then
                If isPortalCommunity Then
                    Dim PersonTypeID As Integer = Me.TipoPersonaID
                    _Servizio = Services_TaskList.Create
                    With _Servizio
                        .AddCommunityProject = False
                        .AddPersonalProject = (PersonTypeID <> Main.TipoPersonaStandard.Guest)
                        .Administration = (PersonTypeID = Main.TipoPersonaStandard.AdminSecondario OrElse PersonTypeID = Main.TipoPersonaStandard.SysAdmin)
                        .ManagementPermission = (PersonTypeID = Main.TipoPersonaStandard.AdminSecondario OrElse PersonTypeID = Main.TipoPersonaStandard.SysAdmin)
                        .ViewCommunityProjects = (PersonTypeID <> Main.TipoPersonaStandard.Guest)

                    End With
                ElseIf Me.isModalitaAmministrazione Then 'And Me.isUtenteAnonimo 
                    _Servizio = New Services_TaskList(COL_Comunita.GetPermessiForServizioByCode(Main.TipoRuoloStandard.AdminComunità, Me.AmministrazioneComunitaID, Services_TaskList.Codex))
                Else
                    _Servizio = Me.PageUtility.GetCurrentServices.Find(Services_TaskList.Codex)
                    If IsNothing(_Servizio) Then
                        _Servizio = Services_TaskList.Create
                    End If
                End If
            End If
            Return _Servizio
        End Get
    End Property

    Private Function TranslateComolPermissionToModulePermission(ByVal oService As Services_TaskList) As ModuleTaskList
        Dim oModulePermission As New ModuleTaskList
        With oService
            'oModulePermission.DeleteMessage = .Admin OrElse .Write
            'oModulePermission.EditMessage = .Admin OrElse .Write
            'oModulePermission.ManagementPermission = .GrantPermission
            'oModulePermission.PrintMessage = .Read OrElse .Write OrElse .Admin
            'oModulePermission.RetrieveOldMessage = .Write OrElse .Admin
            'oModulePermission.ServiceAdministration = .Admin OrElse .Write
            'oModulePermission.ViewCurrentMessage = .Read OrElse .Write OrElse .Admin
            'oModulePermission.ViewOldMessage = .Read OrElse .Write OrElse .Admin
        End With
        Return oModulePermission
    End Function

#End Region


    Public Sub Init() Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewCreateUpdateTask.Init
        Me.CTRLaddAss.CurrentPresenter.InitView(2509)
        Me.WZDprova.ActiveStepIndex = 1
    End Sub



#Region "implementare x forza"
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides Sub BindDati()
        Me.Master.ShowNoPermission = False
        If Not Me.IsPostBack Then
            Me.CurrentPresenter.InitView()
        End If
        'If Page.IsPostBack = False Then
        '    Me.CurrentPresenter.InitView()
        '    'Non ho capito bene a cosa serve questa cosa qui sotto....
        '    Me.PageUtility.AddAction(Services_TaskList.ActionType.AddTaskList, Nothing, InteractionType.UserWithLearningObject)
        'End If
    End Sub

    Public Overrides Sub BindNoPermessi()
        Me.Master.ShowNoPermission = True
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_AssignedTasks.xml", "TaskList")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            Me.Master.ServiceTitle = .getValue("serviceTitle")
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get

        End Get
    End Property

#End Region


    Private Sub BTNa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNa.Click
        Me.CTRLaddAss.CurrentPresenter.SaveTaskAssignment()
    End Sub
End Class