Imports Comol.Entity
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common

Namespace Comol.DAL.StandardDB
	Public Class DALorganization
		Inherits DALabstract

		Public Sub New(ByVal oDbconnection As ConnectionDB)
			MyBase.new(oDbconnection)
		End Sub

		Function ListLazyOrganization() As List(Of Organization)
			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
			Dim oList As New List(Of Organization)

			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = SQL_LazyOrganizations()
					Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand(sqlCommand)
					dbCommand.Connection = connection

					Using oDatareader As IDataReader = dbCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
						While oDatareader.Read()
							Try
								Dim oOrganization As New Organization(oDatareader("ID"), New CommunityType(oDatareader("TypeID")), oDatareader("OrganizzazioneID"), oDatareader("Name"), True)
								oOrganization.IsArchive = oDatareader("isArchiviata")
								oOrganization.isClosedByAdministration = oDatareader("IsBloccata")
								oOrganization.isFaculty = oDatareader("isFaculty")
								oOrganization.FreeAccess = oDatareader("FreeAccess")
								oOrganization.AccessForPrinting = oDatareader("AccessForPrinting")
								oOrganization.Organization = Nothing
								oList.Add(oOrganization)
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
			Return oList
		End Function
		Function GetAllForAdminFromDal() As List(Of Community)
			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
			Dim oList As New List(Of Community)

			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = SQL_LazyAll()
					Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand(sqlCommand)
					dbCommand.Connection = connection

					Using oDatareader As IDataReader = dbCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
						While oDatareader.Read()
							Try
								Dim oCommunity As New Community With {.ID = oDatareader("ID"), .Organization = New Organization() With {.ID = oDatareader("OrganizzazioneID")}, .Type = New CommunityType(oDatareader("TypeID"))}

								oList.Add(oCommunity)
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
			Return oList
		End Function



		Private ReadOnly Property SQL_LazyOrganizations()
			Get
				Dim iResponse As String = "SELECT   CMNT_id as ID , CMNT_nome as Name, ORGN_isFacolta as isFaculty,CMNT_Archiviata as isArchiviata, CMNT_Bloccata as IsBloccata, CMNT_AccessoLibero as FreeAccess, CMNT_AccessoCopisteria as AccessForPrinting,ORGN_ID as OrganizzazioneID " _
   & " FROM ORGANIZZAZIONE INNER JOIN COMUNITA ON ORGN_id = CMNT_ORGN_id"
				Return iResponse
			End Get
		End Property
		Private ReadOnly Property SQL_LazyAll()
			Get
				Dim iResponse As String = "SELECT CMNT_id as ID , CMNT_nome as Name, CMNT_ORGN_ID as OrganizzazioneID, CMNT_TPCM_ID as TypeID " _
	  & " FROM COMUNITA "
				Return iResponse
			End Get
		End Property
	End Class
End Namespace