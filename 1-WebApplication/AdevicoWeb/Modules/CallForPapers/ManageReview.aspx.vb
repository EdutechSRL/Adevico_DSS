Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation
Imports lm.Comol.Modules.CallForPapers.Domain
Imports lm.ActionDataContract
Imports System.Linq
Imports System.Collections.Generic
Public Class ManageReview
    Inherits PageBase
    Implements IViewManageReview


#Region "Context"
    Private _Presenter As ManageReviewPresenter
    Private ReadOnly Property CurrentPresenter() As ManageReviewPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ManageReviewPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property Portalname As String Implements IViewManageReview.Portalname
        Get
            Return Resource.getValue("Portalname")
        End Get
    End Property
    Private ReadOnly Property PreloadIdCall As Long Implements IViewManageReview.PreloadIdCall
        Get
            If IsNumeric(Me.Request.QueryString("idCall")) Then
                Return CLng(Me.Request.QueryString("idCall"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewManageReview.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadAscending As Boolean Implements IViewManageReview.PreloadAscending
        Get
            If String.IsNullOrEmpty(Request.QueryString("Ascending")) Then
                Return False
            Else
                Return (Request.QueryString("Ascending") = "true" OrElse Request.QueryString("Ascending") = "True")
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadView As CallStatusForSubmitters Implements IViewManageReview.PreloadView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of CallStatusForSubmitters).GetByString(Request.QueryString("View"), CallStatusForSubmitters.SubmissionOpened)
        End Get
    End Property
    Private ReadOnly Property PreloadedIdRevision As Long Implements IViewManageReview.PreloadedIdRevision
        Get
            If IsNumeric(Me.Request.QueryString("idRevision")) Then
                Return CLng(Me.Request.QueryString("idRevision"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadAction As CallStandardAction Implements IViewManageReview.PreloadAction
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of CallStandardAction).GetByString(Request.QueryString("action"), CallStandardAction.List)
        End Get
    End Property
    Private ReadOnly Property PreloadFilterSubmission As SubmissionFilterStatus Implements IViewManageReview.PreloadFilterSubmission
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of SubmissionFilterStatus).GetByString(Request.QueryString("filter"), SubmissionFilterStatus.OnlySubmitted)
        End Get
    End Property
    Private ReadOnly Property PreloadFromManagement As Boolean Implements IViewManageReview.PreloadFromManagement
        Get
            Return (Request.QueryString("Manage") = "true" OrElse Request.QueryString("Manage") = "True")
        End Get
    End Property
    Private ReadOnly Property PreloadOrderSubmission As SubmissionsOrder Implements IViewManageReview.PreloadOrderSubmission
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of SubmissionsOrder).GetByString(Request.QueryString("OrderBy"), SubmissionsOrder.ByUser)
        End Get
    End Property
    Private ReadOnly Property PreloadPageIndex As Integer Implements IViewManageReview.PreloadPageIndex
        Get
            If IsNumeric(Me.ViewState("pageIndex")) Then
                Return CInt(Me.ViewState("pageIndex"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadPageSize As Integer Implements IViewManageReview.PreloadPageSize
        Get
            If IsNumeric(Me.ViewState("pageSize")) Then
                Return CInt(Me.ViewState("pageSize"))
            Else
                Return 50
            End If
        End Get
    End Property
    Private Property IdSubmission As Long Implements IViewManageReview.IdSubmission
        Get
            Return ViewStateOrDefault("IdSubmission", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdSubmission") = value
        End Set
    End Property
    Private ReadOnly Property PreloadedIdSubmission As Long Implements IViewManageReview.PreloadedIdSubmission
        Get
            If IsNumeric(Me.Request.QueryString("idSubmission")) Then
                Return CLng(Me.Request.QueryString("idSubmission"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private Property CallType As CallForPaperType Implements IViewManageReview.CallType
        Get
            Return ViewStateOrDefault("CallType", CallForPaperType.CallForBids)
        End Get
        Set(value As CallForPaperType)
            Me.ViewState("CallType") = value
        End Set
    End Property
    Private Property IdCall As Long Implements IViewManageReview.IdCall
        Get
            Return ViewStateOrDefault("IdCall", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdCall") = value
        End Set
    End Property
    Private Property IdCallCommunity As Integer Implements IViewManageReview.IdCallCommunity
        Get
            Return ViewStateOrDefault("IdCallCommunity", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCallCommunity") = value
        End Set
    End Property
    Private Property IdCallModule As Integer Implements IViewManageReview.IdCallModule
        Get
            Return ViewStateOrDefault("IdCallModule", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCallModule") = value
        End Set
    End Property
    Private Property IdSubmitterType As Long Implements IViewManageReview.IdSubmitterType
        Get
            Return ViewStateOrDefault("IdSubmitterType", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdSubmitterType") = value
        End Set
    End Property
    Private Property IdUserSubmitter As Int32 Implements IViewManageReview.IdUserSubmitter
        Get
            Return ViewStateOrDefault("IdUserSubmitter", 0)
        End Get
        Set(value As Int32)
            Me.ViewState("IdUserSubmitter") = value
        End Set
    End Property

    
    Private Property IdRevision As Long Implements IViewManageReview.IdRevision
        Get
            Return ViewStateOrDefault("IdRevision", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdRevision") = value
        End Set
    End Property
   
    Private ReadOnly Property AnonymousOwnerName As String Implements IViewManageReview.AnonymousOwnerName
        Get
            Return Me.Resource.getValue("AnonymousOwnerName")
        End Get
    End Property
    Private Property CurrentStatus As RevisionStatus Implements IViewManageReview.CurrentStatus
        Get
            Return ViewStateOrDefault("CurrentStatus", RevisionStatus.None)
        End Get
        Set(value As RevisionStatus)
            Me.ViewState("CurrentStatus") = value
        End Set
    End Property

    Private Property FieldsToCheck As List(Of Long) Implements IViewManageReview.FieldsToCheck
        Get
            Return ViewStateOrDefault("FieldsToCheck", New List(Of Long))
        End Get
        Set(value As List(Of Long))
            Me.ViewState("FieldsToCheck") = value
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

    'Private Property ContainerName As String
    '    Get
    '        Return ViewStateOrDefault("ContainerName", "")
    '    End Get
    '    Set(value As String)
    '        Me.ViewState("ContainerName") = value
    '    End Set
    'End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        TXBfeedback.Attributes.Add("maxlength", "6000")
        Me.CurrentPresenter.InitView()
    End Sub
    Public Overrides Sub BindNoPermessi()
        MLVpreview.SetActiveView(VIWempty)
        Me.LBemptyMessage.Text = Resource.getValue("ServiceViewSubmissionNopermission")
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_CallSubmission", "Modules", "CallForPapers")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setHyperLink(HYPlist, True, True)
            .setHyperLink(HYPmanage, True, True)
            .setLabel(LBcallDescriptionTitle)

            .setLiteral(LTowner_t)
            .setLiteral(LTsubmitterType_t)
            .setLiteral(LTsubmissionStatus_t)
            .setLiteral(LTsubmittedOn_t)
            .setLiteral(LTsubmittedBy_t)

            .setLabel(LBdeadline_t)
            .setLabel(LBrequestReason_t)
            LTmaxCharsrequest.Text = .getValue("MaxCharsInfo")

            .setLabel(LBfeedback_t)
            .setLabel(LBfeedbackHelp)
            .setLabel(LBstatusInfo_t)
            .setButton(BTNcancelManagerRequest, True, , , True)
            .setButton(BTNcancelUserRevisionRequest, True, , , True)
            .setButton(BTNrefuseRevision, True, , , True)
            .setButton(BTNacceptRevision, True, , , True)
            .setButton(BTNrefuseUserRequest, True, , , True)
            .setButton(BTNacceptUserRequest, True, , , True)
            .setButton(BTNsaveRevisionSettings, True, , , True)
            Me.Master.ServiceTitle = .getValue("serviceTitle.ManageRevision")
            Me.Master.ServiceTitleToolTip = .getValue("serviceTitle.ManageRevision")

            .setLiteral(LTrevisionDate)
            .setLiteral(LTrevisionBy_t)

        End With
    End Sub
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As ModuleCallForPaper.ActionType) Implements IViewManageReview.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleCallForPaper.ObjectType.CallForPaper, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As ModuleRequestForMembership.ActionType) Implements IViewManageReview.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleRequestForMembership.ObjectType.RequestForMembership, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewManageReview.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, ModuleCallForPaper.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewManageReview.DisplaySessionTimeout
        Dim url As String = ""
        If Me.PreloadFromManagement Then
            url = RootObject.ManageReviewSubmission(CallForPaperType.CallForBids, PreloadIdCall, PreloadedIdSubmission, PreloadedIdRevision, PreloadAction, PreLoadedContentView, PreloadFilterSubmission, PreloadOrderSubmission, PreloadAscending, PreloadPageIndex, PreloadPageSize)
        Else
            url = RootObject.ManageReviewSubmission(CallType, PreloadIdCall, PreloadedIdSubmission, PreloadedIdRevision, PreloadAction, PreLoadedContentView)
        End If
        DisplaySessionTimeout(url)
    End Sub
    Private Sub DisplaySessionTimeout(url As String) Implements IViewManageReview.DisplaySessionTimeout
        Dim idCommunity As Integer = PreloadIdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = url

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If
        webPost.Redirect(dto)
    End Sub

    Private Sub DisplayRevisionUnavailable() Implements IViewManageReview.DisplayRevisionUnavailable
        Me.MLVpreview.SetActiveView(VIWempty)
        Me.LBemptyMessage.Text = Resource.getValue("DisplayRevisionUnavailable." & CallType.ToString)
        PageUtility.AddActionToModule(IdCallCommunity, IdCallModule, IIf(CallType = CallForPaperType.CallForBids, ModuleCallForPaper.ActionType.GenericError, ModuleRequestForMembership.ActionType.GenericError), PageUtility.CreateObjectsList(IdCallModule, IIf(CallType = CallForPaperType.CallForBids, ModuleCallForPaper.ObjectType.Revision, ModuleRequestForMembership.ObjectType.Revision), IdRevision.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub DisplayRevisionUnknown() Implements IViewManageReview.DisplayRevisionUnknown
        Me.MLVpreview.SetActiveView(VIWempty)
        Me.LBemptyMessage.Text = Resource.getValue("DisplayRevisionUnknown." & CallType.ToString)
        PageUtility.AddActionToModule(IdCallCommunity, IdCallModule, IIf(CallType = CallForPaperType.CallForBids, ModuleCallForPaper.ActionType.GenericError, ModuleRequestForMembership.ActionType.GenericError), PageUtility.CreateObjectsList(IdCallModule, IIf(CallType = CallForPaperType.CallForBids, ModuleCallForPaper.ObjectType.Revision, ModuleRequestForMembership.ObjectType.Revision), IdRevision.ToString), InteractionType.UserWithLearningObject)
    End Sub

    Private Sub LoadCallInfo(baseCall As dtoCall) Implements IViewManageReview.LoadCallInfo
        FLDcallInfo.Visible = True
        Me.MLVpreview.SetActiveView(VIWcall)
        DVtopMenu.Visible = True
        Me.LTcallDescription.Text = baseCall.Description

        Dim itemName As String = baseCall.Name
        Me.LBcallDescriptionTitle.Text = String.Format(Resource.getValue("DescriptionTitle." & baseCall.Type.ToString()), IIf(Len(itemName) > 70, Left(itemName, 70) & "...", itemName))
        Me.LBcallDescriptionTitle.ToolTip = String.Format(Resource.getValue("DescriptionTitle." & baseCall.Type.ToString() & ".ToolTip"), itemName)
    End Sub
    Public Sub LoadCallInfo(baseCall As dtoRequest) Implements IViewManageReview.LoadCallInfo
        Me.MLVpreview.SetActiveView(VIWcall)
        DVtopMenu.Visible = True
        Me.LTcallDescription.Text = baseCall.StartMessage

        Dim itemName As String = baseCall.Name
        Me.LBcallDescriptionTitle.Text = String.Format(Resource.getValue("DescriptionTitle." & baseCall.Type.ToString()), IIf(Len(itemName) > 70, Left(itemName, 70) & "...", itemName))
        Me.LBcallDescriptionTitle.ToolTip = String.Format(Resource.getValue("DescriptionTitle." & baseCall.Type.ToString() & ".ToolTip"), itemName)
    End Sub
    Private Sub LoadSubmissionInfo(submitterName As String, ownerName As String, status As SubmissionStatus) Implements IViewManageReview.LoadSubmissionInfo
        Me.MLVpreview.SetActiveView(VIWcall)
        DVtopMenu.Visible = True
        Me.LBowner.Text = ownerName
        Me.LBsubmitterType.Text = submitterName
        Me.LBsubmissionStatus.Text = Resource.getValue("SubmissionStatus." & status.ToString)
        Me.LIsubmissionInfo.Visible = False
    End Sub
    Private Sub LoadSubmissionInfo(submitterName As String, ownerName As String, status As SubmissionStatus, submittedOn As DateTime) Implements IViewManageReview.LoadSubmissionInfo
        Me.MLVpreview.SetActiveView(VIWcall)
        LoadSubmissionInfo(submitterName, ownerName, status)
        Me.LIsubmissionInfo.Visible = True
        SPNsubmittedBy.Visible = False
        Me.LBsubmittedOnData.Text = submittedOn.ToString("dd/MM/yy")
        Me.LBsubmittedOnTime.Text = submittedOn.ToString("HH:mm")
    End Sub
    Private Sub LoadSubmissionInfo(submitterName As String, ownerName As String, status As SubmissionStatus, submittedOn As DateTime, submittedBy As String) Implements IViewManageReview.LoadSubmissionInfo
        Me.MLVpreview.SetActiveView(VIWcall)
        LoadSubmissionInfo(submitterName, ownerName, status)
        Me.LBsubmittedBy.Text = submittedBy
        SPNsubmittedBy.Visible = True
    End Sub

    Private Sub InitializeExportControl(isOwner As Boolean, idUser As Integer, idCall As Long, idSubmission As Long, idRevision As Long, idModule As Integer, idCallCommunity As Integer, callType As CallForPaperType) Implements IViewManageReview.InitializeExportControl
        Me.CTRLreport.InitializeControl(isOwner, idUser, LBowner.Text, idCall, idSubmission, idRevision, idModule, idCallCommunity, callType)
    End Sub

    Private Sub DisplayRevisionInfo(revision As dtoRevisionRequest) Implements IViewManageReview.DisplayRevisionInfo
        Me.DVrevision.Visible = True
        Me.DVfeedback.Visible = False
        Me.DVdeadline.Visible = False
        BTNcancelManagerRequest.Visible = False
        BTNcancelUserRevisionRequest.Visible = False
        BTNrefuseUserRequest.Visible = False
        BTNacceptUserRequest.Visible = False
        BTNrefuseRevision.Visible = False
        BTNacceptRevision.Visible = False
        BTNsaveRevisionSettings.Visible = False

        Me.RDPdeadline.Enabled = Not (revision.Status = RevisionStatus.Approved OrElse revision.Status = RevisionStatus.Cancelled OrElse revision.Status = RevisionStatus.Refused OrElse revision.Status = RevisionStatus.Submitted)
        If revision.EndDate.HasValue Then
            Me.RDPdeadline.SelectedDate = revision.EndDate
            If DateTime.Now.AddMinutes(10) > revision.EndDate Then
                Me.RDPdeadline.MinDate = revision.EndDate
            Else
                Me.RDPdeadline.MinDate = DateTime.Now.AddMinutes(10)
            End If
        Else
            Me.RDPdeadline.MinDate = DateTime.Now.AddMinutes(10)
            Me.RDPdeadline.SelectedDate = DateTime.Now.AddDays(4)
        End If
        Me.DVdeadline.Visible = Not (revision.Status = RevisionStatus.Request)
        Me.LBrequestReason.Text = revision.Reason

        If revision.Status = RevisionStatus.Approved OrElse revision.Status = RevisionStatus.Cancelled OrElse revision.Status = RevisionStatus.RequestAccepted OrElse revision.Status = RevisionStatus.Refused Then
            Me.TXBfeedback.Text = revision.Feedback
        Else
            Me.TXBfeedback.Text = ""
        End If

        Me.LBstatusInfo.Text = Resource.getValue("RevisionStatus." & revision.Status.ToString)

        If (revision.ModifiedOn.HasValue) Then
            LBrevisionDate.Text = revision.ModifiedOn.Value.ToShortDateString
            LBrevisionTime.Text = revision.ModifiedOn.Value.ToShortTimeString
        Else
            LBrevisionDate.Text = "//"
        End If
        If Not IsNothing(revision.ModifiedBy) Then
            LBrevisionBy.Text = revision.ModifiedBy.SurnameAndName
        Else
            LBrevisionBy.Text = "//"
        End If

        DVfeedback.Visible = True

        Me.TXBfeedback.Visible = True '(revision.Status <> RevisionStatus.Submitted)
        Me.TXBfeedback.Enabled = Not (revision.Status = RevisionStatus.Approved OrElse revision.Status = RevisionStatus.Cancelled OrElse revision.Status = RevisionStatus.Refused OrElse revision.Status = RevisionStatus.RequestAccepted)
        Me.TXBfeedback.CssClass = IIf(TXBfeedback.Enabled, "textarea", "readonlytextarea")
        SPNmaxChar.Visible = Me.TXBfeedback.Enabled
        Select Case revision.Type
            Case RevisionType.Manager
                Me.BTNacceptRevision.Visible = (revision.Status = RevisionStatus.Submitted)
                Me.BTNrefuseRevision.Visible = (revision.Status = RevisionStatus.Submitted)
                Me.BTNcancelManagerRequest.Visible = (revision.Status = RevisionStatus.Required)
                BTNsaveRevisionSettings.Visible = (revision.Status = RevisionStatus.Required)
                'DVfeedback.Visible = True
                'Me.TXBfeedback.Visible = True '(revision.Status <> RevisionStatus.Submitted)
                'Me.TXBfeedback.Enabled = Not (revision.Status = RevisionStatus.Approved OrElse revision.Status = RevisionStatus.Cancelled OrElse revision.Status = RevisionStatus.Refused)
                'Me.TXBfeedback.CssClass = IIf(TXBfeedback.Enabled, "textarea", "readonlytextarea")
            Case RevisionType.UserRequired
                Me.BTNacceptRevision.Visible = (revision.Status = RevisionStatus.Submitted)
                Me.BTNrefuseRevision.Visible = (revision.Status = RevisionStatus.Submitted)
                Me.BTNacceptUserRequest.Visible = (revision.Status = RevisionStatus.Request)
                Me.BTNrefuseUserRequest.Visible = (revision.Status = RevisionStatus.Request)
                Me.BTNcancelUserRevisionRequest.Visible = (revision.Status = RevisionStatus.Request OrElse revision.Status = RevisionStatus.RequestAccepted)
                BTNsaveRevisionSettings.Visible = (revision.Status = RevisionStatus.RequestAccepted)
        End Select
    End Sub
    Private Sub LoadSections(sections As List(Of dtoCallSection(Of dtoSubmissionValueField))) Implements IViewManageReview.LoadSections
        Me.RPTsections.DataSource = sections
        Me.RPTsections.DataBind()
    End Sub
    Private Sub SetActionUrl(url As String) Implements IViewManageReview.SetActionUrl
        Me.HYPlist.NavigateUrl = BaseUrl & url
        Me.HYPlist.Visible = True

    End Sub
    Private Sub GoToUrl(url As String) Implements IViewManageReview.GoToUrl
        PageUtility.RedirectToUrl(url)
    End Sub
#End Region

#Region "Repeater binding"
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
            If Not field.AllowSelection Then
                renderField.ShowFieldChecked = FieldsToCheck.Contains(field.IdField) AndAlso (CurrentStatus = RevisionStatus.RequestAccepted OrElse CurrentStatus = RevisionStatus.Required OrElse CurrentStatus = RevisionStatus.Submitted)
            End If

            renderField.InitializeControl(field, False, False, field.FieldError, field.AllowSelection)
            renderField.Selected = FieldsToCheck.Contains(field.IdField) 'field.AllowSelection AndAlso FieldsToCheck.Contains(field.IdField)
        Else
        End If
    End Sub

#End Region

    Private Sub CTRLreport_GetConfigTemplate(ByRef template As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template) Handles CTRLreport.GetConfigTemplate
        template = Me.Master.getConfigTemplate()
    End Sub
    Private Sub CTRLreport_GetContainerTemplate(ByRef template As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template) Handles CTRLreport.GetContainerTemplate
        template.Header = Me.Master.getTemplateHeader(" ")
        template.Footer = Me.Master.getTemplateFooter()
    End Sub
    Private Sub CTRLreport_GetHiddenIdentifierValueEvent(ByRef value As String) Handles CTRLreport.GetHiddenIdentifierValueEvent
        value = HDNdownloadTokenValue.Value
    End Sub
    Private Sub CTRLreport_RefreshContainerEvent() Handles CTRLreport.RefreshContainerEvent

    End Sub
    Private Sub Submission_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Me.Master.ShowDocType = True
    End Sub

#Region "Revisions"
    Private Sub BTNacceptRevision_Click(sender As Object, e As System.EventArgs) Handles BTNacceptRevision.Click
        Me.CurrentPresenter.AcceptUserRevision(IdSubmission, IdRevision, Me.TXBfeedback.Text, New dtoRevisionMessage() With {.SmtpConfig = PageUtility.CurrentSmtpConfig, .Subject = Resource.getValue("AcceptUserRevision." & CallType.ToString() & ".Subject"), .Body = Resource.getValue("AcceptUserRevision." & CallType.ToString() & ".Body")}, PageUtility.ApplicationUrlBase)
    End Sub
    Private Sub BTNrefuseRevision_Click(sender As Object, e As System.EventArgs) Handles BTNrefuseRevision.Click
        Me.CurrentPresenter.RefuseUserRevision(IdSubmission, IdRevision, Me.TXBfeedback.Text, New dtoRevisionMessage() With {.SmtpConfig = PageUtility.CurrentSmtpConfig, .Subject = Resource.getValue("RefuseUserRevision." & CallType.ToString() & ".Subject"), .Body = Resource.getValue("RefuseUserRevision." & CallType.ToString() & ".Body")}, PageUtility.ApplicationUrlBase)
    End Sub
    Private Sub BTNacceptUserRequest_Click(sender As Object, e As System.EventArgs) Handles BTNacceptUserRequest.Click
        If GetFieldsToReview.Count > 0 Then
            Me.CurrentPresenter.AcceptUserRequest(IdSubmission, IdRevision, Me.TXBfeedback.Text, Me.RDPdeadline.SelectedDate, GetFieldsToReview, PageUtility.GetRepositoryDiskPath, PageUtility.GetRepositoryThumbnailDiskPath, New dtoRevisionMessage() With {.SmtpConfig = PageUtility.CurrentSmtpConfig, .Subject = Resource.getValue("AcceptUserRequest." & CallType.ToString() & ".Subject"), .Body = Resource.getValue("RefuseUserRequest." & CallType.ToString() & ".Body")}, PageUtility.ApplicationUrlBase)
        Else
            DisplayRevisionError(RevisionErrorView.NoFieldsToReview)
        End If
    End Sub
    Private Sub BTNcancelUserRevisionRequest_Click(sender As Object, e As System.EventArgs) Handles BTNcancelUserRevisionRequest.Click
        Me.CurrentPresenter.RemoveUserRequest(IdSubmission, IdRevision, Me.TXBfeedback.Text, New dtoRevisionMessage() With {.SmtpConfig = PageUtility.CurrentSmtpConfig, .Subject = Resource.getValue("RemoveUserRequest." & CallType.ToString() & ".Subject"), .Body = Resource.getValue("RemoveUserRequest." & CallType.ToString() & ".Body" & IIf(String.IsNullOrEmpty(Me.TXBfeedback.Text), "", ".Reason"))}, PageUtility.ApplicationUrlBase)
    End Sub
    Private Sub BTNcancelManagerRequest_Click(sender As Object, e As System.EventArgs) Handles BTNcancelManagerRequest.Click
        Me.CurrentPresenter.RemoveManagerRequest(IdSubmission, IdRevision, Me.TXBfeedback.Text, New dtoRevisionMessage() With {.SmtpConfig = PageUtility.CurrentSmtpConfig, .Subject = Resource.getValue("RemoveManagerRequest." & CallType.ToString() & ".Subject"), .Body = Resource.getValue("RemoveManagerRequest." & CallType.ToString() & ".Body" & IIf(String.IsNullOrEmpty(Me.TXBfeedback.Text), "", ".Reason"))}, PageUtility.ApplicationUrlBase)
    End Sub
    Private Sub BTNrefuseUserRequest_Click(sender As Object, e As System.EventArgs) Handles BTNrefuseUserRequest.Click
        Me.CurrentPresenter.RefuseUserRequest(IdSubmission, IdRevision, Me.TXBfeedback.Text, New dtoRevisionMessage() With {.SmtpConfig = PageUtility.CurrentSmtpConfig, .Subject = Resource.getValue("RefuseUserRequest." & CallType.ToString() & ".Subject"), .Body = Resource.getValue("RefuseUserRequest." & CallType.ToString() & ".Body")}, PageUtility.ApplicationUrlBase)
    End Sub
    Private Sub BTNsaveRevisionSettings_Click(sender As Object, e As System.EventArgs) Handles BTNsaveRevisionSettings.Click
        If GetFieldsToReview.Count > 0 Then
            Me.CurrentPresenter.SaveSettings(IdSubmission, IdRevision, Me.RDPdeadline.SelectedDate, GetFieldsToReview, New dtoRevisionMessage() With {.SmtpConfig = PageUtility.CurrentSmtpConfig, .Subject = Resource.getValue("SaveSettings." & CallType.ToString() & ".Subject"), .Body = Resource.getValue("SaveSettings." & CallType.ToString() & ".Body")}, PageUtility.ApplicationUrlBase)
        Else
            DisplayRevisionError(RevisionErrorView.NoFieldsToReview)
        End If
    End Sub
    'Private Sub BTNcancelManagerReview_Click(sender As Object, e As System.EventArgs) Handles BTNcancelManagerReview.Click
    '    Me.CurrentPresenter.RemoveManagerRequest(IdSubmission, IdPendingRevision, New dtoRevisionMessage() With {.SmtpConfig = PageUtility.CurrentSmtpConfig, .Subject = Resource.getValue("RemoveManagerRequest." & CallType.ToString() & ".Subject"), .Body = Resource.getValue("RemoveManagerRequest." & CallType.ToString() & ".Body")}, PageUtility.ApplicationUrlBase)
    'End Sub
    'Private Sub BTNcancelUserRequest_Click(sender As Object, e As System.EventArgs) Handles BTNcancelUserRequest.Click
    '    Me.CurrentPresenter.RemoveSelfRequest(IdSubmission, IdPendingRevision, New dtoRevisionMessage() With {.SmtpConfig = PageUtility.CurrentSmtpConfig, .Subject = Resource.getValue("RemoveSelfRequest." & CallType.ToString() & ".Subject"), .Body = Resource.getValue("RemoveSelfRequest." & CallType.ToString() & ".Body")}, PageUtility.ApplicationUrlBase)
    'End Sub

    'Private Sub BTNacceptUserRequest_Click(sender As Object, e As System.EventArgs) Handles BTNacceptUserRequest.Click
    '    Me.CurrentPresenter.AcceptUserRequest(IdSubmission, IdPendingRevision, New dtoRevisionMessage() With {.SmtpConfig = PageUtility.CurrentSmtpConfig, .Subject = Resource.getValue("RefuseUserRequest." & CallType.ToString() & ".Subject"), .Body = Resource.getValue("RefuseUserRequest." & CallType.ToString() & ".Body")})
    'End Sub

    'Private Sub BTNrefuseUserRequest_Click(sender As Object, e As System.EventArgs) Handles BTNrefuseUserRequest.Click
    '    Me.CurrentPresenter.RefuseUserRequest(IdSubmission, IdPendingRevision, New dtoRevisionMessage() With {.SmtpConfig = PageUtility.CurrentSmtpConfig, .Subject = Resource.getValue("RefuseUserRequest." & CallType.ToString() & ".Subject"), .Body = Resource.getValue("RefuseUserRequest." & CallType.ToString() & ".Body")})
    'End Sub

    Private Function GetFieldsToReview() As List(Of dtoRevisionItem) Implements IViewManageReview.GetFieldsToReview
        Dim items As New List(Of dtoRevisionItem)

        For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTsections.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim oRepeater As Repeater = row.FindControl("RPTfields")
            items.AddRange(GetFieldsToReview(oRepeater))
        Next

        Return items
    End Function

    Private Function GetFieldsToReview(oRepeater As Repeater) As List(Of dtoRevisionItem)
        Dim fields As New List(Of dtoRevisionItem)
        For Each row As RepeaterItem In (From r As RepeaterItem In oRepeater.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim field As New dtoRevisionItem
            Dim renderField As UC_RenderField = row.FindControl("CTRLrenderField")
            If renderField.Selected Then
                fields.Add(New dtoRevisionItem() With {.IdField = renderField.IdField, .Type = FieldRevisionType.Optional})
            End If
        Next
        Return fields
    End Function

    Private Sub HideRevisions()
        For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTsections.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim oRepeater As Repeater = row.FindControl("RPTfields")
            HideFieldRevision(oRepeater)
        Next
    End Sub

    Private Sub HideFieldRevision(oRepeater As Repeater)
        Dim fields As New List(Of dtoRevisionItem)
        For Each row As RepeaterItem In (From r As RepeaterItem In oRepeater.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim field As New dtoRevisionItem
            Dim renderField As UC_RenderField = row.FindControl("CTRLrenderField")
            renderField.HideSelection()
        Next
    End Sub
    Private Sub DisplayRevisionError(errors As RevisionErrorView) Implements IViewManageReview.DisplayRevisionError

    End Sub
#End Region


End Class