Imports lm.Comol.Core.FileRepository.Domain
Imports lm.Comol.Core.BaseModules.FileRepository.Presentation
Public Class UC_FolderSelector
    Inherits FRbaseControl
    Implements IViewFolderSelector

#Region "Context"
    Private _Presenter As FolderSelectorPresenter
    Public ReadOnly Property CurrentPresenter() As FolderSelectorPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New FolderSelectorPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public ReadOnly Property IdSelectedFolder As Long Implements IViewFolderSelector.IdSelectedFolder
        Get
            If String.IsNullOrWhiteSpace(HDNselectedIdFolder.Value) Then
                Return -1
            Else
                Return CLng(HDNselectedIdFolder.Value)
            End If
        End Get
    End Property
    Public Property AutoPostBack As Boolean Implements IViewFolderSelector.AutoPostBack
        Get
            Return ViewStateOrDefault("AutoPostBack", False)
        End Get
        Set(value As Boolean)
            ViewState("AutoPostBack") = value
            If value Then
                HDNselectedIdFolder.Attributes("class") = "changetrigger"
            Else
                HDNselectedIdFolder.Attributes("class") = ""
            End If
        End Set
    End Property
    Public Property AlsoSelectedQuota As Boolean Implements IViewFolderSelector.AlsoSelectedQuota
        Get
            Return ViewStateOrDefault("AlsoSelectedQuota", True)
        End Get
        Set(value As Boolean)
            ViewState("AlsoSelectedQuota") = value
        End Set
    End Property
    Public Property RepositoryIdCommunity As Integer Implements IViewFolderSelector.RepositoryIdCommunity
        Get
            Return ViewStateOrDefault("RepositoryIdCommunity", 0)
        End Get
        Set(value As Integer)
            ViewState("RepositoryIdCommunity") = value
        End Set
    End Property
    Public Property RepositoryType As RepositoryType Implements IViewFolderSelector.RepositoryType
        Get
            Return ViewStateOrDefault("RepositoryType", lm.Comol.Core.FileRepository.Domain.RepositoryType.Community)
        End Get
        Set(value As RepositoryType)
            ViewState("RepositoryType") = value
        End Set
    End Property

#End Region

#Region "Internal"
   
    Public Event SelectFolder(ByVal idFolder As Long)
    Public Event SelectFolderWithQuota(ByVal idFolder As Long, quota As dtoContainerQuota)

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(type As RepositoryType, idCommunity As Integer) Implements IViewFolderSelector.InitializeControl
        RepositoryIdCommunity = idCommunity
        RepositoryType = type
        InitializeRepositoryPath(type, idCommunity)
        CurrentPresenter.InitView(type, idCommunity)
    End Sub
    Public Sub InitializeControl(type As RepositoryType, idCommunity As Integer, idFolder As Long, folderName As String, folders As List(Of dtoNodeFolderItem)) Implements IViewFolderSelector.InitializeControl
        RepositoryIdCommunity = idCommunity
        RepositoryType = type
        InitializeRepositoryPath(type, idCommunity)
        InitializeControl(idFolder, folderName, folders)
    End Sub
    Public Sub InitializeControl(idFolder As Long, folderName As String, folders As List(Of dtoNodeFolderItem)) Implements IViewFolderSelector.InitializeControl
        If idFolder >= 0 Then
            HDNselectedIdFolder.Value = idFolder
            LBselectFolderPath.Text = folderName
        Else
            HDNselectedIdFolder.Value = ""
            LBselectFolderPath.Text = ""
        End If

        RPTchildren.DataSource = folders
        RPTchildren.DataBind()
    End Sub
#End Region

