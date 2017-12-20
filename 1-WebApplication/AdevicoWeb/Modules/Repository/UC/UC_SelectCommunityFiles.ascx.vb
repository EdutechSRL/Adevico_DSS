Imports lm.Comol.Core.BaseModules.Repository
Imports lm.Comol.Core.BaseModules.Repository.Presentation
Imports lm.Comol.UI.Presentation

Partial Public Class UC_SelectCommunityFiles
    Inherits BaseControlWithLoad
    Implements IViewCommunityFileSelector

#Region "Context"
    Private _Presenter As CommunityFileSelectorPresenter
    Public ReadOnly Property CurrentPresenter() As CommunityFileSelectorPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New CommunityFileSelectorPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property EnableClientScript As Boolean Implements IViewCommunityFileSelector.EnableClientScript
        Get
            Return ViewStateOrDefault("EnableClientScript", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("EnableClientScript") = value
            If value Then
                Me.RDTcommunityRepository.OnClientNodeChecked = "CheckChildNodesImport"
            End If
        End Set
    End Property
#End Region

#Region "Inherited"
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Control Property / Events"
    Private _BaseUrl As String
    Private _BaseTreeUrl As String
    Public Overloads ReadOnly Property BaseUrl() As String
        Get
            If _BaseUrl = "" Then
                _BaseUrl = Me.PageUtility.BaseUrl
            End If
            Return _BaseUrl
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
    Public Property FolderSelectable() As Boolean
        Get
            If TypeOf Me.ViewState("FolderSelectable") Is Boolean Then
                Return CBool(Me.ViewState("FolderSelectable"))
            Else
                Return True
            End If
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("FolderSelectable") = value
        End Set
    End Property
    Public Property TriStateSelection() As Boolean
        Get
            Return Me.RDTcommunityRepository.TriStateCheckBoxes
        End Get
        Set(ByVal value As Boolean)
            Me.RDTcommunityRepository.TriStateCheckBoxes = value
            If Not value Then
                Me.RDTcommunityRepository.CheckChildNodes = False
            End If
        End Set
    End Property
    Public Property MaxSelectorHeight As System.Web.UI.WebControls.Unit
        Get
            Return Me.RDTcommunityRepository.Height
        End Get
        Set(ByVal value As System.Web.UI.WebControls.Unit)
            Me.RDTcommunityRepository.Height = value
        End Set
    End Property
    Public Property MaxSelectorWidth As System.Web.UI.WebControls.Unit
        Get
            Return Me.RDTcommunityRepository.Width
        End Get
        Set(ByVal value As System.Web.UI.WebControls.Unit)
            Me.RDTcommunityRepository.Width = value
        End Set
    End Property
#End Region



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherited Method"
    Public Overrides Sub BindDati()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("UC_SelectCommunityFiles", "Generici", "UC_File")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLiteral(LTnofile)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(idCommunity As Integer, selectedFiles As List(Of Long), showHiddenItems As Boolean, forAdminPurpose As Boolean, disableWaitingFiles As Boolean, type As lm.Comol.Core.DomainModel.Repository.RepositoryItemType) Implements lm.Comol.Core.BaseModules.Repository.Presentation.IViewCommunityFileSelector.InitializeControl
        Me.CurrentPresenter.InitView(idCommunity, selectedFiles, True, showHiddenItems, forAdminPurpose, disableWaitingFiles, type)
    End Sub
    Public Sub InitializeControl(idCommunity As Integer, selectedFiles As List(Of Long), loadIntoTree As Boolean, showHiddenItems As Boolean, forAdminPurpose As Boolean, disableWaitingFiles As Boolean, type As lm.Comol.Core.DomainModel.Repository.RepositoryItemType) Implements lm.Comol.Core.BaseModules.Repository.Presentation.IViewCommunityFileSelector.InitializeControl
        Me.CurrentPresenter.InitView(idCommunity, selectedFiles, loadIntoTree, showHiddenItems, forAdminPurpose, disableWaitingFiles, type)
    End Sub
    Public Sub LoadTree(tree As lm.Comol.Core.BaseModules.Repository.dtoFileFolder) Implements IViewCommunityFileSelector.LoadTree
        Dim oRootNode As New Telerik.Web.UI.RadTreeNode
        Me.RDTcommunityRepository.Nodes.Clear()

        'oRootNode.Text
        oRootNode = CreateNode(0, 0, "Base", "", False, True)
        Me.RDTcommunityRepository.Nodes.Add(oRootNode)
        For Each oFolder As dtoFileFolder In tree.SubFolders
            Dim oNodeFolder As Telerik.Web.UI.RadTreeNode = Me.CreateNode(oFolder.ID, -oFolder.ID, oFolder.Name, "", oFolder.Selected, False)
            oRootNode.Nodes.Add(oNodeFolder)
            Me.RecursivelyPopulate(oFolder, oNodeFolder)
        Next
        For Each oFile As dtoGenericFile In tree.Files
            oRootNode.Nodes.Add(Me.CreateNode(oFile.ID, oFile.ID, oFile.Name, oFile.Extension, oFile.Selected, False))
        Next
    End Sub
    Private Sub DisplayNoFilesFound() Implements IViewCommunityFileSelector.DisplayNoFilesFound
        Me.LTnofile.Visible = True
        Me.RDTcommunityRepository.Visible = False
    End Sub
    Public Sub UnselectAll() Implements IViewCommunityFileSelector.UnselectAll
        Me.RDTcommunityRepository.ClearCheckedNodes()
    End Sub
#End Region

    Private Sub RecursivelyPopulate(ByVal oFolder As dtoFileFolder, ByVal oFatherNode As Telerik.Web.UI.RadTreeNode)
        For Each oSubFolder As dtoFileFolder In oFolder.SubFolders
            Dim oNodeFolder As Telerik.Web.UI.RadTreeNode = Me.CreateNode(oSubFolder.ID, -oSubFolder.ID, oSubFolder.Name, "", oSubFolder.Selected, False)
            Me.RecursivelyPopulate(oSubFolder, oNodeFolder)
            oFatherNode.Nodes.Add(oNodeFolder)
        Next
        For Each oFile As dtoGenericFile In oFolder.Files
            oFatherNode.Nodes.Add(Me.CreateNode(oFile.ID, oFile.ID, oFile.Name, oFile.Extension, oFile.Selected, False))
        Next
    End Sub

    Private Function CreateNode(ByVal Id As Long, ByVal Category As Long, ByVal Name As String, ByVal Extension As String, ByVal Selected As Boolean, ByVal Expanded As Boolean) As Telerik.Web.UI.RadTreeNode
        Dim oNode As New Telerik.Web.UI.RadTreeNode
        Try
            oNode.Value = Id

            '  oNode.Checkable = (Category > 0)
            oNode.Category = Category
            If Category > 0 Then
                'If FileLayer.Exists.File(Me.BaseUrlDrivePath & "images\ico\" & Extension & ".gif") Then
                '    oNode.ImageUrl = Me.BaseUrl & "images/ico/" & Extension & ".gif"
                'Else
                '    oNode.ImageUrl = Me.BaseUrl & "images/ico/bo.gif"
                'End If
                oNode.ImageUrl = Me.BaseUrl & Me.PageUtility.SystemSettings.Extension.FindIconImage(Extension)
                oNode.Text = Name '" <img src=" & """" & oNode.ImageUrl & """" & "> " & Name
                '         oNode.ImageUrl = ""
                oNode.Checked = Selected
            Else
                oNode.Checkable = Me.FolderSelectable
                oNode.Text = " " & Name
                oNode.Checked = False
                oNode.Expanded = Expanded
                oNode.ImageUrl = Me.BaseTreeUrl & "folder.gif"
                oNode.ExpandedImageUrl = Me.BaseTreeUrl & "folderOpen.gif"
            End If
        Catch ex As Exception

        End Try
        Return oNode
    End Function
    Public Function GetSelectedFiles() As List(Of Long)
        Dim iResponse As New List(Of Long)

        '  iResponse = (From oNode As Telerik.Web.UI.RadTreeNode In Me.RDTcommunityRepository.GetAllNodes() _
        '      Where CLng(oNode.Category) > 0 AndAlso oNode.Checked Select CLng(oNode.Value)).ToList
        iResponse = (From oNode As Telerik.Web.UI.RadTreeNode In Me.RDTcommunityRepository.CheckedNodes Where CLng(oNode.Category) > 0 Select CLng(oNode.Value)).ToList

        Return iResponse
    End Function
    Public Function GetSelectedItemsActionLink() As List(Of lm.Comol.Core.DomainModel.ModuleActionLink)
        Return Me.CurrentPresenter.GetSelectedItemsActionLink((From oNode As Telerik.Web.UI.RadTreeNode In Me.RDTcommunityRepository.CheckedNodes Where CLng(oNode.Category) > 0 Select CLng(oNode.Value)).ToList)
    End Function


    Public Function GetSelectedFolder() As List(Of Long)
        Dim iResponse As New List(Of Long)

        '  iResponse = (From oNode As Telerik.Web.UI.RadTreeNode In Me.RDTcommunityRepository.GetAllNodes() _
        '      Where CLng(oNode.Category) > 0 AndAlso oNode.Checked Select CLng(oNode.Value)).ToList
        iResponse = (From oNode As Telerik.Web.UI.RadTreeNode In Me.RDTcommunityRepository.CheckedNodes Where CLng(oNode.Category) <= 0 Select CLng(oNode.Value)).ToList

        Return iResponse
    End Function
    Public Function GetSelectedItems() As List(Of Long)
        Dim iResponse As New List(Of Long)

        iResponse = (From oNode As Telerik.Web.UI.RadTreeNode In Me.RDTcommunityRepository.CheckedNodes Where oNode.Value > 0 Select CLng(oNode.Value)).ToList
        iResponse.AddRange((From oNode As Telerik.Web.UI.RadTreeNode In Me.RDTcommunityRepository.CheckedNodes Where oNode.Value <= 0 Select CLng(oNode.Value)).ToList)

        Return iResponse
    End Function

#Region "OLD"
    Public Function GetSelectedItemsStructureOld() As lm.Comol.Modules.Base.DomainModel.dtoFileFolder
        Dim iResponse As New lm.Comol.Modules.Base.DomainModel.dtoFileFolder
        iResponse.Id = 0
        iResponse.isVisible = False
        iResponse.Selected = False
        GenerateSelectedItemsStructureOld(iResponse)
        Return iResponse
    End Function
    Private Sub GenerateSelectedItemsStructureOld(ByVal oCommuntyRepository As lm.Comol.Modules.Base.DomainModel.dtoFileFolder)
        Dim oRootNode As Telerik.Web.UI.RadTreeNode = Me.RDTcommunityRepository.Nodes(0)


        For Each oNodeFile As Telerik.Web.UI.RadTreeNode In (From n As Telerik.Web.UI.RadTreeNode In oRootNode.Nodes Where n.Checked AndAlso n.Category > 0 Select n).ToList
            Dim oFile As New lm.Comol.Modules.Base.DomainModel.dtoGenericFile
            oFile.Id = CLng(oNodeFile.Value)
            oFile.Selected = True
            oFile.Name = oNodeFile.Text
            oCommuntyRepository.Files.Add(oFile)
        Next

        For Each oNodeFolder As Telerik.Web.UI.RadTreeNode In (From n As Telerik.Web.UI.RadTreeNode In oRootNode.Nodes Where n.Category <= 0 Select n).ToList
            If oNodeFolder.Checked Then
                Dim oFolder As New lm.Comol.Modules.Base.DomainModel.dtoFileFolder
                oFolder.Id = CLng(oNodeFolder.Value)
                oFolder.Selected = oNodeFolder.Selected
                oFolder.Name = oNodeFolder.Text
                oCommuntyRepository.SubFolders.Add(oFolder)
                RecursivelyGenerateSelectedItemsStructureOld(oFolder, oNodeFolder)
            Else
                RecursivelyGenerateSelectedItemsStructureOld(oCommuntyRepository, oNodeFolder)
            End If
        Next
    End Sub
    Private Sub RecursivelyGenerateSelectedItemsStructureOld(ByVal oFolder As lm.Comol.Modules.Base.DomainModel.dtoFileFolder, ByVal oFatherNode As Telerik.Web.UI.RadTreeNode)
        For Each oNodeFolder As Telerik.Web.UI.RadTreeNode In (From n As Telerik.Web.UI.RadTreeNode In oFatherNode.Nodes Where n.Category <= 0 Select n).ToList
            If oNodeFolder.Checked Then
                Dim oSubFolder As New lm.Comol.Modules.Base.DomainModel.dtoFileFolder
                oSubFolder.ID = CLng(oNodeFolder.Value)
                oSubFolder.Selected = oNodeFolder.Checked
                oSubFolder.Name = oNodeFolder.Text
                oFolder.SubFolders.Add(oSubFolder)
                RecursivelyGenerateSelectedItemsStructureOld(oSubFolder, oNodeFolder)
            Else
                RecursivelyGenerateSelectedItemsStructureOld(oFolder, oNodeFolder)
            End If
        Next


        For Each oNodeFile As Telerik.Web.UI.RadTreeNode In (From n As Telerik.Web.UI.RadTreeNode In oFatherNode.Nodes Where n.Checked AndAlso n.Category > 0 Select n).ToList
            Dim oFile As New lm.Comol.Modules.Base.DomainModel.dtoGenericFile
            oFile.ID = oNodeFile.Value
            oFile.Selected = True
            oFile.Name = oNodeFile.Text
            oFolder.Files.Add(oFile)
        Next
    End Sub
#End Region
 

#Region "NEW"
    Public Function GetSelectedItemsStructure() As dtoFileFolder
        Dim iResponse As New dtoFileFolder
        iResponse.Id = 0
        iResponse.isVisible = False
        iResponse.Selected = False
        GenerateSelectedItemsStructure(iResponse)
        Return iResponse
    End Function
    Private Sub GenerateSelectedItemsStructure(ByVal oCommuntyRepository As dtoFileFolder)
        Dim oRootNode As Telerik.Web.UI.RadTreeNode = Me.RDTcommunityRepository.Nodes(0)


        For Each oNodeFile As Telerik.Web.UI.RadTreeNode In (From n As Telerik.Web.UI.RadTreeNode In oRootNode.Nodes Where n.Checked AndAlso n.Category > 0 Select n).ToList
            Dim oFile As New dtoGenericFile
            oFile.Id = CLng(oNodeFile.Value)
            oFile.Selected = True
            oFile.Name = oNodeFile.Text
            oCommuntyRepository.Files.Add(oFile)
        Next

        For Each oNodeFolder As Telerik.Web.UI.RadTreeNode In (From n As Telerik.Web.UI.RadTreeNode In oRootNode.Nodes Where n.Category <= 0 Select n).ToList
            If oNodeFolder.Checked Then
                Dim oFolder As New dtoFileFolder
                oFolder.Id = CLng(oNodeFolder.Value)
                oFolder.Selected = oNodeFolder.Selected
                oFolder.Name = oNodeFolder.Text
                oCommuntyRepository.SubFolders.Add(oFolder)
                RecursivelyGenerateSelectedItemsStructure(oFolder, oNodeFolder)
            Else
                RecursivelyGenerateSelectedItemsStructure(oCommuntyRepository, oNodeFolder)
            End If
        Next
    End Sub
    Private Sub RecursivelyGenerateSelectedItemsStructure(ByVal oFolder As dtoFileFolder, ByVal oFatherNode As Telerik.Web.UI.RadTreeNode)
        For Each oNodeFolder As Telerik.Web.UI.RadTreeNode In (From n As Telerik.Web.UI.RadTreeNode In oFatherNode.Nodes Where n.Category <= 0 Select n).ToList
            If oNodeFolder.Checked Then
                Dim oSubFolder As New dtoFileFolder
                oSubFolder.Id = CLng(oNodeFolder.Value)
                oSubFolder.Selected = oNodeFolder.Checked
                oSubFolder.Name = oNodeFolder.Text
                oFolder.SubFolders.Add(oSubFolder)
                RecursivelyGenerateSelectedItemsStructure(oSubFolder, oNodeFolder)
            Else
                RecursivelyGenerateSelectedItemsStructure(oFolder, oNodeFolder)
            End If
        Next


        For Each oNodeFile As Telerik.Web.UI.RadTreeNode In (From n As Telerik.Web.UI.RadTreeNode In oFatherNode.Nodes Where n.Checked AndAlso n.Category > 0 Select n).ToList
            Dim oFile As New dtoGenericFile
            oFile.Id = oNodeFile.Value
            oFile.Selected = True
            oFile.Name = oNodeFile.Text
            oFolder.Files.Add(oFile)
        Next
    End Sub
#End Region
   
End Class