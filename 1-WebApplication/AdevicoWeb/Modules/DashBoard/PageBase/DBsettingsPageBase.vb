Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract
Imports lm.Comol.Core.BaseModules.Dashboard.Presentation
Imports lm.Comol.Core.Dashboard.Domain
Public MustInherit Class DBsettingsPageBase
    Inherits PageBase
    Implements IViewBaseSettingsList

#Region "Context"
    Private _presenter As SettingsManagerPresenter
    Protected Friend ReadOnly Property CurrentPresenter As SettingsManagerPresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New SettingsManagerPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _presenter
        End Get
    End Property
#End Region

#Region "Implements"

#Region "Preload"
    Protected Friend ReadOnly Property PreloadRecycleBin As Boolean Implements IViewBaseSettingsList.PreloadRecycleBin
        Get
            Return Request.QueryString("recycle") = "true"
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadIdCommunity As Integer Implements IViewBaseSettingsList.PreloadIdCommunity
        Get
            If IsNumeric(Request.QueryString("idCommunity")) Then
                Return CInt(Request.QueryString("idCommunity"))
            Else
                Return -2
            End If
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadDashboardType As DashboardType Implements IViewBaseSettingsList.PreloadDashboardType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of DashboardType).GetByString(Request.QueryString("type"), DashboardType.Portal)
        End Get
    End Property
#End Region

#Region "Settings"
    Protected Friend Property IdContainerCommunity As Integer Implements IViewBaseSettingsList.IdContainerCommunity
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
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As ModuleDashboard.ActionType) Implements IViewBaseSettingsList.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, Nothing, InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idDashboard As Long, action As ModuleDashboard.ActionType) Implements IViewBaseSettingsList.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleDashboard.ObjectType.Dashboard, idDashboard.ToString), InteractionType.UserWithLearningObject)
    End Sub

#Region "Display"
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewBaseSettingsList.DisplayNoPermission
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
    Private Sub SetBackUrl(url As String) Implements IViewBaseSettingsList.SetBackUrl
        Dim oHyperlink As HyperLink = GetBackUrlItem()
        If Not IsNothing(oHyperlink) Then
            SetPageBackUrl(oHyperlink, url)
            SetPageBackUrl(GetBackUrlItem(False), url)
        End If
    End Sub
    Private Sub SetAddUrl(url As String) Implements IViewBaseSettingsList.SetAddUrl
        Dim oHyperlink As HyperLink = GetAddButton()
        If Not IsNothing(oHyperlink) Then
            SetPageBackUrl(oHyperlink, url)
            SetPageBackUrl(GetAddButton(False), url)
        End If
    End Sub
    Private Sub InitializeListControl(permissions As ModuleDashboard, type As DashboardType, idCommunity As Integer, loadFromRecycleBin As Boolean) Implements IViewBaseSettingsList.InitializeListControl
        GetListControl().InitializeControl(permissions, type, idCommunity, loadFromRecycleBin)
    End Sub
    Private Sub SetRecycleUrl(url As String) Implements IViewBaseSettingsList.SetRecycleUrl
        Dim oHyperlink As HyperLink = GetRecycleUrlItem()
        If Not IsNothing(oHyperlink) Then
            SetPageBackUrl(oHyperlink, url)
            SetPageBackUrl(GetRecycleUrlItem(False), url)
        End If
    End Sub
    Protected Friend MustOverride Sub SetTitle(type As DashboardType, Optional name As String = "") Implements IViewBaseSettingsList.SetTitle
#End Region

#Region "Internal"
    Protected Friend MustOverride Function GetBackUrlItem(Optional ByVal top As Boolean = True) As HyperLink
    Protected Friend MustOverride Function GetRecycleUrlItem(Optional ByVal top As Boolean = True) As HyperLink
    Protected Friend MustOverride Function GetListControl() As UC_SettingsList
    Protected Friend MustOverride Function GetAddButton(Optional ByVal top As Boolean = True) As HyperLink
    Private Sub SetPageBackUrl(oHyperlink As HyperLink, url As String)
        If Not IsNothing(oHyperlink) Then
            oHyperlink.NavigateUrl = PageUtility.ApplicationUrlBase & url
            oHyperlink.Visible = True
        End If
    End Sub

    Public Function GetBaseUrl() As String
        Return PageUtility.BaseUrl
    End Function
#End Region


End Class