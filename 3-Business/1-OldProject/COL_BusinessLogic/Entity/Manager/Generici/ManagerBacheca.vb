Imports Comol.Entity

Namespace Comol.Manager
	Public Class ManagerBacheca
		Inherits ObjectBase
		Implements iManager

#Region "Private property"
		Private _UseCache As Boolean
		Private _CurrentUser As COL_Persona
		Private _CurrentCommunity As COL_Comunita
		Private _CurrentDB As ConnectionDB
#End Region

#Region "Public property"
		Public ReadOnly Property CurrentCommunity() As Comunita.COL_Comunita Implements iManager.CurrentCommunity
			Get
				Return _CurrentCommunity
			End Get
		End Property
		Public ReadOnly Property CurrentUser() As CL_persona.COL_Persona Implements iManager.CurrentUser
			Get
				Return _CurrentUser
			End Get
		End Property
		Private ReadOnly Property UseCache() As Boolean Implements iManager.UseCache
			Get
				Return _UseCache
			End Get
		End Property
		Private ReadOnly Property CurrentDB() As ConnectionDB Implements iManager.CurrentDB
			Get
				If IsNothing(_CurrentDB) Then
					_CurrentDB = ManagerConfiguration.GetInstance.DBconnectionSettings.GetConnection(DBconnectionSettings.DBsetting.COMOL, ConnectionType.SQL)
				End If
				Return _CurrentDB
			End Get
		End Property
