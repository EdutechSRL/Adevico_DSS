Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Helpers
Imports lm.Comol.Core.BaseModules.Dashboard.Presentation
Imports lm.Comol.Core.BaseModules.Dashboard.Domain
Public Class UC_SelectCommunities
    Inherits BaseControl
    Implements IViewSelectCommunities

#Region "Context"
    Private _Presenter As SelectCommunitiesPresenter
    Private ReadOnly Property CurrentPresenter() As SelectCommunitiesPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New SelectCommunitiesPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private Property AddControlUsed As Boolean Implements IViewSelectCommunities.AddControlUsed
        Get
            Return ViewStateOrDefault("AddControlUsed", False)
        End Get
        Set(value As Boolean)
            ViewState("AddControlUsed") = value
        End Set
    End Property
    Public ReadOnly Property HasAvailableCommunitiesToAdd As Boolean Implements IViewSelectCommunities.HasAvailableCommunitiesToAdd
        Get
            Return CTRLaddCommunity.HasAvailableCommunities
        End Get
    End Property

    Public Property isInitialized As Boolean Implements IViewSelectCommunities.isInitialized
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Public Property AdministrationMode As Boolean Implements IViewSelectCommunities.AdministrationMode
        Get
            Return ViewStateOrDefault("AdministrationMode", False)
        End Get
        Set(value As Boolean)
            ViewState("AdministrationMode") = value
        End Set
    End Property
    Public Property AllowAdd As Boolean Implements IViewSelectCommunities.AllowAdd
        Get
            Return ViewStateOrDefault("AllowAdd", True)
        End Get
        Set(value As Boolean)
            ViewState("AllowAdd") = value
            Me.BTNaddUp.Visible = value
            If value AndAlso SelectedIdCommunities.Count > 10 Then
                Me.BTNaddDown.Visible = value
            Else
                Me.BTNaddDown.Visible = False
            End If
        End Set
    End Property
    Public Property andServiceClauses As List(Of dtoModulePermission) Implements IViewSelectCommunities.andServiceClauses
        Get
            Return ViewStateOrDefault("andServiceClauses", New List(Of dtoModulePermission))
        End Get
        Set(value As List(Of dtoModulePermission))
            ViewState("andServiceClauses") = value
        End Set
    End Property
    Public Property orServiceClauses As List(Of dtoModulePermission) Implements IViewSelectCommunities.orServiceClauses
        Get
            Return ViewStateOrDefault("orServiceClauses", New List(Of dtoModulePermission))
        End Get
        Set(value As List(Of dtoModulePermission))
            ViewState("orServiceClauses") = value
        End Set
    End Property
    Public Property NotLoadIdCommunities As List(Of Integer) Implements IViewSelectCommunities.NotLoadIdCommunities
        Get
            Return ViewStateOrDefault("NotLoadIdCommunities", New List(Of Integer))
        End Get
        Set(value As List(Of Integer))
            ViewState("NotLoadIdCommunities") = value
        End Set
    End Property
    Public Property OnlyFromOrganizations As List(Of Integer) Implements IViewSelectCommunities.OnlyFromOrganizations
        Get
            Return ViewStateOrDefault("OnlyFromOrganizations", New List(Of Integer))
        End Get
        Set(value As List(Of Integer))
            ViewState("OnlyFromOrganizations") = value
        End Set
    End Property

    'Public ReadOnly Property SelectedItems As List(Of dtoBaseCommunityNode) Implements IViewSelectCommunities.SelectedItems
    '    Get
    '        Return (From n As dtoCommunityPlain In SelectedCommunities Select New dtoBaseCommunityNode() With {.Id = n.Id, .IdFather = n.IdFather, .Path = n.Path}).ToList()
    '    End Get
    'End Property
    Public Function GetSelectedItems() As List(Of dtoCommunityPlainItem) Implements IViewSelectCommunities.GetSelectedItems
        Return SelectedCommunities
    End Function
    Public Property SelectedCommunities As List(Of dtoCommunityPlainItem) Implements IViewSelectCommunities.SelectedCommunities
        Get
            Return ViewStateOrDefault("SelectedCommunities", New List(Of dtoCommunityPlainItem))
        End Get
        Set(value As List(Of dtoCommunityPlainItem))
            ViewState("SelectedCommunities") = value
        End Set
    End Property
    Public ReadOnly Property SelectedIdCommunities As List(Of Integer) Implements IViewSelectCommunities.SelectedIdCommunities
        Get
            Return (From r As dtoCommunityPlainItem In SelectedCommunities Select r.Community.Id).ToList()
        End Get
    End Property
    Public ReadOnly Property HasSelectedCommunities As Boolean Implements IViewSelectCommunities.HasSelectedCommunities
        Get
            Return (SelectedCommunities.Count > 0)
        End Get
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Internal"
    Public Event LoadDefaultFiltersToHeader(filters As List(Of lm.Comol.Core.DomainModel.Filters.Filter), requiredPermissions As Dictionary(Of Integer, Long), unloadIdCommunities As List(Of Integer), availability As lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability, onlyFromOrganizations As List(Of Integer))
    Public Event ReloadInitializeVariablesToHeader(requiredPermissions As Dictionary(Of Integer, Long), unloadIdCommunities As List(Of Integer), availability As lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability, onlyFromOrganizations As List(Of Integer))
    Public Event SetModalIdentifier(identifier As String)
    Public Event SetModalIdentifierAndVariables(identifier As String, requiredPermissions As Dictionary(Of Integer, Long), unloadIdCommunities As List(Of Integer), availability As lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability, onlyFromOrganizations As List(Of Integer))
    Public Event SetModalTitle(title As String)
    Protected Function TranslateModalView() As String
        Return Resource.getValue("TranslateModalView.AddCommunities")
    End Function
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UC_SelectCommunities", "Modules", "Common")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(LBsessionTimeout)
            .setButton(BTNaddDown, True)
            .setButton(BTNaddUp, True)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(displayInfo As String, loadCommunities As List(Of dtoCommunityPlainItem), unloadIdCommunities As List(Of Integer), onlyFromOrganizations As List(Of Integer)) Implements IViewSelectCommunities.InitializeControl
        Me.CurrentPresenter.InitView(loadCommunities, unloadIdCommunities, onlyFromOrganizations)
        Me.LBinfo.Text = displayInfo
        RaiseEvent SetModalTitle(TranslateModalView)
    End Sub
    Public Sub InitializeControlByServices(displayInfo As String, loadCommunities As List(Of dtoCommunityPlainItem), unloadIdCommunities As List(Of Integer), andClause As List(Of dtoModulePermission)) Implements IViewSelectCommunities.InitializeControlByServices
        Me.CurrentPresenter.InitView(loadCommunities, unloadIdCommunities, andClause)
        Me.LBinfo.Text = displayInfo
        RaiseEvent SetModalTitle(TranslateModalView)
    End Sub
    'Public Sub InitializeControlByServices(displayInfo As String, loadCommunities As List(Of dtoCommunityPlainItem), unloadIdCommunities As List(Of Integer), andClause As List(Of dtoModulePermission), orClause As List(Of dtoModulePermission)) Implements IViewSelectCommunities.InitializeControlByServices
    '    Me.CurrentPresenter.InitView(loadCommunities, unloadIdCommunities, New List(Of Integer), andClause, orClause)
    '    Me.LBinfo.Text = displayInfo
    'End Sub
    Public Sub LoadItems(communities As List(Of dtoCommunityPlainItem)) Implements IViewSelectCommunities.LoadItems
        Me.SelectedCommunities = communities
        Me.RPTcommunities.DataSource = communities
        Me.RPTcommunities.DataBind()
        Me.RPTcommunities.Visible = True
        If communities.Count > 10 AndAlso AllowAdd Then
            Me.BTNaddDown.Visible = True
        End If
        Me.MLVcommunities.SetActiveView(VIWlist)
    End Sub
    Public Sub NoItems() Implements IViewSelectCommunities.NoItems
        Me.SelectedCommunities = New List(Of dtoCommunityPlainItem)
        Me.RPTcommunities.Visible = False
        Me.BTNaddDown.Visible = False
        Me.MLVcommunities.SetActiveView(VIWlist)
    End Sub

    Public Sub DisplaySessionTimeout() Implements IViewSelectCommunities.DisplaySessionTimeout
        Me.MLVcommunities.SetActiveView(VIWsessionTimeout)
    End Sub
    Public Sub UpdateSelectedCommunities(idCommunities As List(Of Integer)) Implements IViewSelectCommunities.UpdateSelectedCommunities
        Me.CurrentPresenter.UpdateSelectedCommunities(idCommunities)
    End Sub
