Imports COL_DataLayer

Namespace CL_permessi
    Public Class COL_Menu
		Inherits ObjectBase

#Region "Private Property"
        Private n_MENU_ID As Integer
        Private n_MENU_SRVZ_ID As Integer
        Private n_MENU_Nome As String
        Private n_MENU_LINK As String
        Private n_MENU_Padre_Id As Integer
        Private n_MENU_Macro As Integer
        Private n_MENU_OrdineV As Integer
        Private n_MENU_OrdineO As Integer
        Private n_MENU_Abilitato As Integer
        Private n_Errore As Errori_Db
#End Region

#Region "Public Property"
        Public Property ID() As Integer
            Get
                ID = n_MENU_ID
            End Get
            Set(ByVal Value As Integer)
                n_MENU_ID = Value
            End Set
        End Property
        Public Property Servizio_ID() As Integer
            Get
                Servizio_ID = n_MENU_SRVZ_ID
            End Get
            Set(ByVal Value As Integer)
                n_MENU_SRVZ_ID = Value
            End Set
        End Property
        Public Property Nome() As String
            Get
                Nome = n_MENU_Nome
            End Get
            Set(ByVal Value As String)
                n_MENU_Nome = Value
            End Set
        End Property
        Public Property Link() As String
            Get
                Link = n_MENU_LINK
            End Get
            Set(ByVal Value As String)
                n_MENU_LINK = Value
            End Set
        End Property
        Public Property Padre_Id() As Integer
            Get
                Padre_Id = n_MENU_Padre_Id
            End Get
            Set(ByVal Value As Integer)
                n_MENU_Padre_Id = Value
            End Set
        End Property
        Public Property IsMacro() As Boolean
            Get
                IsMacro = CBool(n_MENU_Macro = 1)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    n_MENU_Macro = 1
                Else
                    n_MENU_Macro = 0
                End If
            End Set
        End Property
        Public Property isAbilitato() As Boolean
            Get
                isAbilitato = CBool(n_MENU_Abilitato = 1)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    n_MENU_Abilitato = 1
                Else
                    n_MENU_Abilitato = 0
                End If
            End Set
        End Property
        Public Property OrdineV() As Integer
            Get
                OrdineV = n_MENU_OrdineV
            End Get
            Set(ByVal Value As Integer)
                n_MENU_OrdineV = Value
            End Set
        End Property
        Public Property OrdineO() As Integer
            Get
                OrdineO = n_MENU_OrdineO
            End Get
            Set(ByVal Value As Integer)
                n_MENU_OrdineO = Value
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
            Me.n_Errore = Errori_Db.None
            Me.n_MENU_OrdineV = 0
            Me.n_MENU_OrdineO = 0
            Me.n_MENU_SRVZ_ID = 0
            Me.n_MENU_Padre_Id = 0
            Me.n_MENU_Macro = 0
            Me.n_MENU_Abilitato = 1
        End Sub
#End Region

