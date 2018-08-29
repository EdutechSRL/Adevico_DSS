Imports COL_DataLayer
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.FileLayer
Imports lm.Comol.Core.File

<Serializable()> _
Public Class COL_Cover

#Region "Private Property"
	Private _id As Integer
	Private _CMNT_ID As Integer
	Private _DataCreazione As DateTime
	Private _DataModifica As DateTime
	Private _Titolo As String
	Private _Anno As String
	Private _Commenti As String
	Private _Immagine As String
	Private _Didascalia As String
	Private _FontFace1 As String
	Private _FontFace2 As String
	Private _FontFace3 As String
	Private _FontSize1 As Integer
	Private _FontSize2 As Integer
	Private _FontSize3 As Integer
	Private _FontColor1 As String
	Private _FontColor2 As String
	Private _FontColor3 As String
	Private _Bold1 As Integer
	Private _Bold2 As Integer
	Private _Bold3 As Integer
	Private _Italic1 As Integer
	Private _Italic2 As Integer
	Private _Italic3 As Integer
	Private _PRSN_id As Integer
	Private _isAttivata As Integer
	Private _Errore As Errori_Db
#End Region

#Region "Public Property"
	Public Property Id() As Integer
		Get
			Id = _id
		End Get
		Set(ByVal Value As Integer)
			_id = Value
		End Set
	End Property
	Public Property CMNT_ID() As Integer
		Get
			CMNT_ID = _CMNT_ID
		End Get
		Set(ByVal Value As Integer)
			_CMNT_ID = Value
		End Set
	End Property
	Public Property DataCreazione() As DateTime
		Get
			DataCreazione = _DataCreazione
		End Get
		Set(ByVal Value As DateTime)
			_DataCreazione = Value
		End Set
	End Property
	Public Property DataModifica() As DateTime
		Get
			DataModifica = _DataModifica
		End Get
		Set(ByVal Value As DateTime)
			_DataModifica = Value
		End Set
	End Property
	Public Property Titolo() As String
		Get
			Titolo = _Titolo
		End Get
		Set(ByVal Value As String)
			_Titolo = Value
		End Set
	End Property
	Public Property Anno() As String
		Get
			Anno = _Anno
		End Get
		Set(ByVal Value As String)
			_Anno = Value
		End Set
	End Property
	Public Property Commenti() As String
		Get
			Commenti = _Commenti
		End Get
		Set(ByVal Value As String)
			_Commenti = Value
		End Set
	End Property
	Public Property Immagine() As String
		Get
			Immagine = _Immagine
		End Get
		Set(ByVal Value As String)
			_Immagine = Value
		End Set
	End Property
	Public Property Didascalia() As String
		Get
			Didascalia = _Didascalia
		End Get
		Set(ByVal Value As String)
			_Didascalia = Value
		End Set
	End Property
	Public Property FontFace1() As String
		Get
			FontFace1 = _FontFace1
		End Get
		Set(ByVal Value As String)
			_FontFace1 = Value
		End Set
	End Property
	Public Property FontFace2() As String
		Get
			FontFace2 = _FontFace2
		End Get
		Set(ByVal Value As String)
			_FontFace2 = Value
		End Set
	End Property
	Public Property FontFace3() As String
		Get
			FontFace3 = _FontFace3
		End Get
		Set(ByVal Value As String)
			_FontFace3 = Value
		End Set
	End Property
	Public Property FontSize1() As Integer
		Get
			FontSize1 = _FontSize1
		End Get
		Set(ByVal Value As Integer)
			_FontSize1 = Value
		End Set
	End Property
	Public Property FontSize2() As Integer
		Get
			FontSize2 = _FontSize2
		End Get
		Set(ByVal Value As Integer)
			_FontSize2 = Value
		End Set
	End Property
	Public Property FontSize3() As Integer
		Get
			FontSize3 = _FontSize3
		End Get
		Set(ByVal Value As Integer)
			_FontSize3 = Value
		End Set
	End Property
	Public Property FontColor1() As String
		Get
			FontColor1 = _FontColor1
		End Get
		Set(ByVal Value As String)
			_FontColor1 = Value
		End Set
	End Property
	Public Property FontColor2() As String
		Get
			FontColor2 = _FontColor2
		End Get
		Set(ByVal Value As String)
			_FontColor2 = Value
		End Set
	End Property
	Public Property FontColor3() As String
		Get
			FontColor3 = _FontColor3
		End Get
		Set(ByVal Value As String)
			_FontColor3 = Value
		End Set
	End Property

	Public Property IDautore() As Integer
		Get
			IDautore = _PRSN_id
		End Get
		Set(ByVal Value As Integer)
			_PRSN_id = Value
		End Set
	End Property
	Public ReadOnly Property ErroreDB() As Errori_Db
		Get
			ErroreDB = _Errore
		End Get
	End Property

	Public Property Bold1() As Boolean
		Get
			Bold1 = (_Bold1 = 1)
		End Get
		Set(ByVal Value As Boolean)
			If Value Then
				_Bold1 = 1
			Else
				_Bold1 = 0
			End If
		End Set
	End Property
	Public Property Bold2() As Boolean
		Get
			Bold2 = (_Bold2 = 1)
		End Get
		Set(ByVal Value As Boolean)
			If Value Then
				_Bold2 = 1
			Else
				_Bold2 = 0
			End If
		End Set
	End Property
	Public Property Bold3() As Boolean
		Get
			Bold3 = (_Bold3 = 1)
		End Get
		Set(ByVal Value As Boolean)
			If Value Then
				_Bold3 = 1
			Else
				_Bold3 = 0
			End If
		End Set
	End Property

	Public Property Italic1() As Boolean
		Get
			Italic1 = (_Italic1 = 1)
		End Get
		Set(ByVal Value As Boolean)
			If Value Then
				_Italic1 = 1
			Else
				_Italic1 = 0
			End If
		End Set
	End Property
	Public Property Italic2() As Boolean
		Get
			Italic2 = (_Italic2 = 1)
		End Get
		Set(ByVal Value As Boolean)
			If Value Then
				_Italic2 = 1
			Else
				_Italic2 = 0
			End If
		End Set
	End Property
	Public Property Italic3() As Boolean
		Get
			Italic3 = (_Italic3 = 1)
		End Get
		Set(ByVal Value As Boolean)
			If Value Then
				_Italic3 = 1
			Else
				_Italic3 = 0
			End If
		End Set
	End Property
	Public Property isAttivata() As Boolean
		Get
			isAttivata = (_isAttivata = 1)
		End Get
		Set(ByVal Value As Boolean)
			If Value Then
				_isAttivata = 1
			Else
				_isAttivata = 0
			End If
		End Set
	End Property


	Public ReadOnly Property ImageExist(ByVal Percorso As String) As Boolean
		Get
            Return Exists.File(Percorso & Me._Immagine)
		End Get
	End Property
