Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers
Imports Eco = lm.Comol.Modules.CallForPapers.AdvEconomic


Imports lm.ActionDataContract
Imports System.Linq
Imports lm.Comol.Modules.CallForPapers.Presentation
Imports lm.Comol.Modules.CallForPapers.AdvEconomic.dto
Imports lm.Comol.Modules.CallForPapers.AdvEconomic.Presentation.View
Imports lm.Comol.Modules.CallForPapers.Domain

Public Class cpAdvEconomicSubmissions
    Inherits PageBase
    Implements Eco.Presentation.View.iViewEcoSummary


    Private Shared CurrencyFormat As String = "C2"
    Private Shared CurrencyCulture As String = "it-IT"
    Private Totalfunded As Double = 0

    Private Property IsAverage As Boolean
        Get
            Return ViewStateOrDefault("IsAverage", True)
        End Get
        Set(value As Boolean)
            ViewState("IsAverage") = value
        End Set
    End Property

#Region "Context"
    Private _Presenter As Eco.Presentation.AdvEcoSummaryPresenter
    Private ReadOnly Property CurrentPresenter() As Eco.Presentation.AdvEcoSummaryPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New Eco.Presentation.AdvEcoSummaryPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "PageBase"
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


    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True


        CallTrapHelper.SendTrap(
               lm.Comol.Modules.CallForPapers.Trap.CallTrapId.AdvCommissionSummary,
               Me.CommissionId,
               lm.Comol.Modules.CallForPapers.Trap.CallObjectId.AdvCommission,
               "NoPermessi;CommissionType:Economic")

    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

    Public Overrides Sub SetCultureSettings()

    End Sub

    Public Overrides Sub SetInternazionalizzazione()

    End Sub

    Public Overrides Sub BindDati()

        If Not Page.IsPostBack Then
            CallTrapHelper.SendTrap(
                lm.Comol.Modules.CallForPapers.Trap.CallTrapId.AdvCommissionSummary,
                Me.CommissionId,
                lm.Comol.Modules.CallForPapers.Trap.CallObjectId.AdvCommission,
                "Economic")
        End If

        Me.CurrentPresenter.InitView()
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function


#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "DefaultView"
    Public Sub DisplaySessionTimeout() Implements IViewBase.DisplaySessionTimeout
        CallTrapHelper.SendTrap(
               lm.Comol.Modules.CallForPapers.Trap.CallTrapId.AdvCommissionSummary,
               Me.CommissionId,
               lm.Comol.Modules.CallForPapers.Trap.CallObjectId.AdvCommission,
               "SessionTimeOut;CommissionType:Economic")
    End Sub

    Public Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewBase.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.Master.ShowNoPermission = True
    End Sub


#End Region

