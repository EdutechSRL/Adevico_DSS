Imports COL_DataLayer

Namespace CL_persona
    Namespace CL_Esterno
        Public Class COL_Esterno : Inherits COL_Persona
            Private n_ESTR_Mansione As String
            Private n_ESTR_PRSN As Integer
            ' Private n_Errore As Errori_Db

#Region "Proprietà"
            Public Property Persona() As Integer
                Get
                    Persona = n_ESTR_PRSN
                End Get
                Set(ByVal Value As Integer)
                    n_ESTR_PRSN = Value
                End Set
            End Property
            Public Property Mansione() As String
                Get
                    Mansione = n_ESTR_Mansione
                End Get
                Set(ByVal Value As String)
                    n_ESTR_Mansione = Value
                End Set
            End Property
            Public Shadows ReadOnly Property Errore() As Errori_Db
                Get
                    Errore = n_Errore
                End Get
            End Property
#End Region

#Region "Metodi New"
            Public Sub New()
                Me.n_Errore = Errori_Db.None
            End Sub
#End Region


#Region "Metodi Standard "
            'qui elenca solo l'id della persona e la mansione!!!!
            'e se voglio il nome o altro?
            Public Shadows Function Elenca() As DataSet
                Dim oRequest As New COL_Request
                Dim oParam As New COL_Request.Parameter
                Dim dsTable As New DataSet
                Dim objAccesso As New COL_DataAccess

                With oRequest
                    .Command = "sp_Esterno_Elenca"
                    .CommandType = CommandType.StoredProcedure
                    .Role = COL_Request.UserRole.Admin
                    .transactional = False
                End With
                Try
                    dsTable = objAccesso.GetdataSet(oRequest)
                Catch ex As Exception
                    'gestione errore shared
                End Try
                Return dsTable
            End Function
            Public Shadows Sub Modifica()
                Dim oRequest As New COL_Request
                Dim oParam As New COL_Request.Parameter
                Dim dsTable As Integer
                Dim objAccesso As New COL_DataAccess

                Try
                    Me.ModificaPersona()
                    If (Me.Id > 0) Then

                        With oRequest
                            .Command = "sp_Esterno_Modifica"
                            .CommandType = CommandType.StoredProcedure
                            oParam = objAccesso.GetParameter("@ESTR_PRSN", Me.Id, , DbType.Int32)
                            .Parameters.Add(oParam)
                            oParam = objAccesso.GetParameter("@ESTR_mansione", n_ESTR_Mansione.Trim)
                            .Parameters.Add(oParam)
                            .Role = COL_Request.UserRole.Admin
                            .transactional = False
                        End With
                        Try
                            dsTable = objAccesso.GetExecuteNotQuery(oRequest)
                            Me.n_ESTR_PRSN = Me.Id
                        Catch
                            ' Codice per il rollback

                            Me.n_ESTR_PRSN = -1
                            dsTable = -1
                        End Try
                    Else
                        Me.n_ESTR_PRSN = -1      'da rivedere seriamente 'OK
                        dsTable = -1
                    End If
                Catch
                    Me.n_ESTR_PRSN = -1
                    dsTable = -1
                End Try
            End Sub
            Public Shadows Function Aggiungi() As Integer
                Dim oRequest As New COL_Request
                Dim oParam As New COL_Request.Parameter
                Dim dsTable As Integer
                Dim objAccesso As New COL_DataAccess
                Try
                    Me.AggiungiPersona()
                    If (Me.Id > 0) Then

                        With oRequest
                            .Command = "sp_Esterno_Aggiungi"
                            .CommandType = CommandType.StoredProcedure
                            oParam = objAccesso.GetParameter("@ESTR_PRSN_Id", Me.Id, , DbType.Int32)
                            .Parameters.Add(oParam)
                            oParam = objAccesso.GetParameter("@ESTR_mansione", n_ESTR_Mansione.Trim)
                            .Parameters.Add(oParam)
                            .Role = COL_Request.UserRole.Admin
                            .transactional = False
                        End With
                        Try
                            dsTable = objAccesso.GetExecuteNotQuery(oRequest)
                            Me.n_ESTR_PRSN = Me.Id
                        Catch
                            ' Codice per il rollback

                            Me.n_ESTR_PRSN = -1
                            dsTable = -1
                        End Try
                    Else
                        Me.n_ESTR_PRSN = -1
                        dsTable = -1
                    End If
                Catch
                    Me.n_ESTR_PRSN = -1
                    dsTable = -1
                End Try
                Return dsTable
            End Function
            Public Shadows Sub Estrai(ByVal LinguaID As Integer)
                'carica i campi del db nell'oggetto in base all'id della persona
                Dim oRequest As New COL_Request
                Dim oParam As New COL_Request.Parameter
                Dim objAccesso As New COL_DataAccess
                With oRequest
                    .Command = "sp_Esterno_Estrai"
                    .CommandType = CommandType.StoredProcedure
                    oParam = objAccesso.GetParameter("@PRSN_id", Me.n_PRSN_id, ParameterDirection.Input, DbType.Int32)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_Nome", "", ParameterDirection.Output, DbType.String, , 50)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_Cognome", "", ParameterDirection.Output, DbType.String, , 50)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_indirizzo", "", ParameterDirection.Output, DbType.String, , 50)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_cap", "", ParameterDirection.Output, DbType.String, , 10)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_citta", "", ParameterDirection.Output, DbType.String, , 50)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_PRVN_Id", "", ParameterDirection.Output, DbType.Int32)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_Telefono1", "", ParameterDirection.Output, DbType.String, , 25)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_Telefono2", "", ParameterDirection.Output, DbType.String, , 25)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_Cellulare", "", ParameterDirection.Output, DbType.String, , 25)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_Fax", "", ParameterDirection.Output, DbType.String, , 25)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_Mail", "", ParameterDirection.Output, DbType.String, , 255)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_HomePage", "", ParameterDirection.Output, DbType.String, , 255)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_FotoPath", "", ParameterDirection.Output, DbType.String, , 255)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@ESTR_mansione", "", ParameterDirection.Output, DbType.String, , 100)
                    .Parameters.Add(oParam)

                    oParam = objAccesso.GetParameter("@PRSN_login", "", ParameterDirection.Output, DbType.String, , 50)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_luogoNascita", "", ParameterDirection.Output, DbType.String, , 30)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_dataNascita", "", ParameterDirection.Output, DbType.DateTime)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_sesso", "", ParameterDirection.Output, DbType.Byte)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_codFiscale", "", ParameterDirection.Output, DbType.String, , 16)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_STTO_ID", "", ParameterDirection.Output, DbType.Int32)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_LNGU_ID", "", ParameterDirection.Output, DbType.Int32)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_RicezioneSMS", "", ParameterDirection.Output, DbType.Byte)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_AUTN_ID", "", ParameterDirection.Output, DbType.Int32)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_AUTN_RemoteUniqueID", "", ParameterDirection.Output, DbType.String, , 255)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_invisible", "", ParameterDirection.Output, DbType.Int32)
                    .Parameters.Add(oParam)

                    oParam = objAccesso.GetParameter("@PRSN_TPPR_id", "", ParameterDirection.Output, DbType.Int32)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@TPPR_descrizione", "", ParameterDirection.Output, DbType.String, , 100)
                    .Parameters.Add(oParam)

                    oParam = objAccesso.GetParameter("@PRSN_mostraMail", "", ParameterDirection.Output, DbType.Int32)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetAdvancedParameter("@PRSN_Ricevimento", "", ParameterDirection.Output, SqlDbType.VarChar, True, 500)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetAdvancedParameter("@PRSN_MailSecondaria", "", ParameterDirection.Output, SqlDbType.VarChar, True, 255)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@LinguaID", LinguaID, ParameterDirection.Input, DbType.Int32)
                    .Parameters.Add(oParam)

                    .Role = COL_Request.UserRole.Admin
                    .transactional = False
                End With

                Try
                    objAccesso.GetExecuteNotQuery(oRequest)
                    Me.n_PRSN_nome = oRequest.GetValueFromParameter(2)
                    Me.n_PRSN_cognome = oRequest.GetValueFromParameter(3)
                    Me.n_PRSN_indirizzo = oRequest.GetValueFromParameter(4)
                    Me.n_PRSN_cap = oRequest.GetValueFromParameter(5)
                    Me.n_PRSN_citta = oRequest.GetValueFromParameter(6)
                    Me.n_PRSN_PRVN.Id = oRequest.GetValueFromParameter(7)
                    Me.n_PRSN_telefono1 = oRequest.GetValueFromParameter(8)
                    Me.n_PRSN_telefono2 = oRequest.GetValueFromParameter(9)
                    Me.n_PRSN_cellulare = oRequest.GetValueFromParameter(10)
                    Me.n_PRSN_fax = oRequest.GetValueFromParameter(11)
                    Me.n_PRSN_mail = oRequest.GetValueFromParameter(12)
                    Me.n_PRSN_homePage = oRequest.GetValueFromParameter(13)
                    Me.n_PRSN_fotoPath = oRequest.GetValueFromParameter(14)
                    Me.n_ESTR_Mansione = oRequest.GetValueFromParameter(15)

                    Me.n_PRSN_login = oRequest.GetValueFromParameter(16)
                    Me.n_PRSN_luogoNascita = oRequest.GetValueFromParameter(17)
                    Me.n_PRSN_dataNascita = oRequest.GetValueFromParameter(18)
                    Me.n_PRSN_sesso = oRequest.GetValueFromParameter(19)
                    Me.n_PRSN_codFiscale = oRequest.GetValueFromParameter(20)
                    Me.n_PRSN_STTO.Id = oRequest.GetValueFromParameter(21)
                    Me.n_PRSN_LNGU.Id = oRequest.GetValueFromParameter(22)
                    Me.n_PRSN_RicezioneSMS = oRequest.GetValueFromParameter(23)
                    Me.n_PRSN_AUTN_ID = oRequest.GetValueFromParameter(24)
                    Me.n_PRSN_AUTN_RemoteUniqueID = oRequest.GetValueFromParameter(25)
                    Me.n_PRSN_invisibile = oRequest.GetValueFromParameter(26)

                    Me.n_PRSN_TPPR.id = oRequest.GetValueFromParameter(27)
                    Me.n_PRSN_TPPR.Descrizione = oRequest.GetValueFromParameter(28)
                    Me.n_PRSN_mostraMail = oRequest.GetValueFromParameter(29)
                    Me.n_PRSN_Ricevimento = oRequest.GetValueFromParameter(30)
                    Me.n_PRSN_MailSecondaria = oRequest.GetValueFromParameter(31)
                Catch ax As Exception
                    Me.n_PRSN_id = -1
                    Me.n_Errore = Errori_Db.DBReadExist
                End Try
            End Sub
            Public Shadows Sub EstraiTutto(ByVal LinguaID As Integer)
                'carica i campi del db nell'oggetto in base all'id della persona
                Dim oRequest As New COL_Request
                Dim oParam As New COL_Request.Parameter
                Dim objAccesso As New COL_DataAccess
                With oRequest
                    .Command = "sp_Esterno_EstraiTutto"
                    .CommandType = CommandType.StoredProcedure
                    oParam = objAccesso.GetParameter("@PRSN_id", n_PRSN_id, ParameterDirection.Input, DbType.Int32)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_Login", "", ParameterDirection.Output, DbType.String, , 50)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_pwd", "", ParameterDirection.Output, DbType.String, , 255)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_Nome", "", ParameterDirection.Output, DbType.String, False, 50)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_Cognome", "", ParameterDirection.Output, DbType.String, False, 50)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_dataNascita", "", ParameterDirection.Output, DbType.DateTime)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_luogoNascita", "", ParameterDirection.Output, DbType.String, False, 30)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_sesso", "", ParameterDirection.Output, DbType.Byte)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_codFiscale", "", ParameterDirection.Output, DbType.String, False, 16)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_indirizzo", "", ParameterDirection.Output, DbType.String, False, 50)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_cap", "", ParameterDirection.Output, DbType.String, False, 10)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_citta", "", ParameterDirection.Output, DbType.String, False, 50)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_PRVN_Id", "", ParameterDirection.Output, DbType.String, False, 50)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRVN_nome", "", ParameterDirection.Output, DbType.String, False, 50)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_STTO_id", "", ParameterDirection.Output, DbType.Int32)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@STTO_descrizione", "", ParameterDirection.Output, DbType.String, False, 150)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_Telefono1", "", ParameterDirection.Output, DbType.String, False, 25)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_Telefono2", "", ParameterDirection.Output, DbType.String, False, 25)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_Cellulare", "", ParameterDirection.Output, DbType.String, False, 25)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_Fax", "", ParameterDirection.Output, DbType.String, False, 25)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_Mail", "", ParameterDirection.Output, DbType.String, False, 50)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_HomePage", "", ParameterDirection.Output, DbType.String, False, 50)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_FotoPath", "", ParameterDirection.Output, DbType.String, False, 255)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_invisible", "", ParameterDirection.Output, DbType.Int32)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_TPPR_id", "", ParameterDirection.Output, DbType.Int32)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_LNGU_id", "", ParameterDirection.Output, DbType.Int32)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@LNGU_nome", "", ParameterDirection.Output, DbType.String, , 100)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_RicezioneSMS", "", ParameterDirection.Output, DbType.Byte)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@ESTR_mansione", "", ParameterDirection.Output, DbType.String, , 100)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_ISTT_id", "", ParameterDirection.Output, DbType.Int32)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_MaxAree", "", ParameterDirection.Output, DbType.Int32)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_AUTN_ID", "", ParameterDirection.Output, DbType.Int32)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_AUTN_RemoteUniqueID", "", ParameterDirection.Output, DbType.String, , 255)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_ORGNDefault_id", "", ParameterDirection.Output, DbType.Int32)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_ORGNDefault_ragioneSociale", "", ParameterDirection.Output, DbType.String, , 100)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@PRSN_dataInserimento", "", ParameterDirection.Output, DbType.DateTime)
                    .Parameters.Add(oParam)

                    oParam = objAccesso.GetParameter("@PRSN_mostraMail", "", ParameterDirection.Output, DbType.Int32)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@TPPR_descrizione", "", ParameterDirection.Output, DbType.String, , 100)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetAdvancedParameter("@PRSN_Ricevimento", "", ParameterDirection.Output, SqlDbType.VarChar, True, 500)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetAdvancedParameter("@PRSN_MailSecondaria", "", ParameterDirection.Output, SqlDbType.VarChar, True, 255)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@LinguaID", LinguaID, ParameterDirection.Input, DbType.Int32)
                    .Parameters.Add(oParam)

                    .Role = COL_Request.UserRole.Admin
                    .transactional = False
                End With

                Try
                    objAccesso.GetExecuteNotQuery(oRequest)
                    Me.n_PRSN_login = oRequest.GetValueFromParameter(2)
                    Me.n_PRSN_pwd = oRequest.GetValueFromParameter(3)
                    Me.n_PRSN_nome = oRequest.GetValueFromParameter(4)
                    Me.n_PRSN_cognome = oRequest.GetValueFromParameter(5)
                    Me.n_PRSN_dataNascita = oRequest.GetValueFromParameter(6)
                    Me.n_PRSN_luogoNascita = oRequest.GetValueFromParameter(7)
                    Me.n_PRSN_sesso = oRequest.GetValueFromParameter(8)
                    Me.n_PRSN_codFiscale = oRequest.GetValueFromParameter(9)
                    Me.n_PRSN_indirizzo = oRequest.GetValueFromParameter(10)
                    Me.n_PRSN_cap = oRequest.GetValueFromParameter(11)
                    Me.n_PRSN_citta = oRequest.GetValueFromParameter(12)
                    Me.n_PRSN_PRVN.Id = oRequest.GetValueFromParameter(13)
                    Me.n_PRSN_PRVN.Nome = oRequest.GetValueFromParameter(14)
                    Me.n_PRSN_STTO.Id = oRequest.GetValueFromParameter(15)
                    Me.n_PRSN_STTO.Descrizione = oRequest.GetValueFromParameter(16)
                    Me.n_PRSN_telefono1 = oRequest.GetValueFromParameter(17)
                    Me.n_PRSN_telefono2 = oRequest.GetValueFromParameter(18)
                    Me.n_PRSN_cellulare = oRequest.GetValueFromParameter(19)
                    Me.n_PRSN_fax = oRequest.GetValueFromParameter(20)
                    Me.n_PRSN_mail = oRequest.GetValueFromParameter(21)
                    Me.n_PRSN_homePage = oRequest.GetValueFromParameter(22)
                    Me.n_PRSN_fotoPath = oRequest.GetValueFromParameter(23)
                    Me.n_PRSN_invisibile = oRequest.GetValueFromParameter(24)
					Me.n_PRSN_TPPR.ID = oRequest.GetValueFromParameter(25)
					Me.n_PRSN_LNGU = Lingua.CreateByName(oRequest.GetValueFromParameter(26), oRequest.GetValueFromParameter(27))
					'Me.n_PRSN_LNGU.Id = oRequest.GetValueFromParameter(26)
					'Me.n_PRSN_LNGU.Nome = oRequest.GetValueFromParameter(27)
                    Me.n_PRSN_RicezioneSMS = oRequest.GetValueFromParameter(28)
                    Me.n_ESTR_Mansione = oRequest.GetValueFromParameter(29)
                    Me.n_PRSN_ISTT.Id = oRequest.GetValueFromParameter(30)
                    Me.n_PRSN_maxAree = oRequest.GetValueFromParameter(31)
                    Me.n_PRSN_AUTN_ID = oRequest.GetValueFromParameter(32)
                    Me.n_PRSN_AUTN_RemoteUniqueID = oRequest.GetValueFromParameter(33)
                    Me.n_PRSN_ORGNDefault_id = oRequest.GetValueFromParameter(34)
                    Me.n_PRSN_ORGNDefault_ragioneSociale = oRequest.GetValueFromParameter(35)
                    Me.n_PRSN_dataInserimento = oRequest.GetValueFromParameter(36)
                    Me.n_PRSN_mostraMail = oRequest.GetValueFromParameter(37)
                    Me.n_PRSN_TPPR.Descrizione = oRequest.GetValueFromParameter(38)
                    Me.n_PRSN_Ricevimento = oRequest.GetValueFromParameter(39)
                    Me.n_PRSN_MailSecondaria = oRequest.GetValueFromParameter(40)
                Catch ax As Exception
                    Me.n_PRSN_id = -1
                    Me.n_Errore = Errori_Db.DBReadExist
                End Try
            End Sub
            Public Function GetMansione() As Integer
                'tira su la mansione
                Dim oRequest As New COL_Request
                Dim oParam As New COL_Request.Parameter
                Dim responso As Integer
                Dim objAccesso As New COL_DataAccess

                With oRequest
                    .Command = "sp_Esterno_EstraiMansione"
                    .CommandType = CommandType.StoredProcedure
                    oParam = objAccesso.GetParameter("@PRSN_ID", Me.Id, ParameterDirection.Input, DbType.Int32)
                    .Parameters.Add(oParam)
                    oParam = objAccesso.GetParameter("@ESTR_Mansione", "", ParameterDirection.Output, , True, 100)
                    .Parameters.Add(oParam)
                    .Role = COL_Request.UserRole.Admin
                    .transactional = False
                End With
                Try
                    responso = objAccesso.GetExecuteNotQuery(oRequest)
                    Me.n_ESTR_Mansione = oRequest.GetValueFromParameter(2)
                Catch ex As Exception
                    Me.n_Errore = Errori_Db.DBError
                End Try
                Return responso
            End Function
            Public Shadows Sub ModificaNoPwd()
                Dim oRequest As New COL_Request
                Dim oParam As New COL_Request.Parameter
                Dim dsTable As Integer
                Dim objAccesso As New COL_DataAccess
                Try
                    Me.ModificaPersonaNoPwd()
                    If (Me.Id > 0) Then

                        With oRequest
                            .Command = "sp_Esterno_Modifica"
                            .CommandType = CommandType.StoredProcedure
                            oParam = objAccesso.GetParameter("@ESTR_PRSN", Me.Id, , DbType.Int32)
                            .Parameters.Add(oParam)
                            oParam = objAccesso.GetParameter("@ESTR_mansione", n_ESTR_Mansione.Trim)
                            .Parameters.Add(oParam)
                            .Role = COL_Request.UserRole.Admin
                            .transactional = False
                        End With
                        Try
                            dsTable = objAccesso.GetExecuteNotQuery(oRequest)
                            Me.n_ESTR_PRSN = Me.Id
                        Catch
                            ' Codice per il rollback

                            Me.n_ESTR_PRSN = -1
                            dsTable = -1
                        End Try
                    Else
                        Me.n_ESTR_PRSN = -1      'da rivedere seriamente 'OK
                        dsTable = -1
                    End If
                Catch
                    Me.n_ESTR_PRSN = -1
                    dsTable = -1
                End Try
            End Sub
