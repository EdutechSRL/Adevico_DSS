Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation.Evaluation
Imports lm.Comol.Modules.CallForPapers.Domain.Evaluation
Imports lm.ActionDataContract
Imports System.Linq
Public Class ReplaceInEvaluationMembership
    Inherits PageBase
    Implements IViewReplaceInEvaluationMembership

#Region "Context"
    Private _Presenter As ReplaceInEvaluationMembershipPresenter
    Private ReadOnly Property CurrentPresenter() As ReplaceInEvaluationMembershipPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ReplaceInEvaluationMembershipPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property Portalname As String Implements IViewReplaceInEvaluationMembership.Portalname
        Get
            Return Resource.getValue("Portalname")
        End Get
    End Property
    Private ReadOnly Property PreloadIdCall As Long Implements IViewReplaceInEvaluationMembership.PreloadIdCall
        Get
            If IsNumeric(Me.Request.QueryString("idCall")) Then
                Return CLng(Me.Request.QueryString("idCall"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdMembership As Long Implements IViewReplaceInEvaluationMembership.PreloadIdMembership
        Get
            If IsNumeric(Me.Request.QueryString("idMembership")) Then
                Return CLng(Me.Request.QueryString("idMembership"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewReplaceInEvaluationMembership.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadView As lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters Implements IViewReplaceInEvaluationMembership.PreloadView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters).GetByString(Request.QueryString("View"), lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters.SubmissionClosed)
        End Get
    End Property
    Private Property AllowSave As Boolean Implements IViewReplaceInEvaluationMembership.AllowSave
        Get
            Return ViewStateOrDefault("AllowSave", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowSave") = value
        End Set
    End Property
    Private Property IdCall As Long Implements IViewReplaceInEvaluationMembership.IdCall
        Get
            Return ViewStateOrDefault("IdCall", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdCall") = value
        End Set
    End Property
    Private Property IdCommunity As Integer Implements IViewReplaceInEvaluationMembership.IdCommunity
        Get
            Return ViewStateOrDefault("IdCommunity", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCommunity") = value
        End Set
    End Property
    Private Property IdCallModule As Integer Implements IViewReplaceInEvaluationMembership.IdCallModule
        Get
            Return ViewStateOrDefault("IdCallModule", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCallModule") = value
        End Set
    End Property
    Private Property IdMembershipToReplace As Long Implements IViewReplaceInEvaluationMembership.IdMembershipToReplace
        Get
            Return ViewStateOrDefault("IdMembershipToReplace", CLng(0))
        End Get
        Set(value As Long)
            Me.ViewState("IdMembershipToReplace") = value
        End Set
    End Property
    Private Property ExcludedUsers As List(Of Integer) Implements IViewReplaceInEvaluationMembership.ExcludedUsers
        Get
            Return ViewStateOrDefault("ExcludedUsers", New List(Of Integer))
        End Get
        Set(value As List(Of Integer))
            Me.ViewState("ExcludedUsers") = value
        End Set
    End Property
    Private Property SelectedUser As Integer Implements IViewReplaceInEvaluationMembership.SelectedUser
        Get
            Return ViewStateOrDefault("SelectedUser", CInt(0))
        End Get
        Set(value As Integer)
            Me.ViewState("SelectedUser") = value
        End Set
    End Property
    Private Property AssignAll As Boolean Implements IViewReplaceInEvaluationMembership.AssignAll
        Get
            Return ViewStateOrDefault("AssignAll", False)
        End Get
        Set(value As Boolean)
            ViewState("AssignAll") = value
            RBkeepEvaluated.Checked = Not value
            RBreplaceAll.Checked = value
        End Set
    End Property
    Private Property UseDss As Boolean Implements IViewReplaceInEvaluationMembership.UseDss
        Get
            Return ViewStateOrDefault("UseDss", False)
        End Get
        Set(value As Boolean)
            ViewState("UseDss") = value
            RBkeepEvaluated.Enabled = Not value
            RBreplaceAll.Enabled = Not value
            If value Then
                RBreplaceAll.Checked = value
            End If
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
    Protected ReadOnly Property DialogTitleTranslation() As String
        Get
            Return Resource.getValue("LBselectMemberDialgoHeader.text")
        End Get
    End Property
#End Region

    Private ReadOnly Property AnonymousDisplayname As String Implements IViewReplaceInEvaluationMembership.AnonymousDisplayname
        Get
            Return Me.Resource.getValue("AnonymousOwnerName")
        End Get
    End Property
    Private ReadOnly Property UnknownDisplayname As String Implements IViewReplaceInEvaluationMembership.UnknownDisplayname
        Get
            Return Resource.getValue("UnknownDisplayname")
        End Get
    End Property


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

            .setLabel(LBnewEvaluator_t)
            .setButton(BTNselectEvaluator, True)

            .setLabel(LBreplaceAll)
            .setLiteral(LTreplaceAll)
            .setLabel(LBkeepEvaluated)
            .setLiteral(LTkeepEvaluated)
        End With
    End Sub
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType) Implements IViewReplaceInEvaluationMembership.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ObjectType.CallForPaper, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewReplaceInEvaluationMembership.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub

    Private Sub DisplaySessionTimeout() Implements IViewReplaceInEvaluationMembership.DisplaySessionTimeout
        Dim idCommunity As Integer = PreloadIdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

        dto.DestinationUrl = lm.Comol.Modules.CallForPapers.Domain.RootObject.ReplaceInEvaluationMembership(PreloadIdMembership, PreloadIdCall, idCommunity, PreloadView)

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If
        webPost.Redirect(dto)
    End Sub

    Private Sub SetActionUrl(url As String) Implements IViewReplaceInEvaluationMembership.SetActionUrl
        Me.HYPbackToViewBottom.Visible = True
        Me.HYPbackToViewBottom.NavigateUrl = BaseUrl & url
        Me.HYPbackToViewTop.Visible = True
        Me.HYPbackToViewTop.NavigateUrl = BaseUrl & url
    End Sub

    Private Sub SetContainerName1(itemName As String) Implements IViewBaseEditEvaluationSettings.SetContainerName
        Dim title As String = Me.Resource.getValue("serviceTitle.ReplaceCommitteeMember")
        Master.ServiceTitle = title
        Master.ServiceTitleToolTip = itemName
    End Sub
    Private Sub SetContainerName(communityName As String, callName As String) Implements IViewReplaceInEvaluationMembership.SetContainerName
        Dim title As String = Me.Resource.getValue("serviceTitle.ReplaceCommitteeMember")
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

    Private Sub LoadUnknowCall(idCommunity As Integer, idModule As Integer, idCall As Long, type As lm.Comol.Modules.CallForPapers.Domain.CallForPaperType) Implements IViewReplaceInEvaluationMembership.LoadUnknowCall
        MLVsettings.SetActiveView(VIWempty)
        CTRLemptyMessage.InitializeControl(Resource.getValue("LBnoCalls." & type.ToString), Helpers.MessageType.error)
    End Sub
    Public Sub HideErrorMessages() Implements IViewReplaceInEvaluationMembership.HideErrorMessages
        Me.CTRLmessages.Visible = False
    End Sub
    Private Sub DisplayError(err As EvaluationEditorErrors) Implements IViewReplaceInEvaluationMembership.DisplayError
        DisplayError(err, Helpers.MessageType.error)
    End Sub
    Private Sub DisplayWarning(err As EvaluationEditorErrors) Implements IViewReplaceInEvaluationMembership.DisplayWarning
        DisplayError(err, Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayError(err As EvaluationEditorErrors, errorType As Helpers.MessageType)
        Me.CTRLmessages.Visible = (err <> EvaluationEditorErrors.None)
        Me.CTRLmessages.InitializeControl(Resource.getValue("EvaluationEditorErrors." & err.ToString), errorType)
    End Sub
    Private Sub DisplaySettingsSaved() Implements IViewReplaceInEvaluationMembership.DisplaySettingsSaved
        Me.CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayCommitteesSettingsSaved"), Helpers.MessageType.success)
    End Sub

    Private Sub DisplayMemberNotFound() Implements IViewReplaceInEvaluationMembership.DisplayMemberNotFound
        InitializeWizard()
        Me.MLVreplaceWizard.SetActiveView(VIWdefault)
        Me.LBemptyStepInfo.Text = Resource.getValue("DisplayMemberNotFound")
    End Sub
    Private Sub DisplayReplaceNotAvailable() Implements IViewReplaceInEvaluationMembership.DisplayReplaceNotAvailable
        InitializeWizard()
        Me.MLVreplaceWizard.SetActiveView(VIWdefault)
        Me.LBemptyStepInfo.Text = Resource.getValue("DisplayReplaceNotAvailable")
    End Sub
    Private Sub DisplayEvaluationsCompleted(name As String) Implements IViewReplaceInEvaluationMembership.DisplayEvaluationsCompleted
        InitializeWizard()
        Me.MLVreplaceWizard.SetActiveView(VIWdefault)
        Me.LBemptyStepInfo.Text = String.Format(Resource.getValue("DisplayEvaluationsCompleted"), name)
    End Sub

    Private Sub DisplayMemberInfo(name As String, evaluated As Long, inevaluation As Long, notstarted As Long) Implements IViewReplaceInEvaluationMembership.DisplayMemberInfo
        InitializeWizard(False, True, False)
        Me.MLVreplaceWizard.SetActiveView(VIWdetails)
        LTuserInfo.Text = String.Format(LTuserInfo.Text, Me.Resource.getValue("LTuserInfo.0"), name, Me.Resource.getValue("LTuserInfo.2"))
        LTuserStepInfo.Text = String.Format(LTuserStepInfo.Text, Me.Resource.getValue("LTuserStepInfo.0"), Me.BTNnextTop.Text, Me.Resource.getValue("LTuserStepInfo.2"), Me.HYPbackToViewTop.Text, Me.Resource.getValue("LTuserStepInfo.3"))

        LBnotstartedCount.Text = notstarted
        LBstartedCount.Text = inevaluation
        LBcompletedCount.Text = evaluated
    End Sub
    Private Sub LoadCommitteesInfo(committees As List(Of dtoCommitteeEvaluators)) Implements IViewReplaceInEvaluationMembership.LoadCommitteesInfo

    End Sub

    Private Sub LoadEvaluationInfos(items As List(Of dtoBaseEvaluation)) Implements IViewReplaceInEvaluationMembership.LoadEvaluationInfos
        Me.RPTevaluations.DataSource = items
        Me.RPTevaluations.DataBind()
    End Sub

    Private Sub InitializeEvaluatorSelection(usersInEvaluation As List(Of Integer)) Implements IViewReplaceInEvaluationMembership.InitializeEvaluatorSelection
        InitializeWizard(True, False, False)
        Me.MLVreplaceWizard.SetActiveView(VIWselectEvaluator)
        If IdCommunity > 0 Then
            Me.CTRLselectUsers.InitializeControlForSingleSelection(lm.Comol.Core.BaseModules.ProfileManagement.UserSelectionType.CommunityUsers, IdCommunity, usersInEvaluation, Nothing, Resource.getValue("selectSubstituteDescription"))
        Else
            Me.CTRLselectUsers.InitializeControlForSingleSelection(lm.Comol.Core.BaseModules.ProfileManagement.UserSelectionType.SystemUsers, False, usersInEvaluation, Nothing, Resource.getValue("selectSubstituteDescription"))
        End If
        Me.DVoptions.Visible = False
    End Sub

    Public Sub LoadSelectEvaluator(usersInEvaluation As List(Of Integer), idSelectedUser As Integer, name As String, assignAll As Boolean) Implements IViewReplaceInEvaluationMembership.LoadSelectEvaluator
        InitializeWizard(True, False, idSelectedUser > 0)
        Me.MLVreplaceWizard.SetActiveView(VIWselectEvaluator)
        SelectedUser = idSelectedUser
        Me.LBnewEvaluator.Text = name
        Me.AssignAll = assignAll
        Me.DVoptions.Visible = True
    End Sub
    Private Sub ReloadEditor(url As String) Implements IViewReplaceInEvaluationMembership.ReloadEditor
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
            Resource.setLabel(LBstepTitle)
            Resource.setLabel(LBstepDescription)
        Else
            Resource.setLabel(LBstepTitle)
            LBstepDescription.Text = Resource.getValue("replace.Step2")
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
        Me.CurrentPresenter.SelectEvaluator(SelectedUser)
    End Sub

    Private Sub BTNcompleteTop_Click(sender As Object, e As System.EventArgs) Handles BTNcompleteTop.Click, BTNcompleteBottom.Click
        CurrentPresenter.ReplaceEvaluator(IdCall, SelectedUser, RBreplaceAll.Checked)
    End Sub


    Private Sub BTNselectEvaluator_Click(sender As Object, e As System.EventArgs) Handles BTNselectEvaluator.Click
        Me.SetFocus(Me.CTRLselectUsers.GetDefaultTextField)
        Me.DVselectUsers.Visible = True
    End Sub
    Protected Sub CloseWindow() Handles CTRLselectUsers.CloseWindow
        Me.DVselectUsers.Visible = False
        '  Me.SetFocus(Me.btn)
    End Sub

    Private Sub CTRLselectUsers_UserSelected(idUser As Integer) Handles CTRLselectUsers.UserSelected
        Me.DVselectUsers.Visible = False
        Me.CurrentPresenter.SelectEvaluator(idUser)
    End Sub

End Class