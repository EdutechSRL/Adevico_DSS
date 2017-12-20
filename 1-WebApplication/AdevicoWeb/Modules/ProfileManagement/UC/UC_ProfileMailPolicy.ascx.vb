Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.ProfileManagement
Imports lm.Comol.Core.BaseModules.ProfileManagement.Presentation
Imports lm.Comol.Core.Authentication
Imports lm.Comol.Core.BaseModules.CommunityManagement

Public Class UC_ProfileMailPolicy
    Inherits BaseControl
    Implements IViewMailProfilePolicy

#Region "Context"
    Private _Presenter As MailProfilePolicyPresenter
    Private ReadOnly Property CurrentPresenter() As MailProfilePolicyPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New MailProfilePolicyPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property IdProfile As Integer Implements IViewMailProfilePolicy.IdProfile
        Get
            Return ViewStateOrDefault("IdProfile", 0)
        End Get
        Set(value As Integer)
            ViewState("IdProfile") = value
        End Set
    End Property
    Public Property IsInitialized As Boolean Implements IViewMailProfilePolicy.IsInitialized
        Get
            Return ViewStateOrDefault("IsInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("IsInitialized") = value
        End Set
    End Property
    Public ReadOnly Property SelectedCommunities As List(Of Integer) Implements IViewMailProfilePolicy.SelectedCommunities
        Get
            Return (From n As Telerik.Web.UI.RadTreeNode In Me.RDTcommunity.CheckedNodes _
                  Where CInt(n.Value) > 0 AndAlso n.Checkable _
                  Select CInt(n.Value)).Distinct.ToList
        End Get
    End Property
    Public ReadOnly Property PreviousSelectedCommunities As List(Of Integer) Implements IViewMailProfilePolicy.PreviousSelectedCommunities
        Get
            Return ViewStateOrDefault("PreviousSelectedCommunities", New List(Of Integer))
        End Get
    End Property
    Private WriteOnly Property AvailableStatus As List(Of lm.Comol.Core.Communities.CommunityStatus) Implements IViewMailProfilePolicy.AvailableStatus
        Set(value As List(Of lm.Comol.Core.Communities.CommunityStatus))
            Dim translations As List(Of TranslatedItem(Of String)) = (From s In value Select New TranslatedItem(Of String) With {.Id = s.ToString, .Translation = Me.Resource.getValue("CommunityStatus." & s.ToString)}).ToList

            Me.RBLstatus.DataSource = translations
            Me.RBLstatus.DataValueField = "Id"
            Me.RBLstatus.DataTextField = "Translation"
            Me.RBLstatus.DataBind()

            If value.Count > 0 Then
                Me.RBLstatus.SelectedIndex = 0
            End If
        End Set
    End Property
    Private ReadOnly Property CurrentStatus As lm.Comol.Core.Communities.CommunityStatus ' Implements IViewStandardCommunityTree.CurrentStatus
        Get
            If (Me.RBLstatus.SelectedIndex > -1) Then
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Core.Communities.CommunityStatus).GetByString(Me.RBLstatus.SelectedValue, lm.Comol.Core.Communities.CommunityStatus.None)
            Else
                Return lm.Comol.Core.Communities.CommunityStatus.None
            End If
        End Get
    End Property
    Private Property CommunityFilters As dtoCommunitiesFilters Implements IViewMailProfilePolicy.CommunityFilters
        Get
            Dim filter As New dtoCommunitiesFilters
            filter.Ascending = True
            filter.Status = CurrentStatus
            filter.IdOrganization = -1
            filter.IdcommunityType = -1
            filter.Availability = CommunityAvailability.Subscribed
            filter.Value = Me.TXBcontains.Text
            filter.OrderBy = OrderCommunitiesBy.Name
            If String.IsNullOrEmpty(filter.Value) Then
                filter.SearchBy = SearchCommunitiesBy.All
            Else
                filter.SearchBy = SearchCommunitiesBy.Contains
            End If
            Return filter
        End Get
        Set(value As dtoCommunitiesFilters)

        End Set
    End Property

    Public WriteOnly Property ClientScript As String Implements IViewMailProfilePolicy.ClientScript
        Set(value As String)
            If Not String.IsNullOrEmpty(value) Then
                Me.BTNfilter.OnClientClick = value
                RBLstatus.Attributes.Add("onchange", value)
            End If
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

