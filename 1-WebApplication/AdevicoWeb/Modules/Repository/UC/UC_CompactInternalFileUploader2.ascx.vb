Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.UI.Presentation
Imports System.Globalization
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.File

Public Class UC_CompactInternalFileUploader2
    Inherits BaseControl
    Implements IviewModuleSingleFileUploader


    Public Event AddedModuleObjects(ByVal items As List(Of ModuleActionLink))

#Region "IVIEW"
    Private _Presenter As ModuleSingleFileUploaderPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Private ReadOnly Property CurrentPresenter() As ModuleSingleFileUploaderPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ModuleSingleFileUploaderPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
    Private Property RepositoryCommunityID() As Integer Implements IviewModuleSingleFileUploader.RepositoryCommunityID
        Get
            Return CInt(Me.ViewState("RepositoryCommunityID"))
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("RepositoryCommunityID") = value
        End Set
    End Property
    Public Property AllowDownload() As Boolean Implements IviewModuleSingleFileUploader.AllowDownload
        Get
            Return Me.RBLplay.SelectedValue
        End Get
        Set(ByVal value As Boolean)
            Me.RBLplay.SelectedValue = value
            Me.UDPtype.Update()
        End Set
    End Property

    'Public Property AdditionalUploadPath As String
    '    Get
    '        Return ViewStateOrDefault("AddUploadPath", "")
    '    End Get
    '    Set(value As String)
    '        ViewState("AddUploadPath") = value
    '    End Set
    'End Property

    Private Property ItemType() As Repository.RepositoryItemType Implements IviewModuleSingleFileUploader.ItemType
        Get
            Return Me.RBLtype.SelectedValue
        End Get
        Set(ByVal value As Repository.RepositoryItemType)
            Me.RBLtype.SelectedValue = value
            Me.LBplay.Visible = Not (value = Repository.RepositoryItemType.FileStandard OrElse value = Repository.RepositoryItemType.Folder)
            Me.RBLplay.Visible = Not (value = Repository.RepositoryItemType.FileStandard OrElse value = Repository.RepositoryItemType.Folder)
            Me.LTplayClosed.Visible = Not (value = Repository.RepositoryItemType.FileStandard OrElse value = Repository.RepositoryItemType.Folder)
            Me.UDPtype.Update()
        End Set
    End Property
    Private Property AllowAnonymousUpload As Boolean Implements IviewModuleSingleFileUploader.AllowAnonymousUpload
        Get
            Return ViewStateOrDefault("AllowAnonymousUpload", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowAnonymousUpload") = value
        End Set
    End Property
#End Region
    Public Property Enabled() As Boolean
        Get
            Return ViewStateOrDefault("Enabled", False)
        End Get
        Set(ByVal value As Boolean)
            Me.RadAsyncUpload.Enabled = value
            'Me.TXBFile.Disabled = Not value
            Me.ViewState("Enabled") = value
        End Set
    End Property
    'Public Property AjaxEnabled() As Boolean
    '    Get
    '        Return ViewStateOrDefault("AjaxEnabled", False)
    '    End Get
    '    Set(ByVal value As Boolean)
    '        Me.ViewState("AjaxEnabled") = value
    '    End Set
    'End Property
    Public Property ViewTypeSelector() As Boolean
        Get
            Return Me.UDPtype.Visible
        End Get
        Set(ByVal value As Boolean)
            Me.UDPtype.Visible = value
            Me.UDPtype.Update()
        End Set
    End Property

    Public Property ButtonToLock() As String
        Get
            Return ViewStateOrDefault("ButtonToLock", "")
        End Get
        Set(ByVal value As String)
            Me.ViewState("ButtonToLock") = value
        End Set
    End Property


#Region "Default"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
    Private _BaseUrl As String
    Private ReadOnly Property BaseUrl As String
        Get
            If String.IsNullOrEmpty(_BaseUrl) Then
                _BaseUrl = Me.PageUtility.BaseUrl
            End If
            Return _BaseUrl
        End Get
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Default"

    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("UC_CommunityFile", "Generici", "UC_File")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(Me.LBtype_t)


            .setLabel(Me.LBplay)
            .setRadioButtonList(Me.RBLplay, True)
            .setRadioButtonList(Me.RBLplay, False)
        End With
    End Sub
#End Region

#Region "Control Methods"
    Private Sub CloseDialog(ByVal dialogId As String)
        Dim script As String = String.Format("closeDialog('{0}')", dialogId)
        ScriptManager.RegisterClientScriptBlock(MyBase.Page, Me.GetType, Me.UniqueID, script, True)
    End Sub
    Private Sub OpenDialog(ByVal dialogId As String)

    End Sub
    Private Function AddModuleInternalFile(ByVal FileType As lm.Comol.Core.DomainModel.FileRepositoryType, ByVal ObjectOwner As Object, ByVal ServiceOwner As String, ByVal ServiceOwnerActionID As Integer, ByVal ObjectTypeID As Integer) As MultipleUploadResult(Of dtoModuleUploadedFile) Implements lm.Comol.Modules.Base.Presentation.IviewModuleSingleFileUploader.AddModuleInternalFile
        Dim oResult As New MultipleUploadResult(Of dtoModuleUploadedFile)
        'Dim oTelerikFile As Telerik.Web.UI.UploadedFile = Telerik.Web.UI.RadUploadContext.Current.UploadedFiles(TXBFile.UniqueID)

        'If (RadAsyncUpload.UploadedFiles.Count() > 0) Then
        Dim oTelerikFile As Telerik.Web.UI.UploadedFile = RadAsyncUpload.UploadedFiles.Item(0)
        'End If

        If Not IsNothing(oTelerikFile) Then
            Me.AddFile(oResult, oTelerikFile, FileType, ObjectOwner, ServiceOwner, ServiceOwnerActionID, ObjectTypeID)
        End If
        Return oResult
    End Function

    Public Function UploadAndLinkInternalFile(ByVal FileType As lm.Comol.Core.DomainModel.FileRepositoryType, ByVal ObjectOwner As Object, ByVal ServiceOwner As String, ByVal ServiceOwnerActionID As Integer, ByVal ObjectTypeID As Integer) As ModuleActionLink Implements IviewModuleSingleFileUploader.UploadAndLinkInternalFile
        Dim InternalFile As New MultipleUploadResult(Of dtoModuleUploadedFile)

        'Dim oTelerikFile As Telerik.Web.UI.UploadedFile = Telerik.Web.UI.RadUploadContext.Current.UploadedFiles(TXBFile.UniqueID)

        If RadAsyncUpload.UploadedFiles.Count() <= 0 Then
            Return Nothing
        End If

        Dim oTelerikFile As Telerik.Web.UI.UploadedFile = RadAsyncUpload.UploadedFiles.Item(0)

        If Not IsNothing(oTelerikFile) Then
            Me.AddFile(InternalFile, oTelerikFile, FileType, ObjectOwner, ServiceOwner, ServiceOwnerActionID, ObjectTypeID)
        End If

        If IsNothing(InternalFile) OrElse InternalFile.UploadedFile.Count = 0 Then
            Return Nothing
        Else
            Return Me.CurrentPresenter.GetModuleInternalFileLink(InternalFile.UploadedFile(0).File)
        End If
    End Function
    Private Function AddFile(ByVal oResult As MultipleUploadResult(Of dtoModuleUploadedFile), ByVal oUploadedFile As Telerik.Web.UI.UploadedFile, ByVal FileType As lm.Comol.Core.DomainModel.FileRepositoryType, ByVal ObjectOwner As Object, ByVal ServiceOwner As String, ByVal ServiceOwnerActionID As Integer, ByVal ObjectTypeID As Integer) As dtoModuleUploadedFile
        Dim oResponse As dtoModuleUploadedFile
        Dim oBaseFile As ModuleInternalFile = Me.CreateFromTelerik(oUploadedFile)
        Dim oImpersonate As New lm.Comol.Core.File.Impersonate
        Try
            Dim CommunityPath As String = ""
            If Me.SystemSettings.File.Materiale.DrivePath = "" Then
                CommunityPath = Server.MapPath(BaseUrl & Me.SystemSettings.File.Materiale.VirtualPath) & "\" & Me.RepositoryCommunityID
            Else
                CommunityPath = Me.SystemSettings.File.Materiale.DrivePath & "\" & Me.RepositoryCommunityID
            End If
            CommunityPath = Replace(CommunityPath, "\\", "\")

            If oImpersonate.ImpersonateValidUser() = ItemRepositoryStatus.ImpersonationFailed Then
                oResponse = New dtoModuleUploadedFile(oBaseFile, ItemRepositoryStatus.ImpersonationFailed)
            Else
                Create.Directory(CommunityPath)

                Dim SavedFile As String = CommunityPath & "\" & oBaseFile.UniqueID.ToString & ".stored"
                oUploadedFile.SaveAs(SavedFile, True)
                oResponse = Me.CurrentPresenter.AddModuleInternalFile(FileType, oBaseFile, SavedFile, CommunityPath, ObjectOwner, ServiceOwner, ServiceOwnerActionID, ObjectTypeID)

                oResponse.SavedFilePath = SavedFile
                If oResponse.Status <> ItemRepositoryStatus.FileUploaded AndAlso oResponse.Status <> ItemRepositoryStatus.FileExist Then
                    Delete.File_FM(SavedFile)
                End If
            End If
            oImpersonate.UndoImpersonation()
        Catch ex As Exception
            oImpersonate.UndoImpersonation()
            oResponse = New dtoModuleUploadedFile(oBaseFile, ItemRepositoryStatus.UploadError)
        End Try
        If Not IsNothing(oResponse) AndAlso oResponse.Status = ItemRepositoryStatus.FileUploaded Then
            oResult.UploadedFile.Add(oResponse)
        Else
            oResult.NotuploadedFile.Add(oResponse)
        End If
        Return oResponse
    End Function

    Private Function CreateFromTelerik(ByVal oTelerikFile As Telerik.Web.UI.UploadedFile) As ModuleInternalFile
        Dim oBaseFile As New ModuleInternalFile
        With oBaseFile
            .CloneID = 0
            .ContentType = oTelerikFile.ContentType
            .Description = ""
            .Downloads = 0
            .FileCategoryID = Me.SystemSettings.Presenter.DefaultFileCategoryID
            .FolderId = 0
            .isDeleted = False
            .isPersonal = False
            .isSCORM = False
            .isVirtual = False
            .isVideocast = False
            .RepositoryItemType = Me.ItemType

            Select Case .RepositoryItemType
                Case Repository.RepositoryItemType.ScormPackage
                    .isSCORM = True
                Case Repository.RepositoryItemType.VideoStreaming
                    .isVideocast = True
            End Select

            .IsDownloadable = Me.RBLplay.SelectedValue
            .isVisible = True
            .isFile = True
            .Name = oTelerikFile.GetNameWithoutExtension
            .Size = oTelerikFile.ContentLength
            .UniqueID = System.Guid.NewGuid
            .Extension = oTelerikFile.GetExtension
            .IsInternal = True
            If Not String.IsNullOrEmpty(.Extension) Then
                .Extension = .Extension.ToLower
            End If
        End With
        Return oBaseFile
    End Function
#End Region



    Public Sub InitializeControl(
                                ByVal idCommunity As Integer,
                                ByVal anonymousUpload As Boolean) _
            Implements IviewModuleSingleFileUploader.InitializeControl



        AllowAnonymousUpload = anonymousUpload

        Dim availableTypes As List(Of Repository.RepositoryItemType) = (From t In Me.SystemSettings.Presenter.RepositoryConfiguration.AvailableItemType Select DirectCast(t, Repository.RepositoryItemType)).ToList


        RBLtype.Items.Clear()
        For Each type As Repository.RepositoryItemType In (From a In availableTypes Where a <> Repository.RepositoryItemType.Folder).ToList
            RBLtype.Items.Add(New ListItem(Resource.getValue("RepositoryItemType." & type.ToString), type))
        Next
        If RBLtype.Items.Count = 0 Then
            RBLtype.Items.Add(New ListItem(Resource.getValue("RepositoryItemType." & Repository.RepositoryItemType.FileStandard.ToString), Repository.RepositoryItemType.FileStandard))
        End If
        RBLtype.SelectedIndex = 0
        CurrentPresenter.InitView(idCommunity)
    End Sub

    Private Sub RBLtype_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLtype.SelectedIndexChanged
        Dim type As Repository.RepositoryItemType = Me.ItemType
        Me.LBplay.Visible = (type <> Repository.RepositoryItemType.FileStandard)
        Me.RBLplay.Visible = (type <> Repository.RepositoryItemType.FileStandard)
        Me.LTplayClosed.Visible = (type <> Repository.RepositoryItemType.FileStandard)
        If type <> Repository.RepositoryItemType.FileStandard Then
            Me.RBLplay.SelectedValue = SystemSettings.Presenter.RepositoryConfiguration.DefaultDownload.Contains(CInt(type))
            Me.RBLplay.Enabled = SystemSettings.Presenter.RepositoryConfiguration.AllowDownload.Contains(CInt(type))
        Else
            Me.RBLplay.SelectedValue = True
        End If
        Me.UDPtype.Update()
    End Sub

    Public Sub PDFOnly(ByVal onlyPdf As Boolean)

        Dim AllowedExtension As String() = {""}

        If onlyPdf Then
            AllowedExtension = {".pdf", ".p7m"}
        End If

        Me.RadAsyncUpload.AllowedFileExtensions = AllowedExtension

    End Sub

    'Public Sub CV_FileType_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CV_FileType.ServerValidate
    '    'ToDo
    'End Sub
End Class