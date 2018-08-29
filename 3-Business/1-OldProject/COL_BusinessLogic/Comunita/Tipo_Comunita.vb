Imports COL_DataLayer


Namespace Comunita
    Public Class COL_Tipo_Comunita
		Inherits ObjectBase

#Region "Private Property"
        Private n_TPCM_id As Integer
        Private n_TPCM_descrizione As String
        Private n_TPCM_icona As String
        Private n_TPCM_visibile As Integer
        Private n_errore As Errori_Db
#End Region

#Region "Public Property"
        Public Property ID() As Integer
            Get
                ID = n_TPCM_id
            End Get
            Set(ByVal Value As Integer)
                n_TPCM_id = Value
            End Set
        End Property
        Public Property Descrizione() As String
            Get
                Descrizione = n_TPCM_descrizione
            End Get
            Set(ByVal Value As String)
                n_TPCM_descrizione = Value
            End Set
        End Property
        Public Property Icona() As String
            Get
                Icona = n_TPCM_icona
            End Get
            Set(ByVal Value As String)
                n_TPCM_icona = Value
            End Set
        End Property
        Public Property isVisibile() As Boolean
            Get
                isVisibile = CBool(n_TPCM_visibile = 1)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    n_TPCM_visibile = 1
                Else
                    n_TPCM_visibile = 0
                End If
            End Set
        End Property

        Public ReadOnly Property ErroreDB() As Errori_Db
            Get
                ErroreDB = n_errore
            End Get
        End Property
#End Region

#Region "Metodi New"
        Sub New()
            Me.n_errore = Errori_Db.None
            Me.n_TPCM_visibile = 1
		End Sub
		Sub New(ByVal ID As Integer, ByVal nome As String)
			Me.n_errore = Errori_Db.None
			Me.n_TPCM_visibile = 1
			Me.n_TPCM_id = ID
			Me.n_TPCM_descrizione = nome
		End Sub
		Sub New(ByVal ID As Integer, ByVal nome As String, ByVal icona As String)
			Me.n_errore = Errori_Db.None
			Me.n_TPCM_visibile = 1
			Me.n_TPCM_id = ID
			Me.n_TPCM_descrizione = nome
			Me.n_TPCM_icona = icona
		End Sub
		Sub New(ByVal ID As Integer, ByVal nome As String, ByVal icona As String, ByVal Visibile As Boolean)
			Me.n_errore = Errori_Db.None
			Me.n_TPCM_visibile = 1
			Me.n_TPCM_id = ID
			Me.n_TPCM_descrizione = nome
			Me.isVisibile = Visibile
		End Sub
#End Region

