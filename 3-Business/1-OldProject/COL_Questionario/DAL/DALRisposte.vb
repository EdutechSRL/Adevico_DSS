Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports COL_Questionario.RootObject
Imports System.Text.RegularExpressions
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports System.Data.Common
Imports System.Data
Imports lm.Comol.Core.DomainModel

Public Class DALRisposte
    'Public Shared Function SalvaRispAutovalutazione(ByRef oRisposta As RispostaQuestionario)
    '    Dim retVal As String = String.Empty
    '    Try
    '        RispostaQuestionario_Insert(oRisposta)
    '        retVal = 1
    '    Catch ex As Exception
    '        retVal = 0
    '    End Try
    '    Return retVal
    'End Function
    Public Shared Sub readIdPersoneByIdQuestionario(ByVal idQuestionario As Integer, ByRef idPersonaList As List(Of Integer), ByRef idUtenteInvitatoList As List(Of Integer), ByRef idUtenteAnonimo As Integer, Optional ByRef db As Database = Nothing, Optional ByRef conn As DbConnection = Nothing)
        If db Is Nothing Then
            db = DatabaseFactory.CreateDatabase()
        End If
        If conn Is Nothing Then
            conn = db.CreateConnection()
        End If
        Dim sqlCommand As String = "sp_Questionario_idPersoneByIdQuestionario_Select"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        dbCommand.Connection = conn
        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuestionario)

        Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
            While sqlReader.Read()
                Dim idPersona As Integer
                idPersona = isNullInt(sqlReader.Item("RSQS_PRSN_Id"))
                If idPersona = 0 Or idPersona = idUtenteAnonimo Then
                    idUtenteInvitatoList.Add(sqlReader.Item("RSQS_QSUI_Id"))
                Else
                    idPersonaList.Add(idPersona)
                End If
            End While
            sqlReader.Close()
        End Using
    End Sub
    Public Sub clearRisposte(ByRef oRis As RispostaQuestionario)
        'Dim oRisOpzione As New RispostaDomanda
        'For Each oRisOpzione In oRis.risposteDomande
        'Select Case oRisOpzione.tipo
        '   Case Domanda.TipoDomanda.Multipla
        RispostaOpzioneMultipla_Delete(oRis.id)
        '  Case Domanda.TipoDomanda.DropDown
        RispostaDropDown_Delete(oRis.id)
        ' Case Domanda.TipoDomanda.Rating
        RispostaRating_Delete(oRis.id)
        'Case Domanda.TipoDomanda.Meeting
        'RispostaRating_Delete(oRis.id)
        'Case Domanda.TipoDomanda.TestoLibero
        RispostaTestoLibero_Delete(oRis.id)
        'Case Domanda.TipoDomanda.Numerica
        RispostaNumerica_Delete(oRis.id)
        'End Select
        'Next
    End Sub
    Public Shared Sub clearRisposte(ByVal idQuestionnaireAnswer As Integer)
        'Dim oRisOpzione As New RispostaDomanda
        'For Each oRisOpzione In oRis.risposteDomande
        'Select Case oRisOpzione.tipo
        '   Case Domanda.TipoDomanda.Multipla
        RispostaOpzioneMultipla_Delete(idQuestionnaireAnswer)
        '  Case Domanda.TipoDomanda.DropDown
        RispostaDropDown_Delete(idQuestionnaireAnswer)
        ' Case Domanda.TipoDomanda.Rating
        RispostaRating_Delete(idQuestionnaireAnswer)
        'Case Domanda.TipoDomanda.Meeting
        'RispostaRating_Delete(oRis.id)
        'Case Domanda.TipoDomanda.TestoLibero
        RispostaTestoLibero_Delete(idQuestionnaireAnswer)
        'Case Domanda.TipoDomanda.Numerica
        RispostaNumerica_Delete(idQuestionnaireAnswer)
        'End Select
        'Next
    End Sub