#End Region

		Public Shared Function GetCurrentDB() As ConnectionDB
			Return ManagerConfiguration.GetInstance.DBconnectionSettings.GetConnection(DBconnectionSettings.DBsetting.COMOL, ConnectionType.SQL)
		End Function

		Public Sub Add(ByVal oLingua As Lingua)
			Try
				Dim oLinguaCreata As Lingua
				Dim oDALlingua As New DAL.StandardDB.DALlingua(Me.CurrentDB)
				oLinguaCreata = oDALlingua.Add(oLingua)

				If IsNothing(oLinguaCreata) Then
					' generare errore
				Else

				End If
				PurgeCacheItems(CachePolicy.Lingua)
			Catch ex As Exception

			End Try

		End Sub
		Public Sub Save(ByVal oLingua As Lingua)
			Try
				Dim oDALlingua As New DAL.StandardDB.DALlingua(Me.CurrentDB)
				oDALlingua.Save(oLingua)
			Catch ex As Exception

			End Try
			PurgeCacheItems(CachePolicy.Lingua)
		End Sub
		Public Shared Function GetByID(ByVal LinguaID As Integer) As Lingua

			'Dim oList As List(Of Lingua) = ManagerLingua.List
			'Dim o As Lingua = (From oLingua As Lingua In ManagerLingua.List Where oLingua.ID = 1).FirstOrDefault

			'Dim oList2 As List(Of Lingua) = ManagerLingua.List

			Return (From oLingua As Lingua In ManagerLingua.List Where oLingua.ID = LinguaID).FirstOrDefault

			'Dim oLista As New List(Of Lingua)
			'oLista = 

			'If oLista.Count = 0 Then
			'	Return Nothing
			'Else
			'	Dim oLingua As Lingua
			'	oLingua = oLista.Find(New GenericPredicate(Of Lingua, Integer)(LinguaID, AddressOf Lingua.FindByID))
			'	Return oLingua
			'End If
		End Function
		Public Shared Function GetByCode(ByVal Codice As String) As Lingua
			Return (From oLingua As Lingua In ManagerLingua.List Where oLingua.Codice = Codice).FirstOrDefault
			'Dim oLista As New List(Of Lingua)
			'oLista = ManagerLingua.List

			'If oLista.Count = 0 Then
			'	Return Nothing
			'Else
			'	Dim oLingua As Lingua
			'	oLingua = oLista.Find(New GenericPredicate(Of Lingua, String)(Codice, AddressOf Lingua.FindByCode))
			'	Return oLingua
			'End If
		End Function
		Public Shared Function GetDefault() As Lingua
			Return (From oLingua As Lingua In ManagerLingua.List Where oLingua.isDefault).FirstOrDefault
			'Dim oLista As New List(Of Lingua)
			'oLista = ManagerLingua.List

			'If oLista.Count = 0 Then
			'	Return Nothing
			'Else
			'	Dim oLingua As Lingua
			'	oLingua = oLista.Find(New GenericPredicate(Of Lingua, Boolean)(True, AddressOf Lingua.FindByDefault))
			'	Return oLingua
			'End If
		End Function
		Public Shared Function GetByCodeOrDefault(ByVal Codice As String) As Lingua
			Dim oLingua As Lingua = ManagerLingua.GetByCode(Codice)
			If IsNothing(oLingua) Then
				Return ManagerLingua.GetDefault
			Else
				Return oLingua
			End If
			'Dim oLista As New List(Of Lingua)
			'oLista = ManagerLingua.List

			'If oLista.Count = 0 Then
			'	Return Nothing
			'Else
			'	Dim oLingua As Lingua
			'	oLingua = oLista.Find(New GenericPredicate(Of Lingua, String)(Codice, AddressOf Lingua.FindByCode))
			'	If IsNothing(oLingua) Then
			'		oLingua = ManagerLingua.GetDefault
			'	End If
			'	Return oLingua
			'End If
		End Function
		Public Shared Function FindByIDfromList(ByVal iLista As List(Of Lingua), ByVal iLinguaID As String) As Lingua
			'Dim oLingua As Lingua

			Return (From oLingua As Lingua In iLista Where oLingua.ID = iLinguaID).FirstOrDefault
			'oLingua = iLista.Find(New GenericPredicate(Of Lingua, Integer)(iLinguaID, AddressOf Lingua.FindByID))
			'Return oLingua
		End Function

		Public Shared Function List(Optional ByVal sortExpression As String = "", Optional ByVal oSortDirection As sortDirection = sortDirection.None, Optional ByVal ForceRetrieve As Boolean = False) As List(Of Lingua)
			Dim oLista As New List(Of Lingua)


			Dim cacheKey As String = CachePolicy.Lingua
			If ObjectBase.Cache(cacheKey) Is Nothing Or ForceRetrieve Then
				oLista = GetLanguages()
				ObjectBase.Cache.Insert(cacheKey, oLista, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza60minuti)
			Else
				oLista = CType(ObjectBase.Cache(cacheKey), List(Of Lingua))
			End If

			If sortExpression = "ID" Then
				If oSortDirection = sortDirection.Ascending Then
					Dim oQuery = From oLingua As Lingua In oLista Order By oLingua.ID Ascending
					Return oQuery.ToList
				Else
					Dim oQuery = From oLingua As Lingua In oLista Order By oLingua.ID Descending
					Return oQuery.ToList
				End If
			ElseIf sortExpression = "Nome" Then
				If oSortDirection = sortDirection.Ascending Then
					Dim oQuery = From oLingua As Lingua In oLista Order By oLingua.Nome Ascending
					Return oQuery.ToList
				Else
					Dim oQuery = From oLingua As Lingua In oLista Order By oLingua.Nome Descending
					Return oQuery.ToList
				End If
			End If
			Return oLista
		End Function
		Private Shared Function GetLanguages() As List(Of Lingua)
			Dim oDALlingua As New DAL.StandardDB.DALlingua(GetCurrentDB)
			Return oDALlingua.List
		End Function


		'Public Shared Function GetByID(ByVal BachecaID As Integer, ByVal LinguaID As Integer, ByVal ComunitaID As Integer) As COL_BachecaComunita
		'	Dim oListaBacheca As New List(Of COL_BachecaComunita)
		'	oListaBacheca = COL_BachecaComunita.List(LinguaID, ComunitaID, FiltroVisibilità.Tutti)

		'	If oListaBacheca.Count = 0 Then
		'		Return Nothing
		'	Else
		'		Dim oBacheca As COL_BachecaComunita
		'		oBacheca = oListaBacheca.Find(New GenericPredicate(Of COL_BachecaComunita, Integer)(BachecaID, AddressOf FindByID))
		'		Return oBacheca
		'	End If
		'End Function
		'Public Shared Function GetAttiva(ByVal LinguaID As Integer, ByVal ComunitaID As Integer) As COL_BachecaComunita
		'	Dim oListaBacheca As New List(Of COL_BachecaComunita)
		'	oListaBacheca = COL_BachecaComunita.List(LinguaID, ComunitaID, FiltroVisibilità.Visibile)

		'	If oListaBacheca.Count > 0 Then
		'		Return oListaBacheca(0)
		'	Else
		'		Return Nothing
		'	End If
		'End Function
		'Public Shared Function GetLastCreated(ByVal LinguaID As Integer, ByVal ComunitaID As Integer) As COL_BachecaComunita
		'	Dim oListaBacheca As New List(Of COL_BachecaComunita)
		'	oListaBacheca = COL_BachecaComunita.List(LinguaID, ComunitaID, FiltroVisibilità.Tutti, "DataCreazione", "desc")

		'	If oListaBacheca.Count > 0 Then
		'		Return oListaBacheca(0)
		'	Else
		'		Return Nothing
		'	End If
		'End Function

		'Public Sub Add(ByVal LinguaID As Integer)
		'	Dim oRequest As New COL_Request
		'	Dim oParam As New COL_Request.Parameter
		'	Dim objAccesso As New COL_DataAccess
		'	Dim oBachecaUltima As COL_BachecaComunita = COL_BachecaComunita.GetLastCreated(LinguaID, Me._Comunita.Id)

		'	If Not IsNothing(oBachecaUltima) Then
		'		Me._Versione = oBachecaUltima.Versione + 1
		'	Else
		'		Me._Versione = 0
		'	End If
		'	Me._DataCreazione = Now
		'	Me._DataModifica = Now
		'	Me._ModificatoDa = Me._Creatore
		'	With oRequest
		'		.Command = "sp_BachecaComunita_Add"
		'		.CommandType = CommandType.StoredProcedure
		'		oParam = objAccesso.GetAdvancedParameter("@BCHC_Id", "", ParameterDirection.Output, SqlDbType.BigInt)
		'		.Parameters.Add(oParam)
		'		oParam = objAccesso.GetAdvancedParameter("@BCHC_Versione", Me._Versione, ParameterDirection.Input, SqlDbType.Int)
		'		.Parameters.Add(oParam)
		'		oParam = objAccesso.GetAdvancedParameter("@BCHC_CreataIl", Main.DateToString(Me._DataCreazione), ParameterDirection.Input, SqlDbType.VarChar, , 50)
		'		.Parameters.Add(oParam)
		'		oParam = objAccesso.GetAdvancedParameter("@BCHC_ModificataIl", Main.DateToString(Me._DataModifica), ParameterDirection.Input, SqlDbType.VarChar, , 50)
		'		.Parameters.Add(oParam)
		'		oParam = objAccesso.GetAdvancedParameter("@BCHC_CMNT_Id", Me._Comunita.Id, ParameterDirection.Input, SqlDbType.Int)
		'		.Parameters.Add(oParam)
		'		oParam = objAccesso.GetAdvancedParameter("@CreatoreID", Me._Creatore.Id, ParameterDirection.Input, SqlDbType.Int)
		'		.Parameters.Add(oParam)
		'		oParam = objAccesso.GetAdvancedParameter("@BCHC_isSemplificata", Me._isSemplificata, ParameterDirection.Input, SqlDbType.Bit)
		'		.Parameters.Add(oParam)
		'		oParam = objAccesso.GetAdvancedParameter("@BCHC_isDeleted", Me._isDeleted, ParameterDirection.Input, SqlDbType.Bit)
		'		.Parameters.Add(oParam)
		'		oParam = objAccesso.GetAdvancedParameter("@BCHC_isAttiva", Me._isAttiva, ParameterDirection.Input, SqlDbType.Bit)
		'		.Parameters.Add(oParam)
		'		oParam = objAccesso.GetAdvancedParameter("@BCHC_scorrimento", CType(Me._VelocitaScorrimento, Main.VelocitaScorrimento), ParameterDirection.Input, SqlDbType.Int)
		'		.Parameters.Add(oParam)
		'		oParam = objAccesso.GetAdvancedParameter("@BCHC_testo", _Testo, ParameterDirection.Input, SqlDbType.Text)
		'		.Parameters.Add(oParam)
		'		oParam = objAccesso.GetAdvancedParameter("@BCHC_face", Me._Stile.Face, ParameterDirection.Input, SqlDbType.VarChar, , 50)
		'		.Parameters.Add(oParam)
		'		oParam = objAccesso.GetAdvancedParameter("@BCHC_size", Me._Stile.Size, ParameterDirection.Input, SqlDbType.Int)
		'		.Parameters.Add(oParam)
		'		oParam = objAccesso.GetAdvancedParameter("@BCHC_color", Me._Stile.Color, ParameterDirection.Input, SqlDbType.VarChar, , 50)
		'		.Parameters.Add(oParam)
		'		oParam = objAccesso.GetAdvancedParameter("@BCHC_align", Me._Stile.Align, ParameterDirection.Input, SqlDbType.VarChar, , 50)
		'		.Parameters.Add(oParam)
		'		oParam = objAccesso.GetAdvancedParameter("@BCHC_sfondo", Me._Stile.BackGround, ParameterDirection.Input, SqlDbType.VarChar, , 50)
		'		.Parameters.Add(oParam)

		'		.Role = COL_Request.UserRole.Admin
		'		.transactional = False
		'	End With
		'	Try
		'		objAccesso.GetExecuteNotQuery(oRequest)
		'		Me._ID = oRequest.GetValueFromParameter(1)

		'		PurgeCacheItems(CachePolicy.Bacheca(Me._Comunita.Id))
		'		Me._errore = Errori_Db.None
		'	Catch ex As Exception
		'		Me._ID = -1
		'		Me._errore = Errori_Db.DBInsert
		'	End Try
		'End Sub
		'Public Sub Save()
		'	Dim oRequest As New COL_Request
		'	Dim oParam As New COL_Request.Parameter
		'	Dim objAccesso As New COL_DataAccess

		'	Dim oDataModifica As DateTime = Now
		'	With oRequest
		'		.Command = "sp_BachecaComunita_Save"
		'		.CommandType = CommandType.StoredProcedure
		'		oParam = objAccesso.GetAdvancedParameter("@BCHC_Id", Me._ID, ParameterDirection.Input, SqlDbType.BigInt)
		'		.Parameters.Add(oParam)
		'		oParam = objAccesso.GetAdvancedParameter("@BCHC_ModificataIl", Main.DateToString(oDataModifica), ParameterDirection.Input, SqlDbType.VarChar, , 50)
		'		.Parameters.Add(oParam)
		'		oParam = objAccesso.GetAdvancedParameter("@PersonaID", Me._ModificatoDa.Id, ParameterDirection.Input, SqlDbType.Int)
		'		.Parameters.Add(oParam)
		'		oParam = objAccesso.GetAdvancedParameter("@BCHC_isSemplificata", Me._isSemplificata, ParameterDirection.Input, SqlDbType.Bit)
		'		.Parameters.Add(oParam)
		'		oParam = objAccesso.GetAdvancedParameter("@BCHC_scorrimento", CType(Me._VelocitaScorrimento, Main.VelocitaScorrimento), ParameterDirection.Input, SqlDbType.Int)
		'		.Parameters.Add(oParam)
		'		oParam = objAccesso.GetAdvancedParameter("@BCHC_testo", _Testo, ParameterDirection.Input, SqlDbType.Text)
		'		.Parameters.Add(oParam)
		'		oParam = objAccesso.GetAdvancedParameter("@BCHC_face", Me._Stile.Face, ParameterDirection.Input, SqlDbType.VarChar, , 50)
		'		.Parameters.Add(oParam)
		'		oParam = objAccesso.GetAdvancedParameter("@BCHC_size", Me._Stile.Size, ParameterDirection.Input, SqlDbType.Int)
		'		.Parameters.Add(oParam)
		'		oParam = objAccesso.GetAdvancedParameter("@BCHC_color", Me._Stile.Color, ParameterDirection.Input, SqlDbType.VarChar, , 50)
		'		.Parameters.Add(oParam)
		'		oParam = objAccesso.GetAdvancedParameter("@BCHC_align", Me._Stile.Align, ParameterDirection.Input, SqlDbType.VarChar, , 50)
		'		.Parameters.Add(oParam)
		'		oParam = objAccesso.GetAdvancedParameter("@BCHC_sfondo", Me._Stile.BackGround, ParameterDirection.Input, SqlDbType.VarChar, , 50)
		'		.Parameters.Add(oParam)

		'		.Role = COL_Request.UserRole.Admin
		'		.transactional = False
		'	End With
		'	Try
		'		objAccesso.GetExecuteNotQuery(oRequest)
		'		Me._DataModifica = oDataModifica
		'		Me._errore = Errori_Db.None
		'	Catch ex As Exception
		'		Me._errore = Errori_Db.DBChange
		'	End Try
		'End Sub


		'Public Sub Activate(ByVal oPersona As COL_Persona)
		'	Dim oRequest As New COL_Request
		'	Dim oParam As New COL_Request.Parameter
		'	Dim objAccesso As New COL_DataAccess
		'	Dim oDataModifica As DateTime = Now

		'	With oRequest
		'		.Command = "sp_BachecaComunita_ChangeActivation"
		'		.CommandType = CommandType.StoredProcedure
		'		oParam = objAccesso.GetAdvancedParameter("@BCHC_Id", Me._ID, ParameterDirection.Input, SqlDbType.BigInt)
		'		.Parameters.Add(oParam)

		'		oParam = objAccesso.GetAdvancedParameter("@PersonaID", oPersona.Id, ParameterDirection.Input, SqlDbType.Int)
		'		.Parameters.Add(oParam)

		'		oParam = objAccesso.GetAdvancedParameter("@isAttiva", 1, ParameterDirection.Input, SqlDbType.Int)
		'		.Parameters.Add(oParam)

		'		oParam = objAccesso.GetAdvancedParameter("@BCHC_ModificataIl", Main.DateToString(oDataModifica), ParameterDirection.Input, SqlDbType.VarChar, , 50)
		'		.Parameters.Add(oParam)

		'		.Role = COL_Request.UserRole.Admin
		'		.transactional = False
		'	End With
		'	Try
		'		Me._isAttiva = True
		'		objAccesso.GetExecuteNotQuery(oRequest)
		'		Me._ModificatoDa = oPersona
		'		Me._DataModifica = oDataModifica
		'		Me._errore = Errori_Db.None
		'	Catch ex As Exception
		'		Me._errore = Errori_Db.DBChange
		'	End Try
		'End Sub
		'Public Sub DeActivate(ByVal oPersona As COL_Persona)
		'	Dim oRequest As New COL_Request
		'	Dim oParam As New COL_Request.Parameter
		'	Dim objAccesso As New COL_DataAccess
		'	Dim oDataModifica As DateTime = Now

		'	With oRequest
		'		.Command = "sp_BachecaComunita_ChangeActivation"
		'		.CommandType = CommandType.StoredProcedure
		'		oParam = objAccesso.GetAdvancedParameter("@BCHC_Id", Me._ID, ParameterDirection.Input, SqlDbType.BigInt)
		'		.Parameters.Add(oParam)

		'		oParam = objAccesso.GetAdvancedParameter("@PersonaID", oPersona.Id, ParameterDirection.Input, SqlDbType.Int)
		'		.Parameters.Add(oParam)

		'		oParam = objAccesso.GetAdvancedParameter("@isAttiva", 0, ParameterDirection.Input, SqlDbType.Int)
		'		.Parameters.Add(oParam)

		'		oParam = objAccesso.GetAdvancedParameter("@BCHC_ModificataIl", Main.DateToString(oDataModifica), ParameterDirection.Input, SqlDbType.VarChar, , 50)
		'		.Parameters.Add(oParam)

		'		.Role = COL_Request.UserRole.Admin
		'		.transactional = False
		'	End With
		'	Try
		'		objAccesso.GetExecuteNotQuery(oRequest)
		'		Me._isAttiva = False
		'		Me._DataModifica = oDataModifica
		'		Me._ModificatoDa = oPersona
		'		Me._errore = Errori_Db.None
		'	Catch ex As Exception
		'		Me._errore = Errori_Db.DBChange
		'	End Try
		'End Sub
		'Public Sub Delete()
		'	Dim oRequest As New COL_Request
		'	Dim oParam As New COL_Request.Parameter
		'	Dim objAccesso As New COL_DataAccess
		'	With oRequest
		'		.Command = "sp_BachecaComunita_Delete"
		'		.CommandType = CommandType.StoredProcedure
		'		oParam = objAccesso.GetAdvancedParameter("@BCHC_Id", Me._ID, ParameterDirection.Input, SqlDbType.BigInt)
		'		.Parameters.Add(oParam)

		'		.Role = COL_Request.UserRole.Admin
		'		.transactional = False
		'	End With
		'	Try
		'		Me._isDeleted = True
		'		objAccesso.GetExecuteNotQuery(oRequest)
		'		PurgeCacheItems(CachePolicy.Bacheca(Me._Comunita.Id))
		'		Me._errore = Errori_Db.None
		'	Catch ex As Exception
		'		Me._errore = Errori_Db.DBChange
		'	End Try
		'End Sub
		'Public Sub DeleteVirtual(ByVal oPersona As COL_Persona)
		'	Dim oRequest As New COL_Request
		'	Dim oParam As New COL_Request.Parameter
		'	Dim objAccesso As New COL_DataAccess

		'	Dim oDataCancellazione As DateTime = Now
		'	With oRequest
		'		.Command = "sp_BachecaComunita_DeleteVirtual"
		'		.CommandType = CommandType.StoredProcedure
		'		oParam = objAccesso.GetAdvancedParameter("@BCHC_Id", Me._ID, ParameterDirection.Input, SqlDbType.BigInt)
		'		.Parameters.Add(oParam)
		'		oParam = objAccesso.GetAdvancedParameter("@PersonaID", oPersona.Id, ParameterDirection.Input, SqlDbType.Int)
		'		.Parameters.Add(oParam)
		'		oParam = objAccesso.GetAdvancedParameter("@Delete", 1, ParameterDirection.Input, SqlDbType.Int)
		'		.Parameters.Add(oParam)
		'		oParam = objAccesso.GetAdvancedParameter("@BCHC_CancellataIl", Main.DateToString(oDataCancellazione), ParameterDirection.Input, SqlDbType.VarChar, , 50)
		'		.Parameters.Add(oParam)

		'		.Role = COL_Request.UserRole.Admin
		'		.transactional = False
		'	End With
		'	Try
		'		Me._isDeleted = True
		'		objAccesso.GetExecuteNotQuery(oRequest)
		'		Me._ModificatoDa = oPersona
		'		Me._errore = Errori_Db.None
		'	Catch ex As Exception
		'		Me._errore = Errori_Db.DBDelete
		'	End Try
		'End Sub
		'Public Sub UnDeleteVirtual(ByVal oPersona As COL_Persona)
		'	Dim oRequest As New COL_Request
		'	Dim oParam As New COL_Request.Parameter
		'	Dim objAccesso As New COL_DataAccess
		'	With oRequest
		'		.Command = "sp_BachecaComunita_DeleteVirtual"
		'		.CommandType = CommandType.StoredProcedure
		'		oParam = objAccesso.GetAdvancedParameter("@BCHC_Id", Me._ID, ParameterDirection.Input, SqlDbType.BigInt)
		'		.Parameters.Add(oParam)
		'		oParam = objAccesso.GetAdvancedParameter("@PersonaID", oPersona.Id, ParameterDirection.Input, SqlDbType.Int)
		'		.Parameters.Add(oParam)
		'		oParam = objAccesso.GetAdvancedParameter("@Delete", 0, ParameterDirection.Input, SqlDbType.Int)
		'		.Parameters.Add(oParam)

		'		.Role = COL_Request.UserRole.Admin
		'		.transactional = False
		'	End With
		'	Try
		'		Me._isDeleted = False
		'		objAccesso.GetExecuteNotQuery(oRequest)
		'		Me._ModificatoDa = oPersona
		'		Me._errore = Errori_Db.None
		'	Catch ex As Exception
		'		Me._errore = Errori_Db.DBChange
		'	End Try
		'End Sub

		'Private Function SpeedToInteger(ByVal VelocitaScorrimento As Main.VelocitaScorrimento) As Integer
		'	Select Case _VelocitaScorrimento
		'		Case Main.VelocitaScorrimento.Fermo
		'			Return 0
		'		Case Main.VelocitaScorrimento.Lento
		'			Return 1
		'		Case Main.VelocitaScorrimento.MoltoVeloce
		'			Return 3
		'		Case Main.VelocitaScorrimento.Veloce
		'			Return 2
		'		Case Else
		'			Return 1
		'	End Select
		'End Function

		'Public Shared Function List(ByVal LinguaID As Integer, ByVal ComunitaID As Integer, ByVal isVisibile As Main.FiltroVisibilità, Optional ByVal sortExpression As String = "", Optional ByVal sortDirection As String = "") As List(Of COL_BachecaComunita)
		'	Dim oListaBacheca As New List(Of COL_BachecaComunita)
		'	Dim cacheKey As String = CachePolicy.Bacheca(ComunitaID)

		'	If sortDirection <> String.Empty Then
		'		sortDirection = sortDirection.ToLower
		'	End If

		'	If ObjectBase.Cache(cacheKey) Is Nothing Then
		'		oListaBacheca = COL_BachecaComunita.RetrieveListFromDB(ComunitaID, LinguaID)
		'		ObjectBase.Cache.Insert(cacheKey, oListaBacheca, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza60minuti)
		'	Else
		'		oListaBacheca = CType(ObjectBase.Cache(cacheKey), List(Of COL_BachecaComunita))
		'	End If

		'	Dim iLista As New List(Of COL_BachecaComunita)
		'	If isVisibile = FiltroVisibilità.Tutti Then
		'		iLista = oListaBacheca
		'	Else
		'		Dim oQuery = From oBacheca As COL_BachecaComunita In oListaBacheca Where FindByVisibilita(oBacheca, isVisibile)
		'		'iLista = oListaBacheca.FindAll(New GenericPredicate(Of COL_BachecaComunita, Main.FiltroVisibilità)(isVisibile, AddressOf FindByVisibilita))

		'		iLista = oQuery.ToList
		'	End If

		'	If (Not sortExpression Is Nothing AndAlso sortDirection <> String.Empty) Then
		'		iLista.Sort(New GenericComparer(Of COL_BachecaComunita)(sortExpression))
		'	End If

		'	If (Not sortDirection Is Nothing AndAlso sortDirection = "desc") Then
		'		iLista.Reverse()
		'	End If
		'	Return iLista
		'End Function
	End Class
End Namespace