Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.Repository.Presentation
Public Class UC_ModuleInternalFileSelector
    Inherits BaseControl
    Implements IViewModuleInternalFilesSelector

#Region "Implements"
    Public ReadOnly Property FilesCount As Integer Implements IViewModuleInternalFilesSelector.FilesCount
        Get
            Return Me.CBLItemFileLink.Items.Count
        End Get
    End Property
    Public ReadOnly Property HasFileToPublish As Boolean Implements IViewModuleInternalFilesSelector.HasFileToPublish
        Get
            Return (Me.CBLItemFileLink.Items.Count > 0)
        End Get
    End Property
    Public ReadOnly Property SelectedItemFileLinksId As List(Of Long) Implements IViewModuleInternalFilesSelector.SelectedItemFileLinksId
        Get
            Return (From item As ListItem In Me.CBLItemFileLink.Items Where item.Selected Select CLng(item.Value)).ToList
        End Get
    End Property
    Public ReadOnly Property HasPermissionToSelectFile As Boolean Implements IViewModuleInternalFilesSelector.HasPermissionToSelectFile
        Get
            Return ViewStateOrDefault("HasPermissionToSelectFile", False)
        End Get
    End Property
    Public ReadOnly Property SelectorIdCommunity As Integer Implements IViewModuleInternalFilesSelector.SelectorIdCommunity
        Get
            Return ViewStateOrDefault("SelectorIdCommunity", -1)
        End Get
    End Property

    Public ReadOnly Property SelectedModuleFileId As List(Of Long) Implements IViewModuleInternalFilesSelector.SelectedModuleFileId
        Get
            Return (From dto In FilesDtoID Where SelectedItemFileLinksId.Contains(dto.ModuleLinkID) Select dto.FileID).ToList
        End Get
    End Property
   
    Private Property FilesDtoID As List(Of dtoFileLink)
        Get
            Return ViewStateOrDefault("FilesDtoID", New List(Of dtoFileLink))
        End Get
        Set(ByVal value As List(Of dtoFileLink))
            ViewState("FilesDtoID") = value
        End Set
    End Property
    Private Function ViewStateOrDefault(Of T)(ByVal Key As String, ByVal DefaultValue As T) As T
        If (ViewState(Key) Is Nothing) Then
            ViewState(Key) = DefaultValue
            Return DefaultValue
        Else
            Return ViewState(Key)
        End If
    End Function
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()

    End Sub

    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region

    Public Sub InitializeNoPermission(ByVal idCommunity As Integer) Implements IViewModuleInternalFilesSelector.InitializeNoPermission
        ViewState("InternalFileSelectorIdCommunity") = idCommunity
        ViewState("HasPermissionToSelectFile") = False
    End Sub
    Public Sub InitializeView(ByVal links As IList(Of iCoreItemFileLink(Of Long)), ByVal selectedLinkId As Long, ByVal idCommunity As Integer) Implements IViewModuleInternalFilesSelector.InitializeView
        Dim selectedLinksId As New List(Of Long)
        selectedLinksId.Add(selectedLinkId)
        InitializeView(links, selectedLinksId, idCommunity)
    End Sub

    Public Sub InitializeView(ByVal links As IList(Of iCoreItemFileLink(Of Long)), ByVal selectedLinksId As List(Of Long), ByVal idCommunity As Integer) Implements IViewModuleInternalFilesSelector.InitializeView
        Dim list As New List(Of dtoFileLink)

        For Each link As iCoreItemFileLink(Of Long) In links
            Dim oListItem As New ListItem
            If Not IsNothing(link) AndAlso Not IsNothing(link.File) Then
                oListItem.Value = link.Link.Id
                If link.File.isFile Then
                    oListItem.Text = "<img src='" & BaseUrl & Me.PageUtility.SystemSettings.Extension.FindIconImage(link.File.Extension) & "'>" & link.File.DisplayName
                Else
                    oListItem.Text = "<img src='" & BaseUrl & "RadControls/TreeView/Skins/Materiale/folder.gif" & "'>" & link.File.DisplayName
                End If
                If (selectedLinksId.Contains(link.Link.Id)) Then
                    oListItem.Selected = True
                End If
                list.Add(New dtoFileLink() With {.FileID = link.File.Id, .ModuleLinkID = link.Link.Id})
                Me.CBLItemFileLink.Items.Add(oListItem)
            End If
        Next
        FilesDtoID = list
        ViewState("SelectorIdCommunity") = idCommunity
        ViewState("HasPermissionToSelectFile") = True
    End Sub
    Public Sub UpdateSelectedFilesId(ByVal filesID As List(Of Long)) Implements IViewModuleInternalFilesSelector.UpdateSelectedFilesId
        Me.CBLItemFileLink.SelectedIndex = -1

        Dim selectedItems As List(Of Long) = (From d In FilesDtoID Where filesID.Contains(d.FileID) Select d.ModuleLinkID).ToList
        For Each item As ListItem In (From i As ListItem In Me.CBLItemFileLink.Items Where selectedItems.Contains(CLng(i.Value)))
            item.Selected = True
        Next
    End Sub

    <Serializable()> Private Class dtoFileLink
        Public FileID As Long
        Public ModuleLinkID As Long
    End Class

End Class