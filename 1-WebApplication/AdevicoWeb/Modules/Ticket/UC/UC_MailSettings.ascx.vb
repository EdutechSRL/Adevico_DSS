Imports TK = lm.Comol.Core.BaseModules.Tickets

''' <summary>
''' Controllo che permette di gestire le impostazioni mail.
''' 
''' Può funzionare più modi:
''' 
''' - Contenitore:
'''     La pagina si arrangia in modo autonomo a scrivere e leggere i settings,
'''     tramite la proprietà "MailSettings". L'esempio è quello della modifica Ticket,
'''     (ora per retrocompatibilità), ma potrà essere usata anche in altri ambiti,
'''     come l'impostazione per TUTTI i Ticket (VEdi nota su "SetALL()")
''' 
''' - Contorllo Autonomo:
'''     Basta inizializzarlo con:
'''     "InitializeControl()" se è disponibile il Ticket User Id
'''     "InitializeControlForPerson()" per usare PersonId (COMOL) 
'''     ed i relativi parametri,
'''     tra cui "BindData" andrà settato a TRUE.
'''     Per il salvataggio dei dati usare "SaveSettings()"
''' 
''' - Modalità mista
'''     E' possibile usare il controllo per 
'''     - la sola lettura dei dati con "InitializeControl()"
'''         e gestire in modo autonomo i MailSettings
'''     - il solo salvataggio dei dati, fornendo i mail settings
'''         e salvandoli con "SaveSettings()".
'''         In QUESTO CASO, il controllo andrà COMUNQUE inizializzato con BindData a FALSE:
'''         in caso contrario i parametri NON saranno salvati.
''' 
''' - ESEMPIO - In config Utente
'''     Per usarlo nella pagina di configurazione utente (da portale),
'''     è possibile inserire il controllo,
'''     inizializzarlo con:
'''     CTRLticketMail.InitializeControlForPerson(PersonId, true)
'''     al momento del salvataggio, richiamare
'''     CTRLticketMail.SaveSettings()
'''     E' possibile controllare SE il controllo è stato inizializzato con ".IsInitialized"
''' </summary>
''' <remarks></remarks>
Public Class UC_MailSettings1
    Inherits BaseControl
    Implements TK.Presentation.View.iViewUcMailSets

#Region "Context"
    'Definizion Presenter...
    Private _presenter As TK.Presentation.UcMailSettingsPresenter

    Private ReadOnly Property CurrentPresenter() As TK.Presentation.UcMailSettingsPresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New TK.Presentation.UcMailSettingsPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _presenter
        End Get
    End Property
#End Region

