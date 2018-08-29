Imports COL_DataLayer
Imports COL_BusinessLogic_v2.Comunita


Public Class COL_RaccoltaLink

#Region "Private Property"
    Private n_RLNK_ID As Integer
    Private n_RLNK_CreatoreID As Integer
    Private n_RLNK_CMNT_ID As Integer
    Private n_RLNK_Nome As String
    Private n_RLNK_Url As String
    Private n_RLNK_Descrizione As String
    Private n_RLNK_isCartella As Integer
    Private n_RLNK_Padre_Id As Integer
    Private n_RLNK_OrdineV As Integer
    Private n_RLNK_Created As DateTime
    Private n_RLNK_Modified As DateTime
    Private n_RLNK_Livello As Integer
    Private n_RLNK_BOKM_ID As Integer
    Private n_RLNK_Visibile As Integer
    Private n_Errore As Errori_Db
#End Region

#Region "Public Property"
    Public Property ID() As Integer
        Get
            ID = n_RLNK_ID
        End Get
        Set(ByVal Value As Integer)
            n_RLNK_ID = Value
        End Set
    End Property
    Public Property PRSN_ID() As Integer
        Get
            PRSN_ID = n_RLNK_CreatoreID
        End Get
        Set(ByVal Value As Integer)
            n_RLNK_CreatoreID = Value
        End Set
    End Property
    Public Property CMNT_ID() As Integer
        Get
            CMNT_ID = n_RLNK_CMNT_ID
        End Get
        Set(ByVal Value As Integer)
            n_RLNK_CMNT_ID = Value
        End Set
    End Property
    Public Property Nome() As String
        Get
            Nome = n_RLNK_Nome
        End Get
        Set(ByVal Value As String)
            n_RLNK_Nome = Value
        End Set
    End Property
    Public Property Url() As String
        Get
            Url = n_RLNK_Url
        End Get
        Set(ByVal Value As String)
            n_RLNK_Url = Value
        End Set
    End Property
    Public Property Descrizione() As String
        Get
            Descrizione = n_RLNK_Descrizione
        End Get
        Set(ByVal Value As String)
            n_RLNK_Descrizione = Value
        End Set
    End Property
    Public Property isCartella() As Boolean
        Get
            isCartella = CBool(n_RLNK_isCartella = 1)
        End Get
        Set(ByVal Value As Boolean)
            If Value Then
                n_RLNK_isCartella = 1
            Else
                n_RLNK_isCartella = 0
            End If
        End Set
    End Property
    Public Property Padre_Id() As Integer
        Get
            Padre_Id = n_RLNK_Padre_Id
        End Get
        Set(ByVal Value As Integer)
            n_RLNK_Padre_Id = Value
        End Set
    End Property
    Public Property OrdineVisualizzazione() As Integer
        Get
            OrdineVisualizzazione = n_RLNK_OrdineV
        End Get
        Set(ByVal Value As Integer)
            n_RLNK_OrdineV = Value
        End Set
    End Property
    Public Property CreatoIl() As DateTime
        Get
            CreatoIl = n_RLNK_Created
        End Get
        Set(ByVal Value As DateTime)
            n_RLNK_Created = Value
        End Set
    End Property
    Public Property ModificatoIl() As DateTime
        Get
            ModificatoIl = n_RLNK_Modified
        End Get
        Set(ByVal Value As DateTime)
            n_RLNK_Modified = Value
        End Set
    End Property
    Public Property LinkPersonale_ID() As Integer
        Get
            LinkPersonale_ID = n_RLNK_BOKM_ID
        End Get
        Set(ByVal Value As Integer)
            n_RLNK_BOKM_ID = Value
        End Set
    End Property
    Public ReadOnly Property isFromPersonale() As Boolean
        Get
            Return (n_RLNK_BOKM_ID > 0)
        End Get
    End Property
    Public ReadOnly Property Errore() As Errori_Db
        Get
            Errore = n_Errore
        End Get
    End Property
    Public Property Livello() As Integer
        Get
            Livello = n_RLNK_Livello
        End Get
        Set(ByVal Value As Integer)
            n_RLNK_Livello = Value
        End Set
    End Property
    Public Property isVisibile() As Boolean
        Get
            isVisibile = (n_RLNK_Visibile = 1)
        End Get
        Set(ByVal Value As Boolean)
            If Value Then
                n_RLNK_Visibile = 1
            Else
                n_RLNK_Visibile = 0
            End If
        End Set
    End Property

#End Region

