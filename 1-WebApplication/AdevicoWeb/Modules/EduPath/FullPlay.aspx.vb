Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.EduPath.BusinessLogic
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.Modules.EduPath.Domain
Imports lm.Comol.Modules.EduPath.Domain.DTO
Imports lm.ActionDataContract
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.EduPath.Presentation

Public Class FullPlay
    'Inherits System.Web.UI.Page
    Inherits PageBaseEduPath

#Region "Inherits"
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            If IsSessioneScaduta(False) Then
                RedirectOnSessionTimeOut(RootObject.ViewFullPlay(CurrentPathId, CurrentCommunityID), CurrentCommunityID)
            End If
            Return False
        End Get
    End Property
#End Region

#Region "Page"

#Region "Manage"
    Dim CouldActivityWithSingleSubActivityBeOmitted As Boolean = False
    Const CouldUnitMoved As Boolean = False
    Const CouldActivityMoved As Boolean = False
    Private _canViewSubAct_byUnit As Boolean
    Private _canViewSubAct_byAct As Boolean

    Private _VisibleActivity As IList(Of Int64)
    Private _VisibleUnit As IList(Of Int64)

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
    Private _PermissionOverPath As PermissionEP = New PermissionEP(RoleEP.None)
    Private _PermissionOverUnit As PermissionEP = New PermissionEP(RoleEP.None)
    Private _UnitsCount As Int16
    Private _ActivitiesCount As Int16
    Private _unitId As Int16
    Private _isVisibleByDateConstraint As Boolean

    Private _SmartTagsAvailable As SmartTags

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
    Protected ReadOnly Property SubActivityTypeCssClass(item As SubActivityType) As String
        Get
            Select Case item
                Case SubActivityType.None
                    Return "generic"
                Case SubActivityType.File
                    Return "file"
                Case SubActivityType.Forum
                    Return "forum"
                Case SubActivityType.Quiz
                    Return "quiz"
                Case SubActivityType.Text
                    Return "assignment"
                Case SubActivityType.Certificate
                    Return "certificate"
                Case SubActivityType.Wiki
                    Return "wiki"
                Case Else
                    Return "generic"
            End Select
        End Get
    End Property

    Protected ReadOnly Property SubActivityCssClass(item As dtoSubActivity) As String
        Get
            Return SubActivityTypeCssClass(item.ContentType)
        End Get
    End Property

    Protected ReadOnly Property MoveUnit(item As dtoUnit) As String
        Get
            If CouldUnitMoved Then
                Return "moveunit"
            Else
                Return ""
            End If
        End Get
    End Property
    Protected ReadOnly Property MoveActivity(item As dtoActivity) As String
        Get
            If CouldUnitMoved Then
                Return "moveactivity"
            Else
                Return ""
            End If
        End Get
    End Property

    Protected ReadOnly Property UnitEmpty(item As dtoUnit) As String
        Get
            If Not String.IsNullOrEmpty(item.Description) Or item.Activities.Count > 0 Then
                Return ""
            Else
                Return " empty"
            End If
        End Get
    End Property
    Protected ReadOnly Property UnitDescriptionEmpty(item As dtoUnit) As String
        Get
            If Not String.IsNullOrEmpty(item.Description) Then
                Return ""
            Else
                Return " empty"
            End If
        End Get
    End Property
    Protected ReadOnly Property ActivityDescriptionEmpty(item As dtoActivity) As String
        Get
            If (CouldActivityWithSingleSubActivityBeOmitted) Then
                If Not String.IsNullOrEmpty(item.Description) Then
                    Return ""
                Else
                    Return " empty"
                End If
            Else
                If Not String.IsNullOrEmpty(item.Description) Then
                    Return ""
                Else
                    Return " empty"
                End If
            End If

        End Get
    End Property
    Protected ReadOnly Property ActivityEmpty(item As dtoActivity) As String
        Get
            If (CouldActivityWithSingleSubActivityBeOmitted) Then
                If Not String.IsNullOrEmpty(item.Description) Or item.SubActivities.Count > 1 Then
                    Return ""
                Else
                    Return " empty"
                End If
            Else
                If Not String.IsNullOrEmpty(item.Description) Or item.SubActivities.Count > 0 Then
                    Return ""
                Else
                    Return " empty"
                End If
            End If

        End Get
    End Property
    Protected ReadOnly Property IsDataConstrained(item As dtoActivity) As String
        Get
            If (ServiceEP.CanViewAct_byDate(item.StartDate, item.EndDate)) Then
                Return ""
            Else
                Return " dateblocked"
            End If
        End Get
    End Property

    Protected ReadOnly Property HasDataConstrain(item As dtoActivity) As String
        Get
            If item.StartDate.HasValue Or item.EndDate.HasValue Then
                Return " dateconstraint"
            Else
                Return " nodateconstraint"
            End If
        End Get
    End Property

#End Region

#Region "Url"
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
    Public ReadOnly Property CurrentPath() As Path
        Get
            If (CurrentPathId <> -1) Then
                Return ServiceEP.GetPath(CurrentPathId)
            End If
        End Get
    End Property
    Protected Overrides ReadOnly Property PathType As EPType
        Get
            If _PathType = lm.Comol.Modules.EduPath.Domain.EPType.None Then
                _PathType = ServiceEP.GetEpType(CurrentPathId, ItemType.Path)
            End If
            Return _PathType
        End Get
    End Property
    Protected ReadOnly Property ProeloadOpenIdUnit() As Long
        Get
            If IsNumeric(Request.QueryString("OidU")) Then
                Return CLng(Request.QueryString("OidU"))
            Else
                Return CLng(0)
            End If
        End Get
    End Property
    Protected ReadOnly Property ProeloadOpenIdActivity() As Long
        Get
            If IsNumeric(Request.QueryString("OidA")) Then
                Return CLng(Request.QueryString("OidA"))
            Else
                Return CLng(0)
            End If
        End Get
    End Property
#End Region

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

    Public Sub ItemActionResult(err As lm.Comol.Core.Certifications.CertificationError, ByVal savingRequired As Boolean, ByVal saved As Boolean)
        BindDati()
        Dim tString As String = String.Format("CertificationError.{0}.savingRequired.{1}.saved.{2}", err.ToString, savingRequired, saved)
        CTRLmessages.Visible = True
        Select Case err
            Case lm.Comol.Core.Certifications.CertificationError.None
                CTRLmessages.InitializeControl(Resource.getValue(tString), Helpers.MessageType.success)
            Case lm.Comol.Core.Certifications.CertificationError.TransmittingFile
                CTRLmessages.InitializeControl(Resource.getValue(tString), Helpers.MessageType.alert)
            Case Else
                CTRLmessages.InitializeControl(Resource.getValue(tString), Helpers.MessageType.error)
        End Select
    End Sub


    Public Sub RefreshContainerEvent(sender As Object, e As RefreshContainerArgs)
        CTRLmessages.Visible = False
        If (e.Executed) Then
            BindDati()
        Else
            Me.ShowError(EpError.Generic)
        End If
    End Sub

    Public Sub GetHiddenIdentifierValueEvent(ByRef value As String)
        CTRLmessages.Visible = False
        value = Me.HDNdownloadTokenValue.Value
    End Sub


    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_EduPath.Codex)
        CouldActivityWithSingleSubActivityBeOmitted = True ' EduPathConfiguration.UseSingleActionView
        Me.Master.ShowDocType = True
    End Sub

