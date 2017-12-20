Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.EduPath.BusinessLogic
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.Modules.EduPath.Domain
Imports lm.Comol.Modules.EduPath.Domain.DTO
Imports lm.ActionDataContract
Imports lm.Comol.Core.DomainModel

Public Class EduPathPlay
    Inherits PageBaseEduPath

#Region "InitStandard"

    Private _Service As Services_EduPath
    Private _TempUnitId As Long
    Private _PlayerUnitId As Long
    Private _VisibleActivity As IList(Of Int64)
    Private _VisibleUnit As IList(Of Int64)
    Private _canViewSubAct_byUnit As Boolean
    Private _canViewSubAct_byAct As Boolean

    ''' <summary>
    ''' Visibile in base alle regole
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property VisibleActivity As IList(Of Int64)
        Get
            Return _VisibleActivity
        End Get
        Set(ByVal value As IList(Of Int64))
            _VisibleActivity = value
        End Set
    End Property

    ''' <summary>
    ''' 'Visibile in base alle regole
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property VisibleUnit As IList(Of Int64)
        Get
            Return _VisibleUnit
        End Get
        Set(ByVal value As IList(Of Int64))
            _VisibleUnit = value
        End Set
    End Property

    Protected Overrides ReadOnly Property PathType As EPType
        Get
            If _PathType = lm.Comol.Modules.EduPath.Domain.EPType.None Then
                _PathType = ServiceEP.GetEpType(CurrentPathId, ItemType.Path)
            End If
            Return _PathType
        End Get
    End Property

    Private Property PlayerUnitId As Long
        Get
            Return _PlayerUnitId
        End Get
        Set(ByVal value As Long)
            _PlayerUnitId = value
        End Set
    End Property

#End Region


    Dim CouldActivityWithSingleSubActivityBeOmitted As Boolean = False


#Region " Base"


    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_EduPath.Codex)
        CouldActivityWithSingleSubActivityBeOmitted = True 'EduPathConfiguration.UseSingleActionView
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides Sub BindDati()
        If Not ServiceEP.isPlayModePath(CurrentPathId) Then
            RedirectToUrl(RootObject.PathView(CurrentPathId, CurrentCommunityID, EpViewModeType.View, True))
        End If
        Me.PageUtility.AddAction(Me.CurrentCommunityID, Services_EduPath.ActionType.Access, Me.PageUtility.CreateObjectsList(Services_EduPath.ObjectType.EduPath, Me.CurrentPathId), InteractionType.UserWithLearningObject)

        initPageView()

    End Sub

    Public Overrides Sub BindNoPermessi()
        Me.ShowError(EpError.NotPermission)
    End Sub

    Public Overrides Function HasPermessi() As Boolean

        Return ServiceEP.CheckCommunityId(Of Path)(Me.CurrentPathId, Me.CurrentCommunityID) AndAlso ServiceEP.ItemIsVisible(CurrentPathId, ItemType.Path) 'non si possono controllare qui gli altri permessi visto che un utente potrebbe avere i permessi attivi 
        'esclusivamente in un sub item e non sull'intero path
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_EpView", "EduPath")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setHyperLink(Me.HYPstat, False, True)
            .setHyperLink(Me.HYPlistEduPath, False, True)

        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            If IsSessioneScaduta(False) Then
                RedirectOnSessionTimeOut(Request.Url.AbsoluteUri, CurrentCommunityID)
            End If
            Return False
        End Get
    End Property

#End Region