#Region "Risposta Questionario"
    Public Function countRisposteByIdPersona(ByRef idUtente As Integer, ByRef idQuestionario As Integer) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As Integer
        Dim sqlCommand As String = "sp_Questionario_CountRisposteUtenteQuestionario"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuestionario)
        db.AddInParameter(dbCommand, "idUtente", DbType.Int32, idUtente)
        RetVal = Integer.Parse(db.ExecuteScalar(dbCommand))
        Return RetVal
    End Function
    Public Shared Function RispostaQuestionario_Insert(ByRef oRis As RispostaQuestionario, ByRef idPersona As Integer) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""
        Dim sqlCommand As String = "sp_Questionario_RispostaQuestionario_Insert"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, oRis.idQuestionario)
        db.AddInParameter(dbCommand, "idQuestionarioRandom", DbType.Int32, oRis.idQuestionarioRandom)
        db.AddInParameter(dbCommand, "indirizzoIPStart", DbType.String, oRis.indirizzoIPStart)
        db.AddInParameter(dbCommand, "indirizzoIPEdit", DbType.String, oRis.indirizzoIPEdit)
        db.AddInParameter(dbCommand, "indirizzoIPEnd", DbType.String, oRis.indirizzoIPEnd)
        db.AddInParameter(dbCommand, "dataInizio", DbType.DateTime, setNullDate(oRis.dataInizio))
        db.AddInParameter(dbCommand, "dataFine", DbType.DateTime, setNullDate(oRis.dataFine))
        db.AddInParameter(dbCommand, "ultimaRisposta", DbType.Int32, oRis.ultimaRisposta)
        db.AddInParameter(dbCommand, "dataModifica", DbType.DateTime, setNullDate(oRis.dataModifica))
        db.AddInParameter(dbCommand, "idUtenteInvitato", DbType.Int32, setNull(oRis.idUtenteInvitato))
        db.AddInParameter(dbCommand, "idPersona", DbType.Int32, idPersona)
        db.AddOutParameter(dbCommand, "idRispostaQuestionario", DbType.Int32, 4)
        db.AddInParameter(dbCommand, "nTotali", DbType.Int32, setNullInt(oRis.oStatistica.nRisposteTotali))
        db.AddInParameter(dbCommand, "nSaltate", DbType.Int32, setNullInt(oRis.oStatistica.nRisposteSaltate))
        db.AddInParameter(dbCommand, "nCorrette", DbType.Int32, setNullInt(oRis.oStatistica.nRisposteCorrette))
        db.AddInParameter(dbCommand, "nNonValutate", DbType.Int32, setNullInt(oRis.oStatistica.nRisposteNonValutate))
        db.AddInParameter(dbCommand, "nErrate", DbType.Int32, setNullInt(oRis.oStatistica.nRisposteErrate))
        db.AddInParameter(dbCommand, "nParzialmenteCorrette", DbType.Int32, setNullInt(oRis.oStatistica.nRisposteParzialmenteCorrette))
        db.AddInParameter(dbCommand, "punteggioRelativo", DbType.Decimal, setNullDecimal(oRis.oStatistica.punteggioRelativo))
        db.AddInParameter(dbCommand, "punteggio", DbType.Decimal, setNullDecimal(oRis.oStatistica.punteggio))
        db.AddInParameter(dbCommand, "coeffDifficolta", DbType.Decimal, setNullDecimal(oRis.oStatistica.coeffDifficolta))
        RetVal = db.ExecuteNonQuery(dbCommand)
        oRis.id = db.GetParameterValue(dbCommand, "idRispostaQuestionario")
        Dim oRisOpzione As New RispostaDomanda
        For Each oRisOpzione In oRis.risposteDomande
            oRisOpzione.idRispostaQuestionario = oRis.id
            Select Case oRisOpzione.tipo
                Case Domanda.TipoDomanda.Multipla
                    RispostaOpzioneMultipla_Insert(oRisOpzione)
                Case Domanda.TipoDomanda.DropDown
                    RispostaDropDown_Insert(oRisOpzione)
                Case Domanda.TipoDomanda.Rating
                    RispostaRating_Insert(oRisOpzione)
                Case Domanda.TipoDomanda.RatingStars
                    RispostaRating_Insert(oRisOpzione)
                Case Domanda.TipoDomanda.Meeting
                    RispostaRating_Insert(oRisOpzione)
                Case Domanda.TipoDomanda.TestoLibero
                    RispostaTestoLibero_Insert(oRisOpzione)
                Case Domanda.TipoDomanda.Numerica
                    RispostaNumerica_Insert(oRisOpzione)
            End Select
        Next
        Return RetVal
    End Function
    Public Shared Function RispostaQuestionario_Update(ByVal CloseAnswer As Boolean,
                                                      ByRef oRis As RispostaQuestionario,
                                                      Optional ByRef updateRispDomanda As Boolean = True) As String

        If CloseAnswer Then
            oRis.dataFine = DateTime.Now()
        End If


        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""
        Dim sqlCommand As String = "sp_Questionario_RispostaQuestionario_Update"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idRispostaQuestionario", DbType.Int32, oRis.id)
        db.AddInParameter(dbCommand, "ultimaRisposta", DbType.Int32, oRis.ultimaRisposta)
        db.AddInParameter(dbCommand, "dataModifica", DbType.DateTime, setNullDate(oRis.dataModifica))
        db.AddInParameter(dbCommand, "dataFine", DbType.DateTime, setNullDate(oRis.dataFine))
        db.AddInParameter(dbCommand, "indirizzoIPEdit", DbType.String, oRis.indirizzoIPEdit)
        db.AddInParameter(dbCommand, "indirizzoIPEnd", DbType.String, oRis.indirizzoIPEnd)
        db.AddInParameter(dbCommand, "nTotali", DbType.Int32, setNullInt(oRis.oStatistica.nRisposteTotali))
        db.AddInParameter(dbCommand, "nSaltate", DbType.Int32, setNullInt(oRis.oStatistica.nRisposteSaltate))
        db.AddInParameter(dbCommand, "nCorrette", DbType.Int32, setNullInt(oRis.oStatistica.nRisposteCorrette))
        db.AddInParameter(dbCommand, "nNonValutate", DbType.Int32, setNullInt(oRis.oStatistica.nRisposteNonValutate))
        db.AddInParameter(dbCommand, "nErrate", DbType.Int32, setNullInt(oRis.oStatistica.nRisposteErrate))
        db.AddInParameter(dbCommand, "nParzialmenteCorrette", DbType.Int32, setNullInt(oRis.oStatistica.nRisposteParzialmenteCorrette))
        db.AddInParameter(dbCommand, "punteggioRelativo", DbType.Decimal, setNullDecimal(oRis.oStatistica.punteggioRelativo))
        db.AddInParameter(dbCommand, "punteggio", DbType.Decimal, setNullDecimal(oRis.oStatistica.punteggio))
        db.AddInParameter(dbCommand, "coeffDifficolta", DbType.Decimal, setNullDecimal(oRis.oStatistica.coeffDifficolta))

        RetVal = db.ExecuteNonQuery(dbCommand)
        If updateRispDomanda Then
            Dim oRisOpzione As New RispostaDomanda

            For Each oRisOpzione In oRis.risposteDomande

                oRisOpzione.idRispostaQuestionario = oRis.id

                Select Case oRisOpzione.tipo
                    Case Domanda.TipoDomanda.Multipla
                        RispostaOpzioneMultipla_Insert(oRisOpzione)
                    Case Domanda.TipoDomanda.DropDown
                        RispostaDropDown_Insert(oRisOpzione)
                    Case Domanda.TipoDomanda.Rating
                        RispostaRating_Insert(oRisOpzione)
                    Case Domanda.TipoDomanda.RatingStars
                        RispostaRating_Insert(oRisOpzione)
                    Case Domanda.TipoDomanda.Meeting
                        RispostaRating_Insert(oRisOpzione)
                    Case Domanda.TipoDomanda.TestoLibero
                        RispostaTestoLibero_Insert(oRisOpzione)
                    Case Domanda.TipoDomanda.Numerica
                        RispostaNumerica_Insert(oRisOpzione)
                End Select
            Next
        End If

        Return RetVal
    End Function

    Public Shared Function RispostaQuestionario_CheckUpdateUpdate(ByRef oRisId As Integer, ByRef oUsrId As Integer) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "sp_Questionario_RispostaQuestionario_CheckUpdate"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idRispostaQuestionario", DbType.Int32, oRisId)
        db.AddInParameter(dbCommand, "idPersona", DbType.Int32, oUsrId)

        Dim retval As Int32
        retval = db.ExecuteScalar(dbCommand)

        If (retval > 0) Then
            Return True
        End If

        Return False

    End Function
    Public Shared Function ValutazioneRispostaTestoLibero_Update(ByVal idRispostaTestLibero As Int32, ByVal valutazione As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""
        Dim sqlCommand As String = "sp_Questionario_ValutazioneRispostaTestoLibero_Update"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        db.AddInParameter(dbCommand, "idRispostaTestLibero", DbType.Int32, idRispostaTestLibero)
        db.AddInParameter(dbCommand, "Valutazione", DbType.String, valutazione.Trim)
        RetVal = db.ExecuteNonQuery(dbCommand)
        Return RetVal
    End Function
    Public Shared Function readRispostaByIDPersona(ByVal idQuestionario As Int32, ByVal idPersona As String, ByVal oConn As SqlConnection, Optional ByRef idQuestionarioRandom As Int32 = 0) As RispostaQuestionario
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim oRisposta As New RispostaQuestionario
        oRisposta.idQuestionario = idQuestionario
        oRisposta.idQuestionarioRandom = 0
        If idQuestionarioRandom = 0 Then
            Dim sqlCommand As String = "sp_Questionario_RispostaQuestionarioByIDPersona_Select"
            Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuestionario)
            db.AddInParameter(dbCommand, "idPersona", DbType.Int32, idPersona)
            Dim sqlReader As SqlDataReader
            sqlReader = db.ExecuteReader(dbCommand)
            While sqlReader.Read()
                oRisposta.id = isNullInt(sqlReader.Item("RSQS_Id"))
                oRisposta.idPersona = isNullInt(sqlReader.Item("RSQS_PRSN_Id"))
                oRisposta.indirizzoIPStart = isNullString(sqlReader.Item("RSQS_IndirizzoIPStart"))
                oRisposta.indirizzoIPEdit = isNullString(sqlReader.Item("RSQS_IndirizzoIPEdit"))
                oRisposta.indirizzoIPEnd = isNullString(sqlReader.Item("RSQS_IndirizzoIPEnd"))
                oRisposta.dataFine = isNullString(sqlReader.Item("RSQS_DataFine"))
                oRisposta.dataInizio = isNullString(sqlReader.Item("RSQS_DataInizio"))
                oRisposta.dataModifica = isNullString(sqlReader.Item("RSQS_DataModifica"))
                oRisposta.ultimaRisposta = isNullInt(sqlReader.Item("RSQS_UltimaRisposta"))

                oRisposta.oStatistica.nRisposteTotali = isNullInt(sqlReader.Item("RSQS_nTotali"))
                oRisposta.oStatistica.nRisposteSaltate = isNullInt(sqlReader.Item("RSQS_nSaltate"))
                oRisposta.oStatistica.nRisposteCorrette = isNullInt(sqlReader.Item("RSQS_nCorrette"))
                oRisposta.oStatistica.nRisposteNonValutate = isNullInt(sqlReader.Item("RSQS_nNonValutate"))
                oRisposta.oStatistica.nRisposteErrate = isNullInt(sqlReader.Item("RSQS_nErrate"))
                oRisposta.oStatistica.nRisposteParzialmenteCorrette = isNullInt(sqlReader.Item("RSQS_nParzialmenteCorrette"))
                oRisposta.oStatistica.punteggioRelativo = isNullDecimal(sqlReader.Item("RSQS_PunteggioRelativo"))
                oRisposta.oStatistica.coeffDifficolta = isNullDecimal(sqlReader.Item("RSQS_coeffDifficolta"))
                oRisposta.oStatistica.punteggio = isNullDecimal(sqlReader.Item("RSQS_Punteggio"))
            End While
        Else
            Dim sqlCommand As String = "sp_Questionario_RispostaQuestionarioRandomByIDPersona_Select"
            Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            db.AddInParameter(dbCommand, "idQuestionarioRandom", DbType.Int32, idQuestionarioRandom)
            db.AddInParameter(dbCommand, "idPersona", DbType.Int32, idPersona)

            Dim sqlReader As SqlDataReader
            sqlReader = db.ExecuteReader(dbCommand)

            While sqlReader.Read()
                oRisposta.id = isNullInt(sqlReader.Item("RSQS_Id"))
                oRisposta.idPersona = isNullInt(sqlReader.Item("RSQS_PRSN_Id"))
                oRisposta.indirizzoIPStart = isNullString(sqlReader.Item("RSQS_IndirizzoIPStart"))
                oRisposta.indirizzoIPEdit = isNullString(sqlReader.Item("RSQS_IndirizzoIPEdit"))
                oRisposta.indirizzoIPEnd = isNullString(sqlReader.Item("RSQS_IndirizzoIPEnd"))
                oRisposta.dataFine = isNullString(sqlReader.Item("RSQS_DataFine"))
                oRisposta.dataInizio = isNullString(sqlReader.Item("RSQS_DataInizio"))
                oRisposta.dataModifica = isNullString(sqlReader.Item("RSQS_DataModifica"))
                oRisposta.ultimaRisposta = isNullInt(sqlReader.Item("RSQS_UltimaRisposta"))

                oRisposta.oStatistica.nRisposteTotali = isNullInt(sqlReader.Item("RSQS_nTotali"))
                oRisposta.oStatistica.nRisposteSaltate = isNullInt(sqlReader.Item("RSQS_nSaltate"))
                oRisposta.oStatistica.nRisposteCorrette = isNullInt(sqlReader.Item("RSQS_nCorrette"))
                oRisposta.oStatistica.nRisposteNonValutate = isNullInt(sqlReader.Item("RSQS_nNonValutate"))
                oRisposta.oStatistica.nRisposteErrate = isNullInt(sqlReader.Item("RSQS_nErrate"))
                oRisposta.oStatistica.nRisposteParzialmenteCorrette = isNullInt(sqlReader.Item("RSQS_nParzialmenteCorrette"))
                oRisposta.oStatistica.punteggioRelativo = isNullDecimal(sqlReader.Item("RSQS_PunteggioRelativo"))
                oRisposta.oStatistica.punteggio = isNullDecimal(sqlReader.Item("RSQS_Punteggio"))
            End While

        End If


        'oDomanda = readDomandaMultiplaOpzioni(oDomanda, oconn)

        Return oRisposta
    End Function
    Public Shared Function countRisposteBYIDQuestionario(ByVal idQuestionario As Integer) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "sp_Questionario_RispostaQuestionarioByIdQuestionario_count"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuestionario)

        Dim retval As Int32
        retval = db.ExecuteScalar(dbCommand)
        Return retval
    End Function
    Public Shared Function cancellaRisposteBYIDQuestionario(ByVal idQuestionario As Integer, ByVal idPersona As Integer) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "sp_Questionario_RisposteByIDQuestionario_Delete"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        db.AddInParameter(dbCommand, "@idQuestionario", DbType.Int32, idQuestionario)
        db.AddInParameter(dbCommand, "idPersona", DbType.Int32, idPersona)
        db.AddInParameter(dbCommand, "dataCancellazione", DbType.DateTime, Date.Now)

        Dim retval As Int32
        db.ExecuteScalar(dbCommand)
        Return retval
    End Function
    'Public Shared Function cancellaRispostaBYIDPersona(ByVal idQuestionario As Integer, ByVal idPersona As Integer, ByVal idUtenteInvitato As Integer) As Integer
    '    Dim db As Database = DatabaseFactory.CreateDatabase()
    '    Dim sqlCommand As String = "sp_Questionario_RispostaByIDPersona_Delete"
    '    Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
    '    db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuestionario)
    '    db.AddInParameter(dbCommand, "idPersona", DbType.Int32, idPersona)
    '    db.AddInParameter(dbCommand, "idUtenteInvitato", DbType.Int32, idUtenteInvitato)

    '    Dim retval As Int32
    '    db.ExecuteScalar(dbCommand)
    '    Return retval
    'End Function
    Public Shared Function cancellaRispostaBYID(ByVal idRisposta As Integer) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "sp_Questionario_RispostaByID_Delete"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        db.AddInParameter(dbCommand, "idRisposta", DbType.Int32, idRisposta)
        Dim retval As Int32
        db.ExecuteScalar(dbCommand)
        Return retval
    End Function

  
    '/ FOR SERVICE
    Public Shared Function cancellaRispostaQuestionarioRandomBYID(ByVal idRisposta As Integer, ByVal idQuestionarioPadre As Integer) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim retval As Int32

        Using connection As DbConnection = db.CreateConnection()
            Dim sqlCommand As String = "sp_Questionario_RispostaQuestionarioRandomByID_Delete"
            Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            dbCommand.Connection = connection
            db.AddInParameter(dbCommand, "idRisposta", DbType.Int32, idRisposta)
            db.AddInParameter(dbCommand, "idQuestionarioPadre", DbType.Int32, idQuestionarioPadre)

            db.ExecuteScalar(dbCommand)
        End Using
        '  Dim connection As DbConnection = db.CreateConnection()

      
        Return retval
    End Function

    Public Shared Function cancellaRispostaQuestionarioRandomBYID(ByVal idRisposta As Integer, ByVal idQuestionarioPadre As Integer, ByRef db As Database, ByRef conn As DbConnection) As Integer
        db = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "sp_Questionario_RispostaQuestionarioRandomByID_Delete"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        dbCommand.Connection = conn
        db.AddInParameter(dbCommand, "idRisposta", DbType.Int32, idRisposta)
        db.AddInParameter(dbCommand, "idQuestionarioPadre", DbType.Int32, idQuestionarioPadre)
        Dim retval As Int32
        db.ExecuteScalar(dbCommand)
        Return retval
    End Function
    Public Shared Function cancellaRisposteBYIDQuestionarioRandom(appContext As iApplicationContext, ByVal idQuest As Integer) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim connection As DbConnection = db.CreateConnection()
        Dim oQuest As New Questionario

        oQuest = DALQuestionario.readQuestionarioBYLingua(appContext, idQuest, 0, True)
        For Each oRis As RispostaQuestionario In oQuest.risposteQuestionario
            DALRisposte.cancellaRispostaQuestionarioRandomBYID(oRis.id, idQuest, db, connection)
        Next
    End Function
    Public Shared Function readRisposteBYIDQuestionario(ByVal idQuestionario As String, ByVal idPerson As Integer, ByVal idInvitedUser As Integer, Optional ByRef caricaNome As Boolean = False) As List(Of RispostaQuestionario)
        Return readRisposteBYIDQuestionario(idQuestionario, idPerson, idInvitedUser, Nothing, Nothing, caricaNome)
    End Function
    Public Shared Function readRisposteBYIDQuestionario(ByVal idQuestionario As String, ByVal idPerson As Integer, ByVal idInvitedUser As Integer, ByRef db As Database, ByRef conn As DbConnection, Optional ByRef caricaNome As Boolean = False) As List(Of RispostaQuestionario)
        If db Is Nothing Then
            db = DatabaseFactory.CreateDatabase()
        End If
        If conn Is Nothing Then
            conn = db.CreateConnection()
        End If
        Dim sqlCommand As String = ""


        If caricaNome Then
            sqlCommand = "sp_Questionario_RisposteQuestionarioWithNome_Select"
        Else
            sqlCommand = "sp_Questionario_RisposteQuestionario_Select"
        End If
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        dbCommand.Connection = conn
        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuestionario)
        db.AddInParameter(dbCommand, "idPerson", DbType.Int32, idPerson)
        db.AddInParameter(dbCommand, "idInvitedUser", DbType.Int32, idInvitedUser)

        Dim oRisposte As New List(Of RispostaQuestionario)
        Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
            While sqlReader.Read()
                Dim oRisposta As New RispostaQuestionario
                oRisposta.id = isNullInt(sqlReader.Item("RSQS_Id"))
                oRisposta.idQuestionario = isNullInt(sqlReader.Item("RSQS_QSTN_Id"))
                oRisposta.idPersona = isNullInt(sqlReader.Item("RSQS_PRSN_Id"))
                oRisposta.idUtenteInvitato = isNullInt(sqlReader.Item("RSQS_QSUI_Id"))
                oRisposta.indirizzoIPStart = isNullString(sqlReader.Item("RSQS_IndirizzoIPStart"))
                oRisposta.indirizzoIPEdit = isNullString(sqlReader.Item("RSQS_IndirizzoIPEdit"))
                oRisposta.indirizzoIPEnd = isNullString(sqlReader.Item("RSQS_IndirizzoIPEnd"))
                oRisposta.dataFine = isNullString(sqlReader.Item("RSQS_DataFine"))
                oRisposta.dataInizio = isNullString(sqlReader.Item("RSQS_DataInizio"))
                oRisposta.dataModifica = isNullString(sqlReader.Item("RSQS_DataModifica"))
                oRisposta.ultimaRisposta = isNullInt(sqlReader.Item("RSQS_UltimaRisposta"))
                oRisposta.oStatistica.nRisposteTotali = isNullInt(sqlReader.Item("RSQS_nTotali"))
                oRisposta.oStatistica.nRisposteSaltate = isNullInt(sqlReader.Item("RSQS_nSaltate"))
                oRisposta.oStatistica.nRisposteCorrette = isNullInt(sqlReader.Item("RSQS_nCorrette"))
                oRisposta.oStatistica.nRisposteNonValutate = isNullInt(sqlReader.Item("RSQS_nNonValutate"))
                oRisposta.oStatistica.nRisposteErrate = isNullInt(sqlReader.Item("RSQS_nErrate"))
                oRisposta.oStatistica.nRisposteParzialmenteCorrette = isNullInt(sqlReader.Item("RSQS_nParzialmenteCorrette"))
                oRisposta.oStatistica.punteggioRelativo = isNullDecimal(sqlReader.Item("RSQS_PunteggioRelativo"))
                oRisposta.oStatistica.coeffDifficolta = isNullDecimal(sqlReader.Item("RSQS_coeffDifficolta"))
                oRisposta.oStatistica.punteggio = isNullDecimal(sqlReader.Item("RSQS_Punteggio"))

                If caricaNome Then
                    'If oRisposta.idUtenteInvitato = 0 Then
                    '    oRisposta.oStatistica.nomeUtente = isNullString(sqlReader.Item("PRSN_cognome") & " " & isNullString(sqlReader.Item("PRSN_nome")))
                    'Else
                    '    oRisposta.oStatistica.nomeUtente = isNullString(sqlReader.Item("QSUI_Cognome") & " " & isNullString(sqlReader.Item("QSUI_Nome")))
                    'End If
                    oRisposta.oStatistica.nomeUtente = isNullString(sqlReader.Item("Cognome") & " " & isNullString(sqlReader.Item("Nome")))
                End If
                oRisposte.Add(oRisposta)
            End While
            sqlReader.Close()
        End Using
        Return oRisposte
    End Function

  
    Public Shared Function readRisposteIdByIdQuestionario(ByVal idQuestionario As String) As List(Of Integer)
        Return readRisposteIdByIdQuestionario(idQuestionario, Nothing, Nothing)
    End Function
    Public Shared Function readRisposteIdByIdQuestionario(ByVal idQuestionario As String, ByRef db As Database, ByRef conn As DbConnection) As List(Of Integer)
        If db Is Nothing Then
            db = DatabaseFactory.CreateDatabase()
        End If
        If conn Is Nothing Then
            conn = db.CreateConnection()
        End If
        Dim sqlCommand As String = "sp_Questionario_idRisposteQuestionario_Select"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        dbCommand.Connection = conn
        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuestionario)

        Dim oRisposteIdList As New List(Of Integer)

        Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
            While sqlReader.Read()
                oRisposteIdList.Add(isNullInt(sqlReader.Item("RSQS_Id")))
            End While
            sqlReader.Close()
        End Using
        Return oRisposteIdList
    End Function
    Public Shared Function readRisposteAutovalutazione(ByVal idQuestionario As String, ByRef idPersona As Integer) As List(Of RispostaQuestionario)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim conn As DbConnection = db.CreateConnection()
        Dim sqlCommand As String = "sp_Questionario_RisposteAutovalutazione_Select"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        dbCommand.Connection = conn
        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuestionario)
        db.AddInParameter(dbCommand, "idPersona", DbType.Int32, idPersona)

        Dim oRisposte As New List(Of RispostaQuestionario)
        Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
            While sqlReader.Read()
                Dim oRisposta As New RispostaQuestionario
                oRisposta.id = isNullInt(sqlReader.Item("RSQS_Id"))
                oRisposta.idQuestionario = idQuestionario
                oRisposta.idQuestionarioRandom = isNullInt(sqlReader.Item("RSQS_QSRD_Id"))
                oRisposta.idPersona = idPersona
                oRisposta.dataInizio = isNullString(sqlReader.Item("RSQS_DataInizio"))
                oRisposta.dataModifica = isNullString(sqlReader.Item("RSQS_DataModifica"))
                oRisposta.dataFine = isNullString(sqlReader.Item("RSQS_DataFine"))
                oRisposta.indirizzoIPStart = isNullString(sqlReader.Item("RSQS_IndirizzoIPStart"))
                oRisposta.indirizzoIPEdit = isNullString(sqlReader.Item("RSQS_IndirizzoIPEdit"))
                oRisposta.indirizzoIPEnd = isNullString(sqlReader.Item("RSQS_IndirizzoIPEnd"))

                oRisposta.oStatistica.nRisposteTotali = isNullInt(sqlReader.Item("RSQS_nTotali"))
                oRisposta.oStatistica.nRisposteSaltate = isNullInt(sqlReader.Item("RSQS_nSaltate"))
                oRisposta.oStatistica.nRisposteCorrette = isNullInt(sqlReader.Item("RSQS_nCorrette"))
                oRisposta.oStatistica.nRisposteNonValutate = isNullInt(sqlReader.Item("RSQS_nNonValutate"))
                oRisposta.oStatistica.nRisposteErrate = isNullInt(sqlReader.Item("RSQS_nErrate"))
                oRisposta.oStatistica.nRisposteParzialmenteCorrette = isNullInt(sqlReader.Item("RSQS_nParzialmenteCorrette"))
                oRisposta.oStatistica.punteggioRelativo = isNullDecimal(sqlReader.Item("RSQS_PunteggioRelativo"))
                oRisposta.oStatistica.coeffDifficolta = isNullDecimal(sqlReader.Item("RSQS_coeffDifficolta"))
                oRisposta.oStatistica.punteggio = isNullDecimal(sqlReader.Item("RSQS_Punteggio"))
                oRisposte.Add(oRisposta)
            End While
            sqlReader.Close()
        End Using
        Return oRisposte
    End Function
    Public Shared Function readRispostaBYIDPersona(ByVal idQuestionario As Integer, ByVal idPersona As Integer, ByVal idUtenteInvitato As Integer, ByVal db As Database, ByVal conn As DbConnection, Optional ByRef idQuestionarioRandom As Int32 = 0) As RispostaQuestionario

        Dim sqlCommand As String
        Dim oRisposta As New RispostaQuestionario
        Dim dbCommand As DbCommand

        oRisposta.idQuestionario = idQuestionario
        oRisposta.idPersona = idPersona
        oRisposta.idUtenteInvitato = idUtenteInvitato

        If Not (idUtenteInvitato = 0 And idPersona = 0) Then
            If idQuestionarioRandom = 0 Then
                If Not idUtenteInvitato = 0 Then
                    sqlCommand = "sp_Questionario_RispostaQuestionarioByIDUtenteInvitato_Select"
                    dbCommand = db.GetStoredProcCommand(sqlCommand)
                    dbCommand.Connection = conn
                    db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuestionario)
                    db.AddInParameter(dbCommand, "idUtenteInvitato", DbType.Int32, idUtenteInvitato)
                ElseIf Not idPersona = 0 Then
                    sqlCommand = "sp_Questionario_RispostaQuestionarioByIDPersona_Select"
                    dbCommand = db.GetStoredProcCommand(sqlCommand)
                    dbCommand.Connection = conn
                    db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuestionario)
                    db.AddInParameter(dbCommand, "idPersona", DbType.Int32, idPersona)
                Else
                    dbCommand = db.GetStoredProcCommand("")
                End If
            Else
                sqlCommand = "sp_Questionario_RispostaQuestionarioRandom_Select"
                dbCommand = db.GetStoredProcCommand(sqlCommand)
                dbCommand.Connection = conn
                db.AddInParameter(dbCommand, "idQuestionarioRandom", DbType.Int32, idQuestionarioRandom)
            End If
            Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                While sqlReader.Read()
                    oRisposta.id = isNullInt(sqlReader.Item("RSQS_Id"))
                    oRisposta.indirizzoIPStart = isNullString(sqlReader.Item("RSQS_IndirizzoIPStart"))
                    oRisposta.indirizzoIPEdit = isNullString(sqlReader.Item("RSQS_IndirizzoIPEdit"))
                    oRisposta.indirizzoIPEnd = isNullString(sqlReader.Item("RSQS_IndirizzoIPEnd"))
                    oRisposta.dataFine = isNullString(sqlReader.Item("RSQS_DataFine"))
                    oRisposta.dataInizio = isNullString(sqlReader.Item("RSQS_DataInizio"))
                    oRisposta.dataModifica = isNullString(sqlReader.Item("RSQS_DataModifica"))
                    oRisposta.ultimaRisposta = isNullInt(sqlReader.Item("RSQS_UltimaRisposta"))

                    oRisposta.oStatistica.nRisposteTotali = isNullInt(sqlReader.Item("RSQS_nTotali"))
                    oRisposta.oStatistica.nRisposteSaltate = isNullInt(sqlReader.Item("RSQS_nSaltate"))
                    oRisposta.oStatistica.nRisposteCorrette = isNullInt(sqlReader.Item("RSQS_nCorrette"))
                    oRisposta.oStatistica.nRisposteNonValutate = isNullInt(sqlReader.Item("RSQS_nNonValutate"))
                    oRisposta.oStatistica.nRisposteErrate = isNullInt(sqlReader.Item("RSQS_nErrate"))
                    oRisposta.oStatistica.nRisposteParzialmenteCorrette = isNullInt(sqlReader.Item("RSQS_nParzialmenteCorrette"))
                    oRisposta.oStatistica.punteggioRelativo = isNullDecimal(sqlReader.Item("RSQS_PunteggioRelativo"))
                    oRisposta.oStatistica.coeffDifficolta = isNullDecimal(sqlReader.Item("RSQS_coeffDifficolta"))
                    oRisposta.oStatistica.punteggio = isNullDecimal(sqlReader.Item("RSQS_Punteggio"))
                End While
                sqlReader.Close()
            End Using
        End If


        Return oRisposta
    End Function
    Public Shared Function readRispostaBYIDRispostaQuestionario(ByVal idRisposta As Integer, Optional ByVal db As Database = Nothing, Optional ByVal conn As DbConnection = Nothing) As RispostaQuestionario
        If db Is Nothing Then
            db = DatabaseFactory.CreateDatabase()
        End If
        If conn Is Nothing Then
            conn = db.CreateConnection()
        End If
        Dim sqlCommand As String
        Dim oRisposta As New RispostaQuestionario
        Dim dbCommand As DbCommand
        sqlCommand = "sp_Questionario_RispostaQuestionarioByID_Select"
        dbCommand = db.GetStoredProcCommand(sqlCommand)
        dbCommand.Connection = conn
        dbCommand.CommandTimeout = 1200
        db.AddInParameter(dbCommand, "idRisposta", DbType.Int32, idRisposta)
        Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
            While sqlReader.Read()
                oRisposta.id = isNullInt(sqlReader.Item("RSQS_Id"))
                oRisposta.idQuestionario = isNullInt(sqlReader.Item("RSQS_QSTN_Id"))
                oRisposta.idQuestionarioRandom = isNullInt(sqlReader.Item("RSQS_QSRD_Id"))
                oRisposta.idPersona = isNullInt(sqlReader.Item("RSQS_PRSN_Id"))
                oRisposta.idUtenteInvitato = isNullInt(sqlReader.Item("RSQS_QSUI_Id"))
                oRisposta.indirizzoIPStart = isNullString(sqlReader.Item("RSQS_IndirizzoIPStart"))
                oRisposta.indirizzoIPEdit = isNullString(sqlReader.Item("RSQS_IndirizzoIPEdit"))
                oRisposta.indirizzoIPEnd = isNullString(sqlReader.Item("RSQS_IndirizzoIPEnd"))
                oRisposta.dataFine = isNullString(sqlReader.Item("RSQS_DataFine"))
                oRisposta.dataInizio = isNullString(sqlReader.Item("RSQS_DataInizio"))
                oRisposta.dataModifica = isNullString(sqlReader.Item("RSQS_DataModifica"))
                oRisposta.ultimaRisposta = isNullInt(sqlReader.Item("RSQS_UltimaRisposta"))
                oRisposta.oStatistica.nRisposteTotali = isNullInt(sqlReader.Item("RSQS_nTotali"))
                oRisposta.oStatistica.nRisposteSaltate = isNullInt(sqlReader.Item("RSQS_nSaltate"))
                oRisposta.oStatistica.nRisposteCorrette = isNullInt(sqlReader.Item("RSQS_nCorrette"))
                oRisposta.oStatistica.nRisposteNonValutate = isNullInt(sqlReader.Item("RSQS_nNonValutate"))
                oRisposta.oStatistica.nRisposteErrate = isNullInt(sqlReader.Item("RSQS_nErrate"))
                oRisposta.oStatistica.nRisposteParzialmenteCorrette = isNullInt(sqlReader.Item("RSQS_nParzialmenteCorrette"))
                oRisposta.oStatistica.coeffDifficolta = isNullDecimal(sqlReader.Item("RSQS_coeffDifficolta"))
                oRisposta.oStatistica.punteggioRelativo = isNullDecimal(sqlReader.Item("RSQS_PunteggioRelativo"))
                oRisposta.oStatistica.punteggio = isNullDecimal(sqlReader.Item("RSQS_Punteggio"))
            End While
            sqlReader.Close()
        End Using
        Return oRisposta
    End Function
    'Public Shared Function readIDQuestionarioBYIDRispostaQuestionario(ByVal idRispostaQuestionario As String) As Integer

    '    Dim db As Database = DatabaseFactory.CreateDatabase()
    '    Dim retVal As Integer

    '    Dim sqlCommand As String = "sp_Questionario_IDQuestionarioBYIDRisposta_Select"
    '    Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
    '    db.AddInParameter(dbCommand, "idRispostaQuestionario", DbType.Int32, idRispostaQuestionario)

    '    retVal = db.ExecuteScalar(dbCommand)
    '    Return retVal

    'End Function
    Public Shared Function readIDQuestionarioRandomBYIDRisposta(ByVal idRispostaQuestionario As String) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim retVal As Integer

        Dim sqlCommand As String = "sp_Questionario_IdQuestionarioRandomBYIdRisposta_Select"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        db.AddInParameter(dbCommand, "idRispostaQuestionario", DbType.Int32, idRispostaQuestionario)

        retVal = db.ExecuteScalar(dbCommand)
        Return retVal

    End Function
