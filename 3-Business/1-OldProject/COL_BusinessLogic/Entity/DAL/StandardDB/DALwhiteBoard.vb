Imports Comol.Entity

Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common

Namespace Comol.DAL.StandardDB
	Public Class DALwhiteBoard
		Inherits DALabstract

		Public Sub New(ByVal oDbconnection As ConnectionDB)
			MyBase.new(oDbconnection)
		End Sub

		'Public Shared Function GetAttiva(ByVal LinguaID As Integer, ByVal ComunitaID As Integer) As COL_BachecaComunita
		'	Dim oListaBacheca As New List(Of COL_BachecaComunita)
		'	oListaBacheca = COL_BachecaComunita.List(LinguaID, ComunitaID, FiltroVisibilità.Visibile)

		'	If oListaBacheca.Count > 0 Then
		'		Return oListaBacheca(0)
		'	Else
		'		Return Nothing
		'	End If
		'End Function

		Public Sub Add(ByVal oWhiteBoard As WhiteBoard)
			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = "sp_BachecaComunita_Add"
					Dim dbCommand As DbCommand = oDatabase.GetStoredProcCommand(sqlCommand)
					oDatabase.AddOutParameter(dbCommand, "BCHC_Id", DbType.Int64, 4)
					oDatabase.AddInParameter(dbCommand, "BCHC_Versione", DbType.Int32, oWhiteBoard.Version)
					oDatabase.AddInParameter(dbCommand, "BCHC_CMNT_Id", DbType.Int32, oWhiteBoard.CommunityOwner.ID)
					oDatabase.AddInParameter(dbCommand, "BCHC_isSemplificata", DbType.Int32, IIf(oWhiteBoard.isSimpleVersion, 1, 0))
					oDatabase.AddInParameter(dbCommand, "BCHC_isDeleted", DbType.Int32, IIf(oWhiteBoard.IsDeleted, 1, 0))
					oDatabase.AddInParameter(dbCommand, "BCHC_isAttiva", DbType.Int32, IIf(oWhiteBoard.isCurrentVersion, 1, 0))
					oDatabase.AddInParameter(dbCommand, "BCHC_testo", DbType.String, oWhiteBoard.Text)
					oDatabase.AddInParameter(dbCommand, "CreatoreID", DbType.Int32, oWhiteBoard.CreatedBy.ID)
					oDatabase.AddInParameter(dbCommand, "BCHC_scorrimento", DbType.Int32, CType(oWhiteBoard.Speed, SpeedStatus))
					oDatabase.AddInParameter(dbCommand, "BCHC_face", DbType.String, oWhiteBoard.Style.Face)
					oDatabase.AddInParameter(dbCommand, "BCHC_size", DbType.Int32, oWhiteBoard.Style.Size)
					oDatabase.AddInParameter(dbCommand, "BCHC_color", DbType.String, oWhiteBoard.Style.Color)
					oDatabase.AddInParameter(dbCommand, "BCHC_align", DbType.String, oWhiteBoard.Style.Align)
					oDatabase.AddInParameter(dbCommand, "BCHC_sfondo", DbType.String, oWhiteBoard.Style.BackGround)
					oDatabase.AddInParameter(dbCommand, "BCHC_CreataIl", DbType.String, Main.DateToString(oWhiteBoard.CreatedAt))
					oDatabase.AddInParameter(dbCommand, "BCHC_ModificataIl", DbType.String, Main.DateToString(oWhiteBoard.ModifiedAt))

					dbCommand.Connection = connection
					dbCommand.ExecuteNonQuery()
					oWhiteBoard.ID = dbCommand.Parameters("LNGU_Id").Value
				Catch ex As Exception
					oWhiteBoard.ID = 0
				Finally
					If connection.State <> ConnectionState.Closed Then
						connection.Close()
					End If
				End Try
			End Using
		End Sub
		Public Sub Save(ByVal oWhiteBoard As WhiteBoard)
			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = "sp_BachecaComunita_Save"
					Dim dbCommand As DbCommand = oDatabase.GetStoredProcCommand(sqlCommand)
					oDatabase.AddInParameter(dbCommand, "BCHC_Id", DbType.Int64, oWhiteBoard.ID)
					oDatabase.AddInParameter(dbCommand, "BCHC_testo", DbType.String, oWhiteBoard.Text)
					oDatabase.AddInParameter(dbCommand, "PersonaID", DbType.Int32, oWhiteBoard.ModifiedBy.ID)
					oDatabase.AddInParameter(dbCommand, "BCHC_scorrimento", DbType.Int32, CType(oWhiteBoard.Speed, SpeedStatus))
					oDatabase.AddInParameter(dbCommand, "BCHC_face", DbType.String, oWhiteBoard.Style.Face)
					oDatabase.AddInParameter(dbCommand, "BCHC_size", DbType.Int32, oWhiteBoard.Style.Size)
					oDatabase.AddInParameter(dbCommand, "BCHC_color", DbType.String, oWhiteBoard.Style.Color)
					oDatabase.AddInParameter(dbCommand, "BCHC_align", DbType.String, oWhiteBoard.Style.Align)
					oDatabase.AddInParameter(dbCommand, "BCHC_sfondo", DbType.String, oWhiteBoard.Style.BackGround)
					oDatabase.AddInParameter(dbCommand, "BCHC_ModificataIl", DbType.String, Main.DateToString(oWhiteBoard.ModifiedAt))

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
		Public Sub ChangeActivation(ByVal oWhiteBoard As WhiteBoard)
			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = "sp_BachecaComunita_ChangeActivation"
					Dim dbCommand As DbCommand = oDatabase.GetStoredProcCommand(sqlCommand)
					oDatabase.AddInParameter(dbCommand, "BCHC_Id", DbType.Int64, oWhiteBoard.ID)
					oDatabase.AddInParameter(dbCommand, "PersonaID", DbType.Int32, oWhiteBoard.ModifiedBy.ID)
					oDatabase.AddInParameter(dbCommand, "BCHC_ModificataIl", DbType.String, Main.DateToString(oWhiteBoard.ModifiedAt))
					oDatabase.AddInParameter(dbCommand, "isAttiva", DbType.Int32, IIf(oWhiteBoard.isCurrentVersion, 1, 0))
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
		Public Sub Delete(ByVal oWhiteBoard As WhiteBoard)
			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = "sp_BachecaComunita_Delete"
					Dim dbCommand As DbCommand = oDatabase.GetStoredProcCommand(sqlCommand)
					oDatabase.AddInParameter(dbCommand, "BCHC_Id", DbType.Int64, oWhiteBoard.ID)
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
		Public Sub DeleteVirtual(ByVal oWhiteBoard As WhiteBoard)
			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = "sp_BachecaComunita_DeleteVirtual"
					Dim dbCommand As DbCommand = oDatabase.GetStoredProcCommand(sqlCommand)
					oDatabase.AddInParameter(dbCommand, "BCHC_Id", DbType.Int64, oWhiteBoard.ID)
					oDatabase.AddInParameter(dbCommand, "PersonaID", DbType.Int32, oWhiteBoard.Deletedby.ID)
					oDatabase.AddInParameter(dbCommand, "BCHC_CancellataIl", DbType.String, Main.DateToString(oWhiteBoard.DeletedAt))
					oDatabase.AddInParameter(dbCommand, "Delete", DbType.Int32, 1)
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
		Public Sub UnDeleteVirtual(ByVal oWhiteBoard As WhiteBoard)
			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = "sp_BachecaComunita_DeleteVirtual"
					Dim dbCommand As DbCommand = oDatabase.GetStoredProcCommand(sqlCommand)
					oDatabase.AddInParameter(dbCommand, "BCHC_Id", DbType.Int64, oWhiteBoard.ID)
					oDatabase.AddInParameter(dbCommand, "PersonaID", DbType.Int32, oWhiteBoard.Deletedby.ID)
					oDatabase.AddInParameter(dbCommand, "Delete", DbType.Int32, 0)
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
		Public Function List(ByVal CommunityID As Integer, ByVal LanguageID As Integer) As List(Of WhiteBoard)
			Dim oLista As New List(Of WhiteBoard)

			Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.ConnectionDB.Name)
			Using connection As DbConnection = oDatabase.CreateConnection()
				connection.Open()

				Try
					Dim sqlCommand As String = "sp_BachecaComunita_RetrieveListFromDB"
					Dim dbCommand As DbCommand = oDatabase.GetStoredProcCommand(sqlCommand)
					oDatabase.AddInParameter(dbCommand, "ComunitaID", DbType.Int32, CommunityID)
					oDatabase.AddInParameter(dbCommand, "LinguaID", DbType.Int32, LanguageID)
					dbCommand.Connection = connection

					Dim iDatareader As IDataReader = dbCommand.ExecuteReader()
					While iDatareader.Read
						Try
							Dim oChangedBy As New Person(iDatareader("PRSN_ID"), iDatareader("PRSN_Nome"), iDatareader("PRSN_Cognome"))
							Dim oOwner As New Person(iDatareader("Creatore_ID"), iDatareader("Creatore_Nome"), iDatareader("Creatore_Cognome"))
                            Dim oStile As TextStyles
                            Dim isSemplificata As Boolean = GenericValidator.ValBool(iDatareader("BCHC_isSemplificata"), False)
                            Dim isAttiva As Boolean = GenericValidator.ValBool(iDatareader("BCHC_isAttiva"), False)
                            Dim isDeleted As Boolean = GenericValidator.ValBool(iDatareader("BCHC_isDeleted"), False)
                            Dim oSpeed As Main.VelocitaScorrimento = GenericValidator.ValInteger(iDatareader("BCHC_scorrimento"), 1)


                            oStile = New TextStyles
							oStile.Align = GenericValidator.ValString(iDatareader("BCHC_align"), "")
							oStile.BackGround = GenericValidator.ValString(iDatareader("BCHC_sfondo"), "")
							oStile.Color = GenericValidator.ValString(iDatareader("BCHC_color"), "")
							oStile.Face = GenericValidator.ValString(iDatareader("BCHC_face"), "")
							oStile.Size = GenericValidator.ValInteger(iDatareader("BCHC_size"), 3)
							'If Not isSemplificata And oStile.isDefined = flase Then
							'	oStile = Nothing
							'End If


							Dim oWhiteBoard As New WhiteBoard
							With oWhiteBoard
								.CommunityOwner = New Community(CommunityID)
								.CreatedAt = GenericValidator.ValData(iDatareader("BCHC_CreataIl"), Nothing)
								.CreatedBy = oOwner
								.DeletedAt = GenericValidator.ValData(iDatareader("BCHC_CancellataIl"), Nothing)
								.ID = iDatareader("BCHC_ID")
								.isCurrentVersion = isAttiva
								.IsDeleted = isDeleted
								.isSimpleVersion = isSemplificata
								.ModifiedAt = GenericValidator.ValData(iDatareader("BCHC_ModificataIl"), Nothing)
								.ModifiedBy = oChangedBy
								.Speed = oSpeed
								.Style = oStile
								.Text = GenericValidator.ValString(iDatareader("BCHC_testo"), "")
								.Version = GenericValidator.ValInteger(iDatareader("BCHC_Versione"), 0)

							End With
						Catch ex As Exception

						End Try
					End While
				Catch ex As Exception

				Finally
					If connection.State <> ConnectionState.Closed Then
						connection.Close()
					End If
				End Try
			End Using
			Return oLista
		End Function
	End Class
End Namespace