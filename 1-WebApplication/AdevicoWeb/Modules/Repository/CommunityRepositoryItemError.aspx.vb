Imports COL_BusinessLogic_v2.UCServices
Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.Repository.Domain

Partial Public Class CommunityRepositoryItemError
    Inherits PageBase
    Implements IViewCommunityRepositoryError

#Region "context"
    Private _BaseTreeUrl As String
    Public Overloads ReadOnly Property BaseTreeUrl() As String
        Get
            If _BaseTreeUrl = "" Then
                _BaseTreeUrl = Me.PageUtility.BaseUrl & "RadControls/TreeView/Skins/Materiale/"
            End If
            Return _BaseTreeUrl
        End Get
    End Property

    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Presenter As CRerrorPresenter
    Public ReadOnly Property CurrentPresenter() As CRerrorPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New CRerrorPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
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
#End Region
#Region "View"
  
    Public Function CommunityRepositoryPermission(ByVal CommunityID As Integer) As ModuleCommunityRepository Implements IViewCommunityRepositoryError.CommunityRepositoryPermission
        Dim oModule As ModuleCommunityRepository = Nothing

        If CommunityID = 0 Then
            oModule = ModuleCommunityRepository.CreatePortalmodule(Me.CurrentContext.UserContext.UserTypeID)
        Else
            oModule = (From sb In ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, Services_File.Codex) _
                  Where sb.CommunityID = CommunityID Select New ModuleCommunityRepository(New Services_File(sb.PermissionString))).FirstOrDefault
            If IsNothing(oModule) Then
                oModule = New ModuleCommunityRepository
            End If
        End If

        Return oModule
    End Function
    Public ReadOnly Property PreloadedCommunityID() As Integer Implements IViewCommunityRepositoryError.PreloadedCommunityID
        Get
            If IsNumeric(Me.Request.Form("CommunityID")) Then
                Return CInt(Me.Request.Form("CommunityID"))
            Else
                Return 0
            End If
        End Get
    End Property
    Public ReadOnly Property PreloadedCreate() As ItemRepositoryToCreate Implements IViewCommunityRepositoryError.PreloadedCreate
        Get
            If IsNothing(Request.Form("PreloadedCreate")) Then
                Return ItemRepositoryToCreate.File
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ItemRepositoryToCreate).GetByString(Request.Form("PreloadedCreate"), ItemRepositoryToCreate.File)
            End If
        End Get
    End Property
    Public ReadOnly Property PreloadedItemID() As Long Implements IViewCommunityRepositoryError.PreloadedItemID
        Get
            If String.IsNullOrEmpty(Request.Form("PreloadedItemID")) Then
                Return 0
            Else
                Return CLng(Request.Form("PreloadedItemID"))
            End If
        End Get
    End Property
    Public ReadOnly Property PreloadedFolderID() As Long Implements IViewCommunityRepositoryError.PreloadedFolderID
        Get
            If IsNumeric(Me.Request.Form("FolderId")) Then
                Return CLng(Me.Request.Form("FolderId"))
            Else
                Return 0
            End If
        End Get
    End Property
    Public ReadOnly Property PreloadedPreviousPage() As RepositoryPage Implements IViewCommunityRepositoryError.PreloadedPreviousPage
        Get
            If IsNothing(Request.QueryString("PreviousPage")) Then
                Return RepositoryPage.None
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of RepositoryPage).GetByString(Request.QueryString("PreviousPage"), RepositoryPage.DownLoadPage)
            End If
        End Get
    End Property
    Public ReadOnly Property PreloadedPreviousView() As IViewExploreCommunityRepository.ViewRepository Implements IViewCommunityRepositoryError.PreloadedPreviousView
        Get
            If IsNothing(Request.QueryString("PreviousView")) Then
                Return IViewExploreCommunityRepository.ViewRepository.None
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of IViewExploreCommunityRepository.ViewRepository).GetByString(Request.QueryString("PreviousView"), IViewExploreCommunityRepository.ViewRepository.None)
            End If
        End Get
    End Property
    Public ReadOnly Property PreloadedPageSender() As RepositoryPage Implements IViewCommunityRepositoryError.PreloadedPageSender
        Get
            If IsNothing(Request.QueryString("PageSender")) Then
                Return RepositoryPage.None
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of RepositoryPage).GetByString(Request.QueryString("PageSender"), RepositoryPage.DownLoadPage)
            End If
        End Get
    End Property
    Public Property PreservedDownloadUrl() As String Implements IViewCommunityRepositoryError.PreservedDownloadUrl
        Get
            Return Me.ViewState("PreservedDownloadUrl")
        End Get
        Set(ByVal value As String)
            Me.ViewState("PreservedDownloadUrl") = value
        End Set
    End Property
    Public ReadOnly Property PreserveDownloadUrl() As Boolean Implements IViewCommunityRepositoryError.PreserveDownloadUrl
        Get
            Dim iResponse As Boolean = False
            Try
                If Not IsNothing(Request.QueryString("PreserveDownloadUrl")) Then
                    iResponse = CBool(Request.QueryString("PreserveDownloadUrl"))
                End If
            Catch ex As Exception

            End Try
            Return iResponse
        End Get
    End Property
    Public ReadOnly Property PreloadedForUserID() As Integer Implements IViewCommunityRepositoryError.PreloadedForUserID
        Get
            If String.IsNullOrEmpty(Request.Form("ForUserID")) Then
                Return 0
            Else
                Return CLng(Request.Form("ForUserID"))
            End If
        End Get
    End Property
    Public ReadOnly Property PreloadedFirstError() As ItemRepositoryStatus Implements IViewCommunityRepositoryError.PreloadedFirstError
        Get
            Dim StatusValue As String = ""
            If Me.Request.Form.Keys.Count > 0 Then
                StatusValue = (From v As String In Me.Request.Form.Keys Where v.StartsWith("FILE_C_Status_") Select Me.Request.Form(v)).FirstOrDefault
            End If


            If String.IsNullOrEmpty(StatusValue) Then
                Return ItemRepositoryStatus.None
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ItemRepositoryStatus).GetByString(StatusValue, ItemRepositoryStatus.None)
            End If
        End Get
    End Property
    Public ReadOnly Property PreloadedLanguage() As String Implements IViewCommunityRepositoryError.PreloadedLanguageCode
        Get
            Return Request.Form("Language")
        End Get
    End Property
    Public Property CurrentForUserID() As Integer Implements IViewCommunityRepositoryError.CurrentForUserID
        Get
            Return ViewStateOrDefault("CurrentForUserID", 0)
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("CurrentForUserID") = value
        End Set
    End Property
    Public Property CurrentCommunityID() As Integer Implements IViewCommunityRepositoryError.CurrentCommunityID
        Get
            Return ViewStateOrDefault("CurrentCommunityID", 0)
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("CurrentCommunityID") = value
        End Set
    End Property
    Public Property CurrentFolderID() As Long Implements IViewCommunityRepositoryError.CurrentFolderID
        Get
            Return ViewStateOrDefault("CurrentFolderID", CLng(0))
        End Get
        Set(ByVal value As Long)
            Me.ViewState("CurrentFolderID") = value
        End Set
    End Property
    Public Property CurrentItemID() As Long Implements IViewCommunityRepositoryError.CurrentItemID
        Get
            Return ViewStateOrDefault("CurrentItemID", CLng(0))
        End Get
        Set(ByVal value As Long)
            Me.ViewState("CurrentItemID") = value
        End Set
    End Property
    Public Property CurrentFirstError() As ItemRepositoryStatus Implements IViewCommunityRepositoryError.CurrentFirstError
        Get
            Return ViewStateOrDefault("CurrentFirstError", ItemRepositoryStatus.None)
        End Get
        Set(ByVal value As ItemRepositoryStatus)
            Me.ViewState("CurrentFirstError") = value
        End Set
    End Property
    Public Property CurrentItemRepositoryToCreate() As ItemRepositoryToCreate Implements IViewCommunityRepositoryError.CurrentItemRepositoryToCreate
        Get
            Return ViewStateOrDefault("CurrentItemRepositoryToCreate", ItemRepositoryToCreate.File)
        End Get
        Set(ByVal value As ItemRepositoryToCreate)
            Me.ViewState("CurrentItemRepositoryToCreate") = value
        End Set
    End Property
