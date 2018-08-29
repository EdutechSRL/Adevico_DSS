Imports COL_DataLayer
Public Class COL_Parametri
    Private n_PRMT_id As Integer
    Private n_PRMT_Descrizione As String
    Private n_PRMT_Valore As String
    Private n_errore As Errori_Db

#Region "Proprietà"
    Public Property Id() As Integer
        Get
            Id = n_PRMT_id
        End Get
        Set(ByVal Value As Integer)
            n_PRMT_id = Value
        End Set
    End Property
    Public Property Descrizione() As String
        Get
            Descrizione = n_PRMT_Descrizione
        End Get
        Set(ByVal Value As String)
            n_PRMT_Descrizione = Value
        End Set
    End Property
    Public Property Valore() As String
        Get
            Valore = n_PRMT_Valore
        End Get
        Set(ByVal Value As String)
            n_PRMT_Valore = Value
        End Set
    End Property
    Public ReadOnly Property Errore() As Errori_Db
        Get
            Errore = n_errore
        End Get
    End Property
#End Region
#Region "Metodi New"
    Sub New()
        Me.n_errore = Errori_Db.None
    End Sub
#End Region
#Region "Metodi Standard"
    Public Function Elenca() As DataSet
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim objAccesso As New COL_DataAccess
        Dim dsTable As New DataSet
        With oRequest
            .Command = "sp_Parametri_Elenca"
            .CommandType = CommandType.StoredProcedure
            .Role = COL_Request.UserRole.Admin
            .Transactional = False
        End With
        Try
            dsTable = objAccesso.GetDataSet(oRequest)
        Catch ex As Exception
            Me.n_errore = Errori_Db.DBError
        End Try
        Return dsTable
    End Function
    Public Sub Estrai()
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_Parametri_Estrai"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetParameter("@LNGU_ID", Me.n_PRMT_id, , DbType.Int32)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@PRMS_Valore", "", ParameterDirection.Output, DbType.String, , 500)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@PRMS_Descrizione", "", ParameterDirection.Output, DbType.String, , 50)
            .Parameters.Add(oParam)
            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            objAccesso.GetExecuteNotQuery(oRequest)
            Me.n_PRMT_Valore = oRequest.GetValueFromParameter(2)
            Me.n_PRMT_Descrizione = oRequest.GetValueFromParameter(3)
        Catch ex As Exception
            Me.n_errore = Errori_Db.DBError
        End Try
    End Sub
    Public Sub Modifica() 'manca sp
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim objAccesso As New COL_DataAccess
        With oRequest
            .Command = "sp_Parametri_Modifica"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetParameter("@PRMT_Id", Me.n_PRMT_id, ParameterDirection.Input, DbType.Int32)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@PRMT_Valore", Me.n_PRMT_Valore.Trim, ParameterDirection.Input, DbType.String, )
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@PRMT_Descrizione", Me.n_PRMT_Descrizione.Trim, ParameterDirection.Input, DbType.String, )
            .Parameters.Add(oParam)
            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            objAccesso.GetExecuteNotQuery(oRequest)
        Catch ex As Exception
            Me.n_errore = Errori_Db.DBError
        End Try
    End Sub
    Public Sub Aggiungi() 'manca sp
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_Parametri_Modifica"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetParameter("@PRMT_Id", Me.n_PRMT_id, ParameterDirection.Input, DbType.Int32)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@PRMT_Valore", Me.n_PRMT_Valore.Trim, ParameterDirection.Input, DbType.String, )
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@PRMT_Descrizione", Me.n_PRMT_Descrizione.Trim, ParameterDirection.Input, DbType.String, )
            .Parameters.Add(oParam)
            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            objAccesso.GetExecuteNotQuery(oRequest)
            ' Recupero l'ID univoco del permesso
            Me.n_PRMT_id = oRequest.GetValueFromParameter(1)
        Catch ex As Exception
            Me.n_errore = Errori_Db.DBInsert
            Me.n_PRMT_id = -1
        End Try
    End Sub
#End Region
End Class