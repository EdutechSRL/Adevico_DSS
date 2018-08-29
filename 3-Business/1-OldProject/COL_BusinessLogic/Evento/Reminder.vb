Imports COL_DataLayer

Namespace Eventi
    Public Class COL_Reminder
#Region "Private Property"
        Private n_RMND_id As Integer
        Private n_RMND_TPAV_ID As Integer
        Private n_RMND_PRSN_ID As Integer
        Private n_RMND_oggetto As String
        Private n_RMND_testo As String
        Private n_RMND_data_inizio As DateTime
        Private n_RMND_luogo As String
        Private n_RMND_Sospeso As Boolean
        Private n_RMND_data_fine As DateTime
        Private n_RMND_link As String
        Private n_RMND_DBerror As New Errori_Db
        Private n_RMND_data_creazione As New DateTime
#End Region
#Region "Public Property"
        Public Property Id() As Integer
            Get
                Id = n_RMND_id
            End Get
            Set(ByVal Value As Integer)
                n_RMND_id = Value
            End Set
        End Property
        Public Property idTipoAvviso() As Integer
            Get
                idTipoAvviso = n_RMND_TPAV_ID
            End Get
            Set(ByVal Value As Integer)
                n_RMND_TPAV_ID = Value
            End Set
        End Property
        Public Property idPersona() As Integer
            Get
                idPersona = n_RMND_PRSN_ID
            End Get
            Set(ByVal Value As Integer)
                n_RMND_PRSN_ID = Value
            End Set
        End Property
        Public Property Oggetto() As String
            Get
                Oggetto = n_RMND_oggetto
            End Get
            Set(ByVal Value As String)
                n_RMND_oggetto = Value
            End Set
        End Property
        Public Property Testo() As String
            Get
                Testo = n_RMND_testo
            End Get
            Set(ByVal Value As String)
                n_RMND_testo = Value
            End Set
        End Property
        Public Property Inizio() As DateTime
            Get
                Inizio = n_RMND_data_inizio
            End Get
            Set(ByVal Value As DateTime)
                n_RMND_data_inizio = Value
            End Set
        End Property
        Public Property Luogo() As String
            Get
                Luogo = n_RMND_luogo
            End Get
            Set(ByVal Value As String)
                n_RMND_luogo = Value
            End Set
        End Property
        Public Property Sospeso() As Integer
            Get
                Sospeso = n_RMND_Sospeso
            End Get
            Set(ByVal Value As Integer)
                n_RMND_Sospeso = Value
            End Set
        End Property
        Public Property Fine() As DateTime
            Get
                Fine = n_RMND_data_fine
            End Get
            Set(ByVal Value As DateTime)
                n_RMND_data_fine = Value
            End Set
        End Property
        Public Property Link() As String
            Get
                Link = n_RMND_link
            End Get
            Set(ByVal Value As String)
                n_RMND_link = Value
            End Set
        End Property
        Public Property Creazione() As DateTime
            Get
                Creazione = n_RMND_data_creazione
            End Get
            Set(ByVal Value As DateTime)
                n_RMND_data_creazione = Value
            End Set
        End Property
        Public ReadOnly Property Errore() As Errori_Db
            Get
                Errore = n_RMND_DBerror
            End Get
        End Property
#End Region

