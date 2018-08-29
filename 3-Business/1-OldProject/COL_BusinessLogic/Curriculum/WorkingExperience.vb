Imports COL_DataLayer
Namespace Curriculum_Europeo

    Public Class COL_EsperienzeLavorative

#Region "Private Property"
        Private n_ESLV_id As Integer
        Private n_ESLV_inizio As DateTime
        Private n_ESLV_fine As DateTime
        Private n_ESLV_nomeDatore As String
        Private n_ESLV_settore As String
        Private n_ESLV_tipoImpiego As String
        Private n_ESLV_mansione As String
        Private n_ESLV_CREU_id As Integer
        Private n_ESLV_rendiPubblico As Integer
        Private n_ESLV_esperienzaInCorso As Integer
        Private n_ErroreDB As Errori_Db
#End Region

#Region "Public Property"
        Public Property ID() As Integer
            Get
                ID = n_ESLV_id
            End Get
            Set(ByVal Value As Integer)
                n_ESLV_id = Value
            End Set
        End Property
        Public Property inizio() As DateTime
            Get
                inizio = n_ESLV_inizio
            End Get
            Set(ByVal Value As DateTime)
                n_ESLV_inizio = Value
            End Set
        End Property
        Public Property fine() As DateTime
            Get
                fine = n_ESLV_fine
            End Get
            Set(ByVal Value As DateTime)
                n_ESLV_fine = Value
            End Set
        End Property
        Public Property nomeDatore() As String
            Get
                nomeDatore = n_ESLV_nomeDatore
            End Get
            Set(ByVal Value As String)
                n_ESLV_nomeDatore = Value
            End Set
        End Property
        Public Property settore() As String
            Get
                settore = n_ESLV_settore
            End Get
            Set(ByVal Value As String)
                n_ESLV_settore = Value
            End Set
        End Property

        Public Property tipoImpiego() As String
            Get
                tipoImpiego = n_ESLV_tipoImpiego
            End Get
            Set(ByVal Value As String)
                n_ESLV_tipoImpiego = Value
            End Set
        End Property
        Public Property mansione() As String
            Get
                mansione = n_ESLV_mansione
            End Get
            Set(ByVal Value As String)
                n_ESLV_mansione = Value
            End Set
        End Property
        Public Property Curriculum_id() As String
            Get
                Curriculum_id = n_ESLV_CREU_id
            End Get
            Set(ByVal Value As String)
                n_ESLV_CREU_id = Value
            End Set
        End Property
        Public Property RendiPubblico() As Boolean
            Get
                RendiPubblico = (Me.n_ESLV_rendiPubblico = 1)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    n_ESLV_rendiPubblico = 1
                Else
                    n_ESLV_rendiPubblico = 0
                End If
            End Set
        End Property
        Public Property EsperienzaInCorso() As Boolean
            Get
                EsperienzaInCorso = (Me.n_ESLV_esperienzaInCorso = 1)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    n_ESLV_esperienzaInCorso = 1
                Else
                    n_ESLV_esperienzaInCorso = 0
                End If
            End Set
        End Property
        Public ReadOnly Property Errore() As Errori_Db
            Get
                Errore = n_ErroreDB
            End Get
        End Property
#End Region

        Sub New()
            Me.n_ErroreDB = Errori_Db.None
        End Sub

