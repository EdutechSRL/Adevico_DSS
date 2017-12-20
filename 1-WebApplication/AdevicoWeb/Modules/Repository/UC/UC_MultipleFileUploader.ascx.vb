Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.UI.Presentation
Imports System.Globalization
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.File

Partial Public Class UC_MultipleFileUploader
    Inherits BaseControlWithLoad
    Implements IviewMultipleFileUploader


#Region "IVIEW"
    Private _Presenter As MultipleFileUploaderPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Private ReadOnly Property CurrentPresenter() As MultipleFileUploaderPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New MultipleFileUploaderPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
    Private ReadOnly Property Portalname() As String Implements IviewMultipleFileUploader.Portalname
        Get
            Return Me.Resource.getValue("Portalname")
        End Get
    End Property
    Private ReadOnly Property BaseFolder() As String Implements IviewMultipleFileUploader.BaseFolder
        Get
            Return Me.Resource.getValue("BaseFolder")
        End Get
    End Property
    Public Property RepositoryCommunityID() As Integer Implements IviewMultipleFileUploader.RepositoryCommunityID
        Get
            Return CInt(Me.ViewState("RepositoryCommunityID"))
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("RepositoryCommunityID") = value
        End Set
    End Property
    Public Property FolderID() As Long Implements IviewMultipleFileUploader.FolderID
        Get
            Return Me.CTRLCommunityFolder.SelectedFolder
        End Get
        Set(ByVal value As Long)
            Me.CTRLCommunityFolder.SelectedFolder = value
            Me.UDPselectFolder.Update()
        End Set
    End Property
    Private WriteOnly Property AllowFolderChange() As Boolean Implements IviewMultipleFileUploader.AllowFolderChange
        Set(ByVal value As Boolean)
            Me.BTNeditPath.Enabled = value
            Me.UDPdata.Update()
        End Set
    End Property
    Private WriteOnly Property CommunityName() As String Implements IviewMultipleFileUploader.CommunityName
        Set(ByVal value As String)
            Me.LBcommunity.Text = value
        End Set
    End Property
    Private WriteOnly Property FilePath() As String Implements IviewMultipleFileUploader.FilePath
        Set(ByVal value As String)
            Me.LBpath.Text = value
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

    Private Property VisibleToDonwloaders() As Boolean Implements IviewMultipleFileUploader.VisibleToDonwloaders
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
    Private Property DownlodableByCommunity() As Boolean Implements IviewMultipleFileUploader.DownlodableByCommunity
        Get
            Return True
        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property

    Public Property MaxFileInputsCount() As Integer Implements IviewMultipleFileUploader.MaxFileInputsCount
        Get
            Dim FileNumber As Integer = 0
            For i As Integer = 0 To 4
                Dim oDiv As HtmlControls.HtmlGenericControl = Me.FindControl("DVfile" & i.ToString)
                i += IIf(oDiv.Visible = True, 1, 0)
            Next

            Return FileNumber
        End Get
        Set(ByVal value As Integer)
            If value > 5 Then
                value = 5
            End If
            For i As Integer = 0 To 4
                Dim oDiv As HtmlControls.HtmlGenericControl = Me.FindControl("DVfile" & i.ToString)
                oDiv.Visible = (i + 1 <= value)
            Next
        End Set
    End Property
    Public Property AllowPersonalPermission() As Boolean Implements IviewMultipleFileUploader.AllowPersonalPermission
        Get
            Return Me.RBLallowTo.Enabled
        End Get
        Set(ByVal value As Boolean)
            Me.RBLallowTo.Enabled = value
            DVpermision.Visible = value
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