#End Region
#Region "Inherits"
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub CommunityRepositoryItemError_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        'If Me.CurrentContext.UserContext.isAnonymous Then
        '    Me.MasterPageFile = Me.Request.ApplicationPath & "/Registrazione.master"
        'ElseIf Me.Request.Form.Keys.Count = 0 Then
        '    Me.MasterPageFile = Me.Request.ApplicationPath & "/Registrazione.master"
        If Me.Request.Form.Keys.Count = 0 Then
            Dim Status As List(Of String) = (From v As String In Me.Request.Form.Keys Where v.StartsWith("FILE_C_Status_") Select Me.Request.Form(v)).ToList
            Dim Query As List(Of ItemRepositoryStatus) = (From s In Status Select lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ItemRepositoryStatus).GetByString(s, ItemRepositoryStatus.None)).ToList
            If Query.Count = 1 AndAlso (From s As ItemRepositoryStatus In Query Where s = ItemRepositoryStatus.FileDoesntExist).Count = Query.Count Then
                Me.MasterPageFile = Me.Request.ApplicationPath & "/AjaxPopup.master"
            End If
        End If
    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        If TypeOf Me.Master Is AjaxPortal Then
            DirectCast(Me.Master, AjaxPortal).ShowNoPermission = False
            'ElseIf TypeOf Me.Master Is Registrazione Then
            '    DirectCast(Me.Master, Registrazione).ShowNoPermission = False
        ElseIf TypeOf Me.Master Is AjaxPopup Then
            DirectCast(Me.Master, AjaxPopup).ShowNoPermission = False
        End If
        If Page.IsPostBack = False Then
            '  If Me.CurrentContext.UserContext.isAnonymous Then
            Me.CurrentPresenter.InitView()
            'End If
            Me.SaveFilesToProcess()
            'If Not Me.CurrentContext.UserContext.isAnonymous Then
            '    InitViewToDisplay()
            'Else
            LoadErrors()
            'End If
            Me.InitializeView(Me.CurrentFolderID, Me.CurrentCommunityID)
        End If
    End Sub
    Public Overrides Sub BindNoPermessi()
        If TypeOf Me.Master Is AjaxPortal Then
            DirectCast(Me.Master, AjaxPortal).ShowNoPermission = True
            'ElseIf TypeOf Me.Master Is Registrazione Then
            '    DirectCast(Me.Master, Registrazione).ShowNoPermission = True
        ElseIf TypeOf Me.Master Is AjaxPopup Then
            DirectCast(Me.Master, AjaxPopup).ShowNoPermission = True
        End If
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_CommunityRepositoryUpload", "Generici")
    End Sub
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            If TypeOf Me.Master Is AjaxPortal Then
                DirectCast(Me.Master, AjaxPortal).ServiceTitle = .getValue("serviceErrorTitle")
                DirectCast(Me.Master, AjaxPortal).ServiceNopermission = .getValue("nopermission")
                'ElseIf TypeOf Me.Master Is Registrazione Then
                '    DirectCast(Me.Master, Registrazione).ServiceTitle = .getValue("serviceErrorTitle")
                '    DirectCast(Me.Master, Registrazione).ServiceNopermission = .getValue("nopermission")
            ElseIf TypeOf Me.Master Is AjaxPopup Then
                DirectCast(Me.Master, AjaxPopup).ServiceTitle = .getValue("serviceErrorTitle")
                DirectCast(Me.Master, AjaxPopup).ServiceNopermission = .getValue("nopermission")
            End If
            .setHyperLink(HYPback, True, True)
            .setHyperLink(HYPbackToList, True, True)
            .setButton(BTNlogin, True, , , True)
        End With
    End Sub
    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region

    Public Sub InitializeView(ByVal FolderID As Long, ByVal CommunityID As Integer) Implements IViewCommunityRepositoryError.LoadRepositoryPage
        Dim BaseRepositoryUrl As String = "FolderID=" & FolderID.ToString & "&CommunityID=" & CommunityID.ToString
        If PreLoadedContentView <> ContentView.viewAll Then
            BaseRepositoryUrl &= "&CV=" & CInt(PreLoadedContentView)
        End If
        Dim BaseBackToList As String = ""
        Dim BaseBackToRetry As String = ""
        Select Case Me.PreloadedPageSender
            Case RepositoryPage.SingleUploadPage
                BaseBackToRetry = RootObject.RepositoryCreateItem(FolderID, CommunityID, CurrentItemRepositoryToCreate, Me.PreloadedPreviousView.ToString, Me.PreloadedPreviousPage.ToString, PreLoadedContentView)
                '"Modules/Repository/CommunityRepositoryUpload.aspx?" & BaseRepositoryUrl & "&Create=" & Me.PreloadedCreate.ToString & "&PreviousView=" & Me.PreloadedPreviousView.ToString & "&PreviousPage=" & Me.PreloadedPreviousPage.ToString
            Case RepositoryPage.MultipleUpload
                BaseBackToRetry = RootObject.RepositoryUploadMultipleFile(FolderID, CommunityID, Me.PreloadedPreviousView.ToString, Me.PreloadedPreviousPage.ToString, PreLoadedContentView)
                '"Modules/Repository/CommunityRepositoryMultipleUpload.aspx?" & BaseRepositoryUrl & "&PreviousView=" & Me.PreloadedPreviousView.ToString & "&PreviousPage=" & Me.PreloadedPreviousPage.ToString
            Case RepositoryPage.EditItem
                BaseBackToRetry = RootObject.RepositoryEdit(Me.CurrentItemID, FolderID, CommunityID, Me.PreloadedPreviousView.ToString, Me.PreloadedPreviousPage.ToString, PreLoadedContentView)
                '"Modules/Repository/CommunityRepositoryEdit.aspx?ItemID=" & Me.PreloadedItemID.ToString & "&" & BaseRepositoryUrl & "&Create=" & Me.PreloadedCreate.ToString & "&PreviousView=" & Me.PreloadedPreviousView.ToString & "&PreviousPage=" & Me.PreloadedPreviousPage.ToString
            Case RepositoryPage.EditItemPermission
                BaseBackToRetry = RootObject.RepositorySingleItemPermission(Me.CurrentItemID, FolderID, CommunityID, Me.PreloadedPreviousView.ToString, Me.PreloadedPreviousPage.ToString, PreLoadedContentView)
                '"Modules/Repository/CommunityRepositoryItemPermission.aspx?ItemID=" & Me.PreloadedItemID.ToString & "&" & BaseRepositoryUrl & "&Create=" & Me.PreloadedCreate.ToString & "&PreviousView=" & Me.PreloadedPreviousView.ToString & "&PreviousPage=" & Me.PreloadedPreviousPage.ToString
        End Select
        Select Case Me.PreloadedPreviousPage
            Case RepositoryPage.CommunityDiaryPage

            Case RepositoryPage.DownLoadPage
                BaseBackToList = "Modules/Repository/CommunityRepository.aspx?" & BaseRepositoryUrl & "&View=" & PreloadedPreviousView.ToString
            Case RepositoryPage.ManagementPage
                BaseBackToList = "Modules/Repository/CommunityRepositoryManagement.aspx?" & BaseRepositoryUrl & "&View=" & PreloadedPreviousView.ToString
            Case RepositoryPage.None
                BaseBackToList = "Modules/Repository/CommunityRepository.aspx?" & BaseRepositoryUrl & "&View=" & PreloadedPreviousView.ToString
        End Select

        If Me.PreserveDownloadUrl AndAlso Not IsNothing(Request.UrlReferrer) Then
            Me.PreservedDownloadUrl = Request.UrlReferrer.ToString()
        End If

        Me.HYPback.Visible = Not (Me.PreloadedPageSender = RepositoryPage.DownloadingPage)
        Me.HYPbackToList.Visible = Not (Me.PreloadedPageSender = RepositoryPage.DownloadingPage)
        Me.BTNlogin.Visible = (Me.CurrentFirstError = ItemRepositoryStatus.NotLoggedIn)
        If Me.PreloadedPageSender <> RepositoryPage.DownloadingPage Then
            Me.HYPback.NavigateUrl = Me.BaseUrl & BaseBackToRetry
            Me.HYPbackToList.NavigateUrl = Me.BaseUrl & BaseBackToList
            Me.HYPbackToList.NavigateUrl = Me.BaseUrl & BaseBackToRetry
        End If
    End Sub
    Public Sub SaveFilesToProcess() Implements IViewCommunityRepositoryError.SaveFilesToProcess
        Dim FileNames As List(Of String) = (From v As String In Me.Request.Form.Keys Where v.StartsWith("FILE_C_Name_") Select Me.Request.Form(v)).ToList
        Dim isFile As List(Of String) = (From v As String In Me.Request.Form.Keys Where v.StartsWith("FILE_C_isFile_") Select Me.Request.Form(v)).ToList
        Dim Status As List(Of String) = (From v As String In Me.Request.Form.Keys Where v.StartsWith("FILE_C_Status_") Select Me.Request.Form(v)).ToList
        Dim FolderId As List(Of String) = (From v As String In Me.Request.Form.Keys Where v.StartsWith("FILE_C_FolderId_") Select Me.Request.Form(v)).ToList
        Dim SavedFilePath As List(Of String) = (From v As String In Me.Request.Form.Keys Where v.StartsWith("FILE_C_SavedFilePath_") Select Me.Request.Form(v)).ToList
        Dim SavedName As List(Of String) = (From v As String In Me.Request.Form.Keys Where v.StartsWith("FILE_C_SavedName_") Select Me.Request.Form(v)).ToList
        Dim SavedExtension As List(Of String) = (From v As String In Me.Request.Form.Keys Where v.StartsWith("FILE_C_SavedExtension_") Select Me.Request.Form(v)).ToList

        Dim oFiles As New List(Of dtoRemoteFileToRename)
        For i As Integer = 0 To FileNames.Count - 1
            Try
                Dim oDto As New dtoRemoteFileToRename
                oDto.Id = System.Guid.NewGuid
                oDto.DisplayName = FileNames(i)
                oDto.FolderId = CLng(FolderId(i))
                oDto.isFile = CBool(isFile(i))
                oDto.SavedExtension = SavedExtension(i)
                oDto.SavedFilePath = SavedFilePath(i)
                oDto.SavedName = SavedName(i)
                oDto.Status = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ItemRepositoryStatus).GetByString(Status(i), ItemRepositoryStatus.None)
                oFiles.Add(oDto)
            Catch ex As Exception

            End Try
        Next
        Me.ViewState("FilesToProcess") = oFiles
    End Sub
    Public Sub InitViewToDisplay()
        Dim oFiles As List(Of dtoRemoteFileToRename) = Me.FilesToProcess

        Dim oFilesToRename As List(Of dtoFileExist(Of System.Guid))
        oFilesToRename = (From f In oFiles Where f.Status = ItemRepositoryStatus.FileExist OrElse f.Status = ItemRepositoryStatus.FolderExist _
                          Select New dtoFileExist(Of System.Guid) With {.Id = f.Id, .ExistFileName = f.DisplayName, .ProposedFileName = f.SavedName & "_" & Replace(Now.Date, "/", "-"), .Extension = f.SavedExtension}).ToList
        If oFilesToRename.Count > 0 AndAlso oFilesToRename.Count = oFiles.Count Then
            Me.MLVerrors.SetActiveView(VIWrename)
            Me.RPTfileName.DataSource = oFilesToRename
            Me.RPTfileName.DataBind()
        Else
            Me.MLVerrors.SetActiveView(VIWstandard)
            Me.LoadErrors()
        End If
    End Sub

    Public Sub LoadErrors()
        Dim oFiles As List(Of dtoRemoteFileToRename) = Me.FilesToProcess
        Me.LBfileError.Text = Me.Resource.getValue("ErrorGeneric") & vbCrLf

        If oFiles.Count > 0 Then
            Me.LBfileError.Text &= "<ul>"
        End If
        For Each oFile As dtoRemoteFileToRename In oFiles
            Dim FileName As String = ""
            If oFile.DisplayName = "" Then
                Me.LBfileError.Text &= "<li>" & Me.Resource.getValue("ItemRepositoryStatus." & oFile.Status.ToString) & "</li>"
            Else
                If oFile.isFile Then
                    FileName = "<img src='" & BaseUrl & Me.PageUtility.SystemSettings.Extension.FindIconImage(oFile.SavedExtension) & "'>&nbsp;" & oFile.DisplayName
                Else
                    FileName = "<img src='" & BaseTreeUrl & "folder.gif" & "'>&nbsp;" & oFile.DisplayName
                End If

                If String.IsNullOrEmpty(oFile.DisplayName) Then
                    Me.LBfileError.Text &= "<li>" & String.Format(Me.Resource.getValue("ItemRepositoryStatus"), "", Me.Resource.getValue("ItemRepositoryStatus." & oFile.Status.ToString)) & "</li>"
                Else
                    Me.LBfileError.Text &= "<li>" & String.Format(Me.Resource.getValue("ItemRepositoryStatus"), FileName, Me.Resource.getValue("ItemRepositoryStatus." & oFile.Status.ToString)) & "</li>"
                End If

            End If
            
         

        Next
        If oFiles.Count > 0 Then
            Me.LBfileError.Text &= "</ul>"
        End If

        'Dim FileNames As List(Of String) = (From v As String In Me.Request.Form.Keys Where v.StartsWith("FILE_C_Name_") Select Me.Request.Form(v)).ToList
        'Dim isFile As List(Of String) = (From v As String In Me.Request.Form.Keys Where v.StartsWith("FILE_C_isFile_") Select Me.Request.Form(v)).ToList
        'Dim Status As List(Of String) = (From v As String In Me.Request.Form.Keys Where v.StartsWith("FILE_C_Status_") Select Me.Request.Form(v)).ToList

        'Me.LBfileError.Text = Me.Resource.getValue("ErrorGeneric") & vbCrLf

        'If FileNames.Count > 0 Then
        '    Me.LBfileError.Text &= "<ul>"
        'End If
        'For i As Integer = 0 To FileNames.Count - 1

        '    Dim FileName As String = ""
        '    If isFile(i) Then
        '        FileName = "<img src='" & BaseUrl & Me.PageUtility.SystemSettings.Extension.FindIconImage(GetFileExtension(FileNames(i))) & "'>&nbsp;" & FileNames(i)
        '    Else
        '        FileName = "<img src='" & BaseTreeUrl & "folder.gif" & "'>&nbsp;" & FileNames(i)
        '    End If
        '    Me.LBfileError.Text &= "<li>" & String.Format(Me.Resource.getValue("ItemRepositoryStatus"), FileName, Me.Resource.getValue("ItemRepositoryStatus." & Status(i))) & "</li>"
        'Next
        'If FileNames.Count > 0 Then
        '    Me.LBfileError.Text &= "</ul>"
        'End If




    End Sub

    Private Function GetFileExtension(ByVal FileName As String) As String
        Return Right(FileName, Len(FileName) - InStrRev(FileName, ".") + 1)
    End Function
    'oRemotePost.Add("FILE_C_Name_" & index.ToString, oDto.File.Name)
    '              oRemotePost.Add("FILE_C_isFile_" & index.ToString, oDto.File.isFile)
    '              oRemotePost.Add("FILE_C_Status_" & index.ToString, oDto.Status.ToString)


