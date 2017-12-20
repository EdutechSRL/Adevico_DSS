Imports lm.Comol.Core.Dashboard.Domain
Imports lm.Comol.Core.BaseModules.Dashboard.Presentation
Public Class PortalDashboardLoader
    Inherits DBpageBaseDashboardLoader
    Implements IViewDefaultDashboardLoader

#Region "Context"
    Private _Presenter As DefaultDashboardPresenter
    Private ReadOnly Property CurrentPresenter() As DefaultDashboardPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New DefaultDashboardPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        Me.CurrentPresenter.InitView(True)
    End Sub

    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub SetInternazionalizzazione()

    End Sub

#End Region

#Region "Implements"
    Protected Friend Overrides Sub DisplaySessionTimeout()
        RedirectOnSessionTimeOut(PageUtility.SystemSettings.Presenter.DefaultLogonPage, 0)
    End Sub
    Protected Friend Overrides Function GetCurrentView() As lm.Comol.Core.Dashboard.Domain.DashboardViewType
        Return lm.Comol.Core.Dashboard.Domain.DashboardViewType.List
    End Function

    Private Sub LoadDashboard(url As String) Implements IViewDefaultDashboardLoader.LoadDashboard
        If Not String.IsNullOrEmpty(url) Then
            PageUtility.RedirectToUrl(url)
        Else
            PageUtility.RedirectToUrl("modules/Dashboard/portal.aspx")
        End If
    End Sub
#End Region

    Private Sub PortalDashboardLoader_Init(sender As Object, e As EventArgs) Handles Me.Init
        Master.BRheaderActive = False
        Master.DisplayTitleRow = False
        Master.ShowDocType = True
    End Sub
End Class