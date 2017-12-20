Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Domain
Imports lm.Comol.Modules.CallForPapers.Domain.Evaluation
Imports lm.Comol.Modules.CallForPapers.Presentation.Evaluation
Imports lm.ActionDataContract
Imports System.Linq


Public Class cpAdvEvalSummary
    Inherits PageBase
    Implements IViewEvaluationsSummary
#Region "Context"
    Private _Presenter As EvaluationsSummaryPresenter
    Private ReadOnly Property CurrentPresenter() As EvaluationsSummaryPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New EvaluationsSummaryPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public ReadOnly Property IdCallAdvCommission As Long Implements IViewBaseSummary.IdCallAdvCommission
        Get
            Return Request.QueryString("acId")
        End Get
    End Property
    Private ReadOnly Property Portalname As String Implements IViewEvaluationsSummary.Portalname
        Get
            Return Resource.getValue("Portalname")
        End Get
    End Property
    Private ReadOnly Property PreloadIdCall As Long Implements IViewEvaluationsSummary.PreloadIdCall
        Get
            If IsNumeric(Me.Request.QueryString("idCall")) Then
                Return CLng(Me.Request.QueryString("idCall"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewEvaluationsSummary.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdSubmitterType As Long Implements IViewEvaluationsSummary.PreloadIdSubmitterType
        Get
            If IsNumeric(Me.Request.QueryString("idType")) Then
                Return CLng(Me.Request.QueryString("idType"))
            Else
                Return -1
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadView As CallStatusForSubmitters Implements IViewEvaluationsSummary.PreloadView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of CallStatusForSubmitters).GetByString(Request.QueryString("View"), CallStatusForSubmitters.SubmissionOpened)
        End Get
    End Property
    Private ReadOnly Property PreloadPageIndex As Integer Implements IViewEvaluationsSummary.PreloadPageIndex
        Get
            If IsNumeric(Me.Request.QueryString("PageIndex")) Then
                Return CInt(Me.Request.QueryString("PageIndex"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadPageSize As Integer Implements IViewEvaluationsSummary.PreloadPageSize
        Get
            If IsNumeric(Me.Request.QueryString("PageSize")) Then
                Return CInt(Me.Request.QueryString("PageSize"))
            Else
                Return 20
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadOrderBy As SubmissionsOrder Implements IViewEvaluationsSummary.PreloadOrderBy
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of SubmissionsOrder).GetByString(Request.QueryString("OrderBy"), SubmissionsOrder.ByEvaluationIndex)
        End Get
    End Property
    Private ReadOnly Property PreloadFilterBy As EvaluationFilterStatus Implements IViewEvaluationsSummary.PreloadFilterBy
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of EvaluationFilterStatus).GetByString(Request.QueryString("Filter"), EvaluationFilterStatus.All)
        End Get
    End Property
    Private ReadOnly Property PreloadSearchForName As String Implements IViewEvaluationsSummary.PreloadSearchForName
        Get
            Return Request.QueryString("SearchForName")
        End Get
    End Property
    Private ReadOnly Property PreloadFilters As dtoEvaluationsFilters Implements IViewEvaluationsSummary.PreloadFilters
        Get
            Dim dto As New dtoEvaluationsFilters
            With dto
                .Ascending = PreloadAscending
                .OrderBy = PreloadOrderBy
                .SearchForName = PreloadSearchForName
                .Status = PreloadFilterBy
                .IdSubmitterType = PreloadIdSubmitterType
                Dim tStatus As New Dictionary(Of lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationStatus, String)
                For Each name As String In [Enum].GetNames(GetType(lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationStatus))
                    tStatus.Add([Enum].Parse(GetType(lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationStatus), name), Me.Resource.getValue("EvaluationTranslations.EvaluationStatus." & name))
                Next
                .TranslationsEvaluationStatus = tStatus
            End With
            Return dto
        End Get
    End Property
    Private ReadOnly Property PreloadAscending As Boolean Implements IViewEvaluationsSummary.PreloadAscending
        Get
            If String.IsNullOrEmpty(Request.QueryString("Ascending")) Then
                Return True
            Else
                Return (Request.QueryString("Ascending") = "true" OrElse Request.QueryString("Ascending") = "True")
            End If
        End Get
    End Property
    Private Property IdCall As Long Implements IViewEvaluationsSummary.IdCall
        Get
            Return ViewStateOrDefault("IdCall", CLng(0))
        End Get
        Set(ByVal value As Long)
            Me.ViewState("IdCall") = value
        End Set
    End Property
    Private Property IdCallCommunity As Integer Implements IViewEvaluationsSummary.IdCallCommunity
        Get
            Return ViewStateOrDefault("IdCallCommunity", CInt(0))
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("IdCallCommunity") = value
        End Set
    End Property
    Private Property IdCallModule As Integer Implements IViewEvaluationsSummary.IdCallModule
        Get
            Return ViewStateOrDefault("IdCallModule", CInt(0))
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("IdCallModule") = value
        End Set
    End Property
    Private Property Pager As lm.Comol.Core.DomainModel.PagerBase Implements IViewEvaluationsSummary.Pager
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
    Private Property CallType As CallForPaperType Implements IViewEvaluationsSummary.CallType
        Get
            Return ViewStateOrDefault("CallType", CallForPaperType.CallForBids)
        End Get
        Set(value As CallForPaperType)
            Me.ViewState("CallType") = value
        End Set
    End Property

    Private Property CurrentFilters As dtoEvaluationsFilters Implements IViewEvaluationsSummary.CurrentFilters
        Get
            Dim filter As dtoEvaluationsFilters = ViewStateOrDefault("CurrentFilters", PreloadFilters)
            filter.SearchForName = Me.TXBusername.Text
            If Me.RBLevaluationStatus.SelectedIndex = -1 Then
                filter.Status = EvaluationFilterStatus.All
            Else
                filter.Status = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of EvaluationFilterStatus).GetByString(Me.RBLevaluationStatus.SelectedValue, EvaluationFilterStatus.All)
            End If
            If Me.DDLsubmitterTypes.SelectedIndex = -1 Then
                filter.IdSubmitterType = -1
            Else
                filter.IdSubmitterType = CLng(DDLsubmitterTypes.SelectedValue)
            End If
            Return filter
        End Get
        Set(value As dtoEvaluationsFilters)
            ViewState("CurrentFilters") = value
            TXBusername.Text = value.SearchForName
        End Set
    End Property
    Private Property CurrentFilterBy As EvaluationFilterStatus Implements IViewEvaluationsSummary.CurrentFilterBy
        Get
            Return ViewStateOrDefault("CurrentFilterBy", EvaluationFilterStatus.All)
        End Get
        Set(ByVal value As EvaluationFilterStatus)
            Me.ViewState("CurrentFilterBy") = value
        End Set
    End Property

    Private Property CurrentOrderBy As SubmissionsOrder Implements IViewEvaluationsSummary.CurrentOrderBy
        Get
            Return ViewStateOrDefault("CurrentOrderBy", SubmissionsOrder.ByEvaluationIndex)
        End Get
        Set(ByVal value As SubmissionsOrder)
            Me.ViewState("CurrentOrderBy") = value
        End Set
    End Property
    Private Property CurrentAscending As Boolean Implements IViewEvaluationsSummary.CurrentAscending
        Get
            Return ViewStateOrDefault("CurrentAscending", True)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("CurrentAscending") = value
        End Set
    End Property
    Private Property PageSize As Integer Implements IViewEvaluationsSummary.PageSize
        Get
            Return ViewStateOrDefault("PageSize", 30)
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("PageSize") = value
        End Set
    End Property
    Private Property AllowExportAll As Boolean Implements IViewEvaluationsSummary.AllowExportAll
        Get
            Return ViewStateOrDefault("AllowExportAll", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("AllowExportAll") = value
            '   LNBexportAllEvaluationsSummaryToXLS.Visible = value
            LNBexportAllEvaluationsSummaryToCsv.Visible = value
            ' LNBexportAllEvaluationsSummaryData.Visible = value
            'LNBexportAllEvaluationsSummaryDataToCsv.Visible = value
            '  LNBexportFullEvaluationsSummaryDataToXml.Visible = value
            'LNBexportFullEvaluationsSummaryDataToCsv.Visible = value
        End Set
    End Property
    Private Property AllowExportCurrent As Boolean Implements IViewEvaluationsSummary.AllowExportCurrent
        Get
            Return ViewStateOrDefault("AllowExportCurrent", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("AllowExportCurrent") = value
            '      LNBexportFilteredEvaluationsSummaryToXLS.Visible = value
            LNBexportFilteredEvaluationsSummaryToCsv.Visible = value
        End Set
    End Property
    Private ReadOnly Property AnonymousDisplayname As String Implements IViewEvaluationsSummary.AnonymousDisplayname
        Get
            Return Resource.getValue("AnonymousOwnerName")
        End Get
    End Property
    Private ReadOnly Property UnknownDisplayname As String Implements IViewEvaluationsSummary.UnknownDisplayname
        Get
            Return Resource.getValue("UnknownDisplayname")
        End Get
    End Property
    Private Property AvailableOrderBy As Dictionary(Of lm.Comol.Modules.CallForPapers.Domain.SubmissionsOrder, Boolean) Implements IViewBaseSummary.AvailableOrderBy
        Get
            Return ViewStateOrDefault("AvailableOrderBy", New Dictionary(Of lm.Comol.Modules.CallForPapers.Domain.SubmissionsOrder, Boolean))
        End Get
        Set(value As Dictionary(Of lm.Comol.Modules.CallForPapers.Domain.SubmissionsOrder, Boolean))
            ViewState("AvailableOrderBy") = value
        End Set
    End Property

    Private Property CurrentEvaluationType As EvaluationType Implements IViewEvaluationsSummary.CurrentEvaluationType
        Get
            Return ViewStateOrDefault("CurrentEvaluationType", EvaluationType.Sum)
        End Get
        Set(ByVal value As EvaluationType)
            Me.ViewState("CurrentEvaluationType") = value
        End Set
    End Property
    Private Property CallUseFuzzy As Boolean Implements IViewBaseSummary.CallUseFuzzy
        Get
            Return ViewStateOrDefault("CallUseFuzzy", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("CallUseFuzzy") = value
        End Set
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

#Region "Internal"
    Private _AllEvaluated As Boolean
    Private ReadOnly Property CurrentIdSubmitterType As Long
        Get
            If Me.DDLsubmitterTypes.SelectedIndex = -1 Then
                Return -1
            Else
                Return CLng(DDLsubmitterTypes.SelectedValue)
            End If
        End Get

    End Property

    Public Property minRange As Integer Implements IViewEvaluationsSummary.minRange
        Get
            Return ViewStateOrDefault("EvalMinRange", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("EvalMinRange") = value
        End Set
    End Property

    Public Property LockBool As Boolean Implements IViewEvaluationsSummary.LockBool
        Get
            Return ViewStateOrDefault("EvalLockBool", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("EvalLockBool") = value
        End Set
    End Property
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PGgridTop.Pager = Me.Pager
        Me.PGgridBottom.Pager = Me.Pager
    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()


        If Not Page.IsPostBack() Then
            CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.AdvCommissionSummary,
            Me.IdCallAdvCommission,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.AdvCommission,
            "")
        End If

        Master.ShowNoPermission = False
        Me.CurrentPresenter.InitView()
    End Sub

    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True


        CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.AdvCommissionSummary,
            Me.IdCallAdvCommission,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.AdvCommission,
            "NoPermission")

    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_EditCommittees", "Modules", "CallForPapers")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLiteral(LTpageTop)
            .setLiteral(LTpageBottom)
            .setHyperLink(HYPmanage, CallType.ToString(), False, True)
            .setHyperLink(HYPlist, CallType.ToString(), False, True)
            .setLabel(LBsearchEvaluationsFor_t)
            .setLabel(LBevaluationStatusFilter_t)
            .setButton(BTNfindEvaluations, True)
            .setLinkButton(LNBexportAllEvaluationsSummaryToXLS, True, True)
            .setLinkButton(LNBexportFilteredEvaluationsSummaryToXLS, True, True)
            .setLinkButton(LNBexportAllEvaluationsSummaryData, True, True)

            .setLinkButton(LNBexportAllEvaluationsSummaryToCsv, True, True)
            .setLinkButton(LNBexportFilteredEvaluationsSummaryToCsv, True, True)
            .setLinkButton(LNBexportAllEvaluationsSummaryDataToCsv, True, True)
            .setLinkButton(LNBexportFullEvaluationsSummaryDataToXml, True, True)
            .setLinkButton(LNBexportFullEvaluationsSummaryDataToCsv, True, True)
            .setHyperLink(HYPtoCommitteesSummary, False, True)
            .setLabel(LBsubmitterType_t)
        End With
    End Sub

    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
#Region "Display"
    Private Sub DisplayDssWarning(lastUpdate As Date?, isCompleted As Boolean) Implements IViewEvaluationsSummary.DisplayDssWarning
        CTRLdssMessage.Visible = True
        If lastUpdate = Nothing Then
            CTRLdssMessage.InitializeControl(Resource.getValue("DisplayDssWarning.NoDate"), Helpers.MessageType.alert)
        Else
            CTRLdssMessage.InitializeControl(String.Format(Resource.getValue("DisplayDssWarning.Date.isCompleted." & isCompleted.ToString), lastUpdate.Value.ToString()), IIf(isCompleted, Helpers.MessageType.success, Helpers.MessageType.alert))
        End If
    End Sub
    Private Sub HideDssWarning() Implements IViewEvaluationsSummary.HideDssWarning
        CTRLdssMessage.Visible = False
    End Sub
    Private Sub DisplayNoEvaluationsFound() Implements IViewEvaluationsSummary.DisplayNoEvaluationsFound
        DVexport.Visible = AllowExportAll
        Me.MLVevaluations.SetActiveView(VIWnoItems)
        Me.LBnoEvaluations.Text = Resource.getValue("DisplayNoEvaluationsFound")
    End Sub
    Private Sub SetContainerName(callName As String, endEvaluationOn As Date?) Implements IViewEvaluationsSummary.SetContainerName
        Master.ServiceTitle = String.Format(Resource.getValue("serviceTitle.EvaluationsSummary"), IIf(Len(callName) > 70, Left(callName, 70) & "...", callName))
        If endEvaluationOn.HasValue Then
            Master.ServiceTitleToolTip = String.Format(Resource.getValue("serviceTitle.EvaluationsSummary.ToolTip.Deadline"), endEvaluationOn.Value.ToShortDateString & " " & endEvaluationOn.Value.ToString("HH:mm"), callName)
        Else
            Master.ServiceTitleToolTip = String.Format(Resource.getValue("serviceTitle.EvaluationsSummary.ToolTip"), callName)
        End If
    End Sub
    Private Sub DisplayUnknownCall(idCommunity As Integer, idModule As Integer, idCall As Long) Implements IViewBaseSummary.DisplayUnknownCall
        DVexport.Visible = False
        Me.DVfilter.Visible = False
        Me.MLVevaluations.SetActiveView(VIWnoItems)
        Me.LBnoEvaluations.Text = Resource.getValue("DisplayUnknownCall." & CallType.ToString)

        SendUserAction(idCommunity, idModule, idCall, ModuleCallForPaper.ActionType.ViewEvaluationsSummary)
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewEvaluationsSummary.DisplaySessionTimeout
        DisplaySessionTimeout(RootObject.EvaluationsSummary(PreloadIdCall, PreloadIdCommunity, PreloadView, 0, PreloadIdSubmitterType, PreloadFilterBy, PreloadOrderBy, PreloadAscending, PreloadPageIndex, PreloadPageSize))
    End Sub
    Private Sub DisplaySessionTimeout(ByVal url As String) Implements IViewBaseSummary.DisplaySessionTimeout
        Dim idCommunity As Integer = PreloadIdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = url
        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If

        CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.AdvCommissionSummary,
            Me.IdCallAdvCommission,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.AdvCommission,
            "SessionTimeOut")

        webPost.Redirect(dto)
    End Sub

    Private Sub DisplayEvaluationUnavailable() Implements IViewBaseSummary.DisplayEvaluationUnavailable
        DVexport.Visible = False
        Me.DVfilter.Visible = False
        Me.MLVevaluations.SetActiveView(VIWnoItems)
        Me.LBnoEvaluations.Text = Resource.getValue("EvaluationSummary.DisplayEvaluationUnavailable")
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewEvaluationsSummary.DisplayNoPermission
        DVexport.Visible = False
        Me.PageUtility.AddActionToModule(idCommunity, idModule, ModuleCallForPaper.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub
    Private Sub DisplayEvaluationInfo(endEvaluationOn As Date?) Implements IViewEvaluationsSummary.DisplayEvaluationInfo
        Me.DVfilter.Visible = True
    End Sub

    Private Sub LoadAvailableStatus(items As List(Of EvaluationFilterStatus), selected As EvaluationFilterStatus) Implements IViewEvaluationsSummary.LoadAvailableStatus
        Me.RBLevaluationStatus.Items.Clear()
        Me.RBLevaluationStatus.Items.AddRange((From s As EvaluationFilterStatus In items Where s <> EvaluationFilterStatus.All Select New ListItem(Resource.getValue("EvaluationFilterStatus." & s.ToString), CInt(s))).OrderBy(Function(l) l.Text).ToArray)

        If Not IsNothing(items) AndAlso items.Contains(EvaluationFilterStatus.All) Then
            Me.RBLevaluationStatus.Items.Insert(0, New ListItem(Resource.getValue("EvaluationFilterStatus." & EvaluationFilterStatus.All.ToString), CInt(EvaluationFilterStatus.All)))
        End If

        If Me.RBLevaluationStatus.Items.Count = 0 Then
            Me.RBLevaluationStatus.Items.Add(New ListItem(Resource.getValue("EvaluationFilterStatus." & selected.ToString), CInt(selected)))
        ElseIf Not IsNothing(RBLevaluationStatus.Items.FindByValue(selected)) Then
            Me.RBLevaluationStatus.SelectedValue = CInt(selected)
        Else
            Me.RBLevaluationStatus.SelectedIndex = 0
        End If
        DVstatusfilter.Visible = (items.Count > 1)
    End Sub
    Private Sub LoadAvailableSubmitterTypes(items As List(Of dtoSubmitterType), selected As Long) Implements IViewEvaluationsSummary.LoadAvailableSubmitterTypes
        Me.DDLsubmitterTypes.Items.Clear()
        Me.DDLsubmitterTypes.DataSource = items
        Me.DDLsubmitterTypes.DataTextField = "Name"
        Me.DDLsubmitterTypes.DataValueField = "Id"
        Me.DDLsubmitterTypes.DataBind()
        If Not IsNothing(items) AndAlso items.Count > 0 Then
            Me.DDLsubmitterTypes.Items.Insert(0, New ListItem(Resource.getValue("SubmitterType.All"), -1))
        End If

        If Me.DDLsubmitterTypes.Items.Count = 0 Then
            Me.DDLsubmitterTypes.Items.Insert(0, New ListItem(Resource.getValue("SubmitterType.All"), -1))
        ElseIf Not IsNothing(DDLsubmitterTypes.Items.FindByValue(selected)) Then
            Me.DDLsubmitterTypes.SelectedValue = selected
        Else
            Me.DDLsubmitterTypes.SelectedIndex = 0
        End If
        DVsubmitterType.Visible = (items.Count > 1)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As ModuleCallForPaper.ActionType) Implements IViewEvaluationsSummary.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, Nothing, InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As ModuleRequestForMembership.ActionType) Implements IViewEvaluationsSummary.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, Nothing, InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType) Implements IViewEvaluationsSummary.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ObjectType.CallForPaper, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.ActionType) Implements IViewEvaluationsSummary.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.ObjectType.RequestForMembership, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
