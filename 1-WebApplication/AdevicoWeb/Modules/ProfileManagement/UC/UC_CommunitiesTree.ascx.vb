Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.CommunityManagement
Imports lm.Comol.Core.BaseModules.CommunityManagement.Presentation
Imports lm.Comol.UI.Presentation

Public Class UC_CommunitiesTree
    Inherits BaseControl
    Implements IViewStandardCommunityTree

    Public Event LoadCommunities(ByVal status As CommunityStatus)

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
    Private _Presenter As StandardCommunityTreePresenter
    Private ReadOnly Property CurrentPresenter() As StandardCommunityTreePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New StandardCommunityTreePresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Property"
    Public Property IdProfile As Integer Implements IViewStandardCommunityTree.IdProfile
        Get
            Return ViewStateOrDefault("IdProfile", 0)
        End Get
        Set(value As Integer)
            ViewState("IdProfile") = value
        End Set
    End Property
    Public ReadOnly Property CurrentStatus As lm.Comol.Core.Communities.CommunityStatus Implements IViewStandardCommunityTree.CurrentStatus
        Get
            If (Me.RBLstatus.SelectedIndex > -1) Then
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Core.Communities.CommunityStatus).GetByString(Me.RBLstatus.SelectedValue, lm.Comol.Core.Communities.CommunityStatus.None)
            Else
                Return lm.Comol.Core.Communities.CommunityStatus.None
            End If
        End Get
    End Property
    Public Property CurrentAvailability As CommunityAvailability Implements IViewStandardCommunityTree.CurrentAvailability
        Get
            If (Me.DDLsubscriptions.SelectedIndex > -1) Then
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of CommunityAvailability).GetByString(Me.DDLsubscriptions.SelectedValue, CommunityAvailability.All)
            Else
                Return CommunityAvailability.All
            End If
        End Get
        Set(value As CommunityAvailability)
            Me.DDLsubscriptions.SelectedValue = value.ToString
        End Set
    End Property
    Public ReadOnly Property CurrentIdCommunitytype As Int32
        Get
            If (Me.DDLtypes.SelectedIndex > -1) Then
                Return Me.DDLtypes.SelectedValue
            Else
                Return -1
            End If
        End Get
    End Property
    Public Property isInitialized As Boolean
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Public Property TriStateSelection() As Boolean
        Get
            Return Me.RDTcommunity.TriStateCheckBoxes
        End Get
        Set(ByVal value As Boolean)
            Me.RDTcommunity.TriStateCheckBoxes = value
            If Not value Then
                Me.RDTcommunity.CheckChildNodes = False
            End If
        End Set
    End Property
    Public Property HasAvailableCommunities As Boolean Implements IViewStandardCommunityTree.HasAvailableCommunities
        Get
            Return ViewStateOrDefault("HasAvailableCommunities", False)
        End Get
        Set(value As Boolean)
            ViewState("HasAvailableCommunities") = value
        End Set
    End Property
    Public Property CommunityFilters As dtoCommunitiesFilters Implements IViewStandardCommunityTree.CommunityFilters
        Get
            Dim filter As New dtoCommunitiesFilters
            filter.Ascending = True
            filter.Status = CurrentStatus
            filter.IdOrganization = -1
            filter.IdcommunityType = CurrentIdCommunitytype
            filter.Availability = CurrentAvailability
            Return filter
        End Get
        Set(value As dtoCommunitiesFilters)

        End Set
    End Property
    Public Function GetNodesById(idCommunities As List(Of Integer)) As List(Of dtoBaseCommunityNode) Implements IViewStandardCommunityTree.GetNodesById
        Dim iResponse As New List(Of dtoBaseCommunityNode)
        iResponse = (From n As Telerik.Web.UI.RadTreeNode In Me.RDTcommunity.GetAllNodes _
                     Where idCommunities.Contains(CInt(n.Value))
                     Select New dtoBaseCommunityNode() With {.Id = n.Value, .IdFather = IIf(n.Parent Is Nothing, 0, n.ParentNode.Value), .Path = n.Category}).ToList

        Return iResponse
    End Function

    Public Function SelectedIdCommunities() As List(Of Integer) Implements IViewStandardCommunityTree.SelectedIdCommunities
        Return (From n As Telerik.Web.UI.RadTreeNode In Me.RDTcommunity.CheckedNodes _
                     Where CInt(n.Value) > 0 AndAlso n.Checkable _
                     Select CInt(n.Value)).Distinct.ToList
    End Function
    Public Function SelectedIdOrganizations() As List(Of Integer) Implements IViewStandardCommunityTree.SelectedIdOrganizations
        Return (From n As Telerik.Web.UI.RadTreeNode In Me.RDTcommunity.Nodes(0).Nodes _
                    Where CInt(n.Value) > 0 AndAlso n.Checkable _
                    Select CInt(n.Value)).Distinct.ToList
    End Function
    Public Function TreeViewClientID() As String
        Return Me.RDTcommunity.ClientID
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
    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_AddCommunityToProfile", "Modules", "ProfileManagement")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(LBcommunityStatus_t)
            .setLabel(LBcommunityTypes_t)
            .setLabel(LBsubscriptions_t)

        End With
    End Sub