#Region "Metodi Standard"

        Public Sub Modifica()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim DataInizio As String = DateToString(Me.Inizio)
            Dim DataFine As String = DateToString(Me.Fine)
            Dim DataCreazione As String = DateToString(Now, True)
            With oRequest
                .Command = "sp_Reminder_Modifica"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@RMND_id", n_RMND_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPAV_id", n_RMND_TPAV_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_id", n_RMND_PRSN_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Oggetto", n_RMND_oggetto, ParameterDirection.Input, DbType.String, , 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Testo", n_RMND_testo, ParameterDirection.Input, DbType.String, , 4000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@DataInizio", DataInizio, ParameterDirection.Input, DbType.String, , 30)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Luogo", n_RMND_luogo, ParameterDirection.Input, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Sospeso", n_RMND_Sospeso, ParameterDirection.Input, DbType.Boolean)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@DataFine", DataFine, ParameterDirection.Input, DbType.String, , 30)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Link", n_RMND_link, ParameterDirection.Input, DbType.String, , 2500)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@DataCreazione", DataCreazione, ParameterDirection.Input, DbType.String, , 30)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch ex As Exception
                Me.n_RMND_DBerror = Errori_Db.DBReadExist
            End Try
        End Sub

        Public Sub ModificaTutti() 'no testata   NON CI SONO RIPETIZIONE PER I REMINDER
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim DataInizio As String = DateToString(Me.Inizio)
            Dim DataFine As String = DateToString(Me.Fine)
            Dim DataCreazione As String = DateToString(Me.Creazione)
            With oRequest
                .Command = "sp_Reminder_ModificaTutti"  'da fare
                .CommandType = CommandType.StoredProcedure
                'oParam = objAccesso.GetParameter("@RMND_id", n_RMND_id, ParameterDirection.Input, DbType.Int32)
                '.Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPAV_id", n_RMND_TPAV_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_id", n_RMND_PRSN_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Oggetto", n_RMND_oggetto, ParameterDirection.Input, DbType.String, , 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Testo", n_RMND_testo, ParameterDirection.Input, DbType.String, , 4000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@DataInizio", DataInizio, ParameterDirection.Input, DbType.String, , 30)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Luogo", n_RMND_luogo, ParameterDirection.Input, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Sospeso", n_RMND_Sospeso, ParameterDirection.Input, DbType.Boolean)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@DataFine", DataFine, ParameterDirection.Input, DbType.String, , 30)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Link", n_RMND_link, ParameterDirection.Input, DbType.String, , 2500)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@DataCreazione", DataCreazione, ParameterDirection.Input, DbType.String, , 30)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch ex As Exception
                Me.n_RMND_DBerror = Errori_Db.DBReadExist
            End Try
        End Sub
        Public Function Get_Tipo_Avviso(ByVal LNGU_id As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim objAccesso As New COL_DataAccess
            Dim dsTable As New DataSet
            Dim oParam As New COL_Request.Parameter
            With oRequest
                .Command = "sp_Reminder_ElencaTipoAvviso"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@LNGU_id", LNGU_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                dsTable = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception
                Me.n_RMND_DBerror = Errori_Db.DBError
            End Try
            Return dsTable
        End Function

        Public Sub Aggiungi(ByVal ORRI_id As Int32, ByVal EVNT_id As Int32, ByVal Copia As Int32)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim DataInizio As String = DateToString(Me.Inizio)
            Dim DataFine As String = DateToString(Me.Fine)
            Dim DataCreazione As String = DateToString(Me.Creazione)
            With oRequest
                .Command = "sp_Reminder_Aggiungi"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@ORRI_id", ORRI_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_id", EVNT_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Copia", Copia, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPAV_id", n_RMND_TPAV_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_id", n_RMND_PRSN_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Oggetto", n_RMND_oggetto, ParameterDirection.Input, DbType.String, , 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Testo", n_RMND_testo, ParameterDirection.Input, DbType.String, , 4000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@DataInizio", DataInizio, ParameterDirection.Input, DbType.String, , 30)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Luogo", n_RMND_luogo, ParameterDirection.Input, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Sospeso", n_RMND_Sospeso, ParameterDirection.Input, DbType.Boolean)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@DataFine", DataFine, ParameterDirection.Input, DbType.String, , 30)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Link", n_RMND_link, ParameterDirection.Input, DbType.String, , 2500)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@DataCreazione", DataCreazione, ParameterDirection.Input, DbType.String, , 30)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_RMND_DBerror = Errori_Db.None
            Catch ex As Exception
                Me.n_RMND_DBerror = Errori_Db.DBInsert
            End Try
        End Sub
        Public Sub Cancella()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Reminder_Cancella"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@RMND_id", n_RMND_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch ex As Exception
                Me.n_RMND_DBerror = Errori_Db.DBDelete
            End Try
        End Sub
        Public Sub CancellaTutti() 'no testata   NON CI SONO RIPETIZIONE PER I REMINDER
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim DataCreazione As String = DateToString(Me.Creazione)
            With oRequest
                .Command = "sp_Reminder_CancellaTutti"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@RMND_id", n_RMND_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Oggetto", n_RMND_oggetto, ParameterDirection.Input, DbType.String, , 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@DataCreazione", DataCreazione, ParameterDirection.Input, DbType.String, , 30)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch ex As Exception
                Me.n_RMND_DBerror = Errori_Db.DBDelete
            End Try
        End Sub
		Public Function Estrai(ByVal data1 As DateTime, ByVal data2 As DateTime, ByVal Tipo_Ricerca As Int32) As DataSet
			Dim oRequest As New COL_Request
			Dim oParam As New COL_Request.Parameter
			Dim objAccesso As New COL_DataAccess
			Dim dsTable As New DataSet
			With oRequest
				.Command = "sp_Reminder_Estrai"
				.CommandType = CommandType.StoredProcedure
				oParam = objAccesso.GetParameter("@data1", DateToString(data1, False), ParameterDirection.Input, DbType.String, , 50)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@data2", DateToString(data2, False), ParameterDirection.Input, DbType.String, , 50)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@Tipo_Ricerca", Tipo_Ricerca, ParameterDirection.Input, DbType.Int32)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@PRSN_id", Me.n_RMND_PRSN_ID, ParameterDirection.Input, DbType.Int32)
				.Parameters.Add(oParam)
				.Role = COL_Request.UserRole.Admin
				.transactional = False
			End With
			Try
				dsTable = objAccesso.GetdataSet(oRequest)
			Catch ex As Exception
				Me.n_RMND_DBerror = Errori_Db.DBError
			End Try
			Return dsTable
		End Function
		Public Function TrovaEventi(ByVal data1 As DateTime, ByVal data2 As DateTime, ByVal Tipo_Ricerca As Int32) As DataSet
			Dim oRequest As New COL_Request
			Dim oParam As New COL_Request.Parameter
			Dim objAccesso As New COL_DataAccess
			Dim dsTable As New DataSet
			With oRequest
				.Command = "sp_Reminder_Ricerca"
				.CommandType = CommandType.StoredProcedure
				oParam = objAccesso.GetParameter("@data1", DateToString(data1, False), ParameterDirection.Input, DbType.String, , 50)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@data2", DateToString(data2, False), ParameterDirection.Input, DbType.String, , 50)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@Tipo_Ricerca", Tipo_Ricerca, ParameterDirection.Input, DbType.Int32)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@PRSN_id", Me.n_RMND_PRSN_ID, ParameterDirection.Input, DbType.Int32)
				.Parameters.Add(oParam)
				.Role = COL_Request.UserRole.Admin
				.transactional = False
			End With
			Try
				dsTable = objAccesso.GetdataSet(oRequest)
			Catch ex As Exception
				Me.n_RMND_DBerror = Errori_Db.DBError
			End Try
			Return dsTable
		End Function

        Public Sub Estrai_da_id()
            ' Mario
            ' carica gli attributi dell'oggetto dal db

            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Reminder_Estrai_da_id"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@RMND_id", n_RMND_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RMND_TPAV_ID", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RMND_PRSN_ID", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RMND_oggetto", "", ParameterDirection.Output, DbType.String, , 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RMND_testo", "", ParameterDirection.Output, DbType.String, , 4000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RMND_data_inizio", "", ParameterDirection.Output, DbType.Date)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RMND_data_fine", "", ParameterDirection.Output, DbType.Date)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RMND_luogo", "", ParameterDirection.Output, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RMND_data_creazione", "", ParameterDirection.Output, DbType.Date)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RMND_Sospeso", "", ParameterDirection.Output, DbType.Byte)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RMND_link", "", ParameterDirection.Output, DbType.String, , 2500)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                n_RMND_TPAV_ID = oRequest.GetValueFromParameter(2)
                n_RMND_PRSN_ID = oRequest.GetValueFromParameter(3)
                n_RMND_oggetto = oRequest.GetValueFromParameter(4)
                n_RMND_testo = oRequest.GetValueFromParameter(5)
                n_RMND_data_inizio = oRequest.GetValueFromParameter(6)
                n_RMND_data_fine = oRequest.GetValueFromParameter(7)
                n_RMND_luogo = oRequest.GetValueFromParameter(8)
                n_RMND_data_creazione = oRequest.GetValueFromParameter(9)
                n_RMND_Sospeso = oRequest.GetValueFromParameter(10)
                n_RMND_link = oRequest.GetValueFromParameter(11)
            Catch ex As Exception
                n_RMND_DBerror = Errori_Db.DBReadExist
            End Try

        End Sub

        Public Function getPersoneDaAvvisare(ByVal data As String, ByVal Tipo_Avviso_id As Int32) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim dsTable As New DataSet
            With oRequest
                .Command = "sp_Reminder_Persone_daAvvisare"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@giorno", data, ParameterDirection.Input, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPAV_id", Tipo_Avviso_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                dsTable = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception
                Me.n_RMND_DBerror = Errori_Db.DBError
            End Try
            Return dsTable
        End Function

        Public Function getAvvisiPerPersona(ByVal data As String, ByVal Tipo_Avviso_id As Int32, ByVal Persona_id As Int32) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim dsTable As New DataSet
            With oRequest
                .Command = "sp_Reminder_Avvisi_perPersona"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@giorno", data, ParameterDirection.Input, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPAV_id", Tipo_Avviso_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_id", Persona_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                dsTable = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception
                Me.n_RMND_DBerror = Errori_Db.DBError
            End Try
            Return dsTable
        End Function

#End Region
    End Class
End Namespace