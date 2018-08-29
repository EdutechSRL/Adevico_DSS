Imports COL_DataLayer
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.CL_permessi

Public Class IscrizioneComunita
	Inherits ObjectBase

#Region "Private Property"
	Private _Comunita As New COL_Comunita
	Private _Iscritto As New COL_Persona
	Private _Ruolo As New COL_TipoRuolo
	Private _attivato As Boolean
	Private _abilitato As Boolean
	Private _responsabile As Boolean
	Private _UltimoCollegamento As DateTime
	Private _PenultimoCollegamento As DateTime
	Private _IscrittoIl As DateTime
	Private _isConnesso As Boolean
	Private _skipCover As Boolean
	Private _Errore As Errori_Db
#End Region

#Region "Public Property"
	Public ReadOnly Property OrganizzazioneID()
		Get
			Try
				Return Me.Comunita.Organizzazione.Id
			Catch ex As Exception
				Return 0
			End Try
		End Get
	End Property
	Public ReadOnly Property OrganizzazioneNome()
		Get
			Try
				Return Me.Comunita.Organizzazione.RagioneSociale
			Catch ex As Exception
				Return 0
			End Try
		End Get
	End Property
	Public Property Comunita() As COL_Comunita
		Get
			Comunita = _Comunita
		End Get
		Set(ByVal Value As COL_Comunita)
			_Comunita = Value
		End Set
	End Property
	Public Property Persona() As COL_Persona
		Get
			Persona = _Iscritto
		End Get
		Set(ByVal Value As COL_Persona)
			_Iscritto = Value
		End Set
	End Property
	Public Property TipoRuolo() As COL_TipoRuolo
		Get
			TipoRuolo = _Ruolo
		End Get
		Set(ByVal Value As COL_TipoRuolo)
			_Ruolo = Value
		End Set
	End Property
	Public Property Attivato() As Boolean
		Get
			Attivato = _attivato
		End Get
		Set(ByVal Value As Boolean)
			_attivato = Value
		End Set
	End Property
	Public Property Abilitato() As Boolean
		Get
			Abilitato = _abilitato
		End Get
		Set(ByVal Value As Boolean)
			_abilitato = Value
		End Set
	End Property
	Public Property isResponsabile() As Boolean
		Get
			isResponsabile = _responsabile
		End Get
		Set(ByVal Value As Boolean)
			_responsabile = Value
		End Set
	End Property
	Public Property UltimoCollegamento() As DateTime
		Get
			UltimoCollegamento = _UltimoCollegamento
		End Get
		Set(ByVal Value As DateTime)
			_UltimoCollegamento = Value
		End Set
	End Property
	Public Property PenultimoCollegamento() As DateTime
		Get
			PenultimoCollegamento = _PenultimoCollegamento
		End Get
		Set(ByVal Value As DateTime)
			_PenultimoCollegamento = Value
		End Set
	End Property
	Public Property IscrittoIl() As DateTime
		Get
			IscrittoIl = _IscrittoIl
		End Get
		Set(ByVal Value As DateTime)
			_IscrittoIl = Value
		End Set
	End Property
	Public Property isConnesso() As Boolean
		Get
			isConnesso = _isConnesso
		End Get
		Set(ByVal Value As Boolean)
			_isConnesso = Value
		End Set
	End Property
	Public Property SaltaCopertina() As Boolean
		Get
			SaltaCopertina = CBool(_skipCover = 1)
		End Get
		Set(ByVal Value As Boolean)
			_skipCover = Value
		End Set
	End Property
	Public ReadOnly Property Errore() As Errori_Db
		Get
			Errore = _Errore
		End Get
	End Property
#End Region

