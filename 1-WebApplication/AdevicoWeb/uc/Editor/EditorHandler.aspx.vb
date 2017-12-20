Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.Editor
Imports lm.Comol.Core.BaseModules.Editor.Business
Imports PresentationLayer

Partial Public Class EditorHandler
    Inherits System.Web.UI.Page
#Region "Context"
    Private _Service As ServiceRepositoryContent
    Private _ServiceEditor As ServiceEditor
    Private _Person As Person
    Private _Community As Community
    Private _Utility As OLDpageUtility

    Private ReadOnly Property Utility As OLDpageUtility
        Get
            If IsNothing(_Utility) Then
                _Utility = New OLDpageUtility(Me.Context)
            End If
            Return _Utility
        End Get
    End Property


    Private ReadOnly Property CurrentService() As ServiceRepositoryContent
        Get
            If _Service Is Nothing Then
                _Service = New ServiceRepositoryContent(Utility.CurrentContext, Utility.CurrentContext.UserContext.CurrentUserID, Utility.CurrentContext.UserContext.CurrentCommunityID, "", "")
            End If
            Return _Service
        End Get
    End Property
#End Region
    ReadOnly Property Path() As String
        Get
            Return Request.QueryString("path")
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim oManager As New ManagerRepository(Me.UtenteCorrente, Me.ComunitaCorrente, False)
        If Me.Path = "" Then
            Exit Sub
        Else
            Dim item As EditorRepositoryItem = Nothing
            Try
                item = CurrentService.GetItem(New System.Guid(Path))
            Catch ex As Exception

            End Try


            If Not item Is Nothing Then
                Me.WriteFile(item, Response)
            End If
        End If
    End Sub

    Private Sub WriteFile(ByVal item As EditorRepositoryItem, ByVal response As HttpResponse)
        response.Buffer = True
        response.Clear()
        response.ContentType = item.MimeType
        Dim extension As String = item.Extension 'System.IO.Path.GetExtension(item.Name & item.Extension).ToLower()
        If extension <> ".htm" AndAlso extension <> ".html" AndAlso extension <> ".xml" AndAlso extension <> ".jpg" AndAlso extension <> ".gif" AndAlso extension <> ".png" Then
            response.AddHeader("content-disposition", "attachment; filename=" & item.Name & item.Extension)
        End If

        Try
            response.BinaryWrite(Me.CurrentService.GetItemContent(item, Utility.SystemSettings.BaseFileRepositoryPath.DrivePath))
        Catch ex As Exception

        End Try

        response.Flush()
        response.End()
    End Sub



    '	Inherits System.Web.UI.Page
    '	Implements IviewEditor

    '	Private _Presenter As PresenterEditor
    '	Public ReadOnly Property CurrentPresenter() As PresenterEditor
    '		Get
    '			If IsNothing(_Presenter) Then
    '				_Presenter = New PresenterEditor(Me)
    '			End If
    '			Return _Presenter
    '		End Get
    '	End Property
    '	ReadOnly Property Path() As String
    '		Get
    '			Return Request.QueryString("path")
    '		End Get
    '	End Property

    '	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '		'Dim oManager As New ManagerRepository(Me.UtenteCorrente, Me.ComunitaCorrente, False)
    '		If Me.Path = "" Then
    '			Exit Sub
    '		Else
    '			Dim oFile As LearningObjectFile = Me.CurrentPresenter.GetFile(Me.Path)
    '			Dim oContent() As Byte = Me.CurrentPresenter.GetContent(oFile)

    '			If Not oContent Is Nothing Then
    '				Me.WriteFile(oContent, oFile)
    '			End If
    '		End If
    '	End Sub


    '	Private Sub WriteFile(ByVal content() As Byte, ByVal oFile As LearningObjectFile)
    '		Response.Buffer = True
    '		Response.Clear()
    '		Response.ContentType = oFile.ContentType.Type
    '		Response.AddHeader("content-disposition", "attachment; filename=" + oFile.CompleteName)
    '		Response.BinaryWrite(content)
    '		Response.Flush()
    '		Response.End()
    '	End Sub


    '	Public ReadOnly Property CurrentCommunity() As Comol.Entity.Community Implements PresentationLayer.IviewEditor.CurrentCommunity
    '		Get
    '			Dim oOldPageUtility As OLDpageUtility = New OLDpageUtility(Me.Context)
    '			If oOldPageUtility.WorkingCommunityID = 0 Then
    '				Return Nothing
    '			Else
    '				Return New Community(oOldPageUtility.ComunitaCorrente.Id, oOldPageUtility.ComunitaCorrente.Nome, oOldPageUtility.ComunitaCorrente.IdPadre)
    '			End If
    '		End Get
    '	End Property
    '	Public ReadOnly Property CurrentUser() As Comol.Entity.Person Implements PresentationLayer.IviewEditor.CurrentUser
    '		Get
    '			Dim oOldPageUtility As OLDpageUtility = New OLDpageUtility(Me.Context)
    '			If oOldPageUtility.CurrentUser Is Nothing Then
    '                Return oOldPageUtility.AnonymousPerson
    '			Else
    '				Return New Person(oOldPageUtility.CurrentUser.ID, oOldPageUtility.CurrentUser.Nome, oOldPageUtility.CurrentUser.Cognome)
    '			End If
    '		End Get
    '	End Property
    '	Public ReadOnly Property UserLanguage() As Comol.Entity.Lingua Implements PresentationLayer.IviewEditor.UserLanguage
    '		Get
    '			Dim oOldPageUtility As OLDpageUtility = New OLDpageUtility(Me.Context)
    '			Return oOldPageUtility.UserSessionLanguage
    '		End Get
    '	End Property


    '#Region "inusati"
    '	Public Property AutoScrollingSpeed() As Comol.Entity.ScrollingSpeed Implements PresentationLayer.IviewEditor.AutoScrollingSpeed
    '		Get

    '		End Get
    '		Set(ByVal value As Comol.Entity.ScrollingSpeed)

    '		End Set
    '	End Property

    '	Public Property EditorHTML() As String Implements PresentationLayer.IviewEditor.HTML
    '		Get

    '		End Get
    '		Set(ByVal value As String)

    '		End Set
    '	End Property
    '	Public Property EditorMaxChar() As Long Implements PresentationLayer.IviewEditor.EditorMaxChar
    '		Get

    '		End Get
    '		Set(ByVal value As Long)

    '		End Set
    '	End Property
    '	Public ReadOnly Property EditorText() As String Implements PresentationLayer.IviewEditor.Text
    '		Get

    '		End Get
    '	End Property
    '	Public Property ImagesPaths() As String() Implements PresentationLayer.IviewEditor.ImagesPaths
    '		Get

    '		End Get
    '		Set(ByVal value As String())

    '		End Set
    '	End Property
    '	Public WriteOnly Property ShowAdvancedControlsImage() As Boolean Implements PresentationLayer.IviewEditor.ShowAddImage
    '		Set(ByVal value As Boolean)

    '		End Set
    '	End Property
    '	Public Property ShowAddSmartTag() As Boolean Implements PresentationLayer.IviewEditor.ShowAddSmartTag
    '		Get
    '			If Me.ViewState("ShowAddSmartTag") = "" Then
    '				Return False
    '			Else
    '				Return Me.ViewState("ShowAddSmartTag")
    '			End If
    '		End Get
    '		Set(ByVal value As Boolean)
    '			Me.ViewState("ShowAddSmartTag") = value
    '		End Set
    '	End Property
    '	Public Property ShowScrollingSpeed() As Boolean Implements PresentationLayer.IviewEditor.ShowScrollingSpeed
    '		Get

    '		End Get
    '		Set(ByVal value As Boolean)

    '		End Set
    '	End Property
    '	Public ReadOnly Property CustomDialogScript() As String Implements PresentationLayer.IviewEditor.CustomDialogScript
    '		Get

    '		End Get
    '	End Property
    '	Public WriteOnly Property AllowPreview() As Boolean Implements PresentationLayer.IviewEditor.AllowPreview
    '		Set(ByVal value As Boolean)

    '		End Set
    '	End Property

    '	Public Property FontNames() As String Implements PresentationLayer.IviewEditor.FontNames
    '		Get

    '		End Get
    '		Set(ByVal value As String)

    '		End Set
    '	End Property

    '	Public Property FontSizes() As String Implements PresentationLayer.IviewEditor.FontSizes
    '		Get

    '		End Get
    '		Set(ByVal value As String)

    '		End Set
    '	End Property

    '	Public WriteOnly Property ShowAdvancedControlsDocument() As Boolean Implements PresentationLayer.IviewEditor.ShowAddDocument
    '		Set(ByVal value As Boolean)

    '		End Set
    '	End Property
    '	Public Property EditorEnabled() As Boolean Implements PresentationLayer.IviewEditor.EditorEnabled
    '		Get

    '		End Get
    '		Set(ByVal value As Boolean)

    '		End Set
    '	End Property
    '#End Region


    '	Public Property DisabledTags() As String Implements PresentationLayer.IviewEditor.DisabledTags
    '		Get

    '		End Get
    '		Set(ByVal value As String)

    '		End Set
    '	End Property

    '	Public Sub SetAdvancedTools(ByVal oList As System.Collections.Generic.List(Of Comol.Entity.SmartTag)) Implements PresentationLayer.IviewEditor.SetAdvancedTools

    '	End Sub
End Class