Imports lm.Comol.Core.FileRepository.Domain
Public Class UC_FoldersBreadcrumb
    Inherits FRbaseControl

#Region "Internal"
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
    Private Property isSingleItem As Boolean
        Get
            Return ViewStateOrDefault("isSingleItem", False)
        End Get
        Set(value As Boolean)
            ViewState("isSingleItem") = value
        End Set
    End Property
    Private Property isRoot As Boolean
        Get
            Return ViewStateOrDefault("isRoot", False)
        End Get
        Set(value As Boolean)
            ViewState("isRoot") = value
        End Set
    End Property
    Private Property CurrentOrderBy As OrderBy
    Private Property CurrentAscending As Boolean
    Public Event SelectedFolder(idFolder As Long, path As String, type As FolderType)
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setHyperLink(HYProotFolder, False, True)
            HYProotFolder.Text = String.Format(LTrootText.Text, HYProotFolder.Text)
            .setLinkButton(LNBrootFolder, False, True)
            LNBrootFolder.Text = String.Format(LTrootText.Text, LNBrootFolder.Text)
        End With
    End Sub
#End Region

#Region "Internal"
    Public Sub InitializeControl(folders As List(Of dtoFolderItem), rootUrl As String, order As OrderBy, ascending As Boolean)
        CurrentOrderBy = order
        CurrentAscending = ascending
        HYProotFolder.NavigateUrl = BaseUrl & rootUrl
        HYProotFolder.Visible = Not _AutoPostBack
        LNBrootFolder.Visible = _AutoPostBack

        MLVcontent.SetActiveView(VIWbreadCrumb)
        If IsNothing(folders) OrElse Not folders.Any() Then
            isRoot = True
            RPTfolders.DataBind()
        Else
            isRoot = False
            MLVcontent.SetActiveView(VIWbreadCrumb)
            Dim items As New List(Of lm.Comol.Core.DomainModel.GenericOrderItem(Of dtoFolderItem))
            items = folders.Select(Function(o) New lm.Comol.Core.DomainModel.GenericOrderItem(Of dtoFolderItem) With {.DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.item, .Item = o, .DisplayName = o.Name}).ToList()
            If items.Count > 1 Then
                isSingleItem = False
                items.FirstOrDefault().DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.first
            Else
                isSingleItem = True
            End If
            items.LastOrDefault().DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.last
            RPTfolders.DataSource = items
            RPTfolders.DataBind()
        End If
    End Sub
    Private Sub RPTfolders_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTfolders.ItemDataBound
        Dim dto As lm.Comol.Core.DomainModel.GenericOrderItem(Of dtoFolderItem) = e.Item.DataItem
        Dim oHyperLink As HyperLink = e.Item.FindControl("HYPfolder")
        Dim oLinkButton As LinkButton = e.Item.FindControl("LNBfolder")
        oLinkButton.CommandName = dto.Item.Id
        oLinkButton.CommandArgument = dto.Item.IdentifierPath & "|" & dto.Item.FolderType.ToString

        If _AutoPostBack Then
            oLinkButton.Visible = True
            oHyperLink.Visible = False
        Else
            oHyperLink.NavigateUrl = BaseUrl & Replace(Replace(dto.Item.TemplateUrl, "#OrderBy#", CurrentOrderBy.ToString), "#Boolean#", CurrentAscending.ToString().ToLower)
            oLinkButton.Visible = False
            oHyperLink.Visible = True
        End If
    End Sub

    Protected Friend Sub LNBrootFolder_Click(sender As Object, e As EventArgs) Handles LNBrootFolder.Click
        Dim oLinkButton As LinkButton = DirectCast(sender, LinkButton)
        Dim idFolder As Long = 0
        Long.TryParse(oLinkButton.CommandName, idFolder)
        Dim items As List(Of String) = oLinkButton.CommandArgument.Split("|").ToList()
        RaiseEvent SelectedFolder(idFolder, items.FirstOrDefault(), lm.Comol.Core.DomainModel.Helpers.EnumParser(Of FolderType).GetByString(items.Last(), FolderType.standard))
    End Sub

#Region "Styles"
    Public Function ContainerCssClass() As String
        If isRoot Then
            Return " " & LTrootCssClass.Text
        End If
        If isSingleItem Then
            Return " " & LTsingleCssClass.Text
        End If
        Return ""
    End Function
    Public Function ContainerRootCssClass() As String
        If isRoot Then
            Return " " & lm.Comol.Core.DomainModel.ItemDisplayOrder.last.ToString
        End If
        Return ""
    End Function
    Public Function ItemCssClass(item As lm.Comol.Core.DomainModel.GenericOrderItem(Of dtoFolderItem), isSeparator As Boolean, Optional ByVal index As Integer = -1) As String
        If isSeparator Then
            Select Case item.DisplayAs
                Case lm.Comol.Core.DomainModel.ItemDisplayOrder.item
                    Return ""
                Case lm.Comol.Core.DomainModel.ItemDisplayOrder.last
                    Return IIf(index = 0, " " & LTrootCssClass.Text, "")
                Case lm.Comol.Core.DomainModel.ItemDisplayOrder.first
                    Return " " & item.DisplayAs.ToString & IIf(index = 0, " " & LTrootCssClass.Text, "")
                Case Else
                    Return " " & item.DisplayAs.ToString & IIf(index = 0, " " & LTrootCssClass.Text, "")
            End Select
        ElseIf isSingleItem Then
            Return " " & lm.Comol.Core.DomainModel.ItemDisplayOrder.last.ToString
        Else
            Select Case item.DisplayAs
                Case lm.Comol.Core.DomainModel.ItemDisplayOrder.last
                    Return " " & item.DisplayAs.ToString
                Case Else
                    Return ""
            End Select
        End If
        Return ""
    End Function
    Public Function ChildrenCssClass(child As lm.Comol.Core.DomainModel.GenericOrderItem(Of dtoFolderItem)) As String
        Dim cssClass As String = ""
        Select Case child.DisplayAs
            Case lm.Comol.Core.DomainModel.ItemDisplayOrder.first
                cssClass = " " & child.DisplayAs.ToString
            Case lm.Comol.Core.DomainModel.ItemDisplayOrder.last
                cssClass = " " & child.DisplayAs.ToString
            Case lm.Comol.Core.DomainModel.ItemDisplayOrder.item
            Case Else
                cssClass = " " & lm.Comol.Core.DomainModel.ItemDisplayOrder.first.ToString & " " & lm.Comol.Core.DomainModel.ItemDisplayOrder.last.ToString
        End Select
        If child.Item.IsInCurrentPath Then
            cssClass &= " " & "selected"
        End If
        Return cssClass
    End Function
#End Region
    Public Function GetTranslationSeeFolder() As String
        Return Resource.getValue("TranslationSeeOtherFolder")
    End Function
    Public Sub UpdateItemsUrl(orderBy As OrderBy, ascending As Boolean)
        CurrentOrderBy = orderBy
        CurrentAscending = ascending
    End Sub
#End Region

   
End Class