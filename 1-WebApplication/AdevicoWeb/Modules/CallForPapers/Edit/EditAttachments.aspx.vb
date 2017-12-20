Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation
Imports lm.Comol.Modules.CallForPapers.Domain
Imports lm.ActionDataContract
Imports System.Linq
Imports System.Collections.Generic

Public Class EditAttachments
    Inherits PageBase
    Implements IViewEditAttachments


#Region "Context"
    Private _Presenter As EditAttachmentsPresenter
    Private ReadOnly Property CurrentPresenter() As EditAttachmentsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New EditAttachmentsPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property Portalname As String Implements IViewBaseEditCall.Portalname
        Get
            Return Resource.getValue("Portalname")
        End Get
    End Property
    Private ReadOnly Property PreloadIdCall As Long Implements IViewBaseEditCall.PreloadIdCall
        Get
            If IsNumeric(Me.Request.QueryString("idCall")) Then
                Return CLng(Me.Request.QueryString("idCall"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewBaseEditCall.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadType As CallForPaperType Implements IViewBaseEditCall.PreloadType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of CallForPaperType).GetByString(Request.QueryString("type"), CallForPaperType.CallForBids)
        End Get
    End Property
    Private ReadOnly Property PreloadView As CallStatusForSubmitters Implements IViewBaseEditCall.PreloadView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of CallStatusForSubmitters).GetByString(Request.QueryString("View"), CallStatusForSubmitters.SubmissionOpened)
        End Get
    End Property
    Private Property AllowSave As Boolean Implements IViewBaseEditCall.AllowSave
        Get
            Return ViewStateOrDefault("AllowSave", False)
        End Get
        Set(ByVal value As Boolean)
            ViewState("AllowSave") = value
            Me.BTNsaveAttachmentsTop.Enabled = value
            Me.BTNsaveAttachmentsBottom.Enabled = value
            If Not value Then
                CTRLcommands.Visible = value
            End If
        End Set
    End Property
    Private Property CallType As CallForPaperType Implements IViewBaseEditCall.CallType
        Get
            Return ViewStateOrDefault("CallType", CallForPaperType.CallForBids)
        End Get
        Set(value As CallForPaperType)
            Resource.setHyperLink(HYPpreviewCallBottom, value.ToString(), True, True)
            Resource.setHyperLink(HYPpreviewCallTop, value.ToString(), True, True)
            Resource.setHyperLink(HYPbackTop, value.ToString, True, True)
            Resource.setHyperLink(HYPbackBottom, value.ToString, True, True)
            Me.ViewState("CallType") = value
        End Set
    End Property
    Private Property IdCall As Long Implements IViewBaseEditCall.IdCall
        Get
            Return ViewStateOrDefault("IdCall", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdCall") = value
        End Set
    End Property
    Private Property IdCommunity As Integer Implements IViewBaseEditCall.IdCommunity
        Get
            Return ViewStateOrDefault("IdCommunity", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCommunity") = value
        End Set
    End Property
    Private Property IdCallModule As Integer Implements IViewBaseEditCall.IdCallModule
        Get
            Return ViewStateOrDefault("IdCallModule", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCallModule") = value
        End Set
    End Property
    Private Property Availablesubmitters As List(Of dtoSubmitterType) Implements IViewEditAttachments.Availablesubmitters
        Get
            Return ViewStateOrDefault("Availablesubmitters", New List(Of dtoSubmitterType))
        End Get
        Set(value As List(Of dtoSubmitterType))
            Me.ViewState("Availablesubmitters") = value
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

#Region "Page"
    Protected ReadOnly Property GetUploadTitle() As String
        Get
            Return Resource.getValue("LBuploadAttachments_t.text")
        End Get
    End Property

    Public WriteOnly Property AllowUpdateTags As Boolean Implements IViewBaseEditCall.AllowUpdateTags
        Set(value As Boolean)

        End Set
    End Property
#End Region

    Private Sub EditCallAvailability_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Me.Master.ShowDocType = True
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        If Not Page.IsPostBack() Then
            CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.CallEditView,
            Me.IdCall,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.CallForPeaper,
            "")
        End If

        Master.ShowNoPermission = False
        Me.CurrentPresenter.InitView()

        If Not Page.IsPostBack() Then
            CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.CallEditView,
            Me.IdCall,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.CallForPeaper,
            "")
        End If
    End Sub

    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True

        CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.CallEditView,
            Me.IdCall,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.CallForPeaper,
            "NoPermission")

    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_EditCall", "Modules", "CallForPapers")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            CTRLemptyMessage.InitializeControl(Resource.getValue("LBnoCalls." & CallType.ToString), Helpers.MessageType.error)
            .setHyperLink(HYPpreviewCallBottom, CallType.ToString(), True, True)
            .setHyperLink(HYPpreviewCallTop, CallType.ToString(), True, True)
            .setHyperLink(HYPbackTop, CallType.ToString, True, True)
            .setHyperLink(HYPbackBottom, CallType.ToString, True, True)
            .setButton(BTNsaveAttachmentsBottom, True, , , True)
            .setButton(BTNsaveAttachmentsTop, True, , , True)
            .setLabel(LBattachments_t)
        End With
    End Sub

    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As ModuleCallForPaper.ActionType) Implements IViewEditAttachments.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleCallForPaper.ObjectType.CallForPaper, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As ModuleRequestForMembership.ActionType) Implements IViewEditAttachments.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleRequestForMembership.ObjectType.RequestForMembership, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewBase.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, IIf(CallType = CallForPaperType.CallForBids, ModuleCallForPaper.ActionType.NoPermission, ModuleRequestForMembership.ActionType.NoPermission), , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub

    Private Sub DisplaySessionTimeout() Implements IViewBase.DisplaySessionTimeout
        Dim idCommunity As Integer = PreloadIdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

        dto.DestinationUrl = RootObject.EditCallAttachments(CallType, PreloadIdCall, idCommunity, PreloadView)

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If

        CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.CallEditView,
            Me.IdCall,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.CallForPeaper,
            "SessionTimeOut")

        webPost.Redirect(dto)
    End Sub

    Private Sub SetActionUrl(action As CallStandardAction, url As String) Implements IViewBaseEditCall.SetActionUrl
        If action = CallStandardAction.PreviewCall Then
            Me.HYPpreviewCallBottom.Visible = True
            Me.HYPpreviewCallBottom.NavigateUrl = BaseUrl & url
            Me.HYPpreviewCallTop.Visible = True
            Me.HYPpreviewCallTop.NavigateUrl = BaseUrl & url
        Else
            Me.HYPbackBottom.Visible = True
            Me.HYPbackBottom.NavigateUrl = BaseUrl & url
            Me.HYPbackTop.Visible = True
            Me.HYPbackTop.NavigateUrl = BaseUrl & url
        End If
   
    End Sub

    Private Sub SetContainerName(action As CallStandardAction, name As String, itemName As String) Implements IViewBaseEditCall.SetContainerName
        Dim title As String = Me.Resource.getValue("serviceTitle." & action.Edit.ToString & "." & CallType.ToString())
        Master.ServiceTitle = String.Format(title, IIf(Len(itemName) > 70, Left(itemName, 70) & "...", itemName))
        If String.IsNullOrEmpty(name) Then
            Master.ServiceTitleToolTip = String.Format(title, itemName)
        Else
            Dim tooltip As String = Me.Resource.getValue("serviceTitle.Community." & action.Edit.ToString() & "." & CallType.ToString())
            If Not String.IsNullOrEmpty(tooltip) Then
                Master.ServiceTitleToolTip = String.Format(tooltip, name)
            End If
        End If
    End Sub

    Private Sub LoadWizardSteps(idCall As Long, type As lm.Comol.Modules.CallForPapers.Domain.CallForPaperType, idCommunity As Integer, steps As List(Of lm.Comol.Core.Wizard.NavigableWizardItem(Of WizardCallStep))) Implements IViewBaseEditCall.LoadWizardSteps
        Me.CTRLsteps.InitalizeControl(idCall, type, idCommunity, PreloadView, steps)
    End Sub

    Private Sub LoadUnknowCall(idCommunity As Integer, idModule As Integer, idCall As Long, type As CallForPaperType) Implements IViewBaseEditCall.LoadUnknowCall
        MLVsettings.SetActiveView(VIWempty)
        CTRLemptyMessage.InitializeControl(Resource.getValue("LBnoCalls." & type.ToString), Helpers.MessageType.error)
    End Sub
    Private Sub HideErrorMessages() Implements IViewEditAttachments.HideErrorMessages
        CTRLmessages.Visible = False
    End Sub

    Private Sub DisplayNoAttachments() Implements IViewEditAttachments.DisplayNoAttachments
        Me.BTNsaveAttachmentsTop.Visible = False
        Me.BTNsaveAttachmentsBottom.Visible = False
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayNoAttachments"), Helpers.MessageType.info)
    End Sub
    Private Sub DisplaySettingsSaved() Implements IViewEditAttachments.DisplaySettingsSaved
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayAttachmentsSettingsSaved"), Helpers.MessageType.success)
    End Sub
    Private Sub DisplayDeletingError() Implements IViewEditAttachments.DisplayDeletingError
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayDeletingError"), Helpers.MessageType.error)
    End Sub
    Private Sub DisplaySavingError() Implements IViewEditAttachments.DisplaySavingError
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplaySavingError"), Helpers.MessageType.error)
    End Sub
    Private Sub LoadAttachments(items As List(Of dtoAttachmentFilePermission)) Implements IViewEditAttachments.LoadAttachments
        Me.RPTattachments.DataSource = items
        Me.RPTattachments.DataBind()
        Me.MLVsettings.SetActiveView(VIWsettings)
        Me.RPTattachments.Visible = (items.Count > 0)

        Me.BTNsaveAttachmentsTop.Visible = AllowSave
        Me.BTNsaveAttachmentsBottom.Visible = AllowSave
        DVbuttonBottom.Visible = (items.Count > 1)
        ULviewAttachments.Visible = (items.Count > 0)
    End Sub

    Private Sub LoadSubmitterTypes(submitters As List(Of dtoSubmitterType)) Implements IViewEditAttachments.LoadSubmitterTypes
        Me.Availablesubmitters = submitters
    End Sub

    Private Function GetAttachments() As List(Of dtoAttachmentFile) Implements IViewEditAttachments.GetAttachments
        Dim attachments As New List(Of dtoAttachmentFile)

        For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTattachments.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim attachment As New dtoAttachmentFile()
            Dim oLiteral As Literal = row.FindControl("LTidAttachment")
            Dim oTextBox As TextBox = row.FindControl("TXBdescription")

            attachment.Id = CLng(oLiteral.Text)
            attachment.Description = oTextBox.Text

            Dim oChecbox As HtmlInputCheckBox = row.FindControl("CBXforAll")
            attachment.ForAll = oChecbox.Checked

            If Not (attachment.ForAll) Then
                Dim oSelect As HtmlSelect = row.FindControl("SLBsubmitters")
                For Each item As ListItem In oSelect.Items
                    If item.Selected Then
                        attachment.SubmitterAssignments.Add(CLng(item.Value))
                    End If
                Next
            End If
            Dim hidden As HtmlInputHidden = row.FindControl("HDNdisplayOrder")
            If Not String.IsNullOrEmpty(hidden.Value) AndAlso IsNumeric(hidden.Value) Then
                attachment.DisplayOrder = CInt(hidden.Value)
            End If
            attachments.Add(attachment)
        Next
        Return attachments.OrderBy(Function(s) s.DisplayOrder).ToList
    End Function
    Private Sub InitializeUploader(idCall As Long, type As CallForPaperType, identifier As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier, actions As List(Of lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions), Optional dAction As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions = lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.none) Implements IViewEditAttachments.InitializeAttachmentsControl
        CTRLcommands.Visible = actions.Any()
        CTRLcommands.InitializeControlForJQuery(actions, dAction)
        If Not IsNothing(actions) Then
            CTRLattachmentsHeader.InitializeControlForJQuery(actions, actions.ToDictionary(Of lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions, String)(Function(a) a, Function(a) Resource.getValue("dialogTitle.RepositoryAttachmentUploadActions." & a.ToString)))
            If actions.Contains(lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem) Then
                CTRLinternalUpload.Visible = True
                CTRLinternalUpload.InitializeControl(idCall, type, Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem, identifier, Resource.getValue("dialogDescription.CallForPaperType." & type.ToString & ".CallAttachments." & lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem.ToString))
            Else
                CTRLinternalUpload.Visible = False
            End If
        End If
    End Sub
    'Private Function UploadFiles(baseCall As lm.Comol.Modules.CallForPapers.Domain.BaseForPaper, moduleCode As String, moduleAction As Integer, objectType As Integer) As MultipleUploadResult(Of dtoModuleUploadedFile) Implements IViewEditAttachments.UploadFiles
    '    'Return Me.CTRLmoduleUpload.AddModuleInternalFiles(FileRepositoryType.InternalLong, baseCall, moduleCode, moduleAction, objectType)
    'End Function
