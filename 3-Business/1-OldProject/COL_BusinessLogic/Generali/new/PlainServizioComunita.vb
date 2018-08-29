Imports COL_DataLayer

Public Class PlainServizioComunita
	Inherits ObjectBase

#Region "Private Property"
	Private _ID As Integer
	Private _Nome As String
	Private _Permessi As String
	Private _isAbilitato As Boolean
	Private _Codice As String
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
	Public Property Permessi() As String
		Get
			Permessi = _Permessi
		End Get
		Set(ByVal Value As String)
			_Permessi = Value
		End Set
	End Property
	Public Property isAbilitato() As Boolean
		Get
			isAbilitato = _isAbilitato
		End Get
		Set(ByVal Value As Boolean)
			_isAbilitato = Value
		End Set
	End Property
	Public Property Codice() As String
		Get
			Codice = _Codice
		End Get
		Set(ByVal Value As String)
			_Codice = Value
		End Set
	End Property
#End Region

#Region "Metodi New"
	Sub New()
		Me._isAbilitato = True
	End Sub
	Public Sub New(ByVal ServizioID As Integer, ByVal ServizioNome As String, ByVal Permessi As String, ByVal Codice As String)
		_ID = ServizioID
		_Nome = ServizioNome
		_Permessi = Permessi
		_isAbilitato = True
		_Codice = Codice
	End Sub


#End Region


	Private Shared Function RetriveFromDB(ByVal RuoloID As Integer, ByVal ComunitaID As Integer) As GenericCollection(Of PlainServizioComunita)
		Dim oListaServizi As New GenericCollection(Of PlainServizioComunita)
		Dim oRequest As New COL_Request
		Dim oParam As New COL_Request.Parameter
		Dim idataReader As IDataReader
		Dim objAccesso As New COL_DataAccess
		With oRequest
			.Command = "sp_Servizio_ElencaWithPermessoByTipoRuoloByComunita"
			.CommandType = CommandType.StoredProcedure
			oParam = objAccesso.GetAdvancedParameter("@TPRL_Id", RuoloID, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", ComunitaID, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)
			.Role = COL_Request.UserRole.Admin
			.transactional = False
		End With
		Try
			idataReader = objAccesso.GetdataReader(oRequest)
			While idataReader.Read
				oListaServizi.Add(New PlainServizioComunita(idataReader("SRVZ_id"), idataReader("SRVZ_nome"), idataReader("LKSC_Permessi"), idataReader("SRVZ_Codice")))

			End While
		Catch ex As Exception

		End Try
		Return oListaServizi
	End Function

	Public Shared Function ElencaByComunita(ByVal RuoloID As Integer, ByVal ComunitaID As Integer, Optional ByVal sortExpression As String = "", Optional ByVal sortDirection As String = "") As GenericCollection(Of PlainServizioComunita)
		Dim oListaServizi As GenericCollection(Of PlainServizioComunita)
		Dim cacheKey As String = CachePolicy.ServiziComunita(ComunitaID, RuoloID)

		If sortDirection <> String.Empty Then
			sortDirection = sortDirection.ToLower
		End If

		If ObjectBase.Cache(cacheKey) Is Nothing Then
			oListaServizi = PlainServizioComunita.RetriveFromDB(RuoloID, ComunitaID)
			ObjectBase.Cache.Insert(cacheKey, oListaServizi, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.ScadenzaGiornaliera)
		Else
			oListaServizi = CType(ObjectBase.Cache(cacheKey), GenericCollection(Of PlainServizioComunita))
		End If

		If (Not sortExpression Is Nothing AndAlso sortDirection <> String.Empty) Then
			oListaServizi.Sort(New GenericComparer(Of COL_Servizio)(sortExpression))
		End If

		If (Not sortDirection Is Nothing AndAlso sortDirection = "desc") Then
			oListaServizi.Reverse()
		End If

		Return oListaServizi
	End Function
End Class