#End Region

            Public Shared Function CreateFromPerson(ByVal oPerson As COL_Persona) As COL_Esterno
                Dim oExternalUser As New COL_Esterno
                With oExternalUser
                    .AUTN_ID = oPerson.AUTN_ID
                    .AUTN_RemoteUniqueID = oPerson.AUTN_RemoteUniqueID
                    .Nome = oPerson.Nome
                    .Cognome = oPerson.Cognome
                    .CodFiscale = oPerson.CodFiscale
                    .DataNascita = oPerson.DataNascita
                    .LuogoNascita = oPerson.LuogoNascita
                    .Login = oPerson.Login
                    .Mail = oPerson.Mail
                    .MailSecondaria = oPerson.MailSecondaria
                    .MostraMail = oPerson.MostraMail
                    .TipoPersona = oPerson.TipoPersona
                    .Provincia = oPerson.Provincia
                    .Stato = oPerson.Stato
                    .Sesso = oPerson.Sesso
                    .IsInterno = oPerson.IsInterno
                    .Lingua = oPerson.Lingua
                    .Istituzione = oPerson.Istituzione
                    .RicezioneSMS = oPerson.RicezioneSMS
                    .Pwd = oPerson.Pwd
                    .Note = oPerson.Note
                    .Bloccata = oPerson.Bloccata

                    .Cap = oPerson.Cap
                    .Cellulare = oPerson.Telefono1
                    .Citta = oPerson.Citta
                    .Fax = oPerson.Fax
                    .HomePage = oPerson.HomePage
                    .Indirizzo = oPerson.Indirizzo
                    .InfoRicevimento = oPerson.InfoRicevimento

                    .Job = oPerson.Job
                    .Sector = oPerson.Sector

                End With
                Return oExternalUser
            End Function

        End Class
    End Namespace
End Namespace