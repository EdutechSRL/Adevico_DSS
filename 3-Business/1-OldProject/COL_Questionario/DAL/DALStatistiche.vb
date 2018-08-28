Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports COL_Questionario.RootObject
Imports System.Text.RegularExpressions
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports System.Data.Common
Imports System.Data
Imports System.Linq
Imports System.Collections
Imports System.Collections.Generic
Imports COL_Questionario.Business
Imports lm.Comol.Core.DomainModel

Public Class DALStatistiche

#Region "QUESTIONARIO INVIATO"
    Public Function readUtentiConQuestionarioInviato(ByVal idQuestionario As String, ByVal forUtentiComunita As Boolean, ByVal forUtentiPortale As Boolean, ByVal forUtentiEsterni As Boolean, ByVal forUtentiInvitati As Boolean, appContext As iApplicationContext) As List(Of UtenteInvitato)

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim oUtenti As New List(Of UtenteInvitato)
        Dim sql As String = String.Empty
        Dim dbCommand As DbCommand

        If forUtentiComunita Then
            sql = selectUtentiComunitaConQuestionarioInviato
        End If
        If forUtentiPortale Then
            If sql = String.Empty Then
                sql = selectUtentiPortaleConQuestionarioInviato
            Else
                sql = sql + " UNION " + selectUtentiPortaleConQuestionarioInviato
            End If
        End If
        If forUtentiEsterni Then
            If sql = String.Empty Then
                sql = selectUtentiEsterniConQuestionarioInviato
            Else
                sql = sql + " UNION " + selectUtentiEsterniConQuestionarioInviato
            End If
        End If

        If forUtentiInvitati Then
            If sql = String.Empty Then
                sql = selectUtentiInvitatiConQuestionarioInviato
            Else
                sql = sql + " UNION " + selectUtentiInvitatiConQuestionarioInviato
            End If
        End If

        If Not sql = String.Empty Then

            sql += " ORDER BY cognome, idPersona, idUtenteInvitato"

            dbCommand = db.GetSqlStringCommand(sql)

            db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuestionario)
            db.AddInParameter(dbCommand, "tipoPersona", DbType.Int32, COL_BusinessLogic_v2.Main.TipoPersonaStandard.Guest)

            Dim sqlReader As SqlDataReader
            sqlReader = db.ExecuteReader(dbCommand)

            While sqlReader.Read()
                Dim oUtente As New UtenteInvitato
                oUtente.ID = isNullInt(sqlReader.Item("idUtenteInvitato"))
                oUtente.Cognome = isNullString(sqlReader.Item("cognome"))
                oUtente.Nome = isNullString(sqlReader.Item("nome"))
                oUtente.Mail = isNullString(sqlReader.Item("email"))
                oUtente.Descrizione = isNullString(sqlReader.Item("descrizione"))
                oUtente.PersonaID = isNullInt(sqlReader.Item("idPersona"))
                oUtente.RispostaID = isNullInt(sqlReader.Item("idRisposta"))
                oUtente.IdRandomQuestionnaire = isNullInt(sqlReader.Item("IdRandom"))
                oUtenti.Add(oUtente)
            End While
        End If
        Return filterUserList(oUtenti, idQuestionario, appContext)
    End Function
    Private Function filterUserList(ByRef userList As List(Of UtenteInvitato), ByVal idQuestionnaire As Integer, appContext As iApplicationContext) As List(Of UtenteInvitato)
        Dim filteredUserList As New List(Of UtenteInvitato)
        If userList.Count > 0 Then
            Dim counter As Integer
            filteredUserList.Add(userList.Item(0))
            For counter = 1 To (userList.Count - 1)
                If Not (userList.Item(counter).ID = userList.Item(counter - 1).ID And userList.Item(counter).PersonaID = userList.Item(counter - 1).PersonaID And userList.Item(counter).RispostaID = userList.Item(counter - 1).RispostaID) Then
                    filteredUserList.Add(userList(counter))
                End If
            Next

            '     Dim idPerson As Integer = 0, attempt As Integer = 1
            Dim pAttempts As New Dictionary(Of Integer, List(Of dtoUserAnswerBaseItem))
            Dim iAttempts As New Dictionary(Of Integer, List(Of dtoUserAnswerBaseItem))
            Dim service As ServiceQuestionnaire = GetService(appContext)
            Dim idPersons As List(Of Integer) = filteredUserList.Where(Function(p) p.PersonaID > 0).Select(Function(i) i.PersonaID).Distinct.ToList

            For Each idPerson As Integer In idPersons
                pAttempts.Add(idPerson, service.GetQuestionnaireBaseAttempts(idQuestionnaire, idPerson, 0))
            Next
            Dim idUsers As List(Of Integer) = filteredUserList.Where(Function(p) p.PersonaID = 0).Select(Function(i) i.ID).Distinct.ToList
            For Each idUser As Integer In idUsers
                iAttempts.Add(idUser, service.GetQuestionnaireBaseAttempts(idQuestionnaire, 0, idUser))
            Next
            For Each item As UtenteInvitato In (From u In filteredUserList Order By u.PersonaID, u.RispostaID Select u).ToList
                If item.PersonaID > 0 Then
                    item.AttemptNumber = service.GetAttemptsNumber(item.IdRandomQuestionnaire, pAttempts(item.PersonaID))
                Else
                    item.AttemptNumber = service.GetAttemptsNumber(item.IdRandomQuestionnaire, iAttempts(item.ID))
                End If
                '    If idPerson <> item.PersonaID Then
                '        idPerson = item.PersonaID
                '        attempt = 1
                '        item.AttemptNumber = attempt
                '    Else
                '        attempt += 1
                '        item.AttemptNumber = attempt
                '    End If
            Next
            filteredUserList = filteredUserList.OrderBy(Function(u) u.Cognome).ThenBy(Function(u) u.Nome).ThenByDescending(Function(u) u.AttemptNumber).ToList
        End If
        Return filteredUserList
    End Function

    Private Shared ReadOnly Property selectUtentiComunitaConQuestionarioInviato()
        Get
            Dim str As String
            str = "SELECT distinct " & _
            "dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_Id as idRisposta,dbo.PERSONA.PRSN_nome AS nome, dbo.PERSONA.PRSN_mail AS email, " & _
            "dbo.PERSONA.PRSN_cognome AS cognome, dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_QSUI_Id AS idUtenteInvitato, " & _
            "dbo.PERSONA.PRSN_id AS idPersona, 0 as isAnonimo,0 as idComunita,0 as ruolo,'' as descrizione, RSQS_QSRD_Id as IdRandom, RSQS_DataModifica " & _
            "FROM dbo.QS_RISPOSTA_QUESTIONARIO " & _
            "INNER JOIN dbo.PERSONA ON dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_PRSN_Id = dbo.PERSONA.PRSN_id " & _
            "INNER JOIN dbo.QS_QUESTIONARIO ON dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_QSTN_Id = dbo.QS_QUESTIONARIO.QSTN_Id " & _
            "INNER JOIN dbo.TIPO_PERSONA ON dbo.PERSONA.PRSN_TPPR_id = dbo.TIPO_PERSONA.TPPR_id " & _
            "WHERE(dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_QSTN_Id = @idQuestionario) " & _
            "AND (dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_DataFine IS NOT NULL) " & _
            "AND (dbo.QS_QUESTIONARIO.QSTN_forUtentiComunita = 1) and PRSN_TPPR_id!=@tipoPersona "
            Return str
        End Get
    End Property
    Private Shared ReadOnly Property selectUtentiPortaleConQuestionarioInviato()
        Get
            Dim str As String
            str = "SELECT distinct " & _
            "dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_Id as idRisposta,dbo.PERSONA.PRSN_nome AS nome, dbo.PERSONA.PRSN_mail AS email, " & _
            "dbo.PERSONA.PRSN_cognome AS cognome, 0 AS idUtenteInvitato, " & _
            "dbo.PERSONA.PRSN_id AS idPersona, 0 as isAnonimo,0 as idComunita,0 as ruolo,'' as descrizione, RSQS_QSRD_Id as IdRandom, RSQS_DataModifica " & _
            "FROM dbo.QS_RISPOSTA_QUESTIONARIO " & _
            "INNER JOIN dbo.PERSONA ON dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_PRSN_Id = dbo.PERSONA.PRSN_id " & _
            "INNER JOIN dbo.QS_QUESTIONARIO ON dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_QSTN_Id = dbo.QS_QUESTIONARIO.QSTN_Id " & _
            "INNER JOIN dbo.TIPO_PERSONA ON dbo.PERSONA.PRSN_TPPR_id = dbo.TIPO_PERSONA.TPPR_id " & _
            "WHERE(dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_QSTN_Id = @idQuestionario) " & _
            "AND (dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_DataFine IS NOT NULL) " & _
            "AND ((dbo.QS_QUESTIONARIO.QSTN_forUtentiPortale = 1) OR (dbo.QS_QUESTIONARIO.QSTN_forUtentiComunita = 1)) and PRSN_TPPR_id!=@tipoPersona "
            Return str
        End Get
    End Property
    Private Shared ReadOnly Property selectUtentiEsterniConQuestionarioInviato()
        Get
            Dim str As String
            str = "SELECT distinct " & _
            "dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_Id as idRisposta,dbo.PERSONA.PRSN_nome AS nome, dbo.PERSONA.PRSN_mail AS email, " & _
            "dbo.PERSONA.PRSN_cognome AS cognome, 0 AS idUtenteInvitato, " & _
            "dbo.PERSONA.PRSN_id AS idPersona, 1 as isAnonimo,0 as idComunita,0 as ruolo,'' as descrizione, RSQS_QSRD_Id as IdRandom, RSQS_DataModifica " & _
            "FROM dbo.QS_RISPOSTA_QUESTIONARIO " & _
            "INNER JOIN dbo.PERSONA ON dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_PRSN_Id = dbo.PERSONA.PRSN_id " & _
            "INNER JOIN dbo.QS_QUESTIONARIO ON dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_QSTN_Id = dbo.QS_QUESTIONARIO.QSTN_Id " & _
            "INNER JOIN dbo.TIPO_PERSONA ON dbo.PERSONA.PRSN_TPPR_id = dbo.TIPO_PERSONA.TPPR_id " & _
            "WHERE(dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_QSTN_Id = @idQuestionario) " & _
            "AND (dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_DataFine IS NOT NULL) " & _
            "AND (dbo.QS_QUESTIONARIO.QSTN_forUtentiEsterni = 1) and PRSN_TPPR_id=@tipoPersona"
            Return str
        End Get
    End Property
    Private Shared ReadOnly Property selectUtentiInvitatiConQuestionarioInviato()
        Get
            Dim str As String
            str = "SELECT distinct " & _
            "dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_Id as idRisposta,dbo.QS_QUESTIONARIO_UTENTE_INVITATO.QSUI_Nome as nome, dbo.QS_QUESTIONARIO_UTENTE_INVITATO.QSUI_Email as email, " & _
            "dbo.QS_QUESTIONARIO_UTENTE_INVITATO.QSUI_Cognome as cognome, " & _
            "dbo.QS_QUESTIONARIO_UTENTE_INVITATO.QSUI_Id as idUtenteInvitato, " & _
            "dbo.QS_QUESTIONARIO_UTENTE_INVITATO.QSUI_PRSN_Id as idPersona, 0 as isAnonimo,0 as idComunita,0 as ruolo, dbo.QS_QUESTIONARIO_UTENTE_INVITATO.QSUI_Descrizione as descrizione, RSQS_QSRD_Id as IdRandom , RSQS_DataModifica " & _
            "FROM dbo.QS_RISPOSTA_QUESTIONARIO " & _
            "INNER JOIN dbo.QS_QUESTIONARIO_UTENTE_INVITATO ON dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_QSUI_Id = dbo.QS_QUESTIONARIO_UTENTE_INVITATO.QSUI_Id " & _
            "WHERE (dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_QSTN_Id = @idQuestionario) and (dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_DataFine is not null) "
            Return str
        End Get
    End Property
