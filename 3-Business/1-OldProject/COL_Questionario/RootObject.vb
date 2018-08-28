Imports Microsoft.VisualBasic
Imports System.Text.RegularExpressions
Imports System.Web.UI
Imports System.Reflection
Imports COL_BusinessLogic_v2.Comol.Manager
Imports Comol.Entity.Configuration

Public Class RootObject
    Private Shared m_HomeTopic As String

#Region " definizione percorsi pagine e controlli"
    Public Shared ReadOnly Property MasterPageUI() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("MasterPageUI").Path
        End Get
    End Property
    Public Shared ReadOnly Property quizIconPath() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("imgIconQuiz").Path
        End Get
    End Property
    Public Shared ReadOnly Property imgBandieraItaliano() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("imgBandieraItaliano").Path
        End Get
    End Property
    Public Shared ReadOnly Property imgBandieraInglese() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("imgBandieraInglese").Path
        End Get
    End Property
    Public Shared ReadOnly Property imgBandieraTedesco() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("imgBandieraTedesco").Path
        End Get
    End Property
    Public Shared ReadOnly Property imgBandieraFrancese() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("imgBandieraFrancese").Path
        End Get
    End Property
    Public Shared ReadOnly Property imgBandieraSpagnolo() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("imgBandieraSpagnolo").Path
        End Get
    End Property
    Public Shared ReadOnly Property QuestionariGestioneList() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("QuestionariGestioneList").Path
        End Get
    End Property
    Public Shared ReadOnly Property EsameGestione() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("EsameGestione").Path
        End Get
    End Property
    Public Shared ReadOnly Property Esame() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("Esame").Path
        End Get
    End Property
    Public Shared ReadOnly Property QuestionariGestioneListShort() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("QuestionariGestioneListShort").Path
        End Get
    End Property
    Public Shared ReadOnly Property QuestionariStatList() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("QuestionariStatList").Path
        End Get
    End Property
    Public Shared ReadOnly Property QuestionariCestinoList() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("QuestionariCestinoList").Path
        End Get
    End Property
    Public Shared ReadOnly Property QuestionariList() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("QuestionariList").Path
        End Get
    End Property
    Public Shared ReadOnly Property ModelliGestioneList() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("ModelliGestioneList").Path
        End Get
    End Property
    Public Shared ReadOnly Property DomandaAdd() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("DomandaAdd").Path
        End Get
    End Property
    Public Shared ReadOnly Property DomandaEdit() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("DomandaEdit").Path
        End Get
    End Property
    Public Shared ReadOnly Property GruppoAdmin() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("GruppoAdmin").Path
        End Get
    End Property
    Public Shared ReadOnly Property QuestionarioAdd() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("QuestionarioAdd").Path
        End Get
    End Property
    Public Shared ReadOnly Property QuestionarioAdmin() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("QuestionarioAdmin").Path
        End Get
    End Property
    Public Shared ReadOnly Property MeetingAdmin() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("MeetingAdmin").Path
        End Get
    End Property
    Public Shared ReadOnly Property MeetingWiz() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("MeetingWiz").Path
        End Get
    End Property
    Public Shared ReadOnly Property MeetingWizShort() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("MeetingWizShort").Path
        End Get
    End Property
    Public Shared ReadOnly Property SondaggioAdmin() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("SondaggioAdmin").Path
        End Get
    End Property
    Public Shared ReadOnly Property QuestionarioAdminShort() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("QuestionarioAdminShort").Path
        End Get
    End Property
    Public Shared ReadOnly Property SondaggioAdminShort() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("SondaggioAdminShort").Path
        End Get
    End Property
    Public Shared ReadOnly Property MeetingAdminShort() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("MeetingAdminShort").Path
        End Get
    End Property
    Public Shared ReadOnly Property QuestionarioView() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("QuestionarioView").Path
        End Get
    End Property
    Public Shared ReadOnly Property QuestionarioStatisticheGenerali() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("QuestionarioStatisticheGenerali").Path
        End Get
    End Property
    Public Shared ReadOnly Property QuestionarioStatistiche() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("QuestionarioStatistiche").Path
        End Get
    End Property
    Public Shared ReadOnly Property QuestionarioStatisticheGeneraliShort() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("QuestionarioStatisticheGeneraliShort").Path
        End Get
    End Property
    Public Shared ReadOnly Property QuestionarioStatisticheUtenti() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("QuestionarioStatisticheUtenti").Path
        End Get
    End Property
    Public Shared ReadOnly Property PaginaEdit() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("PaginaEdit").Path
        End Get
    End Property
    Public Shared ReadOnly Property QuestionarioEdit() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("QuestionarioEdit").Path
        End Get
    End Property
    Public Shared ReadOnly Property QuestionarioEditShort() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("QuestionarioEditShort").Path
        End Get
    End Property
    Public Shared ReadOnly Property UtentiInvitati() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("UtentiInvitati").Path
        End Get
    End Property
    Public Shared ReadOnly Property Risposte() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("Risposte").Path
        End Get
    End Property
    Public Shared ReadOnly Property RisposteLibere() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("RisposteLibere").Path
        End Get
    End Property

    Public Shared Function DisplayModuleActionControl() As String
        Return "Modules/Questionnaire/UC/UC_ModuleQuizAction.ascx"
    End Function
    Public Shared Function LibraryViewAllQuestions(idQuestionnaire As Integer, idLanguage As Integer, idType As Integer) As String
        Return "Questionari/LibraryViewAll.aspx?type=" & idType.ToString() & "&IdQ=" & idQuestionnaire.ToString & "&idLanguage=" & idLanguage
    End Function
    'Public Shared Function ucDisplayActionControl() As String
    '    Return ManagerConfiguration.GetInstance.Quiz.GetUrl("ucDisplayActionControl").Path
    'End Function
    Public Shared ReadOnly Property ucDomandaMultiplaEdit() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("ucDomandaMultiplaEdit").Path
        End Get
    End Property
    'Public Shared ReadOnly Property ucDomandaMultiplaEditWizard() As String
    '    Get
    '        Return ManagerConfiguration.GetInstance.Quiz.GetUrl("ucDomandaMultiplaEditWizard").Path
    '    End Get
    'End Property
    Public Shared ReadOnly Property ucHeaderCompile() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("ucHeaderCompile").Path
        End Get
    End Property
    Public Shared ReadOnly Property ucDomandaRatingEdit() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("ucDomandaRatingEdit").Path
        End Get
    End Property

    Public Shared ReadOnly Property ucDomandaRatingStarsEdit() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("ucDomandaRatingStarsEdit").Path
        End Get
    End Property

    Public Shared ReadOnly Property ucDomandaDropDownEdit() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("ucDomandaDropDownEdit").Path
        End Get
    End Property
    Public Shared ReadOnly Property ucDomandaDropDown() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("ucDomandaDropDown").Path
        End Get
    End Property
    Public Shared ReadOnly Property ucDomandaMeetingEdit() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("ucDomandaMeetingEdit").Path
        End Get
    End Property
    Public Shared ReadOnly Property ucDomandaMeeting() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("ucDomandaMeeting").Path
        End Get
    End Property
    Public Shared ReadOnly Property ucDomandaTestoLiberoEdit() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("ucDomandaTestoLiberoEdit").Path
        End Get
    End Property
    Public Shared ReadOnly Property ucDomandaNumericaEdit() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("ucDomandaNumericaEdit").Path
        End Get
    End Property
    Public Shared ReadOnly Property ucDomandeLista() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("ucDomandeLista").Path
        End Get
    End Property
    Public Shared ReadOnly Property ucDomandeListaView() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("ucDomandeListaView").Path
        End Get
    End Property
    Public Shared ReadOnly Property ucStatisticheGenerali() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("ucStatisticheGenerali").Path
        End Get
    End Property
    Public Shared ReadOnly Property ucStatisticheUtenti() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("ucStatisticheUtenti").Path
        End Get
    End Property
    Public Shared ReadOnly Property ucUserAttemptsStatistics() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("ucUserAttemptsStatistics").Path
        End Get
    End Property
    Public Shared ReadOnly Property ucPNLValutazione() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("ucPNLValutazione").Path
        End Get
    End Property
    Public Shared ReadOnly Property ucVisualizzaRisposta() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("ucVisualizzaRisposta").Path
        End Get
    End Property
    Public Shared ReadOnly Property compileUrlUI() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("compileUrlUI").Path
        End Get
    End Property
    Public Shared ReadOnly Property compileUrlUI_short() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("compileUrlUI_short").Path
        End Get
    End Property
    Public Shared ReadOnly Property compileUrl() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("compileUrl").Path
        End Get
    End Property
    Public Shared ReadOnly Property GenerateQuestionnaireUrl(ByVal idQuestionnaire As Integer) As String
        Get
            Return "Questionari/GenerateQuestionnaireUrl.aspx?idQ=" & idQuestionnaire.ToString
        End Get
    End Property
    Public Shared ReadOnly Property compileUrl_short() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("compileUrl_short").Path
        End Get
    End Property
    Public Shared ReadOnly Property HistoryAutovalutazione() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("HistoryAutovalutazione").Path
        End Get
    End Property
    Public Shared ReadOnly Property HistoryAutovalutazioneShort() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("HistoryAutovalutazione_short").Path
        End Get
    End Property
    Public Shared ReadOnly Property helpQuestionario() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("helpQuestionario").Path
        End Get
    End Property
    Public Shared ReadOnly Property helpUtentiInvitati_mail() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("helpUtentiInvitati_mail").Path
        End Get
    End Property
    Public Shared ReadOnly Property helpUtentiInvitati_dettagli() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("helpUtentiInvitati_dettagli").Path
        End Get
    End Property
    Public Shared ReadOnly Property helpUtentiInvitati_lista() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("helpUtentiInvitati_lista").Path
        End Get
    End Property
    Public Shared ReadOnly Property helpUtentiInvitati_listaEdit() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("helpUtentiInvitati_listaEdit").Path
        End Get
    End Property
    Public Shared ReadOnly Property helpQuestionariGestioneList() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("helpQuestionariGestioneList").Path
        End Get
    End Property
    Public Shared ReadOnly Property helpSondaggiGestioneList() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("helpSondaggiGestioneList").Path
        End Get
    End Property
    Public Shared ReadOnly Property helpQuestionariEdit() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("helpQuestionariEdit").Path
        End Get
    End Property
    Public Shared ReadOnly Property helpQuestionariList() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("helpQuestionariList").Path
        End Get
    End Property
    Public Shared ReadOnly Property helpSondaggiList() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("helpSondaggiList").Path
        End Get
    End Property
    Public Shared ReadOnly Property anteprimaTemplate() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("anteprimaTemplate").Path
        End Get
    End Property
    Public Shared ReadOnly Property helpDomandaMultipla() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("helpDomandaMultipla").Path
        End Get
    End Property
    Public Shared ReadOnly Property helpDomandaTestoLibero() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("helpDomandaTestoLibero").Path
        End Get
    End Property
    Public Shared ReadOnly Property helpDomandaRating() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("helpDomandaRating").Path
        End Get
    End Property
    Public Shared ReadOnly Property helpDomandaNumerica() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("helpDomandaNumerica").Path
        End Get
    End Property
    Public Shared ReadOnly Property helpDomandaDropDown() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("helpDomandaDropDown").Path
        End Get
    End Property
    Public Shared ReadOnly Property GruppiList() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("GruppiList").Path
        End Get
    End Property
    Public Shared ReadOnly Property ReportElencoUtentiInvitati() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("ReportElencoUtentiInvitati").Path
        End Get
    End Property
    Public Shared ReadOnly Property ReportFogliUtentiInvitatiDomande() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("ReportFogliUtentiInvitatiDomande").Path
        End Get
    End Property
    Public Shared ReadOnly Property WizardDomandaMultipla() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("WizardDomandaMultipla").Path
        End Get
    End Property
    Public Shared ReadOnly Property WizardDomandaTestoLibero() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("WizardDomandaTestoLibero").Path
        End Get
    End Property
    Public Shared ReadOnly Property WizardDomandaNumerica() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("WizardDomandaNumerica").Path
        End Get
    End Property
    Public Shared ReadOnly Property WizardDomandaRating() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("WizardDomandaRating").Path
        End Get
    End Property

    Public Shared ReadOnly Property WizardDomandaRatingStars() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("WizardDomandaRatingStars").Path
        End Get
    End Property

    Public Shared ReadOnly Property WizardDomandaDropDown() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("WizardDomandaDropDown").Path
        End Get
    End Property
    

    Public Shared ReadOnly Property urlQuestionarioEsame() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("urlQuestionarioEsame").Path
        End Get
    End Property

    Public Shared ReadOnly Property LibreriaDomandeSelect() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("LibreriaDomandeSelect").Path
        End Get
    End Property
    Public Shared ReadOnly Property HomePagePortale() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.GetUrl("HomePagePortale").Path
        End Get
    End Property
    ''' <summary>
    ''' Path per tornare all'update dell'activity nella quale e' stato inserito il questionario
    ''' </summary>
    ''' <param name="ActivityId"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property EduPath_CreateInActivity(ByVal ActivityId As Integer, ByVal idQuest As Integer, ByVal owType As Int64) As String
        Get
            'verificare lo step quando la pagina verra' implementata.
            Return String.Format(ManagerConfiguration.GetInstance.Quiz.GetUrl("EduPath_CreateInActivity").Path, ActivityId, idQuest, owType)
        End Get
    End Property
    ''' <summary>
    ''' Path per tornare all'activity dopo aver compilato il questionario (partecipante)
    ''' </summary>
    ''' <param name="ActivityId"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property EduPath_CompileInActivity(ByVal ActivityId As Integer) As String
        Get
            'verificare lo step quando la pagina verra' implementata.
            Return String.Format(ManagerConfiguration.GetInstance.Quiz.GetUrl("EduPath_CompileInActivity").Path, ActivityId.ToString)
        End Get
    End Property
    ''' <summary>
    ''' Path per tornare all'activity dopo aver visualizzato le stat del questionario (amministratore). I partecipanti tornano su EduPath_CompileInActivity.
    ''' </summary>
    ''' <param name="ActivityId"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    'Public Shared ReadOnly Property EduPath_StatInActivity(ByVal ActivityId As Integer) As String
    '    Get
    '        Return ManagerConfiguration.GetInstance.Quiz.GetUrl("EduPath_StatInActivity").Path & "?AId=" & ActivityId.ToString
    '    End Get
    'End Property
    'Public Shared ReadOnly Property EduPath_StatInSubActivity(ByVal ActivityId As Integer) As String
    '    Get
    '        Return ManagerConfiguration.GetInstance.Quiz.GetUrl("EduPath_StatInSubActivity").Path & "?saId=" & ActivityId.ToString
    '    End Get
    'End Property
    'Public Shared ReadOnly Property EduPath_UserStatInSubActivity(ByVal ActivityId As Integer, ByVal CommunityId As Integer, ByVal PersonID As Integer) As String
    '    Get
    '        Return String.Format(ManagerConfiguration.GetInstance.Quiz.GetUrl("EduPath_UserStatInSubActivity").Path, ActivityId.ToString, CommunityId, PersonID)
    '    End Get
    'End Property
    Public Shared ReadOnly Property CreateQuiz(ByVal idOwner As Int64, ByVal ownerType As Int64) As String
        Get
            Return String.Format(ManagerConfiguration.GetInstance.Quiz.GetUrl("CreateQuiz").Path, idOwner, ownerType)
        End Get
    End Property
    Public Shared ReadOnly Property CreateQuiz(ByVal idOwner As Int64, ByVal ownerType As Integer, ByVal qType As QuestionnaireType) As String
        Get
            Return String.Format(ManagerConfiguration.GetInstance.Quiz.GetUrl("CreateQuizByType").Path, idOwner, ownerType, CInt(qType))
        End Get
    End Property
    Public Shared ReadOnly Property UpdateQuiz(ByVal idActivity As Int64, ByVal idQuiz As Int64, ByVal ownerType As Integer, ByVal qType As QuestionnaireType) As String
        Get
            Return String.Format(ManagerConfiguration.GetInstance.Quiz.GetUrl("UpdateQuizByType").Path, idActivity, idQuiz, ownerType, CInt(qType))
        End Get
    End Property
    Public Shared ReadOnly Property UserStat(ByVal idActivity As Int64, ByVal idQuiz As Int64, ByVal ownerType As Integer, ByVal idUser As Integer) As String
        Get
            Return String.Format(ManagerConfiguration.GetInstance.Quiz.GetUrl("StatQuiz").Path, idActivity, idQuiz, ownerType, idUser, CInt(TipoStatistiche.Utente))
        End Get
    End Property
    Public Shared ReadOnly Property UserRandomAttemptsStat(ByVal idActivity As Int64, ByVal idQuiz As Int64, ByVal qType As QuestionnaireType, ByVal ownerType As Integer, ByVal idUser As Integer, attemptsType As StatisticsType) As String
        Get
            Return String.Format(ManagerConfiguration.GetInstance.Quiz.GetUrl("UserRandomAttemptsStat").Path, idActivity, idQuiz, ownerType, idUser, attemptsType.ToString, CInt(qType))
        End Get
    End Property
    Public Shared ReadOnly Property GlobalStat(ByVal idActivity As Int64, ByVal idQuiz As Int64, ByVal ownerType As Integer, ByVal idUser As Integer) As String
        Get
            Return String.Format(ManagerConfiguration.GetInstance.Quiz.GetUrl("StatQuiz").Path, idActivity, idQuiz, ownerType, idUser, CInt(TipoStatistiche.Generali))
        End Get
    End Property
    Public Shared ReadOnly Property UsersStat(ByVal idActivity As Int64, ByVal idQuiz As Int64, ByVal ownerType As Integer, ByVal idUser As Integer) As String
        Get
            Return String.Format(ManagerConfiguration.GetInstance.Quiz.GetUrl("StatQuiz").Path, idActivity, idQuiz, ownerType, idUser, CInt(TipoStatistiche.Utenti))
        End Get
    End Property
    Public Shared ReadOnly Property CompileQuiz(ByVal idActivity As Int64, ByVal idQuiz As Long, ByVal qType As QuestionnaireType, ByVal ownerType As Integer, ByVal idLink As Long) As String
        Get
            Return CompileRandomQuiz(idActivity, idQuiz, 0, qType, ownerType, idLink)
        End Get
    End Property
    Public Shared ReadOnly Property CompileRandomQuiz(ByVal idActivity As Int64, ByVal idQuiz As Long, ByVal idRandomQuiz As Long, ByVal qType As QuestionnaireType, ByVal ownerType As Integer, ByVal idLink As Long) As String
        Get
            Return String.Format(ManagerConfiguration.GetInstance.Quiz.GetUrl("CompileQuizByType").Path, idActivity, idQuiz, ownerType, idLink, CInt(qType), idRandomQuiz)
        End Get
    End Property
    Public Shared ReadOnly Property CompileQuestionnaire(ByVal idActivity As Int64, ByVal idQuiz As Int64, ByVal ownerType As Integer, ByVal LinkId As Int64) As String
        Get
            Return String.Format(ManagerConfiguration.GetInstance.Quiz.GetUrl("CompileQuiz").Path, idActivity, idQuiz, ownerType, LinkId)
        End Get
    End Property

    Public Shared ReadOnly Property ViewAdvancedStatistics(ByVal idActivity As Long, ByVal idQuiz As Long, ByVal ownerType As Integer, ByVal idLink As Long, ByVal backUrl As String) As String
        Get
            Return String.Format(ManagerConfiguration.GetInstance.Quiz.GetUrl("ViewAdvancedStatistics").Path, idActivity, idQuiz, ownerType, idLink, backUrl)
        End Get
    End Property
    Public Shared ReadOnly Property ViewUserStatistics(ByVal idActivity As Long, ByVal idQuiz As Long, ByVal ownerType As Integer, ByVal idLink As Long, ByVal backUrl As String) As String
        Get
            Return String.Format(ManagerConfiguration.GetInstance.Quiz.GetUrl("ViewUserStatistics").Path, idActivity, idQuiz, ownerType, idLink, backUrl)
        End Get
    End Property
    Public Shared ReadOnly Property ViewUserStatisticsStandard(ByVal idQuiz As Integer, ByVal idCommunity As Integer, idUser As Integer) As String
        Get
            Return String.Format(ManagerConfiguration.GetInstance.Quiz.GetUrl("ViewUserStatisticsStandard").Path, idQuiz, idCommunity, idUser)
        End Get
    End Property
    Public Shared ReadOnly Property ViewUserStatisticsStandard(ByVal idQuiz As Integer, ByVal idCommunity As Integer, idUser As Integer, ByVal backUrl As String) As String
        Get
            Return "Modules/Questionnaire/UserStatistics.aspx?idQ=" & idQuiz.ToString & "&idC=" & idCommunity & "&IdP=" & idUser & "&BackUrl=" & backUrl
        End Get
    End Property
