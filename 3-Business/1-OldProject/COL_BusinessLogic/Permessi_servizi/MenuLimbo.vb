Imports COL_DataLayer
Namespace CL_permessi
    Public Class COL_MenuLimbo
		Inherits ObjectBase

#Region "Private Property"
        Private n_MENL_ID As Integer
        Private n_MENL_Nome As String
        Private n_MENL_LINK As String
        Private n_MENL_Padre_Id As Integer
        Private n_MENL_Macro As Integer
        Private n_MENL_OrdineV As Integer
        Private n_MENL_OrdineO As Integer
        Private n_MENL_Abilitato As Integer
        Private n_MENL_ForAdmin As Integer
        Private n_Errore As Errori_Db
#End Region

#Region "Public Property"
        Public Property ID() As Integer
            Get
                ID = n_MENL_ID
            End Get
            Set(ByVal Value As Integer)
                n_MENL_ID = Value
            End Set
        End Property
        Public Property isAbilitato() As Boolean
            Get
                isAbilitato = CBool(n_MENL_Abilitato = 1)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    n_MENL_Abilitato = 1
                Else
                    n_MENL_Abilitato = 0
                End If
            End Set
        End Property
        Public Property Nome() As String
            Get
                Nome = n_MENL_Nome
            End Get
            Set(ByVal Value As String)
                n_MENL_Nome = Value
            End Set
        End Property
        Public Property Link() As String
            Get
                Link = n_MENL_LINK
            End Get
            Set(ByVal Value As String)
                n_MENL_LINK = Value
            End Set
        End Property
        Public Property Padre_Id() As Integer
            Get
                Padre_Id = n_MENL_Padre_Id
            End Get
            Set(ByVal Value As Integer)
                n_MENL_Padre_Id = Value
            End Set
        End Property
        Public Property IsMacro() As Boolean
            Get
                IsMacro = CBool(n_MENL_Macro = 1)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    n_MENL_Macro = 1
                Else
                    n_MENL_Macro = 0
                End If
            End Set
        End Property
        Public Property OrdineV() As Integer
            Get
                OrdineV = n_MENL_OrdineV
            End Get
            Set(ByVal Value As Integer)
                n_MENL_OrdineV = Value
            End Set
        End Property
        Public Property OrdineO() As Integer
            Get
                OrdineO = n_MENL_OrdineO
            End Get
            Set(ByVal Value As Integer)
                n_MENL_OrdineO = Value
            End Set
        End Property
        Public Property IsForAdmin() As Boolean
            Get
                IsMacro = CBool(n_MENL_ForAdmin = 1)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    n_MENL_ForAdmin = 1
                Else
                    n_MENL_ForAdmin = 0
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
            Me.n_Errore = Errori_Db.None
            Me.n_MENL_OrdineV = 0
            Me.n_MENL_OrdineO = 0
            Me.n_MENL_Padre_Id = 0
            Me.n_MENL_Macro = 0
            Me.n_MENL_Abilitato = 1
            Me.n_MENL_ForAdmin = 0
        End Sub
#End Region

