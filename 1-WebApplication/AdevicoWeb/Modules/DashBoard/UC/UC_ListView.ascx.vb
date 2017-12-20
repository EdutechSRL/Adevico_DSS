Imports lm.Comol.Core.Dashboard.Domain
Imports lm.Comol.Core.BaseModules.Dashboard.Presentation
Imports lm.ActionDataContract

Public Class UC_ListView
    Inherits DBbaseControl
    Implements IViewCommunitiesList

#Region "Context"
    Private _Presenter As CommunitiesListPresenter
    Private ReadOnly Property CurrentPresenter() As CommunitiesListPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New CommunitiesListPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property IsInitialized As Boolean Implements IViewCommunitiesList.IsInitialized
        Get
            Return ViewStateOrDefault("IsInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("IsInitialized") = value
        End Set
    End Property
    Public Property UseDefaultStartupItems As Boolean Implements IViewCommunitiesList.UseDefaultStartupItems
        Get
            Return ViewStateOrDefault("UseDefaultStartupItems", False)
        End Get
        Set(value As Boolean)
            ViewState("UseDefaultStartupItems") = value
        End Set
    End Property
    Public Property DefaultStartupItems As Integer Implements IViewCommunitiesList.DefaultStartupItems
        Get
            Return ViewStateOrDefault("DefaultStartupItems", 5)
        End Get
        Set(value As Integer)
            ViewState("DefaultStartupItems") = value
        End Set
    End Property
    Public Property IsCollapsable As Boolean Implements IViewCommunitiesList.IsCollapsable
        Get
            Return ViewStateOrDefault("IsCollapsable", False)
        End Get
        Set(value As Boolean)
            ViewState("IsCollapsable") = value
            SPNexpand.Visible = value
        End Set
    End Property
    Public Property IsCollapsed As Boolean Implements IViewCommunitiesList.IsCollapsed
        Get
            Return ViewStateOrDefault("IsCollapsed", False)
        End Get
        Set(value As Boolean)
            ViewState("IsCollapsed") = value
        End Set
    End Property
    Public Property DataIdForCollapse As String Implements IViewCommunitiesList.DataIdForCollapse
        Get
            Return ViewStateOrDefault("DataIdForCollapse", "")
        End Get
        Set(value As String)
            ViewState("DataIdForCollapse") = value
        End Set
    End Property
    Public Property TitleCssClass As String Implements IViewCommunitiesList.TitleCssClass
        Get
            Return ViewStateOrDefault("TitleCssClass", "")
        End Get
        Set(value As String)
            ViewState("TitleCssClass") = value
        End Set
    End Property
    Public Property TitleImage As String Implements IViewCommunitiesList.TitleImage
        Get
            Return ViewStateOrDefault("TitleImage", "")
        End Get
        Set(value As String)
            ViewState("TitleImage") = value
            If Not String.IsNullOrEmpty(value) Then
                IMGtileIcon.Visible = True
                IMGtileIcon.ImageUrl = PageUtility.ApplicationUrlBase & TilesVirtualPath & value
            Else
                IMGtileIcon.Visible = False
            End If
        End Set
    End Property
    Public Property AutoDisplayTitle As String Implements IViewCommunitiesList.AutoDisplayTitle
        Get
            Return ViewStateOrDefault("AutoDisplayTitle", "")
        End Get
        Set(value As String)
            ViewState("AutoDisplayTitle") = value
            LTtitle.Text = value
        End Set
    End Property
    Public Property DisplayLessCommand As Boolean Implements IViewCommunitiesList.DisplayLessCommand
        Get
            If Me.Visible Then
                Return LNBviewLess.Visible
            Else
                Return ViewStateOrDefault("DisplayLessCommand", False)
            End If
        End Get
        Set(value As Boolean)
            ViewState("DisplayLessCommand") = value
            LNBviewLess.Visible = value
        End Set
    End Property
    Public Property DisplayMoreCommand As Boolean Implements IViewCommunitiesList.DisplayMoreCommand
        Get
            If Me.Visible Then
                Return LNBviewAll.Visible
            Else
                Return ViewStateOrDefault("DisplayMoreCommand", False)
            End If
        End Get
        Set(value As Boolean)
            ViewState("DisplayMoreCommand") = value
            LNBviewAll.Visible = value
        End Set
    End Property
    Private Property CurrentDisplayOrder As lm.Comol.Core.DomainModel.ItemDisplayOrder Implements IViewCommunitiesList.CurrentDisplayOrder
        Get
            Return ViewStateOrDefault("CurrentDisplayOrder", lm.Comol.Core.DomainModel.ItemDisplayOrder.item)
        End Get
        Set(value As lm.Comol.Core.DomainModel.ItemDisplayOrder)
            ViewState("CurrentDisplayOrder") = value
        End Set
    End Property
    Private Property CurrentAscending As Boolean Implements IViewCommunitiesList.CurrentAscending
        Get
            Return ViewStateOrDefault("CurrentAscending", False)
        End Get
        Set(value As Boolean)
            ViewState("CurrentAscending") = value
        End Set
    End Property
    Public Property CurrentOrderBy As OrderItemsBy Implements IViewCommunitiesList.CurrentOrderBy
        Get
            Return ViewStateOrDefault("CurrentOrderBy", OrderItemsBy.LastAccess)
        End Get
        Set(value As OrderItemsBy)
            ViewState("CurrentOrderBy") = value
            Select Case value
                Case OrderItemsBy.Name
                    LBthgenericDateTime.Text = Resource.getValue("LBthgenericDateTime.OrderItemsBy." & OrderItemsBy.LastAccess.ToString)
                Case Else
                    LBthgenericDateTime.Text = Resource.getValue("LBthgenericDateTime.OrderItemsBy." & value.ToString)
            End Select
        End Set
    End Property
    Public Property IsForSearch As Boolean Implements IViewCommunitiesList.IsForSearch
        Get
            Return ViewStateOrDefault("IsForSearch", False)
        End Get
        Set(value As Boolean)
            ViewState("IsForSearch") = value
        End Set
    End Property
    Private Property AvailableColumns As List(Of lm.Comol.Core.BaseModules.Dashboard.Domain.searchColumn) Implements IViewCommunitiesList.AvailableColumns
        Get
            Return ViewStateOrDefault("AvailableColumns", New List(Of lm.Comol.Core.BaseModules.Dashboard.Domain.searchColumn))
        End Get
        Set(value As List(Of lm.Comol.Core.BaseModules.Dashboard.Domain.searchColumn))
            ViewState("AvailableColumns") = value

            THgenericDateTime.Visible = value.Contains(lm.Comol.Core.BaseModules.Dashboard.Domain.searchColumn.genericdate)
            THsubscriptionInfo.Visible = value.Contains(lm.Comol.Core.BaseModules.Dashboard.Domain.searchColumn.subscriptioninfo)
            THactions.Visible = value.Contains(lm.Comol.Core.BaseModules.Dashboard.Domain.searchColumn.actions)
        End Set
    End Property
    Private Property Pager As lm.Comol.Core.DomainModel.PagerBase Implements IViewCommunitiesList.Pager
        Get
            Return ViewStateOrDefault("Pager", New lm.Comol.Core.DomainModel.PagerBase)
        End Get
        Set(ByVal value As lm.Comol.Core.DomainModel.PagerBase)
            Me.ViewState("Pager") = value
            Me.PGgridBottom.Pager = value
            Me.DVpager.Visible = (Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize))
        End Set
    End Property
    Private Property CurrentPageSize As Integer Implements IViewCommunitiesList.CurrentPageSize
        Get
            Return ViewStateOrDefault("CurrentPageSize", 10)
        End Get
        Set(value As Integer)
            ViewState("CurrentPageSize") = value
        End Set
    End Property
    Private ReadOnly Property CurrentPageIndex As Integer Implements IViewCommunitiesList.CurrentPageIndex
        Get
            Return Pager.PageIndex
        End Get
    End Property
    Private Property IdCurrentCommunityType As Integer Implements IViewCommunitiesList.IdCurrentCommunityType
        Get
            Return ViewStateOrDefault("IdCurrentCommunityType", -1)
        End Get
        Set(value As Integer)
            ViewState("IdCurrentCommunityType") = value
        End Set
    End Property
    Private Property IdCurrentRemoveCommunityType As Integer Implements IViewCommunitiesList.IdCurrentRemoveCommunityType
        Get
            Return ViewStateOrDefault("IdCurrentRemoveCommunityType", -1)
        End Get
        Set(value As Integer)
            ViewState("IdCurrentRemoveCommunityType") = value
        End Set
    End Property
    Private Property IdCurrentTag As Long Implements IViewCommunitiesList.IdCurrentTag
        Get
            Return ViewStateOrDefault("IdCurrentTag", -1)
        End Get
        Set(value As Long)
            ViewState("IdCurrentTag") = value
        End Set
    End Property
    Private Property IdCurrentTileTags As List(Of Long) Implements IViewCommunitiesList.IdCurrentTileTags
        Get
            Return ViewStateOrDefault("IdCurrentTileTags", New List(Of Long))
        End Get
        Set(value As List(Of Long))
            ViewState("IdCurrentTileTags") = value
        End Set
    End Property
    Private Property IdCurrentTile As Long Implements IViewCommunitiesList.IdCurrentTile
        Get
            Return ViewStateOrDefault("IdCurrentTile", -1)
        End Get
        Set(value As Long)
            ViewState("IdCurrentTile") = value
        End Set
    End Property
    Private Property DefaultPageSize As Int32 Implements IViewCommunitiesList.DefaultPageSize
        Get
            Return ViewStateOrDefault("DefaultPageSize", 15)
        End Get
        Set(value As Int32)
            ViewState("DefaultPageSize") = value
        End Set
    End Property
    Private Property PageType As DashboardViewType Implements IViewCommunitiesList.PageType
        Get
            Return ViewStateOrDefault("PageType", DashboardViewType.List)
        End Get
        Set(value As DashboardViewType)
            ViewState("PageType") = value
        End Set
    End Property
    Private Property CurrentFilters As lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters Implements IViewCommunitiesList.CurrentFilters
        Get
            Return ViewStateOrDefault("CurrentFilters", New lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters())
        End Get
        Set(value As lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters)
            ViewState("CurrentFilters") = value
        End Set
    End Property
