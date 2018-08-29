Imports COL_DataLayer

Namespace Comunita
    <Serializable()>
     Public Class COL_Istituzione

#Region "Private Property"
        Private n_ISTT_id As Integer
        Private n_ISTT_RagioneSociale As String
        Private n_ISTT_Indirizzo As String
        Private n_ISTT_Cap As String
        Private n_ISTT_Citta As String
        Private n_ISTT_PRVN As New Provincia
        Private n_ISTT_Telefono1 As String
        Private n_ISTT_Telefono2 As String
        Private n_ISTT_Fax As String
        Private n_ISTT_HomePage As String
        Private n_ISTT_IsUniversita As Boolean
        Private n_ISTT_STTO As New COL_Stato
        Private n_ISTT_Logo As String
        Private n_errore As Errori_Db
#End Region

#Region "Public Property"
        Public Property Id() As Integer
            Get
                Id = n_ISTT_id
            End Get
            Set(ByVal Value As Integer)
                n_ISTT_id = Value
            End Set
        End Property
        Public Property RagioneSociale() As String
            Get
                RagioneSociale = n_ISTT_RagioneSociale
            End Get
            Set(ByVal Value As String)
                n_ISTT_RagioneSociale = Value
            End Set
        End Property
        Public Property Indirizzo() As String
            Get
                Indirizzo = n_ISTT_Indirizzo
            End Get
            Set(ByVal Value As String)
                n_ISTT_Indirizzo = Value
            End Set
        End Property
        Public Property Cap() As String
            Get
                Cap = n_ISTT_Cap
            End Get
            Set(ByVal Value As String)
                n_ISTT_Cap = Value
            End Set
        End Property
        Public Property Citta() As String
            Get
                Citta = n_ISTT_Citta
            End Get
            Set(ByVal Value As String)
                n_ISTT_Citta = Value
            End Set
        End Property
        Public Property Provincia() As Provincia
            Get
                Provincia = n_ISTT_PRVN
            End Get
            Set(ByVal Value As Provincia)
                n_ISTT_PRVN = Value
            End Set
        End Property
        Public Property Stato() As COL_Stato
            Get
                Stato = n_ISTT_STTO
            End Get
            Set(ByVal Value As COL_Stato)
                n_ISTT_STTO = Value
            End Set
        End Property
        Public Property Telefono1() As String
            Get
                Telefono1 = n_ISTT_Telefono1
            End Get
            Set(ByVal Value As String)
                n_ISTT_Telefono1 = Value
            End Set
        End Property
        Public Property Telefono2() As String
            Get
                Telefono2 = n_ISTT_Telefono2
            End Get
            Set(ByVal Value As String)
                n_ISTT_Telefono2 = Value
            End Set
        End Property
        Public Property Fax() As String
            Get
                Fax = n_ISTT_Fax
            End Get
            Set(ByVal Value As String)
                n_ISTT_Fax = Value
            End Set
        End Property
        Public Property HomePage() As String
            Get
                HomePage = n_ISTT_HomePage
            End Get
            Set(ByVal Value As String)
                n_ISTT_HomePage = Value
            End Set
        End Property
        Public Property IsUniversita() As Boolean
            Get
                IsUniversita = n_ISTT_IsUniversita
            End Get
            Set(ByVal Value As Boolean)
                n_ISTT_IsUniversita = Value
            End Set
        End Property
        Public ReadOnly Property Errore() As Errori_Db
            Get
                Errore = n_errore
            End Get
        End Property
        Public Property Logo() As String
            Get
                Logo = n_ISTT_Logo
            End Get
            Set(ByVal Value As String)
                n_ISTT_Logo = Value
            End Set
        End Property
#End Region

#Region "Metodi New"
        Sub New()
            Me.n_errore = Errori_Db.None
        End Sub
#End Region

