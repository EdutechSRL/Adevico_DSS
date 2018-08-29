Imports COL_DataLayer

Namespace CL_permessi
    Public Class COL_Permesso

#Region "Public Property"
        Private n_PRMS_id As Integer
        Private n_PRMS_Descrizione As String
        Private n_PRMS_nome As String
        Private n_PRMS_Posizione As Integer
        Private n_Errore As Errori_Db
#End Region

#Region "Public Property"
        Public Property Id() As Integer
            Get
                Id = n_PRMS_id
            End Get
            Set(ByVal Value As Integer)
                n_PRMS_id = Value
            End Set
        End Property
        Public Property Descrizione() As String
            Get
                Descrizione = n_PRMS_Descrizione
            End Get
            Set(ByVal Value As String)
                n_PRMS_Descrizione = Value
            End Set
        End Property
        Public Property Nome() As String
            Get
                Nome = n_PRMS_nome
            End Get
            Set(ByVal Value As String)
                n_PRMS_nome = Value
            End Set
        End Property
        Public Property Posizione() As Integer
            Get
                Posizione = n_PRMS_Posizione
            End Get
            Set(ByVal Value As Integer)
                n_PRMS_Posizione = Value
            End Set
        End Property
        Public ReadOnly Property Errore() As Errori_Db
            Get
                Errore = n_Errore
            End Get
        End Property
#End Region

#Region "Metodi New"
        Sub New()
            Me.n_Errore = Errori_Db.None
        End Sub
#End Region

#Region "Metodi Standard"
        Public Function Elenca() As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As DataSet = Nothing
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Permesso_Elenca"
                .CommandType = CommandType.StoredProcedure
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                dsTable = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try
            Return dsTable
        End Function
        Public Sub Aggiungi()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Permesso_Aggiungi"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@PRMS_Id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRMS_Descrizione", n_PRMS_Descrizione.Trim, ParameterDirection.Input, DbType.String, 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRMS_Nome", n_PRMS_nome.Trim, ParameterDirection.Input, DbType.String, 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRMS_posizione", n_PRMS_Posizione, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                ' Recupero l'ID univoco del Permesso
                Me.n_PRMS_id = oRequest.GetValueFromParameter(1)
            Catch
                Me.n_PRMS_id = -1
                Me.n_Errore = Errori_Db.DBInsert
            End Try
        End Sub
        Public Sub Modifica()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_permesso_modifica"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@PRMS_Id", n_PRMS_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRMS_Descrizione", n_PRMS_Descrizione.Trim, ParameterDirection.Input, DbType.String, 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRMS_Nome", n_PRMS_nome.Trim, ParameterDirection.Input, DbType.String, 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRMS_Posizione", n_PRMS_Posizione, ParameterDirection.Input, DbType.UInt32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBChange
            End Try
        End Sub


        ' MANCA CANCELLAZIONE !!!!!!!!!!!! INSERIRE !!!!!!!!!!!!!!!!!11
#End Region

#Region "Metodi Particolari"
        Public Function ElencaAssociati(ByVal IdServizio As Integer) As DataSet 'elenca i permessi che sono già stati associati ad un determinato servizio
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As DataSet = Nothing
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_PermessoAssociato_Elenca"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@SRVZ_id", IdServizio, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                dsTable = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try
            Return dsTable
        End Function
        Public Function ElencaNonAssociati(ByVal IdServizio As Integer) As DataSet 'elenca i permessi che NON sono stati associati ad un determinato servizio
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As DataSet = Nothing
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_PermessoNonAssociato_Elenca"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@SRVZ_id", IdServizio, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                dsTable = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try
            Return dsTable
        End Function
        Public Function PermessiServizio(ByVal IdServizio As String) As DataSet
            'toglie un record alla tabella di congiunzione "LK_PRMS_SRVZ"
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataSet As New DataSet
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Permessi_Servizio"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@SRVZ_id", IdServizio, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                oDataSet = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try
            Return oDataSet
        End Function
#End Region

    End Class

End Namespace