Imports COL_DataLayer

Public Class COL_Internazionalizzazione

#Region "Private Property"
    Private n_INTR_id As Integer
    Private n_INTR_TBSS_id As Integer
    Private n_INTR_CLNN_id As Integer
    Private n_INTR_RCRD_id As Integer
    Private n_INTR_LNGU_id As Integer
    Private n_INTR_Valore As String
    Private nErrori As Errori_Db

#End Region

#Region "Public Property"
    Public Property Id() As Integer
        Get
            Id = n_INTR_id
        End Get
        Set(ByVal Value As Integer)
            n_INTR_id = Value
        End Set
    End Property
    Public Property TabellaId() As Integer
        Get
            TabellaId = n_INTR_TBSS_id
        End Get
        Set(ByVal Value As Integer)
            n_INTR_TBSS_id = Value
        End Set
    End Property
    Public Property ColonnaId() As Integer
        Get
            ColonnaId = n_INTR_CLNN_id
        End Get
        Set(ByVal Value As Integer)
            n_INTR_CLNN_id = Value
        End Set
    End Property
    Public Property RecordId() As Integer
        Get
            RecordId = n_INTR_RCRD_id
        End Get
        Set(ByVal Value As Integer)
            n_INTR_RCRD_id = Value
        End Set
    End Property
    Public Property LinguaId() As Integer
        Get
            LinguaId = n_INTR_LNGU_id
        End Get
        Set(ByVal Value As Integer)
            n_INTR_LNGU_id = Value
        End Set
    End Property
    Public Property Valore() As String
        Get
            Valore = n_INTR_Valore
        End Get
        Set(ByVal Value As String)
            n_INTR_Valore = Value
        End Set
    End Property
    Public ReadOnly Property Errore() As Errori_Db
        Get
            Errore = nErrori
        End Get
    End Property
#End Region

#Region "Metodi New"

    Sub New()
        Me.nErrori = Errori_Db.None
    End Sub

#End Region

#Region "Metodi Standard"

    Public Function ElencaTabelle() As DataSet
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim dsTable As New DataSet
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_Internazionalizzazione_ElencaTabelle"
            .CommandType = CommandType.StoredProcedure
            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With

        Try
            dsTable = objAccesso.GetdataSet(oRequest)
        Catch ex As Exception
            Me.nErrori = Errori_Db.DBError
        End Try
        Return dsTable
    End Function

    Public Function ElencaColonne() As DataSet
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim dsTable As New DataSet
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_Internazionalizzazione_ElencaColonne"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetParameter("@TBSS_id", Me.n_INTR_TBSS_id, ParameterDirection.Input, DbType.Int32)
            .Parameters.Add(oParam)

            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With

        Try
            dsTable = objAccesso.GetdataSet(oRequest)
        Catch ex As Exception
            Me.nErrori = Errori_Db.DBError
        End Try
        Return dsTable
    End Function

    Public Function ElencaValori() As DataSet
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim dsTable As New DataSet
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_Internazionalizzazione_ElencaValori"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetParameter("@TBSS_id", Me.n_INTR_TBSS_id, ParameterDirection.Input, DbType.Int32)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@CLNN_id", Me.n_INTR_CLNN_id, ParameterDirection.Input, DbType.Int32)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@RCRD_id", Me.n_INTR_RCRD_id, ParameterDirection.Input, DbType.Int32)
            .Parameters.Add(oParam)
            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With

        Try
            dsTable = objAccesso.GetdataSet(oRequest)
        Catch ex As Exception
            Me.nErrori = Errori_Db.DBError
        End Try
        Return dsTable
    End Function

    Public Function ElencaRecord(ByVal NomeTabella As String, ByVal NomeCampo As String) As DataSet
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim dsTable As New DataSet
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_Internazionalizzazione_ElencaRecord"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetParameter("@NomeTabella", NomeTabella, ParameterDirection.Input, DbType.String, , 100)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@NomeCampo", NomeCampo, ParameterDirection.Input, DbType.String, , 100)
            .Parameters.Add(oParam)

            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With

        Try
            dsTable = objAccesso.GetdataSet(oRequest)
        Catch ex As Exception
            Me.nErrori = Errori_Db.DBError
        End Try
        Return dsTable
    End Function

    Public Function Aggiungi() As DataSet
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim dsTable As New DataSet
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_Internazionalizzazione_Aggiungi"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetParameter("@TBSS_id", n_INTR_TBSS_id, ParameterDirection.Input, DbType.Int32)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@CLNN_id", n_INTR_CLNN_id, ParameterDirection.Input, DbType.Int32)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@RCRD_id", n_INTR_RCRD_id, ParameterDirection.Input, DbType.Int32)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@Valore", n_INTR_Valore, ParameterDirection.Input, DbType.String)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@LNGU_id", n_INTR_LNGU_id, ParameterDirection.Input, DbType.Int32)
            .Parameters.Add(oParam)
            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With

        Try
            dsTable = objAccesso.GetdataSet(oRequest)
        Catch ex As Exception
            Me.nErrori = Errori_Db.DBError
        End Try
        Return dsTable
    End Function

    Public Function Elimina() As DataSet
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim dsTable As New DataSet
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_Internazionalizzazione_Elimina"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetParameter("@INTR_id", n_INTR_id, ParameterDirection.Input, DbType.Int32)
            .Parameters.Add(oParam)
            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With

        Try
            dsTable = objAccesso.GetdataSet(oRequest)
        Catch ex As Exception
            Me.nErrori = Errori_Db.DBError
        End Try
        Return dsTable
    End Function

#End Region

End Class
