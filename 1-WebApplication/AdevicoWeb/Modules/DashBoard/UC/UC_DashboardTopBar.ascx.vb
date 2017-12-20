Imports lm.Comol.Core.Dashboard.Domain
Imports lm.Comol.Core.BaseModules.Dashboard.Presentation
Public Class UC_DashboardTopBar
    Inherits DBbaseControl
    Implements IViewPortalDashboardTopBar

#Region "Context"
    Private _Presenter As PortalDashboardTopBarPresenter
    Private ReadOnly Property CurrentPresenter() As PortalDashboardTopBarPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New PortalDashboardTopBarPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property SelectedGroupBy As GroupItemsBy Implements IViewPortalDashboardTopBar.SelectedGroupBy
        Get
            Return ViewStateOrDefault("SelectedGroupBy", GroupItemsBy.None)
        End Get
        Set(value As GroupItemsBy)
            ViewState("SelectedGroupBy") = value
        End Set
    End Property

    Private Property SelectedSearch As DisplaySearchItems Implements IViewPortalDashboardTopBar.SelectedSearch
        Get
            Return ViewStateOrDefault("SelectedSearch", DisplaySearchItems.Hide)
        End Get
        Set(value As DisplaySearchItems)
            ViewState("SelectedSearch") = value
        End Set
    End Property
    Private Property CurrentViewType As DashboardViewType Implements IViewPortalDashboardTopBar.CurrentViewType
        Get
            Return ViewStateOrDefault("CurrentViewType", DashboardViewType.List)
        End Get
        Set(value As DashboardViewType)
            ViewState("CurrentViewType") = value
        End Set
    End Property
#End Region

#Region "Internal"
    Public Event GroupByChanged(ByVal gBy As GroupItemsBy)
    Public Event SearchCommunity(ByVal name As String)
    Public Property ShowSurvey As Boolean
        Get
            Return UCdaySurvey.IsActive
        End Get
        Set(value As Boolean)
            UCdaySurvey.IsActive = value
            UCdaySurvey.Visible = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If UCdaySurvey.IsActive AndAlso Not Page.IsPostBack Then
            Me.UCdaySurvey.InitializeControl(0)
        End If
    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLabel(LBviewSelectorDescription)
            .setLabel(LBgroupedSelectorDescription)
            .setButton(BTNsearchByName, True)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitalizeControl(cView As DashboardViewType, settings As liteDashboardSettings, userSettings As UserCurrentSettings, moreTiles As Boolean, Optional ByVal searchBy As String = "") Implements IViewPortalDashboardTopBar.InitalizeControl
        Me.TXBsearchByName.Text = searchBy
        '   CTRLmessage.InitializeControl("", lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
        CurrentPresenter.InitView(cView, settings, userSettings, moreTiles)
    End Sub
    Private Sub InitializeGroupBySelector(items As List(Of dtoItemFilter(Of GroupItemsBy)), selected As GroupItemsBy) Implements IViewPortalDashboardTopBar.InitializeGroupBySelector
        If items.Count > 1 Then
            DVgroupedSelector.Visible = True
            RPTgroupBy.DataSource = items
            RPTgroupBy.DataBind()
        ElseIf items.Any() Then
            DVgroupedSelector.Visible = False
            If IsNothing(selected) Then
                selected = items.FirstOrDefault().Value
            End If
        Else
            DVgroupedSelector.Visible = False
        End If
        SelectedGroupBy = selected
        If selected = GroupItemsBy.None Then
            LBgroupBySelected.Text = ""
        Else
            LBgroupBySelected.Text = Resource.getValue("GroupItemsBy." & selected.ToString)
        End If
    End Sub
    Private Sub InitializeSearch(settings As DisplaySearchItems) Implements IViewPortalDashboardTopBar.InitializeSearch
        DVsearch.Visible = Not settings = DisplaySearchItems.Hide
        SelectedSearch = settings
    End Sub
    Private Sub InitializeViewSelector(items As List(Of dtoItemFilter(Of DashboardViewType))) Implements IViewPortalDashboardTopBar.InitializeViewSelector
        If items.Any() Then
            SPNviewSelector.Visible = True

            HYPgotoCombinedView.Visible = False
            HYPgotoListView.Visible = False
            HYPgotoTileView.Visible = False
            If items.Where(Function(v) v.Selected).Any() Then
                CurrentViewType = items.Where(Function(v) v.Selected).Select(Function(v) v.Value).FirstOrDefault()
            End If
            LBviewSelectorDescription.Visible = items.Count > 0
            For Each i As dtoItemFilter(Of DashboardViewType) In items.Where(Function(v) v.Value <> DashboardViewType.Search AndAlso v.Value <> DashboardViewType.Subscribe)
                Dim oHyperlink As HyperLink
                Select Case i.Value
                    Case DashboardViewType.List
                        oHyperlink = HYPgotoListView
                    Case DashboardViewType.Combined
                        oHyperlink = HYPgotoCombinedView
                    Case DashboardViewType.Tile
                        oHyperlink = HYPgotoTileView

                End Select
                oHyperlink.Visible = True
                oHyperlink.NavigateUrl = BaseUrl & i.Url
                oHyperlink.Text = String.Format(DirectCast(FindControl("LTtemplateview" & i.Value.ToString), Literal).Text, Resource.getValue("view." & i.Value.ToString & ".ToolTip"), Resource.getValue("view." & i.Value.ToString & ".Text"), IIf(i.Selected, LTcssActiveClass.Text, ""))
            Next

        Else
            SPNviewSelector.Visible = False
        End If
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewPortalDashboardTopBar.DisplaySessionTimeout
        BTNsearchByName.Enabled = False
        HYPgotoCombinedView.Enabled = False
        HYPgotoListView.Enabled = False
        HYPgotoTileView.Enabled = False
        RPTgroupBy.Visible = False
    End Sub
    Public Sub DisplayMessage(message As String, type As lm.Comol.Core.DomainModel.Helpers.MessageType) Implements IViewPortalDashboardTopBar.DisplayMessage
        CTRLmessages.Visible = True
        CTRLmessage.Visible = True
        CTRLmessage.InitializeControl(message, type)
    End Sub
    Public Sub HideDisplayMessage() Implements IViewPortalDashboardTopBar.HideDisplayMessage
        CTRLmessages.Visible = False
        CTRLmessage.Visible = False
    End Sub
