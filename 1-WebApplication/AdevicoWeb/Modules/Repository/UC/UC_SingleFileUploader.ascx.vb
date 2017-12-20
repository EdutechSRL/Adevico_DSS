Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.UI.Presentation
Imports System.Globalization
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.File

Partial Public Class UC_SingleFileUploader
    Inherits BaseControlWithLoad
    Implements IviewSingleFileUploader


    Public Event changeItemPermission(ByVal forAll As Boolean, ByVal ForCreate As ItemRepositoryToCreate)
#Region "IVIEW"
    Private _Presenter As SingleFileUploaderPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Private ReadOnly Property CurrentPresenter() As SingleFileUploaderPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New SingleFileUploaderPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
    Private ReadOnly Property Portalname() As String Implements IviewSingleFileUploader.Portalname
        Get
            Return Me.Resource.getValue("Portalname")
        End Get
    End Property
    Private ReadOnly Property BaseFolder() As String Implements IviewSingleFileUploader.BaseFolder
        Get
            Return Me.Resource.getValue("BaseFolder")
        End Get
    End Property
    Private Property RepositoryCommunityID() As Integer Implements IviewSingleFileUploader.RepositoryCommunityID
        Get
            Return CInt(Me.ViewState("RepositoryCommunityID"))
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("RepositoryCommunityID") = value
        End Set
    End Property
    Private Property FolderID() As Long Implements IviewSingleFileUploader.FolderID
        Get
            Return Me.CTRLCommunityFolder.SelectedFolder
        End Get
        Set(ByVal value As Long)
            Me.CTRLCommunityFolder.SelectedFolder = value
            Me.UDPselectFolder.Update()
        End Set
    End Property
    Private ReadOnly Property ForCreate() As ItemRepositoryToCreate Implements IviewSingleFileUploader.ForCreate
        Get
            If TypeOf Me.ViewState("ForCreate") Is ItemRepositoryToCreate Then
                Return Me.ViewState("ForCreate")
            Else
                Me.ViewState("ForCreate") = ItemRepositoryToCreate.File
                Me.DVfile.Visible = True
                Me.DVfolder.Visible = False
                Return ItemRepositoryToCreate.File
            End If
        End Get
    End Property
    Private WriteOnly Property AllowFolderChange() As Boolean Implements IviewSingleFileUploader.AllowFolderChange
        Set(ByVal value As Boolean)
            Me.BTNeditPath.Enabled = value
        End Set
    End Property
    Private WriteOnly Property CommunityName() As String Implements IviewSingleFileUploader.CommunityName
        Set(ByVal value As String)
            Me.LBcommunity.Text = value
        End Set
    End Property
    Private Property Description() As String Implements IviewSingleFileUploader.Description
        Get
            Return Me.TXBdescription.Text
        End Get
        Set(ByVal value As String)
            Me.TXBdescription.Text = value
        End Set
    End Property
    Public Property DownlodableByCommunity() As Boolean Implements IviewSingleFileUploader.DownlodableByCommunity
        Get
            If Me.RBLallowTo.SelectedIndex = -1 Then
                Return True
            Else
                Return (Me.RBLallowTo.SelectedValue = True)
            End If
        End Get
        Set(ByVal value As Boolean)
            Me.RBLallowTo.SelectedValue = value
        End Set
    End Property
    Private Property ItemType() As Repository.RepositoryItemType Implements IviewSingleFileUploader.ItemType
        Get
            Return Me.RBLtype.SelectedValue
        End Get
        Set(ByVal value As Repository.RepositoryItemType)
            Me.RBLtype.SelectedValue = value
            Me.DVtypeDownload.Visible = Not (value = Repository.RepositoryItemType.FileStandard OrElse value = Repository.RepositoryItemType.Folder)
            Me.UDPtype.Update()
        End Set
    End Property
    Private WriteOnly Property FilePath() As String Implements IviewSingleFileUploader.FilePath
        Set(ByVal value As String)
            Me.LBpath.Text = value
        End Set
    End Property
    Private Property FolderName() As String Implements IviewSingleFileUploader.FolderName
        Get
            Return Me.TXBfolderName.Text
        End Get
        Set(ByVal value As String)
            Me.TXBfolderName.Text = value
        End Set
    End Property
    Private Property CommunityPermission() As ModuleCommunityRepository
        Get
            If TypeOf Me.ViewState("ModuleCommunityRepository") Is ModuleCommunityRepository Then
                Return Me.ViewState("ModuleCommunityRepository")
            Else
                Return New ModuleCommunityRepository
            End If
        End Get
        Set(ByVal value As ModuleCommunityRepository)
            Me.ViewState("ModuleCommunityRepository") = value
        End Set
    End Property
    Public Property AllowUpload() As Boolean Implements IviewSingleFileUploader.AllowUpload
        Get
            Return Me.CBXallowUpload.Checked
        End Get
        Set(ByVal value As Boolean)
            Me.CBXallowUpload.Checked = value
        End Set
    End Property
    Public Property AllowDownload() As Boolean Implements IviewSingleFileUploader.AllowDownload
        Get
            Return Me.RBLplay.SelectedValue
        End Get
        Set(ByVal value As Boolean)
            Me.RBLplay.SelectedValue = value
            Me.UDPtype.Update()
        End Set
    End Property
    Public Property VisibleToDonwloaders() As Boolean Implements IviewSingleFileUploader.VisibleToDonwloaders
        Get
            If Me.RBLvisibleTo.SelectedIndex = -1 Then
                Return True
            Else
                Return (Me.RBLvisibleTo.SelectedValue = True)
            End If
        End Get
        Set(ByVal value As Boolean)
            Me.RBLvisibleTo.SelectedValue = value
        End Set
    End Property
