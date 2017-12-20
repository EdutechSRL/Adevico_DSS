Imports lm.Comol.Core.Dashboard.Domain
Imports lm.Comol.Core.BaseModules.Dashboard.Presentation
Public Class EnrollTo
    Inherits DBpageBaseSearch
    Implements IViewEnrollToDashboard

#Region "Context"
    Private _Presenter As EnrollToPresenter
    Private ReadOnly Property CurrentPresenter() As EnrollToPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New EnrollToPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Protected Friend ReadOnly Property PreloadCommunityList As Boolean Implements IViewEnrollToDashboard.PreloadCommunityList
        Get
            If Not String.IsNullOrEmpty(Request.QueryString("load")) AndAlso Request.QueryString("load").ToLower = Boolean.TrueString.ToLower Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        If Not Page.IsPostBack Then
            CTRLdashboardEnrollHeader.SetTransacionIdContainer(Guid.NewGuid.ToString() & "_" & PageUtility.CurrentContext.UserContext.CurrentUserID & "_" & PageUtility.CurrentContext.UserContext.WorkSessionID.ToString)
        End If
        Me.CurrentPresenter.InitView(PreloadIdcommunityType, PreloadSearchText, PreloadCommunityList)
    End Sub

    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            Master.ServiceTitle = .getValue("DashboardSettings.ServiceTitle.Subscribe")
            Master.ServiceNopermission = Resource.getValue("DashboardSettings.ServiceTitle.Subscribe.NoPermission")
        End With
    End Sub
    Protected Friend Overrides Sub EnableFullWidth(value As Boolean)
        Master.EnabledFullWidth = value
    End Sub
    Protected Friend Overrides Sub DisplaySessionTimeout()
        RedirectOnSessionTimeOut(RootObject.EnrollTo(0, PreloadSearchText, PreloadIdcommunityType, PreloadCommunityList), 0)
    End Sub
    Protected Friend Overrides Function GetCurrentView() As DashboardViewType
        Return lm.Comol.Core.Dashboard.Domain.DashboardViewType.Subscribe
    End Function

#End Region

#Region "Implements"
    Private Sub InitializeSubscriptionControl(itemsForPage As Integer, range As RangeSettings, preloadIdCommunityType As Integer, searchText As String, preloadList As Boolean) Implements IViewEnrollToDashboard.InitializeSubscriptionControl
        CTRLenroll.InitializeControl(itemsForPage, range, preloadIdCommunityType, searchText, preloadList)
    End Sub
#End Region

#Region "Internal"
    Private Sub PortalDashboardLoader_Init(sender As Object, e As EventArgs) Handles Me.Init
        Master.BRheaderActive = False
        'Master.DisplayTitleRow = Fal
        Master.ShowDocType = True
        Master.ShowHeaderLanguageChanger = True
    End Sub
    Private Sub CTRLenroll_SetDefaultFilters(filters As List(Of lm.Comol.Core.DomainModel.Filters.Filter)) Handles CTRLenroll.SetDefaultFilters
        CTRLdashboardEnrollHeader.SetDefaultFilters(filters)
    End Sub
    Private Sub CTRLenroll_OpenConfirmDialog(openCssClass As String) Handles CTRLenroll.OpenConfirmDialog
        Master.SetOpenDialogOnPostbackByCssClass(openCssClass)
    End Sub
    Private Sub CTRLenroll_SessionTimeout() Handles CTRLenroll.SessionTimeout
        DisplaySessionTimeout()
    End Sub
#End Region

  
End Class