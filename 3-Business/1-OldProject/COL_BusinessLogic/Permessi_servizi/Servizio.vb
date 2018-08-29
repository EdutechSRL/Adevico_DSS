Imports COL_DataLayer

Namespace CL_permessi
    Public Class COL_Servizio
		Inherits ObjectBase

#Region "Private Property"
        Private n_SRVZ_id As Integer
        Private n_SRVZ_Nome As String
        'Private n_SRVZ_link As String
        Private n_SRVZ_codice As String
        Private n_SRVZ_Descrizione As String
        Private n_SRVZ_padre As Integer
        Private n_SRVZ_nonDisattivabile As Integer
        Private n_SRVZ_Attivato As Integer
        Private _isNotificabile As Integer
        Private n_Errore As Errori_Db
#End Region

#Region "Public Property"
        Public Property ID() As Integer
            Get
                ID = n_SRVZ_id
            End Get
            Set(ByVal Value As Integer)
                n_SRVZ_id = Value
            End Set
        End Property
        Public Property Nome() As String
            Get
                Nome = n_SRVZ_Nome
            End Get
            Set(ByVal Value As String)
                n_SRVZ_Nome = Value
            End Set
        End Property
        Public Property Codice() As String
            Get
                Codice = n_SRVZ_codice
            End Get
            Set(ByVal Value As String)
                n_SRVZ_codice = Value
            End Set
        End Property
        Public Property Descrizione() As String
            Get
                Descrizione = n_SRVZ_Descrizione
            End Get
            Set(ByVal Value As String)
                n_SRVZ_Descrizione = Value
            End Set
        End Property
        Public Property isNonDisattivabile() As Boolean
            Get
                isNonDisattivabile = CBool(n_SRVZ_nonDisattivabile = 1)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    n_SRVZ_nonDisattivabile = 1
                Else
                    n_SRVZ_nonDisattivabile = 0
                End If
            End Set
        End Property
        Public Property isAttivato() As Boolean
            Get
                isAttivato = CBool(n_SRVZ_Attivato = 1)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    n_SRVZ_Attivato = 1
                Else
                    n_SRVZ_Attivato = 0
                End If
            End Set
        End Property
        Public Property isNotificabile() As Boolean
            Get
                isNotificabile = CBool(_isNotificabile = 1)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    _isNotificabile = 1
                Else
                    _isNotificabile = 0
                End If
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
            Me._isNotificabile = 0
            Me.n_SRVZ_nonDisattivabile = 0
            Me.n_Errore = Errori_Db.None
            Me.n_SRVZ_padre = 0
            Me.n_SRVZ_Attivato = 1
		End Sub
		Sub New(ByVal ID As Integer)
			Me.n_SRVZ_id = ID
			Me._isNotificabile = 0
			Me.n_SRVZ_nonDisattivabile = 0
			Me.n_Errore = Errori_Db.None
			Me.n_SRVZ_padre = 0
			Me.n_SRVZ_Attivato = 1
		End Sub
		Sub New(ByVal ID As Integer, ByVal Codice As String, ByVal isAttivato As Boolean)
			Me.n_SRVZ_id = ID
			Me._isNotificabile = 0
			Me.n_SRVZ_nonDisattivabile = 0
			Me.n_Errore = Errori_Db.None
			Me.n_SRVZ_padre = 0
			If isAttivato Then
				Me.n_SRVZ_Attivato = 1
			Else
				Me.n_SRVZ_Attivato = 0
			End If
			Me.n_SRVZ_codice = Codice
		End Sub
#End Region

