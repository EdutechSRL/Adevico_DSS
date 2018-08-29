Imports COL_DataLayer

Namespace Eventi
    Public Class COL_Evento
        Inherits ObjectBase

#Region "Private Property"
        Private n_EVNT_id As Integer
        Private n_EVNT_idPadre As Integer
        Private n_EVNT_nome As String
        Private n_EVNT_macro As Integer
        Private n_EVNT_visualizza As Integer
        Private n_EVNT_ripeti As Integer
        Private n_EVNT_dataFineRipeti As DateTime
        Private n_EVNT_note As String
        Private n_EVNT_luogo As String
        Private n_EVNT_link As String
        Private n_EVNT_dataModifica As DateTime
        Private n_EVNT_TPEV As New COL_Tipo_Evento
        Private n_EVNT_CMNT As New COL_Comunita
        Private n_EVNT_DBerror As New Errori_Db
        Private n_EVNT_annoAccademico As Integer
        Private n_EVNT_perpetuo As Integer
        Private n_EVNT_PRSN_ID As Integer
#End Region

#Region "Public Property"
        Public Property Id() As Integer
            Get
                Id = n_EVNT_id
            End Get
            Set(ByVal Value As Integer)
                n_EVNT_id = Value
            End Set
        End Property
        Public Property idPadre() As String
            Get
                idPadre = n_EVNT_idPadre
            End Get
            Set(ByVal Value As String)
                n_EVNT_idPadre = Value
            End Set
        End Property
        Public Property Nome() As String
            Get
                Nome = n_EVNT_nome
            End Get
            Set(ByVal Value As String)
                n_EVNT_nome = Value
            End Set
        End Property
        Public Property Macro() As Boolean
            Get
                Macro = (n_EVNT_macro = 1)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    n_EVNT_macro = 1
                Else
                    n_EVNT_macro = 0
                End If
            End Set
        End Property
        Public Property Visualizza() As Boolean
            Get
                Visualizza = (n_EVNT_visualizza = 1)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    n_EVNT_visualizza = 1
                Else
                    n_EVNT_visualizza = 0
                End If
            End Set
        End Property
        Public Property Ripeti() As Boolean
            Get
                Ripeti = (n_EVNT_ripeti = 1)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    n_EVNT_ripeti = 1
                Else
                    n_EVNT_ripeti = 0
                End If
            End Set
        End Property
		'Public Property DataFineRipetizione() As DateTime
		'    Get
		'        DataFineRipetizione = n_EVNT_dataFineRipeti
		'    End Get
		'    Set(ByVal Value As DateTime)
		'        n_EVNT_dataFineRipeti = Value
		'    End Set
		'End Property
        Public Property Note() As String
            Get
                Note = n_EVNT_note
            End Get
            Set(ByVal Value As String)
                n_EVNT_note = Value
            End Set
        End Property
        Public Property Luogo() As String
            Get
                Luogo = n_EVNT_luogo
            End Get
            Set(ByVal Value As String)
                n_EVNT_luogo = Value
            End Set
        End Property
        Public Property Link() As String
            Get
                Link = n_EVNT_link
            End Get
            Set(ByVal Value As String)
                n_EVNT_link = Value
            End Set
        End Property
        Public Property DataModifica() As DateTime
            Get
                DataModifica = n_EVNT_dataModifica
            End Get
            Set(ByVal Value As DateTime)
                n_EVNT_dataModifica = Value
            End Set
        End Property
        Public Property TipoEvento() As COL_Tipo_Evento
            Get
                TipoEvento = n_EVNT_TPEV
            End Get
            Set(ByVal Value As COL_Tipo_Evento)
                n_EVNT_TPEV = Value
            End Set
        End Property
        Public Property Comunita() As COL_Comunita
            Get
                Comunita = n_EVNT_CMNT
            End Get
            Set(ByVal Value As COL_Comunita)
                n_EVNT_CMNT = Value
            End Set
        End Property
        Public Property AnnoAccademico() As Integer
            Get
                AnnoAccademico = n_EVNT_annoAccademico
            End Get
            Set(ByVal Value As Integer)
                n_EVNT_annoAccademico = Value
            End Set
        End Property

        Public Property idPersona() As Integer
            Get
                idPersona = n_EVNT_PRSN_ID
            End Get
            Set(ByVal Value As Integer)
                n_EVNT_PRSN_ID = Value
            End Set
        End Property

        Public Property Perpetuo() As Boolean
            Get
                Perpetuo = (n_EVNT_perpetuo = 1)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    n_EVNT_perpetuo = 1
                Else
                    n_EVNT_perpetuo = 0
                End If
            End Set
        End Property
        Public ReadOnly Property Errore() As Errori_Db
            Get
                Errore = n_EVNT_DBerror
            End Get
        End Property

#End Region

