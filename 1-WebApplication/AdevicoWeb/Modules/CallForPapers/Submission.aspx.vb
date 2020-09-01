Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation
Imports lm.Comol.Modules.CallForPapers.Domain
Imports lm.ActionDataContract
Imports System.Linq
Imports System.Collections.Generic
Imports lm.Comol.Core.DomainModel.Helpers.Export
Imports System.Globalization

Public Class Submission
    Inherits PageBase
    Implements IViewViewSubmission

#Region "Context"
    Private _Presenter As ViewSubmissionPresenter
    Private ReadOnly Property CurrentPresenter() As ViewSubmissionPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ViewSubmissionPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property Portalname As String Implements IViewViewSubmission.Portalname
        Get
            Return Resource.getValue("Portalname")
        End Get
    End Property
    Private ReadOnly Property PreloadIdCall As Long Implements IViewViewSubmission.PreloadIdCall
        Get
            If IsNumeric(Me.Request.QueryString("idCall")) Then
                Return CLng(Me.Request.QueryString("idCall"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewViewSubmission.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadedUniqueID As Guid Implements IViewViewSubmission.PreloadedUniqueID
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
    Private ReadOnly Property PreloadedIdSubmission As Long Implements IViewViewSubmission.PreloadedIdSubmission
        Get
            If IsNumeric(Me.Request.QueryString("idSubmission")) Then
                Return CLng(Me.Request.QueryString("idSubmission"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadedIdRevision As Long Implements IViewViewSubmission.PreloadedIdRevision
        Get
            If IsNumeric(Me.Request.QueryString("idRevision")) Then
                Return CLng(Me.Request.QueryString("idRevision"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdOtherCommunity As Integer Implements IViewViewSubmission.PreloadIdOtherCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idOtherCommunity")) Then
                Return CInt(Me.Request.QueryString("idOtherCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadView As CallStatusForSubmitters Implements IViewViewSubmission.PreloadView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of CallStatusForSubmitters).GetByString(Request.QueryString("View"), CallStatusForSubmitters.SubmissionOpened)
        End Get
    End Property
    Private ReadOnly Property PreloadFilterSubmission As SubmissionFilterStatus Implements IViewViewSubmission.PreloadFilterSubmission
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of SubmissionFilterStatus).GetByString(Request.QueryString("filter"), SubmissionFilterStatus.OnlySubmitted)
        End Get
    End Property
    Private ReadOnly Property PreloadFromManagement As Boolean Implements IViewViewSubmission.PreloadFromManagement
        Get
            Return (Request.QueryString("Manage") = "true" OrElse Request.QueryString("Manage") = "True")
        End Get
    End Property
    Private ReadOnly Property PreloadOrderSubmission As SubmissionsOrder Implements IViewViewSubmission.PreloadOrderSubmission
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of SubmissionsOrder).GetByString(Request.QueryString("OrderBy"), SubmissionsOrder.ByType)
        End Get
    End Property
    Private ReadOnly Property PreloadAscending As Boolean Implements IViewViewSubmission.PreloadAscending
        Get
            If String.IsNullOrEmpty(Request.QueryString("Ascending")) Then
                Return False
            Else
                Return (Request.QueryString("Ascending") = "true" OrElse Request.QueryString("Ascending") = "True")
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadPageIndex As Integer Implements IViewViewSubmission.PreloadPageIndex
        Get
            If IsNumeric(Me.ViewState("pageIndex")) Then
                Return CInt(Me.ViewState("pageIndex"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadPageSize As Integer Implements IViewViewSubmission.PreloadPageSize
        Get
            If IsNumeric(Me.ViewState("pageSize")) Then
                Return CInt(Me.ViewState("pageSize"))
            Else
                Return 50
            End If
        End Get
    End Property
    Private Property IdRevision As Long Implements IViewViewSubmission.IdRevision
        Get
            Return ViewStateOrDefault("IdRevision", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdRevision") = value
        End Set
    End Property
   
    Private ReadOnly Property AnonymousOwnerName As String Implements IViewViewSubmission.AnonymousOwnerName
        Get
            Return Me.Resource.getValue("AnonymousOwnerName")
        End Get
    End Property
    Private Property ShowAdministrationTools As Boolean Implements IViewViewSubmission.ShowAdministrationTools
        Get
            Return ViewStateOrDefault("ShowAdministrationTools", False)
        End Get
        Set(value As Boolean)
            ViewState("ShowAdministrationTools") = value
            SPNmanage.Visible = value
            If value Then
                DVtopMenu.Visible = True
            End If
        End Set
    End Property
    Private Property CallType As CallForPaperType Implements IViewViewSubmission.CallType
        Get
            Return ViewStateOrDefault("CallType", CallForPaperType.CallForBids)
        End Get
        Set(value As CallForPaperType)
            Me.ViewState("CallType") = value
        End Set
    End Property
    Private Property IdCall As Long Implements IViewViewSubmission.IdCall
        Get
            Return ViewStateOrDefault("IdCall", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdCall") = value
        End Set
    End Property
    Private Property IdCallCommunity As Integer Implements IViewViewSubmission.IdCallCommunity
        Get
            Return ViewStateOrDefault("IdCallCommunity", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCallCommunity") = value
        End Set
    End Property
    Private Property IdCallModule As Integer Implements IViewViewSubmission.IdCallModule
        Get
            Return ViewStateOrDefault("IdCallModule", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCallModule") = value
        End Set
    End Property

    Private Property IdSubmission As Long Implements IViewViewSubmission.IdSubmission
        Get
            Return ViewStateOrDefault("IdSubmission", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdSubmission") = value
        End Set
    End Property
    Private Property IdSubmitterType As Long Implements IViewViewSubmission.IdSubmitterType
        Get
            Return ViewStateOrDefault("IdSubmitterType", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdSubmitterType") = value
        End Set
    End Property
    Private Property isAnonymousSubmission As Boolean Implements IViewViewSubmission.isAnonymousSubmission
        Get
            Return ViewStateOrDefault("isAnonymousSubmission", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("isAnonymousSubmission") = value
        End Set
    End Property
    Private Property CallRepository As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier Implements IViewViewBaseSubmission.CallRepository
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

        If Not Page.IsPostBack() Then
            CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.SubmissionView,
            Me.IdSubmission,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.UserSubmission,
            "BindDati")
        End If

        Me.CurrentPresenter.InitView(False)
        Me.Uc_AdvComments.InitUc(Me.CommissionId, Me.IdSubmission)


        RDPintegrationEnd.SelectedDate = Date.Now.AddDays(7)

    End Sub
    Public Overrides Sub BindNoPermessi()
        MLVpreview.SetActiveView(VIWempty)
        Me.LBemptyMessage.Text = Resource.getValue("ServiceViewSubmissionNopermission")

        CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.SubmissionView,
            Me.IdSubmission,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.UserSubmission,
            "NoPermission")
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
            .setHyperLink(HYPsubmissionsList, True, True)
            .setLabel(LBcallDescriptionTitle)
            .setLabel(LBtimeValidity_t)

            .setButton(BTNaccept, True, , , True)
            .setButton(BTNrefuse, True, , , True)
            .setButton(BTNsubmitForUser, True, , , True)

            .setLiteral(LTowner_t)
            .setLiteral(LTsubmitterType_t)
            .setLiteral(LTsubmissionStatus_t)
            .setLiteral(LTsubmittedOn_t)
            .setLiteral(LTsubmittedBy_t)
            .setLiteral(LTrevisionListTitle_t)


            .setLabel(LBdeadline_t)
            .setLabel(LBrequestReason_t)
            LTmaxCharsrequest.Text = .getValue("MaxCharsInfo")
            .setLabel(LBreasonHelp)
            .setButton(BTNaddRequest, True, , , True)
            .setButton(BTNundoRequest, True, , , True)

            .setLabel(LBrequiredRevision_t)
            .setHyperLink(HTPreviewUserSubmission, True, True)
            .setHyperLink(HTPviewUserRequest, True, True)
            .setButton(BTNcancelUserRequest, True, , , True)

            .setLiteral(LTrevisionRequired_t)
            .setLiteral(LTrevisionRequiredBy_t)
            .setLiteral(LTdeadline_t)
            .setHyperLink(HTPreviewManagerSubmission, True, True)
            .setButton(BTNcancelManagerReview, True, , , True)

            .setLiteral(LTrevisionRequiredInfo_t)
            .setLiteral(LTrevisionRequiredByInfo_t)
            .setLiteral(LTrevisionManagedByInfo_t)
            .setLiteral(LTrevisionDate_t)

            .setButton(BTNrefuseUserRequest, True, , , True)
            .setButton(BTNacceptUserRequest, True, , , True)

            'Signs
            .setLabel(LBnotSigned_t)
            .setLabel(LBsubmitted)
            .setLabel(LBtoSubmit_t)
            .setLinkButton(LKBupload, True, True, False, False)

            .setLabel(LBL_Sign_t)
            .setLabel(LBL_Sign)

            .setLinkButton(LKBprintForSign, True, True)

        End With
    End Sub
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As ModuleCallForPaper.ActionType) Implements IViewViewSubmission.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleCallForPaper.ObjectType.CallForPaper, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As ModuleRequestForMembership.ActionType) Implements IViewViewSubmission.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleRequestForMembership.ObjectType.RequestForMembership, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewViewSubmission.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, ModuleCallForPaper.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewViewSubmission.DisplaySessionTimeout
        Dim url As String = ""
        If Me.PreloadFromManagement Then
            url = RootObject.ViewSubmissionAsManager(CallType, PreloadIdCall, PreloadedIdSubmission, PreloadedIdRevision, PreloadedUniqueID, False, PreloadView, PreloadFilterSubmission, PreloadOrderSubmission, PreloadAscending, PreloadPageIndex, PreloadPageSize, 0)
        Else
            url = RootObject.ViewSubmission(CallType, PreloadIdCall, PreloadedIdSubmission, PreloadedUniqueID, False, False, PreloadView, PreloadIdOtherCommunity, 0)
        End If

        CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.SubmissionView,
            Me.IdSubmission,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.UserSubmission,
            "SessionTimeOut")

        DisplaySessionTimeout(url)
    End Sub
    Private Sub DisplaySessionTimeout(url As String) Implements IViewViewBaseSubmission.DisplaySessionTimeout
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
    Private Sub DisplayCallUnavailableForPublic() Implements IViewViewSubmission.DisplayCallUnavailableForPublic
        MLVpreview.SetActiveView(VIWempty)
        Me.LBemptyMessage.Text = Me.Resource.getValue("DisplayCallUnavailableForPublic.View." & CallForPaperType.CallForBids.ToString())
    End Sub
    Private Sub DisplaySubmissionUnavailable() Implements IViewViewSubmission.DisplaySubmissionUnavailable
        Me.MLVpreview.SetActiveView(VIWempty)
        Me.LBemptyMessage.Text = Resource.getValue("DisplaySubmissionUnavailable.View." & CallType.ToString)
    End Sub
    Private Sub DisplayUnknownSubmission(idCommunity As Integer, idModule As Integer, idSubmission As Long, type As CallForPaperType) Implements IViewViewSubmission.DisplayUnknownSubmission
        Me.MLVpreview.SetActiveView(VIWempty)
        Me.LBemptyMessage.Text = Resource.getValue("DisplayUnknownSubmission." & type.ToString)
        PageUtility.AddActionToModule(idCommunity, idModule, IIf(type = CallForPaperType.CallForBids, ModuleCallForPaper.ActionType.GenericError, ModuleRequestForMembership.ActionType.GenericError), PageUtility.CreateObjectsList(idModule, IIf(type = CallForPaperType.CallForBids, ModuleCallForPaper.ObjectType.UserSubmission, ModuleRequestForMembership.ObjectType.UserSubmission), idSubmission.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub DisplayUnknownCall(idCommunity As Integer, idModule As Integer, idCall As Long, type As CallForPaperType) Implements IViewViewSubmission.DisplayUnknownCall
        Me.MLVpreview.SetActiveView(VIWempty)
        Me.LBemptyMessage.Text = Resource.getValue("DisplayUnknownCall." & type.ToString)
        If type = CallForPaperType.CallForBids Then
            SendUserAction(idCommunity, idModule, idCall, ModuleCallForPaper.ActionType.GenericError)
        Else
            SendUserAction(idCommunity, idModule, idCall, ModuleRequestForMembership.ActionType.GenericError)
        End If
    End Sub

    Private Sub SetContainerName(name As String, itemName As String) Implements IViewViewSubmission.SetContainerName
        Dim translation As String = Me.Resource.getValue("serviceTitle.Submit." & CallType.ToString())
        Dim title As String = ""
        If String.IsNullOrEmpty(itemName) Then
            title = String.Format(translation, "", "")
        Else
            title = String.Format(translation, IIf(Len(itemName) > 70, Left(itemName, 70) & "...", itemName))
        End If
        If Not String.IsNullOrEmpty(itemName) Then
            Me.Master.ServiceTitleToolTip = String.Format(translation, itemName)

        ElseIf Not String.IsNullOrEmpty(name) Then
            Dim tooltip As String = Me.Resource.getValue("serviceTitle.Community.Submit." & CallType.ToString())
            If Not String.IsNullOrEmpty(tooltip) Then
                Me.Master.ServiceTitleToolTip = String.Format(tooltip, name)
            End If
        End If
        Me.Master.ServiceTitle = title

        ContainerName = title
    End Sub

    Private Sub LoadAttachments(items As List(Of dtoAttachmentFile)) Implements IViewViewSubmission.LoadAttachments
        Me.RPTattachments.Visible = (items.Count > 0)
        Me.RPTattachments.DataSource = items
        Me.RPTattachments.DataBind()
    End Sub
    Private Sub LoadCallInfo(baseCall As dtoCall) Implements IViewViewSubmission.LoadCallInfo
        FLDcallInfo.Visible = True
        Me.MLVpreview.SetActiveView(VIWcall)
        DVtopMenu.Visible = True
        Me.LTcallDescription.Text = baseCall.Description
        Me.Resource.setLabel(LBstartDate)
        Me.LBstartDate.Text = String.Format(Me.LBstartDate.Text, baseCall.StartDate.ToString("dd/MM/yy"), baseCall.StartDate.ToString("HH:mm"))

        If baseCall.EndDate.HasValue Then
            Me.Resource.setLabel(LBendDate)
            Me.LBendDate.Text = String.Format(Me.LBendDate.Text, baseCall.EndDate.Value.ToString("dd/MM/yy"), baseCall.EndDate.Value.ToString("HH:mm"))
        Else
            Me.LBendDate.Text = Resource.getValue("NoExpirationDate")
        End If
        Me.LBwinnerinfo.Text = baseCall.AwardDate
        Me.LBwinnerinfo.Visible = Not String.IsNullOrEmpty(baseCall.AwardDate)

        Dim itemName As String = baseCall.Name
        Me.LBcallDescriptionTitle.Text = String.Format(Resource.getValue("DescriptionTitle." & baseCall.Type.ToString()), IIf(Len(itemName) > 70, Left(itemName, 70) & "...", itemName))
        Me.LBcallDescriptionTitle.ToolTip = String.Format(Resource.getValue("DescriptionTitle." & baseCall.Type.ToString() & ".ToolTip"), itemName)
    End Sub
    Public Sub LoadCallInfo(baseCall As dtoRequest) Implements IViewViewSubmission.LoadCallInfo
        Me.MLVpreview.SetActiveView(VIWcall)
        DVtopMenu.Visible = True
        Me.LTcallDescription.Text = baseCall.StartMessage
        Me.Resource.setLabel(LBstartDate)
        Me.LBstartDate.Text = String.Format(Me.LBstartDate.Text, baseCall.StartDate.ToString("dd/MM/yy"), baseCall.StartDate.ToString("HH:mm"))

        If baseCall.EndDate.HasValue Then
            Me.Resource.setLabel(LBendDate)
            Me.LBendDate.Text = String.Format(Me.LBendDate.Text, baseCall.EndDate.Value.ToString("dd/MM/yy"), baseCall.EndDate.Value.ToString("HH:mm"))
        Else
            Me.LBendDate.Text = Resource.getValue("NoExpirationDate")
        End If
        Me.LBwinnerinfo.Visible = False

        Dim itemName As String = baseCall.Name
        Me.LBcallDescriptionTitle.Text = String.Format(Resource.getValue("DescriptionTitle." & baseCall.Type.ToString()), IIf(Len(itemName) > 70, Left(itemName, 70) & "...", itemName))
        Me.LBcallDescriptionTitle.ToolTip = String.Format(Resource.getValue("DescriptionTitle." & baseCall.Type.ToString() & ".ToolTip"), itemName)
    End Sub
    Private Sub LoadSubmissionInfo(submitterName As String, ownerName As String, status As SubmissionStatus) Implements IViewViewSubmission.LoadSubmissionInfo
        Me.MLVpreview.SetActiveView(VIWcall)
        DVtopMenu.Visible = True
        Me.LBowner.Text = ownerName
        Me.LBsubmitterType.Text = submitterName
        Me.LBsubmissionStatus.Text = Resource.getValue("SubmissionStatus." & status.ToString)
        Me.LIsubmissionInfo.Visible = False
    End Sub
    Private Sub LoadSubmissionInfo(submitterName As String, ownerName As String, status As SubmissionStatus, submittedOn As DateTime) Implements IViewViewSubmission.LoadSubmissionInfo
        Me.MLVpreview.SetActiveView(VIWcall)
        LoadSubmissionInfo(submitterName, ownerName, status)
        Me.LIsubmissionInfo.Visible = True
        SPNsubmittedBy.Visible = False
        Me.LBsubmittedOnData.Text = submittedOn.ToString("dd/MM/yy")
        Me.LBsubmittedOnTime.Text = submittedOn.ToString("HH:mm")
    End Sub
    Private Sub LoadSubmissionInfo(submitterName As String, ownerName As String, status As SubmissionStatus, submittedOn As DateTime, submittedBy As String) Implements IViewViewSubmission.LoadSubmissionInfo
        Me.MLVpreview.SetActiveView(VIWcall)
        LoadSubmissionInfo(submitterName, ownerName, status)
        Me.LBsubmittedBy.Text = submittedBy
        SPNsubmittedBy.Visible = True
    End Sub
    Private Sub LoadAvailableStatus(items As List(Of SubmissionStatus)) Implements IViewViewSubmission.LoadAvailableStatus
        Me.BTNaccept.Visible = (items.Contains(SubmissionStatus.accepted))
        Me.BTNrefuse.Visible = (items.Contains(SubmissionStatus.rejected))
        Me.BTNsubmitForUser.Visible = (items.Contains(SubmissionStatus.submitted))
    End Sub
    Private Sub UpdateStatus(status As SubmissionStatus) Implements IViewViewSubmission.UpdateStatus
        Me.LBsubmissionStatus.Text = Resource.getValue("SubmissionStatus." & status.ToString)
    End Sub

    Private Sub InitializeExportControl( _
                                       isOwner As Boolean, _
                                       idUser As Integer, _
                                       idCall As Long, _
                                       idSubmission As Long, _
                                       idRevision As Long, _
                                       idModule As Integer, _
                                       idCallCommunity As Integer, _
                                       callType As CallForPaperType, _
                                       ByVal submitterType As Int64, _
                                       ByVal isDraft As Boolean) _
                                   Implements IViewViewSubmission.InitializeExportControl


        Me.CTRLreport.InitializeControl(isOwner, idUser, LBowner.Text, idCall, idSubmission, idRevision, idModule, idCallCommunity, callType)
        CTRLprintDraf.Visible = isDraft

        If (isDraft) Then
            Me.CTRLreport.ShowForDraft()
            CTRLprintDraf.InitializeUC(idCall, submitterType, idRevision, idSubmission)
        End If


    End Sub


    Private Sub LoadSections(sections As List(Of dtoCallSection(Of dtoSubmissionValueField))) Implements IViewViewSubmission.LoadSections
        Me.RPTsections.DataSource = sections
        Me.RPTsections.DataBind()
    End Sub
    Private Sub LoadRequiredFiles(files As List(Of dtoCallSubmissionFile)) Implements IViewViewSubmission.LoadRequiredFiles
        Me.RPTrequiredFiles.Visible = (files.Count > 0)
        Me.RPTrequiredFiles.DataSource = files
        Me.RPTrequiredFiles.DataBind()
    End Sub
    Private Sub SetActionUrl(action As CallStandardAction, url As String) Implements IViewViewSubmission.SetActionUrl
        Select Case action
            Case CallStandardAction.List
                Me.HYPlist.NavigateUrl = BaseUrl & url
                Me.HYPlist.Visible = True
            Case CallStandardAction.Manage
                Me.HYPsubmissionsList.NavigateUrl = BaseUrl & url '.Replace("&cmmId=0", String.Format("&cmmId={1}", Me.CommissionId))
                Me.HYPsubmissionsList.Visible = True
        End Select

        If Me.CommissionId > 0 Then

            If Me.HYPsubmissionsList.NavigateUrl.Contains("&cmmId=0") Then
                Me.HYPsubmissionsList.NavigateUrl = Me.HYPsubmissionsList.NavigateUrl.Replace("&cmmId=0", String.Format("&cmmId={0}", Me.CommissionId))
            Else
                Me.HYPsubmissionsList.NavigateUrl = Me.HYPsubmissionsList.NavigateUrl.Replace("&idCall=", String.Format("&cmmId={0}&idCall=", Me.CommissionId))
            End If
        End If
    End Sub
    Private Sub GoToUrl(url As String) Implements IViewViewSubmission.GoToUrl
        PageUtility.RedirectToUrl(url)
    End Sub
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

            Dim renderField As UC_RenderField = e.Item.FindControl("CTRLrenderField")
            renderField.InitializeControl(field, False, False, field.FieldError, field.AllowSelection)

            Dim integration As Uc_Integration = e.Item.FindControl("CTRLIntegration")
            If Not IsNothing(integration) Then
                integration.InitializeUc(Me.CommissionId, Me.IdSubmission, field.IdField, 0, Me.ComunitaCorrenteID)
            End If



            If field.Field.Type = FieldType.TableSummary Then
                'inputField.InitializeControl(IdCall, IdSubmission, CallRepository, field, Not AllowSave, False)
                renderField.BindTableSummaryReport(Me.CurrentReport)

                'Reset dati
                Me.CurrentReport = New dtoSummaryTableReportTotal()
            End If

            Dim itemSummary As lm.Comol.Modules.CallForPapers.Domain.dtoSummaryTableReportItem = renderField.CurrentSummaryItem
            If Not IsNothing(itemSummary) AndAlso Not String.IsNullOrEmpty(itemSummary.Name) Then
                CurrentReport.Tables.Add(itemSummary)
            End If

        End If
    End Sub



    Private Property CurrentReport As lm.Comol.Modules.CallForPapers.Domain.dtoSummaryTableReportTotal
        Get
            Return ViewStateOrDefault("CurrentSummaryReportData", New lm.Comol.Modules.CallForPapers.Domain.dtoSummaryTableReportTotal())
        End Get
        Set(value As lm.Comol.Modules.CallForPapers.Domain.dtoSummaryTableReportTotal)
            ViewState("CurrentSummaryReportData") = value
        End Set
    End Property









    Private Sub RPTrequiredFiles_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTrequiredFiles.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dto As dtoCallSubmissionFile = DirectCast(e.Item.DataItem, dtoCallSubmissionFile)

            Dim oControl As UC_InputRequiredFile = e.Item.FindControl("CTRLrequiredFile")
            oControl.InitializeControl(IdCall, 0, CallRepository, dto, True, False)

        ElseIf e.Item.ItemType = ListItemType.Header Then
            Dim oLiteral As Literal = e.Item.FindControl("LTrequiredFilesTitle")
            Resource.setLiteral(oLiteral)
            oLiteral = e.Item.FindControl("LTrequiredFilesDescription")
            Resource.setLiteral(oLiteral)
        End If
    End Sub
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
    Private Sub Submission_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Me.Master.ShowDocType = True

        If Me.IsAdvance Then
            LIrevisions.Visible = False
            BTNaccept.Visible = False
            BTNrefuse.Visible = False
        End If


    End Sub

#Region "Revisions"
    Private Sub LoadAvailableRevision(revisions As List(Of dtoRevision), idSelected As Long) Implements IViewViewSubmission.LoadAvailableRevision

        Dim displayDate As DateTime
        Me.DDLrevisions.Items.Clear()
        Dim displayNumber As String = ""
        For Each dto As dtoRevision In revisions
            If dto.Type = RevisionType.Original Then
                Me.DDLrevisions.Items.Add(New ListItem(String.Format(Resource.getValue("Revision.Original"), dto.CreatedOn.Value.ToShortDateString, dto.CreatedOn.Value.ToShortTimeString), dto.Id))
            Else
                Dim dtoRev As dtoRevisionRequest = DirectCast(dto, dtoRevisionRequest)

                If dtoRev.SubmittedOn.HasValue Then
                    displayDate = dtoRev.SubmittedOn.Value
                ElseIf dtoRev.CreatedOn.HasValue Then
                    displayDate = dtoRev.CreatedOn.Value
                End If
                If dtoRev.SubVersion = 0 Then
                    displayNumber = dtoRev.Number.ToString()
                Else
                    displayNumber = dtoRev.Number.ToString() & "." & dtoRev.SubVersion.ToString()
                End If

                Me.DDLrevisions.Items.Add(New ListItem(String.Format(Resource.getValue("Revision.Item"), dtoRev.Number, displayDate.ToShortDateString, displayDate.ToShortTimeString), dto.Id))
            End If
        Next
        If (revisions.Where(Function(r) r.Id = idSelected).Any()) Then
            Me.DDLrevisions.SelectedValue = idSelected
        End If
        Me.LIrevisions.Visible = (revisions.Count > 1) OrElse Me.ShowAdministrationTools
    End Sub
    Private Sub DDLrevisions_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLrevisions.SelectedIndexChanged
        Me.DVrevision.Visible = False
        Me.CurrentPresenter.ChangeRevision(DDLrevisions.SelectedValue)
    End Sub

    Private Property AllowRevisionRequest As Boolean Implements IViewViewSubmission.AllowRevisionRequest
        Get
            Return ViewStateOrDefault("AllowRevisionRequest", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowRevisionRequest") = value
            Me.BTNreview.Visible = value
        End Set
    End Property
    Private Property IdPendingRevision As Long Implements IViewViewSubmission.IdPendingRevision
        Get
            Return ViewStateOrDefault("IdPendingRevision", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdPendingRevision") = value
        End Set
    End Property

    Private Sub InitializeRevisionRequest(type As RevisionType) Implements IViewViewSubmission.InitializeRevisionRequest
        Me.DVrevision.Visible = False
        Me.Resource.setButtonByValue(BTNreview, type.ToString, True, , , True)
        Me.DVdeadline.Visible = (type = RevisionType.Manager)
        If (type = RevisionType.Manager) Then
            Me.RDPdeadline.MinDate = DateTime.Now.AddMinutes(10)
            Me.RDPdeadline.SelectedDate = DateTime.Now.AddDays(4)
        End If
        Me.TXBreason.Text = Resource.getValue("TXBreason." & type.ToString)
        TXBreason.Attributes.Add("maxlength", "6000")
    End Sub

    Private Sub DisplayRevisionInfo(revision As dtoRevision) Implements IViewViewSubmission.DisplayRevisionInfo
        Me.DVmessages.Visible = True
        Me.MLVrevisionInfo.Visible = True

        If Not IsNothing(revision.CreatedBy) Then
            Me.LBrevisionRequiredByInfo.Text = revision.CreatedBy.SurnameAndName
        Else
            Me.LBrevisionRequiredByInfo.Text = ""
        End If
        Me.LBrevisionMessageInfo.Text = ""
        LBrevisionDate.Text = "//"
        LBrevisionManagedByInfo.Text = "//"
        Me.LTrevisionStatus.Text = Resource.getValue("RevisionStatus." & revision.Status.ToString())
        Select Case revision.Type
            Case RevisionType.Original
                Me.MLVrevisionInfo.SetActiveView(VIWinfoEmpty)
            Case RevisionType.Manager, RevisionType.UserRequired
                Dim dto As dtoRevisionRequest = DirectCast(revision, dtoRevisionRequest)

                If Not IsNothing(dto.RequiredBy) Then
                    Me.LBrevisionRequiredByInfo.Text = dto.RequiredBy.SurnameAndName
                End If
                Me.LBrevisionMessageInfo.Text = dto.Reason
                If dto.Status = RevisionStatus.Submitted Then
                    If dto.ModifiedOn.HasValue Then
                        LBrevisionDate.Text = dto.ModifiedOn.Value.ToShortDateString & " " & dto.ModifiedOn.Value.ToShortTimeString
                    End If
                    If Not IsNothing(dto.ModifiedBy) Then
                        LBrevisionManagedByInfo.Text = dto.ModifiedBy.SurnameAndName
                    End If
                Else
                    If dto.ModifiedOn.HasValue Then
                        LBrevisionDate.Text = dto.ModifiedOn.Value.ToShortDateString & " " & dto.ModifiedOn.Value.ToShortTimeString
                    End If
                    If Not IsNothing(dto.ModifiedBy) Then
                        LBrevisionManagedByInfo.Text = dto.ModifiedBy.SurnameAndName
                    End If
                End If
                Me.MLVrevisionInfo.SetActiveView(VIWinfo)
            Case Else
                Me.MLVrevisionInfo.SetActiveView(VIWinfoEmpty)
        End Select
    End Sub


    Private Sub DisplaySelfPendingRequest(revision As dtoRevisionRequest, url As String) Implements IViewViewSubmission.DisplaySelfPendingRequest
        Me.DVmessages.Visible = True
        Me.MLVpendingMessage.Visible = True
        Me.MLVpendingMessage.SetActiveView(VIWpendingUser)
        Resource.setLabel_To_Value(LBrequiredRevision_t, "LBrequiredRevision_t.DisplaySelfPendingRequest." & revision.Status.ToString())

        BTNcancelUserRequest.Visible = revision.Status = RevisionStatus.Request
        BTNacceptUserRequest.Visible = False
        BTNrefuseUserRequest.Visible = False

        HTPreviewUserSubmission.Visible = (revision.Status = RevisionStatus.RequestAccepted OrElse revision.Status.Submitted)
        HTPviewUserRequest.Visible = False

        HTPreviewUserSubmission.NavigateUrl = BaseUrl & url
    End Sub

    Private Sub DisplayUserPendingRequest(revision As dtoRevisionRequest, url As String) Implements IViewViewSubmission.DisplayUserPendingRequest
        Me.DVmessages.Visible = True
        Me.MLVpendingMessage.Visible = True
        Me.MLVpendingMessage.SetActiveView(VIWpendingUser)
        Resource.setLabel_To_Value(LBrequiredRevision_t, "LBrequiredRevision_t.DisplayUserPendingRequest." & revision.Status.ToString())

        BTNcancelUserRequest.Visible = False
        'BTNacceptUserRequest.Visible = (revision.Status = RevisionStatus.Request)
        'BTNrefuseUserRequest.Visible = (revision.Status = RevisionStatus.Request)
        HTPreviewUserSubmission.Visible = False
        HTPviewUserRequest.Visible = (revision.Status = RevisionStatus.Request)
        HTPviewUserRequest.NavigateUrl = BaseUrl & url
    End Sub
    Private Sub DisplayPendingRevision(revision As dtoRevisionRequest, url As String) Implements IViewViewSubmission.DisplayPendingRevision
        Me.DVmessages.Visible = True
        Me.MLVpendingMessage.Visible = True
        Me.MLVpendingMessage.SetActiveView(VIWpendingManager)

        If Not IsNothing(revision.RequiredBy) Then
            Me.LBrevisionRequiredBy.Text = revision.RequiredBy.SurnameAndName
        Else
            Me.LBrevisionRequiredBy.Text = "//"
        End If
        LBrevisionMessage.Text = revision.Reason
        LBdeadlineDate.Text = "//"
        If revision.EndDate.HasValue Then
            LBdeadlineDate.Text = revision.EndDate.Value.ToShortDateString & " " & revision.EndDate.Value.ToShortTimeString
        End If
        BTNcancelManagerReview.Visible = False
        HTPreviewManagerSubmission.Visible = True
        HTPreviewManagerSubmission.NavigateUrl = BaseUrl & url
    End Sub
    Private Sub DisplayManagePendingRevision(revision As dtoRevisionRequest, url As String) Implements IViewViewSubmission.DisplayManagePendingRevision
        Me.DVmessages.Visible = True
        Me.MLVpendingMessage.Visible = True
        Me.MLVpendingMessage.SetActiveView(VIWpendingManager)

        If Not IsNothing(revision.RequiredBy) Then
            Me.LBrevisionRequiredBy.Text = revision.RequiredBy.SurnameAndName
        Else
            Me.LBrevisionRequiredBy.Text = "//"
        End If
        LBrevisionMessage.Text = revision.Reason
        LBdeadlineDate.Text = "//"
        If revision.EndDate.HasValue Then
            LBdeadlineDate.Text = revision.EndDate.Value.ToShortDateString & " " & revision.EndDate.Value.ToShortTimeString
        End If
        BTNcancelManagerReview.Visible = (revision.Status <> RevisionStatus.Approved)
        HTPreviewManagerSubmission.Visible = False
    End Sub
   

    Private Sub BTNreview_Click(sender As Object, e As System.EventArgs) Handles BTNreview.Click
        Me.DVrevision.Visible = True
        Me.DDLrevisions.SelectedIndex = 0
        If Me.ShowAdministrationTools Then
            Me.CurrentPresenter.StartManagerRevision(IdSubmission, Me.DDLrevisions.SelectedValue)
        End If

    End Sub
    Private Sub BTNaddRequest_Click(sender As Object, e As System.EventArgs) Handles BTNaddRequest.Click
    

        If Me.DVdeadline.Visible Then
            Me.CurrentPresenter.RequireRevision(IdSubmission, Me.TXBreason.Text, New dtoRevisionMessage() With {.SmtpConfig = PageUtility.CurrentSmtpConfig, .Subject = Resource.getValue("RequireRevision." & CallType.ToString() & ".Subject"), .Body = Resource.getValue("RequireRevision." & CallType.ToString() & ".Body")}, GetFieldsToReview(), RDPdeadline.SelectedDate, PageUtility.ApplicationUrlBase, PageUtility.GetRepositoryDiskPath, PageUtility.GetRepositoryThumbnailDiskPath)
        Else
            Me.CurrentPresenter.AskForRevision(IdSubmission, Me.TXBreason.Text, New dtoRevisionMessage() With {.SmtpConfig = PageUtility.CurrentSmtpConfig, .Subject = Resource.getValue("AskForRevision." & CallType.ToString() & ".Subject"), .Body = Resource.getValue("AskForRevision." & CallType.ToString() & ".Body")}, PageUtility.ApplicationUrlBase)
        End If
        Me.DVrevision.Visible = False
    End Sub
    Private Sub BTNundoRequest_Click(sender As Object, e As System.EventArgs) Handles BTNundoRequest.Click
        Me.DVrevision.Visible = False
        HideRevisions()
    End Sub
    Private Sub BTNcancelManagerReview_Click(sender As Object, e As System.EventArgs) Handles BTNcancelManagerReview.Click
        Me.CurrentPresenter.RemoveManagerRequest(IdSubmission, IdPendingRevision, New dtoRevisionMessage() With {.SmtpConfig = PageUtility.CurrentSmtpConfig, .Subject = Resource.getValue("RemoveManagerRequest." & CallType.ToString() & ".Subject"), .Body = Resource.getValue("RemoveManagerRequest." & CallType.ToString() & ".Body")}, PageUtility.ApplicationUrlBase)
    End Sub
    Private Sub BTNcancelUserRequest_Click(sender As Object, e As System.EventArgs) Handles BTNcancelUserRequest.Click
        Me.CurrentPresenter.RemoveSelfRequest(IdSubmission, IdPendingRevision, New dtoRevisionMessage() With {.SmtpConfig = PageUtility.CurrentSmtpConfig, .Subject = Resource.getValue("RemoveSelfRequest." & CallType.ToString() & ".Subject"), .Body = Resource.getValue("RemoveSelfRequest." & CallType.ToString() & ".Body")}, PageUtility.ApplicationUrlBase)
    End Sub

    'Private Sub BTNacceptUserRequest_Click(sender As Object, e As System.EventArgs) Handles BTNacceptUserRequest.Click
    '    Me.CurrentPresenter.AcceptUserRequest(IdSubmission, IdPendingRevision, New dtoRevisionMessage() With {.SmtpConfig = PageUtility.CurrentSmtpConfig, .Subject = Resource.getValue("RefuseUserRequest." & CallType.ToString() & ".Subject"), .Body = Resource.getValue("RefuseUserRequest." & CallType.ToString() & ".Body")})
    'End Sub

    'Private Sub BTNrefuseUserRequest_Click(sender As Object, e As System.EventArgs) Handles BTNrefuseUserRequest.Click
    '    Me.CurrentPresenter.RefuseUserRequest(IdSubmission, IdPendingRevision, New dtoRevisionMessage() With {.SmtpConfig = PageUtility.CurrentSmtpConfig, .Subject = Resource.getValue("RefuseUserRequest." & CallType.ToString() & ".Subject"), .Body = Resource.getValue("RefuseUserRequest." & CallType.ToString() & ".Body")})
    'End Sub

    Private Function GetFieldsToReview() As List(Of dtoRevisionItem) Implements IViewViewSubmission.GetFieldsToReview
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
#End Region

    Private Sub BTNaccept_Click(sender As Object, e As System.EventArgs) Handles BTNaccept.Click
        Me.CurrentPresenter.SaveStatus(SubmissionStatus.accepted, PageUtility.ApplicationUrlBase, PageUtility.CurrentSmtpConfig, Resource.getValue("NotificationMessage." & CallType.ToString & ".SubmissionStatus." & SubmissionStatus.accepted.ToString & ".Body"), Resource.getValue("NotificationMessage." & CallType.ToString & ".SubmissionStatus." & SubmissionStatus.accepted.ToString & ".Subject"))
    End Sub

    Private Sub BTNrefuse_Click(sender As Object, e As System.EventArgs) Handles BTNrefuse.Click
        Me.CurrentPresenter.SaveStatus(SubmissionStatus.rejected, PageUtility.ApplicationUrlBase, PageUtility.CurrentSmtpConfig, Resource.getValue("NotificationMessage." & CallType.ToString & ".SubmissionStatus." & SubmissionStatus.rejected.ToString & ".Body"), Resource.getValue("NotificationMessage." & CallType.ToString & ".SubmissionStatus." & SubmissionStatus.rejected.ToString & ".Subject"))
    End Sub


    Public Function AddInternalFile(revision As Revision, moduleCode As String, idModule As Integer, moduleAction As Integer, objectType As Integer) As ModuleActionLink Implements IViewViewSubmission.AddInternalFile

        Return CTRLfileUploader.UploadAndLinkInternalFile(FileRepositoryType.InternalLong, revision, moduleCode, moduleAction, objectType)
    End Function

    Public Sub HideSignSubmission() Implements IViewViewSubmission.HideSignSubmission
        Me.MLVsign.Visible = False
        PNLSigns.Visible = False
    End Sub

    Public Sub InitializeDownloadSign(link As ModuleLink) Implements IViewViewSubmission.InitializeDownloadSign
        PNLSigns.Visible = True

        Me.MLVsign.Visible = True

        Me.MLVsign.SetActiveView(Vsubmitted)

        Dim initializer As New lm.Comol.Core.ModuleLinks.dtoModuleDisplayActionInitializer

        ' DIMENSIONI IMMAGINI
        initializer.IconSize = Helpers.IconSize.Small
        CTRLdisplayFile.EnableAnchor = True

        initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction _
            Or lm.Comol.Core.ModuleLinks.DisplayActionMode.actions

        initializer.Link = link
        CTRLdisplayFile.InsideOtherModule = True
        Dim actions As List(Of dtoModuleActionControl)
        actions = CTRLdisplayFile.InitializeRemoteControl(initializer, initializer.Display)

    End Sub

    Public Sub InitSignSubmission(idCommunity As Integer) Implements IViewViewSubmission.InitSignSubmission
        PNLSigns.Visible = True
        Me.MLVsign.Visible = True
        Me.MLVsign.SetActiveView(VtoSubmit)

        Me.CTRLfileUploader.Enabled = True
        Me.CTRLfileUploader.Visible = True

        'Presenter: int idCommunity = SetCallCurrentCommunity(call);
        CTRLfileUploader.PDFOnly(True)
        CTRLfileUploader.ButtonToLock = LKBupload.ClientID
        Me.CTRLfileUploader.InitializeControl(
            idCommunity, False)
    End Sub

    Public Sub ShowSignNotSubmitted() Implements IViewViewSubmission.ShowSignNotSubmitted
        PNLSigns.Visible = True
        Me.MLVsign.Visible = True
        Me.MLVsign.SetActiveView(VnotSigned)

    End Sub

    Private Sub LKBupload_Click(sender As Object, e As EventArgs) Handles LKBupload.Click

        Dim dtClick As DateTime = DateTime.Now


        Dim sTranslations As New Dictionary(Of lm.Comol.Modules.CallForPapers.Domain.SubmissionTranslations, String)
        For Each name As String In [Enum].GetNames(GetType(lm.Comol.Modules.CallForPapers.Domain.SubmissionTranslations))
            sTranslations.Add([Enum].Parse(GetType(lm.Comol.Modules.CallForPapers.Domain.SubmissionTranslations), name), Me.Resource.getValue("SubmissionTranslations." & name))
        Next

        CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.SubmissionCompileUploadSign,
            Me.IdSubmission,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.UserSubmission,
            "Start")

        Me.CurrentPresenter.UploadSign(
            False,
            GetValues(),
            PageUtility.CurrentSmtpConfig,
            PageUtility.ApplicationUrlBase,
            sTranslations,
            dtClick)
    End Sub

    Private Function GetValues() As List(Of dtoSubmissionValueField)
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
            Dim oControl As UC_RenderField = row.FindControl("CTRLrenderField") 'CTRLinputField
            field = oControl.GetField
            fields.Add(field)
        Next
        Return fields
    End Function

    Private Sub LKBprintForSign_Click(sender As Object, e As EventArgs) Handles LKBprintForSign.Click
        Me.CTRLreport.Download()
    End Sub

    Public Sub DisplayOutOfTime(Info As String, ShowMessage As Boolean, showAsError As Boolean) Implements IViewViewBaseSubmission.DisplayOutOfTime

        CTRLfileUploader.Visible = False
        LKBupload.Visible = False

        If (ShowMessage) Then
            CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.SubmissionCompileUploadSign,
            Me.IdSubmission,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.UserSubmission,
            "Expired")

            If Info = "Expired" Then
                If (showAsError) Then
                    CTRLerrorMessages.InitializeControl("Superato il tempo massimo per la sottomissione.", Helpers.MessageType.error)
                Else
                    CTRLerrorMessages.InitializeControl("Superato il tempo massimo per la sottomissione.", Helpers.MessageType.info)
                End If
            End If

        End If
    End Sub

    Public Sub ShowHideSendIntegration(Show As Boolean) Implements IViewViewSubmission.ShowHideSendIntegration
        PNLsendIntegration.Visible = Show
        'Me.LKBsendIntegration.Visible = Show
    End Sub

    Private Sub IViewViewBaseSubmission_SendUserAction(idCommunity As Integer, idModule As Integer, idSubmission As Long, action As ModuleCallForPaper.ActionType) Implements IViewViewBaseSubmission.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleCallForPaper.ObjectType.CallForPaper, IdCall.ToString), InteractionType.UserWithLearningObject)
    End Sub

    Private Sub LKBsendIntegration_Click(sender As Object, e As EventArgs) Handles LKBsendIntegration.Click

        Dim Subject As String = Resource.getValue("Integration.Mail.Subject")
        Dim Body As String = Resource.getValue("Integration.Mail.Body")

        Dim dtEnd As DateTime = DateTime.Now.AddDays(7)

        If Not IsNothing(RDPintegrationEnd.SelectedDate) Then
            dtEnd = RDPintegrationEnd.SelectedDate
        End If

        Dim DateFormat As String = "F"
        Dim IT_Cult As New CultureInfo("it-IT")


        Dim EndDate As String = dtEnd.ToString(DateFormat, IT_Cult)

        Me.LTintegrationEndOn.Text = EndDate


        Me.CurrentPresenter.SendIntegration(PageUtility.CurrentSmtpConfig, Subject, Body, FullBaseUrl, EndDate)

    End Sub

    Public Sub ShowSendInfo(sended As Boolean) Implements IViewViewSubmission.ShowSendInfo
        If sended Then
            CTRLerrorMessages.InitializeControl(String.Format("Notifica inviata correttamente. Scadenza richiesta: {0}", LTintegrationEndOn.Text), Helpers.MessageType.success)
        Else
            CTRLerrorMessages.InitializeControl("Errore invio notifica.", Helpers.MessageType.alert)
        End If

    End Sub

    Public Sub SendSuccessTrap() Implements IViewViewSubmission.SendSuccessTrap

        CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.SubmissionCompileUploadSign,
            Me.IdSubmission,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.UserSubmission,
            "Success")
    End Sub



#Region "Advanced"


    Public Property IsAdvance As Boolean Implements IViewViewSubmission.IsAdvance
        Get
            Return Me.ViewStateOrDefault("CallIsAdvance", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("CallIsAdvance") = value

        End Set
    End Property

    Public Property CommissionId As Long Implements IViewViewSubmission.CommissionId
        Get
            Dim cId As Int64 = 0

            Try
                cId = Me.ViewState("CommissionId")
            Catch ex As Exception

            End Try

            If cId = 0 Then
                Try
                    cId = Request.QueryString("cmmId")
                Catch ex As Exception

                End Try
            End If

            If cId > 0 Then
                Me.ViewState("CommissionId") = cId
            End If


            Return cId

        End Get
        Set(value As Long)
            Me.ViewState("CommissionId") = value
        End Set
    End Property

#End Region

    'Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As ModuleCallForPaper.ActionType) Implements IViewViewSubmission.SendUserAction
    '    PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleCallForPaper.ObjectType.CallForPaper, idCall.ToString), InteractionType.UserWithLearningObject)
    'End Sub




End Class