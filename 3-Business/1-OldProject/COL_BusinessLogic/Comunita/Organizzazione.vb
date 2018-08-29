Imports COL_DataLayer
Imports COL_BusinessLogic_v2.FileLayer
Imports lm.Comol.Core.File

Namespace Comunita
    Public Class COL_Organizzazione
		Inherits ObjectBase

#Region "Private Property"
        Private n_ORGN_id As Integer
        Private n_ORGN_RagioneSociale As String
        Private n_ORGN_Indirizzo As String
        Private n_ORGN_Cap As String
        Private n_ORGN_Citta As String
		Private n_ORGN_PRVN As New Provincia
		Private n_ORGN_Telefono1 As String
		Private n_ORGN_Telefono2 As String
		Private n_ORGN_Fax As String
		Private n_ORGN_HomePage As String
		Private n_ORGN_STTO As New COL_Stato
		Private n_ORGN_Logo As String
		Private n_ORGN_isChiusa As Boolean
		Private n_ORGN_isFacolta As Integer

		Protected n_CMNT_Bloccata As Integer
		Protected n_CMNT_Archiviata As Integer

		Private n_Errore As Errori_Db

		Protected n_CMNT_AccessoLibero As Integer
		Protected n_CMNT_AccessoCopisteria As Integer

#End Region

#Region "Public Property"
		Public Property Id() As Integer
			Get
				Id = n_ORGN_id
			End Get
			Set(ByVal Value As Integer)
				n_ORGN_id = Value
			End Set
		End Property
		Public Property RagioneSociale() As String
			Get
				RagioneSociale = n_ORGN_RagioneSociale
			End Get
			Set(ByVal Value As String)
				n_ORGN_RagioneSociale = Value
			End Set
		End Property
		Public Property Indirizzo() As String
			Get
				Indirizzo = n_ORGN_Indirizzo
			End Get
			Set(ByVal Value As String)
				n_ORGN_Indirizzo = Value
			End Set
		End Property
		Public Property Cap() As String
			Get
				Cap = n_ORGN_Cap
			End Get
			Set(ByVal Value As String)
				n_ORGN_Cap = Value
			End Set
		End Property
		Public Property Citta() As String
			Get
				Citta = n_ORGN_Citta
			End Get
			Set(ByVal Value As String)
				n_ORGN_Citta = Value
			End Set
		End Property
		Public Property Provincia() As Provincia
			Get
				Provincia = n_ORGN_PRVN
			End Get
			Set(ByVal Value As Provincia)
				n_ORGN_PRVN = Value
			End Set
		End Property
		Public Property Telefono1() As String
			Get
				Telefono1 = n_ORGN_Telefono1
			End Get
			Set(ByVal Value As String)
				n_ORGN_Telefono1 = Value
			End Set
		End Property
		Public Property Telefono2() As String
			Get
				Telefono2 = n_ORGN_Telefono2
			End Get
			Set(ByVal Value As String)
				n_ORGN_Telefono2 = Value
			End Set
		End Property
		Public Property Fax() As String
			Get
				Fax = n_ORGN_Fax
			End Get
			Set(ByVal Value As String)
				n_ORGN_Fax = Value
			End Set
		End Property
		Public Property HomePage() As String
			Get
				HomePage = n_ORGN_HomePage
			End Get
			Set(ByVal Value As String)
				n_ORGN_HomePage = Value
			End Set
		End Property
		Public Property Stato() As COL_Stato
			Get
				Stato = n_ORGN_STTO
			End Get
			Set(ByVal Value As COL_Stato)
				n_ORGN_STTO = Value
			End Set
		End Property
		Public Property IsChiusa() As Boolean
			Get
				IsChiusa = n_ORGN_isChiusa
			End Get
			Set(ByVal Value As Boolean)
				n_ORGN_isChiusa = Value
			End Set
		End Property
		Public ReadOnly Property Errore() As Errori_Db
			Get
				Errore = n_Errore
			End Get
		End Property
		Public Property Logo() As String
			Get
				Logo = n_ORGN_Logo
			End Get
			Set(ByVal Value As String)
				n_ORGN_Logo = Value
			End Set
		End Property

		Public Property Bloccata() As Boolean
			Get
				Bloccata = (Me.n_CMNT_Bloccata = 1)
			End Get
			Set(ByVal Value As Boolean)
				If Value Then
					n_CMNT_Bloccata = 1
				Else
					n_CMNT_Bloccata = 0
				End If
			End Set
		End Property
		Public Property Archiviata() As Boolean
			Get
				Archiviata = (Me.n_CMNT_Archiviata = 1)
			End Get
			Set(ByVal Value As Boolean)
				If Value Then
					n_CMNT_Archiviata = 1
				Else
					n_CMNT_Archiviata = 0
				End If
			End Set
		End Property

		Public Property IsFacolta() As Boolean
			Get
				IsFacolta = (n_ORGN_isFacolta = 1)
			End Get
			Set(ByVal Value As Boolean)
				If Value Then
					n_ORGN_isFacolta = 1
				Else
					n_ORGN_isFacolta = 0
				End If

			End Set
		End Property

		Public Property HasAccessoCopisteria() As Boolean
			Get
				HasAccessoCopisteria = (Me.n_CMNT_AccessoCopisteria = 1)
			End Get
			Set(ByVal Value As Boolean)
				If Value Then
					n_CMNT_AccessoCopisteria = 1
				Else
					n_CMNT_AccessoCopisteria = 0
				End If
			End Set
		End Property
		Public Property HasAccessoLibero() As Boolean
			Get
				HasAccessoLibero = (Me.n_CMNT_AccessoLibero = 1)
			End Get
			Set(ByVal Value As Boolean)
				If Value Then
					n_CMNT_AccessoLibero = 1
				Else
					n_CMNT_AccessoLibero = 0
				End If
			End Set
		End Property

