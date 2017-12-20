Imports lm.Comol.Core.BaseModules.FileRepository.Presentation
Imports lm.Comol.Core.BaseModules.FileRepository.Presentation.Domain
Imports lm.Comol.Core.FileRepository.Domain
Partial Public Class UC_ModuleInternalUploader
    Inherits FRbaseControl
    Implements IViewModuleInternalUploader

#Region "Context"
    Private _Presenter As ModuleInternalUploaderPresenter
    Private ReadOnly Property CurrentPresenter() As ModuleInternalUploaderPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ModuleInternalUploaderPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property AllowAnonymousUpload As Boolean Implements IViewModuleInternalUploader.AllowAnonymousUpload
        Get
            Return ViewStateOrDefault("AllowAnonymousUpload", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowAnonymousUpload") = value
        End Set
    End Property
    Public Property MaxItems As Integer Implements IViewModuleInternalUploader.MaxItems
        Get
            Return ViewStateOrDefault("MaxItems", 1)
        End Get
        Set(value As Integer)
            ViewState("MaxItems") = value
            RAUfiles.MultipleFileSelection = (value > 1)
        End Set
    End Property
    Public Property MaxFileInput As Integer Implements IViewModuleInternalUploader.MaxFileInput
        Get
            Return RAUfiles.MaxFileInputsCount
        End Get
        Set(value As Integer)
            If value > 0 Then
                RAUfiles.MaxFileInputsCount = value
            Else
                RAUfiles.MaxFileInputsCount = 1
            End If
        End Set
    End Property
    Private Property RepositoryIdentifier As RepositoryIdentifier Implements IViewModuleInternalUploader.RepositoryIdentifier
        Get
            Dim result As RepositoryIdentifier = Nothing
            If Not IsNothing(ViewState("RepositoryIdentifier")) Then
                Try
                    result = DirectCast(ViewState("RepositoryIdentifier"), RepositoryIdentifier)
                Catch ex As Exception

                End Try
            End If
            Return result
        End Get
        Set(value As RepositoryIdentifier)
            ViewState("RepositoryIdentifier") = value
        End Set
    End Property
    Private Property IdUploaderUser As Int32 Implements IViewModuleInternalUploader.IdUploaderUser
        Get
            Return ViewStateOrDefault("IdUploaderUser", 0)
        End Get
        Set(value As Int32)
            ViewState("IdUploaderUser") = value
        End Set
    End Property


#End Region

#Region "Internal"
    Private _DisplayTypeSelector As Boolean
    Public Property DisplayTypeSelector() As Boolean
        Get
            Return _DisplayTypeSelector
        End Get
        Set(ByVal value As Boolean)
            DVitemsType.Visible = value
            _DisplayTypeSelector = value
        End Set
    End Property
    Public Property Enabled() As Boolean
        Get
            Return RAUfiles.Enabled
        End Get
        Set(ByVal value As Boolean)
            RAUfiles.Enabled = value
        End Set
    End Property
    Public Property DisplayFileSelectLabel() As Boolean
        Get
            Return LBselectAsyncFiles_t.Visible
        End Get
        Set(ByVal value As Boolean)
            LBselectAsyncFiles_t.Visible = value
        End Set
    End Property
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
    Public Event IsValidOperation(ByRef isvalid As Boolean)
    Public WriteOnly Property PostbackTriggers As String
        Set(value As String)
            If Not String.IsNullOrWhiteSpace(value) Then
                If value.Contains(",") Then
                    RAUfiles.PostbackTriggers = value.Split(",").ToArray
                Else
                    RAUfiles.PostbackTriggers = New String() {value}
                End If
            End If
        End Set
    End Property
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

#Region "Inherits"
    '    MyBase.SetCulture("UC_CommunityFile", "Generici", "UC_File")

    'Public Overrides Sub SetInternazionalizzazione()
    '    With MyBase.Resource
    '        Me.PRAcommunityFileUpload.Localization.Cancel = .getValue("Localization.Cancel")
    '        Me.PRAcommunityFileUpload.Localization.ElapsedTime = .getValue("Localization.ElapsedTime")
    '        Me.PRAcommunityFileUpload.Localization.EstimatedTime = .getValue("Localization.EstimatedTime")
    '        Me.PRAcommunityFileUpload.Localization.Total = .getValue("Localization.Total")
    '        Me.PRAcommunityFileUpload.Localization.TotalFiles = .getValue("Localization.TotalFiles")
    '        Me.PRAcommunityFileUpload.Localization.TransferSpeed = .getValue("Localization.TransferSpeed")
    '        Me.PRAcommunityFileUpload.Localization.Uploaded = .getValue("Localization.Uploaded")
    '        Me.PRAcommunityFileUpload.Localization.UploadedFiles = .getValue("Localization.UploadedFiles")

    '        Dim newCulture As CultureInfo = CultureInfo.CreateSpecificCulture(Me.LinguaCode)
    '        Me.PRAcommunityFileUpload.Culture = newCulture
    '        Dim availableTypes As List(Of Repository.RepositoryItemType) = (From t In Me.SystemSettings.Presenter.RepositoryConfiguration.AvailableItemType Select DirectCast(t, Repository.RepositoryItemType)).ToList

    '        For i As Integer = 0 To 4
    '            Dim oLabel As Label = Me.FindControl("LBfileName_" & i)
    '            oLabel.Text = .getValue("fileName")

    '            oLabel = Me.FindControl("LBfileType_" & i)
    '            oLabel.Text = .getValue("fileType")

    '            Dim oRadioList As RadioButtonList = Me.FindControl("RBLtype_" & i)

    '            For Each type As Repository.RepositoryItemType In (From a In availableTypes Where a <> Repository.RepositoryItemType.Folder).ToList
    '                oRadioList.Items.Add(New ListItem(Resource.getValue("RepositoryItemType." & type.ToString), type))
    '            Next
    '            If oRadioList.Items.Count = 0 Then
    '                oRadioList.Items.Add(New ListItem(Resource.getValue("RepositoryItemType." & Repository.RepositoryItemType.FileStandard.ToString), Repository.RepositoryItemType.FileStandard))
    '            End If
    '            oRadioList.SelectedIndex = 0
    '        Next
    '    End With
    'End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLabel(LBselectAsyncFiles_t)
            .setLabel(LBfilesItemType_t)
            .setLabel(LBitemTypeDescription)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(idUploaderUser As Integer, repository As RepositoryIdentifier) Implements IViewModuleInternalUploader.InitializeControl
        InitializeRepositoryPath(repository.Type, repository.IdCommunity)
        CurrentPresenter.InitView(idUploaderUser, repository, AllowAnonymousUpload)
    End Sub
    Public Sub InitializeControlForCommunity(idUploaderUser As Integer, idCommunity As Integer) Implements IViewModuleInternalUploader.InitializeControlForCommunity
        InitializeControl(idUploaderUser, lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier.Create(RepositoryType.Community, idCommunity))
    End Sub
    Public Sub InitializeControlForPortal(idUploaderUser As Integer) Implements IViewModuleInternalUploader.InitializeControlForPortal
        InitializeControl(idUploaderUser, lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier.Create(RepositoryType.Portal, 0))
    End Sub


    Public Function AddModuleInternalFiles(obj As Object, idObject As Long, idObjectType As Integer, moduleCode As String, idModuleAjaxAction As Integer, Optional idModuleAction As Integer = 0) As List(Of dtoModuleUploadedItem) Implements IViewModuleInternalUploader.AddModuleInternalFiles
        Dim isvalid As Boolean = True
        RaiseEvent IsValidOperation(isvalid)
        If isvalid Then
            Dim files As List(Of dtoUploadedItem) = GetFiles()
            ResetInputItems()
            Return CurrentPresenter.AddModuleInternalFiles(SystemSettings.NotificationErrorService.ComolUniqueID, IdUploaderUser, AllowAnonymousUpload, RepositoryIdentifier, files, obj, idObject, idObjectType, moduleCode, idModuleAjaxAction, idModuleAction)
        Else
            ResetInputItems()
            Return New List(Of dtoModuleUploadedItem)
        End If
    End Function
    Private Sub LoadItemTypes(types As List(Of ItemType)) Implements IViewModuleUploader.LoadItemTypes
        If IsNothing(types) Then
            types = New List(Of ItemType)
        End If
        If Not types.Any() OrElse Not types.Any(Function(t) t = ItemType.File) Then
            types.Add(ItemType.File)
        End If
        RBLitemType.Items.Clear()
        Dim translations As List(Of lm.Comol.Core.DomainModel.TranslatedItem(Of Integer)) = types.Select(Function(t) New lm.Comol.Core.DomainModel.TranslatedItem(Of Integer) With {.Id = CInt(t), .Translation = Resource.getValue("RBLitemType.ItemType." & t.ToString)}).ToList()
        For Each tItem As lm.Comol.Core.DomainModel.TranslatedItem(Of Integer) In translations.OrderBy(Function(t) t.Translation).ToList()
            Dim oListItem As New ListItem() With {.Text = tItem.Translation, .Value = tItem.Id}
            oListItem.Attributes.Add("class", "item")

            RBLitemType.Items.Add(oListItem)
        Next
        RBLitemType.SelectedValue = CInt(ItemType.File)
    End Sub
    Public Sub DisableControl() Implements IViewModuleInternalUploader.DisableControl
        RAUfiles.Enabled = False
        RBLitemType.Enabled = False
    End Sub
#End Region

#Region "Internal"
    Private Function GetFiles() As List(Of dtoUploadedItem)
        Dim items As New List(Of dtoUploadedItem)
        If RAUfiles.UploadedFiles.Count > 0 Then
            items.AddRange(GetFiles(RAUfiles))
        End If
        Return items
    End Function
    Private Function GetFiles(uploader As Telerik.Web.UI.RadAsyncUpload) As List(Of dtoUploadedItem)
        Dim items As New List(Of dtoUploadedItem)
        For Each f As Telerik.Web.UI.UploadedFile In uploader.UploadedFiles
            Dim item As New dtoUploadedItem
            item.UniqueId = Guid.NewGuid
            item.IsVisible = True
            item.Type = CInt(RBLitemType.SelectedValue)
            item.Extension = f.GetExtension
            item.Name = EscapeInvalidCharacter(f.GetNameWithoutExtension)
            item.OriginalName = item.Name
            item.ContentType = f.ContentType
            item.Size = f.ContentLength
            item.SavedFileName = item.UniqueId.ToString & item.Extension
            item.ThumbnailPath = GetFinalThumbnailPath()

            item.SavedPath = GetFinalPath()
            Dim folderExist As Boolean = lm.Comol.Core.File.Exists.Directory(item.SavedPath)
            If Not folderExist Then
                folderExist = lm.Comol.Core.File.Create.Directory(item.SavedPath)
            End If
            If Not lm.Comol.Core.File.Exists.Directory(item.ThumbnailPath) Then
                lm.Comol.Core.File.Create.Directory(item.ThumbnailPath)
            End If
            If folderExist Then
                If lm.Comol.Core.File.Create.UploadFile(f, item.SavedPath & item.SavedFileName) Then
                    item.SavedFullPath = item.SavedPath & item.SavedFileName
                End If
            End If
            items.Add(item)
        Next
        Return items
    End Function
    Private Sub ResetInputItems()
        If RBLitemType.Items.Count > 0 Then
            RBLitemType.SelectedValue = CInt(ItemType.File)
        End If
    End Sub
    Private Function EscapeInvalidCharacter(name As String) As String
        If name.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) = -1 Then
            Return name
        Else
            'Escape invalid characters
            name = System.Text.RegularExpressions.Regex.Replace(name, "[^\w\.@-]", "", System.Text.RegularExpressions.RegexOptions.None)
            'If all characters are invalid return underscore as a file name
            If (System.IO.Path.GetFileNameWithoutExtension(name).Length = 0) Then
                Return name.Insert(0, "_")
            End If
            'Else return the escaped name
            Return name
        End If
    End Function
