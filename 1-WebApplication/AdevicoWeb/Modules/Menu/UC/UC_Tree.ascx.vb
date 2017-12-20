Imports lm.Comol.Modules.Standard.Menu.Domain
Imports lm.Comol.Modules.Standard.Menu.Presentation

Public Class UC_MenubarTree
    Inherits BaseControl
    Implements IViewMenubarTree


    Public Event SelectedItemChanged(ByVal item As dtoItem)
    Public Event ItemMovedTo(ByVal startItem As dtoItem, ByVal endItem As dtoItem)
    Public Event ItemReorderedTo(ByVal startItem As dtoItem, ByVal endItem As dtoItem)
    Public Event ItemToFirstDisplay(ByVal startItem As dtoItem)

    Public Property SelectedItem As dtoItem Implements IViewMenubarTree.SelectedItem
        Get
            Dim node As Telerik.Web.UI.RadTreeNode = Me.RDTmenu.SelectedNode
            Dim item As dtoItem = Nothing
            If Not IsNothing(node) Then
                item = New dtoItem() With {.Id = node.Value, .Type = node.Category}
            End If

            Return item
        End Get
        Set(value As dtoItem)
            Dim selectedNode As New Telerik.Web.UI.RadTreeNode
            Try
                selectedNode = (From n As Telerik.Web.UI.RadTreeNode In Me.RDTmenu.GetAllNodes() _
                                       Where (n.Category = value.Type.ToString) AndAlso (n.ID = value.Id.ToString) Select n).FirstOrDefault()
            Catch ex As Exception

            End Try


            If Not IsNothing(selectedNode) Then
                selectedNode.Selected = True
                selectedNode.ExpandParentNodes()
                selectedNode.Expanded = True
                Me.UpdateDragAndDrop(value)
            End If

        End Set
    End Property

    Public Property EnableDragAndDrop As Boolean
        Get
            Return RDTmenu.EnableDragAndDrop
        End Get
        Set(value As Boolean)
            RDTmenu.EnableDragAndDrop = value
        End Set
    End Property

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_MenubarEdit", "Modules", "Menu")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()

    End Sub


    Public Sub InitalizeControl(ByVal item As dtoTree, ByVal selectedItem As dtoItem) Implements IViewMenubarTree.InitalizeControl
        LoadTree(item, selectedItem)
    End Sub

