Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.UserActions.Presentation
Imports lm.Comol.Modules.UserActions.DomainModel
'Imports lm.Comol.Modules.UserActions.BusinessLogic
'Imports COL_BusinessLogic_v2.UCServices
'Imports COL_BusinessLogic_v2.Comunita
'Imports lm.Comol.UI.Presentation
'Imports WSstatistics
Imports System.Globalization
Imports System.Linq
Imports lm.ActionDataContract
Partial Public Class StatisticDetails
    Inherits PageBase
    Implements IViewStatisticDetails

#Region "Context"
    Private _Presenter As StatisticDetailsPresenter
    Private ReadOnly Property CurrentPresenter() As StatisticDetailsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New StatisticDetailsPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements Property"
    Private Property StatisticContext() As UsageContext Implements IViewStatisticDetails.StatisticContext
        Get
            Dim oContext As New UsageContext With {.CommunityID = -1, .ModuleID = -2, .UserID = 0}
            If TypeOf Me.ViewState("StatisticContext") Is UsageContext Then
                oContext = Me.ViewState("StatisticContext")
            Else
                If IsNumeric(Me.Request.QueryString("IdCommunity")) Then
                    Try
                        oContext.CommunityID = Me.Request.QueryString("IdCommunity")
                        Dim stringList As List(Of String)
                        stringList = Me.Request.QueryString.GetValues("IdCommunity").ToList
                        For Each id As String In stringList
                            oContext.CommunityIDList.Add(CInt(id))
                        Next
                    Catch ex As Exception

                    End Try
                End If
                If IsNumeric(Me.Request.QueryString("IdModule")) Then
                    Try
                        oContext.ModuleID = Me.Request.QueryString("IdModule")
                        Dim stringList As List(Of String)
                        stringList = Me.Request.QueryString.GetValues("IdModule").ToList
                        For Each id As String In stringList
                            oContext.ModuleIDList.Add(CInt(id))
                        Next
                    Catch ex As Exception

                    End Try
                End If
                If IsNumeric(Me.Request.QueryString("IdUser")) Then
                    Try
                        oContext.UserID = Me.Request.QueryString("IdUser")
                        Dim stringList As List(Of String)
                        stringList = Me.Request.QueryString.GetValues("IdUser").ToList
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
                        oContext.Ascending = (Me.Request.QueryString("Dir") = "" OrElse Me.Request.QueryString("Dir").ToLower = "true")
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
    Private ReadOnly Property PreLoadedView() As DetailViewType Implements IViewStatisticDetails.PreLoadedView
        Get
            If IsNothing(Request.QueryString("Show")) Then
                Return DetailViewType.Day
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of DetailViewType).GetByString(Request.QueryString("Show"), DetailViewType.Day)
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadedBackUrl As String Implements IViewStatisticDetails.PreLoadedBackUrl
        Get
            Return Request.QueryString("BackUrl")
        End Get
    End Property
    Private Property BackUrl As String Implements IViewStatisticDetails.BackUrl
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
    Private Property AvailableDates As List(Of dtoYearItem) Implements IViewStatisticDetails.AvailableDates
        Get
            Return ViewStateOrDefault("AvailableDates", New List(Of dtoYearItem))
        End Get
        Set(value As List(Of dtoYearItem))
            ViewState("AvailableDates") = value
            If value.Any Then
                MinAvailableDate = value.OrderBy(Function(y) y.Value).FirstOrDefault.GetFirstDate()
                MaxAvailableDate = value.OrderBy(Function(y) y.Value).LastOrDefault.GetLastDate()
            End If
        End Set
    End Property
    Private Property MinAvailableDate() As DateTime?
        Get
            Return ViewStateOrDefault("MinAvailableDate", DateTime.MinValue)
        End Get
        Set(ByVal value As DateTime?)
            ViewState("MinAvailableDate") = value
        End Set
    End Property
    Private Property MaxAvailableDate() As DateTime?
        Get
            Return ViewStateOrDefault("MaxAvailableDate", DateTime.MaxValue.AddDays(-1))
        End Get
        Set(ByVal value As DateTime?)
            ViewState("MaxAvailableDate") = value
        End Set
    End Property

    Private Property CurrentView() As DetailViewType Implements IViewStatisticDetails.CurrentView
        Get
            Return ViewStateOrDefault("CurrentView", DetailViewType.None)
        End Get
        Set(ByVal value As DetailViewType)
            ViewState("CurrentView") = value
            Dim oTab As Telerik.Web.UI.RadTab = Me.TBSusageTime.FindTabByValue(value.ToString, True)
            If Not IsNothing(oTab) Then
                Me.TBSusageTime.SelectedIndex = oTab.Index
            End If
        End Set
    End Property
    Private Property AvailableView() As List(Of lm.Comol.Modules.UserActions.Presentation.DetailViewType)
        Get
            Return ViewStateOrDefault("AvailableView", New List(Of lm.Comol.Modules.UserActions.Presentation.DetailViewType))
        End Get
        Set(ByVal value As List(Of lm.Comol.Modules.UserActions.Presentation.DetailViewType))
            ViewState("AvailableView") = value
        End Set
    End Property
    Private ReadOnly Property CurrentEndDate(view As DetailViewType) As DateTime Implements IViewStatisticDetails.CurrentEndDate
        Get
            Dim endDate As DateTime = DateTime.Now.Date
            Select Case view
                Case DetailViewType.Day
                    If Me.RDTweek.SelectedDate.HasValue Then
                        endDate = Me.RDTday.SelectedDate
                    End If
                    endDate = endDate.AddHours(23).AddMinutes(59)

                Case DetailViewType.Week
                    If Me.RDTweek.SelectedDate.HasValue Then
                        endDate = DateAdd("d", 7 - Me.RDTweek.SelectedDate.Value.DayOfWeek, Me.RDTweek.SelectedDate.Value)
                    Else
                        endDate = DateAdd("d", 7 - endDate.DayOfWeek, endDate)
                    End If
                Case DetailViewType.Month
                    endDate = DateSerial(DDLYear.SelectedValue, DDLMonth.SelectedValue, 1)
                    endDate = endDate.AddMonths(1).AddDays(-1)
                Case DetailViewType.Year
                    endDate = DateSerial(DDLYear.SelectedValue, 12, 31)
            End Select
            Return endDate
        End Get
    End Property

    Private Property CurrentStartDate(view As DetailViewType) As DateTime Implements IViewStatisticDetails.CurrentStartDate
        Get
            Dim startDate As DateTime = DateTime.Now.Date
            Select Case view
                Case DetailViewType.Day
                    If Me.RDTday.SelectedDate.HasValue Then
                        startDate = Me.RDTday.SelectedDate
                    Else
                        Me.RDTday.SelectedDate = startDate
                    End If
                Case DetailViewType.Week
                    If Me.RDTweek.SelectedDate.HasValue Then
                        startDate = Me.RDTweek.SelectedDate
                    Else
                        startDate = DateAdd("d", 1 - startDate.DayOfWeek, startDate)
                        Me.RDTweek.SelectedDate = startDate
                    End If
                Case DetailViewType.Month
                    startDate = DateSerial(DDLYear.SelectedValue, DDLMonth.SelectedValue, 1)
                Case DetailViewType.Year
                    startDate = DateSerial(DDLYear.SelectedValue, 1, 1)
            End Select
            Return startDate
        End Get
        Set(value As DateTime)
            ViewState("CurrentStartDate") = value
            Select Case view
                Case DetailViewType.Day
                    Me.RDTday.SelectedDate = value
                Case DetailViewType.Week
                    Me.RDTweek.SelectedDate = value
                Case DetailViewType.Month
                    Me.DDLMonth.SelectedValue = value.Month
                    If IsNothing(DDLYear.Items.FindByValue(value.Year)) Then
                        Me.DDLYear.Items.Insert(0, value.Year)
                    Else
                        Me.DDLYear.SelectedValue = value.Year
                    End If
                Case DetailViewType.Year

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
    Private Sub LoadAvailableViews(views As List(Of DetailViewType)) Implements IViewStatisticDetails.LoadAvailableViews
        AvailableView = views
        For Each view As DetailViewType In views
            Dim tab As Telerik.Web.UI.RadTab = Me.TBSusageTime.FindTabByValue("DetailViewType." & view.ToString)
            If Not IsNothing(tab) Then
                tab.Visible = True
            End If
        Next
    End Sub
    Private Sub LoadAvailableYears(items As List(Of dtoYearItem)) Implements IViewStatisticDetails.LoadAvailableYears
        AvailableDates = items
        Me.DDLYear.DataSource = items.Select(Function(i) i.Value).OrderByDescending(Function(y) y).ToList()
        Me.DDLYear.DataBind()
        LoadMonthItems(CInt(Me.DDLYear.SelectedValue), 0)

        'Me.DDLMonth.Items.Clear()
        'If Me.DDLYear.SelectedIndex > -1 Then
        '    Me.DDLMonth.DataSource = (From i As Integer In items.Where(Function(y) y.Value = CInt(Me.DDLYear.SelectedValue)).FirstOrDefault().GetOrderdMonthValues Select New With {.Value = i, .Text = MonthName(i)}).ToList
        'Else
        '    Me.DDLMonth.DataSource = (From i As Integer In (From n In Enumerable.Range(1, 12).ToList) Select New With {.Value = i, .Text = MonthName(i)}).ToList
        'End If
        'Me.DDLMonth.DataTextField = "Text"
        'Me.DDLMonth.DataValueField = "Value"
        'Me.DDLMonth.DataBind()
    End Sub
    Private Sub LoadStatistics(statistics As List(Of dtoDetailUsageStatistic), view As DetailViewType) Implements IViewStatisticDetails.LoadStatistics
        Me.MLVusageDetails.SetActiveView(VIWdata)
        Me.MLVstatistics.SetActiveView(IIf(view = DetailViewType.Week, VIWWeek, IIf(view = DetailViewType.Month, VIWMonth, VIWDay)))
        Me.RPTstatistics.DataSource = statistics
        Me.RPTstatistics.DataBind()
    End Sub


    Private Sub DisplayUserInfo(moduleName As String, userName As String) Implements IViewStatisticDetails.DisplayUserInfo
        Me.Master.ServiceTitle = String.Format(Resource.getValue("DisplayUserInfo"), moduleName, userName)
        Me.Master.ServiceTitleToolTip = String.Format(Resource.getValue("DisplayUserInfo.ToolTip"), moduleName, userName)
    End Sub
    Private Sub DisplayInfo(moduleName As String) Implements IViewStatisticDetails.DisplayInfo
        Me.Master.ServiceTitle = String.Format(Resource.getValue("ModuleInfo"), moduleName)
        Me.Master.ServiceTitleToolTip = String.Format(Resource.getValue("ModuleInfo"), moduleName)
    End Sub
    Private Sub DisplayInfo(moduleName As String, communityName As String) Implements IViewStatisticDetails.DisplayInfo
        Me.Master.ServiceTitle = String.Format(Resource.getValue("DisplayInfo"), moduleName, communityName)
        Me.Master.ServiceTitleToolTip = String.Format(Resource.getValue("DisplayInfo.ToolTip"), moduleName, communityName)
    End Sub
    Private Sub DisplayInfo(moduleName As String, communityName As String, userName As String) Implements IViewStatisticDetails.DisplayInfo
        Me.Master.ServiceTitle = String.Format(Resource.getValue("DisplayInfo.Complete"), moduleName, userName)
        Me.Master.ServiceTitleToolTip = String.Format(Resource.getValue("DisplayInfo.Complete.ToolTip"), moduleName, userName, communityName)
    End Sub
    Private Sub DisplayNoPermission() Implements IViewStatisticDetails.DisplayNoPermission
        Me.MLVusageDetails.SetActiveView(VIWempty)
        Me.Resource.setLabel(LBmessage)
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewStatisticDetails.DisplaySessionTimeout
        Me.MLVusageDetails.SetActiveView(VIWempty)
        LBmessage.Text = Me.Resource.getValue("DisplaySessionTimeout")

        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()

        'If PreloadedIdItem > 0 Then
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = RootObject.UsageDetails() & Request.Url.Query
        '   dto.IdCommunity = iif(StatisticContext.CommunityID > 0 andalso PreLoadedView=)
        'Else
        '    dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.None
        'End If
        webPost.Redirect(dto)
    End Sub


    Private Sub SendAction(idCommunity As Integer, idModule As Integer, statIdUser As Integer, statIdCommunity As Integer, action As ModuleStatistics.ActionType) Implements IViewStatisticDetails.SendAction
        Dim items As New Dictionary(Of Integer, String)
        items.Add(ModuleStatistics.ObjectType.User, statIdUser)
        items.Add(ModuleStatistics.ObjectType.Community, statIdCommunity)
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, items), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendAction(idCommunity As Integer, idModule As Integer, action As ModuleStatistics.ActionType) Implements IViewStatisticDetails.SendAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, , InteractionType.UserWithLearningObject)
    End Sub

