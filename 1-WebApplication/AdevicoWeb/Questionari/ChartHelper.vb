Imports Telerik.Web.UI
Imports Telerik.Web.UI.HtmlChart

Public NotInheritable Class ChartHelper

    Public Const NoCutLabel = -1
   
#Region "Bar Chart"
    Public Shared Function CreateBarChar( _
                              ByVal values As IList(Of Double), _
                              ByVal xAxisLabel As IList(Of String), _
                              ByVal isPercentage As Boolean, _
                              ByVal maxValue As Decimal, _
                              Optional ByVal maxLabelChar As Integer = 50, _
                              Optional ByVal chartTitle As String = "") As RadHtmlChart

        Dim oChart As RadHtmlChart = GetBarChart(chartTitle)
        Dim oSeries As New BarSeries()

        oSeries.Appearance.FillStyle.BackgroundColor = Color.FromArgb(69, 115, 166)
        oSeries.LabelsAppearance.Visible = True
        oSeries.LabelsAppearance.Position = BarColumnLabelsPosition.OutsideEnd

        If (isPercentage) Then
            oSeries.TooltipsAppearance.DataFormatString = "#%"
            oSeries.LabelsAppearance.DataFormatString = "#%"
            maxValue = 1
        End If

        'Dim averages As New List(Of Double)

        'For i As Integer = 1 To 6
        '    Dim val As Double = i * 10
        '    averages.Add(val)
        'Next

        Dim takeValuesforLabel As Boolean = IsNothing(xAxisLabel) OrElse values.Count() <> xAxisLabel.Count


        For Each val As Double In values

            oSeries.Items.Add(New SeriesItem(val))


            If (takeValuesforLabel) Then
                oChart.PlotArea.XAxis.Items.Add(val.ToString())
            End If

        Next

        If Not takeValuesforLabel Then
            For Each label As String In xAxisLabel
                Dim text As String = TxtHelper_HtmlCutToString(label, maxLabelChar)
                oChart.PlotArea.XAxis.Items.Add(text)
            Next
        End If

        oSeries.Spacing = 0.2D
        oSeries.Gap = 0.2D
        oChart.PlotArea.Series.Add(oSeries)
        oChart.PlotArea.YAxis.MaxValue = maxValue
        If (isPercentage) Then
            oChart.PlotArea.YAxis.LabelsAppearance.DataFormatString = "#%"
        End If
        oChart.Height = New Unit(String.Format("{0}px", 30 + values.Count * 30)) '30 is for yaxis label

        Return oChart
    End Function





    Public Shared Function GetBarChart(Optional chartTitle As String = "") As RadHtmlChart
        Dim oChart As New RadHtmlChart

        If Not String.IsNullOrEmpty(chartTitle) Then
            oChart.ChartTitle.Text = TxtHelper_HtmlCutToString(chartTitle, 50)
        Else
            oChart.ChartTitle.Appearance.Visible = False
        End If

        SetBarChar(oChart)

        Return oChart
    End Function

    Public Shared Sub SetBarChar(ByRef oChart As RadHtmlChart)

        ''orizzontale:
        oChart.PlotArea.YAxis.Visible = True
        oChart.PlotArea.YAxis.MajorGridLines.Visible = True
        oChart.PlotArea.YAxis.MinorGridLines.Visible = False
        oChart.PlotArea.YAxis.MinValue = 0

        ''verticale
        oChart.PlotArea.XAxis.Visible = True
        oChart.PlotArea.XAxis.MajorGridLines.Visible = False
        oChart.PlotArea.XAxis.TitleAppearance.Visible = False

        oChart.PlotArea.XAxis.MinorGridLines.Visible = False
        oChart.RenderMode = Telerik.Web.UI.RenderMode.Auto





        oChart.Width = New Unit("620px")
        'oChart.Height = New Unit("200px")


        'END HTML5

    End Sub

#End Region

#Region "Html e stringhe"

    ''' <summary>
    ''' Estra il testo piano da una stringa HTML
    ''' </summary>
    ''' <param name="html"></param>
    ''' <returns></returns>
    ''' <remarks>Usa l'Editor Telerick</remarks>
    Public Shared Function TxtHelper_HtmlToString(ByVal html As String) As String
        Dim oEditor As New RadEditor()
        oEditor.Content = html
        Return oEditor.Text
    End Function

    ''' <summary>
    ''' Tronca una stringa, prendendo i primi caratteri ed aggiungendo ... se maggiore della lunghezza indicata.
    ''' </summary>
    ''' <param name="text"></param>
    ''' <param name="numChar"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function TxtHelper_CutString(ByVal text As String, Optional ByVal numChar As Integer = 0) As String

        If (numChar = NoCutLabel) Then
            text = text.Replace(vbCrLf, "<br/>").Replace(vbCr, "<br/>").Replace(vbLf, "<br/>")
        Else
            text = text.Replace(vbCrLf, " ").Replace(vbCr, " ").Replace(vbLf, " ")
        End If

        If (numChar > 0) AndAlso text.Count() > numChar Then
            text = String.Format("{0}...", text.Substring(0, numChar))
        End If

        Return text
    End Function

    ''' <summary>
    ''' Estrae il testo piano da una stringa HTML prendendo i primi numchar caratteri.
    ''' </summary>
    ''' <param name="html"></param>
    ''' <param name="numChar"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function TxtHelper_HtmlCutToString(ByVal html As String, Optional ByVal numChar As Integer = 0) As String
        Return TxtHelper_CutString(TxtHelper_HtmlToString(html), numChar)
    End Function

#End Region



End Class
