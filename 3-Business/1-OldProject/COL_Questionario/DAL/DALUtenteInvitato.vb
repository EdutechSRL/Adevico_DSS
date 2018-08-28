
Imports System.Data.SqlClient
Imports COL_Questionario.RootObject
Imports System.Text.RegularExpressions
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports System.Data.Common
Imports System.Data

Public Class DALUtenteInvitato
    Public Shared Function Salva(ByRef oUtente As UtenteInvitato) As String
        Dim retVal As String = String.Empty
        If oUtente.ID > 0 Then
            UtenteInvitato_Update(oUtente)
        Else
            UtenteInvitato_Insert(oUtente)
        End If

        Return retVal
    End Function
    'Public Shared Function readUtenteAnonimoId() As Integer
    '    Dim db As Database = DatabaseFactory.CreateDatabase()
    '    Dim sqlCommand As String = "sp_Questionario_Persona_IdUtenteAnonimo_Select"
    '    Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
    '    Return db.ExecuteScalar(dbCommand)
    'End Function
    Public Shared Function readUtentiInvitatiByIDQuestionario(ByVal idQuestionario As String) As List(Of UtenteInvitato)

        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim sqlCommand As String = "sp_Questionario_UtentiInvitati_Select"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuestionario)

        Dim sqlReader As SqlDataReader
        sqlReader = db.ExecuteReader(dbCommand)

        Dim oUtenti As New List(Of UtenteInvitato)

        While sqlReader.Read()
            Dim oUtente As New UtenteInvitato
            oUtente.ID = isNullInt(sqlReader.Item("QSUI_Id"))
            oUtente.PersonaID = isNullInt(sqlReader.Item("QSUI_PRSN_Id"))
            oUtente.Cognome = isNullString(sqlReader.Item("QSUI_Cognome"))
            oUtente.Nome = isNullString(sqlReader.Item("QSUI_Nome"))
            oUtente.Mail = isNullString(sqlReader.Item("QSUI_Email"))
            oUtente.QuestionarioID = isNullInt(sqlReader.Item("QSUI_QSTN_Id"))
            oUtente.Descrizione = isNullString(sqlReader.Item("QSUI_Descrizione"))
            oUtente.isSelezionato = False
            oUtente.Password = isNullString(sqlReader.Item("QSUI_Password"))
            oUtenti.Add(oUtente)
        End While
        sqlReader.Close()
        Return oUtenti
    End Function
    Public Shared Function readUtentiInvitatiNonCompletati(ByVal idQuestionario As String) As List(Of UtenteInvitato)

        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim sqlCommand As String = "sp_Questionario_UtentiConQuestionarioNonInviato_Select"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuestionario)

        Dim sqlReader As SqlDataReader
        sqlReader = db.ExecuteReader(dbCommand)

        Dim oUtenti As New List(Of UtenteInvitato)

        While sqlReader.Read()
            Dim oUtente As New UtenteInvitato
            oUtente.ID = isNullInt(sqlReader.Item("QSUI_Id"))
            oUtente.PersonaID = isNullInt(sqlReader.Item("QSUI_PRSN_Id"))
            oUtente.Cognome = isNullString(sqlReader.Item("QSUI_Cognome"))
            oUtente.Nome = isNullString(sqlReader.Item("QSUI_Nome"))
            oUtente.Mail = isNullString(sqlReader.Item("QSUI_Email"))
            oUtente.QuestionarioID = isNullInt(sqlReader.Item("QSUI_QSTN_Id"))
            oUtente.Descrizione = isNullString(sqlReader.Item("QSUI_Descrizione"))
            oUtente.Password = isNullString(sqlReader.Item("QSUI_Password"))
            oUtente.isSelezionato = False
            oUtenti.Add(oUtente)
        End While
        sqlReader.Close()
        Return oUtenti
    End Function
    Public Shared Function GetInvitedUsersWithQuestionnairesNotStarted(ByVal idQuestionario As String) As List(Of UtenteInvitato)

        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim sqlCommand As String = "sp_Questionario_GetUsersWithNotStartedQuestionnaires"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuestionario)

        Dim sqlReader As SqlDataReader
        sqlReader = db.ExecuteReader(dbCommand)

        Dim oUtenti As New List(Of UtenteInvitato)

        While sqlReader.Read()
            Dim oUtente As New UtenteInvitato
            oUtente.ID = isNullInt(sqlReader.Item("QSUI_Id"))
            oUtente.PersonaID = isNullInt(sqlReader.Item("QSUI_PRSN_Id"))
            oUtente.Cognome = isNullString(sqlReader.Item("QSUI_Cognome"))
            oUtente.Nome = isNullString(sqlReader.Item("QSUI_Nome"))
            oUtente.Mail = isNullString(sqlReader.Item("QSUI_Email"))
            oUtente.QuestionarioID = isNullInt(sqlReader.Item("QSUI_QSTN_Id"))
            oUtente.Descrizione = isNullString(sqlReader.Item("QSUI_Descrizione"))
            oUtente.Password = isNullString(sqlReader.Item("QSUI_Password"))
            oUtente.isSelezionato = False
            oUtenti.Add(oUtente)
        End While
        sqlReader.Close()
        Return oUtenti
    End Function
    Public Shared Function GetInvitedUsersWithQuestionnairesNotCompleted(ByVal idQuestionario As String) As List(Of UtenteInvitato)

        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim sqlCommand As String = "sp_Questionario_GetInvitedUsersWithQuestionnairesNotCompleted"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuestionario)

        Dim sqlReader As SqlDataReader
        sqlReader = db.ExecuteReader(dbCommand)

        Dim oUtenti As New List(Of UtenteInvitato)

        While sqlReader.Read()
            Dim oUtente As New UtenteInvitato
            oUtente.ID = isNullInt(sqlReader.Item("QSUI_Id"))
            oUtente.PersonaID = isNullInt(sqlReader.Item("QSUI_PRSN_Id"))
            oUtente.Cognome = isNullString(sqlReader.Item("QSUI_Cognome"))
            oUtente.Nome = isNullString(sqlReader.Item("QSUI_Nome"))
            oUtente.Mail = isNullString(sqlReader.Item("QSUI_Email"))
            oUtente.QuestionarioID = isNullInt(sqlReader.Item("QSUI_QSTN_Id"))
            oUtente.Descrizione = isNullString(sqlReader.Item("QSUI_Descrizione"))
            oUtente.Password = isNullString(sqlReader.Item("QSUI_Password"))
            oUtente.isSelezionato = False
            oUtenti.Add(oUtente)
        End While
        sqlReader.Close()
        Return oUtenti
    End Function
    Public Shared Function GetInvitedUsersWithQuestionnairesCompleted(ByVal idQuestionario As String) As List(Of UtenteInvitato)

        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim sqlCommand As String = "sp_Questionario_GetInvitedUsersWithQuestionnairesCompleted"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuestionario)

        Dim sqlReader As SqlDataReader
        sqlReader = db.ExecuteReader(dbCommand)

        Dim oUtenti As New List(Of UtenteInvitato)

        While sqlReader.Read()
            Dim oUtente As New UtenteInvitato
            oUtente.ID = isNullInt(sqlReader.Item("QSUI_Id"))
            oUtente.PersonaID = isNullInt(sqlReader.Item("QSUI_PRSN_Id"))
            oUtente.Cognome = isNullString(sqlReader.Item("QSUI_Cognome"))
            oUtente.Nome = isNullString(sqlReader.Item("QSUI_Nome"))
            oUtente.Mail = isNullString(sqlReader.Item("QSUI_Email"))
            oUtente.QuestionarioID = isNullInt(sqlReader.Item("QSUI_QSTN_Id"))
            oUtente.Descrizione = isNullString(sqlReader.Item("QSUI_Descrizione"))
            oUtente.Password = isNullString(sqlReader.Item("QSUI_Password"))
            oUtente.isSelezionato = False
            oUtenti.Add(oUtente)
        End While
        sqlReader.Close()
        Return oUtenti
    End Function
    Public Shared Function GetInvitedUsersWithQuestionnairesNotStartedOrCompleted(ByVal idQuestionario As String) As List(Of UtenteInvitato)
        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim sqlCommand As String = "sp_Questionario_GetUsersWithStartedOrNotStartedQuestionnaires"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuestionario)

        Dim sqlReader As SqlDataReader
        sqlReader = db.ExecuteReader(dbCommand)

        Dim oUtenti As New List(Of UtenteInvitato)

        While sqlReader.Read()
            Dim oUtente As New UtenteInvitato
            oUtente.ID = isNullInt(sqlReader.Item("QSUI_Id"))
            oUtente.PersonaID = isNullInt(sqlReader.Item("QSUI_PRSN_Id"))
            oUtente.Cognome = isNullString(sqlReader.Item("QSUI_Cognome"))
            oUtente.Nome = isNullString(sqlReader.Item("QSUI_Nome"))
            oUtente.Mail = isNullString(sqlReader.Item("QSUI_Email"))
            oUtente.QuestionarioID = isNullInt(sqlReader.Item("QSUI_QSTN_Id"))
            oUtente.Descrizione = isNullString(sqlReader.Item("QSUI_Descrizione"))
            oUtente.Password = isNullString(sqlReader.Item("QSUI_Password"))
            oUtente.isSelezionato = False
            oUtenti.Add(oUtente)
        End While
        sqlReader.Close()
        Return oUtenti
    End Function
    Public Shared Function readDomandeQuestRandomUtenteInvitato(ByVal idQuestionario As String, ByVal idUI As Integer) As List(Of Domanda)

        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim sqlCommand As String = "sp_Questionario_DomandeQuestRandomUtenteInvitato_Select"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuestionario)
        db.AddInParameter(dbCommand, "idUtenteInvitato", DbType.Int32, idUI)

        Dim sqlReader As SqlDataReader
        sqlReader = db.ExecuteReader(dbCommand)

        Dim oDomande As New List(Of Domanda)

        While sqlReader.Read()
            Dim odomanda As New Domanda
            odomanda.id = isNullInt(sqlReader.Item("idDomanda"))
            odomanda.numero = isNullInt(sqlReader.Item("numeroDomanda"))
            odomanda.idPersonaCreator = idUI
            oDomande.Add(odomanda)
        End While
        sqlReader.Close()
        Return oDomande
    End Function
    Public Shared Function countUtentiInvitatiByIDQuestionario(ByVal idQuestionario As String) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim sqlCommand As String = "sp_Questionario_UtentiInvitati_Count"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuestionario)

        Dim count As Integer
        count = db.ExecuteScalar(dbCommand)

        Return count
    End Function
    Public Shared Function readUtentiInvitatiNoMailByIDQuestionario(ByVal idQuestionario As String) As List(Of UtenteInvitato)

        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim sqlCommand As String = "sp_Questionario_UtentiInvitatiNoMail_Select"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuestionario)

        Dim sqlReader As SqlDataReader
        sqlReader = db.ExecuteReader(dbCommand)

        Dim oUtenti As New List(Of UtenteInvitato)

        While sqlReader.Read()
            Dim oUtente As New UtenteInvitato
            oUtente.ID = isNullInt(sqlReader.Item("QSUI_Id"))
            oUtente.PersonaID = isNullInt(sqlReader.Item("QSUI_PRSN_Id"))
            oUtente.Cognome = isNullString(sqlReader.Item("QSUI_Cognome"))
            oUtente.Nome = isNullString(sqlReader.Item("QSUI_Nome"))
            oUtente.Mail = isNullString(sqlReader.Item("QSUI_Email"))
            oUtente.QuestionarioID = isNullInt(sqlReader.Item("QSUI_QSTN_Id"))
            oUtente.Descrizione = isNullString(sqlReader.Item("QSUI_Descrizione"))
            oUtente.isSelezionato = False
            oUtenti.Add(oUtente)
        End While

        Return oUtenti
    End Function
    'Public Shared Function readUtentiConQuestionarioInviato(ByVal idQuestionario As String, ByVal destinatari As Integer) As list(Of UtenteInvitato)

    '    Dim db As Database = DatabaseFactory.CreateDatabase()

    '    Dim sqlCommand As String = String.Empty
    '    Dim dbCommand As DbCommand

    '    Select Case destinatari
    '        Case Questionario.Destinatari.UtentiComunita
    '            sqlCommand = "sp_Questionario_UtentiComunitaConQuestionarioInviato_Select"
    '        Case Questionario.Destinatari.UtentiPortale
    '            sqlCommand = "sp_Questionario_UtentiPortaleConQuestionarioInviato_Select"
    '        Case Questionario.Destinatari.UtentiInvitati
    '            sqlCommand = "sp_Questionario_UtentiInvitatiConQuestionarioInviato_Select"
    '    End Select

    '    dbCommand = db.GetStoredProcCommand(sqlCommand)

    '    db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuestionario)

    '    Dim sqlReader As SqlDataReader
    '    sqlReader = db.ExecuteReader(dbCommand)

    '    Dim oUtenti As New list(Of UtenteInvitato)

    '    While sqlReader.Read()
    '        Dim oUtente As New UtenteInvitato
    '        oUtente.ID = isNullInt(sqlReader.Item("idUtenteInvitato"))
    '        oUtente.Cognome = isNullString(sqlReader.Item("cognome"))
    '        oUtente.Nome = isNullString(sqlReader.Item("nome"))
    '        oUtente.Mail = isNullString(sqlReader.Item("email"))
    '        'oUtente.QuestionarioID = isNullInt(sqlReader.Item("idQuestionario"))
    '        oUtente.PersonaID = isNullInt(sqlReader.Item("idPersona"))
    '        oUtenti.Add(oUtente)
    '    End While

    '    Return oUtenti
    'End Function

    'Public Shared Function readUtentiConQuestionarioIncompleto(ByVal idQuestionario As String) As list(Of UtenteInvitato)

    '    Dim db As Database = DatabaseFactory.CreateDatabase()

    '    Dim sqlCommand As String = "sp_Questionario_UtentiConQuestionarioIncompleto_Select"
    '    Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

    '    db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuestionario)

    '    Dim sqlReader As SqlDataReader
    '    sqlReader = db.ExecuteReader(dbCommand)

    '    Dim oUtenti As New list(Of UtenteInvitato)

    '    While sqlReader.Read()
    '        Dim oUtente As New UtenteInvitato
    '        oUtente.ID = isNullInt(sqlReader.Item("idUtenteInvitato"))
    '        oUtente.Cognome = isNullString(sqlReader.Item("cognome"))
    '        oUtente.Nome = isNullString(sqlReader.Item("nome"))
    '        oUtente.Mail = isNullString(sqlReader.Item("email"))
    '        oUtente.QuestionarioID = isNullInt(sqlReader.Item("idQuestionario"))
    '        oUtente.PersonaID = isNullInt(sqlReader.Item("idPersona"))
    '        oUtenti.Add(oUtente)
    '    End While

    '    Return oUtenti
    'End Function
    Public Shared Function readUtentiConQuestionarioNonCompilato(ByVal idQuestionario As String) As List(Of UtenteInvitato)

        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim sqlCommand As String = "sp_Questionario_UtentiConQuestionarioNonCompilato_Select"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuestionario)

        Dim sqlReader As SqlDataReader
        sqlReader = db.ExecuteReader(dbCommand)

        Dim oUtenti As New List(Of UtenteInvitato)

        While sqlReader.Read()
            Dim oUtente As New UtenteInvitato
            oUtente.ID = isNullInt(sqlReader.Item("QSUI_Id"))
            oUtente.PersonaID = isNullInt(sqlReader.Item("QSUI_PRSN_Id"))
            oUtente.Cognome = isNullString(sqlReader.Item("QSUI_Cognome"))
            oUtente.Nome = isNullString(sqlReader.Item("QSUI_Nome"))
            oUtente.Mail = isNullString(sqlReader.Item("QSUI_Email"))
            oUtente.QuestionarioID = isNullInt(sqlReader.Item("QSUI_QSTN_Id"))
            oUtente.Descrizione = isNullString(sqlReader.Item("QSUI_Descrizione"))
            oUtenti.Add(oUtente)
        End While

        Return oUtenti
    End Function
    Public Shared Function readUtenteInvitatoByID(ByVal idUtente As String) As UtenteInvitato

        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim sqlCommand As String = "sp_Questionario_UtenteInvitatoByID_Select"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idUtente", DbType.Int32, idUtente)

        Dim sqlReader As SqlDataReader
        sqlReader = db.ExecuteReader(dbCommand)

        Dim oUtente As New UtenteInvitato

        While sqlReader.Read()
            oUtente.ID = isNullInt(sqlReader.Item("QSUI_Id"))
            oUtente.PersonaID = isNullInt(sqlReader.Item("QSUI_PRSN_Id"))
            oUtente.Cognome = isNullString(sqlReader.Item("QSUI_Cognome"))
            oUtente.Nome = isNullString(sqlReader.Item("QSUI_Nome"))
            oUtente.Mail = isNullString(sqlReader.Item("QSUI_Email"))
            oUtente.QuestionarioID = isNullInt(sqlReader.Item("QSUI_QSTN_Id"))
            oUtente.Descrizione = isNullString(sqlReader.Item("QSUI_Descrizione"))
            oUtente.Password = isNullString(sqlReader.Item("QSUI_Password"))

        End While
        sqlReader.Close()
        Return oUtente
    End Function
    Public Shared Function readPersonaByID(ByVal idPersona As String) As UtenteInvitato

        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim sqlCommand As String = "sp_Questionario_PersonaByID_Select"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idPersona", DbType.Int32, idPersona)

        Dim sqlReader As SqlDataReader
        sqlReader = db.ExecuteReader(dbCommand)

        Dim oUtente As New UtenteInvitato

        While sqlReader.Read()
            oUtente.ID = 0
            oUtente.PersonaID = isNullInt(sqlReader.Item("PRSN_id"))
            oUtente.Cognome = isNullString(sqlReader.Item("PRSN_cognome"))
            oUtente.Nome = isNullString(sqlReader.Item("PRSN_nome"))
            oUtente.Mail = isNullString(sqlReader.Item("PRSN_mail"))
        End While
        sqlReader.Close()
        Return oUtente
    End Function
    Public Shared Function UtenteInvitato_Insert(ByRef oUtente As UtenteInvitato) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""
        Dim sqlCommand As String = "sp_Questionario_UtenteInvitato_Insert"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, oUtente.QuestionarioID)
        db.AddInParameter(dbCommand, "nome", DbType.String, oUtente.Nome)
        db.AddInParameter(dbCommand, "cognome", DbType.String, oUtente.Cognome)
        db.AddInParameter(dbCommand, "email", DbType.String, oUtente.Mail)
        db.AddInParameter(dbCommand, "descrizione", DbType.String, oUtente.Descrizione)
        db.AddInParameter(dbCommand, "idPersona", DbType.Int32, oUtente.PersonaID)
        db.AddInParameter(dbCommand, "password", DbType.String, RandomPasswordGenerator(6))


        RetVal = db.ExecuteNonQuery(dbCommand)

        Return RetVal
    End Function
    Public Shared Function UtenteInvitatoNumeroMail_Update(ByRef idUtente As Integer) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""
        Dim sqlCommand As String = "sp_Questionario_UtenteInvitatoNumeroMail_Update"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idUtenteInvitato", DbType.Int32, idUtente)

        RetVal = db.ExecuteNonQuery(dbCommand)

        Return RetVal
    End Function
    Public Shared Function UtenteInvitato_Delete(ByRef idUtente As Integer) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""
        Dim sqlCommand As String = "sp_Questionario_UtenteInvitato_Delete"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idUtente", DbType.Int32, idUtente)

        RetVal = db.ExecuteNonQuery(dbCommand)

        Return RetVal
    End Function
    Public Shared Function RandomPasswordGenerator(ByRef lenght As Integer) As String
        Dim n As System.Security.Cryptography.RandomNumberGenerator = _
        System.Security.Cryptography.RandomNumberGenerator.Create
        Dim Symbol(0) As Byte
        Dim SConverter As New System.Text.ASCIIEncoding
        Dim result As String = ""

        Do While (result.Length < lenght)
            n.GetBytes(Symbol)
            Dim st As String = SConverter.GetString(Symbol)
            If Char.IsLetterOrDigit(st(0)) Then result &= st(0)
        Loop

        Return result.ToLower
    End Function
    Public Shared Sub AddPasswordToCSV(ByRef path As String)
        Dim fileContents As String
        Dim StringArray As String()
        fileContents = lm.Comol.Core.File.ContentOf.TextFile(path)
        StringArray = fileContents.Split(vbCrLf)
        Dim index As Integer
        For index = 0 To StringArray.Length - 2
            StringArray(index) = StringArray(index).Trim(vbCrLf, vbCr, vbLf) & ";" & RandomPasswordGenerator(6) & vbCrLf
        Next
        fileContents = String.Empty
        For index = 0 To StringArray.Length - 2
            fileContents &= StringArray(index)
        Next
        lm.Comol.Core.File.Create.TextFile(path, fileContents, True, False)
    End Sub
    Public Shared Function UtenteInvitato_Update(ByRef oUtente As UtenteInvitato) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""
        Dim sqlCommand As String = "sp_Questionario_UtenteInvitato_Update"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idUtenteInvitato", DbType.Int32, oUtente.ID)
        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, oUtente.QuestionarioID)
        db.AddInParameter(dbCommand, "nome", DbType.String, oUtente.Nome)
        db.AddInParameter(dbCommand, "cognome", DbType.String, oUtente.Cognome)
        db.AddInParameter(dbCommand, "email", DbType.String, oUtente.Mail)
        db.AddInParameter(dbCommand, "descrizione", DbType.String, oUtente.Descrizione)
        db.AddInParameter(dbCommand, "idPersona", DbType.Int32, oUtente.PersonaID)

        RetVal = db.ExecuteNonQuery(dbCommand)

        Return RetVal
    End Function
    Public Shared Function importaCSV(ByRef path As String, ByRef terminator As String, ByRef idQuestionario As Integer)
        AddPasswordToCSV(path)


        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""
        Dim sqlCommand As String = "sp_Questionario_CSVimport"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "path", DbType.String, path)
        db.AddInParameter(dbCommand, "terminator", DbType.String, terminator)
        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuestionario)

        RetVal = db.ExecuteNonQuery(dbCommand)

        Return RetVal

    End Function
    ''' <summary>
    ''' Controlla se una certa persona è stata invitata a rispondere a un certo questionario
    ''' </summary>
    ''' <param name="idQuestionario"></param>
    ''' <param name="idPersona"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function isInvited(ByVal idQuestionario As Integer, ByRef idPersona As Integer) As Boolean
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase()

            Dim sqlCommand As String = "sp_Questionario_UtenteInvitato_Check"
            Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuestionario)
            db.AddInParameter(dbCommand, "idPersona", DbType.Int32, idPersona)
            Return (db.ExecuteScalar(dbCommand) > 0)
        Catch ex As Exception
            Return False
        End Try
    End Function
End Class
