Imports COL_DataLayer
Imports COL_BusinessLogic_v2.Comunita

''' <summary>
''' Rappresenta i dati di una lista e contiene le funzioni per salvare e modificare
''' NON contiene nessun metodo per recuperare liste di indirizzi o elenchi di liste
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class MailingList
    Inherits ObjectBase
#Region "Property"
    Private _Id As Integer
    Private _ProprietarioId As Integer
    Private _Nome As String
    Private _IsSelected As Boolean

    Public Property Id() As Integer
        Get
            Return _Id
        End Get
        Set(ByVal value As Integer)
            _Id = value
        End Set
    End Property
    Public Property ProprietarioId() As Integer
        Get
            Return _ProprietarioId
        End Get
        Set(ByVal value As Integer)
            _ProprietarioId = value
        End Set
    End Property
    Public Property Nome() As String
        Get
            Return _Nome
        End Get
        Set(ByVal value As String)
            _Nome = value
        End Set
    End Property
    Public Property IsSelected() As Boolean
        Get
            Return _IsSelected
        End Get
        Set(ByVal value As Boolean)
            _IsSelected = value
        End Set
    End Property
#End Region
#Region "Costruttori"
    Public Sub New()
        Me._Id = -1
        Me._ProprietarioId = -1
        Me._Nome = ""
    End Sub
    Public Sub New(ByVal Id As Integer)
        Me._Id = Id
        Me._ProprietarioId = -1
        Me._Nome = ""
    End Sub
    Public Sub New( _
        ByVal ProprietarioId As Integer, _
        ByVal Nome As String)

        Me._Id = -1
        Me._ProprietarioId = ProprietarioId
        Me._Nome = Nome
    End Sub
    Public Sub New( _
        ByVal Id As Integer, _
        ByVal ProprietarioId As Integer, _
        ByVal Nome As String)

        Me._Id = Id
        Me._ProprietarioId = ProprietarioId
        Me._Nome = Nome
    End Sub
#End Region
#Region "Funzioni dB"
    ''' <summary>
    ''' Svuota nel dB tutti gli indirizzi presenti
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Da utilizzare per poter in seguito inserire la lista di tutti i nuovi indirizzi
    ''' </remarks>
    Public Function EmptyList() As Boolean
        Dim HasNoError As Boolean = True

        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim iResponse As Integer
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_Mailing_EmptyLIST"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetAdvancedParameter("@MailListId", Me._Id, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            iResponse = objAccesso.GetExecuteNotQuery(oRequest)
        Catch ex As Exception
            HasNoError = False
        End Try

        Return HasNoError
    End Function

    ''' <summary>
    ''' Elimina fisicamente da dB la lista corrente e tutti gli indirizzi ad essa associati
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RemoveList() As Boolean
        Dim HasNoError As Boolean = True
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim iResponse As Integer
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_Mailing_DeleteLIST"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetAdvancedParameter("@MailListId", Me._Id, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            iResponse = objAccesso.GetExecuteNotQuery(oRequest)
        Catch ex As Exception
            HasNoError = False
        End Try

        If HasNoError Then
            Me._Id = -1
            Me._ProprietarioId = -1
            Me._Nome = ""
        End If
        Return HasNoError
    End Function

    ''' <summary>
    ''' Aggiunge una nuova lista di distribuzione, senza inserire gli indirizzi.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddLista() As Boolean
        Dim HasNoError As Boolean = True
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_Mailing_AddList"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetAdvancedParameter("@MLLS_Id", "", ParameterDirection.Output, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@MLLS_PRSN_Id", Me._ProprietarioId, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@MLLS_Nome", Me._Nome, ParameterDirection.Input, SqlDbType.VarChar, , 30)
            .Parameters.Add(oParam)

            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            objAccesso.GetExecuteNotQuery(oRequest)
            Me._Id = oRequest.GetValueFromParameter(1)
        Catch ex As Exception
            Me._Id = -1
            HasNoError = False
        End Try

        Return HasNoError
    End Function

    ''' <summary>
    ''' Rinomina una lista di distribuzione
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RenameLista() As Boolean
        Dim HasNoError As Boolean = True
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim objAccesso As New COL_DataAccess
        '@MLLS_Id int,
        '@MLLS_PRSN_Id int,
        '@MLLS_Nome nvarchar(30)
        With oRequest
            .Command = "sp_Mailing_UpdateListName"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetAdvancedParameter("@MLLS_Id", Me._Id, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@MLLS_PRSN_Id", Me._ProprietarioId, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@MLLS_Nome", Me._Nome, ParameterDirection.Input, SqlDbType.VarChar, , 30)
            .Parameters.Add(oParam)

            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            objAccesso.GetExecuteNotQuery(oRequest)
        Catch ex As Exception
            Me._Id = -1
            HasNoError = False
        End Try

        Return HasNoError
    End Function

    ''' <summary>
    '''   Recupera i dati di una singola lista, partendo dal .Id dell'oggetto
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Estrai()
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_Mailing_EstraiLista"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetAdvancedParameter("@MLLS_Id", Me._Id, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetAdvancedParameter("@MLLS_Nome", "", ParameterDirection.Output, SqlDbType.VarChar, , 30)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetAdvancedParameter("@MLLS_PRSN_Id", "", ParameterDirection.Output, SqlDbType.Int)
            .Parameters.Add(oParam)

            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            objAccesso.GetExecuteNotQuery(oRequest)
            Me._Nome = oRequest.GetValueFromParameter(2)
            Me._ProprietarioId = oRequest.GetValueFromParameter(3)
        Catch ex As Exception

        End Try
    End Sub