#End Region

#Region "Internal"
    Private _noItemsToDisplay As Boolean
    Public Event SessionTimeout()
    Public Event HideDisplayMessage()
    Public Event DisplayMessage(message As String, type As lm.Comol.Core.DomainModel.Helpers.MessageType)
    Public Event OpenConfirmDialog(ByVal openCssClass As String)

    Public Property AllowAutoUpdateCookie As Boolean
        Get
            Return ViewStateOrDefault("AllowAutoUpdateCookie", True)
        End Get
        Set(value As Boolean)
            ViewState("AllowAutoUpdateCookie") = value
        End Set
    End Property
    Public ReadOnly Property ItemsCount As Integer
        Get
            Return (From row As RepeaterItem In RPTcommunities.Items Where row.ItemType = ListItemType.Item OrElse row.ItemType = ListItemType.AlternatingItem Select row).Count
        End Get
    End Property
    Private ReadOnly Property TilesVirtualPath As String
        Get
            Return SystemSettings.File.Tiles.VirtualPath
        End Get
    End Property
    Public Property HasDisplayMessage As Boolean
        Get
            Return ViewStateOrDefault("HasDisplayMessage", False)
        End Get
        Set(value As Boolean)
            ViewState("HasDisplayMessage") = value
        End Set
    End Property
    Public Property IsPreview As Boolean
        Get
            Return ViewStateOrDefault("IsPreview", False)
        End Get
        Set(value As Boolean)
            ViewState("IsPreview") = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PGgridBottom.Pager = Me.Pager
    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLabel(LBspanExpandList)
            .setLabel(LBspanCollapseList)
            .setLabel(LBorderBySelectorDescription)
            .setLabel(LBorderBySelected)

            .setLabel(LBthcommunityName)
            .setLiteral(LTthcommunityInfo)
            .setLinkButton(LNBviewAll, False, True)
            .setLinkButton(LNBviewLess, False, True)
        End With
    End Sub