#End Region

#Region "Manage List"
    Private Sub RPTcommunities_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTcommunities.ItemCommand
        If e.CommandName = "remove" Then
            Dim idCommunity As Integer = 0
            Integer.TryParse(e.CommandArgument, idCommunity)
            If idCommunity > 0 Then
                Me.CurrentPresenter.RemoveCommunity(idCommunity)
            End If
        End If
    End Sub

    Private Sub RPTcommunities_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTcommunities.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim item As dtoCommunityPlainItem = DirectCast(e.Item.DataItem, dtoCommunityPlainItem)
            Dim oButton As Button = e.Item.FindControl("BTNdelete")
            Resource.setButton(oButton, True)
            Dim oLabel As Label = e.Item.FindControl("LBcommunityType")

            oLabel.Text = item.Community.CommunityType

            oLabel = e.Item.FindControl("LBcommunityName")
            If Not IsNothing(item.Paths) Then
                If item.Paths.Count = 1 Then
                    oLabel.ToolTip = String.Join(" / ", item.Paths(0).FathersName.ToArray())
                Else
                    oLabel.ToolTip = String.Join(" / ", item.Paths.Where(Function(p) p.isPrimary).FirstOrDefault().FathersName.ToArray())
                End If
            End If
        ElseIf e.Item.ItemType = ListItemType.Header Then
            Dim oLabel As Label = e.Item.FindControl("LBremoveCommunity_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBcommunityName_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBcommunityType_t")
            Me.Resource.setLabel(oLabel)
        End If
    End Sub

    Private Sub BTNaddDown_Click(sender As Object, e As System.EventArgs) Handles BTNaddDown.Click, BTNaddUp.Click
        If AddControlUsed AndAlso AdministrationMode Then
            Dim items As New List(Of Integer)
            items.AddRange(NotLoadIdCommunities)
            items.AddRange(SelectedIdCommunities())
            CTRLaddCommunity.Visible = True

            CTRLaddCommunity.ReloadAdministrationControl(items, lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability.All)
            RaiseEvent SetModalIdentifierAndVariables(CTRLaddCommunity.ModalIdentifier, Nothing, items, lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability.All, OnlyFromOrganizations)
        ElseIf AdministrationMode Then
            CTRLaddCommunity.Visible = True
            RaiseEvent SetModalIdentifier(CTRLaddCommunity.ModalIdentifier)
        End If
    End Sub
