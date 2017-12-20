Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation
Imports lm.Comol.Modules.CallForPapers.Domain
Imports lm.ActionDataContract
Imports System.Linq
Imports System.Collections.Generic

Public Class ReviewSubmission
    Inherits PageBase
    Implements IViewReviewSubmission

#Region "Context"
    Private _Presenter As ReviewSubmissionPresenter
    Private ReadOnly Property CurrentPresenter() As ReviewSubmissionPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ReviewSubmissionPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property Portalname As String Implements IViewReviewSubmission.Portalname
        Get
            Return Resource.getValue("Portalname")
        End Get
    End Property
    Private ReadOnly Property PreloadIdCall As Long Implements IViewReviewSubmission.PreloadIdCall
        Get
            If IsNumeric(Me.Request.QueryString("idCall")) Then
                Return CLng(Me.Request.QueryString("idCall"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewReviewSubmission.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdOtherCommunity As Integer Implements IViewReviewSubmission.PreloadIdOtherCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idOtherCommunity")) Then
                Return CInt(Me.Request.QueryString("idOtherCommunity"))
            Else
                Return -1
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadedUniqueID As Guid Implements IViewReviewSubmission.PreloadedUniqueID
        Get
            If String.IsNullOrEmpty(Me.Request.QueryString("UniqueID")) Then
                Return Guid.Empty
            Else
                Try
                    Return New Guid(Me.Request.QueryString("UniqueID"))
                Catch ex As Exception
                    Return Guid.Empty
                End Try
            End If
        End Get
    End Property
    Private Property CallType As CallForPaperType Implements IViewReviewSubmission.CallType
        Get
            Return ViewStateOrDefault("CallType", CallType)
        End Get
        Set(value As CallForPaperType)
            Me.ViewState("CallType") = value
        End Set
    End Property
    Private Property IdCall As Long Implements IViewReviewSubmission.IdCall
        Get
            Return ViewStateOrDefault("IdCall", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdCall") = value
        End Set
    End Property
    Private Property IdCallCommunity As Integer Implements IViewReviewSubmission.IdCallCommunity
        Get
            Return ViewStateOrDefault("IdCallCommunity", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCallCommunity") = value
        End Set
    End Property
    Private Property IdCallModule As Integer Implements IViewReviewSubmission.IdCallModule
        Get
            Return ViewStateOrDefault("IdCallModule", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCallModule") = value
        End Set
    End Property
    Private ReadOnly Property PreloadView As CallStatusForSubmitters Implements IViewReviewSubmission.PreloadView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of CallStatusForSubmitters).GetByString(Request.QueryString("View"), CallStatusForSubmitters.SubmissionOpened)
        End Get
    End Property
    Private Property IdSubmission As Long Implements IViewReviewSubmission.IdSubmission
        Get
            Return ViewStateOrDefault("IdSubmission", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdSubmission") = value
        End Set
    End Property
    Private Property IdRevision As Long Implements IViewReviewSubmission.IdRevision
        Get
            Return ViewStateOrDefault("IdRevision", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdRevision") = value
        End Set
    End Property
    Private Property AllowCompleteSubmission As Boolean Implements IViewReviewSubmission.AllowCompleteSubmission
        Get
            Return ViewStateOrDefault("AllowCompleteSubmission", False)
        End Get
        Set(value As Boolean)
            Me.BTNsubmitRevisionBottom.Enabled = value
            Me.BTNsubmitRevisionTop.Visible = value
            LTsaveAndSubmitBottomRevisionExplanation.Visible = value
            ViewState("AllowCompleteSubmission") = value
        End Set
    End Property
    Private Property AllowDeleteSubmission As Boolean Implements IViewReviewSubmission.AllowDeleteSubmission
        Get
            Return ViewStateOrDefault("AllowDeleteSubmission", False)
        End Get
        Set(value As Boolean)
            ' Me.BTNvirtualDeleteSubmission.Visible = value
            ViewState("AllowDeleteSubmission") = value
        End Set
    End Property
    Private Property AllowSave As Boolean Implements IViewReviewSubmission.AllowSave
        Get
            Return ViewStateOrDefault("AllowSave", False)
        End Get
        Set(value As Boolean)
            Me.BTNsaveRevisionBottom.Visible = value
            Me.BTNsaveRevisionTop.Visible = value
            ViewState("AllowSave") = value
        End Set
    End Property

    Private Property InitSubmissionTime As DateTime Implements IViewReviewSubmission.InitSubmissionTime
        Get
            Return ViewStateOrDefault("InitSubmissionTime", DateTime.Now)
        End Get
        Set(ByVal value As DateTime)
            Me.ViewState("InitSubmissionTime") = value
        End Set
    End Property
    Private Property TryToComplete As Boolean Implements IViewReviewSubmission.TryToComplete
        Get
            Return ViewStateOrDefault("TryToComplete", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("TryToComplete") = value
        End Set
    End Property
    Private ReadOnly Property AnonymousOwnerName As String Implements IViewReviewSubmission.AnonymousOwnerName
        Get
            Return Me.Resource.getValue("AnonymousOwnerName")
        End Get
    End Property
    Private Property IdSubmitterType As Long Implements IViewReviewSubmission.IdSubmitterType
        Get
            Return ViewStateOrDefault("IdSubmitterType", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdSubmitterType") = value
        End Set
    End Property
    Private ReadOnly Property PreloadedIdSubmission As Long Implements IViewReviewSubmission.PreloadedIdSubmission
        Get
            If IsNumeric(Me.Request.QueryString("idSubmission")) Then
                Return CLng(Me.Request.QueryString("idSubmission"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadedIdRevision As Long Implements IViewReviewSubmission.PreloadedIdRevision
        Get
            If IsNumeric(Me.Request.QueryString("idRevision")) Then
                Return CLng(Me.Request.QueryString("idRevision"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private Property FieldsToReview As List(Of Long) Implements IViewReviewSubmission.FieldsToReview
        Get
            Return ViewStateOrDefault("FieldsToReview", New List(Of Long))
        End Get
        Set(value As List(Of Long))
            Me.ViewState("FieldsToReview") = value
        End Set
    End Property
    Private Property CallRepository As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier Implements IViewReviewSubmission.CallRepository
        Get
            Return ViewStateOrDefault("CallRepository", New lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier() With {.Type = lm.Comol.Core.FileRepository.Domain.RepositoryType.Community})
        End Get
        Set(value As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier)
            Me.ViewState("CallRepository") = value
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
        Me.CurrentPresenter.InitView()
    End Sub
    Public Overrides Sub BindNoPermessi()
        MLVpreview.SetActiveView(VIWempty)
        Me.LBpreviewMessage.Text = Resource.getValue("ServiceReviewSubmissionNopermission")
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
            .setHyperLink(HYPrevisions, True, True)
            .setButton(BTNsaveRevisionBottom, True, , , True)
            .setButton(BTNsaveRevisionTop, True, , , True)
            .setButton(BTNsubmitRevisionBottom, True, , , True)
            .setButton(BTNsubmitRevisionTop, True, , , True)
            .setLabel(LBcallDescriptionTitle)
            .setLiteral(LTsaveAndSubmitBottomRevisionExplanation)
            .setLiteral(LTsaveBottomRevisionExplanation)
            .setLiteral(LTscriptOpen)
            '   .setLabel(LBtimeValidity_t)
        End With
    End Sub
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idRevision As Long, action As ModuleCallForPaper.ActionType) Implements IViewReviewSubmission.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleCallForPaper.ObjectType.Revision, idRevision.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idRevision As Long, action As ModuleRequestForMembership.ActionType) Implements IViewReviewSubmission.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleRequestForMembership.ObjectType.Revision, idRevision.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewReviewSubmission.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, IIf(CallType = CallForPaperType.CallForBids, ModuleCallForPaper.ActionType.NoPermission, ModuleRequestForMembership.ActionType.NoPermission), , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewReviewSubmission.DisplaySessionTimeout
        DisplaySessionTimeout(RootObject.UserReviewCall(CallType, PreloadIdCall, PreloadedIdSubmission, PreloadedIdRevision, PreloadView, PreloadIdOtherCommunity))
    End Sub
    Private Sub DisplaySessionTimeout(url As String) Implements IViewReviewSubmission.DisplaySessionTimeout
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
    Private Sub LoadError(errorView As RevisionErrorView) Implements IViewReviewSubmission.LoadError
        Me.DVmessages.Visible = (errorView <> RevisionErrorView.None) OrElse DVtimeValidity.Visible
        Me.DVmessage.Visible = (errorView <> RevisionErrorView.None)
        LBdisplayMessage.Text = Me.Resource.getValue("RevisionErrorView." & CallType.ToString & "." & errorView.ToString())
    End Sub

    Public Sub DisplayPendingRequest(revision As dtoRevisionRequest) Implements IViewReviewSubmission.DisplayPendingRequest
        Me.DVmessages.Visible = True
        Me.DVtimeValidity.Visible = True

        Me.LTrevisionAction_t.Text = Resource.getValue("RevisionAction." & revision.Type.ToString)
        Me.LTrevisionActionBy_t.Visible = (revision.Type = RevisionType.Manager)

        If revision.Type = RevisionType.UserRequired AndAlso Not IsNothing(revision.RequiredTo) Then
            LBrevisionActionBy.Text = revision.RequiredTo.SurnameAndName
        ElseIf revision.Type = RevisionType.Manager AndAlso Not IsNothing(revision.RequiredBy) Then
            Me.LBrevisionActionBy.Text = revision.RequiredBy.SurnameAndName
        Else
            LTrevisionActionBy_t.Visible = False
            LBrevisionActionBy.Visible = False
        End If
        LBrevisionMessage.Text = revision.Reason

        LTrevisionActionDeadline_t.Visible = revision.EndDate.HasValue
        If revision.EndDate.HasValue Then
            Resource.setLiteral(LTrevisionActionDeadline_t)
            LBrevisionActionDeadline.Text = revision.EndDate.Value.ToShortDateString & " " & revision.EndDate.Value.ToString("HH:mm")
        Else
            LBrevisionActionDeadline.Text = Resource.getValue("revisionActionDeadline")
        End If
        Resource.setLiteral(LTrevisionActionManagedBy_t)
        LTrevisionActionManagedBy_t.Visible = False
        LTrevisionStatus.Visible = False

        LTrevisionActionManagedBy_t.Visible = False
        LBrevisionActionManagedBy.Visible = False
        DVrevisionDate.Visible = False
        DVrevisionAction.Visible = False
        Select Case revision.Status
            Case RevisionStatus.Required
                DVrevisionDate.Visible = True
            Case RevisionStatus.Approved, RevisionStatus.Cancelled, RevisionStatus.RequestAccepted
                DVrevisionDate.Visible = (revision.Status <> RevisionStatus.Cancelled)
                LTrevisionStatus.Text = Resource.getValue("LTrevisionStatus." & revision.Status.ToString)
                LTrevisionActionManagedBy_t.Visible = True
                Me.Resource.setLiteral(LTrevisionActionManagedBy_t)

                If revision.Status = RevisionStatus.Cancelled Then
                    LBrevisionActionManagedBy.Text = revision.RequiredBy.SurnameAndName
                    LBrevisionActionManagedBy.Visible = True
                Else
                    If revision.Type = RevisionType.Manager Then
                        LBrevisionActionManagedBy.Text = revision.RequiredBy.SurnameAndName
                        LBrevisionActionManagedBy.Visible = True
                    ElseIf revision.Type = RevisionType.UserRequired Then
                        LBrevisionActionManagedBy.Text = revision.RequiredTo.SurnameAndName
                        LBrevisionActionManagedBy.Visible = True
                    End If
                End If
                LTrevisionStatus.Visible = True
            Case RevisionStatus.Submitted
                DVrevisionAction.Visible = True
                LTrevisionStatus.Text = Resource.getValue("LTrevisionStatus." & revision.Status.ToString)

                If Not IsNothing(revision.SubmittedBy) Then
                    LTrevisionActionManagedBy_t.Visible = True
                    Me.Resource.setLiteral(LTrevisionActionManagedBy_t)
                    LBrevisionActionManagedBy.Text = revision.SubmittedBy.SurnameAndName
                    LBrevisionActionManagedBy.Visible = True
                End If

                LTrevisionActionSubmittedOn.Visible = revision.SubmittedOn.HasValue
                If revision.SubmittedOn.HasValue Then
                    Me.Resource.setLiteral(LTrevisionActionSubmittedOn)
                    LTrevisionActionSubmittedOn.Text = String.Format(LTrevisionActionSubmittedOn.Text, revision.SubmittedOn.Value.ToShortDateString, revision.SubmittedOn.Value.ToString("HH:mm"))
                    LBrevisionActionManagedBy.Visible = True
                End If
                LTrevisionStatus.Visible = True
                Me.Resource.setLabel(LBrevisionExpectedAction)
        End Select
    End Sub

    Public Sub DisplayRevisionUnavailable() Implements IViewReviewSubmission.DisplayRevisionUnavailable
        Me.DVmessages.Visible = True
        Me.DVmessage.Visible = True
        Me.AllowSave = False
        Me.AllowCompleteSubmission = False
        Me.AllowDeleteSubmission = False
        LBdisplayMessage.Text = Me.Resource.getValue("RevisionErrorView." & CallType.ToString & "." & RevisionErrorView.Unavailable)
    End Sub

    Public Sub DisplayRevisionUnknown() Implements IViewReviewSubmission.DisplayRevisionUnknown
        Me.DVmessages.Visible = True
        Me.DVmessage.Visible = True
        Me.AllowSave = False
        Me.AllowCompleteSubmission = False
        Me.AllowDeleteSubmission = False
        LBdisplayMessage.Text = Me.Resource.getValue("RevisionErrorView." & CallType.ToString & "." & RevisionErrorView.Unknown)
    End Sub

    Private Sub DisplayRevisionTimeExpired() Implements IViewReviewSubmission.DisplayRevisionTimeExpired
        Me.DVmessages.Visible = True
        Me.DVmessage.Visible = True
        Me.AllowSave = False
        Me.AllowCompleteSubmission = False
        Me.AllowDeleteSubmission = False
        LBdisplayMessage.Text = Me.Resource.getValue("RevisionErrorView." & CallType.ToString & "." & RevisionErrorView.TimeExpired)
    End Sub
    Public Sub DisplayCallUnavailableForPublic() Implements IViewReviewSubmission.DisplayCallUnavailableForPublic
        MLVpreview.SetActiveView(VIWempty)
        Me.LBpreviewMessage.Text = Me.Resource.getValue("DisplayCallUnavailableForPublic." & CallType.ToString())
    End Sub
    Public Sub DisplayCallUnavailable() Implements IViewReviewSubmission.DisplayCallUnavailable
        MLVpreview.SetActiveView(VIWempty)
        Me.LBpreviewMessage.Text = Me.Resource.getValue("DisplayCallUnavailable." & CallType.ToString())
    End Sub

    Private Sub InitializeView(displayProgress As Boolean) Implements IViewReviewSubmission.InitializeView
        Me.BTNsaveRevisionBottom.OnClientClick = IIf(displayProgress, "updateProgress();", "")
        Me.BTNsaveRevisionTop.OnClientClick = IIf(displayProgress, "updateProgress();", "")
        Me.BTNsubmitRevisionBottom.OnClientClick = IIf(displayProgress, "updateProgress();", "")
        Me.BTNsubmitRevisionTop.OnClientClick = IIf(displayProgress, "updateProgress();", "")
        If displayProgress Then
            DirectCast(Me.Master.FindControl("SCMmanager"), ScriptManager).RegisterPostBackControl(Me.BTNsaveRevisionBottom)
            DirectCast(Me.Master.FindControl("SCMmanager"), ScriptManager).RegisterPostBackControl(Me.BTNsaveRevisionTop)
            DirectCast(Me.Master.FindControl("SCMmanager"), ScriptManager).RegisterPostBackControl(Me.BTNsubmitRevisionBottom)
            DirectCast(Me.Master.FindControl("SCMmanager"), ScriptManager).RegisterPostBackControl(Me.BTNsubmitRevisionTop)
        End If
    End Sub
    Private Sub SetContainerName(name As String, itemName As String) Implements IViewReviewSubmission.SetContainerName
        Dim translation As String = Me.Resource.getValue("serviceTitle.Review." & CallType.ToString())
        Dim title As String = ""
        If String.IsNullOrEmpty(itemName) Then
            title = String.Format(translation, "", "")
        Else
            title = String.Format(translation, IIf(Len(itemName) > 70, Left(itemName, 70) & "...", itemName))
        End If
        If Not String.IsNullOrEmpty(itemName) Then
            Me.Master.ServiceTitleToolTip = String.Format(translation, itemName)

        ElseIf Not String.IsNullOrEmpty(name) Then
            Dim tooltip As String = Me.Resource.getValue("serviceTitle.Community.Review." & CallType.ToString())
            If Not String.IsNullOrEmpty(tooltip) Then
                Me.Master.ServiceTitleToolTip = String.Format(tooltip, name)
            End If
        End If
        Me.Master.ServiceTitle = title

        ContainerName = title
    End Sub

    Private Sub LoadUnknowCall(idCommunity As Integer, idModule As Integer, idCall As Long, type As CallForPaperType) Implements IViewReviewSubmission.LoadUnknowCall
        Me.MLVpreview.SetActiveView(VIWempty)
        Me.LBpreviewMessage.Text = Resource.getValue("LBnoCalls." & type.ToString)
    End Sub
    Private Sub LoadAttachments(items As List(Of dtoAttachmentFile)) Implements IViewReviewSubmission.LoadAttachments
        Me.RPTattachments.Visible = (items.Count > 0)
        Me.RPTattachments.DataSource = items
        Me.RPTattachments.DataBind()
    End Sub
    Private Sub LoadCallInfo(baseCall As dtoCall) Implements IViewReviewSubmission.LoadCallInfo
        Me.MLVpreview.SetActiveView(VIWcall)
        Me.LTcallDescription.Text = baseCall.Description
        Dim itemName As String = baseCall.Name
        Me.LBcallDescriptionTitle.Text = String.Format(Resource.getValue("DescriptionTitle." & baseCall.Type.ToString()), IIf(Len(itemName) > 70, Left(itemName, 70) & "...", itemName))
        Me.LBcallDescriptionTitle.ToolTip = String.Format(Resource.getValue("DescriptionTitle." & baseCall.Type.ToString() & ".ToolTip"), itemName)
    End Sub
    Public Sub LoadCallInfo(baseCall As dtoRequest) Implements IViewReviewSubmission.LoadCallInfo
        Me.MLVpreview.SetActiveView(VIWcall)
        Me.LTcallDescription.Text = baseCall.StartMessage

        Dim itemName As String = baseCall.Name
        Me.LBcallDescriptionTitle.Text = String.Format(Resource.getValue("DescriptionTitle." & baseCall.Type.ToString()), IIf(Len(itemName) > 70, Left(itemName, 70) & "...", itemName))
        Me.LBcallDescriptionTitle.ToolTip = String.Format(Resource.getValue("DescriptionTitle." & baseCall.Type.ToString() & ".ToolTip"), itemName)
    End Sub

    Private Sub LoadSections(sections As List(Of dtoCallSection(Of dtoSubmissionValueField))) Implements IViewReviewSubmission.LoadSections
        Me.DVbottomBox.Visible = (sections.Count > 0) AndAlso (AllowCompleteSubmission OrElse AllowSave)
        'Me.BTNvirtualDeleteSubmission.Visible = (sections.Count > 0)
        'Me.BTNsubmitRevisionTop.Visible = (sections.Count > 0)
        'Me.BTNsaveRevisionBottom.Visible = (sections.Count > 0)
        Me.RPTsections.DataSource = sections
        Me.RPTsections.DataBind()
    End Sub
    Private Sub SetActionUrl(action As CallStandardAction, url As String) Implements IViewReviewSubmission.SetActionUrl
        Select Case action
            Case CallStandardAction.List
                Me.HYPlist.NavigateUrl = BaseUrl & url
                Me.HYPlist.Visible = True
            Case CallStandardAction.ViewRevisions
                Me.HYPrevisions.NavigateUrl = BaseUrl & url
                Me.HYPrevisions.Visible = True
        End Select
    End Sub
    Private Sub GoToUrl(url As String) Implements IViewReviewSubmission.GoToUrl
        PageUtility.RedirectToUrl(url)
    End Sub
    Private Function GetValues() As List(Of dtoSubmissionValueField) Implements IViewReviewSubmission.GetValues
        Dim fields As New List(Of dtoSubmissionValueField)
        For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTsections.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim oRepeater As Repeater = row.FindControl("RPTfields")
            fields.AddRange(GetFields(oRepeater))
        Next
        Return fields
    End Function
    Private Function GetFields(oRepeater As Repeater) As List(Of dtoSubmissionValueField)
        Dim fields As New List(Of dtoSubmissionValueField)
        For Each row As RepeaterItem In (From r As RepeaterItem In oRepeater.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim field As New dtoSubmissionValueField
            Dim oControl As UC_InputField = row.FindControl("CTRLinputField")

            If oControl.Visible AndAlso oControl.FieldType <> FieldType.FileInput Then
                field = oControl.GetField
                fields.Add(field)
            End If

        Next
        Return fields
    End Function
    Private Function GetFileValues(submission As UserSubmission, moduleCode As String, idModule As Integer, moduleAction As Integer, objectType As Integer) As List(Of dtoSubmissionFileValueField) Implements IViewReviewSubmission.GetFileValues
        Dim fields As List(Of dtoSubmissionFileValueField) = New List(Of dtoSubmissionFileValueField)
        For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTsections.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim oRepeater As Repeater = row.FindControl("RPTfields")
            fields.AddRange(GetFileFields(oRepeater, submission, moduleCode, idModule, moduleAction, objectType))
        Next
        Return fields
    End Function
    Private Function GetFileFields(oRepeater As Repeater, submission As UserSubmission, moduleCode As String, idModule As Integer, moduleAction As Integer, objectType As Integer) As List(Of dtoSubmissionFileValueField)
        Dim fields As New List(Of dtoSubmissionFileValueField)
        For Each row As RepeaterItem In (From r As RepeaterItem In oRepeater.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim field As New dtoSubmissionFileValueField
            Dim oControl As UC_InputField = row.FindControl("CTRLinputField")
            If oControl.Visible AndAlso oControl.FieldType = FieldType.FileInput Then
                field = oControl.AddInternalFile(submission, moduleCode, idModule, moduleAction, objectType)
                If Not IsNothing(field) Then
                    fields.Add(field)
                End If
            End If
        Next
        Return fields
    End Function
#End Region

#Region "Repeater binding"
    Private Sub RPTattachments_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTattachments.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dto As dtoAttachmentFile = DirectCast(e.Item.DataItem, dtoAttachmentFile)
            Dim renderItem As UC_RepositoryRenderAction = e.Item.FindControl("CTRLdisplayItem")
            Dim initializer As New lm.Comol.Core.ModuleLinks.dtoObjectRenderInitializer
            initializer.RefreshContainerPage = False
            initializer.SaveObjectStatistics = True
            initializer.Link = dto.Link
            initializer.SetOnModalPageByItem = True
            initializer.SetPreviousPage = False

            renderItem.InitializeControl(initializer, lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction)
            'Dim renderFile As UC_ModuleRepositoryAction = e.Item.FindControl("CTRLdisplayFile")

            'Dim initializer As New lm.Comol.Core.ModuleLinks.dtoModuleDisplayActionInitializer

            '' DIMENSIONI IMMAGINI
            'initializer.IconSize = Helpers.IconSize.Small
            'renderFile.EnableAnchor = True
            'initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction
            'initializer.Link = dto.Link
            'renderFile.InsideOtherModule = True
            'Dim actions As List(Of dtoModuleActionControl)
            'actions = renderFile.InitializeRemoteControl(initializer, StandardActionType.Play)

            Dim oLabel As Label = e.Item.FindControl("LBattachmentDescription")
            oLabel.Text = dto.Description

            Dim oControl As HtmlGenericControl = e.Item.FindControl("DVdescription")
            oControl.Visible = Not String.IsNullOrEmpty(dto.Description)


        ElseIf e.Item.ItemType = ListItemType.Header Then
            Dim oLiteral As Literal = e.Item.FindControl("LTattachmentsTitle")
            Resource.setLiteral(oLiteral)
        End If
    End Sub

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


            Dim inputField As UC_InputField = e.Item.FindControl("CTRLinputField")
            Dim renderField As UC_RenderField = e.Item.FindControl("CTRLrenderField")
            If FieldsToReview.Contains(field.IdField) Then
                inputField.Visible = True
                If inputField.CurrentError <> FieldError.None OrElse field.FieldError <> FieldError.None Then
                    inputField.InitializeControl(IdCall, IdSubmission, CallRepository, field, Not AllowSave, False, field.FieldError)
                Else
                    inputField.InitializeControl(IdCall, IdSubmission, CallRepository, field, Not AllowSave, False)
                End If
            Else
                renderField.Visible = True
                If inputField.CurrentError <> FieldError.None OrElse field.FieldError <> FieldError.None Then
                    renderField.InitializeControl(field, True, False, FieldsToReview.Contains(field.IdField), field.FieldError)
                Else
                    renderField.InitializeControl(field, True, False, FieldsToReview.Contains(field.IdField))
                End If
            End If
        End If
    End Sub
    Protected Sub RPTfields_ItemCreated(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim inputField As UC_InputField = e.Item.FindControl("CTRLinputField")

            AddHandler inputField.RemoveFile, AddressOf OnRemoveFieldFile
        End If
    End Sub
    Protected Sub OnRemoveFieldFile(ByVal idSubmittedField As Long)
        Me.CurrentPresenter.RemoveFieldFile(idSubmittedField, GetValues())
    End Sub
#End Region

    Private Sub BTNsaveSubmissionBottom_Click(sender As Object, e As System.EventArgs) Handles BTNsaveRevisionBottom.Click, BTNsaveRevisionTop.Click
        Me.DVmessages.Visible = False
        Me.CurrentPresenter.SaveRevision(GetValues())
    End Sub

    Private Sub BTNsubmitSubmissionBottom_Click(sender As Object, e As System.EventArgs) Handles BTNsubmitRevisionBottom.Click, BTNsubmitRevisionTop.Click
        Dim baseFilePath As String = ""
        If Me.SystemSettings.File.Materiale.DrivePath = "" Then
            baseFilePath = Server.MapPath(Me.PageUtility.BaseUrl & Me.SystemSettings.File.Materiale.VirtualPath)
        Else
            baseFilePath = Me.SystemSettings.File.Materiale.DrivePath
        End If

        Me.CurrentPresenter.SaveCompleteRevision(GetValues(), baseFilePath, Me.Resource.getValue("SubmissionTranslations." & SubmissionTranslations.FilesSubmitted.ToString), PageUtility.ApplicationUrlBase, New dtoRevisionMessage() With {.SmtpConfig = PageUtility.CurrentSmtpConfig, .Subject = Resource.getValue("SubmitRevision." & CallType.ToString() & ".Subject"), .Body = Resource.getValue("SubmitRevision." & CallType.ToString() & ".Body")})
    End Sub

    'Private Sub BTNvirtualDeleteSubmission_Click(sender As Object, e As System.EventArgs) Handles BTNvirtualDeleteSubmission.Click
    '    Me.CurrentPresenter.VirtualDeleteSubmission()
    'End Sub

    Private Sub SubmitCall_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Me.Master.ShowDocType = True
    End Sub

End Class