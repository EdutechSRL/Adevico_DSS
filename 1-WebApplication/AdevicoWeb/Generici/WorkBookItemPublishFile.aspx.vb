Imports lm.Comol.Modules.Base.Presentation
Imports COL_BusinessLogic_v2.UCServices
Imports lm.ActionDataContract
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel

Partial Public Class WorkBookItemPublishFile
    Inherits PageBase
    Implements IWKpublishFileToCommunity

#Region "Inherits"
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Presenter As WKIPublishFileToCommunity
    Private _AvaliableCategories As List(Of COL_CategoriaFile)

    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get
            Return True
        End Get
    End Property
    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As WKIPublishFileToCommunity
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New WKIPublishFileToCommunity(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Interface"
    Private _CommunitiesPermission As IList(Of WorkBookCommunityPermission)
    Public ReadOnly Property PreviousWorkBookView() As WorkBookTypeFilter Implements IWKpublishFileToCommunity.PreviousWorkBookView
        Get
            If IsNothing(Request.QueryString("View")) Then
                Return WorkBookTypeFilter.None
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of WorkBookTypeFilter).GetByString(Request.QueryString("View"), WorkBookTypeFilter.None)
            End If
        End Get
    End Property
    Public ReadOnly Property PreloadedFileID() As System.Guid Implements IWKpublishFileToCommunity.PreloadedFileID
        Get
            Dim UrlID As String = Request.QueryString("FileID")
            If Not UrlID = "" Then
                Try
                    Return New System.Guid(UrlID)
                Catch ex As Exception

                End Try
            End If
            Return System.Guid.Empty
        End Get
    End Property
    Public ReadOnly Property PreloadedItemID() As System.Guid Implements IWKpublishFileToCommunity.PreloadedItemID
        Get
            Dim UrlID As String = Request.QueryString("ItemID")
            If Not UrlID = "" Then
                Try
                    Return New System.Guid(UrlID)
                Catch ex As Exception

                End Try
            End If
            Return System.Guid.Empty
        End Get
    End Property
    Public Property SelectedFolder() As Long Implements IWKpublishFileToCommunity.SelectedFolder
        Get
            SelectedFolder = Me.CTRLCommunityFolder.SelectedFolder
        End Get
        Set(ByVal value As Long)
            Me.CTRLCommunityFolder.SelectedFolder = value
        End Set
    End Property
    Public ReadOnly Property SelectedFolderName() As String Implements IWKpublishFileToCommunity.SelectedFolderName
        Get
            Return Me.CTRLCommunityFolder.SelectedFolderName
        End Get
    End Property
    Public Property SelectedCommunityID() As Integer Implements IWKpublishFileToCommunity.SelectedCommunityID
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
    Public ReadOnly Property CommunitiesPermission() As System.Collections.Generic.IList(Of WorkBookCommunityPermission) Implements IWKpublishFileToCommunity.CommunitiesPermission
        Get
            If IsNothing(_CommunitiesPermission) Then
                _CommunitiesPermission = (From sb In ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, Services_WorkBook.Codex) _
                                          Select New WorkBookCommunityPermission() With {.ID = sb.CommunityID, .Permissions = New ModuleWorkBook(New Services_WorkBook(sb.PermissionString))}).ToList
            End If
            Return _CommunitiesPermission
        End Get
    End Property
    Public Function CommunityRepositoryPermission(ByVal CommunityID As Integer) As lm.Comol.Modules.Base.DomainModel.ModuleCommunityRepository Implements IWKpublishFileToCommunity.CommunityRepositoryPermission
        Dim oModule As ModuleCommunityRepository = Nothing

        oModule = (From sb In ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, Services_File.Codex) _
                   Where sb.CommunityID = CommunityID Select New ModuleCommunityRepository(New Services_File(sb.PermissionString))).FirstOrDefault
        If IsNothing(oModule) Then
            oModule = New ModuleCommunityRepository
        End If
        Return oModule
    End Function
    Public WriteOnly Property BackToManagement() As System.Guid Implements IWKpublishFileToCommunity.BackToManagement
        Set(ByVal value As System.Guid)
            Me.HYPbackToFileManagement.Visible = Not (value = System.Guid.Empty)
            Me.HYPbackToFileManagement.NavigateUrl = Me.BaseUrl & "Generici/WorkBookItemManagementFile.aspx?ItemID=" & value.ToString & "&View=" & Me.PreviousWorkBookView.ToString
        End Set
    End Property
    Public Property SelectedFilesID() As List(Of System.Guid) Implements IWKpublishFileToCommunity.SelectedFilesID
        Get
            If Me.CBLworkBookFile.Items.Count = 0 Then
                Return New List(Of System.Guid)
            Else
                Return (From item As ListItem In Me.CBLworkBookFile.Items Where item.Selected Select New System.Guid(item.Value)).ToList
            End If
        End Get
        Set(ByVal value As System.Collections.Generic.List(Of System.Guid))
            Me.CBLworkBookFile.SelectedIndex = -1
            For Each ItemId As System.Guid In value
                Dim oItem As ListItem = Me.CBLworkBookFile.Items.FindByValue(ItemId.ToString)
                If Not IsNothing(oItem) Then
                    oItem.Selected = True
                End If
            Next
        End Set
    End Property
    Public ReadOnly Property SelectedFiles() As List(Of GenericFilterItem(Of System.Guid, String)) Implements IWKpublishFileToCommunity.SelectedFiles
        Get
            Return (From item As ListItem In Me.CBLworkBookFile.Items Where item.Selected Select New GenericFilterItem(Of System.Guid, String) With {.Id = New System.Guid(item.Value), .Name = item.Text}).ToList
        End Get
    End Property
    Private ReadOnly Property BaseFolder() As String Implements IWKpublishFileToCommunity.BaseFolder
        Get
            Return Me.Resource.getValue("BaseFolder")
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
        MyBase.SetCulture("pg_WorkBookItemPublishFile", "Generici")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setHyperLink(HYPbackToFileManagement, True, True)

            Me.Master.ServiceTitle = .getValue("serviceTitle")
            Me.Master.ServiceNopermission = .getValue("nopermission")
            .setLabel(Me.LBfileToExport)
            .setLabel(Me.LBmultipleFileError)
            .setLabel(Me.LBselectFolder)
            .setLabel(Me.LBsummary)
            Dim oButton As Button
            oButton = Me.WZRworkBook.FindControl("StartNavigationTemplateContainerID").FindControl("StartNextButton")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If
            oButton = Me.WZRworkBook.FindControl("StartNavigationTemplateContainerID").FindControl("BTNgoToWorkBookList")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If
            oButton = Me.WZRworkBook.FindControl("StepNavigationTemplateContainerID").FindControl("BTNgoToWorkBookList")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If
            oButton = Me.WZRworkBook.FindControl("StepNavigationTemplateContainerID").FindControl("StepPreviousButton")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If
            oButton = Me.WZRworkBook.FindControl("StepNavigationTemplateContainerID").FindControl("StepNextButton")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If
            oButton = Me.WZRworkBook.FindControl("FinishNavigationTemplateContainerID").FindControl("BTNgoToWorkBookList")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If
            oButton = Me.WZRworkBook.FindControl("FinishNavigationTemplateContainerID").FindControl("FinishPreviousButton")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If
            oButton = Me.WZRworkBook.FindControl("FinishNavigationTemplateContainerID").FindControl("FinishButton")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If
        End With
    End Sub