#End Region

#Region "Metodi New"
	Sub New()
		Me._Errore = Errori_Db.None
		Me._Bold1 = 0
		Me._Bold2 = 0
		Me._Bold3 = 0
		Me._Italic1 = 0
		Me._Italic2 = 0
		Me._Italic3 = 0
		Me._isAttivata = 0
	End Sub
#End Region

#Region "Metodi Standard"
	Public Sub Estrai()
		Dim oRequest As New COL_Request
		Dim oParam As New COL_Request.Parameter
		Dim objAccesso As New COL_DataAccess

		With oRequest
			.Command = "sp_Cover_estrai"
			.CommandType = CommandType.StoredProcedure
			oParam = objAccesso.GetAdvancedParameter("@CVER_id", Me._id, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_CMNT_ID", "", ParameterDirection.Output, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_DataCreazione", "", ParameterDirection.Output, SqlDbType.DateTime)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_DataModifica", "", ParameterDirection.Output, SqlDbType.DateTime)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Titolo", "", ParameterDirection.Output, SqlDbType.VarChar, , 300)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Anno", "", ParameterDirection.Output, SqlDbType.VarChar, , 150)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Commenti", "", ParameterDirection.Output, SqlDbType.VarChar, , 5000)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Immagine", "", ParameterDirection.Output, SqlDbType.VarChar, , 300)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Didascalia", "", ParameterDirection.Output, SqlDbType.VarChar, , 200)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontFace1", "", ParameterDirection.Output, SqlDbType.VarChar, , 50)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontFace2", "", ParameterDirection.Output, SqlDbType.VarChar, , 50)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontFace3", "", ParameterDirection.Output, SqlDbType.VarChar, , 50)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontSize1", "", ParameterDirection.Output, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontSize2", "", ParameterDirection.Output, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontSize3", "", ParameterDirection.Output, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontColor1", "", ParameterDirection.Output, SqlDbType.VarChar, , 50)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontColor2", "", ParameterDirection.Output, SqlDbType.VarChar, , 50)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontColor3", "", ParameterDirection.Output, SqlDbType.VarChar, , 50)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_PRSN_id", "", ParameterDirection.Output, SqlDbType.Int)
			.Parameters.Add(oParam)

			oParam = objAccesso.GetAdvancedParameter("@CVER_Bold1", "", ParameterDirection.Output, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Bold2", "", ParameterDirection.Output, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Bold3", "", ParameterDirection.Output, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Italic1", "", ParameterDirection.Output, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Italic2", "", ParameterDirection.Output, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Italic3", "", ParameterDirection.Output, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_isAttivata", "", ParameterDirection.Output, SqlDbType.Int)
			.Parameters.Add(oParam)

			.Role = COL_Request.UserRole.Admin
			.transactional = False
		End With
		Try

			objAccesso.GetExecuteNotQuery(oRequest)
			Me._CMNT_ID = oRequest.GetValueFromParameter(2)
			If oRequest.GetValueFromParameter(3) <> "" Then
				Try
					If Not Equals(oRequest.GetValueFromParameter(3), New Date) Then
						Me._DataCreazione = oRequest.GetValueFromParameter(3)
					End If
				Catch ex As Exception

				End Try
			End If
			If oRequest.GetValueFromParameter(4) <> "" Then
				Try
					If Not Equals(oRequest.GetValueFromParameter(3), New Date) Then
						Me._DataModifica = oRequest.GetValueFromParameter(4)
					End If
				Catch ex As Exception

				End Try
			End If

			Me._Titolo = oRequest.GetValueFromParameter(5)
			Me._Anno = oRequest.GetValueFromParameter(6)
			Me._Commenti = oRequest.GetValueFromParameter(7)
			Me._Immagine = oRequest.GetValueFromParameter(8)
			Me._Didascalia = oRequest.GetValueFromParameter(9)
			Me._FontFace1 = oRequest.GetValueFromParameter(10)
			Me._FontFace2 = oRequest.GetValueFromParameter(11)
			Me._FontFace3 = oRequest.GetValueFromParameter(12)
			Me._FontSize1 = oRequest.GetValueFromParameter(13)
			Me._FontSize2 = oRequest.GetValueFromParameter(14)
			Me._FontSize3 = oRequest.GetValueFromParameter(15)
			Me._FontColor1 = oRequest.GetValueFromParameter(16)
			Me._FontColor2 = oRequest.GetValueFromParameter(17)
			Me._FontColor3 = oRequest.GetValueFromParameter(18)
			Me._isAttivata = oRequest.GetValueFromParameter(19)
			Me._Bold1 = oRequest.GetValueFromParameter(20)
			Me._Bold2 = oRequest.GetValueFromParameter(21)
			Me._Bold3 = oRequest.GetValueFromParameter(22)
			Me._Italic1 = oRequest.GetValueFromParameter(23)
			Me._Italic2 = oRequest.GetValueFromParameter(24)
			Me._Italic3 = oRequest.GetValueFromParameter(25)
			Me._PRSN_id = oRequest.GetValueFromParameter(26)
		Catch ex As Exception
			Me._Errore = Errori_Db.DBReadExist
		End Try
	End Sub
	Public Sub Aggiungi()
		Dim oRequest As New COL_Request
		Dim oParam As New COL_Request.Parameter
		Dim objAccesso As New COL_DataAccess
		Dim DataCreazione As String

		DataCreazione = DateToString(Now)
		With oRequest
			.Command = "sp_Cover_Aggiungi"
			.CommandType = CommandType.StoredProcedure
			oParam = objAccesso.GetAdvancedParameter("@CVER_id", Me._id, ParameterDirection.Output, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_CMNT_ID", Me._CMNT_ID, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_DataCreazione", DataCreazione, ParameterDirection.Input, SqlDbType.VarChar, , 30)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Titolo", Me._Titolo, ParameterDirection.Input, SqlDbType.VarChar, , 300)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Anno", Me._Anno, ParameterDirection.Input, SqlDbType.VarChar, , 150)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Commenti", Me._Commenti, ParameterDirection.Input, SqlDbType.VarChar, , 5000)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Immagine", Me._Immagine, ParameterDirection.Input, SqlDbType.VarChar, , 300)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Didascalia", Me._Didascalia, ParameterDirection.Input, SqlDbType.VarChar, , 200)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontFace1", Me._FontFace1, ParameterDirection.Input, SqlDbType.VarChar, , 50)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontFace2", Me._FontFace2, ParameterDirection.Input, SqlDbType.VarChar, , 50)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontFace3", Me._FontFace3, ParameterDirection.Input, SqlDbType.VarChar, , 50)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontSize1", Me._FontSize1, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontSize2", Me._FontSize2, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontSize3", Me._FontSize3, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontColor1", Me._FontColor1, ParameterDirection.Input, SqlDbType.VarChar, , 50)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontColor2", Me._FontColor2, ParameterDirection.Input, SqlDbType.VarChar, , 50)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontColor3", Me._FontColor3, ParameterDirection.Input, SqlDbType.VarChar, , 50)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_PRSN_id", Me._PRSN_id, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)

			oParam = objAccesso.GetAdvancedParameter("@CVER_Bold1", Me._Bold1, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Bold2", Me._Bold2, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Bold3", Me._Bold3, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Italic1", Me._Italic1, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Italic2", Me._Italic2, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Italic3", Me._Italic3, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_isAttivata", Me._isAttivata, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)

			.Role = COL_Request.UserRole.Admin
			.transactional = False
		End With
		Try
			objAccesso.GetExecuteNotQuery(oRequest)
			Me._id = oRequest.GetValueFromParameter(1)
			Me._Errore = Errori_Db.None
		Catch ex As Exception
			Me._id = -1
			Me._Errore = Errori_Db.DBInsert
		End Try
	End Sub
	Public Sub Modifica()
		Dim oRequest As New COL_Request
		Dim oParam As New COL_Request.Parameter
		Dim objAccesso As New COL_DataAccess
		Dim DataModifica As String
		DataModifica = DateToString(Now)
		With oRequest
			.Command = "sp_Cover_Modifica"
			.CommandType = CommandType.StoredProcedure
			oParam = objAccesso.GetAdvancedParameter("@CVER_id", Me._id, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_CMNT_ID", Me._CMNT_ID, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_DataModifica", DataModifica, ParameterDirection.Input, SqlDbType.VarChar, , 30)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Titolo", Me._Titolo, ParameterDirection.Input, SqlDbType.VarChar, , 300)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Anno", Me._Anno, ParameterDirection.Input, SqlDbType.VarChar, , 150)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Commenti", Me._Commenti, ParameterDirection.Input, SqlDbType.VarChar, , 5000)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Immagine", Me._Immagine, ParameterDirection.Input, SqlDbType.VarChar, , 300)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Didascalia", Me._Didascalia, ParameterDirection.Input, SqlDbType.VarChar, , 200)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontFace1", Me._FontFace1, ParameterDirection.Input, SqlDbType.VarChar, , 50)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontFace2", Me._FontFace2, ParameterDirection.Input, SqlDbType.VarChar, , 50)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontFace3", Me._FontFace3, ParameterDirection.Input, SqlDbType.VarChar, , 50)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontSize1", Me._FontSize1, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontSize2", Me._FontSize2, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontSize3", Me._FontSize3, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontColor1", Me._FontColor1, ParameterDirection.Input, SqlDbType.VarChar, , 50)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontColor2", Me._FontColor2, ParameterDirection.Input, SqlDbType.VarChar, , 50)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontColor3", Me._FontColor3, ParameterDirection.Input, SqlDbType.VarChar, , 50)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_PRSN_id", Me._PRSN_id, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Bold1", Me._Bold1, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Bold2", Me._Bold2, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Bold3", Me._Bold3, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Italic1", Me._Italic1, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Italic2", Me._Italic2, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Italic3", Me._Italic3, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_isAttivata", Me._isAttivata, ParameterDirection.Input, SqlDbType.Int)
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
	Public Sub EstraiFromComunita(ByVal CMNT_ID As Integer)
		Dim oRequest As New COL_Request
		Dim oParam As New COL_Request.Parameter
		Dim objAccesso As New COL_DataAccess
		Dim oDataset As New DataSet
		With oRequest
			.Command = "sp_Cover_estraiFromComunita"
			.CommandType = CommandType.StoredProcedure
			oParam = objAccesso.GetAdvancedParameter("@CVER_id", "", ParameterDirection.Output, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_CMNT_ID", CMNT_ID, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_DataCreazione", "", ParameterDirection.Output, SqlDbType.DateTime)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_DataModifica", "", ParameterDirection.Output, SqlDbType.DateTime)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Titolo", "", ParameterDirection.Output, SqlDbType.VarChar, , 300)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Anno", "", ParameterDirection.Output, SqlDbType.VarChar, , 150)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Commenti", "", ParameterDirection.Output, SqlDbType.VarChar, , 5000)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Immagine", "", ParameterDirection.Output, SqlDbType.VarChar, , 300)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Didascalia", "", ParameterDirection.Output, SqlDbType.VarChar, , 200)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontFace1", "", ParameterDirection.Output, SqlDbType.VarChar, , 50)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontFace2", "", ParameterDirection.Output, SqlDbType.VarChar, , 50)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontFace3", "", ParameterDirection.Output, SqlDbType.VarChar, , 50)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontSize1", "", ParameterDirection.Output, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontSize2", "", ParameterDirection.Output, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontSize3", "", ParameterDirection.Output, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontColor1", "", ParameterDirection.Output, SqlDbType.VarChar, , 50)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontColor2", "", ParameterDirection.Output, SqlDbType.VarChar, , 50)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontColor3", "", ParameterDirection.Output, SqlDbType.VarChar, , 50)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_PRSN_id", "", ParameterDirection.Output, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Bold1", "", ParameterDirection.Output, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Bold2", "", ParameterDirection.Output, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Bold3", "", ParameterDirection.Output, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Italic1", "", ParameterDirection.Output, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Italic2", "", ParameterDirection.Output, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Italic3", "", ParameterDirection.Output, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_isAttivata", "", ParameterDirection.Output, SqlDbType.Int)
			.Parameters.Add(oParam)
			.Role = COL_Request.UserRole.Admin
			.transactional = False
		End With
		Try
			objAccesso.GetExecuteNotQuery(oRequest)
			Me._id = oRequest.GetValueFromParameter(1)
			If oRequest.GetValueFromParameter(3) <> "" Then
				Try
					If Not Equals(oRequest.GetValueFromParameter(3), New Date) Then
						Me._DataCreazione = oRequest.GetValueFromParameter(3)
					End If
				Catch ex As Exception

				End Try
			End If
			If oRequest.GetValueFromParameter(4) <> "" Then
				Try
					If Not Equals(oRequest.GetValueFromParameter(3), New Date) Then
						Me._DataModifica = oRequest.GetValueFromParameter(4)
					End If
				Catch ex As Exception

				End Try
			End If

			Me._Titolo = oRequest.GetValueFromParameter(5)
			Me._Anno = oRequest.GetValueFromParameter(6)
			Me._Commenti = oRequest.GetValueFromParameter(7)
			Me._Immagine = oRequest.GetValueFromParameter(8)
			Me._Didascalia = oRequest.GetValueFromParameter(9)
			Me._FontFace1 = oRequest.GetValueFromParameter(10)
			Me._FontFace2 = oRequest.GetValueFromParameter(11)
			Me._FontFace3 = oRequest.GetValueFromParameter(12)
			Me._FontSize1 = oRequest.GetValueFromParameter(13)
			Me._FontSize2 = oRequest.GetValueFromParameter(14)
			Me._FontSize3 = oRequest.GetValueFromParameter(15)
			Me._FontColor1 = oRequest.GetValueFromParameter(16)
			Me._FontColor2 = oRequest.GetValueFromParameter(17)
			Me._FontColor3 = oRequest.GetValueFromParameter(18)
			Me._PRSN_id = oRequest.GetValueFromParameter(19)
			Me._Bold1 = oRequest.GetValueFromParameter(20)
			Me._Bold2 = oRequest.GetValueFromParameter(21)
			Me._Bold3 = oRequest.GetValueFromParameter(22)
			Me._Italic1 = oRequest.GetValueFromParameter(23)
			Me._Italic2 = oRequest.GetValueFromParameter(24)
			Me._Italic3 = oRequest.GetValueFromParameter(25)
			Me._isAttivata = oRequest.GetValueFromParameter(26)


			Me._Errore = Errori_Db.None
		Catch ex As Exception
			Me._id = -1
			Me._Errore = Errori_Db.DBReadExist
		End Try

	End Sub

	Public Shared Function GetFromComunita(ByVal ComunitaID As Integer) As COL_Cover
		Dim oRequest As New COL_Request
		Dim oParam As New COL_Request.Parameter
		Dim objAccesso As New COL_DataAccess
		Dim oCover As COL_Cover

		With oRequest
			.Command = "sp_Cover_estraiFromComunita"
			.CommandType = CommandType.StoredProcedure
			oParam = objAccesso.GetAdvancedParameter("@CVER_id", "", ParameterDirection.Output, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_CMNT_ID", ComunitaID, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_DataCreazione", "", ParameterDirection.Output, SqlDbType.DateTime)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_DataModifica", "", ParameterDirection.Output, SqlDbType.DateTime)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Titolo", "", ParameterDirection.Output, SqlDbType.VarChar, , 300)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Anno", "", ParameterDirection.Output, SqlDbType.VarChar, , 150)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Commenti", "", ParameterDirection.Output, SqlDbType.VarChar, , 5000)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Immagine", "", ParameterDirection.Output, SqlDbType.VarChar, , 300)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Didascalia", "", ParameterDirection.Output, SqlDbType.VarChar, , 200)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontFace1", "", ParameterDirection.Output, SqlDbType.VarChar, , 50)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontFace2", "", ParameterDirection.Output, SqlDbType.VarChar, , 50)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontFace3", "", ParameterDirection.Output, SqlDbType.VarChar, , 50)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontSize1", "", ParameterDirection.Output, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontSize2", "", ParameterDirection.Output, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontSize3", "", ParameterDirection.Output, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontColor1", "", ParameterDirection.Output, SqlDbType.VarChar, , 50)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontColor2", "", ParameterDirection.Output, SqlDbType.VarChar, , 50)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_FontColor3", "", ParameterDirection.Output, SqlDbType.VarChar, , 50)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_PRSN_id", "", ParameterDirection.Output, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Bold1", "", ParameterDirection.Output, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Bold2", "", ParameterDirection.Output, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Bold3", "", ParameterDirection.Output, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Italic1", "", ParameterDirection.Output, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Italic2", "", ParameterDirection.Output, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Italic3", "", ParameterDirection.Output, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_isAttivata", "", ParameterDirection.Output, SqlDbType.Int)
			.Parameters.Add(oParam)
			.Role = COL_Request.UserRole.Admin
			.transactional = False
		End With
		Try
			objAccesso.GetExecuteNotQuery(oRequest)
			oCover = New COL_Cover()

			oCover.Id = oRequest.GetValueFromParameter(1)
			If oRequest.GetValueFromParameter(3) <> "" Then
				Try
					If Not Equals(oRequest.GetValueFromParameter(3), New Date) Then
						oCover.DataCreazione = oRequest.GetValueFromParameter(3)
					End If
				Catch ex As Exception

				End Try
			End If
			If oRequest.GetValueFromParameter(4) <> "" Then
				Try
					If Not Equals(oRequest.GetValueFromParameter(3), New Date) Then
						oCover.DataModifica = oRequest.GetValueFromParameter(4)
					End If
				Catch ex As Exception

				End Try
			End If
			oCover.CMNT_ID = ComunitaID
			oCover.Titolo = oRequest.GetValueFromParameter(5)
			oCover.Anno = oRequest.GetValueFromParameter(6)
			oCover.Commenti = oRequest.GetValueFromParameter(7)
			oCover.Immagine = oRequest.GetValueFromParameter(8)
			oCover.Didascalia = oRequest.GetValueFromParameter(9)
			oCover.FontFace1 = oRequest.GetValueFromParameter(10)
			oCover.FontFace2 = oRequest.GetValueFromParameter(11)
			oCover.FontFace3 = oRequest.GetValueFromParameter(12)
			oCover.FontSize1 = oRequest.GetValueFromParameter(13)
			oCover.FontSize2 = oRequest.GetValueFromParameter(14)
			oCover.FontSize3 = oRequest.GetValueFromParameter(15)
			oCover.FontColor1 = oRequest.GetValueFromParameter(16)
			oCover.FontColor2 = oRequest.GetValueFromParameter(17)
			oCover.FontColor3 = oRequest.GetValueFromParameter(18)
			oCover.IDautore = oRequest.GetValueFromParameter(19)
			oCover.Bold1 = oRequest.GetValueFromParameter(20)
			oCover.Bold2 = oRequest.GetValueFromParameter(21)
			oCover.Bold3 = oRequest.GetValueFromParameter(22)
			oCover.Italic1 = oRequest.GetValueFromParameter(23)
			oCover.Italic2 = oRequest.GetValueFromParameter(24)
			oCover.Italic3 = oRequest.GetValueFromParameter(25)
			oCover.isAttivata = oRequest.GetValueFromParameter(26)

		Catch ex As Exception
			oCover = Nothing
		End Try
		Return oCover
	End Function
