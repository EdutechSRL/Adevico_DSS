Imports lm.Comol.Modules.CallForPapers.Advanced
Imports lm.Comol.Modules.CallForPapers.Advanced.dto
Imports lm.Comol.Modules.CallForPapers.Advanced.Presentation
Imports lm.Comol.Modules.CallForPapers.Advanced.Presentation.iView
Imports lm.Comol.Modules.CallForPapers.Domain
Imports lm.Comol.Modules.CallForPapers.Presentation

Public Class cpAdvStepSummary
    Inherits PageBase
    Implements lm.Comol.Modules.CallForPapers.Advanced.Presentation.iView.iViewAdvStepSummary

#Region "Context"
    Private _Presenter As AdvStepSummaryPresenter
    Private ReadOnly Property CurrentPresenter() As AdvStepSummaryPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New AdvStepSummaryPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region



#Region "Inherits"
    Public Overrides Sub BindDati()


        If Not Page.IsPostBack Then
            CallTrapHelper.SendTrap(
                lm.Comol.Modules.CallForPapers.Trap.CallTrapId.StepSummaryView,
                Me.StepId,
                lm.Comol.Modules.CallForPapers.Trap.CallObjectId.Step,
                "")
        End If

        Me.CurrentPresenter.InitView()
    End Sub


    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get

        End Get
    End Property

    Public Property StepId As Long Implements iViewAdvStepSummary.StepId
        Get
            Dim id As Long = 0


            Try
                id = Me.ViewStateOrDefault("stId", 0)
            Catch ex As Exception

            End Try

            If (id = 0) Then
                Try
                    id = Request.QueryString("stId")
                Catch ex As Exception

                End Try
            End If
            Me.ViewState("stId") = id
            Return id
        End Get
        Set(value As Long)
            Me.ViewState("stId") = value
        End Set
    End Property

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub BindNoPermessi()
        CallTrapHelper.SendTrap(
                lm.Comol.Modules.CallForPapers.Trap.CallTrapId.StepClose,
                Me.StepId,
                lm.Comol.Modules.CallForPapers.Trap.CallObjectId.Step,
                "NoPermission")
    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

    Public Overrides Sub SetCultureSettings()

    End Sub

    Public Overrides Sub SetInternazionalizzazione()

    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
