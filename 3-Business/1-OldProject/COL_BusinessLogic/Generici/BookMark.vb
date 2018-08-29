Imports COL_DataLayer
Imports COL_BusinessLogic_v2.Comunita


Public Class COL_BookMark

#Region "Private Property"
    Private n_BOKM_ID As Integer
    Private n_BOKM_PRSN_ID As Integer
    Private n_BOKM_CMNT_ID As Integer
    Private n_BOKM_Nome As String
    Private n_BOKM_Url As String
    Private n_BOKM_Descrizione As String
    Private n_BOKM_isCartella As Integer
    Private n_BOKM_Padre_Id As Integer
    Private n_BOKM_OrdineV As Integer
    Private n_BOKM_Livello As Integer
    Private n_BOKM_Created As DateTime
    Private n_BOKM_Modified As DateTime
    Private n_Errore As Errori_Db
#End Region

#Region "Public Property"
    Public Property ID() As Integer
        Get
            ID = n_BOKM_ID
        End Get
        Set(ByVal Value As Integer)
            n_BOKM_ID = Value
        End Set
    End Property
    Public Property PRSN_ID() As Integer
        Get
            PRSN_ID = n_BOKM_PRSN_ID
        End Get
        Set(ByVal Value As Integer)
            n_BOKM_PRSN_ID = Value
        End Set
    End Property
    Public Property CMNT_ID() As Integer
        Get
            CMNT_ID = n_BOKM_CMNT_ID
        End Get
        Set(ByVal Value As Integer)
            n_BOKM_CMNT_ID = Value
        End Set
    End Property
    Public Property Nome() As String
        Get
            Nome = n_BOKM_Nome
        End Get
        Set(ByVal Value As String)
            n_BOKM_Nome = Value
        End Set
    End Property
    Public Property Url() As String
        Get
            Url = n_BOKM_Url
        End Get
        Set(ByVal Value As String)
            n_BOKM_Url = Value
        End Set
    End Property
    Public Property Descrizione() As String
        Get
            Descrizione = n_BOKM_Descrizione
        End Get
        Set(ByVal Value As String)
            n_BOKM_Descrizione = Value
        End Set
    End Property
    Public Property isCartella() As Boolean
        Get
            isCartella = CBool(n_BOKM_isCartella = 1)
        End Get
        Set(ByVal Value As Boolean)
            If Value Then
                n_BOKM_isCartella = 1
            Else
                n_BOKM_isCartella = 0
            End If
        End Set
    End Property
    Public Property Padre_Id() As Integer
        Get
            Padre_Id = n_BOKM_Padre_Id
        End Get
        Set(ByVal Value As Integer)
            n_BOKM_Padre_Id = Value
        End Set
    End Property
    Public Property Livello() As Integer
        Get
            Livello = n_BOKM_Livello
        End Get
        Set(ByVal Value As Integer)
            n_BOKM_Livello = Value
        End Set
    End Property
    Public Property OrdineVisualizzazione() As Integer
        Get
            OrdineVisualizzazione = n_BOKM_OrdineV
        End Get
        Set(ByVal Value As Integer)
            n_BOKM_OrdineV = Value
        End Set
    End Property
    Public Property CreatoIl() As DateTime
        Get
            CreatoIl = n_BOKM_Created
        End Get
        Set(ByVal Value As DateTime)
            n_BOKM_Created = Value
        End Set
    End Property
    Public Property ModificatoIl() As DateTime
        Get
            ModificatoIl = n_BOKM_Modified
        End Get
        Set(ByVal Value As DateTime)
            n_BOKM_Modified = Value
        End Set
    End Property
    Public ReadOnly Property Errore() As Errori_Db
        Get
            Errore = n_Errore
        End Get
    End Property
    Public ReadOnly Property isFromComunita() As Boolean
        Get
            Return (Me.n_BOKM_CMNT_ID > 0)
        End Get
    End Property

#End Region

#Region "Metodi New"
    Public Sub New()
        Me.n_Errore = Errori_Db.None
        Me.n_BOKM_Created = Now
        Me.n_BOKM_isCartella = 0
        Me.n_BOKM_Padre_Id = 0
        Me.n_BOKM_OrdineV = 0
    End Sub
#End Region

