Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract
Imports lm.Comol.Core.BaseModules.Tiles.Presentation
Imports lm.Comol.Core.Dashboard.Domain
Public MustInherit Class TLpageBase
    Inherits PageBase
    Implements IViewPageBase


#Region "Context"
    Private _presenter As TileManagerPresenter
    Protected Friend ReadOnly Property CurrentPresenter As TileManagerPresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New TileManagerPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _presenter
        End Get
    End Property
#End Region

#Region "Implements"

#Region "Preload"
    Protected Friend ReadOnly Property PreloadRecycleBin As Boolean Implements IViewPageBase.PreloadRecycleBin
        Get
            Return Request.QueryString("recycle") = "true"
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadIdCommunity As Integer Implements IViewPageBase.PreloadIdCommunity
        Get
            If IsNumeric(Request.QueryString("idCommunity")) Then
                Return CInt(Request.QueryString("idCommunity"))
            Else
                Return -2
            End If
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadDashboardType As DashboardType Implements IViewPageBase.PreloadDashboardType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of DashboardType).GetByString(Request.QueryString("type"), DashboardType.Portal)
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadIdTile As Long Implements IViewPageBase.PreloadIdTile
        Get
            If IsNumeric(Request.QueryString("idTile")) Then
                Return CLng(Request.QueryString("idTile"))
            Else
                Return 0
            End If
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadIdDashboard As Long Implements IViewPageBase.PreloadIdDashboard
        Get
            If IsNumeric(Request.QueryString("fromIdDashboard")) Then
                Return CLng(Request.QueryString("fromIdDashboard"))
            Else
                Return 0
            End If
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadStep As WizardDashboardStep Implements IViewPageBase.PreloadStep
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of WizardDashboardStep).GetByString(Request.QueryString("step"), WizardDashboardStep.None)
        End Get
    End Property
#End Region

#Region "Settings"
    Protected Friend Property IdTilesCommunity As Integer Implements IViewPageBase.IdTilesCommunity
        Get
            Return ViewStateOrDefault("IdTilesCommunity", -2)
        End Get
        Set(value As Integer)
            ViewState("IdTilesCommunity") = value
        End Set
    End Property
    Protected Friend Property IdContainerCommunity As Integer Implements IViewPageBase.IdContainerCommunity
        Get
            Return ViewStateOrDefault("IdContainerCommunity", 0)
        End Get
        Set(value As Integer)
            ViewState("IdContainerCommunity") = value
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
#Region "Internal"

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
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As ModuleDashboard.ActionType) Implements IViewPageBase.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, Nothing, InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idTile As Long, action As ModuleDashboard.ActionType) Implements IViewPageBase.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleDashboard.ObjectType.Tile, idTile.ToString), InteractionType.UserWithLearningObject)
    End Sub

#Region "Display"
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewPageBase.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, ModuleDashboard.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
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
    Private Sub SetDashboardSettingsBackUrl(url As String, name As String) Implements IViewPageBase.SetDashboardSettingsBackUrl
        Dim oHyperlink As HyperLink = GetDashboardSettingsBackUrlItem()
        If Not IsNothing(oHyperlink) Then
            oHyperlink.NavigateUrl = PageUtility.BaseUrl & url
            oHyperlink.Visible = True
            If (oHyperlink.ToolTip.Contains("{0}")) Then
                oHyperlink.ToolTip = String.Format(oHyperlink.ToolTip, name)
            End If
        End If
    End Sub
    Private Sub SetAddUrl(url As String) Implements IViewPageBase.SetAddUrl
        Dim oHyperlink As HyperLink = GetAddButton()
        If Not IsNothing(oHyperlink) Then
            oHyperlink.NavigateUrl = PageUtility.BaseUrl & url
            oHyperlink.Visible = True
        End If
    End Sub
    Private Sub InitializeListControl(permissions As ModuleDashboard, type As DashboardType, idCommunity As Integer, loadFromRecycleBin As Boolean, idTile As Long, preloadType As TileType) Implements IViewPageBase.InitializeListControl
        GetListControl().InitializeControl(permissions, type, idCommunity, loadFromRecycleBin, idTile, preloadType)
    End Sub
    Private Sub SetRecycleUrl(url As String) Implements IViewPageBase.SetRecycleUrl
        Dim oHyperlink As HyperLink = GetRecycleUrlItem()
        If Not IsNothing(oHyperlink) Then
            oHyperlink.NavigateUrl = PageUtility.BaseUrl & url
            oHyperlink.Visible = True
        End If
    End Sub
    Protected Friend MustOverride WriteOnly Property AllowCommunityTypesTileAutoGenerate As Boolean Implements IViewPageBase.AllowCommunityTypesTileAutoGenerate
#End Region

#Region "Internal"
    Protected Friend MustOverride Function GetDashboardSettingsBackUrlItem() As HyperLink
    Protected Friend MustOverride Function GetBackUrlItem() As HyperLink
    Protected Friend MustOverride Function GetRecycleUrlItem() As HyperLink
    Protected Friend MustOverride Function GetListControl() As UC_TileList
    Protected Friend MustOverride Function GetListControlHeader() As UC_TileListHeader
    Protected Friend MustOverride Function GetAddButton() As HyperLink
    Public Function GetBaseUrl() As String
        Return PageUtility.BaseUrl
    End Function
#End Region

End Class