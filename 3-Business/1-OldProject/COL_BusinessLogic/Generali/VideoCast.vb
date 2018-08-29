Public Class VideoCast
	Inherits ObjectBase

#Region "Private Property"
	Private _ID As Integer
	Private _Titolo As String
	Private _Comunita As COL_Comunita
	Private _NomeFile As String
	Private _Descrizione As String
	Private _Percorso As String

	Private _DataCreazione As DateTime
	Private _DataModifica As DateTime
	Private _DataRipresa As DateTime
	Private _NumeroDownload As Integer
	Private _Autore As String
	Private _Durata As String
	Private _isVisibile As Boolean
	Private _isPubblico As Boolean
	Private _isDownLodable As Boolean
	Private _isCancellato As Boolean
#End Region

	'VDCS_CreatoreID
	'VDCS_ModificatoDa
	'VDCS_CartellaID
	'VDCS_CMNT_ID
	'VDCS
	'VDCS
	'VDCS_Path


#Region "Pubblic Property"
	Public Property ID() As Integer
		Get
			ID = _ID
		End Get
		Set(ByVal value As Integer)
			_ID = value
		End Set
	End Property
	Public Property Titolo() As String
		Get
			Titolo = _Titolo
		End Get
		Set(ByVal value As String)
			_Titolo = value
		End Set
	End Property
	Public Property Descrizione() As String
		Get
			Descrizione = _Descrizione
		End Get
		Set(ByVal value As String)
			_Descrizione = value
		End Set
	End Property
	Public Property NomeFile() As String
		Get
			NomeFile = _NomeFile
		End Get
		Set(ByVal value As String)
			_NomeFile = value
		End Set
	End Property

	Public Property DataCreazione() As DateTime
		Get
			DataCreazione = _DataCreazione
		End Get
		Set(ByVal value As DateTime)
			_DataCreazione = value
		End Set
	End Property
	Public Property DataModifica() As DateTime
		Get
			DataModifica = _DataModifica
		End Get
		Set(ByVal value As DateTime)
			_DataModifica = value
		End Set
	End Property
	Public Property DataRipresa() As DateTime
		Get
			DataRipresa = _DataRipresa
		End Get
		Set(ByVal value As DateTime)
			_DataRipresa = value
		End Set
	End Property

	Public Property NumeroDownload() As Integer
		Get
			NumeroDownload = _NumeroDownload
		End Get
		Set(ByVal value As Integer)
			_NumeroDownload = value
		End Set
	End Property
	Public Property Autore() As String
		Get
			Autore = _Autore
		End Get
		Set(ByVal value As String)
			_Autore = value
		End Set
	End Property
	Public Property Durata() As String
		Get
			Durata = _Durata
		End Get
		Set(ByVal value As String)
			_Durata = value
		End Set
	End Property
	Public Property isVisibile() As Boolean
		Get
			isVisibile = _isVisibile
		End Get
		Set(ByVal value As Boolean)
			_isVisibile = value
		End Set
	End Property
	Public Property isPubblico() As Boolean
		Get
			isPubblico = _isPubblico
		End Get
		Set(ByVal value As Boolean)
			_isPubblico = value
		End Set
	End Property
	Public Property isDownLodable() As Boolean
		Get
			isDownLodable = _isDownLodable
		End Get
		Set(ByVal value As Boolean)
			_isDownLodable = value
		End Set
	End Property
	Public Property isCancellato() As Boolean
		Get
			isCancellato = _isCancellato
		End Get
		Set(ByVal value As Boolean)
			_isCancellato = value
		End Set
	End Property
#End Region

	Public Sub New()
		Me._isCancellato = False
		Me._isDownLodable = False
		Me._isPubblico = False
		Me._isVisibile = True
		Me._NumeroDownload = 0
	End Sub



End Class