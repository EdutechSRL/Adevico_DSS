Namespace Comunita
	Public Class PlainComunita

#Region "Private Property"
		Private n_CMNT_id As Integer
		Private n_CMNT_idpadre As Integer
		Private n_CMNT_nome As String
		Private n_CMNT_PRSN_ID As Integer
		Private _OrganizzazioneID As Integer
		Private _ischiusa As Boolean
		Private _livello As Integer
		Private _CanUnsubscribe As Boolean
		Private _lvl As Integer
		Private _idPadre_Link As Integer
		Private _isIscritto As Boolean
		Private _RuoloID As Integer
		Private _TipoComunita As New COL_Tipo_Comunita
		Private _IsComunita As Boolean
		Private _path As String
		Private _REALpath As String
		Private _AnagraficaCreatore As String
		'	Private _Responsabile As String
		Private _Bloccata As Boolean
		Private _isArchiviata As Boolean
		Private _HasFigli As Boolean
		Private _isResponsabile As Boolean
		Private _isDiretto As Boolean
		Private _SottoComunita As GenericCollection(Of PlainComunita)

		Private n_CMNT_ischiusa As Boolean
		Private n_CMNT_livello As Integer
		Private n_CMNT_MaxIscritti As Integer

		Private n_CMNT_MaxIscrittiOverList As Integer
		Private _CanSubscribe As Boolean
		Private n_CMNT_AccessoLibero As Integer
		Private n_CMNT_AccessoCopisteria As Integer
		Private n_CMNT_tpcm As COL_Tipo_Comunita
		Private n_CMNT_datacreazione As DateTime
		Private n_CMNT_datacessazione As DateTime
		Private n_CMNT_dataInizioIscrizione As DateTime
		Private n_CMNT_dataFineIscrizione As DateTime
		Private n_CMNT_orgn As COL_Organizzazione
		Private _Creatore As COL_Persona
		Private _Responsabile As COL_Persona
		Private _Iscritti As Integer
		Private _Iscrizione As COL_RuoloPersonaComunita
		Private _IsChiusaForPadre As Boolean
		Private _AnagraficaResponsabile As String
		Private _isPercorsoDiretto As Boolean
#End Region

		Public Property isPercorsoDiretto() As Boolean
			Get
				isPercorsoDiretto = _isPercorsoDiretto
			End Get
			Set(ByVal Value As Boolean)
				_isPercorsoDiretto = Value
			End Set
		End Property

		Public Property AnagraficaResponsabile() As String
			Get
				AnagraficaResponsabile = _AnagraficaResponsabile
			End Get
			Set(ByVal Value As String)
				_AnagraficaResponsabile = Value
			End Set
		End Property
		Public Property Iscrizione() As COL_RuoloPersonaComunita
			Get
				Iscrizione = _Iscrizione
			End Get
			Set(ByVal Value As COL_RuoloPersonaComunita)
				_Iscrizione = Value
			End Set
		End Property
		Public Property Iscritti() As Integer
			Get
				Iscritti = _Iscritti
			End Get
			Set(ByVal Value As Integer)
				_Iscritti = Value
			End Set
		End Property

		Public Property Creatore() As COL_Persona
			Get
				Creatore = _Creatore
			End Get
			Set(ByVal Value As COL_Persona)
				_Creatore = Value
			End Set
		End Property
		Public Property Responsabile() As COL_Persona
			Get
				Responsabile = _Responsabile
			End Get
			Set(ByVal Value As COL_Persona)
				_Responsabile = Value
			End Set
		End Property

		Public Property DataCreazione() As DateTime
			Get
				DataCreazione = n_CMNT_datacreazione
			End Get
			Set(ByVal Value As DateTime)
				n_CMNT_datacreazione = Value
			End Set
		End Property
		Public Property DataCessazione() As DateTime
			Get
				DataCessazione = n_CMNT_datacessazione
			End Get
			Set(ByVal Value As DateTime)
				n_CMNT_datacessazione = Value
			End Set
		End Property
		Public Property DataInizioIscrizione() As DateTime
			Get
				DataInizioIscrizione = n_CMNT_dataInizioIscrizione
			End Get
			Set(ByVal Value As DateTime)
				n_CMNT_dataInizioIscrizione = Value
			End Set
		End Property
		Public Property DataFineIscrizione() As DateTime
			Get
				DataFineIscrizione = n_CMNT_dataFineIscrizione
			End Get
			Set(ByVal Value As DateTime)
				n_CMNT_dataFineIscrizione = Value
			End Set
		End Property
		Public Property Organizzazione() As COL_Organizzazione
			Get
				Organizzazione = n_CMNT_orgn
			End Get
			Set(ByVal Value As COL_Organizzazione)
				n_CMNT_orgn = Value
			End Set
		End Property
		Public Property MaxIscritti() As Integer
			Get
				MaxIscritti = n_CMNT_MaxIscritti
			End Get
			Set(ByVal Value As Integer)
				n_CMNT_MaxIscritti = Value
			End Set
		End Property


		Public Property isArchiviata() As Boolean
			Get
				isArchiviata = _isArchiviata
			End Get
			Set(ByVal Value As Boolean)
				_isArchiviata = Value

			End Set
		End Property
		Public Property CanSubscribe() As Boolean
			Get
				CanSubscribe = _CanSubscribe
			End Get
			Set(ByVal Value As Boolean)
				_CanSubscribe = Value

			End Set
		End Property
		Public Property MaxIscrittiOverList() As Integer
			Get
				MaxIscrittiOverList = n_CMNT_MaxIscrittiOverList
			End Get
			Set(ByVal Value As Integer)
				If Value < 0 Then
					Value = 0
				End If
				n_CMNT_MaxIscrittiOverList = Value
			End Set
		End Property

		Public Property HasAccessoLibero() As Boolean
			Get
				HasAccessoLibero = n_CMNT_AccessoLibero
			End Get
			Set(ByVal Value As Boolean)
				n_CMNT_AccessoLibero = Value
			End Set
		End Property
		Public Property HasAccessoCopisteria() As Boolean
			Get
				HasAccessoCopisteria = n_CMNT_AccessoCopisteria
			End Get
			Set(ByVal Value As Boolean)
				n_CMNT_AccessoCopisteria = Value
			End Set
		End Property

