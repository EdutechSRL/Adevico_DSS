Imports COL_DataLayer
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.Comol.Entities

Namespace CL_permessi
    Public Class COL_TipoRuolo
		Inherits ObjectBase

#Region "Private Property"
        Private n_TPRL_id As Integer
        Private n_TPRL_Descrizione As String
        Private n_TPRL_Nome As String
        Private n_TPRL_noDelete As Boolean
        Private n_TPRL_noModify As Boolean
        Private n_TPRL_gerarchia As Integer
        Private n_errore As Errori_Db
#End Region


#Region "Public Property"
        Public Property Id() As Integer
            Get
                Id = n_TPRL_id
            End Get
            Set(ByVal Value As Integer)
                n_TPRL_id = Value
            End Set
        End Property
        Public Property Descrizione() As String
            Get
                Descrizione = n_TPRL_Descrizione
            End Get
            Set(ByVal Value As String)
                n_TPRL_Descrizione = Value
            End Set
        End Property
        Public Property Nome() As String
            Get
                Nome = n_TPRL_Nome
            End Get
            Set(ByVal Value As String)
                n_TPRL_Nome = Value
            End Set
        End Property
        Public Property Gerarchia() As Integer
            Get
                Gerarchia = n_TPRL_gerarchia
            End Get
            Set(ByVal Value As Integer)
                n_TPRL_gerarchia = Value
            End Set
        End Property
        Public Property noDelete() As Boolean
            Get
                noDelete = n_TPRL_noDelete
            End Get
            Set(ByVal Value As Boolean)
                n_TPRL_noDelete = Value
            End Set
        End Property
        Public Property noModify() As Boolean
            Get
                noModify = n_TPRL_noModify
            End Get
            Set(ByVal Value As Boolean)
                n_TPRL_noModify = Value
            End Set
        End Property
        Public ReadOnly Property Errore() As Errori_Db
            Get
                Errore = n_errore
            End Get
        End Property
#End Region

#Region "Metodi New"

        Sub New(ByVal TPRL_id As Integer)
            Me.Id = TPRL_id
            Me.n_errore = Errori_Db.None
		End Sub
		Sub New(ByVal TPRL_id As Integer, ByVal Nome As String)
			Me.Id = TPRL_id
			Me.n_TPRL_Nome = Nome
			Me.n_errore = Errori_Db.None
		End Sub
        Sub New()
            Me.n_errore = Errori_Db.None
        End Sub
#End Region