#End Region

	' INSERIRE CODICE CHE ELIMINA LOGO DAL SERVER (FISICAMENTE) E DAL DB !!!
	Public Sub EliminaLogo(ByVal PercorsoFile As String)
		Dim oRequest As New COL_Request
		Dim oParam As New COL_Request.Parameter
		Dim objAccesso As New COL_DataAccess


		With oRequest
			.Command = "sp_Cover_EliminaLogo"
			.CommandType = CommandType.StoredProcedure
			oParam = objAccesso.GetAdvancedParameter("@CVER_id", Me._id, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)

			.Role = COL_Request.UserRole.Admin
			.transactional = False
		End With
		Try

			objAccesso.GetExecuteNotQuery(oRequest)
			Me._Errore = Errori_Db.None
		Catch ex As Exception
			Me._Errore = Errori_Db.DBReadExist
		End Try
	End Sub

	Public Sub UpDateImmagine(ByVal PercorsoFisico As String, ByVal CoverImage As String)
		Dim oRequest As New COL_Request
		Dim oParam As New COL_Request.Parameter
		Dim objAccesso As New COL_DataAccess
		Dim Estensione As String = ""


		With oRequest
			.Command = "sp_Cover_UpdateImmagine"
			.CommandType = CommandType.StoredProcedure
			oParam = objAccesso.GetAdvancedParameter("@CVER_Id", Me.Id, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@CVER_Immagine", CoverImage, ParameterDirection.Input, SqlDbType.VarChar, True, 300)
			.Parameters.Add(oParam)
			If Me._Immagine <> "" Then
				Estensione = Right(Me._Immagine, Len(Me._Immagine) - Me._Immagine.LastIndexOf("."))

				oParam = objAccesso.GetAdvancedParameter("@OldFileName", PercorsoFisico & Me._Immagine, ParameterDirection.Input, SqlDbType.VarChar, True, 5000)
				.Parameters.Add(oParam)

				oParam = objAccesso.GetAdvancedParameter("@OldFileNameMini", Replace(PercorsoFisico & Me._Immagine, Estensione, "_mini" & Estensione), ParameterDirection.Input, SqlDbType.VarChar, True, 5000)
				.Parameters.Add(oParam)
			End If
		
			.Role = COL_Request.UserRole.Admin
			.transactional = False
		End With
		Try
			If objAccesso.GetExecuteNotQuery(oRequest) = 1 Then
				Me._Immagine = CoverImage
			End If
		Catch ex As Exception
			Me._Errore = Errori_Db.DBReadExist
		End Try
	End Sub

	Public Sub EliminaFileCover(ByVal PercorsoFisico As String)
        Delete.File(PercorsoFisico & Me._Immagine)
    End Sub
	Public Shared Sub EliminaFileCover(ByVal PercorsoFisico As String, ByVal NomeFile As String)
        Delete.File(PercorsoFisico & NomeFile)
	End Sub

	Public Sub SetDefaultPage(ByVal oPage As ServicePage)
		Dim oRequest As New COL_Request
		Dim oParam As New COL_Request.Parameter
		Dim objAccesso As New COL_DataAccess
		Dim iResponse As Boolean = False

		With oRequest
			.Command = "sp_Comunita_SetDefaultPage"
			.CommandType = CommandType.StoredProcedure
			oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", Me._CMNT_ID, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)
			oParam = objAccesso.GetAdvancedParameter("@DFLP_ID", oPage.ID, ParameterDirection.Input, SqlDbType.Int)
			.Parameters.Add(oParam)

			.Role = COL_Request.UserRole.Admin
			.transactional = False
		End With
		Try
			objAccesso.GetExecuteNotQuery(oRequest)
		Catch ex As Exception

		End Try
	End Sub
End Class
