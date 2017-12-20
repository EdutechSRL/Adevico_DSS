Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.UserActions.Presentation
Imports lm.Comol.Modules.UserActions.DomainModel
Imports lm.Comol.Modules.UserActions.BusinessLogic
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.UI.Presentation
Imports WSstatistics
Imports System.Globalization
Imports System.Linq

Partial Public Class UsageDetails
    Inherits PageBase
    Implements IViewUsageDetails

#Region "Context"
    Private _Presenter As UsageDetailsPresenter
    Private ReadOnly Property CurrentPresenter() As UsageDetailsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New UsageDetailsPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements Property"
    Private Property StatisticContext() As UsageContext Implements IViewUsageDetails.StatisticContext
        Get
            Dim oContext As New UsageContext With {.CommunityID = -1, .ModuleID = -2, .UserID = 0}
            If TypeOf Me.ViewState("StatisticContext") Is UsageContext Then
                oContext = Me.ViewState("StatisticContext")
            Else
                If IsNumeric(Me.Request.QueryString("CommunityID")) Then
                    Try
                        oContext.CommunityID = Me.Request.QueryString("CommunityID")
                        Dim stringList As List(Of String)
                        stringList = Me.Request.QueryString.GetValues("CommunityID").ToList
                        For Each id As String In stringList
                            oContext.CommunityIDList.Add(CInt(id))
                        Next
                    Catch ex As Exception

                    End Try
                End If
                If IsNumeric(Me.Request.QueryString("ModuleID")) Then
                    Try
                        oContext.ModuleID = Me.Request.QueryString("ModuleID")
                        Dim stringList As List(Of String)
                        stringList = Me.Request.QueryString.GetValues("ModuleID").ToList
                        For Each id As String In stringList
                            oContext.ModuleIDList.Add(CInt(id))
                        Next
                    Catch ex As Exception

                    End Try
                End If
                If IsNumeric(Me.Request.QueryString("UserID")) Then
                    Try
                        oContext.UserID = Me.Request.QueryString("UserID")
                        Dim stringList As List(Of String)
                        stringList = Me.Request.QueryString.GetValues("UserID").ToList
                        For Each id As String In stringList
                            oContext.UserIDList.Add(CInt(id))
                        Next
                    Catch ex As Exception

                    End Try
                End If
                oContext.GroupBy = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of UsageContext.grouping).GetByString(Me.Request.QueryString("GroupBy"), UsageContext.grouping.None)
                Me.CurrentView = PreLoadedView
                oContext.CurrentPage = 0
                If IsNumeric(Me.Request.QueryString("Page")) Then
                    Try
                        oContext.CurrentPage = Me.Request.QueryString("Page")
                    Catch ex As Exception
                        oContext.CurrentPage = 0
                    End Try
                End If

                If String.IsNullOrEmpty(Me.Request.QueryString("Order")) Then
                    oContext.Order = StatisticOrder.UsageTime
                    oContext.Ascending = False
                Else
                    oContext.Order = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of StatisticOrder).GetByString(Me.Request.QueryString("Order"), StatisticOrder.UsageTime)
                    oContext.Ascending = True
                    If String.IsNullOrEmpty(Me.Request.QueryString("Dir")) Then
                        If oContext.Order = StatisticOrder.AccessNumber OrElse oContext.Order = StatisticOrder.UsageTime Then
                            oContext.Ascending = False
                        End If
                    Else
                        oContext.Ascending = (Me.Request.QueryString("Dir") = "" OrElse Me.Request.QueryString("Dir").ToLower = "asc")
                    End If
                End If
                Me.ViewState("StatisticContext") = oContext
            End If
            Return oContext
        End Get
        Set(ByVal value As UsageContext)
            Me.ViewState("StatisticContext") = value
        End Set
    End Property
    Private ReadOnly Property PreLoadedView() As IViewUsageDetails.viewType Implements IViewUsageDetails.PreLoadedView
        Get
            If IsNothing(Request.QueryString("Show")) Then
                Return IViewUsageDetails.viewType.Day
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of IViewUsageDetails.viewType).GetByString(Request.QueryString("Show"), IViewUsageDetails.viewType.Day)
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadedBackUrl As String Implements IViewUsageDetails.PreLoadedBackUrl
        Get
            Return Request.QueryString("BackUrl")
        End Get
    End Property
    Private Property BackUrl As String Implements IViewUsageDetails.BackUrl
        Get
            Return ViewStateOrDefault("BackUrl", "")
        End Get
        Set(value As String)
            HYPbackHistory.Visible = Not String.IsNullOrEmpty(value)
            ViewState("BackUrl") = value
            value = BaseUrl & value
            value = value.Replace("//", "/")
            HYPbackHistory.NavigateUrl = value

        End Set
    End Property
    Private Property CurrentView() As IViewUsageDetails.viewType Implements IViewUsageDetails.CurrentView
        Get
            Return ViewStateOrDefault("CurrentView", IViewUsageDetails.viewType.None)
        End Get
        Set(ByVal value As IViewUsageDetails.viewType)
            ViewState("CurrentView") = value
            Dim oTab As Telerik.Web.UI.RadTab = Me.TBSusageTime.FindTabByValue("viewType." & value.ToString, True)
            If Not IsNothing(oTab) Then
                Me.TBSusageTime.SelectedIndex = oTab.Index
            End If
        End Set
    End Property
    Private Property AvailableView() As List(Of lm.Comol.Modules.UserActions.Presentation.IViewUsageDetails.viewType) Implements IViewUsageDetails.AvailableView
        Get
            Return ViewStateOrDefault("AvailableView", New List(Of lm.Comol.Modules.UserActions.Presentation.IViewUsageDetails.viewType))
        End Get
        Set(ByVal value As List(Of lm.Comol.Modules.UserActions.Presentation.IViewUsageDetails.viewType))
            ViewState("AvailableView") = value
        End Set
    End Property
    Private ReadOnly Property CurrentEndDate(view As IViewUsageDetails.viewType) As DateTime Implements IViewUsageDetails.CurrentEndDate
        Get
            Dim endDate As DateTime = DateTime.Now.Date
            Select Case view
                Case IViewUsageDetails.viewType.Day
                    If Me.RDTweek.SelectedDate.HasValue Then
                        endDate = Me.RDTday.SelectedDate
                    End If
                    endDate = endDate.AddHours(23).AddMinutes(59)

                Case IViewUsageDetails.viewType.Week
                    If Me.RDTweek.SelectedDate.HasValue Then
                        endDate = DateAdd("d", 7 - Me.RDTweek.SelectedDate.Value.DayOfWeek, Me.RDTweek.SelectedDate.Value)
                    Else
                        endDate = DateAdd("d", 7 - endDate.DayOfWeek, endDate)
                    End If
                Case IViewUsageDetails.viewType.Month
                    endDate = DateSerial(DDLYear.SelectedValue, DDLMonth.SelectedValue, 1)
                    endDate = endDate.AddMonths(1).AddDays(-1)
                Case IViewUsageDetails.viewType.Year
                    endDate = DateSerial(DDLYear.SelectedValue, 12, 31)
            End Select
            Return endDate
        End Get
    End Property

    Private Property CurrentStartDate(view As IViewUsageDetails.viewType) As DateTime Implements IViewUsageDetails.CurrentStartDate
        Get
            Dim startDate As DateTime = DateTime.Now.Date
            Select Case view
                Case IViewUsageDetails.viewType.Day
                    If Me.RDTday.SelectedDate.HasValue Then
                        startDate = Me.RDTday.SelectedDate
                    Else
                        Me.RDTday.SelectedDate = startDate
                    End If
                Case IViewUsageDetails.viewType.Week
                    If Me.RDTweek.SelectedDate.HasValue Then
                        startDate = Me.RDTweek.SelectedDate
                    Else
                        startDate = DateAdd("d", 1 - startDate.DayOfWeek, startDate)
                        Me.RDTweek.SelectedDate = startDate
                    End If
                Case IViewUsageDetails.viewType.Month
                    startDate = DateSerial(DDLYear.SelectedValue, DDLMonth.SelectedValue, 1)
                Case IViewUsageDetails.viewType.Year
                    startDate = DateSerial(DDLYear.SelectedValue, 1, 1)
            End Select
            Return startDate
        End Get
        Set(value As DateTime)
            ViewState("CurrentStartDate") = value
            Select Case view
                Case IViewUsageDetails.viewType.Day
                    Me.RDTday.SelectedDate = value
                Case IViewUsageDetails.viewType.Week
                    Me.RDTweek.SelectedDate = value
                Case IViewUsageDetails.viewType.Month
                    Me.DDLMonth.SelectedValue = value.Month
                    If IsNothing(DDLYear.Items.FindByValue(value.Year)) Then
                        Me.DDLYear.Items.Insert(0, value.Year)
                    Else
                        Me.DDLYear.SelectedValue = value.Year
                    End If
                Case IViewUsageDetails.viewType.Year

            End Select
        End Set
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Private Property"
    Private _BaseUrl As String
    Private Overloads ReadOnly Property BaseUrl() As String
        Get
            If _BaseUrl = "" Then
                _BaseUrl = Me.PageUtility.BaseUrl
            End If
            Return _BaseUrl
        End Get
    End Property
    Public ReadOnly Property BackGroundItem(ByVal type As ListItemType) As String
        Get
            If type = ListItemType.Item Then
                Return "ROW_Normal_Small"
            Else
                Return "ROW_Alternate_Small"
            End If
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Me.CurrentPresenter.InitView()
    End Sub
    Public Overrides Sub BindNoPermessi()
        Me.Master.ShowNoPermission = True
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UsageDetails", "Statistiche")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setHyperLink(HYPbackHistory, True, True)
            .setLabel(LBmessage)
            For Each Tab As Telerik.Web.UI.RadTab In Me.TBSusageTime.Tabs
                .setRadTab(Tab, True)
            Next
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub LoadAvailableViews(views As List(Of IViewUsageDetails.viewType)) Implements IViewUsageDetails.LoadAvailableViews
        AvailableView = views
        For Each view As IViewUsageDetails.viewType In views
            Dim tab As Telerik.Web.UI.RadTab = Me.TBSusageTime.FindTabByValue("viewType." & view.ToString)
            If Not IsNothing(tab) Then
                tab.Visible = True
            End If
        Next
    End Sub
    Private Sub LoadAvailableYears(years As List(Of Integer)) Implements IViewUsageDetails.LoadAvailableYears
        Me.DDLYear.DataSource = years.OrderByDescending(Function(y) y).ToList()
        Me.DDLYear.DataBind()
        Me.DDLMonth.Items.Clear()
        Me.DDLMonth.DataSource = (From i As Integer In (From n In Enumerable.Range(1, 12).ToList) Select New With {.Value = i, .Text = MonthName(i)}).ToList
        Me.DDLMonth.DataTextField = "Text"
        Me.DDLMonth.DataValueField = "Value"
        Me.DDLMonth.DataBind()
    End Sub
    Private Sub LoadStatistics(statistics As List(Of dtoDetailUsageStatistic), view As IViewUsageDetails.viewType) Implements IViewUsageDetails.LoadStatistics
        Me.MLVusageDetails.SetActiveView(VIWdata)
        Me.MLVstatistics.SetActiveView(IIf(view = IViewUsageDetails.viewType.Week, VIWWeek, IIf(view = IViewUsageDetails.viewType.Month, VIWMonth, VIWDay)))
        Me.RPTstatistics.DataSource = statistics
        Me.RPTstatistics.DataBind()
    End Sub


    Private Sub DisplayUserInfo(moduleName As String, userName As String) Implements IViewUsageDetails.DisplayUserInfo
        Me.Master.ServiceTitle = String.Format(Resource.getValue("DisplayUserInfo"), moduleName, userName)
        Me.Master.ServiceTitleToolTip = String.Format(Resource.getValue("DisplayUserInfo.ToolTip"), moduleName, userName)
    End Sub
    Private Sub DisplayInfo(moduleName As String) Implements IViewUsageDetails.DisplayInfo
        Me.Master.ServiceTitle = String.Format(Resource.getValue("ModuleInfo"), moduleName)
        Me.Master.ServiceTitleToolTip = String.Format(Resource.getValue("ModuleInfo"), moduleName)
    End Sub
    Private Sub DisplayInfo(moduleName As String, communityName As String) Implements IViewUsageDetails.DisplayInfo
        Me.Master.ServiceTitle = String.Format(Resource.getValue("DisplayInfo"), moduleName, communityName)
        Me.Master.ServiceTitleToolTip = String.Format(Resource.getValue("DisplayInfo.ToolTip"), moduleName, communityName)
    End Sub
    Private Sub DisplayInfo(moduleName As String, communityName As String, userName As String) Implements IViewUsageDetails.DisplayInfo
        Me.Master.ServiceTitle = String.Format(Resource.getValue("DisplayInfo.Complete"), moduleName, userName)
        Me.Master.ServiceTitleToolTip = String.Format(Resource.getValue("DisplayInfo.Complete.ToolTip"), moduleName, userName, communityName)
    End Sub
    Private Sub DisplayNoPermission() Implements IViewUsageDetails.DisplayNoPermission
        Me.MLVusageDetails.SetActiveView(VIWempty)
        Me.Resource.setLabel(LBmessage)
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewUsageDetails.DisplaySessionTimeout
        Me.MLVusageDetails.SetActiveView(VIWempty)
        LBmessage.Text = Me.Resource.getValue("DisplaySessionTimeout")

        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()

        'If PreloadedIdItem > 0 Then
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = "Statistiche_Servizi/UsageDetails.aspx" & Request.Url.Query
        '   dto.IdCommunity = iif(StatisticContext.CommunityID > 0 andalso PreLoadedView=)
        'Else
        '    dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.None
        'End If
        webPost.Redirect(dto)
    End Sub
