Imports lm.Comol.Core.FileRepository.Domain
Public Class UC_TreeTelerik
    Inherits FRbaseControl

#Region "Internal"
    Private _EnableFolderAutoNavigation As Boolean
    Private _EnableSelection As Boolean
    Private _AutoOpenFolder As Boolean
    Public Property AutoOpenFolder As Boolean
        Get
            Return _AutoOpenFolder
        End Get
        Set(value As Boolean)
            _AutoOpenFolder = value
        End Set
    End Property
    Public Property EnableSelection As Boolean
        Get
            Return _EnableSelection
        End Get
        Set(value As Boolean)
            _EnableSelection = value
        End Set
    End Property

    Public Property EnableFolderAutoNavigation As Boolean
        Get
            Return _EnableFolderAutoNavigation
        End Get
        Set(value As Boolean)
            _EnableFolderAutoNavigation = value
        End Set
    End Property
    Private DisplaySeparator As Boolean

    Public Property CurrentOrderBy As OrderBy
        Get
            Return ViewStateOrDefault("CurrentOrderBy", OrderBy.name)
        End Get
        Set(value As OrderBy)
            ViewState("CurrentOrderBy") = value
        End Set
    End Property
    Public Property CurrentAscending As Boolean
        Get
            Return ViewStateOrDefault("CurrentAscending", True)
        End Get
        Set(value As Boolean)
            ViewState("CurrentAscending") = value
        End Set
    End Property
    Private _AutoPostBack As Boolean
    Public Property AutoPostBack As Boolean
        Get
            Return ViewStateOrDefault("AutoPostBack", False)
        End Get
        Set(value As Boolean)
            _AutoPostBack = value
            ViewState("AutoPostBack") = value
        End Set
    End Property
    Public Event SelectedFolder(idFolder As Long, path As String, type As FolderType)

    Private _RepositoryIdentifier As RepositoryIdentifier
    Public Property RepositoryIdentifier As RepositoryIdentifier
        Get
            If IsNothing(_RepositoryIdentifier) Then
                _RepositoryIdentifier = ViewStateOrDefault("RepositoryIdentifier", lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier.Create(RepositoryType.Community, 0))
            End If
            Return _RepositoryIdentifier
        End Get
        Set(value As RepositoryIdentifier)
            _RepositoryIdentifier = value
            ViewState("RepositoryIdentifier") = value
        End Set
    End Property
    Private _RepositoryCacheKey As String
    Private Property RepositoryCacheKey As String
        Get
            If String.IsNullOrWhiteSpace(_RepositoryCacheKey) Then
                _RepositoryCacheKey = ViewStateOrDefault("RepositoryCacheKey", "")
            End If
            Return _RepositoryCacheKey
        End Get
        Set(value As String)
            _RepositoryCacheKey = value
            ViewState("RepositoryCacheKey") = value
        End Set
    End Property

    Public Property Width As System.Web.UI.WebControls.Unit
        Get
            Return RDTtree.Width
        End Get
        Set(value As System.Web.UI.WebControls.Unit)
            RDTtree.Width = value
        End Set
    End Property
    Public Property Height As System.Web.UI.WebControls.Unit
        Get
            Return RDTtree.Height
        End Get
        Set(value As System.Web.UI.WebControls.Unit)
            RDTtree.Height = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region

