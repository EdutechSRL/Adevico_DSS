Imports COL_BusinessLogic_v2.UCServices
Imports lm.Comol.Modules.EduPath.BusinessLogic
Imports lm.Comol.Modules.EduPath.Domain
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.UI.Presentation
Imports lm.ActionDataContract
Imports System.Web.Services
Imports System.Web.Script.Services
Imports lm.Comol.Core.DomainModel

Public Class ViewActivity
    Inherits PageBaseEduPath

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
    Protected Sub UpdateAndCloseContainer()
        BindDati()
    End Sub


    Public Sub GetHiddenIdentifierValueEvent(ByRef value As String)
        value = Me.HDNdownloadTokenValue.Value
    End Sub

    Private Sub UpdateScormStat()
        Dim ModuleLinkIds As IList(Of Long) = ServiceEP.GetMaterialModuleLinkIds_ByActId(Me.CurrentActivityID)
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

    Protected ReadOnly Property CookieName() As String
        Get
            Return "COMOL_EPViewActivity"
        End Get
    End Property
    Protected ReadOnly Property DisplayMessageToken() As String
        Get
            Return Resource.getValue("DisplayMessageToken.ViewCertificate")
        End Get
    End Property
    Protected ReadOnly Property DisplayTitleToken() As String
        Get
            Return Resource.getValue("DisplayTitleToken.ViewCertificate")
        End Get
    End Property

