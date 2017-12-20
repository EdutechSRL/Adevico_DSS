Imports System
Imports System.Web
Imports System.Drawing.Imaging
Imports System.Drawing

Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.UI.Presentation


Public Class HTTPscormModule
	Implements System.Web.IHttpModule
	Implements iHttpModuleComol
	Implements IviewBaseFileDownload




	Dim WithEvents _Application As HttpApplication = Nothing
	Public Sub Dispose() Implements System.Web.IHttpModule.Dispose

	End Sub
	Public Sub Init(ByVal context As System.Web.HttpApplication) Implements System.Web.IHttpModule.Init
		_Application = context
	End Sub

	Private Sub _Application_AcquireRequestState(ByVal sender As Object, ByVal e As System.EventArgs) Handles _Application.AcquireRequestState
		If Me._Application.Request.Url.Query.StartsWith("?404;") Then

		End If
		'Dim oGeneralModule As New HTTPmodule_Utility(Me._Application.Context)

		'If Not IsNothing(oGeneralModule) Then
		'	If isFileIconModule(oGeneralModule) Then
		'		Me.ParseFileNameToIconImage(oGeneralModule)
		'	Else
		' Errore generato da mancanza pagina...
		Dim oModule As New HTTPmodule_Utility(Me._Application.Context, FileSettings.ConfigType.Scorm)

		If Me.isCorrectModule(oModule) Then
			oModule.ClearHTTPcontext()
			Me.DefineFileToDownload(oModule)
		End If
		'End If
		'	End If
		'ElseIf Me._Application.Session("StatusCode") = "999" Then
		'	Dim oGeneralModule As New HTTPmodule_Utility(Me._Application.Context)
		'	Me._Application.Session.Remove("StatusCode")
		'	Me._Application.Response.Redirect(oGeneralModule.BaseUrl & "generici/FileTooLong.aspx")

		'End If
	End Sub

	Public Sub CreateFileIconImage(ByVal sender As Object, ByVal e As EventArgs) Handles _Application.AcquireRequestState
		If Me._Application.Request.Url.Query.StartsWith("?404;") Then
			Dim oGeneralModule As New HTTPmodule_Utility(Me._Application.Context)

			If Not IsNothing(oGeneralModule) Then
				If isFileIconModule(oGeneralModule) Then
					Me.ParseFileNameToIconImage(oGeneralModule)
				ElseIf isFileStore AndAlso Me.FileID <> System.Guid.Empty Then
					Me.FileRepositoryPresenter.ModuleDownloadFile(Me.FileID, PageUtility.BaseUserRepositoryPath)
				End If
			End If
			'ElseIf Me.CurrentSize > Me.GetMaxRequestLength() Then
			'	Dim oGeneralModule As New HTTPmodule_Utility(Me._Application.Context)

			'	Me._Application.Response.StatusCode = 999
			'	Me._Application.Response.End()

			'Me._Application.Server.Transfer(oGeneralModule.BaseUrl & "Generici/FileTooLarge.aspx")
			'	Me._Application.Response.Redirect(oGeneralModule.BaseUrl & "Generici/FileTooLarge.aspx")
		End If
	End Sub

