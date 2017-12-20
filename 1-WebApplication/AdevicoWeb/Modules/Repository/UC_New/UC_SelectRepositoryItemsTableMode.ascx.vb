Imports lm.Comol.Core.FileRepository.Domain
Imports lm.Comol.Core.BaseModules.FileRepository.Presentation
Imports lm.Comol.Core.DomainModel

Public Class UC_SelectRepositoryItemsTableMode
    Inherits FRitemsSelectorControl
    Implements IViewItemsSelectorTableMode

#Region "Context"
    Private _Presenter As ItemsSelectorTableModePresenter
    Public ReadOnly Property CurrentPresenter() As ItemsSelectorTableModePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ItemsSelectorTableModePresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private Property CurrentAscending As Boolean Implements IViewItemsSelectorTableMode.CurrentAscending
        Get
            Return ViewStateOrDefault("CurrentAscending", True)
        End Get
        Set(value As Boolean)
            ViewState("CurrentAscending") = value
        End Set
    End Property

    Private Property CurrentOrderBy As OrderBy Implements IViewItemsSelectorTableMode.CurrentOrderBy
        Get
            Return ViewStateOrDefault("CurrentOrderBy", OrderBy.name)
        End Get
        Set(value As OrderBy)
            ViewState("CurrentOrderBy") = value
        End Set
    End Property
   
    Public Property AllowPaging As Boolean Implements IViewItemsSelectorTableMode.AllowPaging
        Get
            Return ViewStateOrDefault("AllowPaging", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowPaging") = value
        End Set
    End Property
    Private Property Pager As PagerBase Implements IViewItemsSelectorTableMode.Pager
        Get
            Return ViewStateOrDefault("Pager", New lm.Comol.Core.DomainModel.PagerBase With {.PageSize = DefaultPageSize})
        End Get
        Set(value As PagerBase)
            Dim displayPaging As Boolean = AllowPaging
            Me.ViewState("Pager") = value
            PGgridBottom.Pager = value
            PGgridBottom.Visible = displayPaging AndAlso (Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize))

            DVpageSizeBottom.Visible = displayPaging AndAlso AllowSelectPageSize AndAlso SelectPageSizeOnBottom
            DVpageSizeTop.Visible = displayPaging AndAlso AllowSelectPageSize AndAlso Not SelectPageSizeOnBottom
            DVpagerTop.Visible = displayPaging AndAlso AllowSelectPageSize AndAlso Not SelectPageSizeOnBottom

            If AllowSelectPageSize AndAlso Not PageSizeSet Then
                DefaultPageSize = 50
            End If
            '  Me.DIVpageSize.Visible = (Not value.Count < Me.DefaultPageSize)
        End Set
    End Property
    Public Property DefaultPageSize As Integer Implements IViewItemsSelectorTableMode.DefaultPageSize
        Get
            Return ViewStateOrDefault("DefaultPageSize", 50)
        End Get
        Set(value As Integer)
            ViewState("DefaultPageSize") = value
            If Not PageSizeSet Then
                InitializePageSizeSelector(DDLpageSizeTop, value)
                InitializePageSizeSelector(DDLpageSizeBottom, value)
                PageSizeSet = True
            End If
        End Set
    End Property
    Public ReadOnly Property CurrentPageSize As Integer Implements IViewItemsSelectorTableMode.CurrentPageSize
        Get
            Dim size As Integer = DefaultPageSize
            Select Case SelectPageSizeOnBottom
                Case True
                    If DDLpageSizeBottom.SelectedIndex > -1 Then
                        size = CInt(DDLpageSizeBottom.SelectedValue)
                    End If
                Case Else
                    If DDLpageSizeTop.SelectedIndex > -1 Then
                        size = CInt(DDLpageSizeTop.SelectedValue)
                    End If
            End Select
            Return size
        End Get
    End Property

#End Region

