Imports lm.Comol.Core.FileRepository.Domain
Public Class UC_RepositorySize
    Inherits FRbaseControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim bItems As New List(Of StackedBarItem)
        'If totalSeats = dSubscriptions Then
        '    bItems.Add(New StackedBarItem() With {.CssClass = LTbusySeatsCssClass.Text, .Title = String.Format(Resource.getValue("ProgressTooltip.Seats.Busy"), totalSeats, "{0}"), .Value = 100})
        'ElseIf availableSeats > 0 Then
        '    bItems.Add(New StackedBarItem() With {.CssClass = LTbusySeatsCssClass.Text, .Title = String.Format(Resource.getValue("ProgressTooltip.Seats.Busy"), dSubscriptions, "{0}"), .Value = Percentual(dSubscriptions, totalSeats)})
        '    bItems.Add(New StackedBarItem() With {.CssClass = LTavailableSeatsCssClass.Text, .Title = String.Format(Resource.getValue("ProgressTooltip.Seats.Available"), availableSeats, "{0}"), .Value = 100 - bItems(0).Value})

        'Else
        '    bItems.Add(New StackedBarItem() With {.CssClass = LTbusySeatsCssClass.Text, .Title = String.Format(Resource.getValue("ProgressTooltip.Seats.Busy"), totalSeats, "{0}"), .Value = Percentual(totalSeats, dSubscriptions)})
        '    bItems.Add(New StackedBarItem() With {.CssClass = LTotherSeatsCssClass.Text, .Title = String.Format(Resource.getValue("ProgressTooltip.Seats.Over"), dSubscriptions - totalSeats, "{0}"), .Value = 100 - bItems(0).Value})
        'End If
        'CTRLprogressBar.InitializeControl(bItems)
    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region

#Region "Internal"
    Public Sub InitializeControl(items As List(Of dtoFolderSize))
        If Not IsNothing(items) AndAlso items.Any() AndAlso items.Any(Function(i) i.FolderType = FolderType.none AndAlso i.GetForProgressBar.Any()) Then
            Dim pItems As List(Of FolderSizeItem) = items.Where(Function(i) i.FolderType = FolderType.none).FirstOrDefault.GetForProgressBar()

            Dim busy As Integer = 0
            Dim busy5 As Integer = 0
            If pItems.Any(Function(i) i.Type = FolderSizeItemType.overflow AndAlso i.Size > 0) Then
                busy = 105
            Else
                busy = pItems.Where(Function(i) i.Type <> FolderSizeItemType.freespace).Select(Function(i) i.IntPercentage).Sum()
                busy5 = (busy \ 5) * 5
                Select Case busy Mod 5
                    Case 0, 1, 2
                        busy5 += 0
                    Case 3, 4, 5
                        busy5 += 5
                End Select
            End If

            CTRLprogressBar.ContainerCssClass = "busy" & busy5.ToString
            CTRLprogressBar.InitializeControl(pItems.Select(Function(i) New StackedBarItem() With {.Value = i.IntPercentage, .Title = String.Format(Resource.getValue("RepositorySize.ProgressTooltip.FolderSizeItemType." + i.Type.ToString()), "{0}", i.DisplaySize()), .CssClass = i.Type.ToString()}))
            CTRLprogressBar.Visible = True
        Else
            CTRLprogressBar.Visible = False
        End If

        RPTfolders.DataSource = items
        RPTfolders.DataBind()
    End Sub
    Private Sub RPTfolders_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTfolders.ItemDataBound
        Dim dto As dtoFolderSize = e.Item.DataItem
        Dim oLabel As Label = e.Item.FindControl("LBfolderDetailsTitle")
        oLabel.Text = Resource.getValue("RepositorySize.Title.FolderType." & dto.FolderType.ToString)

        If oLabel.Text.Contains("{0}") Then
            oLabel.Text = String.Format(oLabel.Text, dto.Name)
        End If
    End Sub
    Protected Sub RPTchildren_ItemDataBound(sender As Object, e As RepeaterItemEventArgs)
        Dim dto As FolderSizeItem = e.Item.DataItem
        Dim oLabel As Label = e.Item.FindControl("LBtext")
        oLabel.Text = Resource.getValue("RepositorySize.FolderSizeItemType." & dto.Type.ToString)

        oLabel = e.Item.FindControl("LBnumber")
        Select Case dto.Type
            Case FolderSizeItemType.folder, FolderSizeItemType.link
                oLabel.Text = dto.Number
                If dto.Size > 0 Then
                    oLabel.ToolTip = String.Format(Resource.getValue("RepositorySize.FolderSizeItemType.ToolTip"), dto.DisplaySize)
                End If
            Case FolderSizeItemType.fullSize
                oLabel.Text = dto.DisplaySize
                oLabel.ToolTip = String.Format(Resource.getValue("RepositorySize.FolderSizeItemType.ToolTip"), dto.DisplaySize)
            Case FolderSizeItemType.file, FolderSizeItemType.deleted
                oLabel.Text = dto.Number
                oLabel.ToolTip = String.Format(Resource.getValue("RepositorySize.FolderSizeItemType.ToolTip"), dto.DisplaySize)
            Case FolderSizeItemType.freespace
                oLabel.Text = String.Format(LTtemplateDisplayPercentage.Text, dto.Percentage.ToString("N2"), dto.DisplaySize)
            Case FolderSizeItemType.overflow, FolderSizeItemType.version, FolderSizeItemType.unavailableItems, FolderSizeItemType.deletedonsubfolders, FolderSizeItemType.onchildrenfolders
                oLabel.Text = dto.DisplaySize
                oLabel.ToolTip = String.Format(Resource.getValue("RepositorySize.FolderSizeItemType.ToolTip"), dto.DisplaySize)
        End Select
    End Sub
    Public Function ItemCssClass(item As dtoFolderSize) As String
        Select Case item.FolderType
            Case FolderType.none
                Return " " & LTtotalCssClass.Text
            Case Else
                Return " " & LTcurrentCssClass.Text
        End Select
        Return ""
    End Function
#End Region
End Class