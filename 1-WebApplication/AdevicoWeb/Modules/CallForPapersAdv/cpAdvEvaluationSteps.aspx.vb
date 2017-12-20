Imports lm.Comol.Modules.CallForPapers.Advanced
Imports lm.Comol.Modules.CallForPapers.Advanced.dto
Imports lm.Comol.Modules.CallForPapers.Advanced.Presentation
Imports lm.Comol.Modules.CallForPapers.Advanced.Presentation.iView
Imports lm.Comol.Modules.CallForPapers.Domain


Public Class cpAdvEvaluationSteps
    Inherits PageBase
    Implements lm.Comol.Modules.CallForPapers.Advanced.Presentation.iView.iViewAdvCallSteps

#Region "Context"
    Private _Presenter As AdvCallStepsPresenter
    Private ReadOnly Property CurrentPresenter() As AdvCallStepsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New AdvCallStepsPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "PageBase"
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides Sub BindDati()
        If Not Page.IsPostBack Then

            CallTrapHelper.SendTrap(
                lm.Comol.Modules.CallForPapers.Trap.CallTrapId.StepView,
                Me.GetCallId(),
                lm.Comol.Modules.CallForPapers.Trap.CallObjectId.CallForPeaper,
                "")

            CurrentPresenter.InitView(GetCallId())
        End If

    End Sub

    Public Overrides Sub BindNoPermessi()
        CallTrapHelper.SendTrap(
                lm.Comol.Modules.CallForPapers.Trap.CallTrapId.StepView,
                Me.GetCallId(),
                lm.Comol.Modules.CallForPapers.Trap.CallObjectId.CallForPeaper,
                "NoPermission")
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()

    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        HypBackManage.NavigateUrl = BaseUrl & RootObject.ViewCalls(CallForPaperType.CallForBids, CallStandardAction.Manage, ComunitaCorrenteID, CallStatusForSubmitters.None)
        HypBackManage.NavigateUrl = BaseUrl & RootObject.ViewCalls(CallForPaperType.CallForBids, CallStandardAction.List, ComunitaCorrenteID, CallStatusForSubmitters.None)

        HYPshowSubmission.NavigateUrl = BaseUrl & RootObject.ViewSubmissions(CallForPaperType.CallForBids, Me.GetCallId(), CallStatusForSubmitters.None)


    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get

        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub




    Public Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements lm.Comol.Modules.CallForPapers.Presentation.IViewBase.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType.NoPermission, , lm.ActionDataContract.InteractionType.UserWithLearningObject)
        Me.Master.ShowNoPermission = True
    End Sub

    Public Sub DisplaySessionTimeout() Implements lm.Comol.Modules.CallForPapers.Presentation.IViewBase.DisplaySessionTimeout
        CallTrapHelper.SendTrap(
               lm.Comol.Modules.CallForPapers.Trap.CallTrapId.StepView,
               Me.GetCallId(),
               lm.Comol.Modules.CallForPapers.Trap.CallObjectId.CallForPeaper,
               "SessionTimeOut")
    End Sub

