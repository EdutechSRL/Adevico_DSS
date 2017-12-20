Imports lm.ActionDataContract
Imports lm.Comol.Core.BaseModules.Tiles.Presentation
Imports lm.Comol.Core.Dashboard.Domain
Imports lm.Comol.Core.BaseModules.Tiles.Domain

Public Class UC_TileList
    Inherits TGbaseControl
    Implements IViewTileList

#Region "Context"
    Private _presenter As TileListPresenter
    Protected Friend ReadOnly Property CurrentPresenter As TileListPresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New TileListPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private Property IdTilesCommunity As Integer Implements IViewTileList.IdTilesCommunity
        Get
            Return ViewStateOrDefault("IdTilesCommunity", 0)
        End Get
        Set(value As Integer)
            ViewState("IdTilesCommunity") = value
        End Set
    End Property
    Private Property CurrentPageSize As Integer Implements IViewTileList.CurrentPageSize
        Get
            Return ViewStateOrDefault("CurrentPageSize", 25)
        End Get
        Set(value As Integer)
            ViewState("CurrentPageSize") = value
        End Set
    End Property
    Public Property IdSelectedTileLanguage As Integer Implements IViewTileList.IdSelectedTileLanguage
        Get
            Return ViewStateOrDefault("IdSelectedTileLanguage", -1)
        End Get
        Set(value As Integer)
            ViewState("IdSelectedTileLanguage") = value
        End Set
    End Property
    Private Property Pager As lm.Comol.Core.DomainModel.PagerBase Implements IViewTileList.Pager
        Get
            Return ViewStateOrDefault("Pager", New lm.Comol.Core.DomainModel.PagerBase)
        End Get
        Set(ByVal value As lm.Comol.Core.DomainModel.PagerBase)
            Me.ViewState("Pager") = value
            Me.PGgridBottom.Pager = value
            Me.DVpager.Visible = (Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize))
        End Set
    End Property
    Private Property FirstLoad As Boolean Implements IViewTileList.FirstLoad
        Get
            Return ViewStateOrDefault("FirstLoad", True)
        End Get
        Set(value As Boolean)
            ViewState("FirstLoad") = value
        End Set
    End Property
    Private Property FirstLoadForLanguages As Dictionary(Of Integer, Boolean) Implements IViewTileList.FirstLoadForLanguages
        Get
            Return ViewStateOrDefault("FirstLoadForLanguages", New Dictionary(Of Integer, Boolean))
        End Get
        Set(value As Dictionary(Of Integer, Boolean))
            ViewState("FirstLoadForLanguages") = value
        End Set
    End Property
    Private Property CurrentFilters As lm.Comol.Core.BaseModules.Tiles.Domain.dtoFilters Implements IViewTileList.CurrentFilters
        Get
            Return ViewStateOrDefault("CurrentFilters", New lm.Comol.Core.BaseModules.Tiles.Domain.dtoFilters() With {.DashboardType = DashboardType.Portal})
        End Get
        Set(value As lm.Comol.Core.BaseModules.Tiles.Domain.dtoFilters)
            ViewState("CurrentFilters") = value
        End Set
    End Property
#End Region

#Region "Internal"
    Public Event HideAutoGenerateButton(hideAutoGenerate As Boolean)
    Public Event LanguageChanged(ByVal idLanguage As Integer)
    Public Event SessionTimeout()
    Public Event SetDefaultFilters(ByVal filters As List(Of lm.Comol.Core.DomainModel.Filters.Filter))
    Protected Friend ReadOnly Property PreloadIdDashboard As Long
        Get
            If IsNumeric(Request.QueryString("fromIdDashboard")) Then
                Return CLng(Request.QueryString("fromIdDashboard"))
            Else
                Return 0
            End If
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadStep As WizardDashboardStep
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of WizardDashboardStep).GetByString(Request.QueryString("step"), WizardDashboardStep.None)
        End Get
    End Property