#Region "Metodi New"
	Sub New()
		Me._isConnesso = 0
		Me._Errore = Errori_Db.None
	End Sub
	Sub New(ByVal Iscritto As COL_Persona, ByVal isResponsabile As Boolean, ByVal isAttivato As Boolean, ByVal isAbilitato As Boolean, _
	  ByVal oComunita As COL_Comunita, ByVal oRuolo As COL_TipoRuolo, ByVal iscrittoIl As DateTime, ByVal UltimoCollegamento As DateTime, ByVal PenultimoCollegamento As DateTime)
		Me._Errore = Errori_Db.None
		Me._Iscritto = Iscritto
		Me._responsabile = isResponsabile
		Me._abilitato = isAbilitato
		Me._attivato = isAttivato
		Me._Comunita = oComunita
		Me._IscrittoIl = iscrittoIl
		Me._UltimoCollegamento = UltimoCollegamento
		Me._PenultimoCollegamento = PenultimoCollegamento
		Me._Ruolo = oRuolo

		Me._isConnesso = False
		Me._skipCover = False
	End Sub

	Sub New(ByVal Iscritto As COL_Persona, ByVal isAttivato As Boolean, ByVal isAbilitato As Boolean, _
	  ByVal oComunita As COL_Comunita, ByVal RuoloID As Integer)
		Me._Errore = Errori_Db.None
		Me._Iscritto = Iscritto
		Me._responsabile = False
		Me._abilitato = isAbilitato
		Me._attivato = isAttivato
		Me._Comunita = oComunita
		Me._Ruolo = New COL_TipoRuolo(RuoloID)
		Me._isConnesso = False
		Me._skipCover = False
	End Sub
#End Region



	Private Shared Function PlainIscrizioni(ByVal CurrentUser As COL_Persona, ByVal LinguaID As Integer) As GenericCollection(Of IscrizioneComunita)
		Dim oRequest As New COL_Request
		Dim oParam As New COL_Request.Parameter
		Dim objAccesso As New COL_DataAccess
        Dim iDataReader As IDataReader = Nothing
		Dim oIscrizioni As New GenericCollection(Of IscrizioneComunita)
		With oRequest
			.Command = "sp_Iscrizioni_PlainIscrizioni"
			.CommandType = CommandType.StoredProcedure

			oParam = objAccesso.GetAdvancedParameter("@CurrentUserID", CurrentUser.Id, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)

			oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)

			.Role = COL_Request.UserRole.Admin
			.transactional = False
		End With

		Try
			iDataReader = objAccesso.GetdataReader(oRequest)

			While iDataReader.Read
				Try
					Dim oOrganizzazione As New COL_Organizzazione(iDataReader("ORGN_id"), iDataReader("ORGN_ragioneSociale"))
					Dim oTipoRuolo As New COL_TipoRuolo(iDataReader("TPRL_ID"), iDataReader("TPRL_nome"))
					Dim oTipoComunita As New COL_Tipo_Comunita(iDataReader("TPCM_id"), iDataReader("TPCM_descrizione"))


					oIscrizioni.Add(New IscrizioneComunita(CurrentUser, GenericValidator.ValBool(iDataReader("RLPC_responsabile"), False), _
					GenericValidator.ValBool(iDataReader("RLPC_attivato"), False), GenericValidator.ValBool(iDataReader("RLPC_abilitato"), False), _
					New COL_Comunita(iDataReader("CMNT_id"), GenericValidator.ValString(iDataReader("CMNT_nome"), ""), oOrganizzazione, oTipoComunita), _
					  oTipoRuolo, GenericValidator.ValData(iDataReader("RLPC_IscrittoIl"), Nothing), GenericValidator.ValData(iDataReader("RLPC_UltimoCollegamento"), Nothing), GenericValidator.ValData(iDataReader("RLPC_PenultimoCollegamento"), Nothing)))

				Catch ex As Exception

				End Try
			End While
		Catch ex As Exception
        Finally
            If Not IsNothing(iDataReader) Then
                If Not iDataReader.IsClosed Then
                    iDataReader.Close()
                End If
            End If
        End Try
		Return oIscrizioni
	End Function

	Public Shared Function TrovaIscrizioni(ByVal CurrentUser As COL_Persona, ByVal LinguaID As Integer, ByVal sortExpression As String, ByVal sortDirection As String) As GenericCollection(Of IscrizioneComunita)
		Dim oIscrizioni As GenericCollection(Of IscrizioneComunita)
		Dim cacheKey As String = CachePolicy.PlainIscrizioni(CurrentUser.Id, LinguaID)

		If sortDirection <> String.Empty Then
			sortDirection = sortDirection.ToLower
		End If

		If ObjectBase.Cache(cacheKey) Is Nothing Then
			oIscrizioni = IscrizioneComunita.PlainIscrizioni(CurrentUser, LinguaID)
			ObjectBase.Cache.Insert(cacheKey, oIscrizioni, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza30minuti)
		Else
			oIscrizioni = CType(ObjectBase.Cache(cacheKey), GenericCollection(Of IscrizioneComunita))
		End If

		If (Not sortExpression Is Nothing AndAlso sortDirection <> String.Empty) Then
			oIscrizioni.Sort(New GenericComparer(Of IscrizioneComunita)(sortExpression))
		End If

		If (Not sortDirection Is Nothing AndAlso sortDirection = "desc") Then
			oIscrizioni.Reverse()
		End If

		Return oIscrizioni
	End Function

	Public Shared Function FindIscrizioneComunita(ByVal CurrentUser As COL_Persona, ByVal LinguaID As Integer, ByVal ComunitaId As Integer) As IscrizioneComunita
		Dim oIscrizioni As GenericCollection(Of IscrizioneComunita)
		Dim cacheKey As String = CachePolicy.PlainIscrizioni(CurrentUser.Id, LinguaID)

		If ObjectBase.Cache(cacheKey) Is Nothing Then
			oIscrizioni = IscrizioneComunita.PlainIscrizioni(CurrentUser, LinguaID)
			oIscrizioni.Sort(New GenericComparer(Of IscrizioneComunita)("IscrittoIl"))
			ObjectBase.Cache.Insert(cacheKey, oIscrizioni, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza30minuti)
		Else
			oIscrizioni = CType(ObjectBase.Cache(cacheKey), GenericCollection(Of IscrizioneComunita))
		End If
		Return oIscrizioni.Find(New GenericPredicate(Of IscrizioneComunita, Integer)(ComunitaId, AddressOf FindByCommunityID))
	End Function

	Public Shared Function FindByCommunityID(ByVal item As IscrizioneComunita, ByVal argument As Integer) As Boolean
		Return item.Comunita.Id = argument
	End Function

	Public Shared Function FindByRuolo(ByVal item As IscrizioneComunita, ByVal RuoloID As Integer) As Boolean
		Return item.TipoRuolo.Id = RuoloID
	End Function
	Public Shared Function FindByAccesso(ByVal item As IscrizioneComunita, ByVal isPassante As Boolean) As Boolean
		If isPassante Then
			Return (item.TipoRuolo.Id = -2 Or item.TipoRuolo.Id = -3)
		Else
			Return (item.TipoRuolo.Id = -2 Or (item.Abilitato = True And item.Attivato = True And item.TipoRuolo.Id >= 0))
		End If

	End Function
