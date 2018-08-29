Imports COL_DataLayer
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Localizzazione
Imports COL_BusinessLogic_v2.Comol.Manager

Namespace Forum

    Public Class COL_Forum_posts

#Region "Private Property"
        Private Delegate Sub MailDelegate(ByVal dsTable As DataSet, ByVal oResourceConfig As ResourceManager, ByVal oPersona As COL_Persona, ByVal Percorso As String)
        Private n_POST_id As Integer
        Private n_POST_FRUM_id As Integer
        Private n_POST_ThreadID As Integer
        Private n_POST_ParentID As Integer
        Private n_POST_PostLevel As Integer
        Private n_POST_Approved As Main.PostApprovazione
        Private n_POST_PostDate As DateTime
        Private n_POST_PRSN_Id As Integer
        Private n_POST_Subject As String
        Private n_POST_Body As String
        Private n_POST_FRIM_id As Integer
        Private n_POST_Notifica As Integer
        Private n_erroreDb As Errori_Db
#End Region

#Region "Public Property"
        Public Property Id() As Integer
            Get
                Id = n_POST_id
            End Get
            Set(ByVal Value As Integer)
                n_POST_id = Value
            End Set
        End Property
        Public Property FRUM_id() As Integer
            Get
                FRUM_id = n_POST_FRUM_id
            End Get
            Set(ByVal Value As Integer)
                n_POST_FRUM_id = Value
            End Set
        End Property
        Public Property ThreadID() As Integer
            Get
                ThreadID = n_POST_ThreadID
            End Get
            Set(ByVal Value As Integer)
                n_POST_ThreadID = Value
            End Set
        End Property
        Public Property ParentID() As Integer
            Get
                ParentID = n_POST_ParentID
            End Get
            Set(ByVal Value As Integer)
                n_POST_ParentID = Value
            End Set
        End Property
        Public Property PostLevel() As Integer
            Get
                PostLevel = n_POST_PostLevel
            End Get
            Set(ByVal Value As Integer)
                n_POST_PostLevel = Value
            End Set
        End Property

        Public Property Approved() As Main.PostApprovazione
            Get
                Approved = n_POST_Approved
            End Get
            Set(ByVal Value As Main.PostApprovazione)
                n_POST_Approved = Value
            End Set
        End Property
        Public Property PRSN_Id() As Integer
            Get
                PRSN_Id = n_POST_PRSN_Id
            End Get
            Set(ByVal Value As Integer)
                n_POST_PRSN_Id = Value
            End Set
        End Property
        Public Property Subject() As String
            Get
                Subject = n_POST_Subject
            End Get
            Set(ByVal Value As String)
                n_POST_Subject = Value
            End Set
        End Property
        Public Property Body() As String
            Get
                Body = n_POST_Body
            End Get
            Set(ByVal Value As String)
                n_POST_Body = Value
            End Set
        End Property

        Public Property PostDate() As DateTime
            Get
                PostDate = n_POST_PostDate
            End Get
            Set(ByVal Value As DateTime)
                n_POST_PostDate = Value
            End Set
        End Property
        Public Property PostImageID() As Integer
            Get
                PostImageID = n_POST_FRIM_id
            End Get
            Set(ByVal Value As Integer)
                n_POST_FRIM_id = Value
            End Set
        End Property
        Public Property NotificaMail() As Boolean
            Get
                NotificaMail = (n_POST_Notifica = 1)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    n_POST_Notifica = 1
                Else
                    n_POST_Notifica = 0
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
			Me.n_POST_PostDate = Now()
            Me.n_POST_FRIM_id = 1
            Me.n_POST_Notifica = 0
            Me.n_erroreDb = Errori_Db.None
        End Sub
