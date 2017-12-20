Imports lm.Comol.Core.FileRepository.Domain
Imports lm.Comol.Core.BaseModules.FileRepository.Presentation
Imports lm.Comol.Core.BaseModules.FileRepository.Presentation.Domain
Imports lm.Comol.Core.DomainModel
Public Class UC_SelectRepositoryItemsTreeMode
    Inherits FRitemsSelectorControl
    Implements IViewItemsSelectorTreeMode

#Region "Context"
    Private _Presenter As ItemsSelectorTreeModePresenter
    Public ReadOnly Property CurrentPresenter() As ItemsSelectorTreeModePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ItemsSelectorTreeModePresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property TreeSelect As TreeMode Implements IViewItemsSelectorTreeMode.TreeSelect
        Get
            Return ViewStateOrDefault("TreeSelect", TreeMode.multiselect)
        End Get
        Set(value As TreeMode)
            ViewState("TreeSelect") = value
            RDTtree.CheckBoxes = (value <> TreeMode.noselect)

            RDTtree.MultipleSelect = False
            RDTtree.TriStateCheckBoxes = (value <> TreeMode.multiselect AndAlso value <> TreeMode.singleselect AndAlso value <> TreeMode.noselect)
            RDTtree.CheckChildNodes = (value = TreeMode.cascadeselect OrElse value = TreeMode.tristateselect)
        End Set
    End Property
    Public Property FolderSelectable As Boolean Implements IViewItemsSelectorTreeMode.FolderSelectable
        Get
            Return ViewStateOrDefault("FolderSelectable", False)
        End Get
        Set(value As Boolean)
            ViewState("FolderSelectable") = value
        End Set
    End Property
    Public Property RemoveEmptyFolders As Boolean Implements IViewItemsSelectorTreeMode.RemoveEmptyFolders
        Get
            Return ViewStateOrDefault("RemoveEmptyFolders", False)
        End Get
        Set(value As Boolean)
            ViewState("RemoveEmptyFolders") = value
        End Set
    End Property
    Private Property HasItemsToSelect As Boolean Implements IViewItemsSelectorTreeMode.HasItemsToSelect
        Get
            Return ViewStateOrDefault("HasItemsToSelect", False)
        End Get
        Set(value As Boolean)
            ViewState("HasItemsToSelect") = value
        End Set
    End Property
    Private Property ControlUniqueId As Guid Implements IViewItemsSelectorTreeMode.ControlUniqueId
        Get
            Return ViewStateOrDefault("ControlUniqueId", Guid.Empty)
        End Get
        Set(value As Guid)
            ViewState("ControlUniqueId") = value
        End Set
    End Property
#End Region

#Region "Internal"
    Public Property MaxSelectorHeight As System.Web.UI.WebControls.Unit
        Get
            Return RDTtree.Height
        End Get
        Set(ByVal value As System.Web.UI.WebControls.Unit)
            RDTtree.Height = value
        End Set
    End Property
    Public Property MaxSelectorWidth As System.Web.UI.WebControls.Unit
        Get
            Return RDTtree.Width
        End Get
        Set(ByVal value As System.Web.UI.WebControls.Unit)
            RDTtree.Width = value
        End Set
    End Property
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource

        End With
    End Sub
    Protected Friend Overrides Sub InternalInitialize(idUser As Integer, identifier As RepositoryIdentifier, adminMode As Boolean, showHiddenItems As Boolean, disableNotAvailableItems As Boolean, typesToLoad As List(Of ItemType), availability As ItemAvailability, displayStatistics As List(Of StatisticType), idRemovedItems As List(Of Long), idSelectedItems As List(Of Long), orderBy As OrderBy, ascending As Boolean)
        CurrentPresenter.InitView(identifier, adminMode, showHiddenItems, disableNotAvailableItems, typesToLoad, availability, displayStatistics, idRemovedItems, idSelectedItems)
    End Sub

    Protected Friend Overrides Sub SetSessionTimeout()
        Dim nodes As List(Of Telerik.Web.UI.RadTreeNode) = RDTtree.GetAllNodes.Where(Function(n) n.Checkable).ToList
        RDTtree.UncheckAllNodes()
        RDTtree.UnselectAllNodes()
        For Each n As Telerik.Web.UI.RadTreeNode In nodes
            n.Checkable = False
        Next
    End Sub
    Protected Friend Overrides Function GetCurrentItemsSelection() As Dictionary(Of Boolean, List(Of Long))
        Dim items As New Dictionary(Of Boolean, List(Of Long))
        Dim nodes As List(Of Telerik.Web.UI.RadTreeNode) = RDTtree.GetAllNodes
        items.Add(True, nodes.Where(Function(n) (TreeSelect = TreeMode.singleselect AndAlso n.Selected) OrElse (TreeSelect <> TreeMode.singleselect AndAlso n.Checked)).Select(Function(n) Long.Parse(n.Value)).Distinct().ToList())
        items.Add(False, nodes.Where(Function(n) (TreeSelect = TreeMode.singleselect AndAlso Not n.Selected) OrElse (TreeSelect <> TreeMode.singleselect AndAlso Not n.Checked)).Select(Function(n) Long.Parse(n.Value)).Distinct().ToList())
        Return items
    End Function
    Protected Friend Overrides Function HasAvailableItemsToSelect() As Boolean
        Return HasItemsToSelect
    End Function