#Region "Internal"
    Public Function ItemCssClass(item As dtoNodeFolderItem) As String
        Dim cssClass As String = ""
        If item.Selected Then
            cssClass = LTselectedItemCssClass.Text
            If Not item.Selectable Then
                cssClass &= " " & LTunselectableItemCssClass.Text
            End If
        ElseIf Not item.Selectable Then
            cssClass = LTunselectableItemCssClass.Text
        End If
        Return " " & cssClass
    End Function

    Private Sub RPTchildren_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTchildren.ItemDataBound
        Dim item As dtoNodeFolderItem = e.Item.DataItem
        Dim oMultiView As MultiView = e.Item.FindControl("MLVnode")
        Dim oView As View = e.Item.FindControl("VIWtype" & item.Type.ToString)
        Select Case item.Type
            Case NodeType.OpenItemNode
                'Dim oLiteral As Literal = Nothing
                'Dim identifier As String = item.Id.ToString & "-" & item.IdCommunity.ToString()
                'oLiteral = e.Item.FindControl("LTnode" & item.Type.ToString)

                'If oLiteral.Text.Contains("{0}") Then
                '    Dim cssClass As String = ""
                '    Select Case item.FolderType
                '        Case FolderType.none, FolderType.standard
                '        Case Else
                '            If Not DisplaySeparator Then
                '                DisplaySeparator = True
                '                Dim control As HtmlGenericControl = e.Item.FindControl("LIseparator")
                '                control.Visible = True
                '            End If
                '            Select Case item.FolderType
                '                Case FolderType.links
                '                    cssClass = " " & LTtemplateFolderTypelinksCssClass.Text
                '                Case FolderType.recycleBin
                '                    cssClass = " " & LTtemplateFolderTyperecycleBinCssClass.Text
                '                Case FolderType.module
                '                    cssClass = " " & LTtemplateFolderTypemoduleCssClass.Text
                '                Case FolderType.modules
                '                    cssClass = " " & LTtemplateFolderTypemodulesCssClass.Text
                '                Case FolderType.tags
                '                    cssClass = " " & LTtemplateFolderTypetagCssClass.Text
                '                Case FolderType.tags
                '                    cssClass = " " & LTtemplateFolderTypetagsCssClass.Text
                '            End Select

                '            If item.IsEmpty Then
                '                cssClass = " " & LTemptyItemCssClass.Text
                '            End If
                '    End Select
                '    If AutoOpenFolder Then
                '        cssClass &= " " & LTtreeAutoOpenCssClass.Text
                '    ElseIf item.HasCurrent Then
                '        cssClass &= " " & LTtreeAutoOpenCssClass.Text
                '        'cssClass &= " " & LTtreeKeepAutoOpenCssClass.Text
                '    End If
                '    If item.IsCurrent Then
                '        cssClass &= " " & LTselectedItemCssClass.Text
                '    End If
                '    oLiteral.Text = String.Format(oLiteral.Text, cssClass, identifier)
                'End If
            Case NodeType.Item
                Dim oHyperlink As HyperLink = e.Item.FindControl("HYPfolder")
                oHyperlink.CssClass = LTitemCssClass.Text & ItemCssClass(item)
                'oControl.InitializeControl(item, EnableSelection, EnableFolderAutoNavigation, CurrentOrderBy, CurrentAscending)
        End Select
        oMultiView.SetActiveView(oView)
    End Sub
    Private Sub BTNupdate_Click(sender As Object, e As EventArgs) Handles BTNupdate.Click
        Dim idSelected = IdSelectedFolder
        For Each row As RepeaterItem In RPTchildren.Items
            Dim oMultiView As MultiView = row.FindControl("MLVnode")
            Dim oView As View = oMultiView.GetActiveView
            If Not IsNothing(oView) AndAlso oView.ID = "VIWtype" & NodeType.Item.ToString Then
                Dim oLiteral As Literal = row.FindControl("LTidFolder")
                Dim oHyperlink As HyperLink = row.FindControl("HYPfolder")
                Dim idFolder As Long = CInt(oLiteral.Text)
                If idSelected = idFolder Then
                    If idSelected > 0 Then
                        LBselectFolderPath.Text = DirectCast(row.FindControl("LTname"), Literal).Text
                    End If
                    If Not oHyperlink.CssClass.Contains(LTselectedItemCssClass.Text) Then
                        oHyperlink.CssClass = oHyperlink.CssClass & " " & LTselectedItemCssClass.Text
                    End If
                Else
                    oHyperlink.CssClass = Replace(oHyperlink.CssClass, " " & LTselectedItemCssClass.Text, "")
                End If
            End If
        Next
        If idSelected = 0 Then
            LBselectFolderPath.Text = Resource.getValue("RootFolder")
        End If
        If AlsoSelectedQuota Then
            RaiseEvent SelectFolderWithQuota(idSelected, CurrentPresenter.GetFolderQuota(GetRepositoryDiskPath, idSelected, RepositoryType, RepositoryIdCommunity))
        Else
            RaiseEvent SelectFolder(idSelected)
        End If
    End Sub
#End Region

End Class