#End Region

        Public Sub Estrai()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_ForumPost_Estrai"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@POST_Id", Me.n_POST_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@POST_FRUM_Id", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@POST_threadId", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@POST_ParentId", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@POST_PostLevel", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@POST_PostSubject", "", ParameterDirection.Output, SqlDbType.VarChar, True, 100)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@POST_PostDate", "", ParameterDirection.Output, SqlDbType.DateTime)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@POST_Post_Approved", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@POST_PRSN_Id", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@POST_FRIM_id", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@POST_Notifica", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)

                With Me
                    .n_POST_FRUM_id = oRequest.GetValueFromParameter(2)
                    .n_POST_ThreadID = oRequest.GetValueFromParameter(3)
                    .n_POST_ParentID = oRequest.GetValueFromParameter(4)
                    .n_POST_PostLevel = oRequest.GetValueFromParameter(5)
                    .n_POST_Subject = oRequest.GetValueFromParameter(6)
                    If IsDate(oRequest.GetValueFromParameter(7)) Then
                        .n_POST_PostDate = oRequest.GetValueFromParameter(7)
                    End If
                    .n_POST_Approved = oRequest.GetValueFromParameter(8)
                    .n_POST_PRSN_Id = oRequest.GetValueFromParameter(9)
                    .n_POST_FRIM_id = oRequest.GetValueFromParameter(10)
                    .n_POST_Notifica = oRequest.GetValueFromParameter(11)

                End With

            Catch ex As Exception
                Me.n_erroreDb = Errori_Db.DBReadExist
            End Try
        End Sub

        Public Sub Aggiungi(ByVal oRuoloForum As Main.RuoloForumStandard)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess


            With oRequest
                .Command = "sp_ForumPost_Aggiungi"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@POST_ID", 0, ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@POST_FRUM_Id", Me.n_POST_FRUM_id, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@POST_threadId", Me.n_POST_ThreadID, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@POST_ParentId", Me.n_POST_ParentID, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@POST_PostLevel", Me.n_POST_PostLevel, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@POST_PostSubject", Me.n_POST_Subject, ParameterDirection.Input, DbType.String, True, 100)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@POST_PostDate", DateToString(PostDate, True), ParameterDirection.Input, DbType.String, True, 30)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@POST_Post_Approved", Me.n_POST_Approved, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@POST_PRSN_Id", Me.n_POST_PRSN_Id, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@POST_Body", Me.n_POST_Body, ParameterDirection.Input, SqlDbType.VarChar, True)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@POST_IdRuolo", CType(oRuoloForum, Main.RuoloForumStandard), ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@POST_FRIM_id", Me.n_POST_FRIM_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@POST_Notifica", Me.n_POST_Notifica, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

               
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.Id = oRequest.GetValueFromParameter(1)
            Catch ex As Exception
                Me.n_erroreDb = Errori_Db.DBInsert
            End Try
        End Sub
        Public Sub Modifica()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_ForumPost_Modifica"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@POST_ID", Me.n_POST_id, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@POST_FRUM_Id", Me.n_POST_FRUM_id, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@POST_threadId", Me.n_POST_ThreadID, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@POST_ParentId", Me.n_POST_ParentID, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@POST_PostLevel", Me.n_POST_PostLevel, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@POST_PostSubject", Me.n_POST_Subject, ParameterDirection.Input, DbType.String, True, 100)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@POST_Post_Approved", Me.n_POST_Approved, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@POST_PRSN_Id", Me.n_POST_PRSN_Id, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@POST_Body", Me.n_POST_Body, ParameterDirection.Input, SqlDbType.VarChar, True)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@POST_FRIM_id", Me.n_POST_FRIM_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@POST_Notifica", Me.n_POST_Notifica, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)

            Catch ex As Exception
                Me.n_erroreDb = Errori_Db.DBInsert
            End Try
        End Sub

        Public Function EstraiEsteso(ByVal fromTreeview As Boolean) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_ForumPost_EstraiEsteso"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@POST_id", Me.n_POST_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                If fromTreeview Then
                    oParam = objAccesso.GetParameter("@forTreeview", 1, ParameterDirection.Input, DbType.Int32)
                Else
                    oParam = objAccesso.GetParameter("@forTreeview", 0, ParameterDirection.Input, DbType.Int32)
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
        Public Function ElencaByThread(ByVal LinguaID As Integer, ByVal ForumID As Integer, ByVal Thread_Id As Integer, ByVal oTipoPost As Main.FiltroPostApprovazione, ByVal forTreeview As Boolean, ByVal PRSN_ID As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_ForumPost_ElencaByThread"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@POST_FRUM_Id", ForumID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@POST_ThreadId", Thread_Id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@FiltroApprovazione", CType(oTipoPost, Main.FiltroPostApprovazione), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                If forTreeview Then
                    oParam = objAccesso.GetParameter("@forTreeview", 1, ParameterDirection.Input, DbType.Int32)
                Else
                    oParam = objAccesso.GetParameter("@forTreeview", 0, ParameterDirection.Input, DbType.Int32)
                End If
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@POST_PRSN_ID", PRSN_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@LinguaID", LinguaID, ParameterDirection.Input, DbType.Int32)
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
        Public Function Elimina() As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_ForumPost_Elimina"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@POST_Id", Me.n_POST_id, ParameterDirection.Input, DbType.Int32, True)
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