#Region "Property"
    Private _PermissionOverPath As PermissionEP = New PermissionEP(RoleEP.None)
    Private _PermissionOverUnit As PermissionEP = New PermissionEP(RoleEP.None)
    Private _UnitsCount As Int16
    Private _ActivitiesCount As Int16
    Private _unitId As Int16
    Private _isVisibleByDateConstraint As Boolean

    Private _SmartTagsAvailable As SmartTags

    Public Property ShowSwitchMessage As Boolean
        Get
            Return Me.ViewState("ShowSwitchMessage")
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("ShowSwitchMessage") = value
        End Set
    End Property

    Private _isSetCanProceedEndDate As Boolean = False
    Private _CanProceedByEndDate As Boolean

    Private _endDateSet As Boolean = False
    Private _EpEndDate As DateTime?
    Private ReadOnly Property EpEndDate As DateTime?
        Get
            If Not _endDateSet Then
                _EpEndDate = ServiceEP.GetEpEndDate(CurrentPathId)
            End If
            Return _EpEndDate
        End Get
    End Property


    Public ReadOnly Property CanProceedByEndDate As Boolean
        Get
            If Not _isSetCanProceedEndDate Then

                If EpEndDate Is Nothing Then
                    _CanProceedByEndDate = True

                ElseIf DateTime.Now <= EpEndDate Then

                    _CanProceedByEndDate = True

                ElseIf ServiceEP.CheckEpType(PathType, EPType.AlwaysStat) Then

                    _CanProceedByEndDate = True
                Else
                    _CanProceedByEndDate = False
                End If

                _isSetCanProceedEndDate = True

            End If

            Return _CanProceedByEndDate
        End Get
    End Property


    Public ReadOnly Property SmartTagsAvailable() As Comol.Entity.SmartTags
        Get
            If _SmartTagsAvailable Is Nothing Then
                _SmartTagsAvailable = ManagerConfiguration.GetSmartTags(Me.ApplicationUrlBase(True))
            End If
            Return _SmartTagsAvailable
        End Get
    End Property
    'Public Property ActivitiesCount As Int16
    '    Get
    '        Return _ActivitiesCount
    '    End Get
    '    Set(ByVal value As Int16)
    '        _ActivitiesCount = value
    '    End Set
    'End Property
    'Public Property UnitsCount As Int16
    '    Get
    '        Return _UnitsCount
    '    End Get
    '    Set(ByVal value As Int16)
    '        _UnitsCount = value
    '    End Set
    'End Property

    Public Property PermissionOverUnit As PermissionEP
        Get
            Return _PermissionOverUnit
        End Get
        Set(ByVal value As PermissionEP)
            _PermissionOverUnit = value
        End Set
    End Property

    Public Property PermissionOverPath As PermissionEP
        Get
            Return _PermissionOverPath
        End Get
        Set(ByVal value As PermissionEP)
            _PermissionOverPath = value
        End Set
    End Property

    'Public Property PlayerCssVisibility As String
    '    Get
    '        Return ViewStateOrDefault("PlayerCssVisibility", "")
    '    End Get
    '    Set(ByVal value As String)
    '        Me.ViewState("PlayerCssVisibility") = value
    '    End Set
    'End Property

    Public Function ViewStateOrDefault(Of T)(ByVal Key As String, ByVal DefaultValue As T) As T
        If (ViewState(Key) Is Nothing) Then
            ViewState(Key) = DefaultValue
            Return DefaultValue
        Else
            Return ViewState(Key)
        End If
    End Function

    Public Property TempUnitId() As Long
        Set(ByVal value As Long)
            _TempUnitId = value
        End Set
        Get
            Return _TempUnitId
        End Get
    End Property

    Public ReadOnly Property sortVisibility() As String
        Get
            Return "false"
        End Get
    End Property


    Private ReadOnly Property CurrentCommunityID() As Integer
        Get
            Dim qs_communityId As String = Request.QueryString("ComId")
            If IsNumeric(qs_communityId) Then
                Return qs_communityId
            Else
                Return Me.CurrentContext.UserContext.CurrentCommunityID
            End If
        End Get
    End Property

    Public ReadOnly Property CurrentPathId() As Long
        Get
            Dim qs_pathId As String = Me.Request.QueryString("PId")
            If IsNumeric(qs_pathId) Then
                Return qs_pathId
            Else
                Return -1
            End If
        End Get
    End Property
    Private ReadOnly Property CurrentUserId() As Integer
        Get
            Return Me.CurrentContext.UserContext.CurrentUserID
        End Get
    End Property
    Private ReadOnly Property CurrentCommRoleID As Integer
        Get
            Return UtenteCorrente.GetIDRuoloForComunita(CurrentCommunityID)
        End Get
    End Property