#Region "Load Tree"
    Private Sub LoadTree(ByVal rootItem As dtoTree, ByVal selectedItem As dtoItem)

        Dim expandedNodes As List(Of dtoItem) = (From n As Telerik.Web.UI.RadTreeNode In Me.RDTmenu.GetAllNodes() _
                                                  Where n.Expanded = True Select GetDtoFromNode(n)).ToList
        Dim oRootNode As New Telerik.Web.UI.RadTreeNode
        Me.RDTmenu.Nodes.Clear()
        oRootNode = CreateNode(rootItem)
        oRootNode.Expanded = True
        Me.RDTmenu.Nodes.Add(oRootNode)
        RecursivelyPopulate(rootItem, oRootNode)

        Dim selectedNode As New Telerik.Web.UI.RadTreeNode
        Try
            selectedNode = (From n As Telerik.Web.UI.RadTreeNode In Me.RDTmenu.GetAllNodes() _
                                   Where (n.Category = selectedItem.Type.ToString) AndAlso (n.Value = selectedItem.Id.ToString) Select n).FirstOrDefault()
        Catch ex As Exception

        End Try

        If IsNothing(selectedNode) Then
            selectedNode = oRootNode
        End If
        selectedNode.Selected = True
        selectedNode.ExpandParentNodes()
        selectedNode.Expanded = True
        If expandedNodes.Count > 0 Then

        End If

        Me.UpdateDragAndDrop(GetDtoFromNode(selectedNode))
    End Sub
    Private Sub RecursivelyPopulate(ByVal oNodeContainer As dtoTree, ByVal oFatherNode As Telerik.Web.UI.RadTreeNode)
        For Each oNode As dtoTree In oNodeContainer.Items
            Dim oNodeFolder As Telerik.Web.UI.RadTreeNode = Me.CreateNode(oNode)
            Me.RecursivelyPopulate(oNode, oNodeFolder)
            oFatherNode.Nodes.Add(oNodeFolder)
        Next
    End Sub

    Private Function CreateNode(ByVal node As dtoTree) As Telerik.Web.UI.RadTreeNode
        Dim oNode As New Telerik.Web.UI.RadTreeNode
        oNode.Value = node.Id
        'oNode.Checked = node.Selected
        oNode.Checkable = True 'selectable
        oNode.Text = GetItemIcon(node.Type) & " " & IIf(node.Type = MenuItemType.ItemColumn, "Column " & node.Name, node.Name)
        '   oNode.Expanded = Expanded
        Try
            oNode.ToolTip = String.Format(Me.Resource.getValue("MenuItemType." & node.Type.ToString), node.Name)

        Catch ex As Exception

        End Try

        oNode.Category = node.Category
        Return oNode
    End Function

    Private Function GetItemIcon(ByVal type As MenuItemType) As String
        'Test per grafica
        Dim BaseImgStr As String = "<img src=""" & Me.BaseUrl & "images/Menu/#Img#"" alt="""">"

        Select Case type
            Case MenuItemType.IconManage
                Return BaseImgStr.Replace("#Img", "IM.png") '
            Case MenuItemType.IconNewItem
                Return BaseImgStr.Replace("#Img", "IN.png") '
            Case MenuItemType.IconStatistic
                Return BaseImgStr.Replace("#Img", "IS.png") '
            Case MenuItemType.ItemColumn
                Return BaseImgStr.Replace("#Img", "Col.png")    '
            Case MenuItemType.Link
                Return BaseImgStr.Replace("#Img", "L.png")  '
            Case MenuItemType.LinkContainer
                Return BaseImgStr.Replace("#Img", "LC.png")
            Case MenuItemType.Menubar
                Return BaseImgStr.Replace("#Img", "M.png")  '
            Case MenuItemType.Separator
                Return BaseImgStr.Replace("#Img", "S.png")  '
            Case MenuItemType.Text
                Return BaseImgStr.Replace("#Img", "T.png")  '
            Case MenuItemType.TextContainer
                Return BaseImgStr.Replace("#Img", "TC.png") '
            Case MenuItemType.TopItemMenu
                Return BaseImgStr.Replace("#Img", "1M.png") '
            Case Else
                Return BaseImgStr.Replace("#Img", "none.png")
        End Select
        Return BaseImgStr.Replace("#Img", "none.png")   '

        'End test

        'Old code
        'Select Case type
        '    Case MenuItemType.IconManage
        '        Return "[IM]"
        '    Case MenuItemType.IconNewItem
        '        Return "[IN]"
        '    Case MenuItemType.IconStatistic
        '        Return "[IS]"
        '    Case MenuItemType.ItemColumn
        '        Return "[Col]"
        '    Case MenuItemType.Link
        '        Return "[L]"
        '    Case MenuItemType.LinkContainer
        '        Return "[LC]"
        '    Case MenuItemType.Menubar
        '        Return "[M]"
        '    Case MenuItemType.Separator
        '        Return "[S]"
        '    Case MenuItemType.Text
        '        Return "[T]"
        '    Case MenuItemType.TextContainer
        '        Return "[TC]"
        '    Case MenuItemType.TopItemMenu
        '        Return "[1M]"
        '    Case Else
        '        Return "[]"
        'End Select
        'Return "[]"
        'end old code
    End Function

    Public Sub UpdateDragAndDrop(ByVal item As dtoItem)
        Me.RDTmenu.EnableDragAndDrop = Not (item.Type = MenuItemType.Menubar)

        Dim oNodes As New List(Of Telerik.Web.UI.RadTreeNode)
        Dim disabledNodes As New List(Of Telerik.Web.UI.RadTreeNode)
        Select Case item.Type
            Case MenuItemType.TopItemMenu
                oNodes = (From n In Me.RDTmenu.GetAllNodes Where n.Category = MenuItemType.TopItemMenu.ToString() Select n).ToList
                disabledNodes = (From n In Me.RDTmenu.GetAllNodes Where n.Category <> MenuItemType.TopItemMenu.ToString() Select n).ToList
            Case MenuItemType.ItemColumn
                oNodes = (From n In Me.RDTmenu.GetAllNodes Where n.Category = MenuItemType.TopItemMenu.ToString() OrElse n.Category = MenuItemType.ItemColumn.ToString() Select n).ToList
                disabledNodes = (From n In Me.RDTmenu.GetAllNodes Where Not (n.Category = MenuItemType.TopItemMenu.ToString() OrElse n.Category = MenuItemType.ItemColumn.ToString()) Select n).ToList
            Case MenuItemType.TextContainer, MenuItemType.LinkContainer
                oNodes = (From n In Me.RDTmenu.GetAllNodes Where Not (n.Category = MenuItemType.TopItemMenu.ToString() OrElse n.Category = MenuItemType.Menubar.ToString() OrElse _
                                                                                      n.Category = MenuItemType.IconNewItem.ToString() OrElse n.Category = MenuItemType.IconManage.ToString() _
                                                                                     OrElse n.Category = MenuItemType.IconStatistic.ToString()) Select n).ToList

                Dim exceptNodes As New List(Of Telerik.Web.UI.RadTreeNode)
                Try
                    exceptNodes = oNodes.Where(Function(n) IsNothing(n.ParentNode) = False).ToList.Where(Function(n) (n.ParentNode.Category = item.Type.ToString AndAlso n.ParentNode.Value = item.Id) OrElse n.ParentNode.Category = MenuItemType.LinkContainer.ToString() OrElse n.ParentNode.Category = MenuItemType.TextContainer.ToString()).ToList()
                Catch ex As Exception

                End Try

                Dim list As New List(Of Telerik.Web.UI.RadTreeNode)
                list = (From n In exceptNodes Where n.Category = MenuItemType.ItemColumn.ToString Select n).ToList
                oNodes = (From n In oNodes Where Not exceptNodes.Contains(n) Select n).ToList
                list = (From n In oNodes Where n.Category = MenuItemType.ItemColumn.ToString Select n).ToList
                '  oNodes = (From n In oNodes Where exceptNodes.Contains(n) Select n).ToList
                disabledNodes = (From n In Me.RDTmenu.GetAllNodes Where Not oNodes.Contains(n) Select n).ToList
            Case MenuItemType.Separator, MenuItemType.Text, MenuItemType.Link
                oNodes = (From n In Me.RDTmenu.GetAllNodes Where Not (n.Category = MenuItemType.TopItemMenu.ToString() OrElse n.Category = MenuItemType.Menubar.ToString() OrElse _
                                                                                      n.Category = MenuItemType.IconNewItem.ToString() OrElse n.Category = MenuItemType.IconManage.ToString() _
                                                                                     OrElse n.Category = MenuItemType.IconStatistic.ToString()) Select n).ToList

                disabledNodes = (From n In Me.RDTmenu.GetAllNodes Where Not oNodes.Contains(n) Select n).ToList
            Case MenuItemType.IconStatistic, MenuItemType.IconManage, MenuItemType.IconNewItem
                oNodes = (From n In Me.RDTmenu.GetAllNodes Where (n.Category = MenuItemType.Text.ToString() OrElse n.Category = MenuItemType.Link.ToString() OrElse _
                                                                                    n.Category = MenuItemType.LinkContainer.ToString()) Select n).ToList

                Dim exceptNodes As New List(Of Telerik.Web.UI.RadTreeNode)
                Try
                    exceptNodes = oNodes.Where(Function(n) n.Nodes.Count > 0).ToList.Where(Function(n) (From node In n.Nodes Where node.Category = item.Type.ToString Select node).Any).ToList()
                Catch ex As Exception

                End Try
                oNodes = (From n In oNodes Where Not exceptNodes.Contains(n) Select n).ToList
                disabledNodes = (From n In Me.RDTmenu.GetAllNodes Where Not oNodes.Contains(n) Select n).ToList
            Case Else
                disabledNodes = Me.RDTmenu.GetAllNodes
        End Select
        For Each node As Telerik.Web.UI.RadTreeNode In oNodes
            node.AllowDrop = True
        Next
        For Each node As Telerik.Web.UI.RadTreeNode In disabledNodes
            node.AllowDrop = False
        Next
        'oNodes.ForEach(Function(n) n.AllowDrop = True)
        'disabledNodes.ForEach(Function(n) n.AllowDrop = False)
    End Sub

    Private Sub ExpandNodes(ByVal expandedNodes As List(Of dtoItem))
        Dim nodes As List(Of Telerik.Web.UI.RadTreeNode) = Me.RDTmenu.GetAllNodes()

        For Each dto As dtoItem In expandedNodes
            Dim node As Telerik.Web.UI.RadTreeNode = (From n In nodes Where n.Value = dto.Id AndAlso n.Category = dto.Type.ToString).FirstOrDefault
            If Not IsNothing(node) Then : node.Expanded = True
            End If
        Next
    End Sub
#End Region

    Private Sub RDTmenu_NodeClick(sender As Object, e As Telerik.Web.UI.RadTreeNodeEventArgs) Handles RDTmenu.NodeClick
        RaiseEvent SelectedItemChanged(GetDtoFromNode(e.Node))
    End Sub

    Private Sub RDTmenu_NodeDrop(sender As Object, e As Telerik.Web.UI.RadTreeNodeDragDropEventArgs) Handles RDTmenu.NodeDrop
        Dim startItem As dtoItem = GetDtoFromNode(e.SourceDragNode)
        Dim endItem As dtoItem = GetDtoFromNode(e.DestDragNode)

        If Not (e.SourceDragNode Is e.DestDragNode) Then
            Dim int As Integer = 0
            Dim isCont As Boolean = isContainer(startItem, endItem, e.SourceDragNode, e.DestDragNode)
            If (e.SourceDragNode.ParentNode Is e.DestDragNode.ParentNode) AndAlso Not isCont Then
                RaiseEvent ItemReorderedTo(startItem, endItem)
                'int = 0 ' stesso padre
            ElseIf e.SourceDragNode.ParentNode Is e.DestDragNode Then
                RaiseEvent ItemToFirstDisplay(startItem)
            Else
                'int = 2 ' spostato
                RaiseEvent ItemMovedTo(startItem, endItem)
            End If
        End If
    End Sub

    Private Function isContainer(startItem As dtoItem, endItem As dtoItem, ByVal sNode As Telerik.Web.UI.RadTreeNode, ByVal eNode As Telerik.Web.UI.RadTreeNode) As Boolean
        Dim result As Boolean = False
        If (startItem.Type = MenuItemType.Link OrElse startItem.Type = MenuItemType.Separator OrElse startItem.Type = MenuItemType.Text) AndAlso (endItem.Type = MenuItemType.TextContainer OrElse endItem.Type = MenuItemType.LinkContainer) AndAlso eNode.Nodes.Count = 0 Then
            result = True
        End If

        Return result
    End Function
    Private Function GetDtoFromNode(ByVal node As Telerik.Web.UI.RadTreeNode) As dtoItem
        Dim result As New dtoItem() With {.Id = node.Value, .Type = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of MenuItemType).GetByString(node.Category, MenuItemType.None)}
        Return result
    End Function

    Public Sub ChangeTreeItemName(item As dtoItem, name As String) Implements IViewMenubarTree.ChangeTreeItemName
        Dim node As Telerik.Web.UI.RadTreeNode = (From n In Me.RDTmenu.GetAllNodes Where (n.Category = item.Type.ToString()) AndAlso (n.Value = item.Id) Select n).FirstOrDefault

        If Not IsNothing(node) Then
            node.Text = name
            node.ToolTip = String.Format(Me.Resource.getValue("MenuItemType." & item.Type.ToString), name)
        End If
    End Sub
End Class