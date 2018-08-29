Imports COL_DataLayer
Imports COL_BusinessLogic_v2.Localizzazione
Imports COL_BusinessLogic_v2.Comol.Manager

Namespace Forum
    Public Class COL_Forum_threads
#Region "Private Properties"
        Private Delegate Sub MailDelegate(ByVal dsTable As DataSet, ByVal oResourceConfig As ResourceManager, ByVal oPersona As CL_persona.COL_Persona, ByVal Percorso As String)
        Private n_THRD_id As Integer
        Private n_THRD_NReply As Integer
        Private n_THRD_NView As Integer
        Private n_THRD_PostDate As DateTime
        Private n_THRD_Hide As Integer
        Private n_THRD_PRSN_Id As Integer
        Private n_THRD_FRUM_id As Integer
        Private n_THRD_FRIM_id As Integer
        Private n_THRD_Subject As String
        Private n_THRD_Notifica As Integer
        Private n_erroreDb As Errori_Db
#End Region

#Region "Public Properties"
        Public Property Id() As Integer
            Get
                Id = n_THRD_id
            End Get
            Set(ByVal Value As Integer)
                n_THRD_id = Value
            End Set
        End Property
        Public Property NReply() As Integer
            Get
                NReply = n_THRD_NReply
            End Get
            Set(ByVal Value As Integer)
                n_THRD_NReply = Value
            End Set
        End Property
        Public Property NView() As Integer
            Get
                NView = n_THRD_NView
            End Get
            Set(ByVal Value As Integer)
                n_THRD_NView = Value
            End Set
        End Property

        Public Property PRSN_Id() As Integer
            Get
                PRSN_Id = n_THRD_PRSN_Id
            End Get
            Set(ByVal Value As Integer)
                n_THRD_PRSN_Id = Value
            End Set
        End Property
        Public Property Subject() As String
            Get
                Subject = n_THRD_Subject
            End Get
            Set(ByVal Value As String)
                n_THRD_Subject = Value
            End Set
        End Property
        Public Property PostDate() As DateTime
            Get
                PostDate = n_THRD_PostDate
            End Get
            Set(ByVal Value As DateTime)
                n_THRD_PostDate = Value
            End Set
        End Property
        Public Property Hide() As Integer
            Get
                Hide = n_THRD_Hide
            End Get
            Set(ByVal Value As Integer)
                n_THRD_Hide = Value
            End Set
        End Property
        Public Property ThreadImage() As Integer
            Get
                ThreadImage = n_THRD_FRIM_id
            End Get
            Set(ByVal Value As Integer)
                n_THRD_FRIM_id = Value
            End Set
        End Property
        Public Property ThreadForum() As Integer
            Get
                ThreadForum = n_THRD_FRUM_id
            End Get
            Set(ByVal Value As Integer)
                n_THRD_FRUM_id = Value
            End Set
        End Property
        Public Property NotificaMail() As Boolean
            Get
                NotificaMail = (n_THRD_Notifica = 1)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    n_THRD_Notifica = 1
                Else
                    n_THRD_Notifica = 0
                End If
            End Set
        End Property
        Public ReadOnly Property Errore() As Errori_Db
            Get
                Errore = n_erroreDb
            End Get
        End Property
#End Region

#Region "Metodi New"
        Sub New()
            Me.n_THRD_FRIM_id = 1
            Me.n_THRD_Notifica = 0
            Me.n_erroreDb = Errori_Db.None
        End Sub
#End Region

