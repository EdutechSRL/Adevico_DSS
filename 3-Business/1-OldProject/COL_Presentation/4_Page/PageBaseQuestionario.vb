Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.FileLayer
Imports COL_Questionario
Imports System.Collections.Generic
Imports COL_BusinessLogic_v2.Comol.Manager


Public MustInherit Class PageBaseQuestionario
    Inherits Base
    Implements IviewQuestionario


    Private _Servizio As Services_Questionario
    Private _EPservice As lm.Comol.Modules.EduPath.BusinessLogic.Service
    Protected ReadOnly Property EPservice() As lm.Comol.Modules.EduPath.BusinessLogic.Service
        Get
            If IsNothing(_EPservice) Then
                _EPservice = New lm.Comol.Modules.EduPath.BusinessLogic.Service(PageUtility.CurrentContext)
            End If
            Return _EPservice
        End Get
    End Property
    Private _serviceEduPath As lm.Comol.Modules.EduPath.BusinessLogic.Service
    Protected ReadOnly Property ServiceEduPath As lm.Comol.Modules.EduPath.BusinessLogic.Service
        Get
            If IsNothing(_serviceEduPath) Then
                _serviceEduPath = New lm.Comol.Modules.EduPath.BusinessLogic.Service(PageUtility.CurrentContext)
            End If
            Return _serviceEduPath
        End Get
    End Property
    Private _PageUtility As PresentationLayer.OLDpageUtility


    Protected Sub SetServiceTitle(ByRef Master As IviewMaster)
        Dim tipoQuest As Questionario.TipoQuestionario
        If Not Request.QueryString("type") Is Nothing Then
            tipoQuest = Request.QueryString("type")
        ElseIf Not Session("QuestionarioCorrente") Is Nothing Then
            tipoQuest = DirectCast(Session("QuestionarioCorrente"), Questionario).tipo
        Else
            tipoQuest = Questionario.TipoQuestionario.Questionario
        End If
        With Me.Resource
            Select Case tipoQuest
                Case Questionario.TipoQuestionario.LibreriaDiDomande
                    Master.ServiceTitle = .getValue("LibreriaDiDomande")
                Case Questionario.TipoQuestionario.Modello
                    Master.ServiceTitle = .getValue("Modello")
                Case Questionario.TipoQuestionario.Autovalutazione
                    Master.ServiceTitle = .getValue("Autovalutazione")
                Case Questionario.TipoQuestionario.Sondaggio
                    Master.ServiceTitle = .getValue("Sondaggio")
                Case Questionario.TipoQuestionario.Meeting
                    Master.ServiceTitle = .getValue("Meeting")
                Case Else
                    Master.ServiceTitle = .getValue("Questionari")
            End Select
        End With
    End Sub

    Protected Function GetServiceTitle() As String
        Dim tipoQuest As Questionario.TipoQuestionario
        If Not Request.QueryString("type") Is Nothing Then
            tipoQuest = Request.QueryString("type")
        ElseIf Not Session("QuestionarioCorrente") Is Nothing Then
            tipoQuest = DirectCast(Session("QuestionarioCorrente"), Questionario).tipo
        Else
            tipoQuest = Questionario.TipoQuestionario.Questionario
        End If
        With Me.Resource
            Select Case tipoQuest
                Case Questionario.TipoQuestionario.LibreriaDiDomande
                    Return .getValue("LibreriaDiDomande")
                Case Questionario.TipoQuestionario.Modello
                    Return .getValue("Modello")
                Case Questionario.TipoQuestionario.Autovalutazione
                    Return .getValue("Autovalutazione")
                Case Questionario.TipoQuestionario.Sondaggio
                    Return .getValue("Sondaggio")
                Case Questionario.TipoQuestionario.Meeting
                    Return .getValue("Meeting")
                Case Else
                    Return .getValue("Questionari")
            End Select
        End With
    End Function

    Sub New()
        MyBase.New()
    End Sub

#Region "gestione history view"


    'Private _viewAttuale As VIWattiva '= Session("_viewAttuale")
    'Public _viewPrecedente As VIWattiva ' = Session("_viewPrecedente")



    Public Function viewPrecedente_isVisible() As Boolean
        If Session("_viewPrecedente") Is Nothing Or Session("_viewPrecedente") = Session("_viewAttuale") Then
            Return False
        Else
            Return True
            'Return VIWattiva.VIWerrore.Equals(Session("_viewPrecedente"))
            ' Return Not (Session("_viewPrecedente") = VIWattiva.VIWerrore)
        End If
    End Function

    Public Function viewPrecedente_readOnly() As Int16
        Return Session("_viewAttuale")
    End Function

    Public Property viewPrecedente() As Int16

        Get
            Session("_viewAttuale") = Session("_viewPrecedente")
            Session.Remove("_viewPrecedente")
            Return Session("_viewAttuale")
        End Get

        Set(ByVal value As Int16)
            Session("_viewPrecedente") = Session("_viewAttuale")
            Session("_viewAttuale") = value
        End Set
    End Property

