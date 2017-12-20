Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common

Public Class UC_OtherModuleLinkRepositoryItems
    Inherits BaseControl
    Implements IViewOtherModuleLinkRepositoryItems

#Region "IView"
    Public Property ViewAlreadySelectedFiles As Boolean Implements IViewOtherModuleLinkRepositoryItems.ViewAlreadySelectedFiles
        Get
            Return CBool(Me.ViewState("ViewAlreadySelectedFiles"))
        End Get
        Set(ByVal value As Boolean)
            HCselectedItems.Visible = value
            Me.ViewState("ViewAlreadySelectedFiles") = value
        End Set
    End Property
#End Region
#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
    Public Property FolderSelectable() As Boolean
        Get
            Return Me.CTRLCommunityFile.FolderSelectable
        End Get
        Set(ByVal value As Boolean)
            Me.CTRLCommunityFile.FolderSelectable = value
        End Set
    End Property
    Public Property TriStateSelection() As Boolean
        Get
            Return Me.CTRLCommunityFile.TriStateSelection
        End Get
        Set(ByVal value As Boolean)
            Me.CTRLCommunityFile.TriStateSelection = value
        End Set
    End Property
    Public Property MaxSelectorHeight As System.Web.UI.WebControls.Unit
        Get
            Return CTRLCommunityFile.MaxSelectorHeight
        End Get
        Set(ByVal value As System.Web.UI.WebControls.Unit)
            CTRLCommunityFile.MaxSelectorHeight = value
        End Set
    End Property
    Public Property MaxSelectorWidth As System.Web.UI.WebControls.Unit
        Get
            Return CTRLCommunityFile.MaxSelectorWidth
        End Get
        Set(ByVal value As System.Web.UI.WebControls.Unit)
            CTRLCommunityFile.MaxSelectorWidth = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub InitializeControl(ByVal IdCommunity As Integer, ByVal itemLinks As List(Of iCoreItemFileLink(Of Long)), ByVal ViewSelectedPanel As Boolean, ByVal LoadSelectedFilesIntoTree As Boolean, ByVal showAlsoHiddenItems As Boolean, ByVal adminPurpose As Boolean) Implements IViewOtherModuleLinkRepositoryItems.InitializeControl
        ViewAlreadySelectedFiles = ViewSelectedPanel AndAlso itemLinks.Count > 0
        If itemLinks.Count > 0 Then
            Me.CBLselectedItems.DataSource = (From f In itemLinks Order By f.File.DisplayName Select f.ItemFileLinkId, f.File.DisplayName).ToList
            For Each item As iCoreItemFileLink(Of Long) In itemLinks
                Dim oListItem As New ListItem With {.Selected = True, .Value = item.ItemFileLinkId}
                If item.File.isFile Then
                    oListItem.Text = "<img src='" & BaseUrl & Me.PageUtility.SystemSettings.Extension.FindIconImage(item.File.Extension) & "'>" & item.File.DisplayName
                Else
                    oListItem.Text = "<img src='" & BaseUrl & "RadControls/TreeView/Skins/Materiale/folder.gif" & "'>" & item.File.DisplayName
                End If
                Me.CBLselectedItems.Items.Add(oListItem)
            Next
        End If

        Me.CTRLCommunityFile.InitializeControl(IdCommunity, (From f In itemLinks Select f.File.Id).ToList, LoadSelectedFilesIntoTree, showAlsoHiddenItems, adminPurpose, True, lm.Comol.Core.DomainModel.Repository.RepositoryItemType.None)
    End Sub

#Region "Inherited"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_OtherModuleItemFiles", "Modules", "Repository")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLiteral(LTselectedItems)
        End With
    End Sub
#End Region

    Public Sub DisableControl() Implements IViewOtherModuleLinkRepositoryItems.DisableControl
        Me.CTRLCommunityFile.Visible = False
        Me.ViewAlreadySelectedFiles = False
    End Sub

    Public Function GetRepositoryItemLinksId() As List(Of Long) Implements IViewOtherModuleLinkRepositoryItems.GetRepositoryItemLinksId
        If Me.CBLselectedItems.Items.Count = 0 Then
            Return New List(Of Long)
        Else
            Return (From item As ListItem In Me.CBLselectedItems.Items Where item.Selected Select CLng(item.Value)).ToList
        End If
    End Function

    Public Function GetNewRepositoryItemLinks() As List(Of ModuleActionLink) Implements IViewOtherModuleLinkRepositoryItems.GetNewRepositoryItemLinks
        Return Me.CTRLCommunityFile.GetSelectedItemsActionLink
    End Function

    Public Function GetSelectedFolder() As List(Of Long) Implements IViewOtherModuleLinkRepositoryItems.GetSelectedFolder
        Return Me.CTRLCommunityFile.GetSelectedFolder
    End Function
End Class