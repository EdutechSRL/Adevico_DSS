Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract
Imports lm.Comol.Core.BaseModules.Tiles.Presentation
Imports lm.Comol.Core.Dashboard.Domain
Public MustInherit Class TLeditPage
    Inherits TLeditPageBase
    Implements IViewEditPage

#Region "Context"
    Private _presenter As TileEditPagePresenter
    Protected Friend ReadOnly Property CurrentPresenter As TileEditPagePresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New TileEditPagePresenter(PageUtility.CurrentContext, Me)
            End If
            Return _presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Protected Friend ReadOnly Property PreloadFromAdd As Boolean Implements IViewEditPage.PreloadFromAdd
        Get
            Return Request.QueryString("fromAdd") = "true"
        End Get
    End Property
#Region "Settings"
    Private WriteOnly Property AllowSave As Boolean Implements IViewEditPage.AllowSave
        Set(value As Boolean)
            GetSaveButton().Visible = value
            GetSaveButton(False).Visible = value
        End Set
    End Property
    Private WriteOnly Property AllowDisable As Boolean Implements IViewEditPage.AllowDisable
        Set(value As Boolean)
            GetDisableButton().Visible = value
            GetDisableButton(False).Visible = value
        End Set
    End Property
    Private WriteOnly Property AllowEnable As Boolean Implements IViewEditPage.AllowEnable
        Set(value As Boolean)
            GetEnableButton().Visible = value
            GetEnableButton(False).Visible = value
        End Set
    End Property
    Private WriteOnly Property AllowVirtualUndelete As Boolean Implements IViewEditPage.AllowVirtualUndelete
        Set(value As Boolean)
            GetVirtualUndeleteButton().Visible = value
            GetVirtualUndeleteButton(False).Visible = value
        End Set
    End Property
    Private WriteOnly Property AllowVirtualDelete As Boolean Implements IViewEditPage.AllowVirtualDelete
        Set(value As Boolean)
            GetVirtualDeleteButton().Visible = value
            GetVirtualDeleteButton(False).Visible = value
        End Set
    End Property
    Private Sub DisplayTileAdded() Implements IViewEditPage.DisplayTileAdded
        Dim oControl As UC_ActionMessages = GetMessageControl()
        If Not IsNothing(oControl) Then
            oControl.InitializeControl(Resource.getValue("DisplayTileAdded"), Helpers.MessageType.success)
        End If
    End Sub
#End Region

#End Region


#Region "Implements"
    Private Sub InitalizeEditor(tile As lm.Comol.Core.BaseModules.Tiles.Domain.dtoEditTile, idTile As Long, idCommunity As Integer, idTileCommunity As Integer, displayName As String) Implements IViewEditPage.InitalizeEditor
        GetTileControl.InitalizeControl(tile, idTile, idCommunity, idTileCommunity, displayName)
    End Sub
    Private Sub InitalizeEditor(tile As lm.Comol.Core.BaseModules.Tiles.Domain.dtoEditTile, idTile As Long, idCommunity As Integer, idTileCommunity As Integer, Optional types As List(Of TileType) = Nothing) Implements IViewEditPage.InitalizeEditor
        GetTileControl.InitalizeControl(tile, idTile, idCommunity, idTileCommunity, types)
    End Sub
    Private Sub InitalizeEditor(tile As lm.Comol.Core.BaseModules.Tiles.Domain.dtoEditTile, idTile As Long, idCommunity As Integer, idTileCommunity As Integer, tags As List(Of lm.Comol.Core.DomainModel.TranslatedItem(Of Long)), Optional types As List(Of TileType) = Nothing) Implements IViewEditPage.InitalizeEditor
        GetTileControl.InitalizeControl(tile, idTile, idCommunity, idTileCommunity, tags, types)
    End Sub
#End Region

#Region "Internal"
    Protected Friend MustOverride Function GetTileControl() As UC_EditTile
    Protected Friend MustOverride Function GetSaveButton(Optional ByVal top As Boolean = True) As LinkButton
    Protected Friend MustOverride Function GetEnableButton(Optional ByVal top As Boolean = True) As LinkButton
    Protected Friend MustOverride Function GetDisableButton(Optional ByVal top As Boolean = True) As LinkButton
    Protected Friend MustOverride Function GetVirtualDeleteButton(Optional ByVal top As Boolean = True) As LinkButton
    Protected Friend MustOverride Function GetVirtualUndeleteButton(Optional ByVal top As Boolean = True) As LinkButton
    Public Function GetBaseUrl() As String
        Return PageUtility.BaseUrl
    End Function
#End Region

   
End Class