Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports System.Data.Common
Imports System.Collections.Generic
Imports System.Data
Imports COL_Questionario.RootObject

Public Class DALPagine

    Public Shared Sub readPagineByIDQuestionario(ByRef oQuestionario As Questionario, ByRef db As Database, ByRef conn As DbConnection)
        Dim sqlCommand As String
        Dim dbCommand As DbCommand
        sqlCommand = "sp_Questionario_PagineByIDQuestionario_Select"
        dbCommand = db.GetStoredProcCommand(sqlCommand)
        dbCommand.Connection = conn
        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, oQuestionario.idQuestionarioMultilingua)
        Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
            While sqlReader.Read()
                Dim oPagina As New QuestionarioPagina
                oPagina.id = isNullInt(sqlReader.Item("QSPG_Id"))
                'oPagina.allaDomanda = Math.Max(1, isNullInt(sqlReader.Item("QSPG_AllaDomanda"))) 'possono esserci pagine vuote!!
                oPagina.allaDomanda = isNullInt(sqlReader.Item("QSPG_AllaDomanda"))
                oPagina.dallaDomanda = isNullInt(sqlReader.Item("QSPG_DallaDomanda"))
                oPagina.idQuestionarioMultilingua = isNullInt(sqlReader.Item("QSPG_QSML_Id"))
                oPagina.nomePagina = isNullString(sqlReader.Item("QSPG_NomePagina"))
                oPagina.numeroPagina = isNullInt(sqlReader.Item("QSPG_NumeroPagina"))
                oPagina.randomOrdineDomande = isNullBoolean(sqlReader.Item("QSPG_RandomOrdineDomande"))
                oPagina.descrizione = isNullString(sqlReader.Item("QSPG_Descrizione"))

                oQuestionario.pagine.Add(oPagina)
            End While
            sqlReader.Close()
        End Using
    End Sub
    Public Shared Function readPaginaByIDQuestionario(ByRef idQuestionarioMultilingua As Integer, ByRef numeroPagina As Integer) As QuestionarioPagina
        'non ottimizzata, sarebbe meglio aggiungere una nuova store con where numeropagina = numeropagina, 
        'lasciato cosi' perche' per ora serve solo nei meeting, che hanno una pagina sola
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim connection As DbConnection = db.CreateConnection()
        Using connection
            connection.Open()
            Dim sqlCommand As String
            Dim dbCommand As DbCommand
            sqlCommand = "sp_Questionario_PagineByIDQuestionario_Select"
            dbCommand = db.GetStoredProcCommand(sqlCommand)
            dbCommand.Connection = connection
            db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuestionarioMultilingua)
            Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                While sqlReader.Read()
                    Dim oPagina As New QuestionarioPagina
                    oPagina.id = isNullInt(sqlReader.Item("QSPG_Id"))
                    oPagina.allaDomanda = isNullInt(sqlReader.Item("QSPG_AllaDomanda"))
                    oPagina.dallaDomanda = isNullInt(sqlReader.Item("QSPG_DallaDomanda"))
                    oPagina.idQuestionarioMultilingua = isNullInt(sqlReader.Item("QSPG_QSML_Id"))
                    oPagina.nomePagina = isNullString(sqlReader.Item("QSPG_NomePagina"))
                    oPagina.numeroPagina = isNullInt(sqlReader.Item("QSPG_NumeroPagina"))
                    oPagina.randomOrdineDomande = isNullBoolean(sqlReader.Item("QSPG_RandomOrdineDomande"))
                    oPagina.descrizione = isNullString(sqlReader.Item("QSPG_Descrizione"))
                    If oPagina.numeroPagina = numeroPagina Then
                        connection.Close()
                        Return oPagina
                    End If
                End While
                sqlReader.Close()
            End Using
            connection.Close()
        End Using
        Return New QuestionarioPagina
    End Function

    Public Shared Function readPaginaByID(ByVal idPagina As String) As QuestionarioPagina
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "sp_Questionario_QuestionarioPaginaByID_Select"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        db.AddInParameter(dbCommand, "idPagina", DbType.Int32, idPagina)
        Dim oPagina As New QuestionarioPagina

        Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
            While sqlReader.Read()
                oPagina.id = isNullInt(sqlReader.Item("QSPG_Id"))
                oPagina.allaDomanda = isNullInt(sqlReader.Item("QSPG_AllaDomanda"))
                oPagina.dallaDomanda = isNullInt(sqlReader.Item("QSPG_DallaDomanda"))
                oPagina.idQuestionarioMultilingua = isNullInt(sqlReader.Item("QSPG_QSML_Id"))
                oPagina.nomePagina = isNullString(sqlReader.Item("QSPG_NomePagina"))
                oPagina.numeroPagina = isNullInt(sqlReader.Item("QSPG_NumeroPagina"))
                oPagina.randomOrdineDomande = isNullBoolean(sqlReader.Item("QSPG_RandomOrdineDomande"))
                oPagina.descrizione = isNullString(sqlReader.Item("QSPG_Descrizione"))

            End While
            sqlReader.Close()
        End Using
        

        Return oPagina
    End Function

    'Public Shared Function readIDPaginaByIDDomanda(ByVal idQuestionario As String, ByVal idDomanda As String) As String

    '    Dim db As Database = DatabaseFactory.CreateDatabase()

    '    Dim sqlCommand As String = "QuestionarioIDPaginaByDomanda_Select"
    '    Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

    '    db.AddInParameter(dbCommand, "idDomanda", DbType.Int32, idDomanda)
    '    db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, idQuestionario)

    '    Dim RetVal As String = ""

    '    RetVal = dbCommand.ExecuteScalar()

    '    Return RetVal
    'End Function

    Public Shared Sub Pagina_Salva(ByRef pag As QuestionarioPagina)

        If pag.id > 0 Then
            Pagina_Update(pag)
        Else
            Pagina_Insert(pag)
        End If

    End Sub

    Public Shared Function Pagina_Update(ByRef sez As QuestionarioPagina) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim sqlCommand As String = "sp_Questionario_QuestionarioPagina_Update"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "idPagina", DbType.Int32, sez.id)
        db.AddInParameter(dbCommand, "dallaDomanda", DbType.Int32, sez.dallaDomanda)
        db.AddInParameter(dbCommand, "allaDomanda", DbType.Int32, sez.allaDomanda)
        db.AddInParameter(dbCommand, "numeroPagina", DbType.Int32, sez.numeroPagina)
        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, sez.idQuestionarioMultilingua)
        db.AddInParameter(dbCommand, "nomePagina", DbType.String, sez.nomePagina)
        db.AddInParameter(dbCommand, "randomOrdineDomande", DbType.Boolean, sez.randomOrdineDomande)
        db.AddInParameter(dbCommand, "descrizione", DbType.String, sez.descrizione)

        Dim RetVal As String = ""

        Try
            RetVal = db.ExecuteNonQuery(dbCommand)
        Catch ex As Exception
            RetVal = ex.Message
        End Try


        Return RetVal
    End Function

    Public Shared Function Pagina_Insert(ByRef sez As QuestionarioPagina) As String


        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim sqlCommand As String = "sp_Questionario_QuestionarioPagina_Insert"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "dallaDomanda", DbType.Int32, sez.dallaDomanda)
        db.AddInParameter(dbCommand, "allaDomanda", DbType.Int32, sez.allaDomanda)
        db.AddInParameter(dbCommand, "numeroPagina", DbType.Int32, sez.numeroPagina)
        db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, sez.idQuestionarioMultilingua)
        db.AddInParameter(dbCommand, "nomePagina", DbType.String, sez.nomePagina)
        db.AddInParameter(dbCommand, "randomOrdineDomande", DbType.Boolean, sez.randomOrdineDomande)
        db.AddInParameter(dbCommand, "descrizione", DbType.String, sez.descrizione)


        Dim RetVal As String = ""

        Try
            RetVal = db.ExecuteNonQuery(dbCommand)
        Catch ex As Exception
            RetVal = ex.Message
        End Try

        Return RetVal
    End Function

    Public Shared Function Pagina_Delete(ByVal oquest As Questionario, ByRef idPagina As Integer, ByVal numeroPagina As Integer) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RetVal As String = ""

        Try

            Using conn As DbConnection = db.CreateConnection()
                conn.Open()
                ' elimino la pagina vuota
                Dim sqlCommand As String = "sp_Questionario_QuestionarioPagina_Delete"
                Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
                dbCommand.Connection = conn
                db.AddInParameter(dbCommand, "idPagina", DbType.Int32, idPagina)

                RetVal = db.ExecuteNonQuery(dbCommand)

                For Each oPag As QuestionarioPagina In oquest.pagine
                    If oPag.numeroPagina > numeroPagina Then
                        oPag.numeroPagina = oPag.numeroPagina - 1
                        ' aggiorno il numero pagina delle pagine successive
                        Dim sqlCommand2 As String = "sp_Questionario_QuestionarioPagina_Update"
                        Dim dbCommand2 As DbCommand = db.GetStoredProcCommand(sqlCommand2)

                        db.AddInParameter(dbCommand2, "idPagina", DbType.Int32, oPag.id)
                        db.AddInParameter(dbCommand2, "dallaDomanda", DbType.Int32, oPag.dallaDomanda)
                        db.AddInParameter(dbCommand2, "allaDomanda", DbType.Int32, oPag.allaDomanda)
                        db.AddInParameter(dbCommand2, "numeroPagina", DbType.Int32, oPag.numeroPagina)
                        db.AddInParameter(dbCommand2, "idQuestionario", DbType.Int32, oPag.idQuestionarioMultilingua)
                        db.AddInParameter(dbCommand2, "nomePagina", DbType.String, oPag.nomePagina)
                        db.AddInParameter(dbCommand2, "randomOrdineDomande", DbType.Boolean, oPag.randomOrdineDomande)
                        db.AddInParameter(dbCommand2, "descrizione", DbType.String, oPag.descrizione)

                        RetVal = db.ExecuteNonQuery(dbCommand2)
                    End If
                Next


                conn.Close()
            End Using

        Catch ex As Exception
            RetVal = ex.Message
        End Try

        Return RetVal
    End Function



End Class
