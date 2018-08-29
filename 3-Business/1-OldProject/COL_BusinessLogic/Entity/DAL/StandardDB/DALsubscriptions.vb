Imports Comol.Entity

Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common

Namespace Comol.DAL.StandardDB
	Public Class DALsubscriptions
		Inherits DALabstract

		Public Sub New(ByVal oDbconnection As ConnectionDB)
			MyBase.new(oDbconnection)
		End Sub
        Public Shared Function isNullString(ByVal obj As Object) As String

            If obj Is Nothing Or obj Is System.DBNull.Value Then
                Return String.Empty
            Else
                Return obj.ToString()
            End If

        End Function
#Region "ucSearchUser"
        Private Function concatenateListOfInt(ByRef column As String, ByRef oListOfInt As List(Of Integer)) As String
            Dim concatenated As String = String.Empty
            Dim isFirst As Boolean = True
            If Not oListOfInt Is Nothing Then
                For Each oListElement As Integer In oListOfInt
                    If isFirst Then
                        concatenated &= column & oListElement
                        isFirst = False
                    Else
                        concatenated &= " OR " & column & oListElement
                    End If
                Next
            End If
			If String.IsNullOrEmpty(concatenated) Then
				concatenated = " 1=1 "
			End If
			Return concatenated
		End Function
        Function ListRolesByCommunity(ByVal LinguaID As Integer, ByVal ListCommunityID As List(Of Integer)) As List(Of Role)

            Dim oRoleList As New List(Of Role)

            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim dbCommand As DbCommand

            Using connection As DbConnection = db.CreateConnection()
                connection.Open()

                Dim sqlCommand As String

                sqlCommand = "SELECT   TPRL_id, dbo.ufn_GetValueByLingua(" & LinguaID.ToString & ",'TPRL_nome','TIPO_RUOLO',TPRL_Id) as TPRL_nome, TPRL_Gerarchia" & _
                " FROM dbo.TIPO_RUOLO INNER JOIN  LK_RUOLO_PERSONA_COMUNITA ON TPRL_id = RLPC_TPRL_id WHERE TPRL_ID > -1 and " & _
                concatenateListOfInt("RLPC_CMNT_id = ", ListCommunityID) & " GROUP BY TPRL_id, TPRL_nome,TPRL_Gerarchia ORDER BY TPRL_nome"

                dbCommand = db.GetSqlStringCommand(sqlCommand)
                dbCommand.Connection = connection

                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim oRole As New Role
                        oRole.ID = sqlReader.Item("TPRL_id")
                        oRole.Name = sqlReader.Item("TPRL_nome")
                        oRole.PriorityOrder = sqlReader.Item("TPRL_Gerarchia")

                        oRoleList.Add(oRole)
                    End While
                End Using

                connection.Close()
            End Using

            Return oRoleList
        End Function
        Function ListRolesAvailableByCommunity(ByVal LinguaID As Integer, ByVal ListCommunityID As List(Of Integer)) As List(Of Role)

            Dim oRoleList As New List(Of Role)

            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim dbCommand As DbCommand

            Using connection As DbConnection = db.CreateConnection()
                connection.Open()

                Dim sqlCommand As String

                sqlCommand = "SELECT   TPRL_id, dbo.ufn_GetValueByLingua(" & LinguaID.ToString & ",'TPRL_nome','TIPO_RUOLO',TPRL_Id) as TPRL_nome, TPRL_Gerarchia FROM      TIPO_RUOLO INNER JOIN   LK_TPRL_TPCM ON TPRL_id = LKTT_TPRL_id INNER JOIN  TIPO_COMUNITA ON LKTT_TPCM_id = TPCM_id INNER JOIN  COMUNITA ON TPCM_id = CMNT_TPCM_id WHERE    TPRL_ID <>-4 and   " & concatenateListOfInt("CMNT_id = ", ListCommunityID) & " AND  TPRL_id  <> -3 AND  TPRL_id  <> -2 and TPRL_ID<>-4  GROUP BY TPRL_id, TPRL_nome,TPRL_gerarchia ORDER BY TPRL_nome"

                dbCommand = db.GetSqlStringCommand(sqlCommand)
                dbCommand.Connection = connection

                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim oRole As New Role
                        oRole.ID = sqlReader.Item("TPRL_id")
                        oRole.Name = sqlReader.Item("TPRL_nome")
                        oRole.PriorityOrder = sqlReader.Item("TPRL_Gerarchia")

                        oRoleList.Add(oRole)
                    End While
                End Using

                connection.Close()
            End Using

            Return oRoleList
        End Function

        Function ConcatenatedCommunitiesId(ByRef CommunitiesId As List(Of Integer)) As String
            'se viene passata la comunita' -1 ritorna tutti gli utenti del sistema
            If CommunitiesId.Count = 1 AndAlso CommunitiesId(0) = -1 Then
                Return String.Empty
            Else
                Return "AND (" & concatenateListOfInt("LK_RUOLO_PERSONA_COMUNITA.RLPC_CMNT_id=", CommunitiesId) & ")"
            End If
        End Function
        Function ListForFilterUser(ByRef CommunitiesId As List(Of Integer)) As List(Of MemberContact)
            Dim filteredUsersList As New List(Of MemberContact)

            Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)

            Using connection As DbConnection = oDatabase.CreateConnection()
                connection.Open()

                Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand("SELECT distinct PERSONA.PRSN_id, PERSONA.PRSN_login, PERSONA.PRSN_nome, PERSONA.PRSN_cognome, PERSONA.PRSN_mail, " _
                 & "LK_RUOLO_PERSONA_COMUNITA.RLPC_TPRL_id, STUDENTE.STDN_matricola, LK_RUOLO_PERSONA_COMUNITA.RLPC_attivato, " _
                 & "LK_RUOLO_PERSONA_COMUNITA.RLPC_abilitato, LK_RUOLO_PERSONA_COMUNITA.RLPC_responsabile " _
                 & "FROM LK_RUOLO_PERSONA_COMUNITA INNER JOIN PERSONA ON LK_RUOLO_PERSONA_COMUNITA.RLPC_PRSN_id = PERSONA.PRSN_id " _
                 & "LEFT JOIN STUDENTE ON PERSONA.PRSN_id = STUDENTE.STDN_PRSN_id WHERE " _
                 & "LK_RUOLO_PERSONA_COMUNITA.RLPC_TPRL_id > -1 " & ConcatenatedCommunitiesId(CommunitiesId) & " ORDER BY PERSONA.PRSN_id")

                dbCommand.Connection = connection

                Using sqlReader As IDataReader = dbCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                    While sqlReader.Read()
                        Dim oMembershipInfo As New MembershipInfo
                        Dim id As Integer = sqlReader("PRSN_id")
                        Dim oRole As New Role
                        Dim oMemberContact As New MemberContact

                        oRole.ID = sqlReader("RLPC_TPRL_id")
                        'oMemberContact.Selected = oConfirmedUserIdList.Contains(id)	'gli utenti aggiunti alla selezione sono ri-checkati

                        oMembershipInfo.MemberID = sqlReader("PRSN_id")
                        oMembershipInfo.MemberRole = oRole
                        If Not sqlReader("RLPC_abilitato") Then
                            oMembershipInfo.Status = SubscriptionStatus.Disabled
                        End If
                        If sqlReader("RLPC_attivato") Then
                            oMembershipInfo.Status = SubscriptionStatus.Active
                        Else
                            oMembershipInfo.Status = SubscriptionStatus.WaitingForActivation
                        End If
                        If sqlReader("RLPC_responsabile") Then
                            oMembershipInfo.isResponsible = True
                        End If
                        'la store torna l'elenco con i risultati ordinati per id. N id uguali di seguito significano che ho uno stesso utente con N ruoli.
                        If filteredUsersList.Count > 0 Then
                            If filteredUsersList.Item(filteredUsersList.Count - 1).Id = id Then
                                filteredUsersList.Item(filteredUsersList.Count - 1).MembershipInfo.Add(oMembershipInfo)
                            Else
                                oMemberContact.Id = id
                                oMemberContact.Name = isNullString(sqlReader("PRSN_nome"))
                                oMemberContact.Surname = isNullString(sqlReader("PRSN_cognome"))
                                oMemberContact.Mail = isNullString(sqlReader("PRSN_mail"))
                                oMemberContact.Login = isNullString(sqlReader("PRSN_login"))
                                oMemberContact.RegistrationCode = isNullString(sqlReader("STDN_matricola"))
                                oMemberContact.MembershipInfo.Add(oMembershipInfo)
                                filteredUsersList.Add(oMemberContact)
                            End If
                        Else
                            oMemberContact.Id = id
                            oMemberContact.Name = isNullString(sqlReader("PRSN_nome"))
                            oMemberContact.Surname = isNullString(sqlReader("PRSN_cognome"))
                            oMemberContact.Mail = isNullString(sqlReader("PRSN_mail"))
                            oMemberContact.Login = isNullString(sqlReader("PRSN_login"))
                            oMemberContact.RegistrationCode = isNullString(sqlReader("STDN_matricola"))
                            oMemberContact.MembershipInfo.Add(oMembershipInfo)
                            filteredUsersList.Add(oMemberContact)
                        End If
                    End While
                End Using
                connection.Close()
            End Using
            Return filteredUsersList
        End Function
        ' serve per comporre la stringa sql, ma solo se il select vien fatto su DB
        'Private ReadOnly Property SQL_ListForFilterUser(ByVal CommunitiesId As List(Of Integer), ByVal Name As String, ByVal Surname As String, ByVal RegistrationCode As String, ByVal MailAddress As String, ByVal Login As String, ByVal SelectedRoleId As Integer)
        '    Get
        '        Dim sqlCommand As String

        '        sqlCommand = "SELECT distinct PERSONA.PRSN_id, PERSONA.PRSN_login, PERSONA.PRSN_nome, PERSONA.PRSN_cognome, PERSONA.PRSN_mail, " _
        '            & "LK_RUOLO_PERSONA_COMUNITA.RLPC_TPRL_id, STUDENTE.STDN_matricola, LK_RUOLO_PERSONA_COMUNITA.RLPC_attivato, " _
        '            & "LK_RUOLO_PERSONA_COMUNITA.RLPC_abilitato, LK_RUOLO_PERSONA_COMUNITA.RLPC_responsabile " _
        '            & "FROM LK_RUOLO_PERSONA_COMUNITA INNER JOIN PERSONA ON LK_RUOLO_PERSONA_COMUNITA.RLPC_PRSN_id = PERSONA.PRSN_id " _
        '            & "INNER JOIN STUDENTE ON PERSONA.PRSN_id = STUDENTE.STDN_PRSN_id WHERE "

        '        If Not isNothingOrEmpty(Name) Then
        '            sqlCommand &= "PERSONA.PRSN_nome = @Name AND "
        '        End If
        '        If Not isNothingOrEmpty(Surname) Then
        '            sqlCommand &= "PERSONA.PRSN_cognome = @Surname AND "
        '        End If
        '        If Not isNothingOrEmpty(MailAddress) Then
        '            sqlCommand &= "PERSONA.PRSN_mail = @MailAddress AND "
        '        End If
        '        If Not isNothingOrEmpty(RegistrationCode) Then
        '            sqlCommand &= "STUDENTE.STDN_matricola = @RegistrationCode AND "
        '        End If
        '        If Not isNothingOrEmpty(Login) Then
        '            sqlCommand &= "PERSONA.PRSN_login = @Login AND "
        '        End If
        '        If Not isNothingOrEmpty(SelectedRoleId) Then
        '            If SelectedRoleId > 1 Then
        '                sqlCommand &= "LK_RUOLO_PERSONA_COMUNITA.RLPC_TPRL_id = @SelectedRoleId AND "
        '            End If
        '        End If
        '        sqlCommand &= "LK_RUOLO_PERSONA_COMUNITA.RLPC_TPRL_id > -1 AND "
        '        If CommunitiesId.Count > 0 Then
        '            sqlCommand &= "(" & concatenateListOfInt("LK_RUOLO_PERSONA_COMUNITA.RLPC_CMNT_id=", CommunitiesId) & ") AND "
        '        End If

        '        sqlCommand &= "PERSONA.PRSN_id = PERSONA.PRSN_id ORDER BY PERSONA.PRSN_id"

        '        Return sqlCommand
        '    End Get
        'End Property
