Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.EduPath.Domain
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.EduPath.BusinessLogic
Imports lm.ActionDataContract
Imports System.Linq
'Imports lm.Comol.Core.BaseModules.ProfileManagement.Business
'Imports lm.Comol.Core.BaseModules.CommunityManagement.Business
Imports lm.Comol.Modules.EduPath.Presentation

Public Class EPSummaryUser
    Inherits EPlitePageBaseEduPath
    Implements IViewSummaryUser

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

    Private _Presenter As SummaryUserPresenter
    Private ReadOnly Property CurrentPresenter() As SummaryUserPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New SummaryUserPresenter(Me.PageUtility.CurrentContext, Me)
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
    Private ReadOnly Property PreloadReloadFilters As Boolean Implements IViewSummaryUser.PreloadReloadFilters
        Get
            If String.IsNullOrEmpty(Me.Request.QueryString("Reload")) OrElse Me.Request.QueryString("Reload") <> "True" Then
                Return False
            Else
                Return True
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewSummaryUser.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadFromSummary As SummaryType Implements IViewSummaryUser.PreloadFromSummary
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of SummaryType).GetByString(Request.QueryString("From"), lm.Comol.Modules.EduPath.Domain.SummaryType.PortalIndex)
        End Get
    End Property
    Private ReadOnly Property PreloadIdUser As Integer Implements IViewSummaryUser.PreloadIdUser
        Get
            If IsNumeric(Me.Request.QueryString("idUser")) Then
                Return CInt(Me.Request.QueryString("idUser"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private Property SummaryIdCommunity As Integer Implements IViewSummaryUser.SummaryIdCommunity
        Get
            Return ViewStateOrDefault("SummaryIdCommunity", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("SummaryIdCommunity") = value
        End Set
    End Property
    Private Property FromSummary As SummaryType Implements IViewSummaryUser.FromSummary
        Get
            Return ViewStateOrDefault("FromSummary", SummaryType.CommunityIndex)
        End Get
        Set(value As SummaryType)
            Me.ViewState("FromSummary") = value
        End Set
    End Property
    Private Property SummaryIdUser As Integer Implements IViewSummaryUser.SummaryIdUser
        Get
            Return ViewStateOrDefault("SummaryIdUser", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("SummaryIdUser") = value
        End Set
    End Property
    Private Property Pager As PagerBase Implements IViewSummaryUser.Pager
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
    Private Property AllowOrganizationSelection As Boolean Implements IViewSummaryUser.AllowOrganizationSelection
        Get
            Return ViewStateOrDefault("AllowOrganizationSelection", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowOrganizationSelection") = value
            Me.DIVfilterorganization.Visible = value
        End Set
    End Property
#End Region

#Region "Controls"
    Protected ReadOnly Property CookieName() As String
        Get
            Return "COMOL_UserPathsStatistics"
        End Get
    End Property
    Protected ReadOnly Property DisplayMessageToken() As String
        Get
            Return Resource.getValue("DisplayMessageToken.UserPathsStatistics")
        End Get
    End Property
    Protected ReadOnly Property DisplayTitleToken() As String
        Get
            Return Resource.getValue("DisplayTitleToken.UserPathsStatistics")
        End Get
    End Property


    Dim _selecteduser As litePerson
    Public Property SelectedUser As litePerson
        Get
            Return _selecteduser
        End Get
        Set(value As litePerson)
            _selecteduser = value
        End Set
    End Property
    'Public Property SelectedIdUser As Integer
    '    Get
    '        Return ViewStateOrDefault("SelectedIdUser", 0)
    '    End Get
    '    Set(value As Integer)
    '        ViewState("SelectedIdUser") = value
    '    End Set
    'End Property
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

    Public Sub OrderBy(e As OrderByEventArgs)
        Order = e.Column
        Ascending = e.Ascending

        'Dim x = (From item As Control In Me.Controls.AsQueryable Where item.GetType() Is GetType(UC_OrderBy) And CType(item, UC_OrderBy).Column <> e.Column Select item).ToList()

        'For Each el As UC_OrderBy In x
        '    el.Status = OrderByStatus.None
        'Next

        Select Case e.Column
            Case "path"
                'OBpathname.Status = OrderByStatus.None
                OBtimerange.Status = OrderByStatus.None
                OBtimerangegrouped.Status = OrderByStatus.None
                OBcommunitypath.Status = OrderByStatus.None
            Case "timerange"
                OBpathname.Status = OrderByStatus.None
                'OBtimerange.Status = OrderByStatus.None
                OBtimerangegrouped.Status = OrderByStatus.None
                OBcommunitypath.Status = OrderByStatus.None
            Case "community/path"
                OBpathname.Status = OrderByStatus.None
                OBtimerange.Status = OrderByStatus.None
                OBtimerangegrouped.Status = OrderByStatus.None
                'OBcommunitypath.Status = OrderByStatus.None
            Case Else

        End Select


        CurrentPresenter.LoadData()

    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PGgridTop.Pager = Me.Pager
        Me.PGgridBottom.Pager = Me.Pager
    End Sub

    Private Sub EPSummaryUser_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Me.Master.ShowDocType = True
    End Sub


    Public Function Status(s As String) As String

        Select Case s
            Case "notstarted"
                Return "gray"
            Case "started"
                Return "yellow"
            Case "completed"
                Return "green"
            Case Else
                Return ""
        End Select

    End Function

    Public Function StatusTitle(s As String) As String

        Select Case s
            Case "notstarted"
                Return Me.Resource.getValue("EduPathTranslations.NotStarted")
            Case "started"
                Return Me.Resource.getValue("EduPathTranslations.Started")
            Case "completed"
                Return Me.Resource.getValue("EduPathTranslations.Completed")
            Case Else
                Return ""
        End Select

    End Function




    'Dim _status As String
    'Public Property StatusFilter As String
    '    Get
    '        If (String.IsNullOrEmpty(Request.QueryString("status"))) Then
    '            _status = RBLfilterStatus.SelectedValue
    '            Return _status
    '        Else
    '            _status = Request.QueryString("status")
    '            Return _status
    '        End If
    '    End Get
    '    Set(value As String)
    '        _status = value
    '        RBLfilterStatus.SelectedValue = _status

    '        'set dropdown

    '    End Set
    'End Property

    Public Sub SetQueryString(key As String, value As String)
        Dim nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString())
        nameValues.Set(key, value)
        Dim url As String = Request.Url.AbsolutePath
        Dim updatedQueryString As String = "?" + nameValues.ToString()
        Response.Redirect(url + updatedQueryString)
    End Sub

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


    Public Function StatusLocked(locked As Boolean) As String
        If locked Then
            Return "locked"
        Else
            Return "unlocked"
        End If
    End Function

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
            .setLabel(LBdetailsUserTitle)
            .setHyperLink(HYPback, False, False)
            HYPerror.Text = HYPback.Text
            HYPerror.ToolTip = HYPback.ToolTip
            .setButton(BTNupdate)
            .setLiteral(LTpageBottom)
            .setLiteral(LTpageTop)
            .setCheckBox(CHBviewcommunity)

            .setLinkButton(LNBuserPathsStatisticsToCsv, False, True)
            '.setLinkButton(LNBuserPathsStatisticsToPdf, False, True)
            .setLinkButton(LNBuserPathsStatisticsToXml, False, True)
            .setLinkButton(LNBuserPathsCertificationToCsv, False, True)
            .setLinkButton(LNBuserPathsCertificationToXml, False, True)


            .setLabel(LBpathNameHeader)
            .setLabel(LBcommunityNameHeader)
            .setLabel(LBactionsHeader)
            .setLabel(LBtimerangeHeader)
            .setLabel(LBcompletionHeader)
            .setLabel(LBcommunityPathNameHeader)

            LBactionsgroupedHeader.Text = LBactionsHeader.Text
            LBtimerangegroupedHeader.Text = LBtimerangeHeader.Text
            LBcompletiongroupedHeader.Text = LBcompletionHeader.Text

            LBdetailsUserCompletedLabel.Text = .getValue("EduPathTranslations.CompletedS")
            LBdetailsUserStartedLabel.Text = .getValue("EduPathTranslations.StartedS")
            LBdetailsUserNotStartedLabel.Text = .getValue("EduPathTranslations.NotStartedS")
            LBdetailsStat.Text = .getValue("EduPathTranslations.Paths")
            BTNgrouping.Text = .getValue("BTNgrouping." + View.ToString())

            Master.ServiceTitle = Resource.getValue("ServiceTitle." & SummaryType.User.ToString())

        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

#End Region

#Region "Implements"
    Private Sub DisplayNoPermission() Implements IViewSummaryUser.DisplayNoPermission
        Me.BindNoPermessi()
    End Sub
    Private Sub DisplayWrongPageAccess() Implements IViewSummaryUser.DisplayWrongPageAccess
        Me.Master.ServiceNopermission = Me.Resource.getValue("DisplayWrongPageAccess")
        Me.Master.ShowNoPermission = True
    End Sub
    Private Sub DisplayWrongPageAccess(ByVal url As String) Implements IViewSummaryUser.DisplayWrongPageAccess
        Me.MLVsummary.SetActiveView(VIWerror)
        Me.HYPerror.NavigateUrl = BaseUrl & url
        Me.LBerror.Text = Me.Resource.getValue("DisplayWrongPageAccess")
    End Sub
    Private Sub DisplayNoUserSelected(ByVal url As String) Implements IViewSummaryUser.DisplayNoUserSelected
        Me.MLVsummary.SetActiveView(VIWerror)
        Me.HYPerror.NavigateUrl = BaseUrl & url
        Me.LBerror.Text = Me.Resource.getValue("DisplayNoUserSelected")
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewSummaryUser.DisplaySessionTimeout
        Dim idCommunity As Integer = PreloadIdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = lm.Comol.Modules.EduPath.BusinessLogic.RootObject.SummaryUser(PreloadFromSummary, PreloadIdUser, PreloadIdCommunity)

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If
        webPost.Redirect(dto)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As lm.Comol.Modules.EduPath.Domain.ModuleEduPath.ActionType) Implements lm.Comol.Modules.EduPath.Presentation.IViewBaseSummary.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, , InteractionType.UserWithLearningObject)
    End Sub

    Private Sub LoadData(ByVal idUser As Integer, person As litePerson) Implements lm.Comol.Modules.EduPath.Presentation.IViewSummaryUser.LoadData
        If Not Page.IsPostBack Then
            MLVsummary.SetActiveView(VIWsummary)
            If (View = UserView.grouped) Then
                MLVsummaryUser.SetActiveView(VIWsummaryUserGrouped)
                CHBviewcommunity.Visible = False
            Else
                MLVsummaryUser.SetActiveView(VIWsummaryUserUngrouped)
                CHBviewcommunity.Visible = True
            End If

        End If
        HYPback.NavigateUrl = BaseUrl & RootObject.SummaryIndex(FromSummary, SummaryIdCommunity)

        SelectedUser = person

        If Not SelectedUser Is Nothing Then

            Dim statuses As IEnumerable(Of String) = [Enum].GetNames(GetType(StatusFilter))
            Dim statuses1 As IEnumerable(Of Int32) = [Enum].GetValues(GetType(StatusFilter))

            If (Not Page.IsPostBack) Then

                Dim statInternaz = (From item In statuses1 Select New With {.value = item, .text = Me.Resource.getValue("Enum." + [Enum].GetName(GetType(StatusFilter), item))}).ToList()
                RBLfilterStatus.RepeatDirection = RepeatDirection.Horizontal
                RBLfilterStatus.RepeatLayout = RepeatLayout.Flow

                RBLfilterStatus.DataSource = statInternaz
                RBLfilterStatus.DataTextField = "text"
                RBLfilterStatus.DataValueField = "value"
                RBLfilterStatus.DataBind()
                RBLfilterStatus.SelectedValue = 0
            End If



            Try


                DDLfilterOrganization.SelectedValue = OrganizationFilter
                TXBfilterCommunity.Text = CommunityFilter
                TXBfilterPath.Text = PathFilter
                RBLfilterStatus.SelectedValue = StatusFilter
            Catch ex As Exception

            End Try


            Try
                RDPStartDate.SelectedDate = DateTime.Parse(StartDateFilter)
            Catch ex As Exception
                RDPStartDate.Clear()
            End Try
            Try
                RDPEndDate.SelectedDate = DateTime.Parse(EndDateFilter)
            Catch ex As Exception
                RDPEndDate.Clear()
            End Try

            Dim list As IList(Of Int32) = Nothing

            If DDLfilterOrganization.SelectedValue > 0 Then
                Dim scm As New lm.Comol.Core.BaseModules.CommunityManagement.Business.ServiceCommunityManagement(PageUtility.CurrentContext)

                list = scm.GetOrganizationIdChildrenCommunities(DDLfilterOrganization.SelectedValue, "")
            End If




            Dim stat As dtoUserPaths = Nothing
            If Not Page.IsPostBack Then


                Dim p As New PagerBase(20, 0)

                stat = ServiceStat.GetSelectedUserPathsCount(PreloadIsMooc, idUser, DateTime.Now, 0, 20, StartEndDeadline, Order, Ascending, PathFilter, CommunityFilter, StartDateFilter, EndDateFilter, StatusFilter, list)

                p.Count = stat.Total - 1
                p.initialize()

                Pager = p
            Else
                stat = ServiceStat.GetSelectedUserPathsCount(PreloadIsMooc, idUser, DateTime.Now, Pager.PageIndex, Pager.PageSize, StartEndDeadline, Order, Ascending, PathFilter, CommunityFilter, StartDateFilter, EndDateFilter, StatusFilter, list)
                Pager.Count = stat.Total
                Pager.initialize()
            End If

            LBdetailsUserNotStarted.Text = stat.NotStarted
            LBdetailsUserStarted.Text = stat.Started
            LBdetailsUserCompleted.Text = stat.Completed

            LBdetailsUser.Text = SelectedUser.SurnameAndName

            If View = UserView.ungrouped Then
                RPTpaths.DataSource = stat.Paths
                RPTpaths.DataBind()
                CHBviewcommunity.Visible = True
            Else
                CHBviewcommunity.Visible = False
                RPTcommunities.DataSource = stat.PathsByCommunity
                RPTcommunities.DataBind()
            End If


        Else
            LBerror.Text = Resource.getValue("Error.Url")
            MLVsummary.SetActiveView(VIWerror)
        End If
    End Sub

    Private Sub LoadOrganizations(ByVal idUser As Integer) Implements IViewSummaryUser.LoadAvailableOrganizations
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
    Private Function GetFileName(ByVal type As lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType) As String Implements IViewSummaryUser.GetFileName
        Dim filename As String = Resource.getValue("Export.Filename.User")
        Dim oDate As DateTime = DateTime.Now
        If String.IsNullOrEmpty(filename) Then
            filename = "User_{0}_Statistics_{1}_{2}_{3}"
        End If
        Dim displayname As String = Me.Resource.getValue("EduPathTranslations." & EduPathTranslations.AnonymousUser.ToString)
        If SummaryIdUser <> 0 Then
            Dim p As Person = QSservice.GetItem(Of Person)(SummaryIdUser)
            If Not IsNothing(p) AndAlso p.TypeID <> UserTypeStandard.Guest Then
                displayname = p.SurnameAndName
            End If

        End If
        Return lm.Comol.Core.DomainModel.Helpers.Export.ExportBaseHelper.HtmlCheckFileName(String.Format(filename, displayname, oDate.Year, oDate.Month, oDate.Day) & IIf(type <> lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType.pdf, "." & type.ToString, ""))
    End Function
    Private Function GetQuizInfos(qInfos As List(Of dtoUserPathQuiz)) As List(Of dtoUserPathQuiz) Implements IViewSummaryUser.GetQuizInfos
        For Each pQuiz As dtoUserPathQuiz In qInfos
            Dim questionnaire As COL_Questionario.LazyQuestionnaire = QSservice.GetItem(Of COL_Questionario.LazyQuestionnaire)(CInt(pQuiz.IdQuestionnaire))
            If Not IsNothing(questionnaire) Then
                pQuiz.QuestionnaireInfo.AllowMultipleAttempts = (questionnaire.IdType = COL_Questionario.QuestionnaireType.RandomMultipleAttempts) OrElse (questionnaire.MaxAttempts > 1)
                ' DA VERIFICARE CON I PERCORSI FORMATIVI A VALUTAZIONE !!!
                pQuiz.QuestionnaireInfo.EvaluationActive = (questionnaire.IdType = COL_Questionario.QuestionnaireType.RandomMultipleAttempts)
                pQuiz.QuestionnaireInfo.MaxAttemptsNumber = questionnaire.MaxAttempts
                pQuiz.QuestionnaireInfo.Name = QSservice.GetItemName(pQuiz.IdQuestionnaire, LinguaID)
                pQuiz.QuestionnaireInfo.MinScore = questionnaire.MinScore
                pQuiz.QuestionnaireInfo.EvaluationScale = questionnaire.EvaluationScale
                Dim answers As List(Of COL_Questionario.LazyUserResponse) = QSservice.GetQuestionnaireAttempts(CInt(pQuiz.IdQuestionnaire), pQuiz.IdPerson, 0)
                If (Not IsNothing(answers) AndAlso answers.Any()) Then
                    Dim index As Integer = 1
                    For Each answer As COL_Questionario.LazyUserResponse In answers.ToList()
                        pQuiz.Answers.Add(New dtoUserQuizAnswer() With {.AttemptNumber = index, .CompletedOn = answer.CompletedOn, .CorrectAnswers = answer.CorrectAnswers, _
                                                                       .IdAnswer = answer.Id, .IsDeleted = answer.IsDeleted, .QuestionsCount = answer.QuestionsCount, .QuestionsSkipped = answer.QuestionsSkipped, _
                                                                        .RelativeScore = answer.RelativeScore, .Score = answer.Score, .SemiCorrectAnswers = answer.SemiCorrectAnswers, .UngradedAnswers = answer.UngradedAnswers, .WrongAnswers = answer.WrongAnswers})
                        index += 1
                        pQuiz.EvaluationsInfo.Add(answer.Id, QSservice.CalculateComplation(answer))
                    Next
                End If
            End If
        Next
        Return qInfos
    End Function
#End Region

#Region "Filters Load"

#End Region

#Region "Filters"
    Private Sub BTNgrouping_Click(sender As Object, e As System.EventArgs) Handles BTNgrouping.Click
        If MLVsummaryUser.GetActiveView().ID = VIWsummaryUserGrouped.ID Then
            MLVsummaryUser.SetActiveView(VIWsummaryUserUngrouped)
            View = UserView.ungrouped
        Else
            MLVsummaryUser.SetActiveView(VIWsummaryUserGrouped)
            View = UserView.grouped
        End If
        BTNgrouping.Text = Resource.getValue("BTNgrouping." + View.ToString())
        CurrentPresenter.LoadData()
    End Sub
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
        CurrentPresenter.LoadData()

    End Sub

    Private Sub PGgridBottom_OnPageSelected() Handles PGgridBottom.OnPageSelected

        CurrentPresenter.LoadData()
    End Sub
    Private Sub PGgridTop_OnPageSelected() Handles PGgridTop.OnPageSelected
        CurrentPresenter.LoadData()
    End Sub

#End Region


    Public Const StartEndTitle As String = "{0} - {1}"
    Public StartEndDeadline As String = "<span class='from {2}'><span class='icon'>&nbsp;</span>{0}</span><span class='to {3}'><span class='icon'>&nbsp;</span>{1}</span>"

    Public Function StartEnd(dto As dtoUserPathInfo)
        Return String.Format(StartEndTitle, dto.StartDate, dto.EndDate)
    End Function

#Region "Repeaters"
    Private Sub RPTpaths_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTpaths.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dto As dtoUserPathInfo = CType(e.Item.DataItem, dtoUserPathInfo)
            Dim olabel As Label
            olabel = e.Item.FindControl("LBpathName")
            olabel.Text = dto.PathName

            Dim c As New COL_Comunita(dto.IdCommunity)
            Dim name As String = c.EstraiNomeBylingua(LinguaID)

            olabel = e.Item.FindControl("LBcommunityName")
            olabel.Text = name

            olabel = e.Item.FindControl("LBtimerange")
            olabel.Text = dto.Deadline

            Dim hyp As HyperLink
            hyp = e.Item.FindControl("HYPplay")
            hyp.NavigateUrl = BaseUrl & RootObject.ViewFullPlay(dto.IdPath, dto.IdCommunity, preloadismooc)
            hyp.Visible = dto.CanPlay

            hyp = e.Item.FindControl("HYPstats")
            If dto.CanManage Then
                hyp.NavigateUrl = BaseUrl & RootObject.UserStatisticsManage(dto.IdPath, dto.IdCommunity, dto.IdPerson, ItemType.Path, 0, DateTime.Now, False, SummaryType.User, SummaryIdCommunity, FromSummary, dto.IsMooc)
            Else
                hyp.NavigateUrl = BaseUrl & RootObject.UserStatisticsView(dto.IdPath, dto.IdCommunity, DateTime.Now, False, SummaryType.User, SummaryIdCommunity, FromSummary, dto.IsMooc)
            End If
            hyp.Visible = dto.CanStat

            hyp = e.Item.FindControl("HYPedit")
            hyp.NavigateUrl = BaseUrl & RootObject.PathView(dto.IdPath, dto.IdCommunity, EpViewModeType.Manage, False, PreloadIsMooc)
            hyp.Visible = dto.CanManage

            hyp = e.Item.FindControl("HYPsettings")
            hyp.NavigateUrl = BaseUrl & RootObject.PathManagement(dto.IdCommunity, dto.IdPath, "-2", dto.PathType, dto.IsMooc)
            hyp.Visible = dto.CanManage

            hyp = e.Item.FindControl("HYPcertificates")
            Me.Resource.setHyperLink(hyp, False, True)
            hyp.Text = ""
            hyp.NavigateUrl = Me.BaseUrl + RootObject.EPCertificationUser(dto.IdCommunity, dto.IdPath, dto.IdPerson, dto.IsMooc)

            hyp.Visible = ServiceEP.PathHasSubActivityType(dto.IdPath, SubActivityType.Certificate) AndAlso Not IsNothing(dto.Ps) AndAlso dto.Ps.Status > StatusStatistic.Browsed


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



        End If
    End Sub

    Private Sub RPTcommunities_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTcommunities.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dto As IGrouping(Of Int32, List(Of dtoUserPathInfo)) = e.Item.DataItem
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


        End If
    End Sub
    Private Sub RPTcommunitypaths_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dto As dtoUserPathInfo = CType(e.Item.DataItem, dtoUserPathInfo)
            Dim olabel As Label
            olabel = e.Item.FindControl("LBpathName")
            olabel.Text = dto.PathName

            olabel = e.Item.FindControl("LBtimerange")
            olabel.Text = dto.Deadline

            Dim hyp As HyperLink
            hyp = e.Item.FindControl("HYPplay")
            hyp.NavigateUrl = BaseUrl & RootObject.ViewFullPlay(dto.IdPath, dto.IdCommunity, PreloadIsMooc)
            hyp.Visible = dto.CanPlay

            hyp = e.Item.FindControl("HYPstats")
            If dto.CanManage Then
                hyp.NavigateUrl = BaseUrl & RootObject.UserStatisticsManage(dto.IdPath, dto.IdCommunity, dto.IdPerson, ItemType.Path, 0, DateTime.Now, False, SummaryType.User, SummaryIdCommunity, FromSummary, dto.IsMooc)
            Else
                hyp.NavigateUrl = BaseUrl & RootObject.UserStatisticsView(dto.IdPath, dto.IdCommunity, DateTime.Now, False, SummaryType.User, SummaryIdCommunity, FromSummary, dto.IsMooc)
            End If
            hyp.Visible = dto.CanStat

            hyp = e.Item.FindControl("HYPedit")
            hyp.NavigateUrl = BaseUrl & RootObject.PathView(dto.IdPath, dto.IdCommunity, EpViewModeType.Manage, False, PreloadIsMooc)
            hyp.Visible = dto.CanManage

            hyp = e.Item.FindControl("HYPsettings")
            hyp.NavigateUrl = BaseUrl & RootObject.PathManagement(dto.IdCommunity, dto.IdPath, "-2", dto.PathType, dto.IsMooc)
            hyp.Visible = dto.CanManage


            hyp = e.Item.FindControl("HYPcertificates")
            Me.Resource.setHyperLink(hyp, False, True)
            hyp.Text = ""
            hyp.NavigateUrl = Me.BaseUrl + RootObject.EPCertificationUser(dto.IdCommunity, dto.IdPath, dto.IdPerson, dto.IsMooc)

            hyp.Visible = ServiceEP.PathHasSubActivityType(dto.IdPath, SubActivityType.Certificate)
        End If
    End Sub
#End Region

#Region "Export"
    'Fix this: Dim x = ServiceEP.ServiceStat.GetSubActivitiesCertificateCompleted(DateTime.Now, 0, 0)

    Private Sub LNBuserPathsStatisticsToCsv_Click(sender As Object, e As System.EventArgs) Handles LNBuserPathsStatisticsToCsv.Click
        ExportUserInfo(Helpers.Export.ExportFileType.csv, ExporPathData.Full)
    End Sub
    Private Sub LNBuserPathsStatisticsToXml_Click(sender As Object, e As System.EventArgs) Handles LNBuserPathsStatisticsToXml.Click
        ExportUserInfo(Helpers.Export.ExportFileType.xml, ExporPathData.Full)
    End Sub
    'Private Sub LNBuserPathsStatisticsToPdf_Click(sender As Object, e As System.EventArgs) Handles LNBuserPathsStatisticsToPdf.Click
    '    ExportUserInfo(Helpers.Export.ExportFileType.pdf)
    'End Sub

    Private Sub LNBuserPathsCertificationToCsv_Click(sender As Object, e As System.EventArgs) Handles LNBuserPathsCertificationToCsv.Click
        ExportUserInfo(Helpers.Export.ExportFileType.csv, ExporPathData.Certification)
    End Sub
    Private Sub LNBuserPathsCertificationToXml_Click(sender As Object, e As System.EventArgs) Handles LNBuserPathsCertificationToXml.Click
        ExportUserInfo(Helpers.Export.ExportFileType.xml, ExporPathData.Certification)
    End Sub

    Private Sub ExportUserInfo(exportType As lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType, ByVal export As ExporPathData)
        Dim cookie As HttpCookie
        If Not String.IsNullOrEmpty(CookieName) Then
            cookie = New HttpCookie(CookieName, HDNdownloadTokenValue.Value)
        End If

        Dim list As IList(Of Int32) = Nothing
        If DDLfilterOrganization.SelectedValue > 0 Then
            Dim scm As New lm.Comol.Core.BaseModules.CommunityManagement.Business.ServiceCommunityManagement(PageUtility.CurrentContext)
            list = scm.GetOrganizationIdChildrenCommunities(DDLfilterOrganization.SelectedValue, "")
        End If
        Dim stat As dtoUserPaths = ServiceStat.GetSelectedUserPathsCount(PreloadIsMooc, SummaryIdUser, DateTime.Now, 0, 0, StartEndDeadline, Order, Ascending, PathFilter, CommunityFilter, StartDateFilter, EndDateFilter, StatusFilter, list, (export = ExporPathData.Certification))

        Dim clientFileName As String = GetFileName(exportType)
        Dim translations As New Dictionary(Of EduPathTranslations, String)
        For Each name As String In [Enum].GetNames(GetType(EduPathTranslations))
            translations.Add([Enum].Parse(GetType(EduPathTranslations), name), Me.Resource.getValue("EduPathTranslations." & name))
        Next
        Dim roles As List(Of Comol.Entity.Role) = COL_BusinessLogic_v2.CL_permessi.COL_TipoRuolo.List(PageUtility.LinguaID).OrderBy(Function(t) t.Name).ToList
        Dim roleTranslations As Dictionary(Of Integer, String) = roles.ToDictionary(Function(r) r.ID, Function(r) r.Name)

        Response.Clear()

        Try
            Dim settings As dtoExportConfigurationSetting = ServiceEP.GetExportSetting(0, 0, StatisticsPageType.AdvancedUser, ConfigurationType.Export)
            If exportType = Helpers.Export.ExportFileType.pdf Then
                Dim oTemplate As New lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template
                'oTemplate.Header = Master.getTemplateHeader
                'oTemplate.Footer = Master.getTemplateFooter
                oTemplate.Settings = lm.Comol.Core.DomainModel.Helpers.Export.ExportBaseHelper.GetDefaultPageSettings()
                oTemplate.Settings.Size = DocTemplateVers.PageSize.A4_L

                'ToDo: Export

            Else
                Response.AppendCookie(cookie)
                Response.AddHeader("Content-Disposition", "attachment; filename=" & clientFileName)
                Response.Charset = ""
                Response.ContentEncoding = System.Text.Encoding.Default
                Select Case exportType
                    Case Helpers.Export.ExportFileType.xml
                        Response.ContentType = "application/ms-excel"
                        Dim hXml As New HelperExportToXml(New lm.Comol.Core.Business.BaseModuleManager(PageUtility.CurrentContext), translations, roleTranslations)
                        Response.Write(hXml.UserPathsStatistics(SummaryIdUser, stat.Person, stat, GetQuizInfos(stat.QuizInfos()), settings, export))
                    Case Else
                        Response.ContentType = "text/csv"
                        Dim hCsv As New HelperExportToCsv(New lm.Comol.Core.Business.BaseModuleManager(PageUtility.CurrentContext), translations, roleTranslations)
                        Response.Write(hCsv.UserPathsStatistics(SummaryIdUser, stat.Person, stat, GetQuizInfos(stat.QuizInfos()), settings, export))
                End Select
            End If
        Catch ex As Exception
            Select Case exportType
                Case Helpers.Export.ExportFileType.pdf

                Case Helpers.Export.ExportFileType.xml
                    Response.Write(HelperExportToXml.GetErrorDocument(translations(EduPathTranslations.FileCreationError), translations(EduPathTranslations.StatisticsInfo), DateTime.Now))
                Case Else
                    Response.Write(HelperExportToCsv.GetErrorDocument(translations(EduPathTranslations.FileCreationError), translations(EduPathTranslations.StatisticsInfo), DateTime.Now))
            End Select
        End Try
        Response.End()
    End Sub

#End Region
    
End Class