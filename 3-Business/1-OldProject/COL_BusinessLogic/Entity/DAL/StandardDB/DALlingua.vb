Imports Comol.Entity
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common

Namespace Comol.DAL.StandardDB
	Public Class DALlingua
		Inherits DALabstract
		Implements iDALbase(Of Lingua)

		Public Sub New(ByVal oDbconnection As ConnectionDB)
			MyBase.new(oDbconnection)
		End Sub


		Public Function List(Optional ByVal oLingua As Lingua = Nothing) As System.Collections.Generic.List(Of Lingua) Implements iDALbase(Of Lingua).List
			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
			Dim oLista As New List(Of Lingua)

			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = "sp_Lingua_Elenca"
					Dim dbCommand As DbCommand = oDatabase.GetStoredProcCommand(sqlCommand)

					dbCommand.Connection = connection

					Using oDatareader As IDataReader = dbCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
						While oDatareader.Read()
							Try
								oLista.Add(New Lingua(oDatareader("LNGU_ID"), GenericValidator.ValString(oDatareader("LNGU_Nome"), ""), GenericValidator.ValString(oDatareader("LNGU_Codice"), ""), GenericValidator.ValBool(oDatareader("LNGU_default"), False), GenericValidator.ValString(oDatareader("LNGU_Icona"), "")))
							Catch ex As Exception

							End Try
						End While
					End Using
				Catch ex As Exception
					If connection.State <> ConnectionState.Closed Then
						connection.Close()
					End If
				End Try
			End Using
			Return oLista
		End Function
		Public Function Add(ByVal oObject As Lingua) As Lingua Implements iDALbase(Of Lingua).Add
			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
			Dim iReturn As New Lingua
			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = "sp_Lingua_Aggiungi"
					Dim dbCommand As DbCommand = oDatabase.GetStoredProcCommand(sqlCommand)
					oDatabase.AddOutParameter(dbCommand, "LNGU_Id", DbType.Int32, 4)
					oDatabase.AddInParameter(dbCommand, "LNGU_nome", DbType.String, oObject.Nome)
					oDatabase.AddInParameter(dbCommand, "LNGU_codice", DbType.String, oObject.Codice)
					oDatabase.AddInParameter(dbCommand, "LNGU_default", DbType.Int32, IIf(oObject.isDefault, 1, 0))
					oDatabase.AddInParameter(dbCommand, "LNGU_Icona", DbType.String, oObject.Icona)
					dbCommand.Connection = connection
					dbCommand.ExecuteNonQuery()
					iReturn = oObject
					iReturn.ID = dbCommand.Parameters("LNGU_Id").Value
				Catch ex As Exception
					iReturn = Nothing
				Finally
					If connection.State <> ConnectionState.Closed Then
						connection.Close()
					End If
				End Try
			End Using
			Return iReturn
		End Function
		Public Sub Save(ByVal oObject As Lingua) Implements iDALbase(Of Lingua).Save
			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)

			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = "sp_Lingua_Modifica"
					Dim dbCommand As DbCommand = oDatabase.GetStoredProcCommand(sqlCommand)
					oDatabase.AddInParameter(dbCommand, "LNGU_Id", DbType.Int32, oObject.ID)
					oDatabase.AddInParameter(dbCommand, "LNGU_nome", DbType.String, oObject.Nome)
					oDatabase.AddInParameter(dbCommand, "LNGU_codice", DbType.String, oObject.Codice)
					oDatabase.AddInParameter(dbCommand, "LNGU_default", DbType.Int32, IIf(oObject.isDefault, 1, 0))
					oDatabase.AddInParameter(dbCommand, "LNGU_Icona", DbType.String, oObject.Icona)
					dbCommand.Connection = connection
					dbCommand.ExecuteNonQuery()
				Catch ex As Exception
				Finally
					If connection.State <> ConnectionState.Closed Then
						connection.Close()
					End If
				End Try

			End Using
		End Sub
		Public Sub Delete(ByVal oObject As Lingua) Implements iDALbase(Of Lingua).Delete

		End Sub
	End Class
End Namespace