#End Region

    Private Sub DisplayLinkToSingleCommittee(idCommittee As Long) Implements IViewEvaluationsSummary.DisplayLinkToSingleCommittee
        Dim searchBy As String = Me.TXBusername.Text
        If Not String.IsNullOrEmpty(searchBy) AndAlso Not String.IsNullOrEmpty(searchBy.Trim) Then
            searchBy = Server.UrlEncode(searchBy)
        End If
        Me.HYPtoCommitteesSummary.Visible = True
        Me.HYPtoCommitteesSummary.NavigateUrl = BaseUrl & RootObject.CommitteeSummary(idCommittee, IdCall, IdCallCommunity, PreloadView, CurrentIdSubmitterType, CurrentFilterBy, CurrentOrderBy, CurrentAscending, Me.Pager.PageIndex, Me.Pager.PageSize, searchBy)
    End Sub
    Private Sub LoadEvaluations(items As List(Of dtoEvaluationSummaryItem), committeesCount As Integer) Implements IViewEvaluationsSummary.LoadEvaluations
        If IsNothing(items) OrElse items.Count = 0 Then
            Me.DisplayNoEvaluationsFound()
            DVexport.Visible = AllowExportAll
        Else





            DVexport.Visible = AllowExportAll OrElse AllowExportCurrent

            Me.MLVevaluations.SetActiveView(VIWlist)

            'If CurrentEvaluationType = EvaluationType.Dss Then
            '    _AllEvaluated = Not items.Any(Function(i) Not IsNothing(i.DssEvaluation) AndAlso Not i.DssEvaluation.IsCompleted)
            'End If

            Me.RPTevaluations.DataSource = items
            Me.RPTevaluations.DataBind()

            Dim searchBy As String = Me.TXBusername.Text
            If Not String.IsNullOrEmpty(searchBy) AndAlso Not String.IsNullOrEmpty(searchBy.Trim) Then
                searchBy = Server.UrlEncode(searchBy)
            End If
            If committeesCount > 1 Then
                Me.HYPtoCommitteesSummary.Visible = True
                Me.HYPtoCommitteesSummary.NavigateUrl = BaseUrl & RootObject.CommitteesSummary(IdCall, IdCallCommunity, PreloadView, CurrentIdSubmitterType, CurrentFilterBy, CurrentOrderBy, CurrentAscending, Me.Pager.PageIndex, Me.Pager.PageSize, searchBy)
            End If
        End If
    End Sub

    Private Sub SetActionUrl(action As CallStandardAction, url As String) Implements IViewEvaluationsSummary.SetActionUrl
        Select Case action
            Case CallStandardAction.List
                Me.HYPlist.Visible = True
                Me.HYPlist.NavigateUrl = BaseUrl & url
            Case CallStandardAction.Manage
                Me.HYPmanage.Visible = True
                Me.HYPmanage.NavigateUrl = BaseUrl & url
        End Select
    End Sub
    Private Function GetItemEncoded(name As String) As String Implements IViewBaseSummary.GetItemEncoded
        Return EncodeSearchBy(name)
    End Function
    'Private Sub LoadRevisions(items As List(Of dtoSubmissionDisplayItemPermission)) Implements IViewEvaluationsSummary.LoadSubmissions
    '    If IsNothing(items) OrElse items.Count = 0 Then
    '        Me.MLVsubmissions.SetActiveView(VIWnoItems)
    '    Else
    '        Me.MLVsubmissions.SetActiveView(VIWlist)
    '        Me.RPTsubmissions.DataSource = items
    '        Me.RPTsubmissions.DataBind()
    '    End If
    'End Sub


    'End Sub
    'Private Sub GotoUrl(url As String) Implements IViewEvaluationsSummary.GotoUrl
    '    PageUtility.RedirectToUrl(url)
    'End Sub

