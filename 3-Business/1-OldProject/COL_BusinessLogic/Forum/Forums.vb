Imports COL_DataLayer
Imports COL_BusinessLogic_v2.Comunita

Namespace Forum

    Public Class COL_Forums

#Region "Private Properties"
        Private n_FRUM_CMNT As New COL_Comunita
        Private n_FRUM_id As Integer
        Private n_FRUM_name As String
        Private n_FRUM_ruoloDefault As Integer
        Private n_FRUM_Description As String
        Private n_FRUM_DateCreated As DateTime
        Private n_FRUM_Moderated As Integer
        Private n_FRUM_NPost As Integer
        Private n_FRUM_NThread As Integer
        Private n_FRUM_Hide As Integer
        Private n_FRUM_PRSN_ID As Integer
        Private n_FRUM_Archiviato As Integer
        Private n_erroreDb As Errori_Db
#End Region

#Region "Public Properties"
        Public Property Comunita() As COL_Comunita
            Get
                Comunita = n_FRUM_CMNT
            End Get
            Set(ByVal Value As COL_Comunita)
                n_FRUM_CMNT = Value
            End Set
        End Property
        Public Property Id() As Integer
            Get
                Id = n_FRUM_id
            End Get
            Set(ByVal Value As Integer)
                n_FRUM_id = Value
            End Set
        End Property
        Public Property Name() As String
            Get
                Name = n_FRUM_name
            End Get
            Set(ByVal Value As String)
                n_FRUM_name = Value
            End Set
        End Property
        Public Property TipoRuoloDefault() As Integer
            Get
                TipoRuoloDefault = n_FRUM_ruoloDefault
            End Get
            Set(ByVal Value As Integer)
                n_FRUM_ruoloDefault = Value
            End Set
        End Property
        Public Property Description() As String
            Get
                Description = n_FRUM_Description
            End Get
            Set(ByVal Value As String)
                n_FRUM_Description = Value
            End Set
        End Property
        Public Property Moderated() As Boolean
            Get
                Moderated = (n_FRUM_Moderated = 1)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    n_FRUM_Moderated = 1
                Else
                    n_FRUM_Moderated = 0
                End If
            End Set
        End Property
        Public Property DateCreated() As DateTime
            Get
                DateCreated = n_FRUM_DateCreated
            End Get
            Set(ByVal Value As DateTime)
                n_FRUM_DateCreated = Value
            End Set
        End Property
        Public ReadOnly Property NPost() As Integer
            Get
                NPost = n_FRUM_NPost
            End Get
        End Property
        Public ReadOnly Property NThread() As Integer
            Get
                NThread = n_FRUM_NThread
            End Get
        End Property
        Public ReadOnly Property Hide() As Integer
            Get
                Hide = n_FRUM_Hide
            End Get
        End Property
        Public Property CreatoreID() As Integer
            Get
                CreatoreID = n_FRUM_PRSN_ID
            End Get
            Set(ByVal Value As Integer)
                n_FRUM_PRSN_ID = Value
            End Set
        End Property
        Public Property isArchiviato() As Boolean
            Get
                isArchiviato = n_FRUM_Archiviato
            End Get
            Set(ByVal Value As Boolean)
                n_FRUM_Archiviato = Value
            End Set
        End Property

        Public ReadOnly Property Errore() As Errori_Db
            Get
                Errore = n_erroreDb
            End Get
        End Property
#End Region


#Region "Private Property"
        Private mFiltroAnagrafica As FltrAnagrafica
        Private mFiltroOrdine As FltrOrdine
        Private mFiltroOrdinamento As FltrOrdinamento
#End Region

#Region "Enum Filtri"
        Public Property FiltroAnagrafica() As FltrAnagrafica
            Get
                FiltroAnagrafica = mFiltroAnagrafica
            End Get
            Set(ByVal Value As FltrAnagrafica)
                mFiltroAnagrafica = Value
            End Set
        End Property
        Public Property FiltroCampoOrdine() As FltrOrdine
            Get
                FiltroCampoOrdine = mFiltroOrdine
            End Get
            Set(ByVal Value As FltrOrdine)
                mFiltroOrdine = Value
            End Set
        End Property
        Public Property FiltroOrdinamento() As FltrOrdinamento
            Get
                FiltroOrdinamento = mFiltroOrdinamento
            End Get
            Set(ByVal Value As FltrOrdinamento)
                mFiltroOrdinamento = Value
            End Set
        End Property

        Public Enum FltrAnagrafica
            nomeCognome = -4
            cognome = -3
            nome = -2
            tutti = -1
            altro = 0
            a = 1
            b = 2
            c = 3
            d = 4
            e = 5
            f = 6
            g = 7
            h = 8
            i = 9
            j = 10
            k = 11
            l = 12
            m = 13
            n = 14
            o = 15
            p = 16
            q = 17
            r = 18
            s = 19
            t = 20
            u = 21
            w = 22
            v = 23
            x = 24
            y = 25
            z = 26
        End Enum
        Public Enum FltrOrdine
            anagrafica = 1
            RuoloComunita = 2
            RuoloForum = 3

        End Enum
        Public Enum FltrOrdinamento
            Crescente = 0
            Decrescente = 1
        End Enum
