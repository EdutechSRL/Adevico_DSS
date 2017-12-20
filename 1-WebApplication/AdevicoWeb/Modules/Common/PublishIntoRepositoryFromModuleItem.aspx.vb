Imports lm.Comol.UI.Presentation
'Imports COL_BusinessLogic_v2.UCServices
Imports lm.ActionDataContract
Imports lm.Comol.Core.DomainModel
'Imports lm.Comol.Core.BaseModules.CommunityDiary.Presentation
Imports lm.Comol.Core.BaseModules.CommunityDiary.Domain
Imports lm.Comol.Core.BaseModules.Domain
Imports lm.Comol.Core.BaseModules.Repository.Presentation

Public Class PublishIntoRepositoryFromModuleItem
    Inherits PageBase
    Implements IViewPublishIntoRepositoryFromModuleItem


#Region "Inherits"
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Presenter As PublishIntoRepositoryFromModulePresenter
    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As PublishIntoRepositoryFromModulePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New PublishIntoRepositoryFromModulePresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property

    Protected Function BackGroundItem(ByVal ItemType As ListItemType) As String
        If ItemType = ListItemType.AlternatingItem Then
            Return "ROW_Alternate_Small"
        Else
            Return "ROW_Normal_Small"
        End If
    End Function

#End Region