#End Region

    Private Sub RPTstatistics_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTstatistics.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim item As dtoDetailUsageStatistic = DirectCast(e.Item.DataItem, dtoDetailUsageStatistic)
            Dim timeInterval As New TimeSpan
            Dim oLiteral As Literal = e.Item.FindControl("LTusageTime")

            oLiteral.Text = timeInterval.FromSeconds(item.UsageTime).ToString()
        ElseIf e.Item.ItemType = ListItemType.Header Then
            Dim oLabel As Label = e.Item.FindControl("LBinfo_t")
            oLabel = e.Item.FindControl("LBinfo_t")
            Resource.setLabel_To_Value(oLabel, "LBinfo_t." & CurrentView.ToString)
            If CurrentView = IViewUsageDetails.viewType.Day Then
                oLabel.Text = FormatDateTime(RDTday.SelectedDate.Value, DateFormat.ShortDate) & " " & oLabel.Text
            ElseIf CurrentView = IViewUsageDetails.viewType.Month Then
                oLabel.Text = String.Format(oLabel.Text, Me.DDLMonth.SelectedItem.Text)
            End If

            oLabel = e.Item.FindControl("LBaccessNumber_t")
            Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBusageTime_t")
            Resource.setLabel(oLabel)
        End If
    End Sub
    Protected Sub TBSusageTime_TabClick(ByVal sender As System.Object, ByVal e As Telerik.Web.UI.RadTabStripEventArgs) Handles TBSusageTime.TabClick
        Dim view As IViewUsageDetails.viewType = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Modules.UserActions.Presentation.IViewUsageDetails.viewType).GetByString(Me.TBSusageTime.SelectedTab.Value.Replace("viewType.", ""), IViewUsageDetails.viewType.None)
        Dim currentDate As DateTime = DateTime.Now
        Select Case CurrentView
            Case IViewUsageDetails.viewType.Day
                If Me.RDTday.SelectedDate.HasValue Then
                    Dim day As DateTime = Me.RDTday.SelectedDate.Value
                    Me.RDTweek.SelectedDate = DateAdd("d", 1 - day.DayOfWeek, day)
                    Me.DDLMonth.SelectedValue = day.Month
                    If Not IsNothing(Me.DDLYear.Items.FindByValue(day.Year)) Then
                        Me.DDLYear.SelectedValue = day.Year
                    End If
                End If
            Case IViewUsageDetails.viewType.Week
                If Me.RDTweek.SelectedDate.HasValue Then
                    Dim day As DateTime = Me.RDTweek.SelectedDate.Value
                    Me.DDLMonth.SelectedValue = day.Month
                    If Not IsNothing(Me.DDLYear.Items.FindByValue(day.Year)) Then
                        Me.DDLYear.SelectedValue = day.Year
                    End If
                End If
            Case IViewUsageDetails.viewType.Month
                If Not (currentDate.Year = Me.DDLYear.SelectedValue AndAlso Me.DDLMonth.SelectedValue = currentDate.Month) Then
                    currentDate = DateSerial(DDLYear.SelectedValue, DDLMonth.SelectedValue, 1)
                End If
                Me.RDTday.SelectedDate = currentDate
                Me.RDTweek.SelectedDate = DateAdd("d", 1 - currentDate.DayOfWeek, currentDate)
        End Select
        ViewState("CurrentView") = view
        Me.CurrentPresenter.DisplayView(StatisticContext, view)
        'RedirectToUrl(ComposeQueryString(TBSusageTime.SelectedTab.Index))
    End Sub
    Protected Sub DDLMonth_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDLMonth.SelectedIndexChanged
        Me.CurrentPresenter.ReloadItems(StatisticContext, IViewUsageDetails.viewType.Month, CurrentStartDate(IViewUsageDetails.viewType.Month), CurrentEndDate(IViewUsageDetails.viewType.Month))
    End Sub
    Protected Sub DDLYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDLYear.SelectedIndexChanged
        Me.CurrentPresenter.ReloadItems(StatisticContext, IViewUsageDetails.viewType.Month, CurrentStartDate(IViewUsageDetails.viewType.Month), CurrentEndDate(IViewUsageDetails.viewType.Month))
    End Sub
    Protected Sub RDTweek_SelectedDateChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs) Handles RDTweek.SelectedDateChanged
        Me.CurrentPresenter.ReloadItems(StatisticContext, IViewUsageDetails.viewType.Week, CurrentStartDate(IViewUsageDetails.viewType.Week), CurrentEndDate(IViewUsageDetails.viewType.Week))
    End Sub

    Protected Sub RDTday_SelectedDateChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs) Handles RDTday.SelectedDateChanged
        Me.CurrentPresenter.ReloadItems(StatisticContext, IViewUsageDetails.viewType.Day, CurrentStartDate(IViewUsageDetails.viewType.Day), CurrentEndDate(IViewUsageDetails.viewType.Day))
    End Sub



    Protected Sub Calendar_OnDayRender(ByVal sender As Object, ByVal e As Telerik.Web.UI.Calendar.DayRenderEventArgs)
        ' modify the cell rendered content for the days we want to be disabled (e.g. every Saturday and Sunday)
        If (e.Day.Date.DayOfWeek <> DayOfWeek.Monday) Then
            ' if you are using the skin bundled as a webresource("Default"), the Skin property returns empty string
            Dim calendarSkin = RDTweek.Calendar.Skin
            If (calendarSkin = "") Then
                calendarSkin = "Default"
            End If
            Dim otherMonthCssClass As String = String.Format("otherMonth_{0}", calendarSkin)

            ' clear the default cell content (anchor tag) as we need to disable the hover effect for this cell
            e.Cell.Text = ""
            e.Cell.CssClass = otherMonthCssClass ' set new CssClass for the disabled calendar day cells (e.g. look like other month days here)

            ' render a span element with the processed calendar day number instead of the removed anchor -- necessary for the calendar skinning mechanism 
            Dim label As Label = New Label
            label.Text = e.Day.Date.Day.ToString
            e.Cell.Controls.Add(label)

            ' disable the selection for the specific day
            Dim calendarDay As Telerik.Web.UI.RadCalendarDay = New Telerik.Web.UI.RadCalendarDay
            calendarDay.Date = e.Day.Date
            calendarDay.IsSelectable = False
            calendarDay.ItemStyle.CssClass = otherMonthCssClass
            RDTweek.Calendar.SpecialDays.Add(calendarDay)
        End If
    End Sub

    'Private Function ComposeQueryString(ByRef show As IViewUsageDetails.viewType) As String
    '    Dim url As String
    '    url = "Statistiche_Servizi/UsageDetails.aspx?From=" & Request.QueryString("From") & "&ModuleID=" & Me.StatisticContext.ModuleID & "&UserID=" & Me.StatisticContext.UserID & "&CommunityID=" & Me.StatisticContext.CommunityID & "&GroupBy=" & Me.StatisticContext.GroupBy.ToString & "&Show=" & show.ToString & "&BackUrl=" & Request.QueryString("BackUrl")
    '    Return url
    'End Function

    ' necessary to disable the weekend days on first page load



