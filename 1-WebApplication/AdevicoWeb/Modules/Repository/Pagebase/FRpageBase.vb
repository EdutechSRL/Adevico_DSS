Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract
Imports lm.Comol.Core.BaseModules.FileRepository.Presentation
Public MustInherit Class FRpageBase
    Inherits PageBase
    Implements IViewPageBase

#Region "Implements"

#Region "Preload"
    Protected Friend ReadOnly Property PreloadIdCommunity As Integer Implements IViewPageBase.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return -1
            End If
        End Get
    End Property
#End Region

#Region "Settings"
    Protected Friend Property RepositoryIdCommunity As Integer Implements IViewPageBase.RepositoryIdCommunity
        Get
            Return ViewStateOrDefault("RepositoryIdCommunity", 0)
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("RepositoryIdCommunity") = value
        End Set
    End Property
#End Region

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
        MyBase.SetCulture("pg_FileRepository", "Modules", "FileRepository")
    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

#End Region

#Region "Implements"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType) Implements IViewPageBase.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, Nothing, InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType, idItem As Long, objType As lm.Comol.Core.FileRepository.Domain.ModuleRepository.ObjectType) Implements IViewPageBase.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, objType, idItem.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType, objects As Dictionary(Of lm.Comol.Core.FileRepository.Domain.ModuleRepository.ObjectType, List(Of Long))) Implements IViewPageBase.SendUserAction

        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, objects.ToDictionary(Of Integer, List(Of Long))(Function(o) CInt(o.Key), Function(o) o.Value)), InteractionType.UserWithLearningObject)
    End Sub
#Region "Display"
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewPageBase.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub
#End Region
    Protected Friend MustOverride Sub DisplaySessionTimeout() Implements IViewBase.DisplaySessionTimeout
    Protected Sub RedirectOnSessionTimeOut(ByVal destinationUrl As String, ByVal idCommunity As Integer, Optional ByVal display As lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow)
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