#Region "Metodi Standard"
        Public Sub Aggiungi(ByVal ForumID As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim NReply As Integer = 0
            Dim NView As Integer = 0
            Dim Hide As Integer = 0

            Me.n_THRD_PostDate = DateTime.Now
            With oRequest
                .Command = "sp_ForumThread_aggiungi"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@THRD_Id", "", ParameterDirection.Output, DbType.Int32, True)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@THRD_FRUM_Id", ForumID, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@THRD_PRSN_Id", Me.n_THRD_PRSN_Id, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@THRD_NReply", 0, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@THRD_NView", 0, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@THRD_Subject", Me.n_THRD_Subject, ParameterDirection.Input, DbType.String, True, 100)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@THRD_PostDate", DateToString(Me.n_THRD_PostDate, True), ParameterDirection.Input, DbType.String, True, 30)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@THRD_hide", 0, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@THRD_FRIM_id", Me.n_THRD_FRIM_id, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@THRD_Notifica", Me.n_THRD_Notifica, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_THRD_id = oRequest.GetValueFromParameter(1)
            Catch ex As Exception
                Me.n_THRD_id = -1
                Me.n_erroreDb = Errori_Db.DBInsert
            End Try
        End Sub
        Public Function Elimina() As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_ForumThread_Elimina"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@THRD_Id", Me.n_THRD_id, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Return True
            Catch ex As Exception
                Me.n_erroreDb = Errori_Db.DBDelete
                Return False
            End Try
        End Function
        Public Function Elimina(ByVal IdThread As Integer) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_ForumThread_Elimina"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@THRD_Id", IdThread, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Return True
            Catch ex As Exception
                Me.n_erroreDb = Errori_Db.DBDelete
                Return False
            End Try
        End Function
        Public Function Mostra() As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_ForumThread_nascondi"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@THRD_Id", Me.n_THRD_id, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@THRD_Hide", 0, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                If objAccesso.GetExecuteNotQuery(oRequest) = 1 Then
                    Return True
                End If
            Catch ex As Exception
                Return False
            End Try
            Return False
        End Function
        Public Function Nascondi() As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_ForumThread_nascondi"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@THRD_Id", Me.n_THRD_id, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@THRD_Hide", 1, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                If objAccesso.GetExecuteNotQuery(oRequest) = 1 Then
                    Return True
                End If
            Catch ex As Exception
                Return False
            End Try
            Return False
        End Function
#End Region

#Region "Metodi"
        Public Function ElencaThreadByForum(ByVal PRSN_ID As Integer, ByVal IdForum As Integer, ByVal oVisibile As Main.FiltroVisibilità, ByVal oFiltroArchiviazione As Main.FiltroArchiviazione, ByVal ForAdmin As Boolean) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "SP_ForumThread_ElencaByForum"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@THRD_FRUM_Id", IdForum, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_ID", PRSN_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@hide", CType(oVisibile, Main.FiltroVisibilità), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@isArchiviato", CType(oFiltroArchiviazione, Main.FiltroArchiviazione), ParameterDirection.Input, SqlDbType.Int)
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
            Catch ex As Exception
                Me.n_erroreDb = Errori_Db.DBError
            End Try
            Return dsTable
        End Function
        Public Sub AggiungiNotifica(ByVal PRSN_id As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_ForumThread_aggiungiNotifica"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@THRD_Id", Me.n_THRD_id, ParameterDirection.Input, DbType.Int32, True)
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
                .Command = "sp_ForumThread_rimuoviNotifica"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@THRD_Id", Me.n_THRD_id, ParameterDirection.Input, DbType.Int32, True)
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
        Public Function VerificaNotificaByPersona(ByVal idPRSN As Integer) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_ForumThread_VerificaNotificaByPersona"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@THRD_Id", Me.n_THRD_id, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Id", idPRSN, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LKPT_id", "", ParameterDirection.Output, DbType.Int32, True)
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


		Public Sub NotificaViaMail(ByVal LinkRimuoviNotifica As String, ByVal Percorso As String, ByVal oPersona As CL_persona.COL_Persona, ByVal oLocalizedMail As MailLocalized, Optional ByVal TPRF_id As Integer = -1)
			Dim oRequest As New COL_Request
			Dim oParam As New COL_Request.Parameter
			Dim dsTable As New DataSet
			Dim objAccesso As New COL_DataAccess

			With oRequest
				.Command = "sp_ForumThread_estraiMailxNotifica"
				.CommandType = CommandType.StoredProcedure
				oParam = objAccesso.GetParameter("@THRD_FRUM_Id", Me.n_THRD_FRUM_id, ParameterDirection.Input, DbType.Int32)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@PRSN_id", oPersona.Id, ParameterDirection.Input, DbType.Int32)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@TPRF_id", TPRF_id, ParameterDirection.Input, DbType.Int32)
				.Parameters.Add(oParam)
				.Role = COL_Request.UserRole.Admin
				.transactional = False
			End With

			Try
				dsTable = objAccesso.GetdataSet(oRequest)
			Catch ex As Exception
				Me.n_erroreDb = Errori_Db.DBError
			End Try

			Me.SendMail(dsTable, oLocalizedMail, oPersona, Percorso, LinkRimuoviNotifica)
		End Sub