End Class



'Namespace Comunita






'	Public Sub Estrai(ByVal CMNT_ID As Integer, ByVal PRSN_ID As Integer)
'		'in base alla comunità e all'id della persona mi ritorna il ruolo
'		Dim oRequest As New COL_Request
'		Dim oParam As New COL_Request.Parameter
'		Dim objAccesso As New COL_DataAccess

'		With oRequest
'			.Command = "sp_RuoloPersonaComunita_Estrai"
'			.CommandType = CommandType.StoredProcedure
'			oParam = objAccesso.GetParameter("@RLPC_PRSN_id", PRSN_ID, ParameterDirection.Input, DbType.Int32)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_CMNT_id", CMNT_ID, ParameterDirection.Input, DbType.Int32)
'			.Parameters.Add(oParam)

'			oParam = objAccesso.GetParameter("@RLPC_Id", "", ParameterDirection.Output, DbType.Int32)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_TPRL_id", "", ParameterDirection.Output, DbType.Int32)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_TPRL_Nome", "", ParameterDirection.Output, DbType.String, , 100)
'			.Parameters.Add(oParam)

'			oParam = objAccesso.GetParameter("@RLPC_attivato", "", ParameterDirection.Output, DbType.Byte)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_abilitato", "", ParameterDirection.Output, DbType.Byte)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_responsabile", "", ParameterDirection.Output, DbType.Byte)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_UltimoCollegamento", "", ParameterDirection.Output, DbType.DateTime)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_TPRL_Gerarchia", "", ParameterDirection.Output, DbType.Int32)
'			.Parameters.Add(oParam)

'			oParam = objAccesso.GetParameter("@RLPC_PenultimoCollegamento", "", ParameterDirection.Output, DbType.DateTime)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_IscrittoIl", "", ParameterDirection.Output, DbType.DateTime)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_isConnesso", "", ParameterDirection.Output, DbType.Int32)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_SkipCover", "", ParameterDirection.Output, DbType.Int32)
'			.Parameters.Add(oParam)

'			.Role = COL_Request.UserRole.Admin
'			.transactional = False
'		End With

