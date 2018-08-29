Imports COL_DataLayer
'Imports COL_BusinessLogic_v2.SMScomponent
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.FileLayer
Imports COL_BusinessLogic_v2.Comol.Manager

Imports System.DirectoryServices.Protocols
Imports System.Net
Imports System.Security.Cryptography.X509Certificates
Imports System.DirectoryServices
Imports lm.Comol.Core.File

Namespace CL_persona
    <Serializable()>
    Public Class COL_Persona
        Inherits ObjectBase
        Implements IEquatable(Of COL_Persona)

#Region "Private Property"
        Protected n_PRSN_id As Integer
        Protected n_PRSN_login As String
        Protected n_PRSN_pwd As String
        Protected n_PRSN_nome As String
        Protected n_PRSN_cognome As String
        Protected n_PRSN_dataNascita As DateTime
        Protected n_PRSN_luogoNascita As String
        Protected n_PRSN_sesso As Integer
        Protected n_PRSN_codFiscale As String
        Protected n_PRSN_indirizzo As String
        Protected n_PRSN_cap As String
        Protected n_PRSN_citta As String
        Protected n_PRSN_PRVN As New Provincia
        Protected n_PRSN_STTO As New COL_Stato
        Protected n_PRSN_telefono1 As String
        Protected n_PRSN_telefono2 As String
        Protected n_PRSN_cellulare As String
        Protected n_PRSN_fax As String
        Protected n_PRSN_mail As String
        Protected n_PRSN_homePage As String
        Protected n_PRSN_ultimoCollegamento As DateTime
        Protected n_PRSN_fotoPath As String
        Protected n_PRSN_invisibile As Boolean
        Protected n_PRSN_TPPR As New COL_TipoPersona
        Protected n_PRSN_LNGU As New Lingua
        Protected n_PRSN_note As String
        Protected n_PRSN_ISTT As New COL_Istituzione
        Protected n_PRSN_IsInterno As Boolean
        Protected n_PRSN_domRiserva As String
        Protected n_PRSN_rispRiserva As String
        Protected n_PRSN_dataInserimento As DateTime
        Protected n_PRSN_RicezioneSMS As Integer
        Protected n_PRSN_maxAree As Integer

        Protected n_PRSN_ORGNDefault_id As Integer
        Protected n_PRSN_ORGNDefault_ragioneSociale As String

        Protected n_PRSN_AUTN_ID As Integer
        Protected n_PRSN_AUTN_RemoteUniqueID As String
        Protected n_PRSN_mostraMail As Integer
        Protected n_PRSN_Ricevimento As String
        Protected n_PRSN_MailSecondaria As String
        Protected n_Errore As Errori_Db

        Protected n_Sector As String
        Protected n_Job As String

#End Region

#Region "Public Property"
        Public Property ID() As Integer
            Get
                ID = n_PRSN_id
            End Get
            Set(ByVal Value As Integer)
                n_PRSN_id = Value
            End Set
        End Property
        Public Property Login() As String
            Get
                Login = n_PRSN_login
            End Get
            Set(ByVal Value As String)
                n_PRSN_login = Value
            End Set
        End Property
        Public Property Pwd() As String
            Get
                Pwd = n_PRSN_pwd
            End Get
            Set(ByVal Value As String)
                n_PRSN_pwd = Value
            End Set
        End Property
        Public Property Nome() As String
            Get
                Nome = n_PRSN_nome
            End Get
            Set(ByVal Value As String)
                n_PRSN_nome = Value
            End Set
        End Property
        Public Property Cognome() As String
            Get
                Cognome = n_PRSN_cognome
            End Get
            Set(ByVal Value As String)
                n_PRSN_cognome = Value
            End Set
        End Property
        Public ReadOnly Property Anagrafica() As String
            Get
                Anagrafica = n_PRSN_nome & " " & n_PRSN_cognome
            End Get
        End Property
        Public ReadOnly Property AnagraficaTitolo() As String
            Get
                AnagraficaTitolo = n_PRSN_cognome & " " & n_PRSN_nome
            End Get
        End Property
        Public Property DataNascita() As DateTime
            Get
                DataNascita = n_PRSN_dataNascita
            End Get
            Set(ByVal Value As DateTime)
                n_PRSN_dataNascita = Value
            End Set
        End Property
        Public Property LuogoNascita() As String
            Get
                LuogoNascita = n_PRSN_luogoNascita
            End Get
            Set(ByVal Value As String)
                n_PRSN_luogoNascita = Value
            End Set
        End Property
        Public Property Sesso() As Integer
            Get
                Sesso = n_PRSN_sesso
            End Get
            Set(ByVal Value As Integer)
                n_PRSN_sesso = Value
            End Set
        End Property
        Public Property CodFiscale() As String
            Get
                CodFiscale = n_PRSN_codFiscale
            End Get
            Set(ByVal Value As String)
                n_PRSN_codFiscale = Value
            End Set
        End Property
        Public Property Indirizzo() As String
            Get
                Indirizzo = n_PRSN_indirizzo
            End Get
            Set(ByVal Value As String)
                n_PRSN_indirizzo = Value
            End Set
        End Property
        Public Property Cap() As String
            Get
                Cap = n_PRSN_cap
            End Get
            Set(ByVal Value As String)
                n_PRSN_cap = Value
            End Set
        End Property
        Public Property Citta() As String
            Get
                Citta = n_PRSN_citta
            End Get
            Set(ByVal Value As String)
                n_PRSN_citta = Value
            End Set
        End Property
        Public Property Provincia() As Provincia
            Get
                Provincia = n_PRSN_PRVN
            End Get
            Set(ByVal Value As Provincia)
                n_PRSN_PRVN = Value
            End Set
        End Property
        Public Property Stato() As COL_Stato
            Get
                Stato = n_PRSN_STTO
            End Get
            Set(ByVal Value As COL_Stato)
                n_PRSN_STTO = Value
            End Set
        End Property
        Public Property Telefono1() As String
            Get
                Telefono1 = n_PRSN_telefono1
            End Get
            Set(ByVal Value As String)
                n_PRSN_telefono1 = Value
            End Set
        End Property
        Public Property Telefono2() As String
            Get
                Telefono2 = n_PRSN_telefono2
            End Get
            Set(ByVal Value As String)
                n_PRSN_telefono2 = Value
            End Set
        End Property
        Public Property Cellulare() As String
            Get
                Cellulare = n_PRSN_cellulare
            End Get
            Set(ByVal Value As String)
                n_PRSN_cellulare = Value
            End Set
        End Property
        Public Property Fax() As String
            Get
                Fax = n_PRSN_fax
            End Get
            Set(ByVal Value As String)
                n_PRSN_fax = Value
            End Set
        End Property
        Public Property Mail() As String
            Get
                Mail = n_PRSN_mail
            End Get
            Set(ByVal Value As String)
                n_PRSN_mail = Value
            End Set
        End Property
        Public Property MailSecondaria() As String
            Get
                MailSecondaria = n_PRSN_MailSecondaria
            End Get
            Set(ByVal Value As String)
                n_PRSN_MailSecondaria = Value
            End Set
        End Property

        Public Property HomePage() As String
            Get
                HomePage = n_PRSN_homePage
            End Get
            Set(ByVal Value As String)
                n_PRSN_homePage = Value
            End Set
        End Property
        Public Property UltimoCollegamento() As DateTime
            Get
                UltimoCollegamento = n_PRSN_ultimoCollegamento
            End Get
            Set(ByVal Value As DateTime)
                n_PRSN_ultimoCollegamento = Value
            End Set
        End Property
        Public Property FotoPath() As String
            Get
                FotoPath = n_PRSN_fotoPath
            End Get
            Set(ByVal Value As String)
                n_PRSN_fotoPath = Value
            End Set
        End Property
        Public Property Bloccata() As Boolean
            Get
                Bloccata = n_PRSN_invisibile
            End Get
            Set(ByVal Value As Boolean)
                n_PRSN_invisibile = Value
            End Set
        End Property
        Public Property TipoPersona() As COL_TipoPersona
            Get
                TipoPersona = n_PRSN_TPPR
            End Get
            Set(ByVal Value As COL_TipoPersona)
                n_PRSN_TPPR = Value
            End Set
        End Property
        Public Property Lingua() As Lingua
            Get
                Lingua = n_PRSN_LNGU
            End Get
            Set(ByVal Value As Lingua)
                n_PRSN_LNGU = Value
            End Set
        End Property
        Public Property Note() As String
            Get
                Note = n_PRSN_note
            End Get
            Set(ByVal Value As String)
                n_PRSN_note = Value
            End Set
        End Property
        Public Property Istituzione() As COL_Istituzione
            Get
                Istituzione = n_PRSN_ISTT
            End Get
            Set(ByVal Value As COL_Istituzione)
                n_PRSN_ISTT = Value
            End Set
        End Property
        Public Property IsInterno() As Boolean
            Get
                IsInterno = Me.n_PRSN_IsInterno
            End Get
            Set(ByVal Value As Boolean)
                n_PRSN_IsInterno = Value
            End Set
        End Property
        Public Property DataInserimento() As DateTime
            Get
                DataInserimento = n_PRSN_dataInserimento
            End Get
            Set(ByVal Value As DateTime)
                n_PRSN_dataInserimento = Value
            End Set
        End Property

        Public Property RicezioneSMS() As Integer
            Get
                RicezioneSMS = n_PRSN_RicezioneSMS
            End Get
            Set(ByVal Value As Integer)
                n_PRSN_RicezioneSMS = Value

            End Set
        End Property

        Public Property maxAree() As Integer
            Get
                maxAree = n_PRSN_maxAree
            End Get
            Set(ByVal Value As Integer)
                n_PRSN_maxAree = Value
            End Set
        End Property

        Public Property ORGNDefault_id() As Integer
            Get
                ORGNDefault_id = n_PRSN_ORGNDefault_id
            End Get
            Set(ByVal Value As Integer)
                n_PRSN_ORGNDefault_id = Value
            End Set
        End Property
        Public Property ORGNDefault_ragioneSociale() As String
            Get
                ORGNDefault_ragioneSociale = n_PRSN_ORGNDefault_ragioneSociale
            End Get
            Set(ByVal Value As String)
                n_PRSN_ORGNDefault_ragioneSociale = Value
            End Set
        End Property


        Public Property AUTN_ID() As Integer
            Get
                AUTN_ID = n_PRSN_AUTN_ID
            End Get
            Set(ByVal Value As Integer)
                n_PRSN_AUTN_ID = Value
            End Set
        End Property
        Public Property AUTN_RemoteUniqueID() As String
            Get
                AUTN_RemoteUniqueID = n_PRSN_AUTN_RemoteUniqueID
            End Get
            Set(ByVal Value As String)
                n_PRSN_AUTN_RemoteUniqueID = Value
            End Set
        End Property
        Public Property MostraMail() As Boolean
            Get
                MostraMail = (Me.n_PRSN_mostraMail = 1)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    n_PRSN_mostraMail = 1
                Else
                    n_PRSN_mostraMail = 0
                End If
            End Set
        End Property
        Public Property InfoRicevimento() As String
            Get
                InfoRicevimento = n_PRSN_Ricevimento
            End Get
            Set(ByVal Value As String)
                n_PRSN_Ricevimento = Value
            End Set
        End Property
        Public Property Errore() As Errori_Db
            Get
                Errore = n_Errore
            End Get
            Set(ByVal Value As Errori_Db)
                n_Errore = Value
            End Set
        End Property


        Public Property Sector As String
            Get
                Return n_Sector
            End Get
            Set(value As String)
                n_Sector = value
            End Set
        End Property
        
        Public Property Job As String
            Get
                Return n_Job
            End Get
            Set(value As String)
                n_Job = value
            End Set
        End Property
#End Region

#Region "Metodi New"
        Sub New()
            Me.n_Errore = Errori_Db.None
            Provincia = New Provincia()
            Lingua = New Lingua()
        End Sub
        Sub New(ByVal Id As Integer)
            Me.n_PRSN_id = Id
            Provincia = New Provincia()
            Lingua = New Lingua()
        End Sub
        Sub New(ByVal Id As Integer, ByVal Nome As String, ByVal cognome As String)
            Me.n_PRSN_id = Id
            Me.n_PRSN_nome = Nome
            Me.n_PRSN_cognome = cognome
            Me.n_Errore = Errori_Db.None
            Provincia = New Provincia()
            Lingua = New Lingua()
        End Sub
#End Region

