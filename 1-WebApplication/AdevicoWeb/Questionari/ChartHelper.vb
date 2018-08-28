Imports Telerik.Web.UI
Imports Telerik.Web.UI.HtmlChart

Public NotInheritable Class ChartHelper

    Public Const NoCutLabel = -1

#Region "Bar Chart"
    Public Shared Function CreateBarChar(
                              ByVal values As IList(Of Double),
                              ByVal xAxisLabel As IList(Of String),
                              ByVal isPercentage As Boolean,
                              ByVal maxValue As Decimal,
                              ByVal maxChar As Integer,
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
        Else
            oSeries.TooltipsAppearance.DataFormatString = "N1"
            oSeries.LabelsAppearance.DataFormatString = "N1"
        End If

        'Dim averages As New List(Of Double)

        'For i As Integer = 1 To 6
        '    Dim val As Double = i * 10
        '    averages.Add(val)
        'Next

        Dim takeValuesforLabel As Boolean = IsNothing(xAxisLabel) OrElse values.Count() <> xAxisLabel.Count


        For Each val As Double In values
            If (Double.IsNaN(val)) Then
                val = 0
            End If

            oSeries.Items.Add(New SeriesItem(val))

            Try
                If (takeValuesforLabel) Then
                    oChart.PlotArea.XAxis.Items.Add(val.ToString())
                End If
            Catch ex As Exception

            End Try


        Next

        If Not takeValuesforLabel Then
            For Each label As String In xAxisLabel
                Dim text As String = TxtHelper_HtmlCutToString(label, maxChar)
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
        oChart.Height = New Unit(String.Format("{0}px", 30 + values.Count * 40)) '30 is for yaxis label

        Return FlatterChart(oChart)
    End Function

    Public Shared Function CreatePieChar(
                              ByVal labeledValue As IList(Of KeyValuePair(Of String, Double)),
                              ByVal maxChar As Integer,
                              Optional ByVal chartTitle As String = ""
                              ) As RadHtmlChart

        Dim oChart As RadHtmlChart = GetBarChart(chartTitle)

        Dim oSeries As New PieSeries()


        'oSeries.Appearance.FillStyle.BackgroundColor = Color.FromArgb(69, 115, 166)
        oSeries.LabelsAppearance.Visible = True
        oSeries.LabelsAppearance.Position = PieAndDonutLabelsPosition.Center
        oSeries.LabelsAppearance.DataFormatString = "#%"

        oSeries.TooltipsAppearance.Visible = False '.DataFormatString = "#%"

        For Each val As KeyValuePair(Of String, Double) In labeledValue

            Dim value As Double = 0

            If Not Double.IsNaN(val.Value) Then
                value = val.Value
            End If

            Dim itm As New SeriesItem(value)
            itm.Name = TxtHelper_HtmlCutToString(val.Key, maxChar)
            itm.TooltipValue = String.Format("{0} ({1}%)", TxtHelper_HtmlCutToString(val.Key, 10), val.Value.ToString("N1"))

            oSeries.Items.Add(itm)

        Next

        oChart.PlotArea.Series.Add(oSeries)
        oChart.PlotArea.YAxis.LabelsAppearance.DataFormatString = "#%"
        oChart.PlotArea.YAxis.LabelsAppearance.Color = Color.FromArgb(255, 68, 68, 68)

        oChart.Height = New Unit("100%") 'New Unit(String.Format("{0}px", 360)) '30 + values.Count * 30)) '30 is for yaxis label

        oChart.Legend.Appearance.Visible = True
        oChart.Legend.Appearance.Position = ChartLegendPosition.Right
        oChart.Legend.Appearance.TextStyle.FontSize = 12
        oChart.Legend.Appearance.TextStyle.Color = Color.FromArgb(255, 68, 68, 68)

        Try
            For Each series As ChartSeries In oChart.PlotArea.Series
                For Each itm As ChartSeriesItem In series.Items
                    itm.Appearance.FillStyle = Telerik.WebControls.FillStyle.Solid
                Next
            Next
        Catch ex As Exception

        End Try

        Return FlatterChart(oChart)
    End Function

    Public Shared Function FlatterChart(ByVal chart As RadHtmlChart) As RadHtmlChart

        Try
            For Each series As ChartSeries In chart.PlotArea.Series
                For Each itm As ChartSeriesItem In series.Items
                    itm.Appearance.FillStyle = Telerik.WebControls.FillStyle.Solid
                Next
            Next
        Catch ex As Exception

        End Try

        'chart.Width = New Unit("900px")
        chart.Width = New Unit("100%")
        Return chart

    End Function

    ''' <summary>
    ''' Data una serie di possibili risposte, con una serie di possibili valori con la frequenza di risposta,
    ''' restituisce un grafico "Stacked-Bar" ad-hoc.
    ''' </summary>
    ''' <param name="chrValues"></param>
    ''' <returns></returns>
    Public Shared Function CreateStackedBarCharForRatingReverse(
        ByVal BarCount As Integer,
        ByVal chrValues As List(Of KeyValuePair(Of String, List(Of KeyValuePair(Of String, Integer)))),
        ByVal maxChar As Integer
        ) _
        As RadHtmlChart

        'Generazione struttura dati per grafico
        Dim xLabels As New List(Of String)

        Dim chrValuesREVCnt As New List(Of KeyValuePair(Of String, List(Of KeyValuePair(Of String, Decimal))))

        Dim Questions As New List(Of String)()
        Dim Answers As New List(Of String)()

        Dim dictItem As New Dictionary(Of String, Decimal)


        For Each ser As KeyValuePair(Of String, List(Of KeyValuePair(Of String, Integer))) In chrValues
            If Not Questions.Contains(ser.Key) Then
                Questions.Add(ser.Key)
            End If

            xLabels.Add(ser.Key)

            Dim Sum As Integer = 0

            For Each itm As KeyValuePair(Of String, Integer) In ser.Value
                If Not Answers.Contains(itm.Key) Then
                    Answers.Add(itm.Key)
                End If
                Sum = Sum + itm.Value
            Next

            For Each itm As KeyValuePair(Of String, Integer) In ser.Value

                Dim key As String = String.Format("{0}-{1}", ser.Key, itm.Key)

                If Not dictItem.ContainsKey(key) Then

                    Dim value As Decimal = 0D
                    If Sum > 0 Then
                        value = CDec(itm.Value) / CDec(Sum)
                    End If

                    dictItem.Add(key, value)
                    'dictItem.Add(key, itm.Value)
                End If
            Next
        Next

        For Each answ As String In Answers

            Dim newSer As New List(Of KeyValuePair(Of String, Decimal))


            For Each qst As String In Questions

                Dim key As String = String.Format("{0}-{1}", qst, answ)

                If dictItem.ContainsKey(key) Then

                    Dim itm As New KeyValuePair(Of String, Decimal)(qst, dictItem(key))
                    newSer.Add(itm)
                Else
                    Dim itm As New KeyValuePair(Of String, Decimal)(qst, 0D)
                    newSer.Add(itm)
                End If
            Next

            chrValuesREVCnt.Add(
                New KeyValuePair(Of String, List(Of KeyValuePair(Of String, Decimal)))(answ, newSer)
            )

        Next

        'Funzione render grafico
        Return FlatterChart(CreateStackedBarCharForRating(BarCount, chrValuesREVCnt, xLabels, maxChar))
    End Function

    Private Shared Function CreateStackedBarCharForRating(
        ByVal BarCount As Integer,
        ByVal chrValues As List(Of KeyValuePair(Of String, List(Of KeyValuePair(Of String, Decimal)))),
        ByVal XLabels As List(Of String),
        ByVal maxChar As Integer) _
        As RadHtmlChart

        Dim oChart As RadHtmlChart = GetBarChart()
        oChart.ChartTitle.Appearance.Visible = False



        'Provo a sovrapporre una nuova serie senza valori e vediamo che succede...

        Dim XValFake As New Dictionary(Of Integer, String)

        For Each chrSerie As KeyValuePair(Of String, List(Of KeyValuePair(Of String, Decimal))) In chrValues

            Dim XValue As Integer = 0

            Dim oSeries As New BarSeries()

            'oSeries.Appearance.FillStyle.BackgroundColor = Color.FromArgb(69, 115, 166)
            oSeries.LabelsAppearance.Visible = True
            oSeries.LabelsAppearance.DataFormatString = "#%"

            oSeries.TooltipsAppearance.Visible = True
            oSeries.TooltipsAppearance.DataFormatString = "#%"

            oSeries.LabelsAppearance.TextStyle.FontSize = 12
            oSeries.LabelsAppearance.TextStyle.Color = Color.FromArgb(255, 68, 68, 68)

            oSeries.LabelsAppearance.TextStyle.FontSize = 12
            oSeries.LabelsAppearance.TextStyle.Color = Color.FromArgb(255, 68, 68, 68)

            oSeries.LabelsAppearance.Position = BarColumnLabelsPosition.Center
            oSeries.Name = TxtHelper_CutString(chrSerie.Key, 25)

            oSeries.Stacked = True

            oSeries.Spacing = 0.2D
            oSeries.Gap = 0.2D


            For Each chrValue As KeyValuePair(Of String, Decimal) In chrSerie.Value

                XValue = XValue + 1D

                Dim chrItem As New SeriesItem(chrValue.Value)


                If chrValue.Value > 0 Then
                    chrItem.Name = chrValue.Key
                    chrItem.TooltipValue = String.Format("{0} ({1}%)", chrValue.Key, (chrValue.Value * 100).ToString("N0"))
                Else
                    chrItem.Name = ""
                    chrItem.YValue = Nothing
                    chrItem.TooltipValue = "Molliccio"
                End If

                chrItem.XValue = CDec(XValue)

                If Not XValFake.ContainsKey(XValue) Then
                    XValFake.Add(XValue, chrSerie.Key)
                End If

                oSeries.Items.Add(chrItem)

            Next
            oChart.PlotArea.Series.Add(oSeries)
        Next


        'Aggiungo un grafico fake per vedere se l'asse X è popolabile
        'Dim oFakeSeries As New BarSeries()
        'oFakeSeries.LabelsAppearance.Visible = True
        'oFakeSeries.TooltipsAppearance.Visible = True

        'For Each fkKey As Integer In XValFake.Keys.ToList()
        '    Dim chrFkItem As New SeriesItem()
        '    chrFkItem.XValue = CDec(fkKey)
        '    chrFkItem.YValue = CDec(fkKey)
        'Next

        'oChart.PlotArea.Series.Add(oFakeSeries)


        Dim QuestCount As Integer = chrValues.Count()

        oChart.PlotArea.YAxis.LabelsAppearance.Visible = True
        oChart.PlotArea.YAxis.MaxValue = 1D
        oChart.PlotArea.YAxis.LabelsAppearance.DataFormatString = "#%"
        oChart.PlotArea.YAxis.LabelsAppearance.TextStyle.FontSize = 12
        oChart.PlotArea.YAxis.LabelsAppearance.TextStyle.Color = Color.FromArgb(255, 68, 68, 68)

        oChart.PlotArea.XAxis.LabelsAppearance.Visible = True
        oChart.PlotArea.XAxis.LabelsAppearance.TextStyle.FontSize = 12
        oChart.PlotArea.XAxis.LabelsAppearance.TextStyle.Color = Color.FromArgb(255, 68, 68, 68)



        'Provo ad aggiungere le etichette a mano!
        For Each xLab As String In XLabels

            Dim labelX As New AxisItem()
            labelX.LabelText = TxtHelper_HtmlCutToString(xLab, maxChar)
            oChart.PlotArea.XAxis.Items.Add(labelX)
        Next


        'oChart.PlotArea.XAxis.MaxValue = CDec(QuestCount)
        'oChart.PlotArea.XAxis.TitleAppearance.Text = "Domande"
        'oChart.PlotArea.XAxis.MaxValue = 1D
        'oChart.PlotArea.XAxis.LabelsAppearance.DataFormatString = "#%"
        'oChart.PlotArea.XAxis.LabelsAppearance.Position = 

        Dim height As Integer = (BarCount + 2) * 30
        If height < 90 Then
            height = 90
        End If

        oChart.Height = New Unit(String.Format("{0}px", height)) '30 is for yaxis label

        Return FlatterChart(oChart)

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





        'oChart.Width = New Unit("620px")
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

        If (numChar > 0) AndAlso text.Count() > (numChar + 3) Then
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
        Return HttpUtility.JavaScriptStringEncode(TxtHelper_CutString(TxtHelper_HtmlToString(html), numChar))
    End Function

#End Region



End Class