#End Region
    Public Property isAnonymousCompiler() As Boolean Implements IviewQuestionario.isAnonymousCompiler
        Get
            Return Session("isAnonymousCompiler")
        End Get
        Set(ByVal value As Boolean)
            Session("isAnonymousCompiler") = value
        End Set
    End Property
    Public Property LinguaQuestionario() As Integer Implements IviewQuestionario.LinguaQuestionario
        Get
            Try
                If IsNumeric(Session("LinguaQuestionario")) Then
                    LinguaQuestionario = CInt(Session("LinguaQuestionario"))
                Else
                    LinguaQuestionario = 1
                End If
            Catch ex As Exception
                LinguaQuestionario = 1
            End Try
        End Get
        Set(ByVal value As Integer)
            Session("LinguaQuestionario") = value
        End Set
    End Property

    Public Property LinguaDefaultQuestionario() As Integer Implements IviewQuestionario.LinguaDefaultQuestionario
        Get
            Try
                If IsNumeric(Session("LinguaDefaultQuestionario")) Then
                    LinguaDefaultQuestionario = CInt(Session("LinguaDefaultQuestionario"))
                Else
                    LinguaDefaultQuestionario = 1
                End If
            Catch ex As Exception
                LinguaDefaultQuestionario = 1
            End Try
        End Get
        Set(ByVal value As Integer)
            Session("LinguaDefaultQuestionario") = value
        End Set
    End Property

    Public MustOverride ReadOnly Property isCompileForm() As Boolean


    Public Property GruppoQuestionariID() As Integer Implements IviewQuestionario.GruppoQuestionariID
        Get
            Try
                GruppoQuestionariID = DirectCast(Session("GruppoQuestionariID"), Integer)
            Catch ex As Exception
                Session("GruppoQuestionariID") = 0
                GruppoQuestionariID = 0
            End Try
        End Get
        Set(ByVal value As Integer)
            Session("GruppoQuestionariID") = value
        End Set
    End Property
    Public Property GruppoDefaultID() As Integer Implements IviewQuestionario.GruppoDefaultID
        Get
            Try
                GruppoDefaultID = DirectCast(Session("GruppoDefaultID"), Integer)
            Catch ex As Exception

            End Try
        End Get
        Set(ByVal value As Integer)
            Session("GruppoDefaultID") = value
        End Set
    End Property

    Public ReadOnly Property Invito() As COL_Questionario.UtenteInvitato Implements IviewQuestionario.Invito
        Get
            Try
                Invito = DirectCast(Session("Invito"), COL_Questionario.UtenteInvitato)
            Catch ex As Exception
                Invito = New COL_Questionario.UtenteInvitato(0)
            End Try
        End Get
    End Property

    Public Property QuestionarioCorrente() As COL_Questionario.Questionario Implements IviewQuestionario.QuestionarioCorrente
        Get
            'Try
            Dim oQuest As Questionario
            oQuest = DirectCast(Session("QuestionarioCorrente"), COL_Questionario.Questionario)
            If oQuest Is Nothing Then
                oQuest = New Questionario
                Session("QuestionarioCorrente") = oQuest
            End If
            Return oQuest
        End Get
        Set(ByVal value As COL_Questionario.Questionario)
            If Not Page.IsPostBack Then
                'SOLO al primo caricamento della pagina
                If QuestionarioCorrente.idDestinatario_Persona > 0 Then
                    Me.QsDestUserId = QuestionarioCorrente.idDestinatario_Persona
                Else
                    Me.QsDestUserId = UtenteCorrente.ID
                End If

            End If

            Session("QuestionarioCorrente") = value
        End Set
    End Property

    ''' <summary>
    ''' Per verifica che la compilazione in SESSIONE corrisponda a quella visualizzata sulla pagina,
    ''' nel caso in cui la sessione venga sovrascritta con altri questionari.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' NON sono ancora chiare le cause!!!
    ''' </remarks>
    Public Property QsDestUserId As Integer
        Get
            Return ViewStateOrDefault("QsCompileId", UtenteCorrente.ID)
        End Get
        Set(value As Integer)
            ViewState("QsCompileId") = value
        End Set
    End Property

    Public Property LoadedLibraries() As List(Of dtoWorkingLibrary)
        Get
            'Try
            Dim items As List(Of dtoWorkingLibrary)
            items = DirectCast(Session("LoadedLibraries"), List(Of dtoWorkingLibrary))
            If items Is Nothing Then
                items = New List(Of dtoWorkingLibrary)
                Session("LoadedLibraries") = items
            End If
            Return items
        End Get
        Set(ByVal value As List(Of dtoWorkingLibrary))
            Session("LoadedLibraries") = value
        End Set
    End Property

    Public Property DomandaCorrente() As COL_Questionario.Domanda Implements IviewQuestionario.DomandaCorrente
        Get
            Try
                DomandaCorrente = DirectCast(Session("DomandaCorrente"), COL_Questionario.Domanda)
            Catch ex As Exception
                DomandaCorrente = New Domanda
            End Try
        End Get
        Set(ByVal value As COL_Questionario.Domanda)
            Session("DomandaCorrente") = value
        End Set
    End Property

    Public Property PaginaCorrenteID() As Integer Implements IviewQuestionario.PaginaCorrenteID
        Get
            Try
                PaginaCorrenteID = Session("idPagina")
            Catch ex As Exception
                PaginaCorrenteID = 0
            End Try
        End Get
        Set(ByVal value As Integer)
            Session("idPagina") = value
        End Set
    End Property

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsSessioneScaduta(True) Then
            If Not Page.IsPostBack Then
                If Me.isCompileForm Then
                    Me.ImpostazioneDatiComunitaByQuestionario()
                Else
                    If LoadDataByUrl Then
                        Me.LoadQuestionnaireByUrl()
                    End If
                    Me.ImpostazioneDatiBase()
                End If

                Me.SetInternazionalizzazione()
                If HasPermessi() Then
                    Me.BindDati()
                Else
                    Me.BindNoPermessi()
                End If
                Me.SetControlliByPermessi()
                Me.RegistraAccessoPagina()
            End If
        ElseIf Not Page.IsPostBack Then
            Me.SetInternazionalizzazione()
        End If
    End Sub

    Public Overrides Function IsSessioneScaduta(ByVal RedirectToLogin As Boolean) As Boolean
        Dim isScaduta As Boolean = True
        Dim RedirectToLoginPage As Boolean = RedirectToLogin
        If Request.RawUrl.Contains("esame.aspx") Then
            isScaduta = False
        Else

            If Me.EncryptedQueryString("idu", SecretKeyUtil.EncType.Questionario) <> "" Then
                RedirectToLoginPage = False
                Try
                    If CInt(Me.EncryptedQueryString("idu", SecretKeyUtil.EncType.Questionario)) > 0 Then
                        isScaduta = False
                    End If
                Catch ex As Exception

                End Try
            ElseIf Not IsNothing(Me.UtenteCorrente) Then
                If Me.UtenteCorrente.ID > 0 Then
                    isScaduta = False
                End If
            ElseIf Me.EncryptedQueryString("ida", SecretKeyUtil.EncType.Questionario) = "1" Then
                isAnonymousCompiler = True
                RedirectToLoginPage = False
                If Not Page.IsPostBack Then
                    isScaduta = False
                End If
            Else
                isAnonymousCompiler = False
            End If
            If isScaduta Then
                If RedirectToLoginPage Then
                    Dim alertMSG As String
                    alertMSG = Me.Resource.getValue("LogoutMessage")
                    If alertMSG <> "" Then
                        alertMSG = alertMSG.Replace("'", "\'")
                    Else
                        alertMSG = "Session timeout"
                    End If
                    'Dim UrlRedirect As String = MyBase.ApplicationUrlBase & Me.SystemSettings.Presenter.DefaultStartPage
                    Dim UrlRedirect As String = PageUtility.GetDefaultLogoutPage ' Me.DefaultUrl
                    Response.Write("<script language='javascript'>function AlertLogout(Messaggio,pagina){" & vbCrLf & "alert(Messaggio);" & vbCrLf & "document.location.replace(pagina);" & vbCrLf & "} " & vbCrLf & "AlertLogout('" & alertMSG & "','" & UrlRedirect & "');</script>")
                    Response.End()
                End If
                isScaduta = True

            ElseIf Me.isPortalCommunity And Me.ComunitaCorrenteID > 0 Then
                MyBase.ExitToLimbo()
                isScaduta = True
            End If

        End If
        Return isScaduta
    End Function


    Public MustOverride Function HasPermessi() As Boolean Implements IviewQuestionario.HasPermessi
    Public MustOverride Sub SetControlliByPermessi() Implements IviewQuestionario.SetControlliByPermessi
    Public MustOverride Sub RegistraAccessoPagina() Implements IviewQuestionario.RegistraAccessoPagina
    Public MustOverride Sub BindNoPermessi() Implements IviewQuestionario.BindNoPermessi

    'Public Sub CambiaImpostazioniLingua(ByVal LinguaID As Integer, ByVal LinguaCode As String) Implements IviewQuestionario.CambiaImpostazioniLingua
    '    Session("LinguaID") = LinguaID
    '    Session("LinguaCode") = LinguaCode
    '    Me.SetCookies(LinguaID, LinguaCode)
    'End Sub


    Public ReadOnly Property Servizio() As COL_BusinessLogic_v2.UCServices.Services_Questionario Implements IviewQuestionario.Servizio
        Get
            If IsNothing(_Servizio) Then
                If Me.ComunitaCorrenteID = 0 Then
                    _Servizio = New Services_Questionario()
                    With _Servizio
                        .Compila = (Me.UtenteCorrente.ID > 0)
                        .Admin = (Me.TipoPersonaID = Main.TipoPersonaStandard.AdminSecondario Or Me.TipoPersonaID = Main.TipoPersonaStandard.Amministrativo Or Me.TipoPersonaID = Main.TipoPersonaStandard.SysAdmin)

                    End With
                Else
                    _Servizio = Me.ElencoServizi.Find(Services_Questionario.Codex)
                    If IsNothing(_Servizio) Then
                        _Servizio = New Services_Questionario()
                    End If
                End If
            End If
            Servizio = _Servizio
        End Get
    End Property

    Public Property LibreriaCorrente() As COL_Questionario.Questionario Implements IviewQuestionario.LibreriaCorrente
        Get
            Try
                LibreriaCorrente = DirectCast(Session("LibreriaCorrente"), COL_Questionario.Questionario)
            Catch ex As Exception
                LibreriaCorrente = Nothing
            End Try
        End Get
        Set(ByVal value As COL_Questionario.Questionario)
            Session("LibreriaCorrente") = value
        End Set
    End Property

    Public Property GruppoCorrente() As COL_Questionario.QuestionarioGruppo Implements IviewQuestionario.GruppoCorrente
        Get
            Try
                GruppoCorrente = DirectCast(Session("GruppoCorrente"), COL_Questionario.QuestionarioGruppo)
            Catch ex As Exception
                GruppoCorrente = Nothing
            End Try

            If GruppoCorrente Is Nothing Then
                Dim oGruppo As New QuestionarioGruppo
                oGruppo.id = DALQuestionarioGruppo.GruppoPrincipaleByComunita_Id(Me.ComunitaCorrenteID)
                Session("GruppoCorrente") = oGruppo
            End If
        End Get
        Set(ByVal value As COL_Questionario.QuestionarioGruppo)
            Session("GruppoCorrente") = value
        End Set
    End Property

    Public Sub ImpostazioneDatiBase() Implements IviewQuestionario.ImpostazioneDatiBase
        If Me.isUtenteAnonimo And IsNothing(Me.UtenteCorrente) Then
            Me.CaricaAnonimo(0)
        End If

        If IsNothing(Session("Invito")) Then 'Or Session("Invito").GetType Is GetType(String) Then
            Session("Invito") = New COL_Questionario.UtenteInvitato(0)
            'CaricaInvito()
        End If

    End Sub

    Public Sub ImpostazioneDatiComunitaByQuestionario() Implements IviewQuestionario.ImpostazioneDatiComunitaByQuestionario
        Dim InvitoID, LinguaQuestID As Integer
        Dim QueryQuestionarioID As Integer = 0
        Dim ReloadQuestionario As Boolean = True

        Dim idQuestionario As String = Me.EncryptedQueryString("idq", SecretKeyUtil.EncType.Questionario)
        Dim idLingua As String = EncryptedQueryString("idl", SecretKeyUtil.EncType.Questionario)
        Dim idInvito As String = EncryptedQueryString("idu", SecretKeyUtil.EncType.Questionario)

        If Not String.IsNullOrEmpty(idQuestionario) Then
            Try
                Integer.TryParse(idQuestionario, QueryQuestionarioID)
            Catch ex As Exception
                QueryQuestionarioID = 0
            End Try
        End If

        Dim oQuestionario As New Questionario
        oQuestionario.id = QueryQuestionarioID

        If Me.isUtenteAnonimo And IsNothing(Me.UtenteCorrente) Then
            Me.CaricaAnonimo(0)
        End If
        If Not String.IsNullOrEmpty(idLingua) Then
            Try
                Integer.TryParse(idLingua, LinguaQuestID)
            Catch ex As Exception
                LinguaQuestID = 0
            End Try
        Else
            LinguaQuestID = Me.LinguaID
        End If
        Me.LinguaQuestionario = LinguaQuestID

        If idInvito <> "" Then
            Try
                If idInvito <> "" Then
                    InvitoID = DirectCast(CInt(idInvito), Integer)
                    If Not DALQuestionario.isInvitato(oQuestionario.id, InvitoID) Then
                        Me.Invito.ID = -1
                        Exit Sub
                    End If
                Else
                    InvitoID = 0
                End If
            Catch ex As Exception
                InvitoID = 0
            End Try
            Dim oUtenteInvitato As UtenteInvitato
            oUtenteInvitato = DALUtenteInvitato.readUtenteInvitatoByID(InvitoID)
            If oUtenteInvitato.PersonaID = 0 Then
                oUtenteInvitato.PersonaID = idUtenteAnonimo()

            ElseIf oUtenteInvitato.PersonaID <> idUtenteAnonimo Then
                Me.CaricaAnonimo(oUtenteInvitato.PersonaID)
            End If
            If oUtenteInvitato.ID = 0 Then
                InvitoID = 0
            End If
            Session("Invito") = oUtenteInvitato
        Else
            Session("Invito") = DALQuestionario.readInvitoByPersona(oQuestionario.id, Me.UtenteCorrente.ID)
            'Session("Invito") = New COL_Questionario.UtenteInvitato(0)
        End If
        Try
            'todo: [quest][notifiche] verificare che non abbia controindicazioni
            'senza l'"or" se si proviene da una notifica precedente al 15/12/2010 non viene caricata la risposta, viene quindi inserita una risposta doppia e si incasina tutto. Le notifiche son state cancellate.
            If Not Me.EncryptedQueryString("ida", SecretKeyUtil.EncType.Questionario) = "1" Then 'Or Request.Url.ToString.Contains(RootObject.compileUrl) Then
                oQuestionario = DALQuestionario.readQuestionarioByPersona(Me.PageUtility.CurrentContext, True, QueryQuestionarioID, LinguaQuestID, Me.UtenteCorrente.ID, Me.Invito.ID)
                isAnonymousCompiler = False
            Else
                'se il questionario e' stato aperto con un link anonimo non devono essere caricate risposte
                oQuestionario = DALQuestionario.readQuestionarioByPersona(Me.PageUtility.CurrentContext, True, QueryQuestionarioID, LinguaQuestID, -1, -1)
                isAnonymousCompiler = True
            End If
            If Me.EncryptedQueryString("pwd", SecretKeyUtil.EncType.Questionario) <> "" Then
                oQuestionario.isPassword = False
            End If
        Catch ex As Exception
            Dim errore As String
            errore = ex.Message
        End Try
        Me.QuestionarioCorrente = oQuestionario
    End Sub
    Public Function idUtenteAnonimo() As Integer
        Return MyBase.PageUtility.AnonymousCOL_Persona.ID
    End Function

    Public Sub CaricaAnonimo(ByVal PersonaID As Integer) Implements IviewQuestionario.CaricaAnonimo
        Dim oPersona As New COL_Persona
        Dim oLingua As New Lingua

        If Me.isUtenteAnonimo Then
            oLingua = ManagerLingua.GetByCodeOrDefault(LinguaCode)
            If Not IsNothing(oLingua) Then
                Session("LinguaCode") = oLingua.Codice
                Session("LinguaID") = oLingua.ID
            Else
                Session("LinguaCode") = "it-IT"
                Session("LinguaID") = 1
                oLingua = Lingua.CreateByCode(1, "it-IT")
            End If
        End If


        If PersonaID = 0 Then
            oPersona = MyBase.PageUtility.AnonymousCOL_Persona
            'Else
            '    oPersona.ID = PersonaID
            '    oPersona.Estrai(oLingua.ID)
            '    If oPersona.Errore <> Errori_Db.None Then
            '        oPersona = MyBase.PageUtility.AnonymousCOL_Persona
            '    End If
        End If
        Session("objPersona") = oPersona
        Session("objPersonaQuest") = oPersona
    End Sub

    Public Sub CaricaUtenteInvitatoEVerificato(ByVal PersonaID As Integer)
        Dim oPersona As New COL_Persona

        Dim oLingua As Lingua = ManagerLingua.GetByCodeOrDefault(LinguaCode)

        If IsNothing(oLingua) Then
            Session("LinguaCode") = "it-IT"
            Session("LinguaID") = 1
            oLingua = Lingua.CreateByCode(1, "it-IT")
        End If

        oPersona.ID = PersonaID
        oPersona.Estrai(oLingua.ID)
        If oPersona.Errore <> Errori_Db.None Then
            oPersona = MyBase.PageUtility.AnonymousCOL_Persona
        End If

        Session("objPersona") = oPersona

    End Sub
    '' ACTIONS 

    Public Function CreateObjectsList(ByVal oType As Services_Questionario.ObjectType, ByVal oValueID As String) As List(Of lm.ActionDataContract.ObjectAction)
        Dim oList As New List(Of lm.ActionDataContract.ObjectAction)
        oList.Add(New lm.ActionDataContract.ObjectAction With {.ObjectTypeId = oType, .ValueID = oValueID, .ModuleID = PageUtility.CurrentModule.ID})
        Return oList
    End Function

    Public Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_Questionario.Codex)
    End Sub


    ' - 1.000.000.000 PUNTI SE CANCELLI !!!!!
    '#######################################################

    Public Sub CaricaPersona(ByVal PersonaID As Integer, ByVal ComunitaID As Integer) Implements IviewQuestionario.CaricaPersona
        '	Dim oPersona As New COL_Persona
        '	Dim oLingua As New Lingua

        '	oLingua.EstraiByCode(LinguaCode, True)
        '	If oLingua.Errore = Errori_Db.None Then
        '		Session("LinguaCode") = oLingua.Id
        '		Session("LinguaID") = oLingua.Codice
        '	Else
        '		Session("LinguaCode") = "it-IT"
        '		Session("LinguaID") = 1
        '	End If

        '	If PersonaID = 0 Then
        '		oPersona = COL_Persona.GetUtenteAnonimo(oLingua.Id)
        '	Else
        '		oPersona.Estrai(oLingua.Id)
        '		If oPersona.Errore <> Errori_Db.None Then
        '			oPersona = COL_Persona.GetUtenteAnonimo(oLingua.Id)
        '		End If
        '	End If

        '	If ComunitaID <= 0 Then
        '		Session("limbo") = True
        '		Session("IdRuolo") = Main.TipoRuoloStandard.AccessoNonAutenticato
        '		Session("IdComunita") = 0
        '		Session("ArrPermessi") = ""
        '		Session("ArrComunita") = ""
        '		Session("RLPC_ID") = Nothing
        '	Else
        '		Session("limbo") = False
        '		Session("IdComunita") = ComunitaID

        '		Dim oIscrizione As New COL_RuoloPersonaComunita
        '		oIscrizione.EstraiByLingua(ComunitaID, oPersona.Id, oLingua.Id)
        '		If oIscrizione.Errore = Errori_Db.None Then
        '			Session("RLPC_ID") = oIscrizione.Id
        '			Session("IdRuolo") = oIscrizione.TipoRuolo.Id
        '		Else
        '			Session("RLPC_ID") = Nothing
        '			Session("IdRuolo") = Main.TipoRuoloStandard.AccessoNonAutenticato
        '		End If
        '		Session("objPersona") = oPersona
        '		Session("ORGN_id") = oPersona.GetOrganizzazioneDefault
        '		Session("Istituzione") = oPersona.GetIstituzione
        '		Session("AdminForChange") = False
        '		Session("CMNT_path_forAdmin") = ""
        '		Session("idComunita_forAdmin") = ""

        '		'caricamento permessi
        '		Me.CaricamentoPermessi(Me.RuoloCorrenteID, Me.ComunitaCorrenteID)
        '		Me.CaricamentoPercorsoComunita(ComunitaID)
        '	End If
    End Sub

    'Private Sub CaricamentoPermessi(ByVal RuoloID As Integer, ByVal ComunitaID As Integer)
    '	Dim oLista As List(Of PlainService)
    '	oLista = COL_Servizio.LazyElencaByTipoRuoloByComunita(RuoloID, ComunitaID)

    '	Dim ArrPermessi(oLista.Count, 2) As String
    '	Dim indice As Integer = 0
    '	For Each oService As PlainService In oLista
    '		ArrPermessi(indice, 0) = oService.Code
    '		ArrPermessi(indice, 1) = oService.ID
    '		ArrPermessi(indice, 2) = oService.Permessi
    '		indice += 1
    '	Next
    '	Session("ArrPermessi") = ArrPermessi
    'End Sub
    'Private Sub CaricamentoPercorsoComunita(ByVal ComunitaID As Integer)
    '	Dim ArrComunita(,) As String = Nothing
    '	Dim oHistory As New HistoryElement

    '	Dim oComunita As New COL_Comunita(ComunitaID)
    '	oComunita.EstraiByLingua(Me.LinguaID)

    '	oHistory.ComunitaID = oComunita.Id
    '	oHistory.Percorso = "." & oComunita.Id & "."
    '	oHistory.Nome = oComunita.Nome
    '	oHistory.RuoloID = Me.RuoloCorrenteID
    '	oHistory.ComunitaPadreID = oComunita.IdPadre
    '	oHistory.SubElement = Nothing
    '	Dim PadreDirettoID As Integer = oComunita.IdPadre
    '	Dim ComunitaCorrenteID As Integer = oComunita.Id

    '	While PadreDirettoID <> 0
    '		Dim oOverHistory As New HistoryElement
    '		Dim Lista As List(Of COL_BusinessLogic_v2.IscrizioneComunita) = COL_Persona.LazyVerificaIscrizioneAPadri(ComunitaCorrenteID, Me.UtenteCorrente)
    '		Dim oComunitaDiretta, oComunitaPassante, oComunitaAnonima, oComunitaAccesso As COL_BusinessLogic_v2.IscrizioneComunita


    '		oComunitaDiretta = Lista.Find(New GenericPredicate(Of COL_BusinessLogic_v2.IscrizioneComunita, Integer)(PadreDirettoID, AddressOf COL_BusinessLogic_v2.IscrizioneComunita.FindByCommunityID))
    '		If Me.isAnonimo Then
    '			If Not IsNothing(oComunitaDiretta) Then
    '				oHistory = IscrizioneToHistory(oComunitaDiretta, oHistory)
    '				PadreDirettoID = oComunitaDiretta.Comunita.IdPadre
    '				ComunitaCorrenteID = oComunitaDiretta.Comunita.Id
    '			Else
    '				Exit While
    '			End If
    '		Else
    '			oComunitaAccesso = Lista.Find(New GenericPredicate(Of COL_BusinessLogic_v2.IscrizioneComunita, Boolean)(True, AddressOf COL_BusinessLogic_v2.IscrizioneComunita.FindByAccesso))
    '			If Not IsNothing(oComunitaAccesso) Then
    '				oHistory = IscrizioneToHistory(oComunitaAccesso, oHistory)
    '				PadreDirettoID = oComunitaAccesso.Comunita.IdPadre
    '				ComunitaCorrenteID = oComunitaAccesso.Comunita.Id
    '			Else
    '				oComunitaPassante = Lista.Find(New GenericPredicate(Of COL_BusinessLogic_v2.IscrizioneComunita, Boolean)(False, AddressOf COL_BusinessLogic_v2.IscrizioneComunita.FindByAccesso))
    '				If Not IsNothing(oComunitaPassante) Then
    '					oHistory = IscrizioneToHistory(oComunitaPassante, oHistory)
    '					PadreDirettoID = oComunitaPassante.Comunita.IdPadre
    '					ComunitaCorrenteID = oComunitaPassante.Comunita.Id
    '				Else
    '					oComunitaAnonima = Lista.Find(New GenericPredicate(Of COL_BusinessLogic_v2.IscrizioneComunita, Integer)(Main.TipoRuoloStandard.AccessoNonAutenticato, AddressOf COL_BusinessLogic_v2.IscrizioneComunita.FindByRuolo))
    '					If Not IsNothing(oComunitaAnonima) Then
    '						oHistory = IscrizioneToHistory(oComunitaAnonima, oHistory)
    '						PadreDirettoID = oComunitaAnonima.Comunita.IdPadre
    '						ComunitaCorrenteID = oComunitaAnonima.Comunita.Id
    '					Else
    '						PadreDirettoID = 0
    '					End If
    '				End If
    '			End If
    '		End If
    '	End While
    '	Session("ArrComunita") = HistoryToArray(oHistory)
    'End Sub

    'Private Function IscrizioneToHistory(ByVal Iscrizione As COL_BusinessLogic_v2.IscrizioneComunita, ByVal oHistory As HistoryElement) As HistoryElement
    '	Dim oOverHistory As New HistoryElement
    '	oOverHistory.ComunitaID = Iscrizione.Comunita.Id
    '	oOverHistory.Nome = Iscrizione.Comunita.Nome
    '	oOverHistory.Percorso = "." & Iscrizione.Comunita.Id & "."
    '	oOverHistory.RuoloID = Iscrizione.TipoRuolo.Id
    '	oOverHistory.SubElement = Nothing

    '	oHistory.ComunitaPadreID = oOverHistory.ComunitaID
    '	oOverHistory.SubElement = oHistory
    '	oHistory = oOverHistory
    '	'PadreDirettoID = oComunitaDiretta.Comunita.IdPadre
    '	'ComunitaCorrenteID = oComunitaDiretta.Comunita.Id

    '	Return oHistory
    'End Function
    'Private Function HistoryToArray(ByVal oHistory As HistoryElement) As String(,)
    '	Dim ListaComunita(,) As String = Nothing
    '	Dim Percorso As String = "."
    '	Dim oVoce As HistoryElement
    '	Dim indice As Integer

    '	oVoce = oHistory
    '	While Not IsNothing(oVoce)
    '		ReDim Preserve ListaComunita(3, indice)
    '		Percorso &= oVoce.ComunitaID & "."
    '		ListaComunita(0, indice) = oVoce.ComunitaID
    '		ListaComunita(1, indice) = oVoce.Nome
    '		ListaComunita(2, indice) = Percorso
    '		ListaComunita(3, indice) = oVoce.RuoloID
    '		oVoce = oVoce.SubElement
    '		indice += 1
    '	End While
    '	Return ListaComunita
    'End Function
    '#######################################################