#End Region

		Public Function HasThreadAssociatiForUtente(ByVal ForumId As Integer, ByRef totaleArchiviati As Integer, ByRef totaleAttivi As Integer, Optional ByVal oVisibilita As Main.FiltroVisibilità = Main.FiltroVisibilità.Visibile) As Boolean
			Dim oRequest As New COL_Request
			Dim oParam As New COL_Request.Parameter
			Dim objAccesso As New COL_DataAccess
			With oRequest
				.Command = "sp_ForumThread_HasThreadAssociatiForUtente"
				.CommandType = CommandType.StoredProcedure
				oParam = objAccesso.GetAdvancedParameter("@FRUM_ID", ForumId, ParameterDirection.Input, SqlDbType.Int)
				.Parameters.Add(oParam)

				oParam = objAccesso.GetAdvancedParameter("@totaleArchiviati", "", ParameterDirection.Output, SqlDbType.Int)
				.Parameters.Add(oParam)

				oParam = objAccesso.GetAdvancedParameter("@totaleAttivi", "", ParameterDirection.Output, SqlDbType.Int)
				.Parameters.Add(oParam)

				oParam = objAccesso.GetAdvancedParameter("@nascosto", CType(oVisibilita, Main.FiltroVisibilità), ParameterDirection.Input, SqlDbType.Int)
				.Parameters.Add(oParam)

				.Role = COL_Request.UserRole.Admin
				.transactional = False
			End With
			Try
				objAccesso.GetExecuteNotQuery(oRequest)

				totaleArchiviati = oRequest.GetValueFromParameter(2)
				totaleAttivi = oRequest.GetValueFromParameter(3)
				Me.n_erroreDb = Errori_Db.None
			Catch ex As Exception
				Me.n_erroreDb = Errori_Db.DBReadExist
			End Try
			Return True
		End Function
		Public Sub ModificaNotifica(ByVal PRSN_id As Integer)
			Dim oRequest As New COL_Request
			Dim oParam As New COL_Request.Parameter
			Dim objAccesso As New COL_DataAccess

			With oRequest
				.Command = "sp_ForumThread_modificaNotifica"
				.CommandType = CommandType.StoredProcedure

				oParam = objAccesso.GetParameter("@THRD_Id", Me.n_THRD_id, ParameterDirection.Input, DbType.Int32, True)
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
		Public Function ModificaArchiviazione() As Boolean
			Dim oRequest As New COL_Request
			Dim oParam As New COL_Request.Parameter
			Dim objAccesso As New COL_DataAccess

			With oRequest
				.Command = "sp_ForumThread_ModificaArchiviazione"
				.CommandType = CommandType.StoredProcedure

				oParam = objAccesso.GetParameter("@THRD_Id", Me.n_THRD_id, ParameterDirection.Input, DbType.Int32, True)
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

		Public Function EstraiSubject() As String
			Dim oRequest As New COL_Request
			Dim oParam As New COL_Request.Parameter
			Dim objAccesso As New COL_DataAccess
			Dim iResponse As String = ""

			With oRequest
				.Command = "sp_ForumThread_EstraiNome"
				.CommandType = CommandType.StoredProcedure

				oParam = objAccesso.GetAdvancedParameter("@THRD_Id", Me.n_THRD_id, ParameterDirection.Input, SqlDbType.Int)
				.Parameters.Add(oParam)

				oParam = objAccesso.GetAdvancedParameter("@Subject", "", ParameterDirection.Output, SqlDbType.VarChar, True, 100)
				.Parameters.Add(oParam)

				.Role = COL_Request.UserRole.Admin
				.transactional = False
			End With

			Try
				objAccesso.GetExecuteNotQuery(oRequest)
				Return oRequest.GetValueFromParameter(2)
			Catch ex As Exception
				Return iResponse
			End Try
		End Function
		Public Shared Function EstraiSubject(ByVal ThreadId As Integer) As String
			Dim oRequest As New COL_Request
			Dim oParam As New COL_Request.Parameter
			Dim objAccesso As New COL_DataAccess
			Dim iResponse As String = ""

			With oRequest
				.Command = "sp_ForumThread_EstraiNome"
				.CommandType = CommandType.StoredProcedure

				oParam = objAccesso.GetAdvancedParameter("@THRD_Id", ThreadId, ParameterDirection.Input, SqlDbType.Int)
				.Parameters.Add(oParam)

				oParam = objAccesso.GetAdvancedParameter("@Subject", "", ParameterDirection.Output, SqlDbType.VarChar, True, 100)
				.Parameters.Add(oParam)

				.Role = COL_Request.UserRole.Admin
				.transactional = False
			End With

			Try
				objAccesso.GetExecuteNotQuery(oRequest)
				Return oRequest.GetValueFromParameter(2)
			Catch ex As Exception
				Return iResponse
			End Try
		End Function
		Public Function isArchiviato() As Boolean
			Dim oRequest As New COL_Request
			Dim oParam As New COL_Request.Parameter
			Dim objAccesso As New COL_DataAccess
			Dim iResponse As String = ""

			With oRequest
				.Command = "sp_ForumThread_isArchiviato"
				.CommandType = CommandType.StoredProcedure

				oParam = objAccesso.GetAdvancedParameter("@THRD_Id", Me.n_THRD_id, ParameterDirection.Input, SqlDbType.Int)
				.Parameters.Add(oParam)

				oParam = objAccesso.GetAdvancedParameter("@isArchiviato", "", ParameterDirection.Output, SqlDbType.Int)
				.Parameters.Add(oParam)

				.Role = COL_Request.UserRole.Admin
				.transactional = False
			End With

			Try
				objAccesso.GetExecuteNotQuery(oRequest)
				If oRequest.GetValueFromParameter(2) = 0 Then
					Return False
				Else
					Return True
				End If
			Catch ex As Exception
				Return False
			End Try
		End Function
		Public Shared Function isVisibile(ByVal ThreadId As Integer) As Boolean
			Dim oRequest As New COL_Request
			Dim oParam As New COL_Request.Parameter
			Dim objAccesso As New COL_DataAccess
			Dim iResponse As String = ""

			With oRequest
				.Command = "sp_ForumThread_isVisibile"
				.CommandType = CommandType.StoredProcedure

				oParam = objAccesso.GetAdvancedParameter("@THRD_Id", ThreadId, ParameterDirection.Input, SqlDbType.Int)
				.Parameters.Add(oParam)

				oParam = objAccesso.GetAdvancedParameter("@isVisibile", "", ParameterDirection.Output, SqlDbType.Int)
				.Parameters.Add(oParam)

				.Role = COL_Request.UserRole.Admin
				.transactional = False
			End With

			Try
				objAccesso.GetExecuteNotQuery(oRequest)
				If oRequest.GetValueFromParameter(2) = 1 Then
					Return True
				Else
					Return False
				End If
			Catch ex As Exception
				Return False
			End Try
		End Function

		Public Shared Function Vota(ByVal TopicID As Integer, ByVal PRSN_ID As Integer, ByVal Voto As Integer) As Boolean
			Dim oRequest As New COL_Request
			Dim oParam As New COL_Request.Parameter
			Dim objAccesso As New COL_DataAccess

			With oRequest
				.Command = "sp_ForumThread_Vota"
				.CommandType = CommandType.StoredProcedure
				oParam = objAccesso.GetAdvancedParameter("@TopicID", TopicID, ParameterDirection.Input, SqlDbType.Int)
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