#Region "Helpers"
    Private Sub InitStackedBar(item As dtoEvaluationSummaryItem, control As UC_StackedBar)
        Dim count As Long = item.Evaluations.Count
        Dim nEvaluated As Long = item.GetEvaluationsCount(EvaluationStatus.Evaluated)
        Dim nEvaluating As Long = item.GetEvaluationsCount(EvaluationStatus.Evaluating)
        Dim nNotstarted As Long = item.GetEvaluationsCount(EvaluationStatus.None)
        Dim nReplace As Long = item.GetEvaluationsCount(EvaluationStatus.EvaluatorReplacement)
        Dim nInvalid As Long = item.GetEvaluationsCount(EvaluationStatus.Invalidated)

        Dim nConfirmed As Long = item.GetEvaluationsCount(EvaluationStatus.Confirmed)


        Dim evaluated As StackedBarItem = New StackedBarItem()
        evaluated.CssClass = "completed"
        evaluated.Title = Me.Resource.getValue("ProgressTooltip.EvaluationStatus." & EvaluationStatus.Evaluated.ToString())
        evaluated.Value = Percentual(nEvaluated, count)

        Dim evaluating As StackedBarItem = New StackedBarItem()
        evaluating.CssClass = "started"
        evaluating.Title = Me.Resource.getValue("ProgressTooltip.EvaluationStatus." & EvaluationStatus.Evaluating.ToString())
        evaluating.Value = Percentual(nEvaluating, count)

        Dim notstarted As StackedBarItem = New StackedBarItem()
        notstarted.CssClass = "notstarted"
        notstarted.Title = Me.Resource.getValue("ProgressTooltip.EvaluationStatus." & EvaluationStatus.None.ToString())
        notstarted.Value = Percentual(nNotstarted, count)

        Dim confirmed As StackedBarItem = New StackedBarItem()
        confirmed.CssClass = "confirmed"
        confirmed.Title = "Confermati: {0}%"  'ToDo: interanzionalizzare
        confirmed.Value = Percentual(nConfirmed, count)

        If nReplace = 0 AndAlso nInvalid = 0 Then
            If notstarted.Value > 0 Then
                notstarted.Value = 100 - evaluated.Value - evaluating.Value - confirmed.Value
            ElseIf evaluating.Value > 0 Then
                evaluating.Value = 100 - evaluated.Value - confirmed.Value
            ElseIf evaluated.Value > 0 Then
                evaluated.Value = 100 - confirmed.Value
            End If
            control.InitializeControl({evaluated, evaluating, notstarted, confirmed})
        Else
            Dim otherValues As StackedBarItem = New StackedBarItem()
            otherValues.CssClass = IIf(nInvalid > 0, EvaluationStatus.Invalidated.ToString.ToLower(), "") & IIf(nReplace > 0, " " & EvaluationStatus.EvaluatorReplacement.ToString.ToLower(), "")
            If nInvalid > 0 AndAlso nReplace > 0 Then
                otherValues.Title = Me.Resource.getValue("ProgressTooltip.EvaluationStatus.Other")
            ElseIf nInvalid > 0 Then
                otherValues.Title = Me.Resource.getValue("ProgressTooltip.EvaluationStatus." & EvaluationStatus.Invalidated.ToString())
            ElseIf nReplace > 0 Then
                otherValues.Title = Me.Resource.getValue("ProgressTooltip.EvaluationStatus." & EvaluationStatus.EvaluatorReplacement.ToString())
            End If

            otherValues.Value = Percentual(nInvalid + nReplace, count)

            If otherValues.Value > 0 Then
                otherValues.Value = 100 - evaluated.Value - evaluating.Value - notstarted.Value - confirmed.Value
            ElseIf notstarted.Value > 0 Then
                notstarted.Value = 100 - evaluated.Value - evaluating.Value - confirmed.Value
            ElseIf evaluating.Value > 0 Then
                evaluating.Value = 100 - evaluated.Value - confirmed.Value
            ElseIf evaluated.Value > 0 Then
                evaluated.Value = 100 - confirmed.Value
            End If


            control.InitializeControl({evaluated, evaluating, otherValues, notstarted, confirmed})
        End If

    End Sub


    Private Function Percentual(value As Long, tot As Long) As Int32
        If tot > 0 Then
            Dim d As Double = value / tot * 100
            Return Math.Floor(d)
        Else
            Return 0
        End If
    End Function