#Region "Metodi Standard"
		Public Function Elenca(ByVal LinguaID As Integer, ByVal withAccessoLibero As Boolean) As DataSet
			Dim oRequest As New COL_Request
			Dim oParam As New COL_Request.Parameter
			Dim dsTable As New DataSet
			Dim objAccesso As New COL_DataAccess

			With oRequest
				.Command = "sp_TipoRuolo_Elenca"
				.CommandType = CommandType.StoredProcedure

				oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
				.Parameters.Add(oParam)

				If withAccessoLibero Then
					oParam = objAccesso.GetAdvancedParameter("@AccessoLibero", 1, ParameterDirection.Input, SqlDbType.Int)
				Else
					oParam = objAccesso.GetAdvancedParameter("@AccessoLibero", 0, ParameterDirection.Input, SqlDbType.Int)
				End If
				.Parameters.Add(oParam)

				.Role = COL_Request.UserRole.Admin
				.transactional = False
			End With
			Try
				dsTable = objAccesso.GetdataSet(oRequest)
				Me.n_errore = Errori_Db.None
			Catch ex As Exception
				Me.n_errore = Errori_Db.DBError
			End Try
			Return dsTable
		End Function
		'Public Function E__ElencaLivelli() As DataSet
		'	Dim oRequest As New COL_Request
		'	Dim oParam As New COL_Request.Parameter
		'	Dim dsTable As New DataSet
		'	Dim objAccesso As New COL_DataAccess

		'	With oRequest
		'		.Command = "sp_Tipo_Ruolo_ElencaLivelli"
		'		.CommandType = CommandType.StoredProcedure


		'		.Role = COL_Request.UserRole.Admin
		'		.transactional = False
		'	End With
		'	Try
		'		dsTable = objAccesso.GetdataSet(oRequest)
		'	Catch ex As Exception
		'		Me.n_errore = Errori_Db.DBError
		'	End Try
		'	Return dsTable
		'End Function
        Public Function EstraiGerarchiaMassima() As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim TPRL_gerarchiaMAX As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Tipo_Ruolo_EstraiGerarchiaMassima"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@TPRL_gerarchiaMAX ", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                TPRL_gerarchiaMAX = objAccesso.GetExecuteNotQuery(oRequest)
                TPRL_gerarchiaMAX = oRequest.GetValueFromParameter(1)
                Me.n_errore = Errori_Db.None
            Catch ex As Exception
                TPRL_gerarchiaMAX = -1
                Me.n_errore = Errori_Db.DBReadExist
            End Try
            Return TPRL_gerarchiaMAX
        End Function

        Public Sub Modifica()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Tipo_Ruolo_Modifica"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@TPRL_Id", n_TPRL_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPRL_nome", n_TPRL_Nome.Trim, ParameterDirection.Input, DbType.String, , 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPRL_Descrizione", n_TPRL_Descrizione.Trim, ParameterDirection.Input, DbType.String, , 500)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPRL_noDelete", n_TPRL_noDelete, ParameterDirection.Input, DbType.Boolean)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPRL_noModify", n_TPRL_noModify, ParameterDirection.Input, DbType.Boolean)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPRL_gerarchia", n_TPRL_gerarchia, ParameterDirection.Input, DbType.Int32)
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
        Public Sub Aggiungi()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Tipo_Ruolo_Aggiungi"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@TPRL_Id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPRL_nome", n_TPRL_Nome.Trim, ParameterDirection.Input, DbType.String, , 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPRL_Descrizione", n_TPRL_Descrizione.Trim, ParameterDirection.Input, DbType.String, , 500)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPRL_noDelete", n_TPRL_noDelete, ParameterDirection.Input, DbType.Boolean)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPRL_noModify", n_TPRL_noModify, ParameterDirection.Input, DbType.Boolean)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPRL_gerarchia", n_TPRL_gerarchia, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                ' Recupero l'ID univoco del tipo ruolo
                Me.n_TPRL_id = oRequest.GetValueFromParameter(1)
                Me.n_errore = Errori_Db.None
            Catch
                Me.n_TPRL_id = -1
                Me.n_errore = Errori_Db.DBInsert
            End Try
        End Sub
        'borsi
        Public Sub Estrai()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim oIstituzione As New COL_Istituzione

            With oRequest
                .Command = "sp_Tipo_Ruolo_Estrai"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@TPRL_id", Id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPRL_nome", "", ParameterDirection.Output, DbType.String, , 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPRL_descrizione", "", ParameterDirection.Output, DbType.String, , 500)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPRL_noDelete", "", ParameterDirection.Output, DbType.Boolean)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPRL_noModify", "", ParameterDirection.Output, DbType.Boolean)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPRL_gerarchia", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)

                Me.n_TPRL_Nome = oRequest.GetValueFromParameter(2)
                Me.n_TPRL_Descrizione = oRequest.GetValueFromParameter(3)
                Me.n_TPRL_noDelete = oRequest.GetValueFromParameter(4)
                Me.n_TPRL_noModify = oRequest.GetValueFromParameter(5)
                Me.n_TPRL_gerarchia = oRequest.GetValueFromParameter(6)
                Me.n_errore = Errori_Db.None
            Catch ax As Exception
                Me.n_errore = Errori_Db.DBReadExist
            End Try
        End Sub
        Public Sub Estrai(ByVal LNGU_ID As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim oIstituzione As New COL_Istituzione

            With oRequest
                .Command = "sp_TipoRuolo_EstraiByLingua"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@TPRL_id", Id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@TPRL_nome", "", ParameterDirection.Output, SqlDbType.VarChar, , 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@TPRL_descrizione", "", ParameterDirection.Output, SqlDbType.VarChar, , 500)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@TPRL_noDelete", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@TPRL_noModify", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@TPRL_gerarchia", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@LNGU_ID", LNGU_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)

                Me.n_TPRL_Nome = oRequest.GetValueFromParameter(2)
                Me.n_TPRL_Descrizione = oRequest.GetValueFromParameter(3)
                Me.n_TPRL_noDelete = oRequest.GetValueFromParameter(4)
                Me.n_TPRL_noModify = oRequest.GetValueFromParameter(5)
                Me.n_TPRL_gerarchia = oRequest.GetValueFromParameter(6)
                Me.n_errore = Errori_Db.None
            Catch ax As Exception
                Me.n_errore = Errori_Db.DBReadExist
            End Try
        End Sub

        Public Sub Elimina()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Tipo_Ruolo_Elimina"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@TPRL_Id", Me.n_TPRL_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
				objAccesso.GetExecuteNotQuery(oRequest)
				ObjectBase.PurgeCacheItems(CachePolicy.ServiziComunita())
                ObjectBase.PurgeCacheItems(CachePolicy.MenuComunita())
                ObjectBase.PurgeCacheItems(CachePolicy.RenderAllCommunity())
                Me.n_errore = Errori_Db.None
            Catch ex As Exception
                Me.n_errore = Errori_Db.DBDelete
            End Try
        End Sub

        Public Sub Translate(ByVal Termine As String, ByVal Descrizione As String, ByVal LinguaID As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_TipoRuolo_Translate"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@TPRL_Id", Me.n_TPRL_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@Termine", Termine, ParameterDirection.Input, SqlDbType.NVarChar, , 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@Descrizione", Descrizione, ParameterDirection.Input, SqlDbType.NVarChar, , 500)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
				objAccesso.GetExecuteNotQuery(oRequest)
				ObjectBase.PurgeCacheItems(CachePolicy.ServiziComunita())
				Me.n_errore = Errori_Db.None
            Catch ex As Exception
                Me.n_errore = Errori_Db.DBChange
            End Try
        End Sub

		Public Function ElencaDefinizioniLingue() As List(Of PlainRole)
			Dim oRequest As New COL_Request
			Dim oParam As New COL_Request.Parameter
			Dim objAccesso As New COL_DataAccess
			Dim oLista As New List(Of PlainRole)

			With oRequest
				.Command = "sp_TipoRuolo_ElencaDefinizioniLingue"
				.CommandType = CommandType.StoredProcedure
				oParam = objAccesso.GetAdvancedParameter("@TPRL_Id", Me.n_TPRL_id, ParameterDirection.Input, SqlDbType.Int)
				.Parameters.Add(oParam)
				.Role = COL_Request.UserRole.Admin
				.transactional = False
			End With
			Try
				Dim iDataReader As IDataReader
				iDataReader = objAccesso.GetdataReader(oRequest)

				While iDataReader.Read
					Dim oLingua As Lingua = Lingua.CreateByName(iDataReader("LNGU_ID"), GenericValidator.ValString(iDataReader("LNGU_nome"), ""))
					Dim oRuolo As New Role
					oRuolo.ID = Me.n_TPRL_id
					oRuolo.Name = GenericValidator.ValString(iDataReader("Nome"), "")
					oRuolo.Description = GenericValidator.ValString(iDataReader("Descrizione"), "")

					oLista.Add(New PlainRole(oLingua, oRuolo))
				End While

				Me.n_errore = Errori_Db.None

			Catch ex As Exception
				Me.n_errore = Errori_Db.DBReadExist
			End Try
			Return oLista
		End Function
#End Region

#Region "Permessi"

        '    Public Function GetDefinizionePermessi(ByVal OrganizzazioneID As Integer, ByVal TipoComunitaID As Integer, ByVal ServizioID As Integer) As String
        '        Dim oRequest As New COL_Request
        '        Dim oParam As New COL_Request.Parameter
        '        Dim iResponse As String = "00000000000000000000000000000000"
        '        Dim objAccesso As New COL_DataAccess

        '        With oRequest
        '            .Command = "sp_TipoRuolo_GetDefinizionePermessi" 'sp_TipoRuolo_DefaultPermessiServizioTipoComunita"
        '            .CommandType = CommandType.StoredProcedure

        '            oParam = objAccesso.GetAdvancedParameter("@RuoloID", Me.n_TPRL_id, ParameterDirection.Input, SqlDbType.Int)
        '            .Parameters.Add(oParam)

        '            oParam = objAccesso.GetAdvancedParameter("@TipoComunitaID", TipoComunitaID, ParameterDirection.Input, SqlDbType.Int)
        '            .Parameters.Add(oParam)

        '            oParam = objAccesso.GetAdvancedParameter("@OrganizzazioneID", OrganizzazioneID, ParameterDirection.Input, SqlDbType.Int)
        '            .Parameters.Add(oParam)

        '            oParam = objAccesso.GetAdvancedParameter("@ServizioID", ServizioID, ParameterDirection.Input, SqlDbType.Int)
        '            .Parameters.Add(oParam)

        '            oParam = objAccesso.GetAdvancedParameter("@permessi", "", ParameterDirection.Output, SqlDbType.VarChar, , 32)
        '            .Parameters.Add(oParam)

        '            .Role = COL_Request.UserRole.Admin
        '            .transactional = False
        '        End With
        '        Try
        '            objAccesso.GetExecuteNotQuery(oRequest)
        '            iResponse = oRequest.GetValueFromParameter(5)

        '        Catch ex As Exception

        '        End Try
        '        Return iResponse
        '    End Function

        '    Public Shared Function AssociaPermessiDefault(ByVal TPRL_Id As Integer, ByVal TPCM_Id As Integer, ByVal ORGN_ID As Integer, ByVal SRVZ_ID As Integer) As String
        '        Dim oRequest As New COL_Request
        '        Dim oParam As New COL_Request.Parameter
        '        Dim iResponse As String = "00000000000000000000000000000000"
        '        Dim objAccesso As New COL_DataAccess

        '        With oRequest
        '            .Command = "sp_TipoRuolo_AssociaPermessiDefault" 'sp_TipoRuolo_DefaultPermessiServizioTipoComunita"
        '            .CommandType = CommandType.StoredProcedure

        '            oParam = objAccesso.GetAdvancedParameter("@TPRL_Id", TPRL_Id, ParameterDirection.Input, SqlDbType.Int)
        '            .Parameters.Add(oParam)

        '            oParam = objAccesso.GetAdvancedParameter("@TPCM_Id", TPCM_Id, ParameterDirection.Input, SqlDbType.Int)
        '            .Parameters.Add(oParam)

        '            oParam = objAccesso.GetAdvancedParameter("@ORGN_ID", ORGN_ID, ParameterDirection.Input, SqlDbType.Int)
        '            .Parameters.Add(oParam)

        '            oParam = objAccesso.GetAdvancedParameter("@SRVZ_ID", SRVZ_ID, ParameterDirection.Input, SqlDbType.Int)
        '            .Parameters.Add(oParam)

        '            oParam = objAccesso.GetAdvancedParameter("@permessi", "", ParameterDirection.Output, SqlDbType.VarChar, , 32)
        '            .Parameters.Add(oParam)

        '            .Role = COL_Request.UserRole.Admin
        '            .transactional = False
        '        End With
        '        Try
        '            objAccesso.GetExecuteNotQuery(oRequest)
        '            iResponse = oRequest.GetValueFromParameter(5)
        'ObjectBase.PurgeCacheItems(CachePolicy.ServiziComunita())
        'ObjectBase.PurgeCacheItems(CachePolicy.MenuComunita())
        '            ObjectBase.PurgeCacheItems(CachePolicy.RenderAllCommunity())
        '        Catch ex As Exception

        '        End Try
        '        Return iResponse
        '    End Function

        Public Sub AssociaTipiComunita(ByVal ListaTipiComunita As String)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_TipoRuolo_AssociaTipiComunita"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@TPRL_Id", Me.n_TPRL_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@Elenco", ListaTipiComunita, ParameterDirection.Input, SqlDbType.VarChar, , 4000)
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
        '        Public Sub DefinisciPermessiRuoli(ByVal OrganizzazioneID As Integer, ByVal ServizioID As Integer, ByVal TipoComunitaId As Integer, ByVal StringaPermessi As String, ByVal Replica As Boolean)
        '            Dim oRequest As New COL_Request
        '            Dim oParam As New COL_Request.Parameter
        '            Dim objAccesso As New COL_DataAccess

        '            With oRequest
        '                .Command = "sp_TipoRuolo_DefinisciPermessiRuoli"
        '                .CommandType = CommandType.StoredProcedure
        '                oParam = objAccesso.GetAdvancedParameter("@StringaPermessi", StringaPermessi, ParameterDirection.Input, SqlDbType.VarChar, True, 32)
        '                .Parameters.Add(oParam)
        '                oParam = objAccesso.GetAdvancedParameter("@TPCM_id", TipoComunitaId, ParameterDirection.Input, SqlDbType.Int)
        '                .Parameters.Add(oParam)
        '                oParam = objAccesso.GetAdvancedParameter("@ORGN_ID", OrganizzazioneID, ParameterDirection.Input, SqlDbType.Int)
        '                .Parameters.Add(oParam)
        '                oParam = objAccesso.GetAdvancedParameter("@ServizioID", ServizioID, ParameterDirection.Input, SqlDbType.Int)
        '                .Parameters.Add(oParam)
        '                oParam = objAccesso.GetAdvancedParameter("@TPRL_ID", Me.n_TPRL_id, ParameterDirection.Input, SqlDbType.Int)
        '                .Parameters.Add(oParam)
        '                If Replica Then
        '                    oParam = objAccesso.GetAdvancedParameter("@Replica", 1, ParameterDirection.Input, SqlDbType.Int)
        '                Else
        '                    oParam = objAccesso.GetAdvancedParameter("@Replica", 0, ParameterDirection.Input, SqlDbType.Int)
        '                End If
        '                .Parameters.Add(oParam)

        '                .Role = COL_Request.UserRole.Admin
        '                .transactional = False
        '            End With
        '            Try
        '				objAccesso.GetExecuteNotQuery(oRequest)
        '				ObjectBase.PurgeCacheItems(CachePolicy.ServiziComunita())
        '                ObjectBase.PurgeCacheItems(CachePolicy.MenuComunita())
        '                ObjectBase.PurgeCacheItems(CachePolicy.RenderAllCommunity())

        '                Me.n_errore = Errori_Db.None
        '            Catch ex As Exception
        '                Me.n_errore = Errori_Db.DBChange
        '            End Try
        '        End Sub
#End Region

        Public Function ElencaConCurriculum(ByVal CMNT_ID As Integer, ByVal LinguaID As Integer, ByVal oVisibilita As Main.FiltroVisibilità) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_TipoRuolo_ElencaConCurriculum"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", CMNT_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@Visibilita", CType(oVisibilita, Main.FiltroVisibilità), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                dsTable = objAccesso.GetdataSet(oRequest)
                Me.n_errore = Errori_Db.None
            Catch ex As Exception
                Me.n_errore = Errori_Db.DBError
            End Try
            Return dsTable
        End Function

        Public Shared Function List(ByVal LanguageID As Integer) As List(Of Role)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oLista As New List(Of Role)
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "select TPRL_Id, dbo.ufn_GetValueByLingua(" & LanguageID.ToString & ",'TPRL_nome','TIPO_RUOLO',TPRL_Id) as TPRL_nome from TIPO_RUOLO"
                .CommandType = CommandType.Text

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                Dim iDataReader As IDataReader
                iDataReader = objAccesso.GetdataReader(oRequest)

                While iDataReader.Read
                    Dim oRuolo As New Role
                    oRuolo.ID = iDataReader("TPRL_Id")
                    oRuolo.Name = GenericValidator.ValString(iDataReader("TPRL_nome"), "")
                    oLista.Add(oRuolo)
                End While
            Catch ex As Exception

            End Try
            Return oLista
        End Function
    End Class

End Namespace