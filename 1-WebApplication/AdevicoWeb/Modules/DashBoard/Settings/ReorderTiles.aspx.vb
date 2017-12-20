
Imports lm.ActionDataContract
Imports lm.Comol.Core.BaseModules.Dashboard.Presentation
Imports lm.Comol.Core.Dashboard.Domain
Public Class ReorderTiles
    Inherits DBbaseEditSettings
    Implements IViewEditTilesOrder


#Region "Context"
    Private _presenter As EditTilesOrderPresenter
    Protected Friend ReadOnly Property CurrentPresenter As EditTilesOrderPresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New EditTilesOrderPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _presenter
        End Get
    End Property
#End Region

#Region "Implements"

#Region "Preload"
    Protected Friend ReadOnly Property PreloadStep As WizardDashboardStep Implements IViewEditTilesOrder.PreloadStep
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of WizardDashboardStep).GetByString(Request.QueryString("step"), WizardDashboardStep.None)
        End Get
    End Property
#End Region

#Region "Settings"
    Protected Friend Property CurrentStep As WizardDashboardStep Implements IViewEditTilesOrder.CurrentStep
        Get
            Return ViewStateOrDefault("CurrentStep", WizardDashboardStep.None)
        End Get
        Set(ByVal value As WizardDashboardStep)
            Me.ViewState("CurrentStep") = value
            Master.ServiceTitle = Resource.getValue("DashboardSettings.ServiceTitle.EditTilesOrder.WizardDashboardStep." & value.ToString)
            Master.ServiceNopermission = Resource.getValue("DashboardSettings.ServiceTitle.EditTilesOrder.NoPermission.WizardDashboardStep." & value.ToString)
        End Set
    End Property
   
#End Region

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        CurrentPresenter.InitView(PreloadStep, PreloadIdDashboard, PreloadDashboardType, PreloadIdCommunity)
    End Sub
    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            Master.ServiceTitle = .getValue("DashboardSettings.ServiceTitle.EditTilesOrder")
            Master.ServiceNopermission = Resource.getValue("DashboardSettings.ServiceTitle.EditTilesOrder.NoPermission")
            .setHyperLink(HYPbackToDashboardListBottom, False, True)
            .setHyperLink(HYPbackToDashboardListTop, False, True)
            .setButton(BTNsaveDashboardTilesOrderTop, True)
            .setButton(BTNsaveDashboardTilesOrderBottom, True)
            .setHyperLink(HYPpreviewDashboardTop, False, True)
            .setHyperLink(HYPpreviewDashboardBottom, False, True)

            .setButton(BTNtilesOrderGenerateCommunityTileTop, True)
            .setButton(BTNtilesOrderGenerateCommunityTileBottom, True)
            .setHyperLink(HYPgotoManageTilesTop, False, True)
            .setHyperLink(HYPgotoManageTilesBottom, False, True)
            .setLabel(LBreorderTileName_t)
            .setLabel(LBreorderTileVisibility_t)
        End With
    End Sub
    Protected Friend Overrides Sub DisplaySessionTimeout()
        RedirectOnSessionTimeOut(lm.Comol.Core.Dashboard.Domain.RootObject.DashboardTileReorder(PreloadStep, PreloadIdDashboard, PreloadDashboardType), IdContainerCommunity)
    End Sub

    Protected Friend Overrides Function GetBackUrlItem(Optional top As Boolean = True) As HyperLink
        Return IIf(top, HYPbackToDashboardListTop, HYPbackToDashboardListBottom)
    End Function
    Protected Friend Overrides Function GetMessageControl() As UC_ActionMessages
        Return CTRLmessages
    End Function
    Protected Friend Overrides Function GetPreviewUrlItem(Optional top As Boolean = True) As HyperLink
        Return IIf(top, HYPpreviewDashboardTop, HYPpreviewDashboardBottom)
    End Function
    Protected Friend Overrides Function GetSaveButton(Optional top As Boolean = True) As Button
        Return IIf(top, BTNsaveDashboardTilesOrderTop, BTNsaveDashboardTilesOrderBottom)
    End Function
    Protected Friend Overrides Function GetStepsControl() As UC_GenericWizardSteps
        Return CTRLsteps
    End Function
#End Region

