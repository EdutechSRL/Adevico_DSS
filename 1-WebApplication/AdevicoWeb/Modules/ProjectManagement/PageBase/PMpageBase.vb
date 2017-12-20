Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Imports lm.Comol.Modules.Standard.ProjectManagement.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract

Public MustInherit Class PMpageBase
    Inherits PageBase
    Implements IViewPageBase

#Region "Implements"

#Region "Preload"
    Protected Friend ReadOnly Property PreloadIdCommunity As Integer Implements IViewPageBase.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("cId")) Then
                Return CInt(Me.Request.QueryString("cId"))
            Else
                Return 0
            End If
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadForPortal As Boolean Implements IViewPageBase.PreloadForPortal
        Get
            Try
                Return System.Convert.ToBoolean(Me.Request.QueryString("isPortal"))
            Catch ex As Exception
            End Try
            Return False
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadIsPersonal As Boolean Implements IViewPageBase.PreloadIsPersonal
        Get
            Try
                Return System.Convert.ToBoolean(Me.Request.QueryString("isPersonal"))
            Catch ex As Exception
            End Try
            Return False
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadFromPage As PageListType Implements IViewPageBase.PreloadFromPage
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of PageListType).GetByString(Request.QueryString("fromView"), PageListType.Ignore)
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadIdContainerCommunity As Integer Implements IViewPageBase.PreloadIdContainerCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCC")) Then
                Return CInt(Me.Request.QueryString("idCC"))
            Else
                Return -1
            End If
        End Get
    End Property
#End Region

#Region "Settings"
    Protected Friend Property ProjectIdCommunity As Integer Implements IViewPageBase.ProjectIdCommunity
        Get
            Return ViewStateOrDefault("ProjectIdCommunity", 0)
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("ProjectIdCommunity") = value
        End Set
    End Property
    Protected Friend Property isPersonal As Boolean Implements IViewPageBase.isPersonal
        Get
            Return ViewStateOrDefault("isPersonal", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("isPersonal") = value
        End Set
    End Property
    Protected Friend Property forPortal As Boolean Implements IViewPageBase.forPortal
        Get
            Return ViewStateOrDefault("forPortal", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("forPortal") = value
        End Set
    End Property
    Protected Friend Property IdContainerCommunity As Integer Implements IViewPageBase.IdContainerCommunity
        Get
            Return ViewStateOrDefault("IdContainerCommunity", -1)
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("IdContainerCommunity") = value
        End Set
    End Property
#End Region

#End Region

#Region "Internal"
    Private previousUrl As String
    Private _FromPage As PageListType
    Protected Friend ReadOnly Property FromPage As PageListType
        Get
            If _FromPage = PageListType.None Then
                _FromPage = PreloadFromPage
            End If
            Return _FromPage
        End Get
    End Property
    Protected ReadOnly Property OnLoadingTranslation() As String
        Get
            Return Resource.getValue("OnLoadingTranslation")
        End Get
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Inherits"
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ProjectManagement", "Modules", "ProjectManagement")
    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

#End Region

#Region "Implements"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idProject As Long, action As ModuleProjectManagement.ActionType) Implements IViewPageBase.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleProjectManagement.ObjectType.Project, idProject.ToString), InteractionType.UserWithLearningObject)
    End Sub

#Region "Display"
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewPageBase.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, ModuleProjectManagement.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub
#End Region

    Protected Sub RedirectOnSessionTimeOut(ByVal destinationUrl As String, ByVal idCommunity As Integer)
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

        dto.DestinationUrl = destinationUrl

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If

        webPost.Redirect(dto)
    End Sub
#End Region

End Class