#End Region

#Region "Notification / Action"
    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_WorkBook.Codex)
    End Sub

    'Public Sub NotifyAddFileToCommunity(ByVal CommunityID As Integer, ByVal CommunityFileID As Long, ByVal FileName As String, ByVal FolderName As String) Implements IWKpublishFileToCommunity.NotifyAddFileToCommunity
    '    Dim oSender As New RepositoryNotificationUtility(Me.PageUtility)
    '    If FolderName = "/" Then
    '        FolderName = MyBase.Resource.getValue("BaseFolder")
    '    Else
    '        FolderName = MyBase.Resource.getValue("BaseFolder") & FolderName
    '    End If

    '    '   oServiceUtility.NotifyFileUploaded(CommunityID, CommunityFileID, FileName, FolderName, PageUtility.EncryptedUrl("ElencoMateriale.download", "FileID=" & CommunityFileID, UtilityLibrary.SecretKeyUtil.EncType.Altro))
    'End Sub
    Private Sub Notify(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal oItem As CommunityFile, ByVal ParentFolder As String) Implements IWKpublishFileToCommunity.Notify
        Dim oSender As New RepositoryNotificationUtility(Me.PageUtility)


        oSender.NotifyFileUploaded(IIf(oItem.isVisible, oSender.PermissionToSee, oSender.PermissionToAdmin), CommunityID, oItem.FolderId, oItem.Id, oItem.UniqueID, oItem.DisplayName, ParentFolder, oItem.RepositoryItemType)
        'If oItem.isSCORM Then
        '    oSender.NotifyScormUploaded(IIf(oItem.isVisible, oSender.PermissionToSee, oSender.PermissionToAdmin), CommunityID, oItem.FolderId, oItem.Id, oItem.UniqueID, oItem.DisplayName, ParentFolder)
        'ElseIf oItem.isFile Then
        '    oSender.NotifyFileUploaded(IIf(oItem.isVisible, oSender.PermissionToSee, oSender.PermissionToAdmin), CommunityID, oItem.FolderId, oItem.Id, oItem.DisplayName, ParentFolder)
        'End If
        Me.PageUtility.AddActionToModule(CommunityID, ModuleID, Services_WorkBook.ActionType.UploadFile, Me.PageUtility.CreateObjectsList(ModuleID, Services_WorkBook.ObjectType.File, oItem.Id), lm.ActionDataContract.InteractionType.UserWithLearningObject)
    End Sub