#End Region
#Region "Risposta Opzione Multipla"

    Public Shared Function readRispostaOpzioneMultipla(ByVal idRispostaQuest As String, ByVal idDomanda As String, ByVal db As Database, ByVal conn As DbConnection) As List(Of RispostaDomanda)

        Dim sqlCommand As String = "sp_Questionario_RispostaOpzioneMultipla_Select"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        dbCommand.Connection = conn
        db.AddInParameter(dbCommand, "idDomanda", DbType.Int32, idDomanda)
        db.AddInParameter(dbCommand, "idRispostaQuestionario", DbType.Int32, idRispostaQuest)
        Dim oRisposteDomanda As New List(Of RispostaDomanda)

        Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
            While sqlReader.Read()
                Dim oRisposta As New RispostaDomanda
                oRisposta.id = isNullInt(sqlReader.Item("RSOM_Id"))
                oRisposta.idDomandaOpzione = isNullInt(sqlReader.Item("RSOM_DMMO_Id"))
                oRisposta.idRispostaQuestionario = isNullInt(sqlReader.Item("RSOM_RSQS_Id"))
                oRisposta.testoOpzione = isNullString(sqlReader.Item("RSOM_TestoIsAltro"))
                oRisposta.numeroOpzione = isNullInt(sqlReader.Item("DMMO_NumeroOpzione"))
                oRisposta.idDomanda = isNullInt(sqlReader.Item("DMML_DMND_Id"))
                oRisposta.tipo = Domanda.TipoDomanda.Multipla
                oRisposteDomanda.Add(oRisposta)
            End While
            sqlReader.Close()
        End Using

        Return oRisposteDomanda
    End Function

    Public Shared Function RispostaOpzioneMultipla_Insert(ByRef oRisOpzione As RispostaDomanda) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""
        Dim sqlCommand As String = "sp_Questionario_RispostaOpzioneMultipla_Insert"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idDomanda", DbType.Int32, oRisOpzione.idDomandaOpzione)
        db.AddInParameter(dbCommand, "idRispostaQuestionario", DbType.Int32, oRisOpzione.idRispostaQuestionario)
        'db.AddInParameter(dbCommand, "numeroOpzione", DbType.Int32, oRisOpzione.numeroOpzione)
        db.AddInParameter(dbCommand, "testoOpzione", DbType.String, oRisOpzione.testoOpzione)

        RetVal = db.ExecuteNonQuery(dbCommand)

        Return RetVal
    End Function

    Public Shared Function RispostaOpzioneMultipla_Delete(ByVal idRS As String) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""
        Dim sqlCommand As String = "sp_Questionario_RispostaOpzioneMultipla_Delete"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idRispostaQuestionario", DbType.Int32, idRS)

        RetVal = db.ExecuteNonQuery(dbCommand)

        Return RetVal
    End Function

