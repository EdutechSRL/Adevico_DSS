Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.UI.Presentation
Imports System.Globalization
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.File

Partial Public Class UC_ModuleInternalFileUploader
    Inherits BaseControlWithLoad
    Implements IviewModuleSingleFileUploader


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
    Private Property ItemType() As Repository.RepositoryItemType Implements IviewModuleSingleFileUploader.ItemType
        Get
            Return Me.RBLtype.SelectedValue
        End Get
        Set(ByVal value As Repository.RepositoryItemType)
            Me.RBLtype.SelectedValue = value
            Me.DVtypeDownload.Visible = Not (value = Repository.RepositoryItemType.FileStandard OrElse value = Repository.RepositoryItemType.Folder)
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
            .setLabel(Me.LBname_t)
            .setLabel(Me.LBtype_t)

            Dim availableTypes As List(Of Repository.RepositoryItemType) = (From t In Me.SystemSettings.Presenter.RepositoryConfiguration.AvailableItemType Select DirectCast(t, Repository.RepositoryItemType)).ToList
            RBLtype.Items.Clear()
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

        Me.LTscript.Text = "<script language='Javascript' type='text/javascript'> "
        Me.LTscript.Text &= String.Format("showDialog('{0}');", dialogId)
        Me.LTscript.Text &= " </script>"
    End Sub

    Public Function AddModuleInternalFile(ByVal FileType As lm.Comol.Core.DomainModel.FileRepositoryType, ByVal ObjectOwner As Object, ByVal ServiceOwner As String, ByVal ServiceOwnerActionID As Integer, ByVal ObjectTypeID As Integer) As MultipleUploadResult(Of dtoModuleUploadedFile) Implements lm.Comol.Modules.Base.Presentation.IviewModuleSingleFileUploader.AddModuleInternalFile
        Dim oResult As New MultipleUploadResult(Of dtoModuleUploadedFile)
        Dim oTelerikFile As Telerik.Web.UI.UploadedFile = Telerik.Web.UI.RadUploadContext.Current.UploadedFiles(TXBFile.UniqueID)
        If Not IsNothing(oTelerikFile) Then
            Me.AddFile(oResult, oTelerikFile, FileType, ObjectOwner, ServiceOwner, ServiceOwnerActionID, ObjectTypeID)
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

    Public Sub InitializeControl(ByVal CommunityID As Integer, allowAnonymousUpload As Boolean) Implements IviewModuleSingleFileUploader.InitializeControl
        Me.AllowAnonymousUpload = allowAnonymousUpload
        Me.PRAcommunityFileUpload.Enabled = True
        Dim oProgress As Telerik.Web.UI.RadProgressContext = Telerik.Web.UI.RadProgressContext.Current
        If Not IsNothing(oProgress) Then
            oProgress.PrimaryPercent = 0
            oProgress.SecondaryValue = ""
            oProgress.SecondaryPercent = ""
        End If
        Me.CurrentPresenter.InitView(CommunityID)
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


    Public Function UploadAndLinkInternalFile(FileType As lm.Comol.Core.DomainModel.FileRepositoryType, ObjectOwner As Object, ServiceOwner As String, ServiceOwnerActionID As Integer, ObjectTypeID As Integer) As lm.Comol.Core.DomainModel.ModuleActionLink Implements lm.Comol.Modules.Base.Presentation.IviewModuleSingleFileUploader.UploadAndLinkInternalFile

    End Function

  
End Class