#Region "Implements"
    Private Sub DisplayNoTiles() Implements IViewEditTilesOrder.DisplayNoTiles
        RPTtiles.Visible = False
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayNoTiles"), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub
    Private Sub InitializeView(wStep As WizardDashboardStep, url As String, Optional allowGenerate As Boolean = False) Implements IViewEditTilesOrder.InitializeView
        Master.ServiceTitle = Resource.getValue("DashboardSettings.ServiceTitle.EditTilesOrder.WizardDashboardStep." & wStep.ToString)
        Master.ServiceNopermission = Resource.getValue("DashboardSettings.ServiceTitle.EditTilesOrder.NoPermission.WizardDashboardStep." & wStep.ToString)
        LTreorderTileTitle.Text = Resource.getValue("LTreorderTileTitle.WizardDashboardStep." & wStep.ToString)
        LTreorderTileDescription.Text = Resource.getValue("LTreorderTileDescription.WizardDashboardStep." & wStep.ToString)

        If Not String.IsNullOrEmpty(url) Then
            HYPgotoManageTilesTop.Visible = True
            HYPgotoManageTilesBottom.Visible = True
            HYPgotoManageTilesTop.NavigateUrl = ApplicationUrlBase() & url
            HYPgotoManageTilesBottom.NavigateUrl = ApplicationUrlBase() & url
        Else
            HYPgotoManageTilesTop.Visible = False
            HYPgotoManageTilesBottom.Visible = False
        End If
        BTNtilesOrderGenerateCommunityTileBottom.Visible = allowGenerate
        BTNtilesOrderGenerateCommunityTileTop.Visible = allowGenerate
    End Sub

    Private Sub LoadTiles(tiles As List(Of dtoTileForReorder)) Implements IViewEditTilesOrder.LoadTiles
        RPTtiles.Visible = True
        RPTtiles.DataSource = tiles
        RPTtiles.DataBind()
    End Sub
    Private Function GetTiles() As List(Of dtoTileForReorder) Implements IViewEditTilesOrder.GetTiles
        Dim tiles As New List(Of dtoTileForReorder)
        Dim orderItems As List(Of Long) = Reorder()
        Dim index As Long = 1

        For Each row As RepeaterItem In RPTtiles.Items
            Dim oLiteral As Literal = row.FindControl("LTidTile")
            Dim tile As New dtoTileForReorder
            Dim oLabel As Label = row.FindControl("LBname")
            tile.IdTile = CLng(oLiteral.Text)
            tile.Name = oLabel.Text
            oLiteral = row.FindControl("LTidAssignment")
            tile.IdAssignment = CLng(oLiteral.Text)

            Dim oCheck As HtmlInputCheckBox = row.FindControl("CBXvisibility")
            If (oCheck.Disabled) Then
                tile.TileStatus = AvailableStatus.Unavailable
            Else
                tile.TileStatus = AvailableStatus.Available
            End If
            tile.AssignmentStatus = IIf(oCheck.Checked, AvailableStatus.Available, AvailableStatus.Unavailable)
            If orderItems.Any Then
                tile.DisplayOrder = orderItems.IndexOf(tile.IdTile) + 1
            Else
                tile.DisplayOrder = index
                index += 1
            End If
            tiles.Add(tile)
        Next

        Return tiles.OrderBy(Function(t) t.DisplayOrder).ToList()
    End Function

    Private Function Reorder() As List(Of Long)
        Dim items As New List(Of Long)
        If Not String.IsNullOrEmpty(HDMserializeTiles.Value) Then
            For Each item As String In HDMserializeTiles.Value.Split("&")
                Dim values As String()
                values = item.Split("=")
                items.Add(Long.Parse(values(1)))
            Next
        End If
        'srt[]=2&srt[]=1&srt[]=3&srt[]=4&srt[]=5
        Return items
    End Function
#End Region

#Region "internal"
    Private Sub DashboardList_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Master.ShowDocType = True
    End Sub
    Private Sub RPTtiles_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTtiles.ItemDataBound
        Dim oItem As dtoTileForReorder = e.Item.DataItem
        Dim oCheck As HtmlInputCheckBox = e.Item.FindControl("CBXvisibility")
        oCheck.Checked = (oItem.AssignmentStatus = AvailableStatus.Available) AndAlso oItem.Deleted = lm.Comol.Core.DomainModel.BaseStatusDeleted.None
        oCheck.Disabled = (oItem.TileStatus <> AvailableStatus.Available)
        Dim oLabel As Label = e.Item.FindControl("LBname")
        oLabel.ToolTip = Resource.getValue("Assignment.AvailableStatus." & oItem.AssignmentStatus.ToString & ".tile.AvailableStatus." & oItem.TileStatus.ToString)
    End Sub

    Private Sub BTNsaveDashboardTilesOrderTop_Click(sender As Object, e As EventArgs) Handles BTNsaveDashboardTilesOrderTop.Click, BTNsaveDashboardTilesOrderBottom.Click
        CurrentPresenter.SaveOrder(CurrentStep, IdDashboard, IdContainerCommunity, GetTiles())
    End Sub
    Private Sub BTNtilesOrderGenerateCommunityTileBottom_Click(sender As Object, e As EventArgs) Handles BTNtilesOrderGenerateCommunityTileBottom.Click, BTNtilesOrderGenerateCommunityTileTop.Click
        CurrentPresenter.GenerateCommunityTypeTiles(CurrentStep, IdDashboard, IdContainerCommunity)
    End Sub
    Public Function GetCssClass(item As dtoTileForReorder) As String
        Dim cssClass = item.Type.ToString.ToLower()
        If item.TileStatus <> AvailableStatus.Available Then
            cssClass &= " " & LTcssClassDisabled.Text
        End If
        Return cssClass
    End Function
#End Region


   
End Class