#End Region
#Region "Risposta DropDown"

    Public Shared Function readRispostaDropDown(ByVal idRispostaQuest As String, ByVal idDomanda As String, ByVal db As Database, ByVal conn As DbConnection) As List(Of RispostaDomanda)

        Dim sqlCommand As String = "sp_Questionario_RispostaDropDown_Select"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        dbCommand.Connection = conn
        db.AddInParameter(dbCommand, "idRispostaQuestionario", DbType.Int32, idRispostaQuest)
        db.AddInParameter(dbCommand, "idDomanda", DbType.Int32, idDomanda)
        Dim oRisposteDomanda As New List(Of RispostaDomanda)
        Dim oRisposta As New RispostaDomanda

        Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
            While sqlReader.Read()
                oRisposta.id = isNullInt(sqlReader.Item("RSDR_Id"))
                oRisposta.idDomanda = isNullInt(sqlReader.Item("DMML_DMND_Id"))
                oRisposta.idDomandaOpzione = isNullInt(sqlReader.Item("RSDR_DMDR_Id"))
                oRisposta.idRispostaQuestionario = isNullInt(sqlReader.Item("RSDR_RSQS_Id"))
                oRisposta.testoOpzione = isNullString(sqlReader.Item("RSDR_Testo"))
                oRisposta.numeroOpzione = isNullInt(sqlReader.Item("RSDR_NumeroOpzione"))
                oRisposta.tipo = Domanda.TipoDomanda.DropDown
                oRisposteDomanda.Add(oRisposta)
            End While
            sqlReader.Close()
        End Using


        Return oRisposteDomanda
    End Function

    Public Shared Function RispostaDropDown_Delete(ByVal idRS As String) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""
        Dim sqlCommand As String = "sp_Questionario_RispostaDropDown_Delete"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idRispostaQuestionario", DbType.Int32, idRS)

        RetVal = db.ExecuteNonQuery(dbCommand)

        Return RetVal
    End Function

    Public Shared Function RispostaDropDown_Insert(ByRef oRisOpzione As RispostaDomanda) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""
        Dim sqlCommand As String = "sp_Questionario_RispostaDropDown_Insert"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idDomandaDropDown", DbType.Int32, oRisOpzione.idDomandaOpzione)
        db.AddInParameter(dbCommand, "idRispostaQuestionario", DbType.Int32, oRisOpzione.idRispostaQuestionario)
        db.AddInParameter(dbCommand, "testo", DbType.String, oRisOpzione.testoOpzione)
        db.AddInParameter(dbCommand, "numeroOpzione", DbType.Int32, oRisOpzione.numeroOpzione)

        RetVal = db.ExecuteNonQuery(dbCommand)

        Return RetVal
    End Function

