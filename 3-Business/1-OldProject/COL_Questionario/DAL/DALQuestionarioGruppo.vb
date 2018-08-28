Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports System.Data.Common
Imports System.Collections.Generic
Imports System.Data
Imports COL_Questionario.RootObject

Public Class DALQuestionarioGruppo

    Public Shared Function readSottoGruppi(ByVal idGruppoPadre As Integer, ByVal idComunita As Integer) As list(Of QuestionarioGruppo)

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "sp_Questionario_QuestionarioGruppoBYIDPadre_Select"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idGruppoPadre", DbType.Int32, idGruppoPadre)
        db.AddInParameter(dbCommand, "idComunita", DbType.Int32, idComunita)

        Dim sqlReader As SqlDataReader
        sqlReader = db.ExecuteReader(dbCommand)

        Dim _listaGruppi As New list(Of QuestionarioGruppo)
        Dim _gruppo As New QuestionarioGruppo

        While sqlReader.Read()
            _gruppo = New QuestionarioGruppo
            _gruppo.id = isNullInt(sqlReader.Item("QSGR_Id"))
            _gruppo.nome = isNullString(sqlReader.Item("QSGR_Nome"))
            _gruppo.idGruppoPadre = isNullInt(sqlReader.Item("QSGR_IdGruppoPadre"))
            _gruppo.dataCreazione = isNullString(sqlReader.Item("QSGR_DataCreazione"))
            _gruppo.descrizione = isNullString(sqlReader.Item("QSGR_Descrizione"))
            _gruppo.dataModifica = isNullString(sqlReader.Item("QSGR_DataModifica"))
            _listaGruppi.Add(_gruppo)
        End While

        Return _listaGruppi

    End Function

    Public Shared Function readGruppi(ByVal idComunita As Integer) As list(Of QuestionarioGruppo)

        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim sqlCommand As String = "sp_Questionario_QuestionarioGruppo_Select"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idComunita", DbType.Int32, idComunita)
        'db.AddInParameter(dbCommand, "tipo", DbType.Int32, tipo)

        Dim sqlReader As SqlDataReader
        sqlReader = db.ExecuteReader(dbCommand)

        Dim _listaGruppi As New list(Of QuestionarioGruppo)
        Dim _gruppo As New QuestionarioGruppo

        While sqlReader.Read()
            _gruppo = New QuestionarioGruppo
            _gruppo.id = isNullInt(sqlReader.Item("QSGR_Id"))
            _gruppo.nome = isNullString(sqlReader.Item("QSGR_Nome"))
            _listaGruppi.Add(_gruppo)
        End While

        Return _listaGruppi

    End Function

    Public Shared Function InsertGruppo(ByRef oGruppo As QuestionarioGruppo) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim sqlCommand As String = "sp_Questionario_QuestionarioGruppo_Insert"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "nome", DbType.String, oGruppo.nome)
        db.AddInParameter(dbCommand, "idGruppoPadre", DbType.Int32, oGruppo.idGruppoPadre)
        db.AddInParameter(dbCommand, "descrizione", DbType.String, oGruppo.descrizione)
        db.AddInParameter(dbCommand, "dataCreazione", DbType.DateTime, setNullDate(oGruppo.dataCreazione))
        db.AddInParameter(dbCommand, "dataModifica", DbType.DateTime, setNullDate(oGruppo.dataModifica))
        db.AddInParameter(dbCommand, "idPersona", DbType.Int32, oGruppo.idPersona)
        db.AddInParameter(dbCommand, "isPubblico", DbType.Boolean, oGruppo.isPubblico)
        db.AddInParameter(dbCommand, "isCondiviso", DbType.Boolean, oGruppo.isCondiviso)
        db.AddInParameter(dbCommand, "isCancellato", DbType.Boolean, oGruppo.isCancellato)
        'db.AddInParameter(dbCommand, "tipo", DbType.Int32, oGruppo.tipo)
        db.AddInParameter(dbCommand, "idComunita", DbType.Int32, oGruppo.idComunita)
        db.AddOutParameter(dbCommand, "idGruppo", DbType.Int32, 4)

        db.ExecuteNonQuery(dbCommand)
        Dim idGruppoDefault As Integer = RootObject.isNullInt(db.GetParameterValue(dbCommand, "idGruppo"))
       

        Return idGruppoDefault
    End Function

    Public Shared Function ComunitaByGruppo(ByRef idGruppo As Integer) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim sqlCommand As String = "sp_Questionario_ComunitaByGruppo"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idGruppo", DbType.Int32, idGruppo)
        Dim retVal As Integer
        retVal = db.ExecuteScalar(dbCommand)
        Return retVal
    End Function

    Public Shared Function GruppoPrincipaleByComunita_Id(ByRef idComunita As Integer) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim sqlCommand As String = "sp_Questionario_GruppoPrincipaleByComunita"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idComunita", DbType.Int32, idComunita)
        'db.AddInParameter(dbCommand, "tipo", DbType.Int32, tipo)

        Dim retVal As Integer
        retVal = db.ExecuteScalar(dbCommand)
        If retVal = 0 Then
            Dim oGruppo As New QuestionarioGruppo(idComunita)
            retVal = InsertGruppo(oGruppo)
        End If
        Return retVal
    End Function
    Public Shared Function GruppoPrincipaleByComunita(ByRef idComunita As Integer) As QuestionarioGruppo
        'ottimizzare creando SP che fa entrambe le cose
        Return readGruppoBYID(GruppoPrincipaleByComunita_Id(idComunita))
    End Function
    Public Shared Function GruppoModelliPubblici() As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim sqlCommand As String = "sp_Questionario_GruppoModelliPubblici"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        Dim retVal As Integer
        retVal = db.ExecuteScalar(dbCommand)
        Return retVal
    End Function

    Public Shared Function readGruppoBYID(ByVal idGruppo As Integer) As QuestionarioGruppo

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim _gruppo As New QuestionarioGruppo

        Using connection As DbConnection = db.CreateConnection()
            connection.Open()

            Dim sqlCommand As String = "sp_Questionario_QuestionarioGruppoBYID_Select"
            Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            dbCommand.Connection = connection
            db.AddInParameter(dbCommand, "IdGruppo", DbType.Int32, idGruppo)

            Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)

                While sqlReader.Read()
                    _gruppo.id = isNullInt(sqlReader.Item("QSGR_Id"))
                    _gruppo.nome = isNullString(sqlReader.Item("QSGR_Nome"))
                    _gruppo.descrizione = isNullString(sqlReader.Item("QSGR_Descrizione"))
                    _gruppo.idGruppoPadre = isNullInt(sqlReader.Item("QSGR_IdGruppoPadre"))
                    _gruppo.dataCreazione = isNullString(sqlReader.Item("QSGR_DataCreazione"))
                    _gruppo.dataModifica = isNullString(sqlReader.Item("QSGR_DataModifica"))
                    _gruppo.idComunita = isNullInt(sqlReader.Item("QSGR_CMNT_Id"))
                    _gruppo.idPersona = isNullInt(sqlReader.Item("QSGR_PRSN_Id"))
                    '_gruppo.tipo = isNullInt(sqlReader.Item("QSGR_Tipo"))
                End While
                sqlReader.Close()
            End Using
        End Using

        Return _gruppo

    End Function

End Class