#End Region

#Region "Metodi New"
        Sub New()
            Me.n_CMNT_Archiviata = 0
            Me.n_CMNT_Bloccata = 0
            Me.n_ORGN_isFacolta = 0

            Me.n_CMNT_AccessoCopisteria = 1
            Me.n_CMNT_AccessoLibero = 0

            Me.n_Errore = Errori_Db.None
        End Sub
        Sub New(ByVal OrganizzazioneID As Integer)
            Me.n_CMNT_Archiviata = 0
            Me.n_CMNT_Bloccata = 0
            Me.n_ORGN_isFacolta = 0

            Me.n_CMNT_AccessoCopisteria = 1
            Me.n_CMNT_AccessoLibero = 0
            Me.n_ORGN_id = OrganizzazioneID
            Me.n_Errore = Errori_Db.None
		End Sub
		Sub New(ByVal OrganizzazioneID As Integer, ByVal Nome As String)
			Me.n_CMNT_Archiviata = 0
			Me.n_CMNT_Bloccata = 0
			Me.n_ORGN_isFacolta = 0

			Me.n_CMNT_AccessoCopisteria = 1
			Me.n_CMNT_AccessoLibero = 0
			Me.n_ORGN_id = OrganizzazioneID
			Me.n_ORGN_RagioneSociale = Nome
			Me.n_Errore = Errori_Db.None
		End Sub
		Sub New(ByVal OrganizzazioneID As Integer, ByVal Nome As String, ByVal isFacolta As Boolean, ByVal Logo As String, ByVal Web As String)
			Me.n_CMNT_Archiviata = 0
			Me.n_CMNT_Bloccata = 0
			If isFacolta Then
				Me.n_ORGN_isFacolta = 1
			Else
				Me.n_ORGN_isFacolta = 0
			End If

			Me.n_CMNT_AccessoCopisteria = 1
			Me.n_CMNT_AccessoLibero = 0
			Me.n_ORGN_id = OrganizzazioneID
			Me.n_ORGN_RagioneSociale = Nome
			Me.n_ORGN_HomePage = Web
			Me.n_ORGN_Logo = Logo
			Me.n_Errore = Errori_Db.None
		End Sub
#End Region

