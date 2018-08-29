Imports COL_DataLayer

Namespace Statistiche
    Public Class COL_Statistiche

#Region "Private Property"
        Private n_STAT_DTBS_ID As Integer
        Private n_erroreDb As Errori_Db
#End Region

#Region "Public Property"
        Public Property Database_ID() As Integer
            Get
                Database_ID = n_STAT_DTBS_ID
            End Get
            Set(ByVal Value As Integer)
                n_STAT_DTBS_ID = Value
            End Set
        End Property
        Public ReadOnly Property Errore() As Errori_Db
            Get
                Errore = n_erroreDb
            End Get
        End Property
#End Region

#Region "Metodi New"
        Sub New(ByVal CodiceDB As Integer)
            Me.n_erroreDb = Errori_Db.None
            Me.n_STAT_DTBS_ID = CodiceDB
        End Sub
#End Region

#Region "Statistiche Accessi Sistema"
        Public Function ElencaAccessiGiornalieri(Optional ByVal giorno As Integer = -1, Optional ByVal mese As Integer = -1, Optional ByVal anno As Integer = -1, Optional ByVal idTPPR As Integer = -1, Optional ByVal ORGN_Id As Integer = -1, Optional ByVal FiltroElencoTipiPersona As Integer = 1) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess
            'FiltroElencoTipiPersona int = 1 -- 0 = con Guest
            With oRequest
                .Command = "sp_Statistiche_ElencaAccessiGiornalieri"
				.CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@STAC_Giorno", giorno, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@STAC_Mese", mese, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@STAC_Anno", anno, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@STAC_TPPR_id", idTPPR, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@DTBS_ID", n_STAT_DTBS_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_Id", ORGN_Id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@FiltroElencoTipiPersona", FiltroElencoTipiPersona, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                dsTable = objAccesso.GetdataSet(oRequest)
                Me.n_erroreDb = Errori_Db.None
			Catch ex As Exception
                'Dim oMail As New MailDBerrori
                'oMail.Oggetto = "Statistiche"
                'oMail.Body = "Errore=" & ex.Message
				Me.n_erroreDb = Errori_Db.DBError
            End Try
            Return dsTable
        End Function
		Public Function ElencaAccessiSettimanali(ByVal DataInizio As DateTime, ByVal DataFine As DateTime, Optional ByVal idTPPR As Integer = -1, Optional ByVal ORGN_Id As Integer = -1, Optional ByVal FiltroElencoTipiPersona As Integer = 1) As DataSet
			Dim oRequest As New COL_Request
			Dim oParam As New COL_Request.Parameter
			Dim dsTable As New DataSet
			Dim objAccesso As New COL_DataAccess

			With oRequest
				.Command = "sp_Statistiche_ElencaAccessiSettimanali"
				.CommandType = CommandType.StoredProcedure
				oParam = objAccesso.GetParameter("@DataInizio", DateToString(DataInizio, False), ParameterDirection.Input, DbType.String, , 50)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@DataFine", DateToString(DataFine, False), ParameterDirection.Input, DbType.String, , 50)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@STAC_TPPR_id", idTPPR, ParameterDirection.Input, DbType.Int32)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetAdvancedParameter("@DTBS_ID", n_STAT_DTBS_ID, ParameterDirection.Input, SqlDbType.Int)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetAdvancedParameter("@ORGN_Id", ORGN_Id, ParameterDirection.Input, SqlDbType.Int)
				.Parameters.Add(oParam)

				oParam = objAccesso.GetAdvancedParameter("@FiltroElencoTipiPersona", FiltroElencoTipiPersona, ParameterDirection.Input, SqlDbType.Int)
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

        Public Function ElencaAccessiMensili(Optional ByVal mese As Integer = -1, Optional ByVal anno As Integer = -1, Optional ByVal idTPPR As Integer = -1, Optional ByVal ORGN_Id As Integer = -1, Optional ByVal FiltroElencoTipiPersona As Integer = 1) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Statistiche_ElencaAccessiMensili"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@STAC_Mese", mese, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@STAC_Anno", anno, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@STAC_TPPR_id", idTPPR, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@DTBS_ID", n_STAT_DTBS_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_Id", ORGN_Id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroElencoTipiPersona", FiltroElencoTipiPersona, ParameterDirection.Input, SqlDbType.Int)
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
        Public Function ElencaAccessiAnnuali(Optional ByVal anno As Integer = -1, Optional ByVal idTPPR As Integer = -1, Optional ByVal ORGN_Id As Integer = -1, Optional ByVal FiltroElencoTipiPersona As Integer = 1) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Statistiche_ElencaAccessiAnnuali"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@STAC_Anno", anno, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@STAC_TPPR_id", idTPPR, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@DTBS_ID", n_STAT_DTBS_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_Id", ORGN_Id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroElencoTipiPersona", FiltroElencoTipiPersona, ParameterDirection.Input, SqlDbType.Int)
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

        Public Function ElencaIscritti(ByVal LinguaID As Integer, Optional ByVal ORGN_id As Integer = -1, Optional ByVal TPPR_id As Integer = -1, Optional ByVal FiltroElencoTipiPersona As Integer = 1) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Statistiche_ElencaIscritti"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_id", ORGN_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@TPPR_id", TPPR_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroElencoTipiPersona", FiltroElencoTipiPersona, ParameterDirection.Input, SqlDbType.Int)
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

        Public Function AndamentoIscrizioni(ByVal LinguaID As Integer, Optional ByVal ORGN_id As Integer = -1, Optional ByVal TPPR_id As Integer = -1, Optional ByVal anno As Integer = -1, Optional ByVal FiltroElencoTipiPersona As Integer = 1) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Statistiche_AndamentoIscrizioni"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_id", ORGN_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@TPPR_id", TPPR_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@anno", anno, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroElencoTipiPersona", FiltroElencoTipiPersona, ParameterDirection.Input, SqlDbType.Int)
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

        Public Function getRiepilogoAccessi(ByVal dataOdierna As DateTime, ByRef totaleG As Integer, ByRef totaleS As Integer, ByRef totaleM As Integer, ByRef totaleA As Integer, Optional ByVal ORGN_ID As Integer = -1, Optional ByVal FiltroElencoTipiPersona As Integer = 1) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResonse As Boolean = False
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Statistiche_getRiepilogoAccessi"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@totaleG", 0, ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@totaleS", 0, ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@totaleM", 0, ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@totaleA", 0, ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@giorno", Day(dataOdierna), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@mese", Month(dataOdierna), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@anno", Year(dataOdierna), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                Dim dataSettimana As New DateTime
                Select Case dataOdierna.DayOfWeek
                    Case DayOfWeek.Monday
                        dataSettimana = dataOdierna
                    Case DayOfWeek.Tuesday
                        dataSettimana = dataOdierna.AddDays(-1)
                    Case DayOfWeek.Wednesday
                        dataSettimana = dataOdierna.AddDays(-2)
                    Case DayOfWeek.Thursday
                        dataSettimana = dataOdierna.AddDays(-3)
                    Case DayOfWeek.Friday
                        dataSettimana = dataOdierna.AddDays(-4)
                    Case DayOfWeek.Saturday
                        dataSettimana = dataOdierna.AddDays(-5)
                    Case DayOfWeek.Sunday
                        dataSettimana = dataOdierna.AddDays(-6)
                End Select
                oParam = objAccesso.GetAdvancedParameter("@DataInizio", DateToString(dataSettimana, False), ParameterDirection.Input, SqlDbType.VarChar, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_ID", ORGN_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)



                'If dataOdierna.DayOfWeek = DayOfWeek.Sunday Then
                '    giornoSettimana -= 6
                'ElseIf dataOdierna.DayOfWeek <> DayOfWeek.Monday Then
                '    giornoSettimana = -Weekday(dataOdierna, FirstDayOfWeek.Monday) + 1
                'Else
                '    giornoSettimana = 0
                'End If
                'oParam = objAccesso.GetAdvancedParameter("@settimana", giornoSettimana, ParameterDirection.Input, SqlDbType.Int)
                '.Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@DTBS_ID", n_STAT_DTBS_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroElencoTipiPersona", FiltroElencoTipiPersona, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                totaleG = oRequest.GetValueFromParameter(1)
                totaleS = oRequest.GetValueFromParameter(2)
                totaleM = oRequest.GetValueFromParameter(3)
                totaleA = oRequest.GetValueFromParameter(4)
                iResonse = True
                Me.n_erroreDb = Errori_Db.None
            Catch ex As Exception
                Me.n_erroreDb = Errori_Db.DBError
            End Try
            Return iResonse

        End Function
#End Region

#Region "Statistiche Accessi Comunita"
        Public Function getMaxAnni_ForComunita(ByVal CMNT_ID As Integer) As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Statistiche_getMaxAnni_ForComunita"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_ID", CMNT_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@anno", 0, ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@DTBS_ID", n_STAT_DTBS_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                iResponse = oRequest.GetValueFromParameter(2)
                Me.n_erroreDb = Errori_Db.None
            Catch ex As Exception
                iResponse = Now.Year
                Me.n_erroreDb = Errori_Db.DBError
            End Try
            Return iResponse
        End Function
        Public Function getMinAnni_ForComunita(ByVal CMNT_ID As Integer) As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Statistiche_getMinAnni_ForComunita"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_ID", CMNT_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@anno", 0, ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@DTBS_ID", n_STAT_DTBS_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                iResponse = oRequest.GetValueFromParameter(2)
                Me.n_erroreDb = Errori_Db.None
            Catch ex As Exception
                iResponse = Now.Year
                Me.n_erroreDb = Errori_Db.DBError
            End Try
            Return iResponse
        End Function

        Public Function getElencoRuoliForComunita(ByVal CMNT_ID As Integer, ByVal LinguaID As Integer, Optional ByVal FiltroRuoli As Integer = 0) As DataSet
            '@FiltroRuoli int = 0  --    -1 con guest
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataset As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Statistiche_getElencoRuoliForComunita"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@CMNT_ID", CMNT_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@LinguaID", LinguaID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@DTBS_ID", n_STAT_DTBS_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroRuoli", FiltroRuoli, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)


                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                oDataset = objAccesso.GetdataSet(oRequest)
                Me.n_erroreDb = Errori_Db.None
            Catch ex As Exception
                Me.n_erroreDb = Errori_Db.DBError
            End Try
            Return oDataset
        End Function

        Public Function ElencaIscrittiComunita(ByVal CMNT_ID As Integer, ByVal LinguaID As Integer, Optional ByVal FiltroRuoli As Integer = 0) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Statistiche_ElencaIscrittiComunita"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@CMNT_ID", CMNT_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@LinguaID", LinguaID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroRuoli", FiltroRuoli, ParameterDirection.Input, SqlDbType.Int)
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

        Public Function StatisticheGiornaliere_Comunita(ByVal CMNT_ID As Integer, Optional ByVal giorno As Integer = -1, Optional ByVal mese As Integer = -1, Optional ByVal anno As Integer = -1, Optional ByVal idRuolo As Integer = -1, Optional ByVal FiltroRuoli As Integer = 0) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Statistiche_GiornaliereComunita"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@giorno", giorno, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@mese", mese, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@anno", anno, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@idRuolo", idRuolo, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_ID", CMNT_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@DTBS_ID", n_STAT_DTBS_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroRuoli", FiltroRuoli, ParameterDirection.Input, SqlDbType.Int)
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
		Public Function StatisticheSettimanali_Comunita(ByVal CMNT_ID As Integer, ByVal DataInizio As DateTime, ByVal DataFine As DateTime, Optional ByVal idRuolo As Integer = -1, Optional ByVal FiltroRuoli As Integer = 0) As DataSet
			Dim oRequest As New COL_Request
			Dim oParam As New COL_Request.Parameter
			Dim dsTable As New DataSet
			Dim objAccesso As New COL_DataAccess

			With oRequest
				.Command = "sp_Statistiche_SettimanaliComunita"
				.CommandType = CommandType.StoredProcedure
				oParam = objAccesso.GetParameter("@DataInizio", DateToString(DataInizio, False), ParameterDirection.Input, DbType.String, , 50)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@DataFine", DateToString(DataFine, False), ParameterDirection.Input, DbType.String, , 50)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@idRuolo", idRuolo, ParameterDirection.Input, DbType.Int32)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@CMNT_ID", CMNT_ID, ParameterDirection.Input, DbType.Int32)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetAdvancedParameter("@DTBS_ID", n_STAT_DTBS_ID, ParameterDirection.Input, SqlDbType.Int)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetAdvancedParameter("@FiltroRuoli", FiltroRuoli, ParameterDirection.Input, SqlDbType.Int)
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
        Public Function StatisticheMensili_Comunita(ByVal CMNT_ID As Integer, Optional ByVal mese As Integer = -1, Optional ByVal anno As Integer = -1, Optional ByVal idRuolo As Integer = -1, Optional ByVal FiltroRuoli As Integer = 0) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Statistiche_MensiliComunita"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@Mese", mese, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Anno", anno, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@idRuolo", idRuolo, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_ID", CMNT_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@DTBS_ID", n_STAT_DTBS_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@FiltroRuoli", FiltroRuoli, ParameterDirection.Input, SqlDbType.Int)
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
        Public Function StatisticheAnnuali_Comunita(ByVal CMNT_ID As Integer, Optional ByVal anno As Integer = -1, Optional ByVal idRuolo As Integer = -1, Optional ByVal FiltroRuoli As Integer = 0) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Statistiche_AnnualiComunita"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@Anno", anno, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@idRuolo", idRuolo, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_ID", CMNT_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@DTBS_ID", n_STAT_DTBS_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroRuoli", FiltroRuoli, ParameterDirection.Input, SqlDbType.Int)

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

        Public Function getRangeDateGenerali_Comunita(ByRef Inizio As DateTime, ByRef Fine As DateTime, Optional ByVal CMNT_ID As Integer = -1) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Statistiche_RangeDateGenerali_ForComunita"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@datainizioMin", "", ParameterDirection.Output, SqlDbType.DateTime)
                .Parameters.Add(oParam) 'datetime
                oParam = objAccesso.GetAdvancedParameter("@datafineMax", "", ParameterDirection.Output, SqlDbType.DateTime)
                .Parameters.Add(oParam) 'datetime
                oParam = objAccesso.GetParameter("@CMNT_ID", CMNT_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@DTBS_ID", n_STAT_DTBS_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Inizio = oRequest.GetValueFromParameter(1)
                Fine = oRequest.GetValueFromParameter(2)
                Me.n_erroreDb = Errori_Db.None
            Catch ex As Exception
                Me.n_erroreDb = Errori_Db.DBReadExist
                Return False
            End Try
            Return True
        End Function

        Public Function getRiepilogoAccessiComunita(ByVal CMNT_ID As Integer, ByVal dataOdierna As DateTime, ByRef totaleG As Integer, ByRef totaleS As Integer, ByRef totaleM As Integer, ByRef totaleA As Integer, Optional ByVal FiltroRuoli As Integer = 0) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResonse As Boolean = False
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Statistiche_getRiepilogoAccessiComunita"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@totaleG", 0, ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@totaleS", 0, ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@totaleM", 0, ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@totaleA", 0, ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@giorno", Day(dataOdierna), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@mese", Month(dataOdierna), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@anno", Year(dataOdierna), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)


                Dim dataSettimana As New DateTime
                Select Case dataOdierna.DayOfWeek
                    Case DayOfWeek.Monday
                        dataSettimana = dataOdierna
                    Case DayOfWeek.Tuesday
                        dataSettimana = dataOdierna.AddDays(-1)
                    Case DayOfWeek.Wednesday
                        dataSettimana = dataOdierna.AddDays(-2)
                    Case DayOfWeek.Thursday
                        dataSettimana = dataOdierna.AddDays(-3)
                    Case DayOfWeek.Friday
                        dataSettimana = dataOdierna.AddDays(-4)
                    Case DayOfWeek.Saturday
                        dataSettimana = dataOdierna.AddDays(-5)
                    Case DayOfWeek.Sunday
                        dataSettimana = dataOdierna.AddDays(-6)
                End Select




                oParam = objAccesso.GetAdvancedParameter("@Datainizio", DateToString(dataSettimana, False), ParameterDirection.Input, SqlDbType.VarChar, , 50)

                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", CMNT_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@DTBS_ID", n_STAT_DTBS_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroRuoli", FiltroRuoli, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                totaleG = oRequest.GetValueFromParameter(1)
                totaleS = oRequest.GetValueFromParameter(2)
                totaleM = oRequest.GetValueFromParameter(3)
                totaleA = oRequest.GetValueFromParameter(4)
                iResonse = True
                Me.n_erroreDb = Errori_Db.None
            Catch ex As Exception
                Me.n_erroreDb = Errori_Db.DBError
            End Try
            Return iResonse

        End Function

