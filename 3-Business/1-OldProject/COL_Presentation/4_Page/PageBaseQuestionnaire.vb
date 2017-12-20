Imports COL_Questionario
Imports lm.Comol.Core.DomainModel

Public MustInherit Class PageBaseQuestionnaire
    Inherits PageBase

    Private _ModuleQuestionnaire As ModuleQuestionnaire
    Private _PageUtility As PresentationLayer.OLDpageUtility
    Private _serviceEduPath As lm.Comol.Modules.EduPath.BusinessLogic.Service
    Protected ReadOnly Property ServiceEduPath As lm.Comol.Modules.EduPath.BusinessLogic.Service
        Get
            If IsNothing(_serviceEduPath) Then
                _serviceEduPath = New lm.Comol.Modules.EduPath.BusinessLogic.Service(PageUtility.CurrentContext)
            End If
            Return _serviceEduPath
        End Get
    End Property
    Public ReadOnly Property PreLoadedContentView As lm.Comol.Core.DomainModel.ContentView
        Get
            If IsNumeric(Request.QueryString("CV")) Then
                Try
                    Return Request.QueryString("CV")
                Catch ex As Exception
                    Return lm.Comol.Core.DomainModel.ContentView.viewAll
                End Try
            Else
                Return lm.Comol.Core.DomainModel.ContentView.viewAll
            End If
        End Get
    End Property


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

    Sub New()
        MyBase.New()
    End Sub

#Region "gestione history view"


    ''Private _viewAttuale As VIWattiva '= Session("_viewAttuale")
    ''Public _viewPrecedente As VIWattiva ' = Session("_viewPrecedente")



    'Public Function viewPrecedente_isVisible() As Boolean
    '    If Session("_viewPrecedente") Is Nothing Or Session("_viewPrecedente") = Session("_viewAttuale") Then
    '        Return False
    '    Else
    '        Return True
    '        'Return VIWattiva.VIWerrore.Equals(Session("_viewPrecedente"))
    '        ' Return Not (Session("_viewPrecedente") = VIWattiva.VIWerrore)
    '    End If
    'End Function

    'Public Function viewPrecedente_readOnly() As Int16
    '    Return Session("_viewAttuale")
    'End Function

    'Public Property viewPrecedente() As Int16

    '    Get
    '        Session("_viewAttuale") = Session("_viewPrecedente")
    '        Session.Remove("_viewPrecedente")
    '        Return Session("_viewAttuale")
    '    End Get

    '    Set(ByVal value As Int16)
    '        Session("_viewPrecedente") = Session("_viewAttuale")
    '        Session("_viewAttuale") = value
    '    End Set
    'End Property