#Region "Querystring"
    Public Const qs_ownerType As String = "owType="
    Public Const qs_questType As String = "type="
    Public Const qs_owner As String = "owId="
    Public Const qs_ownerG As String = "owGUID="
    Public Const qs_quest As String = "IdQ="
    Public Const qs_Persona As String = "IdP="
    Public Const qs_RandomQuest As String = "idrQ="

    Public ReadOnly Property qs_questIdType() As COL_Questionario.Questionario.TipoQuestionario
        Get
            Dim qType = COL_Questionario.Questionario.TipoQuestionario.Questionario
            If Not String.IsNullOrEmpty(Request.QueryString("type")) AndAlso IsNumeric(Request.QueryString("type")) Then
                Try
                    qType = CInt(Request.QueryString("type"))
                Catch ex As Exception
                    qType = COL_Questionario.Questionario.TipoQuestionario.Questionario
                End Try
            End If
            Return qType
        End Get
    End Property
    Public ReadOnly Property qs_ownerTypeId() As String
        Get
            Dim QueryStringItem As String = Request.QueryString("owType")
            If IsNumeric(QueryStringItem) Then
                Return QueryStringItem
            Else
                Return "0"
            End If
        End Get
    End Property
    Public ReadOnly Property qs_ownerId() As String
        Get
            Dim QueryStringItem As String = Request.QueryString("owId")
            If IsNumeric(QueryStringItem) Then
                Return QueryStringItem
            Else
                Return 0
            End If
        End Get
    End Property
    Public ReadOnly Property qs_ownerGUID() As System.Guid
        Get
            Dim QueryStringItem As String = Request.QueryString("owGUID")
            If RootObject.isGUID(QueryStringItem) Then
                Return New System.Guid(QueryStringItem)
            Else
                Return Nothing
            End If
        End Get
    End Property
    Public ReadOnly Property qs_questId() As Integer
        Get
            Dim QueryStringItem As String = Request.QueryString("IdQ")
            If IsNumeric(QueryStringItem) Then
                Return QueryStringItem
            Else
                Return 0
            End If
        End Get
    End Property
    Public ReadOnly Property qs_randomQuestId() As Integer
        Get
            Dim QueryStringItem As String = Request.QueryString("idrQ")
            If IsNumeric(QueryStringItem) Then
                Return QueryStringItem
            Else
                Return 0
            End If
        End Get
    End Property
    Public ReadOnly Property qs_PersonaId() As Integer
        Get
            Dim QueryStringItem As String = Request.QueryString("IdP")
            If IsNumeric(QueryStringItem) Then
                Return QueryStringItem
            Else
                Return 0
            End If
        End Get
    End Property
    Public ReadOnly Property qs_LanguageId() As String
        Get
            Dim QueryStringItem As String = Request.QueryString("LanguageId")
            If IsNumeric(QueryStringItem) Then
                Return QueryStringItem
            Else
                Return 1
            End If
        End Get
    End Property
