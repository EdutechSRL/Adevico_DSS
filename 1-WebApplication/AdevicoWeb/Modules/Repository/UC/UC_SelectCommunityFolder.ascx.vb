Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.UI.Presentation

Partial Public Class UC_SelectCommunityFolder
    Inherits BaseControlWithLoad
    Implements IviewCommunityFolderSelect

    Public Event FolderSelected(ByVal FolderID As Long)
    Public Event AjaxFolderSelected(ByVal sender As Object, ByVal e As System.EventArgs)

#Region "Inherited"
    Private _PageUtility As PresentationLayer.OLDpageUtility
    Private _Presenter As lm.Comol.Modules.Base.Presentation.CommunityFolderSelectPresenter
    Private _BaseUrl As String
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _BaseTreeUrl As String
    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property

    Public Overloads ReadOnly Property BaseUrl() As String
        Get
            If _BaseUrl = "" Then
                _BaseUrl = Me.PageUtility.BaseUrl
            End If
            Return _BaseUrl
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As CommunityFolderSelectPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New CommunityFolderSelectPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
    Protected Friend ReadOnly Property PageUtility() As PresentationLayer.OLDpageUtility
        Get
            If IsNothing(_PageUtility) Then
                _PageUtility = New OLDpageUtility(HttpContext.Current)
            End If
            Return _PageUtility
        End Get
    End Property
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overloads ReadOnly Property BaseTreeUrl() As String
        Get
            If _BaseTreeUrl = "" Then
                _BaseTreeUrl = Me.PageUtility.BaseUrl & "RadControls/TreeView/Skins/Materiale/"
            End If
            Return _BaseTreeUrl
        End Get
    End Property

    Public ReadOnly Property SelectedFolderPath() As List(Of FilterElement) Implements IviewCommunityFolderSelect.SelectedFolderPath
        Get
            Dim oList As New List(Of FilterElement)

            Dim oNode As Telerik.Web.UI.RadTreeNode = Me.RDTcommunityRepository.SelectedNode
            If oNode Is Nothing Then
                Return oList
            Else
                While Not IsNothing(oNode)
                    oList.Insert(0, New FilterElement(oNode.Value, oNode.Text))
                    If oNode.Value = 0 Then
                        oNode = Nothing
                    Else
                        oNode = oNode.ParentNode
                    End If
                End While
            End If
            Return oList
        End Get
    End Property
    Public ReadOnly Property SelectedFolderPathName() As String Implements IviewCommunityFolderSelect.SelectedFolderPathName
        Get
            Dim Path As String

            Dim oNode As Telerik.Web.UI.RadTreeNode = Me.RDTcommunityRepository.SelectedNode
            If oNode Is Nothing Then
                Path = ""
            Else
                While Not IsNothing(oNode)
                    Path = oNode.Text & "/" & Path
                    If oNode.Value = 0 Then
                        oNode = Nothing
                    Else
                        oNode = oNode.ParentNode
                    End If
                End While
            End If
            Return Path
        End Get
    End Property
    Public Property SelectionMode() As ListSelectionMode Implements IviewCommunityFolderSelect.SelectionMode
        Get
            Dim oSelection As ListSelectionMode
            Try
                If String.IsNullOrEmpty(Me.ViewState("SelectionMode")) Then
                    oSelection = ListSelectionMode.Multiple
                Else
                    oSelection = DirectCast(Me.ViewState("SelectionMode"), ListSelectionMode)
                End If
            Catch ex As Exception
                oSelection = ListSelectionMode.Multiple
            End Try
            Return oSelection
        End Get
        Set(ByVal value As ListSelectionMode)
            Me.ViewState("SelectionMode") = value
            Me.RDTcommunityRepository.CheckBoxes = (value = ListSelectionMode.Multiple)
        End Set
    End Property
    Public Property AjaxEnabled() As Boolean Implements IviewCommunityFolderSelect.AjaxEnabled
        Get
            Dim iResponse As Boolean = False
            Try
                iResponse = CBool(Me.ViewState("AjaxEnabled"))
            Catch ex As Exception
            End Try
            Return iResponse
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AjaxEnabled") = value
        End Set
    End Property
    Public Property AutoRedirect() As Boolean
        Get
            Return ViewStateOrDefault("AutoRedirect", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AutoRedirect") = value
        End Set
    End Property
    Private Property RepositoryCommunityID() As Integer
        Get
            Return ViewStateOrDefault("RepositoryCommunityID", 0)
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("RepositoryCommunityID") = value
        End Set
    End Property
    Private Property CurrentView() As IViewExploreCommunityRepository.ViewRepository
        Get
            Return ViewStateOrDefault("CurrentView", IViewExploreCommunityRepository.ViewRepository.FolderList)
        End Get
        Set(ByVal value As IViewExploreCommunityRepository.ViewRepository)
            Me.ViewState("CurrentView") = value
        End Set
    End Property
    Private Property PreLoadedContentView As lm.Comol.Core.DomainModel.ContentView
        Get
            Return ViewStateOrDefault("PreLoadedContentView", lm.Comol.Core.DomainModel.ContentView.viewAll)
        End Get
        Set(ByVal value As lm.Comol.Core.DomainModel.ContentView)
            Me.ViewState("PreLoadedContentView") = value
        End Set
    End Property
#End Region

#Region "Inherited Method"
    Public Overrides Sub BindDati()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("UC_SelectCommunityFiles", "Generici", "UC_File")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLiteral(LTnoFolder)
        End With
    End Sub
#End Region

    Public Sub InitializeControl(ByVal isForManagement As Boolean, CommunityID As Integer, ByVal SelectedFolderID As Long, ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean, Optional ByVal ExludeFolderID As Long = -1, Optional ByVal view As IViewExploreCommunityRepository.ViewRepository = IViewExploreCommunityRepository.ViewRepository.FolderList, Optional contentView As lm.Comol.Core.DomainModel.ContentView = lm.Comol.Core.DomainModel.ContentView.viewAll)
        RepositoryCommunityID = CommunityID
        CurrentView = view
        PreLoadedContentView = contentView
        Me.CurrentPresenter.InitView(isForManagement, CommunityID, SelectedFolderID, ShowHiddenItems, AdminPurpose, ExludeFolderID)
    End Sub

    Public Sub LoadTree(ByVal oCommuntyRepository As dtoFileFolder, isForManagement As Boolean) Implements IviewCommunityFolderSelect.LoadTree
        Dim oRootNode As New Telerik.Web.UI.RadTreeNode
        Me.RDTcommunityRepository.Nodes.Clear()

        'oRootNode.Text
        oRootNode = CreateNode(isForManagement, 0, 0, Me.Resource.getValue("BaseFolder"), False, oCommuntyRepository.Selectable, True)
        Me.RDTcommunityRepository.Nodes.Add(oRootNode)
        For Each oFolder As dtoFileFolder In oCommuntyRepository.SubFolders
            Dim oNodeFolder As Telerik.Web.UI.RadTreeNode = Me.CreateNode(isForManagement, oFolder.ID, oFolder.ID, oFolder.Name, False, oFolder.Selectable, False)
            oRootNode.Nodes.Add(oNodeFolder)
            FoldersCount += 1
            Me.RecursivelyPopulate(oFolder, oNodeFolder, isForManagement)
            HasMoreFolder = True
        Next
    End Sub
    Private Sub RecursivelyPopulate(ByVal oFolder As dtoFileFolder, ByVal oFatherNode As Telerik.Web.UI.RadTreeNode, isForManagement As Boolean)
        For Each oSubFolder As dtoFileFolder In oFolder.SubFolders
            Dim oNodeFolder As Telerik.Web.UI.RadTreeNode = Me.CreateNode(isForManagement, oSubFolder.ID, oSubFolder.ID, oSubFolder.Name, False, oSubFolder.Selectable, False)
            FoldersCount += 1
            Me.RecursivelyPopulate(oSubFolder, oNodeFolder, isForManagement)
            oFatherNode.Nodes.Add(oNodeFolder)
        Next
    End Sub

    Private Function CreateNode(ByVal forAdmin As Boolean, ByVal id As Long, ByVal category As Long, ByVal name As String, ByVal selected As Boolean, ByVal selectable As Boolean, ByVal expanded As Boolean) As Telerik.Web.UI.RadTreeNode
        Dim oNode As New Telerik.Web.UI.RadTreeNode
        Try
            oNode.Value = id

            '  oNode.Checkable = (Category > 0)
            oNode.Category = category
            oNode.Text = " " & name
            oNode.Checked = False
            oNode.Expanded = expanded
            oNode.ImageUrl = Me.BaseTreeUrl & "folder.gif"
            oNode.ExpandedImageUrl = Me.BaseTreeUrl & "folderOpen.gif"
            oNode.Enabled = selectable
            If AutoRedirect Then
                If forAdmin Then
                    oNode.NavigateUrl = PageUtility.ApplicationUrlBase & lm.Comol.Core.BaseModules.Repository.Domain.RootObject.RepositoryManagement(id, RepositoryCommunityID, CurrentView.ToString, 0, PreLoadedContentView)
                Else
                    oNode.NavigateUrl = PageUtility.ApplicationUrlBase & lm.Comol.Core.BaseModules.Repository.Domain.RootObject.RepositoryList(id, RepositoryCommunityID, CurrentView.ToString, 0, PreLoadedContentView)
                End If

                oNode.PostBack = False
            End If
        Catch ex As Exception

        End Try
        Return oNode
    End Function


    Public WriteOnly Property ShowNoFileFound() As Boolean Implements IviewCommunityFolderSelect.ShowNoFolderFound
        Set(ByVal value As Boolean)
            Me.LTnoFolder.Visible = value
            Me.RDTcommunityRepository.Visible = Not value
        End Set
    End Property

    Public Property SelectedFolder() As Long Implements IviewCommunityFolderSelect.SelectedFolder
        Get
            If Me.RDTcommunityRepository.SelectedNode Is Nothing Then
                Return -1
            Else
                Return CLng(Me.RDTcommunityRepository.SelectedValue)
            End If
        End Get
        Set(ByVal value As Long)
            If Me.RDTcommunityRepository.Nodes.Count > 0 Then
                Dim oNode As Telerik.Web.UI.RadTreeNode = Me.RDTcommunityRepository.FindNodeByValue(value)
                If oNode Is Nothing Then
                    oNode = Me.RDTcommunityRepository.FindNodeByValue(0)
                End If
                oNode.Selected = True
                If oNode.Value > 0 AndAlso Not oNode.ParentNode.Expanded Then
                    While Not oNode.ParentNode.Expanded
                        oNode.ParentNode.Expanded = True
                        oNode = oNode.ParentNode
                    End While
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property SelectedFolderName() As String Implements IviewCommunityFolderSelect.SelectedFolderName
        Get
            If Me.RDTcommunityRepository.SelectedNode Is Nothing Then
                Return ""
            Else
                Return Trim(Me.RDTcommunityRepository.SelectedNode.Text)
            End If
        End Get
    End Property

    Private Sub RDTcommunityRepository_NodeClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadTreeNodeEventArgs) Handles RDTcommunityRepository.NodeClick
        If Me.AjaxEnabled Then
            Dim Args As New FolderArgs With {.FolderID = CLng(Me.RDTcommunityRepository.SelectedValue)}
            RaiseEvent AjaxFolderSelected(sender, Args)
        Else
            RaiseEvent FolderSelected(CLng(Me.RDTcommunityRepository.SelectedValue))
        End If
    End Sub

    Public Class FolderArgs
        Inherits System.EventArgs
        Public FolderID As Integer
    End Class

    Public Property HasMoreFolder() As Boolean
        Get
            If CBool(Me.ViewState("HasMoreFolder")) Then
                Return CBool(Me.ViewState("HasMoreFolder"))
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("HasMoreFolder") = value
        End Set
    End Property
    Public Property FoldersCount() As Integer
        Get
            If IsNumeric(Me.ViewState("FoldersCount")) Then
                Return CInt(Me.ViewState("FoldersCount"))
            Else
                Return 0
            End If
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("FoldersCount") = value
        End Set
    End Property

    Public Property ForUpload As Boolean Implements IviewCommunityFolderSelect.ForUpload
        Get
            If Not TypeOf Me.ViewState("ForUpload") Is Boolean Then
                Me.ViewState("ForUpload") = False
            End If
            Return CBool(Me.ViewState("ForUpload"))
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("ForUpload") = value
        End Set
    End Property
End Class