#Region "Metodi Standard"
        'Public Shared Function Elenca() As DataSet
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim dsTable As new DataSet
        '    Dim objAccesso As New COL_DataAccess
        '    With oRequest
        '        .Command = "sp_Servizio_ElencaByTipoComunita"
        '        .CommandType = CommandType.StoredProcedure
        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        dsTable = objAccesso.GetdataSet(oRequest)
        '    Catch ex As Exception

        '    End Try
        '    Return dsTable
        'End Function
        Public Shared Function LoadByLanguage(ByVal IdLanguage) As List(Of COL_Servizio)

            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim list As New List(Of COL_Servizio)
            With oRequest
                .Command = "sp_Servizio_ElencaByLingua"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@LinguaId", IdLanguage, ParameterDirection.Input, DbType.Int32)
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
      
        Public Shared Function ElencaByLingua(ByVal LinguaId As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Servizio_ElencaByLingua"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@LinguaId", LinguaId, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                dsTable = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception

            End Try
            Return dsTable
        End Function
        Public Shared Function ElencaForAdmin(ByVal LinguaId As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Servizio_ElencaForAdmin"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@LinguaId", LinguaId, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                dsTable = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception

            End Try
            Return dsTable
        End Function
        Public Shared Function ElencaByTipoComunita(ByVal TipoComunitaID As Integer, ByVal OrganizzazioneID As Integer, ByVal LinguaID As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Servizio_ElencaByTipoComunita"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@TipoComunitaID", TipoComunitaID, ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@OrganizzazioneID", OrganizzazioneID, ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                dsTable = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception

            End Try
            Return dsTable
        End Function

        Public Shared Function ElencaByTipoComunitaTotali(ByVal TipoComunitaID As Integer, ByVal LinguaID As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Servizio_ElencaByTipoComunitaTotali"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@TipoComunitaID", TipoComunitaID, ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                dsTable = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception

            End Try
            Return dsTable
        End Function

        Public Shared Function ElencaByComunita(ByVal ComunitaID As Integer, ByVal LinguaID As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Servizio_ElencaByComunita"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@ComunitaID", ComunitaID, ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                dsTable = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception

            End Try
            Return dsTable
        End Function
        'Public Function Estrai() As String
        '    ' estrai i dati dalla tabella
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim objAccesso As New COL_DataAccess

        '    With oRequest
        '        .Command = "sp_Servizio_Estrai"
        '        .CommandType = CommandType.StoredProcedure
        '        oParam = objAccesso.GetParameter("@SRVZ_Id", Me.n_SRVZ_id, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@SRVZ_Nome", "", ParameterDirection.Output, DbType.String, , 50)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@SRVZ_codice", "", ParameterDirection.Output, DbType.String, , 15)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@SRVZ_descrizione", "", ParameterDirection.Output, DbType.String, , 255)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@SRVZ_padre", "", ParameterDirection.Output, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@SRVZ_nonDisattivabile", "", ParameterDirection.Output, DbType.Byte)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@SRVZ_Attivato", "", ParameterDirection.Output, DbType.Byte)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@SRVZ_isNotificabile", "", ParameterDirection.Output, DbType.Byte)
        '        .Parameters.Add(oParam)
        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        objAccesso.GetExecuteNotQuery(oRequest)
        '        Me.n_SRVZ_Nome = oRequest.GetValueFromParameter(2)
        '        Me.n_SRVZ_codice = oRequest.GetValueFromParameter(3)
        '        Me.n_SRVZ_Descrizione = oRequest.GetValueFromParameter(4)
        '        Me.n_SRVZ_padre = oRequest.GetValueFromParameter(5)
        '        Me.n_SRVZ_nonDisattivabile = oRequest.GetValueFromParameter(6)
        '        Me.n_SRVZ_Attivato = oRequest.GetValueFromParameter(7)
        '        Me._isNotificabile = oRequest.GetValueFromParameter(8)
        '        Me.n_Errore = Errori_Db.None
        '    Catch ex As Exception
        '        Me.n_Errore = Errori_Db.DBReadExist
        '    End Try

        'End Function
        Public Sub Aggiungi()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Servizio_Aggiungi"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@SRVZ_Id", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@SRVZ_Nome", n_SRVZ_Nome.Trim, ParameterDirection.Input, SqlDbType.VarChar, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@SRVZ_codice", n_SRVZ_codice, ParameterDirection.Input, SqlDbType.VarChar, , 15)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@SRVZ_descrizione", n_SRVZ_Descrizione.Trim, ParameterDirection.Input, SqlDbType.VarChar, , 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@SRVZ_padre", n_SRVZ_padre, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@SRVZ_nonDisattivabile", n_SRVZ_nonDisattivabile, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@SRVZ_Attivato", n_SRVZ_Attivato, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@SRVZ_isNotificabile", Me._isNotificabile, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_SRVZ_id = oRequest.GetValueFromParameter(1)
                Me.n_Errore = Errori_Db.None
            Catch
                Me.n_Errore = Errori_Db.DBInsert
                Me.n_SRVZ_id = -1
            End Try
        End Sub
        Public Sub Modifica()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Servizio_Modifica"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@SRVZ_Id", n_SRVZ_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@SRVZ_Nome", n_SRVZ_Nome, ParameterDirection.Input, SqlDbType.VarChar, , 50)
                .Parameters.Add(oParam)
                'oParam = objAccesso.GetParameter("@SRVZ_link", n_SRVZ_link.Trim)
                '.Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@SRVZ_codice", n_SRVZ_codice, ParameterDirection.Input, SqlDbType.VarChar, , 15)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@SRVZ_descrizione", n_SRVZ_Descrizione, ParameterDirection.Input, SqlDbType.VarChar, , 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@SRVZ_padre", n_SRVZ_padre, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@SRVZ_nonDisattivabile", Me.n_SRVZ_nonDisattivabile, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@SRVZ_Attivato", n_SRVZ_Attivato, ParameterDirection.Input, DbType.Byte)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@SRVZ_isNotificabile", Me._isNotificabile, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBChange
            End Try
        End Sub
        Public Sub Elimina()
            'cancella il servizio ed ogni suo riferimendo dal database!!!
            'attenzione!!!
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .CommandType = CommandType.StoredProcedure
                oRequest.Command = "sp_Servizio_Elimina"
                oParam = objAccesso.GetParameter("@SRVZ_Id", n_SRVZ_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_Errore = Errori_Db.None
            Catch
                Me.n_Errore = Errori_Db.DBDelete
            End Try
        End Sub
#End Region

        Public Shared Function AttivaServizio(ByVal SRVZ_ID As Integer, ByVal isAttivo As Boolean) As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Servizio_AttivaServizio"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@SRVZ_id", SRVZ_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                If isAttivo Then
                    oParam = objAccesso.GetParameter("@SRVZ_Attivato", 1, ParameterDirection.Input, DbType.Byte)
                Else
                    oParam = objAccesso.GetParameter("@SRVZ_Attivato", 0, ParameterDirection.Input, DbType.Byte)
                End If
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
				iResponse = objAccesso.GetExecuteNotQuery(oRequest)
				ObjectBase.PurgeCacheItems(CachePolicy.ServiziComunita())
                ObjectBase.PurgeCacheItems(CachePolicy.MenuComunita())
                ObjectBase.PurgeCacheItems(CachePolicy.RenderAllCommunity())
            Catch ex As Exception
                iResponse = -1
            End Try
            Return iResponse
        End Function

#Region "Metodi Associazione Permessi"
        ' DA COMMENTARE !!!!!!!!!
        Public Function ElencaPermessi() As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Servizio_ElencaPermessi"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@SRVZ_id", Me.ID, ParameterDirection.Input, DbType.Int32)
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
        Public Function ElencaRuoliPermessiByTipoComunita(ByVal ORGN_ID As Integer, ByVal TPCM_ID As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Servizio_ElencaRuoliPermessiByTipoComunita"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@SRVZ_id", Me.n_SRVZ_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@TPCM_id", TPCM_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@ORGN_ID", ORGN_ID, ParameterDirection.Input, DbType.Int32)
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
#End Region

#Region "Metodi Associazione TipiComunita"

        Public Function ElencaTipiComunita(ByVal ORGN_ID As Integer) As DataSet
            'restituisce un dataset contenente i tipi di comunit‡ associati e non a quel servizio
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Servizio_ElencaTipiCmntByOrganizzazione"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@SRVZ_id", Me.ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ORGN_ID", ORGN_ID, ParameterDirection.Input, DbType.Int32)
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
        Public Sub AssociaTipoComunita(ByVal TPCM_ID As Integer, ByVal isDefault As Boolean, ByVal ORGN_ID As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .CommandType = CommandType.StoredProcedure
                oRequest.Command = "sp_Servizio_Associa_TipoComunitaByOrganizzazione"
                oParam = objAccesso.GetParameter("@TPCM_id", TPCM_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@SRVZ_id", n_SRVZ_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                If isDefault Then
                    oParam = objAccesso.GetParameter("@Attivato", 1, ParameterDirection.Input, DbType.Byte)
                Else
                    oParam = objAccesso.GetParameter("@Attivato", 0, ParameterDirection.Input, DbType.Byte)
                End If
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@ORGN_ID", ORGN_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch
                Me.n_Errore = Errori_Db.DBError
            End Try
        End Sub
        Public Sub DisAssociaTipoComunita(ByVal TPCM_ID As Integer, ByVal ORGN_ID As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .CommandType = CommandType.StoredProcedure
                oRequest.Command = "sp_Servizio_DisAssocia_TipoComunitaByOrganizzazione"
                oParam = objAccesso.GetParameter("@TPCM_id", TPCM_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@SRVZ_id", n_SRVZ_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ORGN_ID", ORGN_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch
                Me.n_Errore = Errori_Db.DBError
            End Try
        End Sub

#End Region

#Region "Metodi Recupero Permessi byRuolo"

        'VERIFICARE QUESTA SOTTOSTANTE !!!!!!!!!!!!!!!!!!

        Public Shared Function ElencaByTipoRuoloByComunita(ByVal TPRL_ID As Integer, ByVal CMNT_ID As Integer) As DataSet
            'elenca i servizi in base al loro link, id e ne indica i permessi relativi al ruolo specificato
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oTable As New DataSet
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Servizio_ElencaWithPermessoByTipoRuoloByComunita"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@TPRL_Id", TPRL_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_ID", CMNT_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                oTable = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception

            End Try
            Return oTable
        End Function


        Public Sub AssociaRuolo(ByVal ORGN_ID As Integer, ByVal TPCM_ID As Integer, ByVal TPRL_ID As Integer, ByVal Valore As String)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess


            With oRequest
                .Command = "sp_Servizio_AssociaRuolo"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@LPRS_TPRL_id", TPRL_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LPRS_SRVZ_id", n_SRVZ_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LPRS_valore", Valore.Trim, ParameterDirection.Input, DbType.String)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LPRS_ORGN_ID", ORGN_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LPRS_TPCM_ID", TPCM_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try
        End Sub
        Public Sub DisassociaRuolo(ByVal TPRL_ID As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Servizio_DisassociaRuolo"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@LPRS_TPRL_id", TPRL_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LPRS_SRVZ_id", n_SRVZ_id, ParameterDirection.Input, DbType.Int32)
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

        Public Function ElencaOrganizzazioniAssociate() As DataSet
            'restituisce un dataset contenente i tipi di comunit‡ associati a quel servizio
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Servizio_ElencaOrganizzazioniAssociate"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@SRVZ_id", Me.n_SRVZ_id, ParameterDirection.Input, DbType.Int32)
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
        Public Function ElencaTipiComunitaAssociate(ByVal ORGN_ID As Integer) As DataSet
            'restituisce un dataset contenente i tipi di comunit‡ associati a quel servizio
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Servizio_ElencaTipiComunitaAssociati"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@SRVZ_id", Me.n_SRVZ_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ORGN_ID", ORGN_ID, ParameterDirection.Input, DbType.Int32)
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

#End Region

#Region "Metodi Associazione Lingua - Termine"
        Public Function GetNomeForLingua() As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dstable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Servizio_GetNomeForLingua"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@SRVZ_ID", Me.n_SRVZ_id, , DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                dstable = objAccesso.GetdataSet(oRequest)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBReadExist
            End Try
            Return dstable
        End Function
        Public Sub AssociaNomeByLingua(ByVal LNGU_ID As Integer, ByVal Termine As String, ByVal Descrizione As String)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Servizio_AssociaNomeByLingua"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@LKLS_LNGU_id", LNGU_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LKLS_SRVZ_id", Me.n_SRVZ_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LKLS_termine", Termine, ParameterDirection.Input, DbType.String, , 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LKLS_descrizione", Descrizione, ParameterDirection.Input, DbType.String, , 250)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                ' Recupero l'ID univoco del tipo di limite
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBReadExist
            End Try
        End Sub
        Public Sub EliminaNomeByLingua(ByVal LNGU_ID As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Menu_EliminaNomeByLingua"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@LKLS_LNGU_id", LNGU_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LKLS_SRVZ_id", Me.n_SRVZ_id, ParameterDirection.Input, DbType.Int32)
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
#End Region

#Region "Menu Gestione Servizi - Lato Amministrazione comunita"

        Public Shared Function GestioneServiziComunita(ByVal TPRL_ID As Integer, ByVal CMNT_ID As Integer, ByVal LNGU_ID As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "  sp_Menu_ElencaServiziAssociati"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@TPRL_id", TPRL_ID, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LNGU_id", LNGU_ID, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_id", CMNT_ID, , DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                dsTable = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception
                'gestire errori delle shared
            End Try
            Return dsTable
        End Function

        Public Function EstraiByLingua(ByVal LNGU_ID As Integer) As Integer
            ' estrai i dati dalla tabella
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Servizio_EstraiByLingua"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@SRVZ_Id", Me.n_SRVZ_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@SRVZ_Nome", "", ParameterDirection.Output, SqlDbType.VarChar, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@SRVZ_codice", "", ParameterDirection.Output, SqlDbType.VarChar, , 15)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@SRVZ_descrizione", "", ParameterDirection.Output, SqlDbType.VarChar, , 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@SRVZ_padre", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@SRVZ_nonDisattivabile", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@SRVZ_Attivato", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@SRVZ_isNotificabile", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@LNGU_ID", LNGU_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_SRVZ_Nome = oRequest.GetValueFromParameter(2)
                Me.n_SRVZ_codice = oRequest.GetValueFromParameter(3)
                Me.n_SRVZ_Descrizione = oRequest.GetValueFromParameter(4)
                Me.n_SRVZ_padre = oRequest.GetValueFromParameter(5)
                Me.n_SRVZ_nonDisattivabile = oRequest.GetValueFromParameter(6)
                Me.n_SRVZ_Attivato = oRequest.GetValueFromParameter(7)
                Me._isNotificabile = oRequest.GetValueFromParameter(8)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBReadExist
            End Try

        End Function
        Public Function EstraiByCode(ByVal Code As String, Optional ByVal LNGU_ID As Integer = -1) As Integer
            ' estrai i dati dalla tabella
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Servizio_EstraiByCode"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@SRVZ_codice", Code, ParameterDirection.Input, SqlDbType.VarChar, , 15)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@SRVZ_Id", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@SRVZ_Nome", "", ParameterDirection.Output, SqlDbType.VarChar, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@SRVZ_descrizione", "", ParameterDirection.Output, SqlDbType.VarChar, , 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@SRVZ_padre", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@SRVZ_nonDisattivabile", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@SRVZ_Attivato", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@SRVZ_isNotificabile", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@LNGU_ID", LNGU_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_SRVZ_codice = Code
                Me.n_SRVZ_id = oRequest.GetValueFromParameter(2)
                Me.n_SRVZ_Nome = oRequest.GetValueFromParameter(3)
                Me.n_SRVZ_Descrizione = oRequest.GetValueFromParameter(4)
                Me.n_SRVZ_padre = oRequest.GetValueFromParameter(5)
                Me.n_SRVZ_nonDisattivabile = oRequest.GetValueFromParameter(6)
                Me.n_SRVZ_Attivato = oRequest.GetValueFromParameter(7)
                Me._isNotificabile = oRequest.GetValueFromParameter(8)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBReadExist
            End Try

        End Function
#End Region


#Region "Metodi Associazione Permessi Da GestioneRuoli"
        Public Shared Function ElencaTipiComunitaAssociate_NoOrgn(ByVal SRVZ_Id As Integer, ByVal TPRL_ID As Integer, ByVal LinguaId As Integer) As DataSet
            'restituisce un dataset contenente i tipi di comunit‡ associati e non a quel servizio
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Servizio_ElencaTipiComunitaAssociate_NoOrgn"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@SRVZ_id", SRVZ_Id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPRL_ID", TPRL_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LinguaId", LinguaId, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                dsTable = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception

            End Try
            Return dsTable
        End Function

        Public Shared Function ElencaPermessiOrganizzazioniByServizio_TipoComunita(ByVal SRVZ_Id As Integer, ByVal TPCM_ID As Integer, ByVal TPRL_ID As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Servizio_ElencaPermessiByServizio_TipoComunita"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@SRVZ_id", SRVZ_Id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@TPCM_id", TPCM_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@TPRL_ID", TPRL_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                dsTable = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception

            End Try
            Return dsTable
        End Function

#End Region

#Region "Nuova Gestione Servizi"
        Public Sub Translate(ByVal Termine As String, ByVal Descrizione As String, ByVal LinguaID As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Servizio_Translate"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@SRVZ_id", Me.n_SRVZ_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@Termine", Termine, ParameterDirection.Input, SqlDbType.VarChar, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@Descrizione", Descrizione, ParameterDirection.Input, SqlDbType.VarChar, , 200)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBChange
            End Try
        End Sub
        Public Function ElencaDefinizioniLingue() As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim oDataSet As New DataSet

            With oRequest
                .Command = "sp_Servizio_ElencaDefinizioniLingue"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@SRVZ_id", Me.n_SRVZ_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                oDataSet = objAccesso.GetdataSet(oRequest)
                Me.n_Errore = Errori_Db.None
                Return oDataSet
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBReadExist
            End Try
            Return oDataSet
        End Function
        Public Function ElencaPermessiForDefinizione(ByVal LinguaID As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Servizio_ElencaPermessiForDefinizione"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@SRVZ_id", Me.n_SRVZ_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
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

        Public Function ListPermissionForSelection(ByVal idLanguage As Integer) As List(Of PermissionAssociation)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDatareader As IDataReader
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Servizio_ElencaPermessiForDefinizione"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@SRVZ_id", Me.n_SRVZ_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@LinguaID", idLanguage, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Dim items As New List(Of PermissionAssociation)
            Try
                oDatareader = objAccesso.GetdataReader(oRequest)
                Dim name, description As String
                While oDatareader.Read
                    If IsDBNull(oDatareader.Item("Nome")) Then
                        name = ""
                    Else
                        name = oDatareader.Item("Nome")
                    End If
                    If IsDBNull(oDatareader.Item("Descrizione")) Then
                        description = ""
                    Else
                        description = oDatareader.Item("Descrizione")
                    End If
                    items.Add(New PermissionAssociation() With {.Id = oDatareader.Item("PRMS_ID"), .Name = name, .Description = description, .Position = oDatareader.Item("PRMS_Posizione"), .IsAssociated = oDatareader.Item("Associato") = 1})
                End While
            Catch ex As Exception
            End Try
            Return items
        End Function

        Public Function ElencaDefinizioniLinguePermessi(ByVal PermessoID As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim oDataSet As New DataSet

            With oRequest
                .Command = "sp_Servizio_ElencaDefinizioniLinguePermessi"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@SRVZ_id", Me.n_SRVZ_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PermessoID", PermessoID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                oDataSet = objAccesso.GetdataSet(oRequest)
                Me.n_Errore = Errori_Db.None
                Return oDataSet
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBReadExist
            End Try
            Return oDataSet
        End Function

        Public Sub TranslatePermessoAssociato(ByVal LinguaID As Integer, ByVal PermessoID As Integer, ByVal Nome As String, ByVal Descrizione As String)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Servizio_TranslatePermessoAssociato"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@SRVZ_id", Me.n_SRVZ_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRMS_ID", PermessoID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@Nome", Nome, ParameterDirection.Input, SqlDbType.VarChar, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@Descrizione", Descrizione, ParameterDirection.Input, SqlDbType.VarChar, , 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try
        End Sub

        Public Function TipiComunitaDefiniti(ByVal OrganizzazioneID As Integer, ByVal LinguaId As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim oDataSet As New DataSet

            With oRequest
                .Command = "sp_Servizio_TipiComunitaDefiniti"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@LinguaId", LinguaId, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_Id", OrganizzazioneID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@SRVZ_id", Me.n_SRVZ_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                oDataSet = objAccesso.GetdataSet(oRequest)
                Me.n_Errore = Errori_Db.None
                Return oDataSet
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBReadExist
            End Try
            Return oDataSet
        End Function

        Public Sub GeneraTipiComunitaImpostazioniDefault(ByVal ListaAssocate As String, ByVal ListaAttivate As String)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Servizio_GeneraTipiComunitaImpostazioniDefault"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@ListaAbilitati", ListaAssocate, ParameterDirection.Input, SqlDbType.VarChar, True, 8000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ServizioID", Me.n_SRVZ_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ListaAttivati", ListaAttivate, ParameterDirection.Input, SqlDbType.VarChar, True, 8000)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBChange
            End Try
        End Sub



        Public Sub DefinisciTipoComunitaAttivate(ByVal OrganizzazioneID As Integer, ByVal ListaTipiComunita As String, ByVal Replica As Boolean)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Servizio_DefinisciTipiComunit‡Attivati"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@ListaTipiComunita", ListaTipiComunita, ParameterDirection.Input, SqlDbType.VarChar, True, 4000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@SRVZ_id", Me.n_SRVZ_id, ParameterDirection.Input, SqlDbType.Int)
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
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBChange
            End Try
        End Sub
        Public Sub DefinisciTipiComunitaDisponibili(ByVal OrganizzazioneID As Integer, ByVal ListaTipiComunita As String, ByVal Replica As Boolean)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Servizio_DefinisciTipiComunitaDisponibili"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@ListaTipiComunita", ListaTipiComunita, ParameterDirection.Input, SqlDbType.VarChar, True, 4000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@SRVZ_id", Me.n_SRVZ_id, ParameterDirection.Input, SqlDbType.Int)
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
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBChange
            End Try
        End Sub
        Public Sub ImpostaTipoComunitaToDefault(ByVal TipoComunitaID As Integer, ByVal isAttivato As Boolean, ByVal isDefault As Boolean, ByVal isForReplica As Boolean)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Servizio_ImpostaTipoComunitaToDefault"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@ServizioID", Me.n_SRVZ_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@TipoComunitaID", TipoComunitaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                If isDefault Then
                    isAttivato = True
                    oParam = objAccesso.GetAdvancedParameter("@isDefault", 1, ParameterDirection.Input, SqlDbType.Int)
                Else
                    oParam = objAccesso.GetAdvancedParameter("@isDefault", 0, ParameterDirection.Input, SqlDbType.Int)
                End If
                .Parameters.Add(oParam)

                If isAttivato Then
                    oParam = objAccesso.GetAdvancedParameter("@isAttivato", 1, ParameterDirection.Input, SqlDbType.Int)
                Else
                    oParam = objAccesso.GetAdvancedParameter("@isAttivato", 0, ParameterDirection.Input, SqlDbType.Int)
                End If
                .Parameters.Add(oParam)

                If isForReplica Then
                    oParam = objAccesso.GetAdvancedParameter("@isForReplica", 1, ParameterDirection.Input, SqlDbType.Int)
                Else
                    oParam = objAccesso.GetAdvancedParameter("@isForReplica", 0, ParameterDirection.Input, SqlDbType.Int)
                End If
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
				objAccesso.GetExecuteNotQuery(oRequest)

				ObjectBase.PurgeCacheItems(CachePolicy.ServiziComunita())
                ObjectBase.PurgeCacheItems(CachePolicy.MenuComunita())
                ObjectBase.PurgeCacheItems(CachePolicy.RenderAllCommunity())
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBChange
            End Try
        End Sub
        Public Sub ImpostaTipoComunita(ByVal OrganizzazioneID As Integer, ByVal TipoComunitaID As Integer, ByVal isAssociato As Boolean, ByVal isAttivato As Boolean)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Servizio_ImpostaTipoComunita"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@ServizioID", Me.n_SRVZ_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@OrganizzazioneID", OrganizzazioneID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@TipoComunitaID", TipoComunitaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                If isAttivato Then
                    isAssociato = True
                    oParam = objAccesso.GetAdvancedParameter("@isAttivato", 1, ParameterDirection.Input, SqlDbType.Int)
                Else
                    oParam = objAccesso.GetAdvancedParameter("@isAttivato", 0, ParameterDirection.Input, SqlDbType.Int)
                End If
                .Parameters.Add(oParam)

                If isAssociato Then
                    oParam = objAccesso.GetAdvancedParameter("@isAssociato", 1, ParameterDirection.Input, SqlDbType.Int)
                Else
                    oParam = objAccesso.GetAdvancedParameter("@isAssociato", 0, ParameterDirection.Input, SqlDbType.Int)
                End If
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
				objAccesso.GetExecuteNotQuery(oRequest)
				ObjectBase.PurgeCacheItems(CachePolicy.ServiziComunita())
                ObjectBase.PurgeCacheItems(CachePolicy.MenuComunita())
                ObjectBase.PurgeCacheItems(CachePolicy.RenderAllCommunity())
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBChange
            End Try
        End Sub

        Public Function TipiComunitaAssociati(ByVal OrganizzazioneID As Integer, ByVal LinguaId As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim oDataSet As New DataSet

            With oRequest
                .Command = "sp_Servizio_TipiComunitaAssociati"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@LinguaId", LinguaId, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_Id", OrganizzazioneID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@SRVZ_ID", Me.n_SRVZ_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                oDataSet = objAccesso.GetdataSet(oRequest)
                Me.n_Errore = Errori_Db.None
                Return oDataSet
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBReadExist
            End Try
            Return oDataSet
        End Function

        Public Sub AssociaPermesso(ByVal PermessoID As Integer, ByVal Nome As String, ByVal Descrizione As String)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Servizio_AssociaPermesso"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@SRVZ_id", Me.n_SRVZ_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRMS_ID", PermessoID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@Nome", Nome, ParameterDirection.Input, SqlDbType.VarChar, , 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@descrizione", Descrizione, ParameterDirection.Input, SqlDbType.VarChar, , 100)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try

        End Sub
        Public Sub DisassociaPermesso(ByVal PermessoID As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Servizio_DisassociaPermesso"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@SRVZ_id", Me.n_SRVZ_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRMS_ID", PermessoID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try
        End Sub
		Public Function ElencaPermessiAssociatiByLingua(ByVal LinguaID As Integer) As DataSet
			Dim oRequest As New COL_Request
			Dim oParam As New COL_Request.Parameter
			Dim dsTable As New DataSet
			Dim objAccesso As New COL_DataAccess
			With oRequest
				.Command = "sp_Servizio_ElencaPermessiAssociatiByLingua"
				.CommandType = CommandType.StoredProcedure

				oParam = objAccesso.GetAdvancedParameter("@SRVZ_id", Me.n_SRVZ_id, ParameterDirection.Input, SqlDbType.Int)
				.Parameters.Add(oParam)

				oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
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


        Public Sub ImpostaPermessiDefault(ByVal OrganizzazioneID As Integer, ByVal RuoloID As Integer, ByVal TipoComunitaID As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Servizio_ImpostaPermessiDefault"
                .CommandType = CommandType.StoredProcedure

				oParam = objAccesso.GetAdvancedParameter("@ServizioID", Me.n_SRVZ_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@OrganizzazioneID", OrganizzazioneID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@RuoloID", RuoloID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@TipoComunitaID", TipoComunitaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
				objAccesso.GetExecuteNotQuery(oRequest)
				ObjectBase.PurgeCacheItems(CachePolicy.ServiziComunita())
                ObjectBase.PurgeCacheItems(CachePolicy.MenuComunita())
                ObjectBase.PurgeCacheItems(CachePolicy.RenderAllCommunity())
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try

        End Sub
        Public Sub ImpostaPermessi(ByVal isForReplica As Boolean, ByVal OrganizzazioneID As Integer, ByVal ServizioID As Integer, ByVal RuoloID As Integer, ByVal TipoComunitaID As Integer, ByVal Permessi As String)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Servizio_ImpostaPermessi"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@ServizioID", ServizioID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@RuoloID", RuoloID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@TipoComunitaID", TipoComunitaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@Permessi", Permessi, ParameterDirection.Input, SqlDbType.VarChar, , 32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@OrganizzazioneID", OrganizzazioneID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                If isForReplica Then
                    oParam = objAccesso.GetAdvancedParameter("@isForReplica", 1, ParameterDirection.Input, SqlDbType.Int)
                Else
                    oParam = objAccesso.GetAdvancedParameter("@isForReplica", 0, ParameterDirection.Input, SqlDbType.Int)
                End If
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
				objAccesso.GetExecuteNotQuery(oRequest)

				ObjectBase.PurgeCacheItems(CachePolicy.ServiziComunita())
                ObjectBase.PurgeCacheItems(CachePolicy.MenuComunita())
                ObjectBase.PurgeCacheItems(CachePolicy.RenderAllCommunity())
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try

        End Sub
        Public Sub GeneraPermessiDefault(ByVal ServizioID As Integer, ByVal RuoloID As Integer, ByVal TipoComunitaID As Integer, ByVal Permessi As String)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Servizio_GeneraPermessiDefault"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@ServizioID", ServizioID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@RuoloID", RuoloID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@TipoComunitaID", TipoComunitaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@Permessi", Permessi, ParameterDirection.Input, SqlDbType.VarChar, , 32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try

        End Sub
        Public Sub GeneraProfiloDefault(ByVal ServizioID As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Servizio_GeneraProfilo"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@ServizioID", ServizioID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try
        End Sub
#End Region

		Public Shared Function LazyElencaByTipoRuoloByComunita(ByVal RuoloID As Integer, ByVal ComunitaID As Integer) As List(Of PlainService)
			Dim oRequest As New COL_Request
			Dim oParam As New COL_Request.Parameter
			Dim oLista As New List(Of PlainService)
			Dim oDatareader As IDataReader
			Dim objAccesso As New COL_DataAccess
			With oRequest
				.Command = "sp_Servizio_ElencaWithPermessoByTipoRuoloByComunita"
				.CommandType = CommandType.StoredProcedure
				oParam = objAccesso.GetAdvancedParameter("@TPRL_Id", RuoloID, ParameterDirection.Input, SqlDbType.Int)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", ComunitaID, ParameterDirection.Input, SqlDbType.Int)
				.Parameters.Add(oParam)
				.Role = COL_Request.UserRole.Admin
				.transactional = False
			End With
			Try
				oDatareader = objAccesso.GetdataReader(oRequest)

				While oDatareader.Read
					oLista.Add(New PlainService(oDatareader.Item("SRVZ_ID"), oDatareader.Item("SRVZ_Codice"), oDatareader.Item("LKSC_Permessi")))
				End While
			Catch ex As Exception

			Finally
				If oDatareader.IsClosed = False Then
					oDatareader.Close()
				End If

			End Try
			Return oLista
		End Function



		Public Shared Function ListRuoliServizio(ByVal ComunitaID As Integer, ByVal ServizioID As Integer, ByVal LinguaID As Integer, ByVal DefaultValue As Boolean, Optional ByVal sortExpression As String = "", Optional ByVal sortDirection As String = "") As List(Of RuoloServizio)
			Dim oLista As New List(Of RuoloServizio)
			Dim cacheKey As String = CachePolicy.RuoliServizioComunita(ComunitaID, ServizioID, LinguaID, DefaultValue)

			If sortDirection <> String.Empty Then
				sortDirection = sortDirection.ToLower
			End If

			If ObjectBase.Cache(cacheKey) Is Nothing Then
				oLista = COL_Servizio.RetrieveRuoliServizioFromDB(ComunitaID, ServizioID, LinguaID, DefaultValue)
				ObjectBase.Cache.Insert(cacheKey, oLista, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza30minuti)
			Else
				oLista = CType(ObjectBase.Cache(cacheKey), List(Of RuoloServizio))
			End If

			If (Not sortExpression Is Nothing AndAlso sortDirection <> String.Empty) Then
				oLista.Sort(New GenericComparer(Of RuoloServizio)(sortExpression))
			End If

			If (Not sortDirection Is Nothing AndAlso sortDirection = "desc") Then
				oLista.Reverse()
			End If
			Return oLista
		End Function
        Public Shared Function RetrieveRuoliServizioFromDB(ByVal ComunitaID As Integer, ByVal ServizioID As Integer, ByVal LinguaID As Integer, ByVal DefaultValue As Boolean) As List(Of RuoloServizio)
            Dim oLista As New List(Of RuoloServizio)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim oDatareader As IDataReader

            With oRequest
                .Command = "sp_Servizio_RetrieveRuoliServizioFromDB"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@ComunitaID", ComunitaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@ServizioID", ServizioID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@DefaultValue", DefaultValue, ParameterDirection.Input, SqlDbType.Bit)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try

                Dim oListaPermessi As New List(Of Permessi)
                oListaPermessi = COL_Servizio.ListPermessiServizio(ServizioID, LinguaID, "Nome")
                oDatareader = objAccesso.GetdataReader(oRequest)
                While oDatareader.Read
                    Try
                        Dim oRuoloServizio As New RuoloServizio
                        Dim oRuolo As New COL_TipoRuolo
                        Dim PermDefiniti, PermDefault, PermProfilo As String
                        Dim isAssociato, isFromProfilo, isDefault As Boolean

                        oRuolo.Id = oDatareader("TPRL_id")
                        oRuolo.Nome = GenericValidator.ValString(oDatareader("TPRL_nome"), "")
                        oRuolo.Descrizione = GenericValidator.ValString(oDatareader("TPRL_descrizione"), "")
                        oRuolo.Gerarchia = oDatareader("TPRL_gerarchia")

                        PermDefiniti = GenericValidator.ValString(oDatareader("Permessi_Definiti"), "")
                        PermDefault = GenericValidator.ValString(oDatareader("Permessi_Default"), "")
                        PermProfilo = GenericValidator.ValString(oDatareader("Permessi_Profilo"), "")
                        isAssociato = GenericValidator.ValBool(oDatareader("Associato"), False)
                        isFromProfilo = GenericValidator.ValBool(oDatareader("isFromProfilo"), False)
                        isDefault = GenericValidator.ValBool(oDatareader("isDefault"), False)

                        oLista.Add(New RuoloServizio(oRuolo, oListaPermessi, PermDefiniti, PermDefault, PermProfilo, isAssociato, isDefault, isFromProfilo))
                    Catch ex As Exception

                    End Try
                End While
                oDatareader.Close()
            Catch ex As Exception
            Finally
                If Not IsNothing(oDatareader) Then
                    If oDatareader.IsClosed = False Then
                        oDatareader.Close()
                    End If
                End If
            End Try
            Return oLista
        End Function


		Public Shared Function ListPermessiServizio(ByVal ServizioID As Integer, ByVal LinguaID As Integer, Optional ByVal sortExpression As String = "", Optional ByVal sortDirection As String = "") As List(Of Permessi)
			Dim oLista As New List(Of Permessi)
			Dim cacheKey As String = CachePolicy.PermessiServizio(ServizioID, LinguaID)

			If sortDirection <> String.Empty Then
				sortDirection = sortDirection.ToLower
			End If

			If ObjectBase.Cache(cacheKey) Is Nothing Then
				oLista = COL_Servizio.RetrievePermessiServizioFromDB(ServizioID, LinguaID)
				ObjectBase.Cache.Insert(cacheKey, oLista, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza60minuti)
			Else
				oLista = CType(ObjectBase.Cache(cacheKey), List(Of Permessi))
			End If

			If (Not sortExpression Is Nothing AndAlso sortDirection <> String.Empty) Then
				oLista.Sort(New GenericComparer(Of RuoloServizio)(sortExpression))
			End If

			If (Not sortDirection Is Nothing AndAlso sortDirection = "desc") Then
				oLista.Reverse()
			End If
			Return oLista
		End Function
		Private Shared Function RetrievePermessiServizioFromDB(ByVal ServizioID As Integer, ByVal LinguaID As Integer) As List(Of Permessi)
			Dim oLista As New List(Of Permessi)
			Dim oRequest As New COL_Request
			Dim oParam As New COL_Request.Parameter
			Dim objAccesso As New COL_DataAccess
			Dim oDatareader As IDataReader

			With oRequest
				.Command = "sp_Servizio_ElencaPermessiAssociatiByLingua"
				.CommandType = CommandType.StoredProcedure

				oParam = objAccesso.GetAdvancedParameter("@SRVZ_id", ServizioID, ParameterDirection.Input, SqlDbType.Int)
				.Parameters.Add(oParam)

				oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
				.Parameters.Add(oParam)

				.Role = COL_Request.UserRole.Admin
				.transactional = False
			End With

			Try
				oDatareader = objAccesso.GetdataReader(oRequest)
				While oDatareader.Read
					Try
						Dim oPermesso As New Permessi
						oPermesso.ID = oDatareader("PRMS_id")
						oPermesso.Posizione = oDatareader("PRMS_Posizione")
						oPermesso.Nome = GenericValidator.ValString(oDatareader("Nome"), "")
						oPermesso.Descrizione = GenericValidator.ValString(oDatareader("Descrizione"), "")
						If oPermesso.Nome = "" Then
							oPermesso.Nome = GenericValidator.ValString(oDatareader("NomeDefault"), "")
						End If
						If oPermesso.Descrizione = "" Then
							oPermesso.Descrizione = GenericValidator.ValString(oDatareader("DescrizioneDefault"), "")
						End If
						oLista.Add(oPermesso)
					Catch ex As Exception

					End Try
				End While
				oDatareader.Close()
			Catch ex As Exception
			Finally
				If Not IsNothing(oDatareader) Then
					If oDatareader.IsClosed = False Then
						oDatareader.Close()
					End If
				End If
			End Try
			Return oLista
		End Function
    End Class
End Namespace