#End Region

#End Region

#Region "Export"
    Protected Sub CTRLreport_GetConfigTemplate(ByRef template As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template)
        template = Me.Master.getConfigTemplate()
    End Sub
    Protected Sub CTRLreport_GetContainerTemplate(ByRef template As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template)
        template.Header = Me.Master.getTemplateHeader(" ")
        template.Footer = Me.Master.getTemplateFooter()
    End Sub
    Protected Sub CTRLreport_GetHiddenIdentifierValueEvent(ByRef value As String)
        value = HDNdownloadTokenValue.Value
    End Sub
#End Region

#Region "Grid"
    Private Sub RPTevaluations_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTevaluations.ItemDataBound

        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then




            Dim dto As dtoEvaluationSummaryItem = DirectCast(e.Item.DataItem, dtoEvaluationSummaryItem)
            Dim idUser As Integer = dto.IdSubmissionOwner

            Dim oControl As UC_StackedBar = e.Item.FindControl("CTRLevaluationStackedBar")
            Me.InitStackedBar(dto, oControl)



            Dim LTcommissayCount As Literal = e.Item.FindControl("LTcommissayCount")
            If Not IsNothing(LTcommissayCount) Then
                Dim nNotCompleted As Long = dto.GetEvaluationsCount(EvaluationStatus.Evaluating) _
                + dto.GetEvaluationsCount(EvaluationStatus.None)

                Dim nEvaluated As Long = dto.GetEvaluationsCount(EvaluationStatus.Evaluated) _
                    + dto.GetEvaluationsCount(EvaluationStatus.Confirmed)


                LTcommissayCount.Text = String.Format(LTcommissayCount.Text, nEvaluated, (nEvaluated + nNotCompleted))
            End If



            'Dim oLabel As Label = e.Item.FindControl("LBvote")

            Dim Uc_ScoreItem As Uc_advScoreItem = e.Item.FindControl("Uc_ScoreItem")

            If Not IsNothing(Uc_ScoreItem) Then

                Dim Score As String = ""

                Select Case CurrentEvaluationType
                    Case EvaluationType.Average
                        Score = dto.AverageRatingToString()

                    Case EvaluationType.Sum
                        Score = dto.SumRatingToString()
                End Select

                Uc_ScoreItem.InitUC(Score, Me.minRange, dto.BoolPassedCount, dto.Evaluations.Count(), dto.AllPassed, dto.ShowScore)

            End If

            Dim searchBy As String = Me.TXBusername.Text
                If Not String.IsNullOrEmpty(searchBy) AndAlso Not String.IsNullOrEmpty(searchBy.Trim) Then
                    searchBy = Server.UrlEncode(searchBy)
                End If

                Dim oHyperlink As HyperLink = e.Item.FindControl("HYPviewSubmissionEvaluation")
                oHyperlink.Visible = False
            'Resource.setHyperLink(oHyperlink, False, True)
            'oHyperlink.NavigateUrl = BaseUrl & RootObject.ViewSubmissionEvaluations(dto.IdSubmission, IdCall, IdCallCommunity, advCommissionId?)

            oHyperlink = e.Item.FindControl("HYPviewTableSubmissionEvaluation")
                Resource.setHyperLink(oHyperlink, False, True)
                oHyperlink.NavigateUrl = BaseUrl & RootObject.AdvViewSubmissionTableEvaluations(dto.IdSubmission, IdCall, IdCallCommunity, Me.IdCallAdvCommission)

                Dim oControlReport As UC_SubmissionExport = e.Item.FindControl("CTRLreport")
                Dim loadTypes As New List(Of lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType)
                loadTypes.Add(Helpers.Export.ExportFileType.pdf)
                loadTypes.Add(Helpers.Export.ExportFileType.zip)

                oControlReport.WebOnlyRender = True
                oControlReport.InitializeControl(False, idUser, dto.IdSubmission, IdCall, dto.IdSubmission, dto.IdRevision, IdCallModule, IdCallCommunity, CallType, loadTypes)
                oControlReport.Visible = True

            ElseIf e.Item.ItemType = ListItemType.Header Then
                Dim oLiteral As Literal = e.Item.FindControl("LTsubmitterName_t")
            Resource.setLiteral(oLiteral)

            oLiteral = e.Item.FindControl("LTsubmitterType_t")
            Resource.setLiteral(oLiteral)

            oLiteral = e.Item.FindControl("LTsubmissionPoints_t")
            Resource.setLiteral(oLiteral)

            oLiteral = e.Item.FindControl("LTevaluationStatus_t")
            Resource.setLiteral(oLiteral)

            oLiteral = e.Item.FindControl("LTsubActions_t")
            Resource.setLiteral(oLiteral)


            Dim searchBy As String = ""
            If Not String.IsNullOrEmpty(Me.TXBusername.Text) Then
                searchBy = Me.TXBusername.Text.Trim
            End If
            If Not String.IsNullOrEmpty(searchBy) Then
                searchBy = Server.UrlEncode(searchBy)
            End If

            Dim oHyperlink As HyperLink
            oHyperlink = e.Item.FindControl("HYPorderByUserUp")
            Resource.setHyperLink(oHyperlink, False, True)
            oHyperlink.NavigateUrl = BaseUrl & RootObject.AdvEvalSummary(IdCallCommunity, IdCall, Me.IdCallAdvCommission, PreloadView, 0, CurrentIdSubmitterType, CurrentFilterBy, SubmissionsOrder.ByUser, True, 0, PageSize, searchBy)
            oHyperlink = e.Item.FindControl("HYPorderByUserDown")
            Resource.setHyperLink(oHyperlink, False, True)
            oHyperlink.NavigateUrl = BaseUrl & RootObject.AdvEvalSummary(IdCallCommunity, IdCall, Me.IdCallAdvCommission, PreloadView, 0, CurrentIdSubmitterType, CurrentFilterBy, SubmissionsOrder.ByUser, False, 0, PageSize, searchBy)

            oHyperlink = e.Item.FindControl("HYPorderByTypeUp")
            Resource.setHyperLink(oHyperlink, False, True)
            oHyperlink.NavigateUrl = BaseUrl & RootObject.AdvEvalSummary(IdCallCommunity, IdCall, Me.IdCallAdvCommission, PreloadView, 0, CurrentIdSubmitterType, CurrentFilterBy, SubmissionsOrder.ByType, True, 0, PageSize, searchBy)
            oHyperlink = e.Item.FindControl("HYPorderByTypeDown")
            Resource.setHyperLink(oHyperlink, False, True)
            oHyperlink.NavigateUrl = BaseUrl & RootObject.AdvEvalSummary(IdCallCommunity, IdCall, Me.IdCallAdvCommission, PreloadView, 0, CurrentIdSubmitterType, CurrentFilterBy, SubmissionsOrder.ByType, False, 0, PageSize, searchBy)

            oHyperlink = e.Item.FindControl("HYPorderByEvaluationPointsUp")
            Resource.setHyperLink(oHyperlink, False, True)
            oHyperlink.NavigateUrl = BaseUrl & RootObject.AdvEvalSummary(IdCallCommunity, IdCall, Me.IdCallAdvCommission, PreloadView, 0, CurrentIdSubmitterType, CurrentFilterBy, SubmissionsOrder.ByEvaluationPoints, True, 0, PageSize, searchBy)
            oHyperlink = e.Item.FindControl("HYPorderByEvaluationPointsDown")
            Resource.setHyperLink(oHyperlink, False, True)
            oHyperlink.NavigateUrl = BaseUrl & RootObject.AdvEvalSummary(IdCallCommunity, IdCall, Me.IdCallAdvCommission, PreloadView, 0, CurrentIdSubmitterType, CurrentFilterBy, SubmissionsOrder.ByEvaluationPoints, False, 0, PageSize, searchBy)


            oHyperlink = e.Item.FindControl("HYPorderByEvaluationIndexUp")
            Resource.setHyperLink(oHyperlink, False, True)
            oHyperlink.NavigateUrl = BaseUrl & RootObject.AdvEvalSummary(IdCallCommunity, IdCall, Me.IdCallAdvCommission, PreloadView, 0, CurrentIdSubmitterType, CurrentFilterBy, SubmissionsOrder.ByEvaluationIndex, True, 0, PageSize, searchBy)
            oHyperlink = e.Item.FindControl("HYPorderByEvaluationIndexDown")
            Resource.setHyperLink(oHyperlink, False, True)
            oHyperlink.NavigateUrl = BaseUrl & RootObject.AdvEvalSummary(IdCallCommunity, IdCall, Me.IdCallAdvCommission, PreloadView, 0, CurrentIdSubmitterType, CurrentFilterBy, SubmissionsOrder.ByEvaluationIndex, False, 0, PageSize, searchBy)

            oHyperlink = e.Item.FindControl("HYPorderByEvaluationStatusUp")
            Resource.setHyperLink(oHyperlink, False, True)
            oHyperlink.NavigateUrl = BaseUrl & RootObject.AdvEvalSummary(IdCallCommunity, IdCall, Me.IdCallAdvCommission, PreloadView, 0, CurrentIdSubmitterType, CurrentFilterBy, SubmissionsOrder.ByEvaluationStatus, True, 0, PageSize, searchBy)
            oHyperlink = e.Item.FindControl("HYPorderByEvaluationStatusUp")
            Resource.setHyperLink(oHyperlink, False, True)
            oHyperlink.NavigateUrl = BaseUrl & RootObject.AdvEvalSummary(IdCallCommunity, IdCall, Me.IdCallAdvCommission, PreloadView, 0, CurrentIdSubmitterType, CurrentFilterBy, SubmissionsOrder.ByEvaluationStatus, False, 0, PageSize, searchBy)
        End If
    End Sub
    Private Sub SetDssValue(isCompleted As Boolean, rating As DssCallEvaluation, displayAsFuzzy As Boolean, oLabel As Label, oGenericControl As HtmlGenericControl, oControl As UC_FuzzyNumber)
        Dim name As String = "", shortName As String = ""
        If Not IsNothing(rating) AndAlso rating.IsValid AndAlso isCompleted Then
            If Not rating.IsFuzzy AndAlso Not displayAsFuzzy Then
                oLabel.Text = GetEvaluatedValue(rating.Value, 4)
                oGenericControl.Visible = False
            Else
                oGenericControl.Visible = True
                oLabel.Visible = False
                oControl.InitializeControl(True, rating.Ranking, rating.Value, rating.ValueFuzzy, GetEvaluatedValue(rating.Ranking, 4), "")
            End If
        Else
            oLabel.Text = "--"
            oLabel.ToolTip = Resource.getValue("DssWaitingEvaluations")
        End If
    End Sub
    Private Function GetEvaluatedValue(sum As Double, Optional decimals As Integer = 2) As String
        Dim fractional As Double = sum - Math.Floor(sum)
        If (fractional = 0) Then
            Return String.Format("{0:N0}", sum)
        Else
            Return String.Format("{0:N" & decimals.ToString() & "}", sum)
        End If
    End Function
    Private Sub PGgridBottom_OnPageSelected() Handles PGgridBottom.OnPageSelected
        Me.CurrentPresenter.LoadEvaluations(IdCall, CurrentEvaluationType, IdCallCommunity, Me.CurrentFilters, Me.Pager.PageIndex, Me.Pager.PageSize)
    End Sub
    Private Sub PGgridTop_OnPageSelected() Handles PGgridTop.OnPageSelected
        Me.CurrentPresenter.LoadEvaluations(IdCall, CurrentEvaluationType, IdCallCommunity, Me.CurrentFilters, Me.Pager.PageIndex, Me.Pager.PageSize)
    End Sub
    Private Sub BTNfindEvaluations_Click(sender As Object, e As System.EventArgs) Handles BTNfindEvaluations.Click
        LoadEvaluations()
    End Sub
    Private Sub RBLevaluationStatus_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RBLevaluationStatus.SelectedIndexChanged
        LoadEvaluations()
    End Sub
    Private Sub DDLsubmitterTypes_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLsubmitterTypes.SelectedIndexChanged
        LoadEvaluations()
    End Sub
    Private Sub LoadEvaluations()
        Dim filters As dtoEvaluationsFilters = Me.CurrentFilters
        filters.SearchForName = Me.TXBusername.Text
        filters.Ascending = True

        filters.Status = Me.CurrentFilterBy
        'Select Case filters.Status
        '    Case EvaluationFilterStatus.All, EvaluationFilterStatus.AllValid, EvaluationFilterStatus.Evaluated, EvaluationFilterStatus.Evaluating
        '        filters.OrderBy = SubmissionsOrder.ByEvaluationIndex
        '    Case Else
        '        filters.OrderBy = SubmissionsOrder.ByUser
        '        filters.Ascending = True
        'End Select
        CurrentPresenter.LoadEvaluations(IdCall, CurrentEvaluationType, IdCallCommunity, filters, 0, PreloadPageSize)
    End Sub
