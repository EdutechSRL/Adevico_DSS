Imports Comol.Entity
Imports Comol.Entity.File

Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common

Namespace Comol.DAL.StandardDB
	Public Class DALrepository
		Inherits DALabstract

		Public Sub New(ByVal oDbconnection As ConnectionDB)
			MyBase.new(oDbconnection)
		End Sub


		Public Function AddLabel(ByVal oLabel As LabelTag) As LabelTag
			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
			Dim iReturn As New LabelTag

			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = "sp_LabelTag_Add"
					Dim dbCommand As DbCommand = oDatabase.GetStoredProcCommand(sqlCommand)
					oDatabase.AddOutParameter(dbCommand, "LABL_ID", DbType.Int64, 8)
					If Not oLabel.CommunityOwner Is Nothing Then
						oDatabase.AddInParameter(dbCommand, "LABL_CMNT_ID", DbType.Int64, oLabel.CommunityOwner.ID)
					End If
					oDatabase.AddInParameter(dbCommand, "LABL_DisplayName", DbType.String, oLabel.DisplayName)
					oDatabase.AddInParameter(dbCommand, "LABL_Description", DbType.String, oLabel.Description)
					If Not oLabel.FatherLabel Is Nothing Then
						oDatabase.AddInParameter(dbCommand, "LABL_FatherID", DbType.Int64, oLabel.FatherLabel.ID)
					Else
						oDatabase.AddInParameter(dbCommand, "LABL_FatherID", DbType.Int64, 0)
					End If
					oDatabase.AddInParameter(dbCommand, "LABL_CreateBy", DbType.Int64, oLabel.CreatedBy.ID)
					oDatabase.AddInParameter(dbCommand, "LABL_CreateAt", DbType.DateTime, oLabel.CreatedAt)

					If Not oLabel.ModifiedBy Is Nothing Then
						oDatabase.AddInParameter(dbCommand, "LABL_ModifiedAt", DbType.DateTime, oLabel.ModifiedAt)
						oDatabase.AddInParameter(dbCommand, "LABL_ModifiedBy", DbType.Int64, oLabel.ModifiedBy.ID)
					End If
					If Not oLabel.Deletedby Is Nothing Then
						oDatabase.AddInParameter(dbCommand, "LABL_DeletedAt", DbType.DateTime, oLabel.DeletedAt)
						oDatabase.AddInParameter(dbCommand, "LABL_DeletedBy", DbType.Int64, oLabel.Deletedby.ID)
					End If
					
					oDatabase.AddInParameter(dbCommand, "LABL_isDeleted", DbType.Int32, IIf(oLabel.IsDeleted, 1, 0))
					oDatabase.AddInParameter(dbCommand, "LABL_HardLinks", DbType.Int32, oLabel.HardLinks)
					oDatabase.AddInParameter(dbCommand, "LABL_isSystem", DbType.Int32, IIf(oLabel.isSystemDefined, 1, 0))
					dbCommand.Connection = connection
					dbCommand.ExecuteNonQuery()
					iReturn = oLabel
					iReturn.ID = dbCommand.Parameters(0).Value
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
		Public Function RemoveLabel(ByVal oLabel As LabelTag, ByVal oPerson As Person) As Boolean
			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
			Dim iReturn As New LabelTag

			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = "sp_LabelTag_Remove"
					Dim dbCommand As DbCommand = oDatabase.GetStoredProcCommand(sqlCommand)
					Dim oDeletedAt As DateTime = Now
					oDatabase.AddInParameter(dbCommand, "LABL_ID", DbType.Int64, oLabel.ID)
					oDatabase.AddInParameter(dbCommand, "LABL_DeletedAt", DbType.DateTime, oDeletedAt)
					oDatabase.AddInParameter(dbCommand, "LABL_DeletedBy", DbType.Int64, oPerson.ID)

					dbCommand.Connection = connection
					dbCommand.ExecuteNonQuery()
					oLabel.IsDeleted = True
					oLabel.DeletedAt = oDeletedAt
					oLabel.Deletedby = oPerson
					Return True
				Catch ex As Exception
					Return False
				Finally
					If connection.State <> ConnectionState.Closed Then
						connection.Close()
					End If
				End Try
			End Using
			Return False
		End Function

		Public Function GetUserLabelsAsFolder(ByVal oPerson As Person, ByVal oLabelFather As LabelTag) As System.Collections.Generic.List(Of LabelTag)
			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
			Dim oLista As New List(Of LabelTag)

			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = Me.SQL_UserLabels(oPerson, oLabelFather)
					Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand(sqlCommand)
					dbCommand.Connection = connection

					Using oDatareader As IDataReader = dbCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
						While oDatareader.Read()
							Try
								Dim oLabel As New LabelTag()

								oLabel.ID = oDatareader("LABL_ID")
								oLabel.CommunityOwner = Nothing
								oLabel.CreatedBy = oPerson
								oLabel.CreatedAt = GenericValidator.ValData(oDatareader("LABL_CreateAt"), Nothing)
								If Not IsDBNull(oDatareader("LABL_ModifiedBy")) Then
									oLabel.ModifiedBy = New Person(oDatareader("PersonM_id"), GenericValidator.ValString(oDatareader("PersonM_Nome"), ""), GenericValidator.ValString(oDatareader("PersonM_Cognome"), ""))
									oLabel.ModifiedAt = GenericValidator.ValData(oDatareader("LABL_ModifiedAt"), Nothing)
								End If
								If Not IsDBNull(oDatareader("LABL_DeletedBy")) Then
									oLabel.ModifiedBy = New Person(oDatareader("PersonD_id"), GenericValidator.ValString(oDatareader("PersonD_Nome"), ""), GenericValidator.ValString(oDatareader("PersonD_Cognome"), ""))
									oLabel.DeletedAt = GenericValidator.ValData(oDatareader("LABL_DeletedAt"), Nothing)
								End If
								oLabel.IsDeleted = oDatareader("LABL_isDeleted")
								oLabel.HardLinks = oDatareader("LABL_HardLinks")
								oLabel.isSystemDefined = oDatareader("LABL_isSystem")
								oLabel.Description = GenericValidator.ValString(oDatareader("LABL_Description"), "")
								oLabel.DisplayName = GenericValidator.ValString(oDatareader("LABL_DisplayName"), "")
								oLabel.FatherLabel = oLabelFather
								oLabel.Labels = Me.GetUserLabelsAsFolder(oPerson, oLabel)
								oLista.Add(oLabel)
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
		Public Function GetUserLabels(ByVal oPerson As Person) As System.Collections.Generic.List(Of LabelTag)
			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
			Dim oLista As New List(Of LabelTag)

			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = Me.SQL_UserLabelsPlain(oPerson)
					Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand(sqlCommand)
					dbCommand.Connection = connection

					Using oDatareader As IDataReader = dbCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
						While oDatareader.Read()
							Try
								Dim oLabel As New LabelTag()

								oLabel.ID = oDatareader("LABL_ID")
								oLabel.CommunityOwner = Nothing
								oLabel.CreatedBy = oPerson
								oLabel.CreatedAt = GenericValidator.ValData(oDatareader("LABL_CreateAt"), Nothing)
								If Not IsDBNull(oDatareader("LABL_ModifiedBy")) Then
									oLabel.ModifiedBy = New Person(oDatareader("PersonM_id"), GenericValidator.ValString(oDatareader("PersonM_Nome"), ""), GenericValidator.ValString(oDatareader("PersonM_Cognome"), ""))
									oLabel.ModifiedAt = GenericValidator.ValData(oDatareader("LABL_ModifiedAt"), Nothing)
								End If
								If Not IsDBNull(oDatareader("LABL_DeletedBy")) Then
									oLabel.ModifiedBy = New Person(oDatareader("PersonD_id"), GenericValidator.ValString(oDatareader("PersonD_Nome"), ""), GenericValidator.ValString(oDatareader("PersonD_Cognome"), ""))
									oLabel.DeletedAt = GenericValidator.ValData(oDatareader("LABL_DeletedAt"), Nothing)
								End If
								oLabel.IsDeleted = oDatareader("LABL_isDeleted")
								oLabel.HardLinks = oDatareader("LABL_HardLinks")
								oLabel.isSystemDefined = oDatareader("LABL_isSystem")
								oLabel.Description = GenericValidator.ValString(oDatareader("LABL_Description"), "")
								oLabel.DisplayName = GenericValidator.ValString(oDatareader("LABL_DisplayName"), "")
								If oDatareader("LABL_FatherID") = 0 Then
									oLabel.FatherLabel = Nothing
								Else
									oLabel.FatherLabel = New LabelTag(oDatareader("LABL_FatherID"))
								End If
								oLista.Add(oLabel)
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
		Public Function GetCommunityLabels(ByVal oPerson As Person, ByVal oCommunity As Community) As System.Collections.Generic.List(Of LabelTag)
			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
			Dim oLista As New List(Of LabelTag)

			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = Me.SQL_CommunityLabels(oPerson, oCommunity)
					Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand(sqlCommand)
					dbCommand.Connection = connection

					Using oDatareader As IDataReader = dbCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
						While oDatareader.Read()
							Try
								Dim oLabel As New LabelTag()

								oLabel.ID = oDatareader("LABL_ID")
								oLabel.CommunityOwner = oCommunity
								oLabel.CreatedBy = New Person(oDatareader("PersonO_id"), GenericValidator.ValString(oDatareader("PersonO_Nome"), ""), GenericValidator.ValString(oDatareader("PersonO_Cognome"), ""))
								oLabel.CreatedAt = GenericValidator.ValData(oDatareader("LABL_CreateAt"), Nothing)
								If Not IsDBNull(oDatareader("LABL_ModifiedBy")) Then
									oLabel.ModifiedBy = New Person(oDatareader("PersonM_id"), GenericValidator.ValString(oDatareader("PersonM_Nome"), ""), GenericValidator.ValString(oDatareader("PersonM_Cognome"), ""))
									oLabel.ModifiedAt = GenericValidator.ValData(oDatareader("LABL_ModifiedAt"), Nothing)
								End If
								If Not IsDBNull(oDatareader("LABL_DeletedBy")) Then
									oLabel.ModifiedBy = New Person(oDatareader("PersonD_id"), GenericValidator.ValString(oDatareader("PersonD_Nome"), ""), GenericValidator.ValString(oDatareader("PersonD_Cognome"), ""))
									oLabel.DeletedAt = GenericValidator.ValData(oDatareader("LABL_DeletedAt"), Nothing)
								End If
								oLabel.IsDeleted = oDatareader("LABL_isDeleted")
								oLabel.HardLinks = oDatareader("LABL_HardLinks")
								oLabel.isSystemDefined = oDatareader("LABL_isSystem")
								oLabel.Description = GenericValidator.ValString(oDatareader("LABL_Description"), "")
								oLabel.DisplayName = GenericValidator.ValString(oDatareader("LABL_DisplayName"), "")
								If oDatareader("LABL_FatherID") = 0 Then
									oLabel.FatherLabel = Nothing
								Else
									oLabel.FatherLabel = New LabelTag(oDatareader("LABL_FatherID"))
								End If
								oLista.Add(oLabel)
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

		Public Function GetFileLabels(ByVal oFile As LearningObjectFile) As System.Collections.Generic.List(Of LabelTag)
			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
			Dim oLista As New List(Of LabelTag)

			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = Me.SQL_FileLabel(oFile.ID)
					Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand(sqlCommand)
					dbCommand.Connection = connection

					Using oDatareader As IDataReader = dbCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
						While oDatareader.Read()
							Try
								Dim oLabel As New LabelTag()
								oLabel.ID = oDatareader("LABL_ID")
								oLista.Add(oLabel)
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


		Public Function GetFileData(ByVal oFileID As Guid) As LearningObjectFile
			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)

			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = Me.SQL_GetFileData(oFileID.ToString)
					Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand(sqlCommand)
					dbCommand.Connection = connection

					Using oDatareader As IDataReader = dbCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
						While oDatareader.Read()
							Dim oFile As New LearningObjectFile
							With oFile
								.ID = New Guid(oDatareader("FLRP_ID").ToString)
								.Name = GenericValidator.ValString(oDatareader("FLRP_DisplayName"), "")
								.Description = GenericValidator.ValString(oDatareader("FLRP_Description"), "")
								.FileSystemName = GenericValidator.ValString(oDatareader("FLRP_FileSystemName"), "")
								.FileSystemPath = GenericValidator.ValString(oDatareader("FLRP_FileSystemPath"), "")
								Try
									.Size = oDatareader("FLRP_Size")
								Catch ex As Exception
									.Size = 0
								End Try

								.CreatedAt = GenericValidator.ValData(oDatareader("FLRP_CreateAt"), Nothing)
								.CreatedBy = New Person(oDatareader("PersonO_id"), GenericValidator.ValString(oDatareader("PersonO_Nome"), ""), GenericValidator.ValString(oDatareader("PersonO_Cognome"), ""))
								If Not IsDBNull(oDatareader("FLRP_ModifiedBy")) Then
									.ModifiedBy = New Person(oDatareader("PersonM_id"), GenericValidator.ValString(oDatareader("PersonM_Nome"), ""), GenericValidator.ValString(oDatareader("PersonM_Cognome"), ""))
									.ModifiedAt = GenericValidator.ValData(oDatareader("FLRP_ModifiedAt"), Nothing)
								End If
								If Not IsDBNull(oDatareader("FLRP_DeletedBy")) Then
									.ModifiedBy = New Person(oDatareader("PersonD_id"), GenericValidator.ValString(oDatareader("PersonD_Nome"), ""), GenericValidator.ValString(oDatareader("PersonD_Cognome"), ""))
									.DeletedAt = GenericValidator.ValData(oDatareader("FLRP_DeletedAt"), Nothing)
								End If
								.IsDeleted = GenericValidator.ValBool(oDatareader("FLRP_isDeleted"), False)
								.HardLinks = oDatareader("FLRP_HardLinks")
								.ContentType = New MimeType(oDatareader("FLRP_Extension"), oDatareader("FLRP_ContentType"))
								.Extension = oDatareader("FLRP_Extension")
								Return oFile
							End With
						End While
					End Using
				Catch ex As Exception
					If connection.State <> ConnectionState.Closed Then
						connection.Close()
					End If
				End Try

			End Using
			Return Nothing
		End Function
		Public Function GetUserFiles(ByVal oPerson As Person) As System.Collections.Generic.List(Of UserRepository)
			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
			Dim oLista As New List(Of UserRepository)

			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = Me.SQL_UserFiles(oPerson)
					Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand(sqlCommand)
					dbCommand.Connection = connection

					Using oDatareader As IDataReader = dbCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
						While oDatareader.Read()
							Try
								Dim oFile As LearningObjectFile = Me.GetFileData(oDatareader("USRP_FLRP_ID"))
								If Not oFile Is Nothing Then
									Dim oUserRepository As New UserRepository


									' "  , USRP_CreateBy, , , ,  , ,  " _
									oUserRepository.ID = New Guid(oDatareader("USRP_ID").ToString)
									oUserRepository.CreatedAt = GenericValidator.ValData(oDatareader("USRP_CreateAt"), Nothing)
									oUserRepository.CreatedBy = oPerson
									If Not IsDBNull(oDatareader("USRP_ModifiedBy")) Then
										oUserRepository.ModifiedBy = New Person(oDatareader("PersonM_id"), GenericValidator.ValString(oDatareader("PersonM_Nome"), ""), GenericValidator.ValString(oDatareader("PersonM_Cognome"), ""))
										oUserRepository.ModifiedAt = GenericValidator.ValData(oDatareader("USRP_ModifiedAt"), Nothing)
									End If
									If Not IsDBNull(oDatareader("USRP_DeletedBy")) Then
										oUserRepository.ModifiedBy = New Person(oDatareader("PersonD_id"), GenericValidator.ValString(oDatareader("PersonD_Nome"), ""), GenericValidator.ValString(oDatareader("PersonD_Cognome"), ""))
										oUserRepository.DeletedAt = GenericValidator.ValData(oDatareader("USRP_DeletedAt"), Nothing)
									End If
									oUserRepository.IsDeleted = GenericValidator.ValBool(oDatareader("USRP_isDeleted"), False)
									oUserRepository.UserFile = oFile
									oUserRepository.Labels = GetFileLabels(oFile)

									oLista.Add(oUserRepository)
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


		Public Function AddLearningObjectFile(ByVal oFile As LearningObjectFile) As LearningObjectFile
			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
			Dim iReturn As New LearningObjectFile

			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = "sp_LearningObjectFile_Add"
					Dim dbCommand As DbCommand = oDatabase.GetStoredProcCommand(sqlCommand)

					oDatabase.AddInParameter(dbCommand, "FLRP_ID", DbType.Guid, oFile.ID)
					oDatabase.AddInParameter(dbCommand, "FLRP_DisplayName", DbType.String, oFile.Name)
					oDatabase.AddInParameter(dbCommand, "FLRP_Extension", DbType.String, oFile.Extension)
					oDatabase.AddInParameter(dbCommand, "FLRP_Description", DbType.String, oFile.Description)
					oDatabase.AddInParameter(dbCommand, "FLRP_FileSystemName", DbType.String, oFile.FileSystemName)
					oDatabase.AddInParameter(dbCommand, "FLRP_FileSystemPath", DbType.String, oFile.FileSystemPath)
					oDatabase.AddInParameter(dbCommand, "FLRP_Size", DbType.Int64, oFile.Size)
					oDatabase.AddInParameter(dbCommand, "FLRP_ContentType", DbType.String, oFile.ContentType.Type)
					oDatabase.AddInParameter(dbCommand, "FLRP_CreateBy", DbType.Int64, oFile.CreatedBy.ID)
					oDatabase.AddInParameter(dbCommand, "FLRP_CreateAt", DbType.DateTime, oFile.CreatedAt)

					If Not oFile.ModifiedBy Is Nothing Then
						oDatabase.AddInParameter(dbCommand, "FLRP_ModifiedAt", DbType.DateTime, oFile.ModifiedAt)
						oDatabase.AddInParameter(dbCommand, "FLRP_ModifiedBy", DbType.Int64, oFile.ModifiedBy.ID)
					End If
					If Not oFile.Deletedby Is Nothing Then
						oDatabase.AddInParameter(dbCommand, "FLRP_DeletedAt", DbType.DateTime, oFile.DeletedAt)
						oDatabase.AddInParameter(dbCommand, "FLRP_DeletedBy", DbType.Int64, oFile.Deletedby.ID)
					End If
					oDatabase.AddInParameter(dbCommand, "FLRP_isDeleted", DbType.Int32, IIf(oFile.IsDeleted, 1, 0))
					oDatabase.AddInParameter(dbCommand, "FLRP_HardLinks", DbType.Int32, oFile.HardLinks)
					dbCommand.Connection = connection
					dbCommand.ExecuteNonQuery()
					iReturn = oFile
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
		Public Function AddLabelToLearningObjectFile(ByVal oFile As LearningObjectFile, ByVal oLabel As LabelTag, ByVal RepositoryItemID As Guid) As LabelTag
			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)

			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = "sp_Label_AddToLearningObjectFile"
					Dim dbCommand As DbCommand = oDatabase.GetStoredProcCommand(sqlCommand)

					oDatabase.AddInParameter(dbCommand, "LabelID", DbType.Int64, oLabel.ID)
					oDatabase.AddInParameter(dbCommand, "FileID", DbType.Guid, oFile.ID)
					oDatabase.AddInParameter(dbCommand, "RepositoryID", DbType.Guid, RepositoryItemID)

					dbCommand.Connection = connection
					dbCommand.ExecuteNonQuery()
					Return oLabel
				Catch ex As Exception
					Return Nothing
				Finally
					If connection.State <> ConnectionState.Closed Then
						connection.Close()
					End If
				End Try
			End Using
			Return Nothing
		End Function

		Public Function AddFileToUserRepository(ByVal oRepositoryItem As UserRepository) As UserRepository
			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
			Dim iReturn As New UserRepository

			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					If AddLearningObjectFile(oRepositoryItem.UserFile) Is Nothing Then
						Return Nothing
					Else
						Dim oRepositoryItemID As Guid = Guid.NewGuid

						Dim sqlCommand As String = "sp_UserRepository_Add"
						Dim dbCommand As DbCommand = oDatabase.GetStoredProcCommand(sqlCommand)

						oDatabase.AddInParameter(dbCommand, "@USRP_ID", DbType.Guid, oRepositoryItemID)
						oDatabase.AddInParameter(dbCommand, "@USRP_FLRP_ID", DbType.Guid, oRepositoryItem.UserFile.ID)
						oDatabase.AddInParameter(dbCommand, "@USRP_CreateBy", DbType.Int64, oRepositoryItem.CreatedBy.ID)
						oDatabase.AddInParameter(dbCommand, "@USRP_CreateAt", DbType.DateTime, oRepositoryItem.CreatedAt)

						If Not oRepositoryItem.ModifiedBy Is Nothing Then
							oDatabase.AddInParameter(dbCommand, "@USRP_ModifiedAt", DbType.DateTime, oRepositoryItem.ModifiedAt)
							oDatabase.AddInParameter(dbCommand, "@USRP_ModifiedBy", DbType.Int64, oRepositoryItem.ModifiedBy.ID)
						End If
						If Not oRepositoryItem.Deletedby Is Nothing Then
							oDatabase.AddInParameter(dbCommand, "@USRP_DeletedAt", DbType.DateTime, oRepositoryItem.DeletedAt)
							oDatabase.AddInParameter(dbCommand, "@USRP_DeletedBy", DbType.Int64, oRepositoryItem.Deletedby.ID)
						End If

						oDatabase.AddInParameter(dbCommand, "@USRP_isDeleted", DbType.Int32, IIf(oRepositoryItem.IsDeleted, 1, 0))
						dbCommand.Connection = connection
						dbCommand.ExecuteNonQuery()

						For Each oLabel As LabelTag In oRepositoryItem.Labels
							If AddLabelToLearningObjectFile(oRepositoryItem.UserFile, oLabel, oRepositoryItemID) Is Nothing Then
								oRepositoryItem.Labels.Remove(oLabel)
							End If
						Next
						oRepositoryItem.ID = oRepositoryItemID
						iReturn = oRepositoryItem
					End If

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
		Public Function RemoveFromUserRepository(ByVal oRepositoryItem As UserRepository, ByVal oPerson As Person) As UserRepository
			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
			Dim iReturn As New UserRepository

			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = "sp_UserRepository_RemoveFile"
					Dim dbCommand As DbCommand = oDatabase.GetStoredProcCommand(sqlCommand)

					oDatabase.AddInParameter(dbCommand, "@USRP_ID", DbType.Guid, oRepositoryItem.ID)

					oRepositoryItem.DeletedAt = Now
					oRepositoryItem.Deletedby = oPerson
					oDatabase.AddInParameter(dbCommand, "@USRP_DeletedAt", DbType.DateTime, oRepositoryItem.DeletedAt)
					oDatabase.AddInParameter(dbCommand, "@USRP_DeletedBy", DbType.Int64, oRepositoryItem.Deletedby.ID)
					oDatabase.AddInParameter(dbCommand, "@USRP_isDeleted", DbType.Int32, 1)
					dbCommand.Connection = connection
					dbCommand.ExecuteNonQuery()

					oRepositoryItem.IsDeleted = True
					iReturn = oRepositoryItem
				Catch ex As Exception
					oRepositoryItem.DeletedAt = Nothing
					oRepositoryItem.Deletedby = Nothing
					iReturn = Nothing
				Finally
					If connection.State <> ConnectionState.Closed Then
						connection.Close()
					End If
				End Try
			End Using
			Return iReturn
		End Function

		Private ReadOnly Property SQL_UserLabels(ByVal oPerson As Person, ByVal oLabel As LabelTag) As String
			Get
				Dim stringaSQL As String = "SELECT LABL_ID, LABL_CMNT_ID, LABL_DisplayName, LABL_Description, LABL_FatherID, LABL_CreateBy, LABL_CreateAt, LABL_ModifiedAt, LABL_ModifiedBy, LABL_isDeleted, LABL_HardLinks, LABL_isSystem,  LABL_DeletedAt, LABL_DeletedBy, " _
				  & " PersonM.PRSN_id AS PersonM_id, PersonM.PRSN_nome AS  PersonM_Nome, PersonM.PRSN_cognome AS  PersonM_Cognome, PersonD.PRSN_nome AS PersonD_Nome, PersonD.PRSN_id AS PersonD_id, PersonD.PRSN_cognome AS PersonD_Cognome " _
				& " FROM RP_LABEL  LEFT OUTER JOIN PERSONA AS PersonM ON LABL_ModifiedBy = PersonM.PRSN_id LEFT OUTER JOIN PERSONA AS PersonD ON LABL_DeletedBy = PersonD.PRSN_id "
				If oLabel Is Nothing Then
					stringaSQL &= " Where LABL_CMNT_ID is null and LABL_CreateBy = " & oPerson.ID & " AND LABL_FatherID = 0  ORDER BY LABL_Description"
				Else
					stringaSQL &= " Where LABL_CMNT_ID is null and LABL_CreateBy = " & oPerson.ID & " AND LABL_FatherID = " & oLabel.ID & "  ORDER BY LABL_Description"
				End If

				Return stringaSQL
			End Get
		End Property
		Private ReadOnly Property SQL_UserLabelsPlain(ByVal oPerson As Person) As String
			Get
				Dim stringaSQL As String = "SELECT LABL_ID, LABL_CMNT_ID, LABL_DisplayName, LABL_Description, LABL_FatherID, LABL_CreateBy, LABL_CreateAt, LABL_ModifiedAt, LABL_ModifiedBy, LABL_isDeleted, LABL_HardLinks, LABL_isSystem,  LABL_DeletedAt, LABL_DeletedBy, " _
				  & " PersonM.PRSN_id AS PersonM_id, PersonM.PRSN_nome AS  PersonM_Nome, PersonM.PRSN_cognome AS  PersonM_Cognome, PersonD.PRSN_nome AS PersonD_Nome, PersonD.PRSN_id AS PersonD_id, PersonD.PRSN_cognome AS PersonD_Cognome " _
				& " FROM RP_LABEL  LEFT OUTER JOIN PERSONA AS PersonM ON LABL_ModifiedBy = PersonM.PRSN_id LEFT OUTER JOIN PERSONA AS PersonD ON LABL_DeletedBy = PersonD.PRSN_id " _
				& " Where LABL_CMNT_ID is null and LABL_CreateBy = " & oPerson.ID & " ORDER BY LABL_Description"
				Return stringaSQL
			End Get
		End Property

		Private ReadOnly Property SQL_CommunityLabels(ByVal oPerson As Person, ByVal oCommunity As Community) As String
			Get
				Dim stringaSQL As String = "SELECT LABL_ID, LABL_CMNT_ID, LABL_DisplayName, LABL_Description, LABL_FatherID, LABL_CreateBy, LABL_CreateAt, LABL_ModifiedAt, LABL_ModifiedBy, LABL_isDeleted, LABL_HardLinks, LABL_isSystem,  LABL_DeletedAt, LABL_DeletedBy, " _
				  & " PersonaO.PRSN_id AS PersonO_id, PersonaO.PRSN_nome AS  PersonO_Nome, PersonaO.PRSN_cognome AS  PersonO_Cognome, PersonM.PRSN_id AS PersonM_id, PersonM.PRSN_nome AS  PersonM_Nome, PersonM.PRSN_cognome AS  PersonM_Cognome, PersonD.PRSN_nome AS PersonD_Nome, PersonD.PRSN_id AS PersonD_id, PersonD.PRSN_cognome AS PersonD_Cognome " _
				& " FROM RP_LABEL INNER JOIN PERSONA as PersonaO on LABL_CreateBy=PersonaO.PRSN_id LEFT OUTER JOIN PERSONA AS PersonM ON LABL_ModifiedBy = PersonM.PRSN_id LEFT OUTER JOIN PERSONA AS PersonD ON LABL_DeletedBy = PersonD.PRSN_id " _
				& " Where LABL_CMNT_ID = " & oCommunity.ID & " ORDER BY LABL_Description"
				Return stringaSQL
			End Get
		End Property
		Private ReadOnly Property SQL_GetFileData(ByVal FileID As String) As String
			Get
				Dim stringaSQL As String = "SELECT     FLRP_ID, FLRP_Extension, FLRP_DisplayName, FLRP_Description, FLRP_FileSystemName, FLRP_FileSystemPath, FLRP_Size, FLRP_ContentType, FLRP_CreateBy, FLRP_CreateAt, FLRP_ModifiedAt, FLRP_ModifiedBy, FLRP_DeletedAt, FLRP_DeletedBy, FLRP_isDeleted, FLRP_HardLinks, PersonO.PRSN_id AS PersonO_id, " _
				& " PersonO.PRSN_nome AS PersonO_Nome, PersonO.PRSN_cognome AS PersonO_Cognome, PersonM.PRSN_id AS PersonM_id, PersonM.PRSN_nome AS PersonM_Nome, PersonM.PRSN_cognome AS PersonM_Cognome, PersonD.PRSN_nome AS PersonD_Nome, PersonD.PRSN_id AS PersonD_id, PersonD.PRSN_cognome AS PersonD_Cognome " _
				  & " FROM         dbo.RP_FILE_REPOSITORY LEFT OUTER JOIN dbo.PERSONA AS PersonO ON FLRP_CreateBy = PersonO.PRSN_id LEFT OUTER JOIN dbo.PERSONA AS PersonM ON FLRP_ModifiedBy = PersonM.PRSN_id LEFT OUTER JOIN dbo.PERSONA AS PersonD ON FLRP_DeletedBy = PersonD.PRSN_id " _
				  & " where FLRP_ID='" & FileID & "'"
				Return stringaSQL
			End Get
		End Property
		Private ReadOnly Property SQL_UserFiles(ByVal oPerson As Person) As String
			Get
				Dim stringaSQL As String = "SELECT     PersonO.PRSN_id AS PersonO_id, PersonO.PRSN_nome AS PersonO_Nome, PersonO.PRSN_cognome AS PersonO_Cognome, PersonM.PRSN_id AS PersonM_id, PersonM.PRSN_nome AS PersonM_Nome, PersonM.PRSN_cognome AS PersonM_Cognome, PersonD.PRSN_nome AS PersonD_Nome, PersonD.PRSN_id AS PersonD_id, PersonD.PRSN_cognome AS PersonD_Cognome, " _
				 & " USRP_ID, USRP_FLRP_ID, USRP_CreateBy, USRP_CreateAt, USRP_ModifiedBy, USRP_ModifiedAt,  USRP_DeletedBy, USRP_DeletedAt, USRP_isDeleted " _
				  & " FROM         dbo.RP_USER_REPOSITORY LEFT OUTER JOIN dbo.PERSONA AS PersonO ON USRP_CreateBy = PersonO.PRSN_id LEFT OUTER JOIN dbo.PERSONA AS PersonM ON USRP_ModifiedBy = PersonM.PRSN_id LEFT OUTER JOIN dbo.PERSONA AS PersonD ON USRP_DeletedBy = PersonD.PRSN_id" _
				& " WHERE USRP_CreateBy = " & oPerson.ID
				Return stringaSQL
			End Get
		End Property
		Private ReadOnly Property SQL_FileLabel(ByVal FileId As Guid) As String
			Get
				Dim stringaSQL As String = "SELECT LBUR_LABL_ID as LABL_ID FROM RP_LABEL_USER_REPOSITORY where LBUR_FLRP_ID='" & FileId.ToString & "'"
				Return stringaSQL
			End Get
		End Property

	
	End Class
End Namespace