#End Region
    'Public Property isAnonymousCompiler() As Boolean Implements IviewQuestionario.isAnonymousCompiler
    '    Get
    '        Return Session("isAnonymousCompiler")
    '    End Get
    '    Set(ByVal value As Boolean)
    '        Session("isAnonymousCompiler") = value
    '    End Set
    'End Property
    'Public Property LinguaQuestionario() As Integer Implements IviewQuestionario.LinguaQuestionario
    '    Get
    '        Try
    '            If IsNumeric(Session("LinguaQuestionario")) Then
    '                LinguaQuestionario = CInt(Session("LinguaQuestionario"))
    '            Else
    '                LinguaQuestionario = 1
    '            End If
    '        Catch ex As Exception
    '            LinguaQuestionario = 1
    '        End Try
    '    End Get
    '    Set(ByVal value As Integer)
    '        Session("LinguaQuestionario") = value
    '    End Set
    'End Property

    'Public Property LinguaDefaultQuestionario() As Integer Implements IviewQuestionario.LinguaDefaultQuestionario
    '    Get
    '        Try
    '            If IsNumeric(Session("LinguaDefaultQuestionario")) Then
    '                LinguaDefaultQuestionario = CInt(Session("LinguaDefaultQuestionario"))
    '            Else
    '                LinguaDefaultQuestionario = 1
    '            End If
    '        Catch ex As Exception
    '            LinguaDefaultQuestionario = 1
    '        End Try
    '    End Get
    '    Set(ByVal value As Integer)
    '        Session("LinguaDefaultQuestionario") = value
    '    End Set
    'End Property

    'Public MustOverride ReadOnly Property isCompileForm() As Boolean


    'Public Property GruppoQuestionariID() As Integer Implements IviewQuestionario.GruppoQuestionariID
    '    Get
    '        Try
    '            GruppoQuestionariID = DirectCast(Session("GruppoQuestionariID"), Integer)
    '        Catch ex As Exception
    '            Session("GruppoQuestionariID") = 0
    '            GruppoQuestionariID = 0
    '        End Try
    '    End Get
    '    Set(ByVal value As Integer)
    '        Session("GruppoQuestionariID") = value
    '    End Set
    'End Property
    'Public Property GruppoDefaultID() As Integer Implements IviewQuestionario.GruppoDefaultID
    '    Get
    '        Try
    '            GruppoDefaultID = DirectCast(Session("GruppoDefaultID"), Integer)
    '        Catch ex As Exception

    '        End Try
    '    End Get
    '    Set(ByVal value As Integer)
    '        Session("GruppoDefaultID") = value
    '    End Set
    'End Property

    'Public ReadOnly Property Invito() As COL_Questionario.UtenteInvitato Implements IviewQuestionario.Invito
    '    Get
    '        Try
    '            Invito = DirectCast(Session("Invito"), COL_Questionario.UtenteInvitato)
    '        Catch ex As Exception
    '            Invito = New COL_Questionario.UtenteInvitato(0)
    '        End Try
    '    End Get
    'End Property

    'Public Property QuestionarioCorrente() As COL_Questionario.Questionario Implements IviewQuestionario.QuestionarioCorrente
    '    Get
    '        'Try
    '        Dim oQuest As Questionario
    '        oQuest = DirectCast(Session("QuestionarioCorrente"), COL_Questionario.Questionario)
    '        If oQuest Is Nothing Then
    '            oQuest = New Questionario
    '            Session("QuestionarioCorrente") = oQuest
    '        End If
    '        Return oQuest
    '    End Get
    '    Set(ByVal value As COL_Questionario.Questionario)
    '        Session("QuestionarioCorrente") = value
    '    End Set
    'End Property

    'Public Property DomandaCorrente() As COL_Questionario.Domanda Implements IviewQuestionario.DomandaCorrente
    '    Get
    '        Try
    '            DomandaCorrente = DirectCast(Session("DomandaCorrente"), COL_Questionario.Domanda)
    '        Catch ex As Exception
    '            DomandaCorrente = New Domanda
    '        End Try
    '    End Get
    '    Set(ByVal value As COL_Questionario.Domanda)
    '        Session("DomandaCorrente") = value
    '    End Set
    'End Property

    'Public Property PaginaCorrenteID() As Integer Implements IviewQuestionario.PaginaCorrenteID
    '    Get
    '        Try
    '            PaginaCorrenteID = Session("idPagina")
    '        Catch ex As Exception
    '            PaginaCorrenteID = 0
    '        End Try
    '    End Get
    '    Set(ByVal value As Integer)
    '        Session("idPagina") = value
    '    End Set
    'End Property

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.VerifyAuthentication = False OrElse Not IsSessioneScaduta(Me.VerifyAuthentication) Then
            If Page.IsPostBack = False Then
                Me.SetInternazionalizzazione()
            End If

            If HasPermessi() Then
                If AlwaysBind Or (Not AlwaysBind And Me.IsPostBack = False) Then
                    Me.BindDati()
                    If Page.IsPostBack = False Then
                        Me.RegistraAccessoPagina()
                    End If
                End If
            Else
                Me.BindNoPermessi()
            End If
        Else
            Me.SetInternazionalizzazione()
        End If
        'If Not IsSessioneScaduta(True) Then
        '    If Not Page.IsPostBack Then
        '        If Me.isCompileForm Then
        '            Me.ImpostazioneDatiComunitaByQuestionario()
        '        Else
        '            Me.ImpostazioneDatiBase()
        '        End If

        '        Me.SetInternazionalizzazione()
        '        If HasPermessi() Then
        '            Me.BindDati()
        '        Else
        '            Me.BindNoPermessi()
        '        End If
        '        Me.SetControlliByPermessi()
        '        Me.RegistraAccessoPagina()
        '    End If
        'ElseIf Not Page.IsPostBack Then
        '    Me.SetInternazionalizzazione()
        'End If
    End Sub

    'Public ReadOnly Property Servizio() As COL_BusinessLogic_v2.UCServices.Services_Questionario Implements IviewQuestionario.Servizio
    '    Get
    '        If IsNothing(_Servizio) Then
    '            If Me.ComunitaCorrenteID = 0 Then
    '                _Servizio = New Services_Questionario()
    '                With _Servizio
    '                    .Compila = (Me.UtenteCorrente.ID > 0)
    '                    .Admin = (Me.TipoPersonaID = Main.TipoPersonaStandard.AdminSecondario Or Me.TipoPersonaID = Main.TipoPersonaStandard.Amministrativo Or Me.TipoPersonaID = Main.TipoPersonaStandard.SysAdmin)

    '                End With
    '            Else
    '                _Servizio = Me.ElencoServizi.Find(Services_Questionario.Codex)
    '                If IsNothing(_Servizio) Then
    '                    _Servizio = New Services_Questionario()
    '                End If
    '            End If
    '        End If
    '        Servizio = _Servizio
    '    End Get
    'End Property

    'Public Property LibreriaCorrente() As COL_Questionario.Questionario Implements IviewQuestionario.LibreriaCorrente
    '    Get
    '        Try
    '            LibreriaCorrente = DirectCast(Session("LibreriaCorrente"), COL_Questionario.Questionario)
    '        Catch ex As Exception
    '            LibreriaCorrente = Nothing
    '        End Try
    '    End Get
    '    Set(ByVal value As COL_Questionario.Questionario)
    '        Session("LibreriaCorrente") = value
    '    End Set
    'End Property

    'Public Property GruppoCorrente() As COL_Questionario.QuestionarioGruppo Implements IviewQuestionario.GruppoCorrente
    '    Get
    '        Try
    '            GruppoCorrente = DirectCast(Session("GruppoCorrente"), COL_Questionario.QuestionarioGruppo)
    '        Catch ex As Exception
    '            GruppoCorrente = Nothing
    '        End Try

    '        If GruppoCorrente Is Nothing Then
    '            Dim oGruppo As New QuestionarioGruppo
    '            oGruppo.id = DALQuestionarioGruppo.GruppoPrincipaleByComunita_Id(Me.ComunitaCorrenteID)
    '            Session("GruppoCorrente") = oGruppo
    '        End If
    '    End Get
    '    Set(ByVal value As COL_Questionario.QuestionarioGruppo)
    '        Session("GruppoCorrente") = value
    '    End Set
    'End Property

    'Public Sub ImpostazioneDatiBase() Implements IviewQuestionario.ImpostazioneDatiBase
    '    If Me.isUtenteAnonimo And IsNothing(Me.UtenteCorrente) Then
    '        Me.CaricaAnonimo(0)
    '    End If

    '    If IsNothing(Session("Invito")) Then 'Or Session("Invito").GetType Is GetType(String) Then
    '        Session("Invito") = New COL_Questionario.UtenteInvitato(0)
    '    End If

    'End Sub

    'Public Sub ImpostazioneDatiComunitaByQuestionario() Implements IviewQuestionario.ImpostazioneDatiComunitaByQuestionario
    '    Dim InvitoID, LinguaQuestID As Integer
    '    Dim QueryQuestionarioID As Integer = 0
    '    Dim ReloadQuestionario As Boolean = True

    '    Dim idQuestionario As String = Me.EncryptedQueryString("idq", SecretKeyUtil.EncType.Questionario)
    '    Dim idLingua As String = EncryptedQueryString("idl", SecretKeyUtil.EncType.Questionario)
    '    Dim idInvito As String = EncryptedQueryString("idu", SecretKeyUtil.EncType.Questionario)

    '    If Not String.IsNullOrEmpty(idQuestionario) Then
    '        Try
    '            Integer.TryParse(idQuestionario, QueryQuestionarioID)
    '        Catch ex As Exception
    '            QueryQuestionarioID = 0
    '        End Try
    '    End If

    '    Dim oQuestionario As New Questionario
    '    oQuestionario.id = QueryQuestionarioID

    '    If Me.isUtenteAnonimo And IsNothing(Me.UtenteCorrente) Then
    '        Me.CaricaAnonimo(0)
    '    End If
    '    If Not String.IsNullOrEmpty(idLingua) Then
    '        Try
    '            Integer.TryParse(idLingua, LinguaQuestID)
    '        Catch ex As Exception
    '            LinguaQuestID = 0
    '        End Try
    '    Else
    '        LinguaQuestID = Me.LinguaID
    '    End If
    '    Me.LinguaQuestionario = LinguaQuestID

    '    If idInvito <> "" Then
    '        Try
    '            If idInvito <> "" Then
    '                InvitoID = DirectCast(CInt(idInvito), Integer)
    '                If Not DALQuestionario.isInvitato(oQuestionario.id, InvitoID) Then
    '                    Me.Invito.ID = -1
    '                    Exit Sub
    '                End If
    '            Else
    '                InvitoID = 0
    '            End If
    '        Catch ex As Exception
    '            InvitoID = 0
    '        End Try
    '        Dim oUtenteInvitato As UtenteInvitato
    '        oUtenteInvitato = DALUtenteInvitato.readUtenteInvitatoByID(InvitoID)
    '        If oUtenteInvitato.PersonaID = 0 Then
    '            oUtenteInvitato.PersonaID = idUtenteAnonimo()
    '        End If
    '        If oUtenteInvitato.ID = 0 Then
    '            InvitoID = 0
    '        End If
    '        Session("Invito") = oUtenteInvitato
    '    Else
    '        Session("Invito") = DALQuestionario.readInvitoByPersona(oQuestionario.id, Me.UtenteCorrente.ID)
    '        'Session("Invito") = New COL_Questionario.UtenteInvitato(0)
    '    End If
    '    Try
    '        'todo: [quest][notifiche] verificare che non abbia controindicazioni
    '        'senza l'"or" se si proviene da una notifica precedente al 15/12/2010 non viene caricata la risposta, viene quindi inserita una risposta doppia e si incasina tutto. Le notifiche son state cancellate.
    '        If Not Me.EncryptedQueryString("ida", SecretKeyUtil.EncType.Questionario) = "1" Then 'Or Request.Url.ToString.Contains(RootObject.compileUrl) Then
    '            oQuestionario = DALQuestionario.readQuestionarioByPersona(True, QueryQuestionarioID, LinguaQuestID, Me.UtenteCorrente.ID, Me.Invito.ID)
    '            isAnonymousCompiler = False
    '        Else
    '            'se il questionario e' stato aperto con un link anonimo non devono essere caricate risposte
    '            oQuestionario = DALQuestionario.readQuestionarioByPersona(True, QueryQuestionarioID, LinguaQuestID, -1, -1)
    '            isAnonymousCompiler = True
    '        End If
    '        If Me.EncryptedQueryString("pwd", SecretKeyUtil.EncType.Questionario) <> "" Then
    '            oQuestionario.isPassword = False
    '        End If
    '    Catch ex As Exception
    '        Dim errore As String
    '        errore = ex.Message
    '    End Try
    '    Me.QuestionarioCorrente = oQuestionario
    'End Sub
    Public Function idAnonymousUser() As Integer
        Return MyBase.PageUtility.AnonymousCOL_Persona.ID
    End Function

    Public Sub LoadAnonymousUser(ByVal PersonaID As Integer)
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
        Else
            oPersona.Estrai(oLingua.ID)
            If oPersona.Errore <> Errori_Db.None Then
                oPersona = MyBase.PageUtility.AnonymousCOL_Persona
            End If
        End If
        Session("objPersona") = oPersona
    End Sub

    '' ACTIONS 

    Public Function CreateObjectsList(ByVal oType As ModuleQuestionnaire.ObjectType, ByVal oValueID As String) As List(Of lm.ActionDataContract.ObjectAction)
        Dim oList As New List(Of lm.ActionDataContract.ObjectAction)
        oList.Add(New lm.ActionDataContract.ObjectAction With {.ObjectTypeId = oType, .ValueID = oValueID, .ModuleID = PageUtility.CurrentModule.ID})
        Return oList
    End Function

    Public Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(ModuleQuestionnaire.UniqueID)
    End Sub