'		Try
'			objAccesso.GetExecuteNotQuery(oRequest)
'			Me._PRSN.Id = PRSN_ID
'			Me._CMNT.Id = CMNT_ID
'			Me._id = oRequest.GetValueFromParameter(3)
'			Me.TipoRuolo.Id = oRequest.GetValueFromParameter(4)
'			Me.TipoRuolo.Nome = oRequest.GetValueFromParameter(5)
'			Me._attivato = oRequest.GetValueFromParameter(6)
'			Me._abilitato = oRequest.GetValueFromParameter(7)
'			Me._responsabile = oRequest.GetValueFromParameter(8)
'			If oRequest.GetValueFromParameter(9) <> "" Then
'				If IsDate(oRequest.GetValueFromParameter(9)) Then
'					Me._UltimoCollegamento = oRequest.GetValueFromParameter(9)
'				End If
'			End If
'			Me.TipoRuolo.Gerarchia = oRequest.GetValueFromParameter(10)
'			If oRequest.GetValueFromParameter(11) <> "" Then
'				If IsDate(oRequest.GetValueFromParameter(11)) Then
'					Me._PenultimoCollegamento = oRequest.GetValueFromParameter(11)
'				End If
'			End If
'			If oRequest.GetValueFromParameter(12) <> "" Then
'				If IsDate(oRequest.GetValueFromParameter(12)) Then
'					Me._IscrittoIl = oRequest.GetValueFromParameter(12)
'				End If
'			End If
'			Me._isConnesso = oRequest.GetValueFromParameter(13)
'			Try
'				Me._skipCover = oRequest.GetValueFromParameter(14)
'			Catch ex As Exception
'				Me._skipCover = 0
'			End Try

'			Me.n_Errore = Errori_Db.None
'		Catch ex As Exception 'record non trovato

'			Me.n_Errore = Errori_Db.DBReadExist
'		End Try
'	End Sub

'	Public Sub EstraiByLingua(ByVal CMNT_ID As Integer, ByVal PRSN_ID As Integer, ByVal LinguaID As Integer)
'		'in base alla comunità e all'id della persona mi ritorna il ruolo
'		Dim oRequest As New COL_Request
'		Dim oParam As New COL_Request.Parameter
'		Dim objAccesso As New COL_DataAccess

'		With oRequest
'			.Command = "sp_RuoloPersonaComunita_EstraiByLingua"
'			.CommandType = CommandType.StoredProcedure
'			oParam = objAccesso.GetParameter("@RLPC_PRSN_id", PRSN_ID, ParameterDirection.Input, DbType.Int32)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_CMNT_id", CMNT_ID, ParameterDirection.Input, DbType.Int32)
'			.Parameters.Add(oParam)

'			oParam = objAccesso.GetParameter("@RLPC_Id", "", ParameterDirection.Output, DbType.Int32)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_TPRL_id", "", ParameterDirection.Output, DbType.Int32)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_TPRL_Nome", "", ParameterDirection.Output, DbType.String, , 100)
'			.Parameters.Add(oParam)

'			oParam = objAccesso.GetParameter("@RLPC_attivato", "", ParameterDirection.Output, DbType.Byte)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_abilitato", "", ParameterDirection.Output, DbType.Byte)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_responsabile", "", ParameterDirection.Output, DbType.Byte)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_UltimoCollegamento", "", ParameterDirection.Output, DbType.DateTime)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_TPRL_Gerarchia", "", ParameterDirection.Output, DbType.Int32)
'			.Parameters.Add(oParam)

'			oParam = objAccesso.GetParameter("@RLPC_PenultimoCollegamento", "", ParameterDirection.Output, DbType.DateTime)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_IscrittoIl", "", ParameterDirection.Output, DbType.DateTime)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_isConnesso", "", ParameterDirection.Output, DbType.Int32)
'			.Parameters.Add(oParam)

'			oParam = objAccesso.GetParameter("@RLPC_SkipCover", "", ParameterDirection.Output, DbType.Int32)
'			.Parameters.Add(oParam)

'			oParam = objAccesso.GetParameter("@LinguaID", LinguaID, ParameterDirection.Input, DbType.Int32)
'			.Parameters.Add(oParam)

'			.Role = COL_Request.UserRole.Admin
'			.transactional = False
'		End With

