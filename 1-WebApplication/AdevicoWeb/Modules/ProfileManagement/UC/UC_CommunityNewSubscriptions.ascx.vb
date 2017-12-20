Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.CommunityManagement
Imports lm.Comol.Core.BaseModules.CommunityManagement.Presentation
Imports lm.Comol.UI.Presentation

Public Class UC_CommunityNewSubscriptions
    Inherits BaseControl
    Implements IViewNewProfileSubscriptionsList

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
    Private _Presenter As NewSubscriptionsListPresenter
    Private ReadOnly Property CurrentPresenter() As NewSubscriptionsListPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New NewSubscriptionsListPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property HasAvailableSubscriptions As Boolean Implements IViewNewProfileSubscriptionsList.HasAvailableSubscriptions
        Get
            Return ViewStateOrDefault("HasAvailableSubscriptions", False)
        End Get
        Set(value As Boolean)
            ViewState("HasAvailableSubscriptions") = value
        End Set
    End Property
    Private Property Subscriptions As List(Of dtoNewProfileSubscription)
        Get
            Return ViewStateOrDefault("Subscriptions", New List(Of dtoNewProfileSubscription))
        End Get
        Set(value As List(Of dtoNewProfileSubscription))
            ViewState("Subscriptions") = value
        End Set
    End Property
#End Region

#Region "Property"
    Public Property isInitialized As Boolean
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Protected Function BackGroundItem(ByVal oType As ListItemType) As String
        'If isVisible Then
        Select Case oType
            Case ListItemType.Item
                Return "ROW_Normal_Small"
            Case ListItemType.AlternatingItem
                Return "ROW_Alternate_Small"
            Case Else
                Return ""
        End Select
        'Else
        'Return "ROW_Disabilitate_Small"
        'End If

        Return ""
    End Function
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_AddCommunityToProfile", "Modules", "ProfileManagement")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource

        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(items As List(Of lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityPlainItem)) Implements IViewNewProfileSubscriptionsList.InitializeControl
        Me.CurrentPresenter.InitView(items)
    End Sub
    Private Sub LoadNothing() Implements IViewNewProfileSubscriptionsList.LoadNothing
        Subscriptions = New List(Of dtoNewProfileSubscription)
        Me.MLVdata.SetActiveView(VIWempty)
    End Sub
    Private Sub LoadSubscriptions(items As List(Of dtoNewProfileSubscription), templates As List(Of dtoRoleCommunityTypeTemplate), translatedRoles As Dictionary(Of Int32, String)) Implements IViewNewProfileSubscriptionsList.LoadSubscriptions
        Subscriptions = items
        Me.MLVdata.SetActiveView(VIWlist)
        Me.RPTsubscriptions.DataSource = items
        Me.RPTsubscriptions.DataBind()
        If items.Count > 0 Then
            InitializeRoles(items, templates, translatedRoles)
        End If
    End Sub
#End Region
   
#Region "Grid management"
    Private Sub RPTsubscriptions_ItemDataBound1(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTsubscriptions.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            Dim oLabel As Label = e.Item.FindControl("LBcommunityName_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBcommunityRole_t")
            Me.Resource.setLabel(oLabel)
        End If
    End Sub
    Private Sub InitializeRoles(items As List(Of dtoNewProfileSubscription), templates As List(Of dtoRoleCommunityTypeTemplate), translatedRoles As Dictionary(Of Int32, String))
        Dim index As Integer = 0

        For Each item As dtoNewProfileSubscription In items
            Dim r As RepeaterItem = RPTsubscriptions.Items(index)
            Dim oDropdownlist As DropDownList = r.FindControl("DDLrole")
            Dim roleTemplates As List(Of dtoRoleCommunityTypeTemplate) = (From t As dtoRoleCommunityTypeTemplate In templates Where t.IdCommunityType = item.IdCommunityType Select t).ToList
            Dim roleTemplatesId As List(Of Integer) = roleTemplates.Select(Function(t) t.IdRole).ToList

            oDropdownlist.DataSource = translatedRoles.Where(Function(role) roleTemplatesId.Contains(role.Key))

            oDropdownlist.DataTextField = "Value"
            oDropdownlist.DataValueField = "Key"
            oDropdownlist.DataBind()

            If item.IdPreviousRole <= 0 OrElse IsNothing(oDropdownlist.Items.FindByValue(item.IdPreviousRole)) Then
                oDropdownlist.SelectedValue = roleTemplates.Where(Function(rt) rt.isDefault = True).Select(Function(rtt) rtt.IdRole).FirstOrDefault()
            Else
                oDropdownlist.SelectedValue = item.IdPreviousRole
            End If

            index += 1
        Next
    End Sub
#End Region

    Public Function SelectedSubscriptions() As List(Of dtoNewProfileSubscription) Implements IViewNewProfileSubscriptionsList.SelectedSubscriptions
        Dim result As List(Of dtoNewProfileSubscription) = Me.Subscriptions

        For Each r As RepeaterItem In RPTsubscriptions.Items
            Dim oLiteral As Literal = r.FindControl("LTidCommunity")
            Dim dto As dtoNewProfileSubscription = result.Where(Function(s) s.Id = CInt(oLiteral.Text)).FirstOrDefault()
            If Not IsNothing(dto) Then
                Dim oDropdownlist As DropDownList = r.FindControl("DDLrole")
                dto.IdPreviousRole = dto.IdRole
                If oDropdownlist.SelectedIndex > -1 Then
                    dto.IdRole = CInt(oDropdownlist.SelectedValue)
                    dto.RoleName = oDropdownlist.SelectedItem.Text
                End If
            End If
        Next
        Return result
    End Function
    
End Class