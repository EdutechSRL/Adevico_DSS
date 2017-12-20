Public Class AddTile
    Inherits TLeditPage


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        CurrentPresenter.InitView(True, 0, PreloadDashboardType, PreloadIdCommunity)
    End Sub

    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            .setHyperLink(HYPgoTo_TileListTop, False, True)
            .setHyperLink(HYPgoTo_TileListBottom, False, True)
            .setLinkButton(LNBaddTileTop, False, True)
            .setLinkButton(LNBaddTileBottom, False, True)
            Master.ServiceTitle = .getValue("Tile.NewTile")
            Master.ServiceNopermission = .getValue("Tile.NewTileNoPermission")
        End With
    End Sub
    Protected Friend Overrides Sub DisplaySessionTimeout()
        RedirectOnSessionTimeOut(lm.Comol.Core.Dashboard.Domain.RootObject.TileAdd(PreloadDashboardType, PreloadIdCommunity, PreloadIdDashboard, PreloadStep), IdContainerCommunity)
    End Sub

    Protected Friend Overrides Function GetBackUrlItemBottom() As HyperLink
        Return HYPgoTo_TileListBottom
    End Function
    Protected Friend Overrides Function GetBackUrlItemTop() As HyperLink
        Return HYPgoTo_TileListTop
    End Function
    Protected Friend Overrides Function GetTileControl() As UC_EditTile
        Return CTRLedit
    End Function
    Protected Friend Overrides Function GetSaveButton(Optional ByVal top As Boolean = True) As LinkButton
        Return IIf(top, LNBaddTileTop, LNBaddTileBottom)
    End Function
    Protected Friend Overrides Function GetEnableButton(Optional ByVal top As Boolean = True) As LinkButton
        Return Nothing
    End Function
    Protected Friend Overrides Function GetDisableButton(Optional ByVal top As Boolean = True) As LinkButton
        Return Nothing
    End Function
    Protected Friend Overrides Function GetVirtualDeleteButton(Optional ByVal top As Boolean = True) As LinkButton
        Return Nothing
    End Function
    Protected Friend Overrides Function GetVirtualUndeleteButton(Optional ByVal top As Boolean = True) As LinkButton
        Return Nothing
    End Function
    Protected Friend Overrides Function GetMessageControl() As UC_ActionMessages
        Return CTRLmessages
    End Function
#End Region

#Region "Internal"
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
    Private Sub CTRLedit_EventRedirectToEdit(idCommunity As Integer, idModule As Integer, idTile As Long, action As lm.Comol.Core.Dashboard.Domain.ModuleDashboard.ActionType) Handles CTRLedit.EventRedirectToEdit
        SendUserAction(idCommunity, idModule, idTile, action)
        PageUtility.RedirectToUrl(lm.Comol.Core.Dashboard.Domain.RootObject.TileEdit(idTile, PreloadDashboardType, PreloadIdCommunity, True))
    End Sub
    Private Sub AddTile_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Master.ShowDocType = True
        CTRLheader.InitializeControl(New List(Of String)(New String() {LNBaddTileTop.ClientID, LNBaddTileBottom.ClientID}), CTRLedit.MaxFileSize)
    End Sub
    Private Sub LNBaddTileTop_Click(sender As Object, e As EventArgs) Handles LNBaddTileTop.Click, LNBaddTileBottom.Click
        CTRLedit.Save()
    End Sub
#End Region

   
  
  
End Class