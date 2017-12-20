Imports lm.Comol.Core.BaseModules.NoticeBoard.Presentation
Imports lm.Comol.Core.BaseModules.NoticeBoard.Domain

Public Class DisplayNoticeBoardMessage
    Inherits NoticeboardPageBase
    Implements IViewDisplayMessage


#Region "Context"
    Private _Presenter As DisplayMessagePresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = lm.Comol.UI.Presentation.SessionHelpers.CurrentUserContext, .DataContext = lm.Comol.UI.Presentation.SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As DisplayMessagePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New DisplayMessagePresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region
#Region "View"
    Public ReadOnly Property PreloadedIdCommunity() As Integer Implements IViewDisplayMessage.PreloadedIdCommunity
        Get
            If IsNumeric(Request.QueryString("IdCommunity")) Then
                Return CInt(Request.QueryString("IdCommunity"))
            Else
                Return PageUtility.WorkingCommunityID
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadWorkingApplicationId() As Guid Implements IViewDisplayMessage.PreloadWorkingApplicationId
        Get
            If String.IsNullOrEmpty(Request.QueryString("waid")) Then
                Return Guid.Empty
            Else
                Try
                    Return New Guid(Request.QueryString("waid"))
                Catch ex As Exception
                    Return Guid.Empty
                End Try
            End If
        End Get
    End Property
    Public ReadOnly Property PreloadedIdMessage() As Long Implements IViewDisplayMessage.PreloadedIdMessage
        Get
            If Not IsNumeric(Request.QueryString("IdMessage")) Then
                Return 0
            Else
                Return CLng(Request.QueryString("IdMessage"))
            End If
        End Get
    End Property
    Public Property IdCurrentMessage As Long Implements IViewDisplayMessage.IdCurrentMessage
        Get
            Return ViewStateOrDefault("IdCurrentMessage", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdCurrentMessage") = value
        End Set
    End Property

    Public WriteOnly Property ContainerName As String Implements IViewDisplayMessage.ContainerName
        Set(value As String)
            If String.IsNullOrEmpty(value) Then
                Me.TLtitleCommunityName.Text = ""
            Else
                Dim translated As String = "Bacheca della comunita:{0}"

                Me.TLtitleCommunityName.Text = String.Format(translated, value)
            End If

        End Set
    End Property

    Public ReadOnly Property PortalName As String Implements IViewDisplayMessage.PortalName
        Get
            Return Me.Resource.getValue("PortalName")
        End Get
    End Property

    Private Function ViewStateOrDefault(Of T)(ByVal Key As String, ByVal DefaultValue As T) As T
        If (ViewState(Key) Is Nothing) Then
            ViewState(Key) = DefaultValue
            Return DefaultValue
        Else
            Return ViewState(Key)
        End If
    End Function
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Internal"
    Public ReadOnly Property SmartTagsAvailable() As Comol.Entity.SmartTags
        Get
            Return ManagerConfiguration.GetSmartTags(Me.ApplicationUrlBase(True))
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Me.CurrentPresenter.InitView(PageUtility.BaseUrlDrivePath & lm.Comol.Core.BaseModules.Editor.RootObject.ConfigurationFile(PageUtility.SystemSettings.EditorConfigurationPath), OLDpageUtility.ApplicationWorkingId)
    End Sub

    Public Overrides Sub BindNoPermessi()

    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_noticeboard", "Modules", "Noticeboard")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLiteral(Me.LTnoPermission)
            .setLiteral(Me.LTsessionTimeout)
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

#End Region

    Private Sub PreloadCssFiles(cssFilesPath As String, files As List(Of lm.Comol.Core.BaseModules.Editor.EditorCssFile)) Implements IViewDisplayMessage.PreloadCssFiles
        Dim linkUrl As String = "<link rel=""Stylesheet"" type=""text/css"" href=""{0}""/>"
        For Each f As lm.Comol.Core.BaseModules.Editor.EditorCssFile In files
            If f.isFullAddress Then
                LTpreloadCss.Text &= String.Format(linkUrl, f.FileName) & vbCrLf
            Else
                LTpreloadCss.Text &= String.Format(linkUrl, ApplicationUrlBase & Replace(cssFilesPath, "~", "") & f.FileName) & vbCrLf
            End If
        Next
    End Sub

    Private Sub DisplayMessage(message As NoticeboardMessage) Implements IViewDisplayMessage.DisplayMessage
        Dim text As String = message.Message

        If Not String.IsNullOrEmpty(text) Then
            If Not message.CreateByAdvancedEditor Then
                text = text.Replace(ControlChars.CrLf, "<br>")
            End If

            text = Replace(text, "<script", "&lt; script")
            text = Replace(text, "</script", "&lt;script")
            text = Replace(text, "<script>", "&lt;script&gt;")
            text = Replace(text, "</script>", "&lt;/script&gt;")
        Else
            text = ""
        End If

        If Not message.CreateByAdvancedEditor Then
            RenderStyle(message)
        End If
        Me.LTmessage.Text = SmartTagsAvailable.TagAll(text)
        Me.MLVdisplay.SetActiveView(VIWmessage)
    End Sub

    Private Sub DisplayNoPermission() Implements IViewDisplayMessage.DisplayNoPermission
        Me.MLVdisplay.SetActiveView(VIWnopermission)
    End Sub

    Private Sub DisplaySessionTimeout() Implements IViewDisplayMessage.DisplaySessionTimeout
        Me.MLVdisplay.SetActiveView(VIWsessionTimeout)
    End Sub

    Private Sub DisplayUnknownMessage() Implements IViewDisplayMessage.DisplayUnknownMessage
        Me.MLVdisplay.SetActiveView(VIWmessageError)
        Me.LTdisplay.Text = Resource.getValue("DisplayUnknownMessage")
    End Sub


    Private Sub DisplayEmptyMessage() Implements IViewDisplayMessage.DisplayEmptyMessage
        Me.MLVdisplay.SetActiveView(VIWmessageError)
        Me.LTdisplay.Text = Resource.getValue("DisplayEmptyMessage")
    End Sub

    'Dim oTitoloPaginaBacheca As String = Me.MessageTitoloBacheca
    'If oTitoloPaginaBacheca = "" Then
    '            oTitoloPaginaBacheca = "Bacheca della comunita:{0}"
    '        EndIf 

    Private Sub RenderStyle(message As NoticeboardMessage)
        If message.Style.Align <> "" Then
            BDdisplay.Style.Add("text-align", message.Style.Align)
        End If
        If message.Style.Color <> "" Then
            BDdisplay.Style.Add("color", message.Style.Color)
        End If
        If message.Style.Face <> "" Then
            BDdisplay.Style.Add("font-family", message.Style.Face)
        End If
        If message.Style.BackGroundColor <> "" Then
            BDdisplay.Style.Add("background-color", message.Style.BackGroundColor)
        End If
        If message.Style.Size <> 0 Then
            BDdisplay.Style.Add("font-size", RenderMessageStyleSize(message.Style.Size))
        End If
    End Sub

    Private Function RenderMessageStyleSize(ByVal Size As Integer) As String
        Select Case Size
            Case 1
                Return "8pt"
            Case 2
                Return "10pt"
            Case 3
                Return "12pt"
            Case 4
                Return "14pt"
            Case 5
                Return "18pt"
            Case 6
                Return "24pt"
            Case 7
                Return "36pt"
            Case Else
                Return "12pt"
        End Select
    End Function


   
End Class