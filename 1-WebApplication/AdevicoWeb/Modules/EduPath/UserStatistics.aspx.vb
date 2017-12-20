Imports lm.Comol.Modules.EduPath.Domain
Imports lm.Comol.Modules.EduPath.BusinessLogic
Imports lm.Comol.UI.Presentation
Imports COL_BusinessLogic_v2.UCServices
Imports lm.ActionDataContract
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel

Public Class UserStatistics
    Inherits PageBaseEduPath



#Region "Property"

    Private _endDateSet As Boolean = False
    Private _EpEndDate As DateTime?
    Private ReadOnly Property EpEndDate As DateTime?
        Get
            If Not _endDateSet Then
                _EpEndDate = ServiceEP.GetEpEndDate(PathID)
            End If
            Return _EpEndDate
        End Get
    End Property

    Private ReadOnly Property TimeStat As DateTime
        Get
            Dim qs_time As String = Request.QueryString("ST")
            If IsNumeric(qs_time) Then
                Return New DateTime(qs_time)
            Else
                Return DateTime.Now
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadShowAll As Boolean
        Get
            Dim qs_ShowAll As String = Request.QueryString("ShowAll")
            If Not String.IsNullOrEmpty(qs_ShowAll) Then
                qs_ShowAll = qs_ShowAll.ToLower
            End If

            Return (Request.QueryString("ShowAll") = "true")
        End Get
    End Property
    Protected Overrides ReadOnly Property PathType As EPType
        Get
            If _PathType = lm.Comol.Modules.EduPath.Domain.EPType.None Then
                _PathType = ServiceEP.GetEpType(PathID, ItemType.Path)
            End If
            Return _PathType
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

    'Private ReadOnly Property UnitId As Long
    '    Get
    '        If Type = ItemType.Unit Then
    '            Dim uId As Long

    '            Long.TryParse(Request.QueryString("IId"), uId)

    '            Return uId
    '        End If
    '        Return -1
    '    End Get
    'End Property

    Private ReadOnly Property PageIndex As Int16
        Get
            Dim qs_index As String = Request.QueryString("Pidx")
            If IsNumeric(qs_index) Then
                Return qs_index
            Else
                Return 0
            End If
        End Get
    End Property

    Private ReadOnly Property Type As ItemType
        Get
            Dim qs_type As String = Request.QueryString("Itype")
            If IsNumeric(qs_type) Then

                Select Case qs_type
                    Case ItemType.Unit
                        Return ItemType.Unit
                    Case ItemType.Path
                        Return ItemType.Path
                    Case Else
                        Return ItemType.None
                End Select
            Else
                Return ItemType.None
            End If
        End Get
    End Property

    Private _itemId As Long
    Private ReadOnly Property ItemId As Long
        Get
            If _itemId = 0 Then
                Long.TryParse(Request.QueryString("IId"), _itemId)
            End If
            Return _itemId
        End Get
    End Property

    Private _pathId As Long
    Private ReadOnly Property PathID() As Long
        Get
            If _pathId = 0 Then
                Select Case Type
                    Case ItemType.Path
                        _pathId = ItemId
                    Case ItemType.Unit
                        _pathId = ServiceEP.GetPathId_ByUnitId(ItemId)
                    Case ItemType.Activity
                        _pathId = ServiceEP.GetPathId_ByActivityId(ItemId)
                    Case ItemType.SubActivity
                        _pathId = ServiceEP.GetPathId_BySubActivityId(ItemId)

                End Select
            End If

            Return _pathId
        End Get
    End Property

    Private _UserToViewStat As Integer
    Private ReadOnly Property UserToViewStat As Integer
        Get
            Select Case Me.ViewModeType
                Case EpViewModeType.Manage
                    Dim qs_userToview As String = Request.QueryString("UsId")
                    If IsNumeric(qs_userToview) Then
                        Return qs_userToview
                    Else
                        Return -1
                    End If
                Case EpViewModeType.View
                    Return Me.CurrentUserId
                Case Else
                    Return -1
            End Select
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
    Protected ReadOnly Property CookieName() As String
        Get
            Return "COMOL_UserStatistics"
        End Get
    End Property
    Protected ReadOnly Property DisplayMessageToken() As String
        Get
            Return Resource.getValue("DisplayMessageToken.UserStatistics")
        End Get
    End Property
    Protected ReadOnly Property DisplayTitleToken() As String
        Get
            Return Resource.getValue("DisplayTitleToken.UserStatistics")
        End Get
    End Property
#End Region

    Dim CouldActivityWithSingleSubActivityBeOmitted As Boolean = False