#End Region

    Private Sub ListCalls_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Me.Master.ShowDocType = True
    End Sub

#Region "Export Data"

#Region "Button"
    Private Sub LNBexportAllEvaluationsSummaryData_Click(sender As Object, e As System.EventArgs) Handles LNBexportAllEvaluationsSummaryData.Click
        ExportEvaluations(ItemsToExport.All, ExportData.Fulldata, Helpers.Export.ExportFileType.xml)
    End Sub
    Private Sub LNBexportAllEvaluationsSummaryToXLS_Click(sender As Object, e As System.EventArgs) Handles LNBexportAllEvaluationsSummaryToXLS.Click
        ExportEvaluations(ItemsToExport.All, ExportData.DisplayData, Helpers.Export.ExportFileType.xml)
    End Sub
    Private Sub LNBexportFilteredEvaluationsSummaryToXLS_Click(sender As Object, e As System.EventArgs) Handles LNBexportFilteredEvaluationsSummaryToXLS.Click
        ExportEvaluations(ItemsToExport.Filtered, ExportData.DisplayData, Helpers.Export.ExportFileType.xml)
    End Sub
    Private Sub LNBexportFullEvaluationsSummaryDataToXml_Click(sender As Object, e As System.EventArgs) Handles LNBexportFullEvaluationsSummaryDataToXml.Click
        ExportEvaluations(ItemsToExport.All, ExportData.FulldataToAnalyze, Helpers.Export.ExportFileType.xml)
    End Sub
    Private Sub LNBexportAllEvaluationsSummaryDataToCsv_Click(sender As Object, e As System.EventArgs) Handles LNBexportAllEvaluationsSummaryDataToCsv.Click
        ExportEvaluations(ItemsToExport.All, ExportData.Fulldata, Helpers.Export.ExportFileType.csv)
    End Sub
    Private Sub LNBexportAllEvaluationsSummaryToCsv_Click(sender As Object, e As System.EventArgs) Handles LNBexportAllEvaluationsSummaryToCsv.Click
        ExportEvaluations(ItemsToExport.All, ExportData.DisplayData, Helpers.Export.ExportFileType.csv)
    End Sub
    Private Sub LNBexportFilteredEvaluationsSummaryToCsv_Click(sender As Object, e As System.EventArgs) Handles LNBexportFilteredEvaluationsSummaryToCsv.Click
        ExportEvaluations(ItemsToExport.Filtered, ExportData.DisplayData, Helpers.Export.ExportFileType.csv)
    End Sub
    Private Sub LNBexportFullEvaluationsSummaryDataToCsv_Click(sender As Object, e As System.EventArgs) Handles LNBexportFullEvaluationsSummaryDataToCsv.Click
        ExportEvaluations(ItemsToExport.All, ExportData.FulldataToAnalyze, Helpers.Export.ExportFileType.csv)
    End Sub