'		Try
'			objAccesso.GetExecuteNotQuery(oRequest)
'			Me._PRSN.Id = PRSN_ID
'			Me._CMNT.Id = CMNT_ID
'			Me._id = oRequest.GetValueFromParameter(3)
'			Me.TipoRuolo.Id = oRequest.GetValueFromParameter(4)
'			Me.TipoRuolo.Nome = oRequest.GetValueFromParameter(5)
'			Me._attivato = oRequest.GetValueFromParameter(6)
'			Me._abilitato = oRequest.GetValueFromParameter(7)
'			Me._responsabile = oRequest.GetValueFromParameter(8)
'			If oRequest.GetValueFromParameter(9) <> "" Then
'				If IsDate(oRequest.GetValueFromParameter(9)) Then
'					Me._UltimoCollegamento = oRequest.GetValueFromParameter(9)
'				End If
'			End If
'			Me.TipoRuolo.Gerarchia = oRequest.GetValueFromParameter(10)
'			If oRequest.GetValueFromParameter(11) <> "" Then
'				If IsDate(oRequest.GetValueFromParameter(11)) Then
'					Me._PenultimoCollegamento = oRequest.GetValueFromParameter(11)
'				End If
'			End If
'			If oRequest.GetValueFromParameter(12) <> "" Then
'				If IsDate(oRequest.GetValueFromParameter(12)) Then
'					Me._IscrittoIl = oRequest.GetValueFromParameter(12)
'				End If
'			End If
'			Me._isConnesso = oRequest.GetValueFromParameter(13)
'			Try
'				Me._skipCover = oRequest.GetValueFromParameter(14)
'			Catch ex As Exception
'				Me._skipCover = 0
'			End Try
'			Me.n_Errore = Errori_Db.None
'		Catch ex As Exception 'record non trovato

'			Me.n_Errore = Errori_Db.DBReadExist
'		End Try
'	End Sub
'	Public Sub EstraiByLinguaDefault(ByVal CMNT_ID As Integer, ByVal PRSN_ID As Integer)
'		'in base alla comunità e all'id della persona mi ritorna il ruolo
'		Dim oRequest As New COL_Request
'		Dim oParam As New COL_Request.Parameter
'		Dim objAccesso As New COL_DataAccess

'		With oRequest
'			.Command = "sp_RuoloPersonaComunita_EstraiByLinguaDefault"
'			.CommandType = CommandType.StoredProcedure
'			oParam = objAccesso.GetParameter("@RLPC_PRSN_id", PRSN_ID, ParameterDirection.Input, DbType.Int32)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_CMNT_id", CMNT_ID, ParameterDirection.Input, DbType.Int32)
'			.Parameters.Add(oParam)

'			oParam = objAccesso.GetParameter("@RLPC_Id", "", ParameterDirection.Output, DbType.Int32)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_TPRL_id", "", ParameterDirection.Output, DbType.Int32)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_TPRL_Nome", "", ParameterDirection.Output, DbType.String, , 100)
'			.Parameters.Add(oParam)

'			oParam = objAccesso.GetParameter("@RLPC_attivato", "", ParameterDirection.Output, DbType.Byte)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_abilitato", "", ParameterDirection.Output, DbType.Byte)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_responsabile", "", ParameterDirection.Output, DbType.Byte)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_UltimoCollegamento", "", ParameterDirection.Output, DbType.DateTime)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_TPRL_Gerarchia", "", ParameterDirection.Output, DbType.Int32)
'			.Parameters.Add(oParam)

'			oParam = objAccesso.GetParameter("@RLPC_PenultimoCollegamento", "", ParameterDirection.Output, DbType.DateTime)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_IscrittoIl", "", ParameterDirection.Output, DbType.DateTime)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_isConnesso", "", ParameterDirection.Output, DbType.Int32)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_SkipCover", "", ParameterDirection.Output, DbType.Int32)
'			.Parameters.Add(oParam)

'			.Role = COL_Request.UserRole.Admin
'			.transactional = False
'		End With

