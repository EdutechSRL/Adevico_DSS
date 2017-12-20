Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.BaseModules.FileRepository.Presentation
Imports lm.Comol.Core.BaseModules.FileRepository.Presentation.Domain
Imports lm.Comol.Core.FileRepository.Domain

Public Class UC_ModuleInternalLink
    Inherits FRbaseControl
    Implements IViewModuleInternalLink

#Region "Implements"
    Public Property ViewAlreadySelectedFiles As Boolean Implements IViewModuleInternalLink.ViewAlreadySelectedFiles
        Get
            Return ViewStateOrDefault("ViewAlreadySelectedFiles", False)
        End Get
        Set(ByVal value As Boolean)
            LIselectedItems.Visible = value
            ViewState("ViewAlreadySelectedFiles") = value
        End Set
    End Property
    Public Property TreeSelect As TreeMode Implements IViewModuleInternalLink.TreeSelect
        Get
            Return CTRLrepositorySelector.TreeSelect
        End Get
        Set(ByVal value As TreeMode)
            CTRLrepositorySelector.TreeSelect = value
        End Set
    End Property
    Public Property FolderSelectable As Boolean Implements IViewModuleInternalLink.FolderSelectable
        Get
            Return CTRLrepositorySelector.FolderSelectable
        End Get
        Set(ByVal value As Boolean)
            CTRLrepositorySelector.FolderSelectable = value
        End Set
    End Property
    Public Property RemoveEmptyFolders As Boolean Implements IViewModuleInternalLink.RemoveEmptyFolders
        Get
            Return CTRLrepositorySelector.RemoveEmptyFolders
        End Get
        Set(ByVal value As Boolean)
            CTRLrepositorySelector.RemoveEmptyFolders = value
        End Set
    End Property
#End Region

#Region "Internal"
    Public Property MaxSelectorHeight As System.Web.UI.WebControls.Unit
        Get
            Return CTRLrepositorySelector.MaxSelectorHeight
        End Get
        Set(ByVal value As System.Web.UI.WebControls.Unit)
            CTRLrepositorySelector.MaxSelectorHeight = value
        End Set
    End Property
    Public Property MaxSelectorWidth As System.Web.UI.WebControls.Unit
        Get
            Return CTRLrepositorySelector.MaxSelectorWidth
        End Get
        Set(ByVal value As System.Web.UI.WebControls.Unit)
            CTRLrepositorySelector.MaxSelectorWidth = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub



#Region "Inherited"
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLiteral(LTlinkedItemsToOtherModule)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControlForCommunity(idCurrentUser As Integer, idCommunity As Integer, files As List(Of RepositoryItemLinkBase(Of Long)), viewSelectedPanel As Boolean, loadSelectedFilesIntoTree As Boolean, showAlsoHiddenItems As Boolean, adminPurpose As Boolean) Implements IViewModuleInternalLink.InitializeControlForCommunity
        InitializeControl(idCurrentUser, lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier.Create(lm.Comol.Core.FileRepository.Domain.RepositoryType.Community, idCommunity), files, viewSelectedPanel, loadSelectedFilesIntoTree, showAlsoHiddenItems, adminPurpose)
    End Sub
    Public Sub InitializeControlForPortal(idCurrentUser As Integer, files As List(Of RepositoryItemLinkBase(Of Long)), viewSelectedPanel As Boolean, loadSelectedFilesIntoTree As Boolean, showAlsoHiddenItems As Boolean, adminPurpose As Boolean) Implements IViewModuleInternalLink.InitializeControlForPortal
        InitializeControl(idCurrentUser, lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier.Create(lm.Comol.Core.FileRepository.Domain.RepositoryType.Portal, 0), files, viewSelectedPanel, loadSelectedFilesIntoTree, showAlsoHiddenItems, adminPurpose)
    End Sub
    Public Sub InitializeControl(ByVal idCurrentUser As Integer, identifier As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier, ByVal itemLinks As List(Of RepositoryItemLinkBase(Of Long)), ByVal viewSelectedPanel As Boolean, ByVal loadSelectedFilesIntoTree As Boolean, ByVal showAlsoHiddenItems As Boolean, ByVal adminPurpose As Boolean) Implements IViewModuleInternalLink.InitializeControl
        ViewAlreadySelectedFiles = viewSelectedPanel AndAlso itemLinks.Count > 0
        If itemLinks.Count > 0 Then
            For Each item As RepositoryItemLinkBase(Of Long) In itemLinks.OrderBy(Function(i) i.File.DisplayName)
                Dim oListItem As New ListItem With {.Selected = True, .Value = item.IdObjectLink}
                oListItem.Text = CTRLrepositorySelector.GetFilenameRender(item.File.DisplayName, item.File.Extension, item.File.Type)
                CBLselectedItems.Items.Add(oListItem)
            Next
        End If
        CTRLrepositorySelector.InitializeControlForModule(idCurrentUser, identifier, loadSelectedFilesIntoTree, adminPurpose, showAlsoHiddenItems, True, lm.Comol.Core.FileRepository.Domain.ItemAvailability.available, FolderSelectable, itemLinks.Select(Function(i) i.File.IdItem).Distinct().ToList())
    End Sub
    Public Sub InitializeControl(idCurrentUser As Integer, identifier As RepositoryIdentifier, itemLinks As List(Of Long), typesToLoad As List(Of ItemType), availability As ItemAvailability, showAlsoHiddenItems As Boolean, adminPurpose As Boolean) Implements IViewModuleInternalLink.InitializeControl
        ViewAlreadySelectedFiles = False
        CTRLrepositorySelector.InitializeControlForModule(idCurrentUser, identifier, False, adminPurpose, showAlsoHiddenItems, True, typesToLoad, availability, FolderSelectable, New List(Of Long), itemLinks)
    End Sub
    Public Sub DisableControl() Implements IViewModuleInternalLink.DisableControl
        CTRLrepositorySelector.DisableControl()
        CBLselectedItems.Enabled = True
    End Sub
    Private Function GetRepositoryItemLinksId() As List(Of Long) Implements IViewModuleInternalLink.GetRepositoryItemLinksId
        If CBLselectedItems.Items.Count = 0 Then
            Return New List(Of Long)
        Else
            Return (From item As ListItem In CBLselectedItems.Items Where item.Selected Select CLng(item.Value)).ToList
        End If
    End Function
    Public Function GetNewRepositoryItemLinks() As List(Of ModuleActionLink) Implements IViewModuleInternalLink.GetNewRepositoryItemLinks
        Return CTRLrepositorySelector.GetSelectedItemsActionLink()
    End Function

    Private Function GetSelectedFolders() As List(Of Long) Implements IViewModuleInternalLink.GetSelectedFolders
        Return CTRLrepositorySelector.GetSelectedFolders
    End Function
#End Region

#Region "Internal"
    Public Function UnselectAll()
        CTRLrepositorySelector.UnselectAll()
    End Function
#End Region


    
End Class