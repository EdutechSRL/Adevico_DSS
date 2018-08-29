Imports COL_DataLayer

Namespace Forum
    Public Class COL_TipoRuoloForum
#Region "Private Properties"

        Private n_TPRF_id As Integer
        Private n_TPRF_nome As String
        Private n_TPRF_descrizione As Integer
        Private n_erroreDb As Errori_Db
#End Region

#Region "Public Properties"

        Public Property Id() As Integer
            Get
                Id = n_TPRF_id
            End Get
            Set(ByVal Value As Integer)
                n_TPRF_id = Value
            End Set
        End Property
        Public Property Nome() As String
            Get
                Nome = n_TPRF_nome
            End Get
            Set(ByVal Value As String)
                n_TPRF_nome = Value
            End Set
        End Property
        Public Property Descrizione() As String
            Get
                Descrizione = n_TPRF_descrizione
            End Get
            Set(ByVal Value As String)
                n_TPRF_descrizione = Value
            End Set
        End Property


        Public ReadOnly Property Errore() As Errori_Db
            Get
                Errore = n_erroreDb
            End Get
        End Property
#End Region

#Region "Metodi New"
        Sub New()
            Me.n_erroreDb = Errori_Db.None
        End Sub
#End Region

#Region "Metodi"

        Public Function Elenca(ByVal LinguaID As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_TipoRuoloForum_Elenca"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@LinguaID", LinguaID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                dsTable = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception
                Me.n_erroreDb = Errori_Db.DBError
            End Try
            Return dsTable
        End Function
#End Region
    End Class
End Namespace