#Region "Querystring"
    Private Const qs_ownerType As String = "owType="
    Private Const qs_questType As String = "type="
    Private Const qs_owner As String = "owId="
    Private Const qs_ownerG As String = "owGUID="
    Private Const qs_quest As String = "IdQ="
    Private Const qs_Persona As String = "IdP="
    Private Const qs_Link As String = "IdL="
    Private Const qs_Community As String = "IdC="
    Public ReadOnly Property qs_questTypeId() As Integer
        Get
            Dim idType As Integer = 0
            Integer.TryParse(Request.QueryString("type"), idType)
        End Get
    End Property
    Public ReadOnly Property qs_ownerTypeId() As Integer
        Get
            Dim idOwnerType As Integer = 0
            Integer.TryParse(Request.QueryString("owType"), idOwnerType)

            Return idOwnerType
        End Get
    End Property
    Public ReadOnly Property qs_ownerId() As Long
        Get
            Dim idOwner As Long = 0
            Long.TryParse(Request.QueryString("owId"), idOwner)

            Return idOwner
        End Get
    End Property
    Public ReadOnly Property qs_ownerGUID() As System.Guid
        Get
            Try
                Return New System.Guid(Request.QueryString("owGUID"))
            Catch ex As Exception
                Return Guid.Empty
            End Try
        End Get
    End Property
    Public ReadOnly Property qs_questId() As Integer
        Get
            Dim idQuest As Integer = 0
            Integer.TryParse(Request.QueryString("IdQ"), idQuest)
            Return idQuest
        End Get
    End Property
    Public ReadOnly Property qs_PersonaId() As Integer
        Get
            Dim idPerson As Integer = 0
            Integer.TryParse(Request.QueryString("IdP"), idPerson)

            Return idPerson
        End Get
    End Property
    Public ReadOnly Property qs_LinkId() As Long
        Get
            Dim idLink As Long = 0
            Long.TryParse(Request.QueryString("IdL"), idLink)

            Return idLink
        End Get
    End Property
    Public ReadOnly Property qs_CommunityID() As Integer
        Get
            Dim idCommunity As Integer = 0
            Integer.TryParse(Request.QueryString("IdC"), idCommunity)

            Return idCommunity
        End Get
    End Property
