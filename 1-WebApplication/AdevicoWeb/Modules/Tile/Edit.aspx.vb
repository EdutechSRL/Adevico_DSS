Public Class EditTile
    Inherits TLeditPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        CurrentPresenter.InitView(False, PreloadIdTile, PreloadDashboardType, PreloadIdCommunity)
    End Sub

    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True

    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            .setHyperLink(HYPgoTo_TileListTop, False, True)
            .setHyperLink(HYPgoTo_TileListBottom, False, True)
            .setLinkButton(LNBsaveTileTop, False, True)
            .setLinkButton(LNBsaveTileBottom, False, True)
            .setLinkButton(LNBenableTileTop, False, True)
            .setLinkButton(LNBenableTileBottom, False, True)
            .setLinkButton(LNBdisableTileTop, False, True)
            .setLinkButton(LNBdisableTileBottom, False, True)
            .setLinkButton(LNBvirtualDeleteTileTop, False, True)
            .setLinkButton(LNBvirtualDeleteTileBottom, False, True)
            .setLinkButton(LNBvirtualunDeleteTileTop, False, True)
            .setLinkButton(LNBvirtualunDeleteTileBottom, False, True)
            Master.ServiceTitle = .getValue("Tile.EditTile")
            Master.ServiceNopermission = .getValue("Tile.EditTileNoPermission")
        End With
    End Sub
    Protected Friend Overrides Sub DisplaySessionTimeout()
        RedirectOnSessionTimeOut(lm.Comol.Core.Dashboard.Domain.RootObject.TileEdit(PreloadIdTile, PreloadDashboardType, PreloadIdCommunity, False, PreloadIdDashboard, PreloadStep), IdContainerCommunity)
    End Sub

    Protected Friend Overrides Function GetBackUrlItemBottom() As HyperLink
        Return HYPgoTo_TileListBottom
    End Function
    Protected Friend Overrides Function GetBackUrlItemTop() As HyperLink
        Return HYPgoTo_TileListTop
    End Function
    Protected Friend Overrides Function GetSaveButton(Optional ByVal top As Boolean = True) As LinkButton
        Return IIf(top, LNBsaveTileTop, LNBsaveTileBottom)
    End Function
    Protected Friend Overrides Function GetEnableButton(Optional ByVal top As Boolean = True) As LinkButton
        Return IIf(top, LNBenableTileTop, LNBenableTileBottom)
    End Function
    Protected Friend Overrides Function GetDisableButton(Optional ByVal top As Boolean = True) As LinkButton
        Return IIf(top, LNBdisableTileTop, LNBdisableTileBottom)
    End Function
    Protected Friend Overrides Function GetVirtualDeleteButton(Optional ByVal top As Boolean = True) As LinkButton
        Return IIf(top, LNBvirtualDeleteTileTop, LNBvirtualDeleteTileBottom)
    End Function
    Protected Friend Overrides Function GetVirtualUndeleteButton(Optional ByVal top As Boolean = True) As LinkButton
        Return IIf(top, LNBvirtualUndeleteTileTop, LNBvirtualUndeleteTileBottom)
    End Function
    Protected Friend Overrides Function GetTileControl() As UC_EditTile
        Return CTRLedit
    End Function
 
    Protected Friend Overrides Function GetMessageControl() As UC_ActionMessages
        Return CTRLmessages
    End Function
#End Region

#Region "Internal"
    Private Sub AddTile_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Master.ShowDocType = True
        CTRLheader.InitializeControl(New List(Of String)(New String() {LNBenableTileTop.ClientID, LNBdisableTileTop.ClientID, LNBvirtualDeleteTileTop.ClientID, LNBvirtualUndeleteTileTop.ClientID, LNBsaveTileTop.ClientID,
                                                                       LNBdisableTileBottom.ClientID, LNBdisableTileBottom.ClientID, LNBvirtualDeleteTileBottom.ClientID, LNBvirtualUndeleteTileBottom.ClientID, LNBsaveTileBottom.ClientID}), CTRLedit.maxfilesize)
    End Sub
    Private Sub CTRLedit_SessionTimeout() Handles CTRLedit.SessionTimeout
        DisplaySessionTimeout()
    End Sub
    Private Sub CTRLedit_EventErrorMessage(action As lm.Comol.Core.Dashboard.Domain.ModuleDashboard.ActionType, idTile As Long, idCommunity As Integer, idModule As Integer, errorType As lm.Comol.Core.BaseModules.Tiles.Business.ErrorMessageType) Handles CTRLedit.EventErrorMessage
        SendUserAction(idCommunity, idModule, idTile, action)
        DisplayMessage(errorType)
    End Sub
    Private Sub CTRLedit_EventMessage(ByVal action As lm.Comol.Core.Dashboard.Domain.ModuleDashboard.ActionType, idTile As Long, idCommunity As Integer, idModule As Integer) Handles CTRLedit.EventMessage
        SendUserAction(idCommunity, idModule, idTile, action)
        DisplayMessage(action)
    End Sub
    Private Sub LNBdisableTileBottom_Click(sender As Object, e As EventArgs) Handles LNBdisableTileBottom.Click, LNBdisableTileTop.Click
        CTRLedit.SetStatus(IdTile, lm.Comol.Core.Dashboard.Domain.AvailableStatus.Unavailable)
    End Sub
    Private Sub LNBenableTileBottom_Click(sender As Object, e As EventArgs) Handles LNBenableTileBottom.Click, LNBenableTileTop.Click
        CTRLedit.SetStatus(IdTile, lm.Comol.Core.Dashboard.Domain.AvailableStatus.Available)
    End Sub
    Private Sub LNBvirtualDeleteTileBottom_Click(sender As Object, e As EventArgs) Handles LNBvirtualDeleteTileBottom.Click, LNBvirtualDeleteTileTop.Click
        CTRLedit.VirtualDelete(IdTile, True)
    End Sub
    Private Sub LNBvirtualUndeleteTileBottom_Click(sender As Object, e As EventArgs) Handles LNBvirtualUndeleteTileBottom.Click, LNBvirtualUndeleteTileTop.Click
        CTRLedit.VirtualDelete(IdTile, False)
    End Sub
    Private Sub LNBsaveTileTop_Click(sender As Object, e As EventArgs) Handles LNBsaveTileTop.Click, LNBsaveTileBottom.Click
        CTRLedit.save()
    End Sub
#End Region
   
   
End Class