#End Region

    Private Sub ExportEvaluations(items As ItemsToExport, data As ExportData, fileType As lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType)
        Response.Clear()
        Response.ClearHeaders()
        Response.AppendCookie(New HttpCookie(Me.CTRLreport.CookieName, HDNdownloadTokenValue.Value)) '; //downloadTokenValue will have been provided in the form submit via the hidden input field
        Dim fileName As String = Resource.getValue(SummaryType.Evaluations.ToString() & "." & items.ToString & "." & data.ToString)
        If String.IsNullOrEmpty(fileName) Then
            fileName = GetFileName(fileType)
        Else
            fileName = CurrentPresenter.GetFileName(fileName, items, data)
            fileName &= "." & fileType.ToString()
        End If
        Response.AddHeader("Content-Disposition", "attachment; filename=" & fileName)
        '  Response.ContentType = "application/xml"


        Try
            Dim translations As New Dictionary(Of lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationTranslations, String)
            For Each name As String In [Enum].GetNames(GetType(lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationTranslations))
                translations.Add([Enum].Parse(GetType(lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationTranslations), name), Me.Resource.getValue("EvaluationTranslations." & name))
            Next
            Dim tStatus As New Dictionary(Of lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationStatus, String)
            For Each name As String In [Enum].GetNames(GetType(lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationStatus))
                tStatus.Add([Enum].Parse(GetType(lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationStatus), name), Me.Resource.getValue("EvaluationTranslations.EvaluationStatus." & name))
            Next

            Select Case fileType
                Case Helpers.Export.ExportFileType.xls
                    Response.ContentType = "application/ms-excel"
                Case Helpers.Export.ExportFileType.xml
                    Response.ContentType = "application/ms-excel"
                Case Helpers.Export.ExportFileType.csv
                    Response.Charset = "utf-16"
                    Response.ContentType = "text/csv"
                Case Else
                    Response.Charset = "utf-16"
                    Response.ContentType = "text/csv"
            End Select
            Dim exportData As String = CurrentPresenter.ExportTo(CurrentFilters, SummaryType.Evaluations, items, data, fileType, translations, tStatus)
            Response.BinaryWrite(Response.ContentEncoding.GetPreamble())
            'If fileType = Helpers.Export.ExportFileType.csv Then

            '    Using sw As IO.StreamWriter = New IO.StreamWriter(Context.Response.OutputStream, System.Text.Encoding.Unicode)
            '        sw.Write(exportData)
            '        sw.Close()
            '    End Using
            'Else
            Response.Write(exportData)
            '   End If

        Catch de As Exception

        End Try
        Response.End()
    End Sub


    Private Sub ExportEvaluationsADVFull(
                                    items As ItemsToExport,
                                    data As ExportData,
                                    fileType As lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType)
        Response.Clear()
        Response.ClearHeaders()
        Response.AppendCookie(New HttpCookie(Me.CTRLreport.CookieName, HDNdownloadTokenValue.Value)) '; //downloadTokenValue will have been provided in the form submit via the hidden input field
        Dim fileName As String = Resource.getValue(SummaryType.Evaluations.ToString() & "." & items.ToString & "." & data.ToString)
        If String.IsNullOrEmpty(fileName) Then
            fileName = GetFileName(fileType)
        Else
            fileName = CurrentPresenter.GetFileName(fileName, items, data)
            fileName &= "." & fileType.ToString()
        End If
        Response.AddHeader("Content-Disposition", "attachment; filename=" & fileName)
        '  Response.ContentType = "application/xml"


        Try
            Dim translations As New Dictionary(Of lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationTranslations, String)
            For Each name As String In [Enum].GetNames(GetType(lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationTranslations))
                translations.Add([Enum].Parse(GetType(lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationTranslations), name), Me.Resource.getValue("EvaluationTranslations." & name))
            Next
            Dim tStatus As New Dictionary(Of lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationStatus, String)
            For Each name As String In [Enum].GetNames(GetType(lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationStatus))
                tStatus.Add([Enum].Parse(GetType(lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationStatus), name), Me.Resource.getValue("EvaluationTranslations.EvaluationStatus." & name))
            Next

            Select Case fileType
                Case Helpers.Export.ExportFileType.xls
                    Response.ContentType = "application/ms-excel"
                Case Helpers.Export.ExportFileType.xml
                    Response.ContentType = "application/ms-excel"
                Case Helpers.Export.ExportFileType.csv
                    Response.Charset = "utf-16"
                    Response.ContentType = "text/csv"
                Case Else
                    Response.Charset = "utf-16"
                    Response.ContentType = "text/csv"
            End Select

            Dim exportData As String =
                CurrentPresenter.ExportTo(CurrentFilters, SummaryType.Evaluations, items, data, fileType, translations, tStatus)

            Response.BinaryWrite(Response.ContentEncoding.GetPreamble())
            'If fileType = Helpers.Export.ExportFileType.csv Then

            '    Using sw As IO.StreamWriter = New IO.StreamWriter(Context.Response.OutputStream, System.Text.Encoding.Unicode)
            '        sw.Write(exportData)
            '        sw.Close()
            '    End Using
            'Else
            Response.Write(exportData)
            '   End If

        Catch de As Exception

        End Try
        Response.End()
    End Sub

    Private Function GetFileName(ByVal type As lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType) As String
        Dim filename As String = Resource.getValue("ExportGenericEvaluations")
        Dim oDate As DateTime = DateTime.Now
        If String.IsNullOrEmpty(filename) Then
            filename = "{0}_{1}_{2}_CallEvaluations"
        End If
        Return String.Format(filename, oDate.Year, oDate.Month, oDate.Day) & "." & type.ToString
    End Function