#End Region
        Public Function FiltersOrganization(ByVal oPerson As Person, ByVal oClause As GenericClause(Of ServiceClause), Optional ByVal OnlyAvailable As Boolean = True) As List(Of FilterElement)

            Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
            Dim oListServices As New List(Of EntityBaseServices(Of FilterElement))

            Using connection As DbConnection = oDatabase.CreateConnection()
                connection.Open()

                Try
                    Dim sqlCommand As String = SQL_ServicesFilterOrganization(oClause, OnlyAvailable)
                    Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand(sqlCommand)
                    oDatabase.AddInParameter(dbCommand, "@PersonID", DbType.Int32, oPerson.ID)
                    dbCommand.Connection = connection

                    Using oDatareader As IDataReader = dbCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                        oListServices = GenerateEntityServices("OrganizzazioneID", "Name", oDatareader)
                    End Using
                Catch ex As Exception
                    If connection.State <> ConnectionState.Closed Then
                        connection.Close()
                    End If
                End Try
            End Using

            Return FilterElementsByPermission(oListServices, oClause)
        End Function
		Public Function FiltersCommunityStatus(ByVal oPerson As Person, ByVal OrganizationID As Integer, ByVal oClause As GenericClause(Of ServiceClause), Optional ByVal OnlyAvailable As Boolean = True) As List(Of FilterElement)

			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
			Dim oListServices As New List(Of EntityBaseServices(Of FilterElement))

			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = SQL_ServicesFilterStatus(oClause, OrganizationID)
					Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand(sqlCommand)
					oDatabase.AddInParameter(dbCommand, "@PersonID", DbType.Int32, oPerson.ID)
					dbCommand.Connection = connection

					Using oDatareader As IDataReader = dbCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
						While oDatareader.Read()
							Dim oItem As FilterElement = Nothing
							Try
								If oDatareader("IsBloccata") Then
									oItem = New FilterElement(CommunityStatus.ClosedByAdministration, "")
								ElseIf oDatareader("isArchiviata") Then
									oItem = New FilterElement(CommunityStatus.Archiviata, "")
								Else
									oItem = New FilterElement(CommunityStatus.OnlyActivated, "")
								End If
							Catch ex As Exception

							End Try

							If oListServices.Count <> 0 Then
								Dim oFind As EntityBaseServices(Of FilterElement) = (From o In oListServices Where o.Entity.Value = oItem.Value).FirstOrDefault
								If IsNothing(oFind) Then
									oListServices.Add(New EntityBaseServices(Of FilterElement)(oItem, CreateServiceBase(oDatareader)))
								Else
									oFind.Services.Add(CreateServiceBase(oDatareader))
								End If
							Else
								oListServices.Add(New EntityBaseServices(Of FilterElement)(oItem, CreateServiceBase(oDatareader)))
							End If
						End While
					End Using
				Catch ex As Exception
					If connection.State <> ConnectionState.Closed Then
						connection.Close()
					End If
				End Try
			End Using

			Return FilterElementsByPermission(oListServices, oClause)
		End Function
		Public Function FiltersCommunityType(ByVal oPerson As Person, ByVal oClause As GenericClause(Of ServiceClause), ByVal LanguageID As Integer, ByVal OrganizationID As Integer, ByVal oStatus As CommunityStatus, Optional ByVal OnlyAvailable As Boolean = True) As List(Of FilterElement)
			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
			Dim oListServices As New List(Of EntityBaseServices(Of FilterElement))

			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = SQL_ServicesFilterCommunityType(oClause, OrganizationID, oStatus)
					Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand(sqlCommand)
					oDatabase.AddInParameter(dbCommand, "@LinguaID", DbType.Int32, LanguageID)
					oDatabase.AddInParameter(dbCommand, "@PersonID", DbType.Int32, oPerson.ID)
					dbCommand.Connection = connection

					Using oDatareader As IDataReader = dbCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
						oListServices = GenerateEntityServices("ID", "Name", oDatareader)
					End Using
				Catch ex As Exception
					If connection.State <> ConnectionState.Closed Then
						connection.Close()
					End If
				End Try
			End Using

			Return FilterElementsByPermission(oListServices, oClause)
		End Function
		Public Function FiltersAccademicYear(ByVal oPerson As Person, ByVal OrganizationID As Integer, ByVal oStatus As CommunityStatus, ByVal oClause As GenericClause(Of ServiceClause), Optional ByVal OnlyAvailable As Boolean = True) As List(Of FilterElement)
			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
			Dim oListServices As New List(Of EntityBaseServices(Of FilterElement))

			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = SQL_ServicesFilterAccademiYear(oClause, OrganizationID, oStatus)
					Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand(sqlCommand)
					oDatabase.AddInParameter(dbCommand, "@PersonID", DbType.Int32, oPerson.ID)
					dbCommand.Connection = connection

					Using oDatareader As IDataReader = dbCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
						While oDatareader.Read()
							Dim oAccademycYear As New AcademicYear(oDatareader("Anno"))

							Dim oItem As New FilterElement(oAccademycYear.Year, oAccademycYear.LongDescription)
							If oListServices.Count <> 0 Then
								Dim oFind As EntityBaseServices(Of FilterElement) = (From o In oListServices Where o.Entity.Value = oItem.Value).FirstOrDefault
								If IsNothing(oFind) Then
									oListServices.Add(New EntityBaseServices(Of FilterElement)(oItem, CreateServiceBase(oDatareader)))
								Else
									oFind.Services.Add(CreateServiceBase(oDatareader))
								End If
							Else
								oListServices.Add(New EntityBaseServices(Of FilterElement)(oItem, CreateServiceBase(oDatareader)))
							End If
						End While
					End Using
				Catch ex As Exception
					If connection.State <> ConnectionState.Closed Then
						connection.Close()
					End If
				End Try
			End Using

			Return FilterElementsByPermission(oListServices, oClause)
		End Function
		Public Function FiltersPeriodo(ByVal oPerson As Person, ByVal LanguageID As Integer, ByVal OrganizationID As Integer, ByVal oStatus As CommunityStatus, ByVal oAccademicYear As Integer, ByVal oClause As GenericClause(Of ServiceClause), Optional ByVal OnlyAvailable As Boolean = True) As List(Of FilterElement)
			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
			Dim oListServices As New List(Of EntityBaseServices(Of FilterElement))

			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = SQL_ServicesFilterPeriodo(oClause, OrganizationID, oStatus, oAccademicYear)
					Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand(sqlCommand)
					oDatabase.AddInParameter(dbCommand, "@LinguaID", DbType.Int32, LanguageID)
					oDatabase.AddInParameter(dbCommand, "@PersonID", DbType.Int32, oPerson.ID)
					dbCommand.Connection = connection

					Using oDatareader As IDataReader = dbCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
						oListServices = GenerateEntityServices("PeriodoID", "Name", oDatareader)
					End Using
				Catch ex As Exception
					If connection.State <> ConnectionState.Closed Then
						connection.Close()
					End If
				End Try
			End Using
			Return FilterElementsByPermission(oListServices, oClause)
		End Function
		Public Function FiltersDegreeType(ByVal oPerson As Person, ByVal LanguageID As Integer, ByVal OrganizationID As Integer, ByVal oStatus As CommunityStatus, ByVal oClause As GenericClause(Of ServiceClause), Optional ByVal OnlyAvailable As Boolean = True) As List(Of FilterElement)
			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
			Dim oListServices As New List(Of EntityBaseServices(Of FilterElement))

			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = SQL_ServiceFilterDegreeType(oClause, OrganizationID, oStatus)
					Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand(sqlCommand)
					oDatabase.AddInParameter(dbCommand, "@LinguaID", DbType.Int32, LanguageID)
					oDatabase.AddInParameter(dbCommand, "@PersonID", DbType.Int32, oPerson.ID)
					dbCommand.Connection = connection

					Using oDatareader As IDataReader = dbCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
						oListServices = GenerateEntityServices("TipoCdsID", "Name", oDatareader)
					End Using
				Catch ex As Exception
					If connection.State <> ConnectionState.Closed Then
						connection.Close()
					End If
				End Try
			End Using
			Return FilterElementsByPermission(oListServices, oClause)
		End Function
        Function ListForFilters(ByVal oPerson As Person, ByVal LinguaID As Integer, ByVal oClause As GenericClause(Of ServiceClause), Optional ByVal OnlyAvailable As Boolean = True) As List(Of Subscription)
            Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
            Dim oList As New List(Of Subscription)
            Dim oListSubscriptionWithServices As New List(Of EntityBaseServices(Of Subscription))

            Using connection As DbConnection = oDatabase.CreateConnection()
                connection.Open()

                Try
                    Dim sqlCommand As String = SQL_ServiceFilterSubscription(oClause, OnlyAvailable)
                    Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand(sqlCommand)
                    oDatabase.AddInParameter(dbCommand, "@LinguaID", DbType.Int32, LinguaID)
                    oDatabase.AddInParameter(dbCommand, "@PersonID", DbType.Int32, oPerson.ID)
                    dbCommand.Connection = connection

                    Using oDatareader As IDataReader = dbCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                        While oDatareader.Read()
                            Try
                                Dim oItem As Subscription
                                Dim oService As ServiceBase = CreateServiceBase(oDatareader)
                                Dim oRole As New Role(oDatareader("RLPC_TPRL_ID"))

                                Select Case oDatareader("TipoComunitaID")
                                    Case StandardCommunityType.Organization
                                        Dim oOrganization As New Organization(oDatareader("ID"), New CommunityType(oDatareader("TipoComunitaID"), GenericValidator.ValString(oDatareader("TipoComunitaName"), "")), oDatareader("OrganizzazioneID"), oDatareader("Name"), True)
                                        oOrganization.IsArchive = oDatareader("isArchiviata")
                                        oOrganization.isClosedByAdministration = oDatareader("IsBloccata")
                                        oOrganization.Organization = Nothing
                                        oItem = CreateSubscription(oPerson, oRole, oOrganization, oDatareader("RLPC_attivato"), oDatareader("RLPC_abilitato"))

                                    Case Else
                                        Dim oCommunity As New Community(oDatareader("ID"), New CommunityType(oDatareader("TipoComunitaID"), GenericValidator.ValString(oDatareader("TipoComunitaName"), "")))
                                        oCommunity.IsArchive = oDatareader("isArchiviata")
                                        oCommunity.isClosedByAdministration = oDatareader("IsBloccata")
                                        oCommunity.Organization = New Organization(oDatareader("OrganizzazioneID"), True)
                                        oCommunity.Name = oDatareader("Name")
                                        oItem = CreateSubscription(oPerson, oRole, oCommunity, oDatareader("RLPC_attivato"), oDatareader("RLPC_abilitato"))
                                End Select

                                If oListSubscriptionWithServices.Count <> 0 Then
                                    Dim oFind As EntityBaseServices(Of Subscription) = (From o In oListSubscriptionWithServices Where o.Entity.CommunitySubscriptedID = oItem.CommunitySubscriptedID).FirstOrDefault
                                    If IsNothing(oFind) Then
                                        oListSubscriptionWithServices.Add(New EntityBaseServices(Of Subscription)(oItem, CreateServiceBase(oDatareader)))
                                    Else
                                        oFind.Services.Add(CreateServiceBase(oDatareader))
                                    End If
                                Else
                                    oListSubscriptionWithServices.Add(New EntityBaseServices(Of Subscription)(oItem, CreateServiceBase(oDatareader)))
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
            Dim oQuery = From o As EntityBaseServices(Of Subscription) In oListSubscriptionWithServices Where Me.VerifyPermission(o.Services, oClause) Select o.Entity

            Try
                oList = oQuery.ToList
            Catch ex As Exception

            End Try
            Return oList
        End Function
		Private ReadOnly Property SQL_ListServicePermission(ByVal ServiceCode As String, Optional ByVal OnlyAvailable As Boolean = True)
			Get
				Dim iResponse As String = "SELECT SRVZ_id, RLPC_PRSN_id, RLPC_TPRL_id, CMNT_id as CommunityID, LKSC_Permessi  FROM COMUNITA INNER JOIN SERVIZIO_COMUNITA ON CMNT_id = SRVC_CMNT_ID INNER JOIN " _
				& " SERVIZIO ON SRVC_SRVZ_ID = SRVZ_id INNER JOIN LK_RUOLO_PERSONA_COMUNITA ON CMNT_id = RLPC_CMNT_id INNER JOIN LK_SERVIZIO_COMUNITA ON CMNT_id = LKSC_CMNT_id AND " _
				& " SRVZ_id = LKSC_SRVZ_id AND RLPC_TPRL_id = LKSC_TPRL_id " _
				& "   WHERE     (SRVZ_Attivato = 1) AND (SRVC_isAbilitato = 1) AND RLPC_PRSN_id = @PersonID AND (RLPC_attivato = 1) AND (RLPC_abilitato = 1) "

				If Not ServiceCode = "" Then
					iResponse &= "AND SRVZ_codice= @ServiceCode"
				End If
				If OnlyAvailable Then
					iResponse &= " AND (RLPC_TPRL_ID <>-2 and RLPC_TPRL_ID<> -3) "
				End If
				Return iResponse
			End Get
		End Property

