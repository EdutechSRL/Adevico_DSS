Namespace Comol.DAL.StandardDB
	Public Class DALprovincia
		Implements iDALbase(Of Provincia)

		Public Function Add(ByVal oObject As Provincia) As Provincia Implements iDALbase(Of Provincia).Add
			Dim oRequest As New COL_Request
			Dim oParam As New COL_Request.Parameter
			Dim objAccesso As New COL_DataAccess

			With oRequest
				.Command = "sp_Provincia_Aggiungi"
				.CommandType = CommandType.StoredProcedure
				oParam = objAccesso.GetAdvancedParameter("@PRVN_Id", "", ParameterDirection.Output, SqlDbType.Int)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetAdvancedParameter("@PRVN_nome", oObject.Nome, ParameterDirection.Input, SqlDbType.VarChar, , 50)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetAdvancedParameter("@PRVN_sigla", oObject.Sigla, ParameterDirection.Input, SqlDbType.VarChar, , 2)
				.Parameters.Add(oParam)
				.Role = COL_Request.UserRole.Admin
				.transactional = False
			End With
			Try
				objAccesso.GetExecuteNotQuery(oRequest)
				oObject.ID = oRequest.GetValueFromParameter(1)
				Return oObject
			Catch ex As Exception
				Throw New Exception(ex.Message, ex.InnerException)
			End Try
			Return Nothing
		End Function

		Public Sub Delete(ByVal oObject As Provincia) Implements iDALbase(Of Provincia).Delete

		End Sub

		Public Sub Save(ByVal oObject As Provincia) Implements iDALbase(Of Provincia).Save
			Dim oRequest As New COL_Request
			Dim oParam As New COL_Request.Parameter
			Dim objAccesso As New COL_DataAccess

			With oRequest
				.Command = "sp_Provincia_Modifica"
				.CommandType = CommandType.StoredProcedure
				oParam = objAccesso.GetAdvancedParameter("@PRVN_Id", oObject.ID, ParameterDirection.Input, SqlDbType.Int)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetAdvancedParameter("@PRVN_nome", oObject.Nome, ParameterDirection.Input, SqlDbType.VarChar, , 50)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetAdvancedParameter("@PRVN_sigla", oObject.Sigla, ParameterDirection.Input, SqlDbType.VarChar, , 2)
				.Parameters.Add(oParam)
				.Role = COL_Request.UserRole.Admin
				.transactional = False
			End With
			Try
				objAccesso.GetExecuteNotQuery(oRequest)

			Catch ex As Exception
				Throw New Exception(ex.Message, ex.InnerException)
			End Try
		End Sub

		Public Function List(Optional ByVal Lingua As Lingua = Nothing) As System.Collections.Generic.List(Of Provincia) Implements iDALbase(Of Provincia).List
			Dim oLista As New List(Of Provincia)
			Dim oRequest As New COL_Request
			Dim oParam As New COL_Request.Parameter
			Dim objAccesso As New COL_DataAccess
			Dim oDatareader As IDataReader

			With oRequest
				.Command = "sp_Provincia_Elenca"
				.CommandType = CommandType.StoredProcedure

				.Role = COL_Request.UserRole.Admin
				.transactional = False
			End With

			Try
				oDatareader = objAccesso.GetdataReader(oRequest)
				While oDatareader.Read
					Try
						oLista.Add(New Provincia(oDatareader("PRVN_Id"), GenericValidator.ValString(oDatareader("PRVN_nome"), ""), GenericValidator.ValString(oDatareader("PRVN_sigla"), "")))
					Catch ex As Exception

					End Try
				End While
				oDatareader.Close()
			Catch ex As Exception
			Finally
				If Not IsNothing(oDatareader) Then
					If oDatareader.IsClosed = False Then
						oDatareader.Close()
					End If
				End If
			End Try
			Return oLista
		End Function
	End Class
End Namespace