#Region "Internal"

    Private Property UserId As Int64
        Get
            Return ViewStateOrDefault(Of Int64)("UserId", -1)
        End Get
        Set(value As Int64)
            ViewState("UserId") = value
        End Set
    End Property

    Private Property CommunityId As Integer
        Get
            Return ViewStateOrDefault(Of Int64)("CommunityId", -1)
        End Get
        Set(value As Integer)
            ViewState("CommunityId") = value
        End Set
    End Property

    Private Property TicketId As Int64
        Get
            Return ViewStateOrDefault(Of Int64)("TicketId", -1)
        End Get
        Set(value As Int64)
            ViewState("TicketId") = value
        End Set
    End Property

    Public Property IsInitialized As Boolean
        Get
            Return ViewStateOrDefault(Of Boolean)("IsInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("IsInitialized") = value
        End Set
    End Property



#End Region

#Region "Implements"

    Public Property ViewCommunityId As Integer Implements TK.Presentation.View.iViewBase.ViewCommunityId
        Get
            Return Me.CommunityId
        End Get
        Set(value As Integer)
            Me.CommunityId = value
        End Set
    End Property
    Public Property MailSettings As TK.Domain.Enums.MailSettings Implements TK.Presentation.View.iViewUcMailSets.MailSettings
        Get
            If Me.CBXdefault.Checked Then
                Return TK.Domain.Enums.MailSettings.Default
            End If

            Dim Sets As TK.Domain.Enums.MailSettings

            For Each li As ListItem In CBLMailSettings.Items
                If li.Selected = True Then
                    Sets += [Enum].Parse(GetType(TK.Domain.Enums.MailSettings), li.Value)
                End If
            Next

            Return Sets
        End Get
        Set(value As TK.Domain.Enums.MailSettings)

            If (Me.CBLMailSettings.Items.Count() = 0) Then
                InitCBL()
            End If


            If (value = lm.Comol.Core.BaseModules.Tickets.Domain.Enums.MailSettings.Default) Then
                Me.CBXdefault.Checked = True
                For Each item As ListItem In Me.CBLMailSettings.Items
                    item.Enabled = False
                Next
            Else
                Me.CBXdefault.Checked = False
                For Each item As ListItem In Me.CBLMailSettings.Items
                    item.Selected = value And [Enum].Parse(GetType(TK.Domain.Enums.MailSettings), item.Value)
                    item.Enabled = True
                Next
            End If


        End Set
    End Property

#End Region

#Region "Inherits"

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property


#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    ''' <summary>
    ''' Entry point per l'inizializzazione del controllo.
    ''' </summary>
    ''' <param name="UserId"></param>
    ''' <param name="CommunityId"></param>
    ''' <param name="TicketId"></param>
    ''' <param name="BindData">Se a TRUE, verranno caricati automaticamente i dati dai parametri indicati, altrimenti potrà essere utilizzato come semplice controllo grafico.</param>
    ''' <remarks></remarks>
    Public Sub InitializeControl( _
                                ByVal UserId As Int64, _
                                ByVal BindData As Boolean, _
                                Optional ByVal CommunityId As Int32 = -1, _
                                Optional ByVal TicketId As Int64 = -1)


        Me.UserId = UserId
        Me.CommunityId = CommunityId
        Me.TicketId = TicketId

        Me.IsInitialized = True

        If (BindData) Then
            Me.CurrentPresenter.InitializeView(UserId, CommunityId, TicketId)
        End If

    End Sub

    Public Sub InitializeControlForPerson( _
                                         ByVal PersonId As Integer, _
                                         ByVal BindData As Boolean, _
                                         Optional ByVal CommunityId As Int32 = -1, _
                                         Optional ByVal TicketId As Int64 = -1)

        Dim UserId As Int64 = Me.CurrentPresenter.GetUserIdFromPerson(PersonId)

        If (UserId > 0) Then
            Me.InitializeControl(UserId, BindData, CommunityId, TicketId)
        Else
            'Error!
        End If

    End Sub
    Private Sub InitCBL()

        Me.CBLMailSettings.Items.Clear()
        Dim MailSetsVals As Array = [Enum].GetValues(GetType(TK.Domain.Enums.MailSettings))
        Dim MailSetsNames As Array = [Enum].GetNames(GetType(TK.Domain.Enums.MailSettings))

        For i As Integer = 0 To MailSetsVals.Length - 1
            If Not (MailSetsVals(i) = TK.Domain.Enums.MailSettings.Default) Then
                Dim Li As New ListItem()
                'Li.Text = Resource.Getvalue("CBLMailSettings." & MailSetsNames(i).ToString())
                Li.Text = Resource.getValue("MailSettings." & MailSetsNames(i).ToString())
                Li.Value = MailSetsVals(i)
                Li.Attributes.Add("class", "item")
                Me.CBLMailSettings.Items.Add(Li)
            End If
        Next

    End Sub

    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UserSettings", "Modules", "Ticket")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()

    End Sub

    Public Sub DisplaySessionTimeout(CommunityId As Integer) Implements TK.Presentation.View.iViewBase.DisplaySessionTimeout
        Me.CBLMailSettings.Enabled = False
    End Sub

    Public Sub SendUserActions(ModuleId As Integer, Action As TK.ModuleTicket.ActionType, idCommunity As Integer, Type As TK.ModuleTicket.InteractionType, Optional Objects As System.Collections.Generic.IList(Of System.Collections.Generic.KeyValuePair(Of Integer, String)) = Nothing) Implements TK.Presentation.View.iViewBase.SendUserActions

    End Sub
    Public Function SaveSettings() As Boolean

        If Me.IsInitialized Then
            Me.CurrentPresenter.UpdateSettings(Me.UserId, Me.CommunityId, Me.TicketId)
        Else

        End If

    End Function

    ''' <summary>
    ''' Imposta i parametri per TUTTI i ticket.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 
    '''        NOTA!!!!
    ''' 
    ''' Al momento non è implementata.
    ''' 
    ''' EVENTUALMENTE è da pensare bene come va ad intervenire,
    ''' probabilmente a scelta dell'utente:
    ''' - I MIEI ticket (che ho aperto io)
    ''' - Quelli a cui potrei accedere (vedi recupero di tutti i relativi TicketID)
    ''' - Quelli a cui ho fatto accesso
    ''' - Solo quelli di una data Categoria (e/o le categorie di una comunità)
    ''' - Etc...
    ''' 
    ''' Per questo la farei EVENTUALMENTE come Features futura,
    ''' che andrà a seguire tutto l'iter opportuno, dall'analisi requisiti in avanti.
    ''' 
    ''' </remarks>
    Public Function SetALL() As Boolean

        'Dim total As Integer = -1
        'If Me.IsInitialized Then
        '    total = Me.CurrentPresenter.UpdateSettingsALL(Me.UserId, Me.CommunityId, Me.TicketId)
        'Else

        'End If

        'If (total > 0) Then
        '    Return True
        'End If

        'Return False
    End Function

    Public Sub ShowNoAccess() Implements lm.Comol.Core.BaseModules.Tickets.Presentation.View.iViewBase.ShowNoAccess

    End Sub

    Private Sub CBXdefault_CheckedChanged(sender As Object, e As System.EventArgs) Handles CBXdefault.CheckedChanged

        For Each item As ListItem In Me.CBLMailSettings.Items
            item.Enabled = Not Me.CBXdefault.Checked
        Next

    End Sub

    Public Property ShowDefault As Boolean
        Get
            Return Me.CBXdefault.Visible
        End Get
        Set(value As Boolean)
            Me.CBXdefault.Visible = value

            If value = False Then
                Me.CBXdefault.Checked = False

                For Each item As ListItem In Me.CBLMailSettings.Items
                    item.Enabled = True
                Next

            End If

        End Set
    End Property

    Public Property CssMain As String
        Get
            Return Me.LTmainCss.Text
        End Get
        Set(value As String)
            Me.LTmainCss.Text = value
        End Set
    End Property

    Public Property CssItem As String
        Get
            Return Me.LTitemCss.Text
        End Get
        Set(value As String)
            Me.LTitemCss.Text = value
        End Set
    End Property

    Private Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Me.CBLMailSettings.CssClass = CssMain
        For Each item As ListItem In Me.CBLMailSettings.Items
            item.Attributes.Remove("class")
            item.Attributes.Add("class", CssItem)
        Next

    End Sub

    Public Property CssDefault As String
        Get
            Return Me.CBXdefault.CssClass
        End Get
        Set(value As String)
            Me.CBXdefault.CssClass = value
        End Set
    End Property

    Public Sub SendNotification(Action As lm.Comol.Core.Notification.Domain.NotificationAction, CurrentPersonId As Integer) Implements lm.Comol.Core.BaseModules.Tickets.Presentation.View.iViewBase.SendNotification

        Dim service As SrvNotifications.iNotificationsManagerService = Nothing

        Try
            service = New SrvNotifications.iNotificationsManagerServiceClient()


            service.NotifyActionToModule(SystemSettings.NotificationErrorService.ComolUniqueID, Action, CurrentPersonId, PageUtility.CurrentContext.UserContext.IpAddress, PageUtility.CurrentContext.UserContext.ProxyIpAddress)


        Catch ex As Exception
            If Not IsNothing(service) Then
                Dim iService As SrvNotifications.iNotificationsManagerServiceClient = DirectCast(service, SrvNotifications.iNotificationsManagerServiceClient)
                iService.Abort()
                iService = Nothing
            End If
        Finally
            If Not IsNothing(service) Then
                Dim iService As SrvNotifications.iNotificationsManagerServiceClient = DirectCast(service, SrvNotifications.iNotificationsManagerServiceClient)
                iService.Close()
                iService = Nothing
            End If
        End Try

    End Sub

    'Public Sub SendNotifications(Actions As IList(Of lm.Comol.Core.Notification.Domain.NotificationAction), CurrentPersonId As Integer) Implements lm.Comol.Core.BaseModules.Tickets.Presentation.View.iViewBase.SendNotifications

    'End Sub
End Class