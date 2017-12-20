Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.UI.Presentation
Imports System.Globalization
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.File

Partial Public Class UC_ModuleInternalFileMultipleUploader
    Inherits BaseControlWithLoad
    Implements IviewModuleMultipleFileUploader

#Region "IVIEW"
    Private _Presenter As ModuleMultipleFileUploaderPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Private ReadOnly Property CurrentPresenter() As ModuleMultipleFileUploaderPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ModuleMultipleFileUploaderPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
    Public Property RepositoryCommunityID() As Integer Implements IviewModuleMultipleFileUploader.RepositoryCommunityID
        Get
            Return CInt(Me.ViewState("RepositoryCommunityID"))
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("RepositoryCommunityID") = value
        End Set
    End Property

    Public Property MaxFileInputsCount() As Integer Implements IviewModuleMultipleFileUploader.MaxFileInputsCount
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
    Public Property AllowAnonymousUpload As Boolean Implements IviewModuleMultipleFileUploader.AllowAnonymousUpload
        Get
            Return ViewStateOrDefault("AllowAnonymousUpload", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowAnonymousUpload") = value
        End Set
    End Property
    Public Property UploadAsUser As Int32 Implements IviewModuleMultipleFileUploader.UploadAsUser
        Get
            Return ViewStateOrDefault("UploadAsUser", 0)
        End Get
        Set(value As Int32)
            ViewState("UploadAsUser") = value
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
        If Page.IsPostBack = False Then
            Page.Form.Enctype = "multipart/form-data"
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
            Dim availableTypes As List(Of Repository.RepositoryItemType) = (From t In Me.SystemSettings.Presenter.RepositoryConfiguration.AvailableItemType Select DirectCast(t, Repository.RepositoryItemType)).ToList

            For i As Integer = 0 To 4
                Dim oLabel As Label = Me.FindControl("LBfileName_" & i)
                oLabel.Text = .getValue("fileName")

                oLabel = Me.FindControl("LBfileType_" & i)
                oLabel.Text = .getValue("fileType")

                Dim oRadioList As RadioButtonList = Me.FindControl("RBLtype_" & i)

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
    Private Sub CloseDialog(ByVal dialogId As String)
        Dim script As String = String.Format("closeDialog('{0}')", dialogId)
        ScriptManager.RegisterClientScriptBlock(MyBase.Page, Me.GetType, Me.UniqueID, script, True)
    End Sub
    Private Sub OpenDialog(ByVal dialogId As String)

        Me.LTscript.Text = "<script language='Javascript' type='text/javascript'> "
        Me.LTscript.Text &= String.Format("showDialog('{0}');", dialogId)
        Me.LTscript.Text &= " </script>"
    End Sub

    Function AddModuleInternalFiles(ByVal FileType As FileRepositoryType, ByVal ObjectOwner As Object, ByVal ServiceOwner As String, ByVal ServiceOwnerActionID As Integer, ByVal ObjectTypeID As Integer) As MultipleUploadResult(Of dtoModuleUploadedFile) Implements IviewModuleMultipleFileUploader.AddModuleInternalFiles
        Dim oResult As New MultipleUploadResult(Of dtoModuleUploadedFile)
        Dim FileUploader As HtmlControls.HtmlInputFile
        For i As Integer = 0 To 4
            FileUploader = Me.FindControl("INfile_" & i.ToString)
            If IsNothing(FileUploader) = False Then
                Dim oRadioButtonList As RadioButtonList = Me.FindControl("RBLtype_" & i.ToString)
                Dim oTelerikFile As Telerik.Web.UI.UploadedFile = Telerik.Web.UI.RadUploadContext.Current.UploadedFiles(FileUploader.UniqueID)
                If Not IsNothing(oTelerikFile) Then
                    Dim isDownloadable As Boolean = True
                    If oRadioButtonList.SelectedValue <> CInt(Repository.RepositoryItemType.FileStandard) Then
                        isDownloadable = SystemSettings.Presenter.RepositoryConfiguration.DefaultDownload.Contains(CInt(oRadioButtonList.SelectedValue))
                    End If
                    Me.AddFile(oResult, oTelerikFile, oRadioButtonList.SelectedValue, isDownloadable, FileType, ObjectOwner, ServiceOwner, ServiceOwnerActionID, ObjectTypeID)
                End If
            End If
        Next

        If AjaxEnabled AndAlso oResult.NotuploadedFile.Count > 0 Then
            Me.LBerrorNotification.Text = Me.Resource.getValue("ItemRepositoryStatus." & oResult.NotuploadedFile(0).Status.ToString)
            Me.OpenDialog("uploadError")
            Me.UDPerrors.Update()
        Else
            Me.LTscript.Text = ""
        End If
        Return oResult
    End Function
    Private Function AddFile(ByVal oResult As MultipleUploadResult(Of dtoModuleUploadedFile), ByVal oUploadedFile As Telerik.Web.UI.UploadedFile, ByVal type As Repository.RepositoryItemType, ByVal isDownloadable As Boolean, ByVal FileType As lm.Comol.Core.DomainModel.FileRepositoryType, ByVal ObjectOwner As Object, ByVal ServiceOwner As String, ByVal ServiceOwnerActionID As Integer, ByVal ObjectTypeID As Integer) As dtoModuleUploadedFile
        Dim oResponse As dtoModuleUploadedFile
        Dim oBaseFile As ModuleInternalFile = Me.CreateFromTelerik(oUploadedFile, type, isDownloadable)
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
                Create.Directory_FM(CommunityPath)
                
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

    Private Function CreateFromTelerik(ByVal oTelerikFile As Telerik.Web.UI.UploadedFile, ByVal type As Repository.RepositoryItemType, ByVal isDownloadable As Boolean) As ModuleInternalFile
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
            .RepositoryItemType = type
            Select Case type
                Case Repository.RepositoryItemType.ScormPackage
                    .isSCORM = True
                Case Repository.RepositoryItemType.VideoStreaming
                    .isVideocast = True
            End Select
            .IsDownloadable = isDownloadable
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

    Public Sub InitializeControl(ByVal CommunityID As Integer) Implements IviewModuleMultipleFileUploader.InitializeControl
        Me.PRAcommunityFileUpload.Enabled = True
        Dim oProgress As Telerik.Web.UI.RadProgressContext = Telerik.Web.UI.RadProgressContext.Current
        If Not IsNothing(oProgress) Then
            oProgress.PrimaryPercent = 0
            oProgress.SecondaryValue = ""
            oProgress.SecondaryPercent = ""
        End If
        Me.CurrentPresenter.InitView(CommunityID)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim form As HtmlForm = Page.Form
        If Not IsNothing(form) AndAlso form.Enctype.Length = 0 Then
            form.Enctype = "multipart/form-data"
        End If
    End Sub

    'Private Sub INfile_0_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles INfile_0.PreRender
    '    Dim form As HtmlForm = Page.Form
    '    If Not IsNothing(form) AndAlso form.Enctype.Length = 0 Then
    '        form.Enctype = "multipart/form-data"
    '    End If
    'End Sub

    ''' <summary>
    ''' DISATTIVATO
    ''' </summary>
    ''' <param name="FileType"></param>
    ''' <param name="ObjectOwner"></param>
    ''' <param name="ServiceOwner"></param>
    ''' <param name="ObjectTypeID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UploadAndLinkInternalFile(ByVal FileType As FileRepositoryType, ByVal ObjectOwner As Object, ByVal ServiceOwner As String, ByVal ObjectTypeID As Integer) As IList(Of ModuleActionLink) Implements IviewModuleMultipleFileUploader.UploadAndLinkInternalFile
        Dim InternalFile As New MultipleUploadResult(Of dtoModuleUploadedFile)
    End Function

    'Public Function UploadAndLinkInternalFile(ByVal FileType As lm.Comol.Core.DomainModel.FileRepositoryType, ByVal ObjectOwner As Object, ByVal ServiceOwner As String, ByVal ServiceOwnerActionID As Integer, ByVal ObjectTypeID As Integer) As ModuleActionLink Implements IviewModuleSingleFileUploader.UploadAndLinkInternalFile
    '    Dim InternalFile As New MultipleUploadResult(Of dtoModuleUploadedFile)
    '    Dim oTelerikFile As Telerik.Web.UI.UploadedFile = Telerik.Web.UI.RadUploadContext.Current.UploadedFiles(TXBFile.UniqueID)
    '    If Not IsNothing(oTelerikFile) Then
    '        Me.AddFile(InternalFile, oTelerikFile, FileType, ObjectOwner, ServiceOwner, ServiceOwnerActionID, ObjectTypeID)
    '    End If

    '    If IsNothing(InternalFile) OrElse InternalFile.UploadedFile.Count = 0 Then
    '        Return Nothing
    '    Else
    '        Return Me.CurrentPresenter.GetModuleInternalFileLink(InternalFile.UploadedFile(0).File)
    '    End If
    'End Function
End Class