#End Region

      
        
        Public Function getRangeAnniGenerali() As Integer()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim AnnoMinMax(1) As Integer
            With oRequest
                .Command = "sp_Statistiche_RangeAnniGenerali"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@inizioMin", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@fineMax", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@DTBS_ID", n_STAT_DTBS_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                AnnoMinMax(0) = oRequest.GetValueFromParameter(1)
                AnnoMinMax(1) = oRequest.GetValueFromParameter(2)
            Catch ex As Exception
                Me.n_erroreDb = Errori_Db.DBReadExist
            End Try
            Return AnnoMinMax
        End Function

        Public Function getRangeAnniGenerali(ByRef Inizio As Integer, ByRef Fine As Integer) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            'Dim AnnoMinMax(1) As Integer
            With oRequest
                .Command = "sp_Statistiche_RangeAnniGenerali"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@inizioMin", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@fineMax", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@DTBS_ID", n_STAT_DTBS_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Inizio = oRequest.GetValueFromParameter(1)
                Fine = oRequest.GetValueFromParameter(2)
                Me.n_erroreDb = Errori_Db.None
            Catch ex As Exception
                Me.n_erroreDb = Errori_Db.DBReadExist
                Return False
            End Try
            Return True
        End Function

        Public Function getRangeDateGenerali(ByRef Inizio As DateTime, ByRef Fine As DateTime) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Statistiche_RangeDateGenerali"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@datainizioMin", "", ParameterDirection.Output, SqlDbType.DateTime)
                .Parameters.Add(oParam) 'datetime
                oParam = objAccesso.GetAdvancedParameter("@datafineMax", "", ParameterDirection.Output, SqlDbType.DateTime)
                .Parameters.Add(oParam) 'datetime
                oParam = objAccesso.GetAdvancedParameter("@DTBS_ID", n_STAT_DTBS_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Inizio = oRequest.GetValueFromParameter(1)
                Fine = oRequest.GetValueFromParameter(2)
                Me.n_erroreDb = Errori_Db.None
            Catch ex As Exception
                Me.n_erroreDb = Errori_Db.DBReadExist
                Return False
            End Try
            Return True
        End Function
    End Class
End Namespace