#End Region


#Region "Wizard"
#Region "Manage step "
    Public Sub ShowFoldersList() Implements IWKpublishFileToCommunity.ShowFoldersList
        Me.WZRworkBook.MoveTo(Me.WSTfolder)
    End Sub
    Public Sub ShowFileList() Implements IWKpublishFileToCommunity.ShowFileList
        Me.WZRworkBook.MoveTo(Me.WSTfile)
    End Sub
    Public Sub ShowCompleteMessage() Implements lm.Comol.Modules.Base.Presentation.IWKpublishFileToCommunity.ShowCompleteMessage
        Me.WZRworkBook.MoveTo(Me.WSTcomplete)
    End Sub
    Public Sub ShowRenamedFileList() Implements lm.Comol.Modules.Base.Presentation.IWKpublishFileToCommunity.ShowRenamedFileList
        Me.WZRworkBook.MoveTo(Me.WSTmultipleFileError)
    End Sub
    Public Sub ShowSelectCommunity() Implements IWKpublishFileToCommunity.ShowSelectCommunity
        Me.WZRworkBook.MoveTo(Me.WSTcommunity)
    End Sub

    Private Sub WZRworkBook_NextButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs) Handles WZRworkBook.NextButtonClick
        If Me.WZRworkBook.ActiveStep Is WSTfile Then
            If Me.CBLworkBookFile.SelectedIndex > -1 Then
                Me.CurrentPresenter.FindCommunityFolderOrCommunity()
            Else
                Exit Sub
            End If
        ElseIf Me.WZRworkBook.ActiveStep Is Me.WSTcommunity Then
            If Me.CTRLcommunity.SelectedCommunitiesID.Count = 1 Then
                Me.CurrentPresenter.FindCommunityFolder(Me.CTRLcommunity.SelectedCommunitiesID(0))
            Else
                Exit Sub
            End If
        ElseIf Me.WZRworkBook.ActiveStep Is Me.WSTfolder Then
            If Me.CTRLCommunityFolder.SelectedFolder > -1 Then
                Me.CurrentPresenter.TryToPublish(Me.CTRLCommunityFolder.SelectedFolder)
            Else
                Exit Sub
            End If
        ElseIf Me.WZRworkBook.ActiveStep Is Me.WSTmultipleFileError Then
            Me.CurrentPresenter.GetSummary()
        End If
    End Sub
    Private Sub WZRworkBook_PreviousButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs) Handles WZRworkBook.PreviousButtonClick
        If Me.WZRworkBook.ActiveStep Is WSTfolder Then
            Me.WZRworkBook.MoveTo(Me.WSTfile)
        ElseIf Me.WZRworkBook.ActiveStep Is Me.WSTmultipleFileError Then
            Me.CurrentPresenter.FindCommunityFolder()
        ElseIf Me.WZRworkBook.ActiveStep Is Me.WSTcomplete Then
            Me.CurrentPresenter.GotoPreviousFromSummary()
        End If
    End Sub
    Private Sub WZRworkBook_CancelButtonClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles WZRworkBook.CancelButtonClick
        Me.ReturnToFileManagement(Me.PreloadedItemID)
    End Sub
    Private Sub WZRworkBook_FinishButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs) Handles WZRworkBook.FinishButtonClick
        Me.CurrentPresenter.PublishIntoCommunityRepository(Me.PageUtility.SystemSettings.BaseFileRepositoryPath.DrivePath, Me.SystemSettings.Presenter.DefaultFileCategoryID)
    End Sub