#End Region
End Class

''' <summary>
''' Contiene i dati di una singola lista.
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class MailingAddress
    Inherits ObjectBase
#Region "Property"
#Region "Private"
    Private _Id As Integer
    Private _PersonaID As Integer
    Private _PersonaNome As String
    Private _PersonaCognome As String
    Private _PersonaMail As String
    Private _Struttura As String
    Private _Titolo As String
    Private _IdLista As Integer
#End Region
#Region "Public"
    Public Property Id() As Integer
        Get
            Return _Id
        End Get
        Set(ByVal value As Integer)
            _Id = value
        End Set
    End Property
    Public Property PersonaID() As Integer
        Get
            Return _PersonaID
        End Get
        Set(ByVal value As Integer)
            _PersonaID = value
        End Set
    End Property
    Public Property PersonaNome() As String
        Get
            Return _PersonaNome
        End Get
        Set(ByVal value As String)
            _PersonaNome = value
        End Set
    End Property
    Public Property PersonaCognome() As String
        Get
            Return _PersonaCognome
        End Get
        Set(ByVal value As String)
            _PersonaCognome = value
        End Set
    End Property
    Public Property PersonaMail() As String
        Get
            Return _PersonaMail
        End Get
        Set(ByVal value As String)
            _PersonaMail = value
        End Set
    End Property
    Public Property Struttura() As String
        Get
            Return _Struttura
        End Get
        Set(ByVal value As String)
            _Struttura = value
        End Set
    End Property
    Public Property IdLista() As Integer
        Get
            Return _IdLista
        End Get
        Set(ByVal value As Integer)
            _IdLista = value
        End Set
    End Property
    Public Property Titolo() As String
        Get
            Return _Titolo
        End Get
        Set(ByVal value As String)
            _Titolo = value
        End Set
    End Property
