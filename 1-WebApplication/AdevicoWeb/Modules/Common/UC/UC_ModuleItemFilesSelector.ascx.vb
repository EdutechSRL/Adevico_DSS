Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules
Imports lm.Comol.Core.BaseModules.Repository.Presentation
Imports lm.Comol.UI.Presentation

Public Class UC_ModuleItemFilesSelector
    Inherits BaseControl
    Implements IViewModuleItemFilesSelector


#Region "Inherits"
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
#End Region

#Region "Implements"
    Public ReadOnly Property FilesCount As Integer Implements IViewModuleItemFilesSelector.FilesCount
        Get
            Return (From oNode As Telerik.Web.UI.RadTreeNode In Me.RDTcommunityRepository.CheckedNodes Where CLng(oNode.Category) > 0 Select CLng(oNode.Value)).Count
        End Get
    End Property
    Public ReadOnly Property HasFileToSelect As Boolean Implements IViewModuleItemFilesSelector.HasFileToSelect
        Get
            Return (From oNode As Telerik.Web.UI.RadTreeNode In Me.RDTcommunityRepository.CheckedNodes Where CLng(oNode.Category) > 0 Select CLng(oNode.Value)).Any
        End Get
    End Property
    Public Property isInitialized As Boolean Implements IViewModuleItemFilesSelector.isInitialized
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(ByVal value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Public ReadOnly Property SelectedFiles As List(Of StatFileTreeLeaf) Implements IViewModuleItemFilesSelector.SelectedFiles
        Get
            Dim selectedNodesId As List(Of Long) = (From oNode As Telerik.Web.UI.RadTreeNode In Me.RDTcommunityRepository.CheckedNodes Where CLng(oNode.Category) > 0 Select CLng(oNode.Value)).ToList()
            Return (From node In LeafNodes Where selectedNodesId.Contains(node.Id) Select node).ToList()
        End Get
    End Property
    Private Property LeafNodes As List(Of StatFileTreeLeaf) Implements IViewModuleItemFilesSelector.LeafNodes
        Get
            Return ViewStateOrDefault("LeafNodes", New List(Of StatFileTreeLeaf))
        End Get
        Set(ByVal value As List(Of StatFileTreeLeaf))
            ViewState("LeafNodes") = value
        End Set
    End Property
    Public Property LoadedItems As StatTreeNode(Of StatFileTreeLeaf) Implements IViewModuleItemFilesSelector.LoadedItems
        Get
            If TypeOf (ViewState("LoadedItems")) Is StatTreeNode(Of StatFileTreeLeaf) Then
                Return ViewState("LoadedItems")
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As StatTreeNode(Of StatFileTreeLeaf))
            ViewState("LoadedItems") = value
        End Set
    End Property
    Public ReadOnly Property HasPermissionToSelectFile As Boolean Implements lm.Comol.Core.BaseModules.Repository.Presentation.IViewModuleItemFilesSelector.HasPermissionToSelectFile
        Get

        End Get
    End Property
    Private Function ViewStateOrDefault(Of T)(ByVal Key As String, ByVal DefaultValue As T) As T
        If (ViewState(Key) Is Nothing) Then
            ViewState(Key) = DefaultValue
            Return DefaultValue
        Else
            Return ViewState(Key)
        End If
    End Function
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Private _BaseTreeUrl As String
    Private ReadOnly Property BaseTreeUrl() As String
        Get
            If _BaseTreeUrl = "" Then
                _BaseTreeUrl = Me.PageUtility.BaseUrl & "RadControls/TreeView/Skins/Materiale/"
            End If
            Return _BaseTreeUrl
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub InitializeNoPermission(ByVal idCommunity As Integer) Implements IViewModuleItemFilesSelector.InitializeNoPermission
        MLVselector.SetActiveView(VIWempty)
    End Sub

    Public Sub InitializeView(ByVal moduleCode As String, ByVal objectId As Integer, ByVal objectTypeId As Integer, ByVal FilesIds As IList(Of Long), ByVal idCommunity As Integer) Implements IViewModuleItemFilesSelector.InitializeView
        MLVselector.SetActiveView(VIWselector)
        Dim RootNode As StatTreeNode(Of StatFileTreeLeaf) = Nothing
        Dim Translations As New Dictionary(Of Integer, String)

        Select Case moduleCode
            Case CoreModuleRepository.UniqueID
                Dim oService As New lm.Comol.Core.BaseModules.Repository.Business.ServiceCommunityRepository(CurrentContext)
                RootNode = oService.GetObjectItemFilesForStatistics(idCommunity, CurrentContext.UserContext.CurrentUserID, objectId, objectTypeId, New Dictionary(Of Integer, String)())
            Case lm.Comol.Core.BaseModules.CommunityDiary.Domain.ModuleCommunityDiary.UniqueID
                Dim oService As New lm.Comol.Core.BaseModules.CommunityDiary.Business.ServiceCommunityDiary(CurrentContext)
                Translations.Add(lm.Comol.Core.BaseModules.CommunityDiary.Domain.TreeItemsTranslations.PortalDiaryName, Me.Resource.getValue("SRVLEZ.1"))
                Translations.Add(lm.Comol.Core.BaseModules.CommunityDiary.Domain.TreeItemsTranslations.PortalDiaryNameToolTip, Me.Resource.getValue("SRVLEZ.2"))
                Translations.Add(lm.Comol.Core.BaseModules.CommunityDiary.Domain.TreeItemsTranslations.DiaryName, Me.Resource.getValue("SRVLEZ.3"))
                Translations.Add(lm.Comol.Core.BaseModules.CommunityDiary.Domain.TreeItemsTranslations.DiaryNameToolTip, Me.Resource.getValue("SRVLEZ.4"))
                Translations.Add(lm.Comol.Core.BaseModules.CommunityDiary.Domain.TreeItemsTranslations.StandardDiaryItemName, Me.Resource.getValue("SRVLEZ.5"))
                Translations.Add(lm.Comol.Core.BaseModules.CommunityDiary.Domain.TreeItemsTranslations.StandardDiaryItemNameToolTip, Me.Resource.getValue("SRVLEZ.6"))
                Translations.Add(lm.Comol.Core.BaseModules.CommunityDiary.Domain.TreeItemsTranslations.NoDateDiaryItemName, Me.Resource.getValue("SRVLEZ.7"))
                Translations.Add(lm.Comol.Core.BaseModules.CommunityDiary.Domain.TreeItemsTranslations.NoDateDiaryItemNameToolTip, Me.Resource.getValue("SRVLEZ.8"))
          
                RootNode = oService.GetObjectItemFilesForStatistics(objectId, objectTypeId, Translations, idCommunity, CurrentContext.UserContext.CurrentUserID)
            Case lm.Comol.Modules.TaskList.ModuleTasklist.UniqueID
                Dim oService As New lm.Comol.Modules.TaskList.ServiceTaskList(CurrentContext)
                RootNode = oService.GetObjectItemFilesForStatistics(objectId, objectTypeId, Translations, idCommunity, CurrentContext.UserContext.CurrentUserID)
        End Select
        LoadedItems = RootNode
        If RootNode Is Nothing Then
            Me.MLVselector.SetActiveView(VIWempty)
        Else
            LoadTree(RootNode)
            Me.MLVselector.SetActiveView(VIWselector)
        End If
    End Sub
    

    Public Sub ViewFileByType(ByVal type As lm.Comol.Core.DomainModel.StatTreeLeafType) Implements lm.Comol.Core.BaseModules.Repository.Presentation.IViewModuleItemFilesSelector.ViewFileByType

    End Sub
