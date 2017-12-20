Imports lm.Comol.Modules.EduPath.Domain
Imports lm.Comol.Modules.EduPath.BusinessLogic
Imports lm.Comol.UI.Presentation
Imports COL_BusinessLogic_v2.UCServices
Imports lm.ActionDataContract
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel

Public Class UserListStatistics
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

    Private _buttonClicked As ButtonClicked = ButtonClicked.EduPathView

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
    Private ReadOnly Property Filter As EvaluationFilter
        Get
            Dim qs_filter As String = Request.QueryString("Filter")
            If IsNumeric(qs_filter) AndAlso qs_filter > -1 AndAlso qs_filter < 2 Then
                Return qs_filter
            Else
                Return 1
            End If
        End Get
    End Property

    Protected Overrides ReadOnly Property PathType As EPType
        Get
            If _PathType = lm.Comol.Modules.EduPath.Domain.EPType.None Then
                _PathType = ServiceEP.GetEpType(ItemId, ItemType)
            End If
            Return _PathType
        End Get
    End Property
    Protected ReadOnly Property CookieName() As String
        Get
            Return "COMOL_UsersStatistics"
        End Get
    End Property
    Protected ReadOnly Property DisplayMessageToken() As String
        Get
            Return Resource.getValue("DisplayMessageToken.UsersStatistics")
        End Get
    End Property
    Protected ReadOnly Property DisplayTitleToken() As String
        Get
            Return Resource.getValue("DisplayTitleToken.UsersStatistics")
        End Get
    End Property

    Public Property Pager() As lm.Comol.Core.DomainModel.PagerBase
        Get
            If TypeOf Me.ViewState("Pager") Is lm.Comol.Core.DomainModel.PagerBase Then
                Return Me.ViewState("Pager")
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As lm.Comol.Core.DomainModel.PagerBase)
            Me.ViewState("Pager") = value
            Me.PGgrid.Pager = value

        End Set
    End Property

    Private ReadOnly Property PageSize As Int16
        Get
            Return 25
        End Get
    End Property

    Private ReadOnly Property PageIndex As Int16
        Get
            Dim qs_index As String = Request.QueryString("Page")
            If IsNumeric(qs_index) Then
                Return qs_index
            Else
                Return 0
            End If
        End Get

    End Property

    Private ReadOnly Property SessionUniqueKey() As String
        Get
            If Me.ViewState("SessionUniqueKey") Is Nothing Then
                Me.ViewState("SessionUniqueKey") = Me.ItemId & "_" & ItemType.ToString & "_"
            End If
            Return Me.ViewState("SessionUniqueKey")
        End Get

    End Property

    Private Property ItemsIdToEval As List(Of Long)
        Get
            Return Me.Session("ItemsIdToEval_" & SessionUniqueKey)
        End Get
        Set(ByVal value As List(Of Long))
            Me.Session("ItemsIdToEval_" & SessionUniqueKey) = value
        End Set
    End Property

    Private Property EvaluatedItemsId As List(Of Long)
        Get
            Return Me.Session("EvaluatedItemsId_" & SessionUniqueKey)
        End Get
        Set(ByVal value As List(Of Long))
            Me.Session("EvaluatedItemsId_" & SessionUniqueKey) = value
        End Set
    End Property


    Private Property ListOfUpdatedVote As List(Of dtoUserStatToEvaluate)
        Get
            Return Me.Session("ListOfUpdatedVote_" & SessionUniqueKey)
        End Get
        Set(ByVal value As List(Of dtoUserStatToEvaluate))
            Me.Session("ListOfUpdatedVote_" & SessionUniqueKey) = value
        End Set
    End Property

    Private Property ListOfVote As List(Of dtoUserStatToEvaluate)
        Get
            Return Me.Session("ListOfVote_" & SessionUniqueKey)
        End Get
        Set(ByVal value As List(Of dtoUserStatToEvaluate))
            Me.Session("ListOfVote_" & SessionUniqueKey) = value
        End Set
    End Property

    Private Sub ReloadStat(ByVal dateToView As DateTime?) Handles CTRLselTime.DateSelected
        RedirectToUrl(RootObject.UsersStatistics(ItemId, CurrentCommunityID, Me.ItemType, PageIndex, dateToView, CHBshowall.Checked))
    End Sub

    Public Sub LoadCertifiedDate() Handles CTRLselTime.ViewCertifiedStat

        RedirectToUrl(RootObject.UsersStatistics(ItemId, CurrentCommunityID, Me.ItemType, PageIndex, EpEndDate, CHBshowall.Checked))

    End Sub

    Private Function GetDetailUrl(ByRef userId As Integer) As String

        Select Case ItemType
            Case lm.Comol.Modules.EduPath.Domain.ItemType.Path
                Return Me.BaseUrl & RootObject.UserStatisticsManage(ItemId, Me.CurrentCommunityID, userId, ItemType, PageIndex, TimeStat, CHBshowall.Checked)

            Case lm.Comol.Modules.EduPath.Domain.ItemType.Unit
                Return Me.BaseUrl & RootObject.UserStatisticsManage(ItemId, Me.CurrentCommunityID, userId, ItemType, PageIndex, TimeStat, CHBshowall.Checked)

            Case lm.Comol.Modules.EduPath.Domain.ItemType.Activity
                'Return Me.BaseUrl & RootObject.UserActivityStatManage_PrevUsersStat(GetPathId, ItemId, CurrentCommunityID, userId, PageIndex, TimeStat)
                Return ""
            Case Else
                Return ""
        End Select

    End Function

    Private ReadOnly Property IdCurrentPath As Long
        Get
            Dim idPath As Long = 0
            idPath = ViewStateOrDefault("IdPath", idPath)
            If idPath = 0 Then
                idPath = GetPathId()
                ViewState("IdPath") = idPath
            End If
            Return idPath
        End Get
    End Property

    Private Function GetPathId() As Long

        Select Case ItemType
            Case lm.Comol.Modules.EduPath.Domain.ItemType.Path
                Return ItemId

            Case lm.Comol.Modules.EduPath.Domain.ItemType.Unit
                Return Me.ServiceEP.GetPathId_ByUnitId(ItemId)

            Case lm.Comol.Modules.EduPath.Domain.ItemType.Activity
                Return ServiceEP.GetPathId_ByActivityId(ItemId)

            Case lm.Comol.Modules.EduPath.Domain.ItemType.SubActivity
                Return ServiceEP.GetPathId_BySubActivityId(ItemId)

            Case Else
                Return -1
        End Select
    End Function

    Private ReadOnly Property ItemType As ItemType
        Get
            Dim qs_itemType As String = Request.QueryString("It")
            If IsNumeric(qs_itemType) Then

                Select Case qs_itemType
                    Case lm.Comol.Modules.EduPath.Domain.ItemType.Path
                        Return lm.Comol.Modules.EduPath.Domain.ItemType.Path

                    Case lm.Comol.Modules.EduPath.Domain.ItemType.Unit
                        Return lm.Comol.Modules.EduPath.Domain.ItemType.Unit

                    Case lm.Comol.Modules.EduPath.Domain.ItemType.Activity
                        Return lm.Comol.Modules.EduPath.Domain.ItemType.Activity

                    Case lm.Comol.Modules.EduPath.Domain.ItemType.SubActivity
                        Return lm.Comol.Modules.EduPath.Domain.ItemType.SubActivity

                    Case Else
                        Return lm.Comol.Modules.EduPath.Domain.ItemType.None
                End Select
            Else
                Return ItemType.None
            End If
        End Get
    End Property



    Private ReadOnly Property ItemId() As Long
        Get
            If IsNumeric(Request.QueryString("ItemId")) Then
                Return Request.QueryString("ItemId")
            Else
                Return -1
            End If
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


    Private ReadOnly Property IsEvaluable As Boolean
        Get
            Return Request.QueryString("Eval") = "1"
        End Get
    End Property

    Private _endDateSet As Boolean = False
    Private _EpEndDate As DateTime?
    Private ReadOnly Property EpEndDate As DateTime?
        Get
            If Not _endDateSet Then
                _EpEndDate = ServiceEP.GetEpEndDate(GetPathId)
            End If
            Return _EpEndDate
        End Get
    End Property