#Region "Metodi Standard"
        Public Function Elenca(Optional ByVal byIstituzione As Integer = 0) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Organizzazione_Elenca"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@ISTT_id", byIstituzione, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                dsTable = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try
            Return dsTable
        End Function
        'borsi
        Public Sub Modifica(ByVal idIstituzione As Integer, ByVal MDCM_default As Integer) ', Optional ByVal ComunitaIsAccessoLibero As Boolean = False, Optional ByVal ComunitaIsAccessoCopistaria As Boolean = True)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Organizzazione_Modifica"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@ORGN_Id", n_ORGN_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_RagioneSociale", n_ORGN_RagioneSociale.Trim, ParameterDirection.Input, SqlDbType.VarChar, , 200)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_Indirizzo", n_ORGN_Indirizzo.Trim, ParameterDirection.Input, SqlDbType.VarChar, , 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_Cap", n_ORGN_Cap.Trim, ParameterDirection.Input, SqlDbType.VarChar, , 5)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_Citta", n_ORGN_Citta.Trim, ParameterDirection.Input, SqlDbType.VarChar, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_PRVN_ID", n_ORGN_PRVN.Id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_STTO_ID", n_ORGN_STTO.Id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_Telefono1", n_ORGN_Telefono1.Trim, ParameterDirection.Input, SqlDbType.VarChar, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_Telefono2", n_ORGN_Telefono2.Trim, ParameterDirection.Input, SqlDbType.VarChar, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_Fax", n_ORGN_Fax.Trim, ParameterDirection.Input, SqlDbType.VarChar, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_HomePage", n_ORGN_HomePage.Trim, ParameterDirection.Input, SqlDbType.VarChar, , 250)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ORGN_isChiusa", n_ORGN_isChiusa, , DbType.Boolean)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_Logo", n_ORGN_Logo, ParameterDirection.Input, SqlDbType.VarChar, , 300)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_isFacolta", Me.n_ORGN_isFacolta, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ISTT_id", idIstituzione, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
				objAccesso.GetExecuteNotQuery(oRequest)

				ObjectBase.PurgeCacheItems(CachePolicy.Organizzazione)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBChange
            End Try
            If Me.n_Errore = Errori_Db.None Then
                ModificaComunita(MDCM_default, n_ORGN_id) ', ComunitaIsAccessoLibero, ComunitaIsAccessoCopistaria)
            End If
            ObjectBase.PurgeCacheItems(CachePolicy.OrganizzazioneList)

        End Sub

        'restituisce l'ID della comunità
        Public Function Aggiungi(ByVal ISTT_id As Integer, ByVal MDCM_default As Integer, ByVal IdCreatore As Integer, ByVal Iscrivi As Boolean, ByVal MaxIscritti As Integer) As Integer ', Optional ByVal Archiviata As Boolean = False, Optional ByVal bloccata As Boolean = False, Optional ByVal IsAccessoLibero As Boolean = False, Optional ByVal IsAccessoCopisteria As Boolean = True) As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim idcmnt As Integer

            With oRequest
                .Command = "sp_Organizzazione_Aggiungi"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@ORGN_Id", " ", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_RagioneSociale", n_ORGN_RagioneSociale.Trim, ParameterDirection.Input, SqlDbType.VarChar, , 200)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_Indirizzo", n_ORGN_Indirizzo, ParameterDirection.Input, SqlDbType.VarChar, , 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_Cap", n_ORGN_Cap, ParameterDirection.Input, SqlDbType.VarChar, , 5)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_Citta", n_ORGN_Citta, ParameterDirection.Input, SqlDbType.VarChar, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_PRVN_ID", n_ORGN_PRVN.Id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_STTO_ID", n_ORGN_STTO.Id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_Telefono1", n_ORGN_Telefono1, ParameterDirection.Input, SqlDbType.VarChar, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_Telefono2", n_ORGN_Telefono2, ParameterDirection.Input, SqlDbType.VarChar, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_Fax", n_ORGN_Fax, ParameterDirection.Input, SqlDbType.VarChar, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_HomePage", n_ORGN_HomePage, ParameterDirection.Input, SqlDbType.VarChar, , 250)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ORGN_isChiusa", n_ORGN_isChiusa, , DbType.Boolean)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_ISTT_id", ISTT_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_ID", IdCreatore, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_Logo", n_ORGN_Logo, ParameterDirection.Input, SqlDbType.VarChar, , 300)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_isFacolta", Me.n_ORGN_isFacolta, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                'manca il tipo di autenticazione preferito per quel tipo di comunità
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                ' Recupero l'ID univoco dell'organizzazione.
				Me.n_ORGN_id = oRequest.GetValueFromParameter(1)
                ObjectBase.PurgeCacheItems(CachePolicy.Organizzazione)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBInsert
                Me.n_ORGN_id = -1
            End Try
            If Me.n_ORGN_id > 0 Then
                idcmnt = AggiungiComunita(MDCM_default, IdCreatore, Iscrivi, MaxIscritti, Me.n_CMNT_Archiviata, Me.n_CMNT_Bloccata, , Me.n_CMNT_AccessoLibero, Me.n_CMNT_AccessoCopisteria)
            Else
                idcmnt = -1
            End If
            ObjectBase.PurgeCacheItems(CachePolicy.OrganizzazioneList)
            Return idcmnt
        End Function

        Public Function Elimina() As Integer
            'cancella l'organizzazione e la comunità di livello zero relativa
            'attenzione!!!
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim Codice As Integer

            With oRequest
                .CommandType = CommandType.StoredProcedure
                oRequest.Command = "sp_Organizzazione_Elimina"
                oParam = objAccesso.GetParameter("@ORGN_Id", n_ORGN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Codice", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
				Codice = oRequest.GetValueFromParameter(2)
				ObjectBase.PurgeCacheItems(CachePolicy.Organizzazione)
            Catch
                Me.n_Errore = Errori_Db.DBError
                Codice = -3
            End Try
            ObjectBase.PurgeCacheItems(CachePolicy.OrganizzazioneList)
            Return Codice
        End Function

        Public Sub Estrai()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim oIstituzione As New COL_Istituzione

            With oRequest
                .Command = "sp_Organizzazione_estrai"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@ORGN_id", Id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_RagioneSociale", "", ParameterDirection.Output, SqlDbType.VarChar, , 200)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_Indirizzo", "", ParameterDirection.Output, SqlDbType.VarChar, , 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_Cap", "", ParameterDirection.Output, SqlDbType.VarChar, , 5)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_Citta", "", ParameterDirection.Output, SqlDbType.VarChar, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_PRVN", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_STTO", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_Telefono1", "", ParameterDirection.Output, SqlDbType.VarChar, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_Telefono2", "", ParameterDirection.Output, SqlDbType.VarChar, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_Fax", "", ParameterDirection.Output, SqlDbType.VarChar, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_HomePage", "", ParameterDirection.Output, SqlDbType.VarChar, , 250)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ORGN_isChiusa", "", ParameterDirection.Output, DbType.Boolean)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_Logo", "", ParameterDirection.Output, SqlDbType.VarChar, , 300)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ORGN_isFacolta", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@CMNT_Archiviata", "", ParameterDirection.Output, DbType.Int32, False)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_Bloccata", "", ParameterDirection.Output, DbType.Int32, False)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_AccessoLibero", "", ParameterDirection.Output, DbType.Int32, False)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_AccessoCopisteria", "", ParameterDirection.Output, DbType.Int32, False)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)

                Me.n_ORGN_RagioneSociale = oRequest.GetValueFromParameter(2)
                Me.n_ORGN_Indirizzo = oRequest.GetValueFromParameter(3)
                Me.n_ORGN_Cap = oRequest.GetValueFromParameter(4)
                Me.n_ORGN_Citta = oRequest.GetValueFromParameter(5)
                Me.n_ORGN_PRVN.Id = oRequest.GetValueFromParameter(6)
                Me.n_ORGN_STTO.Id = oRequest.GetValueFromParameter(7)
                Me.n_ORGN_Telefono1 = oRequest.GetValueFromParameter(8)
                Me.n_ORGN_Telefono2 = oRequest.GetValueFromParameter(9)
                Me.n_ORGN_Fax = oRequest.GetValueFromParameter(10)
                Me.n_ORGN_HomePage = oRequest.GetValueFromParameter(11)
                Me.n_ORGN_isChiusa = oRequest.GetValueFromParameter(12)
                Me.n_ORGN_Logo = oRequest.GetValueFromParameter(13)
                Try
                    Me.n_ORGN_isFacolta = oRequest.GetValueFromParameter(14)
                Catch ex As Exception
                    Me.n_ORGN_isFacolta = 0
                End Try

                Me.n_CMNT_Archiviata = oRequest.GetValueFromParameter(15)
                Me.n_CMNT_Bloccata = oRequest.GetValueFromParameter(16)

                Me.n_CMNT_AccessoLibero = oRequest.GetValueFromParameter(17)
                Me.n_CMNT_AccessoCopisteria = oRequest.GetValueFromParameter(18)
                ' oIstituzione.Id = oRequest.GetValueFromParameter(13)

            Catch ax As Exception
                Me.n_Errore = Errori_Db.DBReadExist
            End Try
        End Sub