#End Region

#Region " definizione costanti"

    Public Shared ReadOnly Property nomeGruppoDefault() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.DefaultGroupName
        End Get
    End Property

    Public Shared ReadOnly Property scalaValutazione() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.DefaultScalaValutazione
        End Get
    End Property

    Public Shared ReadOnly Property vitaSessione_max() As Int32
        Get
            Return ManagerConfiguration.GetInstance.Quiz.SessionTimeout
        End Get
    End Property

    Public Shared ReadOnly Property ValidatorMaxDouble() As Double
        Get
            Return ManagerConfiguration.GetInstance.Quiz.MaxDoubleSize
        End Get
    End Property

    Public Shared ReadOnly Property nQuestionsPerPage_Default() As Int16
        Get
            Try
                Return ManagerConfiguration.GetInstance.Quiz.QuestionForPage
            Catch ex As Exception
                Return 5
            End Try
        End Get
    End Property

    Public Shared ReadOnly Property tickMassimo() As Int32
        'timer setting for session's keep-alive
        Get
            Return ManagerConfiguration.GetInstance.Quiz.MaxSessionAliveTick
            '  Return 600000
        End Get
    End Property
    Public Shared ReadOnly Property autoSaveTimer() As Int32
        'answers saved every autoSaveTimer milliseconds (max)
        Get
            Return ManagerConfiguration.GetInstance.Quiz.AutoSave
            ' Return 30000
        End Get
    End Property
    Public Shared ReadOnly Property maxOvertimeSalvataggio() As Int32
        Get
            Return ManagerConfiguration.GetInstance.Quiz.OvertimeSave
            'Return 60
        End Get
    End Property

    Public Shared ReadOnly Property lunghezzaOpzioniDropDown() As Integer
        Get
            Return ManagerConfiguration.GetInstance.Quiz.ItemForDropDown
            '  Return 50
        End Get
    End Property

    Public Shared ReadOnly Property popUpHeight() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.PopUpHeight
            'Return 800
        End Get
    End Property

    Public Shared ReadOnly Property popUpWidth() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.PopUpWidth
            '	Return 750
        End Get
    End Property

    Public Shared ReadOnly Property nRighePaginaGridView() As String
        Get
            Return ManagerConfiguration.GetInstance.Quiz.RowItemsForPage
            'Return 20
        End Get
    End Property