#End Region

#Region "Implements"
    Public Sub InitializeControlForModule(idUser As Integer, identifier As RepositoryIdentifier, loadSelectedItems As Boolean, adminMode As Boolean, showHiddenItems As Boolean, disableNotAvailableItems As Boolean, availability As ItemAvailability, allowSelectFolder As Boolean, idSelectedItems As List(Of Long), Optional idRemovedItems As List(Of Long) = Nothing) Implements IViewItemsSelectorTreeMode.InitializeControlForModule
        CurrentPresenter.InitView(identifier, loadSelectedItems, adminMode, showHiddenItems, disableNotAvailableItems, availability, allowSelectFolder, idSelectedItems, idRemovedItems)
    End Sub
    Public Sub InitializeControlForModule(idUser As Integer, identifier As RepositoryIdentifier, loadSelectedItems As Boolean, adminMode As Boolean, showHiddenItems As Boolean, disableNotAvailableItems As Boolean, typesToLoad As List(Of ItemType), availability As ItemAvailability, allowSelectFolder As Boolean, idSelectedItems As List(Of Long), Optional idRemovedItems As List(Of Long) = Nothing) Implements IViewItemsSelectorTreeMode.InitializeControlForModule
        CurrentPresenter.InitView(identifier, loadSelectedItems, adminMode, showHiddenItems, disableNotAvailableItems, typesToLoad, availability, allowSelectFolder, idSelectedItems, idRemovedItems)
    End Sub
    Private Function GetRootFolderFullname() As String Implements IViewItemsSelectorTreeMode.GetRootFolderFullname
        Return Resource.getValue("RootFolder")
    End Function
    Private Function GetRootFolderName() As String Implements IViewItemsSelectorTreeMode.GetRootFolderName
        Return Resource.getValue("RootFolderName")
    End Function
    Private Sub LoadItems(items As List(Of dtoRepositoryItemToSelect)) Implements IViewItemsSelectorTreeMode.LoadItems
        HasItemsToSelect = IsNothing(items) OrElse Not items.Any()
        If Not IsNothing(items) Then
            RPTitems.DataSource = items
            RPTitems.DataBind()
            Dim uniqueId As Guid = ControlUniqueId
            If uniqueId = Guid.Empty Then
                uniqueId = Guid.NewGuid
                ControlUniqueId = uniqueId
            End If
            RenderTree(uniqueId.ToString, items)

            LoadTree(items)
        Else
            RPTitems.DataSource = New List(Of dtoRepositoryItemToSelect)
            RPTitems.DataBind()
            RDTtree.Visible = False
            LTrenderTree.Visible = False
            CTRLmessages.Visible = True
            CTRLmessages.InitializeControl(Resource.getValue("IViewItemsSelectorTreeMode.NoItemsFound"), Helpers.MessageType.error)
        End If
    End Sub
#End Region