#Region "Inherited"
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
            .setLabel(Me.LBpath_t)
            .setLabel(Me.LBvisibleTo_t)
            .setButton(BTNeditPath, True, , , True)
            .setRadioButtonList(Me.RBLallowTo, True)
            .setRadioButtonList(Me.RBLallowTo, False)
            .setRadioButtonList(Me.RBLvisibleTo, True)
            .setRadioButtonList(Me.RBLvisibleTo, False)

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
            Dim availableTypes As List(Of Repository.RepositoryItemType) = (From t In Me.SystemSettings.Presenter.RepositoryConfiguration.AvailableItemType Select DirectCast(t, Repository.RepositoryItemType)).ToList

            For i As Integer = 0 To 4
                Dim oLabel As Label = Me.FindControl("LBfileName_" & i)
                oLabel.Text = .getValue("fileName")

                oLabel = Me.FindControl("LBfileType_" & i)
                oLabel.Text = .getValue("fileType")

                Dim oRadioList As RadioButtonList = Me.FindControl("RBLtype_" & i)
                'oRadioList.Items(0).Text = .getValue("fileType." & Main.TipoMaterialeUpload.Standard)
                'oRadioList.Items(1).Text = .getValue("fileType." & Main.TipoMaterialeUpload.Scorm)
                'If Not Me.SystemSettings.Presenter.DefaultScormEnabled Then
                '    If Not IsNothing(oRadioList.Items.FindByValue(TipoMaterialeUpload.Scorm)) Then
                '        oRadioList.Items.RemoveAt(1)
                '    End If
                '    oRadioList.SelectedIndex = 0
                'End If
                oRadioList.Items.Clear()
                For Each type As Repository.RepositoryItemType In (From a In availableTypes Where a <> Repository.RepositoryItemType.Folder).ToList
                    oRadioList.Items.Add(New ListItem(Resource.getValue("RepositoryItemType." & type.ToString), type))
                Next
                If oRadioList.Items.Count = 0 Then
                    oRadioList.Items.Add(New ListItem(Resource.getValue("RepositoryItemType." & Repository.RepositoryItemType.FileStandard.ToString), Repository.RepositoryItemType.FileStandard))
                End If
                oRadioList.SelectedIndex = 0
            Next
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

        Me.LTscript.Text = "<script language='Javascript' type='text/javascript'> "
        Me.LTscript.Text &= String.Format("showDialog('{0}');", dialogId)
        Me.LTscript.Text &= " </script>"
    End Sub
    Public Function AddFilesToCommunityRepository() As MultipleUploadResult(Of dtoUploadedFile)
        Dim oResult As New MultipleUploadResult(Of dtoUploadedFile)
        If Me.CommunityPermission.Administration OrElse Me.CommunityPermission.UploadFile Then
            Dim FileUploader As HtmlControls.HtmlInputFile
            For i As Integer = 0 To 4
                FileUploader = Me.FindControl("INfile_" & i.ToString)
                If IsNothing(FileUploader) = False Then
                    Dim oRadioButtonList As RadioButtonList = Me.FindControl("RBLtype_" & i.ToString)
                    Dim oTelerikFile As Telerik.Web.UI.UploadedFile = Telerik.Web.UI.RadUploadContext.Current.UploadedFiles(FileUploader.UniqueID)
                    If Not IsNothing(oTelerikFile) Then
                        Me.AddFile(oResult, Telerik.Web.UI.RadUploadContext.Current.UploadedFiles(FileUploader.UniqueID), oRadioButtonList.SelectedValue)
                    End If
                End If
            Next
        Else
            For Each oTelerikFile As Telerik.Web.UI.UploadedFile In Telerik.Web.UI.RadUploadContext.Current.UploadedFiles
                oResult.NotuploadedFile.Add(New dtoUploadedFile(CreateFromTelerik(oTelerikFile, Repository.RepositoryItemType.FileStandard), ItemRepositoryStatus.UploadError))
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
    Private Function AddFile(ByVal oResult As MultipleUploadResult(Of dtoUploadedFile), ByVal oUploadedFile As Telerik.Web.UI.UploadedFile, ByVal TypeID As Integer) As dtoUploadedFile
        Dim oResponse As dtoUploadedFile
        Dim oCommunityFile As CommunityFile = Me.CreateFromTelerik(oUploadedFile, TypeID)
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
                If oResponse.Status <> ItemRepositoryStatus.FileUploaded Then
                    Delete.File_FM(SavedFile)
                Else
                    Dim oSender As New RepositoryNotificationUtility(Me.PageUtility)
                    Dim ParentFolder As String = Me.BaseFolder

                    If oResponse.File.FolderId > 0 Then
                        ParentFolder &= Me.CurrentPresenter.GetFolderName(oResponse.File.FolderId)
                    End If
                    If oResponse.File.isFile = False Then
                        If oResponse.File.isVisible Then
                            oSender.NotifyFolderCreated(Me.RepositoryCommunityID, oResponse.File.FolderId, oResponse.File.Id, oResponse.File.DisplayName, ParentFolder)
                        Else
                            oSender.NotifyFolderCreated(oSender.PermissionToAdmin, Me.RepositoryCommunityID, oResponse.File.FolderId, oResponse.File.Id, oResponse.File.DisplayName, ParentFolder)
                        End If

                    Else 'If oResponse.File.isSCORM Then
                        oSender.NotifyFileUploaded(IIf(oResponse.File.isVisible, oSender.PermissionToSee, oSender.PermissionToAdmin), Me.RepositoryCommunityID, oResponse.File.FolderId, oResponse.File.Id, oResponse.File.UniqueID, oResponse.File.DisplayName, ParentFolder, oResponse.File.RepositoryItemType)
                        'Else
                        '    oSender.NotifyFileUploaded(IIf(oResponse.File.isVisible, oSender.PermissionToSee, oSender.PermissionToAdmin), Me.RepositoryCommunityID, oResponse.File.FolderId, oResponse.File.Id, oResponse.File.DisplayName, ParentFolder)
                    End If
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
    Private Function CreateFromTelerik(ByVal oTelerikFile As Telerik.Web.UI.UploadedFile, ByVal type As Repository.RepositoryItemType) As CommunityFile
        Dim oCommunityFile As New CommunityFile
        With oCommunityFile
            .CloneID = 0
            .ContentType = oTelerikFile.ContentType
            .Description = ""
            .DisplayOrder = 1
            .Downloads = 0
            .FileCategoryID = Me.SystemSettings.Presenter.DefaultFileCategoryID
            .FolderId = Me.FolderID
            .isDeleted = False
            .isPersonal = False
            .isSCORM = False
            .isVirtual = False
            .isVideocast = False
            .RepositoryItemType = type
            Select Case type
                Case Repository.RepositoryItemType.ScormPackage
                    .isSCORM = True
                    .IsDownloadable = SystemSettings.Presenter.RepositoryConfiguration.DefaultDownload.Contains(Repository.RepositoryItemType.ScormPackage)
                Case Repository.RepositoryItemType.VideoStreaming
                    .isVideocast = True
                    .IsDownloadable = SystemSettings.Presenter.RepositoryConfiguration.DefaultDownload.Contains(Repository.RepositoryItemType.VideoStreaming)
                Case Repository.RepositoryItemType.Multimedia
                    .IsDownloadable = SystemSettings.Presenter.RepositoryConfiguration.DefaultDownload.Contains(Repository.RepositoryItemType.VideoStreaming)
                Case Else
                    .IsDownloadable = True
            End Select

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
#End Region

    Public Sub InitializeControl(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal oPermission As ModuleCommunityRepository) Implements IviewMultipleFileUploader.InitializeControl
        Me.PRAcommunityFileUpload.Enabled = True
        Me.LBuploadInfo.Text = Me.Resource.getValue("fileInfoAdd")
        Dim oProgress As Telerik.Web.UI.RadProgressContext = Telerik.Web.UI.RadProgressContext.Current
        If Not IsNothing(oProgress) Then
            oProgress.PrimaryPercent = 0
            oProgress.SecondaryValue = ""
            oProgress.SecondaryPercent = ""
        End If

        Me.CurrentPresenter.InitView(FolderID, CommunityID)
        CommunityPermission = oPermission
    End Sub

    Public Sub LoadFolderSelector(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean) Implements IviewMultipleFileUploader.LoadFolderSelector
        Me.CTRLCommunityFolder.InitializeControl(False, CommunityID, FolderID, ShowHiddenItems, AdminPurpose)
        Me.BTNeditPath.Enabled = Me.CTRLCommunityFolder.HasMoreFolder
        Me.UDPselectFolder.Update()
    End Sub
End Class