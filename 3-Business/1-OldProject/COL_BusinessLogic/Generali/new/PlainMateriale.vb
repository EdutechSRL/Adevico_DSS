Imports COL_BusinessLogic_v2.FileLayer

Public Class PlainMateriale

#Region "Private Property"
	Private _FileID As Integer
	Private _comunitaID As New Integer
	Private _Nome As String
	Private _Descrizione As String
	Private _Uploaded As DateTime
	Private _Dimensione As Integer
	Private _NumDownload As Integer
	Private _Percorso As String
	Private _Categoria As New COL_CategoriaFile
	Private _Autore As String
	Private _Visibile As Boolean
	Private _isFile As Boolean
	Private _isNew As Boolean
	Private _Ordine As Integer
	Private _Proprietario As String
	Private _Creatore As COL_Persona
	Private _BaseUrlDownload As String
	Private _BaseUrlPath As String
	Private _BaseUrlDrivePath As String
#End Region

#Region "Public Property"
	Public Property Id() As Integer
		Get
			Id = _FileID
		End Get
		Set(ByVal Value As Integer)
			_FileID = Value
		End Set
	End Property
	Public Property Ordine() As Integer
		Get
			Ordine = _Ordine
		End Get
		Set(ByVal Value As Integer)
			_Ordine = Value
		End Set
	End Property
	Public Property ComunitaID() As Integer
		Get
			ComunitaID = _comunitaID
		End Get
		Set(ByVal Value As Integer)
			_comunitaID = Value
		End Set
	End Property
	Public Property Nome() As String
		Get
			Nome = _Nome
		End Get
		Set(ByVal Value As String)
			_Nome = COL_File.FileNameOnly(Value) 'qui viene richiamata la funzione che pulisce il nome del file
		End Set
	End Property
	Public Property Descrizione() As String
		Get
			Descrizione = _Descrizione
		End Get
		Set(ByVal Value As String)
			_Descrizione = Value
		End Set
	End Property
	Public Property DataUpload() As DateTime
		Get
			DataUpload = _Uploaded
		End Get
		Set(ByVal Value As DateTime)
			_Uploaded = Value
		End Set
	End Property
	Public ReadOnly Property Dimensione() As String
		Get
			If _Dimensione = 0 Then
				Dimensione = ""
			ElseIf _Dimensione < 1000 Then
				Dimensione = _Dimensione.ToString & " kb"
			ElseIf _Dimensione Then
				Dimensione = (_Dimensione / 1000).ToString & " mb"
			End If
			Dimensione = _Dimensione
		End Get
	End Property
	Public Property NumeroScaricamenti() As Integer
		Get
			NumeroScaricamenti = _NumDownload
		End Get
		Set(ByVal Value As Integer)
			_NumDownload = Value
		End Set
	End Property
	Public Property PercorsoFile() As String
		Get
			PercorsoFile = _Percorso
		End Get
		Set(ByVal Value As String)
			_Percorso = Value
		End Set
	End Property
	Public Property Autore() As String
		Get
			Autore = _Autore
		End Get
		Set(ByVal Value As String)
			_Autore = Value
		End Set
	End Property
	Public Property Categoria() As COL_CategoriaFile
		Get
			Categoria = _Categoria
		End Get
		Set(ByVal Value As COL_CategoriaFile)
			_Categoria = Value
		End Set
	End Property
	Public Property isVisibile() As Boolean
		Get
			isVisibile = _Visibile
		End Get
		Set(ByVal Value As Boolean)
			_Visibile = Value
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
	Public Property isFile() As Boolean
		Get
			isFile = _isFile
		End Get
		Set(ByVal Value As Boolean)
			_isFile = Value
		End Set
	End Property
	Public ReadOnly Property isNew() As Boolean
		Get
			isNew = _isNew
		End Get
	End Property
	Public Property Proprietario() As String
		Get
			Proprietario = _Proprietario
		End Get
		Set(ByVal Value As String)
			_Proprietario = Value
		End Set
	End Property

	Public ReadOnly Property DownloadUrl()
		Get
			DownloadUrl = _BaseUrlDownload & ".download?FileID=" & _FileID.ToString
		End Get
	End Property
	Public ReadOnly Property Estensione() As String
		Get
			Try
				Estensione = Me._Nome.Substring((Me._Nome.LastIndexOf(".")) + 1)
			Catch ex As Exception
				Estensione = """"
			End Try
		End Get
	End Property
	Public ReadOnly Property ImageEstensione() As String
		Get
            If lm.Comol.Core.File.Exists.File(_BaseUrlDrivePath & "images/ico/" & Estensione & ".gif") Then
                ImageEstensione = _BaseUrlPath & "images/ico/" & Estensione & ".gif"
            Else
                ImageEstensione = _BaseUrlPath & "images/ico/bo.gif"
            End If
		End Get
	End Property



#End Region

	Public Sub New(ByVal FileID As Integer, ByVal isNew As Boolean, ByVal Nome As String, ByVal Percorso As String, ByVal Descrizione As String, _
 ByVal Dimensione As Integer, ByVal Ordine As Integer, ByVal NumDownload As Integer, ByVal Uploaded As DateTime, _
 ByVal Visibile As Boolean, ByVal Autore As String, ByVal comunitaID As Integer, ByVal CategoriaFile As COL_CategoriaFile, ByVal Creatore As COL_Persona, ByVal Proprietario As String, ByVal isFile As String, _
 ByVal BaseUrlDownload As String, ByVal BaseUrlPath As String, ByVal BaseUrlDrivePath As String)
		_FileID = FileID
		_isNew = isNew
		_Nome = Nome
		_Descrizione = Descrizione
		_Dimensione = Dimensione
		_Ordine = Ordine
		_NumDownload = NumDownload
		_Uploaded = Uploaded
		_Percorso = Percorso
		_Visibile = Visibile
		_Autore = Autore
		_comunitaID = comunitaID
		_Categoria = CategoriaFile
		_Creatore = Creatore
		_Proprietario = Proprietario
		_isFile = isFile
		_BaseUrlDownload = BaseUrlDownload
		_BaseUrlPath = BaseUrlPath
		_BaseUrlDrivePath = BaseUrlDrivePath
	End Sub
End Class