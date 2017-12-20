Imports System.Linq
Imports lm.Comol.Core.FileRepository.Domain
Public Class UC_ItemMenu
    Inherits FRbaseControl

#Region "Internal"
    Public Event ItemCommand(ByVal idItem As Long, ByVal action As lm.Comol.Core.FileRepository.Domain.ItemAction)
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region

#Region "Internal"
    Public Sub InitializeControl(item As dtoDisplayRepositoryItem, actionsUrl As Dictionary(Of ItemAction, String), ByVal type As lm.Comol.Core.FileRepository.Domain.RepositoryType, ByVal idCommunity As Integer, order As OrderBy, ByVal ascending As Boolean)
        Dim actions As List(Of ItemAction) = item.Permissions.GetActions()
        Select Case item.Type
            Case ItemType.SharedDocument, ItemType.Folder, ItemType.Link
                actions.Remove(ItemAction.preview)
                actions.Remove(ItemAction.play)
                actions.Remove(ItemAction.download)
        End Select
        actions.Remove(ItemAction.setCurrentVersion)
        actions.Remove(ItemAction.removeVersion)
        If actions.Any Then
            Dim tItems As List(Of lm.Comol.Core.DomainModel.GenericOrderItem(Of dtoItemActionMenu))
            tItems = actions.Select(Function(i) New lm.Comol.Core.DomainModel.GenericOrderItem(Of dtoItemActionMenu) With {.Item = New dtoItemActionMenu(item.Id, i, Resource.getValue("Menu.ToolTip.ItemAction." & i.ToString), GetItemUrl(item, i, type, idCommunity, order, ascending), item.DisplayMode, item.Type), .DisplayName = Resource.getValue("Menu.ItemAction." & i.ToString), .DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.item}).OrderBy(Function(i) i.Item.Action).Where(Function(i) i.Item.IsValid).ToList()
            Select Case tItems.Count
                Case 1
                    tItems(0).DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.first Or lm.Comol.Core.DomainModel.ItemDisplayOrder.last
                Case Else
                    tItems.FirstOrDefault().DisplayAs() = lm.Comol.Core.DomainModel.ItemDisplayOrder.first
                    tItems.LastOrDefault().DisplayAs() = lm.Comol.Core.DomainModel.ItemDisplayOrder.last
            End Select

            MLVmenu.SetActiveView(VIWmenu)
            RPTactions.DataSource = tItems
            RPTactions.DataBind()
        Else
            MLVmenu.SetActiveView(VIWempty)
        End If
    End Sub
    Private Sub RPTactions_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTactions.ItemDataBound
        Dim dto As lm.Comol.Core.DomainModel.GenericOrderItem(Of dtoItemActionMenu) = e.Item.DataItem
        Dim oLinkButton As LinkButton = e.Item.FindControl("LNBaction")
        Dim oLiteral As Literal = e.Item.FindControl("LTpreviewItemAction")
        Dim oHyperlink As HyperLink = e.Item.FindControl("HYPaction")

        Select Case dto.Item.Action
            Case ItemAction.viewOtherStatistics, ItemAction.viewMyStatistics, ItemAction.download, ItemAction.edit, ItemAction.editPermission, ItemAction.editSettings, ItemAction.manageVersions, ItemAction.play, ItemAction.details
                oHyperlink.Visible = True
                oHyperlink.Text = String.Format(oHyperlink.Text, dto.DisplayName)
                oHyperlink.ToolTip = dto.Item.ToolTip
                oHyperlink.NavigateUrl = dto.Item.Url
                If String.IsNullOrEmpty(oHyperlink.NavigateUrl) Then
                    oHyperlink.Enabled = False
                End If
                If dto.Item.Action = ItemAction.play Then
                    If dto.Item.AsModal Then
                        oHyperlink.CssClass = LTmodalCssClass.Text & dto.Item.Type.ToString.ToLower
                    End If
                End If
            Case ItemAction.preview
                oLiteral.Visible = True
                oLiteral.Text = Replace(Replace(LTpreviewImages.Text, "#imageurl#", dto.Item.Url), "#text#", dto.DisplayName)

            Case Else
                oLinkButton.Visible = True
                oLinkButton.CommandName = CInt(dto.Item.Action)
                oLinkButton.CommandArgument = dto.Item.IdItem
                oLinkButton.Text = String.Format(oLinkButton.Text, dto.DisplayName)
                oLinkButton.ToolTip = dto.Item.ToolTip

                Dim controls As List(Of Literal) = Me.Controls.OfType(Of Literal)().ToList

                If controls.Any(Function(c) c.ID.Contains("LTclientScript" & dto.Item.Action.ToString)) Then
                    oLinkButton.OnClientClick = controls.Where(Function(c) c.ID.Contains("LTclientScript" & dto.Item.Action.ToString)).FirstOrDefault().Text
                End If
        End Select

        Dim oControl As HtmlControl = e.Item.FindControl("DVitemAction")
        oControl.Attributes("class") = LTcssClassItem.Text & " " & ItemCssClass(dto)
    End Sub
    Private Sub RPTactions_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles RPTactions.ItemCommand
        Dim idItem As Long = CLng(e.CommandArgument)
        Dim action As ItemAction = CInt(e.CommandName)

        RaiseEvent ItemCommand(idItem, action)
    End Sub
    Public Function ItemCssClass(item As lm.Comol.Core.DomainModel.GenericOrderItem(Of dtoItemActionMenu)) As String
        Select Case item.DisplayAs
            Case lm.Comol.Core.DomainModel.ItemDisplayOrder.item
                Return ""
            Case lm.Comol.Core.DomainModel.ItemDisplayOrder.first, lm.Comol.Core.DomainModel.ItemDisplayOrder.last
                Return " " & item.DisplayAs.ToString()
            Case Else
                Return " " & lm.Comol.Core.DomainModel.ItemDisplayOrder.first.ToString & " " & lm.Comol.Core.DomainModel.ItemDisplayOrder.last.ToString
        End Select

        Return ""
    End Function
#End Region

End Class

<Serializable> Public Class dtoItemActionMenu
    Public Action As ItemAction
    Public ToolTip As String
    Public Url As String
    Public IdItem As Long
    Public AsModal As Boolean
    Public Type As ItemType
    Public ReadOnly Property IsValid As Boolean
        Get
            Select Case Action
                Case ItemAction.viewOtherStatistics, ItemAction.viewMyStatistics, ItemAction.download, ItemAction.edit, ItemAction.editPermission, ItemAction.editSettings, ItemAction.manageVersions, ItemAction.play
                    Return Not String.IsNullOrWhiteSpace(Url)
                Case ItemAction.preview
                    Return Not String.IsNullOrWhiteSpace(Url)
                Case Else
                    Return True
            End Select
        End Get
    End Property
    Public Sub New(id As Long, iAction As ItemAction, iToolTip As String, iUrl As String, mode As DisplayMode, iType As ItemType)
        IdItem = id
        Action = iAction
        ToolTip = iToolTip
        Url = iUrl
        AsModal = (iAction = ItemAction.play AndAlso (mode = DisplayMode.downloadOrPlayOrModal OrElse mode = DisplayMode.inModal))
        Type = iType
    End Sub
End Class