#Region "Internal"
    Public Sub UnselectAll()
        RDTtree.UncheckAllNodes()
        RDTtree.UnselectAllNodes()
    End Sub
    Public Sub LoadTree(items As List(Of dtoRepositoryItemToSelect))
        PopulateFolderFiles(items)
        Dim oRootNode As New Telerik.Web.UI.RadTreeNode
        RDTtree.Visible = True
        RDTtree.Nodes.Clear()

        oRootNode = CreateRootNode(FolderSelectable OrElse TreeSelect = TreeMode.cascadeselect)
        RDTtree.Nodes.Add(oRootNode)
        LoadTree(oRootNode, items, 0)
    End Sub
    Private Sub LoadTree(ByVal fNode As Telerik.Web.UI.RadTreeNode, items As List(Of dtoRepositoryItemToSelect), idFolder As Long)
        For Each item As dtoRepositoryItemToSelect In items.Where(Function(i) i.IdFolder = idFolder).OrderBy(Function(f) f.OrderByFolder).ThenBy(Function(f) f.Name)
            Select Case item.Type
                Case ItemType.Folder
                    If Not RemoveEmptyFolders OrElse Not _EmptyFolders.Contains(item.Id) Then
                        Dim folder As Telerik.Web.UI.RadTreeNode = CreateNode(item)
                        LoadTree(folder, items, item.Id)
                        fNode.Nodes.Add(folder)
                    End If
                Case Else
                    fNode.Nodes.Add(CreateNode(item))
            End Select
        Next
    End Sub
    Private Function CreateRootNode(selectable As Boolean) As Telerik.Web.UI.RadTreeNode
        Dim oNode As New Telerik.Web.UI.RadTreeNode
        Try
            oNode.Value = 0
            oNode.Text = GetFilenameRender(GetRootFolderFullname(), "", ItemType.Folder)
            oNode.Expanded = True
            oNode.Checkable = selectable
            oNode.CssClass = "treenode directory"
            oNode.ContentCssClass = "content"
        Catch ex As Exception

        End Try
        Return oNode
    End Function
    Private Function CreateNode(item As dtoRepositoryItemToSelect) As Telerik.Web.UI.RadTreeNode
        Dim oNode As New Telerik.Web.UI.RadTreeNode
        Try
            oNode.Value = item.Id
            oNode.Category = item.Id.ToString & "|" & item.Type.ToString() & "|" & item.UniqueId.ToString & "|" & item.IdVersion.ToString & "|" & item.UniqueIdVersion.ToString & "|" & item.IgnoreVersion.ToString & "|" & item.Path.ToString
            Select Case item.Type
                Case ItemType.Folder
                    Dim render As String = LTtemplateItemFolder.Text
                    render = Replace(render, "#name#", GetFilenameRender(item.DisplayName, item.Extension, item.Type))

                    oNode.Text = render
                    oNode.CssClass = "treenode directory"
                    oNode.ContentCssClass = "content"
                Case Else
                    Dim render As String = LTtemplateItemFile.Text
                    render = Replace(render, "#name#", GetFilenameRender(item.DisplayName, item.Extension, item.Type))
                    render = Replace(render, "#filesize#", IIf(item.Type <> ItemType.Link, item.GetSize, ""))
                    oNode.Text = render
                    oNode.CssClass = "treenode file"
            End Select
            oNode.Checkable = item.Selectable AndAlso Not _UnselectableFolders.Contains(item.Id)
            oNode.Checked = item.Selected AndAlso oNode.Checkable
        Catch ex As Exception

        End Try
        Return oNode
    End Function
    '    Private Sub DisplayNoFilesFound() Implements IViewCommunityFileSelector.DisplayNoFilesFound
    '        Me.LTnofile.Visible = True
    '        Me.RDTcommunityRepository.Visible = False
    '    End Sub
    '    Public Sub UnselectAll() Implements IViewCommunityFileSelector.UnselectAll
    '        Me.RDTcommunityRepository.ClearCheckedNodes()
    '    End Sub
    '#End Region