#End Region
    Public Property AjaxEnabled() As Boolean
        Get
            If TypeOf Me.ViewState("AjaxEnabled") Is Boolean Then
                Return Me.ViewState("AjaxEnabled")
            Else
                Me.ViewState("AjaxEnabled") = False
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AjaxEnabled") = value
        End Set
    End Property
    Public Property UpdatePermissionButton() As Boolean
        Get
            If TypeOf Me.ViewState("UpdatePermissionButton") Is Boolean Then
                Return Me.ViewState("UpdatePermissionButton")
            Else
                Me.ViewState("UpdatePermissionButton") = True
                Return True
            End If
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("UpdatePermissionButton") = value
            Me.RBLallowTo.AutoPostBack = value
        End Set
    End Property
#Region "Default"
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim oProgress As Telerik.Web.UI.RadProgressContext = Telerik.Web.UI.RadProgressContext.Current
        If Not IsNothing(oProgress) Then
            oProgress.SecondaryTotal = ""
            oProgress.SecondaryValue = ""
            oProgress.SecondaryPercent = ""
        End If
    End Sub

#Region "Default"
    Public Overrides Sub BindDati()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("UC_CommunityFile", "Generici", "UC_File")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(Me.LBcommunity_t)
            .setLabel(Me.LBdescription_t)
            .setLabel(Me.LBname_t)
            .setLabel(Me.LBpath_t)
            .setLabel(Me.LBtype_t)
            .setLabel(Me.LBvisibleTo_t)
            .setButton(BTNeditPath, True, , , True)
            .setRadioButtonList(Me.RBLallowTo, True)
            .setRadioButtonList(Me.RBLallowTo, False)
            .setRadioButtonList(Me.RBLvisibleTo, True)
            .setRadioButtonList(Me.RBLvisibleTo, False)

            Dim availableTypes As List(Of Repository.RepositoryItemType) = (From t In Me.SystemSettings.Presenter.RepositoryConfiguration.AvailableItemType Select DirectCast(t, Repository.RepositoryItemType)).ToList
            Me.RBLtype.Items.Clear()
            For Each type As Repository.RepositoryItemType In (From a In availableTypes Where a <> Repository.RepositoryItemType.Folder).ToList
                Me.RBLtype.Items.Add(New ListItem(Resource.getValue("RepositoryItemType." & type.ToString), type))
            Next
            If Me.RBLtype.Items.Count = 0 Then
                Me.RBLtype.Items.Add(New ListItem(Resource.getValue("RepositoryItemType." & Repository.RepositoryItemType.FileStandard.ToString), Repository.RepositoryItemType.FileStandard))
            End If
            Me.RBLtype.SelectedIndex = 0

            Me.PRAcommunityFileUpload.Localization.Cancel = .getValue("Localization.Cancel")
            Me.PRAcommunityFileUpload.Localization.ElapsedTime = .getValue("Localization.ElapsedTime")
            Me.PRAcommunityFileUpload.Localization.EstimatedTime = .getValue("Localization.EstimatedTime")
            Me.PRAcommunityFileUpload.Localization.Total = .getValue("Localization.Total")
            Me.PRAcommunityFileUpload.Localization.TotalFiles = .getValue("Localization.TotalFiles")
            Me.PRAcommunityFileUpload.Localization.TransferSpeed = .getValue("Localization.TransferSpeed")
            Me.PRAcommunityFileUpload.Localization.Uploaded = .getValue("Localization.Uploaded")
            Me.PRAcommunityFileUpload.Localization.UploadedFiles = .getValue("Localization.UploadedFiles")

            Dim newCulture As CultureInfo = CultureInfo.CreateSpecificCulture(Me.LinguaCode)
            Me.PRAcommunityFileUpload.Culture = newCulture

            .setLabel(Me.LBadvancedInfo)
            .setLabel(Me.LBplay)
            .setRadioButtonList(Me.RBLplay, True)
            .setRadioButtonList(Me.RBLplay, False)
            ' .setLabel(Me.LBallowTo)
            .setLabel(Me.LBpermissionInfo_t)
            .setCheckBox(CBXallowUpload)
        End With
    End Sub