#Region "Metodi Standard"
        Public Shared Function Elenca(ByVal LinguaID As Integer) As DataSet
            'elenca i tipi di comunità visibili. se passo 0 tutti i tipi, anche quelli non visibili
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_TipoComunita_elenca"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            dsTable = objAccesso.GetdataSet(oRequest)
            Return dsTable
        End Function
        Public Sub Estrai()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dstable As Integer
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_TipoComunita_Estrai"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@TPCM_ID", Me.n_TPCM_id, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPCM_Descrizione", "", ParameterDirection.Output, DbType.String, , 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPCM_icona", "", ParameterDirection.Output, DbType.String, , 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPCM_visibile", "", ParameterDirection.Output, DbType.Byte)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                dstable = objAccesso.GetExecuteNotQuery(oRequest)

                Me.n_TPCM_descrizione = oRequest.GetValueFromParameter(2)
                Me.n_TPCM_icona = oRequest.GetValueFromParameter(3)
                Me.n_TPCM_visibile = oRequest.GetValueFromParameter(4)
            Catch ex As Exception
                Me.n_errore = Errori_Db.DBError
            End Try
        End Sub
        Public Sub Aggiungi()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_TipoComunita_Aggiungi"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@TPCM_id", n_TPCM_id, ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPCM_descrizione", n_TPCM_descrizione.Trim, ParameterDirection.Input, DbType.String, , 100)
                .Parameters.Add(oParam)
                'oParam = objAccesso.GetParameter("@TPCM_icona", Me.n_TPCM_icona, ParameterDirection.Input, DbType.String, , 100)
                '.Parameters.Add(oParam)
                'oParam = objAccesso.GetParameter("@TPCM_visibile", Me.n_TPCM_visibile, ParameterDirection.Input, DbType.Byte)
                '.Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_TPCM_id = oRequest.GetValueFromParameter(1)
            Catch
                Me.n_TPCM_id = -1
            End Try
        End Sub
        Public Sub Elimina()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_TipoComunita_Elimina"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@TPCM_Id", Me.n_TPCM_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_errore = Errori_Db.None
            Catch ex As Exception
                Me.n_errore = Errori_Db.DBDelete
            End Try
        End Sub
        Public Sub Modifica()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_TipoComunita_Modifica"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@TPCM_ID", Me.n_TPCM_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPCM_descrizione", Me.n_TPCM_descrizione, ParameterDirection.Input, DbType.String, , 100)
                .Parameters.Add(oParam)
                'intanto si modificano solo queste due cose basilari

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_errore = Errori_Db.None
            Catch ex As Exception
                Me.n_errore = Errori_Db.DBChange
            End Try
        End Sub
#End Region

