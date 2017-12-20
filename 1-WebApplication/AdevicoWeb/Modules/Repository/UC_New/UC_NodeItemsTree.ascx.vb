Imports lm.Comol.Core.FileRepository.Domain
Public Class UC_NodeItemsTree
    Inherits FRbaseControl

#Region "Internal"
    Private _EnableFolderAutoNavigation As Boolean
    Private _EnableSelection As Boolean
    Private _AutoOpenFolder As Boolean
    Public Property AutoOpenFolder As Boolean
        Get
            Return _AutoOpenFolder
        End Get
        Set(value As Boolean)
            _AutoOpenFolder = value
        End Set
    End Property
    Public Property EnableSelection As Boolean
        Get
            Return _EnableSelection
        End Get
        Set(value As Boolean)
            _EnableSelection = value
        End Set
    End Property

    Public Property EnableFolderAutoNavigation As Boolean
        Get
            Return _EnableFolderAutoNavigation
        End Get
        Set(value As Boolean)
            _EnableFolderAutoNavigation = value
        End Set
    End Property
    Private DisplaySeparator As Boolean

    Public Property CurrentOrderBy As OrderBy
        Get
            Return ViewStateOrDefault("CurrentOrderBy", OrderBy.name)
        End Get
        Set(value As OrderBy)
            ViewState("CurrentOrderBy") = value
        End Set
    End Property
    Public Property CurrentAscending As Boolean
        Get
            Return ViewStateOrDefault("CurrentAscending", True)
        End Get
        Set(value As Boolean)
            ViewState("CurrentAscending") = value
        End Set
    End Property
    Private _AutoPostBack As Boolean
    Public Property AutoPostBack As Boolean
        Get
            Return ViewStateOrDefault("AutoPostBack", False)
        End Get
        Set(value As Boolean)
            _AutoPostBack = value
            ViewState("AutoPostBack") = value
        End Set
    End Property
    Public Event SelectedFolder(idFolder As Long, path As String, type As FolderType)

    Private _RepositoryIdentifier As String
    Public Property RepositoryIdentifier As String
        Get
            If Not String.IsNullOrWhiteSpace(_RepositoryIdentifier) Then
                Return _RepositoryIdentifier
            Else
                Return ViewStateOrDefault("RepositoryIdentifier", "")
            End If
        End Get
        Set(value As String)
            _RepositoryIdentifier = value
            ViewState("RepositoryIdentifier") = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region

#Region "Internal"
    Public Sub InitializeControl(items As List(Of dtoNodeItem), identifier As String)
        RepositoryIdentifier = identifier
        RPTchildren.DataSource = items
        RPTchildren.DataBind()
    End Sub
    Public Sub InitializeControl(items As List(Of dtoNodeItem), order As OrderBy, ascending As Boolean, identifier As String)
        CurrentOrderBy = order
        CurrentAscending = ascending
        InitializeControl(items, identifier)
    End Sub
    Private Sub RPTchildren_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTchildren.ItemDataBound
        Dim item As dtoNodeItem = e.Item.DataItem
        Dim oMultiView As MultiView = e.Item.FindControl("MLVnode")
        Dim oView As View = e.Item.FindControl("VIWtype" & item.Type.ToString)
        Select Case item.Type
            Case NodeType.OpenItemNode
                Dim oLiteral As Literal = Nothing
                Dim identifier As String = item.Id.ToString & "-"
                If Not String.IsNullOrWhiteSpace(RepositoryIdentifier) Then
                    identifier &= RepositoryIdentifier
                Else
                    identifier &= item.IdCommunity.ToString()
                End If


                oLiteral = e.Item.FindControl("LTnode" & item.Type.ToString)

                If oLiteral.Text.Contains("{0}") Then
                    Dim cssClass As String = ""
                    Select Case item.FolderType
                        Case FolderType.none, FolderType.standard
                        Case Else
                            If Not DisplaySeparator Then
                                DisplaySeparator = True
                                Dim control As HtmlGenericControl = e.Item.FindControl("LIseparator")
                                control.Visible = True
                            End If
                            Select Case item.FolderType
                                Case FolderType.recycleBin
                                    cssClass = " " & LTtemplateFolderTyperecycleBinCssClass.Text
                            End Select

                            If item.IsEmpty Then
                                cssClass = " " & LTemptyItemCssClass.Text
                            End If
                    End Select
                    If AutoOpenFolder Then
                        cssClass &= " " & LTtreeAutoOpenCssClass.Text
                    ElseIf item.HasCurrent Then
                        cssClass &= " " & LTtreeAutoOpenCssClass.Text
                        'cssClass &= " " & LTtreeKeepAutoOpenCssClass.Text
                    End If
                    If item.IsCurrent Then
                        cssClass &= " " & LTselectedItemCssClass.Text
                    End If
                    oLiteral.Text = String.Format(oLiteral.Text, cssClass, identifier)
                End If
            Case NodeType.Item
                Dim oControl As UC_RepositoryItemNode = DirectCast(e.Item.FindControl("CTRLnode"), UC_RepositoryItemNode)
                oControl.InitializeControl(item, EnableSelection, EnableFolderAutoNavigation, CurrentOrderBy, CurrentAscending, _AutoPostBack)

        End Select
        oMultiView.SetActiveView(oView)
    End Sub
    Public Sub UpdateItemsUrl(orderBy As OrderBy, ascending As Boolean)
        CurrentOrderBy = orderBy
        CurrentAscending = ascending

        For Each row As RepeaterItem In RPTchildren.Items
            Dim oMultiView As MultiView = row.FindControl("MLVnode")
            Dim oView As View = row.FindControl("VIWtype" & NodeType.Item.ToString)
            If oMultiView.GetActiveView() Is oView Then
                Dim oControl As UC_RepositoryItemNode = DirectCast(oView.FindControl("CTRLnode"), UC_RepositoryItemNode)
                If Not IsNothing(oControl) Then
                    oControl.UpdateUrl(orderBy, ascending)
                End If
            End If
        Next
    End Sub
    Public Sub UpdateSelectedFolder(idFolder As Long, path As String)
        For Each row As RepeaterItem In RPTchildren.Items
            Dim oMultiView As MultiView = row.FindControl("MLVnode")
            Dim oView As View = row.FindControl("VIWtype" & NodeType.Item.ToString)
            If oMultiView.GetActiveView() Is oView Then
                Dim oControl As UC_RepositoryItemNode = DirectCast(oView.FindControl("CTRLnode"), UC_RepositoryItemNode)
                If Not IsNothing(oControl) Then
                    oControl.UpdateSelection(idFolder, path)
                End If
            End If
        Next
    End Sub
    Protected Sub ItemSelectedFolder(idFolder As Long, path As String, type As FolderType)
        RaiseEvent SelectedFolder(idFolder, path, type)
    End Sub
#End Region

End Class