#Region "Metodi Standard"
        Public Shared Function GetPersona(ByVal PersonaID As Integer, ByVal LinguaID As Integer) As COL_Persona
            'carica i campi del db nell'oggetto in base all'id della persona
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Persona_Estrai"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@PRSN_id", PersonaID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Nome", "", ParameterDirection.Output, DbType.String, False, 40)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Cognome", "", ParameterDirection.Output, DbType.String, False, 40)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_indirizzo", "", ParameterDirection.Output, DbType.String, False, 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_cap", "", ParameterDirection.Output, DbType.String, False, 10)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_citta", "", ParameterDirection.Output, DbType.String, False, 30)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_PRVN_Id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Telefono1", "", ParameterDirection.Output, DbType.String, False, 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Telefono2", "", ParameterDirection.Output, DbType.String, False, 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Cellulare", "", ParameterDirection.Output, DbType.String, False, 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Fax", "", ParameterDirection.Output, DbType.String, False, 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Mail", "", ParameterDirection.Output, DbType.String, False, 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_HomePage", "", ParameterDirection.Output, DbType.String, False, 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_FotoPath", "", ParameterDirection.Output, DbType.String, False, 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_RicezioneSMS", "", ParameterDirection.Output, DbType.Byte)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Login", "", ParameterDirection.Output, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_maxAree", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_AUTN_ID", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_AUTN_RemoteUniqueID", "", ParameterDirection.Output, DbType.String, , 50)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_LNGU_id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_invisible", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_LNGU_Codice", "", ParameterDirection.Output, DbType.String, , 10)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_TPPR_Id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_mostraMail", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPPR_descrizione", "", ParameterDirection.Output, DbType.String, , 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LinguaID", LinguaID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_sesso", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_Ricevimento", "", ParameterDirection.Output, SqlDbType.VarChar, True, 500)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRSN_MailSecondaria", "", ParameterDirection.Output, SqlDbType.VarChar, True, 255)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_Sector", "", ParameterDirection.Output, DbType.String, , 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Job", "", ParameterDirection.Output, DbType.String, , 255)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                Dim oPersona As New COL_Persona
                objAccesso.GetExecuteNotQuery(oRequest)
                oPersona.ID = PersonaID
                oPersona.Nome = oRequest.GetValueFromParameter(2)
                oPersona.Cognome = oRequest.GetValueFromParameter(3)
                oPersona.Indirizzo = oRequest.GetValueFromParameter(4)
                oPersona.Cap = oRequest.GetValueFromParameter(5)
                oPersona.Citta = oRequest.GetValueFromParameter(6)
                oPersona.Telefono1 = oRequest.GetValueFromParameter(8)
                oPersona.Telefono2 = oRequest.GetValueFromParameter(9)
                oPersona.Cellulare = oRequest.GetValueFromParameter(10)
                oPersona.Fax = oRequest.GetValueFromParameter(11)
                oPersona.Mail = oRequest.GetValueFromParameter(12)
                oPersona.HomePage = oRequest.GetValueFromParameter(13)
                oPersona.FotoPath = oRequest.GetValueFromParameter(14)
                oPersona.RicezioneSMS = oRequest.GetValueFromParameter(15)
                oPersona.Login = oRequest.GetValueFromParameter(16)
                oPersona.maxAree = oRequest.GetValueFromParameter(17)
                oPersona.AUTN_ID = oRequest.GetValueFromParameter(18)
                oPersona.AUTN_RemoteUniqueID = oRequest.GetValueFromParameter(19)
                '	oPersona.Lingua.Id = oRequest.GetValueFromParameter(20)
                oPersona.Bloccata = GenericValidator.ValBool(oRequest.GetValueFromParameter(21), False)
                oPersona.Lingua = Lingua.CreateByCode(oRequest.GetValueFromParameter(20), oRequest.GetValueFromParameter(22))
                oPersona.TipoPersona.ID = oRequest.GetValueFromParameter(23)
                oPersona.MostraMail = GenericValidator.ValBool(oRequest.GetValueFromParameter(24), False)
                oPersona.TipoPersona.Descrizione = oRequest.GetValueFromParameter(25)
                '26 = LinguaId
                oPersona.Sesso = oRequest.GetValueFromParameter(27)
                oPersona.InfoRicevimento = oRequest.GetValueFromParameter(28)
                '29 = Mail secondaria
                oPersona.Sector = oRequest.GetValueFromParameter(30)
                oPersona.Job = oRequest.GetValueFromParameter(31)
                Return oPersona
            Catch ax As Exception
                Return Nothing
            End Try
            Return Nothing
        End Function
        Public Sub Estrai(ByVal LinguaID As Integer)
            'carica i campi del db nell'oggetto in base all'id della persona
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Persona_Estrai"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@PRSN_id", n_PRSN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Nome", "", ParameterDirection.Output, DbType.String, False, 40)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Cognome", "", ParameterDirection.Output, DbType.String, False, 40)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_indirizzo", "", ParameterDirection.Output, DbType.String, False, 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_cap", "", ParameterDirection.Output, DbType.String, False, 10)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_citta", "", ParameterDirection.Output, DbType.String, False, 30)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_PRVN_Id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Telefono1", "", ParameterDirection.Output, DbType.String, False, 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Telefono2", "", ParameterDirection.Output, DbType.String, False, 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Cellulare", "", ParameterDirection.Output, DbType.String, False, 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Fax", "", ParameterDirection.Output, DbType.String, False, 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Mail", "", ParameterDirection.Output, DbType.String, False, 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_HomePage", "", ParameterDirection.Output, DbType.String, False, 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_FotoPath", "", ParameterDirection.Output, DbType.String, False, 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_RicezioneSMS", "", ParameterDirection.Output, DbType.Byte)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Login", "", ParameterDirection.Output, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_maxAree", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_AUTN_ID", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_AUTN_RemoteUniqueID", "", ParameterDirection.Output, DbType.String, , 50)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_LNGU_id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_invisible", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_LNGU_Codice", "", ParameterDirection.Output, DbType.String, , 10)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_TPPR_Id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_mostraMail", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPPR_descrizione", "", ParameterDirection.Output, DbType.String, , 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LinguaID", LinguaID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_sesso", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_Ricevimento", "", ParameterDirection.Output, SqlDbType.VarChar, True, 500)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRSN_MailSecondaria", "", ParameterDirection.Output, SqlDbType.VarChar, True, 255)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_Sector", "", ParameterDirection.Output, DbType.String, , 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Job", "", ParameterDirection.Output, DbType.String, , 255)
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
                Me.n_PRSN_PRVN.ID = oRequest.GetValueFromParameter(7)
                Me.n_PRSN_telefono1 = oRequest.GetValueFromParameter(8)
                Me.n_PRSN_telefono2 = oRequest.GetValueFromParameter(9)
                Me.n_PRSN_cellulare = oRequest.GetValueFromParameter(10)
                Me.n_PRSN_fax = oRequest.GetValueFromParameter(11)
                Me.n_PRSN_mail = oRequest.GetValueFromParameter(12)
                Me.n_PRSN_homePage = oRequest.GetValueFromParameter(13)
                Me.n_PRSN_fotoPath = oRequest.GetValueFromParameter(14)
                Me.n_PRSN_RicezioneSMS = oRequest.GetValueFromParameter(15)
                Me.n_PRSN_login = oRequest.GetValueFromParameter(16)
                Me.n_PRSN_maxAree = oRequest.GetValueFromParameter(17)
                Me.n_PRSN_AUTN_ID = oRequest.GetValueFromParameter(18)
                Me.n_PRSN_AUTN_RemoteUniqueID = oRequest.GetValueFromParameter(19)
                'Me.n_PRSN_LNGU.Id = oRequest.GetValueFromParameter(20)
                Me.n_PRSN_invisibile = oRequest.GetValueFromParameter(21)
                'Me.n_PRSN_LNGU.Codice = oRequest.GetValueFromParameter(22)

                Me.n_PRSN_LNGU = Lingua.CreateByCode(oRequest.GetValueFromParameter(20), oRequest.GetValueFromParameter(22))

                Me.n_PRSN_TPPR.ID = oRequest.GetValueFromParameter(23)
                Me.n_PRSN_mostraMail = oRequest.GetValueFromParameter(24)
                '26 = LinguaId
                Me.n_PRSN_sesso = oRequest.GetValueFromParameter(27)
                Me.n_PRSN_Ricevimento = oRequest.GetValueFromParameter(28)
                '29 = Mail secondaria
                Me.n_Sector = oRequest.GetValueFromParameter(30)
                Me.n_Job = oRequest.GetValueFromParameter(31)
            Catch ax As Exception
                Me.n_PRSN_id = -1
                Me.n_Errore = Errori_Db.DBReadExist
            End Try
        End Sub
        Public Sub EstraiTutto(ByVal LinguaID As Integer)
            'carica i campi del db nell'oggetto in base all'id della persona
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Persona_EstraiTutto"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@PRSN_id", n_PRSN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Login", "", ParameterDirection.Output, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_pwd", "", ParameterDirection.Output, DbType.String, , 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Nome", "", ParameterDirection.Output, DbType.String, False, 40)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Cognome", "", ParameterDirection.Output, DbType.String, False, 40)
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
                oParam = objAccesso.GetParameter("@PRSN_citta", "", ParameterDirection.Output, DbType.String, False, 30)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_PRVN_Id", "", ParameterDirection.Output, DbType.Int32)
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
                oParam = objAccesso.GetParameter("@PRSN_Mail", "", ParameterDirection.Output, DbType.String, False, 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_HomePage", "", ParameterDirection.Output, DbType.String, False, 255)
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
                oParam = objAccesso.GetParameter("@PRSN_ISTT_id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_maxAree", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_AUTN_ID", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_AUTN_RemoteUniqueID", "", ParameterDirection.Output, DbType.String, , 50)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_ORGNDefault_id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_ORGNDefault_ragioneSociale", "", ParameterDirection.Output, DbType.String, , 100)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@TPPR_descrizione", "", ParameterDirection.Output, DbType.String, , 100)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_dataInserimento", "", ParameterDirection.Output, DbType.DateTime)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_mostraMail", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_Ricevimento", "", ParameterDirection.Output, SqlDbType.VarChar, True, 500)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_MailSecondaria", "", ParameterDirection.Output, SqlDbType.VarChar, True, 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LinguaID", LinguaID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_Sector", "", ParameterDirection.Output, DbType.String, , 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Job", "", ParameterDirection.Output, DbType.String, , 255)
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
                Me.n_PRSN_PRVN.ID = oRequest.GetValueFromParameter(13)
                Me.n_PRSN_PRVN.Nome = oRequest.GetValueFromParameter(14)
                Me.n_PRSN_STTO.ID = oRequest.GetValueFromParameter(15)
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
                Me.n_PRSN_ISTT.Id = oRequest.GetValueFromParameter(29)
                Me.n_PRSN_maxAree = oRequest.GetValueFromParameter(30)
                Me.n_PRSN_AUTN_ID = oRequest.GetValueFromParameter(31)
                Me.n_PRSN_AUTN_RemoteUniqueID = oRequest.GetValueFromParameter(32)
                Me.n_PRSN_ORGNDefault_id = oRequest.GetValueFromParameter(33)
                Me.n_PRSN_ORGNDefault_ragioneSociale = oRequest.GetValueFromParameter(34)
                Me.n_PRSN_TPPR.Descrizione = oRequest.GetValueFromParameter(35)
                Me.n_PRSN_dataInserimento = oRequest.GetValueFromParameter(36)
                Me.n_PRSN_mostraMail = oRequest.GetValueFromParameter(37)
                Me.n_PRSN_Ricevimento = oRequest.GetValueFromParameter(38)
                Me.n_PRSN_MailSecondaria = oRequest.GetValueFromParameter(39)
                ' 40 = mail secondaria
                Me.n_Sector = oRequest.GetValueFromParameter(41)
                Me.n_Job = oRequest.GetValueFromParameter(42)


            Catch ax As Exception
                Me.n_PRSN_id = -1
                Me.n_Errore = Errori_Db.DBReadExist
            End Try
        End Sub
        Public Function Elenca() As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_Elenca"
                .CommandType = CommandType.StoredProcedure
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

        Public Sub DeletePerson() '!!! per adesso serve solamente per cancellare la persona se
            'si verificano problemi di inserimento dello studente !!!
            'la SP è temporanea e NON cancella i dati associati!!!
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_Delete"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@PRSN_Id", n_PRSN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBDelete
            End Try
        End Sub
        Public Sub Aggiungi()
            Me.AggiungiPersona()
        End Sub
        Public Sub Modifica()
            Me.ModificaPersona()
        End Sub
        Protected Sub AggiungiPersona()
            'inserimento di una nuova persona
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim pwdCryptata As New COL_Encrypter
            Dim dataNascita As String = DateToString(Me.n_PRSN_dataNascita, False)
            Dim dataInserimento As String
            n_PRSN_dataInserimento = Now
            dataInserimento = DateToString(n_PRSN_dataInserimento)
            n_PRSN_pwd = pwdCryptata.Encrypt(n_PRSN_pwd)
            If n_PRSN_indirizzo Is Nothing Then
                n_PRSN_indirizzo = ""
            ElseIf String.IsNullOrEmpty(n_PRSN_indirizzo) Then
                n_PRSN_indirizzo = ""
            Else
                n_PRSN_indirizzo = n_PRSN_indirizzo.Trim
            End If
            With oRequest
                .Command = "sp_Persona_Aggiungi"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@PRSN_Id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_login", n_PRSN_login.Trim, ParameterDirection.Input, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Pwd", n_PRSN_pwd, ParameterDirection.Input, DbType.String, , 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_nome", n_PRSN_nome.Trim, ParameterDirection.Input, DbType.String, , 40)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_cognome", n_PRSN_cognome.Trim, ParameterDirection.Input, DbType.String, , 40)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_luogoNascita", n_PRSN_luogoNascita.Trim, ParameterDirection.Input, DbType.String, , 30)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_dataNascita", dataNascita, ParameterDirection.Input, DbType.String, , 30)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_sesso", n_PRSN_sesso, ParameterDirection.Input, DbType.Byte)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_codFiscale", n_PRSN_codFiscale.Trim, ParameterDirection.Input, DbType.String, , 16)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_indirizzo", n_PRSN_indirizzo, ParameterDirection.Input, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_cap", n_PRSN_cap.Trim, ParameterDirection.Input, DbType.String, , 10)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_citta", n_PRSN_citta.Trim, ParameterDirection.Input, DbType.String, , 30)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_PRVN_ID", n_PRSN_PRVN.ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_STTO_ID", n_PRSN_STTO.ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_telefono1", n_PRSN_telefono1, ParameterDirection.Input, DbType.String, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_cellulare", n_PRSN_cellulare, ParameterDirection.Input, DbType.String, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_telefono2", n_PRSN_telefono2, ParameterDirection.Input, DbType.String, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_fax", n_PRSN_fax, ParameterDirection.Input, DbType.String, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_mail", n_PRSN_mail, ParameterDirection.Input, DbType.String, , 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_homePage", n_PRSN_homePage, ParameterDirection.Input, DbType.String, , 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_invisible", n_PRSN_invisibile, ParameterDirection.Input, DbType.Boolean)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_TPPR_ID", n_PRSN_TPPR.ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_LNGU_ID", n_PRSN_LNGU.ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_note", n_PRSN_note, ParameterDirection.Input, DbType.String, True, 1000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_ISTT_ID", n_PRSN_ISTT.Id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_intEst", Me.n_PRSN_IsInterno, ParameterDirection.Input, DbType.Boolean)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_dataInserimento", dataInserimento, ParameterDirection.Input, DbType.String, , 30)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_ricezioneSMS", Me.n_PRSN_RicezioneSMS, ParameterDirection.Input, DbType.Byte)
                .Parameters.Add(oParam)
                'per nn avere casini inserisco come ultimo collegamento la data di inserimento!!!!
                oParam = objAccesso.GetParameter("@PRSN_UltimoCollegamento", dataInserimento, ParameterDirection.Input, DbType.String, , 30)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_AUTN_ID", Me.n_PRSN_AUTN_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_AUTN_RemoteUniqueID", Me.n_PRSN_AUTN_RemoteUniqueID, ParameterDirection.Input, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_mostraMail", Me.n_PRSN_mostraMail, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_Ricevimento", Me.n_PRSN_Ricevimento, ParameterDirection.Input, SqlDbType.VarChar, True, 500)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_MailSecondaria", Me.n_PRSN_MailSecondaria, ParameterDirection.Input, SqlDbType.VarChar, True, 255)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRSN_Sector", Me.n_Sector, ParameterDirection.Input, SqlDbType.VarChar, True, 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_Job", Me.n_Job, ParameterDirection.Input, SqlDbType.VarChar, True, 255)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                ' Recupero l'ID univoco della persona appena inserita
                Me.n_PRSN_id = oRequest.GetValueFromParameter(1)
            Catch ex As Exception
                Me.n_PRSN_id = -1
                Me.n_Errore = Errori_Db.DBInsert
            End Try
        End Sub
        Protected Sub ModificaPersona()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim pwdCryptata As New COL_Encrypter

            n_PRSN_pwd = pwdCryptata.Encrypt(n_PRSN_pwd)
            Dim dataNascita As String = (DateToString(Me.n_PRSN_dataNascita, False))
            With oRequest
                .Command = "sp_Persona_Modifica"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@PRSN_Id", n_PRSN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_login", n_PRSN_login.Trim, ParameterDirection.Input, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_PWD", n_PRSN_pwd, ParameterDirection.Input, DbType.String, , 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_nome", n_PRSN_nome.Trim, ParameterDirection.Input, DbType.String, , 40)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_cognome", n_PRSN_cognome.Trim, ParameterDirection.Input, DbType.String, , 40)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_luogoNascita", n_PRSN_luogoNascita.Trim, ParameterDirection.Input, DbType.String, , 30)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_dataNascita", dataNascita, ParameterDirection.Input, DbType.String, , 30)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_sesso", n_PRSN_sesso, ParameterDirection.Input, DbType.Byte)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_codFiscale", n_PRSN_codFiscale.Trim, ParameterDirection.Input, DbType.String, , 16)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_indirizzo", n_PRSN_indirizzo.Trim, ParameterDirection.Input, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_cap", n_PRSN_cap.Trim, ParameterDirection.Input, DbType.String, , 10)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_citta", n_PRSN_citta.Trim, ParameterDirection.Input, DbType.String, , 30)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_PRVN_ID", n_PRSN_PRVN.ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_STTO_ID", n_PRSN_STTO.ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_telefono1", n_PRSN_telefono1, ParameterDirection.Input, DbType.String, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_cellulare", n_PRSN_cellulare, ParameterDirection.Input, DbType.String, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_telefono2", n_PRSN_telefono2, ParameterDirection.Input, DbType.String, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_fax", n_PRSN_fax, ParameterDirection.Input, DbType.String, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_mail", n_PRSN_mail, ParameterDirection.Input, DbType.String, , 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_homePage", n_PRSN_homePage, ParameterDirection.Input, DbType.String, , 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_fotoPath", n_PRSN_fotoPath, ParameterDirection.Input, DbType.String, , 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_invisible", n_PRSN_invisibile, ParameterDirection.Input, DbType.Boolean)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_TPPR_ID", n_PRSN_TPPR.ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_LNGU_ID", n_PRSN_LNGU.ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_note", n_PRSN_note, ParameterDirection.Input, DbType.String, True, 1000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_ISTT_ID", n_PRSN_ISTT.Id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_intEst", Me.n_PRSN_IsInterno, ParameterDirection.Input, DbType.Boolean)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_maxAree", Me.n_PRSN_maxAree, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_AUTN_ID", Me.n_PRSN_AUTN_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_AUTN_RemoteUniqueID", Me.n_PRSN_AUTN_RemoteUniqueID, ParameterDirection.Input, DbType.String, , 50)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_mostraMail", Me.n_PRSN_mostraMail, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRSN_Ricevimento", Me.n_PRSN_Ricevimento, ParameterDirection.Input, SqlDbType.VarChar, True, 500)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRSN_MailSecondaria", Me.n_PRSN_MailSecondaria, ParameterDirection.Input, SqlDbType.VarChar, True, 255)
                .Parameters.Add(oParam)


                oParam = objAccesso.GetParameter("@PRSN_Sector", n_Sector, ParameterDirection.Input, DbType.String, True, 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Job", n_Job, ParameterDirection.Input, DbType.String, True, 255)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBReadExist
            End Try
        End Sub

        Public Sub CancellaTipoPrecedente(ByVal TipoPersona As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_CancellaTipoPrecedente"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@PRSN_Id", n_PRSN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TipoPersona", TipoPersona, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBDelete
            End Try
        End Sub

        Public Sub SalvaImpostazioneLingua(ByVal LinguaID As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_SalvaImpostazioneLingua"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@PRSN_Id", n_PRSN_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBDelete
            End Try
        End Sub

#End Region

#Region "Metodi Gestione Persone"

        Public Sub EstraiFromRuoloPersonaComunita(ByVal RLPC_ID As Integer)
            'carica i campi del db nell'oggetto in base all'id dell' RLPC
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Persona_Estrai_from_RLPC"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@RLPC_id", RLPC_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Nome", "", ParameterDirection.Output, DbType.String, False, 40)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Cognome", "", ParameterDirection.Output, DbType.String, False, 40)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_indirizzo", "", ParameterDirection.Output, DbType.String, False, 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_cap", "", ParameterDirection.Output, DbType.String, False, 10)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_citta", "", ParameterDirection.Output, DbType.String, False, 30)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_PRVN_Id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Telefono1", "", ParameterDirection.Output, DbType.String, True, 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Telefono2", "", ParameterDirection.Output, DbType.String, True, 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Cellulare", "", ParameterDirection.Output, DbType.String, True, 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Fax", "", ParameterDirection.Output, DbType.String, True, 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Mail", "", ParameterDirection.Output, DbType.String, False, 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_HomePage", "", ParameterDirection.Output, DbType.String, False, 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_FotoPath", "", ParameterDirection.Output, DbType.String, True, 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_login", "", ParameterDirection.Output, DbType.String, False, 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_dataNascita", "", ParameterDirection.Output, DbType.DateTime)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_luogoNascita", "", ParameterDirection.Output, DbType.String, False, 30)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_sesso", "", ParameterDirection.Output, DbType.Byte)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_codFiscale", "", ParameterDirection.Output, DbType.String, False, 16)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_STTO_Id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_LNGU_Id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_ISTT_Id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_note", "", ParameterDirection.Output, DbType.String, True, 1000)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_dataInserimento", "", ParameterDirection.Output, DbType.DateTime)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_TPPR_id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_RicezioneSMS", "", ParameterDirection.Output, DbType.Byte)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_maxAree", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_AUTN_ID", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_AUTN_RemoteUniqueID", "", ParameterDirection.Output, DbType.String, , 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_invisible", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_mostraMail", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_Ricevimento", "", ParameterDirection.Output, SqlDbType.VarChar, True, 500)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_MailSecondaria", "", ParameterDirection.Output, SqlDbType.VarChar, True, 255)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_Sector", "", ParameterDirection.Output, DbType.String, , 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Job", "", ParameterDirection.Output, DbType.String, , 255)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_PRSN_id = oRequest.GetValueFromParameter(2)
                Me.n_PRSN_nome = oRequest.GetValueFromParameter(3)
                Me.n_PRSN_cognome = oRequest.GetValueFromParameter(4)
                Me.n_PRSN_indirizzo = oRequest.GetValueFromParameter(5)
                Me.n_PRSN_cap = oRequest.GetValueFromParameter(6)
                Me.n_PRSN_citta = oRequest.GetValueFromParameter(7)
                Me.n_PRSN_PRVN.ID = oRequest.GetValueFromParameter(8)
                Me.n_PRSN_telefono1 = oRequest.GetValueFromParameter(9)
                Me.n_PRSN_telefono2 = oRequest.GetValueFromParameter(10)
                Me.n_PRSN_cellulare = oRequest.GetValueFromParameter(11)
                Me.n_PRSN_fax = oRequest.GetValueFromParameter(12)
                Me.n_PRSN_mail = oRequest.GetValueFromParameter(13)
                Me.n_PRSN_homePage = oRequest.GetValueFromParameter(14)
                Me.n_PRSN_fotoPath = oRequest.GetValueFromParameter(15)
                Me.n_PRSN_login = oRequest.GetValueFromParameter(16)
                Me.n_PRSN_dataNascita = oRequest.GetValueFromParameter(17)
                Me.n_PRSN_luogoNascita = oRequest.GetValueFromParameter(18)
                Me.n_PRSN_sesso = oRequest.GetValueFromParameter(19)
                Me.n_PRSN_codFiscale = oRequest.GetValueFromParameter(20)
                Me.n_PRSN_STTO.ID = oRequest.GetValueFromParameter(21)
                Me.n_PRSN_LNGU.ID = oRequest.GetValueFromParameter(22)
                Me.n_PRSN_ISTT.Id = oRequest.GetValueFromParameter(23)
                Me.n_PRSN_note = oRequest.GetValueFromParameter(24)
                Me.n_PRSN_dataInserimento = oRequest.GetValueFromParameter(25)
                Me.n_PRSN_TPPR.ID = oRequest.GetValueFromParameter(26)
                Me.n_PRSN_RicezioneSMS = oRequest.GetValueFromParameter(27)
                Me.n_PRSN_maxAree = oRequest.GetValueFromParameter(28)
                Me.n_PRSN_AUTN_ID = oRequest.GetValueFromParameter(29)
                Me.n_PRSN_AUTN_RemoteUniqueID = oRequest.GetValueFromParameter(30)
                Me.n_PRSN_invisibile = oRequest.GetValueFromParameter(31)
                Me.n_PRSN_mostraMail = oRequest.GetValueFromParameter(32)
                Me.n_PRSN_Ricevimento = oRequest.GetValueFromParameter(33)
                Me.n_PRSN_MailSecondaria = oRequest.GetValueFromParameter(34)

                Me.n_Sector = oRequest.GetValueFromParameter(35)
                Me.n_Job = oRequest.GetValueFromParameter(36)

            Catch ax As Exception
                Me.n_PRSN_id = -1
                Me.n_Errore = Errori_Db.DBReadExist
            End Try
        End Sub

        Public Function GetPersonePaginateByIstituzione(ByVal ISTT_ID As Integer, ByVal pPAGE_SIZE As Integer, ByVal pPAGE_INDEX As Integer, ByVal PRSN_TPPR_id As Integer, ByVal ORGN_ID As Integer, ByVal oFiltroAnagrafica As Main.FiltroAnagrafica, ByVal oFiltroOrdinamento As Main.FiltroOrdinamento, ByVal oCampoOrdinePersona As Main.FiltroCampoOrdinePersona, Optional ByVal oFiltroPersona As Main.FiltroPersona = Main.FiltroPersona.tutte, Optional ByVal valore As String = "", Optional ByVal tipoAutenticazione As Integer = Main.TipoAutenticazione.Tutti, Optional ByVal oTipoAttivazione As TipoAttivazione = Main.TipoAttivazione.Tutti) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_GetPersonePaginateByIstituzione"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@ISTT_ID", ISTT_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_TPPR_ID", PRSN_TPPR_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PAGE_SIZE", pPAGE_SIZE, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@ANAGRAFICA", CType(oFiltroAnagrafica, Main.FiltroAnagrafica), , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PAGE_INDEX", pPAGE_INDEX, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@ORDINE", CType(oCampoOrdinePersona, Main.FiltroCampoOrdinePersona), , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@ORDINAMENTO", CType(oFiltroOrdinamento, Main.FiltroOrdinamento), , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@ORGN_ID", ORGN_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@FiltroPersona", CType(oFiltroPersona, Main.FiltroPersona), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@valore", valore, ParameterDirection.Input, DbType.String, , 300)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@TipoAttivazione", CType(oTipoAttivazione, Main.TipoAttivazione), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@AUTN_Id", tipoAutenticazione, ParameterDirection.Input, DbType.Int32)
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
        Public Function GetPersoneByCompetenza(ByVal ARCP_ID As Integer, ByVal ISTT_ID As Integer, ByVal pPAGE_SIZE As Integer, ByVal pPAGE_INDEX As Integer, ByVal PRSN_TPPR_id As Integer, ByVal ORGN_ID As Integer, ByVal oFiltroAnagrafica As Main.FiltroAnagrafica, ByVal oFiltroOrdinamento As Main.FiltroOrdinamento, ByVal oCampoOrdinePersona As Main.FiltroCampoOrdinePersona, Optional ByVal oFiltroPersona As Main.FiltroPersona = Main.FiltroPersona.tutte, Optional ByVal valore As String = "", Optional ByVal tipoAutenticazione As Integer = Main.TipoAutenticazione.Tutti, Optional ByVal oTipoAttivazione As TipoAttivazione = Main.TipoAttivazione.Tutti) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_GetPersoneByCompetenza"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@ISTT_ID", ISTT_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@ARCP_ID", ARCP_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_TPPR_ID", PRSN_TPPR_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PAGE_SIZE", pPAGE_SIZE, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@ANAGRAFICA", CType(oFiltroAnagrafica, Main.FiltroAnagrafica), , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PAGE_INDEX", pPAGE_INDEX, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@ORDINE", CType(oCampoOrdinePersona, Main.FiltroCampoOrdinePersona), , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@ORDINAMENTO", CType(oFiltroOrdinamento, Main.FiltroOrdinamento), , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@ORGN_ID", ORGN_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@FiltroPersona", CType(oFiltroPersona, Main.FiltroPersona), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@valore", valore, ParameterDirection.Input, DbType.String, , 300)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@TipoAttivazione", CType(oTipoAttivazione, Main.TipoAttivazione), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@AUTN_Id", tipoAutenticazione, ParameterDirection.Input, DbType.Int32)
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

        Public Function GetOrganizzazioniAssociate(Optional ByVal abilitato As Boolean = False, Optional ByVal isFacoltà As Boolean = False) As DataSet
            'elenca gli id delle organizzazioni a cui una persona è associata
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_OrganizzazioniAssociate"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@PRSN_id", Me.n_PRSN_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                If abilitato Then
                    oParam = objAccesso.GetAdvancedParameter("@abilitato", 1, ParameterDirection.Input, SqlDbType.Int)
                Else
                    oParam = objAccesso.GetAdvancedParameter("@abilitato", 0, ParameterDirection.Input, SqlDbType.Int)
                End If
                .Parameters.Add(oParam)

                If isFacoltà Then
                    oParam = objAccesso.GetAdvancedParameter("@isFacolta", 1, ParameterDirection.Input, SqlDbType.Int)
                Else
                    oParam = objAccesso.GetAdvancedParameter("@isFacolta", 0, ParameterDirection.Input, SqlDbType.Int)
                End If
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
        Public Function GetOrganizzazioniByIstituzione(ByVal ISTT_ID As Integer, Optional ByVal isFacolta As Boolean = False) As DataSet
            'elenca gli id delle organizzazioni a cui una persona è associata
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_OrganizzazioniAssociateByIstituzione"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@PRSN_id", Me.n_PRSN_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@ISTT_ID", ISTT_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                If isFacolta Then
                    oParam = objAccesso.GetAdvancedParameter("@isFacolta", 1, ParameterDirection.Input, SqlDbType.Int)
                Else
                    oParam = objAccesso.GetAdvancedParameter("@isFacolta", 0, ParameterDirection.Input, SqlDbType.Int)
                End If
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

        Public Function GetOrganizzazioneDefault() As Integer
            'restituisce l'id dell'organizzazione di default di una persona
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim ORGN_ID As Integer

            With oRequest
                .Command = "sp_Persona_GetOrganizzazioneDefault"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@ORGN_Id", " ", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Id", Me.n_PRSN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                ' Recupero l'ID univoco dell'organizzazione.
                ORGN_ID = oRequest.GetValueFromParameter(1)
                Me.n_Errore = Errori_Db.None


            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
                ORGN_ID = -1
            End Try
            Return ORGN_ID
        End Function

        Public Sub AssociaComunitaRuoloDefault(ByVal CMNT_ID As Integer, ByVal RicezioneSMS As Boolean, ByVal Attivato As Boolean, ByVal Abilitato As Boolean, Optional ByVal isResponsabile As Boolean = True)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim iResponse As Integer

            With oRequest
                .Command = "sp_Persona_AssociaComunitaRuoloDefault"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@RLPC_Id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_CMNT_Id", CMNT_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_PRSN_ID", Me.n_PRSN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                If Attivato Then
                    oParam = objAccesso.GetParameter("@RLPC_attivato", 1, ParameterDirection.Input, DbType.Byte)
                Else
                    oParam = objAccesso.GetParameter("@RLPC_attivato", 0, ParameterDirection.Input, DbType.Byte)
                End If
                .Parameters.Add(oParam)

                If Abilitato Then
                    oParam = objAccesso.GetParameter("@RLPC_abilitato", 1, ParameterDirection.Input, DbType.Byte)
                Else
                    oParam = objAccesso.GetParameter("@RLPC_abilitato", 0, ParameterDirection.Input, DbType.Byte)
                End If
                .Parameters.Add(oParam)

                If isResponsabile Then
                    oParam = objAccesso.GetParameter("@RLPC_responsabile", 1, ParameterDirection.Input, DbType.Byte)
                Else
                    oParam = objAccesso.GetParameter("@RLPC_responsabile", 0, ParameterDirection.Input, DbType.Byte)
                End If
                .Parameters.Add(oParam)

                If RicezioneSMS Then
                    oParam = objAccesso.GetParameter("@RLPC_RicezioneSMS", 1, ParameterDirection.Input, DbType.Byte)
                Else
                    oParam = objAccesso.GetParameter("@RLPC_RicezioneSMS", 0, ParameterDirection.Input, DbType.Byte)
                End If
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_UltimoCollegamento", DateToString(Now, True), ParameterDirection.Input, DbType.String, , 30)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False

            End With
            Try
                iResponse = objAccesso.GetExecuteNotQuery(oRequest)
                PurgeCacheItems(CachePolicy.PermessiServizioUtente, "_" & Me.n_PRSN_id.ToString)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception

                Me.n_Errore = Errori_Db.DBInsert
            End Try
        End Sub
        Public Sub AssociaComunita(ByVal TPRL_ID As Integer, ByVal CMNT_ID As Integer, ByVal RicezioneSMS As Boolean, ByVal Attivato As Boolean, ByVal Abilitato As Boolean, Optional ByVal isResponsabile As Boolean = True)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim iResponse As Integer

            With oRequest
                .Command = "sp_Persona_AssociaComunita"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@RLPC_Id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_CMNT_Id", CMNT_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_PRSN_ID", Me.n_PRSN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_TPRL_id", TPRL_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                If Attivato Then
                    oParam = objAccesso.GetParameter("@RLPC_attivato", 1, ParameterDirection.Input, DbType.Byte)
                Else
                    oParam = objAccesso.GetParameter("@RLPC_attivato", 0, ParameterDirection.Input, DbType.Byte)
                End If
                .Parameters.Add(oParam)

                If Abilitato Then
                    oParam = objAccesso.GetParameter("@RLPC_abilitato", 1, ParameterDirection.Input, DbType.Byte)
                Else
                    oParam = objAccesso.GetParameter("@RLPC_abilitato", 0, ParameterDirection.Input, DbType.Byte)
                End If
                .Parameters.Add(oParam)

                If isResponsabile Then
                    oParam = objAccesso.GetParameter("@RLPC_responsabile", 1, ParameterDirection.Input, DbType.Byte)
                Else
                    oParam = objAccesso.GetParameter("@RLPC_responsabile", 0, ParameterDirection.Input, DbType.Byte)
                End If
                .Parameters.Add(oParam)

                If RicezioneSMS Then
                    oParam = objAccesso.GetParameter("@RLPC_RicezioneSMS", 1, ParameterDirection.Input, DbType.Byte)
                Else
                    oParam = objAccesso.GetParameter("@RLPC_RicezioneSMS", 0, ParameterDirection.Input, DbType.Byte)
                End If
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_UltimoCollegamento", DateToString(Now, True), ParameterDirection.Input, DbType.String, , 30)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False

            End With
            Try
                iResponse = objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception

                Me.n_Errore = Errori_Db.DBInsert
            End Try
        End Sub

        Public Function GetIDRuoloForComunita(ByVal CMNT_ID As Integer) As Integer
            'in base alla comunità e all'id della persona mi ritorna il ruolo
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim RLPC_ID As Integer

            With oRequest
                .Command = "sp_RuoloPersonaComunitaGetRuolo"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@RLPC_PRSN_id", Me.n_PRSN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_CMNT_id", CMNT_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@RLPC_TPRL_id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                RLPC_ID = oRequest.GetValueFromParameter(3)
                Me.n_Errore = Errori_Db.None
            Catch ax As Exception 'record non trovato
                RLPC_ID = -1
                Me.n_Errore = Errori_Db.DBReadExist
            End Try
            Return RLPC_ID
        End Function

        Public Function GetRuoloPersonaComunita(ByVal CMNT_ID As Integer) As COL_RuoloPersonaComunita
            'in base alla comunità e all'id della persona mi ritorna il ruolo
            Dim oRuoloPersonaComunita As New COL_RuoloPersonaComunita

            Try
                oRuoloPersonaComunita.EstraiByLinguaDefault(CMNT_ID, Me.n_PRSN_id)
                Me.n_Errore = Errori_Db.None
            Catch ax As Exception 'record non trovato

                Me.n_Errore = Errori_Db.DBReadExist
            End Try
            Return oRuoloPersonaComunita
        End Function

        Public Function GetUtentiConnessi(ByVal PRSN_ID As Integer, ByVal pPAGE_SIZE As Integer, ByVal pPAGE_INDEX As Integer, ByVal PRSN_TPPR_id As Integer, ByVal ORGN_ID As Integer, ByVal oFiltroAnagrafica As Main.FiltroAnagrafica, ByVal oFiltroOrdinamento As Main.FiltroOrdinamento, ByVal oCampoOrdinePersona As Main.FiltroCampoOrdinePersona, Optional ByVal oFiltroPersona As Main.FiltroPersona = Main.FiltroPersona.tutte, Optional ByVal valore As String = "", Optional ByVal tipoAutenticazione As Integer = Main.TipoAutenticazione.Tutti, Optional ByVal ISTT_ID As Integer = -1) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_GetUtentiConnessi"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@ISTT_ID", ISTT_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_TPPR_ID", PRSN_TPPR_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_ID", PRSN_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PAGE_SIZE", pPAGE_SIZE, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@ANAGRAFICA", CType(oFiltroAnagrafica, Main.FiltroAnagrafica), , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PAGE_INDEX", pPAGE_INDEX, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@ORDINE", CType(oCampoOrdinePersona, Main.FiltroCampoOrdinePersona), , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@ORDINAMENTO", CType(oFiltroOrdinamento, Main.FiltroOrdinamento), , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@ORGN_ID", ORGN_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@FiltroPersona", CType(oFiltroPersona, Main.FiltroPersona), ParameterDirection.Input, DbType.String, True, 10)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@valore", valore, ParameterDirection.Input, DbType.String, True, 15)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@AUTN_Id", tipoAutenticazione, ParameterDirection.Input, DbType.Int32)
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


        ' Disattivato
