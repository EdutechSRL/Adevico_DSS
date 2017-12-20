Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation
Imports lm.Comol.Modules.CallForPapers.Domain
Imports lm.ActionDataContract
Imports lm.Comol.Core.TemplateMessages.Domain

Public Class ListCalls
    Inherits PageBase
    Implements IViewCallsList


#Region "Context"
    Private _Presenter As CallsListPresenter
    Private ReadOnly Property CurrentPresenter() As CallsListPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New CallsListPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property Portalname As String Implements IViewBaseCallList(Of lm.Comol.Modules.CallForPapers.Domain.dtoCallItemPermission).Portalname
        Get
            Return Resource.getValue("Portalname")
        End Get
    End Property
    Private ReadOnly Property PreloadAction As CallStandardAction Implements IViewBaseCallList(Of dtoCallItemPermission).PreloadAction
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of CallStandardAction).GetByString(Request.QueryString("action"), CallStandardAction.List)
        End Get
    End Property
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewBaseCallList(Of dtoCallItemPermission).PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadView As CallStatusForSubmitters Implements IViewBaseCallList(Of dtoCallItemPermission).PreloadView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of CallStatusForSubmitters).GetByString(Request.QueryString("View"), CallStatusForSubmitters.SubmissionOpened)
        End Get
    End Property
    Private Property AllowManage As Boolean Implements IViewBaseCallList(Of dtoCallItemPermission).AllowManage
        Get
            Return ViewStateOrDefault("AllowManage", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowManage") = value
        End Set
    End Property
    Private Property AllowSubmmissions As Boolean Implements IViewBaseCallList(Of dtoCallItemPermission).AllowSubmmissions
        Get
            Return ViewStateOrDefault("AllowSubmmissions", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowSubmmissions") = value
        End Set
    End Property
    Private Property AllowView As Boolean Implements IViewBaseCallList(Of dtoCallItemPermission).AllowView
        Get
            Return ViewStateOrDefault("AllowView", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowView") = value
        End Set
    End Property
    Private Property CurrentAction As CallStandardAction Implements IViewBaseCallList(Of dtoCallItemPermission).CurrentAction
        Get
            Return ViewStateOrDefault("CurrentAction", CallStandardAction.List)
        End Get
        Set(ByVal value As CallStandardAction)
            Me.ViewState("CurrentAction") = value
        End Set
    End Property
    Private Property CurrentFilter As FilterCallVisibility Implements IViewBaseCallList(Of dtoCallItemPermission).CurrentFilter
        Get
            If Me.RBLvisibility.Items.Count > 0 Then
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of FilterCallVisibility).GetByString(Me.RBLvisibility.SelectedValue, FilterCallVisibility.None)
            End If
            Return FilterCallVisibility.None
        End Get
        Set(ByVal value As FilterCallVisibility)
            If Not IsNothing(Me.RBLvisibility.Items.FindByValue(value.ToString)) Then
                Me.RBLvisibility.SelectedValue = value.ToString
            End If
        End Set
    End Property
    Private Property CurrentView As CallStatusForSubmitters Implements IViewBaseCallList(Of dtoCallItemPermission).CurrentView
        Get
            If TypeOf Me.ViewState("CurrentView") Is CallStatusForSubmitters Then
                Return CType(Me.ViewState("CurrentView"), CallStatusForSubmitters)
            End If
            Return CallStatusForSubmitters.SubmissionOpened
        End Get
        Set(ByVal value As CallStatusForSubmitters)
            Dim oTab As Telerik.Web.UI.RadTab = Me.TBScall.FindTabByValue(value.ToString)
            If Not IsNothing(oTab) Then
                oTab.Selected = True
                Me.ViewState("CurrentView") = value
            End If
        End Set
    End Property
    Private Property IdCallCommunity As Integer Implements IViewBaseCallList(Of dtoCallItemPermission).IdCallCommunity
        Get
            Return ViewStateOrDefault("IdCallCommunity", CInt(0))
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("IdCallCommunity") = value
        End Set
    End Property
    Private Property Pager As lm.Comol.Core.DomainModel.PagerBase Implements IViewBaseCallList(Of dtoCallItemPermission).Pager
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PGgridTop.Pager = Me.Pager
        Me.PGgridBottom.Pager = Me.Pager
    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False



        Me.CurrentPresenter.InitView()

        If Me.CurrentAction = CallStandardAction.Manage Then
            CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.CallListManager,
            -1,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.CallForPeaper,
            "")
        Else
            CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.CallListUser,
            -1,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.CallForPeaper,
            "")
        End If

        'View.CurrentAction = action;

    End Sub

    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True


        If Me.CurrentAction = CallStandardAction.Manage Then
            CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.CallListManager,
            -1,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.CallForPeaper,
            "NoPermission")
        Else
            CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.CallListUser,
            -1,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.CallForPeaper,
            "NoPermission")
        End If

    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Calls", "Modules", "CallForPapers")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            LBnoCalls.Text = Resource.getValue("LBnoCalls." & CallForPaperType.CallForBids.ToString())
            .setLiteral(LTpageTop)
            .setLiteral(LTpageBottom)
            .setLabel(LBitemsVisibility_t)
            .setHyperLink(HYPmanage, CallForPaperType.CallForBids.ToString(), True, True)
            .setHyperLink(HYPlist, CallForPaperType.CallForBids.ToString(), True, True)
            .setHyperLink(HYPcreateCall, CallForPaperType.CallForBids.ToString(), True, True)

            Me.DLGremoveItem.DialogTitle = Me.Resource.getValue("DLGremoveItemTitle." & CallForPaperType.CallForBids.ToString)
            Me.DLGremoveItem.DialogText = Me.Resource.getValue("DLGremoveItemText." & CallForPaperType.CallForBids.ToString)
            Me.DLGremoveItem.InitializeControl(New List(Of String), -1)
        End With
    End Sub

    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub SetContainerName(idCommunity As Integer, name As String) Implements IViewBaseCallList(Of dtoCallItemPermission).SetContainerName
        Dim title As String = Me.Resource.getValue("serviceTitle." & CurrentAction.ToString() & "." & CallForPaperType.CallForBids.ToString())
        Master.ServiceTitle = title
        If idCommunity = 0 Then
            Master.ServiceTitleToolTip = title
        Else
            Dim tooltip As String = Me.Resource.getValue("serviceTitle.Community." & CurrentAction.ToString() & "." & CallForPaperType.CallForBids.ToString())
            If Not String.IsNullOrEmpty(tooltip) Then
                Master.ServiceTitleToolTip = String.Format(tooltip, name)
            End If
        End If
    End Sub
    Private Sub SetActionUrl(action As CallStandardAction, url As String) Implements IViewBaseCallList(Of dtoCallItemPermission).SetActionUrl
        Select Case action
            Case CallStandardAction.List
                Me.HYPlist.Visible = True
                Me.HYPlist.NavigateUrl = BaseUrl & url
            Case CallStandardAction.Manage
                Me.HYPmanage.Visible = True
                Me.HYPmanage.NavigateUrl = BaseUrl & url
            Case CallStandardAction.Add
                Me.HYPcreateCall.Visible = True
                Me.HYPcreateCall.NavigateUrl = BaseUrl & url
        End Select
    End Sub
    Private Sub LoadAvailableFilters(filters As List(Of FilterCallVisibility), current As FilterCallVisibility) Implements IViewBaseCallList(Of dtoCallItemPermission).LoadAvailableFilters
        Me.RBLvisibility.Items.Clear()
        Me.DVfilters.Visible = (filters.Count > 1)
        If filters.Count > 0 Then
            For Each filter As FilterCallVisibility In filters
                Me.RBLvisibility.Items.Add(New ListItem(Me.Resource.getValue("FilterCallVisibility." & CallForPaperType.CallForBids.ToString() & "." & filter.ToString), filter.ToString))
            Next
            Me.RBLvisibility.SelectedValue = current.ToString()
        End If
    End Sub
    Private Sub LoadAvailableView(idCommunity As Integer, views As List(Of CallStatusForSubmitters)) Implements IViewBaseCallList(Of dtoCallItemPermission).LoadAvailableView
        Me.TBScall.Enabled = (views.Count > 0)
        Dim SelectedIndex As Integer = 0
        If views.Count > 0 Then
            For Each view As CallStatusForSubmitters In views
                Dim oTabView As Telerik.Web.UI.RadTab = Me.TBScall.Tabs.FindTabByValue(view.ToString)
                If Not IsNothing(oTabView) Then
                    If view = CallStatusForSubmitters.Revisions Then
                        oTabView.NavigateUrl = Me.PageUtility.BaseUrl & RootObject.ViewRevisions(CallForPaperType.CallForBids, CurrentAction, idCommunity, view)
                    Else
                        oTabView.NavigateUrl = Me.PageUtility.BaseUrl & RootObject.ViewCalls(CallForPaperType.CallForBids, CurrentAction, idCommunity, view)
                    End If
                    oTabView.Text = Resource.getValue("CallStatusForSubmitters." & CallForPaperType.CallForBids.ToString & "." & view.ToString)
                    oTabView.Visible = True
                End If
            Next
        End If
        Me.TBScall.SelectedIndex = SelectedIndex
    End Sub

    Private Sub LoadCalls(items As List(Of dtoCallItemPermission)) Implements IViewBaseCallList(Of dtoCallItemPermission).LoadCalls
        If items.Count = 0 Then
            Me.MLVresults.SetActiveView(VIWnoItems)
        Else
            Me.MLVresults.SetActiveView(VIWlist)
            Me.RPTcalls.DataSource = items
            Me.RPTcalls.DataBind()
        End If
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewBase.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, ModuleCallForPaper.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub

    Private Sub DisplaySessionTimeout() Implements IViewBase.DisplaySessionTimeout
        Dim idCommunity As Integer = PreloadIdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = RootObject.ViewCalls(CallForPaperType.CallForBids, PreloadAction, idCommunity, PreLoadedContentView)
        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If

        If Me.CurrentAction = CallStandardAction.Manage Then
            CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.CallListManager,
            -1,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.CallForPeaper,
            "SessionTimeOut")
        Else
            CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.CallListUser,
            -1,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.CallForPeaper,
            "SessionTimeOut")
        End If

        webPost.Redirect(dto)
    End Sub
    Private Sub SendUserActionList(idCommunity As Integer, idModule As Integer) Implements IViewBaseCallList(Of dtoCallItemPermission).SendUserActionList
        Me.PageUtility.AddActionToModule(idCommunity, idModule, IIf(CurrentAction = CallStandardAction.List, ModuleCallForPaper.ActionType.List, ModuleCallForPaper.ActionType.Manage), , InteractionType.UserWithLearningObject)
    End Sub

    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As ModuleCallForPaper.ActionType) Implements IViewCallsList.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleCallForPaper.ObjectType.CallForPaper, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub

    Private Sub CloneSkinAssociation(idUser As Integer, idModule As Integer, idCommunity As Integer, idOldModuleItem As Long, idNewModuleItem As Long, idItemType As Integer, fullyQualifiedName As String) Implements IViewBaseCallList(Of dtoCallItemPermission).CloneSkinAssociation
        Try
            Dim skinService As New lm.Comol.Modules.Standard.Skin.Business.ServiceSkin(PageUtility.CurrentContext)
            skinService.CloneSkinAssociation(idUser, idModule, idCommunity, idOldModuleItem, idNewModuleItem, idItemType, fullyQualifiedName)
            skinService = Nothing
        Catch ex As Exception

        End Try
    End Sub
    Private Sub RemoveSkinAssociation(idUser As Integer, idModule As Integer, idCommunity As Integer, idCall As Long, idItemType As Integer, fullyQualifiedName As String) Implements IViewBaseCallList(Of dtoCallItemPermission).RemoveSkinAssociation
        Try
            Dim skinService As New lm.Comol.Modules.Standard.Skin.Business.ServiceSkin(PageUtility.CurrentContext)
            skinService.DeleteSkinAssociation(idModule, idCommunity, idCall, idItemType, fullyQualifiedName)
            skinService = Nothing
        Catch ex As Exception

        End Try
    End Sub
    Private Sub GotoUrl(url As String) Implements IViewBaseCallList(Of dtoCallItemPermission).GotoUrl
        PageUtility.RedirectToUrl(url)
    End Sub

    Private Sub DisplayUnableToDeleteCall() Implements IViewBaseCallList(Of dtoCallItemPermission).DisplayUnableToDeleteCall
        CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayUnableToDeleteCall." & CallForPaperType.CallForBids.ToString()), Helpers.MessageType.error)
    End Sub

    Private Sub DisplayUnableToDeleteUnknownCall() Implements IViewBaseCallList(Of dtoCallItemPermission).DisplayUnableToDeleteUnknownCall
        CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayUnableToDeleteUnknownCall." & CallForPaperType.CallForBids.ToString()), Helpers.MessageType.alert)
    End Sub