#End Region

#Region "Metodi New"
        Sub New()
            Me.n_FRUM_Archiviato = 0
            Me.n_erroreDb = Errori_Db.None
        End Sub
#End Region

#Region "Metodi Standard"
        Public Sub Estrai()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Forum_estrai"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@FRUM_Id", Me.n_FRUM_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@FRUM_name", "", ParameterDirection.Output, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@FRUM_Description", "", ParameterDirection.Output, DbType.String, , 150)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@FRUM_RuoloDefault", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@FRUM_CMNT_id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@FRUM_DateCreated", "", ParameterDirection.Output, DbType.DateTime)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@FRUM_Moderated", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@FRUM_NPost", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@FRUM_NThread ", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@FRUM_Hide", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@FRUM_Archiviato", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@FRUM_PRSN_ID", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_FRUM_name = oRequest.GetValueFromParameter(2)
                Me.n_FRUM_Description = oRequest.GetValueFromParameter(3)
                Me.n_FRUM_ruoloDefault = oRequest.GetValueFromParameter(4)

                Me.n_FRUM_CMNT.Id = oRequest.GetValueFromParameter(5)
                Me.n_FRUM_DateCreated = oRequest.GetValueFromParameter(6)
                Me.n_FRUM_Moderated = oRequest.GetValueFromParameter(7)
                Me.n_FRUM_NPost = oRequest.GetValueFromParameter(8)
                Me.n_FRUM_NThread = oRequest.GetValueFromParameter(9)
                Me.n_FRUM_Hide = oRequest.GetValueFromParameter(10)
                Me.n_FRUM_Archiviato = oRequest.GetValueFromParameter(11)
                Me.n_FRUM_PRSN_ID = oRequest.GetValueFromParameter(12)
                Me.n_erroreDb = Errori_Db.None

            Catch ex As Exception
                Me.n_erroreDb = Errori_Db.DBReadExist
            End Try

        End Sub

        Public Sub Aggiungi(ByVal RLPC_idCreatore As Integer)

            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Me.n_FRUM_DateCreated = Now  'data completa (true predefinito)

            With oRequest
                .Command = "sp_Forum_Aggiungi"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@FRUM_id", Me.n_FRUM_id, ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@FRUM_name", Me.n_FRUM_name, ParameterDirection.Input, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@FRUM_Description", Me.n_FRUM_Description, ParameterDirection.Input, DbType.String, , 150)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@FRUM_DateCreated", DateToString(Me.n_FRUM_DateCreated), ParameterDirection.Input, DbType.String, , 30)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@FRUM_CMNT_id", Me.n_FRUM_CMNT.Id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@FRUM_RuoloForumDefault", Me.n_FRUM_ruoloDefault, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_idCreatore", RLPC_idCreatore, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@FRUM_Moderated", Me.n_FRUM_Moderated, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@FRUM_Archiviato", Me.n_FRUM_Archiviato, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                'assegna all'oggetto l'id del forum appena creata
                Me.n_FRUM_id = oRequest.GetValueFromParameter(1)
                Me.n_erroreDb = Errori_Db.None
            Catch ex As Exception
                ' se nn è avvenuto l'inserimento setta a -1 l'id ddel forum
                Me.n_FRUM_id = -1
                Me.n_erroreDb = Errori_Db.DBInsert
            End Try
        End Sub
        Public Sub Modifica()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess


            With oRequest
                .Command = "sp_Forum_Modifica"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@FRUM_Id", Me.n_FRUM_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@FRUM_name", Me.n_FRUM_name, ParameterDirection.Input, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@FRUM_description", Me.n_FRUM_Description, ParameterDirection.Input, DbType.String, , 150)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@FRUM_RuoloDefault", Me.n_FRUM_ruoloDefault, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@FRUM_Moderated", Me.n_FRUM_Moderated, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_erroreDb = Errori_Db.None
            Catch ex As Exception
                Me.n_erroreDb = Errori_Db.DBReadExist
            End Try
        End Sub
        Public Sub Elimina()
            'cancella forumm
            'attenzione!!!
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .CommandType = CommandType.StoredProcedure
                oRequest.Command = "sp_forum_Elimina"
                oParam = objAccesso.GetParameter("@FRUM_Id", Me.n_FRUM_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_erroreDb = Errori_Db.None
            Catch ex As Exception
                Me.n_erroreDb = Errori_Db.DBError
            End Try
        End Sub

        Public Function ElencaByComunita(ByVal IdComunita As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Forum_Elenca"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@FRUM_CMNT_Id", IdComunita, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                dsTable = objAccesso.GetdataSet(oRequest)
                Me.n_erroreDb = Errori_Db.None
            Catch ex As Exception
                Me.n_erroreDb = Errori_Db.DBError
            End Try
            Return dsTable
        End Function
        Public Function ElencaByComunitaIscritto(ByVal PRSN_ID As Integer, ByVal IdComunita As Integer, ByVal RLPC_id As Integer, ByVal oFiltroArchiviazione As Main.FiltroArchiviazione, Optional ByVal ForAdmin As Boolean = False) As DataSet ' = Main.FiltroArchiviazione.NonArchiviato
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Forum_elencaBycomunitaIscritto"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@FRUM_CMNT_Id", IdComunita, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@RLPC_Id", RLPC_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRSN_ID", PRSN_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroArchiviazione", CType(oFiltroArchiviazione, Main.FiltroArchiviazione), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                If ForAdmin Then
                    oParam = objAccesso.GetAdvancedParameter("@ForAdmin", 1, ParameterDirection.Input, SqlDbType.Int)
                Else
                    oParam = objAccesso.GetAdvancedParameter("@ForAdmin", 0, ParameterDirection.Input, SqlDbType.Int)
                End If
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                dsTable = objAccesso.GetdataSet(oRequest)
                Me.n_erroreDb = Errori_Db.None
            Catch ex As Exception
                Me.n_erroreDb = Errori_Db.DBError
            End Try
            Return dsTable
        End Function

        Public Shared Function EliminaForComunita(ByVal CMNT_ID As Integer) As Boolean
            'cancella forumm
            'attenzione!!!
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .CommandType = CommandType.StoredProcedure
                oRequest.Command = "sp_forum_EliminaForComunita"

                oParam = objAccesso.GetParameter("@CMNT_ID", CMNT_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Sub AbilitaDisabilita()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Forum_AbilitaDisabilita"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@FRUM_id", Me.n_FRUM_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch ex As Exception
                Me.n_erroreDb = Errori_Db.DBChange
            End Try
        End Sub
        Public Function VerificaNotificaByPersona(ByVal idPRSN As Integer) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Forum_VerificaNotificaByPersona"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@FRUM_Id", Me.n_FRUM_id, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Id", idPRSN, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LKPF_id", "", ParameterDirection.Output, DbType.Int32, True)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                If oRequest.GetValueFromParameter(3) <> "" Then
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                Me.n_erroreDb = Errori_Db.DBDelete
                Return False
            End Try
        End Function

        Public Sub ModificaNotifica(ByVal PRSN_id As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Forum_modificaNotifica"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@FRUM_Id", Me.n_FRUM_id, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_id", PRSN_id, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_erroreDb = Errori_Db.None
            Catch ex As Exception
                Me.n_erroreDb = Errori_Db.DBError
            End Try
        End Sub
        Public Sub AggiungiNotifica(ByVal PRSN_id As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Forum_aggiungiNotifica"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@FRUM_Id", Me.n_FRUM_id, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_id", PRSN_id, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_erroreDb = Errori_Db.None
            Catch ex As Exception
                Me.n_erroreDb = Errori_Db.DBError
            End Try
        End Sub

        Public Sub RimuoviNotifica(ByVal PRSN_id As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Forum_rimuoviNotifica"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@FRUM_Id", Me.n_FRUM_id, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_id", PRSN_id, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_erroreDb = Errori_Db.None
            Catch ex As Exception
                Me.n_erroreDb = Errori_Db.DBError
            End Try
        End Sub
#End Region

#Region "Metodi Gestione Iscritti"
        'Public Function CreaForum(ByVal RLPC_ID As Integer)

        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim objAccesso As New COL_DataAccess
        '    Me.n_FRUM_DateCreated = Now  'data completa (true predefinito)

        '    With oRequest
        '        .Command = "sp_Forum_Aggiungi"
        '        .CommandType = CommandType.StoredProcedure
        '        oParam = objAccesso.GetParameter("@FRUM_id", Me.n_FRUM_id, ParameterDirection.Output, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@FRUM_name", Me.n_FRUM_name, ParameterDirection.Input, DbType.String, , 50)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@FRUM_Description", Me.n_FRUM_Description, ParameterDirection.Input, DbType.String, , 150)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@FRUM_DateCreated", DateToString(Me.n_FRUM_DateCreated), ParameterDirection.Input, DbType.String, , 30)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@FRUM_CMNT_id", Me.n_FRUM_CMNT.Id, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With

        '    Try
        '        objAccesso.GetExecuteNotQuery(oRequest)
        '        'assegna all'oggetto l'id del forum appena creata
        '        Me.n_FRUM_id = oRequest.GetValueFromParameter(1)

        '    Catch
        '        ' se nn è avvenuto l'inserimento setta a -1 l'id ddel forum
        '        Me.n_FRUM_id = -1
        '        Me.n_erroreDb = Errori_Db.DBInsert
        '    End Try
        'End Function
        Public Function ElencaNoIscritti(ByVal CMNT_id As Integer, ByVal FRUM_id As Integer, ByVal LinguaID As Integer, Optional ByVal TPRL_id As Integer = -1, Optional ByVal pPAGE_SIZE As Integer = -1, Optional ByVal pPAGE_INDEX As Integer = -1, Optional ByVal Valore As String = "", Optional ByVal oAnagrafica As FiltroAnagrafica = Main.FiltroAnagrafica.tutti, Optional ByVal oOrdinamento As FiltroOrdinamento = Main.FiltroOrdinamento.Crescente, Optional ByVal oCampoOrdine As FiltroCampoOrdinePersona = Main.FiltroCampoOrdinePersona.anagrafica) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oTable As New DataSet
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Forum_ElencaNoIscritti"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_Id", CMNT_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Valore", Valore, ParameterDirection.Input, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@anagrafica", CType(oAnagrafica, FiltroAnagrafica), ParameterDirection.Input, DbType.String, , 3)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ordinamento", CType(oOrdinamento, FiltroOrdinamento), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ordinaPer", CType(oCampoOrdine, FiltroCampoOrdinePersona), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PAGE_INDEX", pPAGE_INDEX, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PAGE_SIZE", pPAGE_SIZE, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPRL_ID", TPRL_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LinguaID", LinguaID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@FRUM_id", FRUM_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                oTable = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception
                n_erroreDb = Errori_Db.DBError
            End Try
            Return oTable
        End Function
        Public Function IscriviUtente(ByVal RLPC_id As Integer, Optional ByVal FRUM_ruoloDeault As Integer = -1) As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Forum_IscriviUtente"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@RLPC_id", RLPC_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@FRUM_id", Me.n_FRUM_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@FRUM_ruoloDefault", FRUM_ruoloDeault, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@RuoloAssociato", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Return oRequest.GetValueFromParameter(4)
            Catch ex As Exception
                Return -1
                Me.n_erroreDb = Errori_Db.DBInsert
            End Try
        End Function

        Public Sub GetRuoloDefault()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Forum_getRuoloDefault"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@FRUM_Id", Me.n_FRUM_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@FRUM_RuoloDefault", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_FRUM_ruoloDefault = oRequest.GetValueFromParameter(2)
                Me.n_erroreDb = Errori_Db.None
            Catch ex As Exception
                Me.n_erroreDb = Errori_Db.DBReadExist
            End Try

        End Sub
        Public Function ElencaIscritti()
            'elenca tutti gli iscritti al forum
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataSet As New DataSet
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Forum_ElencaIscritti"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@FRUM_Id", Me.n_FRUM_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                oDataSet = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception
                Me.n_erroreDb = Errori_Db.DBError
            End Try
            Return oDataSet
        End Function

        Public Sub CambiaRuoloIscritto(ByVal TPRF_id As Integer, ByVal RLPC_id As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Forum_CambiaRuoloIscritto"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@FRUM_Id", Me.n_FRUM_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPF_TPRF_id", TPRF_id, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_id", RLPC_id, , DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_erroreDb = Errori_Db.None
            Catch ex As Exception
                Me.n_erroreDb = Errori_Db.DBChange
            End Try
        End Sub
        Public Sub DisiscriviUtente(ByVal RLPC_id As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Forum_DisiscriviUtente"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@FRUM_Id", Me.n_FRUM_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPF_RLPC_id", RLPC_id, , DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch ex As Exception
                Me.n_erroreDb = Errori_Db.DBChange
            End Try
        End Sub
        Public Sub bloccaIscritto(ByVal RLPC_ID As Integer, ByVal Abilitato As Boolean)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Forum_bloccasbloccaIscritto"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@RLPC_id", RLPC_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@FRUM_id", Me.n_FRUM_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Abilitato", Abilitato, ParameterDirection.Input, DbType.Boolean)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch ex As Exception
                Me.n_erroreDb = Errori_Db.DBChange
            End Try
        End Sub

        Public Function GetPersoneIscrittePaginateTotale(ByVal TPRF_id As Integer, ByVal TPRL_id As Integer, Optional ByVal pNome As String = "", Optional ByVal pCognome As String = "") As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Integer
            Dim oDataset As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Forum_GetPersoneIscrittePaginate_Totale"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@FRUM_id", Me.n_FRUM_id, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPRF_id", TPRF_id, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@ANAGRAFICA", CType(Me.FiltroAnagrafica, FltrAnagrafica), , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@ORDINE", CType(Me.FiltroCampoOrdine, FltrOrdine), , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@ORDINAMENTO", CType(Me.FiltroOrdinamento, FltrOrdinamento), , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@TPRL_id", TPRL_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                If pNome <> "" Then
                    oParam = objAccesso.GetParameter("@Nome", pNome, ParameterDirection.Input, DbType.String, True, 40)
                    .Parameters.Add(oParam)
                End If

                If pCognome <> "" Then
                    oParam = objAccesso.GetParameter("@Cognome", pCognome, ParameterDirection.Input, DbType.String, True, 40)
                    .Parameters.Add(oParam)
                End If


                .Role = COL_Request.UserRole.Admin
                .transactional = False

            End With
            Try
                oDataset = objAccesso.GetdataSet(oRequest)
                If oDataset.HasErrors = False Then
                    If oDataset.Tables.Count > 0 Then
                        If oDataset.Tables(0).Rows.Count > 0 Then
                            iResponse = oDataset.Tables(0).Rows(0).Item("Totale")
                        End If
                    End If
                End If

            Catch ex As Exception
                iResponse = 0
            End Try
            Return iResponse
        End Function
        Public Function GetPersoneIscrittePaginate(ByVal LNGU_id As Integer, ByVal TPRF_id As Integer, ByVal pPAGE_SIZE As Integer, ByVal pPAGE_INDEX As Integer, ByVal TPRL_id As Integer, Optional ByVal pNome As String = "", Optional ByVal pCognome As String = "") As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Forum_GetPersoneIscrittePaginate"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@FRUM_ID", Me.n_FRUM_id, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPRF_id", TPRF_id, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PAGE_SIZE", pPAGE_SIZE, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@ANAGRAFICA", CType(Me.FiltroAnagrafica, FltrAnagrafica), , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PAGE_INDEX", pPAGE_INDEX, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@ORDINE", CType(Me.FiltroCampoOrdine, FltrOrdine), , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@ORDINAMENTO", CType(Me.FiltroOrdinamento, FltrOrdinamento), , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@TPRL_id", TPRL_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LNGU_id", LNGU_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                If pNome <> "" Then
                    oParam = objAccesso.GetParameter("@Nome", pNome, ParameterDirection.Input, DbType.String, True, 40)
                    .Parameters.Add(oParam)
                End If

                If pCognome <> "" Then
                    oParam = objAccesso.GetParameter("@Cognome", pCognome, ParameterDirection.Input, DbType.String, True, 40)
                    .Parameters.Add(oParam)
                End If

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

        Public Function ElencaModeratori() As DataSet
            'elenca tutti i moderatori di un forum
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataSet As New DataSet
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Forum_ElencaModeratori"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@FRUM_Id", Me.n_FRUM_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                oDataSet = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception
                Me.n_erroreDb = Errori_Db.DBError
            End Try
            Return oDataSet
        End Function
#End Region
#Region "Statistiche Iscritti"
        Public Function StatistichePostComunita(ByVal IdComunita As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Forum_StatisticheCMNT"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_Id", IdComunita, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                oTable = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception
                n_erroreDb = Errori_Db.DBError
            End Try
            Return oTable
        End Function

        Public Function StatistichePostForum(ByVal IdForum As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Forum_StatisticheFRUM"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@FRUM_Id", IdForum, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                oTable = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception
                n_erroreDb = Errori_Db.DBError
            End Try
            Return oTable
        End Function
#End Region

        Public Function getRuoloForIscritto(ByVal RLPC_ID As Integer, ByVal subscribe As Boolean) As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Forum_getRuoloForIscritto"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@FRUM_Id", Me.n_FRUM_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@RLPC_ID", RLPC_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                If subscribe Then
                    oParam = objAccesso.GetParameter("@subscribe", 1, ParameterDirection.Input, DbType.Int32)
                Else
                    oParam = objAccesso.GetParameter("@subscribe", 0, ParameterDirection.Input, DbType.Int32)
                End If
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@RuoloAssociato", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Return oRequest.GetValueFromParameter(4)
                Me.n_erroreDb = Errori_Db.None
            Catch ex As Exception
                Me.n_erroreDb = Errori_Db.DBReadExist
            End Try
            Return 0
        End Function


        Public Function HasForumAssociatiForUtente(ByVal RLPC_ID As Integer, ByVal CMNT_ID As Integer, ByRef totaleArchiviati As Integer, ByRef totaleAttivi As Integer) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Forum_HasForumAssociatiForUtente"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@FRUM_CMNT_Id", CMNT_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@RLPC_ID", RLPC_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@totaleArchiviati", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@totaleAttivi", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)

                totaleArchiviati = oRequest.GetValueFromParameter(3)
                totaleAttivi = oRequest.GetValueFromParameter(4)
                Me.n_erroreDb = Errori_Db.None
            Catch ex As Exception
                Me.n_erroreDb = Errori_Db.DBReadExist
            End Try
            Return True
        End Function

        Public Function getLastPostID() As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Forum_getLastPostID"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@FRUM_Id", Me.n_FRUM_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@LastPostID", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Return oRequest.GetValueFromParameter(2)
                Me.n_erroreDb = Errori_Db.None
            Catch ex As Exception
                Me.n_erroreDb = Errori_Db.DBReadExist
            End Try
            Return 0
        End Function

        Public Shared Function ModificaArchiviazione(ByVal ForumID As Integer) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Forum_ModificaArchiviazione"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@FRUM_ID", ForumID, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Function UtenteHasAccesso(ByVal RLPC_ID As Integer) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Forum_UtenteHasAccesso"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@FRUM_ID", Me.n_FRUM_id, ParameterDirection.Input, SqlDbType.Int, True)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@RLPC_ID", RLPC_ID, ParameterDirection.Input, SqlDbType.Int, True)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@isAbilitato", "", ParameterDirection.Output, SqlDbType.Int, True)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)

                If oRequest.GetValueFromParameter(3) = 1 Then
                    Return True
                Else
                    Return False
                End If

            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Shared Function Vota(ByVal ForumID As Integer, ByVal PRSN_ID As Integer, ByVal Voto As Integer) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Forum_Vota"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@ForumID", ForumID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_ID", PRSN_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@Voto", Voto, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Return True
            Catch ex As Exception
                Return False
            End Try
            Return False
        End Function


        Public Shared Sub RecuperaDettagli(ByRef ForumID As Integer, ByRef TopicID As Integer, ByRef PostID As Integer, ByRef ComunitaID As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Forum_RecuperaDettagli"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@ForumID", ForumID, ParameterDirection.InputOutput, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@TopicID", TopicID, ParameterDirection.InputOutput, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PostID", PostID, ParameterDirection.InputOutput, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ComunitaID", ComunitaID, ParameterDirection.InputOutput, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Try
                    ForumID = oRequest.GetValueFromParameter(1)
                Catch ex As Exception

                End Try
                Try
                    TopicID = oRequest.GetValueFromParameter(2)
                Catch ex As Exception

                End Try
                Try
                    PostID = oRequest.GetValueFromParameter(3)
                Catch ex As Exception

                End Try
                Try
                    ComunitaID = oRequest.GetValueFromParameter(4)
                Catch ex As Exception

                End Try
            Catch ex As Exception

            End Try
        End Sub


    
    End Class

End Namespace