#End Region

#Region "Control Methods"
    Private Sub CTRLCommunityFolder_AjaxFolderSelected(ByVal sender As Object, ByVal e As System.EventArgs) Handles CTRLCommunityFolder.AjaxFolderSelected
        Me.LBpath.Text = Me.CTRLCommunityFolder.SelectedFolderPathName
        Me.CloseDialog("selectFolder")
        Me.UDPdata.Update()
    End Sub

    Private Sub CloseDialog(ByVal dialogId As String)
        Dim script As String = String.Format("closeDialog('{0}')", dialogId)
        ScriptManager.RegisterClientScriptBlock(MyBase.Page, Me.GetType, Me.UniqueID, script, True)
    End Sub
    Private Sub OpenDialog(ByVal dialogId As String)
        Dim script As String = String.Format("showDialog('{0}')", dialogId)
        ScriptManager.RegisterStartupScript(MyBase.Page, Me.GetType, Me.UniqueID, script, True)
    End Sub
    Public Function AddFileToCommunityRepository() As MultipleUploadResult(Of dtoUploadedFile)
        Dim oResult As New MultipleUploadResult(Of dtoUploadedFile)
        If Me.CommunityPermission.Administration OrElse Me.CommunityPermission.UploadFile OrElse Me.CurrentPresenter.HasPermissionToUploadIntoFolder(Me.FolderID, Me.CommunityPermission) Then
            Dim oTelerikFile As Telerik.Web.UI.UploadedFile = Telerik.Web.UI.RadUploadContext.Current.UploadedFiles(TXBFile.UniqueID)
            If Not IsNothing(oTelerikFile) Then
                Me.AddFile(oResult, Telerik.Web.UI.RadUploadContext.Current.UploadedFiles(TXBFile.UniqueID))
            End If
        Else
            For Each oTelerikFile As Telerik.Web.UI.UploadedFile In Telerik.Web.UI.RadUploadContext.Current.UploadedFiles
                oResult.NotuploadedFile.Add(New dtoUploadedFile(CreateFromTelerik(oTelerikFile), ItemRepositoryStatus.NoPermissionToAdd))
            Next
        End If

        If AjaxEnabled AndAlso oResult.NotuploadedFile.Count > 0 Then
            Me.LBerrorNotification.Text = Me.Resource.getValue("ItemRepositoryStatus." & oResult.NotuploadedFile(0).Status.ToString)
            Me.OpenDialog("uploadError")
            Me.UDPerrors.Update()
        Else
            Me.LTscript.Text = ""
        End If
        Return oResult
    End Function
    Public Function AddFolderToCommunityRepository() As MultipleUploadResult(Of dtoUploadedFile)
        Dim oResult As New MultipleUploadResult(Of dtoUploadedFile)

        If Me.CommunityPermission.Administration OrElse Me.CommunityPermission.UploadFile Then
            Me.AddFolder(oResult)
        Else
            Dim oFolder As CommunityFile = Me.CreateFolder()
            oResult.NotuploadedFile.Add(New dtoUploadedFile(oFolder, ItemRepositoryStatus.NoPermissionToAdd))
        End If
        If AjaxEnabled AndAlso oResult.NotuploadedFile.Count > 0 Then
            Me.LBerrorNotification.Text = Me.Resource.getValue("ItemRepositoryStatus." & oResult.NotuploadedFile(0).Status.ToString)
            Me.OpenDialog("uploadError")
            Me.UDPerrors.Update()
        Else
            Me.LTscript.Text = ""
        End If
        Return oResult
    End Function

    Private Function AddFile(ByVal oResult As MultipleUploadResult(Of dtoUploadedFile), ByVal oUploadedFile As Telerik.Web.UI.UploadedFile) As dtoUploadedFile
        Dim oResponse As dtoUploadedFile
        Dim oCommunityFile As CommunityFile = Me.CreateFromTelerik(oUploadedFile)
        Dim oImpersonate As New lm.Comol.Core.File.Impersonate
        Try
            Dim CommunityPath As String = ""
            Dim oServiceUtility As New FileNotificationUtility(Me.PageUtility)
            If Me.SystemSettings.File.Materiale.DrivePath = "" Then
                CommunityPath = Server.MapPath(BaseUrl & Me.SystemSettings.File.Materiale.VirtualPath) & "\" & Me.RepositoryCommunityID
            Else
                CommunityPath = Me.SystemSettings.File.Materiale.DrivePath & "\" & Me.RepositoryCommunityID
            End If
            CommunityPath = Replace(CommunityPath, "\\", "\")

            If oImpersonate.ImpersonateValidUser() = ItemRepositoryStatus.ImpersonationFailed Then
                oResponse = New dtoUploadedFile(oCommunityFile, ItemRepositoryStatus.ImpersonationFailed)
            Else
                Create.Directory(CommunityPath)
                Dim SavedFile As String = CommunityPath & "\" & oCommunityFile.UniqueID.ToString & ".stored"
                oUploadedFile.SaveAs(SavedFile, True)
                oResponse = Me.CurrentPresenter.AddFileToRepository(oCommunityFile, SavedFile, CommunityPath)
                oResponse.SavedFilePath = SavedFile
                If oResponse.Status <> ItemRepositoryStatus.FileUploaded AndAlso oResponse.Status <> ItemRepositoryStatus.FileExist Then
                    Delete.File_FM(SavedFile)
                Else
                    Notify(oResponse)
                End If
            End If
            oImpersonate.UndoImpersonation()


        Catch ex As Exception
            oImpersonate.UndoImpersonation()
            oResponse = New dtoUploadedFile(oCommunityFile, ItemRepositoryStatus.UploadError)
        End Try
        If Not IsNothing(oResponse) AndAlso oResponse.Status = ItemRepositoryStatus.FileUploaded Then
            oResult.UploadedFile.Add(oResponse)
        Else
            oResult.NotuploadedFile.Add(oResponse)
        End If
        Return oResponse
    End Function
    Private Sub AddFolder(ByVal oResult As MultipleUploadResult(Of dtoUploadedFile))
        Dim oFolder As CommunityFile = Me.CreateFolder()
        Dim oResponse As dtoUploadedFile
        Try
            Dim CommunityPath As String = ""
            Dim oServiceUtility As New FileNotificationUtility(Me.PageUtility)

            oResponse = Me.CurrentPresenter.AddFolderToRepository(oFolder)
            Notify(oResponse)

        Catch ex As Exception

        End Try
        If Not IsNothing(oResponse) AndAlso oResponse.Status = ItemRepositoryStatus.FolderCreated Then
            oResult.UploadedFile.Add(oResponse)
        Else
            oResult.NotuploadedFile.Add(oResponse)
        End If
    End Sub
    Private Function CreateFromTelerik(ByVal oTelerikFile As Telerik.Web.UI.UploadedFile) As CommunityFile
        Dim oCommunityFile As New CommunityFile
        With oCommunityFile
            .CloneID = 0
            .ContentType = oTelerikFile.ContentType
            .Description = Me.Description
            .DisplayOrder = 1
            .Downloads = 0
            .FileCategoryID = Me.SystemSettings.Presenter.DefaultFileCategoryID
            .FolderId = Me.FolderID
            .isDeleted = False
            .isPersonal = False
            .isSCORM = False
            .isVirtual = False
            .isVideocast = False
            .RepositoryItemType = Me.ItemType
            Select Case Me.ItemType
                Case Repository.RepositoryItemType.ScormPackage
                    .isSCORM = True
                Case Repository.RepositoryItemType.VideoStreaming
                    .isVideocast = True
            End Select

            .IsDownloadable = Me.RBLplay.SelectedValue
            .isVisible = Me.VisibleToDonwloaders
            .isFile = True
            .Level = 0
            .Name = oTelerikFile.GetNameWithoutExtension
            .Size = oTelerikFile.ContentLength
            .UniqueID = System.Guid.NewGuid
            .Extension = oTelerikFile.GetExtension
            If Not String.IsNullOrEmpty(.Extension) Then
                .Extension = .Extension.ToLower
            End If
        End With
        Return oCommunityFile
    End Function
    Private Function CreateFolder() As CommunityFile
        Dim oFolder As New CommunityFile
        With oFolder
            .CloneID = 0
            .ContentType = ""
            .Description = Me.Description
            .DisplayOrder = 1
            .Downloads = 0
            .FileCategoryID = Me.SystemSettings.Presenter.DefaultFolderCategoryID
            .FolderId = Me.FolderID
            .isDeleted = False
            .isPersonal = False
            .isSCORM = False
            .isVirtual = False
            .isVideocast = False
            .isVisible = Me.VisibleToDonwloaders
            .isFile = False
            .Level = 0
            .Name = Me.FolderName
            .Size = 0
            .UniqueID = System.Guid.NewGuid
            .Extension = ""
            .RepositoryItemType = Repository.RepositoryItemType.Folder
        End With
        Return oFolder
    End Function

    Private Sub Notify(ByVal oItem As dtoUploadedFile)
        If oItem.Status = ItemRepositoryStatus.FileUploaded OrElse oItem.Status = ItemRepositoryStatus.FolderCreated Then
            Dim oSender As New RepositoryNotificationUtility(Me.PageUtility)
            Dim ParentFolder As String = Me.BaseFolder

            If oItem.File.FolderId > 0 Then
                ParentFolder = Me.CurrentPresenter.GetFolderName(oItem.File.FolderId)
            End If

            If oItem.File.isFile = False Then
                If oItem.File.isVisible Then
                    oSender.NotifyFolderCreated(oSender.PermissionToAdmin, Me.RepositoryCommunityID, oItem.File.FolderId, oItem.File.Id, oItem.File.DisplayName, ParentFolder)
                Else
                    oSender.NotifyFolderCreated(Me.RepositoryCommunityID, oItem.File.FolderId, oItem.File.Id, oItem.File.DisplayName, ParentFolder)
                End If
            Else
                oSender.NotifyFileUploaded(IIf(oItem.File.isVisible, oSender.PermissionToSee, oSender.PermissionToAdmin), Me.RepositoryCommunityID, oItem.File.FolderId, oItem.File.Id, oItem.File.UniqueID, oItem.File.DisplayName, ParentFolder, oItem.File.RepositoryItemType)
                'Else
                'oSender.NotifyFileUploaded(IIf(oItem.File.isVisible, oSender.PermissionToSee, oSender.PermissionToAdmin), Me.RepositoryCommunityID, oItem.File.FolderId, oItem.File.Id, oItem.File.DisplayName, ParentFolder)
            End If
        End If
    End Sub

#End Region

    Public Sub InitializeControl(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal ForCreate As ItemRepositoryToCreate, ByVal oPermission As ModuleCommunityRepository) Implements IviewSingleFileUploader.InitializeControl
        Me.DVfile.Visible = (ForCreate = ItemRepositoryToCreate.File)
        Me.DVfolder.Visible = (ForCreate = ItemRepositoryToCreate.Folder)
        Me.DVtype.Visible = (ForCreate = ItemRepositoryToCreate.File)
        Me.ViewState("ForCreate") = ForCreate
        If ForCreate = ItemRepositoryToCreate.Folder Then
            Me.PRAcommunityFileUpload.Enabled = False
            Me.LBuploadInfo.Text = Me.Resource.getValue("folderInfoAdd")
            Me.TXBfolderName.Text = Me.Resource.getValue("NewFolderName")
        Else
            Me.PRAcommunityFileUpload.Enabled = True
            Me.LBuploadInfo.Text = Me.Resource.getValue("fileInfoAdd")
            Dim oProgress As Telerik.Web.UI.RadProgressContext = Telerik.Web.UI.RadProgressContext.Current
            If Not IsNothing(oProgress) Then
                oProgress.PrimaryPercent = 0
                oProgress.SecondaryValue = ""
                oProgress.SecondaryPercent = ""
            End If
        End If
        Me.CurrentPresenter.InitView(FolderID, CommunityID, ForCreate, oPermission)
        CommunityPermission = oPermission
        DVpermissionDownload.Visible = (oPermission.Administration OrElse oPermission.UploadFile)
        DVpermissionUpload.Visible = (ForCreate = ItemRepositoryToCreate.Folder)
        Me.DVadvancedContainer.Attributes.Add("class", IIf((ForCreate = ItemRepositoryToCreate.File), "AdvancedContainer h120", "AdvancedContainer h150"))
        Resource.setLabel_To_Value(LBallowTo, "LBallowTo." & (ForCreate = ItemRepositoryToCreate.File))
        Resource.setRadioButtonList(Me.RBLvisibleTo, IIf(ForCreate = ItemRepositoryToCreate.File, "File.True", "Folder.True"))
    End Sub

    Public Sub LoadFolderSelector(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean) Implements IviewSingleFileUploader.LoadFolderSelector
        Me.CTRLCommunityFolder.InitializeControl(False, CommunityID, FolderID, ShowHiddenItems, AdminPurpose)
        Me.UDPselectFolder.Update()
    End Sub

    Private Sub RBLallowTo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLallowTo.SelectedIndexChanged
        RaiseEvent changeItemPermission(Me.RBLallowTo.SelectedValue, Me.ForCreate)
    End Sub

    Private Sub RBLtype_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLtype.SelectedIndexChanged
        Dim type As Repository.RepositoryItemType = Me.ItemType
        Me.DVtypeDownload.Visible = (type <> Repository.RepositoryItemType.FileStandard)
        If type <> Repository.RepositoryItemType.FileStandard Then
            Me.RBLplay.SelectedValue = SystemSettings.Presenter.RepositoryConfiguration.DefaultDownload.Contains(CInt(type))
            Me.RBLplay.Enabled = SystemSettings.Presenter.RepositoryConfiguration.AllowDownload.Contains(CInt(type))
        Else
            Me.RBLplay.SelectedValue = True
        End If
        Me.UDPtype.Update()
    End Sub

End Class