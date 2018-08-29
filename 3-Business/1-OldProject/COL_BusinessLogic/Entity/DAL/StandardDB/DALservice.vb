Imports Comol.Entity
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common

Namespace Comol.DAL.StandardDB
	Public Class DALservice
		Inherits DALabstract

		Public Sub New(ByVal oDbconnection As ConnectionDB)
			MyBase.new(oDbconnection)
		End Sub

		Public Function SmallList() As System.Collections.Generic.List(Of PlainService)
			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
			Dim oLista As New List(Of PlainService)

			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = "SELECT SRVZ_id,SRVZ_nome,SRVZ_Codice FROM SERVIZIO "
					Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand(sqlCommand)

					dbCommand.Connection = connection

					Using oDatareader As IDataReader = dbCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
						While oDatareader.Read()
							Try
								oLista.Add(New PlainService(oDatareader("SRVZ_id"), GenericValidator.ValString(oDatareader("SRVZ_Codice"), ""), "00000000000000000000000000000000"))
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
        Public Function SystemTranslated(ByVal LanguageID As Integer) As System.Collections.Generic.List(Of PlainService)
            Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
            Dim oLista As New List(Of PlainService)

            Using connection As DbConnection = oDatabase.CreateConnection()
                connection.Open()

                Try
                    Dim sqlCommand As String = "SELECT SRVZ_id as ID,dbo.ufn_GetValueByLingua(" & LanguageID.ToString & ",'SRVZ_nome','Servizio',SRVZ_id) as Name,SRVZ_Codice as Code FROM SERVIZIO "
                    Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand(sqlCommand)

                    dbCommand.Connection = connection

                    Using oDatareader As IDataReader = dbCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                        While oDatareader.Read()
                            Try
                                oLista.Add(New PlainService(oDatareader("ID"), GenericValidator.ValString(oDatareader("Name"), ""), GenericValidator.ValString(oDatareader("Code"), ""), "00000000000000000000000000000000"))
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
            Return (From s As PlainService In oLista Order By s.Name Select s).ToList()
        End Function
        Public Function TranslatedList(ByVal CommunityID As Integer, ByVal LanguageID As Integer) As System.Collections.Generic.List(Of PlainService)
            Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
            Dim oLista As New List(Of PlainService)

            Using connection As DbConnection = oDatabase.CreateConnection()
                connection.Open()

                Try

                    Dim sqlCommand As String = "SELECT SRVZ_id as ID,dbo.ufn_GetValueByLingua(" & LanguageID.ToString & ",'SRVZ_nome','Servizio',SRVZ_id) as Name,SRVZ_Codice as Code FROM SERVIZIO " _
                    & " INNER JOIN SERVIZIO_COMUNITA ON SRVZ_id=SRVC_SRVZ_ID where SRVC_CMNT_ID=" & CommunityID.ToString

                    Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand(sqlCommand)
                    dbCommand.Connection = connection

                    Using oDatareader As IDataReader = dbCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                        While oDatareader.Read()
                            Try
                                oLista.Add(New PlainService(oDatareader("ID"), GenericValidator.ValString(oDatareader("Name"), ""), GenericValidator.ValString(oDatareader("Code"), ""), "00000000000000000000000000000000"))
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

        Public Function RoleTranslatedList(ByVal RoleID As Integer, ByVal CommunityID As Integer, ByVal LanguageID As Integer) As System.Collections.Generic.List(Of PlainService)
            Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
            Dim oLista As New List(Of PlainService)

            Using connection As DbConnection = oDatabase.CreateConnection()
                connection.Open()

                Try

                    '                    SELECT     SERVIZIO_COMUNITA_1.SRVC_CMNT_ID, SERVIZIO_COMUNITA_1.SRVC_isAbilitato, dbo.SERVIZIO.SRVZ_id, dbo.SERVIZIO.SRVZ_nome, 
                    '                      dbo.SERVIZIO.SRVZ_codice
                    'FROM         dbo.SERVIZIO_COMUNITA AS SERVIZIO_COMUNITA_1 INNER JOIN
                    '                      dbo.SERVIZIO ON SERVIZIO_COMUNITA_1.SRVC_SRVZ_ID = dbo.SERVIZIO.SRVZ_id

                    Dim sqlCommand As String = "SELECT SRVZ_id as ID,dbo.ufn_GetValueByLingua(" & LanguageID.ToString & ",'SRVZ_nome','Servizio',SRVZ_id) as Name,SRVZ_Codice as Code,LKSC_Permessi FROM SERVIZIO " _
                  & " INNER JOIN LK_SERVIZIO_COMUNITA ON SRVZ_id=LKSC_SRVZ_id where LKSC_CMNT_id=" & CommunityID.ToString & " AND LKSC_TPRL_id = " & RoleID.ToString

                    Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand(sqlCommand)

                    dbCommand.Connection = connection

                    Using oDatareader As IDataReader = dbCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                        While oDatareader.Read()
                            Try
                                oLista.Add(New PlainService(oDatareader("ID"), GenericValidator.ValString(oDatareader("Name"), ""), GenericValidator.ValString(oDatareader("Code"), ""), GenericValidator.ValString(oDatareader("LKSC_Permessi"), "00000000000000000000000000000000")))
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



        Public Function LoadModulePermissions(ByVal IdLanguage As Integer, ByVal IdModule As Integer) As List(Of PlainPermission)
            Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
            Dim oLista As New List(Of PlainPermission)

            Using connection As DbConnection = oDatabase.CreateConnection()
                connection.Open()

                Try
                    Dim sqlCommand As String = "SELECT PRMS_id, dbo.ufn_GetValueByLingua(" & IdLanguage & ",'LKPR_Nome','LK_PRMS_SRVZ',LKPR_ID) as Name, LKPR_Nome as NomeDefault, " _
                        & " dbo.ufn_GetValueByLingua(" & IdLanguage & ",'LKPR_Descrizione','LK_PRMS_SRVZ',LKPR_ID) as Descrizione, PRMS_Posizione, LKPR_Descrizione  as DescrizioneDefault " _
                        & " FROM PERMESSO INNER JOIN LK_PRMS_SRVZ ON PRMS_id = LKPR_PRMS_id WHERE LKPR_SRVZ_id=" & IdModule.ToString

                    Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand(sqlCommand)
                    dbCommand.Connection = connection

                    Using oDatareader As IDataReader = dbCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                        While oDatareader.Read()
                            Try
                                Dim permission As New PlainPermission
                                permission.Id = oDatareader("PRMS_id")
                                permission.Name = GenericValidator.ValString(oDatareader("Name"), oDatareader("NomeDefault"))
                                permission.Description = GenericValidator.ValString(oDatareader("Descrizione"), oDatareader("DescrizioneDefault"))
                                permission.Value = oDatareader("PRMS_Posizione")
                                oLista.Add(permission)
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
            Return oLista.OrderBy(Function(p) p.Name).ToList()
        End Function
    End Class
End Namespace