'		Try
'			objAccesso.GetExecuteNotQuery(oRequest)
'			Me._PRSN.Id = PRSN_ID
'			Me._CMNT.Id = CMNT_ID
'			Me._id = oRequest.GetValueFromParameter(3)
'			Me.TipoRuolo.Id = oRequest.GetValueFromParameter(4)
'			Me.TipoRuolo.Nome = oRequest.GetValueFromParameter(5)
'			Me._attivato = oRequest.GetValueFromParameter(6)
'			Me._abilitato = oRequest.GetValueFromParameter(7)
'			Me._responsabile = oRequest.GetValueFromParameter(8)
'			If oRequest.GetValueFromParameter(9) <> "" Then
'				If IsDate(oRequest.GetValueFromParameter(9)) Then
'					Me._UltimoCollegamento = oRequest.GetValueFromParameter(9)
'				End If
'			End If
'			Me.TipoRuolo.Gerarchia = oRequest.GetValueFromParameter(10)
'			If oRequest.GetValueFromParameter(11) <> "" Then
'				If IsDate(oRequest.GetValueFromParameter(11)) Then
'					Me._PenultimoCollegamento = oRequest.GetValueFromParameter(11)
'				End If
'			End If
'			If oRequest.GetValueFromParameter(12) <> "" Then
'				If IsDate(oRequest.GetValueFromParameter(12)) Then
'					Me._IscrittoIl = oRequest.GetValueFromParameter(12)
'				End If
'			End If
'			Me._isConnesso = oRequest.GetValueFromParameter(13)
'			Try
'				Me._skipCover = oRequest.GetValueFromParameter(14)
'			Catch ex As Exception
'				Me._skipCover = 0
'			End Try
'			Me.n_Errore = Errori_Db.None
'		Catch ex As Exception 'record non trovato

'			Me.n_Errore = Errori_Db.DBReadExist
'		End Try
'	End Sub


'	Public Sub Aggiungi(Optional ByVal responsabile As Boolean = False)
'		Dim oRequest As New COL_Request
'		Dim oParam As New COL_Request.Parameter
'		Dim objAccesso As New COL_DataAccess
'		With oRequest
'			.Command = "sp_RuoloPersonaComunita_Aggiungi"
'			.CommandType = CommandType.StoredProcedure
'			oParam = objAccesso.GetParameter("@RLPC_Id", "", ParameterDirection.Output, DbType.Int32)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_CMNT_Id", _CMNT.Id, , DbType.Int32)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_PRSN_ID", _PRSN.Id, , DbType.Int32)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_TPRL_id", _TPRL.Id, , DbType.Int32)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_attivato", _attivato, , DbType.Byte)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_abilitato", _abilitato, , DbType.Byte)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_responsabile", _responsabile, , DbType.Byte)
'			.Parameters.Add(oParam)
'			.Role = COL_Request.UserRole.Admin
'			.transactional = False
'		End With
'		Try
'			objAccesso.GetExecuteNotQuery(oRequest)
'			' Recupero l'ID univoco del ruoloPersonaComunità
'			Me._id = oRequest.GetValueFromParameter(1)
'		Catch ex As Exception
'			Me._id = -1
'			Me.n_Errore = Errori_Db.DBInsert
'		End Try
'	End Sub
'	'##############################################################################
'	Public Function Modifica() As Integer
'		Dim oRequest As New COL_Request
'		Dim oParam As New COL_Request.Parameter
'		Dim objAccesso As New COL_DataAccess
'		With oRequest
'			.Command = "sp_RuoloPersonaComunita_Modifica"
'			.CommandType = CommandType.StoredProcedure
'			oParam = objAccesso.GetParameter("@RLPC_Id", _id, ParameterDirection.Input, DbType.Int32)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_CMNT_Id", _CMNT.Id, , DbType.Int32)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_PRSN_ID", _PRSN.Id, , DbType.Int32)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_TPRL_id", _TPRL.Id, , DbType.Int32)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_attivato", _attivato, , DbType.Byte)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_abilitato", _abilitato, , DbType.Byte)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_responsabile", Me._responsabile, , DbType.Byte)
'			.Parameters.Add(oParam)
'			.Role = COL_Request.UserRole.Admin
'			.transactional = False
'		End With
'		Try
'			objAccesso.GetExecuteNotQuery(oRequest)
'		Catch ex As Exception
'			Me.n_Errore = Errori_Db.DBChange
'		End Try
'	End Function

'	Public Shared Sub AbilitaAttiva(ByVal RLPC_id As Integer, ByVal eAbilita As Abilitazione)
'		'passo il valore dell'enum direttamente alla sp che gestirà i valori passati
'		Dim oRequest As New COL_Request
'		Dim oParam As New COL_Request.Parameter
'		Dim objAccesso As New COL_DataAccess
'		With oRequest
'			.Command = "sp_RuoloPersonaComunita_AbilitaAttiva"
'			.CommandType = CommandType.StoredProcedure
'			oParam = objAccesso.GetParameter("@RLPC_Id", RLPC_id, ParameterDirection.Input, DbType.Int32)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_attivato", CType(eAbilita, Abilitazione), , DbType.Int32)
'			.Parameters.Add(oParam)
'			.Role = COL_Request.UserRole.Admin
'			.transactional = False
'		End With
'		Try
'			objAccesso.GetExecuteNotQuery(oRequest)
'		Catch ex As Exception
'			'gestire errori delle shared
'		End Try
'	End Sub