#Region "Disattivato"
        'Public Function HasNewsForComunita(ByVal IsConnesso As Boolean, ByVal CMNT_ID As Integer, ByVal status As Main.FiltroStatoComunita) As DataSet
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim oDataSet As New DataSet
        '    Dim objAccesso As New COL_DataAccess

        '    With oRequest
        '        .Command = "sp_Persona_HasNews_Comunita"
        '        .CommandType = CommandType.StoredProcedure

        '        oParam = objAccesso.GetAdvancedParameter("@PRSN_ID", Me.n_PRSN_id, ParameterDirection.Input, SqlDbType.Int)
        '        .Parameters.Add(oParam)

        '        If IsConnesso Then
        '            oParam = objAccesso.GetAdvancedParameter("@IsConnesso", 1, ParameterDirection.Input, SqlDbType.Int)
        '        Else
        '            oParam = objAccesso.GetAdvancedParameter("@IsConnesso", 0, ParameterDirection.Input, SqlDbType.Int)
        '        End If
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", CMNT_ID, ParameterDirection.Input, SqlDbType.Int)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetAdvancedParameter("@status", CType(status, Main.FiltroStatoComunita), ParameterDirection.Input, SqlDbType.Int)
        '        .Parameters.Add(oParam)


        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With

        '    Try
        '        oDataSet = objAccesso.GetdataSet(oRequest)
        '    Catch ex As Exception
        '        Me.n_Errore = Errori_Db.DBError
        '    End Try
        '    Return oDataSet
        'End Function
#End Region

        'Public Function GetNewsForComunita_(ByVal CMNT_ID As Integer) As DataSet
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim oDataSet As DataSet
        '    Dim objAccesso As New COL_DataAccess

        '    With oRequest
        '        .Command = "sp_PersonaGetNewsForComunita"
        '        .CommandType = CommandType.StoredProcedure

        '        oParam = objAccesso.GetParameter("@PRSN_ID", Me.n_PRSN_id, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@CMNT_ID", CMNT_ID, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With

        '    Try
        '        oDataSet = objAccesso.GetdataSet(oRequest)
        '    Catch ex As Exception
        '        Me.n_Errore = Errori_Db.DBError
        '    End Try
        '    Return oDataSet
        'End Function
#End Region

        ' DA SISTEMARE IN APPOSITE REGION

        Public Sub blocca(ByVal PRSN_ID As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Persona_bloccasblocca"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@PRSN_id", PRSN_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBChange
            End Try
        End Sub
        Public Sub blocca(ByVal PRSN_ID As Integer, ByVal invisible As Boolean)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Persona_bloccasblocca"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@PRSN_id", PRSN_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                If invisible Then
                    oParam = objAccesso.GetParameter("@PRSN_invisible", 1, ParameterDirection.Input, DbType.Int32)
                Else
                    oParam = objAccesso.GetParameter("@PRSN_invisible", 0, ParameterDirection.Input, DbType.Int32)
                End If
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBChange
            End Try
        End Sub
        Public Shared Function GetLinguaID(ByVal PersonaID As Integer) As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Persona_GetLinguaID"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@PRSN_id", PersonaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@LNGU_ID", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Return oRequest.GetValueFromParameter(2)
            Catch ax As Exception
                Return -1
            End Try
        End Function

        Private Sub MailAttivazione(ByVal oLocalizedMail As MailLocalized)
            Dim oMail As New COL_E_Mail(oLocalizedMail)

            Try
                oMail.Mittente = oLocalizedMail.SystemSender
                oMail.IndirizziTO.Add(New MailAddress(Me.n_PRSN_mail))
                oMail.IndirizziCCN.Add(oLocalizedMail.SystemSender)


                oMail.Oggetto = oLocalizedMail.NotificationMessages(NotificationMessage.NotificationType.ConfermaIscrizionePortale).Subject

                If Hour(Now) <= 12 Then
                    oMail.Body = oLocalizedMail.NotificationMessages(NotificationMessage.NotificationType.Hour0).Message
                ElseIf Hour(Now) > 12 And Hour(Now) < 18 Then
                    oMail.Body = oLocalizedMail.NotificationMessages(NotificationMessage.NotificationType.Hour12).Message
                Else
                    oMail.Body = oLocalizedMail.NotificationMessages(NotificationMessage.NotificationType.Hour24).Message
                End If
                oMail.Body = oMail.Body & vbCrLf & oLocalizedMail.NotificationMessages(NotificationMessage.NotificationType.ConfermaIscrizionePortale).Message & vbCrLf & vbCrLf & vbCrLf
                oMail.Body = Replace(oMail.Body, "#data#", FormatDateTime(Now, DateFormat.LongDate))
                oMail.Body = oMail.Body & vbCrLf & vbCrLf & vbCrLf & oLocalizedMail.SystemFirmaNotifica
                oMail.Body = Replace(oMail.Body, "<br>", vbCrLf)
                oMail.InviaMail()

            Catch ex As Exception

            End Try
        End Sub
        Public Function Aggiungi_InAttesaAttivazione(ByVal SessionId As String, ByVal PRSN_ID As Integer, ByVal Code_Id As String) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_AggiungiInAttesaAttivazione"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@Code_Id", Code_Id, ParameterDirection.Input, DbType.String, False, 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_ID", PRSN_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@SessionId", SessionId, ParameterDirection.Input, DbType.String, False, 250)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_Errore = Errori_Db.None
                Return True
            Catch ex As Exception
                Return False
            End Try
            Return False
        End Function
        Public Function Rimuovi_InAttesaAttivazione(ByVal SessionId As String, ByVal PRSN_ID As Integer, ByVal Code_Id As String, ByVal oLocalizedMail As MailLocalized) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_RimuoviInAttesaAttivazione"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@Code_Id", Code_Id, ParameterDirection.Input, DbType.String, False, 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_ID", PRSN_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@SessionId", SessionId, ParameterDirection.Input, DbType.String, False, 250)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@found", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@LinguaCode", "", ParameterDirection.Output, DbType.String, False, 10)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_Errore = Errori_Db.None
                If oRequest.GetValueFromParameter(4) = 0 Then
                    Return False
                Else
                    'oLocalizedMail.ReloadByLanguage(oRequest.GetValueFromParameter(5))
                    oLocalizedMail = ManagerConfiguration.GetMailLocalized(ManagerLingua.GetByCodeOrDefault(oRequest.GetValueFromParameter(5)))
                    Me.MailAttivazione(oLocalizedMail)
                    Return True
                End If

            Catch ex As Exception
                Return False
            End Try
            Return False
        End Function
        'Public Function AttivaIscrizione_()
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim objAccesso As New COL_DataAccess

        '    With oRequest
        '        .Command = "sp_Persona_AttivaIscrizione"
        '        .CommandType = CommandType.StoredProcedure

        '        oParam = objAccesso.GetParameter("@PRSN_id", Me.n_PRSN_id, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        objAccesso.GetExecuteNotQuery(oRequest)
        '        Me.n_Errore = Errori_Db.None
        '    Catch ex As Exception
        '        Me.n_Errore = Errori_Db.DBChange
        '    End Try
        'End Function
        Public Function AttivaIscrizioneErimuoviInAttesa(ByVal oLocalizedMail As MailLocalized) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_AttivaIscrizioneErimuoviInAtesa"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@PRSN_id", Me.n_PRSN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@found", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@LinguaCode", "", ParameterDirection.Output, DbType.String, False, 10)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)

                If oRequest.GetValueFromParameter(2) = 0 Then
                    Return False
                Else
                    oLocalizedMail = ManagerConfiguration.GetMailLocalized(ManagerLingua.GetByCodeOrDefault(oRequest.GetValueFromParameter(3)))
                    'oLocalizedMail.ReloadByLanguage(oRequest.GetValueFromParameter(3))
                    Me.MailAttivazione(oLocalizedMail)
                    Return True
                End If
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBChange
            End Try
        End Function
        Public Shared Function generaPasswordNumerica(ByVal MaxChar As Integer) As String
            Dim i, codice As Integer
            Dim cifra As String = ""
            ' differenza tra estremo sup e inf
            For i = 1 To MaxChar
                Randomize()
                codice = CInt(Int((9 * Rnd())))
                cifra = cifra & codice
            Next
            generaPasswordNumerica = cifra
        End Function
        Public Function isInattesaAttivazione() As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_isInattesaAttivazione"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@PRSN_id", Me.n_PRSN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@inAttesa", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)

                Me.n_Errore = Errori_Db.None
                If oRequest.GetValueFromParameter(2) = 0 Then
                    Return False
                Else
                    Return True
                End If
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBChange
                Return False
            End Try
        End Function


        Public Shared Function AttivaCambioMail(ByVal SessionID As String, ByVal PersonaID As Integer, ByVal isPrimaria As Boolean) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim totale As Integer

            With oRequest
                .Command = "sp_Persona_AttivaCambioMail"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@PersonaID", PersonaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@SessionId", SessionID, ParameterDirection.Input, SqlDbType.VarChar, False, 250)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@totale", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                If isPrimaria Then
                    oParam = objAccesso.GetAdvancedParameter("@isPrimaria", 1, ParameterDirection.Input, SqlDbType.Int)
                Else
                    oParam = objAccesso.GetAdvancedParameter("@isPrimaria", 0, ParameterDirection.Input, SqlDbType.Int)
                End If
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                totale = oRequest.GetValueFromParameter(3)
                If totale = 0 Then
                    Return False
                Else
                    Return True
                End If
            Catch ex As Exception

            End Try
            Return False
        End Function


        Public Function Aggiungi_InAttesaConfermaMail(ByVal SessionId As String, ByVal Mail As String, ByVal isPrimaria As Boolean) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_AggiungiInAttesaAttivazioneMail"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@PRSN_ID", Me.n_PRSN_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@SessionId", SessionId, ParameterDirection.Input, SqlDbType.VarChar, False, 250)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@Mail", Mail, ParameterDirection.Input, SqlDbType.VarChar, False, 255)
                .Parameters.Add(oParam)
                If isPrimaria Then
                    oParam = objAccesso.GetAdvancedParameter("@isPrimaria", 1, ParameterDirection.Input, SqlDbType.Int)
                Else
                    oParam = objAccesso.GetAdvancedParameter("@isPrimaria", 0, ParameterDirection.Input, SqlDbType.Int)
                End If
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_Errore = Errori_Db.None
                Return True
            Catch ex As Exception
                Return False
            End Try
            Return False
        End Function

        Public Shared Function CriptaParametriAttivazione(ByVal PRSN_Id As Integer, ByVal LimboID As String, ByVal PwdRandom As String) As String
            Dim RandomCode As String
            Dim Link As String
            RandomCode = COL_Persona.generaPasswordNumerica(8)

            Link = "AddCode=" & RandomCode & PwdRandom

            RandomCode = COL_Persona.generaPasswordNumerica(4)
            Link = Link & "&ExpUrl=t" & RandomCode & PRSN_Id

            RandomCode = COL_Persona.generaPasswordNumerica(5)
            Link = Link & "&ExpUrl2=j" & RandomCode & LimboID

            Return Link
        End Function