#Region "Implements"
    Public ReadOnly Property BaseFolder As String Implements IViewPublishIntoRepositoryFromModuleItem.BaseFolder
        Get
            Return Me.Resource.getValue("BaseFolder")
        End Get
    End Property
    Public Property SelectedFolderId() As Long Implements IViewPublishIntoRepositoryFromModuleItem.SelectedFolderId
        Get
            SelectedFolderId = Me.CTRLCommunityFolder.SelectedFolder
        End Get
        Set(ByVal value As Long)
            Me.CTRLCommunityFolder.SelectedFolder = value
        End Set
    End Property
    Public ReadOnly Property SelectedFolderName() As String Implements IViewPublishIntoRepositoryFromModuleItem.SelectedFolderName
        Get
            Return Me.CTRLCommunityFolder.SelectedFolderName
        End Get
    End Property
    Public Property SelectedCommunityID() As Integer Implements IViewPublishIntoRepositoryFromModuleItem.SelectedCommunityID
        Get
            If String.IsNullOrEmpty(Me.ViewState("SelectedCommunityID")) Then
                Return -1
            Else
                Return CInt(Me.ViewState("SelectedCommunityID"))
            End If
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("SelectedCommunityID") = value
        End Set
    End Property
    Public ReadOnly Property RepositoryPermissions As List(Of ModuleCommunityPermission(Of CoreModuleRepository)) Implements IViewPublishIntoRepositoryFromModuleItem.RepositoryPermissions
        Get
            Dim modules As List(Of ModuleCommunityPermission(Of CoreModuleRepository))

            modules = (From sb In ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, CoreModuleRepository.UniqueID, True) _
                    Select New ModuleCommunityPermission(Of CoreModuleRepository) With {.ID = sb.CommunityID, .Permissions = New CoreModuleRepository(sb.PermissionString)}).ToList()

            modules.Add(New ModuleCommunityPermission(Of CoreModuleRepository) With {.ID = 0, .Permissions = CoreModuleRepository.CreatePortalmodule(Me.CurrentContext.UserContext.UserTypeID)})
            Return modules
        End Get
    End Property

    Public ReadOnly Property PreloadedBackUrl As String Implements IViewPublishIntoRepositoryFromModuleItem.PreloadedBackUrl
        Get
            Return PageUtility.DecryptQueryString("BackUrl", UtilityLibrary.SecretKeyUtil.EncType.Altro)
        End Get
    End Property
    Public ReadOnly Property PreloadedModuleOwnerCode As String Implements IViewPublishIntoRepositoryFromModuleItem.PreloadedModuleOwnerCode
        Get
            Return PageUtility.DecryptQueryString("ServiceCode", UtilityLibrary.SecretKeyUtil.EncType.Altro)
        End Get
    End Property
    Public ReadOnly Property PreloadedItemId As String Implements IViewPublishIntoRepositoryFromModuleItem.PreloadedItemId
        Get
            Return PageUtility.DecryptQueryString("ItemID", UtilityLibrary.SecretKeyUtil.EncType.Altro)
        End Get
    End Property
    Public ReadOnly Property PreloadedLinkId As Long Implements IViewPublishIntoRepositoryFromModuleItem.PreloadedLinkId
        Get
            Return PageUtility.DecryptQueryString("LinkID", UtilityLibrary.SecretKeyUtil.EncType.Altro)
        End Get
    End Property
    Public Property ModuleOwnerCode As String Implements IViewPublishIntoRepositoryFromModuleItem.ModuleOwnerCode
        Get
            Return ViewStateOrDefault("ModuleOwnerCode", "")
        End Get
        Set(ByVal value As String)
            ViewState("ModuleOwnerCode") = value
            Me.Master.ServiceTitle = Resource.getValue("serviceTitle_" & value)
        End Set
    End Property
    Public Property ModuleOwnerID As Integer Implements IViewPublishIntoRepositoryFromModuleItem.ModuleOwnerID
        Get
            Return ViewStateOrDefault("ModuleOwnerID", -1)
        End Get
        Set(ByVal value As Integer)
            ViewState("ModuleOwnerID") = value
        End Set
    End Property
    Public ReadOnly Property HasPermissionToSelectFile As Boolean Implements IViewPublishIntoRepositoryFromModuleItem.HasPermissionToSelectFile
        Get
            Return CTRLmoduleFileSelector.HasPermissionToSelectFile
        End Get
    End Property
    Public ReadOnly Property InternalFileSelectorIdCommunity As Integer Implements IViewPublishIntoRepositoryFromModuleItem.InternalFileSelectorIdCommunity
        Get
            Return Me.CTRLmoduleFileSelector.SelectorIdCommunity
        End Get
    End Property
    Public Property DeafultBackUrl As String Implements IViewPublishIntoRepositoryFromModuleItem.DeafultBackUrl
        Get
            Return ViewStateOrDefault("DeafultBackUrl", "")
        End Get
        Set(ByVal value As String)
            ViewState("DeafultBackUrl") = value
        End Set
    End Property

    Public ReadOnly Property PortalHome As String Implements IViewPublishIntoRepositoryFromModuleItem.PortalHome
        Get
            Return Resource.getValue("PortalHome")
        End Get
    End Property
    Private Function ViewStateOrDefault(Of T)(ByVal Key As String, ByVal DefaultValue As T) As T
        If (ViewState(Key) Is Nothing) Then
            ViewState(Key) = DefaultValue
            Return DefaultValue
        Else
            Return ViewState(Key)
        End If
    End Function
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return True
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Me.Master.ShowNoPermission = False
        If Page.IsPostBack = False Then
            Me.CurrentPresenter.InitView()
        End If
    End Sub
    Public Overrides Sub BindNoPermessi()
        Me.Master.ShowNoPermission = True
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_PublishIntoRepositoryFromModuleItem", "Modules", "Repository")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setHyperLink(HYPbackToFileManagement, True, True)

            Me.Master.ServiceTitle = .getValue("serviceTitlePublish_" & ModuleOwnerCode)
            Me.Master.ServiceNopermission = .getValue("nopermission")
            .setLabel(Me.LBfileToExport)
            .setLabel(Me.LBmultipleFileError)
            .setLabel(Me.LBselectFolder)
            .setLabel(Me.LBsummary)
            Dim oButton As Button
            oButton = Me.WZRcommunityDiary.FindControl("StartNavigationTemplateContainerID").FindControl("StartNextButton")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If
            oButton = Me.WZRcommunityDiary.FindControl("StepNavigationTemplateContainerID").FindControl("StepPreviousButton")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If
            oButton = Me.WZRcommunityDiary.FindControl("StepNavigationTemplateContainerID").FindControl("StepNextButton")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If
            oButton = Me.WZRcommunityDiary.FindControl("FinishNavigationTemplateContainerID").FindControl("FinishPreviousButton")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If
            oButton = Me.WZRcommunityDiary.FindControl("FinishNavigationTemplateContainerID").FindControl("FinishButton")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub

#End Region

#Region "File Selector"
    Public Sub SetBackUrl(ByVal url As String) Implements IViewPublishIntoRepositoryFromModuleItem.SetBackUrl
        If String.IsNullOrEmpty(url) Then
            Me.HYPbackToFileManagement.Visible = False
            DeafultBackUrl = ""
        Else
            Me.HYPbackToFileManagement.Visible = True
            HYPbackToFileManagement.NavigateUrl = "~/" & PageUtility.GetUrlDecoded(url)
            DeafultBackUrl = PageUtility.GetUrlDecoded(url)

            Resource.setHyperLink(HYPbackToFileManagement, ModuleOwnerCode, True, True)

            Dim oButton As Button
            oButton = Me.WZRcommunityDiary.FindControl("StartNavigationTemplateContainerID").FindControl("BTNgoToBackUrl")
            If Not IsNothing(oButton) Then
                oButton.Visible = True
                Me.Resource.setButtonByValue(oButton, ModuleOwnerCode, True, , , True)
            End If
            oButton = Me.WZRcommunityDiary.FindControl("StepNavigationTemplateContainerID").FindControl("BTNgoToBackUrl")
            If Not IsNothing(oButton) Then
                oButton.Visible = True
                Me.Resource.setButtonByValue(oButton, ModuleOwnerCode, True, , , True)
            End If
            oButton = Me.WZRcommunityDiary.FindControl("FinishNavigationTemplateContainerID").FindControl("BTNgoToBackUrl")
            If Not IsNothing(oButton) Then
                oButton.Visible = True
                Me.Resource.setButtonByValue(oButton, ModuleOwnerCode, True, , , True)
            End If
        End If
    End Sub
    Public Sub InitializeModuleInternalFileSelector(ByVal IdItem As String, ByVal IdLink As Long, ByVal ModuleOwnerCode As String) Implements IViewPublishIntoRepositoryFromModuleItem.InitializeModuleInternalFileSelector
        Dim links As New List(Of iCoreItemFileLink(Of Long))
        Dim IdCommunity As Integer = -1
        Select Case ModuleOwnerCode
            Case ModuleCommunityDiary.UniqueID
                Dim DiaryItemId As Long = IIf(String.IsNullOrEmpty(IdItem), 0, CLng(IdItem))
                Dim oService As New lm.Comol.Core.BaseModules.CommunityDiary.Business.ServiceCommunityDiary(CurrentContext)
                Dim permission As CoreItemPermission = oService.GetItemPermissionFromLink(IdLink)
                If (permission.AllowFilePublish) Then
                    '         links = oService.GetItemFilesToPublish(DiaryItemId, permission.AllowEdit)
                End If
                IdCommunity = oService.GetCommunityIdFromItemFileLink(IdLink)
                oService = Nothing
            Case Else
                IdCommunity = CurrentContext.UserContext.CurrentCommunityID
        End Select

        If links.Count > 0 Then
            Me.CTRLmoduleFileSelector.InitializeView(links, IdLink, IdCommunity)
        Else
            Me.CTRLmoduleFileSelector.InitializeNoPermission(IdCommunity)
        End If
    End Sub
    Public ReadOnly Property SelectedItemFileLinksId As List(Of Long) Implements IViewPublishIntoRepositoryFromModuleItem.SelectedItemFileLinksId
        Get
            Return CTRLmoduleFileSelector.SelectedItemFileLinksId
        End Get
    End Property
    Public ReadOnly Property SelectedItemFilesId As List(Of Long) Implements IViewPublishIntoRepositoryFromModuleItem.SelectedItemFilesId
        Get
            Return CTRLmoduleFileSelector.SelectedModuleFileId
        End Get
    End Property
    Public Sub UpdateSelectedFilesId(ByVal filesID As List(Of Long)) Implements IViewPublishIntoRepositoryFromModuleItem.UpdateSelectedFilesId
        Me.CTRLmoduleFileSelector.UpdateSelectedFilesId(filesID)
    End Sub
#End Region

