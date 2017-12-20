Imports lm.Comol.Modules.EduPath.Domain
Imports lm.Comol.Modules.EduPath.BusinessLogic
Imports lm.Comol.UI.Presentation
Imports COL_BusinessLogic_v2.UCServices
Imports lm.ActionDataContract
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports System.Linq

Public Class PathStatistics
    Inherits PageBaseEduPath

#Region "Context"
    Private _QSservice As COL_Questionario.Business.ServiceQuestionnaire
    Private ReadOnly Property QSservice() As COL_Questionario.Business.ServiceQuestionnaire
        Get
            If IsNothing(_QSservice) Then
                _QSservice = New COL_Questionario.Business.ServiceQuestionnaire(Me.PageUtility.CurrentContext)
            End If
            Return _QSservice
        End Get
    End Property
#End Region

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
    Private ReadOnly Property PreloadShowAllStatistics As Boolean
        Get
            Dim qs_ShowAll As String = Request.QueryString("ShowAll")
            If Not String.IsNullOrEmpty(qs_ShowAll) Then
                qs_ShowAll = qs_ShowAll.ToLower
            End If

            Return (Request.QueryString("ShowAll") = "true")
        End Get
    End Property
    Private ReadOnly Property IsEvaluable As Boolean
        Get
            Return Request.QueryString("Eval") = "1"
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


    Protected ReadOnly Property CookieName() As String
        Get
            Return "COMOL_PathStatistics"
        End Get
    End Property
    Protected ReadOnly Property DisplayMessageToken() As String
        Get
            Return Resource.getValue("DisplayMessageToken.PathStatistics")
        End Get
    End Property
    Protected ReadOnly Property DisplayTitleToken() As String
        Get
            Return Resource.getValue("DisplayTitleToken.PathStatistics")
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
                        Return EpViewModeType.View
                End Select
            Else
                Return EpViewModeType.View
            End If
        End Get
    End Property

    Private ReadOnly Property PathID() As Long
        Get
            If IsNumeric(Request.QueryString("PId")) Then
                Return Request.QueryString("PId")
            ElseIf IsNumeric(Request.QueryString("saId")) Then
                Return ServiceEP.GetActivity(Request.QueryString("saId")).Path.Id
            Else
                Return -1
            End If
        End Get
    End Property

    Private _canViewStat As Boolean
    Private Property CanViewStat As Boolean
        Get
            Return _canViewStat
        End Get
        Set(ByVal value As Boolean)
            _canViewStat = value
        End Set
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
            If Not IsNothing(UtenteCorrente) Then
                Return UtenteCorrente.GetIDRuoloForComunita(CurrentCommunityID)
            Else
                Return lm.Comol.Core.DomainModel.UserTypeStandard.Guest
            End If
        End Get
    End Property

    Private _isCertifiedTime As Boolean = False
    Private Property IsCertifiedTime As Boolean
        Get
            Return _isCertifiedTime
        End Get
        Set(value As Boolean)
            _isCertifiedTime = value
        End Set
    End Property

#End Region

    Dim CouldActivityWithSingleSubActivityBeOmitted As Boolean = False