#End Region



    'Private Sub UpdateScormStat()
    '    Dim ModuleLinkIds As IList(Of Long) = ServiceEP.GetMaterialModuleLinkIds_ByPathId(Me.CurrentPathId)
    '    If ModuleLinkIds.Count > 0 Then
    '        Dim oSender As PermissionService.IServicePermission = Nothing
    '        Dim results As List(Of dtoItemEvaluation(Of Long))
    '        Dim UserID As Integer = CurrentContext.UserContext.CurrentUserID
    '        Try
    '            oSender = New PermissionService.ServicePermissionClient
    '            results = oSender.GetPendingEvaluations(ModuleLinkIds, UserID)
    '            If Not IsNothing(oSender) Then
    '                Dim service As System.ServiceModel.ClientBase(Of PermissionService.IServicePermission) = DirectCast(oSender, System.ServiceModel.ClientBase(Of PermissionService.IServicePermission))
    '                service.Close()
    '                service = Nothing
    '            End If
    '        Catch ex As Exception
    '            If Not IsNothing(oSender) Then
    '                Dim service As System.ServiceModel.ClientBase(Of PermissionService.IServicePermission) = DirectCast(oSender, System.ServiceModel.ClientBase(Of PermissionService.IServicePermission))
    '                service.Abort()
    '                service = Nothing
    '            End If
    '        End Try
    '        ServiceEP.SaveActionsExecution(results, UserID)
    '    End If
    'End Sub


    Private tempActivityRule As Long = 0

    Private _TempSelected As String

    Public Property TempSelected As String
        Get
            Return _TempSelected
        End Get
        Set(ByVal value As String)
            _TempSelected = value
        End Set
    End Property

    Private Sub InitProgressBar(ByVal isVisible As Boolean)
        If isVisible Then
            If isAutoEp Then
                Dim statForBar As dtoStatWithWeight = ServiceEP.ServiceStat.GetPassedCompletedWeight_byActivity(Me.CurrentPathId, Me.CurrentUserId, DateTime.Now)
                Me.CTRLprogressBar.InitBar(statForBar.Completion, statForBar.MinCompletion, "")
            Else
                Dim statForBar As dtoStatWithWeight = ServiceEP.ServiceStat.GetPassedCompletedWeight_byUnit(Me.CurrentPathId, Me.CurrentUserId, DateTime.Now)
                Me.CTRLprogressBar.InitBar(statForBar.Completion, statForBar.MinCompletion, Me.Resource.getValue("ExecutedWeight") & statForBar.UserTotWeight)

            End If
            MLVprogress.Visible = True
        Else
            MLVprogress.Visible = False
        End If
    End Sub

    Private Sub InitPlayer()
        Dim lastActivity As dtoActivityPlayer = ServiceEP.GetLastViewedActivity(Me.CurrentUserId, Me.CurrentCommRoleID, Me.CurrentPathId, isAutoEp, DateTime.Now)

        Me.Resource.setLabel(Me.LBplayerTitle)

        If IsNothing(lastActivity) Then
            ' NON SERVE ! COME NASCONDO UN dtoActivityPlayer CHE E' ANCHE NULLO ?????   hideControl(lastActivity)
            Me.MLVlastActivity.Visible = False
        Else
            Me.MLVlastActivity.Visible = True
            Me.PlayerUnitId = lastActivity.ParentUnitId
            Me.HYPcurrentActivityName.Text = lastActivity.Name
            Me.HYPcurrentActivityName.NavigateUrl = Me.BaseUrl & RootObject.PathView(CurrentPathId, CurrentCommunityID, EpViewModeType.View, True) & "#" & lastActivity.Id
            'If lastActivity.StartDate Is Nothing AndAlso lastActivity.EndDate Is Nothing Then
            '    hideControl(LBdate)

            'Else
            '    Me.LBdate.Text = Me.SetDateLabel(lastActivity.StartDate, lastActivity.EndDate)
            'End If
            If Not String.IsNullOrWhiteSpace(lastActivity.Description) Then
                Me.LBdescription.Text = SmartTagsAvailable.TagAll(lastActivity.Description)
            Else
                hideControl(LBdescription)
            End If
            SetImgStatusMedium(Me.IMGstatus, lastActivity.StatusStatistic)
        End If
    End Sub

    Private Sub initPageView()
        'UpdateScormStat()
        Me.CTRLhelpStatus.Init()
        Dim dtoEP As dtoEduPath = Me.ServiceEP.GetEduPathStructure_PlayMode(Me.CurrentPathId, Me.CurrentUserId, Me.CurrentCommRoleID, DateTime.Now)
        If IsNothing(dtoEP) OrElse ServiceEP.CheckStatus(dtoEP.Status, Status.Locked) Then
            Me.ShowError(EpError.NotPermission)
        Else
            Dim userId As Int32 = Me.CurrentUserId
            Dim pathId As Int64 = Me.CurrentPathId

            'unit rules
            Dim unit As IList(Of dtoUnitUser) = ServiceEP.GetFreeUnitsByPathId(pathId, userId)

            Dim rulesUnit As IList(Of RuleUnitCompletion) = ServiceEP.MergeUnitRulesWithUserCompletion(pathId, userId)

            Dim engineUnit As New RuleEngine(Of dtoUnitUser)

            'engine.AddRulesRange(rules)
            For Each rule As RuleUnitCompletion In rulesUnit
                engineUnit.AddRule(rule)
            Next

            Dim resultUnit As IList(Of KeyValuePair(Of dtoUnitUser, RuleEngineResult(Of dtoUnitUser))) = engineUnit.ExecuteFromTopByRoots(unit)
            If IsNothing(resultUnit) Then
                resultUnit = New List(Of KeyValuePair(Of dtoUnitUser, RuleEngineResult(Of dtoUnitUser)))
            End If
            VisibleUnit = (From item In resultUnit Select item.Key.Id).ToList()

            'end unit rules



            Dim act As IList(Of dtoActivityUser) = ServiceEP.GetFreeActivitiesByPathId(pathId, userId)

            '        Dim rules As IList(Of RuleActivityCompletion) = ServiceEP.GetActivityRulesByPathId(238)

            Dim rules As IList(Of RuleActivityCompletion) = ServiceEP.MergeActivityRulesWithUserCompletion(pathId, userId)

            Dim engine As New RuleEngine(Of dtoActivityUser)

            'engine.AddRulesRange(rules)
            For Each rule As RuleActivityCompletion In rules
                engine.AddRule(rule)
            Next

            Dim result As IList(Of KeyValuePair(Of dtoActivityUser, RuleEngineResult(Of dtoActivityUser))) = engine.ExecuteFromTopByRoots(act)
            If IsNothing(result) Then
                result = New List(Of KeyValuePair(Of dtoActivityUser, RuleEngineResult(Of dtoActivityUser)))
            End If
            VisibleActivity = (From item In result Select item.Key.Id).ToList()

            'VisibleActivity = Nothing

            If ServiceStat.InitPathBrowsed(pathId, Me.CurrentUserId, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then
                InitPlayer()
                ' InitProgressBar(dtoEP.
                Me.Master.ServiceTitle = dtoEP.Name
                Me.HYPstat.NavigateUrl = Me.BaseUrl & RootObject.UserStatisticsView(CurrentPathId, Me.CurrentCommunityID, DateTime.Now, False)

                If ServiceEP.GetPathCount_ViewMode(CurrentUserId, CurrentCommRoleID, CurrentCommunityID) > 2 Then
                    Me.HYPlistEduPath.NavigateUrl = Me.BaseUrl & RootObject.EduPathList(Me.CurrentCommunityID, EpViewModeType.View)
                Else
                    hideControl(Me.HYPlistEduPath)
                End If

                Me.PermissionOverPath = dtoEP.PermissionEP

                Me.InitProgressBar(True)
                If VisibleUnit Is Nothing Then
                    RPunit.DataSource = dtoEP.Units
                Else
                    RPunit.DataSource = (From item In dtoEP.Units Where VisibleUnit.Contains(item.Id)).ToList()
                End If

                RPunit.DataBind()
            Else
                Me.ShowError(EpError.Generic)
            End If

        End If
    End Sub




    Private Sub ShowError(ByVal ErrorType As EpError)
        Me.Resource.setHyperLink(Me.HYPerror, False, True)
        Me.HYPerror.NavigateUrl = Me.BaseUrl & RootObject.EduPathList(Me.CurrentCommunityID, EpViewModeType.View)
        Select Case ErrorType
            Case EpError.Generic
                Me.LBerror.Text = Me.Resource.getValue("Error." & EpError.Generic.ToString)
                Me.PageUtility.AddAction(Services_EduPath.ActionType.GenericError, Nothing, InteractionType.UserWithLearningObject)
            Case EpError.NotPermission
                Me.LBerror.Text = Me.Resource.getValue("Error." & EpError.NotPermission.ToString)
                Me.PageUtility.AddAction(Services_EduPath.ActionType.NoPermission, Nothing, InteractionType.UserWithLearningObject)
            Case EpError.Url
                Me.LBerror.Text = Me.Resource.getValue("Error." & EpError.Url.ToString)
                Me.PageUtility.AddAction(Services_EduPath.ActionType.GenericError, Nothing, InteractionType.UserWithLearningObject)
            Case EpError.PathNotFind
                Me.LBerror.Text = Me.Resource.getValue("Error." & EpError.Url.ToString)
                Me.PageUtility.AddAction(Services_EduPath.ActionType.GenericError, Nothing, InteractionType.UserWithLearningObject)

        End Select
        Me.MLVeduPathView.ActiveViewIndex = 1
    End Sub

#Region "Repeater Item Data Bound"

    Private Function SetDateLabel(ByVal StartDate As Date?, ByVal EndDate As Date?) As String
        If Not StartDate Is Nothing Then

            If Not EndDate Is Nothing Then
                Return String.Format(Me.Resource.getValue("Between"), ServiceEP.GetDate(StartDate), ServiceEP.GetDate(EndDate))
            Else
                Return Me.Resource.getValue("After") & ServiceEP.GetDate(StartDate)
            End If

        ElseIf Not EndDate Is Nothing Then
            Return Me.Resource.getValue("Before") & ServiceEP.GetDate(EndDate)

        Else
            Return " - "
        End If
    End Function

    Public Function EnableCurrentUnit(ByVal UnitId As Long) As String
        If UnitId = Me.PlayerUnitId Then
            Return " current"
        End If
        Return ""
    End Function

    Public Function SetIfUnitIsUnmovable() As String
        If PermissionOverPath.Update Then
            Return " unmovable"
        End If
        Return ""
    End Function

    Public Function SetIfActivityIsUnmovable() As String
        If PermissionOverUnit.Update Then
            Return " unmovable"
        End If
        Return ""
    End Function


    Private Sub SetImgBlocked(ByRef oImg As System.Web.UI.WebControls.Image, ByVal isBlocked As Boolean)
        If isBlocked Then

            oImg.ImageUrl = RootObject.ImgItemBlocked_Off(Me.BaseUrl)
            oImg.AlternateText = Me.Resource.getValue("Blocked." & isBlocked)
        Else
            oImg.ImageUrl = RootObject.ImgBlindMedium(Me.BaseUrl)
        End If
        oImg.Visible = True
    End Sub


    Private Sub RPunitGeneric_ItemDataBound(ByRef e As System.Web.UI.WebControls.RepeaterItemEventArgs, ByRef dtoItem As dtoUnit)

        Dim LBunit As Label
        Dim RpActivities As Repeater

        If isAutoEp Then
            hideControl(e.Item.FindControl("IMGstatus"))
        Else
            SetImgStatusMedium(e.Item.FindControl("IMGstatus"), dtoItem.statusStat)
        End If

        If Not ServiceStat.UnitIsCompleted(dtoItem.Activities) Then
            Dim oSpan As HtmlControl = e.Item.FindControl("SPopenClose")
            oSpan.Attributes.Add("class", "switch autoOpen")
        End If

        Dim oImg As System.Web.UI.WebControls.Image


        oImg = e.Item.FindControl("IMGvisibility")

        _canViewSubAct_byUnit = ServiceEP.CheckStatus(dtoItem.Status, Status.NotLocked)
        SetImgBlocked(oImg, Not _canViewSubAct_byUnit)

        oImg = e.Item.FindControl("IMGmandatory")

        'If ServiceEP.CheckStatus(dtoItem.Status, Status.Mandatory) Then
        '    oImg.ImageUrl = RootObject.ImgMandatoryMedium(Me.BaseUrl)
        '    oImg.ToolTip = Me.Resource.getValue("UnitMandatory")
        '    oImg.Visible = True
        'Else
        '    hideControl(oImg)
        'End If

        LBunit = e.Item.FindControl("LBunit")
        LBunit.Visible = True
        LBunit.Text = dtoItem.Name

        LBunit = e.Item.FindControl("LBunitWeight")
        LBunit.Visible = True
        If isMarkEp Then
            LBunit.Text = " (" & dtoItem.Weight & Resource.getValue("Points")
        Else
            LBunit.Text = ServiceEP.GetTime(dtoItem.Weight)
        End If

        LBunit = e.Item.FindControl("LBunitDesc")
        If Not String.IsNullOrWhiteSpace(dtoItem.Description) Then

            LBunit.Text = dtoItem.Description
        Else
            hideControl(LBunit)
            hideControl(e.Item.FindControl("DIVunitDescription"))
        End If

        Me.TempUnitId = dtoItem.Id
        RpActivities = e.Item.FindControl("RPactivity")

        'from rule

        If VisibleActivity Is Nothing Then
            RpActivities.DataSource = dtoItem.Activities
        Else
            RpActivities.DataSource = (From item In dtoItem.Activities Where VisibleActivity.Contains(item.Id)).ToList()
        End If

        AddHandler RpActivities.ItemDataBound, AddressOf RPactivity_ItemDataBound
        RpActivities.DataBind()

        Dim Rulelist As List(Of dtoRule) = ServiceEP.GetUnitDtoRuleByUnitId(dtoItem.Id)

        If Rulelist.Count > 0 Then
            Dim RpRules As Repeater = e.Item.FindControl("RPunitrule")
            RpRules.DataSource = Rulelist
            AddHandler RpRules.ItemDataBound, AddressOf RPunitRule_ItemDataBound
            RpRules.DataBind()
        Else

            Dim ulRule As HtmlControl = e.Item.FindControl("ULunitRules")
            ulRule.Attributes("class") = "empty"
            ulRule.Visible = False
            hideControl(ulRule)
        End If
    End Sub

    Private Sub RPunit_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPunit.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dtoItem As dtoUnit = e.Item.DataItem
            _unitId = dtoItem.Id

            If ServiceEP.CheckStatus(dtoItem.Status, Status.Text) Then
                hideControl(e.Item.FindControl("LIunit"))
                Dim oLB As Label = e.Item.FindControl("LBtextNote")
                oLB.Text = dtoItem.Description

            Else
                hideControl(e.Item.FindControl("LInote"))

                Me.RPunitGeneric_ItemDataBound(e, dtoItem)

            End If

        End If
    End Sub

    Public Sub RPunitRule_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim lbl As Label
            Dim dtoItem As dtoRule = e.Item.DataItem
            lbl = e.Item.FindControl("LBunitrule")
            lbl.Text = dtoItem.Name

        End If
    End Sub

    Private Sub RPactivityGeneric_ItemDataBound(ByRef e As System.Web.UI.WebControls.RepeaterItemEventArgs, ByRef dtoItem As dtoActivity)

        Dim oHyp As HyperLink
        Dim oLb As Label
        Dim oRp As Repeater

        Dim isBlocked As Boolean = ServiceEP.CheckStatus(dtoItem.Status, Status.Locked)

        Dim oImg As System.Web.UI.WebControls.Image
        oImg = e.Item.FindControl("IMGvisibility")
        _canViewSubAct_byAct = _canViewSubAct_byUnit And Not isBlocked
        SetImgBlocked(oImg, isBlocked)

        oLb = e.Item.FindControl("LBactName")
        SetLbActName(oLb, dtoItem.Name)

        oLb = e.Item.FindControl("LBdescription")
        If Not String.IsNullOrWhiteSpace(dtoItem.Description) Then
            oLb.Text = dtoItem.Description
        Else
            hideControl(oLb)
        End If

        SetImgStatusSmall(e.Item.FindControl("IMGstatus"), dtoItem.statusStat)
        oImg = e.Item.FindControl("IMGmandatory")
        If ServiceEP.CheckStatus(dtoItem.Status, Status.Mandatory) Then

            oImg.ImageUrl = RootObject.ImgMandatorySmall(Me.BaseUrl)
            oImg.ToolTip = Me.Resource.getValue("ActivityMandatory")
        Else
            hideControl(oImg)
        End If

        oLb = e.Item.FindControl("LBactWeight")
        oLb.Visible = True
        If isMarkEp Then
            oLb.Text = " (" & dtoItem.Weight & Resource.getValue("Points")
        Else
            oLb.Text = ServiceEP.GetTime(dtoItem.Weight)
        End If

        oLb = e.Item.FindControl("LBdate")
        If dtoItem.StartDate Is Nothing AndAlso dtoItem.EndDate Is Nothing Then
            hideControl(oLb)
        Else
            oLb.Text = SetDateLabel(dtoItem.StartDate, dtoItem.EndDate)
        End If


        Dim Rules As List(Of dtoRule) = ServiceEP.GetActivityDtoRuleByActivityId(dtoItem.Id)
        If Rules.Count > 0 Then
            oRp = e.Item.FindControl("RPrule")
            oRp.DataSource = Rules
            AddHandler oRp.ItemDataBound, AddressOf RPrule_ItemDataBound
            oRp.DataBind()
        Else
            Dim ul As HtmlControl = e.Item.FindControl("ULactRules")
            ul.Attributes("class") = "empty"
            hideControl(ul)
        End If

        _isVisibleByDateConstraint = ServiceEP.CanViewAct_byDate(dtoItem.StartDate, dtoItem.EndDate)

        If _canViewSubAct_byAct AndAlso dtoItem.SubActivities.Count > 0 Then

            Dim MLVsingleSubAct As MultiView = e.Item.FindControl("MLVsingleSubAct")

            Dim VIWSingle As View = e.Item.FindControl("VIWSingle")


            If CouldActivityWithSingleSubActivityBeOmitted And dtoItem.SubActivities.Count = 1 Then
                MLVsingleSubAct.ActiveViewIndex = 0

                SingleSubActivity(dtoItem.SubActivities.First(), VIWSingle)


                oImg = e.Item.FindControl("IMGvisibility_single")
                _canViewSubAct_byAct = _canViewSubAct_byUnit And Not isBlocked
                SetImgBlocked(oImg, isBlocked)

                SetImgStatusSmall(e.Item.FindControl("IMGstatus_single"), dtoItem.statusStat)
                oImg = e.Item.FindControl("IMGmandatory_single")
                If ServiceEP.CheckStatus(dtoItem.Status, Status.Mandatory) Or ServiceEP.CheckStatus(dtoItem.SubActivities.First().Status, Status.Mandatory) Then

                    oImg.ImageUrl = RootObject.ImgMandatorySmall(Me.BaseUrl)
                    oImg.ToolTip = Me.Resource.getValue("ActivityMandatory")
                    oImg.Visible = True
                Else
                    hideControl(oImg)
                End If

                oLb = e.Item.FindControl("LBactWeight_single_activity")
                oLb.Visible = True
                If isMarkEp Then
                    oLb.Text = " (" & dtoItem.Weight & Resource.getValue("Points")
                Else
                    oLb.Text = ServiceEP.GetTime(dtoItem.Weight)
                End If

                oLb = e.Item.FindControl("LBdate_single_activity")
                If dtoItem.StartDate Is Nothing AndAlso dtoItem.EndDate Is Nothing Then
                    hideControl(oLb)
                Else
                    oLb.Text = SetDateLabel(dtoItem.StartDate, dtoItem.EndDate)
                End If

            Else
                MLVsingleSubAct.ActiveViewIndex = 1

                oRp = e.Item.FindControl("RPsubAct")
                oRp.DataSource = dtoItem.SubActivities
                AddHandler oRp.ItemDataBound, AddressOf RpSubAct_ItemDataBound
                oRp.DataBind()
            End If





        Else
            hideControl(e.Item.FindControl("UlsubAct"))
        End If
    End Sub



    Private Sub SetImgStatusMedium(ByRef oImg As System.Web.UI.WebControls.Image, ByRef status As StatusStatistic)
        oImg.Visible = True
        Dim tooltip As String
        Select Case status
            Case StatusStatistic.CompletedPassed
                oImg.ImageUrl = RootObject.ImgStatusGreenMedium(Me.BaseUrl)
                tooltip = Resource.getValue("StatusStatistic." & StatusStatistic.CompletedPassed.ToString)

            Case StatusStatistic.Browsed
                oImg.ImageUrl = RootObject.ImgStatusGreyMedium(Me.BaseUrl)
                tooltip = Resource.getValue("StatusStatistic." & StatusStatistic.Browsed.ToString)
            Case StatusStatistic.None
                oImg.ImageUrl = RootObject.ImgStatusGreyMedium(Me.BaseUrl)
                tooltip = Resource.getValue("StatusStatistic." & StatusStatistic.Browsed.ToString)
            Case Else
                oImg.ImageUrl = RootObject.ImgStatusYellowMedium(Me.BaseUrl)
                tooltip = Resource.getValue("StatusStatistic." & StatusStatistic.Started.ToString)
        End Select
        oImg.AlternateText = tooltip
        oImg.ToolTip = tooltip
    End Sub

    Private Sub SetImgStatusSmall(ByRef oImg As System.Web.UI.WebControls.Image, ByRef status As StatusStatistic)
        oImg.Visible = True
        Dim tooltip As String
        Select Case status
            Case StatusStatistic.CompletedPassed
                oImg.ImageUrl = RootObject.ImgStatusGreenSmall(Me.BaseUrl)
                tooltip = Resource.getValue("StatusStatistic." & StatusStatistic.CompletedPassed.ToString)
            Case StatusStatistic.Browsed
                oImg.ImageUrl = RootObject.ImgStatusGreySmall(Me.BaseUrl)
                tooltip = Resource.getValue("StatusStatistic." & StatusStatistic.Browsed.ToString)
            Case StatusStatistic.None
                oImg.ImageUrl = RootObject.ImgStatusGreySmall(Me.BaseUrl)
                tooltip = Resource.getValue("StatusStatistic." & StatusStatistic.Browsed.ToString)
            Case Else
                oImg.ImageUrl = RootObject.ImgStatusYellowSmall(Me.BaseUrl)
                tooltip = Resource.getValue("StatusStatistic." & StatusStatistic.Started.ToString)
        End Select
        oImg.AlternateText = tooltip
        oImg.ToolTip = tooltip
    End Sub

    Private Function UnitIsCompleted(ByRef acts As IList(Of dtoActivity))
        Return ServiceStat.UnitIsCompleted(acts)
    End Function

    Public Sub RPactivity_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dtoItem As dtoActivity = e.Item.DataItem

            If ServiceEP.CheckStatus(dtoItem.Status, Status.Text) Then

                hideControl(e.Item.FindControl("LIact"))
                Dim oLB As Label = e.Item.FindControl("LBtextNote")
                oLB.Text = dtoItem.Description

            Else
                hideControl(e.Item.FindControl("LInote"))
                RPactivityGeneric_ItemDataBound(e, dtoItem)

            End If

        End If
    End Sub

    Private Sub SetLbActName(ByRef oLB As Label, ByRef name As String)
        oLB.Text = name
    End Sub




    Public Sub RPrule_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim lbl As Label
            Dim dtoItem As dtoRule = e.Item.DataItem
            lbl = e.Item.FindControl("LBrule")
            lbl.Text = dtoItem.Name


        End If
    End Sub

#End Region


#Region "SubAct"

    Public Sub RpSubAct_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs)
        If ServiceEP.ExecuteSubActivityInternal(e.CommandArgument, Me.CurrentUserId, Me.CurrentCommRoleID, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then
            BindDati()
        Else
            ShowError(EpError.Generic)
        End If
    End Sub

    Private Sub SingleSubActivity(dtoItem As dtoSubActivity, e As View)
        Dim oLb As Label
        Dim oLkb As LinkButton
        Dim oHyp As HyperLink
        Dim oImg As System.Web.UI.WebControls.Image

        Dim oDisplayAction As lm.Comol.Core.ModuleLinks.IExternalModuleDisplayAction = CType(LoadControl(BaseUrl & Common.CoreRootObject.GenericNewDisplayActionControl), lm.Comol.Core.ModuleLinks.IExternalModuleDisplayAction)

        SetImgStatusSmall(e.FindControl("IMGstatus_single"), dtoItem.StatusStat)

        'oImg = e.FindControl("IMGmandatory_single")

        'If ServiceEP.CheckStatus(dtoItem.Status, Status.Mandatory) Then
        '    oImg.ImageUrl = RootObject.ImgMandatoryMedium(Me.BaseUrl)
        '    oImg.ToolTip = Me.Resource.getValue("SubActivityMandatory")
        '    oImg.Visible = True
        'Else
        '    hideControl(oImg)
        'End If

        Dim isVisible As Boolean = ServiceEP.CheckStatus(dtoItem.Status, Status.NotLocked)

        SetImgBlocked(e.FindControl("IMGvisibility_single"), Not isVisible)


        'oImg = e.Item.FindControl("IMGtype")
        'oImg.ImageUrl = RootObject.ImgContentType(Me.BaseUrl, dtoItem.ContentType)


        oLb = e.FindControl("LBpoints_single")

        If isMarkEp Then
            oLb.Text = " (" & dtoItem.Weight & Resource.getValue("Points")
        Else
            hideControl(oLb)  ' oLb.Text = ServiceEP.GetTime(dtoItem.Weight)
        End If

        oLb = e.FindControl("LBnameTitle_single")
        Me.Resource.setLabel(oLb)

        oHyp = e.FindControl("HYPname_single")
        oHyp.Text = dtoItem.Name
        oHyp.NavigateUrl = dtoItem.Link

        oLb = e.FindControl("LBdescription_single")

        oLkb = e.FindControl("LKBexecute_single")

        If ServiceEP.isSubActityInternalModule(dtoItem.ContentType) Then
            oLb.Text = Me.SmartTagsAvailable.TagAll(dtoItem.Description)

            If isVisible AndAlso _isVisibleByDateConstraint AndAlso CanProceedByEndDate Then
                Me.Resource.setLinkButton(oLkb, False, True)
                oLkb.CommandArgument = dtoItem.Id
            Else
                hideControl(oLkb)
            End If

        Else
            hideControl(oLkb)
            oLb.Text = Me.SmartTagsAvailable.TagAll(dtoItem.Description)
            Dim oPlaceHolder As PlaceHolder = e.FindControl("PLHaction_single")


            Dim initializer As New lm.Comol.Core.ModuleLinks.dtoExternalModuleInitializer
            oDisplayAction.ContainerCSS = SubActivityCssClass(dtoItem)
            oDisplayAction.IconSize = Helpers.IconSize.Small

            oDisplayAction.EnableAnchor = True

            initializer.Link = dtoItem.ModuleLink

            If isVisible AndAlso _isVisibleByDateConstraint AndAlso CanProceedByEndDate Then
                oDisplayAction.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction

                '     oDisplayAction.InitializeControlInlineByLink(dtoItem.ModuleLink)
            Else
                oDisplayAction.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.text
                '  oDisplayAction.InitializeRemoteControlInlineByLink(True, dtoItem.ModuleLink)
            End If
            oDisplayAction.InitializeControl(initializer)
            oPlaceHolder.Controls.Add(oDisplayAction)

        End If

        If Not _isVisibleByDateConstraint Then
            Dim oSp As HtmlControl = e.FindControl("SPsubActName_single")
            oSp.Attributes.Add("class", "row left timeConstraint")
        End If
    End Sub

    Private Sub RpSubAct_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim oLb As Label
            Dim oLkb As LinkButton
            Dim oHyp As HyperLink
            Dim oImg As System.Web.UI.WebControls.Image
            Dim dtoItem As dtoSubActivity = e.Item.DataItem
            Dim oDisplayAction As lm.Comol.Core.ModuleLinks.IExternalModuleDisplayAction = CType(LoadControl(BaseUrl & Common.CoreRootObject.GenericNewDisplayActionControl), lm.Comol.Core.ModuleLinks.IExternalModuleDisplayAction)

            SetImgStatusSmall(e.Item.FindControl("IMGstatus"), dtoItem.StatusStat)

            oImg = e.Item.FindControl("IMGmandatory")

            If ServiceEP.CheckStatus(dtoItem.Status, Status.Mandatory) Then
                oImg.ImageUrl = RootObject.ImgMandatoryMedium(Me.BaseUrl)
                oImg.ToolTip = Me.Resource.getValue("SubActivityMandatory")
                oImg.Visible = True
            Else
                hideControl(oImg)
            End If

            Dim isVisible As Boolean = ServiceEP.CheckStatus(dtoItem.Status, Status.NotLocked)

            SetImgBlocked(e.Item.FindControl("IMGvisibility"), Not isVisible)


            'oImg = e.Item.FindControl("IMGtype")
            'oImg.ImageUrl = RootObject.ImgContentType(Me.BaseUrl, dtoItem.ContentType)


            oLb = e.Item.FindControl("LBpoints")

            If isMarkEp Then
                oLb.Text = " (" & dtoItem.Weight & Resource.getValue("Points")
            Else
                hideControl(oLb)  ' oLb.Text = ServiceEP.GetTime(dtoItem.Weight)
            End If

            oLb = e.Item.FindControl("LBnameTitle")
            Me.Resource.setLabel(oLb)

            oHyp = e.Item.FindControl("HYPname")
            oHyp.Text = dtoItem.Name
            oHyp.NavigateUrl = dtoItem.Link

            oLb = e.Item.FindControl("LBdescription")

            oLkb = e.Item.FindControl("LKBexecute")

            If ServiceEP.isSubActityInternalModule(dtoItem.ContentType) Then
                oLb.Text = Me.SmartTagsAvailable.TagAll(dtoItem.Description)

                If isVisible AndAlso _isVisibleByDateConstraint AndAlso CanProceedByEndDate Then
                    Me.Resource.setLinkButton(oLkb, False, True)
                    oLkb.CommandArgument = dtoItem.Id
                Else
                    hideControl(oLkb)
                End If

            Else
                hideControl(oLkb)
                oLb.Text = Me.SmartTagsAvailable.TagAll(dtoItem.Description)
                Dim oPlaceHolder As PlaceHolder = e.Item.FindControl("PLHaction")

                Dim initializer As New lm.Comol.Core.ModuleLinks.dtoExternalModuleInitializer
                oDisplayAction.ContainerCSS = SubActivityCssClass(dtoItem)
                oDisplayAction.IconSize = Helpers.IconSize.Small

                oDisplayAction.EnableAnchor = True

                initializer.Link = dtoItem.ModuleLink
                If isVisible AndAlso _isVisibleByDateConstraint AndAlso CanProceedByEndDate Then
                    oDisplayAction.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction

                    '     oDisplayAction.InitializeControlInlineByLink(dtoItem.ModuleLink)
                Else
                    oDisplayAction.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.text
                    '  oDisplayAction.InitializeRemoteControlInlineByLink(True, dtoItem.ModuleLink)
                End If
                oDisplayAction.InitializeControl(initializer)

                'If isVisible AndAlso _isVisibleByDateConstraint AndAlso CanProceedByEndDate Then
                '    oDisplayAction.InitializeControlInlineByLink(dtoItem.ModuleLink)
                'Else

                '    oDisplayAction.InitializeRemoteControlInlineByLink(True, dtoItem.ModuleLink)
                'End If

                oPlaceHolder.Controls.Add(oDisplayAction)

            End If

            If Not _isVisibleByDateConstraint Then
                Dim oSp As HtmlControl = e.Item.FindControl("SPsubActName")
                oSp.Attributes.Add("class", "row left timeConstraint")
            End If

        End If

    End Sub


#End Region

    Protected Overrides Sub NotifyModuleStatus(status As ModuleStatus)

    End Sub

    Protected Overrides Sub NotifyUnavailableModule(status As ModuleStatus)

    End Sub
    Protected Overrides ReadOnly Property CheckModuleStatus As Boolean
        Get
            Return False
        End Get
    End Property
End Class