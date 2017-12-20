Imports lm.Comol.Core.BaseModules.Dashboard.Presentation
Imports lm.Comol.Core.Dashboard.Domain
Public Class UC_DashboardViews
    Inherits DBbaseControl

#Region "Internal"
    Private Property CurrentType As DashboardType
        Get
            Return ViewStateOrDefault("CurrentType", DashboardType.Portal)
        End Get
        Set(value As DashboardType)
            ViewState("CurrentType") = value
        End Set
    End Property
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLiteral(LTsettingsCommonTitle)
            .setLiteral(LTsettingsCommonDescription)
            .setLabel(LBsettingsFullWidth_t)
            .setLabel(LBsettingsFullWidth)
            .setLabel(LBsettingsNoticeboard_t)
            .setLabel(LBsettingsOnLoadSettings_t)
            .setLabel(LBsettingsDefaultView_t)
            .setLabel(LBsettingsDisplaySearchItems_t)
            .setLabel(LBsettingsAvailableOrderItemsBy_t)
            .setLabel(LBsettingsDefaultOrderItemsBy_t)
            .setLabel(LBsettingsAvailableGroupItemsBy_t)
            .setLabel(LBsettingsDefaultGroupItemsBy_t)

            .setLiteral(LTsettingsListTitle)
            CTRLlistView.TextOn = .getValue("Switch.TextOn")
            CTRLlistView.ToolTipOn = .getValue("Switch.ToolTipOn")
            CTRLlistView.TextOff = .getValue("Switch.TextOff")
            CTRLlistView.ToolTipOff = .getValue("Switch.ToolTipOff")

            .setLabel(LBsettingsListNoticeboard_t)

            .setLabel(LBsettingsListPlainLayout_t)

            .setLabel(LBsettingsListExpandOrganizationList_t)
            .setLabel(LBsettingsListExpandOrganizationList)
            .setLabel(LBsettingsListMaxItems_t)
            .setLabel(LBsettingsListMaxMoreItems_t)

            .setLiteral(LTsettingsCombinedTitle)
            CTRLcombinedView.TextOn = .getValue("Switch.TextOn")
            CTRLcombinedView.ToolTipOn = .getValue("Switch.ToolTipOn")
            CTRLcombinedView.TextOff = .getValue("Switch.TextOff")
            CTRLcombinedView.ToolTipOff = .getValue("Switch.ToolTipOff")
            .setLabel(LBsettingsCombinedNoticeboard_t)
            .setLabel(LBsettingsCombinedTileLayout_t)
            .setLabel(LBsettingsCombinedDisplayMoreItems_t)

            .setLabel(LBsettingsCombinedAutoUpdateLayout_t)
            .setLabel(LBsettingsCombinedAutoUpdateLayout)
            .setLabel(LBcombinedTileDisplayItems_t)
            .setLabel(LBcombinedTileDisplayItems)
            .setLabel(LBcombinedMaxItems_t)
            .setLabel(LBcombinedMaxItems)
            .setLabel(LBcombinedMaxMoreItems_t)
            .setLabel(LBcombinedMaxMoreItems)

            .setLiteral(LTsettingsTileTitle)
            CTRLtileView.TextOn = .getValue("Switch.TextOn")
            CTRLtileView.ToolTipOn = .getValue("Switch.ToolTipOn")
            CTRLtileView.TextOff = .getValue("Switch.TextOff")
            CTRLtileView.ToolTipOff = .getValue("Switch.ToolTipOff")
            .setLabel(LBsettingsTileNoticeboard_t)

            .setLabel(LBsettingsTileLayout_t)

            .setLabel(LBsettingsTileDisplayMoreItems_t)
            '
            .setLabel(LBsettingsTileAutoUpdateLayout_t)
            .setLabel(LBsettingsTileAutoUpdateLayout)
            .setLabel(LBsettingsRedirectTileOn_t)

            .setLabel(LBtileMaxItems_t)
            .setLabel(LBtileMaxItems)
            .setLiteral(LTsettingsSearchTitle)
            'CTRLsearchView.TextOn = .getValue("Switch.TextOn")
            'CTRLsearchView.ToolTipOn = .getValue("Switch.ToolTipOn")
            'CTRLsearchView.TextOff = .getValue("Switch.TextOff")
            'CTRLsearchView.ToolTipOff = .getValue("Switch.ToolTipOff")
            .setLabel(LBsettingsSearchMaxItems_t)
            .setLabel(LBsettingsSearchMaxItems)
            .setLabel(LBsettingsSearchMaxMoreItems_t)
            .setLabel(LBsettingsSearchMaxMoreItems)

            .setLiteral(LTsettingsSubscribeTitle)
            .setLabel(LBsettingsSubscribeMaxItems_t)
            .setLabel(LBsettingsSubscribeMaxItems)

            .setRangeValidator(RNVlistMaxItems)
            .setRangeValidator(RNVlistMaxMoreItems)
            .setCompareValidator(CMVlistMaxMoreItems)

            .setLabel(LBsettingsCombinedPlainLayout_t)
            '<item name="LTsettingsListDescription.DashboardType.Portal">Impostazioni per la vista piana delle comunità.</item>
            '<item name="LTsettingsListDescription.DashboardType.AllCommunities">Impostazioni per la vista piana dei servizi.</item>
            '<item name="LTsettingsListDescription.DashboardType.Community">Impostazioni per la vista piana dei servizi.</item>
            '<item name="LTsettingsTileDescription.DashboardType.Portal">Impostazioni per la vista a tile delle comunità.</item>
            '<item name="LTsettingsTileDescription.DashboardType.AllCommunities">Impostazioni per la vista a tile dei servizi.</item>
            '<item name="LTsettingsTileDescription.DashboardType.Community">Impostazioni per la vista a tile dei servizi.</item>

         
        End With
    End Sub
