Imports lm.Comol.Modules.EduPath.Domain
Imports lm.Comol.Modules.EduPath.BusinessLogic
Imports lm.Comol.UI.Presentation
Imports COL_BusinessLogic_v2.UCServices
Imports lm.ActionDataContract
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel

Public Class EpUserStat
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

#End Region

#Region " Base"
    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_EduPath.Codex)
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
        Dim currentUserPermission As PermissionEP = Me.ServiceEP.GetUserPermission_ByPath(Me.PathID, Me.CurrentUserId, Me.CurrentCommRoleID)
        Me.Master.ServiceTitle = Me.Resource.getValue("UserEpStat")

        'ABILITARE IN CASO DI OVERTIME
        ' Dim dtoDate As dtoEndTime = ServiceEP.GetEpTime(PathID, UserToViewStat)

        '  Dim IsCertifiedTime As Boolean = IIf(dtoDate.CertifiedTime Is Nothing, False, IIf(Not dtoDate.CertifiedTime Is Nothing AndAlso dtoDate.CertifiedTime = TimeStat, True, False))
        Dim isManageMode As Boolean = Me.ViewModeType = EpViewModeType.Manage
        ' Me.CTRLselTime.InitView(TimeStat, dtoDate.EndDateStr, dtoDate.OverEndDateStr, isManageMode, isManageMode, dtoDate.EndDate)

        Me.CTRLselTime.InitView(TimeStat, IIf(EpEndDate Is Nothing, "", ServiceEP.GetDate(EpEndDate)), "", isManageMode, isManageMode, EpEndDate)

        Me.InitHyperLink()
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
        Else
            setBlockedStatusImg(Me.IMGblocked, ServiceEP.CheckStatus(dto.Status, Status.Locked))
        End If
    End Sub

    Private Sub InitPage_Manage()
        Dim PathStatistics As dtoItemWithStatistic = Me.ServiceEP.ServiceStat.GetParticipantEduPathStatisticStructure(Me.PathID, Me.CurrentUserId, Me.CurrentCommRoleID, Me.UserToViewStat, TimeStat)

        If Not IsNothing(PathStatistics) Then
            Me.LBuserName.Text = Me.ServiceEP.GetUserName(Me.UserToViewStat)
            SetBlockedStatus(PathStatistics)
            Me.InitProgressBar()
            Me.InitPathStat(PathStatistics)
            Me.RPunit.DataSource = PathStatistics.Children
            Me.RPunit.DataBind()
        Else
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


        End Select
        Me.HYPeduPathView.NavigateUrl = Me.BaseUrl & RootObject.PathView(PathID, Me.CurrentCommunityID, Me.ViewModeType, ServiceEP.isPlayModePath(PathID))

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
                Me.LBname.Text = pathStat.Name & " (" & ServiceEP.GetTime(pathStat.Weight) & " )"
                SetImgColor(IMGcompletion, pathStat.Completion < (pathStat.Weight * pathStat.MinCompletion / 100))

            Else

                Me.LBname.Text = pathStat.Name
                SetLbValueWithMinValueFormat(LBcompletion, pathStat.Completion, pathStat.MinCompletion)
                SetImgColor(IMGcompletion, pathStat.Completion < pathStat.MinCompletion)

            End If

        End If

        If pathStat.TotMandatory > 0 Then
            If isAutoEp Then
                LBpathMandatoryItem.Text = pathStat.TotMandatory & "#" & pathStat.CompletedPassedMandatory & "#" & pathStat.CompletedPassedMandatory & "#" & pathStat.CompletedPassedMandatory
            Else
                LBpathMandatoryItem.Text = pathStat.TotMandatory & "#" & pathStat.OnlyCompletedMandatory & "#" & pathStat.OnlyPassedMandatory & "#" & pathStat.CompletedPassedMandatory
            End If
            Me.IMGmandatory.Visible = True
            IMGmandatory.ImageUrl = RootObject.ImgMandatorySubItem(Me.BaseUrl)
            Me.Resource.setImage(IMGmandatory, True)
        End If

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
            Dim dtoItem As dtoItemWithStatistic = e.Item.DataItem

            Dim oLb As Label

            oLb = e.Item.FindControl("LBname")
            If isTimeEp Then
                oLb.Text = Me.Resource.getValue("Unit") & ": " & dtoItem.Name ' & " (" & ServiceEP.GetTime(dtoItem.Weight) & ")"
            Else
                oLb.Text = Me.Resource.getValue("Unit") & ": " & dtoItem.Name & " (" & dtoItem.Weight & "pt)"
            End If

            If ServiceEP.CheckStatus(dtoItem.Status, Status.Mandatory) Then
                oImg = e.Item.FindControl("IMGisMandatory")
                oImg.Visible = True
                oImg.ImageUrl = RootObject.ImgMandatorySmall(Me.BaseUrl)
                oImg.AlternateText = Resource.getValue("IMGmandatory.AlternateText")
            End If

            If Not isAutoEp Then
                If dtoItem.Mark > -1 Then

                    oLb = e.Item.FindControl("LBmark")
                    SetLbValueWithMinValueFormat(oLb, dtoItem.Mark, dtoItem.MinMark)
                    oImg = e.Item.FindControl("IMGmark")
                    SetImgColor(oImg, dtoItem.Mark < dtoItem.MinMark)

                    oLb = e.Item.FindControl("LBcompletion")
                    SetLbValueWithMinValueFormat(oLb, dtoItem.Completion, dtoItem.MinCompletion)

                    oImg = e.Item.FindControl("IMGcompletion")
                    If isTimeEp Then
                        SetImgColor(oImg, dtoItem.Completion < (dtoItem.Weight * dtoItem.MinCompletion / 100))
                    Else
                        SetImgColor(oImg, dtoItem.Completion < dtoItem.MinCompletion)
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
            End If

            If dtoItem.canUpdate Then
                Dim oLkb As LinkButton = e.Item.FindControl("LKBblocked")
                SetBlockedStatusLkb(oLkb, ServiceEP.CheckStatus(dtoItem.Status, Status.Locked), dtoItem.Id)

            Else

                oImg = e.Item.FindControl("IMGblocked")
                setBlockedStatusImg(oImg, ServiceEP.CheckStatus(dtoItem.Status, Status.Locked))
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

            oLb = e.Item.FindControl("LBname")
            If isTimeEp Then
                oLb.Text = Me.Resource.getValue("Activity") & ": " & dtoItem.Name & " (" & ServiceEP.GetTime(dtoItem.Weight) & " )"
            Else
                oLb.Text = Me.Resource.getValue("Activity") & ": " & dtoItem.Name & " (" & dtoItem.Weight & " pt)"
            End If

            Dim oImg As System.Web.UI.WebControls.Image

            If ServiceEP.CheckStatus(dtoItem.Status, Status.Mandatory) Then
                oImg = e.Item.FindControl("IMGisMandatory")
                oImg.Visible = True
                oImg.ImageUrl = RootObject.ImgMandatorySmall(Me.BaseUrl)
                oImg.AlternateText = Resource.getValue("IMGmandatory.AlternateText")
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

                oImg = e.Item.FindControl("IMGcompletion")
                If isTimeEp Then
                    SetImgColor(oImg, dtoItem.Completion < (dtoItem.Weight * dtoItem.MinCompletion / 100))
                Else
                    SetImgColor(oImg, dtoItem.Completion < dtoItem.MinCompletion)
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
                oImg = e.Item.FindControl("IMGblocked")
                setBlockedStatusImg(oImg, ServiceEP.CheckStatus(dtoItem.Status, Status.Locked))
            End If

            If isAutoEp Then
                Dim TD As HtmlControl = e.Item.FindControl("TDmark")
                TD.Visible = False

            End If
        End If
    End Sub



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