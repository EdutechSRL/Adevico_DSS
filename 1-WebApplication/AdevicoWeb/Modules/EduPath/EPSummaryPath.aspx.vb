Imports lm.Comol.Modules.EduPath.Domain
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.EduPath.BusinessLogic
Imports lm.Comol.Core.BaseModules.ProfileManagement.Business
Imports lm.Comol.Core.BaseModules.CommunityManagement.Business
Imports lm.Comol.Modules.EduPath.Presentation
Imports lm.ActionDataContract

Public Class EPSummaryPath
    Inherits EPlitePageBaseEduPath
    Implements IViewSummaryPath

#Region "Context"
    Protected _serviceEP As lm.Comol.Modules.EduPath.BusinessLogic.Service
    Protected _ServiceStat As ServiceStat
    Protected ReadOnly Property ServiceEP As Service
        Get
            If IsNothing(_serviceEP) Then
                _serviceEP = New Service(Me.PageUtility.CurrentContext)
            End If
            Return _serviceEP
        End Get
    End Property
    Protected ReadOnly Property ServiceStat As ServiceStat
        Get
            If IsNothing(_ServiceStat) Then
                _ServiceStat = ServiceEP.ServiceStat
            End If
            Return _ServiceStat
        End Get
    End Property

    Private _QSservice As COL_Questionario.Business.ServiceQuestionnaire
    Private ReadOnly Property QSservice() As COL_Questionario.Business.ServiceQuestionnaire
        Get
            If IsNothing(_QSservice) Then
                _QSservice = New COL_Questionario.Business.ServiceQuestionnaire(Me.PageUtility.CurrentContext)
            End If
            Return _QSservice
        End Get
    End Property

    Private _Presenter As SummaryPathPresenter
    Private ReadOnly Property CurrentPresenter() As SummaryPathPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New SummaryPathPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property PreloadReloadFilters As Boolean Implements IViewSummaryPath.PreloadReloadFilters
        Get
            If String.IsNullOrEmpty(Me.Request.QueryString("Reload")) OrElse Me.Request.QueryString("Reload") <> "True" Then
                Return False
            Else
                Return True
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewSummaryPath.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadFromSummary As SummaryType Implements IViewSummaryPath.PreloadFromSummary
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of SummaryType).GetByString(Request.QueryString("From"), lm.Comol.Modules.EduPath.Domain.SummaryType.PortalIndex)
        End Get
    End Property
    Private Property SummaryIdCommunity As Integer Implements IViewSummaryPath.SummaryIdCommunity
        Get
            Return ViewStateOrDefault("SummaryIdCommunity", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("SummaryIdCommunity") = value
        End Set
    End Property
    Private Property FromSummary As SummaryType Implements IViewSummaryPath.FromSummary
        Get
            Return ViewStateOrDefault("FromSummary", SummaryType.CommunityIndex)
        End Get
        Set(value As SummaryType)
            Me.ViewState("FromSummary") = value
        End Set
    End Property
    Private Property Pager As PagerBase Implements IViewSummaryPath.Pager
        Get
            Return ViewStateOrDefault("Pager", New lm.Comol.Core.DomainModel.PagerBase)
        End Get
        Set(ByVal value As lm.Comol.Core.DomainModel.PagerBase)
            Me.ViewState("Pager") = value
            Me.PGgridTop.Pager = value
            Me.PGgridBottom.Pager = value
            Me.DVpagerTop.Visible = Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize)
            Me.DVpagerBottom.Visible = Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize)
        End Set
    End Property
    Private Property AllowOrganizationSelection As Boolean Implements IViewSummaryPath.AllowOrganizationSelection
        Get
            Return ViewStateOrDefault("AllowOrganizationSelection", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowOrganizationSelection") = value
            Me.DIVfilterorganization.Visible = value
        End Set
    End Property
#End Region

#Region "Page property"
    Public Enum UserView
        none
        ungrouped
        grouped
    End Enum

    Dim _org As Int32
    Public Property OrganizationFilter As Int32
        Get
            _org = DDLfilterOrganization.SelectedValue

            _org = GetQuery("orgId", _org)
            Return _org

            'If (String.IsNullOrEmpty(Request.QueryString("orgId"))) Then
            '    Return _org
            'Else
            '    If (Int32.TryParse(Request.QueryString("orgId"), _org)) Then
            '        Return _org
            '    Else
            '        Return DDLfilterOrganization.SelectedIndex
            '    End If

            'End If
        End Get
        Set(value As Int32)
            _org = value
            ViewState("orgId") = _org
            Try
                DDLfilterOrganization.SelectedValue = _org
            Catch
                DDLfilterOrganization.SelectedIndex = 0
            End Try

            'set dropdown

        End Set
    End Property

    Dim _community As String
    Public Property CommunityFilter As String
        Get
            _community = TXBfilterCommunity.Text

            _community = GetQuery("cmnt", _community)
            Return _community
            'If (String.IsNullOrEmpty(Request.QueryString("cmnt"))) Then
            '    _community = TXBfilterCommunity.Text
            '    Return _community
            'Else
            '    _community = Request.QueryString("cmnt")
            '    Return _community
            'End If
        End Get
        Set(value As String)
            _community = value
            TXBfilterCommunity.Text = _community

            'set dropdown

        End Set
    End Property

    Dim _path As String
    Public Property PathFilter As String
        Get
            _path = TXBfilterPath.Text

            _path = GetQuery("path", _path)
            Return _path
            'If (String.IsNullOrEmpty(Request.QueryString("path"))) Then
            '    _path = TXBfilterPath.Text
            '    Return _path
            'Else
            '    _path = Request.QueryString("path")
            '    Return _path
            'End If
        End Get
        Set(value As String)
            _path = value
            TXBfilterPath.Text = _path

            'set dropdown

        End Set
    End Property

    Dim _status As String
    Public Property StatusFilter As String
        Get
            _status = RBLfilterStatus.SelectedValue

            _status = GetQuery("status", _status)
            Return _status
            'If (String.IsNullOrEmpty(Request.QueryString("status"))) Then
            '    _status = RBLfilterStatus.SelectedValue
            '    Return _status
            'Else
            '    _status = Request.QueryString("status")
            '    Return _status
            'End If
        End Get
        Set(value As String)
            _status = value
            RBLfilterStatus.SelectedValue = _status

            'set dropdown

        End Set
    End Property

    Dim _startdate As String
    Public Property StartDateFilter As String
        Get
            If (RDPStartDate.SelectedDate.HasValue) Then
                _startdate = RDPStartDate.SelectedDate.Value.ToShortDateString()
            Else
                _startdate = ""
            End If

            _startdate = GetQuery("start", _startdate)
            Return _startdate
            'If (String.IsNullOrEmpty(Request.QueryString("start"))) Then
            '    If (RDPStartDate.SelectedDate.HasValue) Then
            '        _startdate = RDPStartDate.SelectedDate.Value.ToShortDateString()
            '    Else
            '        _startdate = ""
            '    End If

            '    Return _startdate
            'Else
            '    _startdate = Request.QueryString("start")
            '    Return _startdate
            'End If
        End Get
        Set(value As String)
            _startdate = value
            Try
                RDPStartDate.SelectedDate = DateTime.Parse(_startdate)
            Catch ex As Exception
                RDPStartDate.Clear()
            End Try

            'set dropdown

        End Set
    End Property

    Dim _enddate As String
    Public Property EndDateFilter As String
        Get
            If (RDPEndDate.SelectedDate.HasValue) Then
                _enddate = RDPEndDate.SelectedDate.Value.ToShortDateString()
            Else
                _enddate = ""
            End If

            _enddate = GetQuery("end", _enddate)

            Return _enddate
            'If (String.IsNullOrEmpty(Request.QueryString("end"))) Then
            '    If (RDPEndDate.SelectedDate.HasValue) Then
            '        _enddate = RDPEndDate.SelectedDate.Value.ToShortDateString()
            '    Else
            '        _enddate = ""
            '    End If

            '    Return _enddate
            'Else
            '    _enddate = Request.QueryString("end")
            '    Return _enddate
            'End If
        End Get
        Set(value As String)
            _enddate = value
            Try
                RDPEndDate.SelectedDate = DateTime.Parse(_enddate)
            Catch ex As Exception
                RDPEndDate.Clear()
            End Try


            'set dropdown

        End Set
    End Property

    Dim _view As UserView = UserView.ungrouped
    Public Property View As UserView
        Get

            _view = ViewStateOrDefault("view", _view)


            _view = [Enum].Parse(GetType(UserView), GetQuery("view", _view.ToString()))

            ViewState("view") = _view

            Return _view

            'If Not String.IsNullOrEmpty(Request.QueryString("view")) Then
            '    _view = [Enum].Parse(GetType(UserView), Request.QueryString("view"))
            'End If
            'If _view = UserView.none Then
            '    MLVsummaryUser.SetActiveView(VIWsummaryUserUngrouped)
            '    _view = UserView.ungrouped
            '    'SetQueryString("view", _view.ToString())
            'End If
            'Return _view
        End Get
        Set(value As UserView)

            _view = value
            ViewState("view") = _view
            'SetQueryString("view", _view.ToString())
            Select Case value
                Case UserView.none
                    MLVsummaryUser.SetActiveView(VIWsummaryUserUngrouped)
                Case UserView.ungrouped
                    MLVsummaryUser.SetActiveView(VIWsummaryUserUngrouped)
                Case UserView.grouped
                    MLVsummaryUser.SetActiveView(VIWsummaryUserGrouped)
            End Select
        End Set
    End Property

    Dim _order As String = ""
    Public Property Order As String
        Get
            _order = ViewStateOrDefault("order", _order)

            _order = GetQuery("order", _order)

            Return _order
        End Get
        Set(value As String)
            _order = value
            ViewState("order") = value
        End Set
    End Property

    Dim _ascending As Boolean
    Public Property Ascending As Boolean
        Get
            _ascending = ViewStateOrDefault("ascending", _ascending)

            _ascending = GetQuery("ascending", _ascending)

            Return _ascending
        End Get
        Set(value As Boolean)
            _ascending = value
            ViewState("ascending") = value
        End Set
    End Property

    Public Function StatusLocked(locked As Boolean) As String
        If locked Then
            Return "locked"
        Else
            Return "unlocked"
        End If
    End Function

    Public Sub PresetOrder(order As String, ascending As Boolean)
        Me.Order = order
        Me.Ascending = ascending

        Select Case order
            Case "path"
                OBpathname.Status = IIf(ascending, OrderByStatus.Ascending, OrderByStatus.Descending)
                OBtimerange.Status = OrderByStatus.None
            Case "timerange"
                OBtimerange.Status = IIf(ascending, OrderByStatus.Ascending, OrderByStatus.Descending)
                OBpathname.Status = OrderByStatus.None
            Case Else
        End Select
    End Sub
    Public Const StartEndTitle As String = "{0} - {1}"
    Public StartEndDeadline As String = "<span class='from {2}'><span class='icon'>&nbsp;</span>{0}</span><span class='to {3}'><span class='icon'>&nbsp;</span>{1}</span>"

    Public Function StartEnd(dto As dtoUserPathInfo)
        Return String.Format(StartEndTitle, dto.StartDate, dto.EndDate)
    End Function

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PGgridTop.Pager = Me.Pager
        Me.PGgridBottom.Pager = Me.Pager
    End Sub

    Private Sub EPSummaryPath_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        Me.Master.ShowDocType = True
    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Me.CurrentPresenter.InitView()
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub BindNoPermessi()
        Me.Master.ServiceNopermission = Me.Resource.getValue("Error.NotPermission")
        Me.Master.ShowNoPermission = True
    End Sub
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        If PreloadIsMooc Then
            MyBase.SetCulture("pg_MoocsSummary", "EduPath")
        Else
            MyBase.SetCulture("pg_Summary", "EduPath")
        End If
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLabel(LBfilterCommunityTitle)
            .setLabel(LBfilterOrganizationTitle)
            .setLabel(LBfilterEndDateTitle)
            .setLabel(LBfilterPathTitle)
            .setLabel(LBfilterStartDateTitle)
            .setLabel(LBfilterStatusTitle)
            .setLabel(LBdetailsUsersTitle)
            .setHyperLink(HYPback, False, False)
            .setButton(BTNupdate)
            .setLiteral(LTpageBottom)
            .setLiteral(LTpageTop)
            .setCheckBox(CHBviewcommunity)

            .setLabel(LBcommunityNameHeader)
            .setLabel(LBactionsHeader)
            .setLabel(LBcompletionHeader)
            .setLabel(LBpathNameHeader)
            .setLabel(LBtimerangeHeader)
            .setLabel(LBcommunityPathNameHeader)

            LBactionsgroupedHeader.Text = LBactionsHeader.Text
            LBtimerangegroupedHeader.Text = LBtimerangeHeader.Text
            LBcompletiongroupedHeader.Text = LBcompletionHeader.Text

            LBdetailsUserCompletedLabel.Text = .getValue("EduPathTranslations.CompletedS")
            LBdetailsUserStartedLabel.Text = .getValue("EduPathTranslations.StartedS")
            LBdetailsUserNotStartedLabel.Text = .getValue("EduPathTranslations.NotStartedS")
            BTNgrouping.Text = .getValue("BTNgrouping." + View.ToString())

            Master.ServiceTitle = Resource.getValue("ServiceTitle." & SummaryType.Path.ToString())
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region


#Region "Implements"
    Private Sub DisplayNoPermission() Implements IViewSummaryPath.DisplayNoPermission
        Me.BindNoPermessi()
    End Sub
    Private Sub DisplayWrongPageAccess() Implements IViewSummaryPath.DisplayWrongPageAccess
        Me.Master.ServiceNopermission = Me.Resource.getValue("DisplayWrongPageAccess")
        Me.Master.ShowNoPermission = True
    End Sub
    Private Sub DisplayWrongPageAccess(ByVal url As String) Implements IViewSummaryPath.DisplayWrongPageAccess
        Me.MLVsummary.SetActiveView(VIWerror)
        Me.HYPerror.NavigateUrl = BaseUrl & url
        Me.LBerror.Text = Me.Resource.getValue("DisplayWrongPageAccess")
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewSummaryPath.DisplaySessionTimeout
        Dim idCommunity As Integer = PreloadIdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = lm.Comol.Modules.EduPath.BusinessLogic.RootObject.SummaryPath(PreloadFromSummary, PreloadIdCommunity)

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If
        webPost.Redirect(dto)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As lm.Comol.Modules.EduPath.Domain.ModuleEduPath.ActionType) Implements lm.Comol.Modules.EduPath.Presentation.IViewBaseSummary.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, , InteractionType.UserWithLearningObject)
    End Sub
    Private Sub InitializeFilters() Implements IViewSummaryPath.InitializeFilters
        If Not Page.IsPostBack Then
            MLVsummary.SetActiveView(VIWsummary)

            PresetOrder("path", True)

            If (View = UserView.grouped) Then
                MLVsummaryUser.SetActiveView(VIWsummaryUserGrouped)
                CHBviewcommunity.Visible = False
            Else
                MLVsummaryUser.SetActiveView(VIWsummaryUserUngrouped)
                CHBviewcommunity.Visible = True
            End If

        End If

        HYPback.NavigateUrl = BaseUrl & RootObject.SummaryIndex(FromSummary, SummaryIdCommunity)

        If Not Page.IsPostBack Then
            Dim statuses As IEnumerable(Of String) = [Enum].GetNames(GetType(StatusFilter))
            Dim statuses1 As IEnumerable(Of Int32) = [Enum].GetValues(GetType(StatusFilter))
            Dim statInternaz = (From item In statuses1 Select New With {.value = item, .text = Me.Resource.getValue("Enum." + [Enum].GetName(GetType(StatusFilter), item))}).ToList()
            RBLfilterStatus.RepeatDirection = RepeatDirection.Horizontal
            RBLfilterStatus.RepeatLayout = RepeatLayout.Flow

            RBLfilterStatus.DataSource = statInternaz
            RBLfilterStatus.DataTextField = "text"
            RBLfilterStatus.DataValueField = "value"
            RBLfilterStatus.DataBind()
            RBLfilterStatus.SelectedValue = 0
        End If

        Dim stat As dtoAllPaths = Nothing
        If Not Page.IsPostBack Then

            Dim p As New PagerBase(20, 0)

            stat = ServiceEP.ServiceStat.GetPathsCount(DateTime.Now, 0, 20, Order, Ascending, StartEndDeadline)

            p.Count = stat.Total - 1
            p.initialize()

            Pager = p
        Else

            Dim list As IList(Of Int32) = Nothing

            If DDLfilterOrganization.SelectedIndex > 0 Then
                Dim scm As New ServiceCommunityManagement(PageUtility.CurrentContext)
                list = scm.GetOrganizationIdChildrenCommunities(DDLfilterOrganization.SelectedValue, "")
            End If

            stat = ServiceEP.ServiceStat.GetPathsCount(DateTime.Now, Pager.PageIndex, Pager.PageSize, Order, Ascending, StartEndDeadline, PathFilter, CommunityFilter, StartDateFilter, EndDateFilter, StatusFilter, list)

            Pager.Count = stat.Total
            Pager.initialize()
        End If

        LBdetailsUserNotStarted.Text = stat.NotStarted
        LBdetailsUserCompleted.Text = stat.Completed
        LBdetailsUserStarted.Text = stat.Started

        If View = UserView.ungrouped Then
            RPTpaths.DataSource = stat.Paths
            RPTpaths.DataBind()
            CHBviewcommunity.Visible = True
        Else
            RPTcommunities.DataSource = stat.PathsByCommunity
            RPTcommunities.DataBind()
            CHBviewcommunity.Visible = False
        End If


    End Sub
    Private Sub LoadOrganizations(ByVal idUser As Integer) Implements IViewSummaryPath.LoadAvailableOrganizations
        Dim pfm As New lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService(Me.PageUtility.CurrentContext)

        Dim Organizations As List(Of Organization) = pfm.GetAvailableOrganizations(idUser, SearchCommunityFor.CommunityManagement)
        Me.DDLfilterOrganization.DataSource = Organizations
        Me.DDLfilterOrganization.DataValueField = "Id"
        Me.DDLfilterOrganization.DataTextField = "Name"
        Me.DDLfilterOrganization.DataBind()
        If Organizations.Count > 1 Then
            Me.DDLfilterOrganization.Items.Insert(0, New ListItem(Me.Resource.getValue("DDLorganizations." & -1), -1))
        End If
    End Sub