#Region " Base"
    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_EduPath.Codex)
        CouldActivityWithSingleSubActivityBeOmitted = True ' EduPathConfiguration.UseSingleActionView
        Me.Master.ShowDocType = True
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides Sub BindDati()
        UpdateScormStat()

        LTRNavUnitTop.Text = Me.Resource.getValue("Unit")
        LTRNavUnitBottom.Text = Me.Resource.getValue("Unit")
        LTRNavActivityTop.Text = Me.Resource.getValue("Activity")
        LTRNavActivityBottom.Text = Me.Resource.getValue("Activity")
        LTRNavSubActivityTop.Text = Me.Resource.getValue("SubActivity")
        LTRNavSubActivityBottom.Text = Me.Resource.getValue("SubActivity")
        'SPANexportStatistics.InnerHtml = Me.Resource.getValue("ExportStatistics")
        SPANviewStatistics.InnerHtml = Me.Resource.getValue("ViewStatistics")

        Dim currentUserPermission As PermissionEP = Me.ServiceEP.GetUserPermission_ByPath(Me.PathID, Me.CurrentUserId, Me.CurrentCommRoleID)
        Me.Master.ServiceTitle = Me.Resource.getValue("UserEpStat")

        'ABILITARE IN CASO DI OVERTIME
        ' Dim dtoDate As dtoEndTime = ServiceEP.GetEpTime(PathID, UserToViewStat)

        '  Dim IsCertifiedTime As Boolean = IIf(dtoDate.CertifiedTime Is Nothing, False, IIf(Not dtoDate.CertifiedTime Is Nothing AndAlso dtoDate.CertifiedTime = TimeStat, True, False))
        Dim isManageMode As Boolean = Me.ViewModeType = EpViewModeType.Manage
        ' Me.CTRLselTime.InitView(TimeStat, dtoDate.EndDateStr, dtoDate.OverEndDateStr, isManageMode, isManageMode, dtoDate.EndDate)
        Me.DVdateFilter.Visible = isManageMode
        Me.CTRLselTime.InitView(TimeStat, IIf(EpEndDate Is Nothing, "", ServiceEP.GetDate(EpEndDate)), "", isManageMode, isManageMode, EpEndDate)

        Me.InitHyperLink()

        If (Type <> lm.Comol.Modules.EduPath.Domain.ItemType.Path) OrElse (Me.ViewModeType <> EpViewModeType.Manage) Then
            LNBexportUserStatisticfullToXml.Visible = False
            LNBexportUserStatisticfullToCsv.Visible = False
            LNBexportUserStatisticfullCertificationToXml.Visible = False
            LNBexportUserStatisticfullCertificationToCsv.Visible = False
        End If

        Select Case Me.ViewModeType

            Case EpViewModeType.Manage
                If Me.UserToViewStat > 0 Then
                    If currentUserPermission.ViewUserStat Then
                        Me.PageUtility.AddAction(Me.CurrentCommunityID, Services_EduPath.ActionType.ViewUserStat, Me.PageUtility.CreateObjectsList(Services_EduPath.ObjectType.EduPath, Me.PathID.ToString), InteractionType.UserWithLearningObject)
                        InitPage_Manage()
                    Else
                        Me.ShowError(EpError.NotPermission)
                    End If

                Else
                    Me.ShowError(EpError.UserNotInEp)
                End If
            Case EpViewModeType.View
                If currentUserPermission.ViewOwnStat Then
                    Me.PageUtility.AddAction(Me.CurrentCommunityID, Services_EduPath.ActionType.ViewOwnStat, Me.PageUtility.CreateObjectsList(Services_EduPath.ObjectType.EduPath, Me.PathID.ToString), InteractionType.UserWithLearningObject)
                    InitPage_View()
                Else
                    Me.ShowError(EpError.NotPermission)
                End If
            Case Else
                Me.ShowError(EpError.Url)
        End Select

    End Sub


    Public Overrides Sub BindNoPermessi()
        Me.ShowError(EpError.NotPermission)
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        'Dim value As Boolean
        'With Me.CurrentService
        '    value = .Admin
        'End With
        'Return value
        Return ServiceEP.HasPermessi_ByItem(Of Path)(PathID, Me.CurrentCommunityID)
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Stat", "EduPath")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLinkButton(Me.LNBexportUserStatisticToCsv, False, True)
            .setLinkButton(Me.LNBexportUserStatisticToXml, False, True)
            .setLinkButton(Me.LNBexportUserStatisticToPdf, False, True)
            .setLinkButton(Me.LNBexportUserStatisticfullToCsv, False, True)
            .setLinkButton(Me.LNBexportUserStatisticfullToXml, False, True)
            .setLinkButton(Me.LNBexportUserStatisticfullCertificationToCsv, False, True)
            .setLinkButton(Me.LNBexportUserStatisticfullCertificationToXml, False, True)

            .setHyperLink(Me.HYPeduPathView, False, True)
            .setHyperLink(Me.HYPuserStat, False, True)
            .setHyperLink(Me.HYPpathStat, False, True)
            .setHyperLink(Me.HYPback, False, True)
            .setLabel(Me.LBnameTit)
            .setLabel(Me.LBmarkTit)
            If isTimeEp Then
                .setLabel_To_Value(Me.LBcompletionTit, "LBcompletionTit.time.text")
            Else
                .setLabel_To_Value(Me.LBcompletionTit, "LBcompletionTit.percent.text")
            End If
            .setLabel(Me.LBstatusTit)
            .setLabel(Me.LBactionsTit)
            setInfoDialog()

            If Not Type = lm.Comol.Modules.EduPath.Domain.ItemType.SubActivity Then
                LBmarkTit.Text &= .getValue("Min")
                LBcompletionTit.Text &= .getValue("Min")
            End If
        End With
    End Sub



    Public Sub setInfoDialog()
        With Me.Resource

            If isAutoEp Then
                Me.TRonlyCompletedAct.Visible = False
                Me.TRonlyCompletedPath.Visible = False
                Me.TRonlyPassedAct.Visible = False
                Me.TRonlyPassedPath.Visible = False

                Me.LbPassComplPath.Text = .getValue("LbPassComplUnit.text")
                Me.LbTotMandPath.Text = .getValue("LbTotMandUnit.text")
            Else
                .setLabel(Me.LbOnlyCompAct)
                .setLabel(Me.LbOnlyPassAct)

                .setLabel(Me.LbTotMandUnit)
                .setLabel(Me.LbOnlyCompUnit)
                .setLabel(Me.LbOnlyPassUnit)
                .setLabel(Me.LbPassComplUnit)

                .setLabel(Me.LbPassComplPath)
                .setLabel(Me.LbTotMandPath)
            End If
            .setLabel(Me.LbTotMandAct)
            .setLabel(Me.LbPassComplAct)

            .setLabel(Me.LbOnlyCompPath)
            .setLabel(Me.LbOnlyPassPath)

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



    Private Sub UpdateScormStat()
        Dim ModuleLinkIds As IList(Of Long) = ServiceEP.GetMaterialModuleLinkIds_ByPathId(Me.PathID)
        If ModuleLinkIds.Count > 0 Then
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

    Public Sub RP_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs)
        Try

            Select Case e.CommandName
                Case "blockedU"
                    If ServiceEP.UpdateUnitVisibilityStatus(e.CommandArgument, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then
                        BindDati()
                    Else
                        ShowError(EpError.Generic)
                    End If
                Case "blockedA"
                    If ServiceEP.UpdateActivityVisibilityStatus(e.CommandArgument, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then
                        BindDati()
                    Else
                        ShowError(EpError.Generic)
                    End If

            End Select

        Catch ex As Exception

        End Try
    End Sub

    Private Sub LKBblocked_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBblocked.Click
        If ServiceEP.UpdatePathVisibilityStatus(Me.LKBblocked.CommandArgument, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then
            BindDati()
        Else
            ShowError(EpError.Generic)
        End If
    End Sub

    Private Sub SetBlockedStatusLkb(ByRef oLkb As LinkButton, ByVal isBlocked As Boolean, ByRef itemID As Long)

        Me.Resource.setLinkButton(oLkb, False, True)
        oLkb.CommandArgument = itemID
        oLkb.Text = "<img src=" & """" & "{0}" & """" & " alt=" & """" & "{1}" & """" & "/>"
        oLkb.Visible = True
        oLkb.Text = String.Format(oLkb.Text, RootObject.ImgBtnBlocked(Me.BaseUrl, Not isBlocked), oLkb.ToolTip)

    End Sub

    Private Sub setBlockedStatusImg(ByRef oImg As System.Web.UI.WebControls.Image, ByVal isBlocked As Boolean)
        oImg.Visible = True
        oImg.ImageUrl = RootObject.ImgItemBlocked(Me.BaseUrl, Not isBlocked)
        oImg.AlternateText = Resource.getValue("Blocked." & isBlocked)
    End Sub

    Private Sub setBlockedStatusImg(ByRef oImg As Label, ByVal isBlocked As Boolean)
        oImg.Visible = True
        'oImg.ImageUrl = RootObject.ImgItemBlocked(Me.BaseUrl, Not isBlocked)
        oImg.ToolTip = Resource.getValue("Blocked." & isBlocked)
        If isBlocked Then
            oImg.CssClass = "icon locked"
        Else
            oImg.CssClass = "icon unlocked"
        End If
    End Sub

    Private Sub InitProgressBar()
        Dim statForBar As dtoStatWithWeight
        If isAutoEp Then
            statForBar = ServiceStat.GetPassedCompletedWeight_byActivity(Me.PathID, Me.UserToViewStat, TimeStat)
        Else
            statForBar = ServiceStat.GetPassedCompletedWeight_byUnit(Me.PathID, Me.UserToViewStat, TimeStat)
        End If

        Me.CTRLprogressBar.InitBar(statForBar.Completion, statForBar.MinCompletion, Me.Resource.getValue("ExecutedWeight") & statForBar.UserTotWeight)
    End Sub

    Private Function setMandatoryItem(ByRef dtoStat As dtoItemWithStatistic) As String
        If isAutoEp Then
            Return dtoStat.TotMandatory & "#" & dtoStat.CompletedPassedMandatory & "#" & dtoStat.CompletedPassedMandatory & "#" & dtoStat.CompletedPassedMandatory

        Else
            Return dtoStat.TotMandatory & "#" & dtoStat.OnlyCompletedMandatory & "#" & dtoStat.OnlyPassedMandatory & "#" & dtoStat.CompletedPassedMandatory

        End If
    End Function

    Private Sub SetBlockedStatus(ByRef dto As dtoItemWithStatistic)
        If dto.canUpdate Then
            SetBlockedStatusLkb(Me.LKBblocked, ServiceEP.CheckStatus(dto.Status, Status.Locked), dto.Id)
            Me.LKBblocked.CssClass = "icon "
            If ServiceEP.CheckStatus(dto.Status, Status.Locked) Then
                Me.LKBblocked.CssClass += "locked"
            Else
                Me.LKBblocked.CssClass += "unlocked"
            End If
        Else
            setBlockedStatusImg(Me.IMGblocked, ServiceEP.CheckStatus(dto.Status, Status.Locked))

            Me.IMGblocked.CssClass = "icon "
            'Me.IMGblocked.ImageUrl = ""
            If ServiceEP.CheckStatus(dto.Status, Status.Locked) Then
                Me.IMGblocked.CssClass += "locked"
            Else
                Me.IMGblocked.CssClass += "unlocked"
            End If
        End If
    End Sub

    Private Sub InitPage_Manage()
        Dim PathStatistics As dtoItemWithStatistic = Me.ServiceEP.ServiceStat.GetParticipantEduPathStatisticStructure(Me.PathID, Me.CurrentUserId, Me.CurrentCommRoleID, Me.UserToViewStat, TimeStat)

        If Not IsNothing(PathStatistics) Then
            'Dim hasCertification As Boolean = (Type = lm.Comol.Modules.EduPath.Domain.ItemType.Path) AndAlso ServiceEP.PathHasCertificateWithQuiz(Me.PathID)
            'Dim hasQuestionnaires As Boolean = (Type = lm.Comol.Modules.EduPath.Domain.ItemType.Path) AndAlso ServiceEP.PathHasSubActivityType(Me.PathID, SubActivityType.Quiz)

            'LNBexportUserStatisticfullCertificationToCsv.Visible = hasCertification
            'LNBexportUserStatisticfullCertificationToXml.Visible = hasCertification
            'LNBexportUserStatisticfullToXml.Visible = hasQuestionnaires
            'LNBexportUserStatisticfullToCsv.Visible = hasQuestionnaires

            Me.LBuserName.Text = Me.ServiceEP.GetUserName(Me.UserToViewStat)
            SetBlockedStatus(PathStatistics)
            Me.InitProgressBar()
            Me.InitPathStat(PathStatistics)
            Me.RPunit.DataSource = PathStatistics.Children
            Me.RPunit.DataBind()
        Else
            DIVexport.Visible = False
            Me.ShowError(EpError.NotPermission)
        End If
    End Sub

    Private Sub InitPage_View()

        Dim PathStatistics As dtoItemWithStatistic = Me.ServiceEP.ServiceStat.GetOwnerEduPathStatisticStructure(Me.PathID, Me.CurrentUserId, Me.CurrentCommRoleID, TimeStat)

        If Not IsNothing(PathStatistics) Then
            Me.LBuserName.Text = Me.ServiceEP.GetUserName(Me.UserToViewStat)
            Me.InitPathStat(PathStatistics)
            SetBlockedStatus(PathStatistics)
            Me.InitProgressBar()
            Me.RPunit.DataSource = PathStatistics.Children
            Me.RPunit.DataBind()
        Else
            DIVexport.Visible = False
            Me.ShowError(EpError.NotPermission)
        End If
    End Sub

    Private Sub InitHyperLink()

        Select Case Me.ViewModeType

            Case EpViewModeType.Manage
                'Me.HYPpathStat.ImageUrl = RootObject.ImgEduPathStat(Me.BaseUrl)
                Me.HYPpathStat.Visible = True
                Me.HYPpathStat.NavigateUrl = Me.BaseUrl & RootObject.PathStatistics(PathID, Me.CurrentCommunityID, TimeStat, PreloadShowAll)

                'Me.HYPuserStat.ImageUrl = RootObject.ImgUsersStat(Me.BaseUrl)
                Me.HYPuserStat.NavigateUrl = Me.BaseUrl & RootObject.UsersStatistics(Me.PathID, Me.CurrentCommunityID, ItemType.Path, PageIndex, TimeStat, PreloadShowAll)

                Me.HYPback.NavigateUrl = Me.BaseUrl & RootObject.UsersStatistics(ItemId, CurrentCommunityID, Type, PageIndex, TimeStat, PreloadShowAll)

            Case EpViewModeType.View
                Me.HYPback.NavigateUrl = Me.BaseUrl & RootObject.EduPathList(Me.CurrentCommunityID, EpViewModeType.View)

                Dim oPermission As PermissionEP = ServiceEP.GetUserPermission_ByPath(PathID, CurrentUserId, CurrentCommRoleID)
                Dim onlystats As Boolean = oPermission.Read = False And oPermission.ViewOwnStat = True

                If onlystats Then
                    HYPeduPathView.Visible = False
                End If

        End Select
        Me.HYPeduPathView.NavigateUrl = Me.BaseUrl & RootObject.PathView(PathID, Me.CurrentCommunityID, Me.ViewModeType, ServiceEP.isPlayModePath(PathID))
        If (Request.QueryString("ReturnUrl") = "PathSummary") Then
            Me.HYPback.NavigateUrl = Me.BaseUrl & RootObject.PathSummary(Me.CurrentCommunityID, UserToViewStat, PathID)
        End If

    End Sub

    Private Sub SetLbValueWithMinValueFormat(ByRef oLb As Label, ByRef value As String, ByRef minValue As String)
        oLb.Text = value & " (" & minValue & ")"
        'If (value >= minValue) Then
        '    oLb.CssClass = oLb.CssClass & " Ep_ItemGreen"
        'ElseIf (value > 0 AndAlso minValue > 0) Then
        '    oLb.CssClass = oLb.CssClass & "Ep_ItemYellow"
        'Else
        '    oLb.CssClass = oLb.CssClass & "Ep_ItemRed"
        'End If
    End Sub

    Private Sub SetLbStatus(ByRef oLb As Label, ByRef status As StatusStatistic)
        oLb.Text = Me.Resource.getValue("StatusStatistic." & status.ToString)

        Select Case status
            Case StatusStatistic.CompletedPassed
                oLb.CssClass = oLb.CssClass & " Ep_ItemGreen"
            Case StatusStatistic.None
                oLb.CssClass = oLb.CssClass & "Ep_ItemRed"
            Case StatusStatistic.Browsed
                oLb.CssClass = oLb.CssClass & "Ep_ItemRed"
            Case Else
                oLb.CssClass = oLb.CssClass & "Ep_ItemYellow"
        End Select
    End Sub

    Protected Function HiddenStatus()
        If isAutoEp Then
            Return " hidden"
        Else
            Return ""
        End If
    End Function

    Private Sub CompletionClass(control As HtmlControl, uncomplete As Boolean)
        If Not uncomplete Then
            control.Attributes("class") = control.Attributes("class") + " yes"
        Else
            control.Attributes("class") = control.Attributes("class") + " no"
        End If
    End Sub

    Protected Function SetStatusSubAct(dtoItem As dtoSubActivityStatistic) As String
        Select Case dtoItem.StatusStat
            Case StatusStatistic.CompletedPassed
                Return "completedpassed"
                'oLb.CssClass = oLb.CssClass & " Ep_ItemGreen"
            Case StatusStatistic.None
                Return "notstarted"
                'oLb.CssClass = oLb.CssClass & "Ep_ItemRed"
            Case StatusStatistic.Browsed
                'oLb.CssClass = oLb.CssClass & "Ep_ItemRed"
                Return "notstarted"
            Case Else
                Return "started"
                'oLb.CssClass = oLb.CssClass & "Ep_Item7Yellow"
        End Select
    End Function

    Protected Function SetStatus(dtoItem As dtoItemWithStatistic) As String
        'oLb.Text = Me.Resource.getValue("StatusStatistic." & status.ToString)
        Select Case dtoItem.StatusStat
            Case StatusStatistic.CompletedPassed
                Return "completedpassed"
                'oLb.CssClass = oLb.CssClass & " Ep_ItemGreen"
            Case StatusStatistic.None
                Return "notstarted"
                'oLb.CssClass = oLb.CssClass & "Ep_ItemRed"
            Case StatusStatistic.Browsed
                'oLb.CssClass = oLb.CssClass & "Ep_ItemRed"
                Return "notstarted"
            Case Else
                Return "started"
                'oLb.CssClass = oLb.CssClass & "Ep_Item7Yellow"
        End Select
    End Function

    Protected Function SetStatusStringSubAct(dtoItem As dtoSubActivityStatistic) As String
        'oLb.Text = Me.Resource.getValue("StatusStatistic." & status.ToString)
        Select Case dtoItem.StatusStat
            Case StatusStatistic.CompletedPassed
                Return Me.Resource.getValue("StatusStatistic.CompletedPassed") '  "Completed Passed"
                'oLb.CssClass = oLb.CssClass & " Ep_ItemGreen"
            Case StatusStatistic.None
                Return Me.Resource.getValue("StatusStatistic.None") '"Not Started"
                'oLb.CssClass = oLb.CssClass & "Ep_ItemRed"
            Case StatusStatistic.Browsed
                'oLb.CssClass = oLb.CssClass & "Ep_ItemRed"
                Return Me.Resource.getValue("StatusStatistic.None") '"Not Started"
            Case Else
                Return Me.Resource.getValue("StatusStatistic.Started") '"Started"
                'oLb.CssClass = oLb.CssClass & "Ep_Item7Yellow"
        End Select
    End Function

    Protected Function SetStatusString(dtoItem As dtoItemWithStatistic) As String
        'oLb.Text = Me.Resource.getValue("StatusStatistic." & status.ToString)
        Select Case dtoItem.StatusStat
            Case StatusStatistic.CompletedPassed
                Return Me.Resource.getValue("StatusStatistic.CompletedPassed") '  "Completed Passed"
                'oLb.CssClass = oLb.CssClass & " Ep_ItemGreen"
            Case StatusStatistic.None
                Return Me.Resource.getValue("StatusStatistic.None") '"Not Started"
                'oLb.CssClass = oLb.CssClass & "Ep_ItemRed"
            Case StatusStatistic.Browsed
                'oLb.CssClass = oLb.CssClass & "Ep_ItemRed"
                Return Me.Resource.getValue("StatusStatistic.None") '"Not Started"
            Case Else
                Return Me.Resource.getValue("StatusStatistic.Started") '"Started"
                'oLb.CssClass = oLb.CssClass & "Ep_Item7Yellow"
        End Select
    End Function


    Private Sub InitPathStat(ByRef pathStat As dtoItemWithStatistic)

        If pathStat.Mark > -1 Then

            If isAutoEp Then
                Me.TDmark.Visible = False
                Me.TDmarkTit.Visible = False
            Else
                SetLbValueWithMinValueFormat(Me.LBmark, pathStat.Mark, pathStat.MinMark)
                SetImgColor(IMGmark, pathStat.Mark < pathStat.MinMark)
            End If

            If isTimeEp Then
                SetLbValueWithMinValueFormat(LBcompletion, ServiceEP.GetTime(pathStat.Completion), ServiceEP.GetMinTime(pathStat.Weight, pathStat.MinCompletion))
                Me.LBname.Text = pathStat.Name
                Me.LBweight.Text = " (" & ServiceEP.GetTime(pathStat.Weight) & ")"
                'SetImgColor(IMGcompletion, pathStat.Completion < (pathStat.Weight * pathStat.MinCompletion / 100))
                CompletionClass(SPANCompletion, pathStat.Completion < (pathStat.Weight * pathStat.MinCompletion / 100))

            Else

                Me.LBname.Text = pathStat.Name
                Me.LBweight.Text = ""
                SetLbValueWithMinValueFormat(LBcompletion, pathStat.Completion, pathStat.MinCompletion)
                CompletionClass(SPANCompletion, pathStat.Completion < pathStat.MinCompletion)
                'SetImgColor(IMGcompletion, pathStat.Completion < pathStat.MinCompletion)

            End If

        End If

        If pathStat.TotMandatory > 0 Then
            If isAutoEp Then
                LBpathMandatoryItem.Text = pathStat.TotMandatory & "#" & pathStat.CompletedPassedMandatory & "#" & pathStat.CompletedPassedMandatory & "#" & pathStat.CompletedPassedMandatory
            Else
                LBpathMandatoryItem.Text = pathStat.TotMandatory & "#" & pathStat.OnlyCompletedMandatory & "#" & pathStat.OnlyPassedMandatory & "#" & pathStat.CompletedPassedMandatory
            End If
            'Me.IMGmandatory.Visible = True
            Me.SPANPathMandatory.Visible = True
            'IMGmandatory.ImageUrl = RootObject.ImgMandatorySubItem(Me.BaseUrl)
            'Me.Resource.setImage(IMGmandatory, True)
        End If

        SPANStatus.Visible = True
        SPANStatus.InnerHtml = SetStatusString(pathStat)
        SPANStatus.Attributes("class") += " " + SetStatus(pathStat)

        SetLbStatus(Me.LBstatus, pathStat.StatusStat)
    End Sub

    Private Sub ShowError(ByVal errorType As EpError)
        Me.MLVstat.SetActiveView(Me.VIWerror)
        Me.Resource.setHyperLink(Me.HYPerror, False, True)
        Me.HYPerror.NavigateUrl = Me.BaseUrl & RootObject.EduPathList(Me.CurrentCommunityID, Me.ViewModeType)
        Select Case errorType
            Case EpError.Generic
                Me.LBerror.Text = Me.Resource.getValue("Error." & EpError.Generic.ToString)
                Me.PageUtility.AddAction(Services_EduPath.ActionType.GenericError, Nothing, InteractionType.UserWithLearningObject)
            Case EpError.NotPermission
                Me.LBerror.Text = Me.Resource.getValue("Error." & EpError.NotPermission.ToString)
                Me.PageUtility.AddAction(Services_EduPath.ActionType.NoPermission, Nothing, InteractionType.UserWithLearningObject)
            Case EpError.Url
                Me.LBerror.Text = Me.Resource.getValue("Error." & EpError.Url.ToString)
                Me.PageUtility.AddAction(Services_EduPath.ActionType.GenericError, Nothing, InteractionType.UserWithLearningObject)
        End Select

    End Sub



    Private Sub RPunit_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPunit.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oImg As System.Web.UI.WebControls.Image
            Dim oImg1 As Label
            Dim dtoItem As dtoItemWithStatistic = e.Item.DataItem

            If Not dtoItem.Children Is Nothing Then
                For Each item As dtoItemWithStatistic In dtoItem.Children
                    item.parentId = dtoItem.Id
                Next
            End If

            Dim oLb As Label
            Dim oLbw As Label

            oLb = e.Item.FindControl("LBname")
            oLbw = e.Item.FindControl("LBweight")
            If isTimeEp Then
                oLb.Text = dtoItem.Name ' & " (" & ServiceEP.GetTime(dtoItem.Weight) & ")"
            Else
                oLb.Text = dtoItem.Name
                oLbw.Text = " (" & dtoItem.Weight & "pt)"
            End If

            If ServiceEP.CheckStatus(dtoItem.Status, Status.Mandatory) Then
                'oImg = e.Item.FindControl("IMGisMandatory")
                'oImg.Visible = True
                'oImg.ImageUrl = RootObject.ImgMandatorySmall(Me.BaseUrl)
                'oImg.AlternateText = Resource.getValue("IMGmandatory.AlternateText")
                Dim oImgL As Label = e.Item.FindControl("IMGisMandatory")
                oImgL.ToolTip = ""
                oImgL.Visible = True
            End If

            If Not isAutoEp Then
                If dtoItem.Mark > -1 Then

                    oLb = e.Item.FindControl("LBmark")
                    SetLbValueWithMinValueFormat(oLb, dtoItem.Mark, dtoItem.MinMark)
                    oImg = e.Item.FindControl("IMGmark")
                    SetImgColor(oImg, dtoItem.Mark < dtoItem.MinMark)

                    oLb = e.Item.FindControl("LBcompletion")
                    SetLbValueWithMinValueFormat(oLb, dtoItem.Completion, dtoItem.MinCompletion)

                    'oImg = e.Item.FindControl("IMGcompletion")
                    'If isTimeEp Then
                    '    SetImgColor(oImg, dtoItem.Completion < (dtoItem.Weight * dtoItem.MinCompletion / 100))
                    'Else
                    '    SetImgColor(oImg, dtoItem.Completion < dtoItem.MinCompletion)
                    'End If

                    Dim spancom As HtmlControl
                    spancom = e.Item.FindControl("SPANCompletion")
                    If isTimeEp Then
                        CompletionClass(spancom, dtoItem.Completion < (dtoItem.Weight * dtoItem.MinCompletion / 100))
                    Else
                        CompletionClass(spancom, dtoItem.Completion < dtoItem.MinCompletion)
                    End If

                    oLb = e.Item.FindControl("LBstatus")
                    SetLbStatus(oLb, dtoItem.StatusStat)
                End If

                Dim colum As HtmlControl = e.Item.FindControl("TDmark")
                colum.Visible = True

                If dtoItem.TotMandatory > 0 Then
                    oLb = e.Item.FindControl("LBmandatoryItem")
                    oLb.Text = setMandatoryItem(dtoItem)
                    oImg = e.Item.FindControl("IMGmandatory")
                    oImg.Visible = True
                    oImg.ImageUrl = RootObject.ImgMandatorySubItem(Me.BaseUrl)
                    Me.Resource.setImage(oImg, True)
                End If
            Else
                Dim spancom As HtmlControl
                spancom = e.Item.FindControl("SPANCompletion")
                spancom.Visible = False
            End If

            If dtoItem.canUpdate Then
                Dim oLkb As LinkButton = e.Item.FindControl("LKBblocked")
                SetBlockedStatusLkb(oLkb, ServiceEP.CheckStatus(dtoItem.Status, Status.Locked), dtoItem.Id)

            Else

                oImg1 = e.Item.FindControl("IMGblocked")
                setBlockedStatusImg(oImg1, ServiceEP.CheckStatus(dtoItem.Status, Status.Locked))
            End If

            Dim oRpActivities As Repeater = e.Item.FindControl("RPactivity")
            oRpActivities.DataSource = dtoItem.Children
            AddHandler oRpActivities.ItemDataBound, AddressOf RPactivity_ItemDataBound
            oRpActivities.DataBind()

        End If
    End Sub
    Private Sub SetImgColor(ByRef oImg As System.Web.UI.WebControls.Image, ByRef isRed As Boolean, Optional ByVal toolTip As String = "")
        If isRed Then
            oImg.ImageUrl = RootObject.ImgRed(Me.BaseUrl)
        Else
            oImg.ImageUrl = RootObject.ImgGreen(Me.BaseUrl)
        End If

        oImg.ToolTip = toolTip
        oImg.Visible = True
    End Sub

    Private Sub ReloadStat(ByVal dateToView As DateTime?) Handles CTRLselTime.DateSelected
        If ViewModeType = EpViewModeType.Manage Then
            RedirectToUrl(RootObject.UserStatisticsManage(PathID, CurrentCommunityID, UserToViewStat, ItemType.Path, PageIndex, dateToView, PreloadShowAll))
        Else
            RedirectToUrl(RootObject.UserStatisticsView(PathID, CurrentCommunityID, dateToView, PreloadShowAll))
        End If
    End Sub
    Public Sub LoadCertifiedDate() Handles CTRLselTime.ViewCertifiedStat
        If ViewModeType = EpViewModeType.Manage Then
            RedirectToUrl(RootObject.UserStatisticsManage(PathID, CurrentCommunityID, UserToViewStat, ItemType.Path, PageIndex, EpEndDate, PreloadShowAll))
        Else
            RedirectToUrl(RootObject.UserStatisticsView(PathID, CurrentCommunityID, EpEndDate, PreloadShowAll))
        End If
    End Sub

    Public Sub RPactivity_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim dtoItem As dtoItemWithStatistic = e.Item.DataItem

            Dim oLb As Label
            Dim oLbw As Label
            oLb = e.Item.FindControl("LBname")



            oLbw = e.Item.FindControl("LBweight")
            If isTimeEp Then
                oLb.Text = dtoItem.Name
                oLbw.Text = " (" & ServiceEP.GetTime(dtoItem.Weight) & ")"
            Else
                oLb.Text = dtoItem.Name
                oLbw.Text = " (" & dtoItem.Weight & "pt)"
            End If

            Dim oImg As System.Web.UI.WebControls.Image
            Dim oImg1 As Label

            If ServiceEP.CheckStatus(dtoItem.Status, Status.Mandatory) Then
                'oImg = e.Item.FindControl("IMGisMandatory")
                'oImg.Visible = True
                'oImg.ImageUrl = RootObject.ImgMandatorySmall(Me.BaseUrl)
                'oImg.AlternateText = Resource.getValue("IMGmandatory.AlternateText")
                Dim oimgL As Label = e.Item.FindControl("IMGisMandatory")
                oimgL.Visible = True
                oimgL.ToolTip = Resource.getValue("IMGmandatory.AlternateText")
            End If

            If dtoItem.TotMandatory > 0 Then

                oLb = e.Item.FindControl("LBmandatoryItem")
                oLb.Text = setMandatoryItem(dtoItem)
                oImg = e.Item.FindControl("IMGmandatory")
                oImg.Visible = True
                oImg.ImageUrl = RootObject.ImgMandatorySubItem(Me.BaseUrl)
                Me.Resource.setImage(oImg, True)
            End If

            If dtoItem.Mark > -1 Then

                oLb = e.Item.FindControl("LBmark")
                SetLbValueWithMinValueFormat(oLb, dtoItem.Mark, dtoItem.MinMark)
                oImg = e.Item.FindControl("IMGmark")
                SetImgColor(oImg, dtoItem.Mark < dtoItem.MinMark)

                oLb = e.Item.FindControl("LBcompletion")
                If isTimeEp Then
                    SetLbValueWithMinValueFormat(oLb, ServiceEP.GetTime(dtoItem.Completion), ServiceEP.GetMinTime(dtoItem.Weight, dtoItem.MinCompletion))
                Else
                    SetLbValueWithMinValueFormat(oLb, dtoItem.Completion, dtoItem.MinCompletion)
                End If

                'oImg = e.Item.FindControl("IMGcompletion")
                'If isTimeEp Then
                '    SetImgColor(oImg, dtoItem.Completion < (dtoItem.Weight * dtoItem.MinCompletion / 100))
                'Else
                '    SetImgColor(oImg, dtoItem.Completion < dtoItem.MinCompletion)
                'End If
                Dim spancom As HtmlControl
                spancom = e.Item.FindControl("SPANCompletion")
                If isTimeEp Then
                    CompletionClass(spancom, dtoItem.Completion < (dtoItem.Weight * dtoItem.MinCompletion / 100))
                Else
                    CompletionClass(spancom, dtoItem.Completion < dtoItem.MinCompletion)
                End If


                oLb = e.Item.FindControl("LBstatus")
                SetLbStatus(oLb, dtoItem.StatusStat)
            End If

            Dim oHyp As HyperLink = e.Item.FindControl("HYPdetail")
            If ViewModeType = EpViewModeType.Manage Then
                oHyp.NavigateUrl = Me.BaseUrl & RootObject.UserActivityStatManage_PrevUsersStat(PathID, dtoItem.Id, CurrentCommunityID, UserToViewStat, TimeStat, PreloadShowAll)
            Else
                oHyp.NavigateUrl = Me.BaseUrl & RootObject.UserActivityStatView(PathID, dtoItem.Id, CurrentCommunityID, Me.TimeStat, PreloadShowAll)
            End If
            Me.Resource.setHyperLink(oHyp, False, True)

            If dtoItem.canUpdate Then
                Dim oLkb As LinkButton = e.Item.FindControl("LKBblocked")
                SetBlockedStatusLkb(oLkb, ServiceEP.CheckStatus(dtoItem.Status, Status.Locked), dtoItem.Id)

            Else
                oImg1 = e.Item.FindControl("IMGblocked")
                setBlockedStatusImg(oImg1, ServiceEP.CheckStatus(dtoItem.Status, Status.Locked))
            End If

            If isAutoEp Then
                Dim TD As HtmlControl = e.Item.FindControl("TDmark")
                TD.Visible = False

            End If

            Dim dtoActStat As dtoActivityStatistic = Me.ServiceEP.ServiceStat.GetDtoActivityStatistic_ByUser(dtoItem.Id, Me.CurrentCommunityID, Me.UserToViewStat, Me.CurrentUserId, Me.CurrentCommRoleID, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress, TimeStat)

            If CouldActivityWithSingleSubActivityBeOmitted And dtoActStat.SubActivities.Count = 1 Then
                Dim first As dtoSubActivityStatistic = dtoActStat.SubActivities.First()

                first.isMandatory = first.isMandatory Or ServiceEP.CheckStatus(dtoItem.Status, Status.Mandatory)
                first.SubActivity.Weight = dtoItem.Weight
                first.isSingle = True
                first.parentId = dtoItem.parentId
                If isTimeEp Then
                    first.ActivityCompleted = Not (dtoItem.Completion < (dtoItem.Weight * dtoItem.MinCompletion / 100))
                Else
                    first.ActivityCompleted = Not (dtoItem.Completion < dtoItem.MinCompletion)
                End If


                oLb = e.Item.FindControl("LBcompletion")
                first.Other = oLb.Text

            End If


            If Not dtoActStat.SubActivities Is Nothing Then
                For Each item As dtoSubActivityStatistic In dtoActStat.SubActivities
                    item.parentId = dtoItem.Id
                    If item.isSingle Then
                        item.parentId = dtoItem.parentId
                    End If
                Next
            End If

            Dim oRpSubActivities As Repeater = e.Item.FindControl("RPSubActivity")
            oRpSubActivities.DataSource = dtoActStat.SubActivities
            AddHandler oRpSubActivities.ItemDataBound, AddressOf RPSubactivity_ItemDataBound
            oRpSubActivities.DataBind()
        End If
    End Sub

    Protected Function isSingle(dtoitem As dtoSubActivityStatistic)
        If dtoitem.isSingle Then
            Return " single"
        Else
            Return ""
        End If
    End Function

    Protected Function ActivityUnit(dtoitem As dtoSubActivityStatistic)
        If dtoitem.isSingle Then
            Return "unit"
        Else
            Return "activity"
        End If
    End Function

    Protected Function HiddenActivity(dtoitem As dtoItemWithStatistic)
        Dim dtoActStat As dtoActivityStatistic = Me.ServiceEP.ServiceStat.GetDtoActivityStatistic_ByUser(dtoitem.Id, Me.CurrentCommunityID, Me.UserToViewStat, Me.CurrentUserId, Me.CurrentCommRoleID, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress, TimeStat)

        If (CouldActivityWithSingleSubActivityBeOmitted And dtoActStat.SubActivities.Count = 1) Then
            Return " hiddenActivity"
        Else
            Return ""
        End If
    End Function

    Public Sub RPSubactivity_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dtoItem As dtoSubActivityStatistic = e.Item.DataItem

            If ServiceEP.CheckStatus(dtoItem.SubActivity.Status, Status.Mandatory) Or dtoItem.isMandatory Then
                'oImg = e.Item.FindControl("IMGisMandatory")
                'oImg.Visible = True
                'oImg.ImageUrl = RootObject.ImgMandatorySmall(Me.BaseUrl)
                'oImg.AlternateText = Resource.getValue("IMGmandatory.AlternateText")
                Dim oimgL As Label = e.Item.FindControl("IMGisMandatory")
                oimgL.Visible = True
                oimgL.ToolTip = Resource.getValue("IMGmandatory.AlternateText")
            End If

            Dim spancom As HtmlControl
            spancom = e.Item.FindControl("SPANCompletion")

            If dtoItem.isSingle Then
                CompletionClass(spancom, Not dtoItem.ActivityCompleted)
            Else
                spancom.Visible = False
            End If

            Dim oLb As Label = e.Item.FindControl("LBcompletion")
            If dtoItem.isSingle Then

                oLb.Text = dtoItem.Other
            Else
                oLb.Text = "-"
            End If

            'If dtoItem.canUpdate Then
            'Dim oLkb As LinkButton = e.Item.FindControl("LKBblocked")
            'SetBlockedStatusLkb(oLkb, ServiceEP.CheckStatus(dtoItem.Status, Status.Locked), dtoItem.Id)

            'Else
            Dim oImg1 As Label = e.Item.FindControl("IMGblocked")
            setBlockedStatusImg(oImg1, ServiceEP.CheckStatus(dtoItem.SubActivity.Status, Status.Locked))
            'End If

            If isAutoEp Then
                Dim TD As HtmlControl = e.Item.FindControl("TDmark")
                TD.Visible = False

            End If
            Dim actions As List(Of dtoModuleActionControl)
            If ServiceEP.isSubActityInternalModule(dtoItem.SubActivity.ContentType) Then
                Dim initializer As New dtoInternalActionInitializer
                initializer.IdPath = PathID
                initializer.IdPathCommunity = Me.CurrentCommunityID
                initializer.CookieName = CookieName ' "PathCertifications_" & initializer.IdPath
                initializer.SubActivity = dtoItem.SubActivity
                If dtoItem.isSingle Then
                    Dim weight = ""
                    If isTimeEp Then
                        weight = " (" & ServiceEP.GetTime(dtoItem.SubActivity.Weight) & ")"
                    Else
                        weight = " (" & dtoItem.SubActivity.Weight & "pt)"
                    End If
                    initializer.PlaceHolders.Add(New lm.Comol.Core.ModuleLinks.dtoPlaceHolder() With {.Text = weight, .Type = lm.Comol.Core.ModuleLinks.PlaceHolderType.three, .CssClass = "duration"})
                End If

                initializer.ContainerCSS = SubActivityCssClass(dtoItem.SubActivity)
                initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.text

                Select Case dtoItem.SubActivity.ContentType
                    Case SubActivityType.Text
                        Dim oDisplayAction As lm.Comol.Modules.EduPath.Presentation.IViewModuleTextAction = e.Item.FindControl("CTRLtextAction")
                        e.Item.FindControl("CTRLtextAction").Visible = True
                        oDisplayAction.RefreshContainer = True

                        ' DIMENSIONI IMMAGINI
                        oDisplayAction.IconSize = Helpers.IconSize.Small
                        oDisplayAction.EnableAnchor = True
                        oDisplayAction.InitializeControl(initializer)
                    Case SubActivityType.Certificate
                        Dim control As UC_CertificationAction = e.Item.FindControl("CTRLcertificationAction")
                        control.Visible = True
                        control.EvaluablePath = Not isTimeEp()
                        Dim oCertAction As lm.Comol.Modules.EduPath.Presentation.IViewModuleCertificationAction = e.Item.FindControl("CTRLcertificationAction")
                        'oCertAction.RefreshContainer = True
                        ' DIMENSIONI IMMAGINI
                        oCertAction.IconSize = Helpers.IconSize.Small
                        oCertAction.EnableAnchor = True


                        'AddHandler oDisplayAction.RefreshContainerEvent, AddressOf RefreshContainerEvent
                        ' oCertAction.InitializeControl(initializer)


                        'If LTcertificationScript.Text = "" Then
                        '    LTcertificationScript.Text = "<script language=""Javascript"">" & control.GetControlScript(HDNdownloadTokenValue.ClientID) & "</script>"
                        'End If

                        actions = oCertAction.InitializeRemoteControl(UserToViewStat, initializer, StandardActionType.None)
                        Dim actStat As dtoModuleActionControl = actions.Where(Function(x) x.ControlType = StandardActionType.ViewPersonalStatistics).FirstOrDefault()

                        Dim atleaststarted As Boolean = dtoItem.StatusStat <> StatusStatistic.None And dtoItem.StatusStat <> StatusStatistic.Browsed

                        If (Not actStat Is Nothing) And (atleaststarted) Then
                            'Dim oLinkButton As LinkButton = e.Item.FindControl("LNBexportCertification")
                            'Me.Resource.setLinkButton(oLinkButton, False, True)
                            'oLinkButton.Visible = True
                            'oLinkButton.CommandArgument = dtoItem.SubActivity.Id

                            Dim oHyperlink As HyperLink = e.Item.FindControl("HYPexportCertification")
                            Resource.setHyperLink(oHyperlink, False, True)
                            oHyperlink.Visible = True
                            If Me.ViewModeType = EpViewModeType.Manage Then
                                oHyperlink.NavigateUrl = PageUtility.ApplicationUrlBase & RootObject.CertificationManagerDownloadPage(CTRLselTime.GetSelectedDate.Ticks, PathID, initializer.SubActivity.ActivityParentId, initializer.SubActivity.Id, UserToViewStat)
                            Else
                                oHyperlink.NavigateUrl = PageUtility.ApplicationUrlBase & RootObject.CertificationUserDownloadPage(CTRLselTime.GetSelectedDate.Ticks, PathID, initializer.SubActivity.ActivityParentId, initializer.SubActivity.Id, UserToViewStat)
                            End If




                            oHyperlink = e.Item.FindControl("HYPrefreshCertificate")
                            Resource.setHyperLink(oHyperlink, False, True)
                            oHyperlink.Visible = (Me.ViewModeType = EpViewModeType.Manage)

                            '(PathID, UserToViewStat, ServiceEP.GetDtoSubActivity(ItemId), Me.HDNdownloadTokenValue.Value)
                            oHyperlink.NavigateUrl = PageUtility.ApplicationUrlBase & ServiceEP.GetCertificationRestoreUrl(PathID, dtoItem.SubActivity.ActivityParentId, dtoItem.SubActivity.Id, UserToViewStat)
                        ElseIf dtoItem.StatusStat = StatusStatistic.Completed OrElse dtoItem.StatusStat = StatusStatistic.CompletedPassed Then
                            'Dim oLinkButton As LinkButton = e.Item.FindControl("LNBexportCertification")
                            'Me.Resource.setLinkButton(oLinkButton, False, True)
                            'oLinkButton.Visible = True
                            Dim oHyperlink As HyperLink = e.Item.FindControl("HYPexportCertification")
                            Resource.setHyperLink(oHyperlink, False, True)
                            oHyperlink.Visible = True
                            If Me.ViewModeType = EpViewModeType.Manage Then
                                oHyperlink.NavigateUrl = PageUtility.ApplicationUrlBase & RootObject.CertificationManagerDownloadPage(CTRLselTime.GetSelectedDate.Ticks, PathID, initializer.SubActivity.ActivityParentId, initializer.SubActivity.Id, UserToViewStat)
                            Else
                                oHyperlink.NavigateUrl = PageUtility.ApplicationUrlBase & RootObject.CertificationUserDownloadPage(CTRLselTime.GetSelectedDate.Ticks, PathID, initializer.SubActivity.ActivityParentId, initializer.SubActivity.Id, UserToViewStat)
                            End If


                        End If
                End Select
                'Dim oDisplayAction As lm.Comol.Modules.EduPath.Presentation.IViewModuleTextAction = e.Item.FindControl("CTRLtextAction")

                'e.Item.FindControl("CTRLtextAction").Visible = True
                'oDisplayAction.RefreshContainer = True
                'Dim initializer As New dtoInternalActionInitializer
                'initializer.SubActivity = New dtoSubActivity() With {.Id = dtoItem.Id, .Description = dtoItem.Name}
                'oDisplayAction.IconSize = Helpers.IconSize.Small
                'oDisplayAction.EnableAnchor = True
                'initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.text
                'oDisplayAction.InitializeControl(initializer)
            Else
                Dim oPlaceHolder As PlaceHolder = e.Item.FindControl("PHLAction")
                oPlaceHolder.Visible = True
                Dim oDisplayAction As lm.Comol.Core.ModuleLinks.IExternalModuleDisplayAction = CType(LoadControl(BaseUrl & lm.Comol.Core.DomainModel.Common.CoreRootObject.GenericNewDisplayActionControl), lm.Comol.Core.ModuleLinks.IExternalModuleDisplayAction)
                Dim initializer As New lm.Comol.Core.ModuleLinks.dtoExternalModuleInitializer
                initializer.Link = dtoItem.SubActivity.ModuleLink
                ' AGGIUNTA PLACEHOLDER
                ' --> 
                'initializer.PlaceHolders.Add(New lm.Comol.Core.ModuleLinks.dtoPlaceHolder() With {.Type = lm.Comol.Core.ModuleLinks.PlaceHolderType.three})

                If dtoItem.isSingle Then
                    Dim weight = ""
                    If isTimeEp Then
                        weight = " (" & ServiceEP.GetTime(dtoItem.SubActivity.Weight) & ")"
                    Else
                        weight = " (" & dtoItem.SubActivity.Weight & "pt)"
                    End If

                    initializer.PlaceHolders.Add(New lm.Comol.Core.ModuleLinks.dtoPlaceHolder() With {.Text = weight, .Type = lm.Comol.Core.ModuleLinks.PlaceHolderType.three, .CssClass = "duration"})

                End If


                ' PLAY MODE
                ' --> oDisplayAction.InitializeControl(initializer)
                'DISPLAY EDIT MODE
                ' -->oDisplayAction.InitializeControl(initializer, StandardActionType.Edit Or StandardActionType.DownloadItem Or StandardActionType.EditMetadata Or StandardActionType.ViewAdvancedStatistics)
                'DISPLAY STATISTICHE amministratore generiche

                oDisplayAction.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.text
                actions = oDisplayAction.InitializeRemoteControl(UserToViewStat, initializer, StandardActionType.EditMetadata Or StandardActionType.ViewAdvancedStatistics)

                Dim actStat = Nothing ' actions.Where(Function(x) x.ControlType = StandardActionType.ViewPersonalStatistics).FirstOrDefault()


                Select Case Me.ViewModeType
                    Case EpViewModeType.View
                        actStat = actions.Where(Function(x) x.ControlType = StandardActionType.ViewPersonalStatistics).FirstOrDefault()
                    Case EpViewModeType.Manage
                        actStat = actions.Where(Function(x) x.ControlType = StandardActionType.ViewUserStatistics).FirstOrDefault()
                End Select

                Dim atleaststarted As Boolean = dtoItem.StatusStat <> StatusStatistic.None And dtoItem.StatusStat <> StatusStatistic.Browsed



                'Select Case dtoItem.StatusStat
                '    Case StatusStatistic.CompletedPassed
                '        atleaststarted = True
                '        'oLb.CssClass = oLb.CssClass & " Ep_ItemGreen"
                '    Case StatusStatistic.None
                '        atleaststarted = False
                '        'oLb.CssClass = oLb.CssClass & "Ep_ItemRed"
                '    Case StatusStatistic.Browsed
                '        'oLb.CssClass = oLb.CssClass & "Ep_ItemRed"
                '        atleaststarted = False
                '    Case Else
                '        atleaststarted = True
                '        'oLb.CssClass = oLb.CssClass & "Ep_Item7Yellow"
                'End Select

                If (Not actStat Is Nothing) And (atleaststarted) Then
                    Dim ohypStat As HyperLink = e.Item.FindControl("HYPstats")
                    Me.Resource.setHyperLink(ohypStat, False, True)
                    ohypStat.Visible = True
                    ohypStat.NavigateUrl = actStat.LinkUrl & "&RUid=" & UserToViewStat.ToString()
                End If

                ' DEFINISCO UNA CLASSE PER IL CONTAINER
                oDisplayAction.ContainerCSS = ""
                ' DIMENSIONI IMMAGINI
                oDisplayAction.IconSize = Helpers.IconSize.Small

                oDisplayAction.EnableAnchor = True
                'DISPLAY PLAY MODE
                ' --> oDisplayAction.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction
                'DISPLAY PLAY MODE SE NON POSSO ESEGUIRE AZIONE
                ' --> oDisplayAction.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.text
                'DISPLAY EDIT MODE
                ' -->  
                oDisplayAction.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.text 'lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction Or lm.Comol.Core.ModuleLinks.DisplayActionMode.actions
                'DISPLAY STATISTICHE
                ' --> oDisplayAction.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.text

                'oDisplayAction.InitializeControl(initializer)

                oPlaceHolder.Controls.Add(oDisplayAction)
            End If

        End If
    End Sub
    Public Sub RPSubactivity_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs)
        CTRLmessages.Visible = False
        Select Case e.CommandName
            'Case "certification"
            '    Dim control As UC_CertificationAction = e.Item.FindControl("CTRLcertificationAction")
            '    control.DownloadCertification(PathID, UserToViewStat, ServiceEP.GetDtoSubActivity(CLng(e.CommandArgument)), CTRLselTime.GetSelectedDate, Me.HDNdownloadTokenValue.Value)

            'Case "refreshcertification"
            '    Dim control As UC_CertificationAction = e.Item.FindControl("CTRLcertificationAction")
            '    control.RestoreCertificate(PathID, UserToViewStat, ServiceEP.GetDtoSubActivity(ItemId), Me.HDNdownloadTokenValue.Value)
            Case Else
                ReloadStat(CTRLselTime.GetSelectedDate)
        End Select
    End Sub

#Region "Action Events"
    Public Sub RefreshContainerEvent(sender As Object, e As lm.Comol.Modules.EduPath.Presentation.RefreshContainerArgs)
        CTRLmessages.Visible = False
        If (e.Executed) Then
            BindDati()
        Else
            Me.ShowError(EpError.Generic)
        End If
    End Sub
    Protected Sub ItemActionResult(err As lm.Comol.Core.Certifications.CertificationError, ByVal savingRequired As Boolean, ByVal saved As Boolean)
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
        Me.BindDati()
    End Sub
    Public Sub GetHiddenIdentifierValueEvent(ByRef value As String)
        value = Me.HDNdownloadTokenValue.Value
    End Sub
#End Region

#Region "Export"
    Private Sub LNBexportUserStatisticToCsv_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBexportUserStatisticToCsv.Click
        ExportUserStatistics(Helpers.Export.ExportFileType.csv)
    End Sub
    Private Sub LNBexportUserStatisticToXml_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBexportUserStatisticToXml.Click
        ExportUserStatistics(Helpers.Export.ExportFileType.xml)
    End Sub
    Private Sub LNBexportUserStatisticToPdf_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBexportUserStatisticToPdf.Click
        ExportUserStatistics(Helpers.Export.ExportFileType.pdf)
    End Sub
    Private Sub ExportUserStatistics(exportType As lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType)
        CTRLmessages.Visible = False
        Dim cookie As HttpCookie = Nothing
        If Not String.IsNullOrEmpty(CookieName) Then
            cookie = New HttpCookie(CookieName, HDNdownloadTokenValue.Value)
        End If

        Dim clientFileName As String = GetFileName(exportType)
        Dim translations As New Dictionary(Of EduPathTranslations, String)
        For Each name As String In [Enum].GetNames(GetType(EduPathTranslations))
            translations.Add([Enum].Parse(GetType(EduPathTranslations), name), Me.Resource.getValue("EduPathTranslations." & name))
        Next
        Dim roles As List(Of Comol.Entity.Role) = COL_BusinessLogic_v2.CL_permessi.COL_TipoRuolo.List(PageUtility.LinguaID).OrderBy(Function(t) t.Name).ToList
        Dim roleTranslations As Dictionary(Of Integer, String) = roles.ToDictionary(Function(r) r.ID, Function(r) r.Name)

        Response.Clear()

        Try
            Dim gAction As lm.Comol.Core.ModuleLinks.IExternalModuleDisplayAction = CType(LoadControl(BaseUrl & lm.Comol.Core.DomainModel.Common.CoreRootObject.GenericNewDisplayActionControl), lm.Comol.Core.ModuleLinks.IExternalModuleDisplayAction)
            Dim tAction As lm.Comol.Modules.EduPath.Presentation.IViewModuleTextAction = CType(LoadControl(BaseUrl & RootObject.RenderTextAction), lm.Comol.Modules.EduPath.Presentation.IViewModuleTextAction)
            Dim cAction As lm.Comol.Modules.EduPath.Presentation.IViewModuleCertificationAction = CType(LoadControl(BaseUrl & RootObject.RenderCertificationAction), lm.Comol.Modules.EduPath.Presentation.IViewModuleCertificationAction)

            Dim statistics As dtoItemWithStatistic = Nothing
            Dim idUser As Integer = Me.CurrentUserId
            If ViewModeType = EpViewModeType.Manage Then
                idUser = Me.UserToViewStat
                statistics = Me.ServiceEP.ServiceStat.GetParticipantEduPathStatisticStructure(Me.PathID, Me.CurrentUserId, Me.CurrentCommRoleID, Me.UserToViewStat, TimeStat)
            Else
                statistics = Me.ServiceEP.ServiceStat.GetOwnerEduPathStatisticStructure(Me.PathID, Me.CurrentUserId, Me.CurrentCommRoleID, TimeStat)
            End If

            Dim settings As dtoExportConfigurationSetting = ServiceEP.GetExportSetting(Me.PathID, CurrentCommunityID, IIf((ViewModeType = EpViewModeType.Manage), StatisticsPageType.UserStatistic, StatisticsPageType.MyPathStatistic), ConfigurationType.Export)

            If exportType = Helpers.Export.ExportFileType.pdf Then
                Dim oTemplate As New lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template
                'oTemplate.Header = Master.getTemplateHeader
                'oTemplate.Footer = Master.getTemplateFooter
                oTemplate.Settings = lm.Comol.Core.DomainModel.Helpers.Export.ExportBaseHelper.GetDefaultPageSettings()
                oTemplate.Settings.Size = DocTemplateVers.PageSize.A4

                Dim doc As iTextSharp5.text.Document
                If Me.Type <> lm.Comol.Modules.EduPath.Domain.ItemType.SubActivity Then
                    Dim hPdf As New HelperExportToPdf(New lm.Comol.Core.Business.BaseModuleManager(PageUtility.CurrentContext), HelperExportToPdf.ExportContentType.UserPathStatistics, translations, roleTranslations, oTemplate)

                    doc = hPdf.UserPathStatistics(ServiceEP.ServiceStat, Me.PathID, Type, idUser, CurrentUserId, statistics, TimeStat, settings, ExporPathData.Normal, gAction, tAction, cAction, False, clientFileName, Response, New HttpCookie(CookieName, HDNdownloadTokenValue.Value))
                    'Else
                    '    oTemplate.Settings.Size = DocTemplateVers.PageSize.A4
                    '    doc = ServiceEP.ServiceStat.ExportUsersSubActivityStatistics_ToPdf(ItemId, Me.CurrentCommunityID, Me.CHBshowall.Checked, translations, gAction, tAction, cAction, oTemplate, False, clientFileName, Response, New HttpCookie(CookieName, HDNdownloadTokenValue.Value), TimeStat)

                End If
                If IsNothing(doc) Then
                    ShowError(EpError.Generic)
                End If
            Else
                If Not IsNothing(cookie) Then
                    Response.AppendCookie(cookie)
                End If

                Response.AddHeader("Content-Disposition", "attachment; filename=" & clientFileName)
                Response.Charset = ""
                Response.ContentEncoding = System.Text.Encoding.Default
                Select Case exportType
                    Case Helpers.Export.ExportFileType.xml
                        Response.ContentType = "application/ms-excel"
                        Dim hXml As New HelperExportToXml(New lm.Comol.Core.Business.BaseModuleManager(PageUtility.CurrentContext), translations, roleTranslations)
                        If Type <> lm.Comol.Modules.EduPath.Domain.ItemType.SubActivity Then
                            Response.Write(hXml.UserPathStatistics(ServiceEP.ServiceStat, Me.PathID, Type, idUser, CurrentUserId, statistics, TimeStat, settings, ExporPathData.Normal, gAction, tAction, cAction))
                        Else
                            'Response.Write(ServiceEP.ServiceStat.ExportUsersSubActivityStatistics_ToXml(ItemId, Me.CurrentCommunityID, CHBshowall.Checked, translations, gAction, tAction, cAction, TimeStat))
                        End If
                    Case Else
                        Response.ContentType = "text/csv"
                        Dim hCsv As New HelperExportToCsv(New lm.Comol.Core.Business.BaseModuleManager(PageUtility.CurrentContext), translations, roleTranslations)
                        If Me.Type <> lm.Comol.Modules.EduPath.Domain.ItemType.SubActivity Then
                            Response.Write(hCsv.UserPathStatistics(ServiceEP.ServiceStat, Me.PathID, Type, idUser, CurrentUserId, statistics, TimeStat, settings, ExporPathData.Normal, gAction, tAction, cAction))
                        Else
                            'Response.Write(ServiceEP.ServiceStat.ExportUsersSubActivityStatistics_ToCsv(ItemId, Me.CurrentCommunityID, Me.CHBshowall.Checked, translations, gAction, tAction, cAction, TimeStat))
                            ' Response.Write(hCsv.UserSubActivityPathStatistics(idUser, statistics, gAction, tAction, cAction, TimeStat))
                        End If
                End Select
            End If
        Catch ex As Exception
            Select Case exportType
                Case Helpers.Export.ExportFileType.pdf
                    ShowError(EpError.Generic)
                Case Helpers.Export.ExportFileType.xml
                    Response.Write(HelperExportToXml.GetErrorDocument(translations(EduPathTranslations.FileCreationError), translations(EduPathTranslations.StatisticsInfo), TimeStat))
                Case Else
                    Response.Write(HelperExportToCsv.GetErrorDocument(translations(EduPathTranslations.FileCreationError)))
            End Select
        End Try
        Response.End()
    End Sub

    Private Function GetFileName(ByVal exportType As lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType) As String
        Dim filename As String = Resource.getValue("Export.Filename." & IIf(Type = Global.lm.Comol.Modules.EduPath.Domain.ItemType.SubActivity, "UserSubActivityStatistics", "UserStatistics"))
        Dim oDate As DateTime = DateTime.Now
        If String.IsNullOrEmpty(filename) Then
            If Type = ItemType.SubActivity Then
                filename = "UserActionStatistics_On_{0}_{1}_{2}"
            Else
                filename = "UserStatistics_On_{0}_{1}_{2}"
            End If
        End If
        Return String.Format(filename, oDate.Year, oDate.Month, oDate.Day) & IIf(exportType <> Helpers.Export.ExportFileType.pdf, "." & exportType.ToString, "")
    End Function
    Private Function GetFileName(ByVal exportType As lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType, onlyForCertifications As Boolean) As String
        Dim filename As String = Resource.getValue("Export.Filename.FullPathData." & onlyForCertifications.ToString)
        Dim oDate As DateTime = DateTime.Now
        If String.IsNullOrEmpty(filename) Then
            If onlyForCertifications Then
                filename = "Path_{0}_UserCertificationStatistics_{1}_{2}_{3}"
            Else
                filename = "Path_{0}_UserFullStatistics_{1}_{2}_{3}"
            End If
        End If
        Dim displayname As String = ServiceEP.GetPathName(PathID)
        If displayname.Length > 50 Then
            displayname = Left(displayname, 60)
        End If
        Return lm.Comol.Core.DomainModel.Helpers.Export.ExportBaseHelper.HtmlCheckFileName(String.Format(filename, displayname, oDate.Year, oDate.Month, oDate.Day) & IIf(exportType <> lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType.pdf, "." & exportType.ToString, ""))
    End Function
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