#End Region
#Region "Risposta Rating"

    Public Shared Function readRispostaRating(ByVal idRispostaQuest As String, ByVal idDomanda As String, ByVal db As Database, ByVal conn As DbConnection) As List(Of RispostaDomanda)

        Dim sqlCommand As String = "sp_Questionario_RispostaRating_Select"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        dbCommand.Connection = conn
        db.AddInParameter(dbCommand, "idDomanda", DbType.Int32, idDomanda)
        db.AddInParameter(dbCommand, "idRispostaQuestionario", DbType.Int32, idRispostaQuest)
        Dim oRisposteDomanda As New List(Of RispostaDomanda)

        Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
            While sqlReader.Read()
                Dim oRisposta As New RispostaDomanda
                oRisposta.id = isNullInt(sqlReader.Item("RSRT_Id"))
                oRisposta.idDomanda = isNullInt(sqlReader.Item("DMML_DMND_Id"))
                oRisposta.idDomandaOpzione = isNullInt(sqlReader.Item("RSRT_DMRO_Id"))
                oRisposta.idRispostaQuestionario = isNullInt(sqlReader.Item("RSRT_RSQS_Id"))
                oRisposta.valore = isNullInt(sqlReader.Item("RSRT_Valore"))
                oRisposta.numeroOpzione = isNullInt(sqlReader.Item("DMRO_NumeroOpzione"))
                oRisposta.testoOpzione = isNullString(sqlReader.Item("RSRT_TestoIsAltro"))
                oRisposta.tipo = Domanda.TipoDomanda.Rating
                oRisposteDomanda.Add(oRisposta)
            End While
            sqlReader.Close()
        End Using

        Return oRisposteDomanda
    End Function
    Public Shared Function readRispostaRatingByIdDomanda(ByVal idDomanda As Int32) As List(Of RispostaDomanda)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "sp_Questionario_RispostaRatingByIdDomanda_Select"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        db.AddInParameter(dbCommand, "idDomanda", DbType.Int32, idDomanda)
        Dim oRisposteDomanda As New List(Of RispostaDomanda)

        Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
            While sqlReader.Read()
                Dim oRisposta As New RispostaDomanda
                oRisposta.id = isNullInt(sqlReader.Item("RSRT_Id"))
                oRisposta.idDomanda = isNullInt(sqlReader.Item("DMML_DMND_Id"))
                oRisposta.idDomandaOpzione = isNullInt(sqlReader.Item("RSRT_DMRO_Id"))
                oRisposta.idRispostaQuestionario = isNullInt(sqlReader.Item("RSRT_RSQS_Id"))
                oRisposta.valore = isNullInt(sqlReader.Item("RSRT_Valore"))
                oRisposta.numeroOpzione = isNullInt(sqlReader.Item("DMRO_NumeroOpzione"))
                oRisposta.testoOpzione = isNullString(sqlReader.Item("RSRT_TestoIsAltro"))
                oRisposta.tipo = Domanda.TipoDomanda.Rating
                oRisposteDomanda.Add(oRisposta)
            End While
            sqlReader.Close()
        End Using

        Return oRisposteDomanda
    End Function

    Public Shared Function readRispostaRatingStars(ByVal idRispostaQuest As String, ByVal idDomanda As String, ByVal db As Database, ByVal conn As DbConnection) As List(Of RispostaDomanda)

        Dim sqlCommand As String = "sp_Questionario_RispostaRating_Select"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        dbCommand.Connection = conn
        db.AddInParameter(dbCommand, "idDomanda", DbType.Int32, idDomanda)
        db.AddInParameter(dbCommand, "idRispostaQuestionario", DbType.Int32, idRispostaQuest)
        Dim oRisposteDomanda As New List(Of RispostaDomanda)

        Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
            While sqlReader.Read()
                Dim oRisposta As New RispostaDomanda
                oRisposta.id = isNullInt(sqlReader.Item("RSRT_Id"))
                oRisposta.idDomanda = isNullInt(sqlReader.Item("DMML_DMND_Id"))
                oRisposta.idDomandaOpzione = isNullInt(sqlReader.Item("RSRT_DMRO_Id"))
                oRisposta.idRispostaQuestionario = isNullInt(sqlReader.Item("RSRT_RSQS_Id"))
                oRisposta.valore = isNullInt(sqlReader.Item("RSRT_Valore"))
                oRisposta.numeroOpzione = isNullInt(sqlReader.Item("DMRO_NumeroOpzione"))
                oRisposta.testoOpzione = isNullString(sqlReader.Item("RSRT_TestoIsAltro"))
                oRisposta.tipo = Domanda.TipoDomanda.RatingStars
                oRisposteDomanda.Add(oRisposta)
            End While
            sqlReader.Close()
        End Using

        Return oRisposteDomanda
    End Function

    Public Shared Function RispostaRating_Insert(ByRef oRisOpzione As RispostaDomanda) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""
        Dim sqlCommand As String = "sp_Questionario_RispostaRating_Insert"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idDomanda", DbType.Int32, oRisOpzione.idDomandaOpzione)
        db.AddInParameter(dbCommand, "idRispostaQuestionario", DbType.Int32, oRisOpzione.idRispostaQuestionario)
        db.AddInParameter(dbCommand, "valore", DbType.Int32, oRisOpzione.valore)
        db.AddInParameter(dbCommand, "TestoIsAltro", DbType.String, oRisOpzione.testoOpzione)

        RetVal = db.ExecuteNonQuery(dbCommand)

        Return RetVal
    End Function

    Public Shared Function RispostaRating_Delete(ByVal idRS As String) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""
        Dim sqlCommand As String = "sp_Questionario_RispostaRating_Delete"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idRispostaQuestionario", DbType.Int32, idRS)

        RetVal = db.ExecuteNonQuery(dbCommand)

        Return RetVal
    End Function