#Region "Metodi"
        Public Function ElencaServiziDefault() As DataSet
            'elenca i servizi disponibili per quel tipo di comunità di default
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As DataSet = Nothing
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Tipo_Comunita_ElencaServiziDefault"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@TPCM_id", n_TPCM_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
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
        Public Function ElencaModelli() As DataSet ' restituisce un dataset con i modelli che possono essere associati ad un tipo di comunità
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As DataSet = Nothing
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Tipo_Comunita_ElencaModelli"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@TPCM_id", n_TPCM_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
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

        Public Function ElencaCategorieFile(ByVal LinguaID As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As DataSet = Nothing
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_TipoComunita_ElencaCategorieFile"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@TPCM_id", n_TPCM_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

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
        Public Sub UpDateNomeFoto(ByVal newpath As String)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Tipo_Comunita_UpdateIcona"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@TPCM_ID", Me.ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPCM_icona", newpath, ParameterDirection.Input, DbType.String, True, 255)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_errore = Errori_Db.None
            Catch ex As Exception
                Me.n_errore = Errori_Db.DBReadExist
            End Try
        End Sub


        Public Function AssociaModello(ByVal MDCM_ID As Integer, Optional ByVal isDefault As Boolean = False) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_TipoComunita_AssociaModello"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@MDCM_ID", MDCM_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPCM_id", Me.n_TPCM_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@isDefault", isDefault, ParameterDirection.Input, DbType.Boolean)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)

                Me.n_errore = Errori_Db.None
                Return True
            Catch ex As Exception
                Me.n_errore = Errori_Db.DBDelete
            End Try
            Return False
        End Function

        Public Sub AssociaCategoriaFile(ByVal CTGR_ID As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_TipoComunita_AssociaCategoriaFile"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CTGR_ID", CTGR_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPCM_id", Me.n_TPCM_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)

                Me.n_errore = Errori_Db.None

            Catch ex As Exception
                Me.n_errore = Errori_Db.DBDelete
            End Try

        End Sub

        Public Sub AssociaServizio()
            'da creare
        End Sub

        'borsi yea!!!!
        Public Function ModelloDefault() As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim idModello As Integer
            With oRequest
                .Command = "sp_TipoComunita_ModelloDefault"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@TPCM_ID", Me.n_TPCM_id, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MDCM_ID", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                idModello = oRequest.GetValueFromParameter(2)
            Catch ex As Exception
                Me.n_errore = Errori_Db.DBInsert
            End Try
            Return idModello
        End Function




        Public Shared Function FindByOrganization(ByVal LinguaID As Integer, ByVal OrganizationID As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_TipoComunita_FindByOrganization"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@OrganizationID", OrganizationID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            dsTable = objAccesso.GetdataSet(oRequest)
            Return dsTable
        End Function

        Public Shared Function ElencaForFiltri(ByVal LinguaID As Integer, Optional ByVal ForAdmin As Boolean = False, Optional ByVal FromComunitaID As Integer = 0) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_TipoComunita_ElencaForFiltri"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                If ForAdmin Then
                    oParam = objAccesso.GetAdvancedParameter("@ForAdmin", 1, ParameterDirection.Input, SqlDbType.Int)
                Else
                    oParam = objAccesso.GetAdvancedParameter("@ForAdmin", 0, ParameterDirection.Input, SqlDbType.Int)
                End If
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FromComunitaID", FromComunitaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                ''oParam = objAccesso.GetAdvancedParameter("@FiltroRicerca", CType(oFiltro, Main.FiltroRicercaComunitaByIscrizione), ParameterDirection.Input, SqlDbType.Int)
                ''.Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            dsTable = objAccesso.GetdataSet(oRequest)
            Return dsTable
        End Function
        Public Shared Function ElencaSottoComunitaForCreation(ByVal TPCM_id As Integer, ByVal LinguaID As Integer) As DataSet
            'elenca i tipi di sotto comunità consentiti per quel tipo di comunita
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_TipoComunita_ElencaSottoComunitaForCreation"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@TPCM_ID", TPCM_id, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LinguaID", LinguaID, , DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            dsTable = objAccesso.GetdataSet(oRequest)
            Return dsTable
        End Function

        Public Shared Function ElencaSottoComunita(ByVal TPCM_id As Integer, ByVal LinguaID As Integer) As DataSet
            'elenca i tipi di sotto comunità associati e non
            'serve per l'admministrazione
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_TipoComunita_ElencaSottoComunita"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@TPCM_ID", TPCM_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@LinguaID", LinguaID, , DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            dsTable = objAccesso.GetdataSet(oRequest)
            Return dsTable
        End Function
		Public Shared Function ElencaSottoComunitaCreabili(ByVal TipoComunitaID As Integer, ByVal LinguaID As Integer) As DataSet
            'elenca i tipi di sotto comunità associati e non
            'serve per l'admministrazione
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_TipoComunita_ElencaSottoComunitaCreabili"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@TipoComunitaID", TipoComunitaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input , SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            dsTable = objAccesso.GetdataSet(oRequest)
            Return dsTable
        End Function
#End Region

#Region "Metodi Gestione TipiRuolo"
        Public Function ElencaTipoRuolo(ByVal LinguaID As Integer) As DataSet ' crea un dataset con i tipi di rouli con cui è possibile registrarsi per quel tipo di comunità
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_TipoComunita_ElencaTipoRuolo"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@TPCM_id", n_TPCM_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)


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

        Public Function ElencaTipoRuoloAssociati(ByVal LinguaID As Integer, ByVal oFiltro As FiltroRuoli) As DataSet ' crea un dataset con i tipi di rouli con cui è possibile registrarsi per quel tipo di comunità
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_TipoComunita_ElencaTipoRuoloAssociati"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@TPCM_id", n_TPCM_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@Filtro", CType(oFiltro, Main.FiltroRuoli), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

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


        Public Sub DefinisciSottoComunita(ByVal ListaSottoComunita As String)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_TipoComunita_DefinisciSottoComunita"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@ListaSottoComunita", ListaSottoComunita, ParameterDirection.Input, SqlDbType.VarChar, True, 4000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@TPCM_id", Me.n_TPCM_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_errore = Errori_Db.None
            Catch ex As Exception
                Me.n_errore = Errori_Db.DBChange
            End Try
        End Sub
        Public Sub DefinisciModelli(ByVal ListaModelli As String)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_TipoComunita_DefinisciModelli"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@ListaModelli", ListaModelli, ParameterDirection.Input, SqlDbType.VarChar, True, 4000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@TPCM_id", Me.n_TPCM_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_errore = Errori_Db.None
            Catch ex As Exception
                Me.n_errore = Errori_Db.DBChange
            End Try
        End Sub
        Public Sub DefinisciModelloDefault(ByVal ModelloID As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_TipoComunita_DefinisciModelloDefault"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@ModelloID", ModelloID, ParameterDirection.Input, SqlDbType.VarChar, True, 4000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@TPCM_id", Me.n_TPCM_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_errore = Errori_Db.None
            Catch ex As Exception
                Me.n_errore = Errori_Db.DBChange
            End Try
        End Sub
        Public Sub DefinisciCategorieFile(ByVal ListaCategorieFile As String)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_TipoComunita_DefinisciCategorieFile"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@ListaCategorieFile", ListaCategorieFile, ParameterDirection.Input, SqlDbType.VarChar, True, 4000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@TPCM_id", Me.n_TPCM_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_errore = Errori_Db.None
            Catch ex As Exception
                Me.n_errore = Errori_Db.DBChange
            End Try
        End Sub


        Public Function ServiziDefiniti(ByVal OrganizzazioneID As Integer, ByVal LinguaId As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim oDataSet As New DataSet

            With oRequest
                .Command = "sp_TipoComunita_ServiziDefiniti"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@LinguaId", LinguaId, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_Id", OrganizzazioneID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@TPCM_id", Me.n_TPCM_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                oDataSet = objAccesso.GetdataSet(oRequest)
                Me.n_errore = Errori_Db.None
                Return oDataSet
            Catch ex As Exception
                Me.n_errore = Errori_Db.DBReadExist
            End Try
            Return oDataSet
        End Function
        Public Function ServiziAssociati(ByVal OrganizzazioneID As Integer, ByVal LinguaId As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim oDataSet As New DataSet

            With oRequest
                .Command = "sp_TipoComunita_ServiziAssociati"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@LinguaId", LinguaId, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_Id", OrganizzazioneID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@TPCM_id", Me.n_TPCM_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                oDataSet = objAccesso.GetdataSet(oRequest)
                Me.n_errore = Errori_Db.None
                Return oDataSet
            Catch ex As Exception
                Me.n_errore = Errori_Db.DBReadExist
            End Try
            Return oDataSet
        End Function

        Public Function OrganizationServices(ByVal OrganizzazioneID As Integer, ByVal LinguaId As Integer) As List(Of COL_Servizio)

            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim list As New List(Of COL_Servizio)
            With oRequest
                .Command = "sp_TipoComunita_ServiziAssociati"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@LinguaId", LinguaId, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_Id", OrganizzazioneID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@TPCM_id", Me.n_TPCM_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                Using reader As IDataReader = objAccesso.GetdataReader(oRequest)
                    Dim service As COL_Servizio
                    While reader.Read
                        service = New COL_Servizio
                        With service
                            .ID = reader("SRVZ_id")
                            .Descrizione = IIf(IsDBNull(reader("SRVZ_descrizione")), "", reader("SRVZ_descrizione"))
                            .isNonDisattivabile = reader("isNonDisattivabile")
                            .isAttivato = reader("isAbilitato")
                            .Nome = reader("SRVZ_nome")
                        End With
                        list.Add(service)
                    End While

                    If Not IsNothing(reader) AndAlso reader.IsClosed = False Then
                        reader.Close()
                    End If
                End Using
            Catch ex As Exception

            End Try
            Return (From s In list Order By s.Nome Select s).ToList
        End Function
        Public Sub DefinisciServiziDisponibili(ByVal OrganizzazioneID As Integer, ByVal ListaServizi As String, ByVal Replica As Boolean)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_TipoComunita_DefinisciServiziDisponibili"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@ListaServizi", ListaServizi, ParameterDirection.Input, SqlDbType.VarChar, True, 4000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@TPCM_id", Me.n_TPCM_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_ID", OrganizzazioneID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                If Replica Then
                    oParam = objAccesso.GetAdvancedParameter("@Replica", 1, ParameterDirection.Input, SqlDbType.Int)
                Else
                    oParam = objAccesso.GetAdvancedParameter("@Replica", 0, ParameterDirection.Input, SqlDbType.Int)
                End If
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_errore = Errori_Db.None
            Catch ex As Exception
                Me.n_errore = Errori_Db.DBChange
            End Try
        End Sub
        Public Sub DefinisciServiziAttivati(ByVal OrganizzazioneID As Integer, ByVal ListaServizi As String, ByVal Replica As Boolean)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_TipoComunita_DefinisciServiziAttivati"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@ListaServizi", ListaServizi, ParameterDirection.Input, SqlDbType.VarChar, True, 4000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@TPCM_id", Me.n_TPCM_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_ID", OrganizzazioneID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                If Replica Then
                    oParam = objAccesso.GetAdvancedParameter("@Replica", 1, ParameterDirection.Input, SqlDbType.Int)
                Else
                    oParam = objAccesso.GetAdvancedParameter("@Replica", 0, ParameterDirection.Input, SqlDbType.Int)
                End If
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_errore = Errori_Db.None
            Catch ex As Exception
                Me.n_errore = Errori_Db.DBChange
            End Try
        End Sub
        Public Sub DefinisciServiziDefault(ByVal OrganizzazioneID As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_TipoComunita_DefinisciServiziDefault"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@TPCM_id", Me.n_TPCM_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_ID", OrganizzazioneID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_errore = Errori_Db.None
            Catch ex As Exception
                Me.n_errore = Errori_Db.DBChange
            End Try
        End Sub

        Public Sub DefinisciRuoli(ByVal ListaRuoli As String)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_TipoComunita_DefinisciRuoli"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@ListaRuoli", ListaRuoli, ParameterDirection.Input, SqlDbType.VarChar, True, 4000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@TPCM_id", Me.n_TPCM_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_errore = Errori_Db.None
            Catch ex As Exception
                Me.n_errore = Errori_Db.DBChange
            End Try
        End Sub
        Public Sub DefinisciRuoloDefault(ByVal RuoloID As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_TipoComunita_DefinisciRuoloDefault"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@TPCM_id", Me.n_TPCM_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@RuoloID", RuoloID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_errore = Errori_Db.None
            Catch ex As Exception
                Me.n_errore = Errori_Db.DBChange
            End Try
        End Sub
        Public Sub DefinisciRuoliProfiloObbligatori(ByVal ListaRuoli As String)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_TipoComunita_DefinisciRuoliProfiloObbligatori"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@ListaRuoli", ListaRuoli, ParameterDirection.Input, SqlDbType.VarChar, True, 4000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@TPCM_id", Me.n_TPCM_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_errore = Errori_Db.None
            Catch ex As Exception
                Me.n_errore = Errori_Db.DBChange
            End Try
        End Sub
        Public Sub DefinisciPermessiRuoli(ByVal OrganizzazioneID As Integer, ByVal ServizioID As Integer, ByVal RuoloId As Integer, ByVal StringaPermessi As String, ByVal Replica As Boolean)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_TipoComunita_DefinisciPermessiRuoli"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@StringaPermessi", StringaPermessi, ParameterDirection.Input, SqlDbType.VarChar, True, 32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@TPCM_id", Me.n_TPCM_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_ID", OrganizzazioneID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ServizioID", ServizioID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@TPRL_ID", RuoloId, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                If Replica Then
                    oParam = objAccesso.GetAdvancedParameter("@Replica", 1, ParameterDirection.Input, SqlDbType.Int)
                Else
                    oParam = objAccesso.GetAdvancedParameter("@Replica", 0, ParameterDirection.Input, SqlDbType.Int)
                End If
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                ObjectBase.PurgeCacheItems(CachePolicy.ServiziComunita)
                ObjectBase.PurgeCacheItems(CachePolicy.MenuComunita)
                ObjectBase.PurgeCacheItems(CachePolicy.RenderAllCommunity())
                Me.n_errore = Errori_Db.None
            Catch ex As Exception
                Me.n_errore = Errori_Db.DBChange
            End Try
        End Sub

        Public Sub DefinisciPermessiRuoliDefault(ByVal OrganizzazioneID As Integer, ByVal ServizioID As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_TipoComunita_DefinisciPermessiRuoliDefault"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@TPCM_id", Me.n_TPCM_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_ID", OrganizzazioneID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ServizioID", ServizioID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)

                ObjectBase.PurgeCacheItems(CachePolicy.ServiziComunita)
                ObjectBase.PurgeCacheItems(CachePolicy.MenuComunita)
                ObjectBase.PurgeCacheItems(CachePolicy.RenderAllCommunity())
                Me.n_errore = Errori_Db.None
            Catch ex As Exception
                Me.n_errore = Errori_Db.DBChange
            End Try
        End Sub

        Public Function ElencaDefinizioniLingue() As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim oDataSet As New DataSet

            With oRequest
                .Command = "sp_TipoComunita_ElencaDefinizioniLingue"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@TPCM_id", Me.n_TPCM_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                oDataSet = objAccesso.GetdataSet(oRequest)
                Me.n_errore = Errori_Db.None
                Return oDataSet
            Catch ex As Exception
                Me.n_errore = Errori_Db.DBReadExist
            End Try
            Return oDataSet
        End Function
        Public Sub Translate(ByVal Termine As String, ByVal LinguaID As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_TipoComunita_Translate"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@TPCM_id", Me.n_TPCM_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@Termine", Termine, ParameterDirection.Input, SqlDbType.VarChar, , 100)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_errore = Errori_Db.None
            Catch ex As Exception
                Me.n_errore = Errori_Db.DBChange
            End Try
        End Sub

#End Region

        Public Function GetLimitiAssociati_DaAssociare() As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As DataSet = Nothing
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Tipo_Comunita_ElencaLimitiAttivati_NonAttivati"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@TPCM_ID", Me.n_TPCM_id, , DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False

            End With
            Try
                dstable = objAccesso.GetdataSet(oRequest)
                Me.n_errore = Errori_Db.None
            Catch ex As Exception
                Me.n_errore = Errori_Db.DBError
            End Try
            Return dstable
        End Function
        Public Shared Function GetWithoutLimitiDefault() As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As DataSet = Nothing
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Tipo_Comunita_GetWithoutLimitiDefault"
                .CommandType = CommandType.StoredProcedure

                .Role = COL_Request.UserRole.Admin
                .transactional = False

            End With
            Try
                dstable = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception
            End Try
            Return dstable
        End Function
        Public Function EliminaLimitiAssociati() As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dstable As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_TipoComunita_EliminaLimiti"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@TPCM_ID", Me.n_TPCM_id, , DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False

            End With
            Try
                dstable = objAccesso.GetExecuteNotQuery(oRequest)
                ' Recupero l'ID univoco della persona.
                Me.n_errore = Errori_Db.None
            Catch ex As Exception
                dstable = -1
                Me.n_errore = Errori_Db.DBError
            End Try
            Return dstable
        End Function
        Public Function ElencaFromPossibiliPadri_ByServizio(ByVal PRSN_ID As Integer, ByVal CMNT_id As Integer, ByVal LinguaID As Integer, ByVal SRVZ_Codice As String, ByVal PRMS_posizione As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dstable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_TipoComunita_ElencaFromPossibiliPadri_ByServizio"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@PRSN_ID", PRSN_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@CMNT_id", CMNT_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@SRVZ_Codice", SRVZ_Codice, ParameterDirection.Input, SqlDbType.VarChar, True, 15)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRMS_posizione", PRMS_posizione, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)


                .Role = COL_Request.UserRole.Admin
                .transactional = False

            End With
            Try
                dstable = objAccesso.GetdataSet(oRequest)
                Me.n_errore = Errori_Db.None
            Catch ex As Exception
                Me.n_errore = Errori_Db.DBError
            End Try
            Return dstable
        End Function

        Public Function ElencaDefinizioneServizi(ByVal servizioID As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dstable As New DataSet
            Dim objAccesso As New COL_DataAccess


            With oRequest
                .Command = "sp_TipoComunita_DefinitoByServizioByOrganizzazione"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@TPCM_ID", Me.n_TPCM_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@SRVZ_ID", servizioID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False

            End With
            Try
                dstable = objAccesso.GetdataSet(oRequest)
                Me.n_errore = Errori_Db.None
            Catch ex As Exception
                Me.n_errore = Errori_Db.DBError
            End Try
            Return dstable
        End Function



		Private Shared Function RetrievePlainFromDB(ByVal LinguaID As Integer) As List(Of COL_Tipo_Comunita)
			Dim oLista As New List(Of COL_Tipo_Comunita)
			Dim oRequest As New COL_Request
			Dim oParam As New COL_Request.Parameter
			Dim objAccesso As New COL_DataAccess
			Dim oDatareader As IDataReader

			With oRequest
				.Command = "sp_TipoComunita_PlainLista"
				.CommandType = CommandType.StoredProcedure

				oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
				.Parameters.Add(oParam)

				.Role = COL_Request.UserRole.Admin
				.transactional = False
			End With

			Try
				oDatareader = objAccesso.GetdataReader(oRequest)
				While oDatareader.Read
					oLista.Add(New COL_Tipo_Comunita(oDatareader("TPCM_ID"), GenericValidator.ValString(oDatareader("TPCM_descrizione"), ""), _
	  GenericValidator.ValString(oDatareader("TPCM_icona"), ""), GenericValidator.ValBool(oDatareader("TPCM_visibile"), True)))
				End While
			Catch ex As Exception
			Finally
				If oDatareader.IsClosed = False Then
					oDatareader.Close()
				End If
			End Try
			Return oLista
		End Function

		Public Shared Function PlainLista(ByVal LinguaID As Integer, Optional ByVal ForAdmin As Boolean = False, Optional ByVal sortExpression As String = "", Optional ByVal sortDirection As String = "") As List(Of COL_Tipo_Comunita)
			Dim oLista As New List(Of COL_Tipo_Comunita)
			Dim cacheKey As String = CachePolicy.TipiComunita(LinguaID)

			If ObjectBase.Cache(cacheKey) Is Nothing Then
				oLista = COL_Tipo_Comunita.RetrievePlainFromDB(LinguaID)
				ObjectBase.Cache.Insert(cacheKey, oLista, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza60minuti)
			Else
				oLista = CType(ObjectBase.Cache(cacheKey), List(Of COL_Tipo_Comunita))
			End If
			If Not ForAdmin Then
				Dim iLista As New List(Of COL_Tipo_Comunita)
				iLista = oLista.FindAll((New GenericPredicate(Of COL_Tipo_Comunita, Boolean)(True, AddressOf FindByVisibilita)))
				oLista = iLista
			End If

			If sortDirection <> String.Empty Then
				sortDirection = sortDirection.ToLower
			End If
			If (Not sortExpression Is Nothing AndAlso sortDirection <> String.Empty) Then
				oLista.Sort(New GenericComparer(Of COL_Tipo_Comunita)(sortExpression))
			End If

			If (Not sortDirection Is Nothing AndAlso sortDirection = "desc") Then
				oLista.Reverse()
			End If


			Return (oLista)
		End Function
		Private Shared Function FindByVisibilita(ByVal item As COL_Tipo_Comunita, ByVal argument As Boolean) As Boolean
			Return (argument = item.isVisibile)
		End Function
    End Class
End Namespace