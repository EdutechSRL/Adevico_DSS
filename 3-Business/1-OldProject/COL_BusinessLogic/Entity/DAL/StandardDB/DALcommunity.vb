Imports Comol.Entity

Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common

Namespace Comol.DAL.StandardDB
	Public Class DALcommunity
		Inherits DALabstract
		Implements iDALbase(Of Community)

		Public Sub New(ByVal oDbconnection As ConnectionDB)
			MyBase.new(oDbconnection)
		End Sub

		Public Function Add(ByVal oObject As Community) As Community Implements iDALbase(Of Community).Add
			Return Nothing
		End Function
		Public Sub Delete(ByVal oObject As Community) Implements iDALbase(Of Community).Delete

		End Sub
		Public Function List(Optional ByVal Lingua As Lingua = Nothing) As System.Collections.Generic.List(Of Community) Implements iDALbase(Of Community).List
			Return Nothing
		End Function
		Public Sub Save(ByVal oObject As Community) Implements iDALbase(Of Community).Save

		End Sub


		Public Function FilterList_StatusAdmin(Optional ByVal ServiceCode As String = "") As System.Collections.Generic.List(Of CommunityStatus)
			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
			Dim oLista As New List(Of CommunityStatus)

			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = SQL_FilterList_StatusAdmin(ServiceCode)
					Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand(sqlCommand)
					If ServiceCode <> "" Then
						oDatabase.AddInParameter(dbCommand, "ServiceCode", DbType.String, ServiceCode)
					End If
					dbCommand.Connection = connection

					Using oDatareader As IDataReader = dbCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
						While oDatareader.Read()
							Try
								If CBool(oDatareader("CMNT_Bloccata")) = True Then
									oLista.Add(CommunityStatus.ClosedByAdministration)
								ElseIf oDatareader("CMNT_Archiviata") = True Then
									oLista.Add(CommunityStatus.Archiviata)
								Else
									oLista.Add(CommunityStatus.OnlyActivated)
								End If
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
		Public Function FilterList_StatusForSubscripted(ByVal UserID As Integer, Optional ByVal ServiceCode As String = "") As System.Collections.Generic.List(Of CommunityStatus)
			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
			Dim oLista As New List(Of CommunityStatus)

			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = SQL_FilterList_StatusForSubscripted(ServiceCode)
					Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand(sqlCommand)
					oDatabase.AddInParameter(dbCommand, "PersonID", DbType.Int32, UserID)
					If ServiceCode <> "" Then
						oDatabase.AddInParameter(dbCommand, "ServiceCode", DbType.String, ServiceCode)
					End If
					dbCommand.Connection = connection

					Using oDatareader As IDataReader = dbCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
						While oDatareader.Read()
							Try
								If CBool(oDatareader("CMNT_Bloccata")) = True Then
									oLista.Add(CommunityStatus.ClosedByAdministration)
								ElseIf oDatareader("CMNT_Archiviata") = True Then
									oLista.Add(CommunityStatus.Archiviata)
								Else
									oLista.Add(CommunityStatus.OnlyActivated)
								End If
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

		Private ReadOnly Property SQL_FilterList_StatusAdmin(Optional ByVal ServiceCode As String = "") As String
			Get
				If ServiceCode = "" Then
					Return "SELECT  CMNT_Archiviata, CMNT_Bloccata FROM COMUNITA INNER JOIN SERVIZIO_COMUNITA ON CMNT_id = SRVC_CMNT_ID INNER JOIN " _
					  & " SERVIZIO ON SRVC_SRVZ_ID = SRVZ_id " _
					 & " WHERE SRVZ_Attivato = 1 AND SRVC_isAbilitato = 1 and RLPC_PRSN_ID=:PersonID " _
					 & " GROUP BY CMNT_Archiviata, CMNT_Bloccata"
				Else
					Return "SELECT  CMNT_Archiviata, CMNT_Bloccata FROM COMUNITA INNER JOIN SERVIZIO_COMUNITA ON CMNT_id = SRVC_CMNT_ID INNER JOIN " _
					  & " SERVIZIO ON SRVC_SRVZ_ID = SRVZ_id " _
					 & " WHERE SRVZ_Attivato = 1 AND SRVC_isAbilitato = 1 and RLPC_PRSN_ID=:PersonID AND SRVZ_codice = :ServiceCode " _
					 & " GROUP BY CMNT_Archiviata, CMNT_Bloccata"
				End If
			End Get
		End Property

		Private ReadOnly Property SQL_FilterList_StatusForSubscripted(ByVal UserID As Integer, Optional ByVal ServiceCode As String = "") As String
			Get
				If ServiceCode = "" Then
					Return "SELECT CMNT_Archiviata, CMNT_Bloccata FROM COMUNITA GROUP BY CMNT_Archiviata, CMNT_Bloccata"
				Else
					Return "SELECT CMNT_Archiviata, CMNT_Bloccata " _
					& " FROM         COMUNITA INNER JOIN SERVIZIO_COMUNITA ON CMNT_id = SRVC_CMNT_ID INNER JOIN SERVIZIO ON SRVC_SRVZ_ID = SRVZ_id INNER JOIN LK_RUOLO_PERSONA_COMUNITA ON CMNT_id = RLPC_CMNT_id " _
					& " WHERE   SRVZ_Attivato = 1 AND SRVC_isAbilitato = 1 AND RLPC_abilitato = 1 and RLPC_attivato = 1 and RLPC_TPRL_ID <> -3  AND SRVZ_codice = :ServiceCode)" _
					 & " GROUP BY CMNT_Archiviata, CMNT_Bloccata"
				End If
			End Get
		End Property
	End Class
End Namespace