#Region "Internal"
    Public Property SelectionMode As ListSelectionMode
        Get
            Return ViewStateOrDefault("SelectionMode", ListSelectionMode.Multiple)
        End Get
        Set(value As ListSelectionMode)
            ViewState("SelectionMode") = value
        End Set
    End Property
    Public Property SelectPageSizeOnTop As Boolean
        Get
            Return ViewStateOrDefault("SelectPageSizeOnTop", False)
        End Get
        Set(value As Boolean)
            ViewState("SelectPageSizeOnTop") = value
        End Set
    End Property
    Public Property SelectPageSizeOnBottom As Boolean
        Get
            Return ViewStateOrDefault("SelectPageSizeOnBottom", True)
        End Get
        Set(value As Boolean)
            ViewState("SelectPageSizeOnBottom") = value
        End Set
    End Property
    Public Property AllowSelectPageSize As Boolean
        Get
            Return ViewStateOrDefault("AllowSelectPageSize", True)
        End Get
        Set(value As Boolean)
            ViewState("AllowSelectPageSize") = value
        End Set
    End Property
    Private Property PageSizeSet As Boolean
        Get
            Return ViewStateOrDefault("PageSizeSet", False)
        End Get
        Set(value As Boolean)
            ViewState("PageSizeSet") = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        PGgridBottom.Pager = Pager
    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLabel(LBrecordsForPageTop_t)
            .setLabel(LBrecordsForPage_t)
        End With
    End Sub
    Protected Friend Overrides Sub InternalInitialize(idUser As Integer, identifier As RepositoryIdentifier, adminMode As Boolean, showHiddenItems As Boolean, disableNotAvailableItems As Boolean, typesToLoad As List(Of ItemType), availability As ItemAvailability, displayStatistics As List(Of StatisticType), idRemovedItems As List(Of Long), idSelectedItems As List(Of Long), orderBy As OrderBy, ascending As Boolean)
        CurrentPresenter.InitView(AllowPaging, False, CurrentPageSize, identifier, adminMode, showHiddenItems, disableNotAvailableItems, typesToLoad, availability, displayStatistics, idRemovedItems, idSelectedItems, orderBy, ascending)
    End Sub

    Protected Friend Overrides Sub SetSessionTimeout()
        'disable all !!
        For Each row As RepeaterItem In (From r As RepeaterItem In RPTrepositoryItems.Items Where r.ItemType = ListItemType.Item OrElse r.ItemType = ListItemType.AlternatingItem Select r)
            Dim oCheck As HtmlInputCheckBox = row.FindControl("CBXrepositoryItem")
            Dim oRadio As HtmlInputRadioButton = row.FindControl("RBselect")
            oCheck.Disabled = True
            oCheck.Checked = False
            oRadio.Disabled = True
            oRadio.Checked = False
        Next
    End Sub
    Protected Friend Overrides Function GetCurrentItemsSelection() As Dictionary(Of Boolean, List(Of Long))
        Dim items As New Dictionary(Of Boolean, List(Of Long))
        items.Add(True, New List(Of Long))
        items.Add(False, New List(Of Long))
        Dim multipleSelection As Boolean = (SelectionMode = ListSelectionMode.Multiple)
        For Each row As RepeaterItem In (From r As RepeaterItem In RPTrepositoryItems.Items Where r.Visible AndAlso (r.ItemType = ListItemType.Item OrElse r.ItemType = ListItemType.AlternatingItem) Select r)
            Dim oLiteral As Literal = row.FindControl("LTidItem")
            Dim idItem As Long = 0
            Long.TryParse(oLiteral.Text, idItem)
            If multipleSelection Then
                Dim oCheck As HtmlInputCheckBox = row.FindControl("CBXrepositoryItem")
                If Not oCheck.Disabled Then
                    items(oCheck.Checked).Add(idItem)
                End If
            Else
                Dim oRadio As HtmlInputRadioButton = row.FindControl("RBselect")
                If Not oRadio.Disabled Then
                    items(oRadio.Checked).Add(idItem)
                End If
            End If
        Next
        Return items
    End Function

    Protected Friend Overrides Function HasAvailableItemsToSelect() As Boolean
        Dim hasItems As Boolean = False
        Dim multipleSelection As Boolean = (SelectionMode = ListSelectionMode.Multiple)
        For Each row As RepeaterItem In (From r As RepeaterItem In RPTrepositoryItems.Items Where r.ItemType = ListItemType.Item OrElse r.ItemType = ListItemType.AlternatingItem Select r)
            Dim oCheck As HtmlInputCheckBox = row.FindControl("CBXrepositoryItem")
            Dim oRadio As HtmlInputRadioButton = row.FindControl("RBselect")
            If multipleSelection AndAlso Not oCheck.Disabled Then
                Return True
            ElseIf Not multipleSelection AndAlso Not oRadio.Disabled Then
                Return True
            End If
        Next
        Return hasItems
    End Function
#End Region