#Region "Metodi Standard"
        'Public Shared Function Elenca() As DataSet
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim dstable As New DataSet
        '    Dim objAccesso As New COL_DataAccess

        '    With oRequest
        '        .Command = "sp_MenuLimbo_Elenca"
        '        .CommandType = CommandType.StoredProcedure
        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        dstable = objAccesso.GetdataSet(oRequest)
        '    Catch ex As Exception
        '        'vedere come gestire gli errori nelle shared
        '    End Try
        '    Return dstable
        'End Function
        Public Shared Function ElencaTreeView(ByVal oFiltroMenuLimbo As Main.FiltroMenuLimbo) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dstable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_MenuLimbo_ElencaTreeView"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@ForAdmin", CType(oFiltroMenuLimbo, Main.FiltroMenuLimbo), ParameterDirection.Input, DbType.Byte)
                .Parameters.Add(oParam)

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
                .Command = "sp_MenuLimbo_Estrai"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@MENL_ID", Me.n_MENL_ID, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENL_Nome", "", ParameterDirection.Output, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENL_LINK", "", ParameterDirection.Output, DbType.String, , 250)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENL_Padre_Id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENL_Macro", "", ParameterDirection.Output, DbType.Byte)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENL_OrdineV", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENL_OrdineO", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@MENL_Abilitato", "", ParameterDirection.Output, DbType.Byte)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@MENL_ForAdmin", "", ParameterDirection.Output, DbType.Byte)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                dstable = objAccesso.GetExecuteNotQuery(oRequest)

                Me.n_MENL_Nome = oRequest.GetValueFromParameter(2)
                Me.n_MENL_LINK = oRequest.GetValueFromParameter(3)
                Me.n_MENL_Padre_Id = oRequest.GetValueFromParameter(4)
                Me.n_MENL_Macro = oRequest.GetValueFromParameter(5)
                Me.n_MENL_OrdineV = oRequest.GetValueFromParameter(6)
                Me.n_MENL_OrdineO = oRequest.GetValueFromParameter(7)
                Me.n_MENL_Abilitato = oRequest.GetValueFromParameter(8)
                Me.n_MENL_ForAdmin = oRequest.GetValueFromParameter(9)
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
                .Command = "sp_MenuLimbo_Aggiungi"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@MENL_ID", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENL_Nome", Me.n_MENL_Nome, ParameterDirection.Input, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENL_LINK", Me.n_MENL_LINK, ParameterDirection.Input, DbType.String, , 250)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENL_Padre_Id", Me.n_MENL_Padre_Id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENL_Macro", Me.n_MENL_Macro, ParameterDirection.Input, DbType.Byte)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENL_OrdineV", Me.n_MENL_OrdineV, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENL_OrdineO", Me.n_MENL_OrdineO, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENL_Abilitato", Me.n_MENL_Abilitato, ParameterDirection.Input, DbType.Byte)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENL_ForAdmin", Me.n_MENL_ForAdmin, ParameterDirection.Input, DbType.Byte)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                ' Recupero l'ID univoco del tipo di limite
                Me.n_MENL_ID = oRequest.GetValueFromParameter(1)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_MENL_ID = -1
                Me.n_Errore = Errori_Db.DBReadExist
            End Try
        End Sub
        Public Sub Modifica()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_MenuLimbo_Modifica"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@MENL_ID", Me.n_MENL_ID, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENL_Nome", Me.n_MENL_Nome, ParameterDirection.Input, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENL_LINK", Me.n_MENL_LINK, ParameterDirection.Input, DbType.String, , 250)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENL_Padre_Id", Me.n_MENL_Padre_Id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENL_Macro", Me.n_MENL_Macro, ParameterDirection.Input, DbType.Byte)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENL_OrdineV", Me.n_MENL_OrdineV, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENL_OrdineO", Me.n_MENL_OrdineO, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENL_Abilitato", Me.n_MENL_Abilitato, ParameterDirection.Input, DbType.Byte)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENL_ForAdmin", Me.n_MENL_ForAdmin, ParameterDirection.Input, DbType.Byte)
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
                .Command = "sp_MenuLimbo_Cancella"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@MENL_ID", Me.n_MENL_ID, , DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
				objAccesso.GetExecuteNotQuery(oRequest)
				ObjectBase.PurgeCacheItems(CachePolicy.MenuPortale)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBDelete
            End Try
        End Sub