#Region "Community Selector"
    Public Sub InitCommunitySelection() Implements IViewPublishIntoRepositoryFromModuleItem.InitializeCommunitySelector
        Me.WZRcommunityDiary.MoveTo(Me.WSTcommunity)

        Dim oService As UCServices.Services_File = UCServices.Services_File.Create
        oService.Upload = True
        oService.Moderate = True
        oService.Admin = True

        Dim oServiceBase As New ServiceBase(0, oService.Codex, oService.PermessiAssociati)
        Dim oClause As New GenericClause(Of ServiceClause)
        oClause.OperatorForNextClause = OperatorType.OrCondition
        oClause.Clause = New ServiceClause(oServiceBase, OperatorType.OrCondition)

        Me.CTRLcommunity.ServiceClauses = oClause
        Me.CTRLcommunity.InitializeControl(-1)

        Dim oNextButton As Button = Me.WZRcommunityDiary.FindControl("StepNavigationTemplateContainerID").FindControl("StepNextButton")
        oNextButton.Visible = True
        oNextButton.Enabled = Not (Me.CTRLcommunity.SelectedCommunitiesID.Count = 0)
    End Sub
    Private Sub CTRLcommunity_SelectedCommunityChanged(ByVal CommunityID As Integer) Handles CTRLcommunity.SelectedCommunityChanged
        Dim oNextButton As Button = Me.WZRcommunityDiary.FindControl("StepNavigationTemplateContainerID").FindControl("StepNextButton")
        oNextButton.Enabled = (CommunityID >= 0)
    End Sub
    Public ReadOnly Property CommunitySelectorLoaded() As Boolean Implements IViewPublishIntoRepositoryFromModuleItem.CommunitySelectorLoaded
        Get
            Return Me.CTRLcommunity.HasCommunity
        End Get
    End Property

#End Region

#Region "Folder Selector"
    Public Sub InitializeFolderSelector(ByVal IdCommunity As Integer, ByVal SelectedFolderID As Long, ByVal ShowAlsoHidden As Boolean) Implements IViewPublishIntoRepositoryFromModuleItem.InitializeFolderSelector
        Me.CTRLCommunityFolder.InitializeControl(False, IdCommunity, SelectedFolderID, ShowAlsoHidden, True)
    End Sub
    Public Sub SetFolderInfo(ByVal CommunityName As String) Implements IViewPublishIntoRepositoryFromModuleItem.SetFolderInfo
        Me.LBinfoFolder.Text = String.Format(Resource.getValue("LBinfoFolder"), CommunityName)
    End Sub
#End Region

#Region "Duplicates"
    Public Sub LoadDuplicates(ByVal files As List(Of dtoFileExist(Of Long))) Implements IViewPublishIntoRepositoryFromModuleItem.LoadDuplicates
        Me.WZRcommunityDiary.MoveTo(Me.WSTmultipleFileError)
        Me.RPTfileName.DataSource = files
        Me.RPTfileName.DataBind()
    End Sub
    Private Sub RPTfileName_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTfileName.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oDto As dtoFileExist(Of Long) = DirectCast(e.Item.DataItem, dtoFileExist(Of Long))

            Dim oLTfileID, oLTimageFile, oLTfileType As Literal
            Dim oTXBfileName As TextBox
            oLTfileID = e.Item.FindControl("LTfileID")
            oLTfileID.Text = oDto.Id.ToString
            oLTimageFile = e.Item.FindControl("LTimageFile")
            oLTimageFile.Text = "<img src='" & BaseUrl & Me.PageUtility.SystemSettings.Extension.FindIconImage(oDto.Extension) & "' alt='" & oDto.ExistFileName & "'>&nbsp;"
            oLTfileType = e.Item.FindControl("LTfileType")
            oLTfileType.Text = oDto.Extension

            oTXBfileName = e.Item.FindControl("TXBfileName")
            oTXBfileName.Text = oDto.ProposedFileName

            Dim oLBfileNameToReplace As Label
            oLBfileNameToReplace = e.Item.FindControl("LBfileNameToReplace")
            Me.Resource.setLabel(oLBfileNameToReplace)
            oLBfileNameToReplace.Text = String.Format(oLBfileNameToReplace.Text, oDto.ExistFileName)
        End If
    End Sub
    Public Function GetRenamedModuleFiles() As List(Of dtoModuleFileToPublish) Implements IViewPublishIntoRepositoryFromModuleItem.GetRenamedModuleFiles
        Dim oList As New List(Of dtoModuleFileToPublish)
        For Each oRow As RepeaterItem In Me.RPTfileName.Items
            Dim oLTfileID, oLTfileType As Literal
            Dim oTXBfileName As TextBox
            oLTfileID = oRow.FindControl("LTfileID")
            oTXBfileName = oRow.FindControl("TXBfileName")
            oLTfileType = oRow.FindControl("LTfileType")
            oList.Add(New dtoModuleFileToPublish With {.FileID = CLng(oLTfileID.Text), .FileName = oTXBfileName.Text, .Extension = oLTfileType.Text, .IsVisible = True})
        Next
        Return oList
    End Function