#End Region
#Region "Control Methods"


    'Private Sub CloseDialog(ByVal dialogId As String)
    '    Dim script As String = String.Format("closeDialog('{0}')", dialogId)
    '    ScriptManager.RegisterClientScriptBlock(MyBase.Page, Me.GetType, Me.UniqueID, script, True)
    'End Sub
    'Private Sub OpenDialog(ByVal dialogId As String)

    '    Me.LTscript.Text = "<script language='Javascript' type='text/javascript'> "
    '    Me.LTscript.Text &= String.Format("showDialog('{0}');", dialogId)
    '    Me.LTscript.Text &= " </script>"
    'End Sub

    'Function AddModuleInternalFiles(ByVal FileType As FileRepositoryType, ByVal ObjectOwner As Object, ByVal ServiceOwner As String, ByVal ServiceOwnerActionID As Integer, ByVal ObjectTypeID As Integer) As MultipleUploadResult(Of dtoModuleUploadedFile) Implements IviewModuleMultipleFileUploader.AddModuleInternalFiles
    '    Dim oResult As New MultipleUploadResult(Of dtoModuleUploadedFile)
    '    Dim FileUploader As HtmlControls.HtmlInputFile
    '    For i As Integer = 0 To 4
    '        FileUploader = Me.FindControl("INfile_" & i.ToString)
    '        If IsNothing(FileUploader) = False Then
    '            Dim oRadioButtonList As RadioButtonList = Me.FindControl("RBLtype_" & i.ToString)
    '            Dim oTelerikFile As Telerik.Web.UI.UploadedFile = Telerik.Web.UI.RadUploadContext.Current.UploadedFiles(FileUploader.UniqueID)
    '            If Not IsNothing(oTelerikFile) Then
    '                Dim isDownloadable As Boolean = True
    '                If oRadioButtonList.SelectedValue <> CInt(Repository.RepositoryItemType.FileStandard) Then
    '                    isDownloadable = SystemSettings.Presenter.RepositoryConfiguration.DefaultDownload.Contains(CInt(oRadioButtonList.SelectedValue))
    '                End If
    '                Me.AddFile(oResult, oTelerikFile, oRadioButtonList.SelectedValue, isDownloadable, FileType, ObjectOwner, ServiceOwner, ServiceOwnerActionID, ObjectTypeID)
    '            End If
    '        End If
    '    Next

    '    If AjaxEnabled AndAlso oResult.NotuploadedFile.Count > 0 Then
    '        Me.LBerrorNotification.Text = Me.Resource.getValue("ItemRepositoryStatus." & oResult.NotuploadedFile(0).Status.ToString)
    '        Me.OpenDialog("uploadError")
    '        Me.UDPerrors.Update()
    '    Else
    '        Me.LTscript.Text = ""
    '    End If
    '    Return oResult
    'End Function
    'Private Function AddFile(ByVal oResult As MultipleUploadResult(Of dtoModuleUploadedFile), ByVal oUploadedFile As Telerik.Web.UI.UploadedFile, ByVal type As Repository.RepositoryItemType, ByVal isDownloadable As Boolean, ByVal FileType As lm.Comol.Core.DomainModel.FileRepositoryType, ByVal ObjectOwner As Object, ByVal ServiceOwner As String, ByVal ServiceOwnerActionID As Integer, ByVal ObjectTypeID As Integer) As dtoModuleUploadedFile
    '    Dim oResponse As dtoModuleUploadedFile
    '    Dim oBaseFile As ModuleInternalFile = Me.CreateFromTelerik(oUploadedFile, type, isDownloadable)
    '    Dim oImpersonate As New lm.Comol.Core.File.Impersonate
    '    Try
    '        Dim CommunityPath As String = ""
    '        If Me.SystemSettings.File.Materiale.DrivePath = "" Then
    '            CommunityPath = Server.MapPath(BaseUrl & Me.SystemSettings.File.Materiale.VirtualPath) & "\" & Me.RepositoryCommunityID
    '        Else
    '            CommunityPath = Me.SystemSettings.File.Materiale.DrivePath & "\" & Me.RepositoryCommunityID
    '        End If
    '        CommunityPath = Replace(CommunityPath, "\\", "\")

    '        If oImpersonate.ImpersonateValidUser() = ItemRepositoryStatus.ImpersonationFailed Then
    '            oResponse = New dtoModuleUploadedFile(oBaseFile, ItemRepositoryStatus.ImpersonationFailed)
    '        Else
    '            Create.Directory_FM(CommunityPath)

    '            Dim SavedFile As String = CommunityPath & "\" & oBaseFile.UniqueID.ToString & ".stored"
    '            oUploadedFile.SaveAs(SavedFile, True)
    '            oResponse = Me.CurrentPresenter.AddModuleInternalFile(FileType, oBaseFile, SavedFile, CommunityPath, ObjectOwner, ServiceOwner, ServiceOwnerActionID, ObjectTypeID)


    '            oResponse.SavedFilePath = SavedFile
    '            If oResponse.Status <> ItemRepositoryStatus.FileUploaded AndAlso oResponse.Status <> ItemRepositoryStatus.FileExist Then
    '                Delete.File_FM(SavedFile)
    '            End If

    '        End If
    '        oImpersonate.UndoImpersonation()
    '    Catch ex As Exception
    '        oImpersonate.UndoImpersonation()
    '        oResponse = New dtoModuleUploadedFile(oBaseFile, ItemRepositoryStatus.UploadError)
    '    End Try
    '    If Not IsNothing(oResponse) AndAlso oResponse.Status = ItemRepositoryStatus.FileUploaded Then
    '        oResult.UploadedFile.Add(oResponse)
    '    Else
    '        oResult.NotuploadedFile.Add(oResponse)
    '    End If
    '    Return oResponse
    'End Function


#End Region

    'Public Sub InitializeControl(ByVal CommunityID As Integer) Implements IviewModuleMultipleFileUploader.InitializeControl
    '    Me.PRAcommunityFileUpload.Enabled = True
    '    Dim oProgress As Telerik.Web.UI.RadProgressContext = Telerik.Web.UI.RadProgressContext.Current
    '    If Not IsNothing(oProgress) Then
    '        oProgress.PrimaryPercent = 0
    '        oProgress.SecondaryValue = ""
    '        oProgress.SecondaryPercent = ""
    '    End If
    '    Me.CurrentPresenter.InitView(CommunityID)
    'End Sub

    'Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
    '    Dim form As HtmlForm = Page.Form
    '    If Not IsNothing(form) AndAlso form.Enctype.Length = 0 Then
    '        form.Enctype = "multipart/form-data"
    '    End If
    'End Sub

End Class