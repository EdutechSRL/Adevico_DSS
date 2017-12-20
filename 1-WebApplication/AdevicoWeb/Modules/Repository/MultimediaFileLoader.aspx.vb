Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Repository
Imports lm.Comol.Core.BaseModules.Repository.Presentation
Imports lm.Comol.UI.Presentation

Public Class MultimediaFileLoader
    Inherits PageBase
    Implements IViewFilePlayer

#Region "Context"
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _BaseUrl As String
    Private _Presenter As FilePlayerPresenter

    Private ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Private ReadOnly Property CurrentPresenter() As FilePlayerPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New FilePlayerPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
#Region "SavedData"
    Public Property PlayerFileUniqueID As System.Guid Implements IViewFilePlayer.PlayerFileUniqueID
        Get
            Return ViewStateOrDefault("PlayerFileUniqueID", System.Guid.Empty)
        End Get
        Set(value As System.Guid)
            ViewState("PlayerFileUniqueID") = value
        End Set
    End Property
    Public Property PlayerIdCommunity As Integer Implements IViewFilePlayer.PlayerIdCommunity
        Get
            Return ViewStateOrDefault("PlayerIdCommunity", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("PlayerIdCommunity") = value
        End Set
    End Property
    Public Property PlayerIdFile As Long Implements IViewFilePlayer.PlayerIdFile
        Get
            Return ViewStateOrDefault("PlayerIdFile", CLng(0))
        End Get
        Set(value As Long)
            ViewState("PlayerIdFile") = value
        End Set
    End Property
    Public Property PlayerIdLink As Long Implements IViewFilePlayer.PlayerIdLink
        Get
            Return ViewStateOrDefault("PlayerIdLink", CLng(0))
        End Get
        Set(value As Long)
            ViewState("PlayerIdLink") = value
        End Set
    End Property
    Public Property PlayerIdModule As Integer Implements IViewFilePlayer.PlayerIdModule
        Get
            Return ViewStateOrDefault("PlayerIdModule", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("PlayerIdModule") = value
        End Set
    End Property
    Public Property PlayerItemTypeID As String Implements IViewFilePlayer.PlayerItemTypeID
        Get
            Return ViewStateOrDefault("PlayerItemTypeID", "")
        End Get
        Set(value As String)
            ViewState("PlayerItemTypeID") = value
        End Set
    End Property
    Public Property PlayerModuleCode As String Implements IViewFilePlayer.PlayerModuleCode
        Get
            Return ViewStateOrDefault("PlayerModuleCode", "")
        End Get
        Set(value As String)
            ViewState("PlayerModuleCode") = value
        End Set
    End Property
    Public ReadOnly Property PlayerWorkingSessionID As System.Guid Implements IViewFilePlayer.PlayerWorkingSessionID
        Get
            Return Me.PageUtility.UniqueGuidSession
        End Get
    End Property
    Protected Friend Property PlayUniqueSessionId As Guid Implements IViewFilePlayer.PlayUniqueSessionId
        Get
            Return ViewStateOrDefault("PlayUniqueSessionId", Guid.Empty)
        End Get
        Set(ByVal value As Guid)
            Me.ViewState("PlayUniqueSessionId") = value
        End Set
    End Property
    Protected Friend Property PlaySessionId As String Implements IViewFilePlayer.PlaySessionId
        Get
            Return ViewStateOrDefault("PlaySessionId", "")
        End Get
        Set(ByVal value As String)
            Me.ViewState("PlaySessionId") = value
        End Set
    End Property
    Public Property PlayerSavingStatistics As Boolean Implements IViewFilePlayer.PlayerSavingStatistics
        Get
            Return ViewStateOrDefault("PlayerSavingStatistics", True)
        End Get
        Set(value As Boolean)
            ViewState("PlayerSavingStatistics") = value
        End Set
    End Property
#End Region

#Region "QueryString"
    Public ReadOnly Property PreloadedFileUniqueID As System.Guid Implements IViewFilePlayer.PreloadedFileUniqueID
        Get
            Dim FileUniqueID As System.Guid
            If Not String.IsNullOrEmpty(Request.QueryString("FileUniqueID")) Then
                Try
                    FileUniqueID = New System.Guid(Request.QueryString("FileUniqueID"))
                Catch ex As Exception

                End Try
            Else
                FileUniqueID = Guid.Empty
            End If
            Return FileUniqueID
        End Get
    End Property

    Public ReadOnly Property PreloadedIdAction As Integer Implements IViewFilePlayer.PreloadedIdAction
        Get
            If IsNumeric(Request.QueryString("ActionID")) Then
                Return CLng(Request.QueryString("ActionID"))
            End If
            Return 0
        End Get
    End Property

    Public ReadOnly Property PreloadedIdCommunity As Integer Implements IViewFilePlayer.PreloadedIdCommunity
        Get
            If Not IsNumeric(Request.QueryString("CommunityID")) Then
                Return -1
            Else
                Return CInt(Request.QueryString("CommunityID"))
            End If
        End Get
    End Property

    Public ReadOnly Property PreloadedIdFile As Long Implements IViewFilePlayer.PreloadedIdFile
        Get
            If IsNumeric(Request.QueryString("FileID")) Then
                Return CLng(Request.QueryString("FileID"))
            ElseIf IsNumeric(Request.QueryString("Id")) Then
                Return CLng(Request.QueryString("Id"))
            ElseIf IsNumeric(Request.QueryString("IdFile")) Then
                Return CLng(Request.QueryString("IdFile"))
            End If
            Return 0
        End Get
    End Property

    Public ReadOnly Property PreloadedIdLink As Long Implements IViewFilePlayer.PreloadedIdLink
        Get
            If IsNumeric(Request.QueryString("LinkID")) Then
                Return CLng(Request.QueryString("LinkID"))
            End If
            Return 0
        End Get
    End Property

    Public ReadOnly Property PreloadedIdModule As Integer Implements IViewFilePlayer.PreloadedIdModule
        Get
            If Not IsNumeric(Request.QueryString("ModuleID")) Then
                Return 0
            Else
                Return CInt(Request.QueryString("ModuleID"))
            End If
        End Get
    End Property

    Public ReadOnly Property PreloadedIdUser As Integer Implements IViewFilePlayer.PreloadedIdUser
        Get
            Dim idUser As Integer = 0
            If IsNumeric(Request.QueryString("ForUserID")) Then
                idUser = CInt(Request.QueryString("ForUserID"))
            End If

            If idUser = 0 Then
                idUser = PageUtility.CurrentUser.ID
            End If
            Return idUser
        End Get
    End Property

    Public ReadOnly Property PreloadedItemTypeID As String Implements IViewFilePlayer.PreloadedItemTypeID
        Get
            If Not String.IsNullOrEmpty(Request.QueryString("ItemTypeID")) Then
                Return Request.QueryString("ItemTypeID")
            End If
            Return ""
        End Get
    End Property

    Public ReadOnly Property PreloadedLanguage As String Implements IViewFilePlayer.PreloadedLanguage
        Get
            If Not String.IsNullOrEmpty(Request.QueryString("Language")) Then
                Return Request.QueryString("Language")
            Else
                Return PageUtility.LinguaCode
            End If
        End Get
    End Property
    Public ReadOnly Property PreloadedSavingStatistics As Boolean Implements IViewFilePlayer.PreloadedSavingStatistics
        Get
            Dim notSaveStat As Boolean = False
            Boolean.TryParse(Request.QueryString("notSaveStat"), notSaveStat)

            Return Not notSaveStat
        End Get
    End Property
#End Region


#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return True
        End Get
    End Property
#End Region


    Public ReadOnly Property BaseUrl() As String
        Get
            Return Me.PageUtility.ApplicationUrlBase(True)
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Implementati "
    Public Overrides Sub BindDati()
        If Page.IsPostBack = False Then
            Me.CurrentPresenter.InitView(SystemSettings.PlattformId)
        End If
    End Sub
    Public Overrides Sub BindNoPermessi()

    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_MultimediaFileLoader", "Modules", "Repository")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Error Messages"
    Public Sub LoadFileNoPermission() Implements IViewFilePlayer.LoadFileNoPermission
        Me.MLVplayer.SetActiveView(VIWfileError)
        Me.LBfileError.Text = Resource.getValue("LoadFileNoPermission")
    End Sub
    Public Sub InvalidFileTypeToPlay(type As RepositoryItemType) Implements IViewFilePlayer.InvalidFileTypeToPlay
        Me.MLVplayer.SetActiveView(VIWfileError)
        Me.LBfileError.Text = Resource.getValue("InvalidType." & type.ToString)
    End Sub
    Public Sub LoadFileNoReadyToPlay(type As RepositoryItemType, status As TransferStatus) Implements IViewFilePlayer.LoadFileNoReadyToPlay
        Me.MLVplayer.SetActiveView(VIWfileError)
        Me.LBfileError.Text = Resource.getValue("NoReadyToPlay." & type.ToString)
        If status <> TransferStatus.Completed Then
            Dim statusInfo As String = Resource.getValue("TransferStatus." & status.ToString)
            If String.IsNullOrEmpty(statusInfo) Then
                Me.LBfileError.Text &= " " & statusInfo
            End If
        End If
    End Sub
    Public Sub LoadFileNotExist() Implements IViewFilePlayer.LoadFileNotExist
        Me.MLVplayer.SetActiveView(VIWfileError)
        Me.LBfileError.Text = Resource.getValue("LoadFileNotExist")
    End Sub
#End Region

    Private Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Not Page.IsPostBack Then
            Me.CurrentPresenter.InitializeContext()
        End If
    End Sub

    Public Sub LoadMultimediaFileIntoPlayer(pSessionId As String, WorkingSessionID As System.Guid, FileUniqueID As System.Guid, IdUser As Integer, IdLink As Long, IdFile As Long, defaultUrl As String) Implements IViewFilePlayer.LoadMultimediaFileIntoPlayer
        LoadFileIntoPlayer(pSessionId, WorkingSessionID, FileUniqueID, IdUser, IdLink, IdFile, RepositoryItemType.Multimedia, defaultUrl)
    End Sub

    Public Sub LoadFileIntoPlayer(pSessionId As String, WorkingSessionID As System.Guid, FileUniqueID As System.Guid, IdUser As Integer, IdLink As Long, IdFile As Long, type As RepositoryItemType) Implements IViewFilePlayer.LoadFileIntoPlayer
        LoadFileIntoPlayer(pSessionId, WorkingSessionID, FileUniqueID, IdUser, IdLink, IdFile, type, "")
    End Sub
    Private Sub LoadFileIntoPlayer(pSessionId As String, WorkingSessionID As System.Guid, FileUniqueID As System.Guid, IdUser As Integer, IdLink As Long, IdFile As Long, type As RepositoryItemType, fileUrl As String)
        Me.MLVplayer.SetActiveView(VIWplayer)
        Dim playerUrl As String = GetPlayerUrl(pSessionId, WorkingSessionID, IdFile, IdLink, FileUniqueID, IdUser, type, fileUrl)

        If RedirectToPage(type) Then
            'Dim oRemotePost As New lm.Comol.Modules.Base.DomainModel.RemotePost
            'oRemotePost.Url = playerUrl

            'oRemotePost.Post()
            Response.Redirect(playerUrl)
        Else
            With Me.LBplayerContainer
                .Text = "<iframe width='100%' id='IFR_ScomContainer' "
                .Text &= "src='"
                .Text &= playerUrl
                .Text &= "'"
                .Text &= " class='Invisible'></iframe>"
            End With


            Dim ActionUrl As String = Me.ApplicationUrlBase(True)
            If Not ActionUrl.EndsWith("/") Then
                ActionUrl &= "/"
            End If
            With Me.LBactionContainer
                .Text = "<iframe width='0%' height='0%' id='IFR_ActionContainer' "
                .Text &= "src='"
                '.Text &= ActionUrl & "Modules/Common/CommonActionSenderAjax.aspx?ForScorm=true&CommunityID=" & Me.PlayerCommunityID & "&ModuleID=" & Me.PlayerModuleID & "&WorkingSessionID=" & Me.PlayerWorkingSessionID.ToString & "&ActionID=" & Me.PlayerActionID & "&LinkID=" & LinkID.ToString & "&Id=" & FileUniqueID.ToString
                .Text &= ActionUrl & lm.Comol.Core.BaseModules.Repository.Domain.RootObject.AjaxScormStatistics(IdFile, Me.PlayerIdCommunity, Me.PlayerIdModule, PageUtility.UniqueGuidSession, Me.PlayerIdLink, IdLink, FileUniqueID)
                .Text &= "'"
                .Text &= " ></iframe>"
            End With
        End If
    End Sub

    Private Function GetPlayerUrl(pSessionId As String, WorkingSessionID As System.Guid, IdFile As Long, IdLink As Long, fileUniqueID As System.Guid, IdUser As Integer, type As RepositoryItemType, fileUrl As String) As String
        Dim result As String = ""
        Select Case type
            Case RepositoryItemType.ScormPackage
                result = ""
            Case RepositoryItemType.Multimedia
                result = (From c As RepositoryUrlConfiguration In Me.PageUtility.SystemSettings.Presenter.RepositoryConfiguration.UrlConfigurations Where c.RepositoryItemType = type Select c.PlayerUrl).FirstOrDefault
                If Not String.IsNullOrEmpty(result) Then
                    result &= "/" & fileUrl & IIf(fileUrl.Contains("?"), "&", "?WorkingSessionID") & "=" & WorkingSessionID.ToString & "&IdUser=" & IdUser.ToString & "&IdFile=" & IdFile.ToString & "&IdLink=" & IdLink.ToString
                End If
        End Select
        Return result
    End Function


    Public Function GetPermissionToLink(IdUser As Integer, IdLink As Long, file As BaseCommunityFile, IdCommunity As Integer) As Long Implements IViewFilePlayer.GetPermissionToLink
        Dim Permission As Long = 0
        Dim oSender As PermissionService.IServicePermission = Nothing
        Try
            oSender = PageUtility.PermissionService
            Permission = oSender.ModuleLinkActionPermission(IdLink, ActionType(file.RepositoryItemType), lm.Comol.Core.DomainModel.ModuleObject.CreateLongObject(file.Id, file, file.RepositoryItemType, IdCommunity, CoreModuleRepository.UniqueID), IdUser, GetExternalUsersLong(), Nothing)
            If Not IsNothing(oSender) Then
                Dim service As System.ServiceModel.ClientBase(Of PermissionService.IServicePermission) = DirectCast(oSender, System.ServiceModel.ClientBase(Of PermissionService.IServicePermission))
                service.Close()
                service = Nothing
            End If
        Catch ex As Exception
            If Not IsNothing(oSender) Then
                Dim service As System.ServiceModel.ClientBase(Of PermissionService.IServicePermission) = DirectCast(oSender, System.ServiceModel.ClientBase(Of PermissionService.IServicePermission))
                service.Abort()
                service = Nothing
            End If
        End Try
        Return Permission

    End Function
  
    Private Sub SendToSessionExpiredPage(idCommunity As Integer, languageCode As String) Implements IViewFilePlayer.SendToSessionExpiredPage
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.NewWindow
        If PageUtility.BaseUrl = "/" Then
            dto.DestinationUrl = Request.RawUrl.Replace("//", "/")
        Else
            dto.DestinationUrl = Request.RawUrl.Replace(PageUtility.BaseUrl, "")
        End If

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If
        webPost.Redirect(dto)
    End Sub

    Private Function ActionType(type As Repository.RepositoryItemType) As CoreModuleRepository.ActionType
        Select Case type
            Case RepositoryItemType.FileStandard
                Return CoreModuleRepository.ActionType.DownloadFile
            Case RepositoryItemType.Folder
                Return CoreModuleRepository.ActionType.ViewFolder
            Case RepositoryItemType.None
                Return CoreModuleRepository.ActionType.None
            Case Else
                Return CoreModuleRepository.ActionType.PlayFile
        End Select

    End Function


    Private Sub SaveLinkEvaluation(ByVal IdLink As Long, ByVal IdUser As Integer) Implements IViewFilePlayer.SaveLinkEvaluation

        Dim oSender As PermissionService.IServicePermission = Nothing
        Try
            oSender = New PermissionService.ServicePermissionClient
            If Not IsNothing(oSender) Then
                oSender.ExecutedAction(IdLink, True, True, 100, True, 100, IdUser)
                Dim service As System.ServiceModel.ClientBase(Of PermissionService.IServicePermission) = DirectCast(oSender, System.ServiceModel.ClientBase(Of PermissionService.IServicePermission))
                service.Close()
                service = Nothing
            End If
        Catch ex As Exception
            If Not IsNothing(oSender) Then
                Dim service As System.ServiceModel.ClientBase(Of PermissionService.IServicePermission) = DirectCast(oSender, System.ServiceModel.ClientBase(Of PermissionService.IServicePermission))
                service.Abort()
                service = Nothing
            End If
        End Try
    End Sub


    Public Function RedirectToPage(type As RepositoryItemType) As Boolean Implements IViewFilePlayer.RedirectToPage
        Dim result As Boolean = False
        Try
            result = (From c As RepositoryUrlConfiguration In Me.PageUtility.SystemSettings.Presenter.RepositoryConfiguration.UrlConfigurations Where c.RepositoryItemType = type Select c.RedirectToFilePage).FirstOrDefault
        Catch ex As Exception

        End Try
     
        Return result
    End Function
End Class