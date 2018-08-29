Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common


Namespace Comol.DAL.StandardDB
	Public Class DALpersona
		Inherits DALabstract

		Public Sub New(ByVal oDbconnection As ConnectionDB)
			MyBase.new(oDbconnection)
		End Sub



		Public Function GetDefaultRoleID(ByVal PersonID As Integer) As Integer
			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
			Dim iResponse As Integer = Main.TipoRuoloStandard.AccessoNonAutenticato

			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = "sp_Persona_GetDefaultRoleID"
					Dim dbCommand As DbCommand = oDatabase.GetStoredProcCommand(sqlCommand)
					oDatabase.AddInParameter(dbCommand, "PersonaID", DbType.Int32, PersonID)
					oDatabase.AddOutParameter(dbCommand, "RoleID", DbType.Int32, 11)
					dbCommand.Connection = connection

					dbCommand.ExecuteNonQuery()
					iResponse = oDatabase.GetParameterValue(dbCommand, "RoleID")
				Catch ex As Exception
					If connection.State <> ConnectionState.Closed Then
						connection.Close()
					End If
				End Try

			End Using
			Return iResponse
		End Function



		Public Function GetCommunitiesPermessionForService(ByVal PersonID As Integer, ByVal ServiceCode As String) As IList(Of ServiceBase)
			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
			Dim oLista As New List(Of ServiceBase)

			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = "sp_Persona_GetCommunitiesPermessionForService"
					Dim dbCommand As DbCommand = oDatabase.GetStoredProcCommand(sqlCommand)
					oDatabase.AddInParameter(dbCommand, "PersonaID", DbType.Int32, PersonID)
					oDatabase.AddInParameter(dbCommand, "ServiceCode", DbType.String, ServiceCode)

					dbCommand.Connection = connection

					Using oDatareader As IDataReader = dbCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
						While oDatareader.Read()
							Try
								oLista.Add(New ServiceBase() With {.CommunityID = oDatareader("ComunitaID"), .PermissionString = oDatareader("Permessi")})
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
	End Class
End Namespace