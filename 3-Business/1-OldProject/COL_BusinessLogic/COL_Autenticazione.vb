Imports COL_DataLayer

Public Class COL_Autenticazione

#Region "Private Property"
    Private _AUTN_ID As Integer
    Private _AUTN_Nome As String
    Private _AUTN_Desc As String
    Private _Errore As Errori_Db
#End Region

#Region "Public Property"
    Public Enum TipoAutenticazione
        ComunitaOnLine = 1
        LDAP = 2
    End Enum
    Public Property ID() As Integer
        Get
            ID = _AUTN_ID
        End Get
        Set(ByVal Value As Integer)
            _AUTN_ID = Value
        End Set
    End Property
    Public Property Nome() As String
        Get
            Nome = _AUTN_Nome
        End Get
        Set(ByVal Value As String)
            _AUTN_Nome = Value
        End Set
    End Property
    Public Property Descrizione() As String
        Get
            Descrizione = _AUTN_Desc
        End Get
        Set(ByVal Value As String)
            _AUTN_Desc = Value
        End Set
    End Property
    Public ReadOnly Property Errore() As Errori_Db
        Get
            Errore = _Errore
        End Get
    End Property
#End Region

    Public Sub New()
        Me._Errore = Errori_Db.None
    End Sub

#Region "Metodi Standard"
	Public Function Elenca() As List(Of COL_Autenticazione)
		Dim oLista As New List(Of COL_Autenticazione)
		Dim oRequest As New COL_Request
		Dim oParam As New COL_Request.Parameter
		Dim objAccesso As New COL_DataAccess
		Dim oDatareader As IDataReader = Nothing

		With oRequest
			.Command = "sp_Autenticazione_Elenca"
			.CommandType = CommandType.StoredProcedure

			.Role = COL_Request.UserRole.Admin
			.transactional = False
		End With

		Try
			oDatareader = objAccesso.GetdataReader(oRequest)
			While oDatareader.Read
				Try
					oLista.Add(New COL_Autenticazione() With {.ID = oDatareader("AUTN_ID"), .Nome = GenericValidator.ValString(oDatareader("AUTN_Nome"), "")})
				Catch ex As Exception

				End Try
			End While
			oDatareader.Close()
		Catch ex As Exception
		Finally
			If Not IsNothing(oDatareader) Then
				If oDatareader.IsClosed = False Then
					oDatareader.Close()
				End If
			End If
		End Try
		Return oLista

	End Function
    Public Sub Aggiungi()
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_Autenticazione_Aggiungi"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetParameter("@AUTN_Id", "", ParameterDirection.Output, DbType.Int32)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@AUTN_nome", _AUTN_Nome, ParameterDirection.Input, DbType.String, , 50)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@AUTN_Desc", Me._AUTN_Desc, ParameterDirection.Input, DbType.String, , 250)
            .Parameters.Add(oParam)
            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            objAccesso.GetExecuteNotQuery(oRequest)
            Me._AUTN_ID = oRequest.GetValueFromParameter(1)
            Me._Errore = Errori_Db.None
        Catch ex As Exception
            Me._AUTN_ID = -1
            Me._Errore = Errori_Db.DBInsert
        End Try

    End Sub
    Public Sub Modifica()
        'esegue l'update del record desiderato della tabella PROVINCIA
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_Autenticazione_Modifica"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetParameter("@AUTN_Id", _AUTN_ID, ParameterDirection.Input, DbType.Int32)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@AUTN_nome", _AUTN_Nome, ParameterDirection.Input, DbType.String, , 50)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetParameter("@AUTN_Desc", _AUTN_Desc, ParameterDirection.Input, DbType.String, , 250)
            .Parameters.Add(oParam)
            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            objAccesso.GetExecuteNotQuery(oRequest)
            Me._Errore = Errori_Db.None
        Catch ex As Exception
            Me._Errore = Errori_Db.DBChange
        End Try
    End Sub
    Public Sub Cancella()
        'esegue l'update del record desiderato della tabella PROVINCIA
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_Autenticazione_Cancella"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetParameter("@AUTN_ID", _AUTN_ID, ParameterDirection.Input, DbType.Int32)
            .Parameters.Add(oParam)

            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            objAccesso.GetExecuteNotQuery(oRequest)
            Me._Errore = Errori_Db.None
        Catch ex As Exception
            Me._Errore = Errori_Db.DBDelete
        End Try
    End Sub
#End Region

    Public Shared Function List() As List(Of COL_Autenticazione)
        Dim oLista As New List(Of COL_Autenticazione)
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim objAccesso As New COL_DataAccess
        Dim oDatareader As IDataReader = Nothing

        With oRequest
            .Command = "sp_Autenticazione_Elenca"
            .CommandType = CommandType.StoredProcedure

            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With

        Try
            oDatareader = objAccesso.GetdataReader(oRequest)
            While oDatareader.Read
                Try
                    oLista.Add(New COL_Autenticazione() With {.ID = oDatareader("AUTN_ID"), .Nome = GenericValidator.ValString(oDatareader("AUTN_Nome"), "")})
                Catch ex As Exception

                End Try
            End While
            oDatareader.Close()
        Catch ex As Exception
        Finally
            If Not IsNothing(oDatareader) Then
                If oDatareader.IsClosed = False Then
                    oDatareader.Close()
                End If
            End If
        End Try
        Return oLista

    End Function
End Class