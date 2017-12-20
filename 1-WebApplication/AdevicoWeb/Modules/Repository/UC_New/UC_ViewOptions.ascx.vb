Imports lm.Comol.Core.FileRepository.Domain
Public Class UC_ViewOptions
    Inherits FRbaseControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setHyperLink(HYPpresetTypeSimple, False, True)
            .setHyperLink(HYPpresetTypeAdvanced, False, True)
            .setHyperLink(HYPpresetTypeStandard, False, True)

            DIVcustomSettings.Attributes.Add("Title", .getValue("DIVcustomSettings.Title"))
        End With

    End Sub
#End Region

#Region "Internal"
    Public Sub InitializeControl(availableOptions As List(Of ViewOption), currentSet As PresetType, Optional presets As List(Of PresetType) = Nothing)
        InitializePresets(currentSet, presets)
        InitializeRepeater(availableOptions)
    End Sub
 
    Private Sub InitializePresets(currentSet As PresetType, presets As List(Of PresetType))
        If IsNothing(presets) Then
            HYPpresetTypeSimple.Visible = False
            HYPpresetTypeStandard.Visible = False
            HYPpresetTypeAdvanced.Visible = False
            DIVcustomSettings.Attributes("class") = LTtemplateCustomCssClass.Text & " " & LTtemplateActiveCssClass.Text
        Else
            HYPpresetTypeSimple.Visible = presets.Contains(PresetType.Simple)
            HYPpresetTypeStandard.Visible = presets.Contains(PresetType.Standard)
            HYPpresetTypeAdvanced.Visible = presets.Contains(PresetType.Advanced)
            DIVcustomSettings.Attributes("class") = LTtemplateCustomCssClass.Text
        End If
    End Sub
    Private Sub InitializeRepeater(availableOptions As List(Of ViewOption))
        Dim items As New List(Of lm.Comol.Core.DomainModel.GenericOrderItem(Of ViewOption))
        items = availableOptions.Where(Function(o) o <> ViewOption.AvailableSpace AndAlso o <> ViewOption.FolderPath AndAlso o <> ViewOption.NarrowWideView AndAlso o <> ViewOption.Tree).Select(Function(o) New lm.Comol.Core.DomainModel.GenericOrderItem(Of ViewOption) With {.DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.item, .Item = o, .DisplayName = Resource.getValue("Option.ViewOption." & o.ToString())}).OrderBy(Function(o) o.DisplayName).ToList()


        If availableOptions.Contains(ViewOption.NarrowWideView) Then
            items.Insert(0, New lm.Comol.Core.DomainModel.GenericOrderItem(Of ViewOption) With {.DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.first, .Item = ViewOption.NarrowWideView, .DisplayName = Resource.getValue("Option.ViewOption." & ViewOption.NarrowWideView.ToString)})
            If availableOptions.Contains(ViewOption.Tree) Then
                items.Insert(1, New lm.Comol.Core.DomainModel.GenericOrderItem(Of ViewOption) With {.DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.first, .Item = ViewOption.Tree, .DisplayName = Resource.getValue("Option.ViewOption." & ViewOption.Tree.ToString)})
            End If
        ElseIf availableOptions.Contains(ViewOption.Tree) Then
            items.Insert(0, New lm.Comol.Core.DomainModel.GenericOrderItem(Of ViewOption) With {.DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.first, .Item = ViewOption.Tree, .DisplayName = Resource.getValue("Option.ViewOption." & ViewOption.Tree.ToString)})
        End If
        Select Case items.Count
            Case 0
                Exit Select
            Case 1
                items.FirstOrDefault().DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.first Or lm.Comol.Core.DomainModel.ItemDisplayOrder.last
            Case Else
                items.LastOrDefault().DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.last
        End Select

        RPTviewOptions.DataSource = items
        RPTviewOptions.DataBind()
        DIVcustomSettings.Visible = items.Any()
    End Sub

    Private Sub RPTviewOptions_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTviewOptions.ItemDataBound
        Select Case e.Item.ItemType
            Case ListItemType.AlternatingItem, ListItemType.Item
                Dim obj As lm.Comol.Core.DomainModel.GenericOrderItem(Of ViewOption) = e.Item.DataItem
                Dim container As MultiView = e.Item.FindControl("MLVviewOption")

                Select Case obj.Item
                    Case ViewOption.Tree
                        Dim oControl As HtmlGenericControl = e.Item.FindControl("HRseparator")
                        oControl.Visible = (obj.DisplayAs <> lm.Comol.Core.DomainModel.ItemDisplayOrder.first Or lm.Comol.Core.DomainModel.ItemDisplayOrder.last)

                    Case ViewOption.NarrowWideView
                        container.ActiveViewIndex = 1
                        Dim oLabel As Label = e.Item.FindControl("LBwideView")
                        Resource.setLabel(oLabel)

                        oLabel = e.Item.FindControl("LBnarrowView")
                        Resource.setLabel(oLabel)

                    Case Else
                End Select
        End Select
    End Sub

    Public Function GetItemCssClass(ByVal obj As lm.Comol.Core.DomainModel.GenericOrderItem(Of ViewOption)) As String
        Dim cssClass As String = ""
        Select Case obj.DisplayAs
            Case lm.Comol.Core.DomainModel.ItemDisplayOrder.first, lm.Comol.Core.DomainModel.ItemDisplayOrder.last
                cssClass = " " & obj.DisplayAs.ToString
            Case lm.Comol.Core.DomainModel.ItemDisplayOrder.item
                cssClass = ""
            Case Else
                cssClass = " " & lm.Comol.Core.DomainModel.ItemDisplayOrder.first.ToString() & " " & lm.Comol.Core.DomainModel.ItemDisplayOrder.last.ToString()
        End Select
        Return cssClass
    End Function
    Public Function GetItemTypeCssClass(ByVal obj As lm.Comol.Core.DomainModel.GenericOrderItem(Of ViewOption)) As String
        Select Case obj.Item
            Case ViewOption.Extrainfo
                Return "info"
            Case ViewOption.Statistics
                Return "stats"
            Case Else
                Return obj.Item.ToString().ToLower()
        End Select
    End Function


#End Region


End Class