#Region "Control"
    Public Event DataUpdated()
    Public Event ShowErrors()

    Public Function TreeViewClientID() As String
        Return Me.RDTcommunity.ClientID
    End Function
    Public Function SearchTextBoxClientID() As String
        Return Me.TXBcontains.ClientID
    End Function
    Public Function SearchButtonClientID() As String
        Return Me.BTNfilter.ClientID
    End Function
    Public Function SearchButton() As Button
        Return Me.BTNfilter
    End Function
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ProfileInfo", "Modules", "ProfileManagement")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(LBmailPolicyInfo)
            .setLabel(LBcommunityStatus_t)
            .setLabel(LBcommunityname_t)
            .setButton(BTNfilter, True)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(idProfile As Integer) Implements IViewMailProfilePolicy.InitializeControl
        IsInitialized = True

        Me.CurrentPresenter.InitView(idProfile)
    End Sub
    Public Sub LoadTree(tree As dtoTreeCommunityNode, items As List(Of Integer)) Implements IViewMailProfilePolicy.LoadTree
        Dim oRootNode As New Telerik.Web.UI.RadTreeNode
        Me.RDTcommunity.Nodes.Clear()

        'oRootNode.Text
        If Not IsNothing(tree) Then
            oRootNode = CreateNode(tree, True)
            oRootNode.Text = Resource.getValue("RootCommunityName")

            Me.RDTcommunity.Nodes.Add(oRootNode)
            For Each node As dtoTreeCommunityNode In tree.Nodes
                Dim subNode As Telerik.Web.UI.RadTreeNode = Me.CreateNode(node, False)
                Me.RecursivelyPopulate(subNode, node.Nodes)
                oRootNode.Nodes.Add(subNode)
            Next

            Me.RDTcommunity.ClearCheckedNodes()
            Dim nodes As List(Of Telerik.Web.UI.RadTreeNode) = (From n As Telerik.Web.UI.RadTreeNode In Me.RDTcommunity.GetAllNodes _
                   Where CInt(n.Value) > 0 AndAlso n.Checkable AndAlso items.Contains(CInt(n.Value)) _
                   Select n).ToList()
            For Each node As Telerik.Web.UI.RadTreeNode In nodes
                node.Checked = True
            Next
            ViewState("PreviousSelectedCommunities") = (From n As Telerik.Web.UI.RadTreeNode In Me.RDTcommunity.CheckedNodes _
                  Where CInt(n.Value) > 0 AndAlso n.Checkable _
                  Select CInt(n.Value)).Distinct.ToList
        End If

        MLVdata.SetActiveView(VIWtree)
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

    Public Sub DisplayError() Implements IViewMailProfilePolicy.DisplayError
        RaiseEvent ShowErrors()
    End Sub
    Public Sub DisplayPolicySaved() Implements IViewMailProfilePolicy.DisplayPolicySaved
        RaiseEvent DataUpdated()
    End Sub

    Public Sub DisplayProfileUnknown() Implements IViewMailProfilePolicy.DisplayProfileUnknown
        MLVdata.SetActiveView(VIWempty)
        Me.RDTcommunity.Nodes.Clear()
    End Sub

    Public Sub DisplaySessionTimeout() Implements IViewMailProfilePolicy.DisplaySessionTimeout
        MLVdata.SetActiveView(VIWempty)
        Me.RDTcommunity.Nodes.Clear()
    End Sub


    Public Sub LoadNothing() Implements IViewMailProfilePolicy.LoadNothing
        MLVdata.SetActiveView(VIWempty)
        Me.RDTcommunity.Nodes.Clear()
    End Sub

#End Region

    Private Sub BTNfilter_Click(sender As Object, e As System.EventArgs) Handles BTNfilter.Click
        Me.CurrentPresenter.LoadCommunities(CommunityFilters)
    End Sub

    Private Sub RBLstatus_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RBLstatus.SelectedIndexChanged
        Me.CurrentPresenter.LoadCommunities(CommunityFilters)
    End Sub

    Public Function SaveData() As Boolean
        Return Me.CurrentPresenter.SavePolicy(Me.SelectedCommunities, Me.PreviousSelectedCommunities)
    End Function

End Class