#Region "Metodi"
        Public Function Elenca() As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Istituzione_Elenca"
                .CommandType = CommandType.StoredProcedure
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                dsTable = objAccesso.GetdataSet(oRequest)
                Me.n_errore = Errori_Db.None
            Catch ex As Exception
                Me.n_errore = Errori_Db.DBError
            End Try
            Return dsTable
        End Function
        Public Sub Modifica()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Istituzione_Modifica"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@ISTT_Id", Me.n_ISTT_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ISTT_RagioneSociale", n_ISTT_RagioneSociale, ParameterDirection.Input, DbType.String, , 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ISTT_Indirizzo", n_ISTT_Indirizzo, ParameterDirection.Input, DbType.String, , 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ISTT_Cap", n_ISTT_Cap, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ISTT_Citta", n_ISTT_Citta, ParameterDirection.Input, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ISTT_PRVN_ID", n_ISTT_PRVN.ID, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ISTT_STTO_ID", n_ISTT_STTO.ID, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ISTT_Telefono1", n_ISTT_Telefono1, ParameterDirection.Input, DbType.String, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ISTT_Telefono2", n_ISTT_Telefono2, ParameterDirection.Input, DbType.String, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ISTT_Fax", n_ISTT_Fax, ParameterDirection.Input, DbType.String, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ISTT_HomePage", n_ISTT_HomePage, ParameterDirection.Input, DbType.String, , 250)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ISTT_IsUniversita", n_ISTT_IsUniversita, , DbType.Boolean)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ISTT_Logo", n_ISTT_Logo, , DbType.String, , 300)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_errore = Errori_Db.None
            Catch ex As Exception
                Me.n_errore = Errori_Db.DBChange
            End Try
        End Sub
        Public Sub Aggiungi()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Istituzione_Aggiungi"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@ISTT_Id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ISTT_RagioneSociale", n_ISTT_RagioneSociale, ParameterDirection.Input, DbType.String, , 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ISTT_Indirizzo", n_ISTT_Indirizzo, ParameterDirection.Input, DbType.String, , 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ISTT_Cap", n_ISTT_Cap, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ISTT_Citta", n_ISTT_Citta, ParameterDirection.Input, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ISTT_PRVN_ID", n_ISTT_PRVN.ID, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ISTT_STTO_ID", n_ISTT_STTO.ID, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ISTT_Telefono1", n_ISTT_Telefono1, ParameterDirection.Input, DbType.String, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ISTT_Telefono2", n_ISTT_Telefono2, ParameterDirection.Input, DbType.String, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ISTT_Fax", n_ISTT_Fax, ParameterDirection.Input, DbType.String, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ISTT_HomePage", n_ISTT_HomePage, ParameterDirection.Input, DbType.String, , 250)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ISTT_IsUniversita", n_ISTT_IsUniversita, , DbType.Boolean)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ISTT_Logo", n_ISTT_Logo, , DbType.String, , 300)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                'eseguo l'inserimento e Recupero l'ID univoco dell'istituzione.
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_errore = Errori_Db.None
                Me.n_ISTT_id = oRequest.GetValueFromParameter(1)
            Catch
                Me.n_ISTT_id = -1
                Me.n_errore = Errori_Db.DBInsert
            End Try
        End Sub
        Public Function OrganizzazioniAssociate(ByVal ISTT_ID As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Istituzione_Elenca_Organizzazioni"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@ISTT_ID", ISTT_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@TPPR_ID", -1, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                dsTable = objAccesso.GetdataSet(oRequest)
                Me.n_errore = Errori_Db.None
            Catch ex As Exception
                Me.n_errore = Errori_Db.DBError
            End Try
            Return dsTable
        End Function
        Public Function OrganizzazioniAssociateByTipoPersona(ByVal IstituzioneID As Integer, ByVal TipoPersonaID As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Istituzione_Elenca_Organizzazioni"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@ISTT_ID", IstituzioneID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@TPPR_ID", TipoPersonaID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                dsTable = objAccesso.GetdataSet(oRequest)
                Me.n_errore = Errori_Db.None
            Catch ex As Exception
                Me.n_errore = Errori_Db.DBError
            End Try
            Return dsTable
        End Function
        Public Sub Estrai()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim oIstituzione As New COL_Istituzione

            With oRequest
                .Command = "sp_Istituzione_estrai"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@ISTT_id", Id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ISTT_RagioneSociale", "", ParameterDirection.Output, DbType.String, , 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ISTT_Indirizzo", "", ParameterDirection.Output, DbType.String, , 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ISTT_Cap", "", ParameterDirection.Output, DbType.String, , 5)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ISTT_Citta", "", ParameterDirection.Output, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ISTT_PRVN", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ISTT_STTO", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ISTT_Telefono1", "", ParameterDirection.Output, DbType.String, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ISTT_Telefono2", "", ParameterDirection.Output, DbType.String, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ISTT_Fax", "", ParameterDirection.Output, DbType.String, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ISTT_HomePage", "", ParameterDirection.Output, DbType.String, , 250)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ISTT_isUniversita", "", ParameterDirection.Output, DbType.Boolean)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ISTT_Logo", "", ParameterDirection.Output, DbType.String, , 300)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)

                Me.n_ISTT_RagioneSociale = oRequest.GetValueFromParameter(2)
                Me.n_ISTT_Indirizzo = oRequest.GetValueFromParameter(3)
                Me.n_ISTT_Cap = oRequest.GetValueFromParameter(4)
                Me.n_ISTT_Citta = oRequest.GetValueFromParameter(5)
                Me.n_ISTT_PRVN.ID = oRequest.GetValueFromParameter(6)
                Me.n_ISTT_STTO.ID = oRequest.GetValueFromParameter(7)
                Me.n_ISTT_Telefono1 = oRequest.GetValueFromParameter(8)
                Me.n_ISTT_Telefono2 = oRequest.GetValueFromParameter(9)
                Me.n_ISTT_Fax = oRequest.GetValueFromParameter(10)
                Me.n_ISTT_HomePage = oRequest.GetValueFromParameter(11)
                Me.n_ISTT_IsUniversita = oRequest.GetValueFromParameter(12)
                Me.n_ISTT_Logo = oRequest.GetValueFromParameter(13)
                ' oIstituzione.Id = oRequest.GetValueFromParameter(13)
                Me.n_errore = Errori_Db.None
            Catch ax As Exception
                Me.n_errore = Errori_Db.DBReadExist
            End Try
        End Sub
        Public Sub Elimina()
            'cancella istituzione
            'attenzione!!!
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .CommandType = CommandType.StoredProcedure
                oRequest.Command = "sp_Istituzione_Elimina"
                oParam = objAccesso.GetParameter("@ISTT_Id", Me.n_ISTT_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_errore = Errori_Db.None
            Catch
                Me.n_errore = Errori_Db.DBDelete
            End Try
        End Sub

        Public Function GetPersoneOrganizzazioniAssociate(ByRef totaleORGN As Integer) As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim totalePRSN As Integer
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Istituzione_getPersoneOrganizzazioniAssociate"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@ISTT_ID", Me.n_ISTT_id, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TotalePRSN", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TotaleORGN", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                totalePRSN = oRequest.GetValueFromParameter(2)
                totaleORGN = oRequest.GetValueFromParameter(3)
                Me.n_errore = Errori_Db.None
            Catch ex As Exception
                n_errore = Errori_Db.DBReadExist
                totaleORGN = -1
                totalePRSN = -1
            End Try
            Return totalePRSN
        End Function
#End Region

        Public Sub AssociaLogo(Optional ByVal newpath As String = Nothing)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Istituzione_AssociaLogo"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@ISTT_Id", Me.n_ISTT_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ISTT_Logo", newpath, ParameterDirection.Input, DbType.String, True, 300)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_errore = Errori_Db.None
            Catch ex As Exception
                Me.n_errore = Errori_Db.DBReadExist
            End Try
        End Sub


    End Class
End Namespace