#Region "Implements"
    Private _itemsCount As Long = 0
    Private _FirstLoad As Boolean = False
    Public Sub InitializeControlByModule(idUser As Integer, moduleCode As String, itemsToLoad As List(Of dtoRepositoryItemToSelect), typesToLoad As List(Of ItemType), availability As ItemAvailability) Implements IViewItemsSelectorTableMode.InitializeControlByModule

    End Sub

    Private Sub LoadItems(items As List(Of dtoRepositoryItemToSelect)) Implements IViewItemsSelectorTableMode.LoadItems
        If IsNothing(items) Then
            items = New List(Of dtoRepositoryItemToSelect)
            _itemsCount = 0
        Else
            _itemsCount = items.Count
        End If
        _FirstLoad = True

        RPTrepositoryItems.DataSource = items
        RPTrepositoryItems.DataBind()
    End Sub
#End Region

#Region "Internal"
    Public Sub SelectAll()
        If (SelectionMode = ListSelectionMode.Multiple) Then
            AllSelected = True
            For Each row As RepeaterItem In (From r As RepeaterItem In RPTrepositoryItems.Items Where r.ItemType = ListItemType.Item OrElse r.ItemType = ListItemType.AlternatingItem Select r)
                Dim oCheck As HtmlInputCheckBox = row.FindControl("CBXrepositoryItem")
                If Not oCheck.Disabled Then
                    oCheck.Checked = True
                End If
            Next
        End If
    End Sub
    Public Function GetAllItems() As List(Of dtoRepositoryItemToSelect)
        If LoadForModule Then
            Return New List(Of dtoRepositoryItemToSelect)
        Else
            Return CurrentPresenter.GetSelectedItems(RepositoryIdentifier, CurrentAdminMode, DisplayStatistics, AvailableTypes, IdSelectedItems, True)
        End If
    End Function
    Protected Friend Overrides Function GetInternalSelectedItems() As List(Of dtoRepositoryItemToSelect)
        If LoadForModule Then
            Return New List(Of dtoRepositoryItemToSelect)
        Else
            Return CurrentPresenter.GetSelectedItems(RepositoryIdentifier, CurrentAdminMode, DisplayStatistics, AvailableTypes, IdSelectedItems, AllSelected)
        End If
    End Function
    Private Sub InitializePageSizeSelector(dropdwon As DropDownList, value As Integer)
        Dim items As List(Of Int32) = (From i As ListItem In dropdwon.Items Select CInt(i.Value)).ToList()
        If items.Any() AndAlso items.Contains(value) Then
            dropdwon.SelectedValue = value.ToString
        Else
            If Not items.Any() Then
                items = (From e As Integer In Enumerable.Range(1, 6) Select e * 25).ToList()
            End If
            If items.Any() Then
                items.Add(value)
            End If
            dropdwon.DataSource = items.Distinct().OrderBy(Function(i) i).ToList
            dropdwon.DataBind()
            dropdwon.SelectedValue = value.ToString
        End If
    End Sub
    Private Sub RPTrepositoryItems_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTrepositoryItems.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            Dim oLabel As Label = e.Item.FindControl("LBitemNameHeader_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBitemStatisticsHeader_t")
            If DisplayStatistics.Count = 1 Then
                oLabel.Text = Resource.getValue("Header.StatisticType." & DisplayStatistics.FirstOrDefault().ToString)
            Else
                Resource.setLabel(oLabel)
                If DisplayStatistics.Count > 1 Then
                    oLabel.ToolTip = oLabel.Text & String.Join(" / ", DisplayStatistics.Select(Function(t) Resource.getValue("Header.StatisticType." & t.ToString)).ToArray())
                End If
            End If
            Resource.setLabel(oLabel)

            Dim oTableCell As HtmlTableCell = e.Item.FindControl("THmultiSelect")
            If SelectionMode = ListSelectionMode.Multiple Then
                oTableCell.Visible = True
                If AllowSelectAll Then
                    Dim oLinkbutton As LinkButton = e.Item.FindControl("LNBselectItemsIntoAllPages")
                    Resource.setLinkButton(oLinkbutton, True, True)

                    oLinkbutton = e.Item.FindControl("LNBunselectItemsIntoAllPages")
                    Resource.setLinkButton(oLinkbutton, True, True)

                    Dim oControl As HtmlControl = e.Item.FindControl("DVselectorAll")
                    oControl.Visible = (Pager.Count > CurrentPageSize)

                    oControl = e.Item.FindControl("DVselectorNone")
                    oControl.Visible = (Pager.Count > CurrentPageSize)
                Else
                    DirectCast(e.Item.FindControl("SPNtopSelector"), HtmlGenericControl).Visible = False
                End If

                oTableCell = e.Item.FindControl("THsingleSelect")
                oTableCell.Visible = False
            Else
                oTableCell.Visible = False
                oTableCell = e.Item.FindControl("THsingleSelect")
                oTableCell.Visible = True
            End If

        ElseIf e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim item As dtoRepositoryItemToSelect = DirectCast(e.Item.DataItem, dtoRepositoryItemToSelect)
            Dim oCheck As HtmlInputCheckBox = e.Item.FindControl("CBXrepositoryItem")
            Dim oRadio As RadioButton = e.Item.FindControl("RBselect")

            Dim oLiteral As Literal = e.Item.FindControl("LTfileName")
            oLiteral.Text = GetFilenameRender(item.DisplayName, item.Extension, item.Type)
            Dim oControl As HtmlGenericControl = e.Item.FindControl("DVpath")
            oLiteral = e.Item.FindControl("LTitemPath")
            oLiteral.Text = item.Path
            oControl.Visible = Not String.IsNullOrWhiteSpace(item.Path)

            Dim oLabel As Label = e.Item.FindControl("LBitemStatistics")
            oLabel.Text = ""
            If DisplayStatistics.Contains(StatisticType.downloads) Then
                oLabel.Text = item.Downloads
            End If
            If DisplayStatistics.Contains(StatisticType.mydownloads) Then
                oLabel.Text = IIf(Not String.IsNullOrWhiteSpace(oLabel.Text), " / ", "") & item.MyDownloads
            End If
            If DisplayStatistics.Contains(StatisticType.plays) Then
                oLabel.Text = IIf(Not String.IsNullOrWhiteSpace(oLabel.Text), " / ", "") & item.Plays
            End If
            If DisplayStatistics.Contains(StatisticType.myplays) Then
                oLabel.Text = IIf(Not String.IsNullOrWhiteSpace(oLabel.Text), " / ", "") & item.MyPlays
            End If

            Dim oTableCell As HtmlTableCell = e.Item.FindControl("TDmultiSelect")

            Dim multiSelect As Boolean = (SelectionMode = ListSelectionMode.Multiple)
            oRadio.Visible = Not multiSelect AndAlso item.Selectable
            oCheck.Visible = multiSelect AndAlso item.Selectable
            oCheck.Disabled = Not item.Selectable
            oRadio.Enabled = item.Selectable
            If multiSelect Then
                oCheck.Checked = (_FirstLoad AndAlso item.Selectable AndAlso item.Selected) OrElse IdSelectedItems.Contains(item.Id)
            ElseIf IdSelectedItems.Count = 1 Then
                oRadio.Checked = IdSelectedItems.Contains(item.Id)
            End If
        ElseIf e.Item.ItemType = ListItemType.Footer Then
            Dim oTableItem As HtmlControl = e.Item.FindControl("TRempty")
            oTableItem.Visible = (RPTrepositoryItems.Items.Count = 0)
            If (RPTrepositoryItems.Items.Count = 0) Then
                Dim oLabel As Label = e.Item.FindControl("LBrepositoryItemsEmpty")

                oLabel.Text = Resource.getValue("LBrepositoryItemsEmpty")
            End If
        End If
    End Sub
    Private Sub RPTrepositoryItemss_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTrepositoryItems.ItemCommand
        Select Case e.CommandName
            Case "all"
                CurrentPresenter.EditItemsSelection(True)
            Case "none"
                CurrentPresenter.EditItemsSelection(False)
        End Select
    End Sub
    Private Sub PGgridBottom_OnPageSelected() Handles PGgridBottom.OnPageSelected
        CurrentPresenter.LoadItems(RepositoryIdentifier, CurrentAdminMode, CurrentShowHiddenItems, CurrentDisableNotAvailableItems, AvailableTypes, CurrentAvailability, DisplayStatistics, IdRemovedItems, IdSelectedItems, AllSelected, CurrentOrderBy, CurrentAscending, PGgridBottom.Pager.PageIndex, CurrentPageSize)
    End Sub
    Private Sub DDLpageSize_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DDLpageSizeBottom.SelectedIndexChanged, DDLpageSizeTop.SelectedIndexChanged
        CurrentPresenter.LoadItems(RepositoryIdentifier, CurrentAdminMode, CurrentShowHiddenItems, CurrentDisableNotAvailableItems, AvailableTypes, CurrentAvailability, DisplayStatistics, IdRemovedItems, IdSelectedItems, AllSelected, CurrentOrderBy, CurrentAscending, PGgridBottom.Pager.PageIndex, CurrentPageSize)
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
#End Region

    
End Class