#Region "Metodi Standard"
        Public Shared Function Elenca() As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dstable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Menu_Elenca"
                .CommandType = CommandType.StoredProcedure
                .Role = COL_Request.UserRole.Admin
                .Transactional = False
            End With
            Try
                dstable = objAccesso.GetDataSet(oRequest)
            Catch ex As Exception
                'vedere come gestire gli errori nelle shared
            End Try
            Return dstable
        End Function
        Public Shared Function ElencaTreeView() As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dstable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Menu_ElencaTreeView"
                .CommandType = CommandType.StoredProcedure
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                dstable = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception
                'vedere come gestire gli errori nelle shared
            End Try
            Return dstable
        End Function
        Public Sub Estrai()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dstable As Integer
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Menu_Estrai"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@MENU_ID", Me.n_MENU_ID, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENU_SRVZ_ID", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENU_Nome", "", ParameterDirection.Output, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENU_LINK", "", ParameterDirection.Output, DbType.String, , 250)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENU_Padre_Id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENU_Macro", "", ParameterDirection.Output, DbType.Byte)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENU_OrdineV", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENU_OrdineO", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENU_Abilitato", "", ParameterDirection.Output, DbType.Byte)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                dstable = objAccesso.GetExecuteNotQuery(oRequest)

                Me.n_MENU_SRVZ_ID = oRequest.GetValueFromParameter(2)
                Me.n_MENU_Nome = oRequest.GetValueFromParameter(3)
                Me.n_MENU_LINK = oRequest.GetValueFromParameter(4)
                Me.n_MENU_Padre_Id = oRequest.GetValueFromParameter(5)
                Me.n_MENU_Macro = oRequest.GetValueFromParameter(6)
                Me.n_MENU_OrdineV = oRequest.GetValueFromParameter(7)
                Me.n_MENU_OrdineO = oRequest.GetValueFromParameter(8)
                Me.n_MENU_Abilitato = oRequest.GetValueFromParameter(9)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try
        End Sub
        Public Sub Aggiungi()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Menu_Aggiungi"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@MENU_ID", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENU_SRVZ_ID", Me.n_MENU_SRVZ_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENU_Nome", Me.n_MENU_Nome, ParameterDirection.Input, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENU_LINK", Me.n_MENU_LINK, ParameterDirection.Input, DbType.String, , 250)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENU_Padre_Id", Me.n_MENU_Padre_Id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENU_Macro", Me.n_MENU_Macro, ParameterDirection.Input, DbType.Byte)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENU_OrdineV", Me.n_MENU_OrdineV, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENU_OrdineO", Me.n_MENU_OrdineO, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENU_Abilitato", Me.n_MENU_Abilitato, ParameterDirection.Input, DbType.Byte)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                ' Recupero l'ID univoco del tipo di limite
                Me.n_MENU_ID = oRequest.GetValueFromParameter(1)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_MENU_ID = -1
                Me.n_Errore = Errori_Db.DBReadExist
            End Try
        End Sub
        Public Sub Modifica()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Menu_Modifica"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@MENU_ID", Me.n_MENU_ID, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENU_SRVZ_ID", Me.n_MENU_SRVZ_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENU_Nome", Me.n_MENU_Nome, ParameterDirection.Input, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENU_LINK", Me.n_MENU_LINK, ParameterDirection.Input, DbType.String, , 250)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENU_Padre_Id", Me.n_MENU_Padre_Id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENU_Macro", Me.n_MENU_Macro, ParameterDirection.Input, DbType.Byte)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENU_OrdineV", Me.n_MENU_OrdineV, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENU_OrdineO", Me.n_MENU_OrdineO, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENU_Abilitato", Me.n_MENU_Abilitato, ParameterDirection.Input, DbType.Byte)
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
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Menu_Cancella"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@MENU_ID", Me.n_MENU_ID, , DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBDelete
            End Try
        End Sub