#End Region

    Public Sub InitializeControl(idProfile As Integer, availability As CommunityAvailability) Implements IViewStandardCommunityTree.InitializeControl
        Me.CurrentPresenter.InitView(idProfile, availability)
    End Sub
    Public Sub InitializeTree(items As List(Of lm.Comol.Core.Communities.CommunityStatus), IdTypes As List(Of Integer), availabilities As List(Of CommunityAvailability)) Implements IViewStandardCommunityTree.InitializeTree
        Dim translations As List(Of TranslatedItem(Of String)) = (From s In items Select New TranslatedItem(Of String) With {.Id = s.ToString, .Translation = Me.Resource.getValue("CommunityStatus." & s.ToString)}).ToList

        Me.RBLstatus.DataSource = translations
        Me.RBLstatus.DataValueField = "Id"
        Me.RBLstatus.DataTextField = "Translation"
        Me.RBLstatus.DataBind()

        If items.Count > 0 Then
            Me.RBLstatus.SelectedIndex = 0
        End If

        Me.DDLsubscriptions.DataSource = (From s In availabilities Select New TranslatedItem(Of String) With {.Id = s.ToString, .Translation = Me.Resource.getValue("CommunityAvailability." & s.ToString)}).ToList
        Me.DDLsubscriptions.DataValueField = "Id"
        Me.DDLsubscriptions.DataTextField = "Translation"
        Me.DDLsubscriptions.DataBind()

        If availabilities.Count < 2 Then
            Me.DDLsubscriptions.Visible = False
            Me.LBsubscriptions_t.Visible = False
        End If
        LoadTypes(IdTypes)
        isInitialized = True
        MLVdata.SetActiveView(VIWtree)
    End Sub
    Public Sub LoadTypes(IdTypes As List(Of Integer)) Implements IViewStandardCommunityTree.LoadTypes
        Dim selectedType As Integer = -9999
        If Me.DDLtypes.SelectedIndex > -1 Then
            selectedType = Me.DDLtypes.SelectedValue
        End If

        Dim types As List(Of COL_BusinessLogic_v2.Comunita.COL_Tipo_Comunita) = COL_BusinessLogic_v2.Comunita.COL_Tipo_Comunita.PlainLista(PageUtility.LinguaID, True)
        Me.DDLtypes.DataSource = (From t In types Where IdTypes.Contains(t.ID) Order By t.Descrizione Select t).ToList
        Me.DDLtypes.DataValueField = "ID"
        Me.DDLtypes.DataTextField = "Descrizione"
        Me.DDLtypes.DataBind()

        If DDLtypes.Items.Count > 1 Then
            Me.DDLtypes.Items.Insert(0, New ListItem(Resource.getValue("AllTypes"), -1))
            Me.DDLtypes.SelectedIndex = 0
        End If

        If selectedType <> -9999 AndAlso Not IsNothing(Me.DDLtypes.Items.FindByValue(selectedType)) Then
            Me.DDLtypes.SelectedValue = selectedType
        End If
    End Sub

    Public Sub LoadTree(ByVal tree As dtoTreeCommunityNode) Implements IViewStandardCommunityTree.LoadTree
        Dim oRootNode As New Telerik.Web.UI.RadTreeNode
        Me.RDTcommunity.Nodes.Clear()

        'oRootNode.Text

        oRootNode = CreateNode(tree, True)
        If tree.Type = dtoCommunityNodeType.Root Then
            oRootNode.Text = Resource.getValue("RootCommunityName")
        End If
        Me.RDTcommunity.Nodes.Add(oRootNode)
        For Each node As dtoTreeCommunityNode In tree.Nodes
            Dim subNode As Telerik.Web.UI.RadTreeNode = Me.CreateNode(node, False)
            Me.RecursivelyPopulate(subNode, node.Nodes)
            oRootNode.Nodes.Add(subNode)
        Next
    End Sub


    Private Sub RecursivelyPopulate(ByVal father As Telerik.Web.UI.RadTreeNode, ByVal nodes As List(Of dtoTreeCommunityNode))
        For Each node As dtoTreeCommunityNode In nodes
            Dim subNode As Telerik.Web.UI.RadTreeNode = Me.CreateNode(node, False)
            Me.RecursivelyPopulate(subNode, node.Nodes)
            father.Nodes.Add(subNode)
        Next
    End Sub
    Private Function CreateNode(tree As dtoTreeCommunityNode, ByVal Expanded As Boolean) As Telerik.Web.UI.RadTreeNode
        Dim oNode As New Telerik.Web.UI.RadTreeNode
        Try
            oNode.Value = tree.Id
            oNode.Category = tree.Path

            oNode.Checkable = (tree.Type <> dtoCommunityNodeType.Root AndAlso tree.Type <> dtoCommunityNodeType.NotSelectable)
            oNode.Text = " " & tree.Name
            'If oNode.Checkable Then
            oNode.Checked = tree.Selected
            'End If
            oNode.Expanded = Expanded
        Catch ex As Exception

        End Try
        Return oNode
    End Function

    Public Function SelectedCommunities() As List(Of dtoBaseCommunityNode) Implements IViewStandardCommunityTree.SelectedCommunities
        Dim iResponse As New List(Of dtoBaseCommunityNode)
        iResponse = (From n As Telerik.Web.UI.RadTreeNode In Me.RDTcommunity.CheckedNodes _
                     Where CInt(n.Value) > 0 AndAlso n.Checkable _
                     Select New dtoBaseCommunityNode() With {.Id = n.Value, .IdFather = IIf(n.Parent Is Nothing, 0, n.ParentNode.Value), .Path = n.Category}).ToList

        Return iResponse
    End Function
    Private Sub RBLstatus_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RBLstatus.SelectedIndexChanged
        Me.CurrentPresenter.ChangeStatus(CurrentStatus)
    End Sub
    Private Sub DDLtypes_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLtypes.SelectedIndexChanged
        Me.CurrentPresenter.LoadCommunities(CommunityFilters)
    End Sub
    Private Sub DDLsubscriptions_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLsubscriptions.SelectedIndexChanged
        Me.CurrentPresenter.LoadCommunities(CommunityFilters)
    End Sub
    Public Sub LoadNothing() Implements IViewStandardCommunityTree.LoadNothing
        MLVdata.SetActiveView(VIWempty)
        Me.RDTcommunity.Nodes.Clear()
    End Sub

    'Private Sub RDTcommunity_NodeCheck(sender As Object, e As Telerik.Web.UI.RadTreeNodeEventArgs) Handles RDTcommunity.NodeCheck
    '    Dim nodes As List(Of Telerik.Web.UI.RadTreeNode) = (From n As Telerik.Web.UI.RadTreeNode In Me.RDTcommunity.GetAllNodes _
    '                 Where n.Value = e.Node.Value AndAlso n.ID <> e.Node.ID
    '                 Select n).ToList
    '    If Not e.Node.Checked Then
    '        For Each node As Telerik.Web.UI.RadTreeNode In nodes
    '            node.Checked = e.Node.Checked
    '        Next
    '    End If
    'End Sub

    
End Class