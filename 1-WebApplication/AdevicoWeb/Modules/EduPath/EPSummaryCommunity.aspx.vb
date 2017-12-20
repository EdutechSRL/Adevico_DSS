Imports lm.Comol.Modules.EduPath.Domain
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.EduPath.BusinessLogic
Imports lm.Comol.Modules.EduPath.Domain.DTO
Imports lm.Comol.Core.BaseModules.ProfileManagement.Business
Imports lm.Comol.Core.BaseModules.CommunityManagement.Business
Imports lm.Comol.Modules.EduPath.Presentation
Imports lm.ActionDataContract

Public Class EPSummaryCommunity
    Inherits PageBase
    Implements IViewSummaryCommunity

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

    Private _Presenter As SummaryCommunityPresenter
    Private ReadOnly Property CurrentPresenter() As SummaryCommunityPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New SummaryCommunityPresenter(Me.PageUtility.CurrentContext, Me)
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
    Private ReadOnly Property PreloadReloadFilters As Boolean Implements IViewSummaryCommunity.PreloadReloadFilters
        Get
            If String.IsNullOrEmpty(Me.Request.QueryString("Reload")) OrElse Me.Request.QueryString("Reload") <> "True" Then
                Return False
            Else
                Return True
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewSummaryCommunity.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadFromSummary As SummaryType Implements IViewSummaryCommunity.PreloadFromSummary
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of SummaryType).GetByString(Request.QueryString("From"), lm.Comol.Modules.EduPath.Domain.SummaryType.PortalIndex)
        End Get
    End Property
    Private ReadOnly Property PreloadSummaryType As SummaryType Implements IViewSummaryCommunity.PreloadSummaryType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of SummaryType).GetByString(Request.QueryString("type"), lm.Comol.Modules.EduPath.Domain.SummaryType.Community)
        End Get
    End Property

    Private Property SummaryIdCommunity As Integer Implements IViewSummaryCommunity.SummaryIdCommunity
        Get
            Return ViewStateOrDefault("SummaryIdCommunity", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("SummaryIdCommunity") = value
        End Set
    End Property
    Private Property FromSummary As lm.Comol.Modules.EduPath.Domain.SummaryType Implements IViewSummaryCommunity.FromSummary
        Get
            Return ViewStateOrDefault("FromSummary", SummaryType.CommunityIndex)
        End Get
        Set(value As SummaryType)
            Me.ViewState("FromSummary") = value
        End Set
    End Property
    Private Property CurrentSummaryType As SummaryType Implements IViewSummaryCommunity.CurrentSummaryType
        Get
            Return ViewStateOrDefault("CurrentSummaryType", SummaryType.Community)
        End Get
        Set(value As SummaryType)
            Me.ViewState("CurrentSummaryType") = value
        End Set
    End Property
    Private Property Pager As PagerBase Implements IViewSummaryCommunity.Pager
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
    Private Property AllowOrganizationSelection As Boolean Implements IViewSummaryCommunity.AllowOrganizationSelection
        Get
            Return ViewStateOrDefault("AllowOrganizationSelection", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowOrganizationSelection") = value
            Me.DIVfilterOrganization.Visible = value
        End Set
    End Property
#End Region

#Region "Controls / Page"
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

    Dim _org As Int32
    Public Property OrganizationFilter As Int32
        Get
            If DDLfilterOrganization.SelectedIndex > -1 Then
                _org = DDLfilterOrganization.SelectedValue
            End If
            _org = GetQuery("orgId", _org)
            Return _org
        End Get
        Set(value As Int32)
            _org = value
            ViewState("orgId") = _org
            If DDLfilterOrganization.SelectedIndex > -1 Then
                Try
                    DDLfilterOrganization.SelectedValue = _org
                Catch
                    DDLfilterOrganization.SelectedIndex = 0
                End Try
            End If
        End Set
    End Property

    Dim _community As String
    Public Property CommunityFilter As String
        Get
            _community = TXBfilterCommunity.Text

            _community = GetQuery("cmnt", _community)
            Return _community
        End Get
        Set(value As String)
            _community = value
            TXBfilterCommunity.Text = _community
        End Set
    End Property

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
        MyBase.SetCulture("pg_Summary", "EduPath")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            '.setLabel(LBservicePathSummaryUser)
            .setLabel(LBfilterCommunityTitle)
            .setLabel(LBfilterOrganizationTitle)
            .setLabel(LBdraftTitle)
            .setLabel(LBlockedTitle)
            .setLabel(LBunlockedTitle)
            '.setLabel(LBfilterEndDateTitle)
            '.setLabel(LBfilterPathTitle)
            '.setLabel(LBfilterStartDateTitle)
            '.setLabel(LBfilterStatusTitle)
            '.setLabel(LBdetailsUserTitle)
            .setHyperLink(HYPback, False, False)
            .setButton(BTNupdate)
            .setLiteral(LTpageBottom)
            .setLiteral(LTpageTop)
            .setLabel(LBcommunityNameHeader)
            .setLabel(LBpathStatusHeader)
            .setLabel(LBactionsHeader)
            '.setCheckBox(CHBviewcommunity)
            'LBdetailsUserCompletedLabel.Text = .getValue("EduPathTranslations.CompletedS")
            'LBdetailsUserStartedLabel.Text = .getValue("EduPathTranslations.StartedS")
            'LBdetailsUserNotStartedLabel.Text = .getValue("EduPathTranslations.NotStartedS")
            'LBdetailsStat.Text = .getValue("EduPathTranslations.Paths")
            'BTNgrouping.Text = .getValue("BTNgrouping." + View.ToString())

            Master.ServiceTitle = Resource.getValue("ServiceTitle." & PreloadSummaryType.ToString)
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub DisplayNoPermission() Implements IViewSummaryCommunity.DisplayNoPermission
        Me.BindNoPermessi()
    End Sub
    Private Sub DisplayWrongPageAccess() Implements IViewSummaryCommunity.DisplayWrongPageAccess
        Me.Master.ServiceNopermission = Me.Resource.getValue("DisplayWrongPageAccess")
        Me.Master.ShowNoPermission = True
    End Sub
    Private Sub DisplayWrongPageAccess(ByVal url As String) Implements IViewSummaryCommunity.DisplayWrongPageAccess
        Me.MLVsummary.SetActiveView(VIWerror)
        Me.HYPerror.NavigateUrl = BaseUrl & url
        Me.LBerror.Text = Me.Resource.getValue("DisplayWrongPageAccess")
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewSummaryCommunity.DisplaySessionTimeout
        Dim idCommunity As Integer = PreloadIdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = lm.Comol.Modules.EduPath.BusinessLogic.RootObject.SummaryGeneric(PreloadFromSummary, PreloadIdCommunity, PreloadSummaryType)

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If
        webPost.Redirect(dto)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As lm.Comol.Modules.EduPath.Domain.ModuleEduPath.ActionType) Implements lm.Comol.Modules.EduPath.Presentation.IViewBaseSummary.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, , InteractionType.UserWithLearningObject)
    End Sub

    Private Sub InitializeFilters(current As lm.Comol.Modules.EduPath.Domain.SummaryType) Implements IViewSummaryCommunity.InitializeFilters
        Master.ServiceTitle = Resource.getValue("ServiceTitle." & current.ToString)
        MLVsummary.SetActiveView(VIWsummary)

        HYPback.NavigateUrl = BaseUrl & RootObject.SummaryIndex(FromSummary, SummaryIdCommunity)

        LBpaths.Text = Me.Resource.getValue("EduPathTranslations.Paths")
        LBpathsOrganization.Text = Me.Resource.getValue("EduPathTranslations.Paths") & " " & Me.Resource.getValue("intoOrg")
        LBorgname.Text = ""

        LBpaths.Visible = (current = SummaryType.Community)
        LBpathsOrganization.Visible = (current = SummaryType.Organization)
    
        If Not Page.IsPostBack Then

            Dim p As New PagerBase(20, 0)
            p.Count = ServiceEP.GetCommunitiesPathsCount()
            p.initialize()


            Pager = p
        Else

            Pager.Count = ServiceEP.GetCommunitiesPathsCount()
            Pager.initialize()
        End If

        LoadSummaryItems(current, True)

    End Sub

    Private Sub LoadOrganizations(ByVal items As List(Of Organization)) Implements IViewSummaryCommunity.LoadAvailableOrganizations
        Me.DDLfilterOrganization.DataSource = items
        Me.DDLfilterOrganization.DataValueField = "Id"
        Me.DDLfilterOrganization.DataTextField = "Name"

        Me.DDLfilterOrganization.DataBind()

        If items.Count > 1 Then
            Me.DDLfilterOrganization.Items.Insert(0, New ListItem(Me.Resource.getValue("DDLorganizations." & -1), -1))
        End If

    End Sub

    Private Sub LoadSummaryItems(ByVal sType As SummaryType, Optional ByVal initialize As Boolean = False) Implements IViewSummaryCommunity.LoadSummaryItems
        Dim stat As DtoCommunitiesPaths
        Dim globalStat As DtoCommunitiesPaths
        Dim status As String = Me.Resource.getValue("PathsStatus")

        If (OrganizationFilter > 0 And sType = SummaryType.Organization) Then

            Dim scm As New ServiceCommunityManagement(PageUtility.CurrentContext)

            Dim list As IList(Of Int32) = scm.GetOrganizationIdChildrenCommunities(DDLfilterOrganization.SelectedValue, "")

            globalStat = ServiceEP.GetCommunitiesPaths(CommunityFilter, 0, 0, Order, Ascending, list)
            stat = ServiceEP.GetCommunitiesPaths(CommunityFilter, Pager.PageIndex, Pager.PageSize, Order, Ascending, list)
            Pager.Count = ServiceEP.GetCommunitiesPathsCount(list)
            If Pager.Count > 0 Then
                Pager.Count -= 1
            End If
            Pager.initialize()
        Else
            If Not (initialize) Then
                Pager.Count = ServiceEP.GetCommunitiesPathsCount()
                If Pager.Count > 0 Then
                    Pager.Count -= 1
                End If
                Pager.initialize()
            End If
            globalStat = ServiceEP.GetCommunitiesPaths(CommunityFilter, 0, 0, Order, Ascending)
            stat = ServiceEP.GetCommunitiesPaths(CommunityFilter, Pager.PageIndex, Pager.PageSize, Order, Ascending)

        End If
        LBdraft.Text = globalStat.DraftCount
        LBlocked.Text = globalStat.LockedCount
        LBunlocked.Text = globalStat.UnlockedCount
        'view = community

        'sysadmin / administrator
        'administrative
        'user

        'all
        'organization

        '-------------------------

        'view = organization

        'sysadmin / administrator
        'administrative
        'user

        'all
        'organization


        RPTcommunities.DataSource = stat.CommunityPaths
        RPTcommunities.DataBind()

        'p.Count = ServiceEP.GetCommunitiesPathsCount()



        ' LBpathsdetailsCommunity.Text = String.Format(status, stat.LockedCount, stat.DraftCount, stat.UnlockedCount)
      
    End Sub
