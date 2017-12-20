Imports TK = lm.Comol.Core.BaseModules.Tickets

Public Class UserSettingsExternal
    Inherits TicketBase


    Private Sub UserSettingsExternal_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit

        Me.Master.ShowDocType = True

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Me.CheckUser() Then
            Me.CTRLtopBar.LogOut()
        End If

        If Not Page.IsPostBack() Then

            'Dim qs As String = Request.QueryString("view")

            'If Not String.IsNullOrEmpty(qs) Then
            '    Select Case qs
            '        Case SettingsView.all.ToString()
            '            Me.PNLmail.Visible = True
            '            Me.PNLpwd.Visible = True
            '        Case SettingsView.mail.ToString()
            '            Me.PNLmail.Visible = True
            '            Me.PNLpwd.Visible = False
            '        Case SettingsView.mail.ToString()
            '            Me.PNLmail.Visible = False
            '            Me.PNLpwd.Visible = True
            '    End Select
            'End If

            Me.Master.Page_Title = Resource.getValue("Page.Title.External")
            Me.Master.BindSkin()


            Me.UCtkSet.ShowSave = True
            Me.UCtkSet.BindUser(Me.CurrentUser.UserId)
        
        'Me.CTRLmailSets.InitializeControl(Me.CurrentUser.UserId, -1, -1, True)

        End If

    End Sub

    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub BindNoPermessi()

    End Sub

    Public Overrides Sub DisplaySessionTimeout(CommunityId As Integer)

    End Sub

    Public Overrides Sub ShowNoAccess()

    End Sub

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UserSettings", "Modules", "Ticket")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLiteral(LTpageTitle_t)
            .setLiteral(LTcontentTitle_t)

            .setLiteral(LTpasswordTitle)

            '.setLabel(Me.LBnotificheMail_t)
            '.setLinkButton(LNBsetMail, True, True)
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

    'Public Sub SetParameter(ByVal MailSets As TK.Domain.Enums.MailSettings)
    '    'Me.CTRLmailSets.MailSettings = MailSets


    '    aaa()

    'End Sub


    Private ReadOnly Property CurrentUser As TK.Domain.DTO.DTO_User
        Get
            Dim Usr As New TK.Domain.DTO.DTO_User

            Try
                Usr = DirectCast(Session(TicketHelper.SessionExtUser), TK.Domain.DTO.DTO_User)
            Catch ex As Exception

            End Try

            Return Usr
        End Get
    End Property

    Private Function CheckUser() As Boolean
        If IsNothing(CurrentUser) OrElse CurrentUser.UserId <= 0 Then
            Return False
        End If

        Return True
    End Function

    'Private Sub LNBsetMail_Click(sender As Object, e As System.EventArgs) Handles LNBsetMail.Click
    '    Me.CTRLmailSets.SaveSettings()
    'End Sub

    'Public Enum SettingsView
    '    pwd
    '    mail
    '    all
    'End Enum
End Class