#Region "Metodi New"
    Public Sub New()
        Me.n_Errore = Errori_Db.None
        Me.n_RLNK_Created = Now
        Me.n_RLNK_isCartella = 0
        Me.n_RLNK_Padre_Id = 0
        Me.n_RLNK_OrdineV = 0
        Me.n_RLNK_BOKM_ID = 0
        Me.n_RLNK_Livello = 0
    End Sub
#End Region

#Region "Metodi Standard "
    Public Sub Estrai()
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim objAccesso As New COL_DataAccess
        With oRequest
            .Command = "sp_RaccoltaLink_Estrai"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetAdvancedParameter("@RLNK_ID", Me.n_RLNK_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@RLNK_PRSN_Id", "", ParameterDirection.Output, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@RLNK_CMNT_ID", "", ParameterDirection.Output, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@RLNK_Descrizione", "", ParameterDirection.Output, SqlDbType.VarChar, True, 300)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@RLNK_Nome", "", ParameterDirection.Output, SqlDbType.VarChar, True, 250)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@RLNK_Url", "", ParameterDirection.Output, SqlDbType.VarChar, True, 250)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@RLNK_OrdineV", "", ParameterDirection.Output, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@RLNK_Padre_Id", "", ParameterDirection.Output, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@RLNK_isCartella", "", ParameterDirection.Output, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@RLNK_Created", "", ParameterDirection.Output, SqlDbType.DateTime)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@RLNK_Modified", "", ParameterDirection.Output, SqlDbType.DateTime)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@RLNK_Visibile", "", ParameterDirection.Output, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@RLNK_BOKM_ID", "", ParameterDirection.Output, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@RLNK_Livello", "", ParameterDirection.Output, SqlDbType.Int)
            .Parameters.Add(oParam)

            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With

        Try
            objAccesso.GetExecuteNotQuery(oRequest)
            Me.n_RLNK_CreatoreID = oRequest.GetValueFromParameter(2)
            If IsNumeric(oRequest.GetValueFromParameter(3)) Then
                Me.n_RLNK_CMNT_ID = oRequest.GetValueFromParameter(3)
            End If
            Me.n_RLNK_Descrizione = oRequest.GetValueFromParameter(4)
            Me.n_RLNK_Nome = oRequest.GetValueFromParameter(5)
            Me.n_RLNK_Url = oRequest.GetValueFromParameter(6)
            Me.n_RLNK_OrdineV = oRequest.GetValueFromParameter(7)
            Me.n_RLNK_Padre_Id = oRequest.GetValueFromParameter(8)
            Me.n_RLNK_isCartella = oRequest.GetValueFromParameter(9)
            If IsDate(oRequest.GetValueFromParameter(10)) Then
                If Not Equals(oRequest.GetValueFromParameter(10), New Date) Then
                    Me.n_RLNK_Created = oRequest.GetValueFromParameter(10)
                End If
            End If
            If IsDate(oRequest.GetValueFromParameter(11)) Then
                If Not Equals(oRequest.GetValueFromParameter(11), New Date) Then
                    Me.n_RLNK_Modified = oRequest.GetValueFromParameter(11)
                End If
            End If
            Me.n_RLNK_Visibile = oRequest.GetValueFromParameter(12)
            Me.n_RLNK_BOKM_ID = oRequest.GetValueFromParameter(13)
            Me.n_RLNK_Livello = oRequest.GetValueFromParameter(14)

            Me.n_Errore = Errori_Db.None
        Catch ex As Exception
            Me.n_RLNK_ID = -1
            Me.n_Errore = Errori_Db.DBReadExist
        End Try
    End Sub
    Public Function Aggiungi() As Integer
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim dsTable As Integer
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_RaccoltaLink_Aggiungi"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetAdvancedParameter("@RLNK_ID", "", ParameterDirection.Output, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@RLNK_PRSN_Id", Me.n_RLNK_CreatoreID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@RLNK_CMNT_ID", Me.n_RLNK_CMNT_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@RLNK_Descrizione", Me.n_RLNK_Descrizione, ParameterDirection.Input, SqlDbType.VarChar, True, 300)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@RLNK_Nome", Me.n_RLNK_Nome, ParameterDirection.Input, SqlDbType.VarChar, True, 250)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@RLNK_Url", Me.n_RLNK_Url, ParameterDirection.Input, SqlDbType.VarChar, True, 250)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@RLNK_OrdineV", Me.n_RLNK_OrdineV, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@RLNK_Visibile", Me.n_RLNK_Visibile, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@RLNK_Padre_Id", Me.n_RLNK_Padre_Id, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@RLNK_isCartella", Me.n_RLNK_isCartella, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@RLNK_BOKM_ID", Me.n_RLNK_BOKM_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@RLNK_Livello", Me.n_RLNK_Livello, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            dsTable = objAccesso.GetExecuteNotQuery(oRequest)
            Me.n_RLNK_ID = oRequest.GetValueFromParameter(1)
            Me.n_Errore = Errori_Db.None
        Catch ex As Exception
            ' Codice per il rollback
            Me.n_Errore = Errori_Db.DBInsert
            Me.n_RLNK_ID = -1
            dsTable = -1
        End Try
        Return dsTable
    End Function
    Public Sub Modifica(ByVal replicaPreferiti As Boolean)
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim dsTable As Integer
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_RaccoltaLink_Modifica"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetAdvancedParameter("@RLNK_ID", Me.n_RLNK_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@RLNK_Modified", "", ParameterDirection.Output, SqlDbType.DateTime)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@RLNK_PRSN_Id", Me.n_RLNK_CreatoreID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@RLNK_CMNT_ID", Me.n_RLNK_CMNT_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@RLNK_Descrizione", Me.n_RLNK_Descrizione, ParameterDirection.Input, SqlDbType.VarChar, True, 300)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@RLNK_Nome", Me.n_RLNK_Nome, ParameterDirection.Input, SqlDbType.VarChar, True, 150)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@RLNK_Url", Me.n_RLNK_Url, ParameterDirection.Input, SqlDbType.VarChar, True, 250)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@RLNK_OrdineV", Me.n_RLNK_OrdineV, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@RLNK_Visibile", Me.n_RLNK_Visibile, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@RLNK_Padre_Id", Me.n_RLNK_Padre_Id, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@RLNK_isCartella", Me.n_RLNK_isCartella, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@RLNK_BOKM_ID", Me.n_RLNK_BOKM_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            If replicaPreferiti Then
                oParam = objAccesso.GetAdvancedParameter("@replica", 1, ParameterDirection.Input, SqlDbType.Int)
            Else
                oParam = objAccesso.GetAdvancedParameter("@replica", 0, ParameterDirection.Input, SqlDbType.Int)
            End If
            .Parameters.Add(oParam)
            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            dsTable = objAccesso.GetExecuteNotQuery(oRequest)
            Me.n_RLNK_Modified = oRequest.GetValueFromParameter(2)

            Me.n_Errore = Errori_Db.None
        Catch ex As Exception
            ' Codice per il rollback
            dsTable = -1
            Me.n_Errore = Errori_Db.DBChange
        End Try
    End Sub
    Public Sub Cancella(ByVal replicaPreferiti As Boolean)
        'carica i campi del db nell'oggetto in base all'id della persona
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim objAccesso As New COL_DataAccess
        With oRequest
            .Command = "sp_RaccoltaLink_Cancella"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetAdvancedParameter("@RLNK_ID", Me.n_RLNK_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            If replicaPreferiti Then
                oParam = objAccesso.GetAdvancedParameter("@replica", 1, ParameterDirection.Input, SqlDbType.Int)
            Else
                oParam = objAccesso.GetAdvancedParameter("@replica", 0, ParameterDirection.Input, SqlDbType.Int)
            End If
            .Parameters.Add(oParam)

            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With

        Try
            objAccesso.GetExecuteNotQuery(oRequest)
            Me.n_Errore = Errori_Db.None
        Catch ex As Exception
            Me.n_RLNK_ID = -1
            Me.n_Errore = Errori_Db.DBDelete
        End Try
    End Sub
#End Region

    Public Shared Function ElencaByComunita(ByVal CMNT_ID As Integer, ByVal PRSN_Id As Integer) As DataSet
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim oDataSet As New DataSet
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_RaccoltaLink_ElencaByComunita"
            .CommandType = CommandType.StoredProcedure

            oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", CMNT_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetAdvancedParameter("@PRSN_Id", PRSN_Id, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            oDataSet = objAccesso.GetdataSet(oRequest)
        Catch ex As Exception

        End Try
        Return oDataSet
    End Function

    Public Function ImportaPreferito(ByVal LinkPadre_ID As Integer, ByVal BOKM_ID As Integer, ByVal CMNT_ID As Integer, ByVal idCreatore As Integer, ByVal visibile As Boolean) As Boolean
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim iResponse As Integer
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_RaccoltaLink_ImportaPreferito"
            .CommandType = CommandType.StoredProcedure

            oParam = objAccesso.GetAdvancedParameter("@PRSN_Id", idCreatore, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@BOKM_ID", BOKM_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@LinkPadre_ID", LinkPadre_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", CMNT_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetAdvancedParameter("@RLNK_ID", "", ParameterDirection.Output, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@RLNK_OrdineV", "", ParameterDirection.Output, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@RLNK_Padre_Id", "", ParameterDirection.Output, SqlDbType.Int)
            .Parameters.Add(oParam)
            If visibile Then
                oParam = objAccesso.GetAdvancedParameter("@visibile", 1, ParameterDirection.Input, SqlDbType.Int)
            Else
                oParam = objAccesso.GetAdvancedParameter("@visibile", 0, ParameterDirection.Input, SqlDbType.Int)
            End If
            .Parameters.Add(oParam)
            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            iResponse = objAccesso.GetExecuteNotQuery(oRequest)

            Me.n_RLNK_ID = oRequest.GetValueFromParameter(5)
            Me.n_RLNK_OrdineV = oRequest.GetValueFromParameter(6)
            Me.n_RLNK_Padre_Id = oRequest.GetValueFromParameter(7)
            Me.n_Errore = Errori_Db.None
        Catch ex As Exception
            Me.n_Errore = Errori_Db.DBInsert
            Return False
        End Try
        Return True
    End Function
    Public Function CopiaForImporta(ByVal BOKM_ID As Integer, ByVal PadreId As Integer, ByVal idCreatore As Integer, ByVal CMNT_ID As Integer, ByVal ordineV As Integer, ByVal visibile As Boolean) As Boolean
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim iResponse As Integer
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_RaccoltaLink_CopiaForImporta"
            .CommandType = CommandType.StoredProcedure

            oParam = objAccesso.GetAdvancedParameter("@PRSN_Id", idCreatore, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetAdvancedParameter("@BOKM_ID", BOKM_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", CMNT_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetAdvancedParameter("@RLNK_ID", "", ParameterDirection.Output, SqlDbType.Int)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetAdvancedParameter("@ordineV", ordineV, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetAdvancedParameter("@PadreId", PadreId, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            If visibile Then
                oParam = objAccesso.GetAdvancedParameter("@visibile", 1, ParameterDirection.Input, SqlDbType.Int)
            Else
                oParam = objAccesso.GetAdvancedParameter("@visibile", 0, ParameterDirection.Input, SqlDbType.Int)
            End If
            .Parameters.Add(oParam)
            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            iResponse = objAccesso.GetExecuteNotQuery(oRequest)
            Me.n_RLNK_ID = oRequest.GetValueFromParameter(4)
            Me.n_Errore = Errori_Db.None
        Catch ex As Exception
            Me.n_Errore = Errori_Db.DBInsert
            Return False
        End Try
        Return True
    End Function

    Public Function AlberoCartelle(ByVal CMNT_ID As Integer) As DataSet
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim oDataset As New DataSet
        Dim objAccesso As New COL_DataAccess

        Try
            With oRequest
                .Command = "sp_RaccoltaLink_AlberoCartelle"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", CMNT_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            oDataset = objAccesso.GetdataSet(oRequest)
            Me.n_Errore = Errori_Db.None
        Catch ex As Exception
            Me.n_Errore = Errori_Db.DBError
        End Try
        Return oDataset
    End Function

    Public Shared Function ElencaForOrdineVisualizzazione(ByVal CMNT_ID As Integer, ByVal PRSN_Id As Integer, ByVal PadreID As Integer, ByVal RLNK_ID As Integer) As DataSet
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim oDataset As New DataSet
        Dim objAccesso As New COL_DataAccess

        Try
            With oRequest
                .Command = "sp_RaccoltaLink_ElencaForOrdineVisualizzazione"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", CMNT_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRSN_Id", PRSN_Id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PadreID", PadreID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@RLNK_ID", RLNK_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            oDataset = objAccesso.GetdataSet(oRequest)

        Catch ex As Exception

        End Try
        Return oDataset
    End Function

    Public Shared Function ElencaForEsporta(ByVal CMNT_ID As Integer, ByVal PRSN_Id As Integer) As DataSet
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim oDataSet As New DataSet
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_RaccoltaLink_ElencaForEsporta"
            .CommandType = CommandType.StoredProcedure

            oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", CMNT_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetAdvancedParameter("@PRSN_Id", PRSN_Id, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            oDataSet = objAccesso.GetdataSet(oRequest)
        Catch ex As Exception

        End Try
        Return oDataSet
    End Function

    Public Sub CancellaContenuto()
        'carica i campi del db nell'oggetto in base all'id della persona
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim objAccesso As New COL_DataAccess
        With oRequest
            .Command = "sp_RaccoltaLink_CancellaContenuto"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetAdvancedParameter("@RLNK_ID", Me.n_RLNK_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With

        Try
            objAccesso.GetExecuteNotQuery(oRequest)
            Me.n_Errore = Errori_Db.None
        Catch ex As Exception
            Me.n_RLNK_ID = -1
            Me.n_Errore = Errori_Db.DBDelete
        End Try
    End Sub
End Class