Imports System.Web
Imports System.Web.SessionState
Imports COL_BusinessLogic_v2.FileLayer
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comol.Entities
Imports COL_BusinessLogic_v2.Comol.Manager
Imports COL_BusinessLogic_v2.Comol.Manager.Esse3

Imports System.Reflection
Imports System.Diagnostics

Public Class [Global]
    Inherits System.Web.HttpApplication

    Private _PageUtility As PresentationLayer.OLDpageUtility
#Region "Property"
    Private ReadOnly Property PageUtility(Optional ByVal oContext As HttpContext = Nothing) As PresentationLayer.OLDpageUtility
        Get
            If IsNothing(_PageUtility) OrElse Not IsNothing(oContext) Then
                _PageUtility = New OLDpageUtility(HttpContext.Current)
            End If
            Return _PageUtility
        End Get
    End Property
    Protected Property Lingua() As Lingua
        Get
            Dim oLingua As Lingua
            Try
                oLingua = DirectCast(Session("Lingua"), Lingua)
            Catch ex As Exception
                oLingua = GetSystemSettings.DefaultLanguage
            End Try
            Return oLingua
        End Get
        Set(ByVal value As Lingua)
            Session("Lingua") = value
        End Set
    End Property
    Protected Property LinguaCode() As String
        Get
            'Ricontrollare, va'...
            Dim CodeLingua As String = ""
            Try
                If Session("LinguaCode") = "" Then
                    'Try
                    '	CodeLingua = Request.UserLanguages(0)
                    'Catch ex As Exception

                    'End Try
                    If Request.Browser.Cookies = True Then
                        Try
                            CodeLingua = Request.Cookies("LinguaCode").Value
                        Catch ex As Exception
                        End Try
                    End If
                Else
                    Try
                        CodeLingua = Session("LinguaCode")
                    Catch ex As Exception
                    End Try
                End If
            Catch ex As Exception
                CodeLingua = ""
            End Try
            If CodeLingua = "" Then
                CodeLingua = GetSystemSettings.DefaultLanguage.Codice
            End If
            Return CodeLingua
        End Get
        Set(ByVal value As String)
            Session("LinguaCode") = value
        End Set
    End Property
    Protected Property LinguaID() As Integer
        Get
            Try
                If IsNumeric(Session("LinguaID")) Then
                    LinguaID = CInt(Session("LinguaID"))
                Else
                    LinguaID = 1
                End If
            Catch ex As Exception
                LinguaID = 1
            End Try
            Return LinguaID
        End Get
        Set(ByVal value As Integer)
            Session("LinguaID") = value
        End Set
    End Property
    Protected Property ComunitaID() As Integer
        Get
            Try
                If IsNumeric(Session("idComunita")) Then
                    ComunitaID = CInt(Session("idComunita"))
                Else
                    ComunitaID = 0
                End If
            Catch ex As Exception
                ComunitaID = 0
            End Try
            Return ComunitaID
        End Get
        Set(ByVal value As Integer)
            Session("idComunita") = value 'ERROR: LinguaID??? oppure idComunita?
        End Set
    End Property
    Protected ReadOnly Property UtenteCorrenteID() As Integer
        Get
            Try
                UtenteCorrenteID = Session("objPersona").id
            Catch ex As Exception
                UtenteCorrenteID = 0
            End Try
        End Get
    End Property
    Protected ReadOnly Property CurrentModule() As PlainService
        Get
            If Not TypeOf Me.Session("CurrentService") Is PlainService Then
                Me.Session("CurrentService") = PlainService.Create(-1, "")
            End If
            CurrentModule = Me.Session("CurrentService")
        End Get
    End Property
    'Protected Property Tutor() As COL_BusinessLogic_v2.Comol.Entities.KnowledgeTutor
    '    Get
    '        Dim oTutor As COL_BusinessLogic_v2.Comol.Entities.KnowledgeTutor
    '        Me.Application.Lock()

    '        Try
    '            If IsNothing(Application("KnowledgeTutor")) Then
    '                oTutor = Nothing
    '            ElseIf GetType(COL_BusinessLogic_v2.Comol.Entities.KnowledgeTutor) Is Application("KnowledgeTutor").GetType Then
    '                oTutor = Application("KnowledgeTutor")
    '            Else
    '                oTutor = Nothing
    '            End If
    '        Catch ex As Exception
    '            oTutor = Nothing
    '        End Try
    '        Me.Application.UnLock()
    '        Return oTutor
    '    End Get
    '    Set(ByVal value As COL_BusinessLogic_v2.Comol.Entities.KnowledgeTutor)
    '        Me.Application.Lock()
    '        Application("KnowledgeTutor") = value
    '        Me.Application.UnLock()
    '    End Set
    'End Property

    Protected Property UtentiConnessi() As Integer
        Get
            Dim NumeroUtenti As Integer
            Try
                If IsNumeric(Me.Application.Item("utentiConnessi")) Then
                    NumeroUtenti = CInt(Me.Application.Item("utentiConnessi"))
                Else
                    NumeroUtenti = 0
                    Me.Application.Lock()
                    Me.Application.Item("utentiConnessi") = 0
                    Me.Application.UnLock()
                End If
            Catch ex As Exception
                NumeroUtenti = 0
                Me.Application.Lock()
                Me.Application.Item("utentiConnessi") = 0
                Me.Application.UnLock()
            End Try
            Return NumeroUtenti
        End Get
        Set(ByVal value As Integer)
            Me.Application.Lock()
            Me.Application.Item("utentiConnessi") = value
            Me.Application.UnLock()
        End Set
    End Property

    Private ReadOnly Property BaseUrl() As String
        Get
            Try
                If IsNothing(Me.Request) Then
                    Return "~/"
                Else
                    Dim url As String = Me.Request.ApplicationPath
                    If url.EndsWith("/") Then
                        Return url
                    Else
                        Return url + "/"
                    End If
                End If
            Catch ex As Exception
                Return "~/"
            End Try
        End Get
    End Property

    Private Sub RedirectToUrl(ByVal Url As String, Optional ByVal sessionEnabled As Boolean = False)
        Dim Redirect As String = "http"
        Dim oSettings As ComolSettings
        If Not IsNothing(Me.Context) Then
            oSettings = ManagerConfiguration.GetInstance   ' (System.Configuration.ConfigurationManager.AppSettings("defaultCulture"), Me.Server.MapPath(BaseUrl), BaseUrl)
            If oSettings.Login.isSSLrequired Then
                Redirect &= "s://" & Me.Request.Url.Host & Me.BaseUrl & Url
            Else
                Redirect &= "://" & Me.Request.Url.Host & Me.BaseUrl & Url
            End If
        Else
            Redirect = "~/"
        End If
        Response.Redirect(Redirect, True)
    End Sub
    Public Property AccessoSistema() As Boolean
        Get
            Try
                AccessoSistema = Me.Application("SystemAcess")
            Catch ex As Exception
                AccessoSistema = True
            End Try
        End Get
        Set(ByVal value As Boolean)
            Application.Lock()
            Me.Application.Add("SystemAcess", value)
            Application.UnLock()
        End Set
    End Property

    Public ReadOnly Property LocalizedMail() As MailLocalized
        Get
            Try
                Return ManagerConfiguration.GetMailLocalized(Me.Lingua)
            Catch ex As Exception

            End Try
            Return Nothing
        End Get
    End Property

    Private Function GetSystemSettings() As ComolSettings
        Dim oSettings As ComolSettings
        oSettings = ManagerConfiguration.GetInstance
        Return oSettings
    End Function