#End Region

        Public Shared Function GetComunitaID(ByVal OrganizzazioneId As Integer) As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim iResponse As Integer = 0
            With oRequest
                .Command = "sp_Organizzazione_GetComunitaID"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@ORGN_ID", OrganizzazioneId, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@ComunitaID", 0, ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                iResponse = oRequest.GetValueFromParameter(2)
            Catch ex As Exception

            End Try
            Return iResponse
        End Function
        Public Sub AssociaLogo(Optional ByVal newpath As String = Nothing)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Organizzazione_AssociaLogo"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@ORGN_ID", Me.n_ORGN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ORGN_Logo", newpath, ParameterDirection.Input, DbType.String, True, 300)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
				Me.Logo = newpath
				ObjectBase.PurgeCacheItems(CachePolicy.Organizzazione)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBReadExist
            End Try
        End Sub

#Region "Gestione Iscrizioni autonome"
        Public Shared Function getIscrizioniAutonome(ByVal OrganizzazioneID As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataSet As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Organizzazione_getIscrizioniAutonome"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@ORGN_Id", OrganizzazioneID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                oDataSet = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception

            End Try
            Return oDataSet
        End Function
        Public Shared Function AssociaTipologiaPersona(ByVal OrganizzazioneID As Integer, ByVal TipoPersonaID As Integer) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Organizzazione_AssociaTipologiaPersona"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@ORGN_Id", OrganizzazioneID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@TPPR_ID", TipoPersonaID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                iResponse = objAccesso.GetExecuteNotQuery(oRequest)
                Return True
            Catch ex As Exception
                Return False
            End Try
            Return False
        End Function
        Public Shared Function DeAssociaTipologiaPersona(ByVal OrganizzazioneID As Integer, ByVal TipoPersonaID As Integer) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Organizzazione_DeAssociaTipologiaPersona"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@ORGN_Id", OrganizzazioneID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@TPPR_ID", TipoPersonaID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                iResponse = objAccesso.GetExecuteNotQuery(oRequest)
                Return True
            Catch ex As Exception
                Return False
            End Try
            Return False
        End Function

#End Region
        '**** Benali ****
        Public Function PercorsiAssociati() As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Organizzazione_PercorsiAssociati"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@ORGN_Id", Me.Id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                dsTable = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try
            Return dsTable
        End Function
        '**** Fine Benali ****
        'borsi
        Protected Sub AssociaIstituzione(ByVal ISTT_id As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Organizzazione_AssociaIstituzione"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@ISTT_Id", ISTT_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ORGN_Id", Me.n_ORGN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBInsert
            End Try
        End Sub
        'borsi
        Protected Function AggiungiComunita(ByVal MDCM_default As Integer, ByVal IdCreatore As Integer, ByVal Iscrivi As Boolean, ByVal MaxIscritti As Integer, ByVal ComunitaArchiviata As Boolean, ByVal ComunitaBloccata As Boolean, Optional ByVal SmsRicezione As Integer = 0, Optional ByVal ComunitaIsAccessoLibero As Boolean = False, Optional ByVal ComunitaIsAccessoCopisteria As Boolean = False) As Integer

            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim DataCreazione As String
            DataCreazione = (DateToString(Now, True))
            '   Me.n_CMNT_datacreazione = DataCreazione
            With oRequest
                .Command = "sp_Comunita_Aggiungi"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_idpadre", "0", ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_nome", Me.n_ORGN_RagioneSociale, ParameterDirection.Input, DbType.String)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_datacreazione", DataCreazione, ParameterDirection.Input, DbType.String, , 30)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_statuto", "Comunità di livello zero", ParameterDirection.Input, DbType.String)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_TPCM_id", "0", ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_MDCM_id", MDCM_default, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_ORGN_id", Me.n_ORGN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_ischiusa", Me.n_ORGN_isChiusa, ParameterDirection.Input, DbType.Boolean)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Creatore", IdCreatore, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@SMS_Ricezione", SmsRicezione, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_PRSN_ID", IdCreatore, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_tpcm_icona", "", ParameterDirection.Output, DbType.String, True, 100)
                .Parameters.Add(oParam)
                If Iscrivi Then
                    oParam = objAccesso.GetParameter("@Iscrivi", 1, ParameterDirection.Input, DbType.Int32)
                Else
                    oParam = objAccesso.GetParameter("@Iscrivi", 0, ParameterDirection.Input, DbType.Int32)
                End If
                .Parameters.Add(oParam)

                If ComunitaArchiviata Then
                    oParam = objAccesso.GetParameter("@CMNT_Archiviata", 1, ParameterDirection.Input, DbType.Int32, False)
                Else
                    oParam = objAccesso.GetParameter("@CMNT_Archiviata", 0, ParameterDirection.Input, DbType.Int32, False)
                End If
                .Parameters.Add(oParam)

                If ComunitaBloccata Then
                    oParam = objAccesso.GetParameter("@CMNT_Bloccata", 1, ParameterDirection.Input, DbType.Int32, False)
                Else
                    oParam = objAccesso.GetParameter("@CMNT_Bloccata", 0, ParameterDirection.Input, DbType.Int32, False)
                End If
                .Parameters.Add(oParam)

                If ComunitaIsAccessoLibero Then
                    oParam = objAccesso.GetParameter("@CMNT_AccessoLibero", 1, ParameterDirection.Input, DbType.Int32, False)
                Else
                    oParam = objAccesso.GetParameter("@CMNT_AccessoLibero", 0, ParameterDirection.Input, DbType.Int32, False)
                End If
                .Parameters.Add(oParam)

                If ComunitaIsAccessoCopisteria Then
                    oParam = objAccesso.GetParameter("@CMNT_AccessoCopisteria", 1, ParameterDirection.Input, DbType.Int32, False)
                Else
                    oParam = objAccesso.GetParameter("@CMNT_AccessoCopisteria", 0, ParameterDirection.Input, DbType.Int32, False)
                End If
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Dim idcmnt As Integer
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                idcmnt = oRequest.GetValueFromParameter(1)

                ObjectBase.PurgeCacheItems(CachePolicy.ListaIScritti(idcmnt))
                ObjectBase.PurgeCacheItems(CachePolicy.Bacheca)
                ObjectBase.PurgeCacheItems(CachePolicy.MenuComunita(idcmnt))
                ObjectBase.PurgeCacheItems(CachePolicy.RenderCommunity(idcmnt))
                ObjectBase.PurgeCacheItems(CachePolicy.PermessiServizioUtente)
                ObjectBase.PurgeCacheItems(CachePolicy.PlainIscrizioni(IdCreatore))
                ObjectBase.PurgeCacheItems(CachePolicy.RuoliServizioComunita(idcmnt))
                ObjectBase.PurgeCacheItems(CachePolicy.ServiziComunita(idcmnt))
            Catch ex As Exception
                ' se nn è avvenuto l'inserimento setta a -1 l'id della comunità
                ' Me.n_CMNT_id = -1
                n_Errore = Errori_Db.DBInsert
            End Try
            Return idcmnt
        End Function
        'borsi
        Public Function ElencaByIstituzione(ByVal ISTT_id As Integer) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Organizzazione_ElencaByIstituzione"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@ISTT_id", ISTT_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                dsTable = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try
            Return dsTable
        End Function

        Protected Sub ModificaComunita(ByVal MDCM_default As Integer, ByVal ORGN_id As Integer) ', ByVal ComunitaIsAccessoLibero As Boolean, ByVal ComunitaIsAccessoCopisteria As Boolean)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Organizzazione_ModificaComunita"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_ORGN_id", ORGN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_nome", Me.n_ORGN_RagioneSociale, ParameterDirection.Input, DbType.String, , 200)
                .Parameters.Add(oParam)
                'oParam = objAccesso.GetParameter("@CMNT_datacreazione", DataCreazione, ParameterDirection.Input, DbType.String, , 30)
                '.Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_MDCM_id", MDCM_default, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_ischiusa", Me.n_ORGN_isChiusa, ParameterDirection.Input, DbType.Boolean)
                .Parameters.Add(oParam)

                If Me.HasAccessoLibero Then
                    oParam = objAccesso.GetParameter("@CMNT_AccessoLibero", 1, ParameterDirection.Input, DbType.Int32, False)
                Else
                    oParam = objAccesso.GetParameter("@CMNT_AccessoLibero", 0, ParameterDirection.Input, DbType.Int32, False)
                End If
                .Parameters.Add(oParam)

                If Me.HasAccessoCopisteria Then
                    oParam = objAccesso.GetParameter("@CMNT_AccessoCopisteria", 1, ParameterDirection.Input, DbType.Int32, False)
                Else
                    oParam = objAccesso.GetParameter("@CMNT_AccessoCopisteria", 0, ParameterDirection.Input, DbType.Int32, False)
                End If
                .Parameters.Add(oParam)


                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                'assegna all'oggetto l'id della comunità appena creata
                '  Me.n_CMNT_id = oRequest.GetValueFromParameter(1)
            Catch
                ' se nn è avvenuto l'inserimento setta a -1 l'id della comunità
                ' Me.n_CMNT_id = -1
                n_Errore = Errori_Db.DBChange
            End Try
        End Sub

        Public Function RitornaComunitaOrganizzazione() As Integer
            'restituisce l'id della comunità padre che corrisponde alla super-comunità organizzazione
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim IdComunita As Integer
            With oRequest
                .Command = "sp_Organizzazione_SuperComunitaCorrispondente"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@CMNT_Id", " ", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ORGN_Id", Me.n_ORGN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                ' Recupero l'ID univoco dell'organizzazione.
                IdComunita = oRequest.GetValueFromParameter(1)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
                IdComunita = -1
            End Try
            Return IdComunita
        End Function

        Public Function GetIstituzione() As Integer
            'restituisce l'id dell'istituzione di cui l'organizzazione fa parte
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim IdIstituzione As Integer
            With oRequest
                .Command = "sp_Organizzazione_GetIstituzione"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@ISTT_Id", " ", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ORGN_Id", Me.n_ORGN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                ' Recupero l'ID univoco dell'organizzazione.
                IdIstituzione = oRequest.GetValueFromParameter(1)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
                IdIstituzione = -1
            End Try
            Return IdIstituzione
        End Function

        Public Sub OrganizzazioneDefaultByPersona(ByVal IdPersona As Integer)
            'restituisce l'id dell'organizzazione di default di una persona
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_OrganizzazioneDefaultByPersona"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@ORGN_Id", " ", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Id", IdPersona, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ORGN_ragioneSociale", "", ParameterDirection.Output, DbType.String, , 200)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                ' Recupero l'ID univoco dell'organizzazione.
                Me.n_ORGN_id = oRequest.GetValueFromParameter(1)
                'e la sua ragione sociale
                Me.RagioneSociale = oRequest.GetValueFromParameter(3)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
                Me.n_ORGN_id = -1
            End Try
        End Sub

        Public Function ElencaNonIscrittiByIstituzione(ByVal LinguaID As Integer, ByVal ISTT_id As Integer, ByVal TipoPersona As Integer, Optional ByVal valore As String = "", Optional ByVal oAnagrafica As FiltroAnagrafica = FiltroAnagrafica.tutti, Optional ByVal oFiltroRicerca As Main.FiltroRicercaAnagrafica = Main.FiltroRicercaAnagrafica.tutti) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Organizzazione_ElencaNonIscrittiByIstituzione"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@ISTT_id", ISTT_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ORGN_id", Me.n_ORGN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPPR_id", TipoPersona, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@valore", valore, ParameterDirection.Input, DbType.String)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@anagrafica", CType(oAnagrafica, FiltroAnagrafica), ParameterDirection.Input, DbType.String, , 3)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LinguaID", LinguaID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@FiltroRicerca", CType(oFiltroRicerca, FiltroRicercaAnagrafica), ParameterDirection.Input, DbType.String, , 3)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                dsTable = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try
            Return dsTable
        End Function

