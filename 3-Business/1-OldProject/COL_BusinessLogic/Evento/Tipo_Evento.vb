Imports COL_DataLayer

Namespace Eventi
    Public Class COL_Tipo_Evento
#Region "Private Property"
        Private n_TPEV_id As Integer
        Private n_TPEV_nome As String
        Private n_TPEV_icon As String
        Private n_DB_error As New Errori_Db
#End Region
#Region "Public Property"
        Public Property Id() As Integer
            Get
                Id = n_TPEV_id
            End Get
            Set(ByVal Value As Integer)
                n_TPEV_id = Value
            End Set
        End Property
        Public Property Nome() As String
            Get
                Nome = n_TPEV_nome
            End Get
            Set(ByVal Value As String)
                n_TPEV_nome = Value
            End Set
        End Property
        Public Property Icon() As String
            Get
                Icon = n_TPEV_icon
            End Get
            Set(ByVal Value As String)
                n_TPEV_icon = Value
            End Set
        End Property
        Public ReadOnly Property Errore() As Errori_Db
            Get
                Errore = n_DB_error
            End Get
        End Property
#End Region

#Region "Metodi Standard"
        Public Function Elenca(ByVal LinguaID As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim objAccesso As New COL_DataAccess
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            With oRequest
                .Command = "sp_Tipo_Evento_Elenca"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@LNGU_ID", LinguaID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                dsTable = objAccesso.GetdataSet(oRequest)
            Catch
                Me.n_DB_error = Errori_Db.DBError
            End Try
            Return dsTable
        End Function
        Public Sub Modifica() 'no testata
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Tipo_Evento_Modifica"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@TPEV_id", n_TPEV_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPEV_Nome", n_TPEV_nome.Trim)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPEV_Icon", n_TPEV_icon.Trim)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)

            Catch ex As Exception
                Me.n_DB_error = Errori_Db.DBReadExist
            End Try
        End Sub
        Public Sub Aggiungi() 'no testata
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Tipo_Evento_Aggiungi"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@TPEV_id", n_TPEV_id, ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPEV_Nome", n_TPEV_nome.Trim)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPEV_Icon", n_TPEV_icon.Trim)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_TPEV_id = oRequest.GetValueFromParameter(1)
            Catch ex As Exception
                Me.n_DB_error = Errori_Db.DBInsert
            End Try
        End Sub
        Public Sub Cancella() 'no testata
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Tipo_Evento_Cancella"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@TPEV_id", n_TPEV_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch
                Me.n_DB_error = Errori_Db.DBDelete
            End Try
        End Sub
        Public Sub Estrai()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            'Dim dsTable As New DataSet
            With oRequest
                .Command = "sp_Tipo_Evento_Estrai"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@TPEV_id", n_TPEV_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPEV_Nome", "", ParameterDirection.Output, DbType.String, False, 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPEV_Icon", "", ParameterDirection.Output, DbType.String, False, 255)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                'dsTable = objAccesso.GetdataSet(oRequest)
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_TPEV_nome = oRequest.GetValueFromParameter(2)
                Me.n_TPEV_icon = oRequest.GetValueFromParameter(3)
            Catch ex As Exception
                Me.n_DB_error = Errori_Db.DBReadExist
            End Try
        End Sub
#End Region
    End Class

End Namespace