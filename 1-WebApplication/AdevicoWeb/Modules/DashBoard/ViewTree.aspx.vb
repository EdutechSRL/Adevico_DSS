Imports lm.Comol.Core.Dashboard.Domain
Imports lm.Comol.Core.BaseModules.Dashboard.Presentation
Public Class ViewCommunityTree
    Inherits DBpageBase
    Implements IViewViewTree

#Region "Context"
    Private _Presenter As ViewTreePresenter
    Private ReadOnly Property CurrentPresenter() As ViewTreePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ViewTreePresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property PreloadFromPage As Boolean Implements IViewViewTree.PreloadFromPage
        Get
            Return (Not String.IsNullOrEmpty(Request.QueryString("FromPage")) AndAlso Request.QueryString("FromPage").ToLower = Boolean.TrueString.ToLower())
        End Get
    End Property
    Private ReadOnly Property PreloadFromSession As Boolean Implements IViewViewTree.PreloadFromSession
        Get
            Return False
        End Get
    End Property
    Private ReadOnly Property PreloadAdvancedTree As Boolean Implements IViewViewTree.PreloadAdvancedTree
        Get
            Return (Not String.IsNullOrEmpty(Request.QueryString("Advanced")) AndAlso Request.QueryString("Advanced").ToLower = Boolean.TrueString.ToLower())
        End Get
    End Property
    Private ReadOnly Property PreloadMode As lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability Implements IViewViewTree.PreloadMode
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability).GetByString(Request.QueryString("mode"), lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability.Subscribed)
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
       
    End Sub


#Region "Inherits"
    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        If Not Page.IsPostBack Then
            CTRLtreeHeader.SetTransacionIdContainer(Guid.NewGuid.ToString() & "_" & PageUtility.CurrentContext.UserContext.CurrentUserID & "_" & PageUtility.CurrentContext.UserContext.WorkSessionID.ToString)
        End If
        Dim url As String = ""
        If Not IsNothing(Request.UrlReferrer) Then
            url = Request.UrlReferrer.AbsoluteUri
        End If
        CurrentPresenter.InitView(PreloadAdvancedTree, PreloadIdCommunity, PreloadFromPage, url, PreloadFromSession)
    End Sub

    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        If Not Page.IsPostBack Then
            With Resource
                Master.ServiceTitle = .getValue("DashboardSettings.ServiceTitle.ViewTree." & PreloadAdvancedTree.ToString)
                Master.ServiceTitleToolTip = .getValue("DashboardSettings.ServiceTitle.ViewTree.ToolTip." & PreloadAdvancedTree.ToString)
                Master.ServiceNopermission = .getValue("DashboardSettings.ServiceTitle.ViewTree.NoPermission." & PreloadAdvancedTree.ToString)
                LBunkownCommunityForTree.Text = .getValue("LBunkownCommunityForTree." & PreloadAdvancedTree.ToString)
            End With
        End If
    End Sub
    Protected Friend Overrides Sub DisplaySessionTimeout()
        RedirectOnSessionTimeOut(lm.Comol.Core.Dashboard.Domain.RootObject.ViewTree(PreloadAdvancedTree, IIf(PreloadMode = lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability.Subscribed OrElse PreloadMode = lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability.None, "", PreloadMode.ToString), DashboardIdCommunity, False, False), DashboardIdCommunity, lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow)
    End Sub

#End Region

#Region "Implements"
    Private Sub LoadTree(advanced As Boolean, Optional cIdCommunity As Integer = 0) Implements IViewViewTree.LoadTree
        CTRLtree.InitializeControl(advanced, lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability.Subscribed, cIdCommunity)
    End Sub
    Private Sub SetTitle(name As String) Implements IViewViewTree.SetTitle
        If Not String.IsNullOrEmpty(name) Then
            Master.ServiceTitle = String.Format(Resource.getValue("DashboardSettings.ServiceTitle.ViewTree.Name." & PreloadAdvancedTree.ToString), name)
            Master.ServiceTitleToolTip = String.Format(Resource.getValue("DashboardSettings.ServiceTitle.ViewTree.Name.ToolTip." & PreloadAdvancedTree.ToString), name)
        End If
    End Sub
    Private Sub DisplayUnknownCommunity() Implements IViewViewTree.DisplayUnknownCommunity
        MLVcontent.SetActiveView(VIWunknown)
    End Sub
    Private Sub SetBackUrl(url As String) Implements IViewViewTree.SetBackUrl
        If String.IsNullOrEmpty(url) Then
            DVmenuTop.Visible = False
        Else
            DVmenuTop.Visible = True
            HYPgoToPreviousPage.NavigateUrl = url
        End If
    End Sub
#End Region
#Region "Internal"
    Private Sub ViewCommunityDetails_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Master.ShowDocType = True
        Master.BRheaderActive = False
    End Sub
    Private Sub CTRLtree_DisplayMessage(message As String, type As lm.Comol.Core.DomainModel.Helpers.MessageType) Handles CTRLtree.DisplayMessage

    End Sub
    Private Sub CTRLtree_HideDisplayMessage() Handles CTRLtree.HideDisplayMessage

    End Sub
    Private Sub CTRLtree_OpenConfirmDialog(openCssClass As String) Handles CTRLtree.OpenConfirmDialog
        Master.SetOpenDialogOnPostbackByCssClass(openCssClass)
    End Sub
    Private Sub CTRLtree_SessionTimeout() Handles CTRLtree.SessionTimeout
        DisplaySessionTimeout()
    End Sub
    Private Sub CTRLtree_SetDefaultFilters(filters As List(Of lm.Comol.Core.DomainModel.Filters.Filter)) Handles CTRLtree.SetDefaultFilters
        CTRLtreeHeader.SetDefaultFilters(filters)
    End Sub
#End Region


  
End Class