#End Region

#Region "Repeater"
    Private Sub RPTpaths_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTpaths.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dto As dtoPathInfo = e.Item.DataItem

            Dim olbl As Label

            olbl = e.Item.FindControl("LBpathName")
            olbl.Text = dto.PathName

            olbl = e.Item.FindControl("LBtimerange")
            olbl.Text = dto.Deadline

            olbl = e.Item.FindControl("LBcommunityName")
            olbl.Text = dto.CommunityName

            olbl = e.Item.FindControl("LBnotstarted")
            olbl.Text = dto.NotStarted

            olbl = e.Item.FindControl("LBstarted")
            olbl.Text = dto.Started


            olbl = e.Item.FindControl("LBcompleted")
            olbl.Text = dto.Completed

            Dim hyp As HyperLink

            'temp
            dto.CanStat = True
            dto.CanManage = True

            hyp = e.Item.FindControl("HYPstats")

            hyp.NavigateUrl = BaseUrl & RootObject.PathStatistics(dto.IdPath, dto.IdCommunity, DateTime.Now, False, SummaryType.Path, SummaryIdCommunity, FromSummary, dto.IsMooc)

            hyp.Visible = dto.CanStat

            hyp = e.Item.FindControl("HYPedit")
            hyp.NavigateUrl = BaseUrl & RootObject.PathView(dto.IdPath, dto.IdCommunity, EpViewModeType.Manage, False, dto.IsMooc)
            hyp.Visible = dto.CanManage

            hyp = e.Item.FindControl("HYPsettings")
            hyp.NavigateUrl = BaseUrl & RootObject.PathManagement(dto.IdCommunity, dto.IdPath, "-2", dto.PathType, dto.IsMooc)
            hyp.Visible = dto.CanManage

        ElseIf e.Item.ItemType = ListItemType.Header Then

            'Dim olabel As Label

            'olabel = e.Item.FindControl("LBpathNameHeader")
            'Me.Resource.setLabel(olabel)
            'olabel = e.Item.FindControl("LBcommunityNameHeader")
            'Me.Resource.setLabel(olabel)
            'olabel = e.Item.FindControl("LBcompletionHeader")
            'Me.Resource.setLabel(olabel)
            'olabel = e.Item.FindControl("LBtimerangeHeader")
            'Me.Resource.setLabel(olabel)
            'olabel = e.Item.FindControl("LBactionsHeader")
            'Me.Resource.setLabel(olabel)

            'Dim ob As UC_OrderBy
            'ob = e.Item.FindControl("OBpathname")
            'AddHandler ob.OnOrderBy, AddressOf OrderBy
        End If
    End Sub

    Public Sub OrderBy(e As OrderByEventArgs)
        Order = e.Column
        Ascending = e.Ascending

        'Dim x = (From item As Control In Me.Controls.AsQueryable Where item.GetType() Is GetType(UC_OrderBy) And CType(item, UC_OrderBy).Column <> e.Column Select item).ToList()

        'For Each el As UC_OrderBy In x
        '    el.Status = OrderByStatus.None
        'Next

        Select Case e.Column
            Case "path"
                OBtimerange.Status = OrderByStatus.None
            Case "timerange"
                OBpathname.Status = OrderByStatus.None
            Case Else

        End Select


        InitializeFilters()

    End Sub
    Private Sub RPTcommunities_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTcommunities.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            'Dim dto As dtoPathInfo = e.Item.DataItem
            Dim dto As IGrouping(Of Int32, List(Of dtoPathInfo)) = e.Item.DataItem
            Dim olabel As Label
            Dim idcomm As Int32 = dto.Key

            Dim c As New COL_Comunita(idcomm)
            Dim name As String = c.EstraiNomeBylingua(LinguaID)

            olabel = e.Item.FindControl("LBcommunityName")
            olabel.Text = name

            Dim rpt As Repeater
            rpt = e.Item.FindControl("RPTpaths")
            AddHandler rpt.ItemDataBound, AddressOf RPTcommunitypaths_ItemDataBound
            rpt.DataSource = dto.First().ToList()
            rpt.DataBind()

            Dim hyp As HyperLink
            hyp = e.Item.FindControl("HYPpathlist")

            Dim p As ModuleEduPath = CurrentPresenter.GetModulePermission(idcomm)
            If p.Administration Then
                hyp.NavigateUrl = Me.BaseUrl & RootObject.EduPathList(idcomm, EpViewModeType.Manage, PreloadIsMooc)
            Else
                hyp.NavigateUrl = Me.BaseUrl & RootObject.EduPathList(idcomm, EpViewModeType.View, PreloadIsMooc)
            End If


        ElseIf e.Item.ItemType = ListItemType.Header Then


        End If
    End Sub

    Private Sub RPTcommunitypaths_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dto As dtoPathInfo = CType(e.Item.DataItem, dtoPathInfo)
            Dim olabel As Label
            olabel = e.Item.FindControl("LBpathName")
            olabel.Text = dto.PathName

            olabel = e.Item.FindControl("LBtimerange")
            olabel.Text = dto.Deadline

            olabel = e.Item.FindControl("LBdetailsUserNotStarted")
            olabel.Text = dto.NotStarted
            olabel = e.Item.FindControl("LBdetailsUserStarted")
            olabel.Text = dto.Started
            olabel = e.Item.FindControl("LBdetailsUserCompleted")
            olabel.Text = dto.Completed

            Dim hyp As HyperLink

            'temp
            dto.CanStat = True
            dto.CanManage = True

            hyp = e.Item.FindControl("HYPstats")

            hyp.NavigateUrl = BaseUrl & RootObject.PathStatistics(dto.IdPath, dto.IdCommunity, DateTime.Now, False, dto.IsMooc)

            hyp.Visible = dto.CanStat

            hyp = e.Item.FindControl("HYPedit")
            hyp.NavigateUrl = BaseUrl & RootObject.PathView(dto.IdPath, dto.IdCommunity, EpViewModeType.Manage, False, dto.IsMooc)
            hyp.Visible = dto.CanManage

            hyp = e.Item.FindControl("HYPsettings")
            hyp.NavigateUrl = BaseUrl & RootObject.PathManagement(dto.IdCommunity, dto.IdPath, "-2", dto.PathType, dto.IsMooc)
            hyp.Visible = dto.CanManage
        End If
    End Sub
