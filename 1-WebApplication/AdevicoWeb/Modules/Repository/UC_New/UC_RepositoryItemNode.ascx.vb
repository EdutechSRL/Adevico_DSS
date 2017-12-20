Imports lm.Comol.Core.FileRepository.Domain
Public Class UC_RepositoryItemNode
    Inherits FRbaseControl

#Region "Internal"
    Private _AutoOpenCssClass As String
    Public Property AutoOpenCssClass As String
        Get
            Return _AutoOpenCssClass
        End Get
        Set(value As String)
            _AutoOpenCssClass = value
        End Set
    End Property
    Private _CssClass As String
    Public Property CssClass As String
        Get
            Return _CssClass
        End Get
        Set(value As String)
            _CssClass = value
        End Set
    End Property
    Public Property EnableSelection As Boolean
        Get
            Return Not CBselect.Disabled
        End Get
        Set(value As Boolean)
            CBselect.Disabled = Not value
            CBselect.Visible = value
        End Set
    End Property
    Private _EnableFolderAutoNavigation As String
    Public Property EnableFolderAutoNavigation As String
        Get
            Return _EnableFolderAutoNavigation
        End Get
        Set(value As String)
            _EnableFolderAutoNavigation = value
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
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region

#Region "Internal"
    Public Sub InitializeControl(item As dtoNodeItem, ByVal order As OrderBy, ByVal ascending As Boolean, ByVal postBack As Boolean)
        InitializeControl(item, EnableSelection, EnableFolderAutoNavigation, order, ascending, postBack)
    End Sub
    Public Sub InitializeControl(item As dtoNodeItem, allowSelection As Boolean, allowFolderAutoNavigation As Boolean, ByVal order As OrderBy, ByVal ascending As Boolean, ByVal postBack As Boolean)
        AutoPostBack = postBack
        EnableSelection = allowSelection
        LTfoldername.Visible = False
        HYPfolder.Visible = False
        LNBfolder.Visible = False
        CBselect.Checked = item.IsCurrent

        
        LTanchor.Text = String.Format(LTanchor.Text, item.Id)
        Select Case item.ItemType
            Case ItemType.Folder
                LNBfolder.CommandName = item.Id
                LNBfolder.CommandArgument = item.IdentifierPath & "|" & item.FolderType.ToString
                If allowFolderAutoNavigation AndAlso Not String.IsNullOrWhiteSpace(item.TemplateNavigateUrl) Then
                    If postBack Then
                        LNBfolder.Visible = True
                        LNBfolder.Text = String.Format(HYPfolder.Text, item.Name)
                    Else
                        HYPfolder.NavigateUrl = BaseUrl & Replace(Replace(item.TemplateNavigateUrl, "#OrderBy#", order.ToString), "#Boolean#", ascending.ToString().ToLower)
                        HYPfolder.Visible = True
                        HYPfolder.Text = String.Format(HYPfolder.Text, item.Name)
                    End If
                Else
                    LTfoldername.Visible = True
                    LTfoldername.Text = String.Format(LTfoldername.Text, item.Name)
                End If
            Case Else
                LTfoldername.Visible = True
                LTfoldername.Text = String.Format(LTfoldername.Text, item.Name)
        End Select
        LTfolder.Text = item.TemplateNavigateUrl
    End Sub
    Public Sub HideItems()
        HYPfolder.Enabled = False
    End Sub
    Public Function SelectedCssClass() As String
        If CBselect.Checked Then
            Return " " & LTselectedItemCssClass.Text
        End If
        Return ""
    End Function
    Public Sub UpdateUrl(orderBy As OrderBy, ascending As Boolean)
        HYPfolder.NavigateUrl = BaseUrl & Replace(Replace(LTfolder.Text, "#OrderBy#", orderBy.ToString), "#Boolean#", ascending.ToString().ToLower)
    End Sub
    Private Sub LNBfolder_Click(sender As Object, e As EventArgs) Handles LNBfolder.Click
        Dim oLinkButton As LinkButton = DirectCast(sender, LinkButton)
        Dim idFolder As Long = 0
        Long.TryParse(oLinkButton.CommandName, idFolder)
        Dim items As List(Of String) = oLinkButton.CommandArgument.Split("|").ToList()
        RaiseEvent SelectedFolder(idFolder, items.FirstOrDefault(), lm.Comol.Core.DomainModel.Helpers.EnumParser(Of FolderType).GetByString(items.Last(), FolderType.standard))
    End Sub

    Public Sub UpdateSelection(idFolder As Long, path As String)
        Dim idItem As Long = Long.MinValue
        Long.TryParse(LNBfolder.CommandName, idItem)
        Dim pItem As String = LNBfolder.CommandArgument.Split("|").ToList().FirstOrDefault

        UpdateSelection(idItem = idFolder AndAlso (pItem = path OrElse (String.IsNullOrWhiteSpace(path) AndAlso String.IsNullOrWhiteSpace(pItem))))
    End Sub
    Public Sub UpdateSelection(selected As Boolean)
        CBselect.Checked = selected
    End Sub
#End Region

   
End Class