#End Region
#End Region
#Region "Costruttori"
    Public Sub New()
        Me._Id = 0
        Me._PersonaID = 0
        Me._PersonaNome = ""
        Me._PersonaCognome = ""
        Me._PersonaMail = ""
        Me._Struttura = ""
        Me._Titolo = ""
    End Sub
    Public Sub New( _
        ByVal Id As Integer _
        )
        Me._Id = Id
        Me._PersonaID = 0
        Me._PersonaNome = ""
        Me._PersonaCognome = ""
        Me._PersonaMail = ""
        Me._Struttura = ""
        Me._Titolo = ""
    End Sub
    Public Sub New( _
        ByVal IdPersona As Integer, _
        ByVal Nome As String, _
        ByVal Cognome As String, _
        ByVal Mail As String, _
        ByVal Struttura As String, _
        ByVal Titolo As String)

        Me._Id = 0
        Me._PersonaID = IdPersona
        Me._PersonaNome = Nome
        Me._PersonaCognome = Cognome
        Me._PersonaMail = Mail
        Me._Struttura = Struttura
        Me._Titolo = Titolo

    End Sub
    Public Sub New( _
        ByVal IdAddress As Integer, _
        ByVal IdPersona As Integer, _
        ByVal Nome As String, _
        ByVal Cognome As String, _
        ByVal Mail As String, _
        ByVal Struttura As String, _
        ByVal Titolo As String)

        Me._Id = IdAddress
        Me._PersonaID = IdPersona
        Me._PersonaNome = Nome
        Me._PersonaCognome = Cognome
        Me._PersonaMail = Mail
        Me._Struttura = Struttura
        Me._Titolo = Titolo
    End Sub
#End Region