#Region "Action"


    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_File.Codex)
    End Sub
#End Region


    Private Sub BTNlogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNlogin.Click
        Me.CurrentPresenter.LogintoSystem()
    End Sub

    Private Sub LoadStandardLoginPage() Implements IViewCommunityRepositoryError.LoadStandardLoginPage
        Me.RedirectToDefault()
    End Sub
    Private Sub LoadExternalProviderPage(ByVal url As String, ByVal idCommunity As Integer, ByVal idPerson As Integer, ByVal PersonLogin As String, ByVal PostLoadDownloadPage As String) Implements IViewCommunityRepositoryError.LoadExternalProviderPage
        Me.WriteLogoutAccessCookie(idCommunity, idPerson, PersonLogin, PostLoadDownloadPage, True, False)
        If String.IsNullOrEmpty(url) Then
            Me.RedirectToDefault()
        Else
            Response.Redirect(url)
        End If
    End Sub

    Private Sub LoadShibbolethLoginPage(ByVal idCommunity As Integer, ByVal idPerson As Integer, ByVal PersonLogin As String, ByVal PostLoadDownloadPage As String) Implements IViewCommunityRepositoryError.LoadShibbolethLoginPage
        Me.WriteLogoutAccessCookie(idCommunity, idPerson, PersonLogin, PostLoadDownloadPage, True, False)
        Me.PageUtility.RedirectToUrl(lm.Comol.Core.BaseModules.AuthenticationManagement.RootObject.ShibbolethLogin(False), SystemSettings.Login.isSSLloginRequired)
    End Sub

    Private Sub LoadInternalLoginPage(ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal PersonLogin As String, ByVal PostLoadDownloadPage As String) Implements IViewCommunityRepositoryError.LoadInternalLoginPage
        Me.WriteLogoutAccessCookie(CommunityID, PersonID, PersonLogin, PostLoadDownloadPage, True, False)
        Me.PageUtility.RedirectToUrl(lm.Comol.Core.BaseModules.AuthenticationManagement.RootObject.InternalLogin(False), SystemSettings.Login.isSSLloginRequired)
    End Sub
    Private Sub AutoRedirectToRemoteLogin(ByVal AuthenticationTypeID As Integer)
        Dim RemoteUrl As String = ""
        'Select Case AuthenticationTypeID
        '    Case Main.TipoAutenticazione.IOP
        '        RemoteUrl = (From o In Me.SystemSettings.UrlProviders Where o.ComolID = AuthenticationTypeID Select o.RemoteLogin).FirstOrDefault

        'End Select
        If RemoteUrl = "" Then
            RemoteUrl = Me.PageUtility.DefaultUrl
        End If
        Me.Response.Redirect(RemoteUrl, True)
    End Sub

    Public Sub LoadLanguage(ByVal oLanguage As lm.Comol.Core.DomainModel.Language) Implements IViewCommunityRepositoryError.LoadLanguage
        Dim oLingua As New Lingua(oLanguage.Id, oLanguage.Code) With {.Icona = oLanguage.Icon, .isDefault = oLanguage.isDefault}

        Me.OverloadLanguage(oLingua)
        Me.SetCultureSettings()
        Me.SetInternazionalizzazione()
    End Sub

#Region "Rename"
    Public ReadOnly Property FilesToProcess() As List(Of dtoRemoteFileToRename) Implements IViewCommunityRepositoryError.FilesToProcess
        Get
            If TypeOf Me.ViewState("FilesToProcess") Is List(Of dtoRemoteFileToRename) Then
                Return DirectCast(Me.ViewState("FilesToProcess"), List(Of dtoRemoteFileToRename))
            Else
                Return New List(Of dtoRemoteFileToRename)
            End If
        End Get
    End Property
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
#End Region

End Class