#End Region

#Region "Summary"
    Public Sub LoadSummary(ByVal communityName As String, ByVal folderName As String, ByVal files As List(Of dtoModuleFileToPublish)) Implements IViewPublishIntoRepositoryFromModuleItem.LoadSummary
        Me.LBsummaryDescription.Text = String.Format(Me.Resource.getValue("LBsummaryDescription.text"), folderName, communityName)
        Me.RPTfileToPublish.DataSource = files
        Me.RPTfileToPublish.DataBind()
    End Sub
    Private Sub RPTfileToPublish_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTfileToPublish.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oDto As dtoModuleFileToPublish = DirectCast(e.Item.DataItem, dtoModuleFileToPublish)

            Dim oLTfileID, oLTfileName, oLTfileNameIcon, oLTfileExtension As Literal
            oLTfileID = e.Item.FindControl("LTfileID")
            oLTfileID.Text = oDto.FileID.ToString
            oLTfileName = e.Item.FindControl("LTfileName")
            oLTfileNameIcon = e.Item.FindControl("LTfileNameIcon")
            oLTfileExtension = e.Item.FindControl("LTfileExtension")
            oLTfileNameIcon.Text = "<img src='" & BaseUrl & Me.PageUtility.SystemSettings.Extension.FindIconImage(oDto.Extension) & "' alt='" & oDto.FileName & oDto.Extension & "'>&nbsp;" & oDto.FileName & oDto.Extension
            oLTfileName.Text = oDto.FileName
            oLTfileExtension.Text = oDto.Extension
            Dim oLabel As Label
            oLabel = e.Item.FindControl("LBvisibleTo_t")
            Me.Resource.setLabel(oLabel)
            Dim oRadioButtonList As RadioButtonList
            oRadioButtonList = e.Item.FindControl("RBLvisibleTo")
            Me.Resource.setRadioButtonList(oRadioButtonList, True)
            Me.Resource.setRadioButtonList(oRadioButtonList, False)

        End If
    End Sub
    Private Function GetFilesToPublish() As List(Of dtoModuleFileToPublish)
        Dim oList As New List(Of dtoModuleFileToPublish)
        For Each oRow As RepeaterItem In Me.RPTfileToPublish.Items
            Dim oLTfileID, oLTfileName, oLTfileExtension As Literal
            Dim oDto As New dtoModuleFileToPublish
            oLTfileID = oRow.FindControl("LTfileID")
            oLTfileName = oRow.FindControl("LTfileName")
            oLTfileExtension = oRow.FindControl("LTfileExtension")
            Dim oRadioButtonList As RadioButtonList = oRow.FindControl("RBLvisibleTo")
            oDto.FileID = CLng(oLTfileID.Text)
            oDto.FileName = oLTfileName.Text
            oDto.Extension = oLTfileExtension.Text
            oDto.IsVisible = IIf(oRadioButtonList.SelectedIndex = 0, True, False)
            oList.Add(oDto)
        Next
        Return oList
    End Function
#End Region
#Region "Wizard"