#Region "Metodi Standard"
        Public Sub Aggiungi()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim DataModifica As String = DateToString(Now, True)
			'Dim DataFineRipeti As String
			'If Equals(Me.n_EVNT_dataFineRipeti, New Date) Then
			'    DataFineRipeti = Nothing
			'Else
			'    DataFineRipeti = DateToString(Me.n_EVNT_dataFineRipeti)
			'End If
            With oRequest
                .Command = "sp_Evento_Aggiungi"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@EVNT_id", "", ParameterDirection.Output, DbType.Int64)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_idPadre", n_EVNT_idPadre, ParameterDirection.Input, DbType.Int64)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_nome", n_EVNT_nome, ParameterDirection.Input, DbType.String, , 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_macro", n_EVNT_macro, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_visualizza", n_EVNT_visualizza, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_ripeti", n_EVNT_ripeti, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_note", n_EVNT_note, ParameterDirection.Input, DbType.String, , 200)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_luogo", n_EVNT_luogo, , DbType.String, , 200)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_link", n_EVNT_link, ParameterDirection.Input, DbType.String, True, 2500)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_dataModifica", DataModifica, ParameterDirection.Input, DbType.String, , 30)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_TPEV_id", n_EVNT_TPEV.Id, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_CMNT_id", n_EVNT_CMNT.Id, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_annoAccademico", Me.n_EVNT_annoAccademico, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_perpetuo", Me.n_EVNT_perpetuo, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_PRSN_ID", n_EVNT_PRSN_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_EVNT_DBerror = Errori_Db.None
                Me.n_EVNT_id = oRequest.GetValueFromParameter(1)
            Catch ex As Exception
                Me.n_EVNT_DBerror = Errori_Db.DBError
            End Try
        End Sub
        Public Sub Modifica()
            'metodo GENERALE per la modifica di qualsiasi tipo di evento
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim DataModifica As String = DateToString(Now, True)
            'Dim DataFine As String = DateToString(Me.Fine)
            With oRequest
                .Command = "sp_Evento_Modifica"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@EVNT_id", n_EVNT_id, ParameterDirection.Input, DbType.Int64)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_idPadre", n_EVNT_idPadre, ParameterDirection.Input, DbType.Int64)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_nome", n_EVNT_nome, ParameterDirection.Input, DbType.String, , 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_macro", n_EVNT_macro, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_visualizza", n_EVNT_visualizza, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_ripeti", n_EVNT_ripeti, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_note", n_EVNT_note.Trim, ParameterDirection.Input, DbType.String, , 200)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_link", n_EVNT_link, ParameterDirection.Input, DbType.String, , 2500)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_dataModifica", DataModifica, ParameterDirection.Input, DbType.String, , 30)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPEV_id", n_EVNT_TPEV.Id, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_id", n_EVNT_CMNT.Id, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_annoAccademico", Me.n_EVNT_annoAccademico, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_perpetuo", Me.n_EVNT_perpetuo, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_luogo", Me.n_EVNT_luogo, , DbType.String, , 200)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_PRSN_ID", n_EVNT_PRSN_ID, , DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch
                Me.n_EVNT_DBerror = Errori_Db.DBReadExist
            End Try
        End Sub
        Public Sub Estrai()
            'carica i campi del db nell'oggetto in base all'id dell'evento
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Evento_Estrai"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@EVNT_id", n_EVNT_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_idPadre", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_nome", "", ParameterDirection.Output, DbType.String, False, 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_macro", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_visualizza", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_ripeti", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_note", "", ParameterDirection.Output, DbType.String, False, 200)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_luogo", "", ParameterDirection.Output, DbType.String, False, 200)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_link", "", ParameterDirection.Output, DbType.String, False, 2500)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_dataModifica", "", ParameterDirection.Output, DbType.DateTime)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_TPEV_id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_CMNT_id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_annoAccademico", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_perpetuo", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@EVNT_PRSN_ID", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                If oRequest.GetValueFromParameter(2) = "" Then
                    Me.n_EVNT_idPadre = 0
                Else
                    Me.n_EVNT_idPadre = oRequest.GetValueFromParameter(2)
                End If

                Me.n_EVNT_nome = oRequest.GetValueFromParameter(3)
                Me.n_EVNT_macro = oRequest.GetValueFromParameter(4)
                Me.n_EVNT_visualizza = oRequest.GetValueFromParameter(5)
                Me.n_EVNT_ripeti = oRequest.GetValueFromParameter(6)
                Me.n_EVNT_note = oRequest.GetValueFromParameter(7)
                Me.n_EVNT_luogo = oRequest.GetValueFromParameter(8)
                Me.n_EVNT_link = oRequest.GetValueFromParameter(9)
                Me.n_EVNT_dataModifica = oRequest.GetValueFromParameter(10)
                Me.n_EVNT_TPEV.Id = oRequest.GetValueFromParameter(11)
                Me.n_EVNT_CMNT.Id = oRequest.GetValueFromParameter(12)
                Me.n_EVNT_annoAccademico = oRequest.GetValueFromParameter(13)
                Me.n_EVNT_perpetuo = oRequest.GetValueFromParameter(14)
                Me.n_EVNT_PRSN_ID = oRequest.GetValueFromParameter(15)
            Catch ax As Exception
                Me.n_EVNT_DBerror = Errori_Db.DBReadExist
            End Try
        End Sub
        Public Sub Cancella()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Evento_Elimina"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@EVNT_id", n_EVNT_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch ex As Exception
                Me.n_EVNT_DBerror = Errori_Db.DBError
            End Try

        End Sub