#Region "Public Property"
		Public Property SottoComunita() As GenericCollection(Of PlainComunita)
			Get
				SottoComunita = _SottoComunita
			End Get
			Set(ByVal Value As GenericCollection(Of PlainComunita))
				_SottoComunita = Value
			End Set
		End Property
		Public Property Id() As Integer
			Get
				Id = n_CMNT_id
			End Get
			Set(ByVal Value As Integer)
				n_CMNT_id = Value
			End Set
		End Property
		Public Property IdPadre() As Integer
			Get
				IdPadre = n_CMNT_idpadre
			End Get
			Set(ByVal Value As Integer)
				n_CMNT_idpadre = Value
			End Set
		End Property
		Public Property Nome() As String
			Get
				Nome = n_CMNT_nome
			End Get
			Set(ByVal Value As String)
				n_CMNT_nome = Value
			End Set
		End Property
		Public Property CreatoreID() As Integer
			Get
				CreatoreID = n_CMNT_PRSN_ID
			End Get
			Set(ByVal Value As Integer)
				n_CMNT_PRSN_ID = Value
			End Set
		End Property
		Public Property TipoComunita() As COL_Tipo_Comunita
			Get
				If IsNothing(_TipoComunita) Then
					_TipoComunita = New COL_Tipo_Comunita
				End If
				TipoComunita = _TipoComunita
			End Get
			Set(ByVal Value As COL_Tipo_Comunita)
				_TipoComunita = Value
			End Set
		End Property
		Public Property OrganizzazioneID() As Integer
			Get
				OrganizzazioneID = _OrganizzazioneID
			End Get
			Set(ByVal Value As Integer)
				_OrganizzazioneID = Value
			End Set
		End Property
		Public Property IsChiusa() As Boolean
			Get
				IsChiusa = _ischiusa
			End Get
			Set(ByVal Value As Boolean)
				_ischiusa = Value
			End Set
		End Property
		Public Property IsChiusaForPadre() As Boolean
			Get
				IsChiusaForPadre = _IsChiusaForPadre
			End Get
			Set(ByVal Value As Boolean)
				_IsChiusaForPadre = Value
			End Set
		End Property
		Public Property Livello() As Integer
			Get
				Livello = _livello
			End Get
			Set(ByVal Value As Integer)
				_livello = Value
			End Set
		End Property
		Public Property CanUnsubscribe() As Boolean
			Get
				CanUnsubscribe = _CanUnsubscribe
			End Get
			Set(ByVal Value As Boolean)
				_CanUnsubscribe = Value
			End Set
		End Property
		Public Property isBloccata() As Boolean
			Get
				isBloccata = _Bloccata
			End Get
			Set(ByVal Value As Boolean)
				_Bloccata = Value
			End Set
		End Property

		Public Property lvl() As Integer
			Get
				lvl = _lvl
			End Get
			Set(ByVal Value As Integer)
				_lvl = Value
			End Set
		End Property
		Public Property idPadre_Link() As Integer
			Get
				idPadre_Link = _idPadre_Link
			End Get
			Set(ByVal Value As Integer)
				_idPadre_Link = Value
			End Set
		End Property
		Public Property isIscritto() As Boolean
			Get
				isIscritto = _isIscritto
			End Get
			Set(ByVal Value As Boolean)
				_isIscritto = Value
			End Set
		End Property
		Public Property RuoloID() As Integer
			Get
				RuoloID = _RuoloID
			End Get
			Set(ByVal Value As Integer)
				_RuoloID = Value
			End Set
		End Property
		Public Property IsComunita() As Integer
			Get
				IsComunita = _IsComunita
			End Get
			Set(ByVal Value As Integer)
				_IsComunita = Value
			End Set
		End Property
		Public Property Path() As String
			Get
				Path = _path
			End Get
			Set(ByVal Value As String)
				_path = Value
			End Set
		End Property
		Public Property REALpath() As String
			Get
				REALpath = _REALpath
			End Get
			Set(ByVal Value As String)
				_REALpath = Value
			End Set
		End Property
		Public Property AnagraficaCreatore() As String
			Get
				AnagraficaCreatore = _AnagraficaCreatore
			End Get
			Set(ByVal Value As String)
				_AnagraficaCreatore = Value
			End Set
		End Property
		'Public Property Responsabile() As String
		'	Get
		'		Responsabile = _Responsabile
		'	End Get
		'	Set(ByVal Value As String)
		'		_Responsabile = Value
		'	End Set
		'End Property
		Public Property HasFigli() As Boolean
			Get
				HasFigli = _HasFigli
			End Get
			Set(ByVal Value As Boolean)
				_HasFigli = Value
			End Set
		End Property
		Public Property isResponsabile() As Boolean
			Get
				isResponsabile = _isResponsabile
			End Get
			Set(ByVal Value As Boolean)
				_isResponsabile = Value
			End Set
		End Property
		Public Property isDiretto() As Boolean
			Get
				isDiretto = _isDiretto
			End Get
			Set(ByVal Value As Boolean)
				_isDiretto = Value
			End Set
		End Property
#End Region

	End Class
End Namespace