#Region "Metodi"
        Public Function ElencaByForum(ByVal IdComunita As Integer, ByVal IdForum As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Forum_post_Elenca"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@POST_FRUM_Id", IdForum, ParameterDirection.Input, DbType.String)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            'Try
            dsTable = objAccesso.GetdataSet(oRequest)
            ' Catch ex As Exception
            '    Me.n_erroreDb = Errori_Db.DBError
            'End Try
            Return dsTable
        End Function
        Public Function ElencaByThreadTutto(ByVal IdComunita As Integer, ByVal IdForum As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Forum_post_thread_Elenca_tutto"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@POST_FRUM_Id", IdForum, ParameterDirection.Input, DbType.Int32)
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

        Public Sub EstraiTestoXQuota()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            ' Dim dsTable As DataSet
            Dim objAccesso As New COL_DataAccess


            With oRequest
                .Command = "sp_Forum_post_EstraiTestoXQuota"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@POST_Id", Me.Id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@POST_body", "", ParameterDirection.Output, DbType.String, , 1000)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_POST_Body = oRequest.GetValueFromParameter(2)
                ' Catch ex As Exception
                '    Me.n_erroreDb = Errori_Db.DBError
                'End Try
            Catch ex As Exception

            End Try
        End Sub
        Public Sub InserisciRisposta(ByVal IdThread As Integer, ByVal IdForum As Integer, ByVal ParentId As Integer, ByVal PostLevel As Integer, ByVal IdComunita As Integer, ByVal User As Integer, ByVal textbody As String, ByVal subject As String, ByVal IdRuolo As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim postdate As DateTime = DateTime.Now
            Dim Post_Approved As Integer = 1

            With oRequest
                .Command = "sp_Forum_post_aggiungiRisposta"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@POST_FRUM_Id", IdForum, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@POST_threadId", IdThread, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@POST_ParentId", ParentId, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@POST_PostLevel", PostLevel, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@POST_PostSubject", subject, ParameterDirection.Input, DbType.String, True, 50)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@POST_PostDate", DateToString(postdate, True), ParameterDirection.Input, DbType.String, True, 30)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@POST_Post_Approved", Post_Approved, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@POST_PRSN_Id", User, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@POST_Body", textbody, ParameterDirection.Input, DbType.String, True)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@POST_IdRuolo", IdRuolo, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch ex As Exception
                '   Me.n_erroreDb = Errori_Db.DBInsert
            End Try
        End Sub

        Public Function Censura(ByVal POST_ID As Integer) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_ForumPost_CambiaVisibilita"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@POST_Id", POST_ID, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@POST_Approved", CType(Main.PostApprovazione.Censurato, Main.PostApprovazione), ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch ex As Exception
                '   Me.n_erroreDb = Errori_Db.DBInsert
            End Try


        End Function
        Public Function Riabilita(ByVal POST_ID As Integer) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_ForumPost_CambiaVisibilita"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@POST_Id", POST_ID, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@POST_Approved", CType(Main.PostApprovazione.Approvato, Main.PostApprovazione), ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            ' Try
            objAccesso.GetExecuteNotQuery(oRequest)
            'Catch
            '   Me.n_erroreDb = Errori_Db.DBInsert
            '  End Try


        End Function

		Public Sub NotificaViaMail(ByVal LinkRimuoviNotifica As String, ByVal Percorso As String, ByVal oPersona As COL_Persona, ByVal oLocalizedMail As MailLocalized, Optional ByVal TPRF_id As Integer = -1)
			Dim oRequest As New COL_Request
			Dim oParam As New COL_Request.Parameter
			Dim dsTable As New DataSet
			Dim objAccesso As New COL_DataAccess

			With oRequest
				.Command = "sp_ForumPost_estraiMailxNotifica"
				.CommandType = CommandType.StoredProcedure
				oParam = objAccesso.GetParameter("@POST_THRD_Id", Me.n_POST_ThreadID, ParameterDirection.Input, DbType.Int32)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@PRSN_id", oPersona.Id, ParameterDirection.Input, DbType.Int32)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@TPRF_id", TPRF_id, ParameterDirection.Input, DbType.Int32)
				.Parameters.Add(oParam)
				.Role = COL_Request.UserRole.Admin
				.transactional = False
			End With

			'Dim oDelegate As New MailDelegate(AddressOf SendMail)
			''invoco il metodo
			'oDelegate.BeginInvoke(oMail, oResourceConfig, New AsyncCallback(AddressOf Me.EndSend), oDelegate)


			Try
				'Me.SendMail(objAccesso.GetdataSet(oRequest), oResourceConfig, oPersona)
				dsTable = objAccesso.GetdataSet(oRequest)
			Catch ex As Exception
				Me.n_erroreDb = Errori_Db.DBError
			End Try
			Me.SendMail(dsTable, oLocalizedMail, oPersona, Percorso, LinkRimuoviNotifica)
			'Try
			'    Dim oDelegate As New MailDelegate(AddressOf SendMail)
			'    oDelegate.BeginInvoke(dsTable, oResourceConfig, oPersona, Percorso, New AsyncCallback(AddressOf Me.EndSend), oDelegate)
			'Catch ex As Exception
			'End Try
		End Sub