#Region "Vecchia logica"
	Public Sub DefineFileToDownload(ByVal oModule As HTTPmodule_Utility) Implements iHttpModuleComol.DefineFileToDownload
		Dim RequestUrlBase As String = ""
		If _Application.Request.Url.Query.Contains("?404;") Then
			RequestUrlBase = Replace(_Application.Request.Url.Query, "?404;", "")
			RequestUrlBase = RequestUrlBase.Substring(RequestUrlBase.IndexOf(oModule.ConfigFilePath.RewritePath) + oModule.ConfigFilePath.RewritePath.Length)
			RequestUrlBase = oModule.ConfigFilePath.Drive & _Application.Server.UrlDecode(RequestUrlBase)
		ElseIf _Application.Request.Url.AbsolutePath.Contains(oModule.BaseUrl & oModule.ConfigFilePath.RewritePath) Then
			RequestUrlBase = Replace(_Application.Request.Url.AbsolutePath, oModule.BaseUrl & oModule.ConfigFilePath.RewritePath, "")
			RequestUrlBase = oModule.ConfigFilePath.Drive & _Application.Server.UrlDecode(RequestUrlBase)
		End If
		If RequestUrlBase = "" Then
			Try
				SendToErrorPage(ErrorSettings.ErrorType.NoScormFile, oModule)
			Catch ex As Exception

			End Try
		Else
			Try
                'Dim oFileInfo As System.IO.FileInfo = lm.Comol.Core.File.ContentOf.File_Info(RequestUrlBase)
                If lm.Comol.Core.File.Exists.File(RequestUrlBase) Then
                    oModule.DownloadFile(0, RequestUrlBase)
                Else
                    SendToErrorPage(ErrorSettings.ErrorType.NoScormFile, oModule)
                End If
			Catch ex As Exception

			End Try
		End If
	End Sub

	Public Function isFileIconModule(ByVal oModule As HTTPmodule_Utility) As Boolean
		Dim RequestUrlBase As String = Replace(_Application.Request.Url.Query, "?404;", "")

		Return RequestUrlBase.EndsWith(oModule.SystemSettings.Extension.ExtensionToShow)
	End Function
	Public Sub ParseFileNameToIconImage(ByVal oModule As HTTPmodule_Utility)
		Dim RequestUrlBase As String = Replace(_Application.Request.Url.Query, "?404;", "")
		If RequestUrlBase <> "" Then
			RequestUrlBase = _Application.Server.UrlDecode(RequestUrlBase)
		End If
		Dim IconName As String = RequestUrlBase.Substring(RequestUrlBase.LastIndexOf("/") + 1)
		IconName = "." & Replace(IconName, oModule.SystemSettings.Extension.ExtensionToShow, "")
		Dim IconFileName As String = ""

		IconFileName = oModule.BaseUrlDrivePath & oModule.SystemSettings.Extension.FindIconImage(IconName)

		oModule.ClearHTTPcontext()
        oModule.DownloadFile(0, IconFileName)

	End Sub
	Public Function isCorrectModule(ByVal oModule As HTTPmodule_Utility) As Boolean Implements iHttpModuleComol.isCorrectModulel
		If _Application.Request.Url.Query.Contains("?404;") Then
			Dim RequestUrlBase As String = Replace(_Application.Request.Url.Query, "?404;", "")
			RequestUrlBase = RequestUrlBase.Substring(RequestUrlBase.IndexOf(oModule.BaseUrl) + oModule.BaseUrl.Length)
			If RequestUrlBase.StartsWith(oModule.ConfigFilePath.RewritePath) Then
				If oModule.CurrentCommunityID = -1 Or oModule.CurrentCommunityID < 0 Then
					Return False
				Else
					Return True
				End If
			Else
				Return False
			End If
		ElseIf _Application.Request.Url.AbsolutePath.Contains(oModule.BaseUrl & oModule.ConfigFilePath.RewritePath) Then
			'If oModule.ComunitaID = -1 Or oModule.ComunitaID < 0 Then
			'    Return False
			'Else
			Return True
			'End If
		Else
			Return False
		End If
	End Function

	Private Function GetSettings() As ComolSettings
		Dim oGeneralModule As New HTTPmodule_Utility(Me._Application.Context)

		Dim oSettings As ComolSettings = oGeneralModule.SystemSettings
		Return oSettings
	End Function
	Public ReadOnly Property LocalizedMail() As MailLocalized
		Get
			Try
				Return ManagerConfiguration.GetMailLocalized(Nothing)
			Catch ex As Exception

			End Try
			Return Nothing
		End Get
	End Property

	Private Sub _Application_Error(ByVal sender As Object, ByVal e As System.EventArgs) Handles _Application.Error
		Dim oGeneralModule As New HTTPmodule_Utility(Me._Application.Context)

		If oGeneralModule.SystemSettings.OnErrorShow404 Then
            Me._Application.Response.Redirect(oGeneralModule.BaseUrl & lm.Comol.Core.BaseModules.Errors.Domain.RootObject.Default((oGeneralModule.CurrentUserID > 0), False, False, True, True))
		End If
	End Sub



	Private ReadOnly Property CurrentSize() As Int64
		Get
			Return Me._Application.Request.ContentLength / 1024
		End Get
	End Property

	Private Function GetMaxRequestLength() As Int64
		Dim iResponse As Int64 = 51200

		Try
			Dim oSection As HttpRuntimeSection
			oSection = ConfigurationManager.GetSection("system.web/httpRuntime")
			If Not IsNothing(oSection) Then
				Return oSection.MaxRequestLength
			End If
		Catch ex As Exception

		End Try
		Return iResponse
	End Function

	Private Sub SendToErrorPage(ByVal oType As ErrorSettings.ErrorType, ByVal oModule As HTTPmodule_Utility, Optional ByVal PercorsoFile As String = "")
		If oType = ErrorSettings.ErrorType.NoParameters Then
            oModule.DownloadErrorMessage(oType, True)
		Else
			Dim oMessage As String = "Errore nel recupero file pacchetto scorm: " & vbCrLf
			oMessage &= "ComunitaID=" & oModule.CurrentCommunityID & vbCrLf
			oMessage &= "PersonaID=" & oModule.CurrentUserID & vbCrLf & vbCrLf
			oMessage &= "PercorsoFile=" & PercorsoFile & vbCrLf & vbCrLf

			oModule.DownloadErrorMessage(oType, oMessage, True)
		End If

	End Sub