#Region "Metodi Logon"

        Public Sub LogonForWebService()
            Dim oTipoPersona As New COL_TipoPersona
            Dim oLingua As New Lingua
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim pwdCryptata As New COL_Encrypter
            n_PRSN_pwd = pwdCryptata.Encrypt(n_PRSN_pwd)

            With oRequest
                .Command = "sp_Persona_LogonForWebService"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@PRSN_id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_login", n_PRSN_login.Trim, ParameterDirection.Input, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_PWD", n_PRSN_pwd, ParameterDirection.Input, DbType.String, , 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_nome", "", ParameterDirection.Output, DbType.String, False, 40)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_cognome", "", ParameterDirection.Output, DbType.String, False, 40)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPPR_descrizione", "", ParameterDirection.Output, DbType.String, False, 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPPR_id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_intEst", "", ParameterDirection.Output, DbType.Boolean)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LNGU_id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Mail", "", ParameterDirection.Output, DbType.String, False, 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_ISTT", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_invisible", "", ParameterDirection.Output, DbType.Boolean)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_AUTN_ID", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_AUTN_RemoteUniqueID", "", ParameterDirection.Output, DbType.String, , 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_mostraMail", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_Ricevimento", "", ParameterDirection.Output, SqlDbType.VarChar, True, 500)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                ID = objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_PRSN_id = oRequest.GetValueFromParameter(1)
                If Me.n_PRSN_id > 0 Then
                    ID = oRequest.GetValueFromParameter(1)
                    Me.n_PRSN_nome = oRequest.GetValueFromParameter(4)
                    Me.n_PRSN_cognome = oRequest.GetValueFromParameter(5)
                    oTipoPersona.Descrizione = oRequest.GetValueFromParameter(6)
                    oTipoPersona.ID = oRequest.GetValueFromParameter(7)
                    Me.n_PRSN_TPPR = oTipoPersona

                    Me.n_PRSN_IsInterno = oRequest.GetValueFromParameter(8)
                    oLingua.ID = oRequest.GetValueFromParameter(9)
                    Me.n_PRSN_LNGU = oLingua
                    Me.n_PRSN_mail = oRequest.GetValueFromParameter(10)
                    Me.n_PRSN_ISTT.Id = oRequest.GetValueFromParameter(11)
                    Me.n_PRSN_invisibile = oRequest.GetValueFromParameter(12)
                    Me.n_PRSN_AUTN_ID = oRequest.GetValueFromParameter(13)
                    Me.n_PRSN_AUTN_RemoteUniqueID = oRequest.GetValueFromParameter(14)
                    Me.n_PRSN_mostraMail = oRequest.GetValueFromParameter(15)
                    Me.n_PRSN_Ricevimento = oRequest.GetValueFromParameter(16)
                Else
                    Me.n_Errore = Errori_Db.DBReadExist
                End If
            Catch ex As Exception
                Me.n_PRSN_id = -1
                Me.n_Errore = Errori_Db.DBReadExist
            End Try
        End Sub

        Public Function Verifica() As Integer ' restituisce:
            ' -1 --> login già in uso da parte di un altro utente
            ' -2 --> codice fiscale già in uso da parte di un altro utente
            ' -3 --> email già in uso da parte di un altro utente
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim codice As Integer
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Persona_Verifica"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@codice", codice, ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_codFiscale", n_PRSN_codFiscale, ParameterDirection.Input, DbType.String, , 16)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Login", n_PRSN_login.Trim, ParameterDirection.Input, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_mail", Me.n_PRSN_mail, ParameterDirection.Input, DbType.String, , 255)
                .Parameters.Add(oParam)

                Try
                    oParam = objAccesso.GetParameter("@TPPR_ID", Me.TipoPersona.ID, ParameterDirection.Input, DbType.Int32)
                Catch ex As Exception
                    oParam = objAccesso.GetParameter("@TPPR_ID", -1, ParameterDirection.Input, DbType.Int32)
                End Try
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRSN_MailSecondaria", Me.n_PRSN_MailSecondaria, ParameterDirection.Input, SqlDbType.VarChar, True, 255)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                codice = oRequest.GetValueFromParameter(1)
            Catch
                'errore generico del db
                Me.n_Errore = Errori_Db.DBError
            End Try
            Return codice
        End Function
        Public Function VerificaForChange() As Integer  ' restituisce:
            ' -1 --> login già in uso da parte di un altro utente
            ' -2 --> codice fiscale già in uso da parte di un altro utente
            ' -3 --> email già in uso da parte di un altro utente
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim codice As Integer
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Persona_VerificaForChange"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@codice", codice, ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_codFiscale", n_PRSN_codFiscale, ParameterDirection.Input, SqlDbType.VarChar, , 16)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_Login", n_PRSN_login.Trim, ParameterDirection.Input, SqlDbType.VarChar, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_mail", Me.n_PRSN_mail, ParameterDirection.Input, SqlDbType.VarChar, , 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_id", Me.n_PRSN_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                Try
                    oParam = objAccesso.GetAdvancedParameter("@TPPR_ID", Me.TipoPersona.ID, ParameterDirection.Input, SqlDbType.Int)
                Catch ex As Exception
                    oParam = objAccesso.GetAdvancedParameter("@TPPR_ID", -1, ParameterDirection.Input, SqlDbType.Int)
                End Try
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_MailSecondaria", Me.n_PRSN_MailSecondaria, ParameterDirection.Input, SqlDbType.VarChar, True, 255)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                codice = oRequest.GetValueFromParameter(1)
            Catch
                'errore generico del db
                Me.n_Errore = Errori_Db.DBError
            End Try
            Return codice
        End Function
        Public Function VerificaForChangeMail(ByVal MailPrincipale As String, ByVal MailSecondaria As String) As Integer ' restituisce:
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim codice As Integer
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Persona_VerificaForChangeMail"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@codice", codice, ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_mail", MailPrincipale, ParameterDirection.Input, SqlDbType.VarChar, , 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_id", Me.n_PRSN_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRSN_MailSecondaria", MailSecondaria, ParameterDirection.Input, SqlDbType.VarChar, True, 255)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                codice = oRequest.GetValueFromParameter(1)
            Catch
                'errore generico del db
                Me.n_Errore = Errori_Db.DBError
            End Try
            Return codice
        End Function
        Public Sub Nascondi()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_Nascondi"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@PRSN_Id", n_PRSN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_invisibile", n_PRSN_invisibile, ParameterDirection.Input, DbType.Boolean)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBReadExist
            End Try
        End Sub

        '*** Benali ***
        Public Sub UpDateNomeFoto(Optional ByVal newpath As String = Nothing)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            'If newpath = "" Then
            '    newpath = null
            'End If
            With oRequest
                .Command = "sp_Persona_UpdatePath"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@PRSN_Id", Me.ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_fotoPath", newpath, ParameterDirection.Input, DbType.String, True, 255)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)

            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBReadExist
            End Try
        End Sub
        '*** Benali ***
        Public Sub AggiornaProfilo()
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim dataNascita As String = (DateToString(Me.n_PRSN_dataNascita, False))
            With oRequest
                .Command = "sp_persona_AggiornaProfilo"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@PRSN_Id", Me.ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Indirizzo", n_PRSN_indirizzo, ParameterDirection.Input, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Cap", n_PRSN_cap, ParameterDirection.Input, DbType.String, , 10)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Citta", n_PRSN_citta, ParameterDirection.Input, DbType.String, , 30)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_PRVN_ID", n_PRSN_PRVN.ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Telefono1", n_PRSN_telefono1, ParameterDirection.Input, DbType.String, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Telefono2", n_PRSN_telefono2, ParameterDirection.Input, DbType.String, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Cellulare", n_PRSN_cellulare, ParameterDirection.Input, DbType.String, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Fax", n_PRSN_fax, ParameterDirection.Input, DbType.String, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Mail", n_PRSN_mail, ParameterDirection.Input, DbType.String, , 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_HomePage", n_PRSN_homePage, ParameterDirection.Input, DbType.String, , 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_LNGU_id", Me.n_PRSN_LNGU.ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_mostraMail", Me.n_PRSN_mostraMail, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_dataNascita", dataNascita, ParameterDirection.Input, DbType.String, , 30)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_Ricevimento", Me.n_PRSN_Ricevimento, ParameterDirection.Input, SqlDbType.VarChar, True, 500)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRSN_MailSecondaria", Me.n_PRSN_MailSecondaria, ParameterDirection.Input, SqlDbType.VarChar, , 255)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBChange
            End Try
        End Sub
        Public Sub EstraiPwd(ByVal PRSN_LOGIN As String)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim PwdCritto As String
            With oRequest
                .Command = "sp_Persona_EstraiPwd_By_Login"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@PRSN_login", PRSN_LOGIN, ParameterDirection.Input, DbType.String, False, 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_pwd_Critto", "", ParameterDirection.Output, DbType.String, False, 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_id", "", ParameterDirection.Output, DbType.Int32, False)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Dim responso As String
            Try
                responso = objAccesso.GetExecuteNotQuery(oRequest)
                PwdCritto = oRequest.GetValueFromParameter(2)
                Dim pwdCryptata As New COL_Encrypter
                Me.n_PRSN_pwd = pwdCryptata.Decrypt(PwdCritto)
                Me.n_PRSN_id = oRequest.GetValueFromParameter(3)
                Me.n_PRSN_login = PRSN_LOGIN
                'Me.AggiornaProfilo()
            Catch ax As Exception
                Me.n_PRSN_id = -1
                responso = ""
            End Try
        End Sub
        '*** benali *** 
        Public Function EstraiPwd() As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Persona_EstraiPwd"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@PRSN_id", Me.ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Pwd", "", ParameterDirection.Output, DbType.String, False, 255)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Dim responso As Integer

            Try
                responso = objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_PRSN_pwd = oRequest.GetValueFromParameter(2)

            Catch ax As Exception
                Me.n_PRSN_id = -1
                responso = -1
            End Try

            Return responso
        End Function
        '**** Benali ****
        Public Function ModificaPassword(ByVal PRSN_Pwd As String) As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_ModificaPwd"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@PRSN_Id", Me.ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Pwd", PRSN_Pwd, ParameterDirection.Input, DbType.String, , 255)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            dsTable = objAccesso.GetExecuteNotQuery(oRequest)
            Return dsTable
        End Function


        Public Sub ModificaPersonaNoPwd()
            'modifica tutto tranne la password. serve per l'admin
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            Dim dataNascita As String = (DateToString(Me.n_PRSN_dataNascita, False))
            With oRequest
                .Command = "sp_Persona_ModificaNoPwd"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@PRSN_Id", n_PRSN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_login", n_PRSN_login.Trim, ParameterDirection.Input, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_nome", n_PRSN_nome, ParameterDirection.Input, DbType.String, , 40)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_cognome", n_PRSN_cognome, ParameterDirection.Input, DbType.String, , 40)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_luogoNascita", n_PRSN_luogoNascita, ParameterDirection.Input, DbType.String, , 30)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_dataNascita", dataNascita, , DbType.String, , 30)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_sesso", n_PRSN_sesso, ParameterDirection.Input, DbType.Byte)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_codFiscale", n_PRSN_codFiscale, ParameterDirection.Input, DbType.String, , 16)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_indirizzo", n_PRSN_indirizzo, ParameterDirection.Input, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_cap", n_PRSN_cap, ParameterDirection.Input, DbType.String, , 10)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_citta", n_PRSN_citta, ParameterDirection.Input, DbType.String, , 30)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_PRVN_ID", n_PRSN_PRVN.ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_STTO_ID", n_PRSN_STTO.ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_telefono1", n_PRSN_telefono1, ParameterDirection.Input, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_cellulare", n_PRSN_cellulare, ParameterDirection.Input, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_telefono2", n_PRSN_telefono2, ParameterDirection.Input, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_fax", n_PRSN_fax, ParameterDirection.Input, , 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_mail", n_PRSN_mail, ParameterDirection.Input, , 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_homePage", n_PRSN_homePage, ParameterDirection.Input, DbType.String, 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_ricezioneSMS", Me.n_PRSN_RicezioneSMS, , DbType.Byte)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_TPPR_ID", n_PRSN_TPPR.ID, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_LNGU_ID", n_PRSN_LNGU.ID, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_maxAree", Me.n_PRSN_maxAree, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                'oParam = objAccesso.GetParameter("@PRSN_ISTT_ID", n_PRSN_ISTT.Id, , DbType.Int32)
                '.Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_AUTN_ID", Me.n_PRSN_AUTN_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_AUTN_RemoteUniqueID", Me.n_PRSN_AUTN_RemoteUniqueID, ParameterDirection.Input, DbType.String, , 255)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_mostraMail", Me.n_PRSN_mostraMail, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRSN_Ricevimento", Me.n_PRSN_Ricevimento, ParameterDirection.Input, SqlDbType.VarChar, True, 500)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRSN_MailSecondaria", Me.n_PRSN_MailSecondaria, ParameterDirection.Input, SqlDbType.VarChar, True, 255)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_Sector", n_Sector, ParameterDirection.Input, DbType.String, True, 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Job", n_Job, ParameterDirection.Input, DbType.String, True, 255)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBReadExist
            End Try
        End Sub
        'Public Shared Function InviaLoginPwdByEmail(ByVal email As String, ByVal LinguaCode As String, ByVal oLocalizedMail As MailLocalized) As Integer
        '    'riceve login e pwd dalla form dell'utente e gli invia login e pwd con le quali è iscritto
        '    'ritorna codici errore
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim Login, PwdCritto, Pwd, RemoteUniqueID As String
        '    Dim AUTN_Id As Integer

        '    Dim errore As Integer
        '    Dim objAccesso As New COL_DataAccess
        '    With oRequest
        '        .Command = "sp_Persona_LoginPwdByMail"
        '        .CommandType = CommandType.StoredProcedure
        '        oParam = objAccesso.GetParameter("@PRSN_mail", email, ParameterDirection.Input, DbType.String, False, 255)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@PRSN_login", "", ParameterDirection.Output, DbType.String, False, 50)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@PRSN_pwd_Critto", "", ParameterDirection.Output, DbType.String, False, 255)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@PRSN_AUTN_ID", "", ParameterDirection.Output, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@PRSN_AUTN_RemoteUniqueID", "", ParameterDirection.Output, DbType.String, True, 255)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@PRSN_LNGU_ID", "", ParameterDirection.Output, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@PRSN_LNGU_codice", "", ParameterDirection.Output, DbType.String, True, 10)
        '        .Parameters.Add(oParam)

        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try 'lettura login e pwd da db
        '        objAccesso.GetExecuteNotQuery(oRequest)
        '        Login = oRequest.GetValueFromParameter(2)
        '        PwdCritto = oRequest.GetValueFromParameter(3)
        '        AUTN_Id = oRequest.GetValueFromParameter(4)
        '        RemoteUniqueID = oRequest.GetValueFromParameter(5)
        '        Try 'decriptazione della pwd
        '            Dim pwdCryptata As New COL_Encrypter
        '            Pwd = pwdCryptata.Decrypt(PwdCritto)
        '            Try 'iviaggio mail
        '                oLocalizedMail = ManagerConfiguration.GetMailLocalized(ManagerLingua.GetByCodeOrDefault(oRequest.GetValueFromParameter(7)))
        '                '	oLocalizedMail.ReloadByLanguage(oRequest.GetValueFromParameter(7))

        '                Dim oMail As New COL_E_Mail(oLocalizedMail)

        '                oMail.Mittente = oLocalizedMail.SystemSender
        '                oMail.IndirizziTO.Add(New MailAddress(email))
        '                oMail.Oggetto = oLocalizedMail.NotificationMessages(NotificationMessage.NotificationType.Login).Subject
        '                If AUTN_Id = Main.TipoAutenticazione.ComunitaOnLine Then
        '                    oMail.Body = oLocalizedMail.NotificationMessages(NotificationMessage.NotificationType.Login).Message
        '                Else
        '                    oMail.Body = oLocalizedMail.NotificationMessages(NotificationMessage.NotificationType.LoginLDAP).Message
        '                End If
        '                oMail.Body = Replace(oMail.Body, "#login#", Login)
        '                oMail.Body = Replace(oMail.Body, "#pwd#", Pwd)
        '                oMail.Body = oMail.Body & vbCrLf & oLocalizedMail.SystemFirmaNotifica
        '                oMail.Body = Replace(oMail.Body, "<br>", vbCrLf)

        '                oMail.InviaMail()
        '                errore = 0 'nessun errore
        '            Catch ex As Exception
        '                errore = -3 'invio fallito
        '            End Try
        '        Catch ex As Exception
        '            errore = -2 'decriptatura fallita
        '        End Try
        '    Catch ex As Exception
        '        errore = -1 'errore db
        '    End Try
        '    Return errore
        'End Function

        Public Function TipiAutenticazioniAssociati() As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDs As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_TipiAutenticazioniAssociate"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@PRSN_ID", Me.n_PRSN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                oDs = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try
            Return oDs
        End Function
        Public Function ParametriAutenticazioniAssociate() As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDs As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_ParametriAutenticazioniAssociate"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@PRSN_ID", Me.n_PRSN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                oDs = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try
            Return oDs
        End Function
        Public Function Logout() As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_Logout"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@PRSN_id", Me.n_PRSN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Return True
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBReadExist
            End Try
            Return False
        End Function
        Public Shared Function LogoutAll() As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_LogoutAll"
                .CommandType = CommandType.StoredProcedure

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Return True
            Catch ex As Exception

            End Try
            Return False
        End Function


        Public Sub AccessoNonAutenticato(ByVal IP As String, ByVal sessionID As String, ByVal browser As String, ByVal DataBaseID As Integer)
            Dim oTipoPersona As New COL_TipoPersona
            Dim oLingua As New Lingua
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_AccessoNonAutenticato"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@PRSN_id", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_dataAggiornata", DateToString(Now, True), ParameterDirection.Input, DbType.String)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_ultimoCollegamento", "", ParameterDirection.Output, SqlDbType.DateTime)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_login", "", ParameterDirection.Output, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_nome", "", ParameterDirection.Output, DbType.String, False, 40)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_cognome", "", ParameterDirection.Output, DbType.String, False, 40)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPPR_descrizione", "", ParameterDirection.Output, DbType.String, False, 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@TPPR_id", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_intEst", "", ParameterDirection.Output, DbType.Boolean)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@LNGU_id", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Mail", "", ParameterDirection.Output, DbType.String, False, 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_ISTT", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_invisible", "", ParameterDirection.Output, DbType.Boolean)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_AUTN_ID", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_AUTN_RemoteUniqueID", "", ParameterDirection.Output, DbType.String, , 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_mostraMail", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_Ricevimento", "", ParameterDirection.Output, SqlDbType.VarChar, True, 500)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_sesso", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_dataNascita", "", ParameterDirection.Output, SqlDbType.DateTime)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_ORGNDefault_id", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_ORGNDefault_ragioneSociale", "", ParameterDirection.Output, SqlDbType.VarChar, True, 200)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRSN_MailSecondaria", "", ParameterDirection.Output, SqlDbType.VarChar, True, 255)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRSN_RicezioneSMS", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@sessionID", sessionID, ParameterDirection.Input, SqlDbType.VarChar, , 250)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@Ip", IP, ParameterDirection.Input, SqlDbType.VarChar, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@Browser", browser, ParameterDirection.Input, SqlDbType.VarChar, , 300)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@DataBaseId", DataBaseID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                ID = objAccesso.GetExecuteNotQuery(oRequest)
                ' Recupero l'ID univoco della persona.
                Me.n_PRSN_id = oRequest.GetValueFromParameter(1)
                If Me.n_PRSN_id > 0 Then
                    ID = oRequest.GetValueFromParameter(1)
                    ' Recupero l'ultima data d'accesso.

                    If oRequest.GetValueFromParameter(3) = "" Then
                        Me.n_PRSN_ultimoCollegamento = Now
                    Else
                        Me.n_PRSN_ultimoCollegamento = oRequest.GetValueFromParameter(3)
                    End If
                    Me.n_PRSN_login = oRequest.GetValueFromParameter(4)
                    Me.n_PRSN_nome = oRequest.GetValueFromParameter(5)
                    Me.n_PRSN_cognome = oRequest.GetValueFromParameter(6)
                    oTipoPersona.Descrizione = oRequest.GetValueFromParameter(7)
                    oTipoPersona.ID = oRequest.GetValueFromParameter(8)
                    Me.n_PRSN_TPPR = oTipoPersona
                    Me.n_PRSN_IsInterno = oRequest.GetValueFromParameter(9)
                    oLingua.ID = oRequest.GetValueFromParameter(10)
                    Me.n_PRSN_LNGU = oLingua
                    Me.n_PRSN_mail = oRequest.GetValueFromParameter(11)
                    Me.n_PRSN_ISTT.Id = oRequest.GetValueFromParameter(12)
                    Me.n_PRSN_invisibile = oRequest.GetValueFromParameter(13)
                    Me.n_PRSN_AUTN_ID = oRequest.GetValueFromParameter(14)
                    Me.n_PRSN_AUTN_RemoteUniqueID = oRequest.GetValueFromParameter(15)
                    Me.n_PRSN_mostraMail = oRequest.GetValueFromParameter(16)
                    Me.n_PRSN_Ricevimento = oRequest.GetValueFromParameter(17)

                    Me.n_PRSN_sesso = oRequest.GetValueFromParameter(18)
                    Try
                        If IsDate(oRequest.GetValueFromParameter(19)) Then
                            If Not Equals(oRequest.GetValueFromParameter(19), New Date) Then
                                Me.n_PRSN_dataNascita = oRequest.GetValueFromParameter(19)
                            End If
                        End If
                    Catch ex As Exception

                    End Try

                    Me.n_PRSN_ORGNDefault_id = oRequest.GetValueFromParameter(20)
                    Me.n_PRSN_ORGNDefault_ragioneSociale = oRequest.GetValueFromParameter(21)
                    Me.n_PRSN_MailSecondaria = oRequest.GetValueFromParameter(22)
                    Me.n_PRSN_RicezioneSMS = oRequest.GetValueFromParameter(23)
                End If

            Catch ex As Exception
                Me.n_PRSN_id = -1
                Me.n_Errore = Errori_Db.DBReadExist
            End Try
        End Sub





        Public Function AccessoNonAutenticatoComunita(ByVal ComunitaID As Integer, ByVal PercorsoProfilo As String, ByVal CodiceDB As Integer, ByVal oSessione As System.Web.SessionState.HttpSessionState) As String
            Dim oTreeComunita As New COL_TreeComunita
            Dim RuoloID As Integer
            Dim PersonaID As Integer
            Dim iResponse As Boolean = False
            Dim Link As String = ""

            Try
                PersonaID = Me.n_PRSN_id

                oTreeComunita.Directory = PercorsoProfilo
                oTreeComunita.Nome = PersonaID & ".xml"
            Catch ex As Exception

            End Try

            Try
                Dim oRuolo As New COL_RuoloPersonaComunita
                oRuolo.EstraiByLinguaDefault(ComunitaID, PersonaID)
                If oRuolo.Errore = Errori_Db.None Then
                    RuoloID = oRuolo.TipoRuolo.Id
                End If

                Dim oComunita As New COL_Comunita
                oComunita.Id = ComunitaID
                oComunita.Estrai()
                If oComunita.Errore = Errori_Db.None Then
                    Dim ComunitaPath As String

                    ComunitaPath = oComunita.getPercorsoPrincipale
                    oComunita.TipoComunita.Icona = "./../" & oComunita.TipoComunita.Icona
                    If oRuolo.Abilitato And oRuolo.Attivato Then
                        Dim i, j As Integer
                        oSessione("IdRuolo") = RuoloID
                        oSessione("IdComunita") = ComunitaID

                        Dim Elenco_CMNT_ID() As String
                        Elenco_CMNT_ID = ComunitaPath.Split(".")

                        Dim totale As Integer
                        Dim ArrComunita(,) As String
                        ArrComunita = Nothing
                        With oComunita
                            oSessione("ORGN_id") = .Organizzazione.Id
                            Try
                                Dim oServizio As New COL_BusinessLogic_v2.CL_permessi.COL_Servizio
                                Dim oDataSet As New DataSet
                                oDataSet = COL_Servizio.ElencaByTipoRuoloByComunita(RuoloID, ComunitaID)
                                totale = oDataSet.Tables(0).Rows.Count - 1

                                Dim ArrPermessi(totale, 2) As String
                                For i = 0 To totale
                                    Dim oRow As DataRow
                                    oRow = oDataSet.Tables(0).Rows(i)
                                    ArrPermessi(i, 0) = oRow.Item("SRVZ_Codice") 'CODICE servizio
                                    ArrPermessi(i, 1) = oRow.Item("SRVZ_ID") 'id servizio
                                    ArrPermessi(i, 2) = oRow.Item("LKSC_Permessi") 'valore servizio
                                Next
                                oSessione("ArrPermessi") = ArrPermessi
                            Catch ex As Exception

                            End Try

                            oRuolo.UpdateUltimocollegamento()

                            Dim Path As String = ""
                            j = 0
                            For i = 0 To UBound(Elenco_CMNT_ID) - 1
                                If IsNumeric(Elenco_CMNT_ID(i)) Then
                                    If Elenco_CMNT_ID(i) > 0 Then
                                        ReDim Preserve ArrComunita(3, j)
                                        ArrComunita(0, j) = Elenco_CMNT_ID(i)
                                        ArrComunita(1, j) = COL_Comunita.EstraiNomeBylingua(Elenco_CMNT_ID(i), oSessione("LinguaID"))

                                        If Path = "" Then
                                            Path = "." & Elenco_CMNT_ID(i) & "."
                                        Else
                                            Path = Path & Elenco_CMNT_ID(i) & "."
                                        End If
                                        ArrComunita(2, j) = Path
                                        ' Ruolo svolto..........
                                        ArrComunita(3, j) = Me.GetIDRuoloForComunita(Elenco_CMNT_ID(i))
                                        j = j + 1
                                    End If
                                End If
                            Next
                            oSessione("ArrComunita") = ArrComunita
                        End With


                        oSessione("RLPC_ID") = oRuolo.Id

                        oComunita.RegistraAccesso(ComunitaID, PersonaID, CodiceDB)
                        oTreeComunita.Update(oComunita, ComunitaPath, oComunita.GetNomeResponsabile_NomeCreatore, oRuolo)


                        oSessione("AdminForChange") = False
                        oSessione("CMNT_path_forAdmin") = ""
                        oSessione("idComunita_forAdmin") = ""

                        If COL_Comunita.ShowCover(ComunitaID, PersonaID) Then
                            If oRuolo.SaltaCopertina Then
                                Link = RedirectToDefaultPage(ComunitaID)
                            Else
                                Link = "./Generici/Cover.aspx"
                            End If
                        Else
                            Link = RedirectToDefaultPage(ComunitaID)
                        End If
                        iResponse = True
                    End If
                End If
            Catch ex As Exception

            End Try
            Return Link
        End Function

        Private Function RedirectToDefaultPage(ByVal ComunitaID As Integer) As String
            Dim urlDefault As String = ""
            Dim Codice As String = ""
            Dim DefaultPageID As Integer
            Dim hasDefaultPage As Boolean = False
            Dim urlRedirect As String = ""

            hasDefaultPage = Comunita.COL_Comunita.GetDefaultPage(ComunitaID, urlDefault, Codice, DefaultPageID)
            If Not hasDefaultPage Or urlDefault = "" Then
                urlRedirect = "./Comunita/comunita.aspx"
            Else
                Dim Redirigi As Boolean = False
                Redirigi = CanRedirectToDefaultPage(Codice, ComunitaID)
                If Redirigi Then
                    urlRedirect = urlDefault
                Else
                    urlRedirect = "./Comunita/comunita.aspx"
                End If
            End If
            Return urlRedirect
        End Function
        Private Function CanRedirectToDefaultPage(ByVal Codice As String, ByVal ComunitaID As Integer) As Boolean
            Dim Redirigi As Boolean = False

            If Codice = Services_Bacheca.Codex Then

                Try
                    Dim oServizio As New Services_Bacheca(Comunita.COL_Comunita.GetPermessiForServizioByPersona(Me.n_PRSN_id, ComunitaID, Codice))
                    If oServizio.Admin Or oServizio.Read Or oServizio.Write Then
                        Redirigi = True
                    End If
                Catch ex As Exception

                End Try
            ElseIf Codice = Services_CHAT.Codex Then
                Dim oServizio As New Services_CHAT
                Try
                    oServizio.PermessiAssociati = Comunita.COL_Comunita.GetPermessiForServizioByPersona(Me.n_PRSN_id, ComunitaID, Codice)
                    If oServizio.Admin Or oServizio.Read Or oServizio.Write Then
                        Redirigi = True
                    End If
                Catch ex As Exception

                End Try
            ElseIf Codice = Services_DiarioLezioni.Codex Then
                Dim oServizio As New Services_DiarioLezioni
                Try
                    oServizio.PermessiAssociati = Comunita.COL_Comunita.GetPermessiForServizioByPersona(Me.n_PRSN_id, ComunitaID, Codice)
                    If oServizio.Admin Or oServizio.Read Or oServizio.Change Or oServizio.Upload Then
                        Redirigi = True
                    End If
                Catch ex As Exception

                End Try
            ElseIf Codice = Services_Eventi.Codex Then
                Dim oServizio As New Services_Eventi
                Try
                    oServizio.PermessiAssociati = Comunita.COL_Comunita.GetPermessiForServizioByPersona(Me.n_PRSN_id, ComunitaID, Codice)
                    If oServizio.AdminService Or oServizio.AddEvents Or oServizio.ChangeEvents Or oServizio.ReadEvents Then
                        Redirigi = True
                    End If
                Catch ex As Exception

                End Try
            ElseIf Codice = Services_File.Codex Then
                Dim oServizio As New Services_File
                Try
                    oServizio.PermessiAssociati = Comunita.COL_Comunita.GetPermessiForServizioByPersona(Me.n_PRSN_id, ComunitaID, Codice)
                    If oServizio.Admin Or oServizio.Moderate Or oServizio.Read Or oServizio.Upload Or oServizio.Change Then
                        Redirigi = True
                    End If
                Catch ex As Exception

                End Try
            ElseIf Codice = Services_Forum.Codex Then
                Dim oServizio As New Services_Forum
                Try
                    oServizio.PermessiAssociati = Comunita.COL_Comunita.GetPermessiForServizioByPersona(Me.n_PRSN_id, ComunitaID, Codice)
                    If oServizio.AccessoForum Or oServizio.GestioneForum Then
                        Redirigi = True
                    End If
                Catch ex As Exception

                End Try
            ElseIf Codice = Services_Gallery.Codex Then
                Dim oServizio As New Services_Gallery
                Try
                    oServizio.PermessiAssociati = Comunita.COL_Comunita.GetPermessiForServizioByPersona(Me.n_PRSN_id, ComunitaID, Codice)
                    If oServizio.Admin Or oServizio.Management Or oServizio.List Then
                        Redirigi = True
                    End If
                Catch ex As Exception

                End Try
            ElseIf Codice = Services_RaccoltaLink.Codex Then
                Dim oServizio As New Services_RaccoltaLink
                Try
                    oServizio.PermessiAssociati = Comunita.COL_Comunita.GetPermessiForServizioByPersona(Me.n_PRSN_id, ComunitaID, Codice)
                    If oServizio.Admin Or oServizio.Moderate Or oServizio.List Or oServizio.AddLink Then
                        Redirigi = True
                    End If
                Catch ex As Exception

                End Try
            ElseIf Codice = Services_Statistiche.Codex Then
                Dim oServizio As New Services_Statistiche
                Try
                    oServizio.PermessiAssociati = Comunita.COL_Comunita.GetPermessiForServizioByPersona(Me.n_PRSN_id, ComunitaID, Codice)
                    If oServizio.Admin Or oServizio.List Or oServizio.Management Then
                        Redirigi = True
                    End If
                Catch ex As Exception

                End Try
            ElseIf Codice = Services_Listaiscritti.Codex Then
                Dim oServizio As New Services_Listaiscritti
                Try
                    oServizio.PermessiAssociati = Comunita.COL_Comunita.GetPermessiForServizioByPersona(Me.n_PRSN_id, ComunitaID, Codice)
                    If oServizio.Management Or oServizio.List Or oServizio.Admin Then
                        Redirigi = True
                    End If
                Catch ex As Exception

                End Try
            ElseIf Codice = Services_Cover.Codex Then
                Dim oServizio As New Services_Cover
                Try
                    If Not COL_BusinessLogic_v2.Comunita.COL_RuoloPersonaComunita.isSkipCover(ComunitaID, Me.n_PRSN_id) Then
                        oServizio.PermessiAssociati = Comunita.COL_Comunita.GetPermessiForServizioByPersona(Me.n_PRSN_id, ComunitaID, Codice)
                        If oServizio.Management Or oServizio.Read Or oServizio.Admin Then
                            Redirigi = True
                        End If
                    End If
                Catch ex As Exception

                End Try
            End If
            Return Redirigi
        End Function


        Public Sub Logon(ByVal IP As String, ByVal sessionID As String, ByVal browser As String, ByVal DataBaseID As Integer)
            Dim oTipoPersona As New COL_TipoPersona
            Dim oLingua As New Lingua
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim pwdCryptata As New COL_Encrypter
            n_PRSN_pwd = pwdCryptata.Encrypt(n_PRSN_pwd)

            With oRequest
                .Command = "sp_Persona_Login"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@PRSN_id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_dataAggiornata", DateToString(Now, True), ParameterDirection.Input, DbType.String)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_ultimoCollegamento", "", ParameterDirection.Output, DbType.DateTime)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_login", n_PRSN_login.Trim, ParameterDirection.Input, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_PWD", n_PRSN_pwd, ParameterDirection.Input, DbType.String, , 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_nome", "", ParameterDirection.Output, DbType.String, False, 40)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_cognome", "", ParameterDirection.Output, DbType.String, False, 40)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPPR_descrizione", "", ParameterDirection.Output, DbType.String, False, 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPPR_id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_intEst", "", ParameterDirection.Output, DbType.Boolean)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LNGU_id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Mail", "", ParameterDirection.Output, DbType.String, False, 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_ISTT", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_invisible", "", ParameterDirection.Output, DbType.Boolean)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_AUTN_ID", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_AUTN_RemoteUniqueID", "", ParameterDirection.Output, DbType.String, , 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_mostraMail", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_Ricevimento", "", ParameterDirection.Output, SqlDbType.VarChar, True, 500)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_sesso", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_dataNascita", "", ParameterDirection.Output, SqlDbType.DateTime)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_ORGNDefault_id", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_ORGNDefault_ragioneSociale", "", ParameterDirection.Output, SqlDbType.VarChar, True, 200)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRSN_MailSecondaria", "", ParameterDirection.Output, SqlDbType.VarChar, True, 255)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRSN_RicezioneSMS", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@sessionID", sessionID, ParameterDirection.Input, SqlDbType.VarChar, , 250)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@Ip", IP, ParameterDirection.Input, SqlDbType.VarChar, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@Browser", browser, ParameterDirection.Input, SqlDbType.VarChar, , 300)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@DataBaseId", DataBaseID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                ID = objAccesso.GetExecuteNotQuery(oRequest)
                ' Recupero l'ID univoco della persona.
                Me.n_PRSN_id = oRequest.GetValueFromParameter(1)
                If Me.n_PRSN_id > 0 Then
                    ID = oRequest.GetValueFromParameter(1)
                    ' Recupero l'ultima data d'accesso.
                    If oRequest.GetValueFromParameter(3) = "" Then
                        Me.n_PRSN_ultimoCollegamento = Now
                    Else
                        Me.n_PRSN_ultimoCollegamento = oRequest.GetValueFromParameter(3)
                    End If
                    Me.n_PRSN_nome = oRequest.GetValueFromParameter(6)
                    Me.n_PRSN_cognome = oRequest.GetValueFromParameter(7)
                    oTipoPersona.Descrizione = oRequest.GetValueFromParameter(8)
                    oTipoPersona.ID = oRequest.GetValueFromParameter(9)
                    Me.n_PRSN_TPPR = oTipoPersona
                    Me.n_PRSN_IsInterno = oRequest.GetValueFromParameter(10)
                    oLingua.ID = oRequest.GetValueFromParameter(11)
                    Me.n_PRSN_LNGU = oLingua
                    Me.n_PRSN_mail = oRequest.GetValueFromParameter(12)
                    Me.n_PRSN_ISTT.Id = oRequest.GetValueFromParameter(13)
                    Me.n_PRSN_invisibile = oRequest.GetValueFromParameter(14)
                    Me.n_PRSN_AUTN_ID = oRequest.GetValueFromParameter(15)
                    Me.n_PRSN_AUTN_RemoteUniqueID = oRequest.GetValueFromParameter(16)
                    Me.n_PRSN_mostraMail = oRequest.GetValueFromParameter(17)
                    Me.n_PRSN_Ricevimento = oRequest.GetValueFromParameter(18)

                    Me.n_PRSN_sesso = oRequest.GetValueFromParameter(19)
                    Try
                        If IsDate(oRequest.GetValueFromParameter(20)) Then
                            If Not Equals(oRequest.GetValueFromParameter(20), New Date) Then
                                Me.n_PRSN_dataNascita = oRequest.GetValueFromParameter(20)
                            End If
                        End If
                    Catch ex As Exception

                    End Try

                    Me.n_PRSN_ORGNDefault_id = oRequest.GetValueFromParameter(21)
                    Me.n_PRSN_ORGNDefault_ragioneSociale = oRequest.GetValueFromParameter(22)
                    Me.n_PRSN_MailSecondaria = oRequest.GetValueFromParameter(23)
                    Me.n_PRSN_RicezioneSMS = oRequest.GetValueFromParameter(24)
                End If

            Catch ex As Exception
                Me.n_PRSN_id = -1
                Me.n_Errore = Errori_Db.DBReadExist
            End Try
        End Sub


        Public Sub LogonAsUser(ByVal PRSN_Id As Integer, ByVal IP As String, ByVal sessionID As String, ByVal browser As String, ByVal DataBaseId As Integer)
            Dim oTipoPersona As New COL_TipoPersona
            Dim oLingua As New Lingua
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim pwdCryptata As New COL_Encrypter

            With oRequest
                .Command = "sp_Persona_LogonAsUser"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@PRSN_id", PRSN_Id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_dataAggiornata", DateToString(Now, True), ParameterDirection.Input, DbType.String)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_ultimoCollegamento", "", ParameterDirection.Output, DbType.DateTime)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_nome", "", ParameterDirection.Output, DbType.String, False, 40)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_cognome", "", ParameterDirection.Output, DbType.String, False, 40)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPPR_descrizione", "", ParameterDirection.Output, DbType.String, False, 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPPR_id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_intEst", "", ParameterDirection.Output, DbType.Boolean)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LNGU_id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Mail", "", ParameterDirection.Output, DbType.String, False, 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_ISTT", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_invisible", "", ParameterDirection.Output, DbType.Boolean)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_AUTN_ID", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_AUTN_RemoteUniqueID", "", ParameterDirection.Output, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_login", "", ParameterDirection.Output, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_mostraMail", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_Ricevimento", "", ParameterDirection.Output, SqlDbType.VarChar, True, 500)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRSN_sesso", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_dataNascita", "", ParameterDirection.Output, SqlDbType.DateTime)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_ORGNDefault_id", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_ORGNDefault_ragioneSociale", "", ParameterDirection.Output, SqlDbType.VarChar, True, 200)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_MailSecondaria", "", ParameterDirection.Output, SqlDbType.VarChar, True, 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_RicezioneSMS", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@sessionID", sessionID, ParameterDirection.Input, DbType.String, , 250)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Ip", IP, ParameterDirection.Input, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Browser", browser, ParameterDirection.Input, DbType.String, , 300)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@DataBaseId", DataBaseId, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                ID = objAccesso.GetExecuteNotQuery(oRequest)
                ' Recupero l'ID univoco della persona.
                Me.n_PRSN_id = PRSN_Id
                ID = PRSN_Id
                ' Recupero l'ultima data d'accesso.
                If oRequest.GetValueFromParameter(3) = "" Then
                    Me.n_PRSN_ultimoCollegamento = Now
                Else
                    Me.n_PRSN_ultimoCollegamento = oRequest.GetValueFromParameter(3)
                End If
                ' Recupero nome della persona.
                Me.n_PRSN_nome = oRequest.GetValueFromParameter(4)
                ' Recupero cognome della persona.
                Me.n_PRSN_cognome = oRequest.GetValueFromParameter(5)
                ' Recupero il tipo_persona
                oTipoPersona.Descrizione = oRequest.GetValueFromParameter(6)
                ' id del tipo persona
                oTipoPersona.ID = oRequest.GetValueFromParameter(7)
                Me.n_PRSN_TPPR = oTipoPersona
                '
                Me.n_PRSN_IsInterno = oRequest.GetValueFromParameter(8)
                '
                oLingua.ID = oRequest.GetValueFromParameter(9)
                Me.n_PRSN_LNGU = oLingua
                Me.n_PRSN_mail = oRequest.GetValueFromParameter(10)
                Me.n_PRSN_ISTT.Id = oRequest.GetValueFromParameter(11)
                Me.n_PRSN_invisibile = oRequest.GetValueFromParameter(12)
                Me.n_PRSN_AUTN_ID = oRequest.GetValueFromParameter(13)
                Me.n_PRSN_AUTN_RemoteUniqueID = oRequest.GetValueFromParameter(14)
                Me.n_PRSN_login = oRequest.GetValueFromParameter(15)
                Me.n_PRSN_mostraMail = oRequest.GetValueFromParameter(16)
                Me.n_PRSN_Ricevimento = oRequest.GetValueFromParameter(17)

                Me.n_PRSN_sesso = oRequest.GetValueFromParameter(18)
                Try
                    If IsDate(oRequest.GetValueFromParameter(19)) Then
                        If Not Equals(oRequest.GetValueFromParameter(19), New Date) Then
                            Me.n_PRSN_dataNascita = oRequest.GetValueFromParameter(19)
                        End If
                    End If
                Catch ex As Exception

                End Try

                Me.n_PRSN_ORGNDefault_id = oRequest.GetValueFromParameter(20)
                Me.n_PRSN_ORGNDefault_ragioneSociale = oRequest.GetValueFromParameter(21)
                Me.n_PRSN_MailSecondaria = oRequest.GetValueFromParameter(22)
                Me.n_PRSN_RicezioneSMS = oRequest.GetValueFromParameter(23)
            Catch ex As Exception
                Me.n_PRSN_id = -1
                Me.n_Errore = Errori_Db.DBReadExist
            End Try
        End Sub
        Public Sub RegistraAccesso(ByVal DataBaseId As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_RegistraAccesso"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@PersonaID", Me.n_PRSN_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@DataBaseId", DataBaseId, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                iResponse = objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                iResponse = 0
                Me.n_Errore = Errori_Db.DBError
            End Try
            'Return iResponse
        End Sub
        Public Function CancellaConnessione(ByVal PRSN_ID As Integer, ByVal sessionID As String) As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_CancellaConnessione"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@PRSN_ID", PRSN_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@sessionID", sessionID, ParameterDirection.Input, DbType.String, , 250)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@totaleConnessioni", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                iResponse = objAccesso.GetExecuteNotQuery(oRequest)
                iResponse = oRequest.GetValueFromParameter(3)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                iResponse = 0
                Me.n_Errore = Errori_Db.DBError
            End Try
            Return iResponse
        End Function
        Public Shared Function CancellaConnessioneById(ByVal PRSN_ID As Integer, ByVal ConnessioneID As Integer) As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_CancellaConnessioneByID"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@ConnessioneID", ConnessioneID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_ID", PRSN_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@totaleConnessioni", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                iResponse = objAccesso.GetExecuteNotQuery(oRequest)
                iResponse = oRequest.GetValueFromParameter(3)
            Catch ex As Exception
                iResponse = 0
            End Try
            Return iResponse
        End Function
        'Public Function CambiaConnessione_(ByVal PRSN_ID As Integer, ByVal sessionID As String, ByVal NewPRSN_ID As Integer)
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim iResponse As Integer
        '    Dim objAccesso As New COL_DataAccess

        '    With oRequest
        '        .Command = "sp_Persona_CambiaConnessione"
        '        .CommandType = CommandType.StoredProcedure

        '        oParam = objAccesso.GetParameter("@PRSN_ID", PRSN_ID, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@sessionID", sessionID, ParameterDirection.Input, DbType.String, , 250)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@NewPRSN_ID", NewPRSN_ID, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With

        '    Try
        '        iResponse = objAccesso.GetExecuteNotQuery(oRequest)
        '        Me.n_Errore = Errori_Db.None
        '    Catch ex As Exception
        '        iResponse = -1
        '        Me.n_Errore = Errori_Db.DBError
        '    End Try
        '    Return iResponse
        'End Function
        Public Function GetNumeroConnessioni() As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_GetNumeroConnessioni"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@totaleConnessioni", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@DataConnessione", DateToString(Now, True), ParameterDirection.Input, DbType.String, , 30)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                iResponse = objAccesso.GetExecuteNotQuery(oRequest)
                iResponse = oRequest.GetValueFromParameter(1)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                iResponse = 0
                Me.n_Errore = Errori_Db.DBError
            End Try
            Return iResponse
        End Function
#End Region

#Region "Metodi SMS"
        Public Function GetCellulare() As Integer
            'tira su il numero di cellulare delle persone, la parte commentata serve
            'per la flag che verrà messa per l'assenso alla ricezione SMS
            'anche la SP è predisposta
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim responso As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_EstraiCellulare"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@PRSN_ID", Me.ID, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Cellulare", "", ParameterDirection.Output, , True, 50)
                .Parameters.Add(oParam)
                'oParam = objAccesso.GetParameter("@PRSN_RicezioneSMS", "", ParameterDirection.Output, DbType.Byte)
                '.Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                responso = objAccesso.GetExecuteNotQuery(oRequest)
                '  Me.RicezioneSMS = oRequest.GetValueFromParameter(3)
                Me.Cellulare = oRequest.GetValueFromParameter(2)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try
            Return responso
        End Function
        Public Function GetMail() As Integer
            'tira su la mail dell'utente
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim responso As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_EstraiMail"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@PRSN_ID", Me.ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Mail", "", ParameterDirection.Output, , True, 255)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                responso = objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_PRSN_mail = oRequest.GetValueFromParameter(2)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try
            Return responso
        End Function

        Public Function ChangeCellulareRicezione() As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim responso As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_ChangeCellulareRicezione"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@PRSN_ID", Me.n_PRSN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_Cellulare", Me.n_PRSN_cellulare, ParameterDirection.Input, DbType.String, True, 25)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_RicezioneSMS", Me.n_PRSN_RicezioneSMS, ParameterDirection.Input, DbType.Byte)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                responso = objAccesso.GetExecuteNotQuery(oRequest)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBChange
            End Try
            Return responso
        End Function
        Public Function ChangeCellulareRicezione(ByVal pCellulare As String, ByVal ricezione As Boolean) As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim responso As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_ChangeCellulareRicezione"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@PRSN_ID", Me.n_PRSN_id, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_Cellulare", pCellulare, , , True, 25)
                .Parameters.Add(oParam)

                If ricezione Then
                    oParam = objAccesso.GetParameter("@PRSN_RicezioneSMS", 1, , DbType.Byte)
                Else
                    oParam = objAccesso.GetParameter("@PRSN_RicezioneSMS", 0, , DbType.Byte)
                End If
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                responso = objAccesso.GetExecuteNotQuery(oRequest)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBChange
            End Try
            Return responso
        End Function
        Public Sub ChangeRicezioneForComunity(ByVal CMNT_ID As Integer, ByVal pRicezioneSMS As Integer)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dstable As Integer
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Persona_ChangeRicezioneForComunity"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@CMNT_ELEF_Id", CMNT_ID, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_ELEF_ID", Me.n_PRSN_id, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@RicezioneSMS", pRicezioneSMS, , DbType.Int32)
                .Parameters.Add(oParam)


                .Role = COL_Request.UserRole.Admin
                .transactional = False

            End With
            Try
                dstable = objAccesso.GetExecuteNotQuery(oRequest)

                Me.n_Errore = Errori_Db.None
            Catch
                Me.ID = -1
                Me.n_Errore = Errori_Db.DBError
                dstable = -1
            End Try

            '  Return dstable
        End Sub

        Public Function ChangeCellulare() As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim responso As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_ChangeCellulare"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@PRSN_ID", Me.n_PRSN_id, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_Cellulare", Me.n_PRSN_cellulare, , , True, 25)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                responso = objAccesso.GetExecuteNotQuery(oRequest)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBChange
            End Try
            Return responso
        End Function
        Public Function ChangeCellulare(ByVal pCellulare As String) As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim responso As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_ChangeCellulare"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@PRSN_ID", Me.n_PRSN_id, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_Cellulare", pCellulare, , , True, 25)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                responso = objAccesso.GetExecuteNotQuery(oRequest)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBChange
            End Try
            Return responso
        End Function