#Region "Render Tree"
    Private _EmptyFolders As List(Of Long)
    Private _UnselectableFolders As List(Of Long)
    Private Sub PopulateFolderFiles(items As List(Of dtoRepositoryItemToSelect))
        _EmptyFolders = items.Where(Function(i) i.Type = ItemType.Folder).Select(Function(f) f.Id).Distinct().ToList()
        _UnselectableFolders = _EmptyFolders
        Dim idFileFolders As List(Of Long) = items.Where(Function(i) i.Type <> ItemType.Folder).Select(Function(f) f.IdFolder).Distinct().ToList()
        Dim idSelectableFolders As List(Of Long) = items.Where(Function(i) i.Selectable AndAlso i.Type <> ItemType.Folder).Select(Function(f) f.IdFolder).Distinct().ToList()
        _EmptyFolders = _EmptyFolders.Except(idFileFolders).ToList
        _UnselectableFolders = _UnselectableFolders.Except(idSelectableFolders).ToList
        If _EmptyFolders.Any() Then
            Dim folders As Dictionary(Of Long, Long) = items.Where(Function(i) i.Type = ItemType.Folder).ToDictionary(Function(f) f.Id, Function(f) f.IdFolder)

            Dim idFathers As List(Of Long) = folders.Where(Function(f) idFileFolders.Contains(f.Key)).Select(Function(f) f.Value).Distinct().ToList()
            While idFathers.Any()
                idFileFolders.AddRange(idFathers)
                idFathers = folders.Where(Function(f) idFathers.Contains(f.Key)).Select(Function(f) f.Value).Distinct().ToList()
            End While
            idFileFolders.AddRange(idFathers)
            _EmptyFolders = _EmptyFolders.Except(idFileFolders.Distinct().ToList()).ToList()
        End If
        If _UnselectableFolders.Any() Then
            Dim folders As Dictionary(Of Long, Long) = items.Where(Function(i) i.Type = ItemType.Folder).ToDictionary(Function(f) f.Id, Function(f) f.IdFolder)

            Dim idFathers As List(Of Long) = folders.Where(Function(f) idSelectableFolders.Contains(f.Key)).Select(Function(f) f.Value).Distinct().ToList()
            While idFathers.Any()
                idSelectableFolders.AddRange(idFathers)
                idFathers = folders.Where(Function(f) idFathers.Contains(f.Key)).Select(Function(f) f.Value).Distinct().ToList()
            End While
            idSelectableFolders.AddRange(idFathers)
            _UnselectableFolders = _UnselectableFolders.Except(idSelectableFolders.Distinct().ToList()).ToList()
        End If
    End Sub
    Private Sub RenderTree(versionIdentifier As String, items As List(Of dtoRepositoryItemToSelect))
        PopulateFolderFiles(items)
        Dim render As String = LTtreeRoot.Text
        render = Replace(render, "#uniqueIdVersion#", versionIdentifier)
        render = Replace(render, "#name#", GetRootFolderName)
        If FolderSelectable OrElse TreeSelect = TreeMode.cascadeselect Then
            render = Replace(render, "#select#", RenderSelect(0, True, False))
        Else
            render = Replace(render, "#select#", "")
        End If
        render = Replace(render, "#selectmode#", LTtreeMode.Text.Split("|").ToList()(CInt(TreeSelect)))

        '      LTcookieTemplate.Text = Replace(LTcookieTemplate.Text, "#uniqueIdVersion#", versionIdentifier)
        If items.Any Then
            render = Replace(render, "#childrennodes#", RenderTree(items, 0))
        Else
            render = Replace(render, "#childrennodes#", "")
        End If
        LTrenderTree.Text = render
    End Sub
    Private Function RenderTree(items As List(Of dtoRepositoryItemToSelect), idFolder As Long) As String
        Dim render As String = ""
        Dim nodeRender As String = ""
        For Each item As dtoRepositoryItemToSelect In items.Where(Function(i) i.IdFolder = idFolder).OrderBy(Function(f) f.OrderByFolder).ThenBy(Function(f) f.Name)
            Select Case item.Type
                Case ItemType.Folder
                    If Not RemoveEmptyFolders OrElse Not _EmptyFolders.Contains(item.Id) Then
                        render &= RenderFolder(item, items)
                    End If
                Case Else
                    render &= RenderFile(item)
            End Select
            render &= nodeRender
        Next
        Return Replace(LTtreeChildrenNodes.Text, "#childrennodes#", render)
    End Function
    Private Function RenderFolder(folder As dtoRepositoryItemToSelect, items As List(Of dtoRepositoryItemToSelect)) As String
        Dim nodeRender As String = LTtreeFolderNode.Text
        nodeRender = Replace(nodeRender, "#name#", folder.Name)
        nodeRender = Replace(nodeRender, "#dataid#", folder.Id)
        If folder.Selectable AndAlso Not _UnselectableFolders.Contains(folder.Id) Then
            nodeRender = Replace(nodeRender, "#select#", RenderSelect(folder))
        Else
            nodeRender = Replace(nodeRender, "#select#", "")
        End If

        If items.Any(Function(f) f.IdFolder = folder.Id) Then
            nodeRender = Replace(nodeRender, "#childrennodes#", RenderTree(items, folder.Id))
        Else
            nodeRender = Replace(nodeRender, "#childrennodes#", "")
        End If
        Return nodeRender
    End Function
    Private Function RenderFile(item As dtoRepositoryItemToSelect) As String
        Dim nodeRender As String = LTtreeFileNode.Text
        nodeRender = Replace(nodeRender, "#name#", GetFilenameRender(item.DisplayName, item.Extension, item.Type))
        nodeRender = Replace(nodeRender, "#dataid#", item.Id)

        If item.Selectable Then
            nodeRender = Replace(nodeRender, "#select#", RenderSelect(item))
        Else
            nodeRender = Replace(nodeRender, "#select#", "")
        End If
        Select Case item.Type
            Case ItemType.Link, ItemType.SharedDocument
                nodeRender = Replace(nodeRender, "#filesize#", "")
            Case Else
                nodeRender = Replace(nodeRender, "#filesize#", item.GetSize())
        End Select
        Return nodeRender
    End Function
    Private Function RenderSelect(item As dtoRepositoryItemToSelect) As String
        Return RenderSelect(item.Id, item.Selectable, item.Selected AndAlso item.Selectable)
    End Function
    Private Function RenderSelect(idItem As Long, selectable As Boolean, selected As Boolean) As String
        Dim render As String = LTmultiselectItem.Text
        render = Replace(render, "#idItem#", idItem)
        render = Replace(render, "#enabled#", IIf(selectable, "", LTreadonlyMode.Text))
        render = Replace(render, "#checked#", IIf(selected, "checked", ""))
        Return render
    End Function