#End Region


    Protected Function SubActivityCompleted(ByVal idLink As Long, ByVal idUser As Integer) As Boolean
        Return SubActivityCompleted(EPservice.ServiceStat.GetSubactivityStatusByModuleLink(idLink, lm.Comol.Modules.EduPath.Domain.SubActivityType.Quiz, idUser))
    End Function
    Private Function SubActivityCompleted(status As lm.Comol.Modules.EduPath.Domain.StatusStatistic) As Boolean
        Return EPservice.ServiceStat.CheckStatusStatistic(status, lm.Comol.Modules.EduPath.Domain.StatusStatistic.Completed)
    End Function
    Protected Sub executedAction(ByVal idLink As Long, ByVal isStarted As Boolean, ByVal isPassed As Boolean, ByVal Completion As Short, ByVal isCompleted As Boolean, ByVal Mark As Short, ByVal idUser As Integer)
        Dim status As lm.Comol.Modules.EduPath.Domain.StatusStatistic = lm.Comol.Modules.EduPath.Domain.StatusStatistic.None
        If Not isCompleted Then
            status = EPservice.ServiceStat.GetSubactivityStatusByModuleLink(idLink, lm.Comol.Modules.EduPath.Domain.SubActivityType.Quiz, idUser)
        End If
        Dim wcf As Boolean = False
        If wcf Then
            If Not (Not isCompleted AndAlso SubActivityCompleted(status)) Then
                Dim oSender As PermissionService.IServicePermission = Nothing
                Try
                    oSender = New PermissionService.ServicePermissionClient
                    oSender.ExecutedAction(idLink, isStarted, isPassed, Completion, isCompleted, Mark, idUser)
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
            End If
        Else
            ServiceEduPath.SaveActionExecution(idUser, idLink, isStarted, isPassed, Completion, isCompleted, Mark)
            'Dim idRole As Integer = ServiceEduPath.GetIdCommunityRole(idUser, source.CommunityID)
            'retval = ServiceEduPath.SaveActionExecution((actionType, source, destination, idUser, idRole)
        End If
    End Sub
    Protected Function allowStandardAction(ByVal actionType As lm.Comol.Core.DomainModel.StandardActionType, ByVal idUser As Integer, ByVal SourceCommunityID As Integer, ByVal source As lm.Comol.Core.DomainModel.ModuleObject, ByVal destination As lm.Comol.Core.DomainModel.ModuleObject) As Boolean
        Dim retval As Boolean = False
        '' ATTENZIONE COSA POTREI FARE ? 
        ' VADO VIA WCF O DIRETTO:
        Dim wcf As Boolean = False
        If wcf Then
            Dim oSender As PermissionService.IServicePermission = Nothing
            Try
                oSender = New PermissionService.ServicePermissionClient
                retval = oSender.AllowStandardAction(actionType, source, destination, idUser)

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
        Else
            '  int idRole = GetIdRole(session, idUser, source.CommunityID);
            'ServiceEduPath.
            Dim idRole As Integer = ServiceEduPath.GetIdCommunityRole(idUser, source.CommunityID)
            retval = ServiceEduPath.AllowStandardAction(actionType, source, destination, idUser, idRole)
        End If
        Return retval
    End Function

    Public Function ViewStateOrDefault(Of T)(ByVal Key As String, ByVal DefaultValue As T) As T
        If (ViewState(Key) Is Nothing) Then
            ViewState(Key) = DefaultValue
            Return DefaultValue
        Else
            Return ViewState(Key)
        End If
    End Function

    Protected Function LoadQuestionnaireById(idQuest As Integer) As Questionario
        Return LoadQuestionnaireById(idQuest, PageUtility.LinguaID)
    End Function
    Protected Function LoadQuestionnaireById(idQuest As Integer, idLanguage As Integer) As Questionario

        Dim quest As COL_Questionario.Questionario = DALQuestionario.readQuestionarioByQueryString(Me.PageUtility.CurrentContext, idQuest, idLanguage)
        Dim oGruppo As New QuestionarioGruppo With {.id = quest.idGruppo}
        Session("GruppoCorrente") = DALQuestionarioGruppo.readGruppoBYID(quest.idGruppo)
        GruppoQuestionariID = quest.idGruppo
        Me.QuestionarioCorrente = quest

        Me.LinguaQuestionario = idLanguage

        Return quest
    End Function
    Protected Function GetQuestionnaireById(idQuest As Integer, ByRef groupItem As QuestionarioGruppo) As Questionario
        Dim questLanguage As Integer = PageUtility.LinguaID
        Dim quest As COL_Questionario.Questionario = DALQuestionario.readQuestionarioByQueryString(Me.PageUtility.CurrentContext, idQuest, questLanguage)
        groupItem = DALQuestionarioGruppo.readGruppoBYID(quest.idGruppo)

        Return quest
    End Function
    Protected Function GetQuestionnaireById(idQuest As Integer) As Questionario
        Dim questLanguage As Integer = PageUtility.LinguaID
        Dim quest As COL_Questionario.Questionario = DALQuestionario.readQuestionarioByQueryString(Me.PageUtility.CurrentContext, idQuest, questLanguage)

        Return quest
    End Function

    Public Sub LoadQuestionnaireByUrl() Implements IviewQuestionario.LoadQuestionnaireByUrl
        Dim quest As Questionario = LoadQuestionnaireById(qs_questId, qs_LanguageId)
        quest.linguePresenti = DALQuestionario.readLingueQuestionario(qs_questId)
        Me.QuestionarioCorrente = quest 'ok

    End Sub
    Public MustOverride ReadOnly Property LoadDataByUrl() As Boolean



    ''' <summary>
    ''' Utente questionario su INVITO!!!
    ''' </summary>
    ''' <returns></returns>
    Protected ReadOnly Property UtenteQuestionario As COL_Persona
        Get

            If Not IsNothing(Invito) Then
                'Dim QuestUser As New COL_Persona

                If Invito.PersonaID = 0 OrElse Invito.PersonaID = idUtenteAnonimo() Then
                    Return MyBase.PageUtility.AnonymousCOL_Persona
                ElseIf Invito.PersonaID <> UtenteCorrente.ID Then
                    Throw New Exception("Invito non corrispondente!")
                End If
            End If

            Return UtenteCorrente
            'Dim QuestUser As COL_Persona

            'Try
            '    If Not IsNothing(Session("objPersonaQuest")) AndAlso TypeOf (Session("objPersonaQuest")) Is COL_Persona Then
            '        QuestUser = Session("objPersonaQuest")
            '    Else
            '        QuestUser = Nothing
            '    End If
            'Catch ex As Exception
            '    QuestUser = Nothing
            'End Try
            'Return QuestUser
        End Get
    End Property

    Public Sub CaricaInvito()
        Session("objPersonaQuest") = MyBase.PageUtility.AnonymousCOL_Persona
    End Sub
End Class