#Region "BindingView"

    'Public Sub BindDayView(ByVal oList As List(Of lm.Comol.Modules.UserActions.DomainModel.dtoDetailsStatistic)) Implements lm.Comol.Modules.UserActions.Presentation.IViewUsageDetails.BindDayView
    '    'Dim tbl As New Table
    '    'tbl.ID = "TBLDayStats"
    '    'tbl.BorderWidth = 1
    '    'tbl.GridLines = GridLines.Both
    '    'tbl.CellPadding = 2
    '    'tbl.CellSpacing = 2
    '    'tbl.Width = 100%

    '    '' riga di intestazione con titolo mese/anno e numeri dei giorni
    '    'Dim rowInt As New TableRow
    '    'Dim clCommunityInt As New TableCell
    '    'Dim clRowName As New TableCell
    '    'Dim LBname As New Label
    '    'LBname.Text = Me.Resource.getValue("Grouping." & StatisticContext.GroupBy)
    '    'clRowName.Controls.Add(LBname)
    '    'rowInt.Cells.Add(clRowName) 'cella con; "nome comunita'/persona/servizio"

    '    'Dim oLiteral1Int As New LiteralControl
    '    'oLiteral1Int.Text = CDate(RDTday.SelectedDate).ToString("d")
    '    'clCommunityInt.Controls.Add(oLiteral1Int)
    '    'rowInt.Cells.Add(clCommunityInt)

    '    '' se sono a cavallo tra 2 mesi ciclo prima nel mese precedente 
    '    'For i As Integer = 0 To 23
    '    '    Dim clDay As New TableCell
    '    '    clDay.Text = i.ToString() + ".00"
    '    '    rowInt.Cells.Add(clDay)
    '    'Next

    '    'tbl.Rows.Add(rowInt)
    '    'For Each oDetailStat As dtoDetailsStatistic In oList

    '    '    ' righe di dati
    '    '    Dim rowAccess As New TableRow
    '    '    Dim rowUsageTime As New TableRow
    '    '    Dim CLname As New TableCell
    '    '    CLname.RowSpan = 2
    '    '    Dim LBnome As New Label
    '    '    'Dim id As Integer = oDetailStat.ID
    '    '    LBnome.Text = oDetailStat.Name
    '    '    CLname.Controls.Add(LBnome)
    '    '    rowAccess.Cells.Add(CLname)

    '    '    ' aggiungo le righe di accessi/tempi
    '    '    rowAccess.BackColor = Color.Azure
    '    '    Dim clDescription As New TableCell
    '    '    Dim oLiteral1 As New LiteralControl
    '    '    oLiteral1.Text = "Numero Accessi"
    '    '    clDescription.Controls.Add(oLiteral1)
    '    '    rowAccess.Cells.Add(clDescription)
    '    '    Dim clDescription2 As New TableCell
    '    '    Dim oLiteral2 As New LiteralControl
    '    '    oLiteral2.Text = "Tempi di uso"
    '    '    clDescription2.Controls.Add(oLiteral2)
    '    '    rowUsageTime.Cells.Add(clDescription2)

    '    '    For i As Integer = 0 To 23
    '    '        Dim clDay As New TableCell
    '    '        Dim clDayUT As New TableCell
    '    '        clDay.Text = "0"
    '    '        clDayUT.Text = "0"
    '    '        For Each statisticItem As dtoDetailsStatistic In oList
    '    '            If oDetailStat.ID = statisticItem.ID Then
    '    '                For Each obj As dtoDetailsTime In statisticItem.Items
    '    '                    If i = obj.Hour Then
    '    '                        clDay.Text = obj.nAccesses.ToString()
    '    '                        Dim timeInterval As New TimeSpan
    '    '                        clDayUT.Text = timeInterval.FromSeconds(obj.UsageTime).ToString()
    '    '                        Exit For
    '    '                    End If
    '    '                Next
    '    '                Exit For
    '    '            End If
    '    '        Next
    '    '        rowAccess.Cells.Add(clDay)
    '    '        rowUsageTime.Cells.Add(clDayUT)
    '    '    Next
    '    '    tbl.Rows.Add(rowAccess)
    '    '    tbl.Rows.Add(rowUsageTime)
    '    'Next

    '    'PHDayStats.Controls.Add(tbl)
    'End Sub

    'Public Sub BindWeekView(ByVal oList As List(Of lm.Comol.Modules.UserActions.DomainModel.dtoDetailsStatistic)) Implements lm.Comol.Modules.UserActions.Presentation.IViewUsageDetails.BindWeekView
    '    'Dim tbl As New Table
    '    'tbl.ID = "TBLWeekStats"
    '    'tbl.BorderWidth = 1
    '    'tbl.GridLines = GridLines.Both
    '    'tbl.CellPadding = 2
    '    'tbl.CellSpacing = 2
    '    'tbl.Width = 100%

    '    '' riga di intestazione con titolo mese/anno e numeri dei giorni
    '    'Dim rowInt As New TableRow
    '    'Dim clRowName As New TableCell
    '    'Dim clCommunityInt As New TableCell
    '    'Dim LBname As New Label
    '    'LBname.Text = Me.Resource.getValue("Grouping." & StatisticContext.GroupBy)
    '    'clRowName.Controls.Add(LBname)
    '    'rowInt.Cells.Add(clRowName) 'cella con; "nome comunita'/persona/servizio"

    '    'Dim oLiteral1Int As New LiteralControl
    '    'oLiteral1Int.Text = CDate(RDTweek.SelectedDate).ToString("MMMM")
    '    'clCommunityInt.Controls.Add(oLiteral1Int)
    '    'rowInt.Cells.Add(clCommunityInt)

    '    '' se sono a cavallo tra 2 mesi ciclo prima nel mese precedente 
    '    'Dim dayOfWeek As Integer = 1
    '    'If Me.EndDate.Day < Me.StartDate.Day Then
    '    '    For j As Integer = Me.StartDate.Day To DaysInMonth(Me.StartDate.Month, Me.StartDate.Year)
    '    '        Dim clDay As New TableCell
    '    '        clDay.Text = Me.Resource.getValue("Giorno." & dayOfWeek.ToString) & j.ToString()
    '    '        dayOfWeek += 1
    '    '        rowInt.Cells.Add(clDay)
    '    '    Next
    '    '    For i As Integer = 1 To Me.EndDate.Day
    '    '        Dim clDay As New TableCell
    '    '        clDay.Text = Me.Resource.getValue("Giorno." & dayOfWeek.ToString) & i.ToString()
    '    '        dayOfWeek += 1
    '    '        rowInt.Cells.Add(clDay)
    '    '    Next
    '    'Else
    '    '    For i As Integer = Me.StartDate.Day To Me.EndDate.Day
    '    '        Dim clDay As New TableCell
    '    '        clDay.Text = Me.Resource.getValue("Giorno." & dayOfWeek.ToString) & i.ToString()
    '    '        dayOfWeek += 1
    '    '        rowInt.Cells.Add(clDay)
    '    '    Next
    '    'End If
    '    'tbl.Rows.Add(rowInt)

    '    'For Each oDetailStat As dtoDetailsStatistic In oList

    '    '    ' righe di dati
    '    '    Dim rowAccess As New TableRow
    '    '    Dim rowUsageTime As New TableRow
    '    '    Dim CLname As New TableCell
    '    '    CLname.RowSpan = 2
    '    '    Dim LBnome As New Label
    '    '    Dim id As Integer = oDetailStat.ID
    '    '    LBnome.Text = oDetailStat.Name
    '    '    CLname.Controls.Add(LBnome)
    '    '    rowAccess.Cells.Add(CLname)

    '    '    'For Each in oList 
    '    '    ' aggiungo la riga degli accessi
    '    '    rowAccess.BackColor = Color.Azure
    '    '    Dim clDescription As New TableCell
    '    '    Dim oLiteral1 As New LiteralControl
    '    '    oLiteral1.Text = "Numero Accessi"
    '    '    clDescription.Controls.Add(oLiteral1)
    '    '    rowAccess.Cells.Add(clDescription)

    '    '    ' aggiungo la riga dei tempi
    '    '    Dim clDescription2 As New TableCell
    '    '    Dim oLiteral2 As New LiteralControl
    '    '    oLiteral2.Text = "Tempi di uso"
    '    '    clDescription2.Controls.Add(oLiteral2)
    '    '    rowUsageTime.Cells.Add(clDescription2)

    '    '    If Me.EndDate.Day < Me.StartDate.Day Then
    '    '        For j As Integer = Me.StartDate.Day To DaysInMonth(Me.StartDate.Month, Me.StartDate.Year)
    '    '            Dim clDay As New TableCell
    '    '            Dim clDayUT As New TableCell
    '    '            clDay.Text = "0"
    '    '            clDayUT.Text = "0"
    '    '            For Each statisticItem As dtoDetailsStatistic In oList
    '    '                If id = statisticItem.ID Then
    '    '                    For Each obj As dtoDetailsTime In statisticItem.Items
    '    '                        If j = obj.Day Then
    '    '                            clDay.Text = obj.nAccesses.ToString()
    '    '                            Dim timeInterval As New TimeSpan
    '    '                            clDayUT.Text = timeInterval.FromSeconds(obj.UsageTime).ToString()
    '    '                            Exit For
    '    '                        End If
    '    '                    Next
    '    '                    Exit For
    '    '                End If
    '    '            Next

    '    '            rowAccess.Cells.Add(clDay)
    '    '            rowUsageTime.Cells.Add(clDayUT)
    '    '        Next
    '    '        For j As Integer = 1 To Me.EndDate.Day
    '    '            Dim clDay As New TableCell
    '    '            Dim clDayUT As New TableCell
    '    '            clDay.Text = "0"
    '    '            clDayUT.Text = "0"
    '    '            For Each statisticItem As dtoDetailsStatistic In oList
    '    '                If id = statisticItem.ID Then
    '    '                    For Each obj As dtoDetailsTime In statisticItem.Items
    '    '                        If j = obj.Day Then
    '    '                            clDay.Text = obj.nAccesses.ToString()
    '    '                            Dim timeInterval As New TimeSpan
    '    '                            clDayUT.Text = timeInterval.FromSeconds(obj.UsageTime).ToString()
    '    '                            Exit For
    '    '                        End If
    '    '                    Next
    '    '                    Exit For
    '    '                End If
    '    '            Next

    '    '            rowAccess.Cells.Add(clDay)
    '    '            rowUsageTime.Cells.Add(clDayUT)
    '    '        Next
    '    '    Else
    '    '        For i As Integer = Me.StartDate.Day To Me.EndDate.Day
    '    '            Dim clDay As New TableCell
    '    '            Dim clDayUT As New TableCell
    '    '            clDay.Text = "0"
    '    '            clDayUT.Text = "0"
    '    '            For Each statisticItem As dtoDetailsStatistic In oList
    '    '                If id = statisticItem.ID Then
    '    '                    For Each obj As dtoDetailsTime In statisticItem.Items
    '    '                        If i = obj.Day Then
    '    '                            clDay.Text = obj.nAccesses.ToString()
    '    '                            Dim timeInterval As New TimeSpan
    '    '                            clDayUT.Text = timeInterval.FromSeconds(obj.UsageTime).ToString()
    '    '                            Exit For
    '    '                        End If
    '    '                    Next
    '    '                    Exit For
    '    '                End If
    '    '            Next
    '    '            rowAccess.Cells.Add(clDay)
    '    '            rowUsageTime.Cells.Add(clDayUT)
    '    '        Next
    '    '    End If
    '    '    tbl.Rows.Add(rowAccess)
    '    '    tbl.Rows.Add(rowUsageTime)
    '    'Next
    '    'PHWeekStats.Controls.Add(tbl)
    'End Sub

    'Public Sub BindMonthView(ByVal oList As List(Of dtoDetailsStatistic)) Implements lm.Comol.Modules.UserActions.Presentation.IViewUsageDetails.BindMonthView

    '    'Dim tbl As New Table
    '    'tbl.ID = "TBLMonthStats"
    '    'tbl.BorderWidth = 1
    '    'tbl.GridLines = GridLines.Both
    '    'tbl.CellPadding = 2
    '    'tbl.CellSpacing = 2
    '    'tbl.Width = 100%

    '    '' riga di intestazione con titolo mese/anno e numeri dei giorni
    '    'Dim rowInt As New TableRow
    '    'Dim clRowName As New TableCell
    '    'Dim clCommunityInt As New TableCell
    '    'Dim oLiteral1Int As New LiteralControl
    '    'oLiteral1Int.Text = DDLMonth.SelectedItem.Text + " " + DDLYear.SelectedItem.Text
    '    'clCommunityInt.Controls.Add(oLiteral1Int)
    '    'Dim LBname As New Label
    '    'LBname.Text = Me.Resource.getValue("Grouping." & StatisticContext.GroupBy)
    '    'clRowName.Controls.Add(LBname)
    '    'rowInt.Cells.Add(clRowName) 'cella con; "nome comunita'/persona/servizio"
    '    'rowInt.Cells.Add(clCommunityInt) 'cella con mese/anno

    '    'For i As Integer = 1 To Date.DaysInMonth(DDLYear.SelectedValue, DDLMonth.SelectedValue)
    '    '    Dim clDay As New TableCell
    '    '    clDay.Text = i.ToString()
    '    '    rowInt.Cells.Add(clDay)
    '    'Next
    '    'tbl.Rows.Add(rowInt)

    '    'For Each oDetailStat As dtoDetailsStatistic In oList
    '    '    ' righe di dati
    '    '    Dim rowAccess As New TableRow
    '    '    Dim rowUsageTime As New TableRow
    '    '    Dim CLname As New TableCell
    '    '    CLname.RowSpan = 2
    '    '    Dim LBnome As New Label
    '    '    Dim id As Integer = oDetailStat.ID
    '    '    LBnome.Text = oDetailStat.Name
    '    '    CLname.Controls.Add(LBnome)
    '    '    rowAccess.Cells.Add(CLname)

    '    '    ' aggiungo la riga degli accessi
    '    '    rowAccess.BackColor = Color.Azure
    '    '    Dim clDescription As New TableCell
    '    '    Dim oLiteral1 As New LiteralControl
    '    '    oLiteral1.Text = "Numero Accessi"
    '    '    clDescription.Controls.Add(oLiteral1)
    '    '    rowAccess.Cells.Add(clDescription)
    '    '    Dim clDescription2 As New TableCell
    '    '    Dim oLiteral2 As New LiteralControl
    '    '    oLiteral2.Text = "Tempi di uso"
    '    '    clDescription2.Controls.Add(oLiteral2)
    '    '    rowUsageTime.Cells.Add(clDescription2)
    '    '    For i As Integer = 1 To Date.DaysInMonth(DDLYear.SelectedValue, DDLMonth.SelectedValue)
    '    '        Dim clDay As New TableCell
    '    '        Dim clDayUT As New TableCell
    '    '        clDay.Text = "0"
    '    '        clDayUT.Text = "0"
    '    '        For Each statisticItem As dtoDetailsStatistic In oList
    '    '            If id = statisticItem.ID Then
    '    '                For Each obj As dtoDetailsTime In statisticItem.Items
    '    '                    If i = obj.Day Then
    '    '                        clDay.Text = obj.nAccesses.ToString()
    '    '                        Dim timeInterval As New TimeSpan
    '    '                        clDayUT.Text = timeInterval.FromSeconds(obj.UsageTime).ToString()
    '    '                        Exit For
    '    '                    End If
    '    '                Next
    '    '                Exit For
    '    '            End If
    '    '        Next
    '    '        rowUsageTime.Cells.Add(clDayUT)
    '    '        rowAccess.Cells.Add(clDay)
    '    '    Next
    '    '    tbl.Rows.Add(rowAccess)
    '    '    tbl.Rows.Add(rowUsageTime)
    '    'Next
    '    'PHMonthStats.Controls.Add(tbl)

    'End Sub

#End Region





End Class