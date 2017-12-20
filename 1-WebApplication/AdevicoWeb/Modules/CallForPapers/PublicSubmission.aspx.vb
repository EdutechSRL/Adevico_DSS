Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation
Imports lm.Comol.Modules.CallForPapers.Domain
Imports lm.ActionDataContract
Imports System.Linq
Imports System.Collections.Generic
Imports lm.Comol.Core.DomainModel.Helpers.Export

Public Class PublicSubmission
    Inherits PageBase
    Implements IViewViewPublicSubmission

#Region "Context"
    Private _Presenter As ViewPublicSubmissionPresenter
    Private ReadOnly Property CurrentPresenter() As ViewPublicSubmissionPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ViewPublicSubmissionPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property Portalname As String Implements IViewViewPublicSubmission.Portalname
        Get
            Return Resource.getValue("Portalname")
        End Get
    End Property
    Private ReadOnly Property PreloadIdCall As Long Implements IViewViewPublicSubmission.PreloadIdCall
        Get
            If IsNumeric(Me.Request.QueryString("idCall")) Then
                Return CLng(Me.Request.QueryString("idCall"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewViewPublicSubmission.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadedUniqueID As Guid Implements IViewViewPublicSubmission.PreloadedUniqueID
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
    Private ReadOnly Property PreloadedIdSubmission As Long Implements IViewViewPublicSubmission.PreloadedIdSubmission
        Get
            If IsNumeric(Me.Request.QueryString("idSubmission")) Then
                Return CLng(Me.Request.QueryString("idSubmission"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdOtherCommunity As Integer Implements IViewViewPublicSubmission.PreloadIdOtherCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idOtherCommunity")) Then
                Return CInt(Me.Request.QueryString("idOtherCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property FromPublicList As Boolean Implements IViewViewPublicSubmission.FromPublicList
        Get
            If String.IsNullOrEmpty(Me.Request.QueryString("FromPublicList")) Then
                Return False
            Else
                Return True
            End If
        End Get
    End Property

    Private Property IdRevision As Long Implements IViewViewPublicSubmission.IdRevision
        Get
            Return ViewStateOrDefault("IdRevision", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdRevision") = value
        End Set
    End Property
    Private ReadOnly Property PreloadedIdRevision As Long Implements IViewViewPublicSubmission.PreloadedIdRevision
        Get
            If IsNumeric(Me.Request.QueryString("idRevision")) Then
                Return CLng(Me.Request.QueryString("idRevision"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadView As CallStatusForSubmitters Implements IViewViewPublicSubmission.PreloadView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of CallStatusForSubmitters).GetByString(Request.QueryString("View"), CallStatusForSubmitters.SubmissionOpened)
        End Get
    End Property

    Private ReadOnly Property AnonymousOwnerName As String Implements IViewViewPublicSubmission.AnonymousOwnerName
        Get
            Return Me.Resource.getValue("AnonymousOwnerName")
        End Get
    End Property
    Private Property CallType As CallForPaperType Implements IViewViewPublicSubmission.CallType
        Get
            Return ViewStateOrDefault("CallType", CallForPaperType.CallForBids)
        End Get
        Set(value As CallForPaperType)
            Me.ViewState("CallType") = value
        End Set
    End Property
    Private Property IdCall As Long Implements IViewViewPublicSubmission.IdCall
        Get
            Return ViewStateOrDefault("IdCall", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdCall") = value
        End Set
    End Property
    Private Property IdCallCommunity As Integer Implements IViewViewPublicSubmission.IdCallCommunity
        Get
            Return ViewStateOrDefault("IdCallCommunity", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCallCommunity") = value
        End Set
    End Property
    Private Property IdCallModule As Integer Implements IViewViewPublicSubmission.IdCallModule
        Get
            Return ViewStateOrDefault("IdCallModule", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCallModule") = value
        End Set
    End Property

    Private Property IdSubmission As Long Implements IViewViewPublicSubmission.IdSubmission
        Get
            Return ViewStateOrDefault("IdSubmission", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdSubmission") = value
        End Set
    End Property
    Private Property IdSubmitterType As Long Implements IViewViewPublicSubmission.IdSubmitterType
        Get
            Return ViewStateOrDefault("IdSubmitterType", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdSubmitterType") = value
        End Set
    End Property
    Private Property isAnonymousSubmission As Boolean Implements IViewViewPublicSubmission.isAnonymousSubmission
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
            .setLabel(LBtimeValidity_t)

            .setLiteral(LTowner_t)
            .setLiteral(LTsubmitterType_t)
            .setLiteral(LTsubmissionStatus_t)
            .setLiteral(LTsubmittedOn_t)
            .setLiteral(LTsubmittedBy_t)

            .setLabel(LBdeadline_t)
            .setLabel(LBrequestReason_t)
            LTmaxCharsrequest.Text = .getValue("MaxCharsInfo")
            .setLabel(LBreasonHelp)
            .setButton(BTNaddRequest, True, , , True)
            .setButton(BTNundoRequest, True, , , True)
        End With
    End Sub
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub InitializeView(skin As lm.Comol.Core.DomainModel.Helpers.ExternalPageContext) Implements IViewViewPublicSubmission.InitializeView
        Me.Master.InitializeMaster(skin)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As ModuleCallForPaper.ActionType) Implements IViewViewPublicSubmission.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleCallForPaper.ObjectType.CallForPaper, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As ModuleRequestForMembership.ActionType) Implements IViewViewPublicSubmission.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleRequestForMembership.ObjectType.RequestForMembership, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewViewPublicSubmission.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, ModuleCallForPaper.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewViewPublicSubmission.DisplaySessionTimeout
        Dim url As String = ""
        url = RootObject.ViewSubmission(CallForPaperType.CallForBids, PreloadIdCall, PreloadedIdSubmission, PreloadedUniqueID, True, FromPublicList, PreloadView, PreloadIdOtherCommunity, 0)

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
    Private Sub DisplayCallUnavailableForPublic() Implements IViewViewPublicSubmission.DisplayCallUnavailableForPublic
        MLVpreview.SetActiveView(VIWempty)
        Me.LBemptyMessage.Text = Me.Resource.getValue("DisplayCallUnavailableForPublic.View." & CallForPaperType.CallForBids.ToString())
    End Sub
    Private Sub DisplaySubmissionUnavailable() Implements IViewViewPublicSubmission.DisplaySubmissionUnavailable
        Me.MLVpreview.SetActiveView(VIWempty)
        Me.LBemptyMessage.Text = Resource.getValue("DisplaySubmissionUnavailable.View." & CallType.ToString)
    End Sub
    Private Sub DisplayUnknownSubmission(idCommunity As Integer, idModule As Integer, idSubmission As Long, type As CallForPaperType) Implements IViewViewPublicSubmission.DisplayUnknownSubmission
        Me.MLVpreview.SetActiveView(VIWempty)
        Me.LBemptyMessage.Text = Resource.getValue("DisplayUnknownSubmission." & type.ToString)
        PageUtility.AddActionToModule(idCommunity, idModule, IIf(type = CallForPaperType.CallForBids, ModuleCallForPaper.ActionType.GenericError, ModuleRequestForMembership.ActionType.GenericError), PageUtility.CreateObjectsList(idModule, IIf(type = CallForPaperType.CallForBids, ModuleCallForPaper.ObjectType.UserSubmission, ModuleRequestForMembership.ObjectType.UserSubmission), idSubmission.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub DisplayUnknownCall(idCommunity As Integer, idModule As Integer, idCall As Long, type As CallForPaperType) Implements IViewViewPublicSubmission.DisplayUnknownCall
        Me.MLVpreview.SetActiveView(VIWempty)
        Me.LBemptyMessage.Text = Resource.getValue("DisplayUnknownCall." & type.ToString)
        If type = CallForPaperType.CallForBids Then
            SendUserAction(idCommunity, idModule, idCall, ModuleCallForPaper.ActionType.GenericError)
        Else
            SendUserAction(idCommunity, idModule, idCall, ModuleRequestForMembership.ActionType.GenericError)
        End If
    End Sub

    Private Sub SetContainerName(name As String, itemName As String) Implements IViewViewPublicSubmission.SetContainerName
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

    Private Sub LoadAttachments(items As List(Of dtoAttachmentFile)) Implements IViewViewPublicSubmission.LoadAttachments
        Me.RPTattachments.Visible = (items.Count > 0)
        Me.RPTattachments.DataSource = items
        Me.RPTattachments.DataBind()
    End Sub
    Private Sub LoadCallInfo(baseCall As dtoCall) Implements IViewViewPublicSubmission.LoadCallInfo
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
    Public Sub LoadCallInfo(baseCall As dtoRequest) Implements IViewViewPublicSubmission.LoadCallInfo
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
    Private Sub LoadSubmissionInfo(submitterName As String, ownerName As String, status As SubmissionStatus) Implements IViewViewPublicSubmission.LoadSubmissionInfo
        Me.MLVpreview.SetActiveView(VIWcall)
        DVtopMenu.Visible = True
        Me.LBowner.Text = ownerName
        Me.LBsubmitterType.Text = submitterName
        Me.LBsubmissionStatus.Text = Resource.getValue("SubmissionStatus." & status.ToString)
        Me.LIsubmissionInfo.Visible = False
    End Sub
    Private Sub LoadSubmissionInfo(submitterName As String, ownerName As String, status As SubmissionStatus, submittedOn As DateTime) Implements IViewViewPublicSubmission.LoadSubmissionInfo
        Me.MLVpreview.SetActiveView(VIWcall)
        LoadSubmissionInfo(submitterName, ownerName, status)
        Me.LIsubmissionInfo.Visible = True
        SPNsubmittedBy.Visible = False
        Me.LBsubmittedOnData.Text = submittedOn.ToString("dd/MM/yy")
        Me.LBsubmittedOnTime.Text = submittedOn.ToString("HH:mm")
    End Sub
    Private Sub LoadSubmissionInfo(submitterName As String, ownerName As String, status As SubmissionStatus, submittedOn As DateTime, submittedBy As String) Implements IViewViewPublicSubmission.LoadSubmissionInfo
        Me.MLVpreview.SetActiveView(VIWcall)
        LoadSubmissionInfo(submitterName, ownerName, status)
        Me.LBsubmittedBy.Text = submittedBy
        SPNsubmittedBy.Visible = True
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
                                   Implements IViewViewPublicSubmission.InitializeExportControl

        Me.CTRLreport.InitializeControl(isOwner, idUser, LBowner.Text, idCall, idSubmission, idRevision, idModule, idCallCommunity, callType)

        CTRLprintDraf.Visible = isDraft

        If (isDraft) Then
            Me.CTRLreport.ShowForDraft()
            CTRLprintDraf.InitializeUC(idCall, submitterType, idRevision, idSubmission)
        End If
    End Sub

    Private Sub LoadSections(sections As List(Of dtoCallSection(Of dtoSubmissionValueField))) Implements IViewViewPublicSubmission.LoadSections
        Me.RPTsections.DataSource = sections
        Me.RPTsections.DataBind()
    End Sub
    Private Sub LoadRequiredFiles(files As List(Of dtoCallSubmissionFile)) Implements IViewViewPublicSubmission.LoadRequiredFiles
        Me.RPTrequiredFiles.Visible = (files.Count > 0)
        Me.RPTrequiredFiles.DataSource = files
        Me.RPTrequiredFiles.DataBind()
    End Sub
    Private Sub SetActionUrl(action As CallStandardAction, url As String) Implements IViewViewPublicSubmission.SetActionUrl
        Select Case action
            Case CallStandardAction.List
                Me.HYPlist.NavigateUrl = BaseUrl & url
                Me.HYPlist.Visible = True
        End Select
    End Sub
    Private Sub GoToUrl(url As String) Implements IViewViewPublicSubmission.GoToUrl
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
            oControl.Visible = String.IsNullOrEmpty(dto.Description)


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
            renderField.InitializeControl(field, False, True, field.FieldError, field.AllowSelection)

        End If
    End Sub
    Private Sub RPTrequiredFiles_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTrequiredFiles.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dto As dtoCallSubmissionFile = DirectCast(e.Item.DataItem, dtoCallSubmissionFile)

            Dim oControl As UC_InputRequiredFile = e.Item.FindControl("CTRLrequiredFile")
            oControl.InitializeControl(IdCall, 0, Callrepository, dto, False, False)

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
    End Sub

    Public Sub DisplayOutOfTime(Info As String, ShowMessage As Boolean, showAsError As Boolean) Implements IViewViewBaseSubmission.DisplayOutOfTime
        'If Info = "Epired" Then
        '    CTRLerrorMessages.InitializeControl(Resource.getValue("ServiceViewSubmissionNopermission"), Helpers.MessageType.error)
        'End If


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
End Class