#End Region
#Region "Internal"
    Public Sub InitializeControl(settings As dtoViewSettings)
        InitializeContainer()
        CurrentType = settings.Type
        DVorganizationList.Visible = (settings.Type = DashboardType.Portal)
        DVsubscribeSettings.Visible = (settings.Type = DashboardType.Portal)
        LBsettingsListMaxItems.Text = Resource.getValue("LBsettingsListMaxItems.DashboardType." & settings.Type.ToString)
        LBsettingsListMaxMoreItems.Text = Resource.getValue("LBsettingsListMaxMoreItems.DashboardType." & settings.Type.ToString)
        CBXfullWidth.Checked = settings.FullWidth
        CTRLlistView.IsOn = (settings.Container.AvailableViews.Contains(DashboardViewType.List) AndAlso settings.Pages.Where(Function(p) p.Type = DashboardViewType.List).Any())
        CTRLtileView.IsOn = (settings.Container.AvailableViews.Contains(DashboardViewType.Tile) AndAlso settings.Pages.Where(Function(p) p.Type = DashboardViewType.Tile).Any())
        Select Case settings.Type
            Case DashboardType.Portal
                DVsearchSettings.Visible = (settings.Container.AvailableViews.Contains(DashboardViewType.Search)) OrElse settings.Container.Default.Search <> DisplaySearchItems.Hide
                CTRLcombinedView.IsOn = (settings.Container.AvailableViews.Contains(DashboardViewType.Combined) AndAlso settings.Pages.Where(Function(p) p.Type = DashboardViewType.Combined).Any())
                MLVcommonSettings.SetActiveView(VIWstandard)
                MLVviews.SetActiveView(VIWportal)
            Case DashboardType.AllCommunities
                MLVviews.SetActiveView(VIWcommunities)
                MLVcommonSettings.SetActiveView(VIWempty)
            Case DashboardType.Community
                MLVviews.SetActiveView(VIWcommunity)
                MLVcommonSettings.SetActiveView(VIWempty)
        End Select
        SetCommonInfo(settings)
        For Each pSettings As dtoPageSettings In settings.Pages
            Select Case pSettings.Type
                Case DashboardViewType.List
                    RBLplainLayout.SelectedValue = CInt(pSettings.PlainLayout)
                    CBXexpandOrganizationList.Checked = pSettings.ExpandOrganizationList
                    If pSettings.MaxItems > 0 Then
                        TXBlistMaxItems.Text = pSettings.MaxItems
                    End If
                    If pSettings.MaxMoreItems > 0 Then
                        TXBlistMaxMoreItems.Text = pSettings.MaxMoreItems
                    End If
                    CTRLlistRangeSettings.InitializeControl(pSettings.Range, settings.Type)
                Case DashboardViewType.Combined
                    RBLcombinedTileLayout.SelectedValue = CInt(pSettings.TileLayout)
                    RBLcombinedPlainLayout.SelectedValue = CInt(pSettings.PlainLayout)
                    '<asp:RadioButtonList ID="RBLcombinedDisplayMoreItems" CssClass="inputgroup" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                    RBLcombinedDisplayMoreItems.SelectedValue = CInt(pSettings.More)
                    CBXcombinedAutoUpdateLayout.Checked = pSettings.AutoUpdateLayout
                    If pSettings.MiniTileDisplayItems > 0 Then
                        TXBcombinedTileDisplayItems.Text = pSettings.MiniTileDisplayItems
                    End If
                    If pSettings.MaxItems > 0 Then
                        TXBcombinedMaxItems.Text = pSettings.MaxItems
                    End If
                    If pSettings.MaxMoreItems > 0 Then
                        TXBcombinedMaxMoreItems.Text = pSettings.MaxMoreItems
                    End If

                Case DashboardViewType.Search
                    CTRLsearchRangeSettings.InitializeControl(pSettings.Range, settings.Type)
                    If pSettings.MaxItems > 0 Then
                        TXBsearchMaxItems.Text = pSettings.MaxItems
                    End If
                    If pSettings.MaxMoreItems > 0 Then
                        TXBsearchMaxMoreItems.Text = pSettings.MaxMoreItems
                    End If
                Case DashboardViewType.Subscribe
                    CTRLsubscribeRangeSettings.InitializeControl(pSettings.Range, settings.Type)
                    If pSettings.MaxItems > 0 Then
                        TXBsubscribeMaxItems.Text = pSettings.MaxItems
                    End If
                Case DashboardViewType.Tile
                    RBLtileDisplayMoreItems.SelectedValue = CInt(pSettings.More)
                    RBLtileLayout.SelectedValue = CInt(pSettings.TileLayout)
                    CBXtileAutoUpdateLayout.Checked = pSettings.AutoUpdateLayout
                    RBLredirectTileOn.SelectedValue = CInt(pSettings.TileRedirectOn)
                    TXBtileMaxItems.Text = pSettings.MaxItems

            End Select
        Next
        SetRadioButtonListItemsCssClass()
        SetCheckBoxLisItemsCssClass()
    End Sub
    Private Sub InitializeContainer()
        Dim items As List(Of lm.Comol.Core.DomainModel.TranslatedItem(Of Integer)) = (From e In [Enum].GetValues(GetType(PlainLayout)).Cast(Of PlainLayout).ToList() Where e <> PlainLayout.ignore Select New lm.Comol.Core.DomainModel.TranslatedItem(Of Integer) With {.Id = CInt(e), .Translation = Resource.getValue("Selector.PlainLayout." & e.ToString)}).ToList()
        RBLplainLayout.DataSource = items
        RBLplainLayout.DataTextField = "Translation"
        RBLplainLayout.DataValueField = "Id"
        RBLplainLayout.DataBind()
        RBLplainLayout.SelectedValue = CInt(PlainLayout.box8box4)

        RBLcombinedPlainLayout.DataSource = items
        RBLcombinedPlainLayout.DataTextField = "Translation"
        RBLcombinedPlainLayout.DataValueField = "Id"
        RBLcombinedPlainLayout.DataBind()
        RBLcombinedPlainLayout.SelectedValue = CInt(PlainLayout.box8box4)

        RBLnoticeboard.DataSource = (From e In [Enum].GetValues(GetType(DisplayNoticeboard)).Cast(Of DisplayNoticeboard).ToList() Where e <> DisplayNoticeboard.InheritsFromDefault Select New lm.Comol.Core.DomainModel.TranslatedItem(Of Integer) With {.Id = CInt(e), .Translation = Resource.getValue("Selector.DisplayNoticeboard." & e.ToString)}).ToList()
        RBLnoticeboard.DataTextField = "Translation"
        RBLnoticeboard.DataValueField = "Id"
        RBLnoticeboard.DataBind()

        items = (From e In [Enum].GetValues(GetType(DisplayNoticeboard)).Cast(Of DisplayNoticeboard).ToList() Where e <> DisplayNoticeboard.DefinedOnAllPages Select New lm.Comol.Core.DomainModel.TranslatedItem(Of Integer) With {.Id = CInt(e), .Translation = Resource.getValue("Selector.DisplayNoticeboard." & e.ToString)}).ToList()

        RBLlistNoticeboard.DataSource = items
        RBLlistNoticeboard.DataTextField = "Translation"
        RBLlistNoticeboard.DataValueField = "Id"
        RBLlistNoticeboard.DataBind()

        RBLcombinedNoticeboard.DataSource = items
        RBLcombinedNoticeboard.DataTextField = "Translation"
        RBLcombinedNoticeboard.DataValueField = "Id"
        RBLcombinedNoticeboard.DataBind()

        RBLtileNoticeboard.DataSource = items
        RBLtileNoticeboard.DataTextField = "Translation"
        RBLtileNoticeboard.DataValueField = "Id"
        RBLtileNoticeboard.DataBind()

        RBLonLoadSettings.DataSource = (From e In [Enum].GetValues(GetType(OnLoadSettings)).Cast(Of OnLoadSettings).ToList() Where e <> OnLoadSettings.UserDefault Select New lm.Comol.Core.DomainModel.TranslatedItem(Of Integer) With {.Id = CInt(e), .Translation = Resource.getValue("Selector.OnLoadSettings." & e.ToString)}).ToList()
        RBLonLoadSettings.DataTextField = "Translation"
        RBLonLoadSettings.DataValueField = "Id"
        RBLonLoadSettings.DataBind()

        RBLdisplaySearchItems.DataSource = (From e In [Enum].GetValues(GetType(DisplaySearchItems)).Cast(Of DisplaySearchItems).ToList() Select New lm.Comol.Core.DomainModel.TranslatedItem(Of Integer) With {.Id = CInt(e), .Translation = Resource.getValue("Selector.DisplaySearchItems." & e.ToString)}).ToList()
        RBLdisplaySearchItems.DataTextField = "Translation"
        RBLdisplaySearchItems.DataValueField = "Id"
        RBLdisplaySearchItems.DataBind()
        RBLcombinedPlainLayout.SelectedValue = CInt(DisplaySearchItems.Hide)

        Dim oItems As List(Of lm.Comol.Core.DomainModel.TranslatedItem(Of Integer)) = (From e In [Enum].GetValues(GetType(OrderItemsBy)).Cast(Of OrderItemsBy).ToList() Select New lm.Comol.Core.DomainModel.TranslatedItem(Of Integer) With {.Id = CInt(e), .Translation = Resource.getValue("Selector.OrderItemsBy." & e.ToString)}).ToList()
        CBLavailableOrderItemsBy.DataSource = oItems
        CBLavailableOrderItemsBy.DataTextField = "Translation"
        CBLavailableOrderItemsBy.DataValueField = "Id"
        CBLavailableOrderItemsBy.DataBind()

        RBLdefaultOrderItemsBy.DataSource = oItems
        RBLdefaultOrderItemsBy.DataTextField = "Translation"
        RBLdefaultOrderItemsBy.DataValueField = "Id"
        RBLdefaultOrderItemsBy.DataBind()
    End Sub
    Public Sub SetCommonInfo(settings As dtoViewSettings)
        CTRLlistRangeSettings.InitializeControl(Nothing, settings.Type)
        CTRLsearchRangeSettings.InitializeControl(Nothing, settings.Type)
        CTRLsubscribeRangeSettings.InitializeControl(Nothing, settings.Type)
        CBXfullWidth.Checked = settings.FullWidth

        RBLnoticeboard.SelectedValue = CInt(settings.Container.Default.DefaultNoticeboard)
        RBLlistNoticeboard.Enabled = (settings.Container.Default.DefaultNoticeboard = DisplayNoticeboard.DefinedOnAllPages)
        RBLcombinedNoticeboard.Enabled = (settings.Container.Default.DefaultNoticeboard = DisplayNoticeboard.DefinedOnAllPages)
        RBLtileNoticeboard.Enabled = (settings.Container.Default.DefaultNoticeboard = DisplayNoticeboard.DefinedOnAllPages)
        If settings.Container.Default.DefaultNoticeboard = DisplayNoticeboard.DefinedOnAllPages Then
            RBLlistNoticeboard.SelectedValue = CInt(settings.Container.Default.ListNoticeboard)
            RBLcombinedNoticeboard.SelectedValue = CInt(settings.Container.Default.CombinedNoticeboard)
            RBLtileNoticeboard.SelectedValue = CInt(settings.Container.Default.TileNoticeboard)
            SetListItemAvailability(False, CInt(DisplayNoticeboard.InheritsFromDefault), RBLlistNoticeboard)
            SetListItemAvailability(False, CInt(DisplayNoticeboard.InheritsFromDefault), RBLcombinedNoticeboard)
            SetListItemAvailability(False, CInt(DisplayNoticeboard.InheritsFromDefault), RBLtileNoticeboard)
        Else
            RBLlistNoticeboard.SelectedValue = CInt(DisplayNoticeboard.InheritsFromDefault)
            RBLcombinedNoticeboard.SelectedValue = CInt(DisplayNoticeboard.InheritsFromDefault)
            RBLtileNoticeboard.SelectedValue = CInt(DisplayNoticeboard.InheritsFromDefault)
        End If


        RBLonLoadSettings.SelectedValue = CInt(settings.Container.Default.AfterUserLogon)
        RBLdisplaySearchItems.SelectedValue = CInt(settings.Container.Default.Search)

        Dim items As List(Of lm.Comol.Core.DomainModel.TranslatedItem(Of Integer)) = (From e In [Enum].GetValues(GetType(GroupItemsBy)).Cast(Of GroupItemsBy).ToList() Where e <> GroupItemsBy.None AndAlso ((settings.Type = DashboardType.Portal AndAlso e <> GroupItemsBy.Service) OrElse (settings.Type <> DashboardType.Portal AndAlso e <> GroupItemsBy.CommunityType AndAlso e <> GroupItemsBy.Tag)) Select New lm.Comol.Core.DomainModel.TranslatedItem(Of Integer) With {.Id = CInt(e), .Translation = Resource.getValue("Selector.GroupItemsBy." & e.ToString)}).ToList()
        RBLdefaultGroupItemsBy.DataSource = items
        RBLdefaultGroupItemsBy.DataTextField = "Translation"
        RBLdefaultGroupItemsBy.DataValueField = "Id"
        RBLdefaultGroupItemsBy.DataBind()

        CBLavailableGroupItemsBy.DataSource = items.Where(Function(i) i.Id <> CInt(GroupItemsBy.None)).ToList()
        CBLavailableGroupItemsBy.DataTextField = "Translation"
        CBLavailableGroupItemsBy.DataValueField = "Id"
        CBLavailableGroupItemsBy.DataBind()
        For Each i As GroupItemsBy In settings.Container.AvailableGroupBy
            Dim gItem As ListItem = CBLavailableGroupItemsBy.Items.FindByValue(CInt(i))
            If Not IsNothing(gItem) Then
                gItem.Selected = True
            End If
        Next

        If IsNothing(RBLdefaultGroupItemsBy.Items.FindByValue(CInt(settings.Container.Default.GroupBy))) Then
            RBLdefaultGroupItemsBy.SelectedValue = CInt(GroupItemsBy.Tile)
        Else
            RBLdefaultGroupItemsBy.SelectedValue = CInt(settings.Container.Default.GroupBy)
        End If
        RBLdefaultOrderItemsBy.SelectedValue = settings.Container.Default.OrderBy

        For Each i As OrderItemsBy In settings.Container.AvailableOrderBy
            CBLavailableOrderItemsBy.Items.FindByValue(CInt(i)).Selected = True
        Next

        items = (From e In [Enum].GetValues(GetType(DashboardViewType)).Cast(Of DashboardViewType).ToList() Where e <> DashboardViewType.Subscribe Select New lm.Comol.Core.DomainModel.TranslatedItem(Of Integer) With {.Id = CInt(e), .Translation = Resource.getValue("Selector.DashboardViewType." & e.ToString)}).ToList()
        If settings.Type <> DashboardType.Portal Then
            items = items.Where(Function(i) i.Id <> CInt(DashboardViewType.Combined) AndAlso i.Id <> CInt(DashboardViewType.Search)).ToList
        End If

        RBLdefaultView.DataSource = items
        RBLdefaultView.DataTextField = "Translation"
        RBLdefaultView.DataValueField = "Id"
        RBLdefaultView.DataBind()
        If IsNothing(RBLdefaultView.Items.FindByValue(CInt(settings.Container.Default.View))) Then
            RBLdefaultView.SelectedIndex = 0
        Else
            RBLdefaultView.SelectedValue = CInt(settings.Container.Default.View)
        End If

        items = (From e In [Enum].GetValues(GetType(TileLayout)).Cast(Of TileLayout).ToList() Where e <> TileLayout.grid_0 AndAlso e <> TileLayout.grid_12 AndAlso e <> TileLayout.grid_1 AndAlso e <> TileLayout.grid_2 Select New lm.Comol.Core.DomainModel.TranslatedItem(Of Integer) With {.Id = CInt(e), .Translation = Resource.getValue("Selector.TileLayout." & e.ToString)}).ToList()
        RBLcombinedTileLayout.DataSource = items.Where(Function(i) i.Id <> CInt(TileLayout.grid_1))
        RBLcombinedTileLayout.DataTextField = "Translation"
        RBLcombinedTileLayout.DataValueField = "Id"
        RBLcombinedTileLayout.DataBind()
        RBLcombinedTileLayout.SelectedValue = CInt(TileLayout.grid_4)

        RBLtileLayout.DataSource = items
        RBLtileLayout.DataTextField = "Translation"
        RBLtileLayout.DataValueField = "Id"
        RBLtileLayout.DataBind()
        RBLtileLayout.SelectedValue = CInt(TileLayout.grid_4)

        items = (From e In [Enum].GetValues(GetType(DisplayMoreItems)).Cast(Of DisplayMoreItems).ToList() Select New lm.Comol.Core.DomainModel.TranslatedItem(Of Integer) With {.Id = CInt(e), .Translation = Resource.getValue("Selector.DisplayMoreItems." & e.ToString)}).ToList()
        RBLcombinedDisplayMoreItems.DataSource = items
        RBLcombinedDisplayMoreItems.DataTextField = "Translation"
        RBLcombinedDisplayMoreItems.DataValueField = "Id"
        RBLcombinedDisplayMoreItems.DataBind()
        RBLcombinedDisplayMoreItems.SelectedValue = CInt(DisplayMoreItems.AsLink)

        RBLtileDisplayMoreItems.DataSource = items
        RBLtileDisplayMoreItems.DataTextField = "Translation"
        RBLtileDisplayMoreItems.DataValueField = "Id"
        RBLtileDisplayMoreItems.DataBind()
        RBLtileDisplayMoreItems.SelectedValue = CInt(DisplayMoreItems.AsLink)

        RBLredirectTileOn.DataSource = (From e In [Enum].GetValues(GetType(DashboardViewType)).Cast(Of DashboardViewType).ToList() Where e <> DashboardViewType.Tile AndAlso e <> DashboardViewType.List AndAlso e <> DashboardViewType.Subscribe Select New lm.Comol.Core.DomainModel.TranslatedItem(Of Integer) With {.Id = CInt(e), .Translation = Resource.getValue("Selector.redirectTileOn.DashboardViewType." & e.ToString)}).ToList()
        RBLredirectTileOn.DataTextField = "Translation"
        RBLredirectTileOn.DataValueField = "Id"
        RBLredirectTileOn.DataBind()
        RBLredirectTileOn.SelectedValue = CInt(DashboardViewType.Combined)

    End Sub
    Private Function SetListItemAvailability(enabled As Boolean, ByVal value As String, ByVal control As RadioButtonList)
        Dim item As ListItem = control.Items.FindByValue(value)
        If Not IsNothing(item) Then
            item.Enabled = enabled
        End If
    End Function
    Public Function GetSettingsCss(name As String) As String
        Select Case name
            Case "searchviewsettings"
                Select Case CurrentType
                    Case DashboardType.Portal
                        Return LTcssClassLastSettings.Text
                    Case Else
                        Return ""
                End Select
            Case "tileviewsettings"
                Select Case CurrentType
                    Case DashboardType.Portal
                        Return ""
                    Case Else
                        Return LTcssClassLastSettings.Text
                End Select
            Case Else
                Return ""
        End Select
    End Function
    Public Function GetSettings() As dtoViewSettings
        Dim settings As New dtoViewSettings
        settings.Type = CurrentType
        settings.FullWidth = CBXfullWidth.Checked
        settings.Container = New dtoContainerSettings()

        settings.Container.Default.GroupBy = CInt(RBLdefaultGroupItemsBy.SelectedValue)
        settings.Container.Default.DefaultNoticeboard = CInt(RBLnoticeboard.SelectedValue)
        settings.Container.Default.OrderBy = CInt(RBLdefaultOrderItemsBy.SelectedValue)
        settings.Container.Default.Search = CInt(RBLdisplaySearchItems.SelectedValue)
        settings.Container.Default.AfterUserLogon = CInt(RBLonLoadSettings.SelectedValue)
        settings.Container.Default.View = CInt(RBLdefaultView.SelectedValue)

        If CTRLlistView.IsOn Then
            settings.Container.AvailableViews.Add(lm.Comol.Core.Dashboard.Domain.DashboardViewType.List)
        End If
        If CTRLcombinedView.IsOn Then
            settings.Container.AvailableViews.Add(lm.Comol.Core.Dashboard.Domain.DashboardViewType.Combined)
        End If
        If CTRLtileView.IsOn Then
            settings.Container.AvailableViews.Add(lm.Comol.Core.Dashboard.Domain.DashboardViewType.Tile)
        End If
        If RBLdisplaySearchItems.SelectedValue <> CInt(DisplaySearchItems.Hide) Then
            settings.Container.AvailableViews.Add(lm.Comol.Core.Dashboard.Domain.DashboardViewType.Search)
        End If
        If CurrentType = DashboardType.Portal Then
            settings.Container.AvailableViews.Add(lm.Comol.Core.Dashboard.Domain.DashboardViewType.Subscribe)
        End If

        settings.Container.AvailableGroupBy = (From i As ListItem In CBLavailableGroupItemsBy.Items Where i.Selected Select DirectCast(CInt(i.Value), lm.Comol.Core.Dashboard.Domain.GroupItemsBy)).ToList()
        settings.Container.AvailableOrderBy = (From i As ListItem In CBLavailableOrderItemsBy.Items Where i.Selected Select DirectCast(CInt(i.Value), lm.Comol.Core.Dashboard.Domain.OrderItemsBy)).ToList()


        If Not (settings.Container.AvailableOrderBy.Contains(settings.Container.Default.OrderBy)) Then
            settings.Container.AvailableOrderBy.Add(settings.Container.Default.OrderBy)
        End If
        If Not (settings.Container.AvailableGroupBy.Contains(settings.Container.Default.GroupBy)) Then
            settings.Container.AvailableGroupBy.Add(settings.Container.Default.GroupBy)
        End If

        For Each vType As lm.Comol.Core.Dashboard.Domain.DashboardViewType In settings.Container.AvailableViews
            Dim pg As lm.Comol.Core.Dashboard.Domain.dtoPageSettings = New lm.Comol.Core.Dashboard.Domain.dtoPageSettings
            pg.Type = vType
            Select Case vType
                Case lm.Comol.Core.Dashboard.Domain.DashboardViewType.List
                    pg.PlainLayout = CInt(RBLplainLayout.SelectedValue)
                    pg.ExpandOrganizationList = CBXexpandOrganizationList.Checked
                    If settings.Container.Default.DefaultNoticeboard = lm.Comol.Core.Dashboard.Domain.DisplayNoticeboard.DefinedOnAllPages Then
                        pg.Noticeboard = CInt(RBLlistNoticeboard.SelectedValue)
                    Else
                        pg.Noticeboard = lm.Comol.Core.Dashboard.Domain.DisplayNoticeboard.InheritsFromDefault
                    End If
                    settings.Container.Default.ListNoticeboard = pg.Noticeboard
                    If IsNumeric(TXBlistMaxItems.Text) Then
                        pg.MaxItems = CInt(TXBlistMaxItems.Text)
                    ElseIf pg.MaxItems = 0 Then
                        pg.MaxItems = 15
                        TXBlistMaxItems.Text = 15
                    End If
                    If IsNumeric(TXBlistMaxMoreItems.Text) Then
                        pg.MaxMoreItems = CInt(TXBlistMaxMoreItems.Text)
                    ElseIf pg.MaxMoreItems = 0 Then
                        pg.MaxMoreItems = 25
                        TXBlistMaxMoreItems.Text = 25
                    End If
                    pg.More = lm.Comol.Core.Dashboard.Domain.DisplayMoreItems.AsLink
                    pg.Range = CTRLlistRangeSettings.GetRange

                Case lm.Comol.Core.Dashboard.Domain.DashboardViewType.Combined
                    pg.TileLayout = CInt(RBLcombinedTileLayout.SelectedValue)
                    pg.PlainLayout = CInt(RBLcombinedPlainLayout.SelectedValue)
                    pg.More = CInt(RBLcombinedDisplayMoreItems.SelectedValue)
                    pg.AutoUpdateLayout = CBXcombinedAutoUpdateLayout.Checked
                    If settings.Container.Default.DefaultNoticeboard = lm.Comol.Core.Dashboard.Domain.DisplayNoticeboard.DefinedOnAllPages Then
                        pg.Noticeboard = CInt(RBLcombinedNoticeboard.SelectedValue)
                    Else
                        pg.Noticeboard = lm.Comol.Core.Dashboard.Domain.DisplayNoticeboard.InheritsFromDefault
                    End If
                    settings.Container.Default.CombinedNoticeboard = pg.Noticeboard
                    If IsNumeric(TXBcombinedTileDisplayItems.Text) Then
                        pg.MiniTileDisplayItems = CInt(TXBcombinedTileDisplayItems.Text)
                    ElseIf pg.MaxItems = 0 Then
                        pg.MiniTileDisplayItems = 15
                        TXBcombinedTileDisplayItems.Text = 15
                    End If
                    If IsNumeric(TXBcombinedMaxItems.Text) Then
                        pg.MaxItems = CInt(TXBcombinedMaxItems.Text)
                    ElseIf pg.MaxMoreItems = 0 Then
                        pg.MaxItems = 15
                        TXBcombinedMaxItems.Text = 15
                    End If
                    If IsNumeric(TXBcombinedMaxItems.Text) Then
                        pg.MaxMoreItems = CInt(TXBcombinedMaxMoreItems.Text)
                    ElseIf pg.MaxMoreItems = 0 Then
                        pg.MaxMoreItems = 25
                        TXBcombinedMaxMoreItems.Text = 25
                    End If
                    pg.TileRedirectOn = DashboardViewType.Combined
                Case lm.Comol.Core.Dashboard.Domain.DashboardViewType.Tile
                    If settings.Container.Default.DefaultNoticeboard = lm.Comol.Core.Dashboard.Domain.DisplayNoticeboard.DefinedOnAllPages Then
                        pg.Noticeboard = CInt(RBLtileNoticeboard.SelectedValue)
                    Else
                        pg.Noticeboard = lm.Comol.Core.Dashboard.Domain.DisplayNoticeboard.InheritsFromDefault
                    End If
                    settings.Container.Default.TileNoticeboard = pg.Noticeboard
                    pg.TileLayout = CInt(RBLtileLayout.SelectedValue)
                    pg.More = CInt(RBLtileDisplayMoreItems.SelectedValue)
                    pg.AutoUpdateLayout = CBXtileAutoUpdateLayout.Checked
                    pg.TileRedirectOn = CInt(RBLredirectTileOn.SelectedValue)
                    If IsNumeric(TXBtileMaxItems.Text) Then
                        pg.MaxItems = CInt(TXBtileMaxItems.Text)
                    ElseIf pg.MaxItems = 0 Then
                        pg.MaxItems = 15
                        TXBtileMaxItems.Text = 15
                    End If
                    pg.PlainLayout = PlainLayout.full
                Case lm.Comol.Core.Dashboard.Domain.DashboardViewType.Search
                    pg.AutoUpdateLayout = True
                    If IsNumeric(TXBsearchMaxItems.Text) Then
                        pg.MaxItems = CInt(TXBsearchMaxItems.Text)
                    ElseIf pg.MaxItems = 0 Then
                        pg.MaxItems = 15
                        TXBsearchMaxItems.Text = 15
                    End If
                    If IsNumeric(TXBsearchMaxMoreItems.Text) Then
                        pg.MaxMoreItems = CInt(TXBsearchMaxMoreItems.Text)
                    ElseIf pg.MaxMoreItems = 0 Then
                        pg.MaxMoreItems = 25
                        TXBsearchMaxMoreItems.Text = 25
                    End If
                    pg.More = DisplayMoreItems.AsLink
                    pg.Noticeboard = DisplayNoticeboard.Hide
                    pg.PlainLayout = PlainLayout.full
                    pg.Range = CTRLsearchRangeSettings.GetRange
                    pg.TileLayout = TileLayout.grid_12
                    pg.TileRedirectOn = DashboardViewType.Search
                Case lm.Comol.Core.Dashboard.Domain.DashboardViewType.Subscribe
                    pg.AutoUpdateLayout = True
                    If IsNumeric(TXBsubscribeMaxItems.Text) Then
                        pg.MaxItems = CInt(TXBsubscribeMaxItems.Text)
                    ElseIf pg.MaxItems = 0 Then
                        pg.MaxItems = 20
                        TXBsubscribeMaxItems.Text = 20
                    End If
                    pg.MaxMoreItems = pg.MaxItems

                    pg.More = DisplayMoreItems.AsLink
                    pg.Noticeboard = DisplayNoticeboard.Hide
                    pg.PlainLayout = PlainLayout.full
                    pg.Range = CTRLsubscribeRangeSettings.GetRange
                    pg.TileLayout = TileLayout.grid_12
                    pg.TileRedirectOn = DashboardViewType.Search
            End Select
            settings.Pages.Add(pg)
        Next

        Return settings
    End Function
    Private Sub SetRadioButtonListItemsCssClass()
        SetRadioButtonListItemsCssClass(RBLcombinedDisplayMoreItems)
        SetRadioButtonListItemsCssClass(RBLcombinedNoticeboard)
        SetRadioButtonListItemsCssClass(RBLcombinedPlainLayout)
        SetRadioButtonListItemsCssClass(RBLcombinedTileLayout)
        SetRadioButtonListItemsCssClass(RBLdefaultGroupItemsBy)
        SetRadioButtonListItemsCssClass(RBLdefaultOrderItemsBy)
        SetRadioButtonListItemsCssClass(RBLdefaultView)
        SetRadioButtonListItemsCssClass(RBLdisplaySearchItems)
        SetRadioButtonListItemsCssClass(RBLlistNoticeboard)
        SetRadioButtonListItemsCssClass(RBLnoticeboard)
        SetRadioButtonListItemsCssClass(RBLonLoadSettings)
        SetRadioButtonListItemsCssClass(RBLplainLayout)
        SetRadioButtonListItemsCssClass(RBLredirectTileOn)
        SetRadioButtonListItemsCssClass(RBLtileDisplayMoreItems)
        SetRadioButtonListItemsCssClass(RBLtileLayout)
        SetRadioButtonListItemsCssClass(RBLtileNoticeboard)
    End Sub
    Private Sub SetRadioButtonListItemsCssClass(ByVal oRadioButtonList As RadioButtonList)
        For Each item As ListItem In oRadioButtonList.Items
            item.Attributes.Add("class", "item")
        Next
    End Sub
    Private Sub SetCheckBoxLisItemsCssClass()
        SetCheckBoxListItemsCssClass(CBLavailableGroupItemsBy)
        SetCheckBoxListItemsCssClass(CBLavailableOrderItemsBy)
    End Sub
    Private Sub SetCheckBoxListItemsCssClass(ByVal obj As CheckBoxList)
        For Each item As ListItem In obj.Items
            item.Attributes.Add("class", "item")
        Next
    End Sub