#Region "Invio mail asincrono"
        Private Sub SendMail(ByVal dsTable As DataSet, ByVal oLocalizedMail As MailLocalized, ByVal oPersona As CL_persona.COL_Persona, ByVal Percorso As String, ByVal LinkRimuoviNotifica As String)
            Dim totale, i As Integer
            Try 'iviaggio mail
                totale = dsTable.Tables(0).Rows.Count
                Dim oRow As DataRow

                If totale = 0 Then


                Else

                    For i = 0 To totale - 1
                        oRow = dsTable.Tables(0).Rows(i)

                        Dim oggetto As String
                        Dim body As String

                        oLocalizedMail = ManagerConfiguration.GetMailLocalized(ManagerLingua.GetByCodeOrDefault(oRow.Item("LNGU_codice")))

                        'oLocalizedMail.ReloadByLanguage(oRow.Item("LNGU_codice"))
                        Dim oMail As New COL_E_Mail(oLocalizedMail)
                        oggetto = oLocalizedMail.NotificationMessages(NotificationMessage.NotificationType.NewTopicComunita).Subject

                        oggetto = oggetto.Replace("#nomeForum#", oRow.Item("FRUM_Name"))
                        oMail.Oggetto = oggetto

                        body = oLocalizedMail.NotificationMessages(NotificationMessage.NotificationType.NewTopicComunita).Message

                        If Hour(Now) < 13 Then
                            body = body.Replace("#saluto#", oLocalizedMail.NotificationMessages(NotificationMessage.NotificationType.Hour0).Message)
                        ElseIf Hour(Now) >= 13 And Hour(Now) < 18 Then
                            body = body.Replace("#saluto#", oLocalizedMail.NotificationMessages(NotificationMessage.NotificationType.Hour12).Message)
                        Else
                            body = body.Replace("#saluto#", oLocalizedMail.NotificationMessages(NotificationMessage.NotificationType.Hour24).Message)
                        End If
                        body = body.Replace(",", "")
                        body = body.Replace("#anagrafica#", oRow.Item("Anagrafica"))

                        body = body.Replace("#nomeMittente#", oPersona.Cognome & " " & oPersona.Nome)
                        body = body.Replace("#nomeForum#", oRow.Item("FRUM_Name"))
                        body = body.Replace("#nomecomunita#", oRow.Item("CMNT_nome"))

                        Dim link As String
                        link = MainNotifica.LinkAccessoForum(oPersona.ID, oRow.Item("CMNT_id"), Percorso, n_THRD_FRUM_id, Me.n_THRD_id, 0)
                        If link <> "" Then
                            body = Replace(body, "#linkMessaggio#", link)
                        Else
                            body = Replace(body, "#linkMessaggio#", "")
                        End If
                        link = MainNotifica.LinkRimuoviNotifica(oPersona.ID, LinkRimuoviNotifica, n_THRD_FRUM_id, Me.n_THRD_id)
                        If link <> "" Then
                            body = Replace(body, "#linkRimuoviNotifiche#", link)
                        Else
                            body = Replace(body, "#linkRimuoviNotifiche#", "")
                        End If


                        body = body & vbCrLf & vbCrLf & vbCrLf & oLocalizedMail.SystemFirmaNotifica
                        body = body.Replace("<br>", vbCrLf)
                        oMail.Body = body
                        oMail.IndirizziTO.Add(New MailAddress(oRow.Item("PRSN_mail")))
                        oMail.Mittente = oLocalizedMail.SystemSender
                        oMail.InviaMail()
                        'System.Threading.Thread.Sleep(100) 'Lascio in pausa 0,1 secondi tra un invio e l'altro per non saturare la cpu

                        'nessun errore
                    Next
                End If
            Catch ex As Exception
                'invio fallito
            End Try
        End Sub
#End Region
    End Class
End Namespace