'	Public Sub CaricaRuoloPersonaComunita()
'		'passando l'id della persona e quello della comunità ritorna l'id del ruolo_persona_comunita
'		'e quello del ruolo espletato nella relazione
'		Dim oRequest As New COL_Request
'		Dim oParam As New COL_Request.Parameter
'		Dim objAccesso As New COL_DataAccess
'		With oRequest
'			.Command = "sp_RuoloPersonaComunita_IdandRuolo"
'			.CommandType = CommandType.StoredProcedure
'			oParam = objAccesso.GetParameter("@RLPC_Id", "", ParameterDirection.Output, DbType.Int32)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_Ruolo", "", ParameterDirection.Output, DbType.Int32)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_CMNT_Id", _CMNT.Id, , DbType.Int32)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_PRSN_ID", _PRSN.Id, , DbType.Int32)
'			.Parameters.Add(oParam)
'			.Role = COL_Request.UserRole.Admin
'			.transactional = False
'		End With
'		Try
'			objAccesso.GetExecuteNotQuery(oRequest)
'			' Recupero l'ID univoco del ruoloPersonaComunità
'			Me._id = oRequest.GetValueFromParameter(1)
'			' carico l'id del ruolo
'			Me._TPRL.Id = oRequest.GetValueFromParameter(2)
'		Catch ex As Exception
'			Me._id = -1
'			Me._TPRL.Id = -1
'			Me.n_Errore = Errori_Db.DBInsert
'		End Try
'	End Sub

'	Public Function CambiaRuolo() As Integer
'		Dim oRequest As New COL_Request
'		Dim oParam As New COL_Request.Parameter
'		Dim objAccesso As New COL_DataAccess
'		With oRequest
'			.Command = "sp_RuoloPersonaComunita_Cambiaruolo"
'			.CommandType = CommandType.StoredProcedure
'			oParam = objAccesso.GetParameter("@RLPC_Id", _id, ParameterDirection.Input, DbType.Int32)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_TPRL_id", _TPRL.Id, , DbType.Int32)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_responsabile", Me._responsabile, , DbType.Int32)
'			.Parameters.Add(oParam)
'			.Role = COL_Request.UserRole.Admin
'			.transactional = False
'		End With
'		Try
'			objAccesso.GetExecuteNotQuery(oRequest)
'		Catch ex As Exception
'			Me.n_Errore = Errori_Db.DBChange
'		End Try
'	End Function

'	Public Function UpdateUltimocollegamento() As Integer
'		Dim oRequest As New COL_Request
'		Dim oParam As New COL_Request.Parameter
'		Dim objAccesso As New COL_DataAccess

'		Me._UltimoCollegamento = Now
'		With oRequest
'			.Command = "sp_RuoloPersonaComunita_UpdateUltimocollegamento"
'			.CommandType = CommandType.StoredProcedure
'			oParam = objAccesso.GetParameter("@RLPC_PRSN_id", Me._PRSN.Id, ParameterDirection.Input, DbType.Int32)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_CMNT_id", _CMNT.Id, ParameterDirection.Input, DbType.Int32)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_UltimoCollegamento", DateToString(Me._UltimoCollegamento, True), ParameterDirection.Input, DbType.String)
'			.Parameters.Add(oParam)

'			.Role = COL_Request.UserRole.Admin
'			.transactional = False
'		End With
'		Try
'			objAccesso.GetExecuteNotQuery(oRequest)
'			Me.n_Errore = Errori_Db.None
'		Catch ex As Exception
'			Me.n_Errore = Errori_Db.DBChange
'		End Try
'	End Function

'	Public Sub Elimina()
'		'disiscrive un utente da una comunità senza eseguire controlli (funzione temporanea da integrare con controlli)
'		Dim oRequest As New COL_Request
'		Dim oParam As New COL_Request.Parameter
'		Dim objAccesso As New COL_DataAccess

