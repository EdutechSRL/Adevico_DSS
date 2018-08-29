Imports COL_DataLayer

Namespace CL_persona
    <Serializable()>
     Public Class COL_TipoPersona

#Region "Private Property"
        Private n_TPPR_id As Integer
        Private n_TPPR_Descrizione As String
        Private n_errore As Errori_Db
#End Region

#Region "Public Property"
        Public Property ID() As Integer
            Get
                ID = n_TPPR_id
            End Get
            Set(ByVal Value As Integer)
                n_TPPR_id = Value
            End Set
        End Property
        Public Property Descrizione() As String
            Get
                Descrizione = n_TPPR_Descrizione
            End Get
            Set(ByVal Value As String)
                n_TPPR_Descrizione = Value
            End Set
        End Property
        Public ReadOnly Property ErroreDB() As String
            Get
                ErroreDB = n_errore
            End Get
        End Property
#End Region

#Region "Metodi New"
        Sub New()
            Me.n_errore = Errori_Db.None
        End Sub
#End Region

#Region "Metodi Standard"
        Public Function Elenca(ByVal LinguaID As Integer, ByVal oFiltro As Main.FiltroElencoTipiPersona) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As DataSet = Nothing
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_TipoPersona_Elenca"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@Filtro", CType(oFiltro, Main.FiltroElencoTipiPersona), ParameterDirection.Input, SqlDbType.Int)
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
        Public Function ElencaConnessi(ByVal LinguaID As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As DataSet = Nothing
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_TipoPersona_ElencaConnessi"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@LinguaID", LinguaID, , DbType.Int32)
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
        Public Sub Modifica()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Tipo_Persona_Modifica"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@TPPR_id", n_TPPR_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPPR_Descrizione", n_TPPR_Descrizione.Trim)
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
                .Command = "sp_Tipo_Persona_Modifica"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@TPPR_Descrizione", n_TPPR_Descrizione.Trim)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_errore = Errori_Db.None
            Catch ex As Exception
                Me.n_errore = Errori_Db.DBInsert
            End Try
        End Sub
        Public Sub Estrai(ByVal LinguaID As Integer)
            'carica i campi del db nell'oggetto in base all'id della persona
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_TipoPersona_Estrai"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@TPPR_id", n_TPPR_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@TPPR_Descrizione", "", ParameterDirection.Output, SqlDbType.VarChar, , 300)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_TPPR_Descrizione = oRequest.GetValueFromParameter(2)
                Me.n_errore = Errori_Db.None
            Catch ax As Exception
                Me.n_errore = Errori_Db.DBReadExist
            End Try
        End Sub
        Public Function ElencaConCurriculum(ByVal LinguaID As Integer, ByVal oVisibilita As Main.FiltroVisibilità) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As DataSet = Nothing
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_TipoPersona_ElencaConCurriculum"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)


                oParam = objAccesso.GetAdvancedParameter("@Visibilita", CType(oVisibilita, Main.FiltroVisibilità), ParameterDirection.Input, SqlDbType.Int)
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
#End Region

#Region "Metodi iscrizione"
        Public Shared Function ListOfCanSubscribe(ByVal LinguaID As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataset As DataSet = Nothing
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_TipoPersona_ListOfCanSubscribe"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@LinguaID", LinguaID, , DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                oDataset = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception

            End Try
            Return oDataset
        End Function
        Public Shared Function ListOfCanSubscribeByOrganization(ByVal LinguaID As Integer, ByVal ORGN_Id As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataset As DataSet = Nothing
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_TipoPersona_ListOfCanSubscribe"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@LinguaID", LinguaID, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@ORGN_Id", ORGN_Id, , DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                oDataset = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception

            End Try
            Return oDataset
        End Function
        Public Shared Function ListOfOrganizationToSubscribe(ByVal IstituzioneID As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataset As DataSet = Nothing
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_TipoPersona_ListOfOrganizationToSubscribe"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@IstituzioneID", IstituzioneID, , DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                oDataset = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception

            End Try
            Return oDataset
        End Function
#End Region

        Public Shared Function ListForCreate(ByVal LinguaID As Integer) As List(Of COL_TipoPersona)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As New List(Of COL_TipoPersona)
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_TipoPersona_Elenca"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@Filtro", CType(Main.FiltroElencoTipiPersona.All, Main.FiltroElencoTipiPersona), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Dim iReader As IDataReader = Nothing
            Try
                iReader = objAccesso.GetdataReader(oRequest)
                While iReader.Read
                    iResponse.Add(New COL_TipoPersona() With {.ID = iReader.Item("TPPR_id"), .Descrizione = iReader.Item("TPPR_descrizione")})
                End While

            Catch ex As Exception
            Finally
                If Not IsNothing(iReader) AndAlso iReader.IsClosed = False Then
                    iReader.Close()
                End If
            End Try
            Return iResponse
        End Function

        Public Shared Function ListForSubscription(ByVal LinguaID As Integer, ByVal OrganizationID As Integer) As List(Of COL_TipoPersona)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As New List(Of COL_TipoPersona)
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_TipoPersona_ListOfCanSubscribe"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@LinguaID", LinguaID, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@ORGN_Id", OrganizationID, , DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Dim iReader As IDataReader = Nothing
            Try
                iReader = objAccesso.GetdataReader(oRequest)
                While iReader.Read
                    iResponse.Add(New COL_TipoPersona() With {.ID = iReader.Item("TPPR_id"), .Descrizione = iReader.Item("TPPR_descrizione")})
                End While
            Catch ex As Exception
            Finally
                If Not IsNothing(iReader) AndAlso iReader.IsClosed = False Then
                    iReader.Close()
                End If
            End Try
            Return iResponse
        End Function
    End Class
End Namespace