#End Region

#Region "Repository Download"
	Private _FileID As System.Guid
	Private _FileName As String
	Private _PageUtility As OLDpageUtility
	Private _RequestUrlBase As String
	Private _FileRepositoryPresenter As BaseFilePresenter
	Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext

	Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
		Get
			If IsNothing(_CurrentContext) Then
				_CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
			End If
			Return _CurrentContext
		End Get
	End Property

    Private ReadOnly Property FileID() As System.Guid Implements IviewBaseFileDownload.BaseFileID
        Get
            Return _FileID
        End Get
    End Property
	Private Property FileName() As String Implements IviewBaseFileDownload.FileName
		Get
			Return _FileName
		End Get
		Set(ByVal value As String)
			_FileName = value
		End Set
	End Property
	Private ReadOnly Property PageUtility() As OLDpageUtility
		Get
			If IsNothing(_PageUtility) Then
				_PageUtility = New OLDpageUtility(Me._Application.Context)
			End If
			Return _PageUtility
		End Get
	End Property
	Private ReadOnly Property RequestUrlBase() As String
		Get
			Return Replace(_Application.Request.Url.Query, "?404;", "")
		End Get
	End Property
	Public ReadOnly Property FileRepositoryPresenter() As BaseFilePresenter
		Get
			If IsNothing(_FileRepositoryPresenter) Then
				_FileRepositoryPresenter = New BaseFilePresenter(Me.CurrentContext, Me)
			End If
			Return _FileRepositoryPresenter
		End Get
	End Property

	Private ReadOnly Property isFileStore() As Boolean
		Get
			Dim oUrlBase As String = RequestUrlBase
			Dim Segments As String() = RequestUrlBase.Split("/")
			Dim TotSegemnts As Integer = Segments.Count - 1

			If Segments.Contains("FileStore") And TotSegemnts > 2 Then
				Try
					_FileID = New System.Guid(Segments(TotSegemnts - 1))
				Catch ex As Exception
					_FileID = System.Guid.Empty
				End Try
				_FileName = Segments(TotSegemnts)

				If _FileID <> System.Guid.Empty And _FileName <> "" Then
					Return True
				End If
			End If
			Return False
		End Get
	End Property
	Public Sub WriteFile(ByVal content() As Byte, ByVal FileName As String, ByVal ContentType As String) Implements lm.Comol.Modules.Base.Presentation.IviewBaseFileDownload.WriteFile
		'Dim oModule As New HTTPmodule_Utility(Me._Application.Context)
		'Dim oFileInfo As New System.IO.FileInfo(RequestUrlBase)
		'If oFileInfo.Exists Then
		'	oModule.DownloadFile(RequestUrlBase)
		'Else
		'	SendToErrorPage(ErrorSettings.ErrorType.NoFile, oModule)
		'End If

		'Response.Buffer = True
		'Response.Clear()
		'Response.ContentType = ContentType
		'Response.AddHeader("content-disposition", "attachment; filename=" + FileName)
		'Response.BinaryWrite(content)
		'Response.Flush()
		'Response.End()
	End Sub
	Public Sub DownloadFile(ByVal FileName As String, ByVal oBasefile As lm.Comol.Core.DomainModel.BaseFile) Implements lm.Comol.Modules.Base.Presentation.IviewBaseFileDownload.DownloadFile
		Dim oModule As New HTTPmodule_Utility(Me._Application.Context)
        'Dim oFileInfo As System.IO.FileInfo = lm.Comol.Core.File.ContentOf.File_Info(FileName)
        If lm.Comol.Core.File.Exists.File(FileName) Then
            oModule.DownloadFile(0, FileName, oBasefile, _Application.Request.Url.Query)
        Else
            SendToErrorPage(ErrorSettings.ErrorType.NoFile, oModule)
        End If
	End Sub
#End Region


End Class