#End Region

#Region "Metodi in disuso"

#Region "Metodo in disuso1"
        'Public Function EstraiLezione()
        '    'carica i campi del db nell'oggetto in base all'id della comunità e all'id del tipo evento = lezione
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim objAccesso As New COL_DataAccess
        '    With oRequest
        '        .Command = "sp_Evento_EstraiLezioneComunita"
        '        .CommandType = CommandType.StoredProcedure
        '        oParam = objAccesso.GetParameter("@EVNT_id", "", ParameterDirection.Output, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@EVNT_idPadre", "", ParameterDirection.Output, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@EVNT_nome", "", ParameterDirection.Output, DbType.String, False, 100)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@EVNT_macro", "", ParameterDirection.Output, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@EVNT_visualizza", "", ParameterDirection.Output, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@EVNT_ripeti", "", ParameterDirection.Output, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@EVNT_note", "", ParameterDirection.Output, DbType.String, False, 200)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@EVNT_luogo", "", ParameterDirection.Output, DbType.String, False, 200)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@EVNT_link", "", ParameterDirection.Output, DbType.String, False, 25)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@EVNT_dataModifica", "", ParameterDirection.Output, DbType.DateTime)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@CMNT_id", Me.n_EVNT_CMNT.Id, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@EVNT_annoAccademico", "", ParameterDirection.Output, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@EVNT_perpetuo", "", ParameterDirection.Output, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With

        '    Try
        '        objAccesso.GetExecuteNotQuery(oRequest)
        '        Me.n_EVNT_id = oRequest.GetValueFromParameter(1)
        '        Me.n_EVNT_idPadre = oRequest.GetValueFromParameter(2)
        '        Me.n_EVNT_nome = oRequest.GetValueFromParameter(3)
        '        Me.n_EVNT_macro = oRequest.GetValueFromParameter(4)
        '        Me.n_EVNT_visualizza = oRequest.GetValueFromParameter(5)
        '        Me.n_EVNT_ripeti = oRequest.GetValueFromParameter(6)
        '        Me.n_EVNT_note = oRequest.GetValueFromParameter(7)
        '        Me.n_EVNT_luogo = oRequest.GetValueFromParameter(8)
        '        Me.n_EVNT_link = oRequest.GetValueFromParameter(9)
        '        Me.n_EVNT_dataModifica = oRequest.GetValueFromParameter(10)
        '        Me.n_EVNT_annoAccademico = oRequest.GetValueFromParameter(12)
        '        Me.n_EVNT_perpetuo = oRequest.GetValueFromParameter(13)
        '    Catch ax As Exception
        '        Me.n_EVNT_DBerror = Errori_Db.DBReadExist
        '    End Try
        'End Function
#End Region
#Region "Metodo in disuso2"
        'LOLLO
        'Public Function TrovaLezioniGeneriche(ByVal annoAccademico1 As String, ByVal annoAccademico2 As String, ByVal idCom As Int32) As DataSet
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim dsTable As DataSet
        '    Dim objAccesso As New COL_DataAccess

        '    With oRequest
        '        .Command = "sp_TrovaLezioniGeneriche" 'prova
        '        .CommandType = CommandType.StoredProcedure
        '        oParam = objAccesso.GetParameter("@EVNT_annoAccademico1", annoAccademico1, ParameterDirection.Input, DbType.String, , 50)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@EVNT_annoAccademico2", annoAccademico2, ParameterDirection.Input, DbType.String, , 50)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@CMNT_id", idCom, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        dsTable = objAccesso.GetdataSet(oRequest)
        '        Me.n_EVNT_DBerror = Errori_Db.None
        '    Catch ex As Exception
        '        Me.n_EVNT_DBerror = Errori_Db.DBError
        '    End Try
        '    Return dsTable
        'End Function
#End Region
#Region "Metodo in disuso3"
        'Public Function TrovaEventiAnnualiLimbo(ByVal PRSN_id As Int32) As DataSet
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim dsTable As DataSet
        '    Dim objAccesso As New COL_DataAccess
        '    With oRequest
        '        .Command = "sp_Evento_Limbo_Annuale" 'prova
        '        .CommandType = CommandType.StoredProcedure
        '        'oParam = objAccesso.GetParameter("@anno", anno, ParameterDirection.Input, DbType.Int32)
        '        '.Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@PRSN_id", PRSN_id, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        dsTable = objAccesso.GetdataSet(oRequest)
        '        Me.n_EVNT_DBerror = Errori_Db.None
        '    Catch ex As Exception
        '        Me.n_EVNT_DBerror = Errori_Db.DBError
        '    End Try
        '    Return dsTable
        'End Function