#End Region

#Region "QUESTIONARIO INCOMPLETO"
    Public Function readUtentiConQuestionarioIncompleto(ByVal idQuestionario As String, ByVal forUtentiComunita As Boolean, ByVal forUtentiPortale As Boolean, ByVal forUtentiEsterni As Boolean, ByVal forUtentiInvitati As Boolean, appContext As iApplicationContext) As List(Of UtenteInvitato)

        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim oUtenti As New List(Of UtenteInvitato)

        Dim sql As String = String.Empty
        Dim dbCommand As DbCommand

        If forUtentiComunita Then
            sql = selectUtentiComunitaConQuestionarioIncompleto
        End If

        If forUtentiPortale Then
            If sql = String.Empty Then
                sql = selectUtentiPortaleConQuestionarioIncompleto
            Else
                sql = sql + " UNION " + selectUtentiPortaleConQuestionarioIncompleto
            End If
        End If

        If forUtentiEsterni Then
            If sql = String.Empty Then
                sql = selectUtentiEsterniConQuestionarioIncompleto
            Else
                sql = sql + " UNION " + selectUtentiEsterniConQuestionarioIncompleto
            End If
        End If

        If forUtentiInvitati Then
            If sql = String.Empty Then
                sql = selectUtentiInvitatiConQuestionarioIncompleto
            Else
                sql = sql + " UNION " + selectUtentiInvitatiConQuestionarioIncompleto
            End If
        End If

        If Not sql = String.Empty Then

            sql += " ORDER BY cognome"

            dbCommand = db.GetSqlStringCommand(sql)

            db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuestionario)
            db.AddInParameter(dbCommand, "tipoPersona", DbType.Int32, COL_BusinessLogic_v2.Main.TipoPersonaStandard.Guest)

            Dim sqlReader As SqlDataReader
            sqlReader = db.ExecuteReader(dbCommand)

            While sqlReader.Read()
                Dim oUtente As New UtenteInvitato
                oUtente.ID = isNullInt(sqlReader.Item("idUtenteInvitato"))
                oUtente.Cognome = isNullString(sqlReader.Item("cognome"))
                oUtente.Nome = isNullString(sqlReader.Item("nome"))
                oUtente.Mail = isNullString(sqlReader.Item("email"))
                oUtente.Descrizione = isNullString(sqlReader.Item("descrizione"))
                oUtente.PersonaID = isNullInt(sqlReader.Item("idPersona"))
                oUtente.RispostaID = isNullInt(sqlReader.Item("idRisposta"))
                oUtente.IdRandomQuestionnaire = isNullInt(sqlReader.Item("IdRandom"))
                oUtenti.Add(oUtente)
            End While
        End If
        Return filterUserList(oUtenti, idQuestionario, appContext)
    End Function

    Private Shared ReadOnly Property selectUtentiComunitaConQuestionarioIncompleto()
        Get
            Dim str As String
            str = "SELECT " & _
            "dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_Id as idRisposta,dbo.PERSONA.PRSN_nome AS nome, dbo.PERSONA.PRSN_mail AS email, " & _
            "dbo.PERSONA.PRSN_cognome AS cognome, 0 AS idUtenteInvitato, " & _
            "dbo.PERSONA.PRSN_id AS idPersona, 0 as isAnonimo,0 as idComunita,0 as ruolo,'' as descrizione, RSQS_QSRD_Id as IdRandom, RSQS_DataModifica  " & _
            "FROM dbo.QS_RISPOSTA_QUESTIONARIO " & _
            "INNER JOIN dbo.PERSONA ON dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_PRSN_Id = dbo.PERSONA.PRSN_id " & _
            "INNER JOIN dbo.QS_QUESTIONARIO ON dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_QSTN_Id = dbo.QS_QUESTIONARIO.QSTN_Id " & _
            "INNER JOIN dbo.TIPO_PERSONA ON dbo.PERSONA.PRSN_TPPR_id = dbo.TIPO_PERSONA.TPPR_id " & _
            "WHERE(dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_QSTN_Id = @idQuestionario) " & _
            "AND (dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_DataFine IS NULL) " & _
            "AND (dbo.QS_QUESTIONARIO.QSTN_forUtentiComunita = 1) and PRSN_TPPR_id!=@tipoPersona "
            Return (str)
        End Get
    End Property

    Private Shared ReadOnly Property selectUtentiPortaleConQuestionarioIncompleto()
        Get
            Dim str As String
            str = "SELECT " & _
            "dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_Id as idRisposta,dbo.PERSONA.PRSN_nome AS nome, dbo.PERSONA.PRSN_mail AS email, " & _
            "dbo.PERSONA.PRSN_cognome AS cognome, 0 AS idUtenteInvitato, " & _
            "dbo.PERSONA.PRSN_id AS idPersona, 0 as isAnonimo,0 as idComunita,0 as ruolo,'' as descrizione, RSQS_QSRD_Id as IdRandom, RSQS_DataModifica  " & _
            "FROM dbo.QS_RISPOSTA_QUESTIONARIO " & _
            "INNER JOIN dbo.PERSONA ON dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_PRSN_Id = dbo.PERSONA.PRSN_id " & _
            "INNER JOIN dbo.QS_QUESTIONARIO ON dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_QSTN_Id = dbo.QS_QUESTIONARIO.QSTN_Id " & _
            "INNER JOIN dbo.TIPO_PERSONA ON dbo.PERSONA.PRSN_TPPR_id = dbo.TIPO_PERSONA.TPPR_id " & _
            "WHERE(dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_QSTN_Id = @idQuestionario) " & _
            "AND (dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_DataFine IS NULL) " & _
            "AND ((dbo.QS_QUESTIONARIO.QSTN_forUtentiPortale = 1) OR (dbo.QS_QUESTIONARIO.QSTN_forUtentiComunita = 1)) and PRSN_TPPR_id!=@tipoPersona "
            Return str
        End Get
    End Property

    Private Shared ReadOnly Property selectUtentiEsterniConQuestionarioIncompleto()
        Get
            Dim str As String
            str = "SELECT " & _
            "dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_Id as idRisposta,dbo.PERSONA.PRSN_nome AS nome, dbo.PERSONA.PRSN_mail AS email, " & _
            "dbo.PERSONA.PRSN_cognome AS cognome, 0 AS idUtenteInvitato, " & _
            "dbo.PERSONA.PRSN_id AS idPersona, 1 as isAnonimo,0 as idComunita,0 as ruolo,'' as descrizione, RSQS_QSRD_Id as IdRandom, RSQS_DataModifica  " & _
            "FROM dbo.QS_RISPOSTA_QUESTIONARIO " & _
            "INNER JOIN dbo.PERSONA ON dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_PRSN_Id = dbo.PERSONA.PRSN_id " & _
            "INNER JOIN dbo.QS_QUESTIONARIO ON dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_QSTN_Id = dbo.QS_QUESTIONARIO.QSTN_Id " & _
            "INNER JOIN dbo.TIPO_PERSONA ON dbo.PERSONA.PRSN_TPPR_id = dbo.TIPO_PERSONA.TPPR_id " & _
            "WHERE(dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_QSTN_Id = @idQuestionario) " & _
            "AND (dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_DataFine IS NULL) " & _
            "AND (dbo.QS_QUESTIONARIO.QSTN_forUtentiEsterni = 1) and PRSN_TPPR_id=@tipoPersona"
            Return str
        End Get
    End Property

    Private Shared ReadOnly Property selectUtentiInvitatiConQuestionarioIncompleto()
        Get
            Dim str As String
            str = "SELECT " & _
            "dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_Id as idRisposta,dbo.QS_QUESTIONARIO_UTENTE_INVITATO.QSUI_Nome as nome, dbo.QS_QUESTIONARIO_UTENTE_INVITATO.QSUI_Email as email, " & _
            "dbo.QS_QUESTIONARIO_UTENTE_INVITATO.QSUI_Cognome as cognome, " & _
            "dbo.QS_QUESTIONARIO_UTENTE_INVITATO.QSUI_Id as idUtenteInvitato, " & _
            "dbo.QS_QUESTIONARIO_UTENTE_INVITATO.QSUI_PRSN_Id as idPersona, 0 as isAnonimo,0 as idComunita,0 as ruolo,dbo.QS_QUESTIONARIO_UTENTE_INVITATO.QSUI_Descrizione as descrizione, RSQS_QSRD_Id as IdRandom, RSQS_DataModifica  " & _
            "FROM dbo.QS_RISPOSTA_QUESTIONARIO " & _
            "INNER JOIN dbo.QS_QUESTIONARIO_UTENTE_INVITATO ON dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_QSUI_Id = dbo.QS_QUESTIONARIO_UTENTE_INVITATO.QSUI_Id " & _
            "WHERE (dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_QSTN_Id = @idQuestionario) and (dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_DataFine is null) "
            Return str
        End Get
    End Property