#Region "Associa/Dissasocia Autenticazione"
        Public Function ElencaAutenticazioni() As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim oDataset As New DataSet

            With oRequest
                .Command = "sp_Organizzazione_ElencaAutenticazioni"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@ORGN_id", Me.n_ORGN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                oDataset = objAccesso.GetdataSet(oRequest)
                n_Errore = Errori_Db.None
            Catch
                n_Errore = Errori_Db.DBError
            End Try
            Return oDataset
        End Function

        Public Function AssociaAutenticazione(ByVal AUTH_ID As Integer, ByVal descrizione As String, Optional ByVal Parametro1 As String = "", Optional ByVal Parametro2 As String = "", Optional ByVal Parametro3 As String = "", Optional ByVal Parametro4 As String = "") As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim iResponse As Integer

            With oRequest
                .Command = "sp_Organizzazione_AssociaAutenticazione"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@ORGN_id", Me.n_ORGN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@LKAO_AUTN_ID", AUTH_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@LKAO_Descrizione", descrizione, ParameterDirection.Input, DbType.String, , 250)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@LKAO_Parametro_1", Parametro1, ParameterDirection.Input, DbType.String, , 150)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@LKAO_Parametro_2", Parametro2, ParameterDirection.Input, DbType.String, , 150)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@LKAO_Parametro_3", Parametro3, ParameterDirection.Input, DbType.String, , 150)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@LKAO_Parametro_4", Parametro4, ParameterDirection.Input, DbType.String, , 150)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                iResponse = objAccesso.GetExecuteNotQuery(oRequest)
                n_Errore = Errori_Db.None
            Catch
                n_Errore = Errori_Db.DBInsert
                Return False
            End Try
        End Function

        Public Function EstraiDatiAutenticazione(ByVal LKAO_ID As Integer, ByRef AUTH_ID As Integer, ByRef descrizione As String, ByRef Parametro1 As String, ByRef Parametro2 As String, ByRef Parametro3 As String, ByRef Parametro4 As String) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim iResponse As Integer

            With oRequest
                .Command = "sp_Organizzazione_EstraiDatiAutenticazione"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@LKAO_ID", LKAO_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@LKAO_AUTN_ID", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@LKAO_Descrizione", "", ParameterDirection.Output, DbType.String, , 250)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@LKAO_Parametro_1", "", ParameterDirection.Output, DbType.String, , 150)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@LKAO_Parametro_2", "", ParameterDirection.Output, DbType.String, , 150)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@LKAO_Parametro_3", "", ParameterDirection.Output, DbType.String, , 150)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@LKAO_Parametro_4", "", ParameterDirection.Output, DbType.String, , 150)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                iResponse = objAccesso.GetExecuteNotQuery(oRequest)
                AUTH_ID = oRequest.GetValueFromParameter(2)
                descrizione = oRequest.GetValueFromParameter(3)
                Parametro1 = oRequest.GetValueFromParameter(4)
                Parametro2 = oRequest.GetValueFromParameter(5)
                Parametro3 = oRequest.GetValueFromParameter(6)
                Parametro4 = oRequest.GetValueFromParameter(7)
                n_Errore = Errori_Db.None
            Catch
                n_Errore = Errori_Db.DBReadExist
                Return False
            End Try
        End Function
        Public Function ModificaDatiAutenticazione(ByVal UniqueID As Integer, ByVal AUTH_ID As Integer, ByVal descrizione As String, Optional ByVal Parametro1 As String = "", Optional ByVal Parametro2 As String = "", Optional ByVal Parametro3 As String = "", Optional ByVal Parametro4 As String = "") As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim iResponse As Integer

            With oRequest
                .Command = "sp_Organizzazione_ModificaDatiAutenticazione"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@ORGN_id", Me.n_ORGN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@UniqueID", UniqueID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@LKAO_AUTN_ID", AUTH_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@LKAO_Descrizione", descrizione, ParameterDirection.Input, DbType.String, , 250)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@LKAO_Parametro_1", Parametro1, ParameterDirection.Input, DbType.String, , 150)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@LKAO_Parametro_2", Parametro2, ParameterDirection.Input, DbType.String, , 150)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@LKAO_Parametro_3", Parametro3, ParameterDirection.Input, DbType.String, , 150)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@LKAO_Parametro_4", Parametro4, ParameterDirection.Input, DbType.String, , 150)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                iResponse = objAccesso.GetExecuteNotQuery(oRequest)
                n_Errore = Errori_Db.None
            Catch
                n_Errore = Errori_Db.DBInsert
                Return False
            End Try
        End Function

        Public Function DisAssociaAutenticazione(ByVal AUTN_ID As Integer, ByVal UniqueID As Integer) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim iResponse As Boolean = False

            With oRequest
                .Command = "sp_Organizzazione_DisAssociaAutenticazione"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@ORGN_id", Me.n_ORGN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@AUTN_ID", AUTN_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@UniqueID", UniqueID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                iResponse = objAccesso.GetExecuteNotQuery(oRequest)
                n_Errore = Errori_Db.None
            Catch
                n_Errore = Errori_Db.DBDelete
            End Try
        End Function
        Public Function DisAssociaAutenticazioni() As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim iResponse As Boolean = False

            With oRequest
                .Command = "sp_Organizzazione_DisAssociaAutenticazioni"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@ORGN_id", Me.n_ORGN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                iResponse = objAccesso.GetExecuteNotQuery(oRequest)
                n_Errore = Errori_Db.None
            Catch
                n_Errore = Errori_Db.DBDelete
            End Try
        End Function

        Public Function ElencaTipiAutenticazioneAssociati() As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim oDataset As New DataSet

            With oRequest
                .Command = "sp_Organizzazione_ElencaTipiAutenticazione"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@ORGN_id", Me.n_ORGN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                oDataset = objAccesso.GetdataSet(oRequest)
                n_Errore = Errori_Db.None
            Catch
                n_Errore = Errori_Db.DBError
            End Try
            Return oDataset
        End Function

        Public Function ElencaTipiAutenticazioneDisponibili() As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim oDataset As New DataSet

            With oRequest
                .Command = "sp_Organizzazione_ElencaTipiAutenticazioneDisponibili"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@ORGN_id", Me.n_ORGN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                oDataset = objAccesso.GetdataSet(oRequest)
                n_Errore = Errori_Db.None
            Catch
                n_Errore = Errori_Db.DBError
            End Try
            Return oDataset
        End Function
