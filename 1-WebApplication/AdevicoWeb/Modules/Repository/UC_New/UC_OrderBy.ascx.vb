Imports lm.Comol.Core.FileRepository.Domain

Public Class UC_RepositoryOrderBy
    Inherits FRbaseControl

#Region "Internal"
    Private _OrderBy As OrderBy
    Private _ContainerCssClass As String
    Public Property ContainerCssClass() As String
        Get
            Return _ContainerCssClass
        End Get
        Set(value As String)
            _ContainerCssClass = value
        End Set
    End Property
    Public Event ChangeOrderBy(ByVal selected As OrderBy, ascending As Boolean)
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLabel(LBorderBySelectorDescription)
        End With
    End Sub
#End Region

#Region "Internal"
    Public Sub InitializeControl(items As List(Of OrderBy), current As OrderBy)
        _OrderBy = current
        If Not IsNothing(items) AndAlso items.Count > 1 Then
            Dim tItems As List(Of lm.Comol.Core.DomainModel.GenericOrderItem(Of OrderBy))
            tItems = items.Select(Function(i) New lm.Comol.Core.DomainModel.GenericOrderItem(Of OrderBy) With {.Item = i, .DisplayName = Resource.getValue("List.OrderBy." & i.ToString), .DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.item}).OrderBy(Function(i) i.DisplayName).ToList()
            Select Case tItems.Count
                Case 1
                    tItems(0).DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.first Or lm.Comol.Core.DomainModel.ItemDisplayOrder.last
                Case Else
                    tItems.FirstOrDefault().DisplayAs() = lm.Comol.Core.DomainModel.ItemDisplayOrder.first
                    tItems.LastOrDefault().DisplayAs() = lm.Comol.Core.DomainModel.ItemDisplayOrder.last
            End Select

            MLVcontent.SetActiveView(VIWcontent)
            RPTorderBy.DataSource = tItems
            RPTorderBy.DataBind()
            LBorderBySelected.Text = Resource.getValue("Selected.OrderBy." & current.ToString)
        Else
            MLVcontent.SetActiveView(VIWempty)
        End If
    End Sub
    Public Function ItemCssClass(item As lm.Comol.Core.DomainModel.GenericOrderItem(Of OrderBy)) As String
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
    Private Sub RPTorderBy_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTorderBy.ItemDataBound
        Dim oLinkButton As LinkButton = e.Item.FindControl("LNBorderItemsBy")
        Dim oItem As lm.Comol.Core.DomainModel.GenericOrderItem(Of OrderBy) = e.Item.DataItem

        oLinkButton.CommandArgument = CInt(oItem.Item)
        oLinkButton.Text = String.Format(oLinkButton.Text, oItem.DisplayName)

        Dim oControl As HtmlControl = e.Item.FindControl("DVitemOrderBy")
        oControl.Attributes("class") = LTcssClassOrderBy.Text & " " & ItemCssClass(oItem) & (IIf(_OrderBy = oItem.Item, LTcssClassActive.Text, ""))
    End Sub
    Private Sub RPTorderBy_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles RPTorderBy.ItemCommand
        Dim selected As OrderBy = CInt(e.CommandArgument)
        Dim ascending As Boolean = True
        Select Case selected
            Case OrderBy.date
                ascending = False
        End Select

        LBorderBySelected.Text = Resource.getValue("Selected.OrderBy." & selected.ToString)

        Dim oControl As HtmlControl = e.Item.FindControl("DVitemOrderBy")
        If Not oControl.Attributes("class").Contains(LTcssClassActive.Text) Then
            oControl.Attributes("class") = oControl.Attributes("class") & " " & LTcssClassActive.Text

            For Each row As RepeaterItem In (From r As RepeaterItem In RPTorderBy.Items Where r.ItemIndex <> e.Item.ItemIndex)
                oControl = row.FindControl("DVitemOrderBy")
                If oControl.Attributes("class").Contains(LTcssClassActive.Text) Then
                    oControl.Attributes("class") = Replace(oControl.Attributes("class"), LTcssClassActive.Text, "")
                    Exit For
                End If
            Next
        End If
        RaiseEvent ChangeOrderBy(selected, ascending)
    End Sub
#End Region
End Class