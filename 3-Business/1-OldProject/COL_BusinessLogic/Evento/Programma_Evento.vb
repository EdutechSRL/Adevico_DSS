Imports COL_DataLayer

Namespace Eventi

    Public Class COL_Programma_Evento

#Region "Private Property"
        Private n_PREV_id As Integer
        Private n_PREV_ProgrammaSvolto As String
        Private n_DB_error As New Errori_Db
#End Region

#Region "Public Property"
        Public Property Id() As Integer
            Get
                Id = n_PREV_id
            End Get
            Set(ByVal Value As Integer)
                n_PREV_id = Value
            End Set
        End Property
        Public Property ProgrammaSvolto() As String
            Get
                ProgrammaSvolto = n_PREV_ProgrammaSvolto
            End Get
            Set(ByVal Value As String)
                n_PREV_ProgrammaSvolto = Value
            End Set
        End Property
        Public ReadOnly Property Errore() As Errori_Db
            Get
                Errore = n_DB_error
            End Get
        End Property
#End Region

#Region "Metodi Standard"
       
        Public Sub Modifica()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_ProgrammaEvento_Modifica"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@PREV_ORRI_id", n_PREV_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PREV_ProgrammaSvolto", n_PREV_ProgrammaSvolto, ParameterDirection.Input, SqlDbType.Text, True, 10000)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch
                Me.n_DB_error = Errori_Db.DBReadExist
            End Try
        End Sub
        Public Sub ModificaTutti()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_ProgrammaEvento_ModificaTutti"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@PREV_ORRI_id", n_PREV_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PREV_ProgrammaSvolto", n_PREV_ProgrammaSvolto, ParameterDirection.Input, SqlDbType.Text, True, 10000)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch
                Me.n_DB_error = Errori_Db.DBReadExist
            End Try
        End Sub

        Public Sub Aggiungi()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Programma_Evento_Aggiungi"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@PREV_ORRI_id", n_PREV_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PREV_ProgrammaSvolto", n_PREV_ProgrammaSvolto, ParameterDirection.Input, SqlDbType.Text, True, 10000)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch ex As Exception
                Me.n_DB_error = Errori_Db.DBInsert
            End Try
        End Sub
        Public Sub Cancella()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_ProgrammaEvento_Cancella"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@PREV_ORRI_id", n_PREV_id, ParameterDirection.Input, SqlDbType.Int)
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
        Public Sub CancellaTutti()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_ProgrammaEvento_Cancella"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@PREV_ORRI_id", n_PREV_id, ParameterDirection.Input, SqlDbType.Int)
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
            'carica i campi del db nell'oggetto in base all'id dell'orario
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_ProgrammaEvento_Estrai"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@PREV_ORRI_id", Me.n_PREV_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                Dim oDataset As New DataSet
                oDataset = objAccesso.GetdataSet(oRequest)
                If IsDBNull(oDataset.Tables(0).Rows(0).Item("PREV_ProgrammaSvolto")) Then
                Else
                    Me.n_PREV_ProgrammaSvolto = oDataset.Tables(0).Rows(0).Item("PREV_ProgrammaSvolto")
                End If
            Catch ax As Exception
                Me.n_DB_error = Errori_Db.DBReadExist
            End Try
        End Sub
#End Region
    End Class

End Namespace