#Region "BindSteps"

    Private _IsManager As Boolean = False

    Public Sub Initialize(Steps As dto.dtoStepsEdit, IsManager As Boolean, CanShowSubmission As Boolean) Implements iView.iViewAdvCallSteps.Initialize

        _IsManager = IsManager

        LkbAddStep.Visible = IsManager
        BindValidation(Steps.ValidationStep)

        BindEconomics(Steps.EconomicStep)

        If (Not IsNothing(Steps.CustomSteps) AndAlso Steps.CustomSteps.Any) Then
            Me.RPTsteps.Visible = True
            Me.RPTsteps.DataSource = Steps.CustomSteps
            RPTsteps.DataBind()
        Else
            Me.RPTsteps.Visible = False
        End If

        HYPshowSubmission.Visible = CanShowSubmission

        LTaddStep_t.Visible = LkbAddStep.Visible
        LKBconfirmReorder.Visible = LkbAddStep.Visible
        HypBackManage.Visible = Not _IsManager

        If _IsManager Then
            HypBackList.NavigateUrl = BaseUrl & RootObject.ViewCalls(CallForPaperType.CallForBids, CallStandardAction.Manage, MyBase.ComunitaCorrenteID, CallStatusForSubmitters.SubmissionClosed)
        Else
            HypBackList.NavigateUrl = BaseUrl & RootObject.ViewCalls(CallForPaperType.CallForBids, CallStandardAction.List, MyBase.ComunitaCorrenteID, CallStatusForSubmitters.SubmissionClosed)
        End If


    End Sub


    Private CanAddCommission = False


    Private Sub RPTsteps_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTsteps.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim valStep As lm.Comol.Modules.CallForPapers.Advanced.dto.dtoAdvStepContainer = e.Item.DataItem
            If Not IsNothing(valStep) Then

                CanAddCommission = (valStep.Status < StepStatus.Closed)

                Dim CanShowCommission As Boolean = _IsManager '_
                'OrElse (valStep.StepPermission And GenericStepPermission.Member = GenericStepPermission.Member) _
                'OrElse (valStep.StepPermission And GenericStepPermission.Secretary = GenericStepPermission.Secretary) _
                'OrElse (valStep.StepPermission And GenericStepPermission.President = GenericStepPermission.President) _
                'OrElse (valStep.StepPermission And GenericStepPermission.MainPresident = GenericStepPermission.MainPresident) _
                'OrElse ((valStep.StepPermission And GenericStepPermission.Submitter = GenericStepPermission.Submitter) And valStep.Status = StepStatus.Closed)

                Dim CanDrag As Boolean = CanAddCommission AndAlso _IsManager
                '(_IsManager _
                'OrElse (valStep.StepPermission And GenericStepPermission.MainPresident = GenericStepPermission.MainPresident) _
                'OrElse (valStep.StepPermission And GenericStepPermission.Secretary = GenericStepPermission.Secretary))

                Dim LTdrag As Literal = e.Item.FindControl("LTdrag")
                If Not IsNothing(LTdrag) Then

                    If Not valStep.Status = StepStatus.Draft _
                        AndAlso valStep.Commissions.All(Function(cm) cm.Status = CommissionStatus.Draft) _
                        AndAlso CanDrag Then

                        LTdrag.Visible = True

                    Else
                        LTdrag.Visible = False
                    End If
                End If

                Dim DVstepContainer As HtmlControl = e.Item.FindControl("DVstepContainer")
                If Not IsNothing(DVstepContainer) Then
                    Dim css As String = DVstepContainer.Attributes("class")


                    If (String.IsNullOrEmpty(css)) Then
                        css = ""
                    End If
                    css = String.Format("{0} Step_{1}", css, valStep.Id)
                    DVstepContainer.Attributes("class") = css
                End If


                Dim LBLstepName As Label = e.Item.FindControl("LBLstepName")
                If Not IsNothing(LBLstepName) Then
                    LBLstepName.Text = String.Format("Step {0}: {1}", valStep.Order, valStep.Name)
                End If

                Dim LTstepStatus As Literal = e.Item.FindControl("LTstepStatus")
                If Not IsNothing(LTstepStatus) Then
                    Try
                        LTstepStatus.Text = String.Format(LTstepStatus.Text, GetStepStatusIcon(valStep.Status), GetStepStatusAlt(valStep.Status))
                    Catch ex As Exception
                        LTstepStatus.Text = valStep.Status.ToString()
                    End Try
                End If




                Dim HYPstepstatus As HyperLink = e.Item.FindControl("HYPstepstatus")
                If Not IsNothing(HYPstepstatus) Then
                    HYPstepstatus.Text = "Riepilogo valutazioni sottomissioni"
                    'If Not IsNothing(valStep.Commissions) AndAlso (valStep.Commissions.Any) Then
                    '    HYPstepstatus.NavigateUrl = BaseUrl & RootObject.AdvStepSummary(valStep.Id, valStep.Commissions.FirstOrDefault().Id)
                    'Else
                    HYPstepstatus.NavigateUrl = BaseUrl & RootObject.AdvStepSummary(valStep.Id, 0)
                    'End If

                    HYPstepstatus.Visible = CanShowCommission AndAlso valStep.Commissions.Any(Function(cm) cm.Status > CommissionStatus.Draft)

                End If



                    Dim RPTcommiss As Repeater = e.Item.FindControl("RPTcommiss")
                    If Not IsNothing(RPTcommiss) Then
                        AddHandler RPTcommiss.ItemDataBound, AddressOf RPTcommis_ItemDataBound
                        'AddHandler RPTcommiss.ItemCommand, AddressOf RPTcommis_ItemCommand
                        RPTcommiss.DataSource = valStep.Commissions
                        RPTcommiss.DataBind()
                    End If

                End If


            End If
    End Sub

    Private Sub RPTcommis_ItemDataBound(sender As Object, e As RepeaterItemEventArgs)
        If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim valCommis As lm.Comol.Modules.CallForPapers.Advanced.dto.dtoAdvCommissionContainer = e.Item.DataItem

            If Not IsNothing(valCommis) Then


                If CanAddCommission AndAlso valCommis.IsMaster Then
                    If valCommis.Status = CommissionStatus.Closed Then
                        CanAddCommission = False
                    End If
                End If


                Dim PHcommMaster As PlaceHolder = e.Item.FindControl("PHcommMaster")
                If Not IsNothing(PHcommMaster) Then
                    PHcommMaster.Visible = valCommis.IsMaster
                End If

                Dim LBLcomStepName As Label = e.Item.FindControl("LBLcomStepName")
                If Not IsNothing(LBLcomStepName) Then
                    LBLcomStepName.Text = valCommis.Name
                End If

                'LTcomStepDesciption
                Dim LTcomStepDesciption As Literal = e.Item.FindControl("LTcomStepDesciption")
                If Not IsNothing(LTcomStepDesciption) Then
                    LTcomStepDesciption.Text = valCommis.Description
                End If


                Dim LTcomStepStatus As Literal = e.Item.FindControl("LTcomStepStatus")
                If Not IsNothing(LTcomStepStatus) Then
                    Try
                        LTcomStepStatus.Text = String.Format(LTcomStepStatus.Text, GetCommStatusIcon(valCommis.Status), GetCommStatusAlt(valCommis.Status))
                    Catch ex As Exception
                        LTcomStepStatus.Text = valCommis.Status.ToString()
                    End Try
                End If

                Dim showCommiss As Boolean = _IsManager OrElse valCommis.HasCommissionPermission(CommissionPermission.View)


                Dim hyp As HyperLink
                hyp = e.Item.FindControl("HYPcomStepEditMember")
                If Not IsNothing(hyp) Then
                    hyp.NavigateUrl = BaseUrl & RootObject.AdvCommissionEdit(Me.GetCallId, valCommis.Id, CommiteeEditPage.Members)
                    hyp.Visible = showCommiss
                End If

                hyp = e.Item.FindControl("HYPcomStepEditRules")
                If Not IsNothing(hyp) Then
                    hyp.NavigateUrl = BaseUrl & RootObject.AdvCommissionEdit(Me.GetCallId, valCommis.Id, CommiteeEditPage.Criterion)
                    hyp.Visible = showCommiss
                End If

                hyp = e.Item.FindControl("HYPcomStepEditRules")
                If Not IsNothing(hyp) Then
                    hyp.NavigateUrl = BaseUrl & RootObject.AdvCommissionEdit(Me.GetCallId, valCommis.Id, CommiteeEditPage.Criterion)
                    hyp.Visible = showCommiss
                End If

                hyp = e.Item.FindControl("HypcomSubmission")
                If Not IsNothing(hyp) Then

                    hyp.NavigateUrl = BaseUrl & RootObject.ViewSubmissionsToEvaluateAdv(
                    CallForPaperType.CallForBids,
                    Me.GetCallId,
                    MyBase.ComunitaCorrenteID,
                    valCommis.Id,
                    CallStatusForSubmitters.None,
                    SubmissionsOrder.None,
                    True,
                    0,
                    Evaluation.EvaluationFilterStatus.Evaluating,
                    "")
                    hyp.Visible = showCommiss AndAlso Not (valCommis.Status = CommissionStatus.Draft)
                End If

                hyp = e.Item.FindControl("HYPshowALLSubmission")
                If Not IsNothing(hyp) Then

                    hyp.NavigateUrl = BaseUrl &
                    lm.Comol.Modules.CallForPapers.Domain.RootObject.ViewSubmissions(
                        lm.Comol.Modules.CallForPapers.Domain.CallForPaperType.CallForBids,
                        Me.GetCallId(),
                        lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters.None) &
                        "&cmmId=" & valCommis.Id
                    hyp.Visible = showCommiss
                End If

                'lkb = e.Item.FindControl("LKBcomStepEditMember")
                'If Not IsNothing(lkb) Then
                '    lkb.CommandArgument = valCommis.Id.ToString()
                '    lkb.CommandName = "EditMember"

                '    lkb.ToolTip = "*Edit Member"
                'End If

                'lkb = e.Item.FindControl("LKBcomStepEditRules")
                'If Not IsNothing(lkb) Then
                '    lkb.CommandArgument = valCommis.Id.ToString()
                '    lkb.CommandName = "EditRules"

                '    lkb.ToolTip = "*Edit Rules"
                'End If

                Dim lkb As LinkButton
                lkb = e.Item.FindControl("LKBcomStepDelCom")
                If Not IsNothing(lkb) Then
                    lkb.CommandArgument = valCommis.Id.ToString()
                    lkb.CommandName = "DelCommission"

                    lkb.ToolTip = "Cancella commissione"
                    ' AddHandler lkb.Click, AddressOf DeleteCommission
                    lkb.Visible = _IsManager AndAlso (valCommis.Status = CommissionStatus.Draft)
                End If

                lkb = e.Item.FindControl("LKBcomStepAddCom")
                If Not IsNothing(lkb) Then
                    lkb.CommandArgument = valCommis.Id.ToString()
                    lkb.CommandName = "AddCommission"
                    lkb.ToolTip = "Nuova commissione"
                    lkb.Visible = CanAddCommission AndAlso _IsManager
                End If



            End If

            End If
    End Sub

    Public Sub RPTcommis_ItemCommand(source As Object, e As RepeaterCommandEventArgs)
        Dim commId As Int64 = System.Convert.ToInt64(e.CommandArgument)

        If (commId > 0) Then

            Select Case e.CommandName
                Case "DelCommission"
                    CurrentPresenter.DelCommission(Me.GetCallId, commId)
                Case "AddCommission"
                    CurrentPresenter.AddCommission(Me.GetCallId, commId)
            End Select



        End If


    End Sub

    Private Sub BindValidation(ByVal validation As dtoAdvStepContainer)

        If IsNothing(validation) Then
            validation = New dtoAdvStepContainer()
            'validation.Commissions = New List(Of dtoAdvCommissionContainer)
        End If

        LBLstepValName.Text = "Step 0: validazione"

        HYPstepValstatus.Text = "Riepilogo valutazioni sottomissioni"


        If Not IsNothing(validation.Commissions) AndAlso validation.Commissions.Any() Then
            Dim commId As Int64 = validation.Commissions.FirstOrDefault().Id



            HYPstepValstatus.Visible = _IsManager
            ' _IsManager _
            'OrElse (validation.StepPermission And GenericStepPermission.Member = GenericStepPermission.Member) _
            'OrElse (validation.StepPermission And GenericStepPermission.Secretary = GenericStepPermission.Secretary) _
            'OrElse (validation.StepPermission And GenericStepPermission.President = GenericStepPermission.President) _
            'OrElse (validation.StepPermission And GenericStepPermission.MainPresident = GenericStepPermission.MainPresident) _
            'OrElse ((validation.StepPermission And GenericStepPermission.Submitter = GenericStepPermission.Submitter) And validation.Status = StepStatus.Closed)

            If HYPstepValstatus.Visible Then
                HYPstepValstatus.NavigateUrl = BaseUrl & RootObject.AdvStepSummary(validation.Id, commId)
            End If

            Dim IsSimpleUser As Boolean = (validation.StepPermission = GenericStepPermission.none) '_
            'OrElse (validation.StepPermission And GenericStepPermission.Submitter = GenericStepPermission.Submitter)

            If Not IsSimpleUser Then
                HYPcomValEditMember.NavigateUrl = BaseUrl & RootObject.AdvCommissionEdit(validation.CallId, commId, CommiteeEditPage.Members)
                HYPcomValEditRules.NavigateUrl = BaseUrl & RootObject.AdvCommissionEdit(validation.CallId, commId, CommiteeEditPage.Criterion)

                If (validation.Status = StepStatus.Draft) Then
                    HYPcomValShowSubmission.Visible = False
                    HYPcomValShowSubmission.Enabled = False
                Else
                    HYPcomValShowSubmission.NavigateUrl = BaseUrl & RootObject.ViewSubmissionsToEvaluateAdv(
                    CallForPaperType.CallForBids,
                    validation.CallId,
                    MyBase.ComunitaCorrenteID,
                    commId,
                    CallStatusForSubmitters.None,
                    SubmissionsOrder.None,
                    True,
                    0,
                    Evaluation.EvaluationFilterStatus.Evaluating,
                    "")


                End If


                Dim commission As dtoAdvCommissionContainer = validation.Commissions.FirstOrDefault()

                If Not IsNothing(commission) AndAlso (_IsManager OrElse commission.HasCommissionPermission(CommissionPermission.View)) Then

                    HYPcomAllShowSubmission.NavigateUrl = BaseUrl &
                   lm.Comol.Modules.CallForPapers.Domain.RootObject.ViewSubmissions(
                       lm.Comol.Modules.CallForPapers.Domain.CallForPaperType.CallForBids,
                       Me.GetCallId(),
                       lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters.None) &
                       "&cmmId=" & commId

                Else
                    HYPcomAllShowSubmission.Visible = False
                End If


                'HYPcomAllShowSubmission.NavigateUrl = BaseUrl &
                '   lm.Comol.Modules.CallForPapers.Domain.RootObject.ViewSubmissions(
                '       lm.Comol.Modules.CallForPapers.Domain.CallForPaperType.CallForBids,
                '       Me.GetCallId(),
                '       lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters.None) &
                '       "&cmmId=" & commId





            Else
                HYPcomValEditMember.Visible = False
                HYPcomValEditRules.Visible = False
                HYPcomValShowSubmission.Visible = False
                HYPcomAllShowSubmission.Visible = False
            End If

        End If
        Try
            LTstepValStatus.Text = String.Format(LTstepValStatus.Text, GetStepStatusIcon(validation.Status), GetStepStatusIcon(validation.Status))
        Catch ex As Exception
            LTstepValStatus.Text = validation.Status.ToString()
        End Try

        If (Not IsNothing(validation.Commissions) AndAlso validation.Commissions.Any()) Then

            Dim commission As dtoAdvCommissionContainer = validation.Commissions.FirstOrDefault()

            Try
                LBLcomValName.Text = commission.Name
                LTcomValDesciption.Text = commission.Description

                Dim comStatus As CommissionStatus = commission.Status



                LTcomValStatus.Text = String.Format(LTcomValStatus.Text, GetCommStatusIcon(commission.Status), GetCommStatusAlt(commission.Status))
            Catch ex As Exception
                LTcomValStatus.Text = validation.Status.ToString()
            End Try

        End If

    End Sub

    Private Sub BindEconomics(ByVal economics As dtoAdvStepContainer)

        If IsNothing(economics) OrElse economics.Id <= 0 Then

            DVecoContainer.Visible = False

        Else
            DVecoContainer.Visible = True

            LBLstepEcoName.Text = "Ultimo step: economico"

            If Not IsNothing(economics.Commissions) AndAlso economics.Commissions.Any() Then
                Dim commId As Int64 = economics.Commissions.FirstOrDefault().Id

                Dim CanShowCommission = _IsManager _
                OrElse (economics.StepPermission And GenericStepPermission.Member = GenericStepPermission.Member) _
                OrElse (economics.StepPermission And GenericStepPermission.Secretary = GenericStepPermission.Secretary) _
                OrElse (economics.StepPermission And GenericStepPermission.President = GenericStepPermission.President)
                'OrElse (economics.StepPermission And GenericStepPermission.MainPresident = GenericStepPermission.MainPresident) _
                'OrElse ((economics.StepPermission And GenericStepPermission.Submitter = GenericStepPermission.Submitter) And economics.Status = StepStatus.Closed)

                If (CanShowCommission) Then
                    HYPcomEcoEditMember.Visible = True
                    HYPEcoEvaluate.Visible = True
                    HYPcomEcoEditMember.NavigateUrl = BaseUrl & RootObject.AdvCommissionEdit(economics.CallId, commId, CommiteeEditPage.Members)
                    'HYPcomEcoEditRules.NavigateUrl = BaseUrl & RootObject.AdvCommissionEdit(economics.CallId, commId, CommiteeEditPage.Criterion)
                    If Not IsNothing(economics.Commissions) AndAlso economics.Commissions.Any() Then
                        HYPEcoEvaluate.NavigateUrl = BaseUrl & RootObject.EcoSummaries(economics.Commissions.FirstOrDefault().Id)
                    End If
                Else
                    HYPcomEcoEditMember.Visible = False
                    HYPEcoEvaluate.Visible = False
                End If
            End If

            ' oHyperlink.NavigateUrl = BaseUrl & RootObject.StartNewSubmission(CallForPaperType.CallForBids, dto.Id, True, True, CallStatusForSubmitters.SubmissionOpened, IdCallCommunity)
            Try
                LTstepEcoStatus.Text = String.Format(LTstepEcoStatus.Text, GetStepStatusIcon(economics.Status), GetStepStatusAlt(economics.Status))
            Catch ex As Exception
                LTstepEcoStatus.Text = economics.Status.ToString()
            End Try

            If (Not IsNothing(economics.Commissions) AndAlso economics.Commissions.Any()) Then

                Dim commission As dtoAdvCommissionContainer = economics.Commissions.FirstOrDefault()

                If LkbAddStep.Visible Then
                    LkbAddStep.Visible = (commission.Status = CommissionStatus.Draft)
                End If
                LBLcomEcoName.Text = commission.Name
                LTcomEcoDesciption.Text = commission.Description

                Try
                    LTcomEcostatus.Text = String.Format(LTcomEcostatus.Text, GetCommStatusIcon(commission.Status), GetCommStatusAlt(commission.Status))
                Catch ex As Exception
                    LTcomEcostatus.Text = commission.Status.ToString()
                End Try

            End If

        End If
    End Sub



    Private Function GetStepStatusIcon(ByVal status As StepStatus) As String
        Dim Icon As String = ""

        Select Case status
            Case StepStatus.Draft
                Icon = "statuslight gray"
            Case StepStatus.Started
                Icon = "statuslight yellow"
            Case StepStatus.Locked
                Icon = "statuslight red"
            Case StepStatus.Closed
                Icon = "statuslight green"
        End Select

        Return Icon
    End Function

    Private Function GetStepStatusAlt(ByVal status As StepStatus) As String

        'return Resource.getValue(string.format("StatusText.{0}", status.ToString()))

        Dim text As String = ""

        Select Case status
            Case StepStatus.Draft
                text = "Bozza"
            Case StepStatus.Started
                text = "In corso"
            Case StepStatus.Locked
                text = "Bloccato" ''ToDo: togliere
            Case StepStatus.Closed
                text = "Chiuso"
        End Select

        If String.IsNullOrEmpty(text) Then
            text = status.ToString()
        End If

        Return text
    End Function

    Private Function GetCommStatusIcon(ByVal status As CommissionStatus) As String
        Dim Icon As String = ""

        Select Case status
            Case StepStatus.Draft
                Icon = "statuslight gray"
            Case StepStatus.Open
                Icon = "statuslight gray"
            Case StepStatus.Started
                Icon = "statuslight yellow"
            Case StepStatus.Locked
                Icon = "statuslight red"
            Case StepStatus.Closed
                Icon = "statuslight green"

        End Select

        Return Icon
    End Function

    Private Function GetCommStatusAlt(ByVal status As CommissionStatus) As String

        'return Resource.getValue(string.format("StatusText.{0}", status.ToString()))

        Dim text As String = ""

        Select Case status
            Case StepStatus.Draft
                text = "Bozza"
            Case StepStatus.Open
                text = "Aperta - Solo visualizzazione"
            Case StepStatus.Started
                text = "In corso"
            Case StepStatus.Locked
                text = "Bloccato"
            Case StepStatus.Closed
                text = "Chiuso"
        End Select

        If String.IsNullOrEmpty(text) Then
            text = status.ToString()
        End If

        Return text
    End Function

    Private Function GetCallId() As Int64

        Dim cId As Int64 = 0

        If (String.IsNullOrEmpty(Me.ViewState("CallId"))) Then

            Try
                cId = System.Convert.ToInt64(Request.QueryString("cId"))

            Catch ex As Exception

            End Try

            Me.ViewState("CallId") = cId
        Else
            Try
                cId = System.Convert.ToInt64(Me.ViewState("CallId"))
            Catch ex As Exception
            End Try
        End If
        Return cId
    End Function

    Public Sub ShowNoCall() Implements iViewAdvCallSteps.ShowNoCall
        'Throw New NotImplementedException()
    End Sub

    Private Sub LkbAddStep_Click(sender As Object, e As EventArgs) Handles LkbAddStep.Click

        CallTrapHelper.SendTrap(
                lm.Comol.Modules.CallForPapers.Trap.CallTrapId.SteppAdd,
                Me.GetCallId(),
                lm.Comol.Modules.CallForPapers.Trap.CallObjectId.CallForPeaper,
                "")

        Me.CurrentPresenter.AddStep(Me.GetCallId)
    End Sub

    'Private Sub RPTsteps_ItemCreated(sender As Object, e As RepeaterItemEventArgs) Handles RPTsteps.ItemCreated

    '    If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then

    '        Dim valStep As lm.Comol.Modules.CallForPapers.Advanced.dto.dtoAdvStepContainer = e.Item.DataItem
    '        If Not IsNothing(valStep) Then
    '            Dim RPTcommiss As Repeater = e.Item.FindControl("RPTcommiss")
    '            If Not IsNothing(RPTcommiss) Then
    '                AddHandler RPTcommiss.ItemCommand, AddressOf RPTcommis_ItemCommand
    '            End If
    '        End If
    '    End If
    'End Sub

    'Private Sub RPTsteps_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles RPTsteps.ItemCommand

    '    Dim commId As Int64 = System.Convert.ToInt64(e.CommandArgument)

    '    If (commId > 0) Then

    '        Select Case e.CommandName
    '            Case "DelCommission"
    '                CurrentPresenter.DelCommission(Me.GetCallId, commId)
    '                'Case "AddCommission"
    '                '    CurrentPresenter.AddCommission(Me.GetCallId, commId)
    '        End Select



    '    End If
    'End Sub


    Public Sub UpdateOrder()

        Dim NewOrderList As New List(Of KeyValuePair(Of Int64, Integer))()

        If Not String.IsNullOrEmpty(LTstepStatusOrder.Value) Then
            Dim Order As String() = LTstepStatusOrder.Value.Split(",")

            Dim i As Integer = 1

            For Each ord As String In Order.ToList()
                Dim Id As Int64 = System.Convert.ToInt64(ord)

                If Id > 0 Then
                    NewOrderList.Add(New KeyValuePair(Of Long, Integer)(Id, i))
                    i += 1
                End If
            Next

            Me.CurrentPresenter.Reorder(Me.GetCallId, NewOrderList)

        End If
    End Sub

    Private Sub LKBconfirmReorder_Click(sender As Object, e As EventArgs) Handles LKBconfirmReorder.Click
        UpdateOrder()
    End Sub


#End Region

#Region "Action"

    Private Sub SendUserAction(
                              idCommunity As Integer,
                              idModule As Integer,
                              action As lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType,
                              objectType As lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ObjectType,
                              IdObject As String) _
                Implements iViewAdvCallSteps.SendUserAction

        PageUtility.AddActionToModule(idCommunity,
                                      idModule,
                                      action,
                                      PageUtility.CreateObjectsList(
                                        idModule,
                                        objectType,
                                        IdObject),
                                      lm.ActionDataContract.InteractionType.UserWithLearningObject)

    End Sub

#End Region

End Class