#End Region

        Public Function getMaxHorizontal(ByVal oFiltroMenuLimbo As Main.FiltroMenuLimbo) As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Integer
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_MenuLimbo_getMaxHorizontal"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@MENL_ID", Me.n_MENL_ID, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MaxHorizontal", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@ForAdmin", CType(oFiltroMenuLimbo, Main.FiltroMenuLimbo), ParameterDirection.Input, DbType.Int32)
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

        Public Function getMaxVertical(ByVal oFiltroMenuLimbo As Main.FiltroMenuLimbo) As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Integer
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_MenuLimbo_getMaxVertical"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@MENL_ID", Me.n_MENL_ID, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@MaxVertical", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@ForAdmin", CType(oFiltroMenuLimbo, Main.FiltroMenuLimbo), ParameterDirection.Input, DbType.Int32)
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
        Public Sub Sposta(ByVal newPadre_ID As Integer, ByVal posX As Integer, ByVal posY As Integer, ByVal oFiltroMenuLimbo As Main.FiltroMenuLimbo)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_MenuLimbo_Sposta"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@MENL_ID", Me.n_MENL_ID, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENL_Padre_Id", Me.n_MENL_Padre_Id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                If newPadre_ID < 0 Then
                    newPadre_ID = 0
                End If
                oParam = objAccesso.GetParameter("@Nuovo_Padre_Id", newPadre_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENL_OrdineV", posY, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENL_OrdineO", posX, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)


                oParam = objAccesso.GetParameter("@ForAdmin", CType(oFiltroMenuLimbo, Main.FiltroMenuLimbo), ParameterDirection.Input, DbType.Byte)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
				objAccesso.GetExecuteNotQuery(oRequest)
				ObjectBase.PurgeCacheItems(CachePolicy.MenuPortale)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBChange
            End Try
        End Sub

        Public Sub Abilita(ByVal IsAbilitato As Boolean)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dstable As Integer
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_MenuLimbo_Abilita"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@MENL_ID", Me.n_MENL_ID, , DbType.Int32)
                .Parameters.Add(oParam)
                If IsAbilitato Then
                    oParam = objAccesso.GetParameter("@MENL_Abilitato", 1, ParameterDirection.Input, DbType.Byte)
                Else
                    oParam = objAccesso.GetParameter("@MENL_Abilitato", 0, ParameterDirection.Input, DbType.Byte)
                End If
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
				dstable = objAccesso.GetExecuteNotQuery(oRequest)
				ObjectBase.PurgeCacheItems(CachePolicy.MenuPortale)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try
        End Sub

#Region "Metodi Associazione TipiPersona"
        Public Function GetTipiPersona(ByVal MENU_ID As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dstable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_MenuLimbo_GetTipiPersona"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@MENL_ID", MENU_ID, , DbType.Int32)
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
        Public Sub AssociaTipoPersona(ByVal TPRS_ID As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_MenuLimbo_AssociaTipoPersona"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@TPRS_ID", TPRS_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENL_id", Me.n_MENL_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
				objAccesso.GetExecuteNotQuery(oRequest)
				ObjectBase.PurgeCacheItems(CachePolicy.MenuPortale)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBInsert
            End Try
        End Sub
        Public Sub DisAssociaTipoPersona(ByVal TPRS_ID As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_MenuLimbo_EliminaTipoPersona"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@TPRS_ID", TPRS_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENL_id", Me.n_MENL_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
				' Recupero l'ID univoco del tipo di limite
				ObjectBase.PurgeCacheItems(CachePolicy.MenuPortale)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBDelete
            End Try
        End Sub
        Public Function GetSottoMenu(ByVal pMENL_ID As Integer, ByVal oFiltroMenuLimbo As Main.FiltroMenuLimbo) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dstable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_MenuLimbo_GetSottoMenu"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@MENL_ID", pMENL_ID, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@ForAdmin", CType(oFiltroMenuLimbo, Main.FiltroMenuLimbo), ParameterDirection.Input, DbType.Byte)
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
#End Region

#Region "Metodi Associazione Lingua - Termine"
        Public Function GetVociForLingua() As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dstable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_MenuLimbo_GetVociForLingua"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@MENL_ID", Me.n_MENL_ID, , DbType.Int32)
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
                .Command = "sp_MenuLimbo_AssociaVoceMenuByLingua"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@LNGU_id", LNGU_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENL_id", Me.n_MENL_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Termine", Termine, ParameterDirection.Input, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Descrizione", Descrizione, ParameterDirection.Input, DbType.String, , 200)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
				' Recupero l'ID univoco del tipo di limite
				ObjectBase.PurgeCacheItems(CachePolicy.MenuPortale)
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
                .Command = "sp_MenuLimbo_EliminaVoceMenuByLingua"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@LNGU_id", LNGU_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@MENL_id", Me.n_MENL_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
				objAccesso.GetExecuteNotQuery(oRequest)
				ObjectBase.PurgeCacheItems(CachePolicy.MenuPortale)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBDelete
            End Try
        End Sub
#End Region