#Region "Internal"
    Public Sub InitializeControl(currentFolder As dtoDisplayRepositoryItem, items As List(Of dtoDisplayRepositoryItem), identifier As RepositoryIdentifier, cacheKey As String)
        InitializeControl(currentFolder, items, OrderBy.name, True, identifier, cacheKey)
    End Sub
    Public Sub InitializeControl(currentFolder As dtoDisplayRepositoryItem, items As List(Of dtoDisplayRepositoryItem), order As OrderBy, ascending As Boolean, identifier As RepositoryIdentifier, cacheKey As String)
        RepositoryIdentifier = identifier
        RepositoryCacheKey = cacheKey
        CurrentOrderBy = order
        CurrentAscending = ascending
        LoadTree(currentFolder, items, order, ascending, identifier, cacheKey)
        For Each node As Telerik.Web.UI.RadTreeNode In RDTtree.SelectedNodes
            node.ExpandParentNodes()
        Next

    End Sub


    Private Sub LoadTree(currentFolder As dtoDisplayRepositoryItem, items As List(Of dtoDisplayRepositoryItem), order As OrderBy, ascending As Boolean, identifier As RepositoryIdentifier, cacheKey As String)
        Dim oRootNode As New Telerik.Web.UI.RadTreeNode
        RDTtree.Visible = True
        RDTtree.Nodes.Clear()
        oRootNode = CreateRootNode(currentFolder, identifier, cacheKey)
        RDTtree.Nodes.Add(oRootNode)
        LoadTree(oRootNode, currentFolder, items.Where(Function(i) i.Type = ItemType.Folder AndAlso i.FolderType = FolderType.standard).ToList, order, ascending, identifier, cacheKey)

        If items.Any(Function(i) i.Type = ItemType.Folder AndAlso i.FolderType <> FolderType.standard) Then
            Dim isFirst As Boolean = True
            For Each item As dtoDisplayRepositoryItem In items.Where(Function(i) i.Type = ItemType.Folder AndAlso i.FolderType <> FolderType.standard)
                Dim folder As Telerik.Web.UI.RadTreeNode = CreateNode(item, currentFolder, order, ascending, identifier, cacheKey)
                If isFirst Then
                    folder.CssClass &= " firstseparator"
                    isFirst = False
                End If
                Select Case item.FolderType
                    Case FolderType.recycleBin
                        Exit Select
                    Case Else
                        LoadTree(folder, currentFolder, item.Children.Where(Function(c) c.Type = ItemType.Folder).ToList(), order, ascending, identifier, cacheKey)
                End Select
                RDTtree.Nodes.Add(folder)
            Next
        End If
    End Sub
    Private Sub LoadTree(ByVal fNode As Telerik.Web.UI.RadTreeNode, currentFolder As dtoDisplayRepositoryItem, items As List(Of dtoDisplayRepositoryItem), order As OrderBy, ascending As Boolean, identifier As RepositoryIdentifier, cacheKey As String)
        For Each item As dtoDisplayRepositoryItem In items.Where(Function(i) i.Type = ItemType.Folder).OrderBy(Function(f) f.Name)
            Dim folder As Telerik.Web.UI.RadTreeNode = CreateNode(item, currentFolder, order, ascending, identifier, cacheKey)
            Select Case item.FolderType
                Case FolderType.recycleBin
                    Exit Select
                Case Else
                    LoadTree(folder, currentFolder, item.Children.Where(Function(c) c.Type = ItemType.Folder).ToList(), order, ascending, identifier, cacheKey)
            End Select
            fNode.Nodes.Add(folder)
        Next
    End Sub
    Private Function CreateRootNode(currentFolder As dtoDisplayRepositoryItem, identifier As RepositoryIdentifier, cacheKey As String) As Telerik.Web.UI.RadTreeNode
        Dim oNode As New Telerik.Web.UI.RadTreeNode
        Try
            oNode.Value = "0||" & FolderType.standard.ToString() & "|" & cacheKey
            oNode.Text = GetFilenameRender(GetRootFolderFullname())
            oNode.Expanded = True
            oNode.Selected = IsNothing(currentFolder) OrElse (currentFolder.Id = 0 AndAlso currentFolder.FolderType = FolderType.standard)
            oNode.SelectedCssClass = LTselectedItemCssClass.Text
            oNode.CssClass = "treenode directory"
            oNode.ContentCssClass = "content"
            oNode.Category = RootObject.FolderUrlTemplate(0, FolderType.standard, identifier.Type, identifier.IdCommunity, identifier.IdPerson)
        Catch ex As Exception

        End Try
        Return oNode
    End Function
    Private Function CreateNode(item As dtoDisplayRepositoryItem, currentFolder As dtoDisplayRepositoryItem, order As OrderBy, ascending As Boolean, identifier As RepositoryIdentifier, cacheKey As String) As Telerik.Web.UI.RadTreeNode
        Dim oNode As New Telerik.Web.UI.RadTreeNode
        Try
            oNode.Value = item.Id & "|" & item.IdentifierPath & "|" & item.FolderType.ToString & "|" & cacheKey
            oNode.Text = GetFilenameRender(item.Name)
            oNode.Selected = Not IsNothing(currentFolder) AndAlso ((currentFolder.Id > 0 AndAlso currentFolder.Id = item.Id) OrElse (currentFolder.Id < 0 AndAlso currentFolder.FolderType = item.FolderType AndAlso currentFolder.Id = item.Id AndAlso currentFolder.IdentifierPath = item.IdentifierPath))
            oNode.SelectedCssClass = LTselectedItemCssClass.Text
            oNode.CssClass &= " treenode directory"
            Select Case item.FolderType
                Case FolderType.recycleBin
                    oNode.CssClass &= " " & LTtemplateRecycleBinCssClass.Text
            End Select

            If item.IsEmpty Then
                oNode.CssClass &= " " & LTemptyItemCssClass.Text
            End If
            Select Case item.FolderType
                Case FolderType.standard
                    oNode.Category = RootObject.FolderUrlTemplate(item.Id, item.FolderType, identifier.Type, identifier.IdCommunity)
                Case Else
                    oNode.Category = RootObject.FolderUrlTemplate(item.Id, item.FolderType, item.IdentifierPath, identifier.Type, identifier.IdCommunity)
            End Select
            If AutoPostBack Then
                oNode.Text = GetFilenameRender(item.Name)
            Else
                oNode.Text = String.Format(LTtemplateFolderNodeUrl.Text, BaseUrl & Replace(Replace(oNode.Category, "#OrderBy#", order.ToString), "#Boolean#", ascending.ToString().ToLower), GetFilenameRender(item.Name))
            End If
        Catch ex As Exception

        End Try
        Return oNode
    End Function

    Protected Friend Function GetFilenameRender(folderName As String) As String
        Return String.Format(LTtemplateFolderNodeText.Text, folderName)
    End Function
    Private Function GetRootFolderFullname() As String
        Return Resource.getValue("RootFolder")
    End Function

    
    Public Sub UpdateSelectedFolder(idFolder As Long, path As String, folderType As FolderType)
        If RDTtree.Nodes.Count > 0 Then
            RDTtree.UnselectAllNodes()
            If idFolder = 0 AndAlso (folderType = lm.Comol.Core.FileRepository.Domain.FolderType.standard) Then
                RDTtree.Nodes(0).Selected = True
            Else
                Dim oNode As Telerik.Web.UI.RadTreeNode = Nothing
                oNode = RDTtree.FindNodeByValue(idFolder & "|" & path & "|" & folderType.ToString & "|" & RepositoryCacheKey)
                If Not IsNothing(oNode) Then
                    oNode.Selected = True
                End If
            End If
        End If
    End Sub
#End Region

    Private Sub RDTtree_NodeClick(sender As Object, e As Telerik.Web.UI.RadTreeNodeEventArgs) Handles RDTtree.NodeClick
        If e.Node.Value.Contains("|") Then
            Dim values As String() = e.Node.Value.Split("|")
            Dim idFolder As Long = 0
            Long.TryParse(values(0), idFolder)
            Dim fType As FolderType = FolderType.standard
            fType = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of FolderType).GetByString(values(2), FolderType.standard)

            RaiseEvent SelectedFolder(idFolder, values(1), fType)
        Else
            RaiseEvent SelectedFolder(0, "", FolderType.standard)
        End If
    End Sub
End Class