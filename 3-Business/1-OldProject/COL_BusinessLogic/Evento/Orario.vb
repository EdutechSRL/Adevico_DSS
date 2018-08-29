Imports COL_DataLayer

Namespace Eventi
    Public Class COL_Orario

#Region "Private Property"
		Private _ID As Integer
		Private _Inizio As DateTime
		Private _Fine As DateTime
		Private _Luogo As String
		Private _Evento As New COL_Evento
		Private _Error As New Errori_Db
		Private _IsVisibile As Integer
		Private _DataModifica As DateTime
		Private _CreatoreID As Integer
		Private _Referente As String
		Private _Link As String
		Private _Note As String
		Private _Title As String
#End Region

#Region "Public Property"
		Public Property Id() As Integer
			Get
				Id = _ID
			End Get
			Set(ByVal Value As Integer)
				_ID = Value
			End Set
		End Property
		Public Property Evento() As COL_Evento
			Get
				Evento = _Evento
			End Get
			Set(ByVal Value As COL_Evento)
				_Evento = Value
			End Set
		End Property
		Public Property Inizio() As DateTime
			Get
				Inizio = _Inizio
			End Get
			Set(ByVal Value As DateTime)
				_Inizio = Value
			End Set
		End Property
		Public Property Fine() As DateTime
			Get
				Fine = _Fine
			End Get
			Set(ByVal Value As DateTime)
				_Fine = Value
			End Set
		End Property
		Public Property Title() As String
			Get
				Title = _Title
			End Get
			Set(ByVal Value As String)
				_Title = Value
			End Set
		End Property
		Public Property Luogo() As String
			Get
				Luogo = _Luogo
			End Get
			Set(ByVal Value As String)
				_Luogo = Value
			End Set
		End Property
		Public ReadOnly Property Referente() As String
			Get
				Referente = _Referente
			End Get
		End Property
		Public Property Visibile() As Boolean
			Get
				Visibile = (_IsVisibile = 1)
			End Get

			Set(ByVal Value As Boolean)
				If Value Then
					_IsVisibile = 1
				Else
					_IsVisibile = 0
				End If
			End Set
		End Property
		Public ReadOnly Property Errore() As Errori_Db
			Get
				Errore = _Error
			End Get
		End Property
		Public Property DataModifica() As DateTime
			Get
				DataModifica = _DataModifica
			End Get
			Set(ByVal Value As DateTime)
				_DataModifica = Value
			End Set
		End Property
		Public Property idPersona() As Integer
			Get
				idPersona = _CreatoreID
			End Get
			Set(ByVal Value As Integer)
				_CreatoreID = Value
			End Set
		End Property
		Public Property Link() As String
			Get
				Link = _Link
			End Get
			Set(ByVal Value As String)
				_Link = Value
			End Set
		End Property
		Public Property Note() As String
			Get
				Note = _Note
			End Get
			Set(ByVal Value As String)
				_Note = Value
			End Set
		End Property
#End Region

