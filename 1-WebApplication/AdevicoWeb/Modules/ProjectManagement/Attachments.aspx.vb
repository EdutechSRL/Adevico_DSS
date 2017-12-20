Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Imports lm.Comol.Modules.Standard.ProjectManagement.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract

Public Class ProjectAttachments
    Inherits PMpageBaseEdit
    Implements IViewAttachments

#Region "Context"
    Private _Presenter As AttachmentsPresenter
    Private ReadOnly Property CurrentPresenter() As AttachmentsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New AttachmentsPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
#Region "Settings"
    Private Property AllowSave As Boolean Implements IViewAttachments.AllowSave
        Get
            Return ViewStateOrDefault("AllowSave", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowSave") = value
            THcheckbox.Visible = value
            THactions.Visible = value
        End Set
    End Property
#End Region

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
    Private LoadingAttachmentsError As Boolean = False
#End Region
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

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ProjectManagement", "Modules", "ProjectManagement")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setHyperLink(HYPbackToProjectsBottom, True, True)
            .setHyperLink(HYPbackToProjectsTop, True, True)
            Master.ServiceTitle = .getValue("ServiceTitle.ProjectAttachments")
            Master.ServiceTitleToolTip = .getValue("ServiceTitle.ProjectAttachments.ToolTip")

            .setHyperLink(HYPgoToProjectMapTop, False, True)
            .setHyperLink(HYPbackToResourceDashboardTop, False, True)
            .setHyperLink(HYPbackToManagerDashboardTop, False, True)
            .setHyperLink(HYPgoToProjectMapBottom, False, True)
            .setHyperLink(HYPbackToResourceDashboardBottom, False, True)
            .setHyperLink(HYPbackToManagerDashboardBottom, False, True)


            .setLiteral(LTprojectAttacments)
            .setLiteral(LTattachmentnameHeader)
            .setLiteral(LTattachmenttypeHeader)
            .setLiteral(LTattachmentactionsHeader)

            .setButton(BTNexportToCommunityFolder, True)
            .setButton(BTNvirtualDeleteSelectedAttacchments, True)
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

#End Region

#Region "Implements"
#Region "Display"
    Private Sub DisplayUnknownProject() Implements IViewAttachments.DisplayUnknownProject
        Me.MLVsettings.SetActiveView(VIWempty)
        Me.LBnoResources.Text = Resource.getValue("DisplayUnknownProject.ProjectAttachments")
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewAttachments.DisplaySessionTimeout
        RedirectOnSessionTimeOut(RootObject.ProjectCalendars(IdProject, ProjectIdCommunity, forPortal, isPersonal, FromPage, PreloadIdContainerCommunity), IIf(PreloadIdContainerCommunity > 0, PreloadIdContainerCommunity, ProjectIdCommunity))
    End Sub
    Private Sub DisplayUrlRemoved() Implements IViewAttachments.DisplayUrlRemoved
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayUrlRemoved"), Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayDeletedItems(count As Long) Implements IViewAttachments.DisplayDeletedItems
        DisplayMessage("DisplayDeletedItems", count, Helpers.MessageType.success)
    End Sub
    Private Sub DisplayNoPermissionToDeleteItems(count As Long) Implements IViewAttachments.DisplayNoPermissionToDeleteItems
        DisplayMessage("DisplayNoPermissionToDeleteItems", count, Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayUnableToDeleteItems(count As Long) Implements IViewAttachments.DisplayUnableToDeleteItems
        DisplayMessage("DisplayNoPermissionToDeleteItems", count, Helpers.MessageType.error)
    End Sub
    Private Sub DisplayMessage(message As String, count As Long, mType As Helpers.MessageType)
        CTRLmessages.Visible = True
        If count > 1 Then
            message = String.Format(Resource.getValue(message & ".n"), count)
        Else
            message = Resource.getValue(message & "." & count.ToString)
        End If
        CTRLmessages.InitializeControl(message, mType)
    End Sub
    Private Sub DisplayNoPermissionToEditUrl() Implements IViewAttachments.DisplayNoPermissionToEditUrl
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayNoPermissionToEditUrl"), Helpers.MessageType.alert)
    End Sub

    Private Sub DisplayUnableToSaveEmptyUrl(urlItem As lm.Comol.Core.DomainModel.dtoUrl) Implements IViewAttachments.DisplayUnableToSaveEmptyUrl
        DisplayMessage("DisplayUnableToSaveEmptyUrl", urlItem, Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayUnableToSaveUrl(urlItem As lm.Comol.Core.DomainModel.dtoUrl) Implements IViewAttachments.DisplayUnableToSaveUrl
        DisplayMessage("DisplayUnableToSaveEmptyUrl", urlItem, Helpers.MessageType.error)
    End Sub
    Private Sub DisplaySavedUrl(urlItem As lm.Comol.Core.DomainModel.dtoUrl) Implements IViewAttachments.DisplaySavedUrl
        DisplayMessage("DisplaySavedUrl", urlItem, Helpers.MessageType.success)
    End Sub
    Private Sub DisplayMessage(message As String, urlItem As lm.Comol.Core.DomainModel.dtoUrl, mType As Helpers.MessageType)
        CTRLmessages.Visible = True
        If urlItem.DisplayName <> urlItem.Address Then
            message = String.Format(Resource.getValue(message & ".Url.Name"), urlItem.Address, urlItem.DisplayName)
        ElseIf Not String.IsNullOrEmpty(urlItem.Address) Then
            message = String.Format(Resource.getValue(message & ".Url"), urlItem.Address)
        ElseIf Not String.IsNullOrEmpty(urlItem.DisplayName) Then
            message = String.Format(Resource.getValue(message & ".Name"), urlItem.DisplayName)
        Else
            message = Resource.getValue(message)
        End If
        CTRLmessages.InitializeControl(message, mType)
    End Sub
#End Region

    Private Sub LoadWizardSteps(idProject As Long, idCommunity As Integer, personal As Boolean, forPortal As Boolean, steps As List(Of lm.Comol.Core.Wizard.NavigableWizardItem(Of dtoProjectManagementStep))) Implements IViewBaseEdit.LoadWizardSteps
        Me.CTRLsteps.InitializeControl(idProject, idCommunity, personal, forPortal, steps, FromPage, PreloadIdContainerCommunity)
    End Sub

    Private Sub SetProjectsUrl(url As String) Implements IViewBaseEdit.SetProjectsUrl
        If Not String.IsNullOrEmpty(url) Then
            HYPbackToProjectsTop.Visible = True
            HYPbackToProjectsBottom.Visible = True
            HYPbackToProjectsTop.NavigateUrl = PageUtility.ApplicationUrlBase() & url
            HYPbackToProjectsBottom.NavigateUrl = HYPbackToProjectsTop.NavigateUrl
        End If
    End Sub
    Private Sub SetProjectMapUrl(url As String) Implements IViewBaseEdit.SetProjectMapUrl
        If Not String.IsNullOrEmpty(url) Then
            HYPgoToProjectMapTop.Visible = True
            HYPgoToProjectMapBottom.Visible = True
            HYPgoToProjectMapTop.NavigateUrl = PageUtility.ApplicationUrlBase() & url
            HYPgoToProjectMapBottom.NavigateUrl = HYPgoToProjectMapTop.NavigateUrl
        End If
    End Sub
    Private Sub SetDashboardUrl(url As String, fromPage As PageListType) Implements IViewBaseEdit.SetDashboardUrl
        If Not String.IsNullOrEmpty(url) Then
            Select Case fromPage
                Case PageListType.DashboardAdministrator, PageListType.DashboardManager, PageListType.ProjectDashboardManager
                    HYPbackToManagerDashboardTop.Visible = True
                    HYPbackToManagerDashboardBottom.Visible = True
                    HYPbackToManagerDashboardTop.NavigateUrl = PageUtility.ApplicationUrlBase() & url
                    HYPbackToManagerDashboardBottom.NavigateUrl = HYPbackToManagerDashboardTop.NavigateUrl
                Case Else
                    HYPbackToResourceDashboardTop.Visible = True
                    HYPbackToResourceDashboardBottom.Visible = True
                    HYPbackToResourceDashboardTop.NavigateUrl = PageUtility.ApplicationUrlBase() & url
                    HYPbackToResourceDashboardBottom.NavigateUrl = HYPbackToResourceDashboardTop.NavigateUrl
            End Select

        End If
    End Sub
    Private Sub InitializeAttachmentsControl(rPermissions As CoreModuleRepository, actions As List(Of lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions), Optional dAction As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions = lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.none) Implements IViewAttachments.InitializeAttachmentsControl
        Me.CTRLcommands.Visible = actions.Any()
        Me.CTRLcommands.InitializeControlForJQuery(actions, dAction)
        If Not IsNothing(actions) Then
            Me.CTRLattachmentsHeader.InitializeControlForJQuery(actions, actions.ToDictionary(Of lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions, String)(Function(a) a, Function(a) Resource.getValue("dialogTitle.RepositoryAttachmentUploadActions." & a.ToString)))

            If actions.Contains(lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.addurltomoduleitem) Then
                CTRLaddUrls.Visible = True
                CTRLaddUrls.InitializeControl(IdProject, Repository.RepositoryAttachmentUploadActions.addurltomoduleitem, ProjectIdCommunity, Resource.getValue("dialogDescription.Project." & lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.addurltomoduleitem.ToString))
            Else
                CTRLaddUrls.Visible = False
            End If

            If actions.Contains(lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem) Then
                CTRLinternalUpload.Visible = True
                CTRLinternalUpload.InitializeControl(IdProject, Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem, ProjectIdCommunity, Resource.getValue("dialogDescription.Project." & lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem.ToString))
            Else
                CTRLinternalUpload.Visible = False
            End If

            If actions.Contains(lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitemandcommunity) Then
                CTRLcommunityUpload.Visible = True
                CTRLcommunityUpload.InitializeControl(IdProject, Repository.RepositoryAttachmentUploadActions.uploadtomoduleitemandcommunity, ProjectIdCommunity, rPermissions, Resource.getValue("dialogDescription.Project." & lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitemandcommunity.ToString))
            Else
                CTRLcommunityUpload.Visible = False
            End If

            If actions.Contains(lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.linkfromcommunity) Then
                CTRLlinkFromCommunity.Visible = True
                CTRLlinkFromCommunity.InitializeControl(IdProject, Repository.RepositoryAttachmentUploadActions.linkfromcommunity, ProjectIdCommunity, rPermissions, Resource.getValue("dialogDescription.Project." & lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.linkfromcommunity.ToString))
            Else
                CTRLlinkFromCommunity.Visible = False
            End If
        End If
    End Sub

    Private Sub LoadAttachments(items As List(Of dtoAttachmentItem)) Implements IViewAttachments.LoadAttachments
        If IsNothing(items) Then
            items = New List(Of dtoAttachmentItem)
            LoadingAttachmentsError = True
        ElseIf AllowSave AndAlso items.Where(Function(item) item.Attachment.Type = AttachmentType.url).Any() Then
            CTRLeditUrlItemsHeader.InitializeControl("opendlg" & CTRLeditUrl.EditingCssClass, CTRLeditUrl.EditingCssClass, Resource.getValue("DisplayUrlForEditing.Title"), CInt(LTediturlWidth.Text), CInt(LTediturlHeight.Text), , , True)
        End If

        RPTattachments.DataSource = items
        RPTattachments.DataBind()

        TFcommands.Visible = items.Any AndAlso AllowSave
        ' BTNexportToCommunityFolder.Visible = items.Where(Function(i) i.Attachment.Type = AttachmentType.File AndAlso Not i.Attachment.File.IsInternal).Any()
        BTNvirtualDeleteSelectedAttacchments.Visible = items.Any
    End Sub

    Private Sub DisplayUrlForEditing(urlItem As lm.Comol.Core.DomainModel.dtoUrl) Implements IViewAttachments.DisplayUrlForEditing
        CTRLeditUrl.Visible = True
        CTRLeditUrl.InitializeControl(urlItem)
        Me.Master.SetOpenDialogOnPostbackByCssClass(CTRLeditUrl.EditingCssClass)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idProject As Long, idAttachment As Long, action As lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement.ActionType) Implements IViewAttachments.SendUserAction
        Dim objects As List(Of WS_Actions.ObjectAction) = PageUtility.CreateObjectsList(idModule, ModuleProjectManagement.ObjectType.Project, idProject.ToString)
        objects.AddRange(PageUtility.CreateObjectsList(idModule, ModuleProjectManagement.ObjectType.ProjectAttachment, idAttachment.ToString))
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, objects, InteractionType.UserWithLearningObject)
    End Sub
#End Region

#Region "Internal"

    Private Sub ProjectResources_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        Me.Master.ShowDocType = True
    End Sub
    Protected Function GetCssClass(t As AttachmentType) As String
        Return t.ToString.ToLower
    End Function

    Private Sub RPTattachments_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTattachments.ItemCommand
        CTRLmessages.Visible = False
        Select Case e.CommandName
            Case "edit"
                CurrentPresenter.EditUrl(e.CommandArgument.ToString.Split("|")(0), e.CommandArgument.ToString.Split("|")(1))
            Case "virtualdelete"
                Dim idAttachmentLink As Long = CLng(e.CommandArgument)
                CurrentPresenter.VirtualDeleteAttachment(idAttachmentLink)
        End Select
    End Sub
    Private Sub RPTattachments_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTattachments.ItemDataBound
        Select Case e.Item.ItemType
            Case ListItemType.AlternatingItem, ListItemType.Item
                Dim dto As dtoAttachmentItem = e.Item.DataItem
                Dim oTableCell As HtmlTableCell = e.Item.FindControl("TDcheckbox")
                oTableCell.Visible = AllowSave

                oTableCell = e.Item.FindControl("TDactions")
                oTableCell.Visible = AllowSave

                Dim oLinkbutton As LinkButton = e.Item.FindControl("LNBeditUrl")
                If dto.Permissions.Edit AndAlso dto.Attachment.Type = AttachmentType.url Then
                    oLinkbutton.Visible = True
                    Resource.setLinkButton(oLinkbutton, True, True)
                End If

                oLinkbutton = e.Item.FindControl("LNBvirtualDeleteAttachment")
                If dto.Permissions.VirtualDelete Then
                    oLinkbutton.Visible = True
                    Resource.setLinkButton(oLinkbutton, True, True)
                End If

                Dim oLiteral As Literal = e.Item.FindControl("LTattachmentType")
                oLiteral.Text = Resource.getValue("LTattachmentType.AttachmentType." & dto.Attachment.Type.ToString)

                Select Case dto.Attachment.Type
                    Case AttachmentType.url
                        Dim renderUrl As UC_DisplayUrlItem = e.Item.FindControl("CTRLdisplayUrl")
                        renderUrl.IconSize = Helpers.IconSize.Small
                        renderUrl.InitializeControl(lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction, dto.Attachment.Url, dto.Attachment.DisplayName)
                        renderUrl.Visible = True
                    Case AttachmentType.file
                        Dim renderFile As UC_ModuleRepositoryAction = e.Item.FindControl("CTRLdisplayFile")

                        Dim initializer As New lm.Comol.Core.ModuleLinks.dtoModuleDisplayActionInitializer

                        ' DIMENSIONI IMMAGINI
                        initializer.IconSize = Helpers.IconSize.Small
                        renderFile.EnableAnchor = True
                        initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction Or lm.Comol.Core.ModuleLinks.DisplayActionMode.actions
                        initializer.Link = dto.Attachment.Link
                        renderFile.InsideOtherModule = True
                        Dim actions As List(Of dtoModuleActionControl)
                        'actions = renderFile.InitializeRemoteControl(initializer, StandardActionType.Play Or StandardActionType.EditMetadata Or StandardActionType.ViewUserStatistics)
                        actions = renderFile.InitializeRemoteControl(initializer, StandardActionType.Play)
                        Dim isReadyToPlay As Boolean = renderFile.IsReadyToPlay
                        Dim oHyperlink As HyperLink
                        If isReadyToPlay AndAlso dto.Permissions.ViewOtherStatistics AndAlso actions.Where(Function(a) a.ControlType = StandardActionType.ViewAdvancedStatistics).Any Then
                            oHyperlink = e.Item.FindControl("HYPstats")
                            oHyperlink.Visible = True
                            oHyperlink.ToolTip = Resource.getValue("statistic.RepositoryItemType." & renderFile.ItemType.ToString)
                            oHyperlink.NavigateUrl = actions.Where(Function(a) a.ControlType = StandardActionType.ViewAdvancedStatistics).Select(Function(a) a.LinkUrl).FirstOrDefault
                        ElseIf isReadyToPlay AndAlso dto.Permissions.ViewMyStatistics AndAlso actions.Where(Function(a) a.ControlType = StandardActionType.ViewUserStatistics).Any Then
                            oHyperlink = e.Item.FindControl("HYPstats")
                            oHyperlink.ToolTip = Resource.getValue("statistic.RepositoryItemType." & renderFile.ItemType.ToString)
                            oHyperlink.Visible = True
                            oHyperlink.NavigateUrl = actions.Where(Function(a) a.ControlType = StandardActionType.ViewUserStatistics).Select(Function(a) a.LinkUrl).FirstOrDefault
                        End If
                        If isReadyToPlay AndAlso dto.Permissions.SetMetadata AndAlso actions.Where(Function(a) a.ControlType = StandardActionType.EditMetadata).Any Then
                            oHyperlink = e.Item.FindControl("HYPeditMetadata")
                            oHyperlink.ToolTip = Resource.getValue("settings.RepositoryItemType." & renderFile.ItemType.ToString)
                            oHyperlink.Visible = True
                            oHyperlink.NavigateUrl = actions.Where(Function(a) a.ControlType = StandardActionType.EditMetadata).Select(Function(a) a.LinkUrl).FirstOrDefault
                        End If
                End Select
            Case ListItemType.Footer

                Dim oTableItem As HtmlControl = e.Item.FindControl("TRempty")
                oTableItem.Visible = (RPTattachments.Items.Count = 0)
                If (RPTattachments.Items.Count = 0) Then
                    Dim oTableCell As HtmlTableCell = e.Item.FindControl("TDemptyItems")

                    oTableCell.ColSpan = 2
                    If AllowSave Then
                        oTableCell.ColSpan += 2
                    End If
                    Dim oLabel As Label = e.Item.FindControl("LBprojectAttachmentsEmptyItems")
                    If LoadingAttachmentsError Then
                        oLabel.Text = Resource.getValue("IViewAttachments.LoadingAttachmentsError")
                    Else
                        Resource.setLabel(oLabel)
                    End If

                End If
        End Select
    End Sub

#Region "Add Controls"
    Private Sub CTRLaddUrls_ItemsAdded(action As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions) Handles CTRLaddUrls.ItemsAdded, CTRLcommunityUpload.ItemsAdded, CTRLinternalUpload.ItemsAdded, CTRLlinkFromCommunity.ItemsAdded
        Master.ClearOpenedDialogOnPostback()
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("ItemsAdded.Project.RepositoryAttachmentUploadActions." & action.ToString), Helpers.MessageType.success)
        CurrentPresenter.LoadAttachments(IdProject, forPortal, isPersonal, ProjectIdCommunity)
    End Sub
    Private Sub CTRLaddUrls_ItemsNotAdded(action As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions) Handles CTRLaddUrls.ItemsNotAdded, CTRLcommunityUpload.ItemsNotAdded, CTRLinternalUpload.ItemsNotAdded, CTRLlinkFromCommunity.ItemsNotAdded
        Master.ClearOpenedDialogOnPostback()
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("ItemsNotAdded.Project.RepositoryAttachmentUploadActions." & action.ToString), Helpers.MessageType.error)
        CurrentPresenter.LoadAttachments(IdProject, forPortal, isPersonal, ProjectIdCommunity)
    End Sub
    Private Sub CTRLadd_NoFilesToAdd(action As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions) Handles CTRLcommunityUpload.NoFilesToAdd, CTRLinternalUpload.NoFilesToAdd, CTRLlinkFromCommunity.NoFilesToAdd
        Master.ClearOpenedDialogOnPostback()
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("NoFilesToAdd.Project.RepositoryAttachmentUploadActions." & action.ToString), Helpers.MessageType.alert)
        CurrentPresenter.LoadAttachments(IdProject, forPortal, isPersonal, ProjectIdCommunity)
    End Sub
    Private Sub CTRLadd_WorkingSessionExpired() Handles CTRLaddUrls.WorkingSessionExpired, CTRLcommunityUpload.WorkingSessionExpired, CTRLinternalUpload.WorkingSessionExpired, CTRLlinkFromCommunity.WorkingSessionExpired
        Master.ClearOpenedDialogOnPostback()
        Me.DisplaySessionTimeout()
    End Sub
    Private Sub CTRLadd_ProjectNotFound() Handles CTRLaddUrls.ProjectNotFound, CTRLcommunityUpload.ProjectNotFound, CTRLinternalUpload.ProjectNotFound, CTRLlinkFromCommunity.ProjectNotFound
        Master.ClearOpenedDialogOnPostback()
        Me.MLVsettings.SetActiveView(VIWempty)
        Me.LBnoResources.Text = Resource.getValue("DisplayUnknownProject.ProjectAttachments")
        CurrentPresenter.LoadAttachments(IdProject, forPortal, isPersonal, ProjectIdCommunity)
    End Sub
#End Region

    Private Sub CTRLeditUrl_SavingSettings(items As List(Of lm.Comol.Core.DomainModel.dtoUrl)) Handles CTRLeditUrl.SavingSettings
        CTRLeditUrl.Visible = False
        CurrentPresenter.SaveUrl(items.FirstOrDefault())
    End Sub
    Private Sub BTNvirtualDeleteSelectedAttacchments_Click(sender As Object, e As System.EventArgs) Handles BTNvirtualDeleteSelectedAttacchments.Click
        CurrentPresenter.VirtualDeleteAttachments(GetSelectedItems)
    End Sub

    Private Function GetSelectedItems() As List(Of Long)
        Dim items As New List(Of Long)

        For Each row As RepeaterItem In (From r As RepeaterItem In RPTattachments.Items Where r.ItemType = ListItemType.Item OrElse r.ItemType = ListItemType.AlternatingItem)
            Dim oLiteral As Literal = row.FindControl("LTidLink")
            Dim oCheck As HtmlInputCheckBox = row.FindControl("CBXattachment")
            If oCheck.Checked Then
                items.Add(CLng(oLiteral.Text))
            End If
        Next
        Return items
    End Function
#End Region

   
    
End Class