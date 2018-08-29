Imports COL_DataLayer
Imports COL_BusinessLogic_v2.Comunita

Public Class COL_ProfiloServizio

#Region "Private Property"
    Private _PRFS_ID As Integer
    Private _PRFS_PRSN_ID As Integer
    Private _PRFS_CreatoreID As Integer
    Private _PRFS_Nome As String
    Private _PRFS_Descrizione As String
    Private _PRFS_TPCM_ID As Integer
    Private _PRFS_CreatoIl As DateTime
    Private _PRFS_ModificatoIl As DateTime
    Private _Errore As Errori_Db
#End Region

#Region "Public Property"
    Public Property Id() As Integer
        Get
            Id = _PRFS_ID
        End Get
        Set(ByVal Value As Integer)
            _PRFS_ID = Value
        End Set
    End Property
    Public Property CreatoreID() As Integer
        Get
            CreatoreID = _PRFS_CreatoreID
        End Get
        Set(ByVal Value As Integer)
            _PRFS_CreatoreID = Value
        End Set
    End Property
    Public Property TipoComunitaID() As Integer
        Get
            TipoComunitaID = _PRFS_TPCM_ID
        End Get
        Set(ByVal Value As Integer)
            _PRFS_TPCM_ID = Value
        End Set
    End Property
    Public Property ModificatoDa() As Integer
        Get
            ModificatoDa = _PRFS_PRSN_ID
        End Get
        Set(ByVal Value As Integer)
            _PRFS_PRSN_ID = Value
        End Set
    End Property
    Public ReadOnly Property CreatoIl() As DateTime
        Get
            CreatoIl = _PRFS_CreatoIl
        End Get
    End Property
    Public ReadOnly Property ModificatoIl() As DateTime
        Get
            ModificatoIl = _PRFS_ModificatoIl
        End Get
    End Property

    Public Property Nome() As String
        Get
            Nome = _PRFS_Nome
        End Get
        Set(ByVal Value As String)
            _PRFS_Nome = Value
        End Set
    End Property
    Public Property Descrizione() As String
        Get
            Descrizione = _PRFS_Descrizione
        End Get
        Set(ByVal Value As String)
            _PRFS_Descrizione = Value
        End Set
    End Property
    Public ReadOnly Property Errore() As Errori_Db
        Get
            Errore = _Errore
        End Get
    End Property
#End Region

#Region "Metodi New"
    Sub New()
        Me._Errore = Errori_Db.None
    End Sub
#End Region