#End Region

        Public Sub Abilita(ByVal IsAbilitato As Boolean)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dstable As Integer
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Menu_Abilita"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@MENU_ID", Me.n_MENU_ID, , DbType.Int32)
                .Parameters.Add(oParam)
                If IsAbilitato Then
                    oParam = objAccesso.GetParameter("@MENU_Abilitato", 1, ParameterDirection.Input, DbType.Byte)
                Else
                    oParam = objAccesso.GetParameter("@MENU_Abilitato", 0, ParameterDirection.Input, DbType.Byte)
                End If

                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
				dstable = objAccesso.GetExecuteNotQuery(oRequest)
				ObjectBase.PurgeCacheItems(CachePolicy.MenuComunita)
                ObjectBase.PurgeCacheItems(CachePolicy.RenderAllCommunity())
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try
        End Sub

        Public Function getMaxHorizontal() As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Integer
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Menu_getMaxHorizontal"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@MENU_ID", Me.n_MENU_ID, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MaxHorizontal", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                iResponse = objAccesso.GetExecuteNotQuery(oRequest)
                iResponse = oRequest.GetValueFromParameter(2)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try
            Return iResponse
        End Function

        Public Function getMaxVertical() As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Integer
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Menu_getMaxVertical"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@MENU_ID", Me.n_MENU_ID, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@MaxVertical", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                iResponse = objAccesso.GetExecuteNotQuery(oRequest)
                iResponse = oRequest.GetValueFromParameter(2)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try
            Return iResponse
        End Function

        Public Sub Sposta(ByVal newPadre_ID As Integer, ByVal posX As Integer, ByVal posY As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Menu_Sposta"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@MENU_ID", Me.n_MENU_ID, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENU_Padre_Id", Me.n_MENU_Padre_Id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                If newPadre_ID < 0 Then
                    newPadre_ID = 0
                End If
                oParam = objAccesso.GetParameter("@Nuovo_Padre_Id", newPadre_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENU_OrdineV", posY, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENU_OrdineO", posX, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
				objAccesso.GetExecuteNotQuery(oRequest)
                ObjectBase.PurgeCacheItems(CachePolicy.MenuComunita)
                ObjectBase.PurgeCacheItems(CachePolicy.RenderAllCommunity())
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBChange
            End Try
        End Sub

#Region "Metodi Associazione Permessi"
        Public Function GetPermessiByServizio(ByVal ServizioID As Integer, ByVal LinguaID As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dstable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Menu_PermessiByServizio"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@MENU_ID", Me.n_MENU_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@ServizioID", ServizioID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
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

        Public Function GetSottoMenu(ByVal MenuId As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dstable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Menu_GetSottoMenu"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@MENU_ID", MenuId, , DbType.Int32)
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

        Public Sub AssociaPermesso(ByVal PRMS_ID As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Menu_AssociaPermesso"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@LKMS_PRMS_id", PRMS_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LKMS_MENU_id", Me.n_MENU_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LKMS_SRVZ_id", Me.n_MENU_SRVZ_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
				objAccesso.GetExecuteNotQuery(oRequest)
                ObjectBase.PurgeCacheItems(CachePolicy.MenuComunita)
                ObjectBase.PurgeCacheItems(CachePolicy.RenderAllCommunity())
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBInsert
            End Try
        End Sub
        Public Sub EliminaPermesso(ByVal PRMS_ID As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Menu_EliminaPermesso"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@LKMS_PRMS_id", PRMS_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LKMS_MENU_id", Me.n_MENU_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LKMS_SRVZ_id", Me.n_MENU_SRVZ_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
				objAccesso.GetExecuteNotQuery(oRequest)
                ObjectBase.PurgeCacheItems(CachePolicy.MenuComunita)
                ObjectBase.PurgeCacheItems(CachePolicy.RenderAllCommunity())
                ' Recupero l'ID univoco del tipo di limite
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBDelete
            End Try
        End Sub
#End Region

#Region "Metodi Associazione Lingua - Termine"
        Public Function GetVociForLingua() As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dstable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Menu_GetVociForLingua"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@MENU_ID", Me.n_MENU_ID, , DbType.Int32)
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
        Public Sub AssociaVoceMenuByLingua(ByVal LNGU_ID As Integer, ByVal Termine As String, ByVal Descrizione As String)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Menu_AssociaVoceMenuByLingua"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@LKLM_LNGU_id", LNGU_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LKLM_MENU_id", Me.n_MENU_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LKLM_termine", Termine, ParameterDirection.Input, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LKLM_descrizione", Descrizione, ParameterDirection.Input, DbType.String, , 200)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
				' Recupero l'ID univoco del tipo di limite
                ObjectBase.PurgeCacheItems(CachePolicy.MenuComunita)
                ObjectBase.PurgeCacheItems(CachePolicy.RenderAllCommunity())
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBReadExist
            End Try
        End Sub
        Public Sub EliminaVoceMenuByLingua(ByVal LNGU_ID As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Menu_EliminaVoceMenuByLingua"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@LKLM_LNGU_id", LNGU_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LKLM_MENU_id", Me.n_MENU_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
				objAccesso.GetExecuteNotQuery(oRequest)
                ObjectBase.PurgeCacheItems(CachePolicy.MenuComunita)
                ObjectBase.PurgeCacheItems(CachePolicy.RenderAllCommunity())
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBDelete
            End Try
        End Sub
#End Region

#Region "Creazione MENU"
        Public Shared Function GeneraMenu(ByVal TPRL_ID As Integer, ByVal CMNT_ID As Integer, ByVal LNGU_ID As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Menu_GeneraByRuolo_Comunita"
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
#End Region

        Public Shared Function ElencaForOrdineVisualizzazione(ByVal IdMenu As Integer, ByVal PadreID As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataset As New DataSet
            Dim objAccesso As New COL_DataAccess

            Try
                With oRequest
                    .Command = "sp_Menu_ElencaForOrdineVisualizzazione"
                    .CommandType = CommandType.StoredProcedure

                    oParam = objAccesso.GetAdvancedParameter("@PadreID", PadreID, ParameterDirection.Input, SqlDbType.Int)
                    .Parameters.Add(oParam)

                    oParam = objAccesso.GetAdvancedParameter("@MENU_ID", IdMenu, ParameterDirection.Input, SqlDbType.Int)
                    .Parameters.Add(oParam)

                    .Role = COL_Request.UserRole.Admin
                    .transactional = False
                End With
                oDataset = objAccesso.GetdataSet(oRequest)

            Catch ex As Exception

            End Try
            Return oDataset
		End Function


		'Public Shared Function LazyGeneraMenu__(ByVal RuoloID As Integer, ByVal ComunitaID As Integer, ByVal LinguaID As Integer, Optional ByVal sortExpression As String = "", Optional ByVal sortDirection As String = "") As GenericCollection(Of MenuElement)
		'	Dim ListaMenu As New GenericCollection(Of MenuElement)
		'	Dim cacheKey As String = CachePolicy.MenuComunita(ComunitaID, RuoloID, LinguaID)

		'	If sortDirection <> String.Empty Then
		'		sortDirection = sortDirection.ToLower
		'	End If
		'	If ObjectBase.Cache(cacheKey) Is Nothing Then
		'		ListaMenu = COL_Menu.RetrieveMenuFromDB(RuoloID, ComunitaID, LinguaID)
		'		ObjectBase.Cache.Insert(cacheKey, ListaMenu, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza60minuti)
		'	Else
		'		ListaMenu = CType(ObjectBase.Cache(cacheKey), GenericCollection(Of MenuElement))
		'	End If

		'	If (Not sortExpression Is Nothing AndAlso sortDirection <> String.Empty) Then
		'		ListaMenu.Sort(New GenericComparer(Of MenuElement)(sortExpression))
		'	End If

		'	If (Not sortDirection Is Nothing AndAlso sortDirection = "desc") Then
		'		ListaMenu.Reverse()
		'	End If

		'	Return ListaMenu
		'End Function

		'Private Shared Function RetrieveMenuFromDB(ByVal RuoloID As Integer, ByVal ComunitaID As Integer, ByVal LinguaID As Integer) As GenericCollection(Of MenuElement)
		'	Dim oRequest As New COL_Request
		'	Dim oParam As New COL_Request.Parameter
		'	Dim iDataReader As IDataReader
		'	Dim objAccesso As New COL_DataAccess

		'	Dim ListaMenu As New GenericCollection(Of MenuElement)
		'	With oRequest
		'		.Command = "sp_Menu_GeneraByRuolo_Comunita"
		'		.CommandType = CommandType.StoredProcedure
		'		oParam = objAccesso.GetAdvancedParameter("@TPRL_id", RuoloID, ParameterDirection.Input, SqlDbType.Int)
		'		.Parameters.Add(oParam)
		'		oParam = objAccesso.GetAdvancedParameter("@LNGU_id", LinguaID, ParameterDirection.Input, SqlDbType.Int)
		'		.Parameters.Add(oParam)
		'		oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", ComunitaID, ParameterDirection.Input, SqlDbType.Int)
		'		.Parameters.Add(oParam)
		'		.Role = COL_Request.UserRole.Admin
		'		.transactional = False
		'	End With
		'	Try
		'		iDataReader = objAccesso.GetdataReader(oRequest)

		'		While iDataReader.Read
		'			ListaMenu.Add(New MenuElement(iDataReader.Item("MENU_ID"), iDataReader.Item("MENU_Padre_Id"), _
		'			GenericValidator.ValString(iDataReader.Item("MENU_Nome"), ""), GenericValidator.ValString(iDataReader.Item("MENU_LINK"), "")))
		'		End While
		'	Catch ex As Exception
		'		ListaMenu = New GenericCollection(Of MenuElement)
		'	End Try
		'	Return ListaMenu
		'End Function
    End Class
End Namespace