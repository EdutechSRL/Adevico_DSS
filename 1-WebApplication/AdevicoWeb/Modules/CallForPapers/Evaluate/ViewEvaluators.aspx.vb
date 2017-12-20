Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation.Evaluation
Imports lm.Comol.Modules.CallForPapers.Domain.Evaluation
Imports lm.ActionDataContract
Imports System.Linq
Public Class ViewEvaluators
    Inherits PageBase
    Implements IViewViewEvaluators

#Region "Context"
    Private _Presenter As ViewEvaluatorsPresenter
    Private ReadOnly Property CurrentPresenter() As ViewEvaluatorsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ViewEvaluatorsPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property Portalname As String Implements IViewViewEvaluators.Portalname
        Get
            Return Resource.getValue("Portalname")
        End Get
    End Property
    Private ReadOnly Property PreloadIdCall As Long Implements IViewViewEvaluators.PreloadIdCall
        Get
            If IsNumeric(Me.Request.QueryString("idCall")) Then
                Return CLng(Me.Request.QueryString("idCall"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewViewEvaluators.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadView As lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters Implements IViewViewEvaluators.PreloadView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters).GetByString(Request.QueryString("View"), lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters.SubmissionClosed)
        End Get
    End Property
    Private Property AllowSave As Boolean Implements IViewViewEvaluators.AllowSave
        Get
            Return ViewStateOrDefault("AllowSave", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowSave") = value
        End Set
    End Property
    Private Property IdCall As Long Implements IViewViewEvaluators.IdCall
        Get
            Return ViewStateOrDefault("IdCall", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdCall") = value
        End Set
    End Property
    Private Property IdCommunity As Integer Implements IViewViewEvaluators.IdCommunity
        Get
            Return ViewStateOrDefault("IdCommunity", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCommunity") = value
        End Set
    End Property
    Private Property IdCallModule As Integer Implements IViewViewEvaluators.IdCallModule
        Get
            Return ViewStateOrDefault("IdCallModule", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCallModule") = value
        End Set
    End Property
    Private Property CommiteesWithEvaluationsCompleted As List(Of Long) Implements IViewViewEvaluators.CommiteesWithEvaluationsCompleted
        Get
            Return ViewStateOrDefault("CommiteesWithEvaluationsCompleted", New List(Of Long))
        End Get
        Set(value As List(Of Long))
            Me.ViewState("CommiteesWithEvaluationsCompleted") = value
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

#Region "Internal"
    Public ReadOnly Property GetEvaluatorCssClass(status As MembershipStatus, display As lm.Comol.Modules.CallForPapers.Domain.displayAs) As String
        Get
            Return status.ToString.ToLower() & IIf(display <> lm.Comol.Modules.CallForPapers.Domain.displayAs.item, " " & display.ToString, "")
        End Get
    End Property
#End Region

    Private Property e As Object

    Private Sub EditEvaluationCommittees_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Me.Master.ShowDocType = True
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        Me.CurrentPresenter.InitView()
    End Sub

    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_EditCommittees", "Modules", "CallForPapers")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            LBnocalls.Text = Resource.getValue("LBnoCalls")

            .setHyperLink(HYPbackTop, True, True)
            .setHyperLink(HYPbackBottom, True, True)
            .setLabel(LBcollapseAllTop)
            .setLabel(LBexpandAllTop)
        End With
    End Sub
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType) Implements IViewViewEvaluators.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ObjectType.CallForPaper, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewViewEvaluators.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub

    Private Sub DisplaySessionTimeout() Implements IViewViewEvaluators.DisplaySessionTimeout
        Dim idCommunity As Integer = PreloadIdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

        dto.DestinationUrl = lm.Comol.Modules.CallForPapers.Domain.RootObject.EditCommiteeByStep(PreloadIdCall, idCommunity, WizardEvaluationStep.ManageEvaluators, PreloadView)

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If
        webPost.Redirect(dto)
    End Sub

    Private Sub SetActionUrl(url As String) Implements IViewViewEvaluators.SetActionUrl
        Me.HYPbackBottom.Visible = True
        Me.HYPbackBottom.NavigateUrl = BaseUrl & url
        Me.HYPbackTop.Visible = True
        Me.HYPbackTop.NavigateUrl = BaseUrl & url
    End Sub

    Private Sub SetContainerName1(itemName As String) Implements IViewBaseEditEvaluationSettings.SetContainerName
        Dim title As String = Me.Resource.getValue("serviceTitle." & WizardEvaluationStep.ManageEvaluators.ToString())
        Master.ServiceTitle = title
        Master.ServiceTitleToolTip = itemName
    End Sub
    Private Sub SetContainerName(communityName As String, callName As String) Implements IViewViewEvaluators.SetContainerName
        Dim title As String = Me.Resource.getValue("serviceTitle." & WizardEvaluationStep.ManageEvaluators.ToString())
        Master.ServiceTitle = title
        If String.IsNullOrEmpty(communityName) Then
            Master.ServiceTitleToolTip = title & " - " & callName
        Else
            Master.ServiceTitleToolTip = title & " - " & callName
        End If
    End Sub


    Private Sub LoadWizardSteps(idCall As Long, idCommunity As Integer, steps As List(Of lm.Comol.Core.Wizard.NavigableWizardItem(Of dtoEvaluationStep))) Implements IViewBaseEditEvaluationSettings.LoadWizardSteps
        Me.CTRLsteps.InitalizeControl(idCall, idCommunity, PreloadView, steps)
    End Sub
    Private Sub LoadWizardSteps(idCall As Long, idCommunity As Integer, steps As List(Of lm.Comol.Core.Wizard.NavigableWizardItem(Of dtoEvaluationStep)), err As EvaluationEditorErrors) Implements IViewBaseEditEvaluationSettings.LoadWizardSteps
        Me.CTRLsteps.InitalizeControl(idCall, idCommunity, PreloadView, steps, err)
    End Sub

    Private Sub LoadUnknowCall(idCommunity As Integer, idModule As Integer, idCall As Long, type As lm.Comol.Modules.CallForPapers.Domain.CallForPaperType) Implements IViewViewEvaluators.LoadUnknowCall
        Me.MLVsettings.SetActiveView(VIWempty)
        Me.LBnocalls.Text = Resource.getValue("LBnoCalls." & type.ToString)
    End Sub
    Public Sub HideErrorMessages() Implements IViewViewEvaluators.HideErrorMessages
        Me.CTRLmessages.Visible = False
    End Sub
    Private Sub DisplayError(err As EvaluationEditorErrors) Implements IViewViewEvaluators.DisplayError
        DisplayError(err, Helpers.MessageType.error)
    End Sub
    Private Sub DisplayWarning(err As EvaluationEditorErrors) Implements IViewViewEvaluators.DisplayWarning
        DisplayError(err, Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayError(err As EvaluationEditorErrors, errorType As Helpers.MessageType)
        Me.CTRLmessages.Visible = (err <> EvaluationEditorErrors.None)
        Me.CTRLmessages.InitializeControl(Resource.getValue("EvaluationEditorErrors." & err.ToString), errorType)
    End Sub
    Private Sub DisplaySettingsSaved() Implements IViewViewEvaluators.DisplaySettingsSaved
        Me.CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayCommitteesSettingsSaved"), Helpers.MessageType.success)
    End Sub
    Private Sub LoadCommitteesInfo(committees As List(Of dtoCommitteeEvaluators)) Implements IViewViewEvaluators.LoadCommitteesInfo
        Me.MLVsettings.SetActiveView(VIWsettings)
        Me.RPTcommittees.DataSource = committees
        Me.RPTcommittees.DataBind()
    End Sub

    Private Sub ReloadEditor(url As String) Implements IViewViewEvaluators.ReloadEditor
        PageUtility.RedirectToUrl(url)
    End Sub
    'Private Sub InitializeAddFieldControl(idCall As Long) Implements IViewCallEditor.InitializeAddFieldControl
    '    Me.CTRLaddField.InitializeControl(idCall)
    'End Sub
#End Region

    Private Sub RPTcommittees_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTcommittees.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim committee As dtoCommitteeEvaluators = DirectCast(e.Item.DataItem, dtoCommitteeEvaluators)

            Dim oLiteral As Literal = e.Item.FindControl("LTidCommittee")
            oLiteral.Text = committee.Id
            Dim oLabel As Label = e.Item.FindControl("LBcommitteeName_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBcommitteeName")
            oLabel.Text = committee.Name

            oLabel = e.Item.FindControl("LBcommitteeDescription_t")
            Me.Resource.setLabel(oLabel)
            oLabel.Visible = Not String.IsNullOrEmpty(committee.Description)

            oLabel = e.Item.FindControl("LBcommitteeDescription")
            oLabel.Visible = Not String.IsNullOrEmpty(committee.Description)
            If Not String.IsNullOrEmpty(committee.Description) Then
                oLabel.Text = committee.Description
            End If

            Dim hyperlink As HyperLink = e.Item.FindControl("HYPtoTopCommittee")
            hyperlink.Visible = (committee.Evaluators.Count > 5)
            Resource.setHyperLink(hyperlink, True, True)
            hyperlink.NavigateUrl = "#committee_" & committee.Id.ToString

           
            oLiteral = e.Item.FindControl("LTevaluatorName_t")
            Resource.setLiteral(oLiteral)
            oLiteral = e.Item.FindControl("LTevaluationsStatus_t")
            Resource.setLiteral(oLiteral)

        End If
    End Sub

    Protected Sub RPTevaluators_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim item As dtoBaseEvaluatorStatistics = DirectCast(e.Item.DataItem, dtoBaseEvaluatorStatistics)
            Dim replacedyBy As String = "", replacingUser As String = ""

            If Not IsNothing(item.ReplacedBy) Then
                replacedyBy = item.ReplacedBy.SurnameAndName
            End If
            If Not IsNothing(item.ReplacingUser) Then
                replacingUser = item.ReplacingUser.SurnameAndName
            End If
            Dim oLabel As Label = e.Item.FindControl("LBextraInfo")
            oLabel.Visible = (item.Status <> MembershipStatus.Standard)
            Select Case item.Status
                Case MembershipStatus.Removed
                    oLabel.Text = String.Format(oLabel.Text, Resource.getValue("extrainfo." & item.Status.ToString), replacedyBy)

                Case MembershipStatus.Replaced
                    oLabel.Text = String.Format(oLabel.Text, Resource.getValue("extrainfo." & item.Status.ToString), replacedyBy)
                    oLabel.CssClass &= " substitutedby"
                Case MembershipStatus.Replacing
                    oLabel.Text = String.Format(oLabel.Text, Resource.getValue("extrainfo." & item.Status.ToString), replacingUser)
                    oLabel.CssClass &= " substitute"
            End Select
  
            Dim oControl As UC_StackedBar = e.Item.FindControl("CTRLstackedBar")
            InitStackedBar(item, oControl)

            Dim oHyperLink As HyperLink = e.Item.FindControl("HYPreplaceEvaluator")
            If AllowSave AndAlso item.Status <> MembershipStatus.Replaced AndAlso Not CommiteesWithEvaluationsCompleted.Contains(item.IdCommittee) Then
                oHyperLink.Visible = True
                oHyperLink.NavigateUrl = BaseUrl & lm.Comol.Modules.CallForPapers.Domain.RootObject.ReplaceInEvaluationMembership(item.IdMembership, IdCall, IdCommunity, PreloadView)
                Resource.setHyperLink(oHyperLink, False, True)

                oHyperLink = e.Item.FindControl("HYPdeleteEvaluator")
                oHyperLink.NavigateUrl = BaseUrl & lm.Comol.Modules.CallForPapers.Domain.RootObject.DeleteInEvaluationMembership(item.IdMembership, IdCall, IdCommunity, PreloadView)
                oHyperLink.Visible = True
                Resource.setHyperLink(oHyperLink, False, True)
            End If
        End If
    End Sub

    Private Sub InitStackedBar(item As dtoBaseEvaluatorStatistics, CTRLStackedBar As UC_StackedBar)
        Dim evaluated As StackedBarItem = New StackedBarItem()
        evaluated.CssClass = "completed"
        evaluated.Title = Me.Resource.getValue("ProgressTooltip.EvaluationStatus." & EvaluationStatus.Evaluated.ToString())
        evaluated.Value = Percentual(item.GetEvaluationsCount(EvaluationStatus.Evaluated), item.GetEvaluationsCount())

        Dim evaluating As StackedBarItem = New StackedBarItem()
        evaluating.CssClass = "started"
        evaluating.Title = Me.Resource.getValue("ProgressTooltip.EvaluationStatus." & EvaluationStatus.Evaluating.ToString())
        evaluating.Value = Percentual(item.GetEvaluationsCount(EvaluationStatus.Evaluating), item.GetEvaluationsCount())

        Dim notstarted As StackedBarItem = New StackedBarItem()
        notstarted.CssClass = "notstarted"
        notstarted.Title = Me.Resource.getValue("ProgressTooltip.EvaluationStatus." & EvaluationStatus.None.ToString())
        notstarted.Value = Percentual(item.GetEvaluationsCount(EvaluationStatus.None), item.GetEvaluationsCount())

        If notstarted.Value > 0 Then
            notstarted.Value = 100 - evaluated.Value - evaluating.Value
        ElseIf evaluating.Value > 0 Then
            evaluating.Value = 100 - evaluated.Value
        End If

        CTRLStackedBar.InitializeControl({evaluated, evaluating, notstarted})
    End Sub

    Private Function Percentual(value As Long, tot As Long) As Int32
        If tot > 0 Then
            Dim d As Double = value / tot * 100
            Return Math.Floor(d)
        Else
            Return 0
        End If
    End Function
    Private Sub RedirectToUrl(url As String) Implements IViewBaseEditEvaluationSettings.RedirectToUrl
        PageUtility.RedirectToUrl(url)
    End Sub
End Class