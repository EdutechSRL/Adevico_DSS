Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.CommunityManagement
Imports lm.Comol.Core.BaseModules.CommunityManagement.Presentation
Imports lm.Comol.UI.Presentation

Public Class UC_ProfileCommunitySubscriptions
    Inherits BaseControl
    Implements IViewStandardSubscriptionsList

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
    Private _Presenter As StandardSubscriptionsListPresenter
    Private ReadOnly Property CurrentPresenter() As StandardSubscriptionsListPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New StandardSubscriptionsListPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property HasAvailableSubscriptions As Boolean Implements IViewStandardSubscriptionsList.HasAvailableSubscriptions
        Get
            Return ViewStateOrDefault("HasAvailableSubscriptions", False)
        End Get
        Set(value As Boolean)
            ViewState("HasAvailableSubscriptions") = value
        End Set
    End Property

    Public Property IdProfile As Integer Implements IViewStandardSubscriptionsList.IdProfile
        Get
            Return ViewStateOrDefault("IdProfile", 0)
        End Get
        Set(value As Integer)
            ViewState("IdProfile") = value
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
        'If Page.IsPostBack = False Then
        '    Me.SetInternazionalizzazione()
        'End If
        'List<dtoGenericAssignment> ListCRole;
        ' Dim oMS As         COL_BusinessLogic_v2.Comol.Manager.ManagerSubscription  = new COL_BusinessLogic_v2.Comol.Manager.ManagerSubscription(UserID, CommunityID, LanguageID);
        '        List<int> MyComm = new List<int>();
        '        MyComm.Add(CommunityID);
        '        List<Entity.Role> CommunityRole = oMS.GetRolesAvailableByCommunity(MyComm);

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
    Public Sub InitializeControl(idProfile As Integer, nodes As List(Of dtoBaseCommunityNode)) Implements IViewStandardSubscriptionsList.InitializeControl
        Me.CurrentPresenter.InitView(idProfile, nodes)
    End Sub
    Private Sub RPTsubscriptions_ItemDataBound1(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTsubscriptions.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            Dim oLabel As Label = e.Item.FindControl("LBcommunityName_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBcommunityRole_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBnewSubscription_t")
            Me.Resource.setLabel(oLabel)
        ElseIf e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dto As dtoUserSubscription = DirectCast(e.Item.DataItem, dtoUserSubscription)

            Dim oLabel As Label = e.Item.FindControl("LBcommunityName")
            oLabel.Text = dto.CommunityName

            Dim oLiteral As Literal = e.Item.FindControl("LTidCommunity")
            oLiteral.Text = dto.IdCommunity
            oLiteral = e.Item.FindControl("LTidSubscriptions")
            oLiteral.Text = dto.Id
            oLiteral = e.Item.FindControl("LTidPreviousRole")
            oLiteral.Text = dto.IdPreviousRole
            oLiteral = e.Item.FindControl("LTpath")

            Dim builder As New System.Text.StringBuilder
            For Each item As String In dto.Path
                builder.Append(item).Append("|")
            Next
            oLiteral.Text = builder.ToString()

            oLiteral = e.Item.FindControl("LTmostLikelyPath")
            oLiteral.Text = dto.MostLikelyPath

            oLabel = e.Item.FindControl("LBnewSubscription")
            oLabel.Text = Me.Resource.getValue("isNew." & dto.isNew.ToString)
        End If
    End Sub

    Private Sub LoadNothing() Implements IViewStandardSubscriptionsList.LoadNothing
        Me.MLVdata.SetActiveView(VIWempty)
    End Sub

    Private Sub LoadSubscriptions(items As List(Of dtoUserSubscription), templates As List(Of dtoRoleCommunityTypeTemplate)) Implements IViewStandardSubscriptionsList.LoadSubscriptions
        Me.MLVdata.SetActiveView(VIWlist)
        Me.RPTsubscriptions.DataSource = items
        Me.RPTsubscriptions.DataBind()
        If items.Count > 0 Then
            InitializeRoles(items, templates)
        End If
    End Sub

    Public Function SelectedSubscriptions() As List(Of dtoUserSubscription) Implements IViewStandardSubscriptionsList.SelectedSubscriptions
        Dim result As New List(Of dtoUserSubscription)

        For Each r As RepeaterItem In RPTsubscriptions.Items
            Dim dto As New dtoUserSubscription

            Dim oDropdownlist As DropDownList = r.FindControl("DDLrole")
            Dim oLiteral As Literal = r.FindControl("LTidCommunity")
            dto.IdCommunity = CInt(oLiteral.Text)
            oLiteral = r.FindControl("LTidSubscriptions")
            dto.Id = CInt(oLiteral.Text)
            oLiteral = r.FindControl("LTidPreviousRole")
            dto.IdPreviousRole = CInt(oLiteral.Text)
            dto.IdRole = CInt(oDropdownlist.SelectedValue)

            oLiteral = r.FindControl("LTpath")
            dto.Path = oLiteral.Text.Split("|").ToList

            oLiteral = r.FindControl("LTmostLikelyPath")
            dto.MostLikelyPath = oLiteral.Text
            Dim oLabel As Label = r.FindControl("LBcommunityName")
            dto.CommunityName = oLabel.Text
            result.Add(dto)
        Next
        Return result
    End Function


    Private Sub InitializeRoles(items As List(Of dtoUserSubscription), templates As List(Of dtoRoleCommunityTypeTemplate))
        Dim roles As List(Of Comol.Entity.Role) = COL_BusinessLogic_v2.CL_permessi.COL_TipoRuolo.List(PageUtility.LinguaID).OrderBy(Function(t) t.Name).ToList
        Dim index As Integer = 0

        For Each item As dtoUserSubscription In items
            Dim r As RepeaterItem = RPTsubscriptions.Items(index)
            Dim oDropdownlist As DropDownList = r.FindControl("DDLrole")
            Dim roleTemplates As List(Of dtoRoleCommunityTypeTemplate) = (From t As dtoRoleCommunityTypeTemplate In templates Where t.IdCommunityType = item.IdCommunityType Select t).ToList
            Dim roleTemplatesId As List(Of Integer) = roleTemplates.Select(Function(t) t.IdRole).ToList

            oDropdownlist.DataSource = (From ro As Comol.Entity.Role In roles Where roleTemplatesId.Contains(ro.ID) Select ro).ToList()

            oDropdownlist.DataTextField = "Name"
            oDropdownlist.DataValueField = "ID"
            oDropdownlist.DataBind()

            If item.IdPreviousRole <= 0 OrElse IsNothing(oDropdownlist.Items.FindByValue(item.IdPreviousRole)) Then
                oDropdownlist.SelectedValue = roleTemplates.Where(Function(rt) rt.isDefault = True).Select(Function(rtt) rtt.IdRole).FirstOrDefault()
            Else
                oDropdownlist.SelectedValue = item.IdPreviousRole
            End If

            index += 1
        Next


    End Sub

End Class