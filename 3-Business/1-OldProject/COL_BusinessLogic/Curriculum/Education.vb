Imports COL_DataLayer

Namespace Curriculum_Europeo
    Public Class COL_IstruzioneFormazione
#Region "Private Property"
        Private n_ISFR_id As Integer
        Private n_ISFR_CREU_id As Integer
        Private n_ISFR_inizio As Integer
        Private n_ISFR_fine As Integer
        Private n_ISFR_nomeeTipoIstituto As String
        Private n_ISFR_principaliMaterieAbilita As String
        Private n_ISFR_qualificaConseguita As String
        Private n_ISFR_livelloClassificazioneNazionale As String
        Private n_ISFR_rendiPubblico As Integer
        Private n_ISFR_esperienzaInCorso As Integer
        Private n_ErroreDB As Errori_Db
#End Region

#Region "Public Property"
        Public Property ID() As Integer
            Get
                ID = n_ISFR_id
            End Get
            Set(ByVal Value As Integer)
                n_ISFR_id = Value
            End Set
        End Property
        Public Property Curriculum_id() As String
            Get
                Curriculum_id = n_ISFR_CREU_id
            End Get
            Set(ByVal Value As String)
                n_ISFR_CREU_id = Value
            End Set
        End Property
        Public Property inizio() As Integer
            Get
                inizio = n_ISFR_inizio
            End Get
            Set(ByVal Value As Integer)
                n_ISFR_inizio = Value
            End Set
        End Property
        Public Property fine() As Integer
            Get
                fine = n_ISFR_fine
            End Get
            Set(ByVal Value As Integer)
                n_ISFR_fine = Value
            End Set
        End Property
        Public Property nomeeTipoIstituto() As String
            Get
                nomeeTipoIstituto = n_ISFR_nomeeTipoIstituto
            End Get
            Set(ByVal Value As String)
                n_ISFR_nomeeTipoIstituto = Value
            End Set
        End Property
        Public Property principaliMaterieAbilita() As String
            Get
                principaliMaterieAbilita = n_ISFR_principaliMaterieAbilita
            End Get
            Set(ByVal Value As String)
                n_ISFR_principaliMaterieAbilita = Value
            End Set
        End Property
        Public Property qualificaConseguita() As String
            Get
                qualificaConseguita = n_ISFR_qualificaConseguita
            End Get
            Set(ByVal Value As String)
                n_ISFR_qualificaConseguita = Value
            End Set
        End Property
        Public Property livelloClassificazioneNazionale() As String
            Get
                livelloClassificazioneNazionale = n_ISFR_livelloClassificazioneNazionale
            End Get
            Set(ByVal Value As String)
                n_ISFR_livelloClassificazioneNazionale = Value
            End Set
        End Property
        Public Property RendiPubblico() As Boolean
            Get
                RendiPubblico = (Me.n_ISFR_rendiPubblico = 1)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    n_ISFR_rendiPubblico = 1
                Else
                    n_ISFR_rendiPubblico = 0
                End If
            End Set
        End Property
        Public Property EsperienzaInCorso() As Boolean
            Get
                EsperienzaInCorso = (Me.n_ISFR_esperienzaInCorso = 1)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    n_ISFR_esperienzaInCorso = 1
                Else
                    n_ISFR_esperienzaInCorso = 0
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
            Dim responso As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_IstruzioneFormazione_Estrai"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@ISFR_id", Me.n_ISFR_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ISFR_CREU_id", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ISFR_inizio", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ISFR_fine", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ISFR_nomeeTipoIstituto", "", ParameterDirection.Output, SqlDbType.VarChar, True, 1000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ISFR_principaliMaterieAbilita", "", ParameterDirection.Output, SqlDbType.VarChar, , 1000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ISFR_qualificaConseguita", "", ParameterDirection.Output, SqlDbType.VarChar, , 1000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ISFR_livelloClassificazioneNazionale", "", ParameterDirection.Output, SqlDbType.VarChar, , 1000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ISFR_rendiPubblico", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ISFR_esperienzaInCorso", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                responso = objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_ISFR_CREU_id = oRequest.GetValueFromParameter(2)
                Me.n_ISFR_inizio = oRequest.GetValueFromParameter(3)
                Me.n_ISFR_fine = oRequest.GetValueFromParameter(4)
                Me.n_ISFR_nomeeTipoIstituto = oRequest.GetValueFromParameter(5)
                Me.n_ISFR_principaliMaterieAbilita = oRequest.GetValueFromParameter(6)
                Me.n_ISFR_qualificaConseguita = oRequest.GetValueFromParameter(7)
                Me.n_ISFR_livelloClassificazioneNazionale = oRequest.GetValueFromParameter(8)
                Me.n_ISFR_rendiPubblico = oRequest.GetValueFromParameter(9)
                Me.n_ISFR_esperienzaInCorso = oRequest.GetValueFromParameter(10)
                Me.n_ErroreDB = Errori_Db.None
            Catch ex As Exception
                Me.n_ErroreDB = Errori_Db.DBReadExist
                responso = -1
            End Try
        End Sub
        Public Sub EstraiByPersona(ByVal PRSN_ID As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Integer = 0
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_IstruzioneFormazione_EstraiByPersona"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@ISFR_id", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ISFR_CREU_id", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ISFR_inizio", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ISFR_fine", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ISFR_nomeeTipoIstituto", "", ParameterDirection.Output, SqlDbType.VarChar, True, 1000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ISFR_principaliMaterieAbilita", "", ParameterDirection.Output, SqlDbType.VarChar, , 1000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ISFR_qualificaConseguita", "", ParameterDirection.Output, SqlDbType.VarChar, , 1000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ISFR_livelloClassificazioneNazionale", "", ParameterDirection.Output, SqlDbType.VarChar, , 1000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ISFR_rendiPubblico", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_id", PRSN_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ISFR_esperienzaInCorso", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                iResponse = objAccesso.GetExecuteNotQuery(oRequest)
                If oRequest.GetValueFromParameter(1) <> "" Then
                    Me.n_ISFR_id = oRequest.GetValueFromParameter(1)
                    Me.n_ISFR_CREU_id = oRequest.GetValueFromParameter(2)
                    Me.n_ISFR_inizio = oRequest.GetValueFromParameter(3)
                    Me.n_ISFR_fine = oRequest.GetValueFromParameter(4)
                    Me.n_ISFR_nomeeTipoIstituto = oRequest.GetValueFromParameter(5)
                    Me.n_ISFR_principaliMaterieAbilita = oRequest.GetValueFromParameter(6)
                    Me.n_ISFR_qualificaConseguita = oRequest.GetValueFromParameter(7)
                    Me.n_ISFR_livelloClassificazioneNazionale = oRequest.GetValueFromParameter(8)
                    Me.n_ISFR_rendiPubblico = oRequest.GetValueFromParameter(9)
                    Me.n_ISFR_esperienzaInCorso = oRequest.GetValueFromParameter(11)
                    Me.n_ErroreDB = Errori_Db.None
                Else
                    Me.n_ISFR_id = -1
                    Me.n_ErroreDB = Errori_Db.DBReadExist
                End If
            Catch ex As Exception
                Me.n_ErroreDB = Errori_Db.DBReadExist
            End Try
        End Sub
        Public Sub Aggiungi()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Integer = 0
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_IstruzioneFormazione_Aggiungi"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@ISFR_id", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ISFR_CREU_id", Me.n_ISFR_CREU_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ISFR_inizio", Me.n_ISFR_inizio, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ISFR_fine", Me.n_ISFR_fine, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ISFR_nomeeTipoIstituto", Me.n_ISFR_nomeeTipoIstituto, ParameterDirection.Input, SqlDbType.VarChar, True, 1000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ISFR_principaliMaterieAbilita", Me.n_ISFR_principaliMaterieAbilita, ParameterDirection.Input, SqlDbType.VarChar, , 1000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ISFR_qualificaConseguita", Me.n_ISFR_qualificaConseguita, ParameterDirection.Input, SqlDbType.VarChar, , 1000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ISFR_livelloClassificazioneNazionale", Me.n_ISFR_livelloClassificazioneNazionale, ParameterDirection.Input, SqlDbType.VarChar, , 1000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ISFR_rendiPubblico", Me.n_ISFR_rendiPubblico, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ISFR_esperienzaInCorso", Me.n_ISFR_esperienzaInCorso, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                iResponse = objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_ISFR_id = oRequest.GetValueFromParameter(1)
                Me.n_ErroreDB = Errori_Db.None
            Catch ex As Exception
                Me.n_ISFR_id = -1
                Me.n_ErroreDB = Errori_Db.DBInsert
            End Try
        End Sub
        Public Sub Modifica()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim IResponse As Integer = 0
            With oRequest
                .Command = "sp_IstruzioneFormazione_Modifica"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@ISFR_id", Me.n_ISFR_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ISFR_CREU_id", Me.n_ISFR_CREU_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ISFR_inizio", Me.n_ISFR_inizio, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ISFR_fine", Me.n_ISFR_fine, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ISFR_nomeeTipoIstituto", Me.n_ISFR_nomeeTipoIstituto, ParameterDirection.Input, SqlDbType.VarChar, True, 1000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ISFR_principaliMaterieAbilita", Me.n_ISFR_principaliMaterieAbilita, ParameterDirection.Input, SqlDbType.VarChar, , 1000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ISFR_qualificaConseguita", Me.n_ISFR_qualificaConseguita, ParameterDirection.Input, SqlDbType.VarChar, , 1000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ISFR_livelloClassificazioneNazionale", Me.n_ISFR_livelloClassificazioneNazionale, ParameterDirection.Input, SqlDbType.VarChar, , 1000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ISFR_rendiPubblico", Me.n_ISFR_rendiPubblico, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ISFR_esperienzaInCorso", Me.n_ISFR_esperienzaInCorso, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                IResponse = objAccesso.GetExecuteNotQuery(oRequest)
                If IResponse = 0 Then
                    Me.n_ErroreDB = Errori_Db.DBChange
                Else
                    Me.n_ErroreDB = Errori_Db.None
                End If
            Catch ex As Exception
                Me.n_ErroreDB = Errori_Db.DBChange
                IResponse = -1
            End Try
        End Sub

        Public Function Elimina(ByVal ISFR_id As Integer) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim responso As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_IstruzioneFormazione_Elimina"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@ISFR_id", ISFR_id, ParameterDirection.Input, SqlDbType.Int)
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

        Public Function GetNumeroIstruzione(ByVal CREU_id As Integer) As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim NumFormazione As Integer

            With oRequest
                .Command = "sp_IstruzioneFormazione_Getnumero"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CREU_id", CREU_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@NumFormazione", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                NumFormazione = oRequest.GetValueFromParameter(2)
                Me.n_ErroreDB = Errori_Db.None
            Catch ax As Exception 'record non trovato
                NumFormazione = 0
                Me.n_ErroreDB = Errori_Db.DBReadExist
            End Try
            Return NumFormazione
        End Function

        Public Function ElencaByPersona(ByVal PRSN_id As Integer, Optional ByVal oVisibilita As Main.FiltroVisibilità = Main.FiltroVisibilità.Tutti) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As DataSet = Nothing
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_IstruzioneFormazione_ElencaByPersona"
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

    End Class
End Namespace