#Region " Base"
    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_EduPath.Codex)
        CouldActivityWithSingleSubActivityBeOmitted = True 'EduPathConfiguration.UseSingleActionView
        Me.Master.ShowDocType = True
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property

    Public Function Percentual(value As Int32, tot As Int32) As Int32
        If tot > 0 Then
            Dim d As Double = value / tot * 100
            Return Math.Floor(d)
        Else
            Return 0
        End If

    End Function

    Public Overrides Sub BindDati()


        'UpdateScormStat()
        Me.Master.ServiceTitle = Me.Resource.getValue("GlobalEpStat")
        If Not Page.IsPostBack Then
            Me.CHBshowall.Checked = PreloadShowAllStatistics
        End If
        LTRNavUnitTop.Text = Me.Resource.getValue("Unit")
        LTRNavUnitBottom.Text = Me.Resource.getValue("Unit")
        LTRNavActivityTop.Text = Me.Resource.getValue("Activity")
        LTRNavActivityBottom.Text = Me.Resource.getValue("Activity")
        LTRNavSubActivityTop.Text = Me.Resource.getValue("SubActivity")
        LTRNavSubActivityBottom.Text = Me.Resource.getValue("SubActivity")
        SPANviewStatistics.InnerHtml = Me.Resource.getValue("ViewStatistics")

        ' Dim dtoDate As dtoEndTime = ServiceEP.GetEpTime(PathID, CurrentUserId) Riattivare in caso di overtime
        ' IsCertifiedTime = IIf(dtoDate.CertifiedTime Is Nothing, False, IIf(Not dtoDate.CertifiedTime Is Nothing AndAlso dtoDate.CertifiedTime = TimeStat, True, False))
        ' Me.CTRLselTime.InitView(TimeStat, dtoDate.EndDateStr, dtoDate.OverEndDateStr, True, True, dtoDate.EndDate)

        Me.CTRLselTime.InitView(TimeStat, IIf(EpEndDate Is Nothing, "", ServiceEP.GetDate(EpEndDate)), "", True, True, EpEndDate)

        Me.InitHyperLink()
        Dim stat As dtoEpGlobalStat = ServiceEP.ServiceStat.GetGlobalEpStats(Me.PathID, Me.CurrentCommunityID, Me.CurrentUserId, Me.CurrentCommRoleID, IsEvaluable, TimeStat, CHBshowall.Checked)
        If IsNothing(stat) Then
            Me.ShowError(EpError.Generic)
        Else
            Me.PageUtility.AddAction(Me.CurrentCommunityID, Services_EduPath.ActionType.ViewUserStat, Me.PageUtility.CreateObjectsList(Services_EduPath.ObjectType.EduPath, Me.PathID.ToString), InteractionType.UserWithLearningObject)
            Dim hasCertification As Boolean = stat.HasSubActivityType(SubActivityType.Certificate)
            Dim hasQuestionnaires As Boolean = stat.HasSubActivityType(SubActivityType.Quiz)

            Me.LNBexportPSfullCertificationToCsv.Visible = hasCertification AndAlso hasQuestionnaires
            Me.LNBexportPSfullCertificationToXml.Visible = hasCertification AndAlso hasQuestionnaires
            Me.LNBexportPSfullToCsv.Visible = hasQuestionnaires
            Me.LNBexportPSfullToXml.Visible = hasQuestionnaires
            If isTimeEp Then

                LBname.Text = stat.itemName
                LBweight.Text = " (" & ServiceEP.GetTime(stat.Weight) & ")"
            Else

                LBname.Text = stat.itemName
            End If

            If isAutoEp Then
                Me.THcompleted.Visible = False
                Me.THpassed.Visible = False
                Me.TDcompleted.Visible = False
                Me.TDpassed.Visible = False
            End If

            If stat.userCount > -1 Then
                Me.CanViewStat = True
                SetTypeCountOverTotal(Me.LBcomplPass, stat.compPassedCount, stat.userCount)
                'SetTypeCountOverTotal(Me.LBcompleted, stat.completedCount, stat.userCount)
                'SetTypeCountOverTotal(Me.LBpassed, stat.passedCount, stat.userCount)
                SetTypeCountOverTotal(Me.LBstarted, stat.startedCount, stat.userCount)
                SetTypeCountOverTotal(Me.LBnotStarted, stat.notStartedCount, stat.userCount)


                InitStackedBar(stat, CTRLStackedBar)


                'InitStackedBar(Me.IMGgreen, Me.IMGyellow, Me.IMGred, stat)
                If Not IsEvaluable Then
                    Me.Resource.setHyperLink(HYPdetail, False, True)
                    HYPdetail.Text = ""
                    HYPdetail.NavigateUrl = Me.BaseUrl & RootObject.UsersStatistics(PathID, Me.CurrentCommunityID, ItemType.Path, 0, TimeStat, CHBshowall.Checked)
                    Me.HYPdetail.Visible = True
                End If

                RPunit.DataSource = stat.childrenStat
                RPunit.DataBind()
                SetBlockedStatusLkb(Me.LBPathLock, ServiceEP.CheckStatus(stat.status, Status.Locked), stat.itemId)
            Else
                'setBlockedStatusImg(Me.IMGblocked, ServiceEP.CheckStatus(stat.status, Status.Locked))
                SetBlockStatus(ImgPathLock, ServiceEP.CheckStatus(stat.status, Status.Locked))
            End If

            'SetBlockStatus(ImgPathLock, ServiceEP.CheckStatus(stat.status, Status.Locked))


            If Not CanViewStat Then
                Me.ShowError(EpError.NotPermission)
            End If

            HYPcertificates.Text = ""
            HYPcertificates.NavigateUrl = Me.BaseUrl + RootObject.EPCertificationList(CurrentCommunityID, PathID)

            HYPcertificates.Visible = ServiceEP.PathHasSubActivityType(PathID, SubActivityType.Certificate)
        End If
    End Sub

    Public Sub SetBlockStatus(c As HtmlControl, lock As Boolean)
        c.Visible = True
        If lock Then
            'ImgPathLock
            c.Attributes("class") = c.Attributes("class").Replace(" unlocked", "") + " locked"
        Else
            c.Attributes("class") = c.Attributes("class").Replace(" locked", "") + " unlocked"
        End If
    End Sub

    Public Sub InitStackedBar(stat As dtoGenericGlobalStat, CTRLStackedBar As UC_StackedBar)
        'If isAutoEp Then
        Dim complPass As StackedBarItem = New StackedBarItem()
        complPass.CssClass = "completed"
        complPass.Title = Me.Resource.getValue("StatusStatistic.CompletedPassed") + ": {0}%"
        complPass.Value = Percentual(stat.compPassedCount, stat.userCount) 'stat.compPassedCount / stat.userCount * 100
        'End If


        Dim notpassed As StackedBarItem = New StackedBarItem()
        notpassed.CssClass = "notpassed"
        notpassed.Title = Me.Resource.getValue("StatusStatistic.NotPassed") + ": {0}%"
        notpassed.Value = Percentual(stat.completedCount - stat.passedCount, stat.userCount) 'stat.completedCount - stat.passedCount / stat.userCount * 100

        Dim started As StackedBarItem = New StackedBarItem()
        started.CssClass = "started"
        started.Title = Me.Resource.getValue("StatusStatistic.Started") + ": {0}%"
        started.Value = Percentual(stat.startedCount, stat.userCount) 'stat.startedCount / stat.userCount * 100

        Dim notstarted As StackedBarItem = New StackedBarItem()
        notstarted.CssClass = "notstarted"
        notstarted.Title = Me.Resource.getValue("StatusStatistic.None") + ": {0}%"
        notstarted.Value = Percentual(stat.notStartedCount, stat.userCount) 'stat.notStartedCount / stat.userCount * 100

        ' {CssClass = "completed", Title = "Completed and Passed: {0}%", Value = complPassPerc}


        CTRLStackedBar.BarItems = {complPass, notpassed, started, notstarted}.ToList()
        CTRLStackedBar.InitializeControl({complPass, notpassed, started, notstarted})
    End Sub

    Public Overrides Sub BindNoPermessi()
        Me.ShowError(EpError.NotPermission)
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        If IsEvaluable Then

            Return Not isAutoEp AndAlso ServiceEP.HasPermessi_ByItem(Of Path)(PathID, CurrentCommunityID) AndAlso ServiceAssignment.ExistActsEvaluableAnalogic(PathID, CurrentUserId, CurrentCommRoleID)

        Else

            Return ServiceEP.HasPermessi_ByItem(Of Path)(PathID, CurrentCommunityID)

        End If
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Stat", "EduPath")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setHyperLink(Me.HYPeduPathView, False, True)
            .setLinkButton(Me.LNBexportPSstandardToCsv, False, True)
            .setLinkButton(Me.LNBexportPSfullToCsv, False, True)
            .setLinkButton(Me.LNBexportPSfullCertificationToCsv, False, True)

            .setLinkButton(Me.LNBexportPSstandardToXml, False, True)
            .setLinkButton(Me.LNBexportPSfullToXml, False, True)
            .setLinkButton(Me.LNBexportPSfullCertificationToXml, False, True)

            .setLinkButton(Me.LNBexportPSstandardToPdf, False, True)
            .setHyperLink(Me.HYPuserStat, False, True)
            '.setHyperLink(Me.HYPdetail, False, True)
            .setHyperLink(Me.HYPeduPathList, False, True)
            .setCheckBox(Me.CHBshowall)
            .setLabel(Me.LBnameTit)
            Me.LBcomplPassTit.Text = .getValue("StatusStatistic.CompletedPassed")
            Me.LBpassedTit.Text = .getValue("StatusStatistic.Passed")
            Me.LBnotStartedTit.Text = .getValue("StatusStatistic.None")
            Me.LBcompletedTit.Text = .getValue("StatusStatistic.Completed")
            Me.LBstartedTit.Text = .getValue("StatusStatistic.Started")

            .setLabel(LBstatusTit)
            .setLabel(LBactionsTit)
            .setHyperLink(Me.HYPcertificates, False, True)
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


    Public Sub LoadCertifiedDate() Handles CTRLselTime.ViewCertifiedStat

        RedirectToUrl(RootObject.PathStatistics(PathID, CurrentCommunityID, EpEndDate, CHBshowall.Checked))

    End Sub

    Private Sub ReloadStat(ByVal dateToView As DateTime?) Handles CTRLselTime.DateSelected
        RedirectToUrl(RootObject.PathStatistics(PathID, CurrentCommunityID, dateToView, CHBshowall.Checked))
    End Sub


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

    Private Sub InitHyperLink()

        ''Me.HYPuserStat.ImageUrl = RootObject.ImgUsersStat(Me.BaseUrl)
        Me.HYPuserStat.NavigateUrl = Me.BaseUrl & RootObject.UsersStatistics(Me.PathID, Me.CurrentCommunityID, ItemType.Path, 0, TimeStat, CHBshowall.Checked)

        Me.HYPeduPathList.NavigateUrl = Me.BaseUrl & RootObject.EduPathList(Me.CurrentCommunityID, EpViewModeType.Manage)

        Me.HYPeduPathView.NavigateUrl = Me.BaseUrl & RootObject.PathView(PathID, Me.CurrentCommunityID, EpViewModeType.Manage, ServiceEP.isPlayModePath(PathID))
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

    'Private Sub RPunit_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPunit.ItemDataBound
    'If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

    '    Dim dtoItem As dtoUnitGlobalStat = e.Item.DataItem
    '    Dim oLb As Label
    '    oLb = e.Item.FindControl("LBname")
    '    If isTimeEp Then

    '        oLb.Text = Me.Resource.getValue("Unit") & ": " & dtoItem.itemName ' & " (" & ServiceEP.GetTime(dtoItem.Weight) & ")"
    '    Else

    '        oLb.Text = Me.Resource.getValue("Unit") & ": " & dtoItem.itemName & " (" & dtoItem.Weight & "pt)"
    '    End If

    '    If dtoItem.userCount > -1 Then
    '        Me.CanViewStat = True
    '        If Not isAutoEp Then


    '            oLb = e.Item.FindControl("LBcomplPass")
    '            SetTypeCountOverTotal(oLb, dtoItem.compPassedCount, dtoItem.userCount)

    '            oLb = e.Item.FindControl("LBcompleted")
    '            SetTypeCountOverTotal(oLb, dtoItem.completedCount, dtoItem.userCount)

    '            oLb = e.Item.FindControl("LBpassed")
    '            SetTypeCountOverTotal(oLb, dtoItem.passedCount, dtoItem.userCount)

    '            oLb = e.Item.FindControl("LBstarted")
    '            SetTypeCountOverTotal(oLb, dtoItem.startedCount, dtoItem.userCount)

    '            oLb = e.Item.FindControl("LBnotStarted")
    '            SetTypeCountOverTotal(oLb, dtoItem.notStartedCount, dtoItem.userCount)

    '            Dim oImgGreen As System.Web.UI.WebControls.Image = e.Item.FindControl("IMGgreen")
    '            Dim oImgYellow As System.Web.UI.WebControls.Image = e.Item.FindControl("IMGyellow")
    '            Dim oImgRed As System.Web.UI.WebControls.Image = e.Item.FindControl("IMGred")
    '            oImgGreen.Visible = True
    '            oImgRed.Visible = True
    '            oImgYellow.Visible = True
    '            InitStackedBar(oImgGreen, oImgYellow, oImgRed, dtoItem)

    '            If ServiceEP.CheckStatus(dtoItem.status, Status.Mandatory) Then
    '                Dim oImg As System.Web.UI.WebControls.Image = e.Item.FindControl("IMGmandatory")
    '                oImg.Visible = True
    '                oImg.ImageUrl = RootObject.ImgMandatorySmall(Me.BaseUrl)
    '                Me.Resource.setImage(oImg, True)
    '            End If

    '            If Not IsEvaluable Then
    '                Dim oHyp As HyperLink = e.Item.FindControl("HYPdetail")
    '                oHyp.Visible = True
    '                Me.Resource.setHyperLink(oHyp, False, True)
    '                oHyp.NavigateUrl = Me.BaseUrl & RootObject.UsersStat(dtoItem.itemId, Me.CurrentCommunityID, ItemType.Unit, 0, TimeStat)

    '            End If


    '        End If

    '        Dim oLkb As LinkButton = e.Item.FindControl("LKBblocked")
    '        SetBlockedStatusLkb(oLkb, ServiceEP.CheckStatus(dtoItem.status, Status.Locked), dtoItem.itemId)

    '    Else

    '        Dim oImg As System.Web.UI.WebControls.Image = e.Item.FindControl("IMGblocked")
    '        setBlockedStatusImg(oImg, ServiceEP.CheckStatus(dtoItem.status, Status.Locked))

    '    End If

    '    If isAutoEp Then
    '        Dim TD As HtmlControl = e.Item.FindControl("TDcompleted")
    '        TD.Visible = False

    '        TD = e.Item.FindControl("TDpassed")
    '        TD.Visible = False

    '    End If

    '    Dim oRpActivities As Repeater = e.Item.FindControl("RPactivity")
    '    oRpActivities.DataSource = dtoItem.childrenStat
    '    AddHandler oRpActivities.ItemDataBound, AddressOf RPactivity_ItemDataBound
    '    oRpActivities.DataBind()

    'End If
    'End Sub



    Public Sub RPactivity_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim dtoItem As dtoActivityGlobalStat = e.Item.DataItem

            If CouldActivityWithSingleSubActivityBeOmitted And dtoItem.childrenStat.Count = 1 Then
                dtoItem.childrenStat.First().parentId = dtoItem.parentId
                dtoItem.childrenStat.First().isSingle = True

                If ServiceEP.CheckStatus(dtoItem.status, Status.Mandatory) Then
                    dtoItem.childrenStat.First().status = dtoItem.status Or Status.Mandatory
                End If
                If ServiceEP.CheckStatus(dtoItem.status, Status.Locked) Then
                    dtoItem.childrenStat.First().status = dtoItem.status Or Status.Locked
                End If

                dtoItem.childrenStat.First().Weight = dtoItem.Weight

            Else


            End If


            Dim oLb As Label
            oLb = e.Item.FindControl("LBname")
            Dim olbDet As Label
            olbDet = e.Item.FindControl("LBnameDetail")
            If isTimeEp Then

                oLb.Text = dtoItem.itemName
                olbDet.Text = " (" & ServiceEP.GetTime(dtoItem.Weight) & ")"
            Else

                oLb.Text = dtoItem.itemName
                olbDet.Text = " (" & dtoItem.Weight & "pt)"
            End If

            If dtoItem.userCount > -1 Then
                Me.CanViewStat = True
                oLb = e.Item.FindControl("LBcomplPass")
                SetTypeCountOverTotal(oLb, dtoItem.compPassedCount, dtoItem.userCount)

                oLb = e.Item.FindControl("lbcompleted")
                SetTypeCountOverTotal(oLb, dtoItem.completedCount, dtoItem.userCount)

                oLb = e.Item.FindControl("lbpassed")
                SetTypeCountOverTotal(oLb, dtoItem.passedCount, dtoItem.userCount)

                oLb = e.Item.FindControl("LBstarted")
                SetTypeCountOverTotal(oLb, dtoItem.startedCount, dtoItem.userCount)

                oLb = e.Item.FindControl("LBnotStarted")
                SetTypeCountOverTotal(oLb, dtoItem.notStartedCount, dtoItem.userCount)

                Dim stackb As UC_StackedBar = e.Item.FindControl("CTRLStackedBar")

                InitStackedBar(dtoItem, stackb)

                '        InitStackedBar(e.Item.FindControl("IMGgreen"), e.Item.FindControl("IMGyellow"), e.Item.FindControl("IMGred"), dtoItem)
                Dim oMand As HtmlControl = e.Item.FindControl("ImgMandatory")
                If ServiceEP.CheckStatus(dtoItem.status, Status.Mandatory) Then

                    oMand.Attributes("title") = Me.Resource.getValue("UnitMandatory")
                    oMand.Visible = True
                Else
                    oMand.Visible = False
                    'hideControl(oImg)
                End If
                '            Dim oImg As System.Web.UI.WebControls.Image = e.Item.FindControl("IMGmandatory")
                '            oImg.Visible = True
                '            oImg.ImageUrl = RootObject.ImgMandatorySmall(Me.BaseUrl)
                '            Me.Resource.setImage(oImg, True)
                'End If

                If Not IsEvaluable Then
                    Dim oHyp As HyperLink = e.Item.FindControl("HYPdetail")
                    Me.Resource.setHyperLink(oHyp, False, True)
                    oHyp.Text = ""
                    oHyp.NavigateUrl = Me.BaseUrl & RootObject.UsersStatistics(dtoItem.itemId, Me.CurrentCommunityID, ItemType.Activity, 0, TimeStat, CHBshowall.Checked)
                    oHyp.Visible = True
                End If

                Dim oLkb As LinkButton = e.Item.FindControl("LBActivityLock")
                SetBlockedStatusLkb(oLkb, ServiceEP.CheckStatus(dtoItem.status, Status.Locked), dtoItem.itemId)

            Else

                '        Dim oImg As System.Web.UI.WebControls.Image = e.Item.FindControl("IMGblocked")
                '        setBlockedStatusImg(oImg, ServiceEP.CheckStatus(dtoItem.status, Status.Locked))
                SetBlockStatus(e.Item.FindControl("ImgActivityLock"), ServiceEP.CheckStatus(dtoItem.status, Status.Locked))
            End If

            'SetBlockStatus(e.Item.FindControl("ImgActivityLock"), ServiceEP.CheckStatus(dtoItem.status, Status.Locked))

            If isAutoEp Then
                Dim TD As HtmlControl = e.Item.FindControl("TDcompleted")
                TD.Visible = False

                TD = e.Item.FindControl("TDpassed")
                TD.Visible = False

            End If

            Dim oRpSubAct As Repeater = e.Item.FindControl("RPsubAct")
            oRpSubAct.DataSource = dtoItem.childrenStat
            AddHandler oRpSubAct.ItemDataBound, AddressOf RPsubAct_ItemDataBound
            oRpSubAct.DataBind()
        End If
    End Sub

    Private Sub RPsubAct_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oImg As System.Web.UI.WebControls.Image
            Dim dtoItem As dtoSubActGlobalStat = e.Item.DataItem
            Dim oLb As Label
            '    oLb = e.Item.FindControl("LBtype")
            '    oLb.Text = Me.Resource.getValue("SubActivity") & ": "
            '    Dim oPlaceHolder As PlaceHolder = e.Item.FindControl("PLHaction")
            '    oPlaceHolder.Visible = True

            Dim oPlaceHolder As PlaceHolder = e.Item.FindControl("PHLAction")
            oPlaceHolder.Visible = True

            If ServiceEP.isSubActityInternalModule(dtoItem.ContentType) Then
                If isAutoEp Then
                    Dim TD As HtmlControl = e.Item.FindControl("TDcompleted")
                    TD.Visible = False

                    TD = e.Item.FindControl("TDpassed")
                    TD.Visible = False
                End If

                Dim initializer As New dtoInternalActionInitializer
                initializer.IdPath = PathID
                initializer.IdPathCommunity = Me.CurrentCommunityID
                initializer.CookieName = CookieName ' "PathCertifications_" & initializer.IdPath
                initializer.SubActivity = dtoItem.SubActivity
                If dtoItem.isSingle Then
                    Dim weight = ""
                    If isTimeEp Then
                        weight = " (" & ServiceEP.GetTime(dtoItem.Weight) & ")"
                    Else
                        weight = " (" & dtoItem.Weight & "pt)"
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

                        ' DIMENSIONI IMMAGINI
                        oCertAction.IconSize = Helpers.IconSize.Small
                        oCertAction.EnableAnchor = True

                        oCertAction.InitializeControl(initializer)

                        Dim ohypStat As HyperLink = e.Item.FindControl("HYPstats")
                        Me.Resource.setHyperLink(ohypStat, False, True)
                        ohypStat.Visible = True

                        ohypStat.NavigateUrl = Me.BaseUrl & RootObject.PathSubActivityCertifications(PathID, dtoItem.SubActivity.Id, Me.CurrentCommunityID, TimeStat)
                        ohypStat.Visible = False 'TEMP HIDDEN
                End Select

                'Dim oDisplayAction As lm.Comol.Modules.EduPath.Presentation.IViewModuleTextAction = e.Item.FindControl("CTRLtextAction")

                'e.Item.FindControl("CTRLtextAction").Visible = True

                'If isAutoEp Then
                '    Dim TD As HtmlControl = e.Item.FindControl("TDcompleted")
                '    TD.Visible = False

                '    TD = e.Item.FindControl("TDpassed")
                '    TD.Visible = False
                'End If

                'oDisplayAction.RefreshContainer = True
                'Dim initializer As New dtoInternalActionInitializer
                'initializer.SubActivity = New dtoSubActivity() With {.Id = dtoItem.itemId, .Description = dtoItem.itemName}
                '' AGGIUNTA PLACEHOLDER
                '' --> 
                'If dtoItem.isSingle Then
                '    Dim weight = ""
                '    If isTimeEp Then
                '        weight = " (" & ServiceEP.GetTime(dtoItem.Weight) & ")"
                '    Else
                '        weight = " (" & dtoItem.Weight & "pt)"
                '    End If
                '    initializer.PlaceHolders.Add(New lm.Comol.Core.ModuleLinks.dtoPlaceHolder() With {.Text = weight, .Type = lm.Comol.Core.ModuleLinks.PlaceHolderType.three, .CssClass = "duration"})
                'End If
                'oDisplayAction.IconSize = Helpers.IconSize.Small
                'oDisplayAction.EnableAnchor = True
                'initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.text
                'oDisplayAction.InitializeControl(initializer)

            Else

                If isAutoEp Then
                    Dim TD As HtmlControl = e.Item.FindControl("TDcompleted")
                    TD.Visible = False

                    TD = e.Item.FindControl("TDpassed")
                    TD.Visible = False
                End If

                Dim oDisplayAction As lm.Comol.Core.ModuleLinks.IExternalModuleDisplayAction = CType(LoadControl(BaseUrl & lm.Comol.Core.DomainModel.Common.CoreRootObject.GenericNewDisplayActionControl), lm.Comol.Core.ModuleLinks.IExternalModuleDisplayAction)
                Dim initializer As New lm.Comol.Core.ModuleLinks.dtoExternalModuleInitializer
                initializer.Link = dtoItem.ModuleLink

                ' AGGIUNTA PLACEHOLDER
                ' --> 
                'initializer.PlaceHolders.Add(New lm.Comol.Core.ModuleLinks.dtoPlaceHolder() With {.Type = lm.Comol.Core.ModuleLinks.PlaceHolderType.three})
                ' DEFINISCO UNA CLASSE PER IL CONTAINER
                oDisplayAction.ContainerCSS = ""
                ' DIMENSIONI IMMAGINI
                oDisplayAction.IconSize = Helpers.IconSize.Small
                oDisplayAction.EnableAnchor = True

                If dtoItem.isSingle Then
                    Dim weight = ""
                    If isTimeEp Then
                        weight = " (" & ServiceEP.GetTime(dtoItem.Weight) & ")"
                    Else
                        weight = " (" & dtoItem.Weight & "pt)"
                    End If

                    initializer.PlaceHolders.Add(New lm.Comol.Core.ModuleLinks.dtoPlaceHolder() With {.Text = weight, .Type = lm.Comol.Core.ModuleLinks.PlaceHolderType.three, .CssClass = "duration"})

                End If

                'DISPLAY PLAY MODE
                ' --> oDisplayAction.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction
                'DISPLAY PLAY MODE SE NON POSSO ESEGUIRE AZIONE
                ' --> oDisplayAction.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.text
                'DISPLAY EDIT MODE
                ' -->  

                Dim actions As List(Of dtoModuleActionControl)

                If dtoItem.ContentType = SubActivityType.Quiz Then
                    oDisplayAction.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.textDefault
                Else
                    oDisplayAction.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.text
                End If
                'lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction Or lm.Comol.Core.ModuleLinks.DisplayActionMode.actions
                actions = oDisplayAction.InitializeRemoteControl(initializer, StandardActionType.EditMetadata Or StandardActionType.ViewAdvancedStatistics)


                'DISPLAY STATISTICHE
                ' --> oDisplayAction.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.text



                ' PLAY MODE
                ' --> oDisplayAction.InitializeControl(initializer)
                'DISPLAY EDIT MODE
                ' -->oDisplayAction.InitializeControl(initializer, StandardActionType.Edit Or StandardActionType.DownloadItem Or StandardActionType.EditMetadata Or StandardActionType.ViewAdvancedStatistics)
                'DISPLAY STATISTICHE amministratore generiche

                'DISPLAY STATISTICHE amministratore su utente
                ' --actions = oDisplayAction.InitializeRemoteControl(idUser,initializer, StandardActionType.DownloadItem Or StandardActionType.EditMetadata Or StandardActionType.ViewAdvancedStatistics)

                'DISPLAY STATISTICHE utente
                ' -->actions = oDisplayAction.InitializeRemoteControl(initializer, StandardActionType.ViewPersonalStatistics)
                ' Display TOTALE

                'oDisplayAction.InitializeControl(initializer, StandardActionType.Play Or StandardActionType.DownloadItem Or StandardActionType.EditMetadata Or StandardActionType.ViewAdvancedStatistics Or StandardActionType.ViewUserStatistics Or StandardActionType.Edit Or StandardActionType.ViewPersonalStatistics)

                Dim actStat = actions.Where(Function(x) x.ControlType = StandardActionType.ViewAdvancedStatistics).FirstOrDefault()

                If (Not actStat Is Nothing) Then
                    Dim ohypStat As HyperLink = e.Item.FindControl("HYPstats")
                    Me.Resource.setHyperLink(ohypStat, False, True)
                    ohypStat.Visible = True
                    ohypStat.NavigateUrl = actStat.LinkUrl
                End If


                Dim actHYPeditMetadata = actions.Where(Function(x) x.ControlType = StandardActionType.EditMetadata).FirstOrDefault()

                If (Not actHYPeditMetadata Is Nothing) Then
                    Dim ohypHYPeditMetadata As HyperLink = e.Item.FindControl("HYPeditMetadata")
                    Me.Resource.setHyperLink(ohypHYPeditMetadata, False, True)
                    ohypHYPeditMetadata.Visible = True
                    ohypHYPeditMetadata.NavigateUrl = actHYPeditMetadata.LinkUrl
                End If

                If dtoItem.ContentType = SubActivityType.Quiz Then
                    Dim actQuizStat = actions.Where(Function(x) x.ControlType = StandardActionType.ViewAdministrationCharts).FirstOrDefault()

                    If (Not actQuizStat Is Nothing) Then
                        Dim ohypHYPquizstats As HyperLink = e.Item.FindControl("HYPquizstats")
                        Me.Resource.setHyperLink(ohypHYPquizstats, False, True)
                        ohypHYPquizstats.Visible = True
                        ohypHYPquizstats.NavigateUrl = actQuizStat.LinkUrl
                    End If
                End If



                oPlaceHolder.Controls.Add(oDisplayAction)


                'Dim oDisplayAction As lm.Comol.Core.DomainModel.Common.iModuleActionView = CType(LoadControl(BaseUrl & lm.Comol.Core.DomainModel.Common.CoreRootObject.GenericDisplayActionControl), lm.Comol.Core.DomainModel.Common.iModuleActionView)
                ''oDisplayAction.InitializeRemoteControlInlineByLink(True, dtoItem.ModuleLink)
                'oDisplayAction.InitializeControlByActionType(dtoItem.ModuleLink, StandardActionType.ViewAdvancedStatistics)
                'oPlaceHolder.Controls.Add(oDisplayAction)
            End If



            '    oLb = e.Item.FindControl("LBpoint")
            '    If isTimeEp Then
            '        hideControl(oLb)

            '    Else
            '        oLb.Text = " (" & dtoItem.Weight & ")"

            '    End If

            If dtoItem.userCount > -1 Then

                oLb = e.Item.FindControl("LBcomplPass")
                SetTypeCountOverTotal(oLb, dtoItem.compPassedCount, dtoItem.userCount)


                '        oLb = e.Item.FindControl("LBcompleted")
                '        SetTypeCountOverTotal(oLb, dtoItem.completedCount, dtoItem.userCount)

                '        oLb = e.Item.FindControl("LBpassed")
                '        SetTypeCountOverTotal(oLb, dtoItem.passedCount, dtoItem.userCount)

                oLb = e.Item.FindControl("LBstarted")
                SetTypeCountOverTotal(oLb, dtoItem.startedCount, dtoItem.userCount)

                oLb = e.Item.FindControl("LBnotStarted")
                SetTypeCountOverTotal(oLb, dtoItem.notStartedCount, dtoItem.userCount)
                '        InitStackedBar(e.Item.FindControl("IMGgreen"), e.Item.FindControl("IMGyellow"), e.Item.FindControl("IMGred"), dtoItem)

                Dim stackb As UC_StackedBar = e.Item.FindControl("CTRLStackedBar")

                InitStackedBar(dtoItem, stackb)

                If ServiceEP.CheckStatus(dtoItem.status, Status.Mandatory) Then
                    '            oImg = e.Item.FindControl("IMGmandatory")
                    '            oImg.Visible = True
                    '            oImg.ImageUrl = RootObject.ImgMandatorySmall(Me.BaseUrl)
                    '            Me.Resource.setImage(oImg, True)
                    e.Item.FindControl("IMGmandatory").Visible = True
                Else
                    e.Item.FindControl("IMGmandatory").Visible = False
                End If

                Dim oHyp As HyperLink
                If IsEvaluable Then
                    'oHyp = e.Item.FindControl("HYPevaluate")
                    'oHyp.Visible = True
                    'Me.Resource.setHyperLink(oHyp, False, True)
                    'oHyp.NavigateUrl = Me.BaseUrl & RootObject.EvaluateSubAct(dtoItem.itemId, Me.CurrentCommunityID, EvaluationFilter.NotEvaluated, EpEndDate)
                    'oHyp.Enabled = (dtoItem.completedCount > 0) OrElse (dtoItem.compPassedCount > 0)
                Else
                    oHyp = e.Item.FindControl("HYPdetail")
                    oHyp.Visible = True
                    Me.Resource.setHyperLink(oHyp, False, True)
                    oHyp.Text = ""
                    oHyp.NavigateUrl = Me.BaseUrl & RootObject.UsersStatistics(dtoItem.itemId, Me.CurrentCommunityID, ItemType.SubActivity, 0, TimeStat, CHBshowall.Checked)

                End If

                Dim oLkb As LinkButton = e.Item.FindControl("LBSubActivityLock")
                SetBlockedStatusLkb(oLkb, ServiceEP.CheckStatus(dtoItem.status, Status.Locked), dtoItem.itemId)

            Else
                SetBlockStatus(e.Item.FindControl("ImgSubActivityLock"), ServiceEP.CheckStatus(dtoItem.status, Status.Locked))
                '        oImg = e.Item.FindControl("IMGblocked")
                '        setBlockedStatusImg(oImg, ServiceEP.CheckStatus(dtoItem.status, Status.Locked))
                '    End If



                'SetBlockStatus(e.Item.FindControl("ImgSubActivityLock"), ServiceEP.CheckStatus(dtoItem.status, Status.Locked))
            End If
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
                Case "blockedS"
                    If ServiceEP.UpdateSubActivityVisibilityStatus(e.CommandArgument, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then
                        BindDati()
                    Else
                        ShowError(EpError.Generic)
                    End If
            End Select

        Catch ex As Exception

        End Try
    End Sub

    'Private Sub LKBblocked_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBblocked.Click

    '    Select Case ServiceEP.UpdatePathVisibilityStatus(Me.LKBblocked.CommandArgument, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress)
    '        Case 1
    '            BindDati()
    '        Case 0
    '            ShowError(EpError.Data)
    '        Case 2
    '            LTalert.Text = Resource.getValue("MSGalert")
    '        Case 3
    '            LTalert.Text = Resource.getValue("EpError.EmptyActivities")
    '    End Select
    'End Sub

    Private Sub SetBlockedStatusLkb(ByRef oLkb As LinkButton, ByVal isBlocked As Boolean, ByRef itemID As Long)

        Me.Resource.setLinkButton(oLkb, False, True)
        oLkb.CommandArgument = itemID
        'oLkb.Text = "<img src=" & """" & "{0}" & """" & " alt=" & """" & "{1}" & """" & "/>"
        oLkb.Visible = True
        If isBlocked Then
            oLkb.ToolTip = Me.Resource.getValue("Blocked.True")
            oLkb.CssClass = "icon locked"
        Else
            oLkb.ToolTip = Me.Resource.getValue("Blocked.False")
            oLkb.CssClass = "icon unlocked"
        End If

        'oLkb.Text = String.Format(oLkb.Text, RootObject.ImgBtnBlocked(Me.BaseUrl, Not isBlocked), oLkb.ToolTip)

    End Sub

    Private Sub setBlockedStatusImg(ByRef oImg As System.Web.UI.WebControls.Image, ByVal isBlocked As Boolean)
        'oImg.ImageUrl = RootObject.ImgItemBlocked(Me.BaseUrl, Not isBlocked)
        'oImg.AlternateText = Resource.getValue("Blocked." & isBlocked)
    End Sub


    Private Sub InitStackedBar(ByRef ImgGreen As System.Web.UI.WebControls.Image, ByRef ImgYellow As System.Web.UI.WebControls.Image, ByRef ImgRed As System.Web.UI.WebControls.Image, ByRef dtoItemStat As dtoGenericGlobalStat)
        'Const barDimension As Int16 = 100
        'Dim dividend As Int16 = barDimension / dtoItemStat.userCount
        'If dtoItemStat.compPassedCount > 0 Then
        '    ImgGreen.ImageUrl = RootObject.ImgRectangleGreen(Me.BaseUrl)
        '    ImgGreen.Visible = True
        '    ImgGreen.Width = dtoItemStat.compPassedCount * dividend
        '    ImgGreen.AlternateText = Me.Resource.getValue("PassedCompleted.Item") & dtoItemStat.compPassedCount
        '    ImgGreen.ToolTip = Me.Resource.getValue("PassedCompleted.Item") & dtoItemStat.compPassedCount
        '    ImgGreen.Style("Display") = "inline"
        'Else
        '    ImgGreen.Width = 0
        '    ImgGreen.Style("Display") = "none"
        'End If
        'Dim started As Int16 = dtoItemStat.completedCount + dtoItemStat.startedCount + dtoItemStat.passedCount
        'If started > 0 Then
        '    ImgYellow.ImageUrl = RootObject.ImgRectangleYellow(Me.BaseUrl)
        '    ImgYellow.Visible = True
        '    ImgYellow.Width = started * dividend
        '    ImgYellow.AlternateText = Me.Resource.getValue("Started.Item") & started
        '    ImgYellow.ToolTip = Me.Resource.getValue("Started.Item") & started
        '    ImgYellow.Style("Display") = "inline"
        'Else
        '    ImgYellow.Width = 0
        '    ImgYellow.Style("Display") = "none"
        'End If
        'If dtoItemStat.notStartedCount > 0 Then
        '    ImgRed.ImageUrl = RootObject.ImgRectangleRed(Me.BaseUrl)
        '    ImgRed.Visible = True
        '    ImgRed.Width = barDimension - ImgGreen.Width.Value - ImgYellow.Width.Value   'dtoItemStat.notStartedCount * dividend
        '    ImgRed.AlternateText = Me.Resource.getValue("NotStarted.Item") & dtoItemStat.notStartedCount
        '    ImgRed.ToolTip = Me.Resource.getValue("NotStarted.Item") & dtoItemStat.notStartedCount
        '    ImgRed.Style("Display") = "inline"
        'Else
        '    ImgRed.Style("Display") = "none"
        'End If
    End Sub

    Private Sub SetTypeCountOverTotal(ByRef oLb As Label, ByRef typeCount As Int16, ByRef totalCount As Int16)
        oLb.Text = typeCount & "/" & totalCount
    End Sub

#Region "Export Data"
    Private Sub LNBcsv_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBexportPSstandardToCsv.Click
        ExportStandardStatistics(Helpers.Export.ExportFileType.csv)
    End Sub
    Private Sub LNBxml_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBexportPSstandardToXml.Click
        ExportStandardStatistics(Helpers.Export.ExportFileType.xml)
    End Sub
    Private Sub LNBpdf_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBexportPSstandardToPdf.Click
        ExportStandardStatistics(Helpers.Export.ExportFileType.pdf)
    End Sub
    Private Sub ExportStandardStatistics(exportType As lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType)
        Dim cookie As HttpCookie
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
            Dim settings As dtoExportConfigurationSetting = ServiceEP.GetExportSetting(Me.PathID, CurrentCommunityID, StatisticsPageType.PathStatistic, ConfigurationType.Export)

            Dim oDisplayAction As lm.Comol.Core.ModuleLinks.IExternalModuleDisplayAction = CType(LoadControl(BaseUrl & lm.Comol.Core.DomainModel.Common.CoreRootObject.GenericNewDisplayActionControl), lm.Comol.Core.ModuleLinks.IExternalModuleDisplayAction)
            Dim textAction As lm.Comol.Modules.EduPath.Presentation.IViewModuleTextAction = CType(LoadControl(BaseUrl & RootObject.RenderTextAction), lm.Comol.Modules.EduPath.Presentation.IViewModuleTextAction)
            Dim certAction As lm.Comol.Modules.EduPath.Presentation.IViewModuleCertificationAction = CType(LoadControl(BaseUrl & RootObject.RenderCertificationAction), lm.Comol.Modules.EduPath.Presentation.IViewModuleCertificationAction)

            If exportType = Helpers.Export.ExportFileType.pdf Then
                Dim oTemplate As New lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template
                'oTemplate.Header = Master.getTemplateHeader
                'oTemplate.Footer = Master.getTemplateFooter
                oTemplate.Settings = lm.Comol.Core.DomainModel.Helpers.Export.ExportBaseHelper.GetDefaultPageSettings()
                oTemplate.Settings.Size = DocTemplateVers.PageSize.A4_L

                Dim doc As iTextSharp5.text.Document = ServiceEP.ServiceStat.ExportGlobalEpStats_ToPdf(PageUtility.CurrentContext, Me.PathID, Me.CurrentCommunityID, Me.CurrentUserId, Me.CurrentCommRoleID, Me.CHBshowall.Checked, translations, roleTranslations, oDisplayAction, textAction, certAction, oTemplate, False, clientFileName, Response, New HttpCookie(CookieName, HDNdownloadTokenValue.Value), TimeStat)

                If IsNothing(doc) Then
                    ShowError(EpError.Generic)
                End If
            Else
                Response.AppendCookie(cookie)
                Response.AddHeader("Content-Disposition", "attachment; filename=" & clientFileName)
                Response.Charset = ""
                Response.ContentEncoding = System.Text.Encoding.Default
                Select Case exportType
                    Case Helpers.Export.ExportFileType.xml
                        Response.ContentType = "application/ms-excel"
                        Response.Write(ServiceEP.ServiceStat.ExportGlobalEpStats_ToXml(PageUtility.CurrentContext, Me.PathID, Me.CurrentCommunityID, Me.CurrentUserId, Me.CurrentCommRoleID, Me.CHBshowall.Checked, translations, roleTranslations, oDisplayAction, textAction, certAction, TimeStat))
                    Case Else
                        Response.ContentType = "text/csv"
                        Response.Write(ServiceEP.ServiceStat.ExportGlobalEpStats_ToCsv(PageUtility.CurrentContext, Me.PathID, Me.CurrentCommunityID, Me.CurrentUserId, Me.CurrentCommRoleID, Me.CHBshowall.Checked, translations, roleTranslations, oDisplayAction, textAction, certAction, TimeStat))
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
    Private Function GetFileName(ByVal type As lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType) As String
        Dim filename As String = Resource.getValue("Export.Filename.PathStatistics")
        Dim oDate As DateTime = DateTime.Now
        If String.IsNullOrEmpty(filename) Then
            filename = "PathStatistics_{0}_{1}_{2}"
        End If
        Return String.Format(filename, oDate.Year, oDate.Month, oDate.Day) & IIf(type <> Helpers.Export.ExportFileType.pdf, "." & type.ToString, "")
    End Function
    Private Function GetFileName(ByVal type As lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType, onlyForCertifications As Boolean) As String
        Dim filename As String = Resource.getValue("Export.Filename.FullPathData." & onlyForCertifications.ToString)
        Dim oDate As DateTime = DateTime.Now
        If String.IsNullOrEmpty(filename) Then
            If onlyForCertifications Then
                filename = "Path_{0}_CertificationStatistics_{1}_{2}_{3}"
            Else
                filename = "Path_{0}_FullStatistics_{1}_{2}_{3}"
            End If
        End If
        Dim displayname As String = ServiceEP.GetPathName(PathID)
        If displayname.Length > 50 Then
            displayname = Left(displayname, 60)
        End If
        Return lm.Comol.Core.DomainModel.Helpers.Export.ExportBaseHelper.HtmlCheckFileName(String.Format(filename, displayname, oDate.Year, oDate.Month, oDate.Day) & IIf(type <> lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType.pdf, "." & type.ToString, ""))
    End Function
    Private Sub LNBexportPSfullCertificationToCsv_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBexportPSfullCertificationToCsv.Click
        ExportFullStatistics(Helpers.Export.ExportFileType.csv, ExporPathData.Certification)
    End Sub
    Private Sub LNBexportPSfullCertificationToXml_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBexportPSfullCertificationToXml.Click
        ExportFullStatistics(Helpers.Export.ExportFileType.xml, ExporPathData.Certification)
    End Sub
    Private Sub LNBexportPSfullToCsv_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBexportPSfullToCsv.Click
        ExportFullStatistics(Helpers.Export.ExportFileType.csv, ExporPathData.Full)
    End Sub
    Private Sub LNBexportPSfullToXml_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBexportPSfullToXml.Click
        ExportFullStatistics(Helpers.Export.ExportFileType.xml, ExporPathData.Full)
    End Sub
    Private Sub ExportFullStatistics(exportType As lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType, ByVal export As ExporPathData)
        Dim cookie As HttpCookie
        If Not String.IsNullOrEmpty(CookieName) Then
            cookie = New HttpCookie(CookieName, HDNdownloadTokenValue.Value)
        End If

        'Dim test As DateTime? = DateTime.Now.AddDays(-2)

        Dim stat As dtoPathUsers = ServiceStat.GetPathUsersStat(Me.PathID, TimeStat, 0, 0, CHBshowall.Checked, (export = ExporPathData.Certification))

        Dim clientFileName As String = GetFileName(exportType, (export = ExporPathData.Certification))
        Dim translations As New Dictionary(Of EduPathTranslations, String)
        For Each name As String In [Enum].GetNames(GetType(EduPathTranslations))
            translations.Add([Enum].Parse(GetType(EduPathTranslations), name), Me.Resource.getValue("EduPathTranslations." & name))
        Next
        Dim roles As List(Of Comol.Entity.Role) = COL_BusinessLogic_v2.CL_permessi.COL_TipoRuolo.List(PageUtility.LinguaID).OrderBy(Function(t) t.Name).ToList
        Dim roleTranslations As Dictionary(Of Integer, String) = roles.ToDictionary(Function(r) r.ID, Function(r) r.Name)

        Response.Clear()
        Try
            Dim settings As dtoExportConfigurationSetting = ServiceEP.GetExportSetting(Me.PathID, CurrentCommunityID, StatisticsPageType.UsersStatistics, ConfigurationType.Export)

            Response.AppendCookie(cookie)
            Response.AddHeader("Content-Disposition", "attachment; filename=" & clientFileName)
            Response.Charset = ""
            Response.ContentEncoding = System.Text.Encoding.Default
            Select Case exportType
                Case Helpers.Export.ExportFileType.xml
                    Response.ContentType = "application/ms-excel"
                    Dim hXml As New HelperExportToXml(New lm.Comol.Core.Business.BaseModuleManager(PageUtility.CurrentContext), translations, roleTranslations)
                    Response.Write(hXml.PathUsersStatistics(CurrentUserId, TimeStat, stat, GetQuizInfos(stat.Questionnaires, stat.QuestionnaireStatistics, settings.isRequiredField(ExportFieldType.QuestionnaireAttempts, export), (export = ExporPathData.Certification)), settings, export))
                Case Else
                    Response.ContentType = "text/csv"
                    Dim hCsv As New HelperExportToCsv(New lm.Comol.Core.Business.BaseModuleManager(PageUtility.CurrentContext), translations, roleTranslations)
                    Response.Write(hCsv.PathUsersStatistics(CurrentUserId, TimeStat, stat, GetQuizInfos(stat.Questionnaires, stat.QuestionnaireStatistics, settings.isRequiredField(ExportFieldType.QuestionnaireAttempts, export), (export = ExporPathData.Certification)), settings, export))
            End Select
        Catch ex As Exception
            Select Case exportType
                Case Helpers.Export.ExportFileType.xml
                    Response.Write(HelperExportToXml.GetErrorDocument(translations(EduPathTranslations.FileCreationError), translations(EduPathTranslations.StatisticsInfo), TimeStat))
                Case Else
                    Response.Write(HelperExportToCsv.GetErrorDocument(translations(EduPathTranslations.FileCreationError)))
            End Select
        End Try
        Response.End()
    End Sub


    Private Function GetQuizInfos(questionnaires As List(Of dtoSubActivityQuestionnaire), qInfos As List(Of dtoBaseUserPathQuiz), showAttempts As Boolean, onlyForCertifications As Boolean) As List(Of dtoBaseUserPathQuiz)
        For Each q As dtoSubActivityQuestionnaire In questionnaires
            Dim questionnaire As COL_Questionario.LazyQuestionnaire = QSservice.GetItem(Of COL_Questionario.LazyQuestionnaire)(CInt(q.IdQuestionnaire))
            If Not IsNothing(questionnaire) Then
                q.QuestionnaireInfo.AllowMultipleAttempts = (questionnaire.IdType = COL_Questionario.QuestionnaireType.RandomMultipleAttempts) OrElse (questionnaire.MaxAttempts > 1)
                ' DA VERIFICARE CON I PERCORSI FORMATIVI A VALUTAZIONE !!!
                q.QuestionnaireInfo.EvaluationActive = (questionnaire.IdType = COL_Questionario.QuestionnaireType.RandomMultipleAttempts)
                q.QuestionnaireInfo.MaxAttemptsNumber = questionnaire.MaxAttempts
                q.QuestionnaireInfo.Name = QSservice.GetItemName(q.IdQuestionnaire, LinguaID)
                q.QuestionnaireInfo.MinScore = questionnaire.MinScore
                q.QuestionnaireInfo.EvaluationScale = questionnaire.EvaluationScale

                Dim answers As List(Of COL_Questionario.dtoFullUserAnswerItem) = QSservice.GetQuestionnaireAnswers(CInt(q.IdQuestionnaire), qInfos.Select(Function(i) i.IdPerson).ToList(), IIf(Not showAttempts, COL_Questionario.AnswerStatus.Completed, COL_Questionario.AnswerStatus.All), onlyForCertifications)
                If (Not IsNothing(answers) AndAlso answers.Any()) Then
                    For Each pQuiz As dtoBaseUserPathQuiz In qInfos.Where(Function(i) i.IdQuestionnaire.Equals(q.IdQuestionnaire)).ToList()
                        Dim found As Boolean = answers.Where(Function(a) a.Answer.IdPerson.Equals(pQuiz.IdPerson)).Any()
                        For Each uAnswer As COL_Questionario.dtoFullUserAnswerItem In answers.Where(Function(a) a.Answer.IdPerson.Equals(pQuiz.IdPerson)).Select(Function(a) a).ToList()
                            pQuiz.Answers.Add(New dtoUserQuizAnswer() With {.AttemptNumber = uAnswer.AttemptNumber, .CompletedOn = uAnswer.Answer.CompletedOn, .CorrectAnswers = uAnswer.Answer.CorrectAnswers, _
                                                                           .IdAnswer = uAnswer.Id, .IsDeleted = uAnswer.Answer.IsDeleted, .QuestionsCount = uAnswer.Answer.QuestionsCount, .QuestionsSkipped = uAnswer.Answer.QuestionsSkipped, _
                                                                            .RelativeScore = uAnswer.Answer.RelativeScore, .Score = uAnswer.Answer.Score, .SemiCorrectAnswers = uAnswer.Answer.SemiCorrectAnswers, .UngradedAnswers = uAnswer.Answer.UngradedAnswers, .WrongAnswers = uAnswer.Answer.WrongAnswers})
                            pQuiz.EvaluationsInfo.Add(uAnswer.Id, QSservice.CalculateComplation(uAnswer.Answer))
                        Next
                    Next
                End If
            End If
        Next
        Return qInfos
    End Function
#End Region

    Protected Function isSingle(dtoitem As dtoSubActGlobalStat)
        If dtoitem.isSingle Then
            Return " single"
        Else
            Return ""
        End If
    End Function

    Protected Function ActivityUnit(dtoitem As dtoSubActGlobalStat)
        If dtoitem.isSingle Then
            Return "unit"
        Else
            Return "activity"
        End If
    End Function

    Protected Function HiddenActivity(dtoitem As dtoActivityGlobalStat)
        If (CouldActivityWithSingleSubActivityBeOmitted And dtoitem.childrenStat.Count = 1) Then
            Return " hiddenActivity"
        Else
            Return ""
        End If
    End Function

    Private Sub RPunit_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPunit.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim dtoItem As dtoUnitGlobalStat = e.Item.DataItem
            Dim oLb As Label
            oLb = e.Item.FindControl("LBname")
            If isTimeEp Then

                oLb.Text = dtoItem.itemName ' & " (" & ServiceEP.GetTime(dtoItem.Weight) & ")"
            Else

                oLb.Text = dtoItem.itemName
                Dim olbDet As Label
                olbDet = e.Item.FindControl("LBnameDetail")
                olbDet.Text = " (" & dtoItem.Weight & "pt)"

            End If

            If dtoItem.userCount > -1 Then
                Me.CanViewStat = True
                If Not isAutoEp Then


                    oLb = e.Item.FindControl("LBcomplPass")
                    SetTypeCountOverTotal(oLb, dtoItem.compPassedCount, dtoItem.userCount)

                    oLb = e.Item.FindControl("LBcompleted")
                    SetTypeCountOverTotal(oLb, dtoItem.completedCount, dtoItem.userCount)

                    oLb = e.Item.FindControl("LBpassed")
                    SetTypeCountOverTotal(oLb, dtoItem.passedCount, dtoItem.userCount)

                    oLb = e.Item.FindControl("LBstarted")
                    SetTypeCountOverTotal(oLb, dtoItem.startedCount, dtoItem.userCount)

                    oLb = e.Item.FindControl("LBnotStarted")
                    SetTypeCountOverTotal(oLb, dtoItem.notStartedCount, dtoItem.userCount)

                    '        Dim oImgGreen As System.Web.UI.WebControls.Image = e.Item.FindControl("IMGgreen")
                    '        Dim oImgYellow As System.Web.UI.WebControls.Image = e.Item.FindControl("IMGyellow")
                    '        Dim oImgRed As System.Web.UI.WebControls.Image = e.Item.FindControl("IMGred")
                    '        oImgGreen.Visible = True
                    '        oImgRed.Visible = True
                    '        oImgYellow.Visible = True
                    '        InitStackedBar(oImgGreen, oImgYellow, oImgRed, dtoItem)

                    '        If ServiceEP.CheckStatus(dtoItem.status, Status.Mandatory) Then
                    '            Dim oImg As System.Web.UI.WebControls.Image = e.Item.FindControl("IMGmandatory")
                    '            oImg.Visible = True
                    '            oImg.ImageUrl = RootObject.ImgMandatorySmall(Me.BaseUrl)
                    '            Me.Resource.setImage(oImg, True)
                    '        End If

                    If Not IsEvaluable Then
                        Dim oHyp As HyperLink = e.Item.FindControl("HYPdetail")
                        oHyp.Visible = True
                        Me.Resource.setHyperLink(oHyp, False, True)
                        oHyp.Text = ""
                        oHyp.NavigateUrl = Me.BaseUrl & RootObject.UsersStatistics(dtoItem.itemId, Me.CurrentCommunityID, ItemType.Unit, 0, TimeStat, CHBshowall.Checked)

                    End If


                End If

                Dim oLkb As LinkButton = e.Item.FindControl("LBUnitLock")
                SetBlockedStatusLkb(oLkb, ServiceEP.CheckStatus(dtoItem.status, Status.Locked), dtoItem.itemId)

                'SetBlockStatus(e.Item.FindControl("ImgUnitLock"), ServiceEP.CheckStatus(dtoItem.status, Status.Locked))

            Else
                SetBlockStatus(e.Item.FindControl("ImgUnitLock"), ServiceEP.CheckStatus(dtoItem.status, Status.Locked))
                '    Dim oImg As System.Web.UI.WebControls.Image = e.Item.FindControl("IMGblocked")
                '    setBlockedStatusImg(oImg, ServiceEP.CheckStatus(dtoItem.status, Status.Locked))

            End If

            If isAutoEp Then
                Dim TD As HtmlControl = e.Item.FindControl("TDcompleted")
                TD.Visible = False

                TD = e.Item.FindControl("TDpassed")
                TD.Visible = False

            End If

            Dim oRpActivities As Repeater = e.Item.FindControl("RPactivity")

            oRpActivities.DataSource = dtoItem.childrenStat
            AddHandler oRpActivities.ItemDataBound, AddressOf RPactivity_ItemDataBound
            oRpActivities.DataBind()

        End If
    End Sub

    Private Sub LBPathLock_Click(sender As Object, e As System.EventArgs) Handles LBPathLock.Click
        Select Case ServiceEP.UpdatePathVisibilityStatus(Me.LBPathLock.CommandArgument, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress)
            Case 1
                BindDati()
            Case 0
                ShowError(EpError.Data)
            Case 2
                'LTalert.Text = Resource.getValue("MSGalert")
            Case 3
                'LTalert.Text = Resource.getValue("EpError.EmptyActivities")
        End Select
    End Sub

    'Protected Function PathType() As String
    '    Return ""
    'End Function

    Protected Function PathIdString() As String
        Return PathID.ToString()
    End Function


    Protected ReadOnly Property PathTypeClass As String
        Get
            If (isAutoEp) Then
                Return "auto"
            Else
                Return ""
            End If

        End Get
    End Property
    Protected ReadOnly Property ExportStatistics As String
        Get
            Return Me.Resource.getValue("ExportStatistics")
        End Get
    End Property
    Protected ReadOnly Property UnitMandatory As String
        Get
            Return Me.Resource.getValue("IMGmandatory.AlternateText")
        End Get
    End Property
    Protected ReadOnly Property ActivityMandatory As String
        Get
            Return Me.Resource.getValue("IMGmandatory.AlternateText")
        End Get
    End Property
    Protected ReadOnly Property SubActivityMandatory As String
        Get
            Return Me.Resource.getValue("IMGmandatory.AlternateText")
        End Get
    End Property
    Protected ReadOnly Property Statistics As String
        Get
            Return "" 'Me.Resource.getValue("IMGmandatory.AlternateText")
        End Get
    End Property
    Protected ReadOnly Property EditMetadata As String
        Get
            Return "" 'Me.Resource.getValue("IMGmandatory.AlternateText")
        End Get
    End Property
    Protected ReadOnly Property ActivityLocal As String
        Get
            Return Me.Resource.getValue("Activity")
        End Get
    End Property
    Protected ReadOnly Property UnitLocal As String
        Get
            Return Me.Resource.getValue("Unit")
        End Get
    End Property
    Protected ReadOnly Property PathLocal As String
        Get
            Return Me.Resource.getValue("Path")
        End Get
    End Property
    Protected ReadOnly Property SubActivityLocal As String
        Get
            Return Me.Resource.getValue("SubActivity")
        End Get
    End Property
    Protected ReadOnly Property ItemBlocked(dtoitem As dtoGenericGlobalStat) As String
        Get
            If ServiceEP.CheckStatus(dtoitem.status, Status.Locked) Then
                Return Me.Resource.getValue("Blocked.True")
            Else
                Return Me.Resource.getValue("Blocked.False")
            End If

        End Get
    End Property
    'Protected ReadOnly Property ActivityBlocked(dtoitem As dtoActivityGlobalStat) As String
    '    Get
    '        Return Me.Resource.getValue(ServiceEP.CheckStatus(dtoitem.status, Status.Locked))
    '    End Get
    'End Property
    'Protected ReadOnly Property SubActivityBlocked(dtoitem As dtoSubActGlobalStat) As String
    '    Get
    '        Return Me.Resource.getValue(ServiceEP.CheckStatus(dtoitem.status, Status.Locked))
    '    End Get
    'End Property

    Private Sub CHBshowall_CheckedChanged(sender As Object, e As System.EventArgs) Handles CHBshowall.CheckedChanged
        BindDati()
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