#End Region

        'MODIFICATE A CASA MIA
        Public Function OrganizzazioniNonAssociateElenca_(Optional ByVal oFiltro As Main.FiltroUtenti = Main.FiltroUtenti.PassantiCreatori, Optional ByVal oFiltroOrdinamento As FiltroOrdinamento = FiltroOrdinamento.Crescente, Optional ByVal oFiltroCampoOrdine As FiltroCampoOrdineComunita = Main.FiltroCampoOrdineComunita.Livello, Optional ByVal oFiltroComunita As Main.FiltroComunita = Main.FiltroComunita.tutti, Optional ByVal valore As String = "", Optional ByVal FiltroStato As Main.FiltroStatoComunita = Main.FiltroStatoComunita.Tutte) As DataSet
            '- restituisce un dataset contenente tutte le organizzazioni che non sono assegnate alla persona
            '- usato dal LIMBO
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDs As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_ElencaOrganizzazioniNonAssociate"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@PRSN_id", Me.n_PRSN_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroUtente", CType(oFiltro, Main.FiltroUtenti), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroOrdinamento", CType(oFiltroOrdinamento, Main.FiltroOrdinamento), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroCampoOrdine", CType(oFiltroCampoOrdine, Main.FiltroCampoOrdineComunita), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroComunita", CType(oFiltroComunita, Main.FiltroComunita), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@valore", valore, ParameterDirection.Input, SqlDbType.VarChar, True, 200)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@FiltroStato", CType(FiltroStato, Main.FiltroStatoComunita), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                oDs = objAccesso.GetdataSet(oRequest)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try
            Return oDs
        End Function
        '#####################

        Public Function GetComunitaForRicezione(ByVal ORGN_ID As Integer, ByVal TPCM_ID As Integer, Optional ByVal CMNT_AnnoAccademico As Integer = 0) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_GetComunitaForRicezione"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@ORGN_ID", ORGN_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@CMNT_AnnoAccademico", CMNT_AnnoAccademico, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@TPCM_ID", TPCM_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_ID", Me.n_PRSN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False

            End With
            Try
                dsTable = objAccesso.GetdataSet(oRequest)

                ' Recupero l'ID univoco della persona.
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try
            Return dsTable
        End Function

#Region "Metodi Associazione credito"

        'Public Function GetCreditoDisponibile() As Integer
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim iDisponibile As Integer
        '    Dim objAccesso As New COL_DataAccess
        '    With oRequest
        '        .Command = "sp_Persona_GetCredito"
        '        .CommandType = CommandType.StoredProcedure

        '        oParam = objAccesso.GetParameter("@CRSM_PRSN_ID", Me.n_PRSN_id, , DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@CRSM_Disponibili", 0, ParameterDirection.Output, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False

        '    End With
        '    iDisponibile = 0
        '    Try
        '        iDisponibile = objAccesso.GetExecuteNotQuery(oRequest)

        '        iDisponibile = oRequest.GetValueFromParameter(2)
        '        Me.n_Errore = Errori_Db.None

        '    Catch ex As Exception
        '        Me.n_Errore = Errori_Db.DBReadExist
        '    End Try

        '    Return iDisponibile
        'End Function
        'Public Function SetCreditoDisponibile(ByVal pDisponibile As Integer)
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim dstable As Integer
        '    Dim objAccesso As New COL_DataAccess
        '    With oRequest
        '        .Command = "sp_Persona_SetCredito"
        '        .CommandType = CommandType.StoredProcedure

        '        oParam = objAccesso.GetParameter("@CRSM_PRSN_ID", Me.n_PRSN_id, , DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@CRSM_Disponibili", pDisponibile, , DbType.Int32)
        '        .Parameters.Add(oParam)

        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False

        '    End With
        '    Try
        '        dstable = objAccesso.GetExecuteNotQuery(oRequest)
        '        If dstable > 0 Then
        '            Me.n_Errore = Errori_Db.None
        '        Else
        '            Me.n_Errore = Errori_Db.DBChange
        '        End If
        '    Catch
        '        Me.n_Errore = Errori_Db.DBError
        '        dstable = -1
        '    End Try

        '    Return dstable
        'End Function

        'Public Function EliminaCredito()
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim dstable As Integer
        '    Dim objAccesso As New COL_DataAccess
        '    With oRequest
        '        .Command = "sp_Persona_EliminaCredito"
        '        .CommandType = CommandType.StoredProcedure

        '        oParam = objAccesso.GetParameter("@CRSM_PRSN_ID", Me.n_PRSN_id, , DbType.Int32)
        '        .Parameters.Add(oParam)

        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False

        '    End With
        '    Try
        '        dstable = objAccesso.GetExecuteNotQuery(oRequest)
        '        If dstable > 0 Then
        '            Me.n_Errore = Errori_Db.None
        '        Else
        '            Me.n_Errore = Errori_Db.DBDelete
        '        End If
        '    Catch
        '        dstable = -1
        '    End Try

        '    Return dstable
        'End Function
        'Public Shared Function EliminaCredito(ByVal pPRSN_ID As Integer)
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim dstable As Integer
        '    Dim objAccesso As New COL_DataAccess
        '    With oRequest
        '        .Command = "sp_Persona_EliminaCredito"
        '        .CommandType = CommandType.StoredProcedure

        '        oParam = objAccesso.GetParameter("@CRSM_PRSN_ID", pPRSN_ID, , DbType.Int32)
        '        .Parameters.Add(oParam)

        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False

        '    End With
        '    Try
        '        dstable = objAccesso.GetExecuteNotQuery(oRequest)
        '    Catch
        '        dstable = -1
        '    End Try

        '    Return dstable
        'End Function

        ''Public Function AggiornaRicezione_Comunity(ByVal pComunita As SMSC_Comunita, ByVal pRicezioneSMS As Integer)
        ''    Dim oRequest As New COL_Request
        ''    Dim oParam As New COL_Request.Parameter
        ''    Dim dstable As Integer
        ''    Dim objAccesso As New COL_DataAccess
        ''    With oRequest
        ''        .Command = "sp_Persona_AggiornaRicezione_Comunity"
        ''        .CommandType = CommandType.StoredProcedure

        ''        oParam = objAccesso.GetParameter("@CMNT_ELEF_Id", pComunita.ELEF_ID, , DbType.Int32)
        ''        .Parameters.Add(oParam)

        ''        oParam = objAccesso.GetParameter("@CMNT_Tipo", CType(pComunita.Tipo, Tipo_Comunita), , DbType.Int32)
        ''        .Parameters.Add(oParam)

        ''        oParam = objAccesso.GetParameter("@PRSN_ELEF_ID", Me.n_PRSN_ELEF_ID, , DbType.Int32)
        ''        .Parameters.Add(oParam)

        ''        oParam = objAccesso.GetParameter("@RicezioneSMS", pRicezioneSMS, , DbType.Int32)
        ''        .Parameters.Add(oParam)

        ''        .Role = COL_Request.UserRole.Admin
        ''        .transactional = False

        ''    End With
        ''    Try
        ''        dstable = objAccesso.GetExecuteNotQuery(oRequest)

        ''        Me.n_Errore = Errori_SMS.None
        ''    Catch
        ''        Me.ID = -1
        ''        Me.n_Errore = Errori_SMS.DBError
        ''        dstable = -1
        ''    End Try

        ''    Return dstable
        ''End Function
        'Public Function AggiornaRicezione_Comunities(ByVal pRicezioneSMS As Integer)
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim dstable As Integer
        '    Dim objAccesso As New COL_DataAccess
        '    With oRequest
        '        .Command = "sp_Persona_AggiornaRicezioneForComunities"
        '        .CommandType = CommandType.StoredProcedure

        '        oParam = objAccesso.GetParameter("@PRSN_ID", Me.n_PRSN_id, , DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@RicezioneSMS", pRicezioneSMS, , DbType.Int32)
        '        .Parameters.Add(oParam)

        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False

        '    End With
        '    Try
        '        dstable = objAccesso.GetExecuteNotQuery(oRequest)

        '        Me.n_Errore = Errori_Db.None
        '    Catch
        '        Me.Id = -1
        '        Me.n_Errore = Errori_Db.DBError
        '        dstable = -1
        '    End Try

        '    Return dstable
        'End Function


