'Imports Microsoft.VisualBasic
'Imports System.Data.SqlClient
'Imports COL_Questionario.RootObject
'Imports System.Text.RegularExpressions
'Imports Microsoft.Practices.EnterpriseLibrary.Data
'Imports Microsoft.Practices.EnterpriseLibrary.Common
'Imports System.Data.Common
'Imports System.Data

'Public Class DALTemplate

'    Public Shared Function readListaTemplate(ByVal idPersona As Integer) As list(Of Template)

'        Dim db As Database = DatabaseFactory.CreateDatabase()
'        Dim listTemplate As New list(Of Template)

'        Using connection As DbConnection = db.CreateConnection()
'            connection.Open()
'            Dim sqlCommand As String = "sp_Questionario_TemplateByPersona_Select"
'            Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
'            dbCommand.Connection = connection
'            db.AddInParameter(dbCommand, "tipo", DbType.Int32, Template.tipoTempate.Questionario)
'            db.AddInParameter(dbCommand, "idPersona", DbType.Int32, idPersona)

'            Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
'                While sqlReader.Read()
'                    Dim oTemplate As New Template
'                    oTemplate.id = isNullInt(sqlReader.Item("TMPL_Id"))
'                    oTemplate.nome = isNullString(sqlReader.Item("TMPL_Nome"))
'                    'oTemplate.testo = isNullString(sqlReader.Item("TMPL_Testo"))
'                    'oTemplate.titolo = isNullString(sqlReader.Item("TMPL_Titolo"))
'                    'oTemplate.idPersona = isNullInt(sqlReader.Item("TMPL_PRSN_Id"))
'                    listTemplate.Add(oTemplate)
'                End While
'            End Using
'            connection.Close()
'        End Using

'        Return listTemplate
'    End Function

'    Public Shared Function readTemplateByID(ByVal id As Integer) As Template

'        Dim db As Database = DatabaseFactory.CreateDatabase()
'        Dim oTemplate As New Template

'        Using connection As DbConnection = db.CreateConnection()
'            connection.Open()
'            Dim sqlCommand As String = "sp_Questionario_TemplateByID_Select"
'            Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
'            dbCommand.Connection = connection
'            db.AddInParameter(dbCommand, "idTemplate", DbType.Int32, id)

'            Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
'                While sqlReader.Read()
'                    oTemplate.id = isNullInt(sqlReader.Item("TMPL_Id"))
'                    oTemplate.nome = isNullString(sqlReader.Item("TMPL_Nome"))
'                    oTemplate.testo = isNullString(sqlReader.Item("TMPL_Testo"))
'                    oTemplate.titolo = isNullString(sqlReader.Item("TMPL_Titolo"))
'                    oTemplate.idPersona = isNullInt(sqlReader.Item("TMPL_PRSN_Id"))
'                End While
'            End Using
'            connection.Close()
'        End Using

'        Return oTemplate
'    End Function

'    Public Shared Function Salva(ByRef oTemp As Template) As String
'        Dim retVal As String

'        If oTemp.id > 0 Then
'            retVal = Update(oTemp)
'        Else
'            retVal = Insert(oTemp)
'        End If

'        Return retVal
'    End Function

'    Public Shared Function Insert(ByRef oTemp As Template) As String
'        Dim retVal As String = String.Empty

'        Dim db As Database = DatabaseFactory.CreateDatabase()

'        Dim sqlCommand As String = "sp_Questionario_Template_Insert"
'        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

'        db.AddInParameter(dbCommand, "tipo", DbType.Int32, oTemp.tipo)
'        db.AddInParameter(dbCommand, "testo", DbType.String, oTemp.testo)
'        db.AddInParameter(dbCommand, "titolo", DbType.String, oTemp.titolo)
'        db.AddInParameter(dbCommand, "idLingua", DbType.Int32, oTemp.idLingua)
'        db.AddInParameter(dbCommand, "idPersona", DbType.Int32, setNull(oTemp.idPersona))
'        db.AddInParameter(dbCommand, "nome", DbType.String, oTemp.nome)

'        Try
'            db.ExecuteNonQuery(dbCommand)
'        Catch ex As Exception
'            retVal = ex.Message
'        End Try

'        Return retVal
'    End Function

'    Public Shared Function Update(ByRef oTemp As Template) As String
'        Dim retVal As String = String.Empty

'        Dim db As Database = DatabaseFactory.CreateDatabase()

'        Dim sqlCommand As String = "sp_Questionario_Template_Update"
'        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

'        db.AddInParameter(dbCommand, "idTemplate", DbType.Int32, oTemp.id)
'        db.AddInParameter(dbCommand, "tipo", DbType.Int32, oTemp.tipo)
'        db.AddInParameter(dbCommand, "testo", DbType.String, oTemp.testo)
'        db.AddInParameter(dbCommand, "titolo", DbType.String, oTemp.titolo)
'        db.AddInParameter(dbCommand, "idLingua", DbType.Int32, oTemp.idLingua)
'        db.AddInParameter(dbCommand, "idPersona", DbType.Int32, setNull(oTemp.idPersona))
'        db.AddInParameter(dbCommand, "nome", DbType.String, oTemp.nome)

'        Try
'            db.ExecuteNonQuery(dbCommand)
'        Catch ex As Exception
'            retVal = ex.Message
'        End Try

'        Return retVal
'    End Function

'    Public Shared Function Elimina(ByRef idTemplate As Integer) As String
'        Dim retVal As String = String.Empty

'        Dim db As Database = DatabaseFactory.CreateDatabase()

'        Dim sqlCommand As String = "sp_Questionario_Template_Delete"
'        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

'        db.AddInParameter(dbCommand, "idTemplate", DbType.Int32, idTemplate)

'        Try
'            db.ExecuteNonQuery(dbCommand)
'        Catch ex As Exception
'            retVal = ex.Message
'        End Try

'        Return retVal
'    End Function

'End Class
