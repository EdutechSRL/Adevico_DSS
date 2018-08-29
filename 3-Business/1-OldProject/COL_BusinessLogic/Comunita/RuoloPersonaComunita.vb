Imports COL_DataLayer

Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.CL_permessi


Namespace Comunita
    Public Class COL_RuoloPersonaComunita
        Inherits ObjectBase
#Region "Private Property"
        Private n_RLPC_id As Integer
        Private n_RLPC_CMNT As New COL_Comunita
        Private n_RLPC_PRSN As New COL_Persona
        Private n_RLPC_TPRL As New COL_TipoRuolo
        Private n_RLPC_attivato As Integer
        Private n_RLPC_abilitato As Integer
        Private n_RLPC_responsabile As Integer
        Private n_RLPC_UltimoCollegamento As DateTime
        Private n_RLPC_PenultimoCollegamento As DateTime
        Private n_RLPC_IscrittoIl As DateTime
        Private n_RLPC_isConnesso As Integer
        Private n_RLPC_skipCover As Integer
        Private n_Errore As Errori_Db
#End Region

#Region "Public Property"
        Public Property Id() As Integer
            Get
                Id = n_RLPC_id
            End Get
            Set(ByVal Value As Integer)
                n_RLPC_id = Value
            End Set
        End Property
        Public Property Comunita() As COL_Comunita
            Get
                Comunita = n_RLPC_CMNT
            End Get
            Set(ByVal Value As COL_Comunita)
                n_RLPC_CMNT = Value
            End Set
        End Property
        Public Property Persona() As COL_Persona
            Get
                Persona = n_RLPC_PRSN
            End Get
            Set(ByVal Value As COL_Persona)
                n_RLPC_PRSN = Value
            End Set
        End Property
        Public Property TipoRuolo() As COL_TipoRuolo
            Get
                TipoRuolo = n_RLPC_TPRL
            End Get
            Set(ByVal Value As COL_TipoRuolo)
                n_RLPC_TPRL = Value
            End Set
        End Property
        Public Property Attivato() As Boolean
            Get
                Attivato = CBool(n_RLPC_attivato = 1)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    n_RLPC_attivato = 1
                Else
                    n_RLPC_attivato = 0
                End If
            End Set
        End Property
        Public Property Abilitato() As Boolean
            Get
                Abilitato = CBool(n_RLPC_abilitato = 1)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    n_RLPC_abilitato = 1
                Else
                    n_RLPC_abilitato = 0
                End If
            End Set
        End Property
        Public Property isResponsabile() As Boolean
            Get
                isResponsabile = CBool(n_RLPC_responsabile = 1)
            End Get
            Set(ByVal Value As Boolean)

                If Value Then
                    n_RLPC_responsabile = 1
                Else
                    n_RLPC_responsabile = 0
                End If
            End Set
        End Property
        Public Property UltimoCollegamento() As DateTime
            Get
                UltimoCollegamento = n_RLPC_UltimoCollegamento
            End Get
            Set(ByVal Value As DateTime)
                n_RLPC_UltimoCollegamento = Value
            End Set
        End Property
        Public Property PenultimoCollegamento() As DateTime
            Get
                PenultimoCollegamento = n_RLPC_PenultimoCollegamento
            End Get
            Set(ByVal Value As DateTime)
                n_RLPC_PenultimoCollegamento = Value
            End Set
        End Property
        Public Property IscrittoIl() As DateTime
            Get
                IscrittoIl = n_RLPC_IscrittoIl
            End Get
            Set(ByVal Value As DateTime)
                n_RLPC_IscrittoIl = Value
            End Set
        End Property
        Public Property isConnesso() As Boolean
            Get
                isConnesso = CBool(n_RLPC_isConnesso = 1)
            End Get
            Set(ByVal Value As Boolean)

                If Value Then
                    n_RLPC_isConnesso = 1
                Else
                    n_RLPC_isConnesso = 0
                End If
            End Set
        End Property
        Public Property SaltaCopertina() As Boolean
            Get
                SaltaCopertina = CBool(n_RLPC_skipCover = 1)
            End Get
            Set(ByVal Value As Boolean)

                If Value Then
                    n_RLPC_skipCover = 1
                Else
                    n_RLPC_skipCover = 0
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
            Me.n_RLPC_isConnesso = 0

            Me.n_Errore = Errori_Db.None
        End Sub
