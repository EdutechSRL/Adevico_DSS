Imports lm.Comol.Modules.Base.Presentation.TaskList
Imports COL_BusinessLogic_v2.UCServices
Imports lm.ActionDataContract
Imports lm.Comol.UI.Presentation
Imports COL_BusinessLogic_v2.Comunita

'Imports Telerik.Web.UI
'Imports Telerik.Charting

Partial Public Class Gantt
    Inherits PageBase
    Implements IviewGantt



    Private _presenter As GanttPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Servizio As Services_TaskList
    Private _BaseUrl As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region " Base"


    Public Overloads ReadOnly Property BaseUrl() As String
        Get
            If _BaseUrl = "" Then
                _BaseUrl = Me.PageUtility.BaseUrl
            End If
            Return _BaseUrl
        End Get
    End Property
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get

        End Get
    End Property

    Public Overrides Sub BindDati()

        Me.Master.ShowNoPermission = False
        If Not IsPostBack Then
            Me.PageUtility.AddAction(Services_TaskList.ActionType.ViewGantt, Me.PageUtility.CreateObjectsList(Services_TaskList.ObjectType.Project, "-1"), InteractionType.UserWithLearningObject)
            Me.CurrentPresenter.InitView()
        End If
    End Sub

    Public Overrides Sub BindNoPermessi()
        Me.Master.ShowNoPermission = True
        Me.PageUtility.AddAction(Services_TaskList.ActionType.NoPermission, Nothing, InteractionType.UserWithLearningObject)
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Gantt", "TaskList")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource

            Me.Master.ServiceTitle = .getValue("serviceTitle")
            .setHyperLink(HYPreturnError, True, True)
            .setHyperLink(HYPprevius, True, True)
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get

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

    Public ReadOnly Property CurrentPresenter() As GanttPresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New GanttPresenter(Me.CurrentContext, Me)
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

#End Region



    Public Sub ShowError(ByVal ErrorString As String) Implements lm.Comol.Modules.Base.Presentation.TaskList.IviewGantt.ShowError
        Me.HYPreturnError.NavigateUrl = Me.BaseUrl & "TaskList/AssignedTasks.aspx"
        Me.LBerror.Text = ErrorString
        Me.PageUtility.AddAction(Services_TaskList.ActionType.NoPermission, Nothing, InteractionType.UserWithLearningObject)
        Me.MLVgantt.SetActiveView(Me.VIWerror)
    End Sub



    Public ReadOnly Property TaskID() As Long Implements lm.Comol.Modules.Base.Presentation.TaskList.IviewGantt.TaskID
        Get
            Try
                Return CType(Request.QueryString("TaskID"), Long)
            Catch ex As Exception
                Return 0
            End Try
        End Get
    End Property

    Public ReadOnly Property PreviusPageType() As lm.Comol.Modules.Base.Presentation.TaskList.IviewGantt.PageType Implements lm.Comol.Modules.Base.Presentation.TaskList.IviewGantt.PreviusPageType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of IviewGantt.PageType).GetByString(Request.QueryString("PreviusPage"), IviewGantt.PageType.None)
        End Get
    End Property

    Public Property ProjectID() As Long Implements lm.Comol.Modules.Base.Presentation.TaskList.IviewGantt.ProjectID
        Get
            Return Me.ViewState("ProjectID")
        End Get
        Set(ByVal value As Long)
            Me.ViewState("ProjectID") = value
        End Set
    End Property

    Public Sub ShowGantt(ByVal Title As String) Implements lm.Comol.Modules.Base.Presentation.TaskList.IviewGantt.ShowGantt
        Me.LBtitolo.Text = Me.Resource.getValue("Title") & System.Web.HttpUtility.HtmlEncode(Title)
        Me.ltprojectId.Text = "<script type=text/javascript language=javascript>var projectId=" + Me.ProjectID.ToString + ";</script>"
        Me.LBnotStarted.Text = Me.Resource.getValue("Legend.NotStarted")
        LBcompleted.Text = Me.Resource.getValue("Legend.Completed")
        LBstarted.Text = Me.Resource.getValue("Legend.Started")
        SetPreviusHyperlink()
        Me.MLVgantt.SetActiveView(Me.VIWgantt)
    End Sub

    Private Sub SetPreviusHyperlink()
        Dim url As String
        Select Case Me.PreviusPageType
            Case IviewGantt.PageType.DetailRead
                url = Me.BaseUrl & "TaskList/TaskDetail.aspx?CurrentTaskID=" & Me.TaskID & "&CurrentViewType=" & IViewTaskDetail.viewDetailType.Read.ToString
            Case IviewGantt.PageType.DetailUpdate
                url = Me.BaseUrl & "TaskList/TaskDetail.aspx?CurrentTaskID=" & Me.TaskID & "&CurrentViewType=" & IViewTaskDetail.viewDetailType.Editable.ToString
            Case IviewGantt.PageType.GeneralMap
                url = Me.BaseUrl & "TaskList/TasksMap.aspx?CurrentTaskID=" & Me.TaskID
            Case IviewGantt.PageType.SwichMap
                url = Me.BaseUrl & "TaskList/TasksMap.aspx?CurrentTaskID=" & Me.TaskID & "&CurrentMapType=" & IViewTaskMap.viewMapType.SwichMap.ToString
            Case Else
                url = Me.BaseUrl & "TaskList/AssignedTasks.aspx"
        End Select
        Me.HYPprevius.NavigateUrl = url
    End Sub


End Class