#End Region

#Region "Module Link functions"
    Protected Sub executedAction(ByVal LinkID As Long, ByVal UserID As Integer, ByVal isStarted As Boolean, ByVal isPassed As Boolean, ByVal Completion As Short, ByVal isCompleted As Boolean, ByVal Mark As Short)
        Dim oSender As PermissionService.IServicePermission = Nothing
        Try
            oSender = New PermissionService.ServicePermissionClient
            oSender.ExecutedAction(LinkID, isStarted, isPassed, Completion, isCompleted, Mark, UserID)

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
    End Sub
    Protected Function allowStandardAction(ByVal actionType As lm.Comol.Core.DomainModel.StandardActionType, ByVal idUser As Integer, ByVal SourceCommunityID As Integer, ByVal source As lm.Comol.Core.DomainModel.ModuleObject, ByVal destination As lm.Comol.Core.DomainModel.ModuleObject) As Boolean
        Dim retval As Boolean = False
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
            Dim idRole As Integer = ServiceEduPath.GetIdCommunityRole(idUser, source.CommunityID)
            retval = ServiceEduPath.AllowStandardAction(actionType, source, destination, idUser, idRole)
        End If
        Return retval
    End Function
    Protected Function GetModuleLinkPermission(IdCommunity As Integer, idUser As Integer, link As lm.Comol.Core.DomainModel.ModuleLink) As ModuleQuestionnaire
        Dim moduleQ As New ModuleQuestionnaire

        Dim oSender As PermissionService.IServicePermission = Nothing
        Try
            oSender = New PermissionService.ServicePermissionClient

            Dim actions As List(Of StandardActionType) = oSender.GetAllowedStandardAction(link.SourceItem, link.DestinationItem, idUser).ToList()
            
            moduleQ.Administration = actions.Contains(StandardActionType.Admin) OrElse actions.Contains(StandardActionType.Edit)
            moduleQ.Compile = actions.Contains(StandardActionType.Play)
            moduleQ.CopyQuestionnaire = actions.Contains(StandardActionType.Admin) OrElse actions.Contains(StandardActionType.Edit)
            moduleQ.DeleteQuestionnaire = actions.Contains(StandardActionType.Admin)
            moduleQ.GrantPermission = actions.Contains(StandardActionType.Admin) OrElse actions.Contains(StandardActionType.EditPermission)
            moduleQ.ViewStatistics = actions.Contains(StandardActionType.Admin) OrElse actions.Contains(StandardActionType.ViewAdvancedStatistics) OrElse actions.Contains(StandardActionType.ViewUserStatistics)
            moduleQ.ViewPersonalStatistics = actions.Contains(StandardActionType.Admin) OrElse actions.Contains(StandardActionType.ViewAdvancedStatistics) OrElse actions.Contains(StandardActionType.ViewUserStatistics) OrElse actions.Contains(StandardActionType.ViewPersonalStatistics)
            If Not IsNothing(oSender) Then
                Dim service As System.ServiceModel.ClientBase(Of PermissionService.IServicePermission) = DirectCast(oSender, System.ServiceModel.ClientBase(Of PermissionService.IServicePermission))
                service.Close()
                service = Nothing
            End If
        Catch ex As Exception
            If Not IsNothing(oSender) Then
                Dim service As System.ServiceModel.ClientBase(Of PermissionService.IServicePermission) = DirectCast(oSender, System.ServiceModel.ClientBase(Of PermissionService.IServicePermission))
                service.Close()
                service = Nothing
            End If
        End Try
        Return moduleQ
    End Function