#End Region

#Region "Implements"
   
    Public Sub InitalizeControl(pageSettings As litePageSettings, userSettings As UserCurrentSettings, items As List(Of dtoItemFilter(Of OrderItemsBy)), Optional display As lm.Comol.Core.DomainModel.ItemDisplayOrder = 3, Optional idRemoveCommunityType As Int32 = -1) Implements IViewCommunitiesList.InitalizeControl
        BaseInitalizeControl(items, display)
        CurrentPresenter.InitView(pageSettings, userSettings, items, -1, idRemoveCommunityType)
    End Sub
    Public Sub InitalizeControlForCommunityType(pageSettings As litePageSettings, userSettings As UserCurrentSettings, items As List(Of dtoItemFilter(Of OrderItemsBy)), idCommunityType As Int32, Optional display As lm.Comol.Core.DomainModel.ItemDisplayOrder = 3) Implements IViewCommunitiesList.InitalizeControlForCommunityType
        BaseInitalizeControl(items, display)
        CurrentPresenter.InitView(pageSettings, userSettings, items, idCommunityType, -1, -1, -1, Nothing)
    End Sub
    Public Sub InitalizeControlForTag(pageSettings As litePageSettings, userSettings As UserCurrentSettings, items As List(Of dtoItemFilter(Of OrderItemsBy)), idTag As Long, Optional display As lm.Comol.Core.DomainModel.ItemDisplayOrder = 3) Implements IViewCommunitiesList.InitalizeControlForTag
        BaseInitalizeControl(items, display)
        CurrentPresenter.InitView(pageSettings, userSettings, items, -1, -1, -1, idTag)
    End Sub
    Public Sub InitalizeControlForTile(pageSettings As litePageSettings, userSettings As UserCurrentSettings, items As List(Of dtoItemFilter(Of OrderItemsBy)), idTile As Long, Optional display As lm.Comol.Core.DomainModel.ItemDisplayOrder = 3) Implements IViewCommunitiesList.InitalizeControlForTile
        BaseInitalizeControl(items, display)
        CurrentPresenter.InitView(pageSettings, userSettings, items, -1, -1, idTile)
    End Sub
    Public Sub InitalizeControlForTile(pageSettings As litePageSettings, userSettings As UserCurrentSettings, items As List(Of dtoItemFilter(Of OrderItemsBy)), tile As dtoTileDisplay, Optional display As lm.Comol.Core.DomainModel.ItemDisplayOrder = 3) Implements IViewCommunitiesList.InitalizeControlForTile
        BaseInitalizeControl(items, display)
        CurrentPresenter.InitViewByTile(pageSettings, userSettings, items, tile)
    End Sub
    Public Sub InitalizeControl(pageSettings As litePageSettings, filters As lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters, orderByItems As List(Of dtoItemFilter(Of OrderItemsBy)), tile As liteTile, Optional display As lm.Comol.Core.DomainModel.ItemDisplayOrder = 3) Implements IViewCommunitiesList.InitalizeControl
        BaseInitalizeControl(orderByItems, display)
        CurrentPresenter.InitView(pageSettings, filters, orderByItems)
    End Sub
    Public Sub ApplyFilters(pageSettings As litePageSettings, filters As lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters) Implements IViewCommunitiesList.ApplyFilters
        CurrentPresenter.ApplyFilters(pageSettings, filters, CurrentOrderBy, CurrentAscending)
    End Sub

    Private Sub BaseInitalizeControl(items As List(Of dtoItemFilter(Of OrderItemsBy)), display As lm.Comol.Core.DomainModel.ItemDisplayOrder)
        InitializeOrderBySelector(items)
        CurrentDisplayOrder = display
    End Sub

    Private Sub DisplaySessionTimeout() Implements IViewCommunitiesList.DisplaySessionTimeout
        HideItems()
        RaiseEvent SessionTimeout()
    End Sub

    Private Sub InitializeOrderBySelector(items As List(Of dtoItemFilter(Of OrderItemsBy))) Implements IViewCommunitiesList.InitializeOrderBySelector
        If Not IsNothing(items) AndAlso items.Count > 1 Then
            DVorderBySelector.Visible = True
            RPTorderBy.DataSource = items
            RPTorderBy.DataBind()
            LBorderBySelected.Text = Resource.getValue("OrderItemsBy." & items.Where(Function(i) i.Selected).FirstOrDefault.Value.ToString)
        Else
            DVorderBySelector.Visible = False
        End If
    End Sub
    Private Sub LoadItems(items As List(Of dtoSubscriptionItem), orderby As OrderItemsBy, ascending As Boolean) Implements IViewCommunitiesList.LoadItems
        RPTcommunities.DataSource = items
        RPTcommunities.DataBind()
        LNBorderByNameUp.Visible = False
        LNBorderByNameDown.Visible = False
        LNBorderByDown.Visible = False
        LNBorderByUp.Visible = False
        Select Case orderby
            Case OrderItemsBy.Name
                LNBorderByNameUp.Visible = (items.Count > 1) AndAlso Not CurrentAscending
                LNBorderByNameDown.Visible = (items.Count > 1) AndAlso CurrentAscending
                LNBorderByNameUp.ToolTip = Resource.getValue("OrderItemsBy." & orderby.ToString & ".True")
                LNBorderByNameDown.ToolTip = Resource.getValue("OrderItemsBy." & orderby.ToString & ".False")
            Case Else
                LNBorderByDown.Visible = (items.Count > 1) AndAlso CurrentAscending
                LNBorderByUp.Visible = (items.Count > 1) AndAlso Not CurrentAscending
                LNBorderByUp.ToolTip = Resource.getValue("OrderItemsBy." & orderby.ToString & ".True")
                LNBorderByDown.ToolTip = Resource.getValue("OrderItemsBy." & orderby.ToString & ".False")
        End Select
    End Sub

    Private Function GetIdcommunitiesWithNews(idCommunities As List(Of Integer), iduser As Integer) As List(Of Integer) Implements IViewCommunitiesList.GetIdcommunitiesWithNews
        If idCommunities.Any Then
            Dim service As New lm.Modules.NotificationSystem.Business.ManagerCommunitynews(PageUtility.CurrentContext)

            Return (From r As lm.Modules.NotificationSystem.Presentation.dtoCommunityNewsCount In service.GetCommunityNewsCount(iduser)
                         Where idCommunities.Contains(r.ID) Select r.ID).ToList()

        Else
            Return New List(Of Integer)
        End If

    End Function

    Private Sub UpdateUserSettings(settings As UserCurrentSettings) Implements IViewCommunitiesList.UpdateUserSettings
        If AllowAutoUpdateCookie Then
            SaveCurrentCookie(settings)
        End If
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As ModuleDashboard.ActionType) Implements IViewCommunitiesList.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, Nothing, InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idActionCommunity As Integer, action As ModuleDashboard.ActionType) Implements IViewCommunitiesList.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleDashboard.ObjectType.Community, idActionCommunity), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub DisplayConfirmMessage(idCommunity As Integer, path As String, community As lm.Comol.Core.BaseModules.CommunityManagement.dtoUnsubscribeTreeNode, actions As List(Of RemoveAction), selected As RemoveAction, Optional alsoFromCommunities As List(Of lm.Comol.Core.BaseModules.CommunityManagement.dtoUnsubscribeTreeNode) = Nothing) Implements IViewCommunitiesList.DisplayConfirmMessage
        RaiseEvent OpenConfirmDialog(CTRLconfirmUnsubscription.DialogIdentifier)
        CTRLconfirmUnsubscription.Visible = True
        CTRLconfirmUnsubscription.InitializeControl(idCommunity, path, community, actions, selected, alsoFromCommunities, Resource.getValue("ConfirmUnsubscription.Description"))
    End Sub

    Private Sub DisplayUnableToUnsubscribe(community As String) Implements IViewCommunitiesList.DisplayUnableToUnsubscribe
        HasDisplayMessage = True
        RaiseEvent DisplayMessage(String.Format(Resource.getValue("DisplayUnableToUnsubscribe"), community), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
    End Sub
    Private Sub DisplayUnsubscribeNotAllowed(community As String) Implements IViewCommunitiesList.DisplayUnsubscribeNotAllowed
        HasDisplayMessage = True
        RaiseEvent DisplayMessage(String.Format(Resource.getValue("DisplayUnsubscribeNotAllowed"), community), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayUnsubscriptionMessage(unsubscribedFrom As List(Of String), unableToUnsubscribeFrom As List(Of String)) Implements IViewCommunitiesList.DisplayUnsubscriptionMessage
        Dim t As lm.Comol.Core.DomainModel.Helpers.MessageType = lm.Comol.Core.DomainModel.Helpers.MessageType.success
        Dim dMessage As String = ""
        Dim unableItems As String = LTtemplateMessageDetails.Text
        Dim removedItems As String = LTtemplateMessageDetails.Text
        HasDisplayMessage = True
        If unsubscribedFrom.Any() AndAlso unableToUnsubscribeFrom.Any() Then
            t = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
            unableItems = String.Format(unableItems, String.Join("", unableToUnsubscribeFrom.Select(Function(s) String.Format(LTtemplateMessageDetail.Text, s)).ToList()))

            removedItems = String.Format(removedItems, String.Join("", unsubscribedFrom.Select(Function(s) String.Format(LTtemplateMessageDetail.Text, s)).ToList()))

            dMessage = String.Format(Resource.getValue("DisplayUnsubscriptionMessage.alert"), removedItems, unableItems)
        ElseIf unableToUnsubscribeFrom.Any() Then
            t = lm.Comol.Core.DomainModel.Helpers.MessageType.error
            unableItems = String.Format(unableItems, String.Join("", unableToUnsubscribeFrom.Select(Function(s) String.Format(LTtemplateMessageDetail.Text, s)).ToList()))

            dMessage = String.Format(Resource.getValue("DisplayUnsubscriptionMessage.error"), unableItems)
        ElseIf unsubscribedFrom.Any() Then
            removedItems = String.Format(removedItems, String.Join("", unsubscribedFrom.Select(Function(s) String.Format(LTtemplateMessageDetail.Text, s)).ToList()))

            dMessage = String.Format(Resource.getValue("DisplayUnsubscriptionMessage.success"), removedItems)
        Else
            dMessage = Resource.getValue("DisplayUnsubscriptionMessage.None")
            t = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
        End If

        RaiseEvent DisplayMessage(dMessage, t)
    End Sub
    Private Sub DisplayUnsubscribedFrom(community As String) Implements IViewCommunitiesList.DisplayUnsubscribedFrom
        HasDisplayMessage = True
        RaiseEvent DisplayMessage(String.Format(Resource.getValue("DisplayUnsubscriptionMessage.success"), community), lm.Comol.Core.DomainModel.Helpers.MessageType.success)
    End Sub
    Private Sub DisplayErrorFromDB() Implements IViewCommunitiesList.DisplayErrorFromDB
        HideItems()
    End Sub
    Private Sub HideItems()
        LNBviewAll.Visible = False
        LNBviewLess.Visible = False
        LNBorderByDown.Visible = False
        LNBorderByNameDown.Visible = False
        LNBorderByNameUp.Visible = False
        LNBorderByUp.Visible = False
        RPTorderBy.Visible = False
        For Each row As RepeaterItem In (From r As RepeaterItem In RPTcommunities.Items)
            Dim oLinkbutton As LinkButton = row.FindControl("LNBcommunityAccess")
            oLinkbutton.Visible = False

            Dim oLabel As Label = row.FindControl("LBcommunityAccess")
            oLabel.Visible = True
        Next
    End Sub
#End Region

#Region "Internal"
    Public Sub InitalizeControl(Optional display As lm.Comol.Core.DomainModel.ItemDisplayOrder = 3)
        CurrentDisplayOrder = display
        _noItemsToDisplay = True
        RPTcommunities.DataSource = New List(Of dtoSubscriptionItem)
        RPTcommunities.DataBind()
    End Sub
    Private Sub RPTorderBy_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTorderBy.ItemDataBound
        Dim oLinkButton As LinkButton = e.Item.FindControl("LNBorderItemsBy")
        Dim oItem As dtoItemFilter(Of OrderItemsBy) = e.Item.DataItem

        oLinkButton.CommandArgument = CInt(oItem.Value)
        oLinkButton.Text = String.Format(LTorderItemsByTemplate.Text, Resource.getValue("OrderItemsBy." & oItem.Value.ToString))

        Dim oControl As HtmlControl = e.Item.FindControl("DVitemOrderBy")
        oControl.Attributes("class") = LTcssClassOrderBy.Text & " " & GetOrderByItemCssClass(oItem)
    End Sub
    Private Sub RPTorderBy_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles RPTorderBy.ItemCommand
        CurrentOrderBy = CInt(e.CommandArgument)
        Select Case CInt(e.CommandArgument)
            Case OrderItemsBy.Name
                CurrentAscending = True
            Case Else
                CurrentAscending = False
        End Select
        LBorderBySelected.Text = Resource.getValue("OrderItemsBy." & CurrentOrderBy.ToString)

        Dim oControl As HtmlControl = e.Item.FindControl("DVitemOrderBy")
        If Not oControl.Attributes("class").Contains(LTcssClassActive.Text) Then
            oControl.Attributes("class") = oControl.Attributes("class") & " " & LTcssClassActive.Text

            For Each row As RepeaterItem In (From r As RepeaterItem In RPTorderBy.Items Where r.ItemIndex <> e.Item.ItemIndex)
                oControl = row.FindControl("DVitemOrderBy")
                If oControl.Attributes("class").Contains(LTcssClassActive.Text) Then
                    oControl.Attributes("class") = Replace(oControl.Attributes("class"), LTcssClassActive.Text, "")
                    Exit For
                End If
            Next
        End If
        If IsForSearch Then
            CurrentPresenter.LoadCommunities(CurrentFilters, CurrentOrderBy, CurrentAscending, 0, CurrentPageSize)
        Else
            If DisplayMoreCommand Then
                CurrentPresenter.LoadCommunities(PageType, GetCurrentCookie, 0, CurrentPageSize, CInt(e.CommandArgument), CurrentAscending, IdCurrentCommunityType, IdCurrentRemoveCommunityType, IdCurrentTile, IdCurrentTile)
            Else
                CurrentPresenter.LoadCommunities(PageType, GetCurrentCookie, 0, DefaultPageSize, CurrentOrderBy, CurrentAscending, IdCurrentCommunityType, IdCurrentRemoveCommunityType, IdCurrentTile, IdCurrentTile)
            End If
        End If
    End Sub
    Private Sub LNBviewAll_Click(sender As Object, e As EventArgs) Handles LNBviewAll.Click
        Dim dto As dtoTileDisplay = Nothing
        If (IdCurrentTile > 0 AndAlso IdCurrentTileTags.Any()) Then
            dto = New dtoTileDisplay() With {.Id = IdCurrentTile, .Tags = IdCurrentTileTags}
        End If
        CurrentPresenter.ShowMoreCommunities(True, PageType, CurrentOrderBy, CurrentAscending, IdCurrentCommunityType, IdCurrentRemoveCommunityType, IdCurrentTile, IdCurrentTag, dto)
    End Sub
    Private Sub LNBviewLess_Click(sender As Object, e As EventArgs) Handles LNBviewLess.Click
        Me.Pager.GoFirst()
        Me.DVpager.Visible = False
        Dim dto As dtoTileDisplay = Nothing
        If (IdCurrentTile > 0 AndAlso IdCurrentTileTags.Any()) Then
            dto = New dtoTileDisplay() With {.Id = IdCurrentTile, .Tags = IdCurrentTileTags}
        End If
        CurrentPresenter.ShowMoreCommunities(False, PageType, CurrentOrderBy, CurrentAscending, IdCurrentCommunityType, IdCurrentRemoveCommunityType, IdCurrentTile, IdCurrentTag, dto)
    End Sub

    Private Sub RPTcommunities_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles RPTcommunities.ItemCommand
        Dim oResourceConfig As New ResourceManager

        If PageUtility.CurrentContext.UserContext.isAnonymous Then
            RaiseEvent SessionTimeout()
        Else
            Select Case e.CommandName
                Case "unsubscribe"
                    Dim info As String = e.CommandArgument
                    CurrentPresenter.UnsubscribeFromCommunity(CInt(info.Split(",")(0)), info.Split(",")(1), CurrentFilters, CurrentOrderBy, CurrentAscending, Me.Pager.PageIndex, CurrentPageSize)
                Case Else
                    oResourceConfig = GetResourceConfig(Session("LinguaCode"))
                    Dim result As lm.Comol.Core.DomainModel.SubscriptionStatus = lm.Comol.Core.DomainModel.SubscriptionStatus.none
                    If String.IsNullOrEmpty(e.CommandArgument) Then
                        result = PageUtility.AccessToCommunity(PageUtility.CurrentContext.UserContext.CurrentUserID, CInt(e.CommandName), oResourceConfig, True)
                    Else
                        result = PageUtility.AccessToCommunity(PageUtility.CurrentContext.UserContext.CurrentUserID, CInt(e.CommandName), e.CommandArgument, oResourceConfig, True)
                    End If
                    Dim oLabel = e.Item.FindControl("LBstatus")
                    Select Case result
                        Case lm.Comol.Core.DomainModel.SubscriptionStatus.blocked, lm.Comol.Core.DomainModel.SubscriptionStatus.communityblocked
                            oLabel.CssClass = LTcssClassStatus.Text & " " & LTcssClassStatusBlocked.Text
                            oLabel.ToolTip = Resource.getValue("LBstatus.SubscriptionStatus." & result.ToString)
                        Case lm.Comol.Core.DomainModel.SubscriptionStatus.waiting
                            oLabel.CssClass = LTcssClassStatus.Text & " " & LTcssClassStatusWaiting.Text
                            oLabel.ToolTip = Resource.getValue("LBstatus.SubscriptionStatus." & result.ToString)
                    End Select
            End Select
        End If
    End Sub

    Private Sub RPTcommunities_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTcommunities.ItemDataBound

        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim dto As dtoSubscriptionItem = e.Item.DataItem

                Dim oHyperlink As HyperLink = e.Item.FindControl("HYPcommunityInfo")
                oHyperlink.ToolTip = Resource.getValue("HYPcommunityInfo.ToolTip." & HasGenericConstraints(dto))
                oHyperlink.NavigateUrl = PageUtility.ApplicationUrlBase() & lm.Comol.Core.Dashboard.Domain.RootObject.CommunityDetails(dto.Community.Id, True)

                Dim oLinkbutton As LinkButton = e.Item.FindControl("LNBcommunityAccess")
                oLinkbutton.Text = dto.Community.Name
                oLinkbutton.CommandArgument = dto.Community.Path
                oLinkbutton.CommandName = dto.Community.Id

                Dim oLabel As Label = e.Item.FindControl("LBcommunityAccess")
                oLabel.Text = dto.Community.Name

                Dim isAvailable As Boolean = (dto.Status <> lm.Comol.Core.DomainModel.SubscriptionStatus.blocked AndAlso dto.Status <> lm.Comol.Core.DomainModel.SubscriptionStatus.communityblocked AndAlso dto.Status <> lm.Comol.Core.DomainModel.SubscriptionStatus.waiting)
                oLinkbutton.Visible = isAvailable AndAlso Not IsPreview
                oLabel.Visible = Not isAvailable OrElse IsPreview

                Dim oControl As HtmlControl = e.Item.FindControl("DVtag")
                oControl.Visible = dto.Community.Tags.Any()
                If dto.Community.Tags.Any() Then
                    oLabel = e.Item.FindControl("LBcommunityTagsTitle")
                    Resource.setLabel(oLabel)
                End If
                If IsForSearch AndAlso dto.Community.IdType = lm.Comol.Core.DomainModel.CommunityTypeStandard.Organization Then
                    oLabel = e.Item.FindControl("LBtagCommunityType")
                    oLabel.Visible = True
                    oLabel.Text = Resource.getValue("LBtagCommunityType." & lm.Comol.Core.DomainModel.CommunityTypeStandard.Organization.ToString)
                End If

                Dim oCell As HtmlTableCell = e.Item.FindControl("TDgenericDateDime")
                If AvailableColumns.Contains(lm.Comol.Core.BaseModules.Dashboard.Domain.searchColumn.genericdate) Then
                    oCell.Visible = True
                    Dim oLiteral As Literal = e.Item.FindControl("LTgenericDateDime")

                    Select Case CurrentOrderBy
                        Case OrderItemsBy.ClosedOn
                            oLiteral.Text = GetDateTimeString(dto.Community.ClosedOn, "")
                        Case OrderItemsBy.CreatedOn
                            oLiteral.Text = GetDateTimeString(dto.Community.CreatedOn, "")
                        Case OrderItemsBy.LastAccess
                            oLiteral.Text = GetDateTimeString(dto.LastAccessOn, "")
                        Case OrderItemsBy.Name
                            oLiteral.Text = GetDateTimeString(dto.LastAccessOn, "")
                    End Select
                Else
                    oCell.Visible = False
                End If

                oCell = e.Item.FindControl("TDsubscriptionInfo")
                If AvailableColumns.Contains(lm.Comol.Core.BaseModules.Dashboard.Domain.searchColumn.subscriptioninfo) Then
                    oCell.Visible = True
                    oLabel = e.Item.FindControl("LBstatus")
                    Select Case dto.Status
                        Case lm.Comol.Core.DomainModel.SubscriptionStatus.activemember, lm.Comol.Core.DomainModel.SubscriptionStatus.newUser, lm.Comol.Core.DomainModel.SubscriptionStatus.responsible
                            oLabel.CssClass = LTcssClassStatus.Text & " " & LTcssClassStatusActive.Text
                        Case lm.Comol.Core.DomainModel.SubscriptionStatus.blocked
                            oLabel.CssClass = LTcssClassStatus.Text & " " & LTcssClassStatusBlocked.Text
                        Case lm.Comol.Core.DomainModel.SubscriptionStatus.communityblocked
                            oLabel.CssClass = LTcssClassStatus.Text & " " & LTcssClassStatusBlocked.Text
                        Case lm.Comol.Core.DomainModel.SubscriptionStatus.waiting
                            oLabel.CssClass = LTcssClassStatus.Text & " " & LTcssClassStatusWaiting.Text
                    End Select
                    oLabel.ToolTip = Resource.getValue("LBstatus.SubscriptionStatus." & dto.Status.ToString)
                    oHyperlink = e.Item.FindControl("HYPnews")
                    oHyperlink.Visible = dto.HasNews
                    If dto.HasNews Then
                        Resource.setHyperLink(oHyperlink, False, True)

                        If IsPreview Then
                            oHyperlink.Enabled = False
                        Else
                            Dim FromDay As DateTime = DateTime.MinValue
                            If dto.LastAccessOn.HasValue Then
                                FromDay = dto.LastAccessOn.Value
                            ElseIf dto.SubscribedOn.HasValue Then
                                FromDay = dto.SubscribedOn.Value
                            Else
                                FromDay = Now.Date.AddDays(-30)
                            End If
                            Dim view As lm.Modules.NotificationSystem.Domain.ViewModeType
                            Select Case PageType
                                Case DashboardViewType.List
                                    view = lm.Modules.NotificationSystem.Domain.ViewModeType.FromDashboardList
                                Case DashboardViewType.Combined
                                    view = lm.Modules.NotificationSystem.Domain.ViewModeType.FromDashboardCombined
                                Case DashboardViewType.Search
                                    view = lm.Modules.NotificationSystem.Domain.ViewModeType.FromDashboardSearch
                            End Select
                            oHyperlink.NavigateUrl = PageUtility.ApplicationUrlBase() & "Notification/CommunityNews.aspx?FromDay=" & Me.PageUtility.GetUrlEncoded(FromDay.ToString) & "&PageSize=25&Page=0&CommunityID=" & dto.Community.Id & "&PR_View=" & view.ToString
                        End If

                    End If
                Else
                    oCell.Visible = False
                End If
                oCell = e.Item.FindControl("TDactions")
                If AvailableColumns.Contains(lm.Comol.Core.BaseModules.Dashboard.Domain.searchColumn.actions) Then
                    oCell.Visible = True
                    oLinkbutton = e.Item.FindControl("LNBvirtualDeleteSubscription")
                    If dto.AllowUnsubscribe Then
                        oLinkbutton.Visible = True
                        oLinkbutton.Enabled = Not IsPreview
                        oLinkbutton.CommandArgument = dto.Community.Id & "," & dto.Community.Path
                        Resource.setLinkButton(oLinkbutton, False, True)
                    Else
                        oLinkbutton.Visible = False
                    End If

                Else
                    oCell.Visible = False
                End If
            Case ListItemType.Footer
                Dim oTableItem As HtmlControl = e.Item.FindControl("TRempty")
                oTableItem.Visible = (RPTcommunities.Items.Count = 0)
                If (RPTcommunities.Items.Count = 0) Then
                    Dim oTableCell As HtmlTableCell = e.Item.FindControl("TDemptyItems")

                    oTableCell.ColSpan = 2 + AvailableColumns.Count
                    Dim oLabel As Label = e.Item.FindControl("LBemptyItems")

                    If _noItemsToDisplay = True Then
                        _noItemsToDisplay = False
                        oLabel.Text = Resource.getValue("NoCommunities.DashboardViewType." & PageType.ToString & ".NoItems")
                    Else
                        oLabel.Text = Resource.getValue("NoCommunities.DashboardViewType." & PageType.ToString)
                    End If
                End If
        End Select
    End Sub
    Private Sub PGgridBottom_OnPageSelected() Handles PGgridBottom.OnPageSelected
        Dim pagesize As Integer = CurrentPageSize

        If IsForSearch Then
            If HasDisplayMessage Then
                RaiseEvent HideDisplayMessage()
                HasDisplayMessage = False
            End If
            CurrentPresenter.LoadCommunities(CurrentFilters, CurrentOrderBy, CurrentAscending, Me.Pager.PageIndex, pagesize)
            '      CurrentPresenter. , Me.Pager.PageIndex, pagesize, CurrentOrderBy, CurrentAscending, IdCurrentCommunityType, IdCurrentRemoveCommunityType, IdCurrentTile, IdCurrentTile)
        Else
            If DisplayLessCommand Then
                pagesize = DefaultPageSize
            End If
            CurrentPresenter.LoadCommunities(PageType, GetCurrentCookie, Me.Pager.PageIndex, pagesize, CurrentOrderBy, CurrentAscending, IdCurrentCommunityType, IdCurrentRemoveCommunityType, IdCurrentTile, IdCurrentTile)
        End If
    End Sub

    Protected Sub LNBorderBy_Click(sender As Object, e As System.EventArgs)
        CurrentAscending = CBool(DirectCast(sender, LinkButton).CommandArgument)
        If HasDisplayMessage Then
            RaiseEvent HideDisplayMessage()
            HasDisplayMessage = False
        End If
        If IsForSearch Then
            CurrentPresenter.LoadCommunities(CurrentFilters, CurrentOrderBy, CurrentAscending, 0, CurrentPageSize)
        Else
            If DisplayMoreCommand Then
                CurrentPresenter.LoadCommunities(PageType, GetCurrentCookie, 0, CurrentPageSize, CurrentOrderBy, CurrentAscending, IdCurrentCommunityType, IdCurrentRemoveCommunityType, IdCurrentTile, IdCurrentTile)
            Else
                CurrentPresenter.LoadCommunities(PageType, GetCurrentCookie, 0, DefaultPageSize, CurrentOrderBy, CurrentAscending, IdCurrentCommunityType, IdCurrentRemoveCommunityType, IdCurrentTile, IdCurrentTile)
            End If
        End If
    End Sub

    Private Sub CTRLconfirmUnsubscription_UnsubscribeFromCommunity(idCommunity As Integer, path As String, rAction As RemoveAction) Handles CTRLconfirmUnsubscription.UnsubscribeFromCommunity
        RaiseEvent OpenConfirmDialog("")
        CTRLconfirmUnsubscription.Visible = False
        CurrentPresenter.UnsubscribeFromCommunity(idCommunity, path, rAction, CurrentFilters, CurrentOrderBy, CurrentAscending, Me.Pager.PageIndex, CurrentPageSize)
    End Sub
#Region "Styles"

    Public Function GetSearchCssClass() As String
        If IsForSearch Then
            Return LTcssClassSearch.Text
        End If
        Return ""
    End Function
    Public Function GetContainerCssClass() As String
        Dim cssClass As String = " "
        If IsCollapsable Then
            cssClass &= LTcssClassCollapsable.Text & " "
            If IsCollapsed AndAlso Not Page.IsPostBack Then
                cssClass &= LTcssClassCollapsed.Text & " "
            End If
        End If
        cssClass &= GetItemCssClass(CurrentDisplayOrder)
        Return cssClass
    End Function
    Public Function GetDataIdForCookie() As String
        Return DataIdForCollapse
    End Function
    Public Function GetTitleCssClass() As String
        Dim cssClass As String = TitleCssClass
        If String.IsNullOrEmpty(cssClass) AndAlso String.IsNullOrEmpty(TitleImage) Then
            cssClass = " " & LTcssClassTileIcon.Text & " " & LTcssClassDefaultItemClass.Text
        ElseIf Not String.IsNullOrEmpty(cssClass) Then
            cssClass = " " & cssClass & " " & LTcssClassTileIcon.Text
        ElseIf Not String.IsNullOrEmpty(TitleImage) Then
            cssClass &= " " & LTcssClassCustomTile.Text
        End If
        Return cssClass
    End Function
    Public Function GetHideMeCssClass() As String
        If IsCollapsable Then
            Return LTcssClassHideme.Text
        Else
            Return ""
        End If
    End Function

    Public Function GetOrderByItemCssClass(ByVal item As dtoItemFilter(Of OrderItemsBy)) As String
        Dim cssClass As String = GetItemCssClass(item.DisplayAs)
        If item.Selected Then
            cssClass &= " " & LTcssClassActive.Text
        End If
        Return cssClass
    End Function
    Private Function GetItemCssClass(ByVal d As lm.Comol.Core.DomainModel.ItemDisplayOrder) As String
        Dim cssClass As String = ""
        Select Case d
            Case lm.Comol.Core.DomainModel.ItemDisplayOrder.first, lm.Comol.Core.DomainModel.ItemDisplayOrder.last
                cssClass = " " & d.ToString
            Case lm.Comol.Core.DomainModel.ItemDisplayOrder.item
                cssClass = ""
            Case Else
                cssClass = " " & lm.Comol.Core.DomainModel.ItemDisplayOrder.first.ToString() & " " & lm.Comol.Core.DomainModel.ItemDisplayOrder.last.ToString()
        End Select
        Return cssClass
    End Function
    Public Function GetCommunityCssClass(ByVal item As dtoSubscriptionItem) As String
        Dim cssClass As String = ""
        If HasGenericConstraints(item) Then
            cssClass &= LTcssClassConstraints.Text
        End If
        Return cssClass
    End Function
#End Region

#End Region

End Class