#End Region

#Region "Internal"
    Public Function GetItemCssClass(ByVal item As dtoItemFilter(Of GroupItemsBy)) As String
        Dim cssClass As String = GetItemCssClass(item.DisplayAs)
        If item.Selected Then
            cssClass &= " " & LTcssActiveClass.Text
        End If
        Return cssClass
    End Function
    Protected Function GetItemCssClass(ByVal d As lm.Comol.Core.DomainModel.ItemDisplayOrder) As String
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
    Private Sub RPTgroupBy_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTgroupBy.ItemDataBound
        Dim oLinkButton As LinkButton = e.Item.FindControl("LNBgroupItemsBy")
        Dim oItem As dtoItemFilter(Of GroupItemsBy) = e.Item.DataItem


        oLinkButton.CommandArgument = CInt(oItem.Value)
        oLinkButton.Text = String.Format(LTgroupItemsByTemplate.Text, Resource.getValue("GroupItemsBy." & oItem.Value.ToString))
        Dim oControl As HtmlControl = e.Item.FindControl("DVitemGroupBy")
        oControl.Attributes("class") = LTcssClassGroupBy.Text & " " & GetItemCssClass(oItem)
    End Sub
    Private Sub RPTgroupBy_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles RPTgroupBy.ItemCommand
        Dim current As GroupItemsBy = SelectedGroupBy
        Dim oControl As HtmlControl = e.Item.FindControl("DVitemGroupBy")
        If Not oControl.Attributes("class").Contains(LTcssClassActive.Text) Then
            oControl.Attributes("class") = oControl.Attributes("class") & " " & LTcssClassActive.Text

            For Each row As RepeaterItem In (From r As RepeaterItem In RPTgroupBy.Items Where r.ItemIndex <> e.Item.ItemIndex)
                oControl = row.FindControl("DVitemGroupBy")
                If oControl.Attributes("class").Contains(LTcssClassActive.Text) Then
                    oControl.Attributes("class") = Replace(oControl.Attributes("class"), LTcssClassActive.Text, "")
                    Exit For
                End If
            Next
        End If
        SelectedGroupBy = CInt(e.CommandArgument)
        LBgroupBySelected.Text = Resource.getValue("GroupItemsBy." & SelectedGroupBy.ToString)

        If Not String.IsNullOrEmpty(HYPgotoCombinedView.NavigateUrl) Then
            HYPgotoCombinedView.NavigateUrl = Replace(HYPgotoCombinedView.NavigateUrl, RootObject.GroupByParameter(current), RootObject.GroupByParameter(CInt(e.CommandArgument)))
        End If
        If Not String.IsNullOrEmpty(HYPgotoListView.NavigateUrl) Then
            HYPgotoListView.NavigateUrl = Replace(HYPgotoListView.NavigateUrl, RootObject.GroupByParameter(current), RootObject.GroupByParameter(CInt(e.CommandArgument)))
        End If
        If Not String.IsNullOrEmpty(HYPgotoTileView.NavigateUrl) Then
            HYPgotoTileView.NavigateUrl = Replace(HYPgotoTileView.NavigateUrl, RootObject.GroupByParameter(current), RootObject.GroupByParameter(CInt(e.CommandArgument)))
        End If
        RaiseEvent GroupByChanged(CInt(e.CommandArgument))
    End Sub

    Private Sub BTNsearchByName_Click(sender As Object, e As EventArgs) Handles BTNsearchByName.Click
        Dim searchBy As String = TXBsearchByName.Text
        If Not String.IsNullOrEmpty(searchBy) Then
            searchBy = searchBy.Trim
        End If
        If CurrentViewType = DashboardViewType.Search Then
            RaiseEvent SearchCommunity(searchBy)
        Else
            If Not String.IsNullOrEmpty(searchBy) AndAlso Not String.IsNullOrEmpty(searchBy.Trim) Then
                searchBy = Server.UrlEncode(searchBy)
            End If
            PageUtility.RedirectToUrl(RootObject.Search(PageUtility.CurrentContext.UserContext.CurrentUserID, SelectedSearch, searchBy))
        End If
    End Sub
    Public Sub SetSearchText(ByVal text As String)
        TXBsearchByName.Text = text
    End Sub
#End Region

   
End Class