#End Region
#Region "Metodo in disuso4"
        'Public Function TrovaEventiGiornaleriLimbo(ByVal PRSN_id As Int32, ByVal giorno As String) As DataSet
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim dsTable As DataSet
        '    Dim objAccesso As New COL_DataAccess
        '    With oRequest
        '        .Command = "sp_Evento_Limbo_Giornaliero" 'prova
        '        .CommandType = CommandType.StoredProcedure
        '        'oParam = objAccesso.GetParameter("@anno", anno, ParameterDirection.Input, DbType.Int32)
        '        '.Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@PRSN_id", PRSN_id, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@giorno", giorno, ParameterDirection.Input, DbType.String, , 50)
        '        .Parameters.Add(oParam)
        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        dsTable = objAccesso.GetdataSet(oRequest)
        '        Me.n_EVNT_DBerror = Errori_Db.None
        '    Catch ex As Exception
        '        Me.n_EVNT_DBerror = Errori_Db.DBError
        '    End Try
        '    Return dsTable
        'End Function
#End Region
#Region "Metodo in disuso5"
        'LOLLO
        'Public Function TrovaEventiLimbo(ByVal PRSN_id As Int32) As DataSet
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim dsTable As DataSet
        '    Dim objAccesso As New COL_DataAccess
        '    With oRequest
        '        .Command = "sp_Evento_Estrai_Limbo"
        '        .CommandType = CommandType.StoredProcedure
        '        oParam = objAccesso.GetParameter("@PRSN_id", PRSN_id, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@TPEV_id", Me.n_EVNT_TPEV.Id, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        dsTable = objAccesso.GetdataSet(oRequest)
        '        Me.n_EVNT_DBerror = Errori_Db.None
        '    Catch ex As Exception
        '        Me.n_EVNT_DBerror = Errori_Db.DBError
        '    End Try
        '    Return dsTable
        'End Function
#End Region
#Region "Metodo in disuso6"
        'Public Function TrovaEventiAnnualiLimbo(ByVal PRSN_id As Integer, ByVal Anno As Integer) As DataSet
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim dsTable As DataSet
        '    Dim objAccesso As New COL_DataAccess
        '    With oRequest
        '        .Command = "sp_Evento_AnnualiLimbo"
        '        .CommandType = CommandType.StoredProcedure

        '        oParam = objAccesso.GetParameter("@EVNT_Anno", Anno, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@PRSN_id", PRSN_id, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@TPEV_id", Me.n_EVNT_TPEV.Id, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        dsTable = objAccesso.GetdataSet(oRequest)
        '        Me.n_EVNT_DBerror = Errori_Db.None
        '    Catch ex As Exception
        '        Me.n_EVNT_DBerror = Errori_Db.DBError
        '    End Try
        '    Return dsTable
        'End Function
#End Region
#Region "Metodo in disuso7"
        'Public Function TrovaEventiMensiliLimbo(ByVal PRSN_id As Integer, ByVal Anno As Integer, ByVal Mese As Integer) As DataSet
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim dsTable As DataSet
        '    Dim objAccesso As New COL_DataAccess
        '    With oRequest
        '        .Command = "sp_Evento_MensiliLimbo"
        '        .CommandType = CommandType.StoredProcedure

        '        oParam = objAccesso.GetParameter("@EVNT_Anno", Anno, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@EVNT_Mese", Mese, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@PRSN_id", PRSN_id, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@TPEV_id", Me.n_EVNT_TPEV.Id, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        dsTable = objAccesso.GetdataSet(oRequest)
        '        Me.n_EVNT_DBerror = Errori_Db.None
        '    Catch ex As Exception
        '        Me.n_EVNT_DBerror = Errori_Db.DBError
        '    End Try
        '    Return dsTable
        'End Function
#End Region
#Region "Metodo in disuso8"
        'Public Function TrovaEventiSettimanaliLimbo(ByVal PRSN_id As Integer, ByVal Anno As Integer, ByVal Mese As Integer, ByVal GiornoSett As Integer) As DataSet
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim dsTable As New DataSet
        '    Dim objAccesso As New COL_DataAccess
        '    With oRequest
        '        .Command = "sp_Evento_SettimanaliLimbo" 'prova
        '        .CommandType = CommandType.StoredProcedure
        '        oParam = objAccesso.GetParameter("@EVNT_Anno", Anno, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@EVNT_Mese", Mese, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@EVNT_GiornoSett", GiornoSett, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@PRSN_id", PRSN_id, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@TPEV_id", Me.n_EVNT_TPEV.Id, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        dsTable = objAccesso.GetdataSet(oRequest)
        '        Me.n_EVNT_DBerror = Errori_Db.None
        '    Catch ex As Exception
        '        Me.n_EVNT_DBerror = Errori_Db.DBError
        '    End Try
        '    Return dsTable
        'End Function

#End Region
#Region "Metodo in disuso9"
        'Public Function TrovaEventiAnnualiComunita(ByVal CMNT_ID As Integer, ByVal Anno As Integer) As DataSet
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim dsTable As New DataSet
        '    Dim objAccesso As New COL_DataAccess
        '    With oRequest
        '        .Command = "sp_Evento_AnnualiComunita" 'prova
        '        .CommandType = CommandType.StoredProcedure
        '        oParam = objAccesso.GetParameter("@EVNT_Anno", Anno, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@CMNT_id", CMNT_ID, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@TPEV_id", Me.n_EVNT_TPEV.Id, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        dsTable = objAccesso.GetdataSet(oRequest)
        '        Me.n_EVNT_DBerror = Errori_Db.None
        '    Catch ex As Exception
        '        Me.n_EVNT_DBerror = Errori_Db.DBError
        '    End Try
        '    Return dsTable
        'End Function