#End Region

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PGgridBottom.Pager = Me.Pager
    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLinkButton(LNBapplyTileFilters, False, True)
            .setLabel(LBdisplayLanguageSelectorDescription)

            .setLiteral(LTthTileName)
            .setLiteral(LTthTileType)
            .setLiteral(LTthTranslations)
            .setLiteral(LTthModifiedBy)
            .setLiteral(LTthModifiedOn)
            LBactions.ToolTip = .getValue("LBactions.ToolTip")

            .setLinkButton(LNBorderByNameUp, False, True)
            .setLinkButton(LNBorderByNameDown, False, True)
            .setLinkButton(LNBorderTileByTypeUp, False, True)
            .setLinkButton(LNBorderTileByTypeDown, False, True)
            .setLinkButton(LNBorderByModifiedByDown, False, True)
            .setLinkButton(LNBorderByModifiedByUp, False, True)
            .setLinkButton(LNBorderByModifiedOnDown, False, True)
            .setLinkButton(LNBorderByModifiedOnUp, False, True)

            .setLabel(LBtileStatistics)
            .setLabel(LBtilesNotTranslated_t)
            .setLabel(LBtableLegend)
            .setLabel(LBspanExpandList)
            .setLabel(LBspanCollapseList)
            .setLiteral(LTsearchTileFiltersTitle)
            .setLabel(LBcommunityTypesWithoutTiles_t)
            .setButton(BTNreloadItems)
            LTdraftItem.Text = String.Format(LTtemplateLegendItem.Text, .getValue("Tile.Draft"), LTcssClassDraft.Text)
        End With
    End Sub
#End Region


