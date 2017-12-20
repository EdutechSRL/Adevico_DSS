Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.Base.Presentation

Partial Public Class _404
	Inherits System.Web.UI.Page
	'	Implements IviewBaseFileDownload




	'#Region "Property"
	'	Private _Presenter As BaseFilePresenter
	'	Private _FileID As System.Guid
	'	Private _FileName As String
	'	Private _PageUtility As OLDpageUtility

	'	Public ReadOnly Property CurrentPresenter() As BaseFilePresenter
	'		Get
	'			If IsNothing(_Presenter) Then
	'				_Presenter = New BaseFilePresenter(Me)
	'			End If
	'			Return _Presenter
	'		End Get
	'	End Property
	'	Private ReadOnly Property isFileStore() As Boolean
	'		Get
	'			Dim TotSegemnts As Integer = Me.Request.Url.Segments.Count - 1
	'			If Me.Request.Url.IsFile AndAlso Me.Request.Url.Segments.Contains("FileStore/") And TotSegemnts > 2 Then
	'				Try
	'					_FileID = New System.Guid(Me.Request.Url.Segments(TotSegemnts - 1))
	'				Catch ex As Exception
	'					_FileID = System.Guid.Empty
	'				End Try
	'				_FileName = Me.Request.Url.Segments.Last

	'				If _FileID <> System.Guid.Empty And _FileName <> "" Then
	'					Return True
	'				End If
	'			End If
	'			Return False
	'		End Get
	'	End Property
	'	Private ReadOnly Property FileID() As System.Guid Implements IviewBaseFileDownload.FileID
	'		Get
	'			Return _FileID
	'		End Get
	'	End Property
	'	Private Property FileName() As String Implements IviewBaseFileDownload.FileName
	'		Get
	'			Return _FileName
	'		End Get
	'		Set(ByVal value As String)
	'			_FileName = value
	'		End Set
	'	End Property
	'	Private ReadOnly Property PageUtility() As OLDpageUtility
	'		Get
	'			If IsNothing(_PageUtility) Then
	'				_PageUtility = New OLDpageUtility(Me.Context)
	'			End If
	'			Return _PageUtility
	'		End Get
	'	End Property
	'#End Region

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		'If isFileStore Then
		'	Me.CurrentPresenter.ModuleDownloadFile(Me.FileID, PageUtility.BaseUserRepositoryPath)
		'Else
		If Me.Request.QueryString("errore") = "true" Then
			Me.LBmessaggio.Text = "Errore nell'esecuzione della pagina <br> Contattare l'amministratore di sistema"
		Else
			Me.LBmessaggio.Text = "File non trovato"
		End If
		'End If
	End Sub

	'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
	'	'Dim oManager As New ManagerRepository(Me.UtenteCorrente, Me.ComunitaCorrente, False)
	'	If Me.Path = "" Then
	'		Exit Sub
	'	Else
	'		Dim oFile As LearningObjectFile = Me.CurrentPresenter.GetFile(Me.Path)
	'		Dim oContent() As Byte = Me.CurrentPresenter.GetContent(oFile)

	'		If Not oContent Is Nothing Then
	'			Me.WriteFile(oContent, oFile)
	'		End If
	'	End If
	'End Sub


	'Private Sub WriteFile(ByVal content() As Byte, ByVal oFile As LearningObjectFile)
	'	Response.Buffer = True
	'	Response.Clear()
	'	Response.ContentType = oFile.ContentType.Type
	'	Response.AddHeader("content-disposition", "attachment; filename=" + oFile.CompleteName)
	'	Response.BinaryWrite(content)
	'	Response.Flush()
	'	Response.End()
	'End Sub
End Class