#End Region
#Region "Metodo in disuso10"
        'Public Function TrovaEventiMensiliComunita(ByVal CMNT_ID As Integer, ByVal Anno As Integer, ByVal Mese As Integer) As DataSet
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim dsTable As New DataSet
        '    Dim objAccesso As New COL_DataAccess
        '    With oRequest
        '        .Command = "sp_Evento_MensiliComunita" 'prova
        '        .CommandType = CommandType.StoredProcedure
        '        oParam = objAccesso.GetParameter("@EVNT_Anno", Anno, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@EVNT_Mese", Mese, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@CMNT_id", CMNT_ID, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@TPEV_id", Me.n_EVNT_TPEV.Id, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        dsTable = objAccesso.GetdataSet(oRequest)
        '        Me.n_EVNT_DBerror = Errori_Db.None
        '    Catch ex As Exception
        '        Me.n_EVNT_DBerror = Errori_Db.DBError
        '    End Try
        '    Return dsTable
        'End Function
#End Region
#Region "Metodo in disuso11"
        'Public Function TrovaEventiSettimanaliComunita(ByVal CMNT_ID As Integer, ByVal Anno As Integer, ByVal Mese As Integer, ByVal GiornoSett As Integer) As DataSet
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim dsTable As New DataSet
        '    Dim objAccesso As New COL_DataAccess
        '    With oRequest
        '        .Command = "sp_Evento_SettimanaliComunita" 'prova
        '        .CommandType = CommandType.StoredProcedure
        '        oParam = objAccesso.GetParameter("@EVNT_Anno", Anno, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@EVNT_Mese", Mese, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@EVNT_GiornoSett", GiornoSett, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@CMNT_id", CMNT_ID, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@TPEV_id", Me.n_EVNT_TPEV.Id, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        dsTable = objAccesso.GetdataSet(oRequest)
        '        Me.n_EVNT_DBerror = Errori_Db.None
        '    Catch ex As Exception
        '        Me.n_EVNT_DBerror = Errori_Db.DBError
        '    End Try
        '    Return dsTable
        'End Function
#End Region
#Region "Metodo in disuso12"
        'LOLLO
        'Public Function TrovaEventiGiornaleriLimbo(ByVal PRSN_id As Integer, ByVal giorno As String) As DataSet
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim dsTable As DataSet
        '    Dim objAccesso As New COL_DataAccess
        '    With oRequest
        '        .Command = "sp_Evento_Limbo_Giornaliero" 'prova
        '        .CommandType = CommandType.StoredProcedure
        '        oParam = objAccesso.GetParameter("@TPEV_id", Me.n_EVNT_TPEV.Id, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@PRSN_id", PRSN_id, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@giorno", giorno, ParameterDirection.Input, DbType.String, , 50)
        '        .Parameters.Add(oParam)
        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        dsTable = objAccesso.GetdataSet(oRequest)
        '        Me.n_EVNT_DBerror = Errori_Db.None
        '    Catch ex As Exception
        '        Me.n_EVNT_DBerror = Errori_Db.DBError
        '    End Try
        '    Return dsTable
        'End Function
#End Region
#Region "Metodo in disuso13"
        'LOLLO
        'Public Function TrovaEventiComunita(ByVal annoAccademico1 As Integer, ByVal annoAccademico2 As Integer, ByVal CMNT_ID As Integer) As DataSet
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim dsTable As New DataSet
        '    Dim objAccesso As New COL_DataAccess
        '    With oRequest
        '        .Command = "sp_Evento_Estrai_Comunita" 'prova
        '        .CommandType = CommandType.StoredProcedure
        '        oParam = objAccesso.GetParameter("@EVNT_annoAccademico1", annoAccademico1, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@EVNT_annoAccademico2", annoAccademico2, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@CMNT_id", CMNT_ID, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@TPEV_id", Me.n_EVNT_TPEV.Id, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        dsTable = objAccesso.GetdataSet(oRequest)
        '        Me.n_EVNT_DBerror = Errori_Db.None
        '    Catch ex As Exception
        '        Me.n_EVNT_DBerror = Errori_Db.DBError
        '    End Try
        '    Return dsTable
        'End Function

#End Region
#Region "Metodo in disuso14"
        'LOLLO
        'Public Function TrovaEventiGiornalieriComunita(ByVal giorno As String) As DataSet
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim dsTable As DataSet
        '    Dim objAccesso As New COL_DataAccess
        '    With oRequest
        '        .Command = "sp_Evento_Comunita_Giornaliero" 'prova
        '        .CommandType = CommandType.StoredProcedure
        '        oParam = objAccesso.GetParameter("@TPEV_id", Me.n_EVNT_TPEV.Id, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@CMNT_id", Me.n_EVNT_CMNT.Id, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@giorno", giorno, ParameterDirection.Input, DbType.String, , 50)
        '        .Parameters.Add(oParam)
        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        dsTable = objAccesso.GetdataSet(oRequest)
        '        Me.n_EVNT_DBerror = Errori_Db.None
        '    Catch ex As Exception
        '        Me.n_EVNT_DBerror = Errori_Db.DBError
        '    End Try
        '    Return dsTable
        'End Function