#End Region

#Region "QUESTIONARIO NON COMPILATO"
    Public Function readUtentiConQuestionarioNonCompilato(ByVal idQuestionario As String, ByVal idComunita As String, ByVal forUtentiComunita As Boolean, ByVal forUtentiPortale As Boolean, ByVal forUtentiEsterni As Boolean, ByVal forUtentiInvitati As Boolean, appContext As iApplicationContext) As List(Of UtenteInvitato)

        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim sql As String = String.Empty
        Dim dbCommand As DbCommand

        Dim oUtenti As New List(Of UtenteInvitato)
        If forUtentiComunita Then
            sql = selectUtentiComunitaConQuestionarioNonCompilato
        End If

        If forUtentiPortale Then
            If sql = String.Empty Then
                sql = selectUtentiPortaleConQuestionarioNonCompilato
            Else
                sql = sql + " UNION " + selectUtentiPortaleConQuestionarioNonCompilato
            End If
        End If

        If forUtentiEsterni And Not forUtentiComunita And Not forUtentiInvitati And Not forUtentiPortale Then
            'verificare che questo controllo sia necessario. Lasciando solo "forUtentiEsterni" le liste rimangono sempre vuote quando il quest e' anche per loro
            Return oUtenti
        End If

        If forUtentiInvitati Then
            If sql = String.Empty Then
                sql = selectUtentiInvitatiConQuestionarioNonCompilato
            Else
                sql = sql + " UNION " + selectUtentiInvitatiConQuestionarioNonCompilato
            End If
        End If

        If Not sql = String.Empty Then

            sql += " ORDER BY cognome"

            dbCommand = db.GetSqlStringCommand(sql)

            db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuestionario)
            db.AddInParameter(dbCommand, "tipoPersona", DbType.Int32, COL_BusinessLogic_v2.Main.TipoPersonaStandard.Guest)

            If forUtentiComunita Then
                db.AddInParameter(dbCommand, "idComunita", DbType.Int32, idComunita)
            End If

            Dim sqlReader As SqlDataReader
            sqlReader = db.ExecuteReader(dbCommand)

            While sqlReader.Read()
                Dim oUtente As New UtenteInvitato
                oUtente.ID = isNullInt(sqlReader.Item("idUtenteInvitato"))
                oUtente.Cognome = isNullString(sqlReader.Item("cognome"))
                oUtente.Nome = isNullString(sqlReader.Item("nome"))
                oUtente.Mail = isNullString(sqlReader.Item("email"))
                oUtente.Descrizione = isNullString(sqlReader.Item("descrizione"))
                oUtente.PersonaID = isNullInt(sqlReader.Item("idPersona"))
                '  oUtente.IdRandomQuestionnaire = isNullInt(sqlReader.Item("IdRandom"))
                oUtenti.Add(oUtente)
            End While
        End If

        Return filterUserList(oUtenti, idQuestionario, appContext)
    End Function

    Private Shared ReadOnly Property selectUtentiComunitaConQuestionarioNonCompilato()
        Get
            Dim str As String
            str = "SELECT dbo.PERSONA.PRSN_nome AS nome, dbo.PERSONA.PRSN_mail AS email, " & _
            "dbo.PERSONA.PRSN_cognome AS cognome, 0 AS idUtenteInvitato, dbo.PERSONA.PRSN_id AS idPersona, " & _
            "1 AS isAnonimo, dbo.LK_RUOLO_PERSONA_COMUNITA.RLPC_CMNT_id as idComunita, " & _
            "dbo.LK_RUOLO_PERSONA_COMUNITA.RLPC_TPRL_id as ruolo,'' as descrizione  " & _
            "FROM dbo.PERSONA INNER JOIN dbo.LK_RUOLO_PERSONA_COMUNITA ON dbo.PERSONA.PRSN_id = dbo.LK_RUOLO_PERSONA_COMUNITA.RLPC_PRSN_id " & _
            "WHERE (dbo.PERSONA.PRSN_TPPR_id <> @tipoPersona) AND " & _
            "((SELECT QSTN_forUtentiComunita FROM dbo.QS_QUESTIONARIO AS QS_QUESTIONARIO_1 " & _
            "WHERE (QSTN_Id = @idQuestionario)) = 1) AND " & _
            "((SELECT COUNT(*) AS nRisposte FROM dbo.QS_RISPOSTA_QUESTIONARIO " & _
            "WHERE (RSQS_PRSN_Id = dbo.PERSONA.PRSN_id  AND RSQS_QSTN_Id = @idQuestionario)) = 0) AND (dbo.LK_RUOLO_PERSONA_COMUNITA.RLPC_CMNT_id = @idComunita) AND " & _
            "(dbo.LK_RUOLO_PERSONA_COMUNITA.RLPC_TPRL_id > 0)"
            Return (str)
        End Get
    End Property

    Private Shared ReadOnly Property selectUtentiPortaleConQuestionarioNonCompilato()
        Get
            Dim str As String
            str = "SELECT dbo.PERSONA.PRSN_nome AS nome, dbo.PERSONA.PRSN_mail AS email, " & _
            "dbo.PERSONA.PRSN_cognome AS cognome, 0 AS idUtenteInvitato, " & _
            "dbo.PERSONA.PRSN_id AS idPersona, 1 AS isAnonimo,0 as idComunita,0 as ruolo,'' as descrizione " & _
            "FROM dbo.PERSONA " & _
            "WHERE(dbo.PERSONA.PRSN_TPPR_id != @tipoPersona) AND " & _
            "(((SELECT QSTN_forUtentiComunita FROM dbo.QS_QUESTIONARIO AS QS_QUESTIONARIO_1 " & _
            "WHERE (QSTN_Id = @idQuestionario)) = 1) OR ((SELECT QSTN_forUtentiPortale FROM dbo.QS_QUESTIONARIO " & _
            "AS QS_QUESTIONARIO_1 WHERE (QSTN_Id = @idQuestionario)) = 1)) AND " & _
            "((SELECT COUNT(*) AS nRisposte " & _
            "FROM dbo.QS_RISPOSTA_QUESTIONARIO " & _
            "WHERE (RSQS_PRSN_Id = dbo.PERSONA.PRSN_id AND RSQS_QSTN_Id = @idQuestionario)) = 0)"
            Return (str)
        End Get
    End Property

    'Private Shared ReadOnly Property selectUtentiEsterniConQuestionarioNonCompilato()
    '    Get
    '        Dim str As String
    '        'str = "SELECT " & _
    '        '"dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_Id as idRisposta,dbo.PERSONA.PRSN_nome AS nome, dbo.PERSONA.PRSN_mail AS email, " & _
    '        '"dbo.PERSONA.PRSN_cognome AS cognome, 0 AS idUtenteInvitato, " & _
    '        '"dbo.PERSONA.PRSN_id AS idPersona, 1 as isAnonimo " & _
    '        '"FROM dbo.QS_RISPOSTA_QUESTIONARIO " & _
    '        '"INNER JOIN dbo.PERSONA ON dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_PRSN_Id = dbo.PERSONA.PRSN_id " & _
    '        '"INNER JOIN dbo.QS_QUESTIONARIO ON dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_QSTN_Id = dbo.QS_QUESTIONARIO.QSTN_Id " & _
    '        '"INNER JOIN dbo.TIPO_PERSONA ON dbo.PERSONA.PRSN_TPPR_id = dbo.TIPO_PERSONA.TPPR_id " & _
    '        '"WHERE(dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_QSTN_Id = @idQuestionario) " & _
    '        '"AND (dbo.QS_RISPOSTA_QUESTIONARIO.RSQS_DataFine IS NULL) " & _
    '        '"AND (dbo.QS_QUESTIONARIO.QSTN_forUtentiEsterni = 1) and PRSN_TPPR_id=@tipoPersona"
    '        Return str
    '    End Get
    'End Property

    Private Shared ReadOnly Property selectUtentiInvitatiConQuestionarioNonCompilato()
        Get
            Dim str As String
            str = "SELECT " & _
            "dbo.QS_QUESTIONARIO_UTENTE_INVITATO.QSUI_Nome as nome, dbo.QS_QUESTIONARIO_UTENTE_INVITATO.QSUI_Email as email, " & _
            "dbo.QS_QUESTIONARIO_UTENTE_INVITATO.QSUI_Cognome as cognome, " & _
            "dbo.QS_QUESTIONARIO_UTENTE_INVITATO.QSUI_Id as idUtenteInvitato, " & _
            "dbo.QS_QUESTIONARIO_UTENTE_INVITATO.QSUI_PRSN_Id as idPersona, 0 as isAnonimo,0 as idComunita, 0 as ruolo,dbo.QS_QUESTIONARIO_UTENTE_INVITATO.QSUI_Descrizione as descrizione " & _
            "FROM dbo.QS_QUESTIONARIO_UTENTE_INVITATO " & _
            "WHERE (QSUI_QSTN_Id = @idQuestionario) AND " & _
            "((SELECT COUNT(*) AS nRisposte FROM dbo.QS_RISPOSTA_QUESTIONARIO " & _
            " WHERE (RSQS_QSUI_Id = dbo.QS_QUESTIONARIO_UTENTE_INVITATO.QSUI_Id)) = 0)"
            Return str
        End Get
    End Property