#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Modules", "Modules")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region

#Region "Load Tree"
    Private Sub LoadTree(ByVal RootNode As StatTreeNode(Of StatFileTreeLeaf))
        Dim oRootNode As New Telerik.Web.UI.RadTreeNode
        Me.RDTcommunityRepository.Nodes.Clear()
        oRootNode = CreateNode(RootNode, True)
        Me.RDTcommunityRepository.Nodes.Add(oRootNode)
        RecursivelyPopulate(RootNode, oRootNode)

    End Sub
    Private Sub RecursivelyPopulate(ByVal oNodeContainer As StatTreeNode(Of StatFileTreeLeaf), ByVal oFatherNode As Telerik.Web.UI.RadTreeNode)
        For Each oNode As StatTreeNode(Of StatFileTreeLeaf) In oNodeContainer.Nodes
            Dim oNodeFolder As Telerik.Web.UI.RadTreeNode = Me.CreateNode(oNode, (From l In oNode.Leaves Where l.Selected Select l).Any)
            Me.RecursivelyPopulate(oNode, oNodeFolder)
            oFatherNode.Nodes.Add(oNodeFolder)
        Next
        For Each leaf As StatFileTreeLeaf In oNodeContainer.Leaves
            oFatherNode.Nodes.Add(Me.CreateLeafNode(leaf))
        Next
        Dim leafnodes As List(Of StatFileTreeLeaf) = Me.LeafNodes
        leafnodes.AddRange(oNodeContainer.Leaves)
        Me.LeafNodes = leafnodes
    End Sub

    Private Function CreateLeafNode(ByVal leaf As StatFileTreeLeaf) As Telerik.Web.UI.RadTreeNode
        Dim oNode As Telerik.Web.UI.RadTreeNode = CreateBaseNode(leaf, True, False)

        oNode.Category = leaf.Id
        oNode.ImageUrl = Me.BaseUrl & Me.PageUtility.SystemSettings.Extension.FindIconImage(leaf.Extension)
        Return oNode
    End Function
    Private Function CreateNode(ByVal nodeContainer As StatTreeNode(Of StatFileTreeLeaf), ByVal Expanded As Boolean) As Telerik.Web.UI.RadTreeNode
        Dim oNode As Telerik.Web.UI.RadTreeNode = CreateBaseNode(nodeContainer, True, Expanded)
        oNode.Value = nodeContainer.Id
        oNode.Category = -nodeContainer.Id
        oNode.ImageUrl = Me.BaseTreeUrl & "folder.gif"
        oNode.ExpandedImageUrl = Me.BaseTreeUrl & "folderOpen.gif"
        oNode.ToolTip = nodeContainer.ToolTip
        Return oNode
    End Function
    Private Function CreateBaseNode(ByVal node As StatBaseTreeNode, ByVal selectable As Boolean, ByVal Expanded As Boolean) As Telerik.Web.UI.RadTreeNode
        Dim oNode As New Telerik.Web.UI.RadTreeNode
        oNode.Value = node.Id
        oNode.Checked = node.Selected
        oNode.Checkable = selectable
        oNode.Text = " " & node.Name
        oNode.Expanded = Expanded
        oNode.ToolTip = node.ToolTip
        Return oNode
    End Function
#End Region

    Public Sub UnselectAllFiles() Implements IViewModuleItemFilesSelector.UnselectAllFiles
        Me.RDTcommunityRepository.ClearCheckedNodes()
    End Sub

  
End Class