#Region "Metodi Standard "

    Public Sub Estrai()
        'carica i campi del db nell'oggetto in base all'id della persona
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim objAccesso As New COL_DataAccess
        With oRequest
            .Command = "sp_BookMark_Estrai"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetParameter("@BOKM_ID", Me.n_BOKM_ID, ParameterDirection.Input, DbType.Int32)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@BOKM_PRSN_Id", "", ParameterDirection.Output, DbType.Int32)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@BOKM_CMNT_ID", "", ParameterDirection.Output, DbType.Int32)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@BOKM_Descrizione", "", ParameterDirection.Output, DbType.String, True, 300)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@BOKM_Nome", "", ParameterDirection.Output, DbType.String, True, 250)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@BOKM_Url", "", ParameterDirection.Output, DbType.String, True, 250)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@BOKM_OrdineV", "", ParameterDirection.Output, DbType.Int32)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@BOKM_Padre_Id", "", ParameterDirection.Output, DbType.Int32)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@BOKM_isCartella", "", ParameterDirection.Output, DbType.Byte)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@BOKM_Created", "", ParameterDirection.Output, DbType.DateTime)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@BOKM_Modified", "", ParameterDirection.Output, DbType.DateTime)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@BOKM_Livello", "", ParameterDirection.Output, DbType.Int32)
            .Parameters.Add(oParam)
            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With

        Try
            objAccesso.GetExecuteNotQuery(oRequest)
            Me.n_BOKM_PRSN_ID = oRequest.GetValueFromParameter(2)
            If IsNumeric(oRequest.GetValueFromParameter(3)) Then
                Me.n_BOKM_CMNT_ID = oRequest.GetValueFromParameter(3)
            End If
            Me.n_BOKM_Descrizione = oRequest.GetValueFromParameter(4)
            Me.n_BOKM_Nome = oRequest.GetValueFromParameter(5)
            Me.n_BOKM_Url = oRequest.GetValueFromParameter(6)
            Me.n_BOKM_OrdineV = oRequest.GetValueFromParameter(7)
            Me.n_BOKM_Padre_Id = oRequest.GetValueFromParameter(8)
            Me.n_BOKM_isCartella = oRequest.GetValueFromParameter(9)
            If IsDate(oRequest.GetValueFromParameter(10)) Then
                If Not Equals(oRequest.GetValueFromParameter(10), New Date) Then
                    Me.n_BOKM_Created = oRequest.GetValueFromParameter(10)
                End If
            End If
            If IsDate(oRequest.GetValueFromParameter(11)) Then
                If Not Equals(oRequest.GetValueFromParameter(11), New Date) Then
                    Me.n_BOKM_Modified = oRequest.GetValueFromParameter(11)
                End If
            End If
            Me.n_BOKM_Livello = oRequest.GetValueFromParameter(12)
            Me.n_Errore = Errori_Db.None
        Catch ex As Exception
            Me.n_BOKM_ID = -1
            Me.n_Errore = Errori_Db.DBReadExist
        End Try
    End Sub
    Public Function Aggiungi() As Integer
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim dsTable As Integer
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_BookMark_Aggiungi"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetParameter("@BOKM_ID", "", ParameterDirection.Output, DbType.Int32)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@BOKM_PRSN_Id", Me.n_BOKM_PRSN_ID, , DbType.Int32)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@BOKM_CMNT_ID", Me.n_BOKM_CMNT_ID, , DbType.Int32)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@BOKM_Descrizione", Me.n_BOKM_Descrizione, , DbType.String, True, 300)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@BOKM_Nome", Me.n_BOKM_Nome, , DbType.String, True, 250)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@BOKM_Url", Me.n_BOKM_Url, , DbType.String, True, 250)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@BOKM_OrdineV", Me.n_BOKM_OrdineV, , DbType.Int32)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@BOKM_Padre_Id", Me.n_BOKM_Padre_Id, , DbType.Int32)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@BOKM_isCartella", Me.n_BOKM_isCartella, , DbType.Byte)
            .Parameters.Add(oParam)
            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            dsTable = objAccesso.GetExecuteNotQuery(oRequest)
            Me.n_BOKM_ID = oRequest.GetValueFromParameter(1)
            Me.n_Errore = Errori_Db.None
        Catch ex As Exception
            ' Codice per il rollback
            Me.n_Errore = Errori_Db.DBInsert
            Me.n_BOKM_ID = -1
            dsTable = -1
        End Try
        Return dsTable
    End Function
    Public Function Modifica()
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim dsTable As Integer
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_BookMark_Modifica"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetParameter("@BOKM_ID", Me.n_BOKM_ID, ParameterDirection.Input, DbType.Int32)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@BOKM_Modified", "", ParameterDirection.Output, DbType.DateTime)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@BOKM_PRSN_Id", Me.n_BOKM_PRSN_ID, , DbType.Int32)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@BOKM_CMNT_ID", Me.n_BOKM_CMNT_ID, , DbType.Int32)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@BOKM_Descrizione", Me.n_BOKM_Descrizione, , DbType.String, True, 300)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@BOKM_Nome", Me.n_BOKM_Nome, , DbType.String, True, 250)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@BOKM_Url", Me.n_BOKM_Url, , DbType.String, True, 250)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@BOKM_OrdineV", Me.n_BOKM_OrdineV, , DbType.Int32)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@BOKM_Padre_Id", Me.n_BOKM_Padre_Id, , DbType.Int32)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@BOKM_isCartella", Me.n_BOKM_isCartella, , DbType.Byte)
            .Parameters.Add(oParam)
            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            dsTable = objAccesso.GetExecuteNotQuery(oRequest)
            Me.n_BOKM_Modified = oRequest.GetValueFromParameter(2)

            Me.n_Errore = Errori_Db.None
        Catch ex As Exception
            ' Codice per il rollback
            dsTable = -1
            Me.n_Errore = Errori_Db.DBChange
        End Try
        Return dsTable
    End Function

    Public Function HasComunitaAssociate() As Boolean
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim objAccesso As New COL_DataAccess
        With oRequest
            .Command = "sp_BookMark_HasComunitaAssociate"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetAdvancedParameter("@totale", "", ParameterDirection.Output, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@BOKM_ID", Me.n_BOKM_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@PRSN_ID", Me.n_BOKM_PRSN_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With

        Try
            objAccesso.GetExecuteNotQuery(oRequest)
            If oRequest.GetValueFromParameter(1) = "0" Then
                Return False
            Else
                Return True
            End If

        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Shared Function HasComunitaAssociate(ByVal ElementoID As Integer, ByVal PersonaID As Integer) As Boolean
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim objAccesso As New COL_DataAccess
        With oRequest
            .Command = "sp_BookMark_HasComunitaAssociate"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetAdvancedParameter("@totale", "", ParameterDirection.Output, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@BOKM_ID", ElementoID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@PRSN_ID", PersonaID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With

        Try
            objAccesso.GetExecuteNotQuery(oRequest)
            If oRequest.GetValueFromParameter(1) = "0" Then
                Return False
            Else
                Return True
            End If

        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Sub Cancella(ByVal ForReplica As Boolean)
        'carica i campi del db nell'oggetto in base all'id della persona
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim objAccesso As New COL_DataAccess
        With oRequest
            .Command = "sp_BookMark_Cancella"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetAdvancedParameter("@BOKM_ID", Me.n_BOKM_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            If ForReplica Then
                oParam = objAccesso.GetAdvancedParameter("@ForReplica", 1, ParameterDirection.Input, SqlDbType.Int)
            Else
                oParam = objAccesso.GetAdvancedParameter("@ForReplica", 0, ParameterDirection.Input, SqlDbType.Int)
            End If
            .Parameters.Add(oParam)

            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With

        Try
            objAccesso.GetExecuteNotQuery(oRequest)
            Me.n_Errore = Errori_Db.None
        Catch ex As Exception
            Me.n_BOKM_ID = -1
            Me.n_Errore = Errori_Db.DBDelete
        End Try
    End Sub

    Public Sub CancellaAllFor(ByVal PRSN_ID As Integer)
        'carica i campi del db nell'oggetto in base all'id della persona
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim objAccesso As New COL_DataAccess
        With oRequest
            .Command = "sp_BookMark_CancellaForUser"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetParameter("@PRSN_ID", PRSN_ID, ParameterDirection.Input, DbType.Int32)
            .Parameters.Add(oParam)

            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With

        Try
            objAccesso.GetExecuteNotQuery(oRequest)
            Me.n_Errore = Errori_Db.None
        Catch ex As Exception
            Me.n_BOKM_ID = -1
            Me.n_Errore = Errori_Db.DBDelete
        End Try
    End Sub


    'Public Function Sposta(ByVal newPadre_ID As Integer, ByVal posX As Integer, ByVal posY As Integer)
    '    Dim oRequest As New COL_Request
    '    Dim oParam As New COL_Request.Parameter
    '    Dim objAccesso As New COL_DataAccess

    '    With oRequest
    '        .Command = "sp_BookMark_Sposta"
    '        .CommandType = CommandType.StoredProcedure
    '        oParam = objAccesso.GetParameter("@BOKM_ID", Me.n_BOKM_ID, , DbType.Int32)
    '        .Parameters.Add(oParam)
    '        oParam = objAccesso.GetParameter("@BOKM_Padre_Id", Me.n_BOKM_Padre_Id, ParameterDirection.Input, DbType.Int32)
    '        .Parameters.Add(oParam)
    '        If newPadre_ID < 0 Then
    '            newPadre_ID = 0
    '        End If
    '        oParam = objAccesso.GetParameter("@Nuovo_Padre_Id", newPadre_ID, ParameterDirection.Input, DbType.Int32)
    '        .Parameters.Add(oParam)
    '        oParam = objAccesso.GetParameter("@BOKM_OrdineV", posY, ParameterDirection.Input, DbType.Int32)
    '        .Parameters.Add(oParam)
    '        oParam = objAccesso.GetParameter("@BOKM_OrdineO", posX, ParameterDirection.Input, DbType.Int32)
    '        .Parameters.Add(oParam)
    '        .Role = COL_Request.UserRole.Admin
    '        .transactional = False
    '    End With
    '    Try
    '        objAccesso.GetExecuteNotQuery(oRequest)
    '        Me.n_Errore = Errori_Db.None
    '    Catch ex As Exception
    '        Me.n_Errore = Errori_Db.DBChange
    '    End Try
    'End Function
#End Region

    Public Shared Function ElencaByUser(ByVal PRSN_Id As Integer) As DataSet
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim oDataSet As New DataSet
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_BookMark_ElencaByUser"
            .CommandType = CommandType.StoredProcedure

            oParam = objAccesso.GetParameter("@PRSN_Id", PRSN_Id, , DbType.Int32)
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

#Region "Gestione Import/export da Raccolta Linki Comunità"
    Public Function Importa(ByVal RLNK_ID As Integer, ByVal CMNT_ID As Integer, ByVal idCreatore As Integer) As Boolean
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim iResponse As Integer
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_BookMark_ImportaLink"
            .CommandType = CommandType.StoredProcedure

            oParam = objAccesso.GetParameter("@PRSN_Id", idCreatore, , DbType.Int32)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetParameter("@RLNK_ID", RLNK_ID, , DbType.Int32)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetParameter("@CMNT_ID", CMNT_ID, , DbType.Int32)
            .Parameters.Add(oParam)


            oParam = objAccesso.GetParameter("@BOKM_ID", "", ParameterDirection.Output, DbType.Int32)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@BOKM_PRSN_Id", "", ParameterDirection.Output, DbType.Int32)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@BOKM_CMNT_ID", "", ParameterDirection.Output, DbType.Int32)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@BOKM_Descrizione", "", ParameterDirection.Output, DbType.String, True, 300)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@BOKM_Nome", "", ParameterDirection.Output, DbType.String, True, 250)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@BOKM_Url", "", ParameterDirection.Output, DbType.String, True, 250)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@BOKM_OrdineV", "", ParameterDirection.Output, DbType.Int32)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@BOKM_Padre_Id", "", ParameterDirection.Output, DbType.Int32)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@BOKM_isCartella", "", ParameterDirection.Output, DbType.Byte)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@BOKM_Created", "", ParameterDirection.Output, DbType.DateTime)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@BOKM_Modified", "", ParameterDirection.Output, DbType.DateTime)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@BOKM_livello", "", ParameterDirection.Output, DbType.Int32)
            .Parameters.Add(oParam)

            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            iResponse = objAccesso.GetExecuteNotQuery(oRequest)

            Me.n_BOKM_ID = oRequest.GetValueFromParameter(4)
            Me.n_BOKM_PRSN_ID = oRequest.GetValueFromParameter(5)
            If IsNumeric(oRequest.GetValueFromParameter(6)) Then
                Me.n_BOKM_CMNT_ID = oRequest.GetValueFromParameter(6)
            End If
            Me.n_BOKM_Descrizione = oRequest.GetValueFromParameter(7)
            Me.n_BOKM_Nome = oRequest.GetValueFromParameter(8)
            Me.n_BOKM_Url = oRequest.GetValueFromParameter(9)
            Me.n_BOKM_OrdineV = oRequest.GetValueFromParameter(10)
            Me.n_BOKM_Padre_Id = oRequest.GetValueFromParameter(11)
            Me.n_BOKM_isCartella = oRequest.GetValueFromParameter(12)
            If IsDate(oRequest.GetValueFromParameter(13)) Then
                If Not Equals(oRequest.GetValueFromParameter(13), New Date) Then
                    Me.n_BOKM_Created = oRequest.GetValueFromParameter(13)
                End If
            End If
            If IsDate(oRequest.GetValueFromParameter(14)) Then
                If Not Equals(oRequest.GetValueFromParameter(14), New Date) Then
                    Me.n_BOKM_Modified = oRequest.GetValueFromParameter(14)
                End If
            End If
            Me.n_BOKM_Livello = oRequest.GetValueFromParameter(15)
            Me.n_Errore = Errori_Db.None
        Catch ex As Exception
            Me.n_Errore = Errori_Db.DBInsert
            Return False
        End Try
        Return True
    End Function
    Public Function CopiaForImporta(ByVal RLNK_ID As Integer, ByVal PadreId As Integer, ByVal idCreatore As Integer, ByVal ordineV As Integer) As Boolean
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim iResponse As Integer
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_BookMark_CopiaForImporta"
            .CommandType = CommandType.StoredProcedure

            oParam = objAccesso.GetParameter("@PRSN_Id", idCreatore, , DbType.Int32)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetParameter("@RLNK_ID", RLNK_ID, , DbType.Int32)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetParameter("@BOKM_ID", "", ParameterDirection.Output, DbType.Int32)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetParameter("@ordineV", ordineV, , DbType.Int32)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetParameter("@PadreId", PadreId, , DbType.Int32)
            .Parameters.Add(oParam)

            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            iResponse = objAccesso.GetExecuteNotQuery(oRequest)
            Me.n_BOKM_ID = oRequest.GetValueFromParameter(3)
            Me.n_Errore = Errori_Db.None
        Catch ex As Exception
            Me.n_Errore = Errori_Db.DBInsert
            Return False
        End Try
        Return True
    End Function

    Public Shared Function ListaForExport(ByVal PRSN_Id As Integer, ByVal CMNT_ID As Integer) As DataSet
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim oDataSet As New DataSet
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_BookMark_ElencaForImport"
            .CommandType = CommandType.StoredProcedure

            oParam = objAccesso.GetParameter("@PRSN_Id", PRSN_Id, , DbType.Int32)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetParameter("@CMNT_ID", CMNT_ID, , DbType.Int32)
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
#End Region

    Public Function GetComunitaAssociate() As DataSet
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim oDataSet As New DataSet
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_BookMark_GetComunitàAssociate"
            .CommandType = CommandType.StoredProcedure

            oParam = objAccesso.GetParameter("@BOKM_ID", Me.n_BOKM_ID, , DbType.Int32)
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



    ' NUOVI METODI 
    Public Function AlberoCartelle(ByVal PRSN_Id As Integer) As DataSet
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim oDataset As New DataSet
        Dim objAccesso As New COL_DataAccess

        Try
            With oRequest
                .Command = "sp_BookMark_AlberoCartelle"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@PRSN_Id", PRSN_Id, ParameterDirection.Input, SqlDbType.Int)
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

    Public Shared Function ElencaForOrdineVisualizzazione(ByVal PRSN_Id As Integer, ByVal PadreID As Integer, ByVal BOKM_ID As Integer) As DataSet
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim oDataset As New DataSet
        Dim objAccesso As New COL_DataAccess

        Try
            With oRequest
                .Command = "sp_BookMark_ElencaForOrdineVisualizzazione"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@PRSN_Id", PRSN_Id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PadreID", PadreID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@BOKM_ID", BOKM_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            oDataset = objAccesso.GetdataSet(oRequest)

        Catch ex As Exception

        End Try
        Return oDataset
    End Function

    Public Sub CancellaContenuto()
        'carica i campi del db nell'oggetto in base all'id della persona
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim objAccesso As New COL_DataAccess
        With oRequest
            .Command = "sp_BookMark_CancellaContenuto"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetAdvancedParameter("@BOKM_ID", Me.n_BOKM_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With

        Try
            objAccesso.GetExecuteNotQuery(oRequest)
            Me.n_Errore = Errori_Db.None
        Catch ex As Exception
            Me.n_Errore = Errori_Db.DBDelete
        End Try
    End Sub
End Class