#Region "Metodi Standard"
    Public Sub Estrai()
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim objAccesso As New COL_DataAccess
        Dim oIstituzione As New COL_Istituzione
        With oRequest
            .Command = "sp_ProfiloServizio_Estrai"
            .CommandType = CommandType.StoredProcedure

            oParam = objAccesso.GetAdvancedParameter("@PRFS_ID", _PRFS_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetAdvancedParameter("@PRFS_PRSN_ID", "", ParameterDirection.Output, SqlDbType.Int)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetAdvancedParameter("@PRFS_CreatoreID", "", ParameterDirection.Output, SqlDbType.Int)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetAdvancedParameter("@PRFS_Nome", "", ParameterDirection.Output, SqlDbType.VarChar, , 120)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetAdvancedParameter("@PRFS_Descrizione", "", ParameterDirection.Output, SqlDbType.VarChar, , 400)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetAdvancedParameter("@PRFS_TPCM_ID", "", ParameterDirection.Output, SqlDbType.Int)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetAdvancedParameter("@PRFS_CreatoIl", "", ParameterDirection.Output, SqlDbType.DateTime)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetAdvancedParameter("@PRFS_ModificatoIl", "", ParameterDirection.Output, SqlDbType.DateTime)
            .Parameters.Add(oParam)

            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            objAccesso.GetExecuteNotQuery(oRequest)

            Me._PRFS_PRSN_ID = oRequest.GetValueFromParameter(2)
            Me._PRFS_CreatoreID = oRequest.GetValueFromParameter(3)
            Me._PRFS_Nome = oRequest.GetValueFromParameter(4)
            Me._PRFS_Descrizione = oRequest.GetValueFromParameter(5)
            Me._PRFS_TPCM_ID = oRequest.GetValueFromParameter(6)

            Try
                If IsDate(oRequest.GetValueFromParameter(7)) Then
                    Me._PRFS_CreatoIl = CDate(oRequest.GetValueFromParameter(7))
                End If
            Catch ex As Exception

            End Try
            Try
                If IsDate(oRequest.GetValueFromParameter(8)) Then
                    Me._PRFS_ModificatoIl = CDate(oRequest.GetValueFromParameter(8))
                End If
            Catch ex As Exception

            End Try
            Me._Errore = Errori_Db.None
        Catch ex As Exception
            Me._Errore = Errori_Db.DBReadExist
        End Try
    End Sub
    Public Sub Aggiungi()
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim objAccesso As New COL_DataAccess
        Dim DataCreazione As String
        DataCreazione = DateToString(Now)

        Me._PRFS_CreatoIl = Now
        With oRequest
            .Command = "sp_ProfiloServizio_Aggiungi"
            .CommandType = CommandType.StoredProcedure

            oParam = objAccesso.GetAdvancedParameter("@PRFS_ID", "", ParameterDirection.Output, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@PRFS_PRSN_ID", _PRFS_PRSN_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@PRFS_CreatoreID", _PRFS_CreatoreID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@PRFS_Nome", _PRFS_Nome, ParameterDirection.Input, SqlDbType.VarChar, False, 120)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@PRFS_Descrizione", _PRFS_Descrizione, ParameterDirection.Input, SqlDbType.VarChar, True, 400)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@PRFS_TPCM_ID", _PRFS_TPCM_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@PRFS_CreatoIl", _PRFS_CreatoIl, ParameterDirection.Input, SqlDbType.DateTime)
            .Parameters.Add(oParam)

            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            objAccesso.GetExecuteNotQuery(oRequest)
            Me._PRFS_ID = oRequest.GetValueFromParameter(1)
            Me._Errore = Errori_Db.None
        Catch ex As Exception
            Me._PRFS_ID = -1
            Me._Errore = Errori_Db.DBInsert
        End Try
    End Sub
    Public Sub Modifica()
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim objAccesso As New COL_DataAccess

        Me._PRFS_ModificatoIl = Now
        With oRequest
            .Command = "sp_ProfiloServizio_Modifica"
            .CommandType = CommandType.StoredProcedure

            oParam = objAccesso.GetAdvancedParameter("@PRFS_ID", _PRFS_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@PRFS_PRSN_ID", _PRFS_PRSN_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@PRFS_CreatoreID", _PRFS_CreatoreID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@PRFS_Nome", _PRFS_Nome, ParameterDirection.Input, SqlDbType.VarChar, False, 120)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@PRFS_Descrizione", _PRFS_Descrizione, ParameterDirection.Input, SqlDbType.VarChar, True, 400)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@PRFS_TPCM_ID", _PRFS_TPCM_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@PRFS_ModificatoIl", _PRFS_ModificatoIl, ParameterDirection.Input, SqlDbType.DateTime)
            .Parameters.Add(oParam)

            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            objAccesso.GetExecuteNotQuery(oRequest)
            Me._Errore = Errori_Db.None
        Catch ex As Exception
            Me._Errore = Errori_Db.DBChange
        End Try
    End Sub
    Public Sub Cancella()
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim objAccesso As New COL_DataAccess
        With oRequest
            .Command = "sp_ProfiloServizio_Cancella"
            .CommandType = CommandType.StoredProcedure

            oParam = objAccesso.GetAdvancedParameter("@PRFS_ID", _PRFS_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            objAccesso.GetExecuteNotQuery(oRequest)
            Me._Errore = Errori_Db.None
        Catch ex As Exception
            Me._Errore = Errori_Db.DBDelete
        End Try
    End Sub

    Public Function Elenca(ByVal PersonaID As Integer, ByVal LinguaID As Integer) As DataSet
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim dsTable As DataSet = Nothing
        Dim objAccesso As New COL_DataAccess
        With oRequest
            .Command = "sp_ProfiloServizio_Elenca"
            .CommandType = CommandType.StoredProcedure

            oParam = objAccesso.GetAdvancedParameter("@PersonaID", PersonaID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            dsTable = objAccesso.GetdataSet(oRequest)
        Catch ex As Exception
            Me._Errore = Errori_Db.DBError
        End Try
        Return dsTable
    End Function
    Public Function Elenca(ByVal PersonaID As Integer, ByVal LinguaID As Integer, ByVal TipoComunitaID As Integer) As DataSet
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim dsTable As DataSet = Nothing
        Dim objAccesso As New COL_DataAccess
        With oRequest
            .Command = "sp_ProfiloServizio_Elenca"
            .CommandType = CommandType.StoredProcedure

            oParam = objAccesso.GetAdvancedParameter("@PersonaID", PersonaID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetAdvancedParameter("@TipoComunitaID", TipoComunitaID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            dsTable = objAccesso.GetdataSet(oRequest)
        Catch ex As Exception
            Me._Errore = Errori_Db.DBError
        End Try
        Return dsTable
    End Function
    Public Function ElencaByComunita(ByVal ComunitaID As Integer, ByVal AdminID As Integer, ByVal ResponsabileID As Integer, ByVal LinguaID As Integer, ByVal TipoComunitaID As Integer) As DataSet
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim dsTable As DataSet = Nothing
        Dim objAccesso As New COL_DataAccess
        With oRequest
            .Command = "sp_ProfiloServizio_ElencaByComunita"
            .CommandType = CommandType.StoredProcedure

            oParam = objAccesso.GetAdvancedParameter("@PersonaID", AdminID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetAdvancedParameter("@TipoComunitaID", TipoComunitaID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetAdvancedParameter("@ResponsabileID", ResponsabileID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetAdvancedParameter("@ComunitaID", ComunitaID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            dsTable = objAccesso.GetdataSet(oRequest)
        Catch ex As Exception
            Me._Errore = Errori_Db.DBError
        End Try
        Return dsTable
    End Function
#End Region

#Region "Nuova Gestione Servizi"
    Public Sub DefinisciRuoli(ByVal ListaRuoli As String)
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_ProfiloServizio_DefinisciRuoli"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetAdvancedParameter("@ListaRuoli", ListaRuoli, ParameterDirection.Input, SqlDbType.VarChar, True, 4000)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@PRFS_ID", Me._PRFS_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@TPCM_id", Me._PRFS_TPCM_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            objAccesso.GetExecuteNotQuery(oRequest)
            Me._Errore = Errori_Db.None
        Catch ex As Exception
            Me._Errore = Errori_Db.DBChange
        End Try
    End Sub
    Public Function ElencaTipoRuolo(ByVal LinguaID As Integer) As DataSet ' crea un dataset con i tipi di rouli con cui è possibile registrarsi per quel tipo di comunità
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim dsTable As DataSet = Nothing
        Dim objAccesso As New COL_DataAccess
        With oRequest
            .Command = "sp_ProfiloServizio_ElencaTipoRuolo"
            .CommandType = CommandType.StoredProcedure

            oParam = objAccesso.GetAdvancedParameter("@TPCM_id", _PRFS_TPCM_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetAdvancedParameter("@PRFS_ID", Me._PRFS_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            dsTable = objAccesso.GetdataSet(oRequest)
        Catch ex As Exception
            Me._Errore = Errori_Db.DBError
        End Try
        Return dsTable
    End Function

    Public Function ElencaServiziDefiniti(ByVal LinguaId As Integer) As DataSet
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim objAccesso As New COL_DataAccess
        Dim oDataSet As New DataSet

        With oRequest
            .Command = "sp_ProfiloServizio_ElencaServiziDefiniti"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetAdvancedParameter("@LinguaId", LinguaId, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@PRFS_ID", _PRFS_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@TPCM_id", Me._PRFS_TPCM_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            oDataSet = objAccesso.GetdataSet(oRequest)
            Me._Errore = Errori_Db.None
            Return oDataSet
        Catch ex As Exception
            Me._Errore = Errori_Db.DBReadExist
        End Try
        Return oDataSet
    End Function
    Public Function ElencaServiziAssociati(ByVal LinguaId As Integer) As DataSet
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim objAccesso As New COL_DataAccess
        Dim oDataSet As New DataSet

        With oRequest
            .Command = "sp_ProfiloServizio_ElencaServiziAssociati"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetAdvancedParameter("@LinguaId", LinguaId, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@PRFS_ID", _PRFS_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@TPCM_id", Me._PRFS_TPCM_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            oDataSet = objAccesso.GetdataSet(oRequest)
            Return oDataSet
        Catch ex As Exception
            Me._Errore = Errori_Db.DBReadExist
        End Try
        Return oDataSet
    End Function

    Public Function ElencaServiziByComunita(ByVal LinguaId As Integer, ByVal TipoComunitaID As Integer, ByVal ComunitaID As Integer, ByVal OrganizzazioneID As Integer) As DataSet
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim objAccesso As New COL_DataAccess
        Dim oDataSet As New DataSet

        With oRequest
            .Command = "sp_ProfiloServizio_ElencaServiziByComunita"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetAdvancedParameter("@LinguaId", LinguaId, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@ProfiloID", _PRFS_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@TipoComunitaID", TipoComunitaID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@ComunitaID", ComunitaID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@OrganizzazioneID", OrganizzazioneID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            oDataSet = objAccesso.GetdataSet(oRequest)
            Return oDataSet
        Catch ex As Exception
            Me._Errore = Errori_Db.DBReadExist
        End Try
        Return oDataSet
    End Function
    Public Function ElencaPermessi(ByVal ServizioID As Integer, ByVal TipoComunitaID As Integer, ByVal LinguaID As Integer) As DataSet
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim objAccesso As New COL_DataAccess
        Dim oDataSet As New DataSet

        With oRequest
            .Command = "sp_ProfiloServizio_ElencaPermessi"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetAdvancedParameter("@SRVZ_ID", ServizioID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@PRFS_ID", _PRFS_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@TPCM_id", TipoComunitaID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            oDataSet = objAccesso.GetdataSet(oRequest)
            Return oDataSet
        Catch ex As Exception
            Me._Errore = Errori_Db.DBReadExist
        End Try
        Return oDataSet
    End Function

    'Public Sub DefinisciServiziDisponibili(ByVal ListaServizi As String)
    '    Dim oRequest As New COL_Request
    '    Dim oParam As New COL_Request.Parameter
    '    Dim objAccesso As New COL_DataAccess

    '    With oRequest
    '        .Command = "sp_ProfiloServizio_DefinisciServiziDisponibili"
    '        .CommandType = CommandType.StoredProcedure
    '        oParam = objAccesso.GetAdvancedParameter("@ListaServizi", ListaServizi, ParameterDirection.Input, SqlDbType.VarChar, True, 4000)
    '        .Parameters.Add(oParam)
    '        oParam = objAccesso.GetAdvancedParameter("@PRFS_ID", _PRFS_ID, ParameterDirection.Input, SqlDbType.Int)
    '        .Parameters.Add(oParam)
    '        oParam = objAccesso.GetAdvancedParameter("@TPCM_id", Me._PRFS_TPCM_ID, ParameterDirection.Input, SqlDbType.Int)
    '        .Parameters.Add(oParam)
    '        .Role = COL_Request.UserRole.Admin
    '        .transactional = False
    '    End With
    '    Try
    '        objAccesso.GetExecuteNotQuery(oRequest)
    '        Me._Errore = Errori_Db.None
    '    Catch ex As Exception
    '        Me._Errore = Errori_Db.DBChange
    '    End Try
    'End Sub
    Public Sub DefinisciServiziAttivati(ByVal ListaServizi As String)
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_ProfiloServizio_DefinisciServiziAttivati"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetAdvancedParameter("@ListaServizi", ListaServizi, ParameterDirection.Input, SqlDbType.VarChar, True, 4000)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@PRFS_ID", _PRFS_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@TPCM_id", Me._PRFS_TPCM_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            objAccesso.GetExecuteNotQuery(oRequest)
            Me._Errore = Errori_Db.None
        Catch ex As Exception
            Me._Errore = Errori_Db.DBChange
        End Try
    End Sub


    Public Sub DefinisciPermessiRuoli(ByVal ServizioID As Integer, ByVal RuoloId As Integer, ByVal StringaPermessi As String, ByVal Replica As Boolean)
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_ProfiloServizio_DefinisciPermessiRuoli"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetAdvancedParameter("@StringaPermessi", StringaPermessi, ParameterDirection.Input, SqlDbType.VarChar, True, 32)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@TPCM_id", Me._PRFS_TPCM_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@PRFS_ID", _PRFS_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@ServizioID", ServizioID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@TPRL_ID", RuoloId, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)


            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            objAccesso.GetExecuteNotQuery(oRequest)
            Me._Errore = Errori_Db.None
        Catch ex As Exception
            Me._Errore = Errori_Db.DBChange
        End Try
    End Sub
    Public Sub DefinisciPermessiRuoliDefault(ByVal ServizioID As Integer)
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_ProfiloServizio_DefinisciPermessiRuoliDefault"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetAdvancedParameter("@TPCM_id", Me._PRFS_TPCM_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@PRFS_ID", _PRFS_ID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@ServizioID", ServizioID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            objAccesso.GetExecuteNotQuery(oRequest)
            Me._Errore = Errori_Db.None
        Catch ex As Exception
            Me._Errore = Errori_Db.DBChange
        End Try
    End Sub

#End Region

End Class