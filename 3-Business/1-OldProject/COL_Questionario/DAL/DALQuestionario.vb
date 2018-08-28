Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports System.Data.Common
Imports System.Collections.Generic
Imports System.Data
Imports System.Linq
Imports COL_Questionario.RootObject
Imports COL_BusinessLogic_v2.Comunita
Imports COL_Questionario.Business
Imports lm.Comol.Core.DomainModel

Public Class DALQuestionario

    Private Shared Function GetService(appContext As iApplicationContext) As ServiceQuestionnaire
        Return New ServiceQuestionnaire(appContext)
    End Function
    Public Shared Function GetQuestionnaire(appContext As iApplicationContext, ByVal idQuestionnaire As Integer) As LazyQuestionnaire
        Dim s As ServiceQuestionnaire = GetService(appContext)
        If IsNothing(s) Then
            Return Nothing
        Else
            Return s.GetItem(Of LazyQuestionnaire)(idQuestionnaire)
        End If
    End Function
    Public Shared Function GetQuestionnaireAttempts(appContext As iApplicationContext, idQuestionnaire As Integer, idUser As Integer, idInvitedUser As Integer) As List(Of LazyUserResponse)
        Dim s As ServiceQuestionnaire = GetService(appContext)
        If IsNothing(s) Then
            Return Nothing
        Else
            Return s.GetQuestionnaireAttempts(idQuestionnaire, idUser, idInvitedUser)
        End If
    End Function
    Public Shared Function CalculateAttemptComplation(appContext As iApplicationContext, userResponse As LazyUserResponse) As dtoItemEvaluation(Of Long)
        Dim s As ServiceQuestionnaire = GetService(appContext)
        If IsNothing(s) Then
            Return Nothing
        Else
            Return s.CalculateComplation(userResponse)
        End If
    End Function
    Public Shared Function CalculateComplation(appContext As iApplicationContext, idQuestionnaire As Integer, idUser As Integer, idResponse As Integer) As dtoItemEvaluation(Of Long)
        Dim s As ServiceQuestionnaire = GetService(appContext)
        If IsNothing(s) Then
            Return Nothing
        Else
            Return s.CalculateComplation(idQuestionnaire, idUser, idResponse)
        End If
    End Function

    Public Shared Function CalculateComplationRandomId(appContext As iApplicationContext, idQuestionnaire As Integer, idUser As Integer, idAttempt As Integer) As dtoItemEvaluation(Of Long)
        Dim s As ServiceQuestionnaire = GetService(appContext)
        If IsNothing(s) Then
            Return Nothing
        Else

            Dim lurId As Integer = TestRandom(idQuestionnaire, idUser, idAttempt)

            Return s.CalculateComplation(idQuestionnaire, idUser, lurId)
        End If
    End Function

    Private Shared Function TestRandom(idQuest As Integer, idUser As Integer, idRandom As Integer) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim connection As DbConnection = db.CreateConnection()
        Dim lurId As Integer = 0

        Try
            Using connection
                connection.Open()
                Dim sqlCommand As String = "sp_Questionario_LazyUserResponseGET"
                Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
                dbCommand.CommandTimeout = 1200
                dbCommand.Connection = connection

                db.AddInParameter(dbCommand, "idQuest", DbType.Int32, idQuest)
                db.AddInParameter(dbCommand, "idUser", DbType.Int32, idUser)
                db.AddInParameter(dbCommand, "idRandom", DbType.Int32, idRandom)

                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()

                        lurId = isNullInt(sqlReader.Item("QSTN_Id"))
                    End While
                    sqlReader.Close()
                End Using
                connection.Close()
            End Using
        Catch ex As Exception

        End Try

        Return lurId
    End Function

    'Public Shared Function CalculateComplationAttempt(appContext As iApplicationContext, idQuestionnaire As Integer, idUser As Integer, idAttempt As Integer) As dtoItemEvaluation(Of Long)
    '    Dim s As ServiceQuestionnaire = GetService(appContext)
    '    If IsNothing(s) Then
    '        Return Nothing
    '    Else
    '        Return s.CalculateComplation(idQuestionnaire, idUser, idAttempt, True)
    '    End If
    'End Function


    Private Shared Function SaveRepeatSettings(appContext As iApplicationContext, ByVal idQuestionnaire As Integer, minScore As Integer, maxAttempts As Integer, displayScoreToUser As Boolean, displayAttemptScoreToUser As Boolean, displayAvailableAttempts As Boolean, displayResultsStatus As Boolean, displayCurrentAttempts As Boolean) As Boolean
        Dim s As ServiceQuestionnaire = GetService(appContext)
        If IsNothing(s) Then
            Return False
        Else
            Return s.SaveRepeatSettings(idQuestionnaire, minScore, maxAttempts, displayScoreToUser, displayAttemptScoreToUser, displayAvailableAttempts, displayResultsStatus, displayCurrentAttempts)
        End If
    End Function
    Private Shared Function SaveSettings(appContext As iApplicationContext, ByVal idQuestionnaire As Integer, displayScoreToUser As Boolean, displayAttemptScoreToUser As Boolean, displayAvailableAttempts As Boolean, displayResultsStatus As Boolean, displayCurrentAttempts As Boolean) As Boolean
        Dim s As ServiceQuestionnaire = GetService(appContext)
        If IsNothing(s) Then
            Return False
        Else
            Return s.SaveSettings(idQuestionnaire, displayScoreToUser, displayAttemptScoreToUser, displayAvailableAttempts, displayResultsStatus, displayCurrentAttempts)
        End If
    End Function
    Public Shared Function GetPersonName(appContext As iApplicationContext, idUser As Integer) As String
        Dim s As ServiceQuestionnaire = GetService(appContext)
        If IsNothing(s) Then
            Return ""
        Else
            Return s.GetPersonName(idUser)
        End If
    End Function

    Public Shared Function GetDisplayNameByResponses(appContext As iApplicationContext, items As List(Of COL_Questionario.RispostaQuestionario), anonymousUser As String) As Dictionary(Of String, String)
        Dim s As ServiceQuestionnaire = GetService(appContext)
        If IsNothing(s) Then
            Return New Dictionary(Of String, String)
        Else
            Return s.GetDisplayNameByResponses(items, anonymousUser)
        End If
    End Function

    Public Shared Function GetDtoDisplayNameByResponses(appContext As iApplicationContext, items As List(Of COL_Questionario.RispostaQuestionario), anonymousUser As String) As Dictionary(Of String, dtoDisplayName)
        Dim s As ServiceQuestionnaire = GetService(appContext)
        If IsNothing(s) Then
            Return New Dictionary(Of String, dtoDisplayName)
        Else
            Return s.GetDtoDisplayNameByResponses(items, anonymousUser)
        End If
    End Function

    Public Shared Function GetDtoDisplayNameByAnonymousResponses(appContext As iApplicationContext, items As List(Of COL_Questionario.RispostaQuestionario), anonymousUser As String) As Dictionary(Of String, dtoDisplayName)
        Dim s As ServiceQuestionnaire = GetService(appContext)
        If IsNothing(s) Then
            Return New Dictionary(Of String, dtoDisplayName)
        Else
            Return s.GetDtoDisplayNameByAnonymousResponses(items, anonymousUser)
        End If
    End Function


    'Private Shared Function GetRandomChildren(appContext As iApplicationContext, ByVal idQuestionnaire As Integer, idUser As Integer, ByVal invitedUser As Integer) As List(Of LazyQuestionnaireRandom)
    '    Dim s As ServiceQuestionnaire = GetService(appContext)
    '    If IsNothing(s) Then
    '        Return New List(Of LazyQuestionnaireRandom)()
    '    Else
    '        Return s.GetQuestionnaireChildren(idQuestionnaire, idUser, invitedUser)
    '    End If
    'End Function

    Public Shared Function IsDuplicatedName(ByVal idCommunity As Integer, idQuest As Integer, idtype As Integer, ByVal name As String) As Boolean
        Return GetDuplicatedItemsByName(idCommunity, idQuest, idtype, name).Any
    End Function
    Public Shared Function GetDuplicatedItemsByName(ByVal idCommunity As Integer, idQuest As Integer, idtype As Integer, ByVal name As String) As List(Of Integer)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim connection As DbConnection = db.CreateConnection()
        Dim items As New List(Of Integer)

        Using connection
            connection.Open()
            Dim sqlCommand As String = "sp_Questionario_GetDuplicatedItemsByName"
            Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            dbCommand.CommandTimeout = 1200
            dbCommand.Connection = connection

            db.AddInParameter(dbCommand, "idQuest", DbType.Int32, idQuest)
            db.AddInParameter(dbCommand, "idType", DbType.Int32, idtype)
            db.AddInParameter(dbCommand, "idComunita", DbType.Int32, idCommunity)
            db.AddInParameter(dbCommand, "nome", DbType.String, name)

            Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                While sqlReader.Read()
                    items.Add(isNullInt(sqlReader.Item("QSTN_Id")))
                End While
                sqlReader.Close()
            End Using
            connection.Close()
        End Using
        Return items.Where(Function(i) i > 0).ToList
    End Function
    'Public Shared Function controllaNome(ByVal idComunita As Integer, ByVal Nome As String) As Integer
    '    Dim db As Database = DatabaseFactory.CreateDatabase()
    '    Dim retVal As Integer

    '    Dim sqlCommand As String = "sp_Questionario_Questionario_ControlloNome"
    '    Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
    '    db.AddInParameter(dbCommand, "idComunita", DbType.String, idComunita)
    '    db.AddInParameter(dbCommand, "Nome", DbType.String, Nome)

    '    retVal = db.ExecuteScalar(dbCommand)
    '    Return retVal
    'End Function
    Public Shared Function isInvitato(ByVal idQuestionario As Integer, ByVal idUtenteInvitato As Integer) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim retVal As Integer

        Dim sqlCommand As String = "sp_Questionario_isInvitato"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        db.AddInParameter(dbCommand, "idQuestionario", DbType.String, idQuestionario)
        db.AddInParameter(dbCommand, "idUtenteInvitato", DbType.String, idUtenteInvitato)
        retVal = db.ExecuteScalar(dbCommand)

        If retVal = 0 Then
            Return False
        Else
            Return True
        End If
    End Function
    Public Shared Function readInvitoByPersona(ByVal idQuestionario As Integer, ByVal idPersona As Integer) As UtenteInvitato
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim retVal As Integer

        Dim sqlCommand As String = "sp_Questionario_readIDInvitoByPersona"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        db.AddInParameter(dbCommand, "idQuestionario", DbType.String, idQuestionario)
        db.AddInParameter(dbCommand, "idPersona", DbType.String, idPersona)
        retVal = db.ExecuteScalar(dbCommand)

        Dim oUI As New UtenteInvitato
        oUI.ID = retVal

        Return oUI

    End Function
    Public Shared Function readIdQuestionarioByIdUtenteInvitato(ByRef idUtenteInvitato As Integer) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim retVal As Integer

        Dim sqlCommand As String = "sp_idQuestionarioByIdUtenteInvitato_select"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        db.AddInParameter(dbCommand, "idUtenteInvitato", DbType.Int32, idUtenteInvitato)
        Try
            retVal = db.ExecuteScalar(dbCommand)
        Catch ex As Exception

        End Try
        Return retVal
    End Function
    Public Shared Function isPasswordVerified(ByRef idQuestionario As Integer, ByRef idUtenteInvitato As Integer, ByRef password As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim retVal As Boolean

        Dim sqlCommand As String = "sp_isPasswordVerified"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuestionario)
        db.AddInParameter(dbCommand, "idUtenteInvitato", DbType.Int32, idUtenteInvitato)
        db.AddInParameter(dbCommand, "password", DbType.String, password)
        Try
            retVal = (db.ExecuteScalar(dbCommand) > 0)
        Catch ex As Exception

        End Try
        Return retVal
    End Function
    Public Shared Function updateQuestionarioDefault(ByVal idQuestionario As Integer, ByVal idLingua As Integer) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim retVal As Integer

        Dim sqlCommand As String = "sp_Questionario_UpdateQuestionarioDefault"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuestionario)
        db.AddInParameter(dbCommand, "idLingua", DbType.Int32, idLingua)

        Try
            retVal = db.ExecuteScalar(dbCommand)
        Catch ex As Exception

        End Try
        Return retVal

    End Function
    Public Shared Function updateIsPassword(ByRef idQuest As Integer, ByRef isPassword As Boolean)
        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim sqlCommand As String = "sp_Questionario_Questionario_UpdateIsPassword"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuest)
        db.AddInParameter(dbCommand, "isPassword", DbType.Boolean, isPassword)

        Try
            db.ExecuteNonQuery(dbCommand)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Shared Function readQuestionarioBYLingua(appContext As iApplicationContext, ByVal idQuestionario As Integer, ByVal idLingua As Integer, ByVal caricaRisposte As Boolean, Optional ByVal isStats As Boolean = False) As Questionario
        Dim _oQuestionario As New Questionario

        Dim oGestioneRisposte As New RispostaQuestionario

        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim connection As DbConnection = db.CreateConnection()

        Using connection
            connection.Open()

            _oQuestionario = readCampiQuestionario(appContext, idQuestionario, idLingua, db, connection)

            _oQuestionario.linguePresenti = readLingueQuestionario(_oQuestionario.id)

            Try
                If _oQuestionario.tipo = Questionario.TipoQuestionario.Random OrElse _oQuestionario.tipo = Questionario.TipoQuestionario.RandomRepeat OrElse _oQuestionario.tipo = Questionario.TipoQuestionario.Autovalutazione Then

                    _oQuestionario.librerieQuestionario = DALQuestionario.readQuestionarioLibrerie(_oQuestionario)

                    If caricaRisposte Then

                        _oQuestionario.risposteQuestionario = DALRisposte.readRisposteBYIDQuestionario(_oQuestionario.id, 0, 0, db, connection)

                    End If

                Else

                    DALPagine.readPagineByIDQuestionario(_oQuestionario, db, connection)

                    DALDomande.readDomandeByPagina(_oQuestionario, db, connection)

                    If _oQuestionario.tipo = Questionario.TipoQuestionario.LibreriaDiDomande And isStats Then
                        DALDomande.readDomandeOldByLibreria(_oQuestionario, db, connection)
                    End If

                    If caricaRisposte Then
                        _oQuestionario.risposteQuestionario = DALRisposte.readRisposteBYIDQuestionario(_oQuestionario.id, 0, 0, db, connection)
                    End If

                    readDomandeOpzioniRisposte(_oQuestionario, db, connection)
                End If

            Catch ex As Exception
                Dim a As String = ex.Message
            End Try
            connection.Close()

        End Using

        Return _oQuestionario

    End Function

    Public Shared Function readFullLibraryBYLingua(appContext As iApplicationContext, ByVal idQuestionario As Integer, ByVal idLingua As Integer) As Questionario
        Dim _oQuestionario As New Questionario

        Dim oGestioneRisposte As New RispostaQuestionario

        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim connection As DbConnection = db.CreateConnection()

        Using connection
            connection.Open()

            _oQuestionario = readCampiQuestionario(appContext, idQuestionario, idLingua, db, connection)

            _oQuestionario.linguePresenti = readLingueQuestionario(_oQuestionario.id)

            Try


                DALPagine.readPagineByIDQuestionario(_oQuestionario, db, connection)

                DALDomande.readDomandeByPagina(_oQuestionario, db, connection)

                If _oQuestionario.tipo = Questionario.TipoQuestionario.LibreriaDiDomande Then
                    DALDomande.readDomandeOldByLibreria(_oQuestionario, db, connection)
                    If Not IsNothing(_oQuestionario.pagine.Last) Then
                        'Dim domanda As Domanda = _oQuestionario.pagine.Last.domande.OrderByDescending(Function(d) d.numero).Skip(0).Take(1).FirstOrDefault()
                        'Dim max As Integer = 0
                        'If IsNothing(domanda) Then
                        '    max = _oQuestionario.pagine.Select(Function(d) d.domande.Count).Sum()
                        'Else
                        '    max = domanda.numero + 1
                        'End If
                    End If
                End If
                readDomandeOpzioniRisposte(_oQuestionario, db, connection)
            Catch ex As Exception
                Dim a As String = ex.Message
            End Try
            connection.Close()

        End Using

        Return _oQuestionario

    End Function

    Public Shared Function readQuestionarioByQueryString(appContext As iApplicationContext, ByVal idQuestionario As Integer, ByRef questLanguage As Integer) As Questionario
        Dim _oQuestionario As New Questionario
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim connection As DbConnection = db.CreateConnection()

        Using connection
            connection.Open()
            _oQuestionario.linguePresenti = readLingueQuestionario(idQuestionario)

            Dim idLanguage As Integer = questLanguage
            If _oQuestionario.linguePresenti.Where(Function(l) l.ID = idLanguage).Any() Then
                idLanguage = questLanguage
            ElseIf _oQuestionario.linguePresenti.Where(Function(l) l.isDefault).Any() Then
                idLanguage = _oQuestionario.linguePresenti.Where(Function(l) l.isDefault).Select(Function(l) l.ID).FirstOrDefault
            ElseIf _oQuestionario.linguePresenti.Count > 0 Then
                idLanguage = _oQuestionario.linguePresenti.Select(Function(l) l.ID).FirstOrDefault
            End If
            questLanguage = idLanguage
            _oQuestionario = readLazyCampiQuestionario(appContext, idQuestionario, idLanguage, db, connection)
            connection.Close()

        End Using

        Return _oQuestionario

    End Function

    ''' <summary>
    ''' Legge il nome del questionario richiesto
    ''' </summary>
    ''' <param name="idQuest"></param>
    ''' <param name="idLanguage"></param>
    ''' <returns>Name of questionnaire</returns>
    ''' <remarks></remarks>
    Public Shared Function getName(ByRef idQuest As Integer, ByRef idLanguage As Integer) As String
        Dim retval As String = String.Empty
        Dim db As Database
        Dim connection As DbConnection
        db = DatabaseFactory.CreateDatabase()
        connection = db.CreateConnection()
        Dim sqlCommand As String = "sp_Questionario_QuestionarioByLingua_Select"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        dbCommand.Connection = connection
        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuest)
        db.AddInParameter(dbCommand, "idLingua", DbType.Int32, idLanguage)

        Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
            While sqlReader.Read()
                retval = isNullString(sqlReader.Item("QSML_Nome"))
            End While
            sqlReader.Close()
        End Using
        Return retval
    End Function
    ''' <summary>
    ''' reads Quiz fields, not objects linked to quiz
    ''' </summary>
    ''' <param name="idQuestionario"></param>
    ''' <param name="idLingua"></param>
    ''' <param name="db">if is nothing, it's inizialized</param>
    ''' <param name="connection">if is nothing, it's inizialized</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    'appContext As iApplicationContext,
    Private Shared Function readCampiQuestionario(appContext As iApplicationContext, ByVal idQuestionario As Integer, ByVal idLingua As Integer, ByVal db As Database, ByVal connection As DbConnection) As Questionario
        If db Is Nothing Then
            db = DatabaseFactory.CreateDatabase()
            connection = db.CreateConnection()
        ElseIf connection Is Nothing Then
            connection = db.CreateConnection()
        End If
        Dim oQuest As New Questionario

        oQuest.id = idQuestionario
        oQuest.idLingua = idLingua

        Dim sqlCommand As String = "sp_Questionario_QuestionarioByLingua_Select"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        dbCommand.CommandTimeout = 1200
        dbCommand.Connection = connection
        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, oQuest.id)
        db.AddInParameter(dbCommand, "idLingua", DbType.Int32, oQuest.idLingua)
        '    Dim t As Integer = connection.ConnectionTimeout

        'Dim service As ServiceQuestionnaire = GetService(appContext)

        Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
            While sqlReader.Read()
                oQuest.id = isNullInt(sqlReader.Item("QSTN_Id"))
                oQuest.nome = isNullString(sqlReader.Item("QSML_Nome"))
                oQuest.descrizione = isNullString(sqlReader.Item("QSML_Descrizione"))
                oQuest.dataCreazione = isNullString(sqlReader.Item("QSTN_DataCreazione"))
                oQuest.idPersonaCreator = isNullInt(sqlReader.Item("QSTN_PRSN_Creator_Id"))
                oQuest.idGruppo = isNullInt(sqlReader.Item("QSTN_QSGR_Id"))
                oQuest.isCancellato = isNullBoolean(sqlReader.Item("QSML_IsCancellato"))
                oQuest.isBloccato = isNullBoolean(sqlReader.Item("QSML_IsBloccato"))
                oQuest.dataInizio = isNullDateMin(sqlReader.Item("QSTN_DataInizio"))
                oQuest.dataFine = isNullDateMax(sqlReader.Item("QSTN_DataFine"))
                oQuest.idLingua = isNullInt(sqlReader.Item("QSML_IdLingua"))
                oQuest.pesoTotale = isNullInt(sqlReader.Item("QSTN_pesoTotale"))
                oQuest.scalaValutazione = isNullInt(sqlReader.Item("QSTN_scalaValutazione"))
                oQuest.isReadOnly = isNullBoolean(sqlReader.Item("QSTN_IsChiuso"))
                oQuest.idQuestionarioMultilingua = isNullInt(sqlReader.Item("QSML_Id"))
                oQuest.isDefault = isNullBoolean(sqlReader.Item("QSML_IsDefault"))
                oQuest.durata = isNullInt(sqlReader.Item("QSTN_Durata"))
                oQuest.risultatiAnonimi = isNullBoolean(sqlReader.Item("QSTN_RisultatiAnonimi"))
                oQuest.forUtentiComunita = isNullBoolean(sqlReader.Item("QSTN_forUtentiComunita"))
                oQuest.forUtentiPortale = isNullBoolean(sqlReader.Item("QSTN_forUtentiPortale"))
                oQuest.forUtentiInvitati = isNullBoolean(sqlReader.Item("QSTN_forUtentiInvitati"))
                oQuest.forUtentiEsterni = isNullBoolean(sqlReader.Item("QSTN_forUtentiEsterni"))
                oQuest.visualizzaRisposta = isNullBoolean(sqlReader.Item("QSTN_visualizzaRisposta"))
                oQuest.visualizzaCorrezione = isNullBoolean(sqlReader.Item("QSTN_visualizzaCorrezione"))
                oQuest.visualizzaSuggerimenti = isNullBoolean(sqlReader.Item("QSTN_visualizzaSuggerimenti"))
                oQuest.editaRisposta = isNullBoolean(sqlReader.Item("QSTN_editaRisposta"))
                oQuest.tipoGrafico = isNullInt(sqlReader.Item("QSTN_TPGF_Id"))
                oQuest.tipo = isNullInt(sqlReader.Item("QSTN_Tipo"))
                oQuest.creator = isNullString(sqlReader.Item("creator"))
                oQuest.dataModifica = isNullString(sqlReader.Item("QSTN_DataModifica"))
                oQuest.idPersonaEditor = isNullInt(sqlReader.Item("QSTN_PRSN_Editor_Id"))
                oQuest.isRandomOrder = isNullBoolean(sqlReader.Item("QSTN_isRandomOrder"))
                oQuest.editor = isNullString(sqlReader.Item("editor"))
                oQuest.nDomandeDiffBassa = isNullInt(sqlReader.Item("QSTN_nDomandeDiffBassa"))
                oQuest.nDomandeDiffMedia = isNullInt(sqlReader.Item("QSTN_nDomandeDiffMedia"))
                oQuest.nDomandeDiffAlta = isNullInt(sqlReader.Item("QSTN_nDomandeDiffAlta"))
                oQuest.isRandomOrder_Options = isNullBoolean(sqlReader.Item("QSTN_isRandomOrder_Options"))
                oQuest.nQuestionsPerPage = isNullInt(sqlReader.Item("QSTN_nQuestionsPerPage"))
                oQuest.isPassword = isNullBoolean(sqlReader.Item("QSTN_isPassword"))
                oQuest.ownerType = isNullInt(sqlReader.Item("QSTN_ownerType"))
                oQuest.ownerId = isNullBigInt(sqlReader.Item("QSTN_ownerId"))
                oQuest.ownerGUID = isNullGuid(sqlReader.Item("QSTN_ownerGUID"))

                Dim lQuest As LazyQuestionnaire = GetQuestionnaire(appContext, idQuestionario)
                If Not IsNothing(lQuest) Then
                    If oQuest.tipo = QuestionnaireType.RandomMultipleAttempts Then
                        'Dim lQuest As LazyQuestionnaire = GetQuestionnaire(appContext, idQuestionario)
                        If Not IsNothing(lQuest) Then
                            oQuest.MinScore = lQuest.MinScore
                            oQuest.MaxAttempts = lQuest.MaxAttempts
                        End If
                    End If
                    oQuest.DisplayScoreToUser = lQuest.DisplayScoreToUser
                    oQuest.DisplayAttemptScoreToUser = lQuest.DisplayAttemptScoreToUser
                    oQuest.DisplayAvailableAttempts = lQuest.DisplayAvailableAttempts
                    oQuest.DisplayResultsStatus = lQuest.DisplayResultsStatus
                    oQuest.DisplayCurrentAttempts = lQuest.DisplayCurrentAttempts
                End If

            End While
            sqlReader.Close()
        End Using
        Return oQuest
    End Function
    Private Shared Function readLazyCampiQuestionario(appContext As iApplicationContext, ByVal idQuestionario As Integer, ByVal idLingua As Integer, ByVal db As Database, ByVal connection As DbConnection) As Questionario
        If db Is Nothing Then
            db = DatabaseFactory.CreateDatabase()
            connection = db.CreateConnection()
        ElseIf connection Is Nothing Then
            connection = db.CreateConnection()
        End If
        Dim oQuest As New Questionario

        oQuest.id = idQuestionario
        oQuest.idLingua = idLingua

        Dim sqlCommand As String = "sp_Questionario_QuestionarioByLingua_Select"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        dbCommand.CommandTimeout = 1200
        dbCommand.Connection = connection
        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, oQuest.id)
        db.AddInParameter(dbCommand, "idLingua", DbType.Int32, oQuest.idLingua)
        '    Dim t As Integer = connection.ConnectionTimeout

        Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
            While sqlReader.Read()
                oQuest.id = isNullInt(sqlReader.Item("QSTN_Id"))
                oQuest.nome = isNullString(sqlReader.Item("QSML_Nome"))
                oQuest.descrizione = isNullString(sqlReader.Item("QSML_Descrizione"))
                oQuest.dataCreazione = isNullString(sqlReader.Item("QSTN_DataCreazione"))
                oQuest.idPersonaCreator = isNullInt(sqlReader.Item("QSTN_PRSN_Creator_Id"))
                oQuest.idGruppo = isNullInt(sqlReader.Item("QSTN_QSGR_Id"))
                oQuest.isCancellato = isNullBoolean(sqlReader.Item("QSML_IsCancellato"))
                oQuest.isBloccato = isNullBoolean(sqlReader.Item("QSML_IsBloccato"))
                oQuest.dataInizio = isNullDateMin(sqlReader.Item("QSTN_DataInizio"))
                oQuest.dataFine = isNullDateMax(sqlReader.Item("QSTN_DataFine"))
                oQuest.idLingua = isNullInt(sqlReader.Item("QSML_IdLingua"))
                oQuest.pesoTotale = isNullInt(sqlReader.Item("QSTN_pesoTotale"))
                oQuest.scalaValutazione = isNullInt(sqlReader.Item("QSTN_scalaValutazione"))
                oQuest.isReadOnly = isNullBoolean(sqlReader.Item("QSTN_IsChiuso"))
                oQuest.idQuestionarioMultilingua = isNullInt(sqlReader.Item("QSML_Id"))
                oQuest.isDefault = isNullBoolean(sqlReader.Item("QSML_IsDefault"))
                oQuest.durata = isNullInt(sqlReader.Item("QSTN_Durata"))
                oQuest.risultatiAnonimi = isNullBoolean(sqlReader.Item("QSTN_RisultatiAnonimi"))
                oQuest.forUtentiComunita = isNullBoolean(sqlReader.Item("QSTN_forUtentiComunita"))
                oQuest.forUtentiPortale = isNullBoolean(sqlReader.Item("QSTN_forUtentiPortale"))
                oQuest.forUtentiInvitati = isNullBoolean(sqlReader.Item("QSTN_forUtentiInvitati"))
                oQuest.forUtentiEsterni = isNullBoolean(sqlReader.Item("QSTN_forUtentiEsterni"))
                oQuest.visualizzaRisposta = isNullBoolean(sqlReader.Item("QSTN_visualizzaRisposta"))
                oQuest.visualizzaCorrezione = isNullBoolean(sqlReader.Item("QSTN_visualizzaCorrezione"))
                oQuest.visualizzaSuggerimenti = isNullBoolean(sqlReader.Item("QSTN_visualizzaSuggerimenti"))
                oQuest.editaRisposta = isNullBoolean(sqlReader.Item("QSTN_editaRisposta"))
                oQuest.tipoGrafico = isNullInt(sqlReader.Item("QSTN_TPGF_Id"))
                oQuest.tipo = isNullInt(sqlReader.Item("QSTN_Tipo"))
                oQuest.creator = isNullString(sqlReader.Item("creator"))
                oQuest.dataModifica = isNullString(sqlReader.Item("QSTN_DataModifica"))
                oQuest.idPersonaEditor = isNullInt(sqlReader.Item("QSTN_PRSN_Editor_Id"))
                oQuest.isRandomOrder = isNullBoolean(sqlReader.Item("QSTN_isRandomOrder"))
                oQuest.editor = isNullString(sqlReader.Item("editor"))
                oQuest.nDomandeDiffBassa = isNullInt(sqlReader.Item("QSTN_nDomandeDiffBassa"))
                oQuest.nDomandeDiffMedia = isNullInt(sqlReader.Item("QSTN_nDomandeDiffMedia"))
                oQuest.nDomandeDiffAlta = isNullInt(sqlReader.Item("QSTN_nDomandeDiffAlta"))
                oQuest.isRandomOrder_Options = isNullBoolean(sqlReader.Item("QSTN_isRandomOrder_Options"))
                oQuest.nQuestionsPerPage = isNullInt(sqlReader.Item("QSTN_nQuestionsPerPage"))
                oQuest.isPassword = isNullBoolean(sqlReader.Item("QSTN_isPassword"))
                oQuest.ownerType = isNullInt(sqlReader.Item("QSTN_ownerType"))
                oQuest.ownerId = isNullBigInt(sqlReader.Item("QSTN_ownerId"))
                oQuest.ownerGUID = isNullGuid(sqlReader.Item("QSTN_ownerGUID"))

                Dim lQuest As LazyQuestionnaire = GetQuestionnaire(appContext, idQuestionario)
                If Not IsNothing(lQuest) Then
                    If oQuest.tipo = QuestionnaireType.RandomMultipleAttempts Then
                        'Dim lQuest As LazyQuestionnaire = GetQuestionnaire(appContext, idQuestionario)
                        If Not IsNothing(lQuest) Then
                            oQuest.MinScore = lQuest.MinScore
                            oQuest.MaxAttempts = lQuest.MaxAttempts
                        End If
                    End If
                    oQuest.DisplayScoreToUser = lQuest.DisplayScoreToUser
                    oQuest.DisplayAttemptScoreToUser = lQuest.DisplayAttemptScoreToUser
                    oQuest.DisplayAvailableAttempts = lQuest.DisplayAvailableAttempts
                    oQuest.DisplayResultsStatus = lQuest.DisplayResultsStatus
                    oQuest.DisplayCurrentAttempts = lQuest.DisplayCurrentAttempts
                End If
            End While
            sqlReader.Close()
        End Using
        Return oQuest
    End Function

    'Public Shared Function readCampiQuestionario(ByVal idQuestionario As Integer, ByVal idLingua As Integer) As Questionario
    '    Dim db As Database = DatabaseFactory.CreateDatabase()
    '    Dim connection As DbConnection = db.CreateConnection()

    '    Return readCampiQuestionario(idQuestionario, idLingua, db, connection)
    'End Function
    Public Shared Sub readDomandeOpzioniRisposte(ByVal _oQuestionario As Questionario)

        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim connection As DbConnection = db.CreateConnection()

        readDomandeOpzioniRisposte(_oQuestionario, db, connection)

    End Sub
    Private Shared Function readDomandeOpzioniRisposte(ByVal _oQuestionario As Questionario, ByVal db As Database, ByVal connection As DbConnection) As Questionario
        For Each oPagina As QuestionarioPagina In _oQuestionario.pagine
            For Each oDomanda As Domanda In oPagina.domande
                'Dim nRisposteDomanda As Integer = 0
                If oDomanda.numero = 0 Then
                    oDomanda.numero = 0
                End If
                Select Case oDomanda.tipo
                    Case Domanda.TipoDomanda.Multipla
                        oDomanda.domandaMultiplaOpzioni = DALDomande.readDomandaMultiplaOpzioni(oDomanda, db, connection)
                        For Each orispostaQ As RispostaQuestionario In _oQuestionario.risposteQuestionario
                            If orispostaQ.id > 0 Then
                                For Each risp As RispostaDomanda In DALRisposte.readRispostaOpzioneMultipla(orispostaQ.id, oDomanda.id, db, connection)
                                    oDomanda.risposteDomanda.Add(risp)
                                    'orispostaQ.risposteDomande.Add(risp)
                                Next
                            End If
                        Next

                    Case Domanda.TipoDomanda.DropDown
                        oDomanda.domandaDropDown = DALDomande.readDomandaDropDownBYIDDomanda(oDomanda, db, connection)
                        For Each orispostaQ As RispostaQuestionario In _oQuestionario.risposteQuestionario
                            If orispostaQ.id > 0 Then
                                For Each risp As RispostaDomanda In DALRisposte.readRispostaDropDown(orispostaQ.id, oDomanda.id, db, connection)
                                    oDomanda.risposteDomanda.Add(risp)
                                    'orispostaQ.risposteDomande.Add(risp)
                                Next

                            End If
                        Next

                    Case Domanda.TipoDomanda.Rating
                        oDomanda.domandaRating = DALDomande.readDomandaRatingByID(oDomanda.idDomandaMultilingua, db, connection)

                        For Each orispostaQ As RispostaQuestionario In _oQuestionario.risposteQuestionario
                            If orispostaQ.id > 0 Then

                                For Each risp As RispostaDomanda In DALRisposte.readRispostaRating(orispostaQ.id, oDomanda.id, db, connection)
                                    oDomanda.risposteDomanda.Add(risp)
                                    'orispostaQ.risposteDomande.Add(risp)
                                Next

                            End If
                        Next

                    Case Domanda.TipoDomanda.RatingStars
                        oDomanda.domandaRating = DALDomande.readDomandaRatingByID(oDomanda.idDomandaMultilingua, db, connection)

                        For Each orispostaQ As RispostaQuestionario In _oQuestionario.risposteQuestionario
                            If orispostaQ.id > 0 Then

                                For Each risp As RispostaDomanda In DALRisposte.readRispostaRating(orispostaQ.id, oDomanda.id, db, connection)
                                    oDomanda.risposteDomanda.Add(risp)
                                    'orispostaQ.risposteDomande.Add(risp)
                                Next

                            End If
                        Next

                    Case Domanda.TipoDomanda.Meeting
                        oDomanda.domandaRating = DALDomande.readDomandaRatingByID(oDomanda.idDomandaMultilingua, db, connection)

                        For Each orispostaQ As RispostaQuestionario In _oQuestionario.risposteQuestionario
                            If orispostaQ.id > 0 Then

                                For Each risp As RispostaDomanda In DALRisposte.readRispostaRating(orispostaQ.id, oDomanda.id, db, connection)
                                    oDomanda.risposteDomanda.Add(risp)
                                    'orispostaQ.risposteDomande.Add(risp)
                                Next

                            End If
                        Next
                    Case Domanda.TipoDomanda.TestoLibero
                        oDomanda.opzioniTestoLibero = DALDomande.readDomandaTestoLiberoByID(oDomanda.idDomandaMultilingua, db, connection)
                        For Each orispostaQ As RispostaQuestionario In _oQuestionario.risposteQuestionario
                            If orispostaQ.id > 0 Then

                                For Each rispostaTestoLibero As RispostaDomanda In DALRisposte.readRispostaTestoLibero(orispostaQ.id, oDomanda.id, db, connection)
                                    If rispostaTestoLibero.id > 0 Then
                                        oDomanda.risposteDomanda.Add(rispostaTestoLibero)
                                        'orispostaQ.risposteDomande.Add(rispostaTestoLibero)
                                    End If
                                Next

                            End If
                        Next

                    Case Domanda.TipoDomanda.Numerica
                        oDomanda.opzioniNumerica = DALDomande.readDomandaNumericaById(oDomanda.idDomandaMultilingua, db, connection)
                        For Each orispostaQ As RispostaQuestionario In _oQuestionario.risposteQuestionario
                            If orispostaQ.id > 0 Then

                                For Each rispostaNumerica As RispostaDomanda In DALRisposte.readRispostaNumerica(orispostaQ.id, oDomanda.id, db, connection)
                                    If rispostaNumerica.id > 0 Then
                                        oDomanda.risposteDomanda.Add(rispostaNumerica)
                                        'orispostaQ.risposteDomande.Add(rispostaNumerica)
                                    End If
                                Next

                            End If
                        Next


                End Select

                oDomanda.numeroRisposteDomanda = 0

                For Each risQ As RispostaQuestionario In _oQuestionario.risposteQuestionario
                    For Each risD As RispostaDomanda In oDomanda.risposteDomanda
                        If risD.idRispostaQuestionario = risQ.id Then
                            oDomanda.numeroRisposteDomanda = oDomanda.numeroRisposteDomanda + 1
                            Exit For
                        End If
                    Next
                Next

                'oDomanda.numeroRisposteDomanda = oDomanda.risposteDomanda.Count
                _oQuestionario.domande.Add(oDomanda)

            Next
        Next

        Return _oQuestionario
    End Function
    Private Shared Function readDomandeOpzioniRisposta(ByVal _oQuestionario As Questionario, ByVal db As Database, ByVal connection As DbConnection) As Questionario

        For Each oPagina As QuestionarioPagina In _oQuestionario.pagine
            For Each oDomanda As Domanda In oPagina.domande
                'Dim nRisposteDomanda As Integer = 0
                Select Case oDomanda.tipo
                    Case Domanda.TipoDomanda.Multipla
                        oDomanda.domandaMultiplaOpzioni = DALDomande.readDomandaMultiplaOpzioni(oDomanda, db, connection)
                        If _oQuestionario.rispostaQuest.id > 0 Then
                            For Each risp As RispostaDomanda In DALRisposte.readRispostaOpzioneMultipla(_oQuestionario.rispostaQuest.id, oDomanda.id, db, connection)
                                oDomanda.risposteDomanda.Add(risp)
                                'orispostaQ.risposteDomande.Add(risp)
                            Next
                        End If

                    Case Domanda.TipoDomanda.DropDown
                        oDomanda.domandaDropDown = DALDomande.readDomandaDropDownBYIDDomanda(oDomanda, db, connection)
                        If _oQuestionario.rispostaQuest.id > 0 Then
                            For Each risp As RispostaDomanda In DALRisposte.readRispostaDropDown(_oQuestionario.rispostaQuest.id, oDomanda.id, db, connection)
                                oDomanda.risposteDomanda.Add(risp)
                                'orispostaQ.risposteDomande.Add(risp)
                            Next

                        End If

                    Case Domanda.TipoDomanda.Rating
                        oDomanda.domandaRating = DALDomande.readDomandaRatingByID(oDomanda.idDomandaMultilingua, db, connection)

                        If _oQuestionario.rispostaQuest.id > 0 Then

                            For Each risp As RispostaDomanda In DALRisposte.readRispostaRating(_oQuestionario.rispostaQuest.id, oDomanda.id, db, connection)
                                oDomanda.risposteDomanda.Add(risp)
                                'orispostaQ.risposteDomande.Add(risp)
                            Next

                        End If

                    Case Domanda.TipoDomanda.RatingStars
                        oDomanda.domandaRating = DALDomande.readDomandaRatingByID(oDomanda.idDomandaMultilingua, db, connection)

                        For Each orispostaQ As RispostaQuestionario In _oQuestionario.risposteQuestionario
                            If orispostaQ.id > 0 Then

                                For Each risp As RispostaDomanda In DALRisposte.readRispostaRating(orispostaQ.id, oDomanda.id, db, connection)
                                    oDomanda.risposteDomanda.Add(risp)
                                    'orispostaQ.risposteDomande.Add(risp)
                                Next

                            End If
                        Next

                    Case Domanda.TipoDomanda.Meeting
                        oDomanda.domandaRating = DALDomande.readDomandaRatingByID(oDomanda.idDomandaMultilingua, db, connection)

                        If _oQuestionario.rispostaQuest.id > 0 Then

                            For Each risp As RispostaDomanda In DALRisposte.readRispostaRating(_oQuestionario.rispostaQuest.id, oDomanda.id, db, connection)
                                oDomanda.risposteDomanda.Add(risp)
                                'orispostaQ.risposteDomande.Add(risp)
                            Next

                        End If
                    Case Domanda.TipoDomanda.TestoLibero
                        oDomanda.opzioniTestoLibero = DALDomande.readDomandaTestoLiberoByID(oDomanda.idDomandaMultilingua, db, connection)
                        If _oQuestionario.rispostaQuest.id > 0 Then

                            For Each rispostaTestoLibero As RispostaDomanda In DALRisposte.readRispostaTestoLibero(_oQuestionario.rispostaQuest.id, oDomanda.id, db, connection)
                                If rispostaTestoLibero.id > 0 Then
                                    oDomanda.risposteDomanda.Add(rispostaTestoLibero)
                                    'orispostaQ.risposteDomande.Add(rispostaTestoLibero)
                                End If
                            Next

                        End If

                    Case Domanda.TipoDomanda.Numerica
                        oDomanda.opzioniNumerica = DALDomande.readDomandaNumericaById(oDomanda.idDomandaMultilingua, db, connection)
                        If _oQuestionario.rispostaQuest.id > 0 Then

                            For Each rispostaNumerica As RispostaDomanda In DALRisposte.readRispostaNumerica(_oQuestionario.rispostaQuest.id, oDomanda.id, db, connection)
                                If rispostaNumerica.id > 0 Then
                                    oDomanda.risposteDomanda.Add(rispostaNumerica)
                                    'orispostaQ.risposteDomande.Add(rispostaNumerica)
                                End If
                            Next

                        End If


                End Select

                oDomanda.numeroRisposteDomanda = 0

                For Each risD As RispostaDomanda In oDomanda.risposteDomanda
                    If risD.idRispostaQuestionario = _oQuestionario.rispostaQuest.id Then
                        oDomanda.numeroRisposteDomanda = oDomanda.numeroRisposteDomanda + 1
                        Exit For
                    End If
                Next

                'oDomanda.numeroRisposteDomanda = oDomanda.risposteDomanda.Count
                _oQuestionario.domande.Add(oDomanda)

            Next
        Next

        Return _oQuestionario
    End Function
    Private Shared Function readDomandeOpzioniRisposteByPersona(ByVal _oQuestionario As Questionario, ByVal db As Database, ByVal connection As DbConnection) As Questionario

        For Each oPagina As QuestionarioPagina In _oQuestionario.pagine
            For Each oDomanda As Domanda In oPagina.domande
                Dim idDomanda As Integer = oDomanda.id
                Select Case oDomanda.tipo
                    Case Domanda.TipoDomanda.Multipla
                        oDomanda.domandaMultiplaOpzioni = DALDomande.readDomandaMultiplaOpzioni(oDomanda, db, connection)
                        If Not _oQuestionario.rispostaQuest Is Nothing Then
                            oDomanda.risposteDomanda = DALRisposte.readRispostaOpzioneMultipla(_oQuestionario.rispostaQuest.id, oDomanda.id, db, connection)
                        End If

                    Case Domanda.TipoDomanda.DropDown

                        oDomanda.domandaDropDown = DALDomande.readDomandaDropDownBYIDDomanda(oDomanda, db, connection)
                        If Not _oQuestionario.rispostaQuest Is Nothing Then
                            oDomanda.risposteDomanda = DALRisposte.readRispostaDropDown(_oQuestionario.rispostaQuest.id, oDomanda.id, db, connection)
                        End If


                    Case Domanda.TipoDomanda.Rating

                        oDomanda.domandaRating = DALDomande.readDomandaRatingByID(oDomanda.idDomandaMultilingua, db, connection)
                        If Not _oQuestionario.rispostaQuest Is Nothing Then
                            oDomanda.risposteDomanda = DALRisposte.readRispostaRating(_oQuestionario.rispostaQuest.id, oDomanda.id, db, connection)
                        End If

                    Case Domanda.TipoDomanda.RatingStars

                        oDomanda.domandaRating = DALDomande.readDomandaRatingByID(oDomanda.idDomandaMultilingua, db, connection)
                        If Not _oQuestionario.rispostaQuest Is Nothing Then
                            oDomanda.risposteDomanda = DALRisposte.readRispostaRatingStars(_oQuestionario.rispostaQuest.id, oDomanda.id, db, connection)
                        End If

                    Case Domanda.TipoDomanda.Meeting

                        oDomanda.domandaRating = DALDomande.readDomandaRatingByID(oDomanda.idDomandaMultilingua, db, connection)
                        If Not _oQuestionario.rispostaQuest Is Nothing Then
                            oDomanda.risposteDomanda = DALRisposte.readRispostaRating(_oQuestionario.rispostaQuest.id, oDomanda.id, db, connection)
                        End If
                    Case Domanda.TipoDomanda.TestoLibero
                        oDomanda.opzioniTestoLibero = DALDomande.readDomandaTestoLiberoByID(oDomanda.idDomandaMultilingua, db, connection)
                        If Not _oQuestionario.rispostaQuest Is Nothing Then
                            oDomanda.risposteDomanda = DALRisposte.readRispostaTestoLibero(_oQuestionario.rispostaQuest.id, oDomanda.id, db, connection)
                        End If

                    Case Domanda.TipoDomanda.Numerica

                        oDomanda.opzioniNumerica = DALDomande.readDomandaNumericaById(oDomanda.idDomandaMultilingua, db, connection)
                        If Not _oQuestionario.rispostaQuest Is Nothing Then
                            oDomanda.risposteDomanda = DALRisposte.readRispostaNumerica(_oQuestionario.rispostaQuest.id, oDomanda.id, db, connection)
                        End If

                End Select

                For Each rispDomanda As RispostaDomanda In oDomanda.risposteDomanda
                    If Not rispDomanda Is Nothing Then
                        _oQuestionario.rispostaQuest.risposteDomande.Add(rispDomanda)
                    End If
                Next

                oDomanda.numeroRisposteDomanda = oDomanda.risposteDomanda.Count

                _oQuestionario.domande.Add(oDomanda)

            Next
        Next

        Return _oQuestionario

    End Function
    Public Shared Function readQuestionarioFiglioByIDPadre(ByRef idQuestionarioPadre As Integer, ByRef idPersonaDestinatario As Integer, ByRef idUIDestinatario As Integer, ByRef db As Database, ByRef connection As DbConnection) As Integer
        Dim idQuestionarioFiglio As Integer

        Try
            If idUIDestinatario = 0 Then
                Dim sqlCommand As String = "sp_Questionario_ReadQuestionarioFiglioByIdPadreIdPersona"
                Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
                dbCommand.Connection = connection

                db.AddInParameter(dbCommand, "idQuestionarioPadre", DbType.Int32, idQuestionarioPadre)
                db.AddInParameter(dbCommand, "idPersonaDestinatario", DbType.Int32, idPersonaDestinatario)

                idQuestionarioFiglio = db.ExecuteScalar(dbCommand)
            Else
                Dim sqlCommand As String = "sp_Questionario_ReadQuestionarioFiglioByIdPadreIdUtente"
                Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
                dbCommand.Connection = connection

                db.AddInParameter(dbCommand, "idQuestionarioPadre", DbType.Int32, idQuestionarioPadre)
                db.AddInParameter(dbCommand, "idUtenteInvitatoDestinatario", DbType.Int32, idUIDestinatario)

                idQuestionarioFiglio = db.ExecuteScalar(dbCommand)
            End If

        Catch ex As Exception
            idQuestionarioFiglio = 0
        End Try

        Return idQuestionarioFiglio
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="isCompiling"></param>
    ''' <param name="idQuestionarioPadre"></param>
    ''' <param name="idLingua"></param>
    ''' <param name="idPersona">0 = none</param>
    ''' <param name="idUtenteInvitato">0 = none</param>
    ''' <param name="idRisposta"></param>
    ''' <param name="oQuestionario"></param>
    ''' <param name="idQuestionarioRandom"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function readQuestionarioByPersona(appContext As iApplicationContext, ByRef isCompiling As Boolean, ByRef idQuestionarioPadre As Integer, ByRef idLingua As Integer, ByRef idPersona As Integer, ByRef idUtenteInvitato As Integer, Optional ByVal idRisposta As Integer = Integer.MinValue, Optional ByRef oQuestionario As Questionario = Nothing, Optional ByRef idQuestionarioRandom As Integer = 0) As Questionario
        Dim oGestioneRisposte As New RispostaQuestionario
        If oQuestionario Is Nothing Then
            oQuestionario = New Questionario
        End If
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using connection As DbConnection = db.CreateConnection()
            connection.Open()
            If oQuestionario.idFiglio = 0 Then
                oQuestionario = readCampiQuestionario(appContext, idQuestionarioPadre, idLingua, db, connection)
                If Not idQuestionarioRandom = 0 Then
                    oQuestionario.idFiglio = idQuestionarioRandom
                End If
            End If

            If (oQuestionario.tipo = QuestionnaireType.Random OrElse (oQuestionario.tipo = Questionario.TipoQuestionario.Autovalutazione And Not isCompiling)) And oQuestionario.idFiglio = 0 Then
                oQuestionario.idFiglio = readQuestionarioFiglioByIDPadre(idQuestionarioPadre, idPersona, idUtenteInvitato, db, connection)
                If oQuestionario.idFiglio = 0 Then
                    oQuestionario.librerieQuestionario = DALQuestionario.readQuestionarioLibrerie(oQuestionario, db)
                Else
                    Return readQuestionarioByPersona(appContext, isCompiling, idQuestionarioPadre, idLingua, idPersona, idUtenteInvitato, idRisposta, oQuestionario)
                End If
            ElseIf (oQuestionario.tipo = QuestionnaireType.RandomMultipleAttempts) And oQuestionario.idFiglio = 0 Then
                If idQuestionarioRandom > 0 Then
                    oQuestionario.idFiglio = idQuestionarioRandom
                Else
                    Dim attempts As List(Of LazyUserResponse) = GetQuestionnaireAttempts(appContext, idQuestionarioPadre, idPersona, 0)
                    If Not IsNothing(attempts) AndAlso attempts.Count > 0 Then
                        Dim l As LazyUserResponse = attempts.OrderByDescending(Function(r) r.Id).FirstOrDefault()
                        Dim calc As dtoItemEvaluation(Of Long) = CalculateAttemptComplation(appContext, l)
                        If Not calc.isCompleted OrElse (calc.isCompleted AndAlso l.RelativeScore >= oQuestionario.MinScore) OrElse Not isCompiling Then
                            oQuestionario.idFiglio = l.IdRandomQuestionnaire
                        Else
                            oQuestionario.idFiglio = 0
                        End If
                    Else
                        oQuestionario.idFiglio = 0 ' readQuestionarioFiglioByIDPadre(idQuestionarioPadre, idPersona, idUtenteInvitato, db, connection)
                    End If
                End If

                If oQuestionario.idFiglio = 0 Then
                    oQuestionario.librerieQuestionario = DALQuestionario.readQuestionarioLibrerie(oQuestionario, db)
                Else
                    oQuestionario.risposteQuestionario = New List(Of RispostaQuestionario)
                    'For Each item As LazyQuestionnaireRandom In GetRandomChildren(appContext, oQuestionario.id, idPersona, idUtenteInvitato)

                    'Next
                    'oQuestionario.idFiglio = readQuestionarioFiglioByIDPadre(idQuestionarioPadre, idPersona, idUtenteInvitato, db, connection)
                    'If oQuestionario.idFiglio = 0 Then
                    '    oQuestionario.librerieQuestionario = DALQuestionario.readQuestionarioLibrerie(oQuestionario, db)
                    'Else
                    ' Return readQuestionarioByPersona(appContext, isCompiling, idQuestionarioPadre, idLingua, idPersona, idUtenteInvitato, idRisposta, oQuestionario, oQuestionario.idFiglio)
                    'End If
                End If

            ElseIf oQuestionario.tipo = Questionario.TipoQuestionario.Autovalutazione Then
                oQuestionario.librerieQuestionario = DALQuestionario.readQuestionarioLibrerie(oQuestionario, db)
            End If
            Try
                If idRisposta = Integer.MinValue Then
                    'se viene passato l'idRisposta la risposta viene letta sempre secondo quello
                    If (oQuestionario.tipo <> QuestionnaireType.AutoEvaluation AndAlso oQuestionario.tipo <> QuestionnaireType.RandomMultipleAttempts) OrElse (oQuestionario.tipo = QuestionnaireType.RandomMultipleAttempts AndAlso oQuestionario.idFiglio > 0) Then
                        oQuestionario.rispostaQuest = DALRisposte.readRispostaByIDPersona(idQuestionarioPadre, idPersona, idUtenteInvitato, db, connection, oQuestionario.idFiglio)
                    End If
                ElseIf (oQuestionario.tipo = QuestionnaireType.RandomMultipleAttempts) Then
                    oQuestionario.rispostaQuest = New RispostaQuestionario
                    If idRisposta > 0 Then
                        oQuestionario.rispostaQuest = DALRisposte.readRispostaByIDPersona(idQuestionarioPadre, idPersona, idUtenteInvitato, db, connection, oQuestionario.idFiglio)
                    End If
                Else
                    oQuestionario.rispostaQuest = DALRisposte.readRispostaBYIDRispostaQuestionario(idRisposta, db, connection)
                    oQuestionario.idFiglio = oQuestionario.rispostaQuest.idQuestionarioRandom
                End If
                If Not isCompiling OrElse (Not oQuestionario.isBloccato AndAlso (oQuestionario.visualizzaRisposta OrElse oQuestionario.rispostaQuest.id = 0 OrElse Not oQuestionario.rispostaQuest.dataFine Is Nothing)) Then
                    'se sto compilando il questionario e questo e' bloccato, oppure se l'utente non ha il permesso 
                    'di visualizzare/modificare le risposte non devo caricare niente
                    If oQuestionario.idFiglio = 0 AndAlso oQuestionario.tipo <> QuestionnaireType.RandomMultipleAttempts Then
                        DALPagine.readPagineByIDQuestionario(oQuestionario, db, connection)
                        DALDomande.readDomandeByPagina(oQuestionario, db, connection)
                    ElseIf oQuestionario.idFiglio > 0 Then
                        DALDomande.readDomandeByQuestionario(oQuestionario, db, connection)
                    End If

                    If oQuestionario.rispostaQuest.id < 1 Then
                        oQuestionario.rispostaQuest.id = idRisposta
                    End If
                    oQuestionario = readDomandeOpzioniRisposteByPersona(oQuestionario, db, connection)
                    'readDomandeOpzioniRisposta(oQuestionario, db, connection)
                End If

            Catch ex As Exception
                Dim a As String = ex.Message
            End Try
            connection.Close()
        End Using
        If (oQuestionario.tipo = QuestionnaireType.Random OrElse oQuestionario.tipo = QuestionnaireType.RandomMultipleAttempts) AndAlso Not oQuestionario.idFiglio = 0 Then
            oQuestionario.pagine.Clear()
            Dim indexPage As Integer
            For indexPage = 0 To Math.Floor((oQuestionario.domande.Count - 1) / oQuestionario.nQuestionsPerPage)
                Dim oPageSurvey As New QuestionarioPagina
                Dim indexQuestion As Integer

                oPageSurvey.dallaDomanda = indexPage * oQuestionario.nQuestionsPerPage + 1
                oPageSurvey.allaDomanda = Math.Min((indexPage + 1) * oQuestionario.nQuestionsPerPage, oQuestionario.domande.Count)
                oPageSurvey.idQuestionarioMultilingua = oQuestionario.idQuestionarioMultilingua
                oPageSurvey.numeroDomande = oQuestionario.nQuestionsPerPage
                oPageSurvey.numeroPagina = indexPage + 1
                Dim remainingQuestions As Int16
                remainingQuestions = Math.Min(oQuestionario.nQuestionsPerPage - 1, oQuestionario.domande.Count - (indexPage * oQuestionario.nQuestionsPerPage + 1))
                For indexQuestion = 0 To remainingQuestions
                    oPageSurvey.domande.Add(oQuestionario.domande(indexQuestion + indexPage * oQuestionario.nQuestionsPerPage))
                Next
                oQuestionario.pagine.Add(oPageSurvey)
            Next
        End If
        If oQuestionario.isRandomOrder Then
            Dim newCollectionOfPages As New List(Of QuestionarioPagina)
            Dim index As Integer = 0
            For Each pageSurvey As QuestionarioPagina In oQuestionario.pagine
                Dim newPageQuestions As List(Of Domanda)
                newPageQuestions = DALDomande.randomizeCollectionOfQuestions(pageSurvey.domande, pageSurvey.dallaDomanda)
                pageSurvey.domande = newPageQuestions
                index += 1
            Next
        End If
        Return oQuestionario

    End Function
    Public Shared Function readQuestionarioStatisticheByPersona(appContext As iApplicationContext, ByVal idQuestionario As Integer, ByVal tipo As Integer, ByVal idLingua As Integer, ByVal idRisposta As Integer) As Questionario
        Dim _oQuestionario As New Questionario
        Dim oGestioneRisposte As New RispostaQuestionario

        Dim db As Database = DatabaseFactory.CreateDatabase()

        Using connection As DbConnection = db.CreateConnection()
            connection.Open()

            _oQuestionario = readCampiQuestionario(appContext, idQuestionario, idLingua, db, connection)

            _oQuestionario.rispostaQuest = DALRisposte.readRispostaBYIDRispostaQuestionario(idRisposta, db, connection)

            If Not _oQuestionario.rispostaQuest.id = 0 Then
                Try
                    If tipo = Questionario.TipoQuestionario.Random Then
                        idQuestionario = DALRisposte.readIDQuestionarioRandomBYIDRisposta(idRisposta)
                        _oQuestionario.idFiglio = idQuestionario
                        DALDomande.readDomandeByQuestionario(_oQuestionario, db, connection)
                    Else
                        DALPagine.readPagineByIDQuestionario(_oQuestionario, db, connection)
                        DALDomande.readDomandeByPagina(_oQuestionario, db, connection)
                    End If

                    ''se sto compilando il questionario e questo e' bloccato, oppure se l'utente non ha il permesso 
                    ''di visualizzare/modificare le risposte non devo caricare niente

                    'DALPagine.readPagineByIDQuestionario(_oQuestionario, db, connection)
                    'DALDomande.readDomandeByPagina(_oQuestionario, db, connection)
                    readDomandeOpzioniRisposteByPersona(_oQuestionario, db, connection)

                Catch ex As Exception
                    Dim a As String = ex.Message
                End Try
                connection.Close()
            End If
        End Using

        Return _oQuestionario

    End Function
    Public Shared Function readQuestionariAutovalutazioneByIdPersona(ByRef idGruppo As Integer, ByRef idUtenteCorrente As Integer) As List(Of Questionario)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim oQuestionariList As New List(Of Questionario)
        Dim dbCommand As DbCommand
        Using connection As DbConnection = db.CreateConnection()
            connection.Open()
            Dim sqlCommand As String
            sqlCommand = "sp_Questionario_QuestionariAutovalutazioneByPersona_Select"
            dbCommand = db.GetStoredProcCommand(sqlCommand)
            dbCommand.Connection = connection
            db.AddInParameter(dbCommand, "idPersona", DbType.Int32, idUtenteCorrente)
            db.AddInParameter(dbCommand, "idgruppo", DbType.Int32, idGruppo)
            Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                While sqlReader.Read()
                    Dim oQuest As New Questionario
                    oQuest.id = isNullInt(sqlReader.Item("QSTN_Id"))
                    oQuest.nome = isNullString(sqlReader.Item("QSML_Nome"))
                    oQuest.nRisposte = Integer.Parse(sqlReader.Item("quanteRisposte"))
                    oQuestionariList.Add(oQuest)
                End While
                sqlReader.Close()
            End Using
            connection.Close()
        End Using
        Return oQuestionariList
    End Function


    Public Shared Function GetQuestionnaireAnswers(appContext As iApplicationContext, ByVal idAnswer As Long, ByVal idQuestionnaire As Long, ByVal idPerson As Integer, ByVal idInvitedUser As Long?, ByVal idRandom As Long, Optional ByVal loadAssociatedQuestions As Boolean = False) As QuestionnaireAnswer
        Dim result As New QuestionnaireAnswer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using connection As DbConnection = db.CreateConnection()
            connection.Open()

            result.Id = idAnswer
            result.IdQuestionnaire = idQuestionnaire
            result.IdUser = idPerson
            If idInvitedUser.HasValue Then
                result.IdInvitedUser = idInvitedUser.Value
            Else
                result.IdInvitedUser = 0
            End If

            result.IdRandomQuestionnaire = idRandom

            If loadAssociatedQuestions Then
                Dim s As New ServiceQuestionnaire(appContext)
                result.Questions = s.GetLazyAssociatedQuestion(idQuestionnaire, idRandom)
            End If
            Dim sqlCommand As String = "sp_Questionario_RispostaOpzioneMultipla_Select"
            Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            Try
                dbCommand.Connection = connection
                db.AddInParameter(dbCommand, "idDomanda", DbType.Int32, -1)
                db.AddInParameter(dbCommand, "idRispostaQuestionario", DbType.Int32, idAnswer)

                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim answer As New QuestionAnswer
                        answer.Id = isNullBigInt(sqlReader.Item("RSOM_Id"))
                        answer.IdQuestionOption = isNullBigInt(sqlReader.Item("RSOM_DMMO_Id"))
                        answer.OptionText = isNullString(sqlReader.Item("RSOM_TestoIsAltro"))
                        answer.OptionNumber = isNullInt(sqlReader.Item("DMMO_NumeroOpzione"))
                        answer.IdQuestion = isNullBigInt(sqlReader.Item("DMML_DMND_Id"))
                        answer.QuestionType = Domanda.TipoDomanda.Multipla
                        result.Answers.Add(answer)
                    End While
                    sqlReader.Close()
                End Using
            Catch ex As Exception

            End Try



            sqlCommand = "sp_Questionario_RispostaDropDown_Select"
            dbCommand = db.GetStoredProcCommand(sqlCommand)
            dbCommand.Connection = connection
            db.AddInParameter(dbCommand, "idRispostaQuestionario", DbType.Int32, idAnswer)
            Try
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim answer As New QuestionAnswer
                        answer.Id = isNullBigInt(sqlReader.Item("RSDR_Id"))
                        answer.IdQuestion = isNullBigInt(sqlReader.Item("DMML_DMND_Id"))
                        answer.IdQuestionOption = isNullBigInt(sqlReader.Item("RSDR_DMDR_Id"))
                        answer.OptionText = isNullString(sqlReader.Item("RSDR_Testo"))
                        answer.OptionNumber = isNullInt(sqlReader.Item("RSDR_NumeroOpzione"))
                        answer.QuestionType = Domanda.TipoDomanda.DropDown
                        result.Answers.Add(answer)
                    End While
                    sqlReader.Close()
                End Using

            Catch ex As Exception

            End Try


            sqlCommand = "sp_Questionario_RispostaNumerica_Select"
            dbCommand = db.GetStoredProcCommand(sqlCommand)
            dbCommand.Connection = connection
            db.AddInParameter(dbCommand, "idRispostaQuestionario", DbType.Int32, idAnswer)
            Try
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim answer As New QuestionAnswer
                        answer.Id = isNullBigInt(sqlReader.Item("RSNM_Id"))
                        answer.IdQuestion = isNullBigInt(sqlReader.Item("DMML_DMND_Id"))
                        answer.IdQuestionOption = isNullBigInt(sqlReader.Item("RSNM_DMNM_Id"))
                        answer.OptionText = isNullString(sqlReader.Item("DMNM_TestoPrima"))
                        answer.OptionNumber = isNullInt(sqlReader.Item("DMNM_Numero"))
                        answer.Value = isNullDouble(sqlReader.Item("RSNM_Numero"))
                        answer.QuestionType = Domanda.TipoDomanda.Numerica
                        result.Answers.Add(answer)
                    End While
                    sqlReader.Close()
                End Using

            Catch ex As Exception

            End Try


            sqlCommand = "sp_Questionario_RispostaTestoLibero_Select"
            dbCommand = db.GetStoredProcCommand(sqlCommand)
            dbCommand.Connection = connection
            db.AddInParameter(dbCommand, "idRispostaQuestionario", DbType.Int32, idAnswer)
            Try
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim answer As New QuestionAnswer
                        answer.Id = isNullBigInt(sqlReader.Item("RSTL_Id"))
                        answer.IdQuestion = isNullBigInt(sqlReader.Item("DMML_DMND_Id"))
                        answer.IdQuestionOption = isNullBigInt(sqlReader.Item("RSTL_DMTL_Id"))
                        answer.OptionText = isNullString(sqlReader.Item("RSTL_Testo"))
                        answer.OptionNumber = isNullInt(sqlReader.Item("DMTL_Numero"))
                        answer.Evaluation = isNullString(sqlReader.Item("RSTL_Valutazione"))
                        answer.QuestionType = Domanda.TipoDomanda.TestoLibero
                        result.Answers.Add(answer)
                    End While
                    sqlReader.Close()
                End Using
            Catch ex As Exception

            End Try


            sqlCommand = "sp_Questionario_RispostaRating_Select"
            dbCommand = db.GetStoredProcCommand(sqlCommand)
            dbCommand.Connection = connection
            db.AddInParameter(dbCommand, "idRispostaQuestionario", DbType.Int32, idAnswer)
            Try
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim answer As New QuestionAnswer
                        answer.Id = isNullBigInt(sqlReader.Item("RSRT_Id"))
                        answer.IdQuestion = isNullBigInt(sqlReader.Item("DMML_DMND_Id"))
                        answer.IdQuestionOption = isNullBigInt(sqlReader.Item("RSRT_DMRO_Id"))

                        answer.Value = isNullInt(sqlReader.Item("RSRT_Valore"))
                        answer.OptionNumber = isNullInt(sqlReader.Item("DMRO_NumeroOpzione"))
                        answer.OptionText = isNullString(sqlReader.Item("RSRT_TestoIsAltro"))
                        answer.QuestionType = Domanda.TipoDomanda.Rating
                        result.Answers.Add(answer)
                    End While
                    sqlReader.Close()
                End Using
            Catch ex As Exception

            End Try



            connection.Close()
        End Using
        Return result
    End Function


    Public Shared Function readQuestionariByGruppo(ByVal idGruppo As Integer, ByVal tipo As Integer) As QuestionarioGruppo

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim _gruppo As New QuestionarioGruppo
        Dim dbCommand2 As DbCommand

        Using connection As DbConnection = db.CreateConnection()
            connection.Open()

            _gruppo = DALQuestionarioGruppo.readGruppoBYID(idGruppo)

            Dim sqlCommand2 As String

            Select Case tipo
                Case Questionario.TipoQuestionario.LibreriaDiDomande
                    sqlCommand2 = "sp_Questionario_LibrerieByIDGruppo_Select"
                Case Questionario.TipoQuestionario.Modello
                    sqlCommand2 = "sp_Questionario_ModelliComunitaByIDGruppo_Select"
                Case Questionario.TipoQuestionario.Sondaggio
                    sqlCommand2 = "sp_Questionario_SondaggiByIDGruppo_Select"
                Case Questionario.TipoQuestionario.Meeting
                    sqlCommand2 = "sp_Questionario_MeetingByIDGruppo_Select"
                Case Else
                    sqlCommand2 = "sp_Questionario_QuestionariByIDGruppo_Select"
            End Select
            dbCommand2 = db.GetStoredProcCommand(sqlCommand2)
            dbCommand2.Connection = connection
            db.AddInParameter(dbCommand2, "IdGruppo", DbType.Int32, idGruppo)
            Using sqlReader2 As IDataReader = db.ExecuteReader(dbCommand2)
                While sqlReader2.Read()
                    Dim _quest As New Questionario
                    _quest.id = isNullInt(sqlReader2.Item("QSTN_Id"))
                    _quest.nome = isNullString(sqlReader2.Item("QSML_Nome"))
                    _quest.descrizione = isNullString(sqlReader2.Item("QSML_Descrizione"))
                    _quest.idGruppo = isNullInt(sqlReader2.Item("QSTN_QSGR_Id"))
                    _quest.dataInizio = isNullDateMin(sqlReader2.Item("QSTN_DataInizio"))
                    _quest.dataFine = isNullDateMax(sqlReader2.Item("QSTN_DataFine"))
                    _quest.isReadOnly = isNullBoolean(sqlReader2.Item("QSTN_IsChiuso"))
                    _quest.isBloccato = isNullBoolean(sqlReader2.Item("QSML_IsBloccato"))
                    _quest.idLingua = isNullInt(sqlReader2.Item("QSML_IdLingua"))
                    _quest.tipo = isNullInt(sqlReader2.Item("QSTN_Tipo"))
                    _quest.forUtentiInvitati = isNullBoolean(sqlReader2.Item("QSTN_forUtentiInvitati"))
                    _quest.forUtentiComunita = isNullBoolean(sqlReader2.Item("QSTN_forUtentiComunita"))
                    _quest.risultatiAnonimi = isNullBoolean(sqlReader2.Item("QSTN_RisultatiAnonimi"))
                    _gruppo.questionari.Add(_quest)
                End While
                sqlReader2.Close()
            End Using

            connection.Close()
        End Using

        Return _gruppo

    End Function
    Public Shared Function readModelliPubblici() As List(Of Questionario)

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim _modelli As New List(Of Questionario)

        Using connection As DbConnection = db.CreateConnection()
            connection.Open()

            Dim sqlCommand2 As String = "sp_Questionario_ModelliPubblici_Select"
            Dim dbCommand2 As DbCommand = db.GetStoredProcCommand(sqlCommand2)
            dbCommand2.Connection = connection

            Using sqlReader2 As IDataReader = db.ExecuteReader(dbCommand2)
                While sqlReader2.Read()
                    Dim _quest As New Questionario
                    _quest.id = isNullInt(sqlReader2.Item("QSTN_Id"))
                    _quest.nome = isNullString(sqlReader2.Item("QSML_Nome"))
                    _quest.descrizione = isNullString(sqlReader2.Item("QSML_Descrizione"))
                    _quest.dataInizio = isNullDateMin(sqlReader2.Item("QSTN_DataInizio"))
                    _quest.dataFine = isNullDateMax(sqlReader2.Item("QSTN_DataFine"))
                    _quest.idLingua = isNullInt(sqlReader2.Item("QSML_IdLingua"))

                    _modelli.Add(_quest)
                End While
                sqlReader2.Close()
            End Using

            connection.Close()
        End Using

        Return _modelli

    End Function
    Public Shared Function QuestionariMultilingua_Count(ByRef idQuestionario) As Integer
        Dim nMultilingua As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()

        Using connection As DbConnection = db.CreateConnection()
            connection.Open()
            Dim sqlCommand As String = "sp_Questionario_QuestionarioMultilingua_Count"
            Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            dbCommand.Connection = connection

            db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuestionario)

            nMultilingua = db.ExecuteScalar(dbCommand)

            connection.Close()

        End Using

        Return nMultilingua
    End Function
    Public Shared Function readQuestionariCancellatiByComunita(ByVal idComunita As Integer, ByVal tipo As Integer) As List(Of Questionario)

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim _gruppo As New QuestionarioGruppo
        Dim sqlCommand2 As String
        Dim dbCommand2 As DbCommand

        Using connection As DbConnection = db.CreateConnection()
            connection.Open()

            Select Case tipo
                Case Questionario.TipoQuestionario.Questionario
                    sqlCommand2 = "sp_Questionario_QuestionariCancellati_Select"
                    dbCommand2 = db.GetStoredProcCommand(sqlCommand2)
                    dbCommand2.Connection = connection
                    db.AddInParameter(dbCommand2, "IdComunita", DbType.Int32, idComunita)
                Case Else
                    sqlCommand2 = "sp_Questionario_QuestionariCancellatiByTipo_Select"
                    dbCommand2 = db.GetStoredProcCommand(sqlCommand2)
                    dbCommand2.Connection = connection
                    db.AddInParameter(dbCommand2, "IdComunita", DbType.Int32, idComunita)
                    db.AddInParameter(dbCommand2, "tipo", DbType.Int32, tipo)
            End Select

            Using sqlReader2 As IDataReader = db.ExecuteReader(dbCommand2)
                While sqlReader2.Read()
                    Dim _quest As New Questionario
                    _quest.id = isNullInt(sqlReader2.Item("QSTN_Id"))
                    _quest.nome = isNullString(sqlReader2.Item("QSML_Nome"))
                    _quest.descrizione = isNullString(sqlReader2.Item("QSML_Descrizione"))
                    _quest.idGruppo = isNullInt(sqlReader2.Item("QSTN_QSGR_Id"))
                    _quest.dataInizio = isNullDateMin(sqlReader2.Item("QSTN_DataInizio"))
                    _quest.dataFine = isNullDateMax(sqlReader2.Item("QSTN_DataFine"))
                    _quest.isReadOnly = isNullBoolean(sqlReader2.Item("QSTN_IsChiuso"))
                    _quest.isBloccato = isNullBoolean(sqlReader2.Item("QSML_IsBloccato"))
                    _quest.idQuestionarioMultilingua = isNullInt(sqlReader2.Item("QSML_Id"))
                    _quest.idLingua = isNullInt(sqlReader2.Item("QSML_IdLingua"))
                    _gruppo.questionari.Add(_quest)
                End While
                sqlReader2.Close()
            End Using

            connection.Close()
        End Using

        Return _gruppo.questionari

    End Function
    Public Shared Function readQuestionariPersonaByComunita(ByVal idPersona As Integer, ByVal idComunita As Integer, ByVal idLingua As Integer, ByVal tipo As Integer) As List(Of Questionario)

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim _listaQuestionari As New List(Of Questionario)
        Dim _listID As New List(Of Integer)
        Dim sqlCommand As String
        Dim dbCommand As DbCommand

        Using connection As DbConnection = db.CreateConnection()
            connection.Open()

            Select Case tipo
                'si ha il gruppoId ma non l'IdComunita': se si mette in join la comunita', i pubblici non sono visualizzabili (Com0 non esiste)
                Case Questionario.TipoQuestionario.Sondaggio
                    sqlCommand = "sp_Questionario_SondaggiComunitaByPersona_Select"
                Case Questionario.TipoQuestionario.Autovalutazione
                    sqlCommand = "sp_Questionario_AutovalutazioneComunitaByPersona_Select"
                Case Questionario.TipoQuestionario.Meeting
                    sqlCommand = "sp_Questionario_MeetingComunitaByPersona_Select"
                Case Else
                    sqlCommand = "sp_Questionario_QuestionariComunitaByPersona_Select"
            End Select
            dbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddInParameter(dbCommand, "idComunita", DbType.Int32, idComunita)
            dbCommand.Connection = connection

            db.AddInParameter(dbCommand, "idPersona", DbType.Int32, idPersona)
            Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                While sqlReader.Read()
                    Dim _quest As New Questionario
                    _quest.id = isNullInt(sqlReader.Item("QSTN_Id"))
                    _quest.nome = isNullString(sqlReader.Item("QSML_Nome"))
                    _quest.descrizione = isNullString(sqlReader.Item("QSML_Descrizione"))
                    _quest.dataInizio = isNullDateMin(sqlReader.Item("QSTN_DataInizio"))
                    _quest.dataFine = isNullDateMax(sqlReader.Item("QSTN_DataFine"))
                    _quest.creator = isNullString(sqlReader.Item("autore"))
                    _quest.idLingua = isNullInt(sqlReader.Item("QSML_IdLingua"))
                    _quest.isDefault = isNullBoolean(sqlReader.Item("QSML_IsDefault"))
                    _quest.isBloccato = isNullBoolean(sqlReader.Item("QSML_IsBloccato"))
                    Dim _risp As New RispostaQuestionario
                    If Not tipo = Questionario.TipoQuestionario.Autovalutazione Then
                        _risp.id = isNullInt(sqlReader.Item("IdRisposta"))
                        _risp.dataFine = isNullDateMax(sqlReader.Item("DataFine"))
                    Else
                        _quest.nRisposte = isNullInt(sqlReader.Item("quanteRisposte"))
                    End If
                    _quest.rispostaQuest = _risp
                    'If _risp.id > 0 And _risp.dataFine = Date.MinValue Then
                    _quest.stato = Questionario.StatoQuestionario.NonCompilato
                    'ElseIf _risp.id > 0 And Not _risp.dataFine = Date.MinValue Then
                    '    _quest.stato = Questionario.StatoQuestionario.Compilato
                    'Else
                    '    _quest.stato = Questionario.StatoQuestionario.NonCompilato
                    'End If
                    _quest.linguePresenti = DALQuestionario.readLingueQuestionario(_quest.id)
                    'If _quest.isDefault Then

                    _quest.visualizzaRisposta = isNullBoolean(sqlReader.Item("QSTN_visualizzaRisposta"))
                    _quest.editaRisposta = isNullBoolean(sqlReader.Item("QSTN_editaRisposta"))
                    _quest.risultatiAnonimi = isNullBoolean(sqlReader.Item("QSTN_RisultatiAnonimi"))
                    _quest.durata = isNullInt(sqlReader.Item("QSTN_Durata"))
                    _listaQuestionari.Add(_quest)
                    'End If
                End While
                sqlReader.Close()
            End Using
            connection.Close()
        End Using
        'Dim listaQuestionariAltreLingue As New list(Of Questionario)
        Dim listaQuestionariUtente As New List(Of Questionario)
        'Dim listID As New ArrayList
        For Each oquest As Questionario In _listaQuestionari
            If oquest.idLingua = idLingua Then
                'listID.Add(oquest.id)
                listaQuestionariUtente.Add(oquest)
            End If
        Next
        For Each oquestUt As Questionario In listaQuestionariUtente
            Questionario.removeQuestionariBYID(_listaQuestionari, oquestUt.id)
        Next
        For Each oquestDef As Questionario In _listaQuestionari
            If oquestDef.isDefault = True Then
                listaQuestionariUtente.Add(oquestDef)
            End If
        Next
        Return listaQuestionariUtente
    End Function
    Public Shared Function readQuestionariCompilatiByPersona(ByVal idPersona As Integer, ByVal idComunita As Integer, ByVal idLingua As Integer, ByVal tipo As Integer) As List(Of Questionario)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim _listaQuestionari As New List(Of Questionario)
        Dim _listID As New List(Of Integer)
        Dim sqlCommand As String
        Dim dbCommand As DbCommand
        Using connection As DbConnection = db.CreateConnection()
            connection.Open()
            If Not idComunita = 0 Then
                ' nella comunità leggo i questionari di comunità compilati
                Select Case tipo
                    Case Questionario.TipoQuestionario.Sondaggio
                        sqlCommand = "sp_Questionario_SondaggiCompilatiByPersona_Select"
                    Case Questionario.TipoQuestionario.Meeting
                        sqlCommand = "sp_Questionario_MeetingCompilatiByPersona_Select"
                    Case Else
                        sqlCommand = "sp_Questionario_QuestionariCompilatiByPersona_Select"
                End Select
                dbCommand = db.GetStoredProcCommand(sqlCommand)
                dbCommand.Connection = connection
                db.AddInParameter(dbCommand, "idComunita", DbType.Int32, idComunita)
                db.AddInParameter(dbCommand, "idPersona", DbType.Int32, idPersona)
            Else
                ' nella comunità 0 leggo i questionari del portale e quelli per utenti esterni
                Select Case tipo
                    Case Questionario.TipoQuestionario.Sondaggio
                        sqlCommand = "sp_Questionario_SondaggiPubbliciCompilatiByPersona_Select"
                    Case Questionario.TipoQuestionario.Meeting
                        sqlCommand = "sp_Questionario_MeetingPubbliciCompilatiByPersona_Select"
                    Case Else
                        sqlCommand = "sp_Questionario_QuestionariPubbliciCompilatiByPersona_Select"
                End Select
                dbCommand = db.GetStoredProcCommand(sqlCommand)
                dbCommand.Connection = connection
                db.AddInParameter(dbCommand, "idPersona", DbType.Int32, idPersona)
            End If

            Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                While sqlReader.Read()
                    Dim _quest As New Questionario
                    _quest.id = isNullInt(sqlReader.Item("QSTN_Id"))
                    _quest.nome = isNullString(sqlReader.Item("QSML_Nome"))
                    _quest.descrizione = isNullString(sqlReader.Item("QSML_Descrizione"))
                    _quest.dataInizio = isNullDateMin(sqlReader.Item("QSTN_DataInizio"))
                    _quest.dataFine = isNullDateMax(sqlReader.Item("QSTN_DataFine"))
                    _quest.creator = isNullString(sqlReader.Item("autore"))
                    _quest.idLingua = isNullInt(sqlReader.Item("QSML_IdLingua"))
                    _quest.isDefault = isNullBoolean(sqlReader.Item("QSML_IsDefault"))
                    _quest.isBloccato = isNullBoolean(sqlReader.Item("QSML_IsBloccato"))

                    Dim _risp As New RispostaQuestionario
                    _risp.id = isNullInt(sqlReader.Item("IdRisposta"))
                    _risp.dataFine = isNullDateMax(sqlReader.Item("DataFine"))
                    _risp.dataInizio = isNullDateMax(sqlReader.Item("DataInizio"))
                    _quest.rispostaQuest = _risp

                    _quest.stato = Questionario.StatoQuestionario.Compilato

                    _quest.linguePresenti = DALQuestionario.readLingueQuestionario(_quest.id)
                    _quest.visualizzaRisposta = isNullBoolean(sqlReader.Item("QSTN_visualizzaRisposta"))
                    _quest.editaRisposta = isNullBoolean(sqlReader.Item("QSTN_editaRisposta"))
                    _quest.risultatiAnonimi = isNullBoolean(sqlReader.Item("QSTN_RisultatiAnonimi"))
                    _quest.durata = isNullInt(sqlReader.Item("QSTN_Durata"))



                    _listaQuestionari.Add(_quest)

                End While
                sqlReader.Close()
            End Using
            connection.Close()
        End Using
        'rifare con LINQ
        Dim listaQuestionariUtente As New List(Of Questionario)
        For Each oquest As Questionario In _listaQuestionari
            If oquest.idLingua = idLingua Then
                listaQuestionariUtente.Add(oquest)
            End If
        Next
        For Each oquestUt As Questionario In listaQuestionariUtente
            Questionario.removeQuestionariBYID(_listaQuestionari, oquestUt.id)
        Next
        For Each oquestDef As Questionario In _listaQuestionari
            If oquestDef.isDefault = True Then
                listaQuestionariUtente.Add(oquestDef)
            End If
        Next
        Return listaQuestionariUtente
    End Function
    Public Shared Function readQuestionariCompilatiTuttiByPersona(ByVal idPersona As Integer, ByVal idLingua As Integer, ByVal tipo As Integer) As List(Of Questionario)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim _listaQuestionari As New List(Of Questionario)
        Dim _listID As New List(Of Integer)
        Dim sqlCommand As String
        Dim dbCommand As DbCommand
        Using connection As DbConnection = db.CreateConnection()
            connection.Open()
            Select Case tipo
                Case Questionario.TipoQuestionario.Sondaggio
                    sqlCommand = "sp_Questionario_SondaggiCompilatiTuttiByPersona_Select"
                Case Questionario.TipoQuestionario.Meeting
                    sqlCommand = "sp_Questionario_MeetingCompilatiTuttiByPersona_Select"
                Case Questionario.TipoQuestionario.Autovalutazione
                    sqlCommand = "sp_Questionario_AutovalutazioneCompilatiTuttiByPersona_Select"
                Case Else
                    sqlCommand = "sp_Questionario_QuestionariCompilatiTuttiByPersona_Select"
            End Select
            dbCommand = db.GetStoredProcCommand(sqlCommand)
            dbCommand.Connection = connection
            db.AddInParameter(dbCommand, "idPersona", DbType.Int32, idPersona)

            Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                While sqlReader.Read()
                    Dim _quest As New Questionario
                    _quest.id = isNullInt(sqlReader.Item("QSTN_Id"))
                    _quest.nome = isNullString(sqlReader.Item("QSML_Nome"))
                    _quest.descrizione = isNullString(sqlReader.Item("QSML_Descrizione"))
                    _quest.dataInizio = isNullDateMin(sqlReader.Item("QSTN_DataInizio"))
                    _quest.dataFine = isNullDateMax(sqlReader.Item("QSTN_DataFine"))
                    _quest.idLingua = isNullInt(sqlReader.Item("QSML_IdLingua"))
                    _quest.isDefault = isNullBoolean(sqlReader.Item("QSML_IsDefault"))
                    _quest.isBloccato = isNullBoolean(sqlReader.Item("QSML_IsBloccato"))
                    Dim _risp As New RispostaQuestionario
                    If Not tipo = Questionario.TipoQuestionario.Autovalutazione Then
                        _risp.id = isNullInt(sqlReader.Item("IdRisposta"))
                        _risp.dataFine = isNullDateMax(sqlReader.Item("DataFine"))
                        _risp.dataInizio = isNullDateMax(sqlReader.Item("DataInizio"))
                    Else
                        _quest.nRisposte = isNullInt(sqlReader.Item("quanteRisposte"))
                    End If

                    _quest.rispostaQuest = _risp

                    _quest.stato = Questionario.StatoQuestionario.Compilato

                    _quest.linguePresenti = DALQuestionario.readLingueQuestionario(_quest.id)
                    _quest.visualizzaRisposta = isNullBoolean(sqlReader.Item("QSTN_visualizzaRisposta"))
                    _quest.editaRisposta = isNullBoolean(sqlReader.Item("QSTN_editaRisposta"))
                    _quest.risultatiAnonimi = isNullBoolean(sqlReader.Item("QSTN_RisultatiAnonimi"))
                    _quest.durata = isNullInt(sqlReader.Item("QSTN_Durata"))
                    _listaQuestionari.Add(_quest)

                End While
                sqlReader.Close()
            End Using
            connection.Close()
        End Using

        'rifare con LINQ
        Dim listaQuestionariUtente As New List(Of Questionario)
        For Each oquest As Questionario In _listaQuestionari
            If oquest.idLingua = idLingua Then
                'listID.Add(oquest.id)
                listaQuestionariUtente.Add(oquest)
            End If
        Next
        For Each oquestUt As Questionario In listaQuestionariUtente
            Questionario.removeQuestionariBYID(_listaQuestionari, oquestUt.id)
        Next
        For Each oquestDef As Questionario In _listaQuestionari
            If oquestDef.isDefault = True Then
                listaQuestionariUtente.Add(oquestDef)
            End If
        Next
        Return listaQuestionariUtente
    End Function

    Public Shared Function readQuestionariPubbliciByPersona(ByVal idPersona As Integer, ByVal idLingua As Integer, ByVal tipo As Integer) As List(Of Questionario)

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim _listaQuestionari As New List(Of Questionario)
        Dim _listID As New List(Of Integer)
        Dim sqlCommand As String
        Dim dbCommand As DbCommand

        Using connection As DbConnection = db.CreateConnection()
            connection.Open()

            Select Case tipo
                Case Questionario.TipoQuestionario.Sondaggio
                    sqlCommand = "sp_Questionario_SondaggiPubblici_Select"
                Case Questionario.TipoQuestionario.Autovalutazione
                    sqlCommand = "sp_Questionario_AutovalutazionePubbliciByPersona_Select"
                Case Questionario.TipoQuestionario.Meeting
                    sqlCommand = "sp_Questionario_MeetingPubblici_Select"
                Case Else
                    sqlCommand = "sp_Questionario_QuestionariPubblici_Select"
            End Select

            dbCommand = db.GetStoredProcCommand(sqlCommand)
            dbCommand.Connection = connection
            db.AddInParameter(dbCommand, "idPersona", DbType.Int32, idPersona)

            Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)

                While sqlReader.Read()
                    Dim _quest As New Questionario
                    _quest.id = isNullInt(sqlReader.Item("QSTN_Id"))
                    _quest.nome = isNullString(sqlReader.Item("QSML_Nome"))
                    _quest.descrizione = isNullString(sqlReader.Item("QSML_Descrizione"))
                    _quest.dataInizio = isNullDateMin(sqlReader.Item("QSTN_DataInizio"))
                    _quest.dataFine = isNullDateMax(sqlReader.Item("QSTN_DataFine"))
                    _quest.creator = isNullString(sqlReader.Item("autore"))
                    _quest.idLingua = isNullInt(sqlReader.Item("QSML_IdLingua"))
                    _quest.isDefault = isNullBoolean(sqlReader.Item("QSML_IsDefault"))
                    _quest.isBloccato = isNullBoolean(sqlReader.Item("QSML_IsBloccato"))

                    If tipo = Questionario.TipoQuestionario.Autovalutazione Then
                        _quest.nRisposte = isNullInt(sqlReader.Item("quanteRisposte"))
                    End If
                    'Dim _risp As New RispostaQuestionario
                    '_risp.id = isNullInt(sqlReader.Item("IdRisposta"))
                    '_risp.dataFine = isNullDate(sqlReader.Item("DataFine"))
                    '_quest.rispostaQuest = _risp

                    'If _risp.id > 0 And _risp.dataFine = Date.MinValue Then
                    _quest.stato = Questionario.StatoQuestionario.NonCompilato
                    'ElseIf _risp.id > 0 And Not _risp.dataFine = Date.MinValue Then
                    '    _quest.stato = Questionario.StatoQuestionario.Compilato
                    'Else
                    '    _quest.stato = Questionario.StatoQuestionario.NonCompilato
                    'End If

                    _quest.linguePresenti = DALQuestionario.readLingueQuestionario(_quest.id)
                    _quest.visualizzaRisposta = isNullBoolean(sqlReader.Item("QSTN_visualizzaRisposta"))
                    _quest.editaRisposta = isNullBoolean(sqlReader.Item("QSTN_editaRisposta"))
                    _quest.risultatiAnonimi = isNullBoolean(sqlReader.Item("QSTN_RisultatiAnonimi"))
                    _quest.durata = isNullInt(sqlReader.Item("QSTN_Durata"))
                    'If _quest.isDefault Then
                    _listaQuestionari.Add(_quest)
                    'End If

                End While
                sqlReader.Close()
            End Using

            connection.Close()
        End Using

        'Dim listaQuestionariAltreLingue As New list(Of Questionario)

        Dim listaQuestionariUtente As New List(Of Questionario)
        'Dim listID As New ArrayList

        For Each oquest As Questionario In _listaQuestionari
            If oquest.idLingua = idLingua Then
                'listID.Add(oquest.id)
                listaQuestionariUtente.Add(oquest)
            End If
        Next

        For Each oquestUt As Questionario In listaQuestionariUtente
            Questionario.removeQuestionariBYID(_listaQuestionari, oquestUt.id)
        Next

        For Each oquestDef As Questionario In _listaQuestionari
            If oquestDef.isDefault = True Then
                listaQuestionariUtente.Add(oquestDef)
            End If
        Next



        Return listaQuestionariUtente

    End Function
    Public Shared Function readQuestionariInvitoByPersona(ByVal idPersona As Integer, ByVal idLingua As Integer, ByVal tipo As Integer) As List(Of Questionario)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim _listaQuestionari As New List(Of Questionario)
        Dim _listID As New List(Of Integer)
        Dim sqlCommand As String
        Dim dbCommand As DbCommand
        Using connection As DbConnection = db.CreateConnection()
            connection.Open()
            Select Case tipo

                Case Questionario.TipoQuestionario.Sondaggio
                    sqlCommand = "sp_Questionario_SondaggiInvitoByPersona_Select"
                Case Questionario.TipoQuestionario.Meeting
                    sqlCommand = "sp_Questionario_MeetingInvitoByPersona_Select"
                Case Questionario.TipoQuestionario.Autovalutazione
                    sqlCommand = "sp_Questionario_AutovalutazioneInvitoByPersona_Select"
                Case Else
                    sqlCommand = "sp_Questionario_QuestionariInvitoByPersona_Select"
            End Select
            dbCommand = db.GetStoredProcCommand(sqlCommand)
            dbCommand.Connection = connection
            db.AddInParameter(dbCommand, "idPersona", DbType.Int32, idPersona)
            Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                While sqlReader.Read()
                    Dim _quest As New Questionario
                    _quest.id = isNullInt(sqlReader.Item("QSTN_Id"))
                    _quest.nome = isNullString(sqlReader.Item("QSML_Nome"))
                    _quest.descrizione = isNullString(sqlReader.Item("QSML_Descrizione"))
                    _quest.dataInizio = isNullDateMin(sqlReader.Item("QSTN_DataInizio"))
                    _quest.dataFine = isNullDateMax(sqlReader.Item("QSTN_DataFine"))
                    _quest.creator = isNullString(sqlReader.Item("autore"))
                    _quest.idLingua = isNullInt(sqlReader.Item("QSML_IdLingua"))
                    _quest.isDefault = isNullBoolean(sqlReader.Item("QSML_IsDefault"))
                    _quest.isBloccato = isNullBoolean(sqlReader.Item("QSML_IsBloccato"))

                    _quest.stato = Questionario.StatoQuestionario.NonCompilato
                    _quest.linguePresenti = DALQuestionario.readLingueQuestionario(_quest.id)
                    If tipo = Questionario.TipoQuestionario.Autovalutazione Then
                        _quest.nRisposte = isNullInt(sqlReader.Item("quanteRisposte"))
                    End If
                    _quest.visualizzaRisposta = isNullBoolean(sqlReader.Item("QSTN_visualizzaRisposta"))
                    _quest.editaRisposta = isNullBoolean(sqlReader.Item("QSTN_editaRisposta"))
                    _quest.risultatiAnonimi = isNullBoolean(sqlReader.Item("QSTN_RisultatiAnonimi"))
                    _quest.durata = isNullInt(sqlReader.Item("QSTN_Durata"))
                    _listaQuestionari.Add(_quest)

                End While
                sqlReader.Close()
            End Using

            connection.Close()
        End Using
        'Dim listaQuestionariAltreLingue As New list(Of Questionario)
        Dim listaQuestionariUtente As New List(Of Questionario)
        'Dim listID As New ArrayList
        For Each oquest As Questionario In _listaQuestionari
            If oquest.idLingua = idLingua Then
                'listID.Add(oquest.id)
                listaQuestionariUtente.Add(oquest)
            End If
        Next
        For Each oquestUt As Questionario In listaQuestionariUtente
            Questionario.removeQuestionariBYID(_listaQuestionari, oquestUt.id)
        Next
        For Each oquestDef As Questionario In _listaQuestionari
            If oquestDef.isDefault = True Then
                listaQuestionariUtente.Add(oquestDef)
            End If
        Next
        Return listaQuestionariUtente

    End Function
    Public Shared Function InsertRandomDestinatario(ByRef oQuestionario As Questionario, Optional ByRef idPersona As Integer = 0) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String
        Dim dbCommand As DbCommand
        Dim RetVal As String = ""
        sqlCommand = "sp_Questionario_QuestionarioRandomDestinatario_Insert"
        dbCommand = db.GetStoredProcCommand(sqlCommand)
        db.AddInParameter(dbCommand, "idPadre", DbType.Int64, oQuestionario.id)
        db.AddInParameter(dbCommand, "dataCreazioneFiglio", DbType.DateTime, Now())
        If idPersona = 0 Then
            'usato per salvare correttamente gli idPersona nei quest di autovalutazione
            db.AddInParameter(dbCommand, "idDestinatario_Persona", DbType.Int32, oQuestionario.idDestinatario_Persona)
        Else
            db.AddInParameter(dbCommand, "idDestinatario_Persona", DbType.Int32, idPersona)
        End If
        db.AddInParameter(dbCommand, "idDestinatario_UtenteInvitato", DbType.Int32, oQuestionario.idDestinatario_UtenteInvitato)
        db.AddOutParameter(dbCommand, "@idQuestionarioRandom", DbType.Int32, 4)
        Try
            db.ExecuteNonQuery(dbCommand)
            oQuestionario.idFiglio = db.GetParameterValue(dbCommand, "@idQuestionarioRandom")
            RetVal = oQuestionario.idFiglio
        Catch ex As Exception
            RetVal = ex.Message
        End Try
        Return RetVal
    End Function
    Public Shared Function Insert(appContext As iApplicationContext, ByRef oQuestionario As Questionario) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String
        Dim dbCommand As DbCommand

        sqlCommand = "sp_Questionario_Questionario_Insert"
        dbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "nome", DbType.String, oQuestionario.nome)
        db.AddInParameter(dbCommand, "descrizione", DbType.String, oQuestionario.descrizione)
        db.AddInParameter(dbCommand, "dataCreazione", DbType.DateTime, Now())
        db.AddInParameter(dbCommand, "idPersonaCreator", DbType.Int32, oQuestionario.idPersonaCreator)
        db.AddInParameter(dbCommand, "idGruppo", DbType.Int32, setNull(oQuestionario.idGruppo))
        db.AddInParameter(dbCommand, "dataInizio", DbType.DateTime, setNullDate(oQuestionario.dataInizio))
        db.AddInParameter(dbCommand, "dataFine", DbType.DateTime, setNullDate(oQuestionario.dataFine))
        db.AddInParameter(dbCommand, "isBloccato", DbType.Boolean, oQuestionario.isBloccato)
        db.AddInParameter(dbCommand, "tipo", DbType.Int32, oQuestionario.tipo)
        db.AddInParameter(dbCommand, "idLingua", DbType.Int32, oQuestionario.idLingua)
        db.AddInParameter(dbCommand, "pesoTotale", DbType.Int32, oQuestionario.pesoTotale)
        db.AddInParameter(dbCommand, "scalaValutazione", DbType.Int32, oQuestionario.scalaValutazione)
        db.AddInParameter(dbCommand, "isChiuso", DbType.Int32, oQuestionario.isReadOnly)
        db.AddOutParameter(dbCommand, "@idQML", DbType.Int32, 4)
        db.AddOutParameter(dbCommand, "@idQuestionario", DbType.Int32, 4)
        db.AddInParameter(dbCommand, "durata", DbType.Int32, setNull(oQuestionario.durata))
        db.AddInParameter(dbCommand, "forUtentiComunita", DbType.Boolean, oQuestionario.forUtentiComunita)
        db.AddInParameter(dbCommand, "forUtentiPortale", DbType.Boolean, oQuestionario.forUtentiPortale)
        db.AddInParameter(dbCommand, "forUtentiInvitati", DbType.Boolean, oQuestionario.forUtentiInvitati)
        db.AddInParameter(dbCommand, "forUtentiEsterni", DbType.Boolean, oQuestionario.forUtentiEsterni)
        db.AddInParameter(dbCommand, "risultatiAnonimi", DbType.Boolean, oQuestionario.risultatiAnonimi)
        db.AddInParameter(dbCommand, "visualizzaRisposta", DbType.Boolean, oQuestionario.visualizzaRisposta)
        db.AddInParameter(dbCommand, "visualizzaCorrezione", DbType.Boolean, oQuestionario.visualizzaCorrezione)
        db.AddInParameter(dbCommand, "visualizzaSuggerimenti", DbType.Boolean, oQuestionario.visualizzaSuggerimenti)
        db.AddInParameter(dbCommand, "editaRisposta", DbType.Boolean, oQuestionario.editaRisposta)
        db.AddInParameter(dbCommand, "idPersonaEditor", DbType.Int32, oQuestionario.idPersonaEditor)
        db.AddInParameter(dbCommand, "dataModifica", DbType.DateTime, setNullDate(oQuestionario.dataModifica))
        db.AddInParameter(dbCommand, "isRandomOrder", DbType.Boolean, oQuestionario.isRandomOrder)
        db.AddInParameter(dbCommand, "isRandomOrder_Options", DbType.Boolean, oQuestionario.isRandomOrder_Options)
        db.AddInParameter(dbCommand, "nQuestionsPerPage", DbType.Int16, oQuestionario.nQuestionsPerPage)
        db.AddInParameter(dbCommand, "ownerType", DbType.Int32, oQuestionario.ownerType)
        db.AddInParameter(dbCommand, "ownerId", DbType.Int64, oQuestionario.ownerId)
        db.AddInParameter(dbCommand, "ownerGUID", DbType.Guid, oQuestionario.ownerGUID)

        Dim RetVal As String = ""

        Try
            db.ExecuteNonQuery(dbCommand)
            oQuestionario.id = db.GetParameterValue(dbCommand, "@idQuestionario")
            RetVal = db.GetParameterValue(dbCommand, "@idQML")
        Catch ex As Exception
            RetVal = ex.Message
        End Try
        If oQuestionario.id > 0 Then
            If oQuestionario.tipo = QuestionnaireType.RandomMultipleAttempts Then
                DALQuestionario.SaveRepeatSettings(appContext, oQuestionario.id, oQuestionario.MinScore, oQuestionario.MaxAttempts, oQuestionario.DisplayScoreToUser, oQuestionario.DisplayAttemptScoreToUser, oQuestionario.DisplayAvailableAttempts, oQuestionario.DisplayResultsStatus, oQuestionario.DisplayCurrentAttempts)
            ElseIf oQuestionario.id > 0 AndAlso (oQuestionario.tipo = QuestionnaireType.Random OrElse oQuestionario.tipo = QuestionnaireType.Standard) AndAlso oQuestionario.ownerType <> COL_BusinessLogic_v2.OwnerType_enum.None Then
                DALQuestionario.SaveSettings(appContext, oQuestionario.id, oQuestionario.DisplayScoreToUser, oQuestionario.DisplayAttemptScoreToUser, oQuestionario.DisplayAvailableAttempts, oQuestionario.DisplayResultsStatus, oQuestionario.DisplayCurrentAttempts)
            End If
        End If

        Return RetVal
    End Function
    Public Shared Function QuestionarioMultiLingua_Insert(ByRef oQuest As Questionario) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim sqlCommand As String = "sp_Questionario_Questionario_Multilingua_Insert"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, oQuest.id)
        db.AddInParameter(dbCommand, "nome", DbType.String, oQuest.nome)
        db.AddInParameter(dbCommand, "descrizione", DbType.String, oQuest.descrizione)
        db.AddInParameter(dbCommand, "isBloccato", DbType.Boolean, oQuest.isBloccato)
        db.AddInParameter(dbCommand, "idLingua", DbType.Int32, oQuest.idLingua)
        db.AddInParameter(dbCommand, "isDefault", DbType.Boolean, oQuest.isDefault)
        db.AddOutParameter(dbCommand, "idQML", DbType.Int32, 4)

        Dim RetVal As Integer

        Try
            db.ExecuteNonQuery(dbCommand)
            RetVal = db.GetParameterValue(dbCommand, "idQML")
        Catch ex As Exception
            RetVal = ex.Message
        End Try
        Return RetVal
    End Function
    Public Shared Function Update(appContext As iApplicationContext, ByRef oQuest As Questionario) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim sqlCommand As String = "sp_Questionario_Questionario_Update"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, oQuest.id)
        db.AddInParameter(dbCommand, "nome", DbType.String, oQuest.nome)
        db.AddInParameter(dbCommand, "descrizione", DbType.String, oQuest.descrizione)
        db.AddInParameter(dbCommand, "idGruppo", DbType.Int32, setNull(oQuest.idGruppo))
        db.AddInParameter(dbCommand, "dataInizio", DbType.DateTime, setNullDate(oQuest.dataInizio))
        db.AddInParameter(dbCommand, "dataFine", DbType.DateTime, setNullDate(oQuest.dataFine))
        db.AddInParameter(dbCommand, "isBloccato", DbType.Boolean, oQuest.isBloccato)
        db.AddInParameter(dbCommand, "idLingua", DbType.Int32, oQuest.idLingua)
        db.AddInParameter(dbCommand, "pesoTotale", DbType.Int32, oQuest.pesoTotale)
        db.AddInParameter(dbCommand, "scalaValutazione", DbType.Int32, oQuest.scalaValutazione)
        db.AddInParameter(dbCommand, "isChiuso", DbType.Boolean, oQuest.isReadOnly)
        db.AddInParameter(dbCommand, "durata", DbType.Int32, setNull(oQuest.durata))
        db.AddInParameter(dbCommand, "forUtentiComunita", DbType.Boolean, oQuest.forUtentiComunita)
        db.AddInParameter(dbCommand, "forUtentiPortale", DbType.Boolean, oQuest.forUtentiPortale)
        db.AddInParameter(dbCommand, "forUtentiInvitati", DbType.Boolean, oQuest.forUtentiInvitati)
        db.AddInParameter(dbCommand, "forUtentiEsterni", DbType.Boolean, oQuest.forUtentiEsterni)
        db.AddInParameter(dbCommand, "risultatiAnonimi", DbType.Boolean, oQuest.risultatiAnonimi)
        db.AddInParameter(dbCommand, "visualizzaRisposta", DbType.Boolean, oQuest.visualizzaRisposta)
        db.AddInParameter(dbCommand, "visualizzaCorrezione", DbType.Boolean, oQuest.visualizzaCorrezione)
        db.AddInParameter(dbCommand, "visualizzaSuggerimenti", DbType.Boolean, oQuest.visualizzaSuggerimenti)
        db.AddInParameter(dbCommand, "editaRisposta", DbType.Boolean, oQuest.editaRisposta)
        db.AddInParameter(dbCommand, "idPersonaEditor", DbType.Int32, oQuest.idPersonaEditor)
        db.AddInParameter(dbCommand, "dataModifica", DbType.DateTime, setNullDate(oQuest.dataModifica))
        db.AddInParameter(dbCommand, "isRandomOrder", DbType.Boolean, oQuest.isRandomOrder)
        db.AddInParameter(dbCommand, "isRandomOrder_Options", DbType.Boolean, oQuest.isRandomOrder_Options)
        db.AddInParameter(dbCommand, "nQuestionsPerPage", DbType.Int16, oQuest.nQuestionsPerPage)
        db.AddInParameter(dbCommand, "ownerType", DbType.Int32, oQuest.ownerType)
        db.AddInParameter(dbCommand, "ownerId", DbType.Int64, oQuest.ownerId)
        db.AddInParameter(dbCommand, "ownerGUID", DbType.Guid, oQuest.ownerGUID)

        Dim RetVal As String = ""

        Try
            RetVal = db.ExecuteNonQuery(dbCommand)
        Catch ex As Exception
            RetVal = ex.Message
        End Try
        If oQuest.id > 0 AndAlso oQuest.tipo = QuestionnaireType.RandomMultipleAttempts Then
            DALQuestionario.SaveRepeatSettings(appContext, oQuest.id, oQuest.MinScore, oQuest.MaxAttempts, oQuest.DisplayScoreToUser, oQuest.DisplayAttemptScoreToUser, oQuest.DisplayAvailableAttempts, oQuest.DisplayResultsStatus, oQuest.DisplayCurrentAttempts)
        ElseIf oQuest.id > 0 AndAlso oQuest.tipo = QuestionnaireType.QuestionLibrary Then
            Dim pIndex As Integer = 0
            For Each p As QuestionarioPagina In oQuest.pagine.OrderBy(Function(qP) qP.numeroPagina).ToList
                p.nomePagina = oQuest.nome & " p. " & p.numeroPagina
                DALPagine.Pagina_Update(p)
            Next
        ElseIf oQuest.id > 0 AndAlso (oQuest.tipo = QuestionnaireType.Random OrElse oQuest.tipo = QuestionnaireType.Standard) AndAlso oQuest.ownerType <> COL_BusinessLogic_v2.OwnerType_enum.None Then
            DALQuestionario.SaveSettings(appContext, oQuest.id, oQuest.DisplayScoreToUser, oQuest.DisplayAttemptScoreToUser, oQuest.DisplayAvailableAttempts, oQuest.DisplayResultsStatus, oQuest.DisplayCurrentAttempts)
        End If
        Return RetVal
    End Function
    Public Shared Function UpdateNome(ByRef oQuest As Questionario) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim sqlCommand As String = "sp_Questionario_Questionario_UpdateNome"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idQuestionarioMultilingua", DbType.Int32, oQuest.idQuestionarioMultilingua)
        db.AddInParameter(dbCommand, "nome", DbType.String, oQuest.nome)
        db.AddInParameter(dbCommand, "descrizione", DbType.String, oQuest.descrizione)
        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, oQuest.id)
        db.AddInParameter(dbCommand, "isChiuso", DbType.Boolean, oQuest.isReadOnly)
        db.AddInParameter(dbCommand, "visualizzaRisposta", DbType.Boolean, oQuest.visualizzaRisposta)
        db.AddInParameter(dbCommand, "visualizzaCorrezione", DbType.Boolean, oQuest.visualizzaCorrezione)
        db.AddInParameter(dbCommand, "visualizzaSuggerimenti", DbType.Boolean, oQuest.visualizzaSuggerimenti)
        db.AddInParameter(dbCommand, "editaRisposta", DbType.Boolean, oQuest.editaRisposta)
        db.AddInParameter(dbCommand, "forUtentiComunita", DbType.Boolean, oQuest.forUtentiComunita)
        db.AddInParameter(dbCommand, "forUtentiPortale", DbType.Boolean, oQuest.forUtentiPortale)
        db.AddInParameter(dbCommand, "forUtentiInvitati", DbType.Boolean, oQuest.forUtentiInvitati)
        db.AddInParameter(dbCommand, "forUtentiEsterni", DbType.Boolean, oQuest.forUtentiEsterni)
        db.AddInParameter(dbCommand, "durata", DbType.Boolean, oQuest.durata)
        db.AddInParameter(dbCommand, "isBloccato", DbType.Boolean, oQuest.isBloccato)

        Dim RetVal As String = ""

        Try
            db.ExecuteNonQuery(dbCommand)
            RetVal = True
        Catch ex As Exception
            RetVal = False
        End Try

        Return RetVal

    End Function
    Public Shared Function chiudiQuestionario(ByRef oQuest As Questionario) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim sqlCommand As String = "sp_Questionario_Questionario_Chiudi"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, oQuest.id)
        db.AddInParameter(dbCommand, "isChiuso", DbType.Boolean, oQuest.isReadOnly)

        Dim RetVal As String = ""

        Try
            RetVal = db.ExecuteNonQuery(dbCommand)
        Catch ex As Exception
            RetVal = ex.Message
        End Try

        Return RetVal
    End Function
    'Public Shared Function DeleteQuestionarioMultilingua_Physical(ByRef idQuestionarioMultilingua As Int32) As String

    '    Dim db As Database = DatabaseFactory.CreateDatabase()

    '    Dim sqlCommand As String = "sp_Questionario_QuestionarioMultilingua_DeletePhysical"
    '    Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

    '    db.AddInParameter(dbCommand, "idQuestionarioMultilingua", DbType.Int32, idQuestionarioMultilingua)

    '    Dim RetVal As Integer

    '    Try
    '        db.ExecuteNonQuery(dbCommand)

    '    Catch ex As Exception

    '    End Try

    '    Return RetVal
    'End Function
    Public Shared Function IsBloccatoByIdQuestionario_Update(ByRef id As Integer, ByRef isBloccato As Boolean) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim sqlCommand As String = "sp_Questionario_isBloccato_Update"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, id)
        db.AddInParameter(dbCommand, "isBloccato", DbType.Int32, isBloccato)

        Dim RetVal As String = ""

        Try
            RetVal = db.ExecuteNonQuery(dbCommand)
        Catch ex As Exception
            RetVal = ex.Message
        End Try

        Return RetVal
    End Function
    Public Shared Function DeleteQuestionario_Physical(ByRef idQuestionarioML As Int32) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim retval As String = String.Empty

        Dim sqlCommand As String = "sp_Questionario_Questionario_DeletePhysical"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idQuestionarioMultilingua", DbType.Int32, idQuestionarioML)

        Try
            retval = db.ExecuteNonQuery(dbCommand)
        Catch ex As Exception

        End Try

        Return retval
    End Function
    Public Shared Function DeleteQuestionarioRandomNoRisposte_Physical(ByRef idPadre As Int32) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim retval As String = String.Empty

        Dim sqlCommand As String = "sp_Questionario_QuestionarioRandomNoRisposte_Delete"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idPadre", DbType.Int32, idPadre)

        Try
            retval = db.ExecuteNonQuery(dbCommand)
        Catch ex As Exception

        End Try

        Return retval
    End Function
    Public Shared Function DeleteQuestionario(ByRef oQuest As Questionario) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim sqlCommand As String = "sp_Questionario_Questionario_Delete"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, oQuest.idQuestionarioMultilingua)

        Dim RetVal As String = ""

        Try
            RetVal = db.ExecuteNonQuery(dbCommand)
        Catch ex As Exception
            RetVal = ex.Message
        End Try

        Return RetVal
    End Function
    Public Shared Function DeleteQuestionarioByIDLingua(ByRef idQuestionario As Integer, ByVal idLingua As Integer) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim sqlCommand As String = "sp_Questionario_QuestionarioBYIDLingua_Delete"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuestionario)
        db.AddInParameter(dbCommand, "idLingua", DbType.Int32, idLingua)

        Dim RetVal As String = ""

        Try
            RetVal = db.ExecuteNonQuery(dbCommand)
        Catch ex As Exception
            RetVal = ex.Message
        End Try

        Return RetVal
    End Function
    Public Shared Function DeleteQuestionarioBYPadre(ByRef oQuest As Questionario) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "sp_Questionario_Questionario_DeleteByPadre"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, oQuest.id)
        Dim RetVal As String = ""
        Try
            RetVal = db.ExecuteNonQuery(dbCommand)
        Catch ex As Exception
            RetVal = ex.Message
        End Try
        Return RetVal
    End Function
    Public Shared Function RipristinaQuestionario(ByRef oQuest As Questionario) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim sqlCommand As String = "sp_Questionario_Questionario_Ripristina"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idQuestionarioMultilingua", DbType.Int32, oQuest.idQuestionarioMultilingua)

        Dim RetVal As String = ""

        Try
            RetVal = db.ExecuteNonQuery(dbCommand)
        Catch ex As Exception
            RetVal = ex.Message
        End Try

        Return RetVal
    End Function
    Public Shared Function Salva(appContext As iApplicationContext, ByRef quest As Questionario) As Integer
        Dim retVal As Integer

        If quest.id > 0 Then
            retVal = Update(appContext, quest)
        Else
            retVal = Insert(appContext, quest)
        End If

        Return retVal
    End Function
    Public Shared Function AggiornaPesoEDifficolta(ByRef oQuest As Questionario) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim sqlCommand As String = "sp_Questionario_PesoEDifficolta_Update"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "pesoTotale", DbType.Int32, oQuest.pesoTotale)
        db.AddInParameter(dbCommand, "nDiffBassa", DbType.Int32, oQuest.nDomandeDiffBassa)
        db.AddInParameter(dbCommand, "nDiffMedia", DbType.Int32, oQuest.nDomandeDiffMedia)
        db.AddInParameter(dbCommand, "nDiffAlta", DbType.Int32, oQuest.nDomandeDiffAlta)
        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, oQuest.id)


        Dim RetVal As String = ""

        Try
            RetVal = db.ExecuteNonQuery(dbCommand)
        Catch ex As Exception
            RetVal = ex.Message
        End Try

        Return RetVal
    End Function
    Public Shared Function readComunitaByIDPersona(ByVal idPersona As Integer, ByVal oComunitaCorrente As COL_Comunita) As List(Of COL_Comunita)

        Dim communityTemp As New List(Of COL_Comunita)
        Dim RetVal As String = ""
        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim sqlCommand As String = "sp_Questionario_ComunitaByIDPersona_Select"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        db.AddInParameter(dbCommand, "idPersona", DbType.Int32, idPersona)
        db.AddInParameter(dbCommand, "idComunita", DbType.Int32, oComunitaCorrente.Id)

        Dim oServizio As New COL_BusinessLogic_v2.UCServices.Services_Questionario
        Dim communityVoid As New COL_Comunita
        communityVoid.Id = Integer.MinValue
        communityVoid.Nome = "-------------------"
        communityTemp.Add(communityVoid)
        communityTemp.Add(oComunitaCorrente)
        Dim idCommunitites As New List(Of Integer)
        Try
            Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                Dim idCommunity As Integer = 0
                While sqlReader.Read()
                    idCommunity = isNullInt(sqlReader.Item("CMNT_Id"))
                    oServizio.PermessiAssociati = isNullString(sqlReader.Item("LKSC_Permessi"))
                    If Not idCommunitites.Contains(idCommunity) AndAlso (oServizio.Admin OrElse oServizio.CopiaQuestionario) Then
                        Dim oCom As New COL_Comunita
                        oCom.Id = idCommunity
                        oCom.Nome = isNullString(sqlReader.Item("CMNT_Nome"))
                        communityTemp.Add(oCom)
                        idCommunitites.Add(idCommunity)
                    End If
                End While
                sqlReader.Close()
            End Using
        Catch ex As Exception

        End Try

        Return communityTemp
    End Function
    Public Shared Function readLibrerieByComunita(appContext As iApplicationContext, ByVal idComunita As Integer) As List(Of Questionario)

        'Dim db As Database = DatabaseFactory.CreateDatabase()
        'Dim items As New List(Of Questionario)

        'Using connection As DbConnection = db.CreateConnection()
        '    connection.Open()

        '    Dim sqlCommand2 As String = "sp_Questionario_LibrerieBYComunita_Select"
        '    Dim dbCommand2 As DbCommand = db.GetStoredProcCommand(sqlCommand2)
        '    dbCommand2.Connection = connection
        '    db.AddInParameter(dbCommand2, "idComunita", DbType.Int32, idComunita)

        '    Using sqlReader2 As IDataReader = db.ExecuteReader(dbCommand2)
        '        While sqlReader2.Read()
        '            Dim _quest As New Questionario
        '            _quest.id = isNullInt(sqlReader2.Item("QSTN_Id"))
        '            _quest.nome = isNullString(sqlReader2.Item("QSML_Nome"))
        '            _quest.descrizione = isNullString(sqlReader2.Item("QSML_Descrizione"))
        '            _quest.nDomandeDiffBassa = isNullInt(sqlReader2.Item("QSTN_nDomandeDiffBassa"))
        '            _quest.nDomandeDiffMedia = isNullInt(sqlReader2.Item("QSTN_nDomandeDiffMedia"))
        '            _quest.nDomandeDiffAlta = isNullInt(sqlReader2.Item("QSTN_nDomandeDiffAlta"))
        '            _quest.idLingua = isNullInt(sqlReader2.Item("QSML_IdLingua"))
        '            _quest.idQuestionarioMultilingua = isNullInt(sqlReader2.Item("QSML_Id"))
        '            items.Add(_quest)
        '        End While
        '        sqlReader2.Close()
        '    End Using
        '    connection.Close()
        'End Using
        Return readLibrerieByComunita(appContext, idComunita, 0)
    End Function
    Public Shared Function readLibrerieByComunita(appContext As iApplicationContext, ByVal idComunita As Integer, ByVal idLanguage As Integer) As List(Of Questionario)
        Dim s As ServiceQuestionnaire = GetService(appContext)
        Dim items As List(Of Questionario) = s.GetAvailableLibraries(appContext.UserContext.CurrentUserID, idComunita, idLanguage)

        Return items
    End Function
    Public Shared Function readLibrerieQuestionarioByComunita(ByVal appContext As iApplicationContext, ByVal oQuest As Questionario, ByVal idComunita As Integer) As List(Of Questionario)

        Dim s As ServiceQuestionnaire = GetService(appContext)
        Dim items As List(Of Questionario) = s.GetAvailableLibraries(appContext.UserContext.CurrentUserID, idComunita, oQuest.idLingua)

        Return items

        'Dim db As Database = DatabaseFactory.CreateDatabase()
        'Dim _lib As New List(Of Questionario)

        'Using connection As DbConnection = db.CreateConnection()
        '    connection.Open()

        '    Dim sqlCommand2 As String = "sp_Questionario_LibrerieBYComunita_Select"
        '    Dim dbCommand2 As DbCommand = db.GetStoredProcCommand(sqlCommand2)
        '    dbCommand2.Connection = connection
        '    db.AddInParameter(dbCommand2, "idComunita", DbType.Int32, idComunita)

        '    Using sqlReader2 As IDataReader = db.ExecuteReader(dbCommand2)
        '        While sqlReader2.Read()
        '            Dim _quest As New Questionario
        '            _quest.id = isNullInt(sqlReader2.Item("QSTN_Id"))
        '            _quest.nome = isNullString(sqlReader2.Item("QSML_Nome"))
        '            _quest.descrizione = isNullString(sqlReader2.Item("QSML_Descrizione"))
        '            _quest.nDomandeDiffBassa = isNullInt(sqlReader2.Item("QSTN_nDomandeDiffBassa"))
        '            _quest.nDomandeDiffMedia = isNullInt(sqlReader2.Item("QSTN_nDomandeDiffMedia"))
        '            _quest.nDomandeDiffAlta = isNullInt(sqlReader2.Item("QSTN_nDomandeDiffAlta"))
        '            _quest.idLingua = isNullInt(sqlReader2.Item("QSML_IdLingua"))
        '            _quest.idQuestionarioMultilingua = isNullInt(sqlReader2.Item("QSML_Id"))
        '            _lib.Add(_quest)
        '        End While
        '        sqlReader2.Close()
        '    End Using

        '    connection.Close()
        'End Using

        'Dim oLibrerieLinguaQuest As New List(Of Questionario)

        'For Each olib As Questionario In _lib
        '    Dim oLibreriaPresente As New Questionario
        '    oLibreriaPresente = Questionario.findQuestionarioBYID(oLibrerieLinguaQuest, olib.id)
        '    If oLibreriaPresente Is Nothing Then
        '        oLibrerieLinguaQuest.Add(olib)
        '    Else
        '        If olib.idLingua = oQuest.idLingua Then
        '            oLibrerieLinguaQuest.Remove(oLibreriaPresente)
        '            oLibrerieLinguaQuest.Add(olib)
        '        End If
        '    End If
        'Next

        ' Return oLibrerieLinguaQuest

    End Function
    Public Shared Function readQuestionariByComunita(ByVal idComunita As Integer) As List(Of Questionario)

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim _lib As New List(Of Questionario)

        Using connection As DbConnection = db.CreateConnection()
            connection.Open()

            '  Dim sqlCommand2 As String = "sp_Questionario_QuestionariBYComunita_Select"
            Dim sqlCommand As String = "SELECT  QSTN_Id, QSTN_nDomandeDiffBassa, QSTN_nDomandeDiffMedia, QSTN_nDomandeDiffAlta, QSML_Id, QSML_Nome, QSML_Descrizione, QSGR_CMNT_Id, QSML_IdLingua"
            sqlCommand &= " FROM QS_QUESTIONARIO INNER JOIN QS_QUESTIONARIO_MULTILINGUA ON QSML_QSTN_Id = QSTN_Id INNER JOIN QS_QUESTIONARIO_GRUPPO ON QSTN_QSGR_Id = QSGR_Id "
            sqlCommand &= " WHERE (QSML_IsCancellato = 0) AND (QSTN_Tipo = 0 or QSTN_Tipo = 4) AND (QSGR_CMNT_Id = " & idComunita.ToString & ") AND dbo.QS_QUESTIONARIO.QSTN_ownerType = 0 "
            sqlCommand &= " order by dbo.QS_QUESTIONARIO.QSTN_Id desc"

            Dim dbCommand2 As DbCommand = db.GetSqlStringCommand(sqlCommand)
            dbCommand2.Connection = connection
            'db.AddInParameter(dbCommand2, "idComunita", DbType.Int32, idComunita)

            Using sqlReader2 As IDataReader = db.ExecuteReader(dbCommand2)
                While sqlReader2.Read()
                    Dim _quest As New Questionario
                    _quest.id = isNullInt(sqlReader2.Item("QSTN_Id"))
                    _quest.nome = isNullString(sqlReader2.Item("QSML_Nome"))
                    _quest.descrizione = isNullString(sqlReader2.Item("QSML_Descrizione"))
                    _quest.nDomandeDiffBassa = isNullInt(sqlReader2.Item("QSTN_nDomandeDiffBassa"))
                    _quest.nDomandeDiffMedia = isNullInt(sqlReader2.Item("QSTN_nDomandeDiffMedia"))
                    _quest.nDomandeDiffAlta = isNullInt(sqlReader2.Item("QSTN_nDomandeDiffAlta"))
                    _quest.idLingua = isNullInt(sqlReader2.Item("QSML_IdLingua"))
                    _quest.idQuestionarioMultilingua = isNullInt(sqlReader2.Item("QSML_Id"))
                    _lib.Add(_quest)
                End While
                sqlReader2.Close()
            End Using

            connection.Close()
        End Using

        Return _lib

    End Function

    Public Shared Function readLingueQuestionario(ByVal idQuest As Integer) As List(Of Lingua)

        Dim _com As New List(Of Lingua)
        Dim RetVal As String = ""
        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim sqlCommand As String = "sp_Questionario_LinguaQuestionario_Select"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        db.AddInParameter(dbCommand, "idQuestionarioPadre", DbType.Int32, idQuest)

        Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
            While sqlReader.Read()
                'oCom.sigla = isNullString(sqlReader.Item("sigla")) 'su comOL non viene letto da DB
                _com.Add(Lingua.CreateByNameAndCode(isNullInt(sqlReader.Item("id")), isNullString(sqlReader.Item("nome")), isNullString(sqlReader.Item("codice"))))
            End While
            sqlReader.Close()
        End Using

        Return _com
    End Function
    Public Shared Function readLingueNonPresentiQuestionario(ByVal idQuest As Integer) As List(Of Lingua)

        Dim _com As New List(Of Lingua)
        Dim RetVal As String = ""
        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim sqlCommand As String = "sp_Questionario_LinguaNonPresenteQuestionario_Select"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuest)

        Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
            While sqlReader.Read()
                'Dim oCom As New Lingua
                'oCom.ID = isNullInt(sqlReader.Item("id"))
                'oCom.Nome = isNullString(sqlReader.Item("nome"))
                'oCom.sigla = isNullString(sqlReader.Item("sigla")) 'su ComOL non viene letto da DB
                _com.Add(Lingua.CreateByNameAndCode(isNullInt(sqlReader.Item("id")), isNullString(sqlReader.Item("nome")), isNullString(sqlReader.Item("codice"))))
            End While
            sqlReader.Close()
        End Using

        Return _com
    End Function
    Public Shared Function readLingue(ByVal idLinguaCorrente As Integer) As List(Of Lingua)

        Dim _com As New List(Of Lingua)
        Dim RetVal As String = ""
        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim sqlCommand As String = "sp_Questionario_Lingua_Select"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        db.AddInParameter(dbCommand, "idLinguaCorrente", DbType.Int32, idLinguaCorrente)


        Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
            While sqlReader.Read()
                _com.Add(Lingua.CreateByNameAndCode(isNullInt(sqlReader.Item("id")), isNullString(sqlReader.Item("nome")), isNullString(sqlReader.Item("codice"))))
            End While
            sqlReader.Close()
        End Using

        Return _com
    End Function
    Public Shared Function QuestionarioLibreria_Insert(ByRef oLibrerie As List(Of LibreriaQuestionario)) As Integer

        Dim RetVal As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()

        For Each oLib As LibreriaQuestionario In oLibrerie
            ' difficolta bassa
            Dim sqlCommand As String = "sp_Questionario_QuestionarioLibreria_Insert"
            Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, oLib.idQuestionario)
            db.AddInParameter(dbCommand, "idLibreria", DbType.Int32, oLib.idLibreria)
            db.AddInParameter(dbCommand, "nDomandeDiffBassa", DbType.Int32, setNullInt(oLib.nDomandeDiffBassa))
            db.AddInParameter(dbCommand, "nDomandeDiffMedia", DbType.Int32, setNullInt(oLib.nDomandeDiffMedia))
            db.AddInParameter(dbCommand, "nDomandeDiffAlta", DbType.Int32, setNullInt(oLib.nDomandeDiffAlta))
            Try
                db.ExecuteNonQuery(dbCommand)
            Catch ex As Exception

            End Try

        Next

        Return RetVal
    End Function
    Public Shared Function QuestionarioLibrerie_Delete(ByVal idQuest As Integer) As Integer

        Dim RetVal As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "sp_Questionario_QuestionarioLibrerie_Delete"

        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuest)

        Try
            db.ExecuteNonQuery(dbCommand)
        Catch ex As Exception

        End Try


        Return RetVal
    End Function
    Public Shared Function readQuestionarioLibrerie(ByVal oQuestionario As Questionario, Optional ByRef db As Database = Nothing) As List(Of LibreriaQuestionario)

        Dim oLibrerie As New List(Of LibreriaQuestionario)
        Dim RetVal As String = ""
        If db Is Nothing Then
            db = DatabaseFactory.CreateDatabase()
        End If
        Dim sqlCommand As String = "sp_Questionario_QuestionarioLibrerie_Select"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, oQuestionario.id)

        Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
            While sqlReader.Read()
                Dim oLibreria As New LibreriaQuestionario
                oLibreria.id = isNullInt(sqlReader.Item("LKQL_Id"))
                oLibreria.idLibreria = isNullInt(sqlReader.Item("LKQL_LIBRERIA_Id"))
                oLibreria.idQuestionario = isNullInt(sqlReader.Item("LKQL_QSTN_Id"))
                oLibreria.nDomandeDiffBassa = isNullInt(sqlReader.Item("LKQL_nDomandeDiffBassa"))
                oLibreria.nDomandeDiffMedia = isNullInt(sqlReader.Item("LKQL_nDomandeDiffMedia"))
                oLibreria.nDomandeDiffAlta = isNullInt(sqlReader.Item("LKQL_nDomandeDiffAlta"))
                oLibreria.nomeLibreria = isNullString(sqlReader.Item("QSML_Nome"))
                oLibreria.nDomandeDiffBassaDisponibili = isNullInt(sqlReader.Item("QSTN_nDomandeDiffBassa"))
                oLibreria.nDomandeDiffMediaDisponibili = isNullInt(sqlReader.Item("QSTN_nDomandeDiffMedia"))
                oLibreria.nDomandeDiffAltaDisponibili = isNullInt(sqlReader.Item("QSTN_nDomandeDiffAlta"))
                oLibreria.idLingua = isNullInt(sqlReader.Item("QSML_IdLingua"))
                oLibrerie.Add(oLibreria)
            End While
            sqlReader.Close()
        End Using

        Dim oLibrerieLinguaQuest As New List(Of LibreriaQuestionario)

        For Each olib As LibreriaQuestionario In oLibrerie

            Dim oLibreriaPresente As LibreriaQuestionario = oLibrerieLinguaQuest.Where(Function(l) l.idLibreria = olib.idLibreria).FirstOrDefault
            If IsNothing(oLibreriaPresente) Then
                oLibrerieLinguaQuest.Add(olib)
            Else
                If olib.idLingua = oQuestionario.idLingua Then
                    oLibrerieLinguaQuest.Remove(oLibreriaPresente)
                    oLibrerieLinguaQuest.Add(olib)
                End If
            End If
        Next

        Return oLibrerieLinguaQuest

    End Function
    Public Shared Function readDaySurveys(ByVal tipo As Integer, ByVal idcomunita As Integer) As List(Of Questionario)

        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim sqlCommand As String = "sp_Questionario_SelectQuestionariDelGiorno"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        db.AddInParameter(dbCommand, "idComunita", DbType.Int32, idcomunita)
        db.AddInParameter(dbCommand, "tipoQuestionario", DbType.Int32, tipo)
        ' db.AddInParameter(dbCommand, "idLingua", DbType.Int32, idLingua)
        'db.AddInParameter(dbCommand, "numberOfSurveys", DbType.Int32, numberOfSurveys)

        Dim oListQuest As New List(Of Questionario)

        Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
            While sqlReader.Read()
                Dim oQuest As New Questionario
                oQuest.id = isNullInt(sqlReader.Item("Id"))
                oQuest.nome = isNullString(sqlReader.Item("nome"))
                oQuest.idLingua = isNullInt(sqlReader.Item("idLingua"))
                oQuest.isDefault = isNullBoolean(sqlReader.Item("isDefault"))
                oQuest.tipo = tipo
                oListQuest.Add(oQuest)
            End While
            sqlReader.Close()
        End Using

        Return oListQuest

    End Function
    Public Shared Function CountQuestionariComunita(ByVal idComunita As Integer, ByVal tipo As Integer) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim retVal As Integer

        Dim sqlCommand As String = "sp_Questionario_CountQuestionariComunita"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        db.AddInParameter(dbCommand, "idComunita", DbType.Int32, idComunita)
        db.AddInParameter(dbCommand, "tipoQuestionario", DbType.Int32, tipo)

        retVal = db.ExecuteScalar(dbCommand)
        Return retVal

    End Function
    'Public Shared Function countDomandeLibreriaInQuestionari(ByVal idQuestionario As Integer) As Integer
    '    Dim db As Database = DatabaseFactory.CreateDatabase()
    '    Dim sqlCommand As String = "sp_Questionario_RispostaQuestionarioByIdQuestionario_count"
    '    Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
    '    db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuestionario)

    '    Dim retval As Int32
    '    retval = db.ExecuteScalar(dbCommand)
    '    Return retval
    'End Function
End Class