#End Region

    Private Sub RPTstatistics_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTstatistics.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim item As dtoDetailUsageStatistic = DirectCast(e.Item.DataItem, dtoDetailUsageStatistic)
            Dim oLiteral As Literal = e.Item.FindControl("LTusageTime")

            oLiteral.Text = TimeSpan.FromSeconds(item.UsageTime).ToString()
        ElseIf e.Item.ItemType = ListItemType.Header Then
            Dim oLabel As Label = e.Item.FindControl("LBinfo_t")
            oLabel = e.Item.FindControl("LBinfo_t")
            Resource.setLabel_To_Value(oLabel, "LBinfo_t." & CurrentView.ToString)
            If CurrentView = DetailViewType.Day Then
                oLabel.Text = FormatDateTime(RDTday.SelectedDate.Value, DateFormat.ShortDate) & " " & oLabel.Text
            ElseIf CurrentView = DetailViewType.Month Then
                oLabel.Text = String.Format(oLabel.Text, Me.DDLMonth.SelectedItem.Text)
            End If

            oLabel = e.Item.FindControl("LBaccessNumber_t")
            Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBusageTime_t")
            Resource.setLabel(oLabel)
        End If
    End Sub
    Protected Sub TBSusageTime_TabClick(ByVal sender As System.Object, ByVal e As Telerik.Web.UI.RadTabStripEventArgs) Handles TBSusageTime.TabClick
        Dim view As DetailViewType = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Modules.UserActions.Presentation.DetailViewType).GetByString(Me.TBSusageTime.SelectedTab.Value, DetailViewType.Month)
        Dim currentDate As DateTime = DateTime.Now
        Select Case CurrentView
            Case DetailViewType.Day
                If Me.RDTday.SelectedDate.HasValue Then
                    Dim day As DateTime = Me.RDTday.SelectedDate.Value
                    Me.RDTweek.SelectedDate = DateAdd("d", 1 - day.DayOfWeek, day)
                    'If Me.DDLMonth.Items.FindByValue(
                    'Me.DDLMonth.SelectedValue = day.Month
                    If Not IsNothing(Me.DDLYear.Items.FindByValue(day.Year)) Then
                        Me.DDLYear.SelectedValue = day.Year
                        LoadMonthItems(day.Year, day.Month)
                    End If
                End If
            Case DetailViewType.Week
                If Me.RDTweek.SelectedDate.HasValue Then
                    Dim day As DateTime = Me.RDTweek.SelectedDate.Value
                    ' Me.DDLMonth.SelectedValue = day.Month
                    If Not IsNothing(Me.DDLYear.Items.FindByValue(day.Year)) Then
                        Me.DDLYear.SelectedValue = day.Year
                        LoadMonthItems(day.Year, day.Month)
                    End If
                End If
            Case DetailViewType.Month
                If Not (currentDate.Year = Me.DDLYear.SelectedValue AndAlso Me.DDLMonth.SelectedValue = currentDate.Month) Then
                    currentDate = DateSerial(DDLYear.SelectedValue, DDLMonth.SelectedValue, 1)
                    If currentDate < MinAvailableDate Then
                        currentDate = MinAvailableDate
                    ElseIf currentDate > MaxAvailableDate Then
                        currentDate = MaxAvailableDate
                    End If
                End If
                Me.RDTday.SelectedDate = currentDate
                Me.RDTweek.SelectedDate = DateAdd("d", 1 - currentDate.DayOfWeek, currentDate)
        End Select
        ViewState("CurrentView") = view
        Me.CurrentPresenter.DisplayView(StatisticContext, view)
        'RedirectToUrl(ComposeQueryString(TBSusageTime.SelectedTab.Index))
    End Sub
    Protected Sub DDLMonth_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDLMonth.SelectedIndexChanged
        Me.CurrentPresenter.ReloadItems(StatisticContext, DetailViewType.Month, CurrentStartDate(DetailViewType.Month), CurrentEndDate(DetailViewType.Month))
    End Sub
    Protected Sub DDLYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDLYear.SelectedIndexChanged
        Dim month As Integer = 0
        If Me.DDLMonth.SelectedIndex > -1 Then
            month = CInt(Me.DDLMonth.SelectedValue)
        End If
        LoadMonthItems(CInt(Me.DDLYear.SelectedValue), month)

        Me.CurrentPresenter.ReloadItems(StatisticContext, DetailViewType.Month, CurrentStartDate(DetailViewType.Month), CurrentEndDate(DetailViewType.Month))
    End Sub

    Private Sub LoadMonthItems(ByVal value As Integer, ByVal month As Integer)
        Dim year As dtoYearItem = AvailableDates.Where(Function(y) y.Value = CInt(value)).FirstOrDefault
        If Not IsNothing(year) AndAlso year.Items.Any Then
            Me.DDLMonth.DataSource = (From i As Integer In year.GetOrderdMonthValues Select New With {.Value = i, .Text = MonthName(i)}).ToList
        Else
            Me.DDLMonth.DataSource = (From i As Integer In (From n In Enumerable.Range(1, 12).ToList) Select New With {.Value = i, .Text = MonthName(i)}).ToList
        End If
        Me.DDLMonth.DataTextField = "Text"
        Me.DDLMonth.DataValueField = "Value"
        Me.DDLMonth.DataBind()
        If month > 0 AndAlso Not IsNothing(Me.DDLMonth.Items.FindByValue(month)) Then
            Me.DDLMonth.SelectedValue = month
        End If
    End Sub
    Protected Sub RDTweek_SelectedDateChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs) Handles RDTweek.SelectedDateChanged
        Me.CurrentPresenter.ReloadItems(StatisticContext, DetailViewType.Week, CurrentStartDate(DetailViewType.Week), CurrentEndDate(DetailViewType.Week))
    End Sub
    Protected Sub RDTday_SelectedDateChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs) Handles RDTday.SelectedDateChanged
        Me.CurrentPresenter.ReloadItems(StatisticContext, DetailViewType.Day, CurrentStartDate(DetailViewType.Day), CurrentEndDate(DetailViewType.Day))
    End Sub

    Protected Sub Calendar_OnDayRender(ByVal sender As Object, ByVal e As Telerik.Web.UI.Calendar.DayRenderEventArgs)
        ' modify the cell rendered content for the days we want to be disabled (e.g. every Saturday and Sunday)
        If (e.Day.Date.DayOfWeek <> DayOfWeek.Monday) Then
            ' if you are using the skin bundled as a webresource("Default"), the Skin property returns empty string
            Dim calendarSkin As String = RDTweek.Calendar.Skin
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

    'Private Function ComposeQueryString(ByRef show As DetailViewType) As String
    '    Dim url As String
    '    url = "Statistiche_Servizi/UsageDetails.aspx?From=" & Request.QueryString("From") & "&ModuleID=" & Me.StatisticContext.ModuleID & "&UserID=" & Me.StatisticContext.UserID & "&CommunityID=" & Me.StatisticContext.CommunityID & "&GroupBy=" & Me.StatisticContext.GroupBy.ToString & "&Show=" & show.ToString & "&BackUrl=" & Request.QueryString("BackUrl")
    '    Return url
    'End Function

    ' necessary to disable the weekend days on first page load



#Region "BindingView"

    'Public Sub BindDayView(ByVal oList As List(Of lm.Comol.Modules.UserActions.DomainModel.dtoDetailsStatistic)) Implements lm.Comol.Modules.UserActions.Presentation.IViewStatisticDetails.BindDayView
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

    'Public Sub BindWeekView(ByVal oList As List(Of lm.Comol.Modules.UserActions.DomainModel.dtoDetailsStatistic)) Implements lm.Comol.Modules.UserActions.Presentation.IViewStatisticDetails.BindWeekView
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

    'Public Sub BindMonthView(ByVal oList As List(Of dtoDetailsStatistic)) Implements lm.Comol.Modules.UserActions.Presentation.IViewStatisticDetails.BindMonthView

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
    Private Sub MySystemStatistics_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Me.Master.ShowDocType = True
    End Sub

End Class