#End Region

    Protected Property UniqueGuidSession() As Guid
        Get
            Dim iUniqueGuidSession As Guid
            If TypeOf Session("UniqueGuidSession") Is System.Guid Then
                iUniqueGuidSession = Session("UniqueGuidSession")
                If iUniqueGuidSession = Guid.Empty Then
                    iUniqueGuidSession = Guid.NewGuid
                    Session("UniqueGuidSession") = iUniqueGuidSession
                End If
            Else
                iUniqueGuidSession = Guid.NewGuid
                Session("UniqueGuidSession") = iUniqueGuidSession
            End If
            Return iUniqueGuidSession
        End Get
        Set(ByVal value As Guid)
            Session("UniqueGuidSession") = value
        End Set
    End Property
#Region " Codice generato da Progettazione componenti "

    Public Sub New()
        MyBase.New()

        'Chiamata richiesta da Progettazione componenti.
        InitializeComponent()

        'Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent()

    End Sub

    'Richiesto da Progettazione componenti
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione componenti.
    'Può essere modificata in Progettazione componenti.  
    'Non modificarla nell'editor del codice.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        components = New System.ComponentModel.Container
    End Sub

#End Region

    ' Settaggi iniziali
    Sub InitSettings()

    End Sub
    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        Dim i, totale As Integer
        Dim oOrganizzazione As New COL_Organizzazione
        Dim oIstituzione As New COL_Istituzione

        ' Generato all'avvio dell'applicazione

        Me.UtentiConnessi = 0
        OLDpageUtility.ApplicationWorkingId = System.Guid.NewGuid

        Me.AccessoSistema = True
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        Me.PageUtility.GenerateNewSession()
        'Session("ISTT_ID") = 1
        'Session("LogonAs") = False
        'Session("CMNT_path_forAdmin") = ""
        'Session("CMNT_path_forNews") = ""
        'Session("OrgnIDtoSubscribe") = ""
        'Session("TipoPersonaIDtoSubscribe") = ""
        'Session("IstituzioneIDtoSubscribe") = ""
        'Session("oImpostazioni") = Nothing
        'Session("NewLanguageId") = 0

        'Dim oLingua As New Lingua
        'oLingua = ManagerLingua.GetByCodeOrDefault(Me.LinguaCode)
        'If Not IsNothing(oLingua) Then
        '	Me.LinguaCode = oLingua.Codice
        '	Me.LinguaID = oLingua.ID
        'Else
        '	Me.LinguaCode = "it-IT"
        '	Me.LinguaID = 1
        'End If
        'Me.Lingua = oLingua
        'Me.PageUtility(Me.Context).UniqueGuidSession = Guid.NewGuid
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)

    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Generato durante il tentativo di autenticazione dell'utente
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        Dim oSettings As ComolSettings = Nothing
        Try
            oSettings = GetSystemSettings()
        Catch ex As Exception
            oSettings = New ComolSettings()
            oSettings.NotificationErrorService.Enabled = True
            oSettings.NotificationErrorService.ComolUniqueID = "DEFAULT"
            oSettings.NotificationErrorService.ErrorsType.Add(New dtoErrorType() With {.Enabled = True, .SendTo = Comol.Entity.ErrorsNotificationService.PersistTo.Mail, .Type = Comol.Entity.ErrorsNotificationService.ErrorType.CommunityModuleError})
            oSettings.NotificationErrorService.ErrorsType.Add(New dtoErrorType() With {.Enabled = True, .SendTo = Comol.Entity.ErrorsNotificationService.PersistTo.Mail, .Type = Comol.Entity.ErrorsNotificationService.ErrorType.DBerror})
            oSettings.NotificationErrorService.ErrorsType.Add(New dtoErrorType() With {.Enabled = True, .SendTo = Comol.Entity.ErrorsNotificationService.PersistTo.Mail, .Type = Comol.Entity.ErrorsNotificationService.ErrorType.FileError})
            oSettings.NotificationErrorService.ErrorsType.Add(New dtoErrorType() With {.Enabled = True, .SendTo = Comol.Entity.ErrorsNotificationService.PersistTo.Mail, .Type = Comol.Entity.ErrorsNotificationService.ErrorType.GenericError})
            oSettings.NotificationErrorService.ErrorsType.Add(New dtoErrorType() With {.Enabled = True, .SendTo = Comol.Entity.ErrorsNotificationService.PersistTo.Mail, .Type = Comol.Entity.ErrorsNotificationService.ErrorType.GenericModuleError})
            oSettings.NotificationErrorService.ErrorsType.Add(New dtoErrorType() With {.Enabled = True, .SendTo = Comol.Entity.ErrorsNotificationService.PersistTo.Mail, .Type = Comol.Entity.ErrorsNotificationService.ErrorType.GenericWebError})
        End Try
        Dim current As HttpRequest = HttpContext.Current.Request
        Dim oLastError As Exception = Server.GetLastError()
        Dim httpException As HttpException = Nothing
        Dim httpCode As Integer = 500

        Try

            If Response.IsClientConnected AndAlso oSettings.NotificationErrorService.isSendingEnabled(ErrorsNotificationService.ErrorType.GenericWebError) AndAlso Not IsNothing(Server.GetLastError) Then
                NotifyError(oSettings, Server.GetLastError())
            End If
        Catch ex As Exception

        End Try
    End Sub

    

         
        

    Private Sub NotifyError(ByVal settings As ComolSettings, ByVal exception As Exception)

        Dim notificationService As ErrorsNotificationService.iErrorsNotificationService = Nothing
        Try
            notificationService = New ErrorsNotificationService.iErrorsNotificationServiceClient()
            Dim oError As ErrorsNotificationService.GenericWebError = New ErrorsNotificationService.GenericWebError()
            With oError
                'CONTEXT VARIABLES
                .ServerName = Server.MachineName
                .SentDate = Now
                .Day = .SentDate.Date
                .ComolUniqueID = settings.NotificationErrorService.ComolUniqueID

                Try
                    .CommunityID = ComunitaID
                    .UserID = UtenteCorrenteID
                    .ModuleID = CurrentModule.ID
                    .ModuleCode = CurrentModule.Code
                Catch ex As Exception

                End Try
                If .ModuleCode Is Nothing Then
                    .ModuleCode = ""
                End If
                .QueryString = Request.QueryString.ToString
                .UniqueID = System.Guid.NewGuid()
                .Url = Request.Url.AbsolutePath
                .Persist = settings.NotificationErrorService.FindPersistTo(ErrorsNotificationService.ErrorType.GenericWebError)
                .Type = ErrorsNotificationService.ErrorType.GenericWebError
                Dim oLastError As Exception = Server.GetLastError()
                Dim oException As Exception = oLastError.GetBaseException()

                .Message = oLastError.Message.ToString
                If Not IsNothing(Server.GetLastError.InnerException) Then
                    .InnerExceptionMessage = Server.GetLastError.InnerException.ToString
                End If
                .BaseExceptionStackTrace = oException.StackTrace
                .ExceptionSource = oLastError.Source.ToString

            End With
            notificationService.sendGenericWebError(oError)
            CloseNotificationService(notificationService)
        Catch ex As Exception
            CloseNotificationService(notificationService)
        End Try
    End Sub

    Private Sub CloseNotificationService(ByVal notificationService As ErrorsNotificationService.iErrorsNotificationService)
        If Not IsNothing(notificationService) Then
            Dim service As System.ServiceModel.ClientBase(Of ErrorsNotificationService.iErrorsNotificationService) = DirectCast(notificationService, System.ServiceModel.ClientBase(Of ErrorsNotificationService.iErrorsNotificationService))
            service.Abort()
            service = Nothing
        End If
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Generato alla fine della sessione
        'forse sarebbe meglio mettere un avviso sulla sessione scaduta

        Try
            Dim oPersona As New CL_persona.COL_Persona
            Dim TotaleConnessioni As Integer
            oPersona = Session("objPersona")

            TotaleConnessioni = oPersona.CancellaConnessione(oPersona.ID, Session.SessionID)
            Me.PageUtility.LogoutAction()
            If oPersona.ID > 0 Then
                oPersona.Logout()
                If TotaleConnessioni > 0 Then
                    Me.UtentiConnessi = TotaleConnessioni
                Else
                    Me.UtentiConnessi = 0
                End If
                'Me.RedirectToUrl("Index.aspx")
            End If
        Catch ex As Exception

        End Try

    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        Dim runtime As HttpRuntime = CType(GetType(System.Web.HttpRuntime).InvokeMember("_theRuntime", (BindingFlags.NonPublic _
          Or (BindingFlags.Static Or BindingFlags.GetField)), Nothing, Nothing, Nothing), HttpRuntime)
        If (runtime Is Nothing) Then
            Return
        End If
        Dim shutDownMessage As String = CType(runtime.GetType.InvokeMember("_shutDownMessage", (BindingFlags.NonPublic _
         Or (BindingFlags.Instance Or BindingFlags.GetField)), Nothing, runtime, Nothing), String)
        Dim shutDownStack As String = CType(runtime.GetType.InvokeMember("_shutDownStack", (BindingFlags.NonPublic _
         Or (BindingFlags.Instance Or BindingFlags.GetField)), Nothing, runtime, Nothing), String)
        If Not EventLog.SourceExists(".NET Runtime") Then
            EventLog.CreateEventSource(".NET Runtime", "Application")
        End If
        Dim log As EventLog = New EventLog
        log.Source = ".NET Runtime"
        log.WriteEntry(String.Format("" & vbCrLf & vbCrLf & "_shutDownMessage={0}" & vbCrLf & vbCrLf & "_shutDownStack={1}", shutDownMessage, shutDownStack), EventLogEntryType.Error)
    End Sub

End Class