#End Region

#Region "Retrieve data"
    'Protected Function LoadQuestionnaireById(idQuest As Integer) As Questionario
    '    Dim questLanguage As Integer = PageUtility.LinguaID
    '    Dim quest As COL_Questionario.Questionario = DALQuestionario.readQuestionarioByQueryString(idQuest, questLanguage)
    '    Dim oGruppo As New QuestionarioGruppo With {.id = quest.idGruppo}
    '    Session("GruppoCorrente") = DALQuestionarioGruppo.readGruppoBYID(quest.idGruppo)
    '    GruppoQuestionariID = quest.idGruppo
    '    Me.QuestionarioCorrente = quest

    '    Me.LinguaQuestionario = questLanguage

    '    Return quest
    'End Function
    Protected Function GetQuestionnaireById(idQuest As Integer, ByRef groupItem As QuestionarioGruppo) As Questionario
        Dim questLanguage As Integer = PageUtility.LinguaID
        Dim quest As COL_Questionario.Questionario = DALQuestionario.readQuestionarioByQueryString(Me.PageUtility.CurrentContext, idQuest, questLanguage)
        groupItem = DALQuestionarioGruppo.readGruppoBYID(quest.idGruppo)

        Return quest
    End Function
#End Region

    'Public MustOverride Overrides ReadOnly Property AlwaysBind() As Boolean
    'Public MustOverride Overrides Property VerifyAuthentication() As Boolean


End Class