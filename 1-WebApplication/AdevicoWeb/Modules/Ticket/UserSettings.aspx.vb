Imports lm.Comol.Core.BaseModules.Tickets.Domain
Imports lm.Comol.Core.DomainModel.Helpers

Public Class UserSettings
    Inherits PageBase
    'Implements IView
#Region "Context"
    'Definizion Presenter...

    'Private _presenter As Presenter
    'Private ReadOnly Property Presenter() As Presenter
    '    Get
    '        If IsNothing(_presenter) Then
    '            _presenter = New Presenter(Me.PageUtility.CurrentContext, Me)
    '        End If
    '        Return _presenter
    '    End Get
    'End Property
#End Region

#Region "Internal"
    'Property interne


#End Region

#Region "Implements"
    'Property della VIEW

#End Region

#Region "Inherits"
    'Property del PageBase

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

    Private Sub Add_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        'Me.Master.ShowDocType = True
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        'Binding => spostare in sezione corretta
        If Not Page.IsPostBack Then
            Me.UCtkSet.Bind()
        End If



    End Sub

#Region "Inherits"
    'Sub e Function PageBase

    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub BindNoPermessi()

    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UserSettings", "Modules", "Ticket")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLinkButton(LKBsaveTop, True, True, False, False)
            .setLinkButton(LKBsaveBot, True, True, False, False)

            .setLiteral(LTtitle)
            .setLiteral(LTpageTitle)
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    'Sub e function della View

    'Private Sub DisplaySessionTimeout() 'Implements IViewProjectGantt.DisplaySessionTimeout

    '    Dim idCommunity As Integer = 0 'PreloadIdCommunity
    '    Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
    '    Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
    '    dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

    '    dto.DestinationUrl = "" 'RootObject.Gantt(IdProject, PreloadIdCommunity, PreloadFromPage, PreloadIdContainerCommunity)

    '    If idCommunity > 0 Then
    '        dto.IdCommunity = idCommunity
    '    End If

    '    webPost.Redirect(dto)

    'End Sub

    'Public Overrides Sub ShowNoAccess()

    'End Sub

#End Region

#Region "Internal"
    'Sub e Function "della pagina"

#End Region

#Region "Handler"
    'Gestione eventi oggetti pagina (Repeater.ItemDataBound(), Button.Click, ...)

#End Region


    'Private Sub UCtkSet_SendUserMessages(Messages As IList(Of dtoMessage)) Handles UCtkSet.SendUserMessages
    '    If Not IsNothing(Messages) AndAlso Messages.Any() Then
    '        Me.UCactionMessages.Visible = True
    '        Me.UCactionMessages.InitializeControl(Messages)
    '    Else
    '        Me.UCactionMessages.Visible = False
    '    End If
    'End Sub

    Private Sub LKBsaveBot_Click(sender As Object, e As EventArgs) Handles LKBsaveBot.Click
        Save()
    End Sub
    Private Sub LKBsaveTop_Click(sender As Object, e As EventArgs) Handles LKBsaveTop.Click
        Save()
    End Sub

    Private Sub Save()
        'Me.LTtitle.Text = String.Format("Title - {0}", System.Convert.ToInt64(UCtkSet.settings.Settings).ToString())
        UCtkSet.Save()
    End Sub


    Private Sub UCtkSet_guiMessageHide() Handles UCtkSet.guiMessageHide
        Me.UCactionMessages.Visible = False
    End Sub

    Private Sub UCtkSet_guiMessagesSend(Messages As IList(Of dtoMessage)) Handles UCtkSet.guiMessagesShow
        Me.UCactionMessages.Visible = True
        Me.UCactionMessages.InitializeControl(Messages)
    End Sub

    'Public Shared Function CssVersion() As String
    '    Return TicketBase.CssVersion()
    'End Function

    Private _CssVersion As String = ""

    Public Function CssVersion() As String

        If String.IsNullOrEmpty(_CssVersion) Then
            Dim tkVerUC As UC_TicketCssVersion = LoadControl(BaseUrl & "/Modules/Ticket/UC/UC_TicketCssVersion.ascx")
            _CssVersion = tkVerUC.GetVersionString()
        End If

        Return _CssVersion
        'Return "?v=201507080935lm"
    End Function

    Private Sub UCtkSet_SessionTimeOut(CommunityId As Integer) Handles UCtkSet.SessionTimeOut
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

        dto.DestinationUrl = RootObject.SettingsUser(CommunityId)
        '"" 'RootObject.Gantt(IdProject, PreloadIdCommunity, PreloadFromPage, PreloadIdContainerCommunity)

        If CommunityId > 0 Then
            dto.IdCommunity = CommunityId
        End If

        webPost.Redirect(dto)
    End Sub
End Class