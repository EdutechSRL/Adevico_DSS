Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation.Evaluation
Imports lm.Comol.Modules.CallForPapers.Domain.Evaluation
Imports lm.ActionDataContract
Imports System.Linq
Public Class ManageEvaluations
    Inherits PageBase
    Implements IViewManageEvaluations

#Region "Context"
    Private _Presenter As ManageEvaluationsPresenter
    Private ReadOnly Property CurrentPresenter() As ManageEvaluationsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ManageEvaluationsPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property Portalname As String Implements IViewManageEvaluations.Portalname
        Get
            Return Resource.getValue("Portalname")
        End Get
    End Property
    Private ReadOnly Property PreloadIdCall As Long Implements IViewManageEvaluations.PreloadIdCall
        Get
            If IsNumeric(Me.Request.QueryString("idCall")) Then
                Return CLng(Me.Request.QueryString("idCall"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewManageEvaluations.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadView As lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters Implements IViewManageEvaluations.PreloadView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters).GetByString(Request.QueryString("View"), lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters.SubmissionClosed)
        End Get
    End Property
    Private Property AllowSave As Boolean Implements IViewManageEvaluations.AllowSave
        Get
            Return ViewStateOrDefault("AllowSave", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowSave") = value
        End Set
    End Property
    Private Property AllowSaveStatus As Boolean Implements IViewManageEvaluations.AllowSaveStatus
        Get
            Return ViewStateOrDefault("AllowSaveStatus", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowSaveStatus") = value
            Me.BTNopenCloseEvaluationsTop.Visible = value
            Me.BTNopenCloseEvaluationsBottom.Visible = value
        End Set
    End Property
    Private Property IdCall As Long Implements IViewManageEvaluations.IdCall
        Get
            Return ViewStateOrDefault("IdCall", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdCall") = value
        End Set
    End Property
    Private Property IdCommunity As Integer Implements IViewManageEvaluations.IdCommunity
        Get
            Return ViewStateOrDefault("IdCommunity", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCommunity") = value
        End Set
    End Property
    Private Property IdCallModule As Integer Implements IViewManageEvaluations.IdCallModule
        Get
            Return ViewStateOrDefault("IdCallModule", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCallModule") = value
        End Set
    End Property

    Private Property FilterByName As String Implements IViewManageEvaluations.FilterByName
        Get
            Return ViewStateOrDefault("FilterByName", "")
        End Get
        Set(value As String)
            ViewState("FilterByName") = value
        End Set
    End Property
    Private Property FilterByType As Long Implements IViewManageEvaluations.FilterByType
        Get
            Return ViewStateOrDefault("FilterByType", -4)
        End Get
        Set(value As Long)
            ViewState("FilterByType") = value
        End Set
    End Property
    Private Property DisplayByEvaluator As Boolean Implements IViewManageEvaluations.DisplayByEvaluator
        Get
            Return ViewStateOrDefault("DisplayByEvaluator", True)
        End Get
        Set(value As Boolean)
            ViewState("DisplayByEvaluator") = value
            Me.HYPviewSubmissions.Visible = Not value
            Me.LBnavigateToCommission.Visible = Not value
            If value Then

                Me.LNBdisplayByEvaluations.CssClass = Replace(Me.LNBdisplayByEvaluations.CssClass, "active", "normal")
                Me.LNBdisplayByEvaluators.CssClass = Replace(Me.LNBdisplayByEvaluators.CssClass, "normal", "active")
                DVnavigation.Attributes.Add("class", Replace(Me.DVnavigation.Attributes("class"), "bysubmission", "byevaluator"))
                Me.DVsubmissionFilters.Visible = False
            Else
                Me.LNBdisplayByEvaluations.CssClass = Replace(Me.LNBdisplayByEvaluations.CssClass, "normal", "active")
                Me.LNBdisplayByEvaluators.CssClass = Replace(Me.LNBdisplayByEvaluators.CssClass, "active", "normal")
                DVnavigation.Attributes.Add("class", Replace(Me.DVnavigation.Attributes("class"), "byevaluator", "bysubmission"))
            End If
        End Set
    End Property
    Private Property EndEvaluationOn As Date? Implements IViewManageEvaluations.EndEvaluationOn
        Get
            Return RDPendEvaluationOn.SelectedDate
        End Get
        Set(value As Date?)
            RDPendEvaluationOn.SelectedDate = value
            If value.HasValue Then
                If value.Value < Now Then
                    RDPendEvaluationOn.MinDate = value.Value
                Else
                    RDPendEvaluationOn.MinDate = Now
                End If
            Else
                RDPendEvaluationOn.MinDate = Now
            End If
        End Set
    End Property
    Protected Property CurrentAction As lm.Comol.Modules.CallForPapers.Domain.ManageEvaluationsAction Implements IViewManageEvaluations.CurrentAction
        Get
            Return ViewStateOrDefault("CurrentAction", IIf(LNBopenEvaluations.Enabled, lm.Comol.Modules.CallForPapers.Domain.ManageEvaluationsAction.OpenAll, IIf(LNBcloseEvaluations.Enabled, lm.Comol.Modules.CallForPapers.Domain.ManageEvaluationsAction.CloseAll, lm.Comol.Modules.CallForPapers.Domain.ManageEvaluationsAction.None)))
        End Get
        Set(value As lm.Comol.Modules.CallForPapers.Domain.ManageEvaluationsAction)
            ViewState("CurrentAction") = value
            Select Case value
                Case lm.Comol.Modules.CallForPapers.Domain.ManageEvaluationsAction.OpenAll
                    If LNBopenEvaluations.Enabled Then
                        Me.LNBopenEvaluations.CssClass = Replace(Me.LNBopenEvaluations.CssClass, "disabled", "active")
                    End If
                    Me.LNBopenEvaluations.CssClass = Replace(Me.LNBopenEvaluations.CssClass, "normal", "active")
                    Me.LNBcloseEvaluations.CssClass = Replace(Me.LNBcloseEvaluations.CssClass, "active", "normal")
                    Me.Resource.setButtonByValue(BTNopenCloseEvaluationsBottom, "open")
                    Me.Resource.setButtonByValue(BTNopenCloseEvaluationsTop, "open")
                Case lm.Comol.Modules.CallForPapers.Domain.ManageEvaluationsAction.CloseAll
                    If LNBcloseEvaluations.Enabled Then
                        Me.LNBcloseEvaluations.CssClass = Replace(Me.LNBcloseEvaluations.CssClass, "disabled", "active")
                    End If
                    Me.LNBopenEvaluations.CssClass = Replace(Me.LNBopenEvaluations.CssClass, "active", "normal")
                    Me.LNBcloseEvaluations.CssClass = Replace(Me.LNBcloseEvaluations.CssClass, "normal", "active")
                    Me.Resource.setButtonByValue(BTNopenCloseEvaluationsBottom, "close")
                    Me.Resource.setButtonByValue(BTNopenCloseEvaluationsTop, "close")
                Case Else
                    Me.LNBopenEvaluations.CssClass = Replace(Me.LNBdisplayByEvaluations.CssClass, "normal", "disabled")
                    Me.LNBopenEvaluations.CssClass = Replace(Me.LNBdisplayByEvaluations.CssClass, "active", "disabled")
                    Me.LNBcloseEvaluations.CssClass = Replace(Me.LNBcloseEvaluations.CssClass, "normal", "disabled")
                    Me.LNBcloseEvaluations.CssClass = Replace(Me.LNBcloseEvaluations.CssClass, "active", "disabled")
            End Select
        End Set
    End Property
    Private Property CurrentPageSize As Int32 Implements IViewManageEvaluations.CurrentPageSize
        Get
            Return ViewStateOrDefault("CurrentPageSize", 25)
        End Get
        Set(value As Int32)
            Me.ViewState("CurrentPageSize") = value
        End Set
    End Property
    Private Property Pager As PagerBase Implements IViewManageEvaluations.Pager
        Get
            Return ViewStateOrDefault("Pager", New lm.Comol.Core.DomainModel.PagerBase With {.PageSize = CurrentPageSize})
        End Get
        Set(value As PagerBase)
            Me.ViewState("Pager") = value
            Me.PGgrid.Pager = value
            Me.PGgrid.Visible = Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize)
            'Me.DIVpageSize.Visible = (Not value.Count < Me.DefaultPageSize)
        End Set
    End Property
    Private ReadOnly Property AnonymousTranslation As String Implements IViewManageEvaluations.AnonymousTranslation
        Get
            Return Resource.getValue("AnonymousOwnerName")
        End Get
    End Property
    Private Property SelectAllItems As Boolean Implements IViewManageEvaluations.SelectAllItems
        Get
            Return ViewStateOrDefault("SelectAllItems", False)
        End Get
        Set(value As Boolean)
            ViewState("SelectAllItems") = value
        End Set
    End Property
    Private Property SelectedEvaluations As List(Of Long) Implements IViewManageEvaluations.SelectedEvaluations
        Get
            Return ViewStateOrDefault("SelectedEvaluations", New List(Of Long))
        End Get
        Set(value As List(Of Long))
            ViewState("SelectedEvaluations") = value
        End Set
    End Property
    Private ReadOnly Property SelectedIdSubmitterType As Long Implements IViewManageEvaluations.SelectedIdSubmitterType
        Get
            If DDLsubmittersType.Items.Count > 0 Then
                Return CLng(Me.DDLsubmittersType.SelectedValue)
            Else
                Return -1
            End If
        End Get
    End Property
    Private ReadOnly Property SelectedSubmissionName As String Implements IViewManageEvaluations.SelectedSubmissionName
        Get
            Return Me.TXBname.Text
        End Get
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
   
#End Region

    Private Sub EditEvaluationCommittees_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Me.Master.ShowDocType = True
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.DisplayByEvaluator Then
            Me.PGgrid.Pager = Pager
        End If
    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        Me.CurrentPresenter.InitView(SystemSettings.Presenter.AllowDssUse)
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
            .setButton(BTNopenCloseEvaluationsBottom, True, , , True)
            .setButton(BTNopenCloseEvaluationsTop, True, , , True)

            .setLabel(LBendEvaluationOn_t)
            .setLabel(LBopenCloseEvaluations_t)
            .setLinkButton(LNBcloseEvaluations, True, True)
            .setLinkButton(LNBopenEvaluations, True, True)

            .setLabel(LBopenCloseEvaluationByType_t)
            .setLinkButton(LNBdisplayByEvaluations, True, True)
            .setLinkButton(LNBdisplayByEvaluators, True, True)

            .setLabel(LBsubmissionName_t)
            .setLabel(LBsubmissionType_t)
            .setButton(BTNupdateManageEvaluationsTable, True, , , True)
            .setHyperLink(HYPviewSubmissions, True, True)
            .setHyperLink(HYPviewCommittees, True, True)
            .setHyperLink(HYPviewEvaluators, True, True)
            .setLiteral(LTnameTableHeader)
            .setButton(BTNupdateEvaluationEndDate, True, , , True)
            .setLinkButton(LNBselectItemsIntoAllPages, True, True)
            .setLinkButton(LNBunselectItemsIntoAllPages, True, True)
            .setLiteral(LTnameTableHeaderbySubmission)
        End With
    End Sub
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType) Implements IViewManageEvaluations.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ObjectType.CallForPaper, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewManageEvaluations.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewManageEvaluations.DisplaySessionTimeout
        Dim idCommunity As Integer = PreloadIdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

        dto.DestinationUrl = lm.Comol.Modules.CallForPapers.Domain.RootObject.EditCommiteeByStep(PreloadIdCall, idCommunity, WizardEvaluationStep.ManageEvaluations, PreloadView)

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If
        webPost.Redirect(dto)
    End Sub

    Private Sub SetActionUrl(url As String) Implements IViewManageEvaluations.SetActionUrl
        Me.HYPbackBottom.Visible = True
        Me.HYPbackBottom.NavigateUrl = BaseUrl & url
        Me.HYPbackTop.Visible = True
        Me.HYPbackTop.NavigateUrl = BaseUrl & url
    End Sub
    Private Sub SetContainerName(itemName As String) Implements IViewBaseEditEvaluationSettings.SetContainerName
        Dim title As String = Me.Resource.getValue("serviceTitle." & WizardEvaluationStep.ManageEvaluations.ToString())
        Master.ServiceTitle = title
        Master.ServiceTitleToolTip = itemName
    End Sub
    Private Sub SetContainerName(communityName As String, callName As String) Implements IViewManageEvaluations.SetContainerName
        Dim title As String = Me.Resource.getValue("serviceTitle." & WizardEvaluationStep.ManageEvaluations.ToString())
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

    Private Sub LoadUnknowCall(idCommunity As Integer, idModule As Integer, idCall As Long, type As lm.Comol.Modules.CallForPapers.Domain.CallForPaperType) Implements IViewManageEvaluations.LoadUnknowCall
        MLVsettings.SetActiveView(VIWempty)
        CTRLemptyMessage.InitializeControl(Resource.getValue("LBnoCalls." & type.ToString), Helpers.MessageType.error)
    End Sub
    Private Sub HideErrorMessages() Implements IViewManageEvaluations.HideErrorMessages
        Me.CTRLmessages.Visible = False
    End Sub

    'Private Sub DisplayError(err As EvaluationEditorErrors) Implements IViewManageEvaluations.DisplayError
    '    DisplayError(err, Helpers.MessageType.error)
    'End Sub
    'Private Sub DisplayWarning(err As EvaluationEditorErrors) Implements IViewManageEvaluations.DisplayWarning
    '    DisplayError(err, Helpers.MessageType.alert)
    'End Sub
    Private Sub DisplayError(err As EvaluationEditorErrors, errorType As Helpers.MessageType)
        Me.CTRLmessages.Visible = (err <> EvaluationEditorErrors.None)
        Me.CTRLmessages.InitializeControl(Resource.getValue("EvaluationEditorErrors." & err.ToString), errorType)
    End Sub

    Private Sub DisplayDateChangingError() Implements IViewManageEvaluations.DisplayDateChangingError
        Me.CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayDateChangingError"), Helpers.MessageType.error)
    End Sub
    Private Sub DisplayStatusEditingError(forClosing As Boolean) Implements IViewManageEvaluations.DisplayStatusEditingError
        Me.CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayStatusEditingError"), Helpers.MessageType.error)
    End Sub
    Private Sub DisplayEndEvaluationDateSaved() Implements IViewManageEvaluations.DisplayEndEvaluationDateSaved
        Me.CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayEndEvaluationDateSaved"), Helpers.MessageType.success)
    End Sub
    Private Sub DisplaySettingsSaved() Implements IViewManageEvaluations.DisplaySettingsSaved
        Me.CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayManageEvaluationsSaved." & CurrentAction.ToString()), Helpers.MessageType.success)
    End Sub
    Private Sub LoadAvailableActions(items As List(Of lm.Comol.Modules.CallForPapers.Domain.ManageEvaluationsAction)) Implements IViewManageEvaluations.LoadAvailableActions
        If items.Contains(lm.Comol.Modules.CallForPapers.Domain.ManageEvaluationsAction.OpenAll) Then
            Me.LNBopenEvaluations.CssClass = Replace(Me.LNBopenEvaluations.CssClass, "disabled", "normal")
            Me.LNBopenEvaluations.Enabled = True
        Else
            Me.LNBopenEvaluations.CssClass = Replace(Me.LNBopenEvaluations.CssClass, "active", "disabled")
            Me.LNBopenEvaluations.CssClass = Replace(Me.LNBopenEvaluations.CssClass, "normal", "disabled")
            Me.LNBopenEvaluations.Enabled = False
        End If
        If items.Contains(lm.Comol.Modules.CallForPapers.Domain.ManageEvaluationsAction.CloseAll) Then
            Me.LNBcloseEvaluations.CssClass = Replace(Me.LNBcloseEvaluations.CssClass, "disabled", "normal")
            Me.LNBcloseEvaluations.Enabled = True
        Else
            Me.LNBcloseEvaluations.CssClass = Replace(Me.LNBcloseEvaluations.CssClass, "active", "disabled")
            Me.LNBcloseEvaluations.CssClass = Replace(Me.LNBcloseEvaluations.CssClass, "normal", "disabled")
            Me.LNBcloseEvaluations.Enabled = False
        End If
    End Sub
    Private Sub DisplayNoEvaluations(showFilters As Boolean, showNavigation As Boolean, showDisplaySelector As Boolean) Implements lm.Comol.Modules.CallForPapers.Presentation.Evaluation.IViewManageEvaluations.DisplayNoEvaluations
        MLVdata.SetActiveView(VIWemptyData)
        LBviewEmptyData.Text = Resource.getValue("DisplayNoEvaluations.ForSelection")
        Me.DVdisplaySelector.Visible = showDisplaySelector
        Me.DVnavigation.Visible = showNavigation
        Me.DVsubmissionFilters.Visible = showFilters
    End Sub

#Region "ByEvaluator"
    Private Sub LoadItems(items As List(Of lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoBasicCommitteeItem)) Implements IViewManageEvaluations.LoadItems
        Me.DVnavigation.Visible = (items.Count > 0)
        Me.DVdisplaySelector.Visible = (items.Count > 0)

        If items.Count > 0 Then
            Me.MLVdata.SetActiveView(VIWbyEvaluators)
            Me.RPTselectByComittees.DataSource = items
            Me.RPTselectByComittees.DataBind()
        Else
            LBviewEmptyData.Text = Resource.getValue("DisplayNoEvaluations.ForSelection")
            Me.MLVdata.SetActiveView(VIWemptyData)
        End If
    End Sub
    Private Function GetSelectedItemsForEvaluators() As Dictionary(Of Long, List(Of Long)) Implements IViewManageEvaluations.GetSelectedItemsForEvaluators
        Dim items As New Dictionary(Of Long, List(Of Long))

        For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTselectByComittees.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim oLiteral As Literal = row.FindControl("LTidCommittee")
            Dim idCommittee As Long = CLng(oLiteral.Text)

            Dim idEvaluators As New List(Of Long)
            Dim oRepeater As Repeater = row.FindControl("RPTselectByEvaluators")
            For Each rEvaluator As RepeaterItem In (From r As RepeaterItem In oRepeater.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
                Dim oCheck As HtmlInputCheckBox = rEvaluator.FindControl("CBXevaluator")
                If oCheck.Checked Then
                    oLiteral = rEvaluator.FindControl("LTidEvaluator")
                    idEvaluators.Add(CLng(oLiteral.Text))
                End If
            Next
            If idEvaluators.Count > 0 Then
                items.Add(idCommittee, idEvaluators)
            End If
        Next
        Return items
    End Function
#End Region

#Region "BySubmission"
    Private Sub LoadSubmitterstype(submitters As List(Of lm.Comol.Modules.CallForPapers.Domain.dtoSubmitterType), dSubmitter As Long) Implements IViewManageEvaluations.LoadSubmitterstype
        DVsubmitterType.Visible = submitters.Count > 1
        If submitters.Count > 0 Then
            Me.DDLsubmittersType.DataSource = submitters
            Me.DDLsubmittersType.DataValueField = "Id"
            Me.DDLsubmittersType.DataTextField = "Name"
            Me.DDLsubmittersType.DataBind()
            If submitters.Count > 1 Then
                Me.DDLsubmittersType.Items.Insert(0, New ListItem(Resource.getValue("DDLsubmittersType.-1"), -1))
            End If
            If dSubmitter > 0 AndAlso submitters.Where(Function(s) s.Id = dSubmitter).Any() Then
                Me.DDLsubmittersType.SelectedValue = dSubmitter
            End If
        End If
    End Sub
    Private Sub LoadItems(items As List(Of dtoBasicSubmissionItem)) Implements IViewManageEvaluations.LoadItems
        Me.DVnavigation.Visible = (items.Count > 0)
        Me.DVdisplaySelector.Visible = (items.Count > 0)
        Me.DVsubmissionFilters.Visible = True
        If items.Count > 0 Then
            Me.MLVdata.SetActiveView(VIWbySubmissions)
            Me.RPTselectBySubmissions.DataSource = items
            Me.RPTselectBySubmissions.DataBind()
        Else
            LBviewEmptyData.Text = Resource.getValue("DisplayNoEvaluations.ForSelection")
            Me.MLVdata.SetActiveView(VIWemptyData)
        End If
    End Sub
    Private Function GetCurrentSumbissionsItems() As List(Of dtoSelectEvaluationItem) Implements IViewManageEvaluations.GetCurrentSumbissionsItems
        Dim items As New List(Of dtoSelectEvaluationItem)

        For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTselectBySubmissions.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim oRepeater As Repeater = row.FindControl("RPTselectByComittees")
            For Each rowComittee As RepeaterItem In (From r As RepeaterItem In oRepeater.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
                Dim oRepeaterEvaluators As Repeater = rowComittee.FindControl("RPTselectByEvaluators")
                For Each rEvaluator As RepeaterItem In (From r As RepeaterItem In oRepeaterEvaluators.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
                    Dim oCheck As HtmlInputCheckBox = rEvaluator.FindControl("CBXevaluator")
                    Dim oLiteral As Literal = rEvaluator.FindControl("LTidEvaluation")
                    items.Add(New dtoSelectEvaluationItem() With {.IdEvaluation = CLng(oLiteral.Text), .Selected = oCheck.Checked})
                Next
            Next
        Next
        Return items
    End Function
#End Region

#End Region

    Private Sub RedirectToUrl(url As String) Implements IViewBaseEditEvaluationSettings.RedirectToUrl
        PageUtility.RedirectToUrl(url)
    End Sub

#Region "manage Actions"
    Private Sub LNBcloseEvaluations_Click(sender As Object, e As System.EventArgs) Handles LNBcloseEvaluations.Click
        Me.HideErrorMessages()
        CurrentPresenter.LoadItems(lm.Comol.Modules.CallForPapers.Domain.ManageEvaluationsAction.CloseAll, DisplayByEvaluator, False)
    End Sub
    Private Sub LNBopenEvaluations_Click(sender As Object, e As System.EventArgs) Handles LNBopenEvaluations.Click
        Me.HideErrorMessages()
        CurrentPresenter.LoadItems(lm.Comol.Modules.CallForPapers.Domain.ManageEvaluationsAction.OpenAll, DisplayByEvaluator, False)
    End Sub

    Private Sub LNBdisplayByEvaluations_Click(sender As Object, e As System.EventArgs) Handles LNBdisplayByEvaluations.Click
        Me.HideErrorMessages()
        CurrentPresenter.LoadItems(CurrentAction, False, True)
    End Sub

    Private Sub LNBdisplayByEvaluators_Click(sender As Object, e As System.EventArgs) Handles LNBdisplayByEvaluators.Click
        Me.HideErrorMessages()
        CurrentPresenter.LoadItems(CurrentAction, True, True)
    End Sub
#End Region

#Region "Manage filters"
    Private Sub BTNupdateManageEvaluationsTable_Click(sender As Object, e As System.EventArgs) Handles BTNupdateManageEvaluationsTable.Click
        Me.HideErrorMessages()
        Me.CurrentPresenter.LoadItemsBySubmissions(CurrentAction, False, TXBname.Text, SelectedIdSubmitterType, 0, Me.CurrentPageSize)
    End Sub

    Private Sub DDLsubmittersType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLsubmittersType.SelectedIndexChanged
        Me.HideErrorMessages()
        Me.CurrentPresenter.LoadItemsBySubmissions(CurrentAction, False, TXBname.Text, DDLsubmittersType.SelectedValue, 0, Me.CurrentPageSize)
    End Sub
    Private Sub PGgrid_OnPageSelected() Handles PGgrid.OnPageSelected
        Me.HideErrorMessages()
        Me.CurrentPresenter.LoadItemsBySubmissions(CurrentAction, False, FilterByName, FilterByType, Me.PGgrid.Pager.PageIndex, Me.CurrentPageSize)
    End Sub
#End Region

    Private Sub BTNopenCloseEvaluationsBottom_Click(sender As Object, e As System.EventArgs) Handles BTNopenCloseEvaluationsBottom.Click, BTNopenCloseEvaluationsTop.Click
        Me.HideErrorMessages()
        If Me.DisplayByEvaluator Then
            Me.CurrentPresenter.SaveSettings(CurrentAction, GetSelectedItemsForEvaluators())
        Else
            Me.CurrentPresenter.SaveSettings(CurrentAction, Me.SelectAllItems)
        End If
    End Sub

    Private Sub RPTselectByComittees_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTselectByComittees.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oLiteral As Literal = e.Item.FindControl("LTcommitteNameHeader")
            Resource.setLiteral(oLiteral)
        End If
    End Sub
  
  
    Private Sub BTNupdateEvaluationEndDate_Click(sender As Object, e As System.EventArgs) Handles BTNupdateEvaluationEndDate.Click
        Me.HideErrorMessages()
        Me.CurrentPresenter.SaveSettings(RDPendEvaluationOn.SelectedDate)
    End Sub

#Region "BySubmission page control"
    Private Sub LNBselectItemsIntoAllPages_Click(sender As Object, e As System.EventArgs) Handles LNBselectItemsIntoAllPages.Click
        Me.HideErrorMessages()
        Me.CurrentPresenter.EditItemsSelection(True)
    End Sub
    Private Sub LNBunselectItemsIntoAllPages_Click(sender As Object, e As System.EventArgs) Handles LNBunselectItemsIntoAllPages.Click
        Me.HideErrorMessages()
        Me.CurrentPresenter.EditItemsSelection(False)
    End Sub
    Private Sub RPTselectBySubmissions_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTselectBySubmissions.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oLiteral As Literal = e.Item.FindControl("LTsubmissionNameHeader")
            Resource.setLiteral(oLiteral)
        End If
    End Sub


    Protected Sub RPTselectByComitteesInternal_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oLiteral As Literal = e.Item.FindControl("LTcommitteNameHeader")
            Resource.setLiteral(oLiteral)
        End If
    End Sub
    Protected Sub RPTselectByEvaluators_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim item As dtoComitteeEvaluatorItem = DirectCast(e.Item.DataItem, dtoComitteeEvaluatorItem)

            Dim oCheckbox As HtmlInputCheckBox = e.Item.FindControl("CBXevaluator")
            oCheckbox.Checked = SelectedEvaluations.Contains(item.IdEvaluation)
        End If
    End Sub
#End Region
End Class