#End Region



    Public Sub OrderBy(e As OrderByEventArgs)
        Order = e.Column
        Ascending = e.Ascending

        'Dim x = (From item As Control In Me.Controls.AsQueryable Where item.GetType() Is GetType(UC_OrderBy) And CType(item, UC_OrderBy).Column <> e.Column Select item).ToList()

        'For Each el As UC_OrderBy In x
        '    el.Status = OrderByStatus.None
        'Next

        'Select Case e.Column
        '    Case "community"
        '        'OBtimerange.Status = OrderByStatus.None
        '    Case "timerange"
        '        'OBpathname.Status = OrderByStatus.None
        '    Case Else

        'End Select


        LoadSummaryItems(CurrentSummaryType, False)

    End Sub

#Region "Filters"
    Private Sub PGgridBottom_OnPageSelected() Handles PGgridBottom.OnPageSelected
        LoadSummaryItems(CurrentSummaryType)
    End Sub

    Private Sub PGgridTop_OnPageSelected() Handles PGgridTop.OnPageSelected
        LoadSummaryItems(CurrentSummaryType)
    End Sub

    Private Sub BTNupdate_Click(sender As Object, e As System.EventArgs) Handles BTNupdate.Click
        If (DDLfilterOrganization.SelectedIndex = -1) Then
            OrganizationFilter = 0
        Else
            OrganizationFilter = DDLfilterOrganization.SelectedValue
        End If

        CommunityFilter = TXBfilterCommunity.Text

        Pager.PageIndex = 0

        LoadSummaryItems(CurrentSummaryType, False)
    End Sub
#End Region


    Private Sub RPTcommunities_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTcommunities.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim status As String = Me.Resource.getValue("PathsStatus")

            Dim dto As DtoCommunityPaths = e.Item.DataItem
            Dim olabel As Label

            olabel = e.Item.FindControl("LBcommunityName")
            olabel.Text = dto.CommunityName

            olabel = e.Item.FindControl("LBpathStatus")
            olabel.Text = String.Format(status, dto.LockedCount, dto.DraftCount, dto.UnlockedCount)

            Dim hyp As HyperLink
            hyp = e.Item.FindControl("HYPstats")
            hyp.NavigateUrl = BaseUrl & RootObject.PathSummary(dto.IdCommunity)

        Else
            If (e.Item.ItemType = ListItemType.Header) Then
                'Dim olabel As Label
                'olabel = e.Item.FindControl("LBcommunityNameHeader")
                'Me.Resource.setLabel(olabel)

                'olabel = e.Item.FindControl("LBpathStatusHeader")
                'Me.Resource.setLabel(olabel)

                'olabel = e.Item.FindControl("LBactionsHeader")
                'Me.Resource.setLabel(olabel)

            End If
        End If
    End Sub

  
End Class