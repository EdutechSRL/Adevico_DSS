Public Class FileSystemItem

#Region "Private Property"
	Private _ID As Long
	Private _ComunitaID As Integer
	Private _Nome As String
	Private _Path As String
	Private _Descrizione As String
	Private _Extension As String
	Private _Size As Long
	Private _DownLoads As Long
	Private _isFolder As Boolean
	Private _isScorm As Boolean
	Private _isVideoCast As Boolean
	Private _isVisibile As Boolean
	Private _Creatore As COL_Persona
	Private _Proprietario As COL_RuoloPersonaComunita
	Private _Categoria As COL_CategoriaFile
	Private _CreatedDate As DateTime
	Private _Info1 As String
	Private _Info2 As String
	Private _Info3 As String
	Private _Autore As String
	Private _Ordine As Integer
	Private _Parent As FileSystemItem
	Private _fileCount As Long
	Private _subFolderCount As Long
#End Region

#Region "Public Property"
	Public Property ID() As Long
		Get
			Return _ID
		End Get
		Set(ByVal value As Long)
			_ID = value
		End Set
	End Property
	Public Property ComunitaID() As Integer
		Get
			Return _ComunitaID
		End Get
		Set(ByVal value As Integer)
			_ComunitaID = value
		End Set
	End Property
	Public Property Nome() As String
		Get
			Return _Nome
		End Get
		Set(ByVal value As String)
			_Nome = value
		End Set
	End Property
	Public Property Path() As String
		Get
			Return _Path
		End Get
		Set(ByVal value As String)
			_Path = value
		End Set
	End Property
	Public Property Descrizione() As String
		Get
			Return _Descrizione
		End Get
		Set(ByVal value As String)
			_Descrizione = value
		End Set
	End Property
	Public Property Extension() As String
		Get
			Return _Extension
		End Get
		Set(ByVal value As String)
			_Extension = value
			_Extension = _Extension.Replace(".", "")
		End Set
	End Property
	Public Property Size() As Long
		Get
			Return _Size
		End Get
		Set(ByVal value As Long)
			_Size = value
		End Set
	End Property
	Public Property DownLoads() As Long
		Get
			Return _DownLoads
		End Get
		Set(ByVal value As Long)
			_DownLoads = value
		End Set
	End Property
	Public Property IsFolder() As Boolean
		Get
			Return _isFolder
		End Get
		Set(ByVal value As Boolean)
			_isFolder = value
		End Set
	End Property
	Public Property isScorm() As Boolean
		Get
			Return _isScorm
		End Get
		Set(ByVal value As Boolean)
			_isScorm = value
		End Set
	End Property
	Public Property isVideoCast() As Boolean
		Get
			Return _isVideoCast
		End Get
		Set(ByVal value As Boolean)
			_isVideoCast = value
		End Set
	End Property
	Public Property isVisibile() As Boolean
		Get
			Return _isVisibile
		End Get
		Set(ByVal value As Boolean)
			_isVisibile = value
		End Set
	End Property
	Public Property Creatore() As COL_Persona
		Get
			Return _Creatore
		End Get
		Set(ByVal value As COL_Persona)
			_Creatore = value
		End Set
	End Property
	Public Property Proprietario() As COL_RuoloPersonaComunita
		Get
			Return _Proprietario
		End Get
		Set(ByVal value As COL_RuoloPersonaComunita)
			_Proprietario = value
		End Set
	End Property
	Public Property CreatedDate() As DateTime
		Get
			Return _CreatedDate
		End Get
		Set(ByVal value As DateTime)
			_CreatedDate = value
		End Set
	End Property
	Public Property Categoria() As COL_CategoriaFile
		Get
			Return _Categoria
		End Get
		Set(ByVal value As COL_CategoriaFile)
			_Categoria = value
		End Set
	End Property
	Public Property Info1() As String
		Get
			Return _Info1
		End Get
		Set(ByVal value As String)
			_Info1 = value
		End Set
	End Property
	Public Property Info2() As String
		Get
			Return _Info2
		End Get
		Set(ByVal value As String)
			_Info2 = value
		End Set
	End Property
	Public Property Info3() As String
		Get
			Return _Info3
		End Get
		Set(ByVal value As String)
			_Info3 = value
		End Set
	End Property
	Public Property Autore() As String
		Get
			Return _Autore
		End Get
		Set(ByVal value As String)
			_Autore = value
		End Set
	End Property
	Public Property Ordine() As Integer
		Get
			Return _Ordine
		End Get
		Set(ByVal value As Integer)
			_Ordine = value
		End Set
	End Property
	Public Property Parent() As FileSystemItem
		Get
			Return _Parent
		End Get
		Set(ByVal value As FileSystemItem)
			_Parent = value
		End Set
	End Property
	Public Property FileCount() As Long
		Get
			Return _fileCount
		End Get
		Set(ByVal value As Long)
			_fileCount = value
		End Set
	End Property
	Public Property SubFolderCount() As Long
		Get
			Return _subFolderCount
		End Get
		Set(ByVal value As Long)
			_subFolderCount = value
		End Set
	End Property
#End Region

	Sub New()

	End Sub

End Class