#End Region

    Private Sub RPTcalls_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTcalls.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dto As dtoCallItemPermission = DirectCast(e.Item.DataItem, dtoCallItemPermission)

            If dto.Call.Status = CallForPaperStatus.SubmissionClosed Then
                Dim lockedInfo As Label = e.Item.FindControl("LBlocked")
                lockedInfo.Visible = True
                lockedInfo.ToolTip = Me.Resource.getValue("SubmissionClosed." & CallForPaperType.CallForBids.ToString())
            End If
            Dim oLiteral As Literal = e.Item.FindControl("LTname")
            oLiteral.Text = dto.Call.Name
            If Not String.IsNullOrEmpty(dto.Call.Edition) Then
                oLiteral = e.Item.FindControl("LTedition")
                oLiteral.Visible = True
                oLiteral.Text = dto.Call.Edition
            End If
            Dim oLabel As Label
            If dto.isNewItem Then
                oLabel = e.Item.FindControl("LBnewItem")
                oLabel.Visible = True
                oLabel.ToolTip = Me.Resource.getValue("NewItem." & CallForPaperType.CallForBids.ToString())
            End If
            If dto.isExpiringItem Then
                oLabel = e.Item.FindControl("LBexpiringItem")
                oLabel.Visible = True
                oLabel.ToolTip = Me.Resource.getValue("ExpiringItem." & CallForPaperType.CallForBids.ToString())
            End If
            If dto.AdvacedEvaluation Then
                oLabel = e.Item.FindControl("LBisAdvance")
                oLabel.Visible = True
                Dim tt As String = Me.Resource.getValue("Advance." & CallForPaperType.CallForBids.ToString())
                If (String.IsNullOrEmpty(tt)) Then
                    tt = "Gestione avanzata commissioni"
                End If
                oLabel.ToolTip = tt
            End If


            oLabel = e.Item.FindControl("LBdescriptionswitch")
            oLabel.ToolTip = Me.Resource.getValue("descriptionswitch")
            oLiteral = e.Item.FindControl("LTintroCall")
            Dim oDiv As HtmlControl = e.Item.FindControl("DVsummary")
            If String.IsNullOrEmpty(dto.Call.Summary) Then
                oLiteral.Text = dto.Call.Description
                oDiv.Visible = True
                oLabel.Visible = False
                oDiv = e.Item.FindControl("DVbody")
                oDiv.Visible = False
            Else
                oLiteral.Text = dto.Call.Summary
                oDiv = e.Item.FindControl("DVbody")
                oLiteral = e.Item.FindControl("LTdescriptionCall")
                If String.IsNullOrEmpty(dto.Call.Description) OrElse dto.Call.Description = dto.Call.Summary Then
                    oLabel.Visible = False
                    oDiv.Visible = False
                Else
                    oLiteral.Text = dto.Call.Description
                    oDiv.Visible = True
                End If
            End If


            With dto.Permission
                Dim dialogButton As MyUC.DialogLinkButton = e.Item.FindControl("LNBdelete")
                dialogButton.Visible = .Delete
                If .Delete Then
                    Me.Resource.setLinkButtonToValue(dialogButton, CallForPaperType.CallForBids.ToString(), True, True)

                    dialogButton.CommandArgument = dto.Id.ToString
                    dialogButton.DialogClass = "mandatoryDial"
                End If

                Dim oLinkButton As LinkButton = e.Item.FindControl("LNBvirtualDelete")
                oLinkButton.Visible = .VirtualDelete
                If .VirtualDelete Then
                    Me.Resource.setLinkButtonToValue(oLinkButton, CallForPaperType.CallForBids.ToString(), True, True)
                End If
                oLinkButton = e.Item.FindControl("LNBrecover")
                oLinkButton.Visible = .UnDelete
                If .UnDelete Then
                    Me.Resource.setLinkButtonToValue(oLinkButton, CallForPaperType.CallForBids.ToString(), True, True)
                End If

                Dim oHyperlink As HyperLink = e.Item.FindControl("HYPedit")
                oHyperlink.Visible = .Edit
                If .Edit Then
                    Resource.setHyperLink(oHyperlink, CallForPaperType.CallForBids.ToString(), True, True)
                    oHyperlink.NavigateUrl = BaseUrl & RootObject.EditCallSettings(CallForPaperType.CallForBids, dto.Id, dto.IdCommunity, CurrentView)
                End If

                oHyperlink = e.Item.FindControl("HYPpreview")
                oHyperlink.Visible = .Edit
                If .Edit Then
                    Resource.setHyperLink(oHyperlink, CallForPaperType.CallForBids.ToString(), True, True)
                    oHyperlink.NavigateUrl = BaseUrl & RootObject.PreviewCall(CallForPaperType.CallForBids, dto.Id, dto.IdCommunity, CurrentView)
                End If

                oLinkButton = e.Item.FindControl("LNBcloneCall")
                oLinkButton.Visible = .Edit
                If .Edit Then
                    Me.Resource.setLinkButtonToValue(oLinkButton, CallForPaperType.CallForBids.ToString(), True, True)
                End If
                oHyperlink = e.Item.FindControl("HYPsubmissions")
                oHyperlink.Visible = .ViewSubmissions 'AndAlso Not dto.AdvacedEvaluation

                If .ViewSubmissions Then
                    Resource.setHyperLink(oHyperlink, CallForPaperType.CallForBids.ToString(), True, True)
                    oHyperlink.NavigateUrl = BaseUrl & RootObject.ViewSubmissions(CallForPaperType.CallForBids, dto.Id, CurrentView)
                End If

                If .ViewSubmissions OrElse .Edit Then
                    oHyperlink = e.Item.FindControl("HYPnotificationMail")
                    oHyperlink.Visible = True
                    Resource.setHyperLink(oHyperlink, False, True)
                    oHyperlink.NavigateUrl = BaseUrl & lm.Comol.Core.TemplateMessages.ExternalModuleRootObject.ListAvailableTemplates(ModuleCallForPaper.UniqueCode, PageUtility.GetUrlEncoded(RootObject.ViewCalls(dto.Call.Id, CallForPaperType.CallForBids, PreloadAction, IdCallCommunity, PreLoadedContentView)), IdCallCommunity, ModuleObject.CreateLongObject(dto.Call.Id, ModuleCallForPaper.ObjectType.CallForPaper, dto.IdCommunity, ModuleCallForPaper.UniqueCode))

                End If

                oHyperlink = e.Item.FindControl("HYPcommitteeSettings")
                oHyperlink.Visible = False
                If .ManageComittees Then

                    oHyperlink.Visible = True
                    Resource.setHyperLink(oHyperlink, False, True)

                    If (dto.AdvacedEvaluation) Then
                        oHyperlink.NavigateUrl = BaseUrl & RootObject.AdvStepsEdit(dto.Id) ', dto.IdCommunity, lm.Comol.Modules.CallForPapers.Domain.Evaluation.WizardEvaluationStep.GeneralSettings, CurrentView)
                    Else
                        oHyperlink.NavigateUrl = BaseUrl & RootObject.EditCommiteeByStep(dto.Id, dto.IdCommunity, lm.Comol.Modules.CallForPapers.Domain.Evaluation.WizardEvaluationStep.GeneralSettings, CurrentView)
                    End If
                ElseIf (dto.AdvacedEvaluation) Then

                    oHyperlink.Visible = True
                    Resource.setHyperLink(oHyperlink, False, True)

                    oHyperlink.NavigateUrl = BaseUrl & RootObject.AdvStepsEdit(dto.Id) ', dto.IdCommunity, lm.Comol.Modules.CallForPapers.Domain.Evaluation.WizardEvaluationStep.GeneralSettings, CurrentView)
                End If



                If .ManageEvaluation Then

                    oHyperlink = e.Item.FindControl("HYPevaluationsStatistics")
                    oHyperlink.Visible = Not dto.AdvacedEvaluation
                    Resource.setHyperLink(oHyperlink, False, True)
                    oHyperlink.NavigateUrl = BaseUrl & RootObject.EvaluationsSummary(dto.Id, dto.IdCommunity, CurrentView)
                End If


                If .Evaluate AndAlso Not dto.Call.AdvacedEvaluation Then

                    oHyperlink = e.Item.FindControl("HYPevaluate")
                    oHyperlink.Visible = .Evaluate

                    Resource.setHyperLink(oHyperlink, CallForPaperType.CallForBids.ToString(), True, True)
                    oHyperlink.NavigateUrl = BaseUrl & RootObject.ViewSubmissionsToEvaluate(CallForPaperType.CallForBids, dto.Id, dto.IdCommunity, CurrentView, SubmissionsOrder.ByUser, True)

                    Dim oDVevaluation As HtmlControl = e.Item.FindControl("DVevaluation")
                    oDVevaluation.Visible = True
                    oLabel = e.Item.FindControl("LBevaluation_t")
                    Me.Resource.setLabel(oLabel)
                    oHyperlink = e.Item.FindControl("HYPevaluation")
                    If dto.ToEvaluateCount = 0 Then
                        oHyperlink.Text = Resource.getValue("Evaluated")
                        oHyperlink.ToolTip = Resource.getValue("Evaluated.ToolTip")
                    Else
                        oHyperlink.Text = String.Format(Resource.getValue("Evaluation"), dto.EvaluatedCount, dto.ToEvaluateCount)
                        oHyperlink.ToolTip = String.Format(Resource.getValue("Evaluation.ToolTip"), dto.EvaluatedCount, dto.ToEvaluateCount)
                    End If
                    oHyperlink.NavigateUrl = BaseUrl & RootObject.ViewSubmissionsToEvaluate(CallForPaperType.CallForBids, dto.Id, dto.IdCommunity, CurrentView, SubmissionsOrder.ByUser, True)
                End If
            End With
            oLabel = e.Item.FindControl("LBendDateTime_t")
            Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBendDate")
            If dto.Call.EndDate.HasValue Then
                oLabel.Text = FormatDateTime(dto.Call.EndDate, DateFormat.GeneralDate)
            Else
                oLabel.Text = Resource.getValue("NoEndDate")
            End If

            Dim oDivStatus As HtmlControl = e.Item.FindControl("DVstatus")
            oDivStatus.Visible = (CurrentAction = CallStandardAction.List)
            oLabel = e.Item.FindControl("LBstatus_t")
            Resource.setLabel(oLabel)

            oLiteral = e.Item.FindControl("LTstatus")
            If CurrentAction = CallStandardAction.List Then
                oLiteral = e.Item.FindControl("LTstatus")
                Dim SubmissionOpened As Boolean = (dto.Status = CallStatusForSubmitters.SubmissionOpened AndAlso Me.AllowSubmmissions)
                Dim url As String = "<a href=""{0}"" title=""{1}"" class=""cfpdetails"">{1}</a>"
                Dim translations As New List(Of String)
                If Not SubmissionOpened AndAlso dto.SubmissionsInfo.Count = 0 Then
                    Select Case dto.Call.Status
                        Case CallForPaperStatus.Published
                            oLiteral.Text = Resource.getValue("StatusForSubmissions." & CallForPaperType.CallForBids.ToString() & ".CallForPaperStatus." & dto.Call.Status.ToString)
                        Case CallForPaperStatus.SubmissionClosed, CallForPaperStatus.SubmissionsLimitReached
                            oLiteral.Text = Resource.getValue("StatusForSubmissions.CallForPaperStatus." & dto.Call.Status.ToString)
                        Case CallForPaperStatus.SubmissionOpened
                            oLiteral.Text = Resource.getValue("StatusForSubmissions.CallForPaperStatus." & CallForPaperStatus.SubmissionClosed.ToString)
                        Case Else
                            oLiteral.Text = "    //      "
                    End Select
                ElseIf Not SubmissionOpened Then
                    Dim userMessage As String = ""
                    If dto.SubmissionsInfo.Where(Function(s) s.Status = SubmissionStatus.draft).Any AndAlso dto.Call.Status <> CallForPaperStatus.SubmissionClosed AndAlso dto.Call.Status <> CallForPaperStatus.SubmissionsLimitReached Then
                        Dim key As String = "Closed." & CallForPaperType.CallForBids.ToString() & ".SubmissionStatus." & SubmissionStatus.draft.ToString & "."
                        If dto.SubmissionsInfo.Where(Function(s) s.Status = SubmissionStatus.draft).Count > 1 Then
                            key &= "n"
                        Else
                            key &= "1"
                        End If
                        userMessage = Resource.getValue(key)
                    ElseIf dto.Call.Status <> CallForPaperStatus.SubmissionClosed OrElse dto.Call.Status <> CallForPaperStatus.SubmissionsLimitReached Then
                        userMessage = Resource.getValue("StatusForSubmissions.CallForPaperStatus." & dto.Call.Status.ToString)
                    End If
                    Dim addedUserMessage As Boolean = False
                    For Each submission As dtoSubmissionDisplayInfo In dto.SubmissionsInfo
                        If submission.Status = SubmissionStatus.draft Then
                            If submission.ExtensionDate <= DateTime.Now AndAlso submission.isOriginal Then
                                translations.Add(String.Format(url, BaseUrl & RootObject.ContinueSubmission(CallForPaperType.CallForBids, dto.Id, dto.Call.IsPublic, submission.Id, False, CurrentView, IdCallCommunity), Me.Resource.getValue("SubmissionStatus." & CallForPaperType.CallForBids.ToString() & "." & submission.Status.ToString)))
                                'translations.Add(String.Format(url, BaseUrl & RootObject.SubmitToCall(CallForPaperType.CallForBids, dto.Id, dto.Call.IsPublic, submission.Id, submission.GetIdLastActiveRevision, False, CurrentView, IdCallCommunity), Me.Resource.getValue("SubmissionStatus." & CallForPaperType.CallForBids.ToString() & "." & submission.Status.ToString))))
                            Else
                                If Not addedUserMessage AndAlso Not String.IsNullOrWhiteSpace(userMessage) Then
                                    translations.Add(userMessage)
                                    addedUserMessage = True
                                End If
                            End If
                        ElseIf submission.HasWorkingRevision Then
                            Dim dtoRevision As dtoRevision = submission.GetWorkingRevision
                            If Not IsNothing(dtoRevision) Then
                                translations.Add(String.Format(url, BaseUrl & RootObject.UserReviewCall(CallForPaperType.CallForBids, dto.Id, submission.Id, submission.GetIdWorkingRevision, CurrentView, IdCallCommunity), Me.Resource.getValue("Revision." & CallForPaperType.CallForBids.ToString() & "." & dtoRevision.Status.ToString)))
                            Else
                                translations.Add(String.Format(url, BaseUrl & RootObject.ViewSubmission(CallForPaperType.CallForBids, dto.Id, submission.Id, False, CurrentView, IdCallCommunity, 0), Me.Resource.getValue("SubmissionStatus." & CallForPaperType.CallForBids.ToString() & "." & submission.Status.ToString)))
                            End If
                        Else
                            translations.Add(String.Format(url, BaseUrl & RootObject.ViewSubmission(CallForPaperType.CallForBids, dto.Id, submission.Id, False, CurrentView, IdCallCommunity, 0), Me.Resource.getValue("SubmissionStatus." & CallForPaperType.CallForBids.ToString() & "." & submission.Status.ToString)))
                        End If
                    Next
                Else
                    For Each submission As dtoSubmissionDisplayInfo In dto.SubmissionsInfo
                        Dim dtoRevision As dtoRevision = submission.GetWorkingRevision
                        If Not IsNothing(dtoRevision) Then
                            translations.Add(String.Format(url, BaseUrl & RootObject.UserReviewCall(CallForPaperType.CallForBids, dto.Id, submission.Id, submission.GetIdWorkingRevision, CurrentView, IdCallCommunity), Me.Resource.getValue("Revision." & CallForPaperType.CallForBids.ToString() & "." & dtoRevision.Status.ToString)))
                        Else
                            If submission.Status < SubmissionStatus.submitted Then
                                translations.Add(String.Format(url, BaseUrl & RootObject.ContinueSubmission(CallForPaperType.CallForBids, dto.Id, Me.isUtenteAnonimo, submission.Id, False, CurrentView, IdCallCommunity), Me.Resource.getValue("SubmissionStatus." & CallForPaperType.CallForBids.ToString() & "." & submission.Status.ToString)))
                            Else
                                translations.Add(String.Format(url, BaseUrl & RootObject.ViewSubmission(CallForPaperType.CallForBids, dto.Id, submission.Id, False, CurrentView, IdCallCommunity, 0), Me.Resource.getValue("SubmissionStatus." & CallForPaperType.CallForBids.ToString() & "." & submission.Status.ToString)))
                            End If
                        End If
                    Next
                    If dto.AllowSubmissionAs.Count > 0 Then
                        Dim status As String = Me.Resource.getValue("SubmissionStatus." & CallForPaperType.CallForBids.ToString() & ".AddAlso")
                        status = String.Format(status, dto.AllowSubmissionAs(0).Name)
                        translations.Add(String.Format(url, BaseUrl & RootObject.SubmitToCallBySubmitterType(CallForPaperType.CallForBids, dto.Id, Me.isUtenteAnonimo, 0, dto.AllowSubmissionAs(0).Id, False, CurrentView, IdCallCommunity), status, status))
                    ElseIf dto.SubmissionsInfo.Count = 0 Then
                        translations.Add(String.Format(url, BaseUrl & RootObject.ContinueSubmission(CallForPaperType.CallForBids, dto.Id, Me.isUtenteAnonimo, 0, False, CurrentView, IdCallCommunity), Me.Resource.getValue("SubmissionStatus." & CallForPaperType.CallForBids.ToString() & "." & SubmissionStatus.none.ToString), IdCallCommunity))
                    End If
                End If
                If translations.Count = 1 Then
                    oLiteral.Text = translations(0)
                ElseIf translations.Count > 1 Then
                    oLiteral.Text = "<ul>"
                    For Each transaltion As String In translations
                        oLiteral.Text &= "<li class=""cfpdetails"">" & transaltion & "</li>"
                    Next
                    oLiteral.Text &= "</ul>"
                End If
            Else
                oLiteral.Text = Me.Resource.getValue("CallForPaperStatus." & CallForPaperType.CallForBids.ToString() & "." & dto.Call.Status.ToString)
            End If
        End If
    End Sub

    Private Sub RPTcalls_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTcalls.ItemCommand
        Dim idCall As Long = 0
        If IsNumeric(e.CommandArgument) Then
            idCall = CLng(e.CommandArgument)
        End If
        CTRLmessages.Visible = False
        Select Case e.CommandName
            Case "virtualdelete"
                Me.CurrentPresenter.VirtualDelete(idCall)
            Case "recover"
                Me.CurrentPresenter.UnDelete(idCall)
            Case "copy"
                Dim basePath As String = ""
                CurrentPresenter.CloneCall(idCall, Resource.getValue("CopyCallPrefix"), PageUtility.GetRepositoryDiskPath, PageUtility.GetRepositoryThumbnailDiskPath)
            Case Else
                Me.CurrentPresenter.LoadCalls(Me.Pager.PageIndex, Me.Pager.PageSize)
        End Select
    End Sub

    Private Sub PGgridBottom_OnPageSelected() Handles PGgridBottom.OnPageSelected
        CTRLmessages.Visible = False
        Me.CurrentPresenter.LoadCalls(Me.Pager.PageIndex, Me.Pager.PageSize)
    End Sub
    Private Sub PGgridTop_OnPageSelected() Handles PGgridTop.OnPageSelected
        CTRLmessages.Visible = False
        Me.CurrentPresenter.LoadCalls(Me.Pager.PageIndex, Me.Pager.PageSize)
    End Sub
    Private Sub RBLvisibility_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RBLvisibility.SelectedIndexChanged
        CTRLmessages.Visible = False
        Me.CurrentPresenter.LoadCalls(0, Me.Pager.PageSize)
    End Sub
    Private Sub DLGremoveItem_ButtonPressed(dialogResult As Integer, commandArgument As String, commandName As String) Handles DLGremoveItem.ButtonPressed
        Dim idCall As Long = 0
        If IsNumeric(commandArgument) Then
            idCall = CLng(commandArgument)
        End If
        CTRLmessages.Visible = False
        If dialogResult = -1 Then
            Select Case commandName
                Case "confirmDelete"
                    CurrentPresenter.Delete(idCall, PageUtility.GetRepositoryDiskPath, PageUtility.GetRepositoryThumbnailDiskPath)
                Case Else
                    Me.CurrentPresenter.LoadCalls(Me.Pager.PageIndex, Me.Pager.PageSize)
            End Select
        End If

    End Sub
    Private Sub ListCalls_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Me.Master.ShowDocType = True
    End Sub
End Class