#Region "Metodi Standard"
		Public Sub Modifica()
			Dim oRequest As New COL_Request
			Dim oParam As New COL_Request.Parameter
			Dim objAccesso As New COL_DataAccess
			Dim DataModifica As String = DateToString(Now, True)
			Dim DataInizio As String = DateToString(Me.Inizio)
			Dim DataFine As String = DateToString(Me.Fine)
			With oRequest
				.Command = "sp_Orario_Modifica"
				.CommandType = CommandType.StoredProcedure
				oParam = objAccesso.GetAdvancedParameter("@ORRI_id", _ID, ParameterDirection.Input, SqlDbType.BigInt)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetAdvancedParameter("@ORRI_inizio", DataInizio, ParameterDirection.Input, SqlDbType.VarChar)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetAdvancedParameter("@ORRI_fine", DataFine, ParameterDirection.Input, SqlDbType.VarChar)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetAdvancedParameter("@ORRI_aula", _Luogo, ParameterDirection.Input, SqlDbType.VarChar, True, 200)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetAdvancedParameter("@ORRI_visibile", _IsVisibile, ParameterDirection.Input, SqlDbType.Int)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetAdvancedParameter("@ORRI_dataModifica", DataModifica, ParameterDirection.Input, SqlDbType.VarChar, , 30)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetAdvancedParameter("@ORRI_PRSN_ID", _CreatoreID, ParameterDirection.Input, SqlDbType.Int)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetAdvancedParameter("@ORRI_link", _Link, ParameterDirection.Input, SqlDbType.VarChar, True, 2500)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetAdvancedParameter("@ORRI_note", _Note, ParameterDirection.Input, SqlDbType.VarChar, True, 4000)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@Title", Me._Title, ParameterDirection.Input, DbType.String, True, 500)
				.Parameters.Add(oParam)
				.Role = COL_Request.UserRole.Admin
				.transactional = False
			End With
			Try
				objAccesso.GetExecuteNotQuery(oRequest)
			Catch
				Me._Error = Errori_Db.DBReadExist
			End Try
		End Sub

        Public Sub ModificaTutti()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim DataInizio As String = DateToString(Me.Inizio)
            Dim DataFine As String = DateToString(Me.Fine)
            Dim DataModifica As String = DateToString(Now, True)
            With oRequest
                .Command = "sp_Orario_ModificaTutti"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@ORRI_EVNT_id", Me._Evento.Id, ParameterDirection.Input, SqlDbType.BigInt)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORRI_inizio", DataInizio, ParameterDirection.Input, SqlDbType.VarChar)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORRI_fine", DataFine, ParameterDirection.Input, SqlDbType.VarChar)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORRI_aula", _Luogo, ParameterDirection.Input, SqlDbType.VarChar, True, 200)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORRI_visibile", _IsVisibile, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORRI_dataModifica", DataModifica, ParameterDirection.Input, SqlDbType.VarChar, , 30)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORRI_PRSN_ID", _CreatoreID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORRI_link", _Link, ParameterDirection.Input, SqlDbType.VarChar, True, 2500)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORRI_note", _Note, ParameterDirection.Input, SqlDbType.VarChar, True, 4000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Title", Me._Title, ParameterDirection.Input, DbType.String, True, 500)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch
                Me._Error = Errori_Db.DBReadExist
            End Try
        End Sub
		Public Sub Estrai()
			'carica i campi del db nell'oggetto in base all'id dell'orario
			Dim oRequest As New COL_Request
			Dim oParam As New COL_Request.Parameter
			Dim objAccesso As New COL_DataAccess
			With oRequest
				.Command = "sp_Orario_Estrai"
				.CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@ORRI_id", Me._ID, ParameterDirection.Input, DbType.Int64)
				.Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ORRI_EVNT_id", "", ParameterDirection.Output, DbType.Int64)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@ORRI_inizio", "", ParameterDirection.Output, DbType.DateTime)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@ORRI_fine", "", ParameterDirection.Output, DbType.DateTime)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@ORRI_aula", "", ParameterDirection.Output, DbType.String, True, 200)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@ORRI_visibile", "", ParameterDirection.Output, DbType.Int32)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@ORRI_dataModifica", "", ParameterDirection.Output, DbType.DateTime)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@ORRI_PRSN_ID", "", ParameterDirection.Output, DbType.Int32)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@ORRI_Referente", "", ParameterDirection.Output, DbType.String, False, 100)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@ORRI_link", "", ParameterDirection.Output, DbType.String, True, 2500)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@ORRI_note", "", ParameterDirection.Output, DbType.String, True, 4000)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@ORRI_Title", "", ParameterDirection.Output, DbType.String, True, 500)
				.Parameters.Add(oParam)
				.Role = COL_Request.UserRole.Admin
				.transactional = False
			End With

			Try
				objAccesso.GetExecuteNotQuery(oRequest)
				Me._Evento.Id = oRequest.GetValueFromParameter(2)
				Me._Inizio = oRequest.GetValueFromParameter(3)
				Me._Fine = oRequest.GetValueFromParameter(4)
				Me._Luogo = oRequest.GetValueFromParameter(5)
				Me._IsVisibile = oRequest.GetValueFromParameter(6)
				Me._DataModifica = oRequest.GetValueFromParameter(7)
				Me._CreatoreID = oRequest.GetValueFromParameter(8)
				Me._Referente = oRequest.GetValueFromParameter(9)
				Me._Link = oRequest.GetValueFromParameter(10)
				Me._Note = oRequest.GetValueFromParameter(11)
				Me._Title = oRequest.GetValueFromParameter(12)

			Catch ax As Exception
				Me._Error = Errori_Db.DBReadExist
			End Try
		End Sub
		Public Sub Aggiungi()
			Dim oRequest As New COL_Request
			Dim oParam As New COL_Request.Parameter
			Dim objAccesso As New COL_DataAccess
			Dim DataModifica As String = DateToString(Now, True)
			Dim DataInizio As String = DateToString(Me.Inizio)
			Dim DataFine As String = DateToString(Me.Fine)
			With oRequest
				.Command = "sp_Orario_Aggiungi"
				.CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@ORRI_id", _ID, ParameterDirection.Output, DbType.Int64)
				.Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ORRI_EVNT_id", _Evento.Id, ParameterDirection.Input, DbType.Int64)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@ORRI_inizio", DataInizio, ParameterDirection.Input, DbType.String, , 30)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@ORRI_fine", DataFine, ParameterDirection.Input, DbType.String, , 30)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@ORRI_aula", _Luogo, ParameterDirection.Input, DbType.String, True, 200)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@ORRI_visibile", _IsVisibile, ParameterDirection.Input, DbType.Int32)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@ORRI_dataModifica", DataModifica, ParameterDirection.Input, DbType.String, , 30)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@ORRI_PRSN_ID", _CreatoreID, , DbType.Int32)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@ORRI_link", _Link, ParameterDirection.Input, DbType.String, True, 2500)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@ORRI_note", _Note, ParameterDirection.Input, DbType.String, True, 4000)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@Title", Me._Title, ParameterDirection.Input, DbType.String, True, 500)
				.Parameters.Add(oParam)


				.Role = COL_Request.UserRole.Admin
				.transactional = False
			End With
			Try
				objAccesso.GetExecuteNotQuery(oRequest)
				Me._ID = oRequest.GetValueFromParameter(1)
			Catch ex As Exception
				Me._Error = Errori_Db.DBInsert
			End Try
		End Sub
		Public Sub Cancella() 'testata ok
			Dim oRequest As New COL_Request
			Dim oParam As New COL_Request.Parameter
			Dim objAccesso As New COL_DataAccess
			With oRequest
				.Command = "sp_Orario_Elimina"
				.CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@ORRI_id", _ID, ParameterDirection.Input, DbType.Int64)
				.Parameters.Add(oParam)
				.Role = COL_Request.UserRole.Admin
				.transactional = False
			End With
			Try
				objAccesso.GetExecuteNotQuery(oRequest)
			Catch
				Me._Error = Errori_Db.DBDelete
			End Try
		End Sub
        'Public Sub AssociaMateriale(ByVal FLDS_id As Integer)
        '	'associa ad un orario un file
        '	Dim oRequest As New COL_Request
        '	Dim oParam As New COL_Request.Parameter
        '	Dim objAccesso As New COL_DataAccess
        '	With oRequest
        '		.Command = "sp_Orario_AssociaMateriale"
        '		.CommandType = CommandType.StoredProcedure
        '		oParam = objAccesso.GetParameter("@ORRI_id", _ID, ParameterDirection.Input, DbType.Int32)
        '		.Parameters.Add(oParam)
        '		oParam = objAccesso.GetParameter("@FLDS_id", FLDS_id, ParameterDirection.Input, DbType.Int32)
        '		.Parameters.Add(oParam)
        '		.Role = COL_Request.UserRole.Admin
        '		.transactional = False
        '	End With
        '	Try
        '		objAccesso.GetExecuteNotQuery(oRequest)
        '	Catch ex As Exception
        '		Me._Error = Errori_Db.DBInsert
        '	End Try
        'End Sub
        'Public Sub DisassociaMateriale(ByVal FLDS_id As Integer)
        '	'disassocia un file dall'orario al quale era stato assegnato
        '	Dim oRequest As New COL_Request
        '	Dim oParam As New COL_Request.Parameter
        '	Dim objAccesso As New COL_DataAccess
        '	With oRequest
        '		.Command = "sp_Orario_DisassociaMateriale"
        '		.CommandType = CommandType.StoredProcedure
        '		oParam = objAccesso.GetParameter("@ORRI_id", _ID, ParameterDirection.Input, DbType.Int32)
        '		.Parameters.Add(oParam)
        '		oParam = objAccesso.GetParameter("@FLDS_id", FLDS_id, ParameterDirection.Input, DbType.Int32)
        '		.Parameters.Add(oParam)
        '		.Role = COL_Request.UserRole.Admin
        '		.transactional = False
        '	End With
        '	Try
        '		objAccesso.GetExecuteNotQuery(oRequest)
        '	Catch ex As Exception
        '		Me._Error = Errori_Db.DBDelete
        '	End Try
        'End Sub
        'Public Function GetMateriale(ByVal PersonaID As Integer, ByVal oVisibile As Main.FiltroVisibilità, ByVal IdCategoria As Integer) As DataSet
        '	'restituisce l'elenco del materiale associato ad un orario
        '	Dim oRequest As New COL_Request
        '	Dim oParam As New COL_Request.Parameter
        '	Dim dsTable As New DataSet
        '	Dim objAccesso As New COL_DataAccess

        '	With oRequest
        '		.Command = "sp_Orario_ElencaMateriale"
        '		.CommandType = CommandType.StoredProcedure
        '		oParam = objAccesso.GetAdvancedParameter("@ORRI_id", Me._ID, ParameterDirection.Input, SqlDbType.Int)
        '		.Parameters.Add(oParam)
        '		oParam = objAccesso.GetAdvancedParameter("@CTGR_id", IdCategoria, ParameterDirection.Input, SqlDbType.Int)
        '		.Parameters.Add(oParam)
        '		oParam = objAccesso.GetAdvancedParameter("@PRSN_ID", PersonaID, ParameterDirection.Input, SqlDbType.Int)
        '		.Parameters.Add(oParam)
        '		oParam = objAccesso.GetAdvancedParameter("@visibile", CType(oVisibile, Main.FiltroVisibilità), ParameterDirection.Input, SqlDbType.Int)
        '		.Parameters.Add(oParam)

        '		.Role = COL_Request.UserRole.Admin
        '		.transactional = False
        '	End With
        '	Try
        '		dsTable = objAccesso.GetdataSet(oRequest)
        '		Me._Error = Errori_Db.None
        '	Catch ex As Exception
        '		Me._Error = Errori_Db.DBError
        '	End Try
        '	Return dsTable
        'End Function

		Public Function getAnni_minMAX() As Integer()

			'Trova l'anno min e max degli gli eventi inseriti

			Dim oRequest As New COL_Request
			Dim oParam As New COL_Request.Parameter
			Dim objAccesso As New COL_DataAccess
			Dim AnnoMinMax(1) As Integer
			With oRequest
				.Command = "sp_Orario_Anni_estraiMinMax"
				.CommandType = CommandType.StoredProcedure
				oParam = objAccesso.GetParameter("@ORRI_inizioMin", "", ParameterDirection.Output, DbType.Date)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@ORRI_fineMax", "", ParameterDirection.Output, DbType.Date)
				.Parameters.Add(oParam)
				.Role = COL_Request.UserRole.Admin
				.transactional = False
			End With
			Try
				objAccesso.GetExecuteNotQuery(oRequest)
				AnnoMinMax(0) = CDate(oRequest.GetValueFromParameter(1)).Year
				AnnoMinMax(1) = CDate(oRequest.GetValueFromParameter(2)).Year
			Catch ex As Exception
				Me._Error = Errori_Db.DBReadExist
			End Try
			Return AnnoMinMax
		End Function

		Public Function HasMaterialeAssociato() As Boolean
			'restituisce l'elenco del materiale associato ad un orario
			Dim oRequest As New COL_Request
			Dim oParam As New COL_Request.Parameter
			Dim iResponse As Boolean = False
			Dim objAccesso As New COL_DataAccess

			With oRequest
				.Command = "sp_Orario_HasMaterialeAssociato"
				.CommandType = CommandType.StoredProcedure
				oParam = objAccesso.GetAdvancedParameter("@OrarioID", Me._ID, ParameterDirection.Input, SqlDbType.Int)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetAdvancedParameter("@numMateriale", 0, ParameterDirection.Output, SqlDbType.Int)
				.Parameters.Add(oParam)
				.Role = COL_Request.UserRole.Admin
				.transactional = False
			End With
			Try
				objAccesso.GetExecuteNotQuery(oRequest)
				iResponse = oRequest.GetValueFromParameter(2)
				Me._Error = Errori_Db.None
			Catch ex As Exception
				Me._Error = Errori_Db.DBError
			End Try
			Return iResponse
		End Function
#End Region

		'Public Shared Function RecuperaLezioniForAdmin() As DataSet
		'    Dim oRequest As New COL_Request
		'    Dim oParam As New COL_Request.Parameter
		'    Dim objAccesso As New COL_DataAccess
		'    Dim oDataset As New DataSet

		'    With oRequest
		'        .Command = "sp_Orario_RecuperaLezioniForAdmin"
		'        .CommandType = CommandType.StoredProcedure

		'        .Role = COL_Request.UserRole.Admin
		'        .transactional = False
		'    End With

		'    Try
		'        oDataset = objAccesso.GetdataSet(oRequest)
		'    Catch ax As Exception
		'    End Try
		'    Return oDataset
		'End Function
    End Class
End Namespace