#Region "Metodi Standard"

        Public Sub Estrai()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Integer = 0
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_EsperienzeLavorative_Estrai"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@ESLV_id", Me.n_ESLV_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ESLV_inizio", "", ParameterDirection.Output, SqlDbType.DateTime, True)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ESLV_fine", "", ParameterDirection.Output, SqlDbType.DateTime, True)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ESLV_nomeDatore", "", ParameterDirection.Output, SqlDbType.VarChar, True, 2000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ESLV_settore", "", ParameterDirection.Output, SqlDbType.VarChar, , 2000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ESLV_tipoImpiego", "", ParameterDirection.Output, SqlDbType.VarChar, , 2000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ESLV_mansione", "", ParameterDirection.Output, SqlDbType.VarChar, , 2000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ESLV_CREU_id", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ESLV_rendiPubblico", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ESLV_esperienzaInCorso", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                iResponse = objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_ESLV_inizio = oRequest.GetValueFromParameter(2)
                Me.n_ESLV_fine = oRequest.GetValueFromParameter(3)
                Me.n_ESLV_nomeDatore = oRequest.GetValueFromParameter(4)
                Me.n_ESLV_settore = oRequest.GetValueFromParameter(5)
                Me.n_ESLV_tipoImpiego = oRequest.GetValueFromParameter(6)
                Me.n_ESLV_mansione = oRequest.GetValueFromParameter(7)
                Me.n_ESLV_CREU_id = oRequest.GetValueFromParameter(8)
                Me.n_ESLV_rendiPubblico = oRequest.GetValueFromParameter(9)
                Me.n_ESLV_esperienzaInCorso = oRequest.GetValueFromParameter(10)
                Me.n_ErroreDB = Errori_Db.None
            Catch ex As Exception
                Me.n_ErroreDB = Errori_Db.DBReadExist

            End Try
        End Sub

        Public Sub EstraiByPersona(ByVal PRSN_ID As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim iResponse As Integer = 0
            With oRequest
                .Command = "sp_EsperienzeLavorative_EstraiByPersona"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@ESLV_id", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ESLV_inizio", "", ParameterDirection.Output, SqlDbType.DateTime, True)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ESLV_fine", "", ParameterDirection.Output, SqlDbType.DateTime, True)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ESLV_nomeDatore", "", ParameterDirection.Output, SqlDbType.VarChar, True, 2000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ESLV_settore", "", ParameterDirection.Output, SqlDbType.VarChar, , 2000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ESLV_tipoImpiego", "", ParameterDirection.Output, SqlDbType.VarChar, , 2000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ESLV_mansione", "", ParameterDirection.Output, SqlDbType.VarChar, , 2000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ESLV_CREU_id", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ESLV_rendiPubblico", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_id", PRSN_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ESLV_esperienzaInCorso", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                iResponse = objAccesso.GetExecuteNotQuery(oRequest)
                If oRequest.GetValueFromParameter(1) <> "" Then
                    Me.n_ESLV_id = oRequest.GetValueFromParameter(1)
                    Me.n_ESLV_inizio = oRequest.GetValueFromParameter(2)
                    Me.n_ESLV_fine = oRequest.GetValueFromParameter(3)
                    Me.n_ESLV_nomeDatore = oRequest.GetValueFromParameter(4)
                    Me.n_ESLV_settore = oRequest.GetValueFromParameter(5)
                    Me.n_ESLV_tipoImpiego = oRequest.GetValueFromParameter(6)
                    Me.n_ESLV_mansione = oRequest.GetValueFromParameter(7)
                    Me.n_ESLV_CREU_id = oRequest.GetValueFromParameter(8)
                    Me.n_ESLV_rendiPubblico = oRequest.GetValueFromParameter(9)
                    Me.n_ESLV_esperienzaInCorso = oRequest.GetValueFromParameter(11)
                    Me.n_ErroreDB = Errori_Db.None
                Else
                    Me.n_ESLV_id = -1
                    iResponse = -1
                    Me.n_ErroreDB = Errori_Db.DBReadExist
                End If

            Catch ex As Exception
                Me.n_ErroreDB = Errori_Db.DBReadExist
            End Try
        End Sub

        Public Sub Aggiungi()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Integer
            Dim objAccesso As New COL_DataAccess
            Dim dataInizio, dataFine As String

            If Equals(Me.n_ESLV_inizio, New Date) Then
                dataInizio = ""
            Else
                dataInizio = DateToString(Me.n_ESLV_inizio)
            End If
            If Equals(Me.n_ESLV_fine, New Date) Then
                dataFine = ""
            Else
                dataFine = DateToString(Me.n_ESLV_fine)
            End If

            With oRequest
                .Command = "sp_EsperienzeLavorative_Aggiungi"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@ESLV_id", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ESLV_inizio", dataInizio, ParameterDirection.Input, SqlDbType.VarChar, , 30)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ESLV_fine", dataFine, ParameterDirection.Input, SqlDbType.VarChar, , 30)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ESLV_nomeDatore", Me.n_ESLV_nomeDatore, ParameterDirection.Input, SqlDbType.VarChar, True, 2000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ESLV_settore", Me.n_ESLV_settore, ParameterDirection.Input, SqlDbType.VarChar, , 2000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ESLV_tipoImpiego", Me.n_ESLV_tipoImpiego, ParameterDirection.Input, SqlDbType.VarChar, , 2000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ESLV_mansione", Me.n_ESLV_mansione, ParameterDirection.Input, SqlDbType.VarChar, , 2000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ESLV_CREU_id", Me.n_ESLV_CREU_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ESLV_rendiPubblico", Me.n_ESLV_rendiPubblico, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ESLV_esperienzaInCorso", Me.n_ESLV_esperienzaInCorso, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                iResponse = objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_ESLV_id = oRequest.GetValueFromParameter(1)
                Me.n_ErroreDB = Errori_Db.None
            Catch ex As Exception
                Me.n_ESLV_id = -1
                Me.n_ErroreDB = Errori_Db.DBInsert
            End Try

        End Sub

        Public Sub Modifica()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Integer
            Dim objAccesso As New COL_DataAccess
            Dim dataInizio, dataFine As String

            If Equals(Me.n_ESLV_inizio, New Date) Then
                dataInizio = ""
            Else
                dataInizio = DateToString(Me.n_ESLV_inizio)
            End If
            If Equals(Me.n_ESLV_fine, New Date) Then
                dataFine = ""
            Else
                dataFine = DateToString(Me.n_ESLV_fine)
            End If
            With oRequest
                .Command = "sp_EsperienzeLavorative_Modifica"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@ESLV_id", Me.n_ESLV_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@ESLV_inizio", Me.n_ESLV_inizio, ParameterDirection.Input, SqlDbType.VarChar, , 30)
                .Parameters.Add(oParam)


                oParam = objAccesso.GetAdvancedParameter("@ESLV_fine", Me.n_ESLV_fine, ParameterDirection.Input, SqlDbType.VarChar, , 30)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@ESLV_nomeDatore", Me.n_ESLV_nomeDatore, ParameterDirection.Input, SqlDbType.VarChar, True, 2000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ESLV_settore", Me.n_ESLV_settore, ParameterDirection.Input, SqlDbType.VarChar, , 2000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ESLV_tipoImpiego", Me.n_ESLV_tipoImpiego, ParameterDirection.Input, SqlDbType.VarChar, , 2000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ESLV_mansione", Me.n_ESLV_mansione, ParameterDirection.Input, SqlDbType.VarChar, , 2000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ESLV_CREU_id", Me.n_ESLV_CREU_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ESLV_rendiPubblico", Me.n_ESLV_rendiPubblico, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ESLV_esperienzaInCorso", Me.n_ESLV_esperienzaInCorso, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                iResponse = objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_ErroreDB = Errori_Db.None
            Catch ex As Exception
                Me.n_ErroreDB = Errori_Db.DBChange
            End Try
        End Sub

        Public Function Elimina(ByVal ESLV_id As Integer) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim responso As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_EsperienzeLavorative_Elimina"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@ESLV_id", ESLV_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)


                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                responso = objAccesso.GetExecuteNotQuery(oRequest)
                Return True
            Catch ex As Exception
                Return False
            End Try
            Return False
        End Function
#End Region

        Public Function ElencaByPersona(ByVal PRSN_id As Integer, Optional ByVal oVisibilita As Main.FiltroVisibilità = Main.FiltroVisibilità.Tutti) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As DataSet = Nothing
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_EsperienzeLavorative_ElencaByPersona"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@PRSN_id", PRSN_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@Visibilita", CType(oVisibilita, Main.FiltroVisibilità), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                dsTable = objAccesso.GetdataSet(oRequest)
                Me.n_ErroreDB = Errori_Db.None
            Catch ex As Exception
                Me.n_ErroreDB = Errori_Db.DBError
            End Try
            Return dsTable
        End Function


        Public Function GetNumeroEsperienze(ByVal CREU_id As Integer) As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim NumEsperienze As Integer

            With oRequest
                .Command = "sp_EsperienzeLavorative_Getnumero"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CREU_id", CREU_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@NumEsperienze", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                NumEsperienze = oRequest.GetValueFromParameter(2)
                Me.n_ErroreDB = Errori_Db.None
            Catch ax As Exception 'record non trovato
                NumEsperienze = 0
                Me.n_ErroreDB = Errori_Db.DBReadExist
            End Try
            Return NumEsperienze
        End Function

    End Class
End Namespace