#End Region
#Region "Metodo in disuso15"
        'Public Function TrovaEventiDaModificare(ByVal data1 As String, ByVal data2 As String, ByVal Tipo_Ricerca As Int32) As DataSet
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim dsTable As DataSet
        '    Dim objAccesso As New COL_DataAccess
        '    With (oRequest)
        '        .Command = "sp_Evento_Estrai_PerModifica" 'prova
        '        .CommandType = CommandType.StoredProcedure
        '        oParam = objAccesso.GetParameter("@data1", data1, ParameterDirection.Input, DbType.String, , 50)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@data2", data2, ParameterDirection.Input, DbType.String, , 50)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@CMNT_id", Me.n_EVNT_CMNT.Id, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@TPEV_id", Me.n_EVNT_TPEV.Id, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@Tipo_Ricerca", Tipo_Ricerca, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        dsTable = objAccesso.GetdataSet(oRequest)
        '        Me.n_EVNT_DBerror = Errori_Db.None
        '    Catch ex As Exception
        '        Me.n_EVNT_DBerror = Errori_Db.DBError
        '    End Try
        '    Return dsTable
        'End Function
#End Region
#Region "Metodo in disuso16"
        'Public Shared Function GetAnniAccademici(Optional ByVal ORGN_ID As Integer = -1) As DataSet
        '    Dim oDataset As New DataSet
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim objAccesso As New COL_DataAccess

        '    With oRequest
        '        .Command = "sp_Evento_GetAnniAccademici"
        '        .CommandType = CommandType.StoredProcedure

        '        oParam = objAccesso.GetParameter("@ORGN_ID", ORGN_ID, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        oDataset = objAccesso.GetdataSet(oRequest)

        '    Catch ex As Exception

        '    End Try
        '    Return oDataset
        'End Function
