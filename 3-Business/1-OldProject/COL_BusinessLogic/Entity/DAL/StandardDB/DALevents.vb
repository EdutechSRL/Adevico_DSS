Imports Comol.Entity
Imports Comol.Entity.Events

Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common


Namespace Comol.DAL.StandardDB
	Public Class DALevents
		Inherits DALabstract

		Public Sub New(ByVal oDbconnection As ConnectionDB)
			MyBase.new(oDbconnection)
		End Sub

		Public Function Add(ByVal oEvent As CommunityEvent) As CommunityEvent
			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)

			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = "sp_Evento_AggiungiNew"
					Dim dbCommand As DbCommand = oDatabase.GetStoredProcCommand(sqlCommand)
					oDatabase.AddOutParameter(dbCommand, "@EVNT_id", DbType.Int32, 4)
					oDatabase.AddInParameter(dbCommand, "@EVNT_idPadre", DbType.Int32, oEvent.FatherEventID)
					oDatabase.AddInParameter(dbCommand, "@EVNT_nome", DbType.String, oEvent.Name)
					oDatabase.AddInParameter(dbCommand, "@EVNT_macro", DbType.Int32, IIf(oEvent.IsMacro, 1, 0))
					oDatabase.AddInParameter(dbCommand, "@EVNT_visualizza", DbType.Int32, IIf(oEvent.Isvisible, 1, 0))
					oDatabase.AddInParameter(dbCommand, "@EVNT_ripeti", DbType.Int32, IIf(oEvent.Repeat, 1, 0))
					oDatabase.AddInParameter(dbCommand, "@EVNT_note", DbType.String, oEvent.Note)
					oDatabase.AddInParameter(dbCommand, "@EVNT_luogo", DbType.String, oEvent.Place)
					oDatabase.AddInParameter(dbCommand, "@EVNT_link", DbType.String, oEvent.Links)
					oDatabase.AddInParameter(dbCommand, "@EVNT_dataModifica", DbType.DateTime, oEvent.CreatedAt)
					oDatabase.AddInParameter(dbCommand, "@EVNT_PRSN_ID", DbType.Int32, oEvent.CreatedBy.ID)
					oDatabase.AddInParameter(dbCommand, "@EVNT_TPEV_id", DbType.Int32, oEvent.Type.ID)
					oDatabase.AddInParameter(dbCommand, "@EVNT_CMNT_id", DbType.Int32, oEvent.CommunityOwner.ID)
					oDatabase.AddInParameter(dbCommand, "@EVNT_perpetuo", DbType.Int32, IIf(oEvent.IsPerpetuo, 1, 0))
					oDatabase.AddInParameter(dbCommand, "@EVNT_annoAccademico", DbType.Int32, oEvent.Year.Year)
					oDatabase.AddInParameter(dbCommand, "@ExternalPlannerID", DbType.String, oEvent.ExternalPlannerID)
					If Not oEvent.ReferTo Is Nothing Then
						oDatabase.AddInParameter(dbCommand, "@ReferTo", DbType.Int32, oEvent.ReferTo.ID)
					End If
					dbCommand.Connection = connection
					If dbCommand.ExecuteNonQuery() > 0 Then
						oEvent.ID = oDatabase.GetParameterValue(dbCommand, "EVNT_id")
						For Each oitem As EventItem In oEvent.Items
							If oitem.ID = 0 Then
								Me.AddEventItem(oEvent.ID, oitem)
							Else
								Me.ChangeEventItem(oEvent.ID, oitem)
							End If
						Next
					End If

				Catch ex As Exception

				End Try
			End Using
			Return oEvent
		End Function
		Public Sub ChangeEvent(ByVal oEvent As CommunityEvent)
			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)

			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = "sp_Evento_ModificaNew"
					Dim dbCommand As DbCommand = oDatabase.GetStoredProcCommand(sqlCommand)
					oDatabase.AddInParameter(dbCommand, "@EVNT_id", DbType.Int32, oEvent.ID)
					oDatabase.AddInParameter(dbCommand, "@EVNT_idPadre", DbType.Int32, oEvent.FatherEventID)
					oDatabase.AddInParameter(dbCommand, "@EVNT_nome", DbType.String, oEvent.Name)
					oDatabase.AddInParameter(dbCommand, "@EVNT_macro", DbType.Int32, IIf(oEvent.IsMacro, 1, 0))
					oDatabase.AddInParameter(dbCommand, "@EVNT_visualizza", DbType.Int32, IIf(oEvent.Isvisible, 1, 0))
					oDatabase.AddInParameter(dbCommand, "@EVNT_ripeti", DbType.Int32, IIf(oEvent.Repeat, 1, 0))
					oDatabase.AddInParameter(dbCommand, "@EVNT_note", DbType.String, oEvent.Note)
					oDatabase.AddInParameter(dbCommand, "@EVNT_luogo", DbType.String, oEvent.Place)
					oDatabase.AddInParameter(dbCommand, "@EVNT_link", DbType.String, oEvent.Links)
					oDatabase.AddInParameter(dbCommand, "@EVNT_dataModifica", DbType.DateTime, oEvent.ModifiedAt)
					oDatabase.AddInParameter(dbCommand, "@EVNT_PRSN_ID", DbType.Int32, oEvent.ModifiedBy.ID)
					oDatabase.AddInParameter(dbCommand, "@TypeID", DbType.Int32, oEvent.Type.ID)
					oDatabase.AddInParameter(dbCommand, "@EVNT_CMNT_id", DbType.Int32, oEvent.CommunityOwner.ID)
					oDatabase.AddInParameter(dbCommand, "@EVNT_perpetuo", DbType.Int32, IIf(oEvent.IsPerpetuo, 1, 0))
					oDatabase.AddInParameter(dbCommand, "@EVNT_annoAccademico", DbType.Int32, oEvent.Year.Year)
					oDatabase.AddInParameter(dbCommand, "@ExternalPlannerID", DbType.String, oEvent.ExternalPlannerID)
					If Not oEvent.ReferTo Is Nothing Then
						oDatabase.AddInParameter(dbCommand, "@ReferTo", DbType.Int32, oEvent.ReferTo.ID)
					End If

					dbCommand.Connection = connection
					If dbCommand.ExecuteNonQuery() > 0 Then
						For Each oitem As EventItem In oEvent.Items
							If oitem.ID = 0 Then
								Me.AddEventItem(oEvent.ID, oitem)
							Else
								Me.ChangeEventItem(oEvent.ID, oitem)
							End If
						Next
					End If

				Catch ex As Exception

				End Try
			End Using
		End Sub


		Public Function AddEventItem(ByVal EventID As Integer, ByVal oItem As EventItem) As EventItem
			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)

			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = "sp_Orario_AggiungiNew"
					Dim dbCommand As DbCommand = oDatabase.GetStoredProcCommand(sqlCommand)
					oDatabase.AddOutParameter(dbCommand, "@ORRI_id", DbType.Int32, 4)
					oDatabase.AddInParameter(dbCommand, "@ORRI_EVNT_id", DbType.Int32, EventID)
					oDatabase.AddInParameter(dbCommand, "@ORRI_inizio", DbType.DateTime, oItem.StartDate)
					oDatabase.AddInParameter(dbCommand, "@ORRI_fine", DbType.DateTime, oItem.EndDate)
					oDatabase.AddInParameter(dbCommand, "@ORRI_dataModifica", DbType.DateTime, oItem.CreatedAt)
					oDatabase.AddInParameter(dbCommand, "@ORRI_visibile", DbType.Int32, IIf(oItem.Isvisible, 1, 0))
					oDatabase.AddInParameter(dbCommand, "@ORRI_note", DbType.String, oItem.Note)
					oDatabase.AddInParameter(dbCommand, "@ORRI_aula", DbType.String, oItem.Place)
					oDatabase.AddInParameter(dbCommand, "@ORRI_link", DbType.String, oItem.Links)
					oDatabase.AddInParameter(dbCommand, "@ORRI_PRSN_ID", DbType.Int32, oItem.CreatedBy.ID)

					oDatabase.AddInParameter(dbCommand, "@Title", DbType.String, oItem.Title)
					oDatabase.AddInParameter(dbCommand, "@Text", DbType.String, oItem.Text)
					oDatabase.AddInParameter(dbCommand, "@ExternalPlannerID", DbType.String, oItem.ExternalPlannerID)
					If Not oItem.ReferTo Is Nothing Then
						oDatabase.AddInParameter(dbCommand, "@ReferTo", DbType.Int32, oItem.ReferTo.ID)
					End If

					dbCommand.Connection = connection
					If dbCommand.ExecuteNonQuery() > 0 Then
						oItem.ID = oDatabase.GetParameterValue(dbCommand, "ORRI_id")
					End If
				Catch ex As Exception

				End Try
			End Using
			Return oItem
		End Function
		Public Function ChangeEventItem(ByVal EventID As Integer, ByVal oItem As EventItem) As EventItem
			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)

			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = "sp_Orario_ModificaNew"
					Dim dbCommand As DbCommand = oDatabase.GetStoredProcCommand(sqlCommand)
					oDatabase.AddInParameter(dbCommand, "@ORRI_id", DbType.Int32, oItem.ID)
					oDatabase.AddInParameter(dbCommand, "@ORRI_inizio", DbType.DateTime, oItem.StartDate)
					oDatabase.AddInParameter(dbCommand, "@ORRI_fine", DbType.DateTime, oItem.EndDate)
					oDatabase.AddInParameter(dbCommand, "@ORRI_dataModifica", DbType.DateTime, oItem.ModifiedAt)
					oDatabase.AddInParameter(dbCommand, "@ORRI_visibile", DbType.Int32, IIf(oItem.Isvisible, 1, 0))
					oDatabase.AddInParameter(dbCommand, "@ORRI_note", DbType.String, oItem.Note)
					oDatabase.AddInParameter(dbCommand, "@ORRI_aula", DbType.String, oItem.Place)
					oDatabase.AddInParameter(dbCommand, "@ORRI_link", DbType.String, oItem.Links)
					oDatabase.AddInParameter(dbCommand, "@ORRI_PRSN_ID", DbType.Int32, oItem.ModifiedBy.ID)

					oDatabase.AddInParameter(dbCommand, "@Title", DbType.String, oItem.Title)
					oDatabase.AddInParameter(dbCommand, "@Text", DbType.String, oItem.Text)
					oDatabase.AddInParameter(dbCommand, "@ExternalPlannerID", DbType.String, oItem.ExternalPlannerID)
					If Not oItem.ReferTo Is Nothing Then
						oDatabase.AddInParameter(dbCommand, "@ReferTo", DbType.Int32, oItem.ReferTo.ID)
					End If

					dbCommand.Connection = connection
					dbCommand.ExecuteNonQuery()

				Catch ex As Exception

				End Try
			End Using
			Return oItem
		End Function

		Function GetEventByID(ByVal EventID As Integer, ByVal LanguageID As Integer) As CommunityEvent
			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
			Dim oEvent As New CommunityEvent

			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = SQL_GetEventByID(LanguageID)
					Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand(sqlCommand)
					oDatabase.AddInParameter(dbCommand, "@EventID", DbType.Int32, EventID)
					dbCommand.Connection = connection

					Using oDatareader As IDataReader = dbCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
						While oDatareader.Read()
							Try
								oEvent.ID = EventID
								oEvent.Items = Me.ListEventItems(EventID)
								oEvent.CommunityOwner = oDatareader("EVNT_CMNT_ID")
								oEvent.ExternalPlannerID = GenericValidator.ValString(oDatareader("EVNT_ExternalPlannerID"), "")
								oEvent.FatherEventID = GenericValidator.ValInteger(oDatareader("EVNT_idPadre"), 0)
								oEvent.IsArchive = False
								oEvent.IsMacro = GenericValidator.ValBool(oDatareader("EVNT_macro"), False)
								oEvent.Isvisible = GenericValidator.ValBool(oDatareader("EVNT_visualizza"), True)
								oEvent.Links = GenericValidator.ValString(oDatareader("EVNT_link"), "")
								oEvent.IsPerpetuo = GenericValidator.ValBool(oDatareader("EVNT_perpetuo"), True)
								oEvent.Name = GenericValidator.ValString(oDatareader("EVNT_nome"), "")
								oEvent.Note = GenericValidator.ValString(oDatareader("EVNT_note"), "")
								oEvent.Place = GenericValidator.ValString(oDatareader("EVNT_luogo"), "")
								oEvent.Repeat = GenericValidator.ValBool(oDatareader("EVNT_ripeti"), False)
								oEvent.Type = New EventType(oDatareader("EVNT_TPEV_id"), GenericValidator.ValString(oDatareader("EventType_Name"), ""))
								oEvent.Year = New AcademicYear(GenericValidator.ValInteger(oDatareader("EVNT_annoAccademico"), IIf(Now.Month < 9, Now.Year - 1, Now.Year)))
								oEvent.CreatedAt = GenericValidator.ValData(oDatareader("EVNT_dataModifica"), Nothing)
								oEvent.ModifiedAt = GenericValidator.ValData(oDatareader("EVNT_dataModifica"), Nothing)
								oEvent.CreatedBy = New Person(oDatareader("EVNT_PRSN_ID"))
								oEvent.ModifiedBy = New Person(oDatareader("EVNT_PRSN_ID"))
								If Not IsDBNull(oDatareader("EVNT_CloneID")) Then
									oEvent.ReferTo = New CommunityEvent(oDatareader("EVNT_CloneID"))
								End If

							Catch ex As Exception

							End Try
						End While
					End Using
				Catch ex As Exception
					oEvent = Nothing
					If connection.State <> ConnectionState.Closed Then
						connection.Close()
					End If
				End Try
			End Using
			Return oEvent
		End Function

		Function ListEventItems(ByVal EventID As Integer) As List(Of EventItem)
			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
			Dim oList As New List(Of EventItem)

			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = SQL_GetEventItems()
					Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand(sqlCommand)
					oDatabase.AddInParameter(dbCommand, "@EventID", DbType.Int32, EventID)
					dbCommand.Connection = connection

					Using oDatareader As IDataReader = dbCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
						While oDatareader.Read()
							Try
								Dim oItem As New EventItem
								oItem.ID = oDatareader("ORRI_id")
								oItem.Isvisible = GenericValidator.ValBool(oDatareader("ORRI_visibile"), True)
								'oItem.Links = GenericValidator.ValString(oDatareader("ORRI_Link"), "")
								'oItem.Note = GenericValidator.ValString(oDatareader("ORRI_Note"), "")
								'oItem.Place = GenericValidator.ValString(oDatareader("ORRI_aula"), "")
								oItem.CreatedBy = New Person(oDatareader("ORRI_PRSN_ID"))
								oItem.StartDate = GenericValidator.ValData(oDatareader("ORRI_inizio"), Nothing)
								oItem.EndDate = GenericValidator.ValData(oDatareader("ORRI_fine"), Nothing)
								oItem.ExternalPlannerID = GenericValidator.ValString(oDatareader("ORRI_ExternalPlannerID"), "")
								'oItem.ReferTo = New EventItem(oDatareader("ORRI_CloneID"))
								oItem.Title = GenericValidator.ValString(oDatareader("ORRI_Title"), "")
								oItem.Text = GenericValidator.ValString(oDatareader("PREV_ProgrammaSvolto"), "")

								oList.Add(oItem)
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


		Function ListEventsImportedFromEsse3(ByVal CommunityID As Integer, ByVal LanguageID As Integer) As List(Of CommunityEvent)
			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
			Dim oList As New List(Of CommunityEvent)

			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = SQL_ListEventsImportedFromEsse3(LanguageID)
					Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand(sqlCommand)
					oDatabase.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
					dbCommand.Connection = connection

					Using oDatareader As IDataReader = dbCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
						While oDatareader.Read()
							Try
								Dim oCommunityEvent As New CommunityEvent
								oCommunityEvent.ID = oDatareader("EVNT_id")
								oCommunityEvent.Items = Me.ListEventItemsImportedFromEsse3(oDatareader("EVNT_id"))
								oCommunityEvent.CommunityOwner = New Community(CommunityID)
								oCommunityEvent.ExternalPlannerID = GenericValidator.ValString(oDatareader("EVNT_ExternalPlannerID"), "")
								oCommunityEvent.FatherEventID = GenericValidator.ValInteger(oDatareader("EVNT_idPadre"), 0)
								oCommunityEvent.IsArchive = False
								oCommunityEvent.IsMacro = GenericValidator.ValBool(oDatareader("EVNT_macro"), False)
								oCommunityEvent.Isvisible = GenericValidator.ValBool(oDatareader("EVNT_visualizza"), True)
								oCommunityEvent.Links = GenericValidator.ValString(oDatareader("EVNT_link"), "")
								oCommunityEvent.IsPerpetuo = GenericValidator.ValBool(oDatareader("EVNT_perpetuo"), True)
								oCommunityEvent.Name = GenericValidator.ValString(oDatareader("EVNT_nome"), "")
								oCommunityEvent.Note = GenericValidator.ValString(oDatareader("EVNT_note"), "")
								oCommunityEvent.Place = GenericValidator.ValString(oDatareader("EVNT_luogo"), "")
								oCommunityEvent.Repeat = GenericValidator.ValBool(oDatareader("EVNT_ripeti"), False)
								oCommunityEvent.Type = New EventType(oDatareader("EVNT_TPEV_id"), GenericValidator.ValString(oDatareader("EventType_Name"), ""))
								oCommunityEvent.Year = New AcademicYear(GenericValidator.ValInteger(oDatareader("EVNT_annoAccademico"), IIf(Now.Month < 9, Now.Year - 1, Now.Year)))
								oCommunityEvent.ModifiedAt = GenericValidator.ValData(oDatareader("EVNT_dataModifica"), Nothing)
								oCommunityEvent.CreatedBy = New Person(oDatareader("EVNT_PRSN_ID"))
								If Not IsDBNull(oDatareader("EVNT_CloneID")) Then
									oCommunityEvent.ReferTo = New CommunityEvent(oDatareader("EVNT_CloneID"))
								End If

								oList.Add(oCommunityEvent)
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
		Function ListEventItemsImportedFromEsse3(ByVal EventID As Integer) As List(Of EventItem)
			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
			Dim oList As New List(Of EventItem)

			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = SQL_GetEventItemsFromEsse3()
					Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand(sqlCommand)
					oDatabase.AddInParameter(dbCommand, "@EventID", DbType.Int32, EventID)
					dbCommand.Connection = connection

					Using oDatareader As IDataReader = dbCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
						While oDatareader.Read()
							Try
								Dim oItem As New EventItem
								oItem.ID = oDatareader("ORRI_id")
								oItem.Isvisible = GenericValidator.ValBool(oDatareader("ORRI_visibile"), True)
								'oItem.Links = GenericValidator.ValString(oDatareader("ORRI_Link"), "")
								'oItem.Note = GenericValidator.ValString(oDatareader("ORRI_Note"), "")
								'oItem.Place = GenericValidator.ValString(oDatareader("ORRI_aula"), "")
								oItem.CreatedBy = New Person(oDatareader("ORRI_PRSN_ID"))
								oItem.StartDate = GenericValidator.ValData(oDatareader("ORRI_inizio"), Nothing)
								oItem.EndDate = GenericValidator.ValData(oDatareader("ORRI_fine"), Nothing)
								oItem.ExternalPlannerID = GenericValidator.ValString(oDatareader("ORRI_ExternalPlannerID"), "")
								'oItem.ReferTo = New EventItem(oDatareader("ORRI_CloneID"))
								oItem.Title = GenericValidator.ValString(oDatareader("ORRI_Title"), "")
								oItem.Text = GenericValidator.ValString(oDatareader("PREV_ProgrammaSvolto"), "")

								oList.Add(oItem)
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


		Private ReadOnly Property SQL_GetEventByID(ByVal LanguageID As Integer)
			Get
				Dim iResponse As String = "SELECT EVNT_id, EVNT_CMNT_id,EVNT_idPadre, EVNT_nome, EVNT_macro, EVNT_visualizza, EVNT_ripeti, EVNT_note, EVNT_luogo, EVNT_link, EVNT_dataModifica,EVNT_TPEV_id, EVNT_CMNT_id, EVNT_annoAccademico, EVNT_perpetuo, EVNT_PRSN_ID, EVNT_ExternalPlannerID, EVNT_CloneID, dbo.ufn_GetValueByLingua('" & LanguageID & "','TPEV_nome','TIPO_EVENTO',EVNT_TPEV_id) as EventType_Name "
				iResponse &= " FROM EVENTO where EVNT_id=@EventID"

				Return iResponse
			End Get
		End Property
		Private ReadOnly Property SQL_GetEventItems()
			Get
				Dim iResponse As String = "SELECT ORRI_id, ORRI_Title,ORRI_EVNT_id, ORRI_inizio, ORRI_fine, ORRI_aula, ORRI_visibile, ORRI_PRSN_ID, ORRI_dataModifica, ORRI_Link,ORRI_CloneID, ORRI_Note, ORRI_ExternalPlannerID,PREV_ProgrammaSvolto "
				iResponse &= " FROM ORARIO INNER JOIN PROGRAMMA_EVENTO on ORRI_id=PREV_ORRI_id  where ORRI_EVNT_id=@EventID ORDER BY ORRI_inizio ASC "
				Return iResponse
			End Get
		End Property
		Private ReadOnly Property SQL_ListEventsImportedFromEsse3(ByVal LanguageID As Integer)
			Get
				Dim iResponse As String = "SELECT EVNT_id, EVNT_idPadre, EVNT_nome, EVNT_macro, EVNT_visualizza, EVNT_ripeti, EVNT_note, EVNT_luogo, EVNT_link, EVNT_dataModifica,EVNT_TPEV_id, EVNT_CMNT_id, EVNT_annoAccademico, EVNT_perpetuo, EVNT_PRSN_ID, EVNT_ExternalPlannerID, EVNT_CloneID, dbo.ufn_GetValueByLingua('" & LanguageID & "','TPEV_nome','TIPO_EVENTO',EVNT_TPEV_id) as EventType_Name "
				iResponse &= " FROM EVENTO where EVNT_CMNT_id=@CommunityID AND ( EVNT_ExternalPlannerID <> '')"

				Return iResponse
			End Get
		End Property
		Private ReadOnly Property SQL_GetEventItemsFromEsse3() 'ByVal CommunityID As Integer
			Get
				Dim iResponse As String = "SELECT ORRI_id, ORRI_Title,ORRI_EVNT_id, ORRI_inizio, ORRI_fine, ORRI_aula, ORRI_visibile, ORRI_PRSN_ID, ORRI_dataModifica, ORRI_Link,ORRI_CloneID, ORRI_Note, ORRI_ExternalPlannerID,PREV_ProgrammaSvolto "
				iResponse &= " FROM ORARIO INNER JOIN PROGRAMMA_EVENTO on ORRI_id=PREV_ORRI_id  where ORRI_EVNT_id=@EventID and (ORRI_ExternalPlannerID <> '') ORDER BY ORRI_inizio ASC "
				'iResponse &= " FROM ORARIO INNER JOIN EVENTI ON ORRI_EVNT_id = EVNT_ID INNER JOIN PROGRAMMA_EVENTO on ORRI_id=PREV_ORRI_id  where EVNT_CMNT_id=" & CommunityID & " AND EVNT_id=@EventID and (ORRI_ExternalPlannerID <> null and ORRI_ExternalPlannerID <> '') ORDER BY ORRI_inizio ASC "

				Return iResponse
			End Get
		End Property

	End Class
End Namespace