#Region "View"

    Public ReadOnly Property CommissionId As Long Implements iViewEcoSummary.CommissionId
        Get
            Dim cmId As Long = 0
            Try
                cmId = ViewStateOrDefault("CommissionId", 0)
            Catch ex As Exception

            End Try

            If (cmId = 0) Then
                Try
                    cmId = Request.QueryString("cmId")
                Catch ex As Exception

                End Try
            End If

            ViewState("CommissionId") = cmId

            Return cmId
        End Get
    End Property

    Public Sub BindView(Summaries As Eco.dto.dtoEcoSummaryContainer) Implements iViewEcoSummary.BindView

        Me.IsAverage = Summaries.IsAverage

        HYPback.NavigateUrl = BaseUrl & RootObject.AdvStepsEdit(Summaries.CallId)
        LKBcloseCommission.Visible = Summaries.CanCloseCommission

        If IsNothing(Summaries.Summaries) OrElse Not Summaries.Summaries.Any() Then
            Me.RPTsubmission.Visible = False
            'ToDo: Show no Data
        Else
            Me.RPTsubmission.DataSource = Summaries.Summaries
            Me.RPTsubmission.DataBind()
        End If

        LTtotal.Text = Totalfunded.ToString(CurrencyFormat, System.Globalization.CultureInfo.CreateSpecificCulture(CurrencyCulture)).Remove(0, 2)

    End Sub

    Private Sub RPTsubmission_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTsubmission.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim Eval As Eco.dto.dtoEcoSummary = e.Item.DataItem

            Dim Lit As Literal = e.Item.FindControl("LTrank")
            If Not IsNothing(Lit) Then
                Lit.Text = Eval.Rank
            End If


            Lit = e.Item.FindControl("LTname")
            If Not IsNothing(Lit) Then
                Lit.Text = Eval.SubmissionName
            End If

            Lit = e.Item.FindControl("LTtype")
            If Not IsNothing(Lit) Then
                Lit.Text = Eval.SubmissionType
            End If

            Lit = e.Item.FindControl("LTscore")
            If Not IsNothing(Lit) Then

                If IsAverage Then
                    Lit.Text = Eval.AverageRating.ToString("F2")
                Else
                    Lit.Text = Eval.SumRating.ToString("F2")
                End If



            End If

            Lit = e.Item.FindControl("LTfunded")
            If Not IsNothing(Lit) Then
                Lit.Text = Eval.Founded.ToString(CurrencyFormat, System.Globalization.CultureInfo.CreateSpecificCulture(CurrencyCulture)).Remove(0, 2)
                Totalfunded += Eval.Founded
            End If


            Lit = e.Item.FindControl("LTspanStatus")
            If Not IsNothing(Lit) Then
                Dim StatusString As String = ""

                Select Case Eval.status
                    Case Eco.EvalStatus.draft
                        StatusString = "bozza"
                    Case Eco.EvalStatus.take
                        StatusString = "In carico"
                    Case Eco.EvalStatus.completed
                        StatusString = "Completato"
                    Case Eco.EvalStatus.confirmed
                        StatusString = "Confermato"
                End Select

                Lit.Text = String.Format(Lit.Text, Eval.status, StatusString)

            End If

            Dim HYPEval As HyperLink = e.Item.FindControl("HYPeval")
            If Not IsNothing(HYPEval) Then
                HYPEval.NavigateUrl = BaseUrl & RootObject.EcoEvaluation(Me.CommissionId, Eval.EvalautionId)
            End If

            HYPEval = e.Item.FindControl("HYPsubmission")
            If Not IsNothing(HYPEval) Then
                HYPEval.NavigateUrl = BaseUrl & RootObject.ViewSubmission(CallForPaperType.CallForBids, Eval.CallId, Eval.SubmissionId, False, CallStatusForSubmitters.None, Me.ComunitaCorrenteID, Me.CommissionId)
            End If

            Dim UcTableExport As Uc_AdvTableExport = e.Item.FindControl("Uc_AdvTableExport")

            If Not IsNothing(UcTableExport) Then
                UcTableExport.EvaluationId = Eval.EvalautionId
                UcTableExport.FileName = String.Format("{0}-{1}", Eval.EvalautionId, Eval.SubmissionName.Replace(" ", "_"))
            End If

        End If
    End Sub

    Private Sub LKBcloseCommission_Click(sender As Object, e As EventArgs) Handles LKBcloseCommission.Click

        CallTrapHelper.SendTrap(
                lm.Comol.Modules.CallForPapers.Trap.CallTrapId.AdvCommissionClose,
                Me.CommissionId,
                lm.Comol.Modules.CallForPapers.Trap.CallObjectId.AdvCommission,
                "Economic")

        Me.CurrentPresenter.CloseCommission()
    End Sub

#End Region


#Region "Action"

    Private Sub SendUserAction(
                              idCommunity As Integer,
                              idModule As Integer,
                              action As lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType,
                              objectType As lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ObjectType,
                              IdObject As String) _
                Implements iViewEcoSummary.SendUserAction

        PageUtility.AddActionToModule(idCommunity,
                                      idModule,
                                      action,
                                      PageUtility.CreateObjectsList(
                                        idModule,
                                        objectType,
                                        IdObject),
                                        InteractionType.UserWithLearningObject)
    End Sub


#End Region

End Class