#End Region

    Protected Friend Overrides Function GetInternalSelectedItems() As List(Of dtoRepositoryItemToSelect)
        If LoadForModule Then
            Return GetItems()
        Else
            Return CurrentPresenter.GetSelectedItems(GetItems, RepositoryIdentifier, CurrentAdminMode, AvailableTypes, DisplayStatistics)
        End If
    End Function
    Private Function GetItems() As List(Of dtoRepositoryItemToSelect)
        Dim nodes As List(Of Telerik.Web.UI.RadTreeNode)
        Select Case TreeSelect
            Case TreeMode.singleselect
                nodes = RDTtree.GetAllNodes.Where(Function(n) n.Selected).ToList()
            Case Else
                nodes = RDTtree.GetAllNodes.Where(Function(n) n.Checked).ToList()
        End Select
        Return GetItems(nodes)
    End Function
    Private Function GetItems(nodes As List(Of Telerik.Web.UI.RadTreeNode), Optional ByVal fullLoad As Boolean = True) As List(Of dtoRepositoryItemToSelect)
        Dim items As New List(Of dtoRepositoryItemToSelect)
        For Each node As Telerik.Web.UI.RadTreeNode In nodes
            Dim item As New dtoRepositoryItemToSelect
            item.Id = node.Value
            If item.Id > 0 AndAlso fullLoad Then
                Dim values As List(Of String) = node.Category.Split("|").ToList
                item.Type = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ItemType).GetByString(values(1), ItemType.None)
                Guid.TryParse(values(2), item.UniqueId)
                Long.TryParse(values(3), item.IdVersion)
                Guid.TryParse(values(4), item.UniqueIdVersion)
                Boolean.TryParse(values(5), item.IgnoreVersion)
                item.Path = values(6)
            Else
                item.Type = ItemType.Folder
            End If
            items.Add(item)
        Next
        Return items
    End Function

    Private Function GetIdItems(alsoFolder As Boolean) As List(Of Long)
        Dim nodes As List(Of Telerik.Web.UI.RadTreeNode)
        Select Case TreeSelect
            Case TreeMode.singleselect
                nodes = RDTtree.GetAllNodes.Where(Function(n) n.Selected).ToList()
            Case Else
                nodes = RDTtree.GetAllNodes.Where(Function(n) n.Checked).ToList()
        End Select
        Return GetIdItems(alsoFolder, nodes)
    End Function
    Private Function GetIdItems(alsoFolder As Boolean, nodes As List(Of Telerik.Web.UI.RadTreeNode)) As List(Of Long)
        Dim items As New List(Of Long)
        Dim folderText As String = "|" & ItemType.Folder.ToString() & "|"
        For Each node As Telerik.Web.UI.RadTreeNode In nodes
            Dim idItem As Long
            Long.TryParse(node.Value, idItem)
            If idItem > 0 Then
                If Not node.Category.Contains(folderText) OrElse alsoFolder Then
                    items.Add(idItem)
                End If
            ElseIf alsoFolder Then
                items.Add(idItem)
            End If
        Next
        Return items.Distinct().ToList()
    End Function

    'Private Function GetItems() As List(Of dtoRepositoryItemToSelect)
    '    Dim allItems As List(Of dtoRepositoryItemToSelect) = GetAllLoadedItems()
    '    Dim selected As New List(Of dtoRepositoryItemToSelect)

    '    'Dim allHiddenKeys As List(Of String) = Request.Form.AllKeys.Where(Function(k) k.StartsWith("HDNitem_")).ToList
    '    'Dim allIdItems As List(Of Long) = allHiddenKeys.Select(Function(k) Long.Parse(k.Remove(8))).ToList()
    '    Dim allCheckBoxKeys As List(Of String) = Request.Form.AllKeys.Where(Function(k) k.StartsWith("CBitem_")).Select(Function(s) Replace(s, "CBitem_", "")).ToList
    '    Dim allIdSelectedItems As List(Of Long) = allCheckBoxKeys.Select(Function(k) Long.Parse(k)).ToList()

    '    Return allItems.Where(Function(i) allIdSelectedItems.Contains(i.Id)).ToList
    'End Function

    'Private Function GetAllLoadedItems() As List(Of dtoRepositoryItemToSelect)
    '    Dim items As New List(Of dtoRepositoryItemToSelect)
    '    For Each row As RepeaterItem In RPTitems.Items
    '        Dim item As New dtoRepositoryItemToSelect
    '        item.Path = DirectCast(row.FindControl("LTpath"), Literal).Text
    '        Dim values As List(Of String) = DirectCast(row.FindControl("LTitem"), Literal).Text.Split("|").ToList
    '        Long.TryParse(values(0), item.Id)
    '        item.Type = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ItemType).GetByString(values(1), ItemType.None)
    '        Guid.TryParse(values(2), item.UniqueId)
    '        Long.TryParse(values(3), item.IdVersion)
    '        Guid.TryParse(values(4), item.UniqueIdVersion)
    '        Boolean.TryParse(values(5), item.IgnoreVersion)
    '        items.Add(item)
    '    Next
    '    Return items
    'End Function
    Private Sub SetCookies(items As List(Of dtoRepositoryItemToSelect))
        'For Each item As dtoRepositoryItemToSelect In items
        '    Dim oCookie As HttpCookie = Request.Cookies(String.Format(LTcookieTemplate.Text, IIf(item.Type = ItemType.Folder, "folder", "file") & "-" & item.Id))
        '    If IsNothing(oCookie) Then
        '        oCookie = New HttpCookie(String.Format(LTcookieTemplate.Text, item.Id))
        '        oCookie.Value = Boolean.FalseString.ToLower()
        '        Response.Cookies.Add(oCookie)
        '    Else
        '        oCookie.Value = Boolean.FalseString.ToLower()
        '    End If
        'Next
    End Sub
    Protected Friend Function GetFilenameRender(fullname As String, fileExtension As String, type As ItemType) As String
        Dim template As String = LTtemplateFile.Text
        Select Case type
            Case lm.Comol.Core.FileRepository.Domain.ItemType.Folder
                template = Replace(template, "#ico#", LTitemFolderCssClass.Text)
            Case lm.Comol.Core.FileRepository.Domain.ItemType.Link
                template = Replace(template, "#ico#", LTitemUrlCssClass.Text)
            Case lm.Comol.Core.FileRepository.Domain.ItemType.Multimedia
                template = Replace(template, "#ico#", LTitemMultimediaCssClass.Text)
            Case lm.Comol.Core.FileRepository.Domain.ItemType.ScormPackage
                template = Replace(template, "#ico#", LTitemScormPackageCssClass.Text)
            Case Else
                If Not String.IsNullOrWhiteSpace(fileExtension) Then
                    fileExtension = fileExtension.ToLower
                End If
                If fileExtension.StartsWith(".") Then
                    template = Replace(template, "#ico#", LTitemExtensionCssClass.Text & Replace(fileExtension, ".", ""))
                Else
                    template = Replace(template, "#ico#", LTitemExtensionCssClass.Text)
                End If
        End Select
        template = Replace(template, "#name#", fullname)
        Return template
    End Function

    Public Sub DisableControl()
        RDTtree.Enabled = False
        RDTtree.UnselectAllNodes()
        RDTtree.UncheckAllNodes()
    End Sub
    Public Function GetSelectedFolders() As List(Of Long)
        Return (From oNode As Telerik.Web.UI.RadTreeNode In RDTtree.CheckedNodes Where oNode.Category.Contains("|" & ItemType.Folder.ToString() & "|") Select CLng(oNode.Value)).ToList
    End Function
    Public Function GetSelectedItemsActionLink() As List(Of lm.Comol.Core.DomainModel.ModuleActionLink)
        Return CurrentPresenter.GetSelectedItemsActionLink(GetIdItems(False))
    End Function
#End Region
End Class
