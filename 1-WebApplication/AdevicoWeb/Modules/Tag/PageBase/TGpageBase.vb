Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract
Imports lm.Comol.Core.BaseModules.Tags.Presentation
Imports lm.Comol.Core.Tag.Domain
Public MustInherit Class TGpageBase
    Inherits PageBase
    Implements IViewPageBase

#Region "Context"
    Private _presenter As TagsManagerPresenter
    Protected Friend ReadOnly Property CurrentPresenter As TagsManagerPresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New TagsManagerPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _presenter
        End Get
    End Property
#End Region

#Region "Implements"

#Region "Preload"
    Protected Friend ReadOnly Property PreloadForOrganization As Boolean Implements IViewPageBase.PreloadForOrganization
        Get
            Return Request.QueryString("forOrganization") = "true"
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadRecycleBin As Boolean Implements IViewPageBase.PreloadRecycleBin
        Get
            Return Request.QueryString("recycle") = "true"
        End Get
    End Property
#End Region

#Region "Settings"

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
#Region "Internal"
    Protected Friend Property IdTagsCommunity As Integer Implements IViewPageBase.IdTagsCommunity
        Get
            Return ViewStateOrDefault("IdTagsCommunity", 0)
        End Get
        Set(value As Integer)
            ViewState("IdTagsCommunity") = value
        End Set
    End Property

#End Region

#Region "Inherits"
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Dashboard", "Modules", "Dashboard")
    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

#End Region

#Region "Implements"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As ModuleTags.ActionType) Implements IViewPageBase.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, Nothing, InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idTag As Long, action As ModuleTags.ActionType) Implements IViewPageBase.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleTags.ObjectType.Tag, idTag.ToString), InteractionType.UserWithLearningObject)
    End Sub

#Region "Display"
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewPageBase.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, ModuleTags.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub
#End Region
    Protected Friend MustOverride Sub DisplaySessionTimeout() Implements IViewBase.DisplaySessionTimeout
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
    Private Sub SetBackUrl(url As String) Implements IViewPageBase.SetBackUrl
        Dim oHyperlink As HyperLink = GetBackUrlItem()
        If Not IsNothing(oHyperlink) Then
            oHyperlink.NavigateUrl = PageUtility.BaseUrl & url
            oHyperlink.Visible = True
        End If
    End Sub
    Private Sub AllowAdd(allow As Boolean) Implements IViewPageBase.AllowAdd
        Dim oLinkbutton As LinkButton = GetAddButton()
        If Not IsNothing(oLinkbutton) Then
            oLinkbutton.Visible = allow
        End If
    End Sub
    Private Sub AllowMultiple(allow As Boolean) Implements IViewPageBase.AllowMultiple
        Dim oLinkbutton As LinkButton = GetAddMultipleButton()
        If Not IsNothing(oLinkbutton) Then
            oLinkbutton.Visible = allow
        End If
    End Sub
    Private Sub InitializeListControl(permissions As ModuleTags, idCommunity As Integer, Optional fromRecycleBin As Boolean = False, Optional fromOrganization As Boolean = False) Implements IViewPageBase.InitializeListControl
        GetListControl().InitializeControl(permissions, idCommunity, fromRecycleBin, fromOrganization)

    End Sub
    Private Sub SetRecycleUrl(url As String) Implements IViewPageBase.SetRecycleUrl
        Dim oHyperlink As HyperLink = GetRecycleUrlItem()
        If Not IsNothing(oHyperlink) Then
            oHyperlink.NavigateUrl = PageUtility.BaseUrl & url
            oHyperlink.Visible = True
        End If
    End Sub
#End Region

#Region "Internal"
    Protected Friend MustOverride Function GetBackUrlItem() As HyperLink
    Protected Friend MustOverride Function GetRecycleUrlItem() As HyperLink
    Protected Friend MustOverride Function GetListControl() As UC_TagsList
    Protected Friend MustOverride Function GetListControlHeader() As UC_TagsListHeader
    Protected Friend MustOverride Function GetAddButton() As LinkButton
    Protected Friend MustOverride Function GetAddMultipleButton() As LinkButton
    Public Function GetBaseUrl() As String
        Return PageUtility.BaseUrl
    End Function
#End Region

End Class