#End Region

    Private Sub RBLnoticeboard_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RBLnoticeboard.SelectedIndexChanged
        Select Case CInt(RBLnoticeboard.SelectedValue)
            Case lm.Comol.Core.Dashboard.Domain.DisplayNoticeboard.DefinedOnAllPages
                RBLlistNoticeboard.Enabled = True
                RBLcombinedNoticeboard.Enabled = True
                RBLtileNoticeboard.Enabled = True
                RBLlistNoticeboard.SelectedValue = CInt(lm.Comol.Core.Dashboard.Domain.DisplayNoticeboard.OnRight)
                RBLcombinedNoticeboard.SelectedValue = CInt(lm.Comol.Core.Dashboard.Domain.DisplayNoticeboard.OnRight)
                RBLtileNoticeboard.SelectedValue = CInt(lm.Comol.Core.Dashboard.Domain.DisplayNoticeboard.OnRight)
                SetListItemAvailability(False, CInt(DisplayNoticeboard.InheritsFromDefault), RBLlistNoticeboard)
                SetListItemAvailability(False, CInt(DisplayNoticeboard.InheritsFromDefault), RBLcombinedNoticeboard)
                SetListItemAvailability(False, CInt(DisplayNoticeboard.InheritsFromDefault), RBLtileNoticeboard)
            Case Else
                RBLlistNoticeboard.Enabled = False
                RBLcombinedNoticeboard.Enabled = False
                RBLtileNoticeboard.Enabled = False
                RBLlistNoticeboard.SelectedValue = CInt(lm.Comol.Core.Dashboard.Domain.DisplayNoticeboard.InheritsFromDefault)
                RBLcombinedNoticeboard.SelectedValue = CInt(lm.Comol.Core.Dashboard.Domain.DisplayNoticeboard.InheritsFromDefault)
                RBLtileNoticeboard.SelectedValue = CInt(lm.Comol.Core.Dashboard.Domain.DisplayNoticeboard.InheritsFromDefault)
        End Select
    End Sub
End Class