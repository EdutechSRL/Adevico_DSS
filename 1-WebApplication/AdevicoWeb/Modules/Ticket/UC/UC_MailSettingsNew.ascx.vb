Imports NHibernate.Hql.Ast.ANTLR.Tree
Imports TK = lm.Comol.Core.BaseModules.Tickets

Public Class UC_MailSettingsNew
    Inherits BaseControl

    Public Event setToDefault()
    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    ''' <summary>
    ''' Inizializza il controllo
    ''' </summary>
    ''' <param name="showDefault">Mostra o nasconde il "Default"</param>
    ''' <param name="defaultSelect">Se Default è checkato</param>
    ''' <param name="value">Impostazioni</param>
    ''' <param name="userType">Tipo utente (manager o utente)</param>
    ''' <param name="isEnabled">SE è abilitato: per controllo a livello di sistema o utente</param>
    ''' <param name="showNewTicket">Mostra il parametro "nuovo ticket"</param>
    ''' <param name="showCreator">Mostra il parametro "modifica proprietario"</param>
    ''' <remarks></remarks>
    Public Sub BindSettings( _
                       ByVal showDefault As Boolean, _
                       ByVal defaultSelect As Boolean, _
                       ByVal value As TK.Domain.Enums.MailSettings, _
                       ByVal userType As TK.Domain.Enums.ViewSettingsUser, _
                       ByVal isEnabled As Boolean, _
                       ByVal showNewTicket As Boolean, _
                       ByVal showCreator As Boolean)
        'Optional ByVal showNewTicket As Boolean = False, _
        '               Optional ByVal showCreator As Boolean = False)

        Me.CBLsettings.Items.Clear()

        Dim li As New ListItem()
        Dim enabled As Boolean = Not (showDefault And defaultSelect) 'And isEnabled

        Me.CBXdefault.Visible = showDefault

        Me.CBXdefault.Checked = Not enabled

        Select Case userType
            Case TK.Domain.Enums.ViewSettingsUser.Manager

                If (showNewTicket) Then
                    Me.CBLsettings.Items.Add(getLI(TK.Domain.Enums.MailSettings.NewTicketManager, value)) ', enabled))
                End If

                Me.CBLsettings.Items.Add(getLI(TK.Domain.Enums.MailSettings.NewMessageManager, value)) ', enabled))
                'Me.CBLsettings.Items.Add(getLI(TK.Domain.Enums.MailSettings.TicketResetAssMan, value)) ', enabled))
                Me.CBLsettings.Items.Add(getLI(TK.Domain.Enums.MailSettings.TicketNewAssignmentMan, value)) ', enabled))
                Me.CBLsettings.Items.Add(getLI(TK.Domain.Enums.MailSettings.StatusChangedManager, value)) ', enabled))
                Me.CBLsettings.Items.Add(getLI(TK.Domain.Enums.MailSettings.ModerationChangedMan, value)) ', enabled))

            Case Else
                If (showNewTicket) Then
                    Me.CBLsettings.Items.Add(getLI(TK.Domain.Enums.MailSettings.NewTicketUsr, value)) ', enabled))
                End If
                Me.CBLsettings.Items.Add(getLI(TK.Domain.Enums.MailSettings.NewMessageUsr, value)) ', enabled))
                'Me.CBLsettings.Items.Add(getLI(TK.Domain.Enums.MailSettings.TicketResetAssUsr, value)) ', enabled))
                Me.CBLsettings.Items.Add(getLI(TK.Domain.Enums.MailSettings.StatusChangedUsr, value)) ', enabled))
                Me.CBLsettings.Items.Add(getLI(TK.Domain.Enums.MailSettings.ModerationChangedUsr, value)) ', enabled))

                If (showCreator) Then
                    Me.CBLsettings.Items.Add(getLI(TK.Domain.Enums.MailSettings.OwnerChanged, value)) ', enabled))
                End If

        End Select

        Me.CBLsettings.Enabled = enabled

        'Me.CBXdefault.Enabled = isEnabled

        'If Not isEnabled Then
        'Me.LBalternateText.Text = Resource.getValue("LBalternateText.ControlDisabled")
        'Me.LBalternateText.Visible = True

        'End If

        LBsendDisabled.Visible = Not isEnabled

        'Dim test As TK.Domain.Enums.MailSettings = TK.Domain.Enums.MailSettings.none
        'Test = TK.Domain.Enums.MailSettings.NewTicketUsr
        'Test = TK.Domain.Enums.MailSettings.NewMessageUsr
        'Test = TK.Domain.Enums.MailSettings.StatusChangedUsr
        'Test = TK.Domain.Enums.MailSettings.ModerationChangedUsr
        'Test = TK.Domain.Enums.MailSettings.TicketResetAssUsr
        'Test = TK.Domain.Enums.MailSettings.OwnerChanged
        'Test = TK.Domain.Enums.MailSettings.TicketNewAssignmentAss
        'Test = TK.Domain.Enums.MailSettings.NewTicketManager
        'Test = TK.Domain.Enums.MailSettings.NewMessageManager
        'Test = TK.Domain.Enums.MailSettings.StatusChangedManager
        'Test = TK.Domain.Enums.MailSettings.ModerationChangedMan
        'Test = TK.Domain.Enums.MailSettings.TicketAssCategoryMan
        'Test = TK.Domain.Enums.MailSettings.TicketNewAssignmentMan
        'Test = TK.Domain.Enums.MailSettings.TicketResetAssMan

    End Sub


    Private Function getLI(
                          ByVal itemValue As TK.Domain.Enums.MailSettings, _
                          ByVal settingsValue As TK.Domain.Enums.MailSettings) As ListItem ', _ ByVal enabled As Boolean

        Dim li As New ListItem()
        li.Value = System.Convert.ToInt64(itemValue).ToString()

        Dim text As String = Resource.getValue(String.Format("MailSettings.{0}", itemValue.ToString()))

        If Not String.IsNullOrEmpty(text) Then
            li.Text = text
        Else
            li.Text = itemValue.ToString()
        End If

        'li.Enabled = enabled
        
        If (itemValue And settingsValue) = itemValue Then
            li.Selected = True
        Else
            li.Selected = False
        End If

        li.Attributes.Add("class", "option")

        Return li

    End Function


    Public Function GetSettings() As TK.Domain.Enums.MailSettings

        Dim value As TK.Domain.Enums.MailSettings = TK.Domain.Enums.MailSettings.none

        If (CBXdefault.Checked) Then
            Return TK.Domain.Enums.MailSettings.Default
        End If

        For Each li As ListItem In Me.CBLsettings.Items
            If (li.Selected) Then

                Try
                    value = value Or [Enum].Parse(GetType(TK.Domain.Enums.MailSettings), li.Value)
                Catch ex As Exception
                End Try

            End If
        Next

        Return value

    End Function



#Region "BASECONTROL"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UserSettings", "Modules", "Ticket")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            Me.CBXdefault.Text = .getValue(String.Format("MailSettings.{0}", TK.Domain.Enums.MailSettings.Default.ToString()))
            .setLabel(LBsendDisabled)
        End With
    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get

        End Get
    End Property

#End Region

    Private Sub CBXdefault_CheckedChanged(sender As Object, e As EventArgs) Handles CBXdefault.CheckedChanged

        
        CBLsettings.Enabled = Not CBXdefault.Checked

        For Each li As ListItem In CBLsettings.Items
            li.Attributes.Add("class", "option")
        Next
        
        'Dim isDefault As Boolean = CBXdefault.Checked 'enabled

        'CBLsettings.Enabled = Not isDefault

        'If (isDefault) Then
        '    For Each li As ListItem In CBLsettings.Items
        '        li.Attributes.Add("class", "option disabled")
        '    Next
        'Else
        '    For Each li As ListItem In CBLsettings.Items
        '        li.Attributes.Add("class", "option")
        '    Next
        'End If

        ''Invio 
        'If (CBXdefault.Checked) Then
        '    RaiseEvent setToDefault()
        'End If

    End Sub

    Public Property LBalternate As Label
        Get
            Return Me.LBalternateText
        End Get
        Set(value As Label)
            If Not IsNothing(value) Then
                Me.LBalternate = value
            Else
                Me.LBalternate.Text = ""
            End If
        End Set
    End Property

    Public Property ShowAlternateText As Boolean
        Get
            Dim visible As Boolean = False
            Try
                visible = Me.ViewState("ShowAlternateText")
            Catch ex As Exception

            End Try

            Return visible
        End Get
        Set(value As Boolean)
            Me.ViewState("ShowAlternateText") = value
        End Set
    End Property

    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        'SOLO VISUALIZZAZIONE

        If (ShowAlternateText) AndAlso Me.CBXdefault.Checked AndAlso Not String.IsNullOrEmpty(LBalternate.Text) Then
            Me.CBLsettings.Visible = False
            Me.LBalternateText.Visible = True
        Else
            Me.CBLsettings.Visible = True
            Me.LBalternateText.Visible = False
        End If
    End Sub
End Class