'		With oRequest
'			.Command = "sp_RuoloPersonaComunita_Elimina"
'			.CommandType = CommandType.StoredProcedure
'			oParam = objAccesso.GetParameter("@RLPC_id", Me._id, ParameterDirection.Input, DbType.Int32)
'			.Parameters.Add(oParam)
'			.Role = COL_Request.UserRole.Admin
'			.transactional = False
'		End With
'		Try
'			objAccesso.GetExecuteNotQuery(oRequest)
'			Me.n_Errore = Errori_Db.None
'		Catch ex As Exception
'			Me.n_Errore = Errori_Db.DBChange
'		End Try

'	End Sub

'	Public Sub EliminaInAttesa_(Optional ByVal tutti As Boolean = False)
'		'disiscrive un utente da una comunità mentre è in attesa

'		Dim oRequest As New COL_Request
'		Dim oParam As New COL_Request.Parameter
'		Dim objAccesso As New COL_DataAccess

'		With oRequest
'			.Command = "sp_RuoloPersonaComunita_EliminaInAttesa"
'			.CommandType = CommandType.StoredProcedure
'			oParam = objAccesso.GetParameter("@RLPC_id", Me._id, ParameterDirection.Input, DbType.Int32)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@tutti", tutti, ParameterDirection.Input, DbType.Boolean)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetParameter("@RLPC_CMNT_id", Me._CMNT.Id, ParameterDirection.Input, DbType.Int32)
'			.Parameters.Add(oParam)
'			.Role = COL_Request.UserRole.Admin
'			.transactional = False
'		End With
'		Try
'			objAccesso.GetExecuteNotQuery(oRequest)
'			Me.n_Errore = Errori_Db.None
'		Catch ex As Exception
'			Me.n_Errore = Errori_Db.DBChange
'		End Try

'	End Sub

'	Public Shared Function skipCover(ByVal CMNT_id As Integer, ByVal PRSN_ID As Integer, ByVal Skip As Boolean) As Boolean
'		'disiscrive un utente da una comunità mentre è in attesa

'		Dim oRequest As New COL_Request
'		Dim oParam As New COL_Request.Parameter
'		Dim objAccesso As New COL_DataAccess

'		With oRequest
'			.Command = "sp_RuoloPersonaComunita_skipCover"
'			.CommandType = CommandType.StoredProcedure
'			oParam = objAccesso.GetAdvancedParameter("@CMNT_id", CMNT_id, ParameterDirection.Input, SqlDbType.Int)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetAdvancedParameter("@PRSN_ID", PRSN_ID, ParameterDirection.Input, SqlDbType.Int)
'			.Parameters.Add(oParam)
'			If Skip Then
'				oParam = objAccesso.GetAdvancedParameter("@Skip", 1, ParameterDirection.Input, SqlDbType.Int)
'			Else
'				oParam = objAccesso.GetAdvancedParameter("@Skip", 0, ParameterDirection.Input, SqlDbType.Int)
'			End If

'			.Parameters.Add(oParam)
'			.Role = COL_Request.UserRole.Admin
'			.transactional = False
'		End With
'		Try
'			objAccesso.GetExecuteNotQuery(oRequest)
'			Return True
'		Catch ex As Exception
'			Return False
'		End Try

'	End Function
'	Public Function isSkipCover(ByVal CMNT_id As Integer, ByVal PRSN_ID As Integer) As Boolean
'		'disiscrive un utente da una comunità mentre è in attesa

'		Dim oRequest As New COL_Request
'		Dim oParam As New COL_Request.Parameter
'		Dim objAccesso As New COL_DataAccess

'		With oRequest
'			.Command = "sp_RuoloPersonaComunita_isSkipCover"
'			.CommandType = CommandType.StoredProcedure
'			oParam = objAccesso.GetAdvancedParameter("@CMNT_id", CMNT_id, ParameterDirection.Input, SqlDbType.Int)
'			.Parameters.Add(oParam)
'			oParam = objAccesso.GetAdvancedParameter("@PRSN_ID", PRSN_ID, ParameterDirection.Input, SqlDbType.Int)
'			.Parameters.Add(oParam)

'			oParam = objAccesso.GetAdvancedParameter("@Skip", 0, ParameterDirection.Output, SqlDbType.Int)
'			.Parameters.Add(oParam)
'			.Role = COL_Request.UserRole.Admin
'			.transactional = False
'		End With
'		Try
'			objAccesso.GetExecuteNotQuery(oRequest)
'			If oRequest.GetValueFromParameter(3) = 0 Then
'				Return False
'			Else
'				Return True
'			End If
'		Catch ex As Exception
'			Return False
'		End Try

'	End Function
'	End Class

'End Namespace