#Region "Implements"
    Public Sub InitializeControl(permissions As ModuleDashboard, type As DashboardType, idCommunity As Integer, fromRecycleBin As Boolean, idTile As Long, preloadType As TileType) Implements IViewTileList.InitializeControl
        CurrentPresenter.InitView(permissions, type, idCommunity, fromRecycleBin, idTile, preloadType)
    End Sub
    Private Sub LoadLanguages(items As List(Of lm.Comol.Core.Dashboard.Domain.dtoItemFilter(Of lm.Comol.Core.DomainModel.Languages.dtoLanguageItem))) Implements IViewTileList.LoadLanguages
        If items.Count > 1 Then
            DVlanguageSelector.Visible = True
            RPTdisplayLanguage.DataSource = items
            RPTdisplayLanguage.DataBind()
            If items.Where(Function(i) i.Selected).Any Then
                LBdisplayLanguageSelected.Text = items.Where(Function(i) i.Selected).FirstOrDefault.Value.LanguageName
            Else
                LBdisplayLanguageSelected.Text = ""
            End If
        Else
            DVlanguageSelector.Visible = False
        End If
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewBase.DisplayNoPermission

    End Sub
    Private Sub DisplayMessage(action As ModuleDashboard.ActionType) Implements IViewTileList.DisplayMessage
        Dim mType = lm.Comol.Core.DomainModel.Helpers.MessageType.error
        Select Case action
            Case ModuleDashboard.ActionType.TileAlreadyGeneratedForCommunityTypes
                mType = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
            Case ModuleDashboard.ActionType.TileDisable, ModuleDashboard.ActionType.TileEnable, ModuleDashboard.ActionType.TileVirtualDelete, ModuleDashboard.ActionType.TileVirtualUndelete, ModuleDashboard.ActionType.TileAutoGenerateForCommunityTypes
                mType = lm.Comol.Core.DomainModel.Helpers.MessageType.success
        End Select
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayMessage.ModuleTile.ActionType." & action.ToString), mType)
    End Sub
    Private Sub DisplayErrorLoadingFromDB() Implements IViewTileList.DisplayErrorLoadingFromDB
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayErrorLoadingFromDB"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewBase.DisplaySessionTimeout
        RaiseEvent SessionTimeout()
    End Sub
    Private Sub AllowApplyFilters(allow As Boolean) Implements IViewTileList.AllowApplyFilters
        DVfilters.Visible = allow
    End Sub
    Private Function GetDefaultLanguageName() As String Implements IViewTileList.GetDefaultLanguageName
        Return Resource.getValue("GetDefaultLanguageName")
    End Function
    Private Function GetDefaultLanguageCode() As String Implements IViewTileList.GetDefaultLanguageCode
        Return Resource.getValue("GetDefaultLanguageCode")
    End Function
    Private Function GetUnknownUserName() As String Implements IViewTileList.GetUnknownUserName
        Return Resource.getValue("UnknownUserName")
    End Function
    Private Sub LoadTilesInfo(unstranslatedTiles As Integer) Implements IViewTileList.LoadTilesInfo
        DVstatistics.Visible = True
        SPNcommunityTypesWithoutTiles.Visible = False
        LBtilesNotTranslated.Text = unstranslatedTiles.ToString
        If unstranslatedTiles > 0 Then
            LBtilesNotTranslated.CssClass = LTcssClassInfoWarning.Text
        Else
            LBtilesNotTranslated.CssClass = LTcssClassInfo.Text
        End If
    End Sub
    Private Sub LoadTilesInfo(communityTypesWithoutTiles As Integer, unstranslatedTiles As Integer) Implements IViewTileList.LoadTilesInfo
        LoadTilesInfo(unstranslatedTiles)
        SPNcommunityTypesWithoutTiles.Visible = True
        LBcommunityTypesWithoutTiles.Text = communityTypesWithoutTiles.ToString
        If communityTypesWithoutTiles > 0 Then
            LBcommunityTypesWithoutTiles.CssClass = LTcssClassInfoWarning.Text
        Else
            LBcommunityTypesWithoutTiles.CssClass = LTcssClassInfo.Text
        End If
    End Sub
    Private Function GetSubmittedFilters() As lm.Comol.Core.BaseModules.Tiles.Domain.dtoFilters Implements IViewTileList.GetSubmittedFilters
        Dim filter As lm.Comol.Core.BaseModules.Tiles.Domain.dtoFilters = CurrentFilters

        With filter
            Dim keys As List(Of String) = Request.Form.AllKeys.ToList()
            For Each item As lm.Comol.Core.BaseModules.Tiles.Domain.searchFilterType In [Enum].GetValues(GetType(lm.Comol.Core.BaseModules.Tiles.Domain.searchFilterType))
                Select Case item
                    Case lm.Comol.Core.BaseModules.Tiles.Domain.searchFilterType.modifiedby
                        If keys.Contains(item.ToString) AndAlso Not String.IsNullOrEmpty(Request.Form(item.ToString)) Then
                            .IdModifiedBy = CInt(Request.Form(item.ToString))
                        Else
                            .IdModifiedBy = -1
                        End If
                    Case lm.Comol.Core.BaseModules.Tiles.Domain.searchFilterType.status
                        If keys.Contains(item.ToString) AndAlso Not String.IsNullOrEmpty(Request.Form(item.ToString)) Then
                            .Status = CInt(Request.Form(item.ToString))
                        Else
                            .Status = lm.Comol.Core.Dashboard.Domain.AvailableStatus.Any
                        End If
                    Case lm.Comol.Core.BaseModules.Tiles.Domain.searchFilterType.type
                        If keys.Contains(item.ToString) AndAlso Not String.IsNullOrEmpty(Request.Form(item.ToString)) Then
                            .IdTileType = CInt(Request.Form(item.ToString))
                        Else
                            .IdTileType = -1
                        End If
                    Case lm.Comol.Core.BaseModules.Tiles.Domain.searchFilterType.name
                        .Name = Request.Form(item.ToString)
                    Case lm.Comol.Core.BaseModules.Tiles.Domain.searchFilterType.letters
                        If Not String.IsNullOrEmpty(Request.Form(item.ToString)) Then
                            Dim charInt As Integer = CInt(Request.Form(item.ToString))
                            Select Case charInt
                                Case -1
                                    .StartWith = ""
                                Case -9
                                    .StartWith = "#"
                                Case Else
                                    .StartWith = Char.ConvertFromUtf32(charInt).ToLower()
                            End Select
                        End If
                End Select
            Next
        End With

        Return filter
    End Function
    Private Sub LoadTiles(items As List(Of dtoTileItem), idLanguage As Integer) Implements IViewTileList.LoadTiles
        RPTtiles.DataSource = items
        RPTtiles.DataBind()

        Dim display As Boolean = items.Any AndAlso items.Count > 1
        LNBorderByNameUp.Visible = display
        LNBorderByNameDown.Visible = display
        LNBorderTileByTypeUp.Visible = display
        LNBorderTileByTypeDown.Visible = display
        LNBorderByModifiedByDown.Visible = display
        LNBorderByModifiedByUp.Visible = display
        LNBorderByModifiedOnDown.Visible = display
        LNBorderByModifiedOnUp.Visible = display
        FirstLoad = False
        FirstLoadForLanguages(idLanguage) = False
        DVlegend.Visible = items.Any()
    End Sub
    Private Function GetTranslatedTileTypes() As Dictionary(Of TileType, String) Implements IViewTileList.GetTranslatedTileTypes
        Return (From t In [Enum].GetValues(GetType(TileType)).Cast(Of TileType).ToList Select t).ToDictionary(Of TileType, String)(Function(t) t, Function(t) Resource.getValue("TileType." & t.ToString))
    End Function
    Public Sub GenerateCommunityTypesTile() Implements IViewTileList.GenerateCommunityTypesTile
        CurrentPresenter.GenerateCommunityTypeTiles(CurrentFilters, IdTilesCommunity, Pager.PageIndex, CurrentPageSize)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As ModuleDashboard.ActionType) Implements IViewTileList.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, Nothing, InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idTile As Long, action As ModuleDashboard.ActionType) Implements IViewTileList.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleDashboard.ObjectType.Tile, idTile.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub HideCommunityTypesTileAutoGenerate(hideAutoGenerate As Boolean) Implements IViewTileList.HideCommunityTypesTileAutoGenerate
        RaiseEvent HideAutoGenerateButton(hideAutoGenerate)
    End Sub
    Private Sub LoadDefaultFilters(filters As List(Of lm.Comol.Core.DomainModel.Filters.Filter)) Implements IViewTileList.LoadDefaultFilters
        RaiseEvent SetDefaultFilters(filters)
        DVfilters.Visible = filters.Any()
    End Sub
    'Public Sub SetTransacionIdContainer(value As String)
    '    CTRLfiltersHeader.SetTransacionIdContainer(value)
    'End Sub
