Imports COL_DataLayer

Public Class COL_TipoMateriale
	Inherits ObjectBase

	Private n_TPMT_id As Integer
	Private n_TPMT_descrizione As String
	Private n_errore As Errori_Db

#Region "Properties"
	Public Property Id() As Integer
		Get
			Id = n_TPMT_id
		End Get
		Set(ByVal Value As Integer)
			n_TPMT_id = Value
		End Set
	End Property
	Public Property Descrizione() As String
		Get
			Descrizione = n_TPMT_descrizione
		End Get
		Set(ByVal Value As String)
			n_TPMT_descrizione = Value
		End Set
	End Property
	Public ReadOnly Property ErroreDB() As String
		Get
			ErroreDB = n_errore
		End Get
	End Property
#End Region
#Region "Metodi New"
	Sub New()
		Me.n_errore = Errori_Db.None
	End Sub
	Sub New(ByVal iID As Integer, ByVal iDescrizione As String)
		Me.n_errore = Errori_Db.None
		Me.n_TPMT_id = iID
		Me.n_TPMT_descrizione = iDescrizione
	End Sub
#End Region
#Region "Metodi"
	Public Function Elenca(ByVal LinguaID As Integer) As DataSet  'mi restituisce un dataset con TUTTE le tipologie di materiale
		Dim oRequest As New COL_Request
		Dim oParam As New COL_Request.Parameter
		Dim dsTable As New DataSet
		Dim objAccesso As New COL_DataAccess

		With oRequest
			.Command = "sp_Tipo_MaterialeElenca"
			.CommandType = CommandType.StoredProcedure

			oParam = objAccesso.GetParameter("@LinguaID", LinguaID, ParameterDirection.Input, DbType.Int32)
			.Parameters.Add(oParam)

			.Role = COL_Request.UserRole.Admin
			.transactional = False
		End With
		Try
			dsTable = objAccesso.GetdataSet(oRequest)
		Catch ex As Exception
			Me.n_errore = Errori_Db.DBError
		End Try
		Return dsTable
	End Function
#End Region

#Region "Nuova Versione Metodi"
	Public Shared Function ElencaForVisualizza(ByVal LinguaID As Integer, Optional ByVal sortExpression As String = "", Optional ByVal sortDirection As String = "") As List(Of COL_TipoMateriale)
		Dim oLista As New List(Of COL_TipoMateriale)
		Dim cacheKey As String = CachePolicy.TipoMateriale(LinguaID)

		If ObjectBase.Cache(cacheKey) Is Nothing Then
			oLista = COL_TipoMateriale.RetrieveByLanguage(LinguaID)
			ObjectBase.Cache.Insert(cacheKey, oLista, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.ScadenzaGiornaliera)
		Else
			oLista = CType(ObjectBase.Cache(cacheKey), List(Of COL_TipoMateriale))
		End If


		If sortDirection <> String.Empty Then
			sortDirection = sortDirection.ToLower
		End If


		If (Not sortDirection Is Nothing AndAlso sortDirection = "desc") Then
			oLista.Reverse()
		End If

		Return oLista
	End Function
	Public Shared Function HashingList(ByVal LinguaID As Integer, Optional ByVal isForAdmin As Boolean = False) As HASH_TipoMateriale
		Dim oHashList As New HASH_TipoMateriale
		Dim cacheKey As String = CachePolicy.HashTipoMateriale(LinguaID)

		If ObjectBase.Cache(cacheKey) Is Nothing Then
			Dim oLista As New List(Of COL_TipoMateriale)
			oLista = COL_TipoMateriale.RetrieveByLanguage(LinguaID)
			For Each oTipoMateriale As COL_TipoMateriale In oLista
				oHashList.Add(oTipoMateriale.Id, oTipoMateriale.Descrizione)
			Next
			ObjectBase.Cache.Insert(cacheKey, oHashList, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza60minuti)
		Else
			oHashList = CType(ObjectBase.Cache(cacheKey), HASH_TipoMateriale)
		End If

		Return oHashList
	End Function
#End Region

#Region "Recupero Dati DB"

	Private Shared Function RetrieveByLanguage(ByVal LinguaID As Integer) As List(Of COL_TipoMateriale)
		Dim oLista As New List(Of COL_TipoMateriale)
		Dim oRequest As New COL_Request
		Dim oParam As New COL_Request.Parameter
		Dim objAccesso As New COL_DataAccess
		Dim oDatareader As IDataReader

		With oRequest
			.Command = "sp_Tipo_MaterialeElenca"
			.CommandType = CommandType.StoredProcedure

			oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)

			.Role = COL_Request.UserRole.Admin
			.transactional = False
		End With

		Try
			oDatareader = objAccesso.GetdataReader(oRequest)
			While oDatareader.Read
				oLista.Add(New COL_TipoMateriale(oDatareader("TPMT_id"), oDatareader("TPMT_descrizione")))
			End While
		Catch ex As Exception
		Finally
			If oDatareader.IsClosed = False Then
				oDatareader.Close()
			End If
		End Try
		Return oLista
	End Function
#End Region
End Class