#End Region

#Region "Metodi"

        Public Function VediOrganizzazioni() As DataSet
            'elenca gli id delle organizzazioni a cui una persona è associata
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                oParam = objAccesso.GetParameter("@PRSN_id", Me.n_PRSN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Command = "sp_Persona_Organizzazione"
                .CommandType = CommandType.StoredProcedure
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

        Public Sub AssociaOrganizzazione(ByVal IdOrganizzazione As Integer, ByVal Principale As Integer)
            ' associa la persona ad una organizzazione
            ' bisognerebbe poter assegnare + di un'organizzazione, nella sp c'è il controllo che lo impedisce
            ' 
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_AssociaOrganizzazione"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@LKPO_PRSN_ID", Me.n_PRSN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LKPO_ORGN_ID", IdOrganizzazione, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LKPO_default", Principale, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch
                Me.n_Errore = Errori_Db.DBInsert
            End Try
        End Sub

        ' PER PALMARE
        Function ElencaComunitaAppartenenza(ByVal TipoComunita As Integer) As DataSet
            'elenca le comunità, se tipocomunità = 0 tira su tutto, altimenti solo quelle per tipocomunita

            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Comunita_AppartenenzaElenca"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@RLPC_PRSN_id", Me.n_PRSN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@TPCM_id", TipoComunita, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                dsTable = objAccesso.GetdataSet(oRequest)
            Catch
                Me.n_Errore = Errori_Db.DBError
            End Try
            Return dsTable
        End Function

        Public Shared Function TipiComunitaAssociati(ByVal IdPersona As Integer) As DataSet
            'sono i tipi di comunità associati alla persona
            'vedere la gestione errori
            ' VECCHIO!!!! era usato nella vecchia selezionacomunita.aspx
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                oParam = objAccesso.GetParameter("@PRSN_id", IdPersona, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                .Command = "sp_Persona_TipiComunitaAssociati"
                .CommandType = CommandType.StoredProcedure
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                dsTable = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception
                'gestione errore delle shared!!!
            End Try
            Return dsTable
        End Function
        Public Function ElencaNoIscrittiCmnt(ByVal idORGN As Integer, ByVal TipoPersona As Integer, ByVal idCMNT As Integer, Optional ByVal termine As String = "") As DataSet
            ' elenca tutte le persone di una certa organizzazione filtrati per tipo persona o per termine escludendo coloro che nn fanno parte ad una data comunità
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_ElencaNoIscrittiCmnt"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@ORGN_id", idORGN, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPPR_id", TipoPersona, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@termine", termine, ParameterDirection.Input, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_id", idCMNT, ParameterDirection.Input, DbType.Int32)
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
        Public Function GetIstituzione() As String
            ' ritorna come stringa la ragione sociale dell'istituzione di appartenenza (il nome in pratica)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim Nome As String = ""
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_getIstituzione"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@PRSN_id", Me.ID, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Istituzione", "", ParameterDirection.Output, DbType.String, False, 50)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Nome = oRequest.GetValueFromParameter(2)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try
            Return Nome
        End Function

        Public Sub getTipoFromRLPC(ByVal idRLPC As Integer)
            'tira su l'id persona e l'id del tipo persona tramite l'RLPC
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_getTipoFromRLPC"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@RLPC_ID", idRLPC, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_ID", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_TPPR_id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_PRSN_id = oRequest.GetValueFromParameter(2)
                Me.n_PRSN_TPPR.ID = oRequest.GetValueFromParameter(3)
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try

        End Sub
#End Region

#Region "Gestione SMS"
        'Public Function LeggiSMSInviati(ByVal pCMNT_ID As Integer, Optional ByVal Archiviato As Integer = -1) As DataSet
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim oDs As DataSet
        '    Dim objAccesso As New COL_DataAccess

        '    With oRequest
        '        .Command = "sp_Persona_SMSInviati"
        '        .CommandType = CommandType.StoredProcedure
        '        oParam = objAccesso.GetParameter("@PRSN_ID", Me.Id, , DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@CMNT_ID", pCMNT_ID, , DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@STSM_Archiviato", Archiviato, , DbType.Int32)
        '        .Parameters.Add(oParam)
        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With

        '    Try
        '        oDs = objAccesso.GetdataSet(oRequest)
        '    Catch ex As Exception
        '        Me.n_Errore = Errori_Db.DBError
        '    End Try
        '    Return oDs
        'End Function
        'Public Function LeggiSMSRicevuti(ByVal pCMNT_ID As Integer, Optional ByVal archiviato As Integer = -1) As DataSet
        '    ' si capisce cosa fa
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim oDs As DataSet
        '    Dim objAccesso As New COL_DataAccess

        '    With oRequest
        '        .Command = "sp_Persona_SMSRicevuti"
        '        .CommandType = CommandType.StoredProcedure
        '        oParam = objAccesso.GetParameter("@PRSN_ID", Me.Id, , DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@CMNT_ID", pCMNT_ID, , DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@Archiviato", archiviato, , DbType.Int32)
        '        .Parameters.Add(oParam)
        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With

        '    Try
        '        oDs = objAccesso.GetdataSet(oRequest)
        '    Catch ex As Exception
        '        Me.n_Errore = Errori_Db.DBError
        '    End Try
        '    Return oDs
        'End Function



        'Public Function GetCreditoAssociato() As COL_Credito
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim iDisponibile As Integer
        '    Dim oCredito As New COL_Credito
        '    Dim objAccesso As New COL_DataAccess

        '    With oRequest
        '        .Command = "sp_Persona_GetCreditoAssociato"
        '        .CommandType = CommandType.StoredProcedure

        '        oParam = objAccesso.GetParameter("@CRSM_PRSN_ID", Me.n_PRSN_id, , DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@CRSM_ID", 0, ParameterDirection.Output, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@CRSM_Disponibili", 0, ParameterDirection.Output, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@CRSM_Illimitato", "", ParameterDirection.Output, DbType.Byte)
        '        .Parameters.Add(oParam)

        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False

        '    End With
        '    iDisponibile = 0
        '    Try
        '        iDisponibile = objAccesso.GetExecuteNotQuery(oRequest)

        '        oCredito.ID = oRequest.GetValueFromParameter(2)
        '        oCredito.Disponibili = oRequest.GetValueFromParameter(3)
        '        If oRequest.GetValueFromParameter(4) = 1 Then
        '            oCredito.Illimitato = True
        '        Else
        '            oCredito.Illimitato = False
        '        End If
        '        Me.n_Errore = Errori_Db.None

        '    Catch ex As Exception
        '        oCredito.ID = -1
        '        Me.n_Errore = Errori_Db.None
        '    End Try

        '    Return oCredito
        'End Function

        'Public Function AssociaCredito(ByVal pDisponibile As Integer, ByVal pIllimitato As Boolean) As Boolean
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim dstable As Integer
        '    Dim objAccesso As New COL_DataAccess
        '    With oRequest
        '        .Command = "sp_Persona_AssociaCredito"
        '        .CommandType = CommandType.StoredProcedure

        '        oParam = objAccesso.GetParameter("@CRSM_PRSN_ID", Me.n_PRSN_id, , DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@CRSM_Disponibili", pDisponibile, , DbType.Int32)
        '        .Parameters.Add(oParam)

        '        If pIllimitato Then
        '            oParam = objAccesso.GetParameter("@CRSM_Illimitato", 1, , DbType.Byte)
        '        Else
        '            oParam = objAccesso.GetParameter("@CRSM_Illimitato", 0, , DbType.Byte)
        '        End If
        '        .Parameters.Add(oParam)

        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False

        '    End With
        '    Try
        '        dstable = objAccesso.GetExecuteNotQuery(oRequest)

        '        Me.n_Errore = Errori_Db.None
        '    Catch
        '        Me.Id = -1
        '        Me.n_Errore = Errori_Db.DBInsert
        '        dstable = -1
        '    End Try

        '    Return dstable
        'End Function

        'Public Shared Function AssociaCredito(ByVal pPRSN_ID As Integer, ByVal pDisponibile As Integer, ByVal pIllimitato As Boolean) As Boolean
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim dstable As Integer
        '    Dim objAccesso As New COL_DataAccess
        '    With oRequest
        '        .Command = "sp_Persona_AssociaCredito"
        '        .CommandType = CommandType.StoredProcedure

        '        oParam = objAccesso.GetParameter("@CRSM_PRSN_ID", pPRSN_ID, , DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@CRSM_Disponibili", pDisponibile, , DbType.Int32)
        '        .Parameters.Add(oParam)

        '        If pIllimitato Then
        '            oParam = objAccesso.GetParameter("@CRSM_Illimitato", 1, , DbType.Byte)
        '        Else
        '            oParam = objAccesso.GetParameter("@CRSM_Illimitato", 0, , DbType.Byte)
        '        End If
        '        .Parameters.Add(oParam)

        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False

        '    End With
        '    Try
        '        dstable = objAccesso.GetExecuteNotQuery(oRequest)
        '    Catch
        '        dstable = -1
        '    End Try

        '    Return dstable
        'End Function

        'Public Function AggiornaCreditoWithResiduo(ByVal pDisponibile As Integer, ByVal pIllimitato As Boolean)
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim dstable As Integer
        '    Dim objAccesso As New COL_DataAccess
        '    With oRequest
        '        .Command = "sp_Persona_AggiornaCreditoWithResiduo"
        '        .CommandType = CommandType.StoredProcedure

        '        oParam = objAccesso.GetParameter("@CRSM_PRSN_ID", Me.n_PRSN_id, , DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@CRSM_Disponibili", pDisponibile, , DbType.Int32)
        '        .Parameters.Add(oParam)

        '        If pIllimitato Then
        '            oParam = objAccesso.GetParameter("@CRSM_Illimitato", 1, , DbType.Byte)
        '        Else
        '            oParam = objAccesso.GetParameter("@CRSM_Illimitato", 0, , DbType.Byte)
        '        End If
        '        .Parameters.Add(oParam)

        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False

        '    End With
        '    Try
        '        dstable = objAccesso.GetExecuteNotQuery(oRequest)
        '        ' Recupero l'ID univoco della persona.
        '        Me.n_Errore = Errori_Db.None
        '    Catch
        '        Me.Id = -1
        '        Me.n_Errore = Errori_Db.DBInsert
        '        dstable = -1
        '    End Try

        '    Return dstable
        'End Function

        'Public Shared Function AggiornaCreditoWithResiduo(ByVal pPRSN_ID As Integer, ByVal pDisponibile As Integer, ByVal pIllimitato As Boolean)
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim dstable As Integer
        '    Dim objAccesso As New COL_DataAccess
        '    With oRequest
        '        .Command = "sp_Persona_AggiornaCreditoWithResiduo"
        '        .CommandType = CommandType.StoredProcedure

        '        oParam = objAccesso.GetParameter("@CRSM_PRSN_ID", pPRSN_ID, , DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@CRSM_Disponibili", pDisponibile, , DbType.Int32)
        '        .Parameters.Add(oParam)

        '        If pIllimitato Then
        '            oParam = objAccesso.GetParameter("@CRSM_Illimitato", 1, , DbType.Byte)
        '        Else
        '            oParam = objAccesso.GetParameter("@CRSM_Illimitato", 0, , DbType.Byte)
        '        End If
        '        .Parameters.Add(oParam)

        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False

        '    End With
        '    Try
        '        dstable = objAccesso.GetExecuteNotQuery(oRequest)
        '    Catch
        '        dstable = -1
        '    End Try

        '    Return dstable
        'End Function
#End Region

        Public Function GetTotaleComunitaAssociate() As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim totaleRLPC As Integer
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_persona_GetTotaleComunitaAssociate"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@PRSN_ID", Me.n_PRSN_id, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@totaleRLPC", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                totaleRLPC = oRequest.GetValueFromParameter(2)

            Catch ex As Exception
                n_Errore = Errori_Db.DBReadExist
                totaleRLPC = -1

            End Try
            Return totaleRLPC
        End Function
        Public Function GetTotaleAreeAssociate() As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim totaleARDL As Integer
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_persona_GetTotaleAreeAssociate"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@PRSN_ID", Me.n_PRSN_id, , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@totaleARDL", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                totaleARDL = oRequest.GetValueFromParameter(2)

            Catch ex As Exception
                n_Errore = Errori_Db.DBReadExist
                totaleARDL = -1

            End Try
            Return totaleARDL
        End Function
        Public Function GetPermessiForServizio(ByVal CMNT_ID As Integer, ByVal SRVZ_codice As String) As String
            'elenca i permessi per quella persona in quella comunita per quel servizio
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As String
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Persona_GetPermessiForServizio"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@PRSN_Id", Me.n_PRSN_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", CMNT_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@SRVZ_codice", SRVZ_codice, ParameterDirection.Input, SqlDbType.VarChar, , 15)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@Permessi", "", ParameterDirection.Output, SqlDbType.VarChar, , 32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                iResponse = oRequest.GetValueFromParameter(4)
            Catch ex As Exception
                iResponse = "00000000000000000000000000000000"
            End Try
            Return iResponse
        End Function

        Public Function GetPermessiForServizioForAdmin(ByVal CMNT_ID As Integer, ByVal SRVZ_codice As String, Optional ByVal andCondition As Boolean = False, Optional ByVal Perm1 As MyServices.PermissionType = MyServices.PermissionType.None, Optional ByVal Perm2 As MyServices.PermissionType = MyServices.PermissionType.None, Optional ByVal Perm3 As MyServices.PermissionType = MyServices.PermissionType.None, Optional ByVal Perm4 As MyServices.PermissionType = MyServices.PermissionType.None) As String
            'elenca i permessi per quella persona in quella comunita per quel servizio
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As String
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Persona_GetPermessiForServizio"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@PRSN_Id", Me.n_PRSN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@CMNT_ID", CMNT_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@SRVZ_codice", SRVZ_codice, ParameterDirection.Input, DbType.String, , 15)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Permessi", "", ParameterDirection.Output, DbType.String, , 32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@FiltroRicerca", CType(Main.FiltroRicercaComunitaByIscrizione.forAdmin, Main.FiltroRicercaComunitaByIscrizione), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PosizionePermesso1", CType(Perm1, MyServices.PermissionType), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PosizionePermesso2", CType(Perm2, MyServices.PermissionType), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PosizionePermesso3", CType(Perm3, MyServices.PermissionType), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PosizionePermesso4", CType(Perm4, MyServices.PermissionType), ParameterDirection.Input, DbType.Int32)
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
                objAccesso.GetExecuteNotQuery(oRequest)
                iResponse = oRequest.GetValueFromParameter(4)
            Catch ex As Exception
                iResponse = "00000000000000000000000000000000"
            End Try
            Return iResponse
        End Function
        'Public Function GetComunitaAssociate(ByVal oFiltroUtenti As FiltroUtenti, ByVal oAbilitazione As Main.FiltroAbilitazione, Optional ByVal TPCM_ID As Integer = -1, Optional ByVal oFiltro As FiltroComunita = FiltroComunita.tutti, Optional ByVal valore As String = "", Optional ByVal TPRL_ID As Integer = -1) As DataSet
        '    Dim oDataset As New DataSet
        '    Dim oRequest As New COL_Request
        '    Dim oParam As New COL_Request.Parameter
        '    Dim objAccesso As New COL_DataAccess

        '    With oRequest
        '        .Command = "sp_Persona_GetComunitaAssociate"
        '        .CommandType = CommandType.StoredProcedure
        '        oParam = objAccesso.GetParameter("@TPCM_id", TPCM_ID, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@Anagrafica", CType(oFiltro, FiltroComunita), ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@valore", valore, ParameterDirection.Input, DbType.String, , 100)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@PRSN_ID", Me.n_PRSN_id, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@TPRL_ID", TPRL_ID, ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)
        '        oParam = objAccesso.GetParameter("@FiltroUtente", CType(oFiltroUtenti, FiltroUtenti), ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        oParam = objAccesso.GetParameter("@Abilitazione", CType(oAbilitazione, Main.FiltroAbilitazione), ParameterDirection.Input, DbType.Int32)
        '        .Parameters.Add(oParam)

        '        .Role = COL_Request.UserRole.Admin
        '        .transactional = False
        '    End With
        '    Try
        '        oDataset = objAccesso.GetdataSet(oRequest)
        '        Me.n_Errore = Me.Errore.None
        '    Catch ex As Exception
        '        'gestione errore metodo shared mancante
        '        Me.n_Errore = Errore.DBReadExist
        '    End Try
        '    Return oDataset
        'End Function

        Public Function ElencaComunitaDaDeiscrivere(ByVal CMNT_Id As Integer, ByVal CMNT_Path As String) As DataSet
            'elenca i permessi per quella persona in quella comunita per quel servizio
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataset As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_ElencaComunitaDaDeiscrivere"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@PRSN_Id", Me.n_PRSN_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", CMNT_Id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                If CMNT_Path = "" Then
                    CMNT_Path = "."
                End If
                oParam = objAccesso.GetAdvancedParameter("@CMNT_Path", CMNT_Path, ParameterDirection.Input, SqlDbType.VarChar, , 4000)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                oDataset = objAccesso.GetdataSet(oRequest)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try
            Return oDataset
        End Function
        Public Function DeIscriviFromComunita(ByVal CMNT_Id As Integer, ByVal CMNT_Path As String, ByVal Sottocomunita As Boolean) As Boolean
            'elenca i permessi per quella persona in quella comunita per quel servizio
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Boolean = True
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_DeIscriviFromComunita"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@PRSN_Id", Me.n_PRSN_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", CMNT_Id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                If CMNT_Path = "" Then
                    CMNT_Path = "."
                End If
                oParam = objAccesso.GetAdvancedParameter("@CMNT_Path", CMNT_Path, ParameterDirection.Input, SqlDbType.VarChar, , 8000)
                .Parameters.Add(oParam)

                If Sottocomunita Then
                    oParam = objAccesso.GetAdvancedParameter("@AllSottoComunita", 1, ParameterDirection.Input, SqlDbType.Int)
                Else
                    oParam = objAccesso.GetAdvancedParameter("@AllSottoComunita", 0, ParameterDirection.Input, SqlDbType.Int)
                End If
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBDelete
                iResponse = False
            End Try
            Return iResponse
        End Function