#End Region

#Region "Internal"
    Private Sub LNBapplyTileFilters_Click(sender As Object, e As EventArgs) Handles LNBapplyTileFilters.Click
        Dim filters As lm.Comol.Core.BaseModules.Tiles.Domain.dtoFilters = GetSubmittedFilters()
        CTRLmessages.Visible = False
        CurrentPresenter.ApplyFilters(filters, IdTilesCommunity, CurrentPageSize)
    End Sub
    Private Sub RPTdisplayLanguage_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTdisplayLanguage.ItemDataBound
        Dim oLinkButton As LinkButton = e.Item.FindControl("LNBdisplayLanguage")
        Dim oItem As lm.Comol.Core.Dashboard.Domain.dtoItemFilter(Of lm.Comol.Core.DomainModel.Languages.dtoLanguageItem) = e.Item.DataItem

        oLinkButton.CommandArgument = CInt(oItem.Value.IdLanguage)
        If oLinkButton.Text.Contains("{0}") Then
            oLinkButton.Text = String.Format(oLinkButton.Text, oItem.Value.LanguageName)
        End If

        Dim oControl As HtmlControl = e.Item.FindControl("DVitemDisplayLanguage")
        oControl.Attributes("class") = LTcssClassSelectBy.Text & " " & GetOrderByItemCssClass(oItem)
    End Sub
    Private Sub RPTdisplayLanguage_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles RPTdisplayLanguage.ItemCommand
        Dim oLinkbutton As LinkButton = e.Item.FindControl("LNBdisplayLanguage")
        LBdisplayLanguageSelected.Text = oLinkbutton.Text

        Dim oControl As HtmlControl = e.Item.FindControl("DVitemDisplayLanguage")
        If Not oControl.Attributes("class").Contains(LTcssClassActive.Text) Then
            oControl.Attributes("class") = oControl.Attributes("class") & " " & LTcssClassActive.Text

            For Each row As RepeaterItem In (From r As RepeaterItem In RPTdisplayLanguage.Items Where r.ItemIndex <> e.Item.ItemIndex)
                oControl = row.FindControl("DVitemDisplayLanguage")
                If oControl.Attributes("class").Contains(LTcssClassActive.Text) Then
                    oControl.Attributes("class") = Replace(oControl.Attributes("class"), LTcssClassActive.Text, "")
                    Exit For
                End If
            Next
        End If
        RaiseEvent LanguageChanged(CInt(e.CommandArgument))
        Dim filter As lm.Comol.Core.BaseModules.Tiles.Domain.dtoFilters = CurrentFilters
        filter.IdSelectedLanguage = CInt(e.CommandArgument)
        filter.Ascending = True
        filter.OrderBy = lm.Comol.Core.BaseModules.Tiles.Domain.OrderTilesBy.Name
        filter.Name = ""
        filter.StartWith = ""
        CurrentFilters = filter
        CTRLmessages.Visible = False
        CurrentPresenter.LoadTiles(filter, IdTilesCommunity, 0, CurrentPageSize)
    End Sub
    Private Sub RPTtiles_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles RPTtiles.ItemCommand
        If PageUtility.CurrentContext.UserContext.isAnonymous Then
            RaiseEvent SessionTimeout()
        Else
            Select Case e.CommandName
                Case "hide"
                    CurrentPresenter.SetStatus(CLng(e.CommandArgument), lm.Comol.Core.Dashboard.Domain.AvailableStatus.Unavailable, CurrentFilters, IdTilesCommunity, Pager.PageIndex, CurrentPageSize)
                Case "show"
                    CurrentPresenter.SetStatus(CLng(e.CommandArgument), lm.Comol.Core.Dashboard.Domain.AvailableStatus.Available, CurrentFilters, IdTilesCommunity, Pager.PageIndex, CurrentPageSize)
                Case "virtualdelete"
                    CurrentPresenter.VirtualDelete(CLng(e.CommandArgument), True, CurrentFilters, IdTilesCommunity, Pager.PageIndex, CurrentPageSize)
                Case "recover"
                    CurrentPresenter.VirtualDelete(CLng(e.CommandArgument), False, CurrentFilters, IdTilesCommunity, Pager.PageIndex, CurrentPageSize)
            End Select
        End If
    End Sub

    Private Sub RPTtiles_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTtiles.ItemDataBound
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim dto As dtoTileItem = e.Item.DataItem

                Dim oLabel As Label = e.Item.FindControl("LBtileLanguageInUse")
                Dim translation As lm.Comol.Core.DomainModel.Languages.dtoLanguageItem = dto.Translations.Where(Function(t) t.IdLanguage.Equals(dto.IdDisplayLanguage)).FirstOrDefault()
                If IsNothing(translation) Then
                    oLabel.Visible = False
                ElseIf translation.IdLanguage = -1 Then
                    oLabel.Visible = True
                    oLabel.Text = translation.ShortCode
                    oLabel.ToolTip = Resource.getValue("LBtileLanguageInUse-1")
                Else
                    oLabel.Text = translation.ShortCode
                    oLabel.Visible = True
                    oLabel.ToolTip = String.Format(Resource.getValue("LBtileLanguageInUse"), translation.LanguageName)
                End If

                oLabel = e.Item.FindControl("LBmodifiedOn")
                oLabel.Text = GetDateToString(dto.ModifiedOn, "-")
                oLabel.ToolTip = GetDateTimeString(dto.ModifiedOn, "-")

                Dim oLiteral As Literal = e.Item.FindControl("LTtileType")
                oLiteral.Text = dto.TranslatedType

                Dim oHyperlink As HyperLink = e.Item.FindControl("HYPviewTile")
                Resource.setHyperLink(oHyperlink, False, True)
                oHyperlink.Visible = dto.Permissions.AllowView
                If dto.Permissions.AllowView Then
                    oHyperlink.NavigateUrl = BaseUrl & RootObject.TileView(dto.Id, DashboardType.Portal, IdTilesCommunity, PreloadIdDashboard, PreloadStep)
                End If
                oHyperlink = e.Item.FindControl("HYPeditTile")
                oHyperlink.Visible = dto.Permissions.AllowEdit
                If dto.Permissions.AllowEdit Then
                    oHyperlink.NavigateUrl = BaseUrl & RootObject.TileEdit(dto.Id, DashboardType.Portal, IdTilesCommunity, PreloadIdDashboard, PreloadStep)
                End If
                Resource.setHyperLink(oHyperlink, False, True)
                Dim oLinkbutton As LinkButton = e.Item.FindControl("LNBhideTile")
                oLinkbutton.Visible = dto.Permissions.AllowSetUnavailable
                Resource.setLinkButton(oLinkbutton, False, True)

                oLinkbutton = e.Item.FindControl("LNBshowTile")
                oLinkbutton.Visible = dto.Permissions.AllowSetAvailable
                Resource.setLinkButton(oLinkbutton, False, True)

                oLinkbutton = e.Item.FindControl("LNBvirtualDeleteTile")
                oLinkbutton.Visible = dto.Permissions.AllowVirtualDelete
                Resource.setLinkButton(oLinkbutton, False, True)

                oLinkbutton = e.Item.FindControl("LNBvirtualUnDeleteTile")
                oLinkbutton.Visible = dto.Permissions.AllowUnDelete
                Resource.setLinkButton(oLinkbutton, False, True)

            Case ListItemType.Footer
                Dim oTableItem As HtmlControl = e.Item.FindControl("TRempty")
                oTableItem.Visible = (RPTtiles.Items.Count = 0)
                If (RPTtiles.Items.Count = 0) Then
                    Dim oLabel As Label = e.Item.FindControl("LBemptyItems")
                    If FirstLoad Then
                        oLabel.Text = Resource.getValue("NoTilesFound.FirstLoad." & CurrentFilters.FromRecycleBin)
                    ElseIf FirstLoadForLanguages(CurrentFilters.IdSelectedLanguage) Then
                        oLabel.Text = Resource.getValue("NoTilesFound.ForLanguage")
                    Else
                        oLabel.Text = Resource.getValue("NoTilesFound.Filters")
                    End If
                End If
        End Select
    End Sub
    Protected Sub RPTlanguages_ItemDataBound(sender As Object, e As RepeaterItemEventArgs)
        Dim dto As lm.Comol.Core.DomainModel.Languages.dtoLanguageItem = e.Item.DataItem

        Dim oLabel As Label = e.Item.FindControl("LBtemplateLanguage")
        oLabel.Text = dto.ShortCode
        If dto.IdLanguage > 0 Then
            oLabel.ToolTip = dto.LanguageName
        Else
            oLabel.ToolTip = Resource.getValue("LBtemplateLanguage.Default")
        End If

    End Sub
    Private Sub PGgridBottom_OnPageSelected() Handles PGgridBottom.OnPageSelected
        CTRLmessages.Visible = False
        CurrentPresenter.LoadTiles(CurrentFilters, IdTilesCommunity, Pager.PageIndex, CurrentPageSize)
    End Sub
    Private Sub BTNreloadItems_Click(sender As Object, e As EventArgs) Handles BTNreloadItems.Click
        CTRLmessages.Visible = False
        CurrentPresenter.LoadTiles(CurrentFilters, IdTilesCommunity, Pager.PageIndex, CurrentPageSize)
    End Sub

    Protected Sub LNBorderBy_Click(sender As Object, e As System.EventArgs)
        Dim ascending As Boolean = CBool(DirectCast(sender, LinkButton).CommandArgument)
        Dim orderBy As lm.Comol.Core.BaseModules.Tiles.Domain.OrderTilesBy = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Core.BaseModules.Tiles.Domain.OrderTilesBy).GetByString(DirectCast(sender, LinkButton).CommandName, lm.Comol.Core.BaseModules.Tiles.Domain.OrderTilesBy.Name)

        Dim filter As lm.Comol.Core.BaseModules.Tiles.Domain.dtoFilters = CurrentFilters
        filter.Ascending = ascending
        filter.OrderBy = orderBy
        CurrentFilters = filter
        CTRLmessages.Visible = False
        CurrentPresenter.LoadTiles(filter, IdTilesCommunity, 0, CurrentPageSize)
    End Sub
    Public Function GetOrderByItemCssClass(ByVal item As lm.Comol.Core.Dashboard.Domain.dtoItemFilter(Of lm.Comol.Core.DomainModel.Languages.dtoLanguageItem)) As String
        Dim cssClass As String = GetItemCssClass(item.DisplayAs)
        If item.Selected Then
            cssClass &= " " & LTcssClassActive.Text
        End If
        Return cssClass
    End Function
    Public Function GetItemCssClass(ByVal d As lm.Comol.Core.DomainModel.ItemDisplayOrder) As String
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
    Public Function GetItemCssClass(item As dtoTileItem) As String
        Dim cssClass As String = ""
        'If item.IsDefault Then
        '    cssClass = LTcssClassDefault.Text
        'End If
        If item.Status = lm.Comol.Core.Dashboard.Domain.AvailableStatus.Draft Then
            cssClass &= " " & LTcssClassDraft.Text
        End If
        Return cssClass
    End Function
#End Region



  
End Class