#End Region

    Private Function EncodeSearchBy(search As String) As String
        If String.IsNullOrEmpty(search) OrElse String.IsNullOrEmpty(search.Trim) Then
            Return ""
        Else
            Return Server.UrlEncode(search)
        End If
    End Function

    Private Sub LNBcommclose_Click(sender As Object, e As EventArgs) Handles LNBcommclose.Click

        CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.AdvCommissionClose,
            Me.IdCallAdvCommission,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.AdvCommission,
            "")

        Me.CurrentPresenter.CommissionClose()
    End Sub

    Public Sub ShowCloseCommission(show As Boolean) Implements IViewEvaluationsSummary.ShowCloseCommission
        Me.LNBcommclose.Enabled = show
        Me.LNBcommclose.Visible = show
    End Sub

    Public Sub SetStepSummaryLink(StepId As Int64, CommId As Int64, visible As Boolean) Implements IViewEvaluationsSummary.SetStepSummaryLink
        HYPcommissionSummary.Visible = visible
        If visible Then
            HYPcommissionSummary.NavigateUrl = BaseUrl & RootObject.AdvStepSummary(StepId, Me.IdCallAdvCommission)
        Else
            HYPcommissionSummary.NavigateUrl = ""
        End If

    End Sub

    Private Sub LKBupdate_Click(sender As Object, e As EventArgs) Handles LKBupdate.Click
        BindDati()
    End Sub
End Class