#End Region

#Region "Filters"
    Private Sub BTNupdate_Click(sender As Object, e As System.EventArgs) Handles BTNupdate.Click
        OrganizationFilter = DDLfilterOrganization.SelectedValue
        CommunityFilter = TXBfilterCommunity.Text
        PathFilter = TXBfilterPath.Text
        StatusFilter = RBLfilterStatus.SelectedValue

        Pager.PageIndex = 0

        If RDPStartDate.SelectedDate.HasValue Then
            StartDateFilter = RDPStartDate.SelectedDate.Value.ToShortDateString
        End If
        If RDPEndDate.SelectedDate.HasValue Then
            EndDateFilter = RDPEndDate.SelectedDate.Value.ToShortDateString
        End If
        InitializeFilters()
        If Not String.IsNullOrWhiteSpace(CommunityFilter) Then
            CHBviewcommunity.Checked = True
        End If
    End Sub
    Private Sub BTNgrouping_Click(sender As Object, e As System.EventArgs) Handles BTNgrouping.Click
        If MLVsummaryUser.GetActiveView().ID = VIWsummaryUserGrouped.ID Then
            MLVsummaryUser.SetActiveView(VIWsummaryUserUngrouped)
            View = UserView.ungrouped
        Else
            MLVsummaryUser.SetActiveView(VIWsummaryUserGrouped)
            View = UserView.grouped
        End If

        BTNgrouping.Text = Resource.getValue("BTNgrouping." + View.ToString())
        InitializeFilters()
    End Sub
    Private Sub PGgridBottom_OnPageSelected() Handles PGgridBottom.OnPageSelected
        InitializeFilters()
    End Sub
    Private Sub PGgridTop_OnPageSelected() Handles PGgridTop.OnPageSelected
        InitializeFilters()
    End Sub
#End Region

    
End Class