#End Region

    Public Sub InitCommunitySelection() Implements IWKpublishFileToCommunity.InitCommunitySelection
        Me.WZRworkBook.MoveTo(Me.WSTcommunity)

        Dim oService As Services_File = Services_File.Create
        oService.Upload = True
        oService.Moderate = True
        oService.Admin = True

        Dim oServiceBase As New ServiceBase(0, oService.Codex, oService.PermessiAssociati)
        Dim oClause As New GenericClause(Of ServiceClause)
        oClause.OperatorForNextClause = OperatorType.OrCondition
        oClause.Clause = New ServiceClause(oServiceBase, OperatorType.OrCondition)

        Me.CTRLcommunity.ServiceClauses = oClause
        Me.CTRLcommunity.InitializeControl(-1)

        Dim oNextButton As Button = Me.WZRworkBook.FindControl("StepNavigationTemplateContainerID").FindControl("StepNextButton")
        oNextButton.Visible = True
        oNextButton.Enabled = Not (Me.CTRLcommunity.SelectedCommunitiesID.Count = 0)
    End Sub
    Private Sub CTRLcommunity_SelectedCommunityChanged(ByVal CommunityID As Integer) Handles CTRLcommunity.SelectedCommunityChanged
        Dim oNextButton As Button = Me.WZRworkBook.FindControl("StepNavigationTemplateContainerID").FindControl("StepNextButton")
        oNextButton.Enabled = (CommunityID >= 0)
    End Sub
    Public Sub InitializeFolderSelector(ByVal CommunityID As Integer, ByVal SelectedFolderID As Long, ByVal ShowHiddenFiles As Boolean) Implements IWKpublishFileToCommunity.InitializeFolderSelector
        Me.CTRLCommunityFolder.InitializeControl(False, CommunityID, SelectedFolderID, ShowHiddenFiles, True)
    End Sub
    Public Sub LoadWorkbookFiles(ByVal oFiles As List(Of GenericFilterItem(Of System.Guid, lm.Comol.Core.DomainModel.BaseFile))) Implements IWKpublishFileToCommunity.LoadWorkbookFiles
        Me.MLVpublish.SetActiveView(Me.VIWselectFolder)

        Me.CBLworkBookFile.Items.Clear()
        For Each oFile As GenericFilterItem(Of System.Guid, lm.Comol.Core.DomainModel.BaseFile) In oFiles
            Dim NomeFile As String = "&nbsp;<img src='" & BaseUrl & Me.PageUtility.SystemSettings.Extension.FindIconImage(oFile.Name.Extension) & "'>&nbsp;" & oFile.Name.DisplayName
            Me.CBLworkBookFile.Items.Add(New ListItem(NomeFile, oFile.Id.ToString))
        Next
        Me.WZRworkBook.MoveTo(Me.WSTfile)
    End Sub

    Public Sub LoadMultipleFileName(ByVal oList As System.Collections.Generic.List(Of lm.Comol.Core.DomainModel.dtoFileExist(Of System.Guid))) Implements lm.Comol.Modules.Base.Presentation.IWKpublishFileToCommunity.LoadMultipleFileName
        Me.WZRworkBook.MoveTo(Me.WSTmultipleFileError)
        Me.RPTfileName.DataSource = oList
        Me.RPTfileName.DataBind()
    End Sub
    Private Sub RPTfileName_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTfileName.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oDto As dtoFileExist(Of System.Guid) = DirectCast(e.Item.DataItem, dtoFileExist(Of System.Guid))

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

    Public Sub LoadSummary(ByVal CommunityName As String, ByVal FolderName As String, ByVal oFiles As List(Of dtoFileToPublish)) Implements IWKpublishFileToCommunity.LoadSummary
        Me.LBsummaryDescription.Text = String.Format(Me.Resource.getValue("LBsummaryDescription.text"), FolderName, CommunityName)
        Me.RPTfileToPublish.DataSource = oFiles
        Me.RPTfileToPublish.DataBind()
    End Sub
    Private Sub RPTfileToPublish_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTfileToPublish.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oDto As dtoFileToPublish = DirectCast(e.Item.DataItem, dtoFileToPublish)

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
#End Region

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub


    Public Sub ReturnToFileManagement(ByVal ItemID As System.Guid) Implements IWKpublishFileToCommunity.ReturnToFileManagement
        Me.PageUtility.RedirectToUrl("Generici/WorkBookItemManagementFile.aspx?ItemID=" & ItemID.ToString & "&View=" & Me.PreviousWorkBookView.ToString)
    End Sub




    Public WriteOnly Property AllowCommunityPublish() As Boolean Implements IWKpublishFileToCommunity.AllowCommunityPublish
        Set(ByVal value As Boolean)

        End Set
    End Property

    Public Sub NoPermissionToAccessPage(ByVal CommunityID As Integer, ByVal ModuleID As Integer) Implements IWKpublishFileToCommunity.NoPermissionToAccessPage
        Me.BindNoPermessi()
        Me.PageUtility.AddActionToModule(CommunityID, ModuleID, Services_WorkBook.ActionType.NoPermission, Nothing, lm.ActionDataContract.InteractionType.SystemToSystem)
    End Sub

    Public Sub NoPermissionToManagementFiles() Implements IWKpublishFileToCommunity.NoPermissionToManagementFiles
        Me.MLVpublish.ActiveViewIndex = 0
        Me.LBerror.Text = Me.Resource.getValue("NoPermissionToManagementFiles")
    End Sub

    Public Sub NoPermissionToPublishFiles() Implements IWKpublishFileToCommunity.NoPermissionToPublishFiles
        Me.MLVpublish.ActiveViewIndex = 0
        Me.LBerror.Text = Me.Resource.getValue("NoPermissionToPublishFiles")
    End Sub


    Public Function GetChangedFileName() As List(Of dtoFileExist(Of System.Guid)) Implements IWKpublishFileToCommunity.GetChangedFileName
        Dim oList As New List(Of dtoFileExist(Of System.Guid))
        For Each oRow As RepeaterItem In Me.RPTfileName.Items
            Dim oLTfileID, oLTfileType As Literal
            Dim oTXBfileName As TextBox
            oLTfileID = oRow.FindControl("LTfileID")
            oTXBfileName = oRow.FindControl("TXBfileName")
            oLTfileType = oRow.FindControl("LTfileType")
            oList.Add(New dtoFileExist(Of System.Guid) With {.Id = New System.Guid(oLTfileID.Text), .ProposedFileName = oTXBfileName.Text, .Extension = oLTfileType.Text, .ExistFileName = Me.CBLworkBookFile.Items.FindByValue(oLTfileID.Text).Text})
        Next
        Return oList
    End Function
    Public Function GetFilesToPublish() As List(Of dtoFileToPublish) Implements IWKpublishFileToCommunity.GetFilesToPublish
        Dim oList As New List(Of dtoFileToPublish)
        For Each oRow As RepeaterItem In Me.RPTfileToPublish.Items
            Dim oLTfileID, oLTfileName, oLTfileExtension As Literal
            Dim oDto As New dtoFileToPublish
            oLTfileID = oRow.FindControl("LTfileID")
            oLTfileName = oRow.FindControl("LTfileName")
            oLTfileExtension = oRow.FindControl("LTfileExtension")
            Dim oRadioButtonList As RadioButtonList = oRow.FindControl("RBLvisibleTo")
            oDto.FileID = New System.Guid(oLTfileID.Text)
            oDto.FileName = oLTfileName.Text
            oDto.Extension = oLTfileExtension.Text
            oDto.IsVisible = IIf(oRadioButtonList.SelectedIndex = 0, True, False)
            oList.Add(oDto)
        Next
        Return oList
    End Function



    Public ReadOnly Property CommunitySelectionLoaded() As Boolean Implements IWKpublishFileToCommunity.CommunitySelectionLoaded
        Get
            Return Me.CTRLcommunity.HasCommunity
        End Get
    End Property
End Class