#End Region
#Region "Risposta Testo Libero"
    Public Shared Function RispostaTestoLibero_Insert(ByRef oRisOpzione As RispostaDomanda) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""
        Dim sqlCommand As String = "sp_Questionario_RispostaTestoLibero_Insert"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idDomandaTL", DbType.Int32, oRisOpzione.idDomandaOpzione)
        db.AddInParameter(dbCommand, "idRispostaQuestionario", DbType.Int32, oRisOpzione.idRispostaQuestionario)
        db.AddInParameter(dbCommand, "testo", DbType.String, oRisOpzione.testoOpzione)

        RetVal = db.ExecuteNonQuery(dbCommand)

        Return RetVal
    End Function

    Public Shared Function RispostaTestoLibero_Delete(ByVal idRS As String) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""
        Dim sqlCommand As String = "sp_Questionario_RispostaTestoLibero_Delete"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idRispostaQuestionario", DbType.Int32, idRS)

        RetVal = db.ExecuteNonQuery(dbCommand)

        Return RetVal
    End Function

    Public Shared Function readRispostaTestoLibero(ByVal idRispostaQuest As String, ByVal idDomanda As String, ByVal db As Database, ByVal conn As DbConnection) As List(Of RispostaDomanda)

        Dim sqlCommand As String = "sp_Questionario_RispostaTestoLibero_Select"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        dbCommand.Connection = conn
        db.AddInParameter(dbCommand, "idDomanda", DbType.Int32, idDomanda)
        db.AddInParameter(dbCommand, "idRispostaQuestionario", DbType.Int32, idRispostaQuest)
        Dim listRisposte As New List(Of RispostaDomanda)

        Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
            While sqlReader.Read()
                Dim oRisposta As New RispostaDomanda
                oRisposta.id = isNullInt(sqlReader.Item("RSTL_Id"))
                oRisposta.idDomanda = isNullInt(sqlReader.Item("DMML_DMND_Id"))
                oRisposta.idDomandaOpzione = isNullInt(sqlReader.Item("RSTL_DMTL_Id"))
                oRisposta.idRispostaQuestionario = isNullInt(sqlReader.Item("RSTL_RSQS_Id"))
                oRisposta.testoOpzione = isNullString(sqlReader.Item("RSTL_Testo"))
                oRisposta.numeroOpzione = isNullInt(sqlReader.Item("DMTL_Numero"))
                oRisposta.valutazione = isNullString(sqlReader.Item("RSTL_Valutazione"))
                oRisposta.tipo = Domanda.TipoDomanda.TestoLibero
                listRisposte.Add(oRisposta)
            End While
            sqlReader.Close()
        End Using

        Return listRisposte
    End Function