#Region "Inherits"
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
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub



    'Protected Overrides ReadOnly Property PathType As lm.Comol.Modules.EduPath.Domain.EPType
    '    Get

    '    End Get
    'End Property

    Private Sub UpdateScormStat()
        Dim ModuleLinkIds As IList(Of Long) = ServiceEP.GetMaterialModuleLinkIds_ByPathId(Me.CurrentPathId)
        If ModuleLinkIds.Any() Then
            Dim oSender As PermissionService.IServicePermission = Nothing
            Dim results As List(Of dtoItemEvaluation(Of Long))
            Dim UserID As Integer = CurrentContext.UserContext.CurrentUserID
            Try
                oSender = New PermissionService.ServicePermissionClient
                results = oSender.GetPendingEvaluations(ModuleLinkIds, UserID).ToList()
                If Not IsNothing(oSender) Then
                    Dim service As System.ServiceModel.ClientBase(Of PermissionService.IServicePermission) = DirectCast(oSender, System.ServiceModel.ClientBase(Of PermissionService.IServicePermission))
                    service.Close()
                    service = Nothing
                End If
            Catch ex As Exception
                If Not IsNothing(oSender) Then
                    Dim service As System.ServiceModel.ClientBase(Of PermissionService.IServicePermission) = DirectCast(oSender, System.ServiceModel.ClientBase(Of PermissionService.IServicePermission))
                    service.Abort()
                    service = Nothing
                End If
            End Try
            ServiceEP.SaveActionsExecution(results, UserID)
        End If
    End Sub

    Private Sub UpdateWebinarStat()
        Dim ModuleLinkIds As List(Of Long) = ServiceEP.GetWebinarModuleLinkIds_ByPathId(Me.CurrentPathId).ToList()

        If ModuleLinkIds.Any() Then
            Dim oSender As PermissionService.IServicePermission = Nothing
            Dim results As List(Of dtoItemEvaluation(Of Long))
            Dim UserID As Integer = CurrentContext.UserContext.CurrentUserID

            Try
                oSender = New PermissionService.ServicePermissionClient
                results = oSender.EvaluateModuleLinks(ModuleLinkIds, UserID)
                'results = oSender.EvaluateModuleLink().ToList()
                If Not IsNothing(oSender) Then
                    Dim service As System.ServiceModel.ClientBase(Of PermissionService.IServicePermission) = DirectCast(oSender, System.ServiceModel.ClientBase(Of PermissionService.IServicePermission))
                    service.Close()
                    service = Nothing
                End If
            Catch ex As Exception
                If Not IsNothing(oSender) Then
                    Dim service As System.ServiceModel.ClientBase(Of PermissionService.IServicePermission) = DirectCast(oSender, System.ServiceModel.ClientBase(Of PermissionService.IServicePermission))
                    service.Abort()
                    service = Nothing
                End If
            End Try
            ServiceEP.SaveActionsExecution(results, UserID)
        End If

    End Sub

    Private Sub InitPlayer()


        Dim lastActivity As dtoActivityPlayer = ServiceEP.GetLastViewedActivity(Me.CurrentUserId, Me.CurrentCommRoleID, Me.CurrentPathId, isAutoEp, DateTime.Now)

        Dim lastSubactivity As SubActivity
        '

        CouldActivityWithSingleSubActivityBeOmitted = CurrentPath.SingleAction

        Dim lastActivities As IList(Of dtoActivityPlayer) = ServiceEP.GetViewedActivity(Me.CurrentUserId, Me.CurrentCommunityID, Me.CurrentPathId, isAutoEp, DateTime.Now)

        Me.Resource.setLabel(Me.LBplayerTitle)

        If IsNothing(lastActivity) Then
            ' NON SERVE ! COME NASCONDO UN dtoActivityPlayer CHE E' ANCHE NULLO ?????   hideControl(lastActivity)
            Me.DIVLastActivity.Visible = False
            'Me.MLVlastActivity.Visible = False
        Else
            Me.DIVLastActivity.Visible = True

            '@Start LastSubactivity
            Dim count As Int64 = ServiceEP.GetCountDtoSubActivity_View(lastActivity.Id, Me.CurrentUserId, Me.CurrentCommRoleID)
            If (CouldActivityWithSingleSubActivityBeOmitted And count = 1) Then
                lastSubactivity = ServiceEP.GetFirstSubActivity_View(lastActivity.Id, Me.CurrentUserId, Me.CurrentCommRoleID)
            Else
                lastSubactivity = Nothing
            End If

            'Dim imgStatus = DIVLastActivity.FindControl("ImgStatus")

            SetStatus(lastActivity.StatusStatistic, DIVLastActivity.FindControl("ImgStatus"))

            Dim alink As String = "<a href='#act-{0}' class='lastactivity'>{1}</a>"
            Dim actname As String = ""
            'Dim alink As HtmlControl = DIVLastActivity.FindControl("ALink")
            'alink.Attributes("href") = "#act-" + lastActivity.Id.ToString()

            If Not lastSubactivity Is Nothing Then
                actname += lastActivity.Name
                'If ServiceEP.isSubActityInternalModule(lastSubactivity.ContentType) Then
                '    Dim oDisplayAction As lm.Comol.Modules.EduPath.Presentation.IViewModuleTextAction = DIVLastActivity.FindControl("CTRLtextAction")
                '    oDisplayAction.RefreshContainer = False
                '    DIVLastActivity.FindControl("CTRLtextAction").Visible = True
                '    Dim initializer As New dtoInternalActionInitializer
                '    initializer.SubActivity = New dtoSubActivity() With {.Id = lastSubactivity.Id, .Description = lastSubactivity.Description}
                '    ' AGGIUNTA PLACEHOLDER
                '    ' --> 
                '    'initializer.PlaceHolders.Add(New lm.Comol.Core.ModuleLinks.dtoPlaceHolder() With {.Text = "", .Type = lm.Comol.Core.ModuleLinks.PlaceHolderType.three})
                '    ' DEFINISCO UNA CLASSE PER IL CONTAINER
                '    initializer.ContainerCSS = SubActivityTypeCssClass(lastSubactivity.ContentType)
                '    ' DIMENSIONI IMMAGINI
                '    oDisplayAction.IconSize = Helpers.IconSize.Small
                '    initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.text
                '    oDisplayAction.InitializeControl(initializer)

                'Else
                '    DIVLastActivity.FindControl("CTRLtextAction").Visible = False
                '    Dim oDisplayAction As lm.Comol.Core.ModuleLinks.IExternalModuleDisplayAction = CType(LoadControl(BaseUrl & lm.Comol.Core.DomainModel.Common.CoreRootObject.GenericNewDisplayActionControl), lm.Comol.Core.ModuleLinks.IExternalModuleDisplayAction)
                '    Dim initializer As New lm.Comol.Core.ModuleLinks.dtoExternalModuleInitializer
                '    initializer.Link = lastSubactivity.ModuleLink
                '    ' AGGIUNTA PLACEHOLDER
                '    ' --> initializer.PlaceHolders.Add(New lm.Comol.Core.ModuleLinks.dtoPlaceHolder() With {.Text = "HH:ss", .Type = lm.Comol.Core.ModuleLinks.PlaceHolderType.three})                    
                '    ' DEFINISCO UNA CLASSE PER IL CONTAINER
                '    oDisplayAction.ContainerCSS = SubActivityTypeCssClass(lastSubactivity.ContentType)
                '    ' DIMENSIONI IMMAGINI
                '    oDisplayAction.IconSize = Helpers.IconSize.Small

                '    oDisplayAction.EnableAnchor = True

                '        oDisplayAction.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.text

                '    oDisplayAction.InitializeControl(initializer)
                '    PHLAction.Controls.Add(oDisplayAction)
                'End If

            Else
                actname += lastActivity.Name
            End If


            LTRLastLink.Text = String.Format(alink, lastActivity.Id, actname)

            'Dim ALink As HtmlControl = DIVLastActivity.FindControl("ALink")
            'ALink.Attributes("href") = "#act-" + lastActivity.Id.ToString()



            '@End LastSubactivity

            'Me.MLVlastActivity.Visible = True
            'Me.PlayerUnitId = lastActivity.ParentUnitId
            'Me.HYPcurrentActivityName.Text = lastActivity.Name
            'Me.HYPcurrentActivityName.NavigateUrl = Me.BaseUrl & RootObject.PathView(CurrentPathId, CurrentCommunityID, EpViewModeType.View, True) & "#" & lastActivity.Id

            ''If lastActivity.StartDate Is Nothing AndAlso lastActivity.EndDate Is Nothing Then
            ''    hideControl(LBdate)

            ''Else
            ''    Me.LBdate.Text = Me.SetDateLabel(lastActivity.StartDate, lastActivity.EndDate)
            ''End If

            If lastActivity.Description.Count > 0 Then
                'Me.LBdescription.Text = SmartTagsAvailable.TagAll(lastActivity.Description)
            Else
                'hideControl(LBdescription)

            End If

            'SetImgStatusMedium(Me.IMGstatus, lastActivity.StatusStatistic)

        End If

    End Sub

    Private Sub initPageView()

        UpdateScormStat()

        UpdateWebinarStat()

        Me.CTRLhelpStatus.Init()
        Dim dtoEP As dtoEduPath = Me.ServiceEP.GetEduPathStructure_PlayMode(Me.CurrentPathId, Me.CurrentUserId, Me.CurrentCommRoleID, DateTime.Now)

        Dim currentUserPermission As PermissionEP = Me.ServiceEP.GetUserPermission_ByPath(Me.CurrentPathId, Me.CurrentUserId, Me.CurrentCommRoleID)

        If IsNothing(dtoEP) OrElse ServiceEP.CheckStatus(dtoEP.Status, Status.Locked) OrElse (currentUserPermission.Read <> True) Then
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

            ''VisibleActivity = Nothing

            If ServiceStat.InitPathBrowsed(pathId, Me.CurrentUserId, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then
                InitPlayer()
                'InitProgressBar(dtoEP.
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
                    'RPunit.DataSource = dtoEP.Units
                    RPTunits.DataSource = dtoEP.Units

                Else
                    RPTunits.DataSource = (From item In dtoEP.Units Where VisibleUnit.Contains(item.Id)).ToList()
                End If

                RPTunits.DataBind()
            Else
                Me.ShowError(EpError.Generic)
            End If
        End If


    End Sub

    Private Sub InitProgressBar(ByVal isVisible As Boolean)
        If isVisible Then
            If isAutoEp Then
                Dim statForBar As dtoStatWithWeight = ServiceEP.ServiceStat.GetPassedCompletedWeight_byActivity(Me.CurrentPathId, Me.CurrentUserId, DateTime.Now)
                Me.CTRLprogressBar.InitBar(statForBar.Completion, statForBar.MinCompletion, "")
            Else
                Dim statForBar As dtoStatWithWeight = ServiceEP.ServiceStat.GetPassedCompletedWeight_byUnit(Me.CurrentPathId, Me.CurrentUserId, DateTime.Now)
                Me.CTRLprogressBar.InitBar(statForBar.Completion, statForBar.MinCompletion, Me.Resource.getValue("ExecutedWeight") & statForBar.UserTotWeight)

            End If
            'MLVprogress.Visible = True
            divProgressBar.Visible = True

        Else
            'MLVprogress.Visible = False
            divProgressBar.Visible = False
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

    Private Sub RPTunits_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTunits.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dtoItem As dtoUnit = e.Item.DataItem
            _unitId = dtoItem.Id

            If ServiceEP.CheckStatus(dtoItem.Status, Status.Text) Then
                'hideControl(e.Item.FindControl("LIunit"))
                'Dim oLB As Label = e.Item.FindControl("LBtextNote")
                'oLB.Text = dtoItem.Description

                hideControl(e.Item.FindControl("DIVGenericUnit"))
                e.Item.FindControl("DIVUnitNoteFix").Visible = True
                Dim x As HtmlControl = e.Item.FindControl("DIVUnitNote")
                x.Visible = True

                Dim oLB As Label = x.FindControl("LBLUnitNote")
                oLB.Text = dtoItem.Description




            Else
                'hideControl(e.Item.FindControl("LInote"))

                hideControl(e.Item.FindControl("DIVUnitNote"))
                hideControl(e.Item.FindControl("DIVUnitNoteFix"))
                e.Item.FindControl("DIVGenericUnit").Visible = True
                'RPactivityGeneric_ItemDataBound(e, dtoItem)

                Me.RPunitGeneric_ItemDataBound(e, dtoItem)

            End If

        End If
    End Sub

    Private Sub RPunitGeneric_ItemDataBound(ByRef e As System.Web.UI.WebControls.RepeaterItemEventArgs, ByRef dtoItem As dtoUnit)
        Dim LBunit As Label
        Dim RpActivities As Repeater

        If isAutoEp Then
            'hideControl(e.Item.FindControl("IMGstatus"))
            SetStatus(dtoItem.statusStat, e.Item.FindControl("ImgStatus"), True)
        Else
            'SetImgStatusMedium(e.Item.FindControl("IMGstatus"), dtoItem.statusStat)
            SetStatus(dtoItem.statusStat, e.Item.FindControl("ImgStatus"))
        End If

        If Not ServiceStat.UnitIsCompleted(dtoItem.Activities) Then
            'Dim oSpan As HtmlControl = e.Item.FindControl("SPopenClose")
            'oSpan.Attributes.Add("class", "switch autoOpen")
        End If

        Dim oImg As System.Web.UI.WebControls.Image


        'oImg = e.Item.FindControl("IMGvisibility")

        _canViewSubAct_byUnit = ServiceEP.CheckStatus(dtoItem.Status, Status.NotLocked)
        'SetImgBlocked(oImg, Not _canViewSubAct_byUnit)

        Dim oMand As HtmlControl = e.Item.FindControl("ImgMandatory")

        Dim oLock As HtmlControl = e.Item.FindControl("ImgLocked")
        oLock.Visible = Not _canViewSubAct_byUnit
        oLock.Attributes("title") = Me.Resource.getValue("Blocked." & oLock.Visible)

        If ServiceEP.CheckStatus(dtoItem.Status, Status.Mandatory) Then
            oMand.Attributes("title") = Me.Resource.getValue("UnitMandatory")
            oMand.Visible = True
        Else
            oMand.Visible = False
            'hideControl(oImg)
        End If

        LBunit = e.Item.FindControl("LBLUnitTitle")
        LBunit.Visible = True
        LBunit.Text = dtoItem.Name

        LBunit = e.Item.FindControl("LBLUnitDuration")
        LBunit.Visible = True

        If isMarkEp Then
            LBunit.Text = " (" & dtoItem.Weight & Resource.getValue("Points")
        Else
            LBunit.Text = ServiceEP.GetTime(dtoItem.Weight)
        End If

        LBunit = e.Item.FindControl("LBLUnitDescription")
        If dtoItem.Description.Count > 0 Then

            LBunit.Text = dtoItem.Description
        Else
            'hideControl(LBunit)
            'hideControl(e.Item.FindControl("DIVunitDescription"))
        End If

        ''Me.TempUnitId = dtoItem.Id
        RpActivities = e.Item.FindControl("RPTactivities")

        ''from rule

        If VisibleActivity Is Nothing Then
            RpActivities.DataSource = dtoItem.Activities
        Else
            RpActivities.DataSource = (From item In dtoItem.Activities Where VisibleActivity.Contains(item.Id)).ToList()
        End If

        'AddHandler RpActivities.ItemDataBound, AddressOf RPactivity_ItemDataBound
        AddHandler RpActivities.ItemDataBound, AddressOf RPTactivities_ItemDataBound
        RpActivities.DataBind()

        Dim Rulelist As List(Of dtoRule) = ServiceEP.GetUnitDtoRuleByUnitId(dtoItem.Id)

        If Rulelist.Count > 0 Then
            Dim RpRules As Repeater = e.Item.FindControl("RPTUnitRules")
            RpRules.DataSource = Rulelist
            AddHandler RpRules.ItemDataBound, AddressOf RPunitRule_ItemDataBound
            RpRules.DataBind()
        Else

            'Dim ulRule As HtmlControl = e.Item.FindControl("ULunitRules")
            'ulRule.Attributes("class") = "empty"
            'ulRule.Visible = False
            'hideControl(ulRule)
        End If
    End Sub

    Private Sub RPTactivities_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dtoItem As dtoActivity = e.Item.DataItem

            If ServiceEP.CheckStatus(dtoItem.Status, Status.Text) Then

                hideControl(e.Item.FindControl("DIVGenericActivity"))
                e.Item.FindControl("DIVActivityNoteFix").Visible = True
                Dim x As HtmlControl = e.Item.FindControl("DIVActivityNote")
                x.Visible = True

                Dim oLB As Label = x.FindControl("LBLActivityNote")
                oLB.Text = dtoItem.Description

            Else
                hideControl(e.Item.FindControl("DIVActivityNote"))
                hideControl(e.Item.FindControl("DIVActivityNoteFix"))
                e.Item.FindControl("DIVGenericActivity").Visible = True

                RPactivityGeneric_ItemDataBound(e, dtoItem)

            End If

        End If
    End Sub

    Public Sub RPunitRule_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim lbl As Label
            Dim dtoItem As dtoRule = e.Item.DataItem
            lbl = e.Item.FindControl("LBLUnitRuleName")
            lbl.Text = dtoItem.Name

        End If
    End Sub

    Private Sub RPactivityGeneric_ItemDataBound(ByRef e As System.Web.UI.WebControls.RepeaterItemEventArgs, ByRef dtoItem As dtoActivity)
        Dim oHyp As HyperLink
        Dim oLb As Label
        Dim oRp As Repeater

        Dim isBlocked As Boolean = ServiceEP.CheckStatus(dtoItem.Status, Status.Locked)

        'Dim oImg As System.Web.UI.WebControls.Image
        'oImg = e.Item.FindControl("IMGvisibility")
        _canViewSubAct_byAct = _canViewSubAct_byUnit And Not isBlocked
        'SetImgBlocked(oImg, isBlocked)
        Dim olock1 As HtmlControl = e.Item.FindControl("IMGLockedActivity")
        If (Not olock1 Is Nothing) Then
            olock1.Visible = isBlocked
            olock1.Attributes("title") = Me.Resource.getValue("Blocked." & isBlocked)
        End If
        Dim oLock As HtmlControl = e.Item.FindControl("ImgLocked")
        If (oLock Is Nothing) Then
            oLock = e.Item.FindControl("IMGLockedActivity")
        End If
        oLock.Attributes("title") = Me.Resource.getValue("Blocked." & isBlocked)
        oLock.Visible = isBlocked

        oLb = e.Item.FindControl("LBLActivityTitle")
        SetLbActName(oLb, dtoItem.Name)

        oLb = e.Item.FindControl("LBLActivityDescription")
        If dtoItem.Description.Count > 0 Then
            oLb.Text = dtoItem.Description
        Else
            hideControl(oLb)
        End If

        Dim test As HtmlControl = e.Item.FindControl("ImgStatus")

        SetStatus(dtoItem.statusStat, e.Item.FindControl("ImgStatus"))
        'SetImgStatusSmall(e.Item.FindControl("IMGstatus"), dtoItem.statusStat)
        'oImg = e.Item.FindControl("IMGmandatory")


        Dim oMand As HtmlControl = e.Item.FindControl("ImgMandatory")

        If ServiceEP.CheckStatus(dtoItem.Status, Status.Mandatory) Then

            'oImg.ImageUrl = RootObject.ImgMandatorySmall(Me.BaseUrl)
            'oImg.ToolTip = Me.Resource.getValue("ActivityMandatory")
            oMand.Attributes("title") = Me.Resource.getValue("ActivityMandatory")
            oMand.Visible = True
        Else
            'hideControl(oImg)
            oMand.Visible = False
        End If

        oLb = e.Item.FindControl("LBLActivityDuration")
        oLb.Visible = True
        If isMarkEp Then
            oLb.Text = " (" & dtoItem.Weight & Resource.getValue("Points")
        Else
            oLb.Text = ServiceEP.GetTime(dtoItem.Weight)
        End If

        oLb = e.Item.FindControl("LBLActivityExpiration")
        If dtoItem.StartDate Is Nothing AndAlso dtoItem.EndDate Is Nothing Then
            hideControl(oLb)
        Else
            oLb.Text = SetDateLabel(dtoItem.StartDate, dtoItem.EndDate)
        End If


        'Dim Rules As List(Of dtoRule) = ServiceEP.GetActivityDtoRuleByActivityId(dtoItem.Id)
        'If Rules.Count > 0 Then
        '    oRp = e.Item.FindControl("RPrule")
        '    oRp.DataSource = Rules
        '    'AddHandler oRp.ItemDataBound, AddressOf RPrule_ItemDataBound
        '    oRp.DataBind()
        'Else
        '    Dim ul As HtmlControl = e.Item.FindControl("ULactRules")
        '    ul.Attributes("class") = "empty"
        '    hideControl(ul)
        'End If

        _isVisibleByDateConstraint = ServiceEP.CanViewAct_byDate(dtoItem.StartDate, dtoItem.EndDate)

        'If _canViewSubAct_byAct AndAlso dtoItem.SubActivities.Count > 0 Then
        If dtoItem.SubActivities.Count > 0 Then

            Dim MLVsingleSubAct As MultiView = e.Item.FindControl("MUVSingleMultiActivity")

            Dim VIWSingle As View = e.Item.FindControl("VIWSingleActivity")


            If CouldActivityWithSingleSubActivityBeOmitted And dtoItem.SubActivities.Count = 1 Then
                MLVsingleSubAct.ActiveViewIndex = 1

                dtoItem.SubActivities.First().Weight = dtoItem.Weight

                SingleSubActivity(dtoItem.SubActivities.First(), VIWSingle)



                'oImg = e.Item.FindControl("IMGvisibility_single")
                _canViewSubAct_byAct = _canViewSubAct_byUnit And Not isBlocked
                'SetImgBlocked(oImg, isBlocked)

                'SetImgStatusSmall(e.Item.FindControl("IMGstatus_single"), dtoItem.statusStat)
                'oImg = e.Item.FindControl("IMGmandatory_single")
                Dim lbl As Label = e.Item.FindControl("LBLActivitySingleDescription")
                lbl.Text = dtoItem.Description

                'verify
                'If Not String.IsNullOrEmpty(dtoItem.SubActivities.First().Description) Then
                '    lbl.Text += "<br />" + dtoItem.SubActivities.First().Description
                'End If



                If ServiceEP.CheckStatus(dtoItem.Status, Status.Mandatory) Or ServiceEP.CheckStatus(dtoItem.SubActivities.First().Status, Status.Mandatory) Then
                    oMand = e.Item.FindControl("IMGMandatorySingle") '
                    'oImg.ImageUrl = RootObject.ImgMandatorySmall(Me.BaseUrl)
                    'oImg.ToolTip = Me.Resource.getValue("ActivityMandatory")
                    'oImg.Visible = True
                    oMand.Attributes("title") = Me.Resource.getValue("ActivityMandatory")
                    oMand.Visible = True
                Else
                    'hideControl(oImg)
                    oMand.Visible = False

                End If

                oLb = e.Item.FindControl("LBLActivityDuration")
                oLb.Visible = True
                If isMarkEp Then
                    oLb.Text = " (" & dtoItem.Weight & Resource.getValue("Points")
                Else
                    oLb.Text = ServiceEP.GetTime(dtoItem.Weight)
                End If

                oLb = e.Item.FindControl("LBLSingleActivityExpiration")
                If dtoItem.StartDate Is Nothing AndAlso dtoItem.EndDate Is Nothing Then
                    hideControl(oLb)
                Else
                    oLb.Text = SetDateLabel(dtoItem.StartDate, dtoItem.EndDate)
                End If

            Else
                MLVsingleSubAct.ActiveViewIndex = 0

                oRp = e.Item.FindControl("RPTsubactivities")
                oRp.DataSource = dtoItem.SubActivities
                AddHandler oRp.ItemDataBound, AddressOf RpSubAct_ItemDataBound
                oRp.DataBind()
            End If





        Else
            hideControl(e.Item.FindControl("UlsubAct"))
        End If
    End Sub

    Private Sub RpSubAct_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

            '    Dim oLb As Label
            '    Dim oLkb As LinkButton
            '    Dim oHyp As HyperLink
            '    Dim oImg As System.Web.UI.WebControls.Image
            Dim dtoItem As dtoSubActivity = e.Item.DataItem

            SetStatus(dtoItem.StatusStat, e.Item.FindControl("ImgStatus"))

            '    'SetImgStatusSmall(e.Item.FindControl("IMGstatus"), dtoItem.StatusStat)

            '    oImg = e.Item.FindControl("IMGmandatory")

            '    If ServiceEP.CheckStatus(dtoItem.Status, Status.Mandatory) Then
            '        oImg.ImageUrl = RootObject.ImgMandatoryMedium(Me.BaseUrl)
            '        oImg.ToolTip = Me.Resource.getValue("SubActivityMandatory")
            '        oImg.Visible = True
            '    Else
            '        hideControl(oImg)
            '    End If

            Dim isVisible As Boolean = ServiceEP.CheckStatus(dtoItem.Status, Status.NotLocked) And _canViewSubAct_byAct

            '    'SetImgBlocked(e.Item.FindControl("IMGvisibility"), Not isVisible)


            '    'oImg = e.Item.FindControl("IMGtype")
            '    'oImg.ImageUrl = RootObject.ImgContentType(Me.BaseUrl, dtoItem.ContentType)


            '    'oLb = e.Item.FindControl("LBpoints")

            '    'If isMarkEp Then
            '    '    oLb.Text = " (" & dtoItem.Weight & Resource.getValue("Points")
            '    'Else
            '    '    hideControl(oLb)  ' oLb.Text = ServiceEP.GetTime(dtoItem.Weight)
            '    'End If

            '    'oLb = e.Item.FindControl("LBnameTitle")
            '    'Me.Resource.setLabel(oLb)

            '    'oHyp = e.Item.FindControl("HYPname")
            '    'oHyp.Text = dtoItem.Name
            '    'oHyp.NavigateUrl = dtoItem.Link

            '    'oLb = e.Item.FindControl("LBdescription")

            '    'oLkb = e.Item.FindControl("LKBexecute")
            Dim oPlaceHolder As PlaceHolder = e.Item.FindControl("PHLAction")

            Dim weight As String
            If isMarkEp Then
                weight = " (" & dtoItem.Weight & Resource.getValue("Points")
            Else
                'hideControl(oLb)  ' oLb.Text = ServiceEP.GetTime(dtoItem.Weight)
                weight = ""
            End If

            If ServiceEP.isSubActityInternalModule(dtoItem.ContentType) Then
                '        oLb.Text = Me.SmartTagsAvailable.TagAll(dtoItem.Description)

                '        If isVisible AndAlso _isVisibleByDateConstraint AndAlso CanProceedByEndDate Then
                '            Me.Resource.setLinkButton(oLkb, False, True)
                '            oLkb.CommandArgument = dtoItem.Id
                '        Else
                '            hideControl(oLkb)
                '        End If
                Dim initializer As New dtoInternalActionInitializer
                initializer.IdPath = CurrentPathId
                initializer.IdPathCommunity = Me.CurrentCommunityID
                initializer.CookieName = "PathCertifications_" & initializer.IdPath
                initializer.SubActivity = dtoItem
                ' AGGIUNTA PLACEHOLDER
                ' --> 
                initializer.PlaceHolders.Add(New lm.Comol.Core.ModuleLinks.dtoPlaceHolder() With {.Text = weight, .Type = lm.Comol.Core.ModuleLinks.PlaceHolderType.three, .CssClass = "duration"})
                ' DEFINISCO UNA CLASSE PER IL CONTAINER
                initializer.ContainerCSS = SubActivityCssClass(dtoItem)
                If isVisible AndAlso _isVisibleByDateConstraint AndAlso CanProceedByEndDate Then
                    initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction
                Else
                    initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.text
                End If
                Select Case dtoItem.ContentType
                    Case SubActivityType.Text
                        Dim oDisplayAction As lm.Comol.Modules.EduPath.Presentation.IViewModuleTextAction = e.Item.FindControl("CTRLtextAction")
                        e.Item.FindControl("CTRLtextAction").Visible = True
                        oDisplayAction.RefreshContainer = True

                        ' DIMENSIONI IMMAGINI
                        oDisplayAction.IconSize = Helpers.IconSize.Small
                        oDisplayAction.EnableAnchor = True
                        'AddHandler oDisplayAction.RefreshContainerEvent, AddressOf RefreshContainerEvent
                        oDisplayAction.InitializeControl(initializer)
                        'oPlaceHolder.Controls.Add(oDisplayAction)
                    Case SubActivityType.Certificate
                        Dim control As UC_CertificationAction = e.Item.FindControl("CTRLcertificationAction")
                        control.Visible = True
                        control.EvaluablePath = Not isTimeEp()
                        Dim oCertAction As lm.Comol.Modules.EduPath.Presentation.IViewModuleCertificationAction = e.Item.FindControl("CTRLcertificationAction")
                        'oCertAction.RefreshContainer = True
                        ' DIMENSIONI IMMAGINI
                        oCertAction.IconSize = Helpers.IconSize.Small
                        oCertAction.EnableAnchor = True

                        If dtoItem.AutoGenerated AndAlso dtoItem.StatusStat <> StatusStatistic.CompletedPassed Then
                            Dim newStatus As StatusStatistic = StatusStatistic.Browsed
                            control.AutoGenerateCertificationNEW(CurrentPathId, initializer.IdPathCommunity, CurrentUserId, dtoItem, DateTime.Now, StatusStatistic.Browsed, newStatus)
                            If newStatus <> StatusStatistic.Browsed Then
                                SetStatus(newStatus, e.Item.FindControl("ImgStatus"))
                                InitProgressBar(True)
                            End If
                        End If


                        'AddHandler oDisplayAction.RefreshContainerEvent, AddressOf RefreshContainerEvent
                        oCertAction.InitializeControl(initializer)
                End Select
            Else
                e.Item.FindControl("CTRLtextAction").Visible = False
                Dim oDisplayAction As lm.Comol.Core.ModuleLinks.IExternalModuleDisplayAction = CType(LoadControl(BaseUrl & lm.Comol.Core.DomainModel.Common.CoreRootObject.GenericNewDisplayActionControl), lm.Comol.Core.ModuleLinks.IExternalModuleDisplayAction)
                'Dim oDisplayAction As lm.Comol.Core.ModuleLinks.IExternalModuleDisplayAction = e.Item .FindControl("textAction")
                '        hideControl(oLkb)
                '        oLb.Text = Me.SmartTagsAvailable.TagAll(dtoItem.Description)

                Dim initializer As New lm.Comol.Core.ModuleLinks.dtoExternalModuleInitializer
                initializer.Link = dtoItem.ModuleLink
                ' AGGIUNTA PLACEHOLDER
                ' --> initializer.PlaceHolders.Add(New lm.Comol.Core.ModuleLinks.dtoPlaceHolder() With {.Text = "HH:ss", .Type = lm.Comol.Core.ModuleLinks.PlaceHolderType.three})
                initializer.PlaceHolders.Add(New lm.Comol.Core.ModuleLinks.dtoPlaceHolder() With {.Text = weight, .Type = lm.Comol.Core.ModuleLinks.PlaceHolderType.three})
                ' DEFINISCO UNA CLASSE PER IL CONTAINER
                oDisplayAction.ContainerCSS = SubActivityCssClass(dtoItem)
                ' DIMENSIONI IMMAGINI
                oDisplayAction.IconSize = Helpers.IconSize.Small

                oDisplayAction.EnableAnchor = True
                If isVisible AndAlso _isVisibleByDateConstraint AndAlso CanProceedByEndDate Then
                    oDisplayAction.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction
                Else
                    oDisplayAction.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.text
                End If
                oDisplayAction.InitializeControl(initializer)
                oPlaceHolder.Controls.Add(oDisplayAction)

            End If

            If Not _isVisibleByDateConstraint Then
                '        Dim oSp As HtmlControl = e.Item.FindControl("SPsubActName")
                '        oSp.Attributes.Add("class", "row left timeConstraint")
            End If

        End If

    End Sub

    'Private Sub SetImgStatusSmall(ByRef oImg As System.Web.UI.WebControls.Image, ByRef status As StatusStatistic)
    '    'oImg.Visible = True
    '    'Dim tooltip As String
    '    'Select Case status
    '    '    Case StatusStatistic.CompletedPassed
    '    '        oImg.ImageUrl = RootObject.ImgStatusGreenSmall(Me.BaseUrl)
    '    '        tooltip = Resource.getValue("StatusStatistic." & StatusStatistic.CompletedPassed.ToString)
    '    '    Case StatusStatistic.Browsed
    '    '        oImg.ImageUrl = RootObject.ImgStatusGreySmall(Me.BaseUrl)
    '    '        tooltip = Resource.getValue("StatusStatistic." & StatusStatistic.Browsed.ToString)
    '    '    Case StatusStatistic.None
    '    '        oImg.ImageUrl = RootObject.ImgStatusGreySmall(Me.BaseUrl)
    '    '        tooltip = Resource.getValue("StatusStatistic." & StatusStatistic.Browsed.ToString)
    '    '    Case Else
    '    '        oImg.ImageUrl = RootObject.ImgStatusYellowSmall(Me.BaseUrl)
    '    '        tooltip = Resource.getValue("StatusStatistic." & StatusStatistic.Started.ToString)
    '    'End Select
    '    'oImg.AlternateText = tooltip
    '    'oImg.ToolTip = tooltip
    'End Sub


    Private Function SetStatus(status As StatusStatistic) As String
        Select Case status
            Case StatusStatistic.CompletedPassed
                Return "green"
                'tooltip = Resource.getValue("StatusStatistic." & StatusStatistic.CompletedPassed.ToString)
            Case StatusStatistic.Browsed
                Return "gray"
                'tooltip = Resource.getValue("StatusStatistic." & StatusStatistic.Browsed.ToString)
            Case StatusStatistic.None
                Return "gray"
                'tooltip = Resource.getValue("StatusStatistic." & StatusStatistic.Browsed.ToString)
            Case Else
                Return "yellow"
                'tooltip = Resource.getValue("StatusStatistic." & StatusStatistic.Started.ToString)
        End Select

    End Function

    Private Sub SetStatus(status As StatusStatistic, ByRef el As HtmlControl, Optional transparent As Boolean = False)
        Dim tooltip As String
        Dim color As String
        Select Case status
            Case StatusStatistic.CompletedPassed
                color = "green"
                tooltip = Resource.getValue("StatusStatistic." & StatusStatistic.CompletedPassed.ToString)
            Case StatusStatistic.Browsed
                color = "gray"
                tooltip = Resource.getValue("StatusStatistic." & StatusStatistic.Browsed.ToString)
            Case StatusStatistic.None
                color = "gray"
                tooltip = Resource.getValue("StatusStatistic." & StatusStatistic.Browsed.ToString)
            Case Else
                color = "yellow"
                tooltip = Resource.getValue("StatusStatistic." & StatusStatistic.Started.ToString)
        End Select
        Dim cssClass As String = el.Attributes("class")
        If Not transparent Then
            el.Attributes("title") = tooltip
            cssClass = Replace(cssClass, " " + "gray", "")
            cssClass = Replace(cssClass, " " + "yellow", "")
            cssClass = Replace(cssClass, " " + "green", "")
            el.Attributes("class") = cssClass + " " + color
        Else
            el.Attributes("title") = ""
            el.Attributes("class") = el.Attributes("class") + " hidden"
        End If
    End Sub

    Private Sub SingleSubActivity(dtoItem As dtoSubActivity, e As View)
        Dim oLb As Label
        Dim oLkb As LinkButton
        Dim oHyp As HyperLink
        Dim oImg As System.Web.UI.WebControls.Image



        'SetImgStatusSmall(e.FindControl("IMGstatus_single"), dtoItem.StatusStat)
        SetStatus(dtoItem.StatusStat, e.FindControl("IMGStatusActivity"))


        Dim oMand As HtmlControl = e.FindControl("ImgMandatorySingle")

        If ServiceEP.CheckStatus(dtoItem.Status, Status.Mandatory) Then

            'oImg.ImageUrl = RootObject.ImgMandatorySmall(Me.BaseUrl)
            'oImg.ToolTip = Me.Resource.getValue("ActivityMandatory")
            oMand.Attributes("title") = Me.Resource.getValue("ActivityMandatory")
            oMand.Visible = True
        Else
            'hideControl(oImg)
            oMand.Visible = False
        End If

        ''If ServiceEP.CheckStatus(dtoItem.Status, Status.Mandatory) Then
        ''    oImg.ImageUrl = RootObject.ImgMandatoryMedium(Me.BaseUrl)
        ''    oImg.ToolTip = Me.Resource.getValue("SubActivityMandatory")
        ''    oImg.Visible = True
        ''Else
        ''    hideControl(oImg)
        ''End If

        Dim isVisible As Boolean = ServiceEP.CheckStatus(dtoItem.Status, Status.NotLocked) And _canViewSubAct_byAct

        'SetImgBlocked(e.FindControl("IMGvisibility_single"), Not isVisible)


        ''oImg = e.Item.FindControl("IMGtype")
        ''oImg.ImageUrl = RootObject.ImgContentType(Me.BaseUrl, dtoItem.ContentType)


        'oLb = e.FindControl("LBpoints_single")

        Dim weight As String

        If isMarkEp Then
            'oLb.Text = " (" & dtoItem.Weight & Resource.getValue("Points")
            weight = " (" & dtoItem.Weight & Resource.getValue("Points")
        Else
            'hideControl(oLb)  ' oLb.Text = ServiceEP.GetTime(dtoItem.Weight)
            weight = ServiceEP.GetTime(dtoItem.Weight)

        End If

        'oLb = e.FindControl("LBnameTitle_single")
        'Me.Resource.setLabel(oLb)

        'oHyp = e.FindControl("HYPname_single")
        'oHyp.Text = dtoItem.Name
        'oHyp.NavigateUrl = dtoItem.Link

        'oLb = e.FindControl("LBdescription_single")

        'oLkb = e.FindControl("LKBexecute_single")

        Dim oPlaceHolder As PlaceHolder = e.FindControl("PHLAction_single")

        If ServiceEP.isSubActityInternalModule(dtoItem.ContentType) Then

            Dim initializer As New dtoInternalActionInitializer
            initializer.IdPath = CurrentPathId
            initializer.IdPathCommunity = Me.CurrentCommunityID
            initializer.CookieName = "PathCertifications_" & initializer.IdPath
            initializer.SubActivity = dtoItem
            ' AGGIUNTA PLACEHOLDER
            ' --> 
            initializer.PlaceHolders.Add(New lm.Comol.Core.ModuleLinks.dtoPlaceHolder() With {.Text = weight, .Type = lm.Comol.Core.ModuleLinks.PlaceHolderType.three, .CssClass = "duration"})
            ' DEFINISCO UNA CLASSE PER IL CONTAINER
            initializer.ContainerCSS = SubActivityCssClass(dtoItem)
            If isVisible AndAlso _isVisibleByDateConstraint AndAlso CanProceedByEndDate Then
                initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction
            Else
                initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.text
            End If
            Select Case dtoItem.ContentType
                Case SubActivityType.Text
                    Dim oDisplayAction As lm.Comol.Modules.EduPath.Presentation.IViewModuleTextAction = e.FindControl("CTRLtextAction")
                    e.FindControl("CTRLtextAction").Visible = True
                    oDisplayAction.RefreshContainer = True

                    ' DIMENSIONI IMMAGINI
                    oDisplayAction.IconSize = Helpers.IconSize.Small
                    oDisplayAction.EnableAnchor = True
                    'AddHandler oDisplayAction.RefreshContainerEvent, AddressOf RefreshContainerEvent
                    oDisplayAction.InitializeControl(initializer)
                    'oPlaceHolder.Controls.Add(oDisplayAction)
                Case SubActivityType.Certificate
                    Dim control As UC_CertificationAction = e.FindControl("CTRLcertificationAction")
                    control.Visible = True
                    control.EvaluablePath = Not isTimeEp()
                    Dim oCertAction As lm.Comol.Modules.EduPath.Presentation.IViewModuleCertificationAction = e.FindControl("CTRLcertificationAction")
                    'oCertAction.RefreshContainer = True
                    ' DIMENSIONI IMMAGINI
                    oCertAction.IconSize = Helpers.IconSize.Small
                    oCertAction.EnableAnchor = True


                    If dtoItem.AutoGenerated AndAlso dtoItem.StatusStat <> StatusStatistic.CompletedPassed Then
                        Dim newStatus As StatusStatistic = StatusStatistic.Browsed

                        control.AutoGenerateCertificationNEW(CurrentPathId, initializer.IdPathCommunity, CurrentUserId, dtoItem, DateTime.Now, StatusStatistic.Browsed, newStatus)
                        If newStatus <> StatusStatistic.Browsed Then
                            SetStatus(newStatus, e.FindControl("IMGStatusActivity"))
                            InitProgressBar(True)
                        End If
                    End If

                    'AddHandler oDisplayAction.RefreshContainerEvent, AddressOf RefreshContainerEvent
                    oCertAction.InitializeControl(initializer)
            End Select


            ''oLb.Text = Me.SmartTagsAvailable.TagAll(dtoItem.Description)
            'Dim oDisplayAction As lm.Comol.Modules.EduPath.Presentation.IViewModuleTextAction = e.FindControl("CTRLtextAction")
            'oDisplayAction.RefreshContainer = True
            'e.FindControl("CTRLtextAction").Visible = True
            ''Dim oDisplayAction As lm.Comol.Modules.EduPath.Presentation.IViewModuleTextAction = CType(LoadControl(BaseUrl & RootObject.DisplayTextActionControl), lm.Comol.Modules.EduPath.Presentation.IViewModuleTextAction)
            'Dim initializer As New dtoInternalActionInitializer
            'initializer.SubActivity = New dtoSubActivity() With {.Id = dtoItem.Id, .Description = dtoItem.Description}
            '' AGGIUNTA PLACEHOLDER
            '' --> 
            'initializer.PlaceHolders.Add(New lm.Comol.Core.ModuleLinks.dtoPlaceHolder() With {.Text = weight, .Type = lm.Comol.Core.ModuleLinks.PlaceHolderType.three, .CssClass = "duration"})
            '' DEFINISCO UNA CLASSE PER IL CONTAINER
            'initializer.ContainerCSS = SubActivityCssClass(dtoItem)
            '' DIMENSIONI IMMAGINI
            'oDisplayAction.IconSize = Helpers.IconSize.Small

            'oDisplayAction.EnableAnchor = True
            'If isVisible AndAlso _isVisibleByDateConstraint AndAlso CanProceedByEndDate Then
            '    initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction
            'Else
            '    initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.text
            'End If
            ''AddHandler oDisplayAction.RefreshContainerEvent, AddressOf RefreshContainerEvent
            'oDisplayAction.InitializeControl(initializer)
            ''oPlaceHolder.Controls.Add(oDisplayAction)


        Else


            e.FindControl("CTRLtextAction").Visible = False
            Dim oDisplayAction As lm.Comol.Core.ModuleLinks.IExternalModuleDisplayAction = CType(LoadControl(BaseUrl & lm.Comol.Core.DomainModel.Common.CoreRootObject.GenericNewDisplayActionControl), lm.Comol.Core.ModuleLinks.IExternalModuleDisplayAction)
            'hideControl(oLkb)
            'oLb.Text = Me.SmartTagsAvailable.TagAll(dtoItem.Description)


            'If isVisible AndAlso _isVisibleByDateConstraint AndAlso CanProceedByEndDate Then
            '    oDisplayAction.InitializeControlInlineByLink(dtoItem.ModuleLink)
            'Else

            '    oDisplayAction.InitializeRemoteControlInlineByLink(True, dtoItem.ModuleLink)
            'End If

            Dim initializer As New lm.Comol.Core.ModuleLinks.dtoExternalModuleInitializer
            initializer.Link = dtoItem.ModuleLink
            ' AGGIUNTA PLACEHOLDER
            ' --> initializer.PlaceHolders.Add(New lm.Comol.Core.ModuleLinks.dtoPlaceHolder() With {.Text = "HH:ss", .Type = lm.Comol.Core.ModuleLinks.PlaceHolderType.three})
            initializer.PlaceHolders.Add(New lm.Comol.Core.ModuleLinks.dtoPlaceHolder() With {.Text = weight, .Type = lm.Comol.Core.ModuleLinks.PlaceHolderType.three, .CssClass = "duration"})
            ' DEFINISCO UNA CLASSE PER IL CONTAINER
            oDisplayAction.ContainerCSS = SubActivityCssClass(dtoItem)
            ' DIMENSIONI IMMAGINI
            oDisplayAction.IconSize = Helpers.IconSize.Small

            oDisplayAction.EnableAnchor = True
            If isVisible AndAlso _isVisibleByDateConstraint AndAlso CanProceedByEndDate Then
                oDisplayAction.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction
            Else
                oDisplayAction.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.text
            End If
            oDisplayAction.InitializeControl(initializer)
            oPlaceHolder.Controls.Add(oDisplayAction)

            'oPlaceHolder.Controls.Add(oDisplayAction)

        End If

        If Not _isVisibleByDateConstraint Then
            Dim oSp As HtmlControl = e.FindControl("SPsubActName_single")
            If (Not oSp Is Nothing) Then
                oSp.Attributes.Add("class", "row left timeConstraint")
            End If
        End If
    End Sub

    Private Sub SetLbActName(ByRef oLB As Label, ByRef name As String)
        oLB.Text = name
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

    Protected ReadOnly Property SingleMulti(item As dtoActivity) As String
        Get
            If CouldActivityWithSingleSubActivityBeOmitted And item.SubActivities.Count = 1 Then
                Return " single"
            Else
                Return ""
            End If
        End Get
    End Property

    Protected ReadOnly Property ActivityNote(item As dtoActivity) As String
        Get
            If ServiceEP.CheckStatus(item.Status, Status.Text) Then
                Return " note"
            Else
                Return ""
            End If
        End Get
    End Property

    Protected ReadOnly Property UnitNote(item As dtoUnit) As String
        Get
            If ServiceEP.CheckStatus(item.Status, Status.Text) Then
                Return " note"
            Else
                Return ""
            End If
        End Get
    End Property

    Protected ReadOnly Property LockedUnit(item As dtoUnit) As String
        Get
            If ServiceEP.CheckStatus(item.Status, Status.Locked) Then
                Return " locked"
            Else
                Return ""
            End If
        End Get
    End Property

    Protected ReadOnly Property LockedActivity(item As dtoActivity) As String
        Get
            If ServiceEP.CheckStatus(item.Status, Status.Locked) Then
                Return " locked"
            Else
                Return ""
            End If
        End Get
    End Property

    Protected ReadOnly Property AutoOpen(item As dtoUnit) As String
        Get
            If Not ServiceStat.UnitIsCompleted(item.Activities) OrElse ProeloadOpenIdUnit = item.Id Then
                Return " autoOpen"
            Else
                Return " autoClose"
            End If
        End Get
    End Property

    'Public Enum SubActivityType
    '{
    '   None=0,
    '   Quiz=1,
    '   Forum=2,
    '   File=3,
    '   Wiki=4,
    '   Text = 5
    '}

    Protected Friend Function GetBlockUiMessage() As String
        Return Resource.getValue("GetFileBlockUiMessage")
    End Function

    Protected Overrides Sub NotifyModuleStatus(status As lm.Comol.Core.DomainModel.ModuleStatus)
        CTRLmoduleStatusMessage.Visible = True
        CTRLmoduleStatusMessage.InitializeControl(Resource.getValue("EduPath.ModuleStatus." & status.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub

    Protected Overrides Sub NotifyUnavailableModule(status As lm.Comol.Core.DomainModel.ModuleStatus)
        Master.ServiceTitle = ServiceEP.GetPathName(CurrentPathId)
        MLVeduPathView.SetActiveView(VIWmessages)
        CTRLgenericMessages.InitializeControl(Resource.getValue("EduPath.ModuleStatus." & status.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
    End Sub

    Protected Overrides ReadOnly Property CheckModuleStatus As Boolean
        Get
            Return True
        End Get
    End Property
End Class