#End Region
#End Region

        'Public Shared Function GetLezioni(ByVal CMNT_ID As Integer, ByVal oVisibilita As Main.FiltroVisibilità) As DataSet
        '	Dim oRequest As New COL_Request
        '	Dim oParam As New COL_Request.Parameter
        '	Dim objAccesso As New COL_DataAccess
        '	Dim oDataset As DataSet = Nothing

        '	With oRequest
        '		.Command = "sp_Evento_GetLezioniForComunita"
        '		.CommandType = CommandType.StoredProcedure

        '		oParam = objAccesso.GetParameter("@CMNT_id", CMNT_ID, ParameterDirection.Input, DbType.Int32)
        '		.Parameters.Add(oParam)

        '		oParam = objAccesso.GetParameter("@visibilita", CType(oVisibilita, Main.FiltroVisibilità), ParameterDirection.Input, DbType.Int32)
        '		.Parameters.Add(oParam)

        '		.Role = COL_Request.UserRole.Admin
        '		.transactional = False
        '	End With

        '	Try
        '		oDataset = objAccesso.GetdataSet(oRequest)
        '	Catch ax As Exception
        '	End Try
        '	Return oDataset
        'End Function


        '      Public Shared Function CancellaLezioni(ByVal CMNT_ID As Integer) As Boolean
        '          Dim oRequest As New COL_Request
        '          Dim oParam As New COL_Request.Parameter
        '          Dim objAccesso As New COL_DataAccess
        '          Dim iResponse As Boolean = False

        '          With oRequest
        '              .Command = "sp_Evento_CancellaLezioniForComunita"
        '              .CommandType = CommandType.StoredProcedure

        '              oParam = objAccesso.GetParameter("@CMNT_id", CMNT_ID, ParameterDirection.Input, DbType.Int32)
        '              .Parameters.Add(oParam)

        '              .Role = COL_Request.UserRole.Admin
        '              .transactional = False
        '          End With

        '          Try
        '              objAccesso.GetExecuteNotQuery(oRequest)
        '              iResponse = True
        '          Catch ax As Exception
        '          End Try
        '          Return iResponse
        '      End Function
		Public Function TrovaEventi(ByVal data1 As Date, ByVal data2 As Date, ByVal Tipo_Ricerca As Int32, ByVal PRSN_id As Integer, ByVal PermVisibilita As MyServices.PermissionType, ByVal LNGU_id As Integer, Optional ByVal andCondition As Boolean = False, Optional ByVal Perm1 As MyServices.PermissionType = MyServices.PermissionType.None, Optional ByVal Perm2 As MyServices.PermissionType = MyServices.PermissionType.None, Optional ByVal Perm3 As MyServices.PermissionType = MyServices.PermissionType.None, Optional ByVal Perm4 As MyServices.PermissionType = MyServices.PermissionType.None, Optional ByVal Perm5 As MyServices.PermissionType = MyServices.PermissionType.None) As DataSet
			Dim oRequest As New COL_Request
			Dim oParam As New COL_Request.Parameter
			Dim dsTable As DataSet = Nothing
			Dim objAccesso As New COL_DataAccess
			With (oRequest)
				.Command = "sp_Evento_Ricerca"
				.CommandType = CommandType.StoredProcedure
				oParam = objAccesso.GetParameter("@data1", DateToString(data1, False), ParameterDirection.Input, DbType.String, , 50)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@data2", DateToString(data2, False), ParameterDirection.Input, DbType.String, , 50)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@CMNT_id", Me.n_EVNT_CMNT.Id, ParameterDirection.Input, DbType.Int32)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@TPEV_id", Me.n_EVNT_TPEV.Id, ParameterDirection.Input, DbType.Int32)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@Tipo_Ricerca", Tipo_Ricerca, ParameterDirection.Input, DbType.Int32)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@PRSN_id", PRSN_id, ParameterDirection.Input, DbType.Int32)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@LNGU_id", LNGU_id, ParameterDirection.Input, DbType.Int32)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@PermessoVisibilita", CType(PermVisibilita, MyServices.PermissionType), ParameterDirection.Input, DbType.Int32)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@PosizionePermesso1", CType(Perm1, MyServices.PermissionType), ParameterDirection.Input, DbType.Int32)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@PosizionePermesso2", CType(Perm2, MyServices.PermissionType), ParameterDirection.Input, DbType.Int32)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@PosizionePermesso3", CType(Perm3, MyServices.PermissionType), ParameterDirection.Input, DbType.Int32)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@PosizionePermesso4", CType(Perm4, MyServices.PermissionType), ParameterDirection.Input, DbType.Int32)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetParameter("@PosizionePermesso5", CType(Perm5, MyServices.PermissionType), ParameterDirection.Input, DbType.Int32)
				.Parameters.Add(oParam)

				If andCondition Then
					oParam = objAccesso.GetParameter("@permessiAND", 1, ParameterDirection.Input, DbType.Int32)
				Else
					oParam = objAccesso.GetParameter("@permessiAND", 0, ParameterDirection.Input, DbType.Int32)
				End If
				.Parameters.Add(oParam)
				.Role = COL_Request.UserRole.Admin
				.transactional = False
			End With
			Try
				dsTable = objAccesso.GetdataSet(oRequest)
				Me.n_EVNT_DBerror = Errori_Db.None
			Catch ex As Exception
				Me.n_EVNT_DBerror = Errori_Db.DBError
			End Try
			Return dsTable
		End Function

        Public Function TrovaEventiSettimanaliPersona(ByVal PRSN_id As Integer, ByVal giornoInizio As String, ByVal giornoFine As String, ByVal codiceServ As String, ByVal CMNT_id As Integer, ByVal TPEV_Id As String, ByVal PermVisibilita As MyServices.PermissionType, ByVal LNGU_id As Integer, Optional ByVal andCondition As Boolean = False, Optional ByVal Perm1 As MyServices.PermissionType = MyServices.PermissionType.None, Optional ByVal Perm2 As MyServices.PermissionType = MyServices.PermissionType.None, Optional ByVal Perm3 As MyServices.PermissionType = MyServices.PermissionType.None, Optional ByVal Perm4 As MyServices.PermissionType = MyServices.PermissionType.None, Optional ByVal Perm5 As MyServices.PermissionType = MyServices.PermissionType.None) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Evento_Settimanale_Persona"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@giornoInizio", giornoInizio, ParameterDirection.Input, DbType.String, , 40)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@giornoFine", giornoFine, ParameterDirection.Input, DbType.String, , 40)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_id", PRSN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Codice", codiceServ, ParameterDirection.Input, DbType.String, , 15)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_id", CMNT_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LNGU_id", LNGU_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPEV_id", TPEV_Id, ParameterDirection.Input, DbType.String, , 2000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PermessoVisibilita", CType(PermVisibilita, MyServices.PermissionType), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PosizionePermesso1", CType(Perm1, MyServices.PermissionType), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PosizionePermesso2", CType(Perm2, MyServices.PermissionType), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PosizionePermesso3", CType(Perm3, MyServices.PermissionType), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PosizionePermesso4", CType(Perm4, MyServices.PermissionType), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PosizionePermesso5", CType(Perm5, MyServices.PermissionType), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                If andCondition Then
                    oParam = objAccesso.GetParameter("@permessiAND", 1, ParameterDirection.Input, DbType.Int32)
                Else
                    oParam = objAccesso.GetParameter("@permessiAND", 0, ParameterDirection.Input, DbType.Int32)
                End If
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                dsTable = objAccesso.GetdataSet(oRequest)
                Me.n_EVNT_DBerror = Errori_Db.None
            Catch ex As Exception
                Dim a As String
                a = ex.Message()
                Me.n_EVNT_DBerror = Errori_Db.DBError
            End Try
            Return dsTable
        End Function

        Public Function getNUMricorrenze(ByVal ORRI_id As Integer) As Integer

            'Trova il numero di ricorrenze per l'evento

            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim NumRicorrenze As Integer
            With oRequest
                .Command = "sp_Evento_numRicorrenze"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@Nricorrenze", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ORRI_id", ORRI_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                NumRicorrenze = oRequest.GetValueFromParameter(1)
            Catch ex As Exception
                n_EVNT_DBerror = Errori_Db.DBReadExist
            End Try
            Return NumRicorrenze
        End Function


        'Public Shared Function GetPlainLezioni(ByVal CurrentUserID As Integer, ByVal ComunitaID As Integer, ByVal oVisibilita As Main.FiltroVisibilità, ByVal BaseUrlDownload As String, ByVal BaseUrlPath As String, ByVal BaseUrlDrivePath As String) As GenericCollection(Of PlainLezione)
        '	Dim oRequest As New COL_Request
        '	Dim oParam As New COL_Request.Parameter
        '	Dim objAccesso As New COL_DataAccess
        '	Dim iDataReader As IDataReader
        '	Dim oLezioni As New GenericCollection(Of PlainLezione)
        '	With oRequest
        '		.Command = "sp_Evento_GetLezioniForComunita"
        '		.CommandType = CommandType.StoredProcedure

        '		oParam = objAccesso.GetParameter("@CMNT_id", ComunitaID, ParameterDirection.Input, DbType.Int32)
        '		.Parameters.Add(oParam)

        '		oParam = objAccesso.GetParameter("@visibilita", CType(oVisibilita, Main.FiltroVisibilità), ParameterDirection.Input, DbType.Int32)
        '		.Parameters.Add(oParam)

        '		.Role = COL_Request.UserRole.Admin
        '		.transactional = False
        '	End With

        '	Try
        '		iDataReader = objAccesso.GetdataReader(oRequest)

        '		Dim numLezione As Integer = 0
        '		While iDataReader.Read
        '			Dim isVisible As Boolean = False
        '			Dim Luogo As String = ""
        '			Dim LuogoEvento As String = ""
        '			Dim LuogoAula As String = ""
        '			Dim Link As String = ""

        '			numLezione += 1


        '			If GenericValidator.ValBool(iDataReader("EVNT_visualizza"), False) And GenericValidator.ValBool(iDataReader("ORRI_visibile"), False) Then
        '				isVisible = True
        '			End If


        '			Link = GenericValidator.ValString(iDataReader("ORRI_Link"), "")
        '			LuogoEvento = GenericValidator.ValString(iDataReader("EVNT_luogo"), "")
        '			LuogoAula = GenericValidator.ValString(iDataReader("ORRI_aula"), "")
        '			If LuogoAula <> "" Then
        '				Luogo = LuogoAula
        '			Else
        '				Luogo = LuogoEvento
        '			End If

        '			oLezioni.Add(New PlainLezione(CurrentUserID, numLezione, iDataReader("EVNT_ID"), iDataReader("ORRI_ID"), _
        '			 GenericValidator.ValString(iDataReader("EVNT_nome"), ""), iDataReader("EVNT_annoAccademico"), isVisible, GenericValidator.ValBool(iDataReader("EVNT_macro"), False), _
        '			 iDataReader("EVNT_CMNT_id"), GenericValidator.ValData(iDataReader("ORRI_inizio"), Now), GenericValidator.ValData(iDataReader("ORRI_fine"), Now), _
        '			 GenericValidator.ValString(iDataReader("ORRI_note"), ""), GenericValidator.ValString(iDataReader("PREV_ProgrammaSvolto"), ""), _
        '			  Luogo, Link, BaseUrlDownload, BaseUrlPath, BaseUrlDrivePath, True, oVisibilita))


        '		End While

        '	Catch ex As Exception
        '	Finally
        '		If Not iDataReader.IsClosed Then
        '			iDataReader.Close()
        '		End If
        '	End Try
        '	Return oLezioni
        'End Function

        '      Public Shared Function GetPlainLezioni(ByVal CurrentUserID As Integer, ByVal CurrentRoleID As Integer, ByVal ComunitaID As Integer, ByVal oVisibilita As Main.FiltroVisibilità, ByVal BaseUrlDownload As String, ByVal BaseUrlPath As String, ByVal BaseUrlDrivePath As String, ByVal sortExpression As String, ByVal sortDirection As String) As GenericCollection(Of PlainLezione)
        '          Dim oLezioni As GenericCollection(Of PlainLezione)
        '	Dim cacheKey As String = CachePolicy.DiarioLezione(ComunitaID, CurrentRoleID)

        '	If sortDirection <> String.Empty Then
        '		sortDirection = sortDirection.ToLower
        '	End If

        '	If ObjectBase.Cache(cacheKey) Is Nothing Then
        '		oLezioni = COL_Evento.GetPlainLezioni(CurrentUserID, ComunitaID, oVisibilita, BaseUrlDownload, BaseUrlPath, BaseUrlDrivePath)
        '		ObjectBase.Cache.Insert(cacheKey, oLezioni, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza30minuti)
        '	Else
        '		oLezioni = CType(ObjectBase.Cache(cacheKey), GenericCollection(Of PlainLezione))
        '	End If

        '          If (Not sortExpression Is Nothing AndAlso sortDirection <> String.Empty) Then
        '              oLezioni.Sort(New GenericComparer(Of PlainLezione)(sortExpression))
        '          End If

        '          If (Not sortDirection Is Nothing AndAlso sortDirection = "desc") Then
        '              oLezioni.Reverse()
        '          End If

        '          Return oLezioni
        '      End Function


    End Class
End Namespace