Imports Telerik.Charting

Public Class UC_ProgressBar
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub



    Public Sub InitBar(ByVal progress As Int16, ByVal minProgress As Int16, ByVal itemLabel As String) ', ByVal width As Double)

        'RDpathBar.Width = New Unit(width)

        RDpathBar.Series.Clear()
        Const lineWidth As Decimal = 0.3
        Const maxBarLenght As Decimal = 100
        Dim serieMinProgress As New ChartSeries
        serieMinProgress.Type = ChartSeriesType.StackedBar100
        serieMinProgress.Appearance.FillStyle.MainColor = Drawing.Color.Black
        serieMinProgress.AddItem(lineWidth)

        Dim serieExtra As New ChartSeries
        serieExtra.Type = ChartSeriesType.StackedBar100
        serieExtra.DefaultLabelValue = ""
        serieExtra.Appearance.FillStyle.FillType = Styles.FillType.Solid

        Dim serie As New ChartSeries
        serie.Type = ChartSeriesType.StackedBar100
        serie.Appearance.FillStyle.MainColor = Color.FromArgb(168, 184, 90)
        'serie.Appearance.FillStyle.MainColor = Color.Green
        serie.Appearance.LabelAppearance.LabelLocation = Styles.StyleSeriesItemLabel.ItemLabelLocation.Inside
        serie.Appearance.LabelAppearance.Dimensions.Width = Styles.Unit.Percentage(40)
        serie.SetItemLabel(0, itemLabel)
        Dim serie2 As New ChartSeries
        serie2.Type = ChartSeriesType.StackedBar100
        serie2.DefaultLabelValue = ""
        serie2.Appearance.FillStyle.MainColor = Drawing.Color.LightGray
        serie2.Appearance.FillStyle.SecondColor = Drawing.Color.White
        serie2.Appearance.FillStyle.FillSettings.HatchStyle = Drawing2D.HatchStyle.ForwardDiagonal
        serie2.Appearance.FillStyle.FillType = Styles.FillType.Hatch

        RDpathBar.PlotArea.YAxis.AxisLabel.TextBlock.Text = "%"
        RDpathBar.PlotArea.YAxis.AxisLabel.TextBlock.Appearance.TextProperties.Color = Drawing.Color.Black
        RDpathBar.PlotArea.YAxis.AxisLabel.Visible = True
        RDpathBar.PlotArea.XAxis.AddRange(1, 1, 1)
        RDpathBar.PlotArea.XAxis(0).TextBlock.Text = " "

        If minProgress > progress Then
            Dim progressLack As Int16 = minProgress - progress
            serie.AddItem(progress)
            serieMinProgress.SetItemLabel(0, minProgress & "%")
            serie2.AddItem(Math.Max(progressLack - lineWidth, 0))
            serieExtra.AddItem(Math.Max(maxBarLenght - minProgress, 0))
            serieExtra.Appearance.FillStyle.MainColor = serie2.Appearance.FillStyle.MainColor
            serieExtra.Appearance.FillStyle.FillType = serie2.Appearance.FillStyle.FillType
            serieExtra.Appearance.FillStyle.FillSettings.HatchStyle = serie2.Appearance.FillStyle.FillSettings.HatchStyle
            serieExtra.Appearance.FillStyle.SecondColor = serie2.Appearance.FillStyle.SecondColor
            Me.RDpathBar.Series.Add(serie)
            Me.RDpathBar.Series.Add(serie2)
            Me.RDpathBar.Series.Add(serieMinProgress)
            Me.RDpathBar.Series.Add(serieExtra)

        ElseIf minProgress = progress Then
            serie.AddItem(Math.Max(minProgress, 0)) '  serie.AddItem(Math.Max(minProgress - lineWidth, 0))
            serie.Appearance.FillStyle.FillType = Styles.FillType.Solid

            serieExtra.AddItem(progress - minProgress)
            serieExtra.Appearance.FillStyle.MainColor = serie.Appearance.FillStyle.MainColor
            serieExtra.Appearance.FillStyle.FillType = serie.Appearance.FillStyle.FillType
            serieExtra.SetItemLabel(0, progress)

            serieMinProgress.SetItemLabel(0, minProgress & "%")

            serie2.AddItem(Math.Max(maxBarLenght - progress, 0))
            Me.RDpathBar.Series.Add(serie)
            Me.RDpathBar.Series.Add(serieMinProgress)
            Me.RDpathBar.Series.Add(serieExtra)
            Me.RDpathBar.Series.Add(serie2)

        Else
            serie.AddItem(Math.Max(minProgress, 0))
            serie.SetItemLabel(serie.Items.Count - 1, " ")
            serie.Appearance.FillStyle.FillType = Styles.FillType.Solid

            serieExtra.AddItem(progress - minProgress)
            serieExtra.Appearance.FillStyle.MainColor = serie.Appearance.FillStyle.MainColor
            serieExtra.Appearance.FillStyle.FillType = serie.Appearance.FillStyle.FillType
            serieExtra.SetItemLabel(0, progress & "%")

            serie2.AddItem(Math.Max(maxBarLenght - progress, 0))

            serieMinProgress.SetItemLabel(0, minProgress & "%")

            Me.RDpathBar.Series.Add(serie)
            Me.RDpathBar.Series.Add(serieMinProgress)
            Me.RDpathBar.Series.Add(serieExtra)
            Me.RDpathBar.Series.Add(serie2)

        End If

    End Sub

End Class