#End Region

		Public Function GetThread(ByVal PostId As Integer) As COL_Forum_threads
			Dim oThread As New COL_Forum_threads
			Dim oRequest As New COL_Request
			Dim oParam As New COL_Request.Parameter
			Dim iResponse As Integer
			Dim objAccesso As New COL_DataAccess

			With oRequest
				.Command = "sp_ForumPost_GetThread"
				.CommandType = CommandType.StoredProcedure
				oParam = objAccesso.GetAdvancedParameter("@POST_id", Me.n_POST_id, ParameterDirection.Input, SqlDbType.Int)
				.Parameters.Add(oParam)

				oParam = objAccesso.GetAdvancedParameter("@THRD_Id", "", ParameterDirection.Output, SqlDbType.Int)
				.Parameters.Add(oParam)

				oParam = objAccesso.GetAdvancedParameter("@THRD_NReply", "", ParameterDirection.Output, SqlDbType.Int)
				.Parameters.Add(oParam)

				oParam = objAccesso.GetAdvancedParameter("@THRD_NView", "", ParameterDirection.Output, SqlDbType.Int)
				.Parameters.Add(oParam)

				oParam = objAccesso.GetAdvancedParameter("@THRD_PRSN_Id", "", ParameterDirection.Output, SqlDbType.Int)
				.Parameters.Add(oParam)

				oParam = objAccesso.GetAdvancedParameter("@THRD_FRUM_Id", "", ParameterDirection.Output, SqlDbType.Int)
				.Parameters.Add(oParam)

				oParam = objAccesso.GetAdvancedParameter("@THRD_Notifica", "", ParameterDirection.Output, SqlDbType.Int)
				.Parameters.Add(oParam)

				oParam = objAccesso.GetAdvancedParameter("@THRD_FRIM_Id", "", ParameterDirection.Output, SqlDbType.Int)
				.Parameters.Add(oParam)

				oParam = objAccesso.GetAdvancedParameter("@THRD_Subject", "", ParameterDirection.Output, SqlDbType.VarChar, , 100)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetAdvancedParameter("@THRD_Hide", "", ParameterDirection.Output, SqlDbType.Int)
				.Parameters.Add(oParam)

				.Role = COL_Request.UserRole.Admin
				.transactional = False
			End With

			Try
				iResponse = objAccesso.GetExecuteNotQuery(oRequest)
				oThread.Id = oRequest.GetValueFromParameter(2)
				oThread.NReply = oRequest.GetValueFromParameter(3)
				oThread.NView = oRequest.GetValueFromParameter(4)
				oThread.PRSN_Id = oRequest.GetValueFromParameter(5)
				oThread.ThreadForum = oRequest.GetValueFromParameter(6)
				oThread.NotificaMail = oRequest.GetValueFromParameter(7)
				oThread.ThreadImage = oRequest.GetValueFromParameter(8)
				oThread.Subject = oRequest.GetValueFromParameter(9)
				oThread.Hide = oRequest.GetValueFromParameter(10)
			Catch ex As Exception
				oThread.Id = -1
			End Try
			Return oThread

		End Function

		Public Shared Function EstraiSubject(ByVal PostId As Integer) As String
			Dim oRequest As New COL_Request
			Dim oParam As New COL_Request.Parameter
			Dim iResponse As String = ""
			Dim objAccesso As New COL_DataAccess

			With oRequest
				.Command = "sp_ForumPost_EstraiSubject"
				.CommandType = CommandType.StoredProcedure
				oParam = objAccesso.GetAdvancedParameter("@POST_Id", PostId, ParameterDirection.Input, SqlDbType.Int)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetAdvancedParameter("@subject", "", ParameterDirection.Output, SqlDbType.VarChar, , 100)
				.Parameters.Add(oParam)

				.Role = COL_Request.UserRole.Admin
				.transactional = False
			End With

			Try
				objAccesso.GetExecuteNotQuery(oRequest)
				iResponse = oRequest.GetValueFromParameter(2)
				If iResponse = "" Then
					iResponse = "--"
				End If
			Catch ex As Exception
				iResponse = "--"
			End Try
			Return iResponse
		End Function

		Public Shared Function Vota(ByVal PostId As Integer, ByVal PRSN_ID As Integer, ByVal Voto As Integer) As Boolean
			Dim oRequest As New COL_Request
			Dim oParam As New COL_Request.Parameter
			'Dim iResponse As String
			Dim objAccesso As New COL_DataAccess

			With oRequest
				.Command = "sp_ForumPost_Vota"
				.CommandType = CommandType.StoredProcedure
				oParam = objAccesso.GetAdvancedParameter("@POST_Id", PostId, ParameterDirection.Input, SqlDbType.Int)
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
		Private Sub SendMail(ByVal dsTable As DataSet, ByVal oLocalizedMail As MailLocalized, ByVal oPersona As COL_Persona, ByVal Percorso As String, ByVal LinkRimuoviNotifica As String)
			Dim totale, i As Integer
			Try	'iviaggio mail
				totale = dsTable.Tables(0).Rows.Count
				Dim oRow As DataRow


				If totale = 0 Then


				Else

					For i = 0 To totale - 1
						oRow = dsTable.Tables(0).Rows(i)

						oLocalizedMail = ManagerConfiguration.GetMailLocalized(ManagerLingua.GetByCodeOrDefault(oRow.Item("LNGU_codice")))
						'oLocalizedMail.ReloadByLanguage(oRow.Item("LNGU_codice"))


						Dim oMail As New COL_E_Mail(oLocalizedMail)
						Dim arrListaFiles As New ArrayList


						Dim body, oggetto As String
						body = oLocalizedMail.NotificationMessages(NotificationMessage.NotificationType.NewPostComunita).Message

						oggetto = oLocalizedMail.NotificationMessages(NotificationMessage.NotificationType.NewPostComunita).Subject
						oggetto = oggetto.Replace("#nomeTopic#", oRow.Item("THRD_Subject"))
						oMail.Oggetto = oggetto

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
						body = body.Replace("#nomeTopic#", oRow.Item("THRD_Subject"))

						Dim link As String = ""

						link = MainNotifica.LinkAccessoForum(oPersona.Id, oRow.Item("CMNT_id"), Percorso, oRow.Item("FRUM_ID"), Me.n_POST_ThreadID, Me.n_POST_id)
						If link <> "" Then
							body = Replace(body, "#linkMessaggio#", link)
						Else
							body = Replace(body, "#linkMessaggio#", "")
						End If
						link = MainNotifica.LinkRimuoviNotifica(oPersona.Id, LinkRimuoviNotifica, oRow.Item("FRUM_ID"), Me.n_POST_ThreadID, Me.n_POST_id)
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
						oMail.Attachment = arrListaFiles

						oMail.InviaMail()
						' System.Threading.Thread.Sleep(100) 'Lascio in pausa 0,1 secondi tra un invio e l'altro per non saturare la cpu
						'nessun errore
					Next
				End If
			Catch ex As Exception
				'invio fallito
			End Try
		End Sub


		Private Sub EndSend(ByVal AsResult As IAsyncResult)
			Dim d As MailDelegate = CType(AsResult.AsyncState, MailDelegate)
			d.EndInvoke(AsResult)
		End Sub
#End Region
    End Class
End Namespace