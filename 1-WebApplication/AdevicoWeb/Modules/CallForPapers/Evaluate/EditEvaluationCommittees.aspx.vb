Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation.Evaluation
Imports lm.Comol.Modules.CallForPapers.Domain.Evaluation
Imports lm.ActionDataContract
Imports System.Linq
Imports lm.Comol.Core.Dss.Domain.Templates

Public Class EditEvaluationCommittees
    Inherits FRcallPageBase
    Implements IViewEvaluationCommitteesEditor



#Region "Context"
    Private _Presenter As EvaluationCommitteesEditorPresenter
    Private ReadOnly Property CurrentPresenter() As EvaluationCommitteesEditorPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New EvaluationCommitteesEditorPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property Portalname As String Implements IViewEvaluationCommitteesEditor.Portalname
        Get
            Return Resource.getValue("Portalname")
        End Get
    End Property
    Private ReadOnly Property PreloadIdCall As Long Implements IViewEvaluationCommitteesEditor.PreloadIdCall
        Get
            If IsNumeric(Me.Request.QueryString("idCall")) Then
                Return CLng(Me.Request.QueryString("idCall"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewEvaluationCommitteesEditor.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadView As lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters Implements IViewEvaluationCommitteesEditor.PreloadView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters).GetByString(Request.QueryString("View"), lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters.SubmissionClosed)
        End Get
    End Property
    Private Property AllowSave As Boolean Implements IViewEvaluationCommitteesEditor.AllowSave
        Get
            Return ViewStateOrDefault("AllowSave", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowSave") = value
            Me.BTNaddCommitteeTop.Visible = value
            Me.BTNsaveCommitteesBottom.Visible = value
            Me.BTNsaveCommitteesTop.Visible = value
        End Set
    End Property
    Private Property AllowSaveBaseInfo As Boolean Implements IViewEvaluationCommitteesEditor.AllowSaveBaseInfo
        Get
            Return ViewStateOrDefault("AllowSaveBaseInfo", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowSaveBaseInfo") = value
            If value Then
                Me.BTNsaveCommitteesBottom.Visible = True
                Me.BTNsaveCommitteesTop.Visible = True
            End If

        End Set
    End Property
    Private Property IdCall As Long Implements IViewEvaluationCommitteesEditor.IdCall
        Get
            Return ViewStateOrDefault("IdCall", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdCall") = value
        End Set
    End Property
    Private Property IdCommunity As Integer Implements IViewEvaluationCommitteesEditor.IdCommunity
        Get
            Return ViewStateOrDefault("IdCommunity", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCommunity") = value
        End Set
    End Property
    Private Property IdCallModule As Integer Implements IViewEvaluationCommitteesEditor.IdCallModule
        Get
            Return ViewStateOrDefault("IdCallModule", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCallModule") = value
        End Set
    End Property
    Private Property Availablesubmitters As List(Of lm.Comol.Modules.CallForPapers.Domain.dtoSubmitterType) Implements IViewEvaluationCommitteesEditor.AvailableSubmitters
        Get
            Return ViewStateOrDefault("Availablesubmitters", New List(Of lm.Comol.Modules.CallForPapers.Domain.dtoSubmitterType))
        End Get
        Set(value As List(Of lm.Comol.Modules.CallForPapers.Domain.dtoSubmitterType))
            Me.ViewState("Availablesubmitters") = value
        End Set
    End Property
    Private ReadOnly Property DefaultCommitteeDescription As String Implements IViewEvaluationCommitteesEditor.DefaultCommitteeDescription
        Get
            Return Resource.getValue("DefaultCommitteeDescription")
        End Get
    End Property
    Private ReadOnly Property DefaultCommitteeName As String Implements IViewEvaluationCommitteesEditor.DefaultCommitteeName
        Get
            Return Resource.getValue("DefaultCommitteeName")
        End Get
    End Property
    Private Property AllowSubmittersSelection As Boolean Implements IViewEvaluationCommitteesEditor.AllowSubmittersSelection
        Get
            Return ViewStateOrDefault("AllowSubmittersSelection", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("AllowSubmittersSelection") = value
        End Set
    End Property
    Private Property CommitteesCount As Integer Implements IViewEvaluationCommitteesEditor.CommitteesCount
        Get
            Return ViewStateOrDefault("CommitteesCount", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("CommitteesCount") = value
        End Set
    End Property
    Private Property UseDssMethods As Boolean Implements IViewEvaluationCommitteesEditor.UseDssMethods
        Get
            Return ViewStateOrDefault("UseDssMethods", False)
        End Get
        Set(ByVal value As Boolean)
            ViewState("UseDssMethods") = value
        End Set
    End Property
    Private ReadOnly Property AllowUseOfDssMethods As Boolean Implements IViewEvaluationCommitteesEditor.AllowUseOfDssMethods
        Get
            Return SystemSettings.Presenter.AllowDssUse
        End Get
    End Property

    Private Property CurrentMethods As List(Of lm.Comol.Core.Dss.Domain.Templates.dtoSelectMethod) Implements IViewEvaluationCommitteesEditor.CurrentMethods
        Get
            Return ViewStateOrDefault("CurrentMethods", New List(Of lm.Comol.Core.Dss.Domain.Templates.dtoSelectMethod))
        End Get
        Set(value As List(Of lm.Comol.Core.Dss.Domain.Templates.dtoSelectMethod))
            ViewState("CurrentMethods") = value
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

    Private Sub EditEvaluationCommittees_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Me.Master.ShowDocType = True
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim keys As List(Of String) = (From key As String In Request.Cookies.AllKeys Where key.StartsWith("c4p-committees-") Select key).ToList

        'For Each key As String In keys
        '    Dim s As HttpCookie = Request.Cookies(key)
        '    s = s
        'Next


    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        CurrentPresenter.InitView(AllowUseOfDssMethods)
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

            .setHyperLink(HYPbackTop, True, True)
            .setHyperLink(HYPbackBottom, True, True)
            .setButton(BTNsaveCommitteesBottom, True, , , True)
            .setButton(BTNsaveCommitteesTop, True, , , True)
            .setButton(BTNaddCommitteeTop, True, , , True)
            .setLabel(LBcollapseAllTop)
            .setLabel(LBexpandAllTop)
            .setLabel(LBcriteriaHideTop)
            .setLabel(LBcriteriaShowTop)

            .setButton(BTNcreateCriterion, True, , , True)
            .setButton(BTNcloseCreateCriterionWindow, True, , , True)
        End With
    End Sub
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType) Implements IViewEvaluationCommitteesEditor.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ObjectType.CallForPaper, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewEvaluationCommitteesEditor.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub

    Private Sub DisplaySessionTimeout() Implements IViewEvaluationCommitteesEditor.DisplaySessionTimeout
        Dim idCommunity As Integer = PreloadIdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

        dto.DestinationUrl = lm.Comol.Modules.CallForPapers.Domain.RootObject.EditCommitteeSettings(PreloadIdCall, idCommunity, PreloadView)

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If
        webPost.Redirect(dto)
    End Sub

    Private Sub SetActionUrl(url As String) Implements IViewEvaluationCommitteesEditor.SetActionUrl
        Me.HYPbackBottom.Visible = True
        Me.HYPbackBottom.NavigateUrl = BaseUrl & url
        Me.HYPbackTop.Visible = True
        Me.HYPbackTop.NavigateUrl = BaseUrl & url
    End Sub

    Private Sub SetContainerName1(itemName As String) Implements IViewBaseEditEvaluationSettings.SetContainerName
        Dim title As String = Me.Resource.getValue("serviceTitle." & WizardEvaluationStep.GeneralSettings.ToString())
        Master.ServiceTitle = title
        Master.ServiceTitleToolTip = itemName
    End Sub
    Private Sub SetContainerName(communityName As String, callName As String) Implements IViewEvaluationCommitteesEditor.SetContainerName
        Dim title As String = Me.Resource.getValue("serviceTitle." & WizardEvaluationStep.GeneralSettings.ToString())
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

    Private Sub LoadUnknowCall(idCommunity As Integer, idModule As Integer, idCall As Long, type As lm.Comol.Modules.CallForPapers.Domain.CallForPaperType) Implements IViewEvaluationCommitteesEditor.LoadUnknowCall
        Me.MLVsettings.SetActiveView(VIWempty)
        CTRLemptyMessage.InitializeControl(Resource.getValue("LBnoCalls." & type.ToString), Helpers.MessageType.error)
    End Sub
    Public Sub HideErrorMessages() Implements IViewEvaluationCommitteesEditor.HideErrorMessages
        Me.CTRLmessages.Visible = False
    End Sub
    Private Sub DisplayError(err As EvaluationEditorErrors) Implements IViewEvaluationCommitteesEditor.DisplayError
        Me.CTRLmessages.Visible = (err <> EvaluationEditorErrors.None)
        Me.CTRLmessages.InitializeControl(Resource.getValue("EvaluationEditorErrors." & err.ToString), Helpers.MessageType.error)
    End Sub
    Private Sub DisplaySettingsSaved() Implements IViewEvaluationCommitteesEditor.DisplaySettingsSaved
        Me.CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayCommitteesSettingsSaved"), Helpers.MessageType.success)
    End Sub
    Private Sub LoadCommittees(committees As List(Of dtoCommittee)) Implements IViewEvaluationCommitteesEditor.LoadCommittees
        _CallAggrecationSettings = Nothing
        Me.MLVsettings.SetActiveView(VIWsettings)
        Me.RPTcommittees.DataSource = committees
        Me.RPTcommittees.DataBind()
    End Sub
    Private Sub LoadSubmitterTypes(submitters As List(Of lm.Comol.Modules.CallForPapers.Domain.dtoSubmitterType)) Implements IViewEvaluationCommitteesEditor.LoadSubmitterTypes
        Me.Availablesubmitters = submitters
    End Sub
    Private Function GetCommittees() As List(Of dtoCommittee) Implements IViewEvaluationCommitteesEditor.GetCommittees
        Dim dtoMehtod As dtoItemMethodSettings = Nothing
        If UseDssMethods Then
            dtoMehtod = GetCallDssSettings()
        End If
        Dim items As New List(Of dtoCommittee)
        Dim criteria As New List(Of dtoCriterion)
        For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTcommittees.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim committee As New dtoCommittee
            Dim oLiteral As Literal = row.FindControl("LTidCommittee")
            Dim oTextBox As TextBox = row.FindControl("TXBcommitteeName")
            committee.Id = CLng(oLiteral.Text)
            committee.Name = oTextBox.Text

            oTextBox = row.FindControl("TXBcommitteeDescription")
            committee.Description = oTextBox.Text

            Dim oCheck As CheckBox = row.FindControl("CBXadvancedSubmittersInfo")
            committee.ForAllSubmittersType = Not oCheck.Checked

            If oCheck.Checked Then
                Dim oSelect As HtmlSelect = row.FindControl("SLBsubmitters")
                committee.Submitters = (From i As ListItem In oSelect.Items Where i.Selected Select CLng(i.Value)).ToList()
            End If

            If UseDssMethods Then
                Dim oControl As UC_DssAggregationSelector = DirectCast(row.FindControl("CTRLcallAggregation"), UC_DssAggregationSelector)
                committee.UseDss = True
                committee.MethodSettings = oControl.GetSettings()
                If committee.MethodSettings.InheritsFromFather Then
                    committee.MethodSettings.IdMethod = dtoMehtod.IdMethod
                    committee.MethodSettings.IdRatingSet = dtoMehtod.IdRatingSet
                    If committee.MethodSettings.UseManualWeights Then
                        committee.MethodSettings.Error = committee.MethodSettings.Error
                    Else
                        committee.MethodSettings.Error = dtoMehtod.Error
                    End If
                    committee.MethodSettings.IsFuzzyMethod = dtoMehtod.IsFuzzyMethod
                    committee.MethodSettings.UseManualWeights = dtoMehtod.UseManualWeights
                    committee.MethodSettings.UseOrderedWeights = dtoMehtod.UseOrderedWeights
                End If
                If committee.MethodSettings.UseManualWeights Then
                    committee.WeightSettings.FuzzyMeWeights = committee.MethodSettings.FuzzyMeWeights
                    committee.WeightSettings.IsFuzzyWeight = committee.MethodSettings.IsFuzzyMethod
                    committee.WeightSettings.ManualWeights = committee.MethodSettings.UseManualWeights
                    committee.WeightSettings.IsValidFuzzyMeWeights = (committee.MethodSettings.Error = lm.Comol.Core.Dss.Domain.DssError.None)
                ElseIf Not dtoMehtod.UseManualWeights Then
                    Dim oInput As UC_FuzzyInput = DirectCast(row.FindControl("CTRLfuzzyInput"), UC_FuzzyInput)
                    committee.WeightSettings = oInput.GetSettings
                End If
            End If
            Dim oRepeater As Repeater = row.FindControl("RPTcriteria")
            criteria.AddRange(GetCriteria(oRepeater))

            items.Add(committee)
        Next
        For Each committee As dtoCommittee In items
            committee.Criteria = (From c In criteria Where c.IdCommittee = committee.Id Select c).ToList
        Next
        Return items
    End Function

    Private Function GetCommiteesDssMethods() As Dictionary(Of Long, dtoItemMethodSettings) Implements IViewEvaluationCommitteesEditor.GetCommiteesDssMethods
        Dim items As New Dictionary(Of Long, dtoItemMethodSettings)
        For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTcommittees.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim method As New dtoItemMethodSettings

            Dim oControl As UC_DssAggregationSelector = DirectCast(row.FindControl("CTRLcallAggregation"), UC_DssAggregationSelector)

            If items.ContainsKey(oControl.IdObjectItem) Then
                items(oControl.IdObjectItem) = oControl.GetSettings()
            Else
                items.Add(oControl.IdObjectItem, oControl.GetSettings())
            End If

            'Dim oInput As UC_FuzzyInput = DirectCast(e.Item.FindControl("CTRLfuzzyInput"), UC_FuzzyInput)
            'oInput.Visible = True
        Next
        Return items
    End Function

    Private Function GetCriteria(oRepeater As Repeater) As List(Of dtoCriterion)
        Dim items As New List(Of dtoCriterion)
        For Each row As RepeaterItem In (From r As RepeaterItem In oRepeater.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim criterion As New dtoCriterion
            Dim oLiteral As Literal = row.FindControl("LTidCriterion")
            Dim oTextBox As TextBox = row.FindControl("TXBcriterionName")

            Dim oControl As UC_EditCriterion = row.FindControl("CTRLeditCriterion")
            criterion = oControl.GetCriterion

            criterion.Id = CLng(oLiteral.Text)
            criterion.Name = oTextBox.Text
            Dim hidden As HtmlInputHidden = row.FindControl("HDNcommitteeOwner")

            If Not String.IsNullOrEmpty(hidden.Value) AndAlso IsNumeric(hidden.Value.Replace("committee_", "")) Then
                criterion.IdCommittee = CLng(hidden.Value.Replace("committee_", ""))
            End If
            hidden = row.FindControl("HDNdisplayOrder")
            If Not String.IsNullOrEmpty(hidden.Value) AndAlso IsNumeric(hidden.Value) Then
                criterion.DisplayOrder = CInt(hidden.Value)
            End If
            Dim oInput As UC_FuzzyInput = DirectCast(row.FindControl("CTRLfuzzyInput"), UC_FuzzyInput)
            criterion.WeightSettings = oInput.GetSettings

            items.Add(criterion)
        Next
        Return items
    End Function
    Private Sub ReloadEditor(url As String) Implements IViewEvaluationCommitteesEditor.ReloadEditor
        PageUtility.RedirectToUrl(url)
    End Sub
    'Private Sub InitializeAddFieldControl(idCall As Long) Implements IViewCallEditor.InitializeAddFieldControl
    '    Me.CTRLaddField.InitializeControl(idCall)
    'End Sub
    Private Sub InitializeAggregationMethods(methods As List(Of lm.Comol.Core.Dss.Domain.Templates.dtoSelectMethod), idDssMethod As Long, idRatingSet As Long, weightItems As List(Of lm.Comol.Core.Dss.Domain.Templates.dtoItemWeightBase)) Implements IViewEvaluationCommitteesEditor.InitializeAggregationMethods
        CTRLcallAggregation.Visible = True
        CTRLcallAggregation.Description = Resource.getValue("CallDssDescription")
        CTRLcallAggregation.TranslationSelectMethod = Resource.getValue("MethodTitleTranslation")
        CTRLcallAggregation.TranslationWeightsSetTitle = Resource.getValue("Call.TranslationWeightsSetTitle")
        CTRLcallAggregation.InitializeControl(0, methods, New lm.Comol.Core.Dss.Domain.Templates.dtoItemMethodSettings() With {.IdMethod = idDssMethod, .IdRatingSet = idRatingSet, .InheritsFromFather = False, .IsDefaultForChildren = True}, weightItems, False, Not AllowSave)
        CurrentMethods = methods
        _CallAggrecationSettings = Nothing
    End Sub
    Private Sub HideCallAggregationMethods(methods As List(Of lm.Comol.Core.Dss.Domain.Templates.dtoSelectMethod), idDssMethod As Long, idRatingSet As Long, weightItems As List(Of lm.Comol.Core.Dss.Domain.Templates.dtoItemWeightBase)) Implements IViewEvaluationCommitteesEditor.HideCallAggregationMethods
        CTRLcallAggregation.Visible = False
        'CTRLcallAggregation.Description = Resource.getValue("CallDssDescription")
        'CTRLcallAggregation.TranslationSelectMethod = Resource.getValue("MethodTitleTranslation")
        'CTRLcallAggregation.TranslationWeightsSetTitle = Resource.getValue("Call.TranslationWeightsSetTitle")
        'CTRLcallAggregation.InitializeControl(0, methods, New lm.Comol.Core.Dss.Domain.Templates.dtoItemMethodSettings() With {.IdMethod = idDssMethod, .IdRatingSet = idRatingSet, .InheritsFromFather = False, .IsDefaultForChildren = False}, weightItems, False, Not AllowSave)
        CurrentMethods = methods
        _CallAggrecationSettings = Nothing
    End Sub
    Private Function GetCallDssSettings() As dtoItemMethodSettings Implements IViewEvaluationCommitteesEditor.GetCallDssSettings
        Return CTRLcallAggregation.GetSettings()
    End Function
    Private Sub DisplayDssErrors(commitees As List(Of dtoCommittee)) Implements IViewEvaluationCommitteesEditor.DisplayDssErrors
        CTRLdssMessages.Visible = commitees.Any()
        If (commitees.Any) Then
            Dim message As String = Resource.getValue("DisplayDssErrors" & IIf(commitees.Count = 1, ".1", ""))
            message &= "<ul>"
            For Each committee As dtoCommittee In commitees
                Dim errors As String = ""
                message &= "<li>"
                message &= String.Format(Resource.getValue("CommiteeDssErrors"), committee.Name)
                If (committee.GetDssErrors().Any() AndAlso Not committee.GetCriteriaDssErrors().Any) Then
                    message &= String.Join(", ", committee.GetDssErrors().Select(Function(e) Resource.getValue("CommiteeDssError.DssError." & e.ToString)))
                Else
                    message &= String.Join(", ", committee.GetDssErrors().Select(Function(e) Resource.getValue("CommiteeDssError.DssError." & e.ToString)))
                    message &= "<ul>"
                    For Each criterion As dtoCriterion In committee.Criteria.Where(Function(c) c.HasDssErrors())
                        message &= "<li>"
                        message &= String.Format(Resource.getValue("CriterionDssErrors"), criterion.Name)
                        message &= String.Join(", ", criterion.GetDssErrors().Select(Function(e) Resource.getValue("CommiteeDssError.DssError." & e.ToString)))
                        message &= "</li>"
                    Next
                    message &= "</ul>"
                End If

                message &= "</li>"
            Next
            message &= "</ul>"
            CTRLdssMessages.InitializeControl(message, Helpers.MessageType.alert)
        End If
    End Sub
#End Region

#Region "Internal"

#Region "Repeater"
    Private _tempCommitteeSettings As dtoItemMethodSettings
    Private _CallAggrecationSettings As dtoItemMethodSettings
    Private Sub RPTcommittees_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTcommittees.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim committee As dtoCommittee = DirectCast(e.Item.DataItem, dtoCommittee)

            Dim oLiteral As Literal = e.Item.FindControl("LTidCommittee")
            oLiteral.Text = committee.Id
            Dim oLabel As Label = e.Item.FindControl("LBcommitteeName_t")
            Me.Resource.setLabel(oLabel)
            Dim oTextBox As TextBox = e.Item.FindControl("TXBcommitteeName")
            oTextBox.Text = committee.Name
            oTextBox.Enabled = AllowSave OrElse AllowSaveBaseInfo

            oLabel = e.Item.FindControl("LBcommitteeDescription_t")
            Me.Resource.setLabel(oLabel)
            oTextBox = e.Item.FindControl("TXBcommitteeDescription")
            oTextBox.Text = committee.Description
            oTextBox.Enabled = AllowSave OrElse AllowSaveBaseInfo

            oLabel = e.Item.FindControl("LBmoveCommittee")
            oLabel.ToolTip = Resource.getValue("LBmoveCommittee.Text")
            oLabel.Visible = (AllowSave OrElse AllowSaveBaseInfo) AndAlso (CommitteesCount > 1)

            Dim oButton As Button = e.Item.FindControl("BTNaddCriteria")
            oButton.CommandArgument = committee.Id
            oButton.Visible = AllowSave
            Resource.setButton(oButton, True)

            oButton = e.Item.FindControl("BTNdeleteCommittee")
            oButton.CommandArgument = committee.Id
            oButton.Visible = AllowSave
            Resource.setButton(oButton, True)

            Dim hyperlink As HyperLink = e.Item.FindControl("HYPtoTopCommittee")
            hyperlink.Visible = (committee.Criteria.Count > 2)
            Resource.setHyperLink(hyperlink, True, True)
            hyperlink.NavigateUrl = "#committee_" & committee.Id.ToString


            oLabel = e.Item.FindControl("LBcommitteeSubmitters_t")
            Me.Resource.setLabel(oLabel)

            Dim oCheck As CheckBox = e.Item.FindControl("CBXadvancedSubmittersInfo")
            Dim oHtmlControl As HtmlGenericControl = Nothing
            If UseDssMethods Then
                oCheck.Checked = False
            Else
                oCheck.Checked = Not committee.ForAllSubmittersType
                Dim oSelect As HtmlSelect = e.Item.FindControl("SLBsubmitters")
                oSelect.DataSource = Availablesubmitters
                oSelect.DataTextField = "Name"
                oSelect.DataValueField = "Id"
                oSelect.DataBind()
                For Each idSubmitter As Long In committee.Submitters
                    Dim oListItem As ListItem = oSelect.Items.FindByValue(idSubmitter)
                    If Not IsNothing(oListItem) Then
                        oListItem.Selected = True
                    End If
                Next
                oSelect.Attributes.Add("data-placeholder", Resource.getValue("Submitters.data-placeholder"))
                oSelect.Disabled = Not AllowSave
                oHtmlControl = e.Item.FindControl("SPNcommitteeSubmittersSelectAll")
                oHtmlControl.Attributes.Add("title", Resource.getValue("SPNcommitteeSubmittersSelectAll.ToolTip"))
                oHtmlControl.Visible = AllowSave
                oHtmlControl = e.Item.FindControl("SPNcommitteeSubmittersSelectNone")
                oHtmlControl.Attributes.Add("title", Resource.getValue("SPNcommitteeSubmittersSelectNone.ToolTip"))
                oHtmlControl.Visible = AllowSave
            End If
            oHtmlControl = e.Item.FindControl("DVsubmitterTypes")
            oHtmlControl.Visible = AllowSubmittersSelection


            If committee.UseDss OrElse UseDssMethods Then
                If IsNothing(_CallAggrecationSettings) Then
                    _CallAggrecationSettings = CTRLcallAggregation.GetSettings()
                End If
                Dim oControl As UC_DssAggregationSelector = DirectCast(e.Item.FindControl("CTRLcallAggregation"), UC_DssAggregationSelector)
                oControl.Visible = True
                oControl.IdObjectItem = committee.Id
                oControl.Description = Resource.getValue("CommitteeDssDescription")
                oControl.TranslationSelectRating = Resource.getValue("CommitteeSelectRatingTranslation")
                oControl.TranslationInherits = Resource.getValue("CommitteeInheritsTranslation")
                oControl.TranslationWeightsSetTitle = Resource.getValue("Committee.TranslationWeightsSetTitle")
                oControl.InitializeControl(_CallAggrecationSettings.IdMethod, CurrentMethods, committee.MethodSettings, committee.BaseWeights, CommitteesCount > 1, Not AllowSave)


                Dim callSettings As dtoItemMethodSettings = _CallAggrecationSettings
                Dim settings As dtoItemMethodSettings = Nothing
                If committee.MethodSettings.InheritsFromFather Then
                    settings = callSettings
                Else
                    settings = oControl.GetSettings()
                End If
                _tempCommitteeSettings = settings
                Dim oInput As UC_FuzzyInput = DirectCast(e.Item.FindControl("CTRLfuzzyInput"), UC_FuzzyInput)
                If (callSettings.UseManualWeights) OrElse CommitteesCount < 2 Then
                    oInput.Visible = False
                ElseIf CommitteesCount > 1 Then
                    oInput.Visible = True
                    oInput.IdObjectItem = committee.Id
                    oInput.TranslationWeightChoose = Resource.getValue("CommitteeTranslationWeightChoose")
                    oInput.TranslationWeightTitle = Resource.getValue("CommitteeTranslationWeightTitle")

                    Dim method As dtoSelectMethod = CurrentMethods.Where(Function(m) m.Id = settings.IdMethod).FirstOrDefault()
                    If Not IsNothing(method) Then
                        Dim rating As dtoSelectRatingSet = method.RatingSets.Where(Function(r) r.Id = settings.IdRatingSet).FirstOrDefault
                        If IsNothing(rating) Then
                            oInput.InitializeDisabledControl()
                        Else
                            oInput.InitializeControl(method, rating, committee.WeightSettings, Not AllowSave)
                        End If
                    Else
                        oInput.InitializeDisabledControl()
                    End If
                End If
            End If
            Dim oRepeater As Repeater = DirectCast(e.Item.FindControl("RPTcriteria"), Repeater)
            oRepeater.DataSource = committee.Criteria
            oRepeater.DataBind()
        End If
    End Sub
    Private Sub RPTcommittees_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTcommittees.ItemCommand
        Dim idCommittee As Long = 0
        If Not String.IsNullOrEmpty(e.CommandArgument) AndAlso IsNumeric(e.CommandArgument) Then
            idCommittee = CLng(e.CommandArgument)
        End If
        If e.CommandName = "virtualDelete" Then
            If isValidOperation() Then
                CurrentPresenter.RemoveCommitte(GetCommittees(), UseDssMethods, idCommittee)
            End If
        ElseIf e.CommandName = "addCriteria" Then
            If isValidOperation() Then
                HDNidCommittee.Value = idCommittee
                If UseDssMethods Then
                    Dim oControl As UC_DssAggregationSelector = DirectCast(e.Item.FindControl("CTRLcallAggregation"), UC_DssAggregationSelector)
                    Dim settings As dtoItemMethodSettings = oControl.GetSettings()
                    If settings.InheritsFromFather Then
                        settings = CTRLcallAggregation.GetSettings()
                    End If
                    CTRLaddCriterion.InitializeControl(IdCall, settings, CurrentMethods.Where(Function(m) m.Id = settings.IdMethod).FirstOrDefault())
                Else
                    CTRLaddCriterion.InitializeControl(IdCall, False)
                End If
                LTscriptOpen.Visible = True
            End If
        End If
    End Sub

    Protected Sub RPTcriteria_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim criterion As dtoCriterion = DirectCast(e.Item.DataItem, dtoCriterion)

            Dim oLiteral As Literal = e.Item.FindControl("LTidCriterion")
            oLiteral.Text = criterion.Id
            Dim oLabel As Label = e.Item.FindControl("LBcriterionName_t")
            Me.Resource.setLabel(oLabel)
            Dim oTextBox As TextBox = e.Item.FindControl("TXBcriterionName")
            oTextBox.Text = criterion.Name
            oTextBox.Enabled = AllowSave OrElse AllowSaveBaseInfo

            oLabel = e.Item.FindControl("LBcriterionType")
            oLabel.Text = Resource.getValue("Criterion.CriterionType." & criterion.Type.ToString)


            oLabel = e.Item.FindControl("LBmoveCriterion")
            oLabel.ToolTip = Resource.getValue("LBmoveCriterion.Text")
            oLabel.Visible = AllowSave OrElse AllowSaveBaseInfo

            Dim oButton As Button = e.Item.FindControl("BTNdeleteCriterion")
            oButton.CommandArgument = criterion.Id
            oButton.Visible = AllowSave
            Resource.setButton(oButton, True)

            Dim oControl As UC_EditCriterion = e.Item.FindControl("CTRLeditCriterion")
            oControl.InitializeControl(criterion, AllowSave, AllowSaveBaseInfo)

            Dim hidden As HtmlInputHidden = e.Item.FindControl("HDNcommitteeOwner")
            hidden.Value = "committee_" & criterion.IdCommittee
            hidden = e.Item.FindControl("HDNdisplayOrder")
            hidden.Value = criterion.DisplayOrder

            If UseDssMethods Then
                If Not IsNothing(_tempCommitteeSettings) Then
                    Dim oInput As UC_FuzzyInput = DirectCast(e.Item.FindControl("CTRLfuzzyInput"), UC_FuzzyInput)
                    If _tempCommitteeSettings.UseManualWeights Then
                        oInput.Visible = False
                    Else
                        oInput.Visible = True
                        oInput.IdObjectItem = criterion.Id
                        oInput.TranslationWeightChoose = Resource.getValue("CriterionTranslationWeightChoose")
                        oInput.TranslationWeightTitle = Resource.getValue("CriterionTranslationWeightTitle")

                        Dim settings As dtoItemMethodSettings = criterion.MethodSettings
                        If criterion.MethodSettings.InheritsFromFather Then
                            settings = CTRLcallAggregation.GetSettings()
                        End If

                        Dim method As dtoSelectMethod = CurrentMethods.Where(Function(m) m.Id = settings.IdMethod).FirstOrDefault()
                        If Not IsNothing(method) Then
                            Dim rating As dtoSelectRatingSet = method.RatingSets.Where(Function(r) r.Id = settings.IdRatingSet).FirstOrDefault
                            If IsNothing(rating) Then
                                oInput.InitializeDisabledControl()
                            Else
                                oInput.InitializeControl(method, rating, criterion.WeightSettings, Not AllowSave)
                            End If
                        Else
                            oInput.InitializeDisabledControl()
                        End If
                    End If
                End If
            End If
        End If
    End Sub
    Protected Sub RPTcriteria_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs)
        Dim idCriterion As Long = 0
        If Not String.IsNullOrEmpty(e.CommandArgument) AndAlso IsNumeric(e.CommandArgument) Then
            idCriterion = CLng(e.CommandArgument)
        End If
        If e.CommandName = "virtualDelete" Then
            Me.CurrentPresenter.RemoveCriterion(GetCommittees(), UseDssMethods, idCriterion)
        End If
    End Sub
#End Region

#Region "Criteria"
    Protected Sub AddOption(idCriterion As Long, name As String, value As Double)
        If isValidOperation() Then
            CurrentPresenter.AddOption(GetCommittees(), UseDssMethods, idCriterion, name, value)
        End If
    End Sub
    Protected Sub RemoveOption(idOption As Long)
        If isValidOperation() Then
            CurrentPresenter.RemoveOption(GetCommittees(), UseDssMethods, idOption)
        End If
    End Sub
    Protected Sub ChangeToIntegerType(idCriterion As Long, minValue As Integer, maxValue As Integer)
        If isValidOperation() Then
            ChangeCriterionType(idCriterion, CriterionType.IntegerRange)
        End If
    End Sub
    Protected Sub ChangeToDecimalType(idCriterion As Long, minValue As Decimal, maxValue As Decimal)
        If isValidOperation() Then
            ChangeCriterionType(idCriterion, CriterionType.DecimalRange)
        End If
    End Sub
    Private Sub ChangeCriterionType(idCriterion As Long, criterionType As lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType)

        For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTcommittees.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim oRepeater As Repeater = row.FindControl("RPTcriteria")
            For Each cRow As RepeaterItem In (From r As RepeaterItem In oRepeater.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
                Dim oLiteral As Literal = cRow.FindControl("LTidCriterion")
                If oLiteral.Text = idCriterion.ToString() Then
                    Dim oLabel As Label = cRow.FindControl("LBcriterionType")
                    oLabel.Text = Resource.getValue("Criterion.CriterionType." & criterionType.ToString)
                    Exit Sub
                End If
            Next
        Next
    End Sub
#End Region

#Region "DSS"
    Private Function GetBaseCommittees(Optional ByVal idObject As Long = 0) As List(Of dtoCommittee)
        Dim items As New List(Of dtoCommittee)
        Dim criteria As New List(Of dtoCriterion)
        For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTcommittees.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim committee As New dtoCommittee
            Dim oLiteral As Literal = row.FindControl("LTidCommittee")
            Dim oTextBox As TextBox = row.FindControl("TXBcommitteeName")
            committee.Id = CLng(oLiteral.Text)
            committee.UseDss = True
            committee.Name = oTextBox.Text
            If idObject > 0 Then
                If committee.Id = idObject Then
                    '
                    'Dim dtoMehtod As dtoItemMethodSettings = Nothing
                    'If UseDssMethods Then
                    '    dtoMehtod = GetCallDssSettings()
                    'End If
                    'If UseDssMethods Then
                    '    Dim oControl As UC_DssAggregationSelector = DirectCast(row.FindControl("CTRLcallAggregation"), UC_DssAggregationSelector)

                    '    committee.MethodSettings = oControl.GetSettings()
                    '    If committee.MethodSettings.InheritsFromFather Then
                    '        committee.MethodSettings.IdMethod = dtoMehtod.IdMethod
                    '        committee.MethodSettings.IdRatingSet = dtoMehtod.IdRatingSet
                    '        If committee.MethodSettings.UseManualWeights Then
                    '            committee.MethodSettings.Error = dtoMehtod.Error Or committee.MethodSettings.Error
                    '        Else
                    '            committee.MethodSettings.Error = dtoMehtod.Error
                    '        End If
                    '        committee.MethodSettings.IsFuzzyMethod = dtoMehtod.IsFuzzyMethod
                    '        committee.MethodSettings.UseManualWeights = dtoMehtod.UseManualWeights
                    '        committee.MethodSettings.UseOrderedWeights = dtoMehtod.UseOrderedWeights
                    '    End If
                    '    If committee.MethodSettings.UseManualWeights Then
                    '        committee.WeightSettings.FuzzyMeWeights = committee.MethodSettings.FuzzyMeWeights
                    '        committee.WeightSettings.IsFuzzyWeight = committee.MethodSettings.IsFuzzyMethod
                    '        committee.WeightSettings.ManualWeights = committee.MethodSettings.UseManualWeights
                    '        committee.WeightSettings.IsValidFuzzyMeWeights = (committee.MethodSettings.Error = lm.Comol.Core.Dss.Domain.DssError.None)
                    '    Else
                    '        Dim oInput As UC_FuzzyInput = DirectCast(row.FindControl("CTRLfuzzyInput"), UC_FuzzyInput)
                    '        committee.WeightSettings = oInput.GetSettings
                    '    End If
                    'End If
                    Dim oRepeater As Repeater = row.FindControl("RPTcriteria")
                    criteria.AddRange(GetBaseCriteria(oRepeater))

                    items.Add(committee)
                    Exit For
                End If
            Else
                Dim oRepeater As Repeater = row.FindControl("RPTcriteria")
                criteria.AddRange(GetBaseCriteria(oRepeater))

                items.Add(committee)
            End If
        Next
        For Each committee As dtoCommittee In items
            committee.Criteria = (From c In criteria Where c.IdCommittee = committee.Id Select c).ToList
        Next
        Return items
    End Function
    Private Function GetBaseCriteria(oRepeater As Repeater) As List(Of dtoCriterion)
        Dim items As New List(Of dtoCriterion)
        For Each row As RepeaterItem In (From r As RepeaterItem In oRepeater.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim criterion As New dtoCriterion
            Dim oLiteral As Literal = row.FindControl("LTidCriterion")
            Dim oTextBox As TextBox = row.FindControl("TXBcriterionName")

            Dim oControl As UC_EditCriterion = row.FindControl("CTRLeditCriterion")
            criterion = oControl.GetCriterion

            criterion.Id = CLng(oLiteral.Text)
            criterion.Name = oTextBox.Text
            Dim hidden As HtmlInputHidden = row.FindControl("HDNcommitteeOwner")

            If Not String.IsNullOrEmpty(hidden.Value) AndAlso IsNumeric(hidden.Value.Replace("committee_", "")) Then
                criterion.IdCommittee = CLng(hidden.Value.Replace("committee_", ""))
            End If
            hidden = row.FindControl("HDNdisplayOrder")
            If Not String.IsNullOrEmpty(hidden.Value) AndAlso IsNumeric(hidden.Value) Then
                criterion.DisplayOrder = CInt(hidden.Value)
            End If
            items.Add(criterion)
        Next
        Return items
    End Function
    Private Sub CTRLcallAggregation_RequireWeights(idObject As Long, isFuzzyWeights As Boolean, orderedWeights As Boolean, ByRef items As List(Of dtoItemWeightBase), obj As UC_DssAggregationSelector) Handles CTRLcallAggregation.RequireWeights
        items = CurrentPresenter.RequireCommitteesWeight(IdCall, isFuzzyWeights, orderedWeights, GetBaseCommittees)
    End Sub
    Protected Sub RequireWeights(idObject As Long, isFuzzyWeights As Boolean, orderedWeights As Boolean, ByRef items As List(Of dtoItemWeightBase), obj As UC_DssAggregationSelector)
        items = CurrentPresenter.RequireCommitteeWeight(IdCall, isFuzzyWeights, orderedWeights, GetBaseCommittees(idObject).FirstOrDefault())
    End Sub
    Private Sub CTRLcallAggregation_SelectedMethod(callMethodSettings As dtoItemMethodSettings, obj As UC_DssAggregationSelector) Handles CTRLcallAggregation.SelectedMethod
        For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTcommittees.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim oControl As UC_DssAggregationSelector = DirectCast(row.FindControl("CTRLcallAggregation"), UC_DssAggregationSelector)
            Dim committeeMethodSettings As dtoItemMethodSettings = oControl.GetSettings()

            If committeeMethodSettings.InheritsFromFather Then
                If committeeMethodSettings.UseManualWeights <> callMethodSettings.UseManualWeights OrElse committeeMethodSettings.UseOrderedWeights <> callMethodSettings.UseOrderedWeights OrElse (committeeMethodSettings.IsFuzzyMethod <> callMethodSettings.IsFuzzyMethod AndAlso callMethodSettings.UseManualWeights) Then
                    committeeMethodSettings = callMethodSettings.Copy
                    committeeMethodSettings.InheritsFromFather = True
                    committeeMethodSettings.IsDefaultForChildren = False
                    committeeMethodSettings.FuzzyMeWeights = ""
                    oControl.InitializeControl(callMethodSettings.IdMethod, CurrentMethods, committeeMethodSettings, CurrentPresenter.RequireCommitteeWeight(IdCall, callMethodSettings.IsFuzzyMethod, callMethodSettings.UseOrderedWeights, GetBaseCommittees(oControl.IdObjectItem).FirstOrDefault()), CommitteesCount > 1, Not AllowSave)
                End If
                Dim method As dtoSelectMethod = CurrentMethods.Where(Function(m) m.Id = callMethodSettings.IdMethod).FirstOrDefault()
                Dim oInput As UC_FuzzyInput = DirectCast(row.FindControl("CTRLfuzzyInput"), UC_FuzzyInput)
                If callMethodSettings.UseManualWeights Then
                    oInput.Visible = False
                Else
                    oInput.Visible = True
                    If Not IsNothing(method) Then
                        Dim rating As dtoSelectRatingSet = method.RatingSets.Where(Function(r) r.Id = callMethodSettings.IdRatingSet).FirstOrDefault
                        If IsNothing(rating) Then
                            oInput.InitializeDisabledControl()
                        Else
                            oInput.InitializeControl(method, callMethodSettings.IdRatingSet, Not AllowSave)
                        End If
                    Else
                        oInput.InitializeDisabledControl()
                    End If
                End If

                UpdateChildrenCriteria(DirectCast(row.FindControl("RPTcriteria"), Repeater), callMethodSettings, method)
            Else
                Dim method As dtoSelectMethod = CurrentMethods.Where(Function(m) m.Id = callMethodSettings.IdMethod).FirstOrDefault()
                Dim oInput As UC_FuzzyInput = DirectCast(row.FindControl("CTRLfuzzyInput"), UC_FuzzyInput)
                If callMethodSettings.UseManualWeights Then
                    oInput.Visible = False
                ElseIf callMethodSettings.IdRatingSet <> oInput.IdRatingSet Then
                    oInput.Visible = True
                    If Not IsNothing(method) Then
                        Dim rating As dtoSelectRatingSet = method.RatingSets.Where(Function(r) r.Id = callMethodSettings.IdRatingSet).FirstOrDefault
                        If IsNothing(rating) Then
                            oInput.InitializeDisabledControl()
                        Else
                            oInput.InitializeControl(method, callMethodSettings.IdRatingSet, Not AllowSave)
                        End If
                    Else
                        oInput.InitializeDisabledControl()
                    End If
                End If
            End If
        Next
    End Sub
    Protected Sub SelectedMethod(settings As dtoItemMethodSettings, obj As UC_DssAggregationSelector)
        If isValidOperation() Then
            Dim oInput As UC_FuzzyInput = DirectCast(obj.Parent.FindControl("CTRLfuzzyInput"), UC_FuzzyInput)
            If settings.InheritsFromFather Then
                settings = CTRLcallAggregation.GetSettings()
            End If

            Dim method As dtoSelectMethod = CurrentMethods.Where(Function(m) m.Id = settings.IdMethod).FirstOrDefault()
            If Not IsNothing(method) Then
                Dim rating As dtoSelectRatingSet = method.RatingSets.Where(Function(r) r.Id = settings.IdRatingSet).FirstOrDefault
                If IsNothing(rating) Then
                    oInput.InitializeDisabledControl()
                ElseIf rating.Id <> oInput.IdRatingSet Then
                    oInput.InitializeControl(method, rating.Id, Not AllowSave)
                End If
            Else
                oInput.InitializeDisabledControl()
            End If
            UpdateChildrenCriteria(DirectCast(obj.Parent.FindControl("RPTcriteria"), Repeater), settings, method)
        End If
    End Sub

    Private Sub UpdateChildrenCriteria(oRepeater As Repeater, callMethodSettings As dtoItemMethodSettings, method As dtoSelectMethod)
        For Each row As RepeaterItem In (From r As RepeaterItem In oRepeater.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim oInput As UC_FuzzyInput = DirectCast(row.FindControl("CTRLfuzzyInput"), UC_FuzzyInput)
            If callMethodSettings.UseManualWeights Then
                oInput.Visible = False
            Else
                oInput.Visible = True
                If Not IsNothing(method) Then
                    Dim rating As dtoSelectRatingSet = method.RatingSets.Where(Function(r) r.Id = callMethodSettings.IdRatingSet).FirstOrDefault()
                    If IsNothing(rating) Then
                        oInput.InitializeDisabledControl()
                    Else
                        oInput.InitializeControl(method, callMethodSettings.IdRatingSet, Not AllowSave)
                    End If
                Else
                    oInput.InitializeDisabledControl()
                End If
            End If
        Next
    End Sub
#End Region
#End Region

    Private Sub BTNsaveCommitteesTop_Click(sender As Object, e As System.EventArgs) Handles BTNsaveCommitteesBottom.Click, BTNsaveCommitteesTop.Click
        If isValidOperation() Then
            CurrentPresenter.SaveSettings(GetCommittees(), AllowUseOfDssMethods, UseDssMethods)
        End If
    End Sub
    Private Sub BTNaddCommitteeTop_Click(sender As Object, e As System.EventArgs) Handles BTNaddCommitteeTop.Click
        If isValidOperation() Then
            CurrentPresenter.AddCommittee(GetCommittees(), UseDssMethods, DefaultCommitteeName, DefaultCommitteeDescription)
        End If
    End Sub
    Protected Sub BTNcreateField_Click(sender As Object, e As System.EventArgs) Handles BTNcreateCriterion.Click
        If isValidOperation() Then
            Dim idCommittee As Long = 0
            If Not String.IsNullOrEmpty(Me.HDNidCommittee.Value) Then
                idCommittee = CLng(Me.HDNidCommittee.Value.Replace("section-", ""))
                Dim settings As dtoItemMethodSettings = Nothing
                If UseDssMethods Then
                    settings = GetCallDssSettings()
                End If
                Dim criteria As List(Of BaseCriterion) = Me.CTRLaddCriterion.CreateCriteria(GetCommittees(), settings, idCommittee)
                If (criteria.Count > 0) Then
                    PageUtility.RedirectToUrl(lm.Comol.Modules.CallForPapers.Domain.RootObject.CriterionAddedToCommittee(criteria(0).Id, IdCall, IdCommunity, PreloadView))
                Else
                    Me.LTscriptOpen.Visible = False
                End If
            End If
        End If

    End Sub
    Protected Sub BTNcloseCreateCriterionWindow_Click(sender As Object, e As System.EventArgs) Handles BTNcloseCreateCriterionWindow.Click
        Me.LTscriptOpen.Visible = False
    End Sub

    Private Sub RedirectToUrl(url As String) Implements IViewBaseEditEvaluationSettings.RedirectToUrl
        PageUtility.RedirectToUrl(url)
    End Sub


   
End Class