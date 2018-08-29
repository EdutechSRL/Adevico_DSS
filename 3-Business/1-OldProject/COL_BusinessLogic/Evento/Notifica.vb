Imports COL_DataLayer

Namespace Eventi
    Public Class COL_Notifica
        Private n_NTFC_id As Integer
        Private n_NTFC_Tipo As String
        Private n_errore As Errori_Db

#Region "Metodi New"
        Sub New()
            Me.n_errore = Errori_Db.None
        End Sub
#End Region

#Region "Proprietà"
        Public Property Id() As Integer
            Get
                Id = n_NTFC_id
            End Get
            Set(ByVal Value As Integer)
                n_NTFC_id = Value
            End Set
        End Property
        Public Property Tipo() As String
            Get
                Tipo = n_NTFC_Tipo
            End Get
            Set(ByVal Value As String)
                n_NTFC_Tipo = Value
            End Set
        End Property
        Public ReadOnly Property Errore() As Errori_Db
            Get
                Errore = n_errore
            End Get
        End Property
#End Region

#Region "Metodi"
        Public Function Elenca() As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Notifica_Elenca"
                .CommandType = CommandType.StoredProcedure
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                dsTable = objAccesso.GetdataSet(oRequest)
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
                .Command = "sp_Notifica_Modifica"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@NTFC_Id", n_NTFC_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@NTFC_Tipo", n_NTFC_Tipo.Trim, ParameterDirection.Input, DbType.String)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch ex As Exception
                Me.n_errore = Errori_Db.DBChange
            End Try
        End Sub
        Public Sub Aggiungi()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Notifica_Aggiungi"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@NTFC_Id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@NTFC_Tipo", n_NTFC_Tipo.Trim, ParameterDirection.Input, DbType.String)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch
                Me.n_errore = Errori_Db.DBInsert
            End Try
        End Sub
#End Region

    End Class
End Namespace