#Region "Generic SQL searcher by services"
		Private Function FilterElementsByPermission(ByVal oListServices As List(Of EntityBaseServices(Of FilterElement)), ByVal oClause As GenericClause(Of ServiceClause)) As List(Of FilterElement)
			Dim oList As New List(Of FilterElement)
			Dim oQuery = From o As EntityBaseServices(Of FilterElement) In oListServices Where Me.VerifyPermission(o.Services, oClause) Select o.Entity

			Try
				oList = oQuery.ToList
			Catch ex As Exception

			End Try
			Return oList
		End Function
		Private Function CreateSubscription(ByVal oPerson As Person, ByVal oRole As Role, ByVal oCommunty As Community, ByVal iIsActivated As Boolean, ByVal iIsEnabled As Boolean) As Subscription
			Return New Subscription(oPerson, oCommunty, oRole, iIsActivated, iIsEnabled)
		End Function
		Private Function GenerateEntityServices(ByVal ValueName As String, ByVal TextName As String, ByVal oDatareader As IDataReader) As List(Of EntityBaseServices(Of FilterElement))
			Dim oListServices As New List(Of EntityBaseServices(Of FilterElement))
			While oDatareader.Read()
				Try
					Dim oItem As New FilterElement(oDatareader(ValueName), oDatareader(TextName))
					If oListServices.Count <> 0 Then
						Dim oFind As EntityBaseServices(Of FilterElement) = (From o In oListServices Where o.Entity.Value = oItem.Value).FirstOrDefault
						If IsNothing(oFind) Then
							oListServices.Add(New EntityBaseServices(Of FilterElement)(oItem, CreateServiceBase(oDatareader)))
						Else
							oFind.Services.Add(CreateServiceBase(oDatareader))
						End If
					Else
						oListServices.Add(New EntityBaseServices(Of FilterElement)(oItem, CreateServiceBase(oDatareader)))
					End If
				Catch ex As Exception

				End Try
			End While
			Return oListServices
		End Function
		Private Function CreateServiceBase(ByVal oDataRecord As IDataRecord) As ServiceBase
			Try
				If oDataRecord.GetOrdinal("ServiceID") > 0 Then
					Return New ServiceBase(oDataRecord("ServiceID"), oDataRecord("ServiceCode"), oDataRecord("ServicePermission"))
				Else
					Return Nothing
				End If
			Catch ex As Exception
				Return Nothing
			End Try
		End Function

		Private ReadOnly Property SQL_ServicesFilterOrganization(ByVal oClause As GenericClause(Of ServiceClause), Optional ByVal OnlyAvailable As Boolean = True)
			Get
				If IsNothing(oClause) Then
					Return Me.SQL_FilterOrganization(OnlyAvailable)
				Else
					Dim iResponse As String = "SELECT DISTINCT ORGN_id as OrganizzazioneID,ORGN_ragioneSociale as Name,ComunitaOrganizzazione.CMNT_ID as ID, ComunitaOrganizzazione.CMNT_TPCM_id as TypeID,ComunitaOrganizzazione.CMNT_Archiviata as isArchiviata, ComunitaOrganizzazione.CMNT_Bloccata as IsBloccata, SRVC_SRVZ_ID as ServiceID,SRVZ_codice as ServiceCode,LKSC_Permessi as ServicePermission FROM ORGANIZZAZIONE INNER JOIN COMUNITA AS ComunitaOrganizzazione ON ORGN_id = ComunitaOrganizzazione.CMNT_ORGN_id INNER JOIN COMUNITA INNER JOIN "


					iResponse &= " SERVIZIO_COMUNITA ON dbo.COMUNITA.CMNT_id = SRVC_CMNT_ID INNER JOIN SERVIZIO ON SRVC_SRVZ_ID = SRVZ_id INNER JOIN LK_RUOLO_PERSONA_COMUNITA ON dbo.COMUNITA.CMNT_id = RLPC_CMNT_id ON  ORGN_id = dbo.COMUNITA.CMNT_ORGN_id " _
					& " INNER JOIN LK_SERVIZIO_COMUNITA ON LK_RUOLO_PERSONA_COMUNITA.RLPC_TPRL_id = LK_SERVIZIO_COMUNITA.LKSC_TPRL_id AND " _
					   & " LK_RUOLO_PERSONA_COMUNITA.RLPC_CMNT_id = LK_SERVIZIO_COMUNITA.LKSC_CMNT_id AND SERVIZIO_COMUNITA.SRVC_SRVZ_ID = LK_SERVIZIO_COMUNITA.LKSC_SRVZ_id " _
					& "   WHERE RLPC_PRSN_id = @PersonID AND (RLPC_attivato = 1) AND (RLPC_abilitato = 1) AND ("


					iResponse &= Me.ServiceStringCondition(oClause)

					iResponse &= " )"

					If OnlyAvailable Then
						iResponse &= " AND (RLPC_TPRL_ID <>-2 and RLPC_TPRL_ID<> -3) "
					End If
					iResponse &= " AND (ComunitaOrganizzazione.CMNT_idPadre = 0) ORDER BY ORGN_id"
					Return iResponse
				End If
			End Get
		End Property
		Private ReadOnly Property SQL_ServicesFilterStatus(ByVal oClause As GenericClause(Of ServiceClause), ByVal OrganizationID As Integer, Optional ByVal OnlyAvailable As Boolean = True)
			Get
				Dim iResponse As String = "SELECT DISTINCT CMNT_Archiviata as IsArchiviata,CMNT_Bloccata as IsBloccata, SRVC_SRVZ_ID as ServiceID,SRVZ_codice as ServiceCode,LKSC_Permessi as ServicePermission "

				If IsNothing(oClause) Then
					Return SQL_FilterStatus(OrganizationID, OnlyAvailable)
				Else

					iResponse &= "FROM COMUNITA INNER JOIN SERVIZIO_COMUNITA ON CMNT_id = SRVC_CMNT_ID INNER JOIN SERVIZIO ON SRVC_SRVZ_ID = SRVZ_id INNER JOIN LK_RUOLO_PERSONA_COMUNITA ON CMNT_id = RLPC_CMNT_id " _
					& " INNER JOIN LK_SERVIZIO_COMUNITA ON LK_RUOLO_PERSONA_COMUNITA.RLPC_TPRL_id = LK_SERVIZIO_COMUNITA.LKSC_TPRL_id AND " _
					   & " LK_RUOLO_PERSONA_COMUNITA.RLPC_CMNT_id = LK_SERVIZIO_COMUNITA.LKSC_CMNT_id AND SERVIZIO_COMUNITA.SRVC_SRVZ_ID = LK_SERVIZIO_COMUNITA.LKSC_SRVZ_id " _
					  & "   WHERE RLPC_PRSN_id = @PersonID AND (RLPC_attivato = 1) AND (RLPC_abilitato = 1) AND ("
					iResponse &= Me.ServiceStringCondition(oClause)

					iResponse &= " )"


				End If

				If OrganizationID = -1 Then
					' iResponse &= " AND CMNT_ID <> 0 "
				Else
					iResponse &= " AND CMNT_ORGN_id=" & OrganizationID.ToString
				End If
				If OnlyAvailable Then
					iResponse &= " AND (RLPC_TPRL_ID <>-2 and RLPC_TPRL_ID<> -3) "
				End If
				Return iResponse
			End Get
		End Property
		Private ReadOnly Property SQL_ServicesFilterCommunityType(ByVal oClause As GenericClause(Of ServiceClause), ByVal OrganizationID As Integer, ByVal oStatus As CommunityStatus, Optional ByVal OnlyAvailable As Boolean = True)
			Get
				If IsNothing(oClause) Then
					Return SQL_FilterCommunityType(OrganizationID, oStatus, OnlyAvailable)
				Else
					Dim iResponse As String = "SELECT DISTINCT CMNT_TPCM_id as ID,dbo.ufn_GetValueByLingua(@LinguaID,'TPCM_descrizione','TIPO_COMUNITA',CMNT_TPCM_id) as Name, SRVC_SRVZ_ID as ServiceID,SRVZ_codice as ServiceCode,LKSC_Permessi as ServicePermission "


					iResponse &= "FROM COMUNITA INNER JOIN SERVIZIO_COMUNITA ON CMNT_id = SRVC_CMNT_ID INNER JOIN SERVIZIO ON SRVC_SRVZ_ID = SRVZ_id INNER JOIN LK_RUOLO_PERSONA_COMUNITA ON CMNT_id = RLPC_CMNT_id " _
					  & " INNER JOIN LK_SERVIZIO_COMUNITA ON LK_RUOLO_PERSONA_COMUNITA.RLPC_TPRL_id = LK_SERVIZIO_COMUNITA.LKSC_TPRL_id AND " _
					  & " LK_RUOLO_PERSONA_COMUNITA.RLPC_CMNT_id = LK_SERVIZIO_COMUNITA.LKSC_CMNT_id AND SERVIZIO_COMUNITA.SRVC_SRVZ_ID = LK_SERVIZIO_COMUNITA.LKSC_SRVZ_id " _
					& "   WHERE  RLPC_PRSN_id = @PersonID AND (RLPC_attivato = 1) AND (RLPC_abilitato = 1) AND ("

					iResponse &= Me.ServiceStringCondition(oClause)
					iResponse &= " )"

					If OrganizationID = -1 Then
						' iResponse &= " AND CMNT_ID <> 0 "
					Else
						iResponse &= " AND CMNT_ORGN_id=" & OrganizationID.ToString
					End If
					If OnlyAvailable Then
						iResponse &= " AND (RLPC_TPRL_ID <>-2 and RLPC_TPRL_ID<> -3) "
					End If
					Select Case oStatus
						Case CommunityStatus.Archiviata
							iResponse &= " AND CMNT_Archiviata=1 and CMNT_Bloccata=0"
						Case CommunityStatus.ClosedByAdministration
							iResponse &= " AND CMNT_Bloccata=1"
						Case CommunityStatus.OnlyActivated
							iResponse &= " AND CMNT_Archiviata=0 and CMNT_Bloccata=0"
					End Select
					Return iResponse
				End If
			End Get
		End Property
		Private ReadOnly Property SQL_ServicesFilterAccademiYear(ByVal oClause As GenericClause(Of ServiceClause), ByVal OrganizationID As Integer, ByVal oStatus As CommunityStatus, Optional ByVal OnlyAvailable As Boolean = True)
			Get
				If IsNothing(oClause) Then
					Return SQL_FilterAccademiYear(OrganizationID, oStatus, OnlyAvailable)
				Else
					Dim iResponse As String = "SELECT DISTINCT CRSO_anno AS Anno, SRVC_SRVZ_ID as ServiceID,SRVZ_codice as ServiceCode,LKSC_Permessi as ServicePermission  FROM CORSO INNER JOIN COMUNITA INNER JOIN "
					iResponse &= " SERVIZIO_COMUNITA ON dbo.COMUNITA.CMNT_id = dbo.SERVIZIO_COMUNITA.SRVC_CMNT_ID INNER JOIN SERVIZIO ON dbo.SERVIZIO_COMUNITA.SRVC_SRVZ_ID = dbo.SERVIZIO.SRVZ_id INNER JOIN LK_RUOLO_PERSONA_COMUNITA ON dbo.COMUNITA.CMNT_id = dbo.LK_RUOLO_PERSONA_COMUNITA.RLPC_CMNT_id ON CRSO_CMNT_id = CMNT_id "

					iResponse &= " INNER JOIN LK_SERVIZIO_COMUNITA ON LK_RUOLO_PERSONA_COMUNITA.RLPC_TPRL_id = LK_SERVIZIO_COMUNITA.LKSC_TPRL_id AND " _
					  & " LK_RUOLO_PERSONA_COMUNITA.RLPC_CMNT_id = LK_SERVIZIO_COMUNITA.LKSC_CMNT_id AND SERVIZIO_COMUNITA.SRVC_SRVZ_ID = LK_SERVIZIO_COMUNITA.LKSC_SRVZ_id "



					iResponse &= " WHERE RLPC_PRSN_id = @PersonID AND (RLPC_attivato = 1) AND (RLPC_abilitato = 1) AND ("
					iResponse &= Me.ServiceStringCondition(oClause)
					iResponse &= " )"

					If OrganizationID = -1 Then
						' iResponse &= " AND CMNT_ID <> 0 "
					Else
						iResponse &= " AND CMNT_ORGN_id=" & OrganizationID.ToString
					End If
					If OnlyAvailable Then
						iResponse &= " AND (RLPC_TPRL_ID <>-2 and RLPC_TPRL_ID<> -3) "
					End If
					iResponse &= " and CMNT_TPCM_id= " & StandardCommunityType.UniversityCourse
					Select Case oStatus
						Case CommunityStatus.Archiviata
							iResponse &= " AND CMNT_Archiviata=1 and CMNT_Bloccata=0"
						Case CommunityStatus.ClosedByAdministration
							iResponse &= " AND CMNT_Bloccata=1"
						Case CommunityStatus.OnlyActivated
							iResponse &= " AND CMNT_Archiviata=0 and CMNT_Bloccata=0"
					End Select
					Return iResponse
				End If
			End Get
		End Property
		Private ReadOnly Property SQL_ServicesFilterPeriodo(ByVal oClause As GenericClause(Of ServiceClause), ByVal OrganizationID As Integer, ByVal oStatus As CommunityStatus, ByVal AccademicYear As Integer, Optional ByVal OnlyAvailable As Boolean = True)
			Get
				If IsNothing(oClause) Then
					Return SQL_FilterPeriodo(OrganizationID, oStatus, OnlyAvailable)
				Else
					Dim iResponse As String = "SELECT DISTINCT PRDO_id AS PeriodoID, dbo.ufn_GetValueByLingua(@LinguaID,'PRDO_Descrizione','PERIODO',PRDO_id) as Name , SRVC_SRVZ_ID as ServiceID,SRVZ_codice as ServiceCode,LKSC_Permessi as ServicePermission  FROM CORSO INNER JOIN COMUNITA INNER JOIN "

					iResponse &= " SERVIZIO_COMUNITA ON dbo.COMUNITA.CMNT_id = dbo.SERVIZIO_COMUNITA.SRVC_CMNT_ID INNER JOIN SERVIZIO ON dbo.SERVIZIO_COMUNITA.SRVC_SRVZ_ID = dbo.SERVIZIO.SRVZ_id INNER JOIN LK_RUOLO_PERSONA_COMUNITA ON dbo.COMUNITA.CMNT_id = dbo.LK_RUOLO_PERSONA_COMUNITA.RLPC_CMNT_id ON CRSO_CMNT_id = CMNT_id INNER JOIN PERIODO ON CRSO_PRDO_id = PRDO_id"


					iResponse &= " INNER JOIN LK_SERVIZIO_COMUNITA ON LK_RUOLO_PERSONA_COMUNITA.RLPC_TPRL_id = LK_SERVIZIO_COMUNITA.LKSC_TPRL_id AND " _
					  & " LK_RUOLO_PERSONA_COMUNITA.RLPC_CMNT_id = LK_SERVIZIO_COMUNITA.LKSC_CMNT_id AND SERVIZIO_COMUNITA.SRVC_SRVZ_ID = LK_SERVIZIO_COMUNITA.LKSC_SRVZ_id "




					iResponse &= "   WHERE RLPC_PRSN_id = @PersonID AND (RLPC_attivato = 1) AND (RLPC_abilitato = 1) AND ("

					iResponse &= Me.ServiceStringCondition(oClause)
					iResponse &= " )"

					If OrganizationID = -1 Then
						' iResponse &= " AND CMNT_ID <> 0 "
					Else
						iResponse &= " AND CMNT_ORGN_id=" & OrganizationID.ToString
					End If

					If AccademicYear = -1 Then
						' iResponse &= " AND CMNT_ID <> 0 "
					Else
						iResponse &= " AND CRSO_anno=" & AccademicYear.ToString
					End If

					If OnlyAvailable Then
						iResponse &= " AND (RLPC_TPRL_ID <>-2 and RLPC_TPRL_ID<> -3) "
					End If
					iResponse &= " and CMNT_TPCM_id= " & StandardCommunityType.UniversityCourse
					Select Case oStatus
						Case CommunityStatus.Archiviata
							iResponse &= " AND CMNT_Archiviata=1 and CMNT_Bloccata=0"
						Case CommunityStatus.ClosedByAdministration
							iResponse &= " AND CMNT_Bloccata=1"
						Case CommunityStatus.OnlyActivated
							iResponse &= " AND CMNT_Archiviata=0 and CMNT_Bloccata=0"
					End Select
					Return iResponse
				End If
			End Get
		End Property
		Private ReadOnly Property SQL_ServiceFilterDegreeType(ByVal oClause As GenericClause(Of ServiceClause), ByVal OrganizationID As Integer, ByVal oStatus As CommunityStatus, Optional ByVal OnlyAvailable As Boolean = True)
			Get
				If IsNothing(oClause) Then
					Return SQL_FilterDegreeType(OrganizationID, oStatus, OnlyAvailable)
				Else
					Dim iResponse As String = "SELECT DISTINCT TPCS_id AS TipoCdsID,dbo.ufn_GetValueByLingua(@LinguaID,'TPCS_nome','TIPO_CORSODiStudio',TPCS_id) as Name ,dbo.ufn_GetValueByLingua(@LinguaID,'TPCS_descrizione','TIPO_CORSODiStudio',TPCS_id) as Description,SRVC_SRVZ_ID as ServiceID,SRVZ_codice as ServiceCode,LKSC_Permessi as ServicePermission  FROM TIPO_CORSODiStudio INNER JOIN CORSO_DI_STUDI ON TPCS_id = CRDS_TPCS_ID INNER JOIN COMUNITA INNER JOIN "



					iResponse &= " SERVIZIO_COMUNITA ON CMNT_id = SRVC_CMNT_ID INNER JOIN SERVIZIO ON SRVC_SRVZ_ID = SRVZ_id INNER JOIN LK_RUOLO_PERSONA_COMUNITA ON CMNT_id = RLPC_CMNT_id ON CRDS_CMNT_ID = CMNT_id"




					iResponse &= " INNER JOIN LK_SERVIZIO_COMUNITA ON LK_RUOLO_PERSONA_COMUNITA.RLPC_TPRL_id = LK_SERVIZIO_COMUNITA.LKSC_TPRL_id AND " _
					  & " LK_RUOLO_PERSONA_COMUNITA.RLPC_CMNT_id = LK_SERVIZIO_COMUNITA.LKSC_CMNT_id AND SERVIZIO_COMUNITA.SRVC_SRVZ_ID = LK_SERVIZIO_COMUNITA.LKSC_SRVZ_id "

					iResponse &= "  WHERE RLPC_PRSN_id = @PersonID AND (RLPC_attivato = 1) AND (RLPC_abilitato = 1) AND ("

					iResponse &= Me.ServiceStringCondition(oClause)
					iResponse &= " )"


					If OrganizationID = -1 Then
						' iResponse &= " AND CMNT_ID <> 0 "
					Else
						iResponse &= " AND CMNT_ORGN_id=" & OrganizationID.ToString
					End If
					If OnlyAvailable Then
						iResponse &= " AND (RLPC_TPRL_ID <>-2 and RLPC_TPRL_ID<> -3) "
					End If
					iResponse &= " and CMNT_TPCM_id= " & StandardCommunityType.Degree
					Select Case oStatus
						Case CommunityStatus.Archiviata
							iResponse &= " AND CMNT_Archiviata=1 and CMNT_Bloccata=0"
						Case CommunityStatus.ClosedByAdministration
							iResponse &= " AND CMNT_Bloccata=1"
						Case CommunityStatus.OnlyActivated
							iResponse &= " AND CMNT_Archiviata=0 and CMNT_Bloccata=0"
					End Select
					Return iResponse
				End If
			End Get
		End Property


		Private ReadOnly Property SQL_ServiceFilterSubscription(ByVal oClause As GenericClause(Of ServiceClause), Optional ByVal OnlyAvailable As Boolean = True)
			Get
				If IsNothing(oClause) Then
					Return SQL_FilterListSubscription(OnlyAvailable)
				Else
					Dim iResponse As String = " SELECT  CMNT_Nome as Name,RLPC_TPRL_ID,RLPC_attivato,RLPC_abilitato,CMNT_Archiviata as isArchiviata, CMNT_Bloccata as IsBloccata, CMNT_id as ID, CMNT_ORGN_ID as OrganizzazioneID, CMNT_TPCM_id as TipoComunitaID,dbo.ufn_GetValueByLingua(@LinguaID,'TPCM_descrizione','TIPO_COMUNITA',CMNT_TPCM_id) as TipoComunitaName,  PRDO_id as PeriodoID, TPCS_id as TipoCdsID,CRSO_anno as Anno,SRVC_SRVZ_ID as ServiceID,SRVZ_codice as ServiceCode,LKSC_Permessi as ServicePermission  "

					iResponse &= " FROM  CORSO INNER JOIN PERIODO ON CRSO_PRDO_id = PRDO_id RIGHT OUTER JOIN COMUNITA INNER JOIN SERVIZIO_COMUNITA ON CMNT_id = dbo.SERVIZIO_COMUNITA.SRVC_CMNT_ID INNER JOIN SERVIZIO ON dbo.SERVIZIO_COMUNITA.SRVC_SRVZ_ID = dbo.SERVIZIO.SRVZ_id INNER JOIN " _
					  & " LK_RUOLO_PERSONA_COMUNITA ON CMNT_id = RLPC_CMNT_id INNER JOIN TIPO_COMUNITA ON CMNT_TPCM_id = TPCM_id ON  CRSO_CMNT_id = CMNT_id LEFT OUTER JOIN TIPO_CORSODiStudio INNER JOIN CORSO_DI_STUDI ON TPCS_id = CRDS_TPCS_ID ON CMNT_id = CRDS_CMNT_ID "


					iResponse &= " INNER JOIN LK_SERVIZIO_COMUNITA ON LK_RUOLO_PERSONA_COMUNITA.RLPC_TPRL_id = LK_SERVIZIO_COMUNITA.LKSC_TPRL_id AND " _
					  & " LK_RUOLO_PERSONA_COMUNITA.RLPC_CMNT_id = LK_SERVIZIO_COMUNITA.LKSC_CMNT_id AND SERVIZIO_COMUNITA.SRVC_SRVZ_ID = LK_SERVIZIO_COMUNITA.LKSC_SRVZ_id "


					iResponse &= "  WHERE RLPC_PRSN_id = @PersonID AND (RLPC_attivato = 1) AND (RLPC_abilitato = 1) AND ("

					iResponse &= Me.ServiceStringCondition(oClause)
					iResponse &= " )"





					'iResponse &= " SERVIZIO_COMUNITA ON CMNT_id = SRVC_CMNT_ID INNER JOIN SERVIZIO ON SRVC_SRVZ_ID = SRVZ_id INNER JOIN LK_RUOLO_PERSONA_COMUNITA ON CMNT_id = RLPC_CMNT_id ON CRDS_CMNT_ID = CMNT_id"


					'iResponse &= " SERVIZIO_COMUNITA ON dbo.COMUNITA.CMNT_id = dbo.SERVIZIO_COMUNITA.SRVC_CMNT_ID INNER JOIN SERVIZIO ON dbo.SERVIZIO_COMUNITA.SRVC_SRVZ_ID = dbo.SERVIZIO.SRVZ_id INNER JOIN LK_RUOLO_PERSONA_COMUNITA ON dbo.COMUNITA.CMNT_id = dbo.LK_RUOLO_PERSONA_COMUNITA.RLPC_CMNT_id ON CRSO_CMNT_id = CMNT_id INNER JOIN PERIODO ON CRSO_PRDO_id = PRDO_id"


					If OnlyAvailable Then
						iResponse &= " AND (RLPC_TPRL_ID <>-2 and RLPC_TPRL_ID<> -3) "
					End If

					Return iResponse
				End If
			End Get
		End Property
		Private ReadOnly Property SQL_ListForFilters(ByVal ByService As Boolean, Optional ByVal OnlyAvailable As Boolean = True)
			Get
				Dim iResponse As String = " SELECT  CMNT_Nome as Name,RLPC_TPRL_ID,RLPC_attivato,RLPC_abilitato,CMNT_Archiviata as isArchiviata, CMNT_Bloccata as IsBloccata, CMNT_id as ID, CMNT_ORGN_ID as OrganizzazioneID, CMNT_TPCM_id as TipoComunitaID,dbo.ufn_GetValueByLingua(@LinguaID,'TPCM_descrizione','TIPO_COMUNITA',CMNT_TPCM_id) as TipoComunitaName,  PRDO_id as PeriodoID, TPCS_id as TipoCdsID,CRSO_anno as Anno "
				If ByService Then
					iResponse &= " FROM  CORSO INNER JOIN PERIODO ON CRSO_PRDO_id = PRDO_id RIGHT OUTER JOIN COMUNITA INNER JOIN SERVIZIO_COMUNITA ON CMNT_id = dbo.SERVIZIO_COMUNITA.SRVC_CMNT_ID INNER JOIN SERVIZIO ON dbo.SERVIZIO_COMUNITA.SRVC_SRVZ_ID = dbo.SERVIZIO.SRVZ_id INNER JOIN " _
					 & " LK_RUOLO_PERSONA_COMUNITA ON CMNT_id = RLPC_CMNT_id INNER JOIN TIPO_COMUNITA ON CMNT_TPCM_id = TPCM_id ON  CRSO_CMNT_id = CMNT_id LEFT OUTER JOIN TIPO_CORSODiStudio INNER JOIN CORSO_DI_STUDI ON TPCS_id = CRDS_TPCS_ID ON CMNT_id = CRDS_CMNT_ID " _
					& "   WHERE     (SRVZ_Attivato = 1) AND (SRVC_isAbilitato = 1) AND RLPC_PRSN_id = @PersonID AND (RLPC_attivato = 1) AND (RLPC_abilitato = 1) AND SRVZ_codice= @ServiceCode"
				Else
					iResponse &= " FROM  CORSO INNER JOIN PERIODO ON CRSO_PRDO_id = PRDO_id RIGHT OUTER JOIN COMUNITA INNER JOIN " _
					& " LK_RUOLO_PERSONA_COMUNITA ON CMNT_id = RLPC_CMNT_id INNER JOIN TIPO_COMUNITA ON CMNT_TPCM_id = TPCM_id ON  CRSO_CMNT_id = CMNT_id LEFT OUTER JOIN TIPO_CORSODiStudio INNER JOIN CORSO_DI_STUDI ON TPCS_id = CRDS_TPCS_ID ON CMNT_id = CRDS_CMNT_ID " _
					  & "   WHERE  RLPC_PRSN_id = @PersonID AND (RLPC_attivato = 1) AND (RLPC_abilitato = 1)"
				End If
				If OnlyAvailable Then
					iResponse &= " AND (RLPC_TPRL_ID <>-2 and RLPC_TPRL_ID<> -3) "
				End If
				Return iResponse
			End Get
		End Property

		Private Function ServiceStringCondition(ByVal oConditions As GenericClause(Of ServiceClause)) As String
			Dim iResponse As String = ""

			iResponse = "(SRVZ_Attivato = 1 and SRVC_isAbilitato = 1 and SRVZ_codice='" & oConditions.Clause.Service.Code & "') "
			If oConditions.NextClause Is Nothing Then
				Return iResponse
			Else
				'If oConditions.OperatorForNextClause = OperatorType.AndCondition Then
				'	iResponse = "(" & iResponse & " AND ( " & ServiceStringCondition(oConditions.NextClause) & "))"
				'Else
				iResponse = "(" & iResponse & " OR ( " & ServiceStringCondition(oConditions.NextClause) & "))"
			End If

			Return iResponse
		End Function

		Private Function HasPermission(ByVal oServiceClause As ServiceClause, ByVal PermissionToValidate As String) As Boolean
			Dim intValid, intValidate As Int64

			intValid = Convert.ToInt64(New String(oServiceClause.Service.PermissionString.Reverse.ToArray), 2)
			intValidate = Convert.ToInt64(New String(PermissionToValidate.Reverse.ToArray), 2)

			Select Case oServiceClause.PermissionOperator
				Case OperatorType.AndCondition
					Return intValid And intValidate
				Case OperatorType.OrCondition
					Return intValid Or intValidate
				Case OperatorType.XorCondition
					Return intValid Xor intValidate
				Case Else
					Return False
			End Select
			Return False
		End Function

		Private Function VerifyPermission(ByVal oServices As List(Of ServiceBase), ByVal oClause As GenericClause(Of ServiceClause)) As Boolean
			Dim iResponse As Boolean = False
			If IsNothing(oClause) Then
				Return True
			ElseIf IsNothing(oServices) Then
				Return True
			Else
				iResponse = VerifyPermissionForService(oClause.Clause, oServices.First(Function(p) p.Code = oClause.Clause.Service.Code).PermissionString)

				If IsNothing(oClause.NextClause) Then
					Return iResponse
				Else
					If oClause.OperatorForNextClause = OperatorType.AndCondition Then
						iResponse = iResponse And VerifyPermission(oServices, oClause.NextClause)
					ElseIf oClause.OperatorForNextClause = OperatorType.OrCondition Then
						iResponse = iResponse Or VerifyPermission(oServices, oClause.NextClause)
					ElseIf oClause.OperatorForNextClause = OperatorType.XorCondition Then
						iResponse = iResponse Xor VerifyPermission(oServices, oClause.NextClause)
					End If
				End If
			End If
			Return iResponse
		End Function

		Private Function VerifyPermissionForService(ByVal oServiceClause As ServiceClause, ByVal PermissionToValidate As String) As Boolean
			Dim intValid, intValidate As Int64

			intValid = Convert.ToInt64(New String(oServiceClause.Service.PermissionString.Reverse.ToArray), 2)
			intValidate = Convert.ToInt64(New String(PermissionToValidate.Reverse.ToArray), 2)

			Select Case oServiceClause.PermissionOperator
				Case OperatorType.AndCondition
					Return intValid And intValidate
				Case OperatorType.OrCondition
					Return intValid Or intValidate
				Case OperatorType.XorCondition
					Return intValid Xor intValidate
				Case Else
					Return False
			End Select
			Return False
		End Function