#Region "Funzioni dB"
    ''' <summary>
    ''' Aggiunge un singolo indirizzo
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Apre e chiude la connessione in maniera autonoma.
    ''' Per inserire in un colpo solo più indirizzi utilizzare l'apposita funzione,
    ''' che utilizza una connessione sola per tutti gli indirizzi
    ''' </remarks>
    Public Function SaveNew() As Boolean
        Dim HasNoError As Boolean = True
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_Mailing_AddIndirizzo"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetAdvancedParameter("@MLND_Id", "", ParameterDirection.Output, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@MLND_PRSN_ID", Me._PersonaID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@MLND_PRSN_Nome", Me._PersonaNome, ParameterDirection.Input, SqlDbType.VarChar, , 40)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@MLND_PRSN_Cognome", Me._PersonaCognome, ParameterDirection.Input, SqlDbType.VarChar, , 40)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@MLND_PRSN_Mail", Me._PersonaMail, ParameterDirection.Input, SqlDbType.VarChar, , 255)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@MLND_Struttura", Me._Struttura, ParameterDirection.Input, SqlDbType.VarChar, , 100)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@MLND_Titolo", Me._Titolo, ParameterDirection.Input, SqlDbType.VarChar, , 25)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@MLND_MLLS_Id", Me._IdLista, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            objAccesso.GetExecuteNotQuery(oRequest)
            Me._Id = oRequest.GetValueFromParameter(1)
        Catch ex As Exception
            Me._Id = -1
            HasNoError = False
        End Try

        Return HasNoError
    End Function

    ''' <summary>
    ''' Salva i dati di un singolo indirizzo...
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Update() As Boolean
        Dim HasNoError As Boolean = True
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_Mailing_UpdateAddress"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetAdvancedParameter("@MLND_Id", Me._Id, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@MLND_PRSN_ID", Me._PersonaID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@MLND_PRSN_Nome", Me._PersonaNome, ParameterDirection.Input, SqlDbType.VarChar, , 40)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@MLND_PRSN_Cognome", Me._PersonaCognome, ParameterDirection.Input, SqlDbType.VarChar, , 40)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@MLND_PRSN_Mail", Me._PersonaMail, ParameterDirection.Input, SqlDbType.VarChar, , 255)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@MLND_Struttura", Me._Struttura, ParameterDirection.Input, SqlDbType.VarChar, , 100)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@MLND_Titolo", Me._Titolo, ParameterDirection.Input, SqlDbType.VarChar, , 25)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@MLND_MLLS_Id", Me._IdLista, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            objAccesso.GetExecuteNotQuery(oRequest)
        Catch ex As Exception
            Me._Id = -1
            HasNoError = False
        End Try

        Return HasNoError
    End Function

    ''' <summary>
    ''' Elimina l'indirizzo corrente
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Delete() As Boolean
        Dim HasNoError As Boolean = True

        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim iResponse As Integer
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_Mailing_DeleteADDRESS"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetAdvancedParameter("@MailAddressId", Me._Id, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            iResponse = objAccesso.GetExecuteNotQuery(oRequest)
        Catch ex As Exception
            HasNoError = False
        End Try
        Return HasNoError
        'If HasNoError Then
        '    Me._Id = -1
        '    Me._PersonaID = 0
        '    Me._PersonaNome = ""
        '    Me._PersonaCognome = ""
        '    Me._PersonaMail = ""
        '    Me._Struttura = ""
        '    Me._Titolo = ""
        'End If
    End Function

    Public Shared Function DeleteInt(ByVal PrsnId As Integer, ByVal MailingListID As Integer)
        Dim HasNoError As Boolean = True

        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim iResponse As Integer
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_Mailing_DeleteADDRESSInt"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetAdvancedParameter("@MLND_PRSN_ID", PrsnId, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetAdvancedParameter("@MLND_MLLS_Id", MailingListID, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            iResponse = objAccesso.GetExecuteNotQuery(oRequest)
        Catch ex As Exception
            HasNoError = False
        End Try
        Return HasNoError
    End Function

    ''' <summary>
    '''   Recupera i dati di una singolo Indirizzo, partendo dal .Id dell'oggetto
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Estrai()
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_Mailing_EstraiIndirizzo"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetAdvancedParameter("@MLND_Id", Me._Id, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetAdvancedParameter("@MLND_PRSN_ID", "", ParameterDirection.Output, SqlDbType.Int)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetAdvancedParameter("@MLND_PRSN_Nome", "", ParameterDirection.Output, SqlDbType.VarChar, , 40)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetAdvancedParameter("@MLND_PRSN_Cognome", "", ParameterDirection.Output, SqlDbType.VarChar, , 40)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@MLND_PRSN_Mail", "", ParameterDirection.Output, SqlDbType.VarChar, , 255)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@MLND_Struttura", "", ParameterDirection.Output, SqlDbType.VarChar, , 100)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@MLND_Titolo", "", ParameterDirection.Output, SqlDbType.VarChar, , 25)
            .Parameters.Add(oParam)

            oParam = objAccesso.GetAdvancedParameter("@MLND_MLLS_Id", "", ParameterDirection.Output, SqlDbType.Int)
            .Parameters.Add(oParam)

            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            objAccesso.GetExecuteNotQuery(oRequest)
            Me._PersonaID = oRequest.GetValueFromParameter(2)
            Me._PersonaNome = oRequest.GetValueFromParameter(3)
            Me._PersonaCognome = oRequest.GetValueFromParameter(4)
            Me._PersonaMail = oRequest.GetValueFromParameter(5)
            Me._Struttura = oRequest.GetValueFromParameter(6)
            Me._Titolo = oRequest.GetValueFromParameter(7)
            Me._IdLista = oRequest.GetValueFromParameter(8)
        Catch ex As Exception

        End Try
    End Sub
#End Region
End Class

''' <summary>
''' Contiene le funzioni shared per recuperare le liste di utenti
''' </summary>
''' <remarks>
''' Eventualmente inserire filtri ed ordinamenti (o vanno nel presenter?!)
''' </remarks>
Public Class MailingManager
    Inherits ObjectBase
    ''' <summary>
    '''   Restituisce l'elenco delle liste di distribuzione di un singolo utente
    ''' </summary>
    ''' <param name="ProprietarioId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function GetListe(ByVal ProprietarioId As Integer) As List(Of MailingList)
        Dim oList As New List(Of MailingList)

        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim oDataReader As IDataReader
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_Mailing_GetAllList"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetAdvancedParameter("@PersonaID", ProprietarioId, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With

        Try
            oDataReader = objAccesso.GetdataReader(oRequest)
            While oDataReader.Read

                Dim oMailingList As MailingList

                Try
                    oMailingList = New MailingList
                    With oMailingList
                        .Id = oDataReader("MLLS_Id")
                        .ProprietarioId = oDataReader("MLLS_PRSN_Id")
                        .Nome = GenericValidator.ValString(oDataReader("MLLS_Nome"), "- -")
                        .IsSelected = False
                    End With
                    oList.Add(oMailingList)
                Catch ex As Exception

                End Try
            End While
        Catch ex As Exception

        Finally
            Try
                If oDataReader.IsClosed = False Then
                    oDataReader.Close()
                End If
            Catch ex As Exception
            End Try
        End Try

        Return oList
    End Function

    ''' <summary>
    '''    Restituisce l'elenco di indirizzi di una specifica lista di distribuzione
    ''' </summary>
    ''' <param name="MailingListId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function GetIndirizzi(ByVal MailingListId As Integer) As List(Of MailingAddress)
        Dim oList As New List(Of MailingAddress)

        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim oDataReader As IDataReader
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_Mailing_GetAllAddress"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetAdvancedParameter("@MailListId", MailingListId, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With

        Try
            oDataReader = objAccesso.GetdataReader(oRequest)
            While oDataReader.Read

                Dim oAddress As New MailingAddress
                'Dim id As Integer


                'MLND_Struttura, 
                'MLND_Titolo, 
                'MLND_MLLS_Id, 

                Try
                    oAddress = New MailingAddress()
                    With oAddress
                        .Id = oDataReader("MLND_Id")
                        .PersonaID = GenericValidator.ValInteger(oDataReader("MLND_PRSN_ID"), 0)

                        If .PersonaID = 0 Then
                            .PersonaNome = GenericValidator.ValString(oDataReader("MLND_PRSN_Nome"), "")
                            .PersonaCognome = GenericValidator.ValString(oDataReader("MLND_PRSN_Cognome"), "")
                            .PersonaMail = GenericValidator.ValString(oDataReader("MLND_PRSN_Mail"), "")
                        Else
                            .PersonaNome = GenericValidator.ValString(oDataReader("PRSN_nome"), "")
                            .PersonaCognome = GenericValidator.ValString(oDataReader("PRSN_cognome"), "")
                            .PersonaMail = GenericValidator.ValString(oDataReader("PRSN_mail"), "")
                        End If
                        .Struttura = GenericValidator.ValString(oDataReader("MLND_Struttura"), "")
                        .Titolo = GenericValidator.ValString(oDataReader("MLND_Titolo"), "")
                        .IdLista = oDataReader("MLND_Id")
                    End With

                    oList.Add(oAddress)

                Catch ex As Exception

                End Try
            End While
        Catch ex As Exception

        Finally
            Try
                If oDataReader.IsClosed = False Then
                    oDataReader.Close()
                End If
            Catch ex As Exception
            End Try
        End Try

        Return oList
    End Function

    ''' <summary>
    ''' Stringa da aggiungere al CC o CCN
    ''' </summary>
    ''' <param name="MailingListId">
    ''' ID della lista in questione
    ''' </param>
    ''' <returns>
    ''' Una stringa già formattata da inserire nei campi To: CC: o CCn: di una mail con tutti gli indirizzi e relativi dati (Titolo ed anagrafica)
    ''' </returns>
    ''' <remarks>Accede al dB per recuperare i dati della lista</remarks>
    Public Shared Function GetCCNString(ByVal MailingListId As Integer) As String
        Dim CCNString As String
        Dim oMailingList As List(Of MailingAddress)
        oMailingList = GetIndirizzi(MailingListId)
        CCNString = GetCCNString(oMailingList)
        Return CCNString
    End Function
    ''' <summary>
    '''  Da una lista, ricostruisce la stringa di indirizzi da inserire in CCn
    ''' </summary>
    ''' <param name="oMailingList"></param>
    ''' <returns>
    ''' Una stringa già formattata da inserire nei campi To: CC: o CCn: di una mail con tutti gli indirizzi e relativi dati (Titolo ed anagrafica)
    ''' </returns>
    ''' <remarks></remarks>
    Public Shared Function GetCCNString(ByVal oMailingList As List(Of MailingAddress)) As String
        Dim CCNString As String = ""

        For Each oAddress As MailingAddress In oMailingList
            CCNString &= Chr(34) & oAddress.Titolo & " " & oAddress.PersonaCognome & " " & oAddress.PersonaNome & Chr(34)
            CCNString &= "<" & oAddress.PersonaMail & ">; "
        Next


        Return CCNString
    End Function


    ''' <summary>
    '''  Aggiunge alle liste di MailAddressCollection passate 
    ''' gli indirizzi di una data liasta.
    ''' Se il campo A è vuoto il primo elemento viene inserito lì, tutto gli altri in oCC.
    ''' </summary>
    ''' <param name="MailingListId"></param>
    ''' <param name="oTo">
    ''' Campo A: Solo il primo elemento della lista viene inserito qui.
    ''' </param>
    ''' <param name="oCC">
    ''' Campo CC: Tutti gli altri qui.
    ''' </param>
    ''' <remarks>
    ''' E' possibile giocare in modo da inserire tutti gli indirizzi in A: (passandolo per entrambe i valori (DA PROVARE) o passando CC o CCn dell'oggetto mail a seconda delle esigenze.
    ''' </remarks>
    Public Shared Sub AddMailAddressCollection(ByVal MailingListId As Integer, ByRef oTo As System.Net.Mail.MailAddressCollection, ByRef oCC As System.Net.Mail.MailAddressCollection)
        Dim oMailingList As List(Of MailingAddress)
        oMailingList = GetIndirizzi(MailingListId)
        For Each address As MailingAddress In oMailingList
            Dim oMailAddress As New System.Net.Mail.MailAddress(address.PersonaMail, address.PersonaCognome & " " & address.PersonaNome)
            If oTo.Count > 0 Then
                oCC.Add(oMailAddress)
            Else
                oTo.Add(oMailAddress)
            End If
        Next
    End Sub


    ''' <summary>
    '''   Restituisce l'elenco delle liste di distribuzione di un singolo utente correttamente ordinate
    ''' </summary>
    ''' <param name="ProprietarioId"></param>
    ''' <param name="sortDirection"></param>
    ''' <param name="sortExpression"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetListeOrdinate(ByVal ProprietarioId As Integer, ByVal sortExpression As String, ByVal sortDirection As String) As List(Of MailingList)
        Dim oList As List(Of MailingList)

        If sortDirection <> String.Empty Then
            sortDirection = sortDirection.ToLower
        End If
        oList = GetListe(ProprietarioId)

        If (Not sortExpression Is Nothing AndAlso sortDirection <> String.Empty) Then
            oList.Sort(New GenericComparer(Of MailingList)(sortExpression))
        End If

        If (Not sortDirection Is Nothing AndAlso sortDirection = "desc") Then
            oList.Reverse()
        End If

        Return oList
    End Function

    ''' <summary>
    '''    Restituisce l'elenco di indirizzi di una specifica lista di distribuzione correttamente ordinate
    ''' </summary>
    ''' <param name="MailingListId"></param>
    ''' <param name="sortDirection"></param>
    ''' <param name="sortExpression"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetIndirizziOrdinati(ByVal MailingListId As Integer, ByVal sortExpression As String, ByVal sortDirection As String) As List(Of MailingAddress)
        Dim oList As List(Of MailingAddress)

        If sortDirection <> String.Empty Then
            sortDirection = sortDirection.ToLower
        End If
        oList = GetIndirizzi(MailingListId)
        'Dim iscritto As New Iscritto
        'iscritto.Persona.Nome
        'iscritto.Persona.Cognome
        'iscritto.Persona.Mail

        If (Not sortExpression Is Nothing AndAlso sortDirection <> String.Empty) Then
            oList.Sort(New GenericComparer(Of MailingAddress)(sortExpression))
        End If

        If (Not sortDirection Is Nothing AndAlso sortDirection = "desc") Then
            oList.Reverse()
        End If

        Return oList
    End Function


End Class