Imports COL_DataLayer

Public Class COL_CategoriaFile
	Inherits ObjectBase

#Region "Private Property"
	Private _ID As Integer
	Private _Nome As String
	Private _NoDelete As Integer
	Private _FileCount As Long
	Private _Errore As Errori_Db
#End Region

#Region "Public Property"
	Public Property ID() As Integer
		Get
			ID = _ID
		End Get
		Set(ByVal Value As Integer)
			_ID = Value
		End Set
	End Property
	Public Property Nome() As String
		Get
			Nome = _Nome
		End Get
		Set(ByVal Value As String)
			_Nome = Value
		End Set
	End Property
	Public Property noDelete() As Boolean
		Get
			noDelete = _NoDelete
		End Get
		Set(ByVal Value As Boolean)
			_NoDelete = Value
		End Set
	End Property
	Public Property FileCount() As Long
		Get
			FileCount = _FileCount
		End Get
		Set(ByVal Value As Long)
			_FileCount = Value
		End Set
	End Property
	Public ReadOnly Property Errore() As Errori_Db
		Get
			Errore = _Errore
		End Get
	End Property
#End Region

#Region "Metodi New"
	Public Sub New()
		Me._Errore = Errori_Db.None
	End Sub
    Public Sub New(ByVal CategoriaID As Long, ByVal CategoriaNome As String, ByVal isNoDelete As Integer, ByVal Totale As Long)
        Me._Errore = Errori_Db.None
        Me._ID = CategoriaID
        Me._Nome = CategoriaNome
        Me._NoDelete = isNoDelete
        Me._FileCount = Totale
    End Sub
#End Region