#End Region

#Region "Internal"
    Private Sub RPTattachments_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTattachments.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dto As dtoAttachmentFilePermission = DirectCast(e.Item.DataItem, dtoAttachmentFilePermission)

            Dim oLiteral As Literal = e.Item.FindControl("LTidAttachment")
            Dim renderItem As UC_RepositoryRenderAction = e.Item.FindControl("CTRLdisplayItem")

            Dim initializer As New lm.Comol.Core.ModuleLinks.dtoObjectRenderInitializer
            initializer.RefreshContainerPage = False
            initializer.SaveObjectStatistics = True
            initializer.Link = dto.Attachment.Link
            initializer.SetOnModalPageByItem = True
            initializer.SetPreviousPage = False

            Dim actions As List(Of dtoModuleActionControl)
            'initializer.OnModalPage
            '  initializer.OpenLinkCssClass

            actions = renderItem.InitializeRemoteControl(initializer, StandardActionType.Play, lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction)

            Dim isReadyToPlay As Boolean = (renderItem.Availability = lm.Comol.Core.FileRepository.Domain.ItemAvailability.available)
            Dim isReadyToManage As Boolean = isReadyToPlay OrElse (renderItem.Availability = lm.Comol.Core.FileRepository.Domain.ItemAvailability.waitingsettings)

            'Dim initializer As New lm.Comol.Core.ModuleLinks.dtoModuleDisplayActionInitializer

            '' DIMENSIONI IMMAGINI
            'initializer.IconSize = Helpers.IconSize.Small
            'renderFile.EnableAnchor = True
            'initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction Or lm.Comol.Core.ModuleLinks.DisplayActionMode.actions
            'initializer.Link = dto.Attachment.Link
            'renderFile.InsideOtherModule = True
            'Dim actions As List(Of dtoModuleActionControl)
            'actions = renderFile.InitializeRemoteControl(initializer, StandardActionType.Play)

            Dim oHyperlink As HyperLink
            If isReadyToPlay AndAlso actions.Where(Function(a) a.ControlType = StandardActionType.ViewUserStatistics).Any Then
                oHyperlink = e.Item.FindControl("HYPstats")
                oHyperlink.ToolTip = Resource.getValue("statistic.ItemType." & renderItem.ItemType.ToString)
                oHyperlink.Visible = True
                oHyperlink.NavigateUrl = actions.Where(Function(a) a.ControlType = StandardActionType.ViewUserStatistics).Select(Function(a) a.LinkUrl).FirstOrDefault
            End If
            If isReadyToManage AndAlso actions.Where(Function(a) a.ControlType = StandardActionType.EditMetadata).Any Then
                oHyperlink = e.Item.FindControl("HYPeditMetadata")
                oHyperlink.Visible = True
                oHyperlink.ToolTip = Resource.getValue("settings.ItemType." & renderItem.ItemType.ToString)
                oHyperlink.NavigateUrl = actions.Where(Function(a) a.ControlType = StandardActionType.EditMetadata).Select(Function(a) a.LinkUrl).FirstOrDefault
            End If


            Dim oButton As Button = e.Item.FindControl("BTNvirtualDelete")
            oButton.CommandArgument = dto.Id
            oButton.Visible = dto.AllowVirtualDelete AndAlso AllowSave

            Dim oLabel As Label = e.Item.FindControl("LBdescriptionAttachment_t")
            Me.Resource.setLabel(oLabel)

            oLabel = e.Item.FindControl("LBmoveAttachment")
            Me.Resource.setLabel(oLabel)
            oLabel.Visible = AllowSave

            Dim oTextBox As TextBox = e.Item.FindControl("TXBdescription")
            oTextBox.Text = dto.Attachment.Description
            oTextBox.Enabled = AllowSave

            oLabel = e.Item.FindControl("LBattachmentForAll_t")
            Me.Resource.setLabel(oLabel)
            Dim oChecbox As HtmlInputCheckBox = e.Item.FindControl("CBXforAll")
            oChecbox.Checked = dto.Attachment.ForAll
            oChecbox.Disabled = Not AllowSave

            oLabel = e.Item.FindControl("LBattachmentSubmitters_t")
            Me.Resource.setLabel(oLabel)

            Dim hidden As HtmlInputHidden = e.Item.FindControl("HDNdisplayOrder")
            hidden.Value = dto.Attachment.DisplayOrder

            Dim oSelect As HtmlSelect = e.Item.FindControl("SLBsubmitters")
            oSelect.DataSource = Availablesubmitters
            oSelect.DataTextField = "Name"
            oSelect.DataValueField = "Id"
            oSelect.DataBind()
            If Not dto.Attachment.ForAll AndAlso dto.Attachment.SubmitterAssignments.Count > 0 Then
                For Each idSubmitter As Long In dto.Attachment.SubmitterAssignments
                    Dim oListItem As ListItem = oSelect.Items.FindByValue(idSubmitter)
                    If Not IsNothing(oListItem) Then
                        oListItem.Selected = True
                    End If
                Next
            End If
            oSelect.Attributes.Add("data-placeholder", Resource.getValue("Submitters.data-placeholder"))
            oSelect.Disabled = Not AllowSave OrElse dto.Attachment.ForAll
            Dim oHtmlControl As HtmlGenericControl = e.Item.FindControl("SPNattchSelectAll")
            oHtmlControl.Attributes.Add("title", Resource.getValue("SPNattchSelectAll.ToolTip"))
            oHtmlControl.Visible = AllowSave
            oHtmlControl = e.Item.FindControl("SPNattchSelectNone")
            oHtmlControl.Attributes.Add("title", Resource.getValue("SPNattchSelectNone.ToolTip"))
            oHtmlControl.Visible = AllowSave
        End If
    End Sub
    Private Sub RPTattachments_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTattachments.ItemCommand
        Dim idAttachment As Long = 0
        If Not String.IsNullOrEmpty(e.CommandArgument) AndAlso IsNumeric(e.CommandArgument) Then
            idAttachment = CLng(e.CommandArgument)
        End If
        If e.CommandName = "virtualDelete" Then 'fix delete->virtualdelete
            Me.CurrentPresenter.RemoveAttachment(idAttachment)
        End If
    End Sub
    Private Sub BTNsaveAttachmentsBottom_Click(sender As Object, e As System.EventArgs) Handles BTNsaveAttachmentsBottom.Click, BTNsaveAttachmentsTop.Click
        Me.CurrentPresenter.SaveSettings(GetAttachments)

        CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.CallEditSave,
            Me.IdCall,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.CallForPeaper,
            "Attachments")

    End Sub