#Region " Base"
    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_EduPath.Codex)

        Me.Master.ShowDocType = True
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.CTRLaddAction.InitializeControl(Me.Master.FindControl("SCMmanager"), Me.CurrentCommunityID, Me.CurrentPathId, Me.CurrentUnitId, Me.CurrentActivityID)
    End Sub

    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides Sub BindDati()

        If CurrentActivityID > 0 Then
            Me.PageUtility.AddAction(Me.CurrentCommunityID, Services_EduPath.ActionType.List, Me.PageUtility.CreateObjectsList(Services_EduPath.ObjectType.Activity, CurrentActivityID), InteractionType.UserWithLearningObject)

            Select Case ViewModeType
                Case EpViewModeType.Manage
                    InitPageManage()
                    If isManageable Then
                        LBErrorMSG.Visible = False
                    Else
                        Me.Resource.setLabel_To_Value(LBErrorMSG, "MSGstarted")
                        LBErrorMSG.Visible = True
                    End If

                Case EpViewModeType.View
                    If ServiceEP.isPlayModePath(CurrentPathId) Then
                        RedirectToUrl(RootObject.PathView(CurrentPathId, CurrentCommunityID, EpViewModeType.View, True))
                    End If
                    UpdateScormStat()
                    InitPageView()

                Case Else
                    Me.ShowError(EpError.Url)

            End Select
            setControls_ByEpType()
        Else
            RedirectToUrl(RootObject.EduPathList(CurrentPathId, EpViewModeType.View))
        End If
    End Sub
    Private Sub setControls_ByEpType()
        If isAutoEp Then
            hideControl(LKBupdateWeight)
        End If
    End Sub
    Public Overrides Sub BindNoPermessi()
        Me.ShowError(EpError.NotPermission)
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        'verifico che l'attività esista e l'utente abbia almeno il permesso di lettura
        If ViewModeType = EpViewModeType.View Then
            Return ServiceEP.HasPermessi_ByItem(Of Activity)(Me.CurrentActivityID, Me.CurrentCommunityID) AndAlso ServiceEP.ItemIsVisible(CurrentActivityID, ItemType.Activity) AndAlso ServiceEP.GetUserPermission_ByActivity(CurrentActivityID, CurrentUserId, Me.PageUtility.CurrentRoleID).Read
        ElseIf ViewModeType = EpViewModeType.Manage Then
            Return ServiceEP.HasPermessi_ByItem(Of Activity)(Me.CurrentActivityID, Me.CurrentCommunityID) AndAlso ServiceEP.GetUserPermission_ByActivity(CurrentActivityID, CurrentUserId, Me.PageUtility.CurrentRoleID).Read
        Else
            Return False
        End If
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ViewActivity", "EduPath")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLabel(Me.LBdescriptionTitle)
            .setLabel(Me.LBdateTitle)
            .setHyperLink(Me.HYPnextAct, False, True)
            .setHyperLink(Me.HYPpreviousAct, False, True)

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
    Private _hideSummary As Boolean = True
    Private _CanSwichSubAct As Boolean
    Private _SubActCount As Int16
    Private _isManageable As Int16 = -10
    Private _SmartTagsAvailable As SmartTags
    Public ReadOnly Property SmartTagsAvailable() As Comol.Entity.SmartTags
        Get
            If _SmartTagsAvailable Is Nothing Then
                _SmartTagsAvailable = ManagerConfiguration.GetSmartTags(Me.ApplicationUrlBase(True))
            End If
            Return _SmartTagsAvailable
        End Get
    End Property
    Protected ReadOnly Property isManageable As Boolean
        Get
            If _isManageable = -10 Then
                _isManageable = ((ViewModeType = EpViewModeType.Manage) AndAlso ServiceEP.isEditablePath(CurrentPathId, CurrentUserId))
            End If
            Return _isManageable
        End Get
    End Property
    Protected Overrides ReadOnly Property PathType As EPType
        Get
            If _PathType = lm.Comol.Modules.EduPath.Domain.EPType.None AndAlso CurrentPathId > 0 Then
                _PathType = ServiceEP.GetEpType(CurrentPathId, ItemType.Path)
            End If
            Return _PathType
        End Get
    End Property

    Public Property SubActCount As Int16
        Get
            Return _SubActCount
        End Get
        Set(ByVal value As Int16)
            _SubActCount = value
        End Set
    End Property

    Public Property CanSwichSubAct As Boolean
        Get
            Return _CanSwichSubAct
        End Get
        Set(ByVal value As Boolean)
            _CanSwichSubAct = value
        End Set
    End Property

    Public ReadOnly Property sortVisibility() As String
        Get
            If Me.CanSwichSubAct Then
                Return "true"
            End If
            Return "false"
        End Get
    End Property

    Private ReadOnly Property CurrentCommRoleID As Integer
        Get
            Return UtenteCorrente.GetIDRuoloForComunita(CurrentCommunityID)
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

    Private ReadOnly Property StatPageType As EpStatPage
        Get
            Dim qs_stat As String = Request.QueryString("St")
            If IsNumeric(qs_stat) Then
                If qs_stat = EpStatPage.UsersStat OrElse qs_stat = EpStatPage.UserStat Then

                    Return qs_stat

                End If

            End If
            Return EpStatPage.None
        End Get
    End Property

    Private ReadOnly Property UserIdToViewStat As Integer
        Get
            Dim qs_uId As String = Request.QueryString("UId")
            If IsNumeric(qs_uId) Then
                Return qs_uId
            End If
        End Get
    End Property

    Private ReadOnly Property CurrentUnitId() As Long
        Get
            Dim qs_unitId As String = Me.Request.QueryString("UId")
            If IsNumeric(qs_unitId) Then
                Return qs_unitId
            Else
                Return ServiceEP.GetUnitId_ByActivityId(CurrentActivityID)
            End If
        End Get
    End Property
    Private ReadOnly Property CurrentPathId() As Long
        Get
            Dim qs_pathId As String = Me.Request.QueryString("PId")
            If IsNumeric(qs_pathId) Then
                Return qs_pathId
            ElseIf CurrentActivityID > 0 Then
                Return ServiceEP.GetPathId_ByActivityId(CurrentActivityID)
            Else
                Return 0
            End If
        End Get
    End Property

    Private ReadOnly Property CurrentActivityID() As Long
        Get
            Dim qs_activityId As String = Me.Request.QueryString("AId")
            If IsNumeric(qs_activityId) Then
                Return qs_activityId
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
#End Region

    Private Sub SetImgStatusMedium(ByRef oImg As System.Web.UI.WebControls.Image, ByRef status As Status)
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

    Private Sub SetImgStatusSmall(ByRef oImg As System.Web.UI.WebControls.Image, ByRef status As Status)
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

    Private Sub SetDateLabel(ByVal StartDate As Date?, ByVal EndDate As Date?)

        If Not StartDate Is Nothing Then
            If Not EndDate Is Nothing Then
                Me.LBdate.Text = (String.Format(Me.Resource.getValue("Between"), StartDate, EndDate))
            Else
                Me.LBdate.Text = Me.Resource.getValue("After") & StartDate
            End If
            _hideSummary = False
        ElseIf Not EndDate Is Nothing Then
            _hideSummary = False
            Me.LBdate.Text = Me.Resource.getValue("Before") & EndDate
        Else
            hideControl(DIVdate)
        End If
    End Sub
    Private Sub ShowError(ByVal ErrorType As EpError)
        Me.Resource.setHyperLink(Me.HYPerror, False, True)
        Me.HYPerror.NavigateUrl = Me.BaseUrl & RootObject.PathView(Me.CurrentPathId, Me.CurrentCommunityID, Me.ViewModeType, ServiceEP.isPlayModePath(CurrentPathId))
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
        End Select
        'Me.MLVviewActivity.ActiveViewIndex = 1
    End Sub
    Private Sub InitDialog()
        'Me.DLGvisibility.DialogTitle = Me.Resource.getValue("DLGvisibilityTitle")
        'Me.DLGvisibility.DialogText = Me.Resource.getValue("DLGvisibilityText")
        'Dim Options As New List(Of String)
        'Options.Add(Me.Resource.getValue("DLGvisbUpdateStat"))
        'Options.Add(Me.Resource.getValue("DLGvisibPersonalizeAss"))
        'Me.DLGvisibility.DataSource = Options
        'Me.DLGvisibility.DataBind()

        'Options = New List(Of String)
        'Me.DLGmandatory.DialogTitle = Me.Resource.getValue("DLGmandatoryTitle")
        'Me.DLGmandatory.DialogText = Me.Resource.getValue("DLGmandatoryText")
        'Options.Add(Me.Resource.getValue("DLGmandUpdateStat"))
        'Options.Add(Me.Resource.getValue("DLGamandPersonlizeAss"))
        'Me.DLGmandatory.DataSource = Options
        'Me.DLGmandatory.DataBind()
    End Sub

    Private Sub InitPageView()

        If ServiceStat.InitActBrowsed(CurrentActivityID, Me.CurrentUserId, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then
            Dim dtoActivity As dtoActivity
            dtoActivity = ServiceEP.GetActivityStructure_View(CurrentActivityID, Me.CurrentContext.UserContext.CurrentUserID, Me.CurrentCommRoleID, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress, DateTime.Now)
            If dtoActivity Is Nothing OrElse Not ServiceEP.CanViewAct_byDate(dtoActivity.StartDate, dtoActivity.EndDate) Then
                Me.ShowError(EpError.NotPermission)
            Else
                Me.CanSwichSubAct = False
                If ServiceEP.CheckStatus(dtoActivity.Status, Status.Text) Then
                    InitNote(dtoActivity.Description)
                Else
                    InitActivityDetail(dtoActivity)
                End If
                InitButton()
                InitNavigationButton()
            End If
        Else
            Me.ShowError(EpError.Generic)
        End If
    End Sub

    Private Sub InitPageManage()
        Dim dtoActivity As dtoActivity
        dtoActivity = ServiceEP.GetActivityStructure_Manage(CurrentActivityID, Me.CurrentContext.UserContext.CurrentUserID, Me.CurrentCommRoleID, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress)
        If dtoActivity Is Nothing Then
            Me.ShowError(EpError.Generic)
        Else
            SubActCount = CType(dtoActivity.SubActivities.Count, Int16)
            Me.CanSwichSubAct = SubActCount > 1 AndAlso ServiceEP.CheckRoleEp(dtoActivity.RoleEP, RoleEP.Manager)
            Me.InitDialog()
            InitButton()
            InitActivityDetail(dtoActivity)
        End If
    End Sub

    Private Sub InitNavigationButton()
        Dim dtoNextActivity As dtoNavigationActivity = ServiceEP.GetdtoNextActivity(Me.CurrentActivityID, Me.CurrentUserId, Me.CurrentCommRoleID)
        If Not dtoNextActivity Is Nothing Then
            Me.HYPnextAct.Visible = True
            Me.HYPnextAct.NavigateUrl = Me.BaseUrl & RootObject.ViewActivity(dtoNextActivity.ActivityId, dtoNextActivity.ParentUnitId, Me.CurrentPathId, Me.CurrentCommunityID, Me.ViewModeType)
        End If

        Dim dtoPreviousActivity As dtoNavigationActivity = ServiceEP.GetdtoPreviousActivity(Me.CurrentActivityID, Me.CurrentUserId, Me.CurrentCommRoleID)
        If Not dtoPreviousActivity Is Nothing Then
            Me.HYPpreviousAct.Visible = True
            Me.HYPpreviousAct.NavigateUrl = Me.BaseUrl & RootObject.ViewActivity(dtoPreviousActivity.ActivityId, dtoPreviousActivity.ParentUnitId, Me.CurrentPathId, Me.CurrentCommunityID, Me.ViewModeType)
        End If
    End Sub

    Private Sub InitNote(ByRef text As String)
        Me.Master.ServiceTitle = Me.Resource.getValue("Note")
        hideControl(DIVsummaryBlock)
        Me.LBtext.Text = text
    End Sub

    Public Sub InitButton()
        Me.Resource.setLinkButton(LKBeduPathView, False, False)
        LKBeduPathView.PostBackUrl = Me.BaseUrl & RootObject.PathView(Me.CurrentPathId, Me.CurrentCommunityID, Me.ViewModeType, ServiceEP.isPlayModePath(CurrentPathId))

    End Sub

    Private Sub InitActivityDetail(ByRef dtoActivity As dtoActivity)
        hideControl(LBtext)
        If dtoActivity.PermissionEP.Create AndAlso isManageable AndAlso Not dtoActivity.isQuiz Then
            Me.Resource.setLinkButton(Me.LNBnewSubAct, False, False)
            Me.LNBnewSubAct.Visible = True
        End If

        If dtoActivity.PermissionEP.Delete AndAlso isManageable AndAlso Not dtoActivity.isQuiz Then
            Me.Resource.setLinkButton(LKBdeleteAct, False, True)
            Me.LKBdeleteAct.Visible = True
        End If
        If dtoActivity.PermissionEP.Update AndAlso isManageable Then
            Me.Resource.setHyperLink(Me.HYPupdateAct, False, True)
            Me.HYPupdateAct.Visible = False
            Me.HYPupdateAct.NavigateUrl = Me.BaseUrl & RootObject.UpdateActivity(Me.CurrentCommunityID, Me.CurrentActivityID, Me.CurrentUnitId)
            Me.Resource.setLinkButton(LKBupdateWeight, False, True)
            Me.LKBupdateWeight.Visible = True
        End If

        Me.Master.ServiceTitle = dtoActivity.Name

        If dtoActivity.Description <> "" Then
            Me.LBdescription.Text = SmartTagsAvailable.TagAll(dtoActivity.Description)
            _hideSummary = False
        Else
            hideControl(DIVdescription)
        End If

        Me.SetDateLabel(dtoActivity.StartDate, dtoActivity.EndDate)

        If ServiceEP.CheckStatus(dtoActivity.Status, Status.Mandatory) Then
            Me.IMGactMandatory.ImageUrl = RootObject.ImgMandatoryMedium(Me.BaseUrl)
            Me.IMGactMandatory.ToolTip = Me.Resource.getValue("ActivityMandatory")
            Me.IMGactMandatory.Visible = True
            _hideSummary = False
        Else
            hideControl(DIVmandatory)

        End If

        If True Then
            hideControl(DIVsummaryBlock)
        End If


        If dtoActivity.SubActivities.Count > 0 Then
            Me.RPsubActivity.DataSource = dtoActivity.SubActivities
            Me.RPsubActivity.DataBind()
        Else
            Me.LBnoSubAct.Visible = True
            If (ViewModeType = EpViewModeType.Manage) Then
                Me.Resource.setLabel_To_Value(Me.LBnoSubAct, "LBnoSubAct.Manage")
            Else
                Me.Resource.setLabel_To_Value(Me.LBnoSubAct, "LBnoSubAct.View")
            End If
        End If


    End Sub
    Private Sub RPsubActivity_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPsubActivity.ItemCommand
        CTRLmessages.Visible = False
        Try
            Select Case e.CommandName

                Case "delete"
                    If ServiceEP.VirtualDeleteSubActivity(e.CommandArgument, Me.CurrentCommunityID, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then
                        Me.PageUtility.RedirectToUrl(RootObject.ViewActivity(Me.CurrentActivityID, Me.CurrentUnitId, Me.CurrentPathId, Me.CurrentCommunityID, Me.ViewModeType))
                    Else
                        ShowError(EpError.Generic)
                    End If

                Case "moveUp"
                    If ServiceEP.MoveSubActivityDisplayOrderBefore(e.CommandArgument, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then
                        Me.PageUtility.RedirectToUrl(RootObject.ViewActivity(Me.CurrentActivityID, Me.CurrentUnitId, Me.CurrentPathId, Me.CurrentCommunityID, Me.ViewModeType))
                    Else
                        ShowError(EpError.Generic)
                    End If

                Case "moveDown"
                    If ServiceEP.MoveSubActivityDisplayOrderAfter(e.CommandArgument, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then
                        Me.PageUtility.RedirectToUrl(RootObject.ViewActivity(Me.CurrentActivityID, Me.CurrentUnitId, Me.CurrentPathId, Me.CurrentCommunityID, Me.ViewModeType))
                    Else
                        ShowError(EpError.Generic)
                    End If
                Case "mandatory"
                    If ServiceEP.UpdateSubActivityMandatoryStatus(e.CommandArgument, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then
                        Me.PageUtility.RedirectToUrl(RootObject.ViewActivity(Me.CurrentActivityID, Me.CurrentUnitId, Me.CurrentPathId, Me.CurrentCommunityID, Me.ViewModeType))
                    Else
                        ShowError(EpError.Generic)
                    End If
                Case "visibility"
                    If ServiceEP.UpdateSubActivityVisibilityStatus(e.CommandArgument, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then
                        Me.PageUtility.RedirectToUrl(RootObject.ViewActivity(Me.CurrentActivityID, Me.CurrentUnitId, Me.CurrentPathId, Me.CurrentCommunityID, Me.ViewModeType))
                    Else
                        ShowError(EpError.Generic)
                    End If
                Case "SetPermission"
                    Session("qsIsAdmin_" & e.CommandArgument.ToString.Split(",")(0)) = True
                    Response.Redirect(e.CommandArgument.ToString.Split(",")(1))

                Case "executed"
                    If ServiceEP.ExecuteSubActivityInternal(e.CommandArgument, Me.CurrentUserId, Me.CurrentCommRoleID, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then
                        Me.PageUtility.RedirectToUrl(RootObject.ViewActivity(Me.CurrentActivityID, Me.CurrentUnitId, Me.CurrentPathId, Me.CurrentCommunityID, Me.ViewModeType))
                    Else
                        ShowError(EpError.Generic)
                    End If
            End Select
        Catch ex As Exception
            ShowError(EpError.Generic)
        End Try
    End Sub

    Private Sub RPsubActivity_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPsubActivity.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim oLb As Label
            Dim oLkb As LinkButton
            Dim oDlkb As LinkButton 'MyUC.DialogLinkButton
            Dim oHyp As HyperLink
            Dim oImg As System.Web.UI.WebControls.Image
            Dim dtoItem As dtoSubActivity = e.Item.DataItem
            Dim oDisplayAction As lm.Comol.Core.ModuleLinks.IExternalModuleDisplayAction = CType(LoadControl(BaseUrl & Common.CoreRootObject.GenericNewDisplayActionControl), lm.Comol.Core.ModuleLinks.IExternalModuleDisplayAction)
            Dim isPlayMode As Boolean = True
            Select Case Me.ViewModeType
                Case EpViewModeType.Manage
                    If Me.CanSwichSubAct Then
                        oLkb = e.Item.FindControl("LKBup")
                        Me.Resource.setLinkButton(oLkb, False, True)
                        oLkb.CommandArgument = dtoItem.Id
                        oLkb.Text = "<img src=" & """" & "{0}" & """" & " alt=" & """" & "{1}" & """" & "/>"
                        oLkb.Visible = True
                        oLkb.Text = String.Format(oLkb.Text, RootObject.ImgUpActive(Me.BaseUrl), oLkb.ToolTip)

                        oLkb = e.Item.FindControl("LKBdown")
                        Me.Resource.setLinkButton(oLkb, False, True)
                        oLkb.CommandArgument = dtoItem.Id
                        oLkb.Text = "<img src=" & """" & "{0}" & """" & " alt=" & """" & "{1}" & """" & "/>"
                        oLkb.Visible = True
                        oLkb.Text = String.Format(oLkb.Text, RootObject.ImgDownActive(Me.BaseUrl), oLkb.ToolTip)
                        DirectCast(e.Item.FindControl("LITmove"), Literal).Text = "<span class='ui-icon ui-icon-arrowthick-2-n-s'>Move</span>"
                    End If

                    If dtoItem.PermissionEP.Update Then
                        If isManageable Then

                            Dim oTlkTxt As Telerik.Web.UI.RadNumericTextBox

                            If isTimeEp Then
                                hideControl(e.Item.FindControl("LBpoints"))
                                hideControl(e.Item.FindControl("TXBweight"))
                                'oTlkTxt = e.Item.FindControl("TXBhours")
                                'oTlkTxt.Text = ServiceEP.GetHour(dtoItem.Weight)

                                'oTlkTxt = e.Item.FindControl("TXBmins")
                                'oTlkTxt.Text = ServiceEP.GetMin(dtoItem.Weight)
                            Else
                                '  hideControl(e.Item.FindControl("DIVtime"))
                                oLb = e.Item.FindControl("LBpoints")
                                oLb.Text = Resource.getValue("Points")
                                Dim oTxb As New TextBox
                                oTxb = e.Item.FindControl("TXBweight")
                                oTxb.Text = dtoItem.Weight
                            End If

                            If Not isAutoEp Then
                                oDlkb = e.Item.FindControl("LKBsetMandatory")
                                oDlkb.CommandArgument = dtoItem.Id
                                'oDlkb.DialogClass = "mandatoryDial"
                                oDlkb.Visible = True
                                Me.Resource.setLinkButton(oDlkb, False, True)
                                oDlkb.Text = "<img src=" & """" & "{0}" & """" & " alt=" & """" & "{1}" & """" & "/>"
                                If ServiceEP.CheckStatus(dtoItem.Status, Status.Mandatory) Then
                                    oDlkb.Text = String.Format(oDlkb.Text, RootObject.ImgRemoveMandatoryMedium(Me.BaseUrl), oDlkb.ToolTip)
                                Else
                                    oDlkb.Text = String.Format(oDlkb.Text, RootObject.ImgSetMandatoryMedium(Me.BaseUrl), oDlkb.ToolTip)
                                End If
                            End If
                            Dim weight As String
                            If isMarkEp Then
                                weight = " (" & dtoItem.Weight & Resource.getValue("Points")
                            Else
                                'hideControl(oLb)  ' oLb.Text = ServiceEP.GetTime(dtoItem.Weight)
                                weight = ""
                            End If
                            If ServiceEP.IsSubActivityModificable(dtoItem.ContentType) Then
                                If ServiceEP.isSubActityInternalModule(dtoItem.ContentType) Then
                                    oHyp = e.Item.FindControl("HYPeditSubAct")
                                    oHyp.Visible = isManageable
                                    Select Case dtoItem.ContentType
                                        Case SubActivityType.Text
                                            oHyp.NavigateUrl = Me.BaseUrl & RootObject.UpdateSubActText(dtoItem.Id, Me.CurrentActivityID, Me.CurrentCommunityID)
                                        Case SubActivityType.Certificate
                                            oHyp.NavigateUrl = Me.BaseUrl & RootObject.UpdateSubActCertification(Me.CurrentPathId, Me.CurrentUnitId, Me.CurrentActivityID, dtoItem.Id, Me.CurrentCommunityID)

                                            Dim oCertAction As lm.Comol.Modules.EduPath.Presentation.IViewModuleCertificationAction = e.Item.FindControl("CTRLcertificationAction")
                                            'oCertAction.RefreshContainer = True
                                            Dim initializer As New dtoInternalActionInitializer
                                            initializer.IdPath = CurrentPathId
                                            initializer.IdPathCommunity = Me.CurrentCommunityID
                                            initializer.CookieName = "PathCertifications_" & initializer.IdPath
                                            initializer.SubActivity = dtoItem
                                            ' AGGIUNTA PLACEHOLDER
                                            ' --> 
                                            initializer.ContainerCSS = SubActivityCssClass(dtoItem)
                                            ' DIMENSIONI IMMAGINI
                                            oCertAction.IconSize = Helpers.IconSize.Small
                                            oCertAction.EnableAnchor = True
                                            initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.adminMode

                                            'AddHandler oDisplayAction.RefreshContainerEvent, AddressOf RefreshContainerEvent
                                            oCertAction.InitializeControl(initializer)
                                            Dim control As UC_CertificationAction = e.Item.FindControl("CTRLcertificationAction")
                                            control.Visible = True
                                            control.EvaluablePath = Not isTimeEp()
                                    End Select
                                Else
                                    oLkb = e.Item.FindControl("LKBeditSubAct")
                                    Me.Resource.setLinkButton(oLkb, False, True)
                                    oLkb.Visible = isManageable
                                    'oLkb.Text = "<img src=" & """" & "{0}" & """" & " alt=" & """" & "{1}" & """" & "/>"

                                    Dim initializer As New lm.Comol.Core.ModuleLinks.dtoExternalModuleInitializer
                                    oDisplayAction.ContainerCSS = SubActivityCssClass(dtoItem)
                                    oDisplayAction.IconSize = Helpers.IconSize.Small

                                    oDisplayAction.EnableAnchor = True
                                    Select Case dtoItem.ContentType
                                        Case SubActivityType.Quiz
                                            oDisplayAction.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.textDefault
                                        Case Else
                                            oDisplayAction.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.text
                                    End Select

                                    initializer.Link = dtoItem.ModuleLink


                                    Dim oList As List(Of dtoModuleActionControl) = oDisplayAction.InitializeRemoteControl(initializer, StandardActionType.Edit)
                                    Dim EditControl As dtoModuleActionControl = (From c In oList Where c.ControlType = StandardActionType.Edit AndAlso c.isEnabled Select c).FirstOrDefault
                                    If Not EditControl Is Nothing Then
                                        oLkb.CommandArgument = dtoItem.ModuleLink.DestinationItem.ObjectLongID & "," & EditControl.LinkUrl
                                    End If
                                End If
                            Else

                                Dim initializer As New lm.Comol.Core.ModuleLinks.dtoExternalModuleInitializer
                                oDisplayAction.ContainerCSS = SubActivityCssClass(dtoItem)
                                oDisplayAction.IconSize = Helpers.IconSize.Small

                                oDisplayAction.EnableAnchor = True
                                oDisplayAction.Display = If(dtoItem.ContentType = SubActivityType.File, lm.Comol.Core.ModuleLinks.DisplayActionMode.adminMode, lm.Comol.Core.ModuleLinks.DisplayActionMode.text)
                                Select Case dtoItem.ContentType
                                    Case SubActivityType.Quiz
                                        oDisplayAction.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.textDefault
                                    
                                End Select
                                initializer.Link = dtoItem.ModuleLink

                                oDisplayAction.InitializeRemoteControl(initializer, StandardActionType.Play)
                            End If
                        Else
                            hideControl(e.Item.FindControl("TXBweight"))
                            oLb = e.Item.FindControl("LBpoints")
                            If isTimeEp Then
                                If isAutoEp Then
                                    hideControl(e.Item.FindControl("DIVweight"))
                                Else
                                    oLb.Text = ServiceEP.GetTime(dtoItem.Weight)
                                End If
                            Else
                                oLb.Text = dtoItem.Weight & Resource.getValue("Points")
                            End If
                        End If
                        oDlkb = e.Item.FindControl("LKBvisibSubAct")
                        Me.Resource.setLinkButton(oDlkb, False, True)
                        oDlkb.Visible = True
                        oDlkb.CommandArgument = dtoItem.Id
                        oDlkb.Text = "<img src=" & """" & "{0}" & """" & " alt=" & """" & "{1}" & """" & "/>"
                        If ServiceEP.CheckStatus(dtoItem.Status, Status.Locked) Then
                            oDlkb.Text = String.Format(oDlkb.Text, RootObject.ImgBtnBlocked_Off(Me.BaseUrl), oDlkb.ToolTip)
                        Else
                            oDlkb.Text = String.Format(oDlkb.Text, RootObject.ImgBtnBlocked_On(Me.BaseUrl), oDlkb.ToolTip)
                        End If
                    ElseIf ServiceEP.CheckStatus(dtoItem.Status, Status.Mandatory) Then
                        oImg = e.Item.FindControl("IMGmandatory")
                        oImg.ImageUrl = RootObject.ImgMandatoryMedium(Me.BaseUrl)
                        oImg.ToolTip = Me.Resource.getValue("SubActivityMandatory")
                        oImg.Visible = True
                    End If
                    If isManageable Then
                        oLkb = e.Item.FindControl("LKBdeleteSubAct")
                        If dtoItem.PermissionEP.Delete Then
                            Me.Resource.setLinkButton(oLkb, False, True)
                            oLkb.CommandArgument = dtoItem.Id
                            oLkb.Visible = True
                        End If
                    Else
                        Select Case dtoItem.ContentType
                            Case SubActivityType.Text
                                Exit Select
                            Case SubActivityType.Certificate
                                Dim control As UC_CertificationAction = e.Item.FindControl("CTRLcertificationAction")
                                control.Visible = True
                                control.EvaluablePath = Not isTimeEp()

                                Dim oCertAction As lm.Comol.Modules.EduPath.Presentation.IViewModuleCertificationAction = e.Item.FindControl("CTRLcertificationAction")
                                'oCertAction.RefreshContainer = True
                                Dim initializer As New dtoInternalActionInitializer
                                initializer.IdPath = CurrentPathId
                                initializer.IdPathCommunity = Me.CurrentCommunityID
                                initializer.CookieName = "PathCertifications_" & initializer.IdPath
                                initializer.SubActivity = dtoItem
                                ' AGGIUNTA PLACEHOLDER
                                ' --> 
                                initializer.ContainerCSS = SubActivityCssClass(dtoItem)
                                ' DIMENSIONI IMMAGINI
                                oCertAction.IconSize = Helpers.IconSize.Small
                                oCertAction.EnableAnchor = True
                                initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.adminMode

                                'AddHandler oDisplayAction.RefreshContainerEvent, AddressOf RefreshContainerEvent
                                oCertAction.InitializeControl(initializer)
                            Case Else
                                Dim initializer As New lm.Comol.Core.ModuleLinks.dtoExternalModuleInitializer
                                oDisplayAction.ContainerCSS = SubActivityCssClass(dtoItem)
                                oDisplayAction.IconSize = Helpers.IconSize.Small

                                oDisplayAction.EnableAnchor = True

                                Select Case dtoItem.ContentType
                                    Case SubActivityType.Quiz
                                        oDisplayAction.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.textDefault
                                    Case Else
                                        oDisplayAction.Display = If(dtoItem.ContentType = SubActivityType.File, lm.Comol.Core.ModuleLinks.DisplayActionMode.adminMode, lm.Comol.Core.ModuleLinks.DisplayActionMode.text)
                                End Select
                                initializer.Link = dtoItem.ModuleLink

                                oDisplayAction.InitializeRemoteControl(initializer, StandardActionType.Play)
                        End Select
                    End If

                Case EpViewModeType.View
                    oLb = e.Item.FindControl("LBcompletionTitle")
                    oLb.Visible = True
                    Me.Resource.setLabel(oLb)
                    oLb = e.Item.FindControl("LBcompletion")
                    oLb.Text = dtoItem.Completion
                    oLb.Visible = True

                    oLb = e.Item.FindControl("LBstatusTitle")
                    oLb.Visible = True
                    oLb.Text = Me.Resource.getValue("StatusStatistic." & dtoItem.StatusStat.ToString)

                    SetImgStatusSmall(e.Item.FindControl("IMGstatus"), dtoItem.StatusStat)


                    oImg = e.Item.FindControl("IMGmandatory")
                    If ServiceEP.CheckStatus(dtoItem.Status, Status.Mandatory) Then
                        oImg.ImageUrl = RootObject.ImgMandatoryMedium(Me.BaseUrl)
                        oImg.ToolTip = Me.Resource.getValue("SubActivityMandatory")
                        oImg.Visible = True
                    End If

                    isPlayMode = ServiceEP.CheckStatus(dtoItem.Status, Status.NotLocked)

                    If isPlayMode AndAlso (dtoItem.ContentType = SubActivityType.Text OrElse dtoItem.ContentType = SubActivityType.Certificate) Then
                        Select Case dtoItem.ContentType
                            Case SubActivityType.Text
                                oLkb = e.Item.FindControl("LKBexecute")
                                Me.Resource.setLinkButton(oLkb, False, True)
                                oLkb.CommandArgument = dtoItem.Id
                                oLkb.Visible = True
                            Case SubActivityType.Certificate
                                Dim initializer As New dtoInternalActionInitializer
                                initializer.IdPath = CurrentPathId
                                initializer.IdPathCommunity = Me.CurrentCommunityID
                                initializer.CookieName = "PathCertifications_" & initializer.IdPath
                                initializer.SubActivity = dtoItem

                                Dim weight As String
                                If isMarkEp Then
                                    weight = " (" & dtoItem.Weight & Resource.getValue("Points")
                                Else
                                    weight = ServiceEP.GetTime(dtoItem.Weight)
                                End If

                                initializer.PlaceHolders.Add(New lm.Comol.Core.ModuleLinks.dtoPlaceHolder() With {.Text = weight, .Type = lm.Comol.Core.ModuleLinks.PlaceHolderType.three, .CssClass = "duration"})
                                ' DEFINISCO UNA CLASSE PER IL CONTAINER
                                initializer.ContainerCSS = SubActivityCssClass(dtoItem)
                                If isPlayMode Then
                                    initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction
                                Else
                                    initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.text
                                End If
                                Dim control As UC_CertificationAction = e.Item.FindControl("CTRLcertificationAction")
                                control.Visible = True
                                control.EvaluablePath = Not isTimeEp()
                                Dim oCertAction As lm.Comol.Modules.EduPath.Presentation.IViewModuleCertificationAction = e.Item.FindControl("CTRLcertificationAction")
                                'oCertAction.RefreshContainer = True
                                ' DIMENSIONI IMMAGINI
                                oCertAction.IconSize = Helpers.IconSize.Small
                                oCertAction.EnableAnchor = True
                                oCertAction.InitializeControl(initializer)
                        End Select
                    End If

                    oImg = e.Item.FindControl("IMGvisibility")
                    oImg.Visible = True
                    oImg.ImageUrl = RootObject.ImgItemBlocked(Me.BaseUrl, isPlayMode)
                    oImg.AlternateText = Me.Resource.getValue("Blocked." & isPlayMode)



                    If isTimeEp Then
                        hideControl(e.Item.FindControl("LBpoints"))
                        hideControl(e.Item.FindControl("TXBweight"))
                    Else
                        oLb = e.Item.FindControl("LBpoints")
                        oLb.Text = Resource.getValue("Points")
                        Dim oTxb As New TextBox
                        oTxb = e.Item.FindControl("TXBweight")
                        oTxb.Text = dtoItem.Weight
                    End If
            End Select

            oImg = e.Item.FindControl("IMGtype")
            oImg.ImageUrl = RootObject.ImgContentType(Me.BaseUrl, dtoItem.ContentType)

            oLb = e.Item.FindControl("LBtype")
            oLb.Text = Me.Resource.getValue("SubActivityType." & dtoItem.ContentType.ToString)
            ' oLb.Text = IIf(isAutoEp, Me.Resource.getValue("SubActivityType." & dtoItem.ContentType.ToString), Me.Resource.getValue("SubActivityType." & dtoItem.ContentType.ToString) & " (")

            oLb = e.Item.FindControl("LBnameTitle")
            Me.Resource.setLabel(oLb)

            oLb = e.Item.FindControl("LBdetail")
            Me.Resource.setLabel(oLb)

            oHyp = e.Item.FindControl("HYPname")
            If Not String.IsNullOrEmpty(dtoItem.Link) Then
                oHyp.Text = dtoItem.Name
                oHyp.NavigateUrl = dtoItem.Link
            Else
                oHyp.Visible = False
            End If

            Dim oDiv As HtmlGenericControl = e.Item.FindControl("DVrenderDescription")
            oLb = e.Item.FindControl("LBdescription")
            If ServiceEP.isSubActityInternalModule(dtoItem.ContentType) Then
                Select Case dtoItem.ContentType
                    Case SubActivityType.Text
                        oLb.Text = Me.SmartTagsAvailable.TagAll(dtoItem.Description)
                    Case Else
                        oDiv.Visible = False
                End Select
            Else
                oLb.Text = Me.SmartTagsAvailable.TagAll(dtoItem.Description)
                Dim oPlaceHolder As PlaceHolder = e.Item.FindControl("PLHaction")
                DisplayAction(oPlaceHolder, oDisplayAction, dtoItem, isPlayMode)
            End If
        End If
    End Sub

    Private Sub DisplayAction(ByRef oPlaceHolder As PlaceHolder, ByRef oDisplayAction As lm.Comol.Core.ModuleLinks.IExternalModuleDisplayAction, ByRef dtoItem As dtoSubActivity, ByVal isPlayActive As Boolean)
        oPlaceHolder.Visible = True

        If Me.ViewModeType = EpViewModeType.View OrElse Me.ViewModeType = EpViewModeType.Stat Then
            Dim initializer As New lm.Comol.Core.ModuleLinks.dtoExternalModuleInitializer
            oDisplayAction.ContainerCSS = SubActivityCssClass(dtoItem)
            oDisplayAction.IconSize = Helpers.IconSize.Small

            oDisplayAction.EnableAnchor = True
            oDisplayAction.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.text
            initializer.Link = dtoItem.ModuleLink


            If isPlayActive Then
                'oDisplayAction.InitializeControlInlineByLink(dtoItem.ModuleLink)
                oDisplayAction.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction

            Else
                ' oDisplayAction.InitializeRemoteControlInlineByLink(True, dtoItem.ModuleLink)
                oDisplayAction.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.text
                oDisplayAction.InitializeControl(initializer)
            End If
            oDisplayAction.InitializeControl(initializer)
        ElseIf Me.ViewModeType = EpViewModeType.Manage Then

        End If
        oPlaceHolder.Controls.Add(oDisplayAction)
        ' OPPURE QUESTO SE VUOI RECUPERARE I PULSANTI:
        'Dim oList As List(Of dtoModuleActionControl) = oDisplayActionFile.InitializeRemoteControlByLink(False, dtoItem.ModuleLink)
        'Dim PlayControl As dtoModuleActionControl = (From c In oList Where c.ControlType = ModuleActionControlType.Play AndAlso c.Enabled Select c).FirstOrDefault
        'Dim EditControl As dtoModuleActionControl = (From c In oList Where c.ControlType = ModuleActionControlType.Edit AndAlso c.Enabled Select c).FirstOrDefault
        'Dim DeleteControl As dtoModuleActionControl = (From c In oList Where c.ControlType = ModuleActionControlType.Delete AndAlso c.Enabled Select c).FirstOrDefault


    End Sub

    Private Sub LKBdeleteAct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBdeleteAct.Click
        Me.ServiceEP.VirtualDeleteAllActivity(Me.CurrentActivityID, isAutoEp, Me.CurrentCommunityID, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress)
        Me.PageUtility.RedirectToUrl(RootObject.PathView(CurrentPathId, CurrentCommunityID, EpViewModeType.Manage, False))
    End Sub

#Region "ADD"
    Private Sub LNBnewSubAct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBnewSubAct.Click
        '  Me.DVaddSubActivity.Visible = True
        Me.CTRLaddAction.Visible = True
        Me.CTRLaddAction.SetActionvView(UC_AddAction.ActionView.SelectAction)
        'DVaddSubActivity.Attributes.Add("class", "view-modal")
        Me.LTscriptOpen.Visible = True

        'Dim script As String = String.Format("showDialog('{0}')", "addSubActivity")
        'ScriptManager.RegisterClientScriptBlock(MyBase.Page, Me.GetType, Me.UniqueID, script, True)
        'Me.UDPaddActivity.Update()
    End Sub
    'Private Sub CloseDialog(ByVal dialogId As String)
    '    Dim script As String = String.Format("closeDialog('{0}')", dialogId)
    '    ScriptManager.RegisterClientScriptBlock(MyBase.Page, Me.GetType, Me.UniqueID, script, True)
    'End Sub

    Private Sub CTRLaddAction_GetScriptManager(ByRef manager As System.Web.UI.ScriptManager) Handles CTRLaddAction.GetScriptManager
        manager = Me.Master.FindControl("SCMmanager")
    End Sub
    Protected Sub CTRLaddAction_UpdateAndCloseContainer(ByVal idSubactivity As Long) Handles CTRLaddAction.UpdateAndCloseContainer
        'DVaddSubActivity.Attributes.Remove("class")
        'DVaddSubActivity.Visible = False
        'Me.CTRLaddAction.Visible = False
        'Me.LTscriptOpen.Visible = False
        'Me.BindDati()+
        PageUtility.RedirectToUrl(RootObject.ViewActivity(idSubactivity, Me.CurrentActivityID, Me.CurrentUnitId, Me.CurrentPathId, Me.CurrentCommunityID, EpViewModeType.Manage))
    End Sub

    'Protected Sub CTRLaddAction_UpdateContainer() Handles CTRLaddAction.UpdateContainer
    '    'DVaddSubActivity.Attributes.Remove("class")
    '    'DVaddSubActivity.Visible = False

    '    Me.CTRLaddAction.Visible = False
    '    Me.LTscriptOpen.Visible = False
    '    Me.BindDati()
    'End Sub
#End Region

#Region "GESTIONE DIALOG ATTUALMENTE ELIMINATE... IN CASO DI MODIFICA GESTIVANO SE AGGIORNARE LE STATISTICHE O PERSONALIZZARE GLI ASSEGNAMENTI DI CHI HA GIA' INIZIATO IL PERCORSO"

    'Private Sub DLGmandatory_ButtonPressed(ByVal dialogResult As Integer, ByVal CommandArgument As String, ByVal CommandName As String) Handles DLGmandatory.ButtonPressed
    '    If dialogResult >= 0 Then
    '        Dim UpdateType As UpdateAssignemtOrStatistic = UpdateAssignemtOrStatistic.None
    '        If dialogResult = 0 Then
    '            UpdateType = UpdateAssignemtOrStatistic.Statistic
    '        ElseIf dialogResult = 1 Then
    '            UpdateType = UpdateAssignemtOrStatistic.Assignment
    '        End If
    '        If ServiceEP.UpdateSubActivityMandatoryOrVisibilityStatus(CommandArgument, UpdateType, UpdateStatusType.MandatoryStatus, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then
    '            Me.PageUtility.RedirectToUrl(RootObject.ViewActivity(Me.CurrentActivityID, Me.CurrentUnitId, Me.CurrentPathId, Me.CurrentCommunityID, Me.ViewModeType))
    '        Else
    '            ShowError(EpError.Generic)
    '        End If
    '    End If
    'End Sub

    'Private Sub DLGvisibility_ButtonPressed(ByVal dialogResult As Integer, ByVal CommandArgument As String, ByVal CommandName As String) Handles DLGvisibility.ButtonPressed
    '    If dialogResult >= 0 Then
    '        Dim UpdateType As UpdateAssignemtOrStatistic = UpdateAssignemtOrStatistic.None
    '        If dialogResult = 0 Then
    '            UpdateType = UpdateAssignemtOrStatistic.Statistic
    '        ElseIf dialogResult = 1 Then
    '            UpdateType = UpdateAssignemtOrStatistic.Assignment
    '        End If

    '        If ServiceEP.UpdateSubActivityMandatoryOrVisibilityStatus(CommandArgument, UpdateType, UpdateStatusType.VisibilityStatus, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then
    '            Me.PageUtility.RedirectToUrl(RootObject.ViewActivity(Me.CurrentActivityID, Me.CurrentUnitId, Me.CurrentPathId, Me.CurrentCommunityID, Me.ViewModeType))
    '        Else
    '            ShowError(EpError.Generic)
    '        End If
    '    End If
    'End Sub
#End Region

    Private Sub LKBupdateWeight_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBupdateWeight.Click
        'THOM
        Dim subActsweight As IList(Of dtoWeight) = New List(Of dtoWeight)
        For Each rpItem As RepeaterItem In RPsubActivity.Items
            Dim dto As New dtoWeight
            dto.Id = DirectCast(rpItem.FindControl("LKBdeleteSubAct"), LinkButton).CommandArgument
            'If isTimeEp Then DA ATTIVARESE SE ACTION HANNO TEMPO SPECIFICO
            '    Dim hour As Int16
            '    Dim min As Int16
            '    Dim oTlkTxt As Telerik.Web.UI.RadNumericTextBox

            '    oTlkTxt = rpItem.FindControl("TXBhours")
            '    hour = IIf(IsNumeric(oTlkTxt.Text), oTlkTxt.Text, 0)

            '    oTlkTxt = rpItem.FindControl("TXBmins")
            '    min = IIf(IsNumeric(oTlkTxt.Text), oTlkTxt.Text, 0)

            '    dto.Weight = ServiceEP.ConvertTime(hour, min)
            'Else
            Dim TXBweight As TextBox = rpItem.FindControl("TXBweight")
            dto.Weight = TXBweight.Text
            'End If
            subActsweight.Add(dto)
        Next
        If ServiceEP.UpdateSubActivityWeight(CurrentActivityID, subActsweight, isAutoEp, CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then
            BindDati()
        Else
            ShowError(EpError.Generic)
        End If

    End Sub

    Private Sub LKBeduPathView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBeduPathView.Click
        LKBupdateWeight_Click(sender, e)
    End Sub

    Protected Friend Function GetBlockUiMessage() As String
        Return Resource.getValue("GetFileBlockUiMessage")
    End Function

    Protected Overrides Sub NotifyModuleStatus(status As lm.Comol.Core.DomainModel.ModuleStatus)
        CTRLmoduleStatusMessage.Visible = True
        CTRLmoduleStatusMessage.InitializeControl(Resource.getValue("EduPath.ModuleStatus." & status.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub

    Protected Overrides Sub NotifyUnavailableModule(status As lm.Comol.Core.DomainModel.ModuleStatus)
        Dim dtoActivity As dtoActivity = ServiceEP.GetActivityStructure_View(CurrentActivityID, Me.CurrentContext.UserContext.CurrentUserID, Me.CurrentCommRoleID, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress, DateTime.Now)
        If Not IsNothing(dtoActivity) Then
            If ServiceEP.CheckStatus(dtoActivity.Status, lm.Comol.Modules.EduPath.Domain.Status.Text) Then
                Master.ServiceTitle = Me.Resource.getValue("Note")
            Else
                Master.ServiceTitle = dtoActivity.Name
            End If
        End If
        MLVviewActivity.SetActiveView(VIWmessages)
        CTRLgenericMessages.InitializeControl(Resource.getValue("EduPath.ModuleStatus." & status.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
    End Sub

    Protected Overrides ReadOnly Property CheckModuleStatus As Boolean
        Get
            Return True
        End Get
    End Property
End Class