#End Region
    ''' <summary>
    ''' Ricorsiva, toglie tutti i "br" a fine stringa (inseriti ad es. dall'editor telerik), sostituisce i nbsp; con " "
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function removeBRfromStringEnd(ByRef value As String) As String
        value = value.Replace("&nbsp;", " ").Trim
        If value.EndsWith("<br>") Then
            Return removeBRfromStringEnd(value.Remove(value.Length - 4)) 'vengono rimossi anche piu' br consecutivi
        Else
            Return value
        End If
    End Function
    Public Shared Function parseInt(ByRef value As String) As Integer
        If String.IsNullOrEmpty(value) Then
            Return 0
        Else
            Return Integer.Parse(value)
        End If
    End Function
    Public Shared Function apriPopUp(ByVal url As String, ByVal target As String, ByVal resize As String, ByVal scrollbars As String) As String

        Dim strScript As String = "window.open('" + url + "', " + target + ", 'height=" + popUpHeight + ",width=" + popUpWidth + ",resizable=" + resize + ",location=no,scrollbars=" + scrollbars + "');return false;"

        Return strScript

    End Function
    Public Shared Function encryptUrl(ByVal obj As Object) As String
        If obj Is Nothing Or obj Is System.DBNull.Value Then
            Return String.Empty
        Else
            Return obj.ToString()
        End If
    End Function
    Public Shared Function mailAdminQuestionari() As String
        Return ManagerConfiguration.GetMailLocalized(Nothing).ErrorSender.Address
    End Function

#Region "isNullSomething"
    Public Shared Function isNullString(ByVal obj As Object) As String

        If obj Is Nothing Or obj Is System.DBNull.Value Then
            Return String.Empty
        Else
            Return obj.ToString()
        End If

    End Function
    Public Shared Function isNullBigInt(ByVal obj As Object) As Int64
        If obj Is Nothing Or obj Is System.DBNull.Value Then
            Return 0 'con integer.minvalue si incasina tutta la gestione delle durate e non si hanno vantaggi
        Else
            Return Integer.Parse(obj)
        End If
    End Function
    Public Shared Function isNullDateMin(ByVal obj As Object) As Date

        If obj Is Nothing Or obj Is System.DBNull.Value Then
            Return Date.MinValue
        Else
            Return Convert.ToDateTime(obj)
        End If

    End Function
    Public Shared Function isNullDateMax(ByVal obj As Object) As Date

        If obj Is Nothing Or obj Is System.DBNull.Value Then
            Return Date.MaxValue
        Else
            Return Convert.ToDateTime(obj)
        End If

    End Function
    Public Shared Function isNullDecimal(ByVal obj As Object) As Decimal

        If obj Is Nothing Or obj Is System.DBNull.Value Then
            Return Decimal.MinValue
        Else
            Return Decimal.Parse(obj)
        End If

    End Function
    Public Shared Function isNullInt(ByVal obj As Object) As Integer
        If obj Is Nothing Or obj Is System.DBNull.Value Then
            Return 0 'con integer.minvalue si incasina tutta la gestione delle durate e non si hanno vantaggi
        Else
            Return Integer.Parse(obj)
        End If

    End Function
    Public Shared Function isNullDouble(ByVal obj As Object) As Double
        If obj Is Nothing Or obj Is System.DBNull.Value Then
            Return 0
        Else
            Return Double.Parse(obj)
        End If

    End Function
    Public Shared Function isNullBoolean(ByVal obj As Object) As Integer
        If obj Is Nothing Or obj Is System.DBNull.Value Then
            Return False
        Else
            Return Boolean.Parse(obj)
        End If

    End Function
    Public Shared Function isNullGuid(ByVal obj As Object) As Guid
        If obj Is Nothing Or obj Is System.DBNull.Value Then
            Return Guid.Empty
        Else
            Return New Guid(obj.ToString)
        End If
    End Function
#End Region
#Region "SetNullSomething"
    Public Shared Function setNullDate(ByVal obj As Object) As Object

        If obj Is Nothing Then
            Return System.DBNull.Value
        Else
            If obj = Date.MinValue.ToString() Or obj.ToString() = "" Then
                Return System.DBNull.Value
            Else
                Try
                    If Convert.ToDateTime(obj).Year >= 2099 Then
                        Return System.DBNull.Value
                    Else
                        Return Convert.ToDateTime(obj)
                    End If
                Catch ex As Exception
                    Return System.DBNull.Value
                End Try
                Return Convert.ToDateTime(obj)
            End If
        End If
    End Function
    Public Shared Function setNull(ByVal obj As Object) As Object

        If obj Is Nothing OrElse Convert.ToInt32(obj) = 0 Then
            Return System.DBNull.Value
        Else
            Return Convert.ToInt32(obj)
        End If

    End Function
    Public Shared Function setNullInt(ByVal value As Integer) As Object

        If value = 0 Then
            Return System.DBNull.Value
        Else
            Return value
        End If

    End Function
    Public Shared Function setNullDecimal(ByVal value As Decimal) As Object
        If value = Decimal.MinValue Then
            Return System.DBNull.Value
        Else
            Return value
        End If
    End Function
#End Region

    Public Shared Function StripHTML(ByVal strHTML As String) As String
        Dim objRegExp As New Regex("<(.|\n)+?>", RegexOptions.IgnoreCase)
        Dim strOutput As String

        strOutput = objRegExp.Replace(strHTML, "")

        Return strOutput
    End Function

    Public Shared Function RadDate_ToList(ByRef oCalendar As Telerik.Web.UI.Calendar.Collections.DateTimeCollection) As List(Of Date)
        Dim retVal As New List(Of Date)
        'Dim oCalendar As New Telerik.WebControls.Base.Calendar.Collections.DateTimeCollection
        'oCalendar = DirectCast(FRVDomanda.FindControl("RDCLCalendar"), RadCalendar).SelectedDates
        For Each oData As Telerik.Web.UI.RadDate In oCalendar
            retVal.Add(oData.Date)
        Next
        Return retVal
    End Function

    Public Shared Function isGUID(ByRef GUID As String) As Boolean
        If Not String.IsNullOrEmpty(GUID) Then
            Dim isGuid_RegEx As Regex = New Regex("^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", RegexOptions.Compiled)
            Return isGuid_RegEx.IsMatch(GUID)
        Else
            Return False
        End If
    End Function
#Region "ManageOwner"
    ''' <summary>
    '''     ''' </summary>
    ''' <param name="ownerType"></param>
    ''' <param name="PersonId">Id dell'utente proprietario dell'oggetto sul quale si vogliono ottenere i permessi</param>
    ''' <param name="CurrentPersonId">Id dell'utente corrente</param>
    ''' <param name="ownerId"></param>
    ''' <param name="ownerGUID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function RequestAuthorizationToOwner(ByVal CurrentContext As Global.lm.Comol.Core.DomainModel.iApplicationContext, ByRef ownerType As Integer, ByRef CurrentPersonId As Integer, ByRef questType As Integer, ByRef permission As EduPath_Permission, ByRef currentRoleId As Integer, Optional ByRef ownerId As Int64 = 0, Optional ByRef ownerGUID As Guid = Nothing, Optional ByRef personID As Integer = 0) As Boolean
        Select Case permission
            Case EduPath_Permission.Create
                Dim oDalExternal As New DALExternal
                Return oDalExternal.EduPathCanCreate(CurrentContext, ownerType, ownerId, CurrentPersonId, questType, currentRoleId)
            Case EduPath_Permission.Compile
                Return False 'da rifare con If PageUtility.PermissionService.AllowStandardAction(lm.Comol.Core.DomainModel.StandardActionType.Play, UtenteCorrente.ID, 0, oSourceObject, oDestinationObject) Then
            Case EduPath_Permission.StatUser
                If personID = 0 OrElse CurrentPersonId = personID Then
                    Return False 'da rifare con If PageUtility.PermissionService.AllowStandardAction(lm.Comol.Core.DomainModel.StandardActionType.Play, UtenteCorrente.ID, 0, oSourceObject, oDestinationObject) Then DALExternal.EduPathCanCompile(CurrentContext, ownerType, ownerId, CurrentPersonId, questType, currentRoleId)
                Else
                    Dim oDalExternal As New DALExternal
                    Return oDalExternal.EduPathCanCreate(CurrentContext, ownerType, ownerId, CurrentPersonId, questType, currentRoleId)
                End If
            Case EduPath_Permission.StatQuiz
                Dim oDalExternal As New DALExternal
                Return oDalExternal.EduPathCanCreate(CurrentContext, ownerType, ownerId, CurrentPersonId, questType, currentRoleId)
            Case EduPath_Permission.Evaluate
                Dim oDalExternal As New DALExternal
                Return oDalExternal.EduPathCanEvaluate(CurrentContext, ownerType, ownerId, CurrentPersonId, questType, currentRoleId)
        End Select
    End Function
    Public Enum EduPath_Permission
        None = 0
        StatQuiz = 1
        StatUser = 2
        Compile = 4
        Create = 8
        Evaluate = 16
    End Enum

#End Region
End Class