#End Region
#Region "Risposta Numerica"

    Public Shared Function readRispostaNumerica(ByVal idRispostaQuest As String, ByVal idDomanda As String, ByVal db As Database, ByVal conn As DbConnection) As List(Of RispostaDomanda)

        Dim sqlCommand As String = "sp_Questionario_RispostaNumerica_Select"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        dbCommand.Connection = conn
        db.AddInParameter(dbCommand, "idDomanda", DbType.Int32, idDomanda)
        db.AddInParameter(dbCommand, "idRispostaQuestionario", DbType.Int32, idRispostaQuest)
        Dim listRisposte As New List(Of RispostaDomanda)

        Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
            While sqlReader.Read()
                Dim oRisposta As New RispostaDomanda
                oRisposta.id = isNullInt(sqlReader.Item("RSNM_Id"))
                oRisposta.idDomanda = isNullInt(sqlReader.Item("DMML_DMND_Id"))
                oRisposta.idDomandaOpzione = isNullInt(sqlReader.Item("RSNM_DMNM_Id"))
                oRisposta.idRispostaQuestionario = isNullInt(sqlReader.Item("RSNM_RSQS_Id"))
                oRisposta.testoOpzione = isNullString(sqlReader.Item("DMNM_TestoPrima"))
                oRisposta.numeroOpzione = isNullInt(sqlReader.Item("DMNM_Numero"))
                oRisposta.valore = isNullDouble(sqlReader.Item("RSNM_Numero"))
                oRisposta.tipo = Domanda.TipoDomanda.Numerica
                listRisposte.Add(oRisposta)
            End While
            sqlReader.Close()
        End Using

        Return listRisposte
    End Function

    Public Shared Function RispostaNumerica_Insert(ByRef oRisOpzione As RispostaDomanda) As String
        Dim RetVal As String = ""
        If Not String.IsNullOrEmpty(oRisOpzione.valore) Then
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Dim sqlCommand As String = "sp_Questionario_RispostaNumerica_Insert"
            Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            db.AddInParameter(dbCommand, "idDomanda", DbType.Int32, oRisOpzione.idDomandaOpzione)
            db.AddInParameter(dbCommand, "idRispostaQuestionario", DbType.Int32, oRisOpzione.idRispostaQuestionario)
            db.AddInParameter(dbCommand, "numero", DbType.Double, Convert.ToDouble(oRisOpzione.valore))

            RetVal = db.ExecuteNonQuery(dbCommand)
        End If

        Return RetVal
    End Function

    Public Shared Function RispostaNumerica_Delete(ByVal idRS As String) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""
        Dim sqlCommand As String = "sp_Questionario_RispostaNumerica_Delete"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idRispostaQuestionario", DbType.Int32, idRS)

        RetVal = db.ExecuteNonQuery(dbCommand)

        Return RetVal
    End Function


#End Region
End Class