#Region "Metodi Standard"
	Public Shared Function Elenca(ByVal LinguaID As Integer, Optional ByVal ForAdmin As Boolean = False) As DataSet
		Dim oRequest As New COL_Request
		Dim oParam As New COL_Request.Parameter
		Dim oDataSet As New DataSet
		Dim objAccesso As New COL_DataAccess

		With oRequest
			.Command = "sp_CategoriaFile_Elenca"
			.CommandType = CommandType.StoredProcedure

			oParam = objAccesso.GetAdvancedParameter("@CMNT_id", -1, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)

			oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)

			If ForAdmin Then
				oParam = objAccesso.GetAdvancedParameter("@forAdmin", 1, ParameterDirection.Input, SqlDbType.Int)
			Else
				oParam = objAccesso.GetAdvancedParameter("@forAdmin", 0, ParameterDirection.Input, SqlDbType.Int)
			End If
			.Parameters.Add(oParam)

			.Role = COL_Request.UserRole.Admin
			.transactional = False
		End With
		Try
			oDataSet = objAccesso.GetdataSet(oRequest)
		Catch ex As Exception

		End Try
		Return oDataSet
	End Function
	Public Sub Estrai()
		Dim oRequest As New COL_Request
		Dim oParam As New COL_Request.Parameter
		Dim objAccesso As New COL_DataAccess

		With oRequest
			.Command = "sp_CategoriaFile_Estrai"
			.CommandType = CommandType.StoredProcedure
			oParam = objAccesso.GetAdvancedParameter("@CTGR_id", Me._ID, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CTGR_nome", "", ParameterDirection.Output, SqlDbType.VarChar, , 200)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CTGR_noDelete", "", ParameterDirection.Output, SqlDbType.Int)
			.Parameters.Add(oParam)

			.Role = COL_Request.UserRole.Admin
			.transactional = False
		End With

		Try
			objAccesso.GetExecuteNotQuery(oRequest)
			Me._Nome = oRequest.GetValueFromParameter(2)
			Me._NoDelete = oRequest.GetValueFromParameter(3)
		Catch ex As Exception
			Me._Errore = Errori_Db.DBReadExist
		End Try

	End Sub
	Public Sub Estrai(ByVal LinguaID As Integer)
		Dim oRequest As New COL_Request
		Dim oParam As New COL_Request.Parameter
		Dim objAccesso As New COL_DataAccess

		With oRequest
			.Command = "sp_CategoriaFile_EstraiByLingua"
			.CommandType = CommandType.StoredProcedure
			oParam = objAccesso.GetAdvancedParameter("@CTGR_id", Me._ID, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CTGR_nome", "", ParameterDirection.Output, SqlDbType.VarChar, , 200)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CTGR_noDelete", "", ParameterDirection.Output, SqlDbType.Int)
			.Parameters.Add(oParam)

			oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)

			.Role = COL_Request.UserRole.Admin
			.transactional = False
		End With

		Try
			objAccesso.GetExecuteNotQuery(oRequest)
			Me._Nome = oRequest.GetValueFromParameter(2)
			Me._NoDelete = oRequest.GetValueFromParameter(3)
		Catch ex As Exception
			Me._Errore = Errori_Db.DBReadExist
		End Try

	End Sub
	Public Sub Aggiungi()
		Dim oRequest As New COL_Request
		Dim oParam As New COL_Request.Parameter
		Dim objAccesso As New COL_DataAccess
		With oRequest
			.Command = "sp_CategoriaFile_Aggiungi"
			.CommandType = CommandType.StoredProcedure
			oParam = objAccesso.GetAdvancedParameter("@CTGR_Id", "", ParameterDirection.Output, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CTGR_nome", Me._Nome, ParameterDirection.Input, SqlDbType.VarChar, , 200)
			.Parameters.Add(oParam)

			.Role = COL_Request.UserRole.Admin
			.transactional = False
		End With
		Try
			'eseguo l'inserimento e Recupero l'ID univoco della categoria
			objAccesso.GetExecuteNotQuery(oRequest)
			Me._Errore = Errori_Db.None
			Me._ID = oRequest.GetValueFromParameter(1)
			ObjectBase.PurgeCacheItems(CachePolicy.CategoriaMateriale)
		Catch ex As Exception
			Me._ID = -1
			Me._Errore = Errori_Db.DBInsert
		End Try
	End Sub
	Public Sub Modifica()
		Dim oRequest As New COL_Request
		Dim oParam As New COL_Request.Parameter
		Dim objAccesso As New COL_DataAccess
		With oRequest
			.Command = "sp_CategoriaFile_Modifica"
			.CommandType = CommandType.StoredProcedure
			oParam = objAccesso.GetAdvancedParameter("@CTGR_Id", Me._ID, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CTGR_nome", Me._Nome, ParameterDirection.Input, SqlDbType.VarChar, , 200)
			.Parameters.Add(oParam)

			.Role = COL_Request.UserRole.Admin
			.transactional = False
		End With
		Try
			objAccesso.GetExecuteNotQuery(oRequest)
			ObjectBase.PurgeCacheItems(CachePolicy.CategoriaMateriale)
			Me._Errore = Errori_Db.None
		Catch ex As Exception
			Me._Errore = Errori_Db.DBChange
		End Try
	End Sub
	Public Sub Elimina()
		Dim oRequest As New COL_Request
		Dim oParam As New COL_Request.Parameter
		Dim objAccesso As New COL_DataAccess

		With oRequest
			.Command = "sp_CategoriaFile_Elimina"
			.CommandType = CommandType.StoredProcedure

			oParam = objAccesso.GetAdvancedParameter("@CTGR_Id", Me._ID, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)

			.Role = COL_Request.UserRole.Admin
			.transactional = False
		End With
		Try
			objAccesso.GetExecuteNotQuery(oRequest)
			ObjectBase.PurgeCacheItems(CachePolicy.CategoriaMateriale)
			Me._Errore = Errori_Db.None
		Catch ex As Exception
			Me._Errore = Errori_Db.DBDelete
		End Try
	End Sub

	Public Shared Function ElencaByComunita(ByVal IdComunita As Integer, ByVal LinguaID As Integer) As DataSet	'mi restituisce un dataset con le categorie di materiale disponibili per la comunità passata in input
		Dim oRequest As New COL_Request
		Dim oParam As New COL_Request.Parameter
		Dim oDataSet As New DataSet
		Dim objAccesso As New COL_DataAccess

		With oRequest
			.Command = "sp_CategoriaFile_ElencaByComunita"
			.CommandType = CommandType.StoredProcedure

			oParam = objAccesso.GetParameter("@CMNT_id", IdComunita, ParameterDirection.Input, DbType.Int32)
			.Parameters.Add(oParam)

			oParam = objAccesso.GetParameter("@LinguaID", LinguaID, ParameterDirection.Input, DbType.Int32)
			.Parameters.Add(oParam)

			.Role = COL_Request.UserRole.Admin
			.transactional = False
		End With
		Try
			oDataSet = objAccesso.GetdataSet(oRequest)
		Catch ex As Exception

		End Try
		Return oDataSet
	End Function
	Public Shared Function ElencaByComunitaForLastInsert(ByVal IdComunita As Integer, ByVal PRSN_ID As Integer, ByVal LinguaID As Integer) As DataSet
		Dim oRequest As New COL_Request
		Dim oParam As New COL_Request.Parameter
		Dim oDataSet As New DataSet
		Dim objAccesso As New COL_DataAccess

		With oRequest
			.Command = "sp_CategoriaFile_ElencaByComunitaForLastInsert"
			.CommandType = CommandType.StoredProcedure

			oParam = objAccesso.GetParameter("@CMNT_id", IdComunita, ParameterDirection.Input, DbType.Int32)
			.Parameters.Add(oParam)

			oParam = objAccesso.GetParameter("@PRSN_ID", PRSN_ID, ParameterDirection.Input, DbType.Int32)
			.Parameters.Add(oParam)

			oParam = objAccesso.GetParameter("@LinguaID", LinguaID, ParameterDirection.Input, DbType.Int32)
			.Parameters.Add(oParam)

			.Role = COL_Request.UserRole.Admin
			.transactional = False
		End With
		Try
			oDataSet = objAccesso.GetdataSet(oRequest)
		Catch ex As Exception

		End Try
		Return oDataSet
	End Function
	Public Shared Function ElencaByTipoComunita(ByVal TPCM_ID As Integer, ByVal LinguaId As Integer) As DataSet	'mi restituisce un dataset con le categorie di materiale disponibili x tipocomunita
		Dim oRequest As New COL_Request
		Dim oParam As New COL_Request.Parameter
		Dim oDataSet As New DataSet
		Dim objAccesso As New COL_DataAccess

		With oRequest
			.Command = "sp_CategoriaFile_ElencaByTipoComunita"
			.CommandType = CommandType.StoredProcedure

			oParam = objAccesso.GetParameter("@TPCM_id", TPCM_ID, ParameterDirection.Input, DbType.Int32)
			.Parameters.Add(oParam)

			oParam = objAccesso.GetParameter("@LinguaId", LinguaId, ParameterDirection.Input, DbType.Int32)
			.Parameters.Add(oParam)

			.Role = COL_Request.UserRole.Admin
			.transactional = False
		End With
		Try
			oDataSet = objAccesso.GetdataSet(oRequest)
		Catch ex As Exception

		End Try
		Return oDataSet
	End Function
#End Region

#Region "Metodi Gestione"
	'Public Shared Function _GetTipoComunitaAssociate(ByVal CTGR_id) As DataSet
	'    Dim oRequest As New COL_Request
	'    Dim oParam As New COL_Request.Parameter
	'    Dim oDataSet As New DataSet
	'    Dim objAccesso As New COL_DataAccess

	'    With oRequest
	'        .Command = "sp_CatFile_GetTipoComunitaAssociate"
	'        .CommandType = CommandType.StoredProcedure
	'        oParam = objAccesso.GetParameter("@CTGR_ID", CTGR_id, ParameterDirection.Input, DbType.Int32)
	'        .Parameters.Add(oParam)

	'        .Role = COL_Request.UserRole.Admin
	'        .transactional = False
	'    End With
	'    Try
	'        oDataSet = objAccesso.GetdataSet(oRequest)

	'    Catch ex As Exception
	'        oDataSet = New DataSet
	'    End Try
	'    Return oDataSet
	'End Function
	'Public Shared Function _GetTipoComunitaAssociateNonAssociate(ByVal CTGR_id) As DataSet
	'    Dim oRequest As New COL_Request
	'    Dim oParam As New COL_Request.Parameter
	'    Dim oDataSet As New DataSet
	'    Dim objAccesso As New COL_DataAccess

	'    With oRequest
	'        .Command = "sp_CatFile_GetTipoComunitaAssociateNonAssociate"
	'        .CommandType = CommandType.StoredProcedure
	'        oParam = objAccesso.GetParameter("@CTGR_ID", CTGR_id, ParameterDirection.Input, DbType.Int32)
	'        .Parameters.Add(oParam)

	'        .Role = COL_Request.UserRole.Admin
	'        .transactional = False
	'    End With
	'    Try
	'        oDataSet = objAccesso.GetdataSet(oRequest)
	'    Catch ex As Exception
	'        oDataSet = New DataSet
	'    End Try
	'    Return oDataSet
	'End Function
#End Region

	Public Sub Translate(ByVal Termine As String, ByVal LinguaID As Integer)
		Dim oRequest As New COL_Request
		Dim oParam As New COL_Request.Parameter
		Dim objAccesso As New COL_DataAccess

		With oRequest
			.Command = "sp_CategoriaFile_Translate"
			.CommandType = CommandType.StoredProcedure
			oParam = objAccesso.GetAdvancedParameter("@CTGR_id", Me._ID, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@Termine", Termine, ParameterDirection.Input, SqlDbType.VarChar, , 50)
			.Parameters.Add(oParam)
			.Role = COL_Request.UserRole.Admin
			.transactional = False
		End With
		Try
			objAccesso.GetExecuteNotQuery(oRequest)
			ObjectBase.PurgeCacheItems(CachePolicy.CategoriaMateriale(LinguaID))
			Me._Errore = Errori_Db.None
		Catch ex As Exception
			Me._Errore = Errori_Db.DBChange
		End Try
	End Sub
	Public Function ElencaDefinizioniLingue() As DataSet
		Dim oRequest As New COL_Request
		Dim oParam As New COL_Request.Parameter
		Dim objAccesso As New COL_DataAccess
		Dim oDataSet As New DataSet

		With oRequest
			.Command = "sp_CategoriaFile_ElencaDefinizioniLingue"
			.CommandType = CommandType.StoredProcedure
			oParam = objAccesso.GetAdvancedParameter("@CTGR_id", Me._ID, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)
			.Role = COL_Request.UserRole.Admin
			.transactional = False
		End With
		Try
			oDataSet = objAccesso.GetdataSet(oRequest)
			Me._Errore = Errori_Db.None
			Return oDataSet
		Catch ex As Exception
			Me._Errore = Errori_Db.DBReadExist
		End Try
		Return oDataSet
	End Function
	Public Sub AssociaTipiComunita(ByVal ListaTipiComunita As String)
		Dim oRequest As New COL_Request
		Dim oParam As New COL_Request.Parameter
		Dim objAccesso As New COL_DataAccess

		With oRequest
			.Command = "sp_CategoriaFile_AssociaTipiComunita"
			.CommandType = CommandType.StoredProcedure

			oParam = objAccesso.GetAdvancedParameter("@CTGR_id", Me._ID, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)

			oParam = objAccesso.GetAdvancedParameter("@Elenco", ListaTipiComunita, ParameterDirection.Input, SqlDbType.VarChar, , 4000)
			.Parameters.Add(oParam)


			.Role = COL_Request.UserRole.Admin
			.transactional = False
		End With
		Try
			objAccesso.GetExecuteNotQuery(oRequest)
			Me._Errore = Errori_Db.None
		Catch ex As Exception
			Me._Errore = Errori_Db.DBChange
		End Try
	End Sub
	Public Shared Function ElencaTipiComunita(ByVal CTGR_id As Integer, ByVal LinguaID As Integer) As DataSet
		Dim oRequest As New COL_Request
		Dim oParam As New COL_Request.Parameter
		Dim oDataSet As New DataSet
		Dim objAccesso As New COL_DataAccess

		With oRequest
			.Command = "sp_CategoriaFile_ElencaTipiComunita"
			.CommandType = CommandType.StoredProcedure
			oParam = objAccesso.GetAdvancedParameter("@CTGR_ID", CTGR_id, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)

			.Role = COL_Request.UserRole.Admin
			.transactional = False
		End With
		Try
			oDataSet = objAccesso.GetdataSet(oRequest)

		Catch ex As Exception
			oDataSet = New DataSet
		End Try
		Return oDataSet
	End Function

#Region "Nuova Versione Metodi"
	Public Shared Function ElencaForVisualizza(ByVal LinguaID As Integer, Optional ByVal isForAdmin As Boolean = False, Optional ByVal sortExpression As String = "", Optional ByVal sortDirection As String = "") As List(Of COL_CategoriaFile)
		Dim oLista As New List(Of COL_CategoriaFile)
		Dim cacheKey As String = CachePolicy.CategoriaMateriale(LinguaID)

		If ObjectBase.Cache(cacheKey) Is Nothing Then
			oLista = COL_CategoriaFile.RetrieveCategoriesByLanguage(LinguaID, isForAdmin)
			ObjectBase.Cache.Insert(cacheKey, oLista, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza60minuti)
		Else
			oLista = CType(ObjectBase.Cache(cacheKey), List(Of COL_CategoriaFile))
		End If


		If sortDirection <> String.Empty Then
			sortDirection = sortDirection.ToLower
		End If


		If (Not sortDirection Is Nothing AndAlso sortDirection = "desc") Then
			oLista.Reverse()
		End If

		Return oLista
	End Function
	Public Shared Function HashingList(ByVal LinguaID As Integer, Optional ByVal isForAdmin As Boolean = False) As HASH_CategoriaFile
		Dim oHashList As New HASH_CategoriaFile
		Dim cacheKey As String = CachePolicy.HashCategoriaMateriale(LinguaID)

		If ObjectBase.Cache(cacheKey) Is Nothing Then
			Dim oLista As New List(Of COL_CategoriaFile)
			oLista = COL_CategoriaFile.RetrieveCategoriesByLanguage(LinguaID, isForAdmin)
			For Each oCategoria As COL_CategoriaFile In oLista
				oHashList.Add(oCategoria.ID, oCategoria.Nome)
			Next
			ObjectBase.Cache.Insert(cacheKey, oHashList, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza60minuti)
		Else
			oHashList = CType(ObjectBase.Cache(cacheKey), HASH_CategoriaFile)
		End If

		Return oHashList
	End Function
#End Region

#Region "Recupero Dati DB"
	
	Private Shared Function RetrieveCategoriesByLanguage(ByVal LinguaID As Integer, ByVal isForAdmin As Boolean) As List(Of COL_CategoriaFile)
		Dim oLista As New List(Of COL_CategoriaFile)
		Dim oRequest As New COL_Request
		Dim oParam As New COL_Request.Parameter
		Dim objAccesso As New COL_DataAccess
		Dim oDatareader As IDataReader

		With oRequest
			.Command = "sp_CategoriaFile_Elenca"
			.CommandType = CommandType.StoredProcedure

			oParam = objAccesso.GetAdvancedParameter("@CMNT_id", -1, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)

			oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)

			If isForAdmin Then
				oParam = objAccesso.GetAdvancedParameter("@forAdmin", 1, ParameterDirection.Input, SqlDbType.Int)
			Else
				oParam = objAccesso.GetAdvancedParameter("@forAdmin", 0, ParameterDirection.Input, SqlDbType.Int)
			End If
			.Parameters.Add(oParam)

			.Role = COL_Request.UserRole.Admin
			.transactional = False
		End With

		Try
			oDatareader = objAccesso.GetdataReader(oRequest)
			While oDatareader.Read
                oLista.Add(New COL_CategoriaFile(oDatareader("CTGR_id"), oDatareader("CTGR_nome"), True, oDatareader("Totale")))
			End While
		Catch ex As Exception
		Finally
			If oDatareader.IsClosed = False Then
				oDatareader.Close()
			End If
		End Try
		Return oLista
    End Function

    Public Shared Function RetrieveCategoriesByLanguage(ByVal CommunityId As Integer, ByVal LanguageID As Integer) As List(Of COL_CategoriaFile)
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim iResponse As New List(Of COL_CategoriaFile)
        Dim oDatareader As IDataReader = Nothing
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_CategoriaFile_ElencaByComunita"
            .CommandType = CommandType.StoredProcedure

            oParam = objAccesso.GetParameter("@CMNT_id", CommunityId, ParameterDirection.Input, DbType.Int32)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetParameter("@LinguaID", LanguageID, ParameterDirection.Input, DbType.Int32)
            .Parameters.Add(oParam)

            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            oDatareader = objAccesso.GetdataReader(oRequest)
            While oDatareader.Read
                iResponse.Add(New COL_CategoriaFile(oDatareader("CTGR_id"), oDatareader("CTGR_nome"), True, 0))
            End While
        Catch ex As Exception
        Finally
            If Not IsNothing(oDatareader) AndAlso oDatareader.IsClosed = False Then
                oDatareader.Close()
            End If
        End Try
        Return iResponse
    End Function
#End Region
End Class