#End Region

#Region "Generic SQL searcher"
		Private ReadOnly Property SQL_FilterOrganization(Optional ByVal OnlyAvailable As Boolean = True)
			Get
				Dim iResponse As String = "SELECT DISTINCT ORGN_id as OrganizzazioneID,ORGN_ragioneSociale as Name,ComunitaOrganizzazione.CMNT_ID as ID, ComunitaOrganizzazione.CMNT_TPCM_id as TypeID,ComunitaOrganizzazione.CMNT_Archiviata as isArchiviata, ComunitaOrganizzazione.CMNT_Bloccata as IsBloccata FROM ORGANIZZAZIONE INNER JOIN COMUNITA AS ComunitaOrganizzazione ON ORGN_id = ComunitaOrganizzazione.CMNT_ORGN_id INNER JOIN COMUNITA INNER JOIN "

				iResponse &= "LK_RUOLO_PERSONA_COMUNITA ON COMUNITA.CMNT_id = RLPC_CMNT_id ON  ORGN_id = COMUNITA.CMNT_ORGN_id  " _
				& "   WHERE RLPC_PRSN_id = @PersonID AND (RLPC_attivato = 1) AND (RLPC_abilitato = 1)"

				If OnlyAvailable Then
					iResponse &= " AND (RLPC_TPRL_ID <>-2 and RLPC_TPRL_ID<> -3) "
				End If
				iResponse &= " AND (ComunitaOrganizzazione.CMNT_idPadre = 0)"
				Return iResponse
			End Get
		End Property
		Private ReadOnly Property SQL_FilterStatus(ByVal OrganizationID As Integer, Optional ByVal OnlyAvailable As Boolean = True)
			Get
				Dim iResponse As String = "SELECT DISTINCT CMNT_Archiviata as IsArchiviata,CMNT_Bloccata as IsBloccata "
				iResponse &= " FROM COMUNITA INNER JOIN LK_RUOLO_PERSONA_COMUNITA ON CMNT_id = RLPC_CMNT_id " _
				& "   WHERE RLPC_PRSN_id = @PersonID AND (RLPC_attivato = 1) AND (RLPC_abilitato = 1)"
				If OrganizationID = -1 Then
					' iResponse &= " AND CMNT_ID <> 0 "
				Else
					iResponse &= " AND CMNT_ORGN_id=" & OrganizationID.ToString
				End If
				If OnlyAvailable Then
					iResponse &= " AND (RLPC_TPRL_ID <>-2 and RLPC_TPRL_ID<> -3) "
				End If
				Return iResponse
			End Get
		End Property
		Private ReadOnly Property SQL_FilterCommunityType(ByVal OrganizationID As Integer, ByVal oStatus As CommunityStatus, Optional ByVal OnlyAvailable As Boolean = True)
			Get
				Dim iResponse As String = "SELECT DISTINCT CMNT_TPCM_id as ID,dbo.ufn_GetValueByLingua(@LinguaID,'TPCM_descrizione','TIPO_COMUNITA',CMNT_TPCM_id) as Name "

				iResponse &= " FROM COMUNITA INNER JOIN LK_RUOLO_PERSONA_COMUNITA ON CMNT_id = RLPC_CMNT_id " _
				& "   WHERE RLPC_PRSN_id = @PersonID AND (RLPC_attivato = 1) AND (RLPC_abilitato = 1)"
				If OrganizationID = -1 Then
					' iResponse &= " AND CMNT_ID <> 0 "
				Else
					iResponse &= " AND CMNT_ORGN_id=" & OrganizationID.ToString
				End If
				If OnlyAvailable Then
					iResponse &= " AND (RLPC_TPRL_ID <>-2 and RLPC_TPRL_ID<> -3) "
				End If
				Select Case oStatus
					Case CommunityStatus.Archiviata
						iResponse &= " AND CMNT_Archiviata=1 and CMNT_Bloccata=0"
					Case CommunityStatus.ClosedByAdministration
						iResponse &= " AND CMNT_Bloccata=1"
					Case CommunityStatus.OnlyActivated
						iResponse &= " AND CMNT_Archiviata=0 and CMNT_Bloccata=0"
				End Select
				Return iResponse
			End Get
		End Property
		Private ReadOnly Property SQL_FilterAccademiYear(ByVal OrganizationID As Integer, ByVal oStatus As CommunityStatus, Optional ByVal OnlyAvailable As Boolean = True)
			Get
				Dim iResponse As String = "SELECT DISTINCT CRSO_anno AS Anno FROM CORSO INNER JOIN COMUNITA INNER JOIN "
				iResponse &= " LK_RUOLO_PERSONA_COMUNITA ON CMNT_id = RLPC_CMNT_id ON CRSO_CMNT_id = CMNT_id " _
				& "   WHERE RLPC_PRSN_id = @PersonID AND (RLPC_attivato = 1) AND (RLPC_abilitato = 1)"

				If OrganizationID = -1 Then
					' iResponse &= " AND CMNT_ID <> 0 "
				Else
					iResponse &= " AND CMNT_ORGN_id=" & OrganizationID.ToString
				End If
				If OnlyAvailable Then
					iResponse &= " AND (RLPC_TPRL_ID <>-2 and RLPC_TPRL_ID<> -3) "
				End If
				iResponse &= " and CMNT_TPCM_id= " & StandardCommunityType.UniversityCourse
				Select Case oStatus
					Case CommunityStatus.Archiviata
						iResponse &= " AND CMNT_Archiviata=1 and CMNT_Bloccata=0"
					Case CommunityStatus.ClosedByAdministration
						iResponse &= " AND CMNT_Bloccata=1"
					Case CommunityStatus.OnlyActivated
						iResponse &= " AND CMNT_Archiviata=0 and CMNT_Bloccata=0"
				End Select
				Return iResponse
			End Get
		End Property
		Private ReadOnly Property SQL_FilterPeriodo(ByVal OrganizationID As Integer, ByVal oStatus As CommunityStatus, ByVal AccademicYear As Integer, Optional ByVal OnlyAvailable As Boolean = True)
			Get
				Dim iResponse As String = "SELECT DISTINCT PRDO_id AS PeriodoID, dbo.ufn_GetValueByLingua(@LinguaID,'PRDO_Descrizione','PERIODO',PRDO_id) as Name FROM CORSO INNER JOIN COMUNITA INNER JOIN "


				iResponse &= " LK_RUOLO_PERSONA_COMUNITA ON CMNT_id = RLPC_CMNT_id ON CRSO_CMNT_id = CMNT_id INNER JOIN PERIODO ON CRSO_PRDO_id = PRDO_id" _
				& "   WHERE RLPC_PRSN_id = @PersonID AND (RLPC_attivato = 1) AND (RLPC_abilitato = 1)"

				If OrganizationID = -1 Then
					' iResponse &= " AND CMNT_ID <> 0 "
				Else
					iResponse &= " AND CMNT_ORGN_id=" & OrganizationID.ToString
				End If

				If AccademicYear = -1 Then
					' iResponse &= " AND CMNT_ID <> 0 "
				Else
					iResponse &= " AND CRSO_anno=" & AccademicYear.ToString
				End If

				If OnlyAvailable Then
					iResponse &= " AND (RLPC_TPRL_ID <>-2 and RLPC_TPRL_ID<> -3) "
				End If
				iResponse &= " and CMNT_TPCM_id= " & StandardCommunityType.UniversityCourse
				Select Case oStatus
					Case CommunityStatus.Archiviata
						iResponse &= " AND CMNT_Archiviata=1 and CMNT_Bloccata=0"
					Case CommunityStatus.ClosedByAdministration
						iResponse &= " AND CMNT_Bloccata=1"
					Case CommunityStatus.OnlyActivated
						iResponse &= " AND CMNT_Archiviata=0 and CMNT_Bloccata=0"
				End Select
				Return iResponse
			End Get
		End Property
		Private ReadOnly Property SQL_FilterDegreeType(ByVal OrganizationID As Integer, ByVal oStatus As CommunityStatus, Optional ByVal OnlyAvailable As Boolean = True)
			Get
				Dim iResponse As String = "SELECT DISTINCT TPCS_id AS TipoCdsID,dbo.ufn_GetValueByLingua(@LinguaID,'TPCS_nome','TIPO_CORSODiStudio',TPCS_id) as Name ,dbo.ufn_GetValueByLingua(@LinguaID,'TPCS_descrizione','TIPO_CORSODiStudio',TPCS_id) as Description FROM TIPO_CORSODiStudio INNER JOIN CORSO_DI_STUDI ON TPCS_id = CRDS_TPCS_ID INNER JOIN COMUNITA INNER JOIN "

				iResponse &= " LK_RUOLO_PERSONA_COMUNITA ON CMNT_id = RLPC_CMNT_id ON CRDS_CMNT_ID = CMNT_id" _
				& "   WHERE RLPC_PRSN_id = @PersonID AND (RLPC_attivato = 1) AND (RLPC_abilitato = 1)"

				If OrganizationID = -1 Then
					' iResponse &= " AND CMNT_ID <> 0 "
				Else
					iResponse &= " AND CMNT_ORGN_id=" & OrganizationID.ToString
				End If
				If OnlyAvailable Then
					iResponse &= " AND (RLPC_TPRL_ID <>-2 and RLPC_TPRL_ID<> -3) "
				End If
				iResponse &= " and CMNT_TPCM_id= " & StandardCommunityType.Degree
				Select Case oStatus
					Case CommunityStatus.Archiviata
						iResponse &= " AND CMNT_Archiviata=1 and CMNT_Bloccata=0"
					Case CommunityStatus.ClosedByAdministration
						iResponse &= " AND CMNT_Bloccata=1"
					Case CommunityStatus.OnlyActivated
						iResponse &= " AND CMNT_Archiviata=0 and CMNT_Bloccata=0"
				End Select
				Return iResponse
			End Get
		End Property
		Private ReadOnly Property SQL_FilterListSubscription(Optional ByVal OnlyAvailable As Boolean = True)
			Get
				Dim iResponse As String = " SELECT  CMNT_Nome as Name,RLPC_TPRL_ID,RLPC_attivato,RLPC_abilitato,CMNT_Archiviata as isArchiviata, CMNT_Bloccata as IsBloccata, CMNT_id as ID, CMNT_ORGN_ID as OrganizzazioneID, CMNT_TPCM_id as TipoComunitaID,dbo.ufn_GetValueByLingua(@LinguaID,'TPCM_descrizione','TIPO_COMUNITA',CMNT_TPCM_id) as TipoComunitaName,  PRDO_id as PeriodoID, TPCS_id as TipoCdsID,CRSO_anno as Anno "

				iResponse &= " FROM  CORSO INNER JOIN PERIODO ON CRSO_PRDO_id = PRDO_id RIGHT OUTER JOIN COMUNITA INNER JOIN " _
				& " LK_RUOLO_PERSONA_COMUNITA ON CMNT_id = RLPC_CMNT_id INNER JOIN TIPO_COMUNITA ON CMNT_TPCM_id = TPCM_id ON  CRSO_CMNT_id = CMNT_id LEFT OUTER JOIN TIPO_CORSODiStudio INNER JOIN CORSO_DI_STUDI ON TPCS_id = CRDS_TPCS_ID ON CMNT_id = CRDS_CMNT_ID " _
				  & "   WHERE  RLPC_PRSN_id = @PersonID AND (RLPC_attivato = 1) AND (RLPC_abilitato = 1)"
				If OnlyAvailable Then
					iResponse &= " AND (RLPC_TPRL_ID <>-2 and RLPC_TPRL_ID<> -3) "
				End If
				Return iResponse
			End Get
		End Property
#End Region

    End Class
End Namespace