#Region "Cancellazione Utente"

        Public Function DeleteFromCommissioni() As Integer
            'elenca i permessi per quella persona in quella comunita per quel servizio
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Integer = 0
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_DeleteFromCommissioni"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@PRSN_Id", Me.n_PRSN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@totale", 0, ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                iResponse = oRequest.GetValueFromParameter(2)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBDelete
                iResponse = 0
            End Try
            Return iResponse
        End Function
        Public Function DeleteFromTesiAsStudente(ByVal File_Path As String) As Integer
            'Dim oTesi As New COL_Tesi

            'Try
            '    oTesi.CancellaFileForStudente(Me.n_PRSN_id, File_Path)
            'Catch ex As Exception

            'End Try
            ''elenca i permessi per quella persona in quella comunita per quel servizio
            'Dim oRequest As New COL_Request
            'Dim oParam As New COL_Request.Parameter
            'Dim iResponse As Integer = 0
            'Dim objAccesso As New COL_DataAccess


            'With oRequest
            '    .Command = "sp_Persona_DeleteFromTesiAsStudente"
            '    .CommandType = CommandType.StoredProcedure
            '    oParam = objAccesso.GetParameter("@PRSN_Id", Me.n_PRSN_id, ParameterDirection.Input, DbType.Int32)
            '    .Parameters.Add(oParam)

            '    oParam = objAccesso.GetParameter("@totale", 0, ParameterDirection.Output, DbType.Int32)
            '    .Parameters.Add(oParam)

            '    .Role = COL_Request.UserRole.Admin
            '    .transactional = False
            'End With
            'Try
            '    objAccesso.GetExecuteNotQuery(oRequest)
            '    iResponse = oRequest.GetValueFromParameter(2)
            '    Me.n_Errore = Errori_Db.None
            'Catch ex As Exception
            '    Me.n_Errore = Errori_Db.DBDelete
            '    iResponse = 0
            'End Try
            '  Return iResponse
        End Function
        Public Function DeleteFromTesiAsDocente(ByVal File_Path As String) As Integer
            'Dim oDataset As New DataSet
            'Dim oTesi As New COL_Tesi

            'Try
            '    Dim i, totale As Integer
            '    oDataset = oTesi.ElencaByDocente(Me.n_PRSN_id, -1, -1, -1)

            '    totale = oDataset.Tables(0).Rows.Count
            '    For i = 0 To totale - 1
            '        Dim oRow As DataRow
            '        oRow = oDataset.Tables(0).Rows(i)

            '        oTesi.DeleteFiles(oRow.Item("TESI_ID"), File_Path)
            '    Next
            'Catch ex As Exception

            'End Try


            ''elenca i permessi per quella persona in quella comunita per quel servizio
            'Dim oRequest As New COL_Request
            'Dim oParam As New COL_Request.Parameter
            'Dim iResponse As Integer = 0
            'Dim objAccesso As New COL_DataAccess


            'With oRequest
            '    .Command = "sp_Persona_DeleteFromTesiAsDocente"
            '    .CommandType = CommandType.StoredProcedure
            '    oParam = objAccesso.GetParameter("@PRSN_Id", Me.n_PRSN_id, ParameterDirection.Input, DbType.Int32)
            '    .Parameters.Add(oParam)

            '    oParam = objAccesso.GetParameter("@totale", 0, ParameterDirection.Output, DbType.Int32)
            '    .Parameters.Add(oParam)

            '    .Role = COL_Request.UserRole.Admin
            '    .transactional = False
            'End With
            'Try
            '    objAccesso.GetExecuteNotQuery(oRequest)
            '    iResponse = oRequest.GetValueFromParameter(2)
            '    Me.n_Errore = Errori_Db.None
            'Catch ex As Exception
            '    Me.n_Errore = Errori_Db.DBDelete
            '    iResponse = 0
            'End Try
            'Return iResponse
        End Function
        Public Function DeleteFileComunitaAssociati(ByVal File_Path As String) As Integer
            Dim oDataset As New DataSet
            Dim oFile As New COL_File
            Dim i, totale As Integer

            totale = 0
            Try

                Dim FLDS_path, FLDS_nome As String
                oDataset = Me.FileComunitaAssociati(Me.n_PRSN_id)

                totale = oDataset.Tables(0).Rows.Count
                For i = 0 To totale - 1
                    Dim oRow As DataRow
                    oRow = oDataset.Tables(0).Rows(i)

                    FLDS_path = oRow.Item("FLDS_path")
                    FLDS_path = FLDS_path.Replace("/", "\")
                    FLDS_nome = oRow.Item("FLDS_nome")
                    Delete.File(File_Path & FLDS_path & FLDS_nome)
                Next
            Catch ex As Exception

            End Try
            Return totale
        End Function
        Public Function DeleteOnlyFromSubscribedComunity() As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim totale As Integer = 0
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_DeleteOnlyFromSubscribedComunity"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@PRSN_Id", Me.n_PRSN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@totale", 0, ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                totale = oRequest.GetValueFromParameter(2)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBDelete
                totale = 0
            End Try
            Return totale
        End Function


        Public Function DeleteOnlyFromCreatedComunity(ByVal File_Path As String) As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim totale As Integer = 0
            Dim objAccesso As New COL_DataAccess

            Try
                Me.DeleteCartelleFileComunitaCreate(File_Path)
            Catch ex As Exception

            End Try

            With oRequest
                .Command = "sp_Persona_DeleteOnlyFromCreatedComunity"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@PRSN_Id", Me.n_PRSN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@totale", 0, ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                totale = oRequest.GetValueFromParameter(2)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBDelete
                totale = 0
            End Try
            Return totale
        End Function

        Private Function DeleteCartelleFileComunitaCreate(ByVal File_Path As String) As Integer
            Dim oDataset As New DataSet
            Dim oFile As New COL_File
            Dim i, totale As Integer


            totale = 0
            Try
                Dim Path_Comunita As String
                oDataset = Me.ElencoCartelleComunitaCreate(Me.n_PRSN_id)

                totale = oDataset.Tables(0).Rows.Count
                For i = 0 To totale - 1
                    Dim oRow As DataRow
                    oRow = oDataset.Tables(0).Rows(i)

                    Path_Comunita = File_Path & oRow.Item("CMNT_ID") & "\"
                    Path_Comunita = Path_Comunita.Replace("/", "\")
                    Path_Comunita = Path_Comunita.Replace("//", "/")

                    Delete.Directory(Path_Comunita, True)

                Next
            Catch ex As Exception

            End Try
            Return totale
        End Function
        Private Function ElencoCartelleComunitaCreate(ByVal PRSN_ID As Integer) As DataSet
            'elenca i permessi per quella persona in quella comunita per quel servizio
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataset As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_ElencoCartelleComunitaCreate"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@PRSN_Id", PRSN_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                oDataset = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception
            End Try
            Return oDataset
        End Function

        Private Function FileComunitaAssociati(ByVal PRSN_ID As Integer) As DataSet
            'elenca i permessi per quella persona in quella comunita per quel servizio
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataset As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_FileAssociati"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@PRSN_Id", PRSN_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@onlyFile", CType(Main.FiltroFile.soloFile, Main.FiltroFile), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                oDataset = objAccesso.GetdataSet(oRequest)
            Catch ex As Exception
            End Try
            Return oDataset
        End Function
        Private Shared Sub DeleteFileAssociati(ByVal PRSN_ID As Integer)
            'elenca i permessi per quella persona in quella comunita per quel servizio
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As New Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_DeleteFileAssociati"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@PRSN_Id", PRSN_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                iResponse = objAccesso.GetExecuteNotQuery(oRequest)
            Catch ex As Exception
                iResponse = -1
            End Try
            '   Return iResponse
        End Sub
#End Region

        Public Function GetListaComunita_ForMail(ByVal LinguaID As Integer, Optional ByVal oFiltroUtenti As Main.FiltroUtenti = Main.FiltroUtenti.NoPassantiNoCreatori) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataSet As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_GetListaComunita_ForMail"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetParameter("@LinguaID", LinguaID, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_Id", Me.n_PRSN_id, , DbType.Int32)
                .Parameters.Add(oParam)


                oParam = objAccesso.GetParameter("@FiltroUtente", CType(oFiltroUtenti, Main.FiltroUtenti), ParameterDirection.Input, DbType.Int32)
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
        Public Function SetPublicMail_ForComunita(ByVal ElencoIDComunita As String, ByVal Attiva As Boolean) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_SetPublicMail_ForComunita"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@ElencoIDComunita", ElencoIDComunita, ParameterDirection.Input, SqlDbType.VarChar, True, 5000)
                .Parameters.Add(oParam)

                If Attiva Then
                    oParam = objAccesso.GetParameter("@Attiva", 1, , DbType.Int32)
                Else
                    oParam = objAccesso.GetParameter("@Attiva", 0, , DbType.Int32)
                End If
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_Id", Me.n_PRSN_id, , DbType.Int32)
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

        Public Shared Function HasComunitaForServizio(ByVal PRSN_id As Integer, ByVal codiceServ As String, Optional ByVal andCondition As Boolean = False, Optional ByVal Perm1 As MyServices.PermissionType = MyServices.PermissionType.None, Optional ByVal Perm2 As MyServices.PermissionType = MyServices.PermissionType.None, Optional ByVal Perm3 As MyServices.PermissionType = MyServices.PermissionType.None, Optional ByVal Perm4 As MyServices.PermissionType = MyServices.PermissionType.None, Optional ByVal Perm5 As MyServices.PermissionType = MyServices.PermissionType.None) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataset As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_HasComunitaForServizio"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@PRSN_id", PRSN_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@Codice", codiceServ, ParameterDirection.Input, DbType.String, , 15)
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

                oDataset = objAccesso.GetdataSet(oRequest)
                If oDataset.Tables(0).Rows.Count > 0 Then
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                oDataset.Dispose()
                Return False
            End Try
            Return False
        End Function

        Public Function GetPersoneCurriculumByIstituzione(ByVal ISTT_ID As Integer, ByVal pPAGE_SIZE As Integer, ByVal pPAGE_INDEX As Integer, ByVal PRSN_TPPR_id As Integer, ByVal ORGN_ID As Integer, ByVal oFiltroAnagrafica As Main.FiltroAnagrafica, ByVal oFiltroOrdinamento As Main.FiltroOrdinamento, ByVal oCampoOrdinePersona As Main.FiltroCampoOrdinePersona, Optional ByVal oFiltroPersona As Main.FiltroPersona = Main.FiltroPersona.tutte, Optional ByVal valore As String = "", Optional ByVal oVisibilita As Main.FiltroVisibilità = Main.FiltroVisibilità.Pubblici) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_GetPersoneCurriculumByIstituzione"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@ISTT_ID", ISTT_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_TPPR_ID", PRSN_TPPR_id, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PAGE_SIZE", pPAGE_SIZE, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@ANAGRAFICA", CType(oFiltroAnagrafica, Main.FiltroAnagrafica), , DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PAGE_INDEX", pPAGE_INDEX, , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@ORDINE", CType(oCampoOrdinePersona, Main.FiltroCampoOrdinePersona), , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@ORDINAMENTO", CType(oFiltroOrdinamento, Main.FiltroOrdinamento), , DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@ORGN_ID", ORGN_ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@FiltroPersona", CType(oFiltroPersona, Main.FiltroPersona), ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@valore", valore, ParameterDirection.Input, DbType.String, , 300)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@Visibilita", CType(oVisibilita, Main.FiltroVisibilità), ParameterDirection.Input, DbType.Int32)
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
        Public Function GetPersoneCurriculumByComunita(ByVal CMNT_Id As Integer, ByVal pPAGE_SIZE As Integer, ByVal pPAGE_INDEX As Integer, ByVal TPRL_ID As Integer, ByVal oFiltroAnagrafica As Main.FiltroAnagrafica, ByVal oFiltroOrdinamento As Main.FiltroOrdinamento, ByVal oCampoOrdinePersona As Main.FiltroCampoOrdinePersona, Optional ByVal oFiltroPersona As Main.FiltroPersona = Main.FiltroPersona.tutte, Optional ByVal valore As String = "", Optional ByVal oVisibilita As Main.FiltroVisibilità = Main.FiltroVisibilità.Pubblici) As DataSet
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim dsTable As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_GetPersoneCurriculumByComunita"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@CMNT_Id", CMNT_Id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@TPRL_ID", TPRL_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PAGE_SIZE", pPAGE_SIZE, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@ANAGRAFICA", CType(oFiltroAnagrafica, Main.FiltroAnagrafica), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PAGE_INDEX", pPAGE_INDEX, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@ORDINE", CType(oCampoOrdinePersona, Main.FiltroCampoOrdinePersona), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@ORDINAMENTO", CType(oFiltroOrdinamento, Main.FiltroOrdinamento), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)


                oParam = objAccesso.GetAdvancedParameter("@FiltroPersona", CType(oFiltroPersona, Main.FiltroPersona), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@valore", valore, ParameterDirection.Input, SqlDbType.VarChar, , 300)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@Visibilita", CType(oVisibilita, Main.FiltroVisibilità), ParameterDirection.Input, SqlDbType.Int)
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

#Region "Status Comunità"
        Public Function StatusComunitaIscritto(ByVal PRSN_ID As Integer, ByRef totale As Integer, ByRef totaleArchiviate As Integer, ByRef totaleBloccate As Integer, Optional ByVal TipoComunitaId As Integer = -1) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Boolean = False
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_StatusComunitaIscritto"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@PRSN_Id", PRSN_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@totale", 0, ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@totaleArchiviate", 0, ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@totaleBloccate", 0, ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@TPCM_ID", TipoComunitaId, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                If IsNumeric(oRequest.GetValueFromParameter(2)) Then
                    Try
                        totale = oRequest.GetValueFromParameter(2)
                    Catch ex As Exception
                        totale = 0
                    End Try
                End If
                If IsNumeric(oRequest.GetValueFromParameter(3)) Then
                    Try
                        totaleArchiviate = oRequest.GetValueFromParameter(3)
                    Catch ex As Exception
                        totaleArchiviate = 0
                    End Try
                End If
                If IsNumeric(oRequest.GetValueFromParameter(4)) Then
                    Try
                        totaleBloccate = oRequest.GetValueFromParameter(4)
                    Catch ex As Exception
                        totaleBloccate = 0
                    End Try
                End If
                iResponse = True
            Catch ex As Exception

            End Try
            Return iResponse
        End Function
        Public Function StatusComunitaNonIscritto(ByVal PRSN_ID As Integer, ByRef totale As Integer, ByRef totaleArchiviate As Integer, ByRef totaleBloccate As Integer, Optional ByVal TPCM_Id As Integer = -1) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Boolean = False
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_StatusComunitaNonIscritto"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@PRSN_Id", PRSN_ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@totale", 0, ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@totaleArchiviate", 0, ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@totaleBloccate", 0, ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@TPCM_ID", TPCM_Id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                If IsNumeric(oRequest.GetValueFromParameter(2)) Then
                    Try
                        totale = oRequest.GetValueFromParameter(2)
                    Catch ex As Exception
                        totale = 0
                    End Try
                End If
                If IsNumeric(oRequest.GetValueFromParameter(3)) Then
                    Try
                        totaleArchiviate = oRequest.GetValueFromParameter(3)
                    Catch ex As Exception
                        totaleArchiviate = 0
                    End Try
                End If
                If IsNumeric(oRequest.GetValueFromParameter(4)) Then
                    Try
                        totaleBloccate = oRequest.GetValueFromParameter(4)
                    Catch ex As Exception
                        totaleBloccate = 0
                    End Try
                End If
                iResponse = True
            Catch ex As Exception

            End Try
            Return iResponse
        End Function
#End Region

        Public Shared Function GetDefaultRole(ByVal PersonaID As Integer) As Integer
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Integer = -1
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_GetDefaultRole"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@PRSN_ID", PersonaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@TPRL_ID", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)


                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                iResponse = oRequest.GetValueFromParameter(2)
            Catch ex As Exception
                iResponse = -1
            End Try
            Return iResponse
        End Function
        Public Function CanCreateProfile(ByVal PersonaID As Integer) As Boolean
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oServizio As New UCServices.Services_AmministraComunita
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Persona_CanCreateProfile"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@PersonaID", PersonaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@Crea", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@Codice", Services_AmministraComunita.Codex, ParameterDirection.Input, SqlDbType.VarChar, , 15)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PosizionePermesso1", CType(oServizio.Admin, MyServices.PermissionType), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PosizionePermesso2", CType(oServizio.Moderate, MyServices.PermissionType), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PosizionePermesso3", CType(oServizio.CreateComunity, MyServices.PermissionType), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PosizionePermesso4", CType(oServizio.Change, MyServices.PermissionType), ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_Errore = Errori_Db.None
                Try
                    If oRequest.GetValueFromParameter(2) = 1 Then
                        Return True
                    End If
                Catch ex As Exception

                End Try
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try
            Return False

        End Function

#Region "Iscrizione a Comunita"
        Public Function IscrizioneComunitaAsAdmin(ByVal ComunitaID As Integer, ByVal CMNT_Path As String, ByVal PercorsoProfilo As String, ByVal PercorsoIcone As String) As ErroriIscrizioneComunita
            Dim iResponse As ErroriIscrizioneComunita = ErroriIscrizioneComunita.ErroreGenerico
            Dim oComunita As New COL_Comunita
            Dim oRuoloComunita As New COL_RuoloPersonaComunita

            Try
                oRuoloComunita.EstraiByLingua(ComunitaID, Me.n_PRSN_id, Me.Lingua.ID)
                If oRuoloComunita.Errore = Errori_Db.None Then
                    oRuoloComunita.TipoRuolo.Id = CType(Main.TipoRuoloStandard.AdminComunità, Main.TipoRuoloStandard)
                    oRuoloComunita.Modifica()
                    If oRuoloComunita.Errore = Errori_Db.None Then
                        oRuoloComunita.EstraiByLingua(ComunitaID, Me.n_PRSN_id, Me.Lingua.ID)
                        iResponse = Main.ErroriIscrizioneComunita.IscrizioneAvvenuta
                    Else
                        iResponse = Main.ErroriIscrizioneComunita.ErroreGenerico
                    End If
                Else
                    oComunita.Id = ComunitaID

                    Dim PRSN_ID, i, totale, TotaleComunita As Integer
                    Dim ElencoComunita(), CMNT_PathPassanti As String

                    Dim isIscritto As Boolean = False
                    Dim oDataset As DataSet

                    oComunita.Id = ComunitaID
                    PRSN_ID = Me.n_PRSN_id

                    CMNT_PathPassanti = CMNT_Path
                    ElencoComunita = CMNT_Path.Split(".")

                    TotaleComunita = UBound(ElencoComunita) - 1
                    For i = TotaleComunita To 0 Step -1
                        If IsNumeric(ElencoComunita(i)) Then
                            Try
                                oDataset = Me.VerificaIscrizioneAPadri(ElencoComunita(i), PRSN_ID)
                                totale = oDataset.Tables(0).Rows.Count
                            Catch ex As Exception
                                totale = 0
                            End Try

                            If totale > 0 Then
                                isIscritto = True
                                iResponse = Me.IscriviComunitaAsAdmin(ComunitaID, CMNT_Path, PercorsoProfilo, PercorsoIcone)
                                Exit For
                            Else
                                If i > 0 And IsNumeric(ElencoComunita(i - 1)) Then
                                    If ElencoComunita(i - 1) <> ComunitaID Then
                                        oComunita.IscriviPassanteAcomunita(PRSN_ID, ElencoComunita(i - 1))
                                        CMNT_PathPassanti = Left(CMNT_PathPassanti, InStr(CMNT_PathPassanti, "." & ElencoComunita(i - 1) & "."))
                                        CMNT_PathPassanti = CMNT_PathPassanti & ElencoComunita(i - 1) & "."
                                        Me.AggiornaProfiloXML(ElencoComunita(i - 1), CMNT_PathPassanti, PercorsoProfilo)
                                    End If
                                End If
                            End If
                        End If
                    Next
                    If Not isIscritto Then
                        iResponse = Me.IscriviComunitaAsAdmin(ComunitaID, CMNT_Path, PercorsoProfilo, PercorsoIcone)
                    End If
                End If
            Catch ex As Exception

            End Try
            Return iResponse
        End Function
        Public Function IscrizioneComunitaNew(ByVal ComunitaID As Integer, ByVal CMNT_Path As String, ByVal isChiusaForPadre As Boolean, ByVal PercorsoProfilo As String, ByVal PercorsoIcone As String, ByVal LinkAttivazione As String, ByVal SessionID As String, ByVal LinguaCode As String, ByVal oLocalizedMail As MailLocalized, ByVal DayToReloadProfile As Integer) As ErroriIscrizioneComunita
            Dim iResponse As ErroriIscrizioneComunita = ErroriIscrizioneComunita.ErroreGenerico
            Dim oComunita As New COL_Comunita
            Dim PRSN_ID, i, totale, TotaleComunita As Integer
            Dim ElencoComunita(), CMNT_PathPassanti As String

            Dim isIscritto As Boolean = False
            Dim oDataset As DataSet

            oComunita.Id = ComunitaID
            PRSN_ID = Me.n_PRSN_id

            CMNT_PathPassanti = CMNT_Path
            ElencoComunita = CMNT_Path.Split(".")

            TotaleComunita = UBound(ElencoComunita) - 1
            For i = TotaleComunita To 0 Step -1
                If IsNumeric(ElencoComunita(i)) Then
                    Try
                        oDataset = Me.VerificaIscrizioneAPadri(ElencoComunita(i), PRSN_ID)
                        totale = oDataset.Tables(0).Rows.Count
                    Catch ex As Exception
                        totale = 0
                    End Try

                    If totale > 0 Then
                        isIscritto = True
                        iResponse = Me.IscriviComunita(ComunitaID, CMNT_Path, isChiusaForPadre, PercorsoProfilo, PercorsoIcone, LinkAttivazione, SessionID, LinguaCode, oLocalizedMail, DayToReloadProfile)
                        Exit For
                    Else
                        If i > 0 And IsNumeric(ElencoComunita(i - 1)) Then
                            If ElencoComunita(i - 1) <> ComunitaID Then
                                oComunita.IscriviPassanteAcomunita(PRSN_ID, ElencoComunita(i - 1))
                                CMNT_PathPassanti = Left(CMNT_PathPassanti, InStr(CMNT_PathPassanti, "." & ElencoComunita(i - 1) & "."))
                                CMNT_PathPassanti = CMNT_PathPassanti & ElencoComunita(i - 1) & "."
                                Me.AggiornaProfiloXML(ElencoComunita(i - 1), CMNT_PathPassanti, PercorsoProfilo)
                            End If
                        End If
                    End If
                End If
            Next
            If Not isIscritto Then
                iResponse = Me.IscriviComunita(ComunitaID, CMNT_Path, isChiusaForPadre, PercorsoProfilo, PercorsoIcone, LinkAttivazione, SessionID, LinguaCode, oLocalizedMail, DayToReloadProfile)
            End If
            Return iResponse
        End Function
        Public Function IscrizioneComunitaWithRole(ByVal ComunitaID As Integer, ByVal ComunitaPath As String, ByVal RuoloID As Integer, ByVal isResponsabile As Boolean, ByVal PercorsoProfilo As String, ByVal PercorsoIcone As String, ByVal LinguaID As Integer) As ErroriIscrizioneComunita
            Dim iResponse As ErroriIscrizioneComunita = ErroriIscrizioneComunita.ErroreGenerico
            Dim oComunita As New COL_Comunita
            Dim PersonaId, i, totale, TotaleComunita As Integer
            Dim ElencoComunita(), CMNT_PathPassanti As String

            Dim isIscritto As Boolean = False
            Dim oDataset As DataSet

            oComunita.Id = ComunitaID
            PersonaId = Me.n_PRSN_id

            CMNT_PathPassanti = ComunitaPath
            ElencoComunita = ComunitaPath.Split(".")

            TotaleComunita = UBound(ElencoComunita) - 1
            For i = TotaleComunita To 0 Step -1
                If IsNumeric(ElencoComunita(i)) Then
                    Try
                        oDataset = Me.VerificaIscrizioneAPadri(ElencoComunita(i), PersonaId)
                        totale = oDataset.Tables(0).Rows.Count
                    Catch ex As Exception
                        totale = 0
                    End Try

                    If totale > 0 Then
                        isIscritto = True
                        iResponse = Me.IscriviComunitaWithRole(ComunitaID, ComunitaPath, RuoloID, isResponsabile, PercorsoProfilo, PercorsoIcone, LinguaID)
                        Exit For
                    Else
                        If i > 0 And IsNumeric(ElencoComunita(i - 1)) Then
                            If ElencoComunita(i - 1) <> ComunitaID Then
                                oComunita.IscriviPassanteAcomunita(PersonaId, ElencoComunita(i - 1))
                                CMNT_PathPassanti = Left(CMNT_PathPassanti, InStr(CMNT_PathPassanti, "." & ElencoComunita(i - 1) & "."))
                                CMNT_PathPassanti = CMNT_PathPassanti & ElencoComunita(i - 1) & "."
                                Me.AggiornaProfiloXML(ElencoComunita(i - 1), CMNT_PathPassanti, PercorsoProfilo)
                            End If
                        End If
                    End If
                End If
            Next
            If Not isIscritto Then
                iResponse = Me.IscriviComunitaWithRole(ComunitaID, ComunitaPath, RuoloID, isResponsabile, PercorsoProfilo, PercorsoIcone, LinguaID)
            End If
            Return iResponse
        End Function
        Private Function IscriviComunitaWithRole(ByVal ComunitaID As Integer, ByVal ComunitaPercorso As String, ByVal RuoloID As Integer, ByVal isResponsabile As Boolean, ByVal PercorsoProfilo As String, ByVal PercorsoIcone As String, ByVal LinguaID As Integer) As ErroriIscrizioneComunita
            Dim iResponse As ErroriIscrizioneComunita = ErroriIscrizioneComunita.ErroreGenerico
            Dim oComunita As New COL_Comunita
            Dim oTreeComunita As New COL_TreeComunita

            oComunita.Id = ComunitaID
            oComunita.EstraiByLingua(LinguaID)
            oComunita.TipoComunita.Icona = PercorsoIcone & oComunita.TipoComunita.Icona

            Try
                oComunita.IscriviUtente(Me.n_PRSN_id, RuoloID, True, True, isResponsabile)

                If oComunita.Errore = Errori_Db.None Then
                    Me.AggiornaProfiloXML(ComunitaID, ComunitaPercorso, PercorsoProfilo)
                    Return Main.ErroriIscrizioneComunita.IscrizioneAvvenuta
                Else
                    Return Main.ErroriIscrizioneComunita.ErroreGenerico
                End If

            Catch ex As Exception

            End Try
            Return Main.ErroriIscrizioneComunita.ErroreGenerico
        End Function

        Private Function IscriviComunitaAsAdmin(ByVal ComunitaID As Integer, ByVal CMNT_Path As String, ByVal PercorsoProfilo As String, ByVal PercorsoIcone As String) As ErroriIscrizioneComunita
            Dim iResponse As ErroriIscrizioneComunita = ErroriIscrizioneComunita.ErroreGenerico
            Dim oComunita As New COL_Comunita
            Dim oRuolo As New COL_RuoloPersonaComunita
            Dim oTreeComunita As New COL_TreeComunita

            oTreeComunita.Directory = PercorsoProfilo
            oTreeComunita.Nome = Me.n_PRSN_id & ".xml"

            Me.AssociaComunita(Main.TipoRuoloStandard.AdminComunità, ComunitaID, Me.RicezioneSMS, True, True, False)
            If Me.n_Errore = Errori_Db.None Then
                oRuolo.EstraiByLinguaDefault(ComunitaID, Me.n_PRSN_id)
               
                Try
                    oTreeComunita.Insert(oComunita, CMNT_Path, oComunita.GetNomeResponsabile_NomeCreatore, oRuolo)
                Catch ex As Exception

                End Try
                iResponse = Main.ErroriIscrizioneComunita.IscrizioneAvvenuta
            Else
                iResponse = Main.ErroriIscrizioneComunita.ErroreGenerico
            End If
            Return iResponse
        End Function


        Private Function IscriviComunita(ByVal ComunitaID As Integer, ByVal CMNT_Path As String, ByVal isChiusaForPadre As Boolean, ByVal PercorsoProfilo As String, ByVal PercorsoIcone As String, ByVal LinkAttivazione As String, ByVal SessionID As String, ByVal LinguaCode As String, ByVal oLocalizedMail As MailLocalized, ByVal DayToReloadProfile As Integer) As ErroriIscrizioneComunita
            Dim iResponse As ErroriIscrizioneComunita = ErroriIscrizioneComunita.ErroreGenerico
            Dim oComunita As New COL_Comunita
            Dim oRuolo As New COL_RuoloPersonaComunita
            Dim oTreeComunita As New COL_TreeComunita

            Dim TPRL_ID, organizzazioneID As Integer
            Dim Attivato, Abilitato As Boolean
            Dim errore As Boolean = False
            Dim Standard As Boolean = True

            oComunita.Id = ComunitaID

            oComunita.Estrai()
            oComunita.TipoComunita.Icona = PercorsoIcone & oComunita.TipoComunita.Icona

            ' Vedo se sono già iscritto, in caso sistemo il tutto......
            Try

                oRuolo = New COL_RuoloPersonaComunita
                oRuolo.EstraiByLinguaDefault(ComunitaID, Me.n_PRSN_id)

                oTreeComunita.Directory = PercorsoProfilo
                oTreeComunita.Nome = Me.n_PRSN_id & ".xml"

                If oRuolo.Errore = Errori_Db.None Then
                    If oRuolo.TipoRuolo.Id = Main.TipoRuoloStandard.Creatore Then
                        ' Se è il creatore...
                        TPRL_ID = CType(Main.TipoRuoloStandard.AdminComunità, Main.TipoRuoloStandard)
                        oRuolo.TipoRuolo.Id = TPRL_ID
                        oRuolo.Modifica()
                        oRuolo.EstraiByLinguaDefault(ComunitaID, Me.n_PRSN_id)
                        If oRuolo.Errore = Errori_Db.None Then
                            iResponse = ErroriIscrizioneComunita.IscrizioneAvvenuta
                        Else
                            iResponse = ErroriIscrizioneComunita.ErroreGenerico
                        End If
                    ElseIf oRuolo.TipoRuolo.Id = Main.TipoRuoloStandard.Passante Then
                        If isChiusaForPadre = True Then
                            oRuolo.Abilitato = False
                            oRuolo.Attivato = False
                        ElseIf oComunita.IsChiusa Then
                            oRuolo.Abilitato = False
                            oRuolo.Attivato = False
                        ElseIf oComunita.TipoComunita.ID = Main.TipoComunitaStandard.Organizzazione Then

                        Else
                            GetAbilitazioneForOrganization(CMNT_Path, Abilitato, Attivato)
                            oRuolo.Abilitato = Abilitato
                            oRuolo.Attivato = Attivato
                        End If

                        Me.AssociaComunitaRuoloDefault(ComunitaID, Me.RicezioneSMS, oRuolo.Attivato, oRuolo.Abilitato, False)

                        If Me.Errore = Errori_Db.None Then
                            oRuolo.EstraiByLinguaDefault(ComunitaID, Me.n_PRSN_id)
                            If oRuolo.Errore = Errori_Db.None Then
                                If oRuolo.Attivato Then
                                    iResponse = ErroriIscrizioneComunita.IscrizioneAvvenuta
                                Else
                                    iResponse = ErroriIscrizioneComunita.IscrizioneInAttesa
                                    Try
                                        oComunita.MailNotifica(Me, LinkAttivazione, SessionID, oRuolo.TipoRuolo.Nome, LinguaCode, oLocalizedMail)
                                    Catch ex As Exception

                                    End Try
                                End If
                            Else
                                iResponse = ErroriIscrizioneComunita.ErroreGenerico
                            End If
                        Else
                            iResponse = ErroriIscrizioneComunita.ErroreGenerico
                        End If
                    End If
                    Standard = False
                End If
            Catch ex As Exception

            End Try

            If Standard Then
                'RuoloPersonaComunita.Abilitato = 1 'sempre a 1! si viene disabilitati solo per comportamento scorretto
                'confronto l'id dell'organizzazione di appartenenza della comunità con quello dell'organizzazione di default della persona che si sta registrando
                organizzazioneID = Me.GetOrganizzazioneDefault
                If oComunita.Organizzazione.Id = organizzazioneID Then
                    ' se la persona fa parte della stessa organizzazione della comunita la iscrivo col ruolo di default
                    Attivato = Not (oComunita.IsChiusa)
                    Abilitato = Not (oComunita.IsChiusa)
                    If isChiusaForPadre = True Then
                        Attivato = False
                        Abilitato = False
                    End If
                Else 'se uno non è di quell'organizzazione come default lo iscrivo disabilitato e disattivato
                    Attivato = False
                    Abilitato = False
                    If isChiusaForPadre = True Then
                        Attivato = False
                        Abilitato = False
                    ElseIf oComunita.IsChiusa = False Then
                        GetAbilitazioneForOrganization(CMNT_Path, Abilitato, Attivato)
                    End If
                End If

                Me.AssociaComunitaRuoloDefault(ComunitaID, Me.RicezioneSMS, Attivato, Abilitato, False)
                If Me.n_Errore = Errori_Db.None Then
                    Try
                        oRuolo = New COL_RuoloPersonaComunita
                        oRuolo.EstraiByLinguaDefault(ComunitaID, Me.n_PRSN_id)
                        If oRuolo.Errore = Errori_Db.None Then
                            Attivato = oRuolo.Attivato
                            Abilitato = oRuolo.Abilitato

                            oRuolo.EstraiByLinguaDefault(ComunitaID, Me.n_PRSN_id)
                            Try
                                oTreeComunita.Insert(oComunita, CMNT_Path, oComunita.GetNomeResponsabile_NomeCreatore, oRuolo)
                            Catch ex As Exception

                            End Try
                            If Attivato Then
                                iResponse = ErroriIscrizioneComunita.IscrizioneAvvenuta
                            Else
                                iResponse = ErroriIscrizioneComunita.IscrizioneInAttesa
                                Try
                                    oComunita.MailNotifica(Me, LinkAttivazione, SessionID, oRuolo.TipoRuolo.Nome, LinguaCode, oLocalizedMail)
                                Catch ex As Exception

                                End Try
                            End If
                        Else
                                If oComunita.MaxIscritti > 0 Then
                                    iResponse = ErroriIscrizioneComunita.LimiteSuperato
                                Else
                                    iResponse = ErroriIscrizioneComunita.ErroreGenerico
                                End If
                            End If
                    Catch ex As Exception
                        iResponse = ErroriIscrizioneComunita.ErroreGenerico
                    End Try
                Else
                    iResponse = ErroriIscrizioneComunita.ErroreGenerico
                End If
            End If

            PurgeCacheItems(CachePolicy.PermessiServizioUtente, "_" & Me.n_PRSN_id.ToString)
            Return iResponse
        End Function

        Private Sub GetAbilitazioneForOrganization(ByVal CMNT_PAth As String, ByRef abilitato As Boolean, ByRef attivato As Boolean)
            Dim oRuoloComunita As New COL_RuoloPersonaComunita
            Dim ElencoID() As String

            Try
                ElencoID = CMNT_PAth.Split(".")
                oRuoloComunita.EstraiByLinguaDefault(ElencoID(1), Me.n_PRSN_id)
                If oRuoloComunita.Errore = Errori_Db.None Then
                    abilitato = oRuoloComunita.Abilitato
                    attivato = oRuoloComunita.Attivato
                End If
            Catch ex As Exception

            End Try
        End Sub
        Protected Friend Sub AggiornaProfiloXML(ByVal ComunitaID As Integer, ByVal CMNT_PathPassante As String, ByVal PercorsoProfilo As String)
            Dim oTreeComunita As New COL_TreeComunita

            Try
                Dim oComunita As New COL_Comunita
                oComunita.Id = ComunitaID
                oComunita.Estrai()

                Dim oRuoloPersonaComunita As New COL_RuoloPersonaComunita
                oRuoloPersonaComunita.EstraiByLinguaDefault(ComunitaID, Me.n_PRSN_id)
                If oRuoloPersonaComunita.Errore = Errori_Db.None Then
                    oTreeComunita.Directory = PercorsoProfilo
                    oTreeComunita.Nome = Me.n_PRSN_id & ".xml"
                    oTreeComunita.Update(oComunita, CMNT_PathPassante, oComunita.GetNomeResponsabile_NomeCreatore, oRuoloPersonaComunita)
                End If
            Catch ex As Exception

            End Try
        End Sub


        Public Function VerificaIscrizioneAPadri(ByVal ComunitaID As Integer, ByVal PersonaID As Integer) As DataSet
            '- restituisce un dataset contenente le comunità padri di quella passata a cui la persona passata è iscritta
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataSet As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_verificaIscrizioneAPadri"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@CMNT_id", ComunitaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_id", PersonaID, ParameterDirection.Input, SqlDbType.Int)
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