#End Region

#Region "Manage Add control"
    Private Sub CTRLaddCommunity_LoadDefaultFiltersToHeader(filters As List(Of lm.Comol.Core.DomainModel.Filters.Filter), requiredPermissions As Dictionary(Of Integer, Long), unloadIdCommunities As List(Of Integer), availability As lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability, onlyFromOrganizations As List(Of Integer)) Handles CTRLaddCommunity.LoadDefaultFiltersToHeader
        RaiseEvent LoadDefaultFiltersToHeader(filters, requiredPermissions, unloadIdCommunities, availability, onlyFromOrganizations)
    End Sub

    Public Sub InitializeAddControl(idprofile As Integer, unloadIdCommunities As List(Of Integer), onlyFromOrganizations As List(Of Integer)) Implements IViewSelectCommunities.InitializeAddControl
        If AdministrationMode Then
            CTRLaddCommunity.Visible = True
            CTRLaddCommunity.InitializeAdministrationControl(idprofile, unloadIdCommunities, lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability.All, onlyFromOrganizations)
            'RaiseEvent SetModalIdentifier(CTRLaddCommunity.ModalIdentifier)
        End If
    End Sub
    Private Sub CTRLaddCommunity_SelectedCommunities(idCommunities As List(Of Integer), identifier As String) Handles CTRLaddCommunity.SelectedCommunities
        AddControlUsed = True
        '  Master.SetOpenDialogOnPostbackByCssClass("")
        CurrentPresenter.AddCommunities(CTRLaddCommunity.GetSelectedItems())
        RaiseEvent SetModalIdentifier("")
    End Sub
#Region "NOT implemented"
    Private Sub InitializeAddControlByService(idprofile As Integer, unloadIdCommunities As List(Of Integer), andClause As List(Of dtoModulePermission)) Implements IViewSelectCommunities.InitializeAddControlByService

    End Sub
    Private Sub InitializeAddControlByService(idprofile As Integer, unloadIdCommunities As List(Of Integer), andClause As List(Of dtoModulePermission), orClause As List(Of dtoModulePermission)) Implements IViewSelectCommunities.InitializeAddControlByService

    End Sub
#End Region

#End Region


  
End Class