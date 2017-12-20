Imports lm.Comol.Modules.Base.DomainModel

Partial Public Class UC_RepositoryFolderPathSelector
    Inherits BaseControlSession

    Public Event FolderSelected(ByVal FolderID As Long)
    Public Event AjaxFolderSelected(ByVal sender As Object, ByVal e As System.EventArgs)

    Public Property AjaxEnabled() As Boolean
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
    Public Property UrlNavigation() As Boolean
        Get
            Dim iResponse As Boolean = False
            Try
                iResponse = CBool(Me.ViewState("UrlNavigation"))
            Catch ex As Exception
            End Try
            Return iResponse
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("UrlNavigation") = value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_CommunityRepository", "Generici")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(Me.LBpath_t)
        End With
    End Sub
    Public Sub InitializeControl(ByVal oList As List(Of FilterElement), ByVal UrlFormat As String)
        If Not Me.Page.IsPostBack Then
            SetInternazionalizzazione()
        End If
        Me.RPTpath.DataSource = (From o In oList Select New dtoFolderFilter(o.Value, o.Text, String.Format(UrlFormat, Me.BaseUrl, o.Value))).ToList
        Me.RPTpath.DataBind()
    End Sub

    Private Sub RPTpath_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTpath.ItemCommand
        If e.CommandName = "folder" Then
            If AjaxEnabled Then
                Dim Args As New FolderArgs With {.FolderID = CLng(e.CommandArgument)}
                RaiseEvent AjaxFolderSelected(source, Args)
            Else
                RaiseEvent FolderSelected(CLng(e.CommandArgument))
            End If
        End If
    End Sub

    Private Sub RPTpath_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTpath.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.Item Then
            Dim oDto As dtoFolderFilter = DirectCast(e.Item.DataItem, dtoFolderFilter)
            Dim oHyperlink As HyperLink = e.Item.FindControl("HYPfolder")
            Dim oLinkButton As LinkButton = e.Item.FindControl("LNBfolder")

            If UrlNavigation Then
                oHyperlink.NavigateUrl = oDto.Url
                oHyperlink.Text = oDto.Name
            Else
                oLinkButton.Text = oDto.Name
                oLinkButton.CommandArgument = oDto.ID
            End If
        End If
    End Sub
    Public Class FolderArgs
        Inherits System.EventArgs
        Public FolderID As Integer
    End Class
End Class