#End Region

#Region " Base"
    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_EduPath.Codex)
        Me.Master.ShowDocType = True
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PGgrid.Pager = Me.Pager

    End Sub

    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides Sub BindDati()
        If Not Page.IsPostBack Then
            Me.CHBshowall.Checked = PreloadShowAll
        End If
        If (ItemType = lm.Comol.Modules.EduPath.Domain.ItemType.SubActivity) Then
            UpdateScormStat()

        End If

        If (ItemType <> lm.Comol.Modules.EduPath.Domain.ItemType.Path) Then
            LBcompletionTit.Text = Me.Resource.getValue("LBcompletionTit.percent.text")
            LNBexportPSfullCertificationToCsv.Visible = False
            LNBexportPSfullCertificationToXml.Visible = False
            LNBexportPSfullToCsv.Visible = False
            LNBexportPSfullToXml.Visible = False
        End If

        'SPANexportStatistics.InnerHtml = Me.Resource.getValue("ExportStatistics")
        SPANviewStatistics.InnerHtml = Me.Resource.getValue("ViewStatistics")

        'Da ATTIVARE IN CASO DI OVERTIME
        'Dim dtoDate As dtoEndTime = ServiceEP.GetEpTime(GetPathId(), CurrentUserId)

        ''   Dim isOverEndTime As Boolean = IIf(dtoDate.CertifiedTime Is Nothing, True, IIf(Not dtoDate.CertifiedTime Is Nothing AndAlso dtoDate.CertifiedTime < DateTime.Now, True, False))
        'Dim IsCertifiedTime As Boolean = IIf(dtoDate.CertifiedTime Is Nothing, False, IIf(Not dtoDate.CertifiedTime Is Nothing AndAlso dtoDate.CertifiedTime = TimeStat, True, False))
        'Me.CTRLselTime.InitView(TimeStat, dtoDate.EndDateStr, dtoDate.OverEndDateStr, True, True, dtoDate.EndDate)
        Me.CTRLselTime.InitView(TimeStat, IIf(EpEndDate Is Nothing, "", ServiceEP.GetDate(EpEndDate)), "", True, True, EpEndDate)

        Me.TDmark.Visible = Not isAutoEp

        If Me.ItemType <> lm.Comol.Modules.EduPath.Domain.ItemType.SubActivity Then 'view stat != subactivity
            SetStartPager()
            Me.Master.ServiceTitle = Me.Resource.getValue("UsersEpStat")
            Me.PageUtility.AddAction(Me.CurrentCommunityID, Services_EduPath.ActionType.ViewUserStat, Me.PageUtility.CreateObjectsList(GetObjectType, ItemId), InteractionType.UserWithLearningObject)
            Me.BTNevaluate.Visible = False


            'Dim ItemStats As dtoListUserStat = ServiceEP.ServiceStat.GetGlobalEpStats(Me.ItemId, Me.CurrentCommunityID, ItemType, PageSize, PageIndex, TimeStat, CHBshowall.Checked)
            'GetUsersStats
            Dim ItemStats As dtoListUserStat = ServiceEP.ServiceStat.GetUsersStats(Me.ItemId, Me.CurrentCommunityID, ItemType, PageSize, PageIndex, TimeStat, CHBshowall.Checked)
            Dim idPath As Long = GetPathId()
            Dim hasCertification As Boolean = (ItemType = lm.Comol.Modules.EduPath.Domain.ItemType.Path) AndAlso ServiceEP.PathHasCertificateWithQuiz(idPath)
            Dim hasQuestionnaires As Boolean = (ItemType = lm.Comol.Modules.EduPath.Domain.ItemType.Path) AndAlso ServiceEP.PathHasSubActivityType(idPath, SubActivityType.Quiz)

            LNBexportPSfullCertificationToCsv.Visible = hasCertification
            LNBexportPSfullCertificationToXml.Visible = hasCertification
            LNBexportPSfullToCsv.Visible = hasQuestionnaires
            LNBexportPSfullToXml.Visible = hasQuestionnaires


            Me.LBitemType.Text = Me.Resource.getValue(ItemType.ToString) & ": "
            Me.LBitemName.Visible = True
            Me.LBitemName.Text = ItemStats.ItemName
            If ServiceEP.CheckStatus(ItemStats.Status, Status.Mandatory) Then
                Me.IMGmandatory.ImageUrl = RootObject.ImgMandatoryMedium(Me.BaseUrl)
                Me.IMGmandatory.Visible = True

            End If
            Me.SetBlockedStatus(ItemStats.Status)
            If ItemStats.usersStat.Count > 0 Then

                Me.RPitem.DataSource = ItemStats.usersStat
                AddHandler RPitem.ItemDataBound, AddressOf RPitem_ItemDataBound
                Me.RPitem.DataBind()
                Me.LBnoPartecipant.Visible = False
                DIVpartecipants.Visible = True
                DIVpartecipants.Style("display") = "block"
            Else
                hideControl(DIVpartecipants)
                Me.LBnoPartecipant.Visible = True

            End If



        ElseIf Me.ItemType = ItemType.SubActivity Then

            Me.TDdetail.Visible = False
            Me.BTNevaluate.Visible = Not isAutoEp
            If IsEvaluable Then 'evaluate subactivity
                Me.Master.ServiceTitle = Me.Resource.getValue("EvaluateSubAct")
                InitRblEvaluationFilter()
                Me.PageUtility.AddAction(Me.CurrentCommunityID, Services_EduPath.ActionType.Evaluate, Me.PageUtility.CreateObjectsList(Services_EduPath.ObjectType.SubActivity, ItemId), InteractionType.UserWithLearningObject)

                If IsNothing(ItemsIdToEval) OrElse IsNothing(EvaluatedItemsId) Then
                    ItemsIdToEval = ServiceEP.ServiceStat.GetSubActUsersStatIdsToEval(ItemId, EvaluationFilter.NotEvaluated)
                    EvaluatedItemsId = ServiceEP.ServiceStat.GetSubActUsersStatIdsToEval(ItemId, EvaluationFilter.Evaluated)
                End If

                Dim itemStat As dtoSubActListUserToEval

                If Filter = EvaluationFilter.NotEvaluated Then
                    itemStat = ServiceEP.ServiceStat.GetSubActUsersStatToEval(ItemId, ItemsIdToEval.Skip(PageIndex * PageSize).Take(PageSize).ToList)
                Else
                    itemStat = ServiceEP.ServiceStat.GetSubActUsersStatToEval(ItemId, EvaluatedItemsId.Skip(PageIndex * PageSize).Take(PageSize).ToList)
                End If

                SetStartPager()
                SetSubActivityName(itemStat)
                Me.SetBlockedStatus(itemStat.Status)
                If itemStat.userStat.Count > 0 Then
                    Me.ListOfVote = itemStat.userStat
                    Me.RPitem.DataSource = Me.ListOfVote

                    AddHandler RPitem.ItemDataBound, AddressOf RPsubAct_ItemDataBound
                    AddHandler RPitem.ItemCommand, AddressOf RPsubAct_ItemCommand
                    Me.RPitem.DataBind()
                Else
                    hideControl(DIVpartecipants)
                    Me.LBnoPartecipant.Visible = True

                End If
            Else 'view stat subactivity

                Me.Master.ServiceTitle = Me.Resource.getValue("UsersEpStat")
                Me.PageUtility.AddAction(Me.CurrentCommunityID, Services_EduPath.ActionType.ViewUserStat, Me.PageUtility.CreateObjectsList(GetObjectType, ItemId), InteractionType.UserWithLearningObject)

                Dim ItemStats As dtoSubActListUserStat = ServiceEP.ServiceStat.GetUsersSubActStats(ItemId, Me.CurrentCommunityID, PageSize, PageIndex, TimeStat)

                SetSubActivityName(ItemStats)
                Me.SetBlockedStatus(ItemStats.Status)
                SetStartPager()
                If ItemStats.usersStat.Count > 0 Then

                    Me.RPitem.DataSource = ItemStats.usersStat
                    AddHandler RPitem.ItemDataBound, AddressOf RPsubAct_ItemDataBound
                    Me.RPitem.DataBind()

                Else
                    hideControl(DIVpartecipants)
                    Me.LBnoPartecipant.Visible = True

                End If
            End If
            'If Me.ItemType = lm.Comol.Modules.EduPath.Domain.ItemType.Activity Then
            '    Me.TDdetail.Visible = False
            'End If

        Else
            ShowError(EpError.Url)
        End If

        Me.InitHyperLink()

    End Sub


    Public Overrides Sub BindNoPermessi()
        Me.ShowError(EpError.NotPermission)
    End Sub



    Public Overrides Function HasPermessi() As Boolean

        If IsEvaluable Then

            Return Not isAutoEp AndAlso ServiceEP.HasPermessi_ByItem(ItemId, CurrentCommunityID, lm.Comol.Modules.EduPath.Domain.ItemType.SubActivity) AndAlso ServiceEP.UserCanEvaluateParticipants(ItemId, Me.CurrentUserId, CurrentCommRoleID)

        Else
            Return ServiceEP.HasPermessi_ByItem(ItemId, CurrentCommunityID, ItemType) AndAlso ServiceEP.AdminCanViewUsersStatRecursive(ItemId, ItemType, CurrentUserId, CurrentCommRoleID) AndAlso Not (ItemType = lm.Comol.Modules.EduPath.Domain.ItemType.Unit And isAutoEp) AndAlso Not ServiceEP.IsTextItem(ItemId, ItemType)

        End If

    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Stat", "EduPath")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLinkButton(Me.LNBexportUsersStatisticToCsv, False, True)
            .setLinkButton(Me.LNBexportUsersStatisticToXml, False, True)
            .setLinkButton(Me.LNBexportUsersStatisticToPdf, False, True)
            .setLinkButton(Me.LNBexportPSfullToCsv, False, True)
            .setLinkButton(Me.LNBexportPSfullToXml, False, True)
            .setLinkButton(Me.LNBexportPSfullCertificationToCsv, False, True)
            .setLinkButton(Me.LNBexportPSfullCertificationToXml, False, True)
            .setHyperLink(Me.HYPuserStat, False, True)
            .setHyperLink(Me.HYPpathStat, False, True)
            .setLabel(Me.LBmarkTit)
            .setLabel(Me.LBnoPartecipant)
            .setCheckBox(Me.CHBshowall)
            If isTimeEp Then
                .setLabel_To_Value(Me.LBcompletionTit, "LBcompletionTit.time.text")
            Else
                .setLabel_To_Value(Me.LBcompletionTit, "LBcompletionTit.percent.text")
            End If

            If Not ItemType = lm.Comol.Modules.EduPath.Domain.ItemType.SubActivity Then
                LBmarkTit.Text &= .getValue("Min")
                LBcompletionTit.Text &= .getValue("Min")
            End If
            .setLabel(Me.LBnameTit)
            .setHyperLink(Me.HYPeduPathView, False, True)
            .setLabel(Me.LBstatusTit)
            .setLabel(Me.LBactionsTit)
            .setImage(Me.IMGmandatory, True)
            .setButton(Me.BTNevaluate, True)
            .setButton(Me.BTNsave, True)
            .setButton(Me.BTNnoSave, True)
            .setLinkButton(Me.LKBeduPathView, False, True)
            .setLinkButton(Me.LKBupdateList, False, True)
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


    Private Sub SetStartPager()
        Dim oPager As New lm.Comol.Core.DomainModel.PagerBase
        oPager.PageSize = PageSize

        If IsEvaluable Then

            If Filter = EvaluationFilter.Evaluated Then
                oPager.Count = Me.EvaluatedItemsId.Count - 1

            Else
                oPager.Count = Me.ItemsIdToEval.Count - 1

            End If

        Else
            oPager.Count = ServiceAssignment.GetAllParticipantInPathCount(ItemId, Me.CurrentCommunityID, ItemType, CTRLselTime.GetSelectedDate, CHBshowall.Checked) - 1

        End If
        Me.PGgrid.Visible = (oPager.Count + 1) > PageSize AndAlso PageIndex <= (Math.Ceiling(oPager.Count / PageSize))

        oPager.PageIndex = PageIndex

        Me.Pager = oPager
    End Sub
    Private Sub InitRblEvaluationFilter()
        If RBLfilter.Items.Count = 0 Then
            Me.RBLfilter.Visible = True
            Dim oListItem As New ListItem
            oListItem.Text = Me.Resource.getValue("EvaluationFilter." & EvaluationFilter.Evaluated.ToString())
            oListItem.Value = EvaluationFilter.Evaluated
            Me.RBLfilter.Items.Add(oListItem)
            oListItem = New ListItem
            oListItem.Text = Me.Resource.getValue("EvaluationFilter." & EvaluationFilter.NotEvaluated.ToString())
            oListItem.Value = EvaluationFilter.NotEvaluated

            Me.RBLfilter.Items.Add(oListItem)
            Me.RBLfilter.SelectedValue = Filter
        End If

    End Sub
    Private Sub UpdateScormStat()
        Dim pathId As Long = ServiceEP.GetPathId_ByItemId(ItemId, ItemType)
        Dim ModuleLinkIds As IList(Of Long) = ServiceEP.GetMaterialModuleLinkIds_ByPathId(pathId)
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

    Public isACertificate As Boolean = False

    Private Sub SetSubActivityName(Of T As IdtoSubActUserList)(ByRef subAct As T)
        Me.LBitemType.Text = Me.Resource.getValue(ItemType.ToString) & ": "
        Dim dtoItem As IdtoSubActUserList = subAct
        If ServiceEP.isSubActityInternalModule(subAct.ContentType) Then
            Me.LBitemName.Visible = True
            Me.PLHaction.Visible = True
            Me.CTRLtextAction.Visible = True

            'Dim oDisplayAction As lm.Comol.Modules.EduPath.Presentation.IViewModuleTextAction = Me.CTRLtextAction





            ''oDisplayAction.RefreshContainer = True
            'Dim initializer As New dtoInternalActionInitializer
            'initializer.SubActivity = ServiceEP.GetDtoSubActivity(subAct.IdSubActivity)
            'initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.text

            ''AddHandler oDisplayAction.RefreshContainerEvent, AddressOf RefreshContainerEvent
            'oDisplayAction.InitializeControl(initializer)

            'PLHaction.Controls.Add(oDisplayAction)

            Dim initializer As New dtoInternalActionInitializer
            ' initializer.IdPath = Me.cur
            initializer.IdPathCommunity = Me.CurrentCommunityID
            initializer.CookieName = CookieName
            initializer.SubActivity = ServiceEP.GetDtoSubActivity(subAct.IdSubActivity)

            initializer.ContainerCSS = SubActivityCssClass(initializer.SubActivity)
            initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.text
            Select Case initializer.SubActivity.ContentType
                Case SubActivityType.Text
                    Dim oDisplayAction As lm.Comol.Modules.EduPath.Presentation.IViewModuleTextAction = Me.CTRLtextAction
                    Me.CTRLtextAction.Visible = True
                    oDisplayAction.RefreshContainer = True

                    ' DIMENSIONI IMMAGINI
                    oDisplayAction.IconSize = Helpers.IconSize.Small
                    oDisplayAction.EnableAnchor = True
                    oDisplayAction.InitializeControl(initializer)
                Case SubActivityType.Certificate
                    Me.CTRLcertificationAction.Visible = True
                    Me.CTRLcertificationAction.EvaluablePath = Not isTimeEp()
                    Dim oCertAction As lm.Comol.Modules.EduPath.Presentation.IViewModuleCertificationAction = Me.CTRLcertificationAction
                    'oCertAction.RefreshContainer = True
                    ' DIMENSIONI IMMAGINI
                    oCertAction.IconSize = Helpers.IconSize.Small
                    oCertAction.EnableAnchor = True

                    oCertAction.InitializeControl(initializer)

                    isACertificate = True

                    TDdetail.Visible = True
            End Select
        Else
            Me.CTRLtextAction.Visible = False
            Me.PLHaction.Visible = True
            'Dim oDisplayAction As lm.Comol.Core.DomainModel.Common.iModuleActionView = CType(LoadControl(BaseUrl & lm.Comol.Core.DomainModel.Common.CoreRootObject.GenericDisplayActionControl), lm.Comol.Core.DomainModel.Common.iModuleActionView)
            'oDisplayAction.InitializeRemoteControlByLink(True, subAct.ModuleLink)


            Dim oDisplayAction As lm.Comol.Core.ModuleLinks.IExternalModuleDisplayAction = CType(LoadControl(BaseUrl & lm.Comol.Core.DomainModel.Common.CoreRootObject.GenericNewDisplayActionControl), lm.Comol.Core.ModuleLinks.IExternalModuleDisplayAction)
            'Dim oDisplayAction As lm.Comol.Core.ModuleLinks.IExternalModuleDisplayAction = e.Item .FindControl("textAction")
            '        hideControl(oLkb)
            '        oLb.Text = Me.SmartTagsAvailable.TagAll(dtoItem.Description)

            Dim initializer As New lm.Comol.Core.ModuleLinks.dtoExternalModuleInitializer
            initializer.Link = dtoItem.ModuleLink
            ' AGGIUNTA PLACEHOLDER
            ' --> initializer.PlaceHolders.Add(New lm.Comol.Core.ModuleLinks.dtoPlaceHolder() With {.Text = "HH:ss", .Type = lm.Comol.Core.ModuleLinks.PlaceHolderType.three})

            ' DEFINISCO UNA CLASSE PER IL CONTAINER
            'oDisplayAction.ContainerCSS = SubActivityCssClass(dtoItem)
            ' DIMENSIONI IMMAGINI
            oDisplayAction.IconSize = Helpers.IconSize.Small

            oDisplayAction.EnableAnchor = True

            oDisplayAction.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.text

            oDisplayAction.InitializeControl(initializer)
            'PLHaction.Controls.Add(oDisplayAction)


            PLHaction.Controls.Add(oDisplayAction)

        End If

        'Me.IMGsubType.Visible = True
        'Me.IMGsubType.ImageUrl = RootObject.ImgContentType(Me.BaseUrl, subAct.ContentType)
        'Me.IMGsubType.AlternateText = Me.Resource.getValue("SubActivityType." & subAct.ContentType.ToString)

        If ServiceEP.CheckStatus(subAct.Status, Status.Mandatory) Then
            Me.IMGmandatory.ImageUrl = RootObject.ImgMandatoryMedium(Me.BaseUrl)
            Me.IMGmandatory.Visible = True
        End If
    End Sub

    Private Sub InitHyperLink()

        Dim pathId As Long = IdCurrentPath

        'Me.HYPpathStat.ImageUrl = RootObject.ImgEduPathStat(Me.BaseUrl)
        Me.HYPpathStat.NavigateUrl = Me.BaseUrl & RootObject.PathStatistics(pathId, Me.CurrentCommunityID, TimeStat, CHBshowall.Checked)

        If ItemType <> lm.Comol.Modules.EduPath.Domain.ItemType.Path Then
            'Me.HYPuserStat.ImageUrl = RootObject.ImgUsersStat(Me.BaseUrl)
            Me.HYPuserStat.NavigateUrl = Me.BaseUrl & RootObject.UsersStatistics(pathId, Me.CurrentCommunityID, ItemType.Path, 0, TimeStat, CHBshowall.Checked)
        Else
            Me.HYPuserStat.Visible = False
        End If
        If IsEvaluable Then
            Me.LKBeduPathView.Visible = True
            Me.LKBupdateList.Visible = True
            Me.HYPeduPathView.Visible = False

        Else

            Me.HYPeduPathView.NavigateUrl = Me.BaseUrl & RootObject.PathView(pathId, Me.CurrentCommunityID, EpViewModeType.Manage, ServiceEP.isPlayModePath(pathId))
        End If
    End Sub


    Private Sub ShowError(ByVal errorType As EpError)
        Me.MLVstat.SetActiveView(Me.VIWerror)
        Me.Resource.setHyperLink(Me.HYPerror, False, True)
        Me.HYPerror.NavigateUrl = Me.BaseUrl & RootObject.EduPathList(Me.CurrentCommunityID, EpViewModeType.Manage)
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

    Private Sub SetLbValueWithMinValueFormat(ByRef oLb As Label, ByRef value As Int16, ByRef minValue As Int16)
        If isAutoEp Then
            oLb.Text = ServiceEP.GetTime(value) & " (" & ServiceEP.GetTime(minValue) & ")"
        Else
            oLb.Text = value & " (" & minValue & ")"
        End If

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
                oLb.CssClass = oLb.CssClass & "Ep_Item7Yellow"
        End Select
    End Sub

    Protected Function SetStatus(dtoItem As dtoUserStat) As String
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

    Protected Function SetStatusString(dtoItem As dtoUserStat) As String
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

    Private Sub SetImgColor(ByRef oImg As System.Web.UI.WebControls.Image, ByRef isRed As Boolean, Optional ByVal toolTip As String = "")
        If isRed Then
            oImg.ImageUrl = RootObject.ImgRed(Me.BaseUrl)
        Else
            oImg.ImageUrl = RootObject.ImgGreen(Me.BaseUrl)
        End If

        oImg.ToolTip = toolTip
        oImg.Visible = True
    End Sub

    Private Sub RPitem_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dtoItem As dtoUserStatExtended = e.Item.DataItem

            Dim oHyp As HyperLink

            Dim oLb As Label
            oLb = e.Item.FindControl("LBname")
            oLb.Text = dtoItem.SurnameAndName

            Dim oImg As System.Web.UI.WebControls.Image


            oLb = e.Item.FindControl("LBcompletion")
            SetLbValueWithMinValueFormat(oLb, dtoItem.Completion, (dtoItem.MinCompletion * dtoItem.Weight / 100))
            'oImg = e.Item.FindControl("IMGcompletion")
            'If isTimeEp Then
            '    SetImgColor(oImg, dtoItem.Completion < (dtoItem.Weight * dtoItem.MinCompletion / 100))
            'Else
            '    SetImgColor(oImg, dtoItem.Completion < dtoItem.MinCompletion)
            'End If

            If isAutoEp Then
                Dim columMark As HtmlTableCell = e.Item.FindControl("TDmark")
                columMark.Visible = False
            Else
                oLb = e.Item.FindControl("LBmark")
                oLb.Visible = True
                SetLbValueWithMinValueFormat(oLb, dtoItem.Mark, dtoItem.MinMark)
                oImg = e.Item.FindControl("IMGmark")
                SetImgColor(oImg, dtoItem.Mark < dtoItem.MinMark)
            End If

            oLb = e.Item.FindControl("LBstatus")
            SetLbStatus(oLb, dtoItem.StatusStat)

            oHyp = e.Item.FindControl("HYPdetail")
            Me.Resource.setHyperLink(oHyp, False, True)
            oHyp.Text = ""
            oHyp.NavigateUrl = Me.GetDetailUrl(dtoItem.UserId)
            If oHyp.NavigateUrl = "" Then
                oHyp.Visible = False
            End If

            'If isACertificate Then
            '    Dim uc As UC_CertificationAction = e.Item.FindControl("CTRLcertificate")
            '    uc.Visible = True

            '    Dim o As HtmlControl = e.Item.FindControl("TDdetail")
            '    o.Visible = True

            'End If

            'dtoItem.
        End If
    End Sub

    Protected Function FlagClass(dtoItem As dtoUserStat) As String


        Dim dtoItem1 As dtoUserStatExtended = TryCast(dtoItem, dtoUserStatExtended)

        If (Not dtoItem1 Is Nothing) Then

            If isTimeEp Then
                'SetImgColor(oImg, dtoItem.Completion < (dtoItem.Weight * dtoItem.MinCompletion / 100))
                If dtoItem.Completion < (dtoItem1.Weight * dtoItem1.MinCompletion / 100) Then
                    Return " no"
                Else
                    Return " yes"

                End If
            Else
                If dtoItem.Completion < dtoItem1.MinCompletion Then
                    Return " no"
                Else
                    Return " yes"

                End If
                'SetImgColor(oImg, dtoItem.Completion < dtoItem.MinCompletion)
            End If
        Else
            Return " hidden"
        End If
    End Function

    Private Sub RPsubAct_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dtoItem As dtoUserStat = e.Item.DataItem

            Dim oHyp As HyperLink

            Dim oLb As Label
            oLb = e.Item.FindControl("LBname")
            oLb.Text = dtoItem.SurnameAndName

            oLb = e.Item.FindControl("LBcompletion")
            oLb.Text = dtoItem.Completion

            oLb = e.Item.FindControl("LBstatus")
            SetLbStatus(oLb, dtoItem.StatusStat)

            Dim columTable As HtmlTableCell = e.Item.FindControl("TDdetail")
            columTable.Visible = False
            If isAutoEp Then

                Dim columMark As HtmlTableCell = e.Item.FindControl("TDmark")
                columMark.Visible = False

            ElseIf IsEvaluable Then

                Dim oPanel As Panel = e.Item.FindControl("PNLeval")
                oPanel.Visible = True

                Dim dtoEval As dtoUserStatToEvaluate = e.Item.DataItem
                Dim oTxt As TextBox = e.Item.FindControl("TXBmark")

                If dtoEval.Mark > -1 Then
                    oTxt.Text = dtoEval.Mark
                End If

                oTxt.Attributes.Add("userId", dtoEval.UserId)
                oTxt.Attributes.Add("statId", dtoEval.StatId)


                If ServiceEP.ServiceStat.CheckStatusStatistic(dtoEval.StatusStat, StatusStatistic.CompletedPassed) OrElse dtoEval.Completion < 100 Then
                    oTxt.Enabled = False
                End If
            Else
                oLb = e.Item.FindControl("LBmark")
                oLb.Visible = True
                oLb.Text = dtoItem.Mark
            End If

            If isACertificate Then
                Dim o As HtmlControl = e.Item.FindControl("TDdetail")
                o.Visible = True
                Dim initializer As New dtoInternalActionInitializer
                initializer.IdPath = IdCurrentPath
                initializer.IdPathCommunity = Me.CurrentCommunityID
                initializer.CookieName = CookieName
                initializer.SubActivity = ServiceEP.GetDtoSubActivity(ItemId)

                initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.none

                Dim control As UC_CertificationAction = e.Item.FindControl("CTRLcertificationAction")
                'control.Visible = False
                control.EvaluablePath = Not isTimeEp()
                Dim oCertAction As lm.Comol.Modules.EduPath.Presentation.IViewModuleCertificationAction = e.Item.FindControl("CTRLcertificationAction")
                'oCertAction.RefreshContainer = True
                ' DIMENSIONI IMMAGINI
                oCertAction.IconSize = Helpers.IconSize.Small

                Dim actions As List(Of dtoModuleActionControl) = oCertAction.InitializeRemoteControl(dtoItem.UserId, initializer, StandardActionType.None)
                Dim actStat As dtoModuleActionControl = actions.Where(Function(x) x.ControlType = StandardActionType.ViewPersonalStatistics).FirstOrDefault()

                Dim atleaststarted As Boolean = dtoItem.StatusStat <> StatusStatistic.None AndAlso dtoItem.StatusStat <> StatusStatistic.Browsed

                If (Not actStat Is Nothing) AndAlso (atleaststarted) Then
                    'Dim oLinkButton As LinkButton = e.Item.FindControl("LNBexportCertification")
                    'Me.Resource.setLinkButton(oLinkButton, False, True)
                    'oLinkButton.Visible = True
                    'oLinkButton.CommandArgument = dtoItem.UserId


                    Dim oHyperlink As HyperLink = e.Item.FindControl("HYPexportCertification")
                    Resource.setHyperLink(oHyperlink, False, True)
                    oHyperlink.Visible = True
                    oHyperlink.NavigateUrl = PageUtility.ApplicationUrlBase & RootObject.CertificationManagerDownloadPage(CTRLselTime.GetSelectedDate.Ticks, IdCurrentPath, initializer.SubActivity.ActivityParentId, initializer.SubActivity.Id, dtoItem.UserId)

                    '  Case "certification"
                    'Dim control As UC_CertificationAction = e.Item.FindControl("CTRLcertificationAction")
                    'control.DownloadCertification(IdCurrentPath, CLng(e.CommandArgument), ServiceEP.GetDtoSubActivity(ItemId), CTRLselTime.GetSelectedDate, Me.HDNdownloadTokenValue.Value)


                    'oLinkButton = e.Item.FindControl("LNBrefreshCertificate")
                    'Me.Resource.setLinkButton(oLinkButton, False, True)
                    'oLinkButton.Visible = True
                    'oLinkButton.CommandArgument = dtoItem.UserId

                    oHyperlink = e.Item.FindControl("HYPrefreshCertificate")
                    Resource.setHyperLink(oHyperlink, False, True)
                    oHyperlink.Visible = True

                    '(PathID, UserToViewStat, ServiceEP.GetDtoSubActivity(ItemId), Me.HDNdownloadTokenValue.Value)
                    oHyperlink.NavigateUrl = PageUtility.ApplicationUrlBase & ServiceEP.GetCertificationRestoreUrl(IdCurrentPath, initializer.SubActivity.ActivityParentId, initializer.SubActivity.Id, dtoItem.UserId)

                ElseIf dtoItem.StatusStat = StatusStatistic.Completed OrElse dtoItem.StatusStat = StatusStatistic.CompletedPassed Then
                    'Dim oLinkButton As LinkButton = e.Item.FindControl("LNBexportCertification")
                    'Me.Resource.setLinkButton(oLinkButton, False, True)
                    'oLinkButton.Visible = True
                    'oLinkButton.CommandArgument = dtoItem.UserId
                    Dim oHyperlink As HyperLink = e.Item.FindControl("HYPexportCertification")
                    Resource.setHyperLink(oHyperlink, False, True)
                    oHyperlink.Visible = True
                    oHyperlink.NavigateUrl = PageUtility.ApplicationUrlBase & RootObject.CertificationManagerDownloadPage(CTRLselTime.GetSelectedDate.Ticks, IdCurrentPath, initializer.SubActivity.ActivityParentId, initializer.SubActivity.Id, dtoItem.UserId)

                End If
            End If

            oHyp = e.Item.FindControl("HYPdetail")
            Me.Resource.setHyperLink(oHyp, False, True)
            oHyp.Text = ""
            oHyp.NavigateUrl = Me.GetDetailUrl(dtoItem.UserId)
            If oHyp.NavigateUrl = "" Then
                oHyp.Visible = False
            End If

        End If
    End Sub
    Public Sub RPsubAct_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs)
        CTRLmessages.Visible = False
        Select Case e.CommandName
            'Case "certification"
            '    Dim control As UC_CertificationAction = e.Item.FindControl("CTRLcertificationAction")
            '    control.DownloadCertification(IdCurrentPath, CLng(e.CommandArgument), ServiceEP.GetDtoSubActivity(ItemId), CTRLselTime.GetSelectedDate, Me.HDNdownloadTokenValue.Value)

            'Case "refreshcertification"
            '    Dim control As UC_CertificationAction = e.Item.FindControl("CTRLcertificationAction")
            '    control.RestoreCertificate(IdCurrentPath, CLng(e.CommandArgument), ServiceEP.GetDtoSubActivity(ItemId), Me.HDNdownloadTokenValue.Value)

        End Select
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
    Private Function GetObjectType() As Services_EduPath.ObjectType

        Select Case ItemType
            Case lm.Comol.Modules.EduPath.Domain.ItemType.Path
                Return Services_EduPath.ObjectType.EduPath

            Case lm.Comol.Modules.EduPath.Domain.ItemType.Unit
                Return Services_EduPath.ObjectType.Unit

            Case lm.Comol.Modules.EduPath.Domain.ItemType.Activity
                Return Services_EduPath.ObjectType.Activity

            Case lm.Comol.Modules.EduPath.Domain.ItemType.SubActivity
                Return Services_EduPath.ObjectType.SubActivity

            Case Else
                Return Services_EduPath.ObjectType.None
        End Select

    End Function

    Private Function GetVoteToSave()
        Dim ListOfUpdatedVote As New List(Of dtoUserStatToEvaluate)

        For Each item As RepeaterItem In RPitem.Items

            Dim oTxt As TextBox = item.FindControl("TXBmark")
            If oTxt.Text.Count > 0 AndAlso oTxt.Text <> ListOfVote.ElementAt(item.ItemIndex).Mark Then
                ListOfVote.ElementAt(item.ItemIndex).Mark = oTxt.Text
                ListOfUpdatedVote.Add(ListOfVote.ElementAt(item.ItemIndex))
            End If

        Next

        Return ListOfUpdatedVote
    End Function

    Private Sub BTNevaluate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNevaluate.Click
        CTRLmessages.Visible = False
        ListOfUpdatedVote = Me.GetVoteToSave
        If Not ServiceEP.ServiceStat.UpdateSubActsMark(ItemId, ListOfUpdatedVote, Me.CurrentCommunityID, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress, TimeStat) Then
            Me.ShowError(EpError.Generic)
        End If

    End Sub


    Private Sub SetBlockedStatus(ByRef status As Status)

        If Me.ServiceEP.AdminCanUpdate(ItemId, ItemType, Me.CurrentUserId, CurrentCommRoleID) Then
            SetBlockedStatusLkb(Me.LKBblocked, Me.ServiceEP.CheckStatus(status, lm.Comol.Modules.EduPath.Domain.Status.Locked))
        Else
            setBlockedStatusImg(Me.IMGblocked, Me.ServiceEP.CheckStatus(status, lm.Comol.Modules.EduPath.Domain.Status.Locked))
        End If
    End Sub
    Private Sub SetBlockedStatusLkb(ByRef oLkb As LinkButton, ByVal isBlocked As Boolean)

        Me.Resource.setLinkButton(oLkb, False, True)

        oLkb.Text = "<img src=" & """" & "{0}" & """" & " alt=" & """" & "{1}" & """" & "/>"
        oLkb.Visible = True
        oLkb.Text = String.Format(oLkb.Text, RootObject.ImgBtnBlocked(Me.BaseUrl, Not isBlocked), oLkb.ToolTip)

    End Sub
    Private Sub setBlockedStatusImg(ByRef oImg As System.Web.UI.WebControls.Image, ByVal isBlocked As Boolean)
        oImg.ImageUrl = RootObject.ImgItemBlocked(Me.BaseUrl, Not isBlocked)
        oImg.AlternateText = Resource.getValue("Blocked." & isBlocked)
    End Sub
    Private Sub LKBblocked_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBblocked.Click
        CTRLmessages.Visible = False
        Select Case Me.ItemType
            Case lm.Comol.Modules.EduPath.Domain.ItemType.Path

                Select Case ServiceEP.UpdatePathVisibilityStatus(Me.ItemId, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress)
                    Case 1
                        BindDati()
                    Case 0
                        ShowError(EpError.Data)
                    Case 2
                        LTalert.Text = Resource.getValue("MSGalert")
                    Case 3
                        LTalert.Text = Resource.getValue("EpError.EmptyActivities")
                End Select
            Case lm.Comol.Modules.EduPath.Domain.ItemType.Unit
                If ServiceEP.UpdateUnitVisibilityStatus(Me.ItemId, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then
                    BindDati()
                Else
                    ShowError(EpError.Generic)
                End If

            Case lm.Comol.Modules.EduPath.Domain.ItemType.Activity
                If ServiceEP.UpdateActivityVisibilityStatus(Me.ItemId, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then
                    BindDati()
                Else
                    ShowError(EpError.Generic)
                End If

            Case lm.Comol.Modules.EduPath.Domain.ItemType.SubActivity
                If ServiceEP.UpdateSubActivityVisibilityStatus(Me.ItemId, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then
                    BindDati()
                Else
                    ShowError(EpError.Generic)
                End If

        End Select
    End Sub


#Region "Export"
    Private Sub LNBexportUsersStatisticToCsv_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBexportUsersStatisticToCsv.Click
        ExportUsersStatistics(Helpers.Export.ExportFileType.csv)
    End Sub
    Private Sub LNBexportUsersStatisticToXml_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBexportUsersStatisticToXml.Click
        ExportUsersStatistics(Helpers.Export.ExportFileType.xml)
    End Sub
    Private Sub LNBexportUsersStatisticToPdf_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBexportUsersStatisticToPdf.Click
        ExportUsersStatistics(Helpers.Export.ExportFileType.pdf)
    End Sub
    Private Sub ExportUsersStatistics(exportType As lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType)
        Dim cookie As HttpCookie = Nothing
        If Not String.IsNullOrEmpty(CookieName) Then
            cookie = New HttpCookie(CookieName, HDNdownloadTokenValue.Value)
        End If
        CTRLmessages.Visible = False
        Dim clientFileName As String = GetFileName(exportType)
        Dim translations As New Dictionary(Of EduPathTranslations, String)
        For Each name As String In [Enum].GetNames(GetType(EduPathTranslations))
            translations.Add([Enum].Parse(GetType(EduPathTranslations), name), Me.Resource.getValue("EduPathTranslations." & name))
        Next

        Dim roles As List(Of Comol.Entity.Role) = COL_BusinessLogic_v2.CL_permessi.COL_TipoRuolo.List(PageUtility.LinguaID).OrderBy(Function(t) t.Name).ToList
        Dim roleTranslations As Dictionary(Of Integer, String) = roles.ToDictionary(Function(r) r.ID, Function(r) r.Name)

        Response.Clear()

        Try
            Dim settings As dtoExportConfigurationSetting = ServiceEP.GetExportSetting(Me.GetPathId, CurrentCommunityID, StatisticsPageType.UsersStatistics, ConfigurationType.Export)

            Dim gAction As lm.Comol.Core.ModuleLinks.IExternalModuleDisplayAction = CType(LoadControl(BaseUrl & lm.Comol.Core.DomainModel.Common.CoreRootObject.GenericNewDisplayActionControl), lm.Comol.Core.ModuleLinks.IExternalModuleDisplayAction)
            Dim tAction As lm.Comol.Modules.EduPath.Presentation.IViewModuleTextAction = CType(LoadControl(BaseUrl & RootObject.RenderTextAction), lm.Comol.Modules.EduPath.Presentation.IViewModuleTextAction)
            Dim cAction As lm.Comol.Modules.EduPath.Presentation.IViewModuleCertificationAction = CType(LoadControl(BaseUrl & RootObject.RenderCertificationAction), lm.Comol.Modules.EduPath.Presentation.IViewModuleCertificationAction)

            If exportType = Helpers.Export.ExportFileType.pdf Then
                Dim oTemplate As New lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template
                'oTemplate.Header = Master.getTemplateHeader
                'oTemplate.Footer = Master.getTemplateFooter
                oTemplate.Settings = lm.Comol.Core.DomainModel.Helpers.Export.ExportBaseHelper.GetDefaultPageSettings()
                oTemplate.Settings.Size = DocTemplateVers.PageSize.A4_L

                Dim doc As iTextSharp5.text.Document
                If Me.ItemType <> lm.Comol.Modules.EduPath.Domain.ItemType.SubActivity Then

                    doc = ServiceEP.ServiceStat.ExportUsersStatistics_ToPDf(PageUtility.CurrentContext, ItemId, Me.CurrentCommunityID, ItemType, Me.CHBshowall.Checked, translations, roleTranslations, settings, ExporPathData.Normal, oTemplate, False, clientFileName, Response, New HttpCookie(CookieName, HDNdownloadTokenValue.Value), TimeStat)
                Else
                    oTemplate.Settings.Size = DocTemplateVers.PageSize.A4
                    doc = ServiceEP.ServiceStat.ExportUsersSubActivityStatistics_ToPdf(PageUtility.CurrentContext, ItemId, Me.CurrentCommunityID, Me.CHBshowall.Checked, translations, roleTranslations, settings, ExporPathData.Normal, gAction, tAction, cAction, oTemplate, False, clientFileName, Response, New HttpCookie(CookieName, HDNdownloadTokenValue.Value), TimeStat)

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
                        If ItemType <> lm.Comol.Modules.EduPath.Domain.ItemType.SubActivity Then
                            Response.Write(ServiceEP.ServiceStat.ExportUsersStatistics_ToXml(PageUtility.CurrentContext, ItemId, Me.CurrentCommunityID, ItemType, CHBshowall.Checked, translations, roleTranslations, settings, ExporPathData.Normal, TimeStat))
                        Else
                            Response.Write(ServiceEP.ServiceStat.ExportUsersSubActivityStatistics_ToXml(PageUtility.CurrentContext, ItemId, Me.CurrentCommunityID, CHBshowall.Checked, translations, roleTranslations, settings, ExporPathData.Normal, gAction, tAction, cAction, TimeStat))
                        End If
                    Case Else
                        Response.ContentType = "text/csv"
                        If Me.ItemType <> lm.Comol.Modules.EduPath.Domain.ItemType.SubActivity Then
                            Response.Write(ServiceEP.ServiceStat.ExportUsersStatistics_ToCsv(PageUtility.CurrentContext, ItemId, Me.CurrentCommunityID, ItemType, Me.CHBshowall.Checked, translations, roleTranslations, settings, ExporPathData.Normal, TimeStat))
                        Else
                            Response.Write(ServiceEP.ServiceStat.ExportUsersSubActivityStatistics_ToCsv(PageUtility.CurrentContext, ItemId, Me.CurrentCommunityID, Me.CHBshowall.Checked, translations, roleTranslations, settings, ExporPathData.Normal, gAction, tAction, cAction, TimeStat))
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

    Private Function GetFileName(ByVal type As lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType) As String
        Dim filename As String = Resource.getValue("Export.Filename." & IIf(ItemType = Global.lm.Comol.Modules.EduPath.Domain.ItemType.SubActivity, "SubActivityStatistics", "UsersStatistics"))
        Dim oDate As DateTime = DateTime.Now
        If String.IsNullOrEmpty(filename) Then
            filename = "PathUsersStatistics_{0}_{1}_{2}"
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
        Dim displayname As String = ServiceEP.GetPathName(GetPathId)
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
        CTRLmessages.Visible = False
        Dim stat As dtoPathUsers = ServiceStat.GetPathUsersStat(Me.GetPathId, TimeStat, 0, 0, CHBshowall.Checked, (export = ExporPathData.Certification))

        Dim clientFileName As String = GetFileName(exportType, (export = ExporPathData.Certification))
        Dim translations As New Dictionary(Of EduPathTranslations, String)
        For Each name As String In [Enum].GetNames(GetType(EduPathTranslations))
            translations.Add([Enum].Parse(GetType(EduPathTranslations), name), Me.Resource.getValue("EduPathTranslations." & name))
        Next

        Dim roles As List(Of Comol.Entity.Role) = COL_BusinessLogic_v2.CL_permessi.COL_TipoRuolo.List(PageUtility.LinguaID).OrderBy(Function(t) t.Name).ToList
        Dim roleTranslations As Dictionary(Of Integer, String) = roles.ToDictionary(Function(r) r.ID, Function(r) r.Name)

        Response.Clear()
        Try
            Dim settings As dtoExportConfigurationSetting = ServiceEP.GetExportSetting(Me.GetPathId, CurrentCommunityID, StatisticsPageType.UsersStatistics, ConfigurationType.Export)
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

    Private Sub RBLfilter_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLfilter.SelectedIndexChanged
        CTRLmessages.Visible = False
        ListOfUpdatedVote = Me.GetVoteToSave
        If ListOfUpdatedVote.Count > 0 Then
            _buttonClicked = ButtonClicked.RblFilter
            Me.LTopenDialog.Visible = True
        Else
            Me.LTopenDialog.Visible = False
            Me.PageUtility.RedirectToUrl(RootObject.EvaluateSubAct(ItemId, Me.CurrentCommunityID, Me.RBLfilter.SelectedValue, Me.Pager.PageIndex, TimeStat))
        End If
    End Sub

    Private Sub PGgrid_OnPageSelected() Handles PGgrid.OnPageSelected
        Me.Pager = Me.PGgrid.Pager
        Dim ItemCount As Integer
        CTRLmessages.Visible = False
        If IsEvaluable Then

            ListOfUpdatedVote = GetVoteToSave()

            If ListOfUpdatedVote.Count > 0 Then
                _buttonClicked = ButtonClicked.Pager
                Me.LTopenDialog.Visible = True

            Else
                Me.LTopenDialog.Visible = False
                Me.PageUtility.RedirectToUrl(RootObject.EvaluateSubAct(ItemId, Me.CurrentCommunityID, Me.RBLfilter.SelectedValue, Me.Pager.PageIndex, TimeStat))

            End If

        Else

            Me.PageUtility.RedirectToUrl(RootObject.UsersStatistics(ItemId, Me.CurrentCommunityID, ItemType, Me.Pager.PageIndex, TimeStat, CHBshowall.Checked))
        End If

    End Sub

    Private Sub ClearSession()
        If Not IsNothing(SessionUniqueKey) Then
            Me.Session("ListOfVote_" & SessionUniqueKey) = Nothing
            Me.Session("ListOfUpdatedVote_" & SessionUniqueKey) = Nothing
            Me.Session("ItemsIdToEval_" & SessionUniqueKey) = Nothing
            Me.Session("EvaluatedItemsId_" & SessionUniqueKey) = Nothing
            Me.Session(SessionUniqueKey.ToString) = Nothing
        End If
    End Sub


    Private Sub BTNnoSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNnoSave.Click
        CTRLmessages.Visible = False
        Me.LTopenDialog.Visible = False
        Redirect_byButtonPressed()

    End Sub


    Private Sub Redirect_byButtonPressed()
        CTRLmessages.Visible = False
        Select Case _buttonClicked

            Case ButtonClicked.Pager
                Me.PageUtility.RedirectToUrl(RootObject.EvaluateSubAct(ItemId, Me.CurrentCommunityID, Me.RBLfilter.SelectedValue, Me.Pager.PageIndex, TimeStat))

            Case ButtonClicked.RblFilter
                Me.PageUtility.RedirectToUrl(RootObject.EvaluateSubAct(ItemId, Me.CurrentCommunityID, Me.RBLfilter.SelectedValue, 0, Me.TimeStat))

            Case ButtonClicked.EduPathView
                Me.ClearSession()
                Dim pathId As Long = IdCurrentPath
                Me.PageUtility.RedirectToUrl(RootObject.PathView(pathId, Me.CurrentCommunityID, EpViewModeType.Manage, ServiceEP.isPlayModePath(pathId)))

            Case ButtonClicked.UpdateList
                Me.ClearSession()
                Me.PageUtility.RedirectToUrl(RootObject.EvaluateSubAct(ItemId, Me.CurrentCommunityID, Me.RBLfilter.SelectedValue, 0, Me.TimeStat))

        End Select
    End Sub

    Private Sub BTNsave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNsave.Click
        CTRLmessages.Visible = False
        Me.LTopenDialog.Visible = False

        If ServiceEP.ServiceStat.UpdateSubActsMark(ItemId, ListOfUpdatedVote, Me.CurrentCommunityID, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress, TimeStat) Then

            Redirect_byButtonPressed()
        Else

            Me.ShowError(EpError.Generic)
        End If

    End Sub


    Private Sub LKBeduPathView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBeduPathView.Click
        CTRLmessages.Visible = False
        ListOfUpdatedVote = GetVoteToSave()

        If ListOfUpdatedVote.Count > 0 Then
            _buttonClicked = ButtonClicked.EduPathView
            Me.LTopenDialog.Visible = True

        Else
            Me.LTopenDialog.Visible = False
            ClearSession()
            Dim pathId As Long = IdCurrentPath
            Me.PageUtility.RedirectToUrl(RootObject.PathView(pathId, Me.CurrentCommunityID, EpViewModeType.Manage, ServiceEP.isPlayModePath(pathId)))
        End If
    End Sub

    Private Sub LKBupdateList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBupdateList.Click
        CTRLmessages.Visible = False
        ListOfUpdatedVote = GetVoteToSave()

        If ListOfUpdatedVote.Count > 0 Then
            _buttonClicked = ButtonClicked.UpdateList
            Me.LTopenDialog.Visible = True

        Else
            Me.LTopenDialog.Visible = False
            ClearSession()
            Me.PageUtility.RedirectToUrl(RootObject.EvaluateSubAct(ItemId, Me.CurrentCommunityID, Me.RBLfilter.SelectedValue, 0, TimeStat))

        End If

    End Sub

    Protected ReadOnly Property BackGroundItem(itemtype As ListItemType) As String
        Get
            If itemtype = ListItemType.Item Then
                Return "ROW_Normal_Small"
            Else
                Return "ROW_Alternate_Small"
            End If
        End Get
    End Property

    Private Enum ButtonClicked
        EduPathView = 0
        UpdateList = 1
        Pager = 2
        RblFilter = 3
    End Enum


    Private Sub BTNevaluate_Unload(sender As Object, e As System.EventArgs) Handles BTNevaluate.Unload

    End Sub

    Private Sub CHBshowall_CheckedChanged(sender As Object, e As System.EventArgs) Handles CHBshowall.CheckedChanged
        CTRLmessages.Visible = False
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