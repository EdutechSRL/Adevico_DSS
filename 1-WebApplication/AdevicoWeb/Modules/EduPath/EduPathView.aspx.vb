Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.EduPath.BusinessLogic
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.Modules.EduPath.Domain
Imports lm.Comol.Modules.EduPath.Domain.DTO
Imports lm.ActionDataContract
Imports lm.Comol.Core.DomainModel

Public Class EduPathView
    Inherits EPpageBaseEduPath

#Region "InitStandard"

    Private _unitIndex As Int16
    Private _actIndex As Int16
    Private _Service As Services_EduPath
    Private _TempUnitId As Long
    Private _PlayerCssVisibility As String
    Private _PlayerUnitId As Long
    Private _VisibleActivity As IList(Of Int64)
    Private _VisibleUnit As IList(Of Int64)

    Private Property VisibleActivity As IList(Of Int64)
        Get
            Return _VisibleActivity
        End Get
        Set(ByVal value As IList(Of Int64))
            _VisibleActivity = value
        End Set
    End Property

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

#Region "Inherits"
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            If IsSessioneScaduta(False) Then
                RedirectOnSessionTimeOut(Request.Url.AbsoluteUri, CurrentCommunityID)
            End If
            Return False
        End Get
    End Property
#End Region

#Region "Internal"
    Private Property IsInReadOnlyMode As Boolean
        Get
            Return ViewStateOrDefault("IsInReadOnlyMode", False)
        End Get
        Set(value As Boolean)
            ViewState("IsInReadOnlyMode") = value
        End Set
    End Property
#Region "Property"
    Private _PermissionOverPath As PermissionEP = New PermissionEP(RoleEP.None)
    Private _PermissionOverUnit As PermissionEP = New PermissionEP(RoleEP.None)
    Private _UnitsCount As Int16
    Private _ActivitiesCount As Int16
    Private _unitId As Int16
    Private _canAddUnitRules As Boolean = False
    Private _canAddActivityRules As Boolean = False


    Private _SmartTagsAvailable As SmartTags

    Public Property ShowSwitchMessage As Boolean
        Get
            Return Me.ViewState("ShowSwitchMessage")
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("ShowSwitchMessage") = value
        End Set
    End Property

    Public Property IsEditablePath As Boolean
        Get
            Return Me.ViewState("IsEditablePath")
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("IsEditablePath") = value
        End Set
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

    Public Property PlayerCssVisibility As String
        Get
            If _PlayerCssVisibility Is Nothing Then
                _PlayerCssVisibility = ""
            End If
            Return _PlayerCssVisibility
        End Get
        Set(ByVal value As String)

            _PlayerCssVisibility = value
        End Set
    End Property

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
            If Me.ViewModeType = EpViewModeType.Manage Then
                Return "true"
            End If
            Return "false"
        End Get
    End Property

    Private ReadOnly Property ViewModeType As EpViewModeType
        Get
            Dim qs_viewModeType As String = Request.QueryString("ViewMode")
            If IsNumeric(qs_viewModeType) Then

                Select Case qs_viewModeType
                    Case EpViewModeType.Manage
                        Return EpViewModeType.Manage
                    Case EpViewModeType.View
                        Return EpViewModeType.View
                    Case Else
                        Return EpViewModeType.None
                End Select
            Else
                Return EpViewModeType.None
            End If
        End Get
    End Property
    Private ReadOnly Property CurrentCommunityID() As Integer
        Get
            Dim qs_communityId As String = Request.QueryString("ComId")
            If IsNumeric(qs_communityId) Then
                Return qs_communityId
            Else
                Return PageUtility.CurrentContext.UserContext.CurrentCommunityID
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
            Return PageUtility.CurrentContext.UserContext.CurrentUserID
        End Get
    End Property
    Private ReadOnly Property IdCommunityRole As Integer
        Get
            Dim key As String = "CurrentCommRoleID_" & CurrentPathId.ToString() & "_" & CurrentUserId.ToString
            Dim idRole As Integer = ViewStateOrDefault(key, -1)
            If idRole = -1 Then
                idRole = ServiceEP.GetIdCommunityRole(CurrentUserId, ServiceEP.GetPathIdCommunity(CurrentPathId))
                ViewState(key) = idRole
            End If
            Return idRole
        End Get
    End Property

#End Region
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region " Inherits"
    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_EduPath.Codex)
    End Sub

    Public Overrides Sub BindDati()
        Me.PageUtility.AddAction(Me.CurrentCommunityID, Services_EduPath.ActionType.Access, Me.PageUtility.CreateObjectsList(Services_EduPath.ObjectType.EduPath, Me.CurrentPathId), InteractionType.UserWithLearningObject)
        Select Case Me.ViewModeType
            Case EpViewModeType.Manage
                initPageManage()
            Case EpViewModeType.View
                RedirectToUrl(RootObject.EduPathList(CurrentCommunityID, EpViewModeType.View, PreloadIsMooc))

                'If ServiceEP.isPlayModePath(CurrentPathId) Then
                '    RedirectToUrl(RootObject.PathView(CurrentPathId, CurrentCommunityID, EpViewModeType.View, True))
                'End If
                'initPageView()
            Case Else
                Me.ShowError(EpError.Url)
        End Select
    End Sub

    Public Overrides Sub BindNoPermessi()
        ShowError(EpError.NotPermission)
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        'non si possono controllare qui gli altri permessi visto che un utente potrebbe avere i permessi attivi 
        'esclusivamente in un sub item e non sull'intero path
        If ViewModeType = EpViewModeType.View Then
            Return ServiceEP.CheckCommunityId(Of Path)(Me.CurrentPathId, Me.CurrentCommunityID) AndAlso ServiceEP.ItemIsVisible(CurrentPathId, ItemType.Path)
        ElseIf ViewModeType = EpViewModeType.Manage Then
            Return ServiceEP.CheckCommunityId(Of Path)(Me.CurrentPathId, Me.CurrentCommunityID)
        Else
            Return False
        End If
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        If Not Page.IsPostBack AndAlso String.IsNullOrWhiteSpace(Request.QueryString("isMooc")) Then
            IsMoocPath = ServiceEP.IsMooc(CurrentPathId)
        ElseIf Not Page.IsPostBack Then
            IsMoocPath = PreloadIsMooc
        Else
            IsMoocPath = ServiceEP.IsMooc(CurrentPathId)
        End If
        If IsMoocPath Then
            MyBase.SetCulture("pg_MoocView", "EduPath")
        Else
            MyBase.SetCulture("pg_EpView", "EduPath")
        End If
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setHyperLink(Me.HYPnewUnit, False, True)
            .setHyperLink(Me.HYPlistEduPath, False, True)
            .setHyperLink(Me.HYPnewUnitNote, False, True)
            .setLabel(Me.LBunitList)
            .setLabel(LBdescriptionTitle)
            .setLinkButton(LNBcreateUnitRule, False, True)
            .setLinkButtonForName(LNBcreateActivityRule, "LNBcreateUnitRule", False, True)

            .setCompareValidator(COVlowerThan)
            .setCompareValidator(COVgreaterThan, COVlowerThan)
            .setCompareValidator(COVupperBound, COVlowerThan)
            .setCompareValidator(COVlowerBound, COVlowerThan)
            .setCompareValidator(COVbetween)
            .setRangeValidator(RVlowerThan)
            .setRangeValidator(RVgreaterThan, RVlowerThan)
            .setRangeValidator(RVlowerBound, RVlowerThan)
            .setRangeValidator(RVupperBound, RVlowerThan)

            .setLiteral(LITactivity1c)

            .setLiteral(LITactivity4)
            .setLiteral(LITactivity5)
            .setLiteral(LITexplanation)
            .setLabel(LBswitch)
            .setLabel(LBswitchCombo)
            .setLabel(LbSwitchFirst)
            .setButton(Me.BTNswitch)
            .setHyperLink(HYPannulSwitch, False, True)
            .setHyperLink(HYPvalidateByAct, False, True)

        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub

#End Region



    Public Function GetActivityCssClass(ByRef status As Status) As String
        If ServiceEP.CheckStatus(status, status.Text) Then
            Return " note "
        Else
            Return " activity "
        End If
    End Function

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
        Else
            hideControl(DIVprogressBar)
        End If
    End Sub

    Private Sub InitPlayer()
        Dim lastActivity As dtoActivityPlayer = ServiceEP.GetLastViewedActivity(Me.CurrentUserId, Me.IdCommunityRole, Me.CurrentPathId, isAutoEp, DateTime.Now)

        Me.Resource.setLabel(Me.LBplayerTitle)


        If lastActivity Is Nothing Then

            Me.PlayerCssVisibility = "hidePlayer"
            Me.LBdescription.Text = Me.Resource.getValue("NoPlayerActivities")
            Me.LBcurrentActivityName.Visible = False
            Me.SPANactivitiesInProgress.Visible = False
            Me.LBdate.Visible = False

            LBstatusPlayerTitle.Visible = False
            LBdateTitle.Visible = False
            LBdescriptionTitle.Visible = False

        Else

            Me.SPANactivitiesInProgress.Visible = True
            Me.PlayerUnitId = lastActivity.ParentUnitId
            Me.Resource.setLabel(Me.LBdescriptionTitle)
            Me.Resource.setLabel(Me.LBstatusPlayerTitle)
            Me.Resource.setLabel(Me.LBdateTitle)
            Me.LBcurrentActivityName.Text = lastActivity.Name
            Me.LBdate.Text = Me.SetDateLabel(lastActivity.StartDate, lastActivity.EndDate)
            Me.LBdescription.Text = SmartTagsAvailable.TagAll(lastActivity.Description)
            Me.LBstatusPlayer.Text = Me.Resource.getValue("StatusStatistic." & lastActivity.StatusStatistic.ToString)
            'Me.IMGhelp.ImageUrl = RootObject.ImgHelp(Me.BaseUrl)
            'Me.IMGhelp.ToolTip = Me.Resource.getValue("IMGhelp")
            'Me.IMGhelp.AlternateText = Me.Resource.getValue("IMGhelp")
            SetImgStatusMedium(Me.IMGstatus, lastActivity.StatusStatistic)

        End If

    End Sub

    Private Sub initPageView()
        'UpdateScormStat()
        Me.CTRLhelpStatus.Init()
        Dim dtoEP As dtoEduPath = Me.ServiceEP.GetEduPathStructure_View(Me.CurrentPathId, Me.CurrentUserId, Me.IdCommunityRole, OLDpageUtility.ProxyIPadress, OLDpageUtility.ClientIPadress, DateTime.Now)



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

            If ServiceStat.InitPathBrowsed(pathId, Me.CurrentUserId, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then 'init path statististc
                InitPlayer()
                ' InitProgressBar(dtoEP.
                Me.Master.ServiceTitle = dtoEP.Name
                Me.HYPlistEduPath.Visible = ViewModeType = EpViewModeType.Manage OrElse (ServiceEP.GetEduPathCountInCommunity(Me.CurrentCommunityID, True) > 1)
                Me.HYPlistEduPath.NavigateUrl = Me.BaseUrl & RootObject.EduPathList(Me.CurrentCommunityID, Me.ViewModeType, dtoEP.IsMooc)
                Me.PermissionOverPath = dtoEP.PermissionEP

                Me.InitProgressBar(True)
                If VisibleUnit Is Nothing Then
                    RPunit.DataSource = dtoEP.Units
                Else
                    RPunit.DataSource = (From item In dtoEP.Units Where VisibleUnit.Contains(item.Id)).ToList()
                End If
                _unitIndex = 1
                RPunit.DataBind()
            Else
                Me.ShowError(EpError.Generic)
            End If

        End If
    End Sub

    Private Sub initPageManage()
        Dim dtoEP As dtoEduPath = Me.ServiceEP.GetEduPathStructure_Manage(Me.CurrentPathId, Me.CurrentUserId, Me.IdCommunityRole, OLDpageUtility.ProxyIPadress, OLDpageUtility.ClientIPadress)
        IsInReadOnlyMode = PreloadIsForReadOnly
        If Not IsNothing(dtoEP) Then
            Dim status As PathAvailability = ServiceEP.GetPathAvailability(CurrentPathId, CurrentUserId)
            IsEditablePath = (status = PathAvailability.Blocked)

            IsMoocPath = dtoEP.IsMooc
            Dim autoWeight As Long = ServiceEP.GetActiveUnitsWeightSum(dtoEP.Id)
            'Dim e As Long = ServiceEP.GetPathWeight(dtoEP.Id)
            If Not IsEditablePath Then
                IsInReadOnlyMode = True
                DisplayPathStatus(status, PreloadIsForReadOnly, dtoEP.IsMooc, dtoEP.Weight, autoWeight)
            Else
                ShowSwitchMessage = True
                UpdateDurationMessage(dtoEP.Weight, autoWeight)
            End If

            'se non ci sono unità, va automaticamente alla creazione della prim
            If dtoEP.Units.Count > 0 Then
                'ECA.InitializeControl(Me.CurrentCommunityID, Me.CurrentPathId, 1, 1)

                If isAutoEp Then
                    _canAddActivityRules = ServiceEP.CanAddActivityRule(dtoEP.Units.SelectMany(Function(x) x.Activities).ToList()) ' Not dtoEP.IsMooc AndAlso ServiceEP.CanAddActivityRule(dtoEP.Units.SelectMany(Function(x) x.Activities).ToList())
                End If

                PlayerCssVisibility = "hidePlayer"
                SPANactivitiesInProgress.Visible = False
                Master.ServiceTitle = dtoEP.Name

                If dtoEP.PermissionEP.Create Then
                    Me.HYPnewUnit.Visible = IsEditablePath
                    HYPnewUnit.NavigateUrl = Me.BaseUrl & RootObject.NewUnit(Me.CurrentCommunityID, Me.CurrentPathId, IsMoocPath) 'path= al redirectToURL nell'else
                    Me.HYPnewUnitNote.Visible = dtoEP.PermissionEP.Create
                    Me.HYPnewUnitNote.NavigateUrl = Me.BaseUrl & RootObject.AddTextItem(Me.CurrentPathId, ItemType.Unit, CurrentCommunityID, IsMoocPath, IsInReadOnlyMode)
                End If

                Me.HYPvalidateByAct.Visible = True
                Me.HYPvalidateByAct.NavigateUrl = Me.BaseUrl & RootObject.Validate(CurrentPathId, CurrentCommunityID, ItemType.Activity, IsMoocPath, IsInReadOnlyMode)

                Me.HYPlistEduPath.Visible = ViewModeType = EpViewModeType.Manage OrElse (ServiceEP.GetEduPathCountInCommunity(Me.CurrentCommunityID, False) > 1)
                Me.HYPlistEduPath.NavigateUrl = Me.BaseUrl & RootObject.EduPathList(Me.CurrentCommunityID, Me.ViewModeType, dtoEP.IsMooc)
                Me.PermissionOverPath = dtoEP.PermissionEP
                Me.InitProgressBar(False)
                Me.InitDialog()
                _canAddUnitRules = ServiceEP.CanAddUnitRule(dtoEP.Units)
                _unitIndex = 1
                RPunit.DataSource = dtoEP.Units
                RPunit.DataBind()
                If ServiceEP.CheckEpType(PathType, EPType.TimeAuto) Then
                    hideControl(DIVmarkSlider)
                    hideControl(DIVruleSlider)
                    TXBcompleteLow.Text = 100
                End If
                DVconfirmEditing.Visible = False
                If Not PreloadIsForReadOnly AndAlso status <> PathAvailability.Blocked Then
                    If dtoEP.IsMooc OrElse status = PathAvailability.Available Then
                        DVconfirmEditing.Visible = True
                        SetConfirmDialogTranslations(dtoEP.Name, status, dtoEP.IsMooc)
                    End If
                End If
            ElseIf (dtoEP.PermissionEP.Create) Then
                Me.RedirectToUrl(RootObject.NewUnit(Me.CurrentCommunityID, Me.CurrentPathId, IsMoocPath)) 'path= a HYPnewUnit.NavigateUrl
            Else
                RedirectToUrl(RootObject.EduPathList(CurrentCommunityID, EpViewModeType.View, dtoEP.IsMooc))
                'initPageView()
            End If
        Else
            Me.ShowError(EpError.PathNotFind)
        End If
    End Sub

    Private Sub SetConfirmDialogTranslations(pathName As String, ByVal status As PathAvailability, ByVal isMooc As Boolean)
        Dim basekey As String = GetKeyString("", status, isMooc)

        LBeditingOptions.Text = String.Format(Resource.getValue("LBeditingOptions" & basekey), pathName)
        BTNreadonlyOption.Text = Resource.getValue("BTNreadonlyOption.text")
        BTNreadonlyOption.ToolTip = Resource.getValue("BTNreadonlyOption.ToolTip.IsMooc." & isMooc.ToString)
        LBreadonlyOption.Text = Resource.getValue("LBreadonlyOption.IsMooc." & isMooc.ToString)
        BTNeditOption.Text = Resource.getValue("BTNeditOption.text")
        BTNeditOption.ToolTip = Resource.getValue("BTNeditOption.ToolTip.IsMooc." & isMooc.ToString)
        BTNeditOption.CommandArgument = CInt(status)
        LBeditOption.Text = Resource.getValue("LBeditOption" & basekey)
  
    End Sub

    Private Sub DisplayPathStatus(ByVal status As PathAvailability, ByVal isForReadonly As Boolean, ByVal isMooc As Boolean, pathWeight As Long, pathAutoWeight As Long)
        Dim messages As New List(Of lm.Comol.Core.DomainModel.Helpers.dtoMessage)
        Select Case VerifyDuration(pathWeight, pathAutoWeight)
            Case TimeError.autoLessThenPath
                messages.Add(New lm.Comol.Core.DomainModel.Helpers.dtoMessage() With {.Text = GetDurationInfoMessage(pathWeight, pathAutoWeight, TimeError.autoLessThenPath), .Type = Helpers.MessageType.alert})
            Case TimeError.autoOverThenPath
                messages.Add(New lm.Comol.Core.DomainModel.Helpers.dtoMessage() With {.Text = GetDurationInfoMessage(pathWeight, pathAutoWeight, TimeError.autoOverThenPath), .Type = Helpers.MessageType.alert})
        End Select

        CTRLgenericMessage.Visible = True
        Select Case status
            Case PathAvailability.Blocked
                If messages.Any Then
                    CTRLgenericMessage.InitializeControl(messages)
                Else
                    CTRLgenericMessage.Visible = False
                End If
            Case PathAvailability.WithMyStatistics
                messages.Insert(0, New lm.Comol.Core.DomainModel.Helpers.dtoMessage() With {.Text = Resource.getValue(GetKeyString("Error", status, isMooc)), .Type = Helpers.MessageType.alert})
                CTRLgenericMessage.InitializeControl(messages)
            Case Else
                messages.Insert(0, New lm.Comol.Core.DomainModel.Helpers.dtoMessage() With {.Text = Resource.getValue(GetKeyString("Error", status, isMooc)), .Type = Helpers.MessageType.error})
                CTRLgenericMessage.InitializeControl(messages)
        End Select
    End Sub
    Private Sub UpdateDurationMessage(pathWeight As Long, pathAutoWeight As Long)
        Select Case VerifyDuration(pathWeight, pathAutoWeight)
            Case TimeError.autoLessThenPath
                CTRLgenericMessage.InitializeControl(GetDurationInfoMessage(pathWeight, pathAutoWeight, TimeError.autoLessThenPath), Helpers.MessageType.alert)
                CTRLgenericMessage.Visible = True
            Case TimeError.autoOverThenPath
                CTRLgenericMessage.Visible = True
                CTRLgenericMessage.InitializeControl(GetDurationInfoMessage(pathWeight, pathAutoWeight, TimeError.autoOverThenPath), Helpers.MessageType.error)
            Case Else
                CTRLgenericMessage.Visible = False
        End Select
    End Sub

    Private Function GetDurationInfoMessage(pathWeight As Long, pathAutoWeight As Long, er As TimeError) As String
        Dim message As String = Resource.getValue("DurationMessage.TimeError." & er.ToString)
        Dim hours As Long = 0
        Dim minutes As Long = 0
        Select Case er
            Case TimeError.autoLessThenPath
                hours = New DateTime().AddMinutes(pathWeight - pathAutoWeight).Hour
                minutes = New DateTime().AddMinutes(pathWeight - pathAutoWeight).Minute

                message = String.Format(message, GetPathDurationToString(pathAutoWeight), GetPathDurationToString(hours, minutes), GetPathDurationToString(pathWeight))
            Case TimeError.autoOverThenPath
                hours = New DateTime().AddMinutes(pathAutoWeight - pathWeight).Hour
                minutes = New DateTime().AddMinutes(pathAutoWeight - pathWeight).Minute

                message = String.Format(message, GetPathDurationToString(pathAutoWeight), GetPathDurationToString(hours, minutes), GetPathDurationToString(pathWeight))
            Case Else
                message = ""
        End Select
        Return message
    End Function

    Private Function GetPathDurationToString(weight) As String
        Dim hours As Long = 0
        Dim minutes As Long = 0
        hours = New DateTime().AddMinutes(weight).Hour
        minutes = New DateTime().AddMinutes(weight).Minute

        Return GetPathDurationToString(hours, minutes)
    End Function
    Private Function GetPathDurationToString(hours As Long, minutes As Long) As String
        Dim key As String = "GetPathDurationToString."
        Select Case hours
            Case 1
                key &= "1."
            Case 0
            Case Else
                key &= "h."
        End Select
        Select Case minutes
            Case 1
                key &= "1"
            Case 0
            Case Else
                key &= "m"
        End Select
        Dim message As String = Resource.getValue(key)
        Select Case hours
            Case 1, 0
            Case Else
                message = Replace(message, "#hour#", hours)
        End Select
        Select Case minutes
            Case 1, 0
            Case Else
                message = Replace(message, "#minutes#", minutes)
        End Select
        Return message
    End Function

    Private Function GetKeyString(prefix As String, ByVal status As PathAvailability, ByVal isMooc As Boolean)
        Dim key As String = prefix
        If Not key.EndsWith(".") Then
            key &= "."
        End If
        key &= "PathAvailability."
        Select Case status
            Case PathAvailability.Available, PathAvailability.UnknownItem, PathAvailability.WithOtherUserStatistics, PathAvailability.WithMyStatistics, PathAvailability.Blocked
                key &= status.ToString()
            Case (PathAvailability.Available Or PathAvailability.WithOtherUserStatistics)
                key &= "Available." & PathAvailability.WithOtherUserStatistics.ToString
            Case (PathAvailability.Available Or PathAvailability.WithMyStatistics)
                key &= "Available." & PathAvailability.WithMyStatistics.ToString
            Case (PathAvailability.Blocked Or PathAvailability.WithOtherUserStatistics)
                key &= PathAvailability.WithOtherUserStatistics.ToString
            Case (PathAvailability.Blocked Or PathAvailability.WithMyStatistics)
                key &= PathAvailability.WithMyStatistics.ToString
        End Select

        key &= ".IsMooc." & isMooc.ToString
        Return key
    End Function
    Private Sub ShowError(ByVal errorType As EpError)
        Resource.setHyperLink(Me.HYPerror, False, True)
        HYPerror.NavigateUrl = Me.BaseUrl & RootObject.EduPathList(Me.CurrentCommunityID, Me.ViewModeType, IsMoocPath)
        Select Case errorType
            Case EpError.Generic
                CTRLerrorMessage.InitializeControl(Resource.getValue("Error." & errorType.ToString), Helpers.MessageType.error)
                Me.PageUtility.AddAction(Services_EduPath.ActionType.GenericError, Nothing, InteractionType.UserWithLearningObject)
            Case EpError.NotPermission
                CTRLerrorMessage.InitializeControl(Resource.getValue("Error." & errorType.ToString), Helpers.MessageType.alert)
                Me.PageUtility.AddAction(Services_EduPath.ActionType.NoPermission, Nothing, InteractionType.UserWithLearningObject)
            Case EpError.Url
                CTRLerrorMessage.InitializeControl(Resource.getValue("Error." & errorType.ToString), Helpers.MessageType.alert)
                Me.PageUtility.AddAction(Services_EduPath.ActionType.GenericError, Nothing, InteractionType.UserWithLearningObject)
            Case EpError.PathNotFind
                CTRLerrorMessage.InitializeControl(Resource.getValue("Error." & errorType.ToString), Helpers.MessageType.error)
                Me.PageUtility.AddAction(Services_EduPath.ActionType.GenericError, Nothing, InteractionType.UserWithLearningObject)
        End Select
        MLVeduPathView.SetActiveView(VIWerror)
    End Sub
#Region "Repeater Item Data Bound"
    Public Sub RPrule_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs)
        Try
            Dim id As Long = Long.Parse(e.CommandArgument)
            ViewState("CurrentEditRule") = id

            Select Case e.CommandName
                Case "ruleDelete"

                    'If ServiceEP.SetPathVisibility(e.CommandArgument, Me.CurrentCommunityID, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then
                    '    Me.PageUtility.RedirectToUrl("Modules/EduPath/EduPathList.aspx?ComId=" & Me.CurrentCommunityID)
                    'Else
                    '    ShowError(EpError.Generic)
                    'End If

                    ServiceEP.DeleteActivityRule(id)
                    BindDati()
                    Me.UDPaddRule.Update()
                Case "ruleEdit"

                    LNBcreateUnitRule.Visible = False
                    LNBcreateActivityRule.Visible = True

                    'TXBlowerBound.Text = ""
                    'TXBupperBound.Text = ""
                    'TXBlowerThan.Text = ""
                    'TXBgreaterThan.Text = ""

                    TXBcompleteLow.Text = ""
                    TXBcompleteUp.Text = ""
                    TXBmarkLow.Text = ""
                    TXBmarkUp.Text = ""

                    Dim Rule As RuleActivityCompletion = ServiceEP.GetActivityRuleById(id)

                    Dim UnitId As Long = ServiceEP.GetUnitId_ByActivityId(Rule.DestinationId)


                    Dim ActivityId As Long = Rule.DestinationId

                    tempActivityRule = Rule.SourceId

                    Dim pathId As Long = ServiceEP.GetPathId_ByUnitId(UnitId)

                    Dim script As String = String.Format("$(document).ready(function () {2}showDialogSender('{0}','{1}');{3});", "addRule", UnitId.ToString + ";" + ActivityId.ToString, "{", "}")

                    'Dim List As List(Of dtoGenericAssignmentWithOldRoleEP) = ServiceEP.GetListPersonUnitAssignment(UnitId, pathId)

                    'Dim selectedUnit As Unit = ServiceEP.GetUnit(UnitId)

                    HIDact1.Value = Rule.DestinationId.ToString
                    HIDact2.Value = Rule.SourceId.ToString

                    Dim Rulelist As New List(Of dtoRule)

                    Rulelist = ServiceEP.GetActivityDtoRuleByActivityId(ActivityId)

                    'Rulelist.Add("Rule1")
                    'Rulelist.Add("Rule2")
                    'Rulelist.Add("Rule3")
                    Me.RPshownRules.DataSource = Rulelist
                    Me.RPshownRules.DataBind()

                    Dim Activitylist As List(Of dtoGenericItem)

                    'TODO: [PF] to avoid branching filter here the activities that are Source in some rules

                    If isAutoEp Then
                        Activitylist = ServiceEP.GetGenericDtoActivitiesByPathId(pathId)
                    Else
                        Activitylist = ServiceEP.GetGenericDtoActivitiesByUnitId(UnitId)
                    End If


                    'Activitylist = ServiceEP.GetDtoActivitiesByUnitIdFilteredByRules(UnitId)

                    'Me.RPactivity1.DataSource = Activitylist
                    'Me.RPactivity1.DataBind()
                    Dim activity1Name As String = (From item In Activitylist Where item.Id = ActivityId Select item.Name).Single()
                    LITactivity1.Text = activity1Name

                    '.Text = activity1Name
                    'LITactivity1b.Text = activity1Name
                    LITexplanation.Text = String.Format(LITexplanation.Text, activity1Name)
                    LITactivity1c.Text = String.Format(LITactivity1c.Text, activity1Name)

                    'Select Case Rule.RangeType
                    '    Case RuleRangeType.Between
                    '        TXBlowerBound.Text = Rule.MinValue
                    '        TXBupperBound.Text = Rule.MaxValue
                    '        RBrangeTypeBT.Checked = True
                    '        RBrangeTypeGT.Checked = False
                    '        RBrangeTypeLT.Checked = False
                    '    Case RuleRangeType.GreaterThan
                    '        TXBgreaterThan.Text = Rule.MinValue
                    '        RBrangeTypeBT.Checked = False
                    '        RBrangeTypeGT.Checked = True
                    '        RBrangeTypeLT.Checked = False
                    '    Case RuleRangeType.LowerThan
                    '        TXBlowerThan.Text = Rule.MaxValue
                    '        RBrangeTypeBT.Checked = False
                    '        RBrangeTypeGT.Checked = False
                    '        RBrangeTypeLT.Checked = True
                    'End Select

                    TXBcompleteLow.Text = Rule.CompletionMinValue
                    TXBcompleteUp.Text = Rule.CompletionMaxValue
                    TXBmarkLow.Text = Rule.MarkMinValue
                    TXBmarkUp.Text = Rule.MarkMaxValue

                    Dim q As List(Of dtoGenericItem) = (From oAct In Activitylist Select oAct Where Not oAct.Id = ActivityId AndAlso Not ServiceEP.CheckStatus(oAct.Status, Status.Text)).ToList
                    '............

                    Dim realRuleList As List(Of RuleActivityCompletion)

                    realRuleList = ServiceEP.GetActivityRuleByActivityId(ActivityId)

                    Dim realRuleIds As List(Of Long)
                    realRuleIds = (From item In realRuleList Select item.SourceId).ToList()

                    realRuleIds.Remove(Rule.SourceId)

                    Dim q1 As List(Of dtoGenericItem) = (From item In q Where Not realRuleIds.Contains(item.Id) Select item).ToList

                    Me.RPactivity2.DataSource = q1
                    Me.RPactivity2.DataBind()

                    Me.HIDact1.Value = ActivityId

                    ScriptManager.RegisterClientScriptBlock(MyBase.Page, Me.GetType, Me.UniqueID, script, True)
                    Me.UDPaddRule.Update()
            End Select
        Catch ex As Exception

        End Try
    End Sub

    Private Sub BTNswitch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNswitch.Click
        If Me.ServiceEP.DeleteActivityRules_byActivityId(BTNswitch.CommandArgument) Then

            If BTNswitch.CommandName = "moveDownA" Then
                MoveDownAct(BTNswitch.CommandArgument)
            ElseIf BTNswitch.CommandName = "moveUpA" Then
                MoveUpAct(BTNswitch.CommandArgument)
            End If

        Else
            Me.ShowError(EpError.Generic)

        End If


    End Sub

    Private Sub MoveDownAct(ByRef actId)
        If ServiceEP.MoveActivityDisplayOrderAfter(actId, Me.CurrentUserId, Me.IdCommunityRole, IsEditablePath, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then
            RedirectToUrl(RootObject.PathView(Me.CurrentPathId, Me.CurrentCommunityID, EpViewModeType.Manage, False, IsMoocPath, IsInReadOnlyMode))
        Else
            ShowError(EpError.Generic)
        End If
    End Sub

    Private Sub MoveUpAct(ByRef actId)
        If ServiceEP.MoveActivityDisplayOrderBefore(actId, Me.CurrentUserId, Me.IdCommunityRole, IsEditablePath, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then
            RedirectToUrl(RootObject.PathView(Me.CurrentPathId, Me.CurrentCommunityID, EpViewModeType.Manage, False, IsMoocPath, IsInReadOnlyMode))
        Else
            ShowError(EpError.Generic)
        End If
    End Sub

    Public Sub RPactivity_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs)
        Try
            Select Case e.CommandName
                Case "mandatory"
                    If ServiceEP.UpdateActivityMandatoryStatus(e.CommandArgument, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then
                        Me.PageUtility.RedirectToUrl(RootObject.PathView(Me.CurrentPathId, Me.CurrentCommunityID, Me.ViewModeType, False, IsMoocPath, IsInReadOnlyMode))
                    Else
                        ShowError(EpError.Generic)
                    End If
                Case "visibility"
                    If ServiceEP.UpdateActivityVisibilityStatus(e.CommandArgument, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then
                        Me.PageUtility.RedirectToUrl(RootObject.PathView(Me.CurrentPathId, Me.CurrentCommunityID, Me.ViewModeType, False, IsMoocPath, IsInReadOnlyMode))
                    Else
                        ShowError(EpError.Generic)
                    End If

                Case "activityDelete"
                    If ServiceEP.VirtualDeleteAllActivity(e.CommandArgument, isAutoEp, Me.CurrentCommunityID, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then
                        Me.PageUtility.RedirectToUrl(RootObject.PathView(Me.CurrentPathId, Me.CurrentCommunityID, Me.ViewModeType, False, IsMoocPath, IsInReadOnlyMode))
                    Else
                        ShowError(EpError.Generic)
                    End If

                Case "moveUpA"
                    If IsEditablePath AndAlso ServiceEP.IsFirstDisplayedActivity(e.CommandArgument) Then

                        SubSwitchManageMessage(e.CommandArgument, e.CommandName)

                    Else
                        MoveUpAct(e.CommandArgument)
                    End If

                Case "moveDownA"
                    If IsEditablePath AndAlso ServiceEP.IsLastDisplayedActivity(e.CommandArgument) Then

                        SubSwitchManageMessage(e.CommandArgument, e.CommandName)

                    Else
                        MoveDownAct(e.CommandArgument)
                    End If

                Case "addActivityRule"
                    Me.Resource.setLiteral(LITactivity3)
                    LNBcreateActivityRule.Visible = True
                    LNBcreateUnitRule.Visible = False

                    Dim script As String = String.Format("$(document).ready(function () {2}showDialogSender('{0}','{1}');{3});", "addRule", e.CommandArgument, "{", "}")

                    Dim values As String()

                    values = e.CommandArgument.ToString.Split(";")

                    Dim UnitId As Long = Long.Parse(values(0))
                    Dim ActivityId As Long = Long.Parse(values(1))
                    Dim pathId As Long = ServiceEP.GetPathId_ByUnitId(UnitId)
                    ViewState("CurrentEditRule") = 0
                    'Dim List As List(Of dtoGenericAssignmentWithOldRoleEP) = ServiceEP.GetListPersonUnitAssignment(UnitId, pathId)

                    'Dim selectedUnit As Unit = ServiceEP.GetUnit(UnitId)



                    Dim Rulelist As List(Of dtoRule)

                    Rulelist = ServiceEP.GetActivityDtoRuleByActivityId(ActivityId)

                    Dim realRuleList As List(Of RuleActivityCompletion)

                    realRuleList = ServiceEP.GetActivityRuleByActivityId(ActivityId)

                    Dim realRuleIds As List(Of Long)
                    realRuleIds = (From item In realRuleList Select item.SourceId).ToList()

                    'Rulelist.Add("Rule1")
                    'Rulelist.Add("Rule2")
                    'Rulelist.Add("Rule3")
                    Me.RPshownRules.DataSource = Rulelist
                    Me.RPshownRules.DataBind()

                    Dim Activitylist As List(Of dtoGenericItem)

                    If isAutoEp Then
                        Activitylist = ServiceEP.GetGenericDtoActivitiesByPathId(pathId)
                    Else
                        Activitylist = ServiceEP.GetGenericDtoActivitiesByUnitId(UnitId)
                    End If

                    'Me.RPactivity1.DataSource = Activitylist
                    'Me.RPactivity1.DataBind()


                    Dim activity1Name As String = (From item In Activitylist Where item.Id = ActivityId Select item.Name).Single()
                    LITactivity1.Text = activity1Name
                    'LITactivity1a.Text = LITactivity1.Text
                    'LITactivity1b.Text = LITactivity1.Text
                    LITexplanation.Text = String.Format(LITexplanation.Text, activity1Name)
                    LITactivity1c.Text = activity1Name

                    Dim q As List(Of dtoGenericItem) = (From oAct In Activitylist Select oAct Where Not oAct.Id = ActivityId AndAlso Not ServiceEP.CheckStatus(oAct.Status, Status.Text)).ToList


                    Dim q1 As List(Of dtoGenericItem) = (From item In q Where Not realRuleIds.Contains(item.Id) Select item).ToList

                    Me.RPactivity2.DataSource = q1
                    Me.RPactivity2.DataBind()

                    Me.HIDact1.Value = ActivityId

                    ScriptManager.RegisterClientScriptBlock(MyBase.Page, Me.GetType, Me.UniqueID, script, True)
                    Me.UDPaddRule.Update()
            End Select
        Catch ex As Exception

        End Try
    End Sub


    Public Sub SubSwitchManageMessage(ByVal actId As Long, ByVal action As String)
        If Not ShowSwitchMessage AndAlso ServiceEP.ExistRule_byActivityId(actId) Then
            BTNswitch.CommandArgument = actId
            BTNswitch.CommandName = action
            Dim script As String = String.Format("$(document).ready(function () {1}showDialog('{0}');{2});", "switchAct", "{", "}")
            Me.LBswitchCombo.Visible = False
            ScriptManager.RegisterClientScriptBlock(MyBase.Page, Me.GetType, Me.UniqueID, script, True)
            Me.UDPpath.Update()
        ElseIf ShowSwitchMessage AndAlso ServiceEP.ExistRule_byActivityId(actId) Then
            'show double message
            BTNswitch.CommandArgument = actId
            BTNswitch.CommandName = action
            Me.LBswitch.Visible = False
            ShowSwitchMessage = False
            Dim script As String = String.Format("$(document).ready(function () {1}showDialog('{0}');{2});", "switchAct", "{", "}")
            ScriptManager.RegisterClientScriptBlock(MyBase.Page, Me.GetType, Me.UniqueID, script, True)
            Me.UDPpath.Update()

        ElseIf ShowSwitchMessage Then
            'only first message
            ShowSwitchMessage = False
            Dim script As String = String.Format("$(document).ready(function () {1}showDialog('{0}');{2});", "switchMessage", "{", "}")

            ScriptManager.RegisterClientScriptBlock(MyBase.Page, Me.GetType, Me.UniqueID, script, True)
            Me.UDPpath.Update()
            SubSwitchManageMessage(actId, action)
        ElseIf action.Equals("moveUpA") Then
            MoveUpAct(actId)
        ElseIf action.Equals("moveDownA") Then
            MoveDownAct(actId)
        End If
    End Sub


    Public Sub RPunit_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPunit.ItemCommand
        Try
            Select Case e.CommandName
                Case "mandatory"
                    If ServiceEP.UpdateUnitMandatoryStatus(e.CommandArgument, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then
                        Me.PageUtility.RedirectToUrl(RootObject.PathView(Me.CurrentPathId, Me.CurrentCommunityID, Me.ViewModeType, False, IsMoocPath, IsInReadOnlyMode))
                    Else
                        ShowError(EpError.Generic)
                    End If
                Case "visibility"
                    If ServiceEP.UpdateUnitVisibilityStatus(e.CommandArgument, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then
                        Me.PageUtility.RedirectToUrl(RootObject.PathView(Me.CurrentPathId, Me.CurrentCommunityID, Me.ViewModeType, False, IsMoocPath, IsInReadOnlyMode))
                    Else
                        ShowError(EpError.Generic)
                    End If
                Case "unitDelete"
                    If ServiceEP.VirtualDeleteAllUnit(e.CommandArgument, Me.CurrentCommunityID, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then
                        Me.PageUtility.RedirectToUrl(RootObject.PathView(Me.CurrentPathId, Me.CurrentCommunityID, Me.ViewModeType, False, IsMoocPath, IsInReadOnlyMode))
                    Else
                        ShowError(EpError.Generic)
                    End If

                Case "moveUpU"
                    If ServiceEP.MoveUnitDisplayOrderBefore(e.CommandArgument, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then
                        RedirectToUrl(RootObject.PathView(Me.CurrentPathId, Me.CurrentCommunityID, EpViewModeType.Manage, False, IsMoocPath, IsInReadOnlyMode))
                    Else
                        ShowError(EpError.Generic)
                    End If

                Case "moveDownU"
                    If ServiceEP.MoveUnitDisplayOrderAfter(e.CommandArgument, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then
                        RedirectToUrl(RootObject.PathView(Me.CurrentPathId, Me.CurrentCommunityID, EpViewModeType.Manage, False, IsMoocPath, IsInReadOnlyMode))
                    Else
                        ShowError(EpError.Generic)
                    End If

                Case "addUnitRule"
                    Me.Resource.setLiteral(LITactivity3, "unit")
                    LNBcreateActivityRule.Visible = False
                    LNBcreateUnitRule.Visible = True
                    Dim script As String = String.Format("$(document).ready(function () {2}showDialogSender('{0}','{1}');{3});", "addRule", e.CommandArgument, "{", "}")

                    Dim values As String()

                    values = e.CommandArgument.ToString.Split(";")

                    Dim UnitId As Long = Long.Parse(values(0))


                    'Dim ActivityId As Long = Long.Parse(values(1))

                    Dim pathId As Long = ServiceEP.GetPathId_ByUnitId(UnitId)
                    ViewState("CurrentEditRule") = 0
                    'Dim List As List(Of dtoGenericAssignmentWithOldRoleEP) = ServiceEP.GetListPersonUnitAssignment(UnitId, pathId)

                    'Dim selectedUnit As Unit = ServiceEP.GetUnit(UnitId)



                    Dim Rulelist As List(Of dtoRule)

                    Rulelist = ServiceEP.GetUnitDtoRuleByUnitId(UnitId)

                    'Rulelist.Add("Rule1")
                    'Rulelist.Add("Rule2")
                    'Rulelist.Add("Rule3")
                    Me.RPshownRules.DataSource = Rulelist
                    Me.RPshownRules.DataBind()

                    Dim Unitlist As List(Of dtoGenericItem)

                    Unitlist = ServiceEP.GetGenericDtoUnitsByPathId(pathId)
                    'Me.RPactivity1.DataSource = Activitylist
                    'Me.RPactivity1.DataBind()


                    Dim activity1Name As String = (From item In Unitlist Where item.Id = UnitId Select item.Name).Single()
                    LITactivity1.Text = activity1Name
                    'LITactivity1a.Text = LITactivity1.Text
                    'LITactivity1b.Text = LITactivity1.Text
                    LITexplanation.Text = String.Format(LITexplanation.Text, activity1Name)
                    LITactivity1c.Text = LITactivity1.Text
                    'elimino se stesso dalla lista.
                    Me.RPactivity2.DataSource = (From unitEl In Unitlist Select unitEl Where Not unitEl.Id = UnitId AndAlso Not ServiceEP.CheckStatus(unitEl.Status, Status.Text)).ToList
                    Me.RPactivity2.DataBind()

                    Me.HIDact1.Value = UnitId

                    ScriptManager.RegisterClientScriptBlock(MyBase.Page, Me.GetType, Me.UniqueID, script, True)
                    Me.UDPaddRule.Update()
            End Select
        Catch ex As Exception

        End Try
    End Sub

    Public Sub RPunitrule_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs)
        Dim id As Long = Long.Parse(e.CommandArgument)
        ViewState("CurrentEditRule") = id
        Try
            Select Case e.CommandName
                Case "ruleDelete"
                    ServiceEP.DeleteUnitRule(id)
                    BindDati()
                    'Me.UDPaddRule.Update()
                Case "ruleEdit"
                    Dim Rule As RuleUnitCompletion = ServiceEP.GetUnitRuleById(id)

                    Dim UnitId As Long = Rule.DestinationId

                    LNBcreateUnitRule.Visible = True
                    LNBcreateActivityRule.Visible = False

                    'TXBlowerBound.Text = ""
                    'TXBupperBound.Text = ""
                    'TXBlowerThan.Text = ""
                    'TXBgreaterThan.Text = ""

                    TXBcompleteLow.Text = ""
                    TXBcompleteUp.Text = ""
                    TXBmarkLow.Text = ""
                    TXBmarkUp.Text = ""

                    'Dim ActivityId As Long = Rule.DestinationId

                    'tempActivityRule = Rule.SourceId

                    Dim pathId As Long = ServiceEP.GetPathId_ByUnitId(UnitId)

                    Dim script As String = String.Format("$(document).ready(function () {2}showDialogSender('{0}','{1}');{3});", "addRule", UnitId.ToString, "{", "}")

                    'Dim List As List(Of dtoGenericAssignmentWithOldRoleEP) = ServiceEP.GetListPersonUnitAssignment(UnitId, pathId)

                    'Dim selectedUnit As Unit = ServiceEP.GetUnit(UnitId)

                    HIDact1.Value = Rule.DestinationId.ToString
                    HIDact2.Value = Rule.SourceId.ToString

                    Dim Rulelist As New List(Of dtoRule)

                    Rulelist = ServiceEP.GetUnitDtoRuleByUnitId(UnitId)

                    'Rulelist.Add("Rule1")
                    'Rulelist.Add("Rule2")
                    'Rulelist.Add("Rule3")
                    Me.RPshownRules.DataSource = Rulelist
                    Me.RPshownRules.DataBind()

                    Dim Unitlist As List(Of dtoGenericItem)
                    'TODO: [PF] to avoid branching filter here the units that are Source in some rules
                    Unitlist = ServiceEP.GetGenericDtoUnitsByPathId(pathId)
                    'Unitlist = ServiceEP.GetDtoUnitsByPathIdFilteredByRules(pathId)

                    'Me.RPactivity1.DataSource = Activitylist
                    'Me.RPactivity1.DataBind()

                    Dim activity1Name As String = (From item In Unitlist Where item.Id = UnitId Select item.Name).Single()
                    LITactivity1.Text = activity1Name
                    'LITactivity1a.Text = LITactivity1.Text
                    'LITactivity1b.Text = LITactivity1.Text
                    LITexplanation.Text = String.Format(LITexplanation.Text, activity1Name)
                    LITactivity1c.Text = LITactivity1.Text

                    'Select Case Rule.RangeType
                    '    Case RuleRangeType.Between
                    '        TXBlowerBound.Text = Rule.MinValue
                    '        TXBupperBound.Text = Rule.MaxValue
                    '        RBrangeTypeBT.Checked = True
                    '        RBrangeTypeGT.Checked = False
                    '        RBrangeTypeLT.Checked = False
                    '    Case RuleRangeType.GreaterThan
                    '        TXBgreaterThan.Text = Rule.MinValue
                    '        RBrangeTypeBT.Checked = False
                    '        RBrangeTypeGT.Checked = True
                    '        RBrangeTypeLT.Checked = False
                    '    Case RuleRangeType.LowerThan
                    '        TXBlowerThan.Text = Rule.MaxValue
                    '        RBrangeTypeBT.Checked = False
                    '        RBrangeTypeGT.Checked = False
                    '        RBrangeTypeLT.Checked = True
                    'End Select

                    TXBcompleteLow.Text = Rule.CompletionMinValue
                    TXBcompleteUp.Text = Rule.CompletionMaxValue
                    TXBmarkLow.Text = Rule.MarkMinValue
                    TXBmarkUp.Text = Rule.MarkMaxValue
                    Me.RPactivity2.DataSource = (From oUnit In Unitlist Select oUnit Where Not oUnit.Id = UnitId AndAlso Not ServiceEP.CheckStatus(oUnit.Status, Status.Text)).ToList
                    Me.RPactivity2.DataBind()

                    Me.HIDact1.Value = UnitId

                    ScriptManager.RegisterClientScriptBlock(MyBase.Page, Me.GetType, Me.UniqueID, script, True)
                    Me.UDPaddRule.Update()
            End Select
        Catch ex As Exception
        End Try
    End Sub

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

    Public Function OpenUnit(ByVal UnitId As Long) As String
        If ViewModeType = EpViewModeType.Manage Then
            Return " current"
        Else


        End If
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

    Private Sub SetSwitchItemButton(ByVal ItemID As Long, ByVal type As String, ByRef e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        '  If ItemListCount > 1 OrElse (type = "A" AndAlso Me.UnitsCount > 1) Then
        If type = "U" Then
            DirectCast(e.Item.FindControl("LITmoveUnit"), Literal).Text = "<span class='button moveUnit'>M</span>"

        ElseIf type = "A" Then
            DirectCast(e.Item.FindControl("LITmoveActivity"), Literal).Text = "<span class='button moveActivity left13' >M</span>"

        End If

        Dim oLkb As LinkButton
        oLkb = e.Item.FindControl("LKBup" & type)
        Me.Resource.setLinkButton(oLkb, False, True)
        oLkb.CommandArgument = ItemID

        oLkb = e.Item.FindControl("LKBdown" & type)
        Me.Resource.setLinkButton(oLkb, False, True)
        oLkb.CommandArgument = ItemID

    End Sub

    Private Sub SetImgBlocked(ByRef oImg As System.Web.UI.WebControls.Image, ByVal isBlocked As Boolean)
        oImg.Visible = True
        oImg.ImageUrl = RootObject.ImgItemBlocked(Me.BaseUrl, Not isBlocked)
        oImg.AlternateText = Me.Resource.getValue("Blocked." & isBlocked)
    End Sub

    Private Sub SetBlockedUnitBtn(ByRef e As System.Web.UI.WebControls.RepeaterItemEventArgs, ByRef dtoItem As dtoUnit)
        Dim oDlkb As LinkButton
        oDlkb = e.Item.FindControl("LKBvisibUnit")
        Me.Resource.setLinkButton(oDlkb, False, True)
        oDlkb.Text = "<img src=" & """" & "{0}" & """" & " alt=" & """" & "{1}" & """" & "/>"
        If Not ServiceEP.CheckStatus(dtoItem.Status, Status.Locked) Then
            oDlkb.Text = String.Format(oDlkb.Text, RootObject.ImgBtnBlocked_On(Me.BaseUrl), oDlkb.ToolTip)
        Else
            oDlkb.Text = String.Format(oDlkb.Text, RootObject.ImgBtnBlocked_Off(Me.BaseUrl), oDlkb.ToolTip)
        End If
        oDlkb.CommandArgument = dtoItem.Id
        'oDlkb.DialogClass = "visibilityUnitDial"
        oDlkb.Visible = True
    End Sub

    Private Sub setUpdateUnitBtn(ByRef e As System.Web.UI.WebControls.RepeaterItemEventArgs, ByRef dtoItem As dtoUnit)
        Dim oHyp As HyperLink
        oHyp = e.Item.FindControl("HYPupdateUnit")
        Me.Resource.setHyperLink(oHyp, False, True)
        If ServiceEP.CheckStatus(dtoItem.Status, Status.Text) Then
            oHyp.NavigateUrl = Me.BaseUrl & RootObject.UpdateTextItem(dtoItem.Id, Me.CurrentPathId, ItemType.Unit, CurrentCommunityID, IsMoocPath, IsInReadOnlyMode)
        Else
            oHyp.NavigateUrl = Me.BaseUrl & RootObject.UpdateUnit(Me.CurrentCommunityID, dtoItem.Id, Me.CurrentPathId, IsMoocPath, IsInReadOnlyMode)

        End If

        oHyp.Visible = True
    End Sub

    Private Sub SetDeleteUnitBtn(ByRef e As System.Web.UI.WebControls.RepeaterItemEventArgs, ByRef dtoItem As dtoUnit)
        Dim oLkb As LinkButton
        oLkb = e.Item.FindControl("LKBdeleteUnit")
        oLkb.Visible = True
        oLkb.CommandArgument = dtoItem.Id
        Me.Resource.setLinkButton(oLkb, False, True, , True)
    End Sub

    Private Sub RPunitGeneric_ItemDataBound(ByRef e As System.Web.UI.WebControls.RepeaterItemEventArgs, ByRef dtoItem As dtoUnit)
        Dim oLkb As LinkButton
        Dim oHyp As HyperLink
        Dim LBunit As Label
        Dim RpActivities As Repeater
        Dim oDlkb As LinkButton

        Select Case Me.ViewModeType
            Case EpViewModeType.Manage
                Me.PermissionOverUnit = dtoItem.PermissionEP


                If Me.PermissionOverPath.Update Then
                    Me.SetSwitchItemButton(dtoItem.Id, "U", e)

                End If

                If dtoItem.PermissionEP.Update Then
                    SetBlockedUnitBtn(e, dtoItem)
                    setUpdateUnitBtn(e, dtoItem)

                    'rules for both type of edupath
                    'oLkb = e.Item.FindControl("LKBaddUnitRule")
                    'If _canAddUnitRules AndAlso Me.PermissionOverPath.ManageRule Then
                    '    Me.Resource.setLinkButton(oLkb, False, True)
                    '    oLkb.Text = "<img src=" & """" & "{0}" & """" & " alt=" & """" & "{1}" & """" & "/>"
                    '    oLkb.Text = String.Format(oLkb.Text, RootObject.ImgAddRule(Me.BaseUrl), oLkb.ToolTip)
                    '    oLkb.Visible = True
                    'Else
                    '    hideControl(oLkb)
                    'End If


                    If Not isAutoEp AndAlso IsEditablePath Then

                        oDlkb = e.Item.FindControl("LKBmandatory")
                        Me.Resource.setLinkButton(oDlkb, False, True)
                        oDlkb.CommandArgument = dtoItem.Id
                        'oDlkb.DialogClass = "mandatoryUnitDial"
                        oDlkb.Text = "<img src=" & """" & "{0}" & """" & " alt=" & """" & "{1}" & """" & "/>"
                        oDlkb.Visible = True
                        If ServiceEP.CheckStatus(dtoItem.Status, Status.Mandatory) Then
                            oDlkb.Text = String.Format(oDlkb.Text, RootObject.ImgRemoveMandatoryMedium(Me.BaseUrl), oDlkb.ToolTip)
                        Else
                            oDlkb.Text = String.Format(oDlkb.Text, RootObject.ImgSetMandatoryMedium(Me.BaseUrl), oDlkb.ToolTip)
                        End If



                        oLkb = e.Item.FindControl("LKBaddUnitRule")
                        If _canAddUnitRules AndAlso Me.PermissionOverPath.ManageRule Then
                            Me.Resource.setLinkButton(oLkb, False, True)
                            oLkb.Text = "<img src=" & """" & "{0}" & """" & " alt=" & """" & "{1}" & """" & "/>"
                            oLkb.Text = String.Format(oLkb.Text, RootObject.ImgAddRule(Me.BaseUrl), oLkb.ToolTip)
                            oLkb.Visible = True
                        Else
                            hideControl(oLkb)
                        End If


                    ElseIf Not IsEditablePath Then

                        Dim oImg As System.Web.UI.WebControls.Image
                        oImg = e.Item.FindControl("IMGmandatory")
                        oImg.Visible = True

                        If ServiceEP.CheckStatus(dtoItem.Status, Status.Mandatory) Then
                            oImg.ImageUrl = RootObject.ImgMandatoryMedium(Me.BaseUrl)
                            oImg.ToolTip = Me.Resource.getValue("UnitMandatory")
                        Else
                            oImg.ImageUrl = RootObject.ImgBlindMedium(Me.BaseUrl)
                        End If

                    ElseIf isAutoEp Then

                        Dim oImg As System.Web.UI.WebControls.Image
                        oImg = e.Item.FindControl("IMGmandatory")
                        oImg.Visible = True
                        oImg.ImageUrl = RootObject.ImgBlindMedium(Me.BaseUrl)
                    End If


                End If

                If dtoItem.PermissionEP.Delete AndAlso IsEditablePath Then
                    SetDeleteUnitBtn(e, dtoItem)
                End If

                If dtoItem.PermissionEP.Create AndAlso IsEditablePath Then
                    oHyp = e.Item.FindControl("HYPnewAct")
                    Me.Resource.setHyperLink(oHyp, False, True)
                    oHyp.NavigateUrl = Me.BaseUrl & RootObject.NewActivity(Me.CurrentCommunityID, dtoItem.Id, IsMoocPath)
                    oHyp.Visible = True
                    oHyp = e.Item.FindControl("HYPnewActNote")
                    Me.Resource.setHyperLink(oHyp, False, True)
                    oHyp.NavigateUrl = Me.BaseUrl & RootObject.AddTextItem(dtoItem.Id, ItemType.Activity, CurrentCommunityID, IsMoocPath, IsInReadOnlyMode)
                    oHyp.Visible = True
                End If

                If dtoItem.PermissionEP.Create Then
                    oHyp = e.Item.FindControl("HYPnewActNote")
                    Me.Resource.setHyperLink(oHyp, False, True)
                    oHyp.NavigateUrl = Me.BaseUrl & RootObject.AddTextItem(dtoItem.Id, ItemType.Activity, CurrentCommunityID, IsMoocPath, IsInReadOnlyMode)
                    oHyp.Visible = True
                End If


                If Not isAutoEp Then
                    _canAddActivityRules = Not IsMoocPath AndAlso ServiceEP.CanAddActivityRule(dtoItem.Activities)
                End If


            Case EpViewModeType.View

                'If ServiceStat.UnitIsCompleted(dtoItem.Activities) Then
                '    Dim li As HtmlControl = e.Item.FindControl("UNIT_" & dtoItem.Id)
                '    li.Attributes.Add("class", "unit")
                'End If

                If Not isAutoEp Then
                    SetImgStatusMedium(e.Item.FindControl("IMGstatus"), dtoItem.statusStat)
                End If

                Dim oImg As System.Web.UI.WebControls.Image

                oImg = e.Item.FindControl("IMGvisibility")
                SetImgBlocked(oImg, ServiceEP.CheckStatus(dtoItem.Status, Status.Locked))

                oImg = e.Item.FindControl("IMGmandatory")
                If ServiceEP.CheckStatus(dtoItem.Status, Status.Mandatory) Then
                    oImg.ImageUrl = RootObject.ImgMandatoryMedium(Me.BaseUrl)
                    oImg.ToolTip = Me.Resource.getValue("UnitMandatory")
                    oImg.Visible = True
                    'oImg.CssClass = "indent-30"
                    'Dim oDiv As HtmlGenericControl
                    'oDiv = e.Item.FindControl("DIVunitName")
                    'oDiv.Attributes.Add("class", "indent-30 title")
                Else
                    oImg.ImageUrl = RootObject.ImgBlindMedium(Me.BaseUrl)
                    oImg.Visible = True
                End If
        End Select

        LBunit = e.Item.FindControl("LBunitNumber")
        LBunit.Visible = True
        LBunit.Text = Me.Resource.getValue("Unit") & " " & _unitIndex & ": "
        _unitIndex = _unitIndex + 1
        LBunit = e.Item.FindControl("LBunit")
        LBunit.Visible = True
        LBunit.Text = dtoItem.Name

        LBunit = e.Item.FindControl("LBunitWeight")

        If isMarkEp Then
            LBunit.Text = " (" & dtoItem.Weight & Resource.getValue("Points")
        Else
            LBunit.Text = ServiceEP.GetTime(dtoItem.Weight)
        End If

        LBunit = e.Item.FindControl("LBunitDesc")
        If Not String.IsNullOrWhiteSpace(dtoItem.Description) Then
            Dim div As HtmlControl = e.Item.FindControl("DIVunitDescription")
            div.Visible = True
            LBunit.Text = SmartTagsAvailable.TagAll(dtoItem.Description)
        Else
            hideControl(LBunit)
        End If

        Me.TempUnitId = dtoItem.Id
        RpActivities = e.Item.FindControl("RPactivity")

        If dtoItem.Activities.Count = 0 Then
            Dim oSpSwitch As HtmlControl = e.Item.FindControl("SPswitch")
            oSpSwitch.Attributes.Add("class", "button switch disabled")
        End If

        'from rule

        If VisibleActivity Is Nothing Then
            RpActivities.DataSource = dtoItem.Activities
        Else
            RpActivities.DataSource = (From item In dtoItem.Activities Where VisibleActivity.Contains(item.Id)).ToList()
        End If



        _actIndex = 1
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
                Dim oImg As System.Web.UI.WebControls.Image
                oImg = e.Item.FindControl("IMGmandatory")
                oImg.ImageUrl = RootObject.ImgTextNoteItem(Me.BaseUrl)
                oImg.ToolTip = Resource.getValue("TextNote")
                oImg.AlternateText = Resource.getValue("TextNote")
                oImg.Visible = True
                Dim oLb As Label = e.Item.FindControl("LBdescription")
                If Not String.IsNullOrWhiteSpace(dtoItem.Description) Then
                    oLb.Visible = True
                    oLb.Text = SmartTagsAvailable.TagAll(dtoItem.Description)
                Else
                    hideControl(LBdescription)
                End If

                Dim oSpSwitch As HtmlControl = e.Item.FindControl("SPswitch")
                oSpSwitch.Attributes.Add("class", "button switch disabled")

                If ViewModeType = EpViewModeType.Manage Then
                    If Me.PermissionOverPath.Update Then
                        SetSwitchItemButton(dtoItem.Id, "U", e)
                    End If
                    ' SetBlockedUnitBtn(e, dtoItem)

                    If dtoItem.PermissionEP.Update Then
                        setUpdateUnitBtn(e, dtoItem)
                        SetBlockedUnitBtn(e, dtoItem)
                    End If

                    If dtoItem.PermissionEP.Delete AndAlso IsEditablePath Then
                        SetDeleteUnitBtn(e, dtoItem)
                    End If
                End If

                Dim ulRule As HtmlControl = e.Item.FindControl("ULunitRules")
                ulRule.Visible = False
                hideControl(ulRule)

            Else

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

            Dim oLkb As LinkButton
            oLkb = e.Item.FindControl("LKBunitDeleteRule")
            oLkb.Visible = (ViewModeType = EpViewModeType.Manage)
            oLkb.CommandArgument = dtoItem.Id
            Me.Resource.setLinkButtonForName(oLkb, "LKBdeleteRule", False, True, , True)

            oLkb = e.Item.FindControl("LKBunitEditRule")
            oLkb.Visible = (ViewModeType = EpViewModeType.Manage)
            oLkb.CommandArgument = dtoItem.Id
            Me.Resource.setLinkButtonForName(oLkb, "LKBeditRule", False, True)


        End If
    End Sub

    Private Sub SetBlockedActivityBtn(ByRef e As System.Web.UI.WebControls.RepeaterItemEventArgs, ByRef dtoItem As dtoActivity)
        Dim oDlkb As LinkButton
        oDlkb = e.Item.FindControl("LKBvisibActivity")
        Me.Resource.setLinkButton(oDlkb, False, True)
        oDlkb.Text = "<img src=" & """" & "{0}" & """" & " alt=" & """" & "{1}" & """" & "/>"
        If Not ServiceEP.CheckStatus(dtoItem.Status, Status.Locked) Then
            oDlkb.Text = String.Format(oDlkb.Text, RootObject.ImgBtnBlocked_On(Me.BaseUrl), oDlkb.ToolTip)
        Else
            oDlkb.Text = String.Format(oDlkb.Text, RootObject.ImgBtnBlocked_Off(Me.BaseUrl), oDlkb.ToolTip)
        End If
        oDlkb.CommandArgument = dtoItem.Id
        'oDlkb.DialogClass = "visibilityActDial"
        oDlkb.Visible = True
    End Sub

    Private Sub setUpdateActivityBtn(ByRef e As System.Web.UI.WebControls.RepeaterItemEventArgs, ByRef dtoItem As dtoActivity)
        Dim oHyp As HyperLink
        oHyp = e.Item.FindControl("HYPupdateAct")
        Me.Resource.setHyperLink(oHyp, False, True)
        If ServiceEP.CheckStatus(dtoItem.Status, Status.Text) Then
            oHyp.NavigateUrl = Me.BaseUrl & RootObject.UpdateTextItem(dtoItem.Id, _unitId, ItemType.Activity, CurrentCommunityID, IsMoocPath, IsInReadOnlyMode)
        Else
            oHyp.NavigateUrl = Me.BaseUrl & RootObject.UpdateActivity(Me.CurrentCommunityID, dtoItem.Id, Me.TempUnitId, IsMoocPath, IsInReadOnlyMode)
        End If
        oHyp.Visible = True
    End Sub

    Private Sub SetDeleteActivityBtn(ByRef e As System.Web.UI.WebControls.RepeaterItemEventArgs, ByRef dtoItem As dtoActivity)
        Dim oLkb As LinkButton
        oLkb = e.Item.FindControl("LKBdeleteAct")
        oLkb.Visible = True
        oLkb.CommandArgument = dtoItem.Id
        Me.Resource.setLinkButton(oLkb, False, True, , True)
        oLkb.Visible = dtoItem.PermissionEP.Delete
    End Sub

    Private Sub RPactivityGeneric_ItemDataBound(ByRef e As System.Web.UI.WebControls.RepeaterItemEventArgs, ByRef dtoItem As dtoActivity)
        Dim oLkb As LinkButton
        Dim oHyp As HyperLink
        Dim oLb As Label
        Dim RpRule As Repeater
        Dim oDlkb As LinkButton

        Select Case Me.ViewModeType
            Case EpViewModeType.Manage

                If Me.PermissionOverUnit.Update Then
                    Me.SetSwitchItemButton(dtoItem.Id, "A", e)

                End If

                If dtoItem.PermissionEP.Update Then

                    SetBlockedActivityBtn(e, dtoItem)

                    If IsEditablePath Then
                        oDlkb = e.Item.FindControl("LKBmandatory")
                        Me.Resource.setLinkButton(oDlkb, False, True)
                        oDlkb.CommandArgument = dtoItem.Id
                        'oDlkb.DialogClass = "mandatoryActDial"
                        oDlkb.Text = "<img src=" & """" & "{0}" & """" & " alt=" & """" & "{1}" & """" & "/>"
                        oDlkb.Visible = True
                        If ServiceEP.CheckStatus(dtoItem.Status, Status.Mandatory) Then
                            oDlkb.Text = String.Format(oDlkb.Text, RootObject.ImgRemoveMandatorySmall(Me.BaseUrl), oDlkb.ToolTip)
                        Else
                            oDlkb.Text = String.Format(oDlkb.Text, RootObject.ImgSetMandatorySmall(Me.BaseUrl), oDlkb.ToolTip)
                        End If

                        setUpdateActivityBtn(e, dtoItem)

                        oLkb = e.Item.FindControl("LKBaddActivityRule")
                        If _canAddActivityRules AndAlso Me.PermissionOverUnit.ManageRule Then
                            oLkb.CommandArgument = Me.TempUnitId.ToString + ";" + dtoItem.Id.ToString
                            Me.Resource.setLinkButton(oLkb, False, True)
                            oLkb.Text = "<img src=" & """" & "{0}" & """" & " alt=" & """" & "{1}" & """" & "/>"
                            oLkb.Text = String.Format(oLkb.Text, RootObject.ImgAddRule(Me.BaseUrl), oLkb.ToolTip)
                            oLkb.Visible = True
                        Else
                            hideControl(oLkb)
                        End If

                    End If

                    If dtoItem.isQuiz AndAlso Not IsEditablePath Then
                        oLb = e.Item.FindControl("LBactName")
                        SetLbActName(oLb, dtoItem.Name)
                    Else
                        oHyp = e.Item.FindControl("HYPactName")
                        SetHypActName(oHyp, dtoItem, True)
                    End If


                Else
                    oLb = e.Item.FindControl("LBactName")
                    SetLbActName(oLb, dtoItem.Name)

                End If

                If dtoItem.PermissionEP.Delete AndAlso IsEditablePath Then
                    SetDeleteActivityBtn(e, dtoItem)

                End If

                Dim oImg As System.Web.UI.WebControls.Image
                If Not IsEditablePath Then

                    oImg = e.Item.FindControl("IMGmandatory")
                    oImg.Visible = True
                    If ServiceEP.CheckStatus(dtoItem.Status, Status.Mandatory) Then
                        oImg.ImageUrl = RootObject.ImgMandatorySmall(Me.BaseUrl)
                        oImg.ToolTip = Me.Resource.getValue("ActivityMandatory")
                    Else
                        oImg.ImageUrl = RootObject.ImgBlindSmall(Me.BaseUrl)
                    End If

                End If
                oImg = e.Item.FindControl("IMGstatus")
                oImg.Visible = False

            Case EpViewModeType.View

                Dim isBlocked As Boolean = ServiceEP.CheckStatus(dtoItem.Status, Status.Locked) OrElse Not ServiceEP.CanViewAct_byDate(dtoItem.StartDate, dtoItem.EndDate)

                Dim oImg As System.Web.UI.WebControls.Image
                oImg = e.Item.FindControl("IMGvisibility")
                SetImgBlocked(oImg, isBlocked)
                If isBlocked Then
                    oLb = e.Item.FindControl("LBactName")
                    SetLbActName(oLb, dtoItem.Name)
                Else
                    oHyp = e.Item.FindControl("HYPactName")
                    SetHypActName(oHyp, dtoItem, False)
                End If
                SetImgStatusSmall(e.Item.FindControl("IMGstatus"), dtoItem.statusStat)
                oImg = e.Item.FindControl("IMGmandatory")
                If ServiceEP.CheckStatus(dtoItem.Status, Status.Mandatory) Then

                    oImg.ImageUrl = RootObject.ImgMandatorySmall(Me.BaseUrl)
                    oImg.ToolTip = Me.Resource.getValue("ActivityMandatory")

                Else
                    oImg.ImageUrl = RootObject.ImgBlindSmall(Me.BaseUrl)

                End If
                oImg.Visible = True
        End Select
        oLb = e.Item.FindControl("LBactNumber")
        oLb.Text = Me.Resource.getValue("Activity") & " " & _actIndex & ": "
        _actIndex = _actIndex + 1

        oLb = e.Item.FindControl("LBactWeight")
        oLb.Visible = True
        If isMarkEp Then
            oLb.Text = " (" & dtoItem.Weight & Resource.getValue("Points")
        Else
            oLb.Text = ServiceEP.GetTime(dtoItem.Weight)
        End If

        oLb = e.Item.FindControl("LBdate")
        oLb.Text = SetDateLabel(dtoItem.StartDate, dtoItem.EndDate)

        Dim Rules As List(Of dtoRule) = ServiceEP.GetActivityDtoRuleByActivityId(dtoItem.Id)
        If Rules.Count > 0 Then
            RpRule = e.Item.FindControl("RPrule")
            RpRule.DataSource = Rules
            AddHandler RpRule.ItemDataBound, AddressOf RPrule_ItemDataBound
            RpRule.DataBind()
        Else
            Dim ul As HtmlControl = e.Item.FindControl("ULactRules")
            ul.Attributes("class") = "empty"
            hideControl(ul)
            ul.Visible = False

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


    Public Sub RPactivity_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dtoItem As dtoActivity = e.Item.DataItem

            If ServiceEP.CheckStatus(dtoItem.Status, Status.Text) Then
                Dim oImg As System.Web.UI.WebControls.Image
                oImg = e.Item.FindControl("IMGmandatory")
                oImg.Visible = True
                oImg.ImageUrl = RootObject.ImgTextNoteItem(Me.BaseUrl)
                oImg.ToolTip = Resource.getValue("TextNote")
                oImg.AlternateText = Resource.getValue("TextNote")

                Dim oLb As Label = e.Item.FindControl("LBdescription")
                oLb.Visible = True
                If Not String.IsNullOrWhiteSpace(dtoItem.Description) Then
                    oLb.Text = SmartTagsAvailable.TagAll(dtoItem.Description)
                End If


                Dim ul As HtmlControl = e.Item.FindControl("ULactRules")
                hideControl(ul)
                ul.Visible = False

                If ViewModeType = EpViewModeType.Manage Then
                    ' SetBlockedActivityBtn(e, dtoItem)
                    If Me.PermissionOverPath.Update Then
                        SetSwitchItemButton(dtoItem.Id, "A", e)
                    End If

                    If dtoItem.PermissionEP.Update Then
                        setUpdateActivityBtn(e, dtoItem)
                    End If

                    If dtoItem.PermissionEP.Delete AndAlso IsEditablePath Then
                        SetDeleteActivityBtn(e, dtoItem)
                    End If
                End If

            Else

                RPactivityGeneric_ItemDataBound(e, dtoItem)

            End If

        End If
    End Sub

    Private Sub SetLbActName(ByRef oLB As Label, ByRef name As String)
        oLB.Visible = True
        oLB.Text = name
    End Sub

    Private Sub SetHypActName(ByRef oHyp As HyperLink, ByRef dtoItem As dtoActivity, ByVal manageMode As Boolean)
        oHyp.Visible = True
        oHyp.Text = dtoItem.Name

        If dtoItem.isQuiz AndAlso manageMode Then
            oHyp.NavigateUrl = Me.BaseUrl & RootObject.UpdateActivity(Me.CurrentCommunityID, dtoItem.Id, Me.TempUnitId, IsMoocPath, IsInReadOnlyMode)
        Else
            oHyp.NavigateUrl = Me.BaseUrl & RootObject.ViewActivity(dtoItem.Id, Me.TempUnitId, Me.CurrentPathId, Me.CurrentCommunityID, Me.ViewModeType, IsMoocPath, IsInReadOnlyMode)
        End If
    End Sub


    Public Sub RPrule_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim lbl As Label
            Dim dtoItem As dtoRule = e.Item.DataItem
            lbl = e.Item.FindControl("LBrule")
            lbl.Text = dtoItem.Name

            Dim oLkb As LinkButton
            oLkb = e.Item.FindControl("LKBdeleteRule")
            oLkb.Visible = (ViewModeType = EpViewModeType.Manage)
            oLkb.CommandArgument = dtoItem.Id
            Me.Resource.setLinkButton(oLkb, False, True, , True)

            oLkb = e.Item.FindControl("LKBeditRule")
            oLkb.Visible = (ViewModeType = EpViewModeType.Manage)
            oLkb.CommandArgument = dtoItem.Id
            Me.Resource.setLinkButton(oLkb, False, True)

        End If
    End Sub

#End Region

#Region "events"
    Private Sub RPshownRules_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPshownRules.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim lbl As Label
            Dim dtoItem As dtoRule = e.Item.DataItem
            lbl = e.Item.FindControl("LBrule")
            lbl.Text = dtoItem.Name
        End If
    End Sub
    'Private Sub RPactivity1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPactivity1.ItemDataBound
    '    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '        Dim lbl As Literal
    '        Dim dtoItem As dtoActivity = e.Item.DataItem
    '        lbl = e.Item.FindControl("LITactivity1")
    '        lbl.Text = dtoItem.Name
    '        lbl = e.Item.FindControl("LITactivity1Id")
    '        lbl.Text = "ACT-" + dtoItem.Id.ToString
    '    End If
    'End Sub
    Private Sub RPactivity2_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPactivity2.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim lbl As Literal
            Dim dtoItem As dtoGenericItem = e.Item.DataItem
            lbl = e.Item.FindControl("LITactivity2")

            'lbl = e.Item.FindControl("LITactivity2Id")
            'lbl.Text = "ACT-" + dtoItem.Id.ToString

            '<option value='<asp:Literal runat="server" ID="LITactivity2Id"></asp:Literal>' <% %> >

            'LITactivity2selected
            'lbl = e.Item.FindControl("LITactivity2selected")
            'lbl.Text = IIf(dtoItem.Id = tempActivityRule, "selected='selected'", "")

            TempSelected = IIf(dtoItem.Id = tempActivityRule, "selected='selected'", "")

            lbl.Text = "<option value='ACT-" + dtoItem.Id.ToString + "' " + TempSelected + " >" + dtoItem.Name + "</option>"
        End If

    End Sub
    Private Sub LNBcreateactivityRule_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBcreateActivityRule.Click

        Dim rule As RuleActivityCompletion

        If ViewState("CurrentEditRule") Is Nothing Or CType(ViewState("CurrentEditRule"), Long) < 1 Then
            rule = New RuleActivityCompletion
            rule.CompletionType = CompletionType.NotNeeded
        Else
            rule = ServiceEP.GetActivityRuleById(CType(ViewState("CurrentEditRule"), Long))
            ViewState("CurrentEditRule") = 0
        End If

        Dim activity1 As Long = Long.Parse(HIDact1.Value)
        Dim activity2 As Long = Long.Parse(HIDact2.Value)

        rule.DestinationId = activity1
        rule.SourceId = activity2



        'If RBrangeTypeBT.Checked Then
        '    rule.RangeType = RuleRangeType.Between
        '    rule.MinValue = Short.Parse(IIf(TXBlowerBound.Text = String.Empty, 0, TXBlowerBound.Text))
        '    rule.MaxValue = Short.Parse(IIf(TXBupperBound.Text = String.Empty, 100, TXBupperBound.Text))
        'ElseIf RBrangeTypeGT.Checked Then
        '    rule.RangeType = RuleRangeType.GreaterThan
        '    rule.MinValue = Short.Parse(IIf(TXBgreaterThan.Text = String.Empty, 0, TXBgreaterThan.Text))
        'ElseIf RBrangeTypeLT.Checked Then
        '    rule.RangeType = RuleRangeType.LowerThan
        '    rule.MaxValue = Short.Parse(IIf(TXBlowerThan.Text = String.Empty, 100, TXBlowerThan.Text))
        'End If

        rule.CompletionMaxValue = Short.Parse(IIf(TXBcompleteUp.Text = String.Empty, 100, TXBcompleteUp.Text))
        rule.CompletionMinValue = Short.Parse(IIf(TXBcompleteLow.Text = String.Empty, 0, TXBcompleteLow.Text))
        rule.MarkMaxValue = Short.Parse(IIf(TXBmarkUp.Text = String.Empty, 100, TXBmarkUp.Text))
        rule.MarkMinValue = Short.Parse(IIf(TXBmarkLow.Text = String.Empty, 0, TXBmarkLow.Text))

        'rule.Name = String.Format("{0}->{1}: ", activity2, activity1)


        'Select Case rule.RangeType
        '    Case RuleRangeType.GreaterThan
        '        rule.Name += String.Format("Complete > {0}%", rule.MinValue)
        '    Case RuleRangeType.LowerThan
        '        rule.Name += String.Format("Complete < {0}%", rule.MaxValue)
        '    Case RuleRangeType.Between
        '        rule.Name += String.Format("{0}% < Complete > {1}%", rule.MinValue, rule.MaxValue)
        'End Select


        ServiceEP.SaveOrUpdateRule(rule, Me.CurrentUserId, Me.PageUtility.ProxyIPadress(), Me.PageUtility.ClientIPadress())

        'TXBlowerBound.Text = "0"
        'TXBupperBound.Text = "100"
        'TXBlowerThan.Text = "100"
        'TXBgreaterThan.Text = "0"
        TXBcompleteLow.Text = "0"
        TXBcompleteUp.Text = "100"
        TXBmarkLow.Text = "0"
        TXBmarkUp.Text = "100"


        Dim script As String = String.Format("closeDialog('{0}')", "addRule")
        ScriptManager.RegisterClientScriptBlock(MyBase.Page, Me.GetType, Me.UniqueID, script, True)


        BindDati()

        Me.UDPpath.Update()
    End Sub

    Private Sub LNBcreateunitRule_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBcreateUnitRule.Click
        Dim rule As RuleUnitCompletion

        If ViewState("CurrentEditRule") Is Nothing Or CType(ViewState("CurrentEditRule"), Long) < 1 Then
            rule = New RuleUnitCompletion
            rule.CompletionType = CompletionType.NotNeeded
        Else
            rule = ServiceEP.GetUnitRuleById(CType(ViewState("CurrentEditRule"), Long))
            ViewState("CurrentEditRule") = 0
        End If

        Dim unit1 As Long = Long.Parse(HIDact1.Value)
        Dim unit2 As Long = Long.Parse(HIDact2.Value)

        rule.DestinationId = unit1
        rule.SourceId = unit2



        'If RBrangeTypeBT.Checked Then
        '    rule.RangeType = RuleRangeType.Between
        '    rule.MinValue = Short.Parse(IIf(TXBlowerBound.Text = String.Empty, 0, TXBlowerBound.Text))
        '    rule.MaxValue = Short.Parse(IIf(TXBupperBound.Text = String.Empty, 100, TXBupperBound.Text))
        'ElseIf RBrangeTypeGT.Checked Then
        '    rule.RangeType = RuleRangeType.GreaterThan
        '    rule.MinValue = Short.Parse(IIf(TXBgreaterThan.Text = String.Empty, 0, TXBgreaterThan.Text))
        'ElseIf RBrangeTypeLT.Checked Then
        '    rule.RangeType = RuleRangeType.LowerThan
        '    rule.MaxValue = Short.Parse(IIf(TXBlowerThan.Text = String.Empty, 100, TXBlowerThan.Text))
        'End If

        rule.CompletionMaxValue = Short.Parse(IIf(TXBcompleteUp.Text = String.Empty, 100, TXBcompleteUp.Text))
        rule.CompletionMinValue = Short.Parse(IIf(TXBcompleteLow.Text = String.Empty, 0, TXBcompleteLow.Text))
        rule.MarkMaxValue = Short.Parse(IIf(TXBmarkUp.Text = String.Empty, 100, TXBmarkUp.Text))
        rule.MarkMinValue = Short.Parse(IIf(TXBmarkLow.Text = String.Empty, 0, TXBmarkLow.Text))

        'rule.Name = String.Format("{0}->{1}: ", activity2, activity1)

        'Select Case rule.RangeType
        '    Case RuleRangeType.GreaterThan
        '        rule.Name += String.Format("Complete > {0}%", rule.MinValue)
        '    Case RuleRangeType.LowerThan
        '        rule.Name += String.Format("Complete < {0}%", rule.MaxValue)
        '    Case RuleRangeType.Between
        '        rule.Name += String.Format("{0}% < Complete > {1}%", rule.MinValue, rule.MaxValue)
        'End Select


        ServiceEP.SaveOrUpdateRule(rule, Me.CurrentUserId, Me.PageUtility.ProxyIPadress(), Me.PageUtility.ClientIPadress())

        'TXBlowerBound.Text = "0"
        'TXBupperBound.Text = "100"
        'TXBlowerThan.Text = "100"
        'TXBgreaterThan.Text = "0"
        TXBcompleteLow.Text = "0"
        TXBcompleteUp.Text = "100"
        TXBmarkLow.Text = "0"
        TXBmarkUp.Text = "100"

        Dim script As String = String.Format("closeDialog('{0}')", "addRule")
        ScriptManager.RegisterClientScriptBlock(MyBase.Page, Me.GetType, Me.UniqueID, script, True)


        BindDati()

        Me.UDPpath.Update()
    End Sub


#Region "GESTIONE DIALOG ATTUALMENTE ELIMINATE... IN CASO DI MODIFICA GESTIVANO SE AGGIORNARE LE STATISTICHE O PERSONALIZZARE GLI ASSEGNAMENTI DI CHI HA GIA' INIZIATO IL PERCORSO"
    Private Sub InitDialog()
        '    Me.DLGactVisibility.DialogTitle = Me.Resource.getValue("DLGactVisibTitle")
        '    Me.DLGactVisibility.DialogText = Me.Resource.getValue("DLGactVisibText")
        '    Dim Options As New List(Of String)
        '    Options.Add(Me.Resource.getValue("DLGactVisibUpdateStat"))
        '    Options.Add(Me.Resource.getValue("DLGactVisibPersonalizeAss"))
        '    Me.DLGactVisibility.DataSource = Options
        '    Me.DLGactVisibility.DataBind()

        '    Options = New List(Of String)
        '    Me.DLGunitVisibility.DialogTitle = Me.Resource.getValue("DLGunitVisibTitle")
        '    Me.DLGunitVisibility.DialogText = Me.Resource.getValue("DLGunitVisibText")
        '    Options.Add(Me.Resource.getValue("DLGunitVisibUpdateStat"))
        '    Options.Add(Me.Resource.getValue("DLGunitVisibPersonalizeAss"))
        '    Me.DLGunitVisibility.DataSource = Options
        '    Me.DLGunitVisibility.DataBind()

        '    Options = New List(Of String)
        '    Me.DLGactMandatory.DialogTitle = Me.Resource.getValue("DLGactMandTitle")
        '    Me.DLGactMandatory.DialogText = Me.Resource.getValue("DLGactMandText")
        '    Options.Add(Me.Resource.getValue("DLGactMandUpdateStat"))
        '    Options.Add(Me.Resource.getValue("DLGactMandPersonalizeAss"))
        '    Me.DLGactMandatory.DataSource = Options
        '    Me.DLGactMandatory.DataBind()

        '    Options = New List(Of String)
        '    Me.DLGunitMandatory.DialogTitle = Me.Resource.getValue("DLGunitMandTitle")
        '    Me.DLGunitMandatory.DialogText = Me.Resource.getValue("DLGunitMandText")
        '    Options.Add(Me.Resource.getValue("DLGunitMandUpdateStat"))
        '    Options.Add(Me.Resource.getValue("DLGunitMandPersonalizeAss"))
        '    Me.DLGunitMandatory.DataSource = Options
        '    Me.DLGunitMandatory.DataBind()
    End Sub

    'Private Sub DLGactMandatory_ButtonPressed(ByVal dialogResult As Integer, ByVal CommandArgument As String, ByVal CommandName As String) Handles DLGactMandatory.ButtonPressed
    '    If dialogResult >= 0 Then

    '        Dim UpdateType As UpdateAssignemtOrStatistic = UpdateAssignemtOrStatistic.None
    '        If dialogResult = 0 Then
    '            UpdateType = UpdateAssignemtOrStatistic.Statistic
    '        ElseIf dialogResult = 1 Then
    '            UpdateType = UpdateAssignemtOrStatistic.Assignment
    '        End If

    '        If ServiceEP.UpdateActivityMandatoryOrVisibilityStatus(CommandArgument, UpdateType, UpdateStatusType.MandatoryStatus, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then
    '            Me.PageUtility.RedirectToUrl(RootObject.PathView(Me.CurrentPathId, Me.CurrentCommunityID, Me.ViewModeType))
    '        Else
    '            ShowError(EpError.Generic.Generic)
    '        End If
    '    End If
    'End Sub

    'Private Sub DLGactVisibility_ButtonPressed(ByVal dialogResult As Integer, ByVal CommandArgument As String, ByVal CommandName As String) Handles DLGactVisibility.ButtonPressed

    '    If dialogResult >= 0 Then
    '        Dim UpdateType As UpdateAssignemtOrStatistic = UpdateAssignemtOrStatistic.None
    '        If dialogResult = 0 Then
    '            UpdateType = UpdateAssignemtOrStatistic.Statistic
    '        ElseIf dialogResult = 1 Then
    '            UpdateType = UpdateAssignemtOrStatistic.Assignment
    '        End If

    '        If ServiceEP.UpdateActivityMandatoryOrVisibilityStatus(CommandArgument, UpdateType, UpdateStatusType.VisibilityStatus, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then
    '            Me.PageUtility.RedirectToUrl(RootObject.PathView(Me.CurrentPathId, Me.CurrentCommunityID, Me.ViewModeType))
    '        Else
    '            ShowError(EpError.Generic.Generic)
    '        End If
    '    End If
    'End Sub

    'Private Sub DLGunitMandatory_ButtonPressed(ByVal dialogResult As Integer, ByVal CommandArgument As String, ByVal CommandName As String) Handles DLGunitMandatory.ButtonPressed
    '    If dialogResult >= 0 Then
    '        Dim UpdateType As UpdateAssignemtOrStatistic = UpdateAssignemtOrStatistic.None
    '        If dialogResult = 0 Then
    '            UpdateType = UpdateAssignemtOrStatistic.Statistic
    '        ElseIf dialogResult = 1 Then
    '            UpdateType = UpdateAssignemtOrStatistic.Assignment
    '        End If
    '        If ServiceEP.UpdateUnitMandatoryOrVisibilityStatus(CommandArgument, UpdateType, UpdateStatusType.VisibilityStatus, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then
    '            Me.PageUtility.RedirectToUrl(RootObject.PathView(Me.CurrentPathId, Me.CurrentCommunityID, Me.ViewModeType))
    '        Else
    '            ShowError(EpError.Generic)
    '        End If
    '    End If
    'End Sub

    'Private Sub DLGunitVisibility_ButtonPressed(ByVal dialogResult As Integer, ByVal CommandArgument As String, ByVal CommandName As String) Handles DLGunitVisibility.ButtonPressed
    '    If dialogResult >= 0 Then
    '        Dim UpdateType As UpdateAssignemtOrStatistic = UpdateAssignemtOrStatistic.None
    '        If dialogResult = 0 Then
    '            UpdateType = UpdateAssignemtOrStatistic.Statistic
    '        ElseIf dialogResult = 1 Then
    '            UpdateType = UpdateAssignemtOrStatistic.Assignment
    '        End If

    '        If ServiceEP.UpdateUnitMandatoryOrVisibilityStatus(CommandArgument, UpdateType, UpdateStatusType.VisibilityStatus, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then
    '            Me.PageUtility.RedirectToUrl(RootObject.PathView(Me.CurrentPathId, Me.CurrentCommunityID, Me.ViewModeType))
    '        Else
    '            ShowError(EpError.Generic)
    '        End If
    '    End If
    'End Sub
#End Region

    Private Sub BTNeditOption_Click(sender As Object, e As EventArgs) Handles BTNeditOption.Click
        Dim oButton As Button = DirectCast(sender, Button)
        If IsNumeric(oButton.CommandArgument) Then
            Dim idPath As Long = CurrentPathId

            GotoPathEditing(idPath, DirectCast(CInt(oButton.CommandArgument), PathAvailability))
        End If
        DVconfirmEditing.Visible = False
    End Sub
  
    Private Sub GotoPathEditing(idPath As Long, status As PathAvailability)
        Dim redirect As Boolean = False

        If (idPath > 0) Then
            If status = PathAvailability.Available OrElse (status Or PathAvailability.Available > 0) Then
                ServiceEP.PathSetToBlockStatus(idPath, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress)
            End If
            If (status Or PathAvailability.WithMyStatistics > 0) OrElse (status Or PathAvailability.WithOtherUserStatistics > 0) Then
                ClearPathStatistics(idPath, IsMoocPath)
            End If
            PageUtility.RedirectToUrl(RootObject.PathView(idPath, CurrentCommunityID, ViewModeType, ServiceEP.CheckEpType(ServiceEP.GetPathType(idPath), EPType.PlayMode), IsMoocPath, IsInReadOnlyMode))
        End If
    End Sub
    Private Sub BTNreadonlyOption_Click(sender As Object, e As EventArgs) Handles BTNreadonlyOption.Click
        Dim idPath As Long = CurrentPathId
        DVconfirmEditing.Visible = False
        PageUtility.RedirectToUrl(RootObject.PathView(idPath, CurrentCommunityID, ViewModeType, ServiceEP.CheckEpType(ServiceEP.GetPathType(idPath), EPType.PlayMode), IsMoocPath, True))
    End Sub
    Private Function ClearPathStatistics(idPath As Long, isMooc As Boolean) As Boolean
        Dim allowReset As Boolean = isMooc
        If Not allowReset Then
            Dim setting As lm.Comol.Modules.EduPath.Domain.dtoConfigurationSetting
            setting = ServiceEP.GetConfigurationSetting(idPath, CurrentCommunityID, ConfigurationType.Module)
            allowReset = setting.AllowDeleteStatistics
        End If
        Dim resetCompleted As Boolean = False
        If allowReset Then
            Dim items As List(Of Long) = Me.ServiceEP.GetSubactiviesIdLinkedObjects(idPath, SubActivityType.Quiz)
            Dim idModule As Integer = ServiceEP.ServiceModuleID()

            ServiceEP.ServiceStat.DeleteAllEpStat(idPath)
            If ServiceEP.GetSubactiviesIdLinkedObjects(idPath, SubActivityType.Certificate).Any() Then
                ServiceCF.DeleteUsersCertifications(idPath, idModule, COL_BusinessLogic_v2.UCServices.Services_EduPath.Codex, True)
            End If
            If items.Count > 0 Then
                Dim s As New COL_Questionario.Business.ServiceQuestionnaire(PageUtility.CurrentContext)
                For Each idQuestionnaire As Long In items
                    s.DeleteStatistics(idQuestionnaire)
                Next
            End If

                'For Each item As ListItem In (From i As ListItem In CBLusers.Items Where i.Selected Select i).ToList
                '    ServiceEP.ServiceStat.DeleteAllEpStat(idPath, CInt(item.Value))
                '    ServiceCF.DeleteUsersCertifications(idPath, idModule, COL_BusinessLogic_v2.UCServices.Services_EduPath.Codex, CInt(item.Value), True)
                '    If items.Count > 0 Then
                '        Dim s As New COL_Questionario.Business.ServiceQuestionnaire(PageUtility.CurrentContext)
                '        For Each idQuestionnaire As Long In items
                '            s.DeleteStatistics(idQuestionnaire, CInt(item.Value))
                '        Next
                '    End If
                'Next
                resetCompleted = True
            End If
            Return resetCompleted
    End Function
#End Region

    Protected Overrides Sub NotifyModuleStatus(status As lm.Comol.Core.DomainModel.ModuleStatus)
        CTRLmoduleStatusMessage.Visible = True
        CTRLmoduleStatusMessage.InitializeControl(Resource.getValue("EduPath.ModuleStatus." & status.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub

    Protected Overrides Sub NotifyUnavailableModule(status As lm.Comol.Core.DomainModel.ModuleStatus)
        Master.ServiceTitle = ServiceEP.GetPathName(CurrentPathId)
        MLVeduPathView.SetActiveView(VIWmessages)
        CTRLmessages.InitializeControl(Resource.getValue("EduPath.ModuleStatus." & status.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
    End Sub

    Protected Overrides ReadOnly Property CheckModuleStatus As Boolean
        Get
            Return True
        End Get
    End Property

   
End Class