#End Region

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

    Public Shared Function GetQuestionnaireStatisticsFilter(appContext As iApplicationContext, ByVal idQuestionnaire As Integer) As QuestionnaireStatisticsFilter
        Dim s As ServiceQuestionnaire = GetService(appContext)
        If IsNothing(s) Then
            Return QuestionnaireStatisticsFilter.GetDefault()
        Else
            Return s.GetStatisticsFilter(idQuestionnaire)
        End If
    End Function


    Public Function readQuestionnaireAttemptsByIdPersona(ByVal idQuestionario As Integer, ByVal idPerson As Integer, ByVal compiled As Boolean, appContext As iApplicationContext) As List(Of UtenteInvitato)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim oUtenti As New List(Of UtenteInvitato)
        Dim sql As String = String.Empty
        Dim dbCommand As DbCommand

        If compiled Then
            sql = selectUtentiComunitaConQuestionarioInviato
        Else
            sql = selectUtentiComunitaConQuestionarioIncompleto
        End If

        If Not sql = String.Empty Then
            sql += " AND PRSN_id=" + idPerson.ToString()
            sql += " ORDER BY cognome, idPersona, idUtenteInvitato"

            dbCommand = db.GetSqlStringCommand(sql)

            db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuestionario)
            db.AddInParameter(dbCommand, "tipoPersona", DbType.Int32, COL_BusinessLogic_v2.Main.TipoPersonaStandard.Guest)

            Dim sqlReader As SqlDataReader
            sqlReader = db.ExecuteReader(dbCommand)

            While sqlReader.Read()
                Dim oUtente As New UtenteInvitato
                oUtente.ID = isNullInt(sqlReader.Item("idUtenteInvitato"))
                oUtente.Cognome = isNullString(sqlReader.Item("cognome"))
                oUtente.Nome = isNullString(sqlReader.Item("nome"))
                oUtente.Mail = isNullString(sqlReader.Item("email"))
                oUtente.Descrizione = isNullString(sqlReader.Item("descrizione"))
                oUtente.PersonaID = isNullInt(sqlReader.Item("idPersona"))
                oUtente.RispostaID = isNullInt(sqlReader.Item("idRisposta"))
                oUtente.IdRandomQuestionnaire = isNullInt(sqlReader.Item("IdRandom"))
                oUtente.ModifyedOn = isNullDateMin(sqlReader.Item("RSQS_DataModifica"))
                oUtenti.Add(oUtente)
            End While
        End If
        Return filterUserList(oUtenti, idQuestionario, appContext)
    End Function

End Class