#Region "Creazione MENU"
        Public Shared Function GeneraMenu(ByVal TPRS_id As Integer, ByVal LNGU_ID As Integer, ByVal oFiltroMenuLimbo As Main.FiltroMenuLimbo) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_MenuLimbo_Genera"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@TPRS_id", TPRS_id, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LNGU_id", LNGU_ID, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ForAdmin", CType(oFiltroMenuLimbo, Main.FiltroMenuLimbo), ParameterDirection.Input, DbType.Byte)
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

        Public Shared Function ElencaForOrdineVisualizzazione(ByVal IdMenu As Integer, ByVal PadreID As Integer, ByVal oFiltroMenuLimbo As Main.FiltroMenuLimbo) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataset As New DataSet
            Dim objAccesso As New COL_DataAccess

            Try
                With oRequest
                    .Command = "sp_MenuLimbo_ElencaForOrdineVisualizzazione"
                    .CommandType = CommandType.StoredProcedure

                    oParam = objAccesso.GetParameter("@ForAdmin", CType(oFiltroMenuLimbo, Main.FiltroMenuLimbo), ParameterDirection.Input, DbType.Int32)
                    .Parameters.Add(oParam)

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



		Public Shared Function LazyGeneraMenu(ByVal TipoPersonaID As Integer, ByVal LinguaID As Integer, ByVal oFiltroMenuLimbo As Main.FiltroMenuLimbo, Optional ByVal sortExpression As String = "", Optional ByVal sortDirection As String = "") As GenericCollection(Of MenuElement)
			Dim ListaMenu As New GenericCollection(Of MenuElement)
			Dim cacheKey As String = ""


			If oFiltroMenuLimbo = FiltroMenuLimbo.LimboForAdmin Then
				cacheKey = CachePolicy.MenuPortale(True, TipoPersonaID, LinguaID)
			Else
				cacheKey = CachePolicy.MenuPortale(False, TipoPersonaID, LinguaID)
			End If


			If sortDirection <> String.Empty Then
				sortDirection = sortDirection.ToLower
			End If
			If ObjectBase.Cache(cacheKey) Is Nothing Then
				ListaMenu = COL_MenuLimbo.RetrieveMenuFromDB(TipoPersonaID, LinguaID, oFiltroMenuLimbo)
				ObjectBase.Cache.Insert(cacheKey, ListaMenu, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.ScadenzaSettimanale)
			Else
				ListaMenu = CType(ObjectBase.Cache(cacheKey), GenericCollection(Of MenuElement))
			End If

			If (Not sortExpression Is Nothing AndAlso sortDirection <> String.Empty) Then
				ListaMenu.Sort(New GenericComparer(Of MenuElement)(sortExpression))
			End If

			If (Not sortDirection Is Nothing AndAlso sortDirection = "desc") Then
				ListaMenu.Reverse()
			End If

			Return ListaMenu
		End Function

		Private Shared Function RetrieveMenuFromDB(ByVal TipoPersonaID As Integer, ByVal LinguaID As Integer, ByVal oFiltroMenuLimbo As Main.FiltroMenuLimbo) As GenericCollection(Of MenuElement)
			Dim oRequest As New COL_Request
			Dim oParam As New COL_Request.Parameter
			Dim iDataReader As IDataReader
			Dim objAccesso As New COL_DataAccess

			Dim ListaMenu As New GenericCollection(Of MenuElement)
			With oRequest
				.Command = "sp_MenuLimbo_Genera"
				.CommandType = CommandType.StoredProcedure
				oParam = objAccesso.GetAdvancedParameter("@TPRS_id", TipoPersonaID, ParameterDirection.Input, SqlDbType.Int)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetAdvancedParameter("@LNGU_id", LinguaID, ParameterDirection.Input, SqlDbType.Int)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetAdvancedParameter("@ForAdmin", CType(oFiltroMenuLimbo, Main.FiltroMenuLimbo), ParameterDirection.Input, SqlDbType.Int)
				.Parameters.Add(oParam)
				.Role = COL_Request.UserRole.Admin
				.transactional = False
			End With
			Try
				iDataReader = objAccesso.GetdataReader(oRequest)

				While iDataReader.Read
					ListaMenu.Add(New MenuElement(iDataReader.Item("MENL_ID"), iDataReader.Item("MENL_Padre_Id"), _
					GenericValidator.ValString(iDataReader.Item("MENL_Nome"), ""), GenericValidator.ValString(iDataReader.Item("MENL_LINK"), "")))
				End While
			Catch ex As Exception
				ListaMenu = New GenericCollection(Of MenuElement)
			End Try
			Return ListaMenu
		End Function


    End Class
End Namespace