#End Region

#Region "De-iscrizione dalle comunita"

        Public Function NEW_ElencaComunitaDaDeiscrivere(ByVal ComunitaID As Integer, ByVal ComunitaPath As String) As DataSet
            'elenca i permessi per quella persona in quella comunita per quel servizio
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataset As New DataSet
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "NEW_sp_Persona_ElencaComunitaDaDeiscrivere"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@PRSN_Id", Me.n_PRSN_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", ComunitaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                If ComunitaPath = "" Then
                    ComunitaPath = "."
                End If
                oParam = objAccesso.GetAdvancedParameter("@CMNT_Path", ComunitaPath, ParameterDirection.Input, SqlDbType.VarChar, , 8000)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                oDataset = objAccesso.GetdataSet(oRequest)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            End Try
            Return oDataset
        End Function
        Public Function NEW_DeIscriviFromComunita(ByVal ComunitaID As Integer, ByVal ComunitaPath As String, ByVal Sottocomunita As Boolean) As Boolean
            'elenca i permessi per quella persona in quella comunita per quel servizio
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim iResponse As Boolean = True
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "NEW_sp_Persona_DeIscriviFromComunita"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@PRSN_Id", Me.n_PRSN_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", ComunitaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                If ComunitaPath = "" Then
                    ComunitaPath = "."
                End If
                oParam = objAccesso.GetAdvancedParameter("@CMNT_Path", ComunitaPath, ParameterDirection.Input, SqlDbType.VarChar, 8000)
                .Parameters.Add(oParam)

                If Sottocomunita Then
                    oParam = objAccesso.GetAdvancedParameter("@AllSottoComunita", 1, ParameterDirection.Input, SqlDbType.Int)
                Else
                    oParam = objAccesso.GetAdvancedParameter("@AllSottoComunita", 0, ParameterDirection.Input, SqlDbType.Int)
                End If
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBDelete
                iResponse = False
            End Try
            Return iResponse
        End Function




        Public Function GetSubscribedCommunities(ByVal ComunitaID As Integer, ByVal ComunitaPath As String) As GenericCollection(Of PlainComunita)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            Dim oDatareader As IDataReader
            With oRequest
                .Command = "NEW_sp_Persona_ElencaComunitaDaDeiscrivere"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@PRSN_Id", Me.n_PRSN_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@CMNT_ID", ComunitaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                If ComunitaPath = "" Then
                    ComunitaPath = "."
                End If
                oParam = objAccesso.GetAdvancedParameter("@CMNT_Path", ComunitaPath, ParameterDirection.Input, SqlDbType.VarChar, , 8000)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Dim ListaComounita As New GenericCollection(Of PlainComunita)
            Try
                oDatareader = objAccesso.GetdataReader(oRequest)
                While oDatareader.Read
                    Dim oComunita As PlainComunita
                    oComunita = CreaComunitaPlain(oDatareader)

                    If oComunita.REALpath = oComunita.Path Then
                        ListaComounita.Add(oComunita)
                    ElseIf oComunita.IdPadre = 0 Then
                        ListaComounita.Add(oComunita)
                    End If
                End While
                Me.n_Errore = Errori_Db.None
            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError

            Finally
                If oDatareader.IsClosed = False Then
                    oDatareader.Close()
                End If
            End Try
            Return ListaComounita
        End Function

        Public Function LazyOrganizationByCorsi(Optional ByVal abilitato As Boolean = True, Optional ByVal isFacoltà As Boolean = False, Optional ByVal OnlyWithCourse As Boolean = False) As GenericCollection(Of COL_Organizzazione)
            'elenca gli id delle organizzazioni a cui una persona è associata
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataReader As IDataReader
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_OrganizzazioniAssociateByCorsi"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@PRSN_id", Me.n_PRSN_id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                If abilitato Then
                    oParam = objAccesso.GetAdvancedParameter("@abilitato", 1, ParameterDirection.Input, SqlDbType.Int)
                Else
                    oParam = objAccesso.GetAdvancedParameter("@abilitato", 0, ParameterDirection.Input, SqlDbType.Int)
                End If
                .Parameters.Add(oParam)

                If isFacoltà Then
                    oParam = objAccesso.GetAdvancedParameter("@isFacolta", 1, ParameterDirection.Input, SqlDbType.Int)
                Else
                    oParam = objAccesso.GetAdvancedParameter("@isFacolta", 0, ParameterDirection.Input, SqlDbType.Int)
                End If
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Dim ListaComounita As New GenericCollection(Of COL_Organizzazione)

            Try
                oDataReader = objAccesso.GetdataReader(oRequest)
                While oDataReader.Read
                    ListaComounita.Add(New COL_Organizzazione(oDataReader("ORGN_id"), oDataReader("ORGN_ragioneSociale")))
                End While

            Catch ex As Exception
                Me.n_Errore = Errori_Db.DBError
            Finally
                If oDataReader.IsClosed = False Then
                    oDataReader.Close()
                End If
            End Try
            Return ListaComounita
        End Function
        Private Function CreaComunitaPlain(ByVal oDatareader As IDataReader) As PlainComunita
            Dim oComunita As New PlainComunita
            oComunita.AnagraficaCreatore = CStr(oDatareader("AnagraficaCreatore"))
            oComunita.CanUnsubscribe = CBool(oDatareader("CMNT_CanUnsubscribe"))
            oComunita.CreatoreID = CInt(oDatareader("CMNT_PRSN_Id"))
            oComunita.HasFigli = CBool(oDatareader("HasFigli"))
            oComunita.Id = CInt(oDatareader("CMNT_ID"))
            oComunita.IdPadre = CInt(oDatareader("CMNT_idPadre"))
            oComunita.idPadre_Link = CInt(oDatareader("CMNT_idPadre_Link"))
            oComunita.isBloccata = CBool(oDatareader("CMNT_Bloccata"))
            oComunita.IsChiusa = CBool(oDatareader("CMNT_IsChiusa"))
            oComunita.IsComunita = CBool(oDatareader("CMNT_IsComunita"))

            oComunita.isDiretto = CBool(oDatareader("CMNT_isDiretto"))
            oComunita.isIscritto = CBool(oDatareader("CMNT_isIscritto"))
            oComunita.isResponsabile = CBool(oDatareader("RLPC_isResponsabile"))
            oComunita.Livello = CInt(oDatareader("CMNT_Livello"))
            oComunita.lvl = CInt(oDatareader("lvl"))
            oComunita.Nome = CStr(oDatareader("CMNT_nome"))
            oComunita.OrganizzazioneID = CInt(oDatareader("CMNT_ORGN_id"))
            oComunita.Path = CStr(oDatareader("CMNT_path"))
            oComunita.REALpath = CStr(oDatareader("CMNT_REALpath"))
            'oComunita.Responsabile = CStr(oDatareader("CMNT_Responsabile"))
            oComunita.RuoloID = CInt(oDatareader("RLPC_TPRL_id"))
            oComunita.TipoComunita.ID = CInt(oDatareader("CMNT_TPCM_id"))
            oComunita.TipoComunita.Icona = CStr(oDatareader("TPCM_icona"))
            oComunita.TipoComunita.Descrizione = CStr(oDatareader("TPCM_Descrizione"))
            oComunita.SottoComunita = Nothing
            Return oComunita
        End Function



        Private Shared Function RetrieveOrganizzazioniAssociateFromDB(ByVal Persona As COL_Persona) As List(Of IscrizioneComunita)
            Dim oLista As New List(Of IscrizioneComunita)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDatareader As IDataReader
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "sp_Persona_OrganizzazioniAssociatePlain"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@PRSN_id", Persona.ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                oDatareader = objAccesso.GetdataReader(oRequest)

                While oDatareader.Read
                    Dim oOrganizzazione As New COL_Organizzazione(oDatareader("ORGN_id"), oDatareader("ORGN_ragioneSociale"))

                    oLista.Add(New IscrizioneComunita(Persona, GenericValidator.ValBool(oDatareader("RLPC_Responsabile"), False), _
                    GenericValidator.ValBool(oDatareader("RLPC_attivato"), False), GenericValidator.ValBool(oDatareader("RLPC_abilitato"), False), _
                     New PlainOrganizzazione(oDatareader("CMNT_ID"), oDatareader("CMNT_Nome"), oDatareader("ORGN_isFacolta"), oOrganizzazione), Nothing, Nothing, Nothing, Nothing))

                End While
            Catch ex As Exception
            Finally
                If oDatareader.IsClosed = False Then
                    oDatareader.Close()
                End If
            End Try


            Return oLista
        End Function


        Public Function PlainOrganizzazioniAssociate(Optional ByVal OnlyAbilitate As Boolean = False, Optional ByVal OnlyisFacoltà As Boolean = False, Optional ByVal sortExpression As String = "", Optional ByVal sortDirection As String = "") As List(Of IscrizioneComunita)
            Dim oLista As New List(Of IscrizioneComunita)
            Dim cacheKey As String = CachePolicy.OrganizzazioniAssociate(Me.n_PRSN_id)

            If ObjectBase.Cache(cacheKey) Is Nothing Then
                oLista = COL_Persona.RetrieveOrganizzazioniAssociateFromDB(Me)
                ObjectBase.Cache.Insert(cacheKey, oLista, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza60minuti)
            Else
                oLista = CType(ObjectBase.Cache(cacheKey), List(Of IscrizioneComunita))
            End If

            If OnlyisFacoltà And oLista.Count > 0 Then
                oLista = oLista.FindAll(New GenericPredicate(Of IscrizioneComunita, Boolean)(True, AddressOf FindOrganizzazioneByisFacolta))
            End If

            If OnlyAbilitate And oLista.Count > 0 Then
                oLista = oLista.FindAll(New GenericPredicate(Of IscrizioneComunita, Boolean)(True, AddressOf FindOrganizzazioneByisAbilitato))
            End If

            If sortDirection <> String.Empty Then
                sortDirection = sortDirection.ToLower
            End If
            If (Not sortExpression Is Nothing AndAlso sortDirection <> String.Empty) Then
                oLista.Sort(New GenericComparer(Of IscrizioneComunita)(sortExpression))
            End If

            If (Not sortDirection Is Nothing AndAlso sortDirection = "desc") Then
                oLista.Reverse()
            End If

            Return (oLista)
        End Function

        Private Shared Function FindOrganizzazioneByisFacolta(ByVal item As IscrizioneComunita, ByVal argument As Boolean) As Boolean
            Return DirectCast(item.Comunita, PlainOrganizzazione).IsFacolta = argument
        End Function
        Private Shared Function FindOrganizzazioneByisAbilitato(ByVal item As IscrizioneComunita, ByVal argument As Boolean) As Boolean
            Return item.Abilitato = argument
        End Function
#End Region

        Public Function isEqual(ByVal other As COL_Persona) As Boolean Implements System.IEquatable(Of COL_Persona).Equals
            If Me.n_PRSN_id = other.ID Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function GetUtenteAnonimo(ByVal oLingua As Lingua) As COL_Persona
            'carica i campi del db nell'oggetto in base all'id della persona
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            With oRequest
                .Command = "sp_Persona_GetUtenteAnonimo"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetParameter("@PRSN_id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Nome", "", ParameterDirection.Output, DbType.String, False, 40)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Cognome", "", ParameterDirection.Output, DbType.String, False, 40)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_indirizzo", "", ParameterDirection.Output, DbType.String, False, 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_cap", "", ParameterDirection.Output, DbType.String, False, 10)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_citta", "", ParameterDirection.Output, DbType.String, False, 30)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_PRVN_Id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Telefono1", "", ParameterDirection.Output, DbType.String, False, 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Telefono2", "", ParameterDirection.Output, DbType.String, False, 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Cellulare", "", ParameterDirection.Output, DbType.String, False, 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Fax", "", ParameterDirection.Output, DbType.String, False, 25)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Mail", "", ParameterDirection.Output, DbType.String, False, 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_HomePage", "", ParameterDirection.Output, DbType.String, False, 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_FotoPath", "", ParameterDirection.Output, DbType.String, False, 255)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_RicezioneSMS", "", ParameterDirection.Output, DbType.Byte)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_Login", "", ParameterDirection.Output, DbType.String, , 50)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_maxAree", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_AUTN_ID", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_AUTN_RemoteUniqueID", "", ParameterDirection.Output, DbType.String, , 50)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_LNGU_id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_invisible", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_LNGU_Codice", "", ParameterDirection.Output, DbType.String, , 10)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetParameter("@PRSN_TPPR_Id", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_mostraMail", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@TPPR_descrizione", "", ParameterDirection.Output, DbType.String, , 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@LinguaID", oLingua.ID, ParameterDirection.Input, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetParameter("@PRSN_sesso", "", ParameterDirection.Output, DbType.Int32)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PRSN_Ricevimento", "", ParameterDirection.Output, SqlDbType.VarChar, True, 500)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PRSN_MailSecondaria", "", ParameterDirection.Output, SqlDbType.VarChar, True, 255)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                Dim oPersona As New COL_Persona
                objAccesso.GetExecuteNotQuery(oRequest)
                oPersona.ID = oRequest.GetValueFromParameter(1)
                oPersona.Nome = oRequest.GetValueFromParameter(2)
                oPersona.Cognome = oRequest.GetValueFromParameter(3)
                oPersona.Indirizzo = oRequest.GetValueFromParameter(4)
                oPersona.Cap = oRequest.GetValueFromParameter(5)
                oPersona.Citta = oRequest.GetValueFromParameter(6)
                oPersona.Telefono1 = oRequest.GetValueFromParameter(8)
                oPersona.Telefono2 = oRequest.GetValueFromParameter(9)
                oPersona.Cellulare = oRequest.GetValueFromParameter(10)
                oPersona.Fax = oRequest.GetValueFromParameter(11)
                oPersona.Mail = oRequest.GetValueFromParameter(12)
                oPersona.HomePage = oRequest.GetValueFromParameter(13)
                oPersona.FotoPath = oRequest.GetValueFromParameter(14)
                oPersona.RicezioneSMS = oRequest.GetValueFromParameter(15)
                oPersona.Login = oRequest.GetValueFromParameter(16)
                oPersona.maxAree = oRequest.GetValueFromParameter(17)
                oPersona.AUTN_ID = oRequest.GetValueFromParameter(18)
                oPersona.AUTN_RemoteUniqueID = oRequest.GetValueFromParameter(19)
                oPersona.Lingua = oLingua
                oPersona.Bloccata = GenericValidator.ValBool(oRequest.GetValueFromParameter(21), False)
                oPersona.TipoPersona.ID = oRequest.GetValueFromParameter(23)
                oPersona.MostraMail = GenericValidator.ValBool(oRequest.GetValueFromParameter(24), False)
                oPersona.TipoPersona.Descrizione = oRequest.GetValueFromParameter(25)
                oPersona.Sesso = oRequest.GetValueFromParameter(27)
                oPersona.InfoRicevimento = oRequest.GetValueFromParameter(28)
                Return oPersona
            Catch ax As Exception
                Return Nothing
            End Try
            Return Nothing
        End Function

        Public Shared Function LazyVerificaIscrizioneAPadri(ByVal ComunitaID As Integer, ByVal oPersona As COL_Persona) As List(Of IscrizioneComunita)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim reader As IDataReader = Nothing
            Dim objAccesso As New COL_DataAccess
            Dim oLista As New List(Of IscrizioneComunita)
            With oRequest
                .Command = "sp_Persona_LazyIscrizioniPadri"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@ComunitaID", ComunitaID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@PersonaID", oPersona.ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                reader = objAccesso.GetdataReader(oRequest)
                While reader.Read
                    oLista.Add(New IscrizioneComunita(oPersona, reader.Item("RLPC_attivato"), reader.Item("RLPC_abilitato"), _
                    New COL_Comunita(reader.Item("CMNT_id"), reader.Item("CMNT_nome"), reader.Item("CMNT_IdPadre")), reader.Item("RLPC_TPRL_id")))

                End While
            Catch ex As Exception
            Finally
                If Not IsNothing(reader) AndAlso reader.IsClosed = False Then
                    reader.Close()
                End If
            End Try
            Return oLista
        End Function

        Public Function DecryptPassword() As String
            Dim pwdCryptata As New COL_Encrypter

            Try
                Return pwdCryptata.Decrypt(Me.n_PRSN_pwd)
            Catch ex As Exception
                Return ""
            End Try
        End Function
        Public Shared Function DecryptPassword(ByVal Password As String) As String
            Dim pwdCryptata As New COL_Encrypter

            Try
                Return pwdCryptata.Decrypt(Password)
            Catch ex As Exception
                Return ""
            End Try
        End Function

    End Class
End Namespace