#End Region


        Public Function GetTotaleIscritti(ByVal EsclusaPersonaID As Integer, ByVal oFiltroAbilitazione As Main.FiltroAbilitazione) As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim NumeroIscritti As Integer
            Dim objAccesso As New COL_DataAccess
            Dim oDataset As New DataSet

            With oRequest
                .Command = "sp_Organizzazione_TotaleIscritti"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@ORGN_ID", Me.n_ORGN_id, ParameterDirection.Input, DbType.Int32, True)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@EsclusaPersonaID", EsclusaPersonaID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@TPPR_ID", 0, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@abilitazione", CType(oFiltroAbilitazione, Main.FiltroAbilitazione), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                oDataset = objAccesso.GetdataSet(oRequest)
                If oDataset.Tables(0).Rows.Count = 1 Then
                    NumeroIscritti = oDataset.Tables(0).Rows(0).Item("Totale")
                Else
                    NumeroIscritti = 0 '-1
                End If

            Catch ex As Exception
                n_Errore = Errori_Db.DBReadExist
                NumeroIscritti = 0 '-1
            End Try
            Return NumeroIscritti
        End Function
        Public Function GetTipiPersonaAssociati(ByVal LinguaID As Integer, ByVal EsclusaPersonaID As Integer, ByVal oFiltroAbilitazione As Main.FiltroAbilitazione, Optional ByVal EsclusiGuest As Boolean = True) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dstable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Organizzazione_TipiPersonaAssociati"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@ORGN_ID", Me.n_ORGN_id, ParameterDirection.Input, SqlDbType.Int, True)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@EsclusaPersonaID", EsclusaPersonaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@filtroAbilitazione", CType(oFiltroAbilitazione, Main.FiltroAbilitazione), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                If EsclusiGuest Then
                    oParam = objAccesso.GetAdvancedParameter("@EsclusiGuest", 1, ParameterDirection.Input, SqlDbType.Int)
                Else
                    oParam = objAccesso.GetAdvancedParameter("@EsclusiGuest", 0, ParameterDirection.Input, SqlDbType.Int)
                End If
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                dstable = objAccesso.GetdataSet(oRequest)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try
            Return dstable
        End Function
        Public Shared Function GetIscrittiByTipoPersona(ByVal ORGN_ID As Integer, ByVal TPPR_ID As Integer, ByVal oFiltroAbilitazione As Abilitazione, Optional ByVal EsclusaPersonaID As Integer = -1) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dstable As New DataSet
            Dim objAccesso As New COL_DataAccess


            With oRequest
                .Command = "sp_Organizzazione_IscrittiByTipoPersona"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@ORGN_ID", ORGN_ID, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPPR_ID", TPPR_ID, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@filtroAbilitazione", CType(oFiltroAbilitazione, Main.FiltroAbilitazione), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@EsclusaPersonaID", EsclusaPersonaID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                dstable = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception

            End Try
            Return dstable
        End Function
        Public Shared Function GetIscrittiEsclusiTipoPersona(ByVal TipoPersonaEsclusi As String, ByVal OrganizzazioneID As Integer, ByVal oFiltroAbilitazione As Abilitazione, Optional ByVal EsclusaPersonaID As Integer = -1, Optional ByVal oFiltro As Main.FiltroElencoTipiPersona = Main.FiltroElencoTipiPersona.All_NoGuest) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dstable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Organizzazione_IscrittiEsclusiTipoPersona"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@ORGN_ID", OrganizzazioneID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@TipoPersonaEsclusi", TipoPersonaEsclusi, ParameterDirection.Input, SqlDbType.VarChar, True, 4000)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@filtroAbilitazione", CType(oFiltroAbilitazione, Main.FiltroAbilitazione), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@EsclusaPersonaID", EsclusaPersonaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@Filtro", CType(oFiltro, Main.FiltroAbilitazione), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False

            End With
            Try
                dstable = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception

            End Try
            Return dstable
        End Function


		Private Shared Function RetrieveDataFromDBForPresenter(Optional ByVal byIstituzione As Integer = 0) As List(Of COL_Organizzazione)
			Dim oLista As New List(Of COL_Organizzazione)

			Dim oRequest As New COL_Request
			Dim oParam As New COL_Request.Parameter
			Dim objAccesso As New COL_DataAccess
			Dim oDatareader As IDataReader

			With oRequest
				.Command = "sp_Organizzazione_Elenca"
				.CommandType = CommandType.StoredProcedure

				oParam = objAccesso.GetAdvancedParameter("@ISTT_id", byIstituzione, ParameterDirection.Input, SqlDbType.Int)
				.Parameters.Add(oParam)

				.Role = COL_Request.UserRole.Admin
				.transactional = False
			End With

			Try
				oDatareader = objAccesso.GetdataReader(oRequest)
				While oDatareader.Read
					oLista.Add(New COL_Organizzazione(oDatareader("ORGN_id"), GenericValidator.ValString(oDatareader("ORGN_ragioneSociale"), ""), _
					GenericValidator.ValBool(oDatareader("ORGN_isFacolta"), False), GenericValidator.ValString(oDatareader("ORGN_Logo"), ""), _
					GenericValidator.ValString(oDatareader("ORGN_homePage"), "")))
				End While
			Catch ex As Exception
			Finally
				If not isnothing(oDatareader) then
					if oDatareader.IsClosed = False Then
					oDatareader.Close()
					End If
				End If
			End Try
			Return oLista
		End Function

		Public Shared Function LazyElenca(ByVal PercorsoFisico As String, ByVal NoLogo As String, Optional ByVal byIstituzione As Integer = 0) As List(Of COL_Organizzazione)
			Dim oLista As New List(Of COL_Organizzazione)
			Dim cacheKey As String = CachePolicy.Organizzazione

			If ObjectBase.Cache(cacheKey) Is Nothing Then
				oLista = COL_Organizzazione.RetrieveDataFromDBForPresenter(byIstituzione)
				ObjectBase.Cache.Insert(cacheKey, oLista, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza30minuti)
			Else
				oLista = CType(ObjectBase.Cache(cacheKey), List(Of COL_Organizzazione))
			End If

			Dim i, totale As Integer

			totale = oLista.Count - 1
			For i = 0 To totale
				If oLista(i).Logo = String.Empty Or oLista(i).Logo = "" Then
					oLista(i).Logo = NoLogo
                ElseIf Not Exists.File(PercorsoFisico & oLista(i).Logo) Then
                    oLista(i).Logo = NoLogo
				End If
			Next
			Return oLista
		End Function

		Public Shared Function FindByID(ByVal item As COL_Organizzazione, ByVal argument As Integer) As Boolean
			Return item.ID = argument
        End Function

        Public Shared Function List() As List(Of COL_Organizzazione)
            Dim oLista As New List(Of COL_Organizzazione)
            Dim cacheKey As String = CachePolicy.OrganizzazioneList

            If ObjectBase.Cache(cacheKey) Is Nothing Then
                oLista = COL_Organizzazione.RetrieveDataFromDBForPresenter()
                ObjectBase.Cache.Insert(cacheKey, oLista, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza30minuti)
            Else
                oLista = CType(ObjectBase.Cache(cacheKey), List(Of COL_Organizzazione))
            End If

            Return oLista
        End Function
        Public Shared Function ListForSubscription() As List(Of COL_Organizzazione)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oLista As New List(Of COL_Organizzazione)
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_TipoPersona_ListOfOrganizationToSubscribe"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@IstituzioneID", -1, , DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Dim iReader As IDataReader = Nothing
            Try
                iReader = objAccesso.GetdataReader(oRequest)
                While iReader.Read
                    oLista.Add(New COL_Organizzazione() With {.Id = iReader("ORGN_id"), .RagioneSociale = GenericValidator.ValString(iReader("ORGN_ragioneSociale"), "")})
                End While
            Catch ex As Exception
                If Not IsNothing(iReader) Then
                    If iReader.IsClosed = False Then
                        iReader.Close()
                    End If
                    iReader.Dispose()
                End If
            End Try
            Return oLista
        End Function
	End Class

End Namespace