#Region "Add Controls"
    Private Sub CTRLaddUrls_ItemsAdded(action As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions) Handles CTRLinternalUpload.ItemsAdded
        Master.ClearOpenedDialogOnPostback()
        CurrentPresenter.SaveSettings(GetAttachments)
        CurrentPresenter.LoadAttachments(IdCall)
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("CallForPaperType." & CallType.ToString & ".ItemsAdded.Call.RepositoryAttachmentUploadActions." & action.ToString), Helpers.MessageType.success)
    End Sub
    Private Sub CTRLaddUrls_ItemsNotAdded(action As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions) Handles CTRLinternalUpload.ItemsNotAdded
        Master.ClearOpenedDialogOnPostback()
        CurrentPresenter.SaveSettings(GetAttachments)
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("CallForPaperType." & CallType.ToString & ".ItemsNotAdded.Call.RepositoryAttachmentUploadActions." & action.ToString), Helpers.MessageType.error)
    End Sub
    Private Sub CTRLadd_NoFilesToAdd(action As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions) Handles CTRLinternalUpload.NoFilesToAdd
        Master.ClearOpenedDialogOnPostback()
        CurrentPresenter.SaveSettings(GetAttachments)
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("CallForPaperType." & CallType.ToString & ".NoFilesToAdd.Call.RepositoryAttachmentUploadActions." & action.ToString), Helpers.MessageType.alert)
    End Sub
    Private Sub CTRLadd_WorkingSessionExpired() Handles CTRLinternalUpload.WorkingSessionExpired
        Master.ClearOpenedDialogOnPostback()
        Me.DisplaySessionTimeout()
    End Sub
    Private Sub CTRLadd_CallNotFound() Handles CTRLinternalUpload.CallNotFound
        Master.ClearOpenedDialogOnPostback()
        MLVsettings.SetActiveView(VIWempty)
    End Sub
#End Region
#End Region
   

    'Private Sub BTNuploadAttachments_Click(sender As Object, e As System.EventArgs) Handles BTNuploadAttachments.Click
    '    Me.CurrentPresenter.StartUpload()
    '    Me.DVuploadAttachments.Visible = False
    'End Sub

    'Private Sub LNBaddAttachments_Click(sender As Object, e As System.EventArgs) Handles LNBaddAttachments.Click
    '    'Me.CurrentPresenter.SaveSettings(GetAttachments)
    '    Me.DVuploadAttachments.Visible = True
    'End Sub

    'Private Sub BTNundoUpload_Click(sender As Object, e As System.EventArgs) Handles BTNundoUpload.Click
    '    Me.DVuploadAttachments.Visible = False
    'End Sub
End Class