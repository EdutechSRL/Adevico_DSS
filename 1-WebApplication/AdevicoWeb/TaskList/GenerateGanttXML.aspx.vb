Imports lm.Comol.Modules.Base.Presentation.TaskList
Imports lm.Comol.UI.Presentation
Imports System.Xml.Serialization
'Imports System.IO
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.Modules.TaskList.Domain

Partial Public Class GenerateGanttXML
    Inherits PageBase
    Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewGenerateGanttXML



    Private _presenter As GenerateGanttXMLPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Servizio As Services_TaskList
    Private _BaseUrl As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region " Base"


    'Public Overloads ReadOnly Property BaseUrl() As String
    '    Get
    '        If _BaseUrl = "" Then
    '            _BaseUrl = Me.PageUtility.BaseUrl
    '        End If
    '        Return _BaseUrl
    '    End Get
    'End Property
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get

        End Get
    End Property

    Public Overrides Sub BindDati()

        If Not IsPostBack Then
            Me.CurrentPresenter.InitView(Me.ApplicationUrlBase)
        End If
    End Sub

    Public Overrides Sub BindNoPermessi()

    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_AssignedTasks", "TaskList")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            'Me.Master.ServiceTitle = .getValue("serviceTitle")

        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get
            Return False
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

    Public ReadOnly Property CurrentPresenter() As GenerateGanttXMLPresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New GenerateGanttXMLPresenter(Me.CurrentContext, Me)
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


    Public ReadOnly Property ProjectID() As Long Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewGenerateGanttXML.ProjectID
        Get
            Try
                Return CType(Request.QueryString("ProjectID"), Long)
            Catch ex As Exception
                Return 0
            End Try
        End Get
    End Property



    Public Sub GenerateGanttXML(ByVal Project As ProjectForGanttXML) Implements lm.Comol.Modules.Base.Presentation.TaskList.IViewGenerateGanttXML.GenerateGanttXML

        Response.ClearContent()
        Response.ContentType = "text/xml"
        Dim serializer As New System.Xml.Serialization.XmlSerializer(GetType(ProjectForGanttXML))
        serializer.Serialize(Response.OutputStream, Project)
        Response.OutputStream.Flush()
        Response.Flush()
    End Sub
End Class