#Region "Implements"
    Public Sub BindView(Summary As dtoStepSummary) Implements iViewAdvStepSummary.BindView


        CurrentEvaluationType = Summary.evType

        forClosing = Summary.isForClosing

        _evType = Summary.evType


        If Summary.items.Any() Then


            Me.MLVevaluations.SetActiveView(Me.VIWlist)

            Me.RPTcommission.DataSource = Summary.Commissions
            Me.RPTcommission.DataBind()

            Me.RPTsubmission.DataSource = Summary.items
            Me.RPTsubmission.DataBind()

        Else

            Me.MLVevaluations.SetActiveView(Me.VIWnoItems)

        End If





        LKBConfirmStep.Visible = forClosing

    End Sub
    Private forClosing As Boolean = False

    Public Sub BindNavigationUrl(CommunityId As Integer, CallId As Long, CommissionId As Long) Implements iViewAdvStepSummary.BindNavigationUrl


        If CommissionId = 0 Then
            CommissionId = Me.CommId
        End If

        'If CommissionId > 0 Then
        '    HYPcommissionSummary.NavigateUrl = BaseUrl & RootObject.AdvEvalSummary(CommunityId, CallId, CommissionId, CallStatusForSubmitters.None)
        '    HYPcommissionSummary.Visible = True
        'Else
        '    HYPcommissionSummary.Visible = False
        'End If


        HypStep.NavigateUrl = BaseUrl & RootObject.AdvStepsEdit(CallId)

    End Sub

    Private Property _evType As EvalType
        Get

            Me.ViewStateOrDefault("EvalType", EvalType.Average)
        End Get
        Set(value As EvalType)
            Me.ViewState("EvalType") = value
        End Set
    End Property



    Public Sub DisplaySessionTimeout() Implements IViewBase.DisplaySessionTimeout
        CallTrapHelper.SendTrap(
                lm.Comol.Modules.CallForPapers.Trap.CallTrapId.StepClose,
                Me.StepId,
                lm.Comol.Modules.CallForPapers.Trap.CallObjectId.Step,
                "SessionTimeOut")
    End Sub

    Public Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewBase.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType.NoPermission, , lm.ActionDataContract.InteractionType.UserWithLearningObject)
        Me.Master.ShowNoPermission = True
    End Sub

    Private Sub RPTcommission_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTcommission.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then


            Dim item As lm.Comol.Modules.CallForPapers.Advanced.dto.dtoCommToreport = Nothing

            Try
                item = e.Item.DataItem
            Catch ex As Exception

            End Try

            If Not IsNothing(item) Then

                Dim HYPcommissionSummary As HyperLink = e.Item.FindControl("HYPcommissionSummary")

                If Not IsNothing(HYPcommissionSummary) Then

                    HYPcommissionSummary.Text = item.CommissionName


                    If item.CommissionId > 0 AndAlso item.IsActive Then
                        HYPcommissionSummary.Enabled = True
                        HYPcommissionSummary.NavigateUrl = BaseUrl & RootObject.AdvEvalSummary(item.CommunityId, item.CallId, item.CommissionId, CallStatusForSubmitters.None)
                    Else
                        HYPcommissionSummary.Enabled = False
                        HYPcommissionSummary.NavigateUrl = "#"
                    End If



                End If


            End If



            'Dim LTcomm As Literal = e.Item.FindControl("LTcomm")
            'LTcomm.Text = e.Item.DataItem.ToString()


            'If CommissionId > 0 Then
            '    HYPcommissionSummary.NavigateUrl = BaseUrl & RootObject.AdvEvalSummary(CommunityId, CallId, CommissionId, CallStatusForSubmitters.None)
            '    HYPcommissionSummary.Visible = True
            'Else
            '    HYPcommissionSummary.Visible = False
            'End If

        End If
    End Sub

    Private Sub RPTsubmission_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTsubmission.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim submission As dtoStepSummarySubmission = e.Item.DataItem

            If Not IsNothing(submission) Then


                Dim hyd As HiddenField = e.Item.FindControl("HYDid")
                If Not IsNothing(hyd) Then
                    hyd.Value = submission.StepEvalId
                End If

                Dim Lit As Literal = e.Item.FindControl("LTsubRank")
                If Not IsNothing(Lit) Then
                    Lit.Text = submission.SubmissionRank
                End If

                Lit = e.Item.FindControl("LTsubName")
                If Not IsNothing(Lit) Then
                    Lit.Text = submission.SubmissionName
                End If

                'Dim Uc_ScoreItem As Uc_advScoreItem = e.Item.FindControl("Uc_ScoreItem")
                Dim stepItem As dtoStepSummaryItem = submission.Commissions.FirstOrDefault(Function(c) c.isStep)

                If Not IsNothing(stepItem) Then
                    Lit = e.Item.FindControl("LTfinaleScore")
                    If Not IsNothing(Lit) Then
                        Dim score As String = ""
                        Select Case CurrentEvaluationType
                            Case EvalType.Average
                                'stepItem.AverageRating
                                score = stepItem.AverageRatingToString

                            Case EvalType.Sum
                                score = stepItem.SumRatingToString
                        End Select
                        Lit.Text = score
                    End If

                    Lit = e.Item.FindControl("LTpassed")
                    If Not IsNothing(Lit) Then

                        Select Case stepItem.Status
                            Case CommissionStatus.Draft
                                Lit.Text = "Valutazione non iniziate"
                            Case CommissionStatus.Started
                                If stepItem.Passed Then
                                    Lit.Text = "Commissione aperta (Approvato)"
                                Else
                                    Lit.Text = "Commissione aperta (Respinto)"
                                End If
                            Case CommissionStatus.ValutationEnded
                                If stepItem.Passed Then
                                    Lit.Text = "Valuazioni concluse (Approvato)"
                                Else
                                    Lit.Text = "Valuazioni concluse (Respinto)"
                                End If

                            Case Else
                                If stepItem.Passed Then
                                    Lit.Text = "Approvato"
                                Else
                                    Lit.Text = "Respinto"
                                End If

                        End Select


                    End If

                    Lit = e.Item.FindControl("LTtd")
                    If Not IsNothing(Lit) Then
                        Lit.Text = String.Format(Lit.Text, stepItem.Status)
                    End If

                    Dim cbx As CheckBox = e.Item.FindControl("CBXcheck")
                    If Not IsNothing(cbx) Then
                        cbx.Checked = stepItem.Admit And stepItem.Passed
                        cbx.Enabled = forClosing And stepItem.Passed
                    End If
                End If

                'If Not IsNothing(stepItem) Then

                '    If _evType = EvalType.Sum Then
                '        Uc_ScoreItem.InitUCStep(GetDoubleToString(stepItem.SumRating, 2), stepItem.BoolRating, stepItem.Passed)
                '    Else
                '        Uc_ScoreItem.InitUCStep(GetDoubleToString(stepItem.AverageRating, 2), stepItem.BoolRating, stepItem.Passed)
                '    End If
                'End If


                Dim RPTcommissionScore As Repeater = e.Item.FindControl("RPTcommissionScore")
                    RPTcommissionScore.DataSource = submission.Commissions.Where(Function(s) Not s.isStep).ToList()
                    RPTcommissionScore.DataBind()



                End If



            End If
    End Sub

    Private Function GetDoubleToString(number As Double, Optional decimals As Integer = 2) As [String]
        Dim fractional As [Double] = number - Math.Floor(number)
        Return If((fractional = 0), [String].Format("{0:N0}", number), [String].Format("{0:N" + decimals.ToString() + "}", number))
    End Function

    Public Sub RPTcommissionScore_ItemDataBound(sender As Object, e As RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim comm As dtoStepSummaryItem = e.Item.DataItem

            Dim Uc_ScoreItem As Uc_advScoreItem = e.Item.FindControl("Uc_ScoreItem")

            If Not IsNothing(Uc_ScoreItem) Then

                If comm.EvaluationType = EvalType.Sum Then
                    Uc_ScoreItem.InitUCStep(GetDoubleToString(comm.SumRating, 2), comm.BoolRating, comm.Passed, "Commissione", comm.HasScoreCriteria)
                Else
                    Uc_ScoreItem.InitUCStep(GetDoubleToString(comm.AverageRating, 2), comm.BoolRating, comm.Passed, "Commissione", comm.HasScoreCriteria)
                End If
            End If
        End If

    End Sub

    Private Sub LKBConfirmStep_Click(sender As Object, e As EventArgs) Handles LKBConfirmStep.Click

        Dim itemIds As New List(Of Long)()

        For Each itm As RepeaterItem In Me.RPTsubmission.Items

            Dim cbx As CheckBox = itm.FindControl("CBXcheck")
            If Not IsNothing(cbx) AndAlso cbx.Checked Then

                Dim hyd As HiddenField = itm.FindControl("HYDid")
                If Not IsNothing(hyd) Then

                    Dim id As Long = 0
                    Try
                        id = System.Convert.ToInt64(hyd.Value)
                    Catch ex As Exception

                    End Try

                    If id > 0 Then
                        itemIds.Add(id)
                    End If

                End If


            End If

        Next


        CallTrapHelper.SendTrap(
                lm.Comol.Modules.CallForPapers.Trap.CallTrapId.StepClose,
                Me.StepId,
                lm.Comol.Modules.CallForPapers.Trap.CallObjectId.Step,
                "")

        Me.CurrentPresenter.ConfirmAdmit(itemIds)



    End Sub

    Private ReadOnly Property CommId As Long
        Get
            Dim cid As Long = 0
            Try
                cid = System.Convert.ToInt64(Request.QueryString("cmId"))
            Catch ex As Exception

            End Try
            Return cid
        End Get
    End Property

    Private Property CurrentEvaluationType As EvalType
        Get
            Return ViewStateOrDefault("CurrentEvaluationType", EvalType.Sum)
        End Get
        Set(ByVal value As EvalType)
            Me.ViewState("CurrentEvaluationType") = value
        End Set
    End Property
#End Region
End Class