#Region "Manage step "
    Public Sub ShowWizardStep(ByVal wstep As WizardStep) Implements IViewPublishIntoRepositoryFromModuleItem.ShowWizardStep
        Select Case wstep
            Case WizardStep.FileSelector
                Me.WZRcommunityDiary.MoveTo(Me.WSTfile)
            Case WizardStep.CommunitySelector
                Me.WZRcommunityDiary.MoveTo(Me.WSTcommunity)
            Case WizardStep.FolderSelector
                Me.WZRcommunityDiary.MoveTo(Me.WSTfolder)
            Case WizardStep.RenamedFileList
                Me.WZRcommunityDiary.MoveTo(Me.WSTmultipleFileError)
            Case WizardStep.ConfirmPublish
                Me.WZRcommunityDiary.MoveTo(Me.WSTcomplete)
        End Select
    End Sub

    Private Sub WZRcommunityDiary_NextButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs) Handles WZRcommunityDiary.NextButtonClick
        If Me.WZRcommunityDiary.ActiveStep Is WSTfile Then
            If Me.CTRLmoduleFileSelector.SelectedItemFileLinksId.Count > 0 Then
                Me.CurrentPresenter.LoadStep(WizardStep.CommunitySelector, True)
            Else
                Exit Sub
            End If
        ElseIf Me.WZRcommunityDiary.ActiveStep Is Me.WSTcommunity Then
            If Me.CTRLcommunity.SelectedCommunitiesID.Count = 1 Then
                CurrentPresenter.ChangeSelectedCommunity(Me.CTRLcommunity.SelectedCommunitiesID(0))
                'Me.CurrentPresenter.FindCommunityFolder(Me.CTRLcommunity.SelectedCommunitiesID(0))
            Else
                Exit Sub
            End If
        ElseIf Me.WZRcommunityDiary.ActiveStep Is Me.WSTfolder Then
            If Me.CTRLCommunityFolder.SelectedFolder > -1 Then
                Me.CurrentPresenter.TryToPublish(Me.CTRLCommunityFolder.SelectedFolder)
            Else
                Exit Sub
            End If
        ElseIf Me.WZRcommunityDiary.ActiveStep Is Me.WSTmultipleFileError Then
            Me.CurrentPresenter.LoadStepSummary()
        End If
    End Sub
    Private Sub WZRcommunityDiary_PreviousButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs) Handles WZRcommunityDiary.PreviousButtonClick
        If Me.WZRcommunityDiary.ActiveStep Is WSTfolder Then
            ShowWizardStep(WizardStep.FileSelector)
        ElseIf Me.WZRcommunityDiary.ActiveStep Is Me.WSTmultipleFileError Then
            Me.CurrentPresenter.LoadStep(WizardStep.FolderSelector, False)
            '  Me.CurrentPresenter.FindCommunityFolder()
        ElseIf Me.WZRcommunityDiary.ActiveStep Is Me.WSTcomplete Then
            Me.CurrentPresenter.GotoPreviousFromSummary()
        End If
    End Sub
    Private Sub WZRcommunityDiary_CancelButtonClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles WZRcommunityDiary.CancelButtonClick
        Me.ReturnToManagement()
    End Sub
    Private Sub WZRcommunityDiary_FinishButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs) Handles WZRcommunityDiary.FinishButtonClick
        Dim BaseFilePath As String = ""
        If Me.SystemSettings.File.Materiale.DrivePath = "" Then
            BaseFilePath = Server.MapPath(BaseUrl & Me.SystemSettings.File.Materiale.VirtualPath) & "\"
        Else
            BaseFilePath = Me.SystemSettings.File.Materiale.DrivePath & "\"
        End If
        BaseFilePath = Replace(BaseFilePath, "\\", "\")

        Me.CurrentPresenter.PublishIntoCommunityRepository(BaseFilePath, Me.SystemSettings.Presenter.DefaultFileCategoryID, GetFilesToPublish)
    End Sub
#End Region

    Public Sub ReturnToManagement() Implements IViewPublishIntoRepositoryFromModuleItem.ReturnToManagement
        If Me.DeafultBackUrl <> "" Then
            PageUtility.RedirectToUrl(DeafultBackUrl)
            'Else
            '    PageUtility.RedirectToDefault()
        End If
    End Sub
#End Region

#Region "Action"
    Public Sub NoPermission(ByVal IdCommunity As Integer, ByVal IdModule As Integer, ByVal moduleCode As String) Implements IViewPublishIntoRepositoryFromModuleItem.NoPermission
        Me.Master.ShowNoPermission = True
        Me.PageUtility.AddActionToModule(IdCommunity, IdModule, ModuleActionNoPermissionToPublish(moduleCode), Nothing, InteractionType.Generic)
    End Sub
    Public Sub NoPermissionToManagementFiles(ByVal IdCommunity As Integer, ByVal IdModule As Integer, ByVal moduleCode As String) Implements lm.Comol.Core.BaseModules.Repository.Presentation.IViewPublishIntoRepositoryFromModuleItem.NoPermissionToManagementFiles

    End Sub
    Public Sub NoPermissionToPublishFiles(ByVal IdCommunity As Integer, ByVal IdModule As Integer, ByVal moduleCode As String) Implements lm.Comol.Core.BaseModules.Repository.Presentation.IViewPublishIntoRepositoryFromModuleItem.NoPermissionToPublishFiles
        Me.MLVpublish.ActiveViewIndex = 0
        Me.LBerror.Text = Me.Resource.getValue("NoPermissionToPublishFiles")

        Me.PageUtility.AddActionToModule(IdCommunity, IdModule, ModuleActionNoPermissionToPublish(moduleCode), Nothing, InteractionType.Generic)
    End Sub

    Public Sub SendInitAction(ByVal IdCommunity As Integer, ByVal IdModule As Integer, ByVal moduleCode As String) Implements IViewPublishIntoRepositoryFromModuleItem.SendInitAction
        Me.PageUtility.AddActionToModule(IdCommunity, IdModule, ModuleActionLoadPage(moduleCode), Nothing, InteractionType.Generic)
    End Sub

    Private Function ModuleActionNoPermission(ByVal ModuleOwnerCode As String) As Integer
        Select Case ModuleOwnerCode
            Case ModuleCommunityDiary.UniqueID
                Return ModuleCommunityDiary.ActionType.NoPermission
        End Select
    End Function
    Private Function ModuleActionNoPermissionToPublish(ByVal ModuleOwnerCode As String) As Integer
        Select Case ModuleOwnerCode
            Case ModuleCommunityDiary.UniqueID
                Return ModuleCommunityDiary.ActionType.NoPermissionToPublish
        End Select
    End Function
    Private Function ModuleActionLoadPage(ByVal ModuleOwnerCode As String) As Integer
        Select Case ModuleOwnerCode
            Case ModuleCommunityDiary.UniqueID
                Return ModuleCommunityDiary.ActionType.InitPublishFileIntoCommunity
        End Select
    End Function

    Public Sub NotifyRepositoryAdd(ByVal IdCommunity As Integer, ByVal IdModule As Integer, ByVal file As BaseCommunityFile, ByVal FolderName As String) Implements IViewPublishIntoRepositoryFromModuleItem.NotifyRepositoryAdd
        Dim oSender As New RepositoryNotificationUtility(Me.PageUtility)
        oSender.NotifyFileUploaded(IIf(file.isVisible, oSender.PermissionToSee, oSender.PermissionToAdmin), IdCommunity, file.FolderId, file.Id, file.UniqueID, file.DisplayName, FolderName, file.RepositoryItemType)
        'If file.isSCORM Then
        '    oSender.NotifyScormUploaded(IIf(file.isVisible, oSender.PermissionToSee, oSender.PermissionToAdmin), IdCommunity, file.FolderId, file.Id, file.UniqueID, file.DisplayName, FolderName)
        'ElseIf file.isFile Then
        '    oSender.NotifyFileUploaded(IIf(file.isVisible, oSender.PermissionToSee, oSender.PermissionToAdmin), IdCommunity, file.FolderId, file.Id, file.DisplayName, FolderName)
        'End If
        ' Me.PageUtility.AddActionToModule(IdCommunity, IdModule, Services_DiarioLezioni.ActionType.AddFile, Me.PageUtility.CreateObjectsList(ModuleID, Services_DiarioLezioni.ObjectType.File, oItem.Id), lm.ActionDataContract.InteractionType.UserWithLearningObject)
    End Sub
#End Region

    
End Class