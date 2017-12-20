Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.ProfileManagement
Imports lm.Comol.Core.BaseModules.ProfileManagement.Presentation
Imports lm.Comol.Core.Authentication

Public Class UC_ProfileCommunities
    Inherits BaseControl
    Implements IViewProfileCommunitySubscriptions


#Region "Context"
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Private _Presenter As ProfileCommunitiesPresenter
    Private ReadOnly Property CurrentPresenter() As ProfileCommunitiesPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ProfileCommunitiesPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property IsInitialized As Boolean Implements IViewProfileCommunitySubscriptions.IsInitialized
        Get
            Return ViewStateOrDefault("IsInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("IsInitialized") = value
        End Set
    End Property
    Public Property IdProfile As Integer Implements IViewProfileCommunitySubscriptions.IdProfile
        Get
            Return ViewStateOrDefault("IdProfile", 0)
        End Get
        Set(value As Integer)
            ViewState("IdProfile") = value
        End Set
    End Property
    Public Property Pager As PagerBase Implements IViewProfileCommunitySubscriptions.Pager
        Get
            Return ViewStateOrDefault("Pager", New PagerBase With {.PageSize = CurrentPageSize})
        End Get
        Set(value As PagerBase)
            Me.ViewState("Pager") = value
            Me.PGgrid.Pager = value
            Me.PGgrid.Visible = Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize)
            Me.DIVpageSize.Visible = (Not value.Count < Me.DDLpage.Items(0).Value)
        End Set
    End Property
    Public Property CurrentPageSize As Integer Implements IViewProfileCommunitySubscriptions.CurrentPageSize
        Get
            Return Me.DDLpage.SelectedValue
        End Get
        Set(value As Integer)
            If Me.DDLpage.Items.FindByValue(value) Is Nothing Then
                Me.DDLpage.SelectedIndex = 0
            Else
                Me.DDLpage.SelectedValue = value
            End If
        End Set
    End Property
    Public Property SelectedStatus As SubscriptionStatus Implements IViewProfileCommunitySubscriptions.SelectedStatus
        Get
            If (Me.RBLstatus.SelectedIndex > -1) Then
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of StatusProfile).GetByString(Me.RBLstatus.SelectedValue, SubscriptionStatus.all)
            Else
                Return SubscriptionStatus.all
            End If
        End Get
        Set(value As SubscriptionStatus)
            If Not IsNothing(Me.RBLstatus.Items.FindByValue(value.ToString)) Then
                Me.RBLstatus.SelectedValue = value.ToString
            End If
        End Set
    End Property
    Public ReadOnly Property GetCurrentFilters As lm.Comol.Core.Subscriptions.dtoSubscriptionFilters Implements IViewProfileCommunitySubscriptions.GetCurrentFilters
        Get
            Dim filter As New lm.Comol.Core.Subscriptions.dtoSubscriptionFilters
            filter.Ascending = OrderAscending
            filter.IdcommunityType = -1
            filter.IdOrganization = -1
            filter.IdOwner = -1
            filter.OrderBy = lm.Comol.Core.Subscriptions.OrderSubscriptionsBy.SubscriptionDate
            filter.SearchBy = lm.Comol.Core.Subscriptions.SearchSubscriptionsBy.None
            filter.PageIndex = Me.Pager.PageIndex
            filter.PageSize = CurrentPageSize
            filter.Status = SelectedStatus
            Return filter
        End Get
    End Property
    Public Property OrderAscending As Boolean Implements IViewProfileCommunitySubscriptions.OrderAscending
        Get
            Return ViewStateOrDefault("OrderAscending", False)
        End Get
        Set(value As Boolean)
            ViewState("OrderAscending") = value
        End Set
    End Property

#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PGgrid.Pager = Pager
    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ProfileInfo", "Modules", "ProfileManagement")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource

        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(idProfile As Integer) Implements IViewProfileCommunitySubscriptions.InitializeControl
        MLVinfo.SetActiveView(VIWdata)

        Me.SetInternazionalizzazione()
        Me.IsInitialized = True
        Me.CurrentPresenter.InitView(idProfile)
    End Sub

    Private Sub DisplayEmpty() Implements IViewProfileCommunitySubscriptions.DisplayEmpty
        MLVinfo.SetActiveView(VIWdefault)
    End Sub

    Public Sub LoadAvaliableStatus(items As List(Of SubscriptionStatus)) Implements IViewProfileCommunitySubscriptions.LoadAvaliableStatus
        Dim translations As List(Of TranslatedItem(Of String)) = (From s In items Select New TranslatedItem(Of String) With {.Id = s.ToString, .Translation = Me.Resource.getValue("SubscriptionStatus." & s.ToString)}).ToList

        Me.RBLstatus.DataSource = translations
        Me.RBLstatus.DataValueField = "Id"
        Me.RBLstatus.DataTextField = "Translation"
        Me.RBLstatus.DataBind()

        If items.Count > 0 Then
            Me.RBLstatus.SelectedIndex = 0
        End If
    End Sub

    Public Sub LoadCommunities(items As List(Of lm.Comol.Core.Subscriptions.dtoBaseSubscription)) Implements IViewProfileCommunitySubscriptions.LoadCommunities
        If items.count = 0 Then
            Me.RPTsubscriptions.Visible = False
        Else
            Me.RPTsubscriptions.Visible = True
            Me.RPTsubscriptions.DataSource = items
            Me.RPTsubscriptions.DataBind()
        End If
    End Sub


   
#End Region


#Region "Display Subscriptions"
    Private Sub RPTsubscriptions_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTsubscriptions.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            Dim oLabel As Label = e.Item.FindControl("LBname_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBsubscription_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBlastVisit_t")
            Me.Resource.setLabel(oLabel)
        ElseIf e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim subscription As lm.Comol.Core.Subscriptions.dtoBaseSubscription = DirectCast(e.Item.DataItem, lm.Comol.Core.Subscriptions.dtoBaseSubscription)

            Dim oLabel As Label = e.Item.FindControl("LBname")
            oLabel.Text = subscription.CommunityName

            oLabel = e.Item.FindControl("LBlastVisit")
            If subscription.LastAccessOn.HasValue Then
                oLabel.Text = subscription.LastAccessOn.Value.ToShortDateString & " " & subscription.LastAccessOn.Value.ToShortTimeString
            Else
                oLabel.Text = " "
            End If

            oLabel = e.Item.FindControl("LBsubscription")
            If subscription.SubscriptedOn.HasValue Then
                oLabel.Text = subscription.SubscriptedOn.Value.ToShortDateString & " " & subscription.SubscriptedOn.Value.ToShortTimeString
            Else
                oLabel.Text = " "
            End If
        End If
    End Sub
    Private Sub DDLpage_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLpage.SelectedIndexChanged
        Me.CurrentPresenter.LoadSubscriptions(Me.PGgrid.Pager.PageIndex, Me.CurrentPageSize)
    End Sub
    Private Sub PGgrid_OnPageSelected() Handles PGgrid.OnPageSelected
        Me.CurrentPresenter.LoadSubscriptions(Me.PGgrid.Pager.PageIndex, Me.CurrentPageSize)
    End Sub
    Private Sub RBLstatus_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RBLstatus.SelectedIndexChanged
        Me.CurrentPresenter.LoadSubscriptions(0, Me.CurrentPageSize)
    End Sub
#End Region

   
   
    
 
    
End Class