#End Region


        Public Sub Estrai(ByVal CMNT_ID As Integer, ByVal PRSN_ID As Integer)
            'in base alla comunità e all'id della persona mi ritorna il ruolo
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_RuoloPersonaComunita_Estrai"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@RLPC_PRSN_id", PRSN_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_CMNT_id", CMNT_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@RLPC_Id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_TPRL_id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_TPRL_Nome", "", ParameterDirection.Output, DbType.String, , 100)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@RLPC_attivato", "", ParameterDirection.Output, DbType.Byte)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_abilitato", "", ParameterDirection.Output, DbType.Byte)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_responsabile", "", ParameterDirection.Output, DbType.Byte)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_UltimoCollegamento", "", ParameterDirection.Output, DbType.DateTime)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_TPRL_Gerarchia", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@RLPC_PenultimoCollegamento", "", ParameterDirection.Output, DbType.DateTime)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_IscrittoIl", "", ParameterDirection.Output, DbType.DateTime)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_isConnesso", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_SkipCover", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_RLPC_PRSN.Id = PRSN_ID
                Me.n_RLPC_CMNT.Id = CMNT_ID
                Me.n_RLPC_id = oRequest.GetValueFromParameter(3)
                Me.TipoRuolo.Id = oRequest.GetValueFromParameter(4)
                Me.TipoRuolo.Nome = oRequest.GetValueFromParameter(5)
                Me.n_RLPC_attivato = oRequest.GetValueFromParameter(6)
                Me.n_RLPC_abilitato = oRequest.GetValueFromParameter(7)
                Me.n_RLPC_responsabile = oRequest.GetValueFromParameter(8)
                If oRequest.GetValueFromParameter(9) <> "" Then
                    If IsDate(oRequest.GetValueFromParameter(9)) Then
                        Me.n_RLPC_UltimoCollegamento = oRequest.GetValueFromParameter(9)
                    End If
                End If
                Me.TipoRuolo.Gerarchia = oRequest.GetValueFromParameter(10)
                If oRequest.GetValueFromParameter(11) <> "" Then
                    If IsDate(oRequest.GetValueFromParameter(11)) Then
                        Me.n_RLPC_PenultimoCollegamento = oRequest.GetValueFromParameter(11)
                    End If
                End If
                If oRequest.GetValueFromParameter(12) <> "" Then
                    If IsDate(oRequest.GetValueFromParameter(12)) Then
                        Me.n_RLPC_IscrittoIl = oRequest.GetValueFromParameter(12)
                    End If
                End If
                Me.n_RLPC_isConnesso = oRequest.GetValueFromParameter(13)
                Try
                    Me.n_RLPC_skipCover = oRequest.GetValueFromParameter(14)
                Catch ex As Exception
                    Me.n_RLPC_skipCover = 0
                End Try

                Me.n_Errore = Errori_Db.None
            Catch ex As Exception 'record non trovato

                Me.n_Errore = Errori_Db.DBReadExist
            End Try
        End Sub

        Public Sub EstraiByLingua(ByVal CMNT_ID As Integer, ByVal PRSN_ID As Integer, ByVal LinguaID As Integer)
            'in base alla comunità e all'id della persona mi ritorna il ruolo
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_RuoloPersonaComunita_EstraiByLingua"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@RLPC_PRSN_id", PRSN_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_CMNT_id", CMNT_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@RLPC_Id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_TPRL_id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_TPRL_Nome", "", ParameterDirection.Output, DbType.String, , 100)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@RLPC_attivato", "", ParameterDirection.Output, DbType.Byte)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_abilitato", "", ParameterDirection.Output, DbType.Byte)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_responsabile", "", ParameterDirection.Output, DbType.Byte)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_UltimoCollegamento", "", ParameterDirection.Output, DbType.DateTime)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_TPRL_Gerarchia", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@RLPC_PenultimoCollegamento", "", ParameterDirection.Output, DbType.DateTime)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_IscrittoIl", "", ParameterDirection.Output, DbType.DateTime)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_isConnesso", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@RLPC_SkipCover", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@LinguaID", LinguaID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_RLPC_PRSN.Id = PRSN_ID
                Me.n_RLPC_CMNT.Id = CMNT_ID
                Me.n_RLPC_id = oRequest.GetValueFromParameter(3)
                Me.TipoRuolo.Id = oRequest.GetValueFromParameter(4)
                Me.TipoRuolo.Nome = oRequest.GetValueFromParameter(5)
                Me.n_RLPC_attivato = oRequest.GetValueFromParameter(6)
                Me.n_RLPC_abilitato = oRequest.GetValueFromParameter(7)
                Me.n_RLPC_responsabile = oRequest.GetValueFromParameter(8)
                If oRequest.GetValueFromParameter(9) <> "" Then
                    If IsDate(oRequest.GetValueFromParameter(9)) Then
                        Me.n_RLPC_UltimoCollegamento = oRequest.GetValueFromParameter(9)
                    End If
                End If
                Me.TipoRuolo.Gerarchia = oRequest.GetValueFromParameter(10)
                If oRequest.GetValueFromParameter(11) <> "" Then
                    If IsDate(oRequest.GetValueFromParameter(11)) Then
                        Me.n_RLPC_PenultimoCollegamento = oRequest.GetValueFromParameter(11)
                    End If
                End If
                If oRequest.GetValueFromParameter(12) <> "" Then
                    If IsDate(oRequest.GetValueFromParameter(12)) Then
                        Me.n_RLPC_IscrittoIl = oRequest.GetValueFromParameter(12)
                    End If
                End If
                Me.n_RLPC_isConnesso = oRequest.GetValueFromParameter(13)
                Try
                    Me.n_RLPC_skipCover = oRequest.GetValueFromParameter(14)
                Catch ex As Exception
                    Me.n_RLPC_skipCover = 0
                End Try
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception 'record non trovato

                Me.n_Errore = Errori_Db.DBReadExist
            End Try
        End Sub
        Public Sub EstraiByLinguaDefault(ByVal CMNT_ID As Integer, ByVal PRSN_ID As Integer)
            'in base alla comunità e all'id della persona mi ritorna il ruolo
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_RuoloPersonaComunita_EstraiByLinguaDefault"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@RLPC_PRSN_id", PRSN_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_CMNT_id", CMNT_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@RLPC_Id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_TPRL_id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_TPRL_Nome", "", ParameterDirection.Output, DbType.String, , 100)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@RLPC_attivato", "", ParameterDirection.Output, DbType.Byte)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_abilitato", "", ParameterDirection.Output, DbType.Byte)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_responsabile", "", ParameterDirection.Output, DbType.Byte)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_UltimoCollegamento", "", ParameterDirection.Output, DbType.DateTime)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_TPRL_Gerarchia", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@RLPC_PenultimoCollegamento", "", ParameterDirection.Output, DbType.DateTime)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_IscrittoIl", "", ParameterDirection.Output, DbType.DateTime)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_isConnesso", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_SkipCover", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_RLPC_PRSN.Id = PRSN_ID
                Me.n_RLPC_CMNT.Id = CMNT_ID
                Me.n_RLPC_id = oRequest.GetValueFromParameter(3)
                Me.TipoRuolo.Id = oRequest.GetValueFromParameter(4)
                Me.TipoRuolo.Nome = oRequest.GetValueFromParameter(5)
                Me.n_RLPC_attivato = oRequest.GetValueFromParameter(6)
                Me.n_RLPC_abilitato = oRequest.GetValueFromParameter(7)
                Me.n_RLPC_responsabile = oRequest.GetValueFromParameter(8)
                If oRequest.GetValueFromParameter(9) <> "" Then
                    If IsDate(oRequest.GetValueFromParameter(9)) Then
                        Me.n_RLPC_UltimoCollegamento = oRequest.GetValueFromParameter(9)
                    End If
                End If
                Me.TipoRuolo.Gerarchia = oRequest.GetValueFromParameter(10)
                If oRequest.GetValueFromParameter(11) <> "" Then
                    If IsDate(oRequest.GetValueFromParameter(11)) Then
                        Me.n_RLPC_PenultimoCollegamento = oRequest.GetValueFromParameter(11)
                    End If
                End If
                If oRequest.GetValueFromParameter(12) <> "" Then
                    If IsDate(oRequest.GetValueFromParameter(12)) Then
                        Me.n_RLPC_IscrittoIl = oRequest.GetValueFromParameter(12)
                    End If
                End If
                Me.n_RLPC_isConnesso = oRequest.GetValueFromParameter(13)
                Try
                    Me.n_RLPC_skipCover = oRequest.GetValueFromParameter(14)
                Catch ex As Exception
                    Me.n_RLPC_skipCover = 0
                End Try
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception 'record non trovato

                Me.n_Errore = Errori_Db.DBReadExist
            End Try
        End Sub


        Public Sub Aggiungi(Optional ByVal responsabile As Boolean = False)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_RuoloPersonaComunita_Aggiungi"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@RLPC_Id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_CMNT_Id", n_RLPC_CMNT.Id, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_PRSN_ID", n_RLPC_PRSN.Id, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_TPRL_id", n_RLPC_TPRL.Id, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_attivato", n_RLPC_attivato, , DbType.Byte)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_abilitato", n_RLPC_abilitato, , DbType.Byte)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_responsabile", n_RLPC_responsabile, , DbType.Byte)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                ' Recupero l'ID univoco del ruoloPersonaComunità
                Me.n_RLPC_id = oRequest.GetValueFromParameter(1)
            Catch ex As Exception
                Me.n_RLPC_id = -1
                Me.n_Errore = Errori_Db.DBInsert
            End Try
        End Sub
        '##############################################################################
        Public Function Modifica() As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_RuoloPersonaComunita_Modifica"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@RLPC_Id", n_RLPC_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_CMNT_Id", n_RLPC_CMNT.Id, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_PRSN_ID", n_RLPC_PRSN.Id, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_TPRL_id", n_RLPC_TPRL.Id, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_attivato", n_RLPC_attivato, , DbType.Byte)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_abilitato", n_RLPC_abilitato, , DbType.Byte)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_responsabile", Me.n_RLPC_responsabile, , DbType.Byte)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBChange
            End Try
        End Function

        Public Shared Sub AbilitaAttiva(ByVal RLPC_id As Integer, ByVal eAbilita As Abilitazione)
            'passo il valore dell'enum direttamente alla sp che gestirà i valori passati
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_RuoloPersonaComunita_AbilitaAttiva"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@RLPC_Id", RLPC_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_attivato", CType(eAbilita, Abilitazione), , DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch ex As Exception
                'gestire errori delle shared
            End Try
        End Sub

        Public Sub CaricaRuoloPersonaComunita()
            'passando l'id della persona e quello della comunità ritorna l'id del ruolo_persona_comunita
            'e quello del ruolo espletato nella relazione
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_RuoloPersonaComunita_IdandRuolo"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@RLPC_Id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_Ruolo", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_CMNT_Id", n_RLPC_CMNT.Id, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_PRSN_ID", n_RLPC_PRSN.Id, , DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                ' Recupero l'ID univoco del ruoloPersonaComunità
                Me.n_RLPC_id = oRequest.GetValueFromParameter(1)
                ' carico l'id del ruolo
                Me.n_RLPC_TPRL.Id = oRequest.GetValueFromParameter(2)
            Catch ex As Exception
                Me.n_RLPC_id = -1
                Me.n_RLPC_TPRL.Id = -1
                Me.n_Errore = Errori_Db.DBInsert
            End Try
        End Sub

        Public Function CambiaRuolo(ByVal PersonID As Integer) As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_RuoloPersonaComunita_Cambiaruolo"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@RLPC_Id", n_RLPC_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_TPRL_id", n_RLPC_TPRL.Id, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_responsabile", Me.n_RLPC_responsabile, , DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                ObjectBase.PurgeCacheItems(CachePolicy.PermessiServizioUtente, "_" & PersonID.tostring)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBChange
            End Try
        End Function

        Public Function UpdateUltimocollegamento() As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            Me.n_RLPC_UltimoCollegamento = Now
            With oRequest
                .Command = "sp_RuoloPersonaComunita_UpdateUltimocollegamento"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@RLPC_PRSN_id", Me.n_RLPC_PRSN.Id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_CMNT_id", n_RLPC_CMNT.Id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_UltimoCollegamento", DateToString(Me.n_RLPC_UltimoCollegamento, True), ParameterDirection.Input, DbType.String)
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
        End Function

        Public Sub Elimina()
            'disiscrive un utente da una comunità senza eseguire controlli (funzione temporanea da integrare con controlli)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_RuoloPersonaComunita_Elimina"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@RLPC_id", Me.n_RLPC_id, ParameterDirection.Input, DbType.Int32)
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

        Public Sub EliminaInAttesa_(Optional ByVal tutti As Boolean = False)
            'disiscrive un utente da una comunità mentre è in attesa

            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_RuoloPersonaComunita_EliminaInAttesa"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@RLPC_id", Me.n_RLPC_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@tutti", tutti, ParameterDirection.Input, DbType.Boolean)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_CMNT_id", Me.n_RLPC_CMNT.Id, ParameterDirection.Input, DbType.Int32)
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

        Public Shared Function skipCover(ByVal CMNT_id As Integer, ByVal PRSN_ID As Integer, ByVal Skip As Boolean) As Boolean
            'disiscrive un utente da una comunità mentre è in attesa

            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_RuoloPersonaComunita_skipCover"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@CMNT_id", CMNT_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_ID", PRSN_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                If Skip Then
                    oParam = objAccesso.GetAdvancedParameter("@Skip", 1, ParameterDirection.Input, SqlDbType.Int)
                Else
                    oParam = objAccesso.GetAdvancedParameter("@Skip", 0, ParameterDirection.Input, SqlDbType.Int)
                End If

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
		Public Shared Function isSkipCover(ByVal ComunitaID As Integer, ByVal PersonaID As Integer) As Boolean
			'disiscrive un utente da una comunità mentre è in attesa

			Dim oRequest As New COL_Request
			Dim oParam As New COL_Request.Parameter
			Dim objAccesso As New COL_DataAccess

			With oRequest
				.Command = "sp_RuoloPersonaComunita_isSkipCover"
				.CommandType = CommandType.StoredProcedure
				oParam = objAccesso.GetAdvancedParameter("@CMNT_id", ComunitaID, ParameterDirection.Input, SqlDbType.Int)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetAdvancedParameter("@PRSN_ID", PersonaID, ParameterDirection.Input, SqlDbType.Int)
				.Parameters.Add(oParam)

				oParam = objAccesso.GetAdvancedParameter("@Skip", 0, ParameterDirection.Output, SqlDbType.Int)
				.Parameters.Add(oParam)
				.Role = COL_Request.UserRole.Admin
				.transactional = False
			End With
			Try
				objAccesso.GetExecuteNotQuery(oRequest)
				If oRequest.GetValueFromParameter(3) = 0 Then
					Return False
				Else
					Return True
				End If
			Catch ex As Exception
				Return False
			End Try

		End Function
		'Public Function isSkipCover(ByVal CMNT_id As Integer, ByVal PRSN_ID As Integer) As Boolean
		'    'disiscrive un utente da una comunità mentre è in attesa

		'    Dim oRequest As New COL_Request
		'    Dim oParam As New COL_Request.Parameter
		'    Dim objAccesso As New COL_DataAccess

		'    With oRequest
		'        .Command = "sp_RuoloPersonaComunita_isSkipCover"
		'        .CommandType = CommandType.StoredProcedure
		'        oParam = objAccesso.GetAdvancedParameter("@CMNT_id", CMNT_id, ParameterDirection.Input, SqlDbType.Int)
		'        .Parameters.Add(oParam)
		'        oParam = objAccesso.GetAdvancedParameter("@PRSN_ID", PRSN_ID, ParameterDirection.Input, SqlDbType.Int)
		'        .Parameters.Add(oParam)

		'        oParam = objAccesso.GetAdvancedParameter("@Skip", 0, ParameterDirection.Output, SqlDbType.Int)
		'        .Parameters.Add(oParam)
		'        .Role = COL_Request.UserRole.Admin
		'        .transactional = False
		'    End With
		'    Try
		'        objAccesso.GetExecuteNotQuery(oRequest)
		'        If oRequest.GetValueFromParameter(3) = 0 Then
		'            Return False
		'        Else
		'            Return True
		'        End If
		'    Catch ex As Exception
		'        Return False
		'    End Try

		'End Function
    End Class

End Namespace