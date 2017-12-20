Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract
Imports System.Linq
Imports System.Collections.Generic
Imports lm.Comol.Modules.CallForPapers.Domain
Imports lm.Comol.Modules.CallForPapers.Presentation.Evaluation
Imports lm.Comol.Core.DomainModel.Helpers.Export

Imports lm.Comol.Modules.CallForPapers.Advanced.Domain

Public Class EvaluateSubmission
    Inherits PageBase
    Implements IViewEvaluateSubmission

#Region "Context"
    Private _Presenter As EvaluateSubmissionPresenter
    Private ReadOnly Property CurrentPresenter() As EvaluateSubmissionPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New EvaluateSubmissionPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property Portalname As String Implements IViewEvaluateSubmission.Portalname
        Get
            Return Resource.getValue("Portalname")
        End Get
    End Property
    Private ReadOnly Property PreloadIdCall As Long Implements IViewEvaluateSubmission.PreloadIdCall
        Get
            If IsNumeric(Me.Request.QueryString("idCall")) Then
                Return CLng(Me.Request.QueryString("idCall"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewEvaluateSubmission.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadedIdEvaluation As Long Implements IViewEvaluateSubmission.PreloadedIdEvaluation
        Get
            If IsNumeric(Me.Request.QueryString("idEvaluation")) Then
                Return CLng(Me.Request.QueryString("idEvaluation"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private Property IdEvaluation As Long Implements IViewEvaluateSubmission.IdEvaluation
        Get
            Return ViewStateOrDefault("IdEvaluation", CLng(0))
        End Get
        Set(value As Long)
            Me.ViewState("IdEvaluation") = value
        End Set
    End Property
    Private Property IdEvaluator As Long Implements IViewEvaluateSubmission.IdEvaluator
        Get
            Return ViewStateOrDefault("IdEvaluator", CLng(0))
        End Get
        Set(value As Long)
            Me.ViewState("IdEvaluator") = value
        End Set
    End Property

    Private ReadOnly Property AnonymousOwnerName As String Implements IViewEvaluateSubmission.AnonymousOwnerName
        Get
            Return Me.Resource.getValue("AnonymousOwnerName")
        End Get
    End Property
    Private ReadOnly Property UnknownDisplayname As String Implements IViewEvaluateSubmission.UnknownDisplayname
        Get
            Return Resource.getValue("UnknownDisplayname")
        End Get
    End Property
    Private ReadOnly Property GeneralTabName As String Implements IViewEvaluateSubmission.GeneralTabName
        Get
            Return Me.Resource.getValue("GeneralTabName")
        End Get
    End Property
    Private Property CallType As CallForPaperType Implements IViewEvaluateSubmission.CallType
        Get
            Return ViewStateOrDefault("CallType", CallForPaperType.CallForBids)
        End Get
        Set(value As CallForPaperType)
            Me.ViewState("CallType") = value
        End Set
    End Property
    Private Property IdCall As Long Implements IViewEvaluateSubmission.IdCall
        Get
            Return ViewStateOrDefault("IdCall", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdCall") = value
        End Set
    End Property
    Private Property IdCallCommunity As Integer Implements IViewEvaluateSubmission.IdCallCommunity
        Get
            Return ViewStateOrDefault("IdCallCommunity", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCallCommunity") = value
        End Set
    End Property
    Private Property IdCallModule As Integer Implements IViewEvaluateSubmission.IdCallModule
        Get
            Return ViewStateOrDefault("IdCallModule", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCallModule") = value
        End Set
    End Property

    Private Property IdSubmission As Long Implements IViewEvaluateSubmission.IdSubmission
        Get
            Return ViewStateOrDefault("IdSubmission", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdSubmission") = value
        End Set
    End Property
    Private Property AllowCompleteEvaluation As Boolean Implements IViewEvaluateSubmission.AllowCompleteEvaluation
        Get
            Return ViewStateOrDefault("AllowCompleteEvaluation", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("AllowCompleteEvaluation") = value
            Me.BTNcompleteEvaluation.Visible = value
        End Set
    End Property
    Private Property AllowEvaluate As Boolean Implements IViewEvaluateSubmission.AllowEvaluate
        Get
            Return ViewStateOrDefault("AllowEvaluate", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("AllowEvaluate") = value
            Me.BTNsaveEvaluation.Visible = value
        End Set
    End Property
    Private Property SavingForComplete As Boolean Implements IViewEvaluateSubmission.SavingForComplete
        Get
            Return ViewStateOrDefault("SavingForComplete", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("SavingForComplete") = value
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

    Private Property ContainerName As String
        Get
            Return ViewStateOrDefault("ContainerName", "")
        End Get
        Set(value As String)
            Me.ViewState("ContainerName") = value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'DirectCast(Me.Master.FindControl("SCMmanager"), ScriptManager).RegisterPostBackControl(Me.BTNsaveSubmissionBottom)
        'DirectCast(Me.Master.FindControl("SCMmanager"), ScriptManager).RegisterPostBackControl(Me.BTNsaveSubmissionTop)
        'DirectCast(Me.Master.FindControl("SCMmanager"), ScriptManager).RegisterPostBackControl(Me.BTNsubmitSubmissionBottom)
        'DirectCast(Me.Master.FindControl("SCMmanager"), ScriptManager).RegisterPostBackControl(Me.BTNsubmitSubmissionTop)
    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()

        If Not Page.IsPostBack Then
            CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.EvaluationView,
            Me.IdEvaluation,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.Evaluation,
            "")
        End If


        Me.CurrentPresenter.InitView(False)
    End Sub
    Public Overrides Sub BindNoPermessi()

        CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.EvaluationView,
            Me.IdEvaluation,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.Evaluation,
            "NoPermission")

        MLVsubmissionDisplay.SetActiveView(VIWempty)
        Me.LBemptyMessage.Text = Resource.getValue("ServiceEvaluateNopermission")
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_CallSubmission", "Modules", "CallForPapers")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLiteral(LTowner_t)
            .setLiteral(LTsubmitterType_t)
            .setLiteral(LTsubmissionStatus_t)
            .setLiteral(LTsubmittedOn_t)
            .setLiteral(LTsubmittedBy_t)
            .setLabel(LBcriterionSelect_t)

            .setButton(BTNsaveEvaluation, True)
            .setButton(BTNcompleteEvaluation, True)

            '            LTmaxCharsrequest.Text = .getValue("MaxCharsInfo")
        End With
    End Sub
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
#Region "Messages"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As ModuleCallForPaper.ActionType) Implements IViewEvaluateSubmission.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleCallForPaper.ObjectType.CallForPaper, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As ModuleRequestForMembership.ActionType) Implements IViewEvaluateSubmission.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleRequestForMembership.ObjectType.RequestForMembership, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewEvaluateSubmission.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, ModuleCallForPaper.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewEvaluateSubmission.DisplaySessionTimeout
        Dim url As String = ""
        'If Me.PreloadFromManagement Then
        '    url = RootObject.ViewSubmissionAsManager(CallType, PreloadIdCall, PreloadedIdSubmission, PreloadedIdRevision, PreloadedUniqueID, False, PreloadView, PreloadFilterSubmission, PreloadOrderSubmission, PreloadAscending, PreloadPageIndex, PreloadPageSize)
        'Else
        '    url = RootObject.ViewSubmission(CallType, PreloadIdCall, PreloadedIdSubmission, PreloadedUniqueID, False, False, PreloadView)
        'End If
        DisplaySessionTimeout(url)
    End Sub
    Private Sub DisplaySessionTimeout(url As String) Implements IViewEvaluateSubmission.DisplaySessionTimeout
        Dim idCommunity As Integer = PreloadIdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.NewWindow
        dto.DestinationUrl = url

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If

        CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.EvaluationView,
            Me.IdEvaluation,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.Evaluation,
            "SessionTimeOut")

        webPost.Redirect(dto)
    End Sub
    Private Sub DisplayEvaluationUnavailable() Implements IViewEvaluateSubmission.DisplayEvaluationUnavailable
        MLVsubmissionDisplay.SetActiveView(VIWempty)
        Me.LBemptyMessage.Text = Me.Resource.getValue("DisplayEvaluationUnavailable")
    End Sub
    Private Sub DisplayNotEvaluationPermission() Implements IViewEvaluateSubmission.DisplayNotEvaluationPermission
        MLVsubmissionDisplay.SetActiveView(VIWempty)
        Me.LBemptyMessage.Text = Me.Resource.getValue("DisplayNotEvaluationPermission")
    End Sub
    Private Sub DisplayUnknownSubmission(idCommunity As Integer, idModule As Integer, idSubmission As Long, type As CallForPaperType) Implements IViewEvaluateSubmission.DisplayUnknownSubmission
        Me.MLVsubmissionDisplay.SetActiveView(VIWempty)
        Me.LBemptyMessage.Text = Resource.getValue("DisplayUnknownSubmission." & type.ToString)
        PageUtility.AddActionToModule(idCommunity, idModule, IIf(type = CallForPaperType.CallForBids, ModuleCallForPaper.ActionType.GenericError, ModuleRequestForMembership.ActionType.GenericError), PageUtility.CreateObjectsList(idModule, IIf(type = CallForPaperType.CallForBids, ModuleCallForPaper.ObjectType.UserSubmission, ModuleRequestForMembership.ObjectType.UserSubmission), idSubmission.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub DisplayUnknownCall(idCommunity As Integer, idModule As Integer, idCall As Long, type As CallForPaperType) Implements IViewEvaluateSubmission.DisplayUnknownCall
        Me.MLVsubmissionDisplay.SetActiveView(VIWempty)
        Me.LBemptyMessage.Text = Resource.getValue("DisplayUnknownCall." & type.ToString)
        If type = CallForPaperType.CallForBids Then
            SendUserAction(idCommunity, idModule, idCall, ModuleCallForPaper.ActionType.GenericError)
        Else
            SendUserAction(idCommunity, idModule, idCall, ModuleRequestForMembership.ActionType.GenericError)
        End If
    End Sub
    Private Sub DisplayUnknownEvaluation(idCommunity As Integer, idModule As Integer, idEvaluation As Long) Implements IViewEvaluateSubmission.DisplayUnknownEvaluation
        Me.MLVsubmissionDisplay.SetActiveView(VIWempty)
        Me.LBemptyMessage.Text = Resource.getValue("DisplayUnknownEvaluation")
        PageUtility.AddActionToModule(idCommunity, idModule, ModuleCallForPaper.ActionType.GenericError, PageUtility.CreateObjectsList(idModule, ModuleCallForPaper.ObjectType.Evaluation, idEvaluation.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Public Sub HideErrorMessages() Implements IViewEvaluateSubmission.HideErrorMessages
        Me.CTRLmessages.Visible = False
    End Sub
    Private Sub DisplayError(err As lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationEditorErrors) Implements IViewEvaluateSubmission.DisplayError
        DisplayError(err, Helpers.MessageType.error)
    End Sub
    Public Sub DisplayWarning(err As lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationEditorErrors, criteriaCount As Long) Implements lm.Comol.Modules.CallForPapers.Presentation.Evaluation.IViewEvaluateSubmission.DisplayWarning
        Me.CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(String.Format(Resource.getValue("EvaluationEditorErrors." & err.ToString & ".Count"), criteriaCount), Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayError(err As lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationEditorErrors, errorType As Helpers.MessageType)
        Me.CTRLmessages.Visible = (err <> lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationEditorErrors.None)
        Me.CTRLmessages.InitializeControl(Resource.getValue("EvaluationEditorErrors." & err.ToString), errorType)
    End Sub
    Private Sub DisplaySettingsSaved() Implements IViewEvaluateSubmission.DisplaySettingsSaved
        Me.CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayEvaluationSaved"), Helpers.MessageType.success)
    End Sub
    Private Sub SetContainerName(callName As String, userName As String, typeName As String, submittedBy As String) Implements IViewEvaluateSubmission.SetContainerName
        Dim title As String = Me.Resource.getValue("Evaluate.Title")
        If Not String.IsNullOrEmpty(title) Then
            title = String.Format(title, userName)
        End If
        Me.Master.ServiceTitle = title
        Dim translation As String = Me.Resource.getValue("Evaluate.Tooltip")
        If Not String.IsNullOrEmpty(title) Then
            translation = String.Format(translation, typeName, submittedBy, callName)
        End If
        Me.Master.ServiceTitleToolTip = translation
   End Sub

    'Private Sub LoadAttachments(items As List(Of dtoAttachmentFile)) Implements IViewEvaluateSubmission.LoadAttachments
    '    Me.RPTattachments.Visible = (items.Count > 0)
    '    Me.RPTattachments.DataSource = items
    '    Me.RPTattachments.DataBind()
    'End Sub
    ''Private Sub LoadCallInfo(baseCall As dtoCall) Implements IViewEvaluateSubmission.LoadCallInfo
    '    FLDcallInfo.Visible = True
    '    Me.MLVsubmissionDisplay.SetActiveView(VIWsubmission)
    '    DVtopMenu.Visible = True
    '    Me.LTcallDescription.Text = baseCall.Description
    '    Me.Resource.setLabel(LBstartDate)
    '    Me.LBstartDate.Text = String.Format(Me.LBstartDate.Text, baseCall.StartDate.ToString("dd/MM/yy"), baseCall.StartDate.ToString("hh:mm"))

    '    If baseCall.EndDate.HasValue Then
    '        Me.Resource.setLabel(LBendDate)
    '        Me.LBendDate.Text = String.Format(Me.LBendDate.Text, baseCall.EndDate.Value.ToString("dd/MM/yy"), baseCall.EndDate.Value.ToString("hh:mm"))
    '    Else
    '        Me.LBendDate.Text = Resource.getValue("NoExpirationDate")
    '    End If
    '    Me.LBwinnerinfo.Text = baseCall.AwardDate
    '    Me.LBwinnerinfo.Visible = Not String.IsNullOrEmpty(baseCall.AwardDate)

    '    Dim itemName As String = baseCall.Name
    '    Me.LBcallDescriptionTitle.Text = String.Format(Resource.getValue("DescriptionTitle." & baseCall.Type.ToString()), IIf(Len(itemName) > 70, Left(itemName, 70) & "...", itemName))
    '    Me.LBcallDescriptionTitle.ToolTip = String.Format(Resource.getValue("DescriptionTitle." & baseCall.Type.ToString() & ".ToolTip"), itemName)
    'End Sub

#End Region

#Region "Evaluations"
    Private Sub InitializeEvaluationSettings(tabs As List(Of lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterionForEvaluation), evaluation As lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoEvaluation) Implements IViewEvaluateSubmission.InitializeEvaluationSettings
        Me.DVevaluationTab.Visible = True
        Me.LBevaluationInfo.Visible = True
        Me.LBevaluationInfo.Text = Resource.getValue("EvaluationInfo." & evaluation.Status.ToString())
        Dim startedOn As String = "", modifiedOn As String = "", completedOn As String = ""
        If evaluation.EvaluationStartedOn.HasValue Then
            startedOn = FormatDateTime(evaluation.EvaluationStartedOn.Value, DateFormat.GeneralDate)
        End If
        If evaluation.ModifiedOn.HasValue Then
            modifiedOn = FormatDateTime(evaluation.ModifiedOn.Value, DateFormat.GeneralDate)
        End If
        If evaluation.EvaluatedOn.HasValue Then
            completedOn = FormatDateTime(evaluation.EvaluatedOn.Value, DateFormat.GeneralDate)
        End If
        Select Case evaluation.Status
            Case lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationStatus.Evaluated
                Me.LBevaluationInfo.Text = String.Format(Me.LBevaluationInfo.Text, startedOn, completedOn)
            Case lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationStatus.EvaluatorReplacement, lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationStatus.Invalidated
                Me.LBevaluationInfo.Text = Resource.getValue("EvaluationInfo." & evaluation.Status.ToString() & "." & evaluation.Evaluated.ToString)
                If evaluation.Evaluated Then
                    Me.LBevaluationInfo.Text = String.Format(Me.LBevaluationInfo.Text, startedOn, modifiedOn, completedOn)
                Else
                    Me.LBevaluationInfo.Text = String.Format(Me.LBevaluationInfo.Text, startedOn, modifiedOn)
                End If

            Case lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationStatus.Evaluating
                Me.LBevaluationInfo.Text = String.Format(Me.LBevaluationInfo.Text, startedOn, modifiedOn)
        End Select

        Me.RPTcriteriaTabs.DataSource = tabs
        Me.RPTcriteriaTabs.DataBind()

        Me.RPTcriteria.DataSource = evaluation.Criteria
        Me.RPTcriteria.DataBind()
    End Sub
    Private Function GetCriteria() As List(Of lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterionEvaluated) Implements IViewEvaluateSubmission.GetCriteria
        Dim criteria As New List(Of lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterionEvaluated)

        For Each row As RepeaterItem In (From r As RepeaterItem In RPTcriteria.Items Where r.ItemIndex < RPTcriteria.Items.Count - 1 Select r).ToList()
            Dim oControl As UC_InputCriterion = row.FindControl("CTRLinputCriterion")
            criteria.Add(oControl.GetCriterion())
        Next

        Return criteria
    End Function
    Private Function GetComment() As String
        Dim oTextbox As TextBox = RPTcriteria.Items(RPTcriteria.Items.Count - 1).FindControl("TXBcomment")
        Return oTextbox.Text
    End Function
#End Region

#Region "Submission"
    Private Sub LoadSubmissionInfo(submitterName As String, ownerName As String, status As SubmissionStatus) Implements IViewEvaluateSubmission.LoadSubmissionInfo
        Me.MLVsubmissionDisplay.SetActiveView(VIWsubmission)
        DVtopMenu.Visible = True
        Me.LBowner.Text = ownerName
        Me.LBsubmitterType.Text = submitterName
        Me.LBsubmissionStatus.Text = Resource.getValue("SubmissionStatus." & status.ToString)
        Me.LIsubmissionInfo.Visible = False
    End Sub
    Private Sub LoadSubmissionInfo(submitterName As String, ownerName As String, status As SubmissionStatus, submittedOn As DateTime) Implements IViewEvaluateSubmission.LoadSubmissionInfo
        Me.MLVsubmissionDisplay.SetActiveView(VIWsubmission)
        LoadSubmissionInfo(submitterName, ownerName, status)
        Me.LIsubmissionInfo.Visible = True
        SPNsubmittedBy.Visible = False
        Me.LBsubmittedOnData.Text = submittedOn.ToString("dd/MM/yy")
        Me.LBsubmittedOnTime.Text = submittedOn.ToString("HH:mm")
    End Sub
    Private Sub LoadSubmissionInfo(submitterName As String, ownerName As String, status As SubmissionStatus, submittedOn As DateTime, submittedBy As String) Implements IViewEvaluateSubmission.LoadSubmissionInfo
        Me.MLVsubmissionDisplay.SetActiveView(VIWsubmission)
        LoadSubmissionInfo(submitterName, ownerName, status)
        Me.LBsubmittedBy.Text = submittedBy
        SPNsubmittedBy.Visible = True
    End Sub
    Private Sub InitializeExportControl(isOwner As Boolean, idUser As Integer, idCall As Long, idSubmission As Long, idRevision As Long, idModule As Integer, idCallCommunity As Integer, callType As CallForPaperType) Implements IViewEvaluateSubmission.InitializeExportControl
        Me.CTRLreport.InitializeControl(isOwner, idUser, LBowner.Text, idCall, idSubmission, idRevision, idModule, idCallCommunity, callType)
    End Sub
    Private Sub LoadSections(sections As List(Of dtoCallSection(Of dtoSubmissionValueField))) Implements IViewEvaluateSubmission.LoadSections
        Me.RPTsections.DataSource = sections
        Me.RPTsections.DataBind()
    End Sub
    'Private Sub LoadRequiredFiles(files As List(Of dtoCallSubmissionFile)) Implements IViewEvaluateSubmission.LoadRequiredFiles
    '    Me.RPTrequiredFiles.Visible = (files.Count > 0)
    '    Me.RPTrequiredFiles.DataSource = files
    '    Me.RPTrequiredFiles.DataBind()
    'End Sub
#End Region

    Private Sub GoToUrl(url As String) Implements IViewEvaluateSubmission.GoToUrl
        PageUtility.RedirectToUrl(url)
    End Sub
#End Region

#Region "Submission"
#Region "Repeater Binding"
    'Private Sub RPTattachments_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTattachments.ItemDataBound
    '    If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
    '        Dim dto As dtoAttachmentFile = DirectCast(e.Item.DataItem, dtoAttachmentFile)

    '        Dim renderFile As UC_ModuleRepositoryAction = e.Item.FindControl("CTRLdisplayFile")

    '        Dim initializer As New lm.Comol.Core.ModuleLinks.dtoModuleDisplayActionInitializer

    '        ' DIMENSIONI IMMAGINI
    '        initializer.IconSize = Helpers.IconSize.Small
    '        renderFile.EnableAnchor = True
    '        initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction
    '        initializer.Link = dto.Link
    '        renderFile.InsideOtherModule = True
    '        Dim actions As List(Of dtoModuleActionControl)
    '        actions = renderFile.InitializeRemoteControl(initializer, StandardActionType.Play)

    '        Dim oLabel As Label = e.Item.FindControl("LBattachmentDescription")
    '        oLabel.Text = dto.Description

    '        Dim oControl As HtmlGenericControl = e.Item.FindControl("DVdescription")
    '        oControl.Visible = Not String.IsNullOrEmpty(dto.Description)


    '    ElseIf e.Item.ItemType = ListItemType.Header Then
    '        Dim oLiteral As Literal = e.Item.FindControl("LTattachmentsTitle")
    '        Resource.setLiteral(oLiteral)
    '    End If
    'End Sub
    Private Sub RPTsections_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTsections.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim section As dtoCallSection(Of dtoSubmissionValueField) = DirectCast(e.Item.DataItem, dtoCallSection(Of dtoSubmissionValueField))

            Dim oLiteral As Literal = e.Item.FindControl("LTsectionTitle")
            oLiteral.Text = section.Name
            oLiteral = e.Item.FindControl("LTsectionDescription")
            oLiteral.Text = section.Description
        End If
    End Sub
    Protected Sub RPTfields_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim field As dtoSubmissionValueField = DirectCast(e.Item.DataItem, dtoSubmissionValueField)

            Dim renderField As UC_RenderField = e.Item.FindControl("CTRLrenderField")
            renderField.TagHelper = Me.TagsHelper
            renderField.InitializeControl(field, False, False, field.FieldError, field.AllowSelection)

        End If
    End Sub
    'Private Sub RPTrequiredFiles_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTrequiredFiles.ItemDataBound
    '    If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
    '        Dim dto As dtoCallSubmissionFile = DirectCast(e.Item.DataItem, dtoCallSubmissionFile)

    '        Dim oControl As UC_InputRequiredFile = e.Item.FindControl("CTRLrequiredFile")
    '        oControl.InitializeControl(IdCall, 0, IdCallCommunity, dto, True, False)

    '    ElseIf e.Item.ItemType = ListItemType.Header Then
    '        Dim oLiteral As Literal = e.Item.FindControl("LTrequiredFilesTitle")
    '        Resource.setLiteral(oLiteral)
    '        oLiteral = e.Item.FindControl("LTrequiredFilesDescription")
    '        Resource.setLiteral(oLiteral)
    '    End If
    'End Sub
#End Region

    Private Sub CTRLreport_GetContainerTemplate(ByRef template As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template) Handles CTRLreport.GetContainerTemplate
        template.Header = Me.Master.getTemplateHeader(" ")
        template.Footer = Me.Master.getTemplateFooter()
    End Sub
    Private Sub CTRLreport_GetConfigTemplate(ByRef template As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template) Handles CTRLreport.GetConfigTemplate
        template = Me.Master.getConfigTemplate()
    End Sub
    Private Sub CTRLreport_GetHiddenIdentifierValueEvent(ByRef value As String) Handles CTRLreport.GetHiddenIdentifierValueEvent
        value = HDNdownloadTokenValue.Value
    End Sub
    Private Sub CTRLreport_RefreshContainerEvent() Handles CTRLreport.RefreshContainerEvent

    End Sub

#End Region

#Region "Evaluations"
    Private Sub RPTcriteriaTabs_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTcriteriaTabs.ItemDataBound
        Dim item As lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterionForEvaluation = DirectCast(e.Item.DataItem, lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterionForEvaluation)
        Dim oLabel As Label = e.Item.FindControl("LBcriterionStatus")
        oLabel.Visible = (item.Id > 0)
        oLabel.CssClass &= " " & IIf(item.IsInvalid, "red", IIf(item.Status = Evaluation.EvaluationStatus.Evaluated, "green", IIf(item.Status = Evaluation.EvaluationStatus.Evaluating, "yellow", "")))
    End Sub
    Private Sub RPTcriteria_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTcriteria.ItemDataBound
        Dim item As lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterionEvaluated = DirectCast(e.Item.DataItem, lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterionEvaluated)
        If item.IdCriterion > 0 Then
            Dim oControl As UC_InputCriterion = e.Item.FindControl("CTRLinputCriterion")
            oControl.InitializeControl(IdCall, IdSubmission, IdEvaluation, IdCallCommunity, item, Not AllowEvaluate, item.CriterionError)
            oControl.Visible = True
        Else
            Dim oGeneric As HtmlControl = e.Item.FindControl("DVcomment")
            oGeneric.Visible = True
            Dim oLabel As Label = e.Item.FindControl("LBcommentEvaluation_t")
            Resource.setLabel(oLabel)
            Dim oTextbox As TextBox = e.Item.FindControl("TXBcomment")
            oTextbox.Text = item.Comment
            oTextbox.Enabled = AllowEvaluate
            If item.Criterion.CommentMaxLength > 0 Then
                oTextbox.Attributes.Add("maxlength", item.Criterion.CommentMaxLength)
                Dim oSpan As HtmlControl = e.Item.FindControl("SPNmaxCharsComment")
                oSpan.Visible = True
            End If


            Dim oLiteral As Literal = e.Item.FindControl("LTmaxCharsComment")

            Resource.setLiteral(oLiteral)
            '<asp:Label runat="server" id="" AssociatedControlID="TXBcomment" CssClass="fieldlabel">Text</asp:Label>
            '                                <asp:TextBox runat="server" ID="TXBcomment" TextMode="multiline" CssClass="textarea"></asp:TextBox>
            '                                <asp:Label runat="server" ID="LBcommentHelp" CssClass="inlinetooltip"></asp:Label>     
            '                                <br/>
            '                                <span class="fieldinfo ">
            '                                    <span class="maxchar" runat="server" id="SPNmaxCharsComment" >
            '                                        <asp:Literal ID="LTmaxCharsComment" runat="server"></asp:Literal>
            '                                        <span class="availableitems">{available}</span>/<span class="totalitems">{total}</span>
            '                                    </span>
            '                                    <asp:Label ID="LBerrorMessageComment" runat="server" Visible="false" cssClass="generic"></asp:Label>
        End If

    End Sub

    Private Sub BTNcompleteEvaluation_Click(sender As Object, e As System.EventArgs) Handles BTNcompleteEvaluation.Click

        CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.EvaluationSave,
            Me.IdEvaluation,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.Evaluation,
            "")

        CurrentPresenter.SaveEvaluation(GetCriteria, GetComment(), True)
    End Sub
    Private Sub BTNsaveEvaluation_Click(sender As Object, e As System.EventArgs) Handles BTNsaveEvaluation.Click

        CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.EvaluationSaveDraft,
            Me.IdEvaluation,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.Evaluation,
            "")

        CurrentPresenter.SaveEvaluation(GetCriteria, GetComment(), False)
    End Sub
#End Region

    Private Sub Submission_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Me.Master.ShowDocType = True
        Me.Master.ReloadOpener = True
    End Sub

#Region "Revisions"
    'Private Sub HideRevisions()
    '    For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTsections.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
    '        Dim oRepeater As Repeater = row.FindControl("RPTfields")
    '        HideFieldRevision(oRepeater)
    '    Next
    'End Sub

    'Private Sub HideFieldRevision(oRepeater As Repeater)
    '    Dim fields As New List(Of dtoRevisionItem)
    '    For Each row As RepeaterItem In (From r As RepeaterItem In oRepeater.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
    '        Dim field As New dtoRevisionItem
    '        Dim renderField As UC_RenderField = row.FindControl("CTRLrenderField")
    '        renderField.HideSelection()
    '    Next
    'End Sub
#End Region


#Region "Tags Management"


    Private Sub BindInternalTags(ByVal Tags As String)

        Dim tagHelperTmp As New AdvTagHelper(Tags)

        'ToDo: continuare sostituendo TagHelper!
        'ToDo: il contenitore iniziale diventerà un UC nel cui viewstate verrà salvato AdvTagHelper

        If Not tagHelperTmp.HasValue Then
            FStagContainer.Visible = False
            Return
        End If

        FStagContainer.Visible = True

        TagsHelper = tagHelperTmp

        Me.RPTtag.DataSource = tagHelperTmp.GetKvpList()
        Me.RPTtag.DataBind()

    End Sub

    Private Property TagsHelper As AdvTagHelper
        Get

            Return ViewStateOrDefault("TagsKvps", New AdvTagHelper(""))
        End Get
        Set(value As AdvTagHelper)
            ViewState("TagsKvps") = value
        End Set
    End Property

    Private Sub RPTtag_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTtag.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then

            Try
                Dim kvp As KeyValuePair(Of Integer, String) = e.Item.DataItem

                Dim LTtag As Literal = e.Item.FindControl("LTtag")

                LTtag.Text = String.Format(LTtag.Text, TagsHelper.GetTagCssSingleString(kvp.Value), kvp.Value)

            Catch ex As Exception

            End Try


        End If
    End Sub

    Public Sub BindTag(tags As String) Implements IViewEvaluateSubmission.BindTag
        BindInternalTags(tags)
    End Sub

#End Region


End Class