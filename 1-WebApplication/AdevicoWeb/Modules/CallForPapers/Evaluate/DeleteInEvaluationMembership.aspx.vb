Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation.Evaluation
Imports lm.Comol.Modules.CallForPapers.Domain.Evaluation
Imports lm.ActionDataContract
Imports System.Linq
Public Class DeleteInEvaluationMembership
    Inherits PageBase
    Implements IViewDeleteInEvaluationMembership

#Region "Context"
    Private _Presenter As DeleteInEvaluationMembershipPresenter
    Private ReadOnly Property CurrentPresenter() As DeleteInEvaluationMembershipPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New DeleteInEvaluationMembershipPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property Portalname As String Implements IViewDeleteInEvaluationMembership.Portalname
        Get
            Return Resource.getValue("Portalname")
        End Get
    End Property
    Private ReadOnly Property PreloadIdCall As Long Implements IViewDeleteInEvaluationMembership.PreloadIdCall
        Get
            If IsNumeric(Me.Request.QueryString("idCall")) Then
                Return CLng(Me.Request.QueryString("idCall"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdMembership As Long Implements IViewDeleteInEvaluationMembership.PreloadIdMembership
        Get
            If IsNumeric(Me.Request.QueryString("idMembership")) Then
                Return CLng(Me.Request.QueryString("idMembership"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewDeleteInEvaluationMembership.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadView As lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters Implements IViewDeleteInEvaluationMembership.PreloadView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters).GetByString(Request.QueryString("View"), lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters.SubmissionClosed)
        End Get
    End Property
    Private Property AllowSave As Boolean Implements IViewDeleteInEvaluationMembership.AllowSave
        Get
            Return ViewStateOrDefault("AllowSave", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowSave") = value
        End Set
    End Property
    Private Property IdCall As Long Implements IViewDeleteInEvaluationMembership.IdCall
        Get
            Return ViewStateOrDefault("IdCall", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdCall") = value
        End Set
    End Property
    Private Property IdCommunity As Integer Implements IViewDeleteInEvaluationMembership.IdCommunity
        Get
            Return ViewStateOrDefault("IdCommunity", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCommunity") = value
        End Set
    End Property
    Private Property IdCallModule As Integer Implements IViewDeleteInEvaluationMembership.IdCallModule
        Get
            Return ViewStateOrDefault("IdCallModule", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCallModule") = value
        End Set
    End Property
    Private Property IdMembershipToRemove As Long Implements IViewDeleteInEvaluationMembership.IdMembershipToRemove
        Get
            Return ViewStateOrDefault("IdMembershipToRemove", CLng(0))
        End Get
        Set(value As Long)
            Me.ViewState("IdMembershipToRemove") = value
        End Set
    End Property
    Private Property RemoveAll As Boolean Implements IViewDeleteInEvaluationMembership.RemoveAll
        Get
            Return ViewStateOrDefault("RemoveAll", True)
        End Get
        Set(value As Boolean)
            ViewState("RemoveAll") = value
            RBkeepEvaluatedFromRemoving.Checked = Not value
            RBremoveAll.Checked = value
        End Set
    End Property
    Private ReadOnly Property AnonymousDisplayname As String Implements IViewDeleteInEvaluationMembership.AnonymousDisplayname
        Get
            Return Resource.getValue("AnonymousOwnerName")
        End Get
    End Property
    Private ReadOnly Property UnknownDisplayname As String Implements IViewDeleteInEvaluationMembership.UnknownDisplayname
        Get
            Return Resource.getValue("UnknownDisplayname")
        End Get
    End Property
    Private Property UseDss As Boolean Implements IViewDeleteInEvaluationMembership.UseDss
        Get
            Return ViewStateOrDefault("UseDss", False)
        End Get
        Set(value As Boolean)
            ViewState("UseDss") = value
            RBkeepEvaluatedFromRemoving.Enabled = Not value
            RBremoveAll.Enabled = Not value
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
    Protected ReadOnly Property GetEvaluationCssClass(status As EvaluationStatus) As String
        Get
            Select Case status
                Case EvaluationStatus.Evaluated
                    Return "completed"
                Case EvaluationStatus.Evaluating
                    Return "started"
                Case EvaluationStatus.None
                    Return "notstarted"
            End Select
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
            CTRLemptyMessage.InitializeControl(Resource.getValue("LBnoCalls"), Helpers.MessageType.error)

            .setHyperLink(HYPbackToViewBottom, True, True)
            .setHyperLink(HYPbackToViewTop, True, True)
            .setButton(BTNbackBottom, True)
            .setButton(BTNbackTop, True)
            .setButton(BTNcompleteBottom, True)
            .setButton(BTNcompleteTop, True)
            .setButton(BTNnextBottom, True)
            .setButton(BTNnextTop, True)
            .setLiteral(LTsubmitterName_t)
            .setLiteral(LTevaluationStatus_t)
            .setLabel(LBevaluationsInfo_t)

            LBcompleted.Text = Resource.getValue("EvaluationStatus." & EvaluationStatus.Evaluated.ToString())
            LBcompletedCount.ToolTip = LBcompleted.Text
            LBstarted.Text = Resource.getValue("EvaluationStatus." & EvaluationStatus.Evaluating.ToString())
            LBstartedCount.ToolTip = LBstarted.Text
            LBnotstarted.Text = Resource.getValue("EvaluationStatus." & EvaluationStatus.None.ToString())
            LBnotstartedCount.ToolTip = LBnotstarted.Text
            .setLabel(LBstepRemoveTitle)
            .setLabel(LBremoveAll)
            .setLiteral(LTremoveAll)
            .setLabel(LBkeepEvaluatedFromRemoving)
            .setLiteral(LTkeepEvaluatedFromRemoving)
        End With
    End Sub
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType) Implements IViewDeleteInEvaluationMembership.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ObjectType.CallForPaper, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewDeleteInEvaluationMembership.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub

    Private Sub DisplaySessionTimeout() Implements IViewDeleteInEvaluationMembership.DisplaySessionTimeout
        Dim idCommunity As Integer = PreloadIdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

        dto.DestinationUrl = lm.Comol.Modules.CallForPapers.Domain.RootObject.DeleteInEvaluationMembership(PreloadIdMembership, PreloadIdCall, idCommunity, PreloadView)

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If
        webPost.Redirect(dto)
    End Sub

    Private Sub SetActionUrl(url As String) Implements IViewDeleteInEvaluationMembership.SetActionUrl
        Me.HYPbackToViewBottom.Visible = True
        Me.HYPbackToViewBottom.NavigateUrl = BaseUrl & url
        Me.HYPbackToViewTop.Visible = True
        Me.HYPbackToViewTop.NavigateUrl = BaseUrl & url
    End Sub

    Private Sub SetContainerName1(itemName As String) Implements IViewBaseEditEvaluationSettings.SetContainerName
        Dim title As String = Me.Resource.getValue("serviceTitle.RemoveCommitteeMember")
        Master.ServiceTitle = title
        Master.ServiceTitleToolTip = itemName
    End Sub
    Private Sub SetContainerName(communityName As String, callName As String) Implements IViewDeleteInEvaluationMembership.SetContainerName
        Dim title As String = Me.Resource.getValue("serviceTitle.RemoveCommitteeMember")
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

    Private Sub LoadUnknowCall(idCommunity As Integer, idModule As Integer, idCall As Long, type As lm.Comol.Modules.CallForPapers.Domain.CallForPaperType) Implements IViewDeleteInEvaluationMembership.LoadUnknowCall
        MLVsettings.SetActiveView(VIWempty)
        CTRLemptyMessage.InitializeControl(Resource.getValue("LBnoCalls." & type.ToString), Helpers.MessageType.error)
    End Sub
    Public Sub HideErrorMessages() Implements IViewDeleteInEvaluationMembership.HideErrorMessages
        Me.CTRLmessages.Visible = False
    End Sub
    Private Sub DisplayError(err As EvaluationEditorErrors) Implements IViewDeleteInEvaluationMembership.DisplayError
        DisplayError(err, Helpers.MessageType.error)
    End Sub
    Private Sub DisplayWarning(err As EvaluationEditorErrors) Implements IViewDeleteInEvaluationMembership.DisplayWarning
        DisplayError(err, Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayError(err As EvaluationEditorErrors, errorType As Helpers.MessageType)
        Me.CTRLmessages.Visible = (err <> EvaluationEditorErrors.None)
        Me.CTRLmessages.InitializeControl(Resource.getValue("EvaluationEditorErrors." & err.ToString), errorType)
    End Sub
    Private Sub DisplaySettingsSaved() Implements IViewDeleteInEvaluationMembership.DisplaySettingsSaved
        Me.CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayCommitteesSettingsSaved"), Helpers.MessageType.success)
    End Sub

    Private Sub DisplayMemberNotFound() Implements IViewDeleteInEvaluationMembership.DisplayMemberNotFound
        InitializeWizard()
        Me.MLVreplaceWizard.SetActiveView(VIWdefault)
        Me.LBemptyStepInfo.Text = Resource.getValue("DisplayMemberNotFound")
    End Sub
    Private Sub DisplayMemberInfo(name As String, evaluated As Long, inevaluation As Long, notstarted As Long) Implements IViewDeleteInEvaluationMembership.DisplayMemberInfo
        InitializeWizard(False, True, False)
        Me.MLVreplaceWizard.SetActiveView(VIWdetails)
        LTuserInfo.Text = String.Format(LTuserInfo.Text, Me.Resource.getValue("LTuserInfo.0"), name, Me.Resource.getValue("LTuserInfo.2"))
        LTuserStepInfo.Text = String.Format(LTuserStepInfo.Text, Me.Resource.getValue("LTuserStepInfo.0"), Me.BTNnextTop.Text, Me.Resource.getValue("LTuserStepInfo.2"), Me.HYPbackToViewTop.Text, Me.Resource.getValue("LTuserStepInfo.3"))

        LBnotstartedCount.Text = notstarted
        LBstartedCount.Text = inevaluation
        LBcompletedCount.Text = evaluated
    End Sub
    Private Sub LoadCommitteesInfo(committees As List(Of dtoCommitteeEvaluators)) Implements IViewDeleteInEvaluationMembership.LoadCommitteesInfo

    End Sub

    Private Sub LoadEvaluationInfos(items As List(Of dtoBaseEvaluation)) Implements IViewDeleteInEvaluationMembership.LoadEvaluationInfos
        Me.RPTevaluations.DataSource = items
        Me.RPTevaluations.DataBind()
    End Sub

    Private Sub ReloadEditor(url As String) Implements IViewDeleteInEvaluationMembership.ReloadEditor
        PageUtility.RedirectToUrl(url)
    End Sub
#End Region

    Private Sub InitializeWizard()
        InitializeWizard(False, False, False)
    End Sub
    Private Sub InitializeWizard(pButton As Boolean, nButton As Boolean, cButton As Boolean)
        Me.MLVsettings.SetActiveView(VIWsettings)
        Me.BTNnextTop.Visible = nButton
        Me.BTNnextBottom.Visible = nButton
        Me.BTNbackBottom.Visible = pButton
        Me.BTNbackTop.Visible = pButton
        Me.BTNcompleteBottom.Visible = cButton
        Me.BTNcompleteTop.Visible = cButton

        If (nButton) OrElse (Not nButton AndAlso Not pButton AndAlso Not cButton) Then
            Resource.setLabel(LBstepRemoveTitle)
            Resource.setLabel(LBstepRemoveDescription)
        Else
            Resource.setLabel(LBstepRemoveDescription)
            LBstepRemoveDescription.Text = Resource.getValue("remove.Step2")
        End If

    End Sub

    Private Sub RedirectToUrl(url As String) Implements IViewBaseEditEvaluationSettings.RedirectToUrl
        PageUtility.RedirectToUrl(url)
    End Sub


    Private Sub RPTevaluations_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTevaluations.ItemDataBound
        Dim oItem As dtoBaseEvaluation = DirectCast(e.Item.DataItem, dtoBaseEvaluation)
        Dim oLabel As Label = e.Item.FindControl("LBstatus")
        oLabel.Text = Resource.getValue("EvaluationStatus." & oItem.Status.ToString())
        oLabel = e.Item.FindControl("LBstatusIcon")
        oLabel.ToolTip = Resource.getValue("EvaluationStatus." & oItem.Status.ToString())
        oLabel.CssClass &= " " & GetEvaluationCssClass(oItem.Status)
    End Sub

    Private Sub BTNbackBottom_Click(sender As Object, e As System.EventArgs) Handles BTNbackBottom.Click, BTNbackTop.Click
        InitializeWizard(False, True, False)
        Me.MLVreplaceWizard.SetActiveView(VIWdetails)
    End Sub

    Private Sub BTNnextBottom_Click(sender As Object, e As System.EventArgs) Handles BTNnextBottom.Click, BTNnextTop.Click
        InitializeWizard(True, False, True)
        Me.MLVreplaceWizard.SetActiveView(VIWactionForRemove)
    End Sub

    Private Sub BTNcompleteTop_Click(sender As Object, e As System.EventArgs) Handles BTNcompleteTop.Click, BTNcompleteBottom.Click
        CurrentPresenter.RemoveEvaluator(IdCall, IdMembershipToRemove, UseDss OrElse RBremoveAll.Checked)
    End Sub

End Class