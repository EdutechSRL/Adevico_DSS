Imports COL_DataLayer

Namespace Comunita
    Public Class COL_Modello_Comunita
        Private n_MDCM_id As Integer
        Private n_MDCM_nome As String
        Private n_MDCM_descrizione As String
        Private n_MDCM_screenshot As String
        Private n_MDCM_pathCss As String
        Private n_errore As Errori_Db

#Region "Properties"
        Public Property Id() As Integer
            Get
                Id = n_MDCM_id
            End Get
            Set(ByVal Value As Integer)
                n_MDCM_id = Value
            End Set
        End Property
        Public Property Nome() As String
            Get
                Nome = n_MDCM_nome
            End Get
            Set(ByVal Value As String)
                n_MDCM_nome = Value
            End Set
        End Property
        Public Property Descrizione() As String
            Get
                Descrizione = n_MDCM_descrizione
            End Get
            Set(ByVal Value As String)
                n_MDCM_descrizione = Value
            End Set
        End Property
        Public Property Screenshot() As String
            Get
                Screenshot = n_MDCM_screenshot
            End Get
            Set(ByVal Value As String)
                n_MDCM_screenshot = Value
            End Set
        End Property
        Public Property PathCSS() As String
            Get
                PathCSS = n_MDCM_pathCss
            End Get
            Set(ByVal Value As String)
                n_MDCM_pathCss = Value
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
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Modello_Comunita_elenca"
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
        Public Sub Aggiungi()
            'aggiunge record nella tabella dei modelli comunità
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Modello_Comunita_Aggiungi"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@MDCM_id", n_MDCM_id, ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MDCM_descrizione", n_MDCM_descrizione.Trim, ParameterDirection.Input, DbType.String)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MDCM_nome", n_MDCM_nome.Trim, ParameterDirection.Input, DbType.String)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MDCM_ScreenShot", n_MDCM_screenshot.Trim, ParameterDirection.Input, DbType.String)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MDCM_pathCss", n_MDCM_pathCss.Trim, ParameterDirection.Input, DbType.String)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_MDCM_id = oRequest.GetValueFromParameter(1)
            Catch
                Me.n_MDCM_id = -1
                Me.n_errore = Errori_Db.DBInsert
            End Try
        End Sub
        Public Sub Modifica()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Modello_Comunita_Modifica"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@MDCM_id", n_MDCM_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MDCM_descrizione", n_MDCM_descrizione.Trim, ParameterDirection.Input, DbType.String)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MDCM_nome", n_MDCM_nome.Trim, ParameterDirection.Input, DbType.String)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MDCM_ScreenShot", n_MDCM_screenshot.Trim, ParameterDirection.Input, DbType.String)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MDCM_pathCss", n_MDCM_pathCss.Trim, ParameterDirection.Input, DbType.String)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